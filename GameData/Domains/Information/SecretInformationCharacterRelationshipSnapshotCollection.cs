using System;
using System.Collections.Generic;
using GameData.Serializer;

namespace GameData.Domains.Information
{
	// Token: 0x0200067E RID: 1662
	[SerializableGameData(NotForDisplayModule = true)]
	public class SecretInformationCharacterRelationshipSnapshotCollection : ISerializableGameData
	{
		// Token: 0x060053DC RID: 21468 RVA: 0x002DC092 File Offset: 0x002DA292
		public SecretInformationCharacterRelationshipSnapshotCollection()
		{
			this.Collection = new Dictionary<int, SecretInformationCharacterRelationshipSnapshot>();
		}

		// Token: 0x060053DD RID: 21469 RVA: 0x002DC0A7 File Offset: 0x002DA2A7
		public SecretInformationCharacterRelationshipSnapshotCollection(SecretInformationCharacterRelationshipSnapshotCollection other) : this()
		{
			this.Assign(other);
		}

		// Token: 0x060053DE RID: 21470 RVA: 0x002DC0BC File Offset: 0x002DA2BC
		public void Assign(SecretInformationCharacterRelationshipSnapshotCollection other)
		{
			this.Collection.Clear();
			foreach (KeyValuePair<int, SecretInformationCharacterRelationshipSnapshot> pair in other.Collection)
			{
				this.Collection.Add(pair.Key, pair.Value);
			}
		}

		// Token: 0x060053DF RID: 21471 RVA: 0x002DC130 File Offset: 0x002DA330
		public bool IsSerializedSizeFixed()
		{
			return false;
		}

		// Token: 0x060053E0 RID: 21472 RVA: 0x002DC134 File Offset: 0x002DA334
		public int GetSerializedSizeActually()
		{
			int size = 4;
			foreach (KeyValuePair<int, SecretInformationCharacterRelationshipSnapshot> pair in this.Collection)
			{
				size += 4 + pair.Value.GetSerializedSize();
			}
			return size;
		}

		// Token: 0x060053E1 RID: 21473 RVA: 0x002DC198 File Offset: 0x002DA398
		public int GetSerializedSize()
		{
			int size = 4;
			foreach (KeyValuePair<int, SecretInformationCharacterRelationshipSnapshot> pair in this.Collection)
			{
				size += 4 + pair.Value.GetSerializedSize();
			}
			bool flag = size >= 65535;
			if (flag)
			{
				size = 4;
			}
			return size;
		}

		// Token: 0x060053E2 RID: 21474 RVA: 0x002DC210 File Offset: 0x002DA410
		public unsafe int Serialize(byte* pData)
		{
			byte* header = pData;
			bool flag = this.GetSerializedSizeActually() >= 65535;
			if (flag)
			{
				this.Collection.Clear();
			}
			*(int*)pData = this.Collection.Count;
			pData += 4;
			foreach (KeyValuePair<int, SecretInformationCharacterRelationshipSnapshot> pair in this.Collection)
			{
				*(int*)pData = pair.Key;
				pData += 4;
				pData += pair.Value.Serialize(pData);
			}
			return (int)((long)(pData - header));
		}

		// Token: 0x060053E3 RID: 21475 RVA: 0x002DC2C0 File Offset: 0x002DA4C0
		public unsafe int Deserialize(byte* pData)
		{
			byte* header = pData;
			this.Collection.Clear();
			int count = *(int*)pData;
			pData += 4;
			for (int i = 0; i < count; i++)
			{
				int j = *(int*)pData;
				SecretInformationCharacterRelationshipSnapshot v = new SecretInformationCharacterRelationshipSnapshot();
				pData += 4;
				pData += v.Deserialize(pData);
				this.Collection.Add(j, v);
			}
			return (int)((long)(pData - header));
		}

		// Token: 0x04001673 RID: 5747
		[SerializableGameDataField]
		public readonly IDictionary<int, SecretInformationCharacterRelationshipSnapshot> Collection;
	}
}
