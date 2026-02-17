using System;
using GameData.Domains.Character;
using GameData.Serializer;

namespace GameData.Domains.Taiwu.LifeSkillCombat.Status
{
	// Token: 0x02000059 RID: 89
	public struct Book : ISerializableGameData
	{
		// Token: 0x1700007B RID: 123
		// (get) Token: 0x0600151F RID: 5407 RVA: 0x001482E2 File Offset: 0x001464E2
		public int DisplayCd
		{
			get
			{
				return (this.IsDisplayCd != this.IsCd) ? this.CoveringCd : this.RemainingCd;
			}
		}

		// Token: 0x1700007C RID: 124
		// (get) Token: 0x06001520 RID: 5408 RVA: 0x00148300 File Offset: 0x00146500
		public bool IsCd
		{
			get
			{
				return this.RemainingCd > 0;
			}
		}

		// Token: 0x1700007D RID: 125
		// (get) Token: 0x06001521 RID: 5409 RVA: 0x0014830C File Offset: 0x0014650C
		public bool IsDisplayCd
		{
			get
			{
				bool flag = this.IsCd && this.CoveringCd == 0;
				bool result;
				if (flag)
				{
					result = false;
				}
				else
				{
					bool flag2 = !this.IsCd && this.CoveringCd > 0;
					result = (flag2 || this.IsCd);
				}
				return result;
			}
		}

		// Token: 0x06001522 RID: 5410 RVA: 0x0014835C File Offset: 0x0014655C
		public bool IsSerializedSizeFixed()
		{
			return true;
		}

		// Token: 0x06001523 RID: 5411 RVA: 0x00148360 File Offset: 0x00146560
		public int GetSerializedSize()
		{
			int totalSize = 0;
			totalSize += 4;
			totalSize += this.LifeSkill.GetSerializedSize();
			totalSize += 4;
			totalSize += 4;
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x06001524 RID: 5412 RVA: 0x001483A0 File Offset: 0x001465A0
		public unsafe int Serialize(byte* pData)
		{
			*(int*)pData = this.BasePoint;
			byte* pCurrData = pData + 4;
			pCurrData += this.LifeSkill.Serialize(pCurrData);
			*(int*)pCurrData = this.RemainingCd;
			pCurrData += 4;
			*(int*)pCurrData = this.CoveringCd;
			pCurrData += 4;
			int totalSize = (int)((long)(pCurrData - pData));
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x06001525 RID: 5413 RVA: 0x00148400 File Offset: 0x00146600
		public unsafe int Deserialize(byte* pData)
		{
			this.BasePoint = *(int*)pData;
			byte* pCurrData = pData + 4;
			pCurrData += this.LifeSkill.Deserialize(pCurrData);
			this.RemainingCd = *(int*)pCurrData;
			pCurrData += 4;
			this.CoveringCd = *(int*)pCurrData;
			pCurrData += 4;
			int totalSize = (int)((long)(pCurrData - pData));
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x06001526 RID: 5414 RVA: 0x0014845F File Offset: 0x0014665F
		public Book(LifeSkillItem lifeSkill)
		{
			this.LifeSkill = lifeSkill;
			this.BasePoint = 0;
			this.RemainingCd = 0;
			this.CoveringCd = -1;
		}

		// Token: 0x0400034D RID: 845
		public int BasePoint;

		// Token: 0x0400034E RID: 846
		public LifeSkillItem LifeSkill;

		// Token: 0x0400034F RID: 847
		public int RemainingCd;

		// Token: 0x04000350 RID: 848
		public int CoveringCd;
	}
}
