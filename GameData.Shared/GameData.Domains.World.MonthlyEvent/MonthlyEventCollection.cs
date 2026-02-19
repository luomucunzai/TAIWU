using System.Collections.Generic;
using Config;
using Config.ConfigCells;
using GameData.Domains.Item;
using GameData.Domains.LifeRecord.GeneralRecord;
using GameData.Domains.Map;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.World.MonthlyEvent;

public class MonthlyEventCollection : WriteableRecordCollection
{
	public void GetRenderInfos(List<MonthlyEventRenderInfo> renderInfos, ArgumentCollection argumentCollection)
	{
		int index = -1;
		int offset = -1;
		while (Next(ref index, ref offset))
		{
			MonthlyEventRenderInfo renderInfo = GetRenderInfo(offset, argumentCollection);
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

	public new unsafe MonthlyEventRenderInfo GetRenderInfo(int offset, ArgumentCollection argumentCollection)
	{
		fixed (byte* rawData = RawData)
		{
			byte* ptr = rawData + offset;
			short num = *(short*)(ptr + 1);
			ptr += 3;
			MonthlyEventItem monthlyEventItem = Config.MonthlyEvent.Instance[num];
			if (monthlyEventItem == null)
			{
				AdaptableLog.Warning($"Unable to render monthly notification with template id {num}");
				return null;
			}
			string[] parameters = monthlyEventItem.Parameters;
			MonthlyEventRenderInfo monthlyEventRenderInfo = new MonthlyEventRenderInfo(num, monthlyEventItem.Desc, offset);
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
				monthlyEventRenderInfo.Arguments.Add((b, item));
			}
			monthlyEventRenderInfo.EventGuid = monthlyEventItem.Event;
			return monthlyEventRenderInfo;
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

	private unsafe static string ReadString(byte** ppData)
	{
		string result = default(string);
		*ppData += SerializationHelper.Deserialize(*ppData, ref result);
		return result;
	}

	private unsafe void AppendString(string value)
	{
		int size = Size;
		int num = Size + 2 + value.Length * 2;
		EnsureCapacity(num);
		Size = num;
		fixed (byte* rawData = RawData)
		{
			SerializationHelper.Serialize(rawData + size, value);
		}
	}

	public void AddReadingEvent(ulong itemKey)
	{
		int beginOffset = BeginAddingRecord(0);
		AppendItemKey(itemKey);
		EndAddingRecord(beginOffset);
	}

	public void AddTaiwuDeath(int charId, Location location)
	{
		int beginOffset = BeginAddingRecord(1);
		AppendCharacter(charId);
		AppendLocation(location);
		EndAddingRecord(beginOffset);
	}

	public void AddAreaTotallyDestoryed(int charId)
	{
		int beginOffset = BeginAddingRecord(2);
		AppendCharacter(charId);
		EndAddingRecord(beginOffset);
	}

	public void AddTaiwuInfected(int charId, Location location)
	{
		int beginOffset = BeginAddingRecord(3);
		AppendCharacter(charId);
		AppendLocation(location);
		EndAddingRecord(beginOffset);
	}

	public void AddTaiwuInfectedPartially(int charId, Location location)
	{
		int beginOffset = BeginAddingRecord(4);
		AppendCharacter(charId);
		AppendLocation(location);
		EndAddingRecord(beginOffset);
	}

	public void AddRandomEnemyAttack(Location location, short charTemplateId)
	{
		int beginOffset = BeginAddingRecord(5);
		AppendLocation(location);
		AppendCharacterTemplate(charTemplateId);
		EndAddingRecord(beginOffset);
	}

	public void AddRandomAnimalAttack(Location location, short charTemplateId)
	{
		int beginOffset = BeginAddingRecord(6);
		AppendLocation(location);
		AppendCharacterTemplate(charTemplateId);
		EndAddingRecord(beginOffset);
	}

	public void AddRandomRighteousAttack(Location location, short charTemplateId)
	{
		int beginOffset = BeginAddingRecord(7);
		AppendLocation(location);
		AppendCharacterTemplate(charTemplateId);
		EndAddingRecord(beginOffset);
	}

	public void AddInfectedCharacterAttack(Location location, int charId)
	{
		int beginOffset = BeginAddingRecord(8);
		AppendLocation(location);
		AppendCharacter(charId);
		EndAddingRecord(beginOffset);
	}

	public void AddHumanSkeletonAttack(Location location, int charId)
	{
		int beginOffset = BeginAddingRecord(9);
		AppendLocation(location);
		AppendCharacter(charId);
		EndAddingRecord(beginOffset);
	}

	public void AddCricketInDream(int charId, Location location)
	{
		int beginOffset = BeginAddingRecord(10);
		AppendCharacter(charId);
		AppendLocation(location);
		EndAddingRecord(beginOffset);
	}

	public void AddGiveBirthToCricketTaiwu(int charId)
	{
		int beginOffset = BeginAddingRecord(11);
		AppendCharacter(charId);
		EndAddingRecord(beginOffset);
	}

	public void AddGiveBirthToCricketWife(int charId)
	{
		int beginOffset = BeginAddingRecord(12);
		AppendCharacter(charId);
		EndAddingRecord(beginOffset);
	}

	public void AddPrenatalEducationTaiwu(int charId)
	{
		int beginOffset = BeginAddingRecord(13);
		AppendCharacter(charId);
		EndAddingRecord(beginOffset);
	}

	public void AddAbortionTaiwu(int charId, Location location)
	{
		int beginOffset = BeginAddingRecord(14);
		AppendCharacter(charId);
		AppendLocation(location);
		EndAddingRecord(beginOffset);
	}

	public void AddLoseFetusWife(int charId, Location location)
	{
		int beginOffset = BeginAddingRecord(15);
		AppendCharacter(charId);
		AppendLocation(location);
		EndAddingRecord(beginOffset);
	}

	public void AddMotherFetusBothDieTaiwu(int charId, Location location)
	{
		int beginOffset = BeginAddingRecord(16);
		AppendCharacter(charId);
		AppendLocation(location);
		EndAddingRecord(beginOffset);
	}

	public void AddMotherFetusBothDieWife(int charId, Location location)
	{
		int beginOffset = BeginAddingRecord(17);
		AppendCharacter(charId);
		AppendLocation(location);
		EndAddingRecord(beginOffset);
	}

	public void AddDystociaLoseFetusTaiwu(int charId, Location location)
	{
		int beginOffset = BeginAddingRecord(18);
		AppendCharacter(charId);
		AppendLocation(location);
		EndAddingRecord(beginOffset);
	}

	public void AddDystociaLoseFetusWife(int charId, Location location)
	{
		int beginOffset = BeginAddingRecord(19);
		AppendCharacter(charId);
		AppendLocation(location);
		EndAddingRecord(beginOffset);
	}

	public void AddHaveChildBoyTaiwu(int charId, Location location, int charId1)
	{
		int beginOffset = BeginAddingRecord(20);
		AppendCharacter(charId);
		AppendLocation(location);
		AppendCharacter(charId1);
		EndAddingRecord(beginOffset);
	}

	public void AddHaveChildGirlTaiwu(int charId, Location location, int charId1)
	{
		int beginOffset = BeginAddingRecord(21);
		AppendCharacter(charId);
		AppendLocation(location);
		AppendCharacter(charId1);
		EndAddingRecord(beginOffset);
	}

	public void AddHaveChildBoyWife(int charId, Location location, int charId1)
	{
		int beginOffset = BeginAddingRecord(22);
		AppendCharacter(charId);
		AppendLocation(location);
		AppendCharacter(charId1);
		EndAddingRecord(beginOffset);
	}

	public void AddHaveChildGirlWife(int charId, Location location, int charId1)
	{
		int beginOffset = BeginAddingRecord(23);
		AppendCharacter(charId);
		AppendLocation(location);
		AppendCharacter(charId1);
		EndAddingRecord(beginOffset);
	}

	public void AddDystociaButHaveChildBoyTaiwu(int charId, Location location, int charId1)
	{
		int beginOffset = BeginAddingRecord(24);
		AppendCharacter(charId);
		AppendLocation(location);
		AppendCharacter(charId1);
		EndAddingRecord(beginOffset);
	}

	public void AddDystociaButHaveChildGirlTaiwu(int charId, Location location, int charId1)
	{
		int beginOffset = BeginAddingRecord(25);
		AppendCharacter(charId);
		AppendLocation(location);
		AppendCharacter(charId1);
		EndAddingRecord(beginOffset);
	}

	public void AddDystociaButHaveChildBoyWife(int charId, Location location, int charId1)
	{
		int beginOffset = BeginAddingRecord(26);
		AppendCharacter(charId);
		AppendLocation(location);
		AppendCharacter(charId1);
		EndAddingRecord(beginOffset);
	}

	public void AddDystociaButHaveChildGirlWife(int charId, Location location, int charId1)
	{
		int beginOffset = BeginAddingRecord(27);
		AppendCharacter(charId);
		AppendLocation(location);
		AppendCharacter(charId1);
		EndAddingRecord(beginOffset);
	}

	public void AddDystociaAndHaveChildBoyTaiwu(int charId, Location location, int charId1)
	{
		int beginOffset = BeginAddingRecord(28);
		AppendCharacter(charId);
		AppendLocation(location);
		AppendCharacter(charId1);
		EndAddingRecord(beginOffset);
	}

	public void AddDystociaAndHaveChildGirlTaiwu(int charId, Location location, int charId1)
	{
		int beginOffset = BeginAddingRecord(29);
		AppendCharacter(charId);
		AppendLocation(location);
		AppendCharacter(charId1);
		EndAddingRecord(beginOffset);
	}

	public void AddDystociaAndHaveChildBoyWife(int charId, Location location, int charId1)
	{
		int beginOffset = BeginAddingRecord(30);
		AppendCharacter(charId);
		AppendLocation(location);
		AppendCharacter(charId1);
		EndAddingRecord(beginOffset);
	}

	public void AddDystociaAndHaveChildGirlWife(int charId, Location location, int charId1)
	{
		int beginOffset = BeginAddingRecord(31);
		AppendCharacter(charId);
		AppendLocation(location);
		AppendCharacter(charId1);
		EndAddingRecord(beginOffset);
	}

	public void AddAbandonedBabyInVilliage(int charId)
	{
		int beginOffset = BeginAddingRecord(32);
		AppendCharacter(charId);
		EndAddingRecord(beginOffset);
	}

	public void AddChildZhuazhou(int charId)
	{
		int beginOffset = BeginAddingRecord(33);
		AppendCharacter(charId);
		EndAddingRecord(beginOffset);
	}

	public void AddTeachChild(int charId)
	{
		int beginOffset = BeginAddingRecord(34);
		AppendCharacter(charId);
		EndAddingRecord(beginOffset);
	}

	public void AddReachAdulthood(int charId)
	{
		int beginOffset = BeginAddingRecord(35);
		AppendCharacter(charId);
		EndAddingRecord(beginOffset);
	}

	public void AddCaptiveHaveChild(int charId, int charId1, int charId2, int charId3, int charId4, int charId5, int charId6)
	{
		int beginOffset = BeginAddingRecord(36);
		AppendCharacter(charId);
		AppendCharacter(charId1);
		AppendCharacter(charId2);
		AppendCharacter(charId3);
		AppendCharacter(charId4);
		AppendCharacter(charId5);
		AppendCharacter(charId6);
		EndAddingRecord(beginOffset);
	}

	public void AddCaptiveBecomeEnemy(int charId, Location location, int charId1)
	{
		int beginOffset = BeginAddingRecord(37);
		AppendCharacter(charId);
		AppendLocation(location);
		AppendCharacter(charId1);
		EndAddingRecord(beginOffset);
	}

	public void AddGroupGetMarried(int charId, int charId1)
	{
		int beginOffset = BeginAddingRecord(38);
		AppendCharacter(charId);
		AppendCharacter(charId1);
		EndAddingRecord(beginOffset);
	}

	public void AddSpringMarket()
	{
		int beginOffset = BeginAddingRecord(39);
		EndAddingRecord(beginOffset);
	}

	public void AddSummerTownCompetition(Location location)
	{
		int beginOffset = BeginAddingRecord(40);
		AppendLocation(location);
		EndAddingRecord(beginOffset);
	}

	public void AddAutumnCricketContest(Location location)
	{
		int beginOffset = BeginAddingRecord(41);
		AppendLocation(location);
		EndAddingRecord(beginOffset);
	}

	public void AddWinterLifeCompetition(short lifeSkillTemplateId, Location location)
	{
		int beginOffset = BeginAddingRecord(42);
		AppendLifeSkill(lifeSkillTemplateId);
		AppendLocation(location);
		EndAddingRecord(beginOffset);
	}

	public void AddMakeEnemy(int charId, Location location, int charId1)
	{
		int beginOffset = BeginAddingRecord(43);
		AppendCharacter(charId);
		AppendLocation(location);
		AppendCharacter(charId1);
		EndAddingRecord(beginOffset);
	}

	public void AddSeverEnemy(int charId, Location location, int charId1)
	{
		int beginOffset = BeginAddingRecord(44);
		AppendCharacter(charId);
		AppendLocation(location);
		AppendCharacter(charId1);
		EndAddingRecord(beginOffset);
	}

	public void AddAdore(int charId, Location location, int charId1)
	{
		int beginOffset = BeginAddingRecord(45);
		AppendCharacter(charId);
		AppendLocation(location);
		AppendCharacter(charId1);
		EndAddingRecord(beginOffset);
	}

	public void AddConfess(int charId, Location location, int charId1)
	{
		int beginOffset = BeginAddingRecord(46);
		AppendCharacter(charId);
		AppendLocation(location);
		AppendCharacter(charId1);
		EndAddingRecord(beginOffset);
	}

	public void AddBreakup(int charId, Location location, int charId1)
	{
		int beginOffset = BeginAddingRecord(47);
		AppendCharacter(charId);
		AppendLocation(location);
		AppendCharacter(charId1);
		EndAddingRecord(beginOffset);
	}

	public void AddProposeMarriage(int charId, Location location, int charId1)
	{
		int beginOffset = BeginAddingRecord(48);
		AppendCharacter(charId);
		AppendLocation(location);
		AppendCharacter(charId1);
		EndAddingRecord(beginOffset);
	}

	public void AddBecomeFriend(int charId, Location location, int charId1)
	{
		int beginOffset = BeginAddingRecord(49);
		AppendCharacter(charId);
		AppendLocation(location);
		AppendCharacter(charId1);
		EndAddingRecord(beginOffset);
	}

	public void AddSeverFriendship(int charId, Location location, int charId1)
	{
		int beginOffset = BeginAddingRecord(50);
		AppendCharacter(charId);
		AppendLocation(location);
		AppendCharacter(charId1);
		EndAddingRecord(beginOffset);
	}

	public void AddBecomeSwornBrotherOrSister(int charId, Location location, int charId1)
	{
		int beginOffset = BeginAddingRecord(51);
		AppendCharacter(charId);
		AppendLocation(location);
		AppendCharacter(charId1);
		EndAddingRecord(beginOffset);
	}

	public void AddSeverSwornBrotherhood(int charId, Location location, int charId1)
	{
		int beginOffset = BeginAddingRecord(52);
		AppendCharacter(charId);
		AppendLocation(location);
		AppendCharacter(charId1);
		EndAddingRecord(beginOffset);
	}

	public void AddGetAdoptedByFather(int charId, Location location, int charId1)
	{
		int beginOffset = BeginAddingRecord(53);
		AppendCharacter(charId);
		AppendLocation(location);
		AppendCharacter(charId1);
		EndAddingRecord(beginOffset);
	}

	public void AddGetAdoptedByMother(int charId, Location location, int charId1)
	{
		int beginOffset = BeginAddingRecord(54);
		AppendCharacter(charId);
		AppendLocation(location);
		AppendCharacter(charId1);
		EndAddingRecord(beginOffset);
	}

	public void AddAdoptSon(int charId, Location location, int charId1)
	{
		int beginOffset = BeginAddingRecord(55);
		AppendCharacter(charId);
		AppendLocation(location);
		AppendCharacter(charId1);
		EndAddingRecord(beginOffset);
	}

	public void AddAdoptDaughter(int charId, Location location, int charId1)
	{
		int beginOffset = BeginAddingRecord(56);
		AppendCharacter(charId);
		AppendLocation(location);
		AppendCharacter(charId1);
		EndAddingRecord(beginOffset);
	}

	public void AddDie(int charId, Location location)
	{
		int beginOffset = BeginAddingRecord(57);
		AppendCharacter(charId);
		AppendLocation(location);
		EndAddingRecord(beginOffset);
	}

	public void AddEscapeFromPrison(int charId, Location location, int charId1)
	{
		int beginOffset = BeginAddingRecord(58);
		AppendCharacter(charId);
		AppendLocation(location);
		AppendCharacter(charId1);
		EndAddingRecord(beginOffset);
	}

	public void AddAppointmentCancelled(int charId, Location location)
	{
		int beginOffset = BeginAddingRecord(59);
		AppendCharacter(charId);
		AppendLocation(location);
		EndAddingRecord(beginOffset);
	}

	public void AddRevengeAttack(int charId, Location location, int charId1)
	{
		int beginOffset = BeginAddingRecord(60);
		AppendCharacter(charId);
		AppendLocation(location);
		AppendCharacter(charId1);
		EndAddingRecord(beginOffset);
	}

	public void AddAskProtectByRevengeAttack(int charId, Location location, int charId1, int charId2)
	{
		int beginOffset = BeginAddingRecord(61);
		AppendCharacter(charId);
		AppendLocation(location);
		AppendCharacter(charId1);
		AppendCharacter(charId2);
		EndAddingRecord(beginOffset);
	}

	public void AddCatchEnemyPoison(int charId, Location location, int charId1)
	{
		int beginOffset = BeginAddingRecord(62);
		AppendCharacter(charId);
		AppendLocation(location);
		AppendCharacter(charId1);
		EndAddingRecord(beginOffset);
	}

	public void AddEnemyPoisonAndEscape(int charId, Location location)
	{
		int beginOffset = BeginAddingRecord(63);
		AppendCharacter(charId);
		AppendLocation(location);
		EndAddingRecord(beginOffset);
	}

	public void AddCatchEnemyPlotHarm(int charId, Location location, int charId1)
	{
		int beginOffset = BeginAddingRecord(64);
		AppendCharacter(charId);
		AppendLocation(location);
		AppendCharacter(charId1);
		EndAddingRecord(beginOffset);
	}

	public void AddEnemyPlotHarmAndEscape(int charId, Location location)
	{
		int beginOffset = BeginAddingRecord(65);
		AppendCharacter(charId);
		AppendLocation(location);
		EndAddingRecord(beginOffset);
	}

	public void AddRequestHealOuterInjuryByItem(int charId, Location location, int charId1, ulong itemKey, sbyte bodyPartType)
	{
		int beginOffset = BeginAddingRecord(66);
		AppendCharacter(charId);
		AppendLocation(location);
		AppendCharacter(charId1);
		AppendItemKey(itemKey);
		AppendBodyPartType(bodyPartType);
		EndAddingRecord(beginOffset);
	}

	public void AddRequestHealOuterInjuryByResource(int charId, Location location, int charId1, int value)
	{
		int beginOffset = BeginAddingRecord(67);
		AppendCharacter(charId);
		AppendLocation(location);
		AppendCharacter(charId1);
		AppendInteger(value);
		EndAddingRecord(beginOffset);
	}

	public void AddRequestHealInnerInjuryByItem(int charId, Location location, int charId1, ulong itemKey, sbyte bodyPartType)
	{
		int beginOffset = BeginAddingRecord(68);
		AppendCharacter(charId);
		AppendLocation(location);
		AppendCharacter(charId1);
		AppendItemKey(itemKey);
		AppendBodyPartType(bodyPartType);
		EndAddingRecord(beginOffset);
	}

	public void AddRequestHealInnerInjuryByResource(int charId, Location location, int charId1, int value)
	{
		int beginOffset = BeginAddingRecord(69);
		AppendCharacter(charId);
		AppendLocation(location);
		AppendCharacter(charId1);
		AppendInteger(value);
		EndAddingRecord(beginOffset);
	}

	public void AddRequestHealPoisonByItem(int charId, Location location, int charId1, ulong itemKey, sbyte poisonType)
	{
		int beginOffset = BeginAddingRecord(70);
		AppendCharacter(charId);
		AppendLocation(location);
		AppendCharacter(charId1);
		AppendItemKey(itemKey);
		AppendPoisonType(poisonType);
		EndAddingRecord(beginOffset);
	}

	public void AddRequestHealPoisonByResource(int charId, Location location, int charId1, int value, sbyte poisonType)
	{
		int beginOffset = BeginAddingRecord(71);
		AppendCharacter(charId);
		AppendLocation(location);
		AppendCharacter(charId1);
		AppendInteger(value);
		AppendPoisonType(poisonType);
		EndAddingRecord(beginOffset);
	}

	public void AddRequestHealth(int charId, Location location, int charId1, ulong itemKey)
	{
		int beginOffset = BeginAddingRecord(72);
		AppendCharacter(charId);
		AppendLocation(location);
		AppendCharacter(charId1);
		AppendItemKey(itemKey);
		EndAddingRecord(beginOffset);
	}

	public void AddRequestHealDisorderOfQi(int charId, Location location, int charId1, ulong itemKey)
	{
		int beginOffset = BeginAddingRecord(73);
		AppendCharacter(charId);
		AppendLocation(location);
		AppendCharacter(charId1);
		AppendItemKey(itemKey);
		EndAddingRecord(beginOffset);
	}

	public void AddRequestNeili(int charId, Location location, int charId1, ulong itemKey)
	{
		int beginOffset = BeginAddingRecord(74);
		AppendCharacter(charId);
		AppendLocation(location);
		AppendCharacter(charId1);
		AppendItemKey(itemKey);
		EndAddingRecord(beginOffset);
	}

	public void AddRequestKillWug(int charId, Location location, int charId1, ulong itemKey, ulong itemKey1)
	{
		int beginOffset = BeginAddingRecord(75);
		AppendCharacter(charId);
		AppendLocation(location);
		AppendCharacter(charId1);
		AppendItemKey(itemKey);
		AppendItemKey(itemKey1);
		EndAddingRecord(beginOffset);
	}

	public void AddRequestFood(int charId, Location location, int charId1, ulong itemKey, int value)
	{
		int beginOffset = BeginAddingRecord(76);
		AppendCharacter(charId);
		AppendLocation(location);
		AppendCharacter(charId1);
		AppendItemKey(itemKey);
		AppendInteger(value);
		EndAddingRecord(beginOffset);
	}

	public void AddRequestTeaWine(int charId, Location location, int charId1, ulong itemKey)
	{
		int beginOffset = BeginAddingRecord(77);
		AppendCharacter(charId);
		AppendLocation(location);
		AppendCharacter(charId1);
		AppendItemKey(itemKey);
		EndAddingRecord(beginOffset);
	}

	public void AddRequestResource(int charId, Location location, int charId1, int value, sbyte resourceType)
	{
		int beginOffset = BeginAddingRecord(78);
		AppendCharacter(charId);
		AppendLocation(location);
		AppendCharacter(charId1);
		AppendInteger(value);
		AppendResource(resourceType);
		EndAddingRecord(beginOffset);
	}

	public void AddRequestItem(int charId, Location location, int charId1, ulong itemKey, int value)
	{
		int beginOffset = BeginAddingRecord(79);
		AppendCharacter(charId);
		AppendLocation(location);
		AppendCharacter(charId1);
		AppendItemKey(itemKey);
		AppendInteger(value);
		EndAddingRecord(beginOffset);
	}

	public void AddRequestRepairItem(int charId, Location location, int charId1, ulong itemKey, ulong itemKey1, int value, sbyte resourceType)
	{
		int beginOffset = BeginAddingRecord(80);
		AppendCharacter(charId);
		AppendLocation(location);
		AppendCharacter(charId1);
		AppendItemKey(itemKey);
		AppendItemKey(itemKey1);
		AppendInteger(value);
		AppendResource(resourceType);
		EndAddingRecord(beginOffset);
	}

	public void AddRequestAddPoisonToItem(int charId, Location location, int charId1, ulong itemKey, ulong itemKey1)
	{
		int beginOffset = BeginAddingRecord(81);
		AppendCharacter(charId);
		AppendLocation(location);
		AppendCharacter(charId1);
		AppendItemKey(itemKey);
		AppendItemKey(itemKey1);
		EndAddingRecord(beginOffset);
	}

	public void AddRequestInstructionOnLifeSkill(int charId, Location location, int charId1, sbyte itemType, short itemTemplateId, int value)
	{
		int beginOffset = BeginAddingRecord(82);
		AppendCharacter(charId);
		AppendLocation(location);
		AppendCharacter(charId1);
		AppendItem(itemType, itemTemplateId);
		AppendInteger(value);
		EndAddingRecord(beginOffset);
	}

	public void AddRequestInstructionOnCombatSkill(int charId, Location location, int charId1, sbyte itemType, short itemTemplateId, int value, int value1, int value2)
	{
		int beginOffset = BeginAddingRecord(83);
		AppendCharacter(charId);
		AppendLocation(location);
		AppendCharacter(charId1);
		AppendItem(itemType, itemTemplateId);
		AppendInteger(value);
		AppendInteger(value1);
		AppendInteger(value2);
		EndAddingRecord(beginOffset);
	}

	public void AddRequestInstructionOnReadingLifeSkill(int charId, Location location, int charId1, ulong itemKey, int value)
	{
		int beginOffset = BeginAddingRecord(84);
		AppendCharacter(charId);
		AppendLocation(location);
		AppendCharacter(charId1);
		AppendItemKey(itemKey);
		AppendInteger(value);
		EndAddingRecord(beginOffset);
	}

	public void AddRequestInstructionOnReadingCombatSkill(int charId, Location location, int charId1, ulong itemKey, int value, int value1)
	{
		int beginOffset = BeginAddingRecord(85);
		AppendCharacter(charId);
		AppendLocation(location);
		AppendCharacter(charId1);
		AppendItemKey(itemKey);
		AppendInteger(value);
		AppendInteger(value1);
		EndAddingRecord(beginOffset);
	}

	public void AddRequestInstructionOnBreakout(int charId, Location location, int charId1, short combatSkillTemplateId)
	{
		int beginOffset = BeginAddingRecord(86);
		AppendCharacter(charId);
		AppendLocation(location);
		AppendCharacter(charId1);
		AppendCombatSkill(combatSkillTemplateId);
		EndAddingRecord(beginOffset);
	}

	public void AddRequestPlayCombat(int charId, Location location, int charId1)
	{
		int beginOffset = BeginAddingRecord(87);
		AppendCharacter(charId);
		AppendLocation(location);
		AppendCharacter(charId1);
		EndAddingRecord(beginOffset);
	}

	public void AddRequestNormalCombat(int charId, Location location, int charId1)
	{
		int beginOffset = BeginAddingRecord(88);
		AppendCharacter(charId);
		AppendLocation(location);
		AppendCharacter(charId1);
		EndAddingRecord(beginOffset);
	}

	public void AddRequestLifeSkillBattle(int charId, Location location, int charId1)
	{
		int beginOffset = BeginAddingRecord(89);
		AppendCharacter(charId);
		AppendLocation(location);
		AppendCharacter(charId1);
		EndAddingRecord(beginOffset);
	}

	public void AddRequestCricketBattle(int charId, Location location, int charId1)
	{
		int beginOffset = BeginAddingRecord(90);
		AppendCharacter(charId);
		AppendLocation(location);
		AppendCharacter(charId1);
		EndAddingRecord(beginOffset);
	}

	public void AddRescueKidnappedCharacterSecretlyButBeCaught(int charId, Location location, int charId1, int charId2)
	{
		int beginOffset = BeginAddingRecord(91);
		AppendCharacter(charId);
		AppendLocation(location);
		AppendCharacter(charId1);
		AppendCharacter(charId2);
		EndAddingRecord(beginOffset);
	}

	public void AddRescueKidnappedCharacterSecretlyAndEscape(int charId, Location location, int charId1)
	{
		int beginOffset = BeginAddingRecord(92);
		AppendCharacter(charId);
		AppendLocation(location);
		AppendCharacter(charId1);
		EndAddingRecord(beginOffset);
	}

	public void AddRescueKidnappedCharacterWithWit(int charId, Location location, int charId1, int charId2)
	{
		int beginOffset = BeginAddingRecord(93);
		AppendCharacter(charId);
		AppendLocation(location);
		AppendCharacter(charId1);
		AppendCharacter(charId2);
		EndAddingRecord(beginOffset);
	}

	public void AddRescueKidnappedCharacterWithForce(int charId, Location location, int charId1, int charId2)
	{
		int beginOffset = BeginAddingRecord(94);
		AppendCharacter(charId);
		AppendLocation(location);
		AppendCharacter(charId1);
		AppendCharacter(charId2);
		EndAddingRecord(beginOffset);
	}

	public void AddStealResourceButBeCaught(int charId, Location location, int charId1, sbyte resourceType, int value)
	{
		int beginOffset = BeginAddingRecord(95);
		AppendCharacter(charId);
		AppendLocation(location);
		AppendCharacter(charId1);
		AppendResource(resourceType);
		AppendInteger(value);
		EndAddingRecord(beginOffset);
	}

	public void AddStealResourceAndEscape(int charId, Location location, int charId1, sbyte resourceType, int value)
	{
		int beginOffset = BeginAddingRecord(96);
		AppendCharacter(charId);
		AppendLocation(location);
		AppendCharacter(charId1);
		AppendResource(resourceType);
		AppendInteger(value);
		EndAddingRecord(beginOffset);
	}

	public void AddScamResource(int charId, Location location, int charId1, sbyte resourceType, int value)
	{
		int beginOffset = BeginAddingRecord(97);
		AppendCharacter(charId);
		AppendLocation(location);
		AppendCharacter(charId1);
		AppendResource(resourceType);
		AppendInteger(value);
		EndAddingRecord(beginOffset);
	}

	public void AddRobResource(int charId, Location location, int charId1, sbyte resourceType, int value)
	{
		int beginOffset = BeginAddingRecord(98);
		AppendCharacter(charId);
		AppendLocation(location);
		AppendCharacter(charId1);
		AppendResource(resourceType);
		AppendInteger(value);
		EndAddingRecord(beginOffset);
	}

	public void AddStealItemButBeCaught(int charId, Location location, int charId1, ulong itemKey)
	{
		int beginOffset = BeginAddingRecord(99);
		AppendCharacter(charId);
		AppendLocation(location);
		AppendCharacter(charId1);
		AppendItemKey(itemKey);
		EndAddingRecord(beginOffset);
	}

	public void AddStealItemAndEscape(int charId, Location location, int charId1, ulong itemKey)
	{
		int beginOffset = BeginAddingRecord(100);
		AppendCharacter(charId);
		AppendLocation(location);
		AppendCharacter(charId1);
		AppendItemKey(itemKey);
		EndAddingRecord(beginOffset);
	}

	public void AddScamItem(int charId, Location location, int charId1, ulong itemKey)
	{
		int beginOffset = BeginAddingRecord(101);
		AppendCharacter(charId);
		AppendLocation(location);
		AppendCharacter(charId1);
		AppendItemKey(itemKey);
		EndAddingRecord(beginOffset);
	}

	public void AddRobItem(int charId, Location location, int charId1, ulong itemKey)
	{
		int beginOffset = BeginAddingRecord(102);
		AppendCharacter(charId);
		AppendLocation(location);
		AppendCharacter(charId1);
		AppendItemKey(itemKey);
		EndAddingRecord(beginOffset);
	}

	public void AddStealLifeSkillButBeCaught(int charId, Location location, int charId1, sbyte itemType, short itemTemplateId, int value)
	{
		int beginOffset = BeginAddingRecord(103);
		AppendCharacter(charId);
		AppendLocation(location);
		AppendCharacter(charId1);
		AppendItem(itemType, itemTemplateId);
		AppendInteger(value);
		EndAddingRecord(beginOffset);
	}

	public void AddStealLifeSkillAndEscape(int charId, Location location, int charId1, sbyte itemType, short itemTemplateId, int value)
	{
		int beginOffset = BeginAddingRecord(104);
		AppendCharacter(charId);
		AppendLocation(location);
		AppendCharacter(charId1);
		AppendItem(itemType, itemTemplateId);
		AppendInteger(value);
		EndAddingRecord(beginOffset);
	}

	public void AddScamLifeSkill(int charId, Location location, int charId1, sbyte itemType, short itemTemplateId, int value)
	{
		int beginOffset = BeginAddingRecord(105);
		AppendCharacter(charId);
		AppendLocation(location);
		AppendCharacter(charId1);
		AppendItem(itemType, itemTemplateId);
		AppendInteger(value);
		EndAddingRecord(beginOffset);
	}

	public void AddStealCombatSkillButBeCaught(int charId, Location location, int charId1, sbyte itemType, short itemTemplateId, int value, int value1, int value2)
	{
		int beginOffset = BeginAddingRecord(106);
		AppendCharacter(charId);
		AppendLocation(location);
		AppendCharacter(charId1);
		AppendItem(itemType, itemTemplateId);
		AppendInteger(value);
		AppendInteger(value1);
		AppendInteger(value2);
		EndAddingRecord(beginOffset);
	}

	public void AddStealCombatSkillAndEscape(int charId, Location location, int charId1, sbyte itemType, short itemTemplateId, int value, int value1, int value2)
	{
		int beginOffset = BeginAddingRecord(107);
		AppendCharacter(charId);
		AppendLocation(location);
		AppendCharacter(charId1);
		AppendItem(itemType, itemTemplateId);
		AppendInteger(value);
		AppendInteger(value1);
		AppendInteger(value2);
		EndAddingRecord(beginOffset);
	}

	public void AddScamCombatSkill(int charId, Location location, int charId1, sbyte itemType, short itemTemplateId, int value, int value1, int value2)
	{
		int beginOffset = BeginAddingRecord(108);
		AppendCharacter(charId);
		AppendLocation(location);
		AppendCharacter(charId1);
		AppendItem(itemType, itemTemplateId);
		AppendInteger(value);
		AppendInteger(value1);
		AppendInteger(value2);
		EndAddingRecord(beginOffset);
	}

	public void AddAdviseExtendFavours(int charId, Location location, int charId1)
	{
		int beginOffset = BeginAddingRecord(109);
		AppendCharacter(charId);
		AppendLocation(location);
		AppendCharacter(charId1);
		EndAddingRecord(beginOffset);
	}

	public void AddAdviseWinPeopleSupport(int charId, Location location, int charId1)
	{
		int beginOffset = BeginAddingRecord(110);
		AppendCharacter(charId);
		AppendLocation(location);
		AppendCharacter(charId1);
		EndAddingRecord(beginOffset);
	}

	public void AddAdviseMerchantFavor(int charId, Location location, int charId1)
	{
		int beginOffset = BeginAddingRecord(111);
		AppendCharacter(charId);
		AppendLocation(location);
		AppendCharacter(charId1);
		EndAddingRecord(beginOffset);
	}

	public void AddAdviseTeaWine(int charId, Location location, int charId1, ulong itemKey, ulong itemKey1)
	{
		int beginOffset = BeginAddingRecord(112);
		AppendCharacter(charId);
		AppendLocation(location);
		AppendCharacter(charId1);
		AppendItemKey(itemKey);
		AppendItemKey(itemKey1);
		EndAddingRecord(beginOffset);
	}

	public void AddAdviseSales(int charId, Location location, int charId1, ulong itemKey, int value, int value1)
	{
		int beginOffset = BeginAddingRecord(113);
		AppendCharacter(charId);
		AppendLocation(location);
		AppendCharacter(charId1);
		AppendItemKey(itemKey);
		AppendInteger(value);
		AppendInteger(value1);
		EndAddingRecord(beginOffset);
	}

	public void AddAdviseHealInjury(int charId, Location location, int charId1, int value)
	{
		int beginOffset = BeginAddingRecord(114);
		AppendCharacter(charId);
		AppendLocation(location);
		AppendCharacter(charId1);
		AppendInteger(value);
		EndAddingRecord(beginOffset);
	}

	public void AddAdviseHealPoison(int charId, Location location, int charId1, int value)
	{
		int beginOffset = BeginAddingRecord(115);
		AppendCharacter(charId);
		AppendLocation(location);
		AppendCharacter(charId1);
		AppendInteger(value);
		EndAddingRecord(beginOffset);
	}

	public void AddAdviseRepairItem(int charId, Location location, int charId1, ulong itemKey, ulong itemKey1, sbyte resourceType, int value)
	{
		int beginOffset = BeginAddingRecord(116);
		AppendCharacter(charId);
		AppendLocation(location);
		AppendCharacter(charId1);
		AppendItemKey(itemKey);
		AppendItemKey(itemKey1);
		AppendResource(resourceType);
		AppendInteger(value);
		EndAddingRecord(beginOffset);
	}

	public void AddAdviseBarb(int charId, Location location, int charId1)
	{
		int beginOffset = BeginAddingRecord(117);
		AppendCharacter(charId);
		AppendLocation(location);
		AppendCharacter(charId1);
		EndAddingRecord(beginOffset);
	}

	public void AddAskForMoney(int charId, Location location, int charId1)
	{
		int beginOffset = BeginAddingRecord(118);
		AppendCharacter(charId);
		AppendLocation(location);
		AppendCharacter(charId1);
		EndAddingRecord(beginOffset);
	}

	public void AddWulinConferenceTaiwuAbsent()
	{
		int beginOffset = BeginAddingRecord(119);
		EndAddingRecord(beginOffset);
	}

	public void AddWulinConferenceAskForHelp(short settlementId, int charId)
	{
		int beginOffset = BeginAddingRecord(120);
		AppendSettlement(settlementId);
		AppendCharacter(charId);
		EndAddingRecord(beginOffset);
	}

	public void AddTaiwuVillageBeDestoryed()
	{
		int beginOffset = BeginAddingRecord(121);
		EndAddingRecord(beginOffset);
	}

	public void AddForeverLoverBePunished(int charId)
	{
		int beginOffset = BeginAddingRecord(122);
		AppendCharacter(charId);
		EndAddingRecord(beginOffset);
	}

	public void AddVillageWoodenManByMonv()
	{
		int beginOffset = BeginAddingRecord(123);
		EndAddingRecord(beginOffset);
	}

	public void AddVillageWoodenManByDayueYaochang()
	{
		int beginOffset = BeginAddingRecord(124);
		EndAddingRecord(beginOffset);
	}

	public void AddVillageWoodenManByJiuhan()
	{
		int beginOffset = BeginAddingRecord(125);
		EndAddingRecord(beginOffset);
	}

	public void AddVillageWoodenManByJinHuanger()
	{
		int beginOffset = BeginAddingRecord(126);
		EndAddingRecord(beginOffset);
	}

	public void AddVillageWoodenManByYiYihou()
	{
		int beginOffset = BeginAddingRecord(127);
		EndAddingRecord(beginOffset);
	}

	public void AddVillageWoodenManByWeiQi()
	{
		int beginOffset = BeginAddingRecord(128);
		EndAddingRecord(beginOffset);
	}

	public void AddVillageWoodenManByYixiang()
	{
		int beginOffset = BeginAddingRecord(129);
		EndAddingRecord(beginOffset);
	}

	public void AddVillageWoodenManByXuefeng()
	{
		int beginOffset = BeginAddingRecord(130);
		EndAddingRecord(beginOffset);
	}

	public void AddVillageWoodenManByShuFang()
	{
		int beginOffset = BeginAddingRecord(131);
		EndAddingRecord(beginOffset);
	}

	public void AddTaiwuNotAttendingWedding(int charId, int charId1)
	{
		int beginOffset = BeginAddingRecord(132);
		AppendCharacter(charId);
		AppendCharacter(charId1);
		EndAddingRecord(beginOffset);
	}

	public void AddTaiwuAlreadyMarried(int charId, int charId1)
	{
		int beginOffset = BeginAddingRecord(133);
		AppendCharacter(charId);
		AppendCharacter(charId1);
		EndAddingRecord(beginOffset);
	}

	public void AddChallengeForLegendaryBook(int charId, Location location, int charId1, ulong itemKey)
	{
		int beginOffset = BeginAddingRecord(134);
		AppendCharacter(charId);
		AppendLocation(location);
		AppendCharacter(charId1);
		AppendItemKey(itemKey);
		EndAddingRecord(beginOffset);
	}

	public void AddRequestLegendaryBook(int charId, Location location, int charId1, ulong itemKey)
	{
		int beginOffset = BeginAddingRecord(135);
		AppendCharacter(charId);
		AppendLocation(location);
		AppendCharacter(charId1);
		AppendItemKey(itemKey);
		EndAddingRecord(beginOffset);
	}

	public void AddExchangeLegendaryBookByMoney(int charId, Location location, int charId1, ulong itemKey)
	{
		int beginOffset = BeginAddingRecord(136);
		AppendCharacter(charId);
		AppendLocation(location);
		AppendCharacter(charId1);
		AppendItemKey(itemKey);
		EndAddingRecord(beginOffset);
	}

	public void AddExchangeLegendaryBookByAuthority(int charId, Location location, int charId1, ulong itemKey)
	{
		int beginOffset = BeginAddingRecord(137);
		AppendCharacter(charId);
		AppendLocation(location);
		AppendCharacter(charId1);
		AppendItemKey(itemKey);
		EndAddingRecord(beginOffset);
	}

	public void AddExchangeLegendaryBookByExperience(int charId, Location location, int charId1, ulong itemKey)
	{
		int beginOffset = BeginAddingRecord(138);
		AppendCharacter(charId);
		AppendLocation(location);
		AppendCharacter(charId1);
		AppendItemKey(itemKey);
		EndAddingRecord(beginOffset);
	}

	public void AddStealLegendaryBookAndEscape(int charId, Location location, int charId1, ulong itemKey, int value)
	{
		int beginOffset = BeginAddingRecord(139);
		AppendCharacter(charId);
		AppendLocation(location);
		AppendCharacter(charId1);
		AppendItemKey(itemKey);
		AppendInteger(value);
		EndAddingRecord(beginOffset);
	}

	public void AddStealLegendaryBookGotCaught(int charId, Location location, int charId1, ulong itemKey, int value)
	{
		int beginOffset = BeginAddingRecord(140);
		AppendCharacter(charId);
		AppendLocation(location);
		AppendCharacter(charId1);
		AppendItemKey(itemKey);
		AppendInteger(value);
		EndAddingRecord(beginOffset);
	}

	public void AddScamLegendaryBook(int charId, Location location, int charId1, ulong itemKey, int value)
	{
		int beginOffset = BeginAddingRecord(141);
		AppendCharacter(charId);
		AppendLocation(location);
		AppendCharacter(charId1);
		AppendItemKey(itemKey);
		AppendInteger(value);
		EndAddingRecord(beginOffset);
	}

	public void AddRobLegendaryBook(int charId, Location location, int charId1, ulong itemKey, int value)
	{
		int beginOffset = BeginAddingRecord(142);
		AppendCharacter(charId);
		AppendLocation(location);
		AppendCharacter(charId1);
		AppendItemKey(itemKey);
		AppendInteger(value);
		EndAddingRecord(beginOffset);
	}

	public void AddLegendaryBookShockedAttack(int charId, Location location, int charId1)
	{
		int beginOffset = BeginAddingRecord(143);
		AppendCharacter(charId);
		AppendLocation(location);
		AppendCharacter(charId1);
		EndAddingRecord(beginOffset);
	}

	public void AddLegendaryBookInsaneAttack(int charId, Location location, int charId1)
	{
		int beginOffset = BeginAddingRecord(144);
		AppendCharacter(charId);
		AppendLocation(location);
		AppendCharacter(charId1);
		EndAddingRecord(beginOffset);
	}

	public void AddLegendaryBookConsumedAttack(int charId, Location location, int charId1)
	{
		int beginOffset = BeginAddingRecord(145);
		AppendCharacter(charId);
		AppendLocation(location);
		AppendCharacter(charId1);
		EndAddingRecord(beginOffset);
	}

	public void AddSwordTombGetStronger(ulong itemKey)
	{
		int beginOffset = BeginAddingRecord(146);
		AppendItemKey(itemKey);
		EndAddingRecord(beginOffset);
	}

	public void AddSwordTombBackToNormal(ulong itemKey)
	{
		int beginOffset = BeginAddingRecord(147);
		AppendItemKey(itemKey);
		EndAddingRecord(beginOffset);
	}

	public void AddFightForNewLegendaryBook(Location location, ulong itemKey)
	{
		int beginOffset = BeginAddingRecord(148);
		AppendLocation(location);
		AppendItemKey(itemKey);
		EndAddingRecord(beginOffset);
	}

	public void AddFightForLegendaryBookAbandoned(int charId, Location location, ulong itemKey)
	{
		int beginOffset = BeginAddingRecord(149);
		AppendCharacter(charId);
		AppendLocation(location);
		AppendItemKey(itemKey);
		EndAddingRecord(beginOffset);
	}

	public void AddFightForLegendaryBookOwnerDie(int charId, Location location, ulong itemKey)
	{
		int beginOffset = BeginAddingRecord(150);
		AppendCharacter(charId);
		AppendLocation(location);
		AppendItemKey(itemKey);
		EndAddingRecord(beginOffset);
	}

	public void AddFightForLegendaryBookOwnerConsumed(int charId, Location location, ulong itemKey)
	{
		int beginOffset = BeginAddingRecord(151);
		AppendCharacter(charId);
		AppendLocation(location);
		AppendItemKey(itemKey);
		EndAddingRecord(beginOffset);
	}

	public void AddDateWithLoverEveryday(int charId, int charId1)
	{
		int beginOffset = BeginAddingRecord(152);
		AppendCharacter(charId);
		AppendCharacter(charId1);
		EndAddingRecord(beginOffset);
	}

	public void AddHappyBirthdayTaiwu(int charId, sbyte month)
	{
		int beginOffset = BeginAddingRecord(153);
		AppendCharacter(charId);
		AppendMonth(month);
		EndAddingRecord(beginOffset);
	}

	public void AddLoveAnniversary(int charId, int charId1, int value)
	{
		int beginOffset = BeginAddingRecord(154);
		AppendCharacter(charId);
		AppendCharacter(charId1);
		AppendInteger(value);
		EndAddingRecord(beginOffset);
	}

	public void AddNeglectedLover(int charId)
	{
		int beginOffset = BeginAddingRecord(155);
		AppendCharacter(charId);
		EndAddingRecord(beginOffset);
	}

	public void AddLoverBecomeJealous(int charId, int charId1)
	{
		int beginOffset = BeginAddingRecord(156);
		AppendCharacter(charId);
		AppendCharacter(charId1);
		EndAddingRecord(beginOffset);
	}

	public void AddLoversBecomeJealousAndViolent(int charId, int charId1, int charId2, int charId3)
	{
		int beginOffset = BeginAddingRecord(157);
		AppendCharacter(charId);
		AppendCharacter(charId1);
		AppendCharacter(charId2);
		AppendCharacter(charId3);
		EndAddingRecord(beginOffset);
	}

	public void AddPregnancyWithLover(int charId, int charId1)
	{
		int beginOffset = BeginAddingRecord(158);
		AppendCharacter(charId);
		AppendCharacter(charId1);
		EndAddingRecord(beginOffset);
	}

	public void AddBeggerSkill2TargetUnavailable(int charId)
	{
		int beginOffset = BeginAddingRecord(159);
		AppendCharacter(charId);
		EndAddingRecord(beginOffset);
	}

	public void AddBeggarSkill2TargetBrought(int charId, Location location)
	{
		int beginOffset = BeginAddingRecord(160);
		AppendCharacter(charId);
		AppendLocation(location);
		EndAddingRecord(beginOffset);
	}

	public void AddBeggarSkill2TargetDeadAndMissing(int charId)
	{
		int beginOffset = BeginAddingRecord(161);
		AppendCharacter(charId);
		EndAddingRecord(beginOffset);
	}

	public void AddBeggarSkill2TargetDead(int charId, Location location)
	{
		int beginOffset = BeginAddingRecord(162);
		AppendCharacter(charId);
		AppendLocation(location);
		EndAddingRecord(beginOffset);
	}

	public void AddBeggarSkill2TargetNoneExistent(string text)
	{
		int beginOffset = BeginAddingRecord(163);
		AppendText(text);
		EndAddingRecord(beginOffset);
	}

	public void AddTaiwuTribulation(int charId, Location location)
	{
		int beginOffset = BeginAddingRecord(164);
		AppendCharacter(charId);
		AppendLocation(location);
		EndAddingRecord(beginOffset);
	}

	public void AddTaiwuComingSuccess(int charId, int charId1, Location location, int charId2)
	{
		int beginOffset = BeginAddingRecord(165);
		AppendCharacter(charId);
		AppendCharacter(charId1);
		AppendLocation(location);
		AppendCharacter(charId2);
		EndAddingRecord(beginOffset);
	}

	public void AddTaiwuComingDefeated(int charId, int charId1)
	{
		int beginOffset = BeginAddingRecord(166);
		AppendCharacter(charId);
		AppendCharacter(charId1);
		EndAddingRecord(beginOffset);
	}

	public void AddTaiwuFreeAndunFettered(int charId)
	{
		int beginOffset = BeginAddingRecord(167);
		AppendCharacter(charId);
		EndAddingRecord(beginOffset);
	}

	public void AddSectMainStoryKongsangTargetFound()
	{
		int beginOffset = BeginAddingRecord(168);
		EndAddingRecord(beginOffset);
	}

	public void AddSectMainStoryKongsangAdventure()
	{
		int beginOffset = BeginAddingRecord(169);
		EndAddingRecord(beginOffset);
	}

	public void AddSectMainStoryKongsangProsperous()
	{
		int beginOffset = BeginAddingRecord(170);
		EndAddingRecord(beginOffset);
	}

	public void AddSectMainStoryKongsangFailing()
	{
		int beginOffset = BeginAddingRecord(171);
		EndAddingRecord(beginOffset);
	}

	public void AddSectMainStoryXuehouGraveDigging()
	{
		int beginOffset = BeginAddingRecord(172);
		EndAddingRecord(beginOffset);
	}

	public void AddSectMainStoryXuehouGraveDiggingNormal()
	{
		int beginOffset = BeginAddingRecord(173);
		EndAddingRecord(beginOffset);
	}

	public void AddSectMainStoryXuehouStrangeDeath()
	{
		int beginOffset = BeginAddingRecord(174);
		EndAddingRecord(beginOffset);
	}

	public void AddSectMainStoryXuehouOldManAppears()
	{
		int beginOffset = BeginAddingRecord(175);
		EndAddingRecord(beginOffset);
	}

	public void AddSectMainStoryXuehouOldManReturns()
	{
		int beginOffset = BeginAddingRecord(176);
		EndAddingRecord(beginOffset);
	}

	public void AddSectMainStoryXuehouOnBloodBlock()
	{
		int beginOffset = BeginAddingRecord(177);
		EndAddingRecord(beginOffset);
	}

	public void AddSectMainStoryXuehouOldManAttacks()
	{
		int beginOffset = BeginAddingRecord(178);
		EndAddingRecord(beginOffset);
	}

	public void AddSectMainStoryXuehouHarmoniousTaiwu()
	{
		int beginOffset = BeginAddingRecord(179);
		EndAddingRecord(beginOffset);
	}

	public void AddSectMainStoryXuehouFeedJixi()
	{
		int beginOffset = BeginAddingRecord(180);
		EndAddingRecord(beginOffset);
	}

	public void AddSectMainStoryXuehouMythInVillage()
	{
		int beginOffset = BeginAddingRecord(181);
		EndAddingRecord(beginOffset);
	}

	public void AddSectMainStoryXuehouProtectJixi()
	{
		int beginOffset = BeginAddingRecord(182);
		EndAddingRecord(beginOffset);
	}

	public void AddSectMainStoryXuehouJixiAskForFood()
	{
		int beginOffset = BeginAddingRecord(183);
		EndAddingRecord(beginOffset);
	}

	public void AddSectMainStoryXuehouJixiFeedChicken()
	{
		int beginOffset = BeginAddingRecord(184);
		EndAddingRecord(beginOffset);
	}

	public void AddSectMainStoryXuehouJixiKills()
	{
		int beginOffset = BeginAddingRecord(185);
		EndAddingRecord(beginOffset);
	}

	public void AddSectMainStoryXuehouVillageWork()
	{
		int beginOffset = BeginAddingRecord(186);
		EndAddingRecord(beginOffset);
	}

	public void AddSectMainStoryXuehouFinale()
	{
		int beginOffset = BeginAddingRecord(187);
		EndAddingRecord(beginOffset);
	}

	public void AddSectMainStoryXuehouProsperous()
	{
		int beginOffset = BeginAddingRecord(188);
		EndAddingRecord(beginOffset);
	}

	public void AddSectMainStoryXuehouFailing()
	{
		int beginOffset = BeginAddingRecord(189);
		EndAddingRecord(beginOffset);
	}

	public void AddSectMainStoryShaolinTowerFalling()
	{
		int beginOffset = BeginAddingRecord(190);
		EndAddingRecord(beginOffset);
	}

	public void AddSectMainStoryShaolinTwoMonksTalk()
	{
		int beginOffset = BeginAddingRecord(191);
		EndAddingRecord(beginOffset);
	}

	public void AddSectMainStoryShaolinDreamFirst()
	{
		int beginOffset = BeginAddingRecord(192);
		EndAddingRecord(beginOffset);
	}

	public void AddSectMainStoryShaolinLearning()
	{
		int beginOffset = BeginAddingRecord(193);
		EndAddingRecord(beginOffset);
	}

	public void AddSectMainStoryShaolinNotEnough()
	{
		int beginOffset = BeginAddingRecord(194);
		EndAddingRecord(beginOffset);
	}

	public void AddSectMainStoryShaolinChallenge()
	{
		int beginOffset = BeginAddingRecord(195);
		EndAddingRecord(beginOffset);
	}

	public void AddSectMainStoryShaolinEndChallenge()
	{
		int beginOffset = BeginAddingRecord(196);
		EndAddingRecord(beginOffset);
	}

	public void AddSectMainStoryShaolinNeverLearnChallenge()
	{
		int beginOffset = BeginAddingRecord(197);
		EndAddingRecord(beginOffset);
	}

	public void AddSectMainStoryShaolinProsperous()
	{
		int beginOffset = BeginAddingRecord(198);
		EndAddingRecord(beginOffset);
	}

	public void AddSectMainStoryShaolinFailing()
	{
		int beginOffset = BeginAddingRecord(199);
		EndAddingRecord(beginOffset);
	}

	public void AddSectMainStoryXuannvPrologue()
	{
		int beginOffset = BeginAddingRecord(200);
		EndAddingRecord(beginOffset);
	}

	public void AddSectMainStoryXuannvWithSister()
	{
		int beginOffset = BeginAddingRecord(201);
		EndAddingRecord(beginOffset);
	}

	public void AddSectMainStoryXuannvReincarnationDeath(int charId)
	{
		int beginOffset = BeginAddingRecord(202);
		AppendCharacter(charId);
		EndAddingRecord(beginOffset);
	}

	public void AddSectMainStoryXuannvProsperous()
	{
		int beginOffset = BeginAddingRecord(203);
		EndAddingRecord(beginOffset);
	}

	public void AddSectMainStoryXuannvFailing()
	{
		int beginOffset = BeginAddingRecord(204);
		EndAddingRecord(beginOffset);
	}

	public void AddSectMainStoryWudangChat()
	{
		int beginOffset = BeginAddingRecord(205);
		EndAddingRecord(beginOffset);
	}

	public void AddSectMainStoryWudangRequest(int charId, Location location)
	{
		int beginOffset = BeginAddingRecord(206);
		AppendCharacter(charId);
		AppendLocation(location);
		EndAddingRecord(beginOffset);
	}

	public void AddSectMainStoryWudangSeekSite(Location location, int charId)
	{
		int beginOffset = BeginAddingRecord(207);
		AppendLocation(location);
		AppendCharacter(charId);
		EndAddingRecord(beginOffset);
	}

	public void AddSectMainStoryWudangProsperous()
	{
		int beginOffset = BeginAddingRecord(208);
		EndAddingRecord(beginOffset);
	}

	public void AddSectMainStoryWudangFailing()
	{
		int beginOffset = BeginAddingRecord(209);
		EndAddingRecord(beginOffset);
	}

	public void AddSectMainStoryYuanshanInfectedCharacterAttack(Location location, int charId)
	{
		int beginOffset = BeginAddingRecord(210);
		AppendLocation(location);
		AppendCharacter(charId);
		EndAddingRecord(beginOffset);
	}

	public void AddSectMainStoryYuanshanDisciplesInfected()
	{
		int beginOffset = BeginAddingRecord(211);
		EndAddingRecord(beginOffset);
	}

	public void AddSectMainStoryYuanshanLastMonsterAppear()
	{
		int beginOffset = BeginAddingRecord(212);
		EndAddingRecord(beginOffset);
	}

	public void AddSectMainStoryYuanshanProsperous()
	{
		int beginOffset = BeginAddingRecord(213);
		EndAddingRecord(beginOffset);
	}

	public void AddSectMainStoryShixiangEnemyAttack()
	{
		int beginOffset = BeginAddingRecord(214);
		EndAddingRecord(beginOffset);
	}

	public void AddSectMainStoryShixiangLetterFrom(Location location)
	{
		int beginOffset = BeginAddingRecord(215);
		AppendLocation(location);
		EndAddingRecord(beginOffset);
	}

	public void AddSectMainStoryShixiangNotLetter(Location location)
	{
		int beginOffset = BeginAddingRecord(216);
		AppendLocation(location);
		EndAddingRecord(beginOffset);
	}

	public void AddSectMainStoryShixiangDuel(int charId, Location location, int charId1)
	{
		int beginOffset = BeginAddingRecord(217);
		AppendCharacter(charId);
		AppendLocation(location);
		AppendCharacter(charId1);
		EndAddingRecord(beginOffset);
	}

	public void AddSectMainStoryShixiangFailing(Location location)
	{
		int beginOffset = BeginAddingRecord(218);
		AppendLocation(location);
		EndAddingRecord(beginOffset);
	}

	public void AddSectMainStoryJingangPeopleSuffering(int charId, Location location)
	{
		int beginOffset = BeginAddingRecord(219);
		AppendCharacter(charId);
		AppendLocation(location);
		EndAddingRecord(beginOffset);
	}

	public void AddSectMainStoryJingangAttack()
	{
		int beginOffset = BeginAddingRecord(220);
		EndAddingRecord(beginOffset);
	}

	public void AddSectMainStoryJingangMonkMurdered()
	{
		int beginOffset = BeginAddingRecord(221);
		EndAddingRecord(beginOffset);
	}

	public void AddSectMainStoryJingangExorcism(int charId)
	{
		int beginOffset = BeginAddingRecord(222);
		AppendCharacter(charId);
		EndAddingRecord(beginOffset);
	}

	public void AddSectMainStoryJingangGhostAppears()
	{
		int beginOffset = BeginAddingRecord(223);
		EndAddingRecord(beginOffset);
	}

	public void AddSectMainStoryJingangHearsay()
	{
		int beginOffset = BeginAddingRecord(224);
		EndAddingRecord(beginOffset);
	}

	public void AddSectMainStoryJingangProsperous()
	{
		int beginOffset = BeginAddingRecord(225);
		EndAddingRecord(beginOffset);
	}

	public void AddSectMainStoryJingangFailing()
	{
		int beginOffset = BeginAddingRecord(226);
		EndAddingRecord(beginOffset);
	}

	public void AddSectMainStoryWuxianPoisonousWug(int charId)
	{
		int beginOffset = BeginAddingRecord(227);
		AppendCharacter(charId);
		EndAddingRecord(beginOffset);
	}

	public void AddSectMainStoryWuxianProsperous()
	{
		int beginOffset = BeginAddingRecord(228);
		EndAddingRecord(beginOffset);
	}

	public void AddSectMainStoryWuxianFailing0()
	{
		int beginOffset = BeginAddingRecord(229);
		EndAddingRecord(beginOffset);
	}

	public void AddSectMainStoryWuxianFailing1()
	{
		int beginOffset = BeginAddingRecord(230);
		EndAddingRecord(beginOffset);
	}

	public void AddSectMainStoryWuxianStrangeThings()
	{
		int beginOffset = BeginAddingRecord(231);
		EndAddingRecord(beginOffset);
	}

	public void AddSectMainStoryWuxianPoison()
	{
		int beginOffset = BeginAddingRecord(232);
		EndAddingRecord(beginOffset);
	}

	public void AddSectMainStoryWuxianAssault(int charId, Location location)
	{
		int beginOffset = BeginAddingRecord(233);
		AppendCharacter(charId);
		AppendLocation(location);
		EndAddingRecord(beginOffset);
	}

	public void AddSectMainStoryEmeiProsperous()
	{
		int beginOffset = BeginAddingRecord(234);
		EndAddingRecord(beginOffset);
	}

	public void AddSectMainStoryEmeiFailing()
	{
		int beginOffset = BeginAddingRecord(235);
		EndAddingRecord(beginOffset);
	}

	public void AddSectMainStoryJieqingAssassinationPlot()
	{
		int beginOffset = BeginAddingRecord(236);
		EndAddingRecord(beginOffset);
	}

	public void AddSectMainStoryJieqingAssassinationGeneral()
	{
		int beginOffset = BeginAddingRecord(237);
		EndAddingRecord(beginOffset);
	}

	public void AddSectMainStoryJieqingProsperous()
	{
		int beginOffset = BeginAddingRecord(238);
		EndAddingRecord(beginOffset);
	}

	public void AddSectMainStoryJieqingFailing()
	{
		int beginOffset = BeginAddingRecord(239);
		EndAddingRecord(beginOffset);
	}

	public void AddSuicideToken()
	{
		int beginOffset = BeginAddingRecord(240);
		EndAddingRecord(beginOffset);
	}

	public void AddSectMainStoryXuehouEmptyGrave()
	{
		int beginOffset = BeginAddingRecord(241);
		EndAddingRecord(beginOffset);
	}

	public void AddSectMainStoryXuehouLookingForTaiwu(int charId)
	{
		int beginOffset = BeginAddingRecord(242);
		AppendCharacter(charId);
		EndAddingRecord(beginOffset);
	}

	public void AddSectMainStoryXuehouComing()
	{
		int beginOffset = BeginAddingRecord(243);
		EndAddingRecord(beginOffset);
	}

	public void AddSectMainStoryRanshanPaperCraneFromYufuFaction(int charId, Location location)
	{
		int beginOffset = BeginAddingRecord(244);
		AppendCharacter(charId);
		AppendLocation(location);
		EndAddingRecord(beginOffset);
	}

	public void AddSectMainStoryRanshanPaperCraneFromShenjianFaction(int charId, Location location)
	{
		int beginOffset = BeginAddingRecord(245);
		AppendCharacter(charId);
		AppendLocation(location);
		EndAddingRecord(beginOffset);
	}

	public void AddSectMainStoryRanshanPaperCraneFromYinyangFaction(int charId, Location location)
	{
		int beginOffset = BeginAddingRecord(246);
		AppendCharacter(charId);
		AppendLocation(location);
		EndAddingRecord(beginOffset);
	}

	public void AddSectMainStoryRanshanThreeApprentice()
	{
		int beginOffset = BeginAddingRecord(247);
		EndAddingRecord(beginOffset);
	}

	public void AddSectMainStoryRanshanHuajuStory()
	{
		int beginOffset = BeginAddingRecord(248);
		EndAddingRecord(beginOffset);
	}

	public void AddSectMainStoryRanshanXuanzhiStory()
	{
		int beginOffset = BeginAddingRecord(249);
		EndAddingRecord(beginOffset);
	}

	public void AddSectMainStoryRanshanYingjiaoStory()
	{
		int beginOffset = BeginAddingRecord(250);
		EndAddingRecord(beginOffset);
	}

	public void AddSectMainStoryRanshanThreeApprenticeRequest()
	{
		int beginOffset = BeginAddingRecord(251);
		EndAddingRecord(beginOffset);
	}

	public void AddSectMainStoryRanshanThreeApprenticeReturn()
	{
		int beginOffset = BeginAddingRecord(252);
		EndAddingRecord(beginOffset);
	}

	public void AddSectMainStoryRanshanThreeFactionCompetetion()
	{
		int beginOffset = BeginAddingRecord(253);
		EndAddingRecord(beginOffset);
	}

	public void AddSectMainStoryRanshanProsperous()
	{
		int beginOffset = BeginAddingRecord(254);
		EndAddingRecord(beginOffset);
	}

	public void AddSectMainStoryRanshanFailing()
	{
		int beginOffset = BeginAddingRecord(255);
		EndAddingRecord(beginOffset);
	}

	public void AddSectMainStoryShaolinDreamOfReadingSutra()
	{
		int beginOffset = BeginAddingRecord(256);
		EndAddingRecord(beginOffset);
	}

	public void AddSectMainStoryShaolinDreamOfNewTaiwu()
	{
		int beginOffset = BeginAddingRecord(257);
		EndAddingRecord(beginOffset);
	}

	public void AddSectMainStoryShaolinEnlightenment()
	{
		int beginOffset = BeginAddingRecord(258);
		EndAddingRecord(beginOffset);
	}

	public void AddSectMainStoryShaolinNotEnoughCommon()
	{
		int beginOffset = BeginAddingRecord(259);
		EndAddingRecord(beginOffset);
	}

	public void AddSectMainStoryShixiangRequestBook(int charId, Location location, ulong itemKey, int charId1)
	{
		int beginOffset = BeginAddingRecord(260);
		AppendCharacter(charId);
		AppendLocation(location);
		AppendItemKey(itemKey);
		AppendCharacter(charId1);
		EndAddingRecord(beginOffset);
	}

	public void AddSectMainStoryShixiangRequestLifeSkill(int charId, Location location, ulong itemKey, int charId1)
	{
		int beginOffset = BeginAddingRecord(261);
		AppendCharacter(charId);
		AppendLocation(location);
		AppendItemKey(itemKey);
		AppendCharacter(charId1);
		EndAddingRecord(beginOffset);
	}

	public void AddSectMainStoryShixiangGoodNews()
	{
		int beginOffset = BeginAddingRecord(262);
		EndAddingRecord(beginOffset);
	}

	public void AddSectMainStoryShixiangProsperous(Location location)
	{
		int beginOffset = BeginAddingRecord(263);
		AppendLocation(location);
		EndAddingRecord(beginOffset);
	}

	public void AddSectMainStoryShaolinChallengeCommon()
	{
		int beginOffset = BeginAddingRecord(264);
		EndAddingRecord(beginOffset);
	}

	public void AddSectMainStoryShaolinEndChallengeCommon()
	{
		int beginOffset = BeginAddingRecord(265);
		EndAddingRecord(beginOffset);
	}

	public void AddSectMainStoryShaolinNeverLearnChallengeCommon()
	{
		int beginOffset = BeginAddingRecord(266);
		EndAddingRecord(beginOffset);
	}

	public void AddSectMainStoryShixiangLetterFrom2(int charId)
	{
		int beginOffset = BeginAddingRecord(267);
		AppendCharacter(charId);
		EndAddingRecord(beginOffset);
	}

	public void AddSectMainStoryShixiangGoodNews2()
	{
		int beginOffset = BeginAddingRecord(268);
		EndAddingRecord(beginOffset);
	}

	public void AddSectMainStoryShixiangEnemyAttack2()
	{
		int beginOffset = BeginAddingRecord(269);
		EndAddingRecord(beginOffset);
	}

	public void AddSectMainStoryShixiangStrange()
	{
		int beginOffset = BeginAddingRecord(270);
		EndAddingRecord(beginOffset);
	}

	public void AddSectMainStoryShixiangEnemyAttack3()
	{
		int beginOffset = BeginAddingRecord(271);
		EndAddingRecord(beginOffset);
	}

	public void AddSectMainStoryWudangProtectHeavenlyTree()
	{
		int beginOffset = BeginAddingRecord(272);
		EndAddingRecord(beginOffset);
	}

	public void AddSectMainStoryWudangHeavenlyTreeDestroyed(Location location)
	{
		int beginOffset = BeginAddingRecord(273);
		AppendLocation(location);
		EndAddingRecord(beginOffset);
	}

	public void AddSectMainStoryWudangGiftsReceived(int charId, Location location)
	{
		int beginOffset = BeginAddingRecord(274);
		AppendCharacter(charId);
		AppendLocation(location);
		EndAddingRecord(beginOffset);
	}

	public void AddSectMainStoryWudangMeetingImmortal(int charId, Location location, int charId1, Location location1)
	{
		int beginOffset = BeginAddingRecord(275);
		AppendCharacter(charId);
		AppendLocation(location);
		AppendCharacter(charId1);
		AppendLocation(location1);
		EndAddingRecord(beginOffset);
	}

	public void AddSectMainStoryWudangGuardHeavenlyTree(int charId, Location location)
	{
		int beginOffset = BeginAddingRecord(276);
		AppendCharacter(charId);
		AppendLocation(location);
		EndAddingRecord(beginOffset);
	}

	public void AddSectMainStoryWudangGiftsReceived2(int charId, Location location)
	{
		int beginOffset = BeginAddingRecord(277);
		AppendCharacter(charId);
		AppendLocation(location);
		EndAddingRecord(beginOffset);
	}

	public void AddSectMainStoryXuannvLetter(int charId)
	{
		int beginOffset = BeginAddingRecord(278);
		AppendCharacter(charId);
		EndAddingRecord(beginOffset);
	}

	public void AddSectMainStoryXuannvHealing(int charId)
	{
		int beginOffset = BeginAddingRecord(279);
		AppendCharacter(charId);
		EndAddingRecord(beginOffset);
	}

	public void AddSectMainStoryXuannvDeadMessage(int charId)
	{
		int beginOffset = BeginAddingRecord(280);
		AppendCharacter(charId);
		EndAddingRecord(beginOffset);
	}

	public void AddSectMainStoryXuannvMirrorDream()
	{
		int beginOffset = BeginAddingRecord(281);
		EndAddingRecord(beginOffset);
	}

	public void AddSectMainStoryXuannvReincarnationDeath2(int charId)
	{
		int beginOffset = BeginAddingRecord(282);
		AppendCharacter(charId);
		EndAddingRecord(beginOffset);
	}

	public void AddSectMainStoryXuannvStrangeMoan()
	{
		int beginOffset = BeginAddingRecord(283);
		EndAddingRecord(beginOffset);
	}

	public void AddSectMainStoryXuannvFirstTrack()
	{
		int beginOffset = BeginAddingRecord(284);
		EndAddingRecord(beginOffset);
	}

	public void AddSectMainStoryXuannvMeetJuner(int charId)
	{
		int beginOffset = BeginAddingRecord(285);
		AppendCharacter(charId);
		EndAddingRecord(beginOffset);
	}

	public void AddSectMainStoryWudangHeavenlyTreeDestroyed2(Location location)
	{
		int beginOffset = BeginAddingRecord(286);
		AppendLocation(location);
		EndAddingRecord(beginOffset);
	}

	public void AddMirrorCreatedImpostureXiangshuInfected(int charId, Location location)
	{
		int beginOffset = BeginAddingRecord(287);
		AppendCharacter(charId);
		AppendLocation(location);
		EndAddingRecord(beginOffset);
	}

	public void AddSectMainStoryWudangProtectHeavenlyTree2()
	{
		int beginOffset = BeginAddingRecord(288);
		EndAddingRecord(beginOffset);
	}

	public void AddCrossArchiveReunionWithAcquaintance(int charId, Location location)
	{
		int beginOffset = BeginAddingRecord(289);
		AppendCharacter(charId);
		AppendLocation(location);
		EndAddingRecord(beginOffset);
	}

	public void AddTeachCombatSkill(int charId, Location location, int charId1, short combatSkillTemplateId)
	{
		int beginOffset = BeginAddingRecord(290);
		AppendCharacter(charId);
		AppendLocation(location);
		AppendCharacter(charId1);
		AppendCombatSkill(combatSkillTemplateId);
		EndAddingRecord(beginOffset);
	}

	public void AddPregnant(int charId, Location location)
	{
		int beginOffset = BeginAddingRecord(291);
		AppendCharacter(charId);
		AppendLocation(location);
		EndAddingRecord(beginOffset);
	}

	public void AddTamingCarriers(short charTemplateId, Location location, ulong itemKey, Location location1)
	{
		int beginOffset = BeginAddingRecord(292);
		AppendCharacterTemplate(charTemplateId);
		AppendLocation(location);
		AppendItemKey(itemKey);
		AppendLocation(location1);
		EndAddingRecord(beginOffset);
	}

	public void AddFiveLoongLetterFromTaiwuVillage()
	{
		int beginOffset = BeginAddingRecord(293);
		EndAddingRecord(beginOffset);
	}

	public void AddJiaoGrowold(Location location, int jiaoLoongId)
	{
		int beginOffset = BeginAddingRecord(294);
		AppendLocation(location);
		AppendJiaoLoong(jiaoLoongId);
		EndAddingRecord(beginOffset);
	}

	public void AddDLCLoongRidingEffectQiuniu(int charId, int jiaoLoongId)
	{
		int beginOffset = BeginAddingRecord(295);
		AppendCharacter(charId);
		AppendJiaoLoong(jiaoLoongId);
		EndAddingRecord(beginOffset);
	}

	public void AddDLCLoongRidingEffectYazi(int charId, int jiaoLoongId, int charId1)
	{
		int beginOffset = BeginAddingRecord(296);
		AppendCharacter(charId);
		AppendJiaoLoong(jiaoLoongId);
		AppendCharacter(charId1);
		EndAddingRecord(beginOffset);
	}

	public void AddDLCLoongRidingEffectChaofeng(int charId, int jiaoLoongId, int charId1)
	{
		int beginOffset = BeginAddingRecord(297);
		AppendCharacter(charId);
		AppendJiaoLoong(jiaoLoongId);
		AppendCharacter(charId1);
		EndAddingRecord(beginOffset);
	}

	public void AddDLCLoongRidingEffectPulao(int charId, int jiaoLoongId, short colorId, short partId)
	{
		int beginOffset = BeginAddingRecord(298);
		AppendCharacter(charId);
		AppendJiaoLoong(jiaoLoongId);
		AppendCricket(colorId, partId);
		EndAddingRecord(beginOffset);
	}

	public void AddDLCLoongRidingEffectSuanni(int charId, int jiaoLoongId, sbyte itemType, short itemTemplateId, int value)
	{
		int beginOffset = BeginAddingRecord(299);
		AppendCharacter(charId);
		AppendJiaoLoong(jiaoLoongId);
		AppendItem(itemType, itemTemplateId);
		AppendInteger(value);
		EndAddingRecord(beginOffset);
	}

	public void AddDLCLoongRidingEffectBaxia(int charId, int jiaoLoongId)
	{
		int beginOffset = BeginAddingRecord(300);
		AppendCharacter(charId);
		AppendJiaoLoong(jiaoLoongId);
		EndAddingRecord(beginOffset);
	}

	public void AddDLCLoongRidingEffectBian(int charId, int jiaoLoongId)
	{
		int beginOffset = BeginAddingRecord(301);
		AppendCharacter(charId);
		AppendJiaoLoong(jiaoLoongId);
		EndAddingRecord(beginOffset);
	}

	public void AddDLCLoongRidingEffectFuxi(int charId, int jiaoLoongId, sbyte itemType, short itemTemplateId, int value)
	{
		int beginOffset = BeginAddingRecord(302);
		AppendCharacter(charId);
		AppendJiaoLoong(jiaoLoongId);
		AppendItem(itemType, itemTemplateId);
		AppendInteger(value);
		EndAddingRecord(beginOffset);
	}

	public void AddDLCLoongRidingEffectChiwen(int charId, int jiaoLoongId)
	{
		int beginOffset = BeginAddingRecord(303);
		AppendCharacter(charId);
		AppendJiaoLoong(jiaoLoongId);
		EndAddingRecord(beginOffset);
	}

	public void AddMinionLoongAttack(int charId, Location location, short charTemplateId)
	{
		int beginOffset = BeginAddingRecord(304);
		AppendCharacter(charId);
		AppendLocation(location);
		AppendCharacterTemplate(charTemplateId);
		EndAddingRecord(beginOffset);
	}

	public void AddDLCLoongJiaoGrowUp(Location location, int jiaoLoongId)
	{
		int beginOffset = BeginAddingRecord(305);
		AppendLocation(location);
		AppendJiaoLoong(jiaoLoongId);
		EndAddingRecord(beginOffset);
	}

	public void AddSectMainStoryWuxianGiftsReceived(int charId, Location location)
	{
		int beginOffset = BeginAddingRecord(306);
		AppendCharacter(charId);
		AppendLocation(location);
		EndAddingRecord(beginOffset);
	}

	public void AddSectMainStoryJingangVisitorsArrive()
	{
		int beginOffset = BeginAddingRecord(307);
		EndAddingRecord(beginOffset);
	}

	public void AddSectMainStoryJingangLettersFromJingang(int charId)
	{
		int beginOffset = BeginAddingRecord(308);
		AppendCharacter(charId);
		EndAddingRecord(beginOffset);
	}

	public void AddSectMainStoryJingangPiety(int charId)
	{
		int beginOffset = BeginAddingRecord(309);
		AppendCharacter(charId);
		EndAddingRecord(beginOffset);
	}

	public void AddSectMainStoryJingangSutraSecrets()
	{
		int beginOffset = BeginAddingRecord(310);
		EndAddingRecord(beginOffset);
	}

	public void AddSectMainStoryJingangRitualsInDream()
	{
		int beginOffset = BeginAddingRecord(311);
		EndAddingRecord(beginOffset);
	}

	public void AddSectMainStoryJingangSutraDisappears(int charId)
	{
		int beginOffset = BeginAddingRecord(312);
		AppendCharacter(charId);
		EndAddingRecord(beginOffset);
	}

	public void AddSectMainStoryJingangResidentsSufferingContinues()
	{
		int beginOffset = BeginAddingRecord(313);
		EndAddingRecord(beginOffset);
	}

	public void AddSectMainStoryJingangReincarnation(int charId)
	{
		int beginOffset = BeginAddingRecord(314);
		AppendCharacter(charId);
		EndAddingRecord(beginOffset);
	}

	public void AddSectMainStoryJingangGhostVanishes()
	{
		int beginOffset = BeginAddingRecord(315);
		EndAddingRecord(beginOffset);
	}

	public void AddSectMainStoryWuxianMiaoWoman(Location location)
	{
		int beginOffset = BeginAddingRecord(316);
		AppendLocation(location);
		EndAddingRecord(beginOffset);
	}

	public void AddSectMainStoryRanshanDragonGate()
	{
		int beginOffset = BeginAddingRecord(317);
		EndAddingRecord(beginOffset);
	}

	public void AddSectMainStoryRanshanMessage(int charId)
	{
		int beginOffset = BeginAddingRecord(318);
		AppendCharacter(charId);
		EndAddingRecord(beginOffset);
	}

	public void AddSectMainStoryRanshanAfterQinglang(int charId, Location location)
	{
		int beginOffset = BeginAddingRecord(319);
		AppendCharacter(charId);
		AppendLocation(location);
		EndAddingRecord(beginOffset);
	}

	public void AddSectMainStoryRanshanSanshiLeave(int charId)
	{
		int beginOffset = BeginAddingRecord(320);
		AppendCharacter(charId);
		EndAddingRecord(beginOffset);
	}

	public void AddSectMainStoryBaihuaEndenmic()
	{
		int beginOffset = BeginAddingRecord(321);
		EndAddingRecord(beginOffset);
	}

	public void AddSectMainStoryBaihuaDreamAboutPastFirst(int charId)
	{
		int beginOffset = BeginAddingRecord(322);
		AppendCharacter(charId);
		EndAddingRecord(beginOffset);
	}

	public void AddSectMainStoryBaihuaDreamAboutPastLast(int charId)
	{
		int beginOffset = BeginAddingRecord(323);
		AppendCharacter(charId);
		EndAddingRecord(beginOffset);
	}

	public void AddSectMainStoryBaihuaMelanoArrived(int charId)
	{
		int beginOffset = BeginAddingRecord(324);
		AppendCharacter(charId);
		EndAddingRecord(beginOffset);
	}

	public void AddSectMainStoryBaihuaProsperous()
	{
		int beginOffset = BeginAddingRecord(325);
		EndAddingRecord(beginOffset);
	}

	public void AddSectMainStoryBaihuaFailing()
	{
		int beginOffset = BeginAddingRecord(326);
		EndAddingRecord(beginOffset);
	}

	public void AddSectMainStoryBaihuaLeukoKills(Location location)
	{
		int beginOffset = BeginAddingRecord(327);
		AppendLocation(location);
		EndAddingRecord(beginOffset);
	}

	public void AddMerchantVisit()
	{
		int beginOffset = BeginAddingRecord(328);
		EndAddingRecord(beginOffset);
	}

	public void AddToRepayKindness(Location location)
	{
		int beginOffset = BeginAddingRecord(329);
		AppendLocation(location);
		EndAddingRecord(beginOffset);
	}

	public void AddSectMainStoryBaihuaAmbushLeuko()
	{
		int beginOffset = BeginAddingRecord(330);
		EndAddingRecord(beginOffset);
	}

	public void AddSectMainStoryBaihuaMelanoKills(Location location)
	{
		int beginOffset = BeginAddingRecord(331);
		AppendLocation(location);
		EndAddingRecord(beginOffset);
	}

	public void AddSectMainStoryBaihuaAmbushMelano()
	{
		int beginOffset = BeginAddingRecord(332);
		EndAddingRecord(beginOffset);
	}

	public void AddSectMainStoryBaihuaLeukoAssistsMelano()
	{
		int beginOffset = BeginAddingRecord(333);
		EndAddingRecord(beginOffset);
	}

	public void AddSectMainStoryBaihuaMelanoAssistsLeuko()
	{
		int beginOffset = BeginAddingRecord(334);
		EndAddingRecord(beginOffset);
	}

	public void AddSectMainStoryBaihuaManicAttack(int charId, Location location)
	{
		int beginOffset = BeginAddingRecord(335);
		AppendCharacter(charId);
		AppendLocation(location);
		EndAddingRecord(beginOffset);
	}

	public void AddSectMainStoryBaihuaAnonymReturns()
	{
		int beginOffset = BeginAddingRecord(336);
		EndAddingRecord(beginOffset);
	}

	public void AddSectMainStoryBaihuaGifts(Location location)
	{
		int beginOffset = BeginAddingRecord(337);
		AppendLocation(location);
		EndAddingRecord(beginOffset);
	}

	public void AddSectMainStoryBaihuaMelanoPlay()
	{
		int beginOffset = BeginAddingRecord(338);
		EndAddingRecord(beginOffset);
	}

	public void AddSectMainStoryBaihuaLeukoPlay()
	{
		int beginOffset = BeginAddingRecord(339);
		EndAddingRecord(beginOffset);
	}

	public void AddSectMainStoryBaihuaLeukoMelanoPlay()
	{
		int beginOffset = BeginAddingRecord(340);
		EndAddingRecord(beginOffset);
	}

	public void AddSectMainStoryFulongDiasterAppear()
	{
		int beginOffset = BeginAddingRecord(341);
		EndAddingRecord(beginOffset);
	}

	public void AddSectMainStoryFulongShadow()
	{
		int beginOffset = BeginAddingRecord(342);
		EndAddingRecord(beginOffset);
	}

	public void AddSectMainStoryFulongLazuliLetter()
	{
		int beginOffset = BeginAddingRecord(343);
		EndAddingRecord(beginOffset);
	}

	public void AddSectMainStoryFulongProsperous()
	{
		int beginOffset = BeginAddingRecord(344);
		EndAddingRecord(beginOffset);
	}

	public void AddSectMainStoryFulongFailing()
	{
		int beginOffset = BeginAddingRecord(345);
		EndAddingRecord(beginOffset);
	}

	public void AddHuntCriminal(int charId, int charId1, int charId2)
	{
		int beginOffset = BeginAddingRecord(346);
		AppendCharacter(charId);
		AppendCharacter(charId1);
		AppendCharacter(charId2);
		EndAddingRecord(beginOffset);
	}

	public void AddSentenceCompleted(int charId, short settlementId)
	{
		int beginOffset = BeginAddingRecord(347);
		AppendCharacter(charId);
		AppendSettlement(settlementId);
		EndAddingRecord(beginOffset);
	}

	public void AddSectMainStoryFulongRobTaiwu(Location location, int charId)
	{
		int beginOffset = BeginAddingRecord(348);
		AppendLocation(location);
		AppendCharacter(charId);
		EndAddingRecord(beginOffset);
	}

	public void AddSectMainStoryFulongInterfereRobbery(int charId, Location location, int charId1)
	{
		int beginOffset = BeginAddingRecord(349);
		AppendCharacter(charId);
		AppendLocation(location);
		AppendCharacter(charId1);
		EndAddingRecord(beginOffset);
	}

	public void AddSectMainStoryFulongProtect(int charId, Location location, int charId1, int charId2)
	{
		int beginOffset = BeginAddingRecord(350);
		AppendCharacter(charId);
		AppendLocation(location);
		AppendCharacter(charId1);
		AppendCharacter(charId2);
		EndAddingRecord(beginOffset);
	}

	public void AddSectMainStoryFulongFireFighting(int charId)
	{
		int beginOffset = BeginAddingRecord(351);
		AppendCharacter(charId);
		EndAddingRecord(beginOffset);
	}

	public void AddSectMainStoryFulongAftermath()
	{
		int beginOffset = BeginAddingRecord(352);
		EndAddingRecord(beginOffset);
	}

	public void AddAdviseHealDisorderOfQi(int charId, Location location, int charId1, int value)
	{
		int beginOffset = BeginAddingRecord(353);
		AppendCharacter(charId);
		AppendLocation(location);
		AppendCharacter(charId1);
		AppendInteger(value);
		EndAddingRecord(beginOffset);
	}

	public void AddAdviseHealHealth(int charId, Location location, int charId1, int value)
	{
		int beginOffset = BeginAddingRecord(354);
		AppendCharacter(charId);
		AppendLocation(location);
		AppendCharacter(charId1);
		AppendInteger(value);
		EndAddingRecord(beginOffset);
	}

	public void AddTaiWuVillagerClothing(sbyte orgTemplateId, sbyte orgGrade, bool orgPrincipal, sbyte gender, sbyte itemType, short itemTemplateId)
	{
		int beginOffset = BeginAddingRecord(355);
		AppendOrgGrade(orgTemplateId, orgGrade, orgPrincipal, gender);
		AppendItem(itemType, itemTemplateId);
		EndAddingRecord(beginOffset);
	}

	public void AddHuntCriminalTaiwu(int charId, int charId1, Location location)
	{
		int beginOffset = BeginAddingRecord(356);
		AppendCharacter(charId);
		AppendCharacter(charId1);
		AppendLocation(location);
		EndAddingRecord(beginOffset);
	}

	public void AddSectMainStoryZhujianHeir(int charId, Location location)
	{
		int beginOffset = BeginAddingRecord(357);
		AppendCharacter(charId);
		AppendLocation(location);
		EndAddingRecord(beginOffset);
	}

	public void AddSectMainStoryZhujianHazyRain(int charId)
	{
		int beginOffset = BeginAddingRecord(358);
		AppendCharacter(charId);
		EndAddingRecord(beginOffset);
	}

	public void AddSectMainStoryZhujianTongshengSpeaks(int charId, Location location)
	{
		int beginOffset = BeginAddingRecord(359);
		AppendCharacter(charId);
		AppendLocation(location);
		EndAddingRecord(beginOffset);
	}

	public void AddSectMainStoryZhujianHuichuntang(int charId, Location location)
	{
		int beginOffset = BeginAddingRecord(360);
		AppendCharacter(charId);
		AppendLocation(location);
		EndAddingRecord(beginOffset);
	}

	public void AddSectMainStoryZhujianProsperous()
	{
		int beginOffset = BeginAddingRecord(361);
		EndAddingRecord(beginOffset);
	}

	public void AddSectMainStoryZhujianFailing()
	{
		int beginOffset = BeginAddingRecord(362);
		EndAddingRecord(beginOffset);
	}

	public void AddJieQingPunishmentAssassin(int charId, Location location, int charId1)
	{
		int beginOffset = BeginAddingRecord(363);
		AppendCharacter(charId);
		AppendLocation(location);
		AppendCharacter(charId1);
		EndAddingRecord(beginOffset);
	}

	public void AddTaiwuBeHuntedHunterDie(int charId, int charId1)
	{
		int beginOffset = BeginAddingRecord(364);
		AppendCharacter(charId);
		AppendCharacter(charId1);
		EndAddingRecord(beginOffset);
	}

	public void AddWardOffXiangshuProtection(int charId, int value)
	{
		int beginOffset = BeginAddingRecord(365);
		AppendCharacter(charId);
		AppendInteger(value);
		EndAddingRecord(beginOffset);
	}

	public void AddProfessionDukeReceiveCricket(int charId)
	{
		int beginOffset = BeginAddingRecord(366);
		AppendCharacter(charId);
		EndAddingRecord(beginOffset);
	}

	public void AddCricketInDreamTaiwuPartnerPregnant(int charId, Location location, int charId1)
	{
		int beginOffset = BeginAddingRecord(367);
		AppendCharacter(charId);
		AppendLocation(location);
		AppendCharacter(charId1);
		EndAddingRecord(beginOffset);
	}

	public void AddSectMainStoryShaolinDharmaCave(int charId)
	{
		int beginOffset = BeginAddingRecord(368);
		AppendCharacter(charId);
		EndAddingRecord(beginOffset);
	}

	public void AddTaiwuVillageStoneClaimed(int charId, short settlementId, short settlementId1, int value)
	{
		int beginOffset = BeginAddingRecord(369);
		AppendCharacter(charId);
		AppendSettlement(settlementId);
		AppendSettlement(settlementId1);
		AppendInteger(value);
		EndAddingRecord(beginOffset);
	}

	public void AddTaiwuVillagerAdoptOrphan(int charId, int charId1, int value)
	{
		int beginOffset = BeginAddingRecord(370);
		AppendCharacter(charId);
		AppendCharacter(charId1);
		AppendInteger(value);
		EndAddingRecord(beginOffset);
	}

	public void AddSectMainStoryRemakeEmeiPrologue()
	{
		int beginOffset = BeginAddingRecord(371);
		EndAddingRecord(beginOffset);
	}

	public void AddSectMainStoryRemakeEmeiMessage()
	{
		int beginOffset = BeginAddingRecord(372);
		EndAddingRecord(beginOffset);
	}

	public void AddSectMainStoryRemakeEmeiHomocideCase()
	{
		int beginOffset = BeginAddingRecord(373);
		EndAddingRecord(beginOffset);
	}

	public void AddSectMainStoryRemakeEmeiChanges()
	{
		int beginOffset = BeginAddingRecord(374);
		EndAddingRecord(beginOffset);
	}

	public void AddSectMainStoryRemakeEmeiMyth()
	{
		int beginOffset = BeginAddingRecord(375);
		EndAddingRecord(beginOffset);
	}

	public void AddSectMainStoryRemakeEmeiFinale()
	{
		int beginOffset = BeginAddingRecord(376);
		EndAddingRecord(beginOffset);
	}

	public void AddSectMainStoryRemakeEmeiProsperous()
	{
		int beginOffset = BeginAddingRecord(377);
		EndAddingRecord(beginOffset);
	}

	public void AddSectMainStoryRemakeEmeiFailing()
	{
		int beginOffset = BeginAddingRecord(378);
		EndAddingRecord(beginOffset);
	}

	public void AddSectMainStoryRemakeYuanshanPrison(int charId)
	{
		int beginOffset = BeginAddingRecord(379);
		AppendCharacter(charId);
		EndAddingRecord(beginOffset);
	}

	public void AddSectMainStoryRemakeYuanshanFailing()
	{
		int beginOffset = BeginAddingRecord(380);
		EndAddingRecord(beginOffset);
	}

	public void AddNormalHeavenlyTreeDestroyed(Location location)
	{
		int beginOffset = BeginAddingRecord(381);
		AppendLocation(location);
		EndAddingRecord(beginOffset);
	}

	public void AddNormalGuardHeavenlyTree(int charId, Location location)
	{
		int beginOffset = BeginAddingRecord(382);
		AppendCharacter(charId);
		AppendLocation(location);
		EndAddingRecord(beginOffset);
	}

	public void AddDLCYearOfHorseCloth()
	{
		int beginOffset = BeginAddingRecord(384);
		EndAddingRecord(beginOffset);
	}

	public void AddTaskMonthlyEvent(IVariantCollection<string> argBox, TaskInfoItem taskInfo, AutoTriggerMonthlyEvent monthlyEvent)
	{
		int beginOffset = BeginAddingRecord(monthlyEvent.MonthlyEventId);
		MonthlyEventItem monthlyEventItem = Config.MonthlyEvent.Instance[monthlyEvent.MonthlyEventId];
		int i = 0;
		Location location = default(Location);
		ItemKey itemKey = default(ItemKey);
		for (int num = monthlyEventItem.Parameters.Length; i < num; i++)
		{
			string text = monthlyEventItem.Parameters[i];
			if (string.IsNullOrEmpty(text))
			{
				break;
			}
			string text2 = monthlyEvent.Args[i];
			switch (ParameterType.Parse(text))
			{
			case 0:
			{
				int charIdFromArgBox = GetCharIdFromArgBox(argBox, text2);
				AppendCharacter(charIdFromArgBox);
				break;
			}
			case 1:
				argBox.Get<Location>(text2, ref location);
				AppendLocation(location);
				break;
			case 5:
			{
				int num3 = -1;
				argBox.Get(text2, ref num3);
				AppendSettlement((short)num3);
				break;
			}
			case 10:
			{
				int num2 = -1;
				argBox.Get(text2, ref num2);
				AppendAdventure((short)num2);
				break;
			}
			case 22:
			{
				int value = 0;
				argBox.Get(text2, ref value);
				AppendInteger(value);
				break;
			}
			case 25:
				argBox.Get<ItemKey>(text2, ref itemKey);
				AppendItemKey((ulong)itemKey);
				break;
			}
		}
		EndAddingRecord(beginOffset);
	}

	public void AddWorldStateMonthlyEvent(WorldStateItem worldState, MonthlyEventItem monthlyEvent)
	{
		int beginOffset = BeginAddingRecord(monthlyEvent.TemplateId);
		EndAddingRecord(beginOffset);
	}

	private int GetCharIdFromArgBox(IVariantCollection<string> argBox, string argKey)
	{
		if (argKey == "RoleTaiwu")
		{
			return ExternalDataBridge.Context.TaiwuCharId;
		}
		int result = -1;
		if (!argBox.Get(argKey, ref result))
		{
			return -1;
		}
		return result;
	}

	public unsafe override void FillEventArgBox(int offset, IVariantCollection<string> eventArgBox)
	{
		string keyPrefix = "MonthlyEvent_arg";
		fixed (byte* rawData = RawData)
		{
			byte* ptr = rawData + offset;
			short index = *(short*)(ptr + 1);
			ptr += 3;
			string[] parameters = Config.MonthlyEvent.Instance[index].Parameters;
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
}
