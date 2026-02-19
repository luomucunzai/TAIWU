using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Common.Attack;

public class AddInjuryByPoisonMark : CombatSkillEffectBase
{
	private const sbyte AddFlawOrAcupointLevel = 1;

	protected sbyte RequirePoisonType;

	protected bool IsInnerInjury;

	protected virtual bool AlsoAddFlaw => false;

	protected virtual bool AlsoAddAcupoint => false;

	protected AddInjuryByPoisonMark()
	{
	}

	protected AddInjuryByPoisonMark(CombatSkillKey skillKey, int type)
		: base(skillKey, type, -1)
	{
	}

	public override void OnEnable(DataContext context)
	{
		Events.RegisterHandler_CastSkillEnd(OnCastSkillEnd);
	}

	public override void OnDisable(DataContext context)
	{
		Events.UnRegisterHandler_CastSkillEnd(OnCastSkillEnd);
	}

	private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
	{
		if (charId != base.CharacterId || skillId != base.SkillTemplateId)
		{
			return;
		}
		if (PowerMatchAffectRequire(power))
		{
			CombatCharacter combatCharacter = DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly, tryGetCoverCharacter: true);
			byte b = (base.IsDirect ? combatCharacter : base.CombatChar).GetDefeatMarkCollection().PoisonMarkList[RequirePoisonType];
			if (b > 0)
			{
				DomainManager.Combat.AddRandomInjury(context, combatCharacter, IsInnerInjury, b, 1, changeToOld: false, -1);
				if (AlsoAddFlaw)
				{
					DomainManager.Combat.AddFlaw(context, combatCharacter, 1, SkillKey, -1, b);
				}
				if (AlsoAddAcupoint)
				{
					DomainManager.Combat.AddAcupoint(context, combatCharacter, 1, SkillKey, -1, b);
				}
				ShowSpecialEffectTips(0);
				ShowSpecialEffectTips(1);
			}
		}
		RemoveSelf(context);
	}
}
