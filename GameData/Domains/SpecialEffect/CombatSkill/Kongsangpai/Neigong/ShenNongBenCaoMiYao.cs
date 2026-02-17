using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Kongsangpai.Neigong
{
	// Token: 0x02000483 RID: 1155
	public class ShenNongBenCaoMiYao : CombatSkillEffectBase
	{
		// Token: 0x06003BAD RID: 15277 RVA: 0x0024910E File Offset: 0x0024730E
		public ShenNongBenCaoMiYao()
		{
		}

		// Token: 0x06003BAE RID: 15278 RVA: 0x00249118 File Offset: 0x00247318
		public ShenNongBenCaoMiYao(CombatSkillKey skillKey) : base(skillKey, 10001, -1)
		{
		}

		// Token: 0x06003BAF RID: 15279 RVA: 0x0024912C File Offset: 0x0024732C
		public override void OnEnable(DataContext context)
		{
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			bool isDirect = base.IsDirect;
			if (isDirect)
			{
				this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 118, -1, -1, -1, -1), EDataModifyType.AddPercent);
				this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 119, -1, -1, -1, -1), EDataModifyType.AddPercent);
				this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 121, -1, -1, -1, -1), EDataModifyType.AddPercent);
				this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 122, -1, -1, -1, -1), EDataModifyType.AddPercent);
			}
			else
			{
				foreach (int enemyId in DomainManager.Combat.GetCharacterList(!base.CombatChar.IsAlly))
				{
					bool flag = enemyId < 0;
					if (!flag)
					{
						this.AffectDatas.Add(new AffectedDataKey(enemyId, 118, -1, -1, -1, -1), EDataModifyType.AddPercent);
						this.AffectDatas.Add(new AffectedDataKey(enemyId, 120, -1, -1, -1, -1), EDataModifyType.AddPercent);
						this.AffectDatas.Add(new AffectedDataKey(enemyId, 121, -1, -1, -1, -1), EDataModifyType.AddPercent);
						this.AffectDatas.Add(new AffectedDataKey(enemyId, 123, -1, -1, -1, -1), EDataModifyType.AddPercent);
					}
				}
			}
		}

		// Token: 0x06003BB0 RID: 15280 RVA: 0x00249278 File Offset: 0x00247478
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = !base.IsDirect && !base.IsCurrent;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				ushort fieldId = dataKey.FieldId;
				bool flag2 = fieldId == 118 || fieldId == 121;
				bool flag3 = flag2;
				if (flag3)
				{
					base.ShowSpecialEffectTips(dataKey.FieldId == 118, 0, 2);
					result = (base.IsDirect ? 30 : -30);
				}
				else
				{
					fieldId = dataKey.FieldId;
					flag2 = (fieldId - 119 <= 1 || fieldId - 122 <= 1);
					bool flag4 = flag2;
					if (flag4)
					{
						bool flag5 = dataKey.CustomParam0 == 0;
						if (flag5)
						{
							fieldId = dataKey.FieldId;
							flag2 = (fieldId - 119 <= 1);
							base.ShowSpecialEffectTips(flag2, 1, 3);
						}
						fieldId = dataKey.FieldId;
						flag2 = (fieldId == 119 || fieldId == 122);
						result = (flag2 ? 30 : -30);
					}
					else
					{
						result = 0;
					}
				}
			}
			return result;
		}

		// Token: 0x0400117A RID: 4474
		private const sbyte SpeedChangePercent = 30;

		// Token: 0x0400117B RID: 4475
		private const sbyte HealEffectChange = 30;
	}
}
