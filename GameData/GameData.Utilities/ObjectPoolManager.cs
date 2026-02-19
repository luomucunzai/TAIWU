using System;
using System.Collections.Generic;
using System.Text;
using Config;
using GameData.Domains.Adventure;
using GameData.Domains.Building;
using GameData.Domains.Character;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.Information;
using GameData.Domains.Item;
using GameData.Domains.Map;
using GameData.Domains.Organization;
using GameData.Domains.SpecialEffect;

namespace GameData.Utilities;

public static class ObjectPoolManager
{
	public static void Initialize()
	{
		int processorCount = Environment.ProcessorCount;
		int num = processorCount * 2;
		ObjectPool<List<bool>>.Instance = new CollectionObjectPool<List<bool>, bool>(processorCount, num);
		ObjectPool<List<sbyte>>.Instance = new CollectionObjectPool<List<sbyte>, sbyte>(processorCount, num);
		ObjectPool<List<byte>>.Instance = new CollectionObjectPool<List<byte>, byte>(processorCount, num);
		ObjectPool<List<short>>.Instance = new CollectionObjectPool<List<short>, short>(processorCount, num);
		ObjectPool<List<short[]>>.Instance = new CollectionObjectPool<List<short[]>, short[]>(processorCount, num);
		ObjectPool<List<ushort>>.Instance = new CollectionObjectPool<List<ushort>, ushort>(processorCount, num);
		ObjectPool<List<ushort[]>>.Instance = new CollectionObjectPool<List<ushort[]>, ushort[]>(processorCount, num);
		ObjectPool<List<int>>.Instance = new CollectionObjectPool<List<int>, int>(processorCount, num);
		ObjectPool<List<int[]>>.Instance = new CollectionObjectPool<List<int[]>, int[]>(processorCount, num);
		ObjectPool<HashSet<int>>.Instance = new CollectionObjectPool<HashSet<int>, int>(processorCount, num);
		ObjectPool<HashSet<short>>.Instance = new CollectionObjectPool<HashSet<short>, short>(processorCount, num);
		ObjectPool<List<long>>.Instance = new CollectionObjectPool<List<long>, long>(processorCount, num);
		ObjectPool<List<(int, int)>>.Instance = new CollectionObjectPool<List<(int, int)>, (int, int)>(processorCount, num);
		ObjectPool<List<(byte, byte)>>.Instance = new CollectionObjectPool<List<(byte, byte)>, (byte, byte)>(processorCount, num);
		ObjectPool<List<(long, int)>>.Instance = new CollectionObjectPool<List<(long, int)>, (long, int)>(processorCount, num);
		ObjectPool<List<(sbyte, bool)>>.Instance = new CollectionObjectPool<List<(sbyte, bool)>, (sbyte, bool)>(processorCount, num);
		ObjectPool<Dictionary<int, int>>.Instance = new CollectionObjectPool<Dictionary<int, int>, KeyValuePair<int, int>>(processorCount, num);
		ObjectPool<Dictionary<short, int>>.Instance = new CollectionObjectPool<Dictionary<short, int>, KeyValuePair<short, int>>(processorCount, num);
		ObjectPool<Dictionary<short, short>>.Instance = new CollectionObjectPool<Dictionary<short, short>, KeyValuePair<short, short>>(processorCount, num);
		ObjectPool<Dictionary<byte, int>>.Instance = new CollectionObjectPool<Dictionary<byte, int>, KeyValuePair<byte, int>>(processorCount, num);
		ObjectPool<Dictionary<int, sbyte>>.Instance = new CollectionObjectPool<Dictionary<int, sbyte>, KeyValuePair<int, sbyte>>(processorCount, num);
		ObjectPool<Dictionary<sbyte, byte>>.Instance = new CollectionObjectPool<Dictionary<sbyte, byte>, KeyValuePair<sbyte, byte>>(processorCount, num);
		ObjectPool<Dictionary<int, IntPair>>.Instance = new CollectionObjectPool<Dictionary<int, IntPair>, KeyValuePair<int, IntPair>>(processorCount, num);
		ObjectPool<Dictionary<ItemKey, int>>.Instance = new CollectionObjectPool<Dictionary<ItemKey, int>, KeyValuePair<ItemKey, int>>(processorCount, num);
		ObjectPool<Dictionary<sbyte, OuterAndInnerInts>>.Instance = new CollectionObjectPool<Dictionary<sbyte, OuterAndInnerInts>, KeyValuePair<sbyte, OuterAndInnerInts>>(processorCount, num);
		ObjectPool<List<string>>.Instance = new CollectionObjectPool<List<string>, string>(processorCount, num);
		ObjectPool<HashSet<string>>.Instance = new CollectionObjectPool<HashSet<string>, string>(2, 4);
		ObjectPool<HashSet<sbyte>>.Instance = new CollectionObjectPool<HashSet<sbyte>, sbyte>(2, 4);
		ObjectPool<List<ByteCoordinate>>.Instance = new CollectionObjectPool<List<ByteCoordinate>, ByteCoordinate>(processorCount * 2, num * 2);
		ObjectPool<List<List<ByteCoordinate>>>.Instance = new CollectionObjectPool<List<List<ByteCoordinate>>, List<ByteCoordinate>>(processorCount * 2, num * 2);
		ObjectPool<List<MapBlockData>>.Instance = new CollectionObjectPool<List<MapBlockData>, MapBlockData>(4, 8);
		ObjectPool<List<Location>>.Instance = new CollectionObjectPool<List<Location>, Location>(processorCount * 2, num * 2);
		ObjectPool<List<GameData.Domains.Character.Character>>.Instance = new CollectionObjectPool<List<GameData.Domains.Character.Character>, GameData.Domains.Character.Character>(2, 4);
		ObjectPool<List<GameData.Domains.Character.CombatSkillHelper.AttainmentSectInfo>>.Instance = new CollectionObjectPool<List<GameData.Domains.Character.CombatSkillHelper.AttainmentSectInfo>, GameData.Domains.Character.CombatSkillHelper.AttainmentSectInfo>(processorCount, num);
		ObjectPool<List<ItemKey>>.Instance = new CollectionObjectPool<List<ItemKey>, ItemKey>(2, 4);
		ObjectPool<List<ItemKeyAndDate>>.Instance = new CollectionObjectPool<List<ItemKeyAndDate>, ItemKeyAndDate>(2, 4);
		ObjectPool<List<Wager>>.Instance = new CollectionObjectPool<List<Wager>, Wager>(2, 4);
		ObjectPool<List<Predicate<GameData.Domains.Character.Character>>>.Instance = new CollectionObjectPool<List<Predicate<GameData.Domains.Character.Character>>, Predicate<GameData.Domains.Character.Character>>(processorCount, num);
		ObjectPool<List<CombatSkillKey>>.Instance = new CollectionObjectPool<List<CombatSkillKey>, CombatSkillKey>(processorCount, num);
		ObjectPool<List<GameData.Domains.CombatSkill.CombatSkill>>.Instance = new CollectionObjectPool<List<GameData.Domains.CombatSkill.CombatSkill>, GameData.Domains.CombatSkill.CombatSkill>(2, 4);
		ObjectPool<List<MapTemplateEnemyInfo>>.Instance = new CollectionObjectPool<List<MapTemplateEnemyInfo>, MapTemplateEnemyInfo>(processorCount, num);
		ObjectPool<List<NeedTrick>>.Instance = new CollectionObjectPool<List<NeedTrick>, NeedTrick>(processorCount, num);
		ObjectPool<List<SpecialEffectBase>>.Instance = new CollectionObjectPool<List<SpecialEffectBase>, SpecialEffectBase>(processorCount, num);
		ObjectPool<List<SkillEffectKey>>.Instance = new CollectionObjectPool<List<SkillEffectKey>, SkillEffectKey>(processorCount, num);
		ObjectPool<List<Settlement>>.Instance = new CollectionObjectPool<List<Settlement>, Settlement>(4, 8);
		ObjectPool<List<SecretInformationMetaData>>.Instance = new CollectionObjectPool<List<SecretInformationMetaData>, SecretInformationMetaData>(processorCount * 2, num * 2);
		ObjectPool<HashSet<ItemKey>>.Instance = new CollectionObjectPool<HashSet<ItemKey>, ItemKey>(processorCount, num);
		ObjectPool<HashSet<SecretInformationRelationshipType>>.Instance = new CollectionObjectPool<HashSet<SecretInformationRelationshipType>, SecretInformationRelationshipType>(processorCount, num);
		ObjectPool<HashSet<BuildingBlockKey>>.Instance = new CollectionObjectPool<HashSet<BuildingBlockKey>, BuildingBlockKey>(processorCount, num);
		ObjectPool<List<BuildingBlockKey>>.Instance = new CollectionObjectPool<List<BuildingBlockKey>, BuildingBlockKey>(processorCount, num);
		ObjectPool<List<(ItemKey, int)>>.Instance = new CollectionObjectPool<List<(ItemKey, int)>, (ItemKey, int)>(processorCount, num);
		ObjectPool<StringBuilder>.Instance = new ObjectPool<StringBuilder>(processorCount, num);
		ObjectPool<List<TemplateKey>>.Instance = new ObjectPool<List<TemplateKey>>(processorCount, num);
	}
}
