using System.Collections.Generic;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Character.Display;

public class CharacterSamsaraData : ISerializableGameData
{
	[SerializableGameDataField]
	public List<DeadCharacter> DeadCharacters;

	[SerializableGameDataField]
	public PreexistenceCharIds PreexistenceCharIds;

	public CharacterSamsaraData()
	{
	}

	public CharacterSamsaraData(CharacterSamsaraData other)
	{
		List<DeadCharacter> deadCharacters = other.DeadCharacters;
		int count = deadCharacters.Count;
		DeadCharacters = new List<DeadCharacter>(count);
		for (int i = 0; i < count; i++)
		{
			DeadCharacters.Add(new DeadCharacter(deadCharacters[i]));
		}
		PreexistenceCharIds = other.PreexistenceCharIds;
	}

	public void Assign(CharacterSamsaraData other)
	{
		List<DeadCharacter> deadCharacters = other.DeadCharacters;
		int count = deadCharacters.Count;
		DeadCharacters = new List<DeadCharacter>(count);
		for (int i = 0; i < count; i++)
		{
			DeadCharacters.Add(new DeadCharacter(deadCharacters[i]));
		}
		PreexistenceCharIds = other.PreexistenceCharIds;
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 52;
		if (DeadCharacters != null)
		{
			num += 2;
			int count = DeadCharacters.Count;
			for (int i = 0; i < count; i++)
			{
				DeadCharacter deadCharacter = DeadCharacters[i];
				num = ((deadCharacter == null) ? (num + 2) : (num + (2 + deadCharacter.GetSerializedSize())));
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
		if (DeadCharacters != null)
		{
			int count = DeadCharacters.Count;
			Tester.Assert(count <= 65535);
			*(ushort*)ptr = (ushort)count;
			ptr += 2;
			for (int i = 0; i < count; i++)
			{
				DeadCharacter deadCharacter = DeadCharacters[i];
				if (deadCharacter != null)
				{
					byte* intPtr = ptr;
					ptr += 2;
					int num = deadCharacter.Serialize(ptr);
					ptr += num;
					Tester.Assert(num <= 65535);
					*(ushort*)intPtr = (ushort)num;
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
		ptr += PreexistenceCharIds.Serialize(ptr);
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
			if (DeadCharacters == null)
			{
				DeadCharacters = new List<DeadCharacter>(num);
			}
			else
			{
				DeadCharacters.Clear();
			}
			for (int i = 0; i < num; i++)
			{
				ushort num2 = *(ushort*)ptr;
				ptr += 2;
				if (num2 > 0)
				{
					DeadCharacter deadCharacter = new DeadCharacter();
					ptr += deadCharacter.Deserialize(ptr);
					DeadCharacters.Add(deadCharacter);
				}
				else
				{
					DeadCharacters.Add(null);
				}
			}
		}
		else
		{
			DeadCharacters?.Clear();
		}
		ptr += PreexistenceCharIds.Deserialize(ptr);
		int num3 = (int)(ptr - pData);
		if (num3 > 4)
		{
			return (num3 + 3) / 4 * 4;
		}
		return num3;
	}
}
