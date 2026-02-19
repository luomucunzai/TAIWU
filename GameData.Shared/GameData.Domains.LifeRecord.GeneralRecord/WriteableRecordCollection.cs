using System;
using GameData.Domains.Map;
using GameData.Serializer;

namespace GameData.Domains.LifeRecord.GeneralRecord;

[SerializableGameData(NotForDisplayModule = true)]
public class WriteableRecordCollection : ReadonlyRecordCollection
{
	private const int DefaultInitialCapacity = 1024;

	public WriteableRecordCollection()
		: this(1024)
	{
	}

	public WriteableRecordCollection(int initialCapacity)
		: base(initialCapacity)
	{
	}

	protected int BeginAddingRecord(short recordType)
	{
		throw new NotImplementedException();
	}

	protected unsafe void EndAddingRecord(int beginOffset)
	{
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

	protected unsafe void AppendCharacter(int charId)
	{
		int size = Size;
		int num = Size + 4;
		EnsureCapacity(num);
		Size = num;
		fixed (byte* rawData = RawData)
		{
			*(int*)(rawData + size) = charId;
		}
	}

	protected unsafe void AppendLocation(Location location)
	{
		int size = Size;
		int num = Size + 2 + 2;
		EnsureCapacity(num);
		Size = num;
		fixed (byte* rawData = RawData)
		{
			byte* num2 = rawData + size;
			*(short*)num2 = location.AreaId;
			((short*)num2)[1] = location.BlockId;
		}
	}

	protected unsafe void AppendItem(sbyte itemType, short itemTemplateId)
	{
		int size = Size;
		int num = Size + 1 + 2;
		EnsureCapacity(num);
		Size = num;
		fixed (byte* rawData = RawData)
		{
			byte* num2 = rawData + size;
			*num2 = (byte)itemType;
			*(short*)(num2 + 1) = itemTemplateId;
		}
	}

	protected unsafe void AppendCombatSkill(short combatSkillTemplateId)
	{
		int size = Size;
		int num = Size + 2;
		EnsureCapacity(num);
		Size = num;
		fixed (byte* rawData = RawData)
		{
			*(short*)(rawData + size) = combatSkillTemplateId;
		}
	}

	protected unsafe void AppendResource(sbyte resourceType)
	{
		int size = Size;
		int num = Size + 1;
		EnsureCapacity(num);
		Size = num;
		fixed (byte* rawData = RawData)
		{
			rawData[size] = (byte)resourceType;
		}
	}

	protected unsafe void AppendSettlement(short settlementId)
	{
		int size = Size;
		int num = Size + 2;
		EnsureCapacity(num);
		Size = num;
		fixed (byte* rawData = RawData)
		{
			*(short*)(rawData + size) = settlementId;
		}
	}

	protected unsafe void AppendOrgGrade(sbyte orgTemplateId, sbyte orgGrade, bool orgPrincipal, sbyte gender)
	{
		int size = Size;
		int num = Size + 1 + 1 + 1 + 1;
		EnsureCapacity(num);
		Size = num;
		fixed (byte* rawData = RawData)
		{
			byte* num2 = rawData + size;
			*num2 = (byte)orgTemplateId;
			num2[1] = (byte)orgGrade;
			(num2 + 1)[1] = (orgPrincipal ? ((byte)1) : ((byte)0));
			(num2 + 1 + 1)[1] = (byte)gender;
		}
	}

	protected unsafe void AppendBuilding(short buildingTemplateId)
	{
		int size = Size;
		int num = Size + 2;
		EnsureCapacity(num);
		Size = num;
		fixed (byte* rawData = RawData)
		{
			*(short*)(rawData + size) = buildingTemplateId;
		}
	}

	protected unsafe void AppendSwordTomb(sbyte xiangshuAvatarId)
	{
		int size = Size;
		int num = Size + 1;
		EnsureCapacity(num);
		Size = num;
		fixed (byte* rawData = RawData)
		{
			rawData[size] = (byte)xiangshuAvatarId;
		}
	}

	protected unsafe void AppendJuniorXiangshu(sbyte xiangshuAvatarId)
	{
		int size = Size;
		int num = Size + 1;
		EnsureCapacity(num);
		Size = num;
		fixed (byte* rawData = RawData)
		{
			rawData[size] = (byte)xiangshuAvatarId;
		}
	}

	protected unsafe void AppendAdventure(short adventureTemplateId)
	{
		int size = Size;
		int num = Size + 2;
		EnsureCapacity(num);
		Size = num;
		fixed (byte* rawData = RawData)
		{
			*(short*)(rawData + size) = adventureTemplateId;
		}
	}

	protected unsafe void AppendBehaviorType(sbyte behaviorType)
	{
		int size = Size;
		int num = Size + 1;
		EnsureCapacity(num);
		Size = num;
		fixed (byte* rawData = RawData)
		{
			rawData[size] = (byte)behaviorType;
		}
	}

	protected unsafe void AppendFavorabilityType(sbyte favorabilityType)
	{
		int size = Size;
		int num = Size + 1;
		EnsureCapacity(num);
		Size = num;
		fixed (byte* rawData = RawData)
		{
			rawData[size] = (byte)favorabilityType;
		}
	}

	protected unsafe void AppendCricket(short colorId, short partId)
	{
		int size = Size;
		int num = Size + 2 + 2;
		EnsureCapacity(num);
		Size = num;
		fixed (byte* rawData = RawData)
		{
			byte* num2 = rawData + size;
			*(short*)num2 = colorId;
			((short*)num2)[1] = partId;
		}
	}

	protected unsafe void AppendItemSubType(short itemSubType)
	{
		int size = Size;
		int num = Size + 2;
		EnsureCapacity(num);
		Size = num;
		fixed (byte* rawData = RawData)
		{
			*(short*)(rawData + size) = itemSubType;
		}
	}

	protected unsafe void AppendChicken(short chickenId)
	{
		int size = Size;
		int num = Size + 2;
		EnsureCapacity(num);
		Size = num;
		fixed (byte* rawData = RawData)
		{
			*(short*)(rawData + size) = chickenId;
		}
	}

	protected unsafe void AppendCharacterPropertyReferencedType(short characterPropertyReferencedType)
	{
		int size = Size;
		int num = Size + 2;
		EnsureCapacity(num);
		Size = num;
		fixed (byte* rawData = RawData)
		{
			*(short*)(rawData + size) = characterPropertyReferencedType;
		}
	}

	protected unsafe void AppendBodyPartType(sbyte bodyPartType)
	{
		int size = Size;
		int num = Size + 1;
		EnsureCapacity(num);
		Size = num;
		fixed (byte* rawData = RawData)
		{
			rawData[size] = (byte)bodyPartType;
		}
	}

	protected unsafe void AppendInjuryType(sbyte injuryType)
	{
		int size = Size;
		int num = Size + 1;
		EnsureCapacity(num);
		Size = num;
		fixed (byte* rawData = RawData)
		{
			rawData[size] = (byte)injuryType;
		}
	}

	protected unsafe void AppendPoisonType(sbyte poisonType)
	{
		int size = Size;
		int num = Size + 1;
		EnsureCapacity(num);
		Size = num;
		fixed (byte* rawData = RawData)
		{
			rawData[size] = (byte)poisonType;
		}
	}

	protected unsafe void AppendCharacterTemplate(short templateId)
	{
		int size = Size;
		int num = Size + 2;
		EnsureCapacity(num);
		Size = num;
		fixed (byte* rawData = RawData)
		{
			*(short*)(rawData + size) = templateId;
		}
	}

	protected unsafe void AppendFeature(short featureId)
	{
		int size = Size;
		int num = Size + 2;
		EnsureCapacity(num);
		Size = num;
		fixed (byte* rawData = RawData)
		{
			*(short*)(rawData + size) = featureId;
		}
	}

	protected unsafe void AppendInteger(int value)
	{
		int size = Size;
		int num = Size + 4;
		EnsureCapacity(num);
		Size = num;
		fixed (byte* rawData = RawData)
		{
			*(int*)(rawData + size) = value;
		}
	}

	protected unsafe void AppendLifeSkill(short lifeSkillTemplateId)
	{
		int size = Size;
		int num = Size + 2;
		EnsureCapacity(num);
		Size = num;
		fixed (byte* rawData = RawData)
		{
			*(short*)(rawData + size) = lifeSkillTemplateId;
		}
	}

	protected unsafe void AppendMerchantType(sbyte merchantType)
	{
		int size = Size;
		int num = Size + 1;
		EnsureCapacity(num);
		Size = num;
		fixed (byte* rawData = RawData)
		{
			rawData[size] = (byte)merchantType;
		}
	}

	protected unsafe void AppendItemKey(ulong itemKey)
	{
		int size = Size;
		int num = Size + 8;
		EnsureCapacity(num);
		Size = num;
		fixed (byte* rawData = RawData)
		{
			*(ulong*)(rawData + size) = itemKey;
		}
	}

	protected unsafe void AppendCombatType(sbyte combatType)
	{
		int size = Size;
		int num = Size + 1;
		EnsureCapacity(num);
		Size = num;
		fixed (byte* rawData = RawData)
		{
			rawData[size] = (byte)combatType;
		}
	}

	protected unsafe void AppendLifeSkillType(sbyte lifeSkillType)
	{
		int size = Size;
		int num = Size + 1;
		EnsureCapacity(num);
		Size = num;
		fixed (byte* rawData = RawData)
		{
			rawData[size] = (byte)lifeSkillType;
		}
	}

	protected unsafe void AppendCombatSkillType(sbyte combatSkillType)
	{
		int size = Size;
		int num = Size + 1;
		EnsureCapacity(num);
		Size = num;
		fixed (byte* rawData = RawData)
		{
			rawData[size] = (byte)combatSkillType;
		}
	}

	protected unsafe void AppendInformation(short infoTemplateId)
	{
		int size = Size;
		int num = Size + 2;
		EnsureCapacity(num);
		Size = num;
		fixed (byte* rawData = RawData)
		{
			*(short*)(rawData + size) = infoTemplateId;
		}
	}

	protected unsafe void AppendSecretInformationTemplate(short secretInfoTemplateId)
	{
		int size = Size;
		int num = Size + 2;
		EnsureCapacity(num);
		Size = num;
		fixed (byte* rawData = RawData)
		{
			*(short*)(rawData + size) = secretInfoTemplateId;
		}
	}

	protected unsafe void AppendPunishmentType(short punishmentType)
	{
		int size = Size;
		int num = Size + 2;
		EnsureCapacity(num);
		Size = num;
		fixed (byte* rawData = RawData)
		{
			*(short*)(rawData + size) = punishmentType;
		}
	}

	protected unsafe void AppendCharacterTitle(short titleTemplateId)
	{
		int size = Size;
		int num = Size + 2;
		EnsureCapacity(num);
		Size = num;
		fixed (byte* rawData = RawData)
		{
			*(short*)(rawData + size) = titleTemplateId;
		}
	}

	protected unsafe void AppendFloat(float floatValue)
	{
		int size = Size;
		int num = Size + 4;
		EnsureCapacity(num);
		Size = num;
		fixed (byte* rawData = RawData)
		{
			*(float*)(rawData + size) = floatValue;
		}
	}

	protected void AppendCharacterRealName(int charId)
	{
		AppendCharacter(charId);
	}

	protected unsafe void AppendMonth(sbyte month)
	{
		int size = Size;
		int num = Size + 1;
		EnsureCapacity(num);
		Size = num;
		fixed (byte* rawData = RawData)
		{
			rawData[size] = (byte)month;
		}
	}

	protected unsafe void AppendProfession(int professionTemplateId)
	{
		int size = Size;
		int num = Size + 4;
		EnsureCapacity(num);
		Size = num;
		fixed (byte* rawData = RawData)
		{
			*(int*)(rawData + size) = professionTemplateId;
		}
	}

	protected unsafe void AppendProfessionSkill(int skillTemplateId)
	{
		int size = Size;
		int num = Size + 4;
		EnsureCapacity(num);
		Size = num;
		fixed (byte* rawData = RawData)
		{
			*(int*)(rawData + size) = skillTemplateId;
		}
	}

	protected unsafe void AppendItemGrade(sbyte grade)
	{
		int size = Size;
		int num = Size + 1;
		EnsureCapacity(num);
		Size = num;
		fixed (byte* rawData = RawData)
		{
			rawData[size] = (byte)grade;
		}
	}

	protected unsafe void AppendText(string value)
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

	protected unsafe void AppendMusic(short musicTemplateId)
	{
		int size = Size;
		int num = Size + 2;
		EnsureCapacity(num);
		Size = num;
		fixed (byte* rawData = RawData)
		{
			*(short*)(rawData + size) = musicTemplateId;
		}
	}

	protected unsafe void AppendMapState(sbyte stateTemplateId)
	{
		int size = Size;
		int num = Size + 1;
		EnsureCapacity(num);
		Size = num;
		fixed (byte* rawData = RawData)
		{
			rawData[size] = (byte)stateTemplateId;
		}
	}

	protected unsafe void AppendJiaoLoong(int jiaoLoongId)
	{
		int size = Size;
		int num = Size + 4;
		EnsureCapacity(num);
		Size = num;
		fixed (byte* rawData = RawData)
		{
			*(int*)(rawData + size) = jiaoLoongId;
		}
	}

	protected unsafe void AppendJiaoProperty(short jiaoPropertyId)
	{
		int size = Size;
		int num = Size + 2;
		EnsureCapacity(num);
		Size = num;
		fixed (byte* rawData = RawData)
		{
			*(short*)(rawData + size) = jiaoPropertyId;
		}
	}

	protected unsafe void AppendDestinyType(sbyte destinyType)
	{
		int size = Size;
		int num = Size + 1;
		EnsureCapacity(num);
		Size = num;
		fixed (byte* rawData = RawData)
		{
			rawData[size] = (byte)destinyType;
		}
	}

	protected unsafe void AppendSecretInformation(short templateId, int id)
	{
		int size = Size;
		int num = Size + 2 + 4;
		EnsureCapacity(num);
		Size = num;
		fixed (byte* rawData = RawData)
		{
			byte* num2 = rawData + size;
			*(short*)num2 = templateId;
			*(int*)(num2 + 2) = id;
		}
	}

	protected unsafe void AppendMerchant(sbyte templateId)
	{
		int size = Size;
		int num = Size + 1;
		EnsureCapacity(num);
		Size = num;
		fixed (byte* rawData = RawData)
		{
			rawData[size] = (byte)templateId;
		}
	}

	protected unsafe void AppendLegacy(short templateId)
	{
		int size = Size;
		int num = Size + 2;
		EnsureCapacity(num);
		Size = num;
		fixed (byte* rawData = RawData)
		{
			*(short*)(rawData + size) = templateId;
		}
	}

	protected unsafe void AppendCharGrade(sbyte grade)
	{
		int size = Size;
		int num = Size + 1;
		EnsureCapacity(num);
		Size = num;
		fixed (byte* rawData = RawData)
		{
			rawData[size] = (byte)grade;
		}
	}

	protected unsafe void AppendFeast(short feast)
	{
		int size = Size;
		int num = Size + 2;
		EnsureCapacity(num);
		Size = num;
		fixed (byte* rawData = RawData)
		{
			*(short*)(rawData + size) = feast;
		}
	}
}
