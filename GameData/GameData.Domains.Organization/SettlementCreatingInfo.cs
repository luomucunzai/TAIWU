using System.Collections.Generic;

namespace GameData.Domains.Organization;

public class SettlementCreatingInfo
{
	private readonly List<short> _villageRandomNameIds;

	private short _nextVillageRandomNameIndex;

	private readonly List<short> _townRandomNameIds;

	private short _nextTownRandomNameIndex;

	private readonly List<short> _walledTownRandomNameIds;

	private short _nextWalledTownRandomNameIndex;

	public SettlementCreatingInfo(List<short> villageRandomNameIds, List<short> townRandomNameIds, List<short> walledTownRandomNameIds)
	{
		_villageRandomNameIds = villageRandomNameIds;
		_nextVillageRandomNameIndex = 0;
		_townRandomNameIds = townRandomNameIds;
		_nextTownRandomNameIndex = 0;
		_walledTownRandomNameIds = walledTownRandomNameIds;
		_nextWalledTownRandomNameIndex = 0;
	}

	public short GenerateRandomName(sbyte orgTemplateId)
	{
		switch (orgTemplateId)
		{
		case 36:
		{
			short nextVillageRandomNameIndex = _nextVillageRandomNameIndex;
			_nextVillageRandomNameIndex++;
			return _villageRandomNameIds[nextVillageRandomNameIndex];
		}
		case 37:
		{
			short nextTownRandomNameIndex = _nextTownRandomNameIndex;
			_nextTownRandomNameIndex++;
			return _townRandomNameIds[nextTownRandomNameIndex];
		}
		case 38:
		{
			short nextWalledTownRandomNameIndex = _nextWalledTownRandomNameIndex;
			_nextWalledTownRandomNameIndex++;
			return _walledTownRandomNameIds[nextWalledTownRandomNameIndex];
		}
		default:
			return -1;
		}
	}
}
