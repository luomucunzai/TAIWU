using System;
using System.Collections.Generic;
using Config;
using GameData.Domains.LifeRecord.GeneralRecord;
using GameData.Domains.Map;
using GameData.Serializer;

namespace GameData.Domains.Information.Collection;

[SerializableGameData(NotForDisplayModule = true)]
public class SecretInformationCollection : WriteableRecordCollection
{
	public void GetRenderInfos(List<SecretInformationRenderInfo> renderInfos, ArgumentCollection argumentCollection)
	{
		int index = -1;
		int offset = -1;
		while (Next(ref index, ref offset))
		{
			SecretInformationRenderInfo renderInfo = GetRenderInfo(offset, argumentCollection);
			renderInfos.Add(renderInfo);
		}
	}

	public new unsafe SecretInformationRenderInfo GetRenderInfo(int offset, ArgumentCollection argumentCollection)
	{
		fixed (byte* rawData = RawData)
		{
			byte* ptr = rawData + offset;
			int date = *(int*)(ptr + 1);
			short num = ((short*)(ptr + 1))[2];
			ptr += 7;
			InstantNotificationItem instantNotificationItem = InstantNotification.Instance[num];
			string[] parameters = instantNotificationItem.Parameters;
			SecretInformationRenderInfo secretInformationRenderInfo = new SecretInformationRenderInfo(num, instantNotificationItem.Desc, date);
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
				secretInformationRenderInfo.Arguments.Add((b, item));
			}
			return secretInformationRenderInfo;
		}
	}

	public int CopyRecord(int recordOffset)
	{
		int recordSize = GetRecordSize(recordOffset);
		int size = Size;
		int desiredSize = Size + recordSize;
		EnsureCapacity(desiredSize);
		for (int i = 0; i < recordSize; i++)
		{
			RawData[size + i] = RawData[recordOffset + i];
		}
		return size;
	}

	private new unsafe int BeginAddingRecord(short recordType)
	{
		int size = Size;
		int num = Size + 1 + 4 + 2;
		EnsureCapacity(num);
		Size = num;
		int currDate = ExternalDataBridge.Context.CurrDate;
		fixed (byte* rawData = RawData)
		{
			byte* num2 = rawData + size;
			*(int*)(num2 + 1) = currDate;
			((short*)(num2 + 1))[2] = recordType;
		}
		return size;
	}

	protected new unsafe void EndAddingRecord(int beginOffset)
	{
		Size = (Size + 4 - 1) & -4;
		int num = Size - beginOffset;
		if (num > 255)
		{
			throw new Exception("Record exceeded the max size");
		}
		fixed (byte* rawData = RawData)
		{
			rawData[beginOffset] = (byte)num;
		}
		Count++;
	}

	public int AddDie(int charId, Location location)
	{
		int num = BeginAddingRecord(0);
		AppendCharacter(charId);
		AppendLocation(location);
		EndAddingRecord(num);
		return num;
	}

	public int AddKillInPublic(int charId, int charId1)
	{
		int num = BeginAddingRecord(1);
		AppendCharacter(charId);
		AppendCharacter(charId1);
		EndAddingRecord(num);
		return num;
	}

	public int AddKidnapInPublic(int charId, int charId1)
	{
		int num = BeginAddingRecord(2);
		AppendCharacter(charId);
		AppendCharacter(charId1);
		EndAddingRecord(num);
		return num;
	}

	public int AddKillForPunishment(int charId, int charId1)
	{
		int num = BeginAddingRecord(3);
		AppendCharacter(charId);
		AppendCharacter(charId1);
		EndAddingRecord(num);
		return num;
	}

	public int AddKidnapForPunishment(int charId, int charId1)
	{
		int num = BeginAddingRecord(4);
		AppendCharacter(charId);
		AppendCharacter(charId1);
		EndAddingRecord(num);
		return num;
	}

	public int AddUnexpectedResourceGain(int charId, sbyte resourceType, Location location)
	{
		int num = BeginAddingRecord(5);
		AppendCharacter(charId);
		AppendResource(resourceType);
		AppendLocation(location);
		EndAddingRecord(num);
		return num;
	}

	public int AddUnexpectedItemGain(int charId, ulong itemKey, Location location)
	{
		int num = BeginAddingRecord(6);
		AppendCharacter(charId);
		AppendItemKey(itemKey);
		AppendLocation(location);
		EndAddingRecord(num);
		return num;
	}

	public int AddUnexpectedSkillBookGain(int charId, ulong itemKey, Location location)
	{
		int num = BeginAddingRecord(7);
		AppendCharacter(charId);
		AppendItemKey(itemKey);
		AppendLocation(location);
		EndAddingRecord(num);
		return num;
	}

	public int AddUnexpectedCure(int charId, Location location)
	{
		int num = BeginAddingRecord(8);
		AppendCharacter(charId);
		AppendLocation(location);
		EndAddingRecord(num);
		return num;
	}

	public int AddUnexpectedResourceLose(int charId, sbyte resourceType, Location location)
	{
		int num = BeginAddingRecord(9);
		AppendCharacter(charId);
		AppendResource(resourceType);
		AppendLocation(location);
		EndAddingRecord(num);
		return num;
	}

	public int AddUnexpectedItemLose(int charId, ulong itemKey, Location location)
	{
		int num = BeginAddingRecord(10);
		AppendCharacter(charId);
		AppendItemKey(itemKey);
		AppendLocation(location);
		EndAddingRecord(num);
		return num;
	}

	public int AddUnexpectedSkillBookLose(int charId, ulong itemKey, Location location)
	{
		int num = BeginAddingRecord(11);
		AppendCharacter(charId);
		AppendItemKey(itemKey);
		AppendLocation(location);
		EndAddingRecord(num);
		return num;
	}

	public int AddUnexpectedHarm(int charId, Location location)
	{
		int num = BeginAddingRecord(12);
		AppendCharacter(charId);
		AppendLocation(location);
		EndAddingRecord(num);
		return num;
	}

	public int AddLifeSkillBattleWin(int charId, int charId1)
	{
		int num = BeginAddingRecord(13);
		AppendCharacter(charId);
		AppendCharacter(charId1);
		EndAddingRecord(num);
		return num;
	}

	public int AddCricketBattleWin(int charId, int charId1)
	{
		int num = BeginAddingRecord(14);
		AppendCharacter(charId);
		AppendCharacter(charId1);
		EndAddingRecord(num);
		return num;
	}

	public int AddMajorVictoryInCombat(int charId, int charId1)
	{
		int num = BeginAddingRecord(15);
		AppendCharacter(charId);
		AppendCharacter(charId1);
		EndAddingRecord(num);
		return num;
	}

	public int AddMinorVictoryInCombat(int charId, int charId1)
	{
		int num = BeginAddingRecord(16);
		AppendCharacter(charId);
		AppendCharacter(charId1);
		EndAddingRecord(num);
		return num;
	}

	public int AddMourn(int charId, int charId1)
	{
		int num = BeginAddingRecord(17);
		AppendCharacter(charId);
		AppendCharacter(charId1);
		EndAddingRecord(num);
		return num;
	}

	public int AddOfferProtection(int charId, int charId1, int charId2)
	{
		int num = BeginAddingRecord(18);
		AppendCharacter(charId);
		AppendCharacter(charId1);
		AppendCharacter(charId2);
		EndAddingRecord(num);
		return num;
	}

	public int AddLoseFetus(int charId, Location location)
	{
		int num = BeginAddingRecord(19);
		AppendCharacter(charId);
		AppendLocation(location);
		EndAddingRecord(num);
		return num;
	}

	public int AddLoseFetus2(int charId, int charId1, Location location)
	{
		int num = BeginAddingRecord(20);
		AppendCharacter(charId);
		AppendCharacter(charId1);
		AppendLocation(location);
		EndAddingRecord(num);
		return num;
	}

	public int AddGiveBirthToChild(int charId, int charId1)
	{
		int num = BeginAddingRecord(21);
		AppendCharacter(charId);
		AppendCharacter(charId1);
		EndAddingRecord(num);
		return num;
	}

	public int AddGiveBirthToChild2(int charId, int charId1, int charId2)
	{
		int num = BeginAddingRecord(22);
		AppendCharacter(charId);
		AppendCharacter(charId1);
		AppendCharacter(charId2);
		EndAddingRecord(num);
		return num;
	}

	public int AddAbandonChild(int charId, int charId1)
	{
		int num = BeginAddingRecord(23);
		AppendCharacter(charId);
		AppendCharacter(charId1);
		EndAddingRecord(num);
		return num;
	}

	public int AddReleaseKidnappedCharacter(int charId, int charId1)
	{
		int num = BeginAddingRecord(24);
		AppendCharacter(charId);
		AppendCharacter(charId1);
		EndAddingRecord(num);
		return num;
	}

	public int AddRescueKidnappedCharacter(int charId, int charId1, int charId2)
	{
		int num = BeginAddingRecord(25);
		AppendCharacter(charId);
		AppendCharacter(charId1);
		AppendCharacter(charId2);
		EndAddingRecord(num);
		return num;
	}

	public int AddKidnappedCharacterEscaped(int charId, int charId1)
	{
		int num = BeginAddingRecord(26);
		AppendCharacter(charId);
		AppendCharacter(charId1);
		EndAddingRecord(num);
		return num;
	}

	public int AddReadBookFail(int charId, ulong itemKey)
	{
		int num = BeginAddingRecord(27);
		AppendCharacter(charId);
		AppendItemKey(itemKey);
		EndAddingRecord(num);
		return num;
	}

	public int AddBreakoutFail(int charId, short combatSkillTemplateId)
	{
		int num = BeginAddingRecord(28);
		AppendCharacter(charId);
		AppendCombatSkill(combatSkillTemplateId);
		EndAddingRecord(num);
		return num;
	}

	public int AddLoseOverloadingItem(int charId, ulong itemKey, Location location)
	{
		int num = BeginAddingRecord(29);
		AppendCharacter(charId);
		AppendItemKey(itemKey);
		AppendLocation(location);
		EndAddingRecord(num);
		return num;
	}

	public int AddSeverEnemy(int charId, int charId1)
	{
		int num = BeginAddingRecord(30);
		AppendCharacter(charId);
		AppendCharacter(charId1);
		EndAddingRecord(num);
		return num;
	}

	public int AddBecomeEnemy(int charId, int charId1)
	{
		int num = BeginAddingRecord(31);
		AppendCharacter(charId);
		AppendCharacter(charId1);
		EndAddingRecord(num);
		return num;
	}

	public int AddBecomeFriend(int charId, int charId1)
	{
		int num = BeginAddingRecord(32);
		AppendCharacter(charId);
		AppendCharacter(charId1);
		EndAddingRecord(num);
		return num;
	}

	public int AddSeverFriend(int charId, int charId1)
	{
		int num = BeginAddingRecord(33);
		AppendCharacter(charId);
		AppendCharacter(charId1);
		EndAddingRecord(num);
		return num;
	}

	public int AddBecomeLover(int charId, int charId1)
	{
		int num = BeginAddingRecord(34);
		AppendCharacter(charId);
		AppendCharacter(charId1);
		EndAddingRecord(num);
		return num;
	}

	public int AddBreakupWithLover(int charId, int charId1)
	{
		int num = BeginAddingRecord(35);
		AppendCharacter(charId);
		AppendCharacter(charId1);
		EndAddingRecord(num);
		return num;
	}

	public int AddBecomeHusbandAndWife(int charId, int charId1)
	{
		int num = BeginAddingRecord(36);
		AppendCharacter(charId);
		AppendCharacter(charId1);
		EndAddingRecord(num);
		return num;
	}

	public int AddBecomeSwornBrothersAndSisters(int charId, int charId1)
	{
		int num = BeginAddingRecord(37);
		AppendCharacter(charId);
		AppendCharacter(charId1);
		EndAddingRecord(num);
		return num;
	}

	public int AddSeverSwornBrothersAndSisters(int charId, int charId1)
	{
		int num = BeginAddingRecord(38);
		AppendCharacter(charId);
		AppendCharacter(charId1);
		EndAddingRecord(num);
		return num;
	}

	public int AddGetAdopted(int charId, int charId1)
	{
		int num = BeginAddingRecord(39);
		AppendCharacter(charId);
		AppendCharacter(charId1);
		EndAddingRecord(num);
		return num;
	}

	public int AddAdoptChild(int charId, int charId1)
	{
		int num = BeginAddingRecord(40);
		AppendCharacter(charId);
		AppendCharacter(charId1);
		EndAddingRecord(num);
		return num;
	}

	public int AddGivingResource(int charId, int charId1, sbyte resourceType)
	{
		int num = BeginAddingRecord(41);
		AppendCharacter(charId);
		AppendCharacter(charId1);
		AppendResource(resourceType);
		EndAddingRecord(num);
		return num;
	}

	public int AddGiveItem(int charId, int charId1, ulong itemKey)
	{
		int num = BeginAddingRecord(42);
		AppendCharacter(charId);
		AppendCharacter(charId1);
		AppendItemKey(itemKey);
		EndAddingRecord(num);
		return num;
	}

	public int AddBuildGrave(int charId, int charId1)
	{
		int num = BeginAddingRecord(43);
		AppendCharacter(charId);
		AppendCharacter(charId1);
		EndAddingRecord(num);
		return num;
	}

	public int AddCure(int charId, int charId1)
	{
		int num = BeginAddingRecord(44);
		AppendCharacter(charId);
		AppendCharacter(charId1);
		EndAddingRecord(num);
		return num;
	}

	public int AddRepairItem(int charId, int charId1, ulong itemKey)
	{
		int num = BeginAddingRecord(45);
		AppendCharacter(charId);
		AppendCharacter(charId1);
		AppendItemKey(itemKey);
		EndAddingRecord(num);
		return num;
	}

	public int AddInstructOnLifeSkill(int charId, int charId1, short lifeSkillTemplateId)
	{
		int num = BeginAddingRecord(46);
		AppendCharacter(charId);
		AppendCharacter(charId1);
		AppendLifeSkill(lifeSkillTemplateId);
		EndAddingRecord(num);
		return num;
	}

	public int AddInstructOnCombatSkill(int charId, int charId1, short combatSkillTemplateId)
	{
		int num = BeginAddingRecord(47);
		AppendCharacter(charId);
		AppendCharacter(charId1);
		AppendCombatSkill(combatSkillTemplateId);
		EndAddingRecord(num);
		return num;
	}

	public int AddAcceptRequestHealInjury(int charId, int charId1)
	{
		int num = BeginAddingRecord(48);
		AppendCharacter(charId);
		AppendCharacter(charId1);
		EndAddingRecord(num);
		return num;
	}

	public int AddAcceptRequestDetoxPoison(int charId, int charId1)
	{
		int num = BeginAddingRecord(49);
		AppendCharacter(charId);
		AppendCharacter(charId1);
		EndAddingRecord(num);
		return num;
	}

	public int AddAcceptRequestIncreaseHealth(int charId, int charId1)
	{
		int num = BeginAddingRecord(50);
		AppendCharacter(charId);
		AppendCharacter(charId1);
		EndAddingRecord(num);
		return num;
	}

	public int AddAcceptRequestRestoreDisorderOfQi(int charId, int charId1)
	{
		int num = BeginAddingRecord(51);
		AppendCharacter(charId);
		AppendCharacter(charId1);
		EndAddingRecord(num);
		return num;
	}

	public int AddAcceptRequestIncreaseNeili(int charId, int charId1)
	{
		int num = BeginAddingRecord(52);
		AppendCharacter(charId);
		AppendCharacter(charId1);
		EndAddingRecord(num);
		return num;
	}

	public int AddAcceptRequestKillWug(int charId, int charId1)
	{
		int num = BeginAddingRecord(53);
		AppendCharacter(charId);
		AppendCharacter(charId1);
		EndAddingRecord(num);
		return num;
	}

	public int AddAcceptRequestFood(int charId, int charId1, ulong itemKey)
	{
		int num = BeginAddingRecord(54);
		AppendCharacter(charId);
		AppendCharacter(charId1);
		AppendItemKey(itemKey);
		EndAddingRecord(num);
		return num;
	}

	public int AddAcceptRequestTeaWine(int charId, int charId1, ulong itemKey)
	{
		int num = BeginAddingRecord(55);
		AppendCharacter(charId);
		AppendCharacter(charId1);
		AppendItemKey(itemKey);
		EndAddingRecord(num);
		return num;
	}

	public int AddAcceptRequestResource(int charId, int charId1, sbyte resourceType)
	{
		int num = BeginAddingRecord(56);
		AppendCharacter(charId);
		AppendCharacter(charId1);
		AppendResource(resourceType);
		EndAddingRecord(num);
		return num;
	}

	public int AddAcceptRequestItem(int charId, int charId1, ulong itemKey)
	{
		int num = BeginAddingRecord(57);
		AppendCharacter(charId);
		AppendCharacter(charId1);
		AppendItemKey(itemKey);
		EndAddingRecord(num);
		return num;
	}

	public int AddAcceptRequestDrinking(int charId, int charId1)
	{
		int num = BeginAddingRecord(58);
		AppendCharacter(charId);
		AppendCharacter(charId1);
		EndAddingRecord(num);
		return num;
	}

	public int AddAcceptRequestGivingMoney(int charId, int charId1, int value)
	{
		int num = BeginAddingRecord(59);
		AppendCharacter(charId);
		AppendCharacter(charId1);
		AppendInteger(value);
		EndAddingRecord(num);
		return num;
	}

	public int AddAcceptRequestInstructionOnReading(int charId, int charId1, ulong itemKey)
	{
		int num = BeginAddingRecord(60);
		AppendCharacter(charId);
		AppendCharacter(charId1);
		AppendItemKey(itemKey);
		EndAddingRecord(num);
		return num;
	}

	public int AddAcceptRequestInstructionOnBreakout(int charId, int charId1, short combatSkillTemplateId)
	{
		int num = BeginAddingRecord(61);
		AppendCharacter(charId);
		AppendCharacter(charId1);
		AppendCombatSkill(combatSkillTemplateId);
		EndAddingRecord(num);
		return num;
	}

	public int AddAcceptRequestRepairItem(int charId, int charId1, ulong itemKey)
	{
		int num = BeginAddingRecord(62);
		AppendCharacter(charId);
		AppendCharacter(charId1);
		AppendItemKey(itemKey);
		EndAddingRecord(num);
		return num;
	}

	public int AddAcceptRequestAddPoisonToItem(int charId, int charId1, ulong itemKey, ulong itemKey1)
	{
		int num = BeginAddingRecord(63);
		AppendCharacter(charId);
		AppendCharacter(charId1);
		AppendItemKey(itemKey);
		AppendItemKey(itemKey1);
		EndAddingRecord(num);
		return num;
	}

	public int AddAcceptRequestInstructionOnLifeSkill(int charId, int charId1, short lifeSkillTemplateId)
	{
		int num = BeginAddingRecord(64);
		AppendCharacter(charId);
		AppendCharacter(charId1);
		AppendLifeSkill(lifeSkillTemplateId);
		EndAddingRecord(num);
		return num;
	}

	public int AddAcceptRequestInstructionOnCombatSkill(int charId, int charId1, short combatSkillTemplateId)
	{
		int num = BeginAddingRecord(65);
		AppendCharacter(charId);
		AppendCharacter(charId1);
		AppendCombatSkill(combatSkillTemplateId);
		EndAddingRecord(num);
		return num;
	}

	public int AddRehaircutSuccess(int charId, int charId1)
	{
		int num = BeginAddingRecord(66);
		AppendCharacter(charId);
		AppendCharacter(charId1);
		EndAddingRecord(num);
		return num;
	}

	public int AddRehaircutIncompleted(int charId, int charId1)
	{
		int num = BeginAddingRecord(67);
		AppendCharacter(charId);
		AppendCharacter(charId1);
		EndAddingRecord(num);
		return num;
	}

	public int AddRehaircutFail(int charId, int charId1)
	{
		int num = BeginAddingRecord(68);
		AppendCharacter(charId);
		AppendCharacter(charId1);
		EndAddingRecord(num);
		return num;
	}

	public int AddRefuseRequestHealInjury(int charId, int charId1)
	{
		int num = BeginAddingRecord(69);
		AppendCharacter(charId);
		AppendCharacter(charId1);
		EndAddingRecord(num);
		return num;
	}

	public int AddRefuseRequestDetoxPoison(int charId, int charId1)
	{
		int num = BeginAddingRecord(70);
		AppendCharacter(charId);
		AppendCharacter(charId1);
		EndAddingRecord(num);
		return num;
	}

	public int AddRefuseRequestIncreaseHealth(int charId, int charId1)
	{
		int num = BeginAddingRecord(71);
		AppendCharacter(charId);
		AppendCharacter(charId1);
		EndAddingRecord(num);
		return num;
	}

	public int AddRefuseRequestRestoreDisorderOfQi(int charId, int charId1)
	{
		int num = BeginAddingRecord(72);
		AppendCharacter(charId);
		AppendCharacter(charId1);
		EndAddingRecord(num);
		return num;
	}

	public int AddRefuseRequestIncreaseNeili(int charId, int charId1)
	{
		int num = BeginAddingRecord(73);
		AppendCharacter(charId);
		AppendCharacter(charId1);
		EndAddingRecord(num);
		return num;
	}

	public int AddRefuseRequestKillWug(int charId, int charId1)
	{
		int num = BeginAddingRecord(74);
		AppendCharacter(charId);
		AppendCharacter(charId1);
		EndAddingRecord(num);
		return num;
	}

	public int AddRefuseRequestFood(int charId, int charId1, ulong itemKey)
	{
		int num = BeginAddingRecord(75);
		AppendCharacter(charId);
		AppendCharacter(charId1);
		AppendItemKey(itemKey);
		EndAddingRecord(num);
		return num;
	}

	public int AddRefuseRequestTeaWine(int charId, int charId1, ulong itemKey)
	{
		int num = BeginAddingRecord(76);
		AppendCharacter(charId);
		AppendCharacter(charId1);
		AppendItemKey(itemKey);
		EndAddingRecord(num);
		return num;
	}

	public int AddRefuseRequestResource(int charId, int charId1, sbyte resourceType)
	{
		int num = BeginAddingRecord(77);
		AppendCharacter(charId);
		AppendCharacter(charId1);
		AppendResource(resourceType);
		EndAddingRecord(num);
		return num;
	}

	public int AddRefuseRequestItem(int charId, int charId1, ulong itemKey)
	{
		int num = BeginAddingRecord(78);
		AppendCharacter(charId);
		AppendCharacter(charId1);
		AppendItemKey(itemKey);
		EndAddingRecord(num);
		return num;
	}

	public int AddRefuseRequestDrinking(int charId, int charId1)
	{
		int num = BeginAddingRecord(79);
		AppendCharacter(charId);
		AppendCharacter(charId1);
		EndAddingRecord(num);
		return num;
	}

	public int AddRefuseRequestGivingMoney(int charId, int charId1, int value)
	{
		int num = BeginAddingRecord(80);
		AppendCharacter(charId);
		AppendCharacter(charId1);
		AppendInteger(value);
		EndAddingRecord(num);
		return num;
	}

	public int AddRefuseRequestInstructionOnReading(int charId, int charId1, ulong itemKey)
	{
		int num = BeginAddingRecord(81);
		AppendCharacter(charId);
		AppendCharacter(charId1);
		AppendItemKey(itemKey);
		EndAddingRecord(num);
		return num;
	}

	public int AddRefuseRequestInstructionOnBreakout(int charId, int charId1, short combatSkillTemplateId)
	{
		int num = BeginAddingRecord(82);
		AppendCharacter(charId);
		AppendCharacter(charId1);
		AppendCombatSkill(combatSkillTemplateId);
		EndAddingRecord(num);
		return num;
	}

	public int AddRefuseRequestRepairItem(int charId, int charId1, ulong itemKey)
	{
		int num = BeginAddingRecord(83);
		AppendCharacter(charId);
		AppendCharacter(charId1);
		AppendItemKey(itemKey);
		EndAddingRecord(num);
		return num;
	}

	public int AddRefuseRequestAddPoisonToItem(int charId, int charId1, ulong itemKey, ulong itemKey1)
	{
		int num = BeginAddingRecord(84);
		AppendCharacter(charId);
		AppendCharacter(charId1);
		AppendItemKey(itemKey);
		AppendItemKey(itemKey1);
		EndAddingRecord(num);
		return num;
	}

	public int AddRefuseRequestInstructionOnLifeSkill(int charId, int charId1, short lifeSkillTemplateId)
	{
		int num = BeginAddingRecord(85);
		AppendCharacter(charId);
		AppendCharacter(charId1);
		AppendLifeSkill(lifeSkillTemplateId);
		EndAddingRecord(num);
		return num;
	}

	public int AddRefuseRequestInstructionOnCombatSkill(int charId, int charId1, short combatSkillTemplateId)
	{
		int num = BeginAddingRecord(86);
		AppendCharacter(charId);
		AppendCharacter(charId1);
		AppendCombatSkill(combatSkillTemplateId);
		EndAddingRecord(num);
		return num;
	}

	public int AddRobGraveResource(int charId, int charId1, sbyte resourceType)
	{
		int num = BeginAddingRecord(87);
		AppendCharacter(charId);
		AppendCharacter(charId1);
		AppendResource(resourceType);
		EndAddingRecord(num);
		return num;
	}

	public int AddStealResource(int charId, int charId1, sbyte resourceType)
	{
		int num = BeginAddingRecord(88);
		AppendCharacter(charId);
		AppendCharacter(charId1);
		AppendResource(resourceType);
		EndAddingRecord(num);
		return num;
	}

	public int AddScamResource(int charId, int charId1, sbyte resourceType)
	{
		int num = BeginAddingRecord(89);
		AppendCharacter(charId);
		AppendCharacter(charId1);
		AppendResource(resourceType);
		EndAddingRecord(num);
		return num;
	}

	public int AddRobResource(int charId, int charId1, sbyte resourceType)
	{
		int num = BeginAddingRecord(90);
		AppendCharacter(charId);
		AppendCharacter(charId1);
		AppendResource(resourceType);
		EndAddingRecord(num);
		return num;
	}

	public int AddRobGraveItem(int charId, int charId1, ulong itemKey)
	{
		int num = BeginAddingRecord(91);
		AppendCharacter(charId);
		AppendCharacter(charId1);
		AppendItemKey(itemKey);
		EndAddingRecord(num);
		return num;
	}

	public int AddStealItem(int charId, int charId1, ulong itemKey)
	{
		int num = BeginAddingRecord(92);
		AppendCharacter(charId);
		AppendCharacter(charId1);
		AppendItemKey(itemKey);
		EndAddingRecord(num);
		return num;
	}

	public int AddScamItem(int charId, int charId1, ulong itemKey)
	{
		int num = BeginAddingRecord(93);
		AppendCharacter(charId);
		AppendCharacter(charId1);
		AppendItemKey(itemKey);
		EndAddingRecord(num);
		return num;
	}

	public int AddRobItem(int charId, int charId1, ulong itemKey)
	{
		int num = BeginAddingRecord(94);
		AppendCharacter(charId);
		AppendCharacter(charId1);
		AppendItemKey(itemKey);
		EndAddingRecord(num);
		return num;
	}

	public int AddKillInPrivate(int charId, int charId1)
	{
		int num = BeginAddingRecord(95);
		AppendCharacter(charId);
		AppendCharacter(charId1);
		EndAddingRecord(num);
		return num;
	}

	public int AddKidnapInPrivate(int charId, int charId1)
	{
		int num = BeginAddingRecord(96);
		AppendCharacter(charId);
		AppendCharacter(charId1);
		EndAddingRecord(num);
		return num;
	}

	public int AddPoisonEnemy(int charId, int charId1)
	{
		int num = BeginAddingRecord(97);
		AppendCharacter(charId);
		AppendCharacter(charId1);
		EndAddingRecord(num);
		return num;
	}

	public int AddPlotHarmEnemy(int charId, int charId1)
	{
		int num = BeginAddingRecord(98);
		AppendCharacter(charId);
		AppendCharacter(charId1);
		EndAddingRecord(num);
		return num;
	}

	public int AddStealLifeSkill(int charId, int charId1, short lifeSkillTemplateId)
	{
		int num = BeginAddingRecord(99);
		AppendCharacter(charId);
		AppendCharacter(charId1);
		AppendLifeSkill(lifeSkillTemplateId);
		EndAddingRecord(num);
		return num;
	}

	public int AddScamLifeSkill(int charId, int charId1, short lifeSkillTemplateId)
	{
		int num = BeginAddingRecord(100);
		AppendCharacter(charId);
		AppendCharacter(charId1);
		AppendLifeSkill(lifeSkillTemplateId);
		EndAddingRecord(num);
		return num;
	}

	public int AddStealCombatSkill(int charId, int charId1, short combatSkillTemplateId)
	{
		int num = BeginAddingRecord(101);
		AppendCharacter(charId);
		AppendCharacter(charId1);
		AppendCombatSkill(combatSkillTemplateId);
		EndAddingRecord(num);
		return num;
	}

	public int AddScamCombatSkill(int charId, int charId1, short combatSkillTemplateId)
	{
		int num = BeginAddingRecord(102);
		AppendCharacter(charId);
		AppendCharacter(charId1);
		AppendCombatSkill(combatSkillTemplateId);
		EndAddingRecord(num);
		return num;
	}

	public int AddAddPoisonToItem(int charId, ulong itemKey, ulong itemKey1)
	{
		int num = BeginAddingRecord(103);
		AppendCharacter(charId);
		AppendItemKey(itemKey);
		AppendItemKey(itemKey1);
		EndAddingRecord(num);
		return num;
	}

	public int AddMonkBreakRule(int charId, ulong itemKey)
	{
		int num = BeginAddingRecord(104);
		AppendCharacter(charId);
		AppendItemKey(itemKey);
		EndAddingRecord(num);
		return num;
	}

	public int AddMakeLoveIllegal(int charId, int charId1)
	{
		int num = BeginAddingRecord(105);
		AppendCharacter(charId);
		AppendCharacter(charId1);
		EndAddingRecord(num);
		return num;
	}

	public int AddRape(int charId, int charId1)
	{
		int num = BeginAddingRecord(106);
		AppendCharacter(charId);
		AppendCharacter(charId1);
		EndAddingRecord(num);
		return num;
	}

	public int AddLoseFetusFatherUnknown(int charId, int charId1, Location location)
	{
		int num = BeginAddingRecord(107);
		AppendCharacter(charId);
		AppendCharacter(charId1);
		AppendLocation(location);
		EndAddingRecord(num);
		return num;
	}

	public int AddGiveBirthToChildFatherUnknown(int charId, int charId1, int charId2)
	{
		int num = BeginAddingRecord(108);
		AppendCharacter(charId);
		AppendCharacter(charId1);
		AppendCharacter(charId2);
		EndAddingRecord(num);
		return num;
	}

	public int AddDatingWithCrush(int charId, int charId1, Location location)
	{
		int num = BeginAddingRecord(109);
		AppendCharacter(charId);
		AppendCharacter(charId1);
		AppendLocation(location);
		EndAddingRecord(num);
		return num;
	}

	public int AddForcingSilence(int charId, int charId1)
	{
		int num = BeginAddingRecord(110);
		AppendCharacter(charId);
		AppendCharacter(charId1);
		EndAddingRecord(num);
		return num;
	}

	public int AddRetrieveChild(int charId, int charId1)
	{
		int num = BeginAddingRecord(111);
		AppendCharacter(charId);
		AppendCharacter(charId1);
		EndAddingRecord(num);
		return num;
	}

	public int AddSolveScripture1(int charId)
	{
		int num = BeginAddingRecord(112);
		AppendCharacter(charId);
		EndAddingRecord(num);
		return num;
	}

	public int AddSolveScripture2(int charId)
	{
		int num = BeginAddingRecord(113);
		AppendCharacter(charId);
		EndAddingRecord(num);
		return num;
	}

	public int AddSolveScripture3(int charId)
	{
		int num = BeginAddingRecord(114);
		AppendCharacter(charId);
		EndAddingRecord(num);
		return num;
	}

	public int AddSolveScripture4(int charId)
	{
		int num = BeginAddingRecord(115);
		AppendCharacter(charId);
		EndAddingRecord(num);
		return num;
	}

	public int AddPrisonBreak(int charId, Location location)
	{
		int num = BeginAddingRecord(116);
		AppendCharacter(charId);
		AppendLocation(location);
		EndAddingRecord(num);
		return num;
	}
}
