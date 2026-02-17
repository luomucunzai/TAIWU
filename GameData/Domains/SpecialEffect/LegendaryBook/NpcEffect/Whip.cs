using System;
using System.Collections.Generic;
using Config;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.Item;

namespace GameData.Domains.SpecialEffect.LegendaryBook.NpcEffect
{
	// Token: 0x02000150 RID: 336
	public class Whip : FeatureEffectBase
	{
		// Token: 0x06002ADB RID: 10971 RVA: 0x00204099 File Offset: 0x00202299
		public Whip()
		{
		}

		// Token: 0x06002ADC RID: 10972 RVA: 0x002040A3 File Offset: 0x002022A3
		public Whip(int charId, short featureId) : base(charId, featureId, 41411)
		{
		}

		// Token: 0x06002ADD RID: 10973 RVA: 0x002040B4 File Offset: 0x002022B4
		public override void OnEnable(DataContext context)
		{
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 69, -1, -1, -1, -1), EDataModifyType.AddPercent);
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 29, -1, -1, -1, -1), EDataModifyType.Add);
		}

		// Token: 0x06002ADE RID: 10974 RVA: 0x0020410C File Offset: 0x0020230C
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
					bool flag3 = base.CombatChar.GetAutoCastingSkill() || dataKey.CombatSkillId < 0 || CombatSkill.Instance[dataKey.CombatSkillId].Type != 11;
					if (flag3)
					{
						result = 0;
					}
					else
					{
						int addDamage = 180 * (30000 - base.CurrEnemyChar.GetBreathValue()) / 30000;
						result = Math.Min(addDamage, 180);
					}
				}
				else
				{
					bool flag4 = dataKey.FieldId == 29;
					if (flag4)
					{
						short subType = ItemTemplateHelper.GetItemSubType((sbyte)dataKey.CustomParam0, (short)dataKey.CustomParam1);
						bool flag5 = !CombatSkillType.Instance[11].LegendaryBookWeaponSlotItemSubTypes.Contains(subType);
						if (flag5)
						{
							result = 0;
						}
						else
						{
							result = 10;
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

		// Token: 0x04000D24 RID: 3364
		private const short MaxAddDamage = 180;

		// Token: 0x04000D25 RID: 3365
		private const sbyte AddAttackRange = 10;
	}
}
