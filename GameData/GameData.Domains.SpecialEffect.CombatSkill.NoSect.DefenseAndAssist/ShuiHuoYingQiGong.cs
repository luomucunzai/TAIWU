using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Defense;

namespace GameData.Domains.SpecialEffect.CombatSkill.NoSect.DefenseAndAssist;

public class ShuiHuoYingQiGong : DefenseSkillBase
{
	private const int DefaultReduceDamage = 80;

	private const int MinReduceDamage = 0;

	private const int ReduceEffectUnit = 1;

	private int _reduceDamage;

	private static CValuePercent ReduceEffectStep => CValuePercent.op_Implicit(20);

	public ShuiHuoYingQiGong()
	{
		AutoRemove = false;
	}

	public ShuiHuoYingQiGong(CombatSkillKey skillKey)
		: base(skillKey, 300)
	{
		AutoRemove = false;
	}

	public override void OnEnable(DataContext context)
	{
		base.OnEnable(context);
		_reduceDamage = 80;
		CreateAffectedData(114, (EDataModifyType)3, -1);
		CreateAffectedData(253, (EDataModifyType)3, -1);
	}

	public override long GetModifiedValue(AffectedDataKey dataKey, long dataValue)
	{
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		if (dataKey.CharId != base.CharacterId || dataKey.FieldId != 114 || !base.CanAffect)
		{
			return base.GetModifiedValue(dataKey, dataValue);
		}
		EDamageType customParam = (EDamageType)dataKey.CustomParam0;
		if (customParam != EDamageType.Direct)
		{
			return base.GetModifiedValue(dataKey, dataValue);
		}
		long num = dataValue * CValuePercent.op_Implicit(_reduceDamage);
		if (num <= 0)
		{
			return base.GetModifiedValue(dataKey, dataValue);
		}
		ShowSpecialEffectTipsOnceInFrame(0);
		bool flag = dataKey.CustomParam1 == 1;
		sbyte b = (sbyte)dataKey.CustomParam2;
		DamageStepCollection damageStepCollection = base.CombatChar.GetDamageStepCollection();
		int num2 = (flag ? damageStepCollection.InnerDamageSteps : damageStepCollection.OuterDamageSteps)[b];
		if (dataValue >= num2 * ReduceEffectStep)
		{
			int num3 = (int)dataValue / Math.Max(num2 * ReduceEffectStep, 1);
			int reduceDamage = _reduceDamage;
			_reduceDamage = Math.Max(_reduceDamage - num3, 0);
			if (reduceDamage != _reduceDamage)
			{
				InvalidateCache(DomainManager.Combat.Context, 253);
			}
		}
		return dataValue - num;
	}

	public override List<CombatSkillEffectData> GetModifiedValue(AffectedDataKey dataKey, List<CombatSkillEffectData> dataValue)
	{
		if (dataKey.SkillKey != SkillKey || dataKey.FieldId != 253)
		{
			return base.GetModifiedValue(dataKey, dataValue);
		}
		dataValue.Add(new CombatSkillEffectData(ECombatSkillEffectType.ShuiHuoYingQiGongReduceDamage, _reduceDamage));
		return dataValue;
	}
}
