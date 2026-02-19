using GameData.Common;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Attack;

namespace GameData.Domains.SpecialEffect.CombatSkill.Baihuagu.Shot;

public class HuaMaiShenZhen : PoisonAddInjury
{
	private const int CostNeiliAllocationUnit = -10;

	private const int SilenceNeiliAllocationUnit = 400;

	public HuaMaiShenZhen()
	{
	}

	public HuaMaiShenZhen(CombatSkillKey skillKey)
		: base(skillKey, 3207)
	{
		RequirePoisonType = 1;
	}

	protected override void OnCastMaxPower(DataContext context)
	{
		CombatCharacter combatCharacter = (base.IsDirect ? base.EnemyChar : base.CombatChar);
		byte b = combatCharacter.GetDefeatMarkCollection().PoisonMarkList[RequirePoisonType];
		if (b > 0)
		{
			for (byte b2 = 0; b2 < 4; b2++)
			{
				base.EnemyChar.ChangeNeiliAllocation(context, b2, b * -10);
			}
			base.EnemyChar.SilenceNeiliAllocationAutoRecover(context, b * 400);
			ShowSpecialEffectTips(1);
		}
	}
}
