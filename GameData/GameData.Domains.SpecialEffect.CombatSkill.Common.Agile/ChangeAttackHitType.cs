using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Common.Agile;

public class ChangeAttackHitType : AgileSkillBase
{
	protected sbyte HitType;

	private bool _affected;

	protected ChangeAttackHitType()
	{
	}

	protected ChangeAttackHitType(CombatSkillKey skillKey, int type)
		: base(skillKey, type)
	{
	}

	public override void OnEnable(DataContext context)
	{
		base.OnEnable(context);
		AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
		if (base.IsDirect)
		{
			AffectDatas.Add(new AffectedDataKey(base.CharacterId, 68, -1), (EDataModifyType)3);
			base.CombatChar.ChangeHitTypeEffectCount++;
		}
		else
		{
			int[] characterList = DomainManager.Combat.GetCharacterList(!base.CombatChar.IsAlly);
			for (int i = 0; i < characterList.Length; i++)
			{
				if (characterList[i] >= 0)
				{
					AffectDatas.Add(new AffectedDataKey(characterList[i], 68, -1), (EDataModifyType)3);
				}
			}
			base.CombatChar.ChangeAvoidTypeEffectCount++;
		}
		Events.RegisterHandler_NormalAttackEnd(OnNormalAttackEnd);
	}

	public override void OnDisable(DataContext context)
	{
		base.OnDisable(context);
		if (base.IsDirect)
		{
			base.CombatChar.ChangeHitTypeEffectCount--;
		}
		else
		{
			base.CombatChar.ChangeAvoidTypeEffectCount--;
		}
		Events.UnRegisterHandler_NormalAttackEnd(OnNormalAttackEnd);
	}

	private void OnNormalAttackEnd(DataContext context, CombatCharacter attacker, CombatCharacter defender, sbyte trickType, int pursueIndex, bool hit, bool isFightBack)
	{
		if (_affected)
		{
			_affected = false;
			if (pursueIndex == 0)
			{
				ShowSpecialEffectTips(0);
			}
		}
	}

	public override int GetModifiedValue(AffectedDataKey dataKey, int dataValue)
	{
		if (dataKey.CombatSkillId >= 0 || !base.CanAffect)
		{
			return dataValue;
		}
		CombatCharacter currEnemyChar = base.CurrEnemyChar;
		if ((base.IsDirect ? currEnemyChar.ChangeAvoidTypeEffectCount : currEnemyChar.ChangeHitTypeEffectCount) > 0)
		{
			return dataValue;
		}
		_affected = true;
		return HitType;
	}
}
