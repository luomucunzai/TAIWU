using System;
using GameData.Serializer;

namespace GameData.Domains.Combat
{
	// Token: 0x020006CF RID: 1743
	public struct ShowAvoidData : ISerializableGameData
	{
		// Token: 0x0600672E RID: 26414 RVA: 0x003B07B3 File Offset: 0x003AE9B3
		public ShowAvoidData(sbyte hitType, short value)
		{
			this.HitType = hitType;
			this.Value = value;
		}

		// Token: 0x0600672F RID: 26415 RVA: 0x003B07C4 File Offset: 0x003AE9C4
		public ShowAvoidData(ShowAvoidData other)
		{
			this.HitType = other.HitType;
			this.Value = other.Value;
		}

		// Token: 0x06006730 RID: 26416 RVA: 0x003B07E0 File Offset: 0x003AE9E0
		public bool IsSerializedSizeFixed()
		{
			return true;
		}

		// Token: 0x06006731 RID: 26417 RVA: 0x003B07F4 File Offset: 0x003AE9F4
		public int GetSerializedSize()
		{
			return 4;
		}

		// Token: 0x06006732 RID: 26418 RVA: 0x003B0808 File Offset: 0x003AEA08
		public unsafe int Serialize(byte* pData)
		{
			*pData = (byte)this.HitType;
			*(short*)(pData + 1) = this.Value;
			return 4;
		}

		// Token: 0x06006733 RID: 26419 RVA: 0x003B0830 File Offset: 0x003AEA30
		public unsafe int Deserialize(byte* pData)
		{
			this.HitType = *(sbyte*)pData;
			this.Value = *(short*)(pData + 1);
			return 4;
		}

		// Token: 0x04001C16 RID: 7190
		public sbyte HitType;

		// Token: 0x04001C17 RID: 7191
		public short Value;
	}
}
