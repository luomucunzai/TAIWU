using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.CombatSkill;
using GameData.Domains.Item;

namespace GameData.Domains.SpecialEffect.CombatSkill.Zhujianshanzhuang.AttackCommon;

public abstract class RawCreateUnlockEffectBase : PolearmUnlockEffectBase
{
	protected RawCreateUnlockEffectBase()
	{
	}

	protected RawCreateUnlockEffectBase(CombatSkillKey skillKey, int type)
		: base(skillKey, type)
	{
	}

	public override void OnEnable(DataContext context)
	{
		base.OnEnable(context);
		CreateAffectedData(312, (EDataModifyType)3, -1);
	}

	protected override void DoAffect(DataContext context, int weaponIndex)
	{
		base.CombatChar.InvokeRawCreate(context, base.EffectId);
	}

	public override List<int> GetModifiedValue(AffectedDataKey dataKey, List<int> dataValue)
	{
		if (dataKey.CharId != base.CharacterId || dataKey.FieldId != 312)
		{
			return dataValue;
		}
		int customParam = dataKey.CustomParam0;
		ItemKey weaponKey = base.CombatChar.GetWeapons()[customParam];
		bool num;
		if (!base.IsDirect)
		{
			if (!IsMatchWeaponType(weaponKey))
			{
				goto IL_0079;
			}
			num = ReverseEffectDoubling;
		}
		else
		{
			num = IsDirectWeapon(weaponKey);
		}
		if (num)
		{
			dataValue.Add(base.EffectId);
		}
		goto IL_0079;
		IL_0079:
		return dataValue;
	}
}
