using System.Collections.Generic;
using Config;
using GameData.Domains.Organization.Display;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Merchant;

[SerializableGameData(NoCopyConstructors = true)]
public class MerchantInfoCaravanData : ISerializableGameData
{
	[SerializableGameDataField]
	public int CaravanId;

	[SerializableGameDataField]
	public short MerchantTemplateId;

	[SerializableGameDataField]
	public short CurrentAreaTemplateId;

	[SerializableGameDataField]
	public short TargetAreaTemplateId;

	[SerializableGameDataField]
	public short StartAreaTemplateId;

	[SerializableGameDataField]
	public List<SettlementDisplayData> RemainSettlementInfoList;

	[SerializableGameDataField]
	public int RemainNodeCount;

	[SerializableGameDataField]
	public CaravanExtraData ExtraData;

	[SerializableGameDataField]
	public bool IsInBrokenArea;

	[SerializableGameDataField]
	public CaravanPath CaravanPath;

	public bool CanInvest
	{
		get
		{
			if (!ExtraData.IsInvested)
			{
				return IsInStartArea;
			}
			return false;
		}
	}

	public bool IsInStartArea => CurrentAreaTemplateId == StartAreaTemplateId;

	public MerchantItem MerchantConfig => Config.Merchant.Instance[MerchantTemplateId];

	public int RemainSettlementCount => RemainSettlementInfoList?.Count ?? 0;

	public int GetInvestIncome()
	{
		if (!ExtraData.IsInvested)
		{
			return 0;
		}
		MerchantItem merchantItem = Config.Merchant.Instance[MerchantTemplateId];
		int num = GlobalConfig.Instance.InvestCaravanNeedMoney[merchantItem.Level];
		short incomeBonus = ExtraData.IncomeBonus;
		return num * incomeBonus / 1000;
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 17;
		num = ((RemainSettlementInfoList == null) ? (num + 2) : (num + (2 + 32 * RemainSettlementInfoList.Count)));
		num = ((ExtraData == null) ? (num + 2) : (num + (2 + ExtraData.GetSerializedSize())));
		num = ((CaravanPath == null) ? (num + 2) : (num + (2 + 2 * CaravanPath.GetSerializedSize())));
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
		*(short*)ptr = CurrentAreaTemplateId;
		ptr += 2;
		*(short*)ptr = TargetAreaTemplateId;
		ptr += 2;
		*(short*)ptr = StartAreaTemplateId;
		ptr += 2;
		if (RemainSettlementInfoList != null)
		{
			int count = RemainSettlementInfoList.Count;
			Tester.Assert(count <= 65535);
			*(ushort*)ptr = (ushort)count;
			ptr += 2;
			for (int i = 0; i < count; i++)
			{
				ptr += RemainSettlementInfoList[i].Serialize(ptr);
			}
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		*(int*)ptr = RemainNodeCount;
		ptr += 4;
		if (ExtraData != null)
		{
			byte* intPtr = ptr;
			ptr += 2;
			int num = ExtraData.Serialize(ptr);
			ptr += num;
			Tester.Assert(num <= 65535);
			*(ushort*)intPtr = (ushort)num;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		*ptr = (IsInBrokenArea ? ((byte)1) : ((byte)0));
		ptr++;
		if (CaravanPath != null)
		{
			byte* intPtr2 = ptr;
			ptr += 2;
			int num2 = CaravanPath.Serialize(ptr);
			ptr += num2;
			Tester.Assert(num2 <= 65535);
			*(ushort*)intPtr2 = (ushort)num2;
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
		CurrentAreaTemplateId = *(short*)ptr;
		ptr += 2;
		TargetAreaTemplateId = *(short*)ptr;
		ptr += 2;
		StartAreaTemplateId = *(short*)ptr;
		ptr += 2;
		ushort num = *(ushort*)ptr;
		ptr += 2;
		if (num > 0)
		{
			if (RemainSettlementInfoList == null)
			{
				RemainSettlementInfoList = new List<SettlementDisplayData>(num);
			}
			else
			{
				RemainSettlementInfoList.Clear();
			}
			for (int i = 0; i < num; i++)
			{
				SettlementDisplayData item = default(SettlementDisplayData);
				ptr += item.Deserialize(ptr);
				RemainSettlementInfoList.Add(item);
			}
		}
		else
		{
			RemainSettlementInfoList?.Clear();
		}
		RemainNodeCount = *(int*)ptr;
		ptr += 4;
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
		IsInBrokenArea = *ptr != 0;
		ptr++;
		ushort num3 = *(ushort*)ptr;
		ptr += 2;
		if (num3 > 0)
		{
			if (CaravanPath == null)
			{
				CaravanPath = new CaravanPath();
			}
			ptr += CaravanPath.Deserialize(ptr);
		}
		else
		{
			CaravanPath = null;
		}
		int num4 = (int)(ptr - pData);
		if (num4 > 4)
		{
			return (num4 + 3) / 4 * 4;
		}
		return num4;
	}
}
