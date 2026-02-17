using System;
using GameData.ArchiveData;
using GameData.Common;
using GameData.Serializer;

namespace GameData.Domains.Organization
{
	// Token: 0x02000646 RID: 1606
	[SerializableGameData(NotForDisplayModule = true)]
	public class SectCharacter : SettlementCharacter, ISerializableGameData
	{
		// Token: 0x060047CB RID: 18379 RVA: 0x0028863B File Offset: 0x0028683B
		public SectCharacter(int charId, sbyte orgTemplateId, short settlementId) : base(charId, orgTemplateId, settlementId)
		{
		}

		// Token: 0x060047CC RID: 18380 RVA: 0x00288648 File Offset: 0x00286848
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

		// Token: 0x060047CD RID: 18381 RVA: 0x002886A8 File Offset: 0x002868A8
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

		// Token: 0x060047CE RID: 18382 RVA: 0x00288708 File Offset: 0x00286908
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

		// Token: 0x060047CF RID: 18383 RVA: 0x00288768 File Offset: 0x00286968
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

		// Token: 0x060047D0 RID: 18384 RVA: 0x002887C8 File Offset: 0x002869C8
		public SectCharacter()
		{
		}

		// Token: 0x060047D1 RID: 18385 RVA: 0x002887D4 File Offset: 0x002869D4
		public bool IsSerializedSizeFixed()
		{
			return true;
		}

		// Token: 0x060047D2 RID: 18386 RVA: 0x002887E8 File Offset: 0x002869E8
		public int GetSerializedSize()
		{
			return 12;
		}

		// Token: 0x060047D3 RID: 18387 RVA: 0x00288800 File Offset: 0x00286A00
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

		// Token: 0x060047D4 RID: 18388 RVA: 0x00288864 File Offset: 0x00286A64
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

		// Token: 0x040014F5 RID: 5365
		public const int FixedSize = 12;

		// Token: 0x040014F6 RID: 5366
		public const int DynamicCount = 0;

		// Token: 0x02000A86 RID: 2694
		internal class FixedFieldInfos
		{
			// Token: 0x04002B35 RID: 11061
			public const uint Id_Offset = 0U;

			// Token: 0x04002B36 RID: 11062
			public const int Id_Size = 4;

			// Token: 0x04002B37 RID: 11063
			public const uint OrgTemplateId_Offset = 4U;

			// Token: 0x04002B38 RID: 11064
			public const int OrgTemplateId_Size = 1;

			// Token: 0x04002B39 RID: 11065
			public const uint SettlementId_Offset = 5U;

			// Token: 0x04002B3A RID: 11066
			public const int SettlementId_Size = 2;

			// Token: 0x04002B3B RID: 11067
			public const uint ApprovedTaiwu_Offset = 7U;

			// Token: 0x04002B3C RID: 11068
			public const int ApprovedTaiwu_Size = 1;

			// Token: 0x04002B3D RID: 11069
			public const uint InfluencePower_Offset = 8U;

			// Token: 0x04002B3E RID: 11070
			public const int InfluencePower_Size = 2;

			// Token: 0x04002B3F RID: 11071
			public const uint InfluencePowerBonus_Offset = 10U;

			// Token: 0x04002B40 RID: 11072
			public const int InfluencePowerBonus_Size = 2;
		}
	}
}
