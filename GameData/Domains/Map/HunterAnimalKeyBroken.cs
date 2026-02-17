using System;
using GameData.Serializer;

namespace GameData.Domains.Map
{
	// Token: 0x020008B3 RID: 2227
	[Obsolete]
	[SerializableGameData(NotForDisplayModule = true)]
	public struct HunterAnimalKeyBroken : ISerializableGameData, IEquatable<HunterAnimalKeyBroken>
	{
		// Token: 0x060078B3 RID: 30899 RVA: 0x004669D2 File Offset: 0x00464BD2
		public HunterAnimalKeyBroken(short areaId, short blockId, short animalId)
		{
			this.AreaId = areaId;
			this.BlockId = blockId;
			this.AnimalId = animalId;
		}

		// Token: 0x060078B4 RID: 30900 RVA: 0x004669EC File Offset: 0x00464BEC
		public bool Equals(HunterAnimalKeyBroken other)
		{
			return this.AreaId == other.AreaId && this.BlockId == other.BlockId && this.AnimalId == other.AnimalId;
		}

		// Token: 0x060078B5 RID: 30901 RVA: 0x00466A2C File Offset: 0x00464C2C
		public override bool Equals(object obj)
		{
			bool result;
			if (obj is HunterAnimalKeyBroken)
			{
				HunterAnimalKeyBroken other = (HunterAnimalKeyBroken)obj;
				result = this.Equals(other);
			}
			else
			{
				result = false;
			}
			return result;
		}

		// Token: 0x060078B6 RID: 30902 RVA: 0x00466A58 File Offset: 0x00464C58
		public override int GetHashCode()
		{
			int hashCode = this.AreaId.GetHashCode();
			hashCode = (hashCode * 397 ^ this.BlockId.GetHashCode());
			return hashCode * 397 ^ this.AnimalId.GetHashCode();
		}

		// Token: 0x060078B7 RID: 30903 RVA: 0x00466AA0 File Offset: 0x00464CA0
		public bool IsSerializedSizeFixed()
		{
			return true;
		}

		// Token: 0x060078B8 RID: 30904 RVA: 0x00466AB4 File Offset: 0x00464CB4
		public int GetSerializedSize()
		{
			int totalSize = 0;
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x060078B9 RID: 30905 RVA: 0x00466AD8 File Offset: 0x00464CD8
		public unsafe int Serialize(byte* pData)
		{
			int totalSize = (int)((long)(pData - pData));
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x060078BA RID: 30906 RVA: 0x00466B04 File Offset: 0x00464D04
		public unsafe int Deserialize(byte* pData)
		{
			int totalSize = (int)((long)(pData - pData));
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x04002199 RID: 8601
		public short AreaId;

		// Token: 0x0400219A RID: 8602
		public short BlockId;

		// Token: 0x0400219B RID: 8603
		public short AnimalId;
	}
}
