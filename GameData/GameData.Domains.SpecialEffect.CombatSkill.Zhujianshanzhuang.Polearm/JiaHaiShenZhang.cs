using System;
using System.Collections.Generic;
using Config;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.CombatSkill;
using GameData.Domains.Item;
using GameData.Domains.SpecialEffect.CombatSkill.Zhujianshanzhuang.AttackCommon;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.Zhujianshanzhuang.Polearm;

public class JiaHaiShenZhang : PolearmUnlockEffectBase
{
	private readonly List<ItemKey> _addedEffectKeys = new List<ItemKey>();

	private static CValuePercent AddDurabilityPercent => CValuePercent.op_Implicit(50);

	private CValuePercent CastAddUnlockPercent => CValuePercent.op_Implicit(base.IsDirectOrReverseEffectDoubling ? 16 : 8);

	protected override IEnumerable<sbyte> RequireMainAttributeTypes { get; } = new sbyte[3] { 3, 4, 2 };

	protected override int RequireMainAttributeValue => 55;

	public JiaHaiShenZhang()
	{
	}

	public JiaHaiShenZhang(CombatSkillKey skillKey)
		: base(skillKey, 9306)
	{
	}

	public override void OnEnable(DataContext context)
	{
		base.OnEnable(context);
		Events.RegisterHandler_CombatSettlement(OnCombatSettlement);
	}

	public override void OnDisable(DataContext context)
	{
		Events.UnRegisterHandler_CombatSettlement(OnCombatSettlement);
		base.OnDisable(context);
	}

	protected override void OnCastAddUnlockAttackValue(DataContext context, CValuePercent power)
	{
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		base.OnCastAddUnlockAttackValue(context, power);
		if (base.IsReverseOrUsingDirectWeapon)
		{
			base.CombatChar.ChangeAllUnlockAttackValue(context, CastAddUnlockPercent * power);
			ShowSpecialEffectTipsOnceInFrame(0);
		}
	}

	protected override void DoAffect(DataContext context, int weaponIndex)
	{
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		ItemKey[] equipment = base.CombatChar.GetCharacter().GetEquipment();
		for (int i = 0; i < equipment.Length; i++)
		{
			ItemKey itemKey = equipment[i];
			if (itemKey.IsValid() && DomainManager.Combat.EquipmentOldDurability.TryGetValue(itemKey, out var value))
			{
				short currDurability = DomainManager.Item.GetBaseItem(itemKey).GetCurrDurability();
				if (currDurability < value)
				{
					int delta = Math.Max((value - currDurability) * AddDurabilityPercent, 1);
					ChangeDurability(context, base.CombatChar, itemKey, delta);
					ShowSpecialEffectTipsOnceInFrame(1);
				}
			}
		}
		List<ItemKey> list = ObjectPool<List<ItemKey>>.Instance.Get();
		ItemKey[] equipment2 = base.EnemyChar.GetCharacter().GetEquipment();
		foreach (ItemKey itemKey2 in equipment2)
		{
			if (!list.Contains(itemKey2) && IsKeyCanCrippledCreate(itemKey2))
			{
				list.Add(itemKey2);
			}
		}
		if (list.Count > 0)
		{
			ItemKey random = list.GetRandom(context.Random);
			SpecialEffectItem specialEffectItem = Config.SpecialEffect.Instance[base.EffectId];
			short rawCreateEffect = specialEffectItem.RawCreateEffect;
			DomainManager.Item.AddExternEquipmentEffect(context, random, rawCreateEffect);
			DomainManager.SpecialEffect.AddEquipmentEffect(context, base.EnemyChar.GetId(), random, rawCreateEffect);
			base.EnemyChar.GetCharacter().SetEquipment(base.EnemyChar.GetCharacter().GetEquipment(), context);
			_addedEffectKeys.Add(random);
			DomainManager.Combat.ShowSpecialEffectTips(base.CharacterId, specialEffectItem.RawCreateTips, 0);
		}
		ObjectPool<List<ItemKey>>.Instance.Return(list);
	}

	private void OnCombatSettlement(DataContext context, sbyte combatStatus)
	{
		short rawCreateEffect = Config.SpecialEffect.Instance[base.EffectId].RawCreateEffect;
		foreach (ItemKey addedEffectKey in _addedEffectKeys)
		{
			DomainManager.Item.RemoveExternEquipmentEffect(context, addedEffectKey, rawCreateEffect);
		}
		_addedEffectKeys.Clear();
	}

	private bool IsKeyCanCrippledCreate(ItemKey key)
	{
		if (!key.IsValid() || _addedEffectKeys.Contains(key))
		{
			return false;
		}
		sbyte itemType = key.ItemType;
		if (1 == 0)
		{
		}
		bool result = itemType switch
		{
			0 => Config.Weapon.Instance[key.TemplateId].AllowCrippledCreate, 
			1 => Config.Armor.Instance[key.TemplateId].AllowCrippledCreate, 
			_ => false, 
		};
		if (1 == 0)
		{
		}
		return result;
	}
}
