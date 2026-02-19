using System;
using System.Collections.Generic;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Organization;

[SerializableGameData(IsExtensible = true, NoCopyConstructors = true)]
public class SettlementLayeredTreasuries : ISerializableGameData
{
	private static class FieldIds
	{
		public const ushort SettlementTreasuries = 0;

		public const ushort AlertTime = 1;

		public const ushort ResupplyTotalValue = 2;

		public const ushort CurrentTotalValue = 3;

		public const ushort SupplyLevelAddOn = 4;

		public const ushort Count = 5;

		public static readonly string[] FieldId2FieldName = new string[5] { "SettlementTreasuries", "AlertTime", "ResupplyTotalValue", "CurrentTotalValue", "SupplyLevelAddOn" };
	}

	[SerializableGameDataField]
	public SettlementTreasury[] SettlementTreasuries = InitTreasuries();

	[SerializableGameDataField]
	public byte AlertTime;

	[SerializableGameDataField]
	public int ResupplyTotalValue;

	[SerializableGameDataField]
	public int CurrentTotalValue;

	[SerializableGameDataField]
	public int SupplyLevelAddOn;

	private static SettlementTreasury[] InitTreasuries()
	{
		int length = Enum.GetValues(typeof(SettlementTreasuryLayers)).Length;
		SettlementTreasury[] array = new SettlementTreasury[length];
		for (sbyte b = 0; b < length; b++)
		{
			array[b] = new SettlementTreasury
			{
				LayerIndex = b
			};
		}
		return array;
	}

	public SettlementTreasury GetTreasury(SettlementTreasuryLayers layer)
	{
		return layer switch
		{
			SettlementTreasuryLayers.Shallow => SettlementTreasuries[0], 
			SettlementTreasuryLayers.Mid => SettlementTreasuries[1], 
			SettlementTreasuryLayers.Deep => SettlementTreasuries[2], 
			_ => throw new ArgumentOutOfRangeException("layer", layer, null), 
		};
	}

	public SettlementTreasury GetTreasury(sbyte layerIndex)
	{
		return SettlementTreasuries[layerIndex];
	}

	public sbyte GetTreasuryResourceStatus()
	{
		if (ResupplyTotalValue == 0)
		{
			return 1;
		}
		int num = CurrentTotalValue * 100 / ResupplyTotalValue;
		if (num >= GlobalConfig.Instance.TreasuryStatusThreshold[0])
		{
			if (num < GlobalConfig.Instance.TreasuryStatusThreshold[1])
			{
				return 1;
			}
			return 2;
		}
		return 0;
	}

	public bool TryRemoveGuard(int charId, out sbyte layerIndex)
	{
		layerIndex = -1;
		SettlementTreasury[] settlementTreasuries = SettlementTreasuries;
		foreach (SettlementTreasury settlementTreasury in settlementTreasuries)
		{
			if (settlementTreasury.GuardIds.Remove(charId).Item2)
			{
				layerIndex = settlementTreasury.LayerIndex;
				return true;
			}
		}
		return false;
	}

	public void GetGuardIds(HashSet<int> ids)
	{
		SettlementTreasury[] settlementTreasuries = SettlementTreasuries;
		for (int i = 0; i < settlementTreasuries.Length; i++)
		{
			foreach (int item in settlementTreasuries[i].GuardIds.GetCollection())
			{
				ids.Add(item);
			}
		}
	}

	public IEnumerable<int> GetGuardIds()
	{
		SettlementTreasury[] settlementTreasuries = SettlementTreasuries;
		foreach (SettlementTreasury settlementTreasury in settlementTreasuries)
		{
			foreach (int item in settlementTreasury.GuardIds.GetCollection())
			{
				yield return item;
			}
		}
	}

	public bool IsGuard(int id)
	{
		SettlementTreasury[] settlementTreasuries = SettlementTreasuries;
		for (int i = 0; i < settlementTreasuries.Length; i++)
		{
			if (settlementTreasuries[i].GuardIds.Contains(id))
			{
				return true;
			}
		}
		return false;
	}

	public byte GuardLevel(int id)
	{
		for (byte b = 3; b != 0; b--)
		{
			if (SettlementTreasuries[b - 1].GuardIds.Contains(id))
			{
				return b;
			}
		}
		return 0;
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 15;
		if (SettlementTreasuries != null)
		{
			num += 2;
			int num2 = SettlementTreasuries.Length;
			for (int i = 0; i < num2; i++)
			{
				SettlementTreasury settlementTreasury = SettlementTreasuries[i];
				num = ((settlementTreasury == null) ? (num + 2) : (num + (2 + settlementTreasury.GetSerializedSize())));
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
		*(short*)ptr = 5;
		ptr += 2;
		if (SettlementTreasuries != null)
		{
			int num = SettlementTreasuries.Length;
			Tester.Assert(num <= 65535);
			*(ushort*)ptr = (ushort)num;
			ptr += 2;
			for (int i = 0; i < num; i++)
			{
				SettlementTreasury settlementTreasury = SettlementTreasuries[i];
				if (settlementTreasury != null)
				{
					byte* intPtr = ptr;
					ptr += 2;
					int num2 = settlementTreasury.Serialize(ptr);
					ptr += num2;
					Tester.Assert(num2 <= 65535);
					*(ushort*)intPtr = (ushort)num2;
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
		*ptr = AlertTime;
		ptr++;
		*(int*)ptr = ResupplyTotalValue;
		ptr += 4;
		*(int*)ptr = CurrentTotalValue;
		ptr += 4;
		*(int*)ptr = SupplyLevelAddOn;
		ptr += 4;
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
		ushort num = *(ushort*)ptr;
		ptr += 2;
		if (num > 0)
		{
			ushort num2 = *(ushort*)ptr;
			ptr += 2;
			if (num2 > 0)
			{
				if (SettlementTreasuries == null || SettlementTreasuries.Length != num2)
				{
					SettlementTreasuries = new SettlementTreasury[num2];
				}
				for (int i = 0; i < num2; i++)
				{
					ushort num3 = *(ushort*)ptr;
					ptr += 2;
					if (num3 > 0)
					{
						SettlementTreasury settlementTreasury = SettlementTreasuries[i] ?? new SettlementTreasury();
						ptr += settlementTreasury.Deserialize(ptr);
						SettlementTreasuries[i] = settlementTreasury;
					}
					else
					{
						SettlementTreasuries[i] = null;
					}
				}
			}
			else
			{
				SettlementTreasuries = null;
			}
		}
		if (num > 1)
		{
			AlertTime = *ptr;
			ptr++;
		}
		if (num > 2)
		{
			ResupplyTotalValue = *(int*)ptr;
			ptr += 4;
		}
		if (num > 3)
		{
			CurrentTotalValue = *(int*)ptr;
			ptr += 4;
		}
		if (num > 4)
		{
			SupplyLevelAddOn = *(int*)ptr;
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
