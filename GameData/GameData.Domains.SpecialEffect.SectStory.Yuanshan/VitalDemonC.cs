using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;

namespace GameData.Domains.SpecialEffect.SectStory.Yuanshan;

public class VitalDemonC : VitalDemonEffectBase
{
	private const int DirectDamageAddPercent = 200;

	private bool _affected;

	public VitalDemonC(int charId)
		: base(charId, 1750)
	{
	}

	public override void OnEnable(DataContext context)
	{
		base.OnEnable(context);
		Events.RegisterHandler_AddDirectDamageValue(OnAddDirectDamageValue);
		Events.RegisterHandler_CombatStateMachineUpdateEnd(OnCombatStateMachineUpdateEnd);
		foreach (CombatCharacter character in DomainManager.Combat.GetCharacters(base.CombatChar.IsAlly))
		{
			CreateAffectedData(character.GetId(), 69, (EDataModifyType)1, -1);
			CreateAffectedData(character.GetId(), 77, (EDataModifyType)3, -1);
		}
	}

	public override void OnDisable(DataContext context)
	{
		Events.UnRegisterHandler_AddDirectDamageValue(OnAddDirectDamageValue);
		Events.UnRegisterHandler_CombatStateMachineUpdateEnd(OnCombatStateMachineUpdateEnd);
		base.OnDisable(context);
	}

	private void OnAddDirectDamageValue(DataContext context, int attackerId, int defenderId, sbyte bodyPart, bool isInner, int damageValue, short combatSkillId)
	{
		if (combatSkillId < 0 && damageValue > 0 && !_affected)
		{
			CombatCharacter element_CombatCharacterDict = DomainManager.Combat.GetElement_CombatCharacterDict(attackerId);
			CombatCharacter element_CombatCharacterDict2 = DomainManager.Combat.GetElement_CombatCharacterDict(defenderId);
			if (element_CombatCharacterDict.IsAlly == base.CombatChar.IsAlly && element_CombatCharacterDict2.IsAlly != base.CombatChar.IsAlly)
			{
				_affected = true;
				ShowSpecialEffect(0);
				ShowSpecialEffect(1);
			}
		}
	}

	private void OnCombatStateMachineUpdateEnd(DataContext context, CombatCharacter combatChar)
	{
		if (combatChar.IsAlly == base.CombatChar.IsAlly)
		{
			_affected = false;
		}
	}

	public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
	{
		if (dataKey.IsNormalAttack && dataKey.FieldId == 69)
		{
			return 200;
		}
		return base.GetModifyValue(dataKey, currModifyValue);
	}

	public override bool GetModifiedValue(AffectedDataKey dataKey, bool dataValue)
	{
		if (dataKey.IsNormalAttack && dataKey.FieldId == 77)
		{
			return true;
		}
		return base.GetModifiedValue(dataKey, dataValue);
	}
}
