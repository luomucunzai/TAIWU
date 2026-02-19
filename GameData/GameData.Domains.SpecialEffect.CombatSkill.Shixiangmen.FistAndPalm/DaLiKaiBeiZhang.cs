using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.Item;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Attack;

namespace GameData.Domains.SpecialEffect.CombatSkill.Shixiangmen.FistAndPalm;

public class DaLiKaiBeiZhang : AddWeaponEquipAttackOnAttack
{
	private const sbyte PenetrateChangePercent = 50;

	private const int AddPenetrateThreshold = 75;

	protected override short AddWeaponEquipAttack => 1500;

	public DaLiKaiBeiZhang()
	{
	}

	public DaLiKaiBeiZhang(CombatSkillKey skillKey)
		: base(skillKey, 6103)
	{
	}

	protected override void OnPrepareSkillEnd(DataContext context, int charId, bool isAlly, short skillId)
	{
		if (charId == base.CharacterId && skillId == base.SkillTemplateId)
		{
			base.OnPrepareSkillEnd(context, charId, isAlly, skillId);
			CombatCharacter combatCharacter = DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly, tryGetCoverCharacter: true);
			Weapon element_Weapons = DomainManager.Item.GetElement_Weapons(DomainManager.Combat.GetUsingWeaponKey(base.CombatChar).Id);
			ItemKey itemKey = combatCharacter.Armors[base.CombatChar.SkillAttackBodyPart];
			Armor armor = (itemKey.IsValid() ? DomainManager.Item.GetElement_Armors(itemKey.Id) : null);
			int num = ((armor != null && armor.GetMaxDurability() > 0) ? (armor.GetCurrDurability() * 100 / armor.GetMaxDurability()) : 0);
			if (CombatDomain.CalcWeaponAttack(base.CombatChar, element_Weapons, base.SkillTemplateId) > CombatDomain.CalcArmorDefend(combatCharacter, armor) && (base.IsDirect ? (num >= 75) : (num <= 75)))
			{
				AppendAffectedData(context, base.CharacterId, 64, (EDataModifyType)2, -1);
				AppendAffectedData(context, base.CharacterId, 65, (EDataModifyType)2, -1);
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
		ushort fieldId = dataKey.FieldId;
		if ((uint)(fieldId - 64) <= 1u)
		{
			return 50;
		}
		return base.GetModifyValue(dataKey, currModifyValue);
	}
}
