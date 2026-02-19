using System;
using System.Collections.Generic;
using Config.Common;

namespace Config;

[Serializable]
public class SkillBook : ConfigData<SkillBookItem, short>
{
	public static class DefKey
	{
		public const short YueLun = 1;

		public const short NichangQupu = 6;

		public const short QiJingShiSanPian = 10;

		public const short LankePu = 15;

		public const short LiWengDuiYun = 19;

		public const short YongziBafa = 24;

		public const short YiZhouMingHuaLu = 28;

		public const short BunianTu = 33;

		public const short YuanHaiZiPing = 37;

		public const short Zouzi = 42;

		public const short JiuPu = 46;

		public const short DamingShuiji = 51;

		public const short HuoBaoJue = 55;

		public const short KaogongJi = 60;

		public const short DongYuanZaLu = 64;

		public const short DajiangZhuan = 69;

		public const short ChuShiYiShu = 73;

		public const short JinkuiYaolue = 78;

		public const short DuJing = 82;

		public const short BenjingXiapinZhu = 87;

		public const short LiRenGuYu = 91;

		public const short Yili = 96;

		public const short LiuRuiTu = 100;

		public const short BaotaJing = 105;

		public const short WenShiZhenJing = 109;

		public const short NanhuaZhenjing = 114;

		public const short JinGangJing = 118;

		public const short HuayanJing = 123;

		public const short SongSaoShiDan = 127;

		public const short ShiJing = 132;

		public const short QingYiLu = 136;

		public const short Qilue = 141;

		public const short JingChanGong = 144;

		public const short FomenShizihou = 351;

		public const short YangxiangongJinshenfa = 360;

		public const short ShengsiBamen = 369;

		public const short Wujijin = 378;

		public const short TaichuGuicangJue = 386;

		public const short XiangniBizhuanGong = 399;

		public const short NvwaButianShi = 407;

		public const short TianzhuXuantieCe = 414;

		public const short BaishuiChangshengFa = 421;

		public const short Dashouyin = 430;

		public const short TiancanShigu = 438;

		public const short QiannianZui = 454;

		public const short JingangBoreZhang = 471;

		public const short JingangSanmeiZhang = 479;

		public const short WudangChunyangQuan = 486;

		public const short Yunkuangshi = 495;

		public const short HanbingShenzhang = 504;

		public const short XumishanSishenzhang = 517;

		public const short QinghuangchiheiShenzhang = 525;

		public const short LihuoLiuyangZhang = 533;

		public const short TianmoCanhunZhang = 542;

		public const short DaguangmingshanYiyuanZhi = 556;

		public const short WanhuaLingyueZhi = 564;

		public const short TiandiYuanyiZhi = 573;

		public const short XuanjiZhixueFa = 580;

		public const short BiyuBingcanGu = 594;

		public const short TaisuJueshou = 603;

		public const short XuechiGuizhua = 612;

		public const short YunlongJiuxianTui = 619;

		public const short FeishanDuanhaiDabashi = 628;

		public const short YanwangGuijiao = 636;

		public const short MinglongZhi = 653;

		public const short YunvShenjian = 675;

		public const short TaijiJianfa = 682;

		public const short WushangPutiJian = 691;

		public const short YinyangNijian = 699;

		public const short ChunjunJianqi = 708;

		public const short QingzhuJianfa = 716;

		public const short WuxiaQijuejian = 723;

		public const short TaingangDaofa = 731;

		public const short JinniZhenmoDao = 739;

		public const short CandaoShi = 748;

		public const short RuyiBaolunDao = 756;

		public const short ChiziZhanlongDao = 765;

		public const short WeituoXiangmoZhang = 773;

		public const short KuanglongShiziQiang = 782;

		public const short JiahaiShenzhang = 790;

		public const short TianshuXuanjiByAdoptiveFather = 870;

		public const short ShuiHuoYingQiGong = 874;
	}

	public static class DefValue
	{
		public static SkillBookItem YueLun => Instance[(short)1];

		public static SkillBookItem NichangQupu => Instance[(short)6];

		public static SkillBookItem QiJingShiSanPian => Instance[(short)10];

		public static SkillBookItem LankePu => Instance[(short)15];

		public static SkillBookItem LiWengDuiYun => Instance[(short)19];

		public static SkillBookItem YongziBafa => Instance[(short)24];

		public static SkillBookItem YiZhouMingHuaLu => Instance[(short)28];

		public static SkillBookItem BunianTu => Instance[(short)33];

		public static SkillBookItem YuanHaiZiPing => Instance[(short)37];

		public static SkillBookItem Zouzi => Instance[(short)42];

		public static SkillBookItem JiuPu => Instance[(short)46];

		public static SkillBookItem DamingShuiji => Instance[(short)51];

		public static SkillBookItem HuoBaoJue => Instance[(short)55];

		public static SkillBookItem KaogongJi => Instance[(short)60];

		public static SkillBookItem DongYuanZaLu => Instance[(short)64];

		public static SkillBookItem DajiangZhuan => Instance[(short)69];

		public static SkillBookItem ChuShiYiShu => Instance[(short)73];

		public static SkillBookItem JinkuiYaolue => Instance[(short)78];

		public static SkillBookItem DuJing => Instance[(short)82];

		public static SkillBookItem BenjingXiapinZhu => Instance[(short)87];

		public static SkillBookItem LiRenGuYu => Instance[(short)91];

		public static SkillBookItem Yili => Instance[(short)96];

		public static SkillBookItem LiuRuiTu => Instance[(short)100];

		public static SkillBookItem BaotaJing => Instance[(short)105];

		public static SkillBookItem WenShiZhenJing => Instance[(short)109];

		public static SkillBookItem NanhuaZhenjing => Instance[(short)114];

		public static SkillBookItem JinGangJing => Instance[(short)118];

		public static SkillBookItem HuayanJing => Instance[(short)123];

		public static SkillBookItem SongSaoShiDan => Instance[(short)127];

		public static SkillBookItem ShiJing => Instance[(short)132];

		public static SkillBookItem QingYiLu => Instance[(short)136];

		public static SkillBookItem Qilue => Instance[(short)141];

		public static SkillBookItem JingChanGong => Instance[(short)144];

		public static SkillBookItem FomenShizihou => Instance[(short)351];

		public static SkillBookItem YangxiangongJinshenfa => Instance[(short)360];

		public static SkillBookItem ShengsiBamen => Instance[(short)369];

		public static SkillBookItem Wujijin => Instance[(short)378];

		public static SkillBookItem TaichuGuicangJue => Instance[(short)386];

		public static SkillBookItem XiangniBizhuanGong => Instance[(short)399];

		public static SkillBookItem NvwaButianShi => Instance[(short)407];

		public static SkillBookItem TianzhuXuantieCe => Instance[(short)414];

		public static SkillBookItem BaishuiChangshengFa => Instance[(short)421];

		public static SkillBookItem Dashouyin => Instance[(short)430];

		public static SkillBookItem TiancanShigu => Instance[(short)438];

		public static SkillBookItem QiannianZui => Instance[(short)454];

		public static SkillBookItem JingangBoreZhang => Instance[(short)471];

		public static SkillBookItem JingangSanmeiZhang => Instance[(short)479];

		public static SkillBookItem WudangChunyangQuan => Instance[(short)486];

		public static SkillBookItem Yunkuangshi => Instance[(short)495];

		public static SkillBookItem HanbingShenzhang => Instance[(short)504];

		public static SkillBookItem XumishanSishenzhang => Instance[(short)517];

		public static SkillBookItem QinghuangchiheiShenzhang => Instance[(short)525];

		public static SkillBookItem LihuoLiuyangZhang => Instance[(short)533];

		public static SkillBookItem TianmoCanhunZhang => Instance[(short)542];

		public static SkillBookItem DaguangmingshanYiyuanZhi => Instance[(short)556];

		public static SkillBookItem WanhuaLingyueZhi => Instance[(short)564];

		public static SkillBookItem TiandiYuanyiZhi => Instance[(short)573];

		public static SkillBookItem XuanjiZhixueFa => Instance[(short)580];

		public static SkillBookItem BiyuBingcanGu => Instance[(short)594];

		public static SkillBookItem TaisuJueshou => Instance[(short)603];

		public static SkillBookItem XuechiGuizhua => Instance[(short)612];

		public static SkillBookItem YunlongJiuxianTui => Instance[(short)619];

		public static SkillBookItem FeishanDuanhaiDabashi => Instance[(short)628];

		public static SkillBookItem YanwangGuijiao => Instance[(short)636];

		public static SkillBookItem MinglongZhi => Instance[(short)653];

		public static SkillBookItem YunvShenjian => Instance[(short)675];

		public static SkillBookItem TaijiJianfa => Instance[(short)682];

		public static SkillBookItem WushangPutiJian => Instance[(short)691];

		public static SkillBookItem YinyangNijian => Instance[(short)699];

		public static SkillBookItem ChunjunJianqi => Instance[(short)708];

		public static SkillBookItem QingzhuJianfa => Instance[(short)716];

		public static SkillBookItem WuxiaQijuejian => Instance[(short)723];

		public static SkillBookItem TaingangDaofa => Instance[(short)731];

		public static SkillBookItem JinniZhenmoDao => Instance[(short)739];

		public static SkillBookItem CandaoShi => Instance[(short)748];

		public static SkillBookItem RuyiBaolunDao => Instance[(short)756];

		public static SkillBookItem ChiziZhanlongDao => Instance[(short)765];

		public static SkillBookItem WeituoXiangmoZhang => Instance[(short)773];

		public static SkillBookItem KuanglongShiziQiang => Instance[(short)782];

		public static SkillBookItem JiahaiShenzhang => Instance[(short)790];

		public static SkillBookItem TianshuXuanjiByAdoptiveFather => Instance[(short)870];

		public static SkillBookItem ShuiHuoYingQiGong => Instance[(short)874];
	}

	public static SkillBook Instance = new SkillBook();

	private readonly HashSet<string> RequiredFields = new HashSet<string>
	{
		"ItemSubType", "GroupId", "ResourceType", "BreakBonusEffect", "LifeSkillType", "LifeSkillTemplateId", "CombatSkillType", "CombatSkillTemplateId", "ReferenceBooksWithBonus", "TemplateId",
		"Name", "Grade", "Icon", "Desc", "MaxDurability", "BaseWeight", "BaseHappinessChange", "DropRate", "LegacyPoint"
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
		_dataArray.Add(new SkillBookItem(0, 0, 10, 1000, 0, 0, "icon_SkillBook_caishiwulong", 1, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 50, 10, 150, 300, 0, 1, 600, 3, allowRandomCreate: true, 45, isSpecial: false, 4, 36, 0, 0, 0, -1, -1, 60, new List<short>()));
		_dataArray.Add(new SkillBookItem(1, 2, 10, 1000, 1, 0, "icon_SkillBook_caishiwulong", 3, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 45, 10, 300, 600, 0, 2, 1200, 4, allowRandomCreate: true, 40, isSpecial: false, 4, 36, 0, 0, 1, -1, -1, 130, new List<short>()));
		_dataArray.Add(new SkillBookItem(2, 4, 10, 1000, 2, 0, "icon_SkillBook_caishiwulong", 5, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 40, 10, 900, 1800, 1, 3, 1800, 5, allowRandomCreate: true, 35, isSpecial: false, 4, 36, 0, 0, 2, -1, -1, 240, new List<short>()));
		_dataArray.Add(new SkillBookItem(3, 6, 10, 1000, 3, 0, "icon_SkillBook_yuefuzalu", 7, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 35, 10, 2250, 4500, 2, 4, 3000, 6, allowRandomCreate: true, 30, isSpecial: false, 4, 36, 0, 0, 3, -1, -1, 400, new List<short>()));
		_dataArray.Add(new SkillBookItem(4, 8, 10, 1000, 4, 0, "icon_SkillBook_yuefuzalu", 9, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 30, 10, 4650, 9300, 3, 5, 4200, 7, allowRandomCreate: true, 25, isSpecial: false, 4, 36, 0, 0, 4, -1, -1, 600, new List<short>()));
		_dataArray.Add(new SkillBookItem(5, 10, 10, 1000, 5, 0, "icon_SkillBook_yuefuzalu", 11, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 25, 10, 8400, 16800, 4, 6, 5400, 7, allowRandomCreate: true, 20, isSpecial: false, 4, 36, 0, 0, 5, -1, -1, 850, new List<short>()));
		_dataArray.Add(new SkillBookItem(6, 12, 10, 1000, 6, 0, "icon_SkillBook_nichangqupu", 13, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 20, 10, 13800, 27600, 5, 7, 7200, 8, allowRandomCreate: true, 15, isSpecial: false, 4, 36, 0, 0, 6, -1, -1, 1140, new List<short>()));
		_dataArray.Add(new SkillBookItem(7, 14, 10, 1000, 7, 0, "icon_SkillBook_dunhuangqupu", 15, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 15, 10, 21150, 42300, 6, 8, 9000, 8, allowRandomCreate: true, 10, isSpecial: false, 4, 36, 0, 0, 7, -1, -1, 1480, new List<short>()));
		_dataArray.Add(new SkillBookItem(8, 16, 10, 1000, 8, 0, "icon_SkillBook_qincao", 17, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 10, 10, 30750, 61500, 6, 9, 10800, 8, allowRandomCreate: true, 5, isSpecial: false, 4, 36, 0, 0, 8, -1, -1, 1860, new List<short>()));
		_dataArray.Add(new SkillBookItem(9, 18, 10, 1000, 0, 9, "icon_SkillBook_weiqishijue", 19, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 50, 10, 150, 300, 0, 1, 600, 3, allowRandomCreate: true, 45, isSpecial: false, 4, 36, 1, 1, 9, -1, -1, 60, new List<short>()));
		_dataArray.Add(new SkillBookItem(10, 20, 10, 1000, 1, 9, "icon_SkillBook_weiqishijue", 21, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 45, 10, 300, 600, 0, 2, 1200, 4, allowRandomCreate: true, 40, isSpecial: false, 4, 36, 1, 1, 10, -1, -1, 130, new List<short>()));
		_dataArray.Add(new SkillBookItem(11, 22, 10, 1000, 2, 9, "icon_SkillBook_weiqishijue", 23, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 40, 10, 900, 1800, 1, 3, 1800, 5, allowRandomCreate: true, 35, isSpecial: false, 4, 36, 1, 1, 11, -1, -1, 240, new List<short>()));
		_dataArray.Add(new SkillBookItem(12, 24, 10, 1000, 3, 9, "icon_SkillBook_aofupu", 25, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 35, 10, 2250, 4500, 2, 4, 3000, 6, allowRandomCreate: true, 30, isSpecial: false, 4, 36, 1, 1, 12, -1, -1, 400, new List<short>()));
		_dataArray.Add(new SkillBookItem(13, 26, 10, 1000, 4, 9, "icon_SkillBook_aofupu", 27, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 30, 10, 4650, 9300, 3, 5, 4200, 7, allowRandomCreate: true, 25, isSpecial: false, 4, 36, 1, 1, 13, -1, -1, 600, new List<short>()));
		_dataArray.Add(new SkillBookItem(14, 28, 10, 1000, 5, 9, "icon_SkillBook_aofupu", 29, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 25, 10, 8400, 16800, 4, 6, 5400, 7, allowRandomCreate: true, 20, isSpecial: false, 4, 36, 1, 1, 14, -1, -1, 850, new List<short>()));
		_dataArray.Add(new SkillBookItem(15, 30, 10, 1000, 6, 9, "icon_SkillBook_lankepu", 31, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 20, 10, 13800, 27600, 5, 7, 7200, 8, allowRandomCreate: true, 15, isSpecial: false, 4, 36, 1, 1, 15, -1, -1, 1140, new List<short>()));
		_dataArray.Add(new SkillBookItem(16, 32, 10, 1000, 7, 9, "icon_SkillBook_jinpengshibabian", 33, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 15, 10, 21150, 42300, 6, 8, 9000, 8, allowRandomCreate: true, 10, isSpecial: false, 4, 36, 1, 1, 16, -1, -1, 1480, new List<short>()));
		_dataArray.Add(new SkillBookItem(17, 34, 10, 1000, 8, 9, "icon_SkillBook_ouxuepu", 35, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 10, 10, 30750, 61500, 6, 9, 10800, 8, allowRandomCreate: true, 5, isSpecial: false, 4, 36, 1, 1, 17, -1, -1, 1860, new List<short>()));
		_dataArray.Add(new SkillBookItem(18, 36, 10, 1000, 0, 18, "icon_SkillBook_chuxueji", 37, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 50, 10, 150, 300, 0, 1, 600, 3, allowRandomCreate: true, 45, isSpecial: false, 4, 36, 2, 2, 18, -1, -1, 60, new List<short>()));
		_dataArray.Add(new SkillBookItem(19, 38, 10, 1000, 1, 18, "icon_SkillBook_chuxueji", 39, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 45, 10, 300, 600, 0, 2, 1200, 4, allowRandomCreate: true, 40, isSpecial: false, 4, 36, 2, 2, 19, -1, -1, 130, new List<short>()));
		_dataArray.Add(new SkillBookItem(20, 40, 10, 1000, 2, 18, "icon_SkillBook_chuxueji", 41, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 40, 10, 900, 1800, 1, 3, 1800, 5, allowRandomCreate: true, 35, isSpecial: false, 4, 36, 2, 2, 20, -1, -1, 240, new List<short>()));
		_dataArray.Add(new SkillBookItem(21, 42, 10, 1000, 3, 18, "icon_SkillBook_hanmozhi", 43, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 35, 10, 2250, 4500, 2, 4, 3000, 6, allowRandomCreate: true, 30, isSpecial: false, 4, 36, 2, 2, 21, -1, -1, 400, new List<short>()));
		_dataArray.Add(new SkillBookItem(22, 44, 10, 1000, 4, 18, "icon_SkillBook_hanmozhi", 45, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 30, 10, 4650, 9300, 3, 5, 4200, 7, allowRandomCreate: true, 25, isSpecial: false, 4, 36, 2, 2, 22, -1, -1, 600, new List<short>()));
		_dataArray.Add(new SkillBookItem(23, 46, 10, 1000, 5, 18, "icon_SkillBook_hanmozhi", 47, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 25, 10, 8400, 16800, 4, 6, 5400, 7, allowRandomCreate: true, 20, isSpecial: false, 4, 36, 2, 2, 23, -1, -1, 850, new List<short>()));
		_dataArray.Add(new SkillBookItem(24, 48, 10, 1000, 6, 18, "icon_SkillBook_yongzibafa", 49, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 20, 10, 13800, 27600, 5, 7, 7200, 8, allowRandomCreate: true, 15, isSpecial: false, 4, 36, 2, 2, 24, -1, -1, 1140, new List<short>()));
		_dataArray.Add(new SkillBookItem(25, 50, 10, 1000, 7, 18, "icon_SkillBook_gesaerwangchuan", 51, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 15, 10, 21150, 42300, 6, 8, 9000, 8, allowRandomCreate: true, 10, isSpecial: false, 4, 36, 2, 2, 25, -1, -1, 1480, new List<short>()));
		_dataArray.Add(new SkillBookItem(26, 52, 10, 1000, 8, 18, "icon_SkillBook_shijing", 53, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 10, 10, 30750, 61500, 6, 9, 10800, 8, allowRandomCreate: true, 5, isSpecial: false, 4, 36, 2, 2, 26, -1, -1, 1860, new List<short>()));
		_dataArray.Add(new SkillBookItem(27, 54, 10, 1000, 0, 27, "icon_SkillBook_tuhuage", 55, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 50, 10, 150, 300, 0, 1, 600, 3, allowRandomCreate: true, 45, isSpecial: false, 4, 36, 3, 3, 27, -1, -1, 60, new List<short>()));
		_dataArray.Add(new SkillBookItem(28, 56, 10, 1000, 1, 27, "icon_SkillBook_tuhuage", 57, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 45, 10, 300, 600, 0, 2, 1200, 4, allowRandomCreate: true, 40, isSpecial: false, 4, 36, 3, 3, 28, -1, -1, 130, new List<short>()));
		_dataArray.Add(new SkillBookItem(29, 58, 10, 1000, 2, 27, "icon_SkillBook_tuhuage", 59, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 40, 10, 900, 1800, 1, 3, 1800, 5, allowRandomCreate: true, 35, isSpecial: false, 4, 36, 3, 3, 29, -1, -1, 240, new List<short>()));
		_dataArray.Add(new SkillBookItem(30, 60, 10, 1000, 3, 27, "icon_SkillBook_wuniutu", 61, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 35, 10, 2250, 4500, 2, 4, 3000, 6, allowRandomCreate: true, 30, isSpecial: false, 4, 36, 3, 3, 30, -1, -1, 400, new List<short>()));
		_dataArray.Add(new SkillBookItem(31, 62, 10, 1000, 4, 27, "icon_SkillBook_wuniutu", 63, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 30, 10, 4650, 9300, 3, 5, 4200, 7, allowRandomCreate: true, 25, isSpecial: false, 4, 36, 3, 3, 31, -1, -1, 600, new List<short>()));
		_dataArray.Add(new SkillBookItem(32, 64, 10, 1000, 5, 27, "icon_SkillBook_wuniutu", 65, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 25, 10, 8400, 16800, 4, 6, 5400, 7, allowRandomCreate: true, 20, isSpecial: false, 4, 36, 3, 3, 32, -1, -1, 850, new List<short>()));
		_dataArray.Add(new SkillBookItem(33, 66, 10, 1000, 6, 27, "icon_SkillBook_buniantu", 67, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 20, 10, 13800, 27600, 5, 7, 7200, 8, allowRandomCreate: true, 15, isSpecial: false, 4, 36, 3, 3, 33, -1, -1, 1140, new List<short>()));
		_dataArray.Add(new SkillBookItem(34, 68, 10, 1000, 7, 27, "icon_SkillBook_qingmingshanghetu", 69, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 15, 10, 21150, 42300, 6, 8, 9000, 8, allowRandomCreate: true, 10, isSpecial: false, 4, 36, 3, 3, 34, -1, -1, 1480, new List<short>()));
		_dataArray.Add(new SkillBookItem(35, 70, 10, 1000, 8, 27, "icon_SkillBook_luoshenfutu", 71, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 10, 10, 30750, 61500, 6, 9, 10800, 8, allowRandomCreate: true, 5, isSpecial: false, 4, 36, 3, 3, 35, -1, -1, 1860, new List<short>()));
		_dataArray.Add(new SkillBookItem(36, 72, 10, 1000, 0, 36, "icon_SkillBook_jinkoujue", 73, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 50, 10, 150, 300, 0, 1, 600, 3, allowRandomCreate: true, 45, isSpecial: false, 4, 36, 10, 4, 36, -1, -1, 60, new List<short>()));
		_dataArray.Add(new SkillBookItem(37, 74, 10, 1000, 1, 36, "icon_SkillBook_jinkoujue", 75, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 45, 10, 300, 600, 0, 2, 1200, 4, allowRandomCreate: true, 40, isSpecial: false, 4, 36, 10, 4, 37, -1, -1, 130, new List<short>()));
		_dataArray.Add(new SkillBookItem(38, 76, 10, 1000, 2, 36, "icon_SkillBook_jinkoujue", 77, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 40, 10, 900, 1800, 1, 3, 1800, 5, allowRandomCreate: true, 35, isSpecial: false, 4, 36, 10, 4, 38, -1, -1, 240, new List<short>()));
		_dataArray.Add(new SkillBookItem(39, 78, 10, 1000, 3, 36, "icon_SkillBook_ziweidoushu", 79, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 35, 10, 2250, 4500, 2, 4, 3000, 6, allowRandomCreate: true, 30, isSpecial: false, 4, 36, 10, 4, 39, -1, -1, 400, new List<short>()));
		_dataArray.Add(new SkillBookItem(40, 80, 10, 1000, 4, 36, "icon_SkillBook_ziweidoushu", 81, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 30, 10, 4650, 9300, 3, 5, 4200, 7, allowRandomCreate: true, 25, isSpecial: false, 4, 36, 10, 4, 40, -1, -1, 600, new List<short>()));
		_dataArray.Add(new SkillBookItem(41, 82, 10, 1000, 5, 36, "icon_SkillBook_ziweidoushu", 83, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 25, 10, 8400, 16800, 4, 6, 5400, 7, allowRandomCreate: true, 20, isSpecial: false, 4, 36, 10, 4, 41, -1, -1, 850, new List<short>()));
		_dataArray.Add(new SkillBookItem(42, 84, 10, 1000, 6, 36, "icon_SkillBook_zouzi", 85, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 20, 10, 13800, 27600, 5, 7, 7200, 8, allowRandomCreate: true, 15, isSpecial: false, 4, 36, 10, 4, 42, -1, -1, 1140, new List<short>()));
		_dataArray.Add(new SkillBookItem(43, 86, 10, 1000, 7, 36, "icon_SkillBook_hetuluoshu", 87, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 15, 10, 21150, 42300, 6, 8, 9000, 8, allowRandomCreate: true, 10, isSpecial: false, 4, 36, 10, 4, 43, -1, -1, 1480, new List<short>()));
		_dataArray.Add(new SkillBookItem(44, 88, 10, 1000, 8, 36, "icon_SkillBook_zhouyi", 89, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 10, 10, 30750, 61500, 6, 9, 10800, 8, allowRandomCreate: true, 5, isSpecial: false, 4, 36, 10, 4, 44, -1, -1, 1860, new List<short>()));
		_dataArray.Add(new SkillBookItem(45, 90, 10, 1000, 0, 45, "icon_SkillBook_zhuquanxiaopin", 91, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 50, 10, 150, 300, 0, 1, 600, 3, allowRandomCreate: true, 45, isSpecial: false, 4, 36, 11, 5, 45, -1, -1, 60, new List<short>()));
		_dataArray.Add(new SkillBookItem(46, 92, 10, 1000, 1, 45, "icon_SkillBook_zhuquanxiaopin", 93, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 45, 10, 300, 600, 0, 2, 1200, 4, allowRandomCreate: true, 40, isSpecial: false, 4, 36, 11, 5, 46, -1, -1, 130, new List<short>()));
		_dataArray.Add(new SkillBookItem(47, 94, 10, 1000, 2, 45, "icon_SkillBook_zhuquanxiaopin", 95, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 40, 10, 900, 1800, 1, 3, 1800, 5, allowRandomCreate: true, 35, isSpecial: false, 4, 36, 11, 5, 47, -1, -1, 240, new List<short>()));
		_dataArray.Add(new SkillBookItem(48, 96, 10, 1000, 3, 45, "icon_SkillBook_chajiulun", 97, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 35, 10, 2250, 4500, 2, 4, 3000, 6, allowRandomCreate: true, 30, isSpecial: false, 4, 36, 11, 5, 48, -1, -1, 400, new List<short>()));
		_dataArray.Add(new SkillBookItem(49, 98, 10, 1000, 4, 45, "icon_SkillBook_chajiulun", 99, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 30, 10, 4650, 9300, 3, 5, 4200, 7, allowRandomCreate: true, 25, isSpecial: false, 4, 36, 11, 5, 49, -1, -1, 600, new List<short>()));
		_dataArray.Add(new SkillBookItem(50, 100, 10, 1000, 5, 45, "icon_SkillBook_chajiulun", 101, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 25, 10, 8400, 16800, 4, 6, 5400, 7, allowRandomCreate: true, 20, isSpecial: false, 4, 36, 11, 5, 50, -1, -1, 850, new List<short>()));
		_dataArray.Add(new SkillBookItem(51, 102, 10, 1000, 6, 45, "icon_SkillBook_damingshuiji", 103, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 20, 10, 13800, 27600, 5, 7, 7200, 8, allowRandomCreate: true, 15, isSpecial: false, 4, 36, 11, 5, 51, -1, -1, 1140, new List<short>()));
		_dataArray.Add(new SkillBookItem(52, 104, 10, 1000, 7, 45, "icon_SkillBook_dongpojiujing", 105, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 15, 10, 21150, 42300, 6, 8, 9000, 8, allowRandomCreate: true, 10, isSpecial: false, 4, 36, 11, 5, 52, -1, -1, 1480, new List<short>()));
		_dataArray.Add(new SkillBookItem(53, 106, 10, 1000, 8, 45, "icon_SkillBook_chajing", 107, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 10, 10, 30750, 61500, 6, 9, 10800, 8, allowRandomCreate: true, 5, isSpecial: false, 4, 36, 11, 5, 53, -1, -1, 1860, new List<short>()));
		_dataArray.Add(new SkillBookItem(54, 108, 10, 1000, 0, 54, "icon_SkillBook_dingshu", 109, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 50, 10, 150, 300, 0, 1, 600, 3, allowRandomCreate: true, 45, isSpecial: false, 4, 36, 6, 6, 54, -1, -1, 60, new List<short>()));
		_dataArray.Add(new SkillBookItem(55, 110, 10, 1000, 1, 54, "icon_SkillBook_dingshu", 111, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 45, 10, 300, 600, 0, 2, 1200, 4, allowRandomCreate: true, 40, isSpecial: false, 4, 36, 6, 6, 55, -1, -1, 130, new List<short>()));
		_dataArray.Add(new SkillBookItem(56, 112, 10, 1000, 2, 54, "icon_SkillBook_dingshu", 113, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 40, 10, 900, 1800, 1, 3, 1800, 5, allowRandomCreate: true, 35, isSpecial: false, 4, 36, 6, 6, 56, -1, -1, 240, new List<short>()));
		_dataArray.Add(new SkillBookItem(57, 114, 10, 1000, 3, 54, "icon_SkillBook_xuezhuxiangjian", 115, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 35, 10, 2250, 4500, 2, 4, 3000, 6, allowRandomCreate: true, 30, isSpecial: false, 4, 36, 6, 6, 57, -1, -1, 400, new List<short>()));
		_dataArray.Add(new SkillBookItem(58, 116, 10, 1000, 4, 54, "icon_SkillBook_xuezhuxiangjian", 117, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 30, 10, 4650, 9300, 3, 5, 4200, 7, allowRandomCreate: true, 25, isSpecial: false, 4, 36, 6, 6, 58, -1, -1, 600, new List<short>()));
		_dataArray.Add(new SkillBookItem(59, 118, 10, 1000, 5, 54, "icon_SkillBook_xuezhuxiangjian", 119, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 25, 10, 8400, 16800, 4, 6, 5400, 7, allowRandomCreate: true, 20, isSpecial: false, 4, 36, 6, 6, 59, -1, -1, 850, new List<short>()));
	}

	private void CreateItems1()
	{
		_dataArray.Add(new SkillBookItem(60, 120, 10, 1000, 6, 54, "icon_SkillBook_kaogongji", 121, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 20, 10, 13800, 27600, 5, 7, 7200, 8, allowRandomCreate: true, 15, isSpecial: false, 4, 36, 6, 6, 60, -1, -1, 1140, new List<short>()));
		_dataArray.Add(new SkillBookItem(61, 122, 10, 1000, 7, 54, "icon_SkillBook_ouzuyishu", 123, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 15, 10, 21150, 42300, 6, 8, 9000, 8, allowRandomCreate: true, 10, isSpecial: false, 4, 36, 6, 6, 61, -1, -1, 1480, new List<short>()));
		_dataArray.Add(new SkillBookItem(62, 124, 10, 1000, 8, 54, "icon_SkillBook_kunwuce", 125, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 10, 10, 30750, 61500, 6, 9, 10800, 8, allowRandomCreate: true, 5, isSpecial: false, 4, 36, 6, 6, 62, -1, -1, 1860, new List<short>()));
		_dataArray.Add(new SkillBookItem(63, 126, 10, 1000, 0, 63, "icon_SkillBook_mugongkao", 127, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 50, 10, 150, 300, 0, 1, 600, 3, allowRandomCreate: true, 45, isSpecial: false, 4, 36, 7, 7, 63, -1, -1, 60, new List<short>()));
		_dataArray.Add(new SkillBookItem(64, 128, 10, 1000, 1, 63, "icon_SkillBook_mugongkao", 129, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 45, 10, 300, 600, 0, 2, 1200, 4, allowRandomCreate: true, 40, isSpecial: false, 4, 36, 7, 7, 64, -1, -1, 130, new List<short>()));
		_dataArray.Add(new SkillBookItem(65, 130, 10, 1000, 2, 63, "icon_SkillBook_mugongkao", 131, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 40, 10, 900, 1800, 1, 3, 1800, 5, allowRandomCreate: true, 35, isSpecial: false, 4, 36, 7, 7, 65, -1, -1, 240, new List<short>()));
		_dataArray.Add(new SkillBookItem(66, 132, 10, 1000, 3, 63, "icon_SkillBook_jiuzhangsuanshu", 133, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 35, 10, 2250, 4500, 2, 4, 3000, 6, allowRandomCreate: true, 30, isSpecial: false, 4, 36, 7, 7, 66, -1, -1, 400, new List<short>()));
		_dataArray.Add(new SkillBookItem(67, 134, 10, 1000, 4, 63, "icon_SkillBook_jiuzhangsuanshu", 135, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 30, 10, 4650, 9300, 3, 5, 4200, 7, allowRandomCreate: true, 25, isSpecial: false, 4, 36, 7, 7, 67, -1, -1, 600, new List<short>()));
		_dataArray.Add(new SkillBookItem(68, 136, 10, 1000, 5, 63, "icon_SkillBook_jiuzhangsuanshu", 137, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 25, 10, 8400, 16800, 4, 6, 5400, 7, allowRandomCreate: true, 20, isSpecial: false, 4, 36, 7, 7, 68, -1, -1, 850, new List<short>()));
		_dataArray.Add(new SkillBookItem(69, 138, 10, 1000, 6, 63, "icon_SkillBook_dajiangchuan", 139, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 20, 10, 13800, 27600, 5, 7, 7200, 8, allowRandomCreate: true, 15, isSpecial: false, 4, 36, 7, 7, 69, -1, -1, 1140, new List<short>()));
		_dataArray.Add(new SkillBookItem(70, 140, 10, 1000, 7, 63, "icon_SkillBook_linglongxuanji", 141, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 15, 10, 21150, 42300, 6, 8, 9000, 8, allowRandomCreate: true, 10, isSpecial: false, 4, 36, 7, 7, 70, -1, -1, 1480, new List<short>()));
		_dataArray.Add(new SkillBookItem(71, 142, 10, 1000, 8, 63, "icon_SkillBook_gongshujiwei", 143, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 10, 10, 30750, 61500, 6, 9, 10800, 8, allowRandomCreate: true, 5, isSpecial: false, 4, 36, 7, 7, 71, -1, -1, 1860, new List<short>()));
		_dataArray.Add(new SkillBookItem(72, 144, 10, 1000, 0, 72, "icon_SkillBook_maijing", 145, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 50, 10, 150, 300, 0, 1, 600, 3, allowRandomCreate: true, 45, isSpecial: false, 4, 36, 4, 8, 72, -1, -1, 60, new List<short>()));
		_dataArray.Add(new SkillBookItem(73, 146, 10, 1000, 1, 72, "icon_SkillBook_maijing", 147, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 45, 10, 300, 600, 0, 2, 1200, 4, allowRandomCreate: true, 40, isSpecial: false, 4, 36, 4, 8, 73, -1, -1, 130, new List<short>()));
		_dataArray.Add(new SkillBookItem(74, 148, 10, 1000, 2, 72, "icon_SkillBook_maijing", 149, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 40, 10, 900, 1800, 1, 3, 1800, 5, allowRandomCreate: true, 35, isSpecial: false, 4, 36, 4, 8, 74, -1, -1, 240, new List<short>()));
		_dataArray.Add(new SkillBookItem(75, 150, 10, 1000, 3, 72, "icon_SkillBook_qianjinyifang", 151, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 35, 10, 2250, 4500, 2, 4, 3000, 6, allowRandomCreate: true, 30, isSpecial: false, 4, 36, 4, 8, 75, -1, -1, 400, new List<short>()));
		_dataArray.Add(new SkillBookItem(76, 152, 10, 1000, 4, 72, "icon_SkillBook_qianjinyifang", 153, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 30, 10, 4650, 9300, 3, 5, 4200, 7, allowRandomCreate: true, 25, isSpecial: false, 4, 36, 4, 8, 76, -1, -1, 600, new List<short>()));
		_dataArray.Add(new SkillBookItem(77, 154, 10, 1000, 5, 72, "icon_SkillBook_qianjinyifang", 155, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 25, 10, 8400, 16800, 4, 6, 5400, 7, allowRandomCreate: true, 20, isSpecial: false, 4, 36, 4, 8, 77, -1, -1, 850, new List<short>()));
		_dataArray.Add(new SkillBookItem(78, 156, 10, 1000, 6, 72, "icon_SkillBook_jinkuiyaolüe", 157, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 20, 10, 13800, 27600, 5, 7, 7200, 8, allowRandomCreate: true, 15, isSpecial: false, 4, 36, 4, 8, 78, -1, -1, 1140, new List<short>()));
		_dataArray.Add(new SkillBookItem(79, 158, 10, 1000, 7, 72, "icon_SkillBook_shennongbencaojing", 159, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 15, 10, 21150, 42300, 6, 8, 9000, 8, allowRandomCreate: true, 10, isSpecial: false, 4, 36, 4, 8, 79, -1, -1, 1480, new List<short>()));
		_dataArray.Add(new SkillBookItem(80, 160, 10, 1000, 8, 72, "icon_SkillBook_huangdineijing", 161, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 10, 10, 30750, 61500, 6, 9, 10800, 8, allowRandomCreate: true, 5, isSpecial: false, 4, 36, 4, 8, 80, -1, -1, 1860, new List<short>()));
		_dataArray.Add(new SkillBookItem(81, 162, 10, 1000, 0, 81, "icon_SkillBook_liuhaijing", 163, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 50, 10, 150, 300, 0, 1, 600, 3, allowRandomCreate: true, 45, isSpecial: false, 4, 36, 5, 9, 81, -1, -1, 60, new List<short>()));
		_dataArray.Add(new SkillBookItem(82, 164, 10, 1000, 1, 81, "icon_SkillBook_liuhaijing", 165, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 45, 10, 300, 600, 0, 2, 1200, 4, allowRandomCreate: true, 40, isSpecial: false, 4, 36, 5, 9, 82, -1, -1, 130, new List<short>()));
		_dataArray.Add(new SkillBookItem(83, 166, 10, 1000, 2, 81, "icon_SkillBook_liuhaijing", 167, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 40, 10, 900, 1800, 1, 3, 1800, 5, allowRandomCreate: true, 35, isSpecial: false, 4, 36, 5, 9, 83, -1, -1, 240, new List<short>()));
		_dataArray.Add(new SkillBookItem(84, 168, 10, 1000, 3, 81, "icon_SkillBook_xiewulun", 169, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 35, 10, 2250, 4500, 2, 4, 3000, 6, allowRandomCreate: true, 30, isSpecial: false, 4, 36, 5, 9, 84, -1, -1, 400, new List<short>()));
		_dataArray.Add(new SkillBookItem(85, 170, 10, 1000, 4, 81, "icon_SkillBook_xiewulun", 171, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 30, 10, 4650, 9300, 3, 5, 4200, 7, allowRandomCreate: true, 25, isSpecial: false, 4, 36, 5, 9, 85, -1, -1, 600, new List<short>()));
		_dataArray.Add(new SkillBookItem(86, 172, 10, 1000, 5, 81, "icon_SkillBook_xiewulun", 173, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 25, 10, 8400, 16800, 4, 6, 5400, 7, allowRandomCreate: true, 20, isSpecial: false, 4, 36, 5, 9, 86, -1, -1, 850, new List<short>()));
		_dataArray.Add(new SkillBookItem(87, 174, 10, 1000, 6, 81, "icon_SkillBook_benjingxiapinzhu", 175, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 20, 10, 13800, 27600, 5, 7, 7200, 8, allowRandomCreate: true, 15, isSpecial: false, 4, 36, 5, 9, 87, -1, -1, 1140, new List<short>()));
		_dataArray.Add(new SkillBookItem(88, 176, 10, 1000, 7, 81, "icon_SkillBook_baisunjing", 177, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 15, 10, 21150, 42300, 6, 8, 9000, 8, allowRandomCreate: true, 10, isSpecial: false, 4, 36, 5, 9, 88, -1, -1, 1480, new List<short>()));
		_dataArray.Add(new SkillBookItem(89, 178, 10, 1000, 8, 81, "icon_SkillBook_qingyimiyao", 179, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 10, 10, 30750, 61500, 6, 9, 10800, 8, allowRandomCreate: true, 5, isSpecial: false, 4, 36, 5, 9, 89, -1, -1, 1860, new List<short>()));
		_dataArray.Add(new SkillBookItem(90, 180, 10, 1000, 0, 90, "icon_SkillBook_hualuopu", 181, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 50, 10, 150, 300, 0, 1, 600, 3, allowRandomCreate: true, 45, isSpecial: false, 4, 36, 8, 10, 90, -1, -1, 60, new List<short>()));
		_dataArray.Add(new SkillBookItem(91, 182, 10, 1000, 1, 90, "icon_SkillBook_hualuopu", 183, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 45, 10, 300, 600, 0, 2, 1200, 4, allowRandomCreate: true, 40, isSpecial: false, 4, 36, 8, 10, 91, -1, -1, 130, new List<short>()));
		_dataArray.Add(new SkillBookItem(92, 184, 10, 1000, 2, 90, "icon_SkillBook_hualuopu", 185, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 40, 10, 900, 1800, 1, 3, 1800, 5, allowRandomCreate: true, 35, isSpecial: false, 4, 36, 8, 10, 92, -1, -1, 240, new List<short>()));
		_dataArray.Add(new SkillBookItem(93, 186, 10, 1000, 3, 90, "icon_SkillBook_sangcanjing", 187, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 35, 10, 2250, 4500, 2, 4, 3000, 6, allowRandomCreate: true, 30, isSpecial: false, 4, 36, 8, 10, 93, -1, -1, 400, new List<short>()));
		_dataArray.Add(new SkillBookItem(94, 188, 10, 1000, 4, 90, "icon_SkillBook_sangcanjing", 189, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 30, 10, 4650, 9300, 3, 5, 4200, 7, allowRandomCreate: true, 25, isSpecial: false, 4, 36, 8, 10, 94, -1, -1, 600, new List<short>()));
		_dataArray.Add(new SkillBookItem(95, 190, 10, 1000, 5, 90, "icon_SkillBook_sangcanjing", 191, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 25, 10, 8400, 16800, 4, 6, 5400, 7, allowRandomCreate: true, 20, isSpecial: false, 4, 36, 8, 10, 95, -1, -1, 850, new List<short>()));
		_dataArray.Add(new SkillBookItem(96, 192, 10, 1000, 6, 90, "icon_SkillBook_yili", 193, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 20, 10, 13800, 27600, 5, 7, 7200, 8, allowRandomCreate: true, 15, isSpecial: false, 4, 36, 8, 10, 96, -1, -1, 1140, new List<short>()));
		_dataArray.Add(new SkillBookItem(97, 194, 10, 1000, 7, 90, "icon_SkillBook_baihuajince", 195, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 15, 10, 21150, 42300, 6, 8, 9000, 8, allowRandomCreate: true, 10, isSpecial: false, 4, 36, 8, 10, 97, -1, -1, 1480, new List<short>()));
		_dataArray.Add(new SkillBookItem(98, 196, 10, 1000, 8, 90, "icon_SkillBook_xiancanleizuji", 197, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 10, 10, 30750, 61500, 6, 9, 10800, 8, allowRandomCreate: true, 5, isSpecial: false, 4, 36, 8, 10, 98, -1, -1, 1860, new List<short>()));
		_dataArray.Add(new SkillBookItem(99, 198, 10, 1000, 0, 99, "icon_SkillBook_jieyu", 199, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 50, 10, 150, 300, 0, 1, 600, 3, allowRandomCreate: true, 45, isSpecial: false, 4, 36, 9, 11, 99, -1, -1, 60, new List<short>()));
		_dataArray.Add(new SkillBookItem(100, 200, 10, 1000, 1, 99, "icon_SkillBook_jieyu", 201, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 45, 10, 300, 600, 0, 2, 1200, 4, allowRandomCreate: true, 40, isSpecial: false, 4, 36, 9, 11, 100, -1, -1, 130, new List<short>()));
		_dataArray.Add(new SkillBookItem(101, 202, 10, 1000, 2, 99, "icon_SkillBook_jieyu", 203, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 40, 10, 900, 1800, 1, 3, 1800, 5, allowRandomCreate: true, 35, isSpecial: false, 4, 36, 9, 11, 101, -1, -1, 240, new List<short>()));
		_dataArray.Add(new SkillBookItem(102, 204, 10, 1000, 3, 99, "icon_SkillBook_bazhenbabaoji", 205, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 35, 10, 2250, 4500, 2, 4, 3000, 6, allowRandomCreate: true, 30, isSpecial: false, 4, 36, 9, 11, 102, -1, -1, 400, new List<short>()));
		_dataArray.Add(new SkillBookItem(103, 206, 10, 1000, 4, 99, "icon_SkillBook_bazhenbabaoji", 207, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 30, 10, 4650, 9300, 3, 5, 4200, 7, allowRandomCreate: true, 25, isSpecial: false, 4, 36, 9, 11, 103, -1, -1, 600, new List<short>()));
		_dataArray.Add(new SkillBookItem(104, 208, 10, 1000, 5, 99, "icon_SkillBook_bazhenbabaoji", 209, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 25, 10, 8400, 16800, 4, 6, 5400, 7, allowRandomCreate: true, 20, isSpecial: false, 4, 36, 9, 11, 104, -1, -1, 850, new List<short>()));
		_dataArray.Add(new SkillBookItem(105, 210, 10, 1000, 6, 99, "icon_SkillBook_baotajing", 211, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 20, 10, 13800, 27600, 5, 7, 7200, 8, allowRandomCreate: true, 15, isSpecial: false, 4, 36, 9, 11, 105, -1, -1, 1140, new List<short>()));
		_dataArray.Add(new SkillBookItem(106, 212, 10, 1000, 7, 99, "icon_SkillBook_yixiangyangyujing", 213, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 15, 10, 21150, 42300, 6, 8, 9000, 8, allowRandomCreate: true, 10, isSpecial: false, 4, 36, 9, 11, 106, -1, -1, 1480, new List<short>()));
		_dataArray.Add(new SkillBookItem(107, 214, 10, 1000, 8, 99, "icon_SkillBook_yaochiyouji", 215, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 10, 10, 30750, 61500, 6, 9, 10800, 8, allowRandomCreate: true, 5, isSpecial: false, 4, 36, 9, 11, 107, -1, -1, 1860, new List<short>()));
		_dataArray.Add(new SkillBookItem(108, 216, 10, 1000, 0, 108, "icon_SkillBook_huangtingjing", 217, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 50, 10, 150, 300, 0, 1, 600, 3, allowRandomCreate: true, 45, isSpecial: false, 4, 36, 15, 12, 108, -1, -1, 60, new List<short>()));
		_dataArray.Add(new SkillBookItem(109, 218, 10, 1000, 1, 108, "icon_SkillBook_huangtingjing", 219, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 45, 10, 300, 600, 0, 2, 1200, 4, allowRandomCreate: true, 40, isSpecial: false, 4, 36, 15, 12, 109, -1, -1, 130, new List<short>()));
		_dataArray.Add(new SkillBookItem(110, 220, 10, 1000, 2, 108, "icon_SkillBook_huangtingjing", 221, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 40, 10, 900, 1800, 1, 3, 1800, 5, allowRandomCreate: true, 35, isSpecial: false, 4, 36, 15, 12, 110, -1, -1, 240, new List<short>()));
		_dataArray.Add(new SkillBookItem(111, 222, 10, 1000, 3, 108, "icon_SkillBook_baopiaozi", 223, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 35, 10, 2250, 4500, 2, 4, 3000, 6, allowRandomCreate: true, 30, isSpecial: false, 4, 36, 15, 12, 111, -1, -1, 400, new List<short>()));
		_dataArray.Add(new SkillBookItem(112, 224, 10, 1000, 4, 108, "icon_SkillBook_baopiaozi", 225, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 30, 10, 4650, 9300, 3, 5, 4200, 7, allowRandomCreate: true, 25, isSpecial: false, 4, 36, 15, 12, 112, -1, -1, 600, new List<short>()));
		_dataArray.Add(new SkillBookItem(113, 226, 10, 1000, 5, 108, "icon_SkillBook_baopiaozi", 227, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 25, 10, 8400, 16800, 4, 6, 5400, 7, allowRandomCreate: true, 20, isSpecial: false, 4, 36, 15, 12, 113, -1, -1, 850, new List<short>()));
		_dataArray.Add(new SkillBookItem(114, 228, 10, 1000, 6, 108, "icon_SkillBook_nanhuazhenjing", 229, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 20, 10, 13800, 27600, 5, 7, 7200, 8, allowRandomCreate: true, 15, isSpecial: false, 4, 36, 15, 12, 114, -1, -1, 1140, new List<short>()));
		_dataArray.Add(new SkillBookItem(115, 230, 10, 1000, 7, 108, "icon_SkillBook_huangdiyinfujing", 231, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 15, 10, 21150, 42300, 6, 8, 9000, 8, allowRandomCreate: true, 10, isSpecial: false, 4, 36, 15, 12, 115, -1, -1, 1480, new List<short>()));
		_dataArray.Add(new SkillBookItem(116, 232, 10, 1000, 8, 108, "icon_SkillBook_daodezhenjing", 233, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 10, 10, 30750, 61500, 6, 9, 10800, 8, allowRandomCreate: true, 5, isSpecial: false, 4, 36, 15, 12, 116, -1, -1, 1860, new List<short>()));
		_dataArray.Add(new SkillBookItem(117, 234, 10, 1000, 0, 117, "icon_SkillBook_xinjing", 235, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 50, 10, 150, 300, 0, 1, 600, 3, allowRandomCreate: true, 45, isSpecial: false, 4, 36, 14, 13, 117, -1, -1, 60, new List<short>()));
		_dataArray.Add(new SkillBookItem(118, 236, 10, 1000, 1, 117, "icon_SkillBook_xinjing", 237, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 45, 10, 300, 600, 0, 2, 1200, 4, allowRandomCreate: true, 40, isSpecial: false, 4, 36, 14, 13, 118, -1, -1, 130, new List<short>()));
		_dataArray.Add(new SkillBookItem(119, 238, 10, 1000, 2, 117, "icon_SkillBook_xinjing", 239, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 40, 10, 900, 1800, 1, 3, 1800, 5, allowRandomCreate: true, 35, isSpecial: false, 4, 36, 14, 13, 119, -1, -1, 240, new List<short>()));
	}

	private void CreateItems2()
	{
		_dataArray.Add(new SkillBookItem(120, 240, 10, 1000, 3, 117, "icon_SkillBook_sishierzhangjing", 241, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 35, 10, 2250, 4500, 2, 4, 3000, 6, allowRandomCreate: true, 30, isSpecial: false, 4, 36, 14, 13, 120, -1, -1, 400, new List<short>()));
		_dataArray.Add(new SkillBookItem(121, 242, 10, 1000, 4, 117, "icon_SkillBook_sishierzhangjing", 243, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 30, 10, 4650, 9300, 3, 5, 4200, 7, allowRandomCreate: true, 25, isSpecial: false, 4, 36, 14, 13, 121, -1, -1, 600, new List<short>()));
		_dataArray.Add(new SkillBookItem(122, 244, 10, 1000, 5, 117, "icon_SkillBook_sishierzhangjing", 245, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 25, 10, 8400, 16800, 4, 6, 5400, 7, allowRandomCreate: true, 20, isSpecial: false, 4, 36, 14, 13, 122, -1, -1, 850, new List<short>()));
		_dataArray.Add(new SkillBookItem(123, 246, 10, 1000, 6, 117, "icon_SkillBook_huayanjing", 247, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 20, 10, 13800, 27600, 5, 7, 7200, 8, allowRandomCreate: true, 15, isSpecial: false, 4, 36, 14, 13, 123, -1, -1, 1140, new List<short>()));
		_dataArray.Add(new SkillBookItem(124, 248, 10, 1000, 7, 117, "icon_SkillBook_miaofalianhuajing", 249, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 15, 10, 21150, 42300, 6, 8, 9000, 8, allowRandomCreate: true, 10, isSpecial: false, 4, 36, 14, 13, 124, -1, -1, 1480, new List<short>()));
		_dataArray.Add(new SkillBookItem(125, 250, 10, 1000, 8, 117, "icon_SkillBook_lengyanjing", 251, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 10, 10, 30750, 61500, 6, 9, 10800, 8, allowRandomCreate: true, 5, isSpecial: false, 4, 36, 14, 13, 125, -1, -1, 1860, new List<short>()));
		_dataArray.Add(new SkillBookItem(126, 252, 10, 1000, 0, 126, "icon_SkillBook_siminshilu", 253, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 50, 10, 150, 300, 0, 1, 600, 3, allowRandomCreate: true, 45, isSpecial: false, 4, 36, 12, 14, 126, -1, -1, 60, new List<short>()));
		_dataArray.Add(new SkillBookItem(127, 254, 10, 1000, 1, 126, "icon_SkillBook_siminshilu", 255, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 45, 10, 300, 600, 0, 2, 1200, 4, allowRandomCreate: true, 40, isSpecial: false, 4, 36, 12, 14, 127, -1, -1, 130, new List<short>()));
		_dataArray.Add(new SkillBookItem(128, 256, 10, 1000, 2, 126, "icon_SkillBook_siminshilu", 257, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 40, 10, 900, 1800, 1, 3, 1800, 5, allowRandomCreate: true, 35, isSpecial: false, 4, 36, 12, 14, 128, -1, -1, 240, new List<short>()));
		_dataArray.Add(new SkillBookItem(129, 258, 10, 1000, 3, 126, "icon_SkillBook_fanshengzhishu", 259, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 35, 10, 2250, 4500, 2, 4, 3000, 6, allowRandomCreate: true, 30, isSpecial: false, 4, 36, 12, 14, 129, -1, -1, 400, new List<short>()));
		_dataArray.Add(new SkillBookItem(130, 260, 10, 1000, 4, 126, "icon_SkillBook_fanshengzhishu", 261, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 30, 10, 4650, 9300, 3, 5, 4200, 7, allowRandomCreate: true, 25, isSpecial: false, 4, 36, 12, 14, 130, -1, -1, 600, new List<short>()));
		_dataArray.Add(new SkillBookItem(131, 262, 10, 1000, 5, 126, "icon_SkillBook_fanshengzhishu", 263, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 25, 10, 8400, 16800, 4, 6, 5400, 7, allowRandomCreate: true, 20, isSpecial: false, 4, 36, 12, 14, 131, -1, -1, 850, new List<short>()));
		_dataArray.Add(new SkillBookItem(132, 264, 10, 1000, 6, 126, "icon_SkillBook_sijing", 265, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 20, 10, 13800, 27600, 5, 7, 7200, 8, allowRandomCreate: true, 15, isSpecial: false, 4, 36, 12, 14, 132, -1, -1, 1140, new List<short>()));
		_dataArray.Add(new SkillBookItem(133, 266, 10, 1000, 7, 126, "icon_SkillBook_qiminyaoshu", 267, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 15, 10, 21150, 42300, 6, 8, 9000, 8, allowRandomCreate: true, 10, isSpecial: false, 4, 36, 12, 14, 133, -1, -1, 1480, new List<short>()));
		_dataArray.Add(new SkillBookItem(134, 268, 10, 1000, 8, 126, "icon_SkillBook_sizhenlu", 269, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 10, 10, 30750, 61500, 6, 9, 10800, 8, allowRandomCreate: true, 5, isSpecial: false, 4, 36, 12, 14, 134, -1, -1, 1860, new List<short>()));
		_dataArray.Add(new SkillBookItem(135, 270, 10, 1000, 0, 135, "icon_SkillBook_dongpozhilin", 271, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 50, 10, 150, 300, 0, 1, 600, 3, allowRandomCreate: true, 45, isSpecial: false, 4, 36, 13, 15, 135, -1, -1, 60, new List<short>()));
		_dataArray.Add(new SkillBookItem(136, 272, 10, 1000, 1, 135, "icon_SkillBook_dongpozhilin", 273, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 45, 10, 300, 600, 0, 2, 1200, 4, allowRandomCreate: true, 40, isSpecial: false, 4, 36, 13, 15, 136, -1, -1, 130, new List<short>()));
		_dataArray.Add(new SkillBookItem(137, 274, 10, 1000, 2, 135, "icon_SkillBook_dongpozhilin", 275, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 40, 10, 900, 1800, 1, 3, 1800, 5, allowRandomCreate: true, 35, isSpecial: false, 4, 36, 13, 15, 137, -1, -1, 240, new List<short>()));
		_dataArray.Add(new SkillBookItem(138, 276, 10, 1000, 3, 135, "icon_SkillBook_erya", 277, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 35, 10, 2250, 4500, 2, 4, 3000, 6, allowRandomCreate: true, 30, isSpecial: false, 4, 36, 13, 15, 138, -1, -1, 400, new List<short>()));
		_dataArray.Add(new SkillBookItem(139, 278, 10, 1000, 4, 135, "icon_SkillBook_erya", 279, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 30, 10, 4650, 9300, 3, 5, 4200, 7, allowRandomCreate: true, 25, isSpecial: false, 4, 36, 13, 15, 139, -1, -1, 600, new List<short>()));
		_dataArray.Add(new SkillBookItem(140, 280, 10, 1000, 5, 135, "icon_SkillBook_erya", 281, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 25, 10, 8400, 16800, 4, 6, 5400, 7, allowRandomCreate: true, 20, isSpecial: false, 4, 36, 13, 15, 140, -1, -1, 850, new List<short>()));
		_dataArray.Add(new SkillBookItem(141, 282, 10, 1000, 6, 135, "icon_SkillBook_qilüe", 283, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 20, 10, 13800, 27600, 5, 7, 7200, 8, allowRandomCreate: true, 15, isSpecial: false, 4, 36, 13, 15, 141, -1, -1, 1140, new List<short>()));
		_dataArray.Add(new SkillBookItem(142, 284, 10, 1000, 7, 135, "icon_SkillBook_mozi", 285, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 15, 10, 21150, 42300, 6, 8, 9000, 8, allowRandomCreate: true, 10, isSpecial: false, 4, 36, 13, 15, 142, -1, -1, 1480, new List<short>()));
		_dataArray.Add(new SkillBookItem(143, 286, 10, 1000, 8, 135, "icon_SkillBook_mengxibitan", 287, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 10, 10, 30750, 61500, 6, 9, 10800, 8, allowRandomCreate: true, 5, isSpecial: false, 4, 36, 13, 15, 143, -1, -1, 1860, new List<short>()));
		_dataArray.Add(new SkillBookItem(144, 288, 10, 1001, 0, 144, "icon_SkillBook_jingchangong", 289, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 50, 10, 150, 300, 0, 2, 600, 3, allowRandomCreate: true, 45, isSpecial: false, 4, 36, -1, -1, -1, 0, 0, 60, new List<short> { 117, 118, 119, 120, 121, 122, 123, 124, 125 }));
		_dataArray.Add(new SkillBookItem(145, 290, 10, 1001, 1, 144, "icon_SkillBook_jingchangong", 291, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 45, 10, 300, 600, 0, 4, 1200, 4, allowRandomCreate: true, 40, isSpecial: false, 4, 36, -1, -1, -1, 0, 1, 130, new List<short> { 118, 119, 120, 121, 122, 123, 124, 125 }));
		_dataArray.Add(new SkillBookItem(146, 292, 10, 1001, 2, 144, "icon_SkillBook_jingchangong", 293, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 40, 10, 900, 1800, 1, 6, 1800, 5, allowRandomCreate: true, 35, isSpecial: false, 4, 36, -1, -1, -1, 0, 2, 240, new List<short> { 119, 120, 121, 122, 123, 124, 125 }));
		_dataArray.Add(new SkillBookItem(147, 294, 10, 1001, 3, 144, "icon_SkillBook_wusechangong", 295, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 35, 10, 2250, 4500, 2, 8, 3000, 6, allowRandomCreate: true, 30, isSpecial: false, 4, 36, -1, -1, -1, 0, 3, 400, new List<short> { 120, 121, 122, 123, 124, 125 }));
		_dataArray.Add(new SkillBookItem(148, 296, 10, 1001, 4, 144, "icon_SkillBook_wusechangong", 297, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 30, 10, 4650, 9300, 3, 10, 4200, 7, allowRandomCreate: true, 25, isSpecial: false, 4, 36, -1, -1, -1, 0, 4, 600, new List<short> { 121, 122, 123, 124, 125 }));
		_dataArray.Add(new SkillBookItem(149, 298, 10, 1001, 5, 144, "icon_SkillBook_wusechangong", 299, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 25, 10, 8400, 16800, 4, 12, 5400, 7, allowRandomCreate: true, 20, isSpecial: false, 4, 36, -1, -1, -1, 0, 5, 850, new List<short> { 122, 123, 124, 125 }));
		_dataArray.Add(new SkillBookItem(150, 300, 10, 1001, 6, 144, "icon_SkillBook_aluohanshengong", 301, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 20, 10, 13800, 27600, 5, 14, 7200, 8, allowRandomCreate: true, 15, isSpecial: false, 4, 36, -1, -1, -1, 0, 6, 1140, new List<short> { 123, 124, 125 }));
		_dataArray.Add(new SkillBookItem(151, 302, 10, 1001, 7, 144, "icon_SkillBook_yijinjing", 303, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 15, 10, 21150, 42300, 6, 16, 9000, 8, allowRandomCreate: true, 10, isSpecial: false, 4, 36, -1, -1, -1, 0, 7, 1480, new List<short> { 124, 125 }));
		_dataArray.Add(new SkillBookItem(152, 304, 10, 1001, 8, 144, "icon_SkillBook_xisuijing", 305, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 10, 10, 30750, 61500, 6, 18, 10800, 8, allowRandomCreate: true, 5, isSpecial: false, 4, 36, -1, -1, -1, 0, 8, 1860, new List<short> { 125 }));
		_dataArray.Add(new SkillBookItem(153, 306, 10, 1001, 0, 153, "icon_SkillBook_dantiankaihegong", 307, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 50, 10, 150, 300, 0, 2, 600, 3, allowRandomCreate: true, 45, isSpecial: false, 4, 36, -1, -1, -1, 0, 9, 60, new List<short>
		{
			108, 109, 110, 111, 112, 113, 114, 115, 116, 117,
			118, 119, 120, 121, 122, 123, 124, 125
		}));
		_dataArray.Add(new SkillBookItem(154, 308, 10, 1001, 1, 153, "icon_SkillBook_dantiankaihegong", 309, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 45, 10, 300, 600, 0, 4, 1200, 4, allowRandomCreate: true, 40, isSpecial: false, 4, 36, -1, -1, -1, 0, 10, 130, new List<short>
		{
			109, 110, 111, 112, 113, 114, 115, 116, 118, 119,
			120, 121, 122, 123, 124, 125
		}));
		_dataArray.Add(new SkillBookItem(155, 310, 10, 1001, 2, 153, "icon_SkillBook_dantiankaihegong", 311, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 40, 10, 900, 1800, 1, 6, 1800, 5, allowRandomCreate: true, 35, isSpecial: false, 4, 36, -1, -1, -1, 0, 11, 240, new List<short>
		{
			110, 111, 112, 113, 114, 115, 116, 119, 120, 121,
			122, 123, 124, 125
		}));
		_dataArray.Add(new SkillBookItem(156, 312, 10, 1001, 3, 153, "icon_SkillBook_lingwugong", 313, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 35, 10, 2250, 4500, 2, 8, 3000, 6, allowRandomCreate: true, 30, isSpecial: false, 4, 36, -1, -1, -1, 0, 12, 400, new List<short>
		{
			111, 112, 113, 114, 115, 116, 120, 121, 122, 123,
			124, 125
		}));
		_dataArray.Add(new SkillBookItem(157, 314, 10, 1001, 4, 153, "icon_SkillBook_lingwugong", 315, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 30, 10, 4650, 9300, 3, 10, 4200, 7, allowRandomCreate: true, 25, isSpecial: false, 4, 36, -1, -1, -1, 0, 13, 600, new List<short> { 112, 113, 114, 115, 116, 121, 122, 123, 124, 125 }));
		_dataArray.Add(new SkillBookItem(158, 316, 10, 1001, 5, 153, "icon_SkillBook_lingwugong", 317, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 25, 10, 8400, 16800, 4, 12, 5400, 7, allowRandomCreate: true, 20, isSpecial: false, 4, 36, -1, -1, -1, 0, 14, 850, new List<short> { 113, 114, 115, 116, 122, 123, 124, 125 }));
		_dataArray.Add(new SkillBookItem(159, 318, 10, 1001, 6, 153, "icon_SkillBook_zuowangxuangong", 319, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 20, 10, 13800, 27600, 5, 14, 7200, 8, allowRandomCreate: true, 15, isSpecial: false, 4, 36, -1, -1, -1, 0, 15, 1140, new List<short> { 114, 115, 116, 123, 124, 125 }));
		_dataArray.Add(new SkillBookItem(160, 320, 10, 1001, 7, 153, "icon_SkillBook_dabanniepanfa", 321, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 15, 10, 21150, 42300, 6, 16, 9000, 8, allowRandomCreate: true, 10, isSpecial: false, 4, 36, -1, -1, -1, 0, 16, 1480, new List<short> { 115, 116, 124, 125 }));
		_dataArray.Add(new SkillBookItem(161, 322, 10, 1001, 0, 161, "icon_SkillBook_tongrenshuxuetujing", 323, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 50, 10, 150, 300, 0, 2, 600, 3, allowRandomCreate: true, 45, isSpecial: false, 4, 36, -1, -1, -1, 0, 17, 60, new List<short> { 72, 73, 74, 75, 76, 77, 78, 79, 80 }));
		_dataArray.Add(new SkillBookItem(162, 324, 10, 1001, 1, 161, "icon_SkillBook_tongrenshuxuetujing", 325, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 45, 10, 300, 600, 0, 4, 1200, 4, allowRandomCreate: true, 40, isSpecial: false, 4, 36, -1, -1, -1, 0, 18, 130, new List<short> { 73, 74, 75, 76, 77, 78, 79, 80 }));
		_dataArray.Add(new SkillBookItem(163, 326, 10, 1001, 2, 161, "icon_SkillBook_tongrenshuxuetujing", 327, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 40, 10, 900, 1800, 1, 6, 1800, 5, allowRandomCreate: true, 35, isSpecial: false, 4, 36, -1, -1, -1, 0, 19, 240, new List<short> { 74, 75, 76, 77, 78, 79, 80 }));
		_dataArray.Add(new SkillBookItem(164, 328, 10, 1001, 3, 161, "icon_SkillBook_najinglingshupian", 329, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 35, 10, 2250, 4500, 2, 8, 3000, 6, allowRandomCreate: true, 30, isSpecial: false, 4, 36, -1, -1, -1, 0, 20, 400, new List<short> { 75, 76, 77, 78, 79, 80 }));
		_dataArray.Add(new SkillBookItem(165, 330, 10, 1001, 4, 161, "icon_SkillBook_najinglingshupian", 331, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 30, 10, 4650, 9300, 3, 10, 4200, 7, allowRandomCreate: true, 25, isSpecial: false, 4, 36, -1, -1, -1, 0, 21, 600, new List<short> { 76, 77, 78, 79, 80 }));
		_dataArray.Add(new SkillBookItem(166, 332, 10, 1001, 0, 166, "icon_SkillBook_wudangtaihegong", 333, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 50, 10, 150, 300, 0, 2, 600, 3, allowRandomCreate: true, 45, isSpecial: false, 4, 36, -1, -1, -1, 0, 22, 60, new List<short> { 108, 109, 110, 111, 112, 113, 114, 115, 116 }));
		_dataArray.Add(new SkillBookItem(167, 334, 10, 1001, 1, 166, "icon_SkillBook_wudangtaihegong", 335, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 45, 10, 300, 600, 0, 4, 1200, 4, allowRandomCreate: true, 40, isSpecial: false, 4, 36, -1, -1, -1, 0, 23, 130, new List<short> { 109, 110, 111, 112, 113, 114, 115, 116 }));
		_dataArray.Add(new SkillBookItem(168, 336, 10, 1001, 2, 166, "icon_SkillBook_wudangtaihegong", 337, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 40, 10, 900, 1800, 1, 6, 1800, 5, allowRandomCreate: true, 35, isSpecial: false, 4, 36, -1, -1, -1, 0, 24, 240, new List<short> { 110, 111, 112, 113, 114, 115, 116 }));
		_dataArray.Add(new SkillBookItem(169, 338, 10, 1001, 3, 166, "icon_SkillBook_luotianzhenjueshierzhuang", 339, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 35, 10, 2250, 4500, 2, 8, 3000, 6, allowRandomCreate: true, 30, isSpecial: false, 4, 36, -1, -1, -1, 0, 25, 400, new List<short> { 111, 112, 113, 114, 115, 116 }));
		_dataArray.Add(new SkillBookItem(170, 340, 10, 1001, 4, 166, "icon_SkillBook_luotianzhenjueshierzhuang", 341, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 30, 10, 4650, 9300, 3, 10, 4200, 7, allowRandomCreate: true, 25, isSpecial: false, 4, 36, -1, -1, -1, 0, 26, 600, new List<short> { 112, 113, 114, 115, 116 }));
		_dataArray.Add(new SkillBookItem(171, 342, 10, 1001, 5, 166, "icon_SkillBook_luotianzhenjueshierzhuang", 343, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 25, 10, 8400, 16800, 4, 12, 5400, 7, allowRandomCreate: true, 20, isSpecial: false, 4, 36, -1, -1, -1, 0, 27, 850, new List<short> { 113, 114, 115, 116 }));
		_dataArray.Add(new SkillBookItem(172, 344, 10, 1001, 6, 166, "icon_SkillBook_taiyishengong", 345, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 20, 10, 13800, 27600, 5, 14, 7200, 8, allowRandomCreate: true, 15, isSpecial: false, 4, 36, -1, -1, -1, 0, 28, 1140, new List<short> { 114, 115, 116 }));
		_dataArray.Add(new SkillBookItem(173, 346, 10, 1001, 7, 166, "icon_SkillBook_chunyangwujigong", 347, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 15, 10, 21150, 42300, 6, 16, 9000, 8, allowRandomCreate: true, 10, isSpecial: false, 4, 36, -1, -1, -1, 0, 29, 1480, new List<short> { 115, 116 }));
		_dataArray.Add(new SkillBookItem(174, 348, 10, 1001, 0, 174, "icon_SkillBook_tuzhuonaqingfa", 349, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 50, 10, 150, 300, 0, 2, 600, 3, allowRandomCreate: true, 45, isSpecial: false, 4, 36, -1, -1, -1, 0, 30, 60, new List<short>
		{
			108, 109, 110, 111, 112, 113, 114, 115, 116, 117,
			118, 119, 120, 121, 122, 123, 124, 125
		}));
		_dataArray.Add(new SkillBookItem(175, 350, 10, 1001, 1, 174, "icon_SkillBook_tuzhuonaqingfa", 351, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 45, 10, 300, 600, 0, 4, 1200, 4, allowRandomCreate: true, 40, isSpecial: false, 4, 36, -1, -1, -1, 0, 31, 130, new List<short>
		{
			109, 110, 111, 112, 113, 114, 115, 116, 118, 119,
			120, 121, 122, 123, 124, 125
		}));
		_dataArray.Add(new SkillBookItem(176, 352, 10, 1001, 2, 174, "icon_SkillBook_tuzhuonaqingfa", 353, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 40, 10, 900, 1800, 1, 6, 1800, 5, allowRandomCreate: true, 35, isSpecial: false, 4, 36, -1, -1, -1, 0, 32, 240, new List<short>
		{
			110, 111, 112, 113, 114, 115, 116, 119, 120, 121,
			122, 123, 124, 125
		}));
		_dataArray.Add(new SkillBookItem(177, 354, 10, 1001, 3, 174, "icon_SkillBook_bunianxinjue", 355, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 35, 10, 2250, 4500, 2, 8, 3000, 6, allowRandomCreate: true, 30, isSpecial: false, 4, 36, -1, -1, -1, 0, 33, 400, new List<short>
		{
			111, 112, 113, 114, 115, 116, 120, 121, 122, 123,
			124, 125
		}));
		_dataArray.Add(new SkillBookItem(178, 356, 10, 1001, 4, 174, "icon_SkillBook_bunianxinjue", 357, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 30, 10, 4650, 9300, 3, 10, 4200, 7, allowRandomCreate: true, 25, isSpecial: false, 4, 36, -1, -1, -1, 0, 34, 600, new List<short> { 112, 113, 114, 115, 116, 121, 122, 123, 124, 125 }));
		_dataArray.Add(new SkillBookItem(179, 358, 10, 1001, 0, 179, "icon_SkillBook_fengkouguqifa", 359, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 50, 10, 150, 300, 0, 2, 600, 3, allowRandomCreate: true, 45, isSpecial: false, 4, 36, -1, -1, -1, 0, 35, 60, new List<short>()));
	}

	private void CreateItems3()
	{
		_dataArray.Add(new SkillBookItem(180, 360, 10, 1001, 1, 179, "icon_SkillBook_fengkouguqifa", 361, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 45, 10, 300, 600, 0, 4, 1200, 4, allowRandomCreate: true, 40, isSpecial: false, 4, 36, -1, -1, -1, 0, 36, 130, new List<short>()));
		_dataArray.Add(new SkillBookItem(181, 362, 10, 1001, 2, 179, "icon_SkillBook_fengkouguqifa", 363, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 40, 10, 900, 1800, 1, 6, 1800, 5, allowRandomCreate: true, 35, isSpecial: false, 4, 36, -1, -1, -1, 0, 37, 240, new List<short>()));
		_dataArray.Add(new SkillBookItem(182, 364, 10, 1001, 3, 179, "icon_SkillBook_guangmingshizijin", 365, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 35, 10, 2250, 4500, 2, 8, 3000, 6, allowRandomCreate: true, 30, isSpecial: false, 4, 36, -1, -1, -1, 0, 38, 400, new List<short>()));
		_dataArray.Add(new SkillBookItem(183, 366, 10, 1001, 0, 183, "icon_SkillBook_yangzhifa", 367, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 50, 10, 150, 300, 0, 2, 600, 3, allowRandomCreate: true, 45, isSpecial: false, 4, 36, -1, -1, -1, 0, 39, 60, new List<short>
		{
			36, 37, 38, 39, 40, 41, 42, 43, 44, 135,
			136, 137, 138, 139, 140, 141, 142, 143
		}));
		_dataArray.Add(new SkillBookItem(184, 368, 10, 1001, 1, 183, "icon_SkillBook_yangzhifa", 369, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 45, 10, 300, 600, 0, 4, 1200, 4, allowRandomCreate: true, 40, isSpecial: false, 4, 36, -1, -1, -1, 0, 40, 130, new List<short>
		{
			37, 38, 39, 40, 41, 42, 43, 44, 136, 137,
			138, 139, 140, 141, 142, 143
		}));
		_dataArray.Add(new SkillBookItem(185, 370, 10, 1001, 2, 183, "icon_SkillBook_yangzhifa", 371, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 40, 10, 900, 1800, 1, 6, 1800, 5, allowRandomCreate: true, 35, isSpecial: false, 4, 36, -1, -1, -1, 0, 41, 240, new List<short>
		{
			38, 39, 40, 41, 42, 43, 44, 137, 138, 139,
			140, 141, 142, 143
		}));
		_dataArray.Add(new SkillBookItem(186, 372, 10, 1001, 3, 183, "icon_SkillBook_sunyuefa", 373, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 35, 10, 2250, 4500, 2, 8, 3000, 6, allowRandomCreate: true, 30, isSpecial: false, 4, 36, -1, -1, -1, 0, 42, 400, new List<short>
		{
			39, 40, 41, 42, 43, 44, 138, 139, 140, 141,
			142, 143
		}));
		_dataArray.Add(new SkillBookItem(187, 374, 10, 1001, 4, 183, "icon_SkillBook_sunyuefa", 375, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 30, 10, 4650, 9300, 3, 10, 4200, 7, allowRandomCreate: true, 25, isSpecial: false, 4, 36, -1, -1, -1, 0, 43, 600, new List<short> { 40, 41, 42, 43, 44, 139, 140, 141, 142, 143 }));
		_dataArray.Add(new SkillBookItem(188, 376, 10, 1001, 5, 183, "icon_SkillBook_sunyuefa", 377, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 25, 10, 8400, 16800, 4, 12, 5400, 7, allowRandomCreate: true, 20, isSpecial: false, 4, 36, -1, -1, -1, 0, 44, 850, new List<short> { 41, 42, 43, 44, 140, 141, 142, 143 }));
		_dataArray.Add(new SkillBookItem(189, 378, 10, 1001, 6, 183, "icon_SkillBook_sanshifa", 379, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 20, 10, 13800, 27600, 5, 14, 7200, 8, allowRandomCreate: true, 15, isSpecial: false, 4, 36, -1, -1, -1, 0, 45, 1140, new List<short> { 42, 43, 44, 141, 142, 143 }));
		_dataArray.Add(new SkillBookItem(190, 380, 10, 1001, 7, 183, "icon_SkillBook_xuanweizhenshu", 381, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 15, 10, 21150, 42300, 6, 16, 9000, 8, allowRandomCreate: true, 10, isSpecial: false, 4, 36, -1, -1, -1, 0, 46, 1480, new List<short> { 43, 44, 142, 143 }));
		_dataArray.Add(new SkillBookItem(191, 382, 10, 1001, 0, 191, "icon_SkillBook_xuannvxinjue", 383, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 50, 10, 150, 300, 0, 2, 600, 3, allowRandomCreate: true, 45, isSpecial: false, 4, 36, -1, -1, -1, 0, 47, 60, new List<short>
		{
			108, 109, 110, 111, 112, 113, 114, 115, 116, 27,
			28, 29, 30, 31, 32, 33, 34, 35
		}));
		_dataArray.Add(new SkillBookItem(192, 384, 10, 1001, 1, 191, "icon_SkillBook_xuannvxinjue", 385, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 45, 10, 300, 600, 0, 4, 1200, 4, allowRandomCreate: true, 40, isSpecial: false, 4, 36, -1, -1, -1, 0, 48, 130, new List<short>
		{
			109, 110, 111, 112, 113, 114, 115, 116, 28, 29,
			30, 31, 32, 33, 34, 35
		}));
		_dataArray.Add(new SkillBookItem(193, 386, 10, 1001, 2, 191, "icon_SkillBook_xuannvxinjue", 387, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 40, 10, 900, 1800, 1, 6, 1800, 5, allowRandomCreate: true, 35, isSpecial: false, 4, 36, -1, -1, -1, 0, 49, 240, new List<short>
		{
			110, 111, 112, 113, 114, 115, 116, 29, 30, 31,
			32, 33, 34, 35
		}));
		_dataArray.Add(new SkillBookItem(194, 388, 10, 1001, 3, 191, "icon_SkillBook_shaoyinyimingjue", 389, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 35, 10, 2250, 4500, 2, 8, 3000, 6, allowRandomCreate: true, 30, isSpecial: false, 4, 36, -1, -1, -1, 0, 50, 400, new List<short>
		{
			111, 112, 113, 114, 115, 116, 30, 31, 32, 33,
			34, 35
		}));
		_dataArray.Add(new SkillBookItem(195, 390, 10, 1001, 4, 191, "icon_SkillBook_shaoyinyimingjue", 391, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 30, 10, 4650, 9300, 3, 10, 4200, 7, allowRandomCreate: true, 25, isSpecial: false, 4, 36, -1, -1, -1, 0, 51, 600, new List<short> { 112, 113, 114, 115, 116, 31, 32, 33, 34, 35 }));
		_dataArray.Add(new SkillBookItem(196, 392, 10, 1001, 5, 191, "icon_SkillBook_shaoyinyimingjue", 393, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 25, 10, 8400, 16800, 4, 12, 5400, 7, allowRandomCreate: true, 20, isSpecial: false, 4, 36, -1, -1, -1, 0, 52, 850, new List<short> { 113, 114, 115, 116, 32, 33, 34, 35 }));
		_dataArray.Add(new SkillBookItem(197, 394, 10, 1001, 6, 191, "icon_SkillBook_yaochixianyufa", 395, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 20, 10, 13800, 27600, 5, 14, 7200, 8, allowRandomCreate: true, 15, isSpecial: false, 4, 36, -1, -1, -1, 0, 53, 1140, new List<short> { 114, 115, 116, 33, 34, 35 }));
		_dataArray.Add(new SkillBookItem(198, 396, 10, 1001, 7, 191, "icon_SkillBook_taiyuanyunvgong", 397, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 15, 10, 21150, 42300, 6, 16, 9000, 8, allowRandomCreate: true, 10, isSpecial: false, 4, 36, -1, -1, -1, 0, 54, 1480, new List<short> { 115, 116, 34, 35 }));
		_dataArray.Add(new SkillBookItem(199, 398, 10, 1001, 8, 191, "icon_SkillBook_taiyinyimingjue", 399, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 10, 10, 30750, 61500, 6, 18, 10800, 8, allowRandomCreate: true, 5, isSpecial: false, 4, 36, -1, -1, -1, 0, 55, 1860, new List<short> { 116, 35 }));
		_dataArray.Add(new SkillBookItem(200, 400, 10, 1001, 0, 200, "icon_SkillBook_fandanchuiduanpian", 401, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 50, 10, 150, 300, 0, 2, 600, 3, allowRandomCreate: true, 45, isSpecial: false, 4, 36, -1, -1, -1, 0, 56, 60, new List<short>
		{
			54, 55, 56, 57, 58, 59, 60, 61, 62, 63,
			64, 65, 66, 67, 68, 69, 70, 71, 90, 91,
			92, 93, 94, 95, 96, 97, 98, 99, 100, 101,
			102, 103, 104, 105, 106, 107
		}));
		_dataArray.Add(new SkillBookItem(201, 402, 10, 1001, 1, 200, "icon_SkillBook_fandanchuiduanpian", 403, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 45, 10, 300, 600, 0, 4, 1200, 4, allowRandomCreate: true, 40, isSpecial: false, 4, 36, -1, -1, -1, 0, 57, 130, new List<short>
		{
			55, 56, 57, 58, 59, 60, 61, 62, 64, 65,
			66, 67, 68, 69, 70, 71, 91, 92, 93, 94,
			95, 96, 97, 98, 100, 101, 102, 103, 104, 105,
			106, 107
		}));
		_dataArray.Add(new SkillBookItem(202, 404, 10, 1001, 2, 200, "icon_SkillBook_fandanchuiduanpian", 405, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 40, 10, 900, 1800, 1, 6, 1800, 5, allowRandomCreate: true, 35, isSpecial: false, 4, 36, -1, -1, -1, 0, 58, 240, new List<short>
		{
			56, 57, 58, 59, 60, 61, 62, 65, 66, 67,
			68, 69, 70, 71, 92, 93, 94, 95, 96, 97,
			98, 101, 102, 103, 104, 105, 106, 107
		}));
		_dataArray.Add(new SkillBookItem(203, 406, 10, 1001, 3, 200, "icon_SkillBook_qianchuibailianpian", 407, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 35, 10, 2250, 4500, 2, 8, 3000, 6, allowRandomCreate: true, 30, isSpecial: false, 4, 36, -1, -1, -1, 0, 59, 400, new List<short>
		{
			57, 58, 59, 60, 61, 62, 66, 67, 68, 69,
			70, 71, 93, 94, 95, 96, 97, 98, 102, 103,
			104, 105, 106, 107
		}));
		_dataArray.Add(new SkillBookItem(204, 408, 10, 1001, 4, 200, "icon_SkillBook_qianchuibailianpian", 409, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 30, 10, 4650, 9300, 3, 10, 4200, 7, allowRandomCreate: true, 25, isSpecial: false, 4, 36, -1, -1, -1, 0, 60, 600, new List<short>
		{
			58, 59, 60, 61, 62, 67, 68, 69, 70, 71,
			94, 95, 96, 97, 98, 103, 104, 105, 106, 107
		}));
		_dataArray.Add(new SkillBookItem(205, 410, 10, 1001, 0, 205, "icon_SkillBook_qihuangyaolüe", 411, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 50, 10, 150, 300, 0, 2, 600, 3, allowRandomCreate: true, 45, isSpecial: false, 4, 36, -1, -1, -1, 0, 61, 60, new List<short>
		{
			72, 73, 74, 75, 76, 77, 78, 79, 80, 81,
			82, 83, 84, 85, 86, 87, 88, 89
		}));
		_dataArray.Add(new SkillBookItem(206, 412, 10, 1001, 1, 205, "icon_SkillBook_qihuangyaolüe", 413, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 45, 10, 300, 600, 0, 4, 1200, 4, allowRandomCreate: true, 40, isSpecial: false, 4, 36, -1, -1, -1, 0, 62, 130, new List<short>
		{
			73, 74, 75, 76, 77, 78, 79, 80, 82, 83,
			84, 85, 86, 87, 88, 89
		}));
		_dataArray.Add(new SkillBookItem(207, 414, 10, 1001, 2, 205, "icon_SkillBook_qihuangyaolüe", 415, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 40, 10, 900, 1800, 1, 6, 1800, 5, allowRandomCreate: true, 35, isSpecial: false, 4, 36, -1, -1, -1, 0, 63, 240, new List<short>
		{
			74, 75, 76, 77, 78, 79, 80, 83, 84, 85,
			86, 87, 88, 89
		}));
		_dataArray.Add(new SkillBookItem(208, 416, 10, 1001, 3, 205, "icon_SkillBook_jiaozhesifa", 417, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 35, 10, 2250, 4500, 2, 8, 3000, 6, allowRandomCreate: true, 30, isSpecial: false, 4, 36, -1, -1, -1, 0, 64, 400, new List<short>
		{
			75, 76, 77, 78, 79, 80, 84, 85, 86, 87,
			88, 89
		}));
		_dataArray.Add(new SkillBookItem(209, 418, 10, 1001, 4, 205, "icon_SkillBook_jiaozhesifa", 419, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 30, 10, 4650, 9300, 3, 10, 4200, 7, allowRandomCreate: true, 25, isSpecial: false, 4, 36, -1, -1, -1, 0, 65, 600, new List<short> { 76, 77, 78, 79, 80, 85, 86, 87, 88, 89 }));
		_dataArray.Add(new SkillBookItem(210, 420, 10, 1001, 0, 210, "icon_SkillBook_jingangsijiaxingfa", 421, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 50, 10, 150, 300, 0, 2, 600, 3, allowRandomCreate: true, 45, isSpecial: false, 4, 36, -1, -1, -1, 0, 66, 60, new List<short> { 117, 118, 119, 120, 121, 122, 123, 124, 125 }));
		_dataArray.Add(new SkillBookItem(211, 422, 10, 1001, 1, 210, "icon_SkillBook_jingangsijiaxingfa", 423, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 45, 10, 300, 600, 0, 4, 1200, 4, allowRandomCreate: true, 40, isSpecial: false, 4, 36, -1, -1, -1, 0, 67, 130, new List<short> { 118, 119, 120, 121, 122, 123, 124, 125 }));
		_dataArray.Add(new SkillBookItem(212, 424, 10, 1001, 2, 210, "icon_SkillBook_jingangsijiaxingfa", 425, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 40, 10, 900, 1800, 1, 6, 1800, 5, allowRandomCreate: true, 35, isSpecial: false, 4, 36, -1, -1, -1, 0, 68, 240, new List<short> { 119, 120, 121, 122, 123, 124, 125 }));
		_dataArray.Add(new SkillBookItem(213, 426, 10, 1001, 3, 210, "icon_SkillBook_baopingqigongfa", 427, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 35, 10, 2250, 4500, 2, 8, 3000, 6, allowRandomCreate: true, 30, isSpecial: false, 4, 36, -1, -1, -1, 0, 69, 400, new List<short> { 120, 121, 122, 123, 124, 125 }));
		_dataArray.Add(new SkillBookItem(214, 428, 10, 1001, 4, 210, "icon_SkillBook_baopingqigongfa", 429, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 30, 10, 4650, 9300, 3, 10, 4200, 7, allowRandomCreate: true, 25, isSpecial: false, 4, 36, -1, -1, -1, 0, 70, 600, new List<short> { 121, 122, 123, 124, 125 }));
		_dataArray.Add(new SkillBookItem(215, 430, 10, 1001, 5, 210, "icon_SkillBook_baopingqigongfa", 431, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 25, 10, 8400, 16800, 4, 12, 5400, 7, allowRandomCreate: true, 20, isSpecial: false, 4, 36, -1, -1, -1, 0, 71, 850, new List<short> { 122, 123, 124, 125 }));
		_dataArray.Add(new SkillBookItem(216, 432, 10, 1001, 6, 210, "icon_SkillBook_jingangdingjing", 433, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 20, 10, 13800, 27600, 5, 14, 7200, 8, allowRandomCreate: true, 15, isSpecial: false, 4, 36, -1, -1, -1, 0, 72, 1140, new List<short> { 123, 124, 125 }));
		_dataArray.Add(new SkillBookItem(217, 434, 10, 1001, 7, 210, "icon_SkillBook_mohejialuohufagong", 435, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 15, 10, 21150, 42300, 6, 16, 9000, 8, allowRandomCreate: true, 10, isSpecial: false, 4, 36, -1, -1, -1, 0, 73, 1480, new List<short> { 124, 125 }));
		_dataArray.Add(new SkillBookItem(218, 436, 10, 1001, 8, 210, "icon_SkillBook_wushangyujiafa", 437, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 10, 10, 30750, 61500, 6, 18, 10800, 8, allowRandomCreate: true, 5, isSpecial: false, 4, 36, -1, -1, -1, 0, 74, 1860, new List<short> { 125 }));
		_dataArray.Add(new SkillBookItem(219, 438, 10, 1001, 0, 219, "icon_SkillBook_yindushu", 439, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 50, 10, 150, 300, 0, 2, 600, 3, allowRandomCreate: true, 45, isSpecial: false, 4, 36, -1, -1, -1, 0, 75, 60, new List<short> { 81, 82, 83, 84, 85, 86, 87, 88, 89 }));
		_dataArray.Add(new SkillBookItem(220, 440, 10, 1001, 1, 219, "icon_SkillBook_yindushu", 441, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 45, 10, 300, 600, 0, 4, 1200, 4, allowRandomCreate: true, 40, isSpecial: false, 4, 36, -1, -1, -1, 0, 76, 130, new List<short> { 82, 83, 84, 85, 86, 87, 88, 89 }));
		_dataArray.Add(new SkillBookItem(221, 442, 10, 1001, 2, 219, "icon_SkillBook_yindushu", 443, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 40, 10, 900, 1800, 1, 6, 1800, 5, allowRandomCreate: true, 35, isSpecial: false, 4, 36, -1, -1, -1, 0, 77, 240, new List<short> { 83, 84, 85, 86, 87, 88, 89 }));
		_dataArray.Add(new SkillBookItem(222, 444, 10, 1001, 3, 219, "icon_SkillBook_heixianfa", 445, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 35, 10, 2250, 4500, 2, 8, 3000, 6, allowRandomCreate: true, 30, isSpecial: false, 4, 36, -1, -1, -1, 0, 78, 400, new List<short> { 84, 85, 86, 87, 88, 89 }));
		_dataArray.Add(new SkillBookItem(223, 446, 10, 1001, 4, 219, "icon_SkillBook_heixianfa", 447, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 30, 10, 4650, 9300, 3, 10, 4200, 7, allowRandomCreate: true, 25, isSpecial: false, 4, 36, -1, -1, -1, 0, 79, 600, new List<short> { 85, 86, 87, 88, 89 }));
		_dataArray.Add(new SkillBookItem(224, 448, 10, 1001, 5, 219, "icon_SkillBook_heixianfa", 449, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 25, 10, 8400, 16800, 4, 12, 5400, 7, allowRandomCreate: true, 20, isSpecial: false, 4, 36, -1, -1, -1, 0, 80, 850, new List<short> { 86, 87, 88, 89 }));
		_dataArray.Add(new SkillBookItem(225, 450, 10, 1001, 6, 219, "icon_SkillBook_baixietidafa", 451, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 20, 10, 13800, 27600, 5, 14, 7200, 8, allowRandomCreate: true, 15, isSpecial: false, 4, 36, -1, -1, -1, 0, 81, 1140, new List<short> { 87, 88, 89 }));
		_dataArray.Add(new SkillBookItem(226, 452, 10, 1001, 0, 226, "icon_SkillBook_jieqingsixiangtu", 453, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 50, 10, 150, 300, 0, 2, 600, 3, allowRandomCreate: true, 45, isSpecial: false, 4, 36, -1, -1, -1, 0, 82, 60, new List<short> { 36, 37, 38, 39, 40, 41, 42, 43, 44 }));
		_dataArray.Add(new SkillBookItem(227, 454, 10, 1001, 1, 226, "icon_SkillBook_jieqingsixiangtu", 455, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 45, 10, 300, 600, 0, 4, 1200, 4, allowRandomCreate: true, 40, isSpecial: false, 4, 36, -1, -1, -1, 0, 83, 130, new List<short> { 37, 38, 39, 40, 41, 42, 43, 44 }));
		_dataArray.Add(new SkillBookItem(228, 456, 10, 1001, 2, 226, "icon_SkillBook_jieqingsixiangtu", 457, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 40, 10, 900, 1800, 1, 6, 1800, 5, allowRandomCreate: true, 35, isSpecial: false, 4, 36, -1, -1, -1, 0, 84, 240, new List<short> { 38, 39, 40, 41, 42, 43, 44 }));
		_dataArray.Add(new SkillBookItem(229, 458, 10, 1001, 3, 226, "icon_SkillBook_butiange", 459, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 35, 10, 2250, 4500, 2, 8, 3000, 6, allowRandomCreate: true, 30, isSpecial: false, 4, 36, -1, -1, -1, 0, 85, 400, new List<short> { 39, 40, 41, 42, 43, 44 }));
		_dataArray.Add(new SkillBookItem(230, 460, 10, 1001, 4, 226, "icon_SkillBook_butiange", 461, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 30, 10, 4650, 9300, 3, 10, 4200, 7, allowRandomCreate: true, 25, isSpecial: false, 4, 36, -1, -1, -1, 0, 86, 600, new List<short> { 40, 41, 42, 43, 44 }));
		_dataArray.Add(new SkillBookItem(231, 462, 10, 1001, 5, 226, "icon_SkillBook_butiange", 463, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 25, 10, 8400, 16800, 4, 12, 5400, 7, allowRandomCreate: true, 20, isSpecial: false, 4, 36, -1, -1, -1, 0, 87, 850, new List<short> { 41, 42, 43, 44 }));
		_dataArray.Add(new SkillBookItem(232, 464, 10, 1001, 6, 226, "icon_SkillBook_huntianxingtu", 465, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 20, 10, 13800, 27600, 5, 14, 7200, 8, allowRandomCreate: true, 15, isSpecial: false, 4, 36, -1, -1, -1, 0, 88, 1140, new List<short> { 42, 43, 44 }));
		_dataArray.Add(new SkillBookItem(233, 466, 10, 1001, 0, 233, "icon_SkillBook_biantihuoqifa", 467, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 50, 10, 150, 300, 0, 2, 600, 3, allowRandomCreate: true, 45, isSpecial: false, 4, 36, -1, -1, -1, 0, 89, 60, new List<short>
		{
			126, 127, 128, 129, 130, 131, 132, 133, 134, 45,
			46, 47, 48, 49, 50, 51, 52, 53
		}));
		_dataArray.Add(new SkillBookItem(234, 468, 10, 1001, 1, 233, "icon_SkillBook_biantihuoqifa", 469, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 45, 10, 300, 600, 0, 4, 1200, 4, allowRandomCreate: true, 40, isSpecial: false, 4, 36, -1, -1, -1, 0, 90, 130, new List<short>
		{
			127, 128, 129, 130, 131, 132, 133, 134, 46, 47,
			48, 49, 50, 51, 52, 53
		}));
		_dataArray.Add(new SkillBookItem(235, 470, 10, 1001, 2, 233, "icon_SkillBook_biantihuoqifa", 471, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 40, 10, 900, 1800, 1, 6, 1800, 5, allowRandomCreate: true, 35, isSpecial: false, 4, 36, -1, -1, -1, 0, 91, 240, new List<short>
		{
			128, 129, 130, 131, 132, 133, 134, 47, 48, 49,
			50, 51, 52, 53
		}));
		_dataArray.Add(new SkillBookItem(236, 472, 10, 1001, 3, 233, "icon_SkillBook_xiaoyuyangshengong", 473, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 35, 10, 2250, 4500, 2, 8, 3000, 6, allowRandomCreate: true, 30, isSpecial: false, 4, 36, -1, -1, -1, 0, 92, 400, new List<short>
		{
			129, 130, 131, 132, 133, 134, 48, 49, 50, 51,
			52, 53
		}));
		_dataArray.Add(new SkillBookItem(237, 474, 10, 1001, 4, 233, "icon_SkillBook_xiaoyuyangshengong", 475, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 30, 10, 4650, 9300, 3, 10, 4200, 7, allowRandomCreate: true, 25, isSpecial: false, 4, 36, -1, -1, -1, 0, 93, 600, new List<short> { 130, 131, 132, 133, 134, 49, 50, 51, 52, 53 }));
		_dataArray.Add(new SkillBookItem(238, 476, 10, 1001, 5, 233, "icon_SkillBook_xiaoyuyangshengong", 477, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 25, 10, 8400, 16800, 4, 12, 5400, 7, allowRandomCreate: true, 20, isSpecial: false, 4, 36, -1, -1, -1, 0, 94, 850, new List<short> { 131, 132, 133, 134, 50, 51, 52, 53 }));
		_dataArray.Add(new SkillBookItem(239, 478, 10, 1001, 6, 233, "icon_SkillBook_rumogong", 479, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 20, 10, 13800, 27600, 5, 14, 7200, 8, allowRandomCreate: true, 15, isSpecial: false, 4, 36, -1, -1, -1, 0, 95, 1140, new List<short> { 132, 133, 134, 51, 52, 53 }));
	}

	private void CreateItems4()
	{
		_dataArray.Add(new SkillBookItem(240, 480, 10, 1001, 7, 233, "icon_SkillBook_dayuyangshengong", 481, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 15, 10, 21150, 42300, 6, 16, 9000, 8, allowRandomCreate: true, 10, isSpecial: false, 4, 36, -1, -1, -1, 0, 96, 1480, new List<short> { 133, 134, 52, 53 }));
		_dataArray.Add(new SkillBookItem(241, 482, 10, 1001, 0, 241, "icon_SkillBook_houmugong", 483, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 50, 10, 150, 300, 0, 2, 600, 3, allowRandomCreate: true, 45, isSpecial: false, 4, 36, -1, -1, -1, 0, 97, 60, new List<short>
		{
			81, 82, 83, 84, 85, 86, 87, 88, 89, 135,
			136, 137, 138, 139, 140, 141, 142, 143
		}));
		_dataArray.Add(new SkillBookItem(242, 484, 10, 1001, 1, 241, "icon_SkillBook_houmugong", 485, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 45, 10, 300, 600, 0, 4, 1200, 4, allowRandomCreate: true, 40, isSpecial: false, 4, 36, -1, -1, -1, 0, 98, 130, new List<short>
		{
			82, 83, 84, 85, 86, 87, 88, 89, 136, 137,
			138, 139, 140, 141, 142, 143
		}));
		_dataArray.Add(new SkillBookItem(243, 486, 10, 1001, 2, 241, "icon_SkillBook_houmugong", 487, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 40, 10, 900, 1800, 1, 6, 1800, 5, allowRandomCreate: true, 35, isSpecial: false, 4, 36, -1, -1, -1, 0, 99, 240, new List<short>
		{
			83, 84, 85, 86, 87, 88, 89, 137, 138, 139,
			140, 141, 142, 143
		}));
		_dataArray.Add(new SkillBookItem(244, 488, 10, 1001, 3, 241, "icon_SkillBook_sanduwumingzhou", 489, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 35, 10, 2250, 4500, 2, 8, 3000, 6, allowRandomCreate: true, 30, isSpecial: false, 4, 36, -1, -1, -1, 0, 100, 400, new List<short>
		{
			84, 85, 86, 87, 88, 89, 138, 139, 140, 141,
			142, 143
		}));
		_dataArray.Add(new SkillBookItem(245, 490, 10, 1001, 4, 241, "icon_SkillBook_sanduwumingzhou", 491, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 30, 10, 4650, 9300, 3, 10, 4200, 7, allowRandomCreate: true, 25, isSpecial: false, 4, 36, -1, -1, -1, 0, 101, 600, new List<short> { 85, 86, 87, 88, 89, 139, 140, 141, 142, 143 }));
		_dataArray.Add(new SkillBookItem(246, 492, 10, 1001, 5, 241, "icon_SkillBook_sanduwumingzhou", 493, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 25, 10, 8400, 16800, 4, 12, 5400, 7, allowRandomCreate: true, 20, isSpecial: false, 4, 36, -1, -1, -1, 0, 102, 850, new List<short> { 86, 87, 88, 89, 140, 141, 142, 143 }));
		_dataArray.Add(new SkillBookItem(247, 494, 10, 1001, 6, 241, "icon_SkillBook_tongzixielianfa", 495, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 20, 10, 13800, 27600, 5, 14, 7200, 8, allowRandomCreate: true, 15, isSpecial: false, 4, 36, -1, -1, -1, 0, 103, 1140, new List<short> { 87, 88, 89, 141, 142, 143 }));
		_dataArray.Add(new SkillBookItem(248, 496, 10, 1001, 7, 241, "icon_SkillBook_xiehouxiejing", 497, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 15, 10, 21150, 42300, 6, 16, 9000, 8, allowRandomCreate: true, 10, isSpecial: false, 4, 36, -1, -1, -1, 0, 104, 1480, new List<short> { 88, 89, 142, 143 }));
		_dataArray.Add(new SkillBookItem(249, 498, 10, 1001, 8, 241, "icon_SkillBook_xietongdafa", 499, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 10, 10, 30750, 61500, 6, 18, 10800, 8, allowRandomCreate: true, 5, isSpecial: false, 4, 36, -1, -1, -1, 0, 105, 1860, new List<short> { 89, 143 }));
		_dataArray.Add(new SkillBookItem(250, 500, 10, 1001, 0, 250, "icon_SkillBook_chuanzonggong", 501, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 50, 10, 150, 300, 0, 2, 600, 3, allowRandomCreate: true, 45, isSpecial: false, 4, 36, -1, -1, -1, 1, 106, 60, new List<short> { 117, 118, 119, 120, 121, 122, 123, 124, 125 }));
		_dataArray.Add(new SkillBookItem(251, 502, 10, 1001, 1, 250, "icon_SkillBook_chuanzonggong", 503, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 45, 10, 300, 600, 0, 4, 1200, 4, allowRandomCreate: true, 40, isSpecial: false, 4, 36, -1, -1, -1, 1, 107, 130, new List<short> { 118, 119, 120, 121, 122, 123, 124, 125 }));
		_dataArray.Add(new SkillBookItem(252, 504, 10, 1001, 2, 250, "icon_SkillBook_chuanzonggong", 505, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 40, 10, 900, 1800, 1, 6, 1800, 5, allowRandomCreate: true, 35, isSpecial: false, 4, 36, -1, -1, -1, 1, 108, 240, new List<short> { 119, 120, 121, 122, 123, 124, 125 }));
		_dataArray.Add(new SkillBookItem(253, 506, 10, 1001, 3, 250, "icon_SkillBook_dafantengshu", 507, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 35, 10, 2250, 4500, 2, 8, 3000, 6, allowRandomCreate: true, 30, isSpecial: false, 4, 36, -1, -1, -1, 1, 109, 400, new List<short> { 120, 121, 122, 123, 124, 125 }));
		_dataArray.Add(new SkillBookItem(254, 508, 10, 1001, 4, 250, "icon_SkillBook_dafantengshu", 509, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 30, 10, 4650, 9300, 3, 10, 4200, 7, allowRandomCreate: true, 25, isSpecial: false, 4, 36, -1, -1, -1, 1, 110, 600, new List<short> { 121, 122, 123, 124, 125 }));
		_dataArray.Add(new SkillBookItem(255, 510, 10, 1001, 5, 250, "icon_SkillBook_dafantengshu", 511, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 25, 10, 8400, 16800, 4, 12, 5400, 7, allowRandomCreate: true, 20, isSpecial: false, 4, 36, -1, -1, -1, 1, 111, 850, new List<short> { 122, 123, 124, 125 }));
		_dataArray.Add(new SkillBookItem(256, 512, 10, 1001, 0, 256, "icon_SkillBook_dantuisuodi", 513, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 50, 10, 150, 300, 0, 2, 600, 3, allowRandomCreate: true, 45, isSpecial: false, 4, 36, -1, -1, -1, 1, 112, 60, new List<short>
		{
			108, 109, 110, 111, 112, 113, 114, 115, 116, 117,
			118, 119, 120, 121, 122, 123, 124, 125
		}));
		_dataArray.Add(new SkillBookItem(257, 514, 10, 1001, 1, 256, "icon_SkillBook_dantuisuodi", 515, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 45, 10, 300, 600, 0, 4, 1200, 4, allowRandomCreate: true, 40, isSpecial: false, 4, 36, -1, -1, -1, 1, 113, 130, new List<short>
		{
			109, 110, 111, 112, 113, 114, 115, 116, 118, 119,
			120, 121, 122, 123, 124, 125
		}));
		_dataArray.Add(new SkillBookItem(258, 516, 10, 1001, 2, 256, "icon_SkillBook_dantuisuodi", 517, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 40, 10, 900, 1800, 1, 6, 1800, 5, allowRandomCreate: true, 35, isSpecial: false, 4, 36, -1, -1, -1, 1, 114, 240, new List<short>
		{
			110, 111, 112, 113, 114, 115, 116, 119, 120, 121,
			122, 123, 124, 125
		}));
		_dataArray.Add(new SkillBookItem(259, 518, 10, 1001, 3, 256, "icon_SkillBook_yunvnuolian", 519, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 35, 10, 2250, 4500, 2, 8, 3000, 6, allowRandomCreate: true, 30, isSpecial: false, 4, 36, -1, -1, -1, 1, 115, 400, new List<short>
		{
			111, 112, 113, 114, 115, 116, 120, 121, 122, 123,
			124, 125
		}));
		_dataArray.Add(new SkillBookItem(260, 520, 10, 1001, 4, 256, "icon_SkillBook_yunvnuolian", 521, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 30, 10, 4650, 9300, 3, 10, 4200, 7, allowRandomCreate: true, 25, isSpecial: false, 4, 36, -1, -1, -1, 1, 116, 600, new List<short> { 112, 113, 114, 115, 116, 121, 122, 123, 124, 125 }));
		_dataArray.Add(new SkillBookItem(261, 522, 10, 1001, 5, 256, "icon_SkillBook_yunvnuolian", 523, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 25, 10, 8400, 16800, 4, 12, 5400, 7, allowRandomCreate: true, 20, isSpecial: false, 4, 36, -1, -1, -1, 1, 117, 850, new List<short> { 113, 114, 115, 116, 122, 123, 124, 125 }));
		_dataArray.Add(new SkillBookItem(262, 524, 10, 1001, 6, 256, "icon_SkillBook_jindingfeixian", 525, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 20, 10, 13800, 27600, 5, 14, 7200, 8, allowRandomCreate: true, 15, isSpecial: false, 4, 36, -1, -1, -1, 1, 118, 1140, new List<short> { 114, 115, 116, 123, 124, 125 }));
		_dataArray.Add(new SkillBookItem(263, 526, 10, 1001, 0, 263, "icon_SkillBook_dizhenfa", 527, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 50, 10, 150, 300, 0, 2, 600, 3, allowRandomCreate: true, 45, isSpecial: false, 4, 36, -1, -1, -1, 1, 119, 60, new List<short> { 72, 73, 74, 75, 76, 77, 78, 79, 80 }));
		_dataArray.Add(new SkillBookItem(264, 528, 10, 1001, 1, 263, "icon_SkillBook_dizhenfa", 529, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 45, 10, 300, 600, 0, 4, 1200, 4, allowRandomCreate: true, 40, isSpecial: false, 4, 36, -1, -1, -1, 1, 120, 130, new List<short> { 73, 74, 75, 76, 77, 78, 79, 80 }));
		_dataArray.Add(new SkillBookItem(265, 530, 10, 1001, 2, 263, "icon_SkillBook_dizhenfa", 531, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 40, 10, 900, 1800, 1, 6, 1800, 5, allowRandomCreate: true, 35, isSpecial: false, 4, 36, -1, -1, -1, 1, 121, 240, new List<short> { 74, 75, 76, 77, 78, 79, 80 }));
		_dataArray.Add(new SkillBookItem(266, 532, 10, 1001, 3, 263, "icon_SkillBook_yuzhenhang", 533, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 35, 10, 2250, 4500, 2, 8, 3000, 6, allowRandomCreate: true, 30, isSpecial: false, 4, 36, -1, -1, -1, 1, 122, 400, new List<short> { 75, 76, 77, 78, 79, 80 }));
		_dataArray.Add(new SkillBookItem(267, 534, 10, 1001, 4, 263, "icon_SkillBook_yuzhenhang", 535, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 30, 10, 4650, 9300, 3, 10, 4200, 7, allowRandomCreate: true, 25, isSpecial: false, 4, 36, -1, -1, -1, 1, 123, 600, new List<short> { 76, 77, 78, 79, 80 }));
		_dataArray.Add(new SkillBookItem(268, 536, 10, 1001, 5, 263, "icon_SkillBook_yuzhenhang", 537, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 25, 10, 8400, 16800, 4, 12, 5400, 7, allowRandomCreate: true, 20, isSpecial: false, 4, 36, -1, -1, -1, 1, 124, 850, new List<short> { 77, 78, 79, 80 }));
		_dataArray.Add(new SkillBookItem(269, 538, 10, 1001, 6, 263, "icon_SkillBook_zhilanyubu", 539, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 20, 10, 13800, 27600, 5, 14, 7200, 8, allowRandomCreate: true, 15, isSpecial: false, 4, 36, -1, -1, -1, 1, 125, 1140, new List<short> { 78, 79, 80 }));
		_dataArray.Add(new SkillBookItem(270, 540, 10, 1001, 7, 263, "icon_SkillBook_wanhuatingyushi", 541, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 15, 10, 21150, 42300, 6, 16, 9000, 8, allowRandomCreate: true, 10, isSpecial: false, 4, 36, -1, -1, -1, 1, 126, 1480, new List<short> { 79, 80 }));
		_dataArray.Add(new SkillBookItem(271, 542, 10, 1001, 0, 271, "icon_SkillBook_wubumeihuazhuang", 543, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 50, 10, 150, 300, 0, 2, 600, 3, allowRandomCreate: true, 45, isSpecial: false, 4, 36, -1, -1, -1, 1, 127, 60, new List<short> { 108, 109, 110, 111, 112, 113, 114, 115, 116 }));
		_dataArray.Add(new SkillBookItem(272, 544, 10, 1001, 1, 271, "icon_SkillBook_wubumeihuazhuang", 545, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 45, 10, 300, 600, 0, 4, 1200, 4, allowRandomCreate: true, 40, isSpecial: false, 4, 36, -1, -1, -1, 1, 128, 130, new List<short> { 109, 110, 111, 112, 113, 114, 115, 116 }));
		_dataArray.Add(new SkillBookItem(273, 546, 10, 1001, 2, 271, "icon_SkillBook_wubumeihuazhuang", 547, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 40, 10, 900, 1800, 1, 6, 1800, 5, allowRandomCreate: true, 35, isSpecial: false, 4, 36, -1, -1, -1, 1, 129, 240, new List<short> { 110, 111, 112, 113, 114, 115, 116 }));
		_dataArray.Add(new SkillBookItem(274, 548, 10, 1001, 3, 271, "icon_SkillBook_tiangangbeidoubu", 549, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 35, 10, 2250, 4500, 2, 8, 3000, 6, allowRandomCreate: true, 30, isSpecial: false, 4, 36, -1, -1, -1, 1, 130, 400, new List<short> { 111, 112, 113, 114, 115, 116 }));
		_dataArray.Add(new SkillBookItem(275, 550, 10, 1001, 4, 271, "icon_SkillBook_tiangangbeidoubu", 551, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 30, 10, 4650, 9300, 3, 10, 4200, 7, allowRandomCreate: true, 25, isSpecial: false, 4, 36, -1, -1, -1, 1, 131, 600, new List<short> { 112, 113, 114, 115, 116 }));
		_dataArray.Add(new SkillBookItem(276, 552, 10, 1001, 5, 271, "icon_SkillBook_tiangangbeidoubu", 553, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 25, 10, 8400, 16800, 4, 12, 5400, 7, allowRandomCreate: true, 20, isSpecial: false, 4, 36, -1, -1, -1, 1, 132, 850, new List<short> { 113, 114, 115, 116 }));
		_dataArray.Add(new SkillBookItem(277, 554, 10, 1001, 6, 271, "icon_SkillBook_lingyunfeidu", 555, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 20, 10, 13800, 27600, 5, 14, 7200, 8, allowRandomCreate: true, 15, isSpecial: false, 4, 36, -1, -1, -1, 1, 133, 1140, new List<short> { 114, 115, 116 }));
		_dataArray.Add(new SkillBookItem(278, 556, 10, 1001, 0, 278, "icon_SkillBook_danzhuanggong", 557, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 50, 10, 150, 300, 0, 2, 600, 3, allowRandomCreate: true, 45, isSpecial: false, 4, 36, -1, -1, -1, 1, 134, 60, new List<short>
		{
			108, 109, 110, 111, 112, 113, 114, 115, 116, 117,
			118, 119, 120, 121, 122, 123, 124, 125
		}));
		_dataArray.Add(new SkillBookItem(279, 558, 10, 1001, 1, 278, "icon_SkillBook_danzhuanggong", 559, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 45, 10, 300, 600, 0, 4, 1200, 4, allowRandomCreate: true, 40, isSpecial: false, 4, 36, -1, -1, -1, 1, 135, 130, new List<short>
		{
			109, 110, 111, 112, 113, 114, 115, 116, 118, 119,
			120, 121, 122, 123, 124, 125
		}));
		_dataArray.Add(new SkillBookItem(280, 560, 10, 1001, 2, 278, "icon_SkillBook_danzhuanggong", 561, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 40, 10, 900, 1800, 1, 6, 1800, 5, allowRandomCreate: true, 35, isSpecial: false, 4, 36, -1, -1, -1, 1, 136, 240, new List<short>
		{
			110, 111, 112, 113, 114, 115, 116, 119, 120, 121,
			122, 123, 124, 125
		}));
		_dataArray.Add(new SkillBookItem(281, 562, 10, 1001, 3, 278, "icon_SkillBook_xuanjiading", 563, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 35, 10, 2250, 4500, 2, 8, 3000, 6, allowRandomCreate: true, 30, isSpecial: false, 4, 36, -1, -1, -1, 1, 137, 400, new List<short>
		{
			111, 112, 113, 114, 115, 116, 120, 121, 122, 123,
			124, 125
		}));
		_dataArray.Add(new SkillBookItem(282, 564, 10, 1001, 0, 282, "icon_SkillBook_zhenshanbu", 565, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 50, 10, 150, 300, 0, 2, 600, 3, allowRandomCreate: true, 45, isSpecial: false, 4, 36, -1, -1, -1, 1, 138, 60, new List<short>()));
		_dataArray.Add(new SkillBookItem(283, 566, 10, 1001, 1, 282, "icon_SkillBook_zhenshanbu", 567, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 45, 10, 300, 600, 0, 4, 1200, 4, allowRandomCreate: true, 40, isSpecial: false, 4, 36, -1, -1, -1, 1, 139, 130, new List<short>()));
		_dataArray.Add(new SkillBookItem(284, 568, 10, 1001, 2, 282, "icon_SkillBook_zhenshanbu", 569, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 40, 10, 900, 1800, 1, 6, 1800, 5, allowRandomCreate: true, 35, isSpecial: false, 4, 36, -1, -1, -1, 1, 140, 240, new List<short>()));
		_dataArray.Add(new SkillBookItem(285, 570, 10, 1001, 3, 282, "icon_SkillBook_shizifenxun", 571, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 35, 10, 2250, 4500, 2, 8, 3000, 6, allowRandomCreate: true, 30, isSpecial: false, 4, 36, -1, -1, -1, 1, 141, 400, new List<short>()));
		_dataArray.Add(new SkillBookItem(286, 572, 10, 1001, 0, 286, "icon_SkillBook_wuguibu", 573, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 50, 10, 150, 300, 0, 2, 600, 3, allowRandomCreate: true, 45, isSpecial: false, 4, 36, -1, -1, -1, 1, 142, 60, new List<short>
		{
			36, 37, 38, 39, 40, 41, 42, 43, 44, 135,
			136, 137, 138, 139, 140, 141, 142, 143
		}));
		_dataArray.Add(new SkillBookItem(287, 574, 10, 1001, 1, 286, "icon_SkillBook_wuguibu", 575, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 45, 10, 300, 600, 0, 4, 1200, 4, allowRandomCreate: true, 40, isSpecial: false, 4, 36, -1, -1, -1, 1, 143, 130, new List<short>
		{
			37, 38, 39, 40, 41, 42, 43, 44, 136, 137,
			138, 139, 140, 141, 142, 143
		}));
		_dataArray.Add(new SkillBookItem(288, 576, 10, 1001, 2, 286, "icon_SkillBook_wuguibu", 577, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 40, 10, 900, 1800, 1, 6, 1800, 5, allowRandomCreate: true, 35, isSpecial: false, 4, 36, -1, -1, -1, 1, 144, 240, new List<short>
		{
			38, 39, 40, 41, 42, 43, 44, 137, 138, 139,
			140, 141, 142, 143
		}));
		_dataArray.Add(new SkillBookItem(289, 578, 10, 1001, 3, 286, "icon_SkillBook_xingyunqiwufa", 579, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 35, 10, 2250, 4500, 2, 8, 3000, 6, allowRandomCreate: true, 30, isSpecial: false, 4, 36, -1, -1, -1, 1, 145, 400, new List<short>
		{
			39, 40, 41, 42, 43, 44, 138, 139, 140, 141,
			142, 143
		}));
		_dataArray.Add(new SkillBookItem(290, 580, 10, 1001, 4, 286, "icon_SkillBook_xingyunqiwufa", 581, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 30, 10, 4650, 9300, 3, 10, 4200, 7, allowRandomCreate: true, 25, isSpecial: false, 4, 36, -1, -1, -1, 1, 146, 600, new List<short> { 40, 41, 42, 43, 44, 139, 140, 141, 142, 143 }));
		_dataArray.Add(new SkillBookItem(291, 582, 10, 1001, 5, 286, "icon_SkillBook_xingyunqiwufa", 583, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 25, 10, 8400, 16800, 4, 12, 5400, 7, allowRandomCreate: true, 20, isSpecial: false, 4, 36, -1, -1, -1, 1, 147, 850, new List<short> { 41, 42, 43, 44, 140, 141, 142, 143 }));
		_dataArray.Add(new SkillBookItem(292, 584, 10, 1001, 6, 286, "icon_SkillBook_lingliuxu", 585, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 20, 10, 13800, 27600, 5, 14, 7200, 8, allowRandomCreate: true, 15, isSpecial: false, 4, 36, -1, -1, -1, 1, 148, 1140, new List<short> { 42, 43, 44, 141, 142, 143 }));
		_dataArray.Add(new SkillBookItem(293, 586, 10, 1001, 0, 293, "icon_SkillBook_meidiantou", 587, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 50, 10, 150, 300, 0, 2, 600, 3, allowRandomCreate: true, 45, isSpecial: false, 4, 36, -1, -1, -1, 1, 149, 60, new List<short>
		{
			108, 109, 110, 111, 112, 113, 114, 115, 116, 27,
			28, 29, 30, 31, 32, 33, 34, 35
		}));
		_dataArray.Add(new SkillBookItem(294, 588, 10, 1001, 1, 293, "icon_SkillBook_meidiantou", 589, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 45, 10, 300, 600, 0, 4, 1200, 4, allowRandomCreate: true, 40, isSpecial: false, 4, 36, -1, -1, -1, 1, 150, 130, new List<short>
		{
			109, 110, 111, 112, 113, 114, 115, 116, 28, 29,
			30, 31, 32, 33, 34, 35
		}));
		_dataArray.Add(new SkillBookItem(295, 590, 10, 1001, 2, 293, "icon_SkillBook_meidiantou", 591, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 40, 10, 900, 1800, 1, 6, 1800, 5, allowRandomCreate: true, 35, isSpecial: false, 4, 36, -1, -1, -1, 1, 151, 240, new List<short>
		{
			110, 111, 112, 113, 114, 115, 116, 29, 30, 31,
			32, 33, 34, 35
		}));
		_dataArray.Add(new SkillBookItem(296, 592, 10, 1001, 3, 293, "icon_SkillBook_wangxiababu", 593, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 35, 10, 2250, 4500, 2, 8, 3000, 6, allowRandomCreate: true, 30, isSpecial: false, 4, 36, -1, -1, -1, 1, 152, 400, new List<short>
		{
			111, 112, 113, 114, 115, 116, 30, 31, 32, 33,
			34, 35
		}));
		_dataArray.Add(new SkillBookItem(297, 594, 10, 1001, 4, 293, "icon_SkillBook_wangxiababu", 595, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 30, 10, 4650, 9300, 3, 10, 4200, 7, allowRandomCreate: true, 25, isSpecial: false, 4, 36, -1, -1, -1, 1, 153, 600, new List<short> { 112, 113, 114, 115, 116, 31, 32, 33, 34, 35 }));
		_dataArray.Add(new SkillBookItem(298, 596, 10, 1001, 5, 293, "icon_SkillBook_wangxiababu", 597, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 25, 10, 8400, 16800, 4, 12, 5400, 7, allowRandomCreate: true, 20, isSpecial: false, 4, 36, -1, -1, -1, 1, 154, 850, new List<short> { 113, 114, 115, 116, 32, 33, 34, 35 }));
		_dataArray.Add(new SkillBookItem(299, 598, 10, 1001, 6, 293, "icon_SkillBook_qingnvlvbing", 599, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 20, 10, 13800, 27600, 5, 14, 7200, 8, allowRandomCreate: true, 15, isSpecial: false, 4, 36, -1, -1, -1, 1, 155, 1140, new List<short> { 114, 115, 116, 33, 34, 35 }));
	}

	private void CreateItems5()
	{
		_dataArray.Add(new SkillBookItem(300, 600, 10, 1001, 7, 293, "icon_SkillBook_luoshenlingbo", 601, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 15, 10, 21150, 42300, 6, 16, 9000, 8, allowRandomCreate: true, 10, isSpecial: false, 4, 36, -1, -1, -1, 1, 156, 1480, new List<short> { 115, 116, 34, 35 }));
		_dataArray.Add(new SkillBookItem(301, 602, 10, 1001, 8, 293, "icon_SkillBook_yuyigong", 603, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 10, 10, 30750, 61500, 6, 18, 10800, 8, allowRandomCreate: true, 5, isSpecial: false, 4, 36, -1, -1, -1, 1, 157, 1860, new List<short> { 116, 35 }));
		_dataArray.Add(new SkillBookItem(302, 604, 10, 1001, 0, 302, "icon_SkillBook_mulvgong", 605, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 50, 10, 150, 300, 0, 2, 600, 3, allowRandomCreate: true, 45, isSpecial: false, 4, 36, -1, -1, -1, 1, 158, 60, new List<short>
		{
			54, 55, 56, 57, 58, 59, 60, 61, 62, 63,
			64, 65, 66, 67, 68, 69, 70, 71, 90, 91,
			92, 93, 94, 95, 96, 97, 98, 99, 100, 101,
			102, 103, 104, 105, 106, 107
		}));
		_dataArray.Add(new SkillBookItem(303, 606, 10, 1001, 1, 302, "icon_SkillBook_mulvgong", 607, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 45, 10, 300, 600, 0, 4, 1200, 4, allowRandomCreate: true, 40, isSpecial: false, 4, 36, -1, -1, -1, 1, 159, 130, new List<short>
		{
			55, 56, 57, 58, 59, 60, 61, 62, 64, 65,
			66, 67, 68, 69, 70, 71, 91, 92, 93, 94,
			95, 96, 97, 98, 100, 101, 102, 103, 104, 105,
			106, 107
		}));
		_dataArray.Add(new SkillBookItem(304, 608, 10, 1001, 2, 302, "icon_SkillBook_mulvgong", 609, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 40, 10, 900, 1800, 1, 6, 1800, 5, allowRandomCreate: true, 35, isSpecial: false, 4, 36, -1, -1, -1, 1, 160, 240, new List<short>
		{
			56, 57, 58, 59, 60, 61, 62, 65, 66, 67,
			68, 69, 70, 71, 92, 93, 94, 95, 96, 97,
			98, 101, 102, 103, 104, 105, 106, 107
		}));
		_dataArray.Add(new SkillBookItem(305, 610, 10, 1001, 3, 302, "icon_SkillBook_huqiushijian", 611, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 35, 10, 2250, 4500, 2, 8, 3000, 6, allowRandomCreate: true, 30, isSpecial: false, 4, 36, -1, -1, -1, 1, 161, 400, new List<short>
		{
			57, 58, 59, 60, 61, 62, 66, 67, 68, 69,
			70, 71, 93, 94, 95, 96, 97, 98, 102, 103,
			104, 105, 106, 107
		}));
		_dataArray.Add(new SkillBookItem(306, 612, 10, 1001, 4, 302, "icon_SkillBook_huqiushijian", 613, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 30, 10, 4650, 9300, 3, 10, 4200, 7, allowRandomCreate: true, 25, isSpecial: false, 4, 36, -1, -1, -1, 1, 162, 600, new List<short>
		{
			58, 59, 60, 61, 62, 67, 68, 69, 70, 71,
			94, 95, 96, 97, 98, 103, 104, 105, 106, 107
		}));
		_dataArray.Add(new SkillBookItem(307, 614, 10, 1001, 5, 302, "icon_SkillBook_huqiushijian", 615, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 25, 10, 8400, 16800, 4, 12, 5400, 7, allowRandomCreate: true, 20, isSpecial: false, 4, 36, -1, -1, -1, 1, 163, 850, new List<short>
		{
			59, 60, 61, 62, 68, 69, 70, 71, 95, 96,
			97, 98, 104, 105, 106, 107
		}));
		_dataArray.Add(new SkillBookItem(308, 616, 10, 1001, 6, 302, "icon_SkillBook_tiankailiuyunshi", 617, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 20, 10, 13800, 27600, 5, 14, 7200, 8, allowRandomCreate: true, 15, isSpecial: false, 4, 36, -1, -1, -1, 1, 164, 1140, new List<short>
		{
			60, 61, 62, 69, 70, 71, 96, 97, 98, 105,
			106, 107
		}));
		_dataArray.Add(new SkillBookItem(309, 618, 10, 1001, 0, 309, "icon_SkillBook_caoshangfei", 619, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 50, 10, 150, 300, 0, 2, 600, 3, allowRandomCreate: true, 45, isSpecial: false, 4, 36, -1, -1, -1, 1, 165, 60, new List<short>
		{
			72, 73, 74, 75, 76, 77, 78, 79, 80, 81,
			82, 83, 84, 85, 86, 87, 88, 89
		}));
		_dataArray.Add(new SkillBookItem(310, 620, 10, 1001, 1, 309, "icon_SkillBook_caoshangfei", 621, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 45, 10, 300, 600, 0, 4, 1200, 4, allowRandomCreate: true, 40, isSpecial: false, 4, 36, -1, -1, -1, 1, 166, 130, new List<short>
		{
			73, 74, 75, 76, 77, 78, 79, 80, 82, 83,
			84, 85, 86, 87, 88, 89
		}));
		_dataArray.Add(new SkillBookItem(311, 622, 10, 1001, 2, 309, "icon_SkillBook_caoshangfei", 623, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 40, 10, 900, 1800, 1, 6, 1800, 5, allowRandomCreate: true, 35, isSpecial: false, 4, 36, -1, -1, -1, 1, 167, 240, new List<short>
		{
			74, 75, 76, 77, 78, 79, 80, 83, 84, 85,
			86, 87, 88, 89
		}));
		_dataArray.Add(new SkillBookItem(312, 624, 10, 1001, 3, 309, "icon_SkillBook_chaishanyixiantian", 625, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 35, 10, 2250, 4500, 2, 8, 3000, 6, allowRandomCreate: true, 30, isSpecial: false, 4, 36, -1, -1, -1, 1, 168, 400, new List<short>
		{
			75, 76, 77, 78, 79, 80, 84, 85, 86, 87,
			88, 89
		}));
		_dataArray.Add(new SkillBookItem(313, 626, 10, 1001, 4, 309, "icon_SkillBook_chaishanyixiantian", 627, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 30, 10, 4650, 9300, 3, 10, 4200, 7, allowRandomCreate: true, 25, isSpecial: false, 4, 36, -1, -1, -1, 1, 169, 600, new List<short> { 76, 77, 78, 79, 80, 85, 86, 87, 88, 89 }));
		_dataArray.Add(new SkillBookItem(314, 628, 10, 1001, 0, 314, "icon_SkillBook_xiandanjiao", 629, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 50, 10, 150, 300, 0, 2, 600, 3, allowRandomCreate: true, 45, isSpecial: false, 4, 36, -1, -1, -1, 1, 170, 60, new List<short> { 117, 118, 119, 120, 121, 122, 123, 124, 125 }));
		_dataArray.Add(new SkillBookItem(315, 630, 10, 1001, 1, 314, "icon_SkillBook_xiandanjiao", 631, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 45, 10, 300, 600, 0, 4, 1200, 4, allowRandomCreate: true, 40, isSpecial: false, 4, 36, -1, -1, -1, 1, 171, 130, new List<short> { 118, 119, 120, 121, 122, 123, 124, 125 }));
		_dataArray.Add(new SkillBookItem(316, 632, 10, 1001, 2, 314, "icon_SkillBook_xiandanjiao", 633, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 40, 10, 900, 1800, 1, 6, 1800, 5, allowRandomCreate: true, 35, isSpecial: false, 4, 36, -1, -1, -1, 1, 172, 240, new List<short> { 119, 120, 121, 122, 123, 124, 125 }));
		_dataArray.Add(new SkillBookItem(317, 634, 10, 1001, 3, 314, "icon_SkillBook_shenzutong", 635, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 35, 10, 2250, 4500, 2, 8, 3000, 6, allowRandomCreate: true, 30, isSpecial: false, 4, 36, -1, -1, -1, 1, 173, 400, new List<short> { 120, 121, 122, 123, 124, 125 }));
		_dataArray.Add(new SkillBookItem(318, 636, 10, 1001, 0, 318, "icon_SkillBook_shexinggong", 637, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 50, 10, 150, 300, 0, 2, 600, 3, allowRandomCreate: true, 45, isSpecial: false, 4, 36, -1, -1, -1, 1, 174, 60, new List<short> { 81, 82, 83, 84, 85, 86, 87, 88, 89 }));
		_dataArray.Add(new SkillBookItem(319, 638, 10, 1001, 1, 318, "icon_SkillBook_shexinggong", 639, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 45, 10, 300, 600, 0, 4, 1200, 4, allowRandomCreate: true, 40, isSpecial: false, 4, 36, -1, -1, -1, 1, 175, 130, new List<short> { 82, 83, 84, 85, 86, 87, 88, 89 }));
		_dataArray.Add(new SkillBookItem(320, 640, 10, 1001, 2, 318, "icon_SkillBook_shexinggong", 641, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 40, 10, 900, 1800, 1, 6, 1800, 5, allowRandomCreate: true, 35, isSpecial: false, 4, 36, -1, -1, -1, 1, 176, 240, new List<short> { 83, 84, 85, 86, 87, 88, 89 }));
		_dataArray.Add(new SkillBookItem(321, 642, 10, 1001, 3, 318, "icon_SkillBook_feitoushu", 643, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 35, 10, 2250, 4500, 2, 8, 3000, 6, allowRandomCreate: true, 30, isSpecial: false, 4, 36, -1, -1, -1, 1, 177, 400, new List<short> { 84, 85, 86, 87, 88, 89 }));
		_dataArray.Add(new SkillBookItem(322, 644, 10, 1001, 4, 318, "icon_SkillBook_feitoushu", 645, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 30, 10, 4650, 9300, 3, 10, 4200, 7, allowRandomCreate: true, 25, isSpecial: false, 4, 36, -1, -1, -1, 1, 178, 600, new List<short> { 85, 86, 87, 88, 89 }));
		_dataArray.Add(new SkillBookItem(323, 646, 10, 1001, 5, 318, "icon_SkillBook_feitoushu", 647, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 25, 10, 8400, 16800, 4, 12, 5400, 7, allowRandomCreate: true, 20, isSpecial: false, 4, 36, -1, -1, -1, 1, 179, 850, new List<short> { 86, 87, 88, 89 }));
		_dataArray.Add(new SkillBookItem(324, 648, 10, 1001, 6, 318, "icon_SkillBook_tianshefan", 649, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 20, 10, 13800, 27600, 5, 14, 7200, 8, allowRandomCreate: true, 15, isSpecial: false, 4, 36, -1, -1, -1, 1, 180, 1140, new List<short> { 87, 88, 89 }));
		_dataArray.Add(new SkillBookItem(325, 650, 10, 1001, 0, 325, "icon_SkillBook_baichizhuang", 651, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 50, 10, 150, 300, 0, 2, 600, 3, allowRandomCreate: true, 45, isSpecial: false, 4, 36, -1, -1, -1, 1, 181, 60, new List<short> { 36, 37, 38, 39, 40, 41, 42, 43, 44 }));
		_dataArray.Add(new SkillBookItem(326, 652, 10, 1001, 1, 325, "icon_SkillBook_baichizhuang", 653, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 45, 10, 300, 600, 0, 4, 1200, 4, allowRandomCreate: true, 40, isSpecial: false, 4, 36, -1, -1, -1, 1, 182, 130, new List<short> { 37, 38, 39, 40, 41, 42, 43, 44 }));
		_dataArray.Add(new SkillBookItem(327, 654, 10, 1001, 2, 325, "icon_SkillBook_baichizhuang", 655, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 40, 10, 900, 1800, 1, 6, 1800, 5, allowRandomCreate: true, 35, isSpecial: false, 4, 36, -1, -1, -1, 1, 183, 240, new List<short> { 38, 39, 40, 41, 42, 43, 44 }));
		_dataArray.Add(new SkillBookItem(328, 656, 10, 1001, 3, 325, "icon_SkillBook_tianheyoubu", 657, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 35, 10, 2250, 4500, 2, 8, 3000, 6, allowRandomCreate: true, 30, isSpecial: false, 4, 36, -1, -1, -1, 1, 184, 400, new List<short> { 39, 40, 41, 42, 43, 44 }));
		_dataArray.Add(new SkillBookItem(329, 658, 10, 1001, 4, 325, "icon_SkillBook_tianheyoubu", 659, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 30, 10, 4650, 9300, 3, 10, 4200, 7, allowRandomCreate: true, 25, isSpecial: false, 4, 36, -1, -1, -1, 1, 185, 600, new List<short> { 40, 41, 42, 43, 44 }));
		_dataArray.Add(new SkillBookItem(330, 660, 10, 1001, 5, 325, "icon_SkillBook_tianheyoubu", 661, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 25, 10, 8400, 16800, 4, 12, 5400, 7, allowRandomCreate: true, 20, isSpecial: false, 4, 36, -1, -1, -1, 1, 186, 850, new List<short> { 41, 42, 43, 44 }));
		_dataArray.Add(new SkillBookItem(331, 662, 10, 1001, 6, 325, "icon_SkillBook_qixingfeixuan", 663, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 20, 10, 13800, 27600, 5, 14, 7200, 8, allowRandomCreate: true, 15, isSpecial: false, 4, 36, -1, -1, -1, 1, 187, 1140, new List<short> { 42, 43, 44 }));
		_dataArray.Add(new SkillBookItem(332, 664, 10, 1001, 7, 325, "icon_SkillBook_tianyuanzong", 665, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 15, 10, 21150, 42300, 6, 16, 9000, 8, allowRandomCreate: true, 10, isSpecial: false, 4, 36, -1, -1, -1, 1, 188, 1480, new List<short> { 43, 44 }));
		_dataArray.Add(new SkillBookItem(333, 666, 10, 1001, 8, 325, "icon_SkillBook_huntianyixinggong", 667, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 10, 10, 30750, 61500, 6, 18, 10800, 8, allowRandomCreate: true, 5, isSpecial: false, 4, 36, -1, -1, -1, 1, 189, 1860, new List<short> { 44 }));
		_dataArray.Add(new SkillBookItem(334, 668, 10, 1001, 0, 334, "icon_SkillBook_chijiaogong", 669, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 50, 10, 150, 300, 0, 2, 600, 3, allowRandomCreate: true, 45, isSpecial: false, 4, 36, -1, -1, -1, 1, 190, 60, new List<short>
		{
			126, 127, 128, 129, 130, 131, 132, 133, 134, 45,
			46, 47, 48, 49, 50, 51, 52, 53
		}));
		_dataArray.Add(new SkillBookItem(335, 670, 10, 1001, 1, 334, "icon_SkillBook_chijiaogong", 671, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 45, 10, 300, 600, 0, 4, 1200, 4, allowRandomCreate: true, 40, isSpecial: false, 4, 36, -1, -1, -1, 1, 191, 130, new List<short>
		{
			127, 128, 129, 130, 131, 132, 133, 134, 46, 47,
			48, 49, 50, 51, 52, 53
		}));
		_dataArray.Add(new SkillBookItem(336, 672, 10, 1001, 2, 334, "icon_SkillBook_chijiaogong", 673, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 40, 10, 900, 1800, 1, 6, 1800, 5, allowRandomCreate: true, 35, isSpecial: false, 4, 36, -1, -1, -1, 1, 192, 240, new List<short>
		{
			128, 129, 130, 131, 132, 133, 134, 47, 48, 49,
			50, 51, 52, 53
		}));
		_dataArray.Add(new SkillBookItem(337, 674, 10, 1001, 3, 334, "icon_SkillBook_fulongfenyue", 675, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 35, 10, 2250, 4500, 2, 8, 3000, 6, allowRandomCreate: true, 30, isSpecial: false, 4, 36, -1, -1, -1, 1, 193, 400, new List<short>
		{
			129, 130, 131, 132, 133, 134, 48, 49, 50, 51,
			52, 53
		}));
		_dataArray.Add(new SkillBookItem(338, 676, 10, 1001, 4, 334, "icon_SkillBook_fulongfenyue", 677, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 30, 10, 4650, 9300, 3, 10, 4200, 7, allowRandomCreate: true, 25, isSpecial: false, 4, 36, -1, -1, -1, 1, 194, 600, new List<short> { 130, 131, 132, 133, 134, 49, 50, 51, 52, 53 }));
		_dataArray.Add(new SkillBookItem(339, 678, 10, 1001, 5, 334, "icon_SkillBook_fulongfenyue", 679, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 25, 10, 8400, 16800, 4, 12, 5400, 7, allowRandomCreate: true, 20, isSpecial: false, 4, 36, -1, -1, -1, 1, 195, 850, new List<short> { 131, 132, 133, 134, 50, 51, 52, 53 }));
		_dataArray.Add(new SkillBookItem(340, 680, 10, 1001, 0, 340, "icon_SkillBook_licuanshu", 681, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 50, 10, 150, 300, 0, 2, 600, 3, allowRandomCreate: true, 45, isSpecial: false, 4, 36, -1, -1, -1, 1, 196, 60, new List<short>
		{
			81, 82, 83, 84, 85, 86, 87, 88, 89, 135,
			136, 137, 138, 139, 140, 141, 142, 143
		}));
		_dataArray.Add(new SkillBookItem(341, 682, 10, 1001, 1, 340, "icon_SkillBook_licuanshu", 683, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 45, 10, 300, 600, 0, 4, 1200, 4, allowRandomCreate: true, 40, isSpecial: false, 4, 36, -1, -1, -1, 1, 197, 130, new List<short>
		{
			82, 83, 84, 85, 86, 87, 88, 89, 136, 137,
			138, 139, 140, 141, 142, 143
		}));
		_dataArray.Add(new SkillBookItem(342, 684, 10, 1001, 2, 340, "icon_SkillBook_licuanshu", 685, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 40, 10, 900, 1800, 1, 6, 1800, 5, allowRandomCreate: true, 35, isSpecial: false, 4, 36, -1, -1, -1, 1, 198, 240, new List<short>
		{
			83, 84, 85, 86, 87, 88, 89, 137, 138, 139,
			140, 141, 142, 143
		}));
		_dataArray.Add(new SkillBookItem(343, 686, 10, 1001, 3, 340, "icon_SkillBook_guihourudizhentianfa", 687, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 35, 10, 2250, 4500, 2, 8, 3000, 6, allowRandomCreate: true, 30, isSpecial: false, 4, 36, -1, -1, -1, 1, 199, 400, new List<short>
		{
			84, 85, 86, 87, 88, 89, 138, 139, 140, 141,
			142, 143
		}));
		_dataArray.Add(new SkillBookItem(344, 688, 10, 1001, 4, 340, "icon_SkillBook_guihourudizhentianfa", 689, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 30, 10, 4650, 9300, 3, 10, 4200, 7, allowRandomCreate: true, 25, isSpecial: false, 4, 36, -1, -1, -1, 1, 200, 600, new List<short> { 85, 86, 87, 88, 89, 139, 140, 141, 142, 143 }));
		_dataArray.Add(new SkillBookItem(345, 690, 10, 1001, 0, 345, "icon_SkillBook_wohugong", 691, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 50, 10, 150, 300, 0, 2, 600, 3, allowRandomCreate: true, 45, isSpecial: false, 4, 36, -1, -1, -1, 2, 201, 60, new List<short> { 117, 118, 119, 120, 121, 122, 123, 124, 125 }));
		_dataArray.Add(new SkillBookItem(346, 692, 10, 1001, 1, 345, "icon_SkillBook_wohugong", 693, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 45, 10, 300, 600, 0, 4, 1200, 4, allowRandomCreate: true, 40, isSpecial: false, 4, 36, -1, -1, -1, 2, 202, 130, new List<short> { 118, 119, 120, 121, 122, 123, 124, 125 }));
		_dataArray.Add(new SkillBookItem(347, 694, 10, 1001, 2, 345, "icon_SkillBook_wohugong", 695, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 40, 10, 900, 1800, 1, 6, 1800, 5, allowRandomCreate: true, 35, isSpecial: false, 4, 36, -1, -1, -1, 2, 203, 240, new List<short> { 119, 120, 121, 122, 123, 124, 125 }));
		_dataArray.Add(new SkillBookItem(348, 696, 10, 1001, 3, 345, "icon_SkillBook_biantitongrenfa", 697, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 35, 10, 2250, 4500, 2, 8, 3000, 6, allowRandomCreate: true, 30, isSpecial: false, 4, 36, -1, -1, -1, 2, 204, 400, new List<short> { 120, 121, 122, 123, 124, 125 }));
		_dataArray.Add(new SkillBookItem(349, 698, 10, 1001, 4, 345, "icon_SkillBook_biantitongrenfa", 699, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 30, 10, 4650, 9300, 3, 10, 4200, 7, allowRandomCreate: true, 25, isSpecial: false, 4, 36, -1, -1, -1, 2, 205, 600, new List<short> { 121, 122, 123, 124, 125 }));
		_dataArray.Add(new SkillBookItem(350, 700, 10, 1001, 5, 345, "icon_SkillBook_biantitongrenfa", 701, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 25, 10, 8400, 16800, 4, 12, 5400, 7, allowRandomCreate: true, 20, isSpecial: false, 4, 36, -1, -1, -1, 2, 206, 850, new List<short> { 122, 123, 124, 125 }));
		_dataArray.Add(new SkillBookItem(351, 702, 10, 1001, 6, 345, "icon_SkillBook_fomenshizihou", 703, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 20, 10, 13800, 27600, 5, 14, 7200, 8, allowRandomCreate: true, 15, isSpecial: false, 4, 36, -1, -1, -1, 2, 207, 1140, new List<short> { 123, 124, 125 }));
		_dataArray.Add(new SkillBookItem(352, 704, 10, 1001, 7, 345, "icon_SkillBook_hunyuanyiqigong", 705, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 15, 10, 21150, 42300, 6, 16, 9000, 8, allowRandomCreate: true, 10, isSpecial: false, 4, 36, -1, -1, -1, 2, 208, 1480, new List<short> { 124, 125 }));
		_dataArray.Add(new SkillBookItem(353, 706, 10, 1001, 8, 345, "icon_SkillBook_jinganghutishengong", 707, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 10, 10, 30750, 61500, 6, 18, 10800, 8, allowRandomCreate: true, 5, isSpecial: false, 4, 36, -1, -1, -1, 2, 209, 1860, new List<short> { 125 }));
		_dataArray.Add(new SkillBookItem(354, 708, 10, 1001, 0, 354, "icon_SkillBook_emeihubugong", 709, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 50, 10, 150, 300, 0, 2, 600, 3, allowRandomCreate: true, 45, isSpecial: false, 4, 36, -1, -1, -1, 2, 210, 60, new List<short>
		{
			108, 109, 110, 111, 112, 113, 114, 115, 116, 117,
			118, 119, 120, 121, 122, 123, 124, 125
		}));
		_dataArray.Add(new SkillBookItem(355, 710, 10, 1001, 1, 354, "icon_SkillBook_emeihubugong", 711, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 45, 10, 300, 600, 0, 4, 1200, 4, allowRandomCreate: true, 40, isSpecial: false, 4, 36, -1, -1, -1, 2, 211, 130, new List<short>
		{
			109, 110, 111, 112, 113, 114, 115, 116, 118, 119,
			120, 121, 122, 123, 124, 125
		}));
		_dataArray.Add(new SkillBookItem(356, 712, 10, 1001, 2, 354, "icon_SkillBook_emeihubugong", 713, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 40, 10, 900, 1800, 1, 6, 1800, 5, allowRandomCreate: true, 35, isSpecial: false, 4, 36, -1, -1, -1, 2, 212, 240, new List<short>
		{
			110, 111, 112, 113, 114, 115, 116, 119, 120, 121,
			122, 123, 124, 125
		}));
		_dataArray.Add(new SkillBookItem(357, 714, 10, 1001, 3, 354, "icon_SkillBook_jinmogu", 715, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 35, 10, 2250, 4500, 2, 8, 3000, 6, allowRandomCreate: true, 30, isSpecial: false, 4, 36, -1, -1, -1, 2, 213, 400, new List<short>
		{
			111, 112, 113, 114, 115, 116, 120, 121, 122, 123,
			124, 125
		}));
		_dataArray.Add(new SkillBookItem(358, 716, 10, 1001, 4, 354, "icon_SkillBook_jinmogu", 717, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 30, 10, 4650, 9300, 3, 10, 4200, 7, allowRandomCreate: true, 25, isSpecial: false, 4, 36, -1, -1, -1, 2, 214, 600, new List<short> { 112, 113, 114, 115, 116, 121, 122, 123, 124, 125 }));
		_dataArray.Add(new SkillBookItem(359, 718, 10, 1001, 5, 354, "icon_SkillBook_jinmogu", 719, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 25, 10, 8400, 16800, 4, 12, 5400, 7, allowRandomCreate: true, 20, isSpecial: false, 4, 36, -1, -1, -1, 2, 215, 850, new List<short> { 113, 114, 115, 116, 122, 123, 124, 125 }));
	}

	private void CreateItems6()
	{
		_dataArray.Add(new SkillBookItem(360, 720, 10, 1001, 6, 354, "icon_SkillBook_yangxiangongjinshenfa", 721, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 20, 10, 13800, 27600, 5, 14, 7200, 8, allowRandomCreate: true, 15, isSpecial: false, 4, 36, -1, -1, -1, 2, 216, 1140, new List<short> { 114, 115, 116, 123, 124, 125 }));
		_dataArray.Add(new SkillBookItem(361, 722, 10, 1001, 7, 354, "icon_SkillBook_qixinghenglian", 723, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 15, 10, 21150, 42300, 6, 16, 9000, 8, allowRandomCreate: true, 10, isSpecial: false, 4, 36, -1, -1, -1, 2, 217, 1480, new List<short> { 115, 116, 124, 125 }));
		_dataArray.Add(new SkillBookItem(362, 724, 10, 1001, 8, 354, "icon_SkillBook_shengdengqiwujue", 725, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 10, 10, 30750, 61500, 6, 18, 10800, 8, allowRandomCreate: true, 5, isSpecial: false, 4, 36, -1, -1, -1, 2, 218, 1860, new List<short> { 116, 125 }));
		_dataArray.Add(new SkillBookItem(363, 726, 10, 1001, 0, 363, "icon_SkillBook_sanshiliushouyizhenfa", 727, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 50, 10, 150, 300, 0, 2, 600, 3, allowRandomCreate: true, 45, isSpecial: false, 4, 36, -1, -1, -1, 2, 219, 60, new List<short> { 72, 73, 74, 75, 76, 77, 78, 79, 80 }));
		_dataArray.Add(new SkillBookItem(364, 728, 10, 1001, 1, 363, "icon_SkillBook_sanshiliushouyizhenfa", 729, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 45, 10, 300, 600, 0, 4, 1200, 4, allowRandomCreate: true, 40, isSpecial: false, 4, 36, -1, -1, -1, 2, 220, 130, new List<short> { 73, 74, 75, 76, 77, 78, 79, 80 }));
		_dataArray.Add(new SkillBookItem(365, 730, 10, 1001, 2, 363, "icon_SkillBook_sanshiliushouyizhenfa", 731, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 40, 10, 900, 1800, 1, 6, 1800, 5, allowRandomCreate: true, 35, isSpecial: false, 4, 36, -1, -1, -1, 2, 221, 240, new List<short> { 74, 75, 76, 77, 78, 79, 80 }));
		_dataArray.Add(new SkillBookItem(366, 732, 10, 1001, 3, 363, "icon_SkillBook_luanzhencuoxue", 733, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 35, 10, 2250, 4500, 2, 8, 3000, 6, allowRandomCreate: true, 30, isSpecial: false, 4, 36, -1, -1, -1, 2, 222, 400, new List<short> { 75, 76, 77, 78, 79, 80 }));
		_dataArray.Add(new SkillBookItem(367, 734, 10, 1001, 4, 363, "icon_SkillBook_luanzhencuoxue", 735, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 30, 10, 4650, 9300, 3, 10, 4200, 7, allowRandomCreate: true, 25, isSpecial: false, 4, 36, -1, -1, -1, 2, 223, 600, new List<short> { 76, 77, 78, 79, 80 }));
		_dataArray.Add(new SkillBookItem(368, 736, 10, 1001, 5, 363, "icon_SkillBook_luanzhencuoxue", 737, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 25, 10, 8400, 16800, 4, 12, 5400, 7, allowRandomCreate: true, 20, isSpecial: false, 4, 36, -1, -1, -1, 2, 224, 850, new List<short> { 77, 78, 79, 80 }));
		_dataArray.Add(new SkillBookItem(369, 738, 10, 1001, 6, 363, "icon_SkillBook_shengsibamen", 739, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 20, 10, 13800, 27600, 5, 14, 7200, 8, allowRandomCreate: true, 15, isSpecial: false, 4, 36, -1, -1, -1, 2, 225, 1140, new List<short> { 78, 79, 80 }));
		_dataArray.Add(new SkillBookItem(370, 740, 10, 1001, 7, 363, "icon_SkillBook_xuehainingbingshu", 741, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 15, 10, 21150, 42300, 6, 16, 9000, 8, allowRandomCreate: true, 10, isSpecial: false, 4, 36, -1, -1, -1, 2, 226, 1480, new List<short> { 79, 80 }));
		_dataArray.Add(new SkillBookItem(371, 742, 10, 1001, 8, 363, "icon_SkillBook_wanbibupofa", 743, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 10, 10, 30750, 61500, 6, 18, 10800, 8, allowRandomCreate: true, 5, isSpecial: false, 4, 36, -1, -1, -1, 2, 227, 1860, new List<short> { 80 }));
		_dataArray.Add(new SkillBookItem(372, 744, 10, 1001, 0, 372, "icon_SkillBook_wudangtiebushan", 745, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 50, 10, 150, 300, 0, 2, 600, 3, allowRandomCreate: true, 45, isSpecial: false, 4, 36, -1, -1, -1, 2, 228, 60, new List<short> { 108, 109, 110, 111, 112, 113, 114, 115, 116 }));
		_dataArray.Add(new SkillBookItem(373, 746, 10, 1001, 1, 372, "icon_SkillBook_wudangtiebushan", 747, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 45, 10, 300, 600, 0, 4, 1200, 4, allowRandomCreate: true, 40, isSpecial: false, 4, 36, -1, -1, -1, 2, 229, 130, new List<short> { 109, 110, 111, 112, 113, 114, 115, 116 }));
		_dataArray.Add(new SkillBookItem(374, 748, 10, 1001, 2, 372, "icon_SkillBook_wudangtiebushan", 749, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 40, 10, 900, 1800, 1, 6, 1800, 5, allowRandomCreate: true, 35, isSpecial: false, 4, 36, -1, -1, -1, 2, 230, 240, new List<short> { 110, 111, 112, 113, 114, 115, 116 }));
		_dataArray.Add(new SkillBookItem(375, 750, 10, 1001, 3, 372, "icon_SkillBook_yinyangzhoutianfa", 751, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 35, 10, 2250, 4500, 2, 8, 3000, 6, allowRandomCreate: true, 30, isSpecial: false, 4, 36, -1, -1, -1, 2, 231, 400, new List<short> { 111, 112, 113, 114, 115, 116 }));
		_dataArray.Add(new SkillBookItem(376, 752, 10, 1001, 4, 372, "icon_SkillBook_yinyangzhoutianfa", 753, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 30, 10, 4650, 9300, 3, 10, 4200, 7, allowRandomCreate: true, 25, isSpecial: false, 4, 36, -1, -1, -1, 2, 232, 600, new List<short> { 112, 113, 114, 115, 116 }));
		_dataArray.Add(new SkillBookItem(377, 754, 10, 1001, 5, 372, "icon_SkillBook_yinyangzhoutianfa", 755, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 25, 10, 8400, 16800, 4, 12, 5400, 7, allowRandomCreate: true, 20, isSpecial: false, 4, 36, -1, -1, -1, 2, 233, 850, new List<short> { 113, 114, 115, 116 }));
		_dataArray.Add(new SkillBookItem(378, 756, 10, 1001, 6, 372, "icon_SkillBook_wujijin", 757, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 20, 10, 13800, 27600, 5, 14, 7200, 8, allowRandomCreate: true, 15, isSpecial: false, 4, 36, -1, -1, -1, 2, 234, 1140, new List<short> { 114, 115, 116 }));
		_dataArray.Add(new SkillBookItem(379, 758, 10, 1001, 7, 372, "icon_SkillBook_sanhuajuding", 759, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 15, 10, 21150, 42300, 6, 16, 9000, 8, allowRandomCreate: true, 10, isSpecial: false, 4, 36, -1, -1, -1, 2, 235, 1480, new List<short> { 115, 116 }));
		_dataArray.Add(new SkillBookItem(380, 760, 10, 1001, 0, 380, "icon_SkillBook_jiugunshibadie", 761, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 50, 10, 150, 300, 0, 2, 600, 3, allowRandomCreate: true, 45, isSpecial: false, 4, 36, -1, -1, -1, 2, 236, 60, new List<short>
		{
			108, 109, 110, 111, 112, 113, 114, 115, 116, 117,
			118, 119, 120, 121, 122, 123, 124, 125
		}));
		_dataArray.Add(new SkillBookItem(381, 762, 10, 1001, 1, 380, "icon_SkillBook_jiugunshibadie", 763, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 45, 10, 300, 600, 0, 4, 1200, 4, allowRandomCreate: true, 40, isSpecial: false, 4, 36, -1, -1, -1, 2, 237, 130, new List<short>
		{
			109, 110, 111, 112, 113, 114, 115, 116, 118, 119,
			120, 121, 122, 123, 124, 125
		}));
		_dataArray.Add(new SkillBookItem(382, 764, 10, 1001, 2, 380, "icon_SkillBook_jiugunshibadie", 765, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 40, 10, 900, 1800, 1, 6, 1800, 5, allowRandomCreate: true, 35, isSpecial: false, 4, 36, -1, -1, -1, 2, 238, 240, new List<short>
		{
			110, 111, 112, 113, 114, 115, 116, 119, 120, 121,
			122, 123, 124, 125
		}));
		_dataArray.Add(new SkillBookItem(383, 766, 10, 1001, 3, 380, "icon_SkillBook_jindaohuanzhanggong", 767, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 35, 10, 2250, 4500, 2, 8, 3000, 6, allowRandomCreate: true, 30, isSpecial: false, 4, 36, -1, -1, -1, 2, 239, 400, new List<short>
		{
			111, 112, 113, 114, 115, 116, 120, 121, 122, 123,
			124, 125
		}));
		_dataArray.Add(new SkillBookItem(384, 768, 10, 1001, 4, 380, "icon_SkillBook_jindaohuanzhanggong", 769, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 30, 10, 4650, 9300, 3, 10, 4200, 7, allowRandomCreate: true, 25, isSpecial: false, 4, 36, -1, -1, -1, 2, 240, 600, new List<short> { 112, 113, 114, 115, 116, 121, 122, 123, 124, 125 }));
		_dataArray.Add(new SkillBookItem(385, 770, 10, 1001, 5, 380, "icon_SkillBook_jindaohuanzhanggong", 771, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 25, 10, 8400, 16800, 4, 12, 5400, 7, allowRandomCreate: true, 20, isSpecial: false, 4, 36, -1, -1, -1, 2, 241, 850, new List<short> { 113, 114, 115, 116, 122, 123, 124, 125 }));
		_dataArray.Add(new SkillBookItem(386, 772, 10, 1001, 6, 380, "icon_SkillBook_taichuguicangjue", 773, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 20, 10, 13800, 27600, 5, 14, 7200, 8, allowRandomCreate: true, 15, isSpecial: false, 4, 36, -1, -1, -1, 2, 242, 1140, new List<short> { 114, 115, 116, 123, 124, 125 }));
		_dataArray.Add(new SkillBookItem(387, 774, 10, 1001, 0, 387, "icon_SkillBook_shixiangtietougong", 775, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 50, 10, 150, 300, 0, 2, 600, 3, allowRandomCreate: true, 45, isSpecial: false, 4, 36, -1, -1, -1, 2, 243, 60, new List<short>()));
		_dataArray.Add(new SkillBookItem(388, 776, 10, 1001, 1, 387, "icon_SkillBook_shixiangtietougong", 777, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 45, 10, 300, 600, 0, 4, 1200, 4, allowRandomCreate: true, 40, isSpecial: false, 4, 36, -1, -1, -1, 2, 244, 130, new List<short>()));
		_dataArray.Add(new SkillBookItem(389, 778, 10, 1001, 2, 387, "icon_SkillBook_shixiangtietougong", 779, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 40, 10, 900, 1800, 1, 6, 1800, 5, allowRandomCreate: true, 35, isSpecial: false, 4, 36, -1, -1, -1, 2, 245, 240, new List<short>()));
		_dataArray.Add(new SkillBookItem(390, 780, 10, 1001, 3, 387, "icon_SkillBook_bawangjuding", 781, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 35, 10, 2250, 4500, 2, 8, 3000, 6, allowRandomCreate: true, 30, isSpecial: false, 4, 36, -1, -1, -1, 2, 246, 400, new List<short>()));
		_dataArray.Add(new SkillBookItem(391, 782, 10, 1001, 4, 387, "icon_SkillBook_bawangjuding", 783, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 30, 10, 4650, 9300, 3, 10, 4200, 7, allowRandomCreate: true, 25, isSpecial: false, 4, 36, -1, -1, -1, 2, 247, 600, new List<short>()));
		_dataArray.Add(new SkillBookItem(392, 784, 10, 1001, 5, 387, "icon_SkillBook_bawangjuding", 785, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 25, 10, 8400, 16800, 4, 12, 5400, 7, allowRandomCreate: true, 20, isSpecial: false, 4, 36, -1, -1, -1, 2, 248, 850, new List<short>()));
		_dataArray.Add(new SkillBookItem(393, 786, 10, 1001, 0, 393, "icon_SkillBook_wuguibanyunfa", 787, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 50, 10, 150, 300, 0, 2, 600, 3, allowRandomCreate: true, 45, isSpecial: false, 4, 36, -1, -1, -1, 2, 249, 60, new List<short>
		{
			36, 37, 38, 39, 40, 41, 42, 43, 44, 135,
			136, 137, 138, 139, 140, 141, 142, 143
		}));
		_dataArray.Add(new SkillBookItem(394, 788, 10, 1001, 1, 393, "icon_SkillBook_wuguibanyunfa", 789, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 45, 10, 300, 600, 0, 4, 1200, 4, allowRandomCreate: true, 40, isSpecial: false, 4, 36, -1, -1, -1, 2, 250, 130, new List<short>
		{
			37, 38, 39, 40, 41, 42, 43, 44, 136, 137,
			138, 139, 140, 141, 142, 143
		}));
		_dataArray.Add(new SkillBookItem(395, 790, 10, 1001, 2, 393, "icon_SkillBook_wuguibanyunfa", 791, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 40, 10, 900, 1800, 1, 6, 1800, 5, allowRandomCreate: true, 35, isSpecial: false, 4, 36, -1, -1, -1, 2, 251, 240, new List<short>
		{
			38, 39, 40, 41, 42, 43, 44, 137, 138, 139,
			140, 141, 142, 143
		}));
		_dataArray.Add(new SkillBookItem(396, 792, 10, 1001, 3, 393, "icon_SkillBook_shehundafa", 793, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 35, 10, 2250, 4500, 2, 8, 3000, 6, allowRandomCreate: true, 30, isSpecial: false, 4, 36, -1, -1, -1, 2, 252, 400, new List<short>
		{
			39, 40, 41, 42, 43, 44, 138, 139, 140, 141,
			142, 143
		}));
		_dataArray.Add(new SkillBookItem(397, 794, 10, 1001, 4, 393, "icon_SkillBook_shehundafa", 795, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 30, 10, 4650, 9300, 3, 10, 4200, 7, allowRandomCreate: true, 25, isSpecial: false, 4, 36, -1, -1, -1, 2, 253, 600, new List<short> { 40, 41, 42, 43, 44, 139, 140, 141, 142, 143 }));
		_dataArray.Add(new SkillBookItem(398, 796, 10, 1001, 5, 393, "icon_SkillBook_shehundafa", 797, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 25, 10, 8400, 16800, 4, 12, 5400, 7, allowRandomCreate: true, 20, isSpecial: false, 4, 36, -1, -1, -1, 2, 254, 850, new List<short> { 41, 42, 43, 44, 140, 141, 142, 143 }));
		_dataArray.Add(new SkillBookItem(399, 798, 10, 1001, 6, 393, "icon_SkillBook_xiangnibizhuaigong", 799, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 20, 10, 13800, 27600, 5, 14, 7200, 8, allowRandomCreate: true, 15, isSpecial: false, 4, 36, -1, -1, -1, 2, 255, 1140, new List<short> { 42, 43, 44, 141, 142, 143 }));
		_dataArray.Add(new SkillBookItem(400, 800, 10, 1001, 7, 393, "icon_SkillBook_wanhuagegui", 801, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 15, 10, 21150, 42300, 6, 16, 9000, 8, allowRandomCreate: true, 10, isSpecial: false, 4, 36, -1, -1, -1, 2, 256, 1480, new List<short> { 43, 44, 142, 143 }));
		_dataArray.Add(new SkillBookItem(401, 802, 10, 1001, 0, 401, "icon_SkillBook_bingqingyujie", 803, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 50, 10, 150, 300, 0, 2, 600, 3, allowRandomCreate: true, 45, isSpecial: false, 4, 36, -1, -1, -1, 2, 257, 60, new List<short>
		{
			108, 109, 110, 111, 112, 113, 114, 115, 116, 27,
			28, 29, 30, 31, 32, 33, 34, 35
		}));
		_dataArray.Add(new SkillBookItem(402, 804, 10, 1001, 1, 401, "icon_SkillBook_bingqingyujie", 805, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 45, 10, 300, 600, 0, 4, 1200, 4, allowRandomCreate: true, 40, isSpecial: false, 4, 36, -1, -1, -1, 2, 258, 130, new List<short>
		{
			109, 110, 111, 112, 113, 114, 115, 116, 28, 29,
			30, 31, 32, 33, 34, 35
		}));
		_dataArray.Add(new SkillBookItem(403, 806, 10, 1001, 2, 401, "icon_SkillBook_bingqingyujie", 807, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 40, 10, 900, 1800, 1, 6, 1800, 5, allowRandomCreate: true, 35, isSpecial: false, 4, 36, -1, -1, -1, 2, 259, 240, new List<short>
		{
			110, 111, 112, 113, 114, 115, 116, 29, 30, 31,
			32, 33, 34, 35
		}));
		_dataArray.Add(new SkillBookItem(404, 808, 10, 1001, 3, 401, "icon_SkillBook_tiannvsanhuazhen", 809, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 35, 10, 2250, 4500, 2, 8, 3000, 6, allowRandomCreate: true, 30, isSpecial: false, 4, 36, -1, -1, -1, 2, 260, 400, new List<short>
		{
			111, 112, 113, 114, 115, 116, 30, 31, 32, 33,
			34, 35
		}));
		_dataArray.Add(new SkillBookItem(405, 810, 10, 1001, 4, 401, "icon_SkillBook_tiannvsanhuazhen", 811, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 30, 10, 4650, 9300, 3, 10, 4200, 7, allowRandomCreate: true, 25, isSpecial: false, 4, 36, -1, -1, -1, 2, 261, 600, new List<short> { 112, 113, 114, 115, 116, 31, 32, 33, 34, 35 }));
		_dataArray.Add(new SkillBookItem(406, 812, 10, 1001, 5, 401, "icon_SkillBook_tiannvsanhuazhen", 813, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 25, 10, 8400, 16800, 4, 12, 5400, 7, allowRandomCreate: true, 20, isSpecial: false, 4, 36, -1, -1, -1, 2, 262, 850, new List<short> { 113, 114, 115, 116, 32, 33, 34, 35 }));
		_dataArray.Add(new SkillBookItem(407, 814, 10, 1001, 6, 401, "icon_SkillBook_nvwabutianshi", 815, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 20, 10, 13800, 27600, 5, 14, 7200, 8, allowRandomCreate: true, 15, isSpecial: false, 4, 36, -1, -1, -1, 2, 263, 1140, new List<short> { 114, 115, 116, 33, 34, 35 }));
		_dataArray.Add(new SkillBookItem(408, 816, 10, 1001, 0, 408, "icon_SkillBook_qishoubajiaogong", 817, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 50, 10, 150, 300, 0, 2, 600, 3, allowRandomCreate: true, 45, isSpecial: false, 4, 36, -1, -1, -1, 2, 264, 60, new List<short>
		{
			54, 55, 56, 57, 58, 59, 60, 61, 62, 63,
			64, 65, 66, 67, 68, 69, 70, 71, 90, 91,
			92, 93, 94, 95, 96, 97, 98, 99, 100, 101,
			102, 103, 104, 105, 106, 107
		}));
		_dataArray.Add(new SkillBookItem(409, 818, 10, 1001, 1, 408, "icon_SkillBook_qishoubajiaogong", 819, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 45, 10, 300, 600, 0, 4, 1200, 4, allowRandomCreate: true, 40, isSpecial: false, 4, 36, -1, -1, -1, 2, 265, 130, new List<short>
		{
			55, 56, 57, 58, 59, 60, 61, 62, 64, 65,
			66, 67, 68, 69, 70, 71, 91, 92, 93, 94,
			95, 96, 97, 98, 100, 101, 102, 103, 104, 105,
			106, 107
		}));
		_dataArray.Add(new SkillBookItem(410, 820, 10, 1001, 2, 408, "icon_SkillBook_qishoubajiaogong", 821, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 40, 10, 900, 1800, 1, 6, 1800, 5, allowRandomCreate: true, 35, isSpecial: false, 4, 36, -1, -1, -1, 2, 266, 240, new List<short>
		{
			56, 57, 58, 59, 60, 61, 62, 65, 66, 67,
			68, 69, 70, 71, 92, 93, 94, 95, 96, 97,
			98, 101, 102, 103, 104, 105, 106, 107
		}));
		_dataArray.Add(new SkillBookItem(411, 822, 10, 1001, 3, 408, "icon_SkillBook_shenjizhen", 823, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 35, 10, 2250, 4500, 2, 8, 3000, 6, allowRandomCreate: true, 30, isSpecial: false, 4, 36, -1, -1, -1, 2, 267, 400, new List<short>
		{
			57, 58, 59, 60, 61, 62, 66, 67, 68, 69,
			70, 71, 93, 94, 95, 96, 97, 98, 102, 103,
			104, 105, 106, 107
		}));
		_dataArray.Add(new SkillBookItem(412, 824, 10, 1001, 4, 408, "icon_SkillBook_shenjizhen", 825, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 30, 10, 4650, 9300, 3, 10, 4200, 7, allowRandomCreate: true, 25, isSpecial: false, 4, 36, -1, -1, -1, 2, 268, 600, new List<short>
		{
			58, 59, 60, 61, 62, 67, 68, 69, 70, 71,
			94, 95, 96, 97, 98, 103, 104, 105, 106, 107
		}));
		_dataArray.Add(new SkillBookItem(413, 826, 10, 1001, 5, 408, "icon_SkillBook_shenjizhen", 827, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 25, 10, 8400, 16800, 4, 12, 5400, 7, allowRandomCreate: true, 20, isSpecial: false, 4, 36, -1, -1, -1, 2, 269, 850, new List<short>
		{
			59, 60, 61, 62, 68, 69, 70, 71, 95, 96,
			97, 98, 104, 105, 106, 107
		}));
		_dataArray.Add(new SkillBookItem(414, 828, 10, 1001, 6, 408, "icon_SkillBook_tianzhuxuantiece", 829, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 20, 10, 13800, 27600, 5, 14, 7200, 8, allowRandomCreate: true, 15, isSpecial: false, 4, 36, -1, -1, -1, 2, 270, 1140, new List<short>
		{
			60, 61, 62, 69, 70, 71, 96, 97, 98, 105,
			106, 107
		}));
		_dataArray.Add(new SkillBookItem(415, 830, 10, 1001, 0, 415, "icon_SkillBook_sizhiqinggao", 831, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 50, 10, 150, 300, 0, 2, 600, 3, allowRandomCreate: true, 45, isSpecial: false, 4, 36, -1, -1, -1, 2, 271, 60, new List<short>
		{
			72, 73, 74, 75, 76, 77, 78, 79, 80, 81,
			82, 83, 84, 85, 86, 87, 88, 89
		}));
		_dataArray.Add(new SkillBookItem(416, 832, 10, 1001, 1, 415, "icon_SkillBook_sizhiqinggao", 833, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 45, 10, 300, 600, 0, 4, 1200, 4, allowRandomCreate: true, 40, isSpecial: false, 4, 36, -1, -1, -1, 2, 272, 130, new List<short>
		{
			73, 74, 75, 76, 77, 78, 79, 80, 82, 83,
			84, 85, 86, 87, 88, 89
		}));
		_dataArray.Add(new SkillBookItem(417, 834, 10, 1001, 2, 415, "icon_SkillBook_sizhiqinggao", 835, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 40, 10, 900, 1800, 1, 6, 1800, 5, allowRandomCreate: true, 35, isSpecial: false, 4, 36, -1, -1, -1, 2, 273, 240, new List<short>
		{
			74, 75, 76, 77, 78, 79, 80, 83, 84, 85,
			86, 87, 88, 89
		}));
		_dataArray.Add(new SkillBookItem(418, 836, 10, 1001, 3, 415, "icon_SkillBook_wuhuangbidushu", 837, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 35, 10, 2250, 4500, 2, 8, 3000, 6, allowRandomCreate: true, 30, isSpecial: false, 4, 36, -1, -1, -1, 2, 274, 400, new List<short>
		{
			75, 76, 77, 78, 79, 80, 84, 85, 86, 87,
			88, 89
		}));
		_dataArray.Add(new SkillBookItem(419, 838, 10, 1001, 4, 415, "icon_SkillBook_wuhuangbidushu", 839, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 30, 10, 4650, 9300, 3, 10, 4200, 7, allowRandomCreate: true, 25, isSpecial: false, 4, 36, -1, -1, -1, 2, 275, 600, new List<short> { 76, 77, 78, 79, 80, 85, 86, 87, 88, 89 }));
	}

	private void CreateItems7()
	{
		_dataArray.Add(new SkillBookItem(420, 840, 10, 1001, 5, 415, "icon_SkillBook_wuhuangbidushu", 841, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 25, 10, 8400, 16800, 4, 12, 5400, 7, allowRandomCreate: true, 20, isSpecial: false, 4, 36, -1, -1, -1, 2, 276, 850, new List<short> { 77, 78, 79, 80, 86, 87, 88, 89 }));
		_dataArray.Add(new SkillBookItem(421, 842, 10, 1001, 6, 415, "icon_SkillBook_baishuichangshengfa", 843, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 20, 10, 13800, 27600, 5, 14, 7200, 8, allowRandomCreate: true, 15, isSpecial: false, 4, 36, -1, -1, -1, 2, 277, 1140, new List<short> { 78, 79, 80, 87, 88, 89 }));
		_dataArray.Add(new SkillBookItem(422, 844, 10, 1001, 7, 415, "icon_SkillBook_qingjiaoshenggugong", 845, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 15, 10, 21150, 42300, 6, 16, 9000, 8, allowRandomCreate: true, 10, isSpecial: false, 4, 36, -1, -1, -1, 2, 278, 1480, new List<short> { 79, 80, 88, 89 }));
		_dataArray.Add(new SkillBookItem(423, 846, 10, 1001, 8, 415, "icon_SkillBook_jiuseyuchanfa", 847, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 10, 10, 30750, 61500, 6, 18, 10800, 8, allowRandomCreate: true, 5, isSpecial: false, 4, 36, -1, -1, -1, 2, 279, 1860, new List<short> { 80, 89 }));
		_dataArray.Add(new SkillBookItem(424, 848, 10, 1001, 0, 424, "icon_SkillBook_suisuoyu", 849, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 50, 10, 150, 300, 0, 2, 600, 3, allowRandomCreate: true, 45, isSpecial: false, 4, 36, -1, -1, -1, 2, 280, 60, new List<short> { 117, 118, 119, 120, 121, 122, 123, 124, 125 }));
		_dataArray.Add(new SkillBookItem(425, 850, 10, 1001, 1, 424, "icon_SkillBook_suisuoyu", 851, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 45, 10, 300, 600, 0, 4, 1200, 4, allowRandomCreate: true, 40, isSpecial: false, 4, 36, -1, -1, -1, 2, 281, 130, new List<short> { 118, 119, 120, 121, 122, 123, 124, 125 }));
		_dataArray.Add(new SkillBookItem(426, 852, 10, 1001, 2, 424, "icon_SkillBook_suisuoyu", 853, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 40, 10, 900, 1800, 1, 6, 1800, 5, allowRandomCreate: true, 35, isSpecial: false, 4, 36, -1, -1, -1, 2, 282, 240, new List<short> { 119, 120, 121, 122, 123, 124, 125 }));
		_dataArray.Add(new SkillBookItem(427, 854, 10, 1001, 3, 424, "icon_SkillBook_qilunganyingfa", 855, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 35, 10, 2250, 4500, 2, 8, 3000, 6, allowRandomCreate: true, 30, isSpecial: false, 4, 36, -1, -1, -1, 2, 283, 400, new List<short> { 120, 121, 122, 123, 124, 125 }));
		_dataArray.Add(new SkillBookItem(428, 856, 10, 1001, 4, 424, "icon_SkillBook_qilunganyingfa", 857, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 30, 10, 4650, 9300, 3, 10, 4200, 7, allowRandomCreate: true, 25, isSpecial: false, 4, 36, -1, -1, -1, 2, 284, 600, new List<short> { 121, 122, 123, 124, 125 }));
		_dataArray.Add(new SkillBookItem(429, 858, 10, 1001, 5, 424, "icon_SkillBook_qilunganyingfa", 859, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 25, 10, 8400, 16800, 4, 12, 5400, 7, allowRandomCreate: true, 20, isSpecial: false, 4, 36, -1, -1, -1, 2, 285, 850, new List<short> { 122, 123, 124, 125 }));
		_dataArray.Add(new SkillBookItem(430, 860, 10, 1001, 6, 424, "icon_SkillBook_dashouyin", 861, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 20, 10, 13800, 27600, 5, 14, 7200, 8, allowRandomCreate: true, 15, isSpecial: false, 4, 36, -1, -1, -1, 2, 286, 1140, new List<short> { 123, 124, 125 }));
		_dataArray.Add(new SkillBookItem(431, 862, 10, 1001, 7, 424, "icon_SkillBook_jishenchengfo", 863, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 15, 10, 21150, 42300, 6, 16, 9000, 8, allowRandomCreate: true, 10, isSpecial: false, 4, 36, -1, -1, -1, 2, 287, 1480, new List<short> { 124, 125 }));
		_dataArray.Add(new SkillBookItem(432, 864, 10, 1001, 0, 432, "icon_SkillBook_michongxiang", 865, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 50, 10, 150, 300, 0, 2, 600, 3, allowRandomCreate: true, 45, isSpecial: false, 4, 36, -1, -1, -1, 2, 288, 60, new List<short> { 81, 82, 83, 84, 85, 86, 87, 88, 89 }));
		_dataArray.Add(new SkillBookItem(433, 866, 10, 1001, 1, 432, "icon_SkillBook_michongxiang", 867, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 45, 10, 300, 600, 0, 4, 1200, 4, allowRandomCreate: true, 40, isSpecial: false, 4, 36, -1, -1, -1, 2, 289, 130, new List<short> { 82, 83, 84, 85, 86, 87, 88, 89 }));
		_dataArray.Add(new SkillBookItem(434, 868, 10, 1001, 2, 432, "icon_SkillBook_michongxiang", 869, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 40, 10, 900, 1800, 1, 6, 1800, 5, allowRandomCreate: true, 35, isSpecial: false, 4, 36, -1, -1, -1, 2, 290, 240, new List<short> { 83, 84, 85, 86, 87, 88, 89 }));
		_dataArray.Add(new SkillBookItem(435, 870, 10, 1001, 3, 432, "icon_SkillBook_mieguzhou", 871, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 35, 10, 2250, 4500, 2, 8, 3000, 6, allowRandomCreate: true, 30, isSpecial: false, 4, 36, -1, -1, -1, 2, 291, 400, new List<short> { 84, 85, 86, 87, 88, 89 }));
		_dataArray.Add(new SkillBookItem(436, 872, 10, 1001, 4, 432, "icon_SkillBook_mieguzhou", 873, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 30, 10, 4650, 9300, 3, 10, 4200, 7, allowRandomCreate: true, 25, isSpecial: false, 4, 36, -1, -1, -1, 2, 292, 600, new List<short> { 85, 86, 87, 88, 89 }));
		_dataArray.Add(new SkillBookItem(437, 874, 10, 1001, 5, 432, "icon_SkillBook_mieguzhou", 875, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 25, 10, 8400, 16800, 4, 12, 5400, 7, allowRandomCreate: true, 20, isSpecial: false, 4, 36, -1, -1, -1, 2, 293, 850, new List<short> { 86, 87, 88, 89 }));
		_dataArray.Add(new SkillBookItem(438, 876, 10, 1001, 6, 432, "icon_SkillBook_tiancanshigu", 877, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 20, 10, 13800, 27600, 5, 14, 7200, 8, allowRandomCreate: true, 15, isSpecial: false, 4, 36, -1, -1, -1, 2, 294, 1140, new List<short> { 87, 88, 89 }));
		_dataArray.Add(new SkillBookItem(439, 878, 10, 1001, 7, 432, "icon_SkillBook_guijiangdafa", 879, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 15, 10, 21150, 42300, 6, 16, 9000, 8, allowRandomCreate: true, 10, isSpecial: false, 4, 36, -1, -1, -1, 2, 295, 1480, new List<short> { 88, 89 }));
		_dataArray.Add(new SkillBookItem(440, 880, 10, 1001, 8, 432, "icon_SkillBook_tianshehuangugong", 881, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 10, 10, 30750, 61500, 6, 18, 10800, 8, allowRandomCreate: true, 5, isSpecial: false, 4, 36, -1, -1, -1, 2, 296, 1860, new List<short> { 89 }));
		_dataArray.Add(new SkillBookItem(441, 882, 10, 1001, 0, 441, "icon_SkillBook_tianguantong", 883, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 50, 10, 150, 300, 0, 2, 600, 3, allowRandomCreate: true, 45, isSpecial: false, 4, 36, -1, -1, -1, 2, 297, 60, new List<short> { 36, 37, 38, 39, 40, 41, 42, 43, 44 }));
		_dataArray.Add(new SkillBookItem(442, 884, 10, 1001, 1, 441, "icon_SkillBook_tianguantong", 885, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 45, 10, 300, 600, 0, 4, 1200, 4, allowRandomCreate: true, 40, isSpecial: false, 4, 36, -1, -1, -1, 2, 298, 130, new List<short> { 37, 38, 39, 40, 41, 42, 43, 44 }));
		_dataArray.Add(new SkillBookItem(443, 886, 10, 1001, 2, 441, "icon_SkillBook_tianguantong", 887, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 40, 10, 900, 1800, 1, 6, 1800, 5, allowRandomCreate: true, 35, isSpecial: false, 4, 36, -1, -1, -1, 2, 299, 240, new List<short> { 38, 39, 40, 41, 42, 43, 44 }));
		_dataArray.Add(new SkillBookItem(444, 888, 10, 1001, 3, 441, "icon_SkillBook_hanbingbamai", 889, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 35, 10, 2250, 4500, 2, 8, 3000, 6, allowRandomCreate: true, 30, isSpecial: false, 4, 36, -1, -1, -1, 2, 300, 400, new List<short> { 39, 40, 41, 42, 43, 44 }));
		_dataArray.Add(new SkillBookItem(445, 890, 10, 1001, 4, 441, "icon_SkillBook_hanbingbamai", 891, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 30, 10, 4650, 9300, 3, 10, 4200, 7, allowRandomCreate: true, 25, isSpecial: false, 4, 36, -1, -1, -1, 2, 301, 600, new List<short> { 40, 41, 42, 43, 44 }));
		_dataArray.Add(new SkillBookItem(446, 892, 10, 1001, 5, 441, "icon_SkillBook_hanbingbamai", 893, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 25, 10, 8400, 16800, 4, 12, 5400, 7, allowRandomCreate: true, 20, isSpecial: false, 4, 36, -1, -1, -1, 2, 302, 850, new List<short> { 41, 42, 43, 44 }));
		_dataArray.Add(new SkillBookItem(447, 894, 10, 1001, 6, 441, "icon_SkillBook_qiyuanjieedafa", 895, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 20, 10, 13800, 27600, 5, 14, 7200, 8, allowRandomCreate: true, 15, isSpecial: false, 4, 36, -1, -1, -1, 2, 303, 1140, new List<short> { 42, 43, 44 }));
		_dataArray.Add(new SkillBookItem(448, 896, 10, 1001, 0, 448, "icon_SkillBook_nuxianggong", 897, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 50, 10, 150, 300, 0, 2, 600, 3, allowRandomCreate: true, 45, isSpecial: false, 4, 36, -1, -1, -1, 2, 304, 60, new List<short>
		{
			126, 127, 128, 129, 130, 131, 132, 133, 134, 45,
			46, 47, 48, 49, 50, 51, 52, 53
		}));
		_dataArray.Add(new SkillBookItem(449, 898, 10, 1001, 1, 448, "icon_SkillBook_nuxianggong", 899, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 45, 10, 300, 600, 0, 4, 1200, 4, allowRandomCreate: true, 40, isSpecial: false, 4, 36, -1, -1, -1, 2, 305, 130, new List<short>
		{
			127, 128, 129, 130, 131, 132, 133, 134, 46, 47,
			48, 49, 50, 51, 52, 53
		}));
		_dataArray.Add(new SkillBookItem(450, 900, 10, 1001, 2, 448, "icon_SkillBook_nuxianggong", 901, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 40, 10, 900, 1800, 1, 6, 1800, 5, allowRandomCreate: true, 35, isSpecial: false, 4, 36, -1, -1, -1, 2, 306, 240, new List<short>
		{
			128, 129, 130, 131, 132, 133, 134, 47, 48, 49,
			50, 51, 52, 53
		}));
		_dataArray.Add(new SkillBookItem(451, 902, 10, 1001, 3, 448, "icon_SkillBook_longchuangzhen", 903, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 35, 10, 2250, 4500, 2, 8, 3000, 6, allowRandomCreate: true, 30, isSpecial: false, 4, 36, -1, -1, -1, 2, 307, 400, new List<short>
		{
			129, 130, 131, 132, 133, 134, 48, 49, 50, 51,
			52, 53
		}));
		_dataArray.Add(new SkillBookItem(452, 904, 10, 1001, 4, 448, "icon_SkillBook_longchuangzhen", 905, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 30, 10, 4650, 9300, 3, 10, 4200, 7, allowRandomCreate: true, 25, isSpecial: false, 4, 36, -1, -1, -1, 2, 308, 600, new List<short> { 130, 131, 132, 133, 134, 49, 50, 51, 52, 53 }));
		_dataArray.Add(new SkillBookItem(453, 906, 10, 1001, 5, 448, "icon_SkillBook_longchuangzhen", 907, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 25, 10, 8400, 16800, 4, 12, 5400, 7, allowRandomCreate: true, 20, isSpecial: false, 4, 36, -1, -1, -1, 2, 309, 850, new List<short> { 131, 132, 133, 134, 50, 51, 52, 53 }));
		_dataArray.Add(new SkillBookItem(454, 908, 10, 1001, 6, 448, "icon_SkillBook_qiannianzui", 909, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 20, 10, 13800, 27600, 5, 14, 7200, 8, allowRandomCreate: true, 15, isSpecial: false, 4, 36, -1, -1, -1, 2, 310, 1140, new List<short> { 132, 133, 134, 51, 52, 53 }));
		_dataArray.Add(new SkillBookItem(455, 910, 10, 1001, 7, 448, "icon_SkillBook_zhetianbirigong", 911, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 15, 10, 21150, 42300, 6, 16, 9000, 8, allowRandomCreate: true, 10, isSpecial: false, 4, 36, -1, -1, -1, 2, 311, 1480, new List<short> { 133, 134, 52, 53 }));
		_dataArray.Add(new SkillBookItem(456, 912, 10, 1001, 0, 456, "icon_SkillBook_lannijueji", 913, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 50, 10, 150, 300, 0, 2, 600, 3, allowRandomCreate: true, 45, isSpecial: false, 4, 36, -1, -1, -1, 2, 312, 60, new List<short>
		{
			81, 82, 83, 84, 85, 86, 87, 88, 89, 135,
			136, 137, 138, 139, 140, 141, 142, 143
		}));
		_dataArray.Add(new SkillBookItem(457, 914, 10, 1001, 1, 456, "icon_SkillBook_lannijueji", 915, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 45, 10, 300, 600, 0, 4, 1200, 4, allowRandomCreate: true, 40, isSpecial: false, 4, 36, -1, -1, -1, 2, 313, 130, new List<short>
		{
			82, 83, 84, 85, 86, 87, 88, 89, 136, 137,
			138, 139, 140, 141, 142, 143
		}));
		_dataArray.Add(new SkillBookItem(458, 916, 10, 1001, 2, 456, "icon_SkillBook_lannijueji", 917, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 40, 10, 900, 1800, 1, 6, 1800, 5, allowRandomCreate: true, 35, isSpecial: false, 4, 36, -1, -1, -1, 2, 314, 240, new List<short>
		{
			83, 84, 85, 86, 87, 88, 89, 137, 138, 139,
			140, 141, 142, 143
		}));
		_dataArray.Add(new SkillBookItem(459, 918, 10, 1001, 3, 456, "icon_SkillBook_xiesigong", 919, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 35, 10, 2250, 4500, 2, 8, 3000, 6, allowRandomCreate: true, 30, isSpecial: false, 4, 36, -1, -1, -1, 2, 315, 400, new List<short>
		{
			84, 85, 86, 87, 88, 89, 138, 139, 140, 141,
			142, 143
		}));
		_dataArray.Add(new SkillBookItem(460, 920, 10, 1001, 4, 456, "icon_SkillBook_xiesigong", 921, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 30, 10, 4650, 9300, 3, 10, 4200, 7, allowRandomCreate: true, 25, isSpecial: false, 4, 36, -1, -1, -1, 2, 316, 600, new List<short> { 85, 86, 87, 88, 89, 139, 140, 141, 142, 143 }));
		_dataArray.Add(new SkillBookItem(461, 922, 10, 1001, 5, 456, "icon_SkillBook_xiesigong", 923, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 25, 10, 8400, 16800, 4, 12, 5400, 7, allowRandomCreate: true, 20, isSpecial: false, 4, 36, -1, -1, -1, 2, 317, 850, new List<short> { 86, 87, 88, 89, 140, 141, 142, 143 }));
		_dataArray.Add(new SkillBookItem(462, 924, 10, 1001, 6, 456, "icon_SkillBook_shierxietongdazhen", 925, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 20, 10, 13800, 27600, 5, 14, 7200, 8, allowRandomCreate: true, 15, isSpecial: false, 4, 36, -1, -1, -1, 2, 318, 1140, new List<short> { 87, 88, 89, 141, 142, 143 }));
		_dataArray.Add(new SkillBookItem(463, 926, 10, 1001, 7, 456, "icon_SkillBook_tianmotong", 927, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 15, 10, 21150, 42300, 6, 16, 9000, 8, allowRandomCreate: true, 10, isSpecial: false, 4, 36, -1, -1, -1, 2, 319, 1480, new List<short> { 88, 89, 142, 143 }));
		_dataArray.Add(new SkillBookItem(464, 928, 10, 1001, 8, 456, "icon_SkillBook_xietongbusishen", 929, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 10, 10, 30750, 61500, 6, 18, 10800, 8, allowRandomCreate: true, 5, isSpecial: false, 4, 36, -1, -1, -1, 2, 320, 1860, new List<short> { 89, 143 }));
		_dataArray.Add(new SkillBookItem(465, 930, 10, 1001, 0, 465, "icon_SkillBook_shaolinchangquan", 931, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 50, 10, 150, 300, 0, 2, 600, 3, allowRandomCreate: true, 45, isSpecial: false, 4, 36, -1, -1, -1, 3, 321, 60, new List<short> { 117, 118, 119, 120, 121, 122, 123, 124, 125 }));
		_dataArray.Add(new SkillBookItem(466, 932, 10, 1001, 1, 465, "icon_SkillBook_shaolinchangquan", 933, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 45, 10, 300, 600, 0, 4, 1200, 4, allowRandomCreate: true, 40, isSpecial: false, 4, 36, -1, -1, -1, 3, 322, 130, new List<short> { 118, 119, 120, 121, 122, 123, 124, 125 }));
		_dataArray.Add(new SkillBookItem(467, 934, 10, 1001, 2, 465, "icon_SkillBook_shaolinchangquan", 935, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 40, 10, 900, 1800, 1, 6, 1800, 5, allowRandomCreate: true, 35, isSpecial: false, 4, 36, -1, -1, -1, 3, 323, 240, new List<short> { 119, 120, 121, 122, 123, 124, 125 }));
		_dataArray.Add(new SkillBookItem(468, 936, 10, 1001, 3, 465, "icon_SkillBook_liushibashiluohanquan", 937, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 35, 10, 2250, 4500, 2, 8, 3000, 6, allowRandomCreate: true, 30, isSpecial: false, 4, 36, -1, -1, -1, 3, 324, 400, new List<short> { 120, 121, 122, 123, 124, 125 }));
		_dataArray.Add(new SkillBookItem(469, 938, 10, 1001, 4, 465, "icon_SkillBook_liushibashiluohanquan", 939, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 30, 10, 4650, 9300, 3, 10, 4200, 7, allowRandomCreate: true, 25, isSpecial: false, 4, 36, -1, -1, -1, 3, 325, 600, new List<short> { 121, 122, 123, 124, 125 }));
		_dataArray.Add(new SkillBookItem(470, 940, 10, 1001, 5, 465, "icon_SkillBook_liushibashiluohanquan", 941, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 25, 10, 8400, 16800, 4, 12, 5400, 7, allowRandomCreate: true, 20, isSpecial: false, 4, 36, -1, -1, -1, 3, 326, 850, new List<short> { 122, 123, 124, 125 }));
		_dataArray.Add(new SkillBookItem(471, 942, 10, 1001, 6, 465, "icon_SkillBook_jingangbanruozhang", 943, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 20, 10, 13800, 27600, 5, 14, 7200, 8, allowRandomCreate: true, 15, isSpecial: false, 4, 36, -1, -1, -1, 3, 327, 1140, new List<short> { 123, 124, 125 }));
		_dataArray.Add(new SkillBookItem(472, 944, 10, 1001, 7, 465, "icon_SkillBook_jiangmoguangmingquan", 945, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 15, 10, 21150, 42300, 6, 16, 9000, 8, allowRandomCreate: true, 10, isSpecial: false, 4, 36, -1, -1, -1, 3, 328, 1480, new List<short> { 124, 125 }));
		_dataArray.Add(new SkillBookItem(473, 946, 10, 1001, 0, 473, "icon_SkillBook_sanshiliubishou", 947, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 50, 10, 150, 300, 0, 2, 600, 3, allowRandomCreate: true, 45, isSpecial: false, 4, 36, -1, -1, -1, 3, 329, 60, new List<short>
		{
			108, 109, 110, 111, 112, 113, 114, 115, 116, 117,
			118, 119, 120, 121, 122, 123, 124, 125
		}));
		_dataArray.Add(new SkillBookItem(474, 948, 10, 1001, 1, 473, "icon_SkillBook_sanshiliubishou", 949, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 45, 10, 300, 600, 0, 4, 1200, 4, allowRandomCreate: true, 40, isSpecial: false, 4, 36, -1, -1, -1, 3, 330, 130, new List<short>
		{
			109, 110, 111, 112, 113, 114, 115, 116, 118, 119,
			120, 121, 122, 123, 124, 125
		}));
		_dataArray.Add(new SkillBookItem(475, 950, 10, 1001, 2, 473, "icon_SkillBook_sanshiliubishou", 951, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 40, 10, 900, 1800, 1, 6, 1800, 5, allowRandomCreate: true, 35, isSpecial: false, 4, 36, -1, -1, -1, 3, 331, 240, new List<short>
		{
			110, 111, 112, 113, 114, 115, 116, 119, 120, 121,
			122, 123, 124, 125
		}));
		_dataArray.Add(new SkillBookItem(476, 952, 10, 1001, 3, 473, "icon_SkillBook_baiyuantongbiquan", 953, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 35, 10, 2250, 4500, 2, 8, 3000, 6, allowRandomCreate: true, 30, isSpecial: false, 4, 36, -1, -1, -1, 3, 332, 400, new List<short>
		{
			111, 112, 113, 114, 115, 116, 120, 121, 122, 123,
			124, 125
		}));
		_dataArray.Add(new SkillBookItem(477, 954, 10, 1001, 4, 473, "icon_SkillBook_baiyuantongbiquan", 955, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 30, 10, 4650, 9300, 3, 10, 4200, 7, allowRandomCreate: true, 25, isSpecial: false, 4, 36, -1, -1, -1, 3, 333, 600, new List<short> { 112, 113, 114, 115, 116, 121, 122, 123, 124, 125 }));
		_dataArray.Add(new SkillBookItem(478, 956, 10, 1001, 5, 473, "icon_SkillBook_baiyuantongbiquan", 957, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 25, 10, 8400, 16800, 4, 12, 5400, 7, allowRandomCreate: true, 20, isSpecial: false, 4, 36, -1, -1, -1, 3, 334, 850, new List<short> { 113, 114, 115, 116, 122, 123, 124, 125 }));
		_dataArray.Add(new SkillBookItem(479, 958, 10, 1001, 6, 473, "icon_SkillBook_jingangsanmeizhang", 959, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 20, 10, 13800, 27600, 5, 14, 7200, 8, allowRandomCreate: true, 15, isSpecial: false, 4, 36, -1, -1, -1, 3, 335, 1140, new List<short> { 114, 115, 116, 123, 124, 125 }));
	}

	private void CreateItems8()
	{
		_dataArray.Add(new SkillBookItem(480, 960, 10, 1001, 0, 480, "icon_SkillBook_wudangchangquan", 961, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 50, 10, 150, 300, 0, 2, 600, 3, allowRandomCreate: true, 45, isSpecial: false, 4, 36, -1, -1, -1, 3, 336, 60, new List<short> { 108, 109, 110, 111, 112, 113, 114, 115, 116 }));
		_dataArray.Add(new SkillBookItem(481, 962, 10, 1001, 1, 480, "icon_SkillBook_wudangchangquan", 963, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 45, 10, 300, 600, 0, 4, 1200, 4, allowRandomCreate: true, 40, isSpecial: false, 4, 36, -1, -1, -1, 3, 337, 130, new List<short> { 109, 110, 111, 112, 113, 114, 115, 116 }));
		_dataArray.Add(new SkillBookItem(482, 964, 10, 1001, 2, 480, "icon_SkillBook_wudangchangquan", 965, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 40, 10, 900, 1800, 1, 6, 1800, 5, allowRandomCreate: true, 35, isSpecial: false, 4, 36, -1, -1, -1, 3, 338, 240, new List<short> { 110, 111, 112, 113, 114, 115, 116 }));
		_dataArray.Add(new SkillBookItem(483, 966, 10, 1001, 3, 480, "icon_SkillBook_youshenbaguazhang", 967, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 35, 10, 2250, 4500, 2, 8, 3000, 6, allowRandomCreate: true, 30, isSpecial: false, 4, 36, -1, -1, -1, 3, 339, 400, new List<short> { 111, 112, 113, 114, 115, 116 }));
		_dataArray.Add(new SkillBookItem(484, 968, 10, 1001, 4, 480, "icon_SkillBook_youshenbaguazhang", 969, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 30, 10, 4650, 9300, 3, 10, 4200, 7, allowRandomCreate: true, 25, isSpecial: false, 4, 36, -1, -1, -1, 3, 340, 600, new List<short> { 112, 113, 114, 115, 116 }));
		_dataArray.Add(new SkillBookItem(485, 970, 10, 1001, 5, 480, "icon_SkillBook_youshenbaguazhang", 971, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 25, 10, 8400, 16800, 4, 12, 5400, 7, allowRandomCreate: true, 20, isSpecial: false, 4, 36, -1, -1, -1, 3, 341, 850, new List<short> { 113, 114, 115, 116 }));
		_dataArray.Add(new SkillBookItem(486, 972, 10, 1001, 6, 480, "icon_SkillBook_wudangchunyangquan", 973, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 20, 10, 13800, 27600, 5, 14, 7200, 8, allowRandomCreate: true, 15, isSpecial: false, 4, 36, -1, -1, -1, 3, 342, 1140, new List<short> { 114, 115, 116 }));
		_dataArray.Add(new SkillBookItem(487, 974, 10, 1001, 7, 480, "icon_SkillBook_ziyangzhengqiquan", 975, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 15, 10, 21150, 42300, 6, 16, 9000, 8, allowRandomCreate: true, 10, isSpecial: false, 4, 36, -1, -1, -1, 3, 343, 1480, new List<short> { 115, 116 }));
		_dataArray.Add(new SkillBookItem(488, 976, 10, 1001, 8, 480, "icon_SkillBook_taijiquan", 977, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 10, 10, 30750, 61500, 6, 18, 10800, 8, allowRandomCreate: true, 5, isSpecial: false, 4, 36, -1, -1, -1, 3, 344, 1860, new List<short> { 116 }));
		_dataArray.Add(new SkillBookItem(489, 978, 10, 1001, 0, 489, "icon_SkillBook_tuishanzhang", 979, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 50, 10, 150, 300, 0, 2, 600, 3, allowRandomCreate: true, 45, isSpecial: false, 4, 36, -1, -1, -1, 3, 345, 60, new List<short>()));
		_dataArray.Add(new SkillBookItem(490, 980, 10, 1001, 1, 489, "icon_SkillBook_tuishanzhang", 981, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 45, 10, 300, 600, 0, 4, 1200, 4, allowRandomCreate: true, 40, isSpecial: false, 4, 36, -1, -1, -1, 3, 346, 130, new List<short>()));
		_dataArray.Add(new SkillBookItem(491, 982, 10, 1001, 2, 489, "icon_SkillBook_tuishanzhang", 983, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 40, 10, 900, 1800, 1, 6, 1800, 5, allowRandomCreate: true, 35, isSpecial: false, 4, 36, -1, -1, -1, 3, 347, 240, new List<short>()));
		_dataArray.Add(new SkillBookItem(492, 984, 10, 1001, 3, 489, "icon_SkillBook_dalikaibeizhang", 985, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 35, 10, 2250, 4500, 2, 8, 3000, 6, allowRandomCreate: true, 30, isSpecial: false, 4, 36, -1, -1, -1, 3, 348, 400, new List<short>()));
		_dataArray.Add(new SkillBookItem(493, 986, 10, 1001, 4, 489, "icon_SkillBook_dalikaibeizhang", 987, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 30, 10, 4650, 9300, 3, 10, 4200, 7, allowRandomCreate: true, 25, isSpecial: false, 4, 36, -1, -1, -1, 3, 349, 600, new List<short>()));
		_dataArray.Add(new SkillBookItem(494, 988, 10, 1001, 5, 489, "icon_SkillBook_dalikaibeizhang", 989, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 25, 10, 8400, 16800, 4, 12, 5400, 7, allowRandomCreate: true, 20, isSpecial: false, 4, 36, -1, -1, -1, 3, 350, 850, new List<short>()));
		_dataArray.Add(new SkillBookItem(495, 990, 10, 1001, 6, 489, "icon_SkillBook_yunkuangshi", 991, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 20, 10, 13800, 27600, 5, 14, 7200, 8, allowRandomCreate: true, 15, isSpecial: false, 4, 36, -1, -1, -1, 3, 351, 1140, new List<short>()));
		_dataArray.Add(new SkillBookItem(496, 992, 10, 1001, 7, 489, "icon_SkillBook_dashenweizhang", 993, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 15, 10, 21150, 42300, 6, 16, 9000, 8, allowRandomCreate: true, 10, isSpecial: false, 4, 36, -1, -1, -1, 3, 352, 1480, new List<short>()));
		_dataArray.Add(new SkillBookItem(497, 994, 10, 1001, 8, 489, "icon_SkillBook_dazhuoshou", 995, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 10, 10, 30750, 61500, 6, 18, 10800, 8, allowRandomCreate: true, 5, isSpecial: false, 4, 36, -1, -1, -1, 3, 353, 1860, new List<short>()));
		_dataArray.Add(new SkillBookItem(498, 996, 10, 1001, 0, 498, "icon_SkillBook_yunvdouluoshou", 997, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 50, 10, 150, 300, 0, 2, 600, 3, allowRandomCreate: true, 45, isSpecial: false, 4, 36, -1, -1, -1, 3, 354, 60, new List<short>
		{
			108, 109, 110, 111, 112, 113, 114, 115, 116, 27,
			28, 29, 30, 31, 32, 33, 34, 35
		}));
		_dataArray.Add(new SkillBookItem(499, 998, 10, 1001, 1, 498, "icon_SkillBook_yunvdouluoshou", 999, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 45, 10, 300, 600, 0, 4, 1200, 4, allowRandomCreate: true, 40, isSpecial: false, 4, 36, -1, -1, -1, 3, 355, 130, new List<short>
		{
			109, 110, 111, 112, 113, 114, 115, 116, 28, 29,
			30, 31, 32, 33, 34, 35
		}));
		_dataArray.Add(new SkillBookItem(500, 1000, 10, 1001, 2, 498, "icon_SkillBook_yunvdouluoshou", 1001, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 40, 10, 900, 1800, 1, 6, 1800, 5, allowRandomCreate: true, 35, isSpecial: false, 4, 36, -1, -1, -1, 3, 356, 240, new List<short>
		{
			110, 111, 112, 113, 114, 115, 116, 29, 30, 31,
			32, 33, 34, 35
		}));
		_dataArray.Add(new SkillBookItem(501, 1002, 10, 1001, 3, 498, "icon_SkillBook_xiaojiutianjiushi", 1003, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 35, 10, 2250, 4500, 2, 8, 3000, 6, allowRandomCreate: true, 30, isSpecial: false, 4, 36, -1, -1, -1, 3, 357, 400, new List<short>
		{
			111, 112, 113, 114, 115, 116, 30, 31, 32, 33,
			34, 35
		}));
		_dataArray.Add(new SkillBookItem(502, 1004, 10, 1001, 4, 498, "icon_SkillBook_xiaojiutianjiushi", 1005, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 30, 10, 4650, 9300, 3, 10, 4200, 7, allowRandomCreate: true, 25, isSpecial: false, 4, 36, -1, -1, -1, 3, 358, 600, new List<short> { 112, 113, 114, 115, 116, 31, 32, 33, 34, 35 }));
		_dataArray.Add(new SkillBookItem(503, 1006, 10, 1001, 5, 498, "icon_SkillBook_xiaojiutianjiushi", 1007, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 25, 10, 8400, 16800, 4, 12, 5400, 7, allowRandomCreate: true, 20, isSpecial: false, 4, 36, -1, -1, -1, 3, 359, 850, new List<short> { 113, 114, 115, 116, 32, 33, 34, 35 }));
		_dataArray.Add(new SkillBookItem(504, 1008, 10, 1001, 6, 498, "icon_SkillBook_hanbingshenzhang", 1009, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 20, 10, 13800, 27600, 5, 14, 7200, 8, allowRandomCreate: true, 15, isSpecial: false, 4, 36, -1, -1, -1, 3, 360, 1140, new List<short> { 114, 115, 116, 33, 34, 35 }));
		_dataArray.Add(new SkillBookItem(505, 1010, 10, 1001, 0, 505, "icon_SkillBook_chaishanwuqinquan", 1011, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 50, 10, 150, 300, 0, 2, 600, 3, allowRandomCreate: true, 45, isSpecial: false, 4, 36, -1, -1, -1, 3, 361, 60, new List<short>
		{
			72, 73, 74, 75, 76, 77, 78, 79, 80, 81,
			82, 83, 84, 85, 86, 87, 88, 89
		}));
		_dataArray.Add(new SkillBookItem(506, 1012, 10, 1001, 1, 505, "icon_SkillBook_chaishanwuqinquan", 1013, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 45, 10, 300, 600, 0, 4, 1200, 4, allowRandomCreate: true, 40, isSpecial: false, 4, 36, -1, -1, -1, 3, 362, 130, new List<short>
		{
			73, 74, 75, 76, 77, 78, 79, 80, 82, 83,
			84, 85, 86, 87, 88, 89
		}));
		_dataArray.Add(new SkillBookItem(507, 1014, 10, 1001, 2, 505, "icon_SkillBook_chaishanwuqinquan", 1015, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 40, 10, 900, 1800, 1, 6, 1800, 5, allowRandomCreate: true, 35, isSpecial: false, 4, 36, -1, -1, -1, 3, 363, 240, new List<short>
		{
			74, 75, 76, 77, 78, 79, 80, 83, 84, 85,
			86, 87, 88, 89
		}));
		_dataArray.Add(new SkillBookItem(508, 1016, 10, 1001, 3, 505, "icon_SkillBook_weilingxianhuaguzhang", 1017, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 35, 10, 2250, 4500, 2, 8, 3000, 6, allowRandomCreate: true, 30, isSpecial: false, 4, 36, -1, -1, -1, 3, 364, 400, new List<short>
		{
			75, 76, 77, 78, 79, 80, 84, 85, 86, 87,
			88, 89
		}));
		_dataArray.Add(new SkillBookItem(509, 1018, 10, 1001, 4, 505, "icon_SkillBook_weilingxianhuaguzhang", 1019, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 30, 10, 4650, 9300, 3, 10, 4200, 7, allowRandomCreate: true, 25, isSpecial: false, 4, 36, -1, -1, -1, 3, 365, 600, new List<short> { 76, 77, 78, 79, 80, 85, 86, 87, 88, 89 }));
		_dataArray.Add(new SkillBookItem(510, 1020, 10, 1001, 5, 505, "icon_SkillBook_weilingxianhuaguzhang", 1021, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 25, 10, 8400, 16800, 4, 12, 5400, 7, allowRandomCreate: true, 20, isSpecial: false, 4, 36, -1, -1, -1, 3, 366, 850, new List<short> { 77, 78, 79, 80, 86, 87, 88, 89 }));
		_dataArray.Add(new SkillBookItem(511, 1022, 10, 1001, 0, 511, "icon_SkillBook_jingangchui", 1023, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 50, 10, 150, 300, 0, 2, 600, 3, allowRandomCreate: true, 45, isSpecial: false, 4, 36, -1, -1, -1, 3, 367, 60, new List<short> { 117, 118, 119, 120, 121, 122, 123, 124, 125 }));
		_dataArray.Add(new SkillBookItem(512, 1024, 10, 1001, 1, 511, "icon_SkillBook_jingangchui", 1025, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 45, 10, 300, 600, 0, 4, 1200, 4, allowRandomCreate: true, 40, isSpecial: false, 4, 36, -1, -1, -1, 3, 368, 130, new List<short> { 118, 119, 120, 121, 122, 123, 124, 125 }));
		_dataArray.Add(new SkillBookItem(513, 1026, 10, 1001, 2, 511, "icon_SkillBook_jingangchui", 1027, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 40, 10, 900, 1800, 1, 6, 1800, 5, allowRandomCreate: true, 35, isSpecial: false, 4, 36, -1, -1, -1, 3, 369, 240, new List<short> { 119, 120, 121, 122, 123, 124, 125 }));
		_dataArray.Add(new SkillBookItem(514, 1028, 10, 1001, 3, 511, "icon_SkillBook_jingangheishazhang", 1029, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 35, 10, 2250, 4500, 2, 8, 3000, 6, allowRandomCreate: true, 30, isSpecial: false, 4, 36, -1, -1, -1, 3, 370, 400, new List<short> { 120, 121, 122, 123, 124, 125 }));
		_dataArray.Add(new SkillBookItem(515, 1030, 10, 1001, 4, 511, "icon_SkillBook_jingangheishazhang", 1031, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 30, 10, 4650, 9300, 3, 10, 4200, 7, allowRandomCreate: true, 25, isSpecial: false, 4, 36, -1, -1, -1, 3, 371, 600, new List<short> { 121, 122, 123, 124, 125 }));
		_dataArray.Add(new SkillBookItem(516, 1032, 10, 1001, 5, 511, "icon_SkillBook_jingangheishazhang", 1033, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 25, 10, 8400, 16800, 4, 12, 5400, 7, allowRandomCreate: true, 20, isSpecial: false, 4, 36, -1, -1, -1, 3, 372, 850, new List<short> { 122, 123, 124, 125 }));
		_dataArray.Add(new SkillBookItem(517, 1034, 10, 1001, 6, 511, "icon_SkillBook_xumishansishenzhang", 1035, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 20, 10, 13800, 27600, 5, 14, 7200, 8, allowRandomCreate: true, 15, isSpecial: false, 4, 36, -1, -1, -1, 3, 373, 1140, new List<short> { 123, 124, 125 }));
		_dataArray.Add(new SkillBookItem(518, 1036, 10, 1001, 7, 511, "icon_SkillBook_darirulaizhang", 1037, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 15, 10, 21150, 42300, 6, 16, 9000, 8, allowRandomCreate: true, 10, isSpecial: false, 4, 36, -1, -1, -1, 3, 374, 1480, new List<short> { 124, 125 }));
		_dataArray.Add(new SkillBookItem(519, 1038, 10, 1001, 0, 519, "icon_SkillBook_yinfengxiezishou", 1039, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 50, 10, 150, 300, 0, 2, 600, 3, allowRandomCreate: true, 45, isSpecial: false, 4, 36, -1, -1, -1, 3, 375, 60, new List<short> { 81, 82, 83, 84, 85, 86, 87, 88, 89 }));
		_dataArray.Add(new SkillBookItem(520, 1040, 10, 1001, 1, 519, "icon_SkillBook_yinfengxiezishou", 1041, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 45, 10, 300, 600, 0, 4, 1200, 4, allowRandomCreate: true, 40, isSpecial: false, 4, 36, -1, -1, -1, 3, 376, 130, new List<short> { 82, 83, 84, 85, 86, 87, 88, 89 }));
		_dataArray.Add(new SkillBookItem(521, 1042, 10, 1001, 2, 519, "icon_SkillBook_yinfengxiezishou", 1043, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 40, 10, 900, 1800, 1, 6, 1800, 5, allowRandomCreate: true, 35, isSpecial: false, 4, 36, -1, -1, -1, 3, 377, 240, new List<short> { 83, 84, 85, 86, 87, 88, 89 }));
		_dataArray.Add(new SkillBookItem(522, 1044, 10, 1001, 3, 519, "icon_SkillBook_sanshiduanhunzhang", 1045, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 35, 10, 2250, 4500, 2, 8, 3000, 6, allowRandomCreate: true, 30, isSpecial: false, 4, 36, -1, -1, -1, 3, 378, 400, new List<short> { 84, 85, 86, 87, 88, 89 }));
		_dataArray.Add(new SkillBookItem(523, 1046, 10, 1001, 4, 519, "icon_SkillBook_sanshiduanhunzhang", 1047, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 30, 10, 4650, 9300, 3, 10, 4200, 7, allowRandomCreate: true, 25, isSpecial: false, 4, 36, -1, -1, -1, 3, 379, 600, new List<short> { 85, 86, 87, 88, 89 }));
		_dataArray.Add(new SkillBookItem(524, 1048, 10, 1001, 5, 519, "icon_SkillBook_sanshiduanhunzhang", 1049, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 25, 10, 8400, 16800, 4, 12, 5400, 7, allowRandomCreate: true, 20, isSpecial: false, 4, 36, -1, -1, -1, 3, 380, 850, new List<short> { 86, 87, 88, 89 }));
		_dataArray.Add(new SkillBookItem(525, 1050, 10, 1001, 6, 519, "icon_SkillBook_qinghuangchiheishenzhang", 1051, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 20, 10, 13800, 27600, 5, 14, 7200, 8, allowRandomCreate: true, 15, isSpecial: false, 4, 36, -1, -1, -1, 3, 381, 1140, new List<short> { 87, 88, 89 }));
		_dataArray.Add(new SkillBookItem(526, 1052, 10, 1001, 7, 519, "icon_SkillBook_jiusilihunshou", 1053, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 15, 10, 21150, 42300, 6, 16, 9000, 8, allowRandomCreate: true, 10, isSpecial: false, 4, 36, -1, -1, -1, 3, 382, 1480, new List<short> { 88, 89 }));
		_dataArray.Add(new SkillBookItem(527, 1054, 10, 1001, 0, 527, "icon_SkillBook_duanguquan", 1055, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 50, 10, 150, 300, 0, 2, 600, 3, allowRandomCreate: true, 45, isSpecial: false, 4, 36, -1, -1, -1, 3, 383, 60, new List<short>
		{
			126, 127, 128, 129, 130, 131, 132, 133, 134, 45,
			46, 47, 48, 49, 50, 51, 52, 53
		}));
		_dataArray.Add(new SkillBookItem(528, 1056, 10, 1001, 1, 527, "icon_SkillBook_duanguquan", 1057, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 45, 10, 300, 600, 0, 4, 1200, 4, allowRandomCreate: true, 40, isSpecial: false, 4, 36, -1, -1, -1, 3, 384, 130, new List<short>
		{
			127, 128, 129, 130, 131, 132, 133, 134, 46, 47,
			48, 49, 50, 51, 52, 53
		}));
		_dataArray.Add(new SkillBookItem(529, 1058, 10, 1001, 2, 527, "icon_SkillBook_duanguquan", 1059, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 40, 10, 900, 1800, 1, 6, 1800, 5, allowRandomCreate: true, 35, isSpecial: false, 4, 36, -1, -1, -1, 3, 385, 240, new List<short>
		{
			128, 129, 130, 131, 132, 133, 134, 47, 48, 49,
			50, 51, 52, 53
		}));
		_dataArray.Add(new SkillBookItem(530, 1060, 10, 1001, 3, 527, "icon_SkillBook_fengmozuiquan", 1061, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 35, 10, 2250, 4500, 2, 8, 3000, 6, allowRandomCreate: true, 30, isSpecial: false, 4, 36, -1, -1, -1, 3, 386, 400, new List<short>
		{
			129, 130, 131, 132, 133, 134, 48, 49, 50, 51,
			52, 53
		}));
		_dataArray.Add(new SkillBookItem(531, 1062, 10, 1001, 4, 527, "icon_SkillBook_fengmozuiquan", 1063, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 30, 10, 4650, 9300, 3, 10, 4200, 7, allowRandomCreate: true, 25, isSpecial: false, 4, 36, -1, -1, -1, 3, 387, 600, new List<short> { 130, 131, 132, 133, 134, 49, 50, 51, 52, 53 }));
		_dataArray.Add(new SkillBookItem(532, 1064, 10, 1001, 5, 527, "icon_SkillBook_fengmozuiquan", 1065, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 25, 10, 8400, 16800, 4, 12, 5400, 7, allowRandomCreate: true, 20, isSpecial: false, 4, 36, -1, -1, -1, 3, 388, 850, new List<short> { 131, 132, 133, 134, 50, 51, 52, 53 }));
		_dataArray.Add(new SkillBookItem(533, 1066, 10, 1001, 6, 527, "icon_SkillBook_lihuoliuyangzhang", 1067, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 20, 10, 13800, 27600, 5, 14, 7200, 8, allowRandomCreate: true, 15, isSpecial: false, 4, 36, -1, -1, -1, 3, 389, 1140, new List<short> { 132, 133, 134, 51, 52, 53 }));
		_dataArray.Add(new SkillBookItem(534, 1068, 10, 1001, 7, 527, "icon_SkillBook_dayuyangshenquan", 1069, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 15, 10, 21150, 42300, 6, 16, 9000, 8, allowRandomCreate: true, 10, isSpecial: false, 4, 36, -1, -1, -1, 3, 390, 1480, new List<short> { 133, 134, 52, 53 }));
		_dataArray.Add(new SkillBookItem(535, 1070, 10, 1001, 8, 527, "icon_SkillBook_hualongzhang", 1071, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 10, 10, 30750, 61500, 6, 18, 10800, 8, allowRandomCreate: true, 5, isSpecial: false, 4, 36, -1, -1, -1, 3, 391, 1860, new List<short> { 134, 53 }));
		_dataArray.Add(new SkillBookItem(536, 1072, 10, 1001, 0, 536, "icon_SkillBook_fenggouquan", 1073, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 50, 10, 150, 300, 0, 2, 600, 3, allowRandomCreate: true, 45, isSpecial: false, 4, 36, -1, -1, -1, 3, 392, 60, new List<short>
		{
			81, 82, 83, 84, 85, 86, 87, 88, 89, 135,
			136, 137, 138, 139, 140, 141, 142, 143
		}));
		_dataArray.Add(new SkillBookItem(537, 1074, 10, 1001, 1, 536, "icon_SkillBook_fenggouquan", 1075, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 45, 10, 300, 600, 0, 4, 1200, 4, allowRandomCreate: true, 40, isSpecial: false, 4, 36, -1, -1, -1, 3, 393, 130, new List<short>
		{
			82, 83, 84, 85, 86, 87, 88, 89, 136, 137,
			138, 139, 140, 141, 142, 143
		}));
		_dataArray.Add(new SkillBookItem(538, 1076, 10, 1001, 2, 536, "icon_SkillBook_fenggouquan", 1077, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 40, 10, 900, 1800, 1, 6, 1800, 5, allowRandomCreate: true, 35, isSpecial: false, 4, 36, -1, -1, -1, 3, 394, 240, new List<short>
		{
			83, 84, 85, 86, 87, 88, 89, 137, 138, 139,
			140, 141, 142, 143
		}));
		_dataArray.Add(new SkillBookItem(539, 1078, 10, 1001, 3, 536, "icon_SkillBook_fushiduzhang", 1079, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 35, 10, 2250, 4500, 2, 8, 3000, 6, allowRandomCreate: true, 30, isSpecial: false, 4, 36, -1, -1, -1, 3, 395, 400, new List<short>
		{
			84, 85, 86, 87, 88, 89, 138, 139, 140, 141,
			142, 143
		}));
	}

	private void CreateItems9()
	{
		_dataArray.Add(new SkillBookItem(540, 1080, 10, 1001, 4, 536, "icon_SkillBook_fushiduzhang", 1081, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 30, 10, 4650, 9300, 3, 10, 4200, 7, allowRandomCreate: true, 25, isSpecial: false, 4, 36, -1, -1, -1, 3, 396, 600, new List<short> { 85, 86, 87, 88, 89, 139, 140, 141, 142, 143 }));
		_dataArray.Add(new SkillBookItem(541, 1082, 10, 1001, 5, 536, "icon_SkillBook_fushiduzhang", 1083, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 25, 10, 8400, 16800, 4, 12, 5400, 7, allowRandomCreate: true, 20, isSpecial: false, 4, 36, -1, -1, -1, 3, 397, 850, new List<short> { 86, 87, 88, 89, 140, 141, 142, 143 }));
		_dataArray.Add(new SkillBookItem(542, 1084, 10, 1001, 6, 536, "icon_SkillBook_tianmocanhunzhang", 1085, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 20, 10, 13800, 27600, 5, 14, 7200, 8, allowRandomCreate: true, 15, isSpecial: false, 4, 36, -1, -1, -1, 3, 398, 1140, new List<short> { 87, 88, 89, 141, 142, 143 }));
		_dataArray.Add(new SkillBookItem(543, 1086, 10, 1001, 7, 536, "icon_SkillBook_xiehoumozhang", 1087, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 15, 10, 21150, 42300, 6, 16, 9000, 8, allowRandomCreate: true, 10, isSpecial: false, 4, 36, -1, -1, -1, 3, 399, 1480, new List<short> { 88, 89, 142, 143 }));
		_dataArray.Add(new SkillBookItem(544, 1088, 10, 1001, 0, 544, "icon_SkillBook_shaolinfuhuzhao", 1089, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 50, 10, 150, 300, 0, 2, 600, 3, allowRandomCreate: true, 45, isSpecial: false, 4, 36, -1, -1, -1, 4, 400, 60, new List<short> { 117, 118, 119, 120, 121, 122, 123, 124, 125 }));
		_dataArray.Add(new SkillBookItem(545, 1090, 10, 1001, 1, 544, "icon_SkillBook_shaolinfuhuzhao", 1091, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 45, 10, 300, 600, 0, 4, 1200, 4, allowRandomCreate: true, 40, isSpecial: false, 4, 36, -1, -1, -1, 4, 401, 130, new List<short> { 118, 119, 120, 121, 122, 123, 124, 125 }));
		_dataArray.Add(new SkillBookItem(546, 1092, 10, 1001, 2, 544, "icon_SkillBook_shaolinfuhuzhao", 1093, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 40, 10, 900, 1800, 1, 6, 1800, 5, allowRandomCreate: true, 35, isSpecial: false, 4, 36, -1, -1, -1, 4, 402, 240, new List<short> { 119, 120, 121, 122, 123, 124, 125 }));
		_dataArray.Add(new SkillBookItem(547, 1094, 10, 1001, 3, 544, "icon_SkillBook_dalijingangzhi", 1095, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 35, 10, 2250, 4500, 2, 8, 3000, 6, allowRandomCreate: true, 30, isSpecial: false, 4, 36, -1, -1, -1, 4, 403, 400, new List<short> { 120, 121, 122, 123, 124, 125 }));
		_dataArray.Add(new SkillBookItem(548, 1096, 10, 1001, 4, 544, "icon_SkillBook_dalijingangzhi", 1097, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 30, 10, 4650, 9300, 3, 10, 4200, 7, allowRandomCreate: true, 25, isSpecial: false, 4, 36, -1, -1, -1, 4, 404, 600, new List<short> { 121, 122, 123, 124, 125 }));
		_dataArray.Add(new SkillBookItem(549, 1098, 10, 1001, 5, 544, "icon_SkillBook_dalijingangzhi", 1099, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 25, 10, 8400, 16800, 4, 12, 5400, 7, allowRandomCreate: true, 20, isSpecial: false, 4, 36, -1, -1, -1, 4, 405, 850, new List<short> { 122, 123, 124, 125 }));
		_dataArray.Add(new SkillBookItem(550, 1100, 10, 1001, 0, 550, "icon_SkillBook_emeiyingzhaogong", 1101, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 50, 10, 150, 300, 0, 2, 600, 3, allowRandomCreate: true, 45, isSpecial: false, 4, 36, -1, -1, -1, 4, 406, 60, new List<short>
		{
			108, 109, 110, 111, 112, 113, 114, 115, 116, 117,
			118, 119, 120, 121, 122, 123, 124, 125
		}));
		_dataArray.Add(new SkillBookItem(551, 1102, 10, 1001, 1, 550, "icon_SkillBook_emeiyingzhaogong", 1103, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 45, 10, 300, 600, 0, 4, 1200, 4, allowRandomCreate: true, 40, isSpecial: false, 4, 36, -1, -1, -1, 4, 407, 130, new List<short>
		{
			109, 110, 111, 112, 113, 114, 115, 116, 118, 119,
			120, 121, 122, 123, 124, 125
		}));
		_dataArray.Add(new SkillBookItem(552, 1104, 10, 1001, 2, 550, "icon_SkillBook_emeiyingzhaogong", 1105, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 40, 10, 900, 1800, 1, 6, 1800, 5, allowRandomCreate: true, 35, isSpecial: false, 4, 36, -1, -1, -1, 4, 408, 240, new List<short>
		{
			110, 111, 112, 113, 114, 115, 116, 119, 120, 121,
			122, 123, 124, 125
		}));
		_dataArray.Add(new SkillBookItem(553, 1106, 10, 1001, 3, 550, "icon_SkillBook_fenhuafuliushi", 1107, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 35, 10, 2250, 4500, 2, 8, 3000, 6, allowRandomCreate: true, 30, isSpecial: false, 4, 36, -1, -1, -1, 4, 409, 400, new List<short>
		{
			111, 112, 113, 114, 115, 116, 120, 121, 122, 123,
			124, 125
		}));
		_dataArray.Add(new SkillBookItem(554, 1108, 10, 1001, 4, 550, "icon_SkillBook_fenhuafuliushi", 1109, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 30, 10, 4650, 9300, 3, 10, 4200, 7, allowRandomCreate: true, 25, isSpecial: false, 4, 36, -1, -1, -1, 4, 410, 600, new List<short> { 112, 113, 114, 115, 116, 121, 122, 123, 124, 125 }));
		_dataArray.Add(new SkillBookItem(555, 1110, 10, 1001, 5, 550, "icon_SkillBook_fenhuafuliushi", 1111, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 25, 10, 8400, 16800, 4, 12, 5400, 7, allowRandomCreate: true, 20, isSpecial: false, 4, 36, -1, -1, -1, 4, 411, 850, new List<short> { 113, 114, 115, 116, 122, 123, 124, 125 }));
		_dataArray.Add(new SkillBookItem(556, 1112, 10, 1001, 6, 550, "icon_SkillBook_daguangmingshanyiyuanzhi", 1113, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 20, 10, 13800, 27600, 5, 14, 7200, 8, allowRandomCreate: true, 15, isSpecial: false, 4, 36, -1, -1, -1, 4, 412, 1140, new List<short> { 114, 115, 116, 123, 124, 125 }));
		_dataArray.Add(new SkillBookItem(557, 1114, 10, 1001, 7, 550, "icon_SkillBook_tiangangzhixuefa", 1115, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 15, 10, 21150, 42300, 6, 16, 9000, 8, allowRandomCreate: true, 10, isSpecial: false, 4, 36, -1, -1, -1, 4, 413, 1480, new List<short> { 115, 116, 124, 125 }));
		_dataArray.Add(new SkillBookItem(558, 1116, 10, 1001, 0, 558, "icon_SkillBook_zhizhengong", 1117, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 50, 10, 150, 300, 0, 2, 600, 3, allowRandomCreate: true, 45, isSpecial: false, 4, 36, -1, -1, -1, 4, 414, 60, new List<short> { 72, 73, 74, 75, 76, 77, 78, 79, 80 }));
		_dataArray.Add(new SkillBookItem(559, 1118, 10, 1001, 1, 558, "icon_SkillBook_zhizhengong", 1119, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 45, 10, 300, 600, 0, 4, 1200, 4, allowRandomCreate: true, 40, isSpecial: false, 4, 36, -1, -1, -1, 4, 415, 130, new List<short> { 73, 74, 75, 76, 77, 78, 79, 80 }));
		_dataArray.Add(new SkillBookItem(560, 1120, 10, 1001, 2, 558, "icon_SkillBook_zhizhengong", 1121, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 40, 10, 900, 1800, 1, 6, 1800, 5, allowRandomCreate: true, 35, isSpecial: false, 4, 36, -1, -1, -1, 4, 416, 240, new List<short> { 74, 75, 76, 77, 78, 79, 80 }));
		_dataArray.Add(new SkillBookItem(561, 1122, 10, 1001, 3, 558, "icon_SkillBook_qinghuayumeiren", 1123, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 35, 10, 2250, 4500, 2, 8, 3000, 6, allowRandomCreate: true, 30, isSpecial: false, 4, 36, -1, -1, -1, 4, 417, 400, new List<short> { 75, 76, 77, 78, 79, 80 }));
		_dataArray.Add(new SkillBookItem(562, 1124, 10, 1001, 4, 558, "icon_SkillBook_qinghuayumeiren", 1125, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 30, 10, 4650, 9300, 3, 10, 4200, 7, allowRandomCreate: true, 25, isSpecial: false, 4, 36, -1, -1, -1, 4, 418, 600, new List<short> { 76, 77, 78, 79, 80 }));
		_dataArray.Add(new SkillBookItem(563, 1126, 10, 1001, 5, 558, "icon_SkillBook_qinghuayumeiren", 1127, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 25, 10, 8400, 16800, 4, 12, 5400, 7, allowRandomCreate: true, 20, isSpecial: false, 4, 36, -1, -1, -1, 4, 419, 850, new List<short> { 77, 78, 79, 80 }));
		_dataArray.Add(new SkillBookItem(564, 1128, 10, 1001, 6, 558, "icon_SkillBook_wanhualingyuezhi", 1129, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 20, 10, 13800, 27600, 5, 14, 7200, 8, allowRandomCreate: true, 15, isSpecial: false, 4, 36, -1, -1, -1, 4, 420, 1140, new List<short> { 78, 79, 80 }));
		_dataArray.Add(new SkillBookItem(565, 1130, 10, 1001, 7, 558, "icon_SkillBook_baihuasha", 1131, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 15, 10, 21150, 42300, 6, 16, 9000, 8, allowRandomCreate: true, 10, isSpecial: false, 4, 36, -1, -1, -1, 4, 421, 1480, new List<short> { 79, 80 }));
		_dataArray.Add(new SkillBookItem(566, 1132, 10, 1001, 8, 558, "icon_SkillBook_xiezhuhuabafa", 1133, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 10, 10, 30750, 61500, 6, 18, 10800, 8, allowRandomCreate: true, 5, isSpecial: false, 4, 36, -1, -1, -1, 4, 422, 1860, new List<short> { 80 }));
		_dataArray.Add(new SkillBookItem(567, 1134, 10, 1001, 0, 567, "icon_SkillBook_sanqingzhi", 1135, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 50, 10, 150, 300, 0, 2, 600, 3, allowRandomCreate: true, 45, isSpecial: false, 4, 36, -1, -1, -1, 4, 423, 60, new List<short>
		{
			36, 37, 38, 39, 40, 41, 42, 43, 44, 135,
			136, 137, 138, 139, 140, 141, 142, 143
		}));
		_dataArray.Add(new SkillBookItem(568, 1136, 10, 1001, 1, 567, "icon_SkillBook_sanqingzhi", 1137, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 45, 10, 300, 600, 0, 4, 1200, 4, allowRandomCreate: true, 40, isSpecial: false, 4, 36, -1, -1, -1, 4, 424, 130, new List<short>
		{
			37, 38, 39, 40, 41, 42, 43, 44, 136, 137,
			138, 139, 140, 141, 142, 143
		}));
		_dataArray.Add(new SkillBookItem(569, 1138, 10, 1001, 2, 567, "icon_SkillBook_sanqingzhi", 1139, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 40, 10, 900, 1800, 1, 6, 1800, 5, allowRandomCreate: true, 35, isSpecial: false, 4, 36, -1, -1, -1, 4, 425, 240, new List<short>
		{
			38, 39, 40, 41, 42, 43, 44, 137, 138, 139,
			140, 141, 142, 143
		}));
		_dataArray.Add(new SkillBookItem(570, 1140, 10, 1001, 3, 567, "icon_SkillBook_chishushi", 1141, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 35, 10, 2250, 4500, 2, 8, 3000, 6, allowRandomCreate: true, 30, isSpecial: false, 4, 36, -1, -1, -1, 4, 426, 400, new List<short>
		{
			39, 40, 41, 42, 43, 44, 138, 139, 140, 141,
			142, 143
		}));
		_dataArray.Add(new SkillBookItem(571, 1142, 10, 1001, 4, 567, "icon_SkillBook_chishushi", 1143, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 30, 10, 4650, 9300, 3, 10, 4200, 7, allowRandomCreate: true, 25, isSpecial: false, 4, 36, -1, -1, -1, 4, 427, 600, new List<short> { 40, 41, 42, 43, 44, 139, 140, 141, 142, 143 }));
		_dataArray.Add(new SkillBookItem(572, 1144, 10, 1001, 5, 567, "icon_SkillBook_chishushi", 1145, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 25, 10, 8400, 16800, 4, 12, 5400, 7, allowRandomCreate: true, 20, isSpecial: false, 4, 36, -1, -1, -1, 4, 428, 850, new List<short> { 41, 42, 43, 44, 140, 141, 142, 143 }));
		_dataArray.Add(new SkillBookItem(573, 1146, 10, 1001, 6, 567, "icon_SkillBook_tiandiyuanyizhi", 1147, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 20, 10, 13800, 27600, 5, 14, 7200, 8, allowRandomCreate: true, 15, isSpecial: false, 4, 36, -1, -1, -1, 4, 429, 1140, new List<short> { 42, 43, 44, 141, 142, 143 }));
		_dataArray.Add(new SkillBookItem(574, 1148, 10, 1001, 0, 574, "icon_SkillBook_yixuanshou", 1149, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 50, 10, 150, 300, 0, 2, 600, 3, allowRandomCreate: true, 45, isSpecial: false, 4, 36, -1, -1, -1, 4, 430, 60, new List<short>
		{
			108, 109, 110, 111, 112, 113, 114, 115, 116, 27,
			28, 29, 30, 31, 32, 33, 34, 35
		}));
		_dataArray.Add(new SkillBookItem(575, 1150, 10, 1001, 1, 574, "icon_SkillBook_yixuanshou", 1151, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 45, 10, 300, 600, 0, 4, 1200, 4, allowRandomCreate: true, 40, isSpecial: false, 4, 36, -1, -1, -1, 4, 431, 130, new List<short>
		{
			109, 110, 111, 112, 113, 114, 115, 116, 28, 29,
			30, 31, 32, 33, 34, 35
		}));
		_dataArray.Add(new SkillBookItem(576, 1152, 10, 1001, 2, 574, "icon_SkillBook_yixuanshou", 1153, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 40, 10, 900, 1800, 1, 6, 1800, 5, allowRandomCreate: true, 35, isSpecial: false, 4, 36, -1, -1, -1, 4, 432, 240, new List<short>
		{
			110, 111, 112, 113, 114, 115, 116, 29, 30, 31,
			32, 33, 34, 35
		}));
		_dataArray.Add(new SkillBookItem(577, 1154, 10, 1001, 3, 574, "icon_SkillBook_leizubaojianshi", 1155, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 35, 10, 2250, 4500, 2, 8, 3000, 6, allowRandomCreate: true, 30, isSpecial: false, 4, 36, -1, -1, -1, 4, 433, 400, new List<short>
		{
			111, 112, 113, 114, 115, 116, 30, 31, 32, 33,
			34, 35
		}));
		_dataArray.Add(new SkillBookItem(578, 1156, 10, 1001, 4, 574, "icon_SkillBook_leizubaojianshi", 1157, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 30, 10, 4650, 9300, 3, 10, 4200, 7, allowRandomCreate: true, 25, isSpecial: false, 4, 36, -1, -1, -1, 4, 434, 600, new List<short> { 112, 113, 114, 115, 116, 31, 32, 33, 34, 35 }));
		_dataArray.Add(new SkillBookItem(579, 1158, 10, 1001, 5, 574, "icon_SkillBook_leizubaojianshi", 1159, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 25, 10, 8400, 16800, 4, 12, 5400, 7, allowRandomCreate: true, 20, isSpecial: false, 4, 36, -1, -1, -1, 4, 435, 850, new List<short> { 113, 114, 115, 116, 32, 33, 34, 35 }));
		_dataArray.Add(new SkillBookItem(580, 1160, 10, 1001, 6, 574, "icon_SkillBook_xuanjizhixuefa", 1161, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 20, 10, 13800, 27600, 5, 14, 7200, 8, allowRandomCreate: true, 15, isSpecial: false, 4, 36, -1, -1, -1, 4, 436, 1140, new List<short> { 114, 115, 116, 33, 34, 35 }));
		_dataArray.Add(new SkillBookItem(581, 1162, 10, 1001, 7, 574, "icon_SkillBook_dataiyinyimingzhi", 1163, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 15, 10, 21150, 42300, 6, 16, 9000, 8, allowRandomCreate: true, 10, isSpecial: false, 4, 36, -1, -1, -1, 4, 437, 1480, new List<short> { 115, 116, 34, 35 }));
		_dataArray.Add(new SkillBookItem(582, 1164, 10, 1001, 0, 582, "icon_SkillBook_chaishanqindieshou", 1165, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 50, 10, 150, 300, 0, 2, 600, 3, allowRandomCreate: true, 45, isSpecial: false, 4, 36, -1, -1, -1, 4, 438, 60, new List<short>
		{
			72, 73, 74, 75, 76, 77, 78, 79, 80, 81,
			82, 83, 84, 85, 86, 87, 88, 89
		}));
		_dataArray.Add(new SkillBookItem(583, 1166, 10, 1001, 1, 582, "icon_SkillBook_chaishanqindieshou", 1167, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 45, 10, 300, 600, 0, 4, 1200, 4, allowRandomCreate: true, 40, isSpecial: false, 4, 36, -1, -1, -1, 4, 439, 130, new List<short>
		{
			73, 74, 75, 76, 77, 78, 79, 80, 82, 83,
			84, 85, 86, 87, 88, 89
		}));
		_dataArray.Add(new SkillBookItem(584, 1168, 10, 1001, 2, 582, "icon_SkillBook_chaishanqindieshou", 1169, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 40, 10, 900, 1800, 1, 6, 1800, 5, allowRandomCreate: true, 35, isSpecial: false, 4, 36, -1, -1, -1, 4, 440, 240, new List<short>
		{
			74, 75, 76, 77, 78, 79, 80, 83, 84, 85,
			86, 87, 88, 89
		}));
		_dataArray.Add(new SkillBookItem(585, 1170, 10, 1001, 3, 582, "icon_SkillBook_yuchanzhi", 1171, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 35, 10, 2250, 4500, 2, 8, 3000, 6, allowRandomCreate: true, 30, isSpecial: false, 4, 36, -1, -1, -1, 4, 441, 400, new List<short>
		{
			75, 76, 77, 78, 79, 80, 84, 85, 86, 87,
			88, 89
		}));
		_dataArray.Add(new SkillBookItem(586, 1172, 10, 1001, 4, 582, "icon_SkillBook_yuchanzhi", 1173, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 30, 10, 4650, 9300, 3, 10, 4200, 7, allowRandomCreate: true, 25, isSpecial: false, 4, 36, -1, -1, -1, 4, 442, 600, new List<short> { 76, 77, 78, 79, 80, 85, 86, 87, 88, 89 }));
		_dataArray.Add(new SkillBookItem(587, 1174, 10, 1001, 5, 582, "icon_SkillBook_yuchanzhi", 1175, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 25, 10, 8400, 16800, 4, 12, 5400, 7, allowRandomCreate: true, 20, isSpecial: false, 4, 36, -1, -1, -1, 4, 443, 850, new List<short> { 77, 78, 79, 80, 86, 87, 88, 89 }));
		_dataArray.Add(new SkillBookItem(588, 1176, 10, 1001, 0, 588, "icon_SkillBook_shegushou", 1177, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 50, 10, 150, 300, 0, 2, 600, 3, allowRandomCreate: true, 45, isSpecial: false, 4, 36, -1, -1, -1, 4, 444, 60, new List<short> { 81, 82, 83, 84, 85, 86, 87, 88, 89 }));
		_dataArray.Add(new SkillBookItem(589, 1178, 10, 1001, 1, 588, "icon_SkillBook_shegushou", 1179, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 45, 10, 300, 600, 0, 4, 1200, 4, allowRandomCreate: true, 40, isSpecial: false, 4, 36, -1, -1, -1, 4, 445, 130, new List<short> { 82, 83, 84, 85, 86, 87, 88, 89 }));
		_dataArray.Add(new SkillBookItem(590, 1180, 10, 1001, 2, 588, "icon_SkillBook_shegushou", 1181, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 40, 10, 900, 1800, 1, 6, 1800, 5, allowRandomCreate: true, 35, isSpecial: false, 4, 36, -1, -1, -1, 4, 446, 240, new List<short> { 83, 84, 85, 86, 87, 88, 89 }));
		_dataArray.Add(new SkillBookItem(591, 1182, 10, 1001, 3, 588, "icon_SkillBook_heixuegujiang", 1183, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 35, 10, 2250, 4500, 2, 8, 3000, 6, allowRandomCreate: true, 30, isSpecial: false, 4, 36, -1, -1, -1, 4, 447, 400, new List<short> { 84, 85, 86, 87, 88, 89 }));
		_dataArray.Add(new SkillBookItem(592, 1184, 10, 1001, 4, 588, "icon_SkillBook_heixuegujiang", 1185, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 30, 10, 4650, 9300, 3, 10, 4200, 7, allowRandomCreate: true, 25, isSpecial: false, 4, 36, -1, -1, -1, 4, 448, 600, new List<short> { 85, 86, 87, 88, 89 }));
		_dataArray.Add(new SkillBookItem(593, 1186, 10, 1001, 5, 588, "icon_SkillBook_heixuegujiang", 1187, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 25, 10, 8400, 16800, 4, 12, 5400, 7, allowRandomCreate: true, 20, isSpecial: false, 4, 36, -1, -1, -1, 4, 449, 850, new List<short> { 86, 87, 88, 89 }));
		_dataArray.Add(new SkillBookItem(594, 1188, 10, 1001, 6, 588, "icon_SkillBook_biyubingcangu", 1189, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 20, 10, 13800, 27600, 5, 14, 7200, 8, allowRandomCreate: true, 15, isSpecial: false, 4, 36, -1, -1, -1, 4, 450, 1140, new List<short> { 87, 88, 89 }));
		_dataArray.Add(new SkillBookItem(595, 1190, 10, 1001, 7, 588, "icon_SkillBook_wanggujincanjiang", 1191, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 15, 10, 21150, 42300, 6, 16, 9000, 8, allowRandomCreate: true, 10, isSpecial: false, 4, 36, -1, -1, -1, 4, 451, 1480, new List<short> { 88, 89 }));
		_dataArray.Add(new SkillBookItem(596, 1192, 10, 1001, 8, 588, "icon_SkillBook_baicaiqingsuigu", 1193, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 10, 10, 30750, 61500, 6, 18, 10800, 8, allowRandomCreate: true, 5, isSpecial: false, 4, 36, -1, -1, -1, 4, 452, 1860, new List<short> { 89 }));
		_dataArray.Add(new SkillBookItem(597, 1194, 10, 1001, 0, 597, "icon_SkillBook_wuxingxiaoqinna", 1195, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 50, 10, 150, 300, 0, 2, 600, 3, allowRandomCreate: true, 45, isSpecial: false, 4, 36, -1, -1, -1, 4, 453, 60, new List<short> { 36, 37, 38, 39, 40, 41, 42, 43, 44 }));
		_dataArray.Add(new SkillBookItem(598, 1196, 10, 1001, 1, 597, "icon_SkillBook_wuxingxiaoqinna", 1197, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 45, 10, 300, 600, 0, 4, 1200, 4, allowRandomCreate: true, 40, isSpecial: false, 4, 36, -1, -1, -1, 4, 454, 130, new List<short> { 37, 38, 39, 40, 41, 42, 43, 44 }));
		_dataArray.Add(new SkillBookItem(599, 1198, 10, 1001, 2, 597, "icon_SkillBook_wuxingxiaoqinna", 1199, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 40, 10, 900, 1800, 1, 6, 1800, 5, allowRandomCreate: true, 35, isSpecial: false, 4, 36, -1, -1, -1, 4, 455, 240, new List<short> { 38, 39, 40, 41, 42, 43, 44 }));
	}

	private void CreateItems10()
	{
		_dataArray.Add(new SkillBookItem(600, 1200, 10, 1001, 3, 597, "icon_SkillBook_fuyinzhi", 1201, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 35, 10, 2250, 4500, 2, 8, 3000, 6, allowRandomCreate: true, 30, isSpecial: false, 4, 36, -1, -1, -1, 4, 456, 400, new List<short> { 39, 40, 41, 42, 43, 44 }));
		_dataArray.Add(new SkillBookItem(601, 1202, 10, 1001, 4, 597, "icon_SkillBook_fuyinzhi", 1203, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 30, 10, 4650, 9300, 3, 10, 4200, 7, allowRandomCreate: true, 25, isSpecial: false, 4, 36, -1, -1, -1, 4, 457, 600, new List<short> { 40, 41, 42, 43, 44 }));
		_dataArray.Add(new SkillBookItem(602, 1204, 10, 1001, 5, 597, "icon_SkillBook_fuyinzhi", 1205, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 25, 10, 8400, 16800, 4, 12, 5400, 7, allowRandomCreate: true, 20, isSpecial: false, 4, 36, -1, -1, -1, 4, 458, 850, new List<short> { 41, 42, 43, 44 }));
		_dataArray.Add(new SkillBookItem(603, 1206, 10, 1001, 6, 597, "icon_SkillBook_taisujueshou", 1207, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 20, 10, 13800, 27600, 5, 14, 7200, 8, allowRandomCreate: true, 15, isSpecial: false, 4, 36, -1, -1, -1, 4, 459, 1140, new List<short> { 42, 43, 44 }));
		_dataArray.Add(new SkillBookItem(604, 1208, 10, 1001, 7, 597, "icon_SkillBook_cuoshenzhi", 1209, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 15, 10, 21150, 42300, 6, 16, 9000, 8, allowRandomCreate: true, 10, isSpecial: false, 4, 36, -1, -1, -1, 4, 460, 1480, new List<short> { 43, 44 }));
		_dataArray.Add(new SkillBookItem(605, 1210, 10, 1001, 8, 597, "icon_SkillBook_taiyiliulizhi", 1211, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 10, 10, 30750, 61500, 6, 18, 10800, 8, allowRandomCreate: true, 5, isSpecial: false, 4, 36, -1, -1, -1, 4, 461, 1860, new List<short> { 44 }));
		_dataArray.Add(new SkillBookItem(606, 1212, 10, 1001, 0, 606, "icon_SkillBook_chuoyanxiaoergong", 1213, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 50, 10, 150, 300, 0, 2, 600, 3, allowRandomCreate: true, 45, isSpecial: false, 4, 36, -1, -1, -1, 4, 462, 60, new List<short>
		{
			81, 82, 83, 84, 85, 86, 87, 88, 89, 135,
			136, 137, 138, 139, 140, 141, 142, 143
		}));
		_dataArray.Add(new SkillBookItem(607, 1214, 10, 1001, 1, 606, "icon_SkillBook_chuoyanxiaoergong", 1215, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 45, 10, 300, 600, 0, 4, 1200, 4, allowRandomCreate: true, 40, isSpecial: false, 4, 36, -1, -1, -1, 4, 463, 130, new List<short>
		{
			82, 83, 84, 85, 86, 87, 88, 89, 136, 137,
			138, 139, 140, 141, 142, 143
		}));
		_dataArray.Add(new SkillBookItem(608, 1216, 10, 1001, 2, 606, "icon_SkillBook_chuoyanxiaoergong", 1217, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 40, 10, 900, 1800, 1, 6, 1800, 5, allowRandomCreate: true, 35, isSpecial: false, 4, 36, -1, -1, -1, 4, 464, 240, new List<short>
		{
			83, 84, 85, 86, 87, 88, 89, 137, 138, 139,
			140, 141, 142, 143
		}));
		_dataArray.Add(new SkillBookItem(609, 1218, 10, 1001, 3, 606, "icon_SkillBook_sanzhiduoyinzhao", 1219, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 35, 10, 2250, 4500, 2, 8, 3000, 6, allowRandomCreate: true, 30, isSpecial: false, 4, 36, -1, -1, -1, 4, 465, 400, new List<short>
		{
			84, 85, 86, 87, 88, 89, 138, 139, 140, 141,
			142, 143
		}));
		_dataArray.Add(new SkillBookItem(610, 1220, 10, 1001, 4, 606, "icon_SkillBook_sanzhiduoyinzhao", 1221, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 30, 10, 4650, 9300, 3, 10, 4200, 7, allowRandomCreate: true, 25, isSpecial: false, 4, 36, -1, -1, -1, 4, 466, 600, new List<short> { 85, 86, 87, 88, 89, 139, 140, 141, 142, 143 }));
		_dataArray.Add(new SkillBookItem(611, 1222, 10, 1001, 5, 606, "icon_SkillBook_sanzhiduoyinzhao", 1223, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 25, 10, 8400, 16800, 4, 12, 5400, 7, allowRandomCreate: true, 20, isSpecial: false, 4, 36, -1, -1, -1, 4, 467, 850, new List<short> { 86, 87, 88, 89, 140, 141, 142, 143 }));
		_dataArray.Add(new SkillBookItem(612, 1224, 10, 1001, 6, 606, "icon_SkillBook_xuechiguizhao", 1225, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 20, 10, 13800, 27600, 5, 14, 7200, 8, allowRandomCreate: true, 15, isSpecial: false, 4, 36, -1, -1, -1, 4, 468, 1140, new List<short> { 87, 88, 89, 141, 142, 143 }));
		_dataArray.Add(new SkillBookItem(613, 1226, 10, 1001, 0, 613, "icon_SkillBook_yuanshandantui", 1227, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 50, 10, 150, 300, 0, 2, 600, 3, allowRandomCreate: true, 45, isSpecial: false, 4, 36, -1, -1, -1, 5, 469, 60, new List<short> { 87, 88, 89, 141, 142, 143 }));
		_dataArray.Add(new SkillBookItem(614, 1228, 10, 1001, 1, 613, "icon_SkillBook_yuanshandantui", 1229, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 45, 10, 300, 600, 0, 4, 1200, 4, allowRandomCreate: true, 40, isSpecial: false, 4, 36, -1, -1, -1, 5, 470, 130, new List<short>
		{
			109, 110, 111, 112, 113, 114, 115, 116, 118, 119,
			120, 121, 122, 123, 124, 125
		}));
		_dataArray.Add(new SkillBookItem(615, 1230, 10, 1001, 2, 613, "icon_SkillBook_yuanshandantui", 1231, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 40, 10, 900, 1800, 1, 6, 1800, 5, allowRandomCreate: true, 35, isSpecial: false, 4, 36, -1, -1, -1, 5, 471, 240, new List<short>
		{
			110, 111, 112, 113, 114, 115, 116, 119, 120, 121,
			122, 123, 124, 125
		}));
		_dataArray.Add(new SkillBookItem(616, 1232, 10, 1001, 3, 613, "icon_SkillBook_jiugongluanbabu", 1233, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 35, 10, 2250, 4500, 2, 8, 3000, 6, allowRandomCreate: true, 30, isSpecial: false, 4, 36, -1, -1, -1, 5, 472, 400, new List<short>
		{
			111, 112, 113, 114, 115, 116, 120, 121, 122, 123,
			124, 125
		}));
		_dataArray.Add(new SkillBookItem(617, 1234, 10, 1001, 4, 613, "icon_SkillBook_jiugongluanbabu", 1235, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 30, 10, 4650, 9300, 3, 10, 4200, 7, allowRandomCreate: true, 25, isSpecial: false, 4, 36, -1, -1, -1, 5, 473, 600, new List<short> { 112, 113, 114, 115, 116, 121, 122, 123, 124, 125 }));
		_dataArray.Add(new SkillBookItem(618, 1236, 10, 1001, 5, 613, "icon_SkillBook_jiugongluanbabu", 1237, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 25, 10, 8400, 16800, 4, 12, 5400, 7, allowRandomCreate: true, 20, isSpecial: false, 4, 36, -1, -1, -1, 5, 474, 850, new List<short> { 113, 114, 115, 116, 122, 123, 124, 125 }));
		_dataArray.Add(new SkillBookItem(619, 1238, 10, 1001, 6, 613, "icon_SkillBook_yunlongjiuxiantui", 1239, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 20, 10, 13800, 27600, 5, 14, 7200, 8, allowRandomCreate: true, 15, isSpecial: false, 4, 36, -1, -1, -1, 5, 475, 1140, new List<short> { 114, 115, 116, 123, 124, 125 }));
		_dataArray.Add(new SkillBookItem(620, 1240, 10, 1001, 7, 613, "icon_SkillBook_aowangshentui", 1241, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 15, 10, 21150, 42300, 6, 16, 9000, 8, allowRandomCreate: true, 10, isSpecial: false, 4, 36, -1, -1, -1, 5, 476, 1480, new List<short> { 115, 116, 124, 125 }));
		_dataArray.Add(new SkillBookItem(621, 1242, 10, 1001, 8, 613, "icon_SkillBook_liuyasishenzu", 1243, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 10, 10, 30750, 61500, 6, 18, 10800, 8, allowRandomCreate: true, 5, isSpecial: false, 4, 36, -1, -1, -1, 5, 477, 1860, new List<short> { 116, 125 }));
		_dataArray.Add(new SkillBookItem(622, 1244, 10, 1001, 0, 622, "icon_SkillBook_daodachongtianzi", 1245, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 50, 10, 150, 300, 0, 2, 600, 3, allowRandomCreate: true, 45, isSpecial: false, 4, 36, -1, -1, -1, 5, 478, 60, new List<short>
		{
			72, 73, 74, 75, 76, 77, 78, 79, 80, 81,
			82, 83, 84, 85, 86, 87, 88, 89
		}));
		_dataArray.Add(new SkillBookItem(623, 1246, 10, 1001, 1, 622, "icon_SkillBook_daodachongtianzi", 1247, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 45, 10, 300, 600, 0, 4, 1200, 4, allowRandomCreate: true, 40, isSpecial: false, 4, 36, -1, -1, -1, 5, 479, 130, new List<short>
		{
			73, 74, 75, 76, 77, 78, 79, 80, 82, 83,
			84, 85, 86, 87, 88, 89
		}));
		_dataArray.Add(new SkillBookItem(624, 1248, 10, 1001, 2, 622, "icon_SkillBook_daodachongtianzi", 1249, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 40, 10, 900, 1800, 1, 6, 1800, 5, allowRandomCreate: true, 35, isSpecial: false, 4, 36, -1, -1, -1, 5, 480, 240, new List<short>
		{
			74, 75, 76, 77, 78, 79, 80, 83, 84, 85,
			86, 87, 88, 89
		}));
		_dataArray.Add(new SkillBookItem(625, 1250, 10, 1001, 3, 622, "icon_SkillBook_kejinzhenyuxiaobashi", 1251, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 35, 10, 2250, 4500, 2, 8, 3000, 6, allowRandomCreate: true, 30, isSpecial: false, 4, 36, -1, -1, -1, 5, 481, 400, new List<short>
		{
			75, 76, 77, 78, 79, 80, 84, 85, 86, 87,
			88, 89
		}));
		_dataArray.Add(new SkillBookItem(626, 1252, 10, 1001, 4, 622, "icon_SkillBook_kejinzhenyuxiaobashi", 1253, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 30, 10, 4650, 9300, 3, 10, 4200, 7, allowRandomCreate: true, 25, isSpecial: false, 4, 36, -1, -1, -1, 5, 482, 600, new List<short> { 76, 77, 78, 79, 80, 85, 86, 87, 88, 89 }));
		_dataArray.Add(new SkillBookItem(627, 1254, 10, 1001, 5, 622, "icon_SkillBook_kejinzhenyuxiaobashi", 1255, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 25, 10, 8400, 16800, 4, 12, 5400, 7, allowRandomCreate: true, 20, isSpecial: false, 4, 36, -1, -1, -1, 5, 483, 850, new List<short> { 77, 78, 79, 80, 86, 87, 88, 89 }));
		_dataArray.Add(new SkillBookItem(628, 1256, 10, 1001, 6, 622, "icon_SkillBook_feishanduanhaidabashi", 1257, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 20, 10, 13800, 27600, 5, 14, 7200, 8, allowRandomCreate: true, 15, isSpecial: false, 4, 36, -1, -1, -1, 5, 484, 1140, new List<short> { 78, 79, 80, 87, 88, 89 }));
		_dataArray.Add(new SkillBookItem(629, 1258, 10, 1001, 7, 622, "icon_SkillBook_xuandingbaibu", 1259, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 15, 10, 21150, 42300, 6, 16, 9000, 8, allowRandomCreate: true, 10, isSpecial: false, 4, 36, -1, -1, -1, 5, 485, 1480, new List<short> { 79, 80, 88, 89 }));
		_dataArray.Add(new SkillBookItem(630, 1260, 10, 1001, 0, 630, "icon_SkillBook_liaoyintui", 1261, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 50, 10, 150, 300, 0, 2, 600, 3, allowRandomCreate: true, 45, isSpecial: false, 4, 36, -1, -1, -1, 5, 486, 60, new List<short>
		{
			81, 82, 83, 84, 85, 86, 87, 88, 89, 135,
			136, 137, 138, 139, 140, 141, 142, 143
		}));
		_dataArray.Add(new SkillBookItem(631, 1262, 10, 1001, 1, 630, "icon_SkillBook_liaoyintui", 1263, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 45, 10, 300, 600, 0, 4, 1200, 4, allowRandomCreate: true, 40, isSpecial: false, 4, 36, -1, -1, -1, 5, 487, 130, new List<short>
		{
			82, 83, 84, 85, 86, 87, 88, 89, 136, 137,
			138, 139, 140, 141, 142, 143
		}));
		_dataArray.Add(new SkillBookItem(632, 1264, 10, 1001, 2, 630, "icon_SkillBook_liaoyintui", 1265, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 40, 10, 900, 1800, 1, 6, 1800, 5, allowRandomCreate: true, 35, isSpecial: false, 4, 36, -1, -1, -1, 5, 488, 240, new List<short>
		{
			83, 84, 85, 86, 87, 88, 89, 137, 138, 139,
			140, 141, 142, 143
		}));
		_dataArray.Add(new SkillBookItem(633, 1266, 10, 1001, 3, 630, "icon_SkillBook_xiezigouhunjiao", 1267, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 35, 10, 2250, 4500, 2, 8, 3000, 6, allowRandomCreate: true, 30, isSpecial: false, 4, 36, -1, -1, -1, 5, 489, 400, new List<short>
		{
			84, 85, 86, 87, 88, 89, 138, 139, 140, 141,
			142, 143
		}));
		_dataArray.Add(new SkillBookItem(634, 1268, 10, 1001, 4, 630, "icon_SkillBook_xiezigouhunjiao", 1269, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 30, 10, 4650, 9300, 3, 10, 4200, 7, allowRandomCreate: true, 25, isSpecial: false, 4, 36, -1, -1, -1, 5, 490, 600, new List<short> { 85, 86, 87, 88, 89, 139, 140, 141, 142, 143 }));
		_dataArray.Add(new SkillBookItem(635, 1270, 10, 1001, 5, 630, "icon_SkillBook_xiezigouhunjiao", 1271, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 25, 10, 8400, 16800, 4, 12, 5400, 7, allowRandomCreate: true, 20, isSpecial: false, 4, 36, -1, -1, -1, 5, 491, 850, new List<short> { 86, 87, 88, 89, 140, 141, 142, 143 }));
		_dataArray.Add(new SkillBookItem(636, 1272, 10, 1001, 6, 630, "icon_SkillBook_yanwangguijiao", 1273, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 20, 10, 13800, 27600, 5, 14, 7200, 8, allowRandomCreate: true, 15, isSpecial: false, 4, 36, -1, -1, -1, 5, 492, 1140, new List<short> { 87, 88, 89, 141, 142, 143 }));
		_dataArray.Add(new SkillBookItem(637, 1274, 10, 1001, 7, 630, "icon_SkillBook_xieyingtui", 1275, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 15, 10, 21150, 42300, 6, 16, 9000, 8, allowRandomCreate: true, 10, isSpecial: false, 4, 36, -1, -1, -1, 5, 493, 1480, new List<short> { 88, 89, 142, 143 }));
		_dataArray.Add(new SkillBookItem(638, 1276, 10, 1001, 0, 638, "icon_SkillBook_zhenyuxiang", 1277, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 50, 10, 150, 300, 0, 2, 600, 3, allowRandomCreate: true, 45, isSpecial: false, 4, 36, -1, -1, -1, 6, 494, 60, new List<short>
		{
			72, 73, 74, 75, 76, 77, 78, 79, 80, 81,
			82, 83, 84, 85, 86, 87, 88, 89
		}));
		_dataArray.Add(new SkillBookItem(639, 1278, 10, 1001, 1, 638, "icon_SkillBook_zhenyuxiang", 1279, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 45, 10, 300, 600, 0, 4, 1200, 4, allowRandomCreate: true, 40, isSpecial: false, 4, 36, -1, -1, -1, 6, 495, 130, new List<short>
		{
			73, 74, 75, 76, 77, 78, 79, 80, 82, 83,
			84, 85, 86, 87, 88, 89
		}));
		_dataArray.Add(new SkillBookItem(640, 1280, 10, 1001, 2, 638, "icon_SkillBook_zhenyuxiang", 1281, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 40, 10, 900, 1800, 1, 6, 1800, 5, allowRandomCreate: true, 35, isSpecial: false, 4, 36, -1, -1, -1, 6, 496, 240, new List<short>
		{
			74, 75, 76, 77, 78, 79, 80, 83, 84, 85,
			86, 87, 88, 89
		}));
		_dataArray.Add(new SkillBookItem(641, 1282, 10, 1001, 3, 638, "icon_SkillBook_bixueshuixian", 1283, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 35, 10, 2250, 4500, 2, 8, 3000, 6, allowRandomCreate: true, 30, isSpecial: false, 4, 36, -1, -1, -1, 6, 497, 400, new List<short>
		{
			75, 76, 77, 78, 79, 80, 84, 85, 86, 87,
			88, 89
		}));
		_dataArray.Add(new SkillBookItem(642, 1284, 10, 1001, 4, 638, "icon_SkillBook_bixueshuixian", 1285, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 30, 10, 4650, 9300, 3, 10, 4200, 7, allowRandomCreate: true, 25, isSpecial: false, 4, 36, -1, -1, -1, 6, 498, 600, new List<short> { 76, 77, 78, 79, 80, 85, 86, 87, 88, 89 }));
		_dataArray.Add(new SkillBookItem(643, 1286, 10, 1001, 5, 638, "icon_SkillBook_bixueshuixian", 1287, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 25, 10, 8400, 16800, 4, 12, 5400, 7, allowRandomCreate: true, 20, isSpecial: false, 4, 36, -1, -1, -1, 6, 499, 850, new List<short> { 77, 78, 79, 80, 86, 87, 88, 89 }));
		_dataArray.Add(new SkillBookItem(644, 1288, 10, 1001, 6, 638, "icon_SkillBook_baixiexiaoguxiang", 1289, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 20, 10, 13800, 27600, 5, 14, 7200, 8, allowRandomCreate: true, 15, isSpecial: false, 4, 36, -1, -1, -1, 6, 500, 1140, new List<short> { 78, 79, 80, 87, 88, 89 }));
		_dataArray.Add(new SkillBookItem(645, 1290, 10, 1001, 7, 638, "icon_SkillBook_qingjiaoxie", 1291, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 15, 10, 21150, 42300, 6, 16, 9000, 8, allowRandomCreate: true, 10, isSpecial: false, 4, 36, -1, -1, -1, 6, 501, 1480, new List<short> { 79, 80, 88, 89 }));
		_dataArray.Add(new SkillBookItem(646, 1292, 10, 1001, 8, 638, "icon_SkillBook_guiguxuehaitang", 1293, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 10, 10, 30750, 61500, 6, 18, 10800, 8, allowRandomCreate: true, 5, isSpecial: false, 4, 36, -1, -1, -1, 6, 502, 1860, new List<short> { 80, 89 }));
		_dataArray.Add(new SkillBookItem(647, 1294, 10, 1001, 0, 647, "icon_SkillBook_jieqingshijue", 1295, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 50, 10, 150, 300, 0, 2, 600, 3, allowRandomCreate: true, 45, isSpecial: false, 4, 36, -1, -1, -1, 6, 503, 60, new List<short>
		{
			36, 37, 38, 39, 40, 41, 42, 43, 44, 9,
			10, 11, 12, 13, 14, 15, 16, 17
		}));
		_dataArray.Add(new SkillBookItem(648, 1296, 10, 1001, 1, 647, "icon_SkillBook_jieqingshijue", 1297, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 45, 10, 300, 600, 0, 4, 1200, 4, allowRandomCreate: true, 40, isSpecial: false, 4, 36, -1, -1, -1, 6, 504, 130, new List<short>
		{
			37, 38, 39, 40, 41, 42, 43, 44, 10, 11,
			12, 13, 14, 15, 16, 17
		}));
		_dataArray.Add(new SkillBookItem(649, 1298, 10, 1001, 2, 647, "icon_SkillBook_jieqingshijue", 1299, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 40, 10, 900, 1800, 1, 6, 1800, 5, allowRandomCreate: true, 35, isSpecial: false, 4, 36, -1, -1, -1, 6, 505, 240, new List<short>
		{
			38, 39, 40, 41, 42, 43, 44, 11, 12, 13,
			14, 15, 16, 17
		}));
		_dataArray.Add(new SkillBookItem(650, 1300, 10, 1001, 3, 647, "icon_SkillBook_feixingshu", 1301, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 35, 10, 2250, 4500, 2, 8, 3000, 6, allowRandomCreate: true, 30, isSpecial: false, 4, 36, -1, -1, -1, 6, 506, 400, new List<short>
		{
			39, 40, 41, 42, 43, 44, 12, 13, 14, 15,
			16, 17
		}));
		_dataArray.Add(new SkillBookItem(651, 1302, 10, 1001, 4, 647, "icon_SkillBook_feixingshu", 1303, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 30, 10, 4650, 9300, 3, 10, 4200, 7, allowRandomCreate: true, 25, isSpecial: false, 4, 36, -1, -1, -1, 6, 507, 600, new List<short> { 40, 41, 42, 43, 44, 13, 14, 15, 16, 17 }));
		_dataArray.Add(new SkillBookItem(652, 1304, 10, 1001, 5, 647, "icon_SkillBook_feixingshu", 1305, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 25, 10, 8400, 16800, 4, 12, 5400, 7, allowRandomCreate: true, 20, isSpecial: false, 4, 36, -1, -1, -1, 6, 508, 850, new List<short> { 41, 42, 43, 44, 14, 15, 16, 17 }));
		_dataArray.Add(new SkillBookItem(653, 1306, 10, 1001, 6, 647, "icon_SkillBook_minglongzhi", 1307, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 20, 10, 13800, 27600, 5, 14, 7200, 8, allowRandomCreate: true, 15, isSpecial: false, 4, 36, -1, -1, -1, 6, 509, 1140, new List<short> { 42, 43, 44, 15, 16, 17 }));
		_dataArray.Add(new SkillBookItem(654, 1308, 10, 1001, 7, 647, "icon_SkillBook_dingyingshenzhen", 1309, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 15, 10, 21150, 42300, 6, 16, 9000, 8, allowRandomCreate: true, 10, isSpecial: false, 4, 36, -1, -1, -1, 6, 510, 1480, new List<short> { 43, 44, 16, 17 }));
		_dataArray.Add(new SkillBookItem(655, 1310, 10, 1001, 8, 647, "icon_SkillBook_moxiangshentong", 1311, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 10, 10, 30750, 61500, 6, 18, 10800, 8, allowRandomCreate: true, 5, isSpecial: false, 4, 36, -1, -1, -1, 6, 511, 1860, new List<short> { 44, 17 }));
		_dataArray.Add(new SkillBookItem(656, 1312, 10, 1001, 0, 656, "icon_SkillBook_feishazoushi", 1313, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 50, 10, 150, 300, 0, 2, 600, 3, allowRandomCreate: true, 45, isSpecial: false, 4, 36, -1, -1, -1, 6, 512, 60, new List<short>
		{
			126, 127, 128, 129, 130, 131, 132, 133, 134, 45,
			46, 47, 48, 49, 50, 51, 52, 53
		}));
		_dataArray.Add(new SkillBookItem(657, 1314, 10, 1001, 1, 656, "icon_SkillBook_feishazoushi", 1315, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 45, 10, 300, 600, 0, 4, 1200, 4, allowRandomCreate: true, 40, isSpecial: false, 4, 36, -1, -1, -1, 6, 513, 130, new List<short>
		{
			127, 128, 129, 130, 131, 132, 133, 134, 46, 47,
			48, 49, 50, 51, 52, 53
		}));
		_dataArray.Add(new SkillBookItem(658, 1316, 10, 1001, 2, 656, "icon_SkillBook_feishazoushi", 1317, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 40, 10, 900, 1800, 1, 6, 1800, 5, allowRandomCreate: true, 35, isSpecial: false, 4, 36, -1, -1, -1, 6, 514, 240, new List<short>
		{
			128, 129, 130, 131, 132, 133, 134, 47, 48, 49,
			50, 51, 52, 53
		}));
		_dataArray.Add(new SkillBookItem(659, 1318, 10, 1001, 3, 656, "icon_SkillBook_chiqingshenhuojin", 1319, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 35, 10, 2250, 4500, 2, 8, 3000, 6, allowRandomCreate: true, 30, isSpecial: false, 4, 36, -1, -1, -1, 6, 515, 400, new List<short>
		{
			129, 130, 131, 132, 133, 134, 48, 49, 50, 51,
			52, 53
		}));
	}

	private void CreateItems11()
	{
		_dataArray.Add(new SkillBookItem(660, 1320, 10, 1001, 4, 656, "icon_SkillBook_chiqingshenhuojin", 1321, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 30, 10, 4650, 9300, 3, 10, 4200, 7, allowRandomCreate: true, 25, isSpecial: false, 4, 36, -1, -1, -1, 6, 516, 600, new List<short> { 130, 131, 132, 133, 134, 49, 50, 51, 52, 53 }));
		_dataArray.Add(new SkillBookItem(661, 1322, 10, 1001, 5, 656, "icon_SkillBook_chiqingshenhuojin", 1323, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 25, 10, 8400, 16800, 4, 12, 5400, 7, allowRandomCreate: true, 20, isSpecial: false, 4, 36, -1, -1, -1, 6, 517, 850, new List<short> { 131, 132, 133, 134, 50, 51, 52, 53 }));
		_dataArray.Add(new SkillBookItem(662, 1324, 10, 1001, 0, 662, "icon_SkillBook_yeguidusha", 1325, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 50, 10, 150, 300, 0, 2, 600, 3, allowRandomCreate: true, 45, isSpecial: false, 4, 36, -1, -1, -1, 6, 518, 60, new List<short>
		{
			81, 82, 83, 84, 85, 86, 87, 88, 89, 135,
			136, 137, 138, 139, 140, 141, 142, 143
		}));
		_dataArray.Add(new SkillBookItem(663, 1326, 10, 1001, 1, 662, "icon_SkillBook_yeguidusha", 1327, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 45, 10, 300, 600, 0, 4, 1200, 4, allowRandomCreate: true, 40, isSpecial: false, 4, 36, -1, -1, -1, 6, 519, 130, new List<short>
		{
			82, 83, 84, 85, 86, 87, 88, 89, 136, 137,
			138, 139, 140, 141, 142, 143
		}));
		_dataArray.Add(new SkillBookItem(664, 1328, 10, 1001, 2, 662, "icon_SkillBook_yeguidusha", 1329, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 40, 10, 900, 1800, 1, 6, 1800, 5, allowRandomCreate: true, 35, isSpecial: false, 4, 36, -1, -1, -1, 6, 520, 240, new List<short>
		{
			83, 84, 85, 86, 87, 88, 89, 137, 138, 139,
			140, 141, 142, 143
		}));
		_dataArray.Add(new SkillBookItem(665, 1330, 10, 1001, 3, 662, "icon_SkillBook_huangfensha", 1331, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 35, 10, 2250, 4500, 2, 8, 3000, 6, allowRandomCreate: true, 30, isSpecial: false, 4, 36, -1, -1, -1, 6, 521, 400, new List<short>
		{
			84, 85, 86, 87, 88, 89, 138, 139, 140, 141,
			142, 143
		}));
		_dataArray.Add(new SkillBookItem(666, 1332, 10, 1001, 4, 662, "icon_SkillBook_huangfensha", 1333, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 30, 10, 4650, 9300, 3, 10, 4200, 7, allowRandomCreate: true, 25, isSpecial: false, 4, 36, -1, -1, -1, 6, 522, 600, new List<short> { 85, 86, 87, 88, 89, 139, 140, 141, 142, 143 }));
		_dataArray.Add(new SkillBookItem(667, 1334, 10, 1001, 5, 662, "icon_SkillBook_huangfensha", 1335, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 25, 10, 8400, 16800, 4, 12, 5400, 7, allowRandomCreate: true, 20, isSpecial: false, 4, 36, -1, -1, -1, 6, 523, 850, new List<short> { 86, 87, 88, 89, 140, 141, 142, 143 }));
		_dataArray.Add(new SkillBookItem(668, 1336, 10, 1001, 6, 662, "icon_SkillBook_xiehousha", 1337, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 20, 10, 13800, 27600, 5, 14, 7200, 8, allowRandomCreate: true, 15, isSpecial: false, 4, 36, -1, -1, -1, 6, 524, 1140, new List<short> { 87, 88, 89, 141, 142, 143 }));
		_dataArray.Add(new SkillBookItem(669, 1338, 10, 1001, 0, 669, "icon_SkillBook_fuhualüeyingjian", 1339, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 50, 10, 150, 300, 0, 2, 600, 3, allowRandomCreate: true, 45, isSpecial: false, 4, 36, -1, -1, -1, 7, 525, 60, new List<short>
		{
			108, 109, 110, 111, 112, 113, 114, 115, 116, 117,
			118, 119, 120, 121, 122, 123, 124, 125
		}));
		_dataArray.Add(new SkillBookItem(670, 1340, 10, 1001, 1, 669, "icon_SkillBook_fuhualüeyingjian", 1341, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 45, 10, 300, 600, 0, 4, 1200, 4, allowRandomCreate: true, 40, isSpecial: false, 4, 36, -1, -1, -1, 7, 526, 130, new List<short>
		{
			109, 110, 111, 112, 113, 114, 115, 116, 118, 119,
			120, 121, 122, 123, 124, 125
		}));
		_dataArray.Add(new SkillBookItem(671, 1342, 10, 1001, 2, 669, "icon_SkillBook_fuhualüeyingjian", 1343, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 40, 10, 900, 1800, 1, 6, 1800, 5, allowRandomCreate: true, 35, isSpecial: false, 4, 36, -1, -1, -1, 7, 527, 240, new List<short>
		{
			110, 111, 112, 113, 114, 115, 116, 119, 120, 121,
			122, 123, 124, 125
		}));
		_dataArray.Add(new SkillBookItem(672, 1344, 10, 1001, 3, 669, "icon_SkillBook_lianhuamiaojian", 1345, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 35, 10, 2250, 4500, 2, 8, 3000, 6, allowRandomCreate: true, 30, isSpecial: false, 4, 36, -1, -1, -1, 7, 528, 400, new List<short>
		{
			111, 112, 113, 114, 115, 116, 120, 121, 122, 123,
			124, 125
		}));
		_dataArray.Add(new SkillBookItem(673, 1346, 10, 1001, 4, 669, "icon_SkillBook_lianhuamiaojian", 1347, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 30, 10, 4650, 9300, 3, 10, 4200, 7, allowRandomCreate: true, 25, isSpecial: false, 4, 36, -1, -1, -1, 7, 529, 600, new List<short> { 112, 113, 114, 115, 116, 121, 122, 123, 124, 125 }));
		_dataArray.Add(new SkillBookItem(674, 1348, 10, 1001, 5, 669, "icon_SkillBook_lianhuamiaojian", 1349, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 25, 10, 8400, 16800, 4, 12, 5400, 7, allowRandomCreate: true, 20, isSpecial: false, 4, 36, -1, -1, -1, 7, 530, 850, new List<short> { 113, 114, 115, 116, 122, 123, 124, 125 }));
		_dataArray.Add(new SkillBookItem(675, 1350, 10, 1001, 6, 669, "icon_SkillBook_yunvshenjian", 1351, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 20, 10, 13800, 27600, 5, 14, 7200, 8, allowRandomCreate: true, 15, isSpecial: false, 4, 36, -1, -1, -1, 7, 531, 1140, new List<short> { 114, 115, 116, 123, 124, 125 }));
		_dataArray.Add(new SkillBookItem(676, 1352, 10, 1001, 0, 676, "icon_SkillBook_wudangdanjian", 1353, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 50, 10, 150, 300, 0, 2, 600, 3, allowRandomCreate: true, 45, isSpecial: false, 4, 36, -1, -1, -1, 7, 532, 60, new List<short> { 108, 109, 110, 111, 112, 113, 114, 115, 116 }));
		_dataArray.Add(new SkillBookItem(677, 1354, 10, 1001, 1, 676, "icon_SkillBook_wudangdanjian", 1355, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 45, 10, 300, 600, 0, 4, 1200, 4, allowRandomCreate: true, 40, isSpecial: false, 4, 36, -1, -1, -1, 7, 533, 130, new List<short> { 109, 110, 111, 112, 113, 114, 115, 116 }));
		_dataArray.Add(new SkillBookItem(678, 1356, 10, 1001, 2, 676, "icon_SkillBook_wudangdanjian", 1357, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 40, 10, 900, 1800, 1, 6, 1800, 5, allowRandomCreate: true, 35, isSpecial: false, 4, 36, -1, -1, -1, 7, 534, 240, new List<short> { 110, 111, 112, 113, 114, 115, 116 }));
		_dataArray.Add(new SkillBookItem(679, 1358, 10, 1001, 3, 676, "icon_SkillBook_taiyixuanmenjian", 1359, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 35, 10, 2250, 4500, 2, 8, 3000, 6, allowRandomCreate: true, 30, isSpecial: false, 4, 36, -1, -1, -1, 7, 535, 400, new List<short> { 111, 112, 113, 114, 115, 116 }));
		_dataArray.Add(new SkillBookItem(680, 1360, 10, 1001, 4, 676, "icon_SkillBook_taiyixuanmenjian", 1361, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 30, 10, 4650, 9300, 3, 10, 4200, 7, allowRandomCreate: true, 25, isSpecial: false, 4, 36, -1, -1, -1, 7, 536, 600, new List<short> { 112, 113, 114, 115, 116 }));
		_dataArray.Add(new SkillBookItem(681, 1362, 10, 1001, 5, 676, "icon_SkillBook_taiyixuanmenjian", 1363, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 25, 10, 8400, 16800, 4, 12, 5400, 7, allowRandomCreate: true, 20, isSpecial: false, 4, 36, -1, -1, -1, 7, 537, 850, new List<short> { 113, 114, 115, 116 }));
		_dataArray.Add(new SkillBookItem(682, 1364, 10, 1001, 6, 676, "icon_SkillBook_taijijianfa", 1365, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 20, 10, 13800, 27600, 5, 14, 7200, 8, allowRandomCreate: true, 15, isSpecial: false, 4, 36, -1, -1, -1, 7, 538, 1140, new List<short> { 114, 115, 116 }));
		_dataArray.Add(new SkillBookItem(683, 1366, 10, 1001, 7, 676, "icon_SkillBook_xiantianshaoyangjianqi", 1367, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 15, 10, 21150, 42300, 6, 16, 9000, 8, allowRandomCreate: true, 10, isSpecial: false, 4, 36, -1, -1, -1, 7, 539, 1480, new List<short> { 115, 116 }));
		_dataArray.Add(new SkillBookItem(684, 1368, 10, 1001, 8, 676, "icon_SkillBook_wujijianshi", 1369, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 10, 10, 30750, 61500, 6, 18, 10800, 8, allowRandomCreate: true, 5, isSpecial: false, 4, 36, -1, -1, -1, 7, 540, 1860, new List<short> { 116 }));
		_dataArray.Add(new SkillBookItem(685, 1370, 10, 1001, 0, 685, "icon_SkillBook_luohanjianfa", 1371, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 50, 10, 150, 300, 0, 2, 600, 3, allowRandomCreate: true, 45, isSpecial: false, 4, 36, -1, -1, -1, 7, 541, 60, new List<short> { 117, 118, 119, 120, 121, 122, 123, 124, 125 }));
		_dataArray.Add(new SkillBookItem(686, 1372, 10, 1001, 1, 685, "icon_SkillBook_luohanjianfa", 1373, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 45, 10, 300, 600, 0, 4, 1200, 4, allowRandomCreate: true, 40, isSpecial: false, 4, 36, -1, -1, -1, 7, 542, 130, new List<short> { 118, 119, 120, 121, 122, 123, 124, 125 }));
		_dataArray.Add(new SkillBookItem(687, 1374, 10, 1001, 2, 685, "icon_SkillBook_luohanjianfa", 1375, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 40, 10, 900, 1800, 1, 6, 1800, 5, allowRandomCreate: true, 35, isSpecial: false, 4, 36, -1, -1, -1, 7, 543, 240, new List<short> { 119, 120, 121, 122, 123, 124, 125 }));
		_dataArray.Add(new SkillBookItem(688, 1376, 10, 1001, 3, 685, "icon_SkillBook_dacibeijian", 1377, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 35, 10, 2250, 4500, 2, 8, 3000, 6, allowRandomCreate: true, 30, isSpecial: false, 4, 36, -1, -1, -1, 7, 544, 400, new List<short> { 120, 121, 122, 123, 124, 125 }));
		_dataArray.Add(new SkillBookItem(689, 1378, 10, 1001, 4, 685, "icon_SkillBook_dacibeijian", 1379, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 30, 10, 4650, 9300, 3, 10, 4200, 7, allowRandomCreate: true, 25, isSpecial: false, 4, 36, -1, -1, -1, 7, 545, 600, new List<short> { 121, 122, 123, 124, 125 }));
		_dataArray.Add(new SkillBookItem(690, 1380, 10, 1001, 5, 685, "icon_SkillBook_dacibeijian", 1381, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 25, 10, 8400, 16800, 4, 12, 5400, 7, allowRandomCreate: true, 20, isSpecial: false, 4, 36, -1, -1, -1, 7, 546, 850, new List<short> { 122, 123, 124, 125 }));
		_dataArray.Add(new SkillBookItem(691, 1382, 10, 1001, 6, 685, "icon_SkillBook_moshangputijian", 1383, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 20, 10, 13800, 27600, 5, 14, 7200, 8, allowRandomCreate: true, 15, isSpecial: false, 4, 36, -1, -1, -1, 7, 547, 1140, new List<short> { 123, 124, 125 }));
		_dataArray.Add(new SkillBookItem(692, 1384, 10, 1001, 7, 685, "icon_SkillBook_damojianfa", 1385, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 15, 10, 21150, 42300, 6, 16, 9000, 8, allowRandomCreate: true, 10, isSpecial: false, 4, 36, -1, -1, -1, 7, 548, 1480, new List<short> { 124, 125 }));
		_dataArray.Add(new SkillBookItem(693, 1386, 10, 1001, 0, 693, "icon_SkillBook_laiqujianfa", 1387, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 50, 10, 150, 300, 0, 2, 600, 3, allowRandomCreate: true, 45, isSpecial: false, 4, 36, -1, -1, -1, 7, 549, 60, new List<short>
		{
			36, 37, 38, 39, 40, 41, 42, 43, 44, 135,
			136, 137, 138, 139, 140, 141, 142, 143
		}));
		_dataArray.Add(new SkillBookItem(694, 1388, 10, 1001, 1, 693, "icon_SkillBook_laiqujianfa", 1389, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 45, 10, 300, 600, 0, 4, 1200, 4, allowRandomCreate: true, 40, isSpecial: false, 4, 36, -1, -1, -1, 7, 550, 130, new List<short>
		{
			37, 38, 39, 40, 41, 42, 43, 44, 136, 137,
			138, 139, 140, 141, 142, 143
		}));
		_dataArray.Add(new SkillBookItem(695, 1390, 10, 1001, 2, 693, "icon_SkillBook_laiqujianfa", 1391, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 40, 10, 900, 1800, 1, 6, 1800, 5, allowRandomCreate: true, 35, isSpecial: false, 4, 36, -1, -1, -1, 7, 551, 240, new List<short>
		{
			38, 39, 40, 41, 42, 43, 44, 137, 138, 139,
			140, 141, 142, 143
		}));
		_dataArray.Add(new SkillBookItem(696, 1392, 10, 1001, 3, 693, "icon_SkillBook_kaihejianshu", 1393, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 35, 10, 2250, 4500, 2, 8, 3000, 6, allowRandomCreate: true, 30, isSpecial: false, 4, 36, -1, -1, -1, 7, 552, 400, new List<short>
		{
			39, 40, 41, 42, 43, 44, 138, 139, 140, 141,
			142, 143
		}));
		_dataArray.Add(new SkillBookItem(697, 1394, 10, 1001, 4, 693, "icon_SkillBook_kaihejianshu", 1395, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 30, 10, 4650, 9300, 3, 10, 4200, 7, allowRandomCreate: true, 25, isSpecial: false, 4, 36, -1, -1, -1, 7, 553, 600, new List<short> { 40, 41, 42, 43, 44, 139, 140, 141, 142, 143 }));
		_dataArray.Add(new SkillBookItem(698, 1396, 10, 1001, 5, 693, "icon_SkillBook_kaihejianshu", 1397, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 25, 10, 8400, 16800, 4, 12, 5400, 7, allowRandomCreate: true, 20, isSpecial: false, 4, 36, -1, -1, -1, 7, 554, 850, new List<short> { 41, 42, 43, 44, 140, 141, 142, 143 }));
		_dataArray.Add(new SkillBookItem(699, 1398, 10, 1001, 6, 693, "icon_SkillBook_yinyangnijian", 1399, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 20, 10, 13800, 27600, 5, 14, 7200, 8, allowRandomCreate: true, 15, isSpecial: false, 4, 36, -1, -1, -1, 7, 555, 1140, new List<short> { 42, 43, 44, 141, 142, 143 }));
		_dataArray.Add(new SkillBookItem(700, 1400, 10, 1001, 7, 693, "icon_SkillBook_taishijianqi", 1401, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 15, 10, 21150, 42300, 6, 16, 9000, 8, allowRandomCreate: true, 10, isSpecial: false, 4, 36, -1, -1, -1, 7, 556, 1480, new List<short> { 43, 44, 142, 143 }));
		_dataArray.Add(new SkillBookItem(701, 1402, 10, 1001, 8, 693, "icon_SkillBook_wanhuashisijian", 1403, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 10, 10, 30750, 61500, 6, 18, 10800, 8, allowRandomCreate: true, 5, isSpecial: false, 4, 36, -1, -1, -1, 7, 557, 1860, new List<short> { 44, 143 }));
		_dataArray.Add(new SkillBookItem(702, 1404, 10, 1001, 0, 702, "icon_SkillBook_shierluyuchangcijian", 1405, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 50, 10, 150, 300, 0, 2, 600, 3, allowRandomCreate: true, 45, isSpecial: false, 4, 36, -1, -1, -1, 7, 558, 60, new List<short>
		{
			54, 55, 56, 57, 58, 59, 60, 61, 62, 63,
			64, 65, 66, 67, 68, 69, 70, 71, 90, 91,
			92, 93, 94, 95, 96, 97, 98, 99, 100, 101,
			102, 103, 104, 105, 106, 107
		}));
		_dataArray.Add(new SkillBookItem(703, 1406, 10, 1001, 1, 702, "icon_SkillBook_shierluyuchangcijian", 1407, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 45, 10, 300, 600, 0, 4, 1200, 4, allowRandomCreate: true, 40, isSpecial: false, 4, 36, -1, -1, -1, 7, 559, 130, new List<short>
		{
			55, 56, 57, 58, 59, 60, 61, 62, 64, 65,
			66, 67, 68, 69, 70, 71, 91, 92, 93, 94,
			95, 96, 97, 98, 100, 101, 102, 103, 104, 105,
			106, 107
		}));
		_dataArray.Add(new SkillBookItem(704, 1408, 10, 1001, 2, 702, "icon_SkillBook_shierluyuchangcijian", 1409, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 40, 10, 900, 1800, 1, 6, 1800, 5, allowRandomCreate: true, 35, isSpecial: false, 4, 36, -1, -1, -1, 7, 560, 240, new List<short>
		{
			56, 57, 58, 59, 60, 61, 62, 65, 66, 67,
			68, 69, 70, 71, 92, 93, 94, 95, 96, 97,
			98, 101, 102, 103, 104, 105, 106, 107
		}));
		_dataArray.Add(new SkillBookItem(705, 1410, 10, 1001, 3, 702, "icon_SkillBook_juqueqianjunjian", 1411, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 35, 10, 2250, 4500, 2, 8, 3000, 6, allowRandomCreate: true, 30, isSpecial: false, 4, 36, -1, -1, -1, 7, 561, 400, new List<short>
		{
			57, 58, 59, 60, 61, 62, 66, 67, 68, 69,
			70, 71, 93, 94, 95, 96, 97, 98, 102, 103,
			104, 105, 106, 107
		}));
		_dataArray.Add(new SkillBookItem(706, 1412, 10, 1001, 4, 702, "icon_SkillBook_juqueqianjunjian", 1413, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 30, 10, 4650, 9300, 3, 10, 4200, 7, allowRandomCreate: true, 25, isSpecial: false, 4, 36, -1, -1, -1, 7, 562, 600, new List<short>
		{
			58, 59, 60, 61, 62, 67, 68, 69, 70, 71,
			94, 95, 96, 97, 98, 103, 104, 105, 106, 107
		}));
		_dataArray.Add(new SkillBookItem(707, 1414, 10, 1001, 5, 702, "icon_SkillBook_juqueqianjunjian", 1415, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 25, 10, 8400, 16800, 4, 12, 5400, 7, allowRandomCreate: true, 20, isSpecial: false, 4, 36, -1, -1, -1, 7, 563, 850, new List<short>
		{
			59, 60, 61, 62, 68, 69, 70, 71, 95, 96,
			97, 98, 104, 105, 106, 107
		}));
		_dataArray.Add(new SkillBookItem(708, 1416, 10, 1001, 6, 702, "icon_SkillBook_chunjunjianqi", 1417, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 20, 10, 13800, 27600, 5, 14, 7200, 8, allowRandomCreate: true, 15, isSpecial: false, 4, 36, -1, -1, -1, 7, 564, 1140, new List<short>
		{
			60, 61, 62, 69, 70, 71, 96, 97, 98, 105,
			106, 107
		}));
		_dataArray.Add(new SkillBookItem(709, 1418, 10, 1001, 7, 702, "icon_SkillBook_zhanlujianfa", 1419, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 15, 10, 21150, 42300, 6, 16, 9000, 8, allowRandomCreate: true, 10, isSpecial: false, 4, 36, -1, -1, -1, 7, 565, 1480, new List<short> { 61, 62, 70, 71, 97, 98, 106, 107 }));
		_dataArray.Add(new SkillBookItem(710, 1420, 10, 1001, 0, 710, "icon_SkillBook_luranjianfa", 1421, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 50, 10, 150, 300, 0, 2, 600, 3, allowRandomCreate: true, 45, isSpecial: false, 4, 36, -1, -1, -1, 7, 566, 60, new List<short> { 81, 82, 83, 84, 85, 86, 87, 88, 89 }));
		_dataArray.Add(new SkillBookItem(711, 1422, 10, 1001, 1, 710, "icon_SkillBook_luranjianfa", 1423, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 45, 10, 300, 600, 0, 4, 1200, 4, allowRandomCreate: true, 40, isSpecial: false, 4, 36, -1, -1, -1, 7, 567, 130, new List<short> { 82, 83, 84, 85, 86, 87, 88, 89 }));
		_dataArray.Add(new SkillBookItem(712, 1424, 10, 1001, 2, 710, "icon_SkillBook_luranjianfa", 1425, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 40, 10, 900, 1800, 1, 6, 1800, 5, allowRandomCreate: true, 35, isSpecial: false, 4, 36, -1, -1, -1, 7, 568, 240, new List<short> { 83, 84, 85, 86, 87, 88, 89 }));
		_dataArray.Add(new SkillBookItem(713, 1426, 10, 1001, 3, 710, "icon_SkillBook_shenmubixiejian", 1427, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 35, 10, 2250, 4500, 2, 8, 3000, 6, allowRandomCreate: true, 30, isSpecial: false, 4, 36, -1, -1, -1, 7, 569, 400, new List<short> { 84, 85, 86, 87, 88, 89 }));
		_dataArray.Add(new SkillBookItem(714, 1428, 10, 1001, 4, 710, "icon_SkillBook_shenmubixiejian", 1429, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 30, 10, 4650, 9300, 3, 10, 4200, 7, allowRandomCreate: true, 25, isSpecial: false, 4, 36, -1, -1, -1, 7, 570, 600, new List<short> { 85, 86, 87, 88, 89 }));
		_dataArray.Add(new SkillBookItem(715, 1430, 10, 1001, 5, 710, "icon_SkillBook_shenmubixiejian", 1431, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 25, 10, 8400, 16800, 4, 12, 5400, 7, allowRandomCreate: true, 20, isSpecial: false, 4, 36, -1, -1, -1, 7, 571, 850, new List<short> { 86, 87, 88, 89 }));
		_dataArray.Add(new SkillBookItem(716, 1432, 10, 1001, 6, 710, "icon_SkillBook_qingzhujianfa", 1433, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 20, 10, 13800, 27600, 5, 14, 7200, 8, allowRandomCreate: true, 15, isSpecial: false, 4, 36, -1, -1, -1, 7, 572, 1140, new List<short> { 87, 88, 89 }));
		_dataArray.Add(new SkillBookItem(717, 1434, 10, 1001, 0, 717, "icon_SkillBook_jieqingkuaijian", 1435, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 50, 10, 150, 300, 0, 2, 600, 3, allowRandomCreate: true, 45, isSpecial: false, 4, 36, -1, -1, -1, 7, 573, 60, new List<short> { 36, 37, 38, 39, 40, 41, 42, 43, 44 }));
		_dataArray.Add(new SkillBookItem(718, 1436, 10, 1001, 1, 717, "icon_SkillBook_jieqingkuaijian", 1437, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 45, 10, 300, 600, 0, 4, 1200, 4, allowRandomCreate: true, 40, isSpecial: false, 4, 36, -1, -1, -1, 7, 574, 130, new List<short> { 37, 38, 39, 40, 41, 42, 43, 44 }));
		_dataArray.Add(new SkillBookItem(719, 1438, 10, 1001, 2, 717, "icon_SkillBook_jieqingkuaijian", 1439, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 40, 10, 900, 1800, 1, 6, 1800, 5, allowRandomCreate: true, 35, isSpecial: false, 4, 36, -1, -1, -1, 7, 575, 240, new List<short> { 38, 39, 40, 41, 42, 43, 44 }));
	}

	private void CreateItems12()
	{
		_dataArray.Add(new SkillBookItem(720, 1440, 10, 1001, 3, 717, "icon_SkillBook_jieqinganshoukuaijian", 1441, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 35, 10, 2250, 4500, 2, 8, 3000, 6, allowRandomCreate: true, 30, isSpecial: false, 4, 36, -1, -1, -1, 7, 576, 400, new List<short> { 39, 40, 41, 42, 43, 44 }));
		_dataArray.Add(new SkillBookItem(721, 1442, 10, 1001, 4, 717, "icon_SkillBook_jieqinganshoukuaijian", 1443, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 30, 10, 4650, 9300, 3, 10, 4200, 7, allowRandomCreate: true, 25, isSpecial: false, 4, 36, -1, -1, -1, 7, 577, 600, new List<short> { 40, 41, 42, 43, 44 }));
		_dataArray.Add(new SkillBookItem(722, 1444, 10, 1001, 5, 717, "icon_SkillBook_jieqinganshoukuaijian", 1445, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 25, 10, 8400, 16800, 4, 12, 5400, 7, allowRandomCreate: true, 20, isSpecial: false, 4, 36, -1, -1, -1, 7, 578, 850, new List<short> { 41, 42, 43, 44 }));
		_dataArray.Add(new SkillBookItem(723, 1446, 10, 1001, 6, 717, "icon_SkillBook_wuxiaqijuejian", 1447, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 20, 10, 13800, 27600, 5, 14, 7200, 8, allowRandomCreate: true, 15, isSpecial: false, 4, 36, -1, -1, -1, 7, 579, 1140, new List<short> { 42, 43, 44 }));
		_dataArray.Add(new SkillBookItem(724, 1448, 10, 1001, 7, 717, "icon_SkillBook_xuanmingjianqi", 1449, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 15, 10, 21150, 42300, 6, 16, 9000, 8, allowRandomCreate: true, 10, isSpecial: false, 4, 36, -1, -1, -1, 7, 580, 1480, new List<short> { 43, 44 }));
		_dataArray.Add(new SkillBookItem(725, 1450, 10, 1001, 0, 725, "icon_SkillBook_baguawuhangdao", 1451, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 50, 10, 150, 300, 0, 2, 600, 3, allowRandomCreate: true, 45, isSpecial: false, 4, 36, -1, -1, -1, 8, 581, 60, new List<short> { 108, 109, 110, 111, 112, 113, 114, 115, 116 }));
		_dataArray.Add(new SkillBookItem(726, 1452, 10, 1001, 1, 725, "icon_SkillBook_baguawuhangdao", 1453, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 45, 10, 300, 600, 0, 4, 1200, 4, allowRandomCreate: true, 40, isSpecial: false, 4, 36, -1, -1, -1, 8, 582, 130, new List<short> { 109, 110, 111, 112, 113, 114, 115, 116 }));
		_dataArray.Add(new SkillBookItem(727, 1454, 10, 1001, 2, 725, "icon_SkillBook_baguawuhangdao", 1455, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 40, 10, 900, 1800, 1, 6, 1800, 5, allowRandomCreate: true, 35, isSpecial: false, 4, 36, -1, -1, -1, 8, 583, 240, new List<short> { 110, 111, 112, 113, 114, 115, 116 }));
		_dataArray.Add(new SkillBookItem(728, 1456, 10, 1001, 3, 725, "icon_SkillBook_taiyijindao", 1457, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 35, 10, 2250, 4500, 2, 8, 3000, 6, allowRandomCreate: true, 30, isSpecial: false, 4, 36, -1, -1, -1, 8, 584, 400, new List<short> { 111, 112, 113, 114, 115, 116 }));
		_dataArray.Add(new SkillBookItem(729, 1458, 10, 1001, 4, 725, "icon_SkillBook_taiyijindao", 1459, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 30, 10, 4650, 9300, 3, 10, 4200, 7, allowRandomCreate: true, 25, isSpecial: false, 4, 36, -1, -1, -1, 8, 585, 600, new List<short> { 112, 113, 114, 115, 116 }));
		_dataArray.Add(new SkillBookItem(730, 1460, 10, 1001, 5, 725, "icon_SkillBook_taiyijindao", 1461, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 25, 10, 8400, 16800, 4, 12, 5400, 7, allowRandomCreate: true, 20, isSpecial: false, 4, 36, -1, -1, -1, 8, 586, 850, new List<short> { 113, 114, 115, 116 }));
		_dataArray.Add(new SkillBookItem(731, 1462, 10, 1001, 6, 725, "icon_SkillBook_tiangangdaofa", 1463, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 20, 10, 13800, 27600, 5, 14, 7200, 8, allowRandomCreate: true, 15, isSpecial: false, 4, 36, -1, -1, -1, 8, 587, 1140, new List<short> { 114, 115, 116 }));
		_dataArray.Add(new SkillBookItem(732, 1464, 10, 1001, 7, 725, "icon_SkillBook_taixuanshendao", 1465, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 15, 10, 21150, 42300, 6, 16, 9000, 8, allowRandomCreate: true, 10, isSpecial: false, 4, 36, -1, -1, -1, 8, 588, 1480, new List<short> { 115, 116 }));
		_dataArray.Add(new SkillBookItem(733, 1466, 10, 1001, 0, 733, "icon_SkillBook_shixiangdetangdao", 1467, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 50, 10, 150, 300, 0, 2, 600, 3, allowRandomCreate: true, 45, isSpecial: false, 4, 36, -1, -1, -1, 8, 589, 60, new List<short>()));
		_dataArray.Add(new SkillBookItem(734, 1468, 10, 1001, 1, 733, "icon_SkillBook_shixiangdetangdao", 1469, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 45, 10, 300, 600, 0, 4, 1200, 4, allowRandomCreate: true, 40, isSpecial: false, 4, 36, -1, -1, -1, 8, 590, 130, new List<short>()));
		_dataArray.Add(new SkillBookItem(735, 1470, 10, 1001, 2, 733, "icon_SkillBook_shixiangdetangdao", 1471, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 40, 10, 900, 1800, 1, 6, 1800, 5, allowRandomCreate: true, 35, isSpecial: false, 4, 36, -1, -1, -1, 8, 591, 240, new List<short>()));
		_dataArray.Add(new SkillBookItem(736, 1472, 10, 1001, 3, 733, "icon_SkillBook_jiuniuerhudao", 1473, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 35, 10, 2250, 4500, 2, 8, 3000, 6, allowRandomCreate: true, 30, isSpecial: false, 4, 36, -1, -1, -1, 8, 592, 400, new List<short>()));
		_dataArray.Add(new SkillBookItem(737, 1474, 10, 1001, 4, 733, "icon_SkillBook_jiuniuerhudao", 1475, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 30, 10, 4650, 9300, 3, 10, 4200, 7, allowRandomCreate: true, 25, isSpecial: false, 4, 36, -1, -1, -1, 8, 593, 600, new List<short>()));
		_dataArray.Add(new SkillBookItem(738, 1476, 10, 1001, 5, 733, "icon_SkillBook_jiuniuerhudao", 1477, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 25, 10, 8400, 16800, 4, 12, 5400, 7, allowRandomCreate: true, 20, isSpecial: false, 4, 36, -1, -1, -1, 8, 594, 850, new List<short>()));
		_dataArray.Add(new SkillBookItem(739, 1478, 10, 1001, 6, 733, "icon_SkillBook_jinnizhenmodao", 1479, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 20, 10, 13800, 27600, 5, 14, 7200, 8, allowRandomCreate: true, 15, isSpecial: false, 4, 36, -1, -1, -1, 8, 595, 1140, new List<short>()));
		_dataArray.Add(new SkillBookItem(740, 1480, 10, 1001, 7, 733, "icon_SkillBook_guibashipanlongdao", 1481, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 15, 10, 21150, 42300, 6, 16, 9000, 8, allowRandomCreate: true, 10, isSpecial: false, 4, 36, -1, -1, -1, 8, 596, 1480, new List<short>()));
		_dataArray.Add(new SkillBookItem(741, 1482, 10, 1001, 8, 733, "icon_SkillBook_kuangdao", 1483, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 10, 10, 30750, 61500, 6, 18, 10800, 8, allowRandomCreate: true, 5, isSpecial: false, 4, 36, -1, -1, -1, 8, 597, 1860, new List<short>()));
		_dataArray.Add(new SkillBookItem(742, 1484, 10, 1001, 0, 742, "icon_SkillBook_xintinghoudaofa", 1485, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 50, 10, 150, 300, 0, 2, 600, 3, allowRandomCreate: true, 45, isSpecial: false, 4, 36, -1, -1, -1, 8, 598, 60, new List<short>
		{
			54, 55, 56, 57, 58, 59, 60, 61, 62, 63,
			64, 65, 66, 67, 68, 69, 70, 71, 90, 91,
			92, 93, 94, 95, 96, 97, 98, 99, 100, 101,
			102, 103, 104, 105, 106, 107
		}));
		_dataArray.Add(new SkillBookItem(743, 1486, 10, 1001, 1, 742, "icon_SkillBook_xintinghoudaofa", 1487, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 45, 10, 300, 600, 0, 4, 1200, 4, allowRandomCreate: true, 40, isSpecial: false, 4, 36, -1, -1, -1, 8, 599, 130, new List<short>
		{
			55, 56, 57, 58, 59, 60, 61, 62, 64, 65,
			66, 67, 68, 69, 70, 71, 91, 92, 93, 94,
			95, 96, 97, 98, 100, 101, 102, 103, 104, 105,
			106, 107
		}));
		_dataArray.Add(new SkillBookItem(744, 1488, 10, 1001, 2, 742, "icon_SkillBook_xintinghoudaofa", 1489, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 40, 10, 900, 1800, 1, 6, 1800, 5, allowRandomCreate: true, 35, isSpecial: false, 4, 36, -1, -1, -1, 8, 600, 240, new List<short>
		{
			56, 57, 58, 59, 60, 61, 62, 65, 66, 67,
			68, 69, 70, 71, 92, 93, 94, 95, 96, 97,
			98, 101, 102, 103, 104, 105, 106, 107
		}));
		_dataArray.Add(new SkillBookItem(745, 1490, 10, 1001, 3, 742, "icon_SkillBook_kunwudaofa", 1491, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 35, 10, 2250, 4500, 2, 8, 3000, 6, allowRandomCreate: true, 30, isSpecial: false, 4, 36, -1, -1, -1, 8, 601, 400, new List<short>
		{
			57, 58, 59, 60, 61, 62, 66, 67, 68, 69,
			70, 71, 93, 94, 95, 96, 97, 98, 102, 103,
			104, 105, 106, 107
		}));
		_dataArray.Add(new SkillBookItem(746, 1492, 10, 1001, 4, 742, "icon_SkillBook_kunwudaofa", 1493, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 30, 10, 4650, 9300, 3, 10, 4200, 7, allowRandomCreate: true, 25, isSpecial: false, 4, 36, -1, -1, -1, 8, 602, 600, new List<short>
		{
			58, 59, 60, 61, 62, 67, 68, 69, 70, 71,
			94, 95, 96, 97, 98, 103, 104, 105, 106, 107
		}));
		_dataArray.Add(new SkillBookItem(747, 1494, 10, 1001, 5, 742, "icon_SkillBook_kunwudaofa", 1495, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 25, 10, 8400, 16800, 4, 12, 5400, 7, allowRandomCreate: true, 20, isSpecial: false, 4, 36, -1, -1, -1, 8, 603, 850, new List<short>
		{
			59, 60, 61, 62, 68, 69, 70, 71, 95, 96,
			97, 98, 104, 105, 106, 107
		}));
		_dataArray.Add(new SkillBookItem(748, 1496, 10, 1001, 6, 742, "icon_SkillBook_candaoshi", 1497, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 20, 10, 13800, 27600, 5, 14, 7200, 8, allowRandomCreate: true, 15, isSpecial: false, 4, 36, -1, -1, -1, 8, 604, 1140, new List<short>
		{
			60, 61, 62, 69, 70, 71, 96, 97, 98, 105,
			106, 107
		}));
		_dataArray.Add(new SkillBookItem(749, 1498, 10, 1001, 7, 742, "icon_SkillBook_minghongjuedao", 1499, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 15, 10, 21150, 42300, 6, 16, 9000, 8, allowRandomCreate: true, 10, isSpecial: false, 4, 36, -1, -1, -1, 8, 605, 1480, new List<short> { 61, 62, 70, 71, 97, 98, 106, 107 }));
		_dataArray.Add(new SkillBookItem(750, 1500, 10, 1001, 0, 750, "icon_SkillBook_jingangdaofa", 1501, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 50, 10, 150, 300, 0, 2, 600, 3, allowRandomCreate: true, 45, isSpecial: false, 4, 36, -1, -1, -1, 8, 606, 60, new List<short> { 117, 118, 119, 120, 121, 122, 123, 124, 125 }));
		_dataArray.Add(new SkillBookItem(751, 1502, 10, 1001, 1, 750, "icon_SkillBook_jingangdaofa", 1503, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 45, 10, 300, 600, 0, 4, 1200, 4, allowRandomCreate: true, 40, isSpecial: false, 4, 36, -1, -1, -1, 8, 607, 130, new List<short> { 118, 119, 120, 121, 122, 123, 124, 125 }));
		_dataArray.Add(new SkillBookItem(752, 1504, 10, 1001, 2, 750, "icon_SkillBook_jingangdaofa", 1505, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 40, 10, 900, 1800, 1, 6, 1800, 5, allowRandomCreate: true, 35, isSpecial: false, 4, 36, -1, -1, -1, 8, 608, 240, new List<short> { 119, 120, 121, 122, 123, 124, 125 }));
		_dataArray.Add(new SkillBookItem(753, 1506, 10, 1001, 3, 750, "icon_SkillBook_lingredao", 1507, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 35, 10, 2250, 4500, 2, 8, 3000, 6, allowRandomCreate: true, 30, isSpecial: false, 4, 36, -1, -1, -1, 8, 609, 400, new List<short> { 120, 121, 122, 123, 124, 125 }));
		_dataArray.Add(new SkillBookItem(754, 1508, 10, 1001, 4, 750, "icon_SkillBook_lingredao", 1509, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 30, 10, 4650, 9300, 3, 10, 4200, 7, allowRandomCreate: true, 25, isSpecial: false, 4, 36, -1, -1, -1, 8, 610, 600, new List<short> { 121, 122, 123, 124, 125 }));
		_dataArray.Add(new SkillBookItem(755, 1510, 10, 1001, 5, 750, "icon_SkillBook_lingredao", 1511, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 25, 10, 8400, 16800, 4, 12, 5400, 7, allowRandomCreate: true, 20, isSpecial: false, 4, 36, -1, -1, -1, 8, 611, 850, new List<short> { 122, 123, 124, 125 }));
		_dataArray.Add(new SkillBookItem(756, 1512, 10, 1001, 6, 750, "icon_SkillBook_ruyibaolundao", 1513, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 20, 10, 13800, 27600, 5, 14, 7200, 8, allowRandomCreate: true, 15, isSpecial: false, 4, 36, -1, -1, -1, 8, 612, 1140, new List<short> { 123, 124, 125 }));
		_dataArray.Add(new SkillBookItem(757, 1514, 10, 1001, 7, 750, "icon_SkillBook_liushenweidao", 1515, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 15, 10, 21150, 42300, 6, 16, 9000, 8, allowRandomCreate: true, 10, isSpecial: false, 4, 36, -1, -1, -1, 8, 613, 1480, new List<short> { 124, 125 }));
		_dataArray.Add(new SkillBookItem(758, 1516, 10, 1001, 8, 750, "icon_SkillBook_mohejialuodao", 1517, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 10, 10, 30750, 61500, 6, 18, 10800, 8, allowRandomCreate: true, 5, isSpecial: false, 4, 36, -1, -1, -1, 8, 614, 1860, new List<short> { 125 }));
		_dataArray.Add(new SkillBookItem(759, 1518, 10, 1001, 0, 759, "icon_SkillBook_fulongdaofa", 1519, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 50, 10, 150, 300, 0, 2, 600, 3, allowRandomCreate: true, 45, isSpecial: false, 4, 36, -1, -1, -1, 8, 615, 60, new List<short>
		{
			126, 127, 128, 129, 130, 131, 132, 133, 134, 45,
			46, 47, 48, 49, 50, 51, 52, 53
		}));
		_dataArray.Add(new SkillBookItem(760, 1520, 10, 1001, 1, 759, "icon_SkillBook_fulongdaofa", 1521, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 45, 10, 300, 600, 0, 4, 1200, 4, allowRandomCreate: true, 40, isSpecial: false, 4, 36, -1, -1, -1, 8, 616, 130, new List<short>
		{
			127, 128, 129, 130, 131, 132, 133, 134, 46, 47,
			48, 49, 50, 51, 52, 53
		}));
		_dataArray.Add(new SkillBookItem(761, 1522, 10, 1001, 2, 759, "icon_SkillBook_fulongdaofa", 1523, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 40, 10, 900, 1800, 1, 6, 1800, 5, allowRandomCreate: true, 35, isSpecial: false, 4, 36, -1, -1, -1, 8, 617, 240, new List<short>
		{
			128, 129, 130, 131, 132, 133, 134, 47, 48, 49,
			50, 51, 52, 53
		}));
		_dataArray.Add(new SkillBookItem(762, 1524, 10, 1001, 3, 759, "icon_SkillBook_liuxinsiyidao", 1525, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 35, 10, 2250, 4500, 2, 8, 3000, 6, allowRandomCreate: true, 30, isSpecial: false, 4, 36, -1, -1, -1, 8, 618, 400, new List<short>
		{
			129, 130, 131, 132, 133, 134, 48, 49, 50, 51,
			52, 53
		}));
		_dataArray.Add(new SkillBookItem(763, 1526, 10, 1001, 4, 759, "icon_SkillBook_liuxinsiyidao", 1527, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 30, 10, 4650, 9300, 3, 10, 4200, 7, allowRandomCreate: true, 25, isSpecial: false, 4, 36, -1, -1, -1, 8, 619, 600, new List<short> { 130, 131, 132, 133, 134, 49, 50, 51, 52, 53 }));
		_dataArray.Add(new SkillBookItem(764, 1528, 10, 1001, 5, 759, "icon_SkillBook_liuxinsiyidao", 1529, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 25, 10, 8400, 16800, 4, 12, 5400, 7, allowRandomCreate: true, 20, isSpecial: false, 4, 36, -1, -1, -1, 8, 620, 850, new List<short> { 131, 132, 133, 134, 50, 51, 52, 53 }));
		_dataArray.Add(new SkillBookItem(765, 1530, 10, 1001, 6, 759, "icon_SkillBook_chizizhanlongdao", 1531, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 20, 10, 13800, 27600, 5, 14, 7200, 8, allowRandomCreate: true, 15, isSpecial: false, 4, 36, -1, -1, -1, 8, 621, 1140, new List<short> { 132, 133, 134, 51, 52, 53 }));
		_dataArray.Add(new SkillBookItem(766, 1532, 10, 1001, 7, 759, "icon_SkillBook_jiulongbadao", 1533, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 15, 10, 21150, 42300, 6, 16, 9000, 8, allowRandomCreate: true, 10, isSpecial: false, 4, 36, -1, -1, -1, 8, 622, 1480, new List<short> { 133, 134, 52, 53 }));
		_dataArray.Add(new SkillBookItem(767, 1534, 10, 1001, 0, 767, "icon_SkillBook_shaolinLuhegun", 1535, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 50, 10, 150, 300, 0, 2, 600, 3, allowRandomCreate: true, 45, isSpecial: false, 4, 36, -1, -1, -1, 9, 623, 60, new List<short> { 117, 118, 119, 120, 121, 122, 123, 124, 125 }));
		_dataArray.Add(new SkillBookItem(768, 1536, 10, 1001, 1, 767, "icon_SkillBook_shaolinLuhegun", 1537, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 45, 10, 300, 600, 0, 4, 1200, 4, allowRandomCreate: true, 40, isSpecial: false, 4, 36, -1, -1, -1, 9, 624, 130, new List<short> { 118, 119, 120, 121, 122, 123, 124, 125 }));
		_dataArray.Add(new SkillBookItem(769, 1538, 10, 1001, 2, 767, "icon_SkillBook_shaolinLuhegun", 1539, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 40, 10, 900, 1800, 1, 6, 1800, 5, allowRandomCreate: true, 35, isSpecial: false, 4, 36, -1, -1, -1, 9, 625, 240, new List<short> { 119, 120, 121, 122, 123, 124, 125 }));
		_dataArray.Add(new SkillBookItem(770, 1540, 10, 1001, 3, 767, "icon_SkillBook_daxiaoyechagun", 1541, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 35, 10, 2250, 4500, 2, 8, 3000, 6, allowRandomCreate: true, 30, isSpecial: false, 4, 36, -1, -1, -1, 9, 626, 400, new List<short> { 120, 121, 122, 123, 124, 125 }));
		_dataArray.Add(new SkillBookItem(771, 1542, 10, 1001, 4, 767, "icon_SkillBook_daxiaoyechagun", 1543, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 30, 10, 4650, 9300, 3, 10, 4200, 7, allowRandomCreate: true, 25, isSpecial: false, 4, 36, -1, -1, -1, 9, 627, 600, new List<short> { 121, 122, 123, 124, 125 }));
		_dataArray.Add(new SkillBookItem(772, 1544, 10, 1001, 5, 767, "icon_SkillBook_daxiaoyechagun", 1545, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 25, 10, 8400, 16800, 4, 12, 5400, 7, allowRandomCreate: true, 20, isSpecial: false, 4, 36, -1, -1, -1, 9, 628, 850, new List<short> { 122, 123, 124, 125 }));
		_dataArray.Add(new SkillBookItem(773, 1546, 10, 1001, 6, 767, "icon_SkillBook_weituojiangmozhang", 1547, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 20, 10, 13800, 27600, 5, 14, 7200, 8, allowRandomCreate: true, 15, isSpecial: false, 4, 36, -1, -1, -1, 9, 629, 1140, new List<short> { 123, 124, 125 }));
		_dataArray.Add(new SkillBookItem(774, 1548, 10, 1001, 7, 767, "icon_SkillBook_dazhiputizhangfa", 1549, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 15, 10, 21150, 42300, 6, 16, 9000, 8, allowRandomCreate: true, 10, isSpecial: false, 4, 36, -1, -1, -1, 9, 630, 1480, new List<short> { 124, 125 }));
		_dataArray.Add(new SkillBookItem(775, 1550, 10, 1001, 8, 767, "icon_SkillBook_damozhangfa", 1551, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 10, 10, 30750, 61500, 6, 18, 10800, 8, allowRandomCreate: true, 5, isSpecial: false, 4, 36, -1, -1, -1, 9, 631, 1860, new List<short> { 125 }));
		_dataArray.Add(new SkillBookItem(776, 1552, 10, 1001, 0, 776, "icon_SkillBook_eshaqiangfa", 1553, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 50, 10, 150, 300, 0, 2, 600, 3, allowRandomCreate: true, 45, isSpecial: false, 4, 36, -1, -1, -1, 9, 632, 60, new List<short>()));
		_dataArray.Add(new SkillBookItem(777, 1554, 10, 1001, 1, 776, "icon_SkillBook_eshaqiangfa", 1555, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 45, 10, 300, 600, 0, 4, 1200, 4, allowRandomCreate: true, 40, isSpecial: false, 4, 36, -1, -1, -1, 9, 633, 130, new List<short>()));
		_dataArray.Add(new SkillBookItem(778, 1556, 10, 1001, 2, 776, "icon_SkillBook_eshaqiangfa", 1557, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 40, 10, 900, 1800, 1, 6, 1800, 5, allowRandomCreate: true, 35, isSpecial: false, 4, 36, -1, -1, -1, 9, 634, 240, new List<short>()));
		_dataArray.Add(new SkillBookItem(779, 1558, 10, 1001, 3, 776, "icon_SkillBook_huanhoushiliuqiang", 1559, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 35, 10, 2250, 4500, 2, 8, 3000, 6, allowRandomCreate: true, 30, isSpecial: false, 4, 36, -1, -1, -1, 9, 635, 400, new List<short>()));
	}

	private void CreateItems13()
	{
		_dataArray.Add(new SkillBookItem(780, 1560, 10, 1001, 4, 776, "icon_SkillBook_huanhoushiliuqiang", 1561, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 30, 10, 4650, 9300, 3, 10, 4200, 7, allowRandomCreate: true, 25, isSpecial: false, 4, 36, -1, -1, -1, 9, 636, 600, new List<short>()));
		_dataArray.Add(new SkillBookItem(781, 1562, 10, 1001, 5, 776, "icon_SkillBook_huanhoushiliuqiang", 1563, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 25, 10, 8400, 16800, 4, 12, 5400, 7, allowRandomCreate: true, 20, isSpecial: false, 4, 36, -1, -1, -1, 9, 637, 850, new List<short>()));
		_dataArray.Add(new SkillBookItem(782, 1564, 10, 1001, 6, 776, "icon_SkillBook_kuanglongshiziqiang", 1565, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 20, 10, 13800, 27600, 5, 14, 7200, 8, allowRandomCreate: true, 15, isSpecial: false, 4, 36, -1, -1, -1, 9, 638, 1140, new List<short>()));
		_dataArray.Add(new SkillBookItem(783, 1566, 10, 1001, 7, 776, "icon_SkillBook_tuolongbawangqiang", 1567, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 15, 10, 21150, 42300, 6, 16, 9000, 8, allowRandomCreate: true, 10, isSpecial: false, 4, 36, -1, -1, -1, 9, 639, 1480, new List<short>()));
		_dataArray.Add(new SkillBookItem(784, 1568, 10, 1001, 0, 784, "icon_SkillBook_jianglonggunfa", 1569, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 50, 10, 150, 300, 0, 2, 600, 3, allowRandomCreate: true, 45, isSpecial: false, 4, 36, -1, -1, -1, 9, 640, 60, new List<short>
		{
			54, 55, 56, 57, 58, 59, 60, 61, 62, 63,
			64, 65, 66, 67, 68, 69, 70, 71, 90, 91,
			92, 93, 94, 95, 96, 97, 98, 99, 100, 101,
			102, 103, 104, 105, 106, 107
		}));
		_dataArray.Add(new SkillBookItem(785, 1570, 10, 1001, 1, 784, "icon_SkillBook_jianglonggunfa", 1571, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 45, 10, 300, 600, 0, 4, 1200, 4, allowRandomCreate: true, 40, isSpecial: false, 4, 36, -1, -1, -1, 9, 641, 130, new List<short>
		{
			55, 56, 57, 58, 59, 60, 61, 62, 64, 65,
			66, 67, 68, 69, 70, 71, 91, 92, 93, 94,
			95, 96, 97, 98, 100, 101, 102, 103, 104, 105,
			106, 107
		}));
		_dataArray.Add(new SkillBookItem(786, 1572, 10, 1001, 2, 784, "icon_SkillBook_jianglonggunfa", 1573, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 40, 10, 900, 1800, 1, 6, 1800, 5, allowRandomCreate: true, 35, isSpecial: false, 4, 36, -1, -1, -1, 9, 642, 240, new List<short>
		{
			56, 57, 58, 59, 60, 61, 62, 65, 66, 67,
			68, 69, 70, 71, 92, 93, 94, 95, 96, 97,
			98, 101, 102, 103, 104, 105, 106, 107
		}));
		_dataArray.Add(new SkillBookItem(787, 1574, 10, 1001, 3, 784, "icon_SkillBook_guitoubangfa", 1575, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 35, 10, 2250, 4500, 2, 8, 3000, 6, allowRandomCreate: true, 30, isSpecial: false, 4, 36, -1, -1, -1, 9, 643, 400, new List<short>
		{
			57, 58, 59, 60, 61, 62, 66, 67, 68, 69,
			70, 71, 93, 94, 95, 96, 97, 98, 102, 103,
			104, 105, 106, 107
		}));
		_dataArray.Add(new SkillBookItem(788, 1576, 10, 1001, 4, 784, "icon_SkillBook_guitoubangfa", 1577, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 30, 10, 4650, 9300, 3, 10, 4200, 7, allowRandomCreate: true, 25, isSpecial: false, 4, 36, -1, -1, -1, 9, 644, 600, new List<short>
		{
			58, 59, 60, 61, 62, 67, 68, 69, 70, 71,
			94, 95, 96, 97, 98, 103, 104, 105, 106, 107
		}));
		_dataArray.Add(new SkillBookItem(789, 1578, 10, 1001, 5, 784, "icon_SkillBook_guitoubangfa", 1579, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 25, 10, 8400, 16800, 4, 12, 5400, 7, allowRandomCreate: true, 20, isSpecial: false, 4, 36, -1, -1, -1, 9, 645, 850, new List<short>
		{
			59, 60, 61, 62, 68, 69, 70, 71, 95, 96,
			97, 98, 104, 105, 106, 107
		}));
		_dataArray.Add(new SkillBookItem(790, 1580, 10, 1001, 6, 784, "icon_SkillBook_jiahaishenzhang", 1581, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 20, 10, 13800, 27600, 5, 14, 7200, 8, allowRandomCreate: true, 15, isSpecial: false, 4, 36, -1, -1, -1, 9, 646, 1140, new List<short>
		{
			60, 61, 62, 69, 70, 71, 96, 97, 98, 105,
			106, 107
		}));
		_dataArray.Add(new SkillBookItem(791, 1582, 10, 1001, 7, 784, "icon_SkillBook_hunyuantiebangfa", 1583, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 15, 10, 21150, 42300, 6, 16, 9000, 8, allowRandomCreate: true, 10, isSpecial: false, 4, 36, -1, -1, -1, 9, 647, 1480, new List<short> { 61, 62, 70, 71, 97, 98, 106, 107 }));
		_dataArray.Add(new SkillBookItem(792, 1584, 10, 1001, 0, 792, "icon_SkillBook_wuhangci", 1585, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 50, 10, 150, 300, 0, 2, 600, 3, allowRandomCreate: true, 45, isSpecial: false, 4, 36, -1, -1, -1, 10, 648, 60, new List<short> { 108, 109, 110, 111, 112, 113, 114, 115, 116 }));
		_dataArray.Add(new SkillBookItem(793, 1586, 10, 1001, 1, 792, "icon_SkillBook_wuhangci", 1587, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 45, 10, 300, 600, 0, 4, 1200, 4, allowRandomCreate: true, 40, isSpecial: false, 4, 36, -1, -1, -1, 10, 649, 130, new List<short> { 109, 110, 111, 112, 113, 114, 115, 116 }));
		_dataArray.Add(new SkillBookItem(794, 1588, 10, 1001, 2, 792, "icon_SkillBook_wuhangci", 1589, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 40, 10, 900, 1800, 1, 6, 1800, 5, allowRandomCreate: true, 35, isSpecial: false, 4, 36, -1, -1, -1, 10, 650, 240, new List<short> { 110, 111, 112, 113, 114, 115, 116 }));
		_dataArray.Add(new SkillBookItem(795, 1590, 10, 1001, 3, 792, "icon_SkillBook_lingsheci", 1591, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 35, 10, 2250, 4500, 2, 8, 3000, 6, allowRandomCreate: true, 30, isSpecial: false, 4, 36, -1, -1, -1, 10, 651, 400, new List<short> { 111, 112, 113, 114, 115, 116 }));
		_dataArray.Add(new SkillBookItem(796, 1592, 10, 1001, 4, 792, "icon_SkillBook_lingsheci", 1593, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 30, 10, 4650, 9300, 3, 10, 4200, 7, allowRandomCreate: true, 25, isSpecial: false, 4, 36, -1, -1, -1, 10, 652, 600, new List<short> { 112, 113, 114, 115, 116 }));
		_dataArray.Add(new SkillBookItem(797, 1594, 10, 1001, 5, 792, "icon_SkillBook_lingsheci", 1595, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 25, 10, 8400, 16800, 4, 12, 5400, 7, allowRandomCreate: true, 20, isSpecial: false, 4, 36, -1, -1, -1, 10, 653, 850, new List<short> { 113, 114, 115, 116 }));
		_dataArray.Add(new SkillBookItem(798, 1596, 10, 1001, 6, 792, "icon_SkillBook_yunvci", 1597, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 20, 10, 13800, 27600, 5, 14, 7200, 8, allowRandomCreate: true, 15, isSpecial: false, 4, 36, -1, -1, -1, 10, 654, 1140, new List<short> { 114, 115, 116 }));
		_dataArray.Add(new SkillBookItem(799, 1598, 10, 1001, 7, 792, "icon_SkillBook_jindingxianzhen", 1599, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 15, 10, 21150, 42300, 6, 16, 9000, 8, allowRandomCreate: true, 10, isSpecial: false, 4, 36, -1, -1, -1, 10, 655, 1480, new List<short> { 115, 116 }));
		_dataArray.Add(new SkillBookItem(800, 1600, 10, 1001, 0, 800, "icon_SkillBook_diedameishanzhou", 1601, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 50, 10, 150, 300, 0, 2, 600, 3, allowRandomCreate: true, 45, isSpecial: false, 4, 36, -1, -1, -1, 10, 656, 60, new List<short>
		{
			36, 37, 38, 39, 40, 41, 42, 43, 44, 135,
			136, 137, 138, 139, 140, 141, 142, 143
		}));
		_dataArray.Add(new SkillBookItem(801, 1602, 10, 1001, 1, 800, "icon_SkillBook_diedameishanzhou", 1603, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 45, 10, 300, 600, 0, 4, 1200, 4, allowRandomCreate: true, 40, isSpecial: false, 4, 36, -1, -1, -1, 10, 657, 130, new List<short>
		{
			37, 38, 39, 40, 41, 42, 43, 44, 136, 137,
			138, 139, 140, 141, 142, 143
		}));
		_dataArray.Add(new SkillBookItem(802, 1604, 10, 1001, 2, 800, "icon_SkillBook_diedameishanzhou", 1605, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 40, 10, 900, 1800, 1, 6, 1800, 5, allowRandomCreate: true, 35, isSpecial: false, 4, 36, -1, -1, -1, 10, 658, 240, new List<short>
		{
			38, 39, 40, 41, 42, 43, 44, 137, 138, 139,
			140, 141, 142, 143
		}));
		_dataArray.Add(new SkillBookItem(803, 1606, 10, 1001, 3, 800, "icon_SkillBook_wuleizhaolaizhou", 1607, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 35, 10, 2250, 4500, 2, 8, 3000, 6, allowRandomCreate: true, 30, isSpecial: false, 4, 36, -1, -1, -1, 10, 659, 400, new List<short>
		{
			39, 40, 41, 42, 43, 44, 138, 139, 140, 141,
			142, 143
		}));
		_dataArray.Add(new SkillBookItem(804, 1608, 10, 1001, 4, 800, "icon_SkillBook_wuleizhaolaizhou", 1609, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 30, 10, 4650, 9300, 3, 10, 4200, 7, allowRandomCreate: true, 25, isSpecial: false, 4, 36, -1, -1, -1, 10, 660, 600, new List<short> { 40, 41, 42, 43, 44, 139, 140, 141, 142, 143 }));
		_dataArray.Add(new SkillBookItem(805, 1610, 10, 1001, 5, 800, "icon_SkillBook_wuleizhaolaizhou", 1611, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 25, 10, 8400, 16800, 4, 12, 5400, 7, allowRandomCreate: true, 20, isSpecial: false, 4, 36, -1, -1, -1, 10, 661, 850, new List<short> { 41, 42, 43, 44, 140, 141, 142, 143 }));
		_dataArray.Add(new SkillBookItem(806, 1612, 10, 1001, 6, 800, "icon_SkillBook_qibaiposhenfa", 1613, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 20, 10, 13800, 27600, 5, 14, 7200, 8, allowRandomCreate: true, 15, isSpecial: false, 4, 36, -1, -1, -1, 10, 662, 1140, new List<short> { 42, 43, 44, 141, 142, 143 }));
		_dataArray.Add(new SkillBookItem(807, 1614, 10, 1001, 7, 800, "icon_SkillBook_jiuxingjinguangzhou", 1615, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 15, 10, 21150, 42300, 6, 16, 9000, 8, allowRandomCreate: true, 10, isSpecial: false, 4, 36, -1, -1, -1, 10, 663, 1480, new List<short> { 43, 44, 142, 143 }));
		_dataArray.Add(new SkillBookItem(808, 1616, 10, 1001, 8, 800, "icon_SkillBook_fujieshentong", 1617, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 10, 10, 30750, 61500, 6, 18, 10800, 8, allowRandomCreate: true, 5, isSpecial: false, 4, 36, -1, -1, -1, 10, 664, 1860, new List<short> { 44, 143 }));
		_dataArray.Add(new SkillBookItem(809, 1618, 10, 1001, 0, 809, "icon_SkillBook_suishichu", 1619, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 50, 10, 150, 300, 0, 2, 600, 3, allowRandomCreate: true, 45, isSpecial: false, 4, 36, -1, -1, -1, 10, 665, 60, new List<short> { 117, 118, 119, 120, 121, 122, 123, 124, 125 }));
		_dataArray.Add(new SkillBookItem(810, 1620, 10, 1001, 1, 809, "icon_SkillBook_suishichu", 1621, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 45, 10, 300, 600, 0, 4, 1200, 4, allowRandomCreate: true, 40, isSpecial: false, 4, 36, -1, -1, -1, 10, 666, 130, new List<short> { 118, 119, 120, 121, 122, 123, 124, 125 }));
		_dataArray.Add(new SkillBookItem(811, 1622, 10, 1001, 2, 809, "icon_SkillBook_suishichu", 1623, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 40, 10, 900, 1800, 1, 6, 1800, 5, allowRandomCreate: true, 35, isSpecial: false, 4, 36, -1, -1, -1, 10, 667, 240, new List<short> { 119, 120, 121, 122, 123, 124, 125 }));
		_dataArray.Add(new SkillBookItem(812, 1624, 10, 1001, 3, 809, "icon_SkillBook_numujingangchu", 1625, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 35, 10, 2250, 4500, 2, 8, 3000, 6, allowRandomCreate: true, 30, isSpecial: false, 4, 36, -1, -1, -1, 10, 668, 400, new List<short> { 120, 121, 122, 123, 124, 125 }));
		_dataArray.Add(new SkillBookItem(813, 1626, 10, 1001, 4, 809, "icon_SkillBook_numujingangchu", 1627, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 30, 10, 4650, 9300, 3, 10, 4200, 7, allowRandomCreate: true, 25, isSpecial: false, 4, 36, -1, -1, -1, 10, 669, 600, new List<short> { 121, 122, 123, 124, 125 }));
		_dataArray.Add(new SkillBookItem(814, 1628, 10, 1001, 5, 809, "icon_SkillBook_numujingangchu", 1629, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 25, 10, 8400, 16800, 4, 12, 5400, 7, allowRandomCreate: true, 20, isSpecial: false, 4, 36, -1, -1, -1, 10, 670, 850, new List<short> { 122, 123, 124, 125 }));
		_dataArray.Add(new SkillBookItem(815, 1630, 10, 1001, 6, 809, "icon_SkillBook_ruyibaoshuchu", 1631, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 20, 10, 13800, 27600, 5, 14, 7200, 8, allowRandomCreate: true, 15, isSpecial: false, 4, 36, -1, -1, -1, 10, 671, 1140, new List<short> { 123, 124, 125 }));
		_dataArray.Add(new SkillBookItem(816, 1632, 10, 1001, 7, 809, "icon_SkillBook_budongmingwangchu", 1633, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 15, 10, 21150, 42300, 6, 16, 9000, 8, allowRandomCreate: true, 10, isSpecial: false, 4, 36, -1, -1, -1, 10, 672, 1480, new List<short> { 124, 125 }));
		_dataArray.Add(new SkillBookItem(817, 1634, 10, 1001, 0, 817, "icon_SkillBook_fuchengong", 1635, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 50, 10, 150, 300, 0, 2, 600, 3, allowRandomCreate: true, 45, isSpecial: false, 4, 36, -1, -1, -1, 11, 673, 60, new List<short>
		{
			108, 109, 110, 111, 112, 113, 114, 115, 116, 18,
			19, 20, 21, 22, 23, 24, 25, 26
		}));
		_dataArray.Add(new SkillBookItem(818, 1636, 10, 1001, 1, 817, "icon_SkillBook_fuchengong", 1637, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 45, 10, 300, 600, 0, 4, 1200, 4, allowRandomCreate: true, 40, isSpecial: false, 4, 36, -1, -1, -1, 11, 674, 130, new List<short>
		{
			109, 110, 111, 112, 113, 114, 115, 116, 19, 20,
			21, 22, 23, 24, 25, 26
		}));
		_dataArray.Add(new SkillBookItem(819, 1638, 10, 1001, 2, 817, "icon_SkillBook_fuchengong", 1639, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 40, 10, 900, 1800, 1, 6, 1800, 5, allowRandomCreate: true, 35, isSpecial: false, 4, 36, -1, -1, -1, 11, 675, 240, new List<short>
		{
			110, 111, 112, 113, 114, 115, 116, 20, 21, 22,
			23, 24, 25, 26
		}));
		_dataArray.Add(new SkillBookItem(820, 1640, 10, 1001, 3, 817, "icon_SkillBook_laojunfuchengong", 1641, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 35, 10, 2250, 4500, 2, 8, 3000, 6, allowRandomCreate: true, 30, isSpecial: false, 4, 36, -1, -1, -1, 11, 676, 400, new List<short>
		{
			111, 112, 113, 114, 115, 116, 21, 22, 23, 24,
			25, 26
		}));
		_dataArray.Add(new SkillBookItem(821, 1642, 10, 1001, 4, 817, "icon_SkillBook_laojunfuchengong", 1643, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 30, 10, 4650, 9300, 3, 10, 4200, 7, allowRandomCreate: true, 25, isSpecial: false, 4, 36, -1, -1, -1, 11, 677, 600, new List<short> { 112, 113, 114, 115, 116, 22, 23, 24, 25, 26 }));
		_dataArray.Add(new SkillBookItem(822, 1644, 10, 1001, 5, 817, "icon_SkillBook_laojunfuchengong", 1645, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 25, 10, 8400, 16800, 4, 12, 5400, 7, allowRandomCreate: true, 20, isSpecial: false, 4, 36, -1, -1, -1, 11, 678, 850, new List<short> { 113, 114, 115, 116, 23, 24, 25, 26 }));
		_dataArray.Add(new SkillBookItem(823, 1646, 10, 1001, 6, 817, "icon_SkillBook_wuliangsaochengong", 1647, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 20, 10, 13800, 27600, 5, 14, 7200, 8, allowRandomCreate: true, 15, isSpecial: false, 4, 36, -1, -1, -1, 11, 679, 1140, new List<short> { 114, 115, 116, 24, 25, 26 }));
		_dataArray.Add(new SkillBookItem(824, 1648, 10, 1001, 7, 817, "icon_SkillBook_xuankongshenfu", 1649, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 15, 10, 21150, 42300, 6, 16, 9000, 8, allowRandomCreate: true, 10, isSpecial: false, 4, 36, -1, -1, -1, 11, 680, 1480, new List<short> { 115, 116, 25, 26 }));
		_dataArray.Add(new SkillBookItem(825, 1650, 10, 1001, 0, 825, "icon_SkillBook_huanglinbianfa", 1651, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 50, 10, 150, 300, 0, 2, 600, 3, allowRandomCreate: true, 45, isSpecial: false, 4, 36, -1, -1, -1, 11, 681, 60, new List<short> { 81, 82, 83, 84, 85, 86, 87, 88, 89 }));
		_dataArray.Add(new SkillBookItem(826, 1652, 10, 1001, 1, 825, "icon_SkillBook_huanglinbianfa", 1653, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 45, 10, 300, 600, 0, 4, 1200, 4, allowRandomCreate: true, 40, isSpecial: false, 4, 36, -1, -1, -1, 11, 682, 130, new List<short> { 82, 83, 84, 85, 86, 87, 88, 89 }));
		_dataArray.Add(new SkillBookItem(827, 1654, 10, 1001, 2, 825, "icon_SkillBook_huanglinbianfa", 1655, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 40, 10, 900, 1800, 1, 6, 1800, 5, allowRandomCreate: true, 35, isSpecial: false, 4, 36, -1, -1, -1, 11, 683, 240, new List<short> { 83, 84, 85, 86, 87, 88, 89 }));
		_dataArray.Add(new SkillBookItem(828, 1656, 10, 1001, 3, 825, "icon_SkillBook_gouhunsuigubian", 1657, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 35, 10, 2250, 4500, 2, 8, 3000, 6, allowRandomCreate: true, 30, isSpecial: false, 4, 36, -1, -1, -1, 11, 684, 400, new List<short> { 84, 85, 86, 87, 88, 89 }));
		_dataArray.Add(new SkillBookItem(829, 1658, 10, 1001, 4, 825, "icon_SkillBook_gouhunsuigubian", 1659, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 30, 10, 4650, 9300, 3, 10, 4200, 7, allowRandomCreate: true, 25, isSpecial: false, 4, 36, -1, -1, -1, 11, 685, 600, new List<short> { 85, 86, 87, 88, 89 }));
		_dataArray.Add(new SkillBookItem(830, 1660, 10, 1001, 5, 825, "icon_SkillBook_gouhunsuigubian", 1661, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 25, 10, 8400, 16800, 4, 12, 5400, 7, allowRandomCreate: true, 20, isSpecial: false, 4, 36, -1, -1, -1, 11, 686, 850, new List<short> { 86, 87, 88, 89 }));
		_dataArray.Add(new SkillBookItem(831, 1662, 10, 1001, 6, 825, "icon_SkillBook_xianzhuqinmanggong", 1663, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 20, 10, 13800, 27600, 5, 14, 7200, 8, allowRandomCreate: true, 15, isSpecial: false, 4, 36, -1, -1, -1, 11, 687, 1140, new List<short> { 87, 88, 89 }));
		_dataArray.Add(new SkillBookItem(832, 1664, 10, 1001, 7, 825, "icon_SkillBook_balongshenbian", 1665, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 15, 10, 21150, 42300, 6, 16, 9000, 8, allowRandomCreate: true, 10, isSpecial: false, 4, 36, -1, -1, -1, 11, 688, 1480, new List<short> { 88, 89 }));
		_dataArray.Add(new SkillBookItem(833, 1666, 10, 1001, 8, 825, "icon_SkillBook_tianshesuo", 1667, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 10, 10, 30750, 61500, 6, 18, 10800, 8, allowRandomCreate: true, 5, isSpecial: false, 4, 36, -1, -1, -1, 11, 689, 1860, new List<short> { 89 }));
		_dataArray.Add(new SkillBookItem(834, 1668, 10, 1001, 0, 834, "icon_SkillBook_yuzhenshu", 1669, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 50, 10, 150, 300, 0, 2, 600, 3, allowRandomCreate: true, 45, isSpecial: false, 4, 36, -1, -1, -1, 12, 690, 60, new List<short> { 72, 73, 74, 75, 76, 77, 78, 79, 80 }));
		_dataArray.Add(new SkillBookItem(835, 1670, 10, 1001, 1, 834, "icon_SkillBook_yuzhenshu", 1671, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 45, 10, 300, 600, 0, 4, 1200, 4, allowRandomCreate: true, 40, isSpecial: false, 4, 36, -1, -1, -1, 12, 691, 130, new List<short> { 73, 74, 75, 76, 77, 78, 79, 80 }));
		_dataArray.Add(new SkillBookItem(836, 1672, 10, 1001, 2, 834, "icon_SkillBook_yuzhenshu", 1673, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 40, 10, 900, 1800, 1, 6, 1800, 5, allowRandomCreate: true, 35, isSpecial: false, 4, 36, -1, -1, -1, 12, 692, 240, new List<short> { 74, 75, 76, 77, 78, 79, 80 }));
		_dataArray.Add(new SkillBookItem(837, 1674, 10, 1001, 3, 834, "icon_SkillBook_bianqueshenzhen", 1675, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 35, 10, 2250, 4500, 2, 8, 3000, 6, allowRandomCreate: true, 30, isSpecial: false, 4, 36, -1, -1, -1, 12, 693, 400, new List<short> { 75, 76, 77, 78, 79, 80 }));
		_dataArray.Add(new SkillBookItem(838, 1676, 10, 1001, 4, 834, "icon_SkillBook_bianqueshenzhen", 1677, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 30, 10, 4650, 9300, 3, 10, 4200, 7, allowRandomCreate: true, 25, isSpecial: false, 4, 36, -1, -1, -1, 12, 694, 600, new List<short> { 76, 77, 78, 79, 80 }));
		_dataArray.Add(new SkillBookItem(839, 1678, 10, 1001, 5, 834, "icon_SkillBook_bianqueshenzhen", 1679, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 25, 10, 8400, 16800, 4, 12, 5400, 7, allowRandomCreate: true, 20, isSpecial: false, 4, 36, -1, -1, -1, 12, 695, 850, new List<short> { 77, 78, 79, 80 }));
	}

	private void CreateItems14()
	{
		_dataArray.Add(new SkillBookItem(840, 1680, 10, 1001, 6, 834, "icon_SkillBook_liuxianjiuzhenshu", 1681, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 20, 10, 13800, 27600, 5, 14, 7200, 8, allowRandomCreate: true, 15, isSpecial: false, 4, 36, -1, -1, -1, 12, 696, 1140, new List<short> { 78, 79, 80 }));
		_dataArray.Add(new SkillBookItem(841, 1682, 10, 1001, 7, 834, "icon_SkillBook_huamaishenzhen", 1683, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 15, 10, 21150, 42300, 6, 16, 9000, 8, allowRandomCreate: true, 10, isSpecial: false, 4, 36, -1, -1, -1, 12, 697, 1480, new List<short> { 79, 80 }));
		_dataArray.Add(new SkillBookItem(842, 1684, 10, 1001, 8, 834, "icon_SkillBook_shierxianqizhengong", 1685, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 10, 10, 30750, 61500, 6, 18, 10800, 8, allowRandomCreate: true, 5, isSpecial: false, 4, 36, -1, -1, -1, 12, 698, 1860, new List<short> { 80 }));
		_dataArray.Add(new SkillBookItem(843, 1686, 10, 1001, 0, 843, "icon_SkillBook_xiulifeiyan", 1687, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 50, 10, 150, 300, 0, 2, 600, 3, allowRandomCreate: true, 45, isSpecial: false, 4, 36, -1, -1, -1, 12, 699, 60, new List<short>
		{
			54, 55, 56, 57, 58, 59, 60, 61, 62, 63,
			64, 65, 66, 67, 68, 69, 70, 71, 90, 91,
			92, 93, 94, 95, 96, 97, 98, 99, 100, 101,
			102, 103, 104, 105, 106, 107
		}));
		_dataArray.Add(new SkillBookItem(844, 1688, 10, 1001, 1, 843, "icon_SkillBook_xiulifeiyan", 1689, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 45, 10, 300, 600, 0, 4, 1200, 4, allowRandomCreate: true, 40, isSpecial: false, 4, 36, -1, -1, -1, 12, 700, 130, new List<short>
		{
			55, 56, 57, 58, 59, 60, 61, 62, 64, 65,
			66, 67, 68, 69, 70, 71, 91, 92, 93, 94,
			95, 96, 97, 98, 100, 101, 102, 103, 104, 105,
			106, 107
		}));
		_dataArray.Add(new SkillBookItem(845, 1690, 10, 1001, 2, 843, "icon_SkillBook_xiulifeiyan", 1691, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 40, 10, 900, 1800, 1, 6, 1800, 5, allowRandomCreate: true, 35, isSpecial: false, 4, 36, -1, -1, -1, 12, 701, 240, new List<short>
		{
			56, 57, 58, 59, 60, 61, 62, 65, 66, 67,
			68, 69, 70, 71, 92, 93, 94, 95, 96, 97,
			98, 101, 102, 103, 104, 105, 106, 107
		}));
		_dataArray.Add(new SkillBookItem(846, 1692, 10, 1001, 3, 843, "icon_SkillBook_qixinglongzhaosuo", 1693, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 35, 10, 2250, 4500, 2, 8, 3000, 6, allowRandomCreate: true, 30, isSpecial: false, 4, 36, -1, -1, -1, 12, 702, 400, new List<short>
		{
			57, 58, 59, 60, 61, 62, 66, 67, 68, 69,
			70, 71, 93, 94, 95, 96, 97, 98, 102, 103,
			104, 105, 106, 107
		}));
		_dataArray.Add(new SkillBookItem(847, 1694, 10, 1001, 4, 843, "icon_SkillBook_qixinglongzhaosuo", 1695, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 30, 10, 4650, 9300, 3, 10, 4200, 7, allowRandomCreate: true, 25, isSpecial: false, 4, 36, -1, -1, -1, 12, 703, 600, new List<short>
		{
			58, 59, 60, 61, 62, 67, 68, 69, 70, 71,
			94, 95, 96, 97, 98, 103, 104, 105, 106, 107
		}));
		_dataArray.Add(new SkillBookItem(848, 1696, 10, 1001, 5, 843, "icon_SkillBook_qixinglongzhaosuo", 1697, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 25, 10, 8400, 16800, 4, 12, 5400, 7, allowRandomCreate: true, 20, isSpecial: false, 4, 36, -1, -1, -1, 12, 704, 850, new List<short>
		{
			59, 60, 61, 62, 68, 69, 70, 71, 95, 96,
			97, 98, 104, 105, 106, 107
		}));
		_dataArray.Add(new SkillBookItem(849, 1698, 10, 1001, 6, 843, "icon_SkillBook_huanglongmuyuan", 1699, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 20, 10, 13800, 27600, 5, 14, 7200, 8, allowRandomCreate: true, 15, isSpecial: false, 4, 36, -1, -1, -1, 12, 705, 1140, new List<short>
		{
			60, 61, 62, 69, 70, 71, 96, 97, 98, 105,
			106, 107
		}));
		_dataArray.Add(new SkillBookItem(850, 1700, 10, 1001, 7, 843, "icon_SkillBook_tianshuxuanji", 1701, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 15, 10, 21150, 42300, 6, 16, 9000, 8, allowRandomCreate: true, 10, isSpecial: false, 4, 36, -1, -1, -1, 12, 706, 1480, new List<short> { 61, 62, 70, 71, 97, 98, 106, 107 }));
		_dataArray.Add(new SkillBookItem(851, 1702, 10, 1001, 0, 851, "icon_SkillBook_yunshuiyin", 1703, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 50, 10, 150, 300, 0, 2, 600, 3, allowRandomCreate: true, 45, isSpecial: false, 4, 36, -1, -1, -1, 13, 707, 60, new List<short> { 0, 1, 2, 3, 4, 5, 6, 7, 8 }));
		_dataArray.Add(new SkillBookItem(852, 1704, 10, 1001, 1, 851, "icon_SkillBook_yunshuiyin", 1705, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 45, 10, 300, 600, 0, 4, 1200, 4, allowRandomCreate: true, 40, isSpecial: false, 4, 36, -1, -1, -1, 13, 708, 130, new List<short> { 1, 2, 3, 4, 5, 6, 7, 8 }));
		_dataArray.Add(new SkillBookItem(853, 1706, 10, 1001, 2, 851, "icon_SkillBook_yunshuiyin", 1707, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 40, 10, 900, 1800, 1, 6, 1800, 5, allowRandomCreate: true, 35, isSpecial: false, 4, 36, -1, -1, -1, 13, 709, 240, new List<short> { 2, 3, 4, 5, 6, 7, 8 }));
		_dataArray.Add(new SkillBookItem(854, 1708, 10, 1001, 3, 851, "icon_SkillBook_yufeiyin", 1709, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 35, 10, 2250, 4500, 2, 8, 3000, 6, allowRandomCreate: true, 30, isSpecial: false, 4, 36, -1, -1, -1, 13, 710, 400, new List<short> { 3, 4, 5, 6, 7, 8 }));
		_dataArray.Add(new SkillBookItem(855, 1710, 10, 1001, 4, 851, "icon_SkillBook_yufeiyin", 1711, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 30, 10, 4650, 9300, 3, 10, 4200, 7, allowRandomCreate: true, 25, isSpecial: false, 4, 36, -1, -1, -1, 13, 711, 600, new List<short> { 4, 5, 6, 7, 8 }));
		_dataArray.Add(new SkillBookItem(856, 1712, 10, 1001, 5, 851, "icon_SkillBook_yufeiyin", 1713, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 25, 10, 8400, 16800, 4, 12, 5400, 7, allowRandomCreate: true, 20, isSpecial: false, 4, 36, -1, -1, -1, 13, 712, 850, new List<short> { 5, 6, 7, 8 }));
		_dataArray.Add(new SkillBookItem(857, 1714, 10, 1001, 6, 851, "icon_SkillBook_tiandixiao", 1715, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 20, 10, 13800, 27600, 5, 14, 7200, 8, allowRandomCreate: true, 15, isSpecial: false, 4, 36, -1, -1, -1, 13, 713, 1140, new List<short> { 6, 7, 8 }));
		_dataArray.Add(new SkillBookItem(858, 1716, 10, 1001, 0, 858, "icon_SkillBook_qiqingqu", 1717, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 50, 10, 150, 300, 0, 2, 600, 3, allowRandomCreate: true, 45, isSpecial: false, 4, 36, -1, -1, -1, 13, 714, 60, new List<short> { 0, 1, 2, 3, 4, 5, 6, 7, 8 }));
		_dataArray.Add(new SkillBookItem(859, 1718, 10, 1001, 1, 858, "icon_SkillBook_qiqingqu", 1719, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 45, 10, 300, 600, 0, 4, 1200, 4, allowRandomCreate: true, 40, isSpecial: false, 4, 36, -1, -1, -1, 13, 715, 130, new List<short> { 1, 2, 3, 4, 5, 6, 7, 8 }));
		_dataArray.Add(new SkillBookItem(860, 1720, 10, 1001, 2, 858, "icon_SkillBook_qiqingqu", 1721, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 40, 10, 900, 1800, 1, 6, 1800, 5, allowRandomCreate: true, 35, isSpecial: false, 4, 36, -1, -1, -1, 13, 716, 240, new List<short> { 2, 3, 4, 5, 6, 7, 8 }));
		_dataArray.Add(new SkillBookItem(861, 1722, 10, 1001, 3, 858, "icon_SkillBook_huangzhuge", 1723, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 35, 10, 2250, 4500, 2, 8, 3000, 6, allowRandomCreate: true, 30, isSpecial: false, 4, 36, -1, -1, -1, 13, 717, 400, new List<short> { 3, 4, 5, 6, 7, 8 }));
		_dataArray.Add(new SkillBookItem(862, 1724, 10, 1001, 4, 858, "icon_SkillBook_huangzhuge", 1725, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 30, 10, 4650, 9300, 3, 10, 4200, 7, allowRandomCreate: true, 25, isSpecial: false, 4, 36, -1, -1, -1, 13, 718, 600, new List<short> { 4, 5, 6, 7, 8 }));
		_dataArray.Add(new SkillBookItem(863, 1726, 10, 1001, 5, 858, "icon_SkillBook_huangzhuge", 1727, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 25, 10, 8400, 16800, 4, 12, 5400, 7, allowRandomCreate: true, 20, isSpecial: false, 4, 36, -1, -1, -1, 13, 719, 850, new List<short> { 5, 6, 7, 8 }));
		_dataArray.Add(new SkillBookItem(864, 1728, 10, 1001, 6, 858, "icon_SkillBook_guanghange", 1729, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 20, 10, 13800, 27600, 5, 14, 7200, 8, allowRandomCreate: true, 15, isSpecial: false, 4, 36, -1, -1, -1, 13, 720, 1140, new List<short> { 6, 7, 8 }));
		_dataArray.Add(new SkillBookItem(865, 1730, 10, 1001, 7, 858, "icon_SkillBook_hourenxiyi", 1731, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 15, 10, 21150, 42300, 6, 16, 9000, 8, allowRandomCreate: true, 10, isSpecial: false, 4, 36, -1, -1, -1, 13, 721, 1480, new List<short> { 7, 8 }));
		_dataArray.Add(new SkillBookItem(866, 1732, 10, 1001, 8, 858, "icon_SkillBook_sunvtianyin", 1733, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 10, 10, 30750, 61500, 6, 18, 10800, 8, allowRandomCreate: true, 5, isSpecial: false, 4, 36, -1, -1, -1, 13, 722, 1860, new List<short> { 8 }));
		_dataArray.Add(new SkillBookItem(867, 1734, 10, 1001, 0, -1, "icon_SkillBook_taizuchangquan", 1735, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 50, 10, 150, 300, 0, 1, 600, 3, allowRandomCreate: true, 0, isSpecial: true, 4, 36, -1, -1, -1, 3, 723, 60, new List<short>()));
		_dataArray.Add(new SkillBookItem(868, 1736, 10, 1001, 0, -1, "icon_SkillBook_peiranjue", 1737, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 50, 10, 150, 300, 0, 1, 600, 3, allowRandomCreate: true, 0, isSpecial: true, 4, 36, -1, -1, -1, 0, 724, 60, new List<short>()));
		_dataArray.Add(new SkillBookItem(869, 1738, 10, 1001, 0, -1, "icon_SkillBook_xiaozongyuegong", 1739, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 50, 10, 150, 300, 0, 1, 600, 3, allowRandomCreate: true, 0, isSpecial: true, 4, 36, -1, -1, -1, 1, 725, 60, new List<short>()));
		_dataArray.Add(new SkillBookItem(870, 1700, 10, 1001, 7, -1, "icon_SkillBook_yifudetianshuxuanji", 1740, transferable: false, stackable: false, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: true, -1, 0, 0, 0, 0, 0, 0, 0, allowRandomCreate: true, 0, isSpecial: true, 4, 36, -1, -1, -1, 12, 706, 1600, new List<short>()));
		_dataArray.Add(new SkillBookItem(871, 1741, 10, 1001, 4, 179, "icon_SkillBook_guangmingshizijin", 1742, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 30, 10, 4650, 9300, 3, 10, 4200, 7, allowRandomCreate: true, 25, isSpecial: false, 4, 36, -1, -1, -1, 0, 916, 600, new List<short>()));
		_dataArray.Add(new SkillBookItem(872, 1743, 10, 1001, 8, 843, "icon_SkillBook_tianshuxuanji", 1744, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 10, 10, 30750, 61500, 6, 18, 10800, 8, allowRandomCreate: true, 5, isSpecial: false, 4, 36, -1, -1, -1, 12, 917, 1860, new List<short> { 62, 71, 98, 107 }));
		_dataArray.Add(new SkillBookItem(873, 1745, 10, 1001, 5, 200, "icon_SkillBook_qianchuibailianpian", 1746, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 25, 10, 8400, 16800, 4, 12, 5400, 7, allowRandomCreate: true, 20, isSpecial: false, 4, 36, -1, -1, -1, 0, 918, 850, new List<short>
		{
			59, 60, 61, 62, 68, 69, 70, 71, 95, 96,
			97, 98, 104, 105, 106, 107
		}));
		_dataArray.Add(new SkillBookItem(874, 1747, 10, 1001, 0, -1, "icon_SkillBook_shuihuoyingqigong", 1748, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 50, 10, 150, 300, 0, 1, 600, 3, allowRandomCreate: true, 0, isSpecial: true, 4, 36, -1, -1, -1, 2, 919, 60, new List<short>()));
		_dataArray.Add(new SkillBookItem(875, 1749, 10, 1001, 8, 166, "icon_SkillBook_yuanshixiantiangong", 1750, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 10, 10, 30750, 61500, 6, 18, 10800, 8, allowRandomCreate: true, 5, isSpecial: false, 4, 36, -1, -1, -1, 0, 920, 1860, new List<short> { 116 }));
		_dataArray.Add(new SkillBookItem(876, 1751, 10, 1001, 8, 550, "icon_SkillBook_tianyishenshou", 1752, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 10, 10, 30750, 61500, 6, 18, 10800, 8, allowRandomCreate: true, 5, isSpecial: false, 4, 36, -1, -1, -1, 4, 921, 1860, new List<short> { 116, 124, 125 }));
		_dataArray.Add(new SkillBookItem(877, 1753, 10, 1001, 8, 183, "icon_SkillBook_tiansuibaolu", 1754, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 10, 10, 30750, 61500, 6, 18, 10800, 8, allowRandomCreate: true, 5, isSpecial: false, 4, 36, -1, -1, -1, 0, 922, 1860, new List<short> { 44, 142, 143 }));
		_dataArray.Add(new SkillBookItem(878, 1755, 10, 1001, 8, 776, "icon_SkillBook_baimobaqiang", 1756, transferable: true, stackable: false, wagerable: true, refinable: false, poisonable: true, repairable: false, inheritable: true, 10, 10, 30750, 61500, 6, 18, 10800, 8, allowRandomCreate: true, 5, isSpecial: false, 4, 36, -1, -1, -1, 9, 923, 1860, new List<short>()));
	}

	public override void Init()
	{
		base.Init();
		_dataArray = new List<SkillBookItem>(879);
		CreateItems0();
		CreateItems1();
		CreateItems2();
		CreateItems3();
		CreateItems4();
		CreateItems5();
		CreateItems6();
		CreateItems7();
		CreateItems8();
		CreateItems9();
		CreateItems10();
		CreateItems11();
		CreateItems12();
		CreateItems13();
		CreateItems14();
	}
}
