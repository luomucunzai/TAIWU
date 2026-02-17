using System;
using System.Collections.Generic;
using Config;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.Item;

namespace GameData.Domains.SpecialEffect.LegendaryBook.NpcEffect
{
	// Token: 0x02000145 RID: 325
	public class ControllableShot : FeatureEffectBase
	{
		// Token: 0x06002AA1 RID: 10913 RVA: 0x00203054 File Offset: 0x00201254
		public ControllableShot()
		{
		}

		// Token: 0x06002AA2 RID: 10914 RVA: 0x0020305E File Offset: 0x0020125E
		public ControllableShot(int charId, short featureId) : base(charId, featureId, 41412)
		{
		}

		// Token: 0x06002AA3 RID: 10915 RVA: 0x00203070 File Offset: 0x00201270
		public override void OnEnable(DataContext context)
		{
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 69, -1, -1, -1, -1), EDataModifyType.AddPercent);
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 76, -1, -1, -1, -1), EDataModifyType.TotalPercent);
		}

		// Token: 0x06002AA4 RID: 10916 RVA: 0x002030C8 File Offset: 0x002012C8
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
					bool flag3 = base.CombatChar.GetAutoCastingSkill() || dataKey.CombatSkillId < 0 || CombatSkill.Instance[dataKey.CombatSkillId].Type != 12;
					if (flag3)
					{
						result = 0;
					}
					else
					{
						int addDamage = (int)(15 * base.CombatChar.GetChangeTrickCount());
						result = Math.Min(addDamage, 180);
					}
				}
				else
				{
					bool flag4 = dataKey.FieldId == 76;
					if (flag4)
					{
						ItemKey currWeapon = DomainManager.Combat.GetUsingWeaponKey(base.CombatChar);
						short subType = ItemTemplateHelper.GetItemSubType(currWeapon.ItemType, currWeapon.TemplateId);
						bool flag5 = !base.CombatChar.GetChangeTrickAttack() || !CombatSkillType.Instance[12].LegendaryBookWeaponSlotItemSubTypes.Contains(subType);
						if (flag5)
						{
							result = 0;
						}
						else
						{
							result = 100;
						}
					}
					else
					{
						result = 0;
					}
				}
			}
			return result;
		}

		// Token: 0x04000D01 RID: 3329
		private const short AddDamageUnit = 15;

		// Token: 0x04000D02 RID: 3330
		private const short MaxAddDamage = 180;

		// Token: 0x04000D03 RID: 3331
		private const short AddPursueOddsPercent = 100;
	}
}
