using System;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Defense;

namespace GameData.Domains.SpecialEffect.CombatSkill.Shixiangmen.DefenseAndAssist;

public class BaWangJieJiaGong : DefenseSkillBase
{
	private const sbyte ChangeNeiliAllocationUnit = 5;

	public BaWangJieJiaGong()
	{
	}

	public BaWangJieJiaGong(CombatSkillKey skillKey)
		: base(skillKey, 6505)
	{
	}

	public override void OnEnable(DataContext context)
	{
		base.OnEnable(context);
		Events.RegisterHandler_AddDirectInjury(OnAddDirectInjury);
		Events.RegisterHandler_AddDirectFatalDamageMark(OnAddDirectFatalDamageMark);
	}

	public override void OnDisable(DataContext context)
	{
		base.OnDisable(context);
		Events.UnRegisterHandler_AddDirectInjury(OnAddDirectInjury);
		Events.UnRegisterHandler_AddDirectFatalDamageMark(OnAddDirectFatalDamageMark);
	}

	private void OnAddDirectInjury(DataContext context, int attackerId, int defenderId, bool isAlly, sbyte bodyPart, sbyte outerMarkCount, sbyte innerMarkCount, short combatSkillId)
	{
		if ((outerMarkCount > 0 || innerMarkCount > 0) && defenderId == base.CharacterId && DomainManager.Combat.GetElement_CombatCharacterDict(attackerId).IsAlly != base.CombatChar.IsAlly)
		{
			ChangeNeiliAllocation(context, outerMarkCount, innerMarkCount);
		}
	}

	private void OnAddDirectFatalDamageMark(DataContext context, int attackerId, int defenderId, bool isAlly, sbyte bodyPart, int outerMarkCount, int innerMarkCount, short combatSkillId)
	{
		if ((outerMarkCount > 0 || innerMarkCount > 0) && defenderId == base.CharacterId && DomainManager.Combat.GetElement_CombatCharacterDict(attackerId).IsAlly != base.CombatChar.IsAlly)
		{
			ChangeNeiliAllocation(context, outerMarkCount, innerMarkCount);
		}
	}

	private unsafe void ChangeNeiliAllocation(DataContext context, int outerMarkCount, int innerMarkCount)
	{
		if (!base.CanAffect)
		{
			return;
		}
		int val = 5 * (outerMarkCount + innerMarkCount);
		NeiliAllocation neiliAllocation = base.CombatChar.GetNeiliAllocation();
		NeiliAllocation neiliAllocation2 = base.CurrEnemyChar.GetNeiliAllocation();
		byte b = (byte)(base.IsDirect ? 2 : 0);
		byte type = (byte)((!base.IsDirect) ? 2 : 0);
		int num = Math.Min(val, neiliAllocation.Items[(int)b]);
		int num2 = Math.Min(val, neiliAllocation2.Items[(int)b]);
		if (num != 0 || num2 != 0)
		{
			if (num > 0)
			{
				base.CombatChar.ChangeNeiliAllocation(context, b, -num);
				base.CombatChar.ChangeNeiliAllocation(context, type, num);
			}
			if (num2 > 0)
			{
				base.CurrEnemyChar.ChangeNeiliAllocation(context, b, -num2);
			}
			ShowSpecialEffectTipsOnceInFrame(0);
		}
	}
}
