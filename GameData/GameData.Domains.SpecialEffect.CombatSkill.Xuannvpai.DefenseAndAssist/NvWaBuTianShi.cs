using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.Character;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Defense;

namespace GameData.Domains.SpecialEffect.CombatSkill.Xuannvpai.DefenseAndAssist;

public class NvWaBuTianShi : DefenseSkillBase
{
	private Injuries _originInjuries;

	public NvWaBuTianShi()
	{
	}

	public NvWaBuTianShi(CombatSkillKey skillKey)
		: base(skillKey, 8506)
	{
		ListenCanAffectChange = true;
	}

	public override void OnEnable(DataContext context)
	{
		base.OnEnable(context);
		CreateAffectedData(282, (EDataModifyType)3, -1);
		_originInjuries = base.CombatChar.GetInjuries();
	}

	public override void OnDisable(DataContext context)
	{
		base.OnDisable(context);
		if (base.CombatChar.GetDefeatMarkCollection().GetTotalCount() >= GlobalConfig.NeedDefeatMarkCount[DomainManager.Combat.GetCombatType()])
		{
			Injuries injuries = base.CombatChar.GetInjuries().Subtract(_originInjuries);
			bool flag = false;
			for (sbyte b = 0; b < 7; b++)
			{
				sbyte b2 = injuries.Get(b, !base.IsDirect);
				if (b2 > 0)
				{
					DomainManager.Combat.RemoveInjury(context, base.CombatChar, b, !base.IsDirect, b2);
					flag = true;
				}
			}
			if (flag)
			{
				DomainManager.Combat.UpdateBodyDefeatMark(context, base.CombatChar);
				ShowSpecialEffectTips(0);
			}
		}
		DomainManager.Combat.AddToCheckFallenSet(base.CombatChar.GetId());
	}

	protected override void OnDefendSkillCanAffectChanged(DataContext context, DataUid dataUid)
	{
		DomainManager.Combat.AddToCheckFallenSet(base.CombatChar.GetId());
	}

	public override bool GetModifiedValue(AffectedDataKey dataKey, bool dataValue)
	{
		if (dataKey.CharId != base.CharacterId || dataKey.FieldId != 282)
		{
			return dataValue;
		}
		return dataValue || base.CanAffect;
	}
}
