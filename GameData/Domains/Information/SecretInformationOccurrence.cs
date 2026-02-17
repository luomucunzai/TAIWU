using System;
using GameData.Serializer;

namespace GameData.Domains.Information
{
	// Token: 0x02000682 RID: 1666
	[SerializableGameData(IsExtensible = true, NotForDisplayModule = true)]
	public class SecretInformationOccurrence : ISerializableGameData
	{
		// Token: 0x06005407 RID: 21511 RVA: 0x002DCC98 File Offset: 0x002DAE98
		public bool IsSerializedSizeFixed()
		{
			return false;
		}

		// Token: 0x06005408 RID: 21512 RVA: 0x002DCCAC File Offset: 0x002DAEAC
		public int GetSerializedSize()
		{
			int totalSize = 2;
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x06005409 RID: 21513 RVA: 0x002DCCD0 File Offset: 0x002DAED0
		public unsafe int Serialize(byte* pData)
		{
			*(short*)pData = 0;
			byte* pCurrData = pData + 2;
			int totalSize = (int)((long)(pCurrData - pData));
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x0600540A RID: 21514 RVA: 0x002DCD04 File Offset: 0x002DAF04
		public unsafe int Deserialize(byte* pData)
		{
			ushort fieldCount = *(ushort*)pData;
			byte* pCurrData = pData + 2;
			int totalSize = (int)((long)(pCurrData - pData));
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x02000AE3 RID: 2787
		private static class FieldIds
		{
			// Token: 0x04002D18 RID: 11544
			public const ushort Count = 0;

			// Token: 0x04002D19 RID: 11545
			public static readonly string[] FieldId2FieldName = new string[0];
		}
	}
}
