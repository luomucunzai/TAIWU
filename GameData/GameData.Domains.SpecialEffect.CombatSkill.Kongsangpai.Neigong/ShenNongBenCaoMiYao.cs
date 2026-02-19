using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Kongsangpai.Neigong;

public class ShenNongBenCaoMiYao : CombatSkillEffectBase
{
	private const sbyte SpeedChangePercent = 30;

	private const sbyte HealEffectChange = 30;

	public ShenNongBenCaoMiYao()
	{
	}

	public ShenNongBenCaoMiYao(CombatSkillKey skillKey)
		: base(skillKey, 10001, -1)
	{
	}

	public override void OnEnable(DataContext context)
	{
		AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
		if (base.IsDirect)
		{
			AffectDatas.Add(new AffectedDataKey(base.CharacterId, 118, -1), (EDataModifyType)1);
			AffectDatas.Add(new AffectedDataKey(base.CharacterId, 119, -1), (EDataModifyType)1);
			AffectDatas.Add(new AffectedDataKey(base.CharacterId, 121, -1), (EDataModifyType)1);
			AffectDatas.Add(new AffectedDataKey(base.CharacterId, 122, -1), (EDataModifyType)1);
			return;
		}
		int[] characterList = DomainManager.Combat.GetCharacterList(!base.CombatChar.IsAlly);
		foreach (int num in characterList)
		{
			if (num >= 0)
			{
				AffectDatas.Add(new AffectedDataKey(num, 118, -1), (EDataModifyType)1);
				AffectDatas.Add(new AffectedDataKey(num, 120, -1), (EDataModifyType)1);
				AffectDatas.Add(new AffectedDataKey(num, 121, -1), (EDataModifyType)1);
				AffectDatas.Add(new AffectedDataKey(num, 123, -1), (EDataModifyType)1);
			}
		}
	}

	public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
	{
		if (!base.IsDirect && !base.IsCurrent)
		{
			return 0;
		}
		ushort fieldId = dataKey.FieldId;
		if ((fieldId == 118 || fieldId == 121) ? true : false)
		{
			ShowSpecialEffectTips(dataKey.FieldId == 118, 0, 2);
			return base.IsDirect ? 30 : (-30);
		}
		fieldId = dataKey.FieldId;
		if (((uint)(fieldId - 119) <= 1u || (uint)(fieldId - 122) <= 1u) ? true : false)
		{
			bool condition;
			if (dataKey.CustomParam0 == 0)
			{
				fieldId = dataKey.FieldId;
				condition = (uint)(fieldId - 119) <= 1u;
				ShowSpecialEffectTips(condition, 1, 3);
			}
			fieldId = dataKey.FieldId;
			condition = ((fieldId == 119 || fieldId == 122) ? true : false);
			return condition ? 30 : (-30);
		}
		return 0;
	}
}
