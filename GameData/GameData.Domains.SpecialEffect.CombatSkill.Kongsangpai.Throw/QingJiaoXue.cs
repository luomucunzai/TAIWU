using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.Item;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Attack;

namespace GameData.Domains.SpecialEffect.CombatSkill.Kongsangpai.Throw;

public class QingJiaoXue : PoisonAddInjury
{
	private const int CostDurabilityPercent = 10;

	private sbyte _affectedBodyPart;

	public QingJiaoXue()
	{
	}

	public QingJiaoXue(CombatSkillKey skillKey)
		: base(skillKey, 10407)
	{
		RequirePoisonType = 4;
	}

	protected override void OnCastOwnBegin(DataContext context)
	{
		_affectedBodyPart = -1;
		AppendAffectedData(context, 327, (EDataModifyType)3, base.SkillTemplateId);
	}

	protected override void OnCastMaxPower(DataContext context)
	{
		if (_affectedBodyPart < 0)
		{
			return;
		}
		byte b = (base.IsDirect ? base.CurrEnemyChar : base.CombatChar).GetDefeatMarkCollection().PoisonMarkList[RequirePoisonType];
		int num = b * 10;
		if (num == 0)
		{
			return;
		}
		bool flag = false;
		foreach (ItemKey item in IterEnemyArmors())
		{
			Armor armor = (item.IsValid() ? DomainManager.Item.GetElement_Armors(item.Id) : null);
			if (armor != null && armor.GetCurrDurability() != 0)
			{
				int num2 = armor.GetMaxDurability() * num / 100;
				ChangeDurability(context, base.CurrEnemyChar, item, -num2);
				flag = true;
			}
		}
		if (flag)
		{
			ShowSpecialEffectTips(1);
		}
	}

	public override bool GetModifiedValue(AffectedDataKey dataKey, bool dataValue)
	{
		if (dataKey.SkillKey != SkillKey || dataKey.FieldId != 327)
		{
			return base.GetModifiedValue(dataKey, dataValue);
		}
		EDamageType customParam = (EDamageType)dataKey.CustomParam2;
		if (customParam != EDamageType.Direct)
		{
			return base.GetModifiedValue(dataKey, dataValue);
		}
		sbyte b = (_affectedBodyPart = (sbyte)dataKey.CustomParam1);
		ItemKey itemKey = base.CurrEnemyChar.Armors[b];
		Armor armor = (itemKey.IsValid() ? DomainManager.Item.GetElement_Armors(itemKey.Id) : null);
		if (armor != null && armor.GetCurrDurability() > 0)
		{
			return base.GetModifiedValue(dataKey, dataValue);
		}
		return false;
	}

	private IEnumerable<ItemKey> IterEnemyArmors()
	{
		ItemKey[] enemyArmors = base.CurrEnemyChar.Armors;
		yield return enemyArmors[2];
		yield return enemyArmors[0];
		yield return enemyArmors[3];
		yield return enemyArmors[5];
	}
}
