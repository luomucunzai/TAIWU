using System.Collections.Generic;
using Config;
using GameData.Domains.Item;
using GameData.Domains.LifeRecord.GeneralRecord;
using GameData.Utilities;

namespace GameData.Domains.Building.ShopEvent;

public class ShopEventCollection : WriteableRecordCollection
{
	public void GetRenderInfos(List<ShopEventRenderInfo> renderInfos, ArgumentCollection argumentCollection)
	{
		int index = -1;
		int offset = -1;
		while (Next(ref index, ref offset))
		{
			ShopEventRenderInfo renderInfo = GetRenderInfo(offset, argumentCollection);
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

	public new unsafe ShopEventRenderInfo GetRenderInfo(int offset, ArgumentCollection argumentCollection)
	{
		fixed (byte* rawData = RawData)
		{
			byte* ptr = rawData + offset;
			ptr++;
			int date = *(int*)ptr;
			ptr += 4;
			short num = *(short*)ptr;
			ptr += 2;
			ShopEventItem shopEventItem = Config.ShopEvent.Instance[num];
			if (shopEventItem == null)
			{
				AdaptableLog.Warning($"Unable to render monthly notification with template id {num}");
				return null;
			}
			string[] parameters = shopEventItem.Parameters;
			ShopEventRenderInfo shopEventRenderInfo = new ShopEventRenderInfo(num, shopEventItem.Desc, date);
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
				shopEventRenderInfo.Arguments.Add((b, item));
			}
			return shopEventRenderInfo;
		}
	}

	private unsafe int BeginAddingRecord(int date, short recordType)
	{
		int size = Size;
		int num = Size + 1 + 4 + 2;
		EnsureCapacity(num);
		Size = num;
		fixed (byte* rawData = RawData)
		{
			byte* num2 = rawData + size;
			*(int*)(num2 + 1) = date;
			((short*)(num2 + 1))[2] = recordType;
		}
		return size;
	}

	public int AddCollectResourceSuccess0(int date, sbyte itemType, short itemTemplateId)
	{
		int num = BeginAddingRecord(date, 0);
		AppendItem(itemType, itemTemplateId);
		EndAddingRecord(num);
		return num;
	}

	public int AddCollectResourceSuccess1(int date, sbyte itemType, short itemTemplateId)
	{
		int num = BeginAddingRecord(date, 1);
		AppendItem(itemType, itemTemplateId);
		EndAddingRecord(num);
		return num;
	}

	public int AddCollectResourceSuccess2(int date, sbyte itemType, short itemTemplateId)
	{
		int num = BeginAddingRecord(date, 2);
		AppendItem(itemType, itemTemplateId);
		EndAddingRecord(num);
		return num;
	}

	public int AddCollectResourceSuccess3(int date, sbyte itemType, short itemTemplateId)
	{
		int num = BeginAddingRecord(date, 3);
		AppendItem(itemType, itemTemplateId);
		EndAddingRecord(num);
		return num;
	}

	public int AddCollectResourceSuccess4(int date, sbyte itemType, short itemTemplateId)
	{
		int num = BeginAddingRecord(date, 4);
		AppendItem(itemType, itemTemplateId);
		EndAddingRecord(num);
		return num;
	}

	public int AddCollectResourceSuccess5(int date, sbyte itemType, short itemTemplateId)
	{
		int num = BeginAddingRecord(date, 5);
		AppendItem(itemType, itemTemplateId);
		EndAddingRecord(num);
		return num;
	}

	public int AddCollectResourceSuccess6(int date, sbyte itemType, short itemTemplateId)
	{
		int num = BeginAddingRecord(date, 6);
		AppendItem(itemType, itemTemplateId);
		EndAddingRecord(num);
		return num;
	}

	public int AddCollectResourceSuccess7(int date, sbyte itemType, short itemTemplateId)
	{
		int num = BeginAddingRecord(date, 7);
		AppendItem(itemType, itemTemplateId);
		EndAddingRecord(num);
		return num;
	}

	public int AddCollectResourceSuccess8(int date, sbyte itemType, short itemTemplateId)
	{
		int num = BeginAddingRecord(date, 8);
		AppendItem(itemType, itemTemplateId);
		EndAddingRecord(num);
		return num;
	}

	public int AddCollectResourceSuccess9(int date, sbyte itemType, short itemTemplateId)
	{
		int num = BeginAddingRecord(date, 9);
		AppendItem(itemType, itemTemplateId);
		EndAddingRecord(num);
		return num;
	}

	public int AddCollectBetterResourceSuccess0(int date, sbyte itemType, short itemTemplateId)
	{
		int num = BeginAddingRecord(date, 10);
		AppendItem(itemType, itemTemplateId);
		EndAddingRecord(num);
		return num;
	}

	public int AddCollectBetterResourceSuccess1(int date, sbyte itemType, short itemTemplateId)
	{
		int num = BeginAddingRecord(date, 11);
		AppendItem(itemType, itemTemplateId);
		EndAddingRecord(num);
		return num;
	}

	public int AddCollectBetterResourceSuccess2(int date, sbyte itemType, short itemTemplateId)
	{
		int num = BeginAddingRecord(date, 12);
		AppendItem(itemType, itemTemplateId);
		EndAddingRecord(num);
		return num;
	}

	public int AddCollectBetterResourceSuccess3(int date, sbyte itemType, short itemTemplateId)
	{
		int num = BeginAddingRecord(date, 13);
		AppendItem(itemType, itemTemplateId);
		EndAddingRecord(num);
		return num;
	}

	public int AddCollectBetterResourceSuccess4(int date, sbyte itemType, short itemTemplateId)
	{
		int num = BeginAddingRecord(date, 14);
		AppendItem(itemType, itemTemplateId);
		EndAddingRecord(num);
		return num;
	}

	public int AddCollectBetterResourceSuccess5(int date, sbyte itemType, short itemTemplateId)
	{
		int num = BeginAddingRecord(date, 15);
		AppendItem(itemType, itemTemplateId);
		EndAddingRecord(num);
		return num;
	}

	public int AddCollectBetterResourceSuccess6(int date, sbyte itemType, short itemTemplateId)
	{
		int num = BeginAddingRecord(date, 16);
		AppendItem(itemType, itemTemplateId);
		EndAddingRecord(num);
		return num;
	}

	public int AddCollectBetterResourceSuccess7(int date, sbyte itemType, short itemTemplateId)
	{
		int num = BeginAddingRecord(date, 17);
		AppendItem(itemType, itemTemplateId);
		EndAddingRecord(num);
		return num;
	}

	public int AddCollectBetterResourceSuccess8(int date, sbyte itemType, short itemTemplateId)
	{
		int num = BeginAddingRecord(date, 18);
		AppendItem(itemType, itemTemplateId);
		EndAddingRecord(num);
		return num;
	}

	public int AddCollectBetterResourceSuccess9(int date, sbyte itemType, short itemTemplateId)
	{
		int num = BeginAddingRecord(date, 19);
		AppendItem(itemType, itemTemplateId);
		EndAddingRecord(num);
		return num;
	}

	public int AddCollectResourceFail0(int date)
	{
		int num = BeginAddingRecord(date, 20);
		EndAddingRecord(num);
		return num;
	}

	public int AddCollectResourceFail1(int date)
	{
		int num = BeginAddingRecord(date, 21);
		EndAddingRecord(num);
		return num;
	}

	public int AddCollectResourceFail2(int date)
	{
		int num = BeginAddingRecord(date, 22);
		EndAddingRecord(num);
		return num;
	}

	public int AddCollectResourceFail3(int date)
	{
		int num = BeginAddingRecord(date, 23);
		EndAddingRecord(num);
		return num;
	}

	public int AddCollectResourceFail4(int date)
	{
		int num = BeginAddingRecord(date, 24);
		EndAddingRecord(num);
		return num;
	}

	public int AddCollectResourceFail5(int date)
	{
		int num = BeginAddingRecord(date, 25);
		EndAddingRecord(num);
		return num;
	}

	public int AddCollectResourceFail6(int date)
	{
		int num = BeginAddingRecord(date, 26);
		EndAddingRecord(num);
		return num;
	}

	public int AddCollectResourceFail7(int date)
	{
		int num = BeginAddingRecord(date, 27);
		EndAddingRecord(num);
		return num;
	}

	public int AddCollectResourceFail8(int date)
	{
		int num = BeginAddingRecord(date, 28);
		EndAddingRecord(num);
		return num;
	}

	public int AddCollectResourceFail9(int date)
	{
		int num = BeginAddingRecord(date, 29);
		EndAddingRecord(num);
		return num;
	}

	public int AddCollectBetterResourceFail0(int date)
	{
		int num = BeginAddingRecord(date, 30);
		EndAddingRecord(num);
		return num;
	}

	public int AddCollectBetterResourceFail1(int date)
	{
		int num = BeginAddingRecord(date, 31);
		EndAddingRecord(num);
		return num;
	}

	public int AddCollectBetterResourceFail2(int date)
	{
		int num = BeginAddingRecord(date, 32);
		EndAddingRecord(num);
		return num;
	}

	public int AddCollectBetterResourceFail3(int date)
	{
		int num = BeginAddingRecord(date, 33);
		EndAddingRecord(num);
		return num;
	}

	public int AddCollectBetterResourceFail4(int date)
	{
		int num = BeginAddingRecord(date, 34);
		EndAddingRecord(num);
		return num;
	}

	public int AddCollectBetterResourceFail5(int date)
	{
		int num = BeginAddingRecord(date, 35);
		EndAddingRecord(num);
		return num;
	}

	public int AddCollectBetterResourceFail6(int date)
	{
		int num = BeginAddingRecord(date, 36);
		EndAddingRecord(num);
		return num;
	}

	public int AddCollectBetterResourceFail7(int date)
	{
		int num = BeginAddingRecord(date, 37);
		EndAddingRecord(num);
		return num;
	}

	public int AddCollectBetterResourceFail8(int date)
	{
		int num = BeginAddingRecord(date, 38);
		EndAddingRecord(num);
		return num;
	}

	public int AddCollectBetterResourceFail9(int date)
	{
		int num = BeginAddingRecord(date, 39);
		EndAddingRecord(num);
		return num;
	}

	public int AddManageCombatSkillBuildingSuccess0(int date, sbyte resourceType, int value)
	{
		int num = BeginAddingRecord(date, 40);
		AppendResource(resourceType);
		AppendInteger(value);
		EndAddingRecord(num);
		return num;
	}

	public int AddManageCombatSkillBuildingSuccess1(int date)
	{
		int num = BeginAddingRecord(date, 41);
		EndAddingRecord(num);
		return num;
	}

	public int AddManageCombatSkillBuildingSuccess2(int date, sbyte resourceType, int value)
	{
		int num = BeginAddingRecord(date, 42);
		AppendResource(resourceType);
		AppendInteger(value);
		EndAddingRecord(num);
		return num;
	}

	public int AddManageCombatSkillBuildingFail0(int date)
	{
		int num = BeginAddingRecord(date, 43);
		EndAddingRecord(num);
		return num;
	}

	public int AddManageCombatSkillBuildingFail1(int date)
	{
		int num = BeginAddingRecord(date, 44);
		EndAddingRecord(num);
		return num;
	}

	public int AddManageCombatSkillBuildingFail2(int date)
	{
		int num = BeginAddingRecord(date, 45);
		EndAddingRecord(num);
		return num;
	}

	public int AddManageMusicBuildingSuccess0(int date, sbyte resourceType, int value)
	{
		int num = BeginAddingRecord(date, 46);
		AppendResource(resourceType);
		AppendInteger(value);
		EndAddingRecord(num);
		return num;
	}

	public int AddManageMusicBuildingSuccess1(int date)
	{
		int num = BeginAddingRecord(date, 47);
		EndAddingRecord(num);
		return num;
	}

	public int AddManageMusicBuildingSuccess2(int date, sbyte resourceType, int value)
	{
		int num = BeginAddingRecord(date, 48);
		AppendResource(resourceType);
		AppendInteger(value);
		EndAddingRecord(num);
		return num;
	}

	public int AddManageMusicBuildingFail0(int date)
	{
		int num = BeginAddingRecord(date, 49);
		EndAddingRecord(num);
		return num;
	}

	public int AddManageMusicBuildingFail1(int date)
	{
		int num = BeginAddingRecord(date, 50);
		EndAddingRecord(num);
		return num;
	}

	public int AddManageMusicBuildingFail2(int date)
	{
		int num = BeginAddingRecord(date, 51);
		EndAddingRecord(num);
		return num;
	}

	public int AddManageChessBuildingSuccess0(int date, sbyte resourceType, int value)
	{
		int num = BeginAddingRecord(date, 52);
		AppendResource(resourceType);
		AppendInteger(value);
		EndAddingRecord(num);
		return num;
	}

	public int AddManageChessBuildingSuccess1(int date)
	{
		int num = BeginAddingRecord(date, 53);
		EndAddingRecord(num);
		return num;
	}

	public int AddManageChessBuildingSuccess2(int date, sbyte resourceType, int value)
	{
		int num = BeginAddingRecord(date, 54);
		AppendResource(resourceType);
		AppendInteger(value);
		EndAddingRecord(num);
		return num;
	}

	public int AddManageChessBuildingFail0(int date)
	{
		int num = BeginAddingRecord(date, 55);
		EndAddingRecord(num);
		return num;
	}

	public int AddManageChessBuildingFail1(int date)
	{
		int num = BeginAddingRecord(date, 56);
		EndAddingRecord(num);
		return num;
	}

	public int AddManageChessBuildingFail2(int date)
	{
		int num = BeginAddingRecord(date, 57);
		EndAddingRecord(num);
		return num;
	}

	public int AddManagePoemBuildingSuccess0(int date, sbyte resourceType, int value)
	{
		int num = BeginAddingRecord(date, 58);
		AppendResource(resourceType);
		AppendInteger(value);
		EndAddingRecord(num);
		return num;
	}

	public int AddManagePoemBuildingSuccess1(int date)
	{
		int num = BeginAddingRecord(date, 59);
		EndAddingRecord(num);
		return num;
	}

	public int AddManagePoemBuildingSuccess2(int date, sbyte resourceType, int value)
	{
		int num = BeginAddingRecord(date, 60);
		AppendResource(resourceType);
		AppendInteger(value);
		EndAddingRecord(num);
		return num;
	}

	public int AddManagePoemBuildingFail0(int date)
	{
		int num = BeginAddingRecord(date, 61);
		EndAddingRecord(num);
		return num;
	}

	public int AddManagePoemBuildingFail1(int date)
	{
		int num = BeginAddingRecord(date, 62);
		EndAddingRecord(num);
		return num;
	}

	public int AddManagePoemBuildingFail2(int date)
	{
		int num = BeginAddingRecord(date, 63);
		EndAddingRecord(num);
		return num;
	}

	public int AddManagePaintingBuildingSuccess0(int date, sbyte resourceType, int value)
	{
		int num = BeginAddingRecord(date, 64);
		AppendResource(resourceType);
		AppendInteger(value);
		EndAddingRecord(num);
		return num;
	}

	public int AddManagePaintingBuildingSuccess1(int date)
	{
		int num = BeginAddingRecord(date, 65);
		EndAddingRecord(num);
		return num;
	}

	public int AddManagePaintingBuildingSuccess2(int date, sbyte resourceType, int value)
	{
		int num = BeginAddingRecord(date, 66);
		AppendResource(resourceType);
		AppendInteger(value);
		EndAddingRecord(num);
		return num;
	}

	public int AddManagePaintingBuildingFail0(int date)
	{
		int num = BeginAddingRecord(date, 67);
		EndAddingRecord(num);
		return num;
	}

	public int AddManagePaintingBuildingFail1(int date)
	{
		int num = BeginAddingRecord(date, 68);
		EndAddingRecord(num);
		return num;
	}

	public int AddManagePaintingBuildingFail2(int date)
	{
		int num = BeginAddingRecord(date, 69);
		EndAddingRecord(num);
		return num;
	}

	public int AddManageMathBuildingSuccess0(int date, sbyte resourceType, int value)
	{
		int num = BeginAddingRecord(date, 70);
		AppendResource(resourceType);
		AppendInteger(value);
		EndAddingRecord(num);
		return num;
	}

	public int AddManageMathBuildingSuccess1(int date)
	{
		int num = BeginAddingRecord(date, 71);
		EndAddingRecord(num);
		return num;
	}

	public int AddManageMathBuildingSuccess2(int date, sbyte resourceType, int value)
	{
		int num = BeginAddingRecord(date, 72);
		AppendResource(resourceType);
		AppendInteger(value);
		EndAddingRecord(num);
		return num;
	}

	public int AddManageMathBuildingFail0(int date)
	{
		int num = BeginAddingRecord(date, 73);
		EndAddingRecord(num);
		return num;
	}

	public int AddManageMathBuildingFail1(int date)
	{
		int num = BeginAddingRecord(date, 74);
		EndAddingRecord(num);
		return num;
	}

	public int AddManageMathBuildingFail2(int date)
	{
		int num = BeginAddingRecord(date, 75);
		EndAddingRecord(num);
		return num;
	}

	public int AddManageAppraisalBuildingSuccess0(int date, sbyte itemType, short itemTemplateId, sbyte resourceType, int value)
	{
		int num = BeginAddingRecord(date, 76);
		AppendItem(itemType, itemTemplateId);
		AppendResource(resourceType);
		AppendInteger(value);
		EndAddingRecord(num);
		return num;
	}

	public int AddManageAppraisalBuildingSuccess1(int date, sbyte itemType, short itemTemplateId, sbyte resourceType, int value)
	{
		int num = BeginAddingRecord(date, 77);
		AppendItem(itemType, itemTemplateId);
		AppendResource(resourceType);
		AppendInteger(value);
		EndAddingRecord(num);
		return num;
	}

	public int AddManageAppraisalBuildingSuccess2(int date)
	{
		int num = BeginAddingRecord(date, 78);
		EndAddingRecord(num);
		return num;
	}

	public int AddManageAppraisalBuildingSuccess3(int date, sbyte itemType, short itemTemplateId, sbyte resourceType, int value)
	{
		int num = BeginAddingRecord(date, 79);
		AppendItem(itemType, itemTemplateId);
		AppendResource(resourceType);
		AppendInteger(value);
		EndAddingRecord(num);
		return num;
	}

	public int AddManageAppraisalBuildingSuccess4(int date, sbyte itemType, short itemTemplateId)
	{
		int num = BeginAddingRecord(date, 80);
		AppendItem(itemType, itemTemplateId);
		EndAddingRecord(num);
		return num;
	}

	public int AddManageAppraisalBuildingSuccess5(int date, sbyte itemType, short itemTemplateId)
	{
		int num = BeginAddingRecord(date, 81);
		AppendItem(itemType, itemTemplateId);
		EndAddingRecord(num);
		return num;
	}

	public int AddManageAppraisalBuildingFail0(int date)
	{
		int num = BeginAddingRecord(date, 82);
		EndAddingRecord(num);
		return num;
	}

	public int AddManageAppraisalBuildingFail1(int date)
	{
		int num = BeginAddingRecord(date, 83);
		EndAddingRecord(num);
		return num;
	}

	public int AddManageAppraisalBuildingFail2(int date)
	{
		int num = BeginAddingRecord(date, 84);
		EndAddingRecord(num);
		return num;
	}

	public int AddManageAppraisalBuildingFail3(int date)
	{
		int num = BeginAddingRecord(date, 85);
		EndAddingRecord(num);
		return num;
	}

	public int AddManageAppraisalBuildingFail4(int date)
	{
		int num = BeginAddingRecord(date, 86);
		EndAddingRecord(num);
		return num;
	}

	public int AddManageAppraisalBuildingFail5(int date)
	{
		int num = BeginAddingRecord(date, 87);
		EndAddingRecord(num);
		return num;
	}

	public int AddManageForgingBuildingSuccess0(int date, sbyte itemType, short itemTemplateId, sbyte resourceType, int value)
	{
		int num = BeginAddingRecord(date, 88);
		AppendItem(itemType, itemTemplateId);
		AppendResource(resourceType);
		AppendInteger(value);
		EndAddingRecord(num);
		return num;
	}

	public int AddManageForgingBuildingSuccess1(int date)
	{
		int num = BeginAddingRecord(date, 89);
		EndAddingRecord(num);
		return num;
	}

	public int AddManageForgingBuildingSuccess2(int date, sbyte itemType, short itemTemplateId, sbyte resourceType, int value)
	{
		int num = BeginAddingRecord(date, 90);
		AppendItem(itemType, itemTemplateId);
		AppendResource(resourceType);
		AppendInteger(value);
		EndAddingRecord(num);
		return num;
	}

	public int AddManageForgingBuildingSuccess3(int date, sbyte itemType, short itemTemplateId)
	{
		int num = BeginAddingRecord(date, 91);
		AppendItem(itemType, itemTemplateId);
		EndAddingRecord(num);
		return num;
	}

	public int AddManageForgingBuildingSuccess4(int date, sbyte itemType, short itemTemplateId)
	{
		int num = BeginAddingRecord(date, 92);
		AppendItem(itemType, itemTemplateId);
		EndAddingRecord(num);
		return num;
	}

	public int AddManageForgingBuildingFail0(int date)
	{
		int num = BeginAddingRecord(date, 93);
		EndAddingRecord(num);
		return num;
	}

	public int AddManageForgingBuildingFail1(int date)
	{
		int num = BeginAddingRecord(date, 94);
		EndAddingRecord(num);
		return num;
	}

	public int AddManageForgingBuildingFail2(int date)
	{
		int num = BeginAddingRecord(date, 95);
		EndAddingRecord(num);
		return num;
	}

	public int AddManageForgingBuildingFail3(int date)
	{
		int num = BeginAddingRecord(date, 96);
		EndAddingRecord(num);
		return num;
	}

	public int AddManageForgingBuildingFail4(int date)
	{
		int num = BeginAddingRecord(date, 97);
		EndAddingRecord(num);
		return num;
	}

	public int AddManageWoodworkingBuildingSuccess0(int date, sbyte itemType, short itemTemplateId, sbyte resourceType, int value)
	{
		int num = BeginAddingRecord(date, 98);
		AppendItem(itemType, itemTemplateId);
		AppendResource(resourceType);
		AppendInteger(value);
		EndAddingRecord(num);
		return num;
	}

	public int AddManageWoodworkingBuildingSuccess1(int date)
	{
		int num = BeginAddingRecord(date, 99);
		EndAddingRecord(num);
		return num;
	}

	public int AddManageWoodworkingBuildingSuccess2(int date, sbyte itemType, short itemTemplateId, sbyte resourceType, int value)
	{
		int num = BeginAddingRecord(date, 100);
		AppendItem(itemType, itemTemplateId);
		AppendResource(resourceType);
		AppendInteger(value);
		EndAddingRecord(num);
		return num;
	}

	public int AddManageWoodworkingBuildingSuccess3(int date, sbyte itemType, short itemTemplateId)
	{
		int num = BeginAddingRecord(date, 101);
		AppendItem(itemType, itemTemplateId);
		EndAddingRecord(num);
		return num;
	}

	public int AddManageWoodworkingBuildingSuccess4(int date, sbyte itemType, short itemTemplateId)
	{
		int num = BeginAddingRecord(date, 102);
		AppendItem(itemType, itemTemplateId);
		EndAddingRecord(num);
		return num;
	}

	public int AddManageWoodworkingBuildingFail0(int date)
	{
		int num = BeginAddingRecord(date, 103);
		EndAddingRecord(num);
		return num;
	}

	public int AddManageWoodworkingBuildingFail1(int date)
	{
		int num = BeginAddingRecord(date, 104);
		EndAddingRecord(num);
		return num;
	}

	public int AddManageWoodworkingBuildingFail2(int date)
	{
		int num = BeginAddingRecord(date, 105);
		EndAddingRecord(num);
		return num;
	}

	public int AddManageWoodworkingBuildingFail3(int date)
	{
		int num = BeginAddingRecord(date, 106);
		EndAddingRecord(num);
		return num;
	}

	public int AddManageWoodworkingBuildingFail4(int date)
	{
		int num = BeginAddingRecord(date, 107);
		EndAddingRecord(num);
		return num;
	}

	public int AddManageMedicineBuildingSuccess0(int date, sbyte itemType, short itemTemplateId, sbyte resourceType, int value)
	{
		int num = BeginAddingRecord(date, 108);
		AppendItem(itemType, itemTemplateId);
		AppendResource(resourceType);
		AppendInteger(value);
		EndAddingRecord(num);
		return num;
	}

	public int AddManageMedicineBuildingSuccess1(int date)
	{
		int num = BeginAddingRecord(date, 109);
		EndAddingRecord(num);
		return num;
	}

	public int AddManageMedicineBuildingSuccess2(int date, sbyte itemType, short itemTemplateId, sbyte resourceType, int value)
	{
		int num = BeginAddingRecord(date, 110);
		AppendItem(itemType, itemTemplateId);
		AppendResource(resourceType);
		AppendInteger(value);
		EndAddingRecord(num);
		return num;
	}

	public int AddManageMedicineBuildingSuccess3(int date, sbyte itemType, short itemTemplateId)
	{
		int num = BeginAddingRecord(date, 111);
		AppendItem(itemType, itemTemplateId);
		EndAddingRecord(num);
		return num;
	}

	public int AddManageMedicineBuildingSuccess4(int date, sbyte itemType, short itemTemplateId)
	{
		int num = BeginAddingRecord(date, 112);
		AppendItem(itemType, itemTemplateId);
		EndAddingRecord(num);
		return num;
	}

	public int AddManageMedicineBuildingFail0(int date)
	{
		int num = BeginAddingRecord(date, 113);
		EndAddingRecord(num);
		return num;
	}

	public int AddManageMedicineBuildingFail1(int date)
	{
		int num = BeginAddingRecord(date, 114);
		EndAddingRecord(num);
		return num;
	}

	public int AddManageMedicineBuildingFail2(int date)
	{
		int num = BeginAddingRecord(date, 115);
		EndAddingRecord(num);
		return num;
	}

	public int AddManageMedicineBuildingFail3(int date)
	{
		int num = BeginAddingRecord(date, 116);
		EndAddingRecord(num);
		return num;
	}

	public int AddManageMedicineBuildingFail4(int date)
	{
		int num = BeginAddingRecord(date, 117);
		EndAddingRecord(num);
		return num;
	}

	public int AddManageToxicologyBuildingSuccess0(int date, sbyte itemType, short itemTemplateId, sbyte resourceType, int value)
	{
		int num = BeginAddingRecord(date, 118);
		AppendItem(itemType, itemTemplateId);
		AppendResource(resourceType);
		AppendInteger(value);
		EndAddingRecord(num);
		return num;
	}

	public int AddManageToxicologyBuildingSuccess1(int date)
	{
		int num = BeginAddingRecord(date, 119);
		EndAddingRecord(num);
		return num;
	}

	public int AddManageToxicologyBuildingSuccess2(int date, sbyte itemType, short itemTemplateId, sbyte resourceType, int value)
	{
		int num = BeginAddingRecord(date, 120);
		AppendItem(itemType, itemTemplateId);
		AppendResource(resourceType);
		AppendInteger(value);
		EndAddingRecord(num);
		return num;
	}

	public int AddManageToxicologyBuildingSuccess3(int date, sbyte itemType, short itemTemplateId)
	{
		int num = BeginAddingRecord(date, 121);
		AppendItem(itemType, itemTemplateId);
		EndAddingRecord(num);
		return num;
	}

	public int AddManageToxicologyBuildingSuccess4(int date, sbyte itemType, short itemTemplateId)
	{
		int num = BeginAddingRecord(date, 122);
		AppendItem(itemType, itemTemplateId);
		EndAddingRecord(num);
		return num;
	}

	public int AddManageToxicologyBuildingFail0(int date)
	{
		int num = BeginAddingRecord(date, 123);
		EndAddingRecord(num);
		return num;
	}

	public int AddManageToxicologyBuildingFail1(int date)
	{
		int num = BeginAddingRecord(date, 124);
		EndAddingRecord(num);
		return num;
	}

	public int AddManageToxicologyBuildingFail2(int date)
	{
		int num = BeginAddingRecord(date, 125);
		EndAddingRecord(num);
		return num;
	}

	public int AddManageToxicologyBuildingFail3(int date)
	{
		int num = BeginAddingRecord(date, 126);
		EndAddingRecord(num);
		return num;
	}

	public int AddManageToxicologyBuildingFail4(int date)
	{
		int num = BeginAddingRecord(date, 127);
		EndAddingRecord(num);
		return num;
	}

	public int AddManageWeavingBuildingSuccess0(int date, sbyte itemType, short itemTemplateId, sbyte resourceType, int value)
	{
		int num = BeginAddingRecord(date, 128);
		AppendItem(itemType, itemTemplateId);
		AppendResource(resourceType);
		AppendInteger(value);
		EndAddingRecord(num);
		return num;
	}

	public int AddManageWeavingBuildingSuccess1(int date)
	{
		int num = BeginAddingRecord(date, 129);
		EndAddingRecord(num);
		return num;
	}

	public int AddManageWeavingBuildingSuccess2(int date, sbyte itemType, short itemTemplateId, sbyte resourceType, int value)
	{
		int num = BeginAddingRecord(date, 130);
		AppendItem(itemType, itemTemplateId);
		AppendResource(resourceType);
		AppendInteger(value);
		EndAddingRecord(num);
		return num;
	}

	public int AddManageWeavingBuildingSuccess3(int date, sbyte itemType, short itemTemplateId)
	{
		int num = BeginAddingRecord(date, 131);
		AppendItem(itemType, itemTemplateId);
		EndAddingRecord(num);
		return num;
	}

	public int AddManageWeavingBuildingSuccess4(int date, sbyte itemType, short itemTemplateId)
	{
		int num = BeginAddingRecord(date, 132);
		AppendItem(itemType, itemTemplateId);
		EndAddingRecord(num);
		return num;
	}

	public int AddManageWeavingBuildingFail0(int date)
	{
		int num = BeginAddingRecord(date, 133);
		EndAddingRecord(num);
		return num;
	}

	public int AddManageWeavingBuildingFail1(int date)
	{
		int num = BeginAddingRecord(date, 134);
		EndAddingRecord(num);
		return num;
	}

	public int AddManageWeavingBuildingFail2(int date)
	{
		int num = BeginAddingRecord(date, 135);
		EndAddingRecord(num);
		return num;
	}

	public int AddManageWeavingBuildingFail3(int date)
	{
		int num = BeginAddingRecord(date, 136);
		EndAddingRecord(num);
		return num;
	}

	public int AddManageWeavingBuildingFail4(int date)
	{
		int num = BeginAddingRecord(date, 137);
		EndAddingRecord(num);
		return num;
	}

	public int AddManageJadeBuildingSuccess0(int date, sbyte itemType, short itemTemplateId, sbyte resourceType, int value)
	{
		int num = BeginAddingRecord(date, 138);
		AppendItem(itemType, itemTemplateId);
		AppendResource(resourceType);
		AppendInteger(value);
		EndAddingRecord(num);
		return num;
	}

	public int AddManageJadeBuildingSuccess1(int date)
	{
		int num = BeginAddingRecord(date, 139);
		EndAddingRecord(num);
		return num;
	}

	public int AddManageJadeBuildingSuccess2(int date, sbyte itemType, short itemTemplateId, sbyte resourceType, int value)
	{
		int num = BeginAddingRecord(date, 140);
		AppendItem(itemType, itemTemplateId);
		AppendResource(resourceType);
		AppendInteger(value);
		EndAddingRecord(num);
		return num;
	}

	public int AddManageJadeBuildingSuccess3(int date, sbyte itemType, short itemTemplateId)
	{
		int num = BeginAddingRecord(date, 141);
		AppendItem(itemType, itemTemplateId);
		EndAddingRecord(num);
		return num;
	}

	public int AddManageJadeBuildingSuccess4(int date, sbyte itemType, short itemTemplateId)
	{
		int num = BeginAddingRecord(date, 142);
		AppendItem(itemType, itemTemplateId);
		EndAddingRecord(num);
		return num;
	}

	public int AddManageJadeBuildingFail0(int date)
	{
		int num = BeginAddingRecord(date, 143);
		EndAddingRecord(num);
		return num;
	}

	public int AddManageJadeBuildingFail1(int date)
	{
		int num = BeginAddingRecord(date, 144);
		EndAddingRecord(num);
		return num;
	}

	public int AddManageJadeBuildingFail2(int date)
	{
		int num = BeginAddingRecord(date, 145);
		EndAddingRecord(num);
		return num;
	}

	public int AddManageJadeBuildingFail3(int date)
	{
		int num = BeginAddingRecord(date, 146);
		EndAddingRecord(num);
		return num;
	}

	public int AddManageJadeBuildingFail4(int date)
	{
		int num = BeginAddingRecord(date, 147);
		EndAddingRecord(num);
		return num;
	}

	public int AddManageTaoismBuildingSuccess0(int date, sbyte resourceType, int value)
	{
		int num = BeginAddingRecord(date, 148);
		AppendResource(resourceType);
		AppendInteger(value);
		EndAddingRecord(num);
		return num;
	}

	public int AddManageTaoismBuildingSuccess1(int date)
	{
		int num = BeginAddingRecord(date, 149);
		EndAddingRecord(num);
		return num;
	}

	public int AddManageTaoismBuildingSuccess2(int date, sbyte resourceType, int value)
	{
		int num = BeginAddingRecord(date, 150);
		AppendResource(resourceType);
		AppendInteger(value);
		EndAddingRecord(num);
		return num;
	}

	public int AddManageTaoismBuildingFail0(int date)
	{
		int num = BeginAddingRecord(date, 151);
		EndAddingRecord(num);
		return num;
	}

	public int AddManageTaoismBuildingFail1(int date)
	{
		int num = BeginAddingRecord(date, 152);
		EndAddingRecord(num);
		return num;
	}

	public int AddManageTaoismBuildingFail2(int date)
	{
		int num = BeginAddingRecord(date, 153);
		EndAddingRecord(num);
		return num;
	}

	public int AddManageBuddhismBuildingSuccess0(int date, sbyte resourceType, int value)
	{
		int num = BeginAddingRecord(date, 154);
		AppendResource(resourceType);
		AppendInteger(value);
		EndAddingRecord(num);
		return num;
	}

	public int AddManageBuddhismBuildingSuccess1(int date)
	{
		int num = BeginAddingRecord(date, 155);
		EndAddingRecord(num);
		return num;
	}

	public int AddManageBuddhismBuildingSuccess2(int date, sbyte resourceType, int value)
	{
		int num = BeginAddingRecord(date, 156);
		AppendResource(resourceType);
		AppendInteger(value);
		EndAddingRecord(num);
		return num;
	}

	public int AddManageBuddhismBuildingFail0(int date)
	{
		int num = BeginAddingRecord(date, 157);
		EndAddingRecord(num);
		return num;
	}

	public int AddManageBuddhismBuildingFail1(int date)
	{
		int num = BeginAddingRecord(date, 158);
		EndAddingRecord(num);
		return num;
	}

	public int AddManageBuddhismBuildingFail2(int date)
	{
		int num = BeginAddingRecord(date, 159);
		EndAddingRecord(num);
		return num;
	}

	public int AddManageCookingBuildingSuccess0(int date, sbyte itemType, short itemTemplateId, sbyte resourceType, int value)
	{
		int num = BeginAddingRecord(date, 160);
		AppendItem(itemType, itemTemplateId);
		AppendResource(resourceType);
		AppendInteger(value);
		EndAddingRecord(num);
		return num;
	}

	public int AddManageCookingBuildingSuccess1(int date)
	{
		int num = BeginAddingRecord(date, 161);
		EndAddingRecord(num);
		return num;
	}

	public int AddManageCookingBuildingSuccess2(int date, sbyte itemType, short itemTemplateId, sbyte resourceType, int value)
	{
		int num = BeginAddingRecord(date, 162);
		AppendItem(itemType, itemTemplateId);
		AppendResource(resourceType);
		AppendInteger(value);
		EndAddingRecord(num);
		return num;
	}

	public int AddManageCookingBuildingSuccess3(int date, sbyte itemType, short itemTemplateId)
	{
		int num = BeginAddingRecord(date, 163);
		AppendItem(itemType, itemTemplateId);
		EndAddingRecord(num);
		return num;
	}

	public int AddManageCookingBuildingSuccess4(int date, sbyte itemType, short itemTemplateId)
	{
		int num = BeginAddingRecord(date, 164);
		AppendItem(itemType, itemTemplateId);
		EndAddingRecord(num);
		return num;
	}

	public int AddManageCookingBuildingFail0(int date)
	{
		int num = BeginAddingRecord(date, 165);
		EndAddingRecord(num);
		return num;
	}

	public int AddManageCookingBuildingFail1(int date)
	{
		int num = BeginAddingRecord(date, 166);
		EndAddingRecord(num);
		return num;
	}

	public int AddManageCookingBuildingFail2(int date)
	{
		int num = BeginAddingRecord(date, 167);
		EndAddingRecord(num);
		return num;
	}

	public int AddManageCookingBuildingFail3(int date)
	{
		int num = BeginAddingRecord(date, 168);
		EndAddingRecord(num);
		return num;
	}

	public int AddManageCookingBuildingFail4(int date)
	{
		int num = BeginAddingRecord(date, 169);
		EndAddingRecord(num);
		return num;
	}

	public int AddManageEclecticBuildingSuccess0(int date, sbyte itemType, short itemTemplateId, sbyte resourceType, int value)
	{
		int num = BeginAddingRecord(date, 170);
		AppendItem(itemType, itemTemplateId);
		AppendResource(resourceType);
		AppendInteger(value);
		EndAddingRecord(num);
		return num;
	}

	public int AddManageEclecticBuildingSuccess1(int date, sbyte resourceType, int value)
	{
		int num = BeginAddingRecord(date, 171);
		AppendResource(resourceType);
		AppendInteger(value);
		EndAddingRecord(num);
		return num;
	}

	public int AddManageEclecticBuildingSuccess2(int date, sbyte resourceType, int value)
	{
		int num = BeginAddingRecord(date, 172);
		AppendResource(resourceType);
		AppendInteger(value);
		EndAddingRecord(num);
		return num;
	}

	public int AddManageEclecticBuildingSuccess3(int date)
	{
		int num = BeginAddingRecord(date, 173);
		EndAddingRecord(num);
		return num;
	}

	public int AddManageEclecticBuildingSuccess4(int date)
	{
		int num = BeginAddingRecord(date, 174);
		EndAddingRecord(num);
		return num;
	}

	public int AddManageEclecticBuildingSuccess5(int date, sbyte resourceType, int value)
	{
		int num = BeginAddingRecord(date, 175);
		AppendResource(resourceType);
		AppendInteger(value);
		EndAddingRecord(num);
		return num;
	}

	public int AddManageEclecticBuildingSuccess6(int date, sbyte itemType, short itemTemplateId, sbyte resourceType, int value)
	{
		int num = BeginAddingRecord(date, 176);
		AppendItem(itemType, itemTemplateId);
		AppendResource(resourceType);
		AppendInteger(value);
		EndAddingRecord(num);
		return num;
	}

	public int AddManageEclecticBuildingSuccess7(int date, sbyte resourceType, int value)
	{
		int num = BeginAddingRecord(date, 177);
		AppendResource(resourceType);
		AppendInteger(value);
		EndAddingRecord(num);
		return num;
	}

	public int AddManageEclecticBuildingFail0(int date)
	{
		int num = BeginAddingRecord(date, 178);
		EndAddingRecord(num);
		return num;
	}

	public int AddManageEclecticBuildingFail1(int date)
	{
		int num = BeginAddingRecord(date, 179);
		EndAddingRecord(num);
		return num;
	}

	public int AddManageEclecticBuildingFail2(int date)
	{
		int num = BeginAddingRecord(date, 180);
		EndAddingRecord(num);
		return num;
	}

	public int AddManageEclecticBuildingFail3(int date)
	{
		int num = BeginAddingRecord(date, 181);
		EndAddingRecord(num);
		return num;
	}

	public int AddManageEclecticBuildingFail4(int date)
	{
		int num = BeginAddingRecord(date, 182);
		EndAddingRecord(num);
		return num;
	}

	public int AddManageEclecticBuildingFail5(int date)
	{
		int num = BeginAddingRecord(date, 183);
		EndAddingRecord(num);
		return num;
	}

	public int AddManageEclecticBuildingFail6(int date)
	{
		int num = BeginAddingRecord(date, 184);
		EndAddingRecord(num);
		return num;
	}

	public int AddManageEclecticBuildingFail7(int date)
	{
		int num = BeginAddingRecord(date, 185);
		EndAddingRecord(num);
		return num;
	}

	public int AddLearnLifeSkillSuccess(int date, int charId, sbyte itemType, short itemTemplateId)
	{
		int num = BeginAddingRecord(date, 186);
		AppendCharacter(charId);
		AppendItem(itemType, itemTemplateId);
		EndAddingRecord(num);
		return num;
	}

	public int AddLearnCombatSkillSuccess(int date, int charId, sbyte itemType, short itemTemplateId)
	{
		int num = BeginAddingRecord(date, 187);
		AppendCharacter(charId);
		AppendItem(itemType, itemTemplateId);
		EndAddingRecord(num);
		return num;
	}

	public int AddLearnLifeSkillFail(int date, int charId, sbyte lifeSkillType)
	{
		int num = BeginAddingRecord(date, 188);
		AppendCharacter(charId);
		AppendLifeSkillType(lifeSkillType);
		EndAddingRecord(num);
		return num;
	}

	public int AddLearnCombatSkillFail(int date, int charId, sbyte combatSkillType)
	{
		int num = BeginAddingRecord(date, 189);
		AppendCharacter(charId);
		AppendCombatSkillType(combatSkillType);
		EndAddingRecord(num);
		return num;
	}

	public int AddManageLifeSkillAbilityUp(int date, int charId, sbyte lifeSkillType)
	{
		int num = BeginAddingRecord(date, 190);
		AppendCharacter(charId);
		AppendLifeSkillType(lifeSkillType);
		EndAddingRecord(num);
		return num;
	}

	public int AddManageCombatSkillAbilityUp(int date, int charId, sbyte combatSkillType)
	{
		int num = BeginAddingRecord(date, 191);
		AppendCharacter(charId);
		AppendCombatSkillType(combatSkillType);
		EndAddingRecord(num);
		return num;
	}

	public int AddBaseDevelopLifeSkill(int date, int charId, sbyte lifeSkillType)
	{
		int num = BeginAddingRecord(date, 192);
		AppendCharacter(charId);
		AppendLifeSkillType(lifeSkillType);
		EndAddingRecord(num);
		return num;
	}

	public int AddBaseDevelopCombatSkill(int date, int charId, sbyte combatSkillType)
	{
		int num = BeginAddingRecord(date, 193);
		AppendCharacter(charId);
		AppendCombatSkillType(combatSkillType);
		EndAddingRecord(num);
		return num;
	}

	public int AddPersonalityDevelopLifeSkill(int date, int charId, sbyte lifeSkillType)
	{
		int num = BeginAddingRecord(date, 194);
		AppendCharacter(charId);
		AppendLifeSkillType(lifeSkillType);
		EndAddingRecord(num);
		return num;
	}

	public int AddPersonalityDevelopCombatSkill(int date, int charId, sbyte combatSkillType)
	{
		int num = BeginAddingRecord(date, 195);
		AppendCharacter(charId);
		AppendCombatSkillType(combatSkillType);
		EndAddingRecord(num);
		return num;
	}

	public int AddLeaderDevelopLifeSkill(int date, int charId, int charId1, sbyte lifeSkillType)
	{
		int num = BeginAddingRecord(date, 196);
		AppendCharacter(charId);
		AppendCharacter(charId1);
		AppendLifeSkillType(lifeSkillType);
		EndAddingRecord(num);
		return num;
	}

	public int AddLeaderDevelopCombatSkill(int date, int charId, int charId1, sbyte combatSkillType)
	{
		int num = BeginAddingRecord(date, 197);
		AppendCharacter(charId);
		AppendCharacter(charId1);
		AppendCombatSkillType(combatSkillType);
		EndAddingRecord(num);
		return num;
	}

	public int AddLearnLifeSkill(int date, int charId, sbyte itemType, short itemTemplateId, int value)
	{
		int num = BeginAddingRecord(date, 198);
		AppendCharacter(charId);
		AppendItem(itemType, itemTemplateId);
		AppendInteger(value);
		EndAddingRecord(num);
		return num;
	}

	public int AddLearnCombatSkill(int date, int charId, sbyte itemType, short itemTemplateId, int value)
	{
		int num = BeginAddingRecord(date, 199);
		AppendCharacter(charId);
		AppendItem(itemType, itemTemplateId);
		AppendInteger(value);
		EndAddingRecord(num);
		return num;
	}

	public int AddSalaryReceived(int date, int charId, int value, sbyte resourceType)
	{
		int num = BeginAddingRecord(date, 200);
		AppendCharacter(charId);
		AppendInteger(value);
		AppendResource(resourceType);
		EndAddingRecord(num);
		return num;
	}

	public int AddBanquet_1(int date, int charId, sbyte itemType, short itemTemplateId, sbyte itemType1, short itemTemplateId1)
	{
		int num = BeginAddingRecord(date, 201);
		AppendCharacter(charId);
		AppendItem(itemType, itemTemplateId);
		AppendItem(itemType1, itemTemplateId1);
		EndAddingRecord(num);
		return num;
	}

	public int AddBanquet_2(int date, int charId, sbyte itemType, short itemTemplateId, sbyte itemType1, short itemTemplateId1)
	{
		int num = BeginAddingRecord(date, 202);
		AppendCharacter(charId);
		AppendItem(itemType, itemTemplateId);
		AppendItem(itemType1, itemTemplateId1);
		EndAddingRecord(num);
		return num;
	}

	public int AddBanquet_3(int date, int charId, sbyte itemType, short itemTemplateId, sbyte itemType1, short itemTemplateId1, short Feast)
	{
		int num = BeginAddingRecord(date, 203);
		AppendCharacter(charId);
		AppendItem(itemType, itemTemplateId);
		AppendItem(itemType1, itemTemplateId1);
		AppendFeast(Feast);
		EndAddingRecord(num);
		return num;
	}

	public int AddBanquet_4(int date, int charId, sbyte itemType, short itemTemplateId, sbyte itemType1, short itemTemplateId1, short Feast)
	{
		int num = BeginAddingRecord(date, 204);
		AppendCharacter(charId);
		AppendItem(itemType, itemTemplateId);
		AppendItem(itemType1, itemTemplateId1);
		AppendFeast(Feast);
		EndAddingRecord(num);
		return num;
	}

	public int AddBanquet_5(int date, int charId, sbyte itemType, short itemTemplateId, sbyte itemType1, short itemTemplateId1)
	{
		int num = BeginAddingRecord(date, 205);
		AppendCharacter(charId);
		AppendItem(itemType, itemTemplateId);
		AppendItem(itemType1, itemTemplateId1);
		EndAddingRecord(num);
		return num;
	}

	public int AddBanquet_6(int date, int charId, sbyte itemType, short itemTemplateId, sbyte itemType1, short itemTemplateId1)
	{
		int num = BeginAddingRecord(date, 206);
		AppendCharacter(charId);
		AppendItem(itemType, itemTemplateId);
		AppendItem(itemType1, itemTemplateId1);
		EndAddingRecord(num);
		return num;
	}

	public int AddBanquet_7(int date, int charId, sbyte itemType, short itemTemplateId, sbyte itemType1, short itemTemplateId1, short Feast)
	{
		int num = BeginAddingRecord(date, 207);
		AppendCharacter(charId);
		AppendItem(itemType, itemTemplateId);
		AppendItem(itemType1, itemTemplateId1);
		AppendFeast(Feast);
		EndAddingRecord(num);
		return num;
	}

	public int AddBanquet_8(int date, int charId, sbyte itemType, short itemTemplateId, sbyte itemType1, short itemTemplateId1, short Feast)
	{
		int num = BeginAddingRecord(date, 208);
		AppendCharacter(charId);
		AppendItem(itemType, itemTemplateId);
		AppendItem(itemType1, itemTemplateId1);
		AppendFeast(Feast);
		EndAddingRecord(num);
		return num;
	}

	public int AddBanquet_9(int date, int charId)
	{
		int num = BeginAddingRecord(date, 209);
		AppendCharacter(charId);
		EndAddingRecord(num);
		return num;
	}

	public int AddBanquet_10(int date, int charId)
	{
		int num = BeginAddingRecord(date, 210);
		AppendCharacter(charId);
		EndAddingRecord(num);
		return num;
	}

	public int AddCollectItemSuccessRecord(ShopEventItem shopEventCfg, int date, sbyte itemType, short itemTemplateId)
	{
		if (shopEventCfg.Parameters[0] != "Item" || !string.IsNullOrEmpty(shopEventCfg.Parameters[1]))
		{
			AdaptableLog.Warning($"shop event {shopEventCfg.TemplateId} is not a standard collect item success record: {shopEventCfg.Desc}", appendWarningMessage: true);
			return -1;
		}
		int num = BeginAddingRecord(date, shopEventCfg.TemplateId);
		AppendItem(itemType, itemTemplateId);
		EndAddingRecord(num);
		return num;
	}

	public int AddRecruitSuccessRecord(ShopEventItem shopEventCfg, int date)
	{
		Tester.Assert(string.IsNullOrEmpty(shopEventCfg.Parameters[0]));
		int num = BeginAddingRecord(date, shopEventCfg.TemplateId);
		EndAddingRecord(num);
		return num;
	}

	public int AddRecruitWithCostSuccessRecord(ShopEventItem shopEventCfg, int date, sbyte resourceType, int amount)
	{
		if (shopEventCfg.Parameters[0] != "Resource" || shopEventCfg.Parameters[1] != "Integer" || !string.IsNullOrEmpty(shopEventCfg.Parameters[2]))
		{
			AdaptableLog.Warning($"shop event {shopEventCfg.TemplateId} is not a standard recruit people success record: {shopEventCfg.Desc} ", appendWarningMessage: true);
			return -1;
		}
		int num = BeginAddingRecord(date, shopEventCfg.TemplateId);
		AppendResource(resourceType);
		AppendInteger(amount);
		EndAddingRecord(num);
		return num;
	}

	public int AddCollectResourceSuccessRecord(ShopEventItem shopEventCfg, int date, sbyte resourceType, int resourceAmount)
	{
		if (shopEventCfg.Parameters[0] != "Resource" || shopEventCfg.Parameters[1] != "Integer" || !string.IsNullOrEmpty(shopEventCfg.Parameters[2]))
		{
			AdaptableLog.Warning($"shop event {shopEventCfg.TemplateId} is not a standard collect resource success record: {shopEventCfg.Desc} ", appendWarningMessage: true);
			return -1;
		}
		int num = BeginAddingRecord(date, shopEventCfg.TemplateId);
		AppendResource(resourceType);
		AppendInteger(resourceAmount);
		EndAddingRecord(num);
		return num;
	}

	public int AddSellItemSuccessRecord(ShopEventItem shopEventCfg, int date, sbyte itemType, short itemTemplateId, sbyte resourceType, int amount)
	{
		if (shopEventCfg.Parameters[0] != "Item" || shopEventCfg.Parameters[1] != "Resource" || shopEventCfg.Parameters[2] != "Integer" || !string.IsNullOrEmpty(shopEventCfg.Parameters[3]))
		{
			AdaptableLog.Warning($"shop event {shopEventCfg.TemplateId} is not a standard sell item success record: {shopEventCfg.Desc} ", appendWarningMessage: true);
			return -1;
		}
		int num = BeginAddingRecord(date, shopEventCfg.TemplateId);
		AppendItem(itemType, itemTemplateId);
		AppendResource(resourceType);
		AppendInteger(amount);
		EndAddingRecord(num);
		return num;
	}

	public int AddFailureRecord(ShopEventItem shopEventCfg, int date)
	{
		if (!string.IsNullOrEmpty(shopEventCfg.Parameters[0]))
		{
			AdaptableLog.Warning($"shop event {shopEventCfg.TemplateId} is not a standard failure record: {shopEventCfg.Desc} ", appendWarningMessage: true);
			return -1;
		}
		int num = BeginAddingRecord(date, shopEventCfg.TemplateId);
		EndAddingRecord(num);
		return num;
	}

	public int AddFeastRecord(int charId, sbyte happiness, ItemKey dish, ItemKey gift, short feastType, bool loveItem)
	{
		int currDate = ExternalDataBridge.Context.CurrDate;
		short recordType = (short)((happiness <= GlobalConfig.Instance.FeastLowHappiness) ? ((feastType != 9) ? ((!loveItem) ? 203 : 204) : ((!loveItem) ? 201 : 202)) : ((feastType != 9) ? ((!loveItem) ? 207 : 208) : ((!loveItem) ? 205 : 206)));
		int num = BeginAddingRecord(currDate, recordType);
		AppendCharacter(charId);
		AppendItem(dish.ItemType, dish.TemplateId);
		AppendItem(gift.ItemType, gift.TemplateId);
		if (feastType != 9)
		{
			AppendFeast(feastType);
		}
		EndAddingRecord(num);
		return num;
	}
}
