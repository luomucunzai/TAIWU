using System;
using GameData.Domains.Character;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Combat
{
	// Token: 0x02000692 RID: 1682
	public class CombatCharacterDisplayData : ISerializableGameData
	{
		// Token: 0x060061C4 RID: 25028 RVA: 0x00378066 File Offset: 0x00376266
		public CombatCharacterDisplayData()
		{
		}

		// Token: 0x060061C5 RID: 25029 RVA: 0x00378070 File Offset: 0x00376270
		public CombatCharacterDisplayData(CombatCharacterDisplayData other)
		{
			this.DefeatMarks = new DefeatMarkCollection(other.DefeatMarks);
			this.OldInjuries = other.OldInjuries;
			this.OldPoisons = other.OldPoisons;
			this.OldDisorderOfQi = other.OldDisorderOfQi;
			this.Happiness = other.Happiness;
		}

		// Token: 0x060061C6 RID: 25030 RVA: 0x003780C8 File Offset: 0x003762C8
		public void Assign(CombatCharacterDisplayData other)
		{
			this.DefeatMarks = new DefeatMarkCollection(other.DefeatMarks);
			this.OldInjuries = other.OldInjuries;
			this.OldPoisons = other.OldPoisons;
			this.OldDisorderOfQi = other.OldDisorderOfQi;
			this.Happiness = other.Happiness;
		}

		// Token: 0x060061C7 RID: 25031 RVA: 0x00378118 File Offset: 0x00376318
		public bool IsSerializedSizeFixed()
		{
			return false;
		}

		// Token: 0x060061C8 RID: 25032 RVA: 0x0037812C File Offset: 0x0037632C
		public int GetSerializedSize()
		{
			int totalSize = 43;
			bool flag = this.DefeatMarks != null;
			if (flag)
			{
				totalSize += 2 + this.DefeatMarks.GetSerializedSize();
			}
			else
			{
				totalSize += 2;
			}
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x060061C9 RID: 25033 RVA: 0x00378174 File Offset: 0x00376374
		public unsafe int Serialize(byte* pData)
		{
			bool flag = this.DefeatMarks != null;
			byte* pCurrData;
			if (flag)
			{
				pCurrData = pData + 2;
				int fieldSize = this.DefeatMarks.Serialize(pCurrData);
				pCurrData += fieldSize;
				Tester.Assert(fieldSize <= 65535, "");
				*(short*)pData = (short)((ushort)fieldSize);
			}
			else
			{
				*(short*)pData = 0;
				pCurrData = pData + 2;
			}
			pCurrData += this.OldInjuries.Serialize(pCurrData);
			pCurrData += this.OldPoisons.Serialize(pCurrData);
			*(short*)pCurrData = this.OldDisorderOfQi;
			pCurrData += 2;
			*pCurrData = (byte)this.Happiness;
			pCurrData++;
			int totalSize = (int)((long)(pCurrData - pData));
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x060061CA RID: 25034 RVA: 0x0037822C File Offset: 0x0037642C
		public unsafe int Deserialize(byte* pData)
		{
			ushort fieldSize = *(ushort*)pData;
			byte* pCurrData = pData + 2;
			bool flag = fieldSize > 0;
			if (flag)
			{
				bool flag2 = this.DefeatMarks == null;
				if (flag2)
				{
					this.DefeatMarks = new DefeatMarkCollection();
				}
				pCurrData += this.DefeatMarks.Deserialize(pCurrData);
			}
			else
			{
				this.DefeatMarks = null;
			}
			pCurrData += this.OldInjuries.Deserialize(pCurrData);
			pCurrData += this.OldPoisons.Deserialize(pCurrData);
			this.OldDisorderOfQi = *(short*)pCurrData;
			pCurrData += 2;
			this.Happiness = *(sbyte*)pCurrData;
			pCurrData++;
			int totalSize = (int)((long)(pCurrData - pData));
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x04001A74 RID: 6772
		[SerializableGameDataField]
		public DefeatMarkCollection DefeatMarks;

		// Token: 0x04001A75 RID: 6773
		[SerializableGameDataField]
		public Injuries OldInjuries;

		// Token: 0x04001A76 RID: 6774
		[SerializableGameDataField]
		public PoisonInts OldPoisons;

		// Token: 0x04001A77 RID: 6775
		[SerializableGameDataField]
		public short OldDisorderOfQi;

		// Token: 0x04001A78 RID: 6776
		[SerializableGameDataField]
		public sbyte Happiness;
	}
}
