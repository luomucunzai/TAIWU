using System;
using GameData.Domains.CombatSkill;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect
{
	// Token: 0x020000DB RID: 219
	[SerializableGameData(NotForArchive = true)]
	public struct CastBoostEffectDisplayData : ISerializableGameData
	{
		// Token: 0x170000DC RID: 220
		// (get) Token: 0x06002878 RID: 10360 RVA: 0x001EFB47 File Offset: 0x001EDD47
		public ECastBoostType Type
		{
			get
			{
				return (ECastBoostType)this._internalType;
			}
		}

		// Token: 0x170000DD RID: 221
		// (get) Token: 0x06002879 RID: 10361 RVA: 0x001EFB4F File Offset: 0x001EDD4F
		public int EffectId
		{
			get
			{
				return this.EffectDescription.EffectId;
			}
		}

		// Token: 0x170000DE RID: 222
		// (get) Token: 0x0600287A RID: 10362 RVA: 0x001EFB5C File Offset: 0x001EDD5C
		public byte NeiliAllocationType
		{
			get
			{
				return (byte)((this.Type == ECastBoostType.CostNeiliAllocation) ? this._internalParam0 : 4);
			}
		}

		// Token: 0x170000DF RID: 223
		// (get) Token: 0x0600287B RID: 10363 RVA: 0x001EFB70 File Offset: 0x001EDD70
		public int NeiliAllocationValue
		{
			get
			{
				return (this.Type == ECastBoostType.CostNeiliAllocation) ? this._internalParam1 : -1;
			}
		}

		// Token: 0x170000E0 RID: 224
		// (get) Token: 0x0600287C RID: 10364 RVA: 0x001EFB83 File Offset: 0x001EDD83
		public short WugMedicineTemplateId
		{
			get
			{
				return (short)((this.Type == ECastBoostType.CostWugKing) ? this._internalParam0 : -1);
			}
		}

		// Token: 0x170000E1 RID: 225
		// (get) Token: 0x0600287D RID: 10365 RVA: 0x001EFB98 File Offset: 0x001EDD98
		public int WugKingCount
		{
			get
			{
				return (this.Type == ECastBoostType.CostWugKing) ? this._internalParam1 : 0;
			}
		}

		// Token: 0x0600287E RID: 10366 RVA: 0x001EFBAC File Offset: 0x001EDDAC
		private CastBoostEffectDisplayData(CombatSkillKey skillKey)
		{
			this._internalType = -1;
			this._internalParam0 = (this._internalParam1 = 0);
			this.EffectDescription = DomainManager.CombatSkill.GetEffectDisplayData(skillKey);
		}

		// Token: 0x0600287F RID: 10367 RVA: 0x001EFBE4 File Offset: 0x001EDDE4
		public static CastBoostEffectDisplayData CostNeiliAllocation(CombatSkillKey skillKey, byte type, int value)
		{
			return new CastBoostEffectDisplayData(skillKey)
			{
				_internalType = 0,
				_internalParam0 = (int)type,
				_internalParam1 = value
			};
		}

		// Token: 0x06002880 RID: 10368 RVA: 0x001EFC18 File Offset: 0x001EDE18
		public static CastBoostEffectDisplayData CostWugKing(CombatSkillKey skillKey, short wugTemplateId, int count)
		{
			return new CastBoostEffectDisplayData(skillKey)
			{
				_internalType = 1,
				_internalParam0 = (int)wugTemplateId,
				_internalParam1 = count
			};
		}

		// Token: 0x06002881 RID: 10369 RVA: 0x001EFC4B File Offset: 0x001EDE4B
		public CastBoostEffectDisplayData(CastBoostEffectDisplayData other)
		{
			this._internalType = other._internalType;
			this._internalParam0 = other._internalParam0;
			this._internalParam1 = other._internalParam1;
			this.EffectDescription = new CombatSkillEffectDescriptionDisplayData(other.EffectDescription);
		}

		// Token: 0x06002882 RID: 10370 RVA: 0x001EFC83 File Offset: 0x001EDE83
		public void Assign(CastBoostEffectDisplayData other)
		{
			this._internalType = other._internalType;
			this._internalParam0 = other._internalParam0;
			this._internalParam1 = other._internalParam1;
			this.EffectDescription = new CombatSkillEffectDescriptionDisplayData(other.EffectDescription);
		}

		// Token: 0x06002883 RID: 10371 RVA: 0x001EFCBC File Offset: 0x001EDEBC
		public bool IsSerializedSizeFixed()
		{
			return false;
		}

		// Token: 0x06002884 RID: 10372 RVA: 0x001EFCD0 File Offset: 0x001EDED0
		public int GetSerializedSize()
		{
			int totalSize = 9;
			totalSize += this.EffectDescription.GetSerializedSize();
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x06002885 RID: 10373 RVA: 0x001EFD04 File Offset: 0x001EDF04
		public unsafe int Serialize(byte* pData)
		{
			*pData = (byte)this._internalType;
			byte* pCurrData = pData + 1;
			*(int*)pCurrData = this._internalParam0;
			pCurrData += 4;
			*(int*)pCurrData = this._internalParam1;
			pCurrData += 4;
			int fieldSize = this.EffectDescription.Serialize(pCurrData);
			pCurrData += fieldSize;
			Tester.Assert(fieldSize <= 65535, "");
			int totalSize = (int)((long)(pCurrData - pData));
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x06002886 RID: 10374 RVA: 0x001EFD80 File Offset: 0x001EDF80
		public unsafe int Deserialize(byte* pData)
		{
			this._internalType = *(sbyte*)pData;
			byte* pCurrData = pData + 1;
			this._internalParam0 = *(int*)pCurrData;
			pCurrData += 4;
			this._internalParam1 = *(int*)pCurrData;
			pCurrData += 4;
			pCurrData += this.EffectDescription.Deserialize(pCurrData);
			int totalSize = (int)((long)(pCurrData - pData));
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x0400081A RID: 2074
		[SerializableGameDataField]
		private sbyte _internalType;

		// Token: 0x0400081B RID: 2075
		[SerializableGameDataField]
		private int _internalParam0;

		// Token: 0x0400081C RID: 2076
		[SerializableGameDataField]
		private int _internalParam1;

		// Token: 0x0400081D RID: 2077
		[SerializableGameDataField]
		public CombatSkillEffectDescriptionDisplayData EffectDescription;
	}
}
