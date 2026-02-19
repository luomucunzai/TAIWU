using System;
using System.Collections.Generic;
using Config.Common;

namespace Config;

[Serializable]
public class MapBlockMatcher : ConfigData<MapBlockMatcherItem, short>
{
	public static class DefKey
	{
		public const short NonDeveloped = 0;

		public const short NonDevelopedNatural = 1;

		public const short NonDevelopedNaturalNoEffectAndAdventure = 2;
	}

	public static class DefValue
	{
		public static MapBlockMatcherItem NonDeveloped => Instance[(short)0];

		public static MapBlockMatcherItem NonDevelopedNatural => Instance[(short)1];

		public static MapBlockMatcherItem NonDevelopedNaturalNoEffectAndAdventure => Instance[(short)2];
	}

	public static MapBlockMatcher Instance = new MapBlockMatcher();

	private readonly HashSet<string> RequiredFields = new HashSet<string> { "IncludeTypes", "IncludeSubTypes", "ExcludeTypes", "ExcludeSubTypes", "TemplateId", "ExcludeBlocksWithAdventure", "ExcludeBlocksWithEffect" };

	internal override int ToInt(short value)
	{
		return value;
	}

	internal override short ToTemplateId(int value)
	{
		return (short)value;
	}

	private void CreateItems0()
	{
		_dataArray.Add(new MapBlockMatcherItem(0, new EMapBlockType[4]
		{
			EMapBlockType.Normal,
			EMapBlockType.Wild,
			EMapBlockType.Bad,
			EMapBlockType.scenery
		}, null, null, null, excludeBlocksWithAdventure: false, excludeBlocksWithEffect: false));
		_dataArray.Add(new MapBlockMatcherItem(1, new EMapBlockType[4]
		{
			EMapBlockType.Normal,
			EMapBlockType.Wild,
			EMapBlockType.Bad,
			EMapBlockType.scenery
		}, null, null, new EMapBlockSubType[4]
		{
			EMapBlockSubType.SwordTomb,
			EMapBlockSubType.DLCLoong,
			EMapBlockSubType.Ruin,
			EMapBlockSubType.DarkPool
		}, excludeBlocksWithAdventure: false, excludeBlocksWithEffect: false));
		_dataArray.Add(new MapBlockMatcherItem(2, new EMapBlockType[4]
		{
			EMapBlockType.Normal,
			EMapBlockType.Wild,
			EMapBlockType.Bad,
			EMapBlockType.scenery
		}, null, null, new EMapBlockSubType[4]
		{
			EMapBlockSubType.SwordTomb,
			EMapBlockSubType.DLCLoong,
			EMapBlockSubType.Ruin,
			EMapBlockSubType.DarkPool
		}, excludeBlocksWithAdventure: true, excludeBlocksWithEffect: true));
	}

	public override void Init()
	{
		base.Init();
		_dataArray = new List<MapBlockMatcherItem>(3);
		CreateItems0();
	}
}
