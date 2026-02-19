using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.TaiwuEvent.DisplayEvent;

public struct OptionAvailableInfo : ISerializableGameData
{
	[SerializableGameDataField]
	public OptionAvailableInfoMinimumElement[] Data;

	[SerializableGameDataField]
	public bool PassState;

	[SerializableGameDataField]
	public bool Hide;

	public OptionAvailableInfo(OptionAvailableInfo other)
	{
		OptionAvailableInfoMinimumElement[] data = other.Data;
		int num = data.Length;
		Data = new OptionAvailableInfoMinimumElement[num];
		for (int i = 0; i < num; i++)
		{
			Data[i] = new OptionAvailableInfoMinimumElement(data[i]);
		}
		PassState = other.PassState;
		Hide = other.Hide;
	}

	public void Assign(OptionAvailableInfo other)
	{
		OptionAvailableInfoMinimumElement[] data = other.Data;
		int num = data.Length;
		Data = new OptionAvailableInfoMinimumElement[num];
		for (int i = 0; i < num; i++)
		{
			Data[i] = new OptionAvailableInfoMinimumElement(data[i]);
		}
		PassState = other.PassState;
		Hide = other.Hide;
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 2;
		if (Data != null)
		{
			num += 2;
			int num2 = Data.Length;
			for (int i = 0; i < num2; i++)
			{
				num += Data[i].GetSerializedSize();
			}
		}
		else
		{
			num += 2;
		}
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		if (Data != null)
		{
			int num = Data.Length;
			Tester.Assert(num <= 65535);
			*(ushort*)ptr = (ushort)num;
			ptr += 2;
			for (int i = 0; i < num; i++)
			{
				int num2 = Data[i].Serialize(ptr);
				ptr += num2;
				Tester.Assert(num2 <= 65535);
			}
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		*ptr = (PassState ? ((byte)1) : ((byte)0));
		ptr++;
		*ptr = (Hide ? ((byte)1) : ((byte)0));
		ptr++;
		int num3 = (int)(ptr - pData);
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
			if (Data == null || Data.Length != num)
			{
				Data = new OptionAvailableInfoMinimumElement[num];
			}
			for (int i = 0; i < num; i++)
			{
				OptionAvailableInfoMinimumElement optionAvailableInfoMinimumElement = default(OptionAvailableInfoMinimumElement);
				ptr += optionAvailableInfoMinimumElement.Deserialize(ptr);
				Data[i] = optionAvailableInfoMinimumElement;
			}
		}
		else
		{
			Data = null;
		}
		PassState = *ptr != 0;
		ptr++;
		Hide = *ptr != 0;
		ptr++;
		int num2 = (int)(ptr - pData);
		if (num2 > 4)
		{
			return (num2 + 3) / 4 * 4;
		}
		return num2;
	}
}
