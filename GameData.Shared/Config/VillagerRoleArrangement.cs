using System;
using System.Collections.Generic;
using Config.Common;

namespace Config;

[Serializable]
public class VillagerRoleArrangement : ConfigData<VillagerRoleArrangementItem, short>
{
	public static class DefKey
	{
		public const short Cooking = 12;

		public const short CollectResource = 5;

		public const short MigrateResource = 6;

		public const short IntensiveCultivation = 7;

		public const short Making = 13;

		public const short MakingMedicine = 14;

		public const short Healing = 0;

		public const short ReduceXiangshuInfection = 8;

		public const short Peddling = 1;

		public const short CommerceContacting = 9;

		public const short MakingTeaWine = 15;

		public const short Entertaining = 2;

		public const short JianghuContacting = 10;

		public const short GuardingSwordTomb = 3;

		public const short ResistXiangshuInfection = 11;

		public const short TaiwuEnvoy = 4;
	}

	public static class DefValue
	{
		public static VillagerRoleArrangementItem Cooking => Instance[(short)12];

		public static VillagerRoleArrangementItem CollectResource => Instance[(short)5];

		public static VillagerRoleArrangementItem MigrateResource => Instance[(short)6];

		public static VillagerRoleArrangementItem IntensiveCultivation => Instance[(short)7];

		public static VillagerRoleArrangementItem Making => Instance[(short)13];

		public static VillagerRoleArrangementItem MakingMedicine => Instance[(short)14];

		public static VillagerRoleArrangementItem Healing => Instance[(short)0];

		public static VillagerRoleArrangementItem ReduceXiangshuInfection => Instance[(short)8];

		public static VillagerRoleArrangementItem Peddling => Instance[(short)1];

		public static VillagerRoleArrangementItem CommerceContacting => Instance[(short)9];

		public static VillagerRoleArrangementItem MakingTeaWine => Instance[(short)15];

		public static VillagerRoleArrangementItem Entertaining => Instance[(short)2];

		public static VillagerRoleArrangementItem JianghuContacting => Instance[(short)10];

		public static VillagerRoleArrangementItem GuardingSwordTomb => Instance[(short)3];

		public static VillagerRoleArrangementItem ResistXiangshuInfection => Instance[(short)11];

		public static VillagerRoleArrangementItem TaiwuEnvoy => Instance[(short)4];
	}

	public static VillagerRoleArrangement Instance = new VillagerRoleArrangement();

	private readonly HashSet<string> RequiredFields = new HashSet<string> { "VillagerRole", "TemplateId", "ShortName", "Name", "DisplayIcon", "DisplayIcon2", "Desc" };

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
		_dataArray.Add(new VillagerRoleArrangementItem(0, 2, 18, 19, null, null, 20, unlockByChicken: false, invisibleInGui: false));
		_dataArray.Add(new VillagerRoleArrangementItem(1, 3, 24, 25, null, null, 26, unlockByChicken: false, invisibleInGui: false));
		_dataArray.Add(new VillagerRoleArrangementItem(2, 4, 33, 34, null, null, 35, unlockByChicken: false, invisibleInGui: false));
		_dataArray.Add(new VillagerRoleArrangementItem(3, 5, 39, 40, null, null, 41, unlockByChicken: false, invisibleInGui: false));
		_dataArray.Add(new VillagerRoleArrangementItem(4, 6, 45, 46, null, null, 47, unlockByChicken: false, invisibleInGui: false));
		_dataArray.Add(new VillagerRoleArrangementItem(5, 0, 3, 4, null, null, 5, unlockByChicken: false, invisibleInGui: true));
		_dataArray.Add(new VillagerRoleArrangementItem(6, 0, 6, 7, null, null, 8, unlockByChicken: false, invisibleInGui: true));
		_dataArray.Add(new VillagerRoleArrangementItem(7, 0, 9, 10, null, null, 11, unlockByChicken: true, invisibleInGui: true));
		_dataArray.Add(new VillagerRoleArrangementItem(8, 2, 21, 22, null, null, 23, unlockByChicken: true, invisibleInGui: true));
		_dataArray.Add(new VillagerRoleArrangementItem(9, 3, 27, 28, null, null, 29, unlockByChicken: true, invisibleInGui: true));
		_dataArray.Add(new VillagerRoleArrangementItem(10, 4, 36, 37, null, null, 38, unlockByChicken: true, invisibleInGui: true));
		_dataArray.Add(new VillagerRoleArrangementItem(11, 5, 42, 43, null, null, 44, unlockByChicken: true, invisibleInGui: true));
		_dataArray.Add(new VillagerRoleArrangementItem(12, 0, 0, 1, null, null, 2, unlockByChicken: false, invisibleInGui: true));
		_dataArray.Add(new VillagerRoleArrangementItem(13, 1, 12, 13, null, null, 14, unlockByChicken: false, invisibleInGui: true));
		_dataArray.Add(new VillagerRoleArrangementItem(14, 2, 15, 16, null, null, 17, unlockByChicken: false, invisibleInGui: true));
		_dataArray.Add(new VillagerRoleArrangementItem(15, 4, 30, 31, null, null, 32, unlockByChicken: false, invisibleInGui: false));
	}

	public override void Init()
	{
		base.Init();
		_dataArray = new List<VillagerRoleArrangementItem>(16);
		CreateItems0();
	}
}
