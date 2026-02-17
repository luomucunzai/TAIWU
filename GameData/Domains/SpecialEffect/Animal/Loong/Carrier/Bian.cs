using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;

namespace GameData.Domains.SpecialEffect.Animal.Loong.Carrier
{
	// Token: 0x02000612 RID: 1554
	public class Bian : CarrierEffectBase
	{
		// Token: 0x0600456D RID: 17773 RVA: 0x00272504 File Offset: 0x00270704
		public Bian(int charId) : base(charId)
		{
		}

		// Token: 0x170002E1 RID: 737
		// (get) Token: 0x0600456E RID: 17774 RVA: 0x0027250F File Offset: 0x0027070F
		protected override short CombatStateId
		{
			get
			{
				return 205;
			}
		}

		// Token: 0x0600456F RID: 17775 RVA: 0x00272516 File Offset: 0x00270716
		protected override void OnEnableSubClass(DataContext context)
		{
			base.CreateAffectedData(64, EDataModifyType.AddPercent, -1);
			base.CreateAffectedData(65, EDataModifyType.AddPercent, -1);
			base.CreateAffectedData(100, EDataModifyType.AddPercent, -1);
			base.CreateAffectedData(101, EDataModifyType.AddPercent, -1);
		}

		// Token: 0x06004570 RID: 17776 RVA: 0x00272548 File Offset: 0x00270748
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.CharId != base.CharacterId;
			bool flag2 = flag;
			if (!flag2)
			{
				ushort fieldId = dataKey.FieldId;
				bool flag3 = fieldId - 64 <= 1 || fieldId - 100 <= 1;
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
				sbyte fameType = base.CurrEnemyChar.GetCharacter().GetFameType();
				int addPercent;
				result = (Bian.FameTypeToAddPercent.TryGetValue(fameType, out addPercent) ? addPercent : 0);
			}
			return result;
		}

		// Token: 0x04001487 RID: 5255
		private static readonly IReadOnlyDictionary<sbyte, int> FameTypeToAddPercent = new Dictionary<sbyte, int>
		{
			{
				2,
				15
			},
			{
				1,
				30
			},
			{
				0,
				45
			}
		};
	}
}
