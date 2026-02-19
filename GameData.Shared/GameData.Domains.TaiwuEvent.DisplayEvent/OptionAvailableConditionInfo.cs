using System.Text;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.TaiwuEvent.DisplayEvent;

[SerializableGameData(NoCopyConstructors = true, NotForArchive = true)]
public class OptionAvailableConditionInfo : ISerializableGameData
{
	[SerializableGameDataField]
	public int EventFunctionId;

	[SerializableGameDataField]
	public string[] Args;

	[SerializableGameDataField]
	public bool Pass;

	public OptionAvailableConditionInfo()
	{
	}

	public OptionAvailableConditionInfo(int funcId, bool pass, params string[] args)
	{
		EventFunctionId = funcId;
		Args = args;
		Pass = pass;
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 5;
		if (Args != null)
		{
			num += 2;
			int num2 = Args.Length;
			for (int i = 0; i < num2; i++)
			{
				string text = Args[i];
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
		*(int*)ptr = EventFunctionId;
		ptr += 4;
		if (Args != null)
		{
			int num = Args.Length;
			Tester.Assert(num <= 65535);
			*(ushort*)ptr = (ushort)num;
			ptr += 2;
			for (int i = 0; i < num; i++)
			{
				string text = Args[i];
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
		EventFunctionId = *(int*)ptr;
		ptr += 4;
		ushort num = *(ushort*)ptr;
		ptr += 2;
		if (num > 0)
		{
			if (Args == null || Args.Length != num)
			{
				Args = new string[num];
			}
			for (int i = 0; i < num; i++)
			{
				ushort num2 = *(ushort*)ptr;
				ptr += 2;
				if (num2 > 0)
				{
					int num3 = 2 * num2;
					Args[i] = Encoding.Unicode.GetString(ptr, num3);
					ptr += num3;
				}
				else
				{
					Args[i] = null;
				}
			}
		}
		else
		{
			Args = null;
		}
		Pass = *ptr != 0;
		ptr++;
		int num4 = (int)(ptr - pData);
		if (num4 > 4)
		{
			return (num4 + 3) / 4 * 4;
		}
		return num4;
	}
}
