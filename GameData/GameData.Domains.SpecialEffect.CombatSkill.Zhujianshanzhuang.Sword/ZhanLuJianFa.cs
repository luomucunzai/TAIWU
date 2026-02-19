using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Zhujianshanzhuang.AttackCommon;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.Zhujianshanzhuang.Sword;

public class ZhanLuJianFa : SwordUnlockEffectBase
{
	private const int GiveValueMultiplier = 2;

	private CValuePercent MaxGiveUnlockValuePercent => CValuePercent.op_Implicit(base.IsDirectOrReverseEffectDoubling ? 50 : 25);

	protected override IEnumerable<sbyte> RequirePersonalityTypes
	{
		get
		{
			yield return 4;
		}
	}

	protected override int RequirePersonalityValue => 50;

	public ZhanLuJianFa()
	{
	}

	public ZhanLuJianFa(CombatSkillKey skillKey)
		: base(skillKey, 9107)
	{
	}

	public override void OnEnable(DataContext context)
	{
		base.OnEnable(context);
		CreateAffectedData(307, (EDataModifyType)3, -1);
	}

	protected override void OnCastAddUnlockAttackValue(DataContext context, CValuePercent power)
	{
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		base.OnCastAddUnlockAttackValue(context, power);
		if (!base.IsReverseOrUsingDirectWeapon)
		{
			return;
		}
		int usingWeaponIndex = base.CombatChar.GetUsingWeaponIndex();
		if (usingWeaponIndex >= 3 || base.CombatChar.GetUnlockEffect(usingWeaponIndex) == null)
		{
			return;
		}
		List<int> unlockPrepareValue = base.CombatChar.GetUnlockPrepareValue();
		if (unlockPrepareValue[usingWeaponIndex] <= 0)
		{
			return;
		}
		int num = GlobalConfig.Instance.UnlockAttackUnit * MaxGiveUnlockValuePercent * power;
		if (num <= 0)
		{
			return;
		}
		num = Math.Min(num, unlockPrepareValue[usingWeaponIndex]);
		List<int> list = ObjectPool<List<int>>.Instance.Get();
		List<int> list2 = ObjectPool<List<int>>.Instance.Get();
		for (int i = 0; i < 3; i++)
		{
			if (i != usingWeaponIndex)
			{
				int num2 = GlobalConfig.Instance.UnlockAttackUnit - unlockPrepareValue[i];
				if (num2 != 0 && base.CombatChar.CanUnlockAttackByConfig(i))
				{
					int val = num2 / 2 + ((num2 % 2 > 0) ? 1 : 0);
					val = Math.Min(val, num);
					list.Add(i);
					list2.Add(val);
				}
			}
		}
		if (list.Count > 0)
		{
			int randomIndex = RandomUtils.GetRandomIndex(list2, context.Random);
			int index = list[randomIndex];
			int num3 = list2[randomIndex];
			base.CombatChar.ChangeUnlockAttackValue(context, index, num3 * 2);
			base.CombatChar.ChangeUnlockAttackValue(context, usingWeaponIndex, -num3);
			ShowSpecialEffectTips(base.IsDirect, 1, 0);
		}
		ObjectPool<List<int>>.Instance.Return(list);
		ObjectPool<List<int>>.Instance.Return(list2);
	}

	public override bool GetModifiedValue(AffectedDataKey dataKey, bool dataValue)
	{
		if (dataKey.CharId != base.CharacterId || dataKey.FieldId != 307 || base.EffectCount <= 0)
		{
			return dataValue;
		}
		ReduceEffectCount();
		ShowSpecialEffectTips(base.IsDirect, 2, 1);
		return true;
	}
}
