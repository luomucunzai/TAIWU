using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.Item;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Attack;

namespace GameData.Domains.SpecialEffect.CombatSkill.Shixiangmen.FistAndPalm
{
	// Token: 0x020003F7 RID: 1015
	public class DaLiKaiBeiZhang : AddWeaponEquipAttackOnAttack
	{
		// Token: 0x17000202 RID: 514
		// (get) Token: 0x06003880 RID: 14464 RVA: 0x0023A99F File Offset: 0x00238B9F
		protected override short AddWeaponEquipAttack
		{
			get
			{
				return 1500;
			}
		}

		// Token: 0x06003881 RID: 14465 RVA: 0x0023A9A6 File Offset: 0x00238BA6
		public DaLiKaiBeiZhang()
		{
		}

		// Token: 0x06003882 RID: 14466 RVA: 0x0023A9B0 File Offset: 0x00238BB0
		public DaLiKaiBeiZhang(CombatSkillKey skillKey) : base(skillKey, 6103)
		{
		}

		// Token: 0x06003883 RID: 14467 RVA: 0x0023A9C0 File Offset: 0x00238BC0
		protected override void OnPrepareSkillEnd(DataContext context, int charId, bool isAlly, short skillId)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				base.OnPrepareSkillEnd(context, charId, isAlly, skillId);
				CombatCharacter enemyChar = DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly, true);
				Weapon weapon = DomainManager.Item.GetElement_Weapons(DomainManager.Combat.GetUsingWeaponKey(base.CombatChar).Id);
				ItemKey armorKey = enemyChar.Armors[(int)base.CombatChar.SkillAttackBodyPart];
				Armor armor = armorKey.IsValid() ? DomainManager.Item.GetElement_Armors(armorKey.Id) : null;
				int armorDurabilityPercent = (int)((armor != null && armor.GetMaxDurability() > 0) ? (armor.GetCurrDurability() * 100 / armor.GetMaxDurability()) : 0);
				bool flag2 = CombatDomain.CalcWeaponAttack(base.CombatChar, weapon, base.SkillTemplateId) > CombatDomain.CalcArmorDefend(enemyChar, armor) && (base.IsDirect ? (armorDurabilityPercent >= 75) : (armorDurabilityPercent <= 75));
				if (flag2)
				{
					base.AppendAffectedData(context, base.CharacterId, 64, EDataModifyType.TotalPercent, -1);
					base.AppendAffectedData(context, base.CharacterId, 65, EDataModifyType.TotalPercent, -1);
					base.ShowSpecialEffectTips(1);
				}
			}
		}

		// Token: 0x06003884 RID: 14468 RVA: 0x0023AAFC File Offset: 0x00238CFC
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.CharId != base.CharacterId;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				ushort fieldId = dataKey.FieldId;
				bool flag2 = fieldId - 64 <= 1;
				bool flag3 = flag2;
				if (flag3)
				{
					result = 50;
				}
				else
				{
					result = base.GetModifyValue(dataKey, currModifyValue);
				}
			}
			return result;
		}

		// Token: 0x04001089 RID: 4233
		private const sbyte PenetrateChangePercent = 50;

		// Token: 0x0400108A RID: 4234
		private const int AddPenetrateThreshold = 75;
	}
}
