using System;
using System.Collections.Generic;
using Config.Common;

namespace Config;

[Serializable]
public class VillagerRoleAutoAction : ConfigData<VillagerRoleAutoActionItem, short>
{
	public static class DefKey
	{
		public const short FarmerAutoCollectResource = 0;

		public const short CraftsmanRepairItems = 1;

		public const short CraftsmanGetMaterial = 2;

		public const short CraftsmanImproveMaterial = 3;

		public const short DoctorCureOthers = 4;

		public const short MerchantCollectMoney = 5;

		public const short LiteratiEntertainOthers = 6;

		public const short SwordTombKeeperFightHeretics = 7;

		public const short VillageHeadBuildRelationships = 8;

		public const short VillageChangeRelationships = 9;
	}

	public static class DefValue
	{
		public static VillagerRoleAutoActionItem FarmerAutoCollectResource => Instance[(short)0];

		public static VillagerRoleAutoActionItem CraftsmanRepairItems => Instance[(short)1];

		public static VillagerRoleAutoActionItem CraftsmanGetMaterial => Instance[(short)2];

		public static VillagerRoleAutoActionItem CraftsmanImproveMaterial => Instance[(short)3];

		public static VillagerRoleAutoActionItem DoctorCureOthers => Instance[(short)4];

		public static VillagerRoleAutoActionItem MerchantCollectMoney => Instance[(short)5];

		public static VillagerRoleAutoActionItem LiteratiEntertainOthers => Instance[(short)6];

		public static VillagerRoleAutoActionItem SwordTombKeeperFightHeretics => Instance[(short)7];

		public static VillagerRoleAutoActionItem VillageHeadBuildRelationships => Instance[(short)8];

		public static VillagerRoleAutoActionItem VillageChangeRelationships => Instance[(short)9];
	}

	public static VillagerRoleAutoAction Instance = new VillagerRoleAutoAction();

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
		_dataArray.Add(new VillagerRoleAutoActionItem(0, 0, 0, 1, null, null, 2, unlockByChicken: false));
		_dataArray.Add(new VillagerRoleAutoActionItem(1, 1, 3, 4, null, null, 5, unlockByChicken: false));
		_dataArray.Add(new VillagerRoleAutoActionItem(2, 1, 6, 7, null, null, 8, unlockByChicken: false));
		_dataArray.Add(new VillagerRoleAutoActionItem(3, 1, 9, 10, null, null, 11, unlockByChicken: true));
		_dataArray.Add(new VillagerRoleAutoActionItem(4, 2, 12, 13, null, null, 14, unlockByChicken: false));
		_dataArray.Add(new VillagerRoleAutoActionItem(5, 3, 15, 16, null, null, 17, unlockByChicken: false));
		_dataArray.Add(new VillagerRoleAutoActionItem(6, 4, 18, 19, null, null, 20, unlockByChicken: false));
		_dataArray.Add(new VillagerRoleAutoActionItem(7, 5, 21, 22, null, null, 23, unlockByChicken: false));
		_dataArray.Add(new VillagerRoleAutoActionItem(8, 6, 24, 25, null, null, 26, unlockByChicken: false));
		_dataArray.Add(new VillagerRoleAutoActionItem(9, 6, 27, 28, null, null, 29, unlockByChicken: true));
	}

	public override void Init()
	{
		base.Init();
		_dataArray = new List<VillagerRoleAutoActionItem>(10);
		CreateItems0();
	}
}
