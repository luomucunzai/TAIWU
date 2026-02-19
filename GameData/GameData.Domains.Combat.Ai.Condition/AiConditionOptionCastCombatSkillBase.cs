using System.Linq;
using Config;

namespace GameData.Domains.Combat.Ai.Condition;

public abstract class AiConditionOptionCastCombatSkillBase : AiConditionCombatBase
{
	protected abstract short SkillId { get; }

	protected AiMemoryNew Memory { get; private set; }

	protected CombatCharacter CombatChar { get; private set; }

	private bool IsValid(short skillId)
	{
		return skillId == SkillId;
	}

	public override bool Check(AiMemoryNew memory, CombatCharacter combatChar)
	{
		Memory = memory;
		CombatChar = combatChar;
		bool result = GetResult(combatChar);
		Memory = null;
		CombatChar = null;
		return result;
	}

	private bool GetResult(CombatCharacter combatChar)
	{
		CombatSkillItem combatSkillItem = Config.CombatSkill.Instance[SkillId];
		if (combatSkillItem == null)
		{
			return false;
		}
		sbyte equipType = combatSkillItem.EquipType;
		if (1 == 0)
		{
		}
		int num = equipType switch
		{
			1 => 0, 
			2 => 1, 
			3 => 2, 
			_ => -1, 
		};
		if (1 == 0)
		{
		}
		int num2 = num;
		if (num2 < 0)
		{
			return false;
		}
		if (combatChar.IsAlly && !DomainManager.Combat.AiOptions.AutoCastSkill[num2])
		{
			return false;
		}
		return combatChar.GetCombatSkillIds().Where(IsValid).Where(combatChar.AiCanCast)
			.Any();
	}
}
