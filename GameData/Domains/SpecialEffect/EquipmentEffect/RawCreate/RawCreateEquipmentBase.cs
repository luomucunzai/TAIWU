using System;
using System.Collections.Generic;
using GameData.Common;
using GameData.Domains.Item;

namespace GameData.Domains.SpecialEffect.EquipmentEffect.RawCreate
{
	// Token: 0x02000197 RID: 407
	public abstract class RawCreateEquipmentBase : EquipmentEffectBase
	{
		// Token: 0x17000124 RID: 292
		// (get) Token: 0x06002BC9 RID: 11209 RVA: 0x00205F9F File Offset: 0x0020419F
		protected bool CanAffect
		{
			get
			{
				return base.CombatChar.GetRawCreateCollection().Contains(this.EquipItemKey);
			}
		}

		// Token: 0x17000125 RID: 293
		// (get) Token: 0x06002BCA RID: 11210
		protected abstract int ReduceDurabilityValue { get; }

		// Token: 0x06002BCB RID: 11211 RVA: 0x00205FB7 File Offset: 0x002041B7
		protected RawCreateEquipmentBase()
		{
		}

		// Token: 0x06002BCC RID: 11212 RVA: 0x00205FC1 File Offset: 0x002041C1
		protected RawCreateEquipmentBase(int charId, ItemKey itemKey, int type) : base(charId, itemKey, type)
		{
			this.EquipItemKey = itemKey;
		}

		// Token: 0x06002BCD RID: 11213 RVA: 0x00205FD5 File Offset: 0x002041D5
		protected override IEnumerable<int> CalcFrameCounterPeriods()
		{
			yield return 600;
			yield break;
		}

		// Token: 0x06002BCE RID: 11214 RVA: 0x00205FE5 File Offset: 0x002041E5
		public override bool IsOn(int counterType)
		{
			return this.CanAffect;
		}

		// Token: 0x06002BCF RID: 11215 RVA: 0x00205FF0 File Offset: 0x002041F0
		public override void OnProcess(DataContext context, int counterType)
		{
			bool flag = counterType != 0;
			if (!flag)
			{
				base.ChangeDurability(context, base.CombatChar, this.EquipItemKey, -this.ReduceDurabilityValue);
				ItemBase baseItem = DomainManager.Item.GetBaseItem(this.EquipItemKey);
				bool flag2 = baseItem.GetCurrDurability() > 0;
				if (!flag2)
				{
					base.CombatChar.RevertRawCreate(context, this.EquipItemKey);
				}
			}
		}

		// Token: 0x04000D4F RID: 3407
		private const int ReduceDurabilityFrame = 600;
	}
}
