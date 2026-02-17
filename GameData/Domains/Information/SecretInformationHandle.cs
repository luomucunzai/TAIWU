using System;
using GameData.Serializer;

namespace GameData.Domains.Information
{
	// Token: 0x02000680 RID: 1664
	[SerializableGameData(IsExtensible = true, NotForDisplayModule = true)]
	public class SecretInformationHandle : ISerializableGameData
	{
		// Token: 0x060053EB RID: 21483 RVA: 0x002DC58C File Offset: 0x002DA78C
		public bool IsSerializedSizeFixed()
		{
			return false;
		}

		// Token: 0x060053EC RID: 21484 RVA: 0x002DC5A0 File Offset: 0x002DA7A0
		public int GetSerializedSize()
		{
			int totalSize = 2;
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x060053ED RID: 21485 RVA: 0x002DC5C4 File Offset: 0x002DA7C4
		public unsafe int Serialize(byte* pData)
		{
			*(short*)pData = 0;
			byte* pCurrData = pData + 2;
			int totalSize = (int)((long)(pCurrData - pData));
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x060053EE RID: 21486 RVA: 0x002DC5F8 File Offset: 0x002DA7F8
		public unsafe int Deserialize(byte* pData)
		{
			ushort fieldCount = *(ushort*)pData;
			byte* pCurrData = pData + 2;
			int totalSize = (int)((long)(pCurrData - pData));
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x02000AE1 RID: 2785
		private static class FieldIds
		{
			// Token: 0x04002D10 RID: 11536
			public const ushort Count = 0;

			// Token: 0x04002D11 RID: 11537
			public static readonly string[] FieldId2FieldName = new string[0];
		}
	}
}
