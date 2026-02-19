using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Attack.RanChenZi;

public class SanJianFuXieTie : CombatSkillEffectBase
{
	public SanJianFuXieTie()
	{
	}

	public SanJianFuXieTie(CombatSkillKey skillKey)
		: base(skillKey, 17132, -1)
	{
	}

	public override void OnEnable(DataContext context)
	{
		sbyte juniorXiangshuTaskStatus = DomainManager.World.GetElement_XiangshuAvatarTaskStatuses(1).JuniorXiangshuTaskStatus;
		if (juniorXiangshuTaskStatus > 4)
		{
			bool flag = juniorXiangshuTaskStatus == 6;
			if (flag)
			{
				DomainManager.Combat.RemoveAllFlaw(context, base.CurrEnemyChar);
			}
			else
			{
				DomainManager.Combat.AddFlaw(context, base.CurrEnemyChar, 3, SkillKey, -1);
			}
			ShowSpecialEffectTips(flag, 1, 2);
		}
		Events.RegisterHandler_CastSkillEnd(OnCastSkillEnd);
	}

	public override void OnDisable(DataContext context)
	{
		Events.UnRegisterHandler_CastSkillEnd(OnCastSkillEnd);
	}

	private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
	{
		if (charId == base.CharacterId && skillId == base.SkillTemplateId)
		{
			if (PowerMatchAffectRequire(power))
			{
				CombatCharacter currEnemyChar = base.CurrEnemyChar;
				ChangeMobilityValue(context, currEnemyChar, -currEnemyChar.GetMobilityValue());
				ClearAffectingAgileSkill(context, currEnemyChar);
				ChangeBreathValue(context, currEnemyChar, -30000);
				ChangeStanceValue(context, currEnemyChar, -4000);
				ShowSpecialEffectTips(0);
			}
			RemoveSelf(context);
		}
	}
}
