using System.Collections.Generic;
using GameData.Domains.Map;

namespace GameData.Domains.Extra;

public class PreAdvanceMonthNpcTamingModification
{
	public List<(int, Location)> TamerRecords = new List<(int, Location)>();
}
