using System;
using GameData.Domains.Character;

namespace GameData.Domains.TaiwuEvent.EventOption;

public class OptionConditionCharacterCombatDefeatMark : TaiwuEventOptionConditionBase
{
	public readonly Func<GameData.Domains.Character.Character, sbyte, bool> ConditionChecker;

	public sbyte CombatType;

	public OptionConditionCharacterCombatDefeatMark(short id, sbyte combatType, Func<GameData.Domains.Character.Character, sbyte, bool> checker)
		: base(id)
	{
		ConditionChecker = checker;
		CombatType = combatType;
	}

	public override bool CheckCondition(EventArgBox box)
	{
		if (box == null)
		{
			return false;
		}
		return ConditionChecker(box.GetCharacter("CharacterId"), CombatType);
	}

	public override (short, string[]) GetDisplayData(EventArgBox box)
	{
		int num = box.GetInt("CharacterId");
		(string, string) displayName = DomainManager.Character.GetNameRelatedData(num).GetDisplayName(num == DomainManager.Taiwu.GetTaiwuCharId());
		byte b = GlobalConfig.NeedDefeatMarkCount[CombatType];
		return (Id, new string[2]
		{
			displayName.Item1 + displayName.Item2,
			b.ToString()
		});
	}
}
