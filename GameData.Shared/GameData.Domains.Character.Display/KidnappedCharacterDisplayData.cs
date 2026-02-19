using GameData.Serializer;
using GameData.Utilities;
using SerializableGameDataSourceGenerator;

namespace GameData.Domains.Character.Display;

[AutoGenerateSerializableGameData]
public class KidnappedCharacterDisplayData : ISerializableGameData
{
	[SerializableGameDataField]
	public CharacterDisplayData CharacterDisplayData;

	[SerializableGameDataField]
	public KidnappedCharacter KidnappedCharacter;

	[SerializableGameDataField]
	public int TotalResistance;

	[SerializableGameDataField]
	public int EscapeRate;

	public KidnappedCharacterDisplayData()
	{
	}

	public KidnappedCharacterDisplayData(KidnappedCharacterDisplayData other)
	{
		CharacterDisplayData = new CharacterDisplayData(other.CharacterDisplayData);
		KidnappedCharacter = new KidnappedCharacter(other.KidnappedCharacter);
		TotalResistance = other.TotalResistance;
		EscapeRate = other.EscapeRate;
	}

	public void Assign(KidnappedCharacterDisplayData other)
	{
		CharacterDisplayData = new CharacterDisplayData(other.CharacterDisplayData);
		KidnappedCharacter = new KidnappedCharacter(other.KidnappedCharacter);
		TotalResistance = other.TotalResistance;
		EscapeRate = other.EscapeRate;
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 8;
		num = ((CharacterDisplayData == null) ? (num + 2) : (num + (2 + CharacterDisplayData.GetSerializedSize())));
		num = ((KidnappedCharacter == null) ? (num + 2) : (num + (2 + KidnappedCharacter.GetSerializedSize())));
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
		ptr += KidnappedCharacter.Serialize(ptr);
		*(int*)ptr = TotalResistance;
		ptr += 4;
		*(int*)ptr = EscapeRate;
		ptr += 4;
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
			CharacterDisplayData = new CharacterDisplayData();
			ptr += CharacterDisplayData.Deserialize(ptr);
		}
		else
		{
			CharacterDisplayData = null;
		}
		KidnappedCharacter = new KidnappedCharacter();
		ptr += KidnappedCharacter.Deserialize(ptr);
		TotalResistance = *(int*)ptr;
		ptr += 4;
		EscapeRate = *(int*)ptr;
		ptr += 4;
		int num2 = (int)(ptr - pData);
		if (num2 > 4)
		{
			return (num2 + 3) / 4 * 4;
		}
		return num2;
	}
}
