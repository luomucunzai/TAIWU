using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Jieqingmen.Throw;

public class FeiXingShu : CombatSkillEffectBase
{
	private const int ShaAddFlawFactor = 10;

	public FeiXingShu()
	{
	}

	public FeiXingShu(CombatSkillKey skillKey)
		: base(skillKey, 13303, -1)
	{
	}

	public override void OnEnable(DataContext context)
	{
		CreateAffectedData(318, (EDataModifyType)1, -1);
		Events.RegisterHandler_PrepareSkillEffectNotYetCreated(OnPrepareSkillEffectNotYetCreated);
		Events.RegisterHandler_CastSkillEnd(OnCastSkillEnd);
	}

	public override void OnDisable(DataContext context)
	{
		Events.UnRegisterHandler_PrepareSkillEffectNotYetCreated(OnPrepareSkillEffectNotYetCreated);
		Events.UnRegisterHandler_CastSkillEnd(OnCastSkillEnd);
	}

	private void OnPrepareSkillEffectNotYetCreated(DataContext context, CombatCharacter character, short skillId)
	{
		if (character.GetId() == base.CharacterId && CombatSkillTemplateHelper.IsAttack(skillId))
		{
			DoRearrangeTrick(context);
		}
	}

	private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
	{
		if (charId == base.CharacterId && skillId == base.SkillTemplateId && PowerMatchAffectRequire(power))
		{
			AddMaxEffectCount();
		}
	}

	private void DoRearrangeTrick(DataContext context)
	{
		CombatCharacter combatCharacter = (base.IsDirect ? base.CombatChar : base.EnemyChar);
		if (combatCharacter.GetTrickCount(19) > 0 && base.EffectCount > 0)
		{
			TrickCollection tricks = combatCharacter.GetTricks();
			tricks.RearrangeTrick(19);
			combatCharacter.SetTricks(tricks, context);
			Events.RaiseRearrangeTrick(context, combatCharacter.GetId(), combatCharacter.IsAlly);
			ReduceEffectCount();
			ShowSpecialEffectTips(0);
		}
	}

	public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
	{
		if (dataKey.CharId != base.CharacterId || dataKey.IsNormalAttack || dataKey.FieldId != 318)
		{
			return 0;
		}
		byte trickCount = (base.IsDirect ? base.CombatChar : base.EnemyChar).GetTrickCount(19);
		if (trickCount <= 0)
		{
			return 0;
		}
		ShowSpecialEffectTipsOnceInFrame(1);
		return trickCount * 10;
	}
}
