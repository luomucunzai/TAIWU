using System;
using System.Text;
using GameData.Domains.Character;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Taiwu.Profession.SkillsData;

[Obsolete]
[SerializableGameData(NotForDisplayModule = true)]
public class ObsoleteBeggarSkillsData : IProfessionSkillsData, ISerializableGameData
{
	[SerializableGameDataField]
	public string LookingForCharName;

	[SerializableGameDataField]
	public CharacterSet AlreadyFoundCharacters;

	public bool FoundMoreAlive;

	public bool FoundMoreDead;

	public void Initialize()
	{
		ClearData();
	}

	public void InheritFrom(IProfessionSkillsData sourceData)
	{
	}

	public void ClearData()
	{
		LookingForCharName = null;
		AlreadyFoundCharacters.Clear();
		FoundMoreDead = false;
		FoundMoreAlive = false;
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 0;
		num = ((LookingForCharName == null) ? (num + 2) : (num + (2 + 2 * LookingForCharName.Length)));
		num += AlreadyFoundCharacters.GetSerializedSize();
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		if (LookingForCharName != null)
		{
			int length = LookingForCharName.Length;
			Tester.Assert(length <= 65535);
			*(ushort*)ptr = (ushort)length;
			ptr += 2;
			fixed (char* lookingForCharName = LookingForCharName)
			{
				for (int i = 0; i < length; i++)
				{
					((short*)ptr)[i] = (short)lookingForCharName[i];
				}
			}
			ptr += 2 * length;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		int num = AlreadyFoundCharacters.Serialize(ptr);
		ptr += num;
		Tester.Assert(num <= 65535);
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
			LookingForCharName = Encoding.Unicode.GetString(ptr, num2);
			ptr += num2;
		}
		else
		{
			LookingForCharName = null;
		}
		ptr += AlreadyFoundCharacters.Deserialize(ptr);
		int num3 = (int)(ptr - pData);
		if (num3 > 4)
		{
			return (num3 + 3) / 4 * 4;
		}
		return num3;
	}
}
