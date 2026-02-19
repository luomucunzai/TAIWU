using System;
using System.Collections.Generic;
using System.IO;
using GameData.Domains.TaiwuEvent.Enum;

namespace Config.EventConfig;

public abstract class EventPackage
{
	protected List<TaiwuEventItem> EventList;

	public string NameSpace { get; protected set; }

	public string Author { get; protected set; }

	public string Group { get; protected set; }

	public string ModIdString { get; protected set; }

	public string Key => $"{NameSpace}_{Author}_{Group}";

	public List<TaiwuEventItem> GetEventsByType(EEventType eventType)
	{
		List<TaiwuEventItem> list = new List<TaiwuEventItem>();
		for (int i = 0; i < EventList.Count; i++)
		{
			EventList[i].Package = this;
			if (EventList[i].EventType == eventType)
			{
				list.Add(EventList[i]);
			}
		}
		return list;
	}

	public List<TaiwuEventItem> GetAllEvents()
	{
		return EventList;
	}

	public void SetModIdString(string modId)
	{
		ModIdString = modId;
	}

	public void InitLanguage(string languageFilePath)
	{
		if (!File.Exists(languageFilePath))
		{
			return;
		}
		Dictionary<string, List<string>> dictionary = new Dictionary<string, List<string>>();
		string text = "- EventGuid : ";
		string text2 = "-- EventContent :";
		string value = "-- Option_";
		string[] array = File.ReadAllLines(languageFilePath);
		string text3 = string.Empty;
		List<string> list = null;
		bool flag = false;
		for (int i = 0; i < array.Length; i++)
		{
			string text4 = array[i].Trim();
			if (text4.StartsWith(text))
			{
				if (list != null && !string.IsNullOrEmpty(text3))
				{
					dictionary.Add(text3, list);
				}
				text3 = text4.Replace(text, string.Empty);
				list = new List<string>();
			}
			else
			{
				if (string.IsNullOrEmpty(text3) || list == null)
				{
					continue;
				}
				if (!text4.StartsWith("-") && list.Count > 0)
				{
					string text5 = text4.Trim();
					if (flag && string.IsNullOrEmpty(text5))
					{
						text5 = "\n";
					}
					List<string> list2 = list;
					list2[list2.Count - 1] += text5;
				}
				else if (text4.StartsWith(text2))
				{
					flag = true;
					list.Add(text4.Replace(text2, string.Empty).Trim());
				}
				else if (text4.StartsWith(value))
				{
					flag = false;
					list.Add(text4.Substring(text4.IndexOf(":", StringComparison.Ordinal) + 1));
				}
			}
		}
		if (!string.IsNullOrEmpty(text3) && list != null)
		{
			dictionary.Add(text3, list);
		}
		foreach (TaiwuEventItem @event in EventList)
		{
			if (dictionary.TryGetValue(@event.Guid.ToString(), out var value2) && value2.Count > 0)
			{
				@event.SetLanguage(value2.ToArray());
				@event.Package = this;
			}
		}
	}
}
