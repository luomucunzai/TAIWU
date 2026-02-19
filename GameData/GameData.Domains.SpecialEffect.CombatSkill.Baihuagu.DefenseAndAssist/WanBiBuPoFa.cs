using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Defense;

namespace GameData.Domains.SpecialEffect.CombatSkill.Baihuagu.DefenseAndAssist;

public class WanBiBuPoFa : DefenseSkillBase
{
	public WanBiBuPoFa()
	{
	}

	public WanBiBuPoFa(CombatSkillKey skillKey)
		: base(skillKey, 3508)
	{
	}

	public override void OnEnable(DataContext context)
	{
		base.OnEnable(context);
		CreateAffectedData(114, (EDataModifyType)3, -1);
	}

	public override long GetModifiedValue(AffectedDataKey dataKey, long dataValue)
	{
		if (dataKey.CharId != base.CharacterId || !base.CanAffect || dataKey.FieldId != 114)
		{
			return dataValue;
		}
		EDamageType customParam = (EDamageType)dataKey.CustomParam0;
		if (customParam != EDamageType.Direct)
		{
			return dataValue;
		}
		bool flag = dataKey.CustomParam1 == 1;
		sbyte b = (sbyte)dataKey.CustomParam2;
		int num = (flag ? base.CombatChar.GetInnerDamageValue() : base.CombatChar.GetOuterDamageValue())[b];
		DamageStepCollection damageStepCollection = base.CombatChar.GetDamageStepCollection();
		int injuryStep = (flag ? damageStepCollection.InnerDamageSteps : damageStepCollection.OuterDamageSteps)[b];
		int item = CombatDomain.CalcInjuryMarkCount((int)Math.Min(dataValue + num, 2147483647L), injuryStep).markCount;
		sbyte oldMark = base.CombatChar.GetInjuries().Get(b, flag);
		long num2 = CalcReturnValue(dataValue, item, oldMark);
		if (dataValue > 0 && num2 == 0)
		{
			ShowSpecialEffectTips(0);
		}
		return num2;
	}

	private long CalcReturnValue(long dataValue, int newMark, int oldMark)
	{
		if (base.IsDirect)
		{
			return (newMark < oldMark) ? dataValue : 0;
		}
		return (newMark > oldMark) ? dataValue : 0;
	}
}
