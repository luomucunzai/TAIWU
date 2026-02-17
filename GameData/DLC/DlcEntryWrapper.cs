using System;
using GameData.Serializer;

namespace GameData.DLC
{
	// Token: 0x020008D7 RID: 2263
	[SerializableGameData(NotForDisplayModule = true)]
	public class DlcEntryWrapper : ISerializableGameData
	{
		// Token: 0x06008136 RID: 33078 RVA: 0x004D0556 File Offset: 0x004CE756
		public DlcEntryWrapper()
		{
		}

		// Token: 0x06008137 RID: 33079 RVA: 0x004D0560 File Offset: 0x004CE760
		public DlcEntryWrapper(DlcId dlcId, IDlcEntry dlcEntry)
		{
			this._dlcId = dlcId;
			this._dlcEntry = dlcEntry;
		}

		// Token: 0x06008138 RID: 33080 RVA: 0x004D0578 File Offset: 0x004CE778
		public IDlcEntry GetDlcEntry()
		{
			return this._dlcEntry;
		}

		// Token: 0x06008139 RID: 33081 RVA: 0x004D0580 File Offset: 0x004CE780
		public void Update(ulong version, IDlcEntry entry)
		{
			this._dlcId.Version = version;
			this._dlcEntry = entry;
		}

		// Token: 0x0600813A RID: 33082 RVA: 0x004D0598 File Offset: 0x004CE798
		public bool IsSerializedSizeFixed()
		{
			return false;
		}

		// Token: 0x0600813B RID: 33083 RVA: 0x004D05AC File Offset: 0x004CE7AC
		public int GetSerializedSize()
		{
			int num = this._dlcId.GetSerializedSize() + 4;
			IDlcEntry dlcEntry = this._dlcEntry;
			return num + ((dlcEntry != null) ? dlcEntry.GetSerializedSize() : 0);
		}

		// Token: 0x0600813C RID: 33084 RVA: 0x004D05E0 File Offset: 0x004CE7E0
		public unsafe int Serialize(byte* pData)
		{
			byte* pCurrData = pData + this._dlcId.Serialize(pData);
			bool flag = this._dlcEntry != null;
			if (flag)
			{
				byte* pFieldSize = pCurrData;
				pCurrData += 4;
				int fieldSize = this._dlcEntry.Serialize(pCurrData);
				pCurrData += fieldSize;
				*(int*)pFieldSize = fieldSize;
			}
			else
			{
				*(int*)pCurrData = 0;
				pCurrData += 4;
			}
			return (int)((long)(pCurrData - pData));
		}

		// Token: 0x0600813D RID: 33085 RVA: 0x004D0640 File Offset: 0x004CE840
		public unsafe int Deserialize(byte* pData)
		{
			byte* pCurrData = pData + this._dlcId.Deserialize(pData);
			uint fieldSize = *(uint*)pCurrData;
			pCurrData += 4;
			this._dlcEntry = DlcManager.CreateDlcEntry(this._dlcId);
			bool flag = fieldSize > 0U;
			if (flag)
			{
				pCurrData += this._dlcEntry.Deserialize(pCurrData);
			}
			return (int)((long)(pCurrData - pData));
		}

		// Token: 0x04002384 RID: 9092
		[SerializableGameDataField]
		private DlcId _dlcId;

		// Token: 0x04002385 RID: 9093
		[SerializableGameDataField]
		private IDlcEntry _dlcEntry;
	}
}
