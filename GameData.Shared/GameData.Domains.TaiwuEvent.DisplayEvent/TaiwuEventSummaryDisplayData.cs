using System.Text;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.TaiwuEvent.DisplayEvent;

public class TaiwuEventSummaryDisplayData : ISerializableGameData
{
	[SerializableGameDataField]
	public string EventGuid;

	[SerializableGameDataField]
	public int CharacterId;

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 4;
		num = ((EventGuid == null) ? (num + 2) : (num + (2 + 2 * EventGuid.Length)));
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		if (EventGuid != null)
		{
			int length = EventGuid.Length;
			Tester.Assert(length <= 65535);
			*(ushort*)ptr = (ushort)length;
			ptr += 2;
			fixed (char* eventGuid = EventGuid)
			{
				for (int i = 0; i < length; i++)
				{
					((short*)ptr)[i] = (short)eventGuid[i];
				}
			}
			ptr += 2 * length;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		*(int*)ptr = CharacterId;
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
			int num2 = 2 * num;
			EventGuid = Encoding.Unicode.GetString(ptr, num2);
			ptr += num2;
		}
		else
		{
			EventGuid = null;
		}
		CharacterId = *(int*)ptr;
		ptr += 4;
		int num3 = (int)(ptr - pData);
		if (num3 > 4)
		{
			return (num3 + 3) / 4 * 4;
		}
		return num3;
	}
}
