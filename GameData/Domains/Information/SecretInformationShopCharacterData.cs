using System;
using System.Collections.Generic;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Information
{
	// Token: 0x02000685 RID: 1669
	[SerializableGameData(IsExtensible = true, NotForDisplayModule = true)]
	public class SecretInformationShopCharacterData : ISerializableGameData
	{
		// Token: 0x060054D2 RID: 21714 RVA: 0x002E79EC File Offset: 0x002E5BEC
		public bool IsSerializedSizeFixed()
		{
			return false;
		}

		// Token: 0x060054D3 RID: 21715 RVA: 0x002E7A00 File Offset: 0x002E5C00
		public int GetSerializedSize()
		{
			int totalSize = 2;
			bool flag = this.CollectedSecretInformationIds != null;
			if (flag)
			{
				totalSize += 2 + 4 * this.CollectedSecretInformationIds.Count;
			}
			else
			{
				totalSize += 2;
			}
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x060054D4 RID: 21716 RVA: 0x002E7A4C File Offset: 0x002E5C4C
		public unsafe int Serialize(byte* pData)
		{
			*(short*)pData = 1;
			byte* pCurrData = pData + 2;
			bool flag = this.CollectedSecretInformationIds != null;
			if (flag)
			{
				int elementsCount = this.CollectedSecretInformationIds.Count;
				Tester.Assert(elementsCount <= 65535, "");
				*(short*)pCurrData = (short)((ushort)elementsCount);
				pCurrData += 2;
				for (int i = 0; i < elementsCount; i++)
				{
					*(int*)(pCurrData + (IntPtr)i * 4) = this.CollectedSecretInformationIds[i];
				}
				pCurrData += 4 * elementsCount;
			}
			else
			{
				*(short*)pCurrData = 0;
				pCurrData += 2;
			}
			int totalSize = (int)((long)(pCurrData - pData));
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x060054D5 RID: 21717 RVA: 0x002E7AFC File Offset: 0x002E5CFC
		public unsafe int Deserialize(byte* pData)
		{
			ushort fieldCount = *(ushort*)pData;
			byte* pCurrData = pData + 2;
			bool flag = fieldCount > 0;
			if (flag)
			{
				ushort elementsCount = *(ushort*)pCurrData;
				pCurrData += 2;
				bool flag2 = elementsCount > 0;
				if (flag2)
				{
					bool flag3 = this.CollectedSecretInformationIds == null;
					if (flag3)
					{
						this.CollectedSecretInformationIds = new List<int>((int)elementsCount);
					}
					else
					{
						this.CollectedSecretInformationIds.Clear();
					}
					for (int i = 0; i < (int)elementsCount; i++)
					{
						this.CollectedSecretInformationIds.Add(*(int*)(pCurrData + (IntPtr)i * 4));
					}
					pCurrData += 4 * elementsCount;
				}
				else
				{
					List<int> collectedSecretInformationIds = this.CollectedSecretInformationIds;
					if (collectedSecretInformationIds != null)
					{
						collectedSecretInformationIds.Clear();
					}
				}
			}
			int totalSize = (int)((long)(pCurrData - pData));
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x040016A5 RID: 5797
		[SerializableGameDataField]
		public List<int> CollectedSecretInformationIds = new List<int>();

		// Token: 0x02000AFD RID: 2813
		private static class FieldIds
		{
			// Token: 0x04002D81 RID: 11649
			public const ushort CollectedSecretInformationIds = 0;

			// Token: 0x04002D82 RID: 11650
			public const ushort Count = 1;

			// Token: 0x04002D83 RID: 11651
			public static readonly string[] FieldId2FieldName = new string[]
			{
				"CollectedSecretInformationIds"
			};
		}
	}
}
