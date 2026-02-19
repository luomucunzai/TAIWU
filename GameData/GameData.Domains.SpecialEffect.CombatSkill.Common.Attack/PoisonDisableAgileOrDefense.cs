using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Common.Attack;

public class PoisonDisableAgileOrDefense : CombatSkillEffectBase
{
	protected sbyte RequirePoisonType;

	private bool _disableSkills;

	private ushort CanAffectFieldId => (ushort)(base.IsDirect ? 285 : 287);

	protected PoisonDisableAgileOrDefense()
	{
	}

	protected PoisonDisableAgileOrDefense(CombatSkillKey skillKey, int type)
		: base(skillKey, type, -1)
	{
	}

	public override void OnEnable(DataContext context)
	{
		CreateAffectedAllEnemyData(CanAffectFieldId, (EDataModifyType)3, -1);
		Events.RegisterHandler_PrepareSkillEnd(OnPrepareSkillEnd);
		Events.RegisterHandler_CastSkillEnd(OnCastSkillEnd);
	}

	public override void OnDisable(DataContext context)
	{
		Events.UnRegisterHandler_PrepareSkillEnd(OnPrepareSkillEnd);
		Events.UnRegisterHandler_CastSkillEnd(OnCastSkillEnd);
	}

	private void OnPrepareSkillEnd(DataContext context, int charId, bool isAlly, short skillId)
	{
		if (charId == base.CharacterId && skillId == base.SkillTemplateId && DomainManager.Combat.InAttackRange(base.CombatChar) && base.CurrEnemyChar.GetDefeatMarkCollection().PoisonMarkList[RequirePoisonType] > 0)
		{
			_disableSkills = true;
			InvalidateCache(context, base.CurrEnemyChar.GetId(), CanAffectFieldId);
			ShowSpecialEffectTips(0);
		}
	}

	private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
	{
		if (charId == base.CharacterId && skillId == base.SkillTemplateId)
		{
			_disableSkills = false;
			RemoveSelf(context);
		}
	}

	public override bool GetModifiedValue(AffectedDataKey dataKey, bool dataValue)
	{
		ushort fieldId = dataKey.FieldId;
		if ((fieldId != 285 && fieldId != 287) || 1 == 0)
		{
			return dataValue;
		}
		return dataValue && !_disableSkills;
	}
}
