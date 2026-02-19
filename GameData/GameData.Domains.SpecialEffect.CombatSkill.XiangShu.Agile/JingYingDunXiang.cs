using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Agile;

namespace GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Agile;

public class JingYingDunXiang : AgileSkillBase
{
	public JingYingDunXiang()
	{
	}

	public JingYingDunXiang(CombatSkillKey skillKey)
		: base(skillKey, 16206)
	{
	}

	public override void OnEnable(DataContext context)
	{
		base.OnEnable(context);
		CreateAffectedData(114, (EDataModifyType)3, -1);
	}

	public override long GetModifiedValue(AffectedDataKey dataKey, long dataValue)
	{
		EDamageType customParam = (EDamageType)dataKey.CustomParam0;
		if (dataKey.CharId != base.CharacterId || customParam != EDamageType.Direct || !base.CanAffect)
		{
			return dataValue;
		}
		if (base.CombatChar.GetMobilityValue() == 0)
		{
			return dataValue;
		}
		int num = base.CombatChar.GetMobilityValue() * 100 / MoveSpecialConstants.MaxMobility + 1;
		int num2 = base.CombatChar.GetDamageStepCollection().FatalDamageStep / 10;
		long num3 = Math.Min(dataValue / num2, num);
		DomainManager.Combat.ChangeMobilityValue(DomainManager.Combat.Context, base.CombatChar, (int)(-MoveSpecialConstants.MaxMobility * num3 / 100), changedByEffect: true, base.CombatChar);
		ShowSpecialEffectTipsOnceInFrame(0);
		return num2 * (dataValue / num2 - num3);
	}
}
