using System;
using System.Collections.Generic;
using GameData.Serializer;
using GameData.Utilities;
using SerializableGameDataSourceGenerator;

namespace GameData.Domains.Adventure
{
	// Token: 0x020008C5 RID: 2245
	[SerializableGameDataSourceGenerator.AutoGenerateSerializableGameData(IsExtensible = true)]
	public class AdventureRemake : ISerializableGameData
	{
		// Token: 0x06007F68 RID: 32616 RVA: 0x004C98D9 File Offset: 0x004C7AD9
		public AdventureRemake()
		{
		}

		// Token: 0x06007F69 RID: 32617 RVA: 0x004C98E4 File Offset: 0x004C7AE4
		public AdventureRemake(AdventureRemake other)
		{
			bool flag = other._elements != null;
			if (flag)
			{
				List<AdventureRemakeElement> item = other._elements;
				int elementsCount = item.Count;
				this._elements = new List<AdventureRemakeElement>(elementsCount);
				foreach (AdventureRemakeElement element in item)
				{
					this._elements.Add(new AdventureRemakeElement(element));
				}
			}
			else
			{
				this._elements = null;
			}
		}

		// Token: 0x06007F6A RID: 32618 RVA: 0x004C997C File Offset: 0x004C7B7C
		public void Assign(AdventureRemake other)
		{
			bool flag = other._elements != null;
			if (flag)
			{
				List<AdventureRemakeElement> item = other._elements;
				int elementsCount = item.Count;
				this._elements = new List<AdventureRemakeElement>(elementsCount);
				foreach (AdventureRemakeElement element in item)
				{
					this._elements.Add(new AdventureRemakeElement(element));
				}
			}
			else
			{
				this._elements = null;
			}
		}

		// Token: 0x06007F6B RID: 32619 RVA: 0x004C9A0C File Offset: 0x004C7C0C
		public bool IsSerializedSizeFixed()
		{
			return false;
		}

		// Token: 0x06007F6C RID: 32620 RVA: 0x004C9A20 File Offset: 0x004C7C20
		public int GetSerializedSize()
		{
			int totalSize = 2;
			bool flag = this._elements != null;
			if (flag)
			{
				totalSize += 2;
				for (int i = 0; i < this._elements.Count; i++)
				{
					bool flag2 = this._elements[i] != null;
					if (flag2)
					{
						totalSize += 2 + this._elements[i].GetSerializedSize();
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

		// Token: 0x06007F6D RID: 32621 RVA: 0x004C9AB0 File Offset: 0x004C7CB0
		public unsafe int Serialize(byte* pData)
		{
			*(short*)pData = 1;
			byte* pCurrData = pData + 2;
			bool flag = this._elements != null;
			if (flag)
			{
				int elementsCount = this._elements.Count;
				Tester.Assert(elementsCount <= 65535, "");
				*(short*)pCurrData = (short)((ushort)elementsCount);
				pCurrData += 2;
				for (int i = 0; i < elementsCount; i++)
				{
					bool flag2 = this._elements[i] != null;
					if (flag2)
					{
						byte* pSubDataCount = pCurrData;
						pCurrData += 2;
						int fieldSize = this._elements[i].Serialize(pCurrData);
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

		// Token: 0x06007F6E RID: 32622 RVA: 0x004C9BAC File Offset: 0x004C7DAC
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
					bool flag3 = this._elements == null;
					if (flag3)
					{
						this._elements = new List<AdventureRemakeElement>();
					}
					else
					{
						this._elements.Clear();
					}
					for (int i = 0; i < (int)elementsCount; i++)
					{
						ushort classCount = *(ushort*)pCurrData;
						pCurrData += 2;
						bool flag4 = classCount > 0;
						AdventureRemakeElement element;
						if (flag4)
						{
							element = new AdventureRemakeElement();
							pCurrData += element.Deserialize(pCurrData);
						}
						else
						{
							element = null;
						}
						this._elements.Add(element);
					}
				}
				else
				{
					List<AdventureRemakeElement> elements = this._elements;
					if (elements != null)
					{
						elements.Clear();
					}
				}
			}
			int totalSize = (int)((long)(pCurrData - pData));
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x040022EF RID: 8943
		[SerializableGameDataField(FieldIndex = 0)]
		private List<AdventureRemakeElement> _elements;

		// Token: 0x02000CBE RID: 3262
		public static class FieldIds
		{
			// Token: 0x04003717 RID: 14103
			public const ushort Elements = 0;

			// Token: 0x04003718 RID: 14104
			public const ushort Count = 1;

			// Token: 0x04003719 RID: 14105
			public static readonly string[] FieldId2FieldName = new string[]
			{
				"Elements"
			};
		}
	}
}
