using System;
using System.Collections.Generic;
using Config.Common;
using Config.ConfigCells;

namespace Config;

[Serializable]
public class TravelingEvent : ConfigData<TravelingEventItem, short>
{
	public static class DefKey
	{
		public const short JingjiMaterial = 0;

		public const short BashuMaterial = 1;

		public const short GuangnanMaterial = 2;

		public const short JingBeiMaterial = 3;

		public const short ShanxiMaterial = 4;

		public const short GuangdongMaterial = 5;

		public const short ShandongMaterial = 6;

		public const short JingnanMaterial = 7;

		public const short FujianMaterial = 8;

		public const short LiaodongMaterial = 9;

		public const short XiyuMaterial = 10;

		public const short YunnanMaterial = 11;

		public const short HuainanMaterial = 12;

		public const short JiangnanMaterial = 13;

		public const short JiangbeiMaterial = 14;

		public const short JingjiResource = 15;

		public const short BashuResource = 16;

		public const short GuangnanResource = 17;

		public const short JingBeiResource = 18;

		public const short ShanxiResource = 19;

		public const short GuangdongResource = 20;

		public const short ShandongResource = 21;

		public const short JingnanResource = 22;

		public const short FujianResource = 23;

		public const short LiaodongResource = 24;

		public const short XiyuResource = 25;

		public const short YunnanResource = 26;

		public const short HuainanResource = 27;

		public const short JiangnanResource = 28;

		public const short JiangbeiResource = 29;

		public const short JingjiFood = 30;

		public const short BashuFood = 31;

		public const short GuangnanFood = 32;

		public const short JingBeiFood = 33;

		public const short ShanxiFood = 34;

		public const short GuangdongFood = 35;

		public const short ShandongFood = 36;

		public const short JingnanFood = 37;

		public const short FujianFood = 38;

		public const short LiaodongFood = 39;

		public const short XiyuFood = 40;

		public const short YunnanFood = 41;

		public const short HuainanFood = 42;

		public const short JiangnanFood = 43;

		public const short JiangbeiFood = 44;

		public const short HealOuterInjury = 45;

		public const short HealInnerInjury = 46;

		public const short HealPoison = 47;

		public const short HealDisorderOfQi = 48;

		public const short HealLifeSpan = 49;

		public const short FriendResource = 50;

		public const short FriendFood = 51;

		public const short FriendTeaWine = 52;

		public const short FriendMedicine = 53;

		public const short FameResource = 54;

		public const short FameFood = 55;

		public const short FameTeaWine = 56;

		public const short FameMedicine = 57;

		public const short RecoverStrength = 58;

		public const short RecoverDexterity = 59;

		public const short RecoverConcentration = 60;

		public const short RecoverVitality = 61;

		public const short RecoverEnergy = 62;

		public const short RecoverIntelligence = 63;

		public const short AreaInteractGood = 64;

		public const short AreaInteractNormal = 65;

		public const short AreaInteractBad = 66;

		public const short AreaInteractIgnored = 67;

		public const short JingjiAreaSpiritualDebtSucceed = 68;

		public const short BashuAreaSpiritualDebtSucceed = 69;

		public const short GuangnanAreaSpiritualDebtSucceed = 70;

		public const short JingBeiAreaSpiritualDebtSucceed = 71;

		public const short ShanxiAreaSpiritualDebtSucceed = 72;

		public const short GuangdongAreaSpiritualDebtSucceed = 73;

		public const short ShandongAreaSpiritualDebtSucceed = 74;

		public const short JingnanAreaSpiritualDebtSucceed = 75;

		public const short FujianAreaSpiritualDebtSucceed = 76;

		public const short LiaodongAreaSpiritualDebtSucceed = 77;

		public const short XiyuAreaSpiritualDebtSucceed = 78;

		public const short YunnanAreaSpiritualDebtSucceed = 79;

		public const short HuainanAreaSpiritualDebtSucceed = 80;

		public const short JiangnanAreaSpiritualDebtSucceed = 81;

		public const short JiangbeiAreaSpiritualDebtSucceed = 82;

		public const short AreaSpiritualDebtIgnored = 83;

		public const short TravelBattlePerfectWin = 84;

		public const short TravelBattleWin = 85;

		public const short TravelBattleLose = 86;

		public const short GroupMemberAccept = 87;

		public const short GroupMemberRefuse = 88;

		public const short GroupMemberIgnored = 89;

		public const short ConsumeStrengthSucceed = 90;

		public const short ConsumeDexteritySucceed = 91;

		public const short ConsumeConcentrationSucceed = 92;

		public const short ConsumeVitalitySucceed = 93;

		public const short ConsumeEnergySucceed = 94;

		public const short ConsumeIntelligenceSucceed = 95;

		public const short NoConsumeMainAttribute = 96;

		public const short RoadBlockAndDetour = 97;

		public const short RoadBlockAndIgnore = 98;

		public const short JingjiInteract = 99;

		public const short BashuInteract = 100;

		public const short GuangnanInteract = 101;

		public const short JingBeiInteract = 102;

		public const short ShanxiInteract = 103;

		public const short GuangdongInteract = 104;

		public const short ShandongInteract = 105;

		public const short JingnanInteract = 106;

		public const short FujianInteract = 107;

		public const short LiaodongInteract = 108;

		public const short XiyuInteract = 109;

		public const short YunnanInteract = 110;

		public const short HuainanInteract = 111;

		public const short JiangnanInteract = 112;

		public const short JiangbeiInteract = 113;

		public const short JingjiAreaSpiritualDebt = 114;

		public const short BashuAreaSpiritualDebt = 115;

		public const short GuangnanAreaSpiritualDebt = 116;

		public const short JingBeiAreaSpiritualDebt = 117;

		public const short ShanxiAreaSpiritualDebt = 118;

		public const short GuangdongAreaSpiritualDebt = 119;

		public const short ShandongAreaSpiritualDebt = 120;

		public const short JingnanAreaSpiritualDebt = 121;

		public const short FujianAreaSpiritualDebt = 122;

		public const short LiaodongAreaSpiritualDebt = 123;

		public const short XiyuAreaSpiritualDebt = 124;

		public const short YunnanAreaSpiritualDebt = 125;

		public const short HuainanAreaSpiritualDebt = 126;

		public const short JiangnanAreaSpiritualDebt = 127;

		public const short JiangbeiAreaSpiritualDebt = 128;

		public const short VisitShaolin = 129;

		public const short VisitEmei = 130;

		public const short VisitBaihua = 131;

		public const short VisitWudang = 132;

		public const short VisitYuanshan = 133;

		public const short VisitShixiang = 134;

		public const short VisitRanshan = 135;

		public const short VisitXuannv = 136;

		public const short VisitZhujian = 137;

		public const short VisitKongsang = 138;

		public const short VisitJingang = 139;

		public const short VisitWuxian = 140;

		public const short VisitJieqing = 141;

		public const short VisitFulong = 142;

		public const short VisitXuehou = 143;

		public const short EnemyAttack = 144;

		public const short RighteousAttack = 145;

		public const short XiangshuMinionAttack = 146;

		public const short ShaolinAttack = 147;

		public const short EmeiAttack = 148;

		public const short BaihuaAttack = 149;

		public const short WudangAttack = 150;

		public const short YuanshanAttack = 151;

		public const short JingangAttack = 152;

		public const short WuxianAttack = 153;

		public const short JieqingAttack = 154;

		public const short FulongAttack = 155;

		public const short XuehouAttack = 156;

		public const short FriendGroupMember = 157;

		public const short FameGroupMember = 158;

		public const short ConsumeStrength = 159;

		public const short ConsumeDexterity = 160;

		public const short ConsumeConcentration = 161;

		public const short ConsumeVitality = 162;

		public const short ConsumeEnergy = 163;

		public const short ConsumeIntelligence = 164;

		public const short RoadBlock = 165;
	}

	public static class DefValue
	{
		public static TravelingEventItem JingjiMaterial => Instance[(short)0];

		public static TravelingEventItem BashuMaterial => Instance[(short)1];

		public static TravelingEventItem GuangnanMaterial => Instance[(short)2];

		public static TravelingEventItem JingBeiMaterial => Instance[(short)3];

		public static TravelingEventItem ShanxiMaterial => Instance[(short)4];

		public static TravelingEventItem GuangdongMaterial => Instance[(short)5];

		public static TravelingEventItem ShandongMaterial => Instance[(short)6];

		public static TravelingEventItem JingnanMaterial => Instance[(short)7];

		public static TravelingEventItem FujianMaterial => Instance[(short)8];

		public static TravelingEventItem LiaodongMaterial => Instance[(short)9];

		public static TravelingEventItem XiyuMaterial => Instance[(short)10];

		public static TravelingEventItem YunnanMaterial => Instance[(short)11];

		public static TravelingEventItem HuainanMaterial => Instance[(short)12];

		public static TravelingEventItem JiangnanMaterial => Instance[(short)13];

		public static TravelingEventItem JiangbeiMaterial => Instance[(short)14];

		public static TravelingEventItem JingjiResource => Instance[(short)15];

		public static TravelingEventItem BashuResource => Instance[(short)16];

		public static TravelingEventItem GuangnanResource => Instance[(short)17];

		public static TravelingEventItem JingBeiResource => Instance[(short)18];

		public static TravelingEventItem ShanxiResource => Instance[(short)19];

		public static TravelingEventItem GuangdongResource => Instance[(short)20];

		public static TravelingEventItem ShandongResource => Instance[(short)21];

		public static TravelingEventItem JingnanResource => Instance[(short)22];

		public static TravelingEventItem FujianResource => Instance[(short)23];

		public static TravelingEventItem LiaodongResource => Instance[(short)24];

		public static TravelingEventItem XiyuResource => Instance[(short)25];

		public static TravelingEventItem YunnanResource => Instance[(short)26];

		public static TravelingEventItem HuainanResource => Instance[(short)27];

		public static TravelingEventItem JiangnanResource => Instance[(short)28];

		public static TravelingEventItem JiangbeiResource => Instance[(short)29];

		public static TravelingEventItem JingjiFood => Instance[(short)30];

		public static TravelingEventItem BashuFood => Instance[(short)31];

		public static TravelingEventItem GuangnanFood => Instance[(short)32];

		public static TravelingEventItem JingBeiFood => Instance[(short)33];

		public static TravelingEventItem ShanxiFood => Instance[(short)34];

		public static TravelingEventItem GuangdongFood => Instance[(short)35];

		public static TravelingEventItem ShandongFood => Instance[(short)36];

		public static TravelingEventItem JingnanFood => Instance[(short)37];

		public static TravelingEventItem FujianFood => Instance[(short)38];

		public static TravelingEventItem LiaodongFood => Instance[(short)39];

		public static TravelingEventItem XiyuFood => Instance[(short)40];

		public static TravelingEventItem YunnanFood => Instance[(short)41];

		public static TravelingEventItem HuainanFood => Instance[(short)42];

		public static TravelingEventItem JiangnanFood => Instance[(short)43];

		public static TravelingEventItem JiangbeiFood => Instance[(short)44];

		public static TravelingEventItem HealOuterInjury => Instance[(short)45];

		public static TravelingEventItem HealInnerInjury => Instance[(short)46];

		public static TravelingEventItem HealPoison => Instance[(short)47];

		public static TravelingEventItem HealDisorderOfQi => Instance[(short)48];

		public static TravelingEventItem HealLifeSpan => Instance[(short)49];

		public static TravelingEventItem FriendResource => Instance[(short)50];

		public static TravelingEventItem FriendFood => Instance[(short)51];

		public static TravelingEventItem FriendTeaWine => Instance[(short)52];

		public static TravelingEventItem FriendMedicine => Instance[(short)53];

		public static TravelingEventItem FameResource => Instance[(short)54];

		public static TravelingEventItem FameFood => Instance[(short)55];

		public static TravelingEventItem FameTeaWine => Instance[(short)56];

		public static TravelingEventItem FameMedicine => Instance[(short)57];

		public static TravelingEventItem RecoverStrength => Instance[(short)58];

		public static TravelingEventItem RecoverDexterity => Instance[(short)59];

		public static TravelingEventItem RecoverConcentration => Instance[(short)60];

		public static TravelingEventItem RecoverVitality => Instance[(short)61];

		public static TravelingEventItem RecoverEnergy => Instance[(short)62];

		public static TravelingEventItem RecoverIntelligence => Instance[(short)63];

		public static TravelingEventItem AreaInteractGood => Instance[(short)64];

		public static TravelingEventItem AreaInteractNormal => Instance[(short)65];

		public static TravelingEventItem AreaInteractBad => Instance[(short)66];

		public static TravelingEventItem AreaInteractIgnored => Instance[(short)67];

		public static TravelingEventItem JingjiAreaSpiritualDebtSucceed => Instance[(short)68];

		public static TravelingEventItem BashuAreaSpiritualDebtSucceed => Instance[(short)69];

		public static TravelingEventItem GuangnanAreaSpiritualDebtSucceed => Instance[(short)70];

		public static TravelingEventItem JingBeiAreaSpiritualDebtSucceed => Instance[(short)71];

		public static TravelingEventItem ShanxiAreaSpiritualDebtSucceed => Instance[(short)72];

		public static TravelingEventItem GuangdongAreaSpiritualDebtSucceed => Instance[(short)73];

		public static TravelingEventItem ShandongAreaSpiritualDebtSucceed => Instance[(short)74];

		public static TravelingEventItem JingnanAreaSpiritualDebtSucceed => Instance[(short)75];

		public static TravelingEventItem FujianAreaSpiritualDebtSucceed => Instance[(short)76];

		public static TravelingEventItem LiaodongAreaSpiritualDebtSucceed => Instance[(short)77];

		public static TravelingEventItem XiyuAreaSpiritualDebtSucceed => Instance[(short)78];

		public static TravelingEventItem YunnanAreaSpiritualDebtSucceed => Instance[(short)79];

		public static TravelingEventItem HuainanAreaSpiritualDebtSucceed => Instance[(short)80];

		public static TravelingEventItem JiangnanAreaSpiritualDebtSucceed => Instance[(short)81];

		public static TravelingEventItem JiangbeiAreaSpiritualDebtSucceed => Instance[(short)82];

		public static TravelingEventItem AreaSpiritualDebtIgnored => Instance[(short)83];

		public static TravelingEventItem TravelBattlePerfectWin => Instance[(short)84];

		public static TravelingEventItem TravelBattleWin => Instance[(short)85];

		public static TravelingEventItem TravelBattleLose => Instance[(short)86];

		public static TravelingEventItem GroupMemberAccept => Instance[(short)87];

		public static TravelingEventItem GroupMemberRefuse => Instance[(short)88];

		public static TravelingEventItem GroupMemberIgnored => Instance[(short)89];

		public static TravelingEventItem ConsumeStrengthSucceed => Instance[(short)90];

		public static TravelingEventItem ConsumeDexteritySucceed => Instance[(short)91];

		public static TravelingEventItem ConsumeConcentrationSucceed => Instance[(short)92];

		public static TravelingEventItem ConsumeVitalitySucceed => Instance[(short)93];

		public static TravelingEventItem ConsumeEnergySucceed => Instance[(short)94];

		public static TravelingEventItem ConsumeIntelligenceSucceed => Instance[(short)95];

		public static TravelingEventItem NoConsumeMainAttribute => Instance[(short)96];

		public static TravelingEventItem RoadBlockAndDetour => Instance[(short)97];

		public static TravelingEventItem RoadBlockAndIgnore => Instance[(short)98];

		public static TravelingEventItem JingjiInteract => Instance[(short)99];

		public static TravelingEventItem BashuInteract => Instance[(short)100];

		public static TravelingEventItem GuangnanInteract => Instance[(short)101];

		public static TravelingEventItem JingBeiInteract => Instance[(short)102];

		public static TravelingEventItem ShanxiInteract => Instance[(short)103];

		public static TravelingEventItem GuangdongInteract => Instance[(short)104];

		public static TravelingEventItem ShandongInteract => Instance[(short)105];

		public static TravelingEventItem JingnanInteract => Instance[(short)106];

		public static TravelingEventItem FujianInteract => Instance[(short)107];

		public static TravelingEventItem LiaodongInteract => Instance[(short)108];

		public static TravelingEventItem XiyuInteract => Instance[(short)109];

		public static TravelingEventItem YunnanInteract => Instance[(short)110];

		public static TravelingEventItem HuainanInteract => Instance[(short)111];

		public static TravelingEventItem JiangnanInteract => Instance[(short)112];

		public static TravelingEventItem JiangbeiInteract => Instance[(short)113];

		public static TravelingEventItem JingjiAreaSpiritualDebt => Instance[(short)114];

		public static TravelingEventItem BashuAreaSpiritualDebt => Instance[(short)115];

		public static TravelingEventItem GuangnanAreaSpiritualDebt => Instance[(short)116];

		public static TravelingEventItem JingBeiAreaSpiritualDebt => Instance[(short)117];

		public static TravelingEventItem ShanxiAreaSpiritualDebt => Instance[(short)118];

		public static TravelingEventItem GuangdongAreaSpiritualDebt => Instance[(short)119];

		public static TravelingEventItem ShandongAreaSpiritualDebt => Instance[(short)120];

		public static TravelingEventItem JingnanAreaSpiritualDebt => Instance[(short)121];

		public static TravelingEventItem FujianAreaSpiritualDebt => Instance[(short)122];

		public static TravelingEventItem LiaodongAreaSpiritualDebt => Instance[(short)123];

		public static TravelingEventItem XiyuAreaSpiritualDebt => Instance[(short)124];

		public static TravelingEventItem YunnanAreaSpiritualDebt => Instance[(short)125];

		public static TravelingEventItem HuainanAreaSpiritualDebt => Instance[(short)126];

		public static TravelingEventItem JiangnanAreaSpiritualDebt => Instance[(short)127];

		public static TravelingEventItem JiangbeiAreaSpiritualDebt => Instance[(short)128];

		public static TravelingEventItem VisitShaolin => Instance[(short)129];

		public static TravelingEventItem VisitEmei => Instance[(short)130];

		public static TravelingEventItem VisitBaihua => Instance[(short)131];

		public static TravelingEventItem VisitWudang => Instance[(short)132];

		public static TravelingEventItem VisitYuanshan => Instance[(short)133];

		public static TravelingEventItem VisitShixiang => Instance[(short)134];

		public static TravelingEventItem VisitRanshan => Instance[(short)135];

		public static TravelingEventItem VisitXuannv => Instance[(short)136];

		public static TravelingEventItem VisitZhujian => Instance[(short)137];

		public static TravelingEventItem VisitKongsang => Instance[(short)138];

		public static TravelingEventItem VisitJingang => Instance[(short)139];

		public static TravelingEventItem VisitWuxian => Instance[(short)140];

		public static TravelingEventItem VisitJieqing => Instance[(short)141];

		public static TravelingEventItem VisitFulong => Instance[(short)142];

		public static TravelingEventItem VisitXuehou => Instance[(short)143];

		public static TravelingEventItem EnemyAttack => Instance[(short)144];

		public static TravelingEventItem RighteousAttack => Instance[(short)145];

		public static TravelingEventItem XiangshuMinionAttack => Instance[(short)146];

		public static TravelingEventItem ShaolinAttack => Instance[(short)147];

		public static TravelingEventItem EmeiAttack => Instance[(short)148];

		public static TravelingEventItem BaihuaAttack => Instance[(short)149];

		public static TravelingEventItem WudangAttack => Instance[(short)150];

		public static TravelingEventItem YuanshanAttack => Instance[(short)151];

		public static TravelingEventItem JingangAttack => Instance[(short)152];

		public static TravelingEventItem WuxianAttack => Instance[(short)153];

		public static TravelingEventItem JieqingAttack => Instance[(short)154];

		public static TravelingEventItem FulongAttack => Instance[(short)155];

		public static TravelingEventItem XuehouAttack => Instance[(short)156];

		public static TravelingEventItem FriendGroupMember => Instance[(short)157];

		public static TravelingEventItem FameGroupMember => Instance[(short)158];

		public static TravelingEventItem ConsumeStrength => Instance[(short)159];

		public static TravelingEventItem ConsumeDexterity => Instance[(short)160];

		public static TravelingEventItem ConsumeConcentration => Instance[(short)161];

		public static TravelingEventItem ConsumeVitality => Instance[(short)162];

		public static TravelingEventItem ConsumeEnergy => Instance[(short)163];

		public static TravelingEventItem ConsumeIntelligence => Instance[(short)164];

		public static TravelingEventItem RoadBlock => Instance[(short)165];
	}

	public static TravelingEvent Instance = new TravelingEvent();

	private readonly HashSet<string> RequiredFields = new HashSet<string>
	{
		"StateTemplateId", "OrgTemplateId", "CharacterProperty", "ItemRange", "TemplateId", "Name", "DisplayType", "Desc", "Event", "ValueRange",
		"ItemGradeWeight", "ResourceWeights"
	};

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
		_dataArray.Add(new TravelingEventItem(0, 0, ETravelingEventDisplayType.TravelingEvent_0, ETravelingEventType.AreaMaterial, 1, new string[5] { "Character", "Item", "", "", "" }, 1, ETravelingEventTriggerType.Any, ETravelingEventTriggerAreaType.NormalArea, -1, isUnique: false, 0, 20, 0, new sbyte[2] { -100, 100 }, 6, null, 0, null, -1, -1, new List<PresetItemTemplateIdGroup>
		{
			new PresetItemTemplateIdGroup("Material", 21, 7)
		}, new short[9] { 0, 243, 81, 27, 9, 3, 1, 0, 0 }, null));
		_dataArray.Add(new TravelingEventItem(1, 2, ETravelingEventDisplayType.TravelingEvent_0, ETravelingEventType.AreaMaterial, 3, new string[5] { "Character", "Item", "", "", "" }, 2, ETravelingEventTriggerType.Any, ETravelingEventTriggerAreaType.NormalArea, -1, isUnique: false, 0, 20, 0, new sbyte[2] { -100, 100 }, 6, null, 0, null, -1, -1, new List<PresetItemTemplateIdGroup>
		{
			new PresetItemTemplateIdGroup("Material", 0, 7)
		}, new short[9] { 0, 243, 81, 27, 9, 3, 1, 0, 0 }, null));
		_dataArray.Add(new TravelingEventItem(2, 4, ETravelingEventDisplayType.TravelingEvent_0, ETravelingEventType.AreaMaterial, 5, new string[5] { "Character", "Item", "", "", "" }, 3, ETravelingEventTriggerType.Any, ETravelingEventTriggerAreaType.NormalArea, -1, isUnique: false, 0, 20, 0, new sbyte[2] { -100, 100 }, 6, null, 0, null, -1, -1, new List<PresetItemTemplateIdGroup>
		{
			new PresetItemTemplateIdGroup("Material", 140, 96)
		}, new short[9] { 0, 256, 0, 16, 0, 1, 0, 0, 0 }, null));
		_dataArray.Add(new TravelingEventItem(3, 6, ETravelingEventDisplayType.TravelingEvent_0, ETravelingEventType.AreaMaterial, 7, new string[5] { "Character", "Item", "", "", "" }, 4, ETravelingEventTriggerType.Any, ETravelingEventTriggerAreaType.NormalArea, -1, isUnique: false, 0, 20, 0, new sbyte[2] { -100, 100 }, 6, null, 0, null, -1, -1, new List<PresetItemTemplateIdGroup>
		{
			new PresetItemTemplateIdGroup("Material", 7, 7)
		}, new short[9] { 0, 243, 81, 27, 9, 3, 1, 0, 0 }, null));
		_dataArray.Add(new TravelingEventItem(4, 8, ETravelingEventDisplayType.TravelingEvent_0, ETravelingEventType.AreaMaterial, 9, new string[5] { "Character", "Item", "", "", "" }, 5, ETravelingEventTriggerType.Any, ETravelingEventTriggerAreaType.NormalArea, -1, isUnique: false, 0, 20, 0, new sbyte[2] { -100, 100 }, 6, null, 0, null, -1, -1, new List<PresetItemTemplateIdGroup>
		{
			new PresetItemTemplateIdGroup("Material", 63, 7)
		}, new short[9] { 0, 243, 81, 27, 9, 3, 1, 0, 0 }, null));
		_dataArray.Add(new TravelingEventItem(5, 10, ETravelingEventDisplayType.TravelingEvent_0, ETravelingEventType.AreaMaterial, 11, new string[5] { "Character", "Item", "", "", "" }, 6, ETravelingEventTriggerType.Any, ETravelingEventTriggerAreaType.NormalArea, -1, isUnique: false, 0, 20, 0, new sbyte[2] { -100, 100 }, 6, null, 0, null, -1, -1, new List<PresetItemTemplateIdGroup>
		{
			new PresetItemTemplateIdGroup("Material", 28, 7)
		}, new short[9] { 0, 243, 81, 27, 9, 3, 1, 0, 0 }, null));
		_dataArray.Add(new TravelingEventItem(6, 12, ETravelingEventDisplayType.TravelingEvent_0, ETravelingEventType.AreaMaterial, 13, new string[5] { "Character", "Item", "", "", "" }, 7, ETravelingEventTriggerType.Any, ETravelingEventTriggerAreaType.NormalArea, -1, isUnique: false, 0, 20, 0, new sbyte[2] { -100, 100 }, 6, null, 0, null, -1, -1, new List<PresetItemTemplateIdGroup>
		{
			new PresetItemTemplateIdGroup("Material", 49, 7)
		}, new short[9] { 0, 243, 81, 27, 9, 3, 1, 0, 0 }, null));
		_dataArray.Add(new TravelingEventItem(7, 14, ETravelingEventDisplayType.TravelingEvent_0, ETravelingEventType.AreaMaterial, 15, new string[5] { "Character", "Item", "", "", "" }, 8, ETravelingEventTriggerType.Any, ETravelingEventTriggerAreaType.NormalArea, -1, isUnique: false, 0, 20, 0, new sbyte[2] { -100, 100 }, 6, null, 0, null, -1, -1, new List<PresetItemTemplateIdGroup>
		{
			new PresetItemTemplateIdGroup("Material", 35, 7)
		}, new short[9] { 0, 243, 81, 27, 9, 3, 1, 0, 0 }, null));
		_dataArray.Add(new TravelingEventItem(8, 16, ETravelingEventDisplayType.TravelingEvent_0, ETravelingEventType.AreaMaterial, 17, new string[5] { "Character", "Item", "", "", "" }, 9, ETravelingEventTriggerType.Any, ETravelingEventTriggerAreaType.NormalArea, -1, isUnique: false, 0, 20, 0, new sbyte[2] { -100, 100 }, 6, null, 0, null, -1, -1, new List<PresetItemTemplateIdGroup>
		{
			new PresetItemTemplateIdGroup("Material", 14, 7)
		}, new short[9] { 0, 243, 81, 27, 9, 3, 1, 0, 0 }, null));
		_dataArray.Add(new TravelingEventItem(9, 18, ETravelingEventDisplayType.TravelingEvent_0, ETravelingEventType.AreaMaterial, 19, new string[5] { "Character", "Item", "", "", "" }, 10, ETravelingEventTriggerType.Any, ETravelingEventTriggerAreaType.NormalArea, -1, isUnique: false, 0, 20, 0, new sbyte[2] { -100, 100 }, 6, null, 0, null, -1, -1, new List<PresetItemTemplateIdGroup>
		{
			new PresetItemTemplateIdGroup("Material", 140, 96)
		}, new short[9] { 0, 256, 0, 16, 0, 1, 0, 0, 0 }, null));
		_dataArray.Add(new TravelingEventItem(10, 20, ETravelingEventDisplayType.TravelingEvent_0, ETravelingEventType.AreaMaterial, 21, new string[5] { "Character", "Item", "", "", "" }, 11, ETravelingEventTriggerType.Any, ETravelingEventTriggerAreaType.NormalArea, -1, isUnique: false, 0, 20, 0, new sbyte[2] { -100, 100 }, 6, null, 0, null, -1, -1, new List<PresetItemTemplateIdGroup>
		{
			new PresetItemTemplateIdGroup("Material", 42, 7)
		}, new short[9] { 0, 243, 81, 27, 9, 3, 1, 0, 0 }, null));
		_dataArray.Add(new TravelingEventItem(11, 22, ETravelingEventDisplayType.TravelingEvent_0, ETravelingEventType.AreaMaterial, 23, new string[5] { "Character", "Item", "", "", "" }, 12, ETravelingEventTriggerType.Any, ETravelingEventTriggerAreaType.NormalArea, -1, isUnique: false, 0, 20, 0, new sbyte[2] { -100, 100 }, 6, null, 0, null, -1, -1, new List<PresetItemTemplateIdGroup>
		{
			new PresetItemTemplateIdGroup("Material", 236, 42)
		}, new short[9] { 0, 243, 81, 27, 9, 3, 1, 0, 0 }, null));
		_dataArray.Add(new TravelingEventItem(12, 24, ETravelingEventDisplayType.TravelingEvent_0, ETravelingEventType.AreaMaterial, 25, new string[5] { "Character", "Item", "", "", "" }, 13, ETravelingEventTriggerType.Any, ETravelingEventTriggerAreaType.NormalArea, -1, isUnique: false, 0, 20, 0, new sbyte[2] { -100, 100 }, 6, null, 0, null, -1, -1, new List<PresetItemTemplateIdGroup>
		{
			new PresetItemTemplateIdGroup("Material", 70, 14)
		}, new short[9] { 0, 243, 81, 27, 9, 3, 1, 0, 0 }, null));
		_dataArray.Add(new TravelingEventItem(13, 26, ETravelingEventDisplayType.TravelingEvent_0, ETravelingEventType.AreaMaterial, 27, new string[5] { "Character", "Item", "", "", "" }, 14, ETravelingEventTriggerType.Any, ETravelingEventTriggerAreaType.NormalArea, -1, isUnique: false, 0, 20, 0, new sbyte[2] { -100, 100 }, 6, null, 0, null, -1, -1, new List<PresetItemTemplateIdGroup>
		{
			new PresetItemTemplateIdGroup("Material", 77, 7)
		}, new short[9] { 0, 243, 81, 27, 9, 3, 1, 0, 0 }, null));
		_dataArray.Add(new TravelingEventItem(14, 28, ETravelingEventDisplayType.TravelingEvent_0, ETravelingEventType.AreaMaterial, 29, new string[5] { "Character", "Item", "", "", "" }, 15, ETravelingEventTriggerType.Any, ETravelingEventTriggerAreaType.NormalArea, -1, isUnique: false, 0, 20, 0, new sbyte[2] { -100, 100 }, 6, null, 0, null, -1, -1, new List<PresetItemTemplateIdGroup>
		{
			new PresetItemTemplateIdGroup("Material", 236, 42)
		}, new short[9] { 0, 243, 81, 27, 9, 3, 1, 0, 0 }, null));
		_dataArray.Add(new TravelingEventItem(15, 30, ETravelingEventDisplayType.TravelingEvent_0, ETravelingEventType.AreaResource, 31, new string[5] { "Character", "Integer", "Resource", "", "" }, 1, ETravelingEventTriggerType.Any, ETravelingEventTriggerAreaType.NormalArea, -1, isUnique: false, 0, 20, 0, new sbyte[2] { -100, 100 }, 6, null, 0, new int[2] { 50, 150 }, -1, -1, new List<PresetItemTemplateIdGroup>(), null, new short[8] { 10, 5, 5, 5, 15, 5, 30, 15 }));
		_dataArray.Add(new TravelingEventItem(16, 32, ETravelingEventDisplayType.TravelingEvent_0, ETravelingEventType.AreaResource, 31, new string[5] { "Character", "Integer", "Resource", "", "" }, 2, ETravelingEventTriggerType.Any, ETravelingEventTriggerAreaType.NormalArea, -1, isUnique: false, 0, 20, 0, new sbyte[2] { -100, 100 }, 6, null, 0, new int[2] { 50, 150 }, -1, -1, new List<PresetItemTemplateIdGroup>(), null, new short[8] { 20, 10, 20, 5, 10, 5, 10, 10 }));
		_dataArray.Add(new TravelingEventItem(17, 33, ETravelingEventDisplayType.TravelingEvent_0, ETravelingEventType.AreaResource, 31, new string[5] { "Character", "Integer", "Resource", "", "" }, 3, ETravelingEventTriggerType.Any, ETravelingEventTriggerAreaType.NormalArea, -1, isUnique: false, 0, 20, 0, new sbyte[2] { -100, 100 }, 6, null, 0, new int[2] { 50, 150 }, -1, -1, new List<PresetItemTemplateIdGroup>(), null, new short[8] { 10, 10, 5, 10, 5, 30, 10, 10 }));
		_dataArray.Add(new TravelingEventItem(18, 34, ETravelingEventDisplayType.TravelingEvent_0, ETravelingEventType.AreaResource, 31, new string[5] { "Character", "Integer", "Resource", "", "" }, 4, ETravelingEventTriggerType.Any, ETravelingEventTriggerAreaType.NormalArea, -1, isUnique: false, 0, 20, 0, new sbyte[2] { -100, 100 }, 6, null, 0, new int[2] { 50, 150 }, -1, -1, new List<PresetItemTemplateIdGroup>(), null, new short[8] { 5, 10, 15, 15, 10, 10, 10, 15 }));
		_dataArray.Add(new TravelingEventItem(19, 35, ETravelingEventDisplayType.TravelingEvent_0, ETravelingEventType.AreaResource, 31, new string[5] { "Character", "Integer", "Resource", "", "" }, 5, ETravelingEventTriggerType.Any, ETravelingEventTriggerAreaType.NormalArea, -1, isUnique: false, 0, 20, 0, new sbyte[2] { -100, 100 }, 6, null, 0, new int[2] { 50, 150 }, -1, -1, new List<PresetItemTemplateIdGroup>(), null, new short[8] { 35, 10, 10, 10, 5, 5, 10, 5 }));
		_dataArray.Add(new TravelingEventItem(20, 36, ETravelingEventDisplayType.TravelingEvent_0, ETravelingEventType.AreaResource, 31, new string[5] { "Character", "Integer", "Resource", "", "" }, 6, ETravelingEventTriggerType.Any, ETravelingEventTriggerAreaType.NormalArea, -1, isUnique: false, 0, 20, 0, new sbyte[2] { -100, 100 }, 6, null, 0, new int[2] { 50, 150 }, -1, -1, new List<PresetItemTemplateIdGroup>(), null, new short[8] { 20, 5, 5, 5, 5, 10, 10, 30 }));
		_dataArray.Add(new TravelingEventItem(21, 37, ETravelingEventDisplayType.TravelingEvent_0, ETravelingEventType.AreaResource, 31, new string[5] { "Character", "Integer", "Resource", "", "" }, 7, ETravelingEventTriggerType.Any, ETravelingEventTriggerAreaType.NormalArea, -1, isUnique: false, 0, 20, 0, new sbyte[2] { -100, 100 }, 6, null, 0, new int[2] { 50, 150 }, -1, -1, new List<PresetItemTemplateIdGroup>(), null, new short[8] { 10, 5, 10, 10, 20, 10, 15, 10 }));
		_dataArray.Add(new TravelingEventItem(22, 38, ETravelingEventDisplayType.TravelingEvent_0, ETravelingEventType.AreaResource, 31, new string[5] { "Character", "Integer", "Resource", "", "" }, 8, ETravelingEventTriggerType.Any, ETravelingEventTriggerAreaType.NormalArea, -1, isUnique: false, 0, 20, 0, new sbyte[2] { -100, 100 }, 6, null, 0, new int[2] { 50, 150 }, -1, -1, new List<PresetItemTemplateIdGroup>(), null, new short[8] { 20, 10, 10, 10, 10, 10, 10, 10 }));
		_dataArray.Add(new TravelingEventItem(23, 39, ETravelingEventDisplayType.TravelingEvent_0, ETravelingEventType.AreaResource, 31, new string[5] { "Character", "Integer", "Resource", "", "" }, 9, ETravelingEventTriggerType.Any, ETravelingEventTriggerAreaType.NormalArea, -1, isUnique: false, 0, 20, 0, new sbyte[2] { -100, 100 }, 6, null, 0, new int[2] { 50, 150 }, -1, -1, new List<PresetItemTemplateIdGroup>(), null, new short[8] { 5, 20, 20, 20, 10, 5, 5, 5 }));
		_dataArray.Add(new TravelingEventItem(24, 40, ETravelingEventDisplayType.TravelingEvent_0, ETravelingEventType.AreaResource, 31, new string[5] { "Character", "Integer", "Resource", "", "" }, 10, ETravelingEventTriggerType.Any, ETravelingEventTriggerAreaType.NormalArea, -1, isUnique: false, 0, 20, 0, new sbyte[2] { -100, 100 }, 6, null, 0, new int[2] { 50, 150 }, -1, -1, new List<PresetItemTemplateIdGroup>(), null, new short[8] { 10, 15, 10, 10, 15, 20, 5, 5 }));
		_dataArray.Add(new TravelingEventItem(25, 41, ETravelingEventDisplayType.TravelingEvent_0, ETravelingEventType.AreaResource, 31, new string[5] { "Character", "Integer", "Resource", "", "" }, 11, ETravelingEventTriggerType.Any, ETravelingEventTriggerAreaType.NormalArea, -1, isUnique: false, 0, 20, 0, new sbyte[2] { -100, 100 }, 6, null, 0, new int[2] { 50, 150 }, -1, -1, new List<PresetItemTemplateIdGroup>(), null, new short[8] { 5, 5, 10, 30, 5, 5, 20, 10 }));
		_dataArray.Add(new TravelingEventItem(26, 42, ETravelingEventDisplayType.TravelingEvent_0, ETravelingEventType.AreaResource, 31, new string[5] { "Character", "Integer", "Resource", "", "" }, 12, ETravelingEventTriggerType.Any, ETravelingEventTriggerAreaType.NormalArea, -1, isUnique: false, 0, 20, 0, new sbyte[2] { -100, 100 }, 6, null, 0, new int[2] { 50, 150 }, -1, -1, new List<PresetItemTemplateIdGroup>(), null, new short[8] { 5, 20, 15, 15, 10, 15, 5, 5 }));
		_dataArray.Add(new TravelingEventItem(27, 43, ETravelingEventDisplayType.TravelingEvent_0, ETravelingEventType.AreaResource, 31, new string[5] { "Character", "Integer", "Resource", "", "" }, 13, ETravelingEventTriggerType.Any, ETravelingEventTriggerAreaType.NormalArea, -1, isUnique: false, 0, 20, 0, new sbyte[2] { -100, 100 }, 6, null, 0, new int[2] { 50, 150 }, -1, -1, new List<PresetItemTemplateIdGroup>(), null, new short[8] { 10, 10, 20, 10, 10, 10, 10, 10 }));
		_dataArray.Add(new TravelingEventItem(28, 44, ETravelingEventDisplayType.TravelingEvent_0, ETravelingEventType.AreaResource, 31, new string[5] { "Character", "Integer", "Resource", "", "" }, 14, ETravelingEventTriggerType.Any, ETravelingEventTriggerAreaType.NormalArea, -1, isUnique: false, 0, 20, 0, new sbyte[2] { -100, 100 }, 6, null, 0, new int[2] { 50, 150 }, -1, -1, new List<PresetItemTemplateIdGroup>(), null, new short[8] { 15, 10, 5, 5, 20, 5, 20, 10 }));
		_dataArray.Add(new TravelingEventItem(29, 45, ETravelingEventDisplayType.TravelingEvent_0, ETravelingEventType.AreaResource, 31, new string[5] { "Character", "Integer", "Resource", "", "" }, 15, ETravelingEventTriggerType.Any, ETravelingEventTriggerAreaType.NormalArea, -1, isUnique: false, 0, 20, 0, new sbyte[2] { -100, 100 }, 6, null, 0, new int[2] { 50, 150 }, -1, -1, new List<PresetItemTemplateIdGroup>(), null, new short[8] { 15, 20, 5, 5, 15, 5, 10, 15 }));
		_dataArray.Add(new TravelingEventItem(30, 46, ETravelingEventDisplayType.TravelingEvent_0, ETravelingEventType.AreaFood, 47, new string[5] { "Character", "Item", "", "", "" }, 1, ETravelingEventTriggerType.Any, ETravelingEventTriggerAreaType.NormalArea, -1, isUnique: false, 0, 20, 1, new sbyte[2] { -100, 100 }, -1, null, 0, null, -1, -1, new List<PresetItemTemplateIdGroup>
		{
			new PresetItemTemplateIdGroup("Food", 9, 42)
		}, new short[9] { 288, 96, 32, 16, 8, 4, 2, 1, 0 }, null));
		_dataArray.Add(new TravelingEventItem(31, 48, ETravelingEventDisplayType.TravelingEvent_0, ETravelingEventType.AreaFood, 49, new string[5] { "Character", "Item", "", "", "" }, 2, ETravelingEventTriggerType.Any, ETravelingEventTriggerAreaType.NormalArea, -1, isUnique: false, 0, 20, 1, new sbyte[2] { -100, 100 }, -1, null, 0, null, -1, -1, new List<PresetItemTemplateIdGroup>
		{
			new PresetItemTemplateIdGroup("Food", 51, 42)
		}, new short[9] { 288, 96, 32, 16, 8, 4, 2, 1, 0 }, null));
		_dataArray.Add(new TravelingEventItem(32, 50, ETravelingEventDisplayType.TravelingEvent_0, ETravelingEventType.AreaFood, 51, new string[5] { "Character", "Item", "", "", "" }, 3, ETravelingEventTriggerType.Any, ETravelingEventTriggerAreaType.NormalArea, -1, isUnique: false, 0, 20, 1, new sbyte[2] { -100, 100 }, -1, null, 0, null, -1, -1, new List<PresetItemTemplateIdGroup>
		{
			new PresetItemTemplateIdGroup("Food", 93, 42)
		}, new short[9] { 288, 96, 32, 16, 8, 4, 2, 1, 0 }, null));
		_dataArray.Add(new TravelingEventItem(33, 52, ETravelingEventDisplayType.TravelingEvent_0, ETravelingEventType.AreaFood, 53, new string[5] { "Character", "Item", "", "", "" }, 4, ETravelingEventTriggerType.Any, ETravelingEventTriggerAreaType.NormalArea, -1, isUnique: false, 0, 20, 1, new sbyte[2] { -100, 100 }, -1, null, 0, null, -1, -1, new List<PresetItemTemplateIdGroup>
		{
			new PresetItemTemplateIdGroup("Food", 9, 42)
		}, new short[9] { 288, 96, 32, 16, 8, 4, 2, 1, 0 }, null));
		_dataArray.Add(new TravelingEventItem(34, 54, ETravelingEventDisplayType.TravelingEvent_0, ETravelingEventType.AreaFood, 55, new string[5] { "Character", "Item", "", "", "" }, 5, ETravelingEventTriggerType.Any, ETravelingEventTriggerAreaType.NormalArea, -1, isUnique: false, 0, 20, 1, new sbyte[2] { -100, 100 }, -1, null, 0, null, -1, -1, new List<PresetItemTemplateIdGroup>
		{
			new PresetItemTemplateIdGroup("Food", 93, 42)
		}, new short[9] { 288, 96, 32, 16, 8, 4, 2, 1, 0 }, null));
		_dataArray.Add(new TravelingEventItem(35, 56, ETravelingEventDisplayType.TravelingEvent_0, ETravelingEventType.AreaFood, 57, new string[5] { "Character", "Item", "", "", "" }, 6, ETravelingEventTriggerType.Any, ETravelingEventTriggerAreaType.NormalArea, -1, isUnique: false, 0, 20, 1, new sbyte[2] { -100, 100 }, -1, null, 0, null, -1, -1, new List<PresetItemTemplateIdGroup>
		{
			new PresetItemTemplateIdGroup("Food", 0, 9)
		}, new short[9] { 288, 96, 32, 16, 8, 4, 2, 1, 0 }, null));
		_dataArray.Add(new TravelingEventItem(36, 58, ETravelingEventDisplayType.TravelingEvent_0, ETravelingEventType.AreaFood, 59, new string[5] { "Character", "Item", "", "", "" }, 7, ETravelingEventTriggerType.Any, ETravelingEventTriggerAreaType.NormalArea, -1, isUnique: false, 0, 20, 1, new sbyte[2] { -100, 100 }, -1, null, 0, null, -1, -1, new List<PresetItemTemplateIdGroup>
		{
			new PresetItemTemplateIdGroup("Food", 0, 9)
		}, new short[9] { 288, 96, 32, 16, 8, 4, 2, 1, 0 }, null));
		_dataArray.Add(new TravelingEventItem(37, 60, ETravelingEventDisplayType.TravelingEvent_0, ETravelingEventType.AreaFood, 61, new string[5] { "Character", "Item", "", "", "" }, 8, ETravelingEventTriggerType.Any, ETravelingEventTriggerAreaType.NormalArea, -1, isUnique: false, 0, 20, 1, new sbyte[2] { -100, 100 }, -1, null, 0, null, -1, -1, new List<PresetItemTemplateIdGroup>
		{
			new PresetItemTemplateIdGroup("Food", 135, 42)
		}, new short[9] { 288, 96, 32, 16, 8, 4, 2, 1, 0 }, null));
		_dataArray.Add(new TravelingEventItem(38, 62, ETravelingEventDisplayType.TravelingEvent_0, ETravelingEventType.AreaFood, 63, new string[5] { "Character", "Item", "", "", "" }, 9, ETravelingEventTriggerType.Any, ETravelingEventTriggerAreaType.NormalArea, -1, isUnique: false, 0, 20, 1, new sbyte[2] { -100, 100 }, -1, null, 0, null, -1, -1, new List<PresetItemTemplateIdGroup>
		{
			new PresetItemTemplateIdGroup("Food", 135, 42)
		}, new short[9] { 288, 96, 32, 16, 8, 4, 2, 1, 0 }, null));
		_dataArray.Add(new TravelingEventItem(39, 64, ETravelingEventDisplayType.TravelingEvent_0, ETravelingEventType.AreaFood, 65, new string[5] { "Character", "Item", "", "", "" }, 10, ETravelingEventTriggerType.Any, ETravelingEventTriggerAreaType.NormalArea, -1, isUnique: false, 0, 20, 1, new sbyte[2] { -100, 100 }, -1, null, 0, null, -1, -1, new List<PresetItemTemplateIdGroup>
		{
			new PresetItemTemplateIdGroup("Food", 51, 42)
		}, new short[9] { 288, 96, 32, 16, 8, 4, 2, 1, 0 }, null));
		_dataArray.Add(new TravelingEventItem(40, 66, ETravelingEventDisplayType.TravelingEvent_0, ETravelingEventType.AreaFood, 67, new string[5] { "Character", "Item", "", "", "" }, 11, ETravelingEventTriggerType.Any, ETravelingEventTriggerAreaType.NormalArea, -1, isUnique: false, 0, 20, 1, new sbyte[2] { -100, 100 }, -1, null, 0, null, -1, -1, new List<PresetItemTemplateIdGroup>
		{
			new PresetItemTemplateIdGroup("Food", 51, 42)
		}, new short[9] { 288, 96, 32, 16, 8, 4, 2, 1, 0 }, null));
		_dataArray.Add(new TravelingEventItem(41, 68, ETravelingEventDisplayType.TravelingEvent_0, ETravelingEventType.AreaFood, 69, new string[5] { "Character", "Item", "", "", "" }, 12, ETravelingEventTriggerType.Any, ETravelingEventTriggerAreaType.NormalArea, -1, isUnique: false, 0, 20, 1, new sbyte[2] { -100, 100 }, -1, null, 0, null, -1, -1, new List<PresetItemTemplateIdGroup>
		{
			new PresetItemTemplateIdGroup("Food", 0, 9)
		}, new short[9] { 288, 96, 32, 16, 8, 4, 2, 1, 0 }, null));
		_dataArray.Add(new TravelingEventItem(42, 70, ETravelingEventDisplayType.TravelingEvent_0, ETravelingEventType.AreaFood, 71, new string[5] { "Character", "Item", "", "", "" }, 13, ETravelingEventTriggerType.Any, ETravelingEventTriggerAreaType.NormalArea, -1, isUnique: false, 0, 20, 1, new sbyte[2] { -100, 100 }, -1, null, 0, null, -1, -1, new List<PresetItemTemplateIdGroup>
		{
			new PresetItemTemplateIdGroup("Food", 93, 42)
		}, new short[9] { 288, 96, 32, 16, 8, 4, 2, 1, 0 }, null));
		_dataArray.Add(new TravelingEventItem(43, 72, ETravelingEventDisplayType.TravelingEvent_0, ETravelingEventType.AreaFood, 73, new string[5] { "Character", "Item", "", "", "" }, 14, ETravelingEventTriggerType.Any, ETravelingEventTriggerAreaType.NormalArea, -1, isUnique: false, 0, 20, 1, new sbyte[2] { -100, 100 }, -1, null, 0, null, -1, -1, new List<PresetItemTemplateIdGroup>
		{
			new PresetItemTemplateIdGroup("Food", 135, 42)
		}, new short[9] { 288, 96, 32, 16, 8, 4, 2, 1, 0 }, null));
		_dataArray.Add(new TravelingEventItem(44, 74, ETravelingEventDisplayType.TravelingEvent_0, ETravelingEventType.AreaFood, 75, new string[5] { "Character", "Item", "", "", "" }, 15, ETravelingEventTriggerType.Any, ETravelingEventTriggerAreaType.NormalArea, -1, isUnique: false, 0, 20, 1, new sbyte[2] { -100, 100 }, -1, null, 0, null, -1, -1, new List<PresetItemTemplateIdGroup>
		{
			new PresetItemTemplateIdGroup("Food", 9, 42)
		}, new short[9] { 288, 96, 32, 16, 8, 4, 2, 1, 0 }, null));
		_dataArray.Add(new TravelingEventItem(45, 76, ETravelingEventDisplayType.TravelingEvent_0, ETravelingEventType.Heal, 77, new string[5] { "Character", "Integer", "", "", "" }, -1, ETravelingEventTriggerType.ToArea, ETravelingEventTriggerAreaType.NormalArea, -1, isUnique: false, 0, 5, 0, new sbyte[2] { -100, 100 }, 5, null, 0, new int[2] { 2, 3 }, -1, -1, new List<PresetItemTemplateIdGroup>(), null, null));
		_dataArray.Add(new TravelingEventItem(46, 78, ETravelingEventDisplayType.TravelingEvent_0, ETravelingEventType.Heal, 79, new string[5] { "Character", "Integer", "", "", "" }, -1, ETravelingEventTriggerType.ToArea, ETravelingEventTriggerAreaType.NormalArea, -1, isUnique: false, 0, 5, 0, new sbyte[2] { -100, 100 }, 5, null, 0, new int[2] { 2, 3 }, -1, -1, new List<PresetItemTemplateIdGroup>(), null, null));
		_dataArray.Add(new TravelingEventItem(47, 80, ETravelingEventDisplayType.TravelingEvent_0, ETravelingEventType.Heal, 81, new string[5] { "Character", "PoisonType", "", "", "" }, -1, ETravelingEventTriggerType.ToArea, ETravelingEventTriggerAreaType.NormalArea, -1, isUnique: false, 0, 5, 0, new sbyte[2] { -100, 100 }, 5, null, 0, new int[2] { 100, 150 }, -1, -1, new List<PresetItemTemplateIdGroup>(), null, null));
		_dataArray.Add(new TravelingEventItem(48, 82, ETravelingEventDisplayType.TravelingEvent_0, ETravelingEventType.Heal, 83, new string[5] { "Character", "Integer", "", "", "" }, -1, ETravelingEventTriggerType.ToArea, ETravelingEventTriggerAreaType.NormalArea, -1, isUnique: false, 0, 5, 0, new sbyte[2] { -100, 100 }, 5, null, 0, new int[2] { 250, 500 }, -1, -1, new List<PresetItemTemplateIdGroup>(), null, null));
		_dataArray.Add(new TravelingEventItem(49, 84, ETravelingEventDisplayType.TravelingEvent_0, ETravelingEventType.Heal, 85, new string[5] { "Character", "Integer", "", "", "" }, -1, ETravelingEventTriggerType.ToArea, ETravelingEventTriggerAreaType.NormalArea, -1, isUnique: false, 0, 5, 0, new sbyte[2] { -100, 100 }, 5, null, 0, new int[2] { 12, 24 }, -1, -1, new List<PresetItemTemplateIdGroup>(), null, null));
		_dataArray.Add(new TravelingEventItem(50, 86, ETravelingEventDisplayType.TravelingEvent_0, ETravelingEventType.CharacterGiftResource, 87, new string[5] { "Character", "Location", "Character", "Integer", "Resource" }, -1, ETravelingEventTriggerType.OnArea, ETravelingEventTriggerAreaType.NormalArea, -1, isUnique: false, 0, 20, 0, new sbyte[2] { -100, 100 }, -1, null, 0, null, -1, -1, new List<PresetItemTemplateIdGroup>(), null, null));
		_dataArray.Add(new TravelingEventItem(51, 88, ETravelingEventDisplayType.TravelingEvent_0, ETravelingEventType.CharacterGiftItem, 89, new string[5] { "Character", "Location", "Character", "Item", "" }, -1, ETravelingEventTriggerType.OnArea, ETravelingEventTriggerAreaType.NormalArea, -1, isUnique: false, 0, 20, 0, new sbyte[2] { -100, 100 }, -1, null, 0, null, -1, 7, new List<PresetItemTemplateIdGroup>(), null, null));
		_dataArray.Add(new TravelingEventItem(52, 90, ETravelingEventDisplayType.TravelingEvent_0, ETravelingEventType.CharacterGiftItem, 89, new string[5] { "Character", "Location", "Character", "Item", "" }, -1, ETravelingEventTriggerType.OnArea, ETravelingEventTriggerAreaType.NormalArea, -1, isUnique: false, 0, 20, 0, new sbyte[2] { -100, 100 }, -1, null, 0, null, -1, 9, new List<PresetItemTemplateIdGroup>(), null, null));
		_dataArray.Add(new TravelingEventItem(53, 91, ETravelingEventDisplayType.TravelingEvent_0, ETravelingEventType.CharacterGiftItem, 92, new string[5] { "Character", "Location", "Character", "Item", "" }, -1, ETravelingEventTriggerType.OnArea, ETravelingEventTriggerAreaType.NormalArea, -1, isUnique: false, 0, 20, 0, new sbyte[2] { -100, 100 }, -1, null, 0, null, -1, 8, new List<PresetItemTemplateIdGroup>(), null, null));
		_dataArray.Add(new TravelingEventItem(54, 93, ETravelingEventDisplayType.TravelingEvent_0, ETravelingEventType.CharacterGiftResource, 94, new string[5] { "Character", "Location", "Character", "Integer", "Resource" }, -1, ETravelingEventTriggerType.OnArea, ETravelingEventTriggerAreaType.NormalArea, -1, isUnique: false, 0, 15, 1, new sbyte[2] { -100, 100 }, -1, null, 0, null, -1, -1, new List<PresetItemTemplateIdGroup>(), null, null));
		_dataArray.Add(new TravelingEventItem(55, 95, ETravelingEventDisplayType.TravelingEvent_0, ETravelingEventType.CharacterGiftItem, 96, new string[5] { "Character", "Location", "Character", "Item", "" }, -1, ETravelingEventTriggerType.OnArea, ETravelingEventTriggerAreaType.NormalArea, -1, isUnique: false, 0, 15, 1, new sbyte[2] { -100, 100 }, -1, null, 0, null, -1, 7, new List<PresetItemTemplateIdGroup>(), null, null));
		_dataArray.Add(new TravelingEventItem(56, 97, ETravelingEventDisplayType.TravelingEvent_0, ETravelingEventType.CharacterGiftItem, 96, new string[5] { "Character", "Location", "Character", "Item", "" }, -1, ETravelingEventTriggerType.OnArea, ETravelingEventTriggerAreaType.NormalArea, -1, isUnique: false, 0, 15, 1, new sbyte[2] { -100, 100 }, -1, null, 0, null, -1, 9, new List<PresetItemTemplateIdGroup>(), null, null));
		_dataArray.Add(new TravelingEventItem(57, 98, ETravelingEventDisplayType.TravelingEvent_0, ETravelingEventType.CharacterGiftItem, 92, new string[5] { "Character", "Location", "Character", "Item", "" }, -1, ETravelingEventTriggerType.OnArea, ETravelingEventTriggerAreaType.NormalArea, -1, isUnique: false, 0, 15, 1, new sbyte[2] { -100, 100 }, -1, null, 0, null, -1, 8, new List<PresetItemTemplateIdGroup>(), null, null));
		_dataArray.Add(new TravelingEventItem(58, 99, ETravelingEventDisplayType.TravelingEvent_0, ETravelingEventType.AttributeRegen, 100, new string[5] { "Character", "Location", "Integer", "", "" }, -1, ETravelingEventTriggerType.ToArea, ETravelingEventTriggerAreaType.NormalArea, -1, isUnique: false, 0, 3, 0, new sbyte[2] { -100, 100 }, -1, null, 0, null, 0, -1, new List<PresetItemTemplateIdGroup>(), null, null));
		_dataArray.Add(new TravelingEventItem(59, 101, ETravelingEventDisplayType.TravelingEvent_0, ETravelingEventType.AttributeRegen, 102, new string[5] { "Character", "Location", "Integer", "", "" }, -1, ETravelingEventTriggerType.ToArea, ETravelingEventTriggerAreaType.NormalArea, -1, isUnique: false, 0, 3, 0, new sbyte[2] { -100, 100 }, -1, null, 0, null, 1, -1, new List<PresetItemTemplateIdGroup>(), null, null));
	}

	private void CreateItems1()
	{
		_dataArray.Add(new TravelingEventItem(60, 103, ETravelingEventDisplayType.TravelingEvent_0, ETravelingEventType.AttributeRegen, 104, new string[5] { "Character", "Location", "Integer", "", "" }, -1, ETravelingEventTriggerType.ToArea, ETravelingEventTriggerAreaType.NormalArea, -1, isUnique: false, 0, 3, 0, new sbyte[2] { -100, 100 }, -1, null, 0, null, 2, -1, new List<PresetItemTemplateIdGroup>(), null, null));
		_dataArray.Add(new TravelingEventItem(61, 105, ETravelingEventDisplayType.TravelingEvent_0, ETravelingEventType.AttributeRegen, 106, new string[5] { "Character", "Location", "Integer", "", "" }, -1, ETravelingEventTriggerType.ToArea, ETravelingEventTriggerAreaType.NormalArea, -1, isUnique: false, 0, 3, 0, new sbyte[2] { -100, 100 }, -1, null, 0, null, 3, -1, new List<PresetItemTemplateIdGroup>(), null, null));
		_dataArray.Add(new TravelingEventItem(62, 107, ETravelingEventDisplayType.TravelingEvent_0, ETravelingEventType.AttributeRegen, 108, new string[5] { "Character", "Location", "Integer", "", "" }, -1, ETravelingEventTriggerType.ToArea, ETravelingEventTriggerAreaType.NormalArea, -1, isUnique: false, 0, 3, 0, new sbyte[2] { -100, 100 }, -1, null, 0, null, 4, -1, new List<PresetItemTemplateIdGroup>(), null, null));
		_dataArray.Add(new TravelingEventItem(63, 109, ETravelingEventDisplayType.TravelingEvent_0, ETravelingEventType.AttributeRegen, 110, new string[5] { "Character", "Location", "Integer", "", "" }, -1, ETravelingEventTriggerType.ToArea, ETravelingEventTriggerAreaType.NormalArea, -1, isUnique: false, 0, 3, 0, new sbyte[2] { -100, 100 }, -1, null, 0, null, 5, -1, new List<PresetItemTemplateIdGroup>(), null, null));
		_dataArray.Add(new TravelingEventItem(64, 111, ETravelingEventDisplayType.TravelingEvent_0, ETravelingEventType.Notification, 112, new string[5] { "Character", "Location", "Character", "", "" }, -1, ETravelingEventTriggerType.Any, ETravelingEventTriggerAreaType.Any, -1, isUnique: false, 0, 0, 0, new sbyte[2] { -100, 100 }, -1, null, 0, null, -1, -1, new List<PresetItemTemplateIdGroup>(), null, null));
		_dataArray.Add(new TravelingEventItem(65, 113, ETravelingEventDisplayType.TravelingEvent_0, ETravelingEventType.Notification, 114, new string[5] { "Character", "Location", "Character", "", "" }, -1, ETravelingEventTriggerType.Any, ETravelingEventTriggerAreaType.Any, -1, isUnique: false, 0, 0, 0, new sbyte[2] { -100, 100 }, -1, null, 0, null, -1, -1, new List<PresetItemTemplateIdGroup>(), null, null));
		_dataArray.Add(new TravelingEventItem(66, 115, ETravelingEventDisplayType.TravelingEvent_0, ETravelingEventType.Notification, 116, new string[5] { "Character", "Location", "Character", "", "" }, -1, ETravelingEventTriggerType.Any, ETravelingEventTriggerAreaType.Any, -1, isUnique: false, 0, 0, 0, new sbyte[2] { -100, 100 }, -1, null, 0, null, -1, -1, new List<PresetItemTemplateIdGroup>(), null, null));
		_dataArray.Add(new TravelingEventItem(67, 117, ETravelingEventDisplayType.TravelingEvent_0, ETravelingEventType.Notification, 118, new string[5] { "Character", "Location", "Character", "", "" }, -1, ETravelingEventTriggerType.Any, ETravelingEventTriggerAreaType.Any, -1, isUnique: false, 0, 0, 0, new sbyte[2] { -100, 100 }, -1, null, 0, null, -1, -1, new List<PresetItemTemplateIdGroup>(), null, null));
		_dataArray.Add(new TravelingEventItem(68, 119, ETravelingEventDisplayType.TravelingEvent_0, ETravelingEventType.Notification, 120, new string[5] { "Character", "Settlement", "CharacterPropertyReferencedType", "Float", "" }, -1, ETravelingEventTriggerType.Any, ETravelingEventTriggerAreaType.Any, -1, isUnique: false, 0, 0, 0, new sbyte[2] { -100, 100 }, -1, null, 0, null, -1, -1, new List<PresetItemTemplateIdGroup>(), null, null));
		_dataArray.Add(new TravelingEventItem(69, 121, ETravelingEventDisplayType.TravelingEvent_0, ETravelingEventType.Notification, 122, new string[5] { "Character", "Settlement", "CharacterPropertyReferencedType", "Float", "" }, -1, ETravelingEventTriggerType.Any, ETravelingEventTriggerAreaType.Any, -1, isUnique: false, 0, 0, 0, new sbyte[2] { -100, 100 }, -1, null, 0, null, -1, -1, new List<PresetItemTemplateIdGroup>(), null, null));
		_dataArray.Add(new TravelingEventItem(70, 123, ETravelingEventDisplayType.TravelingEvent_0, ETravelingEventType.Notification, 124, new string[5] { "Character", "Settlement", "CharacterPropertyReferencedType", "Float", "" }, -1, ETravelingEventTriggerType.Any, ETravelingEventTriggerAreaType.Any, -1, isUnique: false, 0, 0, 0, new sbyte[2] { -100, 100 }, -1, null, 0, null, -1, -1, new List<PresetItemTemplateIdGroup>(), null, null));
		_dataArray.Add(new TravelingEventItem(71, 125, ETravelingEventDisplayType.TravelingEvent_0, ETravelingEventType.Notification, 126, new string[5] { "Character", "Settlement", "CharacterPropertyReferencedType", "Float", "" }, -1, ETravelingEventTriggerType.Any, ETravelingEventTriggerAreaType.Any, -1, isUnique: false, 0, 0, 0, new sbyte[2] { -100, 100 }, -1, null, 0, null, -1, -1, new List<PresetItemTemplateIdGroup>(), null, null));
		_dataArray.Add(new TravelingEventItem(72, 127, ETravelingEventDisplayType.TravelingEvent_0, ETravelingEventType.Notification, 128, new string[5] { "Character", "Settlement", "CharacterPropertyReferencedType", "Float", "" }, -1, ETravelingEventTriggerType.Any, ETravelingEventTriggerAreaType.Any, -1, isUnique: false, 0, 0, 0, new sbyte[2] { -100, 100 }, -1, null, 0, null, -1, -1, new List<PresetItemTemplateIdGroup>(), null, null));
		_dataArray.Add(new TravelingEventItem(73, 129, ETravelingEventDisplayType.TravelingEvent_0, ETravelingEventType.Notification, 130, new string[5] { "Character", "Settlement", "CharacterPropertyReferencedType", "Float", "" }, -1, ETravelingEventTriggerType.Any, ETravelingEventTriggerAreaType.Any, -1, isUnique: false, 0, 0, 0, new sbyte[2] { -100, 100 }, -1, null, 0, null, -1, -1, new List<PresetItemTemplateIdGroup>(), null, null));
		_dataArray.Add(new TravelingEventItem(74, 131, ETravelingEventDisplayType.TravelingEvent_0, ETravelingEventType.Notification, 132, new string[5] { "Character", "Settlement", "CharacterPropertyReferencedType", "Float", "" }, -1, ETravelingEventTriggerType.Any, ETravelingEventTriggerAreaType.Any, -1, isUnique: false, 0, 0, 0, new sbyte[2] { -100, 100 }, -1, null, 0, null, -1, -1, new List<PresetItemTemplateIdGroup>(), null, null));
		_dataArray.Add(new TravelingEventItem(75, 133, ETravelingEventDisplayType.TravelingEvent_0, ETravelingEventType.Notification, 134, new string[5] { "Character", "Settlement", "CharacterPropertyReferencedType", "Float", "" }, -1, ETravelingEventTriggerType.Any, ETravelingEventTriggerAreaType.Any, -1, isUnique: false, 0, 0, 0, new sbyte[2] { -100, 100 }, -1, null, 0, null, -1, -1, new List<PresetItemTemplateIdGroup>(), null, null));
		_dataArray.Add(new TravelingEventItem(76, 135, ETravelingEventDisplayType.TravelingEvent_0, ETravelingEventType.Notification, 136, new string[5] { "Character", "Settlement", "CharacterPropertyReferencedType", "Float", "" }, -1, ETravelingEventTriggerType.Any, ETravelingEventTriggerAreaType.Any, -1, isUnique: false, 0, 0, 0, new sbyte[2] { -100, 100 }, -1, null, 0, null, -1, -1, new List<PresetItemTemplateIdGroup>(), null, null));
		_dataArray.Add(new TravelingEventItem(77, 137, ETravelingEventDisplayType.TravelingEvent_0, ETravelingEventType.Notification, 138, new string[5] { "Character", "Settlement", "CharacterPropertyReferencedType", "Float", "" }, -1, ETravelingEventTriggerType.Any, ETravelingEventTriggerAreaType.Any, -1, isUnique: false, 0, 0, 0, new sbyte[2] { -100, 100 }, -1, null, 0, null, -1, -1, new List<PresetItemTemplateIdGroup>(), null, null));
		_dataArray.Add(new TravelingEventItem(78, 139, ETravelingEventDisplayType.TravelingEvent_0, ETravelingEventType.Notification, 140, new string[5] { "Character", "Settlement", "CharacterPropertyReferencedType", "Float", "" }, -1, ETravelingEventTriggerType.Any, ETravelingEventTriggerAreaType.Any, -1, isUnique: false, 0, 0, 0, new sbyte[2] { -100, 100 }, -1, null, 0, null, -1, -1, new List<PresetItemTemplateIdGroup>(), null, null));
		_dataArray.Add(new TravelingEventItem(79, 141, ETravelingEventDisplayType.TravelingEvent_0, ETravelingEventType.Notification, 142, new string[5] { "Character", "Settlement", "CharacterPropertyReferencedType", "Float", "" }, -1, ETravelingEventTriggerType.Any, ETravelingEventTriggerAreaType.Any, -1, isUnique: false, 0, 0, 0, new sbyte[2] { -100, 100 }, -1, null, 0, null, -1, -1, new List<PresetItemTemplateIdGroup>(), null, null));
		_dataArray.Add(new TravelingEventItem(80, 143, ETravelingEventDisplayType.TravelingEvent_0, ETravelingEventType.Notification, 144, new string[5] { "Character", "Settlement", "CharacterPropertyReferencedType", "Float", "" }, -1, ETravelingEventTriggerType.Any, ETravelingEventTriggerAreaType.Any, -1, isUnique: false, 0, 0, 0, new sbyte[2] { -100, 100 }, -1, null, 0, null, -1, -1, new List<PresetItemTemplateIdGroup>(), null, null));
		_dataArray.Add(new TravelingEventItem(81, 145, ETravelingEventDisplayType.TravelingEvent_0, ETravelingEventType.Notification, 146, new string[5] { "Character", "Settlement", "CharacterPropertyReferencedType", "Float", "" }, -1, ETravelingEventTriggerType.Any, ETravelingEventTriggerAreaType.Any, -1, isUnique: false, 0, 0, 0, new sbyte[2] { -100, 100 }, -1, null, 0, null, -1, -1, new List<PresetItemTemplateIdGroup>(), null, null));
		_dataArray.Add(new TravelingEventItem(82, 147, ETravelingEventDisplayType.TravelingEvent_0, ETravelingEventType.Notification, 148, new string[5] { "Character", "Settlement", "CharacterPropertyReferencedType", "Float", "" }, -1, ETravelingEventTriggerType.Any, ETravelingEventTriggerAreaType.Any, -1, isUnique: false, 0, 0, 0, new sbyte[2] { -100, 100 }, -1, null, 0, null, -1, -1, new List<PresetItemTemplateIdGroup>(), null, null));
		_dataArray.Add(new TravelingEventItem(83, 149, ETravelingEventDisplayType.TravelingEvent_0, ETravelingEventType.Notification, 150, new string[5] { "Character", "Settlement", "", "", "" }, -1, ETravelingEventTriggerType.Any, ETravelingEventTriggerAreaType.Any, -1, isUnique: false, 0, 0, 0, new sbyte[2] { -100, 100 }, -1, null, 0, null, -1, -1, new List<PresetItemTemplateIdGroup>(), null, null));
		_dataArray.Add(new TravelingEventItem(84, 151, ETravelingEventDisplayType.TravelingEvent_0, ETravelingEventType.Notification, 152, new string[5] { "Character", "CharacterTemplate", "", "", "" }, -1, ETravelingEventTriggerType.Any, ETravelingEventTriggerAreaType.Any, -1, isUnique: false, 0, 0, 0, new sbyte[2] { -100, 100 }, -1, null, 0, null, -1, -1, new List<PresetItemTemplateIdGroup>(), null, null));
		_dataArray.Add(new TravelingEventItem(85, 153, ETravelingEventDisplayType.TravelingEvent_0, ETravelingEventType.Notification, 154, new string[5] { "Character", "CharacterTemplate", "", "", "" }, -1, ETravelingEventTriggerType.Any, ETravelingEventTriggerAreaType.Any, -1, isUnique: false, 0, 0, 0, new sbyte[2] { -100, 100 }, -1, null, 0, null, -1, -1, new List<PresetItemTemplateIdGroup>(), null, null));
		_dataArray.Add(new TravelingEventItem(86, 155, ETravelingEventDisplayType.TravelingEvent_0, ETravelingEventType.Notification, 156, new string[5] { "Character", "CharacterTemplate", "", "", "" }, -1, ETravelingEventTriggerType.Any, ETravelingEventTriggerAreaType.Any, -1, isUnique: false, 0, 0, 0, new sbyte[2] { -100, 100 }, -1, null, 0, null, -1, -1, new List<PresetItemTemplateIdGroup>(), null, null));
		_dataArray.Add(new TravelingEventItem(87, 157, ETravelingEventDisplayType.TravelingEvent_0, ETravelingEventType.Notification, 158, new string[5] { "Character", "Character", "Character", "", "" }, -1, ETravelingEventTriggerType.Any, ETravelingEventTriggerAreaType.Any, -1, isUnique: false, 0, 0, 0, new sbyte[2] { -100, 100 }, -1, null, 0, null, -1, -1, new List<PresetItemTemplateIdGroup>(), null, null));
		_dataArray.Add(new TravelingEventItem(88, 159, ETravelingEventDisplayType.TravelingEvent_0, ETravelingEventType.Notification, 160, new string[5] { "Character", "Character", "", "", "" }, -1, ETravelingEventTriggerType.Any, ETravelingEventTriggerAreaType.Any, -1, isUnique: false, 0, 0, 0, new sbyte[2] { -100, 100 }, -1, null, 0, null, -1, -1, new List<PresetItemTemplateIdGroup>(), null, null));
		_dataArray.Add(new TravelingEventItem(89, 161, ETravelingEventDisplayType.TravelingEvent_0, ETravelingEventType.Notification, 162, new string[5] { "Character", "Character", "", "", "" }, -1, ETravelingEventTriggerType.Any, ETravelingEventTriggerAreaType.Any, -1, isUnique: false, 0, 0, 0, new sbyte[2] { -100, 100 }, -1, null, 0, null, -1, -1, new List<PresetItemTemplateIdGroup>(), null, null));
		_dataArray.Add(new TravelingEventItem(90, 163, ETravelingEventDisplayType.TravelingEvent_0, ETravelingEventType.Notification, 164, new string[5] { "Character", "Integer", "", "", "" }, -1, ETravelingEventTriggerType.Any, ETravelingEventTriggerAreaType.Any, -1, isUnique: false, 0, 0, 0, new sbyte[2] { -100, 100 }, -1, null, 0, null, -1, -1, new List<PresetItemTemplateIdGroup>(), null, null));
		_dataArray.Add(new TravelingEventItem(91, 165, ETravelingEventDisplayType.TravelingEvent_0, ETravelingEventType.Notification, 166, new string[5] { "Character", "Integer", "", "", "" }, -1, ETravelingEventTriggerType.Any, ETravelingEventTriggerAreaType.Any, -1, isUnique: false, 0, 0, 0, new sbyte[2] { -100, 100 }, -1, null, 0, null, -1, -1, new List<PresetItemTemplateIdGroup>(), null, null));
		_dataArray.Add(new TravelingEventItem(92, 167, ETravelingEventDisplayType.TravelingEvent_0, ETravelingEventType.Notification, 168, new string[5] { "Character", "Integer", "", "", "" }, -1, ETravelingEventTriggerType.Any, ETravelingEventTriggerAreaType.Any, -1, isUnique: false, 0, 0, 0, new sbyte[2] { -100, 100 }, -1, null, 0, null, -1, -1, new List<PresetItemTemplateIdGroup>(), null, null));
		_dataArray.Add(new TravelingEventItem(93, 169, ETravelingEventDisplayType.TravelingEvent_0, ETravelingEventType.Notification, 170, new string[5] { "Character", "Integer", "", "", "" }, -1, ETravelingEventTriggerType.Any, ETravelingEventTriggerAreaType.Any, -1, isUnique: false, 0, 0, 0, new sbyte[2] { -100, 100 }, -1, null, 0, null, -1, -1, new List<PresetItemTemplateIdGroup>(), null, null));
		_dataArray.Add(new TravelingEventItem(94, 171, ETravelingEventDisplayType.TravelingEvent_0, ETravelingEventType.Notification, 172, new string[5] { "Character", "Integer", "", "", "" }, -1, ETravelingEventTriggerType.Any, ETravelingEventTriggerAreaType.Any, -1, isUnique: false, 0, 0, 0, new sbyte[2] { -100, 100 }, -1, null, 0, null, -1, -1, new List<PresetItemTemplateIdGroup>(), null, null));
		_dataArray.Add(new TravelingEventItem(95, 173, ETravelingEventDisplayType.TravelingEvent_0, ETravelingEventType.Notification, 174, new string[5] { "Character", "Integer", "", "", "" }, -1, ETravelingEventTriggerType.Any, ETravelingEventTriggerAreaType.Any, -1, isUnique: false, 0, 0, 0, new sbyte[2] { -100, 100 }, -1, null, 0, null, -1, -1, new List<PresetItemTemplateIdGroup>(), null, null));
		_dataArray.Add(new TravelingEventItem(96, 175, ETravelingEventDisplayType.TravelingEvent_0, ETravelingEventType.Notification, 176, new string[5] { "Character", "", "", "", "" }, -1, ETravelingEventTriggerType.Any, ETravelingEventTriggerAreaType.Any, -1, isUnique: false, 0, 0, 0, new sbyte[2] { -100, 100 }, -1, null, 0, null, -1, -1, new List<PresetItemTemplateIdGroup>(), null, null));
		_dataArray.Add(new TravelingEventItem(97, 177, ETravelingEventDisplayType.TravelingEvent_0, ETravelingEventType.Notification, 178, new string[5] { "Character", "Integer", "", "", "" }, -1, ETravelingEventTriggerType.Any, ETravelingEventTriggerAreaType.Any, -1, isUnique: false, 0, 0, 0, new sbyte[2] { -100, 100 }, -1, null, 0, null, -1, -1, new List<PresetItemTemplateIdGroup>(), null, null));
		_dataArray.Add(new TravelingEventItem(98, 179, ETravelingEventDisplayType.TravelingEvent_0, ETravelingEventType.Notification, 180, new string[5] { "Character", "ItemKey", "", "", "" }, -1, ETravelingEventTriggerType.Any, ETravelingEventTriggerAreaType.Any, -1, isUnique: false, 0, 0, 0, new sbyte[2] { -100, 100 }, -1, null, 0, null, -1, -1, new List<PresetItemTemplateIdGroup>(), null, null));
		_dataArray.Add(new TravelingEventItem(99, 181, ETravelingEventDisplayType.TravelingEvent_1, ETravelingEventType.AreaInteraction, 182, new string[5] { "Character", "Location", "Character", "", "" }, 1, ETravelingEventTriggerType.OnArea, ETravelingEventTriggerAreaType.NormalArea, -1, isUnique: false, 0, 15, 0, new sbyte[2] { -100, 100 }, 0, "6c26bab3-5247-4c97-a8f8-e7253e58a13e", 3, null, -1, -1, new List<PresetItemTemplateIdGroup>(), null, null));
		_dataArray.Add(new TravelingEventItem(100, 183, ETravelingEventDisplayType.TravelingEvent_1, ETravelingEventType.AreaInteraction, 184, new string[5] { "Character", "Location", "Character", "", "" }, 2, ETravelingEventTriggerType.OnArea, ETravelingEventTriggerAreaType.NormalArea, -1, isUnique: false, 0, 15, 0, new sbyte[2] { -100, 100 }, 0, "fcdca37f-3f5c-4db4-8f3a-d3067e03cb39", 3, null, -1, -1, new List<PresetItemTemplateIdGroup>(), null, null));
		_dataArray.Add(new TravelingEventItem(101, 185, ETravelingEventDisplayType.TravelingEvent_1, ETravelingEventType.AreaInteraction, 186, new string[5] { "Character", "Location", "Character", "", "" }, 3, ETravelingEventTriggerType.OnArea, ETravelingEventTriggerAreaType.NormalArea, -1, isUnique: false, 0, 15, 0, new sbyte[2] { -100, 100 }, 2, "a7b3540b-23d9-4bbc-a8f9-7d755ac0b00b", 3, null, -1, -1, new List<PresetItemTemplateIdGroup>(), null, null));
		_dataArray.Add(new TravelingEventItem(102, 187, ETravelingEventDisplayType.TravelingEvent_1, ETravelingEventType.AreaInteraction, 188, new string[5] { "Character", "Location", "Character", "", "" }, 4, ETravelingEventTriggerType.OnArea, ETravelingEventTriggerAreaType.NormalArea, -1, isUnique: false, 0, 15, 0, new sbyte[2] { -100, 100 }, 4, "05314993-0ab3-4367-811b-127d68ebbf6d", 3, null, -1, -1, new List<PresetItemTemplateIdGroup>(), null, null));
		_dataArray.Add(new TravelingEventItem(103, 189, ETravelingEventDisplayType.TravelingEvent_1, ETravelingEventType.AreaInteraction, 190, new string[5] { "Character", "Location", "Character", "", "" }, 5, ETravelingEventTriggerType.OnArea, ETravelingEventTriggerAreaType.NormalArea, -1, isUnique: false, 0, 15, 0, new sbyte[2] { -100, 100 }, 1, "aa4de24b-f63d-4dc0-8805-81f31ad18ab8", 3, null, -1, -1, new List<PresetItemTemplateIdGroup>(), null, null));
		_dataArray.Add(new TravelingEventItem(104, 191, ETravelingEventDisplayType.TravelingEvent_1, ETravelingEventType.AreaInteraction, 192, new string[5] { "Character", "Location", "Character", "", "" }, 6, ETravelingEventTriggerType.OnArea, ETravelingEventTriggerAreaType.NormalArea, -1, isUnique: false, 0, 15, 0, new sbyte[2] { -100, 100 }, 2, "535d8d3c-3b7a-4d87-8f72-fc4db03784d5", 3, null, -1, -1, new List<PresetItemTemplateIdGroup>(), null, null));
		_dataArray.Add(new TravelingEventItem(105, 193, ETravelingEventDisplayType.TravelingEvent_1, ETravelingEventType.AreaInteraction, 194, new string[5] { "Character", "Location", "Character", "", "" }, 7, ETravelingEventTriggerType.OnArea, ETravelingEventTriggerAreaType.NormalArea, -1, isUnique: false, 0, 15, 0, new sbyte[2] { -100, 100 }, 1, "c3fec540-df56-4d5e-bf08-b4555e18d2ca", 3, null, -1, -1, new List<PresetItemTemplateIdGroup>(), null, null));
		_dataArray.Add(new TravelingEventItem(106, 195, ETravelingEventDisplayType.TravelingEvent_1, ETravelingEventType.AreaInteraction, 196, new string[5] { "Character", "Location", "Character", "", "" }, 8, ETravelingEventTriggerType.OnArea, ETravelingEventTriggerAreaType.NormalArea, -1, isUnique: false, 0, 15, 0, new sbyte[2] { -100, 100 }, 0, "51cdb901-42e2-45da-a559-d476fd2425ff", 3, null, -1, -1, new List<PresetItemTemplateIdGroup>(), null, null));
		_dataArray.Add(new TravelingEventItem(107, 197, ETravelingEventDisplayType.TravelingEvent_1, ETravelingEventType.AreaInteraction, 198, new string[5] { "Character", "Location", "Character", "", "" }, 9, ETravelingEventTriggerType.OnArea, ETravelingEventTriggerAreaType.NormalArea, -1, isUnique: false, 0, 15, 0, new sbyte[2] { -100, 100 }, 4, "f7f252c7-e095-4cb4-bbb4-d9bee30db040", 3, null, -1, -1, new List<PresetItemTemplateIdGroup>(), null, null));
		_dataArray.Add(new TravelingEventItem(108, 199, ETravelingEventDisplayType.TravelingEvent_1, ETravelingEventType.AreaInteraction, 200, new string[5] { "Character", "Location", "Character", "", "" }, 10, ETravelingEventTriggerType.OnArea, ETravelingEventTriggerAreaType.NormalArea, -1, isUnique: false, 0, 15, 0, new sbyte[2] { -100, 100 }, 3, "7af98dc9-d130-4638-94e4-c764645f29fc", 3, null, -1, -1, new List<PresetItemTemplateIdGroup>(), null, null));
		_dataArray.Add(new TravelingEventItem(109, 201, ETravelingEventDisplayType.TravelingEvent_1, ETravelingEventType.AreaInteraction, 202, new string[5] { "Character", "Location", "Character", "", "" }, 11, ETravelingEventTriggerType.OnArea, ETravelingEventTriggerAreaType.NormalArea, -1, isUnique: false, 0, 15, 0, new sbyte[2] { -100, 100 }, 3, "c62f67a8-b4d6-4a38-a8d3-9a115ec7d940", 3, null, -1, -1, new List<PresetItemTemplateIdGroup>(), null, null));
		_dataArray.Add(new TravelingEventItem(110, 203, ETravelingEventDisplayType.TravelingEvent_1, ETravelingEventType.AreaInteraction, 204, new string[5] { "Character", "Location", "Character", "", "" }, 12, ETravelingEventTriggerType.OnArea, ETravelingEventTriggerAreaType.NormalArea, -1, isUnique: false, 0, 15, 0, new sbyte[2] { -100, 100 }, 2, "14bd1d59-57a9-4835-808e-8e00f7d08b4a", 3, null, -1, -1, new List<PresetItemTemplateIdGroup>(), null, null));
		_dataArray.Add(new TravelingEventItem(111, 205, ETravelingEventDisplayType.TravelingEvent_1, ETravelingEventType.AreaInteraction, 206, new string[5] { "Character", "Location", "Character", "", "" }, 13, ETravelingEventTriggerType.OnArea, ETravelingEventTriggerAreaType.NormalArea, -1, isUnique: false, 0, 15, 0, new sbyte[2] { -100, 100 }, 3, "24f51c67-7d85-43a2-ba58-1ba785ae6ee7", 3, null, -1, -1, new List<PresetItemTemplateIdGroup>(), null, null));
		_dataArray.Add(new TravelingEventItem(112, 207, ETravelingEventDisplayType.TravelingEvent_1, ETravelingEventType.AreaInteraction, 208, new string[5] { "Character", "Location", "Character", "", "" }, 14, ETravelingEventTriggerType.OnArea, ETravelingEventTriggerAreaType.NormalArea, -1, isUnique: false, 0, 15, 0, new sbyte[2] { -100, 100 }, 1, "f496a372-c892-48db-9dad-390a18d3d26a", 3, null, -1, -1, new List<PresetItemTemplateIdGroup>(), null, null));
		_dataArray.Add(new TravelingEventItem(113, 209, ETravelingEventDisplayType.TravelingEvent_1, ETravelingEventType.AreaInteraction, 210, new string[5] { "Character", "Location", "Character", "", "" }, 15, ETravelingEventTriggerType.OnArea, ETravelingEventTriggerAreaType.NormalArea, -1, isUnique: false, 0, 15, 0, new sbyte[2] { -100, 100 }, 4, "180336cb-0310-4a55-bdae-e937a1310d6c", 3, null, -1, -1, new List<PresetItemTemplateIdGroup>(), null, null));
		_dataArray.Add(new TravelingEventItem(114, 211, ETravelingEventDisplayType.TravelingEvent_1, ETravelingEventType.SpiritualDebt, 212, new string[5] { "Character", "Settlement", "", "", "" }, 1, ETravelingEventTriggerType.OnArea, ETravelingEventTriggerAreaType.NormalArea, -1, isUnique: false, 0, 15, 0, new sbyte[2] { -100, 100 }, 2, "9f6e1296-c192-4f09-8a27-21b23cf6c716", 3, null, -1, -1, new List<PresetItemTemplateIdGroup>(), null, null));
		_dataArray.Add(new TravelingEventItem(115, 213, ETravelingEventDisplayType.TravelingEvent_1, ETravelingEventType.SpiritualDebt, 214, new string[5] { "Character", "Settlement", "", "", "" }, 2, ETravelingEventTriggerType.OnArea, ETravelingEventTriggerAreaType.NormalArea, -1, isUnique: false, 0, 15, 0, new sbyte[2] { -100, 100 }, 4, "b1000d73-c563-487b-b96c-764584b1f1fb", 3, null, -1, -1, new List<PresetItemTemplateIdGroup>(), null, null));
		_dataArray.Add(new TravelingEventItem(116, 215, ETravelingEventDisplayType.TravelingEvent_1, ETravelingEventType.SpiritualDebt, 216, new string[5] { "Character", "Settlement", "", "", "" }, 3, ETravelingEventTriggerType.OnArea, ETravelingEventTriggerAreaType.NormalArea, -1, isUnique: false, 0, 15, 0, new sbyte[2] { -100, 100 }, 0, "c859d7db-01e4-4660-866e-5a72705cad80", 3, null, -1, -1, new List<PresetItemTemplateIdGroup>(), null, null));
		_dataArray.Add(new TravelingEventItem(117, 217, ETravelingEventDisplayType.TravelingEvent_1, ETravelingEventType.SpiritualDebt, 218, new string[5] { "Character", "Settlement", "", "", "" }, 4, ETravelingEventTriggerType.OnArea, ETravelingEventTriggerAreaType.NormalArea, -1, isUnique: false, 0, 15, 0, new sbyte[2] { -100, 100 }, 3, "84983a51-9f61-45b5-a734-fb0c7d17c8e4", 3, null, -1, -1, new List<PresetItemTemplateIdGroup>(), null, null));
		_dataArray.Add(new TravelingEventItem(118, 219, ETravelingEventDisplayType.TravelingEvent_1, ETravelingEventType.SpiritualDebt, 220, new string[5] { "Character", "Settlement", "", "", "" }, 5, ETravelingEventTriggerType.OnArea, ETravelingEventTriggerAreaType.NormalArea, -1, isUnique: false, 0, 15, 0, new sbyte[2] { -100, 100 }, 0, "7747c4da-902a-4da8-ba59-29221db4e874", 3, null, -1, -1, new List<PresetItemTemplateIdGroup>(), null, null));
		_dataArray.Add(new TravelingEventItem(119, 221, ETravelingEventDisplayType.TravelingEvent_1, ETravelingEventType.SpiritualDebt, 222, new string[5] { "Character", "Settlement", "", "", "" }, 6, ETravelingEventTriggerType.OnArea, ETravelingEventTriggerAreaType.NormalArea, -1, isUnique: false, 0, 15, 0, new sbyte[2] { -100, 100 }, 3, "50cd1a13-a3a5-4e62-bd16-3f7bc6f5b767", 3, null, -1, -1, new List<PresetItemTemplateIdGroup>(), null, null));
	}

	private void CreateItems2()
	{
		_dataArray.Add(new TravelingEventItem(120, 223, ETravelingEventDisplayType.TravelingEvent_1, ETravelingEventType.SpiritualDebt, 224, new string[5] { "Character", "Settlement", "", "", "" }, 7, ETravelingEventTriggerType.OnArea, ETravelingEventTriggerAreaType.NormalArea, -1, isUnique: false, 0, 15, 0, new sbyte[2] { -100, 100 }, 1, "358f6d1c-6994-41fd-bd44-f555219465ea", 3, null, -1, -1, new List<PresetItemTemplateIdGroup>(), null, null));
		_dataArray.Add(new TravelingEventItem(121, 225, ETravelingEventDisplayType.TravelingEvent_1, ETravelingEventType.SpiritualDebt, 226, new string[5] { "Character", "Settlement", "", "", "" }, 8, ETravelingEventTriggerType.OnArea, ETravelingEventTriggerAreaType.NormalArea, -1, isUnique: false, 0, 15, 0, new sbyte[2] { -100, 100 }, 4, "ddca8e22-e230-4c43-8415-959ce9b2899d", 3, null, -1, -1, new List<PresetItemTemplateIdGroup>(), null, null));
		_dataArray.Add(new TravelingEventItem(122, 227, ETravelingEventDisplayType.TravelingEvent_1, ETravelingEventType.SpiritualDebt, 228, new string[5] { "Character", "Settlement", "", "", "" }, 9, ETravelingEventTriggerType.OnArea, ETravelingEventTriggerAreaType.NormalArea, -1, isUnique: false, 0, 15, 0, new sbyte[2] { -100, 100 }, 2, "cad6674d-f904-4100-8634-bf1b3807e293", 3, null, -1, -1, new List<PresetItemTemplateIdGroup>(), null, null));
		_dataArray.Add(new TravelingEventItem(123, 229, ETravelingEventDisplayType.TravelingEvent_1, ETravelingEventType.SpiritualDebt, 230, new string[5] { "Character", "Settlement", "", "", "" }, 10, ETravelingEventTriggerType.OnArea, ETravelingEventTriggerAreaType.NormalArea, -1, isUnique: false, 0, 15, 0, new sbyte[2] { -100, 100 }, 1, "37edc08b-99ff-4b43-8d43-fcff13291091", 3, null, -1, -1, new List<PresetItemTemplateIdGroup>(), null, null));
		_dataArray.Add(new TravelingEventItem(124, 231, ETravelingEventDisplayType.TravelingEvent_1, ETravelingEventType.SpiritualDebt, 232, new string[5] { "Character", "Settlement", "", "", "" }, 11, ETravelingEventTriggerType.OnArea, ETravelingEventTriggerAreaType.NormalArea, -1, isUnique: false, 0, 15, 0, new sbyte[2] { -100, 100 }, 4, "4c5b9706-3834-4de3-8273-01dcbba28b94", 3, null, -1, -1, new List<PresetItemTemplateIdGroup>(), null, null));
		_dataArray.Add(new TravelingEventItem(125, 233, ETravelingEventDisplayType.TravelingEvent_1, ETravelingEventType.SpiritualDebt, 234, new string[5] { "Character", "Settlement", "", "", "" }, 12, ETravelingEventTriggerType.OnArea, ETravelingEventTriggerAreaType.NormalArea, -1, isUnique: false, 0, 15, 0, new sbyte[2] { -100, 100 }, 3, "858060fb-60ed-404e-bdff-e05179409285", 3, null, -1, -1, new List<PresetItemTemplateIdGroup>(), null, null));
		_dataArray.Add(new TravelingEventItem(126, 235, ETravelingEventDisplayType.TravelingEvent_1, ETravelingEventType.SpiritualDebt, 236, new string[5] { "Character", "Settlement", "", "", "" }, 13, ETravelingEventTriggerType.OnArea, ETravelingEventTriggerAreaType.NormalArea, -1, isUnique: false, 0, 15, 0, new sbyte[2] { -100, 100 }, 0, "528ef3f9-3ffb-4652-9c72-e60c2504ae29", 3, null, -1, -1, new List<PresetItemTemplateIdGroup>(), null, null));
		_dataArray.Add(new TravelingEventItem(127, 237, ETravelingEventDisplayType.TravelingEvent_1, ETravelingEventType.SpiritualDebt, 238, new string[5] { "Character", "Settlement", "", "", "" }, 14, ETravelingEventTriggerType.OnArea, ETravelingEventTriggerAreaType.NormalArea, -1, isUnique: false, 0, 15, 0, new sbyte[2] { -100, 100 }, 2, "db69a775-c98d-4398-9f60-0636ab4d026f", 3, null, -1, -1, new List<PresetItemTemplateIdGroup>(), null, null));
		_dataArray.Add(new TravelingEventItem(128, 239, ETravelingEventDisplayType.TravelingEvent_1, ETravelingEventType.SpiritualDebt, 240, new string[5] { "Character", "Settlement", "", "", "" }, 15, ETravelingEventTriggerType.OnArea, ETravelingEventTriggerAreaType.NormalArea, -1, isUnique: false, 0, 15, 0, new sbyte[2] { -100, 100 }, 1, "69ddfc28-4f3c-49a3-91c0-6519e9763f40", 3, null, -1, -1, new List<PresetItemTemplateIdGroup>(), null, null));
		_dataArray.Add(new TravelingEventItem(129, 241, ETravelingEventDisplayType.TravelingEvent_3, ETravelingEventType.SectVisit, 242, new string[5] { "Character", "Location", "", "", "" }, 1, ETravelingEventTriggerType.OnArea, ETravelingEventTriggerAreaType.SectArea, 1, isUnique: true, 100, 100, 0, new sbyte[2] { -100, 100 }, -1, "7466fa32-741c-4413-a7a6-08c704c687ef", 0, null, -1, -1, new List<PresetItemTemplateIdGroup>(), null, null));
		_dataArray.Add(new TravelingEventItem(130, 243, ETravelingEventDisplayType.TravelingEvent_3, ETravelingEventType.SectVisit, 244, new string[5] { "Character", "Location", "", "", "" }, 2, ETravelingEventTriggerType.OnArea, ETravelingEventTriggerAreaType.SectArea, 2, isUnique: true, 100, 100, 0, new sbyte[2] { -100, 100 }, -1, "c0364283-24e7-4e10-b584-48971121299c", 0, null, -1, -1, new List<PresetItemTemplateIdGroup>(), null, null));
		_dataArray.Add(new TravelingEventItem(131, 245, ETravelingEventDisplayType.TravelingEvent_3, ETravelingEventType.SectVisit, 246, new string[5] { "Character", "Location", "", "", "" }, 3, ETravelingEventTriggerType.OnArea, ETravelingEventTriggerAreaType.SectArea, 3, isUnique: true, 100, 100, 0, new sbyte[2] { -100, 100 }, -1, "afccbfd5-d5e4-4dcf-82bf-7c9c256e544e", 0, null, -1, -1, new List<PresetItemTemplateIdGroup>(), null, null));
		_dataArray.Add(new TravelingEventItem(132, 247, ETravelingEventDisplayType.TravelingEvent_3, ETravelingEventType.SectVisit, 248, new string[5] { "Character", "Location", "", "", "" }, 4, ETravelingEventTriggerType.OnArea, ETravelingEventTriggerAreaType.SectArea, 4, isUnique: true, 100, 100, 0, new sbyte[2] { -100, 100 }, -1, "8adacd25-060e-4a0e-b9da-a814ecc1d9c0", 0, null, -1, -1, new List<PresetItemTemplateIdGroup>(), null, null));
		_dataArray.Add(new TravelingEventItem(133, 249, ETravelingEventDisplayType.TravelingEvent_3, ETravelingEventType.SectVisit, 250, new string[5] { "Character", "Location", "", "", "" }, 5, ETravelingEventTriggerType.OnArea, ETravelingEventTriggerAreaType.SectArea, 5, isUnique: true, 100, 100, 0, new sbyte[2] { -100, 100 }, -1, "28475f2c-e368-40c1-88e8-57a18fb0e38e", 0, null, -1, -1, new List<PresetItemTemplateIdGroup>(), null, null));
		_dataArray.Add(new TravelingEventItem(134, 251, ETravelingEventDisplayType.TravelingEvent_3, ETravelingEventType.SectVisit, 252, new string[5] { "Character", "Location", "", "", "" }, 6, ETravelingEventTriggerType.OnArea, ETravelingEventTriggerAreaType.SectArea, 6, isUnique: true, 100, 100, 0, new sbyte[2] { -100, 100 }, -1, "ff0f1368-79c9-49df-b5b4-06e47ecde548", 0, null, -1, -1, new List<PresetItemTemplateIdGroup>(), null, null));
		_dataArray.Add(new TravelingEventItem(135, 253, ETravelingEventDisplayType.TravelingEvent_3, ETravelingEventType.SectVisit, 254, new string[5] { "Character", "Location", "", "", "" }, 7, ETravelingEventTriggerType.OnArea, ETravelingEventTriggerAreaType.SectArea, 7, isUnique: true, 100, 100, 0, new sbyte[2] { -100, 100 }, -1, "618b4db6-ff20-4dac-8c97-018964ed0b0a", 0, null, -1, -1, new List<PresetItemTemplateIdGroup>(), null, null));
		_dataArray.Add(new TravelingEventItem(136, 255, ETravelingEventDisplayType.TravelingEvent_3, ETravelingEventType.SectVisit, 256, new string[5] { "Character", "Location", "", "", "" }, 8, ETravelingEventTriggerType.OnArea, ETravelingEventTriggerAreaType.SectArea, 8, isUnique: true, 100, 100, 0, new sbyte[2] { -100, 100 }, -1, "20ced638-563f-4453-9289-81d811f37851", 0, null, -1, -1, new List<PresetItemTemplateIdGroup>(), null, null));
		_dataArray.Add(new TravelingEventItem(137, 257, ETravelingEventDisplayType.TravelingEvent_3, ETravelingEventType.SectVisit, 258, new string[5] { "Character", "Location", "", "", "" }, 9, ETravelingEventTriggerType.OnArea, ETravelingEventTriggerAreaType.SectArea, 9, isUnique: true, 100, 100, 0, new sbyte[2] { -100, 100 }, -1, "b0e48b48-7e41-4d40-a949-079a35d0b8af", 0, null, -1, -1, new List<PresetItemTemplateIdGroup>(), null, null));
		_dataArray.Add(new TravelingEventItem(138, 259, ETravelingEventDisplayType.TravelingEvent_3, ETravelingEventType.SectVisit, 260, new string[5] { "Character", "Location", "", "", "" }, 10, ETravelingEventTriggerType.OnArea, ETravelingEventTriggerAreaType.SectArea, 10, isUnique: true, 100, 100, 0, new sbyte[2] { -100, 100 }, -1, "f266965b-ff83-4db1-93a0-6e00da4702ab", 0, null, -1, -1, new List<PresetItemTemplateIdGroup>(), null, null));
		_dataArray.Add(new TravelingEventItem(139, 261, ETravelingEventDisplayType.TravelingEvent_3, ETravelingEventType.SectVisit, 262, new string[5] { "Character", "Location", "", "", "" }, 11, ETravelingEventTriggerType.OnArea, ETravelingEventTriggerAreaType.SectArea, 11, isUnique: true, 100, 100, 0, new sbyte[2] { -100, 100 }, -1, "14acf515-76c3-4083-a06c-42e9db67d78f", 0, null, -1, -1, new List<PresetItemTemplateIdGroup>(), null, null));
		_dataArray.Add(new TravelingEventItem(140, 263, ETravelingEventDisplayType.TravelingEvent_3, ETravelingEventType.SectVisit, 264, new string[5] { "Character", "Location", "", "", "" }, 12, ETravelingEventTriggerType.OnArea, ETravelingEventTriggerAreaType.SectArea, 12, isUnique: true, 100, 100, 0, new sbyte[2] { -100, 100 }, -1, "5bb5deda-00d3-4976-b737-f31a718886f2", 0, null, -1, -1, new List<PresetItemTemplateIdGroup>(), null, null));
		_dataArray.Add(new TravelingEventItem(141, 265, ETravelingEventDisplayType.TravelingEvent_3, ETravelingEventType.SectVisit, 266, new string[5] { "Character", "Location", "", "", "" }, 13, ETravelingEventTriggerType.OnArea, ETravelingEventTriggerAreaType.SectArea, 13, isUnique: true, 100, 100, 0, new sbyte[2] { -100, 100 }, -1, "c1fe70a3-c37f-4c19-9643-2a11414cef7c", 0, null, -1, -1, new List<PresetItemTemplateIdGroup>(), null, null));
		_dataArray.Add(new TravelingEventItem(142, 267, ETravelingEventDisplayType.TravelingEvent_3, ETravelingEventType.SectVisit, 268, new string[5] { "Character", "Location", "", "", "" }, 14, ETravelingEventTriggerType.OnArea, ETravelingEventTriggerAreaType.SectArea, 14, isUnique: true, 100, 100, 0, new sbyte[2] { -100, 100 }, -1, "0ac07093-5dd8-479a-85b0-6c80689974f8", 0, null, -1, -1, new List<PresetItemTemplateIdGroup>(), null, null));
		_dataArray.Add(new TravelingEventItem(143, 269, ETravelingEventDisplayType.TravelingEvent_3, ETravelingEventType.SectVisit, 270, new string[5] { "Character", "Location", "", "", "" }, 15, ETravelingEventTriggerType.OnArea, ETravelingEventTriggerAreaType.SectArea, 15, isUnique: true, 100, 100, 0, new sbyte[2] { -100, 100 }, -1, "4772f315-9060-4cf6-9204-3cd41553e4c0", 0, null, -1, -1, new List<PresetItemTemplateIdGroup>(), null, null));
		_dataArray.Add(new TravelingEventItem(144, 271, ETravelingEventDisplayType.TravelingEvent_2, ETravelingEventType.Combat, 272, new string[5] { "Character", "CharacterTemplate", "", "", "" }, -1, ETravelingEventTriggerType.ToArea, ETravelingEventTriggerAreaType.NormalArea, 17, isUnique: false, -1, 25, 0, new sbyte[2] { -100, 100 }, -1, "15892dee-18dc-4ee7-81d5-137068701e13", 3, null, -1, -1, new List<PresetItemTemplateIdGroup>(), null, null));
		_dataArray.Add(new TravelingEventItem(145, 273, ETravelingEventDisplayType.TravelingEvent_2, ETravelingEventType.Combat, 272, new string[5] { "Character", "CharacterTemplate", "", "", "" }, -1, ETravelingEventTriggerType.ToArea, ETravelingEventTriggerAreaType.NormalArea, 18, isUnique: false, -1, 25, -1, new sbyte[2] { -100, -50 }, -1, "15892dee-18dc-4ee7-81d5-137068701e13", 3, null, -1, -1, new List<PresetItemTemplateIdGroup>(), null, null));
		_dataArray.Add(new TravelingEventItem(146, 274, ETravelingEventDisplayType.TravelingEvent_2, ETravelingEventType.Combat, 272, new string[5] { "Character", "CharacterTemplate", "", "", "" }, -1, ETravelingEventTriggerType.ToArea, ETravelingEventTriggerAreaType.BrokenArea, 19, isUnique: false, -1, 5, 0, new sbyte[2] { -100, 100 }, -1, "15892dee-18dc-4ee7-81d5-137068701e13", 3, null, -1, -1, new List<PresetItemTemplateIdGroup>(), null, null));
		_dataArray.Add(new TravelingEventItem(147, 275, ETravelingEventDisplayType.TravelingEvent_2, ETravelingEventType.SectCombat, 276, new string[5] { "Character", "CharacterTemplate", "", "", "" }, 1, ETravelingEventTriggerType.ToArea, ETravelingEventTriggerAreaType.SectArea, 1, isUnique: false, -1, 20, -1, new sbyte[2] { -100, 100 }, -1, "2b76fef8-3491-47d9-9360-9d72a6b3354b", 3, null, -1, -1, new List<PresetItemTemplateIdGroup>(), null, null));
		_dataArray.Add(new TravelingEventItem(148, 277, ETravelingEventDisplayType.TravelingEvent_2, ETravelingEventType.SectCombat, 276, new string[5] { "Character", "CharacterTemplate", "", "", "" }, 2, ETravelingEventTriggerType.ToArea, ETravelingEventTriggerAreaType.SectArea, 2, isUnique: false, -1, 20, -1, new sbyte[2] { -100, 100 }, -1, "f5687014-4ef5-4233-af25-05e55b1012ef", 3, null, -1, -1, new List<PresetItemTemplateIdGroup>(), null, null));
		_dataArray.Add(new TravelingEventItem(149, 278, ETravelingEventDisplayType.TravelingEvent_2, ETravelingEventType.SectCombat, 276, new string[5] { "Character", "CharacterTemplate", "", "", "" }, 3, ETravelingEventTriggerType.ToArea, ETravelingEventTriggerAreaType.SectArea, 3, isUnique: false, -1, 20, -1, new sbyte[2] { -100, 100 }, -1, "e504bf16-5f6f-4960-ae85-abea51e87e52", 3, null, -1, -1, new List<PresetItemTemplateIdGroup>(), null, null));
		_dataArray.Add(new TravelingEventItem(150, 279, ETravelingEventDisplayType.TravelingEvent_2, ETravelingEventType.SectCombat, 276, new string[5] { "Character", "CharacterTemplate", "", "", "" }, 4, ETravelingEventTriggerType.ToArea, ETravelingEventTriggerAreaType.SectArea, 4, isUnique: false, -1, 20, -1, new sbyte[2] { -100, 100 }, -1, "84b18c01-154b-4235-b288-0d3e2697a741", 3, null, -1, -1, new List<PresetItemTemplateIdGroup>(), null, null));
		_dataArray.Add(new TravelingEventItem(151, 280, ETravelingEventDisplayType.TravelingEvent_2, ETravelingEventType.SectCombat, 276, new string[5] { "Character", "CharacterTemplate", "", "", "" }, 5, ETravelingEventTriggerType.ToArea, ETravelingEventTriggerAreaType.SectArea, 5, isUnique: false, -1, 20, -1, new sbyte[2] { -100, 100 }, -1, "5a2f3450-4d04-4a01-b669-617628338e16", 3, null, -1, -1, new List<PresetItemTemplateIdGroup>(), null, null));
		_dataArray.Add(new TravelingEventItem(152, 281, ETravelingEventDisplayType.TravelingEvent_2, ETravelingEventType.SectCombat, 282, new string[5] { "Character", "CharacterTemplate", "", "", "" }, 11, ETravelingEventTriggerType.ToArea, ETravelingEventTriggerAreaType.SectArea, 11, isUnique: false, 1, 20, 1, new sbyte[2] { -100, 100 }, -1, "333fa625-5773-4b50-9056-562a6f2002d2", 3, null, -1, -1, new List<PresetItemTemplateIdGroup>(), null, null));
		_dataArray.Add(new TravelingEventItem(153, 283, ETravelingEventDisplayType.TravelingEvent_2, ETravelingEventType.SectCombat, 282, new string[5] { "Character", "CharacterTemplate", "", "", "" }, 12, ETravelingEventTriggerType.ToArea, ETravelingEventTriggerAreaType.SectArea, 12, isUnique: false, 1, 20, 1, new sbyte[2] { -100, 100 }, -1, "72e08263-ae2c-49ca-8d4a-7dd302f02756", 3, null, -1, -1, new List<PresetItemTemplateIdGroup>(), null, null));
		_dataArray.Add(new TravelingEventItem(154, 284, ETravelingEventDisplayType.TravelingEvent_2, ETravelingEventType.SectCombat, 282, new string[5] { "Character", "CharacterTemplate", "", "", "" }, 13, ETravelingEventTriggerType.ToArea, ETravelingEventTriggerAreaType.SectArea, 13, isUnique: false, 1, 20, 1, new sbyte[2] { -100, 100 }, -1, "e92b0a4b-1478-4b24-b107-588c220d6a6b", 3, null, -1, -1, new List<PresetItemTemplateIdGroup>(), null, null));
		_dataArray.Add(new TravelingEventItem(155, 285, ETravelingEventDisplayType.TravelingEvent_2, ETravelingEventType.SectCombat, 282, new string[5] { "Character", "CharacterTemplate", "", "", "" }, 14, ETravelingEventTriggerType.ToArea, ETravelingEventTriggerAreaType.SectArea, 14, isUnique: false, 1, 20, 1, new sbyte[2] { -100, 100 }, -1, "2cab1ae2-59b2-4486-adb5-256ed3364651", 3, null, -1, -1, new List<PresetItemTemplateIdGroup>(), null, null));
		_dataArray.Add(new TravelingEventItem(156, 286, ETravelingEventDisplayType.TravelingEvent_2, ETravelingEventType.SectCombat, 282, new string[5] { "Character", "CharacterTemplate", "", "", "" }, 15, ETravelingEventTriggerType.ToArea, ETravelingEventTriggerAreaType.SectArea, 15, isUnique: false, 1, 20, 1, new sbyte[2] { -100, 100 }, -1, "f54cdc32-f58f-40ab-93b9-aa926c71ad46", 3, null, -1, -1, new List<PresetItemTemplateIdGroup>(), null, null));
		_dataArray.Add(new TravelingEventItem(157, 287, ETravelingEventDisplayType.TravelingEvent_1, ETravelingEventType.CharacterRecommendVillager, 288, new string[5] { "Character", "Location", "Character", "Character", "" }, -1, ETravelingEventTriggerType.OnArea, ETravelingEventTriggerAreaType.NormalArea, -1, isUnique: false, 0, 15, 0, new sbyte[2] { -100, 100 }, -1, "4c2d630e-bdfd-4f40-9712-d80cf02c746f", 3, null, -1, -1, new List<PresetItemTemplateIdGroup>(), null, null));
		_dataArray.Add(new TravelingEventItem(158, 289, ETravelingEventDisplayType.TravelingEvent_1, ETravelingEventType.CharacterRecommendVillager, 288, new string[5] { "Character", "Location", "Character", "Character", "" }, -1, ETravelingEventTriggerType.OnArea, ETravelingEventTriggerAreaType.NormalArea, -1, isUnique: false, 0, 10, 1, new sbyte[2] { -100, 100 }, -1, "4c2d630e-bdfd-4f40-9712-d80cf02c746f", 3, null, -1, -1, new List<PresetItemTemplateIdGroup>(), null, null));
		_dataArray.Add(new TravelingEventItem(159, 290, ETravelingEventDisplayType.TravelingEvent_3, ETravelingEventType.AttributeCost, 291, new string[5] { "Character", "Integer", "", "", "" }, -1, ETravelingEventTriggerType.ToArea, ETravelingEventTriggerAreaType.Any, -1, isUnique: false, 0, 1, 0, new sbyte[2] { -100, 100 }, -1, "1162ab17-3ad2-445f-987f-8b3deeb39811", 0, null, 0, -1, new List<PresetItemTemplateIdGroup>(), null, null));
		_dataArray.Add(new TravelingEventItem(160, 292, ETravelingEventDisplayType.TravelingEvent_3, ETravelingEventType.AttributeCost, 293, new string[5] { "Character", "Integer", "", "", "" }, -1, ETravelingEventTriggerType.ToArea, ETravelingEventTriggerAreaType.Any, -1, isUnique: false, 0, 1, 0, new sbyte[2] { -100, 100 }, -1, "2fa259d2-b2e2-4cda-b33c-ff6f2b91bf69", 0, null, 1, -1, new List<PresetItemTemplateIdGroup>(), null, null));
		_dataArray.Add(new TravelingEventItem(161, 294, ETravelingEventDisplayType.TravelingEvent_3, ETravelingEventType.AttributeCost, 295, new string[5] { "Character", "Integer", "", "", "" }, -1, ETravelingEventTriggerType.ToArea, ETravelingEventTriggerAreaType.Any, -1, isUnique: false, 0, 1, 0, new sbyte[2] { -100, 100 }, -1, "34675f5a-daad-4e67-88e9-4ed3aa9a5955", 0, null, 2, -1, new List<PresetItemTemplateIdGroup>(), null, null));
		_dataArray.Add(new TravelingEventItem(162, 296, ETravelingEventDisplayType.TravelingEvent_3, ETravelingEventType.AttributeCost, 297, new string[5] { "Character", "Integer", "", "", "" }, -1, ETravelingEventTriggerType.ToArea, ETravelingEventTriggerAreaType.Any, -1, isUnique: false, 0, 1, 0, new sbyte[2] { -100, 100 }, -1, "323d9b3b-e136-4227-a7e3-29ebc11cddec", 0, null, 3, -1, new List<PresetItemTemplateIdGroup>(), null, null));
		_dataArray.Add(new TravelingEventItem(163, 298, ETravelingEventDisplayType.TravelingEvent_3, ETravelingEventType.AttributeCost, 299, new string[5] { "Character", "Integer", "", "", "" }, -1, ETravelingEventTriggerType.ToArea, ETravelingEventTriggerAreaType.Any, -1, isUnique: false, 0, 1, 0, new sbyte[2] { -100, 100 }, -1, "f232f116-4e38-4652-8c6a-ef3c91b0c078", 0, null, 4, -1, new List<PresetItemTemplateIdGroup>(), null, null));
		_dataArray.Add(new TravelingEventItem(164, 300, ETravelingEventDisplayType.TravelingEvent_3, ETravelingEventType.AttributeCost, 301, new string[5] { "Character", "Integer", "", "", "" }, -1, ETravelingEventTriggerType.ToArea, ETravelingEventTriggerAreaType.Any, -1, isUnique: false, 0, 1, 0, new sbyte[2] { -100, 100 }, -1, "ddadb9df-7b41-4076-90f0-95d9afc3a10b", 0, null, 5, -1, new List<PresetItemTemplateIdGroup>(), null, null));
		_dataArray.Add(new TravelingEventItem(165, 302, ETravelingEventDisplayType.TravelingEvent_3, ETravelingEventType.CarrierDurability, 303, new string[5] { "Character", "", "", "", "" }, -1, ETravelingEventTriggerType.Any, ETravelingEventTriggerAreaType.Any, -1, isUnique: false, 0, 3, 0, new sbyte[2] { -100, 100 }, -1, "35a36035-be2a-42cd-8a8e-dc77ee240b09", 0, null, -1, -1, new List<PresetItemTemplateIdGroup>(), null, null));
	}

	public override void Init()
	{
		base.Init();
		_dataArray = new List<TravelingEventItem>(166);
		CreateItems0();
		CreateItems1();
		CreateItems2();
	}
}
