using System;
using GameData.Domains.Character;

namespace GameData.Domains.TaiwuEvent.EventOption;

public class OptionConditionCharacter : TaiwuEventOptionConditionBase
{
	public readonly Func<GameData.Domains.Character.Character, bool> ConditionChecker;

	public OptionConditionCharacter(short id, Func<GameData.Domains.Character.Character, bool> checker)
		: base(id)
	{
		ConditionChecker = checker;
	}

	public override bool CheckCondition(EventArgBox box)
	{
		if (box == null)
		{
			return false;
		}
		return ConditionChecker(box.GetCharacter("CharacterId"));
	}

	public override (short, string[]) GetDisplayData(EventArgBox box)
	{
		int num = box.GetInt("CharacterId");
		(string, string) monasticTitleOrDisplayName = DomainManager.Character.GetNameRelatedData(num).GetMonasticTitleOrDisplayName(num == DomainManager.Taiwu.GetTaiwuCharId());
		return (Id, new string[1] { monasticTitleOrDisplayName.Item1 + monasticTitleOrDisplayName.Item2 });
	}
}
