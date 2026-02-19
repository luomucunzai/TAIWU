using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Attack.JiuHan;

public class PreventMoveAndWeapon : CombatSkillEffectBase
{
	private const int ReduceMobilityRecoverSpeedPercent = -50;

	protected PreventMoveAndWeapon()
	{
	}

	protected PreventMoveAndWeapon(CombatSkillKey skillKey, int type)
		: base(skillKey, type, -1)
	{
	}

	public override void OnEnable(DataContext context)
	{
		CreateAffectedAllEnemyData(151, (EDataModifyType)3, -1);
		CreateAffectedAllEnemyData(197, (EDataModifyType)1, -1);
		Events.RegisterHandler_CastSkillEnd(OnCastSkillEnd);
	}

	public override void OnDisable(DataContext context)
	{
		Events.UnRegisterHandler_CastSkillEnd(OnCastSkillEnd);
	}

	private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
	{
		if (SkillKey.IsMatch(charId, skillId) && PowerMatchAffectRequire(power))
		{
			AddMaxEffectCount();
			ShowSpecialEffectTips(0);
		}
	}

	public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
	{
		if (base.EffectCount <= 0 || dataKey.FieldId != 197)
		{
			return 0;
		}
		return -50;
	}

	public override int GetModifiedValue(AffectedDataKey dataKey, int dataValue)
	{
		if (DomainManager.Combat.GetElement_CombatCharacterDict(dataKey.CharId).IsAlly == base.CombatChar.IsAlly || !DomainManager.Combat.IsCurrentCombatCharacter(base.CombatChar) || base.EffectCount <= 0)
		{
			return dataValue;
		}
		if (dataKey.FieldId == 151 && dataValue != 0)
		{
			int num = Math.Min(Math.Abs(dataValue), base.EffectCount);
			ReduceEffectCount(num);
			return dataValue + ((dataValue > 0) ? (-num) : num);
		}
		return dataValue;
	}
}
