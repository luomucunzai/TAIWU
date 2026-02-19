using System;
using System.Collections.Generic;
using GameData.Domains;
using GameData.Domains.TaiwuEvent;
using GameData.Domains.TaiwuEvent.DisplayEvent;
using GameData.Domains.TaiwuEvent.EventOption;

namespace Config.EventConfig;

public class TaiwuEventOption
{
	public EventArgBox ArgBox;

	public sbyte Behavior = 0;

	public Func<string> GetReplacedContent;

	public Func<bool> OnOptionAvailableCheck;

	public Func<string> OnOptionSelect;

	public Func<bool> OnOptionVisibleCheck;

	public Func<List<string>> GetExtraFormatLanguageKeys;

	public List<TaiwuEventOptionConditionBase> OptionAvailableConditions;

	public List<OptionConsumeInfo> OptionConsumeInfos;

	public string OptionKey;

	public string OptionGuid;

	public sbyte DefaultState = 0;

	public EventScript Script;

	public EventConditionList VisibleConditions;

	public EventConditionList AvailableConditions;

	public bool OneTimeOnly;

	public string OptionContent { get; private set; }

	public bool WasSelected
	{
		get
		{
			return DomainManager.TaiwuEvent.WasTemporaryOptionSelected(OptionGuid);
		}
		set
		{
			DomainManager.TaiwuEvent.SetTemporaryOptionSelected(OptionGuid, value);
		}
	}

	public bool IsVisible
	{
		get
		{
			if (OnOptionVisibleCheck != null && !OnOptionVisibleCheck())
			{
				return false;
			}
			EventScriptRuntime scriptRuntime = DomainManager.TaiwuEvent.ScriptRuntime;
			if (!scriptRuntime.CheckConditionList(VisibleConditions, ArgBox))
			{
				return false;
			}
			if (OneTimeOnly && WasSelected)
			{
				return false;
			}
			return true;
		}
	}

	public bool IsAvailable => CheckAvailableConditionsFromCode() && CheckAvailableConditionsFromScript() && CheckAvailableConditionsFromCodeConfig();

	public void SetContent(string content)
	{
		OptionContent = content;
	}

	public string Select(EventScriptRuntime scriptRuntime)
	{
		if (DefaultState == 1)
		{
			WasSelected = true;
		}
		string text = TryExecuteScript(scriptRuntime);
		string text2 = OnOptionSelect?.Invoke();
		return string.IsNullOrEmpty(text2) ? text : text2;
	}

	public string TryExecuteScript(EventScriptRuntime scriptRuntime)
	{
		if (Script == null)
		{
			return string.Empty;
		}
		scriptRuntime.ExecuteScript(Script, ArgBox);
		return scriptRuntime.Current.NextEvent;
	}

	public bool CheckAvailableConditionsFromCode()
	{
		return OnOptionAvailableCheck == null || OnOptionAvailableCheck();
	}

	public bool CheckAvailableConditionsFromScript()
	{
		return DomainManager.TaiwuEvent.ScriptRuntime.CheckConditionList(AvailableConditions, ArgBox);
	}

	public bool CheckAvailableConditionsFromCodeConfig()
	{
		if (OptionAvailableConditions == null)
		{
			return true;
		}
		for (int i = 0; i < OptionAvailableConditions.Count; i++)
		{
			TaiwuEventOptionConditionBase taiwuEventOptionConditionBase = OptionAvailableConditions[i];
			if (taiwuEventOptionConditionBase.OrConditionCore != null && taiwuEventOptionConditionBase.OrConditionCore.Count > 0)
			{
				bool flag = false;
				for (int j = 0; j < taiwuEventOptionConditionBase.OrConditionCore.Count; j++)
				{
					OptionAvailableInfoMinimumElement element = default(OptionAvailableInfoMinimumElement);
					OptionConditionModifier.ModifyCondition(ref element, taiwuEventOptionConditionBase.OrConditionCore[i], ArgBox);
					if (element.Pass)
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					return false;
				}
			}
			else
			{
				OptionAvailableInfoMinimumElement element2 = default(OptionAvailableInfoMinimumElement);
				OptionConditionModifier.ModifyCondition(ref element2, taiwuEventOptionConditionBase, ArgBox);
				if (!element2.Pass)
				{
					return false;
				}
			}
		}
		return true;
	}
}
