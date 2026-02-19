using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Attack;

namespace GameData.Domains.SpecialEffect.CombatSkill.Ranshanpai.Sword;

public class TaiShiJianQi : BuffByNeiliAllocation
{
	private const int PerHitSilenceFrame = 90;

	protected override bool ShowTipsOnAffecting => false;

	public TaiShiJianQi()
	{
	}

	public TaiShiJianQi(CombatSkillKey skillKey)
		: base(skillKey, 7207)
	{
		RequireNeiliAllocationType = 3;
	}

	public override void OnEnable(DataContext context)
	{
		base.OnEnable(context);
		Events.RegisterHandler_AttackSkillAttackHit(OnAttackSkillAttackHit);
	}

	public override void OnDisable(DataContext context)
	{
		Events.UnRegisterHandler_AttackSkillAttackHit(OnAttackSkillAttackHit);
		base.OnDisable(context);
	}

	private void OnAttackSkillAttackHit(DataContext context, CombatCharacter attacker, CombatCharacter defender, short skillId, int index, bool critical)
	{
		if (!SkillKey.IsMatch(attacker.GetId(), skillId) || !base.Affecting)
		{
			return;
		}
		int num = ((index < 3) ? base.SkillInstance.GetHitDistribution()[base.CombatChar.SkillHitType[index]] : base.CombatChar.GetAttackSkillPower());
		if (num > 0)
		{
			short randomBanableSkillId = defender.GetRandomBanableSkillId(context.Random, null, 4);
			if (randomBanableSkillId >= 0)
			{
				DomainManager.Combat.SilenceSkill(context, defender, randomBanableSkillId, num * 90 / 10);
				ShowSpecialEffectTips(0);
			}
		}
	}
}
