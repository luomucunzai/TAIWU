using System;
using System.Collections.Generic;
using Config;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.Item;

namespace GameData.Domains.SpecialEffect.LegendaryBook.NpcEffect;

public class ControllableShot : FeatureEffectBase
{
	private const short AddDamageUnit = 15;

	private const short MaxAddDamage = 180;

	private const short AddPursueOddsPercent = 100;

	public ControllableShot()
	{
	}

	public ControllableShot(int charId, short featureId)
		: base(charId, featureId, 41412)
	{
	}

	public override void OnEnable(DataContext context)
	{
		AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
		AffectDatas.Add(new AffectedDataKey(base.CharacterId, 69, -1), (EDataModifyType)1);
		AffectDatas.Add(new AffectedDataKey(base.CharacterId, 76, -1), (EDataModifyType)2);
	}

	public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
	{
		if (dataKey.CharId != base.CharacterId)
		{
			return 0;
		}
		if (dataKey.FieldId == 69)
		{
			if (base.CombatChar.GetAutoCastingSkill() || dataKey.CombatSkillId < 0 || Config.CombatSkill.Instance[dataKey.CombatSkillId].Type != 12)
			{
				return 0;
			}
			int val = 15 * base.CombatChar.GetChangeTrickCount();
			return Math.Min(val, 180);
		}
		if (dataKey.FieldId == 76)
		{
			ItemKey usingWeaponKey = DomainManager.Combat.GetUsingWeaponKey(base.CombatChar);
			short itemSubType = ItemTemplateHelper.GetItemSubType(usingWeaponKey.ItemType, usingWeaponKey.TemplateId);
			if (!base.CombatChar.GetChangeTrickAttack() || !CombatSkillType.Instance[(sbyte)12].LegendaryBookWeaponSlotItemSubTypes.Contains(itemSubType))
			{
				return 0;
			}
			return 100;
		}
		return 0;
	}
}
