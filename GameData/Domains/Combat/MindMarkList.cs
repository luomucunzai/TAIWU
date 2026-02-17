using System;
using System.Collections.Generic;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Combat
{
	// Token: 0x020006CB RID: 1739
	[SerializableGameData]
	public class MindMarkList : ISerializableGameData
	{
		// Token: 0x060066FA RID: 26362 RVA: 0x003AF678 File Offset: 0x003AD878
		public MindMarkList()
		{
		}

		// Token: 0x060066FB RID: 26363 RVA: 0x003AF68D File Offset: 0x003AD88D
		public MindMarkList(MindMarkList other)
		{
			this.MarkList = ((other.MarkList == null) ? null : new List<SilenceFrameData>(other.MarkList));
		}

		// Token: 0x060066FC RID: 26364 RVA: 0x003AF6BE File Offset: 0x003AD8BE
		public void Assign(MindMarkList other)
		{
			this.MarkList = ((other.MarkList == null) ? null : new List<SilenceFrameData>(other.MarkList));
		}

		// Token: 0x060066FD RID: 26365 RVA: 0x003AF6E0 File Offset: 0x003AD8E0
		public bool IsSerializedSizeFixed()
		{
			return false;
		}

		// Token: 0x060066FE RID: 26366 RVA: 0x003AF6F4 File Offset: 0x003AD8F4
		public int GetSerializedSize()
		{
			int totalSize = 0;
			bool flag = this.MarkList != null;
			if (flag)
			{
				totalSize += 2 + 8 * this.MarkList.Count;
			}
			else
			{
				totalSize += 2;
			}
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x060066FF RID: 26367 RVA: 0x003AF740 File Offset: 0x003AD940
		public unsafe int Serialize(byte* pData)
		{
			bool flag = this.MarkList != null;
			byte* pCurrData;
			if (flag)
			{
				int elementsCount = this.MarkList.Count;
				Tester.Assert(elementsCount <= 65535, "");
				*(short*)pData = (short)((ushort)elementsCount);
				pCurrData = pData + 2;
				for (int i = 0; i < elementsCount; i++)
				{
					pCurrData += this.MarkList[i].Serialize(pCurrData);
				}
			}
			else
			{
				*(short*)pData = 0;
				pCurrData = pData + 2;
			}
			int totalSize = (int)((long)(pCurrData - pData));
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x06006700 RID: 26368 RVA: 0x003AF7E8 File Offset: 0x003AD9E8
		public unsafe int Deserialize(byte* pData)
		{
			ushort elementsCount = *(ushort*)pData;
			byte* pCurrData = pData + 2;
			bool flag = elementsCount > 0;
			if (flag)
			{
				bool flag2 = this.MarkList == null;
				if (flag2)
				{
					this.MarkList = new List<SilenceFrameData>((int)elementsCount);
				}
				else
				{
					this.MarkList.Clear();
				}
				for (int i = 0; i < (int)elementsCount; i++)
				{
					SilenceFrameData element = default(SilenceFrameData);
					pCurrData += element.Deserialize(pCurrData);
					this.MarkList.Add(element);
				}
			}
			else
			{
				List<SilenceFrameData> markList = this.MarkList;
				if (markList != null)
				{
					markList.Clear();
				}
			}
			int totalSize = (int)((long)(pCurrData - pData));
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x04001C03 RID: 7171
		[SerializableGameDataField]
		public List<SilenceFrameData> MarkList = new List<SilenceFrameData>();
	}
}
