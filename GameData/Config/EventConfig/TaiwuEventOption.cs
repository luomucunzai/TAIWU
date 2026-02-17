using System;
using System.Collections.Generic;
using GameData.Domains;
using GameData.Domains.TaiwuEvent;
using GameData.Domains.TaiwuEvent.DisplayEvent;
using GameData.Domains.TaiwuEvent.EventOption;

namespace Config.EventConfig
{
	// Token: 0x02000014 RID: 20
	public class TaiwuEventOption
	{
		// Token: 0x17000014 RID: 20
		// (get) Token: 0x06000068 RID: 104 RVA: 0x00050136 File Offset: 0x0004E336
		// (set) Token: 0x06000069 RID: 105 RVA: 0x0005013E File Offset: 0x0004E33E
		public string OptionContent { get; private set; }

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x0600006A RID: 106 RVA: 0x00050147 File Offset: 0x0004E347
		// (set) Token: 0x0600006B RID: 107 RVA: 0x00050159 File Offset: 0x0004E359
		public bool WasSelected
		{
			get
			{
				return DomainManager.TaiwuEvent.WasTemporaryOptionSelected(this.OptionGuid);
			}
			set
			{
				DomainManager.TaiwuEvent.SetTemporaryOptionSelected(this.OptionGuid, value);
			}
		}

		// Token: 0x0600006C RID: 108 RVA: 0x0005016D File Offset: 0x0004E36D
		public void SetContent(string content)
		{
			this.OptionContent = content;
		}

		// Token: 0x0600006D RID: 109 RVA: 0x00050178 File Offset: 0x0004E378
		public string Select(EventScriptRuntime scriptRuntime)
		{
			bool flag = this.DefaultState == 1;
			if (flag)
			{
				this.WasSelected = true;
			}
			string nextEvent = this.TryExecuteScript(scriptRuntime);
			Func<string> onOptionSelect = this.OnOptionSelect;
			string ret = (onOptionSelect != null) ? onOptionSelect() : null;
			return string.IsNullOrEmpty(ret) ? nextEvent : ret;
		}

		// Token: 0x0600006E RID: 110 RVA: 0x000501C8 File Offset: 0x0004E3C8
		public string TryExecuteScript(EventScriptRuntime scriptRuntime)
		{
			bool flag = this.Script == null;
			string result;
			if (flag)
			{
				result = string.Empty;
			}
			else
			{
				scriptRuntime.ExecuteScript(this.Script, this.ArgBox);
				result = scriptRuntime.Current.NextEvent;
			}
			return result;
		}

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x0600006F RID: 111 RVA: 0x00050210 File Offset: 0x0004E410
		public bool IsVisible
		{
			get
			{
				bool flag = this.OnOptionVisibleCheck != null && !this.OnOptionVisibleCheck();
				bool result;
				if (flag)
				{
					result = false;
				}
				else
				{
					EventScriptRuntime runtime = DomainManager.TaiwuEvent.ScriptRuntime;
					bool flag2 = !runtime.CheckConditionList(this.VisibleConditions, this.ArgBox);
					if (flag2)
					{
						result = false;
					}
					else
					{
						bool flag3 = this.OneTimeOnly && this.WasSelected;
						result = !flag3;
					}
				}
				return result;
			}
		}

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x06000070 RID: 112 RVA: 0x00050287 File Offset: 0x0004E487
		public bool IsAvailable
		{
			get
			{
				return this.CheckAvailableConditionsFromCode() && this.CheckAvailableConditionsFromScript() && this.CheckAvailableConditionsFromCodeConfig();
			}
		}

		// Token: 0x06000071 RID: 113 RVA: 0x000502A2 File Offset: 0x0004E4A2
		public bool CheckAvailableConditionsFromCode()
		{
			return this.OnOptionAvailableCheck == null || this.OnOptionAvailableCheck();
		}

		// Token: 0x06000072 RID: 114 RVA: 0x000502BA File Offset: 0x0004E4BA
		public bool CheckAvailableConditionsFromScript()
		{
			return DomainManager.TaiwuEvent.ScriptRuntime.CheckConditionList(this.AvailableConditions, this.ArgBox);
		}

		// Token: 0x06000073 RID: 115 RVA: 0x000502D8 File Offset: 0x0004E4D8
		public bool CheckAvailableConditionsFromCodeConfig()
		{
			bool flag = this.OptionAvailableConditions == null;
			bool result;
			if (flag)
			{
				result = true;
			}
			else
			{
				for (int i = 0; i < this.OptionAvailableConditions.Count; i++)
				{
					TaiwuEventOptionConditionBase condition = this.OptionAvailableConditions[i];
					bool flag2 = condition.OrConditionCore != null && condition.OrConditionCore.Count > 0;
					if (flag2)
					{
						bool anyTrue = false;
						for (int j = 0; j < condition.OrConditionCore.Count; j++)
						{
							OptionAvailableInfoMinimumElement element = default(OptionAvailableInfoMinimumElement);
							OptionConditionModifier.ModifyCondition(ref element, condition.OrConditionCore[i], this.ArgBox);
							bool pass = element.Pass;
							if (pass)
							{
								anyTrue = true;
								break;
							}
						}
						bool flag3 = !anyTrue;
						if (flag3)
						{
							return false;
						}
					}
					else
					{
						OptionAvailableInfoMinimumElement element2 = default(OptionAvailableInfoMinimumElement);
						OptionConditionModifier.ModifyCondition(ref element2, condition, this.ArgBox);
						bool flag4 = !element2.Pass;
						if (flag4)
						{
							return false;
						}
					}
				}
				result = true;
			}
			return result;
		}

		// Token: 0x0400005D RID: 93
		public EventArgBox ArgBox;

		// Token: 0x0400005E RID: 94
		public sbyte Behavior = 0;

		// Token: 0x0400005F RID: 95
		public Func<string> GetReplacedContent;

		// Token: 0x04000060 RID: 96
		public Func<bool> OnOptionAvailableCheck;

		// Token: 0x04000061 RID: 97
		public Func<string> OnOptionSelect;

		// Token: 0x04000062 RID: 98
		public Func<bool> OnOptionVisibleCheck;

		// Token: 0x04000063 RID: 99
		public Func<List<string>> GetExtraFormatLanguageKeys;

		// Token: 0x04000064 RID: 100
		public List<TaiwuEventOptionConditionBase> OptionAvailableConditions;

		// Token: 0x04000065 RID: 101
		public List<OptionConsumeInfo> OptionConsumeInfos;

		// Token: 0x04000067 RID: 103
		public string OptionKey;

		// Token: 0x04000068 RID: 104
		public string OptionGuid;

		// Token: 0x04000069 RID: 105
		public sbyte DefaultState = 0;

		// Token: 0x0400006A RID: 106
		public EventScript Script;

		// Token: 0x0400006B RID: 107
		public EventConditionList VisibleConditions;

		// Token: 0x0400006C RID: 108
		public EventConditionList AvailableConditions;

		// Token: 0x0400006D RID: 109
		public bool OneTimeOnly;
	}
}
