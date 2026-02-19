using GameData.Serializer;

namespace GameData.Domains.Character.Display;

[SerializableGameData(NotRestrictCollectionSerializedSize = true)]
public class GraveDisplayData : ISerializableGameData
{
	[SerializableGameDataField]
	public int Id;

	[SerializableGameDataField]
	public short TemplateId;

	[SerializableGameDataField]
	public NameRelatedData NameData;

	[SerializableGameDataField]
	public short OrgSettlementId;

	[SerializableGameDataField]
	public bool Principal;

	[SerializableGameDataField]
	public sbyte Level;

	[SerializableGameDataField]
	public short Durability;

	[SerializableGameDataField]
	public short FavorabilityToTaiwu;

	public GraveDisplayData()
	{
	}

	public GraveDisplayData(GraveDisplayData other)
	{
		Id = other.Id;
		TemplateId = other.TemplateId;
		NameData = other.NameData;
		OrgSettlementId = other.OrgSettlementId;
		Principal = other.Principal;
		Level = other.Level;
		Durability = other.Durability;
		FavorabilityToTaiwu = other.FavorabilityToTaiwu;
	}

	public void Assign(GraveDisplayData other)
	{
		Id = other.Id;
		TemplateId = other.TemplateId;
		NameData = other.NameData;
		OrgSettlementId = other.OrgSettlementId;
		Principal = other.Principal;
		Level = other.Level;
		Durability = other.Durability;
		FavorabilityToTaiwu = other.FavorabilityToTaiwu;
	}

	public bool IsSerializedSizeFixed()
	{
		return true;
	}

	public int GetSerializedSize()
	{
		int num = 46;
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		*(int*)ptr = Id;
		ptr += 4;
		*(short*)ptr = TemplateId;
		ptr += 2;
		ptr += NameData.Serialize(ptr);
		*(short*)ptr = OrgSettlementId;
		ptr += 2;
		*ptr = (Principal ? ((byte)1) : ((byte)0));
		ptr++;
		*ptr = (byte)Level;
		ptr++;
		*(short*)ptr = Durability;
		ptr += 2;
		*(short*)ptr = FavorabilityToTaiwu;
		ptr += 2;
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
		Id = *(int*)ptr;
		ptr += 4;
		TemplateId = *(short*)ptr;
		ptr += 2;
		ptr += NameData.Deserialize(ptr);
		OrgSettlementId = *(short*)ptr;
		ptr += 2;
		Principal = *ptr != 0;
		ptr++;
		Level = (sbyte)(*ptr);
		ptr++;
		Durability = *(short*)ptr;
		ptr += 2;
		FavorabilityToTaiwu = *(short*)ptr;
		ptr += 2;
		int num = (int)(ptr - pData);
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}
}
