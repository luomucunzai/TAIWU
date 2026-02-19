using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.Jingangzong.DefenseAndAssist;

public class QiLunGanYingFaStateEffect
{
	private const int ChangeNeiliAllocationUnit = 1;

	private const int StateAffectFrame = 240;

	private bool _affecting;

	private int _setupCount;

	private readonly Dictionary<int, int> _buffAffectingCounter = new Dictionary<int, int>();

	private readonly Dictionary<int, int> _debuffAffectingCounter = new Dictionary<int, int>();

	public void Setup()
	{
		_setupCount++;
		UpdateAffecting();
	}

	public void Close()
	{
		_setupCount--;
		UpdateAffecting();
	}

	public void Reset()
	{
		if (_affecting)
		{
			Events.UnRegisterHandler_CombatStateMachineUpdateEnd(OnCombatStateMachineUpdateEnd);
		}
		_affecting = false;
		_setupCount = 0;
	}

	private void UpdateAffecting()
	{
		bool flag = _setupCount > 0;
		if (flag != _affecting)
		{
			_affecting = flag;
			if (flag)
			{
				Events.RegisterHandler_CombatStateMachineUpdateEnd(OnCombatStateMachineUpdateEnd);
			}
			else
			{
				Events.UnRegisterHandler_CombatStateMachineUpdateEnd(OnCombatStateMachineUpdateEnd);
			}
		}
	}

	private void OnCombatStateMachineUpdateEnd(DataContext context, CombatCharacter combatChar)
	{
		if (!DomainManager.Combat.Pause)
		{
			DoAffect(context, combatChar, buff: true);
			DoAffect(context, combatChar, buff: false);
		}
	}

	private void DoAffect(DataContext context, CombatCharacter affectChar, bool buff)
	{
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		sbyte stateType = (sbyte)(buff ? 1 : 2);
		short stateId = (short)(buff ? 166 : 167);
		Dictionary<int, int> dictionary = (buff ? _buffAffectingCounter : _debuffAffectingCounter);
		CValuePercent val = CValuePercent.op_Implicit(affectChar.GetCombatStatePower(stateType, stateId));
		int num = 1 * val;
		if (num <= 0)
		{
			dictionary[affectChar.GetId()] = 0;
			return;
		}
		dictionary[affectChar.GetId()] = dictionary.GetOrDefault(affectChar.GetId()) + 1;
		if (dictionary[affectChar.GetId()] == 240)
		{
			dictionary[affectChar.GetId()] = 0;
			short effectId = (short)(buff ? 1697 : 1698);
			if (affectChar.ChangeNeiliAllocationRandom(context, buff ? num : (-num)))
			{
				DomainManager.Combat.ShowSpecialEffectTips(affectChar.GetId(), effectId, 0);
			}
		}
	}
}
