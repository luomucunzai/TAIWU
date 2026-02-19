using System;
using System.Collections.Generic;
using System.Linq;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.Item;
using GameData.Domains.SpecialEffect.CombatSkill.Zhujianshanzhuang.AttackCommon;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.Zhujianshanzhuang.Blade;

public class KunWuDaoFa : BladeUnlockEffectBase
{
	private const int MaxReduceDurabilityCount = 3;

	private static readonly CValuePercent ReduceDurabilityPercent = CValuePercent.op_Implicit(20);

	private CValuePercent AddPrepareProgress => CValuePercent.op_Implicit(base.IsDirectOrReverseEffectDoubling ? 80 : 40);

	protected override IEnumerable<short> RequireWeaponTypes
	{
		get
		{
			yield return 3;
			yield return 11;
		}
	}

	public KunWuDaoFa()
	{
	}

	public KunWuDaoFa(CombatSkillKey skillKey)
		: base(skillKey, 9203)
	{
	}

	public override void OnEnable(DataContext context)
	{
		base.OnEnable(context);
		Events.RegisterHandler_PrepareSkillBegin(OnPrepareSkillBegin);
	}

	public override void OnDisable(DataContext context)
	{
		Events.UnRegisterHandler_PrepareSkillBegin(OnPrepareSkillBegin);
		base.OnDisable(context);
	}

	private void OnPrepareSkillBegin(DataContext context, int charId, bool isAlly, short skillId)
	{
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		if (SkillKey.IsMatch(charId, skillId) && base.IsReverseOrUsingDirectWeapon)
		{
			DomainManager.Combat.ChangeSkillPrepareProgress(base.CombatChar, base.CombatChar.SkillPrepareTotalProgress * AddPrepareProgress);
			ShowSpecialEffectTips(base.IsDirect, 1, 0);
		}
	}

	private IEnumerable<ItemKey> CanReduceDurabilityWeaponOrArmors()
	{
		ItemKey[] weapons = base.CurrEnemyChar.GetWeapons();
		ItemKey[] array = weapons;
		for (int i = 0; i < array.Length; i++)
		{
			ItemKey key = array[i];
			if (key.IsValid())
			{
				ItemBase item = DomainManager.Item.GetBaseItem(key);
				if (item.GetCurrDurability() > 0 && CombatDomain.IsWeaponCanBreak(item.GetItemSubType()))
				{
					yield return key;
				}
			}
		}
		ItemKey[] armors = base.CurrEnemyChar.Armors;
		ItemKey[] array2 = armors;
		for (int j = 0; j < array2.Length; j++)
		{
			ItemKey key2 = array2[j];
			if (key2.IsValid())
			{
				ItemBase item2 = DomainManager.Item.GetBaseItem(key2);
				if (item2.GetCurrDurability() > 0)
				{
					yield return key2;
				}
			}
		}
	}

	protected override bool CanDoAffect()
	{
		return CanReduceDurabilityWeaponOrArmors().Any();
	}

	public override void DoAffectAfterCost(DataContext context, int weaponIndex)
	{
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		List<ItemKey> list = ObjectPool<List<ItemKey>>.Instance.Get();
		list.Clear();
		foreach (ItemKey item in CanReduceDurabilityWeaponOrArmors())
		{
			if (!list.Contains(item))
			{
				list.Add(item);
			}
		}
		foreach (ItemKey item2 in RandomUtils.GetRandomUnrepeated(context.Random, 3, list))
		{
			ItemBase baseItem = DomainManager.Item.GetBaseItem(item2);
			int num = Math.Max((int)baseItem.GetCurrDurability() * ReduceDurabilityPercent, 1);
			ChangeDurability(context, base.CurrEnemyChar, item2, -num);
		}
		ObjectPool<List<ItemKey>>.Instance.Return(list);
		ShowSpecialEffectTips(base.IsDirect, 2, 1);
	}
}
