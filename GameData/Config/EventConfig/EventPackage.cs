using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using GameData.Domains.TaiwuEvent.Enum;

namespace Config.EventConfig
{
	// Token: 0x02000010 RID: 16
	public abstract class EventPackage
	{
		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000034 RID: 52 RVA: 0x0004F174 File Offset: 0x0004D374
		// (set) Token: 0x06000035 RID: 53 RVA: 0x0004F17C File Offset: 0x0004D37C
		public string NameSpace { get; protected set; }

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x06000036 RID: 54 RVA: 0x0004F185 File Offset: 0x0004D385
		// (set) Token: 0x06000037 RID: 55 RVA: 0x0004F18D File Offset: 0x0004D38D
		public string Author { get; protected set; }

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x06000038 RID: 56 RVA: 0x0004F196 File Offset: 0x0004D396
		// (set) Token: 0x06000039 RID: 57 RVA: 0x0004F19E File Offset: 0x0004D39E
		public string Group { get; protected set; }

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x0600003A RID: 58 RVA: 0x0004F1A7 File Offset: 0x0004D3A7
		// (set) Token: 0x0600003B RID: 59 RVA: 0x0004F1AF File Offset: 0x0004D3AF
		public string ModIdString { get; protected set; }

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x0600003C RID: 60 RVA: 0x0004F1B8 File Offset: 0x0004D3B8
		public string Key
		{
			get
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(2, 3);
				defaultInterpolatedStringHandler.AppendFormatted(this.NameSpace);
				defaultInterpolatedStringHandler.AppendLiteral("_");
				defaultInterpolatedStringHandler.AppendFormatted(this.Author);
				defaultInterpolatedStringHandler.AppendLiteral("_");
				defaultInterpolatedStringHandler.AppendFormatted(this.Group);
				return defaultInterpolatedStringHandler.ToStringAndClear();
			}
		}

		// Token: 0x0600003D RID: 61 RVA: 0x0004F21C File Offset: 0x0004D41C
		public List<TaiwuEventItem> GetEventsByType(EEventType eventType)
		{
			List<TaiwuEventItem> list = new List<TaiwuEventItem>();
			for (int i = 0; i < this.EventList.Count; i++)
			{
				this.EventList[i].Package = this;
				bool flag = this.EventList[i].EventType == eventType;
				if (flag)
				{
					list.Add(this.EventList[i]);
				}
			}
			return list;
		}

		// Token: 0x0600003E RID: 62 RVA: 0x0004F290 File Offset: 0x0004D490
		public List<TaiwuEventItem> GetAllEvents()
		{
			return this.EventList;
		}

		// Token: 0x0600003F RID: 63 RVA: 0x0004F2A8 File Offset: 0x0004D4A8
		public void SetModIdString(string modId)
		{
			this.ModIdString = modId;
		}

		// Token: 0x06000040 RID: 64 RVA: 0x0004F2B4 File Offset: 0x0004D4B4
		public void InitLanguage(string languageFilePath)
		{
			bool flag = !File.Exists(languageFilePath);
			if (!flag)
			{
				Dictionary<string, List<string>> contentMap = new Dictionary<string, List<string>>();
				string guidLineStart = "- EventGuid : ";
				string contentLineStart = "-- EventContent :";
				string optionLineStart = "-- Option_";
				string[] lines = File.ReadAllLines(languageFilePath);
				string eventGuid = string.Empty;
				List<string> list = null;
				bool contentHandlingFlag = false;
				for (int i = 0; i < lines.Length; i++)
				{
					string line = lines[i].Trim();
					bool flag2 = line.StartsWith(guidLineStart);
					if (flag2)
					{
						bool flag3 = list != null && !string.IsNullOrEmpty(eventGuid);
						if (flag3)
						{
							contentMap.Add(eventGuid, list);
						}
						eventGuid = line.Replace(guidLineStart, string.Empty);
						list = new List<string>();
					}
					else
					{
						bool flag4 = string.IsNullOrEmpty(eventGuid) || list == null;
						if (!flag4)
						{
							bool flag5 = !line.StartsWith("-") && list.Count > 0;
							if (flag5)
							{
								string lineData = line.Trim();
								bool flag6 = contentHandlingFlag && string.IsNullOrEmpty(lineData);
								if (flag6)
								{
									lineData = "\n";
								}
								List<string> list2 = list;
								int index = list2.Count - 1;
								list2[index] += lineData;
							}
							else
							{
								bool flag7 = line.StartsWith(contentLineStart);
								if (flag7)
								{
									contentHandlingFlag = true;
									list.Add(line.Replace(contentLineStart, string.Empty).Trim());
								}
								else
								{
									bool flag8 = line.StartsWith(optionLineStart);
									if (flag8)
									{
										contentHandlingFlag = false;
										list.Add(line.Substring(line.IndexOf(":", StringComparison.Ordinal) + 1));
									}
								}
							}
						}
					}
				}
				bool flag9 = !string.IsNullOrEmpty(eventGuid) && list != null;
				if (flag9)
				{
					contentMap.Add(eventGuid, list);
				}
				foreach (TaiwuEventItem eventItem in this.EventList)
				{
					List<string> contentList;
					bool flag10 = contentMap.TryGetValue(eventItem.Guid.ToString(), out contentList) && contentList.Count > 0;
					if (flag10)
					{
						eventItem.SetLanguage(contentList.ToArray());
						eventItem.Package = this;
					}
				}
			}
		}

		// Token: 0x04000018 RID: 24
		protected List<TaiwuEventItem> EventList;
	}
}
