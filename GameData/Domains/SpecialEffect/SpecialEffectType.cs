using System;
using System.Runtime.CompilerServices;
using GameData.Domains.SpecialEffect.CombatSkill.Baihuagu.Agile;
using GameData.Domains.SpecialEffect.CombatSkill.Baihuagu.DefenseAndAssist;
using GameData.Domains.SpecialEffect.CombatSkill.Baihuagu.Finger;
using GameData.Domains.SpecialEffect.CombatSkill.Baihuagu.Music;
using GameData.Domains.SpecialEffect.CombatSkill.Baihuagu.Neigong;
using GameData.Domains.SpecialEffect.CombatSkill.Baihuagu.Shot;
using GameData.Domains.SpecialEffect.CombatSkill.Emeipai.Agile;
using GameData.Domains.SpecialEffect.CombatSkill.Emeipai.DefenseAndAssist;
using GameData.Domains.SpecialEffect.CombatSkill.Emeipai.Finger;
using GameData.Domains.SpecialEffect.CombatSkill.Emeipai.FistAndPalm;
using GameData.Domains.SpecialEffect.CombatSkill.Emeipai.Neigong;
using GameData.Domains.SpecialEffect.CombatSkill.Emeipai.Special;
using GameData.Domains.SpecialEffect.CombatSkill.Emeipai.Sword;
using GameData.Domains.SpecialEffect.CombatSkill.Fulongtan.Agile;
using GameData.Domains.SpecialEffect.CombatSkill.Fulongtan.Blade;
using GameData.Domains.SpecialEffect.CombatSkill.Fulongtan.DefenseAndAssist;
using GameData.Domains.SpecialEffect.CombatSkill.Fulongtan.FistAndPalm;
using GameData.Domains.SpecialEffect.CombatSkill.Fulongtan.Neigong;
using GameData.Domains.SpecialEffect.CombatSkill.Fulongtan.Throw;
using GameData.Domains.SpecialEffect.CombatSkill.Jieqingmen.Agile;
using GameData.Domains.SpecialEffect.CombatSkill.Jieqingmen.DefenseAndAssist;
using GameData.Domains.SpecialEffect.CombatSkill.Jieqingmen.Finger;
using GameData.Domains.SpecialEffect.CombatSkill.Jieqingmen.Neigong;
using GameData.Domains.SpecialEffect.CombatSkill.Jieqingmen.Sword;
using GameData.Domains.SpecialEffect.CombatSkill.Jieqingmen.Throw;
using GameData.Domains.SpecialEffect.CombatSkill.Jingangzong.Agile;
using GameData.Domains.SpecialEffect.CombatSkill.Jingangzong.Blade;
using GameData.Domains.SpecialEffect.CombatSkill.Jingangzong.DefenseAndAssist;
using GameData.Domains.SpecialEffect.CombatSkill.Jingangzong.FistAndPalm;
using GameData.Domains.SpecialEffect.CombatSkill.Jingangzong.Neigong;
using GameData.Domains.SpecialEffect.CombatSkill.Jingangzong.PestleEffect;
using GameData.Domains.SpecialEffect.CombatSkill.Jingangzong.Special;
using GameData.Domains.SpecialEffect.CombatSkill.Kongsangpai.Agile;
using GameData.Domains.SpecialEffect.CombatSkill.Kongsangpai.DefenseAndAssist;
using GameData.Domains.SpecialEffect.CombatSkill.Kongsangpai.Finger;
using GameData.Domains.SpecialEffect.CombatSkill.Kongsangpai.FistAndPalm;
using GameData.Domains.SpecialEffect.CombatSkill.Kongsangpai.Leg;
using GameData.Domains.SpecialEffect.CombatSkill.Kongsangpai.Neigong;
using GameData.Domains.SpecialEffect.CombatSkill.Kongsangpai.Throw;
using GameData.Domains.SpecialEffect.CombatSkill.NoSect.Agile;
using GameData.Domains.SpecialEffect.CombatSkill.NoSect.DefenseAndAssist;
using GameData.Domains.SpecialEffect.CombatSkill.NoSect.FistAndPalm;
using GameData.Domains.SpecialEffect.CombatSkill.NoSect.Neigong;
using GameData.Domains.SpecialEffect.CombatSkill.Ranshanpai.Agile;
using GameData.Domains.SpecialEffect.CombatSkill.Ranshanpai.DefenseAndAssist;
using GameData.Domains.SpecialEffect.CombatSkill.Ranshanpai.Finger;
using GameData.Domains.SpecialEffect.CombatSkill.Ranshanpai.Neigong;
using GameData.Domains.SpecialEffect.CombatSkill.Ranshanpai.Special;
using GameData.Domains.SpecialEffect.CombatSkill.Ranshanpai.Sword;
using GameData.Domains.SpecialEffect.CombatSkill.Shaolinpai.Agile;
using GameData.Domains.SpecialEffect.CombatSkill.Shaolinpai.DefenseAndAssist;
using GameData.Domains.SpecialEffect.CombatSkill.Shaolinpai.Finger;
using GameData.Domains.SpecialEffect.CombatSkill.Shaolinpai.FistAndPalm;
using GameData.Domains.SpecialEffect.CombatSkill.Shaolinpai.Neigong;
using GameData.Domains.SpecialEffect.CombatSkill.Shaolinpai.Polearm;
using GameData.Domains.SpecialEffect.CombatSkill.Shixiangmen.Agile;
using GameData.Domains.SpecialEffect.CombatSkill.Shixiangmen.Blade;
using GameData.Domains.SpecialEffect.CombatSkill.Shixiangmen.DefenseAndAssist;
using GameData.Domains.SpecialEffect.CombatSkill.Shixiangmen.FistAndPalm;
using GameData.Domains.SpecialEffect.CombatSkill.Shixiangmen.Neigong;
using GameData.Domains.SpecialEffect.CombatSkill.Shixiangmen.Polearm;
using GameData.Domains.SpecialEffect.CombatSkill.Wudangpai.Agile;
using GameData.Domains.SpecialEffect.CombatSkill.Wudangpai.DefenseAndAssist;
using GameData.Domains.SpecialEffect.CombatSkill.Wudangpai.FistAndPalm;
using GameData.Domains.SpecialEffect.CombatSkill.Wudangpai.Neigong;
using GameData.Domains.SpecialEffect.CombatSkill.Wudangpai.Sword;
using GameData.Domains.SpecialEffect.CombatSkill.Wudangpai.Whip;
using GameData.Domains.SpecialEffect.CombatSkill.Wuxianjiao.Agile;
using GameData.Domains.SpecialEffect.CombatSkill.Wuxianjiao.DefenseAndAssist;
using GameData.Domains.SpecialEffect.CombatSkill.Wuxianjiao.Finger;
using GameData.Domains.SpecialEffect.CombatSkill.Wuxianjiao.FistAndPalm;
using GameData.Domains.SpecialEffect.CombatSkill.Wuxianjiao.Neigong;
using GameData.Domains.SpecialEffect.CombatSkill.Wuxianjiao.Sword;
using GameData.Domains.SpecialEffect.CombatSkill.Wuxianjiao.Whip;
using GameData.Domains.SpecialEffect.CombatSkill.Wuxianjiao.WugEffect;
using GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Agile;
using GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Assist;
using GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Attack.DaYueYaoChang;
using GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Attack.HuanXin;
using GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Attack.JinHuangEr;
using GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Attack.JiuHan;
using GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Attack.LongYuFu;
using GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Attack.MoNv;
using GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Attack.RanChenZi;
using GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Attack.ShuFang;
using GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Attack.WeiQi;
using GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Attack.XiangShu;
using GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Attack.XueFeng;
using GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Attack.YiXiang;
using GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Attack.YiYiHou;
using GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Attack.ZiWuXiao;
using GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Defense;
using GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Neigong.Boss;
using GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Neigong.RandomEnemy;
using GameData.Domains.SpecialEffect.CombatSkill.Xuannvpai.Agile;
using GameData.Domains.SpecialEffect.CombatSkill.Xuannvpai.DefenseAndAssist;
using GameData.Domains.SpecialEffect.CombatSkill.Xuannvpai.Finger;
using GameData.Domains.SpecialEffect.CombatSkill.Xuannvpai.FistAndPalm;
using GameData.Domains.SpecialEffect.CombatSkill.Xuannvpai.Music;
using GameData.Domains.SpecialEffect.CombatSkill.Xuannvpai.Neigong;
using GameData.Domains.SpecialEffect.CombatSkill.Xuehoujiao.Agile;
using GameData.Domains.SpecialEffect.CombatSkill.Xuehoujiao.BreakBodyEffect;
using GameData.Domains.SpecialEffect.CombatSkill.Xuehoujiao.DefenseAndAssist;
using GameData.Domains.SpecialEffect.CombatSkill.Xuehoujiao.Finger;
using GameData.Domains.SpecialEffect.CombatSkill.Xuehoujiao.FistAndPalm;
using GameData.Domains.SpecialEffect.CombatSkill.Xuehoujiao.Leg;
using GameData.Domains.SpecialEffect.CombatSkill.Xuehoujiao.Neigong;
using GameData.Domains.SpecialEffect.CombatSkill.Xuehoujiao.Throw;
using GameData.Domains.SpecialEffect.CombatSkill.Yuanshanpai.Agile;
using GameData.Domains.SpecialEffect.CombatSkill.Yuanshanpai.Blade;
using GameData.Domains.SpecialEffect.CombatSkill.Yuanshanpai.DefenseAndAssist;
using GameData.Domains.SpecialEffect.CombatSkill.Yuanshanpai.Leg;
using GameData.Domains.SpecialEffect.CombatSkill.Yuanshanpai.Neigong;
using GameData.Domains.SpecialEffect.CombatSkill.Yuanshanpai.Sword;
using GameData.Domains.SpecialEffect.CombatSkill.Zhujianshanzhuang.Agile;
using GameData.Domains.SpecialEffect.CombatSkill.Zhujianshanzhuang.Blade;
using GameData.Domains.SpecialEffect.CombatSkill.Zhujianshanzhuang.DefenseAndAssist;
using GameData.Domains.SpecialEffect.CombatSkill.Zhujianshanzhuang.Neigong;
using GameData.Domains.SpecialEffect.CombatSkill.Zhujianshanzhuang.Polearm;
using GameData.Domains.SpecialEffect.CombatSkill.Zhujianshanzhuang.Shot;
using GameData.Domains.SpecialEffect.CombatSkill.Zhujianshanzhuang.Sword;
using GameData.Domains.SpecialEffect.EquipmentEffect.Armor;
using GameData.Domains.SpecialEffect.EquipmentEffect.Armor.AddBounceDamage;
using GameData.Domains.SpecialEffect.EquipmentEffect.Armor.BreakReduceInjury;
using GameData.Domains.SpecialEffect.EquipmentEffect.Armor.ReduceHit;
using GameData.Domains.SpecialEffect.EquipmentEffect.Armor.ReducePenetrate;
using GameData.Domains.SpecialEffect.EquipmentEffect.RawCreate;
using GameData.Domains.SpecialEffect.EquipmentEffect.Weapon;
using GameData.Domains.SpecialEffect.EquipmentEffect.Weapon.BreakAddInjury;
using GameData.Domains.SpecialEffect.EquipmentEffect.Weapon.ReduceAvoid;
using GameData.Domains.SpecialEffect.EquipmentEffect.Weapon.ReduceBounceDamage;
using GameData.Domains.SpecialEffect.EquipmentEffect.Weapon.ReducePenetrateResist;
using GameData.Domains.SpecialEffect.LegendaryBook.Blade;
using GameData.Domains.SpecialEffect.LegendaryBook.CombatMusic;
using GameData.Domains.SpecialEffect.LegendaryBook.ControllableShot;
using GameData.Domains.SpecialEffect.LegendaryBook.Finger;
using GameData.Domains.SpecialEffect.LegendaryBook.FistAndPalm;
using GameData.Domains.SpecialEffect.LegendaryBook.Leg;
using GameData.Domains.SpecialEffect.LegendaryBook.Neigong;
using GameData.Domains.SpecialEffect.LegendaryBook.NpcEffect;
using GameData.Domains.SpecialEffect.LegendaryBook.Polearm;
using GameData.Domains.SpecialEffect.LegendaryBook.Posing;
using GameData.Domains.SpecialEffect.LegendaryBook.Special;
using GameData.Domains.SpecialEffect.LegendaryBook.Stunt;
using GameData.Domains.SpecialEffect.LegendaryBook.Sword;
using GameData.Domains.SpecialEffect.LegendaryBook.Throw;
using GameData.Domains.SpecialEffect.LegendaryBook.Whip;
using GameData.Domains.SpecialEffect.Misc;

namespace GameData.Domains.SpecialEffect
{
	// Token: 0x020000E4 RID: 228
	public static class SpecialEffectType
	{
		// Token: 0x06002954 RID: 10580 RVA: 0x001FB71C File Offset: 0x001F991C
		public static SpecialEffectBase CreateEffectObj(int type)
		{
			bool flag = type < 0;
			if (!flag)
			{
				if (type <= 10205)
				{
					if (type <= 5307)
					{
						if (type <= 2606)
						{
							if (type <= 1405)
							{
								if (type <= 300)
								{
									if (type <= 100)
									{
										if (type == 0)
										{
											return new PeiRanJue();
										}
										if (type == 100)
										{
											return new TaiZuChangQuan();
										}
									}
									else
									{
										if (type == 200)
										{
											return new XiaoZongYueGong();
										}
										if (type == 300)
										{
											return new ShuiHuoYingQiGong();
										}
									}
								}
								else if (type <= 1107)
								{
									switch (type)
									{
									case 1000:
										return new JingChanGong();
									case 1001:
										return new LuoHanGong();
									case 1002:
										return new XinYiQiHunYuanGong();
									case 1003:
										return new WuSeChanGong();
									case 1004:
										return new PuTiXinXiuFa();
									case 1005:
										return new ZaAHanJing();
									case 1006:
										return new ALuoHanShenGong();
									case 1007:
										return new YiJinJing();
									case 1008:
										return new XiSuiJing();
									default:
										switch (type)
										{
										case 1100:
											return new ShaoLinChangQuan();
										case 1101:
											return new ShaoLinJinGangZhang();
										case 1102:
											return new DaTongBiQuan();
										case 1103:
											return new LiuShiBaShiLuoHanQuan();
										case 1104:
											return new DaJinGangQuan();
										case 1105:
											return new DaMoShiBaShou();
										case 1106:
											return new JinGangBoReZhang();
										case 1107:
											return new XiangMoGuangMingQuan();
										}
										break;
									}
								}
								else
								{
									switch (type)
									{
									case 1200:
										return new ShaoLinFuHuZhua();
									case 1201:
										return new ShaoLinYiZhiChan();
									case 1202:
										return new NianHuaZhiGong();
									case 1203:
										return new DaLiJinGangZhi();
									case 1204:
										return new NieZhuanJinGangQuan();
									case 1205:
										return new ShaoLinLongZhuaShou();
									default:
										switch (type)
										{
										case 1300:
											return new ShaoLinLiuHeGun();
										case 1301:
											return new ShaoLinYinYangGun();
										case 1302:
											return new ShiBaDianQiMeiGun();
										case 1303:
											return new DaXiaoYeChaGun();
										case 1304:
											return new WuHuQunYangGun();
										case 1305:
											return new ShaoLinFengMoGun();
										case 1306:
											return new WeiTuoXiangMoZhang();
										case 1307:
											return new DaZhiPuTiZhangFa();
										case 1308:
											return new DaMoZhangFa();
										default:
											switch (type)
											{
											case 1400:
												return new ChuanZongGong();
											case 1401:
												return new BiHuYouQiangGong();
											case 1402:
												return new QingShenShu();
											case 1403:
												return new DaFanTengShu();
											case 1404:
												return new JiuTuLiuZuoXiang();
											case 1405:
												return new YiWeiDuJiang();
											}
											break;
										}
										break;
									}
								}
							}
							else if (type <= 2207)
							{
								if (type <= 1606)
								{
									switch (type)
									{
									case 1501:
										return new ZhanYiShiBaDie();
									case 1502:
										return new QianJinZhui();
									case 1503:
									case 1505:
									case 1506:
										break;
									case 1504:
										return new JinZhongZhao();
									case 1507:
										return new HunYuanYiQiGong();
									case 1508:
										return new JinGangHuTiShenGong();
									default:
										switch (type)
										{
										case 1600:
											return new WoHuGong();
										case 1603:
											return new BianTiTongRenFa();
										case 1605:
											return new XiaoLuoHanGunZhen();
										case 1606:
											return new FoMenShiZiHou();
										}
										break;
									}
								}
								else
								{
									switch (type)
									{
									case 2000:
										return new DanTianKaiHeGong();
									case 2001:
										return new EMeiShiErZhuang();
									case 2002:
										return new XianTianYiYuanQiGong();
									case 2003:
										return new LingWuGong();
									case 2004:
										return new LianHuaTaiXuanGong();
									case 2005:
										return new PuXianXinJing();
									case 2006:
										return new ZuoWangXuanGong();
									case 2007:
										return new DaBoNiePanFa();
									default:
										switch (type)
										{
										case 2100:
											return new SanShiLiuBiShou();
										case 2101:
											return new YaXingQuan();
										case 2102:
											return new ShiErShouLanChanSiQuan();
										case 2103:
											return new BaiYuanTongBiQuan();
										case 2104:
											return new YiHuaJieMuShou();
										case 2105:
											return new TianGangLeiHongZhang();
										case 2106:
											return new JinGangSanMeiZhang();
										default:
											switch (type)
											{
											case 2200:
												return new EMeiYingZhuaGong();
											case 2201:
												return new EMeiYiZhiChan();
											case 2202:
												return new BoDaShiBaShi();
											case 2203:
												return new FenHuaFuLiuShi();
											case 2204:
												return new GuangXiangZhi();
											case 2205:
												return new TongShenQianShouFa();
											case 2206:
												return new DaGuangMingShanYiYuanZhi();
											case 2207:
												return new TianGangZhiXueFa();
											}
											break;
										}
										break;
									}
								}
							}
							else if (type <= 2407)
							{
								switch (type)
								{
								case 2300:
									return new FuHuaLveYingJian();
								case 2301:
									return new YuanGongJianFa();
								case 2302:
									return new BaiMeiJianFa();
								case 2303:
									return new LianHuaMiaoJian();
								case 2304:
									return new YueNvJianFa();
								case 2305:
									return new CanHongJianShi();
								case 2306:
									return new YuNvShenJian();
								default:
									switch (type)
									{
									case 2400:
										return new WuXingCi();
									case 2401:
										return new ShiHunCi();
									case 2402:
										return new MeiRenCi();
									case 2403:
										return new LingSheCi();
									case 2404:
										return new HuaYingCi();
									case 2405:
										return new DingHuiShenZhen();
									case 2406:
										return new YuNvCi();
									case 2407:
										return new JinDingXianZhen();
									}
									break;
								}
							}
							else
							{
								switch (type)
								{
								case 2500:
									return new TanTuiSuoDi();
								case 2501:
									return new BaiYuanFeiZongShu();
								case 2502:
									return new XingWuDingZong();
								case 2503:
									return new YuNvNuoLian();
								case 2504:
									return new XingNvZhiLingSuo();
								case 2505:
									return new QianTanYinYueBu();
								case 2506:
									return new JinDingFeiXian();
								default:
									if (type == 2603)
									{
										return new JinWuGu();
									}
									if (type == 2606)
									{
										return new YangXianGongJinShenFa();
									}
									break;
								}
							}
						}
						else if (type <= 4007)
						{
							if (type <= 3208)
							{
								if (type <= 2707)
								{
									if (type == 2608)
									{
										return new ShengDengQiWuJue();
									}
									switch (type)
									{
									case 2700:
										return new EMeiHuBuGong();
									case 2701:
										return new XinWuDingYi();
									case 2702:
										return new WuHuaBaMen();
									case 2704:
										return new QingJingGuiYi();
									case 2705:
										return new SiXiangLianHuaZhen();
									case 2707:
										return new QiXingHengLian();
									}
								}
								else
								{
									switch (type)
									{
									case 3000:
										return new TongRenShuXueTuJing();
									case 3001:
										return new JiuZhenShiErYuan();
									case 3002:
										return new JiaYiQuanPian();
									case 3003:
										return new NeiJingLingShuPian();
									case 3004:
										return new JinZhenFaMaiGong();
									default:
										switch (type)
										{
										case 3100:
											return new ZhiZhenGong();
										case 3101:
											return new ChangChunZhi();
										case 3102:
											return new YiZhiYiPinHong();
										case 3103:
											return new QingHuaYuMeiRen();
										case 3104:
											return new ManTianHuaYuShi();
										case 3105:
											return new DaHuaManTuoLuoZhi();
										case 3106:
											return new WanHuaLingYueZhi();
										case 3107:
											return new BaiHuaSha();
										case 3108:
											return new XueZhuHuaBaFa();
										default:
											switch (type)
											{
											case 3200:
												return new YuZhenShu();
											case 3201:
												return new WuXingMeiHuaZhen();
											case 3202:
												return new HanBingCiGuFa();
											case 3203:
												return new BianQueShenZhen();
											case 3204:
												return new PoYuanChangZhen();
											case 3205:
												return new ZhenSiBuZhenHuoFa();
											case 3206:
												return new LiuXianJiuZhenShu();
											case 3207:
												return new HuaMaiShenZhen();
											case 3208:
												return new ShiErXianQiZhenGong();
											}
											break;
										}
										break;
									}
								}
							}
							else if (type <= 3407)
							{
								switch (type)
								{
								case 3300:
									return new YunShuiYin();
								case 3301:
									return new BuSiGui();
								case 3302:
									return new FengLaiYi();
								case 3303:
									return new YuFeiYin();
								case 3304:
									return new QiongHuaTan();
								case 3305:
									return new ZangLuLan();
								case 3306:
									return new TianDiXiao();
								default:
									switch (type)
									{
									case 3400:
										return new TiZhenFa();
									case 3401:
										return new QianNiuHuanShenBu();
									case 3402:
										return new FeiZhenChuanSuoShu();
									case 3403:
										return new YuZhenXing();
									case 3404:
										return new TaXueXunMei();
									case 3405:
										return new JinZhanXuanSi();
									case 3406:
										return new ZhiLanYuBu();
									case 3407:
										return new WanHuaTingYuShi();
									}
									break;
								}
							}
							else
							{
								switch (type)
								{
								case 3504:
									return new YinYangMaiNiZhuan();
								case 3505:
									return new ShiErJingDaNuoYi();
								case 3506:
									break;
								case 3507:
									return new XueHaiNingBingShu();
								case 3508:
									return new WanBiBuPoFa();
								default:
									switch (type)
									{
									case 3600:
										return new SanShiLiuShouYiZhenFa();
									case 3601:
										return new JingYiBiXie();
									case 3602:
										return new BaiCaoQueZhuoZhi();
									case 3603:
										return new LuanZhenCuoXue();
									case 3604:
									case 3605:
										break;
									case 3606:
										return new ShengSiBaMen();
									default:
										switch (type)
										{
										case 4000:
											return new WuDangTaiHeGong();
										case 4001:
											return new BaoYuanShouYi();
										case 4002:
											return new YiZiHunYuanZhuang();
										case 4003:
											return new LuoTianZhenJueShiErZhuang();
										case 4004:
											return new XiSuiJinJing();
										case 4005:
											return new WuDangHunYuanGong();
										case 4006:
											return new TaiYiShenGong();
										case 4007:
											return new ChunYangWuJiGong();
										}
										break;
									}
									break;
								}
							}
						}
						else if (type <= 4506)
						{
							if (type <= 4208)
							{
								switch (type)
								{
								case 4100:
									return new WuDangChangQuan();
								case 4101:
									return new ZhuiFengYunHuanZhang();
								case 4102:
									return new WuDangMianZhang();
								case 4103:
									return new YouShenBaGuaZhang();
								case 4104:
									return new TaiYiXiaoYaoZhang();
								case 4105:
									return new XuanGongShenQuan();
								case 4106:
									return new WuDangChunYangQuan();
								case 4107:
									return new ZiYangZhengQiQuan();
								case 4108:
									return new TaiJiQuan();
								default:
									switch (type)
									{
									case 4200:
										return new WuDangDanJian();
									case 4201:
										return new WuDangXiaoYaoJian();
									case 4202:
										return new BaiHongJianFa();
									case 4203:
										return new TaiYiXuanMenJian();
									case 4204:
										return new ChunYangJianYi();
									case 4205:
										return new ZhenWuYouLongJian();
									case 4206:
										return new TaiJiJianFa();
									case 4207:
										return new XianTianShaoYangJianQi();
									case 4208:
										return new WuJiJianShi();
									}
									break;
								}
							}
							else
							{
								switch (type)
								{
								case 4300:
									return new FuChenGong();
								case 4301:
									return new WuDangTieFuChen();
								case 4302:
									return new LiangYiFuChenGong();
								case 4303:
									return new LaoJunFuChenGong();
								case 4304:
									return new CuoDaoYinYangFuChen();
								case 4305:
									return new TaiYiYunFuGong();
								case 4306:
									return new WuLiangSaoChenGong();
								case 4307:
									return new XuanKongShenFu();
								default:
									switch (type)
									{
									case 4400:
										return new WuBuMeiHuaZhuang();
									case 4401:
										return new BaGuaBu();
									case 4402:
										return new YunHuanBu();
									case 4403:
										return new TianGangBeiDouBu();
									case 4404:
										return new TaiYiJiuGongBu();
									case 4405:
										return new ShangTianTi();
									case 4406:
										return new LingYunFeiDu();
									default:
										switch (type)
										{
										case 4500:
											return new WuDangTieBuShan();
										case 4502:
											return new YunChuangJiuLian();
										case 4504:
											return new WuQiChaoYuanGong();
										case 4506:
											return new WuJiJin();
										}
										break;
									}
									break;
								}
							}
						}
						else if (type <= 5004)
						{
							switch (type)
							{
							case 4601:
								return new SanCaiJianZhen();
							case 4602:
							case 4604:
							case 4606:
								break;
							case 4603:
								return new YinYangZhouTianFa();
							case 4605:
								return new ZhenWuDangMoZhen();
							case 4607:
								return new SanHuaJuDing();
							default:
								switch (type)
								{
								case 5000:
									return new TuZhuoNaQingFa();
								case 5001:
									return new XiaoYuanShanJin();
								case 5002:
									return new DaYuanShanJin();
								case 5003:
									return new BuNianXinJue();
								case 5004:
									return new HunYuanWuXiangGong();
								}
								break;
							}
						}
						else
						{
							switch (type)
							{
							case 5100:
								return new YuanShanTanTui();
							case 5101:
								return new YuanYangLianHuanJiao();
							case 5102:
								return new HuWeiGong();
							case 5103:
								return new JiuGongLuanBaBu();
							case 5104:
								return new DaLiJinGangTui();
							case 5105:
								return new WuDingKaiShanTui();
							case 5106:
								return new YunLongJiuXianTui();
							case 5107:
								return new AoWangShenTui();
							case 5108:
								return new LiuYaSiShenZu();
							default:
								switch (type)
								{
								case 5200:
									return new LuoHanJianFa();
								case 5201:
									return new PuDuJianFa();
								case 5202:
									return new QiShiErChenJian();
								case 5203:
									return new DaCiBeiJian();
								case 5204:
									return new WenShuDaZhiHuiJian();
								case 5205:
									return new WeiTuoFuMoJian();
								case 5206:
									return new WuShangPuTiJian();
								case 5207:
									return new DaMoJianFa();
								default:
									switch (type)
									{
									case 5300:
										return new BaGuaWuXingDao();
									case 5301:
										return new LiuHeDaoFa();
									case 5302:
										return new HuBuBaJiDao();
									case 5303:
										return new TaiYiJinDao();
									case 5304:
										return new QiXingDaoFa();
									case 5305:
										return new WuJiDaoFa();
									case 5306:
										return new TianGangDaoFa();
									case 5307:
										return new TaiXuanShenDao();
									}
									break;
								}
								break;
							}
						}
					}
					else if (type <= 7507)
					{
						if (type <= 6403)
						{
							if (type <= 5605)
							{
								if (type <= 5500)
								{
									switch (type)
									{
									case 5400:
										return new ShiZhuangGong();
									case 5401:
										return new MengHuXiaShanShi();
									case 5402:
										return new QianLiDuXing();
									case 5403:
										return new XuanJiaDing();
									default:
										if (type == 5500)
										{
											return new JiuGunShiBaDie();
										}
										break;
									}
								}
								else
								{
									if (type == 5503)
									{
										return new JinDaoHuanZhangGong();
									}
									if (type == 5506)
									{
										return new TaiChuGuiCangJue();
									}
									switch (type)
									{
									case 5601:
										return new YiDaoYiJian();
									case 5602:
										return new YuanShanQiShaZhen();
									case 5604:
										return new ShiJiaZhiXiang();
									case 5605:
										return new TaiBuXuanDian();
									}
								}
							}
							else if (type <= 6108)
							{
								switch (type)
								{
								case 6000:
									return new FengKouGuQiFa();
								case 6001:
									return new ShiSanTaiBaoHengLianGong();
								case 6002:
									return new TieDingJinShenGong();
								case 6003:
									return new GuangMingShiZiJin();
								default:
									switch (type)
									{
									case 6100:
										return new TuiShanZhang();
									case 6101:
										return new ShiXiangTieDanShou();
									case 6102:
										return new DaKaiMenBaShi();
									case 6103:
										return new DaLiKaiBeiZhang();
									case 6104:
										return new ShiXiangJinShaZhang();
									case 6105:
										return new BaWangKaiGongShou();
									case 6106:
										return new YunKuangShi();
									case 6107:
										return new DaShenWeiZhang();
									case 6108:
										return new DaZhuoShou();
									}
									break;
								}
							}
							else
							{
								switch (type)
								{
								case 6200:
									return new ShiXiangDiTangDao();
								case 6201:
									return new KaiShanKuaiDao();
								case 6202:
									return new ShiXiangPiGuaDao();
								case 6203:
									return new JiuNiuErHuDao();
								case 6204:
									return new ZhanAoDaoFa();
								case 6205:
									return new BaWangDao();
								case 6206:
									return new JinNiZhenMoDao();
								case 6207:
									return new GuiBaShiPanLongDao();
								case 6208:
									return new KuangDao();
								default:
									switch (type)
									{
									case 6300:
										return new EShaQiangFa();
									case 6301:
										return new WuHuDuanHunQiang();
									case 6302:
										return new ShiXiangBaMuQiang();
									case 6303:
										return new HuanHouShiLiuQiang();
									case 6304:
										return new QianKunQiangFa();
									case 6305:
										return new BaiShouZhenYueQiang();
									case 6306:
										return new KuangLongShiZiQiang();
									case 6307:
										return new TuoLongBaWangQiang();
									default:
										switch (type)
										{
										case 6400:
											return new ZhenShanBu();
										case 6401:
											return new TieQiaoGong();
										case 6402:
											return new HengJiangSuo();
										case 6403:
											return new ShiZiFenXun();
										}
										break;
									}
									break;
								}
							}
						}
						else if (type <= 7007)
						{
							if (type <= 6502)
							{
								if (type == 6500)
								{
									return new ShiXiangTieTouGong();
								}
								if (type == 6502)
								{
									return new SunGangYiRou();
								}
							}
							else
							{
								if (type == 6505)
								{
									return new BaWangJieJiaGong();
								}
								switch (type)
								{
								case 6601:
									return new BingWenZhuoSu();
								case 6602:
									break;
								case 6603:
									return new BaWangJuDing();
								case 6604:
									return new ShiXiaoGong();
								default:
									switch (type)
									{
									case 7000:
										return new YangZhiFa();
									case 7001:
										return new ZhuanYuanFa();
									case 7002:
										return new ShiYiFa();
									case 7003:
										return new SunYueFa();
									case 7004:
										return new ShengShenFa();
									case 7005:
										return new FenWeiFa();
									case 7006:
										return new SanShiFa();
									case 7007:
										return new XuanWeiZhenShu();
									}
									break;
								}
							}
						}
						else if (type <= 7208)
						{
							switch (type)
							{
							case 7100:
								return new SanQingZhi();
							case 7101:
								return new GouQianZhi();
							case 7102:
								return new WuYanWuTaiShou();
							case 7103:
								return new ChiShuShi();
							case 7104:
								return new LiHeZhi();
							case 7105:
								return new DaYinYangZongHengShou();
							case 7106:
								return new TianDiYuanYiZhi();
							default:
								switch (type)
								{
								case 7200:
									return new LaiQuJianFa();
								case 7201:
									return new SanQingJianFa();
								case 7202:
									return new FeiJianShu();
								case 7203:
									return new KaiHeJianShu();
								case 7204:
									return new FuMuCongZiJian();
								case 7205:
									return new TaiBaoJiuHuaJian();
								case 7206:
									return new YinYangNiJian();
								case 7207:
									return new TaiShiJianQi();
								case 7208:
									return new WanHuaShiSiJian();
								}
								break;
							}
						}
						else
						{
							switch (type)
							{
							case 7300:
								return new DieDaMeiShanZhou();
							case 7301:
								return new DingShenFa();
							case 7302:
								return new JingGuiFu();
							case 7303:
								return new WuLeiZhaoLaiZhou();
							case 7304:
								return new JiuZiZhenYanZhou();
							case 7305:
								return new MuGongZhou();
							case 7306:
								return new QiBaiPoShenFa();
							case 7307:
								return new JiuXingJinGuangZhou();
							case 7308:
								return new FuJieShenTong();
							default:
								switch (type)
								{
								case 7400:
									return new WuGuiBu();
								case 7401:
									return new YuFengFu();
								case 7402:
									return new DaMiLuoBu();
								case 7403:
									return new XingYunQiWuFa();
								case 7404:
									return new XianZongBu();
								case 7405:
									return new WanLiShenXingZhou();
								case 7406:
									return new LingLiuXu();
								default:
									switch (type)
									{
									case 7500:
										return new WuGuiBanYunFa();
									case 7503:
										return new SheHunDaFa();
									case 7505:
										return new TianGangZhou();
									case 7506:
										return new XiangNiBiZhuanGong();
									case 7507:
										return new WanHuaGeGui();
									}
									break;
								}
								break;
							}
						}
					}
					else if (type <= 8604)
					{
						if (type <= 8308)
						{
							if (type <= 8008)
							{
								switch (type)
								{
								case 7601:
									return new ChangMuFeiErGong();
								case 7602:
									return new YinYangYanShu();
								case 7603:
									break;
								case 7604:
									return new LiuDingLiuJiaZhen();
								default:
									switch (type)
									{
									case 8000:
										return new XuanNvXinJue();
									case 8001:
										return new YuGuGong();
									case 8002:
										return new WangShuYuYueGong();
									case 8003:
										return new ShaoYinYiMingJue();
									case 8004:
										return new XiHuaZhenJing();
									case 8005:
										return new DiNvShenGong();
									case 8006:
										return new YaoChiXianYuFa();
									case 8007:
										return new TaiYuanYuNvGong();
									case 8008:
										return new TaiYinYiMingJue();
									}
									break;
								}
							}
							else
							{
								switch (type)
								{
								case 8100:
									return new YuNvDouLuoShou();
								case 8101:
									return new YuZhiShou();
								case 8102:
									return new HanYinZhang();
								case 8103:
									return new XiaoJiuTianJiuShi();
								case 8104:
									return new JinShangYuDingQuan();
								case 8105:
									return new YaoJiYunYuShi();
								case 8106:
									return new HanBingShenZhang();
								default:
									switch (type)
									{
									case 8200:
										return new YiXuanShou();
									case 8201:
										return new XuanBingZhiFa();
									case 8202:
										return new YuYunZhi();
									case 8203:
										return new LeiZuBoJianShi();
									case 8204:
										return new HanTanCangHuiShi();
									case 8205:
										return new ChangXiShiErShi();
									case 8206:
										return new XuanJiZhiXueFa();
									case 8207:
										return new DaTaiYinYiMingZhi();
									default:
										switch (type)
										{
										case 8300:
											return new QiQingQu();
										case 8301:
											return new QingPingDiao();
										case 8302:
											return new DuanHunYouYinQu();
										case 8303:
											return new HuangZhuGe();
										case 8304:
											return new XiangNvQiCangWu();
										case 8305:
											return new SanXiaoMiXianQu();
										case 8306:
											return new GuangHanGe();
										case 8307:
											return new HouRenXiYi();
										case 8308:
											return new SuNvTianYin();
										}
										break;
									}
									break;
								}
							}
						}
						else if (type <= 8501)
						{
							switch (type)
							{
							case 8400:
								return new MeiDianTou();
							case 8401:
								return new ShangYuGe();
							case 8402:
								return new BieLiBu();
							case 8403:
								return new WangXiaBaBu();
							case 8404:
								return new ChangEBenYue();
							case 8405:
								return new GuSheTaXue();
							case 8406:
								return new QingNvLvBing();
							case 8407:
								return new LuoShenLingBo();
							case 8408:
								return new YuYiGong();
							default:
								if (type == 8501)
								{
									return new JingWeiTianHaiShi();
								}
								break;
							}
						}
						else
						{
							if (type == 8505)
							{
								return new JingHuaShuiYue();
							}
							if (type == 8506)
							{
								return new NvWaBuTianShi();
							}
							switch (type)
							{
							case 8600:
								return new BingQingYuJie();
							case 8602:
								return new MoYuGong();
							case 8603:
								return new TianNvSanHuaZhen();
							case 8604:
								return new XiaShuXianYiFa();
							}
						}
					}
					else if (type <= 9408)
					{
						if (type <= 9107)
						{
							switch (type)
							{
							case 9000:
								return new FanShiChuiDuanPian();
							case 9001:
								return new ZhangShiNaiFuPian();
							case 9002:
								return new WuJinJiaBingPian();
							case 9003:
								return new QianChuiBaiLianPian();
							case 9004:
								return new JiHunShengWeiPian();
							case 9005:
								return new FengJieChuYuanPian();
							default:
								switch (type)
								{
								case 9100:
									return new ShiErLuYuChangCiJian();
								case 9101:
									return new GongBuDuYiJian();
								case 9102:
									return new ShengXieCanJian();
								case 9103:
									return new JuQueQianJunJian();
								case 9104:
									return new LongYuanQiXingJianFa();
								case 9105:
									return new TaiEWuLiangJian();
								case 9106:
									return new ChunJunJianQi();
								case 9107:
									return new ZhanLuJianFa();
								}
								break;
							}
						}
						else
						{
							switch (type)
							{
							case 9200:
								return new XinTingHouDaoFa();
							case 9201:
								return new RuanShiDaoFa();
							case 9202:
								return new DaXiaLongQueBaiZhanDao();
							case 9203:
								return new KunWuDaoFa();
							case 9204:
								return new GuiPaoDingDaoFa();
							case 9205:
								return new ChiDaoQiJue();
							case 9206:
								return new CanDaoShi();
							case 9207:
								return new MingHongJueDao();
							default:
								switch (type)
								{
								case 9300:
									return new XiangLongGunFa();
								case 9301:
									return new SaoXiaGunFa();
								case 9302:
									return new ShiFangShanHeZhang();
								case 9303:
									return new GuiTouBangFa();
								case 9304:
									return new JinJingXuanHuBangFa();
								case 9305:
									return new PengLaiXianChi();
								case 9306:
									return new JiaHaiShenZhang();
								case 9307:
									return new HunYuanTieBangFa();
								default:
									switch (type)
									{
									case 9400:
										return new XiuLiFeiYan();
									case 9401:
										return new CuoGuGou();
									case 9402:
										return new WuZiLianHuanKou();
									case 9403:
										return new QiXingLongZhuaSuo();
									case 9404:
										return new TianLuoDiWang();
									case 9405:
										return new TaiShanSuo();
									case 9406:
										return new HuangLongMuYuan();
									case 9407:
										return new TianShuXuanJi();
									case 9408:
										return new ShenGongRuYiTa();
									}
									break;
								}
								break;
							}
						}
					}
					else if (type <= 9706)
					{
						switch (type)
						{
						case 9500:
							return new MuLvGong();
						case 9501:
							return new YuChePian();
						case 9502:
							return new BaFangZhuShi();
						case 9503:
							return new HuQiuShiJian();
						case 9504:
							return new XingDouBiNu();
						case 9505:
							return new ChengLongShu();
						case 9506:
							return new TianKaiLiuYunShi();
						default:
							switch (type)
							{
							case 9700:
								return new QiShouBaJiaoGong();
							case 9701:
								return new MuJiaHuShenFa();
							case 9702:
								return new PiJiaZhenGangShiSiJue();
							case 9703:
								return new ShenJiZhen();
							case 9704:
								return new TianBingPiJiaShu();
							case 9705:
								return new LingLongJiuQiao();
							case 9706:
								return new TianZhuXuanTieCe();
							}
							break;
						}
					}
					else
					{
						switch (type)
						{
						case 10000:
							return new QiHuangYaoLve();
						case 10001:
							return new ShenNongBenCaoMiYao();
						case 10002:
							return new ChaiShanQingNangJue();
						case 10003:
							return new JiaoZheSiFa();
						case 10004:
							return new JiuDingGong();
						default:
							switch (type)
							{
							case 10100:
								return new ChaiShanWuQinQuan();
							case 10101:
								return new DaXueShanZhangFa();
							case 10102:
								return new ZhangXueGong();
							case 10103:
								return new WeiLingXianHuaGuZhang();
							case 10104:
								return new DanShaShenZhang();
							case 10105:
								return new QingJiaoShenZhang();
							default:
								switch (type)
								{
								case 10200:
									return new ChaiShanQinDieShou();
								case 10201:
									return new GouWenZhua();
								case 10202:
									return new TieHuaFuGuZhua();
								case 10203:
									return new YuChanZhi();
								case 10204:
									return new QingJiaoTieXuZhi();
								case 10205:
									return new WuHuanShou();
								}
								break;
							}
							break;
						}
					}
				}
				else if (type <= 14207)
				{
					if (type <= 12208)
					{
						if (type <= 11208)
						{
							if (type <= 10603)
							{
								if (type <= 10408)
								{
									switch (type)
									{
									case 10300:
										return new DaoDaChongTianZi();
									case 10301:
										return new ChaiShanYiBuZhuan();
									case 10302:
										return new DuJuanZhuoZhuShi();
									case 10303:
										return new KeJinZhenYuXiaoBaShi();
									case 10304:
										return new QingJiaoBaiWeiGong();
									case 10305:
										return new QingJiaoFanTengTui();
									case 10306:
										return new FeiShanDuanHaiDaBaShi();
									case 10307:
										return new XuanDingBaiBu();
									default:
										switch (type)
										{
										case 10400:
											return new ZhenYuXiang();
										case 10401:
											return new BiAnBaXianZi();
										case 10402:
											return new JingMengXiang();
										case 10403:
											return new BiXueShuiXian();
										case 10405:
											return new HeiShuiDuanChangSan();
										case 10406:
											return new BaiXieXiaoGuXiang();
										case 10407:
											return new QingJiaoXue();
										case 10408:
											return new GuiGuXueHaiTang();
										}
										break;
									}
								}
								else
								{
									switch (type)
									{
									case 10500:
										return new CaoShangFei();
									case 10501:
										return new WeiYaXuanHu();
									case 10502:
										return new ShaTangTaShui();
									case 10503:
										return new ChaiShanYiXianTian();
									case 10504:
										return new QingJiaoNaoHaiGong();
									default:
										if (type == 10603)
										{
											return new WuHuangBiDuShu();
										}
										break;
									}
								}
							}
							else if (type <= 10708)
							{
								if (type == 10607)
								{
									return new WuHuangBiDuShu();
								}
								switch (type)
								{
								case 10700:
									return new SiZhiQingGao();
								case 10701:
									return new SanBuJiuHouFa();
								case 10702:
									return new TouGuQiePouFa();
								case 10704:
									return new ZiWuAnXiXiang();
								case 10705:
									return new QuXueFa();
								case 10706:
									return new BaiShuiChangShengFa();
								case 10708:
									return new JiuSeYuChanFa();
								}
							}
							else
							{
								switch (type)
								{
								case 11000:
									return new JinGangSiJiaXingFa();
								case 11001:
									return new TuMuGong();
								case 11002:
									return new SanMiHeYingFa();
								case 11003:
									return new BaoPingQiGongFa();
								case 11004:
									return new JinGangGuanDingGong();
								case 11005:
									return new DaRiJing();
								case 11006:
									return new JinGangDingJing();
								case 11007:
									return new MoHeJiaLuoHuFaGong();
								case 11008:
									return new WuShangYuJiaFa();
								default:
									switch (type)
									{
									case 11100:
										return new JinGangChui();
									case 11101:
										return new WuNuShou();
									case 11102:
										return new BoLuoMiZhang();
									case 11103:
										return new JinGangHeiShaZhang();
									case 11104:
										return new LingReZhang();
									case 11105:
										return new JinGangZhuMieShou();
									case 11106:
										return new XuMiShanSiShenZhang();
									case 11107:
										return new DaRiRuLaiZhang();
									default:
										switch (type)
										{
										case 11200:
											return new JinGangDaoFa();
										case 11201:
											return new LuoChaDaoFa();
										case 11202:
											return new MoZhangDaoFa();
										case 11203:
											return new LingReDao();
										case 11204:
											return new JinGangXiangFuDao();
										case 11205:
											return new JinGangBoReDao();
										case 11206:
											return new RuYiBaoLunDao();
										case 11207:
											return new LiuShenWeiDao();
										case 11208:
											return new MoHeJiaLuoDao();
										}
										break;
									}
									break;
								}
							}
						}
						else if (type <= 11700)
						{
							if (type <= 11405)
							{
								switch (type)
								{
								case 11300:
									return new SuiShiChu();
								case 11301:
									return new GameData.Domains.SpecialEffect.CombatSkill.Jingangzong.Special.HuFaJinGangChu();
								case 11302:
									return new LuoChaChuFa();
								case 11303:
									return new GameData.Domains.SpecialEffect.CombatSkill.Jingangzong.Special.NuMuJinGangChu();
								case 11304:
									return new DaLunJinGangChu();
								case 11305:
									return new GameData.Domains.SpecialEffect.CombatSkill.Jingangzong.Special.DaWeiDeJinGangChu();
								case 11306:
									return new RuYiBaoShuChu();
								case 11307:
									return new GameData.Domains.SpecialEffect.CombatSkill.Jingangzong.Special.BuDongMingWangChu();
								default:
									switch (type)
									{
									case 11401:
										return new GameData.Domains.SpecialEffect.CombatSkill.Jingangzong.PestleEffect.HuFaJinGangChu();
									case 11403:
										return new GameData.Domains.SpecialEffect.CombatSkill.Jingangzong.PestleEffect.NuMuJinGangChu();
									case 11405:
										return new GameData.Domains.SpecialEffect.CombatSkill.Jingangzong.PestleEffect.DaWeiDeJinGangChu();
									}
									break;
								}
							}
							else
							{
								switch (type)
								{
								case 11500:
									return new XianShiJiao();
								case 11501:
									return new DaDingJiao();
								case 11502:
									return new JinGangZuoFa();
								case 11503:
									return new ShenZuTong();
								default:
									switch (type)
									{
									case 11601:
										return new NaMaiGong();
									case 11602:
										return new YiJinSuoGuGong();
									case 11603:
									case 11606:
										break;
									case 11604:
										return new ManTuLuoZhenYan();
									case 11605:
										return new MingWangZhuoHuoDing();
									case 11607:
										return new JiShenChengFo();
									default:
										if (type == 11700)
										{
											return new SuiSuoYu();
										}
										break;
									}
									break;
								}
							}
						}
						else if (type <= 11706)
						{
							if (type == 11703)
							{
								return new QiLunGanYingFa();
							}
							if (type == 11706)
							{
								return new DaShouYin();
							}
						}
						else
						{
							switch (type)
							{
							case 12000:
								return new YinDuShu();
							case 12001:
								return new HuangXianFa();
							case 12002:
								return new ChiXianFa();
							case 12003:
								return new HeiXianFa();
							case 12004:
								return new QingXianFa();
							case 12005:
								return new LiHunGong();
							case 12006:
								return new BaiXieTiDaFa();
							default:
								switch (type)
								{
								case 12100:
									return new YinFengXieZiShou();
								case 12101:
									return new FuSheQianZhenShou();
								case 12102:
									return new XueZhiZhang();
								case 12103:
									return new SanShiDuanHunZhang();
								case 12104:
									return new QingZhuChanHunShou();
								case 12105:
									return new ZhuChanDuZhang();
								case 12106:
									return new QingHuangChiHeiShenZhang();
								case 12107:
									return new JiuSiLiHunShou();
								default:
									switch (type)
									{
									case 12200:
										return new SheGuShou();
									case 12201:
										return new ChiMuGuJiang();
									case 12202:
										return new ChiMeiGuJiang();
									case 12203:
										return new HeiXueGuJiang();
									case 12204:
										return new XinMoGuJiang();
									case 12205:
										return new JiuYinShiChiGu();
									case 12206:
										return new BiYuBingCanGu();
									case 12207:
										return new WangGuJinCanJiang();
									case 12208:
										return new BaiCaiQingSuiGu();
									}
									break;
								}
								break;
							}
						}
					}
					else if (type <= 13108)
					{
						if (type <= 12800)
						{
							if (type <= 12408)
							{
								switch (type)
								{
								case 12300:
									return new LvRanJianFa();
								case 12301:
									return new GouLianJianFa();
								case 12302:
									return new GuiPoPoJian();
								case 12303:
									return new ShenMuBiXieJian();
								case 12304:
									return new GuiMianZhuiHunJian();
								case 12305:
									return new ShenChiZhanYaoJian();
								case 12306:
									return new QingZhuJianFa();
								default:
									switch (type)
									{
									case 12400:
										return new HuangLinBianFa();
									case 12401:
										return new WuGongSuo();
									case 12402:
										return new WuShengBianFa();
									case 12403:
										return new GouHunSuiGuBian();
									case 12404:
										return new ChiYouTieBian();
									case 12405:
										return new PoYuSuo();
									case 12406:
										return new XianZhuQinMangGong();
									case 12407:
										return new BaLongShenBian();
									case 12408:
										return new TianSheSuo();
									}
									break;
								}
							}
							else
							{
								switch (type)
								{
								case 12500:
									return new RedEyeGrowingGood1();
								case 12501:
									return new RedEyeGrowingGood2();
								case 12502:
									return new RedEyeGrowingBad1();
								case 12503:
									return new RedEyeGrowingBad2();
								case 12504:
									return new RedEyeGrown();
								case 12505:
									return new ForestSpiritGrowingGood1();
								case 12506:
									return new ForestSpiritGrowingGood2();
								case 12507:
									return new ForestSpiritGrowingBad1();
								case 12508:
									return new ForestSpiritGrowingBad2();
								case 12509:
									return new ForestSpiritGrown();
								case 12510:
									return new BlackBloodGrowingGood1();
								case 12511:
									return new BlackBloodGrowingGood2();
								case 12512:
									return new BlackBloodGrowingBad1();
								case 12513:
									return new BlackBloodGrowingBad2();
								case 12514:
									return new BlackBloodGrown();
								case 12515:
									return new DevilInsideGrowingGood1();
								case 12516:
									return new DevilInsideGrowingGood2();
								case 12517:
									return new DevilInsideGrowingBad1();
								case 12518:
									return new DevilInsideGrowingBad2();
								case 12519:
									return new DevilInsideGrown();
								case 12520:
									return new CorpseWormGrowingGood1();
								case 12521:
									return new CorpseWormGrowingGood2();
								case 12522:
									return new CorpseWormGrowingBad1();
								case 12523:
									return new CorpseWormGrowingBad2();
								case 12524:
									return new CorpseWormGrown();
								case 12525:
									return new IceSilkwormGrowingGood1();
								case 12526:
									return new IceSilkwormGrowingGood2();
								case 12527:
									return new IceSilkwormGrowingBad1();
								case 12528:
									return new IceSilkwormGrowingBad2();
								case 12529:
									return new IceSilkwormGrown();
								case 12530:
									return new GoldenSilkwormGrowingGood1();
								case 12531:
									return new GoldenSilkwormGrowingGood2();
								case 12532:
									return new GoldenSilkwormGrowingBad1();
								case 12533:
									return new GoldenSilkwormGrowingBad2();
								case 12534:
									return new GoldenSilkwormGrown();
								case 12535:
									return new AzureMarrowGrowingGood1();
								case 12536:
									return new AzureMarrowGrowingGood2();
								case 12537:
									return new AzureMarrowGrowingBad1();
								case 12538:
									return new AzureMarrowGrowingBad2();
								case 12539:
									return new AzureMarrowGrown();
								case 12540:
									return new RedEyeKing();
								case 12541:
									return new ForestSpiritKing();
								case 12542:
									return new BlackBloodKing();
								case 12543:
									return new DevilInsideKing();
								case 12544:
									return new CorpseWormKing();
								case 12545:
									return new IceSilkwormKing();
								case 12546:
									return new GoldenSilkwormKing();
								case 12547:
									return new AzureMarrowKing();
								case 12548:
								case 12549:
								case 12550:
								case 12551:
								case 12552:
								case 12553:
								case 12554:
								case 12555:
								case 12556:
								case 12557:
								case 12558:
								case 12559:
								case 12560:
								case 12561:
								case 12562:
								case 12563:
								case 12564:
								case 12565:
								case 12566:
								case 12567:
								case 12568:
								case 12569:
								case 12570:
								case 12571:
								case 12572:
								case 12573:
								case 12574:
								case 12575:
								case 12576:
								case 12577:
								case 12578:
								case 12579:
								case 12580:
								case 12581:
								case 12582:
								case 12583:
								case 12584:
								case 12585:
								case 12586:
								case 12587:
								case 12588:
								case 12589:
								case 12590:
								case 12591:
								case 12592:
								case 12593:
								case 12594:
								case 12595:
								case 12596:
								case 12597:
								case 12598:
								case 12599:
									break;
								case 12600:
									return new SheXingGong();
								case 12601:
									return new ChuangNiTan();
								case 12602:
									return new HaMaZongTiaoGong();
								case 12603:
									return new FeiTouShu();
								case 12604:
									return new YuSuoDaoXuan();
								case 12605:
									return new DunDiBaiZuXian();
								case 12606:
									return new TianSheFan();
								default:
									switch (type)
									{
									case 12701:
										return new LuanQingZhang();
									case 12702:
									case 12706:
										break;
									case 12703:
										return new MieGuZhou();
									case 12704:
										return new XueOuPoShaFa();
									case 12705:
										return new SangHunXi();
									case 12707:
										return new GuiJiangDaFa();
									default:
										if (type == 12800)
										{
											return new MiChongXiang();
										}
										break;
									}
									break;
								}
							}
						}
						else if (type <= 12806)
						{
							if (type == 12802)
							{
								return new HongYuJiuXiangChong();
							}
							if (type == 12806)
							{
								return new TianCanShiGu();
							}
						}
						else
						{
							if (type == 12808)
							{
								return new TianSheHuanGuGong();
							}
							switch (type)
							{
							case 13000:
								return new JieQingSiXiangTu();
							case 13001:
								return new TianYuanYangQiFa();
							case 13002:
								return new DongBiShiErJuanShu();
							case 13003:
								return new BuTianGe();
							case 13004:
								return new XuanMingZhenJing();
							case 13005:
								return new YiSiYiCan();
							case 13006:
								return new HunTianXingTu();
							default:
								switch (type)
								{
								case 13100:
									return new WuXingXiaoQinNa();
								case 13101:
									return new ZhaiXingShi();
								case 13102:
									return new YuJingHuaMaiShou();
								case 13103:
									return new FuYinZhi();
								case 13104:
									return new TianJieQiYaoZhi();
								case 13105:
									return new XuanMingZhi();
								case 13106:
									return new TaiSuJueShou();
								case 13107:
									return new CuoShenZhi();
								case 13108:
									return new TaiYiLiuLiZhi();
								}
								break;
							}
						}
					}
					else if (type <= 13600)
					{
						if (type <= 13308)
						{
							switch (type)
							{
							case 13200:
								return new JieQingKuaiJian();
							case 13201:
								return new JueYiJian();
							case 13202:
								return new XianChiJianQi();
							case 13203:
								return new JieQingAnShouKuaiJian();
							case 13204:
								return new ShaJian();
							case 13205:
								return new YouMingJianFa();
							case 13206:
								return new WuXiaQiJueJian();
							case 13207:
								return new XuanMingJianQi();
							default:
								switch (type)
								{
								case 13300:
									return new JieQingShiJue();
								case 13301:
									return new ZhaiYeFeiHuaShu();
								case 13302:
									return new WuYingLiuShou();
								case 13303:
									return new FeiXingShu();
								case 13304:
									return new LuanFeiHuang();
								case 13305:
									return new YaoTuQiShu();
								case 13306:
									return new MingLongZhi();
								case 13307:
									return new DingYingShenZhen();
								case 13308:
									return new WuXiangShenTong();
								}
								break;
							}
						}
						else
						{
							switch (type)
							{
							case 13400:
								return new BaiChiZhuang();
							case 13401:
								return new WuZongLiuBu();
							case 13402:
								return new WuYouBu();
							case 13403:
								return new TianHeYouBu();
							case 13404:
								return new ChengWuChe();
							case 13405:
								return new ShenXianSuo();
							case 13406:
								return new QiXingFeiXuan();
							case 13407:
								return new TianYuanZong();
							case 13408:
								return new HunTianYiXingGong();
							default:
								switch (type)
								{
								case 13501:
									return new HeBoQuQi();
								case 13502:
									return new LiuZhuanTianHuang();
								case 13503:
									return new HanBingBaMai();
								case 13504:
								case 13505:
									break;
								case 13506:
									return new QiYuanJieEDaFa();
								default:
									if (type == 13600)
									{
										return new TianGuanTong();
									}
									break;
								}
								break;
							}
						}
					}
					else if (type <= 13605)
					{
						if (type == 13604)
						{
							return new NanDouLiuXingZhen();
						}
						if (type == 13605)
						{
							return new JiuYaoXingJunYan();
						}
					}
					else
					{
						switch (type)
						{
						case 14000:
							return new BianTiHuoQiFa();
						case 14001:
							return new FuLongTunHuoGong();
						case 14002:
							return new LiHuoZhenQi();
						case 14003:
							return new XiaoYuYangShenGong();
						case 14004:
							return new KuangLongTunXiangGong();
						case 14005:
							return new JiuLongGuiYiGong();
						case 14006:
							return new RuMoGong();
						case 14007:
							return new DaYuYangShenGong();
						default:
							switch (type)
							{
							case 14100:
								return new DuanGuQuan();
							case 14101:
								return new FuLongZuiBaXianQuan();
							case 14102:
								return new HuoLianZhang();
							case 14103:
								return new FengMoZuiQuan();
							case 14104:
								return new DaXuanJueZhang();
							case 14105:
								return new XiaoYuYangShenQuan();
							case 14106:
								return new LiHuoLiuYangZhang();
							case 14107:
								return new DaYuYangShenQuan();
							case 14108:
								return new HuaLongZhang();
							default:
								switch (type)
								{
								case 14200:
									return new FuLongDaoFa();
								case 14201:
									return new JiuGongZuiDao();
								case 14202:
									return new LongYaSiZhan();
								case 14203:
									return new LiuXinSiYiDao();
								case 14204:
									return new KuangLongBaShiYiZhan();
								case 14205:
									return new DaLiangYiZuiDao();
								case 14206:
									return new ChiZiZhanLongDao();
								case 14207:
									return new JiuLongBaDao();
								}
								break;
							}
							break;
						}
					}
				}
				else if (type <= 17135)
				{
					if (type <= 15509)
					{
						if (type <= 15008)
						{
							if (type <= 14405)
							{
								switch (type)
								{
								case 14300:
									return new FeiShaZouShi();
								case 14301:
									return new LeiHuoDa();
								case 14302:
									return new LongTuZhu();
								case 14303:
									return new ChiQingShenHuoJin();
								case 14304:
									return new FenXueGong();
								case 14305:
									return new JiuLianJinWu();
								default:
									switch (type)
									{
									case 14400:
										return new ChiJiaoGong();
									case 14401:
										return new ZuiBaXianBu();
									case 14402:
										return new LongBaiWei();
									case 14403:
										return new FuLongFenYue();
									case 14404:
										return new FengHuoZhenXingJue();
									case 14405:
										return new ZuiWoDongHai();
									}
									break;
								}
							}
							else
							{
								switch (type)
								{
								case 14501:
									return new LongHuiShou();
								case 14502:
									return new ShenHuoZhaoGong();
								case 14503:
								case 14505:
								case 14506:
									break;
								case 14504:
									return new LeiGongZhuGu();
								case 14507:
									return new ZheTianBiRiGong();
								default:
									switch (type)
									{
									case 14600:
										return new NuXiangGong();
									case 14601:
									case 14602:
									case 14604:
										break;
									case 14603:
										return new LongChuangZhen();
									case 14605:
										return new TianMangDanXinShu();
									case 14606:
										return new QianNianZui();
									default:
										switch (type)
										{
										case 15000:
											return new HouMuGong();
										case 15001:
											return new HeiShiSiJue();
										case 15002:
											return new XieGuiGong();
										case 15003:
											return new SanDuWuMingZhou();
										case 15004:
											return new TaiYinLianXing();
										case 15005:
											return new WuYinDaFa();
										case 15006:
											return new TongZiXueLianFa();
										case 15007:
											return new XueHouXieJing();
										case 15008:
											return new XueTongDaFa();
										}
										break;
									}
									break;
								}
							}
						}
						else if (type <= 15206)
						{
							switch (type)
							{
							case 15100:
								return new FengGouQuan();
							case 15101:
								return new CuiXinZhang();
							case 15102:
								return new QiShiErDiShaQuan();
							case 15103:
								return new FuShiDuZhang();
							case 15104:
								return new XueShaZhang();
							case 15105:
								return new BaiGuiHuaGuZhang();
							case 15106:
								return new TianMoCanHunZhang();
							case 15107:
								return new XueHouMoZhang();
							default:
								switch (type)
								{
								case 15200:
									return new ChuoYanXiaoErGong();
								case 15201:
									return new SuoHouYinShou();
								case 15202:
									return new FenJinCuoGuShou();
								case 15203:
									return new SanZhiDuoYinZhua();
								case 15204:
									return new DuLongZhua();
								case 15205:
									return new HuangQuanZhi();
								case 15206:
									return new XueChiGuiZhua();
								}
								break;
							}
						}
						else
						{
							switch (type)
							{
							case 15300:
								return new LiaoYinTui();
							case 15301:
								return new HaMaDaoTiTui();
							case 15302:
								return new WuChangJiao();
							case 15303:
								return new XieZiGouHunJiao();
							case 15304:
								return new BianZouBianSha();
							case 15305:
								return new DaYinFengTui();
							case 15306:
								return new YanWangGuiJiao();
							case 15307:
								return new XueYingTui();
							default:
								switch (type)
								{
								case 15400:
									return new YeGuiDuSha();
								case 15401:
									return new FuChongSha();
								case 15402:
									return new ShiGuLanChangSha();
								case 15403:
									return new HuangFenSha();
								case 15404:
									return new JinChanWuDuSha();
								case 15405:
									return new DuNiangZiSha();
								case 15406:
									return new XueHouSha();
								default:
									switch (type)
									{
									case 15500:
										return new ChestCrash();
									case 15501:
										return new ChestHurt();
									case 15502:
										return new BellyCrash();
									case 15503:
										return new BellyHurt();
									case 15504:
										return new HeadCrash();
									case 15505:
										return new HeadHurt();
									case 15506:
										return new HandCrash();
									case 15507:
										return new HandHurt();
									case 15508:
										return new LegCrash();
									case 15509:
										return new LegHurt();
									}
									break;
								}
								break;
							}
						}
					}
					else if (type <= 16008)
					{
						if (type <= 15704)
						{
							switch (type)
							{
							case 15600:
								return new LiCuanShu();
							case 15601:
								return new FenTouDun();
							case 15602:
								return new YouHunGuiBu();
							case 15603:
								return new GuiHouRuDiZhenTianFa();
							case 15604:
								return new FeiShiDaNuoWu();
							default:
								switch (type)
								{
								case 15700:
									return new LanNiJueJi();
								case 15702:
									return new GuiYeKu();
								case 15704:
									return new YiHunDaFa();
								}
								break;
							}
						}
						else
						{
							if (type == 15707)
							{
								return new TianMoTong();
							}
							switch (type)
							{
							case 15801:
								return new LuanQiSha();
							case 15802:
							case 15804:
							case 15807:
								break;
							case 15803:
								return new XueShiGong();
							case 15805:
								return new XinWoLiangMieZhou();
							case 15806:
								return new ShiErXueTongDaZhen();
							case 15808:
								return new XueTongBuSiShen();
							default:
								switch (type)
								{
								case 16000:
									return new WenESheng();
								case 16001:
									return new QuShan();
								case 16002:
									return new HuanMu();
								case 16003:
									return new YaoXinShiXian();
								case 16004:
									return new BaiXie();
								case 16005:
									return new DuoXinJiuBuZhong();
								case 16006:
									return new ZhongXiangSheng();
								case 16007:
									return new ShenDuanHuFa();
								case 16008:
									return new XuanYuJiuLao();
								}
								break;
							}
						}
					}
					else if (type <= 16213)
					{
						switch (type)
						{
						case 16100:
							return new ShenNvHuanJian();
						case 16101:
							return new ZhenYuFuXie();
						case 16102:
							return new QiHanLingQi();
						case 16103:
							return new QiWenWuCai();
						case 16104:
							return new QingGuoJueShi();
						case 16105:
							return new ZhuYinHuaMing();
						case 16106:
							return new RongChenHuaYu();
						case 16107:
							return new BaHongBaZhi();
						case 16108:
							return new FangTianChiLing();
						case 16109:
							return new WanXiangHeShu();
						case 16110:
							return new ShiZhuanChenXin();
						case 16111:
							return new FuJunYouYu();
						case 16112:
							return new SuXinWuRan();
						case 16113:
							return new ChiXinWuGai();
						default:
							switch (type)
							{
							case 16200:
								return new JiangXian();
							case 16201:
								return new XieNiWuSheng();
							case 16202:
								return new CheDiBingTian();
							case 16203:
								return new LiuXiang();
							case 16204:
								return new YiFeiYan();
							case 16205:
								return new BaiLongQianYuan();
							case 16206:
								return new JingYingDunXiang();
							case 16207:
								return new XueLiuPiaoLu();
							case 16208:
								return new ChengGuangPiXia();
							case 16209:
								return new MoWoZiZai();
							case 16210:
								return new ChenRan();
							case 16211:
								return new HuaJian();
							case 16212:
								return new YaoYin();
							case 16213:
								return new ShenHuaJiuYao();
							}
							break;
						}
					}
					else
					{
						switch (type)
						{
						case 16300:
							return new YunShuiHuaLingYi();
						case 16301:
							return new BuBai();
						case 16302:
							return new ShuangTaiBingPo();
						case 16303:
							return new JiuBao();
						case 16304:
							return new XinHuaLiaoMeng();
						case 16305:
							return new HaiLongJia();
						case 16306:
							return new XiTuShiLing();
						case 16307:
							return new FengShenZhaoMing();
						case 16308:
							return new SanYuanJiuDunTianDiBian();
						case 16309:
							return new XuanYuJueShen();
						case 16310:
							return new WuMieShiXin();
						case 16311:
							return new HuanXin();
						case 16312:
							return new XuHuaSheYue();
						case 16313:
							return new DiZhaoXuanHuoYin();
						default:
							switch (type)
							{
							case 16400:
								return new DongLaiYiQi();
							case 16401:
								return new JinGangDuanE();
							case 16402:
								return new JiuYinShiYou();
							case 16403:
								return new TianWaiZhenHuo();
							case 16404:
								return new XiangHuanMiZhen();
							case 16405:
								return new ShenWaiQiuFa();
							case 16406:
								return new XiGuiShouShen();
							case 16407:
								return new ShenTuFuSha();
							case 16408:
								return new FenShenJingJie();
							case 16409:
								return new JieWaiTunXin();
							case 16410:
								return new YinChen();
							case 16411:
								return new HuaShengLian();
							case 16412:
								return new LiDuanShenMo();
							case 16413:
								return new FuTianHuoZhao();
							case 16414:
								return new ChiEr();
							case 16415:
								return new ShiShiYiRen();
							case 16416:
								return new SiJie();
							case 16417:
								return new LiYouJinMeng();
							case 16418:
								return new PiXueGuaYan();
							default:
								switch (type)
								{
								case 17000:
									return new CunXinRuMeng();
								case 17001:
									return new YouTanQiShui();
								case 17002:
									return new WuSeYing();
								case 17003:
									return new CunXinDuanMeng();
								case 17004:
									return new XuanTanGuiDu();
								case 17005:
									return new LuanCaiYing();
								case 17010:
									return new PoYaoHui();
								case 17011:
									return new MieShaSanZhan();
								case 17012:
									return new TianMoXiangFu();
								case 17013:
									return new ZhanELong();
								case 17014:
									return new SheDuanSanZhan();
								case 17015:
									return new TianMoPoJin();
								case 17020:
									return new SuXue();
								case 17021:
									return new QingShenYouWu();
								case 17022:
									return new QiBeiQu();
								case 17023:
									return new NingHong();
								case 17024:
									return new QingShenSiWu();
								case 17025:
									return new SuSiGe();
								case 17030:
									return new GuiXiTiFu();
								case 17031:
									return new HuoZhiBaFeng();
								case 17032:
									return new ChiYuPoKong();
								case 17033:
									return new ShanZaiHeShi();
								case 17034:
									return new FengYinJiuChu();
								case 17035:
									return new JinLingPoKong();
								case 17040:
									return new XunHu();
								case 17041:
									return new CanHuTi();
								case 17042:
									return new SiXiangXueHuoShu();
								case 17043:
									return new GuiHu();
								case 17044:
									return new CanHuTong();
								case 17045:
									return new FenShenXueHuoShu();
								case 17050:
									return new LongTunShi();
								case 17051:
									return new LongXianShi();
								case 17052:
									return new LongZhuShi();
								case 17053:
									return new SheShenShi();
								case 17054:
									return new SheMingShi();
								case 17055:
									return new SheXinShi();
								case 17060:
									return new WuXinHuaXin();
								case 17061:
									return new WuJianHuaJian();
								case 17062:
									return new WuHunHuaHun();
								case 17063:
									return new XinHuaWuXin();
								case 17064:
									return new JianHuaWuJian();
								case 17065:
									return new HunHuaWuHun();
								case 17070:
									return new ZhanWuShu();
								case 17071:
									return new XiongBingChuangSanZhen();
								case 17072:
									return new XingFengSaoSiYe();
								case 17073:
									return new ShaWuShe();
								case 17074:
									return new BingZhuZhanLiuHe();
								case 17075:
									return new XueYuYuBaHuang();
								case 17080:
									return new KunLunShiErJinXianLing();
								case 17081:
									return new TianYanShu();
								case 17082:
									return new TianDiXuanYiLing();
								case 17083:
									return new KunLunShiErXieXianLing();
								case 17084:
									return new MoTongShu();
								case 17085:
									return new GuiShenXuanYiLing();
								case 17090:
									return new RuXinYuanXiang();
								case 17091:
									return new RuXuYanXiang();
								case 17092:
									return new RuWangMingXiang();
								case 17093:
									return new XinMiWuJue();
								case 17094:
									return new XieWangWuZheng();
								case 17095:
									return new BianRanWuJing();
								case 17100:
									return new JieFeng();
								case 17101:
									return new ChuQiao();
								case 17102:
									return new ShiFeng();
								case 17103:
									return new ChaiRen();
								case 17104:
									return new BiGuang();
								case 17105:
									return new DuoShen();
								case 17110:
									return new SanSanHuaLing();
								case 17111:
									return new QiQiShengHui();
								case 17112:
									return new JiuJiuZaoMing();
								case 17113:
									return new LingSanTianDiRen();
								case 17114:
									return new HuiWangQiJueZhi();
								case 17115:
									return new MingYunWuJianYu();
								case 17120:
									return new DanFengZhuoRui();
								case 17121:
									return new ChiNiZhengYi();
								case 17122:
									return new HuoLongFenXin();
								case 17123:
									return new HuoRuiShaJiaoFeng();
								case 17124:
									return new ChiYiXiangENi();
								case 17125:
									return new DanXinSheYaoLong();
								case 17130:
									return new YiJianRongChenYin();
								case 17131:
									return new ErJianMoNvYi();
								case 17132:
									return new SanJianFuXieTie();
								case 17133:
									return new SiWuJian();
								case 17134:
									return new LiuQiBaJian();
								case 17135:
									return new JiuJianGuiShenXia();
								}
								break;
							}
							break;
						}
					}
				}
				else if (type <= 40605)
				{
					if (type <= 40106)
					{
						if (type <= 30117)
						{
							switch (type)
							{
							case 30000:
								return new PoZhang();
							case 30001:
								return new PoJian();
							case 30002:
								return new PoDao();
							case 30003:
								return new PoDu();
							case 30004:
								return new PoChangBing();
							case 30005:
								return new PoRuanBing();
							case 30006:
								return new PoAnQi();
							case 30007:
								return new PoQiMen();
							case 30008:
								return new PoMoYin();
							case 30009:
								return new PoJin();
							case 30010:
								return new PoMu();
							case 30011:
								return new PoYu();
							case 30012:
								return new PoJin2();
							case 30013:
								return new HuaXue();
							case 30014:
								return new HuaQi();
							case 30015:
								return new XueSha();
							case 30016:
								return new QiSha();
							case 30017:
								return new GameData.Domains.SpecialEffect.EquipmentEffect.Weapon.DuCi();
							default:
								switch (type)
								{
								case 30100:
									return new PiZhang();
								case 30101:
									return new PiJian();
								case 30102:
									return new PiDao();
								case 30103:
									return new PiDu();
								case 30104:
									return new PiChangBing();
								case 30105:
									return new PiRuanBing();
								case 30106:
									return new PiAnQi();
								case 30107:
									return new PiQiMen();
								case 30108:
									return new PiMoYin();
								case 30109:
									return new PiJin();
								case 30110:
									return new PiMu();
								case 30111:
									return new PiYu();
								case 30112:
									return new PiJin2();
								case 30113:
									return new ZhenXue();
								case 30114:
									return new ZhenQi();
								case 30115:
									return new HuXue();
								case 30116:
									return new HuQi();
								case 30117:
									return new GameData.Domains.SpecialEffect.EquipmentEffect.Armor.DuCi();
								}
								break;
							}
						}
						else
						{
							switch (type)
							{
							case 30200:
								return new YanXiaShenZhu();
							case 30201:
								return new ShanHeShenJie();
							case 30202:
								return new GuiYanLingZhuo();
							case 30203:
								return new XuanHuShenJie();
							case 30204:
								return new XianYuanShenJie();
							case 30205:
								return new HunYuanShenZhu();
							default:
								switch (type)
								{
								case 40001:
									return new GameData.Domains.SpecialEffect.LegendaryBook.Neigong.ZhuanJie();
								case 40002:
									return new YongJi();
								case 40003:
									return new DaYing();
								case 40004:
									return new DaCheng();
								case 40005:
									return new GameData.Domains.SpecialEffect.LegendaryBook.Neigong.JueZhi();
								case 40006:
									return new GameData.Domains.SpecialEffect.LegendaryBook.Neigong.ShouZhi();
								default:
									switch (type)
									{
									case 40101:
										return new GameData.Domains.SpecialEffect.LegendaryBook.Posing.ZhuanJie();
									case 40102:
										return new FengShen();
									case 40103:
										return new YunYong();
									case 40104:
										return new JingShui();
									case 40105:
										return new GameData.Domains.SpecialEffect.LegendaryBook.Posing.JueZhi();
									case 40106:
										return new GameData.Domains.SpecialEffect.LegendaryBook.Posing.ShouZhi();
									}
									break;
								}
								break;
							}
						}
					}
					else if (type <= 40305)
					{
						switch (type)
						{
						case 40201:
							return new GameData.Domains.SpecialEffect.LegendaryBook.Stunt.ZhuanJie();
						case 40202:
							return new YuanLiu();
						case 40203:
							return new ChaNa();
						case 40204:
							return new ZhouQuan();
						case 40205:
							return new GameData.Domains.SpecialEffect.LegendaryBook.Stunt.JueZhi();
						case 40206:
							return new GameData.Domains.SpecialEffect.LegendaryBook.Stunt.ShouZhi();
						default:
							switch (type)
							{
							case 40300:
								return new KeDi();
							case 40301:
								return new GameData.Domains.SpecialEffect.LegendaryBook.FistAndPalm.ZhuanJie();
							case 40302:
								return new JinSha();
							case 40303:
								return new GameData.Domains.SpecialEffect.LegendaryBook.FistAndPalm.JiePo();
							case 40304:
								return new GameData.Domains.SpecialEffect.LegendaryBook.FistAndPalm.JueZhi();
							case 40305:
								return new GameData.Domains.SpecialEffect.LegendaryBook.FistAndPalm.ShouZhi();
							}
							break;
						}
					}
					else
					{
						switch (type)
						{
						case 40400:
							return new DuanXue();
						case 40401:
							return new GameData.Domains.SpecialEffect.LegendaryBook.Finger.ZhuanJie();
						case 40402:
							return new SiXue();
						case 40403:
							return new GameData.Domains.SpecialEffect.LegendaryBook.Finger.JiePo();
						case 40404:
							return new GameData.Domains.SpecialEffect.LegendaryBook.Finger.JueZhi();
						case 40405:
							return new GameData.Domains.SpecialEffect.LegendaryBook.Finger.ShouZhi();
						default:
							switch (type)
							{
							case 40500:
								return new BingZu();
							case 40501:
								return new GameData.Domains.SpecialEffect.LegendaryBook.Leg.ZhuanJie();
							case 40502:
								return new XianSha();
							case 40503:
								return new GameData.Domains.SpecialEffect.LegendaryBook.Leg.JiePo();
							case 40504:
								return new GameData.Domains.SpecialEffect.LegendaryBook.Leg.JueZhi();
							case 40505:
								return new GameData.Domains.SpecialEffect.LegendaryBook.Leg.ShouZhi();
							default:
								switch (type)
								{
								case 40600:
									return new AnSha();
								case 40601:
									return new GameData.Domains.SpecialEffect.LegendaryBook.Throw.ZhuanJie();
								case 40602:
									return new YuanSha();
								case 40603:
									return new GameData.Domains.SpecialEffect.LegendaryBook.Throw.JiePo();
								case 40604:
									return new GameData.Domains.SpecialEffect.LegendaryBook.Throw.JueZhi();
								case 40605:
									return new GameData.Domains.SpecialEffect.LegendaryBook.Throw.ShouZhi();
								}
								break;
							}
							break;
						}
					}
				}
				else if (type <= 41105)
				{
					if (type <= 40805)
					{
						switch (type)
						{
						case 40700:
							return new HuaXi();
						case 40701:
							return new GameData.Domains.SpecialEffect.LegendaryBook.Sword.ZhuanJie();
						case 40702:
							return new ShiSha();
						case 40703:
							return new GameData.Domains.SpecialEffect.LegendaryBook.Sword.JiePo();
						case 40704:
							return new GameData.Domains.SpecialEffect.LegendaryBook.Sword.JueZhi();
						case 40705:
							return new GameData.Domains.SpecialEffect.LegendaryBook.Sword.ShouZhi();
						default:
							switch (type)
							{
							case 40800:
								return new ZhenLie();
							case 40801:
								return new GameData.Domains.SpecialEffect.LegendaryBook.Blade.ZhuanJie();
							case 40802:
								return new PoSha();
							case 40803:
								return new GameData.Domains.SpecialEffect.LegendaryBook.Blade.JiePo();
							case 40804:
								return new GameData.Domains.SpecialEffect.LegendaryBook.Blade.JueZhi();
							case 40805:
								return new GameData.Domains.SpecialEffect.LegendaryBook.Blade.ShouZhi();
							}
							break;
						}
					}
					else
					{
						switch (type)
						{
						case 40900:
							return new ChuangZhen();
						case 40901:
							return new GameData.Domains.SpecialEffect.LegendaryBook.Polearm.ZhuanJie();
						case 40902:
							return new JueDou();
						case 40903:
							return new GameData.Domains.SpecialEffect.LegendaryBook.Polearm.JiePo();
						case 40904:
							return new GameData.Domains.SpecialEffect.LegendaryBook.Polearm.JueZhi();
						case 40905:
							return new GameData.Domains.SpecialEffect.LegendaryBook.Polearm.ShouZhi();
						default:
							switch (type)
							{
							case 41000:
								return new GuiJi();
							case 41001:
								return new GameData.Domains.SpecialEffect.LegendaryBook.Special.ZhuanJie();
							case 41002:
								return new QiShi();
							case 41003:
								return new GameData.Domains.SpecialEffect.LegendaryBook.Special.JiePo();
							case 41004:
								return new GameData.Domains.SpecialEffect.LegendaryBook.Special.JueZhi();
							case 41005:
								return new GameData.Domains.SpecialEffect.LegendaryBook.Special.ShouZhi();
							default:
								switch (type)
								{
								case 41100:
									return new YanSheng();
								case 41101:
									return new GameData.Domains.SpecialEffect.LegendaryBook.Whip.ZhuanJie();
								case 41102:
									return new JieQi();
								case 41103:
									return new GameData.Domains.SpecialEffect.LegendaryBook.Whip.JiePo();
								case 41104:
									return new GameData.Domains.SpecialEffect.LegendaryBook.Whip.JueZhi();
								case 41105:
									return new GameData.Domains.SpecialEffect.LegendaryBook.Whip.ShouZhi();
								}
								break;
							}
							break;
						}
					}
				}
				else if (type <= 41305)
				{
					switch (type)
					{
					case 41200:
						return new QiBian();
					case 41201:
						return new GameData.Domains.SpecialEffect.LegendaryBook.ControllableShot.ZhuanJie();
					case 41202:
						return new BianJie();
					case 41203:
						return new GameData.Domains.SpecialEffect.LegendaryBook.ControllableShot.JiePo();
					case 41204:
						return new GameData.Domains.SpecialEffect.LegendaryBook.ControllableShot.JueZhi();
					case 41205:
						return new GameData.Domains.SpecialEffect.LegendaryBook.ControllableShot.ShouZhi();
					default:
						switch (type)
						{
						case 41300:
							return new LuanXin();
						case 41301:
							return new GameData.Domains.SpecialEffect.LegendaryBook.CombatMusic.ZhuanJie();
						case 41302:
							return new KuangSheng();
						case 41303:
							return new GameData.Domains.SpecialEffect.LegendaryBook.CombatMusic.JiePo();
						case 41304:
							return new GameData.Domains.SpecialEffect.LegendaryBook.CombatMusic.JueZhi();
						case 41305:
							return new GameData.Domains.SpecialEffect.LegendaryBook.CombatMusic.ShouZhi();
						}
						break;
					}
				}
				else
				{
					switch (type)
					{
					case 41400:
						return new Neigong();
					case 41401:
						return new Posing();
					case 41402:
						return new Stunt();
					case 41403:
						return new FistAndPalm();
					case 41404:
						return new Finger();
					case 41405:
						return new Leg();
					case 41406:
						return new Throw();
					case 41407:
						return new Sword();
					case 41408:
						return new Blade();
					case 41409:
						return new Polearm();
					case 41410:
						return new Special();
					case 41411:
						return new Whip();
					case 41412:
						return new ControllableShot();
					case 41413:
						return new CombatMusic();
					default:
						if (type == 1000000)
						{
							return new AddPenetrateAndPenetrateResist();
						}
						if (type == 1000001)
						{
							return new AddMaxHealth();
						}
						break;
					}
				}
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(44, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Cannot find special effect subclass of type ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(type);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			return SpecialEffectBase.Invalid;
		}

		// Token: 0x0400083D RID: 2109
		public const int NoSectNeigong0 = 0;

		// Token: 0x0400083E RID: 2110
		public const int NoSectNeigong4 = 4;

		// Token: 0x0400083F RID: 2111
		public const int NoSectFistAndPalm0 = 100;

		// Token: 0x04000840 RID: 2112
		public const int NoSectAgile0 = 200;

		// Token: 0x04000841 RID: 2113
		public const int NoSectDefense0 = 300;

		// Token: 0x04000842 RID: 2114
		public const int ShaolinNeigong0 = 1000;

		// Token: 0x04000843 RID: 2115
		public const int ShaolinNeigong1 = 1001;

		// Token: 0x04000844 RID: 2116
		public const int ShaolinNeigong2 = 1002;

		// Token: 0x04000845 RID: 2117
		public const int ShaolinNeigong3 = 1003;

		// Token: 0x04000846 RID: 2118
		public const int ShaolinNeigong4 = 1004;

		// Token: 0x04000847 RID: 2119
		public const int ShaolinNeigong5 = 1005;

		// Token: 0x04000848 RID: 2120
		public const int ShaolinNeigong6 = 1006;

		// Token: 0x04000849 RID: 2121
		public const int ShaolinNeigong7 = 1007;

		// Token: 0x0400084A RID: 2122
		public const int ShaolinNeigong8 = 1008;

		// Token: 0x0400084B RID: 2123
		public const int ShaolinFistAndPalm0 = 1100;

		// Token: 0x0400084C RID: 2124
		public const int ShaolinFistAndPalm1 = 1101;

		// Token: 0x0400084D RID: 2125
		public const int ShaolinFistAndPalm2 = 1102;

		// Token: 0x0400084E RID: 2126
		public const int ShaolinFistAndPalm3 = 1103;

		// Token: 0x0400084F RID: 2127
		public const int ShaolinFistAndPalm4 = 1104;

		// Token: 0x04000850 RID: 2128
		public const int ShaolinFistAndPalm5 = 1105;

		// Token: 0x04000851 RID: 2129
		public const int ShaolinFistAndPalm6 = 1106;

		// Token: 0x04000852 RID: 2130
		public const int ShaolinFistAndPalm7 = 1107;

		// Token: 0x04000853 RID: 2131
		public const int ShaolinFinger0 = 1200;

		// Token: 0x04000854 RID: 2132
		public const int ShaolinFinger1 = 1201;

		// Token: 0x04000855 RID: 2133
		public const int ShaolinFinger2 = 1202;

		// Token: 0x04000856 RID: 2134
		public const int ShaolinFinger3 = 1203;

		// Token: 0x04000857 RID: 2135
		public const int ShaolinFinger4 = 1204;

		// Token: 0x04000858 RID: 2136
		public const int ShaolinFinger5 = 1205;

		// Token: 0x04000859 RID: 2137
		public const int ShaolinPolearm0 = 1300;

		// Token: 0x0400085A RID: 2138
		public const int ShaolinPolearm1 = 1301;

		// Token: 0x0400085B RID: 2139
		public const int ShaolinPolearm2 = 1302;

		// Token: 0x0400085C RID: 2140
		public const int ShaolinPolearm3 = 1303;

		// Token: 0x0400085D RID: 2141
		public const int ShaolinPolearm4 = 1304;

		// Token: 0x0400085E RID: 2142
		public const int ShaolinPolearm5 = 1305;

		// Token: 0x0400085F RID: 2143
		public const int ShaolinPolearm6 = 1306;

		// Token: 0x04000860 RID: 2144
		public const int ShaolinPolearm7 = 1307;

		// Token: 0x04000861 RID: 2145
		public const int ShaolinPolearm8 = 1308;

		// Token: 0x04000862 RID: 2146
		public const int ShaolinAgile0 = 1400;

		// Token: 0x04000863 RID: 2147
		public const int ShaolinAgile1 = 1401;

		// Token: 0x04000864 RID: 2148
		public const int ShaolinAgile2 = 1402;

		// Token: 0x04000865 RID: 2149
		public const int ShaolinAgile3 = 1403;

		// Token: 0x04000866 RID: 2150
		public const int ShaolinAgile4 = 1404;

		// Token: 0x04000867 RID: 2151
		public const int ShaolinAgile5 = 1405;

		// Token: 0x04000868 RID: 2152
		public const int ShaolinDefense1 = 1501;

		// Token: 0x04000869 RID: 2153
		public const int ShaolinDefense2 = 1502;

		// Token: 0x0400086A RID: 2154
		public const int ShaolinDefense4 = 1504;

		// Token: 0x0400086B RID: 2155
		public const int ShaolinDefense7 = 1507;

		// Token: 0x0400086C RID: 2156
		public const int ShaolinDefense8 = 1508;

		// Token: 0x0400086D RID: 2157
		public const int ShaolinAssist0 = 1600;

		// Token: 0x0400086E RID: 2158
		public const int ShaolinAssist3 = 1603;

		// Token: 0x0400086F RID: 2159
		public const int ShaolinAssist5 = 1605;

		// Token: 0x04000870 RID: 2160
		public const int ShaolinAssist6 = 1606;

		// Token: 0x04000871 RID: 2161
		public const int EmeiNeigong0 = 2000;

		// Token: 0x04000872 RID: 2162
		public const int EmeiNeigong1 = 2001;

		// Token: 0x04000873 RID: 2163
		public const int EmeiNeigong2 = 2002;

		// Token: 0x04000874 RID: 2164
		public const int EmeiNeigong3 = 2003;

		// Token: 0x04000875 RID: 2165
		public const int EmeiNeigong4 = 2004;

		// Token: 0x04000876 RID: 2166
		public const int EmeiNeigong5 = 2005;

		// Token: 0x04000877 RID: 2167
		public const int EmeiNeigong6 = 2006;

		// Token: 0x04000878 RID: 2168
		public const int EmeiNeigong7 = 2007;

		// Token: 0x04000879 RID: 2169
		public const int EmeiFistAndPalm0 = 2100;

		// Token: 0x0400087A RID: 2170
		public const int EmeiFistAndPalm1 = 2101;

		// Token: 0x0400087B RID: 2171
		public const int EmeiFistAndPalm2 = 2102;

		// Token: 0x0400087C RID: 2172
		public const int EmeiFistAndPalm3 = 2103;

		// Token: 0x0400087D RID: 2173
		public const int EmeiFistAndPalm4 = 2104;

		// Token: 0x0400087E RID: 2174
		public const int EmeiFistAndPalm5 = 2105;

		// Token: 0x0400087F RID: 2175
		public const int EmeiFistAndPalm6 = 2106;

		// Token: 0x04000880 RID: 2176
		public const int EmeiFinger0 = 2200;

		// Token: 0x04000881 RID: 2177
		public const int EmeiFinger1 = 2201;

		// Token: 0x04000882 RID: 2178
		public const int EmeiFinger2 = 2202;

		// Token: 0x04000883 RID: 2179
		public const int EmeiFinger3 = 2203;

		// Token: 0x04000884 RID: 2180
		public const int EmeiFinger4 = 2204;

		// Token: 0x04000885 RID: 2181
		public const int EmeiFinger5 = 2205;

		// Token: 0x04000886 RID: 2182
		public const int EmeiFinger6 = 2206;

		// Token: 0x04000887 RID: 2183
		public const int EmeiFinger7 = 2207;

		// Token: 0x04000888 RID: 2184
		public const int EmeiFinger8 = 2208;

		// Token: 0x04000889 RID: 2185
		public const int EmeiSword0 = 2300;

		// Token: 0x0400088A RID: 2186
		public const int EmeiSword1 = 2301;

		// Token: 0x0400088B RID: 2187
		public const int EmeiSword2 = 2302;

		// Token: 0x0400088C RID: 2188
		public const int EmeiSword3 = 2303;

		// Token: 0x0400088D RID: 2189
		public const int EmeiSword4 = 2304;

		// Token: 0x0400088E RID: 2190
		public const int EmeiSword5 = 2305;

		// Token: 0x0400088F RID: 2191
		public const int EmeiSword6 = 2306;

		// Token: 0x04000890 RID: 2192
		public const int EmeiSpecial0 = 2400;

		// Token: 0x04000891 RID: 2193
		public const int EmeiSpecial1 = 2401;

		// Token: 0x04000892 RID: 2194
		public const int EmeiSpecial2 = 2402;

		// Token: 0x04000893 RID: 2195
		public const int EmeiSpecial3 = 2403;

		// Token: 0x04000894 RID: 2196
		public const int EmeiSpecial4 = 2404;

		// Token: 0x04000895 RID: 2197
		public const int EmeiSpecial5 = 2405;

		// Token: 0x04000896 RID: 2198
		public const int EmeiSpecial6 = 2406;

		// Token: 0x04000897 RID: 2199
		public const int EmeiSpecial7 = 2407;

		// Token: 0x04000898 RID: 2200
		public const int EmeiAgile0 = 2500;

		// Token: 0x04000899 RID: 2201
		public const int EmeiAgile1 = 2501;

		// Token: 0x0400089A RID: 2202
		public const int EmeiAgile2 = 2502;

		// Token: 0x0400089B RID: 2203
		public const int EmeiAgile3 = 2503;

		// Token: 0x0400089C RID: 2204
		public const int EmeiAgile4 = 2504;

		// Token: 0x0400089D RID: 2205
		public const int EmeiAgile5 = 2505;

		// Token: 0x0400089E RID: 2206
		public const int EmeiAgile6 = 2506;

		// Token: 0x0400089F RID: 2207
		public const int EmeiDefense3 = 2603;

		// Token: 0x040008A0 RID: 2208
		public const int EmeiDefense6 = 2606;

		// Token: 0x040008A1 RID: 2209
		public const int EmeiDefense8 = 2608;

		// Token: 0x040008A2 RID: 2210
		public const int EmeiAssist0 = 2700;

		// Token: 0x040008A3 RID: 2211
		public const int EmeiAssist1 = 2701;

		// Token: 0x040008A4 RID: 2212
		public const int EmeiAssist2 = 2702;

		// Token: 0x040008A5 RID: 2213
		public const int EmeiAssist4 = 2704;

		// Token: 0x040008A6 RID: 2214
		public const int EmeiAssist5 = 2705;

		// Token: 0x040008A7 RID: 2215
		public const int EmeiAssist7 = 2707;

		// Token: 0x040008A8 RID: 2216
		public const int BaihuaNeigong0 = 3000;

		// Token: 0x040008A9 RID: 2217
		public const int BaihuaNeigong1 = 3001;

		// Token: 0x040008AA RID: 2218
		public const int BaihuaNeigong2 = 3002;

		// Token: 0x040008AB RID: 2219
		public const int BaihuaNeigong3 = 3003;

		// Token: 0x040008AC RID: 2220
		public const int BaihuaNeigong4 = 3004;

		// Token: 0x040008AD RID: 2221
		public const int BaihuaFinger0 = 3100;

		// Token: 0x040008AE RID: 2222
		public const int BaihuaFinger1 = 3101;

		// Token: 0x040008AF RID: 2223
		public const int BaihuaFinger2 = 3102;

		// Token: 0x040008B0 RID: 2224
		public const int BaihuaFinger3 = 3103;

		// Token: 0x040008B1 RID: 2225
		public const int BaihuaFinger4 = 3104;

		// Token: 0x040008B2 RID: 2226
		public const int BaihuaFinger5 = 3105;

		// Token: 0x040008B3 RID: 2227
		public const int BaihuaFinger6 = 3106;

		// Token: 0x040008B4 RID: 2228
		public const int BaihuaFinger7 = 3107;

		// Token: 0x040008B5 RID: 2229
		public const int BaihuaFinger8 = 3108;

		// Token: 0x040008B6 RID: 2230
		public const int BaihuaShot0 = 3200;

		// Token: 0x040008B7 RID: 2231
		public const int BaihuaShot1 = 3201;

		// Token: 0x040008B8 RID: 2232
		public const int BaihuaShot2 = 3202;

		// Token: 0x040008B9 RID: 2233
		public const int BaihuaShot3 = 3203;

		// Token: 0x040008BA RID: 2234
		public const int BaihuaShot4 = 3204;

		// Token: 0x040008BB RID: 2235
		public const int BaihuaShot5 = 3205;

		// Token: 0x040008BC RID: 2236
		public const int BaihuaShot6 = 3206;

		// Token: 0x040008BD RID: 2237
		public const int BaihuaShot7 = 3207;

		// Token: 0x040008BE RID: 2238
		public const int BaihuaShot8 = 3208;

		// Token: 0x040008BF RID: 2239
		public const int BaihuaMusic0 = 3300;

		// Token: 0x040008C0 RID: 2240
		public const int BaihuaMusic1 = 3301;

		// Token: 0x040008C1 RID: 2241
		public const int BaihuaMusic2 = 3302;

		// Token: 0x040008C2 RID: 2242
		public const int BaihuaMusic3 = 3303;

		// Token: 0x040008C3 RID: 2243
		public const int BaihuaMusic4 = 3304;

		// Token: 0x040008C4 RID: 2244
		public const int BaihuaMusic5 = 3305;

		// Token: 0x040008C5 RID: 2245
		public const int BaihuaMusic6 = 3306;

		// Token: 0x040008C6 RID: 2246
		public const int BaihuaAgile0 = 3400;

		// Token: 0x040008C7 RID: 2247
		public const int BaihuaAgile1 = 3401;

		// Token: 0x040008C8 RID: 2248
		public const int BaihuaAgile2 = 3402;

		// Token: 0x040008C9 RID: 2249
		public const int BaihuaAgile3 = 3403;

		// Token: 0x040008CA RID: 2250
		public const int BaihuaAgile4 = 3404;

		// Token: 0x040008CB RID: 2251
		public const int BaihuaAgile5 = 3405;

		// Token: 0x040008CC RID: 2252
		public const int BaihuaAgile6 = 3406;

		// Token: 0x040008CD RID: 2253
		public const int BaihuaAgile7 = 3407;

		// Token: 0x040008CE RID: 2254
		public const int BaihuaDefense4 = 3504;

		// Token: 0x040008CF RID: 2255
		public const int BaihuaDefense5 = 3505;

		// Token: 0x040008D0 RID: 2256
		public const int BaihuaDefense7 = 3507;

		// Token: 0x040008D1 RID: 2257
		public const int BaihuaDefense8 = 3508;

		// Token: 0x040008D2 RID: 2258
		public const int BaihuaAssist0 = 3600;

		// Token: 0x040008D3 RID: 2259
		public const int BaihuaAssist1 = 3601;

		// Token: 0x040008D4 RID: 2260
		public const int BaihuaAssist2 = 3602;

		// Token: 0x040008D5 RID: 2261
		public const int BaihuaAssist3 = 3603;

		// Token: 0x040008D6 RID: 2262
		public const int BaihuaAssist6 = 3606;

		// Token: 0x040008D7 RID: 2263
		public const int WudangNeigong0 = 4000;

		// Token: 0x040008D8 RID: 2264
		public const int WudangNeigong1 = 4001;

		// Token: 0x040008D9 RID: 2265
		public const int WudangNeigong2 = 4002;

		// Token: 0x040008DA RID: 2266
		public const int WudangNeigong3 = 4003;

		// Token: 0x040008DB RID: 2267
		public const int WudangNeigong4 = 4004;

		// Token: 0x040008DC RID: 2268
		public const int WudangNeigong5 = 4005;

		// Token: 0x040008DD RID: 2269
		public const int WudangNeigong6 = 4006;

		// Token: 0x040008DE RID: 2270
		public const int WudangNeigong7 = 4007;

		// Token: 0x040008DF RID: 2271
		public const int WudangNeigong8 = 4008;

		// Token: 0x040008E0 RID: 2272
		public const int WudangFistAndPalm0 = 4100;

		// Token: 0x040008E1 RID: 2273
		public const int WudangFistAndPalm1 = 4101;

		// Token: 0x040008E2 RID: 2274
		public const int WudangFistAndPalm2 = 4102;

		// Token: 0x040008E3 RID: 2275
		public const int WudangFistAndPalm3 = 4103;

		// Token: 0x040008E4 RID: 2276
		public const int WudangFistAndPalm4 = 4104;

		// Token: 0x040008E5 RID: 2277
		public const int WudangFistAndPalm5 = 4105;

		// Token: 0x040008E6 RID: 2278
		public const int WudangFistAndPalm6 = 4106;

		// Token: 0x040008E7 RID: 2279
		public const int WudangFistAndPalm7 = 4107;

		// Token: 0x040008E8 RID: 2280
		public const int WudangFistAndPalm8 = 4108;

		// Token: 0x040008E9 RID: 2281
		public const int WudangSword0 = 4200;

		// Token: 0x040008EA RID: 2282
		public const int WudangSword1 = 4201;

		// Token: 0x040008EB RID: 2283
		public const int WudangSword2 = 4202;

		// Token: 0x040008EC RID: 2284
		public const int WudangSword3 = 4203;

		// Token: 0x040008ED RID: 2285
		public const int WudangSword4 = 4204;

		// Token: 0x040008EE RID: 2286
		public const int WudangSword5 = 4205;

		// Token: 0x040008EF RID: 2287
		public const int WudangSword6 = 4206;

		// Token: 0x040008F0 RID: 2288
		public const int WudangSword7 = 4207;

		// Token: 0x040008F1 RID: 2289
		public const int WudangSword8 = 4208;

		// Token: 0x040008F2 RID: 2290
		public const int WudangWhip0 = 4300;

		// Token: 0x040008F3 RID: 2291
		public const int WudangWhip1 = 4301;

		// Token: 0x040008F4 RID: 2292
		public const int WudangWhip2 = 4302;

		// Token: 0x040008F5 RID: 2293
		public const int WudangWhip3 = 4303;

		// Token: 0x040008F6 RID: 2294
		public const int WudangWhip4 = 4304;

		// Token: 0x040008F7 RID: 2295
		public const int WudangWhip5 = 4305;

		// Token: 0x040008F8 RID: 2296
		public const int WudangWhip6 = 4306;

		// Token: 0x040008F9 RID: 2297
		public const int WudangWhip7 = 4307;

		// Token: 0x040008FA RID: 2298
		public const int WudangAgile0 = 4400;

		// Token: 0x040008FB RID: 2299
		public const int WudangAgile1 = 4401;

		// Token: 0x040008FC RID: 2300
		public const int WudangAgile2 = 4402;

		// Token: 0x040008FD RID: 2301
		public const int WudangAgile3 = 4403;

		// Token: 0x040008FE RID: 2302
		public const int WudangAgile4 = 4404;

		// Token: 0x040008FF RID: 2303
		public const int WudangAgile5 = 4405;

		// Token: 0x04000900 RID: 2304
		public const int WudangAgile6 = 4406;

		// Token: 0x04000901 RID: 2305
		public const int WudangDefense0 = 4500;

		// Token: 0x04000902 RID: 2306
		public const int WudangDefense2 = 4502;

		// Token: 0x04000903 RID: 2307
		public const int WudangDefense4 = 4504;

		// Token: 0x04000904 RID: 2308
		public const int WudangDefense6 = 4506;

		// Token: 0x04000905 RID: 2309
		public const int WudangAssist1 = 4601;

		// Token: 0x04000906 RID: 2310
		public const int WudangAssist3 = 4603;

		// Token: 0x04000907 RID: 2311
		public const int WudangAssist5 = 4605;

		// Token: 0x04000908 RID: 2312
		public const int WudangAssist7 = 4607;

		// Token: 0x04000909 RID: 2313
		public const int YuanshanNeigong0 = 5000;

		// Token: 0x0400090A RID: 2314
		public const int YuanshanNeigong1 = 5001;

		// Token: 0x0400090B RID: 2315
		public const int YuanshanNeigong2 = 5002;

		// Token: 0x0400090C RID: 2316
		public const int YuanshanNeigong3 = 5003;

		// Token: 0x0400090D RID: 2317
		public const int YuanshanNeigong4 = 5004;

		// Token: 0x0400090E RID: 2318
		public const int YuanshanLeg0 = 5100;

		// Token: 0x0400090F RID: 2319
		public const int YuanshanLeg1 = 5101;

		// Token: 0x04000910 RID: 2320
		public const int YuanshanLeg2 = 5102;

		// Token: 0x04000911 RID: 2321
		public const int YuanshanLeg3 = 5103;

		// Token: 0x04000912 RID: 2322
		public const int YuanshanLeg4 = 5104;

		// Token: 0x04000913 RID: 2323
		public const int YuanshanLeg5 = 5105;

		// Token: 0x04000914 RID: 2324
		public const int YuanshanLeg6 = 5106;

		// Token: 0x04000915 RID: 2325
		public const int YuanshanLeg7 = 5107;

		// Token: 0x04000916 RID: 2326
		public const int YuanshanLeg8 = 5108;

		// Token: 0x04000917 RID: 2327
		public const int YuanshanSword0 = 5200;

		// Token: 0x04000918 RID: 2328
		public const int YuanshanSword1 = 5201;

		// Token: 0x04000919 RID: 2329
		public const int YuanshanSword2 = 5202;

		// Token: 0x0400091A RID: 2330
		public const int YuanshanSword3 = 5203;

		// Token: 0x0400091B RID: 2331
		public const int YuanshanSword4 = 5204;

		// Token: 0x0400091C RID: 2332
		public const int YuanshanSword5 = 5205;

		// Token: 0x0400091D RID: 2333
		public const int YuanshanSword6 = 5206;

		// Token: 0x0400091E RID: 2334
		public const int YuanshanSword7 = 5207;

		// Token: 0x0400091F RID: 2335
		public const int YuanshanBlade0 = 5300;

		// Token: 0x04000920 RID: 2336
		public const int YuanshanBlade1 = 5301;

		// Token: 0x04000921 RID: 2337
		public const int YuanshanBlade2 = 5302;

		// Token: 0x04000922 RID: 2338
		public const int YuanshanBlade3 = 5303;

		// Token: 0x04000923 RID: 2339
		public const int YuanshanBlade4 = 5304;

		// Token: 0x04000924 RID: 2340
		public const int YuanshanBlade5 = 5305;

		// Token: 0x04000925 RID: 2341
		public const int YuanshanBlade6 = 5306;

		// Token: 0x04000926 RID: 2342
		public const int YuanshanBlade7 = 5307;

		// Token: 0x04000927 RID: 2343
		public const int YuanshanAgile0 = 5400;

		// Token: 0x04000928 RID: 2344
		public const int YuanshanAgile1 = 5401;

		// Token: 0x04000929 RID: 2345
		public const int YuanshanAgile2 = 5402;

		// Token: 0x0400092A RID: 2346
		public const int YuanshanAgile3 = 5403;

		// Token: 0x0400092B RID: 2347
		public const int YuanshanDefense0 = 5500;

		// Token: 0x0400092C RID: 2348
		public const int YuanshanDefense3 = 5503;

		// Token: 0x0400092D RID: 2349
		public const int YuanshanDefense6 = 5506;

		// Token: 0x0400092E RID: 2350
		public const int YuanshanAssist1 = 5601;

		// Token: 0x0400092F RID: 2351
		public const int YuanshanAssist2 = 5602;

		// Token: 0x04000930 RID: 2352
		public const int YuanshanAssist4 = 5604;

		// Token: 0x04000931 RID: 2353
		public const int YuanshanAssist5 = 5605;

		// Token: 0x04000932 RID: 2354
		public const int ShixiangNeigong0 = 6000;

		// Token: 0x04000933 RID: 2355
		public const int ShixiangNeigong1 = 6001;

		// Token: 0x04000934 RID: 2356
		public const int ShixiangNeigong2 = 6002;

		// Token: 0x04000935 RID: 2357
		public const int ShixiangNeigong3 = 6003;

		// Token: 0x04000936 RID: 2358
		public const int ShixiangNeigong4 = 6004;

		// Token: 0x04000937 RID: 2359
		public const int ShixiangFistAndPalm0 = 6100;

		// Token: 0x04000938 RID: 2360
		public const int ShixiangFistAndPalm1 = 6101;

		// Token: 0x04000939 RID: 2361
		public const int ShixiangFistAndPalm2 = 6102;

		// Token: 0x0400093A RID: 2362
		public const int ShixiangFistAndPalm3 = 6103;

		// Token: 0x0400093B RID: 2363
		public const int ShixiangFistAndPalm4 = 6104;

		// Token: 0x0400093C RID: 2364
		public const int ShixiangFistAndPalm5 = 6105;

		// Token: 0x0400093D RID: 2365
		public const int ShixiangFistAndPalm6 = 6106;

		// Token: 0x0400093E RID: 2366
		public const int ShixiangFistAndPalm7 = 6107;

		// Token: 0x0400093F RID: 2367
		public const int ShixiangFistAndPalm8 = 6108;

		// Token: 0x04000940 RID: 2368
		public const int ShixiangBlade0 = 6200;

		// Token: 0x04000941 RID: 2369
		public const int ShixiangBlade1 = 6201;

		// Token: 0x04000942 RID: 2370
		public const int ShixiangBlade2 = 6202;

		// Token: 0x04000943 RID: 2371
		public const int ShixiangBlade3 = 6203;

		// Token: 0x04000944 RID: 2372
		public const int ShixiangBlade4 = 6204;

		// Token: 0x04000945 RID: 2373
		public const int ShixiangBlade5 = 6205;

		// Token: 0x04000946 RID: 2374
		public const int ShixiangBlade6 = 6206;

		// Token: 0x04000947 RID: 2375
		public const int ShixiangBlade7 = 6207;

		// Token: 0x04000948 RID: 2376
		public const int ShixiangBlade8 = 6208;

		// Token: 0x04000949 RID: 2377
		public const int ShixiangPolearm0 = 6300;

		// Token: 0x0400094A RID: 2378
		public const int ShixiangPolearm1 = 6301;

		// Token: 0x0400094B RID: 2379
		public const int ShixiangPolearm2 = 6302;

		// Token: 0x0400094C RID: 2380
		public const int ShixiangPolearm3 = 6303;

		// Token: 0x0400094D RID: 2381
		public const int ShixiangPolearm4 = 6304;

		// Token: 0x0400094E RID: 2382
		public const int ShixiangPolearm5 = 6305;

		// Token: 0x0400094F RID: 2383
		public const int ShixiangPolearm6 = 6306;

		// Token: 0x04000950 RID: 2384
		public const int ShixiangPolearm7 = 6307;

		// Token: 0x04000951 RID: 2385
		public const int ShixiangPolearm8 = 6308;

		// Token: 0x04000952 RID: 2386
		public const int ShixiangAgile0 = 6400;

		// Token: 0x04000953 RID: 2387
		public const int ShixiangAgile1 = 6401;

		// Token: 0x04000954 RID: 2388
		public const int ShixiangAgile2 = 6402;

		// Token: 0x04000955 RID: 2389
		public const int ShixiangAgile3 = 6403;

		// Token: 0x04000956 RID: 2390
		public const int ShixiangDefense0 = 6500;

		// Token: 0x04000957 RID: 2391
		public const int ShixiangDefense2 = 6502;

		// Token: 0x04000958 RID: 2392
		public const int ShixiangDefense5 = 6505;

		// Token: 0x04000959 RID: 2393
		public const int ShixiangAssist1 = 6601;

		// Token: 0x0400095A RID: 2394
		public const int ShixiangAssist3 = 6603;

		// Token: 0x0400095B RID: 2395
		public const int ShixiangAssist4 = 6604;

		// Token: 0x0400095C RID: 2396
		public const int RanshanNeigong0 = 7000;

		// Token: 0x0400095D RID: 2397
		public const int RanshanNeigong1 = 7001;

		// Token: 0x0400095E RID: 2398
		public const int RanshanNeigong2 = 7002;

		// Token: 0x0400095F RID: 2399
		public const int RanshanNeigong3 = 7003;

		// Token: 0x04000960 RID: 2400
		public const int RanshanNeigong4 = 7004;

		// Token: 0x04000961 RID: 2401
		public const int RanshanNeigong5 = 7005;

		// Token: 0x04000962 RID: 2402
		public const int RanshanNeigong6 = 7006;

		// Token: 0x04000963 RID: 2403
		public const int RanshanNeigong7 = 7007;

		// Token: 0x04000964 RID: 2404
		public const int RanshanNeigong8 = 7008;

		// Token: 0x04000965 RID: 2405
		public const int RanshanFinger0 = 7100;

		// Token: 0x04000966 RID: 2406
		public const int RanshanFinger1 = 7101;

		// Token: 0x04000967 RID: 2407
		public const int RanshanFinger2 = 7102;

		// Token: 0x04000968 RID: 2408
		public const int RanshanFinger3 = 7103;

		// Token: 0x04000969 RID: 2409
		public const int RanshanFinger4 = 7104;

		// Token: 0x0400096A RID: 2410
		public const int RanshanFinger5 = 7105;

		// Token: 0x0400096B RID: 2411
		public const int RanshanFinger6 = 7106;

		// Token: 0x0400096C RID: 2412
		public const int RanshanSword0 = 7200;

		// Token: 0x0400096D RID: 2413
		public const int RanshanSword1 = 7201;

		// Token: 0x0400096E RID: 2414
		public const int RanshanSword2 = 7202;

		// Token: 0x0400096F RID: 2415
		public const int RanshanSword3 = 7203;

		// Token: 0x04000970 RID: 2416
		public const int RanshanSword4 = 7204;

		// Token: 0x04000971 RID: 2417
		public const int RanshanSword5 = 7205;

		// Token: 0x04000972 RID: 2418
		public const int RanshanSword6 = 7206;

		// Token: 0x04000973 RID: 2419
		public const int RanshanSword7 = 7207;

		// Token: 0x04000974 RID: 2420
		public const int RanshanSword8 = 7208;

		// Token: 0x04000975 RID: 2421
		public const int RanshanSpecial0 = 7300;

		// Token: 0x04000976 RID: 2422
		public const int RanshanSpecial1 = 7301;

		// Token: 0x04000977 RID: 2423
		public const int RanshanSpecial2 = 7302;

		// Token: 0x04000978 RID: 2424
		public const int RanshanSpecial3 = 7303;

		// Token: 0x04000979 RID: 2425
		public const int RanshanSpecial4 = 7304;

		// Token: 0x0400097A RID: 2426
		public const int RanshanSpecial5 = 7305;

		// Token: 0x0400097B RID: 2427
		public const int RanshanSpecial6 = 7306;

		// Token: 0x0400097C RID: 2428
		public const int RanshanSpecial7 = 7307;

		// Token: 0x0400097D RID: 2429
		public const int RanshanSpecial8 = 7308;

		// Token: 0x0400097E RID: 2430
		public const int RanshanAgile0 = 7400;

		// Token: 0x0400097F RID: 2431
		public const int RanshanAgile1 = 7401;

		// Token: 0x04000980 RID: 2432
		public const int RanshanAgile2 = 7402;

		// Token: 0x04000981 RID: 2433
		public const int RanshanAgile3 = 7403;

		// Token: 0x04000982 RID: 2434
		public const int RanshanAgile4 = 7404;

		// Token: 0x04000983 RID: 2435
		public const int RanshanAgile5 = 7405;

		// Token: 0x04000984 RID: 2436
		public const int RanshanAgile6 = 7406;

		// Token: 0x04000985 RID: 2437
		public const int RanshanDefense0 = 7500;

		// Token: 0x04000986 RID: 2438
		public const int RanshanDefense3 = 7503;

		// Token: 0x04000987 RID: 2439
		public const int RanshanDefense5 = 7505;

		// Token: 0x04000988 RID: 2440
		public const int RanshanDefense6 = 7506;

		// Token: 0x04000989 RID: 2441
		public const int RanshanDefense7 = 7507;

		// Token: 0x0400098A RID: 2442
		public const int RanshanAssist1 = 7601;

		// Token: 0x0400098B RID: 2443
		public const int RanshanAssist2 = 7602;

		// Token: 0x0400098C RID: 2444
		public const int RanshanAssist4 = 7604;

		// Token: 0x0400098D RID: 2445
		public const int XuannvNeigong0 = 8000;

		// Token: 0x0400098E RID: 2446
		public const int XuannvNeigong1 = 8001;

		// Token: 0x0400098F RID: 2447
		public const int XuannvNeigong2 = 8002;

		// Token: 0x04000990 RID: 2448
		public const int XuannvNeigong3 = 8003;

		// Token: 0x04000991 RID: 2449
		public const int XuannvNeigong4 = 8004;

		// Token: 0x04000992 RID: 2450
		public const int XuannvNeigong5 = 8005;

		// Token: 0x04000993 RID: 2451
		public const int XuannvNeigong6 = 8006;

		// Token: 0x04000994 RID: 2452
		public const int XuannvNeigong7 = 8007;

		// Token: 0x04000995 RID: 2453
		public const int XuannvNeigong8 = 8008;

		// Token: 0x04000996 RID: 2454
		public const int XuannvFistAndPalm0 = 8100;

		// Token: 0x04000997 RID: 2455
		public const int XuannvFistAndPalm1 = 8101;

		// Token: 0x04000998 RID: 2456
		public const int XuannvFistAndPalm2 = 8102;

		// Token: 0x04000999 RID: 2457
		public const int XuannvFistAndPalm3 = 8103;

		// Token: 0x0400099A RID: 2458
		public const int XuannvFistAndPalm4 = 8104;

		// Token: 0x0400099B RID: 2459
		public const int XuannvFistAndPalm5 = 8105;

		// Token: 0x0400099C RID: 2460
		public const int XuannvFistAndPalm6 = 8106;

		// Token: 0x0400099D RID: 2461
		public const int XuannvFinger0 = 8200;

		// Token: 0x0400099E RID: 2462
		public const int XuannvFinger1 = 8201;

		// Token: 0x0400099F RID: 2463
		public const int XuannvFinger2 = 8202;

		// Token: 0x040009A0 RID: 2464
		public const int XuannvFinger3 = 8203;

		// Token: 0x040009A1 RID: 2465
		public const int XuannvFinger4 = 8204;

		// Token: 0x040009A2 RID: 2466
		public const int XuannvFinger5 = 8205;

		// Token: 0x040009A3 RID: 2467
		public const int XuannvFinger6 = 8206;

		// Token: 0x040009A4 RID: 2468
		public const int XuannvFinger7 = 8207;

		// Token: 0x040009A5 RID: 2469
		public const int XuannvMusic0 = 8300;

		// Token: 0x040009A6 RID: 2470
		public const int XuannvMusic1 = 8301;

		// Token: 0x040009A7 RID: 2471
		public const int XuannvMusic2 = 8302;

		// Token: 0x040009A8 RID: 2472
		public const int XuannvMusic3 = 8303;

		// Token: 0x040009A9 RID: 2473
		public const int XuannvMusic4 = 8304;

		// Token: 0x040009AA RID: 2474
		public const int XuannvMusic5 = 8305;

		// Token: 0x040009AB RID: 2475
		public const int XuannvMusic6 = 8306;

		// Token: 0x040009AC RID: 2476
		public const int XuannvMusic7 = 8307;

		// Token: 0x040009AD RID: 2477
		public const int XuannvMusic8 = 8308;

		// Token: 0x040009AE RID: 2478
		public const int XuannvAgile0 = 8400;

		// Token: 0x040009AF RID: 2479
		public const int XuannvAgile1 = 8401;

		// Token: 0x040009B0 RID: 2480
		public const int XuannvAgile2 = 8402;

		// Token: 0x040009B1 RID: 2481
		public const int XuannvAgile3 = 8403;

		// Token: 0x040009B2 RID: 2482
		public const int XuannvAgile4 = 8404;

		// Token: 0x040009B3 RID: 2483
		public const int XuannvAgile5 = 8405;

		// Token: 0x040009B4 RID: 2484
		public const int XuannvAgile6 = 8406;

		// Token: 0x040009B5 RID: 2485
		public const int XuannvAgile7 = 8407;

		// Token: 0x040009B6 RID: 2486
		public const int XuannvAgile8 = 8408;

		// Token: 0x040009B7 RID: 2487
		public const int XuannvDefense1 = 8501;

		// Token: 0x040009B8 RID: 2488
		public const int XuannvDefense5 = 8505;

		// Token: 0x040009B9 RID: 2489
		public const int XuannvDefense6 = 8506;

		// Token: 0x040009BA RID: 2490
		public const int XuannvAssist0 = 8600;

		// Token: 0x040009BB RID: 2491
		public const int XuannvAssist2 = 8602;

		// Token: 0x040009BC RID: 2492
		public const int XuannvAssist3 = 8603;

		// Token: 0x040009BD RID: 2493
		public const int XuannvAssist4 = 8604;

		// Token: 0x040009BE RID: 2494
		public const int ZhujianNeigong0 = 9000;

		// Token: 0x040009BF RID: 2495
		public const int ZhujianNeigong1 = 9001;

		// Token: 0x040009C0 RID: 2496
		public const int ZhujianNeigong2 = 9002;

		// Token: 0x040009C1 RID: 2497
		public const int ZhujianNeigong3 = 9003;

		// Token: 0x040009C2 RID: 2498
		public const int ZhujianNeigong4 = 9004;

		// Token: 0x040009C3 RID: 2499
		public const int ZhujianNeigong5 = 9005;

		// Token: 0x040009C4 RID: 2500
		public const int ZhujianSword0 = 9100;

		// Token: 0x040009C5 RID: 2501
		public const int ZhujianSword1 = 9101;

		// Token: 0x040009C6 RID: 2502
		public const int ZhujianSword2 = 9102;

		// Token: 0x040009C7 RID: 2503
		public const int ZhujianSword3 = 9103;

		// Token: 0x040009C8 RID: 2504
		public const int ZhujianSword4 = 9104;

		// Token: 0x040009C9 RID: 2505
		public const int ZhujianSword5 = 9105;

		// Token: 0x040009CA RID: 2506
		public const int ZhujianSword6 = 9106;

		// Token: 0x040009CB RID: 2507
		public const int ZhujianSword7 = 9107;

		// Token: 0x040009CC RID: 2508
		public const int ZhujianBlade0 = 9200;

		// Token: 0x040009CD RID: 2509
		public const int ZhujianBlade1 = 9201;

		// Token: 0x040009CE RID: 2510
		public const int ZhujianBlade2 = 9202;

		// Token: 0x040009CF RID: 2511
		public const int ZhujianBlade3 = 9203;

		// Token: 0x040009D0 RID: 2512
		public const int ZhujianBlade4 = 9204;

		// Token: 0x040009D1 RID: 2513
		public const int ZhujianBlade5 = 9205;

		// Token: 0x040009D2 RID: 2514
		public const int ZhujianBlade6 = 9206;

		// Token: 0x040009D3 RID: 2515
		public const int ZhujianBlade7 = 9207;

		// Token: 0x040009D4 RID: 2516
		public const int ZhujianPolearm0 = 9300;

		// Token: 0x040009D5 RID: 2517
		public const int ZhujianPolearm1 = 9301;

		// Token: 0x040009D6 RID: 2518
		public const int ZhujianPolearm2 = 9302;

		// Token: 0x040009D7 RID: 2519
		public const int ZhujianPolearm3 = 9303;

		// Token: 0x040009D8 RID: 2520
		public const int ZhujianPolearm4 = 9304;

		// Token: 0x040009D9 RID: 2521
		public const int ZhujianPolearm5 = 9305;

		// Token: 0x040009DA RID: 2522
		public const int ZhujianPolearm6 = 9306;

		// Token: 0x040009DB RID: 2523
		public const int ZhujianPolearm7 = 9307;

		// Token: 0x040009DC RID: 2524
		public const int ZhujianShot0 = 9400;

		// Token: 0x040009DD RID: 2525
		public const int ZhujianShot1 = 9401;

		// Token: 0x040009DE RID: 2526
		public const int ZhujianShot2 = 9402;

		// Token: 0x040009DF RID: 2527
		public const int ZhujianShot3 = 9403;

		// Token: 0x040009E0 RID: 2528
		public const int ZhujianShot4 = 9404;

		// Token: 0x040009E1 RID: 2529
		public const int ZhujianShot5 = 9405;

		// Token: 0x040009E2 RID: 2530
		public const int ZhujianShot6 = 9406;

		// Token: 0x040009E3 RID: 2531
		public const int ZhujianShot7 = 9407;

		// Token: 0x040009E4 RID: 2532
		public const int ZhujianShot8 = 9408;

		// Token: 0x040009E5 RID: 2533
		public const int ZhujianAgile0 = 9500;

		// Token: 0x040009E6 RID: 2534
		public const int ZhujianAgile1 = 9501;

		// Token: 0x040009E7 RID: 2535
		public const int ZhujianAgile2 = 9502;

		// Token: 0x040009E8 RID: 2536
		public const int ZhujianAgile3 = 9503;

		// Token: 0x040009E9 RID: 2537
		public const int ZhujianAgile4 = 9504;

		// Token: 0x040009EA RID: 2538
		public const int ZhujianAgile5 = 9505;

		// Token: 0x040009EB RID: 2539
		public const int ZhujianAgile6 = 9506;

		// Token: 0x040009EC RID: 2540
		public const int ZhujianDefense1 = 9701;

		// Token: 0x040009ED RID: 2541
		public const int ZhujianDefense4 = 9704;

		// Token: 0x040009EE RID: 2542
		public const int ZhujianDefense5 = 9705;

		// Token: 0x040009EF RID: 2543
		public const int ZhujianAssist0 = 9700;

		// Token: 0x040009F0 RID: 2544
		public const int ZhujianAssist2 = 9702;

		// Token: 0x040009F1 RID: 2545
		public const int ZhujianAssist3 = 9703;

		// Token: 0x040009F2 RID: 2546
		public const int ZhujianAssist6 = 9706;

		// Token: 0x040009F3 RID: 2547
		public const int KongsangNeigong0 = 10000;

		// Token: 0x040009F4 RID: 2548
		public const int KongsangNeigong1 = 10001;

		// Token: 0x040009F5 RID: 2549
		public const int KongsangNeigong2 = 10002;

		// Token: 0x040009F6 RID: 2550
		public const int KongsangNeigong3 = 10003;

		// Token: 0x040009F7 RID: 2551
		public const int KongsangNeigong4 = 10004;

		// Token: 0x040009F8 RID: 2552
		public const int KongsangFistAndPalm0 = 10100;

		// Token: 0x040009F9 RID: 2553
		public const int KongsangFistAndPalm1 = 10101;

		// Token: 0x040009FA RID: 2554
		public const int KongsangFistAndPalm2 = 10102;

		// Token: 0x040009FB RID: 2555
		public const int KongsangFistAndPalm3 = 10103;

		// Token: 0x040009FC RID: 2556
		public const int KongsangFistAndPalm4 = 10104;

		// Token: 0x040009FD RID: 2557
		public const int KongsangFistAndPalm5 = 10105;

		// Token: 0x040009FE RID: 2558
		public const int KongsangFinger0 = 10200;

		// Token: 0x040009FF RID: 2559
		public const int KongsangFinger1 = 10201;

		// Token: 0x04000A00 RID: 2560
		public const int KongsangFinger2 = 10202;

		// Token: 0x04000A01 RID: 2561
		public const int KongsangFinger3 = 10203;

		// Token: 0x04000A02 RID: 2562
		public const int KongsangFinger4 = 10204;

		// Token: 0x04000A03 RID: 2563
		public const int KongsangFinger5 = 10205;

		// Token: 0x04000A04 RID: 2564
		public const int KongsangLeg0 = 10300;

		// Token: 0x04000A05 RID: 2565
		public const int KongsangLeg1 = 10301;

		// Token: 0x04000A06 RID: 2566
		public const int KongsangLeg2 = 10302;

		// Token: 0x04000A07 RID: 2567
		public const int KongsangLeg3 = 10303;

		// Token: 0x04000A08 RID: 2568
		public const int KongsangLeg4 = 10304;

		// Token: 0x04000A09 RID: 2569
		public const int KongsangLeg5 = 10305;

		// Token: 0x04000A0A RID: 2570
		public const int KongsangLeg6 = 10306;

		// Token: 0x04000A0B RID: 2571
		public const int KongsangLeg7 = 10307;

		// Token: 0x04000A0C RID: 2572
		public const int KongsangThrow0 = 10400;

		// Token: 0x04000A0D RID: 2573
		public const int KongsangThrow1 = 10401;

		// Token: 0x04000A0E RID: 2574
		public const int KongsangThrow2 = 10402;

		// Token: 0x04000A0F RID: 2575
		public const int KongsangThrow3 = 10403;

		// Token: 0x04000A10 RID: 2576
		public const int KongsangThrow4 = 10404;

		// Token: 0x04000A11 RID: 2577
		public const int KongsangThrow5 = 10405;

		// Token: 0x04000A12 RID: 2578
		public const int KongsangThrow6 = 10406;

		// Token: 0x04000A13 RID: 2579
		public const int KongsangThrow7 = 10407;

		// Token: 0x04000A14 RID: 2580
		public const int KongsangThrow8 = 10408;

		// Token: 0x04000A15 RID: 2581
		public const int KongsangAgile0 = 10500;

		// Token: 0x04000A16 RID: 2582
		public const int KongsangAgile1 = 10501;

		// Token: 0x04000A17 RID: 2583
		public const int KongsangAgile2 = 10502;

		// Token: 0x04000A18 RID: 2584
		public const int KongsangAgile3 = 10503;

		// Token: 0x04000A19 RID: 2585
		public const int KongsangAgile4 = 10504;

		// Token: 0x04000A1A RID: 2586
		public const int KongsangDefense3 = 10603;

		// Token: 0x04000A1B RID: 2587
		public const int KongsangDefense7 = 10607;

		// Token: 0x04000A1C RID: 2588
		public const int KongsangAssist0 = 10700;

		// Token: 0x04000A1D RID: 2589
		public const int KongsangAssist1 = 10701;

		// Token: 0x04000A1E RID: 2590
		public const int KongsangAssist2 = 10702;

		// Token: 0x04000A1F RID: 2591
		public const int KongsangAssist4 = 10704;

		// Token: 0x04000A20 RID: 2592
		public const int KongsangAssist5 = 10705;

		// Token: 0x04000A21 RID: 2593
		public const int KongsangAssist6 = 10706;

		// Token: 0x04000A22 RID: 2594
		public const int KongsangAssist8 = 10708;

		// Token: 0x04000A23 RID: 2595
		public const int JingangNeigong0 = 11000;

		// Token: 0x04000A24 RID: 2596
		public const int JingangNeigong1 = 11001;

		// Token: 0x04000A25 RID: 2597
		public const int JingangNeigong2 = 11002;

		// Token: 0x04000A26 RID: 2598
		public const int JingangNeigong3 = 11003;

		// Token: 0x04000A27 RID: 2599
		public const int JingangNeigong4 = 11004;

		// Token: 0x04000A28 RID: 2600
		public const int JingangNeigong5 = 11005;

		// Token: 0x04000A29 RID: 2601
		public const int JingangNeigong6 = 11006;

		// Token: 0x04000A2A RID: 2602
		public const int JingangNeigong7 = 11007;

		// Token: 0x04000A2B RID: 2603
		public const int JingangNeigong8 = 11008;

		// Token: 0x04000A2C RID: 2604
		public const int JingangFistAndPalm0 = 11100;

		// Token: 0x04000A2D RID: 2605
		public const int JingangFistAndPalm1 = 11101;

		// Token: 0x04000A2E RID: 2606
		public const int JingangFistAndPalm2 = 11102;

		// Token: 0x04000A2F RID: 2607
		public const int JingangFistAndPalm3 = 11103;

		// Token: 0x04000A30 RID: 2608
		public const int JingangFistAndPalm4 = 11104;

		// Token: 0x04000A31 RID: 2609
		public const int JingangFistAndPalm5 = 11105;

		// Token: 0x04000A32 RID: 2610
		public const int JingangFistAndPalm6 = 11106;

		// Token: 0x04000A33 RID: 2611
		public const int JingangFistAndPalm7 = 11107;

		// Token: 0x04000A34 RID: 2612
		public const int JingangBlade0 = 11200;

		// Token: 0x04000A35 RID: 2613
		public const int JingangBlade1 = 11201;

		// Token: 0x04000A36 RID: 2614
		public const int JingangBlade2 = 11202;

		// Token: 0x04000A37 RID: 2615
		public const int JingangBlade3 = 11203;

		// Token: 0x04000A38 RID: 2616
		public const int JingangBlade4 = 11204;

		// Token: 0x04000A39 RID: 2617
		public const int JingangBlade5 = 11205;

		// Token: 0x04000A3A RID: 2618
		public const int JingangBlade6 = 11206;

		// Token: 0x04000A3B RID: 2619
		public const int JingangBlade7 = 11207;

		// Token: 0x04000A3C RID: 2620
		public const int JingangBlade8 = 11208;

		// Token: 0x04000A3D RID: 2621
		public const int JingangSpecial0 = 11300;

		// Token: 0x04000A3E RID: 2622
		public const int JingangSpecial1 = 11301;

		// Token: 0x04000A3F RID: 2623
		public const int JingangSpecial2 = 11302;

		// Token: 0x04000A40 RID: 2624
		public const int JingangSpecial3 = 11303;

		// Token: 0x04000A41 RID: 2625
		public const int JingangSpecial4 = 11304;

		// Token: 0x04000A42 RID: 2626
		public const int JingangSpecial5 = 11305;

		// Token: 0x04000A43 RID: 2627
		public const int JingangSpecial6 = 11306;

		// Token: 0x04000A44 RID: 2628
		public const int JingangSpecial7 = 11307;

		// Token: 0x04000A45 RID: 2629
		public const int JingangPestle1 = 11401;

		// Token: 0x04000A46 RID: 2630
		public const int JingangPestle3 = 11403;

		// Token: 0x04000A47 RID: 2631
		public const int JingangPestle5 = 11405;

		// Token: 0x04000A48 RID: 2632
		public const int JingangAgile0 = 11500;

		// Token: 0x04000A49 RID: 2633
		public const int JingangAgile1 = 11501;

		// Token: 0x04000A4A RID: 2634
		public const int JingangAgile2 = 11502;

		// Token: 0x04000A4B RID: 2635
		public const int JingangAgile3 = 11503;

		// Token: 0x04000A4C RID: 2636
		public const int JingangDefense1 = 11601;

		// Token: 0x04000A4D RID: 2637
		public const int JingangDefense2 = 11602;

		// Token: 0x04000A4E RID: 2638
		public const int JingangDefense4 = 11604;

		// Token: 0x04000A4F RID: 2639
		public const int JingangDefense5 = 11605;

		// Token: 0x04000A50 RID: 2640
		public const int JingangDefense7 = 11607;

		// Token: 0x04000A51 RID: 2641
		public const int JingangAssist0 = 11700;

		// Token: 0x04000A52 RID: 2642
		public const int JingangAssist3 = 11703;

		// Token: 0x04000A53 RID: 2643
		public const int JingangAssist6 = 11706;

		// Token: 0x04000A54 RID: 2644
		public const int WuxianNeigong0 = 12000;

		// Token: 0x04000A55 RID: 2645
		public const int WuxianNeigong1 = 12001;

		// Token: 0x04000A56 RID: 2646
		public const int WuxianNeigong2 = 12002;

		// Token: 0x04000A57 RID: 2647
		public const int WuxianNeigong3 = 12003;

		// Token: 0x04000A58 RID: 2648
		public const int WuxianNeigong4 = 12004;

		// Token: 0x04000A59 RID: 2649
		public const int WuxianNeigong5 = 12005;

		// Token: 0x04000A5A RID: 2650
		public const int WuxianNeigong6 = 12006;

		// Token: 0x04000A5B RID: 2651
		public const int WuxianFistAndPalm0 = 12100;

		// Token: 0x04000A5C RID: 2652
		public const int WuxianFistAndPalm1 = 12101;

		// Token: 0x04000A5D RID: 2653
		public const int WuxianFistAndPalm2 = 12102;

		// Token: 0x04000A5E RID: 2654
		public const int WuxianFistAndPalm3 = 12103;

		// Token: 0x04000A5F RID: 2655
		public const int WuxianFistAndPalm4 = 12104;

		// Token: 0x04000A60 RID: 2656
		public const int WuxianFistAndPalm5 = 12105;

		// Token: 0x04000A61 RID: 2657
		public const int WuxianFistAndPalm6 = 12106;

		// Token: 0x04000A62 RID: 2658
		public const int WuxianFistAndPalm7 = 12107;

		// Token: 0x04000A63 RID: 2659
		public const int WuxianFinger0 = 12200;

		// Token: 0x04000A64 RID: 2660
		public const int WuxianFinger1 = 12201;

		// Token: 0x04000A65 RID: 2661
		public const int WuxianFinger2 = 12202;

		// Token: 0x04000A66 RID: 2662
		public const int WuxianFinger3 = 12203;

		// Token: 0x04000A67 RID: 2663
		public const int WuxianFinger4 = 12204;

		// Token: 0x04000A68 RID: 2664
		public const int WuxianFinger5 = 12205;

		// Token: 0x04000A69 RID: 2665
		public const int WuxianFinger6 = 12206;

		// Token: 0x04000A6A RID: 2666
		public const int WuxianFinger7 = 12207;

		// Token: 0x04000A6B RID: 2667
		public const int WuxianFinger8 = 12208;

		// Token: 0x04000A6C RID: 2668
		public const int WuxianSword0 = 12300;

		// Token: 0x04000A6D RID: 2669
		public const int WuxianSword1 = 12301;

		// Token: 0x04000A6E RID: 2670
		public const int WuxianSword2 = 12302;

		// Token: 0x04000A6F RID: 2671
		public const int WuxianSword3 = 12303;

		// Token: 0x04000A70 RID: 2672
		public const int WuxianSword4 = 12304;

		// Token: 0x04000A71 RID: 2673
		public const int WuxianSword5 = 12305;

		// Token: 0x04000A72 RID: 2674
		public const int WuxianSword6 = 12306;

		// Token: 0x04000A73 RID: 2675
		public const int WuxianWhip0 = 12400;

		// Token: 0x04000A74 RID: 2676
		public const int WuxianWhip1 = 12401;

		// Token: 0x04000A75 RID: 2677
		public const int WuxianWhip2 = 12402;

		// Token: 0x04000A76 RID: 2678
		public const int WuxianWhip3 = 12403;

		// Token: 0x04000A77 RID: 2679
		public const int WuxianWhip4 = 12404;

		// Token: 0x04000A78 RID: 2680
		public const int WuxianWhip5 = 12405;

		// Token: 0x04000A79 RID: 2681
		public const int WuxianWhip6 = 12406;

		// Token: 0x04000A7A RID: 2682
		public const int WuxianWhip7 = 12407;

		// Token: 0x04000A7B RID: 2683
		public const int WuxianWhip8 = 12408;

		// Token: 0x04000A7C RID: 2684
		public const int WuXianRedEyeGrowingGood1 = 12500;

		// Token: 0x04000A7D RID: 2685
		public const int WuXianRedEyeGrowingGood2 = 12501;

		// Token: 0x04000A7E RID: 2686
		public const int WuXianRedEyeGrowingBad1 = 12502;

		// Token: 0x04000A7F RID: 2687
		public const int WuXianRedEyeGrowingBad2 = 12503;

		// Token: 0x04000A80 RID: 2688
		public const int WuXianRedEyeGrown = 12504;

		// Token: 0x04000A81 RID: 2689
		public const int WuXianForestSpiritGrowingGood1 = 12505;

		// Token: 0x04000A82 RID: 2690
		public const int WuXianForestSpiritGrowingGood2 = 12506;

		// Token: 0x04000A83 RID: 2691
		public const int WuXianForestSpiritGrowingBad1 = 12507;

		// Token: 0x04000A84 RID: 2692
		public const int WuXianForestSpiritGrowingBad2 = 12508;

		// Token: 0x04000A85 RID: 2693
		public const int WuXianForestSpiritGrown = 12509;

		// Token: 0x04000A86 RID: 2694
		public const int WuXianBlackBloodGrowingGood1 = 12510;

		// Token: 0x04000A87 RID: 2695
		public const int WuXianBlackBloodGrowingGood2 = 12511;

		// Token: 0x04000A88 RID: 2696
		public const int WuXianBlackBloodGrowingBad1 = 12512;

		// Token: 0x04000A89 RID: 2697
		public const int WuXianBlackBloodGrowingBad2 = 12513;

		// Token: 0x04000A8A RID: 2698
		public const int WuXianBlackBloodGrown = 12514;

		// Token: 0x04000A8B RID: 2699
		public const int WuXianDevilInsideGrowingGood1 = 12515;

		// Token: 0x04000A8C RID: 2700
		public const int WuXianDevilInsideGrowingGood2 = 12516;

		// Token: 0x04000A8D RID: 2701
		public const int WuXianDevilInsideGrowingBad1 = 12517;

		// Token: 0x04000A8E RID: 2702
		public const int WuXianDevilInsideGrowingBad2 = 12518;

		// Token: 0x04000A8F RID: 2703
		public const int WuXianDevilInsideGrown = 12519;

		// Token: 0x04000A90 RID: 2704
		public const int WuXianCorpseWormGrowingGood1 = 12520;

		// Token: 0x04000A91 RID: 2705
		public const int WuXianCorpseWormGrowingGood2 = 12521;

		// Token: 0x04000A92 RID: 2706
		public const int WuXianCorpseWormGrowingBad1 = 12522;

		// Token: 0x04000A93 RID: 2707
		public const int WuXianCorpseWormGrowingBad2 = 12523;

		// Token: 0x04000A94 RID: 2708
		public const int WuXianCorpseWormGrown = 12524;

		// Token: 0x04000A95 RID: 2709
		public const int WuXianIceSilkwormGrowingGood1 = 12525;

		// Token: 0x04000A96 RID: 2710
		public const int WuXianIceSilkwormGrowingGood2 = 12526;

		// Token: 0x04000A97 RID: 2711
		public const int WuXianIceSilkwormGrowingBad1 = 12527;

		// Token: 0x04000A98 RID: 2712
		public const int WuXianIceSilkwormGrowingBad2 = 12528;

		// Token: 0x04000A99 RID: 2713
		public const int WuXianIceSilkwormGrown = 12529;

		// Token: 0x04000A9A RID: 2714
		public const int WuXianGoldenSilkwormGrowingGood1 = 12530;

		// Token: 0x04000A9B RID: 2715
		public const int WuXianGoldenSilkwormGrowingGood2 = 12531;

		// Token: 0x04000A9C RID: 2716
		public const int WuXianGoldenSilkwormGrowingBad1 = 12532;

		// Token: 0x04000A9D RID: 2717
		public const int WuXianGoldenSilkwormGrowingBad2 = 12533;

		// Token: 0x04000A9E RID: 2718
		public const int WuXianGoldenSilkwormGrown = 12534;

		// Token: 0x04000A9F RID: 2719
		public const int WuXianAzureMarrowGrowingGood1 = 12535;

		// Token: 0x04000AA0 RID: 2720
		public const int WuXianAzureMarrowGrowingGood2 = 12536;

		// Token: 0x04000AA1 RID: 2721
		public const int WuXianAzureMarrowGrowingBad1 = 12537;

		// Token: 0x04000AA2 RID: 2722
		public const int WuXianAzureMarrowGrowingBad2 = 12538;

		// Token: 0x04000AA3 RID: 2723
		public const int WuXianAzureMarrowGrown = 12539;

		// Token: 0x04000AA4 RID: 2724
		public const int WuXianRedEyeKing = 12540;

		// Token: 0x04000AA5 RID: 2725
		public const int WuXianForestSpiritKing = 12541;

		// Token: 0x04000AA6 RID: 2726
		public const int WuXianBlackBloodKing = 12542;

		// Token: 0x04000AA7 RID: 2727
		public const int WuXianDevilInsideKing = 12543;

		// Token: 0x04000AA8 RID: 2728
		public const int WuXianCorpseWormKing = 12544;

		// Token: 0x04000AA9 RID: 2729
		public const int WuXianIceSilkwormKing = 12545;

		// Token: 0x04000AAA RID: 2730
		public const int WuXianGoldenSilkwormKing = 12546;

		// Token: 0x04000AAB RID: 2731
		public const int WuXianAzureMarrowKing = 12547;

		// Token: 0x04000AAC RID: 2732
		public const int WuxianAgile0 = 12600;

		// Token: 0x04000AAD RID: 2733
		public const int WuxianAgile1 = 12601;

		// Token: 0x04000AAE RID: 2734
		public const int WuxianAgile2 = 12602;

		// Token: 0x04000AAF RID: 2735
		public const int WuxianAgile3 = 12603;

		// Token: 0x04000AB0 RID: 2736
		public const int WuxianAgile4 = 12604;

		// Token: 0x04000AB1 RID: 2737
		public const int WuxianAgile5 = 12605;

		// Token: 0x04000AB2 RID: 2738
		public const int WuxianAgile6 = 12606;

		// Token: 0x04000AB3 RID: 2739
		public const int WuxianDefense1 = 12701;

		// Token: 0x04000AB4 RID: 2740
		public const int WuxianDefense3 = 12703;

		// Token: 0x04000AB5 RID: 2741
		public const int WuxianDefense4 = 12704;

		// Token: 0x04000AB6 RID: 2742
		public const int WuxianDefense5 = 12705;

		// Token: 0x04000AB7 RID: 2743
		public const int WuxianDefense7 = 12707;

		// Token: 0x04000AB8 RID: 2744
		public const int WuxianAssist0 = 12800;

		// Token: 0x04000AB9 RID: 2745
		public const int WuxianAssist2 = 12802;

		// Token: 0x04000ABA RID: 2746
		public const int WuxianAssist6 = 12806;

		// Token: 0x04000ABB RID: 2747
		public const int WuxianAssist8 = 12808;

		// Token: 0x04000ABC RID: 2748
		public const int JieqingNeigong0 = 13000;

		// Token: 0x04000ABD RID: 2749
		public const int JieqingNeigong1 = 13001;

		// Token: 0x04000ABE RID: 2750
		public const int JieqingNeigong2 = 13002;

		// Token: 0x04000ABF RID: 2751
		public const int JieqingNeigong3 = 13003;

		// Token: 0x04000AC0 RID: 2752
		public const int JieqingNeigong4 = 13004;

		// Token: 0x04000AC1 RID: 2753
		public const int JieqingNeigong5 = 13005;

		// Token: 0x04000AC2 RID: 2754
		public const int JieqingNeigong6 = 13006;

		// Token: 0x04000AC3 RID: 2755
		public const int JieqingFinger0 = 13100;

		// Token: 0x04000AC4 RID: 2756
		public const int JieqingFinger1 = 13101;

		// Token: 0x04000AC5 RID: 2757
		public const int JieqingFinger2 = 13102;

		// Token: 0x04000AC6 RID: 2758
		public const int JieqingFinger3 = 13103;

		// Token: 0x04000AC7 RID: 2759
		public const int JieqingFinger4 = 13104;

		// Token: 0x04000AC8 RID: 2760
		public const int JieqingFinger5 = 13105;

		// Token: 0x04000AC9 RID: 2761
		public const int JieqingFinger6 = 13106;

		// Token: 0x04000ACA RID: 2762
		public const int JieqingFinger7 = 13107;

		// Token: 0x04000ACB RID: 2763
		public const int JieqingFinger8 = 13108;

		// Token: 0x04000ACC RID: 2764
		public const int JieqingSword0 = 13200;

		// Token: 0x04000ACD RID: 2765
		public const int JieqingSword1 = 13201;

		// Token: 0x04000ACE RID: 2766
		public const int JieqingSword2 = 13202;

		// Token: 0x04000ACF RID: 2767
		public const int JieqingSword3 = 13203;

		// Token: 0x04000AD0 RID: 2768
		public const int JieqingSword4 = 13204;

		// Token: 0x04000AD1 RID: 2769
		public const int JieqingSword5 = 13205;

		// Token: 0x04000AD2 RID: 2770
		public const int JieqingSword6 = 13206;

		// Token: 0x04000AD3 RID: 2771
		public const int JieqingSword7 = 13207;

		// Token: 0x04000AD4 RID: 2772
		public const int JieqingThrow0 = 13300;

		// Token: 0x04000AD5 RID: 2773
		public const int JieqingThrow1 = 13301;

		// Token: 0x04000AD6 RID: 2774
		public const int JieqingThrow2 = 13302;

		// Token: 0x04000AD7 RID: 2775
		public const int JieqingThrow3 = 13303;

		// Token: 0x04000AD8 RID: 2776
		public const int JieqingThrow4 = 13304;

		// Token: 0x04000AD9 RID: 2777
		public const int JieqingThrow5 = 13305;

		// Token: 0x04000ADA RID: 2778
		public const int JieqingThrow6 = 13306;

		// Token: 0x04000ADB RID: 2779
		public const int JieqingThrow7 = 13307;

		// Token: 0x04000ADC RID: 2780
		public const int JieqingThrow8 = 13308;

		// Token: 0x04000ADD RID: 2781
		public const int JieqingAgile0 = 13400;

		// Token: 0x04000ADE RID: 2782
		public const int JieqingAgile1 = 13401;

		// Token: 0x04000ADF RID: 2783
		public const int JieqingAgile2 = 13402;

		// Token: 0x04000AE0 RID: 2784
		public const int JieqingAgile3 = 13403;

		// Token: 0x04000AE1 RID: 2785
		public const int JieqingAgile4 = 13404;

		// Token: 0x04000AE2 RID: 2786
		public const int JieqingAgile5 = 13405;

		// Token: 0x04000AE3 RID: 2787
		public const int JieqingAgile6 = 13406;

		// Token: 0x04000AE4 RID: 2788
		public const int JieqingAgile7 = 13407;

		// Token: 0x04000AE5 RID: 2789
		public const int JieqingAgile8 = 13408;

		// Token: 0x04000AE6 RID: 2790
		public const int JieqingDefense1 = 13501;

		// Token: 0x04000AE7 RID: 2791
		public const int JieqingDefense2 = 13502;

		// Token: 0x04000AE8 RID: 2792
		public const int JieqingDefense3 = 13503;

		// Token: 0x04000AE9 RID: 2793
		public const int JieqingDefense6 = 13506;

		// Token: 0x04000AEA RID: 2794
		public const int JieqingAssist0 = 13600;

		// Token: 0x04000AEB RID: 2795
		public const int JieqingAssist4 = 13604;

		// Token: 0x04000AEC RID: 2796
		public const int JieqingAssist5 = 13605;

		// Token: 0x04000AED RID: 2797
		public const int FulongNeigong0 = 14000;

		// Token: 0x04000AEE RID: 2798
		public const int FulongNeigong1 = 14001;

		// Token: 0x04000AEF RID: 2799
		public const int FulongNeigong2 = 14002;

		// Token: 0x04000AF0 RID: 2800
		public const int FulongNeigong3 = 14003;

		// Token: 0x04000AF1 RID: 2801
		public const int FulongNeigong4 = 14004;

		// Token: 0x04000AF2 RID: 2802
		public const int FulongNeigong5 = 14005;

		// Token: 0x04000AF3 RID: 2803
		public const int FulongNeigong6 = 14006;

		// Token: 0x04000AF4 RID: 2804
		public const int FulongNeigong7 = 14007;

		// Token: 0x04000AF5 RID: 2805
		public const int FulongFistAndPalm0 = 14100;

		// Token: 0x04000AF6 RID: 2806
		public const int FulongFistAndPalm1 = 14101;

		// Token: 0x04000AF7 RID: 2807
		public const int FulongFistAndPalm2 = 14102;

		// Token: 0x04000AF8 RID: 2808
		public const int FulongFistAndPalm3 = 14103;

		// Token: 0x04000AF9 RID: 2809
		public const int FulongFistAndPalm4 = 14104;

		// Token: 0x04000AFA RID: 2810
		public const int FulongFistAndPalm5 = 14105;

		// Token: 0x04000AFB RID: 2811
		public const int FulongFistAndPalm6 = 14106;

		// Token: 0x04000AFC RID: 2812
		public const int FulongFistAndPalm7 = 14107;

		// Token: 0x04000AFD RID: 2813
		public const int FulongFistAndPalm8 = 14108;

		// Token: 0x04000AFE RID: 2814
		public const int FulongBlade0 = 14200;

		// Token: 0x04000AFF RID: 2815
		public const int FulongBlade1 = 14201;

		// Token: 0x04000B00 RID: 2816
		public const int FulongBlade2 = 14202;

		// Token: 0x04000B01 RID: 2817
		public const int FulongBlade3 = 14203;

		// Token: 0x04000B02 RID: 2818
		public const int FulongBlade4 = 14204;

		// Token: 0x04000B03 RID: 2819
		public const int FulongBlade5 = 14205;

		// Token: 0x04000B04 RID: 2820
		public const int FulongBlade6 = 14206;

		// Token: 0x04000B05 RID: 2821
		public const int FulongBlade7 = 14207;

		// Token: 0x04000B06 RID: 2822
		public const int FulongThrow0 = 14300;

		// Token: 0x04000B07 RID: 2823
		public const int FulongThrow1 = 14301;

		// Token: 0x04000B08 RID: 2824
		public const int FulongThrow2 = 14302;

		// Token: 0x04000B09 RID: 2825
		public const int FulongThrow3 = 14303;

		// Token: 0x04000B0A RID: 2826
		public const int FulongThrow4 = 14304;

		// Token: 0x04000B0B RID: 2827
		public const int FulongThrow5 = 14305;

		// Token: 0x04000B0C RID: 2828
		public const int FulongAgile0 = 14400;

		// Token: 0x04000B0D RID: 2829
		public const int FulongAgile1 = 14401;

		// Token: 0x04000B0E RID: 2830
		public const int FulongAgile2 = 14402;

		// Token: 0x04000B0F RID: 2831
		public const int FulongAgile3 = 14403;

		// Token: 0x04000B10 RID: 2832
		public const int FulongAgile4 = 14404;

		// Token: 0x04000B11 RID: 2833
		public const int FulongAgile5 = 14405;

		// Token: 0x04000B12 RID: 2834
		public const int FulongDefense1 = 14501;

		// Token: 0x04000B13 RID: 2835
		public const int FulongDefense2 = 14502;

		// Token: 0x04000B14 RID: 2836
		public const int FulongDefense4 = 14504;

		// Token: 0x04000B15 RID: 2837
		public const int FulongDefense7 = 14507;

		// Token: 0x04000B16 RID: 2838
		public const int FulongAssist0 = 14600;

		// Token: 0x04000B17 RID: 2839
		public const int FulongAssist3 = 14603;

		// Token: 0x04000B18 RID: 2840
		public const int FulongAssist5 = 14605;

		// Token: 0x04000B19 RID: 2841
		public const int FulongAssist6 = 14606;

		// Token: 0x04000B1A RID: 2842
		public const int XuehouNeigong0 = 15000;

		// Token: 0x04000B1B RID: 2843
		public const int XuehouNeigong1 = 15001;

		// Token: 0x04000B1C RID: 2844
		public const int XuehouNeigong2 = 15002;

		// Token: 0x04000B1D RID: 2845
		public const int XuehouNeigong3 = 15003;

		// Token: 0x04000B1E RID: 2846
		public const int XuehouNeigong4 = 15004;

		// Token: 0x04000B1F RID: 2847
		public const int XuehouNeigong5 = 15005;

		// Token: 0x04000B20 RID: 2848
		public const int XuehouNeigong6 = 15006;

		// Token: 0x04000B21 RID: 2849
		public const int XuehouNeigong7 = 15007;

		// Token: 0x04000B22 RID: 2850
		public const int XuehouNeigong8 = 15008;

		// Token: 0x04000B23 RID: 2851
		public const int XuehouFistAndPalm0 = 15100;

		// Token: 0x04000B24 RID: 2852
		public const int XuehouFistAndPalm1 = 15101;

		// Token: 0x04000B25 RID: 2853
		public const int XuehouFistAndPalm2 = 15102;

		// Token: 0x04000B26 RID: 2854
		public const int XuehouFistAndPalm3 = 15103;

		// Token: 0x04000B27 RID: 2855
		public const int XuehouFistAndPalm4 = 15104;

		// Token: 0x04000B28 RID: 2856
		public const int XuehouFistAndPalm5 = 15105;

		// Token: 0x04000B29 RID: 2857
		public const int XuehouFistAndPalm6 = 15106;

		// Token: 0x04000B2A RID: 2858
		public const int XuehouFistAndPalm7 = 15107;

		// Token: 0x04000B2B RID: 2859
		public const int XuehouFinger0 = 15200;

		// Token: 0x04000B2C RID: 2860
		public const int XuehouFinger1 = 15201;

		// Token: 0x04000B2D RID: 2861
		public const int XuehouFinger2 = 15202;

		// Token: 0x04000B2E RID: 2862
		public const int XuehouFinger3 = 15203;

		// Token: 0x04000B2F RID: 2863
		public const int XuehouFinger4 = 15204;

		// Token: 0x04000B30 RID: 2864
		public const int XuehouFinger5 = 15205;

		// Token: 0x04000B31 RID: 2865
		public const int XuehouFinger6 = 15206;

		// Token: 0x04000B32 RID: 2866
		public const int XuehouLeg0 = 15300;

		// Token: 0x04000B33 RID: 2867
		public const int XuehouLeg1 = 15301;

		// Token: 0x04000B34 RID: 2868
		public const int XuehouLeg2 = 15302;

		// Token: 0x04000B35 RID: 2869
		public const int XuehouLeg3 = 15303;

		// Token: 0x04000B36 RID: 2870
		public const int XuehouLeg4 = 15304;

		// Token: 0x04000B37 RID: 2871
		public const int XuehouLeg5 = 15305;

		// Token: 0x04000B38 RID: 2872
		public const int XuehouLeg6 = 15306;

		// Token: 0x04000B39 RID: 2873
		public const int XuehouLeg7 = 15307;

		// Token: 0x04000B3A RID: 2874
		public const int XuehouThrow0 = 15400;

		// Token: 0x04000B3B RID: 2875
		public const int XuehouThrow1 = 15401;

		// Token: 0x04000B3C RID: 2876
		public const int XuehouThrow2 = 15402;

		// Token: 0x04000B3D RID: 2877
		public const int XuehouThrow3 = 15403;

		// Token: 0x04000B3E RID: 2878
		public const int XuehouThrow4 = 15404;

		// Token: 0x04000B3F RID: 2879
		public const int XuehouThrow5 = 15405;

		// Token: 0x04000B40 RID: 2880
		public const int XuehouThrow6 = 15406;

		// Token: 0x04000B41 RID: 2881
		public const int XuehouChestCrash = 15500;

		// Token: 0x04000B42 RID: 2882
		public const int XuehouChestHurt = 15501;

		// Token: 0x04000B43 RID: 2883
		public const int XuehouBellyCrash = 15502;

		// Token: 0x04000B44 RID: 2884
		public const int XuehouBellyHurt = 15503;

		// Token: 0x04000B45 RID: 2885
		public const int XuehouHeadCrash = 15504;

		// Token: 0x04000B46 RID: 2886
		public const int XuehouHeadHurt = 15505;

		// Token: 0x04000B47 RID: 2887
		public const int XuehouHandCrash = 15506;

		// Token: 0x04000B48 RID: 2888
		public const int XuehouHandHurt = 15507;

		// Token: 0x04000B49 RID: 2889
		public const int XuehouLegCrash = 15508;

		// Token: 0x04000B4A RID: 2890
		public const int XuehouLegHurt = 15509;

		// Token: 0x04000B4B RID: 2891
		public const int XuehouAgile0 = 15600;

		// Token: 0x04000B4C RID: 2892
		public const int XuehouAgile1 = 15601;

		// Token: 0x04000B4D RID: 2893
		public const int XuehouAgile2 = 15602;

		// Token: 0x04000B4E RID: 2894
		public const int XuehouAgile3 = 15603;

		// Token: 0x04000B4F RID: 2895
		public const int XuehouAgile4 = 15604;

		// Token: 0x04000B50 RID: 2896
		public const int XuehouDefense0 = 15700;

		// Token: 0x04000B51 RID: 2897
		public const int XuehouDefense2 = 15702;

		// Token: 0x04000B52 RID: 2898
		public const int XuehouDefense4 = 15704;

		// Token: 0x04000B53 RID: 2899
		public const int XuehouDefense7 = 15707;

		// Token: 0x04000B54 RID: 2900
		public const int XuehouAssist1 = 15801;

		// Token: 0x04000B55 RID: 2901
		public const int XuehouAssist3 = 15803;

		// Token: 0x04000B56 RID: 2902
		public const int XuehouAssist5 = 15805;

		// Token: 0x04000B57 RID: 2903
		public const int XuehouAssist6 = 15806;

		// Token: 0x04000B58 RID: 2904
		public const int XuehouAssist8 = 15808;

		// Token: 0x04000B59 RID: 2905
		public const int XiangshuRandomEnemyNeigong0 = 16000;

		// Token: 0x04000B5A RID: 2906
		public const int XiangshuRandomEnemyNeigong1 = 16001;

		// Token: 0x04000B5B RID: 2907
		public const int XiangshuRandomEnemyNeigong2 = 16002;

		// Token: 0x04000B5C RID: 2908
		public const int XiangshuRandomEnemyNeigong3 = 16003;

		// Token: 0x04000B5D RID: 2909
		public const int XiangshuRandomEnemyNeigong4 = 16004;

		// Token: 0x04000B5E RID: 2910
		public const int XiangshuRandomEnemyNeigong5 = 16005;

		// Token: 0x04000B5F RID: 2911
		public const int XiangshuRandomEnemyNeigong6 = 16006;

		// Token: 0x04000B60 RID: 2912
		public const int XiangshuRandomEnemyNeigong7 = 16007;

		// Token: 0x04000B61 RID: 2913
		public const int XiangshuRandomEnemyNeigong8 = 16008;

		// Token: 0x04000B62 RID: 2914
		public const int XiangshuBossNeigong0 = 16100;

		// Token: 0x04000B63 RID: 2915
		public const int XiangshuBossNeigong1 = 16101;

		// Token: 0x04000B64 RID: 2916
		public const int XiangshuBossNeigong2 = 16102;

		// Token: 0x04000B65 RID: 2917
		public const int XiangshuBossNeigong3 = 16103;

		// Token: 0x04000B66 RID: 2918
		public const int XiangshuBossNeigong4 = 16104;

		// Token: 0x04000B67 RID: 2919
		public const int XiangshuBossNeigong5 = 16105;

		// Token: 0x04000B68 RID: 2920
		public const int XiangshuBossNeigong6 = 16106;

		// Token: 0x04000B69 RID: 2921
		public const int XiangshuBossNeigong7 = 16107;

		// Token: 0x04000B6A RID: 2922
		public const int XiangshuBossNeigong8 = 16108;

		// Token: 0x04000B6B RID: 2923
		public const int XiangshuBossNeigong9 = 16109;

		// Token: 0x04000B6C RID: 2924
		public const int XiangshuBossNeigong10 = 16110;

		// Token: 0x04000B6D RID: 2925
		public const int XiangshuBossNeigong11 = 16111;

		// Token: 0x04000B6E RID: 2926
		public const int XiangshuBossNeigong12 = 16112;

		// Token: 0x04000B6F RID: 2927
		public const int XiangshuBossNeigong13 = 16113;

		// Token: 0x04000B70 RID: 2928
		public const int XiangshuBossAgile0 = 16200;

		// Token: 0x04000B71 RID: 2929
		public const int XiangshuBossAgile1 = 16201;

		// Token: 0x04000B72 RID: 2930
		public const int XiangshuBossAgile2 = 16202;

		// Token: 0x04000B73 RID: 2931
		public const int XiangshuBossAgile3 = 16203;

		// Token: 0x04000B74 RID: 2932
		public const int XiangshuBossAgile4 = 16204;

		// Token: 0x04000B75 RID: 2933
		public const int XiangshuBossAgile5 = 16205;

		// Token: 0x04000B76 RID: 2934
		public const int XiangshuBossAgile6 = 16206;

		// Token: 0x04000B77 RID: 2935
		public const int XiangshuBossAgile7 = 16207;

		// Token: 0x04000B78 RID: 2936
		public const int XiangshuBossAgile8 = 16208;

		// Token: 0x04000B79 RID: 2937
		public const int XiangshuBossAgile9 = 16209;

		// Token: 0x04000B7A RID: 2938
		public const int XiangshuBossAgile10 = 16210;

		// Token: 0x04000B7B RID: 2939
		public const int XiangshuBossAgile11 = 16211;

		// Token: 0x04000B7C RID: 2940
		public const int XiangshuBossAgile12 = 16212;

		// Token: 0x04000B7D RID: 2941
		public const int XiangshuBossAgile13 = 16213;

		// Token: 0x04000B7E RID: 2942
		public const int XiangshuBossDefense0 = 16300;

		// Token: 0x04000B7F RID: 2943
		public const int XiangshuBossDefense1 = 16301;

		// Token: 0x04000B80 RID: 2944
		public const int XiangshuBossDefense2 = 16302;

		// Token: 0x04000B81 RID: 2945
		public const int XiangshuBossDefense3 = 16303;

		// Token: 0x04000B82 RID: 2946
		public const int XiangshuBossDefense4 = 16304;

		// Token: 0x04000B83 RID: 2947
		public const int XiangshuBossDefense5 = 16305;

		// Token: 0x04000B84 RID: 2948
		public const int XiangshuBossDefense6 = 16306;

		// Token: 0x04000B85 RID: 2949
		public const int XiangshuBossDefense7 = 16307;

		// Token: 0x04000B86 RID: 2950
		public const int XiangshuBossDefense8 = 16308;

		// Token: 0x04000B87 RID: 2951
		public const int XiangshuBossDefense9 = 16309;

		// Token: 0x04000B88 RID: 2952
		public const int XiangshuBossDefense10 = 16310;

		// Token: 0x04000B89 RID: 2953
		public const int XiangshuBossDefense11 = 16311;

		// Token: 0x04000B8A RID: 2954
		public const int XiangshuBossDefense12 = 16312;

		// Token: 0x04000B8B RID: 2955
		public const int XiangshuBossDefense13 = 16313;

		// Token: 0x04000B8C RID: 2956
		public const int XiangshuBossAssist0 = 16400;

		// Token: 0x04000B8D RID: 2957
		public const int XiangshuBossAssist1 = 16401;

		// Token: 0x04000B8E RID: 2958
		public const int XiangshuBossAssist2 = 16402;

		// Token: 0x04000B8F RID: 2959
		public const int XiangshuBossAssist3 = 16403;

		// Token: 0x04000B90 RID: 2960
		public const int XiangshuBossAssist4 = 16404;

		// Token: 0x04000B91 RID: 2961
		public const int XiangshuBossAssist5 = 16405;

		// Token: 0x04000B92 RID: 2962
		public const int XiangshuBossAssist6 = 16406;

		// Token: 0x04000B93 RID: 2963
		public const int XiangshuBossAssist7 = 16407;

		// Token: 0x04000B94 RID: 2964
		public const int XiangshuBossAssist8 = 16408;

		// Token: 0x04000B95 RID: 2965
		public const int XiangshuBossAssist9 = 16409;

		// Token: 0x04000B96 RID: 2966
		public const int XiangshuBossAssist10 = 16410;

		// Token: 0x04000B97 RID: 2967
		public const int XiangshuBossAssist11 = 16411;

		// Token: 0x04000B98 RID: 2968
		public const int XiangshuBossAssist12 = 16412;

		// Token: 0x04000B99 RID: 2969
		public const int XiangshuBossAssist13 = 16413;

		// Token: 0x04000B9A RID: 2970
		public const int XiangshuBossAssist14 = 16414;

		// Token: 0x04000B9B RID: 2971
		public const int XiangshuBossAssist15 = 16415;

		// Token: 0x04000B9C RID: 2972
		public const int XiangshuBossAssist16 = 16416;

		// Token: 0x04000B9D RID: 2973
		public const int XiangshuBossAssist17 = 16417;

		// Token: 0x04000B9E RID: 2974
		public const int XiangshuBossAssist18 = 16418;

		// Token: 0x04000B9F RID: 2975
		public const int XiangshuAttackMoNv0 = 17000;

		// Token: 0x04000BA0 RID: 2976
		public const int XiangshuAttackMoNv1 = 17001;

		// Token: 0x04000BA1 RID: 2977
		public const int XiangshuAttackMoNv2 = 17002;

		// Token: 0x04000BA2 RID: 2978
		public const int XiangshuAttackMoNv3 = 17003;

		// Token: 0x04000BA3 RID: 2979
		public const int XiangshuAttackMoNv4 = 17004;

		// Token: 0x04000BA4 RID: 2980
		public const int XiangshuAttackMoNv5 = 17005;

		// Token: 0x04000BA5 RID: 2981
		public const int XiangshuAttackDaYueYaoChang0 = 17010;

		// Token: 0x04000BA6 RID: 2982
		public const int XiangshuAttackDaYueYaoChang1 = 17011;

		// Token: 0x04000BA7 RID: 2983
		public const int XiangshuAttackDaYueYaoChang2 = 17012;

		// Token: 0x04000BA8 RID: 2984
		public const int XiangshuAttackDaYueYaoChang3 = 17013;

		// Token: 0x04000BA9 RID: 2985
		public const int XiangshuAttackDaYueYaoChang4 = 17014;

		// Token: 0x04000BAA RID: 2986
		public const int XiangshuAttackDaYueYaoChang5 = 17015;

		// Token: 0x04000BAB RID: 2987
		public const int XiangshuAttackJiuHan0 = 17020;

		// Token: 0x04000BAC RID: 2988
		public const int XiangshuAttackJiuHan1 = 17021;

		// Token: 0x04000BAD RID: 2989
		public const int XiangshuAttackJiuHan2 = 17022;

		// Token: 0x04000BAE RID: 2990
		public const int XiangshuAttackJiuHan3 = 17023;

		// Token: 0x04000BAF RID: 2991
		public const int XiangshuAttackJiuHan4 = 17024;

		// Token: 0x04000BB0 RID: 2992
		public const int XiangshuAttackJiuHan5 = 17025;

		// Token: 0x04000BB1 RID: 2993
		public const int XiangshuAttackJinHuangEr0 = 17030;

		// Token: 0x04000BB2 RID: 2994
		public const int XiangshuAttackJinHuangEr1 = 17031;

		// Token: 0x04000BB3 RID: 2995
		public const int XiangshuAttackJinHuangEr2 = 17032;

		// Token: 0x04000BB4 RID: 2996
		public const int XiangshuAttackJinHuangEr3 = 17033;

		// Token: 0x04000BB5 RID: 2997
		public const int XiangshuAttackJinHuangEr4 = 17034;

		// Token: 0x04000BB6 RID: 2998
		public const int XiangshuAttackJinHuangEr5 = 17035;

		// Token: 0x04000BB7 RID: 2999
		public const int XiangshuAttackYiYiHou0 = 17040;

		// Token: 0x04000BB8 RID: 3000
		public const int XiangshuAttackYiYiHou1 = 17041;

		// Token: 0x04000BB9 RID: 3001
		public const int XiangshuAttackYiYiHou2 = 17042;

		// Token: 0x04000BBA RID: 3002
		public const int XiangshuAttackYiYiHou3 = 17043;

		// Token: 0x04000BBB RID: 3003
		public const int XiangshuAttackYiYiHou4 = 17044;

		// Token: 0x04000BBC RID: 3004
		public const int XiangshuAttackYiYiHou5 = 17045;

		// Token: 0x04000BBD RID: 3005
		public const int XiangshuAttackWeiQi0 = 17050;

		// Token: 0x04000BBE RID: 3006
		public const int XiangshuAttackWeiQi1 = 17051;

		// Token: 0x04000BBF RID: 3007
		public const int XiangshuAttackWeiQi2 = 17052;

		// Token: 0x04000BC0 RID: 3008
		public const int XiangshuAttackWeiQi3 = 17053;

		// Token: 0x04000BC1 RID: 3009
		public const int XiangshuAttackWeiQi4 = 17054;

		// Token: 0x04000BC2 RID: 3010
		public const int XiangshuAttackWeiQi5 = 17055;

		// Token: 0x04000BC3 RID: 3011
		public const int XiangshuAttackYiXiang0 = 17060;

		// Token: 0x04000BC4 RID: 3012
		public const int XiangshuAttackYiXiang1 = 17061;

		// Token: 0x04000BC5 RID: 3013
		public const int XiangshuAttackYiXiang2 = 17062;

		// Token: 0x04000BC6 RID: 3014
		public const int XiangshuAttackYiXiang3 = 17063;

		// Token: 0x04000BC7 RID: 3015
		public const int XiangshuAttackYiXiang4 = 17064;

		// Token: 0x04000BC8 RID: 3016
		public const int XiangshuAttackYiXiang5 = 17065;

		// Token: 0x04000BC9 RID: 3017
		public const int XiangshuAttackXueFeng0 = 17070;

		// Token: 0x04000BCA RID: 3018
		public const int XiangshuAttackXueFeng1 = 17071;

		// Token: 0x04000BCB RID: 3019
		public const int XiangshuAttackXueFeng2 = 17072;

		// Token: 0x04000BCC RID: 3020
		public const int XiangshuAttackXueFeng3 = 17073;

		// Token: 0x04000BCD RID: 3021
		public const int XiangshuAttackXueFeng4 = 17074;

		// Token: 0x04000BCE RID: 3022
		public const int XiangshuAttackXueFeng5 = 17075;

		// Token: 0x04000BCF RID: 3023
		public const int XiangshuAttackShuFang0 = 17080;

		// Token: 0x04000BD0 RID: 3024
		public const int XiangshuAttackShuFang1 = 17081;

		// Token: 0x04000BD1 RID: 3025
		public const int XiangshuAttackShuFang2 = 17082;

		// Token: 0x04000BD2 RID: 3026
		public const int XiangshuAttackShuFang3 = 17083;

		// Token: 0x04000BD3 RID: 3027
		public const int XiangshuAttackShuFang4 = 17084;

		// Token: 0x04000BD4 RID: 3028
		public const int XiangshuAttackShuFang5 = 17085;

		// Token: 0x04000BD5 RID: 3029
		public const int XiangshuAttackXiangShu0 = 17090;

		// Token: 0x04000BD6 RID: 3030
		public const int XiangshuAttackXiangShu1 = 17091;

		// Token: 0x04000BD7 RID: 3031
		public const int XiangshuAttackXiangShu2 = 17092;

		// Token: 0x04000BD8 RID: 3032
		public const int XiangshuAttackXiangShu3 = 17093;

		// Token: 0x04000BD9 RID: 3033
		public const int XiangshuAttackXiangShu4 = 17094;

		// Token: 0x04000BDA RID: 3034
		public const int XiangshuAttackXiangShu5 = 17095;

		// Token: 0x04000BDB RID: 3035
		public const int XiangshuAttackHuanXin0 = 17100;

		// Token: 0x04000BDC RID: 3036
		public const int XiangshuAttackHuanXin1 = 17101;

		// Token: 0x04000BDD RID: 3037
		public const int XiangshuAttackHuanXin2 = 17102;

		// Token: 0x04000BDE RID: 3038
		public const int XiangshuAttackHuanXin3 = 17103;

		// Token: 0x04000BDF RID: 3039
		public const int XiangshuAttackHuanXin4 = 17104;

		// Token: 0x04000BE0 RID: 3040
		public const int XiangshuAttackHuanXin5 = 17105;

		// Token: 0x04000BE1 RID: 3041
		public const int XiangshuAttackZiWuXiao0 = 17110;

		// Token: 0x04000BE2 RID: 3042
		public const int XiangshuAttackZiWuXiao1 = 17111;

		// Token: 0x04000BE3 RID: 3043
		public const int XiangshuAttackZiWuXiao2 = 17112;

		// Token: 0x04000BE4 RID: 3044
		public const int XiangshuAttackZiWuXiao3 = 17113;

		// Token: 0x04000BE5 RID: 3045
		public const int XiangshuAttackZiWuXiao4 = 17114;

		// Token: 0x04000BE6 RID: 3046
		public const int XiangshuAttackZiWuXiao5 = 17115;

		// Token: 0x04000BE7 RID: 3047
		public const int XiangshuAttackLongYuFu0 = 17120;

		// Token: 0x04000BE8 RID: 3048
		public const int XiangshuAttackLongYuFu1 = 17121;

		// Token: 0x04000BE9 RID: 3049
		public const int XiangshuAttackLongYuFu2 = 17122;

		// Token: 0x04000BEA RID: 3050
		public const int XiangshuAttackLongYuFu3 = 17123;

		// Token: 0x04000BEB RID: 3051
		public const int XiangshuAttackLongYuFu4 = 17124;

		// Token: 0x04000BEC RID: 3052
		public const int XiangshuAttackLongYuFu5 = 17125;

		// Token: 0x04000BED RID: 3053
		public const int XiangshuAttackRanChenZi0 = 17130;

		// Token: 0x04000BEE RID: 3054
		public const int XiangshuAttackRanChenZi1 = 17131;

		// Token: 0x04000BEF RID: 3055
		public const int XiangshuAttackRanChenZi2 = 17132;

		// Token: 0x04000BF0 RID: 3056
		public const int XiangshuAttackRanChenZi3 = 17133;

		// Token: 0x04000BF1 RID: 3057
		public const int XiangshuAttackRanChenZi4 = 17134;

		// Token: 0x04000BF2 RID: 3058
		public const int XiangshuAttackRanChenZi5 = 17135;

		// Token: 0x04000BF3 RID: 3059
		public const int XiangshuAttackRanChenZi3Assist = 17136;

		// Token: 0x04000BF4 RID: 3060
		public const int ReduceEnemyAvoid0 = 30000;

		// Token: 0x04000BF5 RID: 3061
		public const int ReduceEnemyAvoid1 = 30001;

		// Token: 0x04000BF6 RID: 3062
		public const int ReduceEnemyAvoid2 = 30002;

		// Token: 0x04000BF7 RID: 3063
		public const int ReduceEnemyAvoid3 = 30003;

		// Token: 0x04000BF8 RID: 3064
		public const int ReduceEnemyAvoid4 = 30004;

		// Token: 0x04000BF9 RID: 3065
		public const int ReduceEnemyAvoid5 = 30005;

		// Token: 0x04000BFA RID: 3066
		public const int ReduceEnemyAvoid6 = 30006;

		// Token: 0x04000BFB RID: 3067
		public const int ReduceEnemyAvoid7 = 30007;

		// Token: 0x04000BFC RID: 3068
		public const int ReduceEnemyAvoid8 = 30008;

		// Token: 0x04000BFD RID: 3069
		public const int ReduceEnemyPenetrateResist0 = 30009;

		// Token: 0x04000BFE RID: 3070
		public const int ReduceEnemyPenetrateResist1 = 30010;

		// Token: 0x04000BFF RID: 3071
		public const int ReduceEnemyPenetrateResist2 = 30011;

		// Token: 0x04000C00 RID: 3072
		public const int ReduceEnemyPenetrateResist3 = 30012;

		// Token: 0x04000C01 RID: 3073
		public const int ReduceBounceOuter = 30013;

		// Token: 0x04000C02 RID: 3074
		public const int ReduceBounceInner = 30014;

		// Token: 0x04000C03 RID: 3075
		public const int BreakAddOuterInjury = 30015;

		// Token: 0x04000C04 RID: 3076
		public const int BreakAddInnerInjury = 30016;

		// Token: 0x04000C05 RID: 3077
		public const int WeaponAddPoison = 30017;

		// Token: 0x04000C06 RID: 3078
		public const int ReduceEnemyHit0 = 30100;

		// Token: 0x04000C07 RID: 3079
		public const int ReduceEnemyHit1 = 30101;

		// Token: 0x04000C08 RID: 3080
		public const int ReduceEnemyHit2 = 30102;

		// Token: 0x04000C09 RID: 3081
		public const int ReduceEnemyHit3 = 30103;

		// Token: 0x04000C0A RID: 3082
		public const int ReduceEnemyHit4 = 30104;

		// Token: 0x04000C0B RID: 3083
		public const int ReduceEnemyHit5 = 30105;

		// Token: 0x04000C0C RID: 3084
		public const int ReduceEnemyHit6 = 30106;

		// Token: 0x04000C0D RID: 3085
		public const int ReduceEnemyHit7 = 30107;

		// Token: 0x04000C0E RID: 3086
		public const int ReduceEnemyHit8 = 30108;

		// Token: 0x04000C0F RID: 3087
		public const int ReduceEnemyPenetrate0 = 30109;

		// Token: 0x04000C10 RID: 3088
		public const int ReduceEnemyPenetrate1 = 30110;

		// Token: 0x04000C11 RID: 3089
		public const int ReduceEnemyPenetrate2 = 30111;

		// Token: 0x04000C12 RID: 3090
		public const int ReduceEnemyPenetrate3 = 30112;

		// Token: 0x04000C13 RID: 3091
		public const int AddBounceOuter = 30113;

		// Token: 0x04000C14 RID: 3092
		public const int AddBounceInner = 30114;

		// Token: 0x04000C15 RID: 3093
		public const int BreakReduceOuterInjury = 30115;

		// Token: 0x04000C16 RID: 3094
		public const int BreakReduceInnerInjury = 30116;

		// Token: 0x04000C17 RID: 3095
		public const int ArmorAddPoison = 30117;

		// Token: 0x04000C18 RID: 3096
		public const int RawCreate0 = 30200;

		// Token: 0x04000C19 RID: 3097
		public const int RawCreate1 = 30201;

		// Token: 0x04000C1A RID: 3098
		public const int RawCreate2 = 30202;

		// Token: 0x04000C1B RID: 3099
		public const int RawCreate3 = 30203;

		// Token: 0x04000C1C RID: 3100
		public const int RawCreate4 = 30204;

		// Token: 0x04000C1D RID: 3101
		public const int RawCreate5 = 30205;

		// Token: 0x04000C1E RID: 3102
		public const int LegendaryBookNeigongWeapon = 40000;

		// Token: 0x04000C1F RID: 3103
		public const int LegendaryBookNeigongSkill0 = 40001;

		// Token: 0x04000C20 RID: 3104
		public const int LegendaryBookNeigongSkill1 = 40002;

		// Token: 0x04000C21 RID: 3105
		public const int LegendaryBookNeigongSkill2 = 40003;

		// Token: 0x04000C22 RID: 3106
		public const int LegendaryBookNeigongSkill3 = 40004;

		// Token: 0x04000C23 RID: 3107
		public const int LegendaryBookNeigongSkill4 = 40005;

		// Token: 0x04000C24 RID: 3108
		public const int LegendaryBookNeigongSkill5 = 40006;

		// Token: 0x04000C25 RID: 3109
		public const int LegendaryBookNeigongSkill6 = 40007;

		// Token: 0x04000C26 RID: 3110
		public const int LegendaryBookNeigongSkill7 = 40008;

		// Token: 0x04000C27 RID: 3111
		public const int LegendaryBookNeigongSkill8 = 40009;

		// Token: 0x04000C28 RID: 3112
		public const int LegendaryBookPosingWeapon = 40100;

		// Token: 0x04000C29 RID: 3113
		public const int LegendaryBookPosingSkill0 = 40101;

		// Token: 0x04000C2A RID: 3114
		public const int LegendaryBookPosingSkill1 = 40102;

		// Token: 0x04000C2B RID: 3115
		public const int LegendaryBookPosingSkill2 = 40103;

		// Token: 0x04000C2C RID: 3116
		public const int LegendaryBookPosingSkill3 = 40104;

		// Token: 0x04000C2D RID: 3117
		public const int LegendaryBookPosingSkill4 = 40105;

		// Token: 0x04000C2E RID: 3118
		public const int LegendaryBookPosingSkill5 = 40106;

		// Token: 0x04000C2F RID: 3119
		public const int LegendaryBookPosingSkill6 = 40107;

		// Token: 0x04000C30 RID: 3120
		public const int LegendaryBookPosingSkill7 = 40108;

		// Token: 0x04000C31 RID: 3121
		public const int LegendaryBookPosingSkill8 = 40109;

		// Token: 0x04000C32 RID: 3122
		public const int LegendaryBookStuntWeapon = 40200;

		// Token: 0x04000C33 RID: 3123
		public const int LegendaryBookStuntSkill0 = 40201;

		// Token: 0x04000C34 RID: 3124
		public const int LegendaryBookStuntSkill1 = 40202;

		// Token: 0x04000C35 RID: 3125
		public const int LegendaryBookStuntSkill2 = 40203;

		// Token: 0x04000C36 RID: 3126
		public const int LegendaryBookStuntSkill3 = 40204;

		// Token: 0x04000C37 RID: 3127
		public const int LegendaryBookStuntSkill4 = 40205;

		// Token: 0x04000C38 RID: 3128
		public const int LegendaryBookStuntSkill5 = 40206;

		// Token: 0x04000C39 RID: 3129
		public const int LegendaryBookStuntSkill6 = 40207;

		// Token: 0x04000C3A RID: 3130
		public const int LegendaryBookStuntSkill7 = 40208;

		// Token: 0x04000C3B RID: 3131
		public const int LegendaryBookStuntSkill8 = 40209;

		// Token: 0x04000C3C RID: 3132
		public const int LegendaryBookFistAndPalmWeapon = 40300;

		// Token: 0x04000C3D RID: 3133
		public const int LegendaryBookFistAndPalmSkill0 = 40301;

		// Token: 0x04000C3E RID: 3134
		public const int LegendaryBookFistAndPalmSkill1 = 40302;

		// Token: 0x04000C3F RID: 3135
		public const int LegendaryBookFistAndPalmSkill2 = 40303;

		// Token: 0x04000C40 RID: 3136
		public const int LegendaryBookFistAndPalmSkill3 = 40304;

		// Token: 0x04000C41 RID: 3137
		public const int LegendaryBookFistAndPalmSkill4 = 40305;

		// Token: 0x04000C42 RID: 3138
		public const int LegendaryBookFistAndPalmSkill5 = 40306;

		// Token: 0x04000C43 RID: 3139
		public const int LegendaryBookFistAndPalmSkill6 = 40307;

		// Token: 0x04000C44 RID: 3140
		public const int LegendaryBookFistAndPalmSkill7 = 40308;

		// Token: 0x04000C45 RID: 3141
		public const int LegendaryBookFistAndPalmSkill8 = 40309;

		// Token: 0x04000C46 RID: 3142
		public const int LegendaryBookFingerWeapon = 40400;

		// Token: 0x04000C47 RID: 3143
		public const int LegendaryBookFingerSkill0 = 40401;

		// Token: 0x04000C48 RID: 3144
		public const int LegendaryBookFingerSkill1 = 40402;

		// Token: 0x04000C49 RID: 3145
		public const int LegendaryBookFingerSkill2 = 40403;

		// Token: 0x04000C4A RID: 3146
		public const int LegendaryBookFingerSkill3 = 40404;

		// Token: 0x04000C4B RID: 3147
		public const int LegendaryBookFingerSkill4 = 40405;

		// Token: 0x04000C4C RID: 3148
		public const int LegendaryBookFingerSkill5 = 40406;

		// Token: 0x04000C4D RID: 3149
		public const int LegendaryBookFingerSkill6 = 40407;

		// Token: 0x04000C4E RID: 3150
		public const int LegendaryBookFingerSkill7 = 40408;

		// Token: 0x04000C4F RID: 3151
		public const int LegendaryBookFingerSkill8 = 40409;

		// Token: 0x04000C50 RID: 3152
		public const int LegendaryBookLegWeapon = 40500;

		// Token: 0x04000C51 RID: 3153
		public const int LegendaryBookLegSkill0 = 40501;

		// Token: 0x04000C52 RID: 3154
		public const int LegendaryBookLegSkill1 = 40502;

		// Token: 0x04000C53 RID: 3155
		public const int LegendaryBookLegSkill2 = 40503;

		// Token: 0x04000C54 RID: 3156
		public const int LegendaryBookLegSkill3 = 40504;

		// Token: 0x04000C55 RID: 3157
		public const int LegendaryBookLegSkill4 = 40505;

		// Token: 0x04000C56 RID: 3158
		public const int LegendaryBookLegSkill5 = 40506;

		// Token: 0x04000C57 RID: 3159
		public const int LegendaryBookLegSkill6 = 40507;

		// Token: 0x04000C58 RID: 3160
		public const int LegendaryBookLegSkill7 = 40508;

		// Token: 0x04000C59 RID: 3161
		public const int LegendaryBookLegSkill8 = 40509;

		// Token: 0x04000C5A RID: 3162
		public const int LegendaryBookThrowWeapon = 40600;

		// Token: 0x04000C5B RID: 3163
		public const int LegendaryBookThrowSkill0 = 40601;

		// Token: 0x04000C5C RID: 3164
		public const int LegendaryBookThrowSkill1 = 40602;

		// Token: 0x04000C5D RID: 3165
		public const int LegendaryBookThrowSkill2 = 40603;

		// Token: 0x04000C5E RID: 3166
		public const int LegendaryBookThrowSkill3 = 40604;

		// Token: 0x04000C5F RID: 3167
		public const int LegendaryBookThrowSkill4 = 40605;

		// Token: 0x04000C60 RID: 3168
		public const int LegendaryBookThrowSkill5 = 40606;

		// Token: 0x04000C61 RID: 3169
		public const int LegendaryBookThrowSkill6 = 40607;

		// Token: 0x04000C62 RID: 3170
		public const int LegendaryBookThrowSkill7 = 40608;

		// Token: 0x04000C63 RID: 3171
		public const int LegendaryBookThrowSkill8 = 40609;

		// Token: 0x04000C64 RID: 3172
		public const int LegendaryBookSwordWeapon = 40700;

		// Token: 0x04000C65 RID: 3173
		public const int LegendaryBookSwordSkill0 = 40701;

		// Token: 0x04000C66 RID: 3174
		public const int LegendaryBookSwordSkill1 = 40702;

		// Token: 0x04000C67 RID: 3175
		public const int LegendaryBookSwordSkill2 = 40703;

		// Token: 0x04000C68 RID: 3176
		public const int LegendaryBookSwordSkill3 = 40704;

		// Token: 0x04000C69 RID: 3177
		public const int LegendaryBookSwordSkill4 = 40705;

		// Token: 0x04000C6A RID: 3178
		public const int LegendaryBookSwordSkill5 = 40706;

		// Token: 0x04000C6B RID: 3179
		public const int LegendaryBookSwordSkill6 = 40707;

		// Token: 0x04000C6C RID: 3180
		public const int LegendaryBookSwordSkill7 = 40708;

		// Token: 0x04000C6D RID: 3181
		public const int LegendaryBookSwordSkill8 = 40709;

		// Token: 0x04000C6E RID: 3182
		public const int LegendaryBookBladeWeapon = 40800;

		// Token: 0x04000C6F RID: 3183
		public const int LegendaryBookBladeSkill0 = 40801;

		// Token: 0x04000C70 RID: 3184
		public const int LegendaryBookBladeSkill1 = 40802;

		// Token: 0x04000C71 RID: 3185
		public const int LegendaryBookBladeSkill2 = 40803;

		// Token: 0x04000C72 RID: 3186
		public const int LegendaryBookBladeSkill3 = 40804;

		// Token: 0x04000C73 RID: 3187
		public const int LegendaryBookBladeSkill4 = 40805;

		// Token: 0x04000C74 RID: 3188
		public const int LegendaryBookBladeSkill5 = 40806;

		// Token: 0x04000C75 RID: 3189
		public const int LegendaryBookBladeSkill6 = 40807;

		// Token: 0x04000C76 RID: 3190
		public const int LegendaryBookBladeSkill7 = 40808;

		// Token: 0x04000C77 RID: 3191
		public const int LegendaryBookBladeSkill8 = 40809;

		// Token: 0x04000C78 RID: 3192
		public const int LegendaryBookPolearmWeapon = 40900;

		// Token: 0x04000C79 RID: 3193
		public const int LegendaryBookPolearmSkill0 = 40901;

		// Token: 0x04000C7A RID: 3194
		public const int LegendaryBookPolearmSkill1 = 40902;

		// Token: 0x04000C7B RID: 3195
		public const int LegendaryBookPolearmSkill2 = 40903;

		// Token: 0x04000C7C RID: 3196
		public const int LegendaryBookPolearmSkill3 = 40904;

		// Token: 0x04000C7D RID: 3197
		public const int LegendaryBookPolearmSkill4 = 40905;

		// Token: 0x04000C7E RID: 3198
		public const int LegendaryBookPolearmSkill5 = 40906;

		// Token: 0x04000C7F RID: 3199
		public const int LegendaryBookPolearmSkill6 = 40907;

		// Token: 0x04000C80 RID: 3200
		public const int LegendaryBookPolearmSkill7 = 40908;

		// Token: 0x04000C81 RID: 3201
		public const int LegendaryBookPolearmSkill8 = 40909;

		// Token: 0x04000C82 RID: 3202
		public const int LegendaryBookSpecialWeapon = 41000;

		// Token: 0x04000C83 RID: 3203
		public const int LegendaryBookSpecialSkill0 = 41001;

		// Token: 0x04000C84 RID: 3204
		public const int LegendaryBookSpecialSkill1 = 41002;

		// Token: 0x04000C85 RID: 3205
		public const int LegendaryBookSpecialSkill2 = 41003;

		// Token: 0x04000C86 RID: 3206
		public const int LegendaryBookSpecialSkill3 = 41004;

		// Token: 0x04000C87 RID: 3207
		public const int LegendaryBookSpecialSkill4 = 41005;

		// Token: 0x04000C88 RID: 3208
		public const int LegendaryBookSpecialSkill5 = 41006;

		// Token: 0x04000C89 RID: 3209
		public const int LegendaryBookSpecialSkill6 = 41007;

		// Token: 0x04000C8A RID: 3210
		public const int LegendaryBookSpecialSkill7 = 41008;

		// Token: 0x04000C8B RID: 3211
		public const int LegendaryBookSpecialSkill8 = 41009;

		// Token: 0x04000C8C RID: 3212
		public const int LegendaryBookWhipWeapon = 41100;

		// Token: 0x04000C8D RID: 3213
		public const int LegendaryBookWhipSkill0 = 41101;

		// Token: 0x04000C8E RID: 3214
		public const int LegendaryBookWhipSkill1 = 41102;

		// Token: 0x04000C8F RID: 3215
		public const int LegendaryBookWhipSkill2 = 41103;

		// Token: 0x04000C90 RID: 3216
		public const int LegendaryBookWhipSkill3 = 41104;

		// Token: 0x04000C91 RID: 3217
		public const int LegendaryBookWhipSkill4 = 41105;

		// Token: 0x04000C92 RID: 3218
		public const int LegendaryBookWhipSkill5 = 41106;

		// Token: 0x04000C93 RID: 3219
		public const int LegendaryBookWhipSkill6 = 41107;

		// Token: 0x04000C94 RID: 3220
		public const int LegendaryBookWhipSkill7 = 41108;

		// Token: 0x04000C95 RID: 3221
		public const int LegendaryBookWhipSkill8 = 41109;

		// Token: 0x04000C96 RID: 3222
		public const int LegendaryBookControllableShotWeapon = 41200;

		// Token: 0x04000C97 RID: 3223
		public const int LegendaryBookControllableShotSkill0 = 41201;

		// Token: 0x04000C98 RID: 3224
		public const int LegendaryBookControllableShotSkill1 = 41202;

		// Token: 0x04000C99 RID: 3225
		public const int LegendaryBookControllableShotSkill2 = 41203;

		// Token: 0x04000C9A RID: 3226
		public const int LegendaryBookControllableShotSkill3 = 41204;

		// Token: 0x04000C9B RID: 3227
		public const int LegendaryBookControllableShotSkill4 = 41205;

		// Token: 0x04000C9C RID: 3228
		public const int LegendaryBookControllableShotSkill5 = 41206;

		// Token: 0x04000C9D RID: 3229
		public const int LegendaryBookControllableShotSkill6 = 41207;

		// Token: 0x04000C9E RID: 3230
		public const int LegendaryBookControllableShotSkill7 = 41208;

		// Token: 0x04000C9F RID: 3231
		public const int LegendaryBookControllableShotSkill8 = 41209;

		// Token: 0x04000CA0 RID: 3232
		public const int LegendaryBookCombatMusicWeapon = 41300;

		// Token: 0x04000CA1 RID: 3233
		public const int LegendaryBookCombatMusicSkill0 = 41301;

		// Token: 0x04000CA2 RID: 3234
		public const int LegendaryBookCombatMusicSkill1 = 41302;

		// Token: 0x04000CA3 RID: 3235
		public const int LegendaryBookCombatMusicSkill2 = 41303;

		// Token: 0x04000CA4 RID: 3236
		public const int LegendaryBookCombatMusicSkill3 = 41304;

		// Token: 0x04000CA5 RID: 3237
		public const int LegendaryBookCombatMusicSkill4 = 41305;

		// Token: 0x04000CA6 RID: 3238
		public const int LegendaryBookCombatMusicSkill5 = 41306;

		// Token: 0x04000CA7 RID: 3239
		public const int LegendaryBookCombatMusicSkill6 = 41307;

		// Token: 0x04000CA8 RID: 3240
		public const int LegendaryBookCombatMusicSkill7 = 41308;

		// Token: 0x04000CA9 RID: 3241
		public const int LegendaryBookCombatMusicSkill8 = 41309;

		// Token: 0x04000CAA RID: 3242
		public const int LegendaryBookNpcNeigong = 41400;

		// Token: 0x04000CAB RID: 3243
		public const int LegendaryBookNpcPosing = 41401;

		// Token: 0x04000CAC RID: 3244
		public const int LegendaryBookNpcStunt = 41402;

		// Token: 0x04000CAD RID: 3245
		public const int LegendaryBookNpcFistAndPalm = 41403;

		// Token: 0x04000CAE RID: 3246
		public const int LegendaryBookNpcFinger = 41404;

		// Token: 0x04000CAF RID: 3247
		public const int LegendaryBookNpcLeg = 41405;

		// Token: 0x04000CB0 RID: 3248
		public const int LegendaryBookNpcThrow = 41406;

		// Token: 0x04000CB1 RID: 3249
		public const int LegendaryBookNpcSword = 41407;

		// Token: 0x04000CB2 RID: 3250
		public const int LegendaryBookNpcBlade = 41408;

		// Token: 0x04000CB3 RID: 3251
		public const int LegendaryBookNpcPolearm = 41409;

		// Token: 0x04000CB4 RID: 3252
		public const int LegendaryBookNpcSpecial = 41410;

		// Token: 0x04000CB5 RID: 3253
		public const int LegendaryBookNpcWhip = 41411;

		// Token: 0x04000CB6 RID: 3254
		public const int LegendaryBookNpcControllableShot = 41412;

		// Token: 0x04000CB7 RID: 3255
		public const int LegendaryBookNpcCombatMusic = 41413;

		// Token: 0x04000CB8 RID: 3256
		public const int AddPenetrateAndPenetrateResist = 1000000;

		// Token: 0x04000CB9 RID: 3257
		public const int AddMaxHealth = 1000001;
	}
}
