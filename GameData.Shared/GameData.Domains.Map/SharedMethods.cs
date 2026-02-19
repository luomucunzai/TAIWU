using System.Collections.Generic;

namespace GameData.Domains.Map;

public static class SharedMethods
{
	public static void GetAreaListInState(sbyte stateId, List<short> areaList)
	{
		if (stateId >= 0)
		{
			int num = 3;
			areaList.Clear();
			for (int i = 0; i < num; i++)
			{
				areaList.Add((short)(stateId * num + i));
			}
			for (int j = 0; j < 6; j++)
			{
				areaList.Add((short)(45 + stateId * 6 + j));
			}
		}
	}

	public static void GetRegularAreaListInState(sbyte stateId, List<short> areaList)
	{
		if (stateId >= 0)
		{
			int num = 3;
			areaList.Clear();
			for (int i = 0; i < num; i++)
			{
				areaList.Add((short)(stateId * num + i));
			}
		}
	}

	public static void GetBrokenAreaListInState(sbyte stateId, List<short> areaList)
	{
		if (stateId >= 0)
		{
			areaList.Clear();
			for (int i = 0; i < 6; i++)
			{
				areaList.Add((short)(45 + stateId * 6 + i));
			}
		}
	}
}
