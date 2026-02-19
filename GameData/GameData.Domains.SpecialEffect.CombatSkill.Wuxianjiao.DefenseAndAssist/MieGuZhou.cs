using System.Collections.Generic;
using Config;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Defense;

namespace GameData.Domains.SpecialEffect.CombatSkill.Wuxianjiao.DefenseAndAssist;

public class MieGuZhou : DefenseSkillBase
{
	public MieGuZhou()
	{
	}

	public MieGuZhou(CombatSkillKey skillKey)
		: base(skillKey, 12703)
	{
	}

	public override void OnEnable(DataContext context)
	{
		base.OnEnable(context);
		AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
		AffectDatas.Add(new AffectedDataKey(base.CharacterId, 180, -1), (EDataModifyType)3);
		Events.RegisterHandler_NormalAttackEnd(OnNormalAttackEnd);
		ShowSpecialEffectTips(0);
	}

	public override void OnDisable(DataContext context)
	{
		base.OnDisable(context);
		Events.UnRegisterHandler_NormalAttackEnd(OnNormalAttackEnd);
	}

	private void OnNormalAttackEnd(DataContext context, CombatCharacter attacker, CombatCharacter defender, sbyte trickType, int pursueIndex, bool hit, bool isFightBack)
	{
		if (isFightBack && hit && attacker == base.CombatChar && base.CanAffect)
		{
			CombatCharacter combatChar = (base.IsDirect ? base.CombatChar : base.CurrEnemyChar);
			CombatCharacter affectChar = (base.IsDirect ? base.CurrEnemyChar : base.CombatChar);
			short num = DomainManager.Combat.RemoveRandomWug(context, combatChar);
			if (num >= 0)
			{
				sbyte wugType = Medicine.Instance[num].WugType;
				DomainManager.Combat.AddWug(context, affectChar, wugType, base.IsDirect, base.CharacterId);
				ShowSpecialEffectTips(1);
			}
		}
	}

	public override bool GetModifiedValue(AffectedDataKey dataKey, bool dataValue)
	{
		if (dataKey.CharId != base.CharacterId || !base.CanAffect)
		{
			return dataValue;
		}
		if (dataKey.FieldId == 180 && dataKey.CustomParam0 != base.CharacterId)
		{
			return false;
		}
		return dataValue;
	}
}
