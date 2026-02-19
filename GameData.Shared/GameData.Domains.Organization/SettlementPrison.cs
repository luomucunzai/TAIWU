using System.Collections.Generic;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Organization;

[SerializableGameData(IsExtensible = true)]
public class SettlementPrison : ISerializableGameData
{
	private static class FieldIds
	{
		public const ushort LastBreakInDate = 0;

		public const ushort Prisoners = 1;

		public const ushort Bounties = 2;

		public const ushort Count = 3;

		public static readonly string[] FieldId2FieldName = new string[3] { "LastBreakInDate", "Prisoners", "Bounties" };
	}

	[SerializableGameDataField]
	public int LastBreakInDate;

	[SerializableGameDataField]
	public List<SettlementPrisoner> Prisoners;

	[SerializableGameDataField]
	public List<SettlementBounty> Bounties;

	public SettlementPrison()
	{
		LastBreakInDate = int.MinValue;
		Prisoners = new List<SettlementPrisoner>();
		Bounties = new List<SettlementBounty>();
	}

	public SettlementPrisoner GetPrisoner(int charId)
	{
		for (int num = Prisoners.Count - 1; num >= 0; num--)
		{
			SettlementPrisoner settlementPrisoner = Prisoners[num];
			if (settlementPrisoner.CharId == charId)
			{
				return settlementPrisoner;
			}
		}
		return null;
	}

	public SettlementBounty GetBounty(int charId)
	{
		for (int num = Bounties.Count - 1; num >= 0; num--)
		{
			SettlementBounty settlementBounty = Bounties[num];
			if (settlementBounty.CharId == charId)
			{
				return settlementBounty;
			}
		}
		return null;
	}

	public SettlementPrisoner OfflineRemovePrisoner(int charId)
	{
		for (int num = Prisoners.Count - 1; num >= 0; num--)
		{
			SettlementPrisoner settlementPrisoner = Prisoners[num];
			if (settlementPrisoner.CharId == charId)
			{
				Prisoners.RemoveAt(num);
				return settlementPrisoner;
			}
		}
		return null;
	}

	public SettlementBounty OfflineRemoveBounty(int charId)
	{
		for (int num = Bounties.Count - 1; num >= 0; num--)
		{
			SettlementBounty settlementBounty = Bounties[num];
			if (settlementBounty.CharId == charId)
			{
				Bounties.RemoveAt(num);
				return settlementBounty;
			}
		}
		return null;
	}

	public SettlementPrison(SettlementPrison other)
	{
		LastBreakInDate = other.LastBreakInDate;
		if (other.Prisoners != null)
		{
			List<SettlementPrisoner> prisoners = other.Prisoners;
			int count = prisoners.Count;
			Prisoners = new List<SettlementPrisoner>(count);
			for (int i = 0; i < count; i++)
			{
				Prisoners.Add(new SettlementPrisoner(prisoners[i]));
			}
		}
		else
		{
			Prisoners = null;
		}
		if (other.Bounties != null)
		{
			List<SettlementBounty> bounties = other.Bounties;
			int count2 = bounties.Count;
			Bounties = new List<SettlementBounty>(count2);
			for (int j = 0; j < count2; j++)
			{
				Bounties.Add(new SettlementBounty(bounties[j]));
			}
		}
		else
		{
			Bounties = null;
		}
	}

	public void Assign(SettlementPrison other)
	{
		LastBreakInDate = other.LastBreakInDate;
		if (other.Prisoners != null)
		{
			List<SettlementPrisoner> prisoners = other.Prisoners;
			int count = prisoners.Count;
			Prisoners = new List<SettlementPrisoner>(count);
			for (int i = 0; i < count; i++)
			{
				Prisoners.Add(new SettlementPrisoner(prisoners[i]));
			}
		}
		else
		{
			Prisoners = null;
		}
		if (other.Bounties != null)
		{
			List<SettlementBounty> bounties = other.Bounties;
			int count2 = bounties.Count;
			Bounties = new List<SettlementBounty>(count2);
			for (int j = 0; j < count2; j++)
			{
				Bounties.Add(new SettlementBounty(bounties[j]));
			}
		}
		else
		{
			Bounties = null;
		}
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 6;
		if (Prisoners != null)
		{
			num += 2;
			int count = Prisoners.Count;
			for (int i = 0; i < count; i++)
			{
				SettlementPrisoner settlementPrisoner = Prisoners[i];
				num = ((settlementPrisoner == null) ? (num + 2) : (num + (2 + settlementPrisoner.GetSerializedSize())));
			}
		}
		else
		{
			num += 2;
		}
		if (Bounties != null)
		{
			num += 2;
			int count2 = Bounties.Count;
			for (int j = 0; j < count2; j++)
			{
				SettlementBounty settlementBounty = Bounties[j];
				num = ((settlementBounty == null) ? (num + 2) : (num + (2 + settlementBounty.GetSerializedSize())));
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
		*(int*)ptr = LastBreakInDate;
		ptr += 4;
		if (Prisoners != null)
		{
			int count = Prisoners.Count;
			Tester.Assert(count <= 65535);
			*(ushort*)ptr = (ushort)count;
			ptr += 2;
			for (int i = 0; i < count; i++)
			{
				SettlementPrisoner settlementPrisoner = Prisoners[i];
				if (settlementPrisoner != null)
				{
					byte* intPtr = ptr;
					ptr += 2;
					int num = settlementPrisoner.Serialize(ptr);
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
		if (Bounties != null)
		{
			int count2 = Bounties.Count;
			Tester.Assert(count2 <= 65535);
			*(ushort*)ptr = (ushort)count2;
			ptr += 2;
			for (int j = 0; j < count2; j++)
			{
				SettlementBounty settlementBounty = Bounties[j];
				if (settlementBounty != null)
				{
					byte* intPtr2 = ptr;
					ptr += 2;
					int num2 = settlementBounty.Serialize(ptr);
					ptr += num2;
					Tester.Assert(num2 <= 65535);
					*(ushort*)intPtr2 = (ushort)num2;
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
			LastBreakInDate = *(int*)ptr;
			ptr += 4;
		}
		if (num > 1)
		{
			ushort num2 = *(ushort*)ptr;
			ptr += 2;
			if (num2 > 0)
			{
				if (Prisoners == null)
				{
					Prisoners = new List<SettlementPrisoner>(num2);
				}
				else
				{
					Prisoners.Clear();
				}
				for (int i = 0; i < num2; i++)
				{
					ushort num3 = *(ushort*)ptr;
					ptr += 2;
					if (num3 > 0)
					{
						SettlementPrisoner settlementPrisoner = new SettlementPrisoner();
						ptr += settlementPrisoner.Deserialize(ptr);
						Prisoners.Add(settlementPrisoner);
					}
					else
					{
						Prisoners.Add(null);
					}
				}
			}
			else
			{
				Prisoners?.Clear();
			}
		}
		if (num > 2)
		{
			ushort num4 = *(ushort*)ptr;
			ptr += 2;
			if (num4 > 0)
			{
				if (Bounties == null)
				{
					Bounties = new List<SettlementBounty>(num4);
				}
				else
				{
					Bounties.Clear();
				}
				for (int j = 0; j < num4; j++)
				{
					ushort num5 = *(ushort*)ptr;
					ptr += 2;
					if (num5 > 0)
					{
						SettlementBounty settlementBounty = new SettlementBounty();
						ptr += settlementBounty.Deserialize(ptr);
						Bounties.Add(settlementBounty);
					}
					else
					{
						Bounties.Add(null);
					}
				}
			}
			else
			{
				Bounties?.Clear();
			}
		}
		int num6 = (int)(ptr - pData);
		if (num6 > 4)
		{
			return (num6 + 3) / 4 * 4;
		}
		return num6;
	}
}
