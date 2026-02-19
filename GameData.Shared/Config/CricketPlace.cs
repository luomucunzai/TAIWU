using System;
using System.Collections.Generic;
using Config.Common;

namespace Config;

[Serializable]
public class CricketPlace : ConfigData<CricketPlaceItem, sbyte>
{
	public static CricketPlace Instance = new CricketPlace();

	private readonly HashSet<string> RequiredFields = new HashSet<string> { "UselessItemList", "TemplateId", "Icon", "CatchAniBack", "CatchAni" };

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
		_dataArray.Add(new CricketPlaceItem(0, 1, 2, 3, 6, 12, 96, 12, 32, "CricketPlaceImage0", "catchcricket_01_mh_mubu_0", "cricket_grass_SkeletonData", new short[18]
		{
			10, 11, 12, 13, 14, 15, 16, 17, 18, 19,
			20, 21, 22, 23, 24, 25, 26, 27
		}));
		_dataArray.Add(new CricketPlaceItem(1, 2, 3, 4, 8, 16, 88, 11, 28, "CricketPlaceImage1", "catchcricket_01_mh_mubu_0", "cricket_wood_SkeletonData", new short[18]
		{
			10, 11, 12, 13, 14, 15, 16, 17, 18, 19,
			20, 21, 22, 23, 24, 25, 26, 27
		}));
		_dataArray.Add(new CricketPlaceItem(2, 3, 4, 5, 10, 20, 80, 10, 24, "CricketPlaceImage2", "catchcricket_01_mh_mubu_3", "cricket_hill_SkeletonData", new short[18]
		{
			10, 11, 12, 13, 14, 15, 16, 17, 18, 19,
			20, 21, 22, 23, 24, 25, 26, 27
		}));
		_dataArray.Add(new CricketPlaceItem(3, 4, 5, 6, 12, 24, 72, 9, 20, "CricketPlaceImage3", "catchcricket_01_mh_mubu_2", "cricket_farm_SkeletonData", new short[18]
		{
			10, 11, 12, 13, 14, 15, 16, 17, 18, 19,
			20, 21, 22, 23, 24, 25, 26, 27
		}));
		_dataArray.Add(new CricketPlaceItem(4, 5, 6, 7, 14, 28, 64, 8, 16, "CricketPlaceImage4", "catchcricket_01_mh_mubu_0", "cricket_stone_SkeletonData", new short[18]
		{
			10, 11, 12, 13, 14, 15, 16, 17, 18, 19,
			20, 21, 22, 23, 24, 25, 26, 27
		}));
		_dataArray.Add(new CricketPlaceItem(5, 6, 7, 8, 16, 32, 56, 7, 12, "CricketPlaceImage5", "catchcricket_01_mh_mubu_1", "cricket_crock_SkeletonData", new short[18]
		{
			10, 11, 12, 13, 14, 15, 16, 17, 18, 19,
			20, 21, 22, 23, 24, 25, 26, 27
		}));
		_dataArray.Add(new CricketPlaceItem(6, 7, 8, 9, 18, 36, 48, 6, 6, "CricketPlaceImage6", "catchcricket_01_mh_mubu_0", "cricket_rick_SkeletonData", new short[18]
		{
			10, 11, 12, 13, 14, 15, 16, 17, 18, 19,
			20, 21, 22, 23, 24, 25, 26, 27
		}));
		_dataArray.Add(new CricketPlaceItem(7, 8, 9, 10, 20, 40, 40, 5, 3, "CricketPlaceImage7", "catchcricket_01_mh_mubu_2", "cricket_cave_SkeletonData", new short[18]
		{
			10, 11, 12, 13, 14, 15, 16, 17, 18, 19,
			20, 21, 22, 23, 24, 25, 26, 27
		}));
		_dataArray.Add(new CricketPlaceItem(8, 9, 10, 11, 22, 44, 32, 4, 2, "CricketPlaceImage8", "catchcricket_01_mh_mubu_1", "cricket_bull_SkeletonData", new short[18]
		{
			10, 11, 12, 13, 14, 15, 16, 17, 18, 19,
			20, 21, 22, 23, 24, 25, 26, 27
		}));
		_dataArray.Add(new CricketPlaceItem(9, 10, 11, 12, 24, 48, 24, 3, 1, "CricketPlaceImage9", "catchcricket_01_mh_mubu_1", "cricket_tomb_SkeletonData", new short[18]
		{
			10, 11, 12, 13, 14, 15, 16, 17, 18, 19,
			20, 21, 22, 23, 24, 25, 26, 27
		}));
	}

	public override void Init()
	{
		base.Init();
		_dataArray = new List<CricketPlaceItem>(10);
		CreateItems0();
	}
}
