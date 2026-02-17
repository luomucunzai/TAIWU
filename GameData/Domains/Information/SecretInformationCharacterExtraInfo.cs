using System;
using GameData.Domains.Character;
using GameData.Serializer;

namespace GameData.Domains.Information
{
	// Token: 0x0200067B RID: 1659
	[SerializableGameData(NotForDisplayModule = true)]
	public struct SecretInformationCharacterExtraInfo : ISerializableGameData
	{
		// Token: 0x060053CE RID: 21454 RVA: 0x002DBCBC File Offset: 0x002D9EBC
		public bool IsSerializedSizeFixed()
		{
			return true;
		}

		// Token: 0x060053CF RID: 21455 RVA: 0x002DBCD0 File Offset: 0x002D9ED0
		public int GetSerializedSize()
		{
			int totalSize = 11;
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x060053D0 RID: 21456 RVA: 0x002DBCF8 File Offset: 0x002D9EF8
		public unsafe int Serialize(byte* pData)
		{
			byte* pCurrData = pData + this.OrgInfo.Serialize(pData);
			*pCurrData = (byte)this.FameType;
			pCurrData++;
			*pCurrData = this.MonkType;
			pCurrData++;
			*pCurrData = (byte)this.AliveState;
			pCurrData++;
			int totalSize = (int)((long)(pCurrData - pData));
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x060053D1 RID: 21457 RVA: 0x002DBD58 File Offset: 0x002D9F58
		public unsafe int Deserialize(byte* pData)
		{
			byte* pCurrData = pData + this.OrgInfo.Deserialize(pData);
			this.FameType = *(sbyte*)pCurrData;
			pCurrData++;
			this.MonkType = *pCurrData;
			pCurrData++;
			this.AliveState = *(sbyte*)pCurrData;
			pCurrData++;
			int totalSize = (int)((long)(pCurrData - pData));
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x0400166D RID: 5741
		[SerializableGameDataField]
		public OrganizationInfo OrgInfo;

		// Token: 0x0400166E RID: 5742
		[SerializableGameDataField]
		public sbyte FameType;

		// Token: 0x0400166F RID: 5743
		[SerializableGameDataField]
		public byte MonkType;

		// Token: 0x04001670 RID: 5744
		[SerializableGameDataField]
		public sbyte AliveState;
	}
}
