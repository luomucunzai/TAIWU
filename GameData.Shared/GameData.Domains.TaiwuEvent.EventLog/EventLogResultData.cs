using System.Collections.Generic;
using System.Text;
using GameData.Domains.TaiwuEvent.DisplayEvent;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.TaiwuEvent.EventLog;

public class EventLogResultData : ISerializableGameData
{
	[SerializableGameDataField]
	public sbyte Type;

	[SerializableGameDataField]
	public bool IsLosing;

	[SerializableGameDataField]
	public List<int> ValueList;

	[SerializableGameDataField]
	public string Text;

	[SerializableGameDataField]
	public EventActorData LeftActorData;

	[SerializableGameDataField]
	public EventActorData RightActorData;

	[SerializableGameDataField]
	public string LeftName;

	[SerializableGameDataField]
	public string RightName;

	public EventLogResultData()
	{
		Type = -1;
		IsLosing = false;
		ValueList = new List<int> { 0 };
		Text = null;
		LeftActorData = null;
		RightActorData = null;
		LeftName = null;
		RightName = null;
	}

	public override string ToString()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(string.Format("type: {0}; isLosing: {1}; text: {2}; avatar count: {3}; avatars: ", Type, IsLosing, Text ?? "null", ValueList[0]));
		for (int i = 1; i <= ValueList[0]; i++)
		{
			stringBuilder.Append($"{ValueList[i]}, ");
		}
		stringBuilder.Append("; remaining values: ");
		for (int j = ValueList[0] + 1; j < ValueList.Count; j++)
		{
			stringBuilder.Append($"{ValueList[j]}, ");
		}
		return stringBuilder.ToString();
	}

	public EventLogResultData(EventLogResultData other)
	{
		Type = other.Type;
		IsLosing = other.IsLosing;
		ValueList = ((other.ValueList == null) ? null : new List<int>(other.ValueList));
		Text = other.Text;
		LeftActorData = new EventActorData(other.LeftActorData);
		RightActorData = new EventActorData(other.RightActorData);
		LeftName = other.LeftName;
		RightName = other.RightName;
	}

	public void Assign(EventLogResultData other)
	{
		Type = other.Type;
		IsLosing = other.IsLosing;
		ValueList = ((other.ValueList == null) ? null : new List<int>(other.ValueList));
		Text = other.Text;
		LeftActorData = new EventActorData(other.LeftActorData);
		RightActorData = new EventActorData(other.RightActorData);
		LeftName = other.LeftName;
		RightName = other.RightName;
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 2;
		num = ((ValueList == null) ? (num + 2) : (num + (2 + 4 * ValueList.Count)));
		num = ((Text == null) ? (num + 2) : (num + (2 + 2 * Text.Length)));
		num = ((LeftActorData == null) ? (num + 2) : (num + (2 + LeftActorData.GetSerializedSize())));
		num = ((RightActorData == null) ? (num + 2) : (num + (2 + RightActorData.GetSerializedSize())));
		num = ((LeftName == null) ? (num + 2) : (num + (2 + 2 * LeftName.Length)));
		num = ((RightName == null) ? (num + 2) : (num + (2 + 2 * RightName.Length)));
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		*ptr = (byte)Type;
		ptr++;
		*ptr = (IsLosing ? ((byte)1) : ((byte)0));
		ptr++;
		if (ValueList != null)
		{
			int count = ValueList.Count;
			Tester.Assert(count <= 65535);
			*(ushort*)ptr = (ushort)count;
			ptr += 2;
			for (int i = 0; i < count; i++)
			{
				((int*)ptr)[i] = ValueList[i];
			}
			ptr += 4 * count;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		if (Text != null)
		{
			int length = Text.Length;
			Tester.Assert(length <= 65535);
			*(ushort*)ptr = (ushort)length;
			ptr += 2;
			fixed (char* text = Text)
			{
				for (int j = 0; j < length; j++)
				{
					((short*)ptr)[j] = (short)text[j];
				}
			}
			ptr += 2 * length;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		if (LeftActorData != null)
		{
			byte* intPtr = ptr;
			ptr += 2;
			int num = LeftActorData.Serialize(ptr);
			ptr += num;
			Tester.Assert(num <= 65535);
			*(ushort*)intPtr = (ushort)num;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		if (RightActorData != null)
		{
			byte* intPtr2 = ptr;
			ptr += 2;
			int num2 = RightActorData.Serialize(ptr);
			ptr += num2;
			Tester.Assert(num2 <= 65535);
			*(ushort*)intPtr2 = (ushort)num2;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		if (LeftName != null)
		{
			int length2 = LeftName.Length;
			Tester.Assert(length2 <= 65535);
			*(ushort*)ptr = (ushort)length2;
			ptr += 2;
			fixed (char* leftName = LeftName)
			{
				for (int k = 0; k < length2; k++)
				{
					((short*)ptr)[k] = (short)leftName[k];
				}
			}
			ptr += 2 * length2;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		if (RightName != null)
		{
			int length3 = RightName.Length;
			Tester.Assert(length3 <= 65535);
			*(ushort*)ptr = (ushort)length3;
			ptr += 2;
			fixed (char* rightName = RightName)
			{
				for (int l = 0; l < length3; l++)
				{
					((short*)ptr)[l] = (short)rightName[l];
				}
			}
			ptr += 2 * length3;
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
		Type = (sbyte)(*ptr);
		ptr++;
		IsLosing = *ptr != 0;
		ptr++;
		ushort num = *(ushort*)ptr;
		ptr += 2;
		if (num > 0)
		{
			if (ValueList == null)
			{
				ValueList = new List<int>(num);
			}
			else
			{
				ValueList.Clear();
			}
			for (int i = 0; i < num; i++)
			{
				ValueList.Add(((int*)ptr)[i]);
			}
			ptr += 4 * num;
		}
		else
		{
			ValueList?.Clear();
		}
		ushort num2 = *(ushort*)ptr;
		ptr += 2;
		if (num2 > 0)
		{
			int num3 = 2 * num2;
			Text = Encoding.Unicode.GetString(ptr, num3);
			ptr += num3;
		}
		else
		{
			Text = null;
		}
		ushort num4 = *(ushort*)ptr;
		ptr += 2;
		if (num4 > 0)
		{
			if (LeftActorData == null)
			{
				LeftActorData = new EventActorData();
			}
			ptr += LeftActorData.Deserialize(ptr);
		}
		else
		{
			LeftActorData = null;
		}
		ushort num5 = *(ushort*)ptr;
		ptr += 2;
		if (num5 > 0)
		{
			if (RightActorData == null)
			{
				RightActorData = new EventActorData();
			}
			ptr += RightActorData.Deserialize(ptr);
		}
		else
		{
			RightActorData = null;
		}
		ushort num6 = *(ushort*)ptr;
		ptr += 2;
		if (num6 > 0)
		{
			int num7 = 2 * num6;
			LeftName = Encoding.Unicode.GetString(ptr, num7);
			ptr += num7;
		}
		else
		{
			LeftName = null;
		}
		ushort num8 = *(ushort*)ptr;
		ptr += 2;
		if (num8 > 0)
		{
			int num9 = 2 * num8;
			RightName = Encoding.Unicode.GetString(ptr, num9);
			ptr += num9;
		}
		else
		{
			RightName = null;
		}
		int num10 = (int)(ptr - pData);
		if (num10 > 4)
		{
			return (num10 + 3) / 4 * 4;
		}
		return num10;
	}
}
