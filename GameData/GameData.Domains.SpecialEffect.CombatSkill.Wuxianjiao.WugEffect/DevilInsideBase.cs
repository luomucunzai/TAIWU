using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.Combat;
using GameData.Domains.LifeRecord;

namespace GameData.Domains.SpecialEffect.CombatSkill.Wuxianjiao.WugEffect;

public class DevilInsideBase : WugEffectBase
{
	private bool _changingNeiliAllocation;

	private bool _affectedXiangshuOnMonthChange;

	private bool _affectedHappinessOnMonthChange;

	private int AddXiangshuInfectionValue => base.IsElite ? 50 : 5;

	private int ChangeNeiliAllocationValue => 2 * (base.IsGood ? 1 : (-1)) * ((base.IsGrown || !base.IsElite) ? 1 : 2);

	public static bool CanGrown(GameData.Domains.Character.Character character)
	{
		sbyte happinessType = character.GetHappinessType();
		if (happinessType == 0 || happinessType == 6)
		{
			return true;
		}
		return false;
	}

	protected DevilInsideBase()
	{
	}

	protected DevilInsideBase(int charId, int type, short wugTemplateId, short effectId)
		: base(charId, type, wugTemplateId, effectId)
	{
		CostWugCount = 0;
	}

	public override void OnEnable(DataContext context)
	{
		base.OnEnable(context);
		if (base.IsGrown)
		{
			CreateAffectedData(262, (EDataModifyType)0, -1);
		}
		if (base.CanChangeToGrown)
		{
			CreateAffectedData(270, (EDataModifyType)0, -1);
		}
		Events.RegisterHandler_AdvanceMonthFinish(OnAdvanceMonthFinish);
	}

	public override void OnDisable(DataContext context)
	{
		Events.UnRegisterHandler_AdvanceMonthFinish(OnAdvanceMonthFinish);
		base.OnDisable(context);
	}

	protected override void AddAffectDataAndEvent(DataContext context)
	{
		_changingNeiliAllocation = false;
		Events.RegisterHandler_NeiliAllocationChanged(OnNeiliAllocationChanged);
	}

	protected override void ClearAffectDataAndEvent(DataContext context)
	{
		Events.UnRegisterHandler_NeiliAllocationChanged(OnNeiliAllocationChanged);
	}

	private void OnAdvanceMonthFinish(DataContext context)
	{
		LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
		if (_affectedXiangshuOnMonthChange)
		{
			_affectedXiangshuOnMonthChange = false;
			AddLifeRecord(lifeRecordCollection.AddWugDevilInsideXiangshuInfection);
		}
		if (_affectedHappinessOnMonthChange)
		{
			_affectedHappinessOnMonthChange = false;
			AddLifeRecord(lifeRecordCollection.AddWugDevilInsideChangeHappiness);
		}
		if (base.CanChangeToGrown && CanGrown(CharObj))
		{
			ChangeToGrown(context);
		}
	}

	private void OnNeiliAllocationChanged(DataContext context, int charId, byte type, int changeValue)
	{
		if (charId != base.CharacterId || _changingNeiliAllocation || !(base.IsGood ? (changeValue > 0) : (changeValue < 0)) || !base.CanAffect)
		{
			return;
		}
		CombatCharacter combatCharacter = (base.IsGood ? base.CombatChar : DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly));
		_changingNeiliAllocation = true;
		int num = base.CombatChar.ChangeNeiliAllocation(context, (byte)context.Random.Next(0, 4), ChangeNeiliAllocationValue);
		_changingNeiliAllocation = false;
		combatCharacter.ChangeWugCount(context, Math.Abs(changeValue));
		if (num != 0)
		{
			ShowEffectTips(context, 1);
			if (base.IsElite)
			{
				ShowEffectTips(context, 2);
			}
			CostWugInCombat(context);
		}
	}

	public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
	{
		if (dataKey.CharId != base.CharacterId || !base.CanAffect)
		{
			return 0;
		}
		if (dataKey.FieldId == 270)
		{
			_affectedHappinessOnMonthChange = true;
			sbyte happinessType = CharObj.GetHappinessType();
			if (1 == 0)
			{
			}
			int result = ((happinessType > 3) ? 5 : ((happinessType < 3) ? (-5) : 0));
			if (1 == 0)
			{
			}
			return result;
		}
		if (dataKey.FieldId == 262)
		{
			_affectedXiangshuOnMonthChange = true;
			return AddXiangshuInfectionValue;
		}
		return 0;
	}
}
