using System;
using System.Collections.Generic;
using System.Text;
using GameData.Domains.Character;
using GameData.Domains.Character.Display;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.TaiwuEvent.DisplayEvent;

public struct CharacterSelectFilter : ISerializableGameData
{
	[SerializableGameDataField]
	public short FilterTemplateId;

	[SerializableGameDataField]
	public string SelectKey;

	[Obsolete]
	[SerializableGameDataField]
	public CharacterSet AvailableCharacters;

	[SerializableGameDataField]
	public List<CharacterDisplayData> AvailableCharactersDisplayDataList;

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 2;
		num = ((SelectKey == null) ? (num + 2) : (num + (2 + 2 * SelectKey.Length)));
		num += AvailableCharacters.GetSerializedSize();
		if (AvailableCharactersDisplayDataList != null)
		{
			num += 2;
			int count = AvailableCharactersDisplayDataList.Count;
			for (int i = 0; i < count; i++)
			{
				CharacterDisplayData characterDisplayData = AvailableCharactersDisplayDataList[i];
				num = ((characterDisplayData == null) ? (num + 2) : (num + (2 + characterDisplayData.GetSerializedSize())));
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
		*(short*)ptr = FilterTemplateId;
		ptr += 2;
		if (SelectKey != null)
		{
			int length = SelectKey.Length;
			Tester.Assert(length <= 65535);
			*(ushort*)ptr = (ushort)length;
			ptr += 2;
			fixed (char* selectKey = SelectKey)
			{
				for (int i = 0; i < length; i++)
				{
					((short*)ptr)[i] = (short)selectKey[i];
				}
			}
			ptr += 2 * length;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		int num = AvailableCharacters.Serialize(ptr);
		ptr += num;
		Tester.Assert(num <= 65535);
		if (AvailableCharactersDisplayDataList != null)
		{
			int count = AvailableCharactersDisplayDataList.Count;
			Tester.Assert(count <= 65535);
			*(ushort*)ptr = (ushort)count;
			ptr += 2;
			for (int j = 0; j < count; j++)
			{
				CharacterDisplayData characterDisplayData = AvailableCharactersDisplayDataList[j];
				if (characterDisplayData != null)
				{
					byte* intPtr = ptr;
					ptr += 2;
					int num2 = characterDisplayData.Serialize(ptr);
					ptr += num2;
					Tester.Assert(num2 <= 65535);
					*(ushort*)intPtr = (ushort)num2;
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
		FilterTemplateId = *(short*)ptr;
		ptr += 2;
		ushort num = *(ushort*)ptr;
		ptr += 2;
		if (num > 0)
		{
			int num2 = 2 * num;
			SelectKey = Encoding.Unicode.GetString(ptr, num2);
			ptr += num2;
		}
		else
		{
			SelectKey = null;
		}
		ptr += AvailableCharacters.Deserialize(ptr);
		ushort num3 = *(ushort*)ptr;
		ptr += 2;
		if (num3 > 0)
		{
			if (AvailableCharactersDisplayDataList == null)
			{
				AvailableCharactersDisplayDataList = new List<CharacterDisplayData>(num3);
			}
			else
			{
				AvailableCharactersDisplayDataList.Clear();
			}
			for (int i = 0; i < num3; i++)
			{
				ushort num4 = *(ushort*)ptr;
				ptr += 2;
				if (num4 > 0)
				{
					CharacterDisplayData characterDisplayData = new CharacterDisplayData();
					ptr += characterDisplayData.Deserialize(ptr);
					AvailableCharactersDisplayDataList.Add(characterDisplayData);
				}
				else
				{
					AvailableCharactersDisplayDataList.Add(null);
				}
			}
		}
		else
		{
			AvailableCharactersDisplayDataList?.Clear();
		}
		int num5 = (int)(ptr - pData);
		if (num5 > 4)
		{
			return (num5 + 3) / 4 * 4;
		}
		return num5;
	}
}
