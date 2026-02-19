using GameData.Common;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Agile;

namespace GameData.Domains.SpecialEffect.CombatSkill.Wuxianjiao.Agile;

public class DunDiBaiZuXian : CheckHitEffect
{
	private const sbyte ReducePercent = 30;

	public DunDiBaiZuXian()
	{
	}

	public DunDiBaiZuXian(CombatSkillKey skillKey)
		: base(skillKey, 12605)
	{
		CheckHitType = 2;
	}

	protected override bool HitEffect(DataContext context)
	{
		CombatCharacter currEnemyChar = base.CurrEnemyChar;
		if ((base.IsDirect ? currEnemyChar.GetAffectingDefendSkillId() : currEnemyChar.GetAffectingMoveSkillId()) < 0)
		{
			return false;
		}
		int num = (base.IsDirect ? currEnemyChar.GetDefendSkillTimePercent() : (currEnemyChar.GetMobilityValue() * 100 / MoveSpecialConstants.MaxMobility));
		if (num > 30)
		{
			if (base.IsDirect)
			{
				currEnemyChar.DefendSkillLeftFrame = (short)(currEnemyChar.DefendSkillLeftFrame - currEnemyChar.DefendSkillTotalFrame * 30 / 100);
				currEnemyChar.SetDefendSkillTimePercent((byte)(currEnemyChar.DefendSkillLeftFrame * 100 / currEnemyChar.DefendSkillTotalFrame), context);
			}
			else
			{
				ChangeMobilityValue(context, currEnemyChar, -MoveSpecialConstants.MaxMobility * 30 / 100);
			}
		}
		else if (base.IsDirect)
		{
			DomainManager.Combat.ClearAffectingDefenseSkill(context, currEnemyChar);
		}
		else
		{
			ClearAffectingAgileSkill(context, currEnemyChar);
		}
		return true;
	}
}
