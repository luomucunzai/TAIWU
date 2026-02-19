using GameData.Domains.Character;
using GameData.Domains.Character.Display;
using GameData.Domains.Item;
using GameData.Serializer;
using GameData.Utilities;
using SerializableGameDataSourceGenerator;

namespace GameData.Domains.Taiwu.Display;

[AutoGenerateSerializableGameData(NoCopyConstructors = true, NotForArchive = true)]
public class VillagerRoleCharacterDisplayData : ISerializableGameData
{
	[SerializableGameDataField]
	public int Id;

	[SerializableGameDataField]
	public short RoleTemplateId;

	[SerializableGameDataField]
	public VillagerRoleArrangementDisplayDataWrapper ArrangementDisplayData;

	[SerializableGameDataField]
	public byte Flags;

	[SerializableGameDataField]
	public sbyte AliveState;

	[SerializableGameDataField]
	public short Age;

	[SerializableGameDataField]
	public Personalities Personalities;

	[SerializableGameDataField]
	public CombatSkillShorts CombatSkillAttainments;

	[SerializableGameDataField]
	public LifeSkillShorts LifeSkillAttainments;

	[SerializableGameDataField]
	public CombatSkillShorts CombatSkillQualifications;

	[SerializableGameDataField]
	public LifeSkillShorts LifeSkillQualifications;

	[SerializableGameDataField]
	public NameRelatedData Name;

	[SerializableGameDataField]
	public AvatarRelatedData Avatar;

	[SerializableGameDataField]
	public sbyte ReadBookMaxGrade;

	[SerializableGameDataField]
	public bool MatchVillagerRole;

	[SerializableGameDataField]
	public sbyte LeftPotentialCount;

	[SerializableGameDataField]
	public TemplateKey ItemTemplateKey = TemplateKey.Invalid;

	[SerializableGameDataField]
	public VillagerWorkData VillagerWorkData;

	[SerializableGameDataField]
	public int CollectResourceAmount;

	public const byte FlagInVillage = 1;

	public const byte FlagInMissing = 2;

	public const byte FlagIsLeader = 4;

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 17;
		num = ((ArrangementDisplayData == null) ? (num + 2) : (num + (2 + ArrangementDisplayData.GetSerializedSize())));
		num += Personalities.GetSerializedSize();
		num += CombatSkillAttainments.GetSerializedSize();
		num += LifeSkillAttainments.GetSerializedSize();
		num += CombatSkillQualifications.GetSerializedSize();
		num += LifeSkillQualifications.GetSerializedSize();
		num += Name.GetSerializedSize();
		num = ((Avatar == null) ? (num + 2) : (num + (2 + Avatar.GetSerializedSize())));
		num += ItemTemplateKey.GetSerializedSize();
		num = ((VillagerWorkData == null) ? (num + 2) : (num + (2 + VillagerWorkData.GetSerializedSize())));
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		*(int*)ptr = Id;
		ptr += 4;
		*(short*)ptr = RoleTemplateId;
		ptr += 2;
		if (ArrangementDisplayData != null)
		{
			byte* intPtr = ptr;
			ptr += 2;
			int num = ArrangementDisplayData.Serialize(ptr);
			ptr += num;
			Tester.Assert(num <= 65535);
			*(ushort*)intPtr = (ushort)num;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		*ptr = Flags;
		ptr++;
		*ptr = (byte)AliveState;
		ptr++;
		*(short*)ptr = Age;
		ptr += 2;
		ptr += Personalities.Serialize(ptr);
		ptr += CombatSkillAttainments.Serialize(ptr);
		ptr += LifeSkillAttainments.Serialize(ptr);
		ptr += CombatSkillQualifications.Serialize(ptr);
		ptr += LifeSkillQualifications.Serialize(ptr);
		ptr += Name.Serialize(ptr);
		ptr += Avatar.Serialize(ptr);
		*ptr = (byte)ReadBookMaxGrade;
		ptr++;
		*ptr = (MatchVillagerRole ? ((byte)1) : ((byte)0));
		ptr++;
		*ptr = (byte)LeftPotentialCount;
		ptr++;
		ptr += ItemTemplateKey.Serialize(ptr);
		ptr += VillagerWorkData.Serialize(ptr);
		*(int*)ptr = CollectResourceAmount;
		ptr += 4;
		int num2 = (int)(ptr - pData);
		if (num2 > 4)
		{
			return (num2 + 3) / 4 * 4;
		}
		return num2;
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		Id = *(int*)ptr;
		ptr += 4;
		RoleTemplateId = *(short*)ptr;
		ptr += 2;
		ushort num = *(ushort*)ptr;
		ptr += 2;
		if (num > 0)
		{
			ArrangementDisplayData = new VillagerRoleArrangementDisplayDataWrapper();
			ptr += ArrangementDisplayData.Deserialize(ptr);
		}
		else
		{
			ArrangementDisplayData = null;
		}
		Flags = *ptr;
		ptr++;
		AliveState = (sbyte)(*ptr);
		ptr++;
		Age = *(short*)ptr;
		ptr += 2;
		ptr += Personalities.Deserialize(ptr);
		ptr += CombatSkillAttainments.Deserialize(ptr);
		ptr += LifeSkillAttainments.Deserialize(ptr);
		ptr += CombatSkillQualifications.Deserialize(ptr);
		ptr += LifeSkillQualifications.Deserialize(ptr);
		ptr += Name.Deserialize(ptr);
		Avatar = new AvatarRelatedData();
		ptr += Avatar.Deserialize(ptr);
		ReadBookMaxGrade = (sbyte)(*ptr);
		ptr++;
		MatchVillagerRole = *ptr != 0;
		ptr++;
		LeftPotentialCount = (sbyte)(*ptr);
		ptr++;
		ptr += ItemTemplateKey.Deserialize(ptr);
		VillagerWorkData = new VillagerWorkData();
		ptr += VillagerWorkData.Deserialize(ptr);
		CollectResourceAmount = *(int*)ptr;
		ptr += 4;
		int num2 = (int)(ptr - pData);
		if (num2 > 4)
		{
			return (num2 + 3) / 4 * 4;
		}
		return num2;
	}
}
