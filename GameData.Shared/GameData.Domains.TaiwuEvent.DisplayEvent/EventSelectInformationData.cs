using System.Collections.Generic;
using System.Text;
using GameData.Domains.Character.Display;
using GameData.Domains.Information;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.TaiwuEvent.DisplayEvent;

[SerializableGameData(NoCopyConstructors = true)]
public class EventSelectInformationData : ISerializableGameData
{
	[SerializableGameDataField]
	public int RelatedCharacterId;

	public bool SelectComplete;

	[SerializableGameDataField]
	public bool AvailableData;

	[SerializableGameDataField]
	public bool IsForShopping;

	[SerializableGameDataField]
	public NameRelatedData CharacterNameRelatedData;

	[SerializableGameDataField]
	public List<int> ToSelectSecretInformationDataIdList;

	[SerializableGameDataField]
	public NormalInformationCollection ToSelectNormalInformation;

	[SerializableGameDataField]
	public string SaveKey;

	public string SelectForEventGuid;

	public string SelectForOptionKey;

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 38;
		num = ((ToSelectSecretInformationDataIdList == null) ? (num + 2) : (num + (2 + 4 * ToSelectSecretInformationDataIdList.Count)));
		num = ((ToSelectNormalInformation == null) ? (num + 2) : (num + (2 + ToSelectNormalInformation.GetSerializedSize())));
		num = ((SaveKey == null) ? (num + 2) : (num + (2 + 2 * SaveKey.Length)));
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		*(int*)ptr = RelatedCharacterId;
		ptr += 4;
		*ptr = (AvailableData ? ((byte)1) : ((byte)0));
		ptr++;
		*ptr = (IsForShopping ? ((byte)1) : ((byte)0));
		ptr++;
		ptr += CharacterNameRelatedData.Serialize(ptr);
		if (ToSelectSecretInformationDataIdList != null)
		{
			int count = ToSelectSecretInformationDataIdList.Count;
			Tester.Assert(count <= 65535);
			*(ushort*)ptr = (ushort)count;
			ptr += 2;
			for (int i = 0; i < count; i++)
			{
				((int*)ptr)[i] = ToSelectSecretInformationDataIdList[i];
			}
			ptr += 4 * count;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		if (ToSelectNormalInformation != null)
		{
			byte* intPtr = ptr;
			ptr += 2;
			int num = ToSelectNormalInformation.Serialize(ptr);
			ptr += num;
			Tester.Assert(num <= 65535);
			*(ushort*)intPtr = (ushort)num;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		if (SaveKey != null)
		{
			int length = SaveKey.Length;
			Tester.Assert(length <= 65535);
			*(ushort*)ptr = (ushort)length;
			ptr += 2;
			fixed (char* saveKey = SaveKey)
			{
				for (int j = 0; j < length; j++)
				{
					((short*)ptr)[j] = (short)saveKey[j];
				}
			}
			ptr += 2 * length;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
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
		RelatedCharacterId = *(int*)ptr;
		ptr += 4;
		AvailableData = *ptr != 0;
		ptr++;
		IsForShopping = *ptr != 0;
		ptr++;
		ptr += CharacterNameRelatedData.Deserialize(ptr);
		ushort num = *(ushort*)ptr;
		ptr += 2;
		if (num > 0)
		{
			if (ToSelectSecretInformationDataIdList == null)
			{
				ToSelectSecretInformationDataIdList = new List<int>(num);
			}
			else
			{
				ToSelectSecretInformationDataIdList.Clear();
			}
			for (int i = 0; i < num; i++)
			{
				ToSelectSecretInformationDataIdList.Add(((int*)ptr)[i]);
			}
			ptr += 4 * num;
		}
		else
		{
			ToSelectSecretInformationDataIdList?.Clear();
		}
		ushort num2 = *(ushort*)ptr;
		ptr += 2;
		if (num2 > 0)
		{
			if (ToSelectNormalInformation == null)
			{
				ToSelectNormalInformation = new NormalInformationCollection();
			}
			ptr += ToSelectNormalInformation.Deserialize(ptr);
		}
		else
		{
			ToSelectNormalInformation = null;
		}
		ushort num3 = *(ushort*)ptr;
		ptr += 2;
		if (num3 > 0)
		{
			int num4 = 2 * num3;
			SaveKey = Encoding.Unicode.GetString(ptr, num4);
			ptr += num4;
		}
		else
		{
			SaveKey = null;
		}
		int num5 = (int)(ptr - pData);
		if (num5 > 4)
		{
			return (num5 + 3) / 4 * 4;
		}
		return num5;
	}
}
