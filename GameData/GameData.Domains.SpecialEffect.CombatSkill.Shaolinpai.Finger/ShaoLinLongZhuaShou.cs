using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.Item;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Attack;

namespace GameData.Domains.SpecialEffect.CombatSkill.Shaolinpai.Finger;

public class ShaoLinLongZhuaShou : AddWeaponEquipAttackOnAttack
{
	private const int AttackerPenetrateAddPercentMax = 40;

	private int _addPenetrate;

	protected override short AddWeaponEquipAttack => 500;

	public ShaoLinLongZhuaShou()
	{
	}

	public ShaoLinLongZhuaShou(CombatSkillKey skillKey)
		: base(skillKey, 1205)
	{
	}

	protected override void OnPrepareSkillEnd(DataContext context, int charId, bool isAlly, short skillId)
	{
		base.OnPrepareSkillEnd(context, charId, isAlly, skillId);
		if (charId == base.CharacterId && skillId == base.SkillTemplateId && DomainManager.Combat.InAttackRange(base.CombatChar))
		{
			ItemKey itemKey = base.CurrEnemyChar.Armors[base.CombatChar.SkillAttackBodyPart];
			Armor armor = (itemKey.IsValid() ? DomainManager.Item.GetElement_Armors(itemKey.Id) : null);
			int num = CombatDomain.CalcWeaponAttack(base.CombatChar, DomainManager.Item.GetElement_Weapons(DomainManager.Combat.GetUsingWeaponKey(base.CombatChar).Id), base.SkillTemplateId);
			int num2 = CombatDomain.CalcArmorDefend(base.CurrEnemyChar, armor);
			_addPenetrate = Math.Min(40, (num > num2) ? (num * 100 / num2 - 20) : 0);
			if (_addPenetrate > 0)
			{
				AppendAffectedData(context, base.CharacterId, (ushort)(base.IsDirect ? 64 : 65), (EDataModifyType)1, -1);
				ShowSpecialEffectTips(1);
			}
		}
	}

	public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
	{
		if (dataKey.CharId != base.CharacterId)
		{
			return 0;
		}
		if (dataKey.FieldId == 64 || dataKey.FieldId == 65)
		{
			return _addPenetrate;
		}
		return base.GetModifyValue(dataKey, currModifyValue);
	}
}
