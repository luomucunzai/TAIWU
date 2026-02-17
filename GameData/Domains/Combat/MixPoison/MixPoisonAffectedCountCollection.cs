using System;
using System.Collections.Generic;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Combat.MixPoison
{
	// Token: 0x0200070B RID: 1803
	[SerializableGameData]
	public class MixPoisonAffectedCountCollection : ISerializableGameData
	{
		// Token: 0x06006823 RID: 26659 RVA: 0x003B3BF4 File Offset: 0x003B1DF4
		private Dictionary<sbyte, int> GetOrCreateMixPoisonAffectedCount()
		{
			Dictionary<sbyte, int> result;
			if ((result = this._mixPoisonAffectedCount) == null)
			{
				result = (this._mixPoisonAffectedCount = new Dictionary<sbyte, int>());
			}
			return result;
		}

		// Token: 0x06006824 RID: 26660 RVA: 0x003B3C1E File Offset: 0x003B1E1E
		public int GetAffectedCount(sbyte templateId)
		{
			return this.GetOrCreateMixPoisonAffectedCount().GetOrDefault(templateId);
		}

		// Token: 0x06006825 RID: 26661 RVA: 0x003B3C2C File Offset: 0x003B1E2C
		public MixPoisonAffectedCountCollection AddAffectedCount(sbyte templateId)
		{
			this.GetOrCreateMixPoisonAffectedCount()[templateId] = this.GetAffectedCount(templateId) + 1;
			return this;
		}

		// Token: 0x06006826 RID: 26662 RVA: 0x003B3C58 File Offset: 0x003B1E58
		public MixPoisonAffectedCountCollection Clear()
		{
			this.GetOrCreateMixPoisonAffectedCount().Clear();
			return this;
		}

		// Token: 0x06006827 RID: 26663 RVA: 0x003B3C77 File Offset: 0x003B1E77
		public MixPoisonAffectedCountCollection()
		{
		}

		// Token: 0x06006828 RID: 26664 RVA: 0x003B3C81 File Offset: 0x003B1E81
		public MixPoisonAffectedCountCollection(MixPoisonAffectedCountCollection other)
		{
			this._mixPoisonAffectedCount = ((other._mixPoisonAffectedCount == null) ? null : new Dictionary<sbyte, int>(other._mixPoisonAffectedCount));
		}

		// Token: 0x06006829 RID: 26665 RVA: 0x003B3CA7 File Offset: 0x003B1EA7
		public void Assign(MixPoisonAffectedCountCollection other)
		{
			this._mixPoisonAffectedCount = ((other._mixPoisonAffectedCount == null) ? null : new Dictionary<sbyte, int>(other._mixPoisonAffectedCount));
		}

		// Token: 0x0600682A RID: 26666 RVA: 0x003B3CC8 File Offset: 0x003B1EC8
		public bool IsSerializedSizeFixed()
		{
			return false;
		}

		// Token: 0x0600682B RID: 26667 RVA: 0x003B3CDC File Offset: 0x003B1EDC
		public int GetSerializedSize()
		{
			int totalSize = 0;
			totalSize += SerializationHelper.DictionaryOfBasicTypePair.GetSerializedSize<sbyte, int>(this._mixPoisonAffectedCount);
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x0600682C RID: 26668 RVA: 0x003B3D10 File Offset: 0x003B1F10
		public unsafe int Serialize(byte* pData)
		{
			byte* pCurrData = pData + SerializationHelper.DictionaryOfBasicTypePair.Serialize<sbyte, int>(pData, ref this._mixPoisonAffectedCount);
			int totalSize = (int)((long)(pCurrData - pData));
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x0600682D RID: 26669 RVA: 0x003B3D4C File Offset: 0x003B1F4C
		public unsafe int Deserialize(byte* pData)
		{
			byte* pCurrData = pData + SerializationHelper.DictionaryOfBasicTypePair.Deserialize<sbyte, int>(pData, ref this._mixPoisonAffectedCount);
			int totalSize = (int)((long)(pCurrData - pData));
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x04001C82 RID: 7298
		[SerializableGameDataField]
		private Dictionary<sbyte, int> _mixPoisonAffectedCount;
	}
}
