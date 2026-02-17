using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.Item;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Attack;

namespace GameData.Domains.SpecialEffect.CombatSkill.Kongsangpai.Throw
{
	// Token: 0x0200047D RID: 1149
	public class QingJiaoXue : PoisonAddInjury
	{
		// Token: 0x06003B95 RID: 15253 RVA: 0x00248C23 File Offset: 0x00246E23
		public QingJiaoXue()
		{
		}

		// Token: 0x06003B96 RID: 15254 RVA: 0x00248C2D File Offset: 0x00246E2D
		public QingJiaoXue(CombatSkillKey skillKey) : base(skillKey, 10407)
		{
			this.RequirePoisonType = 4;
		}

		// Token: 0x06003B97 RID: 15255 RVA: 0x00248C44 File Offset: 0x00246E44
		protected override void OnCastOwnBegin(DataContext context)
		{
			this._affectedBodyPart = -1;
			base.AppendAffectedData(context, 327, EDataModifyType.Custom, base.SkillTemplateId);
		}

		// Token: 0x06003B98 RID: 15256 RVA: 0x00248C64 File Offset: 0x00246E64
		protected override void OnCastMaxPower(DataContext context)
		{
			bool flag = this._affectedBodyPart < 0;
			if (!flag)
			{
				byte poisonMarkCount = (base.IsDirect ? base.CurrEnemyChar : base.CombatChar).GetDefeatMarkCollection().PoisonMarkList[(int)this.RequirePoisonType];
				int costDurabilityPercent = (int)(poisonMarkCount * 10);
				bool flag2 = costDurabilityPercent == 0;
				if (!flag2)
				{
					bool anyChanged = false;
					foreach (ItemKey armorKey in this.IterEnemyArmors())
					{
						Armor armor = armorKey.IsValid() ? DomainManager.Item.GetElement_Armors(armorKey.Id) : null;
						bool flag3 = armor == null || armor.GetCurrDurability() == 0;
						if (!flag3)
						{
							int costDurability = (int)armor.GetMaxDurability() * costDurabilityPercent / 100;
							base.ChangeDurability(context, base.CurrEnemyChar, armorKey, -costDurability);
							anyChanged = true;
						}
					}
					bool flag4 = anyChanged;
					if (flag4)
					{
						base.ShowSpecialEffectTips(1);
					}
				}
			}
		}

		// Token: 0x06003B99 RID: 15257 RVA: 0x00248D70 File Offset: 0x00246F70
		public override bool GetModifiedValue(AffectedDataKey dataKey, bool dataValue)
		{
			bool flag = dataKey.SkillKey != this.SkillKey || dataKey.FieldId != 327;
			bool result;
			if (flag)
			{
				result = base.GetModifiedValue(dataKey, dataValue);
			}
			else
			{
				EDamageType damageType = (EDamageType)dataKey.CustomParam2;
				bool flag2 = damageType != EDamageType.Direct;
				if (flag2)
				{
					result = base.GetModifiedValue(dataKey, dataValue);
				}
				else
				{
					sbyte bodyPart = this._affectedBodyPart = (sbyte)dataKey.CustomParam1;
					ItemKey armorKey = base.CurrEnemyChar.Armors[(int)bodyPart];
					Armor armor = armorKey.IsValid() ? DomainManager.Item.GetElement_Armors(armorKey.Id) : null;
					bool flag3 = armor != null && armor.GetCurrDurability() > 0;
					result = (flag3 && base.GetModifiedValue(dataKey, dataValue));
				}
			}
			return result;
		}

		// Token: 0x06003B9A RID: 15258 RVA: 0x00248E44 File Offset: 0x00247044
		private IEnumerable<ItemKey> IterEnemyArmors()
		{
			ItemKey[] enemyArmors = base.CurrEnemyChar.Armors;
			yield return enemyArmors[2];
			yield return enemyArmors[0];
			yield return enemyArmors[3];
			yield return enemyArmors[5];
			yield break;
		}

		// Token: 0x04001174 RID: 4468
		private const int CostDurabilityPercent = 10;

		// Token: 0x04001175 RID: 4469
		private sbyte _affectedBodyPart;
	}
}
