using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Character.Display;

public class CharacterDisplayDataWithInfo : ISerializableGameData
{
	[SerializableGameDataField]
	public CharacterDisplayData CharacterDisplayData;

	[SerializableGameDataField]
	public CharacterInfoCountData CharacterInfoCountData;

	public CharacterDisplayDataWithInfo()
	{
	}

	public CharacterDisplayDataWithInfo(CharacterDisplayDataWithInfo other)
	{
		CharacterDisplayData = new CharacterDisplayData(other.CharacterDisplayData);
		CharacterInfoCountData = new CharacterInfoCountData(other.CharacterInfoCountData);
	}

	public void Assign(CharacterDisplayDataWithInfo other)
	{
		CharacterDisplayData = new CharacterDisplayData(other.CharacterDisplayData);
		CharacterInfoCountData = new CharacterInfoCountData(other.CharacterInfoCountData);
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 12;
		num = ((CharacterDisplayData == null) ? (num + 2) : (num + (2 + CharacterDisplayData.GetSerializedSize())));
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		if (CharacterDisplayData != null)
		{
			byte* intPtr = ptr;
			ptr += 2;
			int num = CharacterDisplayData.Serialize(ptr);
			ptr += num;
			Tester.Assert(num <= 65535);
			*(ushort*)intPtr = (ushort)num;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		ptr += CharacterInfoCountData.Serialize(ptr);
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
			if (CharacterDisplayData == null)
			{
				CharacterDisplayData = new CharacterDisplayData();
			}
			ptr += CharacterDisplayData.Deserialize(ptr);
		}
		else
		{
			CharacterDisplayData = null;
		}
		if (CharacterInfoCountData == null)
		{
			CharacterInfoCountData = new CharacterInfoCountData();
		}
		ptr += CharacterInfoCountData.Deserialize(ptr);
		int num2 = (int)(ptr - pData);
		if (num2 > 4)
		{
			return (num2 + 3) / 4 * 4;
		}
		return num2;
	}
}
