using System;
using GameData.Domains.TaiwuEvent.EventHelper;
using GameData.Utilities;

namespace GameData.Domains.TaiwuEvent.EventOption;

public class OptionConditionSecretInformationArgBoxKey : TaiwuEventOptionConditionBase
{
	public readonly string Key;

	public readonly string CharIdKey;

	public readonly sbyte TaiwuNameFormatIndex;

	public readonly Func<int, string, bool> ConditionChecker;

	public OptionConditionSecretInformationArgBoxKey(short id, string boxKey, string charIdKey, sbyte taiwuNameFormatIndex, Func<int, string, bool> checkFunc)
		: base(id)
	{
		Key = boxKey;
		CharIdKey = charIdKey;
		TaiwuNameFormatIndex = taiwuNameFormatIndex;
		ConditionChecker = checkFunc;
	}

	public override bool CheckCondition(EventArgBox box)
	{
		int arg = -1;
		if (box.Get(Key, ref arg))
		{
			return ConditionChecker(arg, CharIdKey);
		}
		return false;
	}

	public override (short, string[]) GetDisplayData(EventArgBox box)
	{
		int arg = -1;
		if (box.Get(Key, ref arg))
		{
			EventArgBox secretInformationParameters = GameData.Domains.TaiwuEvent.EventHelper.EventHelper.GetSecretInformationParameters(arg);
			int arg2 = -1;
			if (secretInformationParameters.Get(CharIdKey, ref arg2))
			{
				int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
				bool isTaiwu = arg2 == taiwuCharId;
				(string, string) displayName = DomainManager.Character.GetNameRelatedData(arg2).GetDisplayName(isTaiwu);
				string text = displayName.Item1 + displayName.Item2;
				if (-1 == TaiwuNameFormatIndex)
				{
					return (Id, new string[1] { text });
				}
				string[] array = new string[2] { text, text };
				if (array.CheckIndex(TaiwuNameFormatIndex))
				{
					(string, string) displayName2 = DomainManager.Character.GetNameRelatedData(taiwuCharId).GetDisplayName(isTaiwu: true);
					array[TaiwuNameFormatIndex] = displayName2.Item1 + displayName2.Item2;
				}
				return (Id, array);
			}
		}
		return (Id, new string[1] { "role decode error" });
	}
}
