using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.Character;
using GameData.Domains.CombatSkill;
using GameData.Domains.Item;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.Kongsangpai.Neigong;

public class ChaiShanQingNangJue : CombatSkillEffectBase
{
	private const sbyte MaxItemCount = 5;

	private int _damageChangePercent;

	public ChaiShanQingNangJue()
	{
	}

	public ChaiShanQingNangJue(CombatSkillKey skillKey)
		: base(skillKey, 10002, -1)
	{
	}

	public unsafe override void OnEnable(DataContext context)
	{
		AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
		AffectDatas.Add(new AffectedDataKey(base.CharacterId, (ushort)(base.IsDirect ? 102 : 69), -1), (EDataModifyType)2);
		ref EatingItems eatingItems = ref CharObj.GetEatingItems();
		List<sbyte> list = ObjectPool<List<sbyte>>.Instance.Get();
		_damageChangePercent = 0;
		list.Clear();
		for (int i = 0; i < 9; i++)
		{
			ItemKey itemKey = (ItemKey)eatingItems.ItemKeys[i];
			if (itemKey.IsValid() && ItemTemplateHelper.GetItemSubType(itemKey.ItemType, itemKey.TemplateId) == 800)
			{
				list.Add(ItemTemplateHelper.GetGrade(itemKey.ItemType, itemKey.TemplateId));
			}
		}
		list.Sort();
		int num = Math.Min(list.Count, 5);
		for (int j = list.Count - num; j < list.Count; j++)
		{
			_damageChangePercent = _damageChangePercent + list[j] + 1;
		}
	}

	public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
	{
		if (dataKey.CharId != base.CharacterId)
		{
			return 0;
		}
		if (dataKey.FieldId == 69)
		{
			return _damageChangePercent;
		}
		if (dataKey.FieldId == 102)
		{
			return -_damageChangePercent;
		}
		return 0;
	}
}
