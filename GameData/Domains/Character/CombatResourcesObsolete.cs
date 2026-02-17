using System;
using GameData.Serializer;

namespace GameData.Domains.Character
{
	// Token: 0x0200080B RID: 2059
	[SerializableGameData(NotForDisplayModule = true)]
	[Obsolete("This class is only for archive module. Cannot use to do anything.")]
	public class CombatResourcesObsolete : ISerializableGameData
	{
		// Token: 0x06007461 RID: 29793 RVA: 0x0044336E File Offset: 0x0044156E
		public bool IsSerializedSizeFixed()
		{
			return true;
		}

		// Token: 0x06007462 RID: 29794 RVA: 0x00443371 File Offset: 0x00441571
		public int GetSerializedSize()
		{
			return 2;
		}

		// Token: 0x06007463 RID: 29795 RVA: 0x00443374 File Offset: 0x00441574
		public unsafe int Serialize(byte* pData)
		{
			return 2;
		}

		// Token: 0x06007464 RID: 29796 RVA: 0x00443377 File Offset: 0x00441577
		public unsafe int Deserialize(byte* pData)
		{
			return 2;
		}
	}
}
