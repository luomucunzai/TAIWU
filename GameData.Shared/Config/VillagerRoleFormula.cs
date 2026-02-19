using System;
using System.Collections.Generic;
using Config.Common;

namespace Config;

[Serializable]
public class VillagerRoleFormula : ConfigData<VillagerRoleFormulaItem, int>
{
	public static class DefKey
	{
		public const int FarmerAutoCollectActionCount = 0;

		public const int FarmerAutoCollectActionResult = 1;

		public const int FarmerMigrateResourceSuccessRate = 2;

		public const int FarmerMigrateResourceExtraSuccessRate = 47;

		public const int FarmerChickenUpgradeBuildingCoreRate = 3;

		public const int DoctorInteractTargetGrade = 4;

		public const int DoctorInteractTargetMaxGrade = 5;

		public const int DoctorAutoActionAuthorityIncome = 6;

		public const int DoctorAutoActionAuthorityIncomeAdjust = 7;

		public const int DoctorWorkSpiritualDebtIncome = 8;

		public const int DoctorChickenUpgradeInfectionChangeAmount = 9;

		public const int DoctorCureRequirement = 10;

		public const int VillageHeadAutoActionAffectCount = 11;

		public const int VillageHeadAutoActionFavorChange = 12;

		public const int VillageHeadAutoActionFavorIncreaseRate = 13;

		public const int VillageHeadWorkChangeRuleRange = 14;

		public const int VillageHeadWorkSpecialRuleCount = 15;

		public const int VillageHeadWorkMonthlyAuthorityCost = 16;

		public const int VillageHeadChickenUpgradeActionChance = 17;

		public const int LiteratiAutoActionInfluenceCount = 18;

		public const int LiteratiAutoActionHappinessChange = 19;

		public const int LiteratiWorkUsableCount = 20;

		public const int LiteratiWorkEffectiveValue = 21;

		public const int LiteratiChickenInfluenceCount = 22;

		public const int LiteratiChickenRelationChange = 23;

		public const int MerchantInteractTargetGrade = 24;

		public const int MerchantInteractTargetMaxGrade = 25;

		public const int MerchantAutoActionMoneyIncome = 26;

		public const int MerchantAutoActionMoneyIncomeAdjust = 27;

		public const int MerchantAutoActionTargetMoneyRequirement = 28;

		public const int MerchantBuyItemPriceRate = 29;

		public const int MerchantSellItemPriceRate = 30;

		public const int MerchantChickenIncreaseHeadMerchantFavor = 31;

		public const int MerchantChickenIncreaseBranchMerchantFavor = 32;

		public const int SwordTombKeeperAutoActionKOCount = 33;

		public const int SwordTombKeeperWorkInfectAddPerMonth = 34;

		public const int SwordTombKeeperWorkCollectOdd = 35;

		public const int SwordTombKeeperWorkHurtOdd = 36;

		public const int SwordTombKeeperWorkHurtCount = 37;

		public const int SwordTombKeeperWorkFeatureOddWhenInformationCollect = 38;

		public const int SwordTombKeeperWorkFeatureOddWhenBeAttacked = 39;

		public const int SwordTombKeeperChickenDecreaseFactor = 40;

		public const int CraftsmanAutoRepairCount = 41;

		public const int CraftsmanAutoGainRefineMaterialChance = 42;

		public const int CraftsmanAutoGainRefineMaterialGrade = 43;

		public const int CraftsmanChikenUpgradeAutoGainRefineMaterialGrade = 44;

		public const int BaseFavorAddGainRole = 45;

		public const int BaseFavorCostLostRole = 46;
	}

	public static class DefValue
	{
		public static VillagerRoleFormulaItem FarmerAutoCollectActionCount => Instance[0];

		public static VillagerRoleFormulaItem FarmerAutoCollectActionResult => Instance[1];

		public static VillagerRoleFormulaItem FarmerMigrateResourceSuccessRate => Instance[2];

		public static VillagerRoleFormulaItem FarmerMigrateResourceExtraSuccessRate => Instance[47];

		public static VillagerRoleFormulaItem FarmerChickenUpgradeBuildingCoreRate => Instance[3];

		public static VillagerRoleFormulaItem DoctorInteractTargetGrade => Instance[4];

		public static VillagerRoleFormulaItem DoctorInteractTargetMaxGrade => Instance[5];

		public static VillagerRoleFormulaItem DoctorAutoActionAuthorityIncome => Instance[6];

		public static VillagerRoleFormulaItem DoctorAutoActionAuthorityIncomeAdjust => Instance[7];

		public static VillagerRoleFormulaItem DoctorWorkSpiritualDebtIncome => Instance[8];

		public static VillagerRoleFormulaItem DoctorChickenUpgradeInfectionChangeAmount => Instance[9];

		public static VillagerRoleFormulaItem DoctorCureRequirement => Instance[10];

		public static VillagerRoleFormulaItem VillageHeadAutoActionAffectCount => Instance[11];

		public static VillagerRoleFormulaItem VillageHeadAutoActionFavorChange => Instance[12];

		public static VillagerRoleFormulaItem VillageHeadAutoActionFavorIncreaseRate => Instance[13];

		public static VillagerRoleFormulaItem VillageHeadWorkChangeRuleRange => Instance[14];

		public static VillagerRoleFormulaItem VillageHeadWorkSpecialRuleCount => Instance[15];

		public static VillagerRoleFormulaItem VillageHeadWorkMonthlyAuthorityCost => Instance[16];

		public static VillagerRoleFormulaItem VillageHeadChickenUpgradeActionChance => Instance[17];

		public static VillagerRoleFormulaItem LiteratiAutoActionInfluenceCount => Instance[18];

		public static VillagerRoleFormulaItem LiteratiAutoActionHappinessChange => Instance[19];

		public static VillagerRoleFormulaItem LiteratiWorkUsableCount => Instance[20];

		public static VillagerRoleFormulaItem LiteratiWorkEffectiveValue => Instance[21];

		public static VillagerRoleFormulaItem LiteratiChickenInfluenceCount => Instance[22];

		public static VillagerRoleFormulaItem LiteratiChickenRelationChange => Instance[23];

		public static VillagerRoleFormulaItem MerchantInteractTargetGrade => Instance[24];

		public static VillagerRoleFormulaItem MerchantInteractTargetMaxGrade => Instance[25];

		public static VillagerRoleFormulaItem MerchantAutoActionMoneyIncome => Instance[26];

		public static VillagerRoleFormulaItem MerchantAutoActionMoneyIncomeAdjust => Instance[27];

		public static VillagerRoleFormulaItem MerchantAutoActionTargetMoneyRequirement => Instance[28];

		public static VillagerRoleFormulaItem MerchantBuyItemPriceRate => Instance[29];

		public static VillagerRoleFormulaItem MerchantSellItemPriceRate => Instance[30];

		public static VillagerRoleFormulaItem MerchantChickenIncreaseHeadMerchantFavor => Instance[31];

		public static VillagerRoleFormulaItem MerchantChickenIncreaseBranchMerchantFavor => Instance[32];

		public static VillagerRoleFormulaItem SwordTombKeeperAutoActionKOCount => Instance[33];

		public static VillagerRoleFormulaItem SwordTombKeeperWorkInfectAddPerMonth => Instance[34];

		public static VillagerRoleFormulaItem SwordTombKeeperWorkCollectOdd => Instance[35];

		public static VillagerRoleFormulaItem SwordTombKeeperWorkHurtOdd => Instance[36];

		public static VillagerRoleFormulaItem SwordTombKeeperWorkHurtCount => Instance[37];

		public static VillagerRoleFormulaItem SwordTombKeeperWorkFeatureOddWhenInformationCollect => Instance[38];

		public static VillagerRoleFormulaItem SwordTombKeeperWorkFeatureOddWhenBeAttacked => Instance[39];

		public static VillagerRoleFormulaItem SwordTombKeeperChickenDecreaseFactor => Instance[40];

		public static VillagerRoleFormulaItem CraftsmanAutoRepairCount => Instance[41];

		public static VillagerRoleFormulaItem CraftsmanAutoGainRefineMaterialChance => Instance[42];

		public static VillagerRoleFormulaItem CraftsmanAutoGainRefineMaterialGrade => Instance[43];

		public static VillagerRoleFormulaItem CraftsmanChikenUpgradeAutoGainRefineMaterialGrade => Instance[44];

		public static VillagerRoleFormulaItem BaseFavorAddGainRole => Instance[45];

		public static VillagerRoleFormulaItem BaseFavorCostLostRole => Instance[46];
	}

	public static VillagerRoleFormula Instance = new VillagerRoleFormula();

	private readonly HashSet<string> RequiredFields = new HashSet<string> { "TemplateId", "DisplayName", "DisplayFormat" };

	internal override int ToInt(int value)
	{
		return value;
	}

	internal override int ToTemplateId(int value)
	{
		return value;
	}

	private void CreateItems0()
	{
		_dataArray.Add(new VillagerRoleFormulaItem(0, EVillagerRoleFormulaType.Formula4, new int[2] { 1, 20 }, -1, 0, 1));
		_dataArray.Add(new VillagerRoleFormulaItem(1, EVillagerRoleFormulaType.Formula3, new int[1] { 5 }, -1, 2, 3));
		_dataArray.Add(new VillagerRoleFormulaItem(2, EVillagerRoleFormulaType.Formula4, new int[2] { 10, 5 }, -1, 4, 5));
		_dataArray.Add(new VillagerRoleFormulaItem(3, EVillagerRoleFormulaType.Formula4, new int[2] { 5, 10 }, -1, 8, 9));
		_dataArray.Add(new VillagerRoleFormulaItem(4, EVillagerRoleFormulaType.Formula4, new int[2] { 2, 15 }, -1, 10, 11));
		_dataArray.Add(new VillagerRoleFormulaItem(5, EVillagerRoleFormulaType.Formula1, new int[1] { 2 }, -1, 12, 13));
		_dataArray.Add(new VillagerRoleFormulaItem(6, EVillagerRoleFormulaType.Formula7, new int[2] { 25, 100 }, -1, 14, 15));
		_dataArray.Add(new VillagerRoleFormulaItem(7, EVillagerRoleFormulaType.Formula8, new int[3] { 80, 120, 100 }, -1, 16, 17));
		_dataArray.Add(new VillagerRoleFormulaItem(8, EVillagerRoleFormulaType.Formula6, new int[2] { 1, 50 }, -1, 18, 19));
		_dataArray.Add(new VillagerRoleFormulaItem(9, EVillagerRoleFormulaType.Formula9, new int[3] { 5, 50, 100 }, -1, 20, 21));
		_dataArray.Add(new VillagerRoleFormulaItem(10, EVillagerRoleFormulaType.Formula10, new int[5] { 6, 3, 2000, 25, 100 }, -1, 22, 23));
		_dataArray.Add(new VillagerRoleFormulaItem(11, EVillagerRoleFormulaType.Formula4, new int[2] { 1, 20 }, -1, 24, 25));
		_dataArray.Add(new VillagerRoleFormulaItem(12, EVillagerRoleFormulaType.Formula2, new int[1] { 20 }, -1, 26, 27));
		_dataArray.Add(new VillagerRoleFormulaItem(13, EVillagerRoleFormulaType.Formula10, new int[5] { 75, 100, 50, 0, 25 }, -1, 28, 29));
		_dataArray.Add(new VillagerRoleFormulaItem(14, EVillagerRoleFormulaType.Formula1, new int[1] { 1 }, -1, 30, 31));
		_dataArray.Add(new VillagerRoleFormulaItem(15, EVillagerRoleFormulaType.Formula4, new int[2] { 1, 20 }, -1, 32, 33));
		_dataArray.Add(new VillagerRoleFormulaItem(16, EVillagerRoleFormulaType.Formula11, new int[4] { 10, 100, 10, 100 }, -1, 34, 35));
		_dataArray.Add(new VillagerRoleFormulaItem(17, EVillagerRoleFormulaType.Formula3, new int[1] { 2 }, -1, 36, 37));
		_dataArray.Add(new VillagerRoleFormulaItem(18, EVillagerRoleFormulaType.Formula4, new int[2] { 1, 10 }, -1, 38, 39));
		_dataArray.Add(new VillagerRoleFormulaItem(19, EVillagerRoleFormulaType.Formula4, new int[2] { 3, 100 }, -1, 40, 41));
		_dataArray.Add(new VillagerRoleFormulaItem(20, EVillagerRoleFormulaType.Formula4, new int[2] { 1, 20 }, -1, 42, 33));
		_dataArray.Add(new VillagerRoleFormulaItem(21, EVillagerRoleFormulaType.Formula4, new int[2] { 3, 100 }, -1, 43, 33));
		_dataArray.Add(new VillagerRoleFormulaItem(22, EVillagerRoleFormulaType.Formula4, new int[2] { 1, 20 }, -1, 44, 33));
		_dataArray.Add(new VillagerRoleFormulaItem(23, EVillagerRoleFormulaType.Formula2, new int[1] { 5 }, -1, 45, 33));
		_dataArray.Add(new VillagerRoleFormulaItem(24, EVillagerRoleFormulaType.Formula4, new int[2] { 2, 15 }, -1, 46, 11));
		_dataArray.Add(new VillagerRoleFormulaItem(25, EVillagerRoleFormulaType.Formula1, new int[1] { 2 }, -1, 47, 48));
		_dataArray.Add(new VillagerRoleFormulaItem(26, EVillagerRoleFormulaType.Formula7, new int[2] { 50, 100 }, -1, 49, 50));
		_dataArray.Add(new VillagerRoleFormulaItem(27, EVillagerRoleFormulaType.Formula8, new int[3] { 80, 120, 100 }, -1, 51, 52));
		_dataArray.Add(new VillagerRoleFormulaItem(28, EVillagerRoleFormulaType.Formula3, new int[1] { 2 }, -1, 53, 54));
		_dataArray.Add(new VillagerRoleFormulaItem(29, EVillagerRoleFormulaType.Formula4, new int[2] { 300, -5 }, -1, 55, 35));
		_dataArray.Add(new VillagerRoleFormulaItem(30, EVillagerRoleFormulaType.Formula4, new int[2] { 50, 10 }, -1, 56, 35));
		_dataArray.Add(new VillagerRoleFormulaItem(31, EVillagerRoleFormulaType.Formula9, new int[3] { 1000, 10, 1 }, -1, 57, 58));
		_dataArray.Add(new VillagerRoleFormulaItem(32, EVillagerRoleFormulaType.Formula9, new int[3] { 500, 20, 1 }, -1, 59, 58));
		_dataArray.Add(new VillagerRoleFormulaItem(33, EVillagerRoleFormulaType.Formula4, new int[2] { 1, 20 }, -1, 60, 61));
		_dataArray.Add(new VillagerRoleFormulaItem(34, EVillagerRoleFormulaType.Formula0, new int[1] { 5 }, -1, 62, 63));
		_dataArray.Add(new VillagerRoleFormulaItem(35, EVillagerRoleFormulaType.Formula9, new int[3] { 10, 20, 100 }, -1, 64, 35));
		_dataArray.Add(new VillagerRoleFormulaItem(36, EVillagerRoleFormulaType.Formula9, new int[3] { 100, -20, 100 }, -1, 65, 35));
		_dataArray.Add(new VillagerRoleFormulaItem(37, EVillagerRoleFormulaType.Formula12, new int[1] { 2 }, -1, 66, 67));
		_dataArray.Add(new VillagerRoleFormulaItem(38, EVillagerRoleFormulaType.Formula9, new int[3] { 5, 20, 100 }, -1, 68, 35));
		_dataArray.Add(new VillagerRoleFormulaItem(39, EVillagerRoleFormulaType.Formula9, new int[3] { 5, 20, 100 }, -1, 69, 35));
		_dataArray.Add(new VillagerRoleFormulaItem(40, EVillagerRoleFormulaType.Formula9, new int[3] { 10, 10, 100 }, -1, 20, 70));
		_dataArray.Add(new VillagerRoleFormulaItem(41, EVillagerRoleFormulaType.Formula4, new int[2] { 1, 20 }, -1, 71, 72));
		_dataArray.Add(new VillagerRoleFormulaItem(42, EVillagerRoleFormulaType.Formula4, new int[2] { 10, 10 }, -1, 73, 74));
		_dataArray.Add(new VillagerRoleFormulaItem(43, EVillagerRoleFormulaType.Formula3, new int[1] { 100 }, -1, 75, 76));
		_dataArray.Add(new VillagerRoleFormulaItem(44, EVillagerRoleFormulaType.Formula4, new int[2] { 10, 10 }, -1, 77, 78));
		_dataArray.Add(new VillagerRoleFormulaItem(45, EVillagerRoleFormulaType.Formula2, new int[1] { 1000 }, -1, 79, 80));
		_dataArray.Add(new VillagerRoleFormulaItem(46, EVillagerRoleFormulaType.Formula2, new int[1] { 2000 }, -1, 81, 82));
		_dataArray.Add(new VillagerRoleFormulaItem(47, EVillagerRoleFormulaType.Formula14, new int[3] { 10, 5, 10 }, -1, 6, 7));
	}

	public override void Init()
	{
		base.Init();
		_dataArray = new List<VillagerRoleFormulaItem>(48);
		CreateItems0();
	}
}
