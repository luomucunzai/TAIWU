using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using GameData.ArchiveData;
using GameData.Common;
using GameData.Serializer;

namespace GameData.Domains.Information
{
	// Token: 0x02000681 RID: 1665
	[SerializableGameData(NotForDisplayModule = true)]
	public class SecretInformationMetaData : BaseGameDataObject, ISerializableGameData
	{
		// Token: 0x060053F0 RID: 21488 RVA: 0x002DC636 File Offset: 0x002DA836
		public SecretInformationMetaData(int id, int offset, int relevanceSecretInformationMetaDataId = -1) : this()
		{
			this._id = id;
			this._offset = offset;
			this._relevanceSecretInformationMetaDataId = relevanceSecretInformationMetaDataId;
		}

		// Token: 0x060053F1 RID: 21489 RVA: 0x002DC655 File Offset: 0x002DA855
		public void UpdateOffset(int delta)
		{
			this._offset += delta;
		}

		// Token: 0x060053F2 RID: 21490 RVA: 0x002DC668 File Offset: 0x002DA868
		public void IncreaseCharacterDisseminationCount(int characterId)
		{
			int count;
			bool flag = this._disseminationData.DisseminationCounts.TryGetValue(characterId, out count);
			if (flag)
			{
				this._disseminationData.DisseminationCounts[characterId] = count + 1;
			}
			else
			{
				this._disseminationData.DisseminationCounts.Add(characterId, 1);
			}
		}

		// Token: 0x060053F3 RID: 21491 RVA: 0x002DC6BC File Offset: 0x002DA8BC
		public int GetCharacterDisseminationCount(int characterId)
		{
			int count;
			return this._disseminationData.DisseminationCounts.TryGetValue(characterId, out count) ? count : 0;
		}

		// Token: 0x060053F4 RID: 21492 RVA: 0x002DC6E2 File Offset: 0x002DA8E2
		public ICollection<int> GetDisseminationBranchCharacterIds()
		{
			return this._disseminationData.DisseminationCounts.Keys;
		}

		// Token: 0x1700033A RID: 826
		// (get) Token: 0x060053F5 RID: 21493 RVA: 0x002DC6F4 File Offset: 0x002DA8F4
		public SecretInformationCharacterRelationshipSnapshotCollection CharacterRelationshipSnapshotCollection
		{
			get
			{
				return this._secretInformationCharacterRelationshipSnapshotCollection;
			}
		}

		// Token: 0x1700033B RID: 827
		// (get) Token: 0x060053F6 RID: 21494 RVA: 0x002DC6FC File Offset: 0x002DA8FC
		public SecretInformationCharacterExtraInfoCollection CharacterExtraInfoCollection
		{
			get
			{
				return this._secretInformationCharacterExtraInfoCollection;
			}
		}

		// Token: 0x060053F7 RID: 21495 RVA: 0x002DC704 File Offset: 0x002DA904
		public int GetId()
		{
			return this._id;
		}

		// Token: 0x060053F8 RID: 21496 RVA: 0x002DC71C File Offset: 0x002DA91C
		public int GetOffset()
		{
			return this._offset;
		}

		// Token: 0x060053F9 RID: 21497 RVA: 0x002DC734 File Offset: 0x002DA934
		public unsafe void SetOffset(int offset, DataContext context)
		{
			this._offset = offset;
			base.SetModifiedAndInvalidateInfluencedCache(1, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				byte* pData = OperationAdder.DynamicObjectCollection_SetFixedField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 4U, 4);
				*(int*)pData = this._offset;
				pData += 4;
			}
		}

		// Token: 0x060053FA RID: 21498 RVA: 0x002DC794 File Offset: 0x002DA994
		public SecretInformationDisseminationData GetDisseminationData()
		{
			return this._disseminationData;
		}

		// Token: 0x060053FB RID: 21499 RVA: 0x002DC7AC File Offset: 0x002DA9AC
		public unsafe void SetDisseminationData(SecretInformationDisseminationData disseminationData, DataContext context)
		{
			this._disseminationData = disseminationData;
			base.SetModifiedAndInvalidateInfluencedCache(2, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._disseminationData.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 0, dataSize);
				pData += this._disseminationData.Serialize(pData);
			}
		}

		// Token: 0x060053FC RID: 21500 RVA: 0x002DC81C File Offset: 0x002DAA1C
		public int GetRelevanceSecretInformationMetaDataId()
		{
			return this._relevanceSecretInformationMetaDataId;
		}

		// Token: 0x060053FD RID: 21501 RVA: 0x002DC834 File Offset: 0x002DAA34
		public unsafe void SetRelevanceSecretInformationMetaDataId(int relevanceSecretInformationMetaDataId, DataContext context)
		{
			this._relevanceSecretInformationMetaDataId = relevanceSecretInformationMetaDataId;
			base.SetModifiedAndInvalidateInfluencedCache(3, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				byte* pData = OperationAdder.DynamicObjectCollection_SetFixedField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 8U, 4);
				*(int*)pData = this._relevanceSecretInformationMetaDataId;
				pData += 4;
			}
		}

		// Token: 0x060053FE RID: 21502 RVA: 0x002DC894 File Offset: 0x002DAA94
		public SecretInformationCharacterRelationshipSnapshotCollection GetSecretInformationCharacterRelationshipSnapshotCollection()
		{
			return this._secretInformationCharacterRelationshipSnapshotCollection;
		}

		// Token: 0x060053FF RID: 21503 RVA: 0x002DC8AC File Offset: 0x002DAAAC
		public unsafe void SetSecretInformationCharacterRelationshipSnapshotCollection(SecretInformationCharacterRelationshipSnapshotCollection secretInformationCharacterRelationshipSnapshotCollection, DataContext context)
		{
			this._secretInformationCharacterRelationshipSnapshotCollection = secretInformationCharacterRelationshipSnapshotCollection;
			base.SetModifiedAndInvalidateInfluencedCache(4, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._secretInformationCharacterRelationshipSnapshotCollection.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 1, dataSize);
				pData += this._secretInformationCharacterRelationshipSnapshotCollection.Serialize(pData);
			}
		}

		// Token: 0x06005400 RID: 21504 RVA: 0x002DC91C File Offset: 0x002DAB1C
		public SecretInformationCharacterExtraInfoCollection GetSecretInformationCharacterExtraInfoCollection()
		{
			return this._secretInformationCharacterExtraInfoCollection;
		}

		// Token: 0x06005401 RID: 21505 RVA: 0x002DC934 File Offset: 0x002DAB34
		public unsafe void SetSecretInformationCharacterExtraInfoCollection(SecretInformationCharacterExtraInfoCollection secretInformationCharacterExtraInfoCollection, DataContext context)
		{
			this._secretInformationCharacterExtraInfoCollection = secretInformationCharacterExtraInfoCollection;
			base.SetModifiedAndInvalidateInfluencedCache(5, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._secretInformationCharacterExtraInfoCollection.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 2, dataSize);
				pData += this._secretInformationCharacterExtraInfoCollection.Serialize(pData);
			}
		}

		// Token: 0x06005402 RID: 21506 RVA: 0x002DC9A2 File Offset: 0x002DABA2
		public SecretInformationMetaData()
		{
			this._disseminationData = new SecretInformationDisseminationData();
			this._secretInformationCharacterRelationshipSnapshotCollection = new SecretInformationCharacterRelationshipSnapshotCollection();
			this._secretInformationCharacterExtraInfoCollection = new SecretInformationCharacterExtraInfoCollection();
		}

		// Token: 0x06005403 RID: 21507 RVA: 0x002DC9D0 File Offset: 0x002DABD0
		public bool IsSerializedSizeFixed()
		{
			return false;
		}

		// Token: 0x06005404 RID: 21508 RVA: 0x002DC9E4 File Offset: 0x002DABE4
		public int GetSerializedSize()
		{
			int totalSize = 24;
			int dataSize = this._disseminationData.GetSerializedSize();
			totalSize += dataSize;
			int dataSize2 = this._secretInformationCharacterRelationshipSnapshotCollection.GetSerializedSize();
			totalSize += dataSize2;
			int dataSize3 = this._secretInformationCharacterExtraInfoCollection.GetSerializedSize();
			return totalSize + dataSize3;
		}

		// Token: 0x06005405 RID: 21509 RVA: 0x002DCA34 File Offset: 0x002DAC34
		public unsafe int Serialize(byte* pData)
		{
			*(int*)pData = this._id;
			byte* pCurrData = pData + 4;
			*(int*)pCurrData = this._offset;
			pCurrData += 4;
			*(int*)pCurrData = this._relevanceSecretInformationMetaDataId;
			pCurrData += 4;
			byte* pBegin = pCurrData;
			pCurrData += 4;
			pCurrData += this._disseminationData.Serialize(pCurrData);
			int fieldSize = (int)((long)(pCurrData - pBegin) - 4L);
			bool flag = fieldSize > 4194304;
			if (flag)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_disseminationData");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin = fieldSize;
			byte* pBegin2 = pCurrData;
			pCurrData += 4;
			pCurrData += this._secretInformationCharacterRelationshipSnapshotCollection.Serialize(pCurrData);
			int fieldSize2 = (int)((long)(pCurrData - pBegin2) - 4L);
			bool flag2 = fieldSize2 > 4194304;
			if (flag2)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_secretInformationCharacterRelationshipSnapshotCollection");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin2 = fieldSize2;
			byte* pBegin3 = pCurrData;
			pCurrData += 4;
			pCurrData += this._secretInformationCharacterExtraInfoCollection.Serialize(pCurrData);
			int fieldSize3 = (int)((long)(pCurrData - pBegin3) - 4L);
			bool flag3 = fieldSize3 > 4194304;
			if (flag3)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_secretInformationCharacterExtraInfoCollection");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin3 = fieldSize3;
			return (int)((long)(pCurrData - pData));
		}

		// Token: 0x06005406 RID: 21510 RVA: 0x002DCC20 File Offset: 0x002DAE20
		public unsafe int Deserialize(byte* pData)
		{
			this._id = *(int*)pData;
			byte* pCurrData = pData + 4;
			this._offset = *(int*)pCurrData;
			pCurrData += 4;
			this._relevanceSecretInformationMetaDataId = *(int*)pCurrData;
			pCurrData += 4;
			pCurrData += 4;
			pCurrData += this._disseminationData.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._secretInformationCharacterRelationshipSnapshotCollection.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._secretInformationCharacterExtraInfoCollection.Deserialize(pCurrData);
			return (int)((long)(pCurrData - pData));
		}

		// Token: 0x04001675 RID: 5749
		[CollectionObjectField(false, true, false, true, false)]
		private int _id;

		// Token: 0x04001676 RID: 5750
		[CollectionObjectField(false, true, false, false, false)]
		private int _offset;

		// Token: 0x04001677 RID: 5751
		[CollectionObjectField(false, true, false, false, false)]
		private SecretInformationDisseminationData _disseminationData;

		// Token: 0x04001678 RID: 5752
		[CollectionObjectField(false, true, false, false, false)]
		private int _relevanceSecretInformationMetaDataId;

		// Token: 0x04001679 RID: 5753
		[CollectionObjectField(false, true, false, false, false)]
		private SecretInformationCharacterRelationshipSnapshotCollection _secretInformationCharacterRelationshipSnapshotCollection;

		// Token: 0x0400167A RID: 5754
		[CollectionObjectField(false, true, false, false, false)]
		private SecretInformationCharacterExtraInfoCollection _secretInformationCharacterExtraInfoCollection;

		// Token: 0x0400167B RID: 5755
		public const int FixedSize = 12;

		// Token: 0x0400167C RID: 5756
		public const int DynamicCount = 3;

		// Token: 0x02000AE2 RID: 2786
		internal class FixedFieldInfos
		{
			// Token: 0x04002D12 RID: 11538
			public const uint Id_Offset = 0U;

			// Token: 0x04002D13 RID: 11539
			public const int Id_Size = 4;

			// Token: 0x04002D14 RID: 11540
			public const uint Offset_Offset = 4U;

			// Token: 0x04002D15 RID: 11541
			public const int Offset_Size = 4;

			// Token: 0x04002D16 RID: 11542
			public const uint RelevanceSecretInformationMetaDataId_Offset = 8U;

			// Token: 0x04002D17 RID: 11543
			public const int RelevanceSecretInformationMetaDataId_Size = 4;
		}
	}
}
