using System.Collections.Generic;
using System.Linq;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.Item;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.Zhujianshanzhuang.AttackCommon;

public abstract class BladeUnlockEffectBase : WeaponUnlockEffectBase, IExtraUnlockEffect
{
	protected override short WeaponType => 9;

	protected override CValuePercent DirectAddUnlockPercent => CValuePercent.op_Implicit(24);

	protected override bool ReverseEffectDoubling
	{
		get
		{
			List<int> list = ObjectPool<List<int>>.Instance.Get();
			DomainManager.Combat.GetAllCharInCombat(list);
			bool result = list.Any(CharHasRequireWeapon);
			ObjectPool<List<int>>.Instance.Return(list);
			return result;
		}
	}

	protected abstract IEnumerable<short> RequireWeaponTypes { get; }

	protected BladeUnlockEffectBase()
	{
	}

	protected BladeUnlockEffectBase(CombatSkillKey skillKey, int type)
		: base(skillKey, type)
	{
	}

	private bool IsWeaponMatchRequire(ItemKey weaponKey)
	{
		return RequireWeaponTypes.Any((short x) => x == ItemTemplateHelper.GetItemSubType(weaponKey.ItemType, weaponKey.TemplateId));
	}

	private bool CharHasRequireWeapon(int charId)
	{
		CombatCharacter element_CombatCharacterDict = DomainManager.Combat.GetElement_CombatCharacterDict(charId);
		ItemKey[] weapons = element_CombatCharacterDict.GetWeapons();
		for (int i = 0; i < 3; i++)
		{
			if (weapons[i].IsValid() && IsWeaponMatchRequire(weapons[i]))
			{
				return true;
			}
		}
		return false;
	}

	protected virtual bool CanDoAffect()
	{
		return true;
	}

	protected sealed override void DoAffect(DataContext context, int weaponIndex)
	{
		if (CanDoAffect())
		{
			base.CombatChar.InvokeExtraUnlockEffect(this, weaponIndex);
		}
	}

	public abstract void DoAffectAfterCost(DataContext context, int weaponIndex);
}
