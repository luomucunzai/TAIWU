using System;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Utilities;

namespace GameData.Domains.Combat;

public class CombatCharacterStateChangeCharacter : CombatCharacterStateBase
{
	private short _leftWaitFrame;

	public CombatCharacterStateChangeCharacter(CombatDomain combatDomain, CombatCharacter combatChar)
		: base(combatDomain, combatChar, CombatCharacterStateType.ChangeCharacter)
	{
		IsUpdateOnPause = true;
	}

	public override void OnEnter()
	{
		_leftWaitFrame = (short)Math.Round(90.0);
		DataContext dataContext = CombatChar.GetDataContext();
		CombatChar.SetAnimationToPlayOnce("M_003", dataContext);
		CombatChar.SetAnimationToLoop(CurrentCombatDomain.GetIdleAni(CombatChar), dataContext);
		if (!CurrentCombatDomain.IsMainCharacter(CombatChar))
		{
			CombatChar.SetBreathValue(15000, dataContext);
			CombatChar.SetStanceValue(2000, dataContext);
			CombatChar.SetMobilityValue(MoveSpecialConstants.MaxMobility * 50 / 100, dataContext);
			CombatChar.ClearAllDoingOrReserveCommand(dataContext);
			CombatChar.ClearAllSound(dataContext);
			CombatChar.SetExecutingTeammateCommand(CombatChar.ExecutingTeammateCommandConfig.TemplateId, dataContext);
			int num = CurrentCombatDomain.GetCharacterList(CombatChar.IsAlly).IndexOf(CombatChar.GetId()) - 1;
			CurrentCombatDomain.GetMainCharacter(CombatChar.IsAlly).TeammateHasCommand[num] = true;
		}
	}

	public override bool OnUpdate()
	{
		_leftWaitFrame--;
		if (_leftWaitFrame == 0)
		{
			DataContext dataContext = CombatChar.GetDataContext();
			if (!CurrentCombatDomain.IsMainCharacter(CombatChar))
			{
				CombatChar.ResetTeammateCommandLeftTime(dataContext);
			}
			else
			{
				bool flag = CurrentCombatDomain.IsCharacterFallen(CombatChar);
				CombatCharacter mainCharacter = CurrentCombatDomain.GetMainCharacter(!CombatChar.IsAlly);
				bool flag2 = CurrentCombatDomain.GetCombatCharacter(!CombatChar.IsAlly).ChangeCharId < 0 && (flag || CurrentCombatDomain.IsCharacterFallen(mainCharacter));
				int[] characterList = CurrentCombatDomain.GetCharacterList(CombatChar.IsAlly);
				for (int i = 1; i < characterList.Length; i++)
				{
					if (characterList[i] >= 0)
					{
						CurrentCombatDomain.GetElement_CombatCharacterDict(characterList[i]).SetVisible(visible: false, dataContext);
					}
				}
				if (flag2)
				{
					CurrentCombatDomain.EndCombat(dataContext, flag ? CombatChar : mainCharacter);
				}
			}
			CombatChar.StateMachine.TranslateState();
			Events.RaiseCombatCharChanged(dataContext, CombatChar.IsAlly);
		}
		return false;
	}
}
