using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Zhujianshanzhuang.AttackCommon;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.Zhujianshanzhuang.Blade;

public class MingHongJueDao : BladeUnlockEffectBase
{
	private const int AddDirectDamagePercent = 300;

	private bool _affected;

	private CValuePercent MaxStealUnlockValuePercent => CValuePercent.op_Implicit(base.IsDirectOrReverseEffectDoubling ? 40 : 20);

	protected override IEnumerable<short> RequireWeaponTypes
	{
		get
		{
			yield return 8;
		}
	}

	public MingHongJueDao()
	{
	}

	public MingHongJueDao(CombatSkillKey skillKey)
		: base(skillKey, 9207)
	{
	}

	public override void OnEnable(DataContext context)
	{
		base.OnEnable(context);
		CreateAffectedData(69, (EDataModifyType)1, -1);
		Events.RegisterHandler_UnlockAttackEnd(OnUnlockAttackEnd);
	}

	public override void OnDisable(DataContext context)
	{
		Events.UnRegisterHandler_UnlockAttackEnd(OnUnlockAttackEnd);
		base.OnDisable(context);
	}

	protected override void OnCastAddUnlockAttackValue(DataContext context, CValuePercent power)
	{
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
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
		int num = GlobalConfig.Instance.UnlockAttackUnit - unlockPrepareValue[usingWeaponIndex];
		if (num == 0)
		{
			return;
		}
		int num2 = GlobalConfig.Instance.UnlockAttackUnit * MaxStealUnlockValuePercent * power;
		if (num2 == 0)
		{
			return;
		}
		int num3 = num / 2;
		List<int> list = ObjectPool<List<int>>.Instance.Get();
		List<int> list2 = ObjectPool<List<int>>.Instance.Get();
		for (int i = 0; i < 3; i++)
		{
			if (i != usingWeaponIndex)
			{
				int num4 = Math.Min(unlockPrepareValue[i], num2);
				if (num4 != 0)
				{
					num3 = Math.Min(num3, num4);
					list.Add(i);
					list2.Add(num4);
				}
			}
		}
		int num5 = Math.Min(num2 * list.Count, num);
		int num6 = num5 - num3 * list.Count;
		for (int j = 0; j < list.Count; j++)
		{
			int num7 = Math.Min(list2[j] - num3, num6);
			num6 -= num7;
			list2[j] = num3 + num7;
		}
		for (int k = 0; k < list.Count; k++)
		{
			int index = list[k];
			int num8 = list2[k];
			base.CombatChar.ChangeUnlockAttackValue(context, index, -num8);
			base.CombatChar.ChangeUnlockAttackValue(context, base.CombatChar.GetUsingWeaponIndex(), num8);
		}
		if (list.Count > 0)
		{
			ShowSpecialEffectTips(base.IsDirect, 1, 0);
		}
		ObjectPool<List<int>>.Instance.Return(list);
		ObjectPool<List<int>>.Instance.Return(list2);
	}

	public override void DoAffectAfterCost(DataContext context, int weaponIndex)
	{
		_affected = true;
		ShowSpecialEffectTips(base.IsDirect, 2, 1);
	}

	private void OnUnlockAttackEnd(DataContext context, CombatCharacter attacker)
	{
		if (attacker.GetId() == base.CharacterId)
		{
			_affected = false;
		}
	}

	public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
	{
		if (dataKey.CharId != base.CharacterId || dataKey.FieldId != 69 || !_affected)
		{
			return 0;
		}
		return 300;
	}
}
