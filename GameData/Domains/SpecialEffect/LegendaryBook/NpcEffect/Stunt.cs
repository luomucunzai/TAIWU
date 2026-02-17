using System;
using System.Collections.Generic;
using Config;
using GameData.Combat.Math;
using GameData.Common;

namespace GameData.Domains.SpecialEffect.LegendaryBook.NpcEffect
{
	// Token: 0x0200014D RID: 333
	public class Stunt : FeatureEffectBase
	{
		// Token: 0x06002ACE RID: 10958 RVA: 0x00203D16 File Offset: 0x00201F16
		public Stunt()
		{
		}

		// Token: 0x06002ACF RID: 10959 RVA: 0x00203D20 File Offset: 0x00201F20
		public Stunt(int charId, short featureId) : base(charId, featureId, 41402)
		{
		}

		// Token: 0x06002AD0 RID: 10960 RVA: 0x00203D31 File Offset: 0x00201F31
		public override void OnEnable(DataContext context)
		{
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 199, -1, -1, -1, -1), EDataModifyType.AddPercent);
		}

		// Token: 0x06002AD1 RID: 10961 RVA: 0x00203D60 File Offset: 0x00201F60
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
				sbyte equipType = CombatSkill.Instance[dataKey.CombatSkillId].EquipType;
				bool flag2 = equipType != 3 && equipType != 4;
				if (flag2)
				{
					result = 0;
				}
				else
				{
					bool flag3 = dataKey.FieldId == 199;
					if (flag3)
					{
						result = 100;
					}
					else
					{
						result = 0;
					}
				}
			}
			return result;
		}

		// Token: 0x04000D1C RID: 3356
		private const short AddPowerPercent = 100;
	}
}
