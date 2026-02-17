using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.Character;
using GameData.Domains.Item;

namespace GameData.Domains.SpecialEffect.EquipmentEffect.RawCreate
{
	// Token: 0x02000195 RID: 405
	public class GuiYanLingZhuo : RawCreateEquipmentBase
	{
		// Token: 0x17000122 RID: 290
		// (get) Token: 0x06002BBE RID: 11198 RVA: 0x00205E73 File Offset: 0x00204073
		protected override int ReduceDurabilityValue
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x06002BBF RID: 11199 RVA: 0x00205E76 File Offset: 0x00204076
		public GuiYanLingZhuo()
		{
		}

		// Token: 0x06002BC0 RID: 11200 RVA: 0x00205E80 File Offset: 0x00204080
		public GuiYanLingZhuo(int charId, ItemKey itemKey) : base(charId, itemKey, 30202)
		{
		}

		// Token: 0x06002BC1 RID: 11201 RVA: 0x00205E91 File Offset: 0x00204091
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			base.CreateAffectedData(310, EDataModifyType.AddPercent, -1);
		}

		// Token: 0x06002BC2 RID: 11202 RVA: 0x00205EAA File Offset: 0x002040AA
		protected override IEnumerable<int> CalcFrameCounterPeriods()
		{
			foreach (int period in base.CalcFrameCounterPeriods())
			{
				yield return period;
			}
			IEnumerator<int> enumerator = null;
			yield return 240;
			yield break;
			yield break;
		}

		// Token: 0x06002BC3 RID: 11203 RVA: 0x00205EBC File Offset: 0x002040BC
		public override void OnProcess(DataContext context, int counterType)
		{
			base.OnProcess(context, counterType);
			bool flag = counterType == 1;
			if (flag)
			{
				base.CombatChar.ChangeNeiliAllocationRandom(context, 1, 1, true);
			}
		}

		// Token: 0x06002BC4 RID: 11204 RVA: 0x00205EEC File Offset: 0x002040EC
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || dataKey.FieldId != 310 || !base.CanAffect;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				bool flag2 = dataKey.CustomParam0 != 2 || dataKey.CustomParam1 != this.EquipItemKey.Id;
				if (flag2)
				{
					result = 0;
				}
				else
				{
					ECharacterPropertyReferencedType propertyType = (ECharacterPropertyReferencedType)dataKey.CustomParam2;
					bool flag3 = !Character.CombatPropertyTypes.Contains(propertyType);
					if (flag3)
					{
						result = 0;
					}
					else
					{
						result = 100;
					}
				}
			}
			return result;
		}

		// Token: 0x04000D4C RID: 3404
		private const int AddEquipmentBonus = 100;

		// Token: 0x04000D4D RID: 3405
		private const int ChangeNeiliAllocationUnit = 1;

		// Token: 0x04000D4E RID: 3406
		private const int AffectFrame = 240;
	}
}
