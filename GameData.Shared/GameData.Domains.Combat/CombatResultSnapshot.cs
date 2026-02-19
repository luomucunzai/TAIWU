using GameData.Domains.Character;
using GameData.Serializer;

namespace GameData.Domains.Combat;

[SerializableGameData(NotForArchive = true)]
public struct CombatResultSnapshot : ISerializableGameData
{
	[SerializableGameDataField]
	public int Exp;

	[SerializableGameDataField]
	public ResourceInts Resource;

	[SerializableGameDataField]
	public int AreaSpiritualDebt;

	[SerializableGameDataField]
	public sbyte CanEatingMaxCount;

	[SerializableGameDataField]
	public EatingItems EatingItemList;

	[SerializableGameDataField]
	public Injuries Injuries;

	[SerializableGameDataField]
	public PoisonInts Poisons;

	[SerializableGameDataField]
	public PoisonInts PoisonResists;

	[SerializableGameDataField]
	public byte ImmunePoisonExtra;

	[SerializableGameDataField]
	public MainAttributes MainAttribute;

	[SerializableGameDataField]
	public short DisorderOfQi;

	[SerializableGameDataField]
	public short ChangeOfQiDisorder;

	[SerializableGameDataField]
	public short TemplateId;

	[SerializableGameDataField]
	public short DisplayAge;

	[SerializableGameDataField]
	public short ActualAge;

	[SerializableGameDataField]
	public sbyte BirthMonth;

	[SerializableGameDataField]
	public short Health;

	[SerializableGameDataField]
	public short LeftMaxHealth;

	[SerializableGameDataField]
	public short HealthRecovery;

	[SerializableGameDataField]
	public byte CreatingType;

	public bool IsSerializedSizeFixed()
	{
		return true;
	}

	public int GetSerializedSize()
	{
		int num = 226;
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		*(int*)ptr = Exp;
		ptr += 4;
		ptr += Resource.Serialize(ptr);
		*(int*)ptr = AreaSpiritualDebt;
		ptr += 4;
		*ptr = (byte)CanEatingMaxCount;
		ptr++;
		ptr += EatingItemList.Serialize(ptr);
		ptr += Injuries.Serialize(ptr);
		ptr += Poisons.Serialize(ptr);
		ptr += PoisonResists.Serialize(ptr);
		*ptr = ImmunePoisonExtra;
		ptr++;
		ptr += MainAttribute.Serialize(ptr);
		*(short*)ptr = DisorderOfQi;
		ptr += 2;
		*(short*)ptr = ChangeOfQiDisorder;
		ptr += 2;
		*(short*)ptr = TemplateId;
		ptr += 2;
		*(short*)ptr = DisplayAge;
		ptr += 2;
		*(short*)ptr = ActualAge;
		ptr += 2;
		*ptr = (byte)BirthMonth;
		ptr++;
		*(short*)ptr = Health;
		ptr += 2;
		*(short*)ptr = LeftMaxHealth;
		ptr += 2;
		*(short*)ptr = HealthRecovery;
		ptr += 2;
		*ptr = CreatingType;
		ptr++;
		int num = (int)(ptr - pData);
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		Exp = *(int*)ptr;
		ptr += 4;
		ptr += Resource.Deserialize(ptr);
		AreaSpiritualDebt = *(int*)ptr;
		ptr += 4;
		CanEatingMaxCount = (sbyte)(*ptr);
		ptr++;
		ptr += EatingItemList.Deserialize(ptr);
		ptr += Injuries.Deserialize(ptr);
		ptr += Poisons.Deserialize(ptr);
		ptr += PoisonResists.Deserialize(ptr);
		ImmunePoisonExtra = *ptr;
		ptr++;
		ptr += MainAttribute.Deserialize(ptr);
		DisorderOfQi = *(short*)ptr;
		ptr += 2;
		ChangeOfQiDisorder = *(short*)ptr;
		ptr += 2;
		TemplateId = *(short*)ptr;
		ptr += 2;
		DisplayAge = *(short*)ptr;
		ptr += 2;
		ActualAge = *(short*)ptr;
		ptr += 2;
		BirthMonth = (sbyte)(*ptr);
		ptr++;
		Health = *(short*)ptr;
		ptr += 2;
		LeftMaxHealth = *(short*)ptr;
		ptr += 2;
		HealthRecovery = *(short*)ptr;
		ptr += 2;
		CreatingType = *ptr;
		ptr++;
		int num = (int)(ptr - pData);
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}
}
