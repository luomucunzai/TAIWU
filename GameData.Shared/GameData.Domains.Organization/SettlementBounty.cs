using GameData.Serializer;

namespace GameData.Domains.Organization;

[SerializableGameData(IsExtensible = true)]
public class SettlementBounty : ISerializableGameData
{
	private static class FieldIds
	{
		public const ushort CharId = 0;

		public const ushort BountyAmount = 1;

		public const ushort PunishmentType = 2;

		public const ushort PunishmentSeverity = 3;

		public const ushort ExpireDate = 4;

		public const ushort RequiredConsummateLevel = 5;

		public const ushort CurrentHunterId = 6;

		public const ushort Count = 7;

		public static readonly string[] FieldId2FieldName = new string[7] { "CharId", "BountyAmount", "PunishmentType", "PunishmentSeverity", "ExpireDate", "RequiredConsummateLevel", "CurrentHunterId" };
	}

	[SerializableGameDataField]
	public int CharId;

	[SerializableGameDataField]
	public int BountyAmount;

	[SerializableGameDataField]
	public short PunishmentType;

	[SerializableGameDataField]
	public sbyte PunishmentSeverity;

	[SerializableGameDataField]
	public int ExpireDate;

	[SerializableGameDataField]
	public sbyte RequiredConsummateLevel = -1;

	[SerializableGameDataField]
	public int CurrentHunterId = -1;

	public short CaptorFameActionMultiplier => (short)(PunishmentSeverity * 10);

	public SettlementBounty()
	{
	}

	public SettlementBounty(SettlementBounty other)
	{
		CharId = other.CharId;
		BountyAmount = other.BountyAmount;
		PunishmentType = other.PunishmentType;
		PunishmentSeverity = other.PunishmentSeverity;
		ExpireDate = other.ExpireDate;
		RequiredConsummateLevel = other.RequiredConsummateLevel;
		CurrentHunterId = other.CurrentHunterId;
	}

	public void Assign(SettlementBounty other)
	{
		CharId = other.CharId;
		BountyAmount = other.BountyAmount;
		PunishmentType = other.PunishmentType;
		PunishmentSeverity = other.PunishmentSeverity;
		ExpireDate = other.ExpireDate;
		RequiredConsummateLevel = other.RequiredConsummateLevel;
		CurrentHunterId = other.CurrentHunterId;
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 22;
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		*(short*)pData = 7;
		byte* num = pData + 2;
		*(int*)num = CharId;
		byte* num2 = num + 4;
		*(int*)num2 = BountyAmount;
		byte* num3 = num2 + 4;
		*(short*)num3 = PunishmentType;
		byte* num4 = num3 + 2;
		*num4 = (byte)PunishmentSeverity;
		byte* num5 = num4 + 1;
		*(int*)num5 = ExpireDate;
		byte* num6 = num5 + 4;
		*num6 = (byte)RequiredConsummateLevel;
		byte* num7 = num6 + 1;
		*(int*)num7 = CurrentHunterId;
		int num8 = (int)(num7 + 4 - pData);
		if (num8 > 4)
		{
			return (num8 + 3) / 4 * 4;
		}
		return num8;
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		ushort num = *(ushort*)ptr;
		ptr += 2;
		if (num > 0)
		{
			CharId = *(int*)ptr;
			ptr += 4;
		}
		if (num > 1)
		{
			BountyAmount = *(int*)ptr;
			ptr += 4;
		}
		if (num > 2)
		{
			PunishmentType = *(short*)ptr;
			ptr += 2;
		}
		if (num > 3)
		{
			PunishmentSeverity = (sbyte)(*ptr);
			ptr++;
		}
		if (num > 4)
		{
			ExpireDate = *(int*)ptr;
			ptr += 4;
		}
		if (num > 5)
		{
			RequiredConsummateLevel = (sbyte)(*ptr);
			ptr++;
		}
		if (num > 6)
		{
			CurrentHunterId = *(int*)ptr;
			ptr += 4;
		}
		int num2 = (int)(ptr - pData);
		if (num2 > 4)
		{
			return (num2 + 3) / 4 * 4;
		}
		return num2;
	}
}
