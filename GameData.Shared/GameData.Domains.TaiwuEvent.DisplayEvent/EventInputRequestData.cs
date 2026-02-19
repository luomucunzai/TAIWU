using System.Text;
using GameData.Domains.Character;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.TaiwuEvent.DisplayEvent;

public class EventInputRequestData : ISerializableGameData
{
	[SerializableGameDataField]
	public string DataKey;

	[SerializableGameDataField]
	public sbyte InputDataType;

	[SerializableGameDataField]
	public int[] NumberRange;

	[SerializableGameDataField]
	public FullName FullName;

	[SerializableGameDataField]
	public string ConfirmDisableTips;

	public EventInputRequestData()
	{
	}

	public EventInputRequestData(EventInputRequestData other)
	{
		DataKey = other.DataKey;
		InputDataType = other.InputDataType;
		int[] numberRange = other.NumberRange;
		int num = numberRange.Length;
		NumberRange = new int[num];
		for (int i = 0; i < num; i++)
		{
			NumberRange[i] = numberRange[i];
		}
		FullName = other.FullName;
		ConfirmDisableTips = other.ConfirmDisableTips;
	}

	public void Assign(EventInputRequestData other)
	{
		DataKey = other.DataKey;
		InputDataType = other.InputDataType;
		int[] numberRange = other.NumberRange;
		int num = numberRange.Length;
		NumberRange = new int[num];
		for (int i = 0; i < num; i++)
		{
			NumberRange[i] = numberRange[i];
		}
		FullName = other.FullName;
		ConfirmDisableTips = other.ConfirmDisableTips;
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 11;
		num = ((DataKey == null) ? (num + 2) : (num + (2 + 2 * DataKey.Length)));
		num = ((NumberRange == null) ? (num + 2) : (num + (2 + 4 * NumberRange.Length)));
		num = ((ConfirmDisableTips == null) ? (num + 2) : (num + (2 + 2 * ConfirmDisableTips.Length)));
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		if (DataKey != null)
		{
			int length = DataKey.Length;
			Tester.Assert(length <= 65535);
			*(ushort*)ptr = (ushort)length;
			ptr += 2;
			fixed (char* dataKey = DataKey)
			{
				for (int i = 0; i < length; i++)
				{
					((short*)ptr)[i] = (short)dataKey[i];
				}
			}
			ptr += 2 * length;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		*ptr = (byte)InputDataType;
		ptr++;
		if (NumberRange != null)
		{
			int num = NumberRange.Length;
			Tester.Assert(num <= 65535);
			*(ushort*)ptr = (ushort)num;
			ptr += 2;
			for (int j = 0; j < num; j++)
			{
				((int*)ptr)[j] = NumberRange[j];
			}
			ptr += 4 * num;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		ptr += FullName.Serialize(ptr);
		if (ConfirmDisableTips != null)
		{
			int length2 = ConfirmDisableTips.Length;
			Tester.Assert(length2 <= 65535);
			*(ushort*)ptr = (ushort)length2;
			ptr += 2;
			fixed (char* confirmDisableTips = ConfirmDisableTips)
			{
				for (int k = 0; k < length2; k++)
				{
					((short*)ptr)[k] = (short)confirmDisableTips[k];
				}
			}
			ptr += 2 * length2;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
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
		ushort num = *(ushort*)ptr;
		ptr += 2;
		if (num > 0)
		{
			int num2 = 2 * num;
			DataKey = Encoding.Unicode.GetString(ptr, num2);
			ptr += num2;
		}
		else
		{
			DataKey = null;
		}
		InputDataType = (sbyte)(*ptr);
		ptr++;
		ushort num3 = *(ushort*)ptr;
		ptr += 2;
		if (num3 > 0)
		{
			if (NumberRange == null || NumberRange.Length != num3)
			{
				NumberRange = new int[num3];
			}
			for (int i = 0; i < num3; i++)
			{
				NumberRange[i] = ((int*)ptr)[i];
			}
			ptr += 4 * num3;
		}
		else
		{
			NumberRange = null;
		}
		ptr += FullName.Deserialize(ptr);
		ushort num4 = *(ushort*)ptr;
		ptr += 2;
		if (num4 > 0)
		{
			int num5 = 2 * num4;
			ConfirmDisableTips = Encoding.Unicode.GetString(ptr, num5);
			ptr += num5;
		}
		else
		{
			ConfirmDisableTips = null;
		}
		int num6 = (int)(ptr - pData);
		if (num6 > 4)
		{
			return (num6 + 3) / 4 * 4;
		}
		return num6;
	}
}
