using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;

namespace GameData.Domains.SpecialEffect.CombatSkill.Jingangzong.PestleEffect;

public class NuMuJinGangChu : PestleEffectBase
{
	public NuMuJinGangChu()
	{
	}

	public NuMuJinGangChu(int charId)
		: base(charId, 11403)
	{
	}

	public override void OnEnable(DataContext context)
	{
		AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
		int[] characterList = DomainManager.Combat.GetCharacterList(!base.CombatChar.IsAlly);
		for (int i = 0; i < characterList.Length; i++)
		{
			if (characterList[i] >= 0)
			{
				AffectDatas.Add(new AffectedDataKey(characterList[i], 116, -1), (EDataModifyType)0);
			}
		}
		base.OnEnable(context);
	}

	public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
	{
		if (!base.CanAffect || !DomainManager.Combat.IsCurrentCombatCharacter(base.CombatChar) || base.CombatChar.TeammateBeforeMainChar >= 0)
		{
			return 0;
		}
		if (dataKey.FieldId == 116 && dataKey.CustomParam1 == ((!base.IsDirect) ? 1 : 0))
		{
			return 1;
		}
		return 0;
	}
}
