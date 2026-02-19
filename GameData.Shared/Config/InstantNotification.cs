using System;
using System.Collections.Generic;
using Config.Common;

namespace Config;

[Serializable]
public class InstantNotification : ConfigData<InstantNotificationItem, short>
{
	public static class DefKey
	{
		public const short BuildingUpgradingCompleted = 0;

		public const short BuildingDemolitionCompleted = 1;

		public const short BuildingCraftingCompleted = 2;

		public const short BuildingConstructionCompleted = 3;

		public const short BuildingProductGenerated = 4;

		public const short BuildingDamaged = 5;

		public const short BuildingRuined = 6;

		public const short BeginBuildingConstruction = 7;

		public const short BeginBuildingUpgrading = 8;

		public const short BeginBuildingDemolition = 9;

		public const short CancelBuildingDemolition = 10;

		public const short CandidateArrived = 11;

		public const short CandidateLeaved = 12;

		public const short JoinTaiwuVillage = 13;

		public const short LeaveTaiwuVillage = 14;

		public const short WarehouseItemLost = 15;

		public const short BuildingLoseAuthority = 16;

		public const short DiscoverRelay = 17;

		public const short WalkThroughAbyss = 18;

		public const short BeginAdventure = 19;

		public const short NaturalDisasterEncountered = 20;

		public const short JoinGroup = 21;

		public const short LeaveGroup = 22;

		public const short BehaviorTypeChanged = 23;

		public const short InheritedApprovingRateReceived = 24;

		public const short FameIncreased = 25;

		public const short FameDecreased = 26;

		public const short HappinessIncreased = 27;

		public const short HappinessDecreased = 28;

		public const short FavorabilityIncreased = 29;

		public const short FavorabilityDecreased = 30;

		public const short FavorabilityIncreasedAcrossLevels = 31;

		public const short FavorabilityDecreasedAcrossLevels = 32;

		public const short LovingItemRevealed = 33;

		public const short HatingItemRevealed = 34;

		public const short LovingItemRevealedNothing = 35;

		public const short HatingItemRevealedNothing = 36;

		public const short EatBloodDew = 37;

		public const short CombatSkillLearned = 38;

		public const short HealthIncreased = 39;

		public const short HealthDecreased = 40;

		public const short XiangshuInfectionIncreased = 41;

		public const short XiangshuInfectionDecreased = 42;

		public const short XiangshuPartlyInfected = 43;

		public const short XiangshuCompletelyInfected = 44;

		public const short MainAttributeRecovered = 45;

		public const short MainAttributeConsumed = 46;

		public const short DisorderOfQiIncreased = 47;

		public const short DisorderOfQiDecreased = 48;

		public const short InjuryIncreased = 49;

		public const short InjuryDecreased = 50;

		public const short PoisonIncreased = 51;

		public const short PoisonDecreased = 52;

		public const short ExpIncreased = 53;

		public const short ExpDecreased = 54;

		public const short ResourceIncreased = 55;

		public const short ResourceDecreased = 56;

		public const short GetItem = 57;

		public const short LoseItem = 58;

		public const short CharacterGrownUp = 59;

		public const short CharacterDead = 60;

		public const short CricketDead = 61;

		public const short FamilyDied = 62;

		public const short EnemyDied = 63;

		public const short EnemyLucky = 64;

		public const short EnemyUnlucky = 65;

		public const short EnemyLoseInLifeSkill = 66;

		public const short EnemyLoseInCombat = 67;

		public const short EnemyGreatLoseInCombat = 68;

		public const short FamilyMiscarriage = 69;

		public const short AbandonExposed = 70;

		public const short AbandonAcknowledged = 71;

		public const short FamilyAbandon = 72;

		public const short SelfImmoralLove = 73;

		public const short FamilyHaveKid = 74;

		public const short HaveKid = 75;

		public const short FamilyImmoralKid = 76;

		public const short ReligiousFamilyHaveKid = 77;

		public const short FamilyInLove = 78;

		public const short FamilyImmoralLove = 79;

		public const short ReligiousFamilyInLove = 80;

		public const short FamilyMarried = 81;

		public const short FamilyImmoralMarriage = 82;

		public const short FamilyBeFriendWithEnemy = 83;

		public const short FamilyJieyiWithEnemy = 84;

		public const short FamilyAdoptedMaleEnemy = 85;

		public const short FamilyAdoptedFemaleEnemy = 86;

		public const short FamilyAdoptedByMaleEnemy = 87;

		public const short FamilyAdoptedByFemaleEnemy = 88;

		public const short FamilyEndedImmoralLove = 89;

		public const short FamilyEndedFriendshipWithEnemy = 90;

		public const short FamilyEndedJieyiWithEnemy = 91;

		public const short LoverHaveSex = 92;

		public const short TaiwuVillageIdleCount = 93;

		public const short ProfessionSeniorityIncrease = 94;

		public const short ProfessionUnlockSkill = 95;

		public const short ProfessionSkillHasCoolDown = 96;

		public const short ProfessionSkillEffectIsEnd = 97;

		public const short ProfessionHunterSkill0 = 98;

		public const short ProfessionMartialArtistSkill2 = 99;

		public const short ProfessionLiteratiSkill2 = 100;

		public const short ProfessionCivilianSkill1 = 101;

		public const short ProfessionCivilianSkill2 = 102;

		public const short ProfessionDoctorSkill1 = 103;

		public const short ProfessionMonkBreakFoodRule = 104;

		public const short ProfessionMonkBreakLoveRule = 105;

		public const short ProfessionHunterSkill0None = 106;

		public const short SettlementStoryGoodEnd = 107;

		public const short SettlementStoryBadEnd = 108;

		public const short ReadInCombat = 109;

		public const short ReadInLifeSkillCombat = 110;

		public const short ReadInCombatNoChance = 111;

		public const short ReadInLifeSkillCombatNoChance = 112;

		public const short BookRepairSuccess = 113;

		public const short ReincarnationArchitectureReincarnationEnd = 114;

		public const short TheNestOfRegulationDies = 115;

		public const short XuannvBlockMusicTranscribe = 116;

		public const short XuannvStateMusicTranscribe = 117;

		public const short DuChuangYiGeReady = 118;

		public const short CultureDecline = 119;

		public const short ThunderPowerGrow = 120;

		public const short FloodPowerGrow = 121;

		public const short BlazePowerGrow = 122;

		public const short StormPowerGrow = 123;

		public const short SandPowerGrow = 124;

		public const short ThunderPowerDecline = 125;

		public const short FloodPowerDecline = 126;

		public const short BlazePowerDecline = 127;

		public const short StormPowerDecline = 128;

		public const short SandPowerDecline = 129;

		public const short JiaoAbilityUp = 130;

		public const short JiaoAbilityDown = 131;

		public const short JiaoGiftAbilityUp = 132;

		public const short JiaoGiftAbilityDown = 133;

		public const short JiaoAbilityUpPercent = 134;

		public const short JiaoAbilityDownPercent = 135;

		public const short JiaoAbilityUpFloat = 136;

		public const short JiaoAbilityDownFloat = 137;

		public const short WugKingEscape = 138;

		public const short WugKingParasitiferDead = 139;

		public const short WugKingDead = 140;

		public const short WugKingDeadSpecial = 141;

		public const short KnowMonkSecret = 142;

		public const short GraceIncreased = 143;

		public const short WugKingEscape1 = 144;

		public const short WugKingEscape2 = 145;

		public const short QiArtInCombatNoChance = 146;

		public const short QiArtInLifeSkillCombatNoChance = 147;

		public const short SectStoryBaihuaToAnimal = 148;

		public const short SectStoryBaihuaToHuman = 149;

		public const short MechanismOfDetonation = 150;

		public const short ProfessionSeniorityIncrease1 = 151;

		public const short BlockResourceRecovery = 152;

		public const short ShenTreeGrow = 153;

		public const short BeastUpgrade = 154;

		public const short BeastDowngrade = 155;

		public const short GatherCompanions = 156;

		public const short DisseminateSecretInformation = 157;

		public const short DisseminateInformation = 158;

		public const short RecommendFellowUp = 159;

		public const short RecommendFellowDown = 160;

		public const short ComradePropertyUp = 161;

		public const short ComradeCombatSkillUp = 162;

		public const short ComradeLifeSkillUp = 163;

		public const short ComradeFeatureUp = 164;

		public const short ReleasePrisoners = 165;

		public const short DriveAwayPeople = 166;

		public const short QuenchHatred = 167;

		public const short VisitTemple = 168;

		public const short DrinkTeaRecharge = 169;

		public const short ReleaseSouls = 170;

		public const short SectPunishmentWarrantRelieved = 171;

		public const short SectPunishmentCharacterFeatureRelieved = 172;

		public const short ResignationPosition = 173;

		public const short Legacy = 174;

		public const short CultureUp = 175;

		public const short SecurityUp = 176;

		public const short CultureDown = 177;

		public const short SecurityDown = 178;

		public const short MapPickupsResource = 179;

		public const short MapPickupsFoodIngredients = 180;

		public const short MapPickupsMaterials = 181;

		public const short MapPickupsHerbal0 = 182;

		public const short MapPickupsHerbal1 = 183;

		public const short MapPickupsPoison = 184;

		public const short MapPickupsInjuryMedicine = 185;

		public const short MapPickupsAntidote = 186;

		public const short MapPickupsGainMedicine = 187;

		public const short MapPickupsFruit = 188;

		public const short MapPickupsChickenDishes = 189;

		public const short MapPickupsMeatDishes = 190;

		public const short MapPickupsVegetarianDishes = 191;

		public const short MapPickupsSeafoodDishes = 192;

		public const short MapPickupsWine = 193;

		public const short MapPickupsTea = 194;

		public const short MapPickupsTool = 195;

		public const short MapPickupsAccessory = 196;

		public const short MapPickupsPoisonCream = 197;

		public const short MapPickupsHarrier = 198;

		public const short MapPickupsToken = 199;

		public const short MapPickupsNeedleBox = 200;

		public const short MapPickupsThorn = 201;

		public const short MapPickupsHiddenWeapon = 202;

		public const short MapPickupsFlute = 203;

		public const short MapPickupsGloves = 204;

		public const short MapPickupsFurGloves = 205;

		public const short MapPickupsPestle = 206;

		public const short MapPickupsSword = 207;

		public const short MapPickupsBlade = 208;

		public const short MapPickupsPolearm = 209;

		public const short MapPickupQin = 210;

		public const short MapPickupsWhisk = 211;

		public const short MapPickupsWhip = 212;

		public const short MapPickupsCrest = 213;

		public const short MapPickupsShoes = 214;

		public const short MapPickupsArmor = 215;

		public const short MapPickupsArmGuard = 216;

		public const short MapPickupsCarDrop = 217;

		public const short MapPickupsExp = 218;

		public const short MapPickupsReading = 219;

		public const short MapPickupsQiArt = 220;

		public const short MapPickupsMorale = 221;

		public const short MapPickupsProperty = 222;

		public const short MapPickupsEnemyEscape = 223;

		public const short BuildingExp = 224;

		public const short WalkThroughDestroyBlock = 225;

		public const short WalkThroughErosionBlock = 226;

		public const short ComradePropertyUpNew = 227;

		public const short ComradeCombatSkillUpNew = 228;

		public const short ComradeLifeSkillUpNew = 229;

		public const short ComradePropertyUpNew1 = 230;

		public const short ComradeCombatSkillUpNew1 = 231;

		public const short ComradeLifeSkillUpNew1 = 232;

		public const short CharacterEscape = 233;

		public const short GraceUp = 234;

		public const short GraceDown = 235;

		public const short ExpelEnemy = 236;

		public const short ExpelRighteous = 237;

		public const short ExpelXiangshuMinion = 238;

		public const short ExpelBeast = 239;

		public const short MapPickupsPoisonCorrected = 240;

		public const short MapPickupsInjuryMedicineCorrected = 241;

		public const short MapPickupsAntidoteCorrected = 242;

		public const short MapPickupsGainMedicineCorrected = 243;

		public const short MapPickupsResourceUpdate = 244;

		public const short MapPickupsExpUpdate = 245;

		public const short MapPickupsMoraleUpdate = 246;

		public const short MapPickupsItemUpdate = 247;

		public const short MapPickupsReadingUpdate = 248;

		public const short MapPickupsQiArtUpdate = 249;

		public const short MakeItemOutsideSettlement = 250;

		public const short GainFuyuFaith1 = 251;

		public const short GainFuyuFaith2 = 252;

		public const short GainFuyuFaith3 = 253;

		public const short MapPickupsMedicineUpdate = 254;
	}

	public static class DefValue
	{
		public static InstantNotificationItem BuildingUpgradingCompleted => Instance[(short)0];

		public static InstantNotificationItem BuildingDemolitionCompleted => Instance[(short)1];

		public static InstantNotificationItem BuildingCraftingCompleted => Instance[(short)2];

		public static InstantNotificationItem BuildingConstructionCompleted => Instance[(short)3];

		public static InstantNotificationItem BuildingProductGenerated => Instance[(short)4];

		public static InstantNotificationItem BuildingDamaged => Instance[(short)5];

		public static InstantNotificationItem BuildingRuined => Instance[(short)6];

		public static InstantNotificationItem BeginBuildingConstruction => Instance[(short)7];

		public static InstantNotificationItem BeginBuildingUpgrading => Instance[(short)8];

		public static InstantNotificationItem BeginBuildingDemolition => Instance[(short)9];

		public static InstantNotificationItem CancelBuildingDemolition => Instance[(short)10];

		public static InstantNotificationItem CandidateArrived => Instance[(short)11];

		public static InstantNotificationItem CandidateLeaved => Instance[(short)12];

		public static InstantNotificationItem JoinTaiwuVillage => Instance[(short)13];

		public static InstantNotificationItem LeaveTaiwuVillage => Instance[(short)14];

		public static InstantNotificationItem WarehouseItemLost => Instance[(short)15];

		public static InstantNotificationItem BuildingLoseAuthority => Instance[(short)16];

		public static InstantNotificationItem DiscoverRelay => Instance[(short)17];

		public static InstantNotificationItem WalkThroughAbyss => Instance[(short)18];

		public static InstantNotificationItem BeginAdventure => Instance[(short)19];

		public static InstantNotificationItem NaturalDisasterEncountered => Instance[(short)20];

		public static InstantNotificationItem JoinGroup => Instance[(short)21];

		public static InstantNotificationItem LeaveGroup => Instance[(short)22];

		public static InstantNotificationItem BehaviorTypeChanged => Instance[(short)23];

		public static InstantNotificationItem InheritedApprovingRateReceived => Instance[(short)24];

		public static InstantNotificationItem FameIncreased => Instance[(short)25];

		public static InstantNotificationItem FameDecreased => Instance[(short)26];

		public static InstantNotificationItem HappinessIncreased => Instance[(short)27];

		public static InstantNotificationItem HappinessDecreased => Instance[(short)28];

		public static InstantNotificationItem FavorabilityIncreased => Instance[(short)29];

		public static InstantNotificationItem FavorabilityDecreased => Instance[(short)30];

		public static InstantNotificationItem FavorabilityIncreasedAcrossLevels => Instance[(short)31];

		public static InstantNotificationItem FavorabilityDecreasedAcrossLevels => Instance[(short)32];

		public static InstantNotificationItem LovingItemRevealed => Instance[(short)33];

		public static InstantNotificationItem HatingItemRevealed => Instance[(short)34];

		public static InstantNotificationItem LovingItemRevealedNothing => Instance[(short)35];

		public static InstantNotificationItem HatingItemRevealedNothing => Instance[(short)36];

		public static InstantNotificationItem EatBloodDew => Instance[(short)37];

		public static InstantNotificationItem CombatSkillLearned => Instance[(short)38];

		public static InstantNotificationItem HealthIncreased => Instance[(short)39];

		public static InstantNotificationItem HealthDecreased => Instance[(short)40];

		public static InstantNotificationItem XiangshuInfectionIncreased => Instance[(short)41];

		public static InstantNotificationItem XiangshuInfectionDecreased => Instance[(short)42];

		public static InstantNotificationItem XiangshuPartlyInfected => Instance[(short)43];

		public static InstantNotificationItem XiangshuCompletelyInfected => Instance[(short)44];

		public static InstantNotificationItem MainAttributeRecovered => Instance[(short)45];

		public static InstantNotificationItem MainAttributeConsumed => Instance[(short)46];

		public static InstantNotificationItem DisorderOfQiIncreased => Instance[(short)47];

		public static InstantNotificationItem DisorderOfQiDecreased => Instance[(short)48];

		public static InstantNotificationItem InjuryIncreased => Instance[(short)49];

		public static InstantNotificationItem InjuryDecreased => Instance[(short)50];

		public static InstantNotificationItem PoisonIncreased => Instance[(short)51];

		public static InstantNotificationItem PoisonDecreased => Instance[(short)52];

		public static InstantNotificationItem ExpIncreased => Instance[(short)53];

		public static InstantNotificationItem ExpDecreased => Instance[(short)54];

		public static InstantNotificationItem ResourceIncreased => Instance[(short)55];

		public static InstantNotificationItem ResourceDecreased => Instance[(short)56];

		public static InstantNotificationItem GetItem => Instance[(short)57];

		public static InstantNotificationItem LoseItem => Instance[(short)58];

		public static InstantNotificationItem CharacterGrownUp => Instance[(short)59];

		public static InstantNotificationItem CharacterDead => Instance[(short)60];

		public static InstantNotificationItem CricketDead => Instance[(short)61];

		public static InstantNotificationItem FamilyDied => Instance[(short)62];

		public static InstantNotificationItem EnemyDied => Instance[(short)63];

		public static InstantNotificationItem EnemyLucky => Instance[(short)64];

		public static InstantNotificationItem EnemyUnlucky => Instance[(short)65];

		public static InstantNotificationItem EnemyLoseInLifeSkill => Instance[(short)66];

		public static InstantNotificationItem EnemyLoseInCombat => Instance[(short)67];

		public static InstantNotificationItem EnemyGreatLoseInCombat => Instance[(short)68];

		public static InstantNotificationItem FamilyMiscarriage => Instance[(short)69];

		public static InstantNotificationItem AbandonExposed => Instance[(short)70];

		public static InstantNotificationItem AbandonAcknowledged => Instance[(short)71];

		public static InstantNotificationItem FamilyAbandon => Instance[(short)72];

		public static InstantNotificationItem SelfImmoralLove => Instance[(short)73];

		public static InstantNotificationItem FamilyHaveKid => Instance[(short)74];

		public static InstantNotificationItem HaveKid => Instance[(short)75];

		public static InstantNotificationItem FamilyImmoralKid => Instance[(short)76];

		public static InstantNotificationItem ReligiousFamilyHaveKid => Instance[(short)77];

		public static InstantNotificationItem FamilyInLove => Instance[(short)78];

		public static InstantNotificationItem FamilyImmoralLove => Instance[(short)79];

		public static InstantNotificationItem ReligiousFamilyInLove => Instance[(short)80];

		public static InstantNotificationItem FamilyMarried => Instance[(short)81];

		public static InstantNotificationItem FamilyImmoralMarriage => Instance[(short)82];

		public static InstantNotificationItem FamilyBeFriendWithEnemy => Instance[(short)83];

		public static InstantNotificationItem FamilyJieyiWithEnemy => Instance[(short)84];

		public static InstantNotificationItem FamilyAdoptedMaleEnemy => Instance[(short)85];

		public static InstantNotificationItem FamilyAdoptedFemaleEnemy => Instance[(short)86];

		public static InstantNotificationItem FamilyAdoptedByMaleEnemy => Instance[(short)87];

		public static InstantNotificationItem FamilyAdoptedByFemaleEnemy => Instance[(short)88];

		public static InstantNotificationItem FamilyEndedImmoralLove => Instance[(short)89];

		public static InstantNotificationItem FamilyEndedFriendshipWithEnemy => Instance[(short)90];

		public static InstantNotificationItem FamilyEndedJieyiWithEnemy => Instance[(short)91];

		public static InstantNotificationItem LoverHaveSex => Instance[(short)92];

		public static InstantNotificationItem TaiwuVillageIdleCount => Instance[(short)93];

		public static InstantNotificationItem ProfessionSeniorityIncrease => Instance[(short)94];

		public static InstantNotificationItem ProfessionUnlockSkill => Instance[(short)95];

		public static InstantNotificationItem ProfessionSkillHasCoolDown => Instance[(short)96];

		public static InstantNotificationItem ProfessionSkillEffectIsEnd => Instance[(short)97];

		public static InstantNotificationItem ProfessionHunterSkill0 => Instance[(short)98];

		public static InstantNotificationItem ProfessionMartialArtistSkill2 => Instance[(short)99];

		public static InstantNotificationItem ProfessionLiteratiSkill2 => Instance[(short)100];

		public static InstantNotificationItem ProfessionCivilianSkill1 => Instance[(short)101];

		public static InstantNotificationItem ProfessionCivilianSkill2 => Instance[(short)102];

		public static InstantNotificationItem ProfessionDoctorSkill1 => Instance[(short)103];

		public static InstantNotificationItem ProfessionMonkBreakFoodRule => Instance[(short)104];

		public static InstantNotificationItem ProfessionMonkBreakLoveRule => Instance[(short)105];

		public static InstantNotificationItem ProfessionHunterSkill0None => Instance[(short)106];

		public static InstantNotificationItem SettlementStoryGoodEnd => Instance[(short)107];

		public static InstantNotificationItem SettlementStoryBadEnd => Instance[(short)108];

		public static InstantNotificationItem ReadInCombat => Instance[(short)109];

		public static InstantNotificationItem ReadInLifeSkillCombat => Instance[(short)110];

		public static InstantNotificationItem ReadInCombatNoChance => Instance[(short)111];

		public static InstantNotificationItem ReadInLifeSkillCombatNoChance => Instance[(short)112];

		public static InstantNotificationItem BookRepairSuccess => Instance[(short)113];

		public static InstantNotificationItem ReincarnationArchitectureReincarnationEnd => Instance[(short)114];

		public static InstantNotificationItem TheNestOfRegulationDies => Instance[(short)115];

		public static InstantNotificationItem XuannvBlockMusicTranscribe => Instance[(short)116];

		public static InstantNotificationItem XuannvStateMusicTranscribe => Instance[(short)117];

		public static InstantNotificationItem DuChuangYiGeReady => Instance[(short)118];

		public static InstantNotificationItem CultureDecline => Instance[(short)119];

		public static InstantNotificationItem ThunderPowerGrow => Instance[(short)120];

		public static InstantNotificationItem FloodPowerGrow => Instance[(short)121];

		public static InstantNotificationItem BlazePowerGrow => Instance[(short)122];

		public static InstantNotificationItem StormPowerGrow => Instance[(short)123];

		public static InstantNotificationItem SandPowerGrow => Instance[(short)124];

		public static InstantNotificationItem ThunderPowerDecline => Instance[(short)125];

		public static InstantNotificationItem FloodPowerDecline => Instance[(short)126];

		public static InstantNotificationItem BlazePowerDecline => Instance[(short)127];

		public static InstantNotificationItem StormPowerDecline => Instance[(short)128];

		public static InstantNotificationItem SandPowerDecline => Instance[(short)129];

		public static InstantNotificationItem JiaoAbilityUp => Instance[(short)130];

		public static InstantNotificationItem JiaoAbilityDown => Instance[(short)131];

		public static InstantNotificationItem JiaoGiftAbilityUp => Instance[(short)132];

		public static InstantNotificationItem JiaoGiftAbilityDown => Instance[(short)133];

		public static InstantNotificationItem JiaoAbilityUpPercent => Instance[(short)134];

		public static InstantNotificationItem JiaoAbilityDownPercent => Instance[(short)135];

		public static InstantNotificationItem JiaoAbilityUpFloat => Instance[(short)136];

		public static InstantNotificationItem JiaoAbilityDownFloat => Instance[(short)137];

		public static InstantNotificationItem WugKingEscape => Instance[(short)138];

		public static InstantNotificationItem WugKingParasitiferDead => Instance[(short)139];

		public static InstantNotificationItem WugKingDead => Instance[(short)140];

		public static InstantNotificationItem WugKingDeadSpecial => Instance[(short)141];

		public static InstantNotificationItem KnowMonkSecret => Instance[(short)142];

		public static InstantNotificationItem GraceIncreased => Instance[(short)143];

		public static InstantNotificationItem WugKingEscape1 => Instance[(short)144];

		public static InstantNotificationItem WugKingEscape2 => Instance[(short)145];

		public static InstantNotificationItem QiArtInCombatNoChance => Instance[(short)146];

		public static InstantNotificationItem QiArtInLifeSkillCombatNoChance => Instance[(short)147];

		public static InstantNotificationItem SectStoryBaihuaToAnimal => Instance[(short)148];

		public static InstantNotificationItem SectStoryBaihuaToHuman => Instance[(short)149];

		public static InstantNotificationItem MechanismOfDetonation => Instance[(short)150];

		public static InstantNotificationItem ProfessionSeniorityIncrease1 => Instance[(short)151];

		public static InstantNotificationItem BlockResourceRecovery => Instance[(short)152];

		public static InstantNotificationItem ShenTreeGrow => Instance[(short)153];

		public static InstantNotificationItem BeastUpgrade => Instance[(short)154];

		public static InstantNotificationItem BeastDowngrade => Instance[(short)155];

		public static InstantNotificationItem GatherCompanions => Instance[(short)156];

		public static InstantNotificationItem DisseminateSecretInformation => Instance[(short)157];

		public static InstantNotificationItem DisseminateInformation => Instance[(short)158];

		public static InstantNotificationItem RecommendFellowUp => Instance[(short)159];

		public static InstantNotificationItem RecommendFellowDown => Instance[(short)160];

		public static InstantNotificationItem ComradePropertyUp => Instance[(short)161];

		public static InstantNotificationItem ComradeCombatSkillUp => Instance[(short)162];

		public static InstantNotificationItem ComradeLifeSkillUp => Instance[(short)163];

		public static InstantNotificationItem ComradeFeatureUp => Instance[(short)164];

		public static InstantNotificationItem ReleasePrisoners => Instance[(short)165];

		public static InstantNotificationItem DriveAwayPeople => Instance[(short)166];

		public static InstantNotificationItem QuenchHatred => Instance[(short)167];

		public static InstantNotificationItem VisitTemple => Instance[(short)168];

		public static InstantNotificationItem DrinkTeaRecharge => Instance[(short)169];

		public static InstantNotificationItem ReleaseSouls => Instance[(short)170];

		public static InstantNotificationItem SectPunishmentWarrantRelieved => Instance[(short)171];

		public static InstantNotificationItem SectPunishmentCharacterFeatureRelieved => Instance[(short)172];

		public static InstantNotificationItem ResignationPosition => Instance[(short)173];

		public static InstantNotificationItem Legacy => Instance[(short)174];

		public static InstantNotificationItem CultureUp => Instance[(short)175];

		public static InstantNotificationItem SecurityUp => Instance[(short)176];

		public static InstantNotificationItem CultureDown => Instance[(short)177];

		public static InstantNotificationItem SecurityDown => Instance[(short)178];

		public static InstantNotificationItem MapPickupsResource => Instance[(short)179];

		public static InstantNotificationItem MapPickupsFoodIngredients => Instance[(short)180];

		public static InstantNotificationItem MapPickupsMaterials => Instance[(short)181];

		public static InstantNotificationItem MapPickupsHerbal0 => Instance[(short)182];

		public static InstantNotificationItem MapPickupsHerbal1 => Instance[(short)183];

		public static InstantNotificationItem MapPickupsPoison => Instance[(short)184];

		public static InstantNotificationItem MapPickupsInjuryMedicine => Instance[(short)185];

		public static InstantNotificationItem MapPickupsAntidote => Instance[(short)186];

		public static InstantNotificationItem MapPickupsGainMedicine => Instance[(short)187];

		public static InstantNotificationItem MapPickupsFruit => Instance[(short)188];

		public static InstantNotificationItem MapPickupsChickenDishes => Instance[(short)189];

		public static InstantNotificationItem MapPickupsMeatDishes => Instance[(short)190];

		public static InstantNotificationItem MapPickupsVegetarianDishes => Instance[(short)191];

		public static InstantNotificationItem MapPickupsSeafoodDishes => Instance[(short)192];

		public static InstantNotificationItem MapPickupsWine => Instance[(short)193];

		public static InstantNotificationItem MapPickupsTea => Instance[(short)194];

		public static InstantNotificationItem MapPickupsTool => Instance[(short)195];

		public static InstantNotificationItem MapPickupsAccessory => Instance[(short)196];

		public static InstantNotificationItem MapPickupsPoisonCream => Instance[(short)197];

		public static InstantNotificationItem MapPickupsHarrier => Instance[(short)198];

		public static InstantNotificationItem MapPickupsToken => Instance[(short)199];

		public static InstantNotificationItem MapPickupsNeedleBox => Instance[(short)200];

		public static InstantNotificationItem MapPickupsThorn => Instance[(short)201];

		public static InstantNotificationItem MapPickupsHiddenWeapon => Instance[(short)202];

		public static InstantNotificationItem MapPickupsFlute => Instance[(short)203];

		public static InstantNotificationItem MapPickupsGloves => Instance[(short)204];

		public static InstantNotificationItem MapPickupsFurGloves => Instance[(short)205];

		public static InstantNotificationItem MapPickupsPestle => Instance[(short)206];

		public static InstantNotificationItem MapPickupsSword => Instance[(short)207];

		public static InstantNotificationItem MapPickupsBlade => Instance[(short)208];

		public static InstantNotificationItem MapPickupsPolearm => Instance[(short)209];

		public static InstantNotificationItem MapPickupQin => Instance[(short)210];

		public static InstantNotificationItem MapPickupsWhisk => Instance[(short)211];

		public static InstantNotificationItem MapPickupsWhip => Instance[(short)212];

		public static InstantNotificationItem MapPickupsCrest => Instance[(short)213];

		public static InstantNotificationItem MapPickupsShoes => Instance[(short)214];

		public static InstantNotificationItem MapPickupsArmor => Instance[(short)215];

		public static InstantNotificationItem MapPickupsArmGuard => Instance[(short)216];

		public static InstantNotificationItem MapPickupsCarDrop => Instance[(short)217];

		public static InstantNotificationItem MapPickupsExp => Instance[(short)218];

		public static InstantNotificationItem MapPickupsReading => Instance[(short)219];

		public static InstantNotificationItem MapPickupsQiArt => Instance[(short)220];

		public static InstantNotificationItem MapPickupsMorale => Instance[(short)221];

		public static InstantNotificationItem MapPickupsProperty => Instance[(short)222];

		public static InstantNotificationItem MapPickupsEnemyEscape => Instance[(short)223];

		public static InstantNotificationItem BuildingExp => Instance[(short)224];

		public static InstantNotificationItem WalkThroughDestroyBlock => Instance[(short)225];

		public static InstantNotificationItem WalkThroughErosionBlock => Instance[(short)226];

		public static InstantNotificationItem ComradePropertyUpNew => Instance[(short)227];

		public static InstantNotificationItem ComradeCombatSkillUpNew => Instance[(short)228];

		public static InstantNotificationItem ComradeLifeSkillUpNew => Instance[(short)229];

		public static InstantNotificationItem ComradePropertyUpNew1 => Instance[(short)230];

		public static InstantNotificationItem ComradeCombatSkillUpNew1 => Instance[(short)231];

		public static InstantNotificationItem ComradeLifeSkillUpNew1 => Instance[(short)232];

		public static InstantNotificationItem CharacterEscape => Instance[(short)233];

		public static InstantNotificationItem GraceUp => Instance[(short)234];

		public static InstantNotificationItem GraceDown => Instance[(short)235];

		public static InstantNotificationItem ExpelEnemy => Instance[(short)236];

		public static InstantNotificationItem ExpelRighteous => Instance[(short)237];

		public static InstantNotificationItem ExpelXiangshuMinion => Instance[(short)238];

		public static InstantNotificationItem ExpelBeast => Instance[(short)239];

		public static InstantNotificationItem MapPickupsPoisonCorrected => Instance[(short)240];

		public static InstantNotificationItem MapPickupsInjuryMedicineCorrected => Instance[(short)241];

		public static InstantNotificationItem MapPickupsAntidoteCorrected => Instance[(short)242];

		public static InstantNotificationItem MapPickupsGainMedicineCorrected => Instance[(short)243];

		public static InstantNotificationItem MapPickupsResourceUpdate => Instance[(short)244];

		public static InstantNotificationItem MapPickupsExpUpdate => Instance[(short)245];

		public static InstantNotificationItem MapPickupsMoraleUpdate => Instance[(short)246];

		public static InstantNotificationItem MapPickupsItemUpdate => Instance[(short)247];

		public static InstantNotificationItem MapPickupsReadingUpdate => Instance[(short)248];

		public static InstantNotificationItem MapPickupsQiArtUpdate => Instance[(short)249];

		public static InstantNotificationItem MakeItemOutsideSettlement => Instance[(short)250];

		public static InstantNotificationItem GainFuyuFaith1 => Instance[(short)251];

		public static InstantNotificationItem GainFuyuFaith2 => Instance[(short)252];

		public static InstantNotificationItem GainFuyuFaith3 => Instance[(short)253];

		public static InstantNotificationItem MapPickupsMedicineUpdate => Instance[(short)254];
	}

	public static InstantNotification Instance = new InstantNotification();

	private readonly HashSet<string> RequiredFields = new HashSet<string> { "TemplateId", "Type", "Name", "SimpleDesc", "Desc", "MergeableParameters" };

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
		_dataArray.Add(new InstantNotificationItem(0, EInstantNotificationType.TaiwuVillage, 0, 2, 1, 2, new string[4] { "Settlement", "Building", "", "" }, new List<sbyte> { 1 }));
		_dataArray.Add(new InstantNotificationItem(1, EInstantNotificationType.TaiwuVillage, 3, 2, 4, 5, new string[4] { "Settlement", "Building", "", "" }, new List<sbyte> { 1 }));
		_dataArray.Add(new InstantNotificationItem(2, EInstantNotificationType.TaiwuVillage, 6, 1, 7, 8, new string[4] { "Settlement", "Building", "", "" }, new List<sbyte> { 1 }));
		_dataArray.Add(new InstantNotificationItem(3, EInstantNotificationType.TaiwuVillage, 9, 2, 10, 11, new string[4] { "Settlement", "Building", "", "" }, new List<sbyte> { 1 }));
		_dataArray.Add(new InstantNotificationItem(4, EInstantNotificationType.TaiwuVillage, 12, 1, 13, 14, new string[4] { "Settlement", "Building", "", "" }, new List<sbyte> { 1 }));
		_dataArray.Add(new InstantNotificationItem(5, EInstantNotificationType.TaiwuVillage, 15, 1, 16, 17, new string[4] { "Settlement", "Building", "", "" }, new List<sbyte> { 1 }));
		_dataArray.Add(new InstantNotificationItem(6, EInstantNotificationType.TaiwuVillage, 18, 2, 19, 20, new string[4] { "Settlement", "Building", "", "" }, new List<sbyte> { 1 }));
		_dataArray.Add(new InstantNotificationItem(7, EInstantNotificationType.TaiwuVillage, 21, 2, 22, 22, new string[4] { "Settlement", "Building", "", "" }, null));
		_dataArray.Add(new InstantNotificationItem(8, EInstantNotificationType.TaiwuVillage, 23, 2, 24, 24, new string[4] { "Settlement", "Building", "", "" }, null));
		_dataArray.Add(new InstantNotificationItem(9, EInstantNotificationType.TaiwuVillage, 25, 2, 26, 26, new string[4] { "Settlement", "Building", "", "" }, null));
		_dataArray.Add(new InstantNotificationItem(10, EInstantNotificationType.TaiwuVillage, 27, 2, 28, 29, new string[4] { "Settlement", "Building", "", "" }, null));
		_dataArray.Add(new InstantNotificationItem(11, EInstantNotificationType.TaiwuVillage, 30, 1, 31, 32, new string[4] { "Settlement", "Building", "", "" }, null));
		_dataArray.Add(new InstantNotificationItem(12, EInstantNotificationType.TaiwuVillage, 33, 1, 34, 35, new string[4] { "Settlement", "Building", "", "" }, null));
		_dataArray.Add(new InstantNotificationItem(13, EInstantNotificationType.TaiwuVillage, 36, 0, 37, 37, new string[4] { "Character", "", "", "" }, new List<sbyte> { 0 }));
		_dataArray.Add(new InstantNotificationItem(14, EInstantNotificationType.TaiwuVillage, 38, 0, 39, 39, new string[4] { "Character", "", "", "" }, new List<sbyte> { 0 }));
		_dataArray.Add(new InstantNotificationItem(15, EInstantNotificationType.TaiwuVillage, 40, 1, 41, 42, new string[4] { "Item", "", "", "" }, new List<sbyte> { 0 }));
		_dataArray.Add(new InstantNotificationItem(16, EInstantNotificationType.TaiwuVillage, 43, 2, 44, 45, new string[4] { "Settlement", "Building", "", "" }, new List<sbyte> { 1 }));
		_dataArray.Add(new InstantNotificationItem(17, EInstantNotificationType.Team, 46, 2, 47, 48, new string[4] { "Location", "Settlement", "", "" }, null));
		_dataArray.Add(new InstantNotificationItem(18, EInstantNotificationType.Team, 49, 1, 50, 51, new string[4] { "", "", "", "" }, null));
		_dataArray.Add(new InstantNotificationItem(19, EInstantNotificationType.Team, 52, 2, 53, 54, new string[4] { "Location", "Adventure", "", "" }, null));
		_dataArray.Add(new InstantNotificationItem(20, EInstantNotificationType.Team, 55, 1, 56, 57, new string[4] { "Location", "", "", "" }, null));
		_dataArray.Add(new InstantNotificationItem(21, EInstantNotificationType.Team, 58, 0, 59, 59, new string[4] { "Character", "", "", "" }, new List<sbyte> { 0 }));
		_dataArray.Add(new InstantNotificationItem(22, EInstantNotificationType.Team, 60, 0, 61, 61, new string[4] { "Character", "", "", "" }, new List<sbyte> { 0 }));
		_dataArray.Add(new InstantNotificationItem(23, EInstantNotificationType.Property, 62, 0, 63, 64, new string[4] { "Character", "BehaviorType", "", "" }, null));
		_dataArray.Add(new InstantNotificationItem(24, EInstantNotificationType.Society, 65, 0, 66, 67, new string[4] { "Settlement", "Character", "", "" }, new List<sbyte> { 1 }));
		_dataArray.Add(new InstantNotificationItem(25, EInstantNotificationType.Society, 68, 0, 69, 69, new string[4] { "Character", "", "", "" }, new List<sbyte> { 0 }));
		_dataArray.Add(new InstantNotificationItem(26, EInstantNotificationType.Society, 70, 0, 71, 72, new string[4] { "Character", "", "", "" }, new List<sbyte> { 0 }));
		_dataArray.Add(new InstantNotificationItem(27, EInstantNotificationType.DuringMonth, 73, 0, 74, 74, new string[4] { "Character", "", "", "" }, new List<sbyte> { 0 }));
		_dataArray.Add(new InstantNotificationItem(28, EInstantNotificationType.DuringMonth, 75, 0, 76, 77, new string[4] { "Character", "", "", "" }, new List<sbyte> { 0 }));
		_dataArray.Add(new InstantNotificationItem(29, EInstantNotificationType.Society, 78, 0, 79, 80, new string[4] { "Settlement", "Character", "Character", "" }, new List<sbyte> { 1 }));
		_dataArray.Add(new InstantNotificationItem(30, EInstantNotificationType.Society, 81, 0, 82, 83, new string[4] { "Settlement", "Character", "Character", "" }, new List<sbyte> { 1 }));
		_dataArray.Add(new InstantNotificationItem(31, EInstantNotificationType.Society, 84, 0, 85, 86, new string[4] { "Character", "Character", "FavorabilityType", "" }, null));
		_dataArray.Add(new InstantNotificationItem(32, EInstantNotificationType.Society, 87, 0, 88, 89, new string[4] { "Character", "Character", "FavorabilityType", "" }, null));
		_dataArray.Add(new InstantNotificationItem(33, EInstantNotificationType.Society, 90, 0, 91, 92, new string[4] { "Settlement", "Character", "ItemSubType", "" }, null));
		_dataArray.Add(new InstantNotificationItem(34, EInstantNotificationType.Society, 93, 0, 94, 95, new string[4] { "Settlement", "Character", "ItemSubType", "" }, null));
		_dataArray.Add(new InstantNotificationItem(35, EInstantNotificationType.Society, 96, 0, 97, 98, new string[4] { "Settlement", "Character", "", "" }, null));
		_dataArray.Add(new InstantNotificationItem(36, EInstantNotificationType.Society, 99, 0, 100, 101, new string[4] { "Settlement", "Character", "", "" }, null));
		_dataArray.Add(new InstantNotificationItem(37, EInstantNotificationType.Property, 102, 0, 103, 104, new string[4] { "Character", "Item", "Integer", "" }, null));
		_dataArray.Add(new InstantNotificationItem(38, EInstantNotificationType.Property, 105, 0, 106, 107, new string[4] { "Character", "CombatSkill", "", "" }, null));
		_dataArray.Add(new InstantNotificationItem(39, EInstantNotificationType.Property, 108, 0, 109, 110, new string[4] { "Character", "", "", "" }, new List<sbyte> { 0 }));
		_dataArray.Add(new InstantNotificationItem(40, EInstantNotificationType.Property, 111, 0, 112, 113, new string[4] { "Character", "", "", "" }, new List<sbyte> { 0 }));
		_dataArray.Add(new InstantNotificationItem(41, EInstantNotificationType.DuringMonth, 114, 0, 115, 116, new string[4] { "Character", "", "", "" }, null));
		_dataArray.Add(new InstantNotificationItem(42, EInstantNotificationType.DuringMonth, 117, 0, 118, 119, new string[4] { "Character", "", "", "" }, null));
		_dataArray.Add(new InstantNotificationItem(43, EInstantNotificationType.DuringMonth, 120, 0, 121, 122, new string[4] { "Character", "", "", "" }, new List<sbyte> { 0 }));
		_dataArray.Add(new InstantNotificationItem(44, EInstantNotificationType.DuringMonth, 123, 0, 124, 125, new string[4] { "Character", "", "", "" }, new List<sbyte> { 0 }));
		_dataArray.Add(new InstantNotificationItem(45, EInstantNotificationType.Property, 126, 0, 127, 128, new string[4] { "Character", "CharacterPropertyReferencedType", "Integer", "" }, null));
		_dataArray.Add(new InstantNotificationItem(46, EInstantNotificationType.Property, 129, 0, 130, 131, new string[4] { "Character", "CharacterPropertyReferencedType", "Integer", "" }, null));
		_dataArray.Add(new InstantNotificationItem(47, EInstantNotificationType.Property, 132, 0, 133, 134, new string[4] { "Character", "Integer", "", "" }, null));
		_dataArray.Add(new InstantNotificationItem(48, EInstantNotificationType.Property, 135, 0, 136, 137, new string[4] { "Character", "Integer", "", "" }, null));
		_dataArray.Add(new InstantNotificationItem(49, EInstantNotificationType.Property, 138, 0, 139, 140, new string[4] { "Character", "BodyPartType", "InjuryType", "" }, null));
		_dataArray.Add(new InstantNotificationItem(50, EInstantNotificationType.Property, 141, 0, 142, 143, new string[4] { "Character", "BodyPartType", "InjuryType", "" }, null));
		_dataArray.Add(new InstantNotificationItem(51, EInstantNotificationType.Property, 144, 0, 145, 146, new string[4] { "Character", "PoisonType", "Integer", "" }, null));
		_dataArray.Add(new InstantNotificationItem(52, EInstantNotificationType.Property, 147, 0, 148, 149, new string[4] { "Character", "PoisonType", "Integer", "" }, null));
		_dataArray.Add(new InstantNotificationItem(53, EInstantNotificationType.Property, 150, 0, 151, 152, new string[4] { "Character", "Integer", "", "" }, null));
		_dataArray.Add(new InstantNotificationItem(54, EInstantNotificationType.Property, 153, 0, 154, 155, new string[4] { "Character", "Integer", "", "" }, null));
		_dataArray.Add(new InstantNotificationItem(55, EInstantNotificationType.Item, 156, 0, 157, 158, new string[4] { "Character", "Resource", "Integer", "" }, null));
		_dataArray.Add(new InstantNotificationItem(56, EInstantNotificationType.Item, 159, 0, 160, 161, new string[4] { "Character", "Resource", "Integer", "" }, null));
		_dataArray.Add(new InstantNotificationItem(57, EInstantNotificationType.Item, 162, 0, 163, 164, new string[4] { "Character", "Item", "", "" }, new List<sbyte> { 1 }));
		_dataArray.Add(new InstantNotificationItem(58, EInstantNotificationType.Item, 165, 0, 166, 167, new string[4] { "Character", "Item", "", "" }, new List<sbyte> { 1 }));
		_dataArray.Add(new InstantNotificationItem(59, EInstantNotificationType.Property, 168, 0, 169, 170, new string[4] { "Character", "", "", "" }, null));
	}

	private void CreateItems1()
	{
		_dataArray.Add(new InstantNotificationItem(60, EInstantNotificationType.Property, 171, 0, 172, 172, new string[4] { "Character", "", "", "" }, new List<sbyte> { 0 }));
		_dataArray.Add(new InstantNotificationItem(61, EInstantNotificationType.Item, 173, 1, 174, 175, new string[4] { "Cricket", "", "", "" }, new List<sbyte> { 0 }));
		_dataArray.Add(new InstantNotificationItem(62, EInstantNotificationType.Society, 176, 0, 177, 178, new string[4] { "Character", "Location", "Character", "" }, null));
		_dataArray.Add(new InstantNotificationItem(63, EInstantNotificationType.Society, 179, 0, 177, 180, new string[4] { "Character", "Location", "Character", "" }, null));
		_dataArray.Add(new InstantNotificationItem(64, EInstantNotificationType.Society, 181, 0, 182, 183, new string[4] { "Character", "Location", "Character", "" }, null));
		_dataArray.Add(new InstantNotificationItem(65, EInstantNotificationType.Society, 184, 0, 185, 186, new string[4] { "Character", "Location", "Character", "" }, null));
		_dataArray.Add(new InstantNotificationItem(66, EInstantNotificationType.Society, 187, 0, 188, 189, new string[4] { "Character", "Character", "Character", "" }, null));
		_dataArray.Add(new InstantNotificationItem(67, EInstantNotificationType.Society, 187, 0, 190, 191, new string[4] { "Character", "Character", "Character", "" }, null));
		_dataArray.Add(new InstantNotificationItem(68, EInstantNotificationType.Society, 187, 0, 192, 193, new string[4] { "Character", "Character", "Character", "" }, null));
		_dataArray.Add(new InstantNotificationItem(69, EInstantNotificationType.Society, 194, 0, 195, 196, new string[4] { "Character", "Character", "", "" }, null));
		_dataArray.Add(new InstantNotificationItem(70, EInstantNotificationType.Society, 197, 0, 198, 199, new string[4] { "Character", "", "", "" }, null));
		_dataArray.Add(new InstantNotificationItem(71, EInstantNotificationType.Society, 200, 0, 201, 202, new string[4] { "Character", "", "", "" }, null));
		_dataArray.Add(new InstantNotificationItem(72, EInstantNotificationType.Society, 203, 0, 204, 205, new string[4] { "Character", "Character", "", "" }, null));
		_dataArray.Add(new InstantNotificationItem(73, EInstantNotificationType.Society, 206, 0, 207, 208, new string[4] { "Character", "", "", "" }, null));
		_dataArray.Add(new InstantNotificationItem(74, EInstantNotificationType.Society, 209, 0, 210, 211, new string[4] { "Character", "Character", "Character", "" }, null));
		_dataArray.Add(new InstantNotificationItem(75, EInstantNotificationType.Society, 212, 0, 213, 214, new string[4] { "Character", "Character", "", "" }, null));
		_dataArray.Add(new InstantNotificationItem(76, EInstantNotificationType.Society, 215, 0, 216, 217, new string[4] { "Character", "Character", "Character", "" }, null));
		_dataArray.Add(new InstantNotificationItem(77, EInstantNotificationType.Society, 218, 0, 219, 220, new string[4] { "Character", "Character", "Character", "" }, null));
		_dataArray.Add(new InstantNotificationItem(78, EInstantNotificationType.Society, 221, 0, 222, 223, new string[4] { "Character", "Character", "Character", "" }, null));
		_dataArray.Add(new InstantNotificationItem(79, EInstantNotificationType.Society, 215, 0, 216, 224, new string[4] { "Character", "Character", "Character", "" }, null));
		_dataArray.Add(new InstantNotificationItem(80, EInstantNotificationType.Society, 218, 0, 219, 225, new string[4] { "Character", "Character", "Character", "" }, null));
		_dataArray.Add(new InstantNotificationItem(81, EInstantNotificationType.Society, 226, 0, 227, 228, new string[4] { "Character", "Character", "Character", "" }, null));
		_dataArray.Add(new InstantNotificationItem(82, EInstantNotificationType.Society, 215, 0, 216, 229, new string[4] { "Character", "Character", "Character", "" }, null));
		_dataArray.Add(new InstantNotificationItem(83, EInstantNotificationType.Society, 230, 0, 231, 232, new string[4] { "Character", "Character", "Character", "" }, null));
		_dataArray.Add(new InstantNotificationItem(84, EInstantNotificationType.Society, 230, 0, 231, 233, new string[4] { "Character", "Character", "Character", "" }, null));
		_dataArray.Add(new InstantNotificationItem(85, EInstantNotificationType.Society, 230, 0, 231, 234, new string[4] { "Character", "Character", "Character", "" }, null));
		_dataArray.Add(new InstantNotificationItem(86, EInstantNotificationType.Society, 230, 0, 231, 235, new string[4] { "Character", "Character", "Character", "" }, null));
		_dataArray.Add(new InstantNotificationItem(87, EInstantNotificationType.Society, 230, 0, 231, 236, new string[4] { "Character", "Character", "Character", "" }, null));
		_dataArray.Add(new InstantNotificationItem(88, EInstantNotificationType.Society, 230, 0, 231, 237, new string[4] { "Character", "Character", "Character", "" }, null));
		_dataArray.Add(new InstantNotificationItem(89, EInstantNotificationType.Society, 238, 0, 239, 240, new string[4] { "Character", "Character", "Character", "" }, null));
		_dataArray.Add(new InstantNotificationItem(90, EInstantNotificationType.Society, 241, 0, 242, 243, new string[4] { "Character", "Character", "Character", "" }, null));
		_dataArray.Add(new InstantNotificationItem(91, EInstantNotificationType.Society, 241, 0, 244, 245, new string[4] { "Character", "Character", "Character", "" }, null));
		_dataArray.Add(new InstantNotificationItem(92, EInstantNotificationType.Society, 246, 0, 247, 248, new string[4] { "Character", "Character", "Character", "" }, null));
		_dataArray.Add(new InstantNotificationItem(93, EInstantNotificationType.Team, 249, 2, 250, 251, new string[4] { "Settlement", "Integer", "", "" }, null));
		_dataArray.Add(new InstantNotificationItem(94, EInstantNotificationType.Team, 252, 0, 253, 253, new string[4] { "Profession", "", "", "" }, null));
		_dataArray.Add(new InstantNotificationItem(95, EInstantNotificationType.Team, 254, 0, 255, 256, new string[4] { "Profession", "ProfessionSkill", "", "" }, null));
		_dataArray.Add(new InstantNotificationItem(96, EInstantNotificationType.Team, 257, 0, 258, 258, new string[4] { "ProfessionSkill", "", "", "" }, null));
		_dataArray.Add(new InstantNotificationItem(97, EInstantNotificationType.Team, 259, 0, 260, 261, new string[4] { "ProfessionSkill", "", "", "" }, null));
		_dataArray.Add(new InstantNotificationItem(98, EInstantNotificationType.Team, 262, 0, 263, 264, new string[4] { "OrgGrade", "", "", "" }, null));
		_dataArray.Add(new InstantNotificationItem(99, EInstantNotificationType.Team, 265, 0, 266, 267, new string[4] { "Location", "CombatSkillType", "Character", "Integer" }, null));
		_dataArray.Add(new InstantNotificationItem(100, EInstantNotificationType.Team, 268, 0, 266, 267, new string[4] { "Location", "LifeSkillType", "Character", "Integer" }, null));
		_dataArray.Add(new InstantNotificationItem(101, EInstantNotificationType.Team, 269, 0, 270, 271, new string[4] { "Character", "Character", "Integer", "" }, null));
		_dataArray.Add(new InstantNotificationItem(102, EInstantNotificationType.Team, 272, 0, 273, 274, new string[4] { "Character", "Character", "OrgGrade", "" }, null));
		_dataArray.Add(new InstantNotificationItem(103, EInstantNotificationType.Team, 275, 0, 276, 277, new string[4] { "Character", "Location", "Integer", "" }, null));
		_dataArray.Add(new InstantNotificationItem(104, EInstantNotificationType.Team, 278, 0, 279, 280, new string[4] { "Character", "Profession", "Integer", "" }, null));
		_dataArray.Add(new InstantNotificationItem(105, EInstantNotificationType.Team, 281, 0, 282, 283, new string[4] { "Character", "Profession", "Integer", "" }, null));
		_dataArray.Add(new InstantNotificationItem(106, EInstantNotificationType.Team, 284, 0, 285, 285, new string[4] { "", "", "", "" }, null));
		_dataArray.Add(new InstantNotificationItem(107, EInstantNotificationType.Team, 286, 0, 287, 288, new string[4] { "Settlement", "", "", "" }, null));
		_dataArray.Add(new InstantNotificationItem(108, EInstantNotificationType.Team, 289, 0, 290, 291, new string[4] { "Settlement", "", "", "" }, null));
		_dataArray.Add(new InstantNotificationItem(109, EInstantNotificationType.Team, 292, 0, 293, 294, new string[4] { "Item", "Integer", "Integer", "" }, null));
		_dataArray.Add(new InstantNotificationItem(110, EInstantNotificationType.Team, 295, 0, 293, 296, new string[4] { "Item", "Integer", "Integer", "" }, null));
		_dataArray.Add(new InstantNotificationItem(111, EInstantNotificationType.Team, 292, 0, 293, 297, new string[4] { "Item", "Integer", "", "" }, null));
		_dataArray.Add(new InstantNotificationItem(112, EInstantNotificationType.Team, 295, 0, 293, 298, new string[4] { "Item", "Integer", "", "" }, null));
		_dataArray.Add(new InstantNotificationItem(113, EInstantNotificationType.TaiwuVillage, 299, 0, 300, 301, new string[4] { "Item", "", "", "" }, null));
		_dataArray.Add(new InstantNotificationItem(114, EInstantNotificationType.TaiwuVillage, 302, 0, 303, 303, new string[4] { "Character", "", "", "" }, null));
		_dataArray.Add(new InstantNotificationItem(115, EInstantNotificationType.TaiwuVillage, 304, 0, 305, 306, new string[4] { "Location", "Adventure", "Character", "" }, null));
		_dataArray.Add(new InstantNotificationItem(116, EInstantNotificationType.Team, 307, 0, 308, 309, new string[4] { "Settlement", "Music", "", "" }, null));
		_dataArray.Add(new InstantNotificationItem(117, EInstantNotificationType.Team, 310, 0, 308, 309, new string[4] { "MapState", "Music", "", "" }, null));
		_dataArray.Add(new InstantNotificationItem(118, EInstantNotificationType.Team, 311, 0, 312, 313, new string[4] { "Character", "", "", "" }, null));
		_dataArray.Add(new InstantNotificationItem(119, EInstantNotificationType.TaiwuVillage, 314, 0, 315, 316, new string[4] { "Settlement", "Character", "Building", "" }, null));
	}

	private void CreateItems2()
	{
		_dataArray.Add(new InstantNotificationItem(120, EInstantNotificationType.Team, 317, 0, 318, 319, new string[4] { "Integer", "", "", "" }, null));
		_dataArray.Add(new InstantNotificationItem(121, EInstantNotificationType.Team, 320, 0, 321, 322, new string[4] { "Integer", "", "", "" }, null));
		_dataArray.Add(new InstantNotificationItem(122, EInstantNotificationType.Team, 323, 0, 324, 325, new string[4] { "Integer", "", "", "" }, null));
		_dataArray.Add(new InstantNotificationItem(123, EInstantNotificationType.Team, 326, 0, 327, 328, new string[4] { "Integer", "", "", "" }, null));
		_dataArray.Add(new InstantNotificationItem(124, EInstantNotificationType.Team, 329, 0, 330, 331, new string[4] { "Integer", "", "", "" }, null));
		_dataArray.Add(new InstantNotificationItem(125, EInstantNotificationType.Team, 332, 0, 333, 334, new string[4] { "Integer", "", "", "" }, null));
		_dataArray.Add(new InstantNotificationItem(126, EInstantNotificationType.Team, 335, 0, 336, 337, new string[4] { "Integer", "", "", "" }, null));
		_dataArray.Add(new InstantNotificationItem(127, EInstantNotificationType.Team, 338, 0, 339, 340, new string[4] { "Integer", "", "", "" }, null));
		_dataArray.Add(new InstantNotificationItem(128, EInstantNotificationType.Team, 341, 0, 342, 343, new string[4] { "Integer", "", "", "" }, null));
		_dataArray.Add(new InstantNotificationItem(129, EInstantNotificationType.Team, 344, 0, 345, 346, new string[4] { "Integer", "", "", "" }, null));
		_dataArray.Add(new InstantNotificationItem(130, EInstantNotificationType.Property, 347, 0, 348, 349, new string[4] { "JiaoLoong", "JiaoProperty", "Integer", "" }, null));
		_dataArray.Add(new InstantNotificationItem(131, EInstantNotificationType.Property, 350, 0, 351, 352, new string[4] { "JiaoLoong", "JiaoProperty", "Integer", "" }, null));
		_dataArray.Add(new InstantNotificationItem(132, EInstantNotificationType.Property, 353, 0, 348, 354, new string[4] { "JiaoLoong", "JiaoProperty", "", "" }, null));
		_dataArray.Add(new InstantNotificationItem(133, EInstantNotificationType.Property, 355, 0, 351, 356, new string[4] { "JiaoLoong", "JiaoProperty", "", "" }, null));
		_dataArray.Add(new InstantNotificationItem(134, EInstantNotificationType.Property, 357, 0, 348, 358, new string[4] { "JiaoLoong", "JiaoProperty", "Integer", "" }, null));
		_dataArray.Add(new InstantNotificationItem(135, EInstantNotificationType.Property, 359, 0, 351, 360, new string[4] { "JiaoLoong", "JiaoProperty", "Integer", "" }, null));
		_dataArray.Add(new InstantNotificationItem(136, EInstantNotificationType.Property, 361, 0, 348, 349, new string[4] { "JiaoLoong", "JiaoProperty", "Float", "" }, null));
		_dataArray.Add(new InstantNotificationItem(137, EInstantNotificationType.Property, 362, 0, 351, 352, new string[4] { "JiaoLoong", "JiaoProperty", "Float", "" }, null));
		_dataArray.Add(new InstantNotificationItem(138, EInstantNotificationType.Team, 363, 0, 364, 365, new string[4] { "Item", "Character", "Location", "" }, null));
		_dataArray.Add(new InstantNotificationItem(139, EInstantNotificationType.Team, 366, 0, 364, 367, new string[4] { "Item", "Location", "Character", "" }, null));
		_dataArray.Add(new InstantNotificationItem(140, EInstantNotificationType.Team, 368, 0, 369, 370, new string[4] { "Character", "Item", "", "" }, null));
		_dataArray.Add(new InstantNotificationItem(141, EInstantNotificationType.Team, 368, 0, 369, 371, new string[4] { "Character", "Item", "", "" }, null));
		_dataArray.Add(new InstantNotificationItem(142, EInstantNotificationType.Society, 372, 0, 373, 374, new string[4] { "Character", "", "", "" }, new List<sbyte> { 0 }));
		_dataArray.Add(new InstantNotificationItem(143, EInstantNotificationType.Item, 375, 0, 376, 376, new string[4] { "Location", "", "", "" }, null));
		_dataArray.Add(new InstantNotificationItem(144, EInstantNotificationType.Team, 377, 0, 364, 365, new string[4] { "Item", "CharacterTemplate", "Location", "" }, null));
		_dataArray.Add(new InstantNotificationItem(145, EInstantNotificationType.Team, 378, 0, 364, 379, new string[4] { "Item", "Location", "", "" }, null));
		_dataArray.Add(new InstantNotificationItem(146, EInstantNotificationType.Team, 380, 0, 381, 382, new string[4] { "CombatSkill", "", "", "" }, null));
		_dataArray.Add(new InstantNotificationItem(147, EInstantNotificationType.Team, 383, 0, 381, 384, new string[4] { "CombatSkill", "", "", "" }, null));
		_dataArray.Add(new InstantNotificationItem(148, EInstantNotificationType.Society, 385, 0, 386, 387, new string[4] { "Character", "", "", "" }, null));
		_dataArray.Add(new InstantNotificationItem(149, EInstantNotificationType.Society, 388, 0, 389, 390, new string[4] { "", "", "", "" }, null));
		_dataArray.Add(new InstantNotificationItem(150, EInstantNotificationType.Team, 391, 1, 56, 392, new string[4] { "", "", "", "" }, null));
		_dataArray.Add(new InstantNotificationItem(151, EInstantNotificationType.Team, 252, 0, 253, 393, new string[4] { "Profession", "Integer", "", "" }, null));
		_dataArray.Add(new InstantNotificationItem(152, EInstantNotificationType.DuringMonth, 394, 0, 395, 396, new string[4] { "Integer", "", "", "" }, null));
		_dataArray.Add(new InstantNotificationItem(153, EInstantNotificationType.DuringMonth, 394, 0, 397, 398, new string[4] { "", "", "", "" }, null));
		_dataArray.Add(new InstantNotificationItem(154, EInstantNotificationType.Team, 399, 0, 400, 400, new string[4] { "Item", "", "", "" }, null));
		_dataArray.Add(new InstantNotificationItem(155, EInstantNotificationType.Team, 401, 0, 402, 403, new string[4] { "Item", "", "", "" }, null));
		_dataArray.Add(new InstantNotificationItem(156, EInstantNotificationType.DuringMonth, 404, 0, 405, 406, new string[4] { "Integer", "Integer", "", "" }, null));
		_dataArray.Add(new InstantNotificationItem(157, EInstantNotificationType.DuringMonth, 407, 0, 408, 409, new string[4] { "SecretInformation", "", "", "" }, null));
		_dataArray.Add(new InstantNotificationItem(158, EInstantNotificationType.DuringMonth, 410, 0, 411, 412, new string[4] { "Location", "Integer", "", "" }, null));
		_dataArray.Add(new InstantNotificationItem(159, EInstantNotificationType.Property, 413, 0, 414, 415, new string[4] { "Character", "Integer", "Integer", "" }, null));
		_dataArray.Add(new InstantNotificationItem(160, EInstantNotificationType.Property, 413, 0, 416, 417, new string[4] { "Character", "Integer", "Integer", "" }, null));
		_dataArray.Add(new InstantNotificationItem(161, EInstantNotificationType.Team, 418, 0, 419, 419, new string[4] { "Character", "CharacterPropertyReferencedType", "Integer", "" }, null));
		_dataArray.Add(new InstantNotificationItem(162, EInstantNotificationType.Team, 418, 0, 420, 420, new string[4] { "Character", "", "", "" }, null));
		_dataArray.Add(new InstantNotificationItem(163, EInstantNotificationType.Team, 418, 0, 421, 421, new string[4] { "Character", "", "", "" }, null));
		_dataArray.Add(new InstantNotificationItem(164, EInstantNotificationType.Team, 418, 0, 422, 422, new string[4] { "Character", "", "", "" }, null));
		_dataArray.Add(new InstantNotificationItem(165, EInstantNotificationType.DuringMonth, 423, 0, 424, 425, new string[4] { "Settlement", "Integer", "", "" }, null));
		_dataArray.Add(new InstantNotificationItem(166, EInstantNotificationType.DuringMonth, 426, 0, 427, 428, new string[4] { "Location", "", "", "" }, null));
		_dataArray.Add(new InstantNotificationItem(167, EInstantNotificationType.DuringMonth, 269, 0, 429, 430, new string[4] { "Integer", "", "", "" }, null));
		_dataArray.Add(new InstantNotificationItem(168, EInstantNotificationType.DuringMonth, 431, 0, 432, 433, new string[4] { "Location", "Integer", "", "" }, null));
		_dataArray.Add(new InstantNotificationItem(169, EInstantNotificationType.Item, 434, 0, 435, 436, new string[4] { "Character", "Integer", "", "" }, null));
		_dataArray.Add(new InstantNotificationItem(170, EInstantNotificationType.DuringMonth, 437, 0, 438, 438, new string[4] { "Integer", "", "", "" }, null));
		_dataArray.Add(new InstantNotificationItem(171, EInstantNotificationType.DuringMonth, 439, 0, 440, 441, new string[4] { "Character", "Settlement", "", "" }, new List<sbyte> { 1 }));
		_dataArray.Add(new InstantNotificationItem(172, EInstantNotificationType.DuringMonth, 442, 0, 443, 444, new string[4] { "Character", "Settlement", "", "" }, new List<sbyte> { 1 }));
		_dataArray.Add(new InstantNotificationItem(173, EInstantNotificationType.Society, 445, 0, 446, 447, new string[4] { "Character", "", "", "" }, null));
		_dataArray.Add(new InstantNotificationItem(174, EInstantNotificationType.Team, 448, 0, 449, 450, new string[4] { "Legacy", "", "", "" }, null));
		_dataArray.Add(new InstantNotificationItem(175, EInstantNotificationType.DuringMonth, 451, 0, 452, 453, new string[4] { "Settlement", "Integer", "", "" }, null));
		_dataArray.Add(new InstantNotificationItem(176, EInstantNotificationType.DuringMonth, 454, 0, 455, 456, new string[4] { "Settlement", "Integer", "", "" }, null));
		_dataArray.Add(new InstantNotificationItem(177, EInstantNotificationType.DuringMonth, 457, 0, 458, 459, new string[4] { "Settlement", "Integer", "", "" }, null));
		_dataArray.Add(new InstantNotificationItem(178, EInstantNotificationType.DuringMonth, 460, 0, 461, 462, new string[4] { "Settlement", "Integer", "", "" }, null));
		_dataArray.Add(new InstantNotificationItem(179, EInstantNotificationType.Item, 463, 0, 464, 465, new string[4] { "Location", "Resource", "Integer", "" }, null));
	}

	private void CreateItems3()
	{
		_dataArray.Add(new InstantNotificationItem(180, EInstantNotificationType.Item, 466, 0, 467, 468, new string[4] { "Location", "Item", "", "" }, null));
		_dataArray.Add(new InstantNotificationItem(181, EInstantNotificationType.Item, 469, 0, 467, 470, new string[4] { "Location", "Item", "", "" }, null));
		_dataArray.Add(new InstantNotificationItem(182, EInstantNotificationType.Item, 471, 0, 467, 472, new string[4] { "Location", "Item", "", "" }, null));
		_dataArray.Add(new InstantNotificationItem(183, EInstantNotificationType.Item, 473, 0, 467, 474, new string[4] { "Location", "Item", "", "" }, null));
		_dataArray.Add(new InstantNotificationItem(184, EInstantNotificationType.Item, 475, 0, 476, 477, new string[4] { "Item", "", "", "" }, null));
		_dataArray.Add(new InstantNotificationItem(185, EInstantNotificationType.Item, 478, 0, 476, 477, new string[4] { "Item", "", "", "" }, null));
		_dataArray.Add(new InstantNotificationItem(186, EInstantNotificationType.Item, 479, 0, 476, 477, new string[4] { "Item", "", "", "" }, null));
		_dataArray.Add(new InstantNotificationItem(187, EInstantNotificationType.Item, 480, 0, 476, 477, new string[4] { "Item", "", "", "" }, null));
		_dataArray.Add(new InstantNotificationItem(188, EInstantNotificationType.Item, 481, 0, 467, 482, new string[4] { "Location", "Item", "", "" }, null));
		_dataArray.Add(new InstantNotificationItem(189, EInstantNotificationType.Item, 483, 0, 467, 484, new string[4] { "Location", "Item", "", "" }, null));
		_dataArray.Add(new InstantNotificationItem(190, EInstantNotificationType.Item, 485, 0, 467, 486, new string[4] { "Location", "Item", "", "" }, null));
		_dataArray.Add(new InstantNotificationItem(191, EInstantNotificationType.Item, 487, 0, 467, 488, new string[4] { "Location", "Item", "", "" }, null));
		_dataArray.Add(new InstantNotificationItem(192, EInstantNotificationType.Item, 489, 0, 467, 490, new string[4] { "Location", "Item", "", "" }, null));
		_dataArray.Add(new InstantNotificationItem(193, EInstantNotificationType.Item, 491, 0, 467, 492, new string[4] { "Location", "Item", "", "" }, null));
		_dataArray.Add(new InstantNotificationItem(194, EInstantNotificationType.Item, 493, 0, 467, 494, new string[4] { "Location", "Item", "", "" }, null));
		_dataArray.Add(new InstantNotificationItem(195, EInstantNotificationType.Item, 495, 0, 467, 496, new string[4] { "Location", "Item", "", "" }, null));
		_dataArray.Add(new InstantNotificationItem(196, EInstantNotificationType.Item, 497, 0, 467, 498, new string[4] { "Location", "Item", "", "" }, null));
		_dataArray.Add(new InstantNotificationItem(197, EInstantNotificationType.Item, 499, 0, 467, 500, new string[4] { "Location", "Item", "", "" }, null));
		_dataArray.Add(new InstantNotificationItem(198, EInstantNotificationType.Item, 501, 0, 467, 500, new string[4] { "Location", "Item", "", "" }, null));
		_dataArray.Add(new InstantNotificationItem(199, EInstantNotificationType.Item, 502, 0, 467, 500, new string[4] { "Location", "Item", "", "" }, null));
		_dataArray.Add(new InstantNotificationItem(200, EInstantNotificationType.Item, 503, 0, 467, 500, new string[4] { "Location", "Item", "", "" }, null));
		_dataArray.Add(new InstantNotificationItem(201, EInstantNotificationType.Item, 504, 0, 467, 500, new string[4] { "Location", "Item", "", "" }, null));
		_dataArray.Add(new InstantNotificationItem(202, EInstantNotificationType.Item, 505, 0, 467, 500, new string[4] { "Location", "Item", "", "" }, null));
		_dataArray.Add(new InstantNotificationItem(203, EInstantNotificationType.Item, 506, 0, 467, 500, new string[4] { "Location", "Item", "", "" }, null));
		_dataArray.Add(new InstantNotificationItem(204, EInstantNotificationType.Item, 507, 0, 467, 500, new string[4] { "Location", "Item", "", "" }, null));
		_dataArray.Add(new InstantNotificationItem(205, EInstantNotificationType.Item, 508, 0, 467, 500, new string[4] { "Location", "Item", "", "" }, null));
		_dataArray.Add(new InstantNotificationItem(206, EInstantNotificationType.Item, 509, 0, 467, 500, new string[4] { "Location", "Item", "", "" }, null));
		_dataArray.Add(new InstantNotificationItem(207, EInstantNotificationType.Item, 510, 0, 467, 500, new string[4] { "Location", "Item", "", "" }, null));
		_dataArray.Add(new InstantNotificationItem(208, EInstantNotificationType.Item, 511, 0, 467, 500, new string[4] { "Location", "Item", "", "" }, null));
		_dataArray.Add(new InstantNotificationItem(209, EInstantNotificationType.Item, 512, 0, 467, 500, new string[4] { "Location", "Item", "", "" }, null));
		_dataArray.Add(new InstantNotificationItem(210, EInstantNotificationType.Item, 513, 0, 467, 500, new string[4] { "Location", "Item", "", "" }, null));
		_dataArray.Add(new InstantNotificationItem(211, EInstantNotificationType.Item, 514, 0, 467, 500, new string[4] { "Location", "Item", "", "" }, null));
		_dataArray.Add(new InstantNotificationItem(212, EInstantNotificationType.Item, 515, 0, 467, 500, new string[4] { "Location", "Item", "", "" }, null));
		_dataArray.Add(new InstantNotificationItem(213, EInstantNotificationType.Item, 516, 0, 467, 517, new string[4] { "Location", "Item", "", "" }, null));
		_dataArray.Add(new InstantNotificationItem(214, EInstantNotificationType.Item, 518, 0, 467, 517, new string[4] { "Location", "Item", "", "" }, null));
		_dataArray.Add(new InstantNotificationItem(215, EInstantNotificationType.Item, 519, 0, 467, 517, new string[4] { "Location", "Item", "", "" }, null));
		_dataArray.Add(new InstantNotificationItem(216, EInstantNotificationType.Item, 520, 0, 467, 517, new string[4] { "Location", "Item", "", "" }, null));
		_dataArray.Add(new InstantNotificationItem(217, EInstantNotificationType.Item, 521, 0, 467, 522, new string[4] { "Location", "Item", "", "" }, null));
		_dataArray.Add(new InstantNotificationItem(218, EInstantNotificationType.Item, 523, 0, 524, 525, new string[4] { "Location", "Integer", "", "" }, null));
		_dataArray.Add(new InstantNotificationItem(219, EInstantNotificationType.Item, 526, 0, 527, 528, new string[4] { "", "", "", "" }, null));
		_dataArray.Add(new InstantNotificationItem(220, EInstantNotificationType.Item, 529, 0, 530, 531, new string[4] { "", "", "", "" }, null));
		_dataArray.Add(new InstantNotificationItem(221, EInstantNotificationType.Item, 532, 0, 533, 534, new string[4] { "MapState", "", "", "" }, null));
		_dataArray.Add(new InstantNotificationItem(222, EInstantNotificationType.Item, 535, 0, 354, 536, new string[4] { "Character", "CharacterPropertyReferencedType", "Integer", "" }, null));
		_dataArray.Add(new InstantNotificationItem(223, EInstantNotificationType.DuringMonth, 537, 0, 538, 539, new string[4] { "Location", "CharacterTemplate", "", "" }, null));
		_dataArray.Add(new InstantNotificationItem(224, EInstantNotificationType.Item, 523, 0, 540, 541, new string[4] { "Integer", "", "", "" }, null));
		_dataArray.Add(new InstantNotificationItem(225, EInstantNotificationType.Team, 542, 0, 543, 544, new string[4] { "", "", "", "" }, null));
		_dataArray.Add(new InstantNotificationItem(226, EInstantNotificationType.Team, 545, 0, 543, 546, new string[4] { "", "", "", "" }, null));
		_dataArray.Add(new InstantNotificationItem(227, EInstantNotificationType.Team, 418, 0, 547, 547, new string[4] { "Character", "OrgGrade", "", "" }, null));
		_dataArray.Add(new InstantNotificationItem(228, EInstantNotificationType.Team, 418, 0, 548, 548, new string[4] { "Character", "OrgGrade", "", "" }, null));
		_dataArray.Add(new InstantNotificationItem(229, EInstantNotificationType.Team, 418, 0, 549, 549, new string[4] { "Character", "OrgGrade", "", "" }, null));
		_dataArray.Add(new InstantNotificationItem(230, EInstantNotificationType.Team, 418, 0, 547, 547, new string[4] { "Character", "CharGrade", "", "" }, null));
		_dataArray.Add(new InstantNotificationItem(231, EInstantNotificationType.Team, 418, 0, 548, 548, new string[4] { "Character", "CharGrade", "", "" }, null));
		_dataArray.Add(new InstantNotificationItem(232, EInstantNotificationType.Team, 418, 0, 549, 549, new string[4] { "Character", "CharGrade", "", "" }, null));
		_dataArray.Add(new InstantNotificationItem(233, EInstantNotificationType.DuringMonth, 550, 0, 551, 551, new string[4] { "Character", "", "", "" }, null));
		_dataArray.Add(new InstantNotificationItem(234, EInstantNotificationType.Item, 375, 0, 376, 552, new string[4] { "Location", "Integer", "", "" }, null));
		_dataArray.Add(new InstantNotificationItem(235, EInstantNotificationType.Item, 553, 0, 554, 555, new string[4] { "Location", "Integer", "", "" }, null));
		_dataArray.Add(new InstantNotificationItem(236, EInstantNotificationType.DuringMonth, 556, 0, 557, 557, new string[4] { "CharacterTemplate", "", "", "" }, null));
		_dataArray.Add(new InstantNotificationItem(237, EInstantNotificationType.DuringMonth, 558, 0, 559, 559, new string[4] { "CharacterTemplate", "", "", "" }, null));
		_dataArray.Add(new InstantNotificationItem(238, EInstantNotificationType.DuringMonth, 560, 0, 561, 561, new string[4] { "CharacterTemplate", "", "", "" }, null));
		_dataArray.Add(new InstantNotificationItem(239, EInstantNotificationType.DuringMonth, 562, 0, 563, 563, new string[4] { "CharacterTemplate", "", "", "" }, null));
	}

	private void CreateItems4()
	{
		_dataArray.Add(new InstantNotificationItem(240, EInstantNotificationType.Item, 475, 0, 467, 564, new string[4] { "Location", "Item", "", "" }, null));
		_dataArray.Add(new InstantNotificationItem(241, EInstantNotificationType.Item, 478, 0, 467, 564, new string[4] { "Location", "Item", "", "" }, null));
		_dataArray.Add(new InstantNotificationItem(242, EInstantNotificationType.Item, 479, 0, 467, 564, new string[4] { "Location", "Item", "", "" }, null));
		_dataArray.Add(new InstantNotificationItem(243, EInstantNotificationType.Item, 480, 0, 467, 564, new string[4] { "Location", "Item", "", "" }, null));
		_dataArray.Add(new InstantNotificationItem(244, EInstantNotificationType.Item, 565, 0, 566, 567, new string[4] { "Resource", "Integer", "", "" }, null));
		_dataArray.Add(new InstantNotificationItem(245, EInstantNotificationType.Item, 568, 0, 569, 570, new string[4] { "Integer", "", "", "" }, null));
		_dataArray.Add(new InstantNotificationItem(246, EInstantNotificationType.Item, 571, 0, 572, 573, new string[4] { "MapState", "", "", "" }, null));
		_dataArray.Add(new InstantNotificationItem(247, EInstantNotificationType.Item, 574, 0, 575, 576, new string[4] { "Item", "", "", "" }, null));
		_dataArray.Add(new InstantNotificationItem(248, EInstantNotificationType.Item, 577, 0, 578, 579, new string[4] { "", "", "", "" }, null));
		_dataArray.Add(new InstantNotificationItem(249, EInstantNotificationType.Item, 580, 0, 581, 582, new string[4] { "", "", "", "" }, null));
		_dataArray.Add(new InstantNotificationItem(250, EInstantNotificationType.Item, 583, 0, 584, 585, new string[4] { "", "", "", "" }, new List<sbyte>()));
		_dataArray.Add(new InstantNotificationItem(251, EInstantNotificationType.Item, 586, 0, 587, 588, new string[4] { "Character", "Integer", "", "" }, null));
		_dataArray.Add(new InstantNotificationItem(252, EInstantNotificationType.Item, 589, 0, 587, 590, new string[4] { "Integer", "Integer", "", "" }, null));
		_dataArray.Add(new InstantNotificationItem(253, EInstantNotificationType.Item, 591, 0, 592, 593, new string[4] { "Integer", "", "", "" }, null));
		_dataArray.Add(new InstantNotificationItem(254, EInstantNotificationType.Item, 594, 0, 595, 596, new string[4] { "Item", "", "", "" }, null));
	}

	public override void Init()
	{
		base.Init();
		_dataArray = new List<InstantNotificationItem>(255);
		CreateItems0();
		CreateItems1();
		CreateItems2();
		CreateItems3();
		CreateItems4();
	}
}
