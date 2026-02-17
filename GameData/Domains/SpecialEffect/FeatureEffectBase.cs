using System;
using GameData.Serializer;

namespace GameData.Domains.SpecialEffect
{
	// Token: 0x020000DD RID: 221
	[SerializableGameData(NotForDisplayModule = true)]
	public class FeatureEffectBase : SpecialEffectBase
	{
		// Token: 0x06002887 RID: 10375 RVA: 0x001EFDDF File Offset: 0x001EDFDF
		public FeatureEffectBase()
		{
		}

		// Token: 0x06002888 RID: 10376 RVA: 0x001EFDE9 File Offset: 0x001EDFE9
		public FeatureEffectBase(int charId, short featureId, int type) : base(charId, type)
		{
			this.FeatureId = featureId;
		}

		// Token: 0x06002889 RID: 10377 RVA: 0x001EFDFC File Offset: 0x001EDFFC
		protected override int GetSubClassSerializedSize()
		{
			return 2;
		}

		// Token: 0x0600288A RID: 10378 RVA: 0x001EFE10 File Offset: 0x001EE010
		protected unsafe override int SerializeSubClass(byte* pData)
		{
			*(short*)pData = this.FeatureId;
			byte* pCurrData = pData + 2;
			return (int)((long)(pCurrData - pData));
		}

		// Token: 0x0600288B RID: 10379 RVA: 0x001EFE38 File Offset: 0x001EE038
		protected unsafe override int DeserializeSubClass(byte* pData)
		{
			this.FeatureId = *(short*)pData;
			byte* pCurrData = pData + 2;
			return (int)((long)(pCurrData - pData));
		}

		// Token: 0x04000821 RID: 2081
		public short FeatureId;
	}
}
