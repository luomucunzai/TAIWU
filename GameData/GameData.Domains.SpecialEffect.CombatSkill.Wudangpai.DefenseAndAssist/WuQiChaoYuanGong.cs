using System.Collections.Generic;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Defense;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.Wudangpai.DefenseAndAssist;

public class WuQiChaoYuanGong : DefenseSkillBase
{
	private const sbyte ChangeNeiliAllocationFrame = 30;

	private List<sbyte> _frameCounter;

	public WuQiChaoYuanGong()
	{
	}

	public WuQiChaoYuanGong(CombatSkillKey skillKey)
		: base(skillKey, 4504)
	{
	}

	public override void OnEnable(DataContext context)
	{
		base.OnEnable(context);
		_frameCounter = ObjectPool<List<sbyte>>.Instance.Get();
		_frameCounter.Clear();
		for (int i = 0; i < 4; i++)
		{
			_frameCounter.Add(0);
		}
		Events.RegisterHandler_CombatStateMachineUpdateEnd(OnStateMachineUpdateEnd);
		if (!base.IsDirect)
		{
			Events.RegisterHandler_CombatCharChanged(OnCombatCharChanged);
		}
	}

	public override void OnDisable(DataContext context)
	{
		base.OnDisable(context);
		ObjectPool<List<sbyte>>.Instance.Return(_frameCounter);
		Events.UnRegisterHandler_CombatStateMachineUpdateEnd(OnStateMachineUpdateEnd);
		if (!base.IsDirect)
		{
			Events.UnRegisterHandler_CombatCharChanged(OnCombatCharChanged);
		}
	}

	private unsafe void OnStateMachineUpdateEnd(DataContext context, CombatCharacter combatChar)
	{
		if (combatChar.IsAlly != base.CombatChar.IsAlly || DomainManager.Combat.Pause)
		{
			return;
		}
		CombatCharacter combatCharacter = (base.IsDirect ? base.CombatChar : DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly));
		NeiliAllocation neiliAllocation = combatCharacter.GetNeiliAllocation();
		NeiliAllocation originNeiliAllocation = combatCharacter.GetOriginNeiliAllocation();
		bool flag = false;
		for (byte b = 0; b < 4; b++)
		{
			short num = neiliAllocation.Items[(int)b];
			short num2 = originNeiliAllocation.Items[(int)b];
			if (base.IsDirect ? (num < num2) : (num > num2))
			{
				_frameCounter[b]++;
				if (_frameCounter[b] >= 30)
				{
					_frameCounter[b] = 0;
					if (base.CanAffect)
					{
						flag = true;
						combatCharacter.ChangeNeiliAllocation(context, b, base.IsDirect ? 1 : (-1));
					}
				}
			}
			else
			{
				_frameCounter[b] = 0;
			}
		}
		if (flag)
		{
			ShowSpecialEffectTips(0);
		}
	}

	private void OnCombatCharChanged(DataContext context, bool isAlly)
	{
		if (isAlly != base.CombatChar.IsAlly)
		{
			for (int i = 0; i < 4; i++)
			{
				_frameCounter[i] = 0;
			}
		}
	}
}
