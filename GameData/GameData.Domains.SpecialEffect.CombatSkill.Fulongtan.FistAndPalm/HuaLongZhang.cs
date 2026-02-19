using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Fulongtan.FistAndPalm;

public class HuaLongZhang : CombatSkillEffectBase
{
	private bool _changedSkill;

	private static CValuePercent AddPowerPercent => CValuePercent.op_Implicit(10);

	public HuaLongZhang()
	{
	}

	public HuaLongZhang(CombatSkillKey skillKey)
		: base(skillKey, 14108, -1)
	{
	}

	public override void OnEnable(DataContext context)
	{
		AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
		AffectDatas.Add(new AffectedDataKey(base.CharacterId, 156, -1), (EDataModifyType)3);
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
			if (!_changedSkill)
			{
				AddMaxEffectCount();
			}
			else
			{
				CombatCharacter currEnemyChar = base.CurrEnemyChar;
				DomainManager.Combat.ClearAffectingDefenseSkill(context, currEnemyChar);
				ClearAffectingAgileSkill(context, currEnemyChar);
				ChangeMobilityValue(context, currEnemyChar, -currEnemyChar.GetMobilityValue());
				ShowSpecialEffectTips(1);
			}
		}
		_changedSkill = false;
		DomainManager.Combat.RemoveSkillPowerReplaceInCombat(context, SkillKey);
		DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 199);
	}

	public override int GetModifiedValue(AffectedDataKey dataKey, int dataValue)
	{
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		if (dataKey.CharId != base.CharacterId || base.EffectCount == 0 || dataValue == base.SkillTemplateId)
		{
			return dataValue;
		}
		if (dataKey.FieldId == 156)
		{
			DataContext context = DomainManager.Combat.Context;
			CombatSkillKey combatSkillKey = new CombatSkillKey(base.CharacterId, (short)dataValue);
			GameData.Domains.CombatSkill.CombatSkill element_CombatSkills = DomainManager.CombatSkill.GetElement_CombatSkills(combatSkillKey);
			if (DomainManager.CombatSkill.GetSkillType(base.CharacterId, (short)dataValue) == 3 && element_CombatSkills.GetDirection() == base.SkillInstance.GetDirection())
			{
				_changedSkill = true;
				int power = (int)base.SkillInstance.GetPower() * AddPowerPercent;
				DomainManager.Combat.AddSkillPowerInCombat(context, combatSkillKey, new SkillEffectKey(base.SkillTemplateId, base.IsDirect), power);
				DomainManager.Combat.SetSkillPowerReplaceInCombat(context, SkillKey, combatSkillKey);
				ShowSpecialEffectTips(0);
				ReduceEffectCount();
				return base.SkillTemplateId;
			}
		}
		return dataValue;
	}
}
