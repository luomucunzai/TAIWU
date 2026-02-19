using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.Animal.Loong.Neigong;

public class Chiwen : AnimalEffectBase
{
	private const int AddOrCostBaseValue = 3;

	private static readonly CValuePercent AddOrCostPercent = CValuePercent.op_Implicit(25);

	private int _damageAddPercent;

	public Chiwen()
	{
	}

	public Chiwen(CombatSkillKey skillKey)
		: base(skillKey)
	{
	}

	public override void OnEnable(DataContext context)
	{
		base.OnEnable(context);
		CreateAffectedData(69, (EDataModifyType)1, -1);
		Events.RegisterHandler_NormalAttackCalcHitEnd(OnNormalAttackCalcHitEnd);
		Events.RegisterHandler_NormalAttackAllEnd(OnNormalAttackAllEnd);
	}

	public override void OnDisable(DataContext context)
	{
		Events.UnRegisterHandler_NormalAttackCalcHitEnd(OnNormalAttackCalcHitEnd);
		Events.UnRegisterHandler_NormalAttackAllEnd(OnNormalAttackAllEnd);
		base.OnDisable(context);
	}

	private void OnNormalAttackCalcHitEnd(DataContext context, CombatCharacter attacker, CombatCharacter defender, int pursueIndex, bool hit, bool isFightback, bool isMind)
	{
		if (attacker.GetId() == base.CharacterId && hit && pursueIndex <= 0)
		{
			TryChangeNeiliAllocation(context, attacker, buff: true);
			TryChangeNeiliAllocation(context, defender, buff: false);
		}
	}

	private void OnNormalAttackAllEnd(DataContext context, CombatCharacter attacker, CombatCharacter defender)
	{
		if (attacker.GetId() == base.CharacterId)
		{
			_damageAddPercent = 0;
		}
	}

	private void TryChangeNeiliAllocation(DataContext context, CombatCharacter targetChar, bool buff)
	{
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		NeiliAllocation neiliAllocation = targetChar.GetNeiliAllocation();
		NeiliAllocation originNeiliAllocation = targetChar.GetOriginNeiliAllocation();
		List<byte> list = ObjectPool<List<byte>>.Instance.Get();
		for (byte b = 0; b < 4; b++)
		{
			if (!(buff ? (neiliAllocation[b] >= originNeiliAllocation[b]) : (neiliAllocation[b] <= originNeiliAllocation[b])))
			{
				list.Add(b);
			}
		}
		if (list.Count > 0)
		{
			byte random = list.GetRandom(context.Random);
			int addValue = (3 + (int)neiliAllocation[random] * AddOrCostPercent) * (buff ? 1 : (-1));
			addValue = targetChar.ChangeNeiliAllocation(context, random, addValue, applySpecialEffect: true, raiseEvent: false);
			_damageAddPercent += Math.Abs(addValue);
			ShowSpecialEffectTips(buff, 1, 0);
		}
		ObjectPool<List<byte>>.Instance.Return(list);
	}

	public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
	{
		if (dataKey.CharId != base.CharacterId || dataKey.FieldId != 69 || _damageAddPercent <= 0)
		{
			return 0;
		}
		ShowSpecialEffectTipsOnceInFrame(2);
		return _damageAddPercent;
	}
}
