using System;
using System.Collections.Generic;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Item
{
	// Token: 0x02000665 RID: 1637
	[SerializableGameData(IsExtensible = true, NotForDisplayModule = true)]
	public class CricketCombatPlan : ISerializableGameData
	{
		// Token: 0x06004F82 RID: 20354 RVA: 0x002B4D1A File Offset: 0x002B2F1A
		public CricketCombatPlan()
		{
		}

		// Token: 0x06004F83 RID: 20355 RVA: 0x002B4D24 File Offset: 0x002B2F24
		public CricketCombatPlan(CricketCombatPlan other)
		{
			this.Crickets = ((other.Crickets == null) ? null : new List<ItemKey>(other.Crickets));
		}

		// Token: 0x06004F84 RID: 20356 RVA: 0x002B4D4A File Offset: 0x002B2F4A
		public void Assign(CricketCombatPlan other)
		{
			this.Crickets = ((other.Crickets == null) ? null : new List<ItemKey>(other.Crickets));
		}

		// Token: 0x06004F85 RID: 20357 RVA: 0x002B4D6C File Offset: 0x002B2F6C
		public bool IsSerializedSizeFixed()
		{
			return false;
		}

		// Token: 0x06004F86 RID: 20358 RVA: 0x002B4D80 File Offset: 0x002B2F80
		public int GetSerializedSize()
		{
			int totalSize = 2;
			bool flag = this.Crickets != null;
			if (flag)
			{
				totalSize += 2 + 8 * this.Crickets.Count;
			}
			else
			{
				totalSize += 2;
			}
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x06004F87 RID: 20359 RVA: 0x002B4DCC File Offset: 0x002B2FCC
		public unsafe int Serialize(byte* pData)
		{
			*(short*)pData = 1;
			byte* pCurrData = pData + 2;
			bool flag = this.Crickets != null;
			if (flag)
			{
				int elementsCount = this.Crickets.Count;
				Tester.Assert(elementsCount <= 65535, "");
				*(short*)pCurrData = (short)((ushort)elementsCount);
				pCurrData += 2;
				for (int i = 0; i < elementsCount; i++)
				{
					pCurrData += this.Crickets[i].Serialize(pCurrData);
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

		// Token: 0x06004F88 RID: 20360 RVA: 0x002B4E7C File Offset: 0x002B307C
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
					bool flag3 = this.Crickets == null;
					if (flag3)
					{
						this.Crickets = new List<ItemKey>((int)elementsCount);
					}
					else
					{
						this.Crickets.Clear();
					}
					for (int i = 0; i < (int)elementsCount; i++)
					{
						ItemKey element = default(ItemKey);
						pCurrData += element.Deserialize(pCurrData);
						this.Crickets.Add(element);
					}
				}
				else
				{
					List<ItemKey> crickets = this.Crickets;
					if (crickets != null)
					{
						crickets.Clear();
					}
				}
			}
			int totalSize = (int)((long)(pCurrData - pData));
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x040015AA RID: 5546
		[SerializableGameDataField]
		public List<ItemKey> Crickets;

		// Token: 0x02000AAB RID: 2731
		private static class FieldIds
		{
			// Token: 0x04002C0A RID: 11274
			public const ushort Crickets = 0;

			// Token: 0x04002C0B RID: 11275
			public const ushort Count = 1;

			// Token: 0x04002C0C RID: 11276
			public static readonly string[] FieldId2FieldName = new string[]
			{
				"Crickets"
			};
		}
	}
}
