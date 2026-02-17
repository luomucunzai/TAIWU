using System;
using GameData.Combat.Math;
using GameData.Common;

namespace GameData.Domains.SpecialEffect.Animal.Loong.Carrier
{
	// Token: 0x02000617 RID: 1559
	public class Qiuniu : CarrierEffectBase
	{
		// Token: 0x06004584 RID: 17796 RVA: 0x002729C8 File Offset: 0x00270BC8
		public Qiuniu(int charId) : base(charId)
		{
		}

		// Token: 0x170002E6 RID: 742
		// (get) Token: 0x06004585 RID: 17797 RVA: 0x002729D3 File Offset: 0x00270BD3
		protected override short CombatStateId
		{
			get
			{
				return 199;
			}
		}

		// Token: 0x06004586 RID: 17798 RVA: 0x002729DA File Offset: 0x00270BDA
		protected override void OnEnableSubClass(DataContext context)
		{
			base.OnEnableSubClass(context);
			base.CreateAffectedData(277, EDataModifyType.Add, -1);
			base.CreateAffectedData(278, EDataModifyType.Add, -1);
		}

		// Token: 0x06004587 RID: 17799 RVA: 0x00272A04 File Offset: 0x00270C04
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.CharId != base.CharacterId;
			bool flag2 = flag;
			if (!flag2)
			{
				ushort fieldId = dataKey.FieldId;
				bool flag3 = fieldId - 277 <= 1;
				flag2 = !flag3;
			}
			bool flag4 = flag2;
			int result;
			if (flag4)
			{
				result = 0;
			}
			else
			{
				sbyte hitType = (sbyte)dataKey.CustomParam0;
				bool flag5 = hitType != 3;
				if (flag5)
				{
					result = 0;
				}
				else
				{
					int currentValue = dataKey.CustomParam1;
					result = currentValue * Qiuniu.AddOtherPercent;
				}
			}
			return result;
		}

		// Token: 0x0400148D RID: 5261
		private static readonly CValuePercent AddOtherPercent = 33;
	}
}
