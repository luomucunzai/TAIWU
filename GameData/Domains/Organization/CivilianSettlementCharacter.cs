using System;
using GameData.ArchiveData;
using GameData.Common;
using GameData.Serializer;

namespace GameData.Domains.Organization
{
	// Token: 0x02000640 RID: 1600
	[SerializableGameData(NotForDisplayModule = true)]
	public class CivilianSettlementCharacter : SettlementCharacter, ISerializableGameData
	{
		// Token: 0x06004674 RID: 18036 RVA: 0x002751DC File Offset: 0x002733DC
		public CivilianSettlementCharacter(int charId, sbyte orgTemplateId, short settlementId) : base(charId, orgTemplateId, settlementId)
		{
		}

		// Token: 0x06004675 RID: 18037 RVA: 0x002751EC File Offset: 0x002733EC
		public unsafe override void SetSettlementId(short settlementId, DataContext context)
		{
			this.SettlementId = settlementId;
			base.SetModifiedAndInvalidateInfluencedCache(2, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				byte* pData = OperationAdder.DynamicObjectCollection_SetFixedField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this.Id, 5U, 2);
				*(short*)pData = this.SettlementId;
				pData += 2;
			}
		}

		// Token: 0x06004676 RID: 18038 RVA: 0x0027524C File Offset: 0x0027344C
		public unsafe override void SetApprovedTaiwu(bool approvedTaiwu, DataContext context)
		{
			this.ApprovedTaiwu = approvedTaiwu;
			base.SetModifiedAndInvalidateInfluencedCache(3, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				byte* pData = OperationAdder.DynamicObjectCollection_SetFixedField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this.Id, 7U, 1);
				*pData = (this.ApprovedTaiwu ? 1 : 0);
				pData++;
			}
		}

		// Token: 0x06004677 RID: 18039 RVA: 0x002752AC File Offset: 0x002734AC
		public unsafe override void SetInfluencePower(short influencePower, DataContext context)
		{
			this.InfluencePower = influencePower;
			base.SetModifiedAndInvalidateInfluencedCache(4, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				byte* pData = OperationAdder.DynamicObjectCollection_SetFixedField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this.Id, 8U, 2);
				*(short*)pData = this.InfluencePower;
				pData += 2;
			}
		}

		// Token: 0x06004678 RID: 18040 RVA: 0x0027530C File Offset: 0x0027350C
		public unsafe override void SetInfluencePowerBonus(short influencePowerBonus, DataContext context)
		{
			this.InfluencePowerBonus = influencePowerBonus;
			base.SetModifiedAndInvalidateInfluencedCache(5, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				byte* pData = OperationAdder.DynamicObjectCollection_SetFixedField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this.Id, 10U, 2);
				*(short*)pData = this.InfluencePowerBonus;
				pData += 2;
			}
		}

		// Token: 0x06004679 RID: 18041 RVA: 0x0027536C File Offset: 0x0027356C
		public CivilianSettlementCharacter()
		{
		}

		// Token: 0x0600467A RID: 18042 RVA: 0x00275378 File Offset: 0x00273578
		public bool IsSerializedSizeFixed()
		{
			return true;
		}

		// Token: 0x0600467B RID: 18043 RVA: 0x0027538C File Offset: 0x0027358C
		public int GetSerializedSize()
		{
			return 12;
		}

		// Token: 0x0600467C RID: 18044 RVA: 0x002753A4 File Offset: 0x002735A4
		public unsafe int Serialize(byte* pData)
		{
			*(int*)pData = this.Id;
			byte* pCurrData = pData + 4;
			*pCurrData = (byte)this.OrgTemplateId;
			pCurrData++;
			*(short*)pCurrData = this.SettlementId;
			pCurrData += 2;
			*pCurrData = (this.ApprovedTaiwu ? 1 : 0);
			pCurrData++;
			*(short*)pCurrData = this.InfluencePower;
			pCurrData += 2;
			*(short*)pCurrData = this.InfluencePowerBonus;
			pCurrData += 2;
			return (int)((long)(pCurrData - pData));
		}

		// Token: 0x0600467D RID: 18045 RVA: 0x00275408 File Offset: 0x00273608
		public unsafe int Deserialize(byte* pData)
		{
			this.Id = *(int*)pData;
			byte* pCurrData = pData + 4;
			this.OrgTemplateId = *(sbyte*)pCurrData;
			pCurrData++;
			this.SettlementId = *(short*)pCurrData;
			pCurrData += 2;
			this.ApprovedTaiwu = (*pCurrData != 0);
			pCurrData++;
			this.InfluencePower = *(short*)pCurrData;
			pCurrData += 2;
			this.InfluencePowerBonus = *(short*)pCurrData;
			pCurrData += 2;
			return (int)((long)(pCurrData - pData));
		}

		// Token: 0x040014A2 RID: 5282
		public const int FixedSize = 12;

		// Token: 0x040014A3 RID: 5283
		public const int DynamicCount = 0;

		// Token: 0x02000A7C RID: 2684
		internal class FixedFieldInfos
		{
			// Token: 0x04002AED RID: 10989
			public const uint Id_Offset = 0U;

			// Token: 0x04002AEE RID: 10990
			public const int Id_Size = 4;

			// Token: 0x04002AEF RID: 10991
			public const uint OrgTemplateId_Offset = 4U;

			// Token: 0x04002AF0 RID: 10992
			public const int OrgTemplateId_Size = 1;

			// Token: 0x04002AF1 RID: 10993
			public const uint SettlementId_Offset = 5U;

			// Token: 0x04002AF2 RID: 10994
			public const int SettlementId_Size = 2;

			// Token: 0x04002AF3 RID: 10995
			public const uint ApprovedTaiwu_Offset = 7U;

			// Token: 0x04002AF4 RID: 10996
			public const int ApprovedTaiwu_Size = 1;

			// Token: 0x04002AF5 RID: 10997
			public const uint InfluencePower_Offset = 8U;

			// Token: 0x04002AF6 RID: 10998
			public const int InfluencePower_Size = 2;

			// Token: 0x04002AF7 RID: 10999
			public const uint InfluencePowerBonus_Offset = 10U;

			// Token: 0x04002AF8 RID: 11000
			public const int InfluencePowerBonus_Size = 2;
		}
	}
}
