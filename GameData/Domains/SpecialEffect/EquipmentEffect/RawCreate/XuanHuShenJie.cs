using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.Item;

namespace GameData.Domains.SpecialEffect.EquipmentEffect.RawCreate
{
	// Token: 0x0200019A RID: 410
	public class XuanHuShenJie : RawCreateEquipmentBase
	{
		// Token: 0x17000129 RID: 297
		// (get) Token: 0x06002BDC RID: 11228 RVA: 0x00206277 File Offset: 0x00204477
		protected override int ReduceDurabilityValue
		{
			get
			{
				return 8;
			}
		}

		// Token: 0x06002BDD RID: 11229 RVA: 0x0020627A File Offset: 0x0020447A
		public XuanHuShenJie()
		{
		}

		// Token: 0x06002BDE RID: 11230 RVA: 0x00206284 File Offset: 0x00204484
		public XuanHuShenJie(int charId, ItemKey itemKey) : base(charId, itemKey, 30203)
		{
		}

		// Token: 0x06002BDF RID: 11231 RVA: 0x00206295 File Offset: 0x00204495
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			base.CreateAffectedData(69, EDataModifyType.AddPercent, -1);
		}

		// Token: 0x06002BE0 RID: 11232 RVA: 0x002062AC File Offset: 0x002044AC
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || dataKey.FieldId != 69 || !base.CanAffect;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				bool flag2 = DomainManager.Combat.GetUsingWeaponKey(base.CombatChar) != this.EquipItemKey;
				if (flag2)
				{
					result = 0;
				}
				else
				{
					result = 50;
				}
			}
			return result;
		}

		// Token: 0x04000D50 RID: 3408
		private const int AddDirectDamagePercent = 50;
	}
}
