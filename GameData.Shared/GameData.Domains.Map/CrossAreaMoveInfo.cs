using GameData.Serializer;

namespace GameData.Domains.Map;

public class CrossAreaMoveInfo : ISerializableGameData
{
	public const int Invalid = -1;

	public short FromAreaId;

	public short FromBlockId;

	public short ToAreaId;

	public int MoneyCost;

	public int AuthorityCost;

	public int CostedDays;

	public TravelRoute Route;

	public bool Traveling => ToAreaId >= 0;

	public int RouteIndex => ParseRouteIndex();

	public short CurrentAreaId => ParseAreaId();

	public short LastAreaId => ParseLastAreaId();

	public short NextAreaId => ParseNextAreaId();

	public int NextCostDays => ParseNextCostDays();

	public CrossAreaMoveInfo()
	{
		ToAreaId = -1;
		Route = new TravelRoute();
	}

	public int ParseRouteIndex()
	{
		if (!Traveling)
		{
			return -1;
		}
		int num = CostedDays;
		int num2 = -1;
		for (int i = 0; i < Route.CostList.Count; i++)
		{
			num -= Route.CostList[i];
			if (num < 0)
			{
				break;
			}
			num2++;
		}
		return num2;
	}

	public short ParseAreaId()
	{
		if (!Traveling)
		{
			return -1;
		}
		int routeIndex = RouteIndex;
		if (routeIndex >= 0)
		{
			return Route.AreaList[routeIndex];
		}
		return FromAreaId;
	}

	public short ParseLastAreaId()
	{
		if (!Traveling)
		{
			return -1;
		}
		int num = RouteIndex - 1;
		if (num >= 0)
		{
			return Route.AreaList[num];
		}
		return FromAreaId;
	}

	public short ParseNextAreaId()
	{
		if (!Traveling)
		{
			return -1;
		}
		int num = RouteIndex + 1;
		if (num < Route.AreaList.Count)
		{
			return Route.AreaList[num];
		}
		return ToAreaId;
	}

	public int ParseNextCostDays()
	{
		if (!Traveling || CurrentAreaId == ToAreaId)
		{
			return -1;
		}
		int num = RouteIndex + 1;
		int num2 = 0;
		for (int i = 0; i <= num; i++)
		{
			num2 += Route.CostList[i];
		}
		return num2 - CostedDays;
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 18;
		num += Route.GetSerializedSize();
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		*(short*)pData = FromAreaId;
		((short*)pData)[1] = FromBlockId;
		((short*)pData)[2] = ToAreaId;
		*(int*)(pData + 6) = MoneyCost;
		*(int*)(pData + 10) = AuthorityCost;
		*(int*)(pData + 14) = CostedDays;
		Route.Serialize(pData + 18);
		return GetSerializedSize();
	}

	public unsafe int Deserialize(byte* pData)
	{
		FromAreaId = *(short*)pData;
		FromBlockId = ((short*)pData)[1];
		ToAreaId = ((short*)pData)[2];
		MoneyCost = *(int*)(pData + 6);
		AuthorityCost = *(int*)(pData + 10);
		CostedDays = *(int*)(pData + 14);
		Route.Deserialize(pData + 18);
		return GetSerializedSize();
	}
}
