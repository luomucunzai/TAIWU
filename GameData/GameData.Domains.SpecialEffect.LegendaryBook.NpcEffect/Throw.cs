using System;
using System.Collections.Generic;
using Config;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.Item;

namespace GameData.Domains.SpecialEffect.LegendaryBook.NpcEffect;

public class Throw : FeatureEffectBase
{
	private const short MinDistance = 50;

	private const short BaseAddDamage = 40;

	private const short AddDamageUnit = 20;

	private const short MaxAddDamage = 180;

	public Throw()
	{
	}

	public Throw(int charId, short featureId)
		: base(charId, featureId, 41406)
	{
	}

	public override void OnEnable(DataContext context)
	{
		AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
		AffectDatas.Add(new AffectedDataKey(base.CharacterId, 69, -1), (EDataModifyType)1);
		AffectDatas.Add(new AffectedDataKey(base.CharacterId, 281, -1), (EDataModifyType)3);
	}

	public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
	{
		if (dataKey.CharId != base.CharacterId || dataKey.CombatSkillId < 0 || base.CombatChar.GetAutoCastingSkill() || Config.CombatSkill.Instance[dataKey.CombatSkillId].Type != 6)
		{
			return 0;
		}
		if (dataKey.FieldId == 69)
		{
			short currentDistance = DomainManager.Combat.GetCurrentDistance();
			int val = ((currentDistance >= 50) ? (40 + (currentDistance - 50) / 10 * 20) : 0);
			return Math.Min(val, 180);
		}
		return 0;
	}

	public override bool GetModifiedValue(AffectedDataKey dataKey, bool dataValue)
	{
		if (dataKey.CharId != base.CharacterId)
		{
			return dataValue;
		}
		ItemKey usingWeaponKey = DomainManager.Combat.GetUsingWeaponKey(base.CombatChar);
		short itemSubType = ItemTemplateHelper.GetItemSubType(usingWeaponKey.ItemType, usingWeaponKey.TemplateId);
		if (!CombatSkillType.Instance[(sbyte)6].LegendaryBookWeaponSlotItemSubTypes.Contains(itemSubType))
		{
			return dataValue;
		}
		if (dataKey.FieldId == 281)
		{
			return true;
		}
		return dataValue;
	}
}
