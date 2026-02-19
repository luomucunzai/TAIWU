using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Defense;

namespace GameData.Domains.SpecialEffect.CombatSkill.Yuanshanpai.DefenseAndAssist;

public class TaiChuGuiCangJue : DefenseSkillBase
{
	public TaiChuGuiCangJue()
	{
	}

	public TaiChuGuiCangJue(CombatSkillKey skillKey)
		: base(skillKey, 5506)
	{
	}

	public override void OnEnable(DataContext context)
	{
		base.OnEnable(context);
		AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
		if (base.IsDirect)
		{
			AffectDatas.Add(new AffectedDataKey(base.CharacterId, 137, -1), (EDataModifyType)3);
			AffectDatas.Add(new AffectedDataKey(base.CharacterId, 136, -1), (EDataModifyType)3);
			return;
		}
		int[] characterList = DomainManager.Combat.GetCharacterList(!base.CombatChar.IsAlly);
		for (int i = 0; i < characterList.Length; i++)
		{
			if (characterList[i] >= 0)
			{
				AffectDatas.Add(new AffectedDataKey(characterList[i], 137, -1), (EDataModifyType)3);
				AffectDatas.Add(new AffectedDataKey(characterList[i], 135, -1), (EDataModifyType)3);
			}
		}
	}

	public override bool GetModifiedValue(AffectedDataKey dataKey, bool dataValue)
	{
		if (!base.CanAffect)
		{
			return dataValue;
		}
		if (dataKey.FieldId == 137 && dataKey.CustomParam0 == (base.IsDirect ? 1 : 0))
		{
			return false;
		}
		return dataValue;
	}

	public override int GetModifiedValue(AffectedDataKey dataKey, int dataValue)
	{
		if (!base.CanAffect && !(base.IsDirect ? (dataValue < 0) : (dataValue > 0)))
		{
			return dataValue;
		}
		bool flag = DomainManager.SpecialEffect.ModifyData(dataKey.CharId, -1, 137, dataValue: true, (!base.IsDirect) ? 1 : 0);
		ShowSpecialEffectTips(flag, 0, 1);
		return flag ? (-dataValue * 2) : 0;
	}
}
