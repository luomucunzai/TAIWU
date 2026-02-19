using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Agile;

namespace GameData.Domains.SpecialEffect.CombatSkill.Wuxianjiao.Agile;

public class ChuangNiTan : AgileSkillBase
{
	private bool _affecting;

	public ChuangNiTan()
	{
	}

	public ChuangNiTan(CombatSkillKey skillKey)
		: base(skillKey, 12601)
	{
	}

	public override void OnEnable(DataContext context)
	{
		base.OnEnable(context);
		_affecting = base.CanAffect;
		AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
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
		if (dataKey.FieldId == 55 && dataKey.CustomParam0 == (base.IsDirect ? 1 : 0))
		{
			return false;
		}
		return dataValue;
	}
}
