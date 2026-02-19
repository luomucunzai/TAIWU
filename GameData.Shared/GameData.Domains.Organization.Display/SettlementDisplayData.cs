using GameData.Serializer;

namespace GameData.Domains.Organization.Display;

public struct SettlementDisplayData : ISerializableGameData
{
	[SerializableGameDataField]
	public int SettlementId;

	[SerializableGameDataField]
	public short Culture;

	[SerializableGameDataField]
	public short MaxCulture;

	[SerializableGameDataField]
	public short Safety;

	[SerializableGameDataField]
	public short MaxSafety;

	[SerializableGameDataField]
	public int Population;

	[SerializableGameDataField]
	public int MaxPopulation;

	[SerializableGameDataField]
	public short RandomNameId;

	[SerializableGameDataField]
	public sbyte OrgTemplateId;

	[SerializableGameDataField]
	public short AreaTemplateId;

	[SerializableGameDataField]
	public int InfluencePowerUpdateDate;

	public bool IsSerializedSizeFixed()
	{
		return true;
	}

	public int GetSerializedSize()
	{
		int num = 29;
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		*(int*)pData = SettlementId;
		byte* num = pData + 4;
		*(short*)num = Culture;
		byte* num2 = num + 2;
		*(short*)num2 = MaxCulture;
		byte* num3 = num2 + 2;
		*(short*)num3 = Safety;
		byte* num4 = num3 + 2;
		*(short*)num4 = MaxSafety;
		byte* num5 = num4 + 2;
		*(int*)num5 = Population;
		byte* num6 = num5 + 4;
		*(int*)num6 = MaxPopulation;
		byte* num7 = num6 + 4;
		*(short*)num7 = RandomNameId;
		byte* num8 = num7 + 2;
		*num8 = (byte)OrgTemplateId;
		byte* num9 = num8 + 1;
		*(short*)num9 = AreaTemplateId;
		byte* num10 = num9 + 2;
		*(int*)num10 = InfluencePowerUpdateDate;
		int num11 = (int)(num10 + 4 - pData);
		if (num11 > 4)
		{
			return (num11 + 3) / 4 * 4;
		}
		return num11;
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		SettlementId = *(int*)ptr;
		ptr += 4;
		Culture = *(short*)ptr;
		ptr += 2;
		MaxCulture = *(short*)ptr;
		ptr += 2;
		Safety = *(short*)ptr;
		ptr += 2;
		MaxSafety = *(short*)ptr;
		ptr += 2;
		Population = *(int*)ptr;
		ptr += 4;
		MaxPopulation = *(int*)ptr;
		ptr += 4;
		RandomNameId = *(short*)ptr;
		ptr += 2;
		OrgTemplateId = (sbyte)(*ptr);
		ptr++;
		AreaTemplateId = *(short*)ptr;
		ptr += 2;
		InfluencePowerUpdateDate = *(int*)ptr;
		ptr += 4;
		int num = (int)(ptr - pData);
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}
}
