using System.Text;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.TaiwuEvent.DisplayEvent;

[SerializableGameData(NotRestrictCollectionSerializedSize = true)]
public struct SelectItemFilter : ISerializableGameData
{
	[SerializableGameDataField]
	public short FilterTemplateId;

	[SerializableGameDataField]
	public ushort DisplayDataFilterId;

	[SerializableGameDataField]
	public string Key;

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 4;
		num = ((Key == null) ? (num + 2) : (num + (2 + 2 * Key.Length)));
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
		*(ushort*)ptr = DisplayDataFilterId;
		ptr += 2;
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
		FilterTemplateId = *(short*)ptr;
		ptr += 2;
		DisplayDataFilterId = *(ushort*)ptr;
		ptr += 2;
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
		int num3 = (int)(ptr - pData);
		if (num3 > 4)
		{
			return (num3 + 3) / 4 * 4;
		}
		return num3;
	}
}
