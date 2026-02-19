using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.Combat;
using GameData.Domains.SpecialEffect.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Implement;

namespace GameData.Domains.SpecialEffect.Animal.Loong.Neigong;

public class LoongFireImplementBrokenHit : ISpecialEffectImplement, ISpecialEffectModifier
{
	public CombatSkillEffectBase EffectBase { get; set; }

	public void OnEnable(DataContext context)
	{
		EffectBase.CreateAffectedData(251, (EDataModifyType)3, -1);
	}

	public void OnDisable(DataContext context)
	{
	}

	public bool GetModifiedValue(AffectedDataKey dataKey, bool dataValue)
	{
		if (dataKey.CharId != EffectBase.CharacterId || dataKey.FieldId != 251 || !dataKey.IsNormalAttack)
		{
			return dataValue;
		}
		CombatCharacter currEnemyChar = EffectBase.CurrEnemyChar;
		sbyte normalAttackBodyPart = EffectBase.CombatChar.NormalAttackBodyPart;
		if (!DomainManager.Combat.CheckBodyPartInjury(currEnemyChar, normalAttackBodyPart))
		{
			return dataValue;
		}
		EffectBase.ShowSpecialEffectTips(1);
		return true;
	}
}
