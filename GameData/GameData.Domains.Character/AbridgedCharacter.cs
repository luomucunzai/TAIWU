using GameData.Domains.Character.AvatarSystem;
using GameData.Domains.Character.Display;
using GameData.Domains.Map;
using GameData.Serializer;

namespace GameData.Domains.Character;

[SerializableGameData(IsExtensible = true, NotForDisplayModule = true)]
public class AbridgedCharacter : ISerializableGameData
{
	private static class FieldIds
	{
		public const ushort Id = 0;

		public const ushort CharTemplateId = 1;

		public const ushort Gender = 2;

		public const ushort MonkType = 3;

		public const ushort Avatar = 4;

		public const ushort ClothingDisplayId = 5;

		public const ushort FullName = 6;

		public const ushort OrganizationInfo = 7;

		public const ushort MonasticTitle = 8;

		public const ushort CustomDisplayNameId = 9;

		public const ushort SelfRelationToTaiwu = 10;

		public const ushort TaiwuRelationToSelf = 11;

		public const ushort AliveState = 12;

		public const ushort CurrAge = 13;

		public const ushort ActualAge = 14;

		public const ushort Location = 15;

		public const ushort BirthDate = 16;

		public const ushort Count = 17;

		public static readonly string[] FieldId2FieldName = new string[17]
		{
			"Id", "CharTemplateId", "Gender", "MonkType", "Avatar", "ClothingDisplayId", "FullName", "OrganizationInfo", "MonasticTitle", "CustomDisplayNameId",
			"SelfRelationToTaiwu", "TaiwuRelationToSelf", "AliveState", "CurrAge", "ActualAge", "Location", "BirthDate"
		};
	}

	[SerializableGameDataField]
	public int Id;

	[SerializableGameDataField]
	public short CharTemplateId;

	[SerializableGameDataField]
	public sbyte Gender;

	[SerializableGameDataField]
	public short CurrAge;

	[SerializableGameDataField]
	public short ActualAge;

	[SerializableGameDataField]
	public byte MonkType;

	[SerializableGameDataField]
	public AvatarData Avatar;

	[SerializableGameDataField]
	public short ClothingDisplayId;

	[SerializableGameDataField]
	public FullName FullName;

	[SerializableGameDataField]
	public OrganizationInfo OrganizationInfo;

	[SerializableGameDataField]
	public MonasticTitle MonasticTitle;

	[SerializableGameDataField]
	public int CustomDisplayNameId;

	[SerializableGameDataField]
	public ushort SelfRelationToTaiwu;

	[SerializableGameDataField]
	public ushort TaiwuRelationToSelf;

	[SerializableGameDataField]
	public sbyte AliveState;

	[SerializableGameDataField]
	public Location Location;

	[SerializableGameDataField]
	public int BirthDate;

	public AvatarRelatedData GenerateAvatarRelatedData()
	{
		return new AvatarRelatedData
		{
			AvatarData = new AvatarData(Avatar),
			DisplayAge = CurrAge,
			ClothingDisplayId = ClothingDisplayId
		};
	}

	public AbridgedCharacter()
	{
		Id = -1;
		Avatar = new AvatarData();
	}

	public AbridgedCharacter(AbridgedCharacter other)
	{
		Id = other.Id;
		CharTemplateId = other.CharTemplateId;
		Gender = other.Gender;
		MonkType = other.MonkType;
		Avatar = new AvatarData(other.Avatar);
		ClothingDisplayId = other.ClothingDisplayId;
		FullName = other.FullName;
		OrganizationInfo = other.OrganizationInfo;
		MonasticTitle = other.MonasticTitle;
		CustomDisplayNameId = other.CustomDisplayNameId;
		SelfRelationToTaiwu = other.SelfRelationToTaiwu;
		TaiwuRelationToSelf = other.TaiwuRelationToSelf;
		AliveState = other.AliveState;
		CurrAge = other.CurrAge;
		ActualAge = other.ActualAge;
		Location = other.Location;
		BirthDate = other.BirthDate;
	}

	public void Assign(AbridgedCharacter other)
	{
		Id = other.Id;
		CharTemplateId = other.CharTemplateId;
		Gender = other.Gender;
		MonkType = other.MonkType;
		Avatar = new AvatarData(other.Avatar);
		ClothingDisplayId = other.ClothingDisplayId;
		FullName = other.FullName;
		OrganizationInfo = other.OrganizationInfo;
		MonasticTitle = other.MonasticTitle;
		CustomDisplayNameId = other.CustomDisplayNameId;
		SelfRelationToTaiwu = other.SelfRelationToTaiwu;
		TaiwuRelationToSelf = other.TaiwuRelationToSelf;
		AliveState = other.AliveState;
		CurrAge = other.CurrAge;
		ActualAge = other.ActualAge;
		Location = other.Location;
		BirthDate = other.BirthDate;
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 131;
		return (num <= 4) ? num : ((num + 3) / 4 * 4);
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		*(short*)ptr = 17;
		ptr += 2;
		*(int*)ptr = Id;
		ptr += 4;
		*(short*)ptr = CharTemplateId;
		ptr += 2;
		*ptr = (byte)Gender;
		ptr++;
		*ptr = MonkType;
		ptr++;
		ptr += Avatar.Serialize(ptr);
		*(short*)ptr = ClothingDisplayId;
		ptr += 2;
		ptr += FullName.Serialize(ptr);
		ptr += OrganizationInfo.Serialize(ptr);
		ptr += MonasticTitle.Serialize(ptr);
		*(int*)ptr = CustomDisplayNameId;
		ptr += 4;
		*(ushort*)ptr = SelfRelationToTaiwu;
		ptr += 2;
		*(ushort*)ptr = TaiwuRelationToSelf;
		ptr += 2;
		*ptr = (byte)AliveState;
		ptr++;
		*(short*)ptr = CurrAge;
		ptr += 2;
		*(short*)ptr = ActualAge;
		ptr += 2;
		ptr += Location.Serialize(ptr);
		*(int*)ptr = BirthDate;
		ptr += 4;
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
			Id = *(int*)ptr;
			ptr += 4;
		}
		if (num > 1)
		{
			CharTemplateId = *(short*)ptr;
			ptr += 2;
		}
		if (num > 2)
		{
			Gender = (sbyte)(*ptr);
			ptr++;
		}
		if (num > 3)
		{
			MonkType = *ptr;
			ptr++;
		}
		if (num > 4)
		{
			if (Avatar == null)
			{
				Avatar = new AvatarData();
			}
			ptr += Avatar.Deserialize(ptr);
		}
		if (num > 5)
		{
			ClothingDisplayId = *(short*)ptr;
			ptr += 2;
		}
		if (num > 6)
		{
			ptr += FullName.Deserialize(ptr);
		}
		if (num > 7)
		{
			ptr += OrganizationInfo.Deserialize(ptr);
		}
		if (num > 8)
		{
			ptr += MonasticTitle.Deserialize(ptr);
		}
		if (num > 9)
		{
			CustomDisplayNameId = *(int*)ptr;
			ptr += 4;
		}
		if (num > 10)
		{
			SelfRelationToTaiwu = *(ushort*)ptr;
			ptr += 2;
		}
		if (num > 11)
		{
			TaiwuRelationToSelf = *(ushort*)ptr;
			ptr += 2;
		}
		if (num > 12)
		{
			AliveState = (sbyte)(*ptr);
			ptr++;
		}
		if (num > 13)
		{
			CurrAge = *(short*)ptr;
			ptr += 2;
		}
		if (num > 14)
		{
			ActualAge = *(short*)ptr;
			ptr += 2;
		}
		if (num > 15)
		{
			ptr += Location.Deserialize(ptr);
		}
		if (num > 16)
		{
			BirthDate = *(int*)ptr;
			ptr += 4;
		}
		int num2 = (int)(ptr - pData);
		return (num2 <= 4) ? num2 : ((num2 + 3) / 4 * 4);
	}
}
