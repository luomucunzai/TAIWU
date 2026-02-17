using System;
using System.Collections.Generic;
using GameData.DLC.FiveLoong;
using GameData.Serializer;

namespace GameData.Domains.Item
{
	// Token: 0x0200066D RID: 1645
	[SerializableGameData(NoCopyConstructors = true, NotForDisplayModule = true, IsExtensible = true)]
	public class ItemGroupPackage : ISerializableGameData
	{
		// Token: 0x06005145 RID: 20805 RVA: 0x002CA914 File Offset: 0x002C8B14
		public bool IsSerializedSizeFixed()
		{
			return false;
		}

		// Token: 0x06005146 RID: 20806 RVA: 0x002CA928 File Offset: 0x002C8B28
		public int GetSerializedSize()
		{
			int totalSize = 2;
			totalSize += ItemGroupPackage.ItemBaseDictSerializationHandler.GetSerializedSize(this.Items);
			totalSize += SerializationHelper.DictionaryOfBasicTypeCustomTypePair.GetSerializedSize<int, RefiningEffects>(this.RefiningEffects);
			totalSize += SerializationHelper.DictionaryOfBasicTypeCustomTypePair.GetSerializedSize<int, FullPoisonEffects>(this.FullPoisonEffects);
			totalSize += SerializationHelper.DictionaryOfBasicTypePair.GetSerializedSize<int, bool>(this.CricketIsSmart);
			totalSize += SerializationHelper.DictionaryOfBasicTypePair.GetSerializedSize<int, bool>(this.CricketIsIdentified);
			totalSize += SerializationHelper.DictionaryOfBasicTypePair.GetSerializedSize<int, int>(this.CarrierTamePoint);
			totalSize += SerializationHelper.DictionaryOfBasicTypeCustomTypePair.GetSerializedSize<int, Jiao>(this.Jiaos);
			totalSize += SerializationHelper.DictionaryOfBasicTypeCustomTypePair.GetSerializedSize<int, ChildrenOfLoong>(this.ChildrenOfLoong);
			totalSize += SerializationHelper.DictionaryOfCustomTypeBasicTypePair.GetSerializedSize<ItemKey, int>(this.JiaoKeyToId);
			totalSize += SerializationHelper.DictionaryOfCustomTypeBasicTypePair.GetSerializedSize<ItemKey, int>(this.ChildrenOfLoongKeyToId);
			totalSize += SerializationHelper.DictionaryOfBasicTypePair.GetSerializedSize<int, short>(this.ClothingDisplayModifications);
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x06005147 RID: 20807 RVA: 0x002CA9E8 File Offset: 0x002C8BE8
		public unsafe int Serialize(byte* pData)
		{
			*(short*)pData = 11;
			byte* pCurrData = pData + 2;
			pCurrData += ItemGroupPackage.ItemBaseDictSerializationHandler.Serialize(pCurrData, this.Items);
			pCurrData += SerializationHelper.DictionaryOfBasicTypeCustomTypePair.Serialize<int, RefiningEffects>(pCurrData, ref this.RefiningEffects);
			pCurrData += SerializationHelper.DictionaryOfBasicTypeCustomTypePair.Serialize<int, FullPoisonEffects>(pCurrData, ref this.FullPoisonEffects);
			pCurrData += SerializationHelper.DictionaryOfBasicTypePair.Serialize<int, bool>(pCurrData, ref this.CricketIsSmart);
			pCurrData += SerializationHelper.DictionaryOfBasicTypePair.Serialize<int, bool>(pCurrData, ref this.CricketIsIdentified);
			pCurrData += SerializationHelper.DictionaryOfBasicTypePair.Serialize<int, int>(pCurrData, ref this.CarrierTamePoint);
			pCurrData += SerializationHelper.DictionaryOfBasicTypeCustomTypePair.Serialize<int, Jiao>(pCurrData, ref this.Jiaos);
			pCurrData += SerializationHelper.DictionaryOfBasicTypeCustomTypePair.Serialize<int, ChildrenOfLoong>(pCurrData, ref this.ChildrenOfLoong);
			pCurrData += SerializationHelper.DictionaryOfCustomTypeBasicTypePair.Serialize<ItemKey, int>(pCurrData, ref this.JiaoKeyToId);
			pCurrData += SerializationHelper.DictionaryOfCustomTypeBasicTypePair.Serialize<ItemKey, int>(pCurrData, ref this.ChildrenOfLoongKeyToId);
			pCurrData += SerializationHelper.DictionaryOfBasicTypePair.Serialize<int, short>(pCurrData, ref this.ClothingDisplayModifications);
			int totalSize = (int)((long)(pCurrData - pData));
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x06005148 RID: 20808 RVA: 0x002CAAC4 File Offset: 0x002C8CC4
		public unsafe int Deserialize(byte* pData)
		{
			ushort fieldCount = *(ushort*)pData;
			byte* pCurrData = pData + 2;
			bool flag = fieldCount > 0;
			if (flag)
			{
				pCurrData += ItemGroupPackage.ItemBaseDictSerializationHandler.Deserialize(pCurrData, ref this.Items);
			}
			bool flag2 = fieldCount > 1;
			if (flag2)
			{
				pCurrData += SerializationHelper.DictionaryOfBasicTypeCustomTypePair.Deserialize<int, RefiningEffects>(pCurrData, ref this.RefiningEffects);
			}
			bool flag3 = fieldCount > 2;
			if (flag3)
			{
				pCurrData += SerializationHelper.DictionaryOfBasicTypeCustomTypePair.Deserialize<int, FullPoisonEffects>(pCurrData, ref this.FullPoisonEffects);
			}
			bool flag4 = fieldCount > 3;
			if (flag4)
			{
				pCurrData += SerializationHelper.DictionaryOfBasicTypePair.Deserialize<int, bool>(pCurrData, ref this.CricketIsSmart);
			}
			bool flag5 = fieldCount > 4;
			if (flag5)
			{
				pCurrData += SerializationHelper.DictionaryOfBasicTypePair.Deserialize<int, bool>(pCurrData, ref this.CricketIsIdentified);
			}
			bool flag6 = fieldCount > 5;
			if (flag6)
			{
				pCurrData += SerializationHelper.DictionaryOfBasicTypePair.Deserialize<int, int>(pCurrData, ref this.CarrierTamePoint);
			}
			bool flag7 = fieldCount > 6;
			if (flag7)
			{
				pCurrData += SerializationHelper.DictionaryOfBasicTypeCustomTypePair.Deserialize<int, Jiao>(pCurrData, ref this.Jiaos);
			}
			bool flag8 = fieldCount > 7;
			if (flag8)
			{
				pCurrData += SerializationHelper.DictionaryOfBasicTypeCustomTypePair.Deserialize<int, ChildrenOfLoong>(pCurrData, ref this.ChildrenOfLoong);
			}
			bool flag9 = fieldCount > 8;
			if (flag9)
			{
				pCurrData += SerializationHelper.DictionaryOfCustomTypeBasicTypePair.Deserialize<ItemKey, int>(pCurrData, ref this.JiaoKeyToId);
			}
			bool flag10 = fieldCount > 9;
			if (flag10)
			{
				pCurrData += SerializationHelper.DictionaryOfCustomTypeBasicTypePair.Deserialize<ItemKey, int>(pCurrData, ref this.ChildrenOfLoongKeyToId);
			}
			bool flag11 = fieldCount > 10;
			if (flag11)
			{
				pCurrData += SerializationHelper.DictionaryOfBasicTypePair.Deserialize<int, short>(pCurrData, ref this.ClothingDisplayModifications);
			}
			int totalSize = (int)((long)(pCurrData - pData));
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x040015FF RID: 5631
		[SerializableGameDataField(SerializationHandler = "ItemBaseDictSerializationHandler")]
		public Dictionary<int, ItemBase> Items = new Dictionary<int, ItemBase>();

		// Token: 0x04001600 RID: 5632
		[SerializableGameDataField]
		public Dictionary<int, RefiningEffects> RefiningEffects;

		// Token: 0x04001601 RID: 5633
		[SerializableGameDataField]
		public Dictionary<int, FullPoisonEffects> FullPoisonEffects;

		// Token: 0x04001602 RID: 5634
		[SerializableGameDataField]
		public Dictionary<int, bool> CricketIsSmart = new Dictionary<int, bool>();

		// Token: 0x04001603 RID: 5635
		[SerializableGameDataField]
		public Dictionary<int, bool> CricketIsIdentified = new Dictionary<int, bool>();

		// Token: 0x04001604 RID: 5636
		[SerializableGameDataField]
		public Dictionary<int, int> CarrierTamePoint = new Dictionary<int, int>();

		// Token: 0x04001605 RID: 5637
		[SerializableGameDataField]
		public Dictionary<int, Jiao> Jiaos = new Dictionary<int, Jiao>();

		// Token: 0x04001606 RID: 5638
		[SerializableGameDataField]
		public Dictionary<int, ChildrenOfLoong> ChildrenOfLoong = new Dictionary<int, ChildrenOfLoong>();

		// Token: 0x04001607 RID: 5639
		[SerializableGameDataField]
		public Dictionary<ItemKey, int> JiaoKeyToId = new Dictionary<ItemKey, int>();

		// Token: 0x04001608 RID: 5640
		[SerializableGameDataField]
		public Dictionary<ItemKey, int> ChildrenOfLoongKeyToId = new Dictionary<ItemKey, int>();

		// Token: 0x04001609 RID: 5641
		[SerializableGameDataField]
		public Dictionary<int, short> ClothingDisplayModifications;

		// Token: 0x02000ABF RID: 2751
		private static class ItemBaseDictSerializationHandler
		{
			// Token: 0x06008928 RID: 35112 RVA: 0x004EE4F8 File Offset: 0x004EC6F8
			public static int GetSerializedSize(Dictionary<int, ItemBase> target)
			{
				bool flag = target == null;
				int result;
				if (flag)
				{
					result = 4;
				}
				else
				{
					int size = 4;
					foreach (KeyValuePair<int, ItemBase> pair in target)
					{
						size += 4;
						size++;
						size += ((ISerializableGameData)pair.Value).GetSerializedSize();
					}
					result = size;
				}
				return result;
			}

			// Token: 0x06008929 RID: 35113 RVA: 0x004EE574 File Offset: 0x004EC774
			public unsafe static int Serialize(byte* pData, Dictionary<int, ItemBase> target)
			{
				byte* pCurrData = pData;
				bool flag = target != null;
				if (flag)
				{
					*(int*)pCurrData = target.Count;
					pCurrData += 4;
					foreach (KeyValuePair<int, ItemBase> pair in target)
					{
						*(int*)pCurrData = pair.Key;
						pCurrData += 4;
						byte* ptr = pCurrData;
						*ptr = (byte)(*(sbyte*)ptr + pair.Value.GetItemType());
						pCurrData++;
						pCurrData += ((ISerializableGameData)pair.Value).Serialize(pCurrData);
					}
				}
				else
				{
					*(int*)pCurrData = 0;
					pCurrData += 4;
				}
				return (int)((long)(pCurrData - pData));
			}

			// Token: 0x0600892A RID: 35114 RVA: 0x004EE628 File Offset: 0x004EC828
			public unsafe static int Deserialize(byte* pData, ref Dictionary<int, ItemBase> target)
			{
				int count = *(int*)pData;
				byte* pCurrData = pData + 4;
				bool flag = count > 0;
				if (flag)
				{
					bool flag2 = target == null;
					if (flag2)
					{
						target = new Dictionary<int, ItemBase>();
					}
					else
					{
						target.Clear();
					}
					for (int i = 0; i < count; i++)
					{
						int key = *(int*)pCurrData;
						pCurrData += 4;
						sbyte itemType = *(sbyte*)pCurrData;
						pCurrData++;
						if (!true)
						{
						}
						ItemBase itemBase;
						switch (itemType)
						{
						case 0:
							itemBase = new Weapon();
							break;
						case 1:
							itemBase = new Armor();
							break;
						case 2:
							itemBase = new Accessory();
							break;
						case 3:
							itemBase = new Clothing();
							break;
						case 4:
							itemBase = new Carrier();
							break;
						case 5:
							itemBase = new Material();
							break;
						case 6:
							itemBase = new CraftTool();
							break;
						case 7:
							itemBase = new Food();
							break;
						case 8:
							itemBase = new Medicine();
							break;
						case 9:
							itemBase = new TeaWine();
							break;
						case 10:
							itemBase = new SkillBook();
							break;
						case 11:
							itemBase = new Cricket();
							break;
						case 12:
							itemBase = new Misc();
							break;
						default:
							throw ItemTemplateHelper.CreateItemTypeException(itemType);
						}
						if (!true)
						{
						}
						ItemBase value = itemBase;
						pCurrData += ((ISerializableGameData)value).Deserialize(pCurrData);
						target.Add(key, value);
					}
				}
				else
				{
					Dictionary<int, ItemBase> dictionary = target;
					if (dictionary != null)
					{
						dictionary.Clear();
					}
				}
				return (int)((long)(pCurrData - pData));
			}
		}

		// Token: 0x02000AC0 RID: 2752
		private static class FieldIds
		{
			// Token: 0x04002C81 RID: 11393
			public const ushort Items = 0;

			// Token: 0x04002C82 RID: 11394
			public const ushort RefiningEffects = 1;

			// Token: 0x04002C83 RID: 11395
			public const ushort FullPoisonEffects = 2;

			// Token: 0x04002C84 RID: 11396
			public const ushort CricketIsSmart = 3;

			// Token: 0x04002C85 RID: 11397
			public const ushort CricketIsIdentified = 4;

			// Token: 0x04002C86 RID: 11398
			public const ushort CarrierTamePoint = 5;

			// Token: 0x04002C87 RID: 11399
			public const ushort Jiaos = 6;

			// Token: 0x04002C88 RID: 11400
			public const ushort ChildrenOfLoong = 7;

			// Token: 0x04002C89 RID: 11401
			public const ushort JiaoKeyToId = 8;

			// Token: 0x04002C8A RID: 11402
			public const ushort ChildrenOfLoongKeyToId = 9;

			// Token: 0x04002C8B RID: 11403
			public const ushort ClothingDisplayModifications = 10;

			// Token: 0x04002C8C RID: 11404
			public const ushort Count = 11;

			// Token: 0x04002C8D RID: 11405
			public static readonly string[] FieldId2FieldName = new string[]
			{
				"Items",
				"RefiningEffects",
				"FullPoisonEffects",
				"CricketIsSmart",
				"CricketIsIdentified",
				"CarrierTamePoint",
				"Jiaos",
				"ChildrenOfLoong",
				"JiaoKeyToId",
				"ChildrenOfLoongKeyToId",
				"ClothingDisplayModifications"
			};
		}
	}
}
