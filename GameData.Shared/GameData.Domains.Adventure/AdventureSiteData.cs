using System;
using Config;
using GameData.Domains.TaiwuEvent.MonthlyEventActions;
using GameData.Serializer;

namespace GameData.Domains.Adventure;

[Serializable]
public class AdventureSiteData : ISerializableGameData
{
	[SerializableGameDataField]
	public short TemplateId;

	[SerializableGameDataField]
	public short RemainingMonths;

	[SerializableGameDataField]
	public int SiteInitData;

	[SerializableGameDataField]
	public sbyte SiteState;

	[SerializableGameDataField]
	public MonthlyActionKey MonthlyActionKey = MonthlyActionKey.Invalid;

	public AdventureItem GetConfig()
	{
		return Config.Adventure.Instance[TemplateId];
	}

	public bool IsEnemyNest()
	{
		AdventureItem config = GetConfig();
		if (config.Type != 4)
		{
			return config.Type == 5;
		}
		return true;
	}

	public bool IsMaterialResource()
	{
		AdventureItem config = GetConfig();
		sbyte b = 9;
		if (config.Type >= b)
		{
			return config.Type < b + 6;
		}
		return false;
	}

	public AdventureSiteData(short templateId, short remainingMonths, MonthlyActionKey monthlyActionKey)
	{
		TemplateId = templateId;
		RemainingMonths = remainingMonths;
		SiteInitData = int.MinValue;
		SiteState = ((!monthlyActionKey.IsValid()) ? ((sbyte)1) : ((sbyte)0));
		MonthlyActionKey = monthlyActionKey;
	}

	public void OnDestroy()
	{
	}

	public AdventureSiteData()
	{
	}

	public AdventureSiteData(AdventureSiteData other)
	{
		TemplateId = other.TemplateId;
		RemainingMonths = other.RemainingMonths;
		SiteInitData = other.SiteInitData;
		SiteState = other.SiteState;
		MonthlyActionKey = other.MonthlyActionKey;
	}

	public void Assign(AdventureSiteData other)
	{
		TemplateId = other.TemplateId;
		RemainingMonths = other.RemainingMonths;
		SiteInitData = other.SiteInitData;
		SiteState = other.SiteState;
		MonthlyActionKey = other.MonthlyActionKey;
	}

	public bool IsSerializedSizeFixed()
	{
		return true;
	}

	public int GetSerializedSize()
	{
		int num = 12;
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		*(short*)ptr = TemplateId;
		ptr += 2;
		*(short*)ptr = RemainingMonths;
		ptr += 2;
		*(int*)ptr = SiteInitData;
		ptr += 4;
		*ptr = (byte)SiteState;
		ptr++;
		ptr += MonthlyActionKey.Serialize(ptr);
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
		TemplateId = *(short*)ptr;
		ptr += 2;
		RemainingMonths = *(short*)ptr;
		ptr += 2;
		SiteInitData = *(int*)ptr;
		ptr += 4;
		SiteState = (sbyte)(*ptr);
		ptr++;
		ptr += MonthlyActionKey.Deserialize(ptr);
		int num = (int)(ptr - pData);
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}
}
