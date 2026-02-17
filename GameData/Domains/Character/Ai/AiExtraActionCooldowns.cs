using System;
using System.Collections.Generic;
using GameData.Serializer;

namespace GameData.Domains.Character.Ai
{
	// Token: 0x0200084A RID: 2122
	[SerializableGameData(NotForDisplayModule = true)]
	public class AiExtraActionCooldowns : ISerializableGameData
	{
		// Token: 0x06007616 RID: 30230 RVA: 0x0044F403 File Offset: 0x0044D603
		public AiExtraActionCooldowns()
		{
			this.OffCooldownDates = new Dictionary<AiActionKey, int>();
		}

		// Token: 0x06007617 RID: 30231 RVA: 0x0044F418 File Offset: 0x0044D618
		public bool IsSerializedSizeFixed()
		{
			return false;
		}

		// Token: 0x06007618 RID: 30232 RVA: 0x0044F42C File Offset: 0x0044D62C
		public int GetSerializedSize()
		{
			int num = 2;
			Dictionary<AiActionKey, int> offCooldownDates = this.OffCooldownDates;
			int totalSize = num + ((offCooldownDates != null) ? offCooldownDates.Count : 0) * 6;
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x06007619 RID: 30233 RVA: 0x0044F468 File Offset: 0x0044D668
		public unsafe int Serialize(byte* pData)
		{
			byte* pCurrData = pData;
			*(short*)pCurrData = (short)this.OffCooldownDates.Count;
			pCurrData += 2;
			foreach (KeyValuePair<AiActionKey, int> pair in this.OffCooldownDates)
			{
				pCurrData += pair.Key.Serialize(pCurrData);
				*(int*)pCurrData = pair.Value;
				pCurrData += 4;
			}
			int totalSize = (int)((long)(pCurrData - pData));
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x0600761A RID: 30234 RVA: 0x0044F510 File Offset: 0x0044D710
		public unsafe int Deserialize(byte* pData)
		{
			this.OffCooldownDates.Clear();
			short count = *(short*)pData;
			byte* pCurrData = pData + 2;
			for (int i = 0; i < (int)count; i++)
			{
				AiActionKey key = default(AiActionKey);
				pCurrData += key.Deserialize(pCurrData);
				int value = *(int*)pCurrData;
				pCurrData += 4;
				this.OffCooldownDates.Add(key, value);
			}
			int totalSize = (int)((long)(pCurrData - pData));
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x04002090 RID: 8336
		[SerializableGameDataField]
		public Dictionary<AiActionKey, int> OffCooldownDates;
	}
}
