using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Defense;

namespace GameData.Domains.SpecialEffect.CombatSkill.Shixiangmen.DefenseAndAssist;

public class ShiXiangTieTouGong : DefenseSkillBase
{
	private const sbyte RequireInjuryCount = 3;

	public ShiXiangTieTouGong()
	{
	}

	public ShiXiangTieTouGong(CombatSkillKey skillKey)
		: base(skillKey, 6500)
	{
	}

	public override void OnEnable(DataContext context)
	{
		base.OnEnable(context);
		AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
		AffectDatas.Add(new AffectedDataKey(base.CharacterId, 114, -1), (EDataModifyType)3);
	}

	public override long GetModifiedValue(AffectedDataKey dataKey, long dataValue)
	{
		EDamageType customParam = (EDamageType)dataKey.CustomParam0;
		if (dataKey.CharId != base.CharacterId || customParam != EDamageType.Direct || !base.CanAffect)
		{
			return dataValue;
		}
		bool flag = dataKey.CustomParam1 == 1;
		sbyte b = (sbyte)dataKey.CustomParam2;
		if (b != 2 || flag == base.IsDirect)
		{
			return dataValue;
		}
		DamageStepCollection damageStepCollection = base.CombatChar.GetDamageStepCollection();
		int num = (flag ? base.CombatChar.GetInnerDamageValue()[b] : base.CombatChar.GetOuterDamageValue()[b]);
		int injuryStep = (flag ? damageStepCollection.InnerDamageSteps[b] : damageStepCollection.OuterDamageSteps[b]);
		if (CombatDomain.CalcInjuryMarkCount((int)Math.Min(num + dataValue, 2147483647L), injuryStep).markCount > 3)
		{
			return dataValue;
		}
		ShowSpecialEffectTips(0);
		return 0L;
	}
}
