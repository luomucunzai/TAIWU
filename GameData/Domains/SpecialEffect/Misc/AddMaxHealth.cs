using System;
using GameData.Combat.Math;
using GameData.Common;

namespace GameData.Domains.SpecialEffect.Misc
{
	// Token: 0x02000117 RID: 279
	public class AddMaxHealth : SpecialEffectBase
	{
		// Token: 0x06002A15 RID: 10773 RVA: 0x00201D37 File Offset: 0x001FFF37
		public AddMaxHealth()
		{
		}

		// Token: 0x06002A16 RID: 10774 RVA: 0x00201D41 File Offset: 0x001FFF41
		public AddMaxHealth(int charId, int addFinalHealth) : base(charId, 1000001)
		{
			this._addFinalHealth = addFinalHealth;
		}

		// Token: 0x06002A17 RID: 10775 RVA: 0x00201D58 File Offset: 0x001FFF58
		public override void OnEnable(DataContext context)
		{
			base.CreateAffectedData(53, EDataModifyType.Add, -1);
		}

		// Token: 0x06002A18 RID: 10776 RVA: 0x00201D68 File Offset: 0x001FFF68
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || dataKey.FieldId != 53;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				result = this._addFinalHealth;
			}
			return result;
		}

		// Token: 0x06002A19 RID: 10777 RVA: 0x00201DA8 File Offset: 0x001FFFA8
		protected override int GetSubClassSerializedSize()
		{
			return 4;
		}

		// Token: 0x06002A1A RID: 10778 RVA: 0x00201DBC File Offset: 0x001FFFBC
		protected unsafe override int SerializeSubClass(byte* pData)
		{
			*(int*)pData = this._addFinalHealth;
			byte* pCurrData = pData + 4;
			return (int)((long)(pCurrData - pData));
		}

		// Token: 0x06002A1B RID: 10779 RVA: 0x00201DE4 File Offset: 0x001FFFE4
		protected unsafe override int DeserializeSubClass(byte* pData)
		{
			this._addFinalHealth = *(int*)pData;
			byte* pCurrData = pData + 4;
			return (int)((long)(pCurrData - pData));
		}

		// Token: 0x04000CE7 RID: 3303
		private int _addFinalHealth;
	}
}
