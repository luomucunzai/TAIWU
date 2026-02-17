using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.Item;
using GameData.Domains.SpecialEffect.EquipmentEffect;

namespace GameData.Domains.SpecialEffect.LegendaryBook.Sword
{
	// Token: 0x02000125 RID: 293
	public class HuaXi : EquipmentEffectBase
	{
		// Token: 0x06002A41 RID: 10817 RVA: 0x002022F0 File Offset: 0x002004F0
		public HuaXi()
		{
		}

		// Token: 0x06002A42 RID: 10818 RVA: 0x002022FA File Offset: 0x002004FA
		public HuaXi(int charId, ItemKey itemKey) : base(charId, itemKey, 40700, false)
		{
		}

		// Token: 0x06002A43 RID: 10819 RVA: 0x0020230C File Offset: 0x0020050C
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			base.CreateAffectedData(76, EDataModifyType.AddPercent, -1);
		}

		// Token: 0x06002A44 RID: 10820 RVA: 0x00202324 File Offset: 0x00200524
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || dataKey.FieldId != 76 || !base.IsCurrWeapon();
			int result;
			if (flag)
			{
				result = base.GetModifyValue(dataKey, currModifyValue);
			}
			else
			{
				result = ((dataKey.CustomParam1 == 1) ? 200 : 0);
			}
			return result;
		}

		// Token: 0x04000CEE RID: 3310
		private const int AddPursueOdds = 200;
	}
}
