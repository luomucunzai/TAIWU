using System.Text;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.TaiwuEvent.DisplayEvent;

public struct OptionAvailableInfoMinimumElement : ISerializableGameData
{
	[SerializableGameDataField]
	public short ConditionId;

	[SerializableGameDataField]
	public string[] FormatArgs;

	[SerializableGameDataField]
	public bool Pass;

	[SerializableGameDataField]
	public bool Hide;

	public OptionAvailableInfoMinimumElement(OptionAvailableInfoMinimumElement other)
	{
		ConditionId = other.ConditionId;
		string[] formatArgs = other.FormatArgs;
		int num = formatArgs.Length;
		FormatArgs = new string[num];
		for (int i = 0; i < num; i++)
		{
			FormatArgs[i] = formatArgs[i];
		}
		Pass = other.Pass;
		Hide = other.Hide;
	}

	public void Assign(OptionAvailableInfoMinimumElement other)
	{
		ConditionId = other.ConditionId;
		string[] formatArgs = other.FormatArgs;
		int num = formatArgs.Length;
		FormatArgs = new string[num];
		for (int i = 0; i < num; i++)
		{
			FormatArgs[i] = formatArgs[i];
		}
		Pass = other.Pass;
		Hide = other.Hide;
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 4;
		if (FormatArgs != null)
		{
			num += 2;
			int num2 = FormatArgs.Length;
			for (int i = 0; i < num2; i++)
			{
				string text = FormatArgs[i];
				num = ((text == null) ? (num + 2) : (num + (2 + 2 * text.Length)));
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
		*(short*)ptr = ConditionId;
		ptr += 2;
		if (FormatArgs != null)
		{
			int num = FormatArgs.Length;
			Tester.Assert(num <= 65535);
			*(ushort*)ptr = (ushort)num;
			ptr += 2;
			for (int i = 0; i < num; i++)
			{
				string text = FormatArgs[i];
				if (text != null)
				{
					int length = text.Length;
					Tester.Assert(length <= 65535);
					*(ushort*)ptr = (ushort)length;
					ptr += 2;
					fixed (char* ptr2 = text)
					{
						for (int j = 0; j < length; j++)
						{
							((short*)ptr)[j] = (short)ptr2[j];
						}
					}
					ptr += 2 * length;
				}
				else
				{
					*(short*)ptr = 0;
					ptr += 2;
				}
			}
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		*ptr = (Pass ? ((byte)1) : ((byte)0));
		ptr++;
		*ptr = (Hide ? ((byte)1) : ((byte)0));
		ptr++;
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
		ConditionId = *(short*)ptr;
		ptr += 2;
		ushort num = *(ushort*)ptr;
		ptr += 2;
		if (num > 0)
		{
			if (FormatArgs == null || FormatArgs.Length != num)
			{
				FormatArgs = new string[num];
			}
			for (int i = 0; i < num; i++)
			{
				ushort num2 = *(ushort*)ptr;
				ptr += 2;
				if (num2 > 0)
				{
					int num3 = 2 * num2;
					FormatArgs[i] = Encoding.Unicode.GetString(ptr, num3);
					ptr += num3;
				}
				else
				{
					FormatArgs[i] = null;
				}
			}
		}
		else
		{
			FormatArgs = null;
		}
		Pass = *ptr != 0;
		ptr++;
		Hide = *ptr != 0;
		ptr++;
		int num4 = (int)(ptr - pData);
		if (num4 > 4)
		{
			return (num4 + 3) / 4 * 4;
		}
		return num4;
	}
}
