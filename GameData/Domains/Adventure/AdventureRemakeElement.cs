using System;
using GameData.Domains.Map;
using GameData.Serializer;
using GameData.Utilities;
using SerializableGameDataSourceGenerator;

namespace GameData.Domains.Adventure
{
	// Token: 0x020008C6 RID: 2246
	[SerializableGameDataSourceGenerator.AutoGenerateSerializableGameData(IsExtensible = true)]
	public class AdventureRemakeElement : ISerializableGameData
	{
		// Token: 0x06007F6F RID: 32623 RVA: 0x004C9C96 File Offset: 0x004C7E96
		public AdventureRemakeElement()
		{
		}

		// Token: 0x06007F70 RID: 32624 RVA: 0x004C9CA0 File Offset: 0x004C7EA0
		public AdventureRemakeElement(AdventureRemakeElement other)
		{
			this.Location = other.Location;
		}

		// Token: 0x06007F71 RID: 32625 RVA: 0x004C9CB6 File Offset: 0x004C7EB6
		public void Assign(AdventureRemakeElement other)
		{
			this.Location = other.Location;
		}

		// Token: 0x06007F72 RID: 32626 RVA: 0x004C9CC8 File Offset: 0x004C7EC8
		public bool IsSerializedSizeFixed()
		{
			return false;
		}

		// Token: 0x06007F73 RID: 32627 RVA: 0x004C9CDC File Offset: 0x004C7EDC
		public int GetSerializedSize()
		{
			int totalSize = 2;
			totalSize += this.Location.GetSerializedSize();
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x06007F74 RID: 32628 RVA: 0x004C9D10 File Offset: 0x004C7F10
		public unsafe int Serialize(byte* pData)
		{
			*(short*)pData = 1;
			byte* pCurrData = pData + 2;
			int fieldSize = this.Location.Serialize(pCurrData);
			pCurrData += fieldSize;
			Tester.Assert(fieldSize <= 65535, "");
			int totalSize = (int)((long)(pCurrData - pData));
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x06007F75 RID: 32629 RVA: 0x004C9D70 File Offset: 0x004C7F70
		public unsafe int Deserialize(byte* pData)
		{
			ushort fieldCount = *(ushort*)pData;
			byte* pCurrData = pData + 2;
			bool flag = fieldCount > 0;
			if (flag)
			{
				pCurrData += this.Location.Deserialize(pCurrData);
			}
			int totalSize = (int)((long)(pCurrData - pData));
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x040022F0 RID: 8944
		[SerializableGameDataField(FieldIndex = 0)]
		public Location Location;

		// Token: 0x02000CBF RID: 3263
		public static class FieldIds
		{
			// Token: 0x0400371A RID: 14106
			public const ushort Location = 0;

			// Token: 0x0400371B RID: 14107
			public const ushort Count = 1;

			// Token: 0x0400371C RID: 14108
			public static readonly string[] FieldId2FieldName = new string[]
			{
				"Location"
			};
		}
	}
}
