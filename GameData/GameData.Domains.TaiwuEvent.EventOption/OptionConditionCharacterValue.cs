using System;
using GameData.Domains.Character;

namespace GameData.Domains.TaiwuEvent.EventOption;

public class OptionConditionCharacterValue : TaiwuEventOptionConditionBase
{
	public readonly Func<GameData.Domains.Character.Character, int, bool> ConditionChecker;

	public int Value;

	public OptionConditionCharacterValue(short id, int value, Func<GameData.Domains.Character.Character, int, bool> checker)
		: base(id)
	{
		ConditionChecker = checker;
		Value = value;
	}

	public override bool CheckCondition(EventArgBox box)
	{
		if (box == null)
		{
			return false;
		}
		return ConditionChecker(box.GetCharacter("CharacterId"), Value);
	}

	public override (short, string[]) GetDisplayData(EventArgBox box)
	{
		int num = box.GetInt("CharacterId");
		(string, string) displayName = DomainManager.Character.GetNameRelatedData(num).GetDisplayName(num == DomainManager.Taiwu.GetTaiwuCharId());
		return (Id, new string[2]
		{
			displayName.Item1 + displayName.Item2,
			Value.ToString()
		});
	}
}
