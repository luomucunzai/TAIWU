using System;
using System.Collections.Generic;
using GameData.Domains.Item;
using GameData.Domains.Map;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.LifeRecord.GeneralRecord;

[SerializableGameData(NotForDisplayModule = true)]
public class ReadonlyRecordCollection : RawDataBlock, IBinary, ISerializableGameData
{
	public int Count;

	public ReadonlyRecordCollection()
	{
		Count = 0;
	}

	public ReadonlyRecordCollection(int initialCapacity)
		: base(initialCapacity)
	{
		Count = 0;
	}

	public new bool IsSerializedSizeFixed()
	{
		return false;
	}

	public new int GetSerializedSize()
	{
		return base.GetSerializedSize() + 4;
	}

	public new unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		ptr += base.Serialize(ptr);
		*(int*)ptr = Count;
		ptr += 4;
		return (int)(ptr - pData);
	}

	public new unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		ptr += base.Deserialize(ptr);
		Count = *(int*)ptr;
		ptr += 4;
		return (int)(ptr - pData);
	}

	public new void Clear()
	{
		Size = 0;
		Count = 0;
	}

	public new ushort GetSerializedFixedSizeOfMetadata()
	{
		return 8;
	}

	public new unsafe int SerializeMetadata(byte* pData)
	{
		*(int*)pData = Size;
		((int*)pData)[1] = Count;
		return 8;
	}

	public new unsafe int DeserializeMetadata(byte* pData)
	{
		Size = *(int*)pData;
		Count = ((int*)pData)[1];
		EnsureCapacity(Size);
		return 8;
	}

	public bool Next(ref int index, ref int offset)
	{
		if (index < 0)
		{
			if (Count > 0)
			{
				index = 0;
				offset = 0;
				return true;
			}
			return false;
		}
		if (++index >= Count)
		{
			return false;
		}
		byte b = RawData[offset];
		offset += b;
		return true;
	}

	public int Next(int offset)
	{
		if (offset < 0)
		{
			if (Size <= 0)
			{
				return -1;
			}
			return 0;
		}
		byte b = RawData[offset];
		offset += b;
		if (offset >= Size)
		{
			return -1;
		}
		return offset;
	}

	public int GetRecordSize(int offset)
	{
		return RawData[offset];
	}

	public void GetRenderInfos(List<RenderInfo> renderInfos, ArgumentCollection argumentCollection)
	{
		int index = -1;
		int offset = -1;
		while (Next(ref index, ref offset))
		{
			RenderInfo renderInfo = GetRenderInfo(offset, argumentCollection);
			if (renderInfo != null)
			{
				renderInfos.Add(renderInfo);
			}
		}
	}

	public RenderInfo GetRenderInfo(int offset, ArgumentCollection argumentCollection)
	{
		throw new NotImplementedException();
	}

	protected unsafe static int ReadArgumentAndGetIndex(sbyte paramType, byte** ppData, ArgumentCollection argumentCollection)
	{
		switch (paramType)
		{
		case 0:
		{
			int charId2 = ReadCharacter(ppData);
			return argumentCollection.AddCharacter(charId2);
		}
		case 1:
		{
			Location location = ReadLocation(ppData);
			return argumentCollection.AddLocation(location);
		}
		case 2:
			var (itemType, itemTemplateId) = ReadItem(ppData);
			return argumentCollection.AddItem(itemType, itemTemplateId);
		case 3:
		{
			short combatSkillId = ReadCombatSkill(ppData);
			return argumentCollection.AddCombatSkill(combatSkillId);
		}
		case 4:
		{
			sbyte resourceType = ReadResource(ppData);
			return argumentCollection.AddResource(resourceType);
		}
		case 5:
		{
			short settlementId = ReadSettlement(ppData);
			return argumentCollection.AddSettlement(settlementId);
		}
		case 6:
			var (orgTemplateId, orgGrade, orgPrincipal, gender) = ReadOrgGrade(ppData);
			return argumentCollection.AddOrgGrade(orgTemplateId, orgGrade, orgPrincipal, gender);
		case 7:
		{
			short buildingTemplateId = ReadBuilding(ppData);
			return argumentCollection.AddBuilding(buildingTemplateId);
		}
		case 8:
		{
			sbyte xiangshuAvatarId2 = ReadSwordTomb(ppData);
			return argumentCollection.AddSwordTomb(xiangshuAvatarId2);
		}
		case 9:
		{
			sbyte xiangshuAvatarId = ReadJuniorXiangshu(ppData);
			return argumentCollection.AddJuniorXiangshu(xiangshuAvatarId);
		}
		case 10:
		{
			short adventureTemplateId = ReadAdventure(ppData);
			return argumentCollection.AddAdventure(adventureTemplateId);
		}
		case 11:
		{
			sbyte behaviorType = ReadBehaviorType(ppData);
			return argumentCollection.AddBehaviorType(behaviorType);
		}
		case 12:
		{
			sbyte favorabilityType = ReadFavorabilityType(ppData);
			return argumentCollection.AddFavorabilityType(favorabilityType);
		}
		case 13:
			var (colorId, partId) = ReadCricket(ppData);
			return argumentCollection.AddCricket(colorId, partId);
		case 14:
		{
			short itemSubType = ReadItemSubType(ppData);
			return argumentCollection.AddItemSubType(itemSubType);
		}
		case 15:
		{
			short chickenId = ReadChicken(ppData);
			return argumentCollection.AddChicken(chickenId);
		}
		case 16:
		{
			short characterPropertyReferencedType = ReadCharacterPropertyReferencedType(ppData);
			return argumentCollection.AddCharacterPropertyReferencedType(characterPropertyReferencedType);
		}
		case 17:
		{
			sbyte bodyPartType = ReadBodyPartType(ppData);
			return argumentCollection.AddBodyPartType(bodyPartType);
		}
		case 18:
		{
			sbyte injuryType = ReadInjuryType(ppData);
			return argumentCollection.AddInjuryType(injuryType);
		}
		case 19:
		{
			sbyte poisonType = ReadPoisonType(ppData);
			return argumentCollection.AddPoisonType(poisonType);
		}
		case 20:
		{
			short templateId4 = ReadCharacterTemplate(ppData);
			return argumentCollection.AddCharacterTemplate(templateId4);
		}
		case 21:
		{
			short featureId = ReadFeature(ppData);
			return argumentCollection.AddFeature(featureId);
		}
		case 22:
		{
			int value = ReadInteger(ppData);
			return argumentCollection.AddInteger(value);
		}
		case 23:
		{
			short lifeSkillTemplateId = ReadLifeSkill(ppData);
			return argumentCollection.AddLifeSkill(lifeSkillTemplateId);
		}
		case 24:
		{
			sbyte merchantType = ReadMerchantType(ppData);
			return argumentCollection.AddMerchantType(merchantType);
		}
		case 25:
		{
			ulong itemKey = ReadItemKey(ppData);
			return argumentCollection.AddItemKey(itemKey);
		}
		case 26:
		{
			sbyte combatType = ReadCombatType(ppData);
			return argumentCollection.AddCombatType(combatType);
		}
		case 27:
		{
			sbyte lifeSkillType = ReadLifeSkillType(ppData);
			return argumentCollection.AddLifeSkillType(lifeSkillType);
		}
		case 28:
		{
			sbyte combatSkillType = ReadCombatSkillType(ppData);
			return argumentCollection.AddCombatSkillType(combatSkillType);
		}
		case 29:
		{
			short infoTemplateId = ReadInformation(ppData);
			return argumentCollection.AddInformation(infoTemplateId);
		}
		case 30:
		{
			short secretInfoTemplateId = ReadSecretInformationTemplate(ppData);
			return argumentCollection.AddSecretInformationTemplate(secretInfoTemplateId);
		}
		case 31:
		{
			short punishmentType = ReadPunishmentType(ppData);
			return argumentCollection.AddPunishmentType(punishmentType);
		}
		case 32:
		{
			short titleTemplateId = ReadCharacterTitle(ppData);
			return argumentCollection.AddCharacterTitle(titleTemplateId);
		}
		case 33:
		{
			float floatValue = ReadFloat(ppData);
			return argumentCollection.AddFloat(floatValue);
		}
		case 34:
		{
			int charId = ReadCharacter(ppData);
			return argumentCollection.AddCharacterRealName(charId);
		}
		case 35:
		{
			sbyte month = ReadMonth(ppData);
			return argumentCollection.AddMonth(month);
		}
		case 36:
		{
			int professionTemplateId = ReadInt(ppData);
			return argumentCollection.AddProfession(professionTemplateId);
		}
		case 37:
		{
			int skillTemplateId = ReadInt(ppData);
			return argumentCollection.AddProfessionSkill(skillTemplateId);
		}
		case 38:
		{
			sbyte grade2 = ReadSByte(ppData);
			return argumentCollection.AddItemGrade(grade2);
		}
		case 39:
		{
			string text = ReadText(ppData);
			return argumentCollection.AddText(text);
		}
		case 40:
		{
			short musicTemplateId = ReadGeneric<short>(ppData);
			return argumentCollection.AddMusic(musicTemplateId);
		}
		case 41:
		{
			sbyte stateTemplateId = ReadGeneric<sbyte>(ppData);
			return argumentCollection.AddMapState(stateTemplateId);
		}
		case 42:
		{
			int jiaoLoongId = ReadGeneric<int>(ppData);
			return argumentCollection.AddJiaoLoong(jiaoLoongId);
		}
		case 43:
		{
			short jiaoPropertyId = ReadGeneric<short>(ppData);
			return argumentCollection.AddJiaoProperty(jiaoPropertyId);
		}
		case 44:
		{
			sbyte destinyType = ReadGeneric<sbyte>(ppData);
			return argumentCollection.AddDestinyType(destinyType);
		}
		case 45:
			var (templateId3, id) = ReadSecretInformation(ppData);
			return argumentCollection.AddSecretInformation(templateId3, id);
		case 46:
		{
			sbyte templateId2 = ReadGeneric<sbyte>(ppData);
			return argumentCollection.AddMerchant(templateId2);
		}
		case 47:
		{
			short templateId = ReadGeneric<short>(ppData);
			return argumentCollection.AddLegacy(templateId);
		}
		case 48:
		{
			sbyte grade = ReadGeneric<sbyte>(ppData);
			return argumentCollection.AddCharGrade(grade);
		}
		case 49:
		{
			sbyte feast = ReadGeneric<sbyte>(ppData);
			return argumentCollection.AddFeast(feast);
		}
		default:
			throw new Exception($"Unsupported ParameterType: {paramType}");
		}
	}

	private unsafe static int ReadCharacter(byte** ppData)
	{
		int result = *(int*)(*ppData);
		*ppData += 4;
		return result;
	}

	private unsafe static Location ReadLocation(byte** ppData)
	{
		short areaId = *(short*)(*ppData);
		short blockId = ((short*)(*ppData))[1];
		*ppData += 4;
		return new Location(areaId, blockId);
	}

	private unsafe static (sbyte itemType, short itemTemplateId) ReadItem(byte** ppData)
	{
		byte item = *(*ppData);
		short item2 = *(short*)(*ppData + 1);
		*ppData += 3;
		return (itemType: (sbyte)item, itemTemplateId: item2);
	}

	private unsafe static short ReadCombatSkill(byte** ppData)
	{
		short result = *(short*)(*ppData);
		*ppData += 2;
		return result;
	}

	private unsafe static sbyte ReadResource(byte** ppData)
	{
		byte result = *(*ppData);
		(*ppData)++;
		return (sbyte)result;
	}

	private unsafe static short ReadSettlement(byte** ppData)
	{
		short result = *(short*)(*ppData);
		*ppData += 2;
		return result;
	}

	private unsafe static (sbyte orgTemplateId, sbyte orgGrade, bool orgPrincipal, sbyte gender) ReadOrgGrade(byte** ppData)
	{
		byte item = *(*ppData);
		sbyte item2 = (sbyte)(*ppData)[1];
		bool item3 = (*ppData + 1)[1] != 0;
		sbyte item4 = (sbyte)(*ppData + 1 + 1)[1];
		*ppData += 4;
		return (orgTemplateId: (sbyte)item, orgGrade: item2, orgPrincipal: item3, gender: item4);
	}

	private unsafe static short ReadBuilding(byte** ppData)
	{
		short result = *(short*)(*ppData);
		*ppData += 2;
		return result;
	}

	private unsafe static sbyte ReadSwordTomb(byte** ppData)
	{
		byte result = *(*ppData);
		(*ppData)++;
		return (sbyte)result;
	}

	private unsafe static sbyte ReadJuniorXiangshu(byte** ppData)
	{
		byte result = *(*ppData);
		(*ppData)++;
		return (sbyte)result;
	}

	private unsafe static short ReadAdventure(byte** ppData)
	{
		short result = *(short*)(*ppData);
		*ppData += 2;
		return result;
	}

	private unsafe static sbyte ReadBehaviorType(byte** ppData)
	{
		byte result = *(*ppData);
		(*ppData)++;
		return (sbyte)result;
	}

	private unsafe static sbyte ReadFavorabilityType(byte** ppData)
	{
		byte result = *(*ppData);
		(*ppData)++;
		return (sbyte)result;
	}

	private unsafe static (short colorId, short partId) ReadCricket(byte** ppData)
	{
		short item = *(short*)(*ppData);
		short item2 = ((short*)(*ppData))[1];
		*ppData += 4;
		return (colorId: item, partId: item2);
	}

	private unsafe static short ReadItemSubType(byte** ppData)
	{
		short result = *(short*)(*ppData);
		*ppData += 2;
		return result;
	}

	private unsafe static short ReadChicken(byte** ppData)
	{
		short result = *(short*)(*ppData);
		*ppData += 2;
		return result;
	}

	private unsafe static short ReadCharacterPropertyReferencedType(byte** ppData)
	{
		short result = *(short*)(*ppData);
		*ppData += 2;
		return result;
	}

	private unsafe static sbyte ReadBodyPartType(byte** ppData)
	{
		byte result = *(*ppData);
		(*ppData)++;
		return (sbyte)result;
	}

	private unsafe static sbyte ReadInjuryType(byte** ppData)
	{
		byte result = *(*ppData);
		(*ppData)++;
		return (sbyte)result;
	}

	private unsafe static sbyte ReadPoisonType(byte** ppData)
	{
		byte result = *(*ppData);
		(*ppData)++;
		return (sbyte)result;
	}

	private unsafe static short ReadCharacterTemplate(byte** ppData)
	{
		short result = *(short*)(*ppData);
		*ppData += 2;
		return result;
	}

	private unsafe static short ReadFeature(byte** ppData)
	{
		short result = *(short*)(*ppData);
		*ppData += 2;
		return result;
	}

	private unsafe static int ReadInteger(byte** ppData)
	{
		int result = *(int*)(*ppData);
		*ppData += 4;
		return result;
	}

	private unsafe static short ReadLifeSkill(byte** ppData)
	{
		short result = *(short*)(*ppData);
		*ppData += 2;
		return result;
	}

	private unsafe static sbyte ReadMerchantType(byte** ppData)
	{
		byte result = *(*ppData);
		(*ppData)++;
		return (sbyte)result;
	}

	private unsafe static ulong ReadItemKey(byte** ppData)
	{
		long result = *(long*)(*ppData);
		*ppData += 8;
		return (ulong)result;
	}

	private unsafe static sbyte ReadCombatType(byte** ppData)
	{
		byte result = *(*ppData);
		(*ppData)++;
		return (sbyte)result;
	}

	private unsafe static sbyte ReadLifeSkillType(byte** ppData)
	{
		byte result = *(*ppData);
		(*ppData)++;
		return (sbyte)result;
	}

	private unsafe static sbyte ReadCombatSkillType(byte** ppData)
	{
		byte result = *(*ppData);
		(*ppData)++;
		return (sbyte)result;
	}

	private unsafe static short ReadInformation(byte** ppData)
	{
		short result = *(short*)(*ppData);
		*ppData += 2;
		return result;
	}

	private unsafe static short ReadSecretInformationTemplate(byte** ppData)
	{
		short result = *(short*)(*ppData);
		*ppData += 2;
		return result;
	}

	private unsafe static short ReadPunishmentType(byte** ppData)
	{
		short result = *(short*)(*ppData);
		*ppData += 2;
		return result;
	}

	private unsafe static short ReadCharacterTitle(byte** ppData)
	{
		short result = *(short*)(*ppData);
		*ppData += 2;
		return result;
	}

	private unsafe static float ReadFloat(byte** ppData)
	{
		float result = *(float*)(*ppData);
		*ppData += 4;
		return result;
	}

	private unsafe static sbyte ReadMonth(byte** ppData)
	{
		byte result = *(*ppData);
		(*ppData)++;
		return (sbyte)result;
	}

	private unsafe static int ReadInt(byte** ppData)
	{
		int result = *(int*)(*ppData);
		*ppData += 4;
		return result;
	}

	private unsafe static sbyte ReadSByte(byte** ppData)
	{
		byte result = *(*ppData);
		(*ppData)++;
		return (sbyte)result;
	}

	private unsafe static T ReadGeneric<T>(byte** ppData) where T : unmanaged
	{
		T result = *(T*)(*ppData);
		*ppData += sizeof(T);
		return result;
	}

	private unsafe static string ReadText(byte** ppData)
	{
		string result = default(string);
		int num = SerializationHelper.Deserialize(*ppData, ref result);
		*ppData += num;
		return result;
	}

	public unsafe static (short templateId, int id) ReadSecretInformation(byte** ppData)
	{
		short item = *(short*)(*ppData);
		int item2 = *(int*)(*ppData + 2);
		*ppData += 6;
		return (templateId: item, id: item2);
	}

	public virtual void FillEventArgBox(int offset, IVariantCollection<string> eventArgBox)
	{
	}

	protected unsafe void ReadArgumentToEventArgBox(string keyPrefix, int argIndex, sbyte paramType, byte** ppData, IVariantCollection<string> argBox)
	{
		string text = $"{keyPrefix}{argIndex}";
		switch (paramType)
		{
		case 0:
		{
			int num29 = ReadCharacter(ppData);
			argBox.Set(text, num29);
			break;
		}
		case 1:
		{
			Location location = ReadLocation(ppData);
			argBox.Set(text, (ISerializableGameData)(object)location);
			break;
		}
		case 2:
			var (b20, num28) = ReadItem(ppData);
			argBox.Set(text + "_itemType", (int)b20);
			argBox.Set(text + "_itemTemplateId", (int)num28);
			break;
		case 3:
		{
			short num27 = ReadCombatSkill(ppData);
			argBox.Set(text, (int)num27);
			break;
		}
		case 4:
		{
			sbyte b19 = ReadResource(ppData);
			argBox.Set(text, (int)b19);
			break;
		}
		case 5:
		{
			short num26 = ReadSettlement(ppData);
			argBox.Set(text, (int)num26);
			break;
		}
		case 6:
			var (b16, b17, flag, b18) = ReadOrgGrade(ppData);
			argBox.Set(text + "_orgTemplateId", (int)b16);
			argBox.Set(text + "_orgGrade", (int)b17);
			argBox.Set(text + "_orgPrincipal", flag);
			argBox.Set(text + "_gender", (int)b18);
			break;
		case 7:
		{
			short num25 = ReadBuilding(ppData);
			argBox.Set(text, (int)num25);
			break;
		}
		case 8:
		{
			sbyte b15 = ReadSwordTomb(ppData);
			argBox.Set(text, (int)b15);
			break;
		}
		case 9:
		{
			sbyte b14 = ReadJuniorXiangshu(ppData);
			argBox.Set(text, (int)b14);
			break;
		}
		case 10:
		{
			short num24 = ReadAdventure(ppData);
			argBox.Set(text, (int)num24);
			break;
		}
		case 11:
		{
			sbyte b13 = ReadBehaviorType(ppData);
			argBox.Set(text, (int)b13);
			break;
		}
		case 12:
		{
			sbyte b12 = ReadFavorabilityType(ppData);
			argBox.Set(text, (int)b12);
			break;
		}
		case 13:
			var (num22, num23) = ReadCricket(ppData);
			argBox.Set(text + "_colorId", (int)num22);
			argBox.Set(text + "_partId", (int)num23);
			break;
		case 14:
		{
			short num21 = ReadItemSubType(ppData);
			argBox.Set(text, (int)num21);
			break;
		}
		case 15:
		{
			short num20 = ReadChicken(ppData);
			argBox.Set(text, (int)num20);
			break;
		}
		case 16:
		{
			short num19 = ReadCharacterPropertyReferencedType(ppData);
			argBox.Set(text, (int)num19);
			break;
		}
		case 17:
		{
			sbyte b11 = ReadBodyPartType(ppData);
			argBox.Set(text, (int)b11);
			break;
		}
		case 18:
		{
			sbyte b10 = ReadInjuryType(ppData);
			argBox.Set(text, (int)b10);
			break;
		}
		case 19:
		{
			sbyte b9 = ReadPoisonType(ppData);
			argBox.Set(text, (int)b9);
			break;
		}
		case 20:
		{
			short num18 = ReadCharacterTemplate(ppData);
			argBox.Set(text, (int)num18);
			break;
		}
		case 21:
		{
			short num17 = ReadFeature(ppData);
			argBox.Set(text, (int)num17);
			break;
		}
		case 22:
		{
			int num16 = ReadInteger(ppData);
			argBox.Set(text, num16);
			break;
		}
		case 23:
		{
			short num15 = ReadLifeSkill(ppData);
			argBox.Set(text, (int)num15);
			break;
		}
		case 24:
		{
			sbyte b8 = ReadMerchantType(ppData);
			argBox.Set(text, (int)b8);
			break;
		}
		case 25:
		{
			ulong num14 = ReadItemKey(ppData);
			argBox.Set(text, (ISerializableGameData)(object)(ItemKey)num14);
			break;
		}
		case 26:
		{
			sbyte b7 = ReadCombatType(ppData);
			argBox.Set(text, (int)b7);
			break;
		}
		case 27:
		{
			sbyte b6 = ReadLifeSkillType(ppData);
			argBox.Set(text, (int)b6);
			break;
		}
		case 28:
		{
			sbyte b5 = ReadCombatSkillType(ppData);
			argBox.Set(text, (int)b5);
			break;
		}
		case 29:
		{
			short num13 = ReadInformation(ppData);
			argBox.Set(text, (int)num13);
			break;
		}
		case 30:
		{
			short num12 = ReadSecretInformationTemplate(ppData);
			argBox.Set(text, (int)num12);
			break;
		}
		case 31:
		{
			short num11 = ReadPunishmentType(ppData);
			argBox.Set(text, (int)num11);
			break;
		}
		case 32:
		{
			short num10 = ReadCharacterTitle(ppData);
			argBox.Set(text, (int)num10);
			break;
		}
		case 33:
		{
			float num9 = ReadFloat(ppData);
			argBox.Set(text, num9);
			break;
		}
		case 34:
		{
			int num8 = ReadCharacter(ppData);
			argBox.Set(text, num8);
			break;
		}
		case 35:
		{
			sbyte b4 = ReadMonth(ppData);
			argBox.Set(text, (int)b4);
			break;
		}
		case 36:
		{
			int num7 = ReadInt(ppData);
			argBox.Set(text, num7);
			break;
		}
		case 37:
		{
			int num6 = ReadInt(ppData);
			argBox.Set(text, num6);
			break;
		}
		case 38:
		{
			sbyte b3 = ReadSByte(ppData);
			argBox.Set(text, (int)b3);
			break;
		}
		case 39:
		{
			string text2 = ReadText(ppData);
			argBox.Set(text, text2);
			break;
		}
		case 40:
		{
			short num5 = ReadGeneric<short>(ppData);
			argBox.Set(text, (int)num5);
			break;
		}
		case 41:
		{
			sbyte b2 = ReadGeneric<sbyte>(ppData);
			argBox.Set(text, (int)b2);
			break;
		}
		case 42:
		{
			int num4 = ReadGeneric<int>(ppData);
			argBox.Set(text, num4);
			break;
		}
		case 43:
		{
			short num3 = ReadGeneric<short>(ppData);
			argBox.Set(text, (int)num3);
			break;
		}
		case 44:
		{
			sbyte b = ReadGeneric<sbyte>(ppData);
			argBox.Set(text, (int)b);
			break;
		}
		case 45:
			var (num, num2) = ReadSecretInformation(ppData);
			argBox.Set(text + "_templateId", (int)num);
			argBox.Set(text + "_id", num2);
			break;
		default:
			throw new Exception($"Unsupported ParameterType: {paramType}");
		}
	}
}
