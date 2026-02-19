using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.Item;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Defense;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.Zhujianshanzhuang.DefenseAndAssist;

public class LingLongJiuQiao : DefenseSkillBase
{
	private const int AddOrReduceDurabilityValue = 3;

	private static readonly IReadOnlyList<sbyte> TargetEquipmentSlots = new sbyte[7] { 0, 1, 2, 3, 5, 6, 7 };

	public LingLongJiuQiao()
	{
	}

	public LingLongJiuQiao(CombatSkillKey skillKey)
		: base(skillKey, 9705)
	{
	}

	public override void OnEnable(DataContext context)
	{
		base.OnEnable(context);
		Events.RegisterHandler_NormalAttackEnd(OnNormalAttackEnd);
	}

	public override void OnDisable(DataContext context)
	{
		Events.UnRegisterHandler_NormalAttackEnd(OnNormalAttackEnd);
		base.OnDisable(context);
	}

	private void OnNormalAttackEnd(DataContext context, CombatCharacter attacker, CombatCharacter defender, sbyte trickType, int pursueIndex, bool hit, bool isFightBack)
	{
		if (!hit && pursueIndex <= 0 && defender == base.CombatChar && base.CanAffect)
		{
			DoEffect(context);
		}
	}

	private void DoEffect(DataContext context)
	{
		ItemKey[] equipment = CharObj.GetEquipment();
		List<ItemKey> list = ObjectPool<List<ItemKey>>.Instance.Get();
		list.Clear();
		for (int i = 8; i <= 10; i++)
		{
			ItemKey item = equipment[i];
			if (item.IsValid() && DomainManager.Item.GetElement_Accessories(item.Id).GetCurrDurability() > 0)
			{
				list.Add(item);
			}
		}
		if (list.Count > 0)
		{
			ItemKey random = list.GetRandom(context.Random);
			bool flag = AddRandomUnlockValue(context, ItemTemplateHelper.GetGrade(random.ItemType, random.TemplateId));
			if (AddOrReduceDurability(context) || flag)
			{
				ChangeDurability(context, base.CombatChar, random, -1);
			}
		}
		ObjectPool<List<ItemKey>>.Instance.Return(list);
	}

	private bool AddOrReduceDurability(DataContext context)
	{
		CombatCharacter combatCharacter = (base.IsDirect ? base.CombatChar : base.CurrEnemyChar);
		ItemKey[] equipment = combatCharacter.GetCharacter().GetEquipment();
		List<ItemKey> list = ObjectPool<List<ItemKey>>.Instance.Get();
		foreach (sbyte targetEquipmentSlot in TargetEquipmentSlots)
		{
			ItemKey itemKey = equipment[targetEquipmentSlot];
			if (itemKey.IsValid())
			{
				ItemBase baseItem = DomainManager.Item.GetBaseItem(itemKey);
				if (base.IsDirect ? (baseItem.GetCurrDurability() < baseItem.GetMaxDurability()) : (baseItem.GetCurrDurability() > 0))
				{
					list.Add(itemKey);
				}
			}
		}
		bool flag = list.Count > 0;
		if (flag)
		{
			ItemKey random = list.GetRandom(context.Random);
			int delta = 3 * (base.IsDirect ? 1 : (-1));
			ChangeDurability(context, combatCharacter, random, delta);
			ShowSpecialEffectTips(1);
		}
		ObjectPool<List<ItemKey>>.Instance.Return(list);
		return flag;
	}

	private bool AddRandomUnlockValue(DataContext context, sbyte grade)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		int delta = GlobalConfig.Instance.UnlockAttackUnit * CValuePercent.op_Implicit(grade + 1);
		List<int> list = ObjectPool<List<int>>.Instance.Get();
		list.Clear();
		List<int> unlockPrepareValue = base.CombatChar.GetUnlockPrepareValue();
		for (int i = 0; i < 3; i++)
		{
			if (unlockPrepareValue[i] < GlobalConfig.Instance.UnlockAttackUnit && base.CombatChar.CanUnlockAttackByConfig(i))
			{
				list.Add(i);
			}
		}
		bool flag = list.Count > 0;
		if (flag)
		{
			int random = list.GetRandom(context.Random);
			base.CombatChar.ChangeUnlockAttackValue(context, random, delta);
			ShowSpecialEffectTips(0);
		}
		ObjectPool<List<int>>.Instance.Return(list);
		return flag;
	}
}
