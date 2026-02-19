using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Assist;

namespace GameData.Domains.SpecialEffect.CombatSkill.Baihuagu.DefenseAndAssist;

public class SanShiLiuShouYiZhenFa : AssistSkillBase
{
	private const sbyte ChangePercent = 20;

	private int CriticalOddsAddPercent => base.IsDirect ? 40 : (-40);

	public SanShiLiuShouYiZhenFa()
	{
	}

	public SanShiLiuShouYiZhenFa(CombatSkillKey skillKey)
		: base(skillKey, 3600)
	{
		SetConstAffectingOnCombatBegin = true;
	}

	public override void OnEnable(DataContext context)
	{
		base.OnEnable(context);
		for (sbyte b = 0; b < 4; b++)
		{
			CreateAffectedData((ushort)((base.IsDirect ? 56 : 90) + b), (EDataModifyType)1, -1);
		}
		Events.RegisterHandler_CombatBegin(OnCombatBegin);
		Events.RegisterHandler_NormalAttackBegin(OnNormalAttackBegin);
	}

	public override void OnDisable(DataContext context)
	{
		Events.UnRegisterHandler_CombatBegin(OnCombatBegin);
		Events.UnRegisterHandler_NormalAttackBegin(OnNormalAttackBegin);
		base.OnDisable(context);
	}

	private void OnCombatBegin(DataContext context)
	{
		if (base.IsDirect)
		{
			AppendAffectedData(context, 254, (EDataModifyType)1, -1);
		}
		else
		{
			AppendAffectedAllEnemyData(context, 254, (EDataModifyType)1, -1);
		}
	}

	private void OnNormalAttackBegin(DataContext context, CombatCharacter attacker, CombatCharacter defender, sbyte trickType, int pursueIndex)
	{
		if (!(base.IsDirect ? (attacker.GetId() != base.CharacterId) : (attacker.IsAlly == base.CombatChar.IsAlly)) && attacker.GetChangeTrickAttack() && base.IsCurrent)
		{
			ShowSpecialEffectTips(0);
		}
	}

	protected override void OnCanUseChanged(DataContext context, DataUid dataUid)
	{
		SetConstAffecting(context, base.CanAffect);
	}

	public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
	{
		if (!dataKey.IsNormalAttack || !base.CanAffect)
		{
			return 0;
		}
		CombatCharacter element_CombatCharacterDict = DomainManager.Combat.GetElement_CombatCharacterDict(dataKey.CharId);
		if (dataKey.FieldId == 254)
		{
			return (base.IsCurrent && element_CombatCharacterDict.GetChangeTrickAttack()) ? CriticalOddsAddPercent : 0;
		}
		if (dataKey.CharId != base.CharacterId || !(base.IsDirect ? base.CombatChar : base.CurrEnemyChar).GetChangeTrickAttack() || dataKey.CustomParam1 != 0)
		{
			return 0;
		}
		return base.IsDirect ? 20 : (-20);
	}
}
