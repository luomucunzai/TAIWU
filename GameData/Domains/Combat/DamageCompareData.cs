using System;
using GameData.Domains.Character;
using GameData.Serializer;

namespace GameData.Domains.Combat
{
	// Token: 0x020006BF RID: 1727
	public class DamageCompareData : ISerializableGameData
	{
		// Token: 0x06006680 RID: 26240 RVA: 0x003AC2F8 File Offset: 0x003AA4F8
		public DamageCompareData()
		{
			this.Clear();
		}

		// Token: 0x06006681 RID: 26241 RVA: 0x003AC330 File Offset: 0x003AA530
		public void Clear()
		{
			this.SkillId = -1;
			this.OuterAttackValue = -1;
			this.InnerAttackValue = -1;
			this.OuterDefendValue = -1;
			this.InnerDefendValue = -1;
			for (int i = 0; i < 3; i++)
			{
				this.HitType[i] = -1;
				this.HitValue[i] = -1;
				this.AvoidValue[i] = -1;
			}
		}

		// Token: 0x06006682 RID: 26242 RVA: 0x003AC390 File Offset: 0x003AA590
		public CombatProperty GetProperty(int index = 0)
		{
			return new CombatProperty
			{
				HitValue = this.HitValue[index],
				AvoidValue = this.AvoidValue[index],
				AttackValue = new OuterAndInnerInts(this.OuterAttackValue, this.InnerAttackValue),
				DefendValue = new OuterAndInnerInts(this.OuterDefendValue, this.InnerDefendValue)
			};
		}

		// Token: 0x06006683 RID: 26243 RVA: 0x003AC400 File Offset: 0x003AA600
		public bool IsSerializedSizeFixed()
		{
			return true;
		}

		// Token: 0x06006684 RID: 26244 RVA: 0x003AC414 File Offset: 0x003AA614
		public int GetSerializedSize()
		{
			int totalSize = 46;
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x06006685 RID: 26245 RVA: 0x003AC43C File Offset: 0x003AA63C
		public unsafe int Serialize(byte* pData)
		{
			*pData = (this.IsAlly ? 1 : 0);
			byte* pCurrData = pData + 1;
			*(short*)pCurrData = this.SkillId;
			pCurrData += 2;
			*(int*)pCurrData = this.OuterAttackValue;
			pCurrData += 4;
			*(int*)pCurrData = this.InnerAttackValue;
			pCurrData += 4;
			*(int*)pCurrData = this.OuterDefendValue;
			pCurrData += 4;
			*(int*)pCurrData = this.InnerDefendValue;
			pCurrData += 4;
			for (int i = 0; i < 3; i++)
			{
				*pCurrData = (byte)this.HitType[i];
				pCurrData++;
				*(int*)pCurrData = this.HitValue[i];
				pCurrData += 4;
				*(int*)pCurrData = this.AvoidValue[i];
				pCurrData += 4;
			}
			int totalSize = (int)((long)(pCurrData - pData));
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x06006686 RID: 26246 RVA: 0x003AC4F0 File Offset: 0x003AA6F0
		public unsafe int Deserialize(byte* pData)
		{
			this.IsAlly = (*pData != 0);
			byte* pCurrData = pData + 1;
			this.SkillId = *(short*)pCurrData;
			pCurrData += 2;
			this.OuterAttackValue = *(int*)pCurrData;
			pCurrData += 4;
			this.InnerAttackValue = *(int*)pCurrData;
			pCurrData += 4;
			this.OuterDefendValue = *(int*)pCurrData;
			pCurrData += 4;
			this.InnerDefendValue = *(int*)pCurrData;
			pCurrData += 4;
			for (int i = 0; i < 3; i++)
			{
				this.HitType[i] = *(sbyte*)pCurrData;
				pCurrData++;
				this.HitValue[i] = *(int*)pCurrData;
				pCurrData += 4;
				this.AvoidValue[i] = *(int*)pCurrData;
				pCurrData += 4;
			}
			int totalSize = (int)((long)(pCurrData - pData));
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x04001BDF RID: 7135
		public bool IsAlly;

		// Token: 0x04001BE0 RID: 7136
		public short SkillId;

		// Token: 0x04001BE1 RID: 7137
		public int OuterAttackValue;

		// Token: 0x04001BE2 RID: 7138
		public int InnerAttackValue;

		// Token: 0x04001BE3 RID: 7139
		public int OuterDefendValue;

		// Token: 0x04001BE4 RID: 7140
		public int InnerDefendValue;

		// Token: 0x04001BE5 RID: 7141
		public readonly sbyte[] HitType = new sbyte[3];

		// Token: 0x04001BE6 RID: 7142
		public readonly int[] HitValue = new int[3];

		// Token: 0x04001BE7 RID: 7143
		public readonly int[] AvoidValue = new int[3];
	}
}
