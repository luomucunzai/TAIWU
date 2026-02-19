using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Agile;

namespace GameData.Domains.SpecialEffect.CombatSkill.Shaolinpai.Agile;

public class YiWeiDuJiang : AgileSkillBase
{
	private bool _affecting;

	public YiWeiDuJiang()
	{
	}

	public YiWeiDuJiang(CombatSkillKey skillKey)
		: base(skillKey, 1405)
	{
		ListenCanAffectChange = true;
	}

	public override void OnEnable(DataContext context)
	{
		base.OnEnable(context);
		_affecting = false;
		OnMoveSkillCanAffectChanged(context, default(DataUid));
		AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
		AffectDatas.Add(new AffectedDataKey(base.CharacterId, 149, -1), (EDataModifyType)3);
		AffectDatas.Add(new AffectedDataKey(base.CharacterId, 147, -1), (EDataModifyType)3);
		if (base.IsDirect)
		{
			AffectDatas.Add(new AffectedDataKey(base.CharacterId, 55, -1), (EDataModifyType)3);
		}
		else
		{
			int[] characterList = DomainManager.Combat.GetCharacterList(!base.CombatChar.IsAlly);
			for (int i = 0; i < characterList.Length; i++)
			{
				if (characterList[i] >= 0)
				{
					AffectDatas.Add(new AffectedDataKey(characterList[i], 55, -1), (EDataModifyType)3);
				}
			}
		}
		ShowSpecialEffectTips(0);
	}

	protected override void OnMoveSkillCanAffectChanged(DataContext context, DataUid dataUid)
	{
		bool canAffect = base.CanAffect;
		if (_affecting == canAffect)
		{
			return;
		}
		_affecting = canAffect;
		if (base.IsDirect)
		{
			DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 55);
			return;
		}
		int[] characterList = DomainManager.Combat.GetCharacterList(!base.CombatChar.IsAlly);
		for (int i = 0; i < characterList.Length; i++)
		{
			if (characterList[i] >= 0)
			{
				DomainManager.SpecialEffect.InvalidateCache(context, characterList[i], 55);
			}
		}
	}

	public override bool GetModifiedValue(AffectedDataKey dataKey, bool dataValue)
	{
		if (!_affecting)
		{
			return dataValue;
		}
		if ((dataKey.FieldId == 149 && dataKey.CustomParam0 >= 0 && DomainManager.Combat.GetElement_CombatCharacterDict(dataKey.CustomParam0).IsAlly != base.CombatChar.IsAlly) || dataKey.FieldId == 147)
		{
			return false;
		}
		if (dataKey.FieldId == 55 && dataKey.CustomParam0 == (base.IsDirect ? 1 : 0))
		{
			return false;
		}
		return dataValue;
	}
}
