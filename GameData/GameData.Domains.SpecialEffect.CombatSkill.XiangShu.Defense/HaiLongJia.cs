using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Defense;

namespace GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Defense;

public class HaiLongJia : DefenseSkillBase
{
	public HaiLongJia()
	{
	}

	public HaiLongJia(CombatSkillKey skillKey)
		: base(skillKey, 16305)
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
		if (dataKey.CharId != base.CharacterId || !base.CanAffect)
		{
			return dataValue;
		}
		int num = base.CombatChar.DefendSkillLeftFrame * 100 / base.CombatChar.DefendSkillTotalFrame;
		if (num > 0)
		{
			int num2 = base.CombatChar.GetDamageStepCollection().FatalDamageStep / 10;
			long num3 = Math.Min(dataValue / num2, num);
			base.CombatChar.DefendSkillLeftFrame = (short)Math.Max(0L, base.CombatChar.DefendSkillLeftFrame - base.CombatChar.DefendSkillTotalFrame * num3 / 100);
			ShowSpecialEffectTipsOnceInFrame(0);
			return num2 * (dataValue / num2 - num3);
		}
		return dataValue;
	}
}
