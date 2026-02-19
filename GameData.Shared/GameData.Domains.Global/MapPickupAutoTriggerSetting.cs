using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Global;

[SerializableGameData(NotForArchive = true, NoCopyConstructors = true)]
public class MapPickupAutoTriggerSetting : ISerializableGameData
{
	[SerializableGameDataField]
	public bool IncludeXiangshuMinion;

	[SerializableGameDataField]
	public sbyte MinGrade;

	[SerializableGameDataField]
	public bool[] PickupTypes = new bool[14];

	public MapPickupAutoTriggerSetting()
	{
		IncludeXiangshuMinion = true;
		MinGrade = -1;
		PickupTypes = new bool[14];
		for (int i = 0; i < PickupTypes.Length; i++)
		{
			PickupTypes[i] = true;
		}
	}

	public MapPickupAutoTriggerSetting(bool includeXiangshuMinion, sbyte minGrade, int pickupTypeMask)
	{
		IncludeXiangshuMinion = includeXiangshuMinion;
		MinGrade = minGrade;
		PickupTypes = new bool[14];
		for (int i = 0; i < PickupTypes.Length; i++)
		{
			PickupTypes[i] = ((pickupTypeMask >> i) & 1) == 1;
		}
	}

	public int GetPickupTypeMask()
	{
		int num = 0;
		for (int i = 0; i < PickupTypes.Length; i++)
		{
			if (PickupTypes[i])
			{
				num |= 1 << i;
			}
		}
		return num;
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 2;
		num = ((PickupTypes == null) ? (num + 2) : (num + (2 + PickupTypes.Length)));
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		*ptr = (IncludeXiangshuMinion ? ((byte)1) : ((byte)0));
		ptr++;
		*ptr = (byte)MinGrade;
		ptr++;
		if (PickupTypes != null)
		{
			int num = PickupTypes.Length;
			Tester.Assert(num <= 65535);
			*(ushort*)ptr = (ushort)num;
			ptr += 2;
			for (int i = 0; i < num; i++)
			{
				ptr[i] = (PickupTypes[i] ? ((byte)1) : ((byte)0));
			}
			ptr += num;
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
		IncludeXiangshuMinion = *ptr != 0;
		ptr++;
		MinGrade = (sbyte)(*ptr);
		ptr++;
		ushort num = *(ushort*)ptr;
		ptr += 2;
		if (num > 0)
		{
			if (PickupTypes == null || PickupTypes.Length != num)
			{
				PickupTypes = new bool[num];
			}
			for (int i = 0; i < num; i++)
			{
				PickupTypes[i] = ptr[i] != 0;
			}
			ptr += (int)num;
		}
		else
		{
			PickupTypes = null;
		}
		int num2 = (int)(ptr - pData);
		if (num2 > 4)
		{
			return (num2 + 3) / 4 * 4;
		}
		return num2;
	}
}
