using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Defense;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.Baihuagu.DefenseAndAssist;

public class ShiErJingDaNuoYi : DefenseSkillBase
{
	private const int HealAcupointSpeedAddPercent = 200;

	public ShiErJingDaNuoYi()
	{
	}

	public ShiErJingDaNuoYi(CombatSkillKey skillKey)
		: base(skillKey, 3505)
	{
	}

	public override void OnEnable(DataContext context)
	{
		base.OnEnable(context);
		CreateAffectedData(134, (EDataModifyType)3, -1);
		CreateAffectedData(300, (EDataModifyType)1, -1);
		ShowSpecialEffectTips(1);
	}

	public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
	{
		if (dataKey.CharId != base.CharacterId || dataKey.FieldId != 300 || !base.CanAffect)
		{
			return 0;
		}
		return 200;
	}

	public override int GetModifiedValue(AffectedDataKey dataKey, int dataValue)
	{
		if (dataKey.CharId != base.CharacterId || dataValue <= 0 || !base.CanAffect)
		{
			return dataValue;
		}
		DataContext context = DomainManager.Combat.Context;
		sbyte b = (sbyte)dataKey.CustomParam0;
		CombatCharacter combatCharacter = (base.IsDirect ? base.CombatChar : base.CurrEnemyChar);
		byte[] acupointCount = combatCharacter.GetAcupointCount();
		if (b < 0)
		{
			List<sbyte> list = ObjectPool<List<sbyte>>.Instance.Get();
			list.Clear();
			for (sbyte b2 = 0; b2 < 7; b2++)
			{
				if (!base.IsDirect || acupointCount[b2] > 0)
				{
					list.Add(b2);
				}
			}
			if (list.Count > 0)
			{
				b = list[context.Random.Next(list.Count)];
			}
			ObjectPool<List<sbyte>>.Instance.Return(list);
		}
		if (b < 0)
		{
			return 0;
		}
		if (base.IsDirect)
		{
			if (acupointCount[b] > 0)
			{
				DomainManager.Combat.RemoveAcupoint(context, combatCharacter, b, 0, raiseEvent: false);
				ShowSpecialEffectTips(0);
			}
		}
		else
		{
			DomainManager.Combat.AddAcupoint(context, combatCharacter, (sbyte)dataKey.CustomParam1, new CombatSkillKey(-1, -1), b, 1, raiseEvent: false);
			ShowSpecialEffectTips(0);
		}
		return 0;
	}
}
