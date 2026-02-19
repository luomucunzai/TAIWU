using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Kongsangpai.Neigong;

public class JiuDingGong : CombatSkillEffectBase
{
	private const sbyte AddPowerRatio = 40;

	private const sbyte MaxAddPower = 20;

	public JiuDingGong()
	{
	}

	public JiuDingGong(CombatSkillKey skillKey)
		: base(skillKey, 10004, -1)
	{
	}

	public override void OnEnable(DataContext context)
	{
		CreateAffectedData(199, (EDataModifyType)0, -1);
	}

	public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
	{
		if (dataKey.CharId != base.CharacterId || dataKey.FieldId != 199)
		{
			return 0;
		}
		int num = CValuePercent.ParseInt((int)CharObj.GetHealth(), (int)CharObj.GetMaxHealth());
		int value = (base.IsDirect ? (num * 40 / 100) : ((100 - num) * 40 / 100));
		return Math.Clamp(value, 0, 20);
	}
}
