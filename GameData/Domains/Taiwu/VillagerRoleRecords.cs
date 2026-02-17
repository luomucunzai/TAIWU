using System;
using System.Collections.Generic;
using GameData.Serializer;
using GameData.Utilities;
using SerializableGameDataSourceGenerator;

namespace GameData.Domains.Taiwu
{
	// Token: 0x02000044 RID: 68
	[SerializableGameDataSourceGenerator.AutoGenerateSerializableGameData(IsExtensible = true, NoCopyConstructors = true, NotForDisplayModule = true)]
	public class VillagerRoleRecords : ISerializableGameData
	{
		// Token: 0x06001364 RID: 4964 RVA: 0x00138368 File Offset: 0x00136568
		public bool IsSerializedSizeFixed()
		{
			return false;
		}

		// Token: 0x06001365 RID: 4965 RVA: 0x0013837C File Offset: 0x0013657C
		public int GetSerializedSize()
		{
			int totalSize = 2;
			bool flag = this.History != null;
			if (flag)
			{
				totalSize += 2;
				for (int i = 0; i < this.History.Count; i++)
				{
					bool flag2 = this.History[i] != null;
					if (flag2)
					{
						totalSize += 2 + this.History[i].GetSerializedSize();
					}
					else
					{
						totalSize += 2;
					}
				}
			}
			else
			{
				totalSize += 2;
			}
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x06001366 RID: 4966 RVA: 0x0013840C File Offset: 0x0013660C
		public unsafe int Serialize(byte* pData)
		{
			*(short*)pData = 1;
			byte* pCurrData = pData + 2;
			bool flag = this.History != null;
			if (flag)
			{
				int elementsCount = this.History.Count;
				Tester.Assert(elementsCount <= 65535, "");
				*(short*)pCurrData = (short)((ushort)elementsCount);
				pCurrData += 2;
				for (int i = 0; i < elementsCount; i++)
				{
					bool flag2 = this.History[i] != null;
					if (flag2)
					{
						byte* pSubDataCount = pCurrData;
						pCurrData += 2;
						int fieldSize = this.History[i].Serialize(pCurrData);
						pCurrData += fieldSize;
						Tester.Assert(fieldSize <= 65535, "");
						*(short*)pSubDataCount = (short)((ushort)fieldSize);
					}
					else
					{
						*(short*)pCurrData = 0;
						pCurrData += 2;
					}
				}
			}
			else
			{
				*(short*)pCurrData = 0;
				pCurrData += 2;
			}
			int totalSize = (int)((long)(pCurrData - pData));
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x06001367 RID: 4967 RVA: 0x00138508 File Offset: 0x00136708
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
					bool flag3 = this.History == null;
					if (flag3)
					{
						this.History = new List<VillagerRoleRecordElement>();
					}
					else
					{
						this.History.Clear();
					}
					for (int i = 0; i < (int)elementsCount; i++)
					{
						ushort classCount = *(ushort*)pCurrData;
						pCurrData += 2;
						bool flag4 = classCount > 0;
						VillagerRoleRecordElement element;
						if (flag4)
						{
							element = new VillagerRoleRecordElement();
							pCurrData += element.Deserialize(pCurrData);
						}
						else
						{
							element = null;
						}
						this.History.Add(element);
					}
				}
				else
				{
					List<VillagerRoleRecordElement> history = this.History;
					if (history != null)
					{
						history.Clear();
					}
				}
			}
			int totalSize = (int)((long)(pCurrData - pData));
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x0400030A RID: 778
		[SerializableGameDataField(FieldIndex = 0)]
		public List<VillagerRoleRecordElement> History = new List<VillagerRoleRecordElement>();

		// Token: 0x0200095A RID: 2394
		public static class FieldIds
		{
			// Token: 0x04002740 RID: 10048
			public const ushort History = 0;

			// Token: 0x04002741 RID: 10049
			public const ushort Count = 1;

			// Token: 0x04002742 RID: 10050
			public static readonly string[] FieldId2FieldName = new string[]
			{
				"History"
			};
		}
	}
}
