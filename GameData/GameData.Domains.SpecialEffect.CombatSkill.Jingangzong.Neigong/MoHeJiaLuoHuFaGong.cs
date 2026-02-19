using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Jingangzong.Neigong;

public class MoHeJiaLuoHuFaGong : CombatSkillEffectBase
{
	private const sbyte DirectChangePercent = 60;

	private const sbyte ReverseChangePercent = -30;

	public MoHeJiaLuoHuFaGong()
	{
	}

	public MoHeJiaLuoHuFaGong(CombatSkillKey skillKey)
		: base(skillKey, 11007, -1)
	{
	}

	public override void OnEnable(DataContext context)
	{
		AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
		if (base.IsDirect)
		{
			AffectDatas.Add(new AffectedDataKey(base.CharacterId, 135, -1), (EDataModifyType)1);
			AffectDatas.Add(new AffectedDataKey(base.CharacterId, 126, -1), (EDataModifyType)3);
		}
		else
		{
			AffectDatas.Add(new AffectedDataKey(base.CharacterId, 136, -1), (EDataModifyType)1);
			AffectDatas.Add(new AffectedDataKey(base.CharacterId, 131, -1), (EDataModifyType)3);
		}
	}

	public override bool GetModifiedValue(AffectedDataKey dataKey, bool dataValue)
	{
		if (dataKey.CharId != base.CharacterId)
		{
			return dataValue;
		}
		ushort fieldId = dataKey.FieldId;
		if ((fieldId == 126 || fieldId == 131) ? true : false)
		{
			bool flag = CanAffecting();
			if (flag)
			{
				ShowSpecialEffectTipsOnceInFrame(0);
			}
			return !flag;
		}
		return dataValue;
	}

	public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
	{
		if (dataKey.CharId != base.CharacterId || !CanAffecting())
		{
			return 0;
		}
		if (dataKey.FieldId == 135)
		{
			return 60;
		}
		if (dataKey.FieldId == 136)
		{
			return -30;
		}
		return 0;
	}

	private bool CanAffecting()
	{
		int totalCount = base.CombatChar.GetDefeatMarkCollection().GetTotalCount();
		int num = GlobalConfig.NeedDefeatMarkCount[DomainManager.Combat.GetCombatType()] / 2;
		return totalCount > num;
	}
}
