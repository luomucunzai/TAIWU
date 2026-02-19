using System;
using System.Collections.Generic;
using Config.Common;

namespace Config;

[Serializable]
public class MapBlockItem : ConfigItem<MapBlockItem, short>
{
	public readonly short TemplateId;

	public readonly EMapBlockType Type;

	public readonly EMapBlockSubType SubType;

	public readonly string Name;

	public readonly string AdventureEditorName;

	public readonly string AdventureBlockImage;

	public readonly string Desc;

	public readonly byte Size;

	public readonly byte Range;

	public readonly sbyte ViewRange;

	public readonly sbyte MoveCost;

	public readonly sbyte PathFindingCost;

	public readonly bool FreeStepIgnorePathCost;

	public readonly bool ShowTips;

	public readonly string[] Image;

	public readonly string[] Effect;

	public readonly string VillagerWorkCorrectImage;

	public readonly bool IgnoreDestroyed;

	public readonly bool TaiwuEventChangedBlock;

	public readonly short SplitOrMergeBlockId;

	public readonly List<sbyte> MainResourceType;

	public readonly short[] Resources;

	public readonly sbyte ResourceCollectionType;

	public readonly short MaxMalice;

	public readonly string[] BlockNames;

	public readonly string Bgm;

	public readonly string[] Bgs;

	public readonly sbyte BuildingAreaWidth;

	public readonly sbyte LandFormType;

	public readonly short CenterBuilding;

	public readonly sbyte CenterBuildingMaxLevel;

	public readonly string FixedBuildingImage;

	public readonly List<short> PresetBuildingList;

	public readonly List<short> RandomBuildingList;

	public readonly short InformationTemplateId;

	public readonly List<(short, short)> AdventureTerrainWeights;

	public readonly string EventBack;

	public readonly short CombatScene;

	public readonly short CombatState;

	public readonly bool CanGenerate;

	public MapBlockItem(short templateId, EMapBlockType type, EMapBlockSubType subType, int name, int adventureEditorName, int adventureBlockImage, int desc, byte size, byte range, sbyte viewRange, sbyte moveCost, sbyte pathFindingCost, bool freeStepIgnorePathCost, bool showTips, string[] image, string[] effect, string villagerWorkCorrectImage, bool ignoreDestroyed, bool taiwuEventChangedBlock, short splitOrMergeBlockId, List<sbyte> mainResourceType, short[] resources, sbyte resourceCollectionType, short maxMalice, int[] blockNames, string bgm, string[] bgs, sbyte buildingAreaWidth, sbyte landFormType, short centerBuilding, sbyte centerBuildingMaxLevel, string fixedBuildingImage, List<short> presetBuildingList, List<short> randomBuildingList, short informationTemplateId, List<(short, short)> adventureTerrainWeights, string eventBack, short combatScene, short combatState, bool canGenerate)
	{
		TemplateId = templateId;
		Type = type;
		SubType = subType;
		Name = LocalStringManager.GetConfig("MapBlock_language", name);
		AdventureEditorName = LocalStringManager.GetConfig("MapBlock_language", adventureEditorName);
		AdventureBlockImage = LocalStringManager.GetConfig("MapBlock_language", adventureBlockImage);
		Desc = LocalStringManager.GetConfig("MapBlock_language", desc);
		Size = size;
		Range = range;
		ViewRange = viewRange;
		MoveCost = moveCost;
		PathFindingCost = pathFindingCost;
		FreeStepIgnorePathCost = freeStepIgnorePathCost;
		ShowTips = showTips;
		Image = image;
		Effect = effect;
		VillagerWorkCorrectImage = villagerWorkCorrectImage;
		IgnoreDestroyed = ignoreDestroyed;
		TaiwuEventChangedBlock = taiwuEventChangedBlock;
		SplitOrMergeBlockId = splitOrMergeBlockId;
		MainResourceType = mainResourceType;
		Resources = resources;
		ResourceCollectionType = resourceCollectionType;
		MaxMalice = maxMalice;
		BlockNames = LocalStringManager.ConvertConfigList("MapBlock_language", blockNames);
		Bgm = bgm;
		Bgs = bgs;
		BuildingAreaWidth = buildingAreaWidth;
		LandFormType = landFormType;
		CenterBuilding = centerBuilding;
		CenterBuildingMaxLevel = centerBuildingMaxLevel;
		FixedBuildingImage = fixedBuildingImage;
		PresetBuildingList = presetBuildingList;
		RandomBuildingList = randomBuildingList;
		InformationTemplateId = informationTemplateId;
		AdventureTerrainWeights = adventureTerrainWeights;
		EventBack = eventBack;
		CombatScene = combatScene;
		CombatState = combatState;
		CanGenerate = canGenerate;
	}

	public MapBlockItem()
	{
		TemplateId = 0;
		Type = EMapBlockType.Invalid;
		SubType = EMapBlockSubType.Invalid;
		Name = null;
		AdventureEditorName = null;
		AdventureBlockImage = null;
		Desc = null;
		Size = 0;
		Range = 0;
		ViewRange = 1;
		MoveCost = -1;
		PathFindingCost = -1;
		FreeStepIgnorePathCost = false;
		ShowTips = false;
		Image = new string[1] { "" };
		Effect = new string[1] { "" };
		VillagerWorkCorrectImage = null;
		IgnoreDestroyed = false;
		TaiwuEventChangedBlock = false;
		SplitOrMergeBlockId = 0;
		MainResourceType = new List<sbyte>();
		Resources = new short[6];
		ResourceCollectionType = 0;
		MaxMalice = -1;
		BlockNames = LocalStringManager.ConvertConfigList("MapBlock_language", null);
		Bgm = null;
		Bgs = new string[1] { "" };
		BuildingAreaWidth = -1;
		LandFormType = 0;
		CenterBuilding = 0;
		CenterBuildingMaxLevel = -1;
		FixedBuildingImage = null;
		PresetBuildingList = new List<short>();
		RandomBuildingList = new List<short>();
		InformationTemplateId = 0;
		AdventureTerrainWeights = new List<(short, short)> { (1, 100) };
		EventBack = null;
		CombatScene = 0;
		CombatState = 0;
		CanGenerate = true;
	}

	public MapBlockItem(short templateId, MapBlockItem other)
	{
		TemplateId = templateId;
		Type = other.Type;
		SubType = other.SubType;
		Name = other.Name;
		AdventureEditorName = other.AdventureEditorName;
		AdventureBlockImage = other.AdventureBlockImage;
		Desc = other.Desc;
		Size = other.Size;
		Range = other.Range;
		ViewRange = other.ViewRange;
		MoveCost = other.MoveCost;
		PathFindingCost = other.PathFindingCost;
		FreeStepIgnorePathCost = other.FreeStepIgnorePathCost;
		ShowTips = other.ShowTips;
		Image = other.Image;
		Effect = other.Effect;
		VillagerWorkCorrectImage = other.VillagerWorkCorrectImage;
		IgnoreDestroyed = other.IgnoreDestroyed;
		TaiwuEventChangedBlock = other.TaiwuEventChangedBlock;
		SplitOrMergeBlockId = other.SplitOrMergeBlockId;
		MainResourceType = other.MainResourceType;
		Resources = other.Resources;
		ResourceCollectionType = other.ResourceCollectionType;
		MaxMalice = other.MaxMalice;
		BlockNames = other.BlockNames;
		Bgm = other.Bgm;
		Bgs = other.Bgs;
		BuildingAreaWidth = other.BuildingAreaWidth;
		LandFormType = other.LandFormType;
		CenterBuilding = other.CenterBuilding;
		CenterBuildingMaxLevel = other.CenterBuildingMaxLevel;
		FixedBuildingImage = other.FixedBuildingImage;
		PresetBuildingList = other.PresetBuildingList;
		RandomBuildingList = other.RandomBuildingList;
		InformationTemplateId = other.InformationTemplateId;
		AdventureTerrainWeights = other.AdventureTerrainWeights;
		EventBack = other.EventBack;
		CombatScene = other.CombatScene;
		CombatState = other.CombatState;
		CanGenerate = other.CanGenerate;
	}

	public override int GetTemplateId()
	{
		return TemplateId;
	}

	public override MapBlockItem Duplicate(int templateId)
	{
		return new MapBlockItem((short)templateId, this);
	}
}
