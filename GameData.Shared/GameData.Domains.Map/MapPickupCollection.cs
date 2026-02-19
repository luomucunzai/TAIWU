using System;
using System.Collections;
using System.Collections.Generic;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Map;

[SerializableGameData(NoCopyConstructors = true, IsExtensible = true)]
public class MapPickupCollection : ISerializableGameData, IEnumerable<MapPickup>, IEnumerable
{
	private static class FieldIds
	{
		public const ushort PickupList = 0;

		public const ushort IsNormalPickupTriggeredThisMonth = 1;

		public const ushort IsEventPickupTriggeredThisMonth = 2;

		public const ushort Count = 3;

		public static readonly string[] FieldId2FieldName = new string[3] { "PickupList", "IsNormalPickupTriggeredThisMonth", "IsEventPickupTriggeredThisMonth" };
	}

	[SerializableGameDataField]
	public List<MapPickup> PickupList;

	[Obsolete("现在可以堆叠显示了")]
	[SerializableGameDataField]
	public bool IsNormalPickupTriggeredThisMonth;

	[Obsolete("现在可以堆叠显示了")]
	[SerializableGameDataField]
	public bool IsEventPickupTriggeredThisMonth;

	public int Count
	{
		get
		{
			if (PickupList == null)
			{
				return 0;
			}
			return PickupList.Count;
		}
	}

	public MapPickupCollection()
	{
		PickupList = new List<MapPickup>();
	}

	public void AddPickup(MapPickup pickup)
	{
		if (pickup != null)
		{
			PickupList.Add(pickup);
		}
	}

	public void SetPickupAtFirst(MapPickup pickup)
	{
		if (pickup != null)
		{
			int num = PickupList.IndexOf(pickup);
			if (num < 0)
			{
				throw new ArgumentException("IgnorePickup: pickup not found in PickupList");
			}
			if (num != 0)
			{
				List<MapPickup> pickupList = PickupList;
				List<MapPickup> pickupList2 = PickupList;
				int index = num;
				MapPickup value = PickupList[num];
				MapPickup value2 = PickupList[0];
				pickupList[0] = value;
				pickupList2[index] = value2;
			}
		}
	}

	public void IgnorePickup(MapPickup pickup)
	{
		if (pickup != null)
		{
			int num = PickupList.IndexOf(pickup);
			if (num < 0)
			{
				throw new ArgumentException("IgnorePickup: pickup not found in PickupList");
			}
			pickup.Ignored = true;
			PickupList.RemoveAt(num);
			PickupList.Add(pickup);
		}
	}

	public MapPickup Get(int index)
	{
		if (PickupList == null || index < 0 || index >= PickupList.Count)
		{
			return null;
		}
		return PickupList[index];
	}

	public bool ClearIgnoredAndTriggered()
	{
		bool result = false;
		foreach (MapPickup pickup in PickupList)
		{
			if (pickup.Ignored)
			{
				result = true;
				pickup.Ignored = false;
			}
		}
		return result;
	}

	public IEnumerator<MapPickup> GetEnumerator()
	{
		return PickupList.GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 4;
		if (PickupList != null)
		{
			num += 2;
			int count = PickupList.Count;
			for (int i = 0; i < count; i++)
			{
				MapPickup mapPickup = PickupList[i];
				num = ((mapPickup == null) ? (num + 2) : (num + (2 + mapPickup.GetSerializedSize())));
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
		*(short*)ptr = 3;
		ptr += 2;
		if (PickupList != null)
		{
			int count = PickupList.Count;
			Tester.Assert(count <= 65535);
			*(ushort*)ptr = (ushort)count;
			ptr += 2;
			for (int i = 0; i < count; i++)
			{
				MapPickup mapPickup = PickupList[i];
				if (mapPickup != null)
				{
					byte* intPtr = ptr;
					ptr += 2;
					int num = mapPickup.Serialize(ptr);
					ptr += num;
					Tester.Assert(num <= 65535);
					*(ushort*)intPtr = (ushort)num;
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
		*ptr = (IsNormalPickupTriggeredThisMonth ? ((byte)1) : ((byte)0));
		ptr++;
		*ptr = (IsEventPickupTriggeredThisMonth ? ((byte)1) : ((byte)0));
		ptr++;
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
		ushort num = *(ushort*)ptr;
		ptr += 2;
		if (num > 0)
		{
			ushort num2 = *(ushort*)ptr;
			ptr += 2;
			if (num2 > 0)
			{
				if (PickupList == null)
				{
					PickupList = new List<MapPickup>(num2);
				}
				else
				{
					PickupList.Clear();
				}
				for (int i = 0; i < num2; i++)
				{
					ushort num3 = *(ushort*)ptr;
					ptr += 2;
					if (num3 > 0)
					{
						MapPickup mapPickup = new MapPickup();
						ptr += mapPickup.Deserialize(ptr);
						PickupList.Add(mapPickup);
					}
					else
					{
						PickupList.Add(null);
					}
				}
			}
			else
			{
				PickupList?.Clear();
			}
		}
		if (num > 1)
		{
			IsNormalPickupTriggeredThisMonth = *ptr != 0;
			ptr++;
		}
		if (num > 2)
		{
			IsEventPickupTriggeredThisMonth = *ptr != 0;
			ptr++;
		}
		int num4 = (int)(ptr - pData);
		if (num4 > 4)
		{
			return (num4 + 3) / 4 * 4;
		}
		return num4;
	}
}
