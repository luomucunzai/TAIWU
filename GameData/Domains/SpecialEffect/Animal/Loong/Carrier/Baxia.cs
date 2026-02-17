using System;
using GameData.Combat.Math;
using GameData.Common;

namespace GameData.Domains.SpecialEffect.Animal.Loong.Carrier
{
	// Token: 0x02000611 RID: 1553
	public class Baxia : CarrierEffectBase
	{
		// Token: 0x06004568 RID: 17768 RVA: 0x0027241F File Offset: 0x0027061F
		public Baxia(int charId) : base(charId)
		{
		}

		// Token: 0x170002E0 RID: 736
		// (get) Token: 0x06004569 RID: 17769 RVA: 0x0027242A File Offset: 0x0027062A
		protected override short CombatStateId
		{
			get
			{
				return 204;
			}
		}

		// Token: 0x0600456A RID: 17770 RVA: 0x00272431 File Offset: 0x00270631
		protected override void OnEnableSubClass(DataContext context)
		{
			base.CreateAffectedData(27, EDataModifyType.Add, -1);
			base.CreateAffectedData(30, EDataModifyType.Add, -1);
			base.CreateAffectedData(279, EDataModifyType.Custom, -1);
		}

		// Token: 0x0600456B RID: 17771 RVA: 0x00272458 File Offset: 0x00270658
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.CharId != base.CharacterId;
			bool flag2 = flag;
			if (!flag2)
			{
				ushort fieldId = dataKey.FieldId;
				bool flag3 = fieldId == 27 || fieldId == 30;
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
				int weight = this.CharObj.GetCurrEquipmentLoad();
				result = weight / 500;
			}
			return result;
		}

		// Token: 0x0600456C RID: 17772 RVA: 0x002724C8 File Offset: 0x002706C8
		public override bool GetModifiedValue(AffectedDataKey dataKey, bool dataValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || dataKey.FieldId != 279;
			return !flag || dataValue;
		}

		// Token: 0x04001485 RID: 5253
		private const int WeightUnit = 500;

		// Token: 0x04001486 RID: 5254
		private const int MaxPowerUnit = 1;
	}
}
