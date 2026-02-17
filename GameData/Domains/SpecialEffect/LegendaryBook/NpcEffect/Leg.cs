using System;
using System.Collections.Generic;
using Config;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.Combat;

namespace GameData.Domains.SpecialEffect.LegendaryBook.NpcEffect
{
	// Token: 0x02000148 RID: 328
	public class Leg : FeatureEffectBase
	{
		// Token: 0x06002AB1 RID: 10929 RVA: 0x002034D9 File Offset: 0x002016D9
		public Leg()
		{
		}

		// Token: 0x06002AB2 RID: 10930 RVA: 0x002034E3 File Offset: 0x002016E3
		public Leg(int charId, short featureId) : base(charId, featureId, 41405)
		{
		}

		// Token: 0x06002AB3 RID: 10931 RVA: 0x002034F4 File Offset: 0x002016F4
		public override void OnEnable(DataContext context)
		{
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 69, -1, -1, -1, -1), EDataModifyType.AddPercent);
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 88, -1, -1, -1, -1), EDataModifyType.Custom);
		}

		// Token: 0x06002AB4 RID: 10932 RVA: 0x0020354C File Offset: 0x0020174C
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || dataKey.CombatSkillId < 0 || base.CombatChar.GetAutoCastingSkill() || CombatSkill.Instance[dataKey.CombatSkillId].Type != 5;
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
					int mobilityPercent = base.CurrEnemyChar.GetMobilityValue() * 100 / MoveSpecialConstants.MaxMobility;
					int addDamage = (mobilityPercent < 50) ? 180 : (180 * (100 - mobilityPercent) / 50);
					result = Math.Min(addDamage, 180);
				}
				else
				{
					result = 0;
				}
			}
			return result;
		}

		// Token: 0x06002AB5 RID: 10933 RVA: 0x002035F8 File Offset: 0x002017F8
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
				bool flag2 = dataKey.FieldId == 88;
				result = (!flag2 && dataValue);
			}
			return result;
		}

		// Token: 0x04000D0C RID: 3340
		private const short MaxDamageMobilityPercent = 50;

		// Token: 0x04000D0D RID: 3341
		private const short MaxAddDamage = 180;
	}
}
