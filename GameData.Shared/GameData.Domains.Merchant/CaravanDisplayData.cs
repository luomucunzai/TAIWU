using System.Collections.Generic;
using GameData.Domains.Organization.Display;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Merchant;

[SerializableGameData(NotRestrictCollectionSerializedSize = true)]
public class CaravanDisplayData : ISerializableGameData
{
	[SerializableGameDataField]
	public int CaravanId;

	[SerializableGameDataField]
	public short MerchantTemplateId;

	[SerializableGameDataField]
	public short TargetArea;

	[SerializableGameDataField]
	public int Favorability;

	[SerializableGameDataField]
	public CaravanPath PathInArea = new CaravanPath();

	[SerializableGameDataField]
	public CaravanExtraData ExtraData = new CaravanExtraData();

	[SerializableGameDataField]
	public List<SettlementDisplayData> SettlementDisplayDataList;

	public override string ToString()
	{
		return $"商队 ID{CaravanId}，{ExtraData?.ToString() ?? string.Empty}";
	}

	public CaravanDisplayData()
	{
	}

	public CaravanDisplayData(CaravanDisplayData other)
	{
		CaravanId = other.CaravanId;
		MerchantTemplateId = other.MerchantTemplateId;
		TargetArea = other.TargetArea;
		Favorability = other.Favorability;
		PathInArea = new CaravanPath(other.PathInArea);
		ExtraData = new CaravanExtraData(other.ExtraData);
		SettlementDisplayDataList = ((other.SettlementDisplayDataList == null) ? null : new List<SettlementDisplayData>(other.SettlementDisplayDataList));
	}

	public void Assign(CaravanDisplayData other)
	{
		CaravanId = other.CaravanId;
		MerchantTemplateId = other.MerchantTemplateId;
		TargetArea = other.TargetArea;
		Favorability = other.Favorability;
		PathInArea = new CaravanPath(other.PathInArea);
		ExtraData = new CaravanExtraData(other.ExtraData);
		SettlementDisplayDataList = ((other.SettlementDisplayDataList == null) ? null : new List<SettlementDisplayData>(other.SettlementDisplayDataList));
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 12;
		num = ((PathInArea == null) ? (num + 2) : (num + (2 + PathInArea.GetSerializedSize())));
		num = ((ExtraData == null) ? (num + 2) : (num + (2 + ExtraData.GetSerializedSize())));
		num = ((SettlementDisplayDataList == null) ? (num + 2) : (num + (2 + 32 * SettlementDisplayDataList.Count)));
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		*(int*)ptr = CaravanId;
		ptr += 4;
		*(short*)ptr = MerchantTemplateId;
		ptr += 2;
		*(short*)ptr = TargetArea;
		ptr += 2;
		*(int*)ptr = Favorability;
		ptr += 4;
		if (PathInArea != null)
		{
			byte* intPtr = ptr;
			ptr += 2;
			int num = PathInArea.Serialize(ptr);
			ptr += num;
			Tester.Assert(num <= 65535);
			*(ushort*)intPtr = (ushort)num;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		if (ExtraData != null)
		{
			byte* intPtr2 = ptr;
			ptr += 2;
			int num2 = ExtraData.Serialize(ptr);
			ptr += num2;
			Tester.Assert(num2 <= 65535);
			*(ushort*)intPtr2 = (ushort)num2;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		if (SettlementDisplayDataList != null)
		{
			int count = SettlementDisplayDataList.Count;
			Tester.Assert(count <= 65535);
			*(ushort*)ptr = (ushort)count;
			ptr += 2;
			for (int i = 0; i < count; i++)
			{
				ptr += SettlementDisplayDataList[i].Serialize(ptr);
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
		CaravanId = *(int*)ptr;
		ptr += 4;
		MerchantTemplateId = *(short*)ptr;
		ptr += 2;
		TargetArea = *(short*)ptr;
		ptr += 2;
		Favorability = *(int*)ptr;
		ptr += 4;
		ushort num = *(ushort*)ptr;
		ptr += 2;
		if (num > 0)
		{
			if (PathInArea == null)
			{
				PathInArea = new CaravanPath();
			}
			ptr += PathInArea.Deserialize(ptr);
		}
		else
		{
			PathInArea = null;
		}
		ushort num2 = *(ushort*)ptr;
		ptr += 2;
		if (num2 > 0)
		{
			if (ExtraData == null)
			{
				ExtraData = new CaravanExtraData();
			}
			ptr += ExtraData.Deserialize(ptr);
		}
		else
		{
			ExtraData = null;
		}
		ushort num3 = *(ushort*)ptr;
		ptr += 2;
		if (num3 > 0)
		{
			if (SettlementDisplayDataList == null)
			{
				SettlementDisplayDataList = new List<SettlementDisplayData>(num3);
			}
			else
			{
				SettlementDisplayDataList.Clear();
			}
			for (int i = 0; i < num3; i++)
			{
				SettlementDisplayData item = default(SettlementDisplayData);
				ptr += item.Deserialize(ptr);
				SettlementDisplayDataList.Add(item);
			}
		}
		else
		{
			SettlementDisplayDataList?.Clear();
		}
		int num4 = (int)(ptr - pData);
		if (num4 > 4)
		{
			return (num4 + 3) / 4 * 4;
		}
		return num4;
	}
}
