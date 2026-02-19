using System;
using System.Collections.Generic;
using System.Text;
using CompDevLib.Interpreter;
using CompDevLib.Interpreter.Parse;
using GameData.Domains.Adventure;
using GameData.Domains.Character;
using GameData.Domains.Character.AvatarSystem;
using GameData.Domains.Character.Display;
using GameData.Domains.Item;
using GameData.Domains.Map;
using GameData.Domains.TaiwuEvent.DisplayEvent;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.TaiwuEvent;

[SerializableGameData(NotForDisplayModule = true, NotRestrictCollectionSerializedSize = true)]
public class EventArgBox : ISerializableGameData, IVariantCollection<string>, IValueSelector
{
	public const string ShuffleOption = "ShuffleOptions";

	public const string MainRoleUseAlternativeName = "MainRoleUseAlternativeName";

	public const string TargetRoleUseAlternativeName = "TargetRoleUseAlternativeName";

	public const string NotShowTargetRole = "NotShowTargetRole";

	public const string NotShowMainRole = "NotShowMainRole";

	public const string ForbidViewCharacter = "ForbidViewCharacter";

	public const string ForbidViewSelf = "ForbidViewSelf";

	public const string HideFavorability = "HideFavorability";

	public const string HideLeftFavorability = "ConchShip_PresetKey_HideLeftFavorability";

	public const string MainRoleShowBlush = "ConchShip_PresetKey_MainRoleShowBlush";

	public const string TargetRoleShowBlush = "ConchShip_PresetKey_TargetRoleShowBlush";

	public const string LeftRoleShowInjuryInfo = "ConchShip_PresetKey_LeftRoleShowInjuryInfo";

	public const string RightRoleShowInjuryInfo = "ConchShip_PresetKey_RightRoleShowInjuryInfo";

	public const string RightCharacterShadow = "ConchShip_PresetKey_RightCharacterShadow";

	public const string RightForbiddenConsummateLevel = "ConchShip_PresetKey_RightForbiddenConsummateLevel";

	public const string LeftForbidShowFavorChangeEffect = "CS_PK_LeftForbidShowFavorChangeEffect";

	public const string RightForbidShowFavorChangeEffect = "CS_PK_RightForbidShowFavorChangeEffect";

	public const string OrgTemplateId = "OrgTemplateId";

	public const string RoleTaiwu = "RoleTaiwu";

	[Obsolete]
	public const string ShowLeftFavorability = "ConchShip_PresetKey_ShowLeftFavorability";

	public const string SelectItemInfo = "SelectItemInfo";

	public const string SelectReadingBookCount = "SelectReadingBookCount";

	public const string SelectNeigongLoopingCount = "SelectNeigongLoopingCount";

	public const string SelectFuyuFaithCount = "SelectFuyuFaithCount";

	public const string SelectFameData = "SelectFameData";

	public const string SelectCountResult = "SelectCountResult";

	public const string SelectCharacterData = "SelectCharacterData";

	public const string InputRequestData = "InputRequestData";

	public const string ActorKey = "ActorKey";

	public const string LeftActorKey = "ConchShip_PresetKey_LeftActorKey";

	public const string TargetCharacterTemplateId = "TargetCharacterTemplateId";

	public const string SelectAvatarEvent = "SelectAvatarEvent";

	public const string MainCharacterDisplayAge = "MainCharacterDisplayAge";

	public const string MainCharacterDisplayCloth = "MainCharacterDisplayCloth";

	public const string TargetCharacterDisplayAge = "TargetCharacterDisplayAge";

	public const string TargetCharacterDisplayCloth = "TargetCharacterDisplayCloth";

	public const string TargetCharacterDisplayingClothId = "TargetCharacterDisplayingClothId";

	public const string EventInstanceGuid = "EventInstanceGuid";

	public const string CharIdSeizedInCombat = "CharIdSeizedInCombat";

	public const string ItemKeySeizeCharacterInCombat = "ItemKeySeizeCharacterInCombat";

	public const string UseItemKeySeizeCharacterId = "UseItemKeySeizeCharacterId";

	public const string ItemKeySeizeCarrierInCombat = "ItemKeySeizeCarrierInCombat";

	public const string CarrierItemKeyGotInCombat = "CarrierItemKeyGotInCombat";

	public const string UsedFuyuSwordInCombat = "UsedFuyuSwordInCombat";

	public const string DefaultHandleFlag = "DefaultHandleFlag";

	public const string PresetKeyDateOfNextSwordTombActivate = "ConchShip_PresetKey_DateOfNextSwordTombActivate";

	public const string ShopBuyMoney = "ConchShip_PresetKey_ShopBuyMoney";

	public const string ShopHasAnyTrade = "ConchShip_PresetKey_ShopHasAnyTrade";

	public const string ProtectedByWeiQiCharacter = "ProtectedByWeiQiCharacter";

	public const string MainRoleAdjustClothId = "ConchShip_PresetKey_MainRoleAdjustClothId";

	public const string TargetRoleAdjustClothId = "ConchShip_PresetKey_TargetRoleAdjustClothId";

	public const string TradeItemPrice = "ConchShip_PresetKey_TradeItemPrice";

	public const string SetGivenNameMatchSensitive = "ConchShip_PresetKey_SetGivenNameMatchSensitive";

	public const string SetGivenNameMatchSystemRuleType = "ConchShip_PresetKey_SetGivenNameMatchSystemRuleType";

	public const string CaravanCount = "ConchShip_PresetKey_CaravanCount";

	public const string CaravanIndex = "ConchShip_PresetKey_CaravanIndex";

	public const string CaravanPresentCount = "ConchShip_PresetKey_CaravanPresentCount";

	public const string FinishSkillExecute = "ConchShip_PresetKey_FinishSkillExecute";

	public const string SpecifyEventBackground = "ConchShip_PresetKey_SpecifyEventBackground";

	public const string OptionWaitConfirmKey = "ConchShip_PresetKey_OptionWaitConfirm";

	public const string ConfirmWaitOptionSignal = "ConchShip_PresetKey_ConfirmWaitOptionSignal";

	public const string JiaoId = "JiaoId";

	public const string EventLogMainCharacter = "ConchShip_PresetKey_EventLogMainCharacter";

	public const string StillAtShaolin = "ConchShip_PresetKey_StillAtShaolin";

	public const string StillAtEmei = "ConchShip_PresetKey_StillAtEmei";

	public const string StillAtBaihua = "ConchShip_PresetKey_StillAtBaihua";

	public const string StillAtWudang = "ConchShip_PresetKey_StillAtWudang";

	public const string StillAtYuanshan = "ConchShip_PresetKey_StillAtYuanshan";

	public const string StillAtShixiang = "ConchShip_PresetKey_StillAtShixiang";

	public const string StillAtRanshan = "ConchShip_PresetKey_StillAtRanshan";

	public const string StillAtXuannv = "ConchShip_PresetKey_StillAtXuannv";

	public const string StillAtZhujian = "ConchShip_PresetKey_StillAtZhujian";

	public const string StillAtKongsang = "ConchShip_PresetKey_StillAtKongsang";

	public const string StillAtJingang = "ConchShip_PresetKey_StillAtJingang";

	public const string StillAtWuxian = "ConchShip_PresetKey_StillAtWuxian";

	public const string StillAtJieqing = "ConchShip_PresetKey_StillAtJieqing";

	public const string StillAtFulong = "ConchShip_PresetKey_StillAtFulong";

	public const string StillAtXuehou = "ConchShip_PresetKey_StillAtXuehou";

	public const string MonkeyRobRoad = "MonkeyRobRoad";

	public const string CatchCricketTimes = "CatchCricketTimes";

	public const string RoleWoodenMan = "RoleWoodenMan";

	public const string WaitFinalCatchResult = "WaitFinalCatchResult";

	public const string WaitForCriketSecond = "WaitForCriketSecond";

	public const string WaitForCriketFourth = "WaitForCriketFourth";

	public const string MeetMonkey = "MeetMonkey";

	public const string SmallVillage_GirlCharId = "GirlCharId";

	public const string SmallVillage_YouthCharId = "YouthCharId";

	public const string SmallVillage_BigWigCharId = "BigWigCharId";

	public const string SmallVillage_ChildCharId = "ChildCharId";

	public const string SmallVillage_ChuiXingId = "ChuiXingId";

	public const string SmallVillage_DaoshiAskForWildFood = "DaoshiAskForWildFood";

	public const string SmallVillage_BattleWithXiangshuMinion = "BattleWithXiangshuMinion";

	public const string SmallVillage_MeetInfectedVillagerOnMap = "ConchShip_PresetKey_SmallVillage_MeetInfectedVillagerOnMap";

	public const string VillageChange_SaveInfectedVillagerCount = "ConchShip_PresetKey_VillageChange_SaveInfectedVillagerCount";

	public const string SaveAnyVillager = "SaveAnyVillager";

	public const string RiverHaveBoat = "RiverHaveBoat";

	public const string VillageHaveChanged = "VillageHaveChanged";

	public const string ChatWithANiu = "ChatWithANiu";

	public const string ChatWithGuoYan = "ChatWithGuoYan";

	public const string ChatWithXiaomao = "ChatWithXiaomao";

	public const string ChatWithHuanyue = "ChatWithHuanyue";

	public const string TryStealBoat = "TryStealBoat";

	public const string VillageRecordCount = "VillageRecordCount";

	public const string AllVillagerDieMonth = "AllVillagerDieMonth";

	public const string VillagePlayCombatInteract = "VillagePlayCombatInteract";

	public const string VillageCricketInteract = "VillageCricketInteract";

	public const string LoopDeliver = "LoopDeliver";

	public const string DeliverVegetable = "DeliverVegetable";

	public const string GirlLocation = "GirlLocation";

	public const string BigWigLocation = "BigWigLocation";

	public const string ChildLocation = "ChildLocation";

	public const string YouthLocation = "YouthLocation";

	public const string VillageLocation = "VillageLocation";

	public const string RecordResource = "RecordResource";

	public const string MeetSmallVilliage = "MeetSmallVilliage";

	public const string StonePotGot = "StonePotGot";

	public const string HaveReturnedVillage = "HaveReturnedVillage";

	public const string VillageChangedWithoutTaiwu = "VillageChangedWithoutTaiwu";

	public const string BrokenDate = "BrokenDate";

	public const string PostLocation = "PostLocation";

	public const string WangliuLocation = "WangliuLocation";

	public const string WangliuFirstMeet = "WangliuFirstMeet";

	public const string CarterActivated = "CarterActivated";

	public const string FuyuHiltGuiding = "ConchShip_PresetKey_FuyuHiltGuiding";

	public const string FuyuHiltCatchUpCount = "ConchShip_PresetKey_FuyuHiltCatchUpCount";

	public const string IsDreamBackArchive = "IsDreamBackArchive";

	public const string TaiwuCrossArchiveEventTriggered = "ConchShip_PresetKey_TaiwuCrossArchiveEventTriggered";

	public const string TaiwuCrossArchiveAvatarData = "ConchShip_PresetKey_TaiwuCrossArchiveAvatarData";

	public const string TaiwuCrossArchiveDisplayName = "ConchShip_PresetKey_TaiwuCrossArchiveDisplayName";

	public const string TaiwuCrossArchiveOptionSelected = "ConchShip_PresetKey_TaiwuCrossArchiveAvatarData";

	public const string TaiwuVillageStationOpenDate = "CS_PK_StationOpenDate";

	public const string CaravanVisitMonthEventTriggered = "CS_PK_CaravanVisit";

	public const string OldMonkId = "OldMonk";

	public const string MissNingId = "MissNing";

	public const string YirenId = "Yiren";

	public const string BloodCharacter = "BloodCharacter";

	public const string DefeatXiangshuMinion = "DefeatXiangshuMinion";

	public const string SaveVillager = "SaveVillager";

	public const string OutTaiwuVillage = "OutTaiwuVillage";

	public const string ReturnSmallVillageEvent = "ReturnSmallVillageEvent";

	public const string BorrowBoat = "BorrowBoat";

	public const string TaiwuAncestral = "TaiwuAncestral";

	public const string HelpAreaSect = "HelpAreaSect";

	public const string HelpEnemySect = "HelpEnemySect";

	public const string WaitingForPostStory = "WaitingForPostStory";

	public const string WaitForWesternMerchants = "WaitForWesternMerchants";

	public const string WaitForReincarnationOpen = "WaitForReincarnationOpen";

	public const string TaiwuPostLocation = "TaiwuPostLocation";

	public const string OldMonkToSwordTombCount = "OldMonkToSwordTombCount";

	public const string OldMonkSwordTombTalk = "OldMonkSwordTombTalk";

	public const string WakeUpAfterUsingSwordFirst = "WakeUpAfterUsingSwordFirst";

	public const string WaitTaiwuShrineComplete = "WaitTaiwuShrineComplete";

	public const string WakeUpAfterImmortalXuDestory = "WakeUpAfterImmortalXuDestory";

	public const string WaitForTombImmortalFirst = "WaitForTombImmortalFirst";

	public const string WaitForTombImmortalSecond = "WaitForTombImmortalSecond";

	public const string WaitForTombImmortalThird = "WaitForTombImmortalThird";

	public const string WaitForXiangongBack = "WaitForXiangongBack";

	public const string WaitForSwordTombAppearance = "WaitForSwordTombAppearance";

	public const string FarewellXuXiangong = "FarewellXuXiangong";

	public const string WaitForPurpleBambooAppear = "WaitForPurpleBambooAppear";

	public const string WaitForRanchenVisit = "WaitForRanchenVisit";

	public const string WaitTaiwuVillageDestory = "WaitTaiwuVillageDestory";

	public const string TrySurroundTaiwuVillage = "TrySurroundTaiwuVillage";

	public const string WaitForXiangongTime = "WaitForXiangongTime";

	public const string ImmortalXuMoveForSpiriteLand = "ImmortalXuMoveForSpiriteLand";

	public const string ImmortalXuBattle = "ImmortalXuBattle";

	public const string MeetLongYufu = "CS_PK_MeetLongYufu";

	public const string WaitForFirstWulinConference = "WaitForFirstWulinConference";

	public const string SwordStoveTombName = "SwordStoveTombName";

	public const string SwordStoveTombId = "SwordStoveTombId";

	public const string HuanxinCombatDie = "HuanxinCombatDie";

	public const string FirstMeetJunior = "FirstMeetJunior";

	public const string WaitForWuXiaoSpirit = "WaitForWuXiaoSpirit";

	public const string WuXiaoSacrificeStory = "WuXiaoSacrificeStory";

	public const string WaitWuXiaoDream = "WaitWuXiaoDream";

	public const string BlackBambooTime = "BlackBambooTime";

	public const string WaitForBlackBambooBorn = "WaitForBlackBambooBorn";

	public const string RockBambooCreateMonth = "RockBambooCreateMonth";

	public const string TaiwuMeetXiangshu = "TaiwuMeetXiangshu";

	public const string WaitFightXiangshuBegin = "WaitFightXiangshuBegin";

	public const string SealEvilPoints = "SealEvilPoints";

	public const string PassOutByUsingSwordFirst = "PassOutByUsingSwordFirst";

	public const string MarriedTaiwuId = "MarriedTaiwuId";

	public const string GivenCloth = "GivenCloth";

	public const string WaitBlackToReturnMainMenu = "WaitBlackToReturnMainMenu";

	public const string WaitCollectWoodOuter3 = "WaitCollectWoodOuter3";

	public const string WaitCreateBambooThorn = "WaitCreateBambooThorn";

	public const string WaitBambooComplete = "WaitBambooComplete";

	public const string AwayForeverTime = "AwayForeverTime";

	public const string ForeverLoverId = "ForeverLoverId";

	public const string StoryForeverLoverId = "StoryForeverLoverId";

	public const string YuFuTellRanchenziStory = "YuFuTellRanchenziStory";

	public const string IsQuickStartGame = "CS_PK_IsQuickStartGame";

	public const string IsGuardCombat = "IsGuardCombat";

	public const string GuardCombatLevel = "GuardCombatLevel";

	public const string FulongServantSetGender = "FulongServantSetGender";

	public const string FulongServantSetTransgender = "FulongServantSetTransgender";

	public const string FulongServantSetBehaviorType = "FulongServantSetBehaviorType";

	public const string FulongServantSetLifeSkillType = "FulongServantSetLifeSkillType";

	public const string FulongServantSetCombatSkillType = "FulongServantSetCombatSkillType";

	public const string FulongServantSetMainAttributeType = "FulongServantSetMainAttributeType";

	private Dictionary<string, int> _intBox;

	private Dictionary<string, string> _stringBox;

	private Dictionary<string, float> _floatBox;

	private Dictionary<string, bool> _boolBox;

	private Dictionary<string, ISerializableGameData> _serializableObjectBox;

	public static readonly Dictionary<Type, sbyte> SerializeObjectMap = new Dictionary<Type, sbyte>
	{
		{
			typeof(Location),
			0
		},
		{
			typeof(AdventureMapPoint),
			1
		},
		{
			typeof(ItemKey),
			2
		},
		{
			typeof(AdventureSiteData),
			3
		},
		{
			typeof(MapTemplateEnemyInfo),
			4
		},
		{
			typeof(AvatarRelatedData),
			5
		},
		{
			typeof(EventActorData),
			6
		},
		{
			typeof(ShortList),
			7
		},
		{
			typeof(AvatarData),
			8
		},
		{
			typeof(IntList),
			9
		}
	};

	private static int _instancesCount = 0;

	private static int _objectCollectionsCount = 0;

	public static int TaiwuCharacterId => DomainManager.Taiwu.GetTaiwuCharId();

	public static short TaiwuAreaId => DomainManager.Taiwu.GetTaiwu().GetLocation().AreaId;

	public static short TaiwuBlockId => DomainManager.Taiwu.GetTaiwu().GetLocation().BlockId;

	public static short TaiwuVillageAreaId => DomainManager.Taiwu.GetTaiwuVillageLocation().AreaId;

	public static short TaiwuVillageBlockId => DomainManager.Taiwu.GetTaiwuVillageLocation().BlockId;

	public static void ShowStatus()
	{
		int instancesCount = _instancesCount;
		int objectCollectionsCount = _objectCollectionsCount;
		if (instancesCount > 0 || objectCollectionsCount > 0)
		{
			AdaptableLog.Info($"EventArgBox newly created: {instancesCount} instances, {objectCollectionsCount} object collections.");
		}
		_instancesCount = 0;
		_objectCollectionsCount = 0;
	}

	public void Clear()
	{
		_intBox?.Clear();
		_stringBox?.Clear();
		_floatBox?.Clear();
		_boolBox?.Clear();
		_serializableObjectBox?.Clear();
		_intBox = null;
		_stringBox = null;
		_floatBox = null;
		_boolBox = null;
		_serializableObjectBox = null;
	}

	public void CloneTo(EventArgBox argBox)
	{
		if (_intBox != null)
		{
			foreach (KeyValuePair<string, int> item in _intBox)
			{
				EventArgBox eventArgBox = argBox;
				(eventArgBox._intBox ?? (eventArgBox._intBox = new Dictionary<string, int>())).Add(item.Key, item.Value);
			}
		}
		if (_stringBox != null)
		{
			foreach (KeyValuePair<string, string> item2 in _stringBox)
			{
				EventArgBox eventArgBox = argBox;
				(eventArgBox._stringBox ?? (eventArgBox._stringBox = new Dictionary<string, string>())).Add(item2.Key, item2.Value);
			}
		}
		if (_floatBox != null)
		{
			foreach (KeyValuePair<string, float> item3 in _floatBox)
			{
				EventArgBox eventArgBox = argBox;
				(eventArgBox._floatBox ?? (eventArgBox._floatBox = new Dictionary<string, float>())).Add(item3.Key, item3.Value);
			}
		}
		if (_boolBox != null)
		{
			foreach (KeyValuePair<string, bool> item4 in _boolBox)
			{
				EventArgBox eventArgBox = argBox;
				(eventArgBox._boolBox ?? (eventArgBox._boolBox = new Dictionary<string, bool>())).Add(item4.Key, item4.Value);
			}
		}
		if (_serializableObjectBox == null)
		{
			return;
		}
		foreach (KeyValuePair<string, ISerializableGameData> item5 in _serializableObjectBox)
		{
			EventArgBox eventArgBox = argBox;
			(eventArgBox._serializableObjectBox ?? (eventArgBox._serializableObjectBox = new Dictionary<string, ISerializableGameData>())).Add(item5.Key, GetCopyOfSerializableObject(item5.Value));
		}
	}

	public void Set(string key, sbyte arg)
	{
		if (!string.IsNullOrEmpty(key))
		{
			if (_intBox == null)
			{
				_intBox = new Dictionary<string, int>();
			}
			if (_intBox.ContainsKey(key))
			{
				_intBox[key] = arg;
			}
			else
			{
				_intBox.Add(key, arg);
			}
		}
	}

	public void Set(string key, byte arg)
	{
		if (!string.IsNullOrEmpty(key))
		{
			if (_intBox == null)
			{
				_intBox = new Dictionary<string, int>();
			}
			if (_intBox.ContainsKey(key))
			{
				_intBox[key] = arg;
			}
			else
			{
				_intBox.Add(key, arg);
			}
		}
	}

	public void Set(string key, ushort arg)
	{
		if (!string.IsNullOrEmpty(key))
		{
			if (_intBox == null)
			{
				_intBox = new Dictionary<string, int>();
			}
			if (_intBox.ContainsKey(key))
			{
				_intBox[key] = arg;
			}
			else
			{
				_intBox.Add(key, arg);
			}
		}
	}

	public void Set(string key, short arg)
	{
		if (!string.IsNullOrEmpty(key))
		{
			if (_intBox == null)
			{
				_intBox = new Dictionary<string, int>();
			}
			if (_intBox.ContainsKey(key))
			{
				_intBox[key] = arg;
			}
			else
			{
				_intBox.Add(key, arg);
			}
		}
	}

	public void Set(string key, int arg)
	{
		if (!string.IsNullOrEmpty(key))
		{
			if (_intBox == null)
			{
				_intBox = new Dictionary<string, int>();
			}
			if (_intBox.ContainsKey(key))
			{
				_intBox[key] = arg;
			}
			else
			{
				_intBox.Add(key, arg);
			}
		}
	}

	public void Set(string key, float arg)
	{
		if (!string.IsNullOrEmpty(key))
		{
			if (_floatBox == null)
			{
				_floatBox = new Dictionary<string, float>();
			}
			if (_floatBox.ContainsKey(key))
			{
				_floatBox[key] = arg;
			}
			else
			{
				_floatBox.Add(key, arg);
			}
		}
	}

	public void Set(string key, string arg)
	{
		if (!string.IsNullOrEmpty(key))
		{
			if (_stringBox == null)
			{
				_stringBox = new Dictionary<string, string>();
			}
			if (_stringBox.ContainsKey(key))
			{
				_stringBox[key] = arg;
			}
			else
			{
				_stringBox.Add(key, arg);
			}
		}
	}

	public void Set(string key, bool arg)
	{
		if (!string.IsNullOrEmpty(key))
		{
			if (_boolBox == null)
			{
				_boolBox = new Dictionary<string, bool>();
			}
			if (_boolBox.ContainsKey(key))
			{
				_boolBox[key] = arg;
			}
			else
			{
				_boolBox.Add(key, arg);
			}
		}
	}

	public void Set(string key, ISerializableGameData arg)
	{
		if (!string.IsNullOrEmpty(key))
		{
			if (_serializableObjectBox == null)
			{
				_serializableObjectBox = new Dictionary<string, ISerializableGameData>();
				_objectCollectionsCount++;
			}
			if (arg == null)
			{
				_serializableObjectBox.Remove(key);
			}
			else if (_serializableObjectBox.ContainsKey(key))
			{
				_serializableObjectBox[key] = arg;
			}
			else
			{
				_serializableObjectBox.Add(key, arg);
			}
		}
	}

	public void Remove<T>(string key)
	{
		if (typeof(T) == typeof(int) || typeof(T) == typeof(short) || typeof(T) == typeof(ushort) || typeof(T) == typeof(byte) || typeof(T) == typeof(sbyte))
		{
			_intBox?.Remove(key);
		}
		else if (typeof(T) == typeof(float))
		{
			_floatBox?.Remove(key);
		}
		else if (typeof(T) == typeof(bool))
		{
			_boolBox?.Remove(key);
		}
		else if (typeof(T) == typeof(string))
		{
			_stringBox?.Remove(key);
		}
		else
		{
			_serializableObjectBox?.Remove(key);
		}
		if (this == DomainManager.TaiwuEvent.GetGlobalEventArgumentBox())
		{
			DomainManager.TaiwuEvent.SaveGlobalEventArgumentBox();
		}
	}

	public void GenericSet<T>(string key, T value)
	{
		T val = value;
		T val2 = val;
		if (!(val2 is int arg))
		{
			if (!(val2 is short arg2))
			{
				if (!(val2 is ushort arg3))
				{
					if (!(val2 is byte arg4))
					{
						if (!(val2 is sbyte arg5))
						{
							if (!(val2 is float arg6))
							{
								if (!(val2 is string arg7))
								{
									if (!(val2 is bool arg8))
									{
										object obj = val2;
										ISerializableGameData val3 = (ISerializableGameData)((obj is ISerializableGameData) ? obj : null);
										if (val3 == null)
										{
											throw new Exception($"Value {value} of type {typeof(T)} cannot be saved in EventArgBox");
										}
										if (!SerializeObjectMap.ContainsKey(value.GetType()))
										{
											AdaptableLog.Warning($"{value.GetType()} is a type can only set to EventArgBox in runtime,but will not be saved when saving game!");
										}
										Set(key, val3);
									}
									else
									{
										Set(key, arg8);
									}
								}
								else
								{
									Set(key, arg7);
								}
							}
							else
							{
								Set(key, arg6);
							}
						}
						else
						{
							Set(key, arg5);
						}
					}
					else
					{
						Set(key, arg4);
					}
				}
				else
				{
					Set(key, arg3);
				}
			}
			else
			{
				Set(key, arg2);
			}
		}
		else
		{
			Set(key, arg);
		}
	}

	public void SetActorKey(string key)
	{
		Set("ActorKey", key);
	}

	public void SetLeftActorKey(string key)
	{
		Set("ConchShip_PresetKey_LeftActorKey", key);
	}

	public bool Contains<T>(string key)
	{
		if (typeof(T) == typeof(int) || typeof(T) == typeof(short) || typeof(T) == typeof(ushort) || typeof(T) == typeof(byte) || typeof(T) == typeof(sbyte))
		{
			return _intBox != null && _intBox.ContainsKey(key);
		}
		if (typeof(T) == typeof(float))
		{
			return _floatBox != null && _floatBox.ContainsKey(key);
		}
		if (typeof(T) == typeof(bool))
		{
			return _boolBox != null && _boolBox.ContainsKey(key);
		}
		if (typeof(T) == typeof(string))
		{
			return _stringBox != null && _stringBox.ContainsKey(key);
		}
		return _serializableObjectBox != null && _serializableObjectBox.ContainsKey(key);
	}

	public bool Get(string key, ref sbyte arg)
	{
		if (_intBox == null)
		{
			return false;
		}
		if (_intBox.TryGetValue(key, out var value))
		{
			arg = (sbyte)value;
			return true;
		}
		return false;
	}

	public bool Get(string key, ref byte arg)
	{
		if (_intBox == null)
		{
			return false;
		}
		if (_intBox.TryGetValue(key, out var value))
		{
			arg = (byte)value;
			return true;
		}
		return false;
	}

	public bool Get(string key, ref ushort arg)
	{
		if (_intBox == null)
		{
			return false;
		}
		if (_intBox.TryGetValue(key, out var value))
		{
			arg = (ushort)value;
			return true;
		}
		return false;
	}

	public bool Get(string key, ref short arg)
	{
		if (_intBox == null)
		{
			return false;
		}
		if (_intBox.TryGetValue(key, out var value))
		{
			arg = (short)value;
			return true;
		}
		return false;
	}

	public bool Get(string key, ref int arg)
	{
		if (_intBox == null)
		{
			return false;
		}
		return _intBox.TryGetValue(key, out arg);
	}

	public bool Get(string key, ref float arg)
	{
		if (_floatBox == null)
		{
			return false;
		}
		return _floatBox.TryGetValue(key, out arg);
	}

	public bool Get(string key, ref string arg)
	{
		if (_stringBox == null)
		{
			return false;
		}
		return _stringBox.TryGetValue(key, out arg);
	}

	public bool Get(string key, ref bool arg)
	{
		if (_boolBox == null)
		{
			return false;
		}
		return _boolBox.TryGetValue(key, out arg);
	}

	public bool Get<T>(string key, out T arg) where T : ISerializableGameData
	{
		arg = default(T);
		if (_serializableObjectBox == null)
		{
			return false;
		}
		ISerializableGameData value;
		bool flag = _serializableObjectBox.TryGetValue(key, out value);
		if (flag && value != null)
		{
			arg = (T)(object)value;
		}
		return arg != null && flag;
	}

	public sbyte GetSbyte(string key)
	{
		if (_intBox == null || !_intBox.ContainsKey(key))
		{
			return 0;
		}
		_intBox.TryGetValue(key, out var value);
		return (sbyte)value;
	}

	public byte GetByte(string key)
	{
		if (_intBox == null || !_intBox.ContainsKey(key))
		{
			return 0;
		}
		_intBox.TryGetValue(key, out var value);
		return (byte)value;
	}

	public short GetShort(string key)
	{
		if (_intBox == null || !_intBox.ContainsKey(key))
		{
			return 0;
		}
		_intBox.TryGetValue(key, out var value);
		return (short)value;
	}

	public ushort GetUshort(string key)
	{
		if (_intBox == null || !_intBox.ContainsKey(key))
		{
			return 0;
		}
		_intBox.TryGetValue(key, out var value);
		return (ushort)value;
	}

	public int GetInt(string key)
	{
		if (_intBox == null || !_intBox.ContainsKey(key))
		{
			return 0;
		}
		_intBox.TryGetValue(key, out var value);
		return value;
	}

	public float GetFloat(string key)
	{
		if (_floatBox == null || !_floatBox.ContainsKey(key))
		{
			return 0f;
		}
		_floatBox.TryGetValue(key, out var value);
		return value;
	}

	public string GetString(string key)
	{
		if (_stringBox == null || !_stringBox.ContainsKey(key))
		{
			return null;
		}
		_stringBox.TryGetValue(key, out var value);
		return value;
	}

	public bool GetBool(string key)
	{
		if (_boolBox == null || !_boolBox.ContainsKey(key))
		{
			return false;
		}
		_boolBox.TryGetValue(key, out var value);
		return value;
	}

	public T Get<T>(string key) where T : ISerializableGameData
	{
		if (_serializableObjectBox == null || !_serializableObjectBox.ContainsKey(key))
		{
			return default(T);
		}
		_serializableObjectBox.TryGetValue(key, out var value);
		return (T)(object)value;
	}

	public GameData.Domains.Character.Character GetCharacter(string key)
	{
		if (key == "RoleTaiwu")
		{
			return DomainManager.Taiwu.GetTaiwu();
		}
		GameData.Domains.Character.Character element = null;
		if (_intBox != null && _intBox.TryGetValue(key, out var value))
		{
			if (!DomainManager.Character.TryGetElement_Objects(value, out element))
			{
				AdaptableLog.Warning($"failed to get character {key} from ArgBox!curId in box is {value}");
			}
		}
		else
		{
			AdaptableLog.Warning("Failed to get character " + key + " from ArgBox.");
		}
		return element;
	}

	public DeadCharacter GetDeadCharacter(string key)
	{
		if (key == "RoleTaiwu")
		{
			return null;
		}
		if (_intBox == null || !_intBox.TryGetValue(key, out var value))
		{
			return null;
		}
		return DomainManager.Character.TryGetDeadCharacter(value);
	}

	public GameData.Domains.Character.Character GetAdventureMajorCharacter(int group, int index)
	{
		if (_intBox.TryGetValue($"MajorCharacter_{group}_{index}", out var value))
		{
			return DomainManager.Character.GetElement_Objects(value);
		}
		return null;
	}

	public int GetAdventureMajorCharacterCount(int group)
	{
		if (_intBox.TryGetValue($"MajorCharacter_{group}_Count", out var value))
		{
			return value;
		}
		return -1;
	}

	public GameData.Domains.Character.Character GetAdventureParticipateCharacter(int group, int index)
	{
		if (_intBox.TryGetValue($"ParticipateCharacter_{group}_{index}", out var value))
		{
			return DomainManager.Character.GetElement_Objects(value);
		}
		return null;
	}

	public int GetAdventureParticipateCharacterCount(int group)
	{
		if (_intBox.TryGetValue($"ParticipateCharacter_{group}_Count", out var value))
		{
			return value;
		}
		return -1;
	}

	public ItemBase GetItem(string key)
	{
		if (_serializableObjectBox.TryGetValue(key, out var value))
		{
			return DomainManager.Item.GetBaseItem((ItemKey)(object)value);
		}
		return null;
	}

	public EventArgBox()
	{
		_instancesCount++;
	}

	public EventArgBox(EventArgBox other)
	{
		_instancesCount++;
		if (other._intBox != null)
		{
			_intBox = new Dictionary<string, int>();
			foreach (KeyValuePair<string, int> item in other._intBox)
			{
				_intBox.Add(item.Key, item.Value);
			}
		}
		if (other._stringBox != null)
		{
			_stringBox = new Dictionary<string, string>();
			foreach (KeyValuePair<string, string> item2 in other._stringBox)
			{
				_stringBox.Add(item2.Key, item2.Value);
			}
		}
		if (other._floatBox != null)
		{
			_floatBox = new Dictionary<string, float>();
			foreach (KeyValuePair<string, float> item3 in other._floatBox)
			{
				_floatBox.Add(item3.Key, item3.Value);
			}
		}
		if (other._boolBox != null)
		{
			_boolBox = new Dictionary<string, bool>();
			foreach (KeyValuePair<string, bool> item4 in other._boolBox)
			{
				_boolBox.Add(item4.Key, item4.Value);
			}
		}
		if (other._serializableObjectBox == null)
		{
			return;
		}
		_serializableObjectBox = new Dictionary<string, ISerializableGameData>();
		_objectCollectionsCount++;
		foreach (KeyValuePair<string, ISerializableGameData> item5 in other._serializableObjectBox)
		{
			_serializableObjectBox.Add(item5.Key, item5.Value);
		}
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 10;
		if (_intBox != null)
		{
			foreach (KeyValuePair<string, int> item in _intBox)
			{
				num += 2 + 2 * item.Key.Length + 4;
			}
		}
		if (_stringBox != null)
		{
			foreach (KeyValuePair<string, string> item2 in _stringBox)
			{
				num += 2 + 2 * item2.Key.Length + 2 + 2 * item2.Value.Length;
			}
		}
		if (_floatBox != null)
		{
			foreach (KeyValuePair<string, float> item3 in _floatBox)
			{
				num += 2 + 2 * item3.Key.Length + 4;
			}
		}
		if (_boolBox != null)
		{
			foreach (KeyValuePair<string, bool> item4 in _boolBox)
			{
				num += 2 + 2 * item4.Key.Length + 1;
			}
		}
		if (_serializableObjectBox != null)
		{
			foreach (KeyValuePair<string, ISerializableGameData> item5 in _serializableObjectBox)
			{
				Type type = ((object)item5.Value).GetType();
				if (SerializeObjectMap.ContainsKey(type))
				{
					num += 3 + 2 * item5.Key.Length + item5.Value.GetSerializedSize();
				}
			}
		}
		return (num <= 4) ? num : ((num + 3) / 4 * 4);
	}

	private unsafe int WriteString(byte* pData, string target)
	{
		byte* ptr = pData;
		if (!string.IsNullOrEmpty(target))
		{
			int length = target.Length;
			Tester.Assert(length <= 65535);
			*(ushort*)ptr = (ushort)length;
			ptr += 2;
			fixed (char* ptr2 = target)
			{
				for (int i = 0; i < length; i++)
				{
					((short*)ptr)[i] = (short)ptr2[i];
				}
			}
			ptr += 2 * length;
		}
		else
		{
			*(short*)pData = 0;
			ptr += 2;
		}
		return (int)(ptr - pData);
	}

	private unsafe string ReadString(ref byte* pData)
	{
		ushort num = *(ushort*)pData;
		pData += 2;
		if (num > 0)
		{
			int num2 = 2 * num;
			string result = Encoding.Unicode.GetString(pData, num2);
			pData += num2;
			return result;
		}
		return string.Empty;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		if (_intBox != null)
		{
			*(ushort*)ptr = (ushort)_intBox.Count;
			ptr += 2;
			foreach (KeyValuePair<string, int> item in _intBox)
			{
				ptr += WriteString(ptr, item.Key);
				*(int*)ptr = item.Value;
				ptr += 4;
			}
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		if (_stringBox != null)
		{
			*(ushort*)ptr = (ushort)_stringBox.Count;
			ptr += 2;
			foreach (KeyValuePair<string, string> item2 in _stringBox)
			{
				ptr += WriteString(ptr, item2.Key);
				ptr += WriteString(ptr, item2.Value);
			}
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		if (_floatBox != null)
		{
			*(ushort*)ptr = (ushort)_floatBox.Count;
			ptr += 2;
			foreach (KeyValuePair<string, float> item3 in _floatBox)
			{
				ptr += WriteString(ptr, item3.Key);
				*(float*)ptr = item3.Value;
				ptr += 4;
			}
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		if (_boolBox != null)
		{
			*(ushort*)ptr = (ushort)_boolBox.Count;
			ptr += 2;
			foreach (KeyValuePair<string, bool> item4 in _boolBox)
			{
				ptr += WriteString(ptr, item4.Key);
				*ptr = (item4.Value ? ((byte)1) : ((byte)0));
				ptr++;
			}
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		if (_serializableObjectBox != null)
		{
			byte* ptr2 = ptr;
			ushort num = 0;
			ptr += 2;
			foreach (KeyValuePair<string, ISerializableGameData> item5 in _serializableObjectBox)
			{
				Type type = ((object)item5.Value).GetType();
				if (SerializeObjectMap.ContainsKey(type))
				{
					ptr += WriteString(ptr, item5.Key);
					*ptr = (byte)SerializeObjectMap[((object)item5.Value).GetType()];
					ptr++;
					ptr += item5.Value.Serialize(ptr);
					num++;
				}
			}
			*(ushort*)ptr2 = num;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		int num2 = (int)(ptr - pData);
		return (num2 <= 4) ? num2 : ((num2 + 3) / 4 * 4);
	}

	private void CreateSerializableObject(sbyte code, out ISerializableGameData obj)
	{
		switch (code)
		{
		case 0:
			obj = (ISerializableGameData)(object)default(Location);
			break;
		case 1:
			obj = (ISerializableGameData)(object)new AdventureMapPoint();
			break;
		case 2:
			obj = (ISerializableGameData)(object)default(ItemKey);
			break;
		case 3:
			obj = (ISerializableGameData)(object)new AdventureSiteData();
			break;
		case 4:
			obj = (ISerializableGameData)(object)default(MapTemplateEnemyInfo);
			break;
		case 5:
			obj = (ISerializableGameData)(object)new AvatarRelatedData();
			break;
		case 6:
			obj = (ISerializableGameData)(object)new EventActorData();
			break;
		case 7:
			obj = (ISerializableGameData)(object)ShortList.Create();
			break;
		case 8:
			obj = (ISerializableGameData)(object)new AvatarData();
			break;
		case 9:
			obj = (ISerializableGameData)(object)default(IntList);
			break;
		default:
			obj = null;
			break;
		}
	}

	private ISerializableGameData GetCopyOfSerializableObject(ISerializableGameData obj)
	{
		if (!(obj is Location location))
		{
			if (!(obj is AdventureMapPoint other))
			{
				if (!(obj is ItemKey itemKey))
				{
					if (!(obj is AdventureSiteData other2))
					{
						if (!(obj is MapTemplateEnemyInfo mapTemplateEnemyInfo))
						{
							if (!(obj is AvatarRelatedData other3))
							{
								if (!(obj is EventActorData other4))
								{
									if (obj is AvatarData other5)
									{
										return (ISerializableGameData)(object)new AvatarData(other5);
									}
									return null;
								}
								return (ISerializableGameData)(object)new EventActorData(other4);
							}
							return (ISerializableGameData)(object)new AvatarRelatedData(other3);
						}
						return (ISerializableGameData)(object)mapTemplateEnemyInfo;
					}
					return (ISerializableGameData)(object)new AdventureSiteData(other2);
				}
				return (ISerializableGameData)(object)itemKey;
			}
			AdventureMapPoint adventureMapPoint = new AdventureMapPoint();
			adventureMapPoint.Assign(other);
			return (ISerializableGameData)(object)adventureMapPoint;
		}
		return (ISerializableGameData)(object)location;
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		ushort num = *(ushort*)ptr;
		ptr += 2;
		if (num > 0)
		{
			if (_intBox == null)
			{
				_intBox = new Dictionary<string, int>();
			}
			for (int i = 0; i < num; i++)
			{
				string key = ReadString(ref ptr);
				int value = *(int*)ptr;
				ptr += 4;
				_intBox.Add(key, value);
			}
		}
		num = *(ushort*)ptr;
		ptr += 2;
		if (num > 0)
		{
			if (_stringBox == null)
			{
				_stringBox = new Dictionary<string, string>();
			}
			for (int j = 0; j < num; j++)
			{
				string key2 = ReadString(ref ptr);
				string value2 = ReadString(ref ptr);
				_stringBox.Add(key2, value2);
			}
		}
		num = *(ushort*)ptr;
		ptr += 2;
		if (num > 0)
		{
			if (_floatBox == null)
			{
				_floatBox = new Dictionary<string, float>();
			}
			for (int k = 0; k < num; k++)
			{
				string key3 = ReadString(ref ptr);
				float value3 = *(float*)ptr;
				ptr += 4;
				_floatBox.Add(key3, value3);
			}
		}
		num = *(ushort*)ptr;
		ptr += 2;
		if (num > 0)
		{
			if (_boolBox == null)
			{
				_boolBox = new Dictionary<string, bool>();
			}
			for (int l = 0; l < num; l++)
			{
				string key4 = ReadString(ref ptr);
				bool value4 = *ptr != 0;
				ptr++;
				_boolBox.Add(key4, value4);
			}
		}
		num = *(ushort*)ptr;
		ptr += 2;
		if (num > 0)
		{
			if (_serializableObjectBox == null)
			{
				_serializableObjectBox = new Dictionary<string, ISerializableGameData>();
			}
			for (int m = 0; m < num; m++)
			{
				string key5 = ReadString(ref ptr);
				sbyte code = (sbyte)(*ptr);
				ptr++;
				CreateSerializableObject(code, out var obj);
				if (obj != null)
				{
					ptr += obj.Deserialize(ptr);
					_serializableObjectBox.Add(key5, obj);
				}
			}
		}
		int num2 = (int)(ptr - pData);
		return (num2 <= 4) ? num2 : ((num2 + 3) / 4 * 4);
	}

	public void SetValueFromStack(EvaluationStack evaluationStack, string identifier, EValueType valueType)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_0004: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected I4, but got Unknown
		switch (valueType - 1)
		{
		case 0:
		{
			int arg5 = evaluationStack.PopUnmanaged<int>();
			Set(identifier, arg5);
			break;
		}
		case 1:
		{
			float arg4 = evaluationStack.PopUnmanaged<float>();
			Set(identifier, arg4);
			break;
		}
		case 2:
		{
			bool arg3 = evaluationStack.PopUnmanaged<bool>();
			Set(identifier, arg3);
			break;
		}
		case 3:
		{
			string arg2 = evaluationStack.PopObject<string>();
			Set(identifier, arg2);
			break;
		}
		case 4:
		{
			ISerializableGameData arg = evaluationStack.PopObject<ISerializableGameData>();
			Set(identifier, arg);
			break;
		}
		}
	}

	public ValueInfo SelectValue(Evaluator evaluator, string identifier)
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		int arg = 0;
		if (Get(identifier, ref arg))
		{
			return evaluator.PushEvaluationResult(arg);
		}
		bool arg2 = false;
		if (Get(identifier, ref arg2))
		{
			return evaluator.PushEvaluationResult(arg2);
		}
		float arg3 = 0f;
		if (Get(identifier, ref arg3))
		{
			return evaluator.PushEvaluationResult(arg3);
		}
		string arg4 = null;
		if (Get(identifier, ref arg4))
		{
			return evaluator.PushEvaluationResult(arg4);
		}
		if (this.Get<ISerializableGameData>(identifier, out ISerializableGameData arg5))
		{
			return evaluator.PushEvaluationResult((object)arg5);
		}
		return ValueInfo.Void;
	}
}
