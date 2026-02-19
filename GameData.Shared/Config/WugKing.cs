using System;
using System.Collections.Generic;
using Config.Common;

namespace Config;

[Serializable]
public class WugKing : ConfigData<WugKingItem, sbyte>
{
	public static WugKing Instance = new WugKing();

	private readonly HashSet<string> RequiredFields = new HashSet<string>
	{
		"GrowingBadWugs", "GrowingGoodWugs", "GrownWug", "WugFinger", "WugMedicine", "RefiningPoisons", "TemplateId", "GrowingBadEffectDesc", "GrowingGoodEffectDesc", "GrownEffectDesc",
		"MakeTip", "RefiningWeight", "PoisonMinPercent", "PoisonMaxPercent"
	};

	internal override int ToInt(sbyte value)
	{
		return value;
	}

	internal override sbyte ToTemplateId(int value)
	{
		return (sbyte)value;
	}

	private void CreateItems0()
	{
		_dataArray.Add(new WugKingItem(0, new List<short> { 349, 350 }, 0, new List<short> { 347, 348 }, 1, 351, 2, 3, 445, 424, 12, new List<sbyte> { 3 }, 40, 100, poisonUnique: true));
		_dataArray.Add(new WugKingItem(1, new List<short> { 354, 355 }, 4, new List<short> { 352, 353 }, 5, 356, 6, 7, 446, 425, 12, new List<sbyte> { 1 }, 40, 100, poisonUnique: true));
		_dataArray.Add(new WugKingItem(2, new List<short> { 359, 360 }, 8, new List<short> { 357, 358 }, 9, 361, 10, 11, 447, 426, 12, new List<sbyte> { 4 }, 40, 100, poisonUnique: true));
		_dataArray.Add(new WugKingItem(3, new List<short> { 364, 365 }, 12, new List<short> { 362, 363 }, 13, 366, 14, 15, 448, 427, 6, new List<sbyte> { 0, 1, 2, 3, 4, 5 }, 12, 20, poisonUnique: false));
		_dataArray.Add(new WugKingItem(4, new List<short> { 369, 370 }, 16, new List<short> { 367, 368 }, 17, 371, 18, 19, 449, 428, 3, new List<sbyte> { 0, 3, 4 }, 30, 100, poisonUnique: false));
		_dataArray.Add(new WugKingItem(5, new List<short> { 374, 375 }, 20, new List<short> { 372, 373 }, 21, 376, 22, 23, 450, 429, 9, new List<sbyte> { 2 }, 40, 100, poisonUnique: true));
		_dataArray.Add(new WugKingItem(6, new List<short> { 379, 380 }, 24, new List<short> { 377, 378 }, 25, 381, 26, 27, 451, 430, 9, new List<sbyte> { 0 }, 40, 100, poisonUnique: true));
		_dataArray.Add(new WugKingItem(7, new List<short> { 384, 385 }, 28, new List<short> { 382, 383 }, 29, 386, 30, 31, 452, 431, 1, new List<sbyte> { 1, 2, 5 }, 30, 100, poisonUnique: false));
	}

	public override void Init()
	{
		base.Init();
		_dataArray = new List<WugKingItem>(8);
		CreateItems0();
	}
}
