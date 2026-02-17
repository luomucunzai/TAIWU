using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.Character;
using GameData.Domains.CombatSkill;
using GameData.Domains.Item;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.Kongsangpai.Neigong
{
	// Token: 0x0200047F RID: 1151
	public class ChaiShanQingNangJue : CombatSkillEffectBase
	{
		// Token: 0x06003BA0 RID: 15264 RVA: 0x00248E7F File Offset: 0x0024707F
		public ChaiShanQingNangJue()
		{
		}

		// Token: 0x06003BA1 RID: 15265 RVA: 0x00248E89 File Offset: 0x00247089
		public ChaiShanQingNangJue(CombatSkillKey skillKey) : base(skillKey, 10002, -1)
		{
		}

		// Token: 0x06003BA2 RID: 15266 RVA: 0x00248E9C File Offset: 0x0024709C
		public unsafe override void OnEnable(DataContext context)
		{
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, base.IsDirect ? 102 : 69, -1, -1, -1, -1), EDataModifyType.TotalPercent);
			ref EatingItems eatingItems = ref this.CharObj.GetEatingItems();
			List<sbyte> itemGradeList = ObjectPool<List<sbyte>>.Instance.Get();
			this._damageChangePercent = 0;
			itemGradeList.Clear();
			for (int i = 0; i < 9; i++)
			{
				ItemKey itemKey = (ItemKey)(*(ref eatingItems.ItemKeys.FixedElementField + (IntPtr)i * 8));
				bool flag = itemKey.IsValid() && ItemTemplateHelper.GetItemSubType(itemKey.ItemType, itemKey.TemplateId) == 800;
				if (flag)
				{
					itemGradeList.Add(ItemTemplateHelper.GetGrade(itemKey.ItemType, itemKey.TemplateId));
				}
			}
			itemGradeList.Sort();
			int itemCount = Math.Min(itemGradeList.Count, 5);
			for (int j = itemGradeList.Count - itemCount; j < itemGradeList.Count; j++)
			{
				this._damageChangePercent = this._damageChangePercent + (int)itemGradeList[j] + 1;
			}
		}

		// Token: 0x06003BA3 RID: 15267 RVA: 0x00248FC4 File Offset: 0x002471C4
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
				bool flag2 = dataKey.FieldId == 69;
				if (flag2)
				{
					result = this._damageChangePercent;
				}
				else
				{
					bool flag3 = dataKey.FieldId == 102;
					if (flag3)
					{
						result = -this._damageChangePercent;
					}
					else
					{
						result = 0;
					}
				}
			}
			return result;
		}

		// Token: 0x04001176 RID: 4470
		private const sbyte MaxItemCount = 5;

		// Token: 0x04001177 RID: 4471
		private int _damageChangePercent;
	}
}
