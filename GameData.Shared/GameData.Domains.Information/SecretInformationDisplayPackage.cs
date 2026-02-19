using System;
using System.Collections.Generic;
using GameData.Domains.Character.Display;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Information;

public class SecretInformationDisplayPackage
{
	[SerializableGameDataField]
	public readonly List<SecretInformationDisplayData> SecretInformationDisplayDataList = new List<SecretInformationDisplayData>();

	[SerializableGameDataField]
	public readonly IDictionary<int, CharacterDisplayData> CharacterData = new Dictionary<int, CharacterDisplayData>();

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 0;
		if (SecretInformationDisplayDataList != null)
		{
			num += 2;
			int count = SecretInformationDisplayDataList.Count;
			for (int i = 0; i < count; i++)
			{
				SecretInformationDisplayData secretInformationDisplayData = SecretInformationDisplayDataList[i];
				num = ((secretInformationDisplayData == null) ? (num + 2) : (num + (2 + secretInformationDisplayData.GetSerializedSize())));
			}
		}
		else
		{
			num += 2;
		}
		if (CharacterData != null)
		{
			num += 2;
			foreach (KeyValuePair<int, CharacterDisplayData> characterDatum in CharacterData)
			{
				num += 4;
				num += 4;
				num += characterDatum.Value.GetSerializedSize();
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
		if (SecretInformationDisplayDataList != null)
		{
			int count = SecretInformationDisplayDataList.Count;
			*(ushort*)ptr = (ushort)count;
			ptr += 2;
			for (int i = 0; i < count; i++)
			{
				SecretInformationDisplayData secretInformationDisplayData = SecretInformationDisplayDataList[i];
				if (secretInformationDisplayData != null)
				{
					byte* intPtr = ptr;
					ptr += 2;
					int num = secretInformationDisplayData.Serialize(ptr);
					ptr += num;
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
		if (CharacterData != null)
		{
			byte* ptr2 = stackalloc byte[65535];
			int count2 = CharacterData.Count;
			*(ushort*)ptr = (ushort)count2;
			ptr += 2;
			foreach (KeyValuePair<int, CharacterDisplayData> characterDatum in CharacterData)
			{
				*(int*)ptr = characterDatum.Key;
				ptr += 4;
				int num2 = (*(int*)ptr = characterDatum.Value.Serialize(ptr2));
				ptr += 4;
				Buffer.MemoryCopy(ptr2, ptr, num2, num2);
				ptr += num2;
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
			SecretInformationDisplayDataList.Clear();
			for (int i = 0; i < num; i++)
			{
				ushort num2 = *(ushort*)ptr;
				ptr += 2;
				if (num2 > 0)
				{
					SecretInformationDisplayData secretInformationDisplayData = new SecretInformationDisplayData();
					ptr += secretInformationDisplayData.Deserialize(ptr);
					SecretInformationDisplayDataList.Add(secretInformationDisplayData);
				}
				else
				{
					SecretInformationDisplayDataList.Add(null);
				}
			}
		}
		else
		{
			SecretInformationDisplayDataList?.Clear();
		}
		ushort num3 = *(ushort*)ptr;
		ptr += 2;
		if (num3 > 0)
		{
			CharacterData.Clear();
			for (int j = 0; j < num3; j++)
			{
				int key = *(int*)ptr;
				ptr += 4;
				int num4 = *(int*)ptr;
				ptr += 4;
				byte* ptr2 = ptr;
				CharacterDisplayData characterDisplayData = new CharacterDisplayData();
				int num5 = characterDisplayData.Deserialize(ptr);
				CharacterData.Add(key, characterDisplayData);
				if (num5 != num4)
				{
					AdaptableLog.Error(string.Format("{0} {1} size: {2}, should be: {3}", "Deserialize", "CharacterDisplayData", num5, num4));
				}
				ptr = ptr2 + num4;
			}
		}
		else
		{
			CharacterData?.Clear();
		}
		int num6 = (int)(ptr - pData);
		if (num6 > 4)
		{
			return (num6 + 3) / 4 * 4;
		}
		return num6;
	}
}
