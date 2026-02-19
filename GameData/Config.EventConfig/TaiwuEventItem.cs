using System;
using System.Collections.Generic;
using GameData.Domains;
using GameData.Domains.TaiwuEvent;
using GameData.Domains.TaiwuEvent.Enum;
using GameData.Domains.TaiwuEvent.EventOption;
using GameData.Utilities;

namespace Config.EventConfig;

public abstract class TaiwuEventItem
{
	public TaiwuEvent TaiwuEvent;

	public EventArgBox ArgBox;

	public string EventAudio;

	public string EventBackground;

	public string EventGroup;

	public TaiwuEventOption[] EventOptions;

	public short EventSortingOrder;

	public EEventType EventType;

	public bool ForceSingle;

	public Guid Guid;

	public bool IsHeadEvent;

	public string MainRoleKey;

	public sbyte MaskControl;

	public float MaskTweenTime;

	public EventPackage Package;

	public string TargetRoleKey;

	public short TriggerType;

	public bool IgnoreShowingEvent;

	public EventScript Script;

	public EventConditionList Conditions;

	public Dictionary<short, EventBoolStateInfo> BoolStateDict;

	public string EscOptionKey;

	public string EventContent { get; private set; }

	public TaiwuEventOption this[string key]
	{
		get
		{
			TaiwuEventOption taiwuEventOption = Array.Find(EventOptions, (TaiwuEventOption cell) => cell.OptionKey == key);
			if (taiwuEventOption == null && TaiwuEvent != null && TaiwuEvent.ExtendEventOptions != null)
			{
				for (int num = 0; num < TaiwuEvent.ExtendEventOptions.Count; num++)
				{
					(string, string) tuple = TaiwuEvent.ExtendEventOptions[num];
					if (tuple.Item2 == key)
					{
						TaiwuEvent taiwuEvent = DomainManager.TaiwuEvent.GetEvent(tuple.Item1);
						taiwuEvent.ArgBox = ArgBox;
						taiwuEventOption = taiwuEvent.EventConfig[tuple.Item2];
						if (taiwuEventOption != null)
						{
							break;
						}
					}
				}
			}
			return taiwuEventOption;
		}
	}

	public bool CheckCondition()
	{
		EventScriptRuntime scriptRuntime = DomainManager.TaiwuEvent.ScriptRuntime;
		if (!scriptRuntime.CheckConditionList(Conditions, ArgBox))
		{
			return false;
		}
		return OnCheckEventCondition();
	}

	public abstract bool OnCheckEventCondition();

	public abstract void OnEventEnter();

	public abstract void OnEventExit();

	public abstract string GetReplacedContentString();

	public virtual List<string> GetExtraFormatLanguageKeys()
	{
		return null;
	}

	public void SetLanguage(string[] languageArray)
	{
		EventContent = languageArray[0].Replace("<NL>", "\n");
		for (int i = 1; i < languageArray.Length; i++)
		{
			string text = languageArray[i];
			if (!string.IsNullOrEmpty(text))
			{
				TaiwuEventOption taiwuEventOption = EventOptions[i - 1];
				if (taiwuEventOption == null)
				{
					AdaptableLog.Warning($"Unable to set language for null option: {TaiwuEvent.EventGuid} option {i - 1} \n{text}.");
				}
				else
				{
					taiwuEventOption.SetContent(text);
					EventOptions[i - 1].SetContent(languageArray[i].Replace("<NL>", "\n"));
				}
			}
		}
	}

	public void SetOptionConsume(string optionKeyOrGuid, sbyte consumeType, int consumeCount)
	{
		if (EventOptions == null)
		{
			return;
		}
		TaiwuEventOption[] eventOptions = EventOptions;
		foreach (TaiwuEventOption taiwuEventOption in eventOptions)
		{
			if ((!(optionKeyOrGuid == taiwuEventOption.OptionKey) && !(optionKeyOrGuid == taiwuEventOption.OptionGuid)) || taiwuEventOption.OptionConsumeInfos == null)
			{
				continue;
			}
			int count = taiwuEventOption.OptionConsumeInfos.Count;
			while (count-- > 0)
			{
				if (taiwuEventOption.OptionConsumeInfos[count].ConsumeType == consumeType)
				{
					OptionConsumeInfo value = taiwuEventOption.OptionConsumeInfos[count];
					value.ConsumeCount = consumeCount;
					taiwuEventOption.OptionConsumeInfos[count] = value;
					break;
				}
			}
			break;
		}
	}

	[Obsolete]
	public void SetOptionRead(string optionKey)
	{
		if (ArgBox == null)
		{
			throw new Exception("can not set option read when event is not showing!");
		}
		TaiwuEventOption taiwuEventOption = this[optionKey];
		if (taiwuEventOption == null)
		{
			throw new Exception($"{optionKey} is not an option of event {Guid}!");
		}
		string key = $"{Guid}_{optionKey}";
		ArgBox.Set(key, arg: true);
	}

	[Obsolete]
	public bool GetOptionRead(string optionKey)
	{
		if (ArgBox == null)
		{
			throw new Exception("can not get option read when event is not showing!");
		}
		TaiwuEventOption taiwuEventOption = this[optionKey];
		if (taiwuEventOption == null)
		{
			throw new Exception($"{optionKey} is not an option of event {Guid}!");
		}
		string key = $"{Guid}_{optionKey}";
		bool arg = false;
		ArgBox.Get(key, ref arg);
		return arg;
	}
}
