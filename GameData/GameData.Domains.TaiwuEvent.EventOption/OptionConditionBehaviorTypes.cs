using System;
using System.Text;
using Config;
using GameData.Domains.Character;

namespace GameData.Domains.TaiwuEvent.EventOption;

public class OptionConditionBehaviorTypes : TaiwuEventOptionConditionBase
{
	public readonly sbyte[] BehaviorRange;

	public readonly Func<sbyte, sbyte, sbyte, sbyte, sbyte, bool> ConditionChecker;

	public OptionConditionBehaviorTypes(short id, sbyte b1, sbyte b2, sbyte b3, sbyte b4, sbyte b5, Func<sbyte, sbyte, sbyte, sbyte, sbyte, bool> checkFunc)
		: base(id)
	{
		BehaviorRange = new sbyte[5] { b1, b2, b3, b4, b5 };
		ConditionChecker = checkFunc;
	}

	public override bool CheckCondition(EventArgBox box)
	{
		GameData.Domains.Character.Character character = box.GetCharacter("CharacterId");
		if (character != null)
		{
			return ConditionChecker(BehaviorRange[0], BehaviorRange[1], BehaviorRange[2], BehaviorRange[3], BehaviorRange[4]);
		}
		return false;
	}

	public override (short, string[]) GetDisplayData(EventArgBox box)
	{
		StringBuilder stringBuilder = new StringBuilder();
		int num = 0;
		sbyte[] behaviorRange = BehaviorRange;
		foreach (sbyte b in behaviorRange)
		{
			if (b >= 0)
			{
				if (num != 0)
				{
					stringBuilder.Append('、');
				}
				stringBuilder.Append(Config.BehaviorType.Instance[b].Name);
				num++;
			}
		}
		return (Id, new string[1] { stringBuilder.ToString() });
	}
}
