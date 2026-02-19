using Config;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Agile;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.Xuannvpai.Agile;

public class QingNvLvBing : BuffHitOrDebuffAvoid
{
	private const int ReverseSilenceFrame = 2400;

	protected override sbyte AffectHitType => 2;

	public QingNvLvBing()
	{
	}

	public QingNvLvBing(CombatSkillKey skillKey)
		: base(skillKey, 8406)
	{
		ListenCanAffectChange = true;
	}

	public override void OnEnable(DataContext context)
	{
		base.OnEnable(context);
		Events.RegisterHandler_CastSkillEnd(OnCastSkillEnd);
	}

	public override void OnDisable(DataContext context)
	{
		Events.UnRegisterHandler_CastSkillEnd(OnCastSkillEnd);
		base.OnDisable(context);
	}

	private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
	{
		if (interrupted || (base.IsDirect ? (isAlly != base.CombatChar.IsAlly) : (isAlly == base.CombatChar.IsAlly)) || !base.CanAffect || !DomainManager.Combat.TryGetElement_CombatCharacterDict(charId, out var element) || element.GetAutoCastingSkill() || Config.CombatSkill.Instance[skillId].EquipType != 1 || !DomainManager.CombatSkill.TryGetElement_CombatSkills(new CombatSkillKey(charId, skillId), out var element2))
		{
			return;
		}
		int num = element2.GetHitDistribution()[AffectHitType];
		int num2 = num / 2;
		if (num2 > 0 && context.Random.CheckPercentProb(num2))
		{
			if (base.IsDirect && DomainManager.Combat.CanCastSkill(base.CombatChar, skillId, costFree: true))
			{
				DomainManager.Combat.CastSkillFree(context, element, skillId, ECombatCastFreePriority.QingNvLvBing);
			}
			else if (!base.IsDirect)
			{
				DomainManager.Combat.SilenceSkill(context, element, skillId, 2400);
			}
			ShowSpecialEffectTips(1);
		}
	}
}
