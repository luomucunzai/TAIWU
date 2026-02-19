using GameData.Domains.Character;
using GameData.Domains.Character.Display;
using GameData.Serializer;
using GameData.Utilities;
using SerializableGameDataSourceGenerator;

namespace GameData.Domains.Taiwu;

[AutoGenerateSerializableGameData(IsExtensible = true, NoCopyConstructors = true, NotForDisplayModule = true)]
public class VillagerRoleRecordElement : ISerializableGameData
{
	public static class FieldIds
	{
		public const ushort CharacterId = 0;

		public const ushort CharacterTemplateId = 1;

		public const ushort RoleTemplateId = 2;

		public const ushort Personalities = 3;

		public const ushort CombatSkillAttainments = 4;

		public const ushort LifeSkillAttainments = 5;

		public const ushort Avatar = 6;

		public const ushort Name = 7;

		public const ushort Date = 8;

		public const ushort Count = 9;

		public static readonly string[] FieldId2FieldName = new string[9] { "CharacterId", "CharacterTemplateId", "RoleTemplateId", "Personalities", "CombatSkillAttainments", "LifeSkillAttainments", "Avatar", "Name", "Date" };
	}

	[SerializableGameDataField(FieldIndex = 0)]
	public int CharacterId;

	[SerializableGameDataField(FieldIndex = 1)]
	public short CharacterTemplateId;

	[SerializableGameDataField(FieldIndex = 2)]
	public short RoleTemplateId;

	[SerializableGameDataField(FieldIndex = 3)]
	public Personalities Personalities;

	[SerializableGameDataField(FieldIndex = 4)]
	public CombatSkillShorts CombatSkillAttainments;

	[SerializableGameDataField(FieldIndex = 5)]
	public LifeSkillShorts LifeSkillAttainments;

	[SerializableGameDataField(FieldIndex = 6)]
	public AvatarRelatedData Avatar;

	[SerializableGameDataField(FieldIndex = 7)]
	public NameRelatedData Name;

	[SerializableGameDataField(FieldIndex = 8)]
	public int Date;

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 14;
		num += Personalities.GetSerializedSize();
		num += CombatSkillAttainments.GetSerializedSize();
		num += LifeSkillAttainments.GetSerializedSize();
		num = ((Avatar == null) ? (num + 2) : (num + (2 + Avatar.GetSerializedSize())));
		num += Name.GetSerializedSize();
		return (num <= 4) ? num : ((num + 3) / 4 * 4);
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		*(short*)ptr = 9;
		ptr += 2;
		*(int*)ptr = CharacterId;
		ptr += 4;
		*(short*)ptr = CharacterTemplateId;
		ptr += 2;
		*(short*)ptr = RoleTemplateId;
		ptr += 2;
		int num = Personalities.Serialize(ptr);
		ptr += num;
		Tester.Assert(num <= 65535);
		int num2 = CombatSkillAttainments.Serialize(ptr);
		ptr += num2;
		Tester.Assert(num2 <= 65535);
		int num3 = LifeSkillAttainments.Serialize(ptr);
		ptr += num3;
		Tester.Assert(num3 <= 65535);
		if (Avatar != null)
		{
			byte* ptr2 = ptr;
			ptr += 2;
			int num4 = Avatar.Serialize(ptr);
			ptr += num4;
			Tester.Assert(num4 <= 65535);
			*(ushort*)ptr2 = (ushort)num4;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		int num5 = Name.Serialize(ptr);
		ptr += num5;
		Tester.Assert(num5 <= 65535);
		*(int*)ptr = Date;
		ptr += 4;
		int num6 = (int)(ptr - pData);
		return (num6 <= 4) ? num6 : ((num6 + 3) / 4 * 4);
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		ushort num = *(ushort*)ptr;
		ptr += 2;
		if (num > 0)
		{
			CharacterId = *(int*)ptr;
			ptr += 4;
		}
		if (num > 1)
		{
			CharacterTemplateId = *(short*)ptr;
			ptr += 2;
		}
		if (num > 2)
		{
			RoleTemplateId = *(short*)ptr;
			ptr += 2;
		}
		if (num > 3)
		{
			ptr += Personalities.Deserialize(ptr);
		}
		if (num > 4)
		{
			ptr += CombatSkillAttainments.Deserialize(ptr);
		}
		if (num > 5)
		{
			ptr += LifeSkillAttainments.Deserialize(ptr);
		}
		if (num > 6)
		{
			ushort num2 = *(ushort*)ptr;
			ptr += 2;
			if (num2 > 0)
			{
				Avatar = new AvatarRelatedData();
				ptr += Avatar.Deserialize(ptr);
			}
			else
			{
				Avatar = null;
			}
		}
		if (num > 7)
		{
			ptr += Name.Deserialize(ptr);
		}
		if (num > 8)
		{
			Date = *(int*)ptr;
			ptr += 4;
		}
		int num3 = (int)(ptr - pData);
		return (num3 <= 4) ? num3 : ((num3 + 3) / 4 * 4);
	}
}
