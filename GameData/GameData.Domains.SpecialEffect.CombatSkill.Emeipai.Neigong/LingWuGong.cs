using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Neigong;

namespace GameData.Domains.SpecialEffect.CombatSkill.Emeipai.Neigong;

public class LingWuGong : ReduceMainAttributeCharacter
{
	private const int NeedTrickCount = 5;

	private const int UnitValue = 3;

	private int MaxAddValue => base.IsDirect ? 50 : 100;

	protected override sbyte MainAttributeType => 1;

	protected override bool DirectIsAlly => false;

	protected override IReadOnlyList<ushort> FieldIds { get; } = new ushort[2] { 316, 300 };

	protected override EDataModifyType ModifyType => (EDataModifyType)1;

	protected override int CurrAddValue => Math.Min(base.CurrMainAttribute / 3, MaxAddValue) * ((!base.IsDirect) ? 1 : (-1));

	protected override bool IsAffect => (base.IsDirect ? base.CombatChar : base.EnemyChar).UsableTrickCount >= 5;

	public LingWuGong()
	{
	}

	public LingWuGong(CombatSkillKey skillKey)
		: base(skillKey, 2003)
	{
	}

	protected override bool IsTargetMatchAffect(CombatCharacter target)
	{
		target = (base.IsDirect ? base.CombatChar : base.EnemyChar);
		return target.UsableTrickCount >= 5;
	}

	protected override IEnumerable<DataUid> GetUpdateAffectingDataUids()
	{
		yield return ParseCombatCharacterDataUid(28);
		int[] characterList = DomainManager.Combat.GetCharacterList(!base.CombatChar.IsAlly);
		foreach (int charId in characterList)
		{
			if (charId >= 0)
			{
				yield return ParseCombatCharacterDataUid(charId, 28);
			}
		}
	}

	protected override void OnAffected()
	{
		ShowSpecialEffectTips(0);
	}
}
