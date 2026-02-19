using System.Collections.Generic;
using Config;
using GameData.Domains.LifeRecord.GeneralRecord;
using GameData.Domains.Map;
using GameData.Domains.Taiwu;
using GameData.Utilities;

namespace GameData.Domains.Organization.TaiwuVillageStoragesRecord;

public class TaiwuVillageStoragesRecordCollection : WriteableRecordCollection
{
	public void GetRenderInfos(List<TaiwuVillageStoragesRecordRenderInfo> renderInfos, ArgumentCollection argumentCollection)
	{
		int index = -1;
		int offset = -1;
		while (Next(ref index, ref offset))
		{
			TaiwuVillageStoragesRecordRenderInfo renderInfo = GetRenderInfo(offset, argumentCollection);
			if (renderInfo != null)
			{
				renderInfos.Add(renderInfo);
			}
		}
	}

	public unsafe short GetRecordType(int offset)
	{
		fixed (byte* rawData = RawData)
		{
			return ((short*)(rawData + offset + 1))[2];
		}
	}

	private unsafe int GetDate(int offset)
	{
		fixed (byte* rawData = RawData)
		{
			return *(int*)(rawData + offset + 1);
		}
	}

	public new unsafe TaiwuVillageStoragesRecordRenderInfo GetRenderInfo(int offset, ArgumentCollection argumentCollection)
	{
		fixed (byte* rawData = RawData)
		{
			byte* ptr = rawData + offset;
			ptr++;
			int date = *(int*)ptr;
			ptr += 4;
			sbyte storageType = (sbyte)(*ptr);
			ptr++;
			short num = *(short*)ptr;
			ptr += 2;
			TaiwuVillageStoragesRecordItem taiwuVillageStoragesRecordItem = Config.TaiwuVillageStoragesRecord.Instance[num];
			if (taiwuVillageStoragesRecordItem == null)
			{
				AdaptableLog.Warning($"Unable to render monthly notification with template id {num}");
				return null;
			}
			string[] parameters = taiwuVillageStoragesRecordItem.Parameters;
			TaiwuVillageStoragesRecordRenderInfo taiwuVillageStoragesRecordRenderInfo = new TaiwuVillageStoragesRecordRenderInfo(num, taiwuVillageStoragesRecordItem.Desc, date, storageType);
			int i = 0;
			for (int num2 = parameters.Length; i < num2; i++)
			{
				string text = parameters[i];
				if (string.IsNullOrEmpty(text))
				{
					break;
				}
				sbyte b = ParameterType.Parse(text);
				int item = ReadonlyRecordCollection.ReadArgumentAndGetIndex(b, &ptr, argumentCollection);
				taiwuVillageStoragesRecordRenderInfo.Arguments.Add((b, item));
			}
			return taiwuVillageStoragesRecordRenderInfo;
		}
	}

	private unsafe int BeginAddingRecord(int date, TaiwuVillageStorageType storageType, short recordType)
	{
		int size = Size;
		sbyte b = (sbyte)storageType;
		int num = Size + 1 + 4 + 1 + 2;
		EnsureCapacity(num);
		Size = num;
		fixed (byte* rawData = RawData)
		{
			byte* num2 = rawData + size;
			*(int*)(num2 + 1) = date;
			(num2 + 1)[4] = (byte)b;
			*(short*)(num2 + 1 + 4 + 1) = recordType;
		}
		return size;
	}

	public int AddTakeItem(int date, TaiwuVillageStorageType storageType, int charId, sbyte itemType, short itemTemplateId)
	{
		int num = BeginAddingRecord(date, storageType, 0);
		AppendCharacter(charId);
		AppendItem(itemType, itemTemplateId);
		EndAddingRecord(num);
		return num;
	}

	public int AddStorageItem(int date, TaiwuVillageStorageType storageType, int charId, sbyte itemType, short itemTemplateId)
	{
		int num = BeginAddingRecord(date, storageType, 1);
		AppendCharacter(charId);
		AppendItem(itemType, itemTemplateId);
		EndAddingRecord(num);
		return num;
	}

	public int AddStorageResources(int date, TaiwuVillageStorageType storageType, int charId, int value, sbyte resourceType)
	{
		int num = BeginAddingRecord(date, storageType, 2);
		AppendCharacter(charId);
		AppendInteger(value);
		AppendResource(resourceType);
		EndAddingRecord(num);
		return num;
	}

	public int AddTakeResources(int date, TaiwuVillageStorageType storageType, int charId, int value, sbyte resourceType)
	{
		int num = BeginAddingRecord(date, storageType, 3);
		AppendCharacter(charId);
		AppendInteger(value);
		AppendResource(resourceType);
		EndAddingRecord(num);
		return num;
	}

	public int AddGatherResources(int date, TaiwuVillageStorageType storageType, int charId, sbyte itemType, short itemTemplateId)
	{
		int num = BeginAddingRecord(date, storageType, 4);
		AppendCharacter(charId);
		AppendItem(itemType, itemTemplateId);
		EndAddingRecord(num);
		return num;
	}

	public int AddMigrateResources(int date, TaiwuVillageStorageType storageType, int charId, sbyte itemType, short itemTemplateId)
	{
		int num = BeginAddingRecord(date, storageType, 5);
		AppendCharacter(charId);
		AppendItem(itemType, itemTemplateId);
		EndAddingRecord(num);
		return num;
	}

	public int AddCookingIngredient(int date, TaiwuVillageStorageType storageType, int charId, sbyte itemType, short itemTemplateId, sbyte itemType1, short itemTemplateId1)
	{
		int num = BeginAddingRecord(date, storageType, 6);
		AppendCharacter(charId);
		AppendItem(itemType, itemTemplateId);
		AppendItem(itemType1, itemTemplateId1);
		EndAddingRecord(num);
		return num;
	}

	public int AddVillagerMakingItem(int date, TaiwuVillageStorageType storageType, int charId, sbyte itemType, short itemTemplateId, sbyte itemType1, short itemTemplateId1)
	{
		int num = BeginAddingRecord(date, storageType, 7);
		AppendCharacter(charId);
		AppendItem(itemType, itemTemplateId);
		AppendItem(itemType1, itemTemplateId1);
		EndAddingRecord(num);
		return num;
	}

	public int AddVillagerRepairItem(int date, TaiwuVillageStorageType storageType, int charId, sbyte itemType, short itemTemplateId)
	{
		int num = BeginAddingRecord(date, storageType, 8);
		AppendCharacter(charId);
		AppendItem(itemType, itemTemplateId);
		EndAddingRecord(num);
		return num;
	}

	public int AddVillagerDisassembleItem0(int date, TaiwuVillageStorageType storageType, int charId, sbyte itemType, short itemTemplateId)
	{
		int num = BeginAddingRecord(date, storageType, 9);
		AppendCharacter(charId);
		AppendItem(itemType, itemTemplateId);
		EndAddingRecord(num);
		return num;
	}

	public int AddVillagerDisassembleItem1(int date, TaiwuVillageStorageType storageType, int charId, sbyte itemType, short itemTemplateId, sbyte itemType1, short itemTemplateId1)
	{
		int num = BeginAddingRecord(date, storageType, 10);
		AppendCharacter(charId);
		AppendItem(itemType, itemTemplateId);
		AppendItem(itemType1, itemTemplateId1);
		EndAddingRecord(num);
		return num;
	}

	public int AddVillagerRefiningMedicine(int date, TaiwuVillageStorageType storageType, int charId, sbyte itemType, short itemTemplateId, sbyte itemType1, short itemTemplateId1)
	{
		int num = BeginAddingRecord(date, storageType, 11);
		AppendCharacter(charId);
		AppendItem(itemType, itemTemplateId);
		AppendItem(itemType1, itemTemplateId1);
		EndAddingRecord(num);
		return num;
	}

	public int AddVillagerDetoxify0(int date, TaiwuVillageStorageType storageType, int charId, sbyte itemType, short itemTemplateId, sbyte itemType1, short itemTemplateId1)
	{
		int num = BeginAddingRecord(date, storageType, 12);
		AppendCharacter(charId);
		AppendItem(itemType, itemTemplateId);
		AppendItem(itemType1, itemTemplateId1);
		EndAddingRecord(num);
		return num;
	}

	public int AddVillagerDetoxify1(int date, TaiwuVillageStorageType storageType, int charId, sbyte itemType, short itemTemplateId, sbyte itemType1, short itemTemplateId1, sbyte itemType2, short itemTemplateId2)
	{
		int num = BeginAddingRecord(date, storageType, 13);
		AppendCharacter(charId);
		AppendItem(itemType, itemTemplateId);
		AppendItem(itemType1, itemTemplateId1);
		AppendItem(itemType2, itemTemplateId2);
		EndAddingRecord(num);
		return num;
	}

	public int AddVillagerEnvenomedItem(int date, TaiwuVillageStorageType storageType, int charId, sbyte itemType, short itemTemplateId, sbyte itemType1, short itemTemplateId1)
	{
		int num = BeginAddingRecord(date, storageType, 14);
		AppendCharacter(charId);
		AppendItem(itemType, itemTemplateId);
		AppendItem(itemType1, itemTemplateId1);
		EndAddingRecord(num);
		return num;
	}

	public int AddVillagerCure(int date, TaiwuVillageStorageType storageType, int charId, Location location, int value, sbyte resourceType)
	{
		int num = BeginAddingRecord(date, storageType, 15);
		AppendCharacter(charId);
		AppendLocation(location);
		AppendInteger(value);
		AppendResource(resourceType);
		EndAddingRecord(num);
		return num;
	}

	public int AddVillagerSoldItem(int date, TaiwuVillageStorageType storageType, int charId, sbyte itemType, short itemTemplateId, int value, sbyte resourceType)
	{
		int num = BeginAddingRecord(date, storageType, 16);
		AppendCharacter(charId);
		AppendItem(itemType, itemTemplateId);
		AppendInteger(value);
		AppendResource(resourceType);
		EndAddingRecord(num);
		return num;
	}

	public int AddVillagerBuyItem(int date, TaiwuVillageStorageType storageType, int charId, int value, sbyte resourceType, sbyte itemType, short itemTemplateId)
	{
		int num = BeginAddingRecord(date, storageType, 17);
		AppendCharacter(charId);
		AppendInteger(value);
		AppendResource(resourceType);
		AppendItem(itemType, itemTemplateId);
		EndAddingRecord(num);
		return num;
	}

	public int AddOperatingBuilding(int date, TaiwuVillageStorageType storageType, sbyte itemType, short itemTemplateId, short buildingTemplateId)
	{
		int num = BeginAddingRecord(date, storageType, 18);
		AppendItem(itemType, itemTemplateId);
		AppendBuilding(buildingTemplateId);
		EndAddingRecord(num);
		return num;
	}

	public int AddClearRecord(int date, TaiwuVillageStorageType storageType)
	{
		int num = BeginAddingRecord(date, storageType, 19);
		EndAddingRecord(num);
		return num;
	}

	public int AddEnvenomedItemOverload(int date, TaiwuVillageStorageType storageType, int charId, sbyte itemType, short itemTemplateId, sbyte itemType1, short itemTemplateId1, sbyte itemType2, short itemTemplateId2, sbyte itemType3, short itemTemplateId3)
	{
		int num = BeginAddingRecord(date, storageType, 20);
		AppendCharacter(charId);
		AppendItem(itemType, itemTemplateId);
		AppendItem(itemType1, itemTemplateId1);
		AppendItem(itemType2, itemTemplateId2);
		AppendItem(itemType3, itemTemplateId3);
		EndAddingRecord(num);
		return num;
	}

	public int AddDetoxifyItemOverload(int date, TaiwuVillageStorageType storageType, int charId, sbyte itemType, short itemTemplateId, sbyte itemType1, short itemTemplateId1, sbyte itemType2, short itemTemplateId2, sbyte itemType3, short itemTemplateId3, sbyte itemType4, short itemTemplateId4)
	{
		int num = BeginAddingRecord(date, storageType, 21);
		AppendCharacter(charId);
		AppendItem(itemType, itemTemplateId);
		AppendItem(itemType1, itemTemplateId1);
		AppendItem(itemType2, itemTemplateId2);
		AppendItem(itemType3, itemTemplateId3);
		AppendItem(itemType4, itemTemplateId4);
		EndAddingRecord(num);
		return num;
	}

	public int AddGatherResourcesToTreasury(int date, TaiwuVillageStorageType storageType, int charId, sbyte itemType, short itemTemplateId)
	{
		int num = BeginAddingRecord(date, storageType, 22);
		AppendCharacter(charId);
		AppendItem(itemType, itemTemplateId);
		EndAddingRecord(num);
		return num;
	}

	public int AddGatherResourcesToStockStorageGoodsShelf(int date, TaiwuVillageStorageType storageType, int charId, sbyte itemType, short itemTemplateId)
	{
		int num = BeginAddingRecord(date, storageType, 23);
		AppendCharacter(charId);
		AppendItem(itemType, itemTemplateId);
		EndAddingRecord(num);
		return num;
	}

	public int AddGatherResourcesToFoodStorage(int date, TaiwuVillageStorageType storageType, int charId, sbyte itemType, short itemTemplateId)
	{
		int num = BeginAddingRecord(date, storageType, 24);
		AppendCharacter(charId);
		AppendItem(itemType, itemTemplateId);
		EndAddingRecord(num);
		return num;
	}

	public int AddGatherResourcesToMedicineStorage(int date, TaiwuVillageStorageType storageType, int charId, sbyte itemType, short itemTemplateId)
	{
		int num = BeginAddingRecord(date, storageType, 25);
		AppendCharacter(charId);
		AppendItem(itemType, itemTemplateId);
		EndAddingRecord(num);
		return num;
	}

	public int AddGatherResourcesToCraftStorage(int date, TaiwuVillageStorageType storageType, int charId, sbyte itemType, short itemTemplateId)
	{
		int num = BeginAddingRecord(date, storageType, 26);
		AppendCharacter(charId);
		AppendItem(itemType, itemTemplateId);
		EndAddingRecord(num);
		return num;
	}

	public int AddGatherResourcesToCraftStorageToDisassemble(int date, TaiwuVillageStorageType storageType, int charId, sbyte itemType, short itemTemplateId)
	{
		int num = BeginAddingRecord(date, storageType, 27);
		AppendCharacter(charId);
		AppendItem(itemType, itemTemplateId);
		EndAddingRecord(num);
		return num;
	}

	public int AddLoseOverloadResources(int date, TaiwuVillageStorageType storageType, sbyte resourceType, int value)
	{
		int num = BeginAddingRecord(date, storageType, 28);
		AppendResource(resourceType);
		AppendInteger(value);
		EndAddingRecord(num);
		return num;
	}

	public int AddLoseOverloadWarehouseItems(int date, TaiwuVillageStorageType storageType, sbyte itemType, short itemTemplateId)
	{
		int num = BeginAddingRecord(date, storageType, 29);
		AppendItem(itemType, itemTemplateId);
		EndAddingRecord(num);
		return num;
	}

	public int AddVillagerGetRefineItem(int date, TaiwuVillageStorageType storageType, int charId, sbyte itemType, short itemTemplateId)
	{
		int num = BeginAddingRecord(date, storageType, 30);
		AppendCharacter(charId);
		AppendItem(itemType, itemTemplateId);
		EndAddingRecord(num);
		return num;
	}

	public int AddVillagerUpgradeRefineItem(int date, TaiwuVillageStorageType storageType, int charId, sbyte itemType, short itemTemplateId)
	{
		int num = BeginAddingRecord(date, storageType, 31);
		AppendCharacter(charId);
		AppendItem(itemType, itemTemplateId);
		EndAddingRecord(num);
		return num;
	}

	public int AddVillagerEarnMoney(int date, TaiwuVillageStorageType storageType, int charId, int charId1, sbyte resourceType, int value)
	{
		int num = BeginAddingRecord(date, storageType, 32);
		AppendCharacter(charId);
		AppendCharacter(charId1);
		AppendResource(resourceType);
		AppendInteger(value);
		EndAddingRecord(num);
		return num;
	}

	public int AddVillagerEnemyDropItem(int date, TaiwuVillageStorageType storageType, int charId, sbyte itemType, short itemTemplateId)
	{
		int num = BeginAddingRecord(date, storageType, 33);
		AppendCharacter(charId);
		AppendItem(itemType, itemTemplateId);
		EndAddingRecord(num);
		return num;
	}

	public int AddVillagerEnemyDropResources(int date, TaiwuVillageStorageType storageType, int charId, sbyte resourceType, int value)
	{
		int num = BeginAddingRecord(date, storageType, 34);
		AppendCharacter(charId);
		AppendResource(resourceType);
		AppendInteger(value);
		EndAddingRecord(num);
		return num;
	}

	public int AddVillagerMakeHarvest(int date, TaiwuVillageStorageType storageType, short buildingTemplateId, sbyte itemType, short itemTemplateId)
	{
		int num = BeginAddingRecord(date, storageType, 35);
		AppendBuilding(buildingTemplateId);
		AppendItem(itemType, itemTemplateId);
		EndAddingRecord(num);
		return num;
	}

	public int AddOutsiderMakeHarvest(int date, TaiwuVillageStorageType storageType, int charId, short settlementId, sbyte itemType, short itemTemplateId)
	{
		int num = BeginAddingRecord(date, storageType, 36);
		AppendCharacter(charId);
		AppendSettlement(settlementId);
		AppendItem(itemType, itemTemplateId);
		EndAddingRecord(num);
		return num;
	}

	public int AddVillagerMakeHarvest1(int date, TaiwuVillageStorageType storageType, short buildingTemplateId, sbyte itemType, short itemTemplateId)
	{
		int num = BeginAddingRecord(date, storageType, 37);
		AppendBuilding(buildingTemplateId);
		AppendItem(itemType, itemTemplateId);
		EndAddingRecord(num);
		return num;
	}

	public int AddOutsiderMakeHarvest1(int date, TaiwuVillageStorageType storageType, int charId, short settlementId, sbyte itemType, short itemTemplateId)
	{
		int num = BeginAddingRecord(date, storageType, 38);
		AppendCharacter(charId);
		AppendSettlement(settlementId);
		AppendItem(itemType, itemTemplateId);
		EndAddingRecord(num);
		return num;
	}

	public int AddVillagerMakeHarvest2(int date, TaiwuVillageStorageType storageType, short buildingTemplateId, sbyte itemType, short itemTemplateId)
	{
		int num = BeginAddingRecord(date, storageType, 41);
		AppendBuilding(buildingTemplateId);
		AppendItem(itemType, itemTemplateId);
		EndAddingRecord(num);
		return num;
	}

	public int AddOutsiderMakeHarvest2(int date, TaiwuVillageStorageType storageType, int charId, short settlementId, sbyte itemType, short itemTemplateId)
	{
		int num = BeginAddingRecord(date, storageType, 42);
		AppendCharacter(charId);
		AppendSettlement(settlementId);
		AppendItem(itemType, itemTemplateId);
		EndAddingRecord(num);
		return num;
	}

	public int AddVillagerUpgradeRefineItem1(int date, TaiwuVillageStorageType storageType, int charId, sbyte itemType, short itemTemplateId, sbyte itemType1, short itemTemplateId1)
	{
		int num = BeginAddingRecord(date, storageType, 39);
		AppendCharacter(charId);
		AppendItem(itemType, itemTemplateId);
		AppendItem(itemType1, itemTemplateId1);
		EndAddingRecord(num);
		return num;
	}

	public int AddVillagerDonateLegacy(int date, TaiwuVillageStorageType storageType, int charId, sbyte itemType, short itemTemplateId)
	{
		int num = BeginAddingRecord(date, storageType, 40);
		AppendCharacter(charId);
		AppendItem(itemType, itemTemplateId);
		EndAddingRecord(num);
		return num;
	}
}
