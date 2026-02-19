using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Defense;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.Yuanshanpai.DefenseAndAssist;

public class JinDaoHuanZhangGong : DefenseSkillBase
{
	private const int InevitableAvoidOdds = 75;

	public JinDaoHuanZhangGong()
	{
	}

	public JinDaoHuanZhangGong(CombatSkillKey skillKey)
		: base(skillKey, 5503)
	{
	}

	public override void OnEnable(DataContext context)
	{
		base.OnEnable(context);
		CreateAffectedData(291, (EDataModifyType)3, -1);
	}

	public override bool GetModifiedValue(AffectedDataKey dataKey, bool dataValue)
	{
		if (dataKey.CharId != base.CharacterId || dataKey.FieldId != 291 || dataValue)
		{
			return base.GetModifiedValue(dataKey, dataValue);
		}
		int customParam = dataKey.CustomParam2;
		CombatCharacter element_CombatCharacterDict = DomainManager.Combat.GetElement_CombatCharacterDict(customParam);
		if (base.IsDirect ? (!element_CombatCharacterDict.GetChangeTrickAttack()) : (element_CombatCharacterDict.PursueAttackCount == 0))
		{
			return false;
		}
		DataContext context = DomainManager.Combat.Context;
		bool flag = context.Random.CheckPercentProb(75);
		if (flag)
		{
			ShowSpecialEffectTipsOnceInFrame(0);
		}
		return flag;
	}
}
