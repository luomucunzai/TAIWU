using GameData.Combat.Math;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Character;

[SerializableGameData(IsExtensible = true)]
public struct CharacterPropertyBonus : ISerializableGameData
{
	private static class FieldIds
	{
		public const ushort AddValue = 0;

		public const ushort AddPercent = 1;

		public const ushort Count = 2;

		public static readonly string[] FieldId2FieldName = new string[2] { "AddValue", "AddPercent" };
	}

	[SerializableGameDataField]
	public int AddValue;

	[SerializableGameDataField]
	public int AddPercent;

	public bool IsZero
	{
		get
		{
			if (AddValue == 0)
			{
				return AddPercent == 0;
			}
			return false;
		}
	}

	public static implicit operator CValueModify(CharacterPropertyBonus bonus)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		return new CValueModify(bonus.AddValue, CValuePercentBonus.op_Implicit(bonus.AddPercent), default(CValuePercentBonus), default(CValuePercentBonus));
	}

	public void AddBonus(EDataModifyType bonusType, int bonusValue)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected I4, but got Unknown
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		switch ((int)bonusType)
		{
		case 0:
			AddValue += bonusValue;
			break;
		case 1:
			AddPercent += bonusValue;
			break;
		default:
			AdaptableLog.Warning($"try add bonus {bonusValue} by invalid type {bonusType}");
			break;
		}
	}

	public int GetBonus(EDataModifyType bonusType)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Invalid comparison between Unknown and I4
		if ((int)bonusType != 0)
		{
			if ((int)bonusType == 1)
			{
				return AddPercent;
			}
			return 0;
		}
		return AddValue;
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 10;
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		*(short*)pData = 2;
		byte* num = pData + 2;
		*(int*)num = AddValue;
		byte* num2 = num + 4;
		*(int*)num2 = AddPercent;
		int num3 = (int)(num2 + 4 - pData);
		if (num3 > 4)
		{
			return (num3 + 3) / 4 * 4;
		}
		return num3;
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		ushort num = *(ushort*)ptr;
		ptr += 2;
		if (num > 0)
		{
			AddValue = *(int*)ptr;
			ptr += 4;
		}
		if (num > 1)
		{
			AddPercent = *(int*)ptr;
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
