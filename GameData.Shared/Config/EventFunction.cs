using System;
using System.Collections.Generic;
using Config.Common;

namespace Config;

[Serializable]
public class EventFunction : ConfigData<EventFunctionItem, int>
{
	public static class DefKey
	{
		public const int If = 0;

		public const int Else = 1;

		public const int ElseIf = 2;

		public const int Loop = 3;

		public const int Break = 4;

		public const int End = 5;

		public const int Continue = 6;

		public const int Label = 7;

		public const int Jump = 8;

		public const int Return = 9;

		public const int Assign = 10;

		public const int Random = 11;

		public const int CheckProb = 12;

		public const int EventTransition = 13;

		public const int OptionInjection = 94;

		public const int ExecuteGlobalScript = 91;

		public const int SaveSectMainStoryValue = 101;

		public const int ReadSectMainStoryValue = 102;

		public const int GetListLength = 215;

		public const int GetListElement = 216;

		public const int Log = 14;

		public const int Comment = 15;

		public const int PlayAudio = 16;

		public const int ScreenShake = 17;

		public const int SpecifyEventBackground = 197;

		public const int OpenEmeiCombatSkillSpecialBreak = 187;

		public const int SpecifyCurrMainAttribute = 18;

		public const int ChangeCurrMainAttribute = 19;

		public const int SpecifyInjury = 20;

		public const int ChangeInjury = 21;

		public const int ClearInjuries = 22;

		public const int SpecifyPoisoned = 23;

		public const int ChangePoisoned = 24;

		public const int ClearPoisons = 25;

		public const int SpecifyDisorderOfQi = 26;

		public const int ChangeDisorderOfQi = 27;

		public const int SpecifyHealth = 28;

		public const int ChangeHealth = 29;

		public const int SpecifyHappiness = 30;

		public const int ChangeHappiness = 31;

		public const int SpecifyFavorabilities = 32;

		public const int ChangeFavorability = 33;

		public const int AddFeature = 34;

		public const int RemoveFeature = 35;

		public const int AddKidnappedCharacter = 36;

		public const int RemoveKidnappedCharacter = 37;

		public const int JoinGroup = 38;

		public const int LeaveGroup = 39;

		public const int KillCharacter = 40;

		public const int TakeRandomDamage = 189;

		public const int AddInventoryItem = 41;

		public const int RemoveInventoryItem = 65;

		public const int TransferInventoryItem = 42;

		public const int SpecifyCharacterResource = 129;

		public const int ChangeCharacterResource = 66;

		public const int TransferCharacterResource = 130;

		public const int ChangeCharBaseCombatSkillQualification = 43;

		public const int ChangeCharBaseLifeSkillQualification = 44;

		public const int LearnCombatSkill = 45;

		public const int LearnLifeSkill = 46;

		public const int AddLegacyPoint = 47;

		public const int ExpelTaiwuVillager = 48;

		public const int MakeAppointment = 49;

		public const int RemoveAppointment = 50;

		public const int AdvanceDays = 51;

		public const int ChangeMainStoryLineProgress = 52;

		public const int SetWorldFunctionsStatus = 53;

		public const int ChangeSpiritualDebt = 54;

		public const int ChangeSettlementSafety = 55;

		public const int ChangeSettlementCulture = 56;

		public const int SetBlockAndViewRangeVisible = 57;

		public const int SetSectAllowLearning = 58;

		public const int JoinOrganization = 59;

		public const int SetSectCharApprovedTaiwu = 60;

		public const int SetSectSpiritualDebtInteractionOccurred = 226;

		public const int ChangeMerchantFavorability = 61;

		public const int CreateItem = 62;

		public const int CreateCricket = 63;

		public const int GetCharacterFavorability = 111;

		public const int SetCharCombatSkillPracticeLevel = 64;

		public const int CreateAdventureSite = 67;

		public const int CreateConfigMonthlyAction = 145;

		public const int CreateEnemyCharacter = 92;

		public const int CreateEventActor = 93;

		public const int GetFixedCharacter = 99;

		public const int GetIntelligentCharacterByFilter = 114;

		public const int MoveCharacter = 100;

		public const int StartCombat = 89;

		public const int TriggerExtraTask = 68;

		public const int FinishExtraTask = 69;

		public const int FinishExtraTaskChain = 70;

		public const int TriggerSectMainStoryEndingCountDown = 115;

		public const int SetSectMainStoryEnding = 116;

		public const int GetTemplateIdOfFixedCharacterCombatWith = 113;

		public const int SetCharacterFollowTaiwu = 117;

		public const int CancelCharacterFollowTaiwu = 120;

		public const int StartLifeSkillCombat = 118;

		public const int ExitAdventure = 124;

		public const int GetAdventureCharacter = 143;

		public const int GetAdventureCharacterCount = 144;

		public const int FinishAdventureEvent = 125;

		public const int SelectAdventureBranch = 126;

		public const int GenerateAdventureMap = 119;

		public const int FilterCharacterItem = 127;

		public const int FilterMapBlockInRange = 167;

		public const int RegisterToSelectItemSubTypes = 132;

		public const int RegisterToSelectItemTemplateIds = 133;

		public const int RegisterToExcludeItemTemplateIds = 134;

		public const int FilterCharacterItemByRegister = 135;

		public const int CharacterTeachTaiwuProfession = 147;

		public const int RegisterSettlementMemberFeature = 227;

		public const int AddBuilding = 149;

		public const int SectStoryZhujianCreateCatchableThief = 139;

		public const int SectStoryZhujianCreateGearMate = 150;

		public const int SectStoryZhujianAddAreaMerchantType = 151;

		public const int SectStoryZhujianRemoveAreaMerchantType = 152;

		public const int SectStoryEmeiSetMemberInsaneState = 188;

		public const int GetMapBlockSettlement = 243;

		public const int GetOtherSmallSettlement = 154;

		public const int GetRandomSettlementInState = 192;

		public const int GetSettlementListInState = 217;

		public const int GetCharacterCurrentMapBlock = 198;

		public const int GetCharacterSettlement = 263;

		public const int GetSettlementMapBlock = 199;

		public const int GetCharacterCurrentMapArea = 261;

		public const int GetSettlementMapArea = 262;

		public const int CreateMerchantRandomItem = 153;

		public const int TriggerLegacyPassingEvent = 156;

		public const int StartSetCharacterGivenName = 164;

		public const int FinishSetCharacterGivenName = 165;

		public const int CheckExpression = 71;

		public const int CheckAnd = 109;

		public const int CheckOr = 110;

		public const int CheckListElement = 218;

		public const int CheckWorldFunctionStatus = 155;

		public const int CheckMainStoryProgress = 72;

		public const int CheckTask = 73;

		public const int CheckTaskChain = 74;

		public const int CheckXiangshuLevel = 75;

		public const int CheckFixedCharacterTemplate = 128;

		public const int CheckCharacterCurrMainAttribute = 76;

		public const int CheckCharacterMainAttribute = 77;

		public const int CheckCharacterLifeSkillQualification = 78;

		public const int CheckCharacterLifeSkillAttainment = 79;

		public const int CheckCharacterCombatSkillQualification = 80;

		public const int CheckCharacterCombatSkillAttainment = 81;

		public const int CheckCharacterPersonality = 82;

		public const int CheckCharacterBehaviorType = 107;

		public const int CheckCharacterMorality = 108;

		public const int CheckCharacterCurrAge = 160;

		public const int CheckCharacterActualAge = 161;

		public const int CheckCharacterAgeGroup = 162;

		public const int CheckCharacterGender = 203;

		public const int CheckCharacterResource = 83;

		public const int CheckCharacterFeature = 84;

		public const int CheckCharacterCurrentProfession = 140;

		public const int CheckCharacterSeniorityPercent = 141;

		public const int CheckCharacterInventoryByTemplate = 85;

		public const int CheckCharacterOnSettlementBlock = 86;

		public const int CheckCharacterInSettlementInfluenceRange = 87;

		public const int CheckCharacterInMapState = 95;

		public const int CheckCharacterInMapArea = 96;

		public const int CheckCharacterInMapBlockRange = 168;

		public const int CheckCharacterOnAnySettlement = 97;

		public const int CheckCharacterInAnySettlementInfluenceRange = 98;

		public const int CheckCharacterInSettlementArea = 194;

		public const int CheckCharacterFavorability = 88;

		public const int CheckCharacterFavorabilityType = 142;

		public const int CheckCharacterGrade = 103;

		public const int CheckCharacterSettlement = 104;

		public const int CheckCharacterHasItem = 121;

		public const int CheckCharacterMerchantType = 122;

		public const int CheckCharacterReadLifeSkillPageCount = 131;

		public const int CheckPreviousCombatResult = 90;

		public const int CheckSettlementInMapState = 105;

		public const int CheckSettlementInMapArea = 106;

		public const int CheckAreaSpiritualDebt = 112;

		public const int CheckAreaHasAdventure = 204;

		public const int CheckSectMainStoryValueExists = 123;

		public const int CheckItemType = 136;

		public const int CheckItemSubType = 137;

		public const int CheckItemTemplate = 138;

		public const int TryGetCharacterCurrentProfession = 146;

		public const int CheckCharacterPassMatcher = 213;

		public const int CheckCharacterCanTeachTaiwuProfession = 148;

		public const int CheckCharacterCanTeachTaiwuProfessionSkillUnlock = 163;

		public const int CheckCharacterConsummateLevel = 157;

		public const int CheckIsDreamBack = 158;

		public const int CheckCharacterAlive = 159;

		public const int CreateFixedSkillBook = 166;

		public const int CheckAdventureParameterCount = 169;

		public const int CheckMovePoint = 170;

		public const int CheckCurrMonth = 171;

		public const int CheckCharacterKidnapSpecificGender = 172;

		public const int CheckCharacterKidnapSpecificAgeGroup = 173;

		public const int CheckCharacterKidnapSpecificId = 174;

		public const int CheckCharacterTeammateSpecificIdGender = 175;

		public const int CheckCharacterTeammateSpecificIdAgeGroup = 176;

		public const int CheckCharacterTeammateSpecificIdId = 177;

		public const int CheckCharacterExp = 178;

		public const int CheckCharacterReadCombatSkillPageCount = 179;

		public const int CheckCharacterCombatSkillBreakout = 180;

		public const int CheckSettlementApprovingRate = 190;

		public const int CheckStateHasSettlementType = 193;

		public const int CheckAdventureTemplate = 202;

		public const int CheckAdventurePerMoveCount = 181;

		public const int CheckAdventurePerCostMovePoint = 182;

		public const int CheckAdventureElementVisible = 183;

		public const int SwitchEmeiBlood = 184;

		public const int CheckAdventureCharacterGroup = 185;

		public const int CheckAdventureElementGroup = 186;

		public const int GetCharacterConsummateLevel = 191;

		public const int OpenYuanshanMiniGame = 195;

		public const int ProcessYuanshanMiniGameResults = 196;

		public const int SpecifyXiangshuInfectionValue = 200;

		public const int ChangeXiangshuInfectionValue = 201;

		public const int SetCharacterMarriageStyleOne = 205;

		public const int SetCharacterMarriageStyleTwo = 206;

		public const int IsVitalInPrison = 207;

		public const int SetVitalInPrison = 208;

		public const int PlayVitalAnim = 209;

		public const int GetCharacterBySettlementGradeAndAge = 210;

		public const int AreVitalsDemon = 211;

		public const int GetCurrentVitalIndex = 212;

		public const int InitThreeVitals = 214;

		public const int CheckAdventureParameter = 219;

		public const int SetAdventureParameter = 220;

		public const int ChangeAdventureParameter = 221;

		public const int CheckAdventureParameterStartWith = 222;

		public const int SetAdventureParameterStartWith = 223;

		public const int ChangeAdventureParameterStartWith = 224;

		public const int AdventureCheckProb = 225;

		public const int CheckAdventureElementCount = 228;

		public const int CheckAdventureElementTagCount = 229;

		public const int CheckAdventureParameterIsMax = 230;

		public const int CheckAdventureParameterIsMin = 231;

		public const int CheckAdventureElementInElement = 232;

		public const int CheckAdventureTaiwuInElement = 233;

		public const int CheckAdventureTaiwuInBlockGroup = 234;

		public const int AdventureCreateItem = 235;

		public const int AdventureRemoveItem = 236;

		public const int AdventureCheckUseItem = 237;

		public const int AddJieqingMaskCharId = 238;

		public const int RemoveJieqingMaskCharId = 239;

		public const int AdventureSetAutoDeleteDate = 240;

		public const int AdventureExit = 241;

		public const int DestroyEnemyNest = 242;

		public const int SectMainStoryUnlockUI = 244;

		public const int CheckAdventureElementVisibleWithTag = 245;

		public const int CheckAdventureElementGroupWithTag = 246;

		public const int CheckAdventureElementInElementWithTag = 247;

		public const int CheckAdventureTaiwuInElementWithTag = 248;

		public const int GetCurrentFaith = 249;

		public const int GetFaithLevel = 250;

		public const int GetFuyuFaithTime = 251;

		public const int OpenFuyuFaithPanel = 252;

		public const int OpenFuyuGiftPanel = 253;

		public const int ApplyFuyuFaith = 254;

		public const int ReadSelectResultCount = 255;

		public const int TryGetMaxAcceptableFuyuFaith = 256;

		public const int CheckTaiwuHasFuyuFaith = 257;

		public const int CheckCharacterFuyuFaith = 258;

		public const int AdventureElementFillByGroup = 259;

		public const int AdventureElementFillByElement = 260;

		public const int AdventureDelete = 264;

		public const int CheckAdventureElementParameter = 265;

		public const int SetAdventureElementParameter = 266;

		public const int ChangeAdventureElementParameter = 267;

		public const int AdventureChangeElementCountAtTaiwuLocation = 268;

		public const int AdventureClearElementAtTaiwuLocation = 269;

		public const int AdventureClearElement = 270;
	}

	public static class DefValue
	{
		public static EventFunctionItem If => Instance[0];

		public static EventFunctionItem Else => Instance[1];

		public static EventFunctionItem ElseIf => Instance[2];

		public static EventFunctionItem Loop => Instance[3];

		public static EventFunctionItem Break => Instance[4];

		public static EventFunctionItem End => Instance[5];

		public static EventFunctionItem Continue => Instance[6];

		public static EventFunctionItem Label => Instance[7];

		public static EventFunctionItem Jump => Instance[8];

		public static EventFunctionItem Return => Instance[9];

		public static EventFunctionItem Assign => Instance[10];

		public static EventFunctionItem Random => Instance[11];

		public static EventFunctionItem CheckProb => Instance[12];

		public static EventFunctionItem EventTransition => Instance[13];

		public static EventFunctionItem OptionInjection => Instance[94];

		public static EventFunctionItem ExecuteGlobalScript => Instance[91];

		public static EventFunctionItem SaveSectMainStoryValue => Instance[101];

		public static EventFunctionItem ReadSectMainStoryValue => Instance[102];

		public static EventFunctionItem GetListLength => Instance[215];

		public static EventFunctionItem GetListElement => Instance[216];

		public static EventFunctionItem Log => Instance[14];

		public static EventFunctionItem Comment => Instance[15];

		public static EventFunctionItem PlayAudio => Instance[16];

		public static EventFunctionItem ScreenShake => Instance[17];

		public static EventFunctionItem SpecifyEventBackground => Instance[197];

		public static EventFunctionItem OpenEmeiCombatSkillSpecialBreak => Instance[187];

		public static EventFunctionItem SpecifyCurrMainAttribute => Instance[18];

		public static EventFunctionItem ChangeCurrMainAttribute => Instance[19];

		public static EventFunctionItem SpecifyInjury => Instance[20];

		public static EventFunctionItem ChangeInjury => Instance[21];

		public static EventFunctionItem ClearInjuries => Instance[22];

		public static EventFunctionItem SpecifyPoisoned => Instance[23];

		public static EventFunctionItem ChangePoisoned => Instance[24];

		public static EventFunctionItem ClearPoisons => Instance[25];

		public static EventFunctionItem SpecifyDisorderOfQi => Instance[26];

		public static EventFunctionItem ChangeDisorderOfQi => Instance[27];

		public static EventFunctionItem SpecifyHealth => Instance[28];

		public static EventFunctionItem ChangeHealth => Instance[29];

		public static EventFunctionItem SpecifyHappiness => Instance[30];

		public static EventFunctionItem ChangeHappiness => Instance[31];

		public static EventFunctionItem SpecifyFavorabilities => Instance[32];

		public static EventFunctionItem ChangeFavorability => Instance[33];

		public static EventFunctionItem AddFeature => Instance[34];

		public static EventFunctionItem RemoveFeature => Instance[35];

		public static EventFunctionItem AddKidnappedCharacter => Instance[36];

		public static EventFunctionItem RemoveKidnappedCharacter => Instance[37];

		public static EventFunctionItem JoinGroup => Instance[38];

		public static EventFunctionItem LeaveGroup => Instance[39];

		public static EventFunctionItem KillCharacter => Instance[40];

		public static EventFunctionItem TakeRandomDamage => Instance[189];

		public static EventFunctionItem AddInventoryItem => Instance[41];

		public static EventFunctionItem RemoveInventoryItem => Instance[65];

		public static EventFunctionItem TransferInventoryItem => Instance[42];

		public static EventFunctionItem SpecifyCharacterResource => Instance[129];

		public static EventFunctionItem ChangeCharacterResource => Instance[66];

		public static EventFunctionItem TransferCharacterResource => Instance[130];

		public static EventFunctionItem ChangeCharBaseCombatSkillQualification => Instance[43];

		public static EventFunctionItem ChangeCharBaseLifeSkillQualification => Instance[44];

		public static EventFunctionItem LearnCombatSkill => Instance[45];

		public static EventFunctionItem LearnLifeSkill => Instance[46];

		public static EventFunctionItem AddLegacyPoint => Instance[47];

		public static EventFunctionItem ExpelTaiwuVillager => Instance[48];

		public static EventFunctionItem MakeAppointment => Instance[49];

		public static EventFunctionItem RemoveAppointment => Instance[50];

		public static EventFunctionItem AdvanceDays => Instance[51];

		public static EventFunctionItem ChangeMainStoryLineProgress => Instance[52];

		public static EventFunctionItem SetWorldFunctionsStatus => Instance[53];

		public static EventFunctionItem ChangeSpiritualDebt => Instance[54];

		public static EventFunctionItem ChangeSettlementSafety => Instance[55];

		public static EventFunctionItem ChangeSettlementCulture => Instance[56];

		public static EventFunctionItem SetBlockAndViewRangeVisible => Instance[57];

		public static EventFunctionItem SetSectAllowLearning => Instance[58];

		public static EventFunctionItem JoinOrganization => Instance[59];

		public static EventFunctionItem SetSectCharApprovedTaiwu => Instance[60];

		public static EventFunctionItem SetSectSpiritualDebtInteractionOccurred => Instance[226];

		public static EventFunctionItem ChangeMerchantFavorability => Instance[61];

		public static EventFunctionItem CreateItem => Instance[62];

		public static EventFunctionItem CreateCricket => Instance[63];

		public static EventFunctionItem GetCharacterFavorability => Instance[111];

		public static EventFunctionItem SetCharCombatSkillPracticeLevel => Instance[64];

		public static EventFunctionItem CreateAdventureSite => Instance[67];

		public static EventFunctionItem CreateConfigMonthlyAction => Instance[145];

		public static EventFunctionItem CreateEnemyCharacter => Instance[92];

		public static EventFunctionItem CreateEventActor => Instance[93];

		public static EventFunctionItem GetFixedCharacter => Instance[99];

		public static EventFunctionItem GetIntelligentCharacterByFilter => Instance[114];

		public static EventFunctionItem MoveCharacter => Instance[100];

		public static EventFunctionItem StartCombat => Instance[89];

		public static EventFunctionItem TriggerExtraTask => Instance[68];

		public static EventFunctionItem FinishExtraTask => Instance[69];

		public static EventFunctionItem FinishExtraTaskChain => Instance[70];

		public static EventFunctionItem TriggerSectMainStoryEndingCountDown => Instance[115];

		public static EventFunctionItem SetSectMainStoryEnding => Instance[116];

		public static EventFunctionItem GetTemplateIdOfFixedCharacterCombatWith => Instance[113];

		public static EventFunctionItem SetCharacterFollowTaiwu => Instance[117];

		public static EventFunctionItem CancelCharacterFollowTaiwu => Instance[120];

		public static EventFunctionItem StartLifeSkillCombat => Instance[118];

		public static EventFunctionItem ExitAdventure => Instance[124];

		public static EventFunctionItem GetAdventureCharacter => Instance[143];

		public static EventFunctionItem GetAdventureCharacterCount => Instance[144];

		public static EventFunctionItem FinishAdventureEvent => Instance[125];

		public static EventFunctionItem SelectAdventureBranch => Instance[126];

		public static EventFunctionItem GenerateAdventureMap => Instance[119];

		public static EventFunctionItem FilterCharacterItem => Instance[127];

		public static EventFunctionItem FilterMapBlockInRange => Instance[167];

		public static EventFunctionItem RegisterToSelectItemSubTypes => Instance[132];

		public static EventFunctionItem RegisterToSelectItemTemplateIds => Instance[133];

		public static EventFunctionItem RegisterToExcludeItemTemplateIds => Instance[134];

		public static EventFunctionItem FilterCharacterItemByRegister => Instance[135];

		public static EventFunctionItem CharacterTeachTaiwuProfession => Instance[147];

		public static EventFunctionItem RegisterSettlementMemberFeature => Instance[227];

		public static EventFunctionItem AddBuilding => Instance[149];

		public static EventFunctionItem SectStoryZhujianCreateCatchableThief => Instance[139];

		public static EventFunctionItem SectStoryZhujianCreateGearMate => Instance[150];

		public static EventFunctionItem SectStoryZhujianAddAreaMerchantType => Instance[151];

		public static EventFunctionItem SectStoryZhujianRemoveAreaMerchantType => Instance[152];

		public static EventFunctionItem SectStoryEmeiSetMemberInsaneState => Instance[188];

		public static EventFunctionItem GetMapBlockSettlement => Instance[243];

		public static EventFunctionItem GetOtherSmallSettlement => Instance[154];

		public static EventFunctionItem GetRandomSettlementInState => Instance[192];

		public static EventFunctionItem GetSettlementListInState => Instance[217];

		public static EventFunctionItem GetCharacterCurrentMapBlock => Instance[198];

		public static EventFunctionItem GetCharacterSettlement => Instance[263];

		public static EventFunctionItem GetSettlementMapBlock => Instance[199];

		public static EventFunctionItem GetCharacterCurrentMapArea => Instance[261];

		public static EventFunctionItem GetSettlementMapArea => Instance[262];

		public static EventFunctionItem CreateMerchantRandomItem => Instance[153];

		public static EventFunctionItem TriggerLegacyPassingEvent => Instance[156];

		public static EventFunctionItem StartSetCharacterGivenName => Instance[164];

		public static EventFunctionItem FinishSetCharacterGivenName => Instance[165];

		public static EventFunctionItem CheckExpression => Instance[71];

		public static EventFunctionItem CheckAnd => Instance[109];

		public static EventFunctionItem CheckOr => Instance[110];

		public static EventFunctionItem CheckListElement => Instance[218];

		public static EventFunctionItem CheckWorldFunctionStatus => Instance[155];

		public static EventFunctionItem CheckMainStoryProgress => Instance[72];

		public static EventFunctionItem CheckTask => Instance[73];

		public static EventFunctionItem CheckTaskChain => Instance[74];

		public static EventFunctionItem CheckXiangshuLevel => Instance[75];

		public static EventFunctionItem CheckFixedCharacterTemplate => Instance[128];

		public static EventFunctionItem CheckCharacterCurrMainAttribute => Instance[76];

		public static EventFunctionItem CheckCharacterMainAttribute => Instance[77];

		public static EventFunctionItem CheckCharacterLifeSkillQualification => Instance[78];

		public static EventFunctionItem CheckCharacterLifeSkillAttainment => Instance[79];

		public static EventFunctionItem CheckCharacterCombatSkillQualification => Instance[80];

		public static EventFunctionItem CheckCharacterCombatSkillAttainment => Instance[81];

		public static EventFunctionItem CheckCharacterPersonality => Instance[82];

		public static EventFunctionItem CheckCharacterBehaviorType => Instance[107];

		public static EventFunctionItem CheckCharacterMorality => Instance[108];

		public static EventFunctionItem CheckCharacterCurrAge => Instance[160];

		public static EventFunctionItem CheckCharacterActualAge => Instance[161];

		public static EventFunctionItem CheckCharacterAgeGroup => Instance[162];

		public static EventFunctionItem CheckCharacterGender => Instance[203];

		public static EventFunctionItem CheckCharacterResource => Instance[83];

		public static EventFunctionItem CheckCharacterFeature => Instance[84];

		public static EventFunctionItem CheckCharacterCurrentProfession => Instance[140];

		public static EventFunctionItem CheckCharacterSeniorityPercent => Instance[141];

		public static EventFunctionItem CheckCharacterInventoryByTemplate => Instance[85];

		public static EventFunctionItem CheckCharacterOnSettlementBlock => Instance[86];

		public static EventFunctionItem CheckCharacterInSettlementInfluenceRange => Instance[87];

		public static EventFunctionItem CheckCharacterInMapState => Instance[95];

		public static EventFunctionItem CheckCharacterInMapArea => Instance[96];

		public static EventFunctionItem CheckCharacterInMapBlockRange => Instance[168];

		public static EventFunctionItem CheckCharacterOnAnySettlement => Instance[97];

		public static EventFunctionItem CheckCharacterInAnySettlementInfluenceRange => Instance[98];

		public static EventFunctionItem CheckCharacterInSettlementArea => Instance[194];

		public static EventFunctionItem CheckCharacterFavorability => Instance[88];

		public static EventFunctionItem CheckCharacterFavorabilityType => Instance[142];

		public static EventFunctionItem CheckCharacterGrade => Instance[103];

		public static EventFunctionItem CheckCharacterSettlement => Instance[104];

		public static EventFunctionItem CheckCharacterHasItem => Instance[121];

		public static EventFunctionItem CheckCharacterMerchantType => Instance[122];

		public static EventFunctionItem CheckCharacterReadLifeSkillPageCount => Instance[131];

		public static EventFunctionItem CheckPreviousCombatResult => Instance[90];

		public static EventFunctionItem CheckSettlementInMapState => Instance[105];

		public static EventFunctionItem CheckSettlementInMapArea => Instance[106];

		public static EventFunctionItem CheckAreaSpiritualDebt => Instance[112];

		public static EventFunctionItem CheckAreaHasAdventure => Instance[204];

		public static EventFunctionItem CheckSectMainStoryValueExists => Instance[123];

		public static EventFunctionItem CheckItemType => Instance[136];

		public static EventFunctionItem CheckItemSubType => Instance[137];

		public static EventFunctionItem CheckItemTemplate => Instance[138];

		public static EventFunctionItem TryGetCharacterCurrentProfession => Instance[146];

		public static EventFunctionItem CheckCharacterPassMatcher => Instance[213];

		public static EventFunctionItem CheckCharacterCanTeachTaiwuProfession => Instance[148];

		public static EventFunctionItem CheckCharacterCanTeachTaiwuProfessionSkillUnlock => Instance[163];

		public static EventFunctionItem CheckCharacterConsummateLevel => Instance[157];

		public static EventFunctionItem CheckIsDreamBack => Instance[158];

		public static EventFunctionItem CheckCharacterAlive => Instance[159];

		public static EventFunctionItem CreateFixedSkillBook => Instance[166];

		public static EventFunctionItem CheckAdventureParameterCount => Instance[169];

		public static EventFunctionItem CheckMovePoint => Instance[170];

		public static EventFunctionItem CheckCurrMonth => Instance[171];

		public static EventFunctionItem CheckCharacterKidnapSpecificGender => Instance[172];

		public static EventFunctionItem CheckCharacterKidnapSpecificAgeGroup => Instance[173];

		public static EventFunctionItem CheckCharacterKidnapSpecificId => Instance[174];

		public static EventFunctionItem CheckCharacterTeammateSpecificIdGender => Instance[175];

		public static EventFunctionItem CheckCharacterTeammateSpecificIdAgeGroup => Instance[176];

		public static EventFunctionItem CheckCharacterTeammateSpecificIdId => Instance[177];

		public static EventFunctionItem CheckCharacterExp => Instance[178];

		public static EventFunctionItem CheckCharacterReadCombatSkillPageCount => Instance[179];

		public static EventFunctionItem CheckCharacterCombatSkillBreakout => Instance[180];

		public static EventFunctionItem CheckSettlementApprovingRate => Instance[190];

		public static EventFunctionItem CheckStateHasSettlementType => Instance[193];

		public static EventFunctionItem CheckAdventureTemplate => Instance[202];

		public static EventFunctionItem CheckAdventurePerMoveCount => Instance[181];

		public static EventFunctionItem CheckAdventurePerCostMovePoint => Instance[182];

		public static EventFunctionItem CheckAdventureElementVisible => Instance[183];

		public static EventFunctionItem SwitchEmeiBlood => Instance[184];

		public static EventFunctionItem CheckAdventureCharacterGroup => Instance[185];

		public static EventFunctionItem CheckAdventureElementGroup => Instance[186];

		public static EventFunctionItem GetCharacterConsummateLevel => Instance[191];

		public static EventFunctionItem OpenYuanshanMiniGame => Instance[195];

		public static EventFunctionItem ProcessYuanshanMiniGameResults => Instance[196];

		public static EventFunctionItem SpecifyXiangshuInfectionValue => Instance[200];

		public static EventFunctionItem ChangeXiangshuInfectionValue => Instance[201];

		public static EventFunctionItem SetCharacterMarriageStyleOne => Instance[205];

		public static EventFunctionItem SetCharacterMarriageStyleTwo => Instance[206];

		public static EventFunctionItem IsVitalInPrison => Instance[207];

		public static EventFunctionItem SetVitalInPrison => Instance[208];

		public static EventFunctionItem PlayVitalAnim => Instance[209];

		public static EventFunctionItem GetCharacterBySettlementGradeAndAge => Instance[210];

		public static EventFunctionItem AreVitalsDemon => Instance[211];

		public static EventFunctionItem GetCurrentVitalIndex => Instance[212];

		public static EventFunctionItem InitThreeVitals => Instance[214];

		public static EventFunctionItem CheckAdventureParameter => Instance[219];

		public static EventFunctionItem SetAdventureParameter => Instance[220];

		public static EventFunctionItem ChangeAdventureParameter => Instance[221];

		public static EventFunctionItem CheckAdventureParameterStartWith => Instance[222];

		public static EventFunctionItem SetAdventureParameterStartWith => Instance[223];

		public static EventFunctionItem ChangeAdventureParameterStartWith => Instance[224];

		public static EventFunctionItem AdventureCheckProb => Instance[225];

		public static EventFunctionItem CheckAdventureElementCount => Instance[228];

		public static EventFunctionItem CheckAdventureElementTagCount => Instance[229];

		public static EventFunctionItem CheckAdventureParameterIsMax => Instance[230];

		public static EventFunctionItem CheckAdventureParameterIsMin => Instance[231];

		public static EventFunctionItem CheckAdventureElementInElement => Instance[232];

		public static EventFunctionItem CheckAdventureTaiwuInElement => Instance[233];

		public static EventFunctionItem CheckAdventureTaiwuInBlockGroup => Instance[234];

		public static EventFunctionItem AdventureCreateItem => Instance[235];

		public static EventFunctionItem AdventureRemoveItem => Instance[236];

		public static EventFunctionItem AdventureCheckUseItem => Instance[237];

		public static EventFunctionItem AddJieqingMaskCharId => Instance[238];

		public static EventFunctionItem RemoveJieqingMaskCharId => Instance[239];

		public static EventFunctionItem AdventureSetAutoDeleteDate => Instance[240];

		public static EventFunctionItem AdventureExit => Instance[241];

		public static EventFunctionItem DestroyEnemyNest => Instance[242];

		public static EventFunctionItem SectMainStoryUnlockUI => Instance[244];

		public static EventFunctionItem CheckAdventureElementVisibleWithTag => Instance[245];

		public static EventFunctionItem CheckAdventureElementGroupWithTag => Instance[246];

		public static EventFunctionItem CheckAdventureElementInElementWithTag => Instance[247];

		public static EventFunctionItem CheckAdventureTaiwuInElementWithTag => Instance[248];

		public static EventFunctionItem GetCurrentFaith => Instance[249];

		public static EventFunctionItem GetFaithLevel => Instance[250];

		public static EventFunctionItem GetFuyuFaithTime => Instance[251];

		public static EventFunctionItem OpenFuyuFaithPanel => Instance[252];

		public static EventFunctionItem OpenFuyuGiftPanel => Instance[253];

		public static EventFunctionItem ApplyFuyuFaith => Instance[254];

		public static EventFunctionItem ReadSelectResultCount => Instance[255];

		public static EventFunctionItem TryGetMaxAcceptableFuyuFaith => Instance[256];

		public static EventFunctionItem CheckTaiwuHasFuyuFaith => Instance[257];

		public static EventFunctionItem CheckCharacterFuyuFaith => Instance[258];

		public static EventFunctionItem AdventureElementFillByGroup => Instance[259];

		public static EventFunctionItem AdventureElementFillByElement => Instance[260];

		public static EventFunctionItem AdventureDelete => Instance[264];

		public static EventFunctionItem CheckAdventureElementParameter => Instance[265];

		public static EventFunctionItem SetAdventureElementParameter => Instance[266];

		public static EventFunctionItem ChangeAdventureElementParameter => Instance[267];

		public static EventFunctionItem AdventureChangeElementCountAtTaiwuLocation => Instance[268];

		public static EventFunctionItem AdventureClearElementAtTaiwuLocation => Instance[269];

		public static EventFunctionItem AdventureClearElement => Instance[270];
	}

	public static EventFunction Instance = new EventFunction();

	private readonly HashSet<string> RequiredFields = new HashSet<string> { "ParameterTypes", "ReturnValue", "FollowUp", "RequiredPreviousCommands", "TemplateId", "Name", "Desc", "InGameHint" };

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
		_dataArray.Add(new EventFunctionItem(0, EEventFunctionType.Basic, EEventFunctionConditionSubType.Invalid, 0, 1, new int[1] { 3 }, new int[1] { 2 }, -1, indentNext: true, 5, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 3));
		_dataArray.Add(new EventFunctionItem(1, EEventFunctionType.Basic, EEventFunctionConditionSubType.Invalid, 4, 5, new int[0], new int[0], -1, indentNext: true, -1, canCreateManually: true, new List<int> { 0, 2 }, allowedInCondition: false, isTransition: false, allowExternalUsage: true, 6));
		_dataArray.Add(new EventFunctionItem(2, EEventFunctionType.Basic, EEventFunctionConditionSubType.Invalid, 7, 8, new int[1] { 3 }, new int[1] { 2 }, -1, indentNext: true, -1, canCreateManually: true, new List<int> { 0, 2 }, allowedInCondition: false, isTransition: false, allowExternalUsage: true, 9));
		_dataArray.Add(new EventFunctionItem(3, EEventFunctionType.Basic, EEventFunctionConditionSubType.Invalid, 10, 11, new int[0], new int[0], -1, indentNext: true, 5, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 12));
		_dataArray.Add(new EventFunctionItem(4, EEventFunctionType.Basic, EEventFunctionConditionSubType.Invalid, 13, 14, new int[0], new int[0], -1, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 15));
		_dataArray.Add(new EventFunctionItem(5, EEventFunctionType.Basic, EEventFunctionConditionSubType.Invalid, 16, 17, new int[0], new int[0], -1, indentNext: false, -1, canCreateManually: false, new List<int> { 0, 1, 2, 3, 109, 110 }, allowedInCondition: false, isTransition: false, allowExternalUsage: true, 18));
		_dataArray.Add(new EventFunctionItem(6, EEventFunctionType.Basic, EEventFunctionConditionSubType.Invalid, 19, 20, new int[0], new int[0], -1, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 21));
		_dataArray.Add(new EventFunctionItem(7, EEventFunctionType.Basic, EEventFunctionConditionSubType.Invalid, 22, 23, new int[1] { 4 }, new int[1] { 24 }, -1, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 25));
		_dataArray.Add(new EventFunctionItem(8, EEventFunctionType.Basic, EEventFunctionConditionSubType.Invalid, 26, 27, new int[1] { 4 }, new int[1] { 24 }, -1, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 28));
		_dataArray.Add(new EventFunctionItem(9, EEventFunctionType.Basic, EEventFunctionConditionSubType.Invalid, 29, 30, new int[0], new int[0], -1, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 31));
		_dataArray.Add(new EventFunctionItem(10, EEventFunctionType.Basic, EEventFunctionConditionSubType.Invalid, 32, 33, new int[1], new int[1] { 34 }, 0, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 35));
		_dataArray.Add(new EventFunctionItem(11, EEventFunctionType.Basic, EEventFunctionConditionSubType.Invalid, 36, 37, new int[2] { 1, 1 }, new int[2] { 38, 39 }, 1, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 40));
		_dataArray.Add(new EventFunctionItem(12, EEventFunctionType.Basic, EEventFunctionConditionSubType.Invalid, 41, 42, new int[1] { 1 }, new int[1] { 43 }, 3, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 44));
		_dataArray.Add(new EventFunctionItem(13, EEventFunctionType.Basic, EEventFunctionConditionSubType.Invalid, 45, 46, new int[1] { 5 }, new int[1] { 47 }, -1, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: true, allowExternalUsage: true, 48));
		_dataArray.Add(new EventFunctionItem(14, EEventFunctionType.Basic, EEventFunctionConditionSubType.Invalid, 74, 75, new int[1] { 4 }, new int[1] { 76 }, -1, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 77));
		_dataArray.Add(new EventFunctionItem(15, EEventFunctionType.Basic, EEventFunctionConditionSubType.Invalid, 78, 79, new int[1] { 4 }, new int[1] { 80 }, -1, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 81));
		_dataArray.Add(new EventFunctionItem(16, EEventFunctionType.UI, EEventFunctionConditionSubType.Invalid, 82, 83, new int[1] { 4 }, new int[1] { 84 }, -1, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 85));
		_dataArray.Add(new EventFunctionItem(17, EEventFunctionType.UI, EEventFunctionConditionSubType.Invalid, 86, 87, new int[0], new int[0], -1, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 88));
		_dataArray.Add(new EventFunctionItem(18, EEventFunctionType.Behavior, EEventFunctionConditionSubType.Invalid, 97, 98, new int[3] { 6, 19, 1 }, new int[3] { 99, 100, 101 }, -1, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 102));
		_dataArray.Add(new EventFunctionItem(19, EEventFunctionType.Behavior, EEventFunctionConditionSubType.Invalid, 103, 104, new int[3] { 6, 19, 1 }, new int[3] { 105, 106, 107 }, -1, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 108));
		_dataArray.Add(new EventFunctionItem(20, EEventFunctionType.Behavior, EEventFunctionConditionSubType.Invalid, 109, 110, new int[4] { 6, 14, 3, 1 }, new int[4] { 111, 112, 113, 101 }, -1, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 114));
		_dataArray.Add(new EventFunctionItem(21, EEventFunctionType.Behavior, EEventFunctionConditionSubType.Invalid, 115, 116, new int[4] { 6, 14, 3, 1 }, new int[4] { 117, 118, 113, 107 }, -1, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 119));
		_dataArray.Add(new EventFunctionItem(22, EEventFunctionType.Behavior, EEventFunctionConditionSubType.Invalid, 120, 121, new int[1] { 6 }, new int[0], -1, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 122));
		_dataArray.Add(new EventFunctionItem(23, EEventFunctionType.Behavior, EEventFunctionConditionSubType.Invalid, 123, 124, new int[4] { 6, 15, 1, 1 }, new int[4] { 125, 126, 127, 101 }, -1, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 128));
		_dataArray.Add(new EventFunctionItem(24, EEventFunctionType.Behavior, EEventFunctionConditionSubType.Invalid, 129, 130, new int[4] { 6, 15, 1, 1 }, new int[4] { 131, 132, 127, 107 }, -1, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 133));
		_dataArray.Add(new EventFunctionItem(25, EEventFunctionType.Behavior, EEventFunctionConditionSubType.Invalid, 134, 135, new int[1] { 6 }, new int[0], -1, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 136));
		_dataArray.Add(new EventFunctionItem(26, EEventFunctionType.Behavior, EEventFunctionConditionSubType.Invalid, 137, 138, new int[2] { 6, 1 }, new int[2] { 139, 101 }, -1, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 140));
		_dataArray.Add(new EventFunctionItem(27, EEventFunctionType.Behavior, EEventFunctionConditionSubType.Invalid, 141, 142, new int[2] { 6, 1 }, new int[2] { 143, 107 }, -1, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 144));
		_dataArray.Add(new EventFunctionItem(28, EEventFunctionType.Behavior, EEventFunctionConditionSubType.Invalid, 145, 146, new int[2] { 6, 1 }, new int[2] { 147, 101 }, -1, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 148));
		_dataArray.Add(new EventFunctionItem(29, EEventFunctionType.Behavior, EEventFunctionConditionSubType.Invalid, 149, 150, new int[2] { 6, 1 }, new int[2] { 151, 107 }, -1, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 152));
		_dataArray.Add(new EventFunctionItem(30, EEventFunctionType.Behavior, EEventFunctionConditionSubType.Invalid, 153, 154, new int[2] { 6, 1 }, new int[2] { 155, 101 }, -1, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 156));
		_dataArray.Add(new EventFunctionItem(31, EEventFunctionType.Behavior, EEventFunctionConditionSubType.Invalid, 157, 158, new int[2] { 6, 1 }, new int[2] { 159, 107 }, -1, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 160));
		_dataArray.Add(new EventFunctionItem(32, EEventFunctionType.Behavior, EEventFunctionConditionSubType.Invalid, 161, 162, new int[4] { 6, 6, 1, 1 }, new int[4] { 163, 164, 165, 166 }, -1, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 167));
		_dataArray.Add(new EventFunctionItem(33, EEventFunctionType.Behavior, EEventFunctionConditionSubType.Invalid, 168, 169, new int[4] { 6, 6, 1, 63 }, new int[0], -1, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 170));
		_dataArray.Add(new EventFunctionItem(34, EEventFunctionType.Behavior, EEventFunctionConditionSubType.Invalid, 171, 172, new int[3] { 6, 29, 3 }, new int[3] { 173, 174, 175 }, -1, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 176));
		_dataArray.Add(new EventFunctionItem(35, EEventFunctionType.Behavior, EEventFunctionConditionSubType.Invalid, 177, 178, new int[2] { 6, 29 }, new int[0], -1, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 179));
		_dataArray.Add(new EventFunctionItem(36, EEventFunctionType.Behavior, EEventFunctionConditionSubType.Invalid, 180, 181, new int[3] { 6, 6, 7 }, new int[3] { 182, 183, 184 }, -1, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 185));
		_dataArray.Add(new EventFunctionItem(37, EEventFunctionType.Behavior, EEventFunctionConditionSubType.Invalid, 186, 187, new int[3] { 6, 6, 3 }, new int[3] { 182, 183, 188 }, -1, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 189));
		_dataArray.Add(new EventFunctionItem(38, EEventFunctionType.Behavior, EEventFunctionConditionSubType.Invalid, 190, 191, new int[2] { 6, 6 }, new int[2] { 192, 193 }, -1, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 194));
		_dataArray.Add(new EventFunctionItem(39, EEventFunctionType.Behavior, EEventFunctionConditionSubType.Invalid, 195, 196, new int[1] { 6 }, new int[0], -1, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 197));
		_dataArray.Add(new EventFunctionItem(40, EEventFunctionType.Behavior, EEventFunctionConditionSubType.Invalid, 198, 199, new int[3] { 6, 6, 31 }, new int[3] { 200, 201, 202 }, -1, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 203));
		_dataArray.Add(new EventFunctionItem(41, EEventFunctionType.Behavior, EEventFunctionConditionSubType.Invalid, 209, 210, new int[3] { 6, 7, 1 }, new int[0], -1, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 211));
		_dataArray.Add(new EventFunctionItem(42, EEventFunctionType.Behavior, EEventFunctionConditionSubType.Invalid, 219, 220, new int[5] { 6, 6, 7, 1, 3 }, new int[5] { 221, 206, 222, 223, 224 }, -1, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 225));
		_dataArray.Add(new EventFunctionItem(43, EEventFunctionType.Behavior, EEventFunctionConditionSubType.Invalid, 240, 241, new int[3] { 6, 24, 1 }, new int[0], -1, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 242));
		_dataArray.Add(new EventFunctionItem(44, EEventFunctionType.Behavior, EEventFunctionConditionSubType.Invalid, 243, 244, new int[3] { 6, 25, 1 }, new int[0], -1, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 245));
		_dataArray.Add(new EventFunctionItem(45, EEventFunctionType.Behavior, EEventFunctionConditionSubType.Invalid, 246, 247, new int[2] { 6, 35 }, new int[0], -1, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 248));
		_dataArray.Add(new EventFunctionItem(46, EEventFunctionType.Behavior, EEventFunctionConditionSubType.Invalid, 249, 250, new int[2] { 6, 36 }, new int[0], -1, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 251));
		_dataArray.Add(new EventFunctionItem(47, EEventFunctionType.Behavior, EEventFunctionConditionSubType.Invalid, 252, 253, new int[1] { 30 }, new int[0], -1, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 254));
		_dataArray.Add(new EventFunctionItem(48, EEventFunctionType.Behavior, EEventFunctionConditionSubType.Invalid, 255, 256, new int[1] { 6 }, new int[0], -1, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 257));
		_dataArray.Add(new EventFunctionItem(49, EEventFunctionType.Behavior, EEventFunctionConditionSubType.Invalid, 258, 259, new int[2] { 6, 10 }, new int[0], -1, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 260));
		_dataArray.Add(new EventFunctionItem(50, EEventFunctionType.Behavior, EEventFunctionConditionSubType.Invalid, 261, 262, new int[1] { 6 }, new int[0], -1, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 263));
		_dataArray.Add(new EventFunctionItem(51, EEventFunctionType.Behavior, EEventFunctionConditionSubType.Invalid, 264, 265, new int[1] { 1 }, new int[0], -1, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 266));
		_dataArray.Add(new EventFunctionItem(52, EEventFunctionType.Behavior, EEventFunctionConditionSubType.Invalid, 267, 268, new int[1] { 32 }, new int[0], -1, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: false, 269));
		_dataArray.Add(new EventFunctionItem(53, EEventFunctionType.Behavior, EEventFunctionConditionSubType.Invalid, 270, 271, new int[1] { 33 }, new int[0], -1, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: false, 272));
		_dataArray.Add(new EventFunctionItem(54, EEventFunctionType.Behavior, EEventFunctionConditionSubType.Invalid, 273, 274, new int[2] { 12, 1 }, new int[0], -1, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 275));
		_dataArray.Add(new EventFunctionItem(55, EEventFunctionType.Behavior, EEventFunctionConditionSubType.Invalid, 276, 277, new int[2] { 10, 1 }, new int[0], -1, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 278));
		_dataArray.Add(new EventFunctionItem(56, EEventFunctionType.Behavior, EEventFunctionConditionSubType.Invalid, 279, 280, new int[2] { 10, 1 }, new int[0], -1, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 281));
		_dataArray.Add(new EventFunctionItem(57, EEventFunctionType.Behavior, EEventFunctionConditionSubType.Invalid, 282, 283, new int[1] { 9 }, new int[0], -1, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 284));
		_dataArray.Add(new EventFunctionItem(58, EEventFunctionType.Behavior, EEventFunctionConditionSubType.Invalid, 285, 286, new int[1] { 11 }, new int[0], -1, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 287));
		_dataArray.Add(new EventFunctionItem(59, EEventFunctionType.Behavior, EEventFunctionConditionSubType.Invalid, 288, 289, new int[3] { 6, 10, 1 }, new int[3] { 290, 291, 292 }, -1, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 293));
	}

	private void CreateItems1()
	{
		_dataArray.Add(new EventFunctionItem(60, EEventFunctionType.Behavior, EEventFunctionConditionSubType.Invalid, 294, 295, new int[2] { 6, 3 }, new int[2] { 296, 297 }, -1, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 298));
		_dataArray.Add(new EventFunctionItem(61, EEventFunctionType.Behavior, EEventFunctionConditionSubType.Invalid, 304, 305, new int[2] { 37, 1 }, new int[2] { 306, 107 }, -1, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 307));
		_dataArray.Add(new EventFunctionItem(62, EEventFunctionType.Behavior, EEventFunctionConditionSubType.Invalid, 308, 309, new int[1] { 8 }, new int[0], 7, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 310));
		_dataArray.Add(new EventFunctionItem(63, EEventFunctionType.Behavior, EEventFunctionConditionSubType.Invalid, 311, 312, new int[2] { 34, 34 }, new int[2] { 313, 314 }, 7, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 315));
		_dataArray.Add(new EventFunctionItem(64, EEventFunctionType.Behavior, EEventFunctionConditionSubType.Invalid, 321, 322, new int[3] { 6, 35, 1 }, new int[3] { 323, 324, 325 }, -1, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 326));
		_dataArray.Add(new EventFunctionItem(65, EEventFunctionType.Behavior, EEventFunctionConditionSubType.Invalid, 212, 213, new int[4] { 6, 7, 1, 3 }, new int[4] { 214, 215, 216, 217 }, -1, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 218));
		_dataArray.Add(new EventFunctionItem(66, EEventFunctionType.Behavior, EEventFunctionConditionSubType.Invalid, 231, 232, new int[3] { 6, 21, 1 }, new int[3] { 233, 234, 107 }, -1, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 235));
		_dataArray.Add(new EventFunctionItem(67, EEventFunctionType.Behavior, EEventFunctionConditionSubType.Invalid, 327, 328, new int[2] { 40, 9 }, new int[0], -1, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 329));
		_dataArray.Add(new EventFunctionItem(68, EEventFunctionType.Behavior, EEventFunctionConditionSubType.Invalid, 363, 364, new int[2] { 39, 38 }, new int[0], -1, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: false, 365));
		_dataArray.Add(new EventFunctionItem(69, EEventFunctionType.Behavior, EEventFunctionConditionSubType.Invalid, 366, 367, new int[2] { 39, 38 }, new int[0], -1, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: false, 368));
		_dataArray.Add(new EventFunctionItem(70, EEventFunctionType.Behavior, EEventFunctionConditionSubType.Invalid, 369, 370, new int[1] { 39 }, new int[0], -1, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: false, 371));
		_dataArray.Add(new EventFunctionItem(71, EEventFunctionType.Condition, EEventFunctionConditionSubType.GeneralCondition, 522, 523, new int[1] { 3 }, new int[0], 3, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 524));
		_dataArray.Add(new EventFunctionItem(72, EEventFunctionType.Condition, EEventFunctionConditionSubType.GeneralCondition, 538, 539, new int[2] { 41, 32 }, new int[0], 3, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 540));
		_dataArray.Add(new EventFunctionItem(73, EEventFunctionType.Condition, EEventFunctionConditionSubType.GeneralCondition, 541, 542, new int[1] { 38 }, new int[0], 3, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 543));
		_dataArray.Add(new EventFunctionItem(74, EEventFunctionType.Condition, EEventFunctionConditionSubType.GeneralCondition, 544, 545, new int[1] { 39 }, new int[0], 3, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 546));
		_dataArray.Add(new EventFunctionItem(75, EEventFunctionType.Condition, EEventFunctionConditionSubType.GeneralCondition, 547, 548, new int[2] { 41, 1 }, new int[0], 3, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 549));
		_dataArray.Add(new EventFunctionItem(76, EEventFunctionType.Condition, EEventFunctionConditionSubType.GeneralCondition, 553, 554, new int[4] { 6, 19, 41, 1 }, new int[0], 3, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 555));
		_dataArray.Add(new EventFunctionItem(77, EEventFunctionType.Condition, EEventFunctionConditionSubType.GeneralCondition, 556, 557, new int[4] { 6, 19, 41, 1 }, new int[0], 3, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 558));
		_dataArray.Add(new EventFunctionItem(78, EEventFunctionType.Condition, EEventFunctionConditionSubType.GeneralCondition, 559, 560, new int[4] { 6, 25, 41, 1 }, new int[0], 3, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 561));
		_dataArray.Add(new EventFunctionItem(79, EEventFunctionType.Condition, EEventFunctionConditionSubType.GeneralCondition, 562, 563, new int[4] { 6, 25, 41, 1 }, new int[0], 3, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 564));
		_dataArray.Add(new EventFunctionItem(80, EEventFunctionType.Condition, EEventFunctionConditionSubType.GeneralCondition, 565, 566, new int[4] { 6, 24, 41, 1 }, new int[0], 3, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 567));
		_dataArray.Add(new EventFunctionItem(81, EEventFunctionType.Condition, EEventFunctionConditionSubType.GeneralCondition, 568, 569, new int[4] { 6, 24, 41, 1 }, new int[0], 3, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 570));
		_dataArray.Add(new EventFunctionItem(82, EEventFunctionType.Condition, EEventFunctionConditionSubType.GeneralCondition, 571, 572, new int[4] { 6, 18, 41, 1 }, new int[0], 3, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 573));
		_dataArray.Add(new EventFunctionItem(83, EEventFunctionType.Condition, EEventFunctionConditionSubType.GeneralCondition, 592, 593, new int[4] { 6, 21, 41, 1 }, new int[0], 3, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 594));
		_dataArray.Add(new EventFunctionItem(84, EEventFunctionType.Condition, EEventFunctionConditionSubType.GeneralCondition, 595, 596, new int[2] { 6, 29 }, new int[0], 3, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 597));
		_dataArray.Add(new EventFunctionItem(85, EEventFunctionType.Condition, EEventFunctionConditionSubType.GeneralCondition, 608, 609, new int[2] { 6, 8 }, new int[0], 3, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 610));
		_dataArray.Add(new EventFunctionItem(86, EEventFunctionType.Condition, EEventFunctionConditionSubType.GeneralCondition, 611, 612, new int[2] { 6, 10 }, new int[0], 3, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 613));
		_dataArray.Add(new EventFunctionItem(87, EEventFunctionType.Condition, EEventFunctionConditionSubType.GeneralCondition, 614, 615, new int[2] { 6, 10 }, new int[0], 3, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 616));
		_dataArray.Add(new EventFunctionItem(88, EEventFunctionType.Condition, EEventFunctionConditionSubType.GeneralCondition, 637, 638, new int[4] { 6, 6, 41, 1 }, new int[0], 3, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 639));
		_dataArray.Add(new EventFunctionItem(89, EEventFunctionType.Behavior, EEventFunctionConditionSubType.Invalid, 356, 357, new int[4] { 6, 42, 5, 3 }, new int[4] { 358, 359, 360, 361 }, -1, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: true, allowExternalUsage: true, 362));
		_dataArray.Add(new EventFunctionItem(90, EEventFunctionType.Condition, EEventFunctionConditionSubType.GeneralCondition, 658, 659, new int[1] { 43 }, new int[0], 3, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 660));
		_dataArray.Add(new EventFunctionItem(91, EEventFunctionType.Basic, EEventFunctionConditionSubType.Invalid, 53, 54, new int[1] { 48 }, new int[1] { 55 }, -1, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 56));
		_dataArray.Add(new EventFunctionItem(92, EEventFunctionType.Behavior, EEventFunctionConditionSubType.Invalid, 335, 336, new int[2] { 45, 3 }, new int[2] { 337, 338 }, 6, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 339));
		_dataArray.Add(new EventFunctionItem(93, EEventFunctionType.Behavior, EEventFunctionConditionSubType.Invalid, 340, 341, new int[1] { 44 }, new int[0], 0, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 342));
		_dataArray.Add(new EventFunctionItem(94, EEventFunctionType.Basic, EEventFunctionConditionSubType.Invalid, 49, 50, new int[2] { 1, 5 }, new int[2] { 51, 47 }, -1, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 52));
		_dataArray.Add(new EventFunctionItem(95, EEventFunctionType.Condition, EEventFunctionConditionSubType.GeneralCondition, 617, 618, new int[2] { 6, 47 }, new int[0], 3, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 619));
		_dataArray.Add(new EventFunctionItem(96, EEventFunctionType.Condition, EEventFunctionConditionSubType.GeneralCondition, 620, 621, new int[2] { 6, 12 }, new int[0], 3, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 622));
		_dataArray.Add(new EventFunctionItem(97, EEventFunctionType.Condition, EEventFunctionConditionSubType.GeneralCondition, 628, 629, new int[1] { 6 }, new int[0], 3, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 630));
		_dataArray.Add(new EventFunctionItem(98, EEventFunctionType.Condition, EEventFunctionConditionSubType.GeneralCondition, 631, 632, new int[1] { 6 }, new int[0], 3, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 633));
		_dataArray.Add(new EventFunctionItem(99, EEventFunctionType.Behavior, EEventFunctionConditionSubType.Invalid, 343, 344, new int[1] { 46 }, new int[0], 6, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 345));
		_dataArray.Add(new EventFunctionItem(100, EEventFunctionType.Behavior, EEventFunctionConditionSubType.Invalid, 353, 354, new int[2] { 6, 9 }, new int[0], -1, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 355));
		_dataArray.Add(new EventFunctionItem(101, EEventFunctionType.Basic, EEventFunctionConditionSubType.Invalid, 57, 58, new int[3] { 11, 4, 0 }, new int[3] { 59, 60, 61 }, -1, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: false, 62));
		_dataArray.Add(new EventFunctionItem(102, EEventFunctionType.Basic, EEventFunctionConditionSubType.Invalid, 63, 64, new int[2] { 11, 4 }, new int[2] { 59, 60 }, 0, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: false, 65));
		_dataArray.Add(new EventFunctionItem(103, EEventFunctionType.Condition, EEventFunctionConditionSubType.GeneralCondition, 643, 644, new int[3] { 6, 41, 1 }, new int[0], 3, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 645));
		_dataArray.Add(new EventFunctionItem(104, EEventFunctionType.Condition, EEventFunctionConditionSubType.GeneralCondition, 646, 647, new int[2] { 6, 10 }, new int[0], 3, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 648));
		_dataArray.Add(new EventFunctionItem(105, EEventFunctionType.Condition, EEventFunctionConditionSubType.GeneralCondition, 661, 662, new int[2] { 10, 47 }, new int[0], 3, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 663));
		_dataArray.Add(new EventFunctionItem(106, EEventFunctionType.Condition, EEventFunctionConditionSubType.GeneralCondition, 664, 665, new int[2] { 10, 12 }, new int[0], 3, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 666));
		_dataArray.Add(new EventFunctionItem(107, EEventFunctionType.Condition, EEventFunctionConditionSubType.GeneralCondition, 574, 575, new int[2] { 6, 17 }, new int[0], 3, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 576));
		_dataArray.Add(new EventFunctionItem(108, EEventFunctionType.Condition, EEventFunctionConditionSubType.GeneralCondition, 577, 578, new int[3] { 6, 41, 1 }, new int[0], 3, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 579));
		_dataArray.Add(new EventFunctionItem(109, EEventFunctionType.Condition, EEventFunctionConditionSubType.GeneralCondition, 525, 526, new int[0], new int[0], 3, indentNext: true, 5, canCreateManually: true, new List<int>(), allowedInCondition: true, isTransition: false, allowExternalUsage: true, 527));
		_dataArray.Add(new EventFunctionItem(110, EEventFunctionType.Condition, EEventFunctionConditionSubType.GeneralCondition, 528, 529, new int[0], new int[0], 3, indentNext: true, 5, canCreateManually: true, new List<int>(), allowedInCondition: true, isTransition: false, allowExternalUsage: true, 530));
		_dataArray.Add(new EventFunctionItem(111, EEventFunctionType.Behavior, EEventFunctionConditionSubType.Invalid, 316, 317, new int[2] { 6, 6 }, new int[2] { 318, 319 }, 1, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 320));
		_dataArray.Add(new EventFunctionItem(112, EEventFunctionType.Condition, EEventFunctionConditionSubType.GeneralCondition, 667, 668, new int[3] { 12, 41, 1 }, new int[0], 3, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 669));
		_dataArray.Add(new EventFunctionItem(113, EEventFunctionType.Behavior, EEventFunctionConditionSubType.Invalid, 384, 385, new int[3] { 6, 1, 3 }, new int[3] { 386, 387, 388 }, 1, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 389));
		_dataArray.Add(new EventFunctionItem(114, EEventFunctionType.Behavior, EEventFunctionConditionSubType.Invalid, 346, 347, new int[4] { 50, 12, 49, 3 }, new int[4] { 348, 349, 350, 351 }, 6, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 352));
		_dataArray.Add(new EventFunctionItem(115, EEventFunctionType.Behavior, EEventFunctionConditionSubType.Invalid, 372, 373, new int[2] { 11, 3 }, new int[2] { 374, 375 }, -1, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: false, 376));
		_dataArray.Add(new EventFunctionItem(116, EEventFunctionType.Behavior, EEventFunctionConditionSubType.Invalid, 377, 378, new int[5] { 11, 3, 1, 3, 5 }, new int[5] { 379, 375, 380, 381, 382 }, -1, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: false, 383));
		_dataArray.Add(new EventFunctionItem(117, EEventFunctionType.Behavior, EEventFunctionConditionSubType.Invalid, 390, 391, new int[2] { 6, 1 }, new int[2] { 392, 393 }, -1, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 394));
		_dataArray.Add(new EventFunctionItem(118, EEventFunctionType.Behavior, EEventFunctionConditionSubType.Invalid, 398, 399, new int[5] { 6, 5, 3, 3, 25 }, new int[5] { 400, 360, 401, 402, 403 }, -1, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: true, allowExternalUsage: true, 404));
		_dataArray.Add(new EventFunctionItem(119, EEventFunctionType.Behavior, EEventFunctionConditionSubType.Invalid, 426, 427, new int[1] { 4 }, new int[1] { 428 }, -1, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 429));
	}

	private void CreateItems2()
	{
		_dataArray.Add(new EventFunctionItem(120, EEventFunctionType.Behavior, EEventFunctionConditionSubType.Invalid, 395, 396, new int[1] { 6 }, new int[0], -1, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 397));
		_dataArray.Add(new EventFunctionItem(121, EEventFunctionType.Condition, EEventFunctionConditionSubType.GeneralCondition, 649, 650, new int[2] { 6, 8 }, new int[0], 3, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 651));
		_dataArray.Add(new EventFunctionItem(122, EEventFunctionType.Condition, EEventFunctionConditionSubType.GeneralCondition, 652, 653, new int[2] { 6, 37 }, new int[0], 3, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 654));
		_dataArray.Add(new EventFunctionItem(123, EEventFunctionType.Condition, EEventFunctionConditionSubType.GeneralCondition, 673, 674, new int[3] { 11, 4, 4 }, new int[3] { 675, 60, 533 }, 3, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 676));
		_dataArray.Add(new EventFunctionItem(124, EEventFunctionType.Behavior, EEventFunctionConditionSubType.Invalid, 405, 406, new int[2] { 3, 5 }, new int[2] { 407, 408 }, -1, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 409));
		_dataArray.Add(new EventFunctionItem(125, EEventFunctionType.Behavior, EEventFunctionConditionSubType.Invalid, 419, 420, new int[0], new int[0], -1, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 421));
		_dataArray.Add(new EventFunctionItem(126, EEventFunctionType.Behavior, EEventFunctionConditionSubType.Invalid, 422, 423, new int[1] { 4 }, new int[1] { 424 }, -1, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 425));
		_dataArray.Add(new EventFunctionItem(127, EEventFunctionType.Behavior, EEventFunctionConditionSubType.Invalid, 430, 430, new int[5] { 6, 4, 22, 23, 3 }, new int[3] { 400, 431, 432 }, -1, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 433));
		_dataArray.Add(new EventFunctionItem(128, EEventFunctionType.Condition, EEventFunctionConditionSubType.GeneralCondition, 550, 551, new int[2] { 6, 46 }, new int[0], 3, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 552));
		_dataArray.Add(new EventFunctionItem(129, EEventFunctionType.Behavior, EEventFunctionConditionSubType.Invalid, 226, 227, new int[3] { 6, 21, 1 }, new int[3] { 228, 229, 101 }, -1, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 230));
		_dataArray.Add(new EventFunctionItem(130, EEventFunctionType.Behavior, EEventFunctionConditionSubType.Invalid, 236, 237, new int[5] { 6, 6, 21, 1, 3 }, new int[5] { 221, 206, 238, 223, 224 }, -1, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 239));
		_dataArray.Add(new EventFunctionItem(131, EEventFunctionType.Condition, EEventFunctionConditionSubType.GeneralCondition, 655, 656, new int[4] { 6, 51, 41, 1 }, new int[0], 3, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 657));
		_dataArray.Add(new EventFunctionItem(132, EEventFunctionType.Behavior, EEventFunctionConditionSubType.Invalid, 441, 441, new int[1] { 23 }, new int[1] { 442 }, -1, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 443));
		_dataArray.Add(new EventFunctionItem(133, EEventFunctionType.Behavior, EEventFunctionConditionSubType.Invalid, 444, 444, new int[1] { 8 }, new int[1] { 445 }, -1, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 446));
		_dataArray.Add(new EventFunctionItem(134, EEventFunctionType.Behavior, EEventFunctionConditionSubType.Invalid, 447, 447, new int[1] { 8 }, new int[1] { 445 }, -1, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 448));
		_dataArray.Add(new EventFunctionItem(135, EEventFunctionType.Behavior, EEventFunctionConditionSubType.Invalid, 449, 449, new int[3] { 6, 4, 3 }, new int[3] { 400, 431, 450 }, -1, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 451));
		_dataArray.Add(new EventFunctionItem(136, EEventFunctionType.Condition, EEventFunctionConditionSubType.GeneralCondition, 677, 678, new int[2] { 7, 22 }, new int[0], 3, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 679));
		_dataArray.Add(new EventFunctionItem(137, EEventFunctionType.Condition, EEventFunctionConditionSubType.GeneralCondition, 680, 681, new int[2] { 7, 23 }, new int[0], 3, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 682));
		_dataArray.Add(new EventFunctionItem(138, EEventFunctionType.Condition, EEventFunctionConditionSubType.GeneralCondition, 683, 684, new int[2] { 7, 8 }, new int[0], 3, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 685));
		_dataArray.Add(new EventFunctionItem(139, EEventFunctionType.Behavior, EEventFunctionConditionSubType.Invalid, 469, 469, new int[1] { 12 }, new int[0], -1, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: false, 470));
		_dataArray.Add(new EventFunctionItem(140, EEventFunctionType.Condition, EEventFunctionConditionSubType.GeneralCondition, 598, 599, new int[2] { 6, 52 }, new int[0], 3, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 600));
		_dataArray.Add(new EventFunctionItem(141, EEventFunctionType.Condition, EEventFunctionConditionSubType.GeneralCondition, 601, 602, new int[4] { 6, 52, 41, 1 }, new int[4] { 603, 604, 605, 606 }, 3, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 607));
		_dataArray.Add(new EventFunctionItem(142, EEventFunctionType.Condition, EEventFunctionConditionSubType.GeneralCondition, 640, 641, new int[4] { 6, 6, 41, 53 }, new int[0], 3, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 642));
		_dataArray.Add(new EventFunctionItem(143, EEventFunctionType.Behavior, EEventFunctionConditionSubType.Invalid, 410, 411, new int[3] { 3, 1, 1 }, new int[3] { 412, 413, 414 }, 6, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 415));
		_dataArray.Add(new EventFunctionItem(144, EEventFunctionType.Behavior, EEventFunctionConditionSubType.Invalid, 416, 417, new int[2] { 3, 1 }, new int[2] { 412, 413 }, 1, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 418));
		_dataArray.Add(new EventFunctionItem(145, EEventFunctionType.Behavior, EEventFunctionConditionSubType.Invalid, 330, 331, new int[2] { 54, 12 }, new int[2] { 332, 333 }, -1, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 334));
		_dataArray.Add(new EventFunctionItem(146, EEventFunctionType.Condition, EEventFunctionConditionSubType.GeneralCondition, 686, 687, new int[2] { 6, 4 }, new int[2] { 688, 533 }, 3, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 689));
		_dataArray.Add(new EventFunctionItem(147, EEventFunctionType.Behavior, EEventFunctionConditionSubType.Invalid, 452, 453, new int[2] { 6, 52 }, new int[0], -1, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 454));
		_dataArray.Add(new EventFunctionItem(148, EEventFunctionType.Condition, EEventFunctionConditionSubType.GeneralCondition, 693, 694, new int[1] { 6 }, new int[0], 3, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 695));
		_dataArray.Add(new EventFunctionItem(149, EEventFunctionType.Behavior, EEventFunctionConditionSubType.Invalid, 462, 463, new int[4] { 55, 10, 3, 3 }, new int[4] { 464, 465, 466, 467 }, -1, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 468));
		_dataArray.Add(new EventFunctionItem(150, EEventFunctionType.Behavior, EEventFunctionConditionSubType.Invalid, 471, 472, new int[0], new int[0], -1, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: false, 473));
		_dataArray.Add(new EventFunctionItem(151, EEventFunctionType.Behavior, EEventFunctionConditionSubType.Invalid, 474, 474, new int[2] { 12, 37 }, new int[2] { 475, 306 }, -1, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: false, 476));
		_dataArray.Add(new EventFunctionItem(152, EEventFunctionType.Behavior, EEventFunctionConditionSubType.Invalid, 477, 477, new int[1] { 12 }, new int[1] { 475 }, -1, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: false, 478));
		_dataArray.Add(new EventFunctionItem(153, EEventFunctionType.Behavior, EEventFunctionConditionSubType.Invalid, 510, 510, new int[1] { 56 }, new int[1] { 511 }, 7, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: false, 512));
		_dataArray.Add(new EventFunctionItem(154, EEventFunctionType.Behavior, EEventFunctionConditionSubType.Invalid, 486, 487, new int[2] { 12, 10 }, new int[0], 10, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 488));
		_dataArray.Add(new EventFunctionItem(155, EEventFunctionType.Condition, EEventFunctionConditionSubType.GeneralCondition, 535, 536, new int[1] { 1 }, new int[0], 3, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 537));
		_dataArray.Add(new EventFunctionItem(156, EEventFunctionType.Behavior, EEventFunctionConditionSubType.Invalid, 513, 513, new int[2] { 3, 4 }, new int[2] { 514, 515 }, -1, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 516));
		_dataArray.Add(new EventFunctionItem(157, EEventFunctionType.Condition, EEventFunctionConditionSubType.GeneralCondition, 699, 700, new int[3] { 6, 41, 1 }, new int[0], 3, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 701));
		_dataArray.Add(new EventFunctionItem(158, EEventFunctionType.Condition, EEventFunctionConditionSubType.GeneralCondition, 702, 703, new int[0], new int[0], 3, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 704));
		_dataArray.Add(new EventFunctionItem(159, EEventFunctionType.Condition, EEventFunctionConditionSubType.GeneralCondition, 705, 706, new int[1] { 6 }, new int[0], 3, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 707));
		_dataArray.Add(new EventFunctionItem(160, EEventFunctionType.Condition, EEventFunctionConditionSubType.GeneralCondition, 580, 581, new int[3] { 6, 41, 1 }, new int[0], 3, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 582));
		_dataArray.Add(new EventFunctionItem(161, EEventFunctionType.Condition, EEventFunctionConditionSubType.GeneralCondition, 583, 584, new int[3] { 6, 41, 1 }, new int[0], 3, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 585));
		_dataArray.Add(new EventFunctionItem(162, EEventFunctionType.Condition, EEventFunctionConditionSubType.GeneralCondition, 586, 587, new int[3] { 6, 41, 57 }, new int[0], 3, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 588));
		_dataArray.Add(new EventFunctionItem(163, EEventFunctionType.Condition, EEventFunctionConditionSubType.GeneralCondition, 696, 697, new int[2] { 6, 52 }, new int[0], 3, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 698));
		_dataArray.Add(new EventFunctionItem(164, EEventFunctionType.Behavior, EEventFunctionConditionSubType.Invalid, 517, 517, new int[2] { 6, 1 }, new int[2] { 400, 518 }, -1, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 519));
		_dataArray.Add(new EventFunctionItem(165, EEventFunctionType.Behavior, EEventFunctionConditionSubType.Invalid, 520, 520, new int[0], new int[0], -1, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 521));
		_dataArray.Add(new EventFunctionItem(166, EEventFunctionType.Behavior, EEventFunctionConditionSubType.Invalid, 708, 709, new int[1] { 8 }, new int[0], 7, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 710));
		_dataArray.Add(new EventFunctionItem(167, EEventFunctionType.Behavior, EEventFunctionConditionSubType.Invalid, 434, 435, new int[4] { 9, 1, 1, 58 }, new int[4] { 436, 437, 438, 439 }, 9, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 440));
		_dataArray.Add(new EventFunctionItem(168, EEventFunctionType.Condition, EEventFunctionConditionSubType.GeneralCondition, 623, 624, new int[3] { 6, 9, 1 }, new int[3] { 625, 626, 393 }, 3, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 627));
		_dataArray.Add(new EventFunctionItem(169, EEventFunctionType.Condition, EEventFunctionConditionSubType.AdventureTriggerCondition, 711, 712, new int[2] { 41, 1 }, new int[0], 3, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 713));
		_dataArray.Add(new EventFunctionItem(170, EEventFunctionType.Condition, EEventFunctionConditionSubType.GeneralCondition, 714, 715, new int[2] { 41, 1 }, new int[0], 3, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 716));
		_dataArray.Add(new EventFunctionItem(171, EEventFunctionType.Condition, EEventFunctionConditionSubType.GeneralCondition, 717, 718, new int[2] { 41, 1 }, new int[0], 3, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 719));
		_dataArray.Add(new EventFunctionItem(172, EEventFunctionType.Condition, EEventFunctionConditionSubType.GeneralCondition, 720, 721, new int[2] { 6, 20 }, new int[0], 3, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 722));
		_dataArray.Add(new EventFunctionItem(173, EEventFunctionType.Condition, EEventFunctionConditionSubType.GeneralCondition, 723, 724, new int[2] { 6, 57 }, new int[0], 3, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 725));
		_dataArray.Add(new EventFunctionItem(174, EEventFunctionType.Condition, EEventFunctionConditionSubType.GeneralCondition, 726, 727, new int[2] { 6, 6 }, new int[0], 3, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 728));
		_dataArray.Add(new EventFunctionItem(175, EEventFunctionType.Condition, EEventFunctionConditionSubType.GeneralCondition, 729, 730, new int[2] { 6, 20 }, new int[0], 3, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 731));
		_dataArray.Add(new EventFunctionItem(176, EEventFunctionType.Condition, EEventFunctionConditionSubType.GeneralCondition, 732, 733, new int[2] { 6, 57 }, new int[0], 3, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 734));
		_dataArray.Add(new EventFunctionItem(177, EEventFunctionType.Condition, EEventFunctionConditionSubType.GeneralCondition, 735, 736, new int[2] { 6, 6 }, new int[0], 3, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 737));
		_dataArray.Add(new EventFunctionItem(178, EEventFunctionType.Condition, EEventFunctionConditionSubType.GeneralCondition, 738, 739, new int[3] { 6, 41, 1 }, new int[0], 3, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 740));
		_dataArray.Add(new EventFunctionItem(179, EEventFunctionType.Condition, EEventFunctionConditionSubType.GeneralCondition, 741, 742, new int[4] { 6, 35, 41, 1 }, new int[0], 3, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 743));
	}

	private void CreateItems3()
	{
		_dataArray.Add(new EventFunctionItem(180, EEventFunctionType.Condition, EEventFunctionConditionSubType.GeneralCondition, 744, 745, new int[2] { 6, 35 }, new int[0], 3, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 746));
		_dataArray.Add(new EventFunctionItem(181, EEventFunctionType.Condition, EEventFunctionConditionSubType.AdventureTriggerCondition, 756, 757, new int[1] { 1 }, new int[0], 3, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 758));
		_dataArray.Add(new EventFunctionItem(182, EEventFunctionType.Condition, EEventFunctionConditionSubType.AdventureTriggerCondition, 759, 715, new int[1] { 1 }, new int[0], 3, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 760));
		_dataArray.Add(new EventFunctionItem(183, EEventFunctionType.Condition, EEventFunctionConditionSubType.AdventureTriggerCondition, 761, 761, new int[1] { 61 }, new int[1] { 762 }, 3, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 763));
		_dataArray.Add(new EventFunctionItem(184, EEventFunctionType.Behavior, EEventFunctionConditionSubType.Invalid, 764, 764, new int[2] { 9, 3 }, new int[0], -1, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 765));
		_dataArray.Add(new EventFunctionItem(185, EEventFunctionType.Condition, EEventFunctionConditionSubType.AdventureTriggerCondition, 766, 767, new int[2] { 6, 1 }, new int[0], 3, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 768));
		_dataArray.Add(new EventFunctionItem(186, EEventFunctionType.Condition, EEventFunctionConditionSubType.AdventureTriggerCondition, 769, 770, new int[3] { 61, 3, 1 }, new int[3] { 762, 771, 772 }, 3, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 773));
		_dataArray.Add(new EventFunctionItem(187, EEventFunctionType.UI, EEventFunctionConditionSubType.Invalid, 93, 94, new int[1] { 6 }, new int[1] { 95 }, -1, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: false, 96));
		_dataArray.Add(new EventFunctionItem(188, EEventFunctionType.Behavior, EEventFunctionConditionSubType.Invalid, 479, 480, new int[1] { 3 }, new int[1] { 481 }, -1, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: false, 482));
		_dataArray.Add(new EventFunctionItem(189, EEventFunctionType.Behavior, EEventFunctionConditionSubType.Invalid, 204, 205, new int[2] { 6, 1 }, new int[2] { 206, 207 }, -1, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 208));
		_dataArray.Add(new EventFunctionItem(190, EEventFunctionType.Condition, EEventFunctionConditionSubType.GeneralCondition, 747, 748, new int[3] { 10, 41, 1 }, new int[0], 3, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 749));
		_dataArray.Add(new EventFunctionItem(191, EEventFunctionType.Behavior, EEventFunctionConditionSubType.Invalid, 774, 774, new int[1] { 6 }, new int[0], 1, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 701));
		_dataArray.Add(new EventFunctionItem(192, EEventFunctionType.Behavior, EEventFunctionConditionSubType.Invalid, 489, 490, new int[2] { 47, 59 }, new int[0], 10, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 491));
		_dataArray.Add(new EventFunctionItem(193, EEventFunctionType.Condition, EEventFunctionConditionSubType.GeneralCondition, 750, 751, new int[2] { 47, 59 }, new int[0], 3, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 752));
		_dataArray.Add(new EventFunctionItem(194, EEventFunctionType.Condition, EEventFunctionConditionSubType.GeneralCondition, 634, 635, new int[2] { 6, 10 }, new int[0], 3, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 636));
		_dataArray.Add(new EventFunctionItem(195, EEventFunctionType.Behavior, EEventFunctionConditionSubType.Invalid, 775, 776, new int[2] { 5, 1 }, new int[2] { 777, 778 }, -1, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: true, allowExternalUsage: true, 779));
		_dataArray.Add(new EventFunctionItem(196, EEventFunctionType.Behavior, EEventFunctionConditionSubType.Invalid, 780, 781, new int[0], new int[0], 1, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: true, allowExternalUsage: true, 782));
		_dataArray.Add(new EventFunctionItem(197, EEventFunctionType.UI, EEventFunctionConditionSubType.Invalid, 89, 90, new int[1] { 4 }, new int[1] { 91 }, -1, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 92));
		_dataArray.Add(new EventFunctionItem(198, EEventFunctionType.Behavior, EEventFunctionConditionSubType.Invalid, 495, 496, new int[1] { 6 }, new int[0], 9, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 497));
		_dataArray.Add(new EventFunctionItem(199, EEventFunctionType.Behavior, EEventFunctionConditionSubType.Invalid, 501, 502, new int[1] { 10 }, new int[0], 9, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 503));
		_dataArray.Add(new EventFunctionItem(200, EEventFunctionType.Behavior, EEventFunctionConditionSubType.Invalid, 783, 784, new int[2] { 6, 1 }, new int[2] { 785, 101 }, -1, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 786));
		_dataArray.Add(new EventFunctionItem(201, EEventFunctionType.Behavior, EEventFunctionConditionSubType.Invalid, 787, 788, new int[2] { 6, 1 }, new int[2] { 789, 790 }, -1, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 791));
		_dataArray.Add(new EventFunctionItem(202, EEventFunctionType.Condition, EEventFunctionConditionSubType.GeneralCondition, 753, 754, new int[2] { 40, 40 }, new int[0], 3, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 755));
		_dataArray.Add(new EventFunctionItem(203, EEventFunctionType.Condition, EEventFunctionConditionSubType.GeneralCondition, 589, 590, new int[2] { 6, 20 }, new int[0], 3, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 591));
		_dataArray.Add(new EventFunctionItem(204, EEventFunctionType.Condition, EEventFunctionConditionSubType.GeneralCondition, 670, 671, new int[2] { 12, 40 }, new int[0], 3, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 672));
		_dataArray.Add(new EventFunctionItem(205, EEventFunctionType.Behavior, EEventFunctionConditionSubType.Invalid, 792, 793, new int[2] { 6, 3 }, new int[2] { 794, 795 }, -1, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 796));
		_dataArray.Add(new EventFunctionItem(206, EEventFunctionType.Behavior, EEventFunctionConditionSubType.Invalid, 797, 798, new int[2] { 6, 3 }, new int[2] { 799, 795 }, -1, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 800));
		_dataArray.Add(new EventFunctionItem(207, EEventFunctionType.Condition, EEventFunctionConditionSubType.GeneralCondition, 801, 801, new int[1] { 1 }, new int[1] { 802 }, 3, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 803));
		_dataArray.Add(new EventFunctionItem(208, EEventFunctionType.Behavior, EEventFunctionConditionSubType.Invalid, 804, 804, new int[2] { 1, 3 }, new int[2] { 802, 805 }, -1, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 803));
		_dataArray.Add(new EventFunctionItem(209, EEventFunctionType.Behavior, EEventFunctionConditionSubType.Invalid, 806, 806, new int[2] { 1, 3 }, new int[2] { 802, 805 }, -1, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 803));
		_dataArray.Add(new EventFunctionItem(210, EEventFunctionType.Behavior, EEventFunctionConditionSubType.Invalid, 807, 807, new int[5] { 10, 13, 13, 57, 57 }, new int[5] { 808, 459, 460, 809, 810 }, 6, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 811));
		_dataArray.Add(new EventFunctionItem(211, EEventFunctionType.Behavior, EEventFunctionConditionSubType.Invalid, 812, 812, new int[0], new int[0], 3, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 813));
		_dataArray.Add(new EventFunctionItem(212, EEventFunctionType.Behavior, EEventFunctionConditionSubType.Invalid, 814, 814, new int[0], new int[0], 1, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 815));
		_dataArray.Add(new EventFunctionItem(213, EEventFunctionType.Condition, EEventFunctionConditionSubType.GeneralCondition, 690, 691, new int[2] { 6, 60 }, new int[0], 3, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 692));
		_dataArray.Add(new EventFunctionItem(214, EEventFunctionType.Behavior, EEventFunctionConditionSubType.Invalid, 816, 816, new int[0], new int[0], -1, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 817));
		_dataArray.Add(new EventFunctionItem(215, EEventFunctionType.Basic, EEventFunctionConditionSubType.Invalid, 66, 67, new int[1], new int[1] { 68 }, 1, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 69));
		_dataArray.Add(new EventFunctionItem(216, EEventFunctionType.Basic, EEventFunctionConditionSubType.Invalid, 70, 71, new int[2] { 0, 1 }, new int[2] { 68, 72 }, 0, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 73));
		_dataArray.Add(new EventFunctionItem(217, EEventFunctionType.Behavior, EEventFunctionConditionSubType.Invalid, 492, 493, new int[2] { 47, 59 }, new int[0], 0, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 494));
		_dataArray.Add(new EventFunctionItem(218, EEventFunctionType.Condition, EEventFunctionConditionSubType.GeneralCondition, 531, 532, new int[3] { 0, 1, 4 }, new int[3] { 68, 72, 533 }, 3, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 534));
		_dataArray.Add(new EventFunctionItem(219, EEventFunctionType.Condition, EEventFunctionConditionSubType.AdventureTriggerCondition, 818, 819, new int[3] { 4, 41, 1 }, new int[3] { 820, 821, 822 }, 3, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 823));
		_dataArray.Add(new EventFunctionItem(220, EEventFunctionType.Behavior, EEventFunctionConditionSubType.Invalid, 824, 825, new int[2] { 4, 1 }, new int[2] { 820, 822 }, -1, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 826));
		_dataArray.Add(new EventFunctionItem(221, EEventFunctionType.Behavior, EEventFunctionConditionSubType.Invalid, 827, 828, new int[2] { 4, 1 }, new int[2] { 820, 822 }, -1, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 829));
		_dataArray.Add(new EventFunctionItem(222, EEventFunctionType.Condition, EEventFunctionConditionSubType.AdventureTriggerCondition, 818, 830, new int[4] { 4, 41, 1, 3 }, new int[4] { 820, 821, 822, 831 }, 3, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 832));
		_dataArray.Add(new EventFunctionItem(223, EEventFunctionType.Behavior, EEventFunctionConditionSubType.Invalid, 824, 833, new int[2] { 4, 1 }, new int[2] { 820, 822 }, -1, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 834));
		_dataArray.Add(new EventFunctionItem(224, EEventFunctionType.Behavior, EEventFunctionConditionSubType.Invalid, 827, 835, new int[2] { 4, 1 }, new int[2] { 820, 822 }, -1, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 836));
		_dataArray.Add(new EventFunctionItem(225, EEventFunctionType.Condition, EEventFunctionConditionSubType.AdventureTriggerCondition, 41, 42, new int[1] { 1 }, new int[1] { 43 }, 3, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 837));
		_dataArray.Add(new EventFunctionItem(226, EEventFunctionType.Behavior, EEventFunctionConditionSubType.Invalid, 299, 300, new int[2] { 11, 3 }, new int[2] { 301, 302 }, -1, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 303));
		_dataArray.Add(new EventFunctionItem(227, EEventFunctionType.Behavior, EEventFunctionConditionSubType.Invalid, 455, 456, new int[4] { 10, 29, 13, 13 }, new int[4] { 457, 458, 459, 460 }, -1, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 461));
		_dataArray.Add(new EventFunctionItem(228, EEventFunctionType.Condition, EEventFunctionConditionSubType.AdventureTriggerCondition, 838, 838, new int[3] { 61, 41, 1 }, new int[3] { 762, 839, 822 }, 3, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 840));
		_dataArray.Add(new EventFunctionItem(229, EEventFunctionType.Condition, EEventFunctionConditionSubType.AdventureTriggerCondition, 841, 842, new int[3] { 62, 41, 1 }, new int[3] { 843, 839, 822 }, 3, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 844));
		_dataArray.Add(new EventFunctionItem(230, EEventFunctionType.Condition, EEventFunctionConditionSubType.AdventureTriggerCondition, 845, 846, new int[3] { 4, 4, 3 }, new int[3] { 847, 847, 848 }, 3, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 849));
		_dataArray.Add(new EventFunctionItem(231, EEventFunctionType.Condition, EEventFunctionConditionSubType.AdventureTriggerCondition, 850, 851, new int[3] { 4, 4, 3 }, new int[3] { 847, 847, 848 }, 3, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 852));
		_dataArray.Add(new EventFunctionItem(232, EEventFunctionType.Condition, EEventFunctionConditionSubType.AdventureTriggerCondition, 853, 854, new int[3] { 61, 3, 61 }, new int[3] { 762, 771, 762 }, 3, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 855));
		_dataArray.Add(new EventFunctionItem(233, EEventFunctionType.Condition, EEventFunctionConditionSubType.AdventureTriggerCondition, 856, 856, new int[1] { 61 }, new int[1] { 762 }, 3, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 857));
		_dataArray.Add(new EventFunctionItem(234, EEventFunctionType.Condition, EEventFunctionConditionSubType.AdventureTriggerCondition, 858, 858, new int[1] { 1 }, new int[1] { 772 }, 3, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 859));
		_dataArray.Add(new EventFunctionItem(235, EEventFunctionType.Behavior, EEventFunctionConditionSubType.Invalid, 860, 861, new int[2] { 8, 1 }, new int[2] { 862, 863 }, -1, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 864));
		_dataArray.Add(new EventFunctionItem(236, EEventFunctionType.Behavior, EEventFunctionConditionSubType.Invalid, 865, 866, new int[1] { 8 }, new int[0], -1, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 867));
		_dataArray.Add(new EventFunctionItem(237, EEventFunctionType.Condition, EEventFunctionConditionSubType.GeneralCondition, 868, 868, new int[1] { 8 }, new int[0], 3, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 869));
		_dataArray.Add(new EventFunctionItem(238, EEventFunctionType.Behavior, EEventFunctionConditionSubType.Invalid, 870, 870, new int[1] { 6 }, new int[0], -1, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 871));
		_dataArray.Add(new EventFunctionItem(239, EEventFunctionType.Behavior, EEventFunctionConditionSubType.Invalid, 872, 872, new int[1] { 6 }, new int[0], -1, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 873));
	}

	private void CreateItems4()
	{
		_dataArray.Add(new EventFunctionItem(240, EEventFunctionType.Behavior, EEventFunctionConditionSubType.Invalid, 874, 875, new int[1] { 1 }, new int[1] { 876 }, -1, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 877));
		_dataArray.Add(new EventFunctionItem(241, EEventFunctionType.Behavior, EEventFunctionConditionSubType.Invalid, 405, 878, new int[1] { 4 }, new int[1] { 879 }, -1, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 880));
		_dataArray.Add(new EventFunctionItem(242, EEventFunctionType.Behavior, EEventFunctionConditionSubType.Invalid, 881, 881, new int[2] { 64, 17 }, new int[0], -1, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 882));
		_dataArray.Add(new EventFunctionItem(243, EEventFunctionType.Behavior, EEventFunctionConditionSubType.Invalid, 483, 484, new int[1] { 9 }, new int[0], 10, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 485));
		_dataArray.Add(new EventFunctionItem(244, EEventFunctionType.Behavior, EEventFunctionConditionSubType.Invalid, 883, 883, new int[2] { 11, 4 }, new int[2] { 884, 885 }, -1, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 886));
		_dataArray.Add(new EventFunctionItem(245, EEventFunctionType.Condition, EEventFunctionConditionSubType.AdventureTriggerCondition, 887, 887, new int[2] { 62, 3 }, new int[2] { 888, 771 }, 3, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 889));
		_dataArray.Add(new EventFunctionItem(246, EEventFunctionType.Condition, EEventFunctionConditionSubType.AdventureTriggerCondition, 890, 890, new int[3] { 62, 1, 3 }, new int[3] { 888, 413, 771 }, 3, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 891));
		_dataArray.Add(new EventFunctionItem(247, EEventFunctionType.Condition, EEventFunctionConditionSubType.AdventureTriggerCondition, 892, 892, new int[3] { 62, 61, 3 }, new int[3] { 888, 762, 771 }, 3, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 893));
		_dataArray.Add(new EventFunctionItem(248, EEventFunctionType.Condition, EEventFunctionConditionSubType.AdventureTriggerCondition, 894, 894, new int[1] { 62 }, new int[1] { 888 }, 3, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 895));
		_dataArray.Add(new EventFunctionItem(249, EEventFunctionType.Behavior, EEventFunctionConditionSubType.Invalid, 896, 897, new int[0], new int[0], 1, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 898));
		_dataArray.Add(new EventFunctionItem(250, EEventFunctionType.Behavior, EEventFunctionConditionSubType.Invalid, 899, 900, new int[6] { 6, 1, 4, 4, 4, 4 }, new int[6] { 901, 902, 903, 904, 905, 906 }, -1, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 907));
		_dataArray.Add(new EventFunctionItem(251, EEventFunctionType.Behavior, EEventFunctionConditionSubType.Invalid, 908, 909, new int[1] { 6 }, new int[1] { 910 }, 1, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 911));
		_dataArray.Add(new EventFunctionItem(252, EEventFunctionType.Behavior, EEventFunctionConditionSubType.Invalid, 912, 913, new int[1] { 6 }, new int[1] { 910 }, -1, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 914));
		_dataArray.Add(new EventFunctionItem(253, EEventFunctionType.Behavior, EEventFunctionConditionSubType.Invalid, 915, 916, new int[3] { 6, 1, 4 }, new int[3] { 910, 917, 918 }, -1, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 919));
		_dataArray.Add(new EventFunctionItem(254, EEventFunctionType.Behavior, EEventFunctionConditionSubType.Invalid, 920, 921, new int[2] { 6, 1 }, new int[2] { 910, 922 }, -1, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 923));
		_dataArray.Add(new EventFunctionItem(255, EEventFunctionType.Behavior, EEventFunctionConditionSubType.Invalid, 924, 925, new int[0], new int[0], 1, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 926));
		_dataArray.Add(new EventFunctionItem(256, EEventFunctionType.Condition, EEventFunctionConditionSubType.Invalid, 927, 928, new int[2] { 6, 4 }, new int[2] { 910, 533 }, 3, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 929));
		_dataArray.Add(new EventFunctionItem(257, EEventFunctionType.Condition, EEventFunctionConditionSubType.GeneralCondition, 930, 931, new int[2] { 41, 1 }, new int[2] { 821, 822 }, 3, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 932));
		_dataArray.Add(new EventFunctionItem(258, EEventFunctionType.Condition, EEventFunctionConditionSubType.GeneralCondition, 933, 934, new int[3] { 6, 41, 1 }, new int[3] { 910, 821, 822 }, 3, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 935));
		_dataArray.Add(new EventFunctionItem(259, EEventFunctionType.Behavior, EEventFunctionConditionSubType.Invalid, 936, 937, new int[4] { 61, 1, 1, 1 }, new int[4] { 762, 413, 938, 939 }, -1, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 940));
		_dataArray.Add(new EventFunctionItem(260, EEventFunctionType.Behavior, EEventFunctionConditionSubType.Invalid, 941, 942, new int[5] { 61, 4, 3, 1, 1 }, new int[5] { 762, 943, 944, 938, 939 }, -1, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 945));
		_dataArray.Add(new EventFunctionItem(261, EEventFunctionType.Behavior, EEventFunctionConditionSubType.Invalid, 504, 505, new int[1] { 6 }, new int[0], 12, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 506));
		_dataArray.Add(new EventFunctionItem(262, EEventFunctionType.Behavior, EEventFunctionConditionSubType.Invalid, 507, 508, new int[1] { 10 }, new int[0], 12, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 509));
		_dataArray.Add(new EventFunctionItem(263, EEventFunctionType.Behavior, EEventFunctionConditionSubType.Invalid, 498, 499, new int[1] { 6 }, new int[0], 10, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 500));
		_dataArray.Add(new EventFunctionItem(264, EEventFunctionType.Behavior, EEventFunctionConditionSubType.Invalid, 946, 946, new int[0], new int[0], -1, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 947));
		_dataArray.Add(new EventFunctionItem(265, EEventFunctionType.Condition, EEventFunctionConditionSubType.AdventureTriggerCondition, 948, 949, new int[4] { 4, 4, 41, 1 }, new int[4] { 950, 820, 821, 822 }, 3, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 951));
		_dataArray.Add(new EventFunctionItem(266, EEventFunctionType.Behavior, EEventFunctionConditionSubType.Invalid, 952, 953, new int[3] { 4, 4, 1 }, new int[3] { 950, 820, 822 }, -1, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 954));
		_dataArray.Add(new EventFunctionItem(267, EEventFunctionType.Behavior, EEventFunctionConditionSubType.Invalid, 955, 956, new int[3] { 4, 4, 1 }, new int[3] { 950, 820, 822 }, -1, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 957));
		_dataArray.Add(new EventFunctionItem(268, EEventFunctionType.Behavior, EEventFunctionConditionSubType.Invalid, 958, 959, new int[2] { 61, 1 }, new int[2] { 762, 790 }, -1, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 960));
		_dataArray.Add(new EventFunctionItem(269, EEventFunctionType.Behavior, EEventFunctionConditionSubType.Invalid, 961, 961, new int[1] { 61 }, new int[1] { 762 }, -1, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 962));
		_dataArray.Add(new EventFunctionItem(270, EEventFunctionType.Behavior, EEventFunctionConditionSubType.Invalid, 963, 963, new int[1] { 61 }, new int[1] { 762 }, -1, indentNext: false, -1, canCreateManually: true, new List<int>(), allowedInCondition: false, isTransition: false, allowExternalUsage: true, 964));
	}

	public override void Init()
	{
		base.Init();
		_dataArray = new List<EventFunctionItem>(271);
		CreateItems0();
		CreateItems1();
		CreateItems2();
		CreateItems3();
		CreateItems4();
	}
}
