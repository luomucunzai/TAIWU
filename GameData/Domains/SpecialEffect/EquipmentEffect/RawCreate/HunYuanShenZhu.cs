using System;
using GameData.Domains.Item;

namespace GameData.Domains.SpecialEffect.EquipmentEffect.RawCreate
{
	// Token: 0x02000196 RID: 406
	public class HunYuanShenZhu : RawCreateEquipmentBase
	{
		// Token: 0x17000123 RID: 291
		// (get) Token: 0x06002BC6 RID: 11206 RVA: 0x00205F80 File Offset: 0x00204180
		protected override int ReduceDurabilityValue
		{
			get
			{
				return 12;
			}
		}

		// Token: 0x06002BC7 RID: 11207 RVA: 0x00205F84 File Offset: 0x00204184
		public HunYuanShenZhu()
		{
		}

		// Token: 0x06002BC8 RID: 11208 RVA: 0x00205F8E File Offset: 0x0020418E
		public HunYuanShenZhu(int charId, ItemKey itemKey) : base(charId, itemKey, 30205)
		{
		}
	}
}
