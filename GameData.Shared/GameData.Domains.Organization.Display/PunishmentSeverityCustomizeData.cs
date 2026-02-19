using System;
using Config;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Organization.Display;

[SerializableGameData(NotRestrictCollectionSerializedSize = true, IsExtensible = true)]
public class PunishmentSeverityCustomizeData : ISerializableGameData
{
	private static class FieldIds
	{
		public const ushort PunishmentTypeTemplateId = 0;

		public const ushort CustomizedPunishmentSeverityTemplateId = 1;

		public const ushort ModifyDate = 2;

		public const ushort Count = 3;

		public static readonly string[] FieldId2FieldName = new string[3] { "PunishmentTypeTemplateId", "CustomizedPunishmentSeverityTemplateId", "ModifyDate" };
	}

	[SerializableGameDataField]
	public short PunishmentTypeTemplateId;

	[SerializableGameDataField]
	public sbyte CustomizedPunishmentSeverityTemplateId;

	[SerializableGameDataField]
	public int ModifyDate;

	public int ModificationDiff(sbyte stateTemplateId, bool isSect)
	{
		return Math.Abs(PunishmentType.Instance[PunishmentTypeTemplateId].GetSeverity(stateTemplateId, isSect) - CustomizedPunishmentSeverityTemplateId);
	}

	public int ModificationDiff(short key)
	{
		var (stateTemplateId, isSect) = DecodePunishmentSeverityCustomizeKey(key);
		return ModificationDiff(stateTemplateId, isSect);
	}

	public static short GetPunishmentSeverityCustomizeKey(sbyte stateTemplateId, bool isSect)
	{
		return (short)(((isSect ? 1u : 0u) << 8) | (byte)stateTemplateId);
	}

	public static (sbyte stateTemplateId, bool isSect) DecodePunishmentSeverityCustomizeKey(short key)
	{
		ushort num = (ushort)key;
		sbyte item = (sbyte)BitOperation.GetSubUshort(num, 0, 8);
		bool item2 = BitOperation.GetSubUshort(num, 8, 8) == 1;
		return (stateTemplateId: item, isSect: item2);
	}

	public PunishmentSeverityCustomizeData()
	{
	}

	public PunishmentSeverityCustomizeData(PunishmentSeverityCustomizeData other)
	{
		PunishmentTypeTemplateId = other.PunishmentTypeTemplateId;
		CustomizedPunishmentSeverityTemplateId = other.CustomizedPunishmentSeverityTemplateId;
		ModifyDate = other.ModifyDate;
	}

	public void Assign(PunishmentSeverityCustomizeData other)
	{
		PunishmentTypeTemplateId = other.PunishmentTypeTemplateId;
		CustomizedPunishmentSeverityTemplateId = other.CustomizedPunishmentSeverityTemplateId;
		ModifyDate = other.ModifyDate;
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 9;
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		*(short*)pData = 3;
		byte* num = pData + 2;
		*(short*)num = PunishmentTypeTemplateId;
		byte* num2 = num + 2;
		*num2 = (byte)CustomizedPunishmentSeverityTemplateId;
		byte* num3 = num2 + 1;
		*(int*)num3 = ModifyDate;
		int num4 = (int)(num3 + 4 - pData);
		if (num4 > 4)
		{
			return (num4 + 3) / 4 * 4;
		}
		return num4;
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		ushort num = *(ushort*)ptr;
		ptr += 2;
		if (num > 0)
		{
			PunishmentTypeTemplateId = *(short*)ptr;
			ptr += 2;
		}
		if (num > 1)
		{
			CustomizedPunishmentSeverityTemplateId = (sbyte)(*ptr);
			ptr++;
		}
		if (num > 2)
		{
			ModifyDate = *(int*)ptr;
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
