using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Defense;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.Ranshanpai.DefenseAndAssist;

public class WanHuaGeGui : DefenseSkillBase
{
	private static readonly CValuePercent ChangeNeiliAllocationPercent = CValuePercent.op_Implicit(10);

	private const int SilenceFrame = 3000;

	public WanHuaGeGui()
	{
	}

	public WanHuaGeGui(CombatSkillKey skillKey)
		: base(skillKey, 7507)
	{
	}

	public override void OnEnable(DataContext context)
	{
		base.OnEnable(context);
		CreateAffectedData(291, (EDataModifyType)3, -1);
		Events.RegisterHandler_NormalAttackEnd(OnNormalAttackEnd);
	}

	public override void OnDisable(DataContext context)
	{
		Events.UnRegisterHandler_NormalAttackEnd(OnNormalAttackEnd);
		DomainManager.Combat.SilenceSkill(context, base.CombatChar, base.SkillTemplateId, 3000, -1);
		base.OnDisable(context);
	}

	private void OnNormalAttackEnd(DataContext context, CombatCharacter attacker, CombatCharacter defender, sbyte trickType, int pursueIndex, bool hit, bool isFightBack)
	{
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		if (!isFightBack || !hit || attacker != base.CombatChar || !base.CanAffect)
		{
			return;
		}
		CombatCharacter combatCharacter = (base.IsDirect ? attacker : defender);
		NeiliAllocation neiliAllocation = combatCharacter.GetNeiliAllocation();
		NeiliAllocation originNeiliAllocation = combatCharacter.GetOriginNeiliAllocation();
		List<byte> list = ObjectPool<List<byte>>.Instance.Get();
		list.Clear();
		for (byte b = 0; b < 4; b++)
		{
			if (!(base.IsDirect ? (neiliAllocation[b] >= originNeiliAllocation[b]) : (neiliAllocation[b] <= originNeiliAllocation[b])))
			{
				list.Add(b);
			}
		}
		if (list.Count > 0)
		{
			byte random = list.GetRandom(context.Random);
			int addValue = Math.Max(Math.Abs(originNeiliAllocation[random] - neiliAllocation[random]) * ChangeNeiliAllocationPercent, 1) * (base.IsDirect ? 1 : (-1));
			combatCharacter.ChangeNeiliAllocation(context, random, addValue);
			ShowSpecialEffectTipsOnceInFrame(1);
		}
		ObjectPool<List<byte>>.Instance.Return(list);
	}

	public override bool GetModifiedValue(AffectedDataKey dataKey, bool dataValue)
	{
		if (dataKey.CharId != base.CharacterId || dataKey.FieldId != 291 || !base.CanAffect)
		{
			return dataValue;
		}
		if (dataKey.CustomParam0 == 1)
		{
			return dataValue;
		}
		ShowSpecialEffectTipsOnceInFrame(0);
		return true;
	}
}
