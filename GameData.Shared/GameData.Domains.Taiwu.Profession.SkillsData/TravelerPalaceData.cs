using System.Text;
using GameData.Domains.Map;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Taiwu.Profession.SkillsData;

[SerializableGameData(IsExtensible = true)]
public class TravelerPalaceData : ISerializableGameData
{
	private static class FieldIds
	{
		public const ushort CustomName = 0;

		public const ushort Location = 1;

		public const ushort Count = 2;

		public static readonly string[] FieldId2FieldName = new string[2] { "CustomName", "Location" };
	}

	[SerializableGameDataField]
	public string CustomName;

	[SerializableGameDataField]
	public Location Location;

	public TravelerPalaceData()
	{
	}

	public TravelerPalaceData(TravelerPalaceData other)
	{
		CustomName = other.CustomName;
		Location = other.Location;
	}

	public void Assign(TravelerPalaceData other)
	{
		CustomName = other.CustomName;
		Location = other.Location;
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 6;
		num = ((CustomName == null) ? (num + 2) : (num + (2 + 2 * CustomName.Length)));
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		*(short*)ptr = 2;
		ptr += 2;
		if (CustomName != null)
		{
			int length = CustomName.Length;
			Tester.Assert(length <= 65535);
			*(ushort*)ptr = (ushort)length;
			ptr += 2;
			fixed (char* customName = CustomName)
			{
				for (int i = 0; i < length; i++)
				{
					((short*)ptr)[i] = (short)customName[i];
				}
			}
			ptr += 2 * length;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		ptr += Location.Serialize(ptr);
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
		ushort num = *(ushort*)ptr;
		ptr += 2;
		if (num > 0)
		{
			ushort num2 = *(ushort*)ptr;
			ptr += 2;
			if (num2 > 0)
			{
				int num3 = 2 * num2;
				CustomName = Encoding.Unicode.GetString(ptr, num3);
				ptr += num3;
			}
			else
			{
				CustomName = null;
			}
		}
		if (num > 1)
		{
			ptr += Location.Deserialize(ptr);
		}
		int num4 = (int)(ptr - pData);
		if (num4 > 4)
		{
			return (num4 + 3) / 4 * 4;
		}
		return num4;
	}
}
