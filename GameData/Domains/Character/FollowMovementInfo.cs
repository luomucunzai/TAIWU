using System;
using GameData.Serializer;

namespace GameData.Domains.Character
{
	// Token: 0x02000810 RID: 2064
	[SerializableGameData(IsExtensible = true, NotForDisplayModule = true, NoCopyConstructors = true)]
	public class FollowMovementInfo : ISerializableGameData
	{
		// Token: 0x06007477 RID: 29815 RVA: 0x00443DEB File Offset: 0x00441FEB
		public FollowMovementInfo(int charId, int distance)
		{
			this.TargetCharId = charId;
			this.Distance = distance;
		}

		// Token: 0x06007478 RID: 29816 RVA: 0x00443E03 File Offset: 0x00442003
		public FollowMovementInfo()
		{
		}

		// Token: 0x06007479 RID: 29817 RVA: 0x00443E10 File Offset: 0x00442010
		public bool IsSerializedSizeFixed()
		{
			return false;
		}

		// Token: 0x0600747A RID: 29818 RVA: 0x00443E24 File Offset: 0x00442024
		public int GetSerializedSize()
		{
			int totalSize = 10;
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x0600747B RID: 29819 RVA: 0x00443E4C File Offset: 0x0044204C
		public unsafe int Serialize(byte* pData)
		{
			*(short*)pData = 2;
			byte* pCurrData = pData + 2;
			*(int*)pCurrData = this.TargetCharId;
			pCurrData += 4;
			*(int*)pCurrData = this.Distance;
			pCurrData += 4;
			int totalSize = (int)((long)(pCurrData - pData));
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x0600747C RID: 29820 RVA: 0x00443E98 File Offset: 0x00442098
		public unsafe int Deserialize(byte* pData)
		{
			ushort fieldCount = *(ushort*)pData;
			byte* pCurrData = pData + 2;
			bool flag = fieldCount > 0;
			if (flag)
			{
				this.TargetCharId = *(int*)pCurrData;
				pCurrData += 4;
			}
			bool flag2 = fieldCount > 1;
			if (flag2)
			{
				this.Distance = *(int*)pCurrData;
				pCurrData += 4;
			}
			int totalSize = (int)((long)(pCurrData - pData));
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x04001EC3 RID: 7875
		[SerializableGameDataField]
		public int TargetCharId;

		// Token: 0x04001EC4 RID: 7876
		[SerializableGameDataField]
		public int Distance;

		// Token: 0x02000C04 RID: 3076
		private static class FieldIds
		{
			// Token: 0x040033EF RID: 13295
			public const ushort TargetCharId = 0;

			// Token: 0x040033F0 RID: 13296
			public const ushort Distance = 1;

			// Token: 0x040033F1 RID: 13297
			public const ushort Count = 2;

			// Token: 0x040033F2 RID: 13298
			public static readonly string[] FieldId2FieldName = new string[]
			{
				"TargetCharId",
				"Distance"
			};
		}
	}
}
