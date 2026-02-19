using System.Collections.Generic;
using System.Text;
using GameData.DLC.FiveLoong;
using GameData.Domains.Character.Display;
using GameData.Domains.Map;
using GameData.Domains.Organization.Display;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.LifeRecord.GeneralRecord;

[SerializableGameData(NotForArchive = true, NoCopyConstructors = true)]
public class ArgumentCollectionRenderArguments : ISerializableGameData
{
	[SerializableGameDataField]
	public string Key;

	[SerializableGameDataField]
	public List<NameAndLifeRelatedData> CharNameAndLifeDataList;

	[SerializableGameDataField]
	public List<SettlementNameRelatedData> SettlementNames;

	[SerializableGameDataField]
	public List<LocationNameRelatedData> LocationNames;

	[SerializableGameDataField]
	public List<JiaoLoongNameRelatedData> JiaoLoongNames;

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 0;
		num = ((Key == null) ? (num + 2) : (num + (2 + 2 * Key.Length)));
		num = ((CharNameAndLifeDataList == null) ? (num + 2) : (num + (2 + 36 * CharNameAndLifeDataList.Count)));
		num = ((SettlementNames == null) ? (num + 2) : (num + (2 + 4 * SettlementNames.Count)));
		num = ((LocationNames == null) ? (num + 2) : (num + (2 + 8 * LocationNames.Count)));
		num = ((JiaoLoongNames == null) ? (num + 2) : (num + (2 + 12 * JiaoLoongNames.Count)));
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		if (Key != null)
		{
			int length = Key.Length;
			Tester.Assert(length <= 65535);
			*(ushort*)ptr = (ushort)length;
			ptr += 2;
			fixed (char* key = Key)
			{
				for (int i = 0; i < length; i++)
				{
					((short*)ptr)[i] = (short)key[i];
				}
			}
			ptr += 2 * length;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		if (CharNameAndLifeDataList != null)
		{
			int count = CharNameAndLifeDataList.Count;
			Tester.Assert(count <= 65535);
			*(ushort*)ptr = (ushort)count;
			ptr += 2;
			for (int j = 0; j < count; j++)
			{
				ptr += CharNameAndLifeDataList[j].Serialize(ptr);
			}
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		if (SettlementNames != null)
		{
			int count2 = SettlementNames.Count;
			Tester.Assert(count2 <= 65535);
			*(ushort*)ptr = (ushort)count2;
			ptr += 2;
			for (int k = 0; k < count2; k++)
			{
				ptr += SettlementNames[k].Serialize(ptr);
			}
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		if (LocationNames != null)
		{
			int count3 = LocationNames.Count;
			Tester.Assert(count3 <= 65535);
			*(ushort*)ptr = (ushort)count3;
			ptr += 2;
			for (int l = 0; l < count3; l++)
			{
				ptr += LocationNames[l].Serialize(ptr);
			}
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		if (JiaoLoongNames != null)
		{
			int count4 = JiaoLoongNames.Count;
			Tester.Assert(count4 <= 65535);
			*(ushort*)ptr = (ushort)count4;
			ptr += 2;
			for (int m = 0; m < count4; m++)
			{
				ptr += JiaoLoongNames[m].Serialize(ptr);
			}
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
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
			int num2 = 2 * num;
			Key = Encoding.Unicode.GetString(ptr, num2);
			ptr += num2;
		}
		else
		{
			Key = null;
		}
		ushort num3 = *(ushort*)ptr;
		ptr += 2;
		if (num3 > 0)
		{
			if (CharNameAndLifeDataList == null)
			{
				CharNameAndLifeDataList = new List<NameAndLifeRelatedData>(num3);
			}
			else
			{
				CharNameAndLifeDataList.Clear();
			}
			for (int i = 0; i < num3; i++)
			{
				NameAndLifeRelatedData item = default(NameAndLifeRelatedData);
				ptr += item.Deserialize(ptr);
				CharNameAndLifeDataList.Add(item);
			}
		}
		else
		{
			CharNameAndLifeDataList?.Clear();
		}
		ushort num4 = *(ushort*)ptr;
		ptr += 2;
		if (num4 > 0)
		{
			if (SettlementNames == null)
			{
				SettlementNames = new List<SettlementNameRelatedData>(num4);
			}
			else
			{
				SettlementNames.Clear();
			}
			for (int j = 0; j < num4; j++)
			{
				SettlementNameRelatedData item2 = default(SettlementNameRelatedData);
				ptr += item2.Deserialize(ptr);
				SettlementNames.Add(item2);
			}
		}
		else
		{
			SettlementNames?.Clear();
		}
		ushort num5 = *(ushort*)ptr;
		ptr += 2;
		if (num5 > 0)
		{
			if (LocationNames == null)
			{
				LocationNames = new List<LocationNameRelatedData>(num5);
			}
			else
			{
				LocationNames.Clear();
			}
			for (int k = 0; k < num5; k++)
			{
				LocationNameRelatedData item3 = default(LocationNameRelatedData);
				ptr += item3.Deserialize(ptr);
				LocationNames.Add(item3);
			}
		}
		else
		{
			LocationNames?.Clear();
		}
		ushort num6 = *(ushort*)ptr;
		ptr += 2;
		if (num6 > 0)
		{
			if (JiaoLoongNames == null)
			{
				JiaoLoongNames = new List<JiaoLoongNameRelatedData>(num6);
			}
			else
			{
				JiaoLoongNames.Clear();
			}
			for (int l = 0; l < num6; l++)
			{
				JiaoLoongNameRelatedData item4 = default(JiaoLoongNameRelatedData);
				ptr += item4.Deserialize(ptr);
				JiaoLoongNames.Add(item4);
			}
		}
		else
		{
			JiaoLoongNames?.Clear();
		}
		int num7 = (int)(ptr - pData);
		if (num7 > 4)
		{
			return (num7 + 3) / 4 * 4;
		}
		return num7;
	}
}
