using System.Collections.Generic;
using Config;
using GameData.Domains.LifeRecord.GeneralRecord;
using GameData.Domains.Map;
using GameData.Utilities;

namespace GameData.Domains.World.TravelingEvent;

public class TravelingEventCollection : WriteableRecordCollection
{
	public void GetRenderInfos(List<TravelingEventRenderInfo> renderInfos, ArgumentCollection argumentCollection)
	{
		int index = -1;
		int offset = -1;
		while (Next(ref index, ref offset))
		{
			TravelingEventRenderInfo renderInfo = GetRenderInfo(offset, argumentCollection);
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
			return *(short*)(rawData + offset + 1);
		}
	}

	public new unsafe TravelingEventRenderInfo GetRenderInfo(int offset, ArgumentCollection argumentCollection)
	{
		fixed (byte* rawData = RawData)
		{
			byte* ptr = rawData + offset;
			short num = *(short*)(ptr + 1);
			ptr += 3;
			TravelingEventItem travelingEventItem = Config.TravelingEvent.Instance[num];
			if (travelingEventItem == null)
			{
				AdaptableLog.Warning($"Unable to render monthly notification with template id {num}");
				return null;
			}
			string[] parameters = travelingEventItem.Parameters;
			TravelingEventRenderInfo travelingEventRenderInfo = new TravelingEventRenderInfo(num, travelingEventItem.Desc, offset);
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
				travelingEventRenderInfo.Arguments.Add((b, item));
			}
			travelingEventRenderInfo.EventGuid = travelingEventItem.Event;
			return travelingEventRenderInfo;
		}
	}

	private new unsafe int BeginAddingRecord(short recordType)
	{
		int size = Size;
		int num = Size + 1 + 2;
		EnsureCapacity(num);
		Size = num;
		fixed (byte* rawData = RawData)
		{
			*(short*)(rawData + size + 1) = recordType;
		}
		return size;
	}

	public int AddJingjiMaterial(int charId, sbyte itemType, short itemTemplateId)
	{
		int num = BeginAddingRecord(0);
		AppendCharacter(charId);
		AppendItem(itemType, itemTemplateId);
		EndAddingRecord(num);
		return num;
	}

	public int AddBashuMaterial(int charId, sbyte itemType, short itemTemplateId)
	{
		int num = BeginAddingRecord(1);
		AppendCharacter(charId);
		AppendItem(itemType, itemTemplateId);
		EndAddingRecord(num);
		return num;
	}

	public int AddGuangnanMaterial(int charId, sbyte itemType, short itemTemplateId)
	{
		int num = BeginAddingRecord(2);
		AppendCharacter(charId);
		AppendItem(itemType, itemTemplateId);
		EndAddingRecord(num);
		return num;
	}

	public int AddJingBeiMaterial(int charId, sbyte itemType, short itemTemplateId)
	{
		int num = BeginAddingRecord(3);
		AppendCharacter(charId);
		AppendItem(itemType, itemTemplateId);
		EndAddingRecord(num);
		return num;
	}

	public int AddShanxiMaterial(int charId, sbyte itemType, short itemTemplateId)
	{
		int num = BeginAddingRecord(4);
		AppendCharacter(charId);
		AppendItem(itemType, itemTemplateId);
		EndAddingRecord(num);
		return num;
	}

	public int AddGuangdongMaterial(int charId, sbyte itemType, short itemTemplateId)
	{
		int num = BeginAddingRecord(5);
		AppendCharacter(charId);
		AppendItem(itemType, itemTemplateId);
		EndAddingRecord(num);
		return num;
	}

	public int AddShandongMaterial(int charId, sbyte itemType, short itemTemplateId)
	{
		int num = BeginAddingRecord(6);
		AppendCharacter(charId);
		AppendItem(itemType, itemTemplateId);
		EndAddingRecord(num);
		return num;
	}

	public int AddJingnanMaterial(int charId, sbyte itemType, short itemTemplateId)
	{
		int num = BeginAddingRecord(7);
		AppendCharacter(charId);
		AppendItem(itemType, itemTemplateId);
		EndAddingRecord(num);
		return num;
	}

	public int AddFujianMaterial(int charId, sbyte itemType, short itemTemplateId)
	{
		int num = BeginAddingRecord(8);
		AppendCharacter(charId);
		AppendItem(itemType, itemTemplateId);
		EndAddingRecord(num);
		return num;
	}

	public int AddLiaodongMaterial(int charId, sbyte itemType, short itemTemplateId)
	{
		int num = BeginAddingRecord(9);
		AppendCharacter(charId);
		AppendItem(itemType, itemTemplateId);
		EndAddingRecord(num);
		return num;
	}

	public int AddXiyuMaterial(int charId, sbyte itemType, short itemTemplateId)
	{
		int num = BeginAddingRecord(10);
		AppendCharacter(charId);
		AppendItem(itemType, itemTemplateId);
		EndAddingRecord(num);
		return num;
	}

	public int AddYunnanMaterial(int charId, sbyte itemType, short itemTemplateId)
	{
		int num = BeginAddingRecord(11);
		AppendCharacter(charId);
		AppendItem(itemType, itemTemplateId);
		EndAddingRecord(num);
		return num;
	}

	public int AddHuainanMaterial(int charId, sbyte itemType, short itemTemplateId)
	{
		int num = BeginAddingRecord(12);
		AppendCharacter(charId);
		AppendItem(itemType, itemTemplateId);
		EndAddingRecord(num);
		return num;
	}

	public int AddJiangnanMaterial(int charId, sbyte itemType, short itemTemplateId)
	{
		int num = BeginAddingRecord(13);
		AppendCharacter(charId);
		AppendItem(itemType, itemTemplateId);
		EndAddingRecord(num);
		return num;
	}

	public int AddJiangbeiMaterial(int charId, sbyte itemType, short itemTemplateId)
	{
		int num = BeginAddingRecord(14);
		AppendCharacter(charId);
		AppendItem(itemType, itemTemplateId);
		EndAddingRecord(num);
		return num;
	}

	public int AddJingjiResource(int charId, int value, sbyte resourceType)
	{
		int num = BeginAddingRecord(15);
		AppendCharacter(charId);
		AppendInteger(value);
		AppendResource(resourceType);
		EndAddingRecord(num);
		return num;
	}

	public int AddBashuResource(int charId, int value, sbyte resourceType)
	{
		int num = BeginAddingRecord(16);
		AppendCharacter(charId);
		AppendInteger(value);
		AppendResource(resourceType);
		EndAddingRecord(num);
		return num;
	}

	public int AddGuangnanResource(int charId, int value, sbyte resourceType)
	{
		int num = BeginAddingRecord(17);
		AppendCharacter(charId);
		AppendInteger(value);
		AppendResource(resourceType);
		EndAddingRecord(num);
		return num;
	}

	public int AddJingBeiResource(int charId, int value, sbyte resourceType)
	{
		int num = BeginAddingRecord(18);
		AppendCharacter(charId);
		AppendInteger(value);
		AppendResource(resourceType);
		EndAddingRecord(num);
		return num;
	}

	public int AddShanxiResource(int charId, int value, sbyte resourceType)
	{
		int num = BeginAddingRecord(19);
		AppendCharacter(charId);
		AppendInteger(value);
		AppendResource(resourceType);
		EndAddingRecord(num);
		return num;
	}

	public int AddGuangdongResource(int charId, int value, sbyte resourceType)
	{
		int num = BeginAddingRecord(20);
		AppendCharacter(charId);
		AppendInteger(value);
		AppendResource(resourceType);
		EndAddingRecord(num);
		return num;
	}

	public int AddShandongResource(int charId, int value, sbyte resourceType)
	{
		int num = BeginAddingRecord(21);
		AppendCharacter(charId);
		AppendInteger(value);
		AppendResource(resourceType);
		EndAddingRecord(num);
		return num;
	}

	public int AddJingnanResource(int charId, int value, sbyte resourceType)
	{
		int num = BeginAddingRecord(22);
		AppendCharacter(charId);
		AppendInteger(value);
		AppendResource(resourceType);
		EndAddingRecord(num);
		return num;
	}

	public int AddFujianResource(int charId, int value, sbyte resourceType)
	{
		int num = BeginAddingRecord(23);
		AppendCharacter(charId);
		AppendInteger(value);
		AppendResource(resourceType);
		EndAddingRecord(num);
		return num;
	}

	public int AddLiaodongResource(int charId, int value, sbyte resourceType)
	{
		int num = BeginAddingRecord(24);
		AppendCharacter(charId);
		AppendInteger(value);
		AppendResource(resourceType);
		EndAddingRecord(num);
		return num;
	}

	public int AddXiyuResource(int charId, int value, sbyte resourceType)
	{
		int num = BeginAddingRecord(25);
		AppendCharacter(charId);
		AppendInteger(value);
		AppendResource(resourceType);
		EndAddingRecord(num);
		return num;
	}

	public int AddYunnanResource(int charId, int value, sbyte resourceType)
	{
		int num = BeginAddingRecord(26);
		AppendCharacter(charId);
		AppendInteger(value);
		AppendResource(resourceType);
		EndAddingRecord(num);
		return num;
	}

	public int AddHuainanResource(int charId, int value, sbyte resourceType)
	{
		int num = BeginAddingRecord(27);
		AppendCharacter(charId);
		AppendInteger(value);
		AppendResource(resourceType);
		EndAddingRecord(num);
		return num;
	}

	public int AddJiangnanResource(int charId, int value, sbyte resourceType)
	{
		int num = BeginAddingRecord(28);
		AppendCharacter(charId);
		AppendInteger(value);
		AppendResource(resourceType);
		EndAddingRecord(num);
		return num;
	}

	public int AddJiangbeiResource(int charId, int value, sbyte resourceType)
	{
		int num = BeginAddingRecord(29);
		AppendCharacter(charId);
		AppendInteger(value);
		AppendResource(resourceType);
		EndAddingRecord(num);
		return num;
	}

	public int AddJingjiFood(int charId, sbyte itemType, short itemTemplateId)
	{
		int num = BeginAddingRecord(30);
		AppendCharacter(charId);
		AppendItem(itemType, itemTemplateId);
		EndAddingRecord(num);
		return num;
	}

	public int AddBashuFood(int charId, sbyte itemType, short itemTemplateId)
	{
		int num = BeginAddingRecord(31);
		AppendCharacter(charId);
		AppendItem(itemType, itemTemplateId);
		EndAddingRecord(num);
		return num;
	}

	public int AddGuangnanFood(int charId, sbyte itemType, short itemTemplateId)
	{
		int num = BeginAddingRecord(32);
		AppendCharacter(charId);
		AppendItem(itemType, itemTemplateId);
		EndAddingRecord(num);
		return num;
	}

	public int AddJingBeiFood(int charId, sbyte itemType, short itemTemplateId)
	{
		int num = BeginAddingRecord(33);
		AppendCharacter(charId);
		AppendItem(itemType, itemTemplateId);
		EndAddingRecord(num);
		return num;
	}

	public int AddShanxiFood(int charId, sbyte itemType, short itemTemplateId)
	{
		int num = BeginAddingRecord(34);
		AppendCharacter(charId);
		AppendItem(itemType, itemTemplateId);
		EndAddingRecord(num);
		return num;
	}

	public int AddGuangdongFood(int charId, sbyte itemType, short itemTemplateId)
	{
		int num = BeginAddingRecord(35);
		AppendCharacter(charId);
		AppendItem(itemType, itemTemplateId);
		EndAddingRecord(num);
		return num;
	}

	public int AddShandongFood(int charId, sbyte itemType, short itemTemplateId)
	{
		int num = BeginAddingRecord(36);
		AppendCharacter(charId);
		AppendItem(itemType, itemTemplateId);
		EndAddingRecord(num);
		return num;
	}

	public int AddJingnanFood(int charId, sbyte itemType, short itemTemplateId)
	{
		int num = BeginAddingRecord(37);
		AppendCharacter(charId);
		AppendItem(itemType, itemTemplateId);
		EndAddingRecord(num);
		return num;
	}

	public int AddFujianFood(int charId, sbyte itemType, short itemTemplateId)
	{
		int num = BeginAddingRecord(38);
		AppendCharacter(charId);
		AppendItem(itemType, itemTemplateId);
		EndAddingRecord(num);
		return num;
	}

	public int AddLiaodongFood(int charId, sbyte itemType, short itemTemplateId)
	{
		int num = BeginAddingRecord(39);
		AppendCharacter(charId);
		AppendItem(itemType, itemTemplateId);
		EndAddingRecord(num);
		return num;
	}

	public int AddXiyuFood(int charId, sbyte itemType, short itemTemplateId)
	{
		int num = BeginAddingRecord(40);
		AppendCharacter(charId);
		AppendItem(itemType, itemTemplateId);
		EndAddingRecord(num);
		return num;
	}

	public int AddYunnanFood(int charId, sbyte itemType, short itemTemplateId)
	{
		int num = BeginAddingRecord(41);
		AppendCharacter(charId);
		AppendItem(itemType, itemTemplateId);
		EndAddingRecord(num);
		return num;
	}

	public int AddHuainanFood(int charId, sbyte itemType, short itemTemplateId)
	{
		int num = BeginAddingRecord(42);
		AppendCharacter(charId);
		AppendItem(itemType, itemTemplateId);
		EndAddingRecord(num);
		return num;
	}

	public int AddJiangnanFood(int charId, sbyte itemType, short itemTemplateId)
	{
		int num = BeginAddingRecord(43);
		AppendCharacter(charId);
		AppendItem(itemType, itemTemplateId);
		EndAddingRecord(num);
		return num;
	}

	public int AddJiangbeiFood(int charId, sbyte itemType, short itemTemplateId)
	{
		int num = BeginAddingRecord(44);
		AppendCharacter(charId);
		AppendItem(itemType, itemTemplateId);
		EndAddingRecord(num);
		return num;
	}

	public int AddHealOuterInjury(int charId, int value)
	{
		int num = BeginAddingRecord(45);
		AppendCharacter(charId);
		AppendInteger(value);
		EndAddingRecord(num);
		return num;
	}

	public int AddHealInnerInjury(int charId, int value)
	{
		int num = BeginAddingRecord(46);
		AppendCharacter(charId);
		AppendInteger(value);
		EndAddingRecord(num);
		return num;
	}

	public int AddHealPoison(int charId, sbyte poisonType)
	{
		int num = BeginAddingRecord(47);
		AppendCharacter(charId);
		AppendPoisonType(poisonType);
		EndAddingRecord(num);
		return num;
	}

	public int AddHealDisorderOfQi(int charId, int value)
	{
		int num = BeginAddingRecord(48);
		AppendCharacter(charId);
		AppendInteger(value);
		EndAddingRecord(num);
		return num;
	}

	public int AddHealLifeSpan(int charId, int value)
	{
		int num = BeginAddingRecord(49);
		AppendCharacter(charId);
		AppendInteger(value);
		EndAddingRecord(num);
		return num;
	}

	public int AddFriendResource(int charId, Location location, int charId1, int value, sbyte resourceType)
	{
		int num = BeginAddingRecord(50);
		AppendCharacter(charId);
		AppendLocation(location);
		AppendCharacter(charId1);
		AppendInteger(value);
		AppendResource(resourceType);
		EndAddingRecord(num);
		return num;
	}

	public int AddFriendFood(int charId, Location location, int charId1, sbyte itemType, short itemTemplateId)
	{
		int num = BeginAddingRecord(51);
		AppendCharacter(charId);
		AppendLocation(location);
		AppendCharacter(charId1);
		AppendItem(itemType, itemTemplateId);
		EndAddingRecord(num);
		return num;
	}

	public int AddFriendTeaWine(int charId, Location location, int charId1, sbyte itemType, short itemTemplateId)
	{
		int num = BeginAddingRecord(52);
		AppendCharacter(charId);
		AppendLocation(location);
		AppendCharacter(charId1);
		AppendItem(itemType, itemTemplateId);
		EndAddingRecord(num);
		return num;
	}

	public int AddFriendMedicine(int charId, Location location, int charId1, sbyte itemType, short itemTemplateId)
	{
		int num = BeginAddingRecord(53);
		AppendCharacter(charId);
		AppendLocation(location);
		AppendCharacter(charId1);
		AppendItem(itemType, itemTemplateId);
		EndAddingRecord(num);
		return num;
	}

	public int AddFameResource(int charId, Location location, int charId1, int value, sbyte resourceType)
	{
		int num = BeginAddingRecord(54);
		AppendCharacter(charId);
		AppendLocation(location);
		AppendCharacter(charId1);
		AppendInteger(value);
		AppendResource(resourceType);
		EndAddingRecord(num);
		return num;
	}

	public int AddFameFood(int charId, Location location, int charId1, sbyte itemType, short itemTemplateId)
	{
		int num = BeginAddingRecord(55);
		AppendCharacter(charId);
		AppendLocation(location);
		AppendCharacter(charId1);
		AppendItem(itemType, itemTemplateId);
		EndAddingRecord(num);
		return num;
	}

	public int AddFameTeaWine(int charId, Location location, int charId1, sbyte itemType, short itemTemplateId)
	{
		int num = BeginAddingRecord(56);
		AppendCharacter(charId);
		AppendLocation(location);
		AppendCharacter(charId1);
		AppendItem(itemType, itemTemplateId);
		EndAddingRecord(num);
		return num;
	}

	public int AddFameMedicine(int charId, Location location, int charId1, sbyte itemType, short itemTemplateId)
	{
		int num = BeginAddingRecord(57);
		AppendCharacter(charId);
		AppendLocation(location);
		AppendCharacter(charId1);
		AppendItem(itemType, itemTemplateId);
		EndAddingRecord(num);
		return num;
	}

	public int AddRecoverStrength(int charId, Location location, int value)
	{
		int num = BeginAddingRecord(58);
		AppendCharacter(charId);
		AppendLocation(location);
		AppendInteger(value);
		EndAddingRecord(num);
		return num;
	}

	public int AddRecoverDexterity(int charId, Location location, int value)
	{
		int num = BeginAddingRecord(59);
		AppendCharacter(charId);
		AppendLocation(location);
		AppendInteger(value);
		EndAddingRecord(num);
		return num;
	}

	public int AddRecoverConcentration(int charId, Location location, int value)
	{
		int num = BeginAddingRecord(60);
		AppendCharacter(charId);
		AppendLocation(location);
		AppendInteger(value);
		EndAddingRecord(num);
		return num;
	}

	public int AddRecoverVitality(int charId, Location location, int value)
	{
		int num = BeginAddingRecord(61);
		AppendCharacter(charId);
		AppendLocation(location);
		AppendInteger(value);
		EndAddingRecord(num);
		return num;
	}

	public int AddRecoverEnergy(int charId, Location location, int value)
	{
		int num = BeginAddingRecord(62);
		AppendCharacter(charId);
		AppendLocation(location);
		AppendInteger(value);
		EndAddingRecord(num);
		return num;
	}

	public int AddRecoverIntelligence(int charId, Location location, int value)
	{
		int num = BeginAddingRecord(63);
		AppendCharacter(charId);
		AppendLocation(location);
		AppendInteger(value);
		EndAddingRecord(num);
		return num;
	}

	public int AddAreaInteractGood(int charId, Location location, int charId1)
	{
		int num = BeginAddingRecord(64);
		AppendCharacter(charId);
		AppendLocation(location);
		AppendCharacter(charId1);
		EndAddingRecord(num);
		return num;
	}

	public int AddAreaInteractNormal(int charId, Location location, int charId1)
	{
		int num = BeginAddingRecord(65);
		AppendCharacter(charId);
		AppendLocation(location);
		AppendCharacter(charId1);
		EndAddingRecord(num);
		return num;
	}

	public int AddAreaInteractBad(int charId, Location location, int charId1)
	{
		int num = BeginAddingRecord(66);
		AppendCharacter(charId);
		AppendLocation(location);
		AppendCharacter(charId1);
		EndAddingRecord(num);
		return num;
	}

	public int AddAreaInteractIgnored(int charId, Location location, int charId1)
	{
		int num = BeginAddingRecord(67);
		AppendCharacter(charId);
		AppendLocation(location);
		AppendCharacter(charId1);
		EndAddingRecord(num);
		return num;
	}

	public int AddJingjiAreaSpiritualDebtSucceed(int charId, short settlementId, short characterPropertyReferencedType, float floatValue)
	{
		int num = BeginAddingRecord(68);
		AppendCharacter(charId);
		AppendSettlement(settlementId);
		AppendCharacterPropertyReferencedType(characterPropertyReferencedType);
		AppendFloat(floatValue);
		EndAddingRecord(num);
		return num;
	}

	public int AddBashuAreaSpiritualDebtSucceed(int charId, short settlementId, short characterPropertyReferencedType, float floatValue)
	{
		int num = BeginAddingRecord(69);
		AppendCharacter(charId);
		AppendSettlement(settlementId);
		AppendCharacterPropertyReferencedType(characterPropertyReferencedType);
		AppendFloat(floatValue);
		EndAddingRecord(num);
		return num;
	}

	public int AddGuangnanAreaSpiritualDebtSucceed(int charId, short settlementId, short characterPropertyReferencedType, float floatValue)
	{
		int num = BeginAddingRecord(70);
		AppendCharacter(charId);
		AppendSettlement(settlementId);
		AppendCharacterPropertyReferencedType(characterPropertyReferencedType);
		AppendFloat(floatValue);
		EndAddingRecord(num);
		return num;
	}

	public int AddJingBeiAreaSpiritualDebtSucceed(int charId, short settlementId, short characterPropertyReferencedType, float floatValue)
	{
		int num = BeginAddingRecord(71);
		AppendCharacter(charId);
		AppendSettlement(settlementId);
		AppendCharacterPropertyReferencedType(characterPropertyReferencedType);
		AppendFloat(floatValue);
		EndAddingRecord(num);
		return num;
	}

	public int AddShanxiAreaSpiritualDebtSucceed(int charId, short settlementId, short characterPropertyReferencedType, float floatValue)
	{
		int num = BeginAddingRecord(72);
		AppendCharacter(charId);
		AppendSettlement(settlementId);
		AppendCharacterPropertyReferencedType(characterPropertyReferencedType);
		AppendFloat(floatValue);
		EndAddingRecord(num);
		return num;
	}

	public int AddGuangdongAreaSpiritualDebtSucceed(int charId, short settlementId, short characterPropertyReferencedType, float floatValue)
	{
		int num = BeginAddingRecord(73);
		AppendCharacter(charId);
		AppendSettlement(settlementId);
		AppendCharacterPropertyReferencedType(characterPropertyReferencedType);
		AppendFloat(floatValue);
		EndAddingRecord(num);
		return num;
	}

	public int AddShandongAreaSpiritualDebtSucceed(int charId, short settlementId, short characterPropertyReferencedType, float floatValue)
	{
		int num = BeginAddingRecord(74);
		AppendCharacter(charId);
		AppendSettlement(settlementId);
		AppendCharacterPropertyReferencedType(characterPropertyReferencedType);
		AppendFloat(floatValue);
		EndAddingRecord(num);
		return num;
	}

	public int AddJingnanAreaSpiritualDebtSucceed(int charId, short settlementId, short characterPropertyReferencedType, float floatValue)
	{
		int num = BeginAddingRecord(75);
		AppendCharacter(charId);
		AppendSettlement(settlementId);
		AppendCharacterPropertyReferencedType(characterPropertyReferencedType);
		AppendFloat(floatValue);
		EndAddingRecord(num);
		return num;
	}

	public int AddFujianAreaSpiritualDebtSucceed(int charId, short settlementId, short characterPropertyReferencedType, float floatValue)
	{
		int num = BeginAddingRecord(76);
		AppendCharacter(charId);
		AppendSettlement(settlementId);
		AppendCharacterPropertyReferencedType(characterPropertyReferencedType);
		AppendFloat(floatValue);
		EndAddingRecord(num);
		return num;
	}

	public int AddLiaodongAreaSpiritualDebtSucceed(int charId, short settlementId, short characterPropertyReferencedType, float floatValue)
	{
		int num = BeginAddingRecord(77);
		AppendCharacter(charId);
		AppendSettlement(settlementId);
		AppendCharacterPropertyReferencedType(characterPropertyReferencedType);
		AppendFloat(floatValue);
		EndAddingRecord(num);
		return num;
	}

	public int AddXiyuAreaSpiritualDebtSucceed(int charId, short settlementId, short characterPropertyReferencedType, float floatValue)
	{
		int num = BeginAddingRecord(78);
		AppendCharacter(charId);
		AppendSettlement(settlementId);
		AppendCharacterPropertyReferencedType(characterPropertyReferencedType);
		AppendFloat(floatValue);
		EndAddingRecord(num);
		return num;
	}

	public int AddYunnanAreaSpiritualDebtSucceed(int charId, short settlementId, short characterPropertyReferencedType, float floatValue)
	{
		int num = BeginAddingRecord(79);
		AppendCharacter(charId);
		AppendSettlement(settlementId);
		AppendCharacterPropertyReferencedType(characterPropertyReferencedType);
		AppendFloat(floatValue);
		EndAddingRecord(num);
		return num;
	}

	public int AddHuainanAreaSpiritualDebtSucceed(int charId, short settlementId, short characterPropertyReferencedType, float floatValue)
	{
		int num = BeginAddingRecord(80);
		AppendCharacter(charId);
		AppendSettlement(settlementId);
		AppendCharacterPropertyReferencedType(characterPropertyReferencedType);
		AppendFloat(floatValue);
		EndAddingRecord(num);
		return num;
	}

	public int AddJiangnanAreaSpiritualDebtSucceed(int charId, short settlementId, short characterPropertyReferencedType, float floatValue)
	{
		int num = BeginAddingRecord(81);
		AppendCharacter(charId);
		AppendSettlement(settlementId);
		AppendCharacterPropertyReferencedType(characterPropertyReferencedType);
		AppendFloat(floatValue);
		EndAddingRecord(num);
		return num;
	}

	public int AddJiangbeiAreaSpiritualDebtSucceed(int charId, short settlementId, short characterPropertyReferencedType, float floatValue)
	{
		int num = BeginAddingRecord(82);
		AppendCharacter(charId);
		AppendSettlement(settlementId);
		AppendCharacterPropertyReferencedType(characterPropertyReferencedType);
		AppendFloat(floatValue);
		EndAddingRecord(num);
		return num;
	}

	public int AddAreaSpiritualDebtIgnored(int charId, short settlementId)
	{
		int num = BeginAddingRecord(83);
		AppendCharacter(charId);
		AppendSettlement(settlementId);
		EndAddingRecord(num);
		return num;
	}

	public int AddTravelBattlePerfectWin(int charId, short charTemplateId)
	{
		int num = BeginAddingRecord(84);
		AppendCharacter(charId);
		AppendCharacterTemplate(charTemplateId);
		EndAddingRecord(num);
		return num;
	}

	public int AddTravelBattleWin(int charId, short charTemplateId)
	{
		int num = BeginAddingRecord(85);
		AppendCharacter(charId);
		AppendCharacterTemplate(charTemplateId);
		EndAddingRecord(num);
		return num;
	}

	public int AddTravelBattleLose(int charId, short charTemplateId)
	{
		int num = BeginAddingRecord(86);
		AppendCharacter(charId);
		AppendCharacterTemplate(charTemplateId);
		EndAddingRecord(num);
		return num;
	}

	public int AddGroupMemberAccept(int charId, int charId1, int charId2)
	{
		int num = BeginAddingRecord(87);
		AppendCharacter(charId);
		AppendCharacter(charId1);
		AppendCharacter(charId2);
		EndAddingRecord(num);
		return num;
	}

	public int AddGroupMemberRefuse(int charId, int charId1)
	{
		int num = BeginAddingRecord(88);
		AppendCharacter(charId);
		AppendCharacter(charId1);
		EndAddingRecord(num);
		return num;
	}

	public int AddGroupMemberIgnored(int charId, int charId1)
	{
		int num = BeginAddingRecord(89);
		AppendCharacter(charId);
		AppendCharacter(charId1);
		EndAddingRecord(num);
		return num;
	}

	public int AddConsumeStrengthSucceed(int charId, int value)
	{
		int num = BeginAddingRecord(90);
		AppendCharacter(charId);
		AppendInteger(value);
		EndAddingRecord(num);
		return num;
	}

	public int AddConsumeDexteritySucceed(int charId, int value)
	{
		int num = BeginAddingRecord(91);
		AppendCharacter(charId);
		AppendInteger(value);
		EndAddingRecord(num);
		return num;
	}

	public int AddConsumeConcentrationSucceed(int charId, int value)
	{
		int num = BeginAddingRecord(92);
		AppendCharacter(charId);
		AppendInteger(value);
		EndAddingRecord(num);
		return num;
	}

	public int AddConsumeVitalitySucceed(int charId, int value)
	{
		int num = BeginAddingRecord(93);
		AppendCharacter(charId);
		AppendInteger(value);
		EndAddingRecord(num);
		return num;
	}

	public int AddConsumeEnergySucceed(int charId, int value)
	{
		int num = BeginAddingRecord(94);
		AppendCharacter(charId);
		AppendInteger(value);
		EndAddingRecord(num);
		return num;
	}

	public int AddConsumeIntelligenceSucceed(int charId, int value)
	{
		int num = BeginAddingRecord(95);
		AppendCharacter(charId);
		AppendInteger(value);
		EndAddingRecord(num);
		return num;
	}

	public int AddNoConsumeMainAttribute(int charId)
	{
		int num = BeginAddingRecord(96);
		AppendCharacter(charId);
		EndAddingRecord(num);
		return num;
	}

	public int AddRoadBlockAndDetour(int charId, int value)
	{
		int num = BeginAddingRecord(97);
		AppendCharacter(charId);
		AppendInteger(value);
		EndAddingRecord(num);
		return num;
	}

	public int AddRoadBlockAndIgnore(int charId, ulong itemKey)
	{
		int num = BeginAddingRecord(98);
		AppendCharacter(charId);
		AppendItemKey(itemKey);
		EndAddingRecord(num);
		return num;
	}

	public int AddJingjiInteract(int charId, Location location, int charId1)
	{
		int num = BeginAddingRecord(99);
		AppendCharacter(charId);
		AppendLocation(location);
		AppendCharacter(charId1);
		EndAddingRecord(num);
		return num;
	}

	public int AddBashuInteract(int charId, Location location, int charId1)
	{
		int num = BeginAddingRecord(100);
		AppendCharacter(charId);
		AppendLocation(location);
		AppendCharacter(charId1);
		EndAddingRecord(num);
		return num;
	}

	public int AddGuangnanInteract(int charId, Location location, int charId1)
	{
		int num = BeginAddingRecord(101);
		AppendCharacter(charId);
		AppendLocation(location);
		AppendCharacter(charId1);
		EndAddingRecord(num);
		return num;
	}

	public int AddJingBeiInteract(int charId, Location location, int charId1)
	{
		int num = BeginAddingRecord(102);
		AppendCharacter(charId);
		AppendLocation(location);
		AppendCharacter(charId1);
		EndAddingRecord(num);
		return num;
	}

	public int AddShanxiInteract(int charId, Location location, int charId1)
	{
		int num = BeginAddingRecord(103);
		AppendCharacter(charId);
		AppendLocation(location);
		AppendCharacter(charId1);
		EndAddingRecord(num);
		return num;
	}

	public int AddGuangdongInteract(int charId, Location location, int charId1)
	{
		int num = BeginAddingRecord(104);
		AppendCharacter(charId);
		AppendLocation(location);
		AppendCharacter(charId1);
		EndAddingRecord(num);
		return num;
	}

	public int AddShandongInteract(int charId, Location location, int charId1)
	{
		int num = BeginAddingRecord(105);
		AppendCharacter(charId);
		AppendLocation(location);
		AppendCharacter(charId1);
		EndAddingRecord(num);
		return num;
	}

	public int AddJingnanInteract(int charId, Location location, int charId1)
	{
		int num = BeginAddingRecord(106);
		AppendCharacter(charId);
		AppendLocation(location);
		AppendCharacter(charId1);
		EndAddingRecord(num);
		return num;
	}

	public int AddFujianInteract(int charId, Location location, int charId1)
	{
		int num = BeginAddingRecord(107);
		AppendCharacter(charId);
		AppendLocation(location);
		AppendCharacter(charId1);
		EndAddingRecord(num);
		return num;
	}

	public int AddLiaodongInteract(int charId, Location location, int charId1)
	{
		int num = BeginAddingRecord(108);
		AppendCharacter(charId);
		AppendLocation(location);
		AppendCharacter(charId1);
		EndAddingRecord(num);
		return num;
	}

	public int AddXiyuInteract(int charId, Location location, int charId1)
	{
		int num = BeginAddingRecord(109);
		AppendCharacter(charId);
		AppendLocation(location);
		AppendCharacter(charId1);
		EndAddingRecord(num);
		return num;
	}

	public int AddYunnanInteract(int charId, Location location, int charId1)
	{
		int num = BeginAddingRecord(110);
		AppendCharacter(charId);
		AppendLocation(location);
		AppendCharacter(charId1);
		EndAddingRecord(num);
		return num;
	}

	public int AddHuainanInteract(int charId, Location location, int charId1)
	{
		int num = BeginAddingRecord(111);
		AppendCharacter(charId);
		AppendLocation(location);
		AppendCharacter(charId1);
		EndAddingRecord(num);
		return num;
	}

	public int AddJiangnanInteract(int charId, Location location, int charId1)
	{
		int num = BeginAddingRecord(112);
		AppendCharacter(charId);
		AppendLocation(location);
		AppendCharacter(charId1);
		EndAddingRecord(num);
		return num;
	}

	public int AddJiangbeiInteract(int charId, Location location, int charId1)
	{
		int num = BeginAddingRecord(113);
		AppendCharacter(charId);
		AppendLocation(location);
		AppendCharacter(charId1);
		EndAddingRecord(num);
		return num;
	}

	public int AddJingjiAreaSpiritualDebt(int charId, short settlementId)
	{
		int num = BeginAddingRecord(114);
		AppendCharacter(charId);
		AppendSettlement(settlementId);
		EndAddingRecord(num);
		return num;
	}

	public int AddBashuAreaSpiritualDebt(int charId, short settlementId)
	{
		int num = BeginAddingRecord(115);
		AppendCharacter(charId);
		AppendSettlement(settlementId);
		EndAddingRecord(num);
		return num;
	}

	public int AddGuangnanAreaSpiritualDebt(int charId, short settlementId)
	{
		int num = BeginAddingRecord(116);
		AppendCharacter(charId);
		AppendSettlement(settlementId);
		EndAddingRecord(num);
		return num;
	}

	public int AddJingBeiAreaSpiritualDebt(int charId, short settlementId)
	{
		int num = BeginAddingRecord(117);
		AppendCharacter(charId);
		AppendSettlement(settlementId);
		EndAddingRecord(num);
		return num;
	}

	public int AddShanxiAreaSpiritualDebt(int charId, short settlementId)
	{
		int num = BeginAddingRecord(118);
		AppendCharacter(charId);
		AppendSettlement(settlementId);
		EndAddingRecord(num);
		return num;
	}

	public int AddGuangdongAreaSpiritualDebt(int charId, short settlementId)
	{
		int num = BeginAddingRecord(119);
		AppendCharacter(charId);
		AppendSettlement(settlementId);
		EndAddingRecord(num);
		return num;
	}

	public int AddShandongAreaSpiritualDebt(int charId, short settlementId)
	{
		int num = BeginAddingRecord(120);
		AppendCharacter(charId);
		AppendSettlement(settlementId);
		EndAddingRecord(num);
		return num;
	}

	public int AddJingnanAreaSpiritualDebt(int charId, short settlementId)
	{
		int num = BeginAddingRecord(121);
		AppendCharacter(charId);
		AppendSettlement(settlementId);
		EndAddingRecord(num);
		return num;
	}

	public int AddFujianAreaSpiritualDebt(int charId, short settlementId)
	{
		int num = BeginAddingRecord(122);
		AppendCharacter(charId);
		AppendSettlement(settlementId);
		EndAddingRecord(num);
		return num;
	}

	public int AddLiaodongAreaSpiritualDebt(int charId, short settlementId)
	{
		int num = BeginAddingRecord(123);
		AppendCharacter(charId);
		AppendSettlement(settlementId);
		EndAddingRecord(num);
		return num;
	}

	public int AddXiyuAreaSpiritualDebt(int charId, short settlementId)
	{
		int num = BeginAddingRecord(124);
		AppendCharacter(charId);
		AppendSettlement(settlementId);
		EndAddingRecord(num);
		return num;
	}

	public int AddYunnanAreaSpiritualDebt(int charId, short settlementId)
	{
		int num = BeginAddingRecord(125);
		AppendCharacter(charId);
		AppendSettlement(settlementId);
		EndAddingRecord(num);
		return num;
	}

	public int AddHuainanAreaSpiritualDebt(int charId, short settlementId)
	{
		int num = BeginAddingRecord(126);
		AppendCharacter(charId);
		AppendSettlement(settlementId);
		EndAddingRecord(num);
		return num;
	}

	public int AddJiangnanAreaSpiritualDebt(int charId, short settlementId)
	{
		int num = BeginAddingRecord(127);
		AppendCharacter(charId);
		AppendSettlement(settlementId);
		EndAddingRecord(num);
		return num;
	}

	public int AddJiangbeiAreaSpiritualDebt(int charId, short settlementId)
	{
		int num = BeginAddingRecord(128);
		AppendCharacter(charId);
		AppendSettlement(settlementId);
		EndAddingRecord(num);
		return num;
	}

	public int AddVisitShaolin(int charId, Location location)
	{
		int num = BeginAddingRecord(129);
		AppendCharacter(charId);
		AppendLocation(location);
		EndAddingRecord(num);
		return num;
	}

	public int AddVisitEmei(int charId, Location location)
	{
		int num = BeginAddingRecord(130);
		AppendCharacter(charId);
		AppendLocation(location);
		EndAddingRecord(num);
		return num;
	}

	public int AddVisitBaihua(int charId, Location location)
	{
		int num = BeginAddingRecord(131);
		AppendCharacter(charId);
		AppendLocation(location);
		EndAddingRecord(num);
		return num;
	}

	public int AddVisitWudang(int charId, Location location)
	{
		int num = BeginAddingRecord(132);
		AppendCharacter(charId);
		AppendLocation(location);
		EndAddingRecord(num);
		return num;
	}

	public int AddVisitYuanshan(int charId, Location location)
	{
		int num = BeginAddingRecord(133);
		AppendCharacter(charId);
		AppendLocation(location);
		EndAddingRecord(num);
		return num;
	}

	public int AddVisitShixiang(int charId, Location location)
	{
		int num = BeginAddingRecord(134);
		AppendCharacter(charId);
		AppendLocation(location);
		EndAddingRecord(num);
		return num;
	}

	public int AddVisitRanshan(int charId, Location location)
	{
		int num = BeginAddingRecord(135);
		AppendCharacter(charId);
		AppendLocation(location);
		EndAddingRecord(num);
		return num;
	}

	public int AddVisitXuannv(int charId, Location location)
	{
		int num = BeginAddingRecord(136);
		AppendCharacter(charId);
		AppendLocation(location);
		EndAddingRecord(num);
		return num;
	}

	public int AddVisitZhujian(int charId, Location location)
	{
		int num = BeginAddingRecord(137);
		AppendCharacter(charId);
		AppendLocation(location);
		EndAddingRecord(num);
		return num;
	}

	public int AddVisitKongsang(int charId, Location location)
	{
		int num = BeginAddingRecord(138);
		AppendCharacter(charId);
		AppendLocation(location);
		EndAddingRecord(num);
		return num;
	}

	public int AddVisitJingang(int charId, Location location)
	{
		int num = BeginAddingRecord(139);
		AppendCharacter(charId);
		AppendLocation(location);
		EndAddingRecord(num);
		return num;
	}

	public int AddVisitWuxian(int charId, Location location)
	{
		int num = BeginAddingRecord(140);
		AppendCharacter(charId);
		AppendLocation(location);
		EndAddingRecord(num);
		return num;
	}

	public int AddVisitJieqing(int charId, Location location)
	{
		int num = BeginAddingRecord(141);
		AppendCharacter(charId);
		AppendLocation(location);
		EndAddingRecord(num);
		return num;
	}

	public int AddVisitFulong(int charId, Location location)
	{
		int num = BeginAddingRecord(142);
		AppendCharacter(charId);
		AppendLocation(location);
		EndAddingRecord(num);
		return num;
	}

	public int AddVisitXuehou(int charId, Location location)
	{
		int num = BeginAddingRecord(143);
		AppendCharacter(charId);
		AppendLocation(location);
		EndAddingRecord(num);
		return num;
	}

	public int AddEnemyAttack(int charId, short charTemplateId)
	{
		int num = BeginAddingRecord(144);
		AppendCharacter(charId);
		AppendCharacterTemplate(charTemplateId);
		EndAddingRecord(num);
		return num;
	}

	public int AddRighteousAttack(int charId, short charTemplateId)
	{
		int num = BeginAddingRecord(145);
		AppendCharacter(charId);
		AppendCharacterTemplate(charTemplateId);
		EndAddingRecord(num);
		return num;
	}

	public int AddXiangshuMinionAttack(int charId, short charTemplateId)
	{
		int num = BeginAddingRecord(146);
		AppendCharacter(charId);
		AppendCharacterTemplate(charTemplateId);
		EndAddingRecord(num);
		return num;
	}

	public int AddShaolinAttack(int charId, short charTemplateId)
	{
		int num = BeginAddingRecord(147);
		AppendCharacter(charId);
		AppendCharacterTemplate(charTemplateId);
		EndAddingRecord(num);
		return num;
	}

	public int AddEmeiAttack(int charId, short charTemplateId)
	{
		int num = BeginAddingRecord(148);
		AppendCharacter(charId);
		AppendCharacterTemplate(charTemplateId);
		EndAddingRecord(num);
		return num;
	}

	public int AddBaihuaAttack(int charId, short charTemplateId)
	{
		int num = BeginAddingRecord(149);
		AppendCharacter(charId);
		AppendCharacterTemplate(charTemplateId);
		EndAddingRecord(num);
		return num;
	}

	public int AddWudangAttack(int charId, short charTemplateId)
	{
		int num = BeginAddingRecord(150);
		AppendCharacter(charId);
		AppendCharacterTemplate(charTemplateId);
		EndAddingRecord(num);
		return num;
	}

	public int AddYuanshanAttack(int charId, short charTemplateId)
	{
		int num = BeginAddingRecord(151);
		AppendCharacter(charId);
		AppendCharacterTemplate(charTemplateId);
		EndAddingRecord(num);
		return num;
	}

	public int AddJingangAttack(int charId, short charTemplateId)
	{
		int num = BeginAddingRecord(152);
		AppendCharacter(charId);
		AppendCharacterTemplate(charTemplateId);
		EndAddingRecord(num);
		return num;
	}

	public int AddWuxianAttack(int charId, short charTemplateId)
	{
		int num = BeginAddingRecord(153);
		AppendCharacter(charId);
		AppendCharacterTemplate(charTemplateId);
		EndAddingRecord(num);
		return num;
	}

	public int AddJieqingAttack(int charId, short charTemplateId)
	{
		int num = BeginAddingRecord(154);
		AppendCharacter(charId);
		AppendCharacterTemplate(charTemplateId);
		EndAddingRecord(num);
		return num;
	}

	public int AddFulongAttack(int charId, short charTemplateId)
	{
		int num = BeginAddingRecord(155);
		AppendCharacter(charId);
		AppendCharacterTemplate(charTemplateId);
		EndAddingRecord(num);
		return num;
	}

	public int AddXuehouAttack(int charId, short charTemplateId)
	{
		int num = BeginAddingRecord(156);
		AppendCharacter(charId);
		AppendCharacterTemplate(charTemplateId);
		EndAddingRecord(num);
		return num;
	}

	public int AddFriendGroupMember(int charId, Location location, int charId1, int charId2)
	{
		int num = BeginAddingRecord(157);
		AppendCharacter(charId);
		AppendLocation(location);
		AppendCharacter(charId1);
		AppendCharacter(charId2);
		EndAddingRecord(num);
		return num;
	}

	public int AddFameGroupMember(int charId, Location location, int charId1, int charId2)
	{
		int num = BeginAddingRecord(158);
		AppendCharacter(charId);
		AppendLocation(location);
		AppendCharacter(charId1);
		AppendCharacter(charId2);
		EndAddingRecord(num);
		return num;
	}

	public int AddConsumeStrength(int charId, int value)
	{
		int num = BeginAddingRecord(159);
		AppendCharacter(charId);
		AppendInteger(value);
		EndAddingRecord(num);
		return num;
	}

	public int AddConsumeDexterity(int charId, int value)
	{
		int num = BeginAddingRecord(160);
		AppendCharacter(charId);
		AppendInteger(value);
		EndAddingRecord(num);
		return num;
	}

	public int AddConsumeConcentration(int charId, int value)
	{
		int num = BeginAddingRecord(161);
		AppendCharacter(charId);
		AppendInteger(value);
		EndAddingRecord(num);
		return num;
	}

	public int AddConsumeVitality(int charId, int value)
	{
		int num = BeginAddingRecord(162);
		AppendCharacter(charId);
		AppendInteger(value);
		EndAddingRecord(num);
		return num;
	}

	public int AddConsumeEnergy(int charId, int value)
	{
		int num = BeginAddingRecord(163);
		AppendCharacter(charId);
		AppendInteger(value);
		EndAddingRecord(num);
		return num;
	}

	public int AddConsumeIntelligence(int charId, int value)
	{
		int num = BeginAddingRecord(164);
		AppendCharacter(charId);
		AppendInteger(value);
		EndAddingRecord(num);
		return num;
	}

	public int AddRoadBlock(int charId)
	{
		int num = BeginAddingRecord(165);
		AppendCharacter(charId);
		EndAddingRecord(num);
		return num;
	}

	public unsafe override void FillEventArgBox(int offset, IVariantCollection<string> eventArgBox)
	{
		string keyPrefix = "TravelingEvent_arg";
		fixed (byte* rawData = RawData)
		{
			byte* ptr = rawData + offset;
			short index = *(short*)(ptr + 1);
			ptr += 3;
			string[] parameters = Config.TravelingEvent.Instance[index].Parameters;
			int i = 0;
			for (int num = parameters.Length; i < num; i++)
			{
				string text = parameters[i];
				if (string.IsNullOrEmpty(text))
				{
					break;
				}
				sbyte paramType = ParameterType.Parse(text);
				ReadArgumentToEventArgBox(keyPrefix, i, paramType, &ptr, eventArgBox);
			}
		}
	}

	public void CheckParameters(short templateId, params string[] parameters)
	{
		string[] parameters2 = Config.TravelingEvent.Instance[templateId].Parameters;
		for (int i = 0; i < parameters2.Length; i++)
		{
			if (parameters.Length <= i)
			{
				Tester.Assert(string.IsNullOrEmpty(parameters2[i]));
			}
			else
			{
				Tester.Assert(parameters2[i] == parameters[i]);
			}
		}
	}

	public int AddType_AreaMaterial(short templateId, int charId, sbyte itemType, short itemTemplateId)
	{
		Tester.Assert(Config.TravelingEvent.Instance[templateId].Type == ETravelingEventType.AreaMaterial);
		int num = BeginAddingRecord(templateId);
		AppendCharacter(charId);
		AppendItem(itemType, itemTemplateId);
		EndAddingRecord(num);
		return num;
	}

	public int AddType_AreaResource(short templateId, int charId, int value, sbyte resourceType)
	{
		Tester.Assert(Config.TravelingEvent.Instance[templateId].Type == ETravelingEventType.AreaResource);
		int num = BeginAddingRecord(templateId);
		AppendCharacter(charId);
		AppendInteger(value);
		AppendResource(resourceType);
		EndAddingRecord(num);
		return num;
	}

	public int AddType_AreaFood(short templateId, int charId, sbyte itemType, short itemTemplateId)
	{
		Tester.Assert(Config.TravelingEvent.Instance[templateId].Type == ETravelingEventType.AreaFood);
		int num = BeginAddingRecord(templateId);
		AppendCharacter(charId);
		AppendItem(itemType, itemTemplateId);
		EndAddingRecord(num);
		return num;
	}

	public int AddType_CharacterGiftResource(short templateId, int charId, Location location, int charId1, int value, sbyte resourceType)
	{
		Tester.Assert(Config.TravelingEvent.Instance[templateId].Type == ETravelingEventType.CharacterGiftResource);
		int num = BeginAddingRecord(templateId);
		AppendCharacter(charId);
		AppendLocation(location);
		AppendCharacter(charId1);
		AppendInteger(value);
		AppendResource(resourceType);
		EndAddingRecord(num);
		return num;
	}

	public int AddType_CharacterGiftItem(short templateId, int charId, Location location, int charId1, sbyte itemType, short itemTemplateId)
	{
		Tester.Assert(Config.TravelingEvent.Instance[templateId].Type == ETravelingEventType.CharacterGiftItem);
		int num = BeginAddingRecord(templateId);
		AppendCharacter(charId);
		AppendLocation(location);
		AppendCharacter(charId1);
		AppendItem(itemType, itemTemplateId);
		EndAddingRecord(num);
		return num;
	}

	public int AddType_AttributeRegen(short templateId, int charId, Location location, int value)
	{
		Tester.Assert(Config.TravelingEvent.Instance[templateId].Type == ETravelingEventType.AttributeRegen);
		int num = BeginAddingRecord(templateId);
		AppendCharacter(charId);
		AppendLocation(location);
		AppendInteger(value);
		EndAddingRecord(num);
		return num;
	}

	public int AddType_AreaInteraction(short templateId, int charId, Location location, int charId1)
	{
		Tester.Assert(Config.TravelingEvent.Instance[templateId].Type == ETravelingEventType.AreaInteraction);
		int num = BeginAddingRecord(templateId);
		AppendCharacter(charId);
		AppendLocation(location);
		AppendCharacter(charId1);
		EndAddingRecord(num);
		return num;
	}

	public int AddType_SpiritualDebt(short templateId, int charId, short settlementId)
	{
		Tester.Assert(Config.TravelingEvent.Instance[templateId].Type == ETravelingEventType.SpiritualDebt);
		int num = BeginAddingRecord(templateId);
		AppendCharacter(charId);
		AppendSettlement(settlementId);
		EndAddingRecord(num);
		return num;
	}

	public int AddType_SectVisit(short templateId, int charId, Location location)
	{
		Tester.Assert(Config.TravelingEvent.Instance[templateId].Type == ETravelingEventType.SectVisit);
		int num = BeginAddingRecord(templateId);
		AppendCharacter(charId);
		AppendLocation(location);
		EndAddingRecord(num);
		return num;
	}

	public int AddType_Combat(short templateId, int charId, short charTemplateId)
	{
		Tester.Assert(Config.TravelingEvent.Instance[templateId].Type == ETravelingEventType.Combat);
		int num = BeginAddingRecord(templateId);
		AppendCharacter(charId);
		AppendCharacterTemplate(charTemplateId);
		EndAddingRecord(num);
		return num;
	}

	public int AddType_SectCombat(short templateId, int charId, short charTemplateId)
	{
		Tester.Assert(Config.TravelingEvent.Instance[templateId].Type == ETravelingEventType.SectCombat);
		int num = BeginAddingRecord(templateId);
		AppendCharacter(charId);
		AppendCharacterTemplate(charTemplateId);
		EndAddingRecord(num);
		return num;
	}

	public int AddType_CharacterRecommendVillager(short templateId, int charId, Location location, int charId1, int charId2)
	{
		Tester.Assert(Config.TravelingEvent.Instance[templateId].Type == ETravelingEventType.CharacterRecommendVillager);
		int num = BeginAddingRecord(templateId);
		AppendCharacter(charId);
		AppendLocation(location);
		AppendCharacter(charId1);
		AppendCharacter(charId2);
		EndAddingRecord(num);
		return num;
	}

	public int AddType_AttributeCost(short templateId, int charId, int value)
	{
		Tester.Assert(Config.TravelingEvent.Instance[templateId].Type == ETravelingEventType.AttributeCost);
		int num = BeginAddingRecord(templateId);
		AppendCharacter(charId);
		AppendInteger(value);
		EndAddingRecord(num);
		return num;
	}
}
