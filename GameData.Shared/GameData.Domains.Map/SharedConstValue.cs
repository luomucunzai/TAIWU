using System;
using System.Collections.Generic;

namespace GameData.Domains.Map;

public class SharedConstValue
{
	public static readonly List<List<short>> AnimalCharIdGroups = new List<List<short>>
	{
		new List<short> { 213, 222 },
		new List<short> { 214, 223 },
		new List<short> { 211, 220 },
		new List<short> { 216, 225 },
		new List<short> { 217, 226 },
		new List<short> { 210, 219 },
		new List<short> { 212, 221 },
		new List<short> { 215, 224 },
		new List<short> { 218, 227 }
	};

	[Obsolete]
	public static readonly List<short> SwordTombAdventureIdList = new List<short> { 108, 116, 110, 109, 111, 112, 113, 114, 115 };

	public static (int, int)[] TaiwuEnsuredSurroundingBlockOffsets = new(int, int)[16]
	{
		(5, 0),
		(-5, 0),
		(0, 5),
		(0, -5),
		(2, 4),
		(2, -4),
		(-2, 4),
		(-2, -4),
		(4, 2),
		(4, -2),
		(-4, 2),
		(-2, -2),
		(4, 6),
		(4, -6),
		(-4, 6),
		(-4, -6)
	};

	public static List<short> MainStoryCreatedCharTemplateIds = new List<short> { 465, 466, 464, 463 };

	public static readonly int FreeTravelCostTimeRate = 3;
}
