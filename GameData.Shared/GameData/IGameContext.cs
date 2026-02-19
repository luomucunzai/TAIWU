using System.Collections.Generic;
using GameData.Domains.Map;
using Redzen.Random;

namespace GameData;

public interface IGameContext
{
	IRandomSource Random { get; }

	string Language { get; }

	string DataPath { get; }

	bool HideTaiwuOriginalSurname { get; }

	int TaiwuCharId { get; }

	int CurrDate { get; }

	short MainStoryLineProgress { get; }

	byte WorldResourceAmountType { get; }

	sbyte XiangshuProgress { get; }

	bool NoProfessionSkillCooldown { get; }

	IReadOnlyDictionary<int, string> CustomTexts { get; }

	bool IsProfessionalSkillUnlockedAndEquipped(int professionSkillTemplateId);

	bool GetWorldFunctionsStatus(byte worldFunctionType);

	byte GetAreaSize(short areaId);

	MapBlockData GetBlockData(Location location);

	IEnumerable<short> GetGroupBlockIds(Location rootLocation, MapBlockData rootBlock);
}
