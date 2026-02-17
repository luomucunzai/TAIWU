using System;
using GameData.Serializer;

namespace GameData.Domains.SpecialEffect
{
	// Token: 0x020000E6 RID: 230
	public class SpecialEffectWrapper : ISerializableGameData
	{
		// Token: 0x06002957 RID: 10583 RVA: 0x00200470 File Offset: 0x001FE670
		public bool IsSerializedSizeFixed()
		{
			return false;
		}

		// Token: 0x06002958 RID: 10584 RVA: 0x00200484 File Offset: 0x001FE684
		public int GetSerializedSize()
		{
			int totalSize = 4 + this.Effect.GetSerializedSize();
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x06002959 RID: 10585 RVA: 0x002004B8 File Offset: 0x001FE6B8
		public unsafe int Serialize(byte* pData)
		{
			*(int*)pData = this.Effect.Type;
			byte* pCurrData = pData + 4;
			pCurrData += this.Effect.Serialize(pCurrData);
			int totalSize = (int)((long)(pCurrData - pData));
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x0600295A RID: 10586 RVA: 0x00200504 File Offset: 0x001FE704
		public unsafe int Deserialize(byte* pData)
		{
			int type = *(int*)pData;
			byte* pCurrData = pData + 4;
			this.Effect = SpecialEffectType.CreateEffectObj(type);
			pCurrData += this.Effect.Deserialize(pCurrData);
			int totalSize = (int)((long)(pCurrData - pData));
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x04000CBA RID: 3258
		public SpecialEffectBase Effect;
	}
}
