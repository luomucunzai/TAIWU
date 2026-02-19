using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.Item;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.Common.Attack;

public abstract class WeaponAddAttackPrepareValue : CombatSkillEffectBase
{
	private const int ChangeWeaponCdCount = 2;

	private const int FreeNormalAttackCount = 1;

	protected abstract int RequireWeaponSubType { get; }

	protected abstract int DirectSrcWeaponSubType { get; }

	private static short GetItemSubType(ItemKey itemKey)
	{
		return ItemTemplateHelper.GetItemSubType(itemKey.ItemType, itemKey.TemplateId);
	}

	protected WeaponAddAttackPrepareValue()
	{
	}

	protected WeaponAddAttackPrepareValue(CombatSkillKey skillKey, int type)
		: base(skillKey, type, -1)
	{
	}

	public override void OnEnable(DataContext context)
	{
		Events.RegisterHandler_CastSkillEnd(OnCastSkillEnd);
	}

	public override void OnDisable(DataContext context)
	{
		Events.UnRegisterHandler_CastSkillEnd(OnCastSkillEnd);
	}

	private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
	{
		if (charId == base.CharacterId && skillId == base.SkillTemplateId)
		{
			if (PowerMatchAffectRequire(power))
			{
				DoChangeWeaponCd(context);
				DoChangeWeapon(context);
			}
			RemoveSelf(context);
		}
	}

	private void DoChangeWeaponCd(DataContext context)
	{
		//IL_0100: Unknown result type (might be due to invalid IL or missing references)
		CombatCharacter combatCharacter = (base.IsDirect ? base.CombatChar : DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly));
		ItemKey[] weapons = combatCharacter.GetWeapons();
		int usingWeaponIndex = combatCharacter.GetUsingWeaponIndex();
		List<int> list = ObjectPool<List<int>>.Instance.Get();
		list.Clear();
		for (int i = 0; i < 3; i++)
		{
			if (i == usingWeaponIndex)
			{
				continue;
			}
			ItemKey itemKey = weapons[i];
			if (itemKey.IsValid())
			{
				CombatWeaponData weaponData = combatCharacter.GetWeaponData(i);
				if (weaponData.GetDurability() > 0 && (!base.IsDirect || weaponData.GetCdFrame() > 0))
				{
					list.Add(i);
				}
			}
		}
		foreach (int item in RandomUtils.GetRandomUnrepeated(context.Random, 2, list))
		{
			DomainManager.Combat.ChangeWeaponCd(context, combatCharacter, item, CValuePercent.op_Implicit(base.IsDirect ? (-50) : 50));
			ShowSpecialEffectTipsOnceInFrame(0);
		}
		ObjectPool<List<int>>.Instance.Return(list);
	}

	private void DoChangeWeapon(DataContext context)
	{
		int usingWeaponIndex = base.CombatChar.GetUsingWeaponIndex();
		ItemKey[] weapons = base.CombatChar.GetWeapons();
		ItemKey itemKey = weapons[usingWeaponIndex];
		bool flag = itemKey.IsValid() && GetItemSubType(itemKey) == (base.IsDirect ? DirectSrcWeaponSubType : RequireWeaponSubType);
		List<int> list = ObjectPool<List<int>>.Instance.Get();
		list.Clear();
		if (flag && base.IsDirect)
		{
			bool flag2 = false;
			for (int i = 0; i < 3; i++)
			{
				if (i == usingWeaponIndex)
				{
					continue;
				}
				ItemKey itemKey2 = weapons[i];
				if (itemKey2.IsValid() && GetItemSubType(itemKey2) == RequireWeaponSubType)
				{
					CombatWeaponData element_WeaponDataDict = DomainManager.Combat.GetElement_WeaponDataDict(itemKey2.Id);
					if (element_WeaponDataDict.GetDurability() > 0 && element_WeaponDataDict.NotInAnyCd)
					{
						flag2 = true;
						list.Add(i);
					}
				}
			}
			flag = flag2;
		}
		if (flag)
		{
			if (base.IsDirect && list.Count > 0)
			{
				DomainManager.Combat.ChangeWeapon(context, list[context.Random.Next(0, list.Count)], base.CombatChar.IsAlly, forceChange: true);
			}
			if (!base.IsDirect && DomainManager.Combat.InAttackRange(base.CombatChar))
			{
				base.CombatChar.NeedNormalAttackSkipPrepare++;
				ShowSpecialEffectTips(1);
			}
		}
		ObjectPool<List<int>>.Instance.Return(list);
	}
}
