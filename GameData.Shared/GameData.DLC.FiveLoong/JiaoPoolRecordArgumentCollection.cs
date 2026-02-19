using System.Collections.Generic;
using System.Text;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.DLC.FiveLoong;

public class JiaoPoolRecordArgumentCollection : ISerializableGameData
{
	[SerializableGameDataField]
	public List<int> JiaoIdList;

	[SerializableGameDataField]
	public List<string> JiaoNameList;

	public Dictionary<int, string> JiaoNameMap;

	public void InitMap()
	{
		if (JiaoNameMap == null)
		{
			JiaoNameMap = new Dictionary<int, string>();
		}
		JiaoNameMap.Clear();
		int i = 0;
		for (int count = JiaoIdList.Count; i < count; i++)
		{
			JiaoNameMap.Add(JiaoIdList[i], JiaoNameList[i]);
		}
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 0;
		num = ((JiaoIdList == null) ? (num + 2) : (num + (2 + 4 * JiaoIdList.Count)));
		if (JiaoNameList != null)
		{
			num += 2;
			int count = JiaoNameList.Count;
			for (int i = 0; i < count; i++)
			{
				string text = JiaoNameList[i];
				num = ((text == null) ? (num + 2) : (num + (2 + 2 * text.Length)));
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
		if (JiaoIdList != null)
		{
			int count = JiaoIdList.Count;
			Tester.Assert(count <= 65535);
			*(ushort*)ptr = (ushort)count;
			ptr += 2;
			for (int i = 0; i < count; i++)
			{
				((int*)ptr)[i] = JiaoIdList[i];
			}
			ptr += 4 * count;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		if (JiaoNameList != null)
		{
			int count2 = JiaoNameList.Count;
			Tester.Assert(count2 <= 65535);
			*(ushort*)ptr = (ushort)count2;
			ptr += 2;
			for (int j = 0; j < count2; j++)
			{
				string text = JiaoNameList[j];
				if (text != null)
				{
					int length = text.Length;
					Tester.Assert(length <= 65535);
					*(ushort*)ptr = (ushort)length;
					ptr += 2;
					fixed (char* ptr2 = text)
					{
						for (int k = 0; k < length; k++)
						{
							((short*)ptr)[k] = (short)ptr2[k];
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
			if (JiaoIdList == null)
			{
				JiaoIdList = new List<int>(num);
			}
			else
			{
				JiaoIdList.Clear();
			}
			for (int i = 0; i < num; i++)
			{
				JiaoIdList.Add(((int*)ptr)[i]);
			}
			ptr += 4 * num;
		}
		else
		{
			JiaoIdList?.Clear();
		}
		ushort num2 = *(ushort*)ptr;
		ptr += 2;
		if (num2 > 0)
		{
			if (JiaoNameList == null)
			{
				JiaoNameList = new List<string>(num2);
			}
			else
			{
				JiaoNameList.Clear();
			}
			for (int j = 0; j < num2; j++)
			{
				ushort num3 = *(ushort*)ptr;
				ptr += 2;
				if (num3 > 0)
				{
					int num4 = 2 * num3;
					JiaoNameList.Add(Encoding.Unicode.GetString(ptr, num4));
					ptr += num4;
				}
				else
				{
					JiaoNameList.Add(null);
				}
			}
		}
		else
		{
			JiaoNameList?.Clear();
		}
		int num5 = (int)(ptr - pData);
		if (num5 > 4)
		{
			return (num5 + 3) / 4 * 4;
		}
		return num5;
	}
}
