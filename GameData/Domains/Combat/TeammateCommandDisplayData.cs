using System;
using GameData.Serializer;

namespace GameData.Domains.Combat
{
	// Token: 0x020006FD RID: 1789
	[SerializableGameData(NotForArchive = true)]
	public struct TeammateCommandDisplayData : ISerializableGameData
	{
		// Token: 0x060067CF RID: 26575 RVA: 0x003B2488 File Offset: 0x003B0688
		public bool IsSerializedSizeFixed()
		{
			return true;
		}

		// Token: 0x060067D0 RID: 26576 RVA: 0x003B249C File Offset: 0x003B069C
		public int GetSerializedSize()
		{
			int totalSize = 5;
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x060067D1 RID: 26577 RVA: 0x003B24C0 File Offset: 0x003B06C0
		public unsafe int Serialize(byte* pData)
		{
			*pData = (this.IsAlly ? 1 : 0);
			byte* pCurrData = pData + 1;
			*pCurrData = (byte)this.IndexCharacter;
			pCurrData++;
			*pCurrData = (byte)this.ValidIndexCharacter;
			pCurrData++;
			*pCurrData = (byte)this.IndexCommand;
			pCurrData++;
			*pCurrData = (byte)this.CmdType;
			pCurrData++;
			int totalSize = (int)((long)(pCurrData - pData));
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x060067D2 RID: 26578 RVA: 0x003B2528 File Offset: 0x003B0728
		public unsafe int Deserialize(byte* pData)
		{
			this.IsAlly = (*pData != 0);
			byte* pCurrData = pData + 1;
			this.IndexCharacter = *(sbyte*)pCurrData;
			pCurrData++;
			this.ValidIndexCharacter = *(sbyte*)pCurrData;
			pCurrData++;
			this.IndexCommand = *(sbyte*)pCurrData;
			pCurrData++;
			this.CmdType = *(sbyte*)pCurrData;
			pCurrData++;
			int totalSize = (int)((long)(pCurrData - pData));
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x04001C4D RID: 7245
		[SerializableGameDataField]
		public bool IsAlly;

		// Token: 0x04001C4E RID: 7246
		[SerializableGameDataField]
		public sbyte IndexCharacter;

		// Token: 0x04001C4F RID: 7247
		[SerializableGameDataField]
		public sbyte ValidIndexCharacter;

		// Token: 0x04001C50 RID: 7248
		[SerializableGameDataField]
		public sbyte IndexCommand;

		// Token: 0x04001C51 RID: 7249
		[SerializableGameDataField]
		public sbyte CmdType;
	}
}
