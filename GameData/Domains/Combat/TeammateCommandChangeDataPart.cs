using System;
using System.Collections.Generic;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Combat
{
	// Token: 0x020006D7 RID: 1751
	[SerializableGameData(NotForArchive = true)]
	public class TeammateCommandChangeDataPart : ISerializableGameData
	{
		// Token: 0x0600676A RID: 26474 RVA: 0x003B17CA File Offset: 0x003AF9CA
		public TeammateCommandChangeDataPart()
		{
		}

		// Token: 0x0600676B RID: 26475 RVA: 0x003B1800 File Offset: 0x003AFA00
		public TeammateCommandChangeDataPart(TeammateCommandChangeDataPart other)
		{
			this.TeammateCharIds = ((other.TeammateCharIds == null) ? null : new List<int>(other.TeammateCharIds));
			bool flag = other.OriginTeammateCommands != null;
			if (flag)
			{
				List<SByteList> item = other.OriginTeammateCommands;
				int elementsCount = item.Count;
				this.OriginTeammateCommands = new List<SByteList>(elementsCount);
				for (int i = 0; i < elementsCount; i++)
				{
					this.OriginTeammateCommands.Add(new SByteList(item[i]));
				}
			}
			else
			{
				this.OriginTeammateCommands = null;
			}
			bool flag2 = other.ReplaceTeammateCommands != null;
			if (flag2)
			{
				List<SByteList> item2 = other.ReplaceTeammateCommands;
				int elementsCount2 = item2.Count;
				this.ReplaceTeammateCommands = new List<SByteList>(elementsCount2);
				for (int j = 0; j < elementsCount2; j++)
				{
					this.ReplaceTeammateCommands.Add(new SByteList(item2[j]));
				}
			}
			else
			{
				this.ReplaceTeammateCommands = null;
			}
			this.BetrayedCharIds = ((other.BetrayedCharIds == null) ? null : new Dictionary<int, int>(other.BetrayedCharIds));
		}

		// Token: 0x0600676C RID: 26476 RVA: 0x003B1940 File Offset: 0x003AFB40
		public void Assign(TeammateCommandChangeDataPart other)
		{
			this.TeammateCharIds = ((other.TeammateCharIds == null) ? null : new List<int>(other.TeammateCharIds));
			bool flag = other.OriginTeammateCommands != null;
			if (flag)
			{
				List<SByteList> item = other.OriginTeammateCommands;
				int elementsCount = item.Count;
				this.OriginTeammateCommands = new List<SByteList>(elementsCount);
				for (int i = 0; i < elementsCount; i++)
				{
					this.OriginTeammateCommands.Add(new SByteList(item[i]));
				}
			}
			else
			{
				this.OriginTeammateCommands = null;
			}
			bool flag2 = other.ReplaceTeammateCommands != null;
			if (flag2)
			{
				List<SByteList> item2 = other.ReplaceTeammateCommands;
				int elementsCount2 = item2.Count;
				this.ReplaceTeammateCommands = new List<SByteList>(elementsCount2);
				for (int j = 0; j < elementsCount2; j++)
				{
					this.ReplaceTeammateCommands.Add(new SByteList(item2[j]));
				}
			}
			else
			{
				this.ReplaceTeammateCommands = null;
			}
			this.BetrayedCharIds = ((other.BetrayedCharIds == null) ? null : new Dictionary<int, int>(other.BetrayedCharIds));
		}

		// Token: 0x0600676D RID: 26477 RVA: 0x003B1A4C File Offset: 0x003AFC4C
		public bool IsSerializedSizeFixed()
		{
			return false;
		}

		// Token: 0x0600676E RID: 26478 RVA: 0x003B1A60 File Offset: 0x003AFC60
		public int GetSerializedSize()
		{
			int totalSize = 0;
			bool flag = this.TeammateCharIds != null;
			if (flag)
			{
				totalSize += 2 + 4 * this.TeammateCharIds.Count;
			}
			else
			{
				totalSize += 2;
			}
			bool flag2 = this.OriginTeammateCommands != null;
			if (flag2)
			{
				totalSize += 2;
				int elementsCount = this.OriginTeammateCommands.Count;
				for (int i = 0; i < elementsCount; i++)
				{
					totalSize += this.OriginTeammateCommands[i].GetSerializedSize();
				}
			}
			else
			{
				totalSize += 2;
			}
			bool flag3 = this.ReplaceTeammateCommands != null;
			if (flag3)
			{
				totalSize += 2;
				int elementsCount2 = this.ReplaceTeammateCommands.Count;
				for (int j = 0; j < elementsCount2; j++)
				{
					totalSize += this.ReplaceTeammateCommands[j].GetSerializedSize();
				}
			}
			else
			{
				totalSize += 2;
			}
			totalSize += SerializationHelper.DictionaryOfBasicTypePair.GetSerializedSize<int, int>(this.BetrayedCharIds);
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x0600676F RID: 26479 RVA: 0x003B1B68 File Offset: 0x003AFD68
		public unsafe int Serialize(byte* pData)
		{
			bool flag = this.TeammateCharIds != null;
			byte* pCurrData;
			if (flag)
			{
				int elementsCount = this.TeammateCharIds.Count;
				Tester.Assert(elementsCount <= 65535, "");
				*(short*)pData = (short)((ushort)elementsCount);
				pCurrData = pData + 2;
				for (int i = 0; i < elementsCount; i++)
				{
					*(int*)(pCurrData + (IntPtr)i * 4) = this.TeammateCharIds[i];
				}
				pCurrData += 4 * elementsCount;
			}
			else
			{
				*(short*)pData = 0;
				pCurrData = pData + 2;
			}
			bool flag2 = this.OriginTeammateCommands != null;
			if (flag2)
			{
				int elementsCount2 = this.OriginTeammateCommands.Count;
				Tester.Assert(elementsCount2 <= 65535, "");
				*(short*)pCurrData = (short)((ushort)elementsCount2);
				pCurrData += 2;
				for (int j = 0; j < elementsCount2; j++)
				{
					int subDataSize = this.OriginTeammateCommands[j].Serialize(pCurrData);
					pCurrData += subDataSize;
					Tester.Assert(subDataSize <= 65535, "");
				}
			}
			else
			{
				*(short*)pCurrData = 0;
				pCurrData += 2;
			}
			bool flag3 = this.ReplaceTeammateCommands != null;
			if (flag3)
			{
				int elementsCount3 = this.ReplaceTeammateCommands.Count;
				Tester.Assert(elementsCount3 <= 65535, "");
				*(short*)pCurrData = (short)((ushort)elementsCount3);
				pCurrData += 2;
				for (int k = 0; k < elementsCount3; k++)
				{
					int subDataSize2 = this.ReplaceTeammateCommands[k].Serialize(pCurrData);
					pCurrData += subDataSize2;
					Tester.Assert(subDataSize2 <= 65535, "");
				}
			}
			else
			{
				*(short*)pCurrData = 0;
				pCurrData += 2;
			}
			pCurrData += SerializationHelper.DictionaryOfBasicTypePair.Serialize<int, int>(pCurrData, ref this.BetrayedCharIds);
			int totalSize = (int)((long)(pCurrData - pData));
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x06006770 RID: 26480 RVA: 0x003B1D50 File Offset: 0x003AFF50
		public unsafe int Deserialize(byte* pData)
		{
			ushort elementsCount = *(ushort*)pData;
			byte* pCurrData = pData + 2;
			bool flag = elementsCount > 0;
			if (flag)
			{
				bool flag2 = this.TeammateCharIds == null;
				if (flag2)
				{
					this.TeammateCharIds = new List<int>((int)elementsCount);
				}
				else
				{
					this.TeammateCharIds.Clear();
				}
				for (int i = 0; i < (int)elementsCount; i++)
				{
					this.TeammateCharIds.Add(*(int*)(pCurrData + (IntPtr)i * 4));
				}
				pCurrData += 4 * elementsCount;
			}
			else
			{
				List<int> teammateCharIds = this.TeammateCharIds;
				if (teammateCharIds != null)
				{
					teammateCharIds.Clear();
				}
			}
			ushort elementsCount2 = *(ushort*)pCurrData;
			pCurrData += 2;
			bool flag3 = elementsCount2 > 0;
			if (flag3)
			{
				bool flag4 = this.OriginTeammateCommands == null;
				if (flag4)
				{
					this.OriginTeammateCommands = new List<SByteList>((int)elementsCount2);
				}
				else
				{
					this.OriginTeammateCommands.Clear();
				}
				for (int j = 0; j < (int)elementsCount2; j++)
				{
					SByteList element = default(SByteList);
					pCurrData += element.Deserialize(pCurrData);
					this.OriginTeammateCommands.Add(element);
				}
			}
			else
			{
				List<SByteList> originTeammateCommands = this.OriginTeammateCommands;
				if (originTeammateCommands != null)
				{
					originTeammateCommands.Clear();
				}
			}
			ushort elementsCount3 = *(ushort*)pCurrData;
			pCurrData += 2;
			bool flag5 = elementsCount3 > 0;
			if (flag5)
			{
				bool flag6 = this.ReplaceTeammateCommands == null;
				if (flag6)
				{
					this.ReplaceTeammateCommands = new List<SByteList>((int)elementsCount3);
				}
				else
				{
					this.ReplaceTeammateCommands.Clear();
				}
				for (int k = 0; k < (int)elementsCount3; k++)
				{
					SByteList element2 = default(SByteList);
					pCurrData += element2.Deserialize(pCurrData);
					this.ReplaceTeammateCommands.Add(element2);
				}
			}
			else
			{
				List<SByteList> replaceTeammateCommands = this.ReplaceTeammateCommands;
				if (replaceTeammateCommands != null)
				{
					replaceTeammateCommands.Clear();
				}
			}
			pCurrData += SerializationHelper.DictionaryOfBasicTypePair.Deserialize<int, int>(pCurrData, ref this.BetrayedCharIds);
			int totalSize = (int)((long)(pCurrData - pData));
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x04001C45 RID: 7237
		[SerializableGameDataField]
		public List<int> TeammateCharIds = new List<int>();

		// Token: 0x04001C46 RID: 7238
		[SerializableGameDataField]
		public List<SByteList> OriginTeammateCommands = new List<SByteList>();

		// Token: 0x04001C47 RID: 7239
		[SerializableGameDataField]
		public List<SByteList> ReplaceTeammateCommands = new List<SByteList>();

		// Token: 0x04001C48 RID: 7240
		[SerializableGameDataField]
		public Dictionary<int, int> BetrayedCharIds = new Dictionary<int, int>();
	}
}
