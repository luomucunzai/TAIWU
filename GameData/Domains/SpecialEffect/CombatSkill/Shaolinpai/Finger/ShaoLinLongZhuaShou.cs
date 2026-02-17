using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.Item;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Attack;

namespace GameData.Domains.SpecialEffect.CombatSkill.Shaolinpai.Finger
{
	// Token: 0x0200042F RID: 1071
	public class ShaoLinLongZhuaShou : AddWeaponEquipAttackOnAttack
	{
		// Token: 0x1700020B RID: 523
		// (get) Token: 0x060039A8 RID: 14760 RVA: 0x0023FB4E File Offset: 0x0023DD4E
		protected override short AddWeaponEquipAttack
		{
			get
			{
				return 500;
			}
		}

		// Token: 0x060039A9 RID: 14761 RVA: 0x0023FB55 File Offset: 0x0023DD55
		public ShaoLinLongZhuaShou()
		{
		}

		// Token: 0x060039AA RID: 14762 RVA: 0x0023FB5F File Offset: 0x0023DD5F
		public ShaoLinLongZhuaShou(CombatSkillKey skillKey) : base(skillKey, 1205)
		{
		}

		// Token: 0x060039AB RID: 14763 RVA: 0x0023FB70 File Offset: 0x0023DD70
		protected override void OnPrepareSkillEnd(DataContext context, int charId, bool isAlly, short skillId)
		{
			base.OnPrepareSkillEnd(context, charId, isAlly, skillId);
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId || !DomainManager.Combat.InAttackRange(base.CombatChar);
			if (!flag)
			{
				ItemKey armorKey = base.CurrEnemyChar.Armors[(int)base.CombatChar.SkillAttackBodyPart];
				Armor armor = armorKey.IsValid() ? DomainManager.Item.GetElement_Armors(armorKey.Id) : null;
				int weaponAttack = CombatDomain.CalcWeaponAttack(base.CombatChar, DomainManager.Item.GetElement_Weapons(DomainManager.Combat.GetUsingWeaponKey(base.CombatChar).Id), base.SkillTemplateId);
				int armorDefense = CombatDomain.CalcArmorDefend(base.CurrEnemyChar, armor);
				this._addPenetrate = Math.Min(40, (weaponAttack > armorDefense) ? (weaponAttack * 100 / armorDefense - 20) : 0);
				bool flag2 = this._addPenetrate > 0;
				if (flag2)
				{
					base.AppendAffectedData(context, base.CharacterId, base.IsDirect ? 64 : 65, EDataModifyType.AddPercent, -1);
					base.ShowSpecialEffectTips(1);
				}
			}
		}

		// Token: 0x060039AC RID: 14764 RVA: 0x0023FC8C File Offset: 0x0023DE8C
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
				bool flag2 = dataKey.FieldId == 64 || dataKey.FieldId == 65;
				if (flag2)
				{
					result = this._addPenetrate;
				}
				else
				{
					result = base.GetModifyValue(dataKey, currModifyValue);
				}
			}
			return result;
		}

		// Token: 0x040010DD RID: 4317
		private const int AttackerPenetrateAddPercentMax = 40;

		// Token: 0x040010DE RID: 4318
		private int _addPenetrate;
	}
}
