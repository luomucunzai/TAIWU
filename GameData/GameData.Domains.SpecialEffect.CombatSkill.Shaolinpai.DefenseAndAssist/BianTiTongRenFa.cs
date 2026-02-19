using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Assist;

namespace GameData.Domains.SpecialEffect.CombatSkill.Shaolinpai.DefenseAndAssist;

public class BianTiTongRenFa : AssistSkillBase
{
	public BianTiTongRenFa()
	{
	}

	public BianTiTongRenFa(CombatSkillKey skillKey)
		: base(skillKey, 1603)
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
		if (flag == base.IsDirect)
		{
			return dataValue;
		}
		sbyte b = (sbyte)dataKey.CustomParam2;
		sbyte b2 = base.CombatChar.GetInjuries().Get(b, flag);
		if (b2 > 0)
		{
			return dataValue;
		}
		DamageStepCollection damageStepCollection = base.CombatChar.GetDamageStepCollection();
		int num = (flag ? damageStepCollection.InnerDamageSteps : damageStepCollection.OuterDamageSteps)[b];
		int damageValue = base.CombatChar.GetDamageValue(b, flag);
		if (damageValue + dataValue <= num)
		{
			return dataValue;
		}
		ShowSpecialEffectTips(0);
		return num - damageValue;
	}
}
