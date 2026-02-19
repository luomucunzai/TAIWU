using System.Collections.Generic;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Combat;

[SerializableGameData]
public class MindMarkList : ISerializableGameData
{
	[SerializableGameDataField]
	public List<SilenceFrameData> MarkList = new List<SilenceFrameData>();

	public MindMarkList()
	{
	}

	public MindMarkList(MindMarkList other)
	{
		MarkList = ((other.MarkList == null) ? null : new List<SilenceFrameData>(other.MarkList));
	}

	public void Assign(MindMarkList other)
	{
		MarkList = ((other.MarkList == null) ? null : new List<SilenceFrameData>(other.MarkList));
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 0;
		num = ((MarkList == null) ? (num + 2) : (num + (2 + 8 * MarkList.Count)));
		return (num <= 4) ? num : ((num + 3) / 4 * 4);
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		if (MarkList != null)
		{
			int count = MarkList.Count;
			Tester.Assert(count <= 65535);
			*(ushort*)ptr = (ushort)count;
			ptr += 2;
			for (int i = 0; i < count; i++)
			{
				ptr += MarkList[i].Serialize(ptr);
			}
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		int num = (int)(ptr - pData);
		return (num <= 4) ? num : ((num + 3) / 4 * 4);
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		ushort num = *(ushort*)ptr;
		ptr += 2;
		if (num > 0)
		{
			if (MarkList == null)
			{
				MarkList = new List<SilenceFrameData>(num);
			}
			else
			{
				MarkList.Clear();
			}
			for (int i = 0; i < num; i++)
			{
				SilenceFrameData item = default(SilenceFrameData);
				ptr += item.Deserialize(ptr);
				MarkList.Add(item);
			}
		}
		else
		{
			MarkList?.Clear();
		}
		int num2 = (int)(ptr - pData);
		return (num2 <= 4) ? num2 : ((num2 + 3) / 4 * 4);
	}
}
