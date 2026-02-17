using System;
using GameData.Serializer;

namespace GameData.Domains.Character.TemporaryModification
{
	// Token: 0x0200081D RID: 2077
	[SerializableGameData(NotForDisplayModule = true)]
	public struct ShortListModification : ISerializableGameData
	{
		// Token: 0x06007508 RID: 29960 RVA: 0x00447964 File Offset: 0x00445B64
		public ShortListModification(sbyte modificationType, short index, short element)
		{
			this.ModificationType = modificationType;
			this.Index = index;
			this.Element = element;
		}

		// Token: 0x06007509 RID: 29961 RVA: 0x0044797C File Offset: 0x00445B7C
		public static int GetFixedSerializedSize()
		{
			return 5;
		}

		// Token: 0x0600750A RID: 29962 RVA: 0x00447990 File Offset: 0x00445B90
		public bool IsSerializedSizeFixed()
		{
			return true;
		}

		// Token: 0x0600750B RID: 29963 RVA: 0x004479A4 File Offset: 0x00445BA4
		public int GetSerializedSize()
		{
			return 5;
		}

		// Token: 0x0600750C RID: 29964 RVA: 0x004479B8 File Offset: 0x00445BB8
		public unsafe int Serialize(byte* pData)
		{
			*pData = (byte)this.ModificationType;
			*(short*)(pData + 1) = this.Index;
			*(short*)(pData + 1 + 2) = this.Element;
			return 5;
		}

		// Token: 0x0600750D RID: 29965 RVA: 0x004479EC File Offset: 0x00445BEC
		public unsafe int Deserialize(byte* pData)
		{
			this.ModificationType = *(sbyte*)pData;
			this.Index = *(short*)(pData + 1);
			this.Element = *(short*)(pData + 1 + 2);
			return 5;
		}

		// Token: 0x04001F17 RID: 7959
		public sbyte ModificationType;

		// Token: 0x04001F18 RID: 7960
		public short Index;

		// Token: 0x04001F19 RID: 7961
		public short Element;
	}
}
