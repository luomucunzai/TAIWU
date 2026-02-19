using System.Collections.Generic;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Extra;

[SerializableGameData(IsExtensible = true)]
public class SectStoryThiefData : ISerializableGameData
{
	private static class FieldIds
	{
		public const ushort CatchThiefTimes = 0;

		public const ushort AreaId = 1;

		public const ushort ThiefBlockIds = 2;

		public const ushort ThiefTriggered = 3;

		public const ushort RealThiefIndex = 4;

		public const ushort Count = 5;

		public static readonly string[] FieldId2FieldName = new string[5] { "CatchThiefTimes", "AreaId", "ThiefBlockIds", "ThiefTriggered", "RealThiefIndex" };
	}

	[SerializableGameDataField]
	public int CatchThiefTimes;

	[SerializableGameDataField]
	public short AreaId;

	[SerializableGameDataField]
	public List<short> ThiefBlockIds;

	[SerializableGameDataField]
	public List<bool> ThiefTriggered;

	[SerializableGameDataField]
	public int RealThiefIndex;

	public bool AllIsTriggered()
	{
		bool flag = true;
		for (int i = 0; i < ThiefTriggered.Count; i++)
		{
			flag = flag && ThiefTriggered[i];
		}
		return flag;
	}

	public SectStoryThiefData()
	{
	}

	public SectStoryThiefData(SectStoryThiefData other)
	{
		CatchThiefTimes = other.CatchThiefTimes;
		AreaId = other.AreaId;
		ThiefBlockIds = ((other.ThiefBlockIds == null) ? null : new List<short>(other.ThiefBlockIds));
		ThiefTriggered = ((other.ThiefTriggered == null) ? null : new List<bool>(other.ThiefTriggered));
		RealThiefIndex = other.RealThiefIndex;
	}

	public void Assign(SectStoryThiefData other)
	{
		CatchThiefTimes = other.CatchThiefTimes;
		AreaId = other.AreaId;
		ThiefBlockIds = ((other.ThiefBlockIds == null) ? null : new List<short>(other.ThiefBlockIds));
		ThiefTriggered = ((other.ThiefTriggered == null) ? null : new List<bool>(other.ThiefTriggered));
		RealThiefIndex = other.RealThiefIndex;
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 12;
		num = ((ThiefBlockIds == null) ? (num + 2) : (num + (2 + 2 * ThiefBlockIds.Count)));
		num = ((ThiefTriggered == null) ? (num + 2) : (num + (2 + ThiefTriggered.Count)));
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		*(short*)ptr = 5;
		ptr += 2;
		*(int*)ptr = CatchThiefTimes;
		ptr += 4;
		*(short*)ptr = AreaId;
		ptr += 2;
		if (ThiefBlockIds != null)
		{
			int count = ThiefBlockIds.Count;
			Tester.Assert(count <= 65535);
			*(ushort*)ptr = (ushort)count;
			ptr += 2;
			for (int i = 0; i < count; i++)
			{
				((short*)ptr)[i] = ThiefBlockIds[i];
			}
			ptr += 2 * count;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		if (ThiefTriggered != null)
		{
			int count2 = ThiefTriggered.Count;
			Tester.Assert(count2 <= 65535);
			*(ushort*)ptr = (ushort)count2;
			ptr += 2;
			for (int j = 0; j < count2; j++)
			{
				ptr[j] = (ThiefTriggered[j] ? ((byte)1) : ((byte)0));
			}
			ptr += count2;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		*(int*)ptr = RealThiefIndex;
		ptr += 4;
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
			CatchThiefTimes = *(int*)ptr;
			ptr += 4;
		}
		if (num > 1)
		{
			AreaId = *(short*)ptr;
			ptr += 2;
		}
		if (num > 2)
		{
			ushort num2 = *(ushort*)ptr;
			ptr += 2;
			if (num2 > 0)
			{
				if (ThiefBlockIds == null)
				{
					ThiefBlockIds = new List<short>(num2);
				}
				else
				{
					ThiefBlockIds.Clear();
				}
				for (int i = 0; i < num2; i++)
				{
					ThiefBlockIds.Add(((short*)ptr)[i]);
				}
				ptr += 2 * num2;
			}
			else
			{
				ThiefBlockIds?.Clear();
			}
		}
		if (num > 3)
		{
			ushort num3 = *(ushort*)ptr;
			ptr += 2;
			if (num3 > 0)
			{
				if (ThiefTriggered == null)
				{
					ThiefTriggered = new List<bool>(num3);
				}
				else
				{
					ThiefTriggered.Clear();
				}
				for (int j = 0; j < num3; j++)
				{
					ThiefTriggered.Add(ptr[j] != 0);
				}
				ptr += (int)num3;
			}
			else
			{
				ThiefTriggered?.Clear();
			}
		}
		if (num > 4)
		{
			RealThiefIndex = *(int*)ptr;
			ptr += 4;
		}
		int num4 = (int)(ptr - pData);
		if (num4 > 4)
		{
			return (num4 + 3) / 4 * 4;
		}
		return num4;
	}
}
