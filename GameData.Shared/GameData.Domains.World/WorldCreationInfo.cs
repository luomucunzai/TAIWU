using System;
using System.Collections.Generic;
using Config;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.World;

[Serializable]
public struct WorldCreationInfo : ISerializableGameData
{
	[SerializableGameDataField]
	public byte WorldPopulationType;

	[SerializableGameDataField]
	public byte CharacterLifespanType;

	[SerializableGameDataField]
	public byte CombatDifficulty;

	[SerializableGameDataField]
	public byte ReadingDifficulty;

	[SerializableGameDataField]
	public byte BreakoutDifficulty;

	[SerializableGameDataField]
	public byte LoopingDifficulty;

	[SerializableGameDataField]
	public byte EnemyPracticeLevel;

	[SerializableGameDataField]
	public byte FavorabilityChange;

	[SerializableGameDataField]
	public byte HereticsAmountType;

	[SerializableGameDataField]
	public byte BossInvasionSpeedType;

	[SerializableGameDataField]
	public byte WorldResourceAmountType;

	[SerializableGameDataField]
	public byte ProfessionUpgrade;

	[SerializableGameDataField]
	public short LootYield;

	[SerializableGameDataField]
	public bool AllowRandomTaiwuHeir;

	[SerializableGameDataField]
	public bool RestrictOptionsBehaviorType;

	[SerializableGameDataField]
	public sbyte TaiwuVillageStateTemplateId;

	[SerializableGameDataField]
	public sbyte TaiwuVillageLandFormType;

	public int Get(byte templateId)
	{
		return templateId switch
		{
			0 => CharacterLifespanType, 
			1 => CombatDifficulty, 
			8 => ReadingDifficulty, 
			9 => BreakoutDifficulty, 
			10 => LoopingDifficulty, 
			2 => HereticsAmountType, 
			3 => BossInvasionSpeedType, 
			4 => WorldResourceAmountType, 
			5 => WorldPopulationType, 
			6 => (!RestrictOptionsBehaviorType) ? 1 : 0, 
			7 => (!AllowRandomTaiwuHeir) ? 1 : 0, 
			11 => EnemyPracticeLevel, 
			12 => FavorabilityChange, 
			13 => ProfessionUpgrade, 
			14 => LootYield, 
			_ => throw new ArgumentOutOfRangeException("templateId", templateId, null), 
		};
	}

	public void Set(byte templateId, byte value)
	{
		switch (templateId)
		{
		case 0:
			CharacterLifespanType = value;
			break;
		case 1:
			CombatDifficulty = value;
			break;
		case 8:
			ReadingDifficulty = value;
			break;
		case 9:
			BreakoutDifficulty = value;
			break;
		case 10:
			LoopingDifficulty = value;
			break;
		case 2:
			HereticsAmountType = value;
			break;
		case 3:
			BossInvasionSpeedType = value;
			break;
		case 4:
			WorldResourceAmountType = value;
			break;
		case 5:
			WorldPopulationType = value;
			break;
		case 6:
			RestrictOptionsBehaviorType = value == 0;
			break;
		case 7:
			AllowRandomTaiwuHeir = value == 0;
			break;
		case 11:
			EnemyPracticeLevel = value;
			break;
		case 12:
			FavorabilityChange = value;
			break;
		case 13:
			ProfessionUpgrade = value;
			break;
		case 14:
			LootYield = value;
			break;
		default:
			throw new ArgumentOutOfRangeException("templateId", templateId, null);
		}
	}

	public static WorldCreationInfo CreateByDifficultyPreset(sbyte difficulty)
	{
		WorldCreationInfo result = default(WorldCreationInfo);
		foreach (WorldCreationItem item in (IEnumerable<WorldCreationItem>)WorldCreation.Instance)
		{
			result.Set(item.TemplateId, (byte)item.DifficultyPreset[difficulty]);
		}
		return result;
	}

	public int GetGroupLevel(sbyte groupId)
	{
		int groupLegacyBonusSum = GetGroupLegacyBonusSum(groupId);
		for (sbyte b = (sbyte)(GlobalConfig.Instance.LegacyGroupLevelThresholds.Length - 1); b >= 0; b--)
		{
			if (groupLegacyBonusSum >= GlobalConfig.Instance.LegacyGroupLevelThresholds[b])
			{
				return b;
			}
		}
		throw new Exception($"Invalid legacy bonus sum {groupLegacyBonusSum} for group {groupId}.");
	}

	public int GetGroupLegacyBonusSum(sbyte groupId)
	{
		WorldCreationGroupItem worldCreationGroupItem = WorldCreationGroup.Instance[groupId];
		int num = 0;
		byte[] worldCreations = worldCreationGroupItem.WorldCreations;
		foreach (byte b in worldCreations)
		{
			WorldCreationItem worldCreationItem = WorldCreation.Instance[b];
			int num2 = Get(b);
			if (worldCreationItem.LegacyPointBonus.CheckIndex(num2))
			{
				num += worldCreationItem.LegacyPointBonus[num2];
			}
		}
		return num;
	}

	public bool IsSerializedSizeFixed()
	{
		return true;
	}

	public int GetSerializedSize()
	{
		int num = 18;
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		*pData = WorldPopulationType;
		byte* num = pData + 1;
		*num = CharacterLifespanType;
		byte* num2 = num + 1;
		*num2 = CombatDifficulty;
		byte* num3 = num2 + 1;
		*num3 = ReadingDifficulty;
		byte* num4 = num3 + 1;
		*num4 = BreakoutDifficulty;
		byte* num5 = num4 + 1;
		*num5 = LoopingDifficulty;
		byte* num6 = num5 + 1;
		*num6 = EnemyPracticeLevel;
		byte* num7 = num6 + 1;
		*num7 = FavorabilityChange;
		byte* num8 = num7 + 1;
		*num8 = HereticsAmountType;
		byte* num9 = num8 + 1;
		*num9 = BossInvasionSpeedType;
		byte* num10 = num9 + 1;
		*num10 = WorldResourceAmountType;
		byte* num11 = num10 + 1;
		*num11 = ProfessionUpgrade;
		byte* num12 = num11 + 1;
		*(short*)num12 = LootYield;
		byte* num13 = num12 + 2;
		*num13 = (AllowRandomTaiwuHeir ? ((byte)1) : ((byte)0));
		byte* num14 = num13 + 1;
		*num14 = (RestrictOptionsBehaviorType ? ((byte)1) : ((byte)0));
		byte* num15 = num14 + 1;
		*num15 = (byte)TaiwuVillageStateTemplateId;
		byte* num16 = num15 + 1;
		*num16 = (byte)TaiwuVillageLandFormType;
		int num17 = (int)(num16 + 1 - pData);
		if (num17 > 4)
		{
			return (num17 + 3) / 4 * 4;
		}
		return num17;
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		WorldPopulationType = *ptr;
		ptr++;
		CharacterLifespanType = *ptr;
		ptr++;
		CombatDifficulty = *ptr;
		ptr++;
		ReadingDifficulty = *ptr;
		ptr++;
		BreakoutDifficulty = *ptr;
		ptr++;
		LoopingDifficulty = *ptr;
		ptr++;
		EnemyPracticeLevel = *ptr;
		ptr++;
		FavorabilityChange = *ptr;
		ptr++;
		HereticsAmountType = *ptr;
		ptr++;
		BossInvasionSpeedType = *ptr;
		ptr++;
		WorldResourceAmountType = *ptr;
		ptr++;
		ProfessionUpgrade = *ptr;
		ptr++;
		LootYield = *(short*)ptr;
		ptr += 2;
		AllowRandomTaiwuHeir = *ptr != 0;
		ptr++;
		RestrictOptionsBehaviorType = *ptr != 0;
		ptr++;
		TaiwuVillageStateTemplateId = (sbyte)(*ptr);
		ptr++;
		TaiwuVillageLandFormType = (sbyte)(*ptr);
		ptr++;
		int num = (int)(ptr - pData);
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}
}
