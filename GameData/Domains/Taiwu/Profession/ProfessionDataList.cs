using System;
using System.Collections.Generic;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Taiwu.Profession
{
	// Token: 0x02000055 RID: 85
	[SerializableGameData(IsExtensible = true, NoCopyConstructors = true, NotForDisplayModule = true)]
	public class ProfessionDataList : ISerializableGameData
	{
		// Token: 0x0600144A RID: 5194 RVA: 0x0013E58C File Offset: 0x0013C78C
		public ProfessionDataList()
		{
			this._items = new List<ProfessionData>();
			this._lastTeachTaiwuDate = int.MinValue;
		}

		// Token: 0x17000077 RID: 119
		// (get) Token: 0x0600144B RID: 5195 RVA: 0x0013E5AC File Offset: 0x0013C7AC
		public ProfessionData CurrProfession
		{
			get
			{
				for (int index = this._items.Count - 1; index >= 0; index--)
				{
					ProfessionData profession = this._items[index];
					bool flag = profession.TemplateId == this.CurrProfessionId;
					if (flag)
					{
						return profession;
					}
				}
				return null;
			}
		}

		// Token: 0x0600144C RID: 5196 RVA: 0x0013E608 File Offset: 0x0013C808
		[Obsolete]
		public int GetTeachTaiwuCount(int professionId)
		{
			Dictionary<int, int> taiwuDemandTeachingCount = this._taiwuDemandTeachingCount;
			return (taiwuDemandTeachingCount != null) ? taiwuDemandTeachingCount.GetValueOrDefault(professionId, 0) : 0;
		}

		// Token: 0x0600144D RID: 5197 RVA: 0x0013E630 File Offset: 0x0013C830
		[Obsolete]
		public void AddTeachTaiwuCount(int professionId, int count)
		{
			if (this._taiwuDemandTeachingCount == null)
			{
				this._taiwuDemandTeachingCount = new Dictionary<int, int>();
			}
			int prevCount = this._taiwuDemandTeachingCount.GetValueOrDefault(professionId, 0);
			this._taiwuDemandTeachingCount[professionId] = prevCount + count;
		}

		// Token: 0x0600144E RID: 5198 RVA: 0x0013E670 File Offset: 0x0013C870
		public int GetTeachTaiwuDate()
		{
			return this._lastTeachTaiwuDate;
		}

		// Token: 0x0600144F RID: 5199 RVA: 0x0013E688 File Offset: 0x0013C888
		public void SetTeachTaiwuDate(int date)
		{
			this._lastTeachTaiwuDate = date;
		}

		// Token: 0x06001450 RID: 5200 RVA: 0x0013E694 File Offset: 0x0013C894
		public ProfessionData GetProfession(int professionId)
		{
			for (int i = this._items.Count - 1; i >= 0; i--)
			{
				ProfessionData profession = this._items[i];
				bool flag = profession.TemplateId == professionId;
				if (flag)
				{
					return profession;
				}
			}
			return null;
		}

		// Token: 0x06001451 RID: 5201 RVA: 0x0013E6E8 File Offset: 0x0013C8E8
		public ProfessionData ChangeCurrProfession(int professionId)
		{
			this.CurrProfessionId = professionId;
			ProfessionData professionData = this.GetProfession(professionId);
			bool flag = professionData != null;
			ProfessionData result;
			if (flag)
			{
				result = professionData;
			}
			else
			{
				professionData = new ProfessionData(professionId, 1);
				this._items.Add(professionData);
				result = professionData;
			}
			return result;
		}

		// Token: 0x06001452 RID: 5202 RVA: 0x0013E72C File Offset: 0x0013C92C
		public bool IsSerializedSizeFixed()
		{
			return false;
		}

		// Token: 0x06001453 RID: 5203 RVA: 0x0013E740 File Offset: 0x0013C940
		public int GetSerializedSize()
		{
			int totalSize = 10;
			bool flag = this._items != null;
			if (flag)
			{
				totalSize += 2;
				int elementsCount = this._items.Count;
				for (int i = 0; i < elementsCount; i++)
				{
					ProfessionData element = this._items[i];
					bool flag2 = element != null;
					if (flag2)
					{
						totalSize += 4 + element.GetSerializedSize();
					}
					else
					{
						totalSize += 4;
					}
				}
			}
			else
			{
				totalSize += 2;
			}
			totalSize += SerializationHelper.DictionaryOfBasicTypePair.GetSerializedSize<int, int>(this._taiwuDemandTeachingCount);
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x06001454 RID: 5204 RVA: 0x0013E7E0 File Offset: 0x0013C9E0
		public unsafe int Serialize(byte* pData)
		{
			*(short*)pData = 4;
			byte* pCurrData = pData + 2;
			*(int*)pCurrData = this.CurrProfessionId;
			pCurrData += 4;
			bool flag = this._items != null;
			if (flag)
			{
				int elementsCount = this._items.Count;
				Tester.Assert(elementsCount <= 65535, "");
				*(short*)pCurrData = (short)((ushort)elementsCount);
				pCurrData += 2;
				for (int i = 0; i < elementsCount; i++)
				{
					ProfessionData element = this._items[i];
					bool flag2 = element != null;
					if (flag2)
					{
						byte* pSubDataCount = pCurrData;
						pCurrData += 4;
						int subDataSize = element.Serialize(pCurrData);
						pCurrData += subDataSize;
						Tester.Assert(subDataSize <= int.MaxValue, "");
						*(int*)pSubDataCount = subDataSize;
					}
					else
					{
						*(int*)pCurrData = 0;
						pCurrData += 4;
					}
				}
			}
			else
			{
				*(short*)pCurrData = 0;
				pCurrData += 2;
			}
			pCurrData += SerializationHelper.DictionaryOfBasicTypePair.Serialize<int, int>(pCurrData, ref this._taiwuDemandTeachingCount);
			*(int*)pCurrData = this._lastTeachTaiwuDate;
			pCurrData += 4;
			int totalSize = (int)((long)(pCurrData - pData));
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x06001455 RID: 5205 RVA: 0x0013E8F8 File Offset: 0x0013CAF8
		public unsafe int Deserialize(byte* pData)
		{
			ushort fieldCount = *(ushort*)pData;
			byte* pCurrData = pData + 2;
			bool flag = fieldCount > 0;
			if (flag)
			{
				this.CurrProfessionId = *(int*)pCurrData;
				pCurrData += 4;
			}
			bool flag2 = fieldCount > 1;
			if (flag2)
			{
				ushort elementsCount = *(ushort*)pCurrData;
				pCurrData += 2;
				bool flag3 = elementsCount > 0;
				if (flag3)
				{
					bool flag4 = this._items == null;
					if (flag4)
					{
						this._items = new List<ProfessionData>((int)elementsCount);
					}
					else
					{
						this._items.Clear();
					}
					for (int i = 0; i < (int)elementsCount; i++)
					{
						int subDataCount = *(int*)pCurrData;
						pCurrData += 4;
						bool flag5 = subDataCount > 0;
						if (flag5)
						{
							ProfessionData element = new ProfessionData();
							pCurrData += element.Deserialize(pCurrData);
							this._items.Add(element);
						}
						else
						{
							this._items.Add(null);
						}
					}
				}
				else
				{
					List<ProfessionData> items = this._items;
					if (items != null)
					{
						items.Clear();
					}
				}
			}
			bool flag6 = fieldCount > 2;
			if (flag6)
			{
				pCurrData += SerializationHelper.DictionaryOfBasicTypePair.Deserialize<int, int>(pCurrData, ref this._taiwuDemandTeachingCount);
			}
			bool flag7 = fieldCount > 3;
			if (flag7)
			{
				this._lastTeachTaiwuDate = *(int*)pCurrData;
				pCurrData += 4;
			}
			int totalSize = (int)((long)(pCurrData - pData));
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x04000329 RID: 809
		[SerializableGameDataField]
		public int CurrProfessionId;

		// Token: 0x0400032A RID: 810
		[SerializableGameDataField(SubDataMaxCount = 2147483647)]
		private List<ProfessionData> _items;

		// Token: 0x0400032B RID: 811
		[SerializableGameDataField]
		[Obsolete]
		private Dictionary<int, int> _taiwuDemandTeachingCount;

		// Token: 0x0400032C RID: 812
		[SerializableGameDataField]
		private int _lastTeachTaiwuDate;

		// Token: 0x0200096D RID: 2413
		private static class FieldIds
		{
			// Token: 0x04002788 RID: 10120
			public const ushort CurrProfessionId = 0;

			// Token: 0x04002789 RID: 10121
			public const ushort Items = 1;

			// Token: 0x0400278A RID: 10122
			public const ushort TaiwuDemandTeachingCount = 2;

			// Token: 0x0400278B RID: 10123
			public const ushort LastTeachTaiwuDate = 3;

			// Token: 0x0400278C RID: 10124
			public const ushort Count = 4;

			// Token: 0x0400278D RID: 10125
			public static readonly string[] FieldId2FieldName = new string[]
			{
				"CurrProfessionId",
				"Items",
				"TaiwuDemandTeachingCount",
				"LastTeachTaiwuDate"
			};
		}
	}
}
