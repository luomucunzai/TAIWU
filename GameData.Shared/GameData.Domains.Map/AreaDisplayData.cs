using System.Collections.Generic;
using Config;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Map;

[SerializableGameData(NotForArchive = true)]
public struct AreaDisplayData : ISerializableGameData
{
	[SerializableGameDataField]
	public bool IsBroken;

	[SerializableGameDataField]
	public bool AnyPurpleBamboo;

	[SerializableGameDataField]
	public bool AnyFleeBeast;

	[SerializableGameDataField]
	public bool AnyFleeLoongson;

	[SerializableGameDataField]
	public int InfectedCount;

	[SerializableGameDataField]
	public int LegendaryCount;

	[SerializableGameDataField]
	public int BrokenLevel;

	[SerializableGameDataField]
	public List<short> PurpleBambooTemplateIds;

	[SerializableGameDataField]
	public List<short> AdventureTemplates;

	[SerializableGameDataField]
	public int PastLifeRelationCount;

	[SerializableGameDataField]
	public byte _loongStatusInternal;

	[SerializableGameDataField]
	public bool HasSectZhujianSpecialMerchant;

	public bool AnyLoong
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			BoolArray8 loongStatus = LoongStatus;
			return ((BoolArray8)(ref loongStatus)).Any();
		}
	}

	public BoolArray8 LoongStatus => BoolArray8.op_Implicit(_loongStatusInternal);

	public int AdventureCount => GetAdventureCount();

	public int GetAdventureCount()
	{
		if (AdventureTemplates == null)
		{
			return 0;
		}
		int num = 0;
		foreach (short adventureTemplate in AdventureTemplates)
		{
			AdventureItem adventureItem = Config.Adventure.Instance[adventureTemplate];
			if (!string.IsNullOrEmpty(AdventureType.Instance[adventureItem.Type].DisplayName))
			{
				num++;
			}
		}
		return num;
	}

	public AreaDisplayData(AreaDisplayData other)
	{
		IsBroken = other.IsBroken;
		AnyPurpleBamboo = other.AnyPurpleBamboo;
		AnyFleeBeast = other.AnyFleeBeast;
		AnyFleeLoongson = other.AnyFleeLoongson;
		InfectedCount = other.InfectedCount;
		LegendaryCount = other.LegendaryCount;
		BrokenLevel = other.BrokenLevel;
		PurpleBambooTemplateIds = ((other.PurpleBambooTemplateIds == null) ? null : new List<short>(other.PurpleBambooTemplateIds));
		AdventureTemplates = ((other.AdventureTemplates == null) ? null : new List<short>(other.AdventureTemplates));
		PastLifeRelationCount = other.PastLifeRelationCount;
		_loongStatusInternal = other._loongStatusInternal;
		HasSectZhujianSpecialMerchant = other.HasSectZhujianSpecialMerchant;
	}

	public void Assign(AreaDisplayData other)
	{
		IsBroken = other.IsBroken;
		AnyPurpleBamboo = other.AnyPurpleBamboo;
		AnyFleeBeast = other.AnyFleeBeast;
		AnyFleeLoongson = other.AnyFleeLoongson;
		InfectedCount = other.InfectedCount;
		LegendaryCount = other.LegendaryCount;
		BrokenLevel = other.BrokenLevel;
		PurpleBambooTemplateIds = ((other.PurpleBambooTemplateIds == null) ? null : new List<short>(other.PurpleBambooTemplateIds));
		AdventureTemplates = ((other.AdventureTemplates == null) ? null : new List<short>(other.AdventureTemplates));
		PastLifeRelationCount = other.PastLifeRelationCount;
		_loongStatusInternal = other._loongStatusInternal;
		HasSectZhujianSpecialMerchant = other.HasSectZhujianSpecialMerchant;
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 22;
		num = ((PurpleBambooTemplateIds == null) ? (num + 2) : (num + (2 + 2 * PurpleBambooTemplateIds.Count)));
		num = ((AdventureTemplates == null) ? (num + 2) : (num + (2 + 2 * AdventureTemplates.Count)));
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		*ptr = (IsBroken ? ((byte)1) : ((byte)0));
		ptr++;
		*ptr = (AnyPurpleBamboo ? ((byte)1) : ((byte)0));
		ptr++;
		*ptr = (AnyFleeBeast ? ((byte)1) : ((byte)0));
		ptr++;
		*ptr = (AnyFleeLoongson ? ((byte)1) : ((byte)0));
		ptr++;
		*(int*)ptr = InfectedCount;
		ptr += 4;
		*(int*)ptr = LegendaryCount;
		ptr += 4;
		*(int*)ptr = BrokenLevel;
		ptr += 4;
		if (PurpleBambooTemplateIds != null)
		{
			int count = PurpleBambooTemplateIds.Count;
			Tester.Assert(count <= 65535);
			*(ushort*)ptr = (ushort)count;
			ptr += 2;
			for (int i = 0; i < count; i++)
			{
				((short*)ptr)[i] = PurpleBambooTemplateIds[i];
			}
			ptr += 2 * count;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		if (AdventureTemplates != null)
		{
			int count2 = AdventureTemplates.Count;
			Tester.Assert(count2 <= 65535);
			*(ushort*)ptr = (ushort)count2;
			ptr += 2;
			for (int j = 0; j < count2; j++)
			{
				((short*)ptr)[j] = AdventureTemplates[j];
			}
			ptr += 2 * count2;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		*(int*)ptr = PastLifeRelationCount;
		ptr += 4;
		*ptr = _loongStatusInternal;
		ptr++;
		*ptr = (HasSectZhujianSpecialMerchant ? ((byte)1) : ((byte)0));
		ptr++;
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
		IsBroken = *ptr != 0;
		ptr++;
		AnyPurpleBamboo = *ptr != 0;
		ptr++;
		AnyFleeBeast = *ptr != 0;
		ptr++;
		AnyFleeLoongson = *ptr != 0;
		ptr++;
		InfectedCount = *(int*)ptr;
		ptr += 4;
		LegendaryCount = *(int*)ptr;
		ptr += 4;
		BrokenLevel = *(int*)ptr;
		ptr += 4;
		ushort num = *(ushort*)ptr;
		ptr += 2;
		if (num > 0)
		{
			if (PurpleBambooTemplateIds == null)
			{
				PurpleBambooTemplateIds = new List<short>(num);
			}
			else
			{
				PurpleBambooTemplateIds.Clear();
			}
			for (int i = 0; i < num; i++)
			{
				PurpleBambooTemplateIds.Add(((short*)ptr)[i]);
			}
			ptr += 2 * num;
		}
		else
		{
			PurpleBambooTemplateIds?.Clear();
		}
		ushort num2 = *(ushort*)ptr;
		ptr += 2;
		if (num2 > 0)
		{
			if (AdventureTemplates == null)
			{
				AdventureTemplates = new List<short>(num2);
			}
			else
			{
				AdventureTemplates.Clear();
			}
			for (int j = 0; j < num2; j++)
			{
				AdventureTemplates.Add(((short*)ptr)[j]);
			}
			ptr += 2 * num2;
		}
		else
		{
			AdventureTemplates?.Clear();
		}
		PastLifeRelationCount = *(int*)ptr;
		ptr += 4;
		_loongStatusInternal = *ptr;
		ptr++;
		HasSectZhujianSpecialMerchant = *ptr != 0;
		ptr++;
		int num3 = (int)(ptr - pData);
		if (num3 > 4)
		{
			return (num3 + 3) / 4 * 4;
		}
		return num3;
	}
}
