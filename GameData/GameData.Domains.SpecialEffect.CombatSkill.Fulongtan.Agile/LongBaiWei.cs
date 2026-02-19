using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Agile;

namespace GameData.Domains.SpecialEffect.CombatSkill.Fulongtan.Agile;

public class LongBaiWei : AgileSkillBase
{
	public LongBaiWei()
	{
	}

	public LongBaiWei(CombatSkillKey skillKey)
		: base(skillKey, 14402)
	{
	}

	public override void OnEnable(DataContext context)
	{
		base.OnEnable(context);
		AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
		if (base.IsDirect)
		{
			int[] characterList = DomainManager.Combat.GetCharacterList(!base.CombatChar.IsAlly);
			for (int i = 0; i < characterList.Length; i++)
			{
				if (characterList[i] >= 0)
				{
					AffectDatas.Add(new AffectedDataKey(characterList[i], 127, -1), (EDataModifyType)0);
				}
			}
		}
		else
		{
			AffectDatas.Add(new AffectedDataKey(base.CharacterId, 127, -1), (EDataModifyType)0);
		}
		ShowSpecialEffectTips(0);
	}

	public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
	{
		if (!base.CanAffect)
		{
			return 0;
		}
		if (dataKey.FieldId == 127)
		{
			return base.IsDirect ? 1 : (-1);
		}
		return 0;
	}
}
