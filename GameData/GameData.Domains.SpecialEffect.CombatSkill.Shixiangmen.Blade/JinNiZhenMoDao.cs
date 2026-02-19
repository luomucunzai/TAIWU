using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Shixiangmen.Blade;

public class JinNiZhenMoDao : CombatSkillEffectBase
{
	public JinNiZhenMoDao()
	{
	}

	public JinNiZhenMoDao(CombatSkillKey skillKey)
		: base(skillKey, 6206, -1)
	{
	}

	public override void OnEnable(DataContext context)
	{
		Events.RegisterHandler_PrepareSkillBegin(OnPrepareSkillBegin);
		Events.RegisterHandler_CastSkillEnd(OnCastSkillEnd);
		Events.RegisterHandler_SkillEffectChange(OnSkillEffectChange);
	}

	public override void OnDisable(DataContext context)
	{
		SetDirectionCanCast(context, base.CombatChar, !base.IsDirect, canCast: true);
		Events.UnRegisterHandler_PrepareSkillBegin(OnPrepareSkillBegin);
		Events.UnRegisterHandler_CastSkillEnd(OnCastSkillEnd);
		Events.UnRegisterHandler_SkillEffectChange(OnSkillEffectChange);
	}

	private void OnPrepareSkillBegin(DataContext context, int charId, bool isAlly, short skillId)
	{
		if (charId != base.CharacterId)
		{
			return;
		}
		sbyte b = (sbyte)(base.IsDirect ? 1 : 0);
		if (!IsSrcSkillPerformed)
		{
			if (skillId == base.SkillTemplateId)
			{
				CombatCharacter combatCharacter = DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly);
				CombatCharacter combatCharacter2 = DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly, tryGetCoverCharacter: true);
				if (combatCharacter.GetPreparingSkillId() >= 0 && DomainManager.CombatSkill.GetSkillDirection(combatCharacter.GetId(), combatCharacter.GetPreparingSkillId()) == b && DomainManager.Combat.InterruptSkill(context, combatCharacter))
				{
					combatCharacter.SetAnimationToPlayOnce(DomainManager.Combat.GetHittedAni(combatCharacter, 2), context);
					DomainManager.Combat.SetProperLoopAniAndParticle(context, combatCharacter);
				}
				ClearAffectingSkill(context, combatCharacter, b);
				if (combatCharacter2 != combatCharacter)
				{
					ClearAffectingSkill(context, combatCharacter2, b);
				}
				SetDirectionCanCast(context, !isAlly, !base.IsDirect, canCast: false);
				ShowSpecialEffectTips(0);
			}
		}
		else if ((int)DomainManager.CombatSkill.GetSkillDirection(charId, skillId) == ((!base.IsDirect) ? 1 : 0))
		{
			ReduceEffectCount();
		}
	}

	private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
	{
		if (charId == base.CharacterId && skillId == base.SkillTemplateId)
		{
			if (!IsSrcSkillPerformed)
			{
				IsSrcSkillPerformed = true;
				SetDirectionCanCast(context, !isAlly, !base.IsDirect, canCast: true);
				SetDirectionCanCast(context, base.CombatChar, !base.IsDirect, canCast: false);
				AddMaxEffectCount();
			}
			else
			{
				RemoveSelf(context);
			}
		}
	}

	private void OnSkillEffectChange(DataContext context, int charId, SkillEffectKey key, short oldCount, short newCount, bool removed)
	{
		if (removed && IsSrcSkillPerformed && charId == base.CharacterId && key.SkillId == base.SkillTemplateId && key.IsDirect == base.IsDirect)
		{
			RemoveSelf(context);
		}
	}

	private void ClearAffectingSkill(DataContext context, CombatCharacter combatChar, sbyte direction)
	{
		short affectingDefendSkillId = combatChar.GetAffectingDefendSkillId();
		short affectingMoveSkillId = combatChar.GetAffectingMoveSkillId();
		if (affectingDefendSkillId >= 0 && DomainManager.CombatSkill.GetSkillDirection(combatChar.GetId(), affectingDefendSkillId) == direction)
		{
			DomainManager.Combat.ClearAffectingDefenseSkill(context, combatChar);
		}
		if (affectingMoveSkillId >= 0 && DomainManager.CombatSkill.GetSkillDirection(combatChar.GetId(), affectingMoveSkillId) == direction)
		{
			ClearAffectingAgileSkill(context, combatChar);
		}
	}

	private void SetDirectionCanCast(DataContext context, bool isAlly, bool isDirect, bool canCast)
	{
		int[] characterList = DomainManager.Combat.GetCharacterList(isAlly);
		for (int i = 0; i < characterList.Length; i++)
		{
			if (characterList[i] >= 0)
			{
				SetDirectionCanCast(context, DomainManager.Combat.GetElement_CombatCharacterDict(characterList[i]), isDirect, canCast);
			}
		}
	}

	private void SetDirectionCanCast(DataContext context, CombatCharacter combatChar, bool isDirect, bool canCast)
	{
		if (isDirect)
		{
			combatChar.CanCastDirectSkill = canCast;
		}
		else
		{
			combatChar.CanCastReverseSkill = canCast;
		}
		DomainManager.Combat.UpdateSkillCanUse(context, combatChar);
		DomainManager.Combat.UpdateTeammateCommandUsable(context, combatChar, ETeammateCommandImplement.Defend);
	}

	public static int CalcInterruptOdds(CombatSkillKey selfSkill, bool isDirect, CombatSkillKey enemySkill)
	{
		sbyte skillDirection = DomainManager.CombatSkill.GetSkillDirection(enemySkill.CharId, enemySkill.SkillTemplateId);
		return (skillDirection == (isDirect ? 1 : 0)) ? 100 : 0;
	}
}
