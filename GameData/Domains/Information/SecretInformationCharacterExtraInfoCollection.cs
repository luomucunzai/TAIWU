using System;
using System.Collections.Generic;
using GameData.Serializer;

namespace GameData.Domains.Information
{
	// Token: 0x0200067C RID: 1660
	[SerializableGameData(NotForDisplayModule = true)]
	public class SecretInformationCharacterExtraInfoCollection : ISerializableGameData
	{
		// Token: 0x060053D2 RID: 21458 RVA: 0x002DBDB7 File Offset: 0x002D9FB7
		public SecretInformationCharacterExtraInfoCollection()
		{
			this.Collection = new Dictionary<int, SecretInformationCharacterExtraInfo>();
		}

		// Token: 0x060053D3 RID: 21459 RVA: 0x002DBDCC File Offset: 0x002D9FCC
		public bool IsSerializedSizeFixed()
		{
			return false;
		}

		// Token: 0x060053D4 RID: 21460 RVA: 0x002DBDD0 File Offset: 0x002D9FD0
		public int GetSerializedSize()
		{
			int size = 4;
			foreach (KeyValuePair<int, SecretInformationCharacterExtraInfo> pair in this.Collection)
			{
				size += 4 + pair.Value.GetSerializedSize();
			}
			return size;
		}

		// Token: 0x060053D5 RID: 21461 RVA: 0x002DBE38 File Offset: 0x002DA038
		public unsafe int Serialize(byte* pData)
		{
			byte* header = pData;
			*(int*)pData = this.Collection.Count;
			pData += 4;
			foreach (KeyValuePair<int, SecretInformationCharacterExtraInfo> pair in this.Collection)
			{
				*(int*)pData = pair.Key;
				pData += 4;
				pData += pair.Value.Serialize(pData);
			}
			return (int)((long)(pData - header));
		}

		// Token: 0x060053D6 RID: 21462 RVA: 0x002DBEC8 File Offset: 0x002DA0C8
		public unsafe int Deserialize(byte* pData)
		{
			byte* header = pData;
			this.Collection.Clear();
			int count = *(int*)pData;
			pData += 4;
			for (int i = 0; i < count; i++)
			{
				int j = *(int*)pData;
				SecretInformationCharacterExtraInfo v = default(SecretInformationCharacterExtraInfo);
				pData += 4;
				pData += v.Deserialize(pData);
				this.Collection.Add(j, v);
			}
			return (int)((long)(pData - header));
		}

		// Token: 0x04001671 RID: 5745
		public readonly IDictionary<int, SecretInformationCharacterExtraInfo> Collection;
	}
}
