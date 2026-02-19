using System.Collections.Generic;
using GameData.DLC.FiveLoong;
using GameData.Serializer;

namespace GameData.Domains.Item;

[SerializableGameData(NoCopyConstructors = true, NotForDisplayModule = true, IsExtensible = true)]
public class ItemGroupPackage : ISerializableGameData
{
	private static class ItemBaseDictSerializationHandler
	{
		public static int GetSerializedSize(Dictionary<int, ItemBase> target)
		{
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			if (target == null)
			{
				return 4;
			}
			int num = 4;
			foreach (KeyValuePair<int, ItemBase> item in target)
			{
				num += 4;
				num++;
				num += ((ISerializableGameData)item.Value).GetSerializedSize();
			}
			return num;
		}

		public unsafe static int Serialize(byte* pData, Dictionary<int, ItemBase> target)
		{
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			byte* ptr = pData;
			if (target != null)
			{
				*(int*)ptr = target.Count;
				ptr += 4;
				foreach (KeyValuePair<int, ItemBase> item in target)
				{
					*(int*)ptr = item.Key;
					ptr += 4;
					*ptr += (byte)item.Value.GetItemType();
					ptr++;
					ptr += ((ISerializableGameData)item.Value).Serialize(ptr);
				}
			}
			else
			{
				*(int*)ptr = 0;
				ptr += 4;
			}
			return (int)(ptr - pData);
		}

		public unsafe static int Deserialize(byte* pData, ref Dictionary<int, ItemBase> target)
		{
			//IL_0112: Unknown result type (might be due to invalid IL or missing references)
			byte* ptr = pData;
			int num = *(int*)ptr;
			ptr += 4;
			if (num > 0)
			{
				if (target == null)
				{
					target = new Dictionary<int, ItemBase>();
				}
				else
				{
					target.Clear();
				}
				for (int i = 0; i < num; i++)
				{
					int key = *(int*)ptr;
					ptr += 4;
					sbyte b = (sbyte)(*ptr);
					ptr++;
					if (1 == 0)
					{
					}
					ItemBase itemBase = b switch
					{
						0 => new Weapon(), 
						1 => new Armor(), 
						2 => new Accessory(), 
						3 => new Clothing(), 
						4 => new Carrier(), 
						5 => new Material(), 
						6 => new CraftTool(), 
						7 => new Food(), 
						8 => new Medicine(), 
						9 => new TeaWine(), 
						10 => new SkillBook(), 
						11 => new Cricket(), 
						12 => new Misc(), 
						_ => throw ItemTemplateHelper.CreateItemTypeException(b), 
					};
					if (1 == 0)
					{
					}
					ItemBase itemBase2 = itemBase;
					ptr += ((ISerializableGameData)itemBase2).Deserialize(ptr);
					target.Add(key, itemBase2);
				}
			}
			else
			{
				target?.Clear();
			}
			return (int)(ptr - pData);
		}
	}

	private static class FieldIds
	{
		public const ushort Items = 0;

		public const ushort RefiningEffects = 1;

		public const ushort FullPoisonEffects = 2;

		public const ushort CricketIsSmart = 3;

		public const ushort CricketIsIdentified = 4;

		public const ushort CarrierTamePoint = 5;

		public const ushort Jiaos = 6;

		public const ushort ChildrenOfLoong = 7;

		public const ushort JiaoKeyToId = 8;

		public const ushort ChildrenOfLoongKeyToId = 9;

		public const ushort ClothingDisplayModifications = 10;

		public const ushort Count = 11;

		public static readonly string[] FieldId2FieldName = new string[11]
		{
			"Items", "RefiningEffects", "FullPoisonEffects", "CricketIsSmart", "CricketIsIdentified", "CarrierTamePoint", "Jiaos", "ChildrenOfLoong", "JiaoKeyToId", "ChildrenOfLoongKeyToId",
			"ClothingDisplayModifications"
		};
	}

	[SerializableGameDataField(SerializationHandler = "ItemBaseDictSerializationHandler")]
	public Dictionary<int, ItemBase> Items = new Dictionary<int, ItemBase>();

	[SerializableGameDataField]
	public Dictionary<int, RefiningEffects> RefiningEffects;

	[SerializableGameDataField]
	public Dictionary<int, FullPoisonEffects> FullPoisonEffects;

	[SerializableGameDataField]
	public Dictionary<int, bool> CricketIsSmart = new Dictionary<int, bool>();

	[SerializableGameDataField]
	public Dictionary<int, bool> CricketIsIdentified = new Dictionary<int, bool>();

	[SerializableGameDataField]
	public Dictionary<int, int> CarrierTamePoint = new Dictionary<int, int>();

	[SerializableGameDataField]
	public Dictionary<int, Jiao> Jiaos = new Dictionary<int, Jiao>();

	[SerializableGameDataField]
	public Dictionary<int, ChildrenOfLoong> ChildrenOfLoong = new Dictionary<int, ChildrenOfLoong>();

	[SerializableGameDataField]
	public Dictionary<ItemKey, int> JiaoKeyToId = new Dictionary<ItemKey, int>();

	[SerializableGameDataField]
	public Dictionary<ItemKey, int> ChildrenOfLoongKeyToId = new Dictionary<ItemKey, int>();

	[SerializableGameDataField]
	public Dictionary<int, short> ClothingDisplayModifications;

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 2;
		num += ItemBaseDictSerializationHandler.GetSerializedSize(Items);
		num += DictionaryOfBasicTypeCustomTypePair.GetSerializedSize<int, RefiningEffects>(RefiningEffects);
		num += DictionaryOfBasicTypeCustomTypePair.GetSerializedSize<int, FullPoisonEffects>(FullPoisonEffects);
		num += DictionaryOfBasicTypePair.GetSerializedSize<int, bool>((IReadOnlyDictionary<int, bool>)CricketIsSmart);
		num += DictionaryOfBasicTypePair.GetSerializedSize<int, bool>((IReadOnlyDictionary<int, bool>)CricketIsIdentified);
		num += DictionaryOfBasicTypePair.GetSerializedSize<int, int>((IReadOnlyDictionary<int, int>)CarrierTamePoint);
		num += DictionaryOfBasicTypeCustomTypePair.GetSerializedSize<int, Jiao>(Jiaos);
		num += DictionaryOfBasicTypeCustomTypePair.GetSerializedSize<int, ChildrenOfLoong>(ChildrenOfLoong);
		num += DictionaryOfCustomTypeBasicTypePair.GetSerializedSize<ItemKey, int>(JiaoKeyToId);
		num += DictionaryOfCustomTypeBasicTypePair.GetSerializedSize<ItemKey, int>(ChildrenOfLoongKeyToId);
		num += DictionaryOfBasicTypePair.GetSerializedSize<int, short>((IReadOnlyDictionary<int, short>)ClothingDisplayModifications);
		return (num <= 4) ? num : ((num + 3) / 4 * 4);
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		*(short*)ptr = 11;
		ptr += 2;
		ptr += ItemBaseDictSerializationHandler.Serialize(ptr, Items);
		ptr += DictionaryOfBasicTypeCustomTypePair.Serialize<int, RefiningEffects>(ptr, ref RefiningEffects);
		ptr += DictionaryOfBasicTypeCustomTypePair.Serialize<int, FullPoisonEffects>(ptr, ref FullPoisonEffects);
		ptr += DictionaryOfBasicTypePair.Serialize<int, bool>(ptr, ref CricketIsSmart);
		ptr += DictionaryOfBasicTypePair.Serialize<int, bool>(ptr, ref CricketIsIdentified);
		ptr += DictionaryOfBasicTypePair.Serialize<int, int>(ptr, ref CarrierTamePoint);
		ptr += DictionaryOfBasicTypeCustomTypePair.Serialize<int, Jiao>(ptr, ref Jiaos);
		ptr += DictionaryOfBasicTypeCustomTypePair.Serialize<int, ChildrenOfLoong>(ptr, ref ChildrenOfLoong);
		ptr += DictionaryOfCustomTypeBasicTypePair.Serialize<ItemKey, int>(ptr, ref JiaoKeyToId);
		ptr += DictionaryOfCustomTypeBasicTypePair.Serialize<ItemKey, int>(ptr, ref ChildrenOfLoongKeyToId);
		ptr += DictionaryOfBasicTypePair.Serialize<int, short>(ptr, ref ClothingDisplayModifications);
		int num = (int)(ptr - pData);
		return (num <= 4) ? num : ((num + 3) / 4 * 4);
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		ushort num = *(ushort*)ptr;
		ptr += 2;
		if (num > 0)
		{
			ptr += ItemBaseDictSerializationHandler.Deserialize(ptr, ref Items);
		}
		if (num > 1)
		{
			ptr += DictionaryOfBasicTypeCustomTypePair.Deserialize<int, RefiningEffects>(ptr, ref RefiningEffects);
		}
		if (num > 2)
		{
			ptr += DictionaryOfBasicTypeCustomTypePair.Deserialize<int, FullPoisonEffects>(ptr, ref FullPoisonEffects);
		}
		if (num > 3)
		{
			ptr += DictionaryOfBasicTypePair.Deserialize<int, bool>(ptr, ref CricketIsSmart);
		}
		if (num > 4)
		{
			ptr += DictionaryOfBasicTypePair.Deserialize<int, bool>(ptr, ref CricketIsIdentified);
		}
		if (num > 5)
		{
			ptr += DictionaryOfBasicTypePair.Deserialize<int, int>(ptr, ref CarrierTamePoint);
		}
		if (num > 6)
		{
			ptr += DictionaryOfBasicTypeCustomTypePair.Deserialize<int, Jiao>(ptr, ref Jiaos);
		}
		if (num > 7)
		{
			ptr += DictionaryOfBasicTypeCustomTypePair.Deserialize<int, ChildrenOfLoong>(ptr, ref ChildrenOfLoong);
		}
		if (num > 8)
		{
			ptr += DictionaryOfCustomTypeBasicTypePair.Deserialize<ItemKey, int>(ptr, ref JiaoKeyToId);
		}
		if (num > 9)
		{
			ptr += DictionaryOfCustomTypeBasicTypePair.Deserialize<ItemKey, int>(ptr, ref ChildrenOfLoongKeyToId);
		}
		if (num > 10)
		{
			ptr += DictionaryOfBasicTypePair.Deserialize<int, short>(ptr, ref ClothingDisplayModifications);
		}
		int num2 = (int)(ptr - pData);
		return (num2 <= 4) ? num2 : ((num2 + 3) / 4 * 4);
	}
}
