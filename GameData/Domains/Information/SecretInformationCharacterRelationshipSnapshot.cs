using System;
using GameData.Domains.Character.Relation;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Information
{
	// Token: 0x0200067D RID: 1661
	[SerializableGameData(NotForDisplayModule = true)]
	public class SecretInformationCharacterRelationshipSnapshot : ISerializableGameData
	{
		// Token: 0x060053D7 RID: 21463 RVA: 0x002DBF38 File Offset: 0x002DA138
		public bool IsSerializedSizeFixed()
		{
			return false;
		}

		// Token: 0x060053D8 RID: 21464 RVA: 0x002DBF4C File Offset: 0x002DA14C
		public int GetSerializedSize()
		{
			int totalSize = 0;
			bool flag = this.RelatedCharacters != null;
			if (flag)
			{
				totalSize += 2 + this.RelatedCharacters.GetSerializedSize();
			}
			else
			{
				totalSize += 2;
			}
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x060053D9 RID: 21465 RVA: 0x002DBF94 File Offset: 0x002DA194
		public unsafe int Serialize(byte* pData)
		{
			bool flag = this.RelatedCharacters != null;
			byte* pCurrData;
			if (flag)
			{
				pCurrData = pData + 2;
				int fieldSize = this.RelatedCharacters.Serialize(pCurrData);
				pCurrData += fieldSize;
				Tester.Assert(fieldSize <= 65535, "");
				*(short*)pData = (short)((ushort)fieldSize);
			}
			else
			{
				*(short*)pData = 0;
				pCurrData = pData + 2;
			}
			int totalSize = (int)((long)(pCurrData - pData));
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x060053DA RID: 21466 RVA: 0x002DC014 File Offset: 0x002DA214
		public unsafe int Deserialize(byte* pData)
		{
			ushort fieldSize = *(ushort*)pData;
			byte* pCurrData = pData + 2;
			bool flag = fieldSize > 0;
			if (flag)
			{
				bool flag2 = this.RelatedCharacters == null;
				if (flag2)
				{
					this.RelatedCharacters = new RelatedCharacters();
				}
				pCurrData += this.RelatedCharacters.Deserialize(pCurrData);
			}
			else
			{
				this.RelatedCharacters = null;
			}
			int totalSize = (int)((long)(pCurrData - pData));
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x04001672 RID: 5746
		[SerializableGameDataField]
		public RelatedCharacters RelatedCharacters;
	}
}
