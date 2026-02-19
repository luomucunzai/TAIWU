using System.Collections.Generic;
using System.Text;
using GameData.Domains.Map;
using GameData.Domains.Organization.Display;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.World.Task;

public struct TaskDisplayData : ISerializableGameData
{
	[SerializableGameDataField]
	public TaskData InnerTaskData;

	[SerializableGameDataField]
	public Location TargetLocation;

	[SerializableGameDataField]
	public ShortList SkillIdList;

	[SerializableGameDataField]
	public int CountDown;

	[SerializableGameDataField]
	public string[] StringArray;

	[SerializableGameDataField]
	public int DisplayType;

	[SerializableGameDataField]
	public SettlementNameRelatedData SettlementNameData;

	[SerializableGameDataField]
	public List<Location> TargetLocations;

	[SerializableGameDataField]
	public List<SettlementNameRelatedData> SettlementNameDatas;

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 28;
		num += SkillIdList.GetSerializedSize();
		if (StringArray != null)
		{
			num += 2;
			int num2 = StringArray.Length;
			for (int i = 0; i < num2; i++)
			{
				string text = StringArray[i];
				num = ((text == null) ? (num + 2) : (num + (2 + 2 * text.Length)));
			}
		}
		else
		{
			num += 2;
		}
		num = ((TargetLocations == null) ? (num + 2) : (num + (2 + 4 * TargetLocations.Count)));
		num = ((SettlementNameDatas == null) ? (num + 2) : (num + (2 + 4 * SettlementNameDatas.Count)));
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		ptr += InnerTaskData.Serialize(ptr);
		ptr += TargetLocation.Serialize(ptr);
		int num = SkillIdList.Serialize(ptr);
		ptr += num;
		Tester.Assert(num <= 65535);
		*(int*)ptr = CountDown;
		ptr += 4;
		if (StringArray != null)
		{
			int num2 = StringArray.Length;
			Tester.Assert(num2 <= 65535);
			*(ushort*)ptr = (ushort)num2;
			ptr += 2;
			for (int i = 0; i < num2; i++)
			{
				string text = StringArray[i];
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
		*(int*)ptr = DisplayType;
		ptr += 4;
		ptr += SettlementNameData.Serialize(ptr);
		if (TargetLocations != null)
		{
			int count = TargetLocations.Count;
			Tester.Assert(count <= 65535);
			*(ushort*)ptr = (ushort)count;
			ptr += 2;
			for (int k = 0; k < count; k++)
			{
				ptr += TargetLocations[k].Serialize(ptr);
			}
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		if (SettlementNameDatas != null)
		{
			int count2 = SettlementNameDatas.Count;
			Tester.Assert(count2 <= 65535);
			*(ushort*)ptr = (ushort)count2;
			ptr += 2;
			for (int l = 0; l < count2; l++)
			{
				ptr += SettlementNameDatas[l].Serialize(ptr);
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
		ptr += InnerTaskData.Deserialize(ptr);
		ptr += TargetLocation.Deserialize(ptr);
		ptr += SkillIdList.Deserialize(ptr);
		CountDown = *(int*)ptr;
		ptr += 4;
		ushort num = *(ushort*)ptr;
		ptr += 2;
		if (num > 0)
		{
			if (StringArray == null || StringArray.Length != num)
			{
				StringArray = new string[num];
			}
			for (int i = 0; i < num; i++)
			{
				ushort num2 = *(ushort*)ptr;
				ptr += 2;
				if (num2 > 0)
				{
					int num3 = 2 * num2;
					StringArray[i] = Encoding.Unicode.GetString(ptr, num3);
					ptr += num3;
				}
				else
				{
					StringArray[i] = null;
				}
			}
		}
		else
		{
			StringArray = null;
		}
		DisplayType = *(int*)ptr;
		ptr += 4;
		ptr += SettlementNameData.Deserialize(ptr);
		ushort num4 = *(ushort*)ptr;
		ptr += 2;
		if (num4 > 0)
		{
			if (TargetLocations == null)
			{
				TargetLocations = new List<Location>(num4);
			}
			else
			{
				TargetLocations.Clear();
			}
			for (int j = 0; j < num4; j++)
			{
				Location item = default(Location);
				ptr += item.Deserialize(ptr);
				TargetLocations.Add(item);
			}
		}
		else
		{
			TargetLocations?.Clear();
		}
		ushort num5 = *(ushort*)ptr;
		ptr += 2;
		if (num5 > 0)
		{
			if (SettlementNameDatas == null)
			{
				SettlementNameDatas = new List<SettlementNameRelatedData>(num5);
			}
			else
			{
				SettlementNameDatas.Clear();
			}
			for (int k = 0; k < num5; k++)
			{
				SettlementNameRelatedData item2 = default(SettlementNameRelatedData);
				ptr += item2.Deserialize(ptr);
				SettlementNameDatas.Add(item2);
			}
		}
		else
		{
			SettlementNameDatas?.Clear();
		}
		int num6 = (int)(ptr - pData);
		if (num6 > 4)
		{
			return (num6 + 3) / 4 * 4;
		}
		return num6;
	}
}
