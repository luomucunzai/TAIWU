using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Wudangpai.Neigong;

public class YuanShiXianTianGong : CombatSkillEffectBase
{
	private const long DamageToGangqiFactor = 30L;

	private const long GangqiToDamageFactor = 20L;

	private const int GangqiToBreathOrStanceFactor = 1000;

	public YuanShiXianTianGong()
	{
	}

	public YuanShiXianTianGong(CombatSkillKey skillKey)
		: base(skillKey, 4008, -1)
	{
	}

	public override void OnEnable(DataContext context)
	{
		base.OnEnable(context);
		Events.RegisterHandler_CreateGangqiAfterChangeNeiliAllocation(OnCreateGangqiAfterChangeNeiliAllocation);
		Events.RegisterHandler_AddDirectDamageValue(OnAddDirectDamageValue);
		CreateAffectedData(323, (EDataModifyType)3, -1);
		CreateAffectedData(320, (EDataModifyType)3, -1);
	}

	public override void OnDisable(DataContext context)
	{
		Events.UnRegisterHandler_CreateGangqiAfterChangeNeiliAllocation(OnCreateGangqiAfterChangeNeiliAllocation);
		Events.UnRegisterHandler_AddDirectDamageValue(OnAddDirectDamageValue);
		base.OnDisable(context);
	}

	private void OnCreateGangqiAfterChangeNeiliAllocation(DataContext context, CombatCharacter character)
	{
		if (character.GetId() == base.CharacterId)
		{
			int num = character.GetNeiliAllocation().Sum();
			if (num > 0)
			{
				character.CreateGangqi(context, num);
				ShowSpecialEffectTips(0);
			}
		}
	}

	private void OnAddDirectDamageValue(DataContext context, int attackerId, int defenderId, sbyte bodyPart, bool isInner, int damageValue, short combatSkillId)
	{
		if (base.CharacterId == (base.IsDirect ? attackerId : defenderId) && (long)damageValue >= 30L)
		{
			int num = (int)Math.Min((long)damageValue / 30L, 2147483647L);
			if (num > 0)
			{
				base.CombatChar.ChangeGangqi(context, num);
				ShowSpecialEffectTipsOnceInFrame(2);
			}
		}
	}

	public override long GetModifiedValue(AffectedDataKey dataKey, long dataValue)
	{
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
		if (dataKey.CharId != base.CharacterId || base.CombatChar.GetGangqi() <= 0 || dataKey.CustomParam0 == base.CharacterId || dataKey.FieldId != (base.IsDirect ? 320 : 323))
		{
			return dataValue;
		}
		int objectId = (base.IsDirect ? dataKey.CustomParam0 : base.CharacterId);
		CombatCharacter element_CombatCharacterDict = DomainManager.Combat.GetElement_CombatCharacterDict(objectId);
		CombatSkillKey objectId2 = new CombatSkillKey(element_CombatCharacterDict.GetId(), dataKey.CombatSkillId);
		sbyte b = (dataKey.IsNormalAttack ? DomainManager.Combat.GetUsingWeaponData(element_CombatCharacterDict).GetInnerRatio() : DomainManager.CombatSkill.GetElement_CombatSkills(objectId2).GetCurrInnerRatio());
		int num = 100 - b;
		int num2 = Math.Abs(b - num) / 2;
		CValuePercent val = CValuePercent.op_Implicit(base.IsDirect ? (50 + num2) : (100 - num2));
		long num3 = Math.Min(dataValue * val, (long)base.CombatChar.GetGangqi() * 20L);
		if (num3 == 0)
		{
			return dataValue;
		}
		ShowSpecialEffectTipsOnceInFrame(1);
		DataContext context = DomainManager.Combat.Context;
		int num4 = (int)(num3 / 20);
		if (num4 > 0)
		{
			DoCostGangqi(context, num4, b, num);
		}
		return base.IsDirect ? (dataValue - num3) : (dataValue + num3);
	}

	private void DoCostGangqi(DataContext context, int costGangqi, sbyte innerRatio, int outerRatio)
	{
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		base.CombatChar.ChangeGangqi(context, -costGangqi);
		CombatCharacter combatCharacter = (base.IsDirect ? base.CombatChar : base.EnemyChar);
		int num = (base.IsDirect ? 1 : (-1));
		CValuePercent val = CValuePercent.op_Implicit(innerRatio * costGangqi / 1000);
		if (val > 0)
		{
			ChangeBreathValue(context, combatCharacter, combatCharacter.GetMaxBreathValue() * val * num);
		}
		CValuePercent val2 = CValuePercent.op_Implicit(outerRatio * costGangqi / 1000);
		if (val2 > 0)
		{
			ChangeStanceValue(context, combatCharacter, combatCharacter.GetMaxStanceValue() * val2 * num);
		}
	}
}
