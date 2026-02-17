using System;
using GameData.Serializer;

namespace GameData.Domains.Combat
{
	// Token: 0x020006D4 RID: 1748
	[SerializableGameData(NotForArchive = true)]
	public struct SpecialMiscData : ISerializableGameData, IEquatable<SpecialMiscData>
	{
		// Token: 0x1700040C RID: 1036
		// (get) Token: 0x06006755 RID: 26453 RVA: 0x003B12C8 File Offset: 0x003AF4C8
		public bool CanUse
		{
			get
			{
				return this.Chance > 0;
			}
		}

		// Token: 0x06006756 RID: 26454 RVA: 0x003B12D4 File Offset: 0x003AF4D4
		public static implicit operator SpecialMiscData(int chance)
		{
			return new SpecialMiscData
			{
				Chance = chance
			};
		}

		// Token: 0x06006757 RID: 26455 RVA: 0x003B12F4 File Offset: 0x003AF4F4
		public static bool operator ==(SpecialMiscData lhs, SpecialMiscData rhs)
		{
			return lhs.Chance == rhs.Chance;
		}

		// Token: 0x06006758 RID: 26456 RVA: 0x003B1314 File Offset: 0x003AF514
		public static bool operator !=(SpecialMiscData lhs, SpecialMiscData rhs)
		{
			return lhs.Chance != rhs.Chance;
		}

		// Token: 0x06006759 RID: 26457 RVA: 0x003B1338 File Offset: 0x003AF538
		public bool Equals(SpecialMiscData other)
		{
			return this.Chance == other.Chance;
		}

		// Token: 0x0600675A RID: 26458 RVA: 0x003B1358 File Offset: 0x003AF558
		public override bool Equals(object obj)
		{
			bool result;
			if (obj is SpecialMiscData)
			{
				SpecialMiscData other = (SpecialMiscData)obj;
				result = this.Equals(other);
			}
			else
			{
				result = false;
			}
			return result;
		}

		// Token: 0x0600675B RID: 26459 RVA: 0x003B1384 File Offset: 0x003AF584
		public override int GetHashCode()
		{
			return this.Chance;
		}

		// Token: 0x0600675C RID: 26460 RVA: 0x003B139C File Offset: 0x003AF59C
		public bool IsSerializedSizeFixed()
		{
			return true;
		}

		// Token: 0x0600675D RID: 26461 RVA: 0x003B13B0 File Offset: 0x003AF5B0
		public int GetSerializedSize()
		{
			int totalSize = 4;
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x0600675E RID: 26462 RVA: 0x003B13D4 File Offset: 0x003AF5D4
		public unsafe int Serialize(byte* pData)
		{
			*(int*)pData = this.Chance;
			byte* pCurrData = pData + 4;
			int totalSize = (int)((long)(pCurrData - pData));
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x0600675F RID: 26463 RVA: 0x003B140C File Offset: 0x003AF60C
		public unsafe int Deserialize(byte* pData)
		{
			this.Chance = *(int*)pData;
			byte* pCurrData = pData + 4;
			int totalSize = (int)((long)(pCurrData - pData));
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x04001C23 RID: 7203
		[SerializableGameDataField]
		public int Chance;
	}
}
