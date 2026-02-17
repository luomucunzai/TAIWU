using System;
using GameData.Serializer;
using SerializableGameDataSourceGenerator;

namespace GameData.Domains.Organization
{
	// Token: 0x0200064A RID: 1610
	[SerializableGameDataSourceGenerator.AutoGenerateSerializableGameData(IsExtensible = true, NotForDisplayModule = true, NoCopyConstructors = true)]
	public class SettlementMemberFeature : ISerializableGameData
	{
		// Token: 0x0600484F RID: 18511 RVA: 0x0028CBB0 File Offset: 0x0028ADB0
		public bool IsSerializedSizeFixed()
		{
			return false;
		}

		// Token: 0x06004850 RID: 18512 RVA: 0x0028CBC4 File Offset: 0x0028ADC4
		public int GetSerializedSize()
		{
			int totalSize = 6;
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x06004851 RID: 18513 RVA: 0x0028CBE8 File Offset: 0x0028ADE8
		public unsafe int Serialize(byte* pData)
		{
			*(short*)pData = 3;
			byte* pCurrData = pData + 2;
			*(short*)pCurrData = this.FeatureId;
			pCurrData += 2;
			*pCurrData = (byte)this.MinGrade;
			pCurrData++;
			*pCurrData = (byte)this.MaxGrade;
			pCurrData++;
			int totalSize = (int)((long)(pCurrData - pData));
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x06004852 RID: 18514 RVA: 0x0028CC40 File Offset: 0x0028AE40
		public unsafe int Deserialize(byte* pData)
		{
			ushort fieldCount = *(ushort*)pData;
			byte* pCurrData = pData + 2;
			bool flag = fieldCount > 0;
			if (flag)
			{
				this.FeatureId = *(short*)pCurrData;
				pCurrData += 2;
			}
			bool flag2 = fieldCount > 1;
			if (flag2)
			{
				this.MinGrade = *(sbyte*)pCurrData;
				pCurrData++;
			}
			bool flag3 = fieldCount > 2;
			if (flag3)
			{
				this.MaxGrade = *(sbyte*)pCurrData;
				pCurrData++;
			}
			int totalSize = (int)((long)(pCurrData - pData));
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x04001517 RID: 5399
		[SerializableGameDataField(FieldIndex = 0)]
		public short FeatureId;

		// Token: 0x04001518 RID: 5400
		[SerializableGameDataField(FieldIndex = 1)]
		public sbyte MinGrade;

		// Token: 0x04001519 RID: 5401
		[SerializableGameDataField(FieldIndex = 2)]
		public sbyte MaxGrade;

		// Token: 0x02000A8A RID: 2698
		public static class FieldIds
		{
			// Token: 0x04002B58 RID: 11096
			public const ushort FeatureId = 0;

			// Token: 0x04002B59 RID: 11097
			public const ushort MinGrade = 1;

			// Token: 0x04002B5A RID: 11098
			public const ushort MaxGrade = 2;

			// Token: 0x04002B5B RID: 11099
			public const ushort Count = 3;

			// Token: 0x04002B5C RID: 11100
			public static readonly string[] FieldId2FieldName = new string[]
			{
				"FeatureId",
				"MinGrade",
				"MaxGrade"
			};
		}
	}
}
