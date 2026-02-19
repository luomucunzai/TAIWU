using GameData.Domains.Character;
using GameData.Serializer;

namespace GameData.Domains.Organization;

[SerializableGameData(IsExtensible = true)]
public class SettlementPrisoner : KidnappedCharacter, ISerializableGameData
{
	private static class FieldIds
	{
		public const ushort Duration = 0;

		public const ushort PunishmentType = 1;

		public const ushort PunishmentSeverity = 2;

		public const ushort CharId = 3;

		public const ushort RopeItemKey = 4;

		public const ushort KidnapBeginDate = 5;

		public const ushort Resistance = 6;

		public const ushort InitialMorality = 7;

		public const ushort Count = 8;

		public static readonly string[] FieldId2FieldName = new string[8] { "Duration", "PunishmentType", "PunishmentSeverity", "CharId", "RopeItemKey", "KidnapBeginDate", "Resistance", "InitialMorality" };
	}

	[SerializableGameDataField]
	public int Duration;

	[SerializableGameDataField]
	public short PunishmentType;

	[SerializableGameDataField]
	public sbyte PunishmentSeverity;

	[SerializableGameDataField]
	public short InitialMorality;

	[SerializableGameDataField]
	public int SpouseCharId = -1;

	public SettlementPrisoner()
	{
	}

	public SettlementPrisoner(SettlementPrisoner other)
	{
		Duration = other.Duration;
		PunishmentType = other.PunishmentType;
		PunishmentSeverity = other.PunishmentSeverity;
		CharId = other.CharId;
		RopeItemKey = other.RopeItemKey;
		KidnapBeginDate = other.KidnapBeginDate;
		ExtraResistance = other.ExtraResistance;
		InitialMorality = other.InitialMorality;
	}

	public void Assign(SettlementPrisoner other)
	{
		Duration = other.Duration;
		PunishmentType = other.PunishmentType;
		PunishmentSeverity = other.PunishmentSeverity;
		CharId = other.CharId;
		RopeItemKey = other.RopeItemKey;
		KidnapBeginDate = other.KidnapBeginDate;
		ExtraResistance = other.ExtraResistance;
		InitialMorality = other.InitialMorality;
	}

	public new bool IsSerializedSizeFixed()
	{
		return false;
	}

	public new int GetSerializedSize()
	{
		int num = 28;
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public new unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		*(short*)ptr = 8;
		ptr += 2;
		*(int*)ptr = Duration;
		ptr += 4;
		*(short*)ptr = PunishmentType;
		ptr += 2;
		*ptr = (byte)PunishmentSeverity;
		ptr++;
		*(int*)ptr = CharId;
		ptr += 4;
		ptr += RopeItemKey.Serialize(ptr);
		*(int*)ptr = KidnapBeginDate;
		ptr += 4;
		*ptr = (byte)ExtraResistance;
		ptr++;
		*(short*)ptr = InitialMorality;
		ptr += 2;
		int num = (int)(ptr - pData);
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public new unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		ushort num = *(ushort*)ptr;
		ptr += 2;
		if (num > 0)
		{
			Duration = *(int*)ptr;
			ptr += 4;
		}
		if (num > 1)
		{
			PunishmentType = *(short*)ptr;
			ptr += 2;
		}
		if (num > 2)
		{
			PunishmentSeverity = (sbyte)(*ptr);
			ptr++;
		}
		if (num > 3)
		{
			CharId = *(int*)ptr;
			ptr += 4;
		}
		if (num > 4)
		{
			ptr += RopeItemKey.Deserialize(ptr);
		}
		if (num > 5)
		{
			KidnapBeginDate = *(int*)ptr;
			ptr += 4;
		}
		if (num > 6)
		{
			ExtraResistance = (sbyte)(*ptr);
			ptr++;
		}
		if (num > 7)
		{
			InitialMorality = *(short*)ptr;
			ptr += 2;
		}
		int num2 = (int)(ptr - pData);
		if (num2 > 4)
		{
			return (num2 + 3) / 4 * 4;
		}
		return num2;
	}
}
