using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Attack;

namespace GameData.Domains.SpecialEffect.CombatSkill.Fulongtan.FistAndPalm
{
	// Token: 0x0200051A RID: 1306
	public class DaYuYangShenQuan : PowerUpOnCast
	{
		// Token: 0x1700024D RID: 589
		// (get) Token: 0x06003EFF RID: 16127 RVA: 0x00257E79 File Offset: 0x00256079
		private CValuePercent AddPowerPercent
		{
			get
			{
				return base.IsDirect ? 40 : 80;
			}
		}

		// Token: 0x1700024E RID: 590
		// (get) Token: 0x06003F00 RID: 16128 RVA: 0x00257E8E File Offset: 0x0025608E
		protected override EDataModifyType ModifyType
		{
			get
			{
				return EDataModifyType.AddPercent;
			}
		}

		// Token: 0x06003F01 RID: 16129 RVA: 0x00257E91 File Offset: 0x00256091
		public DaYuYangShenQuan()
		{
		}

		// Token: 0x06003F02 RID: 16130 RVA: 0x00257E9B File Offset: 0x0025609B
		public DaYuYangShenQuan(CombatSkillKey skillKey) : base(skillKey, 14107)
		{
		}

		// Token: 0x06003F03 RID: 16131 RVA: 0x00257EAC File Offset: 0x002560AC
		public override void OnEnable(DataContext context)
		{
			CombatCharacter enemyChar = DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly, true);
			SkillEffectCollection effectCollection = (base.IsDirect ? base.CombatChar : enemyChar).GetSkillEffectCollection();
			Dictionary<SkillEffectKey, short> effectDict = effectCollection.EffectDict;
			bool flag = effectDict != null && effectDict.Count > 0;
			if (flag)
			{
				int totalPercent = 0;
				int effectCount = 0;
				foreach (KeyValuePair<SkillEffectKey, short> effect in effectDict)
				{
					int percent = (int)(effect.Value * 100 / effectCollection.MaxEffectCountDict[effect.Key]);
					totalPercent += (base.IsDirect ? percent : (100 - percent)) * this.AddPowerPercent;
					effectCount++;
				}
				bool flag2 = effectCount == 0;
				if (flag2)
				{
					this.PowerUpValue = 0;
				}
				else
				{
					this.PowerUpValue = totalPercent / effectCount;
				}
			}
			base.OnEnable(context);
		}
	}
}
