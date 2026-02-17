using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.Item;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.EquipmentEffect.RawCreate
{
	// Token: 0x0200019B RID: 411
	public class YanXiaShenZhu : RawCreateEquipmentBase
	{
		// Token: 0x1700012A RID: 298
		// (get) Token: 0x06002BE1 RID: 11233 RVA: 0x0020630F File Offset: 0x0020450F
		protected override int ReduceDurabilityValue
		{
			get
			{
				return 16;
			}
		}

		// Token: 0x06002BE2 RID: 11234 RVA: 0x00206313 File Offset: 0x00204513
		public YanXiaShenZhu()
		{
		}

		// Token: 0x06002BE3 RID: 11235 RVA: 0x0020631D File Offset: 0x0020451D
		public YanXiaShenZhu(int charId, ItemKey itemKey) : base(charId, itemKey, 30200)
		{
		}

		// Token: 0x06002BE4 RID: 11236 RVA: 0x0020632E File Offset: 0x0020452E
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			base.CreateAffectedData(308, EDataModifyType.AddPercent, -1);
			base.CreateAffectedData(291, EDataModifyType.Custom, -1);
		}

		// Token: 0x06002BE5 RID: 11237 RVA: 0x00206358 File Offset: 0x00204558
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || dataKey.FieldId != 308 || !base.CanAffect;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				int bodyPart = dataKey.CustomParam0;
				bool flag2 = bodyPart < 0 || base.CombatChar.Armors[bodyPart] != this.EquipItemKey;
				if (flag2)
				{
					result = 0;
				}
				else
				{
					result = 100;
				}
			}
			return result;
		}

		// Token: 0x06002BE6 RID: 11238 RVA: 0x002063D0 File Offset: 0x002045D0
		public override bool GetModifiedValue(AffectedDataKey dataKey, bool dataValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || dataKey.FieldId != 291 || !dataKey.IsNormalAttack || !base.CanAffect;
			bool result;
			if (flag)
			{
				result = dataValue;
			}
			else
			{
				int bodyPart = dataKey.CustomParam1;
				bool flag2 = bodyPart < 0 || base.CombatChar.Armors[bodyPart] != this.EquipItemKey;
				if (flag2)
				{
					result = dataValue;
				}
				else
				{
					result = (dataValue || DomainManager.Combat.Context.Random.CheckPercentProb(33));
				}
			}
			return result;
		}

		// Token: 0x04000D51 RID: 3409
		private const int AddAttackOdds = 100;

		// Token: 0x04000D52 RID: 3410
		private const int InevitableAvoidOdds = 33;
	}
}
