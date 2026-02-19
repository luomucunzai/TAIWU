using System.Collections.Generic;
using GameData.Domains;
using GameData.Domains.Global;
using GameData.Domains.Map;
using Redzen.Random;

namespace GameData.Common;

public class GameContext : IGameContext
{
	public IRandomSource Random => DataContextManager.GetCurrentThreadDataContext().Random;

	public string Language => GlobalDomain.Settings.Language;

	public bool NoProfessionSkillCooldown => DomainManager.Extra.NoProfessionSkillCooldown;

	public IReadOnlyDictionary<int, string> CustomTexts => DomainManager.World.GetCustomTexts();

	public string DataPath => "../The Scroll of Taiwu_Data";

	public bool HideTaiwuOriginalSurname => DomainManager.World.GetHideTaiwuOriginalSurname();

	public int TaiwuCharId => DomainManager.Taiwu.GetTaiwuCharId();

	public int CurrDate => DomainManager.World.GetCurrDate();

	public short MainStoryLineProgress => DomainManager.World.GetMainStoryLineProgress();

	public byte WorldResourceAmountType => DomainManager.World.GetWorldResourceAmountType();

	public sbyte XiangshuProgress => DomainManager.World.GetXiangshuProgress();

	public bool IsProfessionalSkillUnlockedAndEquipped(int professionSkillTemplateId)
	{
		return DomainManager.Extra.IsProfessionalSkillUnlockedAndEquipped(professionSkillTemplateId);
	}

	public bool GetWorldFunctionsStatus(byte worldFunctionType)
	{
		return DomainManager.World.GetWorldFunctionsStatus(worldFunctionType);
	}

	public byte GetAreaSize(short areaId)
	{
		return DomainManager.Map.GetAreaSize(areaId);
	}

	public MapBlockData GetBlockData(Location location)
	{
		return DomainManager.Map.GetBlock(location);
	}

	public IEnumerable<short> GetGroupBlockIds(Location rootLocation, MapBlockData rootBlock)
	{
		if (rootBlock.GroupBlockList == null)
		{
			yield break;
		}
		foreach (MapBlockData groupBlock in rootBlock.GroupBlockList)
		{
			yield return groupBlock.BlockId;
		}
	}
}
