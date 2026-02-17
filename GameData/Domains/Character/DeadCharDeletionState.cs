using System;
using GameData.Serializer;

namespace GameData.Domains.Character
{
	// Token: 0x0200080E RID: 2062
	[SerializableGameData(NotForDisplayModule = true)]
	public struct DeadCharDeletionState : ISerializableGameData
	{
		// Token: 0x06007469 RID: 29801 RVA: 0x00443654 File Offset: 0x00441854
		public bool IsSerializedSizeFixed()
		{
			return true;
		}

		// Token: 0x0600746A RID: 29802 RVA: 0x00443668 File Offset: 0x00441868
		public int GetSerializedSize()
		{
			return 2;
		}

		// Token: 0x0600746B RID: 29803 RVA: 0x0044367C File Offset: 0x0044187C
		public unsafe int Serialize(byte* pData)
		{
			*pData = (this.GraveRemoved ? 1 : 0);
			pData[1] = (this.DeletedFromOthersPreexistence ? 1 : 0);
			return 2;
		}

		// Token: 0x0600746C RID: 29804 RVA: 0x004436A4 File Offset: 0x004418A4
		public unsafe int Deserialize(byte* pData)
		{
			this.GraveRemoved = (*pData != 0);
			this.DeletedFromOthersPreexistence = (pData[1] != 0);
			return 2;
		}

		// Token: 0x04001EC1 RID: 7873
		public bool GraveRemoved;

		// Token: 0x04001EC2 RID: 7874
		public bool DeletedFromOthersPreexistence;
	}
}
