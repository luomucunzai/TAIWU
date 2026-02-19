using System;
using System.Collections.Generic;
using Config;
using GameData.Domains.Character;
using GameData.Domains.World;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Building;

public class BuildingBlockData : ISerializableGameData
{
	private const int SupportingBlockResourceDivider = 5;

	[SerializableGameDataField]
	public short BlockIndex;

	[SerializableGameDataField]
	public short TemplateId;

	[SerializableGameDataField]
	public sbyte Level;

	[SerializableGameDataField]
	public short RootBlockIndex;

	[SerializableGameDataField]
	public sbyte Durability;

	[SerializableGameDataField]
	[Obsolete]
	public bool Maintenance;

	[SerializableGameDataField]
	public sbyte OperationType;

	[SerializableGameDataField]
	public short OperationProgress;

	[SerializableGameDataField]
	public bool OperationStopping;

	[SerializableGameDataField]
	public short ShopProgress { get; private set; }

	public BuildingBlockItem ConfigData => BuildingBlock.Instance[TemplateId];

	public BuildingBlockData(short blockIndex, short templateId, sbyte level, short rootBlockIndex = -1)
	{
		BlockIndex = blockIndex;
		TemplateId = templateId;
		Level = level;
		Durability = (sbyte)((templateId >= 0) ? BuildingBlock.Instance[templateId].MaxDurability : (-1));
		Maintenance = true;
		RootBlockIndex = rootBlockIndex;
		OperationType = -1;
	}

	public void ResetData(short templateId, sbyte level = 1, short rootBlockIndex = -1)
	{
		TemplateId = templateId;
		Level = level;
		RootBlockIndex = rootBlockIndex;
		Durability = (sbyte)((templateId >= 0) ? BuildingBlock.Instance[templateId].MaxDurability : (-1));
		Maintenance = true;
		OperationType = -1;
		OperationProgress = 0;
		OperationStopping = false;
	}

	public void OfflineResetShopProgress()
	{
		ShopProgress = 0;
	}

	public void OfflineChangeShopProgress(int delta)
	{
		BuildingBlockItem configData = ConfigData;
		ShopProgress = (short)MathUtils.Clamp(ShopProgress + delta, 0, (configData.MaxProduceValue > 0) ? configData.MaxProduceValue : short.MaxValue);
	}

	public bool CanUse()
	{
		return OperationType != 0;
	}

	public bool NeedMaintenanceCost()
	{
		return OperationType != 0;
	}

	public int ResourceYieldLevelOriginal()
	{
		return 10 + Level;
	}

	public void CalcInfluences(IEnumerable<BuildingBlockData> neighborBlockDataList, List<int> neighborDistanceList, Action<BuildingBlockData, int> onInfluenceFound)
	{
		int num = 0;
		foreach (BuildingBlockData neighborBlockData in neighborBlockDataList)
		{
			if (CanInfluenceBuildingBlock(neighborBlockData))
			{
				onInfluenceFound(neighborBlockData, neighborDistanceList[num]);
			}
			num++;
		}
	}

	public bool CanInfluenceBuildingBlock(BuildingBlockData other)
	{
		BuildingBlockItem configData = other.ConfigData;
		if (configData != null && configData.DependBuildings != null)
		{
			return configData.DependBuildings.Contains(TemplateId);
		}
		return false;
	}

	public bool CanDependOnBuildingBlock(BuildingBlockData other)
	{
		return other.CanInfluenceBuildingBlock(this);
	}

	public static bool IsBuilding(EBuildingBlockType type)
	{
		if (type != EBuildingBlockType.Building)
		{
			return type == EBuildingBlockType.MainBuilding;
		}
		return true;
	}

	public static bool CanUpgrade(EBuildingBlockType type)
	{
		if (!IsBuilding(type))
		{
			return type == EBuildingBlockType.NormalResource;
		}
		return true;
	}

	public static bool IsResource(EBuildingBlockType type)
	{
		if (type != EBuildingBlockType.NormalResource && type != EBuildingBlockType.SpecialResource)
		{
			return type == EBuildingBlockType.UselessResource;
		}
		return true;
	}

	public static bool IsUsefulResource(EBuildingBlockType type)
	{
		if (type != EBuildingBlockType.NormalResource)
		{
			return type == EBuildingBlockType.SpecialResource;
		}
		return true;
	}

	[Obsolete]
	public static bool IsLegacyResource(EBuildingBlockType type)
	{
		return type == EBuildingBlockType.SpecialResource;
	}

	public static ResourceInts GetSupportingBlockResource(BuildingBlockData blockData, int level, int safety, int safetyMax)
	{
		BuildingBlockItem buildingBlockItem = BuildingBlock.Instance[blockData.TemplateId];
		ResourceInts result = default(ResourceInts);
		sbyte level2 = blockData.Level;
		int num = Math.Min(level, level2);
		for (int i = 0; i < buildingBlockItem.CollectResourcePercent.Length; i++)
		{
			if (buildingBlockItem.CollectResourcePercent[i] > 0)
			{
				sbyte b = buildingBlockItem.CollectResourcePercent[i];
				int num2 = 400 + num * 20;
				int num3 = 50 + safety * 50 / safetyMax;
				int num4 = level2 / 2 + 10;
				int num5 = 50 + 50 * b;
				int num6 = num4 * num5 / 100 * num2 / 100 * num3 / 100;
				result.Add((sbyte)Math.Min(i, 5), num6 * GameData.Domains.World.SharedMethods.GetGainResourcePercent(3) / 100);
			}
		}
		return result;
	}

	public BuildingBlockData Clone()
	{
		return new BuildingBlockData(this);
	}

	public bool Equals(BuildingBlockData other)
	{
		if (BlockIndex == other.BlockIndex && TemplateId == other.TemplateId && Level == other.Level && RootBlockIndex == other.RootBlockIndex && Durability == other.Durability && OperationType == other.OperationType && OperationProgress == other.OperationProgress && OperationStopping == other.OperationStopping)
		{
			return ShopProgress == other.ShopProgress;
		}
		return false;
	}

	public BuildingBlockData()
	{
	}

	public BuildingBlockData(BuildingBlockData other)
	{
		BlockIndex = other.BlockIndex;
		TemplateId = other.TemplateId;
		Level = other.Level;
		RootBlockIndex = other.RootBlockIndex;
		Durability = other.Durability;
		Maintenance = other.Maintenance;
		OperationType = other.OperationType;
		OperationProgress = other.OperationProgress;
		OperationStopping = other.OperationStopping;
		ShopProgress = other.ShopProgress;
	}

	public void Assign(BuildingBlockData other)
	{
		BlockIndex = other.BlockIndex;
		TemplateId = other.TemplateId;
		Level = other.Level;
		RootBlockIndex = other.RootBlockIndex;
		Durability = other.Durability;
		Maintenance = other.Maintenance;
		OperationType = other.OperationType;
		OperationProgress = other.OperationProgress;
		OperationStopping = other.OperationStopping;
		ShopProgress = other.ShopProgress;
	}

	public bool IsSerializedSizeFixed()
	{
		return true;
	}

	public int GetSerializedSize()
	{
		int num = 15;
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		*(short*)pData = BlockIndex;
		byte* num = pData + 2;
		*(short*)num = TemplateId;
		byte* num2 = num + 2;
		*num2 = (byte)Level;
		byte* num3 = num2 + 1;
		*(short*)num3 = RootBlockIndex;
		byte* num4 = num3 + 2;
		*num4 = (byte)Durability;
		byte* num5 = num4 + 1;
		*num5 = (Maintenance ? ((byte)1) : ((byte)0));
		byte* num6 = num5 + 1;
		*num6 = (byte)OperationType;
		byte* num7 = num6 + 1;
		*(short*)num7 = OperationProgress;
		byte* num8 = num7 + 2;
		*num8 = (OperationStopping ? ((byte)1) : ((byte)0));
		byte* num9 = num8 + 1;
		*(short*)num9 = ShopProgress;
		int num10 = (int)(num9 + 2 - pData);
		if (num10 > 4)
		{
			return (num10 + 3) / 4 * 4;
		}
		return num10;
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		BlockIndex = *(short*)ptr;
		ptr += 2;
		TemplateId = *(short*)ptr;
		ptr += 2;
		Level = (sbyte)(*ptr);
		ptr++;
		RootBlockIndex = *(short*)ptr;
		ptr += 2;
		Durability = (sbyte)(*ptr);
		ptr++;
		Maintenance = *ptr != 0;
		ptr++;
		OperationType = (sbyte)(*ptr);
		ptr++;
		OperationProgress = *(short*)ptr;
		ptr += 2;
		OperationStopping = *ptr != 0;
		ptr++;
		ShopProgress = *(short*)ptr;
		ptr += 2;
		int num = (int)(ptr - pData);
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}
}
