using System;
using Config;
using GameData.Domains.Character;

namespace GameData.Domains.TaiwuEvent.EventOption;

public class OptionConditionCharacterProfession : TaiwuEventOptionConditionBase
{
	public readonly Func<GameData.Domains.Character.Character, int, bool> ConditionChecker;

	public int ProfessionId;

	public OptionConditionCharacterProfession(short id, int professionId, Func<GameData.Domains.Character.Character, int, bool> checker)
		: base(id)
	{
		ConditionChecker = checker;
		ProfessionId = professionId;
	}

	public override bool CheckCondition(EventArgBox box)
	{
		if (box == null)
		{
			return false;
		}
		return ConditionChecker(box.GetCharacter("CharacterId"), ProfessionId);
	}

	public override (short, string[]) GetDisplayData(EventArgBox box)
	{
		int num = box.GetInt("CharacterId");
		(string, string) displayName = DomainManager.Character.GetNameRelatedData(num).GetDisplayName(num == DomainManager.Taiwu.GetTaiwuCharId());
		return (Id, new string[2]
		{
			displayName.Item1 + displayName.Item2,
			Profession.Instance[ProfessionId].Name
		});
	}
}
