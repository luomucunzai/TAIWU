using Config;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Wudangpai.FistAndPalm;

public class WuDangChunYangQuan : CombatSkillEffectBase
{
	private const sbyte PrepareProgressPercent = 50;

	public WuDangChunYangQuan()
	{
	}

	public WuDangChunYangQuan(CombatSkillKey skillKey)
		: base(skillKey, 4106, -1)
	{
	}

	public override void OnEnable(DataContext context)
	{
		Events.RegisterHandler_PrepareSkillBegin(OnPrepareSkillBegin);
		Events.RegisterHandler_AttackSkillAttackEnd(OnAttackSkillAttackEnd);
	}

	public override void OnDisable(DataContext context)
	{
		Events.UnRegisterHandler_PrepareSkillBegin(OnPrepareSkillBegin);
		Events.UnRegisterHandler_AttackSkillAttackEnd(OnAttackSkillAttackEnd);
	}

	private void OnPrepareSkillBegin(DataContext context, int charId, bool isAlly, short skillId)
	{
		if (charId == base.CharacterId && skillId == base.SkillTemplateId && GetCanInterruptSKill() >= 0)
		{
			DomainManager.Combat.ChangeSkillPrepareProgress(base.CombatChar, base.CombatChar.SkillPrepareTotalProgress * 50 / 100);
		}
	}

	private void OnAttackSkillAttackEnd(CombatContext context, sbyte hitType, bool hit, int index)
	{
		if (context.SkillKey != SkillKey || index != 3)
		{
			return;
		}
		short canInterruptSKill = GetCanInterruptSKill();
		if (CombatCharPowerMatchAffectRequire() && canInterruptSKill >= 0)
		{
			if (DomainManager.Combat.InterruptSkill(context, base.CurrEnemyChar))
			{
				base.CurrEnemyChar.SetAnimationToPlayOnce(DomainManager.Combat.GetHittedAni(base.CurrEnemyChar, 2), context);
				DomainManager.Combat.SetProperLoopAniAndParticle(context, base.CurrEnemyChar);
			}
			ShowSpecialEffectTips(0);
		}
	}

	private short GetCanInterruptSKill()
	{
		short preparingSkillId = base.CurrEnemyChar.GetPreparingSkillId();
		if (preparingSkillId < 0 || Config.CombatSkill.Instance[preparingSkillId].EquipType != 1)
		{
			return -1;
		}
		sbyte currInnerRatio = DomainManager.CombatSkill.GetElement_CombatSkills(new CombatSkillKey(base.CurrEnemyChar.GetId(), preparingSkillId)).GetCurrInnerRatio();
		return (short)((base.IsDirect ? (currInnerRatio > 50) : (currInnerRatio < 50)) ? preparingSkillId : (-1));
	}

	public static int CalcInterruptOdds(CombatSkillKey selfSkill, bool isDirect, CombatSkillKey enemySkill)
	{
		sbyte currInnerRatio = DomainManager.CombatSkill.GetElement_CombatSkills(enemySkill).GetCurrInnerRatio();
		return (short)((isDirect ? (currInnerRatio > 50) : (currInnerRatio < 50)) ? 100 : (-1));
	}
}
