using System;
using System.Collections.Generic;
using Config;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.Item;

namespace GameData.Domains.SpecialEffect.LegendaryBook.NpcEffect
{
	// Token: 0x0200014F RID: 335
	public class Throw : FeatureEffectBase
	{
		// Token: 0x06002AD6 RID: 10966 RVA: 0x00203EFD File Offset: 0x002020FD
		public Throw()
		{
		}

		// Token: 0x06002AD7 RID: 10967 RVA: 0x00203F07 File Offset: 0x00202107
		public Throw(int charId, short featureId) : base(charId, featureId, 41406)
		{
		}

		// Token: 0x06002AD8 RID: 10968 RVA: 0x00203F18 File Offset: 0x00202118
		public override void OnEnable(DataContext context)
		{
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 69, -1, -1, -1, -1), EDataModifyType.AddPercent);
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 281, -1, -1, -1, -1), EDataModifyType.Custom);
		}

		// Token: 0x06002AD9 RID: 10969 RVA: 0x00203F70 File Offset: 0x00202170
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || dataKey.CombatSkillId < 0 || base.CombatChar.GetAutoCastingSkill() || CombatSkill.Instance[dataKey.CombatSkillId].Type != 6;
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
					short distance = DomainManager.Combat.GetCurrentDistance();
					int addDamage = (int)((distance < 50) ? 0 : (40 + (distance - 50) / 10 * 20));
					result = Math.Min(addDamage, 180);
				}
				else
				{
					result = 0;
				}
			}
			return result;
		}

		// Token: 0x06002ADA RID: 10970 RVA: 0x00204010 File Offset: 0x00202210
		public override bool GetModifiedValue(AffectedDataKey dataKey, bool dataValue)
		{
			bool flag = dataKey.CharId != base.CharacterId;
			bool result;
			if (flag)
			{
				result = dataValue;
			}
			else
			{
				ItemKey currWeapon = DomainManager.Combat.GetUsingWeaponKey(base.CombatChar);
				short subType = ItemTemplateHelper.GetItemSubType(currWeapon.ItemType, currWeapon.TemplateId);
				bool flag2 = !CombatSkillType.Instance[6].LegendaryBookWeaponSlotItemSubTypes.Contains(subType);
				if (flag2)
				{
					result = dataValue;
				}
				else
				{
					bool flag3 = dataKey.FieldId == 281;
					result = (flag3 || dataValue);
				}
			}
			return result;
		}

		// Token: 0x04000D20 RID: 3360
		private const short MinDistance = 50;

		// Token: 0x04000D21 RID: 3361
		private const short BaseAddDamage = 40;

		// Token: 0x04000D22 RID: 3362
		private const short AddDamageUnit = 20;

		// Token: 0x04000D23 RID: 3363
		private const short MaxAddDamage = 180;
	}
}
