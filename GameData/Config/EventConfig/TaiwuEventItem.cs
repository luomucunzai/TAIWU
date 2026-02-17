using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using GameData.Domains;
using GameData.Domains.TaiwuEvent;
using GameData.Domains.TaiwuEvent.Enum;
using GameData.Domains.TaiwuEvent.EventOption;
using GameData.Utilities;

namespace Config.EventConfig
{
	// Token: 0x02000013 RID: 19
	public abstract class TaiwuEventItem
	{
		// Token: 0x17000012 RID: 18
		// (get) Token: 0x0600005A RID: 90 RVA: 0x0004FC48 File Offset: 0x0004DE48
		// (set) Token: 0x0600005B RID: 91 RVA: 0x0004FC50 File Offset: 0x0004DE50
		public string EventContent { get; private set; }

		// Token: 0x17000013 RID: 19
		public TaiwuEventOption this[string key]
		{
			get
			{
				TaiwuEventOption option = Array.Find<TaiwuEventOption>(this.EventOptions, (TaiwuEventOption cell) => cell.OptionKey == key);
				bool flag = option == null;
				if (flag)
				{
					bool flag2 = this.TaiwuEvent != null && this.TaiwuEvent.ExtendEventOptions != null;
					if (flag2)
					{
						for (int i = 0; i < this.TaiwuEvent.ExtendEventOptions.Count; i++)
						{
							ValueTuple<string, string> tuple = this.TaiwuEvent.ExtendEventOptions[i];
							bool flag3 = tuple.Item2 == key;
							if (flag3)
							{
								TaiwuEvent srcEvent = DomainManager.TaiwuEvent.GetEvent(tuple.Item1);
								srcEvent.ArgBox = this.ArgBox;
								option = srcEvent.EventConfig[tuple.Item2];
								bool flag4 = option != null;
								if (flag4)
								{
									break;
								}
							}
						}
					}
				}
				return option;
			}
		}

		// Token: 0x0600005D RID: 93 RVA: 0x0004FD60 File Offset: 0x0004DF60
		public bool CheckCondition()
		{
			EventScriptRuntime runtime = DomainManager.TaiwuEvent.ScriptRuntime;
			bool flag = !runtime.CheckConditionList(this.Conditions, this.ArgBox);
			return !flag && this.OnCheckEventCondition();
		}

		// Token: 0x0600005E RID: 94
		public abstract bool OnCheckEventCondition();

		// Token: 0x0600005F RID: 95
		public abstract void OnEventEnter();

		// Token: 0x06000060 RID: 96
		public abstract void OnEventExit();

		// Token: 0x06000061 RID: 97
		public abstract string GetReplacedContentString();

		// Token: 0x06000062 RID: 98 RVA: 0x0004FDA0 File Offset: 0x0004DFA0
		public virtual List<string> GetExtraFormatLanguageKeys()
		{
			return null;
		}

		// Token: 0x06000063 RID: 99 RVA: 0x0004FDB4 File Offset: 0x0004DFB4
		public void SetLanguage(string[] languageArray)
		{
			this.EventContent = languageArray[0].Replace("<NL>", "\n");
			for (int i = 1; i < languageArray.Length; i++)
			{
				string language = languageArray[i];
				bool flag = string.IsNullOrEmpty(language);
				if (!flag)
				{
					TaiwuEventOption option = this.EventOptions[i - 1];
					bool flag2 = option == null;
					if (flag2)
					{
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(51, 3);
						defaultInterpolatedStringHandler.AppendLiteral("Unable to set language for null option: ");
						defaultInterpolatedStringHandler.AppendFormatted(this.TaiwuEvent.EventGuid);
						defaultInterpolatedStringHandler.AppendLiteral(" option ");
						defaultInterpolatedStringHandler.AppendFormatted<int>(i - 1);
						defaultInterpolatedStringHandler.AppendLiteral(" \n");
						defaultInterpolatedStringHandler.AppendFormatted(language);
						defaultInterpolatedStringHandler.AppendLiteral(".");
						AdaptableLog.Warning(defaultInterpolatedStringHandler.ToStringAndClear(), false);
					}
					else
					{
						option.SetContent(language);
						this.EventOptions[i - 1].SetContent(languageArray[i].Replace("<NL>", "\n"));
					}
				}
			}
		}

		// Token: 0x06000064 RID: 100 RVA: 0x0004FEC0 File Offset: 0x0004E0C0
		public void SetOptionConsume(string optionKeyOrGuid, sbyte consumeType, int consumeCount)
		{
			bool flag = this.EventOptions != null;
			if (flag)
			{
				foreach (TaiwuEventOption opt in this.EventOptions)
				{
					bool flag2 = (optionKeyOrGuid == opt.OptionKey || optionKeyOrGuid == opt.OptionGuid) && opt.OptionConsumeInfos != null;
					if (flag2)
					{
						int i = opt.OptionConsumeInfos.Count;
						while (i-- > 0)
						{
							bool flag3 = opt.OptionConsumeInfos[i].ConsumeType == consumeType;
							if (flag3)
							{
								OptionConsumeInfo item = opt.OptionConsumeInfos[i];
								item.ConsumeCount = consumeCount;
								opt.OptionConsumeInfos[i] = item;
								break;
							}
						}
						break;
					}
				}
			}
		}

		// Token: 0x06000065 RID: 101 RVA: 0x0004FF9C File Offset: 0x0004E19C
		[Obsolete]
		public void SetOptionRead(string optionKey)
		{
			bool flag = this.ArgBox == null;
			if (flag)
			{
				throw new Exception("can not set option read when event is not showing!");
			}
			TaiwuEventOption option = this[optionKey];
			bool flag2 = option == null;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			if (flag2)
			{
				defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(28, 2);
				defaultInterpolatedStringHandler.AppendFormatted(optionKey);
				defaultInterpolatedStringHandler.AppendLiteral(" is not an option of event ");
				defaultInterpolatedStringHandler.AppendFormatted<Guid>(this.Guid);
				defaultInterpolatedStringHandler.AppendLiteral("!");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(1, 2);
			defaultInterpolatedStringHandler.AppendFormatted<Guid>(this.Guid);
			defaultInterpolatedStringHandler.AppendLiteral("_");
			defaultInterpolatedStringHandler.AppendFormatted(optionKey);
			string optionArgBoxKey = defaultInterpolatedStringHandler.ToStringAndClear();
			this.ArgBox.Set(optionArgBoxKey, true);
		}

		// Token: 0x06000066 RID: 102 RVA: 0x00050060 File Offset: 0x0004E260
		[Obsolete]
		public bool GetOptionRead(string optionKey)
		{
			bool flag = this.ArgBox == null;
			if (flag)
			{
				throw new Exception("can not get option read when event is not showing!");
			}
			TaiwuEventOption option = this[optionKey];
			bool flag2 = option == null;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			if (flag2)
			{
				defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(28, 2);
				defaultInterpolatedStringHandler.AppendFormatted(optionKey);
				defaultInterpolatedStringHandler.AppendLiteral(" is not an option of event ");
				defaultInterpolatedStringHandler.AppendFormatted<Guid>(this.Guid);
				defaultInterpolatedStringHandler.AppendLiteral("!");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(1, 2);
			defaultInterpolatedStringHandler.AppendFormatted<Guid>(this.Guid);
			defaultInterpolatedStringHandler.AppendLiteral("_");
			defaultInterpolatedStringHandler.AppendFormatted(optionKey);
			string optionArgBoxKey = defaultInterpolatedStringHandler.ToStringAndClear();
			bool readState = false;
			this.ArgBox.Get(optionArgBoxKey, ref readState);
			return readState;
		}

		// Token: 0x04000046 RID: 70
		public TaiwuEvent TaiwuEvent;

		// Token: 0x04000047 RID: 71
		public EventArgBox ArgBox;

		// Token: 0x04000048 RID: 72
		public string EventAudio;

		// Token: 0x04000049 RID: 73
		public string EventBackground;

		// Token: 0x0400004B RID: 75
		public string EventGroup;

		// Token: 0x0400004C RID: 76
		public TaiwuEventOption[] EventOptions;

		// Token: 0x0400004D RID: 77
		public short EventSortingOrder;

		// Token: 0x0400004E RID: 78
		public EEventType EventType;

		// Token: 0x0400004F RID: 79
		public bool ForceSingle;

		// Token: 0x04000050 RID: 80
		public Guid Guid;

		// Token: 0x04000051 RID: 81
		public bool IsHeadEvent;

		// Token: 0x04000052 RID: 82
		public string MainRoleKey;

		// Token: 0x04000053 RID: 83
		public sbyte MaskControl;

		// Token: 0x04000054 RID: 84
		public float MaskTweenTime;

		// Token: 0x04000055 RID: 85
		public EventPackage Package;

		// Token: 0x04000056 RID: 86
		public string TargetRoleKey;

		// Token: 0x04000057 RID: 87
		public short TriggerType;

		// Token: 0x04000058 RID: 88
		public bool IgnoreShowingEvent;

		// Token: 0x04000059 RID: 89
		public EventScript Script;

		// Token: 0x0400005A RID: 90
		public EventConditionList Conditions;

		// Token: 0x0400005B RID: 91
		public Dictionary<short, EventBoolStateInfo> BoolStateDict;

		// Token: 0x0400005C RID: 92
		public string EscOptionKey;
	}
}
