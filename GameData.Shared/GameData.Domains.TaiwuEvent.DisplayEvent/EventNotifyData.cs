using System.Text;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.TaiwuEvent.DisplayEvent;

public class EventNotifyData : ISerializableGameData
{
	[SerializableGameDataField]
	public string TitleKey;

	[SerializableGameDataField]
	public string[] TitleFormatArgs;

	[SerializableGameDataField]
	public string ContentKey;

	[SerializableGameDataField]
	public string[] ContentFormatArgs;

	public static readonly EventNotifyData Empty = new EventNotifyData();

	public EventNotifyData()
	{
	}

	public EventNotifyData(EventNotifyData other)
	{
		TitleKey = other.TitleKey;
		string[] titleFormatArgs = other.TitleFormatArgs;
		int num = titleFormatArgs.Length;
		TitleFormatArgs = new string[num];
		for (int i = 0; i < num; i++)
		{
			TitleFormatArgs[i] = titleFormatArgs[i];
		}
		ContentKey = other.ContentKey;
		string[] contentFormatArgs = other.ContentFormatArgs;
		int num2 = contentFormatArgs.Length;
		ContentFormatArgs = new string[num2];
		for (int j = 0; j < num2; j++)
		{
			ContentFormatArgs[j] = contentFormatArgs[j];
		}
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 0;
		num = ((TitleKey == null) ? (num + 2) : (num + (2 + 2 * TitleKey.Length)));
		if (TitleFormatArgs != null)
		{
			num += 2;
			int num2 = TitleFormatArgs.Length;
			for (int i = 0; i < num2; i++)
			{
				string text = TitleFormatArgs[i];
				num = ((text == null) ? (num + 2) : (num + (2 + 2 * text.Length)));
			}
		}
		else
		{
			num += 2;
		}
		num = ((ContentKey == null) ? (num + 2) : (num + (2 + 2 * ContentKey.Length)));
		if (ContentFormatArgs != null)
		{
			num += 2;
			int num3 = ContentFormatArgs.Length;
			for (int j = 0; j < num3; j++)
			{
				string text2 = ContentFormatArgs[j];
				num = ((text2 == null) ? (num + 2) : (num + (2 + 2 * text2.Length)));
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
		if (TitleKey != null)
		{
			int length = TitleKey.Length;
			Tester.Assert(length <= 65535);
			*(ushort*)ptr = (ushort)length;
			ptr += 2;
			fixed (char* titleKey = TitleKey)
			{
				for (int i = 0; i < length; i++)
				{
					((short*)ptr)[i] = (short)titleKey[i];
				}
			}
			ptr += 2 * length;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		if (TitleFormatArgs != null)
		{
			int num = TitleFormatArgs.Length;
			Tester.Assert(num <= 65535);
			*(ushort*)ptr = (ushort)num;
			ptr += 2;
			for (int j = 0; j < num; j++)
			{
				string text = TitleFormatArgs[j];
				if (text != null)
				{
					int length2 = text.Length;
					Tester.Assert(length2 <= 65535);
					*(ushort*)ptr = (ushort)length2;
					ptr += 2;
					fixed (char* ptr2 = text)
					{
						for (int k = 0; k < length2; k++)
						{
							((short*)ptr)[k] = (short)ptr2[k];
						}
					}
					ptr += 2 * length2;
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
		if (ContentKey != null)
		{
			int length3 = ContentKey.Length;
			Tester.Assert(length3 <= 65535);
			*(ushort*)ptr = (ushort)length3;
			ptr += 2;
			fixed (char* contentKey = ContentKey)
			{
				for (int l = 0; l < length3; l++)
				{
					((short*)ptr)[l] = (short)contentKey[l];
				}
			}
			ptr += 2 * length3;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		if (ContentFormatArgs != null)
		{
			int num2 = ContentFormatArgs.Length;
			Tester.Assert(num2 <= 65535);
			*(ushort*)ptr = (ushort)num2;
			ptr += 2;
			for (int m = 0; m < num2; m++)
			{
				string text2 = ContentFormatArgs[m];
				if (text2 != null)
				{
					int length4 = text2.Length;
					Tester.Assert(length4 <= 65535);
					*(ushort*)ptr = (ushort)length4;
					ptr += 2;
					fixed (char* ptr3 = text2)
					{
						for (int n = 0; n < length4; n++)
						{
							((short*)ptr)[n] = (short)ptr3[n];
						}
					}
					ptr += 2 * length4;
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
			int num2 = 2 * num;
			TitleKey = Encoding.Unicode.GetString(ptr, num2);
			ptr += num2;
		}
		else
		{
			TitleKey = null;
		}
		ushort num3 = *(ushort*)ptr;
		ptr += 2;
		if (num3 > 0)
		{
			if (TitleFormatArgs == null || TitleFormatArgs.Length != num3)
			{
				TitleFormatArgs = new string[num3];
			}
			for (int i = 0; i < num3; i++)
			{
				ushort num4 = *(ushort*)ptr;
				ptr += 2;
				if (num4 > 0)
				{
					int num5 = 2 * num4;
					TitleFormatArgs[i] = Encoding.Unicode.GetString(ptr, num5);
					ptr += num5;
				}
				else
				{
					TitleFormatArgs[i] = null;
				}
			}
		}
		else
		{
			TitleFormatArgs = null;
		}
		ushort num6 = *(ushort*)ptr;
		ptr += 2;
		if (num6 > 0)
		{
			int num7 = 2 * num6;
			ContentKey = Encoding.Unicode.GetString(ptr, num7);
			ptr += num7;
		}
		else
		{
			ContentKey = null;
		}
		ushort num8 = *(ushort*)ptr;
		ptr += 2;
		if (num8 > 0)
		{
			if (ContentFormatArgs == null || ContentFormatArgs.Length != num8)
			{
				ContentFormatArgs = new string[num8];
			}
			for (int j = 0; j < num8; j++)
			{
				ushort num9 = *(ushort*)ptr;
				ptr += 2;
				if (num9 > 0)
				{
					int num10 = 2 * num9;
					ContentFormatArgs[j] = Encoding.Unicode.GetString(ptr, num10);
					ptr += num10;
				}
				else
				{
					ContentFormatArgs[j] = null;
				}
			}
		}
		else
		{
			ContentFormatArgs = null;
		}
		int num11 = (int)(ptr - pData);
		if (num11 > 4)
		{
			return (num11 + 3) / 4 * 4;
		}
		return num11;
	}
}
