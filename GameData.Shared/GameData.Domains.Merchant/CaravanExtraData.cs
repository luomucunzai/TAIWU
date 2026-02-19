using System;
using System.Collections.Generic;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Merchant;

[SerializableGameData(IsExtensible = true)]
public class CaravanExtraData : ISerializableGameData
{
	private static class FieldIds
	{
		public const ushort IncomeCriticalRate = 0;

		public const ushort IncomeBonus = 1;

		public const ushort IncomeCriticalResult = 2;

		public const ushort RobbedRate = 3;

		public const ushort State = 4;

		public const ushort IsInvested = 5;

		public const ushort SettlementIdList = 6;

		public const ushort Count = 7;

		public static readonly string[] FieldId2FieldName = new string[7] { "IncomeCriticalRate", "IncomeBonus", "IncomeCriticalResult", "RobbedRate", "State", "IsInvested", "SettlementIdList" };
	}

	public const short InitialIncomeBonus = 1000;

	[SerializableGameDataField]
	public short IncomeCriticalRate;

	[SerializableGameDataField]
	public short IncomeBonus = 1000;

	[SerializableGameDataField]
	public short IncomeCriticalResult;

	[SerializableGameDataField]
	public short RobbedRate;

	[SerializableGameDataField]
	public sbyte State;

	[SerializableGameDataField]
	public bool IsInvested;

	[SerializableGameDataField]
	public List<short> SettlementIdList;

	public CaravanState StateEnum => (CaravanState)State;

	public override string ToString()
	{
		string text = StateEnum switch
		{
			CaravanState.Normal => "正常", 
			CaravanState.Robbed => "正在被抢", 
			CaravanState.RobEnd => "被抢结束", 
			_ => throw new ArgumentOutOfRangeException(), 
		};
		string text2 = (IsInvested ? "已经投资" : "未投资");
		return $"收益比例{IncomeBonus}‰，暴击概率{IncomeCriticalRate}‰，暴击倍率{IncomeCriticalResult}‰，遇劫概率{RobbedRate}‰，{text2}，当前状态{text}";
	}

	public CaravanExtraData()
	{
	}

	public CaravanExtraData(CaravanExtraData other)
	{
		IncomeCriticalRate = other.IncomeCriticalRate;
		IncomeBonus = other.IncomeBonus;
		IncomeCriticalResult = other.IncomeCriticalResult;
		RobbedRate = other.RobbedRate;
		State = other.State;
		IsInvested = other.IsInvested;
		SettlementIdList = ((other.SettlementIdList == null) ? null : new List<short>(other.SettlementIdList));
	}

	public void Assign(CaravanExtraData other)
	{
		IncomeCriticalRate = other.IncomeCriticalRate;
		IncomeBonus = other.IncomeBonus;
		IncomeCriticalResult = other.IncomeCriticalResult;
		RobbedRate = other.RobbedRate;
		State = other.State;
		IsInvested = other.IsInvested;
		SettlementIdList = ((other.SettlementIdList == null) ? null : new List<short>(other.SettlementIdList));
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 12;
		num = ((SettlementIdList == null) ? (num + 2) : (num + (2 + 2 * SettlementIdList.Count)));
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		*(short*)ptr = 7;
		ptr += 2;
		*(short*)ptr = IncomeCriticalRate;
		ptr += 2;
		*(short*)ptr = IncomeBonus;
		ptr += 2;
		*(short*)ptr = IncomeCriticalResult;
		ptr += 2;
		*(short*)ptr = RobbedRate;
		ptr += 2;
		*ptr = (byte)State;
		ptr++;
		*ptr = (IsInvested ? ((byte)1) : ((byte)0));
		ptr++;
		if (SettlementIdList != null)
		{
			int count = SettlementIdList.Count;
			Tester.Assert(count <= 65535);
			*(ushort*)ptr = (ushort)count;
			ptr += 2;
			for (int i = 0; i < count; i++)
			{
				((short*)ptr)[i] = SettlementIdList[i];
			}
			ptr += 2 * count;
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
			IncomeCriticalRate = *(short*)ptr;
			ptr += 2;
		}
		if (num > 1)
		{
			IncomeBonus = *(short*)ptr;
			ptr += 2;
		}
		if (num > 2)
		{
			IncomeCriticalResult = *(short*)ptr;
			ptr += 2;
		}
		if (num > 3)
		{
			RobbedRate = *(short*)ptr;
			ptr += 2;
		}
		if (num > 4)
		{
			State = (sbyte)(*ptr);
			ptr++;
		}
		if (num > 5)
		{
			IsInvested = *ptr != 0;
			ptr++;
		}
		if (num > 6)
		{
			ushort num2 = *(ushort*)ptr;
			ptr += 2;
			if (num2 > 0)
			{
				if (SettlementIdList == null)
				{
					SettlementIdList = new List<short>(num2);
				}
				else
				{
					SettlementIdList.Clear();
				}
				for (int i = 0; i < num2; i++)
				{
					SettlementIdList.Add(((short*)ptr)[i]);
				}
				ptr += 2 * num2;
			}
			else
			{
				SettlementIdList?.Clear();
			}
		}
		int num3 = (int)(ptr - pData);
		if (num3 > 4)
		{
			return (num3 + 3) / 4 * 4;
		}
		return num3;
	}
}
