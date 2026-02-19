using System;
using GameData.Combat.Math;
using GameData.Common;

namespace GameData.Domains.SpecialEffect.CombatSkill.Xuehoujiao.BreakBodyEffect;

public class HandBreakBase : BreakBodyEffectBase
{
	private const sbyte MaxTrickCount = 6;

	protected HandBreakBase()
	{
	}

	protected HandBreakBase(int charId, int type)
		: base(charId, type)
	{
		AffectBodyParts = new sbyte[2] { 3, 4 };
	}

	public override void OnEnable(DataContext context)
	{
		base.OnEnable(context);
		CreateAffectedData(170, (EDataModifyType)3, -1);
	}

	public override void OnDataAdded(DataContext context)
	{
		if (DomainManager.Combat.IsCharInCombat(base.CharacterId) && base.CombatChar.GetTricks().Tricks.Count > base.CombatChar.GetMaxTrickCount())
		{
			DomainManager.Combat.RemoveOverflowTrick(context, base.CombatChar, updateFieldAndSkill: true);
		}
	}

	public override int GetModifiedValue(AffectedDataKey dataKey, int dataValue)
	{
		if (dataKey.CharId != base.CharacterId)
		{
			return dataValue;
		}
		if (dataKey.FieldId == 170)
		{
			return Math.Min(6, dataValue);
		}
		return base.GetModifiedValue(dataKey, dataValue);
	}
}
