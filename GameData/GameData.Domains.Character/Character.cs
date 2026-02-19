using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using CompDevLib.Interpreter;
using Config;
using Config.ConfigCells;
using Config.ConfigCells.Character;
using GameData.ArchiveData;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DLC;
using GameData.Dependencies;
using GameData.DomainEvents;
using GameData.Domains.Adventure;
using GameData.Domains.Building;
using GameData.Domains.Character.Ai;
using GameData.Domains.Character.Ai.GeneralAction.BehaviorAction;
using GameData.Domains.Character.Ai.GeneralAction.HealthDemand;
using GameData.Domains.Character.Ai.GeneralAction.LifeSkillRandom;
using GameData.Domains.Character.Ai.GeneralAction.SocialStatusRandom;
using GameData.Domains.Character.Ai.GeneralAction.StudyDemand;
using GameData.Domains.Character.Ai.GeneralAction.TeachRandom;
using GameData.Domains.Character.Ai.GeneralAction.WealthDemand;
using GameData.Domains.Character.Ai.PrioritizedAction;
using GameData.Domains.Character.AvatarSystem;
using GameData.Domains.Character.AvatarSystem.AvatarRes;
using GameData.Domains.Character.Creation;
using GameData.Domains.Character.Display;
using GameData.Domains.Character.Filters;
using GameData.Domains.Character.ParallelModifications;
using GameData.Domains.Character.Relation;
using GameData.Domains.Character.TemporaryModification;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.Extra;
using GameData.Domains.Global.Inscription;
using GameData.Domains.Information;
using GameData.Domains.Information.Collection;
using GameData.Domains.Item;
using GameData.Domains.Item.Filters;
using GameData.Domains.LifeRecord;
using GameData.Domains.Map;
using GameData.Domains.Organization;
using GameData.Domains.SpecialEffect;
using GameData.Domains.Taiwu;
using GameData.Domains.Taiwu.Profession;
using GameData.Domains.Taiwu.Profession.SkillsData;
using GameData.Domains.Taiwu.VillagerRole;
using GameData.Domains.TaiwuEvent;
using GameData.Domains.TaiwuEvent.DisplayEvent;
using GameData.Domains.TaiwuEvent.EventHelper;
using GameData.Domains.TaiwuEvent.MonthlyEventActions;
using GameData.Domains.World.MonthlyEvent;
using GameData.Domains.World.Notification;
using GameData.Serializer;
using GameData.Utilities;
using Redzen.Random;

namespace GameData.Domains.Character;

[SerializableGameData(NotForDisplayModule = true)]
public class Character : BaseGameDataObject, ISerializableGameData, IValueSelector
{
	internal class FixedFieldInfos
	{
		public const uint Id_Offset = 0u;

		public const int Id_Size = 4;

		public const uint TemplateId_Offset = 4u;

		public const int TemplateId_Size = 2;

		public const uint CreatingType_Offset = 6u;

		public const int CreatingType_Size = 1;

		public const uint Gender_Offset = 7u;

		public const int Gender_Size = 1;

		public const uint ActualAge_Offset = 8u;

		public const int ActualAge_Size = 2;

		public const uint BirthMonth_Offset = 10u;

		public const int BirthMonth_Size = 1;

		public const uint Happiness_Offset = 11u;

		public const int Happiness_Size = 1;

		public const uint BaseMorality_Offset = 12u;

		public const int BaseMorality_Size = 2;

		public const uint OrganizationInfo_Offset = 14u;

		public const int OrganizationInfo_Size = 8;

		public const uint IdealSect_Offset = 22u;

		public const int IdealSect_Size = 1;

		public const uint LifeSkillTypeInterest_Offset = 23u;

		public const int LifeSkillTypeInterest_Size = 1;

		public const uint CombatSkillTypeInterest_Offset = 24u;

		public const int CombatSkillTypeInterest_Size = 1;

		public const uint MainAttributeInterest_Offset = 25u;

		public const int MainAttributeInterest_Size = 1;

		public const uint Transgender_Offset = 26u;

		public const int Transgender_Size = 1;

		public const uint Bisexual_Offset = 27u;

		public const int Bisexual_Size = 1;

		public const uint XiangshuType_Offset = 28u;

		public const int XiangshuType_Size = 1;

		public const uint MonkType_Offset = 29u;

		public const int MonkType_Size = 1;

		public const uint BaseMainAttributes_Offset = 30u;

		public const int BaseMainAttributes_Size = 12;

		public const uint Health_Offset = 42u;

		public const int Health_Size = 2;

		public const uint BaseMaxHealth_Offset = 44u;

		public const int BaseMaxHealth_Size = 2;

		public const uint DisorderOfQi_Offset = 46u;

		public const int DisorderOfQi_Size = 2;

		public const uint HaveLeftArm_Offset = 48u;

		public const int HaveLeftArm_Size = 1;

		public const uint HaveRightArm_Offset = 49u;

		public const int HaveRightArm_Size = 1;

		public const uint HaveLeftLeg_Offset = 50u;

		public const int HaveLeftLeg_Size = 1;

		public const uint HaveRightLeg_Offset = 51u;

		public const int HaveRightLeg_Size = 1;

		public const uint Injuries_Offset = 52u;

		public const int Injuries_Size = 16;

		public const uint ExtraNeili_Offset = 68u;

		public const int ExtraNeili_Size = 4;

		public const uint ConsummateLevel_Offset = 72u;

		public const int ConsummateLevel_Size = 1;

		public const uint BaseLifeSkillQualifications_Offset = 73u;

		public const int BaseLifeSkillQualifications_Size = 32;

		public const uint LifeSkillQualificationGrowthType_Offset = 105u;

		public const int LifeSkillQualificationGrowthType_Size = 1;

		public const uint BaseCombatSkillQualifications_Offset = 106u;

		public const int BaseCombatSkillQualifications_Size = 28;

		public const uint CombatSkillQualificationGrowthType_Offset = 134u;

		public const int CombatSkillQualificationGrowthType_Size = 1;

		public const uint Resources_Offset = 135u;

		public const int Resources_Size = 32;

		public const uint LovingItemSubType_Offset = 167u;

		public const int LovingItemSubType_Size = 2;

		public const uint HatingItemSubType_Offset = 169u;

		public const int HatingItemSubType_Size = 2;

		public const uint FullName_Offset = 171u;

		public const int FullName_Size = 10;

		public const uint MonasticTitle_Offset = 181u;

		public const int MonasticTitle_Size = 4;

		public const uint Avatar_Offset = 185u;

		public const int Avatar_Size = 76;

		public const uint Genome_Offset = 261u;

		public const int Genome_Size = 64;

		public const uint CurrMainAttributes_Offset = 325u;

		public const int CurrMainAttributes_Size = 12;

		public const uint Poisoned_Offset = 337u;

		public const int Poisoned_Size = 24;

		public const uint InjuriesRecoveryProgress_Offset = 361u;

		public const int InjuriesRecoveryProgress_Size = 14;

		public const uint CurrNeili_Offset = 375u;

		public const int CurrNeili_Size = 4;

		public const uint LoopingNeigong_Offset = 379u;

		public const int LoopingNeigong_Size = 2;

		public const uint BaseNeiliAllocation_Offset = 381u;

		public const int BaseNeiliAllocation_Size = 8;

		public const uint ExtraNeiliAllocation_Offset = 389u;

		public const int ExtraNeiliAllocation_Size = 8;

		public const uint BaseNeiliProportionOfFiveElements_Offset = 397u;

		public const int BaseNeiliProportionOfFiveElements_Size = 8;

		public const uint HobbyExpirationDate_Offset = 405u;

		public const int HobbyExpirationDate_Size = 4;

		public const uint LovingItemRevealed_Offset = 409u;

		public const int LovingItemRevealed_Size = 1;

		public const uint HatingItemRevealed_Offset = 410u;

		public const int HatingItemRevealed_Size = 1;

		public const uint LegitimateBoysCount_Offset = 411u;

		public const int LegitimateBoysCount_Size = 1;

		public const uint BirthLocation_Offset = 412u;

		public const int BirthLocation_Size = 4;

		public const uint Location_Offset = 416u;

		public const int Location_Size = 4;

		public const uint Equipment_Offset = 420u;

		public const int Equipment_Size = 96;

		public const uint EatingItems_Offset = 516u;

		public const int EatingItems_Size = 90;

		public const uint EquippedCombatSkills_Offset = 606u;

		public const int EquippedCombatSkills_Size = 96;

		public const uint CombatSkillAttainmentPanels_Offset = 702u;

		public const int CombatSkillAttainmentPanels_Size = 252;

		public const uint PreexistenceCharIds_Offset = 954u;

		public const int PreexistenceCharIds_Size = 52;

		public const uint XiangshuInfection_Offset = 1006u;

		public const int XiangshuInfection_Size = 1;

		public const uint CurrAge_Offset = 1007u;

		public const int CurrAge_Size = 2;

		public const uint Exp_Offset = 1009u;

		public const int Exp_Size = 4;

		public const uint ExternalRelationState_Offset = 1013u;

		public const int ExternalRelationState_Size = 1;

		public const uint KidnapperId_Offset = 1014u;

		public const int KidnapperId_Size = 4;

		public const uint LeaderId_Offset = 1018u;

		public const int LeaderId_Size = 4;

		public const uint FactionId_Offset = 1022u;

		public const int FactionId_Size = 4;

		public const uint ActionEnergies_Offset = 1026u;

		public const int ActionEnergies_Size = 5;

		public const uint PrioritizedActionCooldowns_Offset = 1031u;

		public const int PrioritizedActionCooldowns_Size = 9;
	}

	public class ProtagonistFeatureRelatedStatus
	{
		public List<GameData.Domains.CombatSkill.CombatSkill> CombatSkills;

		public bool AddFeatureDreamLover;

		public bool AddFeatureLifeSkillLearning;

		public bool AddFeatureCombatSkillLearning;

		public bool AddFeatureWhiteSnake;

		public bool AddFeatureLongevity;

		public bool CreateCloseFriend;

		public short ReadLifeSkillTemplateId;

		public short ReadCombatSkillTemplateId;

		public byte CombatSkillBookPageTypes;

		public ProtagonistFeatureRelatedStatus(List<GameData.Domains.CombatSkill.CombatSkill> combatSkills)
		{
			CombatSkills = combatSkills;
			AddFeatureDreamLover = false;
			AddFeatureLifeSkillLearning = false;
			AddFeatureCombatSkillLearning = false;
			AddFeatureWhiteSnake = false;
			CreateCloseFriend = false;
			ReadLifeSkillTemplateId = -1;
			ReadCombatSkillTemplateId = -1;
			CombatSkillBookPageTypes = 0;
		}
	}

	[CollectionObjectField(false, true, false, true, false)]
	private int _id;

	[CollectionObjectField(true, true, false, true, false)]
	private short _templateId;

	[CollectionObjectField(true, true, false, true, false)]
	private byte _creatingType;

	[CollectionObjectField(true, true, false, true, false)]
	private sbyte _gender;

	[CollectionObjectField(true, true, false, false, false)]
	private short _actualAge;

	[CollectionObjectField(true, true, false, true, false)]
	private sbyte _birthMonth;

	[CollectionObjectField(true, true, false, false, false)]
	private sbyte _happiness;

	[CollectionObjectField(true, true, false, false, false)]
	private short _baseMorality;

	[CollectionObjectField(true, true, false, false, false)]
	private OrganizationInfo _organizationInfo;

	[CollectionObjectField(true, true, false, false, false)]
	private sbyte _idealSect;

	[CollectionObjectField(true, true, false, false, false)]
	private sbyte _lifeSkillTypeInterest;

	[CollectionObjectField(true, true, false, false, false)]
	private sbyte _combatSkillTypeInterest;

	[CollectionObjectField(true, true, false, false, false)]
	private sbyte _mainAttributeInterest;

	[CollectionObjectField(true, true, false, true, false)]
	private bool _transgender;

	[CollectionObjectField(true, true, false, true, false)]
	private bool _bisexual;

	[CollectionObjectField(true, true, false, true, false)]
	private sbyte _xiangshuType;

	[CollectionObjectField(true, true, false, false, false)]
	private byte _monkType;

	[CollectionObjectField(true, true, false, false, false)]
	private List<short> _featureIds;

	[CollectionObjectField(true, true, false, false, false)]
	private MainAttributes _baseMainAttributes;

	[CollectionObjectField(true, true, false, false, false)]
	private short _health;

	[CollectionObjectField(true, true, false, false, false)]
	private short _baseMaxHealth;

	[CollectionObjectField(true, true, false, false, false)]
	private short _disorderOfQi;

	[CollectionObjectField(true, true, false, false, false)]
	private bool _haveLeftArm;

	[CollectionObjectField(true, true, false, false, false)]
	private bool _haveRightArm;

	[CollectionObjectField(true, true, false, false, false)]
	private bool _haveLeftLeg;

	[CollectionObjectField(true, true, false, false, false)]
	private bool _haveRightLeg;

	[CollectionObjectField(true, true, false, false, false)]
	private Injuries _injuries;

	[CollectionObjectField(true, true, false, false, false)]
	private int _extraNeili;

	[CollectionObjectField(true, true, false, false, false)]
	private sbyte _consummateLevel;

	[CollectionObjectField(true, true, false, false, false)]
	private List<LifeSkillItem> _learnedLifeSkills;

	[CollectionObjectField(true, true, false, false, false)]
	private LifeSkillShorts _baseLifeSkillQualifications;

	[CollectionObjectField(true, true, false, false, false)]
	private sbyte _lifeSkillQualificationGrowthType;

	[CollectionObjectField(true, true, false, false, false)]
	private CombatSkillShorts _baseCombatSkillQualifications;

	[CollectionObjectField(true, true, false, false, false)]
	private sbyte _combatSkillQualificationGrowthType;

	[CollectionObjectField(true, true, false, false, false)]
	private ResourceInts _resources;

	[CollectionObjectField(true, true, false, false, false)]
	private short _lovingItemSubType;

	[CollectionObjectField(true, true, false, false, false)]
	private short _hatingItemSubType;

	[CollectionObjectField(false, true, false, false, false)]
	private FullName _fullName;

	[CollectionObjectField(false, true, false, false, false)]
	private MonasticTitle _monasticTitle;

	[CollectionObjectField(false, true, false, false, false)]
	private AvatarData _avatar;

	[CollectionObjectField(false, true, false, false, false)]
	private List<short> _potentialFeatureIds;

	[CollectionObjectField(false, true, false, false, false)]
	private List<FameActionRecord> _fameActionRecords;

	[CollectionObjectField(false, true, false, true, false)]
	private Genome _genome;

	[CollectionObjectField(false, true, false, false, false)]
	private MainAttributes _currMainAttributes;

	[CollectionObjectField(false, true, false, false, false)]
	private PoisonInts _poisoned;

	[CollectionObjectField(false, true, false, false, false, ArrayElementsCount = 14)]
	private byte[] _injuriesRecoveryProgress;

	[CollectionObjectField(false, true, false, false, false)]
	private int _currNeili;

	[CollectionObjectField(false, true, false, false, false)]
	private short _loopingNeigong;

	[CollectionObjectField(false, true, false, false, false)]
	private NeiliAllocation _baseNeiliAllocation;

	[CollectionObjectField(false, true, false, false, false)]
	private NeiliAllocation _extraNeiliAllocation;

	[CollectionObjectField(false, true, false, false, false)]
	private NeiliProportionOfFiveElements _baseNeiliProportionOfFiveElements;

	[CollectionObjectField(false, true, false, false, false)]
	private int _hobbyExpirationDate;

	[CollectionObjectField(false, true, false, false, false)]
	private bool _lovingItemRevealed;

	[CollectionObjectField(false, true, false, false, false)]
	private bool _hatingItemRevealed;

	[CollectionObjectField(false, true, false, false, false)]
	private sbyte _legitimateBoysCount;

	[CollectionObjectField(false, true, false, true, false)]
	private Location _birthLocation;

	[CollectionObjectField(false, true, false, false, false)]
	private Location _location;

	[CollectionObjectField(false, true, false, false, false, ArrayElementsCount = 12)]
	private ItemKey[] _equipment;

	[CollectionObjectField(false, true, false, false, false)]
	private Inventory _inventory;

	[CollectionObjectField(false, true, false, false, false)]
	private EatingItems _eatingItems;

	[CollectionObjectField(false, true, false, false, false)]
	private List<short> _learnedCombatSkills;

	[CollectionObjectField(false, true, false, false, false, ArrayElementsCount = 48)]
	private short[] _equippedCombatSkills;

	[CollectionObjectField(false, true, false, false, false, ArrayElementsCount = 126)]
	private short[] _combatSkillAttainmentPanels;

	[CollectionObjectField(false, true, false, false, false)]
	private List<SkillQualificationBonus> _skillQualificationBonuses;

	[CollectionObjectField(false, true, false, true, false)]
	private PreexistenceCharIds _preexistenceCharIds;

	[CollectionObjectField(false, true, false, false, false)]
	private byte _xiangshuInfection;

	[CollectionObjectField(false, true, false, false, false)]
	private short _currAge;

	[CollectionObjectField(false, true, false, false, false)]
	private int _exp;

	[CollectionObjectField(false, true, false, false, false)]
	private byte _externalRelationState;

	[CollectionObjectField(false, true, false, false, false)]
	private int _kidnapperId;

	[CollectionObjectField(false, true, false, false, false)]
	private int _leaderId;

	[CollectionObjectField(false, true, false, false, false)]
	private int _factionId;

	[CollectionObjectField(false, true, false, false, false)]
	private List<GameData.Domains.Character.Ai.PersonalNeed> _personalNeeds;

	[CollectionObjectField(false, true, false, false, false)]
	private ActionEnergySbytes _actionEnergies;

	[CollectionObjectField(false, true, false, false, false)]
	private List<NpcTravelTarget> _npcTravelTargets;

	[CollectionObjectField(false, true, false, false, false)]
	private PrioritizedActionCooldownSbytes _prioritizedActionCooldowns;

	[CollectionObjectField(false, false, true, false, false)]
	private short _physiologicalAge;

	[CollectionObjectField(false, false, true, false, false)]
	private sbyte _fame;

	[CollectionObjectField(false, false, true, false, false)]
	private short _morality;

	[CollectionObjectField(false, false, true, false, false)]
	private short _attraction;

	[CollectionObjectField(false, false, true, false, false)]
	private MainAttributes _maxMainAttributes;

	[CollectionObjectField(false, false, true, false, false)]
	private HitOrAvoidInts _hitValues;

	[CollectionObjectField(false, false, true, false, false)]
	private OuterAndInnerInts _penetrations;

	[CollectionObjectField(false, false, true, false, false)]
	private HitOrAvoidInts _avoidValues;

	[CollectionObjectField(false, false, true, false, false)]
	private OuterAndInnerInts _penetrationResists;

	[CollectionObjectField(false, false, true, false, false)]
	private OuterAndInnerShorts _recoveryOfStanceAndBreath;

	[CollectionObjectField(false, false, true, false, false)]
	private short _moveSpeed;

	[CollectionObjectField(false, false, true, false, false)]
	private short _recoveryOfFlaw;

	[CollectionObjectField(false, false, true, false, false)]
	private short _castSpeed;

	[CollectionObjectField(false, false, true, false, false)]
	private short _recoveryOfBlockedAcupoint;

	[CollectionObjectField(false, false, true, false, false)]
	private short _weaponSwitchSpeed;

	[CollectionObjectField(false, false, true, false, false)]
	private short _attackSpeed;

	[CollectionObjectField(false, false, true, false, false)]
	private short _innerRatio;

	[CollectionObjectField(false, false, true, false, false)]
	private short _recoveryOfQiDisorder;

	[CollectionObjectField(false, false, true, false, false)]
	private PoisonInts _poisonResists;

	[CollectionObjectField(false, false, true, false, false)]
	private short _maxHealth;

	[CollectionObjectField(false, false, true, false, false)]
	private short _fertility;

	[CollectionObjectField(false, false, true, false, false)]
	private LifeSkillShorts _lifeSkillQualifications;

	[CollectionObjectField(false, false, true, false, false)]
	private LifeSkillShorts _lifeSkillAttainments;

	[CollectionObjectField(false, false, true, false, false)]
	private CombatSkillShorts _combatSkillQualifications;

	[CollectionObjectField(false, false, true, false, false)]
	private CombatSkillShorts _combatSkillAttainments;

	[CollectionObjectField(false, false, true, false, false)]
	private Personalities _personalities;

	[CollectionObjectField(false, false, true, false, false)]
	private sbyte _hobbyChangingPeriod;

	[CollectionObjectField(false, false, true, false, false)]
	private OuterAndInnerShorts _favorabilityChangingFactor;

	[CollectionObjectField(false, false, true, false, false)]
	private int _maxInventoryLoad;

	[CollectionObjectField(false, false, true, false, false)]
	private int _currInventoryLoad;

	[CollectionObjectField(false, false, true, false, false)]
	private int _maxEquipmentLoad;

	[CollectionObjectField(false, false, true, false, false)]
	private int _currEquipmentLoad;

	[CollectionObjectField(false, false, true, false, false)]
	private int _inventoryTotalValue;

	[CollectionObjectField(false, false, true, false, false)]
	private int _maxNeili;

	[CollectionObjectField(false, false, true, false, false)]
	private NeiliAllocation _neiliAllocation;

	[CollectionObjectField(false, false, true, false, false)]
	private NeiliProportionOfFiveElements _neiliProportionOfFiveElements;

	[CollectionObjectField(false, false, true, false, false)]
	private sbyte _neiliType;

	[CollectionObjectField(false, false, true, false, false)]
	private int _combatPower;

	[CollectionObjectField(false, false, true, false, false)]
	private sbyte _attackTendencyOfInnerAndOuter;

	[CollectionObjectField(false, false, true, false, false)]
	private NeiliAllocation _allocatedNeiliEffects;

	[CollectionObjectField(false, false, true, false, false)]
	private sbyte _maxConsummateLevel;

	[CollectionObjectField(false, false, true, false, false)]
	private CombatSkillEquipment _combatSkillEquipment;

	[CollectionObjectField(false, false, true, false, false)]
	private uint _darkAshProtector;

	private const int BasePersonality = 10;

	private const int PreexistenceAttributeBonusDivisor = 10;

	private const int BaseFertility = 100;

	private static readonly List<(short min, short max)> RandomFavorabilityRanges = new List<(short, short)>
	{
		(-30000, -18000),
		(-17999, 0),
		(1, 17999),
		(18000, 30000)
	};

	public const int InnateSkillQualificationBonusesCount = 2;

	public const int AcquiredSkillQualificationBonusesMaxCount = 9;

	public static readonly List<ECharacterPropertyReferencedType> HitAvoidAttackDefendPropertyTypes = new List<ECharacterPropertyReferencedType>
	{
		ECharacterPropertyReferencedType.HitRateStrength,
		ECharacterPropertyReferencedType.HitRateTechnique,
		ECharacterPropertyReferencedType.HitRateSpeed,
		ECharacterPropertyReferencedType.HitRateMind,
		ECharacterPropertyReferencedType.AvoidRateStrength,
		ECharacterPropertyReferencedType.AvoidRateTechnique,
		ECharacterPropertyReferencedType.AvoidRateSpeed,
		ECharacterPropertyReferencedType.AvoidRateMind,
		ECharacterPropertyReferencedType.PenetrateOfOuter,
		ECharacterPropertyReferencedType.PenetrateOfInner,
		ECharacterPropertyReferencedType.PenetrateResistOfOuter,
		ECharacterPropertyReferencedType.PenetrateResistOfInner
	};

	public static readonly List<ECharacterPropertyReferencedType> CombatPropertyTypes = new List<ECharacterPropertyReferencedType>(HitAvoidAttackDefendPropertyTypes)
	{
		ECharacterPropertyReferencedType.Strength,
		ECharacterPropertyReferencedType.Dexterity,
		ECharacterPropertyReferencedType.Concentration,
		ECharacterPropertyReferencedType.Vitality,
		ECharacterPropertyReferencedType.Energy,
		ECharacterPropertyReferencedType.Intelligence,
		ECharacterPropertyReferencedType.RecoveryOfStance,
		ECharacterPropertyReferencedType.RecoveryOfBreath,
		ECharacterPropertyReferencedType.RecoveryOfFlaw,
		ECharacterPropertyReferencedType.RecoveryOfBlockedAcupoint,
		ECharacterPropertyReferencedType.RecoveryOfQiDisorder,
		ECharacterPropertyReferencedType.MoveSpeed,
		ECharacterPropertyReferencedType.CastSpeed,
		ECharacterPropertyReferencedType.AttackSpeed,
		ECharacterPropertyReferencedType.WeaponSwitchSpeed,
		ECharacterPropertyReferencedType.InnerRatio
	};

	public static readonly List<ECharacterPropertyReferencedType> BonusPropertyTypes = new List<ECharacterPropertyReferencedType>(CombatPropertyTypes)
	{
		ECharacterPropertyReferencedType.QualificationAppraisal,
		ECharacterPropertyReferencedType.QualificationBlade,
		ECharacterPropertyReferencedType.QualificationBuddhism,
		ECharacterPropertyReferencedType.QualificationChess,
		ECharacterPropertyReferencedType.QualificationCooking,
		ECharacterPropertyReferencedType.QualificationEclectic,
		ECharacterPropertyReferencedType.QualificationFinger,
		ECharacterPropertyReferencedType.QualificationForging,
		ECharacterPropertyReferencedType.QualificationJade,
		ECharacterPropertyReferencedType.QualificationLeg,
		ECharacterPropertyReferencedType.QualificationMath,
		ECharacterPropertyReferencedType.QualificationMedicine,
		ECharacterPropertyReferencedType.QualificationMusic,
		ECharacterPropertyReferencedType.QualificationNeigong,
		ECharacterPropertyReferencedType.QualificationPainting,
		ECharacterPropertyReferencedType.QualificationPoem,
		ECharacterPropertyReferencedType.QualificationPolearm,
		ECharacterPropertyReferencedType.QualificationPosing,
		ECharacterPropertyReferencedType.QualificationSpecial,
		ECharacterPropertyReferencedType.QualificationStunt,
		ECharacterPropertyReferencedType.QualificationSword,
		ECharacterPropertyReferencedType.QualificationTaoism,
		ECharacterPropertyReferencedType.QualificationThrow,
		ECharacterPropertyReferencedType.QualificationToxicology,
		ECharacterPropertyReferencedType.QualificationWeaving,
		ECharacterPropertyReferencedType.QualificationWhip,
		ECharacterPropertyReferencedType.QualificationWoodworking,
		ECharacterPropertyReferencedType.QualificationCombatMusic,
		ECharacterPropertyReferencedType.QualificationControllableShot,
		ECharacterPropertyReferencedType.QualificationFistAndPalm
	};

	public static readonly IReadOnlyList<EHealActionType> AllHealActions = new EHealActionType[4]
	{
		EHealActionType.Healing,
		EHealActionType.Detox,
		EHealActionType.Breathing,
		EHealActionType.Recover
	};

	private byte _advanceMonthStatus;

	private const sbyte SearchRandomDestinationMaxCount = 10;

	public const int FixedSize = 1040;

	public const int DynamicCount = 9;

	private SpinLock _spinLock = new SpinLock(enableThreadOwnerTracking: false);

	private int _srcCharId = -1;

	private static readonly (TemplateKey templateKey, int amount)[] ProtagonistInitialItems = new(TemplateKey, int)[9]
	{
		(new TemplateKey(8, 54), 3),
		(new TemplateKey(8, 60), 2),
		(new TemplateKey(8, 58), 1),
		(new TemplateKey(8, 66), 3),
		(new TemplateKey(8, 72), 2),
		(new TemplateKey(8, 70), 1),
		(new TemplateKey(8, 88), 3),
		(new TemplateKey(8, 94), 3),
		(new TemplateKey(6, 0), 1)
	};

	private static readonly sbyte[][] LovingAndHatingSectsCandidates = new sbyte[5][]
	{
		new sbyte[3] { 5, 8, 14 },
		new sbyte[5] { 1, 2, 3, 5, 8 },
		new sbyte[9] { 1, 2, 3, 4, 6, 7, 9, 12, 13 },
		new sbyte[6] { 6, 10, 11, 12, 13, 15 },
		new sbyte[3] { 10, 11, 15 }
	};

	private readonly List<ItemKey> _itemsToBeDeleted = new List<ItemKey>();

	private static readonly short[] ProtagonistLifeSkillBookIds = new short[16]
	{
		6, 15, 24, 33, 42, 51, 60, 69, 78, 87,
		96, 105, 114, 123, 132, 141
	};

	private static readonly short[][] ProtagonistCombatSkillBookIds = new short[15][]
	{
		new short[3] { 351, 471, 773 },
		new short[4] { 360, 479, 556, 675 },
		new short[2] { 369, 564 },
		new short[3] { 378, 486, 682 },
		new short[4] { 386, 619, 691, 731 },
		new short[3] { 495, 739, 782 },
		new short[3] { 399, 573, 699 },
		new short[3] { 407, 504, 580 },
		new short[4] { 414, 708, 748, 790 },
		new short[2] { 421, 628 },
		new short[3] { 430, 517, 756 },
		new short[4] { 438, 525, 594, 716 },
		new short[3] { 603, 653, 723 },
		new short[3] { 454, 533, 765 },
		new short[3] { 542, 612, 636 }
	};

	public bool CanNotSpeak
	{
		get
		{
			bool flag = DomainManager.Character.GetSkeletonSourceGraveId(_id) >= 0 || !Config.Character.Instance[_templateId].CanSpeak;
			bool flag2 = flag;
			if (!flag2)
			{
				short templateId = _equipment[4].TemplateId;
				bool flag3 = (uint)(templateId - 69) <= 2u;
				flag2 = flag3;
			}
			return flag2;
		}
	}

	public bool CanAffectedByCombatDifficulty => !IsTaiwu() && _leaderId != DomainManager.Taiwu.GetTaiwuCharId() && !DomainManager.Combat.GetTaiwuSpecialGroupCharIds().Contains(_id);

	public bool IsGearMate => GetTemplateId() == 836;

	public CharacterItem Template => Config.Character.Instance[_templateId];

	public bool IsFavorabilityGainFixed
	{
		get
		{
			int result;
			if (_creatingType == 1)
			{
				short templateId = _templateId;
				result = ((templateId <= 466 && templateId >= 463) ? 1 : 0);
			}
			else
			{
				result = 1;
			}
			return (byte)result != 0;
		}
	}

	public int DarkAshDuration
	{
		get
		{
			int temporaryFeatureExpireDate = DomainManager.Extra.GetTemporaryFeatureExpireDate(_id, 758);
			return (temporaryFeatureExpireDate != -1) ? (temporaryFeatureExpireDate - DomainManager.World.GetCurrDate()) : (-1);
		}
	}

	public int SaveFromInfectedGainFaith => GlobalConfig.FuyuFaithCountBySaveInfected[Math.Clamp(_consummateLevel, 0, GlobalConfig.FuyuFaithCountBySaveInfected.Length - 1)];

	public bool IsOverweight => GetCurrInventoryLoad() > GetMaxInventoryLoad();

	[ObjectCollectionDependency(4, 0, new ushort[] { 66 }, Scope = InfluenceScope.Self)]
	[ObjectCollectionDependency(17, 2, new ushort[] { 25 }, Scope = InfluenceScope.CharacterAffectedByTheSpecialEffects)]
	private short CalcPhysiologicalAge()
	{
		short currAge = _currAge;
		return (short)DomainManager.SpecialEffect.ModifyData(_id, (short)(-1), (ushort)25, (int)currAge, -1, -1, -1);
	}

	[ObjectCollectionDependency(4, 0, new ushort[] { 17, 8 }, Scope = InfluenceScope.Self)]
	private uint CalcDarkAshProtector()
	{
		ECharacterFeatureDarkAshProtector eCharacterFeatureDarkAshProtector = ((_organizationInfo.OrgTemplateId == 16) ? ECharacterFeatureDarkAshProtector.IsTaiwu : ECharacterFeatureDarkAshProtector.None);
		foreach (short featureId in _featureIds)
		{
			eCharacterFeatureDarkAshProtector |= CharacterFeature.Instance[featureId].DarkAshProtector;
		}
		return (uint)eCharacterFeatureDarkAshProtector;
	}

	[ObjectCollectionDependency(4, 0, new ushort[] { 8, 41, 17 }, Scope = InfluenceScope.Self)]
	[SingleValueDependency(1, new ushort[] { 27 })]
	private sbyte CalcFame()
	{
		CharacterItem characterItem = Config.Character.Instance[_templateId];
		if (_creatingType != 1)
		{
			return characterItem.PresetFame;
		}
		int num = 0;
		bool isTaiwu = IsTaiwu();
		foreach (short featureId in _featureIds)
		{
			num += SharedMethods.GetSectFeatureFameBonus(featureId, isTaiwu, _organizationInfo);
		}
		int currDate = DomainManager.World.GetCurrDate();
		int i = 0;
		for (int count = _fameActionRecords.Count; i < count; i++)
		{
			FameActionRecord fameActionRecord = _fameActionRecords[i];
			if (fameActionRecord.EndDate > currDate)
			{
				num += fameActionRecord.Value;
			}
		}
		return (sbyte)Math.Clamp(num, -100, 100);
	}

	[ObjectCollectionDependency(4, 0, new ushort[] { 17, 7, 56 }, Scope = InfluenceScope.Self)]
	[SingleValueDependency(4, new ushort[] { 27, 26 })]
	private short CalcMorality()
	{
		short fixedMorality = GetFixedMorality();
		return (fixedMorality != short.MaxValue) ? fixedMorality : Math.Clamp(_baseMorality, (short)(-500), (short)500);
	}

	[ObjectCollectionDependency(4, 0, new ushort[] { 17, 57, 59, 66, 76, 39, 61 }, Scope = InfluenceScope.Self)]
	[SingleValueCollectionDependency(19, new ushort[] { 13, 150, 121 }, Scope = InfluenceScope.TaiwuChar)]
	[ObjectCollectionDependency(6, 3, new ushort[] { 4 }, Scope = InfluenceScope.CharWhoEquippedTheItem, Condition = InfluenceCondition.ItemIsEquipped)]
	private short CalcAttraction()
	{
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		if (GetAgeGroup() != 2)
		{
			return GlobalConfig.Instance.ImmaturityAttraction;
		}
		int num = CalcAvatarAttraction();
		num *= CalcPropertyModify(ECharacterPropertyReferencedType.Attraction, (EDataSumType)0);
		if (!IsCreatedWithFixedTemplate())
		{
			ItemKey itemKey = _equipment[4];
			if (!itemKey.IsValid() || DomainManager.Item.GetBaseItem(itemKey).IsDurabilityRunningOut())
			{
				num /= 2;
			}
		}
		return (short)Math.Clamp(num, 0, 900);
	}

	[ObjectCollectionDependency(4, 0, new ushort[] { 17, 57, 59, 18, 76 }, Scope = InfluenceScope.Self)]
	[ObjectCollectionDependency(7, 0, new ushort[] { 3 }, Scope = InfluenceScope.CombatSkillOwner)]
	[ObjectCollectionDependency(17, 2, new ushort[] { 1, 2, 3, 4, 5, 6, 293 }, Scope = InfluenceScope.CharacterAffectedByTheSpecialEffects)]
	[SingleValueDependency(5, new ushort[] { 0 })]
	[SingleValueDependency(9, new ushort[] { 16 }, Scope = InfluenceScope.TaiwuChar)]
	[SingleValueCollectionDependency(19, new ushort[] { 150 }, Scope = InfluenceScope.TaiwuChar)]
	[SingleValueDependency(19, new ushort[] { 246 }, Scope = InfluenceScope.TaiwuChar)]
	private unsafe MainAttributes CalcMaxMainAttributes()
	{
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		MainAttributes baseMainAttributes = _baseMainAttributes;
		short physiologicalAge = GetPhysiologicalAge();
		short clampedAgeOfAgeEffect = GetClampedAgeOfAgeEffect(physiologicalAge);
		MainAttributes mainAttributes = AgeEffect.Instance[clampedAgeOfAgeEffect].MainAttributes;
		for (int i = 0; i < 6; i++)
		{
			ECharacterPropertyReferencedType propertyType = (ECharacterPropertyReferencedType)(0 + i);
			ushort fieldId = (ushort)(1 + i);
			CValueModify val = CalcPropertyModify(propertyType, (EDataSumType)0);
			val += DomainManager.SpecialEffect.GetModify(_id, fieldId, -1, -1, -1, (EDataSumType)0);
			baseMainAttributes[i] = (short)Math.Clamp((int)baseMainAttributes.Items[i] * val * CValuePercent.op_Implicit((int)mainAttributes[i]), GlobalConfig.Instance.MinValueOfMaxMainAttributes, GlobalConfig.Instance.MaxValueOfMaxMainAttributes);
		}
		return baseMainAttributes;
	}

	[ObjectCollectionDependency(4, 0, new ushort[] { 17, 57, 59, 61, 80, 112, 115, 28 }, Scope = InfluenceScope.Self)]
	[ObjectCollectionDependency(7, 0, new ushort[] { 3, 5, 1, 10 }, Scope = InfluenceScope.CombatSkillOwner)]
	[SingleValueCollectionDependency(5, new ushort[] { 20 }, Scope = InfluenceScope.TaiwuChar)]
	[ElementListDependency(5, 16, 9, Scope = InfluenceScope.TaiwuChar)]
	[SingleValueCollectionDependency(19, new ushort[] { 121 }, Scope = InfluenceScope.TaiwuChar)]
	[SingleValueDependency(5, new ushort[] { 0 })]
	[SingleValueDependency(1, new ushort[] { 15 })]
	[ObjectCollectionDependency(17, 2, new ushort[] { 32, 33, 34, 35, 36, 37, 236, 277, 293 }, Scope = InfluenceScope.CharacterAffectedByTheSpecialEffects)]
	[ElementListDependency(11, 0, 14)]
	[SingleValueCollectionDependency(19, new ushort[] { 245 }, Scope = InfluenceScope.TaiwuChar)]
	[SingleValueDependency(19, new ushort[] { 46, 47, 246, 123 }, Scope = InfluenceScope.TaiwuChar)]
	[SingleValueDependency(8, new ushort[] { 19 }, Scope = InfluenceScope.AllCharsInCombat)]
	private unsafe HitOrAvoidInts CalcHitValues()
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0105: Unknown result type (might be due to invalid IL or missing references)
		//IL_010a: Unknown result type (might be due to invalid IL or missing references)
		//IL_010f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0111: Unknown result type (might be due to invalid IL or missing references)
		//IL_0116: Unknown result type (might be due to invalid IL or missing references)
		//IL_012f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0142: Unknown result type (might be due to invalid IL or missing references)
		//IL_0151: Unknown result type (might be due to invalid IL or missing references)
		//IL_0156: Unknown result type (might be due to invalid IL or missing references)
		//IL_015b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0160: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0171: Unknown result type (might be due to invalid IL or missing references)
		//IL_0184: Unknown result type (might be due to invalid IL or missing references)
		//IL_0193: Unknown result type (might be due to invalid IL or missing references)
		//IL_0198: Unknown result type (might be due to invalid IL or missing references)
		//IL_019d: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a2: Unknown result type (might be due to invalid IL or missing references)
		HitOrAvoidInts baseHitValues = Template.BaseHitValues;
		BoolArray8 val = default(BoolArray8);
		BoolArray8 val2 = default(BoolArray8);
		Span<int> span = stackalloc int[4];
		Span<int> span2 = stackalloc int[4];
		for (int i = 0; i < 4; i++)
		{
			((BoolArray8)(ref val))[i] = DomainManager.SpecialEffect.ModifyData(_id, -1, 36, dataValue: true, i, 0);
			((BoolArray8)(ref val2))[i] = DomainManager.SpecialEffect.ModifyData(_id, -1, 36, dataValue: true, i, 1);
			span[i] = DomainManager.SpecialEffect.GetModifyValue(_id, 37, (EDataModifyType)0, i, 0, -1, (EDataSumType)0);
			span2[i] = DomainManager.SpecialEffect.GetModifyValue(_id, 37, (EDataModifyType)0, i, 1, -1, (EDataSumType)0);
		}
		for (int j = 0; j < 4; j++)
		{
			ECharacterPropertyReferencedType propertyType = (ECharacterPropertyReferencedType)(6 + j);
			EDataSumType valueSumType = DataSumTypeHelper.CalcSumType(((BoolArray8)(ref val))[j], ((BoolArray8)(ref val2))[j]);
			CValueModify val3 = CalcPropertyModify(propertyType, valueSumType);
			ushort fieldId = (ushort)(32 + j);
			if (((BoolArray8)(ref val))[j])
			{
				val3 += DomainManager.SpecialEffect.GetModify(_id, fieldId, -1, -1, -1, (EDataSumType)1) * CValuePercentBonus.op_Implicit(span[j]);
			}
			if (((BoolArray8)(ref val2))[j])
			{
				val3 += DomainManager.SpecialEffect.GetModify(_id, fieldId, -1, -1, -1, (EDataSumType)2) * CValuePercentBonus.op_Implicit(span2[j]);
			}
			int index = j;
			baseHitValues[index] *= val3;
		}
		Span<int> span3 = stackalloc int[4];
		span3.Fill(0);
		for (sbyte b = 0; b < 4; b++)
		{
			int modifyValue = DomainManager.SpecialEffect.GetModifyValue(_id, 277, (EDataModifyType)0, b, baseHitValues.Items[b], -1, (EDataSumType)0);
			for (sbyte b2 = 0; b2 < 4; b2++)
			{
				if (b2 != b)
				{
					span3[b2] += modifyValue;
				}
			}
		}
		for (sbyte b3 = 0; b3 < 4; b3++)
		{
			if (((BoolArray8)(ref val))[(int)b3])
			{
				ref int reference = ref baseHitValues.Items[b3];
				reference += span3[b3];
			}
		}
		for (sbyte b4 = 0; b4 < 4; b4++)
		{
			baseHitValues.Items[b4] = DomainManager.SpecialEffect.ModifyData(_id, -1, (ushort)(32 + b4), baseHitValues.Items[b4]);
		}
		if (CanAffectedByCombatDifficulty)
		{
			byte combatDifficulty = DomainManager.World.GetCombatDifficulty();
			short hitValues = CombatDifficulty.Instance[combatDifficulty].HitValues;
			for (int k = 0; k < 4; k++)
			{
				baseHitValues.Items[k] = baseHitValues.Items[k] * hitValues / 100;
			}
		}
		for (int l = 0; l < 4; l++)
		{
			if (baseHitValues.Items[l] < GlobalConfig.Instance.MinValueOfAttackAndDefenseAttributes)
			{
				baseHitValues.Items[l] = GlobalConfig.Instance.MinValueOfAttackAndDefenseAttributes;
			}
		}
		return baseHitValues;
	}

	[ObjectCollectionDependency(4, 0, new ushort[] { 17, 57, 59, 61, 80, 112, 115, 28 }, Scope = InfluenceScope.Self)]
	[ObjectCollectionDependency(7, 0, new ushort[] { 3, 5, 1, 10 }, Scope = InfluenceScope.CombatSkillOwner)]
	[SingleValueCollectionDependency(5, new ushort[] { 20 }, Scope = InfluenceScope.TaiwuChar)]
	[ElementListDependency(5, 16, 9, Scope = InfluenceScope.TaiwuChar)]
	[SingleValueCollectionDependency(19, new ushort[] { 121 }, Scope = InfluenceScope.TaiwuChar)]
	[SingleValueDependency(5, new ushort[] { 0 })]
	[SingleValueDependency(1, new ushort[] { 15 })]
	[ObjectCollectionDependency(17, 2, new ushort[] { 44, 45, 238, 293 }, Scope = InfluenceScope.CharacterAffectedByTheSpecialEffects)]
	[ElementListDependency(11, 0, 14)]
	[SingleValueCollectionDependency(19, new ushort[] { 245 }, Scope = InfluenceScope.TaiwuChar)]
	[SingleValueDependency(19, new ushort[] { 46, 47, 246 }, Scope = InfluenceScope.TaiwuChar)]
	[SingleValueDependency(8, new ushort[] { 19 }, Scope = InfluenceScope.AllCharsInCombat)]
	private OuterAndInnerInts CalcPenetrations()
	{
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		CharacterItem characterItem = Config.Character.Instance[_templateId];
		OuterAndInnerInts basePenetrations = characterItem.BasePenetrations;
		CValueModify val = CalcPropertyModify(ECharacterPropertyReferencedType.PenetrateOfOuter, (EDataSumType)0);
		val += DomainManager.SpecialEffect.GetModify(_id, 44, -1, -1, -1, (EDataSumType)0);
		ref int outer = ref basePenetrations.Outer;
		outer *= val;
		CValueModify val2 = CalcPropertyModify(ECharacterPropertyReferencedType.PenetrateOfInner, (EDataSumType)0);
		val2 += DomainManager.SpecialEffect.GetModify(_id, 45, -1, -1, -1, (EDataSumType)0);
		ref int inner = ref basePenetrations.Inner;
		inner *= val2;
		basePenetrations.Outer = DomainManager.SpecialEffect.ModifyData(_id, -1, 44, basePenetrations.Outer);
		basePenetrations.Inner = DomainManager.SpecialEffect.ModifyData(_id, -1, 45, basePenetrations.Inner);
		if (CanAffectedByCombatDifficulty)
		{
			byte combatDifficulty = DomainManager.World.GetCombatDifficulty();
			short penetrations = CombatDifficulty.Instance[combatDifficulty].Penetrations;
			basePenetrations.Outer = basePenetrations.Outer * penetrations / 100;
			basePenetrations.Inner = basePenetrations.Inner * penetrations / 100;
		}
		if (basePenetrations.Outer < GlobalConfig.Instance.MinValueOfAttackAndDefenseAttributes)
		{
			basePenetrations.Outer = GlobalConfig.Instance.MinValueOfAttackAndDefenseAttributes;
		}
		if (basePenetrations.Inner < GlobalConfig.Instance.MinValueOfAttackAndDefenseAttributes)
		{
			basePenetrations.Inner = GlobalConfig.Instance.MinValueOfAttackAndDefenseAttributes;
		}
		return basePenetrations;
	}

	[ObjectCollectionDependency(4, 0, new ushort[] { 17, 57, 59, 61, 80, 112, 115, 28 }, Scope = InfluenceScope.Self)]
	[ObjectCollectionDependency(7, 0, new ushort[] { 3, 5, 1, 10 }, Scope = InfluenceScope.CombatSkillOwner)]
	[SingleValueCollectionDependency(5, new ushort[] { 20 }, Scope = InfluenceScope.TaiwuChar)]
	[ElementListDependency(5, 16, 9, Scope = InfluenceScope.TaiwuChar)]
	[SingleValueCollectionDependency(19, new ushort[] { 121 }, Scope = InfluenceScope.TaiwuChar)]
	[SingleValueDependency(5, new ushort[] { 0 })]
	[SingleValueDependency(1, new ushort[] { 15 })]
	[ObjectCollectionDependency(17, 2, new ushort[] { 38, 39, 40, 41, 42, 43, 237, 278, 293 }, Scope = InfluenceScope.CharacterAffectedByTheSpecialEffects)]
	[ElementListDependency(11, 0, 14)]
	[SingleValueCollectionDependency(19, new ushort[] { 245 }, Scope = InfluenceScope.TaiwuChar)]
	[SingleValueDependency(19, new ushort[] { 46, 47, 246, 123 }, Scope = InfluenceScope.TaiwuChar)]
	[SingleValueDependency(8, new ushort[] { 19 }, Scope = InfluenceScope.AllCharsInCombat)]
	private unsafe HitOrAvoidInts CalcAvoidValues()
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0106: Unknown result type (might be due to invalid IL or missing references)
		//IL_010b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		//IL_0112: Unknown result type (might be due to invalid IL or missing references)
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		//IL_0130: Unknown result type (might be due to invalid IL or missing references)
		//IL_0143: Unknown result type (might be due to invalid IL or missing references)
		//IL_0152: Unknown result type (might be due to invalid IL or missing references)
		//IL_0157: Unknown result type (might be due to invalid IL or missing references)
		//IL_015c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0161: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_0172: Unknown result type (might be due to invalid IL or missing references)
		//IL_0185: Unknown result type (might be due to invalid IL or missing references)
		//IL_0194: Unknown result type (might be due to invalid IL or missing references)
		//IL_0199: Unknown result type (might be due to invalid IL or missing references)
		//IL_019e: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
		HitOrAvoidInts baseAvoidValues = Template.BaseAvoidValues;
		BoolArray8 val = default(BoolArray8);
		BoolArray8 val2 = default(BoolArray8);
		Span<int> span = stackalloc int[4];
		Span<int> span2 = stackalloc int[4];
		for (int i = 0; i < 4; i++)
		{
			((BoolArray8)(ref val))[i] = DomainManager.SpecialEffect.ModifyData(_id, -1, 42, dataValue: true, i, 0);
			((BoolArray8)(ref val2))[i] = DomainManager.SpecialEffect.ModifyData(_id, -1, 42, dataValue: true, i, 1);
			span[i] = DomainManager.SpecialEffect.GetModifyValue(_id, 43, (EDataModifyType)0, i, 0, -1, (EDataSumType)0);
			span2[i] = DomainManager.SpecialEffect.GetModifyValue(_id, 43, (EDataModifyType)0, i, 1, -1, (EDataSumType)0);
		}
		for (int j = 0; j < 4; j++)
		{
			ECharacterPropertyReferencedType propertyType = (ECharacterPropertyReferencedType)(12 + j);
			EDataSumType valueSumType = DataSumTypeHelper.CalcSumType(((BoolArray8)(ref val))[j], ((BoolArray8)(ref val2))[j]);
			CValueModify val3 = CalcPropertyModify(propertyType, valueSumType);
			ushort fieldId = (ushort)(38 + j);
			if (((BoolArray8)(ref val))[j])
			{
				val3 += DomainManager.SpecialEffect.GetModify(_id, fieldId, -1, -1, -1, (EDataSumType)1) * CValuePercentBonus.op_Implicit(span[j]);
			}
			if (((BoolArray8)(ref val2))[j])
			{
				val3 += DomainManager.SpecialEffect.GetModify(_id, fieldId, -1, -1, -1, (EDataSumType)2) * CValuePercentBonus.op_Implicit(span2[j]);
			}
			int index = j;
			baseAvoidValues[index] *= val3;
		}
		Span<int> span3 = stackalloc int[4];
		span3.Fill(0);
		for (sbyte b = 0; b < 4; b++)
		{
			int modifyValue = DomainManager.SpecialEffect.GetModifyValue(_id, 278, (EDataModifyType)0, b, baseAvoidValues.Items[b], -1, (EDataSumType)0);
			for (sbyte b2 = 0; b2 < 4; b2++)
			{
				if (b2 != b)
				{
					span3[b2] += modifyValue;
				}
			}
		}
		for (sbyte b3 = 0; b3 < 4; b3++)
		{
			if (((BoolArray8)(ref val))[(int)b3])
			{
				ref int reference = ref baseAvoidValues.Items[b3];
				reference += span3[b3];
			}
		}
		for (sbyte b4 = 0; b4 < 4; b4++)
		{
			baseAvoidValues.Items[b4] = DomainManager.SpecialEffect.ModifyData(_id, -1, (ushort)(38 + b4), baseAvoidValues.Items[b4]);
		}
		if (CanAffectedByCombatDifficulty)
		{
			byte combatDifficulty = DomainManager.World.GetCombatDifficulty();
			short avoidValues = CombatDifficulty.Instance[combatDifficulty].AvoidValues;
			for (int k = 0; k < 4; k++)
			{
				baseAvoidValues.Items[k] = baseAvoidValues.Items[k] * avoidValues / 100;
			}
		}
		for (int l = 0; l < 4; l++)
		{
			if (baseAvoidValues.Items[l] < GlobalConfig.Instance.MinValueOfAttackAndDefenseAttributes)
			{
				baseAvoidValues.Items[l] = GlobalConfig.Instance.MinValueOfAttackAndDefenseAttributes;
			}
		}
		return baseAvoidValues;
	}

	[ObjectCollectionDependency(4, 0, new ushort[] { 17, 57, 59, 61, 80, 112, 115, 28 }, Scope = InfluenceScope.Self)]
	[ObjectCollectionDependency(7, 0, new ushort[] { 3, 5, 1, 10 }, Scope = InfluenceScope.CombatSkillOwner)]
	[SingleValueCollectionDependency(5, new ushort[] { 20 }, Scope = InfluenceScope.TaiwuChar)]
	[ElementListDependency(5, 16, 9, Scope = InfluenceScope.TaiwuChar)]
	[SingleValueCollectionDependency(19, new ushort[] { 121 }, Scope = InfluenceScope.TaiwuChar)]
	[SingleValueDependency(5, new ushort[] { 0 })]
	[SingleValueDependency(1, new ushort[] { 15 })]
	[ObjectCollectionDependency(17, 2, new ushort[] { 46, 47, 239, 293 }, Scope = InfluenceScope.CharacterAffectedByTheSpecialEffects)]
	[ElementListDependency(11, 0, 14)]
	[SingleValueCollectionDependency(19, new ushort[] { 245 }, Scope = InfluenceScope.TaiwuChar)]
	[SingleValueDependency(19, new ushort[] { 46, 47, 246 }, Scope = InfluenceScope.TaiwuChar)]
	[SingleValueDependency(8, new ushort[] { 19 }, Scope = InfluenceScope.AllCharsInCombat)]
	[ObjectCollectionDependency(8, 10, new ushort[] { 46 }, Scope = InfluenceScope.CharOfTheCombatChar)]
	private OuterAndInnerInts CalcPenetrationResists()
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0139: Unknown result type (might be due to invalid IL or missing references)
		//IL_014a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0129: Unknown result type (might be due to invalid IL or missing references)
		//IL_012e: Unknown result type (might be due to invalid IL or missing references)
		OuterAndInnerInts basePenetrationResists = Template.BasePenetrationResists;
		CValueModify val = CalcPropertyModify(ECharacterPropertyReferencedType.PenetrateResistOfOuter, (EDataSumType)0);
		val += DomainManager.SpecialEffect.GetModify(_id, 46, -1, -1, -1, (EDataSumType)0);
		ref int outer = ref basePenetrationResists.Outer;
		outer *= val;
		CValueModify val2 = CalcPropertyModify(ECharacterPropertyReferencedType.PenetrateResistOfInner, (EDataSumType)0);
		val2 += DomainManager.SpecialEffect.GetModify(_id, 47, -1, -1, -1, (EDataSumType)0);
		ref int inner = ref basePenetrationResists.Inner;
		inner *= val2;
		if (DomainManager.Combat.IsCharInCombat(_id))
		{
			CombatCharacter element_CombatCharacterDict = DomainManager.Combat.GetElement_CombatCharacterDict(_id);
			PoisonInts poison = element_CombatCharacterDict.GetPoison();
			CValuePercentBonus val3 = CValuePercentBonus.op_Implicit(0);
			CValuePercentBonus val4 = CValuePercentBonus.op_Implicit(0);
			sbyte b = PoisonsAndLevels.CalcPoisonedLevel(poison[4]);
			if (b > 0)
			{
				val3 = CValuePercentBonus.op_Implicit(-b * Poison.Instance[(sbyte)4].ReduceOuterResist);
			}
			sbyte b2 = PoisonsAndLevels.CalcPoisonedLevel(poison[5]);
			if (b2 > 0)
			{
				val4 = CValuePercentBonus.op_Implicit(-b2 * Poison.Instance[(sbyte)5].ReduceInnerResist);
			}
			ref int outer2 = ref basePenetrationResists.Outer;
			outer2 *= val3;
			ref int inner2 = ref basePenetrationResists.Inner;
			inner2 *= val4;
		}
		basePenetrationResists.Outer = DomainManager.SpecialEffect.ModifyData(_id, -1, 46, basePenetrationResists.Outer);
		basePenetrationResists.Inner = DomainManager.SpecialEffect.ModifyData(_id, -1, 47, basePenetrationResists.Inner);
		if (CanAffectedByCombatDifficulty)
		{
			byte combatDifficulty = DomainManager.World.GetCombatDifficulty();
			short penetrationResists = CombatDifficulty.Instance[combatDifficulty].PenetrationResists;
			basePenetrationResists.Outer = basePenetrationResists.Outer * penetrationResists / 100;
			basePenetrationResists.Inner = basePenetrationResists.Inner * penetrationResists / 100;
		}
		if (basePenetrationResists.Outer < GlobalConfig.Instance.MinValueOfAttackAndDefenseAttributes)
		{
			basePenetrationResists.Outer = GlobalConfig.Instance.MinValueOfAttackAndDefenseAttributes;
		}
		if (basePenetrationResists.Inner < GlobalConfig.Instance.MinValueOfAttackAndDefenseAttributes)
		{
			basePenetrationResists.Inner = GlobalConfig.Instance.MinValueOfAttackAndDefenseAttributes;
		}
		return basePenetrationResists;
	}

	[ObjectCollectionDependency(4, 0, new ushort[] { 17, 57, 59, 61, 112, 115 }, Scope = InfluenceScope.Self)]
	[ObjectCollectionDependency(7, 0, new ushort[] { 3, 5, 1, 10 }, Scope = InfluenceScope.CombatSkillOwner)]
	[SingleValueCollectionDependency(5, new ushort[] { 20 }, Scope = InfluenceScope.TaiwuChar)]
	[ElementListDependency(5, 16, 9, Scope = InfluenceScope.TaiwuChar)]
	[SingleValueCollectionDependency(19, new ushort[] { 121 }, Scope = InfluenceScope.TaiwuChar)]
	[SingleValueDependency(5, new ushort[] { 0 })]
	[SingleValueDependency(1, new ushort[] { 15 })]
	[ObjectCollectionDependency(17, 2, new ushort[] { 7, 8, 17, 18, 293 }, Scope = InfluenceScope.CharacterAffectedByTheSpecialEffects)]
	[ElementListDependency(11, 0, 14)]
	[SingleValueDependency(19, new ushort[] { 46 }, Scope = InfluenceScope.TaiwuChar)]
	[SingleValueDependency(19, new ushort[] { 47 }, Scope = InfluenceScope.TaiwuChar)]
	private OuterAndInnerShorts CalcRecoveryOfStanceAndBreath()
	{
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		if (TryGetFixedSubAttributeValue(out var fixedSubAttributeValue))
		{
			return new OuterAndInnerShorts(fixedSubAttributeValue, fixedSubAttributeValue);
		}
		OuterAndInnerInts outerAndInnerInts = Template.BaseRecoveryOfStanceAndBreath;
		CValueModify val = CalcPropertyModify(ECharacterPropertyReferencedType.RecoveryOfStance, (EDataSumType)0);
		val += DomainManager.SpecialEffect.GetModify(_id, 7, -1, -1, -1, (EDataSumType)0);
		val = ((CValueModify)(ref val)).MaxA(outerAndInnerInts.Outer, (int)GlobalConfig.Instance.MinAValueOfMinorAttributes);
		ref int outer = ref outerAndInnerInts.Outer;
		outer *= val;
		CValueModify val2 = CalcPropertyModify(ECharacterPropertyReferencedType.RecoveryOfBreath, (EDataSumType)0);
		val2 += DomainManager.SpecialEffect.GetModify(_id, 8, -1, -1, -1, (EDataSumType)0);
		val2 = ((CValueModify)(ref val2)).MaxA(outerAndInnerInts.Inner, (int)GlobalConfig.Instance.MinAValueOfMinorAttributes);
		ref int inner = ref outerAndInnerInts.Inner;
		inner *= val2;
		outerAndInnerInts.Outer = (short)DomainManager.SpecialEffect.ModifyData(_id, -1, 7, outerAndInnerInts.Outer);
		outerAndInnerInts.Inner = (short)DomainManager.SpecialEffect.ModifyData(_id, -1, 8, outerAndInnerInts.Inner);
		if (CanAffectedByCombatDifficulty)
		{
			byte combatDifficulty = DomainManager.World.GetCombatDifficulty();
			OuterAndInnerShorts recoveryOfStanceAndBreath = CombatDifficulty.Instance[combatDifficulty].RecoveryOfStanceAndBreath;
			outerAndInnerInts.Outer = (short)(outerAndInnerInts.Outer * recoveryOfStanceAndBreath.Outer / 100);
			outerAndInnerInts.Inner = (short)(outerAndInnerInts.Inner * recoveryOfStanceAndBreath.Inner / 100);
		}
		outerAndInnerInts.Outer = Math.Clamp(outerAndInnerInts.Outer, 0, 1000);
		outerAndInnerInts.Inner = Math.Clamp(outerAndInnerInts.Inner, 0, 1000);
		return (OuterAndInnerShorts)outerAndInnerInts;
	}

	[ObjectCollectionDependency(4, 0, new ushort[] { 17, 57, 59, 61, 112, 115, 107, 106 }, Scope = InfluenceScope.Self)]
	[ObjectCollectionDependency(7, 0, new ushort[] { 3, 5, 1, 10 }, Scope = InfluenceScope.CombatSkillOwner)]
	[SingleValueCollectionDependency(5, new ushort[] { 20 }, Scope = InfluenceScope.TaiwuChar)]
	[ElementListDependency(5, 16, 9, Scope = InfluenceScope.TaiwuChar)]
	[SingleValueCollectionDependency(19, new ushort[] { 121 }, Scope = InfluenceScope.TaiwuChar)]
	[SingleValueDependency(5, new ushort[] { 0 })]
	[SingleValueDependency(1, new ushort[] { 15 })]
	[ObjectCollectionDependency(8, 10, new ushort[] { 62 }, Scope = InfluenceScope.CharOfTheCombatChar)]
	[ObjectCollectionDependency(17, 2, new ushort[] { 9, 55, 17, 18, 279, 293 }, Scope = InfluenceScope.CharacterAffectedByTheSpecialEffects)]
	[ElementListDependency(11, 0, 14)]
	[SingleValueDependency(19, new ushort[] { 46 }, Scope = InfluenceScope.TaiwuChar)]
	[SingleValueDependency(19, new ushort[] { 47 }, Scope = InfluenceScope.TaiwuChar)]
	private short CalcMoveSpeed()
	{
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_012c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0131: Unknown result type (might be due to invalid IL or missing references)
		//IL_0134: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_010c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0116: Unknown result type (might be due to invalid IL or missing references)
		//IL_011b: Unknown result type (might be due to invalid IL or missing references)
		if (TryGetFixedSubAttributeValue(out var fixedSubAttributeValue))
		{
			return fixedSubAttributeValue;
		}
		int baseMoveSpeed = Template.BaseMoveSpeed;
		bool flag = DomainManager.SpecialEffect.ModifyData(_id, -1, 55, dataValue: true, 0);
		bool flag2 = DomainManager.SpecialEffect.ModifyData(_id, -1, 55, dataValue: true, 1);
		EDataSumType valueSumType = DataSumTypeHelper.CalcSumType(flag, flag2);
		CValueModify val = CalcPropertyModify(ECharacterPropertyReferencedType.MoveSpeed, valueSumType);
		val += DomainManager.SpecialEffect.GetModify(_id, 9, -1, -1, -1, valueSumType);
		if (DomainManager.Combat.IsCharInCombat(_id) && flag)
		{
			CombatCharacter element_CombatCharacterDict = DomainManager.Combat.GetElement_CombatCharacterDict(_id);
			short affectingMoveSkillId = element_CombatCharacterDict.GetAffectingMoveSkillId();
			if (affectingMoveSkillId >= 0)
			{
				GameData.Domains.CombatSkill.CombatSkill element_CombatSkills = DomainManager.CombatSkill.GetElement_CombatSkills(new CombatSkillKey(_id, affectingMoveSkillId));
				val = ((CValueModify)(ref val)).ChangeA((int)CombatSkillDomain.CalcCastAddMoveSpeed(element_CombatSkills, CValuePercent.op_Implicit((int)element_CombatSkills.GetPower())));
				val = ((CValueModify)(ref val)).ChangeB((int)CombatSkillDomain.CalcCastAddPercentMoveSpeed(element_CombatSkills, CValuePercent.op_Implicit((int)element_CombatSkills.GetPower())));
			}
		}
		val = ((CValueModify)(ref val)).MaxA(baseMoveSpeed, (int)GlobalConfig.Instance.MinAValueOfMinorAttributes);
		baseMoveSpeed *= val;
		baseMoveSpeed = CalcOverloadBonus(baseMoveSpeed, GlobalConfig.Instance.EquipLoadSpeedPercent);
		baseMoveSpeed = DomainManager.SpecialEffect.ModifyData(_id, -1, 9, baseMoveSpeed);
		if (CanAffectedByCombatDifficulty)
		{
			byte combatDifficulty = DomainManager.World.GetCombatDifficulty();
			short moveSpeed = CombatDifficulty.Instance[combatDifficulty].MoveSpeed;
			baseMoveSpeed = baseMoveSpeed * moveSpeed / 100;
		}
		return (short)Math.Clamp(baseMoveSpeed, 0, 1000);
	}

	[ObjectCollectionDependency(4, 0, new ushort[] { 17, 57, 59, 61, 112, 115, 107, 106 }, Scope = InfluenceScope.Self)]
	[ObjectCollectionDependency(7, 0, new ushort[] { 3, 5, 1, 10 }, Scope = InfluenceScope.CombatSkillOwner)]
	[SingleValueCollectionDependency(5, new ushort[] { 20 }, Scope = InfluenceScope.TaiwuChar)]
	[ElementListDependency(5, 16, 9, Scope = InfluenceScope.TaiwuChar)]
	[SingleValueCollectionDependency(19, new ushort[] { 121 }, Scope = InfluenceScope.TaiwuChar)]
	[SingleValueDependency(5, new ushort[] { 0 })]
	[SingleValueDependency(1, new ushort[] { 15 })]
	[ObjectCollectionDependency(17, 2, new ushort[] { 10, 17, 18, 293, 279 }, Scope = InfluenceScope.CharacterAffectedByTheSpecialEffects)]
	[ElementListDependency(11, 0, 14)]
	[SingleValueDependency(19, new ushort[] { 46 }, Scope = InfluenceScope.TaiwuChar)]
	[SingleValueDependency(19, new ushort[] { 47 }, Scope = InfluenceScope.TaiwuChar)]
	private short CalcRecoveryOfFlaw()
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		if (TryGetFixedSubAttributeValue(out var fixedSubAttributeValue))
		{
			return fixedSubAttributeValue;
		}
		int baseRecoveryOfFlaw = Template.BaseRecoveryOfFlaw;
		CValueModify val = CalcPropertyModify(ECharacterPropertyReferencedType.RecoveryOfFlaw, (EDataSumType)0);
		val += DomainManager.SpecialEffect.GetModify(_id, 10, -1, -1, -1, (EDataSumType)0);
		val = ((CValueModify)(ref val)).MaxA(baseRecoveryOfFlaw, (int)GlobalConfig.Instance.MinAValueOfMinorAttributes);
		baseRecoveryOfFlaw *= val;
		baseRecoveryOfFlaw = CalcOverloadBonus(baseRecoveryOfFlaw, GlobalConfig.Instance.EquipHealSpeedPercent);
		baseRecoveryOfFlaw = DomainManager.SpecialEffect.ModifyData(_id, -1, 10, baseRecoveryOfFlaw);
		if (CanAffectedByCombatDifficulty)
		{
			byte combatDifficulty = DomainManager.World.GetCombatDifficulty();
			short recoveryOfFlaw = CombatDifficulty.Instance[combatDifficulty].RecoveryOfFlaw;
			baseRecoveryOfFlaw = baseRecoveryOfFlaw * recoveryOfFlaw / 100;
		}
		return (short)Math.Clamp(baseRecoveryOfFlaw, 0, 1000);
	}

	[ObjectCollectionDependency(4, 0, new ushort[] { 17, 57, 59, 61, 112, 115, 107, 106 }, Scope = InfluenceScope.Self)]
	[ObjectCollectionDependency(7, 0, new ushort[] { 3, 5, 1, 10 }, Scope = InfluenceScope.CombatSkillOwner)]
	[SingleValueCollectionDependency(5, new ushort[] { 20 }, Scope = InfluenceScope.TaiwuChar)]
	[ElementListDependency(5, 16, 9, Scope = InfluenceScope.TaiwuChar)]
	[SingleValueCollectionDependency(19, new ushort[] { 121 }, Scope = InfluenceScope.TaiwuChar)]
	[SingleValueDependency(5, new ushort[] { 0 })]
	[SingleValueDependency(1, new ushort[] { 15 })]
	[ObjectCollectionDependency(17, 2, new ushort[] { 11, 279, 17, 18, 293 }, Scope = InfluenceScope.CharacterAffectedByTheSpecialEffects)]
	[ElementListDependency(11, 0, 14)]
	[SingleValueDependency(19, new ushort[] { 46 }, Scope = InfluenceScope.TaiwuChar)]
	[SingleValueDependency(19, new ushort[] { 47 }, Scope = InfluenceScope.TaiwuChar)]
	private short CalcCastSpeed()
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		if (TryGetFixedSubAttributeValue(out var fixedSubAttributeValue))
		{
			return fixedSubAttributeValue;
		}
		int baseCastSpeed = Template.BaseCastSpeed;
		CValueModify val = CalcPropertyModify(ECharacterPropertyReferencedType.CastSpeed, (EDataSumType)0);
		val += DomainManager.SpecialEffect.GetModify(_id, 11, -1, -1, -1, (EDataSumType)0);
		val = ((CValueModify)(ref val)).MaxA(baseCastSpeed, (int)GlobalConfig.Instance.MinAValueOfMinorAttributes);
		baseCastSpeed *= val;
		baseCastSpeed = CalcOverloadBonus(baseCastSpeed, GlobalConfig.Instance.EquipLoadSpeedPercent);
		baseCastSpeed = DomainManager.SpecialEffect.ModifyData(_id, -1, 11, baseCastSpeed);
		if (CanAffectedByCombatDifficulty)
		{
			byte combatDifficulty = DomainManager.World.GetCombatDifficulty();
			short castSpeed = CombatDifficulty.Instance[combatDifficulty].CastSpeed;
			baseCastSpeed = baseCastSpeed * castSpeed / 100;
		}
		return (short)Math.Clamp(baseCastSpeed, 0, 1000);
	}

	[ObjectCollectionDependency(4, 0, new ushort[] { 17, 57, 59, 61, 112, 115, 107, 106 }, Scope = InfluenceScope.Self)]
	[ObjectCollectionDependency(7, 0, new ushort[] { 3, 5, 1, 10 }, Scope = InfluenceScope.CombatSkillOwner)]
	[SingleValueCollectionDependency(5, new ushort[] { 20 }, Scope = InfluenceScope.TaiwuChar)]
	[ElementListDependency(5, 16, 9, Scope = InfluenceScope.TaiwuChar)]
	[SingleValueCollectionDependency(19, new ushort[] { 121 }, Scope = InfluenceScope.TaiwuChar)]
	[SingleValueDependency(5, new ushort[] { 0 })]
	[SingleValueDependency(1, new ushort[] { 15 })]
	[ObjectCollectionDependency(17, 2, new ushort[] { 12, 17, 18, 293, 279 }, Scope = InfluenceScope.CharacterAffectedByTheSpecialEffects)]
	[ElementListDependency(11, 0, 14)]
	[SingleValueDependency(19, new ushort[] { 46 }, Scope = InfluenceScope.TaiwuChar)]
	[SingleValueDependency(19, new ushort[] { 47 }, Scope = InfluenceScope.TaiwuChar)]
	private short CalcRecoveryOfBlockedAcupoint()
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		if (TryGetFixedSubAttributeValue(out var fixedSubAttributeValue))
		{
			return fixedSubAttributeValue;
		}
		int baseRecoveryOfBlockedAcupoint = Template.BaseRecoveryOfBlockedAcupoint;
		CValueModify val = CalcPropertyModify(ECharacterPropertyReferencedType.RecoveryOfBlockedAcupoint, (EDataSumType)0);
		val += DomainManager.SpecialEffect.GetModify(_id, 12, -1, -1, -1, (EDataSumType)0);
		val = ((CValueModify)(ref val)).MaxA(baseRecoveryOfBlockedAcupoint, (int)GlobalConfig.Instance.MinAValueOfMinorAttributes);
		baseRecoveryOfBlockedAcupoint *= val;
		baseRecoveryOfBlockedAcupoint = CalcOverloadBonus(baseRecoveryOfBlockedAcupoint, GlobalConfig.Instance.EquipHealSpeedPercent);
		baseRecoveryOfBlockedAcupoint = DomainManager.SpecialEffect.ModifyData(_id, -1, 12, baseRecoveryOfBlockedAcupoint);
		if (CanAffectedByCombatDifficulty)
		{
			byte combatDifficulty = DomainManager.World.GetCombatDifficulty();
			short recoveryOfBlockedAcupoint = CombatDifficulty.Instance[combatDifficulty].RecoveryOfBlockedAcupoint;
			baseRecoveryOfBlockedAcupoint = baseRecoveryOfBlockedAcupoint * recoveryOfBlockedAcupoint / 100;
		}
		return (short)Math.Clamp(baseRecoveryOfBlockedAcupoint, 0, 1000);
	}

	[ObjectCollectionDependency(4, 0, new ushort[] { 17, 57, 59, 61, 112, 115, 107, 106 }, Scope = InfluenceScope.Self)]
	[ObjectCollectionDependency(7, 0, new ushort[] { 3, 5, 1, 10 }, Scope = InfluenceScope.CombatSkillOwner)]
	[SingleValueCollectionDependency(5, new ushort[] { 20 }, Scope = InfluenceScope.TaiwuChar)]
	[ElementListDependency(5, 16, 9, Scope = InfluenceScope.TaiwuChar)]
	[SingleValueCollectionDependency(19, new ushort[] { 121 }, Scope = InfluenceScope.TaiwuChar)]
	[SingleValueDependency(5, new ushort[] { 0 })]
	[SingleValueDependency(1, new ushort[] { 15 })]
	[ObjectCollectionDependency(17, 2, new ushort[] { 13, 17, 18, 293, 279 }, Scope = InfluenceScope.CharacterAffectedByTheSpecialEffects)]
	[ElementListDependency(11, 0, 14)]
	[SingleValueDependency(19, new ushort[] { 46 }, Scope = InfluenceScope.TaiwuChar)]
	[SingleValueDependency(19, new ushort[] { 47 }, Scope = InfluenceScope.TaiwuChar)]
	private short CalcWeaponSwitchSpeed()
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		if (TryGetFixedSubAttributeValue(out var fixedSubAttributeValue))
		{
			return fixedSubAttributeValue;
		}
		int baseWeaponSwitchSpeed = Template.BaseWeaponSwitchSpeed;
		CValueModify val = CalcPropertyModify(ECharacterPropertyReferencedType.WeaponSwitchSpeed, (EDataSumType)0);
		val += DomainManager.SpecialEffect.GetModify(_id, 13, -1, -1, -1, (EDataSumType)0);
		val = ((CValueModify)(ref val)).MaxA(baseWeaponSwitchSpeed, (int)GlobalConfig.Instance.MinAValueOfMinorAttributes);
		baseWeaponSwitchSpeed *= val;
		baseWeaponSwitchSpeed = CalcOverloadBonus(baseWeaponSwitchSpeed, GlobalConfig.Instance.EquipHealSpeedPercent);
		baseWeaponSwitchSpeed = DomainManager.SpecialEffect.ModifyData(_id, -1, 13, baseWeaponSwitchSpeed);
		if (CanAffectedByCombatDifficulty)
		{
			byte combatDifficulty = DomainManager.World.GetCombatDifficulty();
			short weaponSwitchSpeed = CombatDifficulty.Instance[combatDifficulty].WeaponSwitchSpeed;
			baseWeaponSwitchSpeed = baseWeaponSwitchSpeed * weaponSwitchSpeed / 100;
		}
		return (short)Math.Clamp(baseWeaponSwitchSpeed, 0, 1000);
	}

	[ObjectCollectionDependency(4, 0, new ushort[] { 17, 57, 59, 61, 112, 115, 107, 106 }, Scope = InfluenceScope.Self)]
	[ObjectCollectionDependency(7, 0, new ushort[] { 3, 5, 1, 10 }, Scope = InfluenceScope.CombatSkillOwner)]
	[SingleValueCollectionDependency(5, new ushort[] { 20 }, Scope = InfluenceScope.TaiwuChar)]
	[ElementListDependency(5, 16, 9, Scope = InfluenceScope.TaiwuChar)]
	[SingleValueCollectionDependency(19, new ushort[] { 121 }, Scope = InfluenceScope.TaiwuChar)]
	[SingleValueDependency(5, new ushort[] { 0 })]
	[SingleValueDependency(1, new ushort[] { 15 })]
	[ObjectCollectionDependency(17, 2, new ushort[] { 14, 279, 17, 18, 293 }, Scope = InfluenceScope.CharacterAffectedByTheSpecialEffects)]
	[ElementListDependency(11, 0, 14)]
	[SingleValueDependency(19, new ushort[] { 46 }, Scope = InfluenceScope.TaiwuChar)]
	[SingleValueDependency(19, new ushort[] { 47 }, Scope = InfluenceScope.TaiwuChar)]
	private short CalcAttackSpeed()
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		if (TryGetFixedSubAttributeValue(out var fixedSubAttributeValue))
		{
			return fixedSubAttributeValue;
		}
		int baseAttackSpeed = Template.BaseAttackSpeed;
		CValueModify val = CalcPropertyModify(ECharacterPropertyReferencedType.AttackSpeed, (EDataSumType)0);
		val += DomainManager.SpecialEffect.GetModify(_id, 14, -1, -1, -1, (EDataSumType)0);
		val = ((CValueModify)(ref val)).MaxA(baseAttackSpeed, (int)GlobalConfig.Instance.MinAValueOfMinorAttributes);
		baseAttackSpeed *= val;
		baseAttackSpeed = CalcOverloadBonus(baseAttackSpeed, GlobalConfig.Instance.EquipLoadSpeedPercent);
		baseAttackSpeed = DomainManager.SpecialEffect.ModifyData(_id, -1, 14, baseAttackSpeed);
		if (CanAffectedByCombatDifficulty)
		{
			byte combatDifficulty = DomainManager.World.GetCombatDifficulty();
			short attackSpeed = CombatDifficulty.Instance[combatDifficulty].AttackSpeed;
			baseAttackSpeed = baseAttackSpeed * attackSpeed / 100;
		}
		return (short)Math.Clamp(baseAttackSpeed, 0, 1000);
	}

	[ObjectCollectionDependency(4, 0, new ushort[] { 17, 57, 59, 61, 112, 115 }, Scope = InfluenceScope.Self)]
	[ObjectCollectionDependency(7, 0, new ushort[] { 3, 5, 1, 10 }, Scope = InfluenceScope.CombatSkillOwner)]
	[SingleValueCollectionDependency(5, new ushort[] { 20 }, Scope = InfluenceScope.TaiwuChar)]
	[ElementListDependency(5, 16, 9, Scope = InfluenceScope.TaiwuChar)]
	[SingleValueCollectionDependency(19, new ushort[] { 121 }, Scope = InfluenceScope.TaiwuChar)]
	[SingleValueDependency(5, new ushort[] { 0 })]
	[SingleValueDependency(1, new ushort[] { 15 })]
	[ObjectCollectionDependency(17, 2, new ushort[] { 15, 17, 18, 293 }, Scope = InfluenceScope.CharacterAffectedByTheSpecialEffects)]
	[ElementListDependency(11, 0, 14)]
	[SingleValueCollectionDependency(19, new ushort[] { 245 }, Scope = InfluenceScope.TaiwuChar)]
	[SingleValueDependency(19, new ushort[] { 46, 47, 246 }, Scope = InfluenceScope.TaiwuChar)]
	private short CalcInnerRatio()
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		if (TryGetFixedSubAttributeValue(out var fixedSubAttributeValue))
		{
			return fixedSubAttributeValue;
		}
		int baseInnerRatio = Template.BaseInnerRatio;
		CValueModify val = CalcPropertyModify(ECharacterPropertyReferencedType.InnerRatio, (EDataSumType)0);
		val += DomainManager.SpecialEffect.GetModify(_id, 15, -1, -1, -1, (EDataSumType)0);
		val = ((CValueModify)(ref val)).MaxA(baseInnerRatio, (int)GlobalConfig.Instance.MinAValueOfMinorAttributes);
		baseInnerRatio *= val;
		baseInnerRatio = DomainManager.SpecialEffect.ModifyData(_id, -1, 15, baseInnerRatio);
		if (CanAffectedByCombatDifficulty)
		{
			byte combatDifficulty = DomainManager.World.GetCombatDifficulty();
			short innerRatio = CombatDifficulty.Instance[combatDifficulty].InnerRatio;
			baseInnerRatio = baseInnerRatio * innerRatio / 100;
		}
		return (short)Math.Clamp(baseInnerRatio, 0, 1000);
	}

	[ObjectCollectionDependency(4, 0, new ushort[] { 17, 57, 59, 61, 112, 115, 56, 68 }, Scope = InfluenceScope.Self)]
	[ObjectCollectionDependency(7, 0, new ushort[] { 3, 5, 1, 10 }, Scope = InfluenceScope.CombatSkillOwner)]
	[SingleValueCollectionDependency(5, new ushort[] { 20 }, Scope = InfluenceScope.TaiwuChar)]
	[ElementListDependency(5, 16, 9, Scope = InfluenceScope.TaiwuChar)]
	[SingleValueCollectionDependency(19, new ushort[] { 121 }, Scope = InfluenceScope.TaiwuChar)]
	[SingleValueDependency(5, new ushort[] { 0 })]
	[SingleValueDependency(1, new ushort[] { 15 })]
	[ObjectCollectionDependency(17, 2, new ushort[] { 16, 17, 18, 293 }, Scope = InfluenceScope.CharacterAffectedByTheSpecialEffects)]
	[ElementListDependency(11, 0, 14)]
	[SingleValueCollectionDependency(19, new ushort[] { 245 }, Scope = InfluenceScope.TaiwuChar)]
	[SingleValueDependency(19, new ushort[] { 46, 47, 246 }, Scope = InfluenceScope.TaiwuChar)]
	private short CalcRecoveryOfQiDisorder()
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		if (TryGetFixedSubAttributeValue(out var fixedSubAttributeValue))
		{
			return fixedSubAttributeValue;
		}
		int baseRecoveryOfQiDisorder = Template.BaseRecoveryOfQiDisorder;
		CValueModify val = CalcPropertyModify(ECharacterPropertyReferencedType.RecoveryOfQiDisorder, (EDataSumType)0);
		val += DomainManager.SpecialEffect.GetModify(_id, 16, -1, -1, -1, (EDataSumType)0);
		val = ((CValueModify)(ref val)).MaxA(baseRecoveryOfQiDisorder, (int)GlobalConfig.Instance.MinAValueOfMinorAttributes);
		baseRecoveryOfQiDisorder *= val;
		if (IsActiveExternalRelationState(32) && DomainManager.Organization.GetPrisonerSect(_id) == 4)
		{
			baseRecoveryOfQiDisorder /= 2;
		}
		baseRecoveryOfQiDisorder = DomainManager.SpecialEffect.ModifyData(_id, -1, 16, baseRecoveryOfQiDisorder);
		if (CanAffectedByCombatDifficulty)
		{
			byte combatDifficulty = DomainManager.World.GetCombatDifficulty();
			short recoveryOfQiDisorder = CombatDifficulty.Instance[combatDifficulty].RecoveryOfQiDisorder;
			baseRecoveryOfQiDisorder = baseRecoveryOfQiDisorder * recoveryOfQiDisorder / 100;
		}
		return (short)Math.Clamp(baseRecoveryOfQiDisorder, 0, 1000);
	}

	[ObjectCollectionDependency(4, 0, new ushort[] { 17, 57, 59, 61, 112, 115, 21 }, Scope = InfluenceScope.Self)]
	[ObjectCollectionDependency(7, 0, new ushort[] { 3, 5, 1, 10 }, Scope = InfluenceScope.CombatSkillOwner)]
	[SingleValueCollectionDependency(5, new ushort[] { 20 }, Scope = InfluenceScope.TaiwuChar)]
	[ElementListDependency(5, 16, 9, Scope = InfluenceScope.TaiwuChar)]
	[SingleValueCollectionDependency(19, new ushort[] { 121 }, Scope = InfluenceScope.TaiwuChar)]
	[SingleValueCollectionDependency(19, new ushort[] { 150 }, Scope = InfluenceScope.TaiwuChar)]
	[ObjectCollectionDependency(17, 2, new ushort[] { 19, 20, 21, 22, 23, 24, 245 }, Scope = InfluenceScope.CharacterAffectedByTheSpecialEffects)]
	private PoisonInts CalcPoisonResists()
	{
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		CharacterItem characterItem = Config.Character.Instance[_templateId];
		PoisonInts basePoisonResists = characterItem.BasePoisonResists;
		CValueModify val = DomainManager.SpecialEffect.GetModify(_id, 245, -1, -1, -1, (EDataSumType)0);
		sbyte disorderLevelOfQi = DisorderLevelOfQi.GetDisorderLevelOfQi(_disorderOfQi);
		val = ((CValueModify)(ref val)).ChangeC(QiDisorderEffect.Instance[disorderLevelOfQi].PoisonResistChange);
		for (int i = 0; i < 6; i++)
		{
			ECharacterPropertyReferencedType propertyType = (ECharacterPropertyReferencedType)(28 + i);
			ushort fieldId = (ushort)(19 + i);
			CValueModify val2 = CalcPropertyModify(propertyType, (EDataSumType)0);
			val2 += DomainManager.SpecialEffect.GetModify(_id, fieldId, -1, -1, -1, (EDataSumType)0);
			val2 += val;
			val2 = ((CValueModify)(ref val2)).ReverseByValue(basePoisonResists[i]);
			ref int reference = ref basePoisonResists[i];
			reference *= val2;
		}
		return basePoisonResists;
	}

	[ObjectCollectionDependency(4, 0, new ushort[] { 17, 20 }, Scope = InfluenceScope.Self)]
	[ObjectCollectionDependency(17, 2, new ushort[] { 53 }, Scope = InfluenceScope.CharacterAffectedByTheSpecialEffects)]
	[SingleValueCollectionDependency(19, new ushort[] { 245, 150 }, Scope = InfluenceScope.TaiwuChar)]
	private short CalcMaxHealth()
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		int num = (int)_baseMaxHealth * CalcPropertyModify(ECharacterPropertyReferencedType.MaxHealth, (EDataSumType)0);
		num += DomainManager.SpecialEffect.GetModifyValue(_id, 53, (EDataModifyType)0, -1, -1, -1, (EDataSumType)0);
		if (IsTaiwu())
		{
			num += ProfessionSkillHandle.TravelingTaoistMonkSkill_GetMaxHealthBonus();
		}
		return (short)Math.Clamp(num, 0, 32767);
	}

	[ObjectCollectionDependency(4, 0, new ushort[] { 17, 76, 44, 94, 59, 61 }, Scope = InfluenceScope.Self)]
	[SingleValueCollectionDependency(19, new ushort[] { 121 }, Scope = InfluenceScope.TaiwuChar)]
	private short CalcFertility()
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		int num = 100;
		num *= CalcPropertyModify(ECharacterPropertyReferencedType.Fertility, (EDataSumType)0);
		short physiologicalAge = GetPhysiologicalAge();
		short clampedAgeOfAgeEffect = GetClampedAgeOfAgeEffect(physiologicalAge);
		AgeEffectItem ageEffectItem = AgeEffect.Instance[clampedAgeOfAgeEffect];
		num += ((_gender == 1) ? ageEffectItem.FertilityMale : ageEffectItem.FertilityFemale);
		num -= GetMixedPoisonTypeRelatedMarkCount(25) * 30;
		return (short)Math.Clamp(num, 0, 32767);
	}

	[ObjectCollectionDependency(4, 0, new ushort[] { 17, 57, 59, 30, 31, 63, 4 }, Scope = InfluenceScope.Self)]
	[SingleValueDependency(5, new ushort[] { 0 })]
	[SingleValueDependency(9, new ushort[] { 18 }, Scope = InfluenceScope.TaiwuChar)]
	[SingleValueCollectionDependency(19, new ushort[] { 150 }, Scope = InfluenceScope.TaiwuChar)]
	[ObjectCollectionDependency(17, 2, new ushort[] { 293 }, Scope = InfluenceScope.CharacterAffectedByTheSpecialEffects)]
	private LifeSkillShorts CalcLifeSkillQualifications()
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		LifeSkillShorts baseLifeSkillQualifications = _baseLifeSkillQualifications;
		short clampedAgeOfAgeEffect = GetClampedAgeOfAgeEffect(_actualAge);
		for (int i = 0; i < 16; i++)
		{
			ECharacterPropertyReferencedType propertyType = (ECharacterPropertyReferencedType)(34 + i);
			CValueModify val = CalcPropertyModify(propertyType, (EDataSumType)0);
			int num = (int)baseLifeSkillQualifications[i] * val;
			if (clampedAgeOfAgeEffect < 16)
			{
				num = num * clampedAgeOfAgeEffect / 16;
			}
			baseLifeSkillQualifications[i] = (short)Math.Clamp(num, 0, 32767);
		}
		return baseLifeSkillQualifications;
	}

	[ObjectCollectionDependency(4, 0, new ushort[] { 29, 97, 56, 8 }, Scope = InfluenceScope.Self)]
	private LifeSkillShorts CalcLifeSkillAttainments()
	{
		LifeSkillShorts value = default(LifeSkillShorts);
		value.Initialize();
		GetLifeSkillBaseAttainment(ref value);
		GetLifeSkillAttainmentAddOns(ref value);
		return value;
	}

	[ObjectCollectionDependency(4, 0, new ushort[] { 17, 57, 59, 32, 33, 63, 4 }, Scope = InfluenceScope.Self)]
	[SingleValueDependency(5, new ushort[] { 0 })]
	[SingleValueDependency(9, new ushort[] { 17 }, Scope = InfluenceScope.TaiwuChar)]
	[ElementListDependency(11, 0, 14)]
	[SingleValueDependency(19, new ushort[] { 46, 47 }, Scope = InfluenceScope.TaiwuChar)]
	[SingleValueCollectionDependency(19, new ushort[] { 150 }, Scope = InfluenceScope.TaiwuChar)]
	[ObjectCollectionDependency(17, 2, new ushort[] { 293 }, Scope = InfluenceScope.CharacterAffectedByTheSpecialEffects)]
	private CombatSkillShorts CalcCombatSkillQualifications()
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		CombatSkillShorts baseCombatSkillQualifications = _baseCombatSkillQualifications;
		short clampedAgeOfAgeEffect = GetClampedAgeOfAgeEffect(_actualAge);
		for (int i = 0; i < 14; i++)
		{
			ECharacterPropertyReferencedType propertyType = (ECharacterPropertyReferencedType)(66 + i);
			CValueModify val = CalcPropertyModify(propertyType, (EDataSumType)0);
			int num = (int)baseCombatSkillQualifications[i] * val;
			if (clampedAgeOfAgeEffect < 16)
			{
				num = num * clampedAgeOfAgeEffect / 16;
			}
			baseCombatSkillQualifications[i] = (short)Math.Clamp(num, 0, 32767);
		}
		return baseCombatSkillQualifications;
	}

	[ObjectCollectionDependency(4, 0, new ushort[] { 62, 99, 8 }, Scope = InfluenceScope.Self)]
	private unsafe CombatSkillShorts CalcCombatSkillAttainments()
	{
		CombatSkillShorts result = default(CombatSkillShorts);
		result.Initialize();
		CombatSkillShorts combatSkillShorts = default(CombatSkillShorts);
		combatSkillShorts.Initialize();
		for (int i = 0; i < 14; i++)
		{
			for (int j = 0; j < 9; j++)
			{
				int num = 9 * i + j;
				short num2 = _combatSkillAttainmentPanels[num];
				if (num2 >= 0)
				{
					CombatSkillItem combatSkillItem = Config.CombatSkill.Instance[num2];
					ref short reference = ref result.Items[i];
					reference += GlobalConfig.Instance.AddAttainmentPerGrade[combatSkillItem.Grade];
				}
			}
		}
		ref CombatSkillShorts combatSkillQualifications = ref GetCombatSkillQualifications();
		for (int k = 0; k < 14; k++)
		{
			result.Items[k] = (short)(combatSkillQualifications.Items[k] * (100 + result.Items[k]) / 100 + result.Items[k]);
		}
		for (int l = 0; l < 14; l++)
		{
			int num3 = result.Items[l] + combatSkillShorts.Items[l];
			result.Items[l] = (short)((num3 >= 0) ? num3 : 0);
		}
		return result;
	}

	[ObjectCollectionDependency(4, 0, new ushort[] { 17, 57, 59, 56, 8 }, Scope = InfluenceScope.Self)]
	[SingleValueCollectionDependency(19, new ushort[] { 224 }, Scope = InfluenceScope.AllCharsInTaiwuVillage)]
	[SingleValueCollectionDependency(9, new ushort[] { 8 }, Scope = InfluenceScope.AllCharsInTaiwuVillage)]
	[ObjectCollectionDependency(17, 2, new ushort[] { 303 }, Scope = InfluenceScope.CharacterAffectedByTheSpecialEffects)]
	private Personalities CalcPersonalities()
	{
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		Personalities result = default(Personalities);
		CValueModify modify = DomainManager.SpecialEffect.GetModify(_id, 303, -1, -1, -1, (EDataSumType)0);
		for (int i = 0; i < 7; i++)
		{
			ECharacterPropertyReferencedType propertyType = (ECharacterPropertyReferencedType)(94 + i);
			CValueModify val = CalcPropertyModify(propertyType, (EDataSumType)0) + modify;
			int value = 10 * val;
			result[i] = (sbyte)Math.Clamp(value, 0, 100);
		}
		return result;
	}

	[ObjectCollectionDependency(4, 0, new ushort[] { 17 }, Scope = InfluenceScope.Self)]
	private sbyte CalcHobbyChangingPeriod()
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		int baseHobbyChangingPeriod = GlobalConfig.Instance.BaseHobbyChangingPeriod;
		baseHobbyChangingPeriod *= CalcPropertyModify(ECharacterPropertyReferencedType.HobbyChangingPeriod, (EDataSumType)0);
		return (sbyte)((baseHobbyChangingPeriod < 1) ? 1 : baseHobbyChangingPeriod);
	}

	[ObjectCollectionDependency(4, 0, new ushort[] { 17 }, Scope = InfluenceScope.Self)]
	private OuterAndInnerShorts CalcFavorabilityChangingFactor()
	{
		OuterAndInnerShorts result = new OuterAndInnerShorts(100, 100);
		int i = 0;
		for (int count = _featureIds.Count; i < count; i++)
		{
			short index = _featureIds[i];
			CharacterFeatureItem characterFeatureItem = CharacterFeature.Instance[index];
			result.Outer = (short)(result.Outer * characterFeatureItem.FavorabilityIncrementFactor / 100);
			result.Inner = (short)(result.Inner * characterFeatureItem.FavorabilityDecrementFactor / 100);
		}
		return result;
	}

	[ObjectCollectionDependency(4, 0, new ushort[] { 57 }, Scope = InfluenceScope.Self)]
	[ObjectCollectionDependency(6, 4, new ushort[] { 4 }, Scope = InfluenceScope.CharWhoEquippedTheItem, Condition = InfluenceCondition.ItemIsEquipped)]
	private int CalcMaxInventoryLoad()
	{
		int num = 3000;
		ItemKey itemKey = _equipment[11];
		if (itemKey.IsValid())
		{
			GameData.Domains.Item.Carrier element_Carriers = DomainManager.Item.GetElement_Carriers(itemKey.Id);
			if (element_Carriers.GetMaxDurability() == 0 || element_Carriers.GetCurrDurability() > 0)
			{
				num += element_Carriers.GetMaxInventoryLoadBonus();
			}
		}
		for (sbyte b = 8; b <= 10; b++)
		{
			ItemKey itemKey2 = _equipment[b];
			if (itemKey2.IsValid())
			{
				EquipmentBase baseEquipment = DomainManager.Item.GetBaseEquipment(itemKey2);
				if (baseEquipment.GetMaxDurability() == 0 || baseEquipment.GetCurrDurability() > 0)
				{
					AccessoryItem accessoryItem = Config.Accessory.Instance[itemKey2.TemplateId];
					num += accessoryItem.MaxInventoryLoadBonus;
				}
			}
		}
		return num;
	}

	[ObjectCollectionDependency(4, 0, new ushort[] { 58, 57 }, Scope = InfluenceScope.Self)]
	private int CalcCurrInventoryLoad()
	{
		int num = 0;
		foreach (KeyValuePair<ItemKey, int> item in _inventory.Items)
		{
			item.Deconstruct(out var key, out var value);
			ItemKey itemKey = key;
			int num2 = value;
			ItemBase baseItem = DomainManager.Item.GetBaseItem(itemKey);
			num += baseItem.GetWeight() * num2;
		}
		for (int i = 0; i < 11; i++)
		{
			ItemKey itemKey2 = _equipment[i];
			if (itemKey2.IsValid())
			{
				ItemBase baseItem2 = DomainManager.Item.GetBaseItem(itemKey2);
				num += baseItem2.GetWeight();
			}
		}
		return num;
	}

	[ObjectCollectionDependency(4, 0, new ushort[] { 80 }, Scope = InfluenceScope.Self)]
	private unsafe int CalcMaxEquipmentLoad()
	{
		short items = GetMaxMainAttributes().Items[0];
		int extraEquipmentLoad = Config.Character.Instance[_templateId].ExtraEquipmentLoad;
		return GlobalConfig.Instance.EquipmentLoadBaseValue + items * GlobalConfig.Instance.StrengthToEquipmentLoadFactor + extraEquipmentLoad;
	}

	[ObjectCollectionDependency(4, 0, new ushort[] { 57 }, Scope = InfluenceScope.Self)]
	[ObjectCollectionDependency(17, 2, new ushort[] { 311 }, Scope = InfluenceScope.CharacterAffectedByTheSpecialEffects)]
	private int CalcCurrEquipmentLoad()
	{
		int num = 0;
		for (sbyte b = 0; b <= 10; b++)
		{
			ItemKey itemKey = _equipment[b];
			if (itemKey.IsValid())
			{
				int weight = DomainManager.Item.GetBaseItem(itemKey).GetWeight();
				num += DomainManager.SpecialEffect.ModifyValue(_id, 311, weight, itemKey.Id, itemKey.ItemType);
			}
		}
		return num;
	}

	[ObjectCollectionDependency(4, 0, new ushort[] { 58 }, Scope = InfluenceScope.Self)]
	private int CalcInventoryTotalValue()
	{
		int num = 0;
		foreach (var (itemKey2, num3) in _inventory.Items)
		{
			if (itemKey2.ItemType != 10)
			{
				num += DomainManager.Item.GetValue(itemKey2) * num3;
			}
		}
		return num;
	}

	[ObjectCollectionDependency(4, 0, new ushort[] { 27, 60, 48 }, Scope = InfluenceScope.Self)]
	[ObjectCollectionDependency(7, 0, new ushort[] { 8, 7 }, Scope = InfluenceScope.CombatSkillOwner)]
	[SingleValueCollectionDependency(19, new ushort[] { 150 }, Scope = InfluenceScope.TaiwuChar)]
	[SingleValueCollectionDependency(19, new ushort[] { 254 }, Scope = InfluenceScope.TaiwuAndGearMates)]
	private unsafe int CalcMaxNeili()
	{
		int num = GetPureMaxNeili();
		for (int i = 0; i < 4; i++)
		{
			num -= CombatHelper.CalcNeiliCostFromZero(_baseNeiliAllocation.Items[i]);
		}
		return num;
	}

	[ObjectCollectionDependency(4, 0, new ushort[] { 48, 49, 1 }, Scope = InfluenceScope.Self)]
	private unsafe NeiliAllocation CalcNeiliAllocation()
	{
		NeiliAllocation baseNeiliAllocation = _baseNeiliAllocation;
		for (int i = 0; i < 4; i++)
		{
			int num = baseNeiliAllocation.Items[i] + _extraNeiliAllocation.Items[i];
			baseNeiliAllocation.Items[i] = (short)((num >= 0) ? num : 0);
		}
		return baseNeiliAllocation;
	}

	[ObjectCollectionDependency(4, 0, new ushort[] { 50 }, Scope = InfluenceScope.Self)]
	[ObjectCollectionDependency(8, 10, new ushort[] { 120 }, Scope = InfluenceScope.CharOfTheCombatChar)]
	[SingleValueDependency(8, new ushort[] { 19 }, Scope = InfluenceScope.AllCharsInCombat)]
	[ObjectCollectionDependency(17, 2, new ushort[] { 26 }, Scope = InfluenceScope.CharacterAffectedByTheSpecialEffects)]
	private NeiliProportionOfFiveElements CalcNeiliProportionOfFiveElements()
	{
		NeiliProportionOfFiveElements neiliProportionOfFiveElements = _baseNeiliProportionOfFiveElements;
		neiliProportionOfFiveElements = DomainManager.SpecialEffect.ModifyData(_id, -1, 26, neiliProportionOfFiveElements);
		if (DomainManager.Combat.IsCharInCombat(_id))
		{
			CombatCharacter element_CombatCharacterDict = DomainManager.Combat.GetElement_CombatCharacterDict(_id);
			NeiliProportionOfFiveElements proportionDelta = element_CombatCharacterDict.GetProportionDelta();
			Tester.Assert(proportionDelta.Sum() == 0, "delta.Sum() == 0");
			for (int i = 0; i < 5; i++)
			{
				neiliProportionOfFiveElements[i] += proportionDelta[i];
			}
		}
		Tester.Assert(neiliProportionOfFiveElements.SumCheck() == 100);
		return neiliProportionOfFiveElements;
	}

	[ObjectCollectionDependency(4, 0, new ushort[] { 111 }, Scope = InfluenceScope.Self)]
	private sbyte CalcNeiliType()
	{
		return GetNeiliProportionOfFiveElements().GetNeiliType(_birthMonth);
	}

	[ObjectCollectionDependency(4, 0, new ushort[] { 110 }, Scope = InfluenceScope.Self)]
	[ObjectCollectionDependency(8, 10, new ushort[] { 3 }, Scope = InfluenceScope.CharOfTheCombatChar)]
	[SingleValueDependency(8, new ushort[] { 19 }, Scope = InfluenceScope.AllCharsInCombat)]
	private unsafe NeiliAllocation CalcAllocatedNeiliEffects()
	{
		NeiliAllocation result = default(NeiliAllocation);
		NeiliAllocation neiliAllocation = GetNeiliAllocation();
		NeiliAllocation neiliAllocation2 = (DomainManager.Combat.IsCharInCombat(_id) ? DomainManager.Combat.GetElement_CombatCharacterDict(_id).GetNeiliAllocation() : neiliAllocation);
		for (sbyte b = 0; b < 4; b++)
		{
			result.Items[b] = (short)(neiliAllocation2.Items[b] * GlobalConfig.Instance.AllocatedNeiliEffectPercent / 100);
		}
		return result;
	}

	[SingleValueDependency(11, new ushort[] { 0 })]
	[ObjectCollectionDependency(4, 0, new ushort[]
	{
		61, 110, 28, 80, 81, 82, 83, 84, 85, 86,
		87, 88, 89, 90, 91, 92, 93, 97, 98, 99,
		100, 57, 26, 44, 94
	}, Scope = InfluenceScope.Self)]
	[ObjectCollectionDependency(7, 0, new ushort[] { 14 }, Scope = InfluenceScope.CombatSkillOwner)]
	private unsafe int CalcCombatPower()
	{
		int num = 0;
		num += _consummateLevel * 1000;
		MainAttributes maxMainAttributes = GetMaxMainAttributes();
		int num2 = 0;
		for (int i = 0; i < 6; i++)
		{
			num2 += maxMainAttributes.Items[i];
		}
		num += num2 * 3;
		int num3 = 0;
		HitOrAvoidInts hitValues = GetHitValues();
		for (int j = 0; j < 4; j++)
		{
			num3 += hitValues.Items[j];
		}
		OuterAndInnerInts penetrations = GetPenetrations();
		num3 += penetrations.Outer + penetrations.Inner;
		num += num3;
		int num4 = 0;
		HitOrAvoidInts avoidValues = GetAvoidValues();
		for (int k = 0; k < 4; k++)
		{
			num4 += avoidValues.Items[k];
		}
		OuterAndInnerInts penetrationResists = GetPenetrationResists();
		num4 += penetrationResists.Outer + penetrationResists.Inner;
		num += num4;
		OuterAndInnerShorts recoveryOfStanceAndBreath = GetRecoveryOfStanceAndBreath();
		num += (recoveryOfStanceAndBreath.Outer + recoveryOfStanceAndBreath.Inner + GetMoveSpeed() + GetRecoveryOfFlaw() + GetCastSpeed() + GetRecoveryOfBlockedAcupoint() + GetWeaponSwitchSpeed() + GetAttackSpeed() + GetInnerRatio() + GetRecoveryOfQiDisorder() - 1000) * 5;
		int num5 = 0;
		int num6 = 0;
		int num7 = 0;
		for (int l = 0; l < 16; l++)
		{
			short num8 = _lifeSkillAttainments.Items[l];
			if (num8 > num5)
			{
				num7 = num6;
				num6 = num5;
				num5 = num8;
			}
			else if (num8 > num6)
			{
				num7 = num6;
				num6 = num8;
			}
			else if (num8 > num7)
			{
				num7 = num8;
			}
		}
		num += (num5 + num6 + num7) * 20;
		int num9 = 0;
		int num10 = 0;
		int num11 = 0;
		for (int m = 0; m < 14; m++)
		{
			short num12 = _combatSkillAttainments.Items[m];
			if (num12 > num9)
			{
				num11 = num10;
				num10 = num9;
				num9 = num12;
			}
			else if (num12 > num10)
			{
				num11 = num10;
				num10 = num12;
			}
			else if (num12 > num11)
			{
				num11 = num12;
			}
		}
		num += (num9 + num10 + num11) * 40;
		num += GetEquipmentCombatPowerValue();
		int defeatMarksCountOutOfCombat = CombatDomain.GetDefeatMarksCountOutOfCombat(this);
		byte b = GlobalConfig.NeedDefeatMarkCount[2];
		int num13 = defeatMarksCountOutOfCombat * 100 / b;
		if (DomainManager.LegendaryBook.GetCharOwnedBookTypes(_id) != null)
		{
			num13 /= 2;
		}
		num -= num * num13 / 100;
		return (num >= 0) ? num : 0;
	}

	[ObjectCollectionDependency(4, 0, new ushort[] { 61, 92 }, Scope = InfluenceScope.Self)]
	[ObjectCollectionDependency(7, 0, new ushort[] { 15 }, Scope = InfluenceScope.CombatSkillOwner)]
	private sbyte CalcAttackTendencyOfInnerAndOuter()
	{
		int num = 0;
		int num2 = 0;
		ArraySegmentList<short> attack = GetCombatSkillEquipment().Attack;
		int i = 0;
		for (int count = attack.Count; i < count; i++)
		{
			short num3 = attack[i];
			if (num3 >= 0)
			{
				CombatSkillKey objectId = new CombatSkillKey(_id, num3);
				GameData.Domains.CombatSkill.CombatSkill element_CombatSkills = DomainManager.CombatSkill.GetElement_CombatSkills(objectId);
				num += element_CombatSkills.GetCurrInnerRatio();
				num2++;
			}
		}
		if (num2 > 0)
		{
			num /= num2;
		}
		return (sbyte)num;
	}

	[ObjectCollectionDependency(4, 0, new ushort[] { 62 }, Scope = InfluenceScope.Self)]
	private sbyte CalcMaxConsummateLevel()
	{
		int num = -1;
		short[] combatSkillAttainmentPanels = GetCombatSkillAttainmentPanels();
		for (sbyte b = 0; b < 14; b++)
		{
			int num2 = 0;
			int num3 = 0;
			int num4 = 0;
			int num5 = 0;
			for (sbyte b2 = 0; b2 <= 8; b2++)
			{
				int num6 = 9 * b + b2;
				short num7 = combatSkillAttainmentPanels[num6];
				if (num7 >= 0)
				{
					if (b2 != 0)
					{
						num2++;
					}
					int val = GlobalConfig.Instance.ConsummateLevelPoints[b2];
					switch (Grade.GetGroup(b2))
					{
					case 0:
						num3 = Math.Max(val, num3);
						break;
					case 1:
						num4 = Math.Max(val, num4);
						break;
					case 2:
						num5 = Math.Max(val, num5);
						break;
					}
				}
			}
			num2 += (num3 + num4 + num5) / 10;
			if (num < num2)
			{
				num = num2;
			}
		}
		return (sbyte)Math.Clamp(num, 0, GlobalConfig.Instance.MaxConsummateLevel);
	}

	[ObjectCollectionDependency(4, 0, new ushort[] { 61 }, Scope = InfluenceScope.Self)]
	private void CalcCombatSkillEquipment(CombatSkillEquipment value)
	{
		if (DomainManager.Extra.TryGetCharacterEquippedCombatSkills(_id, out var plan))
		{
			value.Set(plan);
		}
		else
		{
			value.Set(_equippedCombatSkills);
		}
	}

	private static short GetClampedAgeOfAgeEffect(short age)
	{
		if (age < 0)
		{
			return 20;
		}
		if (age > 100)
		{
			return 100;
		}
		return age;
	}

	public AvatarRelatedData GenerateAvatarRelatedData()
	{
		return new AvatarRelatedData
		{
			AvatarData = new AvatarData(GetAvatar()),
			DisplayAge = GetPhysiologicalAge(),
			ClothingDisplayId = GetClothingDisplayId(),
			HasNewGoods = DomainManager.Character.MerchantHasNewGoods(GetId())
		};
	}

	public bool IsAbleToGrowAvatarElement(sbyte growableElementType, short physiologicalAge)
	{
		if (1 == 0)
		{
		}
		bool result = growableElementType switch
		{
			0 => IsAbleToGrowHair(), 
			1 => IsAbleToGrowBeard1(physiologicalAge), 
			2 => IsAbleToGrowBeard2(physiologicalAge), 
			3 => IsAbleToGrowWrinkle1(physiologicalAge), 
			4 => IsAbleToGrowWrinkle2(physiologicalAge), 
			5 => IsAbleToGrowWrinkle3(physiologicalAge), 
			6 => IsAbleToGrowEyebrow(), 
			_ => throw new Exception($"Unsupported AvatarGrowableElementType: {growableElementType}"), 
		};
		if (1 == 0)
		{
		}
		return result;
	}

	public bool IsAbleToGrowHair()
	{
		return !IsMonkType(2);
	}

	public (bool beard1, bool beard2) IsAbleToGrowBeards(short physiologicalAge)
	{
		if (_gender != 1 || _transgender || _featureIds.Contains(168))
		{
			return (beard1: false, beard2: false);
		}
		return (beard1: physiologicalAge >= GlobalConfig.Instance.AgeShowBeard1, beard2: physiologicalAge >= GlobalConfig.Instance.AgeShowBeard2);
	}

	public bool IsAbleToGrowBeard1(short physiologicalAge)
	{
		return _gender == 1 && physiologicalAge >= GlobalConfig.Instance.AgeShowBeard1 && !_transgender && !_featureIds.Contains(168);
	}

	public bool IsAbleToGrowBeard2(short physiologicalAge)
	{
		return _gender == 1 && physiologicalAge >= GlobalConfig.Instance.AgeShowBeard2 && !_transgender && !_featureIds.Contains(168);
	}

	public void SetAvatar(DataContext context, AvatarData customAvatarData)
	{
		for (sbyte b = 0; b < 7; b++)
		{
			customAvatarData.SetGrowableElementShowingAbility(b, IsAbleToGrowAvatarElement(b, GetPhysiologicalAge()));
		}
		for (sbyte b2 = 0; b2 < 7; b2++)
		{
			if (!customAvatarData.GetGrowableElementShowingState(b2) && IsAbleToGrowAvatarElement(b2, GetPhysiologicalAge()))
			{
				DomainManager.Character.InitializeAvatarElementGrowthProgress(context, _id, b2);
			}
		}
		SetAvatar(customAvatarData, context);
	}

	public static bool IsAbleToGrowWrinkle1(short physiologicalAge)
	{
		return physiologicalAge >= GlobalConfig.Instance.AgeShowWrinkle1;
	}

	public static bool IsAbleToGrowWrinkle2(short physiologicalAge)
	{
		return physiologicalAge >= GlobalConfig.Instance.AgeShowWrinkle2;
	}

	public static bool IsAbleToGrowWrinkle3(short physiologicalAge)
	{
		return physiologicalAge >= GlobalConfig.Instance.AgeShowWrinkle3;
	}

	public static bool IsAbleToGrowEyebrow()
	{
		return true;
	}

	public sbyte GetInnateFiveElementsType()
	{
		return SharedMethods.GetInnateFiveElementsType(_birthMonth);
	}

	public void SpecifyCurrNeili(DataContext context, int value)
	{
		if (value < 0)
		{
			throw new Exception($"{this}'s current neili not enough: {value}");
		}
		int maxNeili = GetMaxNeili();
		if (value > maxNeili)
		{
			value = maxNeili;
		}
		SetCurrNeili(value, context);
	}

	public void ChangeCurrNeili(DataContext context, int delta)
	{
		int num = _currNeili + delta;
		if (num < 0)
		{
			throw new Exception($"{this}'s current neili not enough: {_currNeili}, {delta}");
		}
		int maxNeili = GetMaxNeili();
		if (num > maxNeili)
		{
			num = maxNeili;
		}
		SetCurrNeili(num, context);
	}

	public void ChangeCurrNeiliWithoutChecking(DataContext context, int delta)
	{
		int maxNeili = GetMaxNeili();
		int currNeili = Math.Clamp(_currNeili + delta, 0, maxNeili);
		SetCurrNeili(currNeili, context);
	}

	public int GetCurrNeiliRecovery(int maxNeili)
	{
		short combatSkillAttainment = GetCombatSkillAttainment(0);
		if (IsActiveExternalRelationState(32) && DomainManager.Organization.GetPrisonerSect(_id) == 3)
		{
			return 0;
		}
		return combatSkillAttainment + _extraNeili / 4;
	}

	public unsafe int GetPureCurrNeili()
	{
		int num = _currNeili;
		for (int i = 0; i < 4; i++)
		{
			num += CombatHelper.CalcNeiliCostFromZero(_baseNeiliAllocation.Items[i]);
		}
		int pureMaxNeili = GetPureMaxNeili();
		if (num > pureMaxNeili)
		{
			return pureMaxNeili;
		}
		return num;
	}

	public void ChangeExtraNeili(DataContext context, int delta)
	{
		_extraNeili += delta;
		if (_extraNeili < 0)
		{
			_extraNeili = 0;
		}
		SetExtraNeili(_extraNeili, context);
	}

	public void TransferNeiliProportionOfFiveElements(DataContext context, sbyte destType, sbyte transferType, int amount)
	{
		_baseNeiliProportionOfFiveElements.Transfer(destType, transferType, amount);
		SetBaseNeiliProportionOfFiveElements(_baseNeiliProportionOfFiveElements, context);
	}

	public unsafe bool AllocateNeili(DataContext context, byte neiliAllocationType)
	{
		if (!CombatHelper.CanAllocateNeiliConsideringFeature(neiliAllocationType, _baseNeiliAllocation, _currNeili, _consummateLevel, GetFeatureIds()))
		{
			return false;
		}
		short num = _baseNeiliAllocation.Items[(int)neiliAllocationType];
		_baseNeiliAllocation.Items[(int)neiliAllocationType] = (short)(num + 1);
		SetBaseNeiliAllocation(_baseNeiliAllocation, context);
		int num2 = CombatHelper.CalcNeiliCost(num);
		ChangeCurrNeili(context, -num2);
		Tester.Assert(_currNeili <= GetMaxNeili());
		return true;
	}

	public unsafe bool DeallocateNeili(DataContext context, byte neiliAllocationType)
	{
		short num = _baseNeiliAllocation.Items[(int)neiliAllocationType];
		if (num <= 0)
		{
			return false;
		}
		short num2 = (short)(num - 1);
		_baseNeiliAllocation.Items[(int)neiliAllocationType] = num2;
		SetBaseNeiliAllocation(_baseNeiliAllocation, context);
		int delta = CombatHelper.CalcNeiliCost(num2);
		ChangeCurrNeili(context, delta);
		Tester.Assert(_currNeili <= GetMaxNeili());
		return true;
	}

	public void SpecifyBaseNeiliAllocation(DataContext context, NeiliAllocation allocations)
	{
		int pureCurrNeili = GetPureCurrNeili();
		int num = CombatHelper.CalcRequiredNeili(allocations);
		int value = pureCurrNeili - num;
		SetBaseNeiliAllocation(allocations, context);
		SpecifyCurrNeili(context, value);
	}

	public void ResetBaseNeiliAllocation(DataContext context)
	{
		int pureCurrNeili = GetPureCurrNeili();
		_baseNeiliAllocation.Initialize();
		SetBaseNeiliAllocation(_baseNeiliAllocation, context);
		SpecifyCurrNeili(context, pureCurrNeili);
	}

	public unsafe void ChangeExtraNeiliAllocation(DataContext context, NeiliAllocation delta)
	{
		for (int i = 0; i < 4; i++)
		{
			ref short reference = ref _extraNeiliAllocation.Items[i];
			reference += delta.Items[i];
		}
		SetExtraNeiliAllocation(_extraNeiliAllocation, context);
	}

	public unsafe void ChangeExtraNeiliAllocation(DataContext context, byte neiliAllocationType, short delta)
	{
		ref short reference = ref _extraNeiliAllocation.Items[(int)neiliAllocationType];
		reference += delta;
		SetExtraNeiliAllocation(_extraNeiliAllocation, context);
	}

	public void ChangePoisoned(DataContext context, sbyte poisonType, sbyte poisonLevel, int delta)
	{
		if (OfflineChangePoisoned(poisonType, poisonLevel, delta))
		{
			SetPoisoned(ref _poisoned, context);
		}
	}

	public bool CalcChangedPoisoned(ref PoisonInts targetPoisoned, sbyte poisonType, sbyte poisonLevel, int delta)
	{
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		if (HasPoisonImmunity(poisonType))
		{
			return false;
		}
		int poisonResist = GetPoisonResists()[poisonType];
		int num = targetPoisoned[poisonType];
		if (delta > 0)
		{
			if (1 == 0)
			{
			}
			int num2 = poisonType switch
			{
				3 => GetMixedPoisonTypeRelatedMarkCount(18), 
				0 => GetMixedPoisonTypeRelatedMarkCount(20), 
				1 => GetMixedPoisonTypeRelatedMarkCount(30), 
				2 => GetMixedPoisonTypeRelatedMarkCount(34), 
				_ => 0, 
			};
			if (1 == 0)
			{
			}
			int num3 = num2;
			if (num3 > 0)
			{
				delta += num3 * 10 + 10;
			}
			foreach (SolarTermItem item in GetInvokedSolarTerm())
			{
				if (item.PoisonBuffType == poisonType)
				{
					delta *= GetSolarTermBonus(GlobalConfig.Instance.SolarTermAddPoisonEffect);
				}
			}
			delta = PoisonsAndLevels.CalcPoisonDelta(delta, poisonLevel, num, poisonResist);
		}
		else if (poisonLevel < PoisonsAndLevels.CalcPoisonedLevel(num))
		{
			return false;
		}
		targetPoisoned[poisonType] = Math.Clamp(num + delta, 0, 25000);
		return true;
	}

	private bool OfflineChangePoisoned(sbyte poisonType, sbyte poisonLevel, int delta)
	{
		return CalcChangedPoisoned(ref _poisoned, poisonType, poisonLevel, delta);
	}

	public void ChangePoisoned(DataContext context, ref PoisonsAndLevels delta)
	{
		OfflineChangePoisoned(ref delta);
		SetPoisoned(ref _poisoned, context);
	}

	private unsafe void OfflineChangePoisoned(ref PoisonsAndLevels delta)
	{
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		CharacterItem characterCfg = Config.Character.Instance[_templateId];
		byte poisonImmunities = DomainManager.Extra.GetPoisonImmunities(_id);
		ref PoisonInts poisonResists = ref GetPoisonResists();
		for (sbyte b = 0; b < 6; b++)
		{
			if (!SharedMethods.HasPoisonImmunity(b, characterCfg, ref poisonResists, poisonImmunities))
			{
				int num = delta.Values[b];
				sbyte level = delta.Levels[b];
				int num2 = _poisoned.Items[b];
				int poisonResist = poisonResists.Items[b];
				if (num >= 0)
				{
					foreach (SolarTermItem item in GetInvokedSolarTerm())
					{
						if (item.PoisonBuffType == b)
						{
							num *= GetSolarTermBonus(GlobalConfig.Instance.SolarTermAddPoisonEffect);
						}
					}
					num = PoisonsAndLevels.CalcPoisonDelta(num, level, num2, poisonResist);
					_poisoned.Items[b] = Math.Clamp(num2 + num, 0, 25000);
				}
			}
		}
	}

	public unsafe void DirectlyChangePoisoned(DataContext context, ref PoisonInts delta)
	{
		for (sbyte b = 0; b < 6; b++)
		{
			if (!HasPoisonImmunity(b))
			{
				int max = 25000;
				_poisoned.Items[b] = Math.Clamp(_poisoned.Items[b] + delta.Items[b], 0, max);
			}
		}
		SetPoisoned(ref _poisoned, context);
	}

	public void TransferDisorderOfQi(DataContext context, Character target, int delta)
	{
		_disorderOfQi = (short)Math.Clamp(_disorderOfQi - delta, DisorderLevelOfQi.MinValue, DisorderLevelOfQi.MaxValue);
		target._disorderOfQi = (short)Math.Clamp(target._disorderOfQi + delta, DisorderLevelOfQi.MinValue, DisorderLevelOfQi.MaxValue);
		SetDisorderOfQi(_disorderOfQi, context);
		target.SetDisorderOfQi(target._disorderOfQi, context);
	}

	public void ChangeDisorderOfQi(DataContext context, int baseDelta)
	{
		int delta = CalcDisorderOfQiDelta(baseDelta);
		short disorderOfQi = CalcChangedDisorderOfQiWithoutEffect(_disorderOfQi, delta);
		SetDisorderOfQi(disorderOfQi, context);
	}

	public short CalcChangedDisorderOfQiWithoutEffect(short disorderOfQi, int delta)
	{
		return (short)Math.Clamp(disorderOfQi + delta, DisorderLevelOfQi.MinValue, DisorderLevelOfQi.MaxValue);
	}

	public int CalcDisorderOfQiDelta(int baseDelta)
	{
		//IL_0004: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0102: Unknown result type (might be due to invalid IL or missing references)
		//IL_0107: Unknown result type (might be due to invalid IL or missing references)
		//IL_0149: Unknown result type (might be due to invalid IL or missing references)
		//IL_0155: Unknown result type (might be due to invalid IL or missing references)
		//IL_015a: Unknown result type (might be due to invalid IL or missing references)
		//IL_015f: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_017c: Unknown result type (might be due to invalid IL or missing references)
		int num = baseDelta;
		CValuePercentBonus val = CValuePercentBonus.op_Implicit(0);
		if (baseDelta < 0)
		{
			foreach (short featureId in _featureIds)
			{
				val += CValuePercentBonus.op_Implicit((int)CharacterFeature.Instance[featureId].QiDisorderBuffPercent);
			}
			foreach (SolarTermItem item in GetInvokedSolarTerm())
			{
				if (item.QiDisorderRecoveringBuff)
				{
					val += GetSolarTermBonus(GlobalConfig.Instance.SolarTermAddRecoverQiDisorder);
				}
			}
		}
		else
		{
			foreach (short featureId2 in _featureIds)
			{
				if (!IgnoreFeature(featureId2))
				{
					val += CValuePercentBonus.op_Implicit((int)CharacterFeature.Instance[featureId2].QiDisorderDebuffPercent);
				}
			}
			foreach (SolarTermItem item2 in GetInvokedSolarTerm())
			{
				if (item2.QiDisorderRecoveringBuff)
				{
					val -= GetSolarTermBonus(GlobalConfig.Instance.SolarTermAddRecoverQiDisorder);
				}
			}
		}
		num *= ((CValuePercentBonus)(ref val)).StaySymbol();
		if (num < 0)
		{
			return num + num / 2 * GetRecoveryOfQiDisorder() / 1000;
		}
		return num - num / 2 * GetRecoveryOfQiDisorder() / 1000;
	}

	public void ChangeDisorderOfQiRandomRecovery(DataContext context, int delta)
	{
		delta = CFormula.RandomCalcDisorderOfQiDelta(context.Random, delta);
		ChangeDisorderOfQi(context, delta);
	}

	public void ChangeInjury(DataContext context, sbyte bodyPartType, bool isInnerInjury, sbyte delta)
	{
		CharacterItem characterItem = Config.Character.Instance[_templateId];
		if (!(isInnerInjury ? characterItem.InnerInjuryImmunity : characterItem.OuterInjuryImmunity))
		{
			_injuries.Change(bodyPartType, isInnerInjury, delta);
			SetInjuries(_injuries, context);
		}
	}

	public void ChangeInjuries(DataContext context, Injuries delta)
	{
		CharacterItem characterItem = Config.Character.Instance[_templateId];
		_injuries.Change(delta, characterItem.OuterInjuryImmunity, characterItem.InnerInjuryImmunity);
		SetInjuries(_injuries, context);
	}

	public void ChangeConsummateLevel(DataContext context, int delta)
	{
		sbyte consummateLevel = (sbyte)Math.Clamp(_consummateLevel + delta, 0, GlobalConfig.Instance.MaxConsummateLevel);
		SetConsummateLevel(consummateLevel, context);
	}

	public sbyte GetCombatSkillSlotCountWithGeneric(sbyte equipType)
	{
		if (_id == DomainManager.Taiwu.GetTaiwuCharId())
		{
			byte[] genericGridAllocation = DomainManager.Taiwu.GetGenericGridAllocation();
			byte orDefault = genericGridAllocation.GetOrDefault(equipType - 1);
			return (sbyte)(GetCombatSkillSlotCount(equipType) + orDefault);
		}
		Span<sbyte> slotCounts = stackalloc sbyte[5];
		GetCombatSkillSlotCountsWithGeneric(slotCounts);
		return slotCounts[equipType];
	}

	public int GetCombatSkillSlotCountsWithGeneric(Span<sbyte> slotCounts)
	{
		sbyte combatSkillSlotCounts = GetCombatSkillSlotCounts(slotCounts);
		return ApplyGenericCombatSkillSlotAllocations(slotCounts, combatSkillSlotCounts);
	}

	public int ApplyGenericCombatSkillSlotAllocations(Span<sbyte> slotCounts, int genericCount)
	{
		if (IsTaiwu())
		{
			byte[] genericGridAllocation = DomainManager.Taiwu.GetGenericGridAllocation();
			for (sbyte b = 1; b < 5; b++)
			{
				sbyte b2 = (sbyte)genericGridAllocation[b - 1];
				slotCounts[b] += b2;
				int genericAllocationTotalCost = CombatSkillHelper.GetGenericAllocationTotalCost(b, b2);
				genericCount -= genericAllocationTotalCost;
			}
		}
		else if (_leaderId == DomainManager.Taiwu.GetTaiwuCharId())
		{
			CharacterCombatSkillConfiguration characterCombatSkillConfiguration = DomainManager.Extra.TryGetCharacterCombatSkillConfiguration(_id);
			if (characterCombatSkillConfiguration == null)
			{
				return genericCount;
			}
			byte[] genericGridAllocation2 = characterCombatSkillConfiguration.CurrentEquipPlan.GenericGridAllocation;
			for (sbyte b3 = 1; b3 < 5; b3++)
			{
				sbyte b4 = (sbyte)genericGridAllocation2[b3 - 1];
				slotCounts[b3] += b4;
				int genericAllocationTotalCost2 = CombatSkillHelper.GetGenericAllocationTotalCost(b3, b4);
				genericCount -= genericAllocationTotalCost2;
			}
		}
		else
		{
			for (sbyte b5 = 1; b5 < 5; b5++)
			{
				sbyte b6 = (sbyte)GetCombatSkillTypeRequireGrid(b5);
				if (b6 > slotCounts[b5])
				{
					sbyte b7 = (sbyte)(b6 - slotCounts[b5]);
					int genericAllocationTotalCost3 = CombatSkillHelper.GetGenericAllocationTotalCost(b5, b7);
					if (b7 <= genericAllocationTotalCost3)
					{
						slotCounts[b5] = b6;
						genericCount -= genericAllocationTotalCost3;
					}
				}
			}
		}
		return genericCount;
	}

	public sbyte GetCombatSkillSlotCounts(Span<sbyte> slotCounts)
	{
		CombatSkillEquipment combatSkillEquipment = GetCombatSkillEquipment();
		return GetCombatSkillSlotCounts(slotCounts, combatSkillEquipment.Neigong);
	}

	public sbyte GetCombatSkillSlotCounts(Span<sbyte> slotCounts, ArraySegmentList<short> neigongList)
	{
		for (sbyte b = 0; b < 5; b++)
		{
			slotCounts[b] = GetCombatSkillSlotCount(b, neigongList);
		}
		return GetCombatSkillBasicSlotCount(5, neigongList);
	}

	public void GetCombatSkillExtraSlotCounts(Span<sbyte> slotCounts)
	{
		for (sbyte b = 0; b < 5; b++)
		{
			slotCounts[b] = GetCombatSkillExtraSlotCount(b);
		}
	}

	private sbyte GetCombatSkillSlotCount(sbyte equipType)
	{
		CombatSkillEquipment combatSkillEquipment = GetCombatSkillEquipment();
		return GetCombatSkillSlotCount(equipType, combatSkillEquipment.Neigong);
	}

	private sbyte GetCombatSkillSlotCount(sbyte equipType, ArraySegmentList<short> neigongList)
	{
		if (equipType == 0)
		{
			return GetCombatSkillSlotCountNeigong();
		}
		int combatSkillBasicSlotCount = GetCombatSkillBasicSlotCount(equipType, neigongList);
		combatSkillBasicSlotCount += GetCombatSkillExtraSlotCount(equipType);
		return (sbyte)Math.Clamp(combatSkillBasicSlotCount, 0, 99);
	}

	public sbyte GetCombatSkillSlotCountNeigong()
	{
		int num = GlobalConfig.Instance.CombatSkillInitialEquipSlotCounts[0];
		num += GetCombatSkillExtraSlotCount(0);
		return (sbyte)Math.Clamp(num, 0, 99);
	}

	private sbyte GetCombatSkillGenericSlotCount()
	{
		return GetCombatSkillBasicSlotCount(5);
	}

	private sbyte GetCombatSkillBasicSlotCount(sbyte equipType)
	{
		CombatSkillEquipment combatSkillEquipment = GetCombatSkillEquipment();
		return GetCombatSkillBasicSlotCount(equipType, combatSkillEquipment.Neigong);
	}

	public sbyte GetCombatSkillBasicSlotCount(sbyte equipType, ArraySegmentList<short> neigongList)
	{
		if (equipType == 0)
		{
			return GlobalConfig.Instance.CombatSkillInitialEquipSlotCounts[0];
		}
		bool flag = equipType == 5;
		int num = GlobalConfig.Instance.CombatSkillInitialEquipSlotCounts[equipType];
		sbyte b = MixedPoisonType.FromCombatSkillEquipType(equipType);
		if (b != -1)
		{
			num -= Math.Max(0, GetRealMixedPoisonTypeRelatedMarkCount(b) - 2);
		}
		sbyte b2 = GetCombatSkillSlotCountNeigong();
		ArraySegmentList<short>.Enumerator enumerator = neigongList.GetEnumerator();
		while (enumerator.MoveNext())
		{
			short current = enumerator.Current;
			if (current >= 0)
			{
				sbyte combatSkillGridCost = GetCombatSkillGridCost(current);
				if (combatSkillGridCost <= b2 && GetCombatSkillCanAffectBasic(current))
				{
					b2 -= combatSkillGridCost;
					GameData.Domains.CombatSkill.CombatSkill element_CombatSkills = DomainManager.CombatSkill.GetElement_CombatSkills((charId: _id, skillId: current));
					num += (flag ? element_CombatSkills.GetGenericGridCount() : element_CombatSkills.GetSpecificGridCount(equipType));
				}
			}
		}
		sbyte max = (flag ? sbyte.MaxValue : CombatSkillHelper.MaxSlotCounts[equipType]);
		return (sbyte)Math.Clamp(num, 0, max);
	}

	private sbyte GetCombatSkillExtraSlotCount(sbyte equipType)
	{
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		int num = 0;
		foreach (short featureId in _featureIds)
		{
			num += CharacterFeature.Instance[featureId].CombatSkillSlotBonuses[equipType];
		}
		sbyte[] extraCombatSkillGrids = Template.ExtraCombatSkillGrids;
		sbyte[] extraCombatSkillGrids2 = OrganizationDomain.GetOrgMemberConfig(_organizationInfo).ExtraCombatSkillGrids;
		if (CanAffectedByCombatDifficulty)
		{
			byte combatDifficulty = DomainManager.World.GetCombatDifficulty();
			CValuePercent val = CValuePercent.op_Implicit((int)CombatDifficulty.Instance[combatDifficulty].ExtraCombatSkillGrids);
			num += extraCombatSkillGrids[equipType] + (int)extraCombatSkillGrids2[equipType] * val;
		}
		else
		{
			num += extraCombatSkillGrids[equipType] + extraCombatSkillGrids2[equipType];
		}
		return (sbyte)Math.Clamp(num, 0, 127);
	}

	public void ResetCombatStatus(DataContext context)
	{
		ClearEatingItems(context);
		_injuries.Initialize();
		SetInjuries(_injuries, context);
		_poisoned.Initialize();
		SetPoisoned(ref _poisoned, context);
		SetDisorderOfQi(0, context);
		SetHealth(GetLeftMaxHealth(), context);
	}

	public int CalcNeiliAllocationStepCount(ECharacterPropertyReferencedType type)
	{
		NeiliAllocation neiliAllocation = GetNeiliAllocation();
		NeiliAllocation allocatedNeiliEffects = GetAllocatedNeiliEffects();
		return type.CalcNeiliAllocationStepCount(neiliAllocation, allocatedNeiliEffects);
	}

	public int CalcNeiliAllocationBonus(ECharacterPropertyReferencedType type)
	{
		sbyte neiliType = GetNeiliType();
		NeiliTypeItem config = NeiliType.Instance[neiliType];
		int mapping = config.GetMapping(type);
		if (mapping == 0)
		{
			return 0;
		}
		int num = CalcNeiliAllocationStepCount(type);
		int num2 = mapping * num;
		CombatSkillEquipment combatSkillEquipment = GetCombatSkillEquipment();
		foreach (short item in combatSkillEquipment)
		{
			if (item >= 0 && DomainManager.CombatSkill.TryGetElement_CombatSkills((charId: _id, skillId: item), out var element))
			{
				num2 += element.CalcNeiliAllocationBonus(type, num);
			}
		}
		return num2;
	}

	public short GetRandomFavorability(IRandomSource random)
	{
		CharacterItem characterItem = Config.Character.Instance[_templateId];
		int randomIndex = RandomUtils.GetRandomIndex(characterItem.RandomEnemyFavorability, random);
		(short, short) tuple = RandomFavorabilityRanges[randomIndex];
		return (short)random.Next((int)tuple.Item1, tuple.Item2 + 1);
	}

	public sbyte CalcWeaponInnerRatio(short weaponTemplateId, sbyte expectRatio)
	{
		WeaponItem weaponItem = Config.Weapon.Instance[weaponTemplateId];
		int defaultInnerRatio = weaponItem.DefaultInnerRatio;
		int num = weaponItem.InnerRatioAdjustRange * GetInnerRatio() / 100;
		int min = Math.Max(defaultInnerRatio - num, 0);
		int max = Math.Min(defaultInnerRatio + num, 100);
		return (sbyte)Math.Clamp(expectRatio, min, max);
	}

	private int GetPureMaxNeili()
	{
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		int num = ((_templateId != 836) ? GlobalConfig.Instance.CharacterInitialNeili : 0);
		Dictionary<short, GameData.Domains.CombatSkill.CombatSkill> charCombatSkills = DomainManager.CombatSkill.GetCharCombatSkills(_id);
		foreach (var (item, combatSkill2) in charCombatSkills)
		{
			Tester.Assert(_learnedCombatSkills.Contains(item));
			if (!combatSkill2.GetRevoked())
			{
				num += combatSkill2.GetObtainedNeili();
			}
		}
		num += _extraNeili;
		if (_id == DomainManager.Taiwu.GetTaiwuCharId())
		{
			CharacterPropertyBonus taiwuPropertyPermanentBonus = DomainManager.Extra.GetTaiwuPropertyPermanentBonus(ECharacterPropertyReferencedType.MaxNeili);
			num *= (CValueModify)taiwuPropertyPermanentBonus;
		}
		GearMate value;
		if (_id == DomainManager.Taiwu.GetTaiwuCharId())
		{
			num -= DomainManager.Extra.GetAllGearMateNeili();
		}
		else if (DomainManager.Extra.TryGetElement_SectZhujianGearMates(_id, out value))
		{
			num += value.Neili;
		}
		return num;
	}

	private bool TryGetFixedSubAttributeValue(out short fixedSubAttributeValue)
	{
		bool flag = DomainManager.SpecialEffect.ModifyData(_id, -1, 17, dataValue: false);
		bool flag2 = DomainManager.SpecialEffect.ModifyData(_id, -1, 18, dataValue: false);
		bool flag3 = flag != flag2;
		if (flag3)
		{
			fixedSubAttributeValue = (short)(flag ? 1000 : 0);
		}
		else
		{
			fixedSubAttributeValue = -1;
		}
		return flag3;
	}

	private int CalcOverloadBonus(int value, IReadOnlyList<int> configBonus)
	{
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		if (DomainManager.SpecialEffect.ModifyData(_id, -1, 279, dataValue: false))
		{
			return value;
		}
		int currEquipmentLoad = GetCurrEquipmentLoad();
		int maxEquipmentLoad = GetMaxEquipmentLoad();
		if (currEquipmentLoad <= maxEquipmentLoad || maxEquipmentLoad <= 0)
		{
			return value;
		}
		int index = Math.Min((currEquipmentLoad - 1) / maxEquipmentLoad - 1, configBonus.Count - 1);
		CValuePercent val = CValuePercent.op_Implicit(configBonus[index]);
		return value * val;
	}

	private int CalcTreasuryGuardBonus(int value)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		return value * CValuePercent.op_Implicit(GlobalConfig.Instance.TreasuryGuardPropertyPercent);
	}

	private bool IsFeatureMakeRelated()
	{
		for (int i = 0; i < _featureIds.Count; i++)
		{
			if (CharacterFeature.Instance[_featureIds[i]].MakeConsummateLevelRelated)
			{
				return true;
			}
		}
		return false;
	}

	private int CalcMainAttributeRelatedConsummateLevel(ushort affectedDataFieldId, sbyte mainAttributeType, int hitOrInnerType, int divisor)
	{
		bool flag = DomainManager.SpecialEffect.ModifyData(_id, -1, affectedDataFieldId, dataValue: false, mainAttributeType, hitOrInnerType);
		short maxMainAttribute = GetMaxMainAttribute(mainAttributeType);
		int num = 100 + maxMainAttribute / 2;
		int num2 = maxMainAttribute / divisor * _consummateLevel;
		if (flag)
		{
			num += num2;
		}
		if (IsFeatureMakeRelated())
		{
			num += num2;
		}
		return num;
	}

	public void AddEquippedCombatSkill(DataContext context, short skillTemplateId)
	{
		if (skillTemplateId >= 0)
		{
			CombatSkillItem combatSkillItem = Config.CombatSkill.Instance[skillTemplateId];
			sbyte equipType = combatSkillItem.EquipType;
			GameData.Domains.CombatSkill.CombatSkill element_CombatSkills = DomainManager.CombatSkill.GetElement_CombatSkills(new CombatSkillKey(_id, skillTemplateId));
			if (element_CombatSkills.GetRevoked())
			{
				throw new Exception($"Combat skill {combatSkillItem.Name} of character {this} is revoked");
			}
			sbyte combatSkillGridCost = GetCombatSkillGridCost(skillTemplateId);
			int num = GetCombatSkillSlotCountWithGeneric(equipType) - GetCombatSkillTypeRequireGrid(equipType);
			if (num < combatSkillGridCost)
			{
				throw new Exception($"Combat skill {combatSkillItem.Name} exceed slot count: {num} available, {combatSkillGridCost} required.");
			}
		}
		CombatSkillEquipment combatSkillEquipment = GetCombatSkillEquipment();
		combatSkillEquipment.OfflineAddSkill(skillTemplateId);
		ApplyCombatSkillEquipmentModification(context, combatSkillEquipment);
	}

	public void RemoveEquippedCombatSkill(DataContext context, short skillTemplateId)
	{
		CombatSkillEquipment combatSkillEquipment = GetCombatSkillEquipment();
		if (combatSkillEquipment.OfflineRemoveSkill(skillTemplateId))
		{
			ApplyCombatSkillEquipmentModification(context, combatSkillEquipment);
		}
	}

	public void ClearCombatSkillEquipment(DataContext context)
	{
		CombatSkillHelper.InitializeEquippedSkills(_equippedCombatSkills);
		SetEquippedCombatSkills(_equippedCombatSkills, context);
		DomainManager.Extra.RemoveCharacterEquippedCombatSkills(context, _id);
		DomainManager.SpecialEffect.UpdateEquippedSkillEffect(context, this);
		if (IsTaiwu())
		{
			byte[] genericGridAllocation = DomainManager.Taiwu.GetGenericGridAllocation();
			Array.Fill(genericGridAllocation, (byte)0);
			DomainManager.Taiwu.SetGenericGridAllocation(context, genericGridAllocation);
		}
		else if (_leaderId == DomainManager.Taiwu.GetTaiwuCharId())
		{
			CharacterCombatSkillConfiguration characterCombatSkillConfiguration = DomainManager.Extra.TryGetCharacterCombatSkillConfiguration(_id);
			if (characterCombatSkillConfiguration != null)
			{
				Array.Fill(characterCombatSkillConfiguration.CurrentEquipPlan.GenericGridAllocation, (byte)0);
				DomainManager.Extra.SetCharacterCombatSkillConfiguration(context, _id, characterCombatSkillConfiguration);
			}
		}
	}

	public bool ClearExceededCombatSkills(DataContext context, sbyte equipType)
	{
		Span<sbyte> slotCounts = stackalloc sbyte[5];
		GetCombatSkillSlotCountsWithGeneric(slotCounts);
		return ClearExceededCombatSkills(context, equipType, slotCounts[equipType]);
	}

	public bool ClearExceededCombatSkills(DataContext context, sbyte equipType, int slotCount)
	{
		int num = GetCombatSkillTypeRequireGrid(equipType);
		CombatSkillEquipment combatSkillEquipment = GetCombatSkillEquipment();
		bool flag = false;
		while (num > slotCount)
		{
			short num2 = combatSkillEquipment.OfflineRemoveLastSkill(equipType);
			if (num2 < 0)
			{
				break;
			}
			num -= GetCombatSkillGridCost(num2);
			flag = true;
		}
		if (flag)
		{
			ApplyCombatSkillEquipmentModification(context, combatSkillEquipment);
		}
		return flag;
	}

	public void UpdateAllocatedGenericGrids(DataContext context)
	{
		int num = GetCombatSkillGenericSlotCount();
		CharacterCombatSkillConfiguration characterCombatSkillConfiguration = DomainManager.Extra.TryGetCharacterCombatSkillConfiguration(_id);
		bool flag = IsTaiwu();
		byte[] array = (flag ? DomainManager.Taiwu.GetGenericGridAllocation() : characterCombatSkillConfiguration?.CurrentEquipPlan.GenericGridAllocation);
		if (array == null)
		{
			return;
		}
		bool flag2 = false;
		for (int i = 0; i < array.Length; i++)
		{
			sbyte b = (sbyte)(i + 1);
			byte b2 = array[i];
			sbyte b3 = CombatSkillHelper.MaxSlotCounts[b];
			byte b4 = (byte)(b3 - GetCombatSkillBasicSlotCount(b));
			if (b2 > b4)
			{
				b2 = (array[i] = b4);
				flag2 = true;
			}
			int genericAllocationTotalCost = CombatSkillHelper.GetGenericAllocationTotalCost(b, b2);
			if (num >= genericAllocationTotalCost)
			{
				num -= genericAllocationTotalCost;
				continue;
			}
			do
			{
				b2--;
				genericAllocationTotalCost = CombatSkillHelper.GetGenericAllocationTotalCost(b, b2);
			}
			while (num < genericAllocationTotalCost && b2 > 0);
			array[i] = b2;
			num -= genericAllocationTotalCost;
			flag2 = true;
		}
		if (flag2)
		{
			if (flag)
			{
				DomainManager.Taiwu.SetGenericGridAllocation(context, array);
			}
			else
			{
				DomainManager.Extra.SetCharacterCombatSkillConfiguration(context, _id, characterCombatSkillConfiguration);
			}
		}
	}

	public void ApplyCombatSkillEquipmentModification(DataContext context, CombatSkillEquipment combatSkillEquipment)
	{
		SetEquippedCombatSkills(_equippedCombatSkills, context);
		CombatSkillPlan sourceObject = combatSkillEquipment.GetSourceObject<CombatSkillPlan>();
		if (sourceObject != null)
		{
			DomainManager.Extra.SetCharacterEquippedCombatSkills(context, _id, sourceObject);
		}
		else
		{
			DomainManager.Extra.RemoveCharacterEquippedCombatSkills(context, _id);
		}
		UpdateAllocatedGenericGrids(context);
		DomainManager.SpecialEffect.UpdateEquippedSkillEffect(context, this);
	}

	public bool IsCombatSkillEquipped(short skillTemplateId)
	{
		CombatSkillEquipment combatSkillEquipment = GetCombatSkillEquipment();
		return combatSkillEquipment.IsCombatSkillEquipped(skillTemplateId);
	}

	internal void CopyCombatSkillEquipmentFrom(DataContext context, Character other)
	{
		SetEquippedCombatSkills(other._equippedCombatSkills.ToArray(), context);
		if (DomainManager.Extra.TryGetCharacterEquippedCombatSkills(other._id, out var plan))
		{
			if (DomainManager.Extra.TryGetCharacterEquippedCombatSkills(_id, out var plan2))
			{
				plan2.Assign(plan);
			}
			else
			{
				plan2 = new CombatSkillPlan(plan);
			}
			DomainManager.Extra.SetCharacterEquippedCombatSkills(context, _id, plan2);
		}
		else
		{
			DomainManager.Extra.RemoveCharacterEquippedCombatSkills(context, _id);
		}
		DomainManager.SpecialEffect.UpdateEquippedSkillEffect(context, this);
	}

	public sbyte GetCombatSkillGridCost(short skillTemplateId)
	{
		sbyte b = Config.CombatSkill.Instance[skillTemplateId].GridCost;
		if (_id == DomainManager.Taiwu.GetTaiwuCharId())
		{
			b = (sbyte)DomainManager.SpecialEffect.ModifyData(_id, skillTemplateId, (ushort)211, (int)b, -1, -1, -1);
		}
		if (DomainManager.Extra.IsCombatSkillMasteredByCharacter(_id, skillTemplateId))
		{
			b--;
		}
		return Math.Max(b, 1);
	}

	public int GetCombatSkillTypeRequireGrid(sbyte combatSkillEquipType)
	{
		int num = 0;
		ArraySegmentList<short> arraySegmentList = GetCombatSkillEquipment()[combatSkillEquipType];
		ArraySegmentList<short>.Enumerator enumerator = arraySegmentList.GetEnumerator();
		while (enumerator.MoveNext())
		{
			short current = enumerator.Current;
			if (current >= 0)
			{
				num += GetCombatSkillGridCost(current);
			}
		}
		return num;
	}

	public bool GetCombatSkillCanAffect(short skillTemplateId)
	{
		if (skillTemplateId < 0)
		{
			return false;
		}
		if (_leaderId != DomainManager.Taiwu.GetTaiwuCharId())
		{
			return true;
		}
		sbyte equipType = Config.CombatSkill.Instance[skillTemplateId].EquipType;
		CombatSkillEquipment combatSkillEquipment = GetCombatSkillEquipment();
		int num = combatSkillEquipment[equipType].IndexOf(skillTemplateId);
		if (num < 0)
		{
			return true;
		}
		if (!GetCombatSkillCanAffectBasic(skillTemplateId))
		{
			return false;
		}
		sbyte combatSkillSlotCountWithGeneric = GetCombatSkillSlotCountWithGeneric(equipType);
		int num2 = 0;
		ArraySegmentList<short> arraySegmentList = combatSkillEquipment[equipType];
		for (int i = 0; i < arraySegmentList.Count; i++)
		{
			short num3 = arraySegmentList[i];
			num2 += GetCombatSkillGridCost(num3);
			if (num3 == skillTemplateId)
			{
				return num2 <= combatSkillSlotCountWithGeneric;
			}
		}
		return false;
	}

	private bool GetCombatSkillCanAffectBasic(short skillTemplateId)
	{
		return !IsTaiwu() || DomainManager.Extra.GetConflictCombatSkill(skillTemplateId) == null;
	}

	public GameData.Domains.CombatSkill.CombatSkill LearnNewCombatSkill(DataContext context, short combatSkillTemplateId, ushort readingState)
	{
		GameData.Domains.CombatSkill.CombatSkill combatSkill = DomainManager.CombatSkill.CreateCombatSkill(_id, combatSkillTemplateId, readingState);
		_learnedCombatSkills.Add(combatSkillTemplateId);
		SetLearnedCombatSkills(_learnedCombatSkills, context);
		if (_id == DomainManager.Taiwu.GetTaiwuCharId())
		{
			DomainManager.Taiwu.RegisterCombatSkill(context, combatSkill);
			DomainManager.Taiwu.AddLegacyPoint(context, 20);
		}
		return combatSkill;
	}

	public void GetLearnedCombatSkillsFromSect(List<short> result, sbyte orgTemplateId, sbyte minGrade = 0, sbyte maxGrade = 8)
	{
		result.Clear();
		foreach (short learnedCombatSkill in _learnedCombatSkills)
		{
			CombatSkillItem combatSkillItem = Config.CombatSkill.Instance[learnedCombatSkill];
			if (combatSkillItem.SectId == orgTemplateId && combatSkillItem.Grade >= minGrade && combatSkillItem.Grade <= maxGrade)
			{
				result.Add(learnedCombatSkill);
			}
		}
	}

	public sbyte GetLearnedCombatSkillMaxGradeByType(sbyte combatSkillType)
	{
		sbyte b = 0;
		int i = 0;
		for (int count = _learnedCombatSkills.Count; i < count; i++)
		{
			short index = _learnedCombatSkills[i];
			CombatSkillItem combatSkillItem = Config.CombatSkill.Instance[index];
			if (combatSkillItem.Type == combatSkillType && combatSkillItem.Grade > b)
			{
				b = combatSkillItem.Grade;
			}
		}
		return b;
	}

	public int GetLearnedCombatSkillTotalValue(sbyte lifeSkillType)
	{
		Dictionary<short, GameData.Domains.CombatSkill.CombatSkill> charCombatSkills = DomainManager.CombatSkill.GetCharCombatSkills(_id);
		int num = 0;
		foreach (var (num3, combatSkill2) in charCombatSkills)
		{
			if (combatSkill2.Template.Type == lifeSkillType)
			{
				int baseValue = ItemTemplateHelper.GetBaseValue(10, combatSkill2.Template.BookId);
				int num4 = baseValue * combatSkill2.GetReadNormalPagesCount() / 5;
				num += num4;
			}
		}
		return num;
	}

	public unsafe short GetCombatSkillAttainment(sbyte combatSkillType)
	{
		return GetCombatSkillAttainments().Items[combatSkillType];
	}

	public unsafe short GetCombatSkillQualification(sbyte combatSkillType)
	{
		return GetCombatSkillQualifications().Items[combatSkillType];
	}

	public int GetSkillBreakoutStepsMaxPower(short combatSkillTemplateId)
	{
		GameData.Domains.CombatSkill.CombatSkill element;
		if (IsTaiwu())
		{
			return DomainManager.CombatSkill.TryGetElement_CombatSkills((charId: _id, skillId: combatSkillTemplateId), out element) ? element.GetPlateAddMaxPower() : 0;
		}
		if (IsGearMate)
		{
			GearMate gearMateById = DomainManager.Extra.GetGearMateById(_id);
			CombatSkillKey objectId = new CombatSkillKey(_id, combatSkillTemplateId);
			if (!DomainManager.CombatSkill.TryGetElement_CombatSkills(objectId, out var element2))
			{
				return 0;
			}
			if (!CombatSkillStateHelper.IsBrokenOut(element2.GetActivationState()))
			{
				return 0;
			}
			if (gearMateById.SkillBreakMaxPowerDict.TryGetValue(combatSkillTemplateId, out var value))
			{
				return value;
			}
		}
		CombatSkillItem combatSkillItem = Config.CombatSkill.Instance[combatSkillTemplateId];
		int skillBreakoutStepsPercentage = GetSkillBreakoutStepsPercentage(combatSkillTemplateId);
		return combatSkillItem.SkillBreakPlate.TotalMaxPower * skillBreakoutStepsPercentage / 100;
	}

	public int GetSkillBreakoutStepsPercentage(short combatSkillTemplateId)
	{
		sbyte skillBreakoutAvailableStepsCount = GetSkillBreakoutAvailableStepsCount(combatSkillTemplateId);
		return skillBreakoutAvailableStepsCount * 100 / GlobalConfig.Instance.BreakoutBaseAvailableStepsCount;
	}

	public sbyte GetSkillBreakoutAvailableStepsCount(short combatSkillTemplateId)
	{
		if (_creatingType != 1)
		{
			return GlobalConfig.Instance.BreakoutSpecialNpcStepsCount;
		}
		CombatSkillItem combatSkillItem = Config.CombatSkill.Instance[combatSkillTemplateId];
		SkillGradeDataItem skillGradeDataItem = SkillGradeData.Instance[combatSkillItem.Grade];
		bool flag = IsTaiwu();
		short practiceQualificationRequirement = skillGradeDataItem.PracticeQualificationRequirement;
		short num = GetCombatSkillQualification(combatSkillItem.Type);
		if (flag)
		{
			num = DomainManager.Taiwu.GetQualificationWithSectApprovalBonus(combatSkillItem.SectId, num, practiceQualificationRequirement);
		}
		int num2 = ((num >= practiceQualificationRequirement) ? GlobalConfig.Instance.BreakoutBaseAvailableStepsCount : ((num >= practiceQualificationRequirement / 2) ? Math.Min(15 * num / Math.Max((short)1, practiceQualificationRequirement), 15) : Math.Min(10 * num / Math.Max((short)1, practiceQualificationRequirement), 10)));
		Location validLocation = GetValidLocation();
		if (_creatingType == 1)
		{
			num2 += DomainManager.Building.GetBuildingBlockEffect(validLocation, EBuildingScaleEffect.BreakOutSteps);
		}
		int index = ((!IsLoseConsummateBonusByFeature()) ? GetConsummateLevel() : 0);
		num2 += ConsummateLevel.Instance[index].AddBreakStepCount;
		if (!flag)
		{
			num2 += GetInteractionGrade() - 3;
		}
		else
		{
			if (validLocation.IsValid())
			{
				AreaAdventureData adventuresInArea = DomainManager.Adventure.GetAdventuresInArea(validLocation.AreaId);
				if (adventuresInArea.AdventureSites.TryGetValue(validLocation.BlockId, out var value) && value.SiteState >= 2 && value.TemplateId == 42)
				{
					num2 += 3;
				}
			}
			if (combatSkillItem.SectId != 0)
			{
				short settlementIdByOrgTemplateId = DomainManager.Organization.GetSettlementIdByOrgTemplateId(combatSkillItem.SectId);
				Sect element_Sects = DomainManager.Organization.GetElement_Sects(settlementIdByOrgTemplateId);
				short num3 = element_Sects.CalcApprovingRate();
				if (num3 >= 700)
				{
					num2 += 3;
				}
			}
		}
		return (sbyte)Math.Clamp(num2, GlobalConfig.Instance.BreakoutMinAvailableStepsCount, GlobalConfig.Instance.BreakoutMaxAvailableStepsCount);
	}

	public unsafe int GetPropertyValue(ECharacterPropertyReferencedType type)
	{
		switch (type)
		{
		case ECharacterPropertyReferencedType.Strength:
		case ECharacterPropertyReferencedType.Dexterity:
		case ECharacterPropertyReferencedType.Concentration:
		case ECharacterPropertyReferencedType.Vitality:
		case ECharacterPropertyReferencedType.Energy:
		case ECharacterPropertyReferencedType.Intelligence:
		{
			MainAttributes maxMainAttributes = GetMaxMainAttributes();
			int num3 = (int)(0 + type - 0);
			return maxMainAttributes.Items[num3];
		}
		case ECharacterPropertyReferencedType.AttainmentMusic:
		case ECharacterPropertyReferencedType.AttainmentChess:
		case ECharacterPropertyReferencedType.AttainmentPoem:
		case ECharacterPropertyReferencedType.AttainmentPainting:
		case ECharacterPropertyReferencedType.AttainmentMath:
		case ECharacterPropertyReferencedType.AttainmentAppraisal:
		case ECharacterPropertyReferencedType.AttainmentForging:
		case ECharacterPropertyReferencedType.AttainmentWoodworking:
		case ECharacterPropertyReferencedType.AttainmentMedicine:
		case ECharacterPropertyReferencedType.AttainmentToxicology:
		case ECharacterPropertyReferencedType.AttainmentWeaving:
		case ECharacterPropertyReferencedType.AttainmentJade:
		case ECharacterPropertyReferencedType.AttainmentTaoism:
		case ECharacterPropertyReferencedType.AttainmentBuddhism:
		case ECharacterPropertyReferencedType.AttainmentCooking:
		case ECharacterPropertyReferencedType.AttainmentEclectic:
		{
			ref LifeSkillShorts lifeSkillAttainments = ref GetLifeSkillAttainments();
			int num2 = (int)(0 + type - 50);
			return lifeSkillAttainments.Items[num2];
		}
		case ECharacterPropertyReferencedType.AttainmentNeigong:
		case ECharacterPropertyReferencedType.AttainmentPosing:
		case ECharacterPropertyReferencedType.AttainmentStunt:
		case ECharacterPropertyReferencedType.AttainmentFistAndPalm:
		case ECharacterPropertyReferencedType.AttainmentFinger:
		case ECharacterPropertyReferencedType.AttainmentLeg:
		case ECharacterPropertyReferencedType.AttainmentThrow:
		case ECharacterPropertyReferencedType.AttainmentSword:
		case ECharacterPropertyReferencedType.AttainmentBlade:
		case ECharacterPropertyReferencedType.AttainmentPolearm:
		case ECharacterPropertyReferencedType.AttainmentSpecial:
		case ECharacterPropertyReferencedType.AttainmentWhip:
		case ECharacterPropertyReferencedType.AttainmentControllableShot:
		case ECharacterPropertyReferencedType.AttainmentCombatMusic:
		{
			ref CombatSkillShorts combatSkillAttainments = ref GetCombatSkillAttainments();
			int num = (int)(0 + type - 80);
			return combatSkillAttainments.Items[num];
		}
		default:
			throw new Exception($"Cannot get value of character property type {type}");
		}
	}

	public unsafe void ModifyBasePropertyValue(DataContext context, ECharacterPropertyReferencedType type, int delta)
	{
		switch (type)
		{
		case ECharacterPropertyReferencedType.Strength:
		case ECharacterPropertyReferencedType.Dexterity:
		case ECharacterPropertyReferencedType.Concentration:
		case ECharacterPropertyReferencedType.Vitality:
		case ECharacterPropertyReferencedType.Energy:
		case ECharacterPropertyReferencedType.Intelligence:
		{
			int num3 = (int)(0 + type - 0);
			int num4 = _baseMainAttributes.Items[num3] + delta;
			if (num4 < 0)
			{
				num4 = 0;
			}
			_baseMainAttributes.Items[num3] = (short)num4;
			SetBaseMainAttributes(_baseMainAttributes, context);
			break;
		}
		case ECharacterPropertyReferencedType.QualificationMusic:
		case ECharacterPropertyReferencedType.QualificationChess:
		case ECharacterPropertyReferencedType.QualificationPoem:
		case ECharacterPropertyReferencedType.QualificationPainting:
		case ECharacterPropertyReferencedType.QualificationMath:
		case ECharacterPropertyReferencedType.QualificationAppraisal:
		case ECharacterPropertyReferencedType.QualificationForging:
		case ECharacterPropertyReferencedType.QualificationWoodworking:
		case ECharacterPropertyReferencedType.QualificationMedicine:
		case ECharacterPropertyReferencedType.QualificationToxicology:
		case ECharacterPropertyReferencedType.QualificationWeaving:
		case ECharacterPropertyReferencedType.QualificationJade:
		case ECharacterPropertyReferencedType.QualificationTaoism:
		case ECharacterPropertyReferencedType.QualificationBuddhism:
		case ECharacterPropertyReferencedType.QualificationCooking:
		case ECharacterPropertyReferencedType.QualificationEclectic:
		{
			int num5 = (int)(0 + type - 34);
			int num6 = _baseLifeSkillQualifications.Items[num5] + delta;
			if (num6 < 0)
			{
				num6 = 0;
			}
			_baseLifeSkillQualifications.Items[num5] = (short)num6;
			SetBaseLifeSkillQualifications(ref _baseLifeSkillQualifications, context);
			break;
		}
		case ECharacterPropertyReferencedType.QualificationNeigong:
		case ECharacterPropertyReferencedType.QualificationPosing:
		case ECharacterPropertyReferencedType.QualificationStunt:
		case ECharacterPropertyReferencedType.QualificationFistAndPalm:
		case ECharacterPropertyReferencedType.QualificationFinger:
		case ECharacterPropertyReferencedType.QualificationLeg:
		case ECharacterPropertyReferencedType.QualificationThrow:
		case ECharacterPropertyReferencedType.QualificationSword:
		case ECharacterPropertyReferencedType.QualificationBlade:
		case ECharacterPropertyReferencedType.QualificationPolearm:
		case ECharacterPropertyReferencedType.QualificationSpecial:
		case ECharacterPropertyReferencedType.QualificationWhip:
		case ECharacterPropertyReferencedType.QualificationControllableShot:
		case ECharacterPropertyReferencedType.QualificationCombatMusic:
		{
			int num = (int)(0 + type - 66);
			int num2 = _baseCombatSkillQualifications.Items[num] + delta;
			if (num2 < 0)
			{
				num2 = 0;
			}
			_baseCombatSkillQualifications.Items[num] = (short)num2;
			SetBaseCombatSkillQualifications(ref _baseCombatSkillQualifications, context);
			break;
		}
		default:
			throw new Exception($"Cannot get value of character property type {type}");
		}
	}

	public static bool IsCharacterIdValid(int charId)
	{
		return charId >= 0;
	}

	public static bool IsOppositeGender(Character characterA, Character characterB)
	{
		return characterA._transgender || characterB.CheckGenderMeetsRequirement(Gender.Flip(characterA._gender));
	}

	public int GetAdjustedResourceSatisfyingThreshold(sbyte resourceType)
	{
		OrganizationMemberItem orgMemberConfig = OrganizationDomain.GetOrgMemberConfig(_organizationInfo);
		return orgMemberConfig.GetAdjustedResourceSatisfyingThreshold(resourceType);
	}

	public int GetAdjustedResourceSatisfyingAmount(sbyte resourceType)
	{
		OrganizationMemberItem orgMemberConfig = OrganizationDomain.GetOrgMemberConfig(_organizationInfo);
		return orgMemberConfig.GetAdjustedResourceSatisfyingAmount(resourceType);
	}

	public bool IdentifyCanCraftItem(sbyte itemType, short itemTemplateId)
	{
		OrganizationMemberItem orgMemberConfig = OrganizationDomain.GetOrgMemberConfig(_organizationInfo);
		return orgMemberConfig.CanCraftItem(itemType, itemTemplateId);
	}

	public int GetExpPerMonth()
	{
		OrganizationMemberItem orgMemberConfig = OrganizationDomain.GetOrgMemberConfig(_organizationInfo);
		int num = orgMemberConfig.ExpPerMonth;
		if (_organizationInfo.SettlementId >= 0 && OrganizationDomain.IsSect(_organizationInfo.OrgTemplateId))
		{
			Settlement settlement = DomainManager.Organization.GetSettlement(_organizationInfo.SettlementId);
			num = num * settlement.GetMemberSelfImproveSpeedFactor() / 100;
		}
		return num;
	}

	public int GetContributionPerMonth()
	{
		OrganizationMemberItem orgMemberConfig = OrganizationDomain.GetOrgMemberConfig(_organizationInfo);
		int num = orgMemberConfig.ContributionPerMonth;
		if (_organizationInfo.SettlementId >= 0 && OrganizationDomain.IsSect(_organizationInfo.OrgTemplateId))
		{
			Settlement settlement = DomainManager.Organization.GetSettlement(_organizationInfo.SettlementId);
			num = num * settlement.GetMemberSelfImproveSpeedFactor() / 100;
		}
		return num;
	}

	public bool OrgAndMonkTypeAllowMarriage()
	{
		return _monkType == 0 && OrganizationDomain.GetOrgMemberConfig(_organizationInfo).ChildGrade >= 0;
	}

	public sbyte GetLegendaryBookOwnerState()
	{
		if (DomainManager.Extra.IsLegendaryBookConsumed(_id))
		{
			return 3;
		}
		if (DomainManager.LegendaryBook.GetCharOwnedBookTypes(_id) == null)
		{
			return -1;
		}
		if (_featureIds.Contains(204))
		{
			return 1;
		}
		if (_featureIds.Contains(205))
		{
			return 2;
		}
		return 0;
	}

	public bool IsCreatedWithFixedTemplate()
	{
		return CreatingType.IsFixedPresetType(_creatingType);
	}

	public int GetSrcCharId()
	{
		return _srcCharId;
	}

	public sbyte GetAgeGroup()
	{
		return AgeGroup.GetAgeGroup(_currAge);
	}

	public EMarriageAgeGroup GetMarriageAgeGroup()
	{
		return MarriageAgeGroupHelper.GetMarriageAgeGroup(_currAge);
	}

	public bool CheckGenderMeetsRequirement(sbyte requiredGender)
	{
		return _gender == requiredGender || _transgender;
	}

	public bool CheckSexualOrientationMeetsRequirement(sbyte sexualOrientation)
	{
		return sexualOrientation == -1 || (sexualOrientation == 1 && _bisexual) || (sexualOrientation == 0 && !_bisexual);
	}

	public sbyte GetDisplayingGender()
	{
		return _transgender ? Gender.Flip(_gender) : _gender;
	}

	public sbyte GetInteractionGrade()
	{
		if (DomainManager.Taiwu.GetTaiwuCharId() == _id)
		{
			return MathUtils.Clamp(DomainManager.World.GetXiangshuLevel(), (sbyte)0, (sbyte)8);
		}
		return _organizationInfo.Grade;
	}

	public bool IsInTaiwuGroup()
	{
		int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
		return _id == taiwuCharId || _leaderId == taiwuCharId;
	}

	public bool IsTaiwu()
	{
		return _id == DomainManager.Taiwu.GetTaiwuCharId();
	}

	public bool IsInteractableAsIntelligentCharacter()
	{
		return CharacterMatcher.DefValue.CanInteractAsIntelligentCharacter.Match(this);
	}

	public bool IsTreasuryGuard()
	{
		return GetGroupFeature(536) >= 0;
	}

	public void TryRetireTreasuryGuard(DataContext context)
	{
		if (IsTreasuryGuard() && _organizationInfo.OrgTemplateId >= 0 && OrganizationDomain.IsSect(_organizationInfo.OrgTemplateId) && DomainManager.Organization.TryGetElement_Sects(_organizationInfo.SettlementId, out var element))
		{
			SettlementLayeredTreasuries treasuries = element.Treasuries;
			if (treasuries.TryRemoveGuard(_id, out var _))
			{
				RemoveFeatureGroup(context, 536);
				element.ForceUpdateTreasuryGuards(context);
			}
		}
	}

	public bool NeedToAvoidCombat(CombatType combatType)
	{
		int defeatMarksCountOutOfCombat = CombatDomain.GetDefeatMarksCountOutOfCombat(this);
		return defeatMarksCountOutOfCombat >= AiHelper.CombatRelatedConstants.AvoidCombatDefeatMarkCount[(int)combatType];
	}

	public bool ReachFallenInCombat(CombatType combatType)
	{
		int defeatMarksCountOutOfCombat = CombatDomain.GetDefeatMarksCountOutOfCombat(this);
		return defeatMarksCountOutOfCombat >= GlobalConfig.NeedDefeatMarkCount[(int)combatType];
	}

	public unsafe int GetPoisonResist(sbyte poisonType)
	{
		return GetPoisonResists().Items[poisonType];
	}

	public bool HasPoisonImmunity(sbyte poisonType)
	{
		return Config.Character.Instance[_templateId].PoisonImmunities[poisonType] || GetPoisonResist(poisonType) >= 1000 || DomainManager.Extra.HasPoisonImmunity(_id, poisonType);
	}

	public bool HasInnatePoisonImmunity(sbyte poisonType)
	{
		return Config.Character.Instance[_templateId].PoisonImmunities[poisonType] || DomainManager.Extra.HasPoisonImmunity(_id, poisonType);
	}

	public bool HasAcquiredPoisonImmunity(sbyte poisonType)
	{
		return GetPoisonResist(poisonType) >= 1000;
	}

	public unsafe int GetPoisonMarkCount()
	{
		int num = 0;
		CharacterItem characterCfg = Config.Character.Instance[_templateId];
		byte poisonImmunities = DomainManager.Extra.GetPoisonImmunities(_id);
		ref PoisonInts poisonResists = ref GetPoisonResists();
		for (sbyte b = 0; b < 6; b++)
		{
			if (!SharedMethods.HasPoisonImmunity(b, characterCfg, ref poisonResists, poisonImmunities))
			{
				num += PoisonsAndLevels.CalcPoisonedLevel(_poisoned.Items[b]);
			}
		}
		return num;
	}

	public sbyte GetRandomInjuredBodyPartToHeal(IRandomSource random, bool isInnerInjury, sbyte maxVal)
	{
		List<sbyte> list = ObjectPool<List<sbyte>>.Instance.Get();
		list.Clear();
		for (sbyte b = 0; b < 7; b++)
		{
			var (b2, b3) = _injuries.Get(b);
			if (isInnerInjury)
			{
				if (b3 > 0 && b3 <= maxVal)
				{
					list.Add(b);
				}
			}
			else if (b2 > 0 && b2 <= maxVal)
			{
				list.Add(b);
			}
		}
		int num = ((list.Count > 0) ? list.GetRandom(random) : (-1));
		ObjectPool<List<sbyte>>.Instance.Return(list);
		return (sbyte)num;
	}

	public unsafe sbyte GetRandomPoisonTypeToDetox(IRandomSource random, int maxLevel)
	{
		List<sbyte> list = ObjectPool<List<sbyte>>.Instance.Get();
		list.Clear();
		for (sbyte b = 0; b < 6; b++)
		{
			int poisoned = _poisoned.Items[b];
			sbyte b2 = PoisonsAndLevels.CalcPoisonedLevel(poisoned);
			if (_poisoned.Items[b] > 0 && b2 <= maxLevel)
			{
				list.Add(b);
			}
		}
		int num = ((list.Count > 0) ? list.GetRandom(random) : (-1));
		ObjectPool<List<sbyte>>.Instance.Return(list);
		return (sbyte)num;
	}

	public unsafe void TakeRandomDamage(DataContext context, int damage)
	{
		IRandomSource random = context.Random;
		Span<sbyte> span = stackalloc sbyte[14];
		SpanList<sbyte> spanList = span;
		spanList.Clear();
		for (sbyte b = 0; b < 14; b++)
		{
			if (_injuries.Items[b] < 6)
			{
				spanList.Add(b);
			}
		}
		for (int i = 0; i < damage; i++)
		{
			if (spanList.Count == 0)
			{
				break;
			}
			int index = random.Next(spanList.Count);
			sbyte b2 = spanList[index];
			ref sbyte reference = ref _injuries.Items[b2];
			reference++;
			if (_injuries.Items[b2] >= 6)
			{
				spanList.RemoveAt(index);
			}
		}
		SetInjuries(_injuries, context);
	}

	public void TakeRandomDamage(DataContext context, int damage, bool isInnerInjury)
	{
		IRandomSource random = context.Random;
		Span<sbyte> span = stackalloc sbyte[7];
		SpanList<sbyte> spanList = span;
		spanList.Clear();
		for (sbyte b = 0; b < 7; b++)
		{
			if (_injuries.Get(b, isInnerInjury) < 6)
			{
				spanList.Add(b);
			}
		}
		for (int i = 0; i < damage; i++)
		{
			if (spanList.Count == 0)
			{
				break;
			}
			int index = random.Next(spanList.Count);
			sbyte bodyPartType = spanList[index];
			_injuries.Change(bodyPartType, isInnerInjury, 1);
			if (_injuries.Get(bodyPartType, isInnerInjury) >= 6)
			{
				spanList.RemoveAt(index);
			}
		}
		SetInjuries(_injuries, context);
	}

	public int GetBirthDate()
	{
		return CharacterDomain.CalcBirthDate(_actualAge, _birthMonth);
	}

	public short GetLeftMaxHealth(bool obsoleteArg = false)
	{
		if (_srcCharId >= 0)
		{
			return GetMaxHealth();
		}
		int num = ((!IsCompletelyInfected()) ? CharacterDomain.GetLivedMonths(_currAge, _birthMonth) : (_currAge * 12));
		return (short)Math.Clamp(0, GetMaxHealth() - num, 32767);
	}

	public EHealthType GetHealthType()
	{
		return HealthTypeHelper.CalcType(_featureIds, _health, GetLeftMaxHealth());
	}

	public void MakeLove(DataContext context, Character targetChar, bool isRape)
	{
		Character character = ((_gender == 1) ? this : targetChar);
		Character character2 = ((_gender == 0) ? this : targetChar);
		bool flag = OfflineMakeLove(context.Random, character, character2, isRape);
		character.SetFeatureIds(character._featureIds, context);
		character2.SetFeatureIds(character2._featureIds, context);
		PeriAdvanceMonthFixedActionModification.MakeLoveState makeLoveState = (isRape ? PeriAdvanceMonthFixedActionModification.MakeLoveState.RapeSucceed : ((!DomainManager.Character.HasRelation(character.GetId(), character2.GetId(), 1024)) ? PeriAdvanceMonthFixedActionModification.MakeLoveState.Illegal : PeriAdvanceMonthFixedActionModification.MakeLoveState.Legal));
		Events.RaiseMakeLove(context, character, character2, (sbyte)makeLoveState);
		if (flag)
		{
			DomainManager.Character.CreatePregnantState(context, character2, character, isRape);
		}
	}

	public short CalcChangedHealth(short health, int delta)
	{
		short leftMaxHealth = GetLeftMaxHealth();
		delta = CalcHealthDelta(delta);
		return (short)((leftMaxHealth >= 0) ? ((short)Math.Clamp(health + delta, 0, leftMaxHealth)) : 0);
	}

	public void ChangeHealth(DataContext context, int delta)
	{
		_health = CalcChangedHealth(_health, delta);
		SetHealth(_health, context);
	}

	public void ChangeBaseMaxHealth(DataContext context, int delta, bool autoAdjustHealth = true)
	{
		short leftMaxHealth = GetLeftMaxHealth();
		short baseMaxHealth = (short)Math.Clamp(_baseMaxHealth + delta, 0, 32767);
		SetBaseMaxHealth(baseMaxHealth, context);
		if (autoAdjustHealth)
		{
			short leftMaxHealth2 = GetLeftMaxHealth();
			if (leftMaxHealth2 != leftMaxHealth)
			{
				ChangeHealth(context, leftMaxHealth2 - leftMaxHealth);
			}
		}
	}

	public void ChangeCurrAge(DataContext context, int delta)
	{
		sbyte ageGroup = GetAgeGroup();
		short currAge = _currAge;
		short num = (short)Math.Clamp(_currAge + delta, 0, 2730);
		SetCurrAge(num, context);
		Events.RaiseCharacterAgeChanged(context, this, currAge, num);
		sbyte ageGroup2 = GetAgeGroup();
		if (ageGroup != ageGroup2)
		{
			ReAdjustClothingByAge(context);
		}
	}

	public void ReAdjustClothingByAge(DataContext context)
	{
		sbyte ageGroup = GetAgeGroup();
		short templateId = _equipment[4].TemplateId;
		if (templateId >= 0)
		{
			ClothingItem clothingItem = Config.Clothing.Instance[templateId];
			if (clothingItem.AgeGroup == ageGroup)
			{
				return;
			}
			if (clothingItem.Detachable)
			{
				ChangeEquipment(context, 4, -1, ItemKey.Invalid);
			}
		}
		if (1 == 0)
		{
		}
		short num = ageGroup switch
		{
			0 => 64, 
			1 => 65, 
			_ => OrganizationDomain.GetRandomOrgMemberClothing(context.Random, OrganizationDomain.GetOrgMemberConfig(_organizationInfo)), 
		};
		if (1 == 0)
		{
		}
		short newClothingTemplateId = num;
		ForceReplaceClothing(context, newClothingTemplateId);
	}

	public int CalcHealthDeltaAfterSpecialEffect(DataContext context, int delta)
	{
		int num = 100 + DomainManager.SpecialEffect.GetModifyValue(_id, 54, (EDataModifyType)1, -1, -1, -1, (EDataSumType)0);
		delta = delta * num / 100;
		return delta;
	}

	public int CalcHealthDelta(int delta)
	{
		if (delta > 0)
		{
			int num = 100;
			foreach (SolarTermItem item in GetInvokedSolarTerm())
			{
				if (item.HealthBuff)
				{
					num += GetSolarTermValue(GlobalConfig.Instance.SolarTermAddHealth);
				}
			}
			delta = delta * num / 100;
		}
		else if (delta < 0)
		{
			int num2 = 100 + DomainManager.SpecialEffect.GetModifyValue(_id, 54, (EDataModifyType)1, -1, -1, -1, (EDataSumType)0);
			num2 -= DomainManager.Building.GetBuildingBlockEffect(_location, EBuildingScaleEffect.HealthDecreaseReduction);
			if (num2 < 0)
			{
				num2 = 0;
			}
			delta = delta * num2 / 100;
		}
		return delta;
	}

	public void AdjustLifespan(DataContext context)
	{
		int characterLifeSpanFactor = DomainManager.World.GetCharacterLifeSpanFactor();
		int num = 36 * characterLifeSpanFactor / 100;
		for (int num2 = CharacterDomain.GetLivedMonths(_actualAge, _birthMonth) + num - GetMaxHealth(); num2 > 0; num2 = CharacterDomain.GetLivedMonths(_actualAge, _birthMonth) + num - GetMaxHealth())
		{
			int num3 = 84 * characterLifeSpanFactor / 100;
			int num4 = _baseMaxHealth + num2 + context.Random.Next(num3);
			if (num4 > 32767)
			{
				SetBaseMaxHealth(short.MaxValue, context);
				break;
			}
			SetBaseMaxHealth((short)num4, context);
		}
		short leftMaxHealth = GetLeftMaxHealth();
		if (leftMaxHealth < 0)
		{
			throw new Exception($"Character {_id}: left max health must equal or greater than zero");
		}
		_health = Math.Clamp(_health, (short)0, leftMaxHealth);
		SetHealth(_health, context);
	}

	public void GetTitles(List<short> titleIds)
	{
		switch (GetXiangshuType())
		{
		case 1:
			titleIds.Add(0);
			break;
		case 2:
			titleIds.Add(1);
			break;
		case 3:
			titleIds.Add(2);
			break;
		}
		List<sbyte> charOwnedBookTypes = DomainManager.LegendaryBook.GetCharOwnedBookTypes(_id);
		if (charOwnedBookTypes != null)
		{
			int i = 0;
			for (int count = charOwnedBookTypes.Count; i < count; i++)
			{
				sbyte b = charOwnedBookTypes[i];
				short item = (short)(3 + b);
				titleIds.Add(item);
			}
		}
		DomainManager.Extra.FillCharacterExtraTitles(_id, titleIds);
	}

	public bool HasTitle(short templateId)
	{
		switch (templateId)
		{
		case 0:
			return GetXiangshuType() == 1;
		case 1:
			return GetXiangshuType() == 2;
		case 2:
			return GetXiangshuType() == 3;
		case 3:
		case 4:
		case 5:
		case 6:
		case 7:
		case 8:
		case 9:
		case 10:
		case 11:
		case 12:
		case 13:
		case 14:
		case 15:
		case 16:
			return DomainManager.LegendaryBook.GetCharOwnedBookTypes(_id)?.Contains((sbyte)(templateId - 3)) ?? false;
		default:
			return DomainManager.Extra.GetExtraTitleExpireDate(_id, templateId) >= 0;
		}
	}

	public unsafe void ChangeBaseMainAttributes(DataContext context, MainAttributes delta)
	{
		for (int i = 0; i < 6; i++)
		{
			int num = _baseMainAttributes.Items[i] + delta.Items[i];
			if (num < 0)
			{
				num = 0;
			}
			_baseMainAttributes.Items[i] = (short)num;
		}
		SetBaseMainAttributes(_baseMainAttributes, context);
	}

	public void ChangeBaseMainAttribute(DataContext context, sbyte mainAttributeType, short delta)
	{
		Tester.Assert(mainAttributeType >= 0 && mainAttributeType < 6);
		_baseMainAttributes[mainAttributeType] = (short)Math.Clamp(_baseMainAttributes[mainAttributeType] + delta, 0, GlobalConfig.Instance.MaxValueOfMaxMainAttributes);
		SetBaseMainAttributes(_baseMainAttributes, context);
	}

	public unsafe sbyte GetPersonality(sbyte personalityType)
	{
		Personalities personalities = GetPersonalities();
		return personalities.Items[personalityType];
	}

	public unsafe short GetMaxMainAttribute(sbyte mainAttributeType)
	{
		MainAttributes maxMainAttributes = GetMaxMainAttributes();
		return maxMainAttributes.Items[mainAttributeType];
	}

	public unsafe short GetCurrMainAttribute(sbyte mainAttributeType)
	{
		MainAttributes currMainAttributes = GetCurrMainAttributes();
		return currMainAttributes.Items[mainAttributeType];
	}

	public void ChangeCurrMainAttribute(DataContext context, sbyte mainAttributeType, int delta)
	{
		MainAttributes maxMainAttributes = GetMaxMainAttributes();
		_currMainAttributes[mainAttributeType] = (short)Math.Clamp(_currMainAttributes[mainAttributeType] + delta, 0, maxMainAttributes[mainAttributeType]);
		SetCurrMainAttributes(_currMainAttributes, context);
	}

	public unsafe void ChangeCurrMainAttributes(DataContext context, MainAttributes delta)
	{
		MainAttributes maxMainAttributes = GetMaxMainAttributes();
		for (sbyte b = 0; b < 6; b++)
		{
			_currMainAttributes.Items[b] = (short)Math.Clamp(_currMainAttributes.Items[b] + delta.Items[b], 0, maxMainAttributes.Items[b]);
		}
		SetCurrMainAttributes(_currMainAttributes, context);
	}

	public unsafe void ChangeBaseLifeSkillQualifications(DataContext context, ref LifeSkillShorts delta)
	{
		for (int i = 0; i < 16; i++)
		{
			int num = _baseLifeSkillQualifications.Items[i] + delta.Items[i];
			if (num < 0)
			{
				num = 0;
			}
			_baseLifeSkillQualifications.Items[i] = (short)num;
		}
		SetBaseLifeSkillQualifications(ref _baseLifeSkillQualifications, context);
	}

	public unsafe void ChangeBaseLifeSkillQualification(DataContext context, sbyte skillType, int delta)
	{
		Tester.Assert(skillType >= 0 && skillType < 16);
		_baseLifeSkillQualifications.Items[skillType] = (short)Math.Clamp(_baseLifeSkillQualifications.Items[skillType] + delta, 0, 32767);
		SetBaseLifeSkillQualifications(ref _baseLifeSkillQualifications, context);
	}

	public unsafe void ChangeBaseCombatSkillQualifications(DataContext context, ref CombatSkillShorts delta)
	{
		for (int i = 0; i < 14; i++)
		{
			int num = _baseCombatSkillQualifications.Items[i] + delta.Items[i];
			if (num < 0)
			{
				num = 0;
			}
			_baseCombatSkillQualifications.Items[i] = (short)num;
		}
		SetBaseCombatSkillQualifications(ref _baseCombatSkillQualifications, context);
	}

	public unsafe void ChangeBaseCombatSkillQualification(DataContext context, sbyte skillType, int delta)
	{
		Tester.Assert(skillType >= 0 && skillType < 14);
		_baseCombatSkillQualifications.Items[skillType] = (short)Math.Clamp(_baseCombatSkillQualifications.Items[skillType] + delta, 0, 32767);
		SetBaseCombatSkillQualifications(ref _baseCombatSkillQualifications, context);
	}

	public sbyte GetLifeSkillQualificationAgeAdjust()
	{
		short clampedAgeOfAgeEffect = GetClampedAgeOfAgeEffect(_actualAge);
		AgeEffectItem ageEffectItem = AgeEffect.Instance[clampedAgeOfAgeEffect];
		sbyte lifeSkillQualificationGrowthType = _lifeSkillQualificationGrowthType;
		if (1 == 0)
		{
		}
		sbyte result = lifeSkillQualificationGrowthType switch
		{
			0 => ageEffectItem.SkillQualificationAverage, 
			1 => ageEffectItem.SkillQualificationPrecocious, 
			2 => ageEffectItem.SkillQualificationLateBlooming, 
			_ => throw new Exception($"Unsupported GrowthType {_lifeSkillQualificationGrowthType}"), 
		};
		if (1 == 0)
		{
		}
		return result;
	}

	public sbyte GetCombatSkillQualificationAgeAdjust()
	{
		short clampedAgeOfAgeEffect = GetClampedAgeOfAgeEffect(_actualAge);
		AgeEffectItem ageEffectItem = AgeEffect.Instance[clampedAgeOfAgeEffect];
		sbyte combatSkillQualificationGrowthType = _combatSkillQualificationGrowthType;
		if (1 == 0)
		{
		}
		sbyte result = combatSkillQualificationGrowthType switch
		{
			0 => ageEffectItem.SkillQualificationAverage, 
			1 => ageEffectItem.SkillQualificationPrecocious, 
			2 => ageEffectItem.SkillQualificationLateBlooming, 
			_ => throw new Exception($"Unsupported GrowthType {_lifeSkillQualificationGrowthType}"), 
		};
		if (1 == 0)
		{
		}
		return result;
	}

	public sbyte GetBodyType()
	{
		return _avatar.GetBodyType();
	}

	public void SetBodyType(DataContext context, sbyte bodyType)
	{
		_avatar.ChangeBodyType(bodyType);
		SetAvatar(_avatar, context);
	}

	public sbyte GetHappinessType()
	{
		return HappinessType.GetHappinessType(_happiness);
	}

	public void ChangeHappiness(DataContext context, int delta)
	{
		_happiness = (sbyte)Math.Clamp(_happiness + delta, -119, 119);
		SetHappiness(_happiness, context);
	}

	public sbyte GetBehaviorType()
	{
		return BehaviorType.GetBehaviorType(GetMorality());
	}

	public short GetFixedMorality()
	{
		sbyte legendaryBookOwnerState = GetLegendaryBookOwnerState();
		if (legendaryBookOwnerState == 1 || _featureIds.Contains(217))
		{
			return -250;
		}
		if (legendaryBookOwnerState >= 2 || _featureIds.Contains(218))
		{
			return -438;
		}
		Location forceRebelLocation = DomainManager.Character.GetForceRebelLocation();
		if (forceRebelLocation.IsValid() && forceRebelLocation.AreaId == _location.AreaId)
		{
			byte areaSize = DomainManager.Map.GetAreaSize(_location.AreaId);
			ByteCoordinate byteCoordinate = ByteCoordinate.IndexToCoordinate(forceRebelLocation.BlockId, areaSize);
			ByteCoordinate byteCoordinate2 = ByteCoordinate.IndexToCoordinate(_location.BlockId, areaSize);
			if (byteCoordinate.GetManhattanDistance(byteCoordinate2) <= 3)
			{
				return -250;
			}
		}
		Location forceKindLocation = DomainManager.Character.GetForceKindLocation();
		if (forceKindLocation.IsValid() && forceKindLocation.AreaId == _location.AreaId)
		{
			byte areaSize2 = DomainManager.Map.GetAreaSize(_location.AreaId);
			ByteCoordinate byteCoordinate3 = ByteCoordinate.IndexToCoordinate(forceKindLocation.BlockId, areaSize2);
			ByteCoordinate byteCoordinate4 = ByteCoordinate.IndexToCoordinate(_location.BlockId, areaSize2);
			if (byteCoordinate3.GetManhattanDistance(byteCoordinate4) <= 3)
			{
				return 250;
			}
		}
		for (int i = 0; i < _featureIds.Count; i++)
		{
			switch (_featureIds[i])
			{
			case 484:
				return 250;
			case 485:
				return -250;
			}
		}
		return short.MaxValue;
	}

	public void ChangeBaseMorality(DataContext context, int delta)
	{
		_baseMorality = (short)Math.Clamp(_baseMorality + delta, -500, 500);
		SetBaseMorality(_baseMorality, context);
	}

	public void ChangeHobby(DataContext context)
	{
		OfflineChangeHobby(context.Random);
		CommitChangingHobby(context);
	}

	public void ChangeLovingItem(DataContext context)
	{
		short item = GenerateRandomHobby(context.Random).lovingItemSubType;
		sbyte favorabilityType = FavorabilityType.GetFavorabilityType(DomainManager.Character.GetFavorability(_id, DomainManager.Taiwu.GetTaiwuCharId()));
		bool flag = false;
		if (favorabilityType < 3)
		{
			SetLovingItemRevealed(lovingItemRevealed: false, context);
		}
		else if (DomainManager.Extra.IsCharacterLovingItemRevealed(_id))
		{
			flag = true;
		}
		_lovingItemSubType = (short)((item == _hatingItemSubType) ? (-1) : item);
		SetLovingItemSubType(_lovingItemSubType, context);
		if (flag)
		{
			DomainManager.Extra.SetCharacterRevealedHobbies(context, _id, isLovingItem: true);
		}
	}

	public void ChangeHatingItem(DataContext context)
	{
		short item = GenerateRandomHobby(context.Random).hatingItemSubType;
		sbyte favorabilityType = FavorabilityType.GetFavorabilityType(DomainManager.Character.GetFavorability(_id, DomainManager.Taiwu.GetTaiwuCharId()));
		bool flag = false;
		if (favorabilityType < 3)
		{
			SetHatingItemRevealed(hatingItemRevealed: false, context);
		}
		else if (DomainManager.Extra.IsCharacterHatingItemRevealed(_id))
		{
			flag = true;
		}
		_hatingItemSubType = (short)((item == _lovingItemSubType) ? (-1) : item);
		SetHatingItemSubType(_hatingItemSubType, context);
		if (flag)
		{
			DomainManager.Extra.SetCharacterRevealedHobbies(context, _id, isLovingItem: false);
		}
	}

	public void UpdateHobbyExpirationDate(DataContext context)
	{
		OfflineUpdateHobbyExpirationDate();
		SetHobbyExpirationDate(_hobbyExpirationDate, context);
	}

	public int GetCharacterWorth()
	{
		throw new NotImplementedException();
	}

	public bool CanBeXiangshuInfected()
	{
		sbyte b = (sbyte)((_id != DomainManager.Taiwu.GetTaiwuCharId()) ? _organizationInfo.Grade : 0);
		sbyte maxGradeOfXiangshuInfection = DomainManager.World.GetMaxGradeOfXiangshuInfection();
		return b <= maxGradeOfXiangshuInfection;
	}

	public bool CanHaveProfession()
	{
		return GetAgeGroup() == 2;
	}

	public void ChangeXiangshuInfection(DataContext context, int delta)
	{
		_xiangshuInfection = (byte)Math.Clamp(_xiangshuInfection + delta, 0, 200);
		SetXiangshuInfection(_xiangshuInfection, context);
	}

	public void UpdateXiangshuInfectionState(DataContext context)
	{
		short infectionFeatureIdThatShouldBe = XiangshuInfectionTypeHelper.GetInfectionFeatureIdThatShouldBe(_xiangshuInfection);
		if (!_featureIds.Contains(infectionFeatureIdThatShouldBe) && (infectionFeatureIdThatShouldBe != 218 || (_id != DomainManager.Taiwu.GetTaiwuCharId() && !IsActiveExternalRelationState(4) && !_featureIds.Contains(415))))
		{
			AddFeature(context, infectionFeatureIdThatShouldBe, removeMutexFeature: true);
			if (_creatingType == 1)
			{
				Events.RaiseXiangshuInfectionFeatureChanged(context, this, infectionFeatureIdThatShouldBe);
			}
		}
	}

	public void ChangeExp(DataContext context, int delta)
	{
		_exp = Math.Clamp(_exp + delta, 0, 999999999);
		SetExp(_exp, context);
	}

	public unsafe int GetResource(sbyte resourceType)
	{
		return _resources.Items[resourceType];
	}

	public unsafe void SpecifyResource(DataContext context, sbyte resourceType, int value)
	{
		if (value < 0)
		{
			throw new Exception($"{_id}'s resource amount cannot be negative: {resourceType}, {value}");
		}
		_resources.Items[resourceType] = value;
		SetResources(ref _resources, context);
	}

	public unsafe void SpecifyResources(DataContext context, ref ResourceInts values)
	{
		for (int i = 0; i < 8; i++)
		{
			int num = values.Items[i];
			if (num < 0)
			{
				throw new Exception($"{_id}'s resource amount cannot be negative: {i}, {num}");
			}
			_resources.Items[i] = num;
		}
		SetResources(ref _resources, context);
	}

	public unsafe void ChangeResource(DataContext context, sbyte resourceType, int delta)
	{
		int num = _resources.Items[resourceType] + delta;
		if (num < 0)
		{
			AdaptableLog.TagWarning("Character", $"{_id}'s resource amount cannot be negative: {resourceType}, {num} (delta {delta})\n{Environment.StackTrace}");
			num = 0;
		}
		else if (num > 999999999)
		{
			num = 999999999;
		}
		_resources.Items[resourceType] = num;
		SetResources(ref _resources, context);
	}

	public unsafe void ChangeResources(DataContext context, ref ResourceInts delta)
	{
		for (int i = 0; i < 8; i++)
		{
			int num = _resources.Items[i] + delta.Items[i];
			if (num < 0)
			{
				AdaptableLog.TagWarning("Character", $"{_id}'s resource amount cannot be negative: {i}, {num} (delta {delta})\n{Environment.StackTrace}");
				num = 0;
			}
			else if (num > 999999999)
			{
				num = 999999999;
			}
			_resources.Items[i] = num;
		}
		SetResources(ref _resources, context);
	}

	public bool CheckResources(DataContext context, ref ResourceInts resource)
	{
		return _resources.CheckIsMeet(ref resource);
	}

	public bool CheckResources(DataContext context, sbyte type, int value)
	{
		return _resources.CheckIsMeet(type, value);
	}

	public unsafe void ChangeResourceWithoutChecking(DataContext context, sbyte resourceType, int delta)
	{
		int num = _resources.Items[resourceType] + delta;
		if (num < 0)
		{
			num = 0;
		}
		_resources.Items[resourceType] = num;
		SetResources(ref _resources, context);
	}

	public unsafe void ChangeResourcesWithoutChecking(DataContext context, ref ResourceInts delta)
	{
		for (int i = 0; i < 8; i++)
		{
			int num = _resources.Items[i] + delta.Items[i];
			if (num < 0)
			{
				num = 0;
			}
			_resources.Items[i] = num;
		}
		SetResources(ref _resources, context);
	}

	public Location GetValidLocation()
	{
		if (_location.IsValid())
		{
			return _location;
		}
		if (_kidnapperId >= 0)
		{
			if (DomainManager.Character.TryGetElement_Objects(_kidnapperId, out var element))
			{
				Location location = element.GetLocation();
				if (location.IsValid())
				{
					return location;
				}
				if (element._kidnapperId >= 0)
				{
					throw new Exception($"Nested kidnapping detected: {this} => {_kidnapperId} => {element._kidnapperId}.");
				}
				return element.GetValidLocation();
			}
			Grave element_Graves = DomainManager.Character.GetElement_Graves(_kidnapperId);
			return element_Graves.GetLocation();
		}
		if (IsActiveExternalRelationState(32))
		{
			sbyte prisonerSect = DomainManager.Organization.GetPrisonerSect(_id);
			Settlement settlementByOrgTemplateId = DomainManager.Organization.GetSettlementByOrgTemplateId(prisonerSect);
			return settlementByOrgTemplateId.GetLocation();
		}
		if (DomainManager.Character.TryGetElement_CrossAreaMoveInfos((_leaderId >= 0) ? _leaderId : _id, out var value))
		{
			Location result = DomainManager.Map.CrossAreaTravelInfoToLocation(value);
			if (result.IsValid())
			{
				return result;
			}
			throw new Exception($"Character {this} is traveling outside of the planned route.");
		}
		if (DomainManager.Taiwu.GetTaiwuCharId() == _id || DomainManager.Taiwu.IsInGroup(_id))
		{
			return DomainManager.Map.GetTravelCurrLocation();
		}
		if (DomainManager.Extra.GetKidnappedTravelData().HunterCharId == _id)
		{
			return DomainManager.Map.GetTravelCurrLocation();
		}
		throw new Exception($"Character {this} is in invalid location for unknown reasons.");
	}

	public bool IsMoralityLocked()
	{
		sbyte legendaryBookOwnerState = GetLegendaryBookOwnerState();
		if (legendaryBookOwnerState == 1 || _featureIds.Contains(217))
		{
			return true;
		}
		if (legendaryBookOwnerState >= 2 || _featureIds.Contains(218))
		{
			return true;
		}
		Location forceRebelLocation = DomainManager.Character.GetForceRebelLocation();
		if (IsNearbyLocation(forceRebelLocation, 3))
		{
			return true;
		}
		Location forceKindLocation = DomainManager.Character.GetForceKindLocation();
		if (IsNearbyLocation(forceKindLocation, 3))
		{
			return true;
		}
		return false;
	}

	public bool IsNearbyLocation(Location location, int steps)
	{
		if (!location.IsValid())
		{
			return false;
		}
		if (location.AreaId != _location.AreaId)
		{
			return false;
		}
		byte areaSize = DomainManager.Map.GetAreaSize(_location.AreaId);
		ByteCoordinate byteCoordinate = ByteCoordinate.IndexToCoordinate(location.BlockId, areaSize);
		ByteCoordinate byteCoordinate2 = ByteCoordinate.IndexToCoordinate(_location.BlockId, areaSize);
		return byteCoordinate.GetManhattanDistance(byteCoordinate2) <= steps;
	}

	public bool IsForbiddenToDrinkingWines()
	{
		return Config.Organization.Instance[_organizationInfo.OrgTemplateId].NoDrinking || IsMonkType(2);
	}

	public bool IsForbiddenToEatMeat()
	{
		return Config.Organization.Instance[_organizationInfo.OrgTemplateId].NoMeatEating || _monkType != 0;
	}

	public bool IsMonkType(byte monkType)
	{
		return (_monkType & monkType) != 0;
	}

	public void BecomeMonkType(DataContext context, byte monkType)
	{
		Tester.Assert((monkType & 0x80) == 0);
		_monkType |= monkType;
		SetMonkType(_monkType, context);
	}

	public void RemoveMonkType(DataContext context, byte monkType)
	{
		Tester.Assert((monkType & 0x80) == 0);
		Tester.Assert((_monkType & 0x80) == 0);
		_monkType &= (byte)(~monkType);
		SetMonkType(_monkType, context);
	}

	public bool IsActiveExternalRelationState(byte type)
	{
		return (_externalRelationState & type) != 0;
	}

	public bool IsActiveExternalRelationState(ulong extraType)
	{
		return (_externalRelationState & 0x80) != 0;
	}

	public void ActiveExternalRelationState(DataContext context, byte type)
	{
		_externalRelationState |= type;
		SetExternalRelationState(_externalRelationState, context);
	}

	public short GetKidnappingEnemyNestAdventure()
	{
		if (!IsActiveExternalRelationState(4))
		{
			return -1;
		}
		AdventureSiteData adventureSite = DomainManager.Adventure.GetAdventureSite(_location.AreaId, _location.BlockId);
		if (adventureSite == null)
		{
			return -1;
		}
		if (!adventureSite.MonthlyActionKey.IsValid())
		{
			return -1;
		}
		if (!(DomainManager.TaiwuEvent.GetMonthlyAction(adventureSite.MonthlyActionKey) is EnemyNestMonthlyAction enemyNestMonthlyAction))
		{
			return -1;
		}
		ConfigMonthlyAction configAction = enemyNestMonthlyAction.GetConfigAction(_location.AreaId, _location.BlockId);
		if (configAction == null || configAction.MajorCharacterSets == null)
		{
			return -1;
		}
		foreach (CharacterSet majorCharacterSet in configAction.MajorCharacterSets)
		{
			if (majorCharacterSet.Contains(_id))
			{
				return configAction.ConfigData.AdventureId;
			}
		}
		return -1;
	}

	public void DeactivateExternalRelationState(DataContext context, byte type)
	{
		_externalRelationState &= (byte)(~type);
		SetExternalRelationState(_externalRelationState, context);
	}

	public bool IsInvincibleInNpcCombat()
	{
		short groupFeature = GetGroupFeature(484);
		return groupFeature >= 0;
	}

	public sbyte GetBelongMapState()
	{
		if (_organizationInfo.SettlementId < 0)
		{
			return -1;
		}
		Settlement settlement = DomainManager.Organization.GetSettlement(_organizationInfo.SettlementId);
		if (settlement == null)
		{
			return -1;
		}
		Location location = settlement.GetLocation();
		if (location.AreaId < 0)
		{
			return -1;
		}
		return DomainManager.Map.GetStateIdByAreaId(location.AreaId);
	}

	public override string ToString()
	{
		(string, string) realName = CharacterDomain.GetRealName(this);
		return $"{_organizationInfo}-{realName.Item1}{realName.Item2}({_id})";
	}

	public bool IsOnCityTown()
	{
		Location location = GetLocation();
		if (location.AreaId >= 0 && location.BlockId >= 0)
		{
			MapBlockData block = DomainManager.Map.GetBlock(location);
			return block.IsCityTown();
		}
		return false;
	}

	public short GetIdealClothingTemplateId()
	{
		if (_organizationInfo.OrgTemplateId == 16)
		{
			VillagerRoleBase villagerRole = DomainManager.Extra.GetVillagerRole(_id);
			if (villagerRole != null)
			{
				return villagerRole.RoleConfig.Clothing;
			}
		}
		return OrganizationDomain.GetOrgMemberConfig(_organizationInfo).Clothing.TemplateId;
	}

	public static short GetSectRandomEnemyTemplateIdByGrade(sbyte orgTemplateId, sbyte grade)
	{
		OrganizationItem organizationItem = Config.Organization.Instance[orgTemplateId];
		return organizationItem.RandomEnemyTemplateIds[grade];
	}

	public int GetExtraNameTextTemplateId()
	{
		short templateId = _templateId;
		if (templateId >= 673 && templateId <= 677 && DomainManager.Extra.TryGetHeavenlyTreeById(_id, out var tree))
		{
			return GameData.Domains.Extra.SharedMethods.GetTreeExtraNameTextTemplateId(tree.TemplateId);
		}
		return -1;
	}

	public int GetMaxProfessionSeniority(int professionId)
	{
		if (IsTaiwu())
		{
			return 3000000;
		}
		short professionBestAttainment = GetProfessionBestAttainment(professionId);
		int attainmentCanUnlockProfessionSkill = GetAttainmentCanUnlockProfessionSkill(professionBestAttainment);
		int skillId = GameData.Domains.Taiwu.Profession.SharedMethods.GetSkillId(professionId, attainmentCanUnlockProfessionSkill);
		return GameData.Domains.Taiwu.Profession.SharedMethods.GetSkillUnlockSeniority(skillId);
	}

	public short GetProfessionBestAttainment(int professionId)
	{
		short num = 0;
		ProfessionItem professionItem = Profession.Instance[professionId];
		if (professionItem.BonusLifeSkills != null)
		{
			foreach (sbyte bonusLifeSkill in professionItem.BonusLifeSkills)
			{
				short lifeSkillAttainment = GetLifeSkillAttainment(bonusLifeSkill);
				if (lifeSkillAttainment > num)
				{
					num = lifeSkillAttainment;
				}
			}
		}
		if (professionItem.BonusCombatSkills != null)
		{
			foreach (sbyte bonusCombatSkill in professionItem.BonusCombatSkills)
			{
				short combatSkillAttainment = GetCombatSkillAttainment(bonusCombatSkill);
				if (combatSkillAttainment > num)
				{
					num = combatSkillAttainment;
				}
			}
		}
		return num;
	}

	private static int GetAttainmentCanUnlockProfessionSkill(short attainment)
	{
		int[] maxSeniorityAttainmentThresholds = ProfessionRelatedConstants.MaxSeniorityAttainmentThresholds;
		for (int num = maxSeniorityAttainmentThresholds.Length - 1; num >= 0; num--)
		{
			int num2 = maxSeniorityAttainmentThresholds[num];
			if (attainment >= num2)
			{
				return num;
			}
		}
		return 0;
	}

	private CValueModify CalcPropertyModify(ECharacterPropertyReferencedType propertyType, EDataSumType valueSumType = (EDataSumType)0)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0108: Unknown result type (might be due to invalid IL or missing references)
		//IL_010d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0132: Unknown result type (might be due to invalid IL or missing references)
		//IL_0137: Unknown result type (might be due to invalid IL or missing references)
		//IL_0138: Unknown result type (might be due to invalid IL or missing references)
		//IL_0139: Unknown result type (might be due to invalid IL or missing references)
		//IL_0123: Unknown result type (might be due to invalid IL or missing references)
		//IL_0128: Unknown result type (might be due to invalid IL or missing references)
		//IL_013d: Unknown result type (might be due to invalid IL or missing references)
		CValueModify val = CValueModify.Zero;
		val = ((CValueModify)(ref val)).ChangeA(CalcMainAttributeAddValue(propertyType));
		val += GetPropertyBonusOfFeatures(propertyType);
		val = ((CValueModify)(ref val)).ChangeA(CalcTreasureGuardAddValue(propertyType));
		val = ((CValueModify)(ref val)).ChangeA(DataSumTypeHelper.Sum(valueSumType, CalcNeiliAllocationBonus(propertyType)));
		if (IsTaiwu())
		{
			val = ((CValueModify)(ref val)).ChangeA(CalcProfessionAddValue(propertyType));
		}
		if (IsTaiwu())
		{
			val += (CValueModify)DomainManager.Extra.GetTaiwuPropertyPermanentBonus(propertyType);
		}
		val = ((CValueModify)(ref val)).ChangeA(CalcPreexistenceAddValue(propertyType));
		val = ((CValueModify)(ref val)).ChangeA(CalcAgeEffectAddValue(propertyType));
		val = ((CValueModify)(ref val)).ChangeA(GetPropertyBonusOfEquipments(propertyType));
		val += _eatingItems.GetCharacterPropertyBonus(propertyType, IsTaiwu());
		if (IsTaiwu())
		{
			val += DomainManager.Extra.CalcLegendaryBookAddPropertyValue(propertyType);
		}
		val = ((CValueModify)(ref val)).ChangeA(CalcChickenAddValue(propertyType));
		if (IsTaiwu())
		{
			val = ((CValueModify)(ref val)).ChangeA(CalcSamsaraPlatformAddValue(propertyType));
		}
		if (IsTaiwu())
		{
			val = ((CValueModify)(ref val)).ChangeA(CalcSectXuannvUnlockedMusicAddValue(propertyType));
		}
		val = ((CValueModify)(ref val)).ChangeA(GetPropertyBonusOfCombatSkillEquippingAndBreakout(propertyType));
		return val;
	}

	private int CalcMainAttributeAddValue(ECharacterPropertyReferencedType propertyType)
	{
		sbyte mainAttributeType = propertyType.GetMainAttributeType();
		if (mainAttributeType == 6)
		{
			return 0;
		}
		ushort mainAttributeConsummateFieldId = propertyType.GetMainAttributeConsummateFieldId();
		int mainAttributeConsummateDivisor = propertyType.GetMainAttributeConsummateDivisor();
		int hitOrInnerType;
		sbyte avoidType;
		bool penetrateIsInner;
		if (propertyType.TryParseHitType(out var hitType))
		{
			hitOrInnerType = hitType;
		}
		else if (propertyType.TryParseAvoidType(out avoidType))
		{
			hitOrInnerType = avoidType;
		}
		else if (propertyType.TryParsePenetrateIsInner(out penetrateIsInner))
		{
			hitOrInnerType = (penetrateIsInner ? 1 : 0);
		}
		else
		{
			if (!propertyType.TryParsePenetrateResistIsInner(out var penetrateResistIsInner))
			{
				return 0;
			}
			hitOrInnerType = (penetrateResistIsInner ? 1 : 0);
		}
		return CalcMainAttributeRelatedConsummateLevel(mainAttributeConsummateFieldId, mainAttributeType, hitOrInnerType, mainAttributeConsummateDivisor);
	}

	private int CalcTreasureGuardAddValue(ECharacterPropertyReferencedType propertyType)
	{
		EventArgBox globalEventArgumentBox = DomainManager.TaiwuEvent.GetGlobalEventArgumentBox();
		bool arg = false;
		globalEventArgumentBox.Get("IsGuardCombat", ref arg);
		if (!arg || !IsTreasuryGuard() || !HitAvoidAttackDefendPropertyTypes.Contains(propertyType))
		{
			return 0;
		}
		int num = 0;
		foreach (CombatCharacter teammateCharacter in DomainManager.Combat.GetTeammateCharacters(_id))
		{
			int value = 0;
			sbyte avoidType;
			bool penetrateIsInner;
			bool penetrateResistIsInner;
			if (propertyType.TryParseHitType(out var hitType))
			{
				value = teammateCharacter.GetCharacter().GetHitValues()[hitType];
			}
			else if (propertyType.TryParseAvoidType(out avoidType))
			{
				value = teammateCharacter.GetCharacter().GetAvoidValues()[avoidType];
			}
			else if (propertyType.TryParsePenetrateIsInner(out penetrateIsInner))
			{
				value = teammateCharacter.GetCharacter().GetPenetrations().Get(penetrateIsInner);
			}
			else if (propertyType.TryParsePenetrateResistIsInner(out penetrateResistIsInner))
			{
				value = teammateCharacter.GetCharacter().GetPenetrationResists().Get(penetrateResistIsInner);
			}
			num += CalcTreasuryGuardBonus(value);
		}
		return num;
	}

	private int CalcProfessionAddValue(ECharacterPropertyReferencedType propertyType)
	{
		if (!propertyType.TryParseMainAttributeType(out var mainAttributeType))
		{
			return 0;
		}
		int professionId = ProfessionRelatedConstants.MainAttributeRecoverProfessionIds[mainAttributeType];
		if (!DomainManager.Extra.IsProfessionalSkillUnlocked(professionId, 0))
		{
			return 0;
		}
		ProfessionData professionData = DomainManager.Extra.GetProfessionData(professionId);
		return professionData.GetSeniorityMainAttributeAdditional();
	}

	private unsafe int CalcPreexistenceAddValue(ECharacterPropertyReferencedType propertyType)
	{
		if (_preexistenceCharIds.Count <= 0)
		{
			return 0;
		}
		int num = 0;
		int i = 0;
		for (int count = _preexistenceCharIds.Count; i < count; i++)
		{
			int charId = _preexistenceCharIds.CharIds[i];
			DeadCharacter deadCharacter = DomainManager.Character.TryGetDeadCharacter(charId);
			if (deadCharacter != null)
			{
				sbyte lifeSkillType;
				sbyte combatSkillType;
				if (propertyType.TryParseMainAttributeType(out var mainAttributeType))
				{
					num += deadCharacter.BaseMainAttributes[mainAttributeType];
				}
				else if (propertyType.TryParseLifeSkillQualificationType(out lifeSkillType))
				{
					num += deadCharacter.BaseLifeSkillQualifications[lifeSkillType];
				}
				else if (propertyType.TryParseCombatSkillQualificationType(out combatSkillType))
				{
					num += deadCharacter.BaseCombatSkillQualifications[combatSkillType];
				}
			}
		}
		return num / 10;
	}

	private int CalcAgeEffectAddValue(ECharacterPropertyReferencedType propertyType)
	{
		bool flag;
		if (propertyType.TryParseLifeSkillQualificationType(out var lifeSkillType))
		{
			flag = true;
		}
		else
		{
			if (!propertyType.TryParseCombatSkillQualificationType(out lifeSkillType))
			{
				return 0;
			}
			flag = false;
		}
		short clampedAgeOfAgeEffect = GetClampedAgeOfAgeEffect(_actualAge);
		AgeEffectItem ageEffectItem = AgeEffect.Instance[clampedAgeOfAgeEffect];
		sbyte b = (flag ? _lifeSkillQualificationGrowthType : _combatSkillQualificationGrowthType);
		if (1 == 0)
		{
		}
		sbyte b2 = b switch
		{
			0 => ageEffectItem.SkillQualificationAverage, 
			1 => ageEffectItem.SkillQualificationPrecocious, 
			2 => ageEffectItem.SkillQualificationLateBlooming, 
			_ => throw new Exception($"Unsupported GrowthType {b}"), 
		};
		if (1 == 0)
		{
		}
		sbyte b3 = b2;
		sbyte b4 = ((!flag) ? ((sbyte)1) : ((sbyte)0));
		int i = 0;
		for (int count = _skillQualificationBonuses.Count; i < count; i++)
		{
			SkillQualificationBonus skillQualificationBonus = _skillQualificationBonuses[i];
			var (b5, b6) = skillQualificationBonus.GetSkillGroupAndType();
			if (b5 == b4 && b6 == lifeSkillType)
			{
				b3 += skillQualificationBonus.Bonus;
			}
		}
		return b3;
	}

	private int CalcChickenAddValue(ECharacterPropertyReferencedType propertyType)
	{
		if (!propertyType.TryParsePersonalityType(out var personalityType))
		{
			return 0;
		}
		int num = 0;
		short templateId = OrganizationDomain.GetOrgMemberConfig(_organizationInfo).TemplateId;
		foreach (GameData.Domains.Building.Chicken fulongChicken in DomainManager.Building.GetFulongChickens(templateId))
		{
			ChickenItem chickenItem = Config.Chicken.Instance[fulongChicken.TemplateId];
			if (chickenItem.PersonalityType == personalityType)
			{
				num += chickenItem.PersonalityValue;
			}
		}
		return num;
	}

	private int CalcSamsaraPlatformAddValue(ECharacterPropertyReferencedType propertyType)
	{
		if (propertyType.TryParseMainAttributeType(out var mainAttributeType))
		{
			return DomainManager.Building.GetSamsaraPlatformAddMainAttributes()[mainAttributeType];
		}
		if (propertyType.TryParseLifeSkillQualificationType(out var lifeSkillType))
		{
			return DomainManager.Building.GetSamsaraPlatformAddLifeSkillQualifications()[lifeSkillType];
		}
		if (propertyType.TryParseCombatSkillQualificationType(out var combatSkillType))
		{
			return DomainManager.Building.GetSamsaraPlatformAddCombatSkillQualifications()[combatSkillType];
		}
		return 0;
	}

	private int CalcSectXuannvUnlockedMusicAddValue(ECharacterPropertyReferencedType propertyType)
	{
		if ((propertyType != ECharacterPropertyReferencedType.HitRateMind && propertyType != ECharacterPropertyReferencedType.AvoidRateMind) || 1 == 0)
		{
			return 0;
		}
		int num = 0;
		bool flag = propertyType == ECharacterPropertyReferencedType.HitRateMind;
		foreach (short sectXuannvUnlockedMusic in DomainManager.Extra.GetSectXuannvUnlockedMusicList())
		{
			MusicItem musicItem = Music.Instance[sectXuannvUnlockedMusic];
			num += (flag ? musicItem.HitRateMind : musicItem.AvoidRateMind);
		}
		return num;
	}

	private int CalcAvatarAttraction()
	{
		if (!IsCreatedWithFixedTemplate())
		{
			return _avatar.GetCharm(GetPhysiologicalAge(), GetClothingDisplayIdForCharm());
		}
		ProfessionData professionData = DomainManager.Extra.GetProfessionData(1);
		HunterSkillsData skillsData = professionData.GetSkillsData<HunterSkillsData>();
		if (skillsData.AnimalCharIdToItemKey != null && skillsData.AnimalCharIdToItemKey.TryGetValue(_id, out var value) && skillsData.AnimalCharIdToAttraction != null && skillsData.AnimalCharIdToAttraction.TryGetValue(value, out var value2))
		{
			return value2;
		}
		return Template.BaseAttraction;
	}

	private int GetReversedFeatureBonus(ECharacterPropertyReferencedType propertyType, int originBonus, short featureId)
	{
		if (!BonusPropertyTypes.Contains(propertyType))
		{
			return originBonus;
		}
		CharacterFeatureItem config = CharacterFeature.Instance[featureId];
		int modifyValue = DomainManager.SpecialEffect.GetModifyValue(_id, 293, (EDataModifyType)0, -1, -1, -1, (EDataSumType)0);
		if ((originBonus < 0 && modifyValue > 0 && config.IsBad()) || (originBonus > 0 && modifyValue < 0 && config.IsGood()))
		{
			return -originBonus;
		}
		return originBonus;
	}

	private CValueModify GetPropertyBonusOfFeatures(ECharacterPropertyReferencedType propertyType)
	{
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0100: Unknown result type (might be due to invalid IL or missing references)
		//IL_0102: Unknown result type (might be due to invalid IL or missing references)
		//IL_0107: Unknown result type (might be due to invalid IL or missing references)
		//IL_010b: Unknown result type (might be due to invalid IL or missing references)
		int num = 0;
		int num2 = 0;
		bool featureStandardIsAdd = CharacterPropertyReferenced.Instance[(int)propertyType].FeatureStandardIsAdd;
		foreach (short featureId in _featureIds)
		{
			if (!IgnoreFeature(featureId) || propertyType < ECharacterPropertyReferencedType.PersonalityCalm || propertyType > ECharacterPropertyReferencedType.PersonalityPerceptive)
			{
				CharacterFeatureItem characterFeatureItem = CharacterFeature.Instance[featureId];
				int characterPropertyBonusInt = characterFeatureItem.GetCharacterPropertyBonusInt(propertyType);
				characterPropertyBonusInt = GetReversedFeatureBonus(propertyType, characterPropertyBonusInt, featureId);
				if (featureStandardIsAdd)
				{
					num += characterPropertyBonusInt;
				}
				else
				{
					num2 += characterPropertyBonusInt;
				}
				int num3 = num2;
				if (1 == 0)
				{
				}
				int num4 = propertyType switch
				{
					ECharacterPropertyReferencedType.Attraction => characterFeatureItem.AttractionPercentBonus, 
					ECharacterPropertyReferencedType.MaxHealth => characterFeatureItem.MaxHealthPercentBonus, 
					_ => 0, 
				};
				if (1 == 0)
				{
				}
				num2 = num3 + num4;
			}
		}
		return new CValueModify(num, CValuePercentBonus.op_Implicit(num2), default(CValuePercentBonus), default(CValuePercentBonus));
	}

	private int GetPropertyBonusOfEquipments(ECharacterPropertyReferencedType propertyType)
	{
		int num = 0;
		for (int i = 0; i < 12; i++)
		{
			ItemKey itemKey = _equipment[i];
			if (itemKey.IsValid())
			{
				int characterPropertyBonus = DomainManager.Item.GetCharacterPropertyBonus(itemKey, propertyType);
				num += DomainManager.SpecialEffect.ModifyValue(_id, 310, characterPropertyBonus, itemKey.ItemType, itemKey.Id, (int)propertyType);
			}
		}
		return num;
	}

	private int GetPropertyBonusOfCombatSkillEquippingAndBreakout(ECharacterPropertyReferencedType propertyType)
	{
		int num = 0;
		Dictionary<short, GameData.Domains.CombatSkill.CombatSkill> charCombatSkills = DomainManager.CombatSkill.GetCharCombatSkills(_id);
		CombatSkillEquipment combatSkillEquipment = GetCombatSkillEquipment();
		foreach (short item in combatSkillEquipment)
		{
			if (item >= 0 && GetCombatSkillCanAffect(item))
			{
				if (!charCombatSkills.ContainsKey(item))
				{
					AdaptableLog.Warning($"character {this} has never learned combat skill {item}({Config.CombatSkill.Instance[item].Name}) but is trying to access it.");
				}
				else
				{
					num += charCombatSkills[item].GetCharPropertyBonus(propertyType);
				}
			}
		}
		return num;
	}

	private void OfflineUpdateHobbyExpirationDate()
	{
		int num = DomainManager.World.GetCurrYear() + GetHobbyChangingPeriod();
		int hobbyExpirationDate = num * 12 + _birthMonth;
		_hobbyExpirationDate = hobbyExpirationDate;
	}

	private void OfflineChangeHobby(IRandomSource random)
	{
		(short, short) tuple = GenerateRandomHobby(random);
		_lovingItemSubType = tuple.Item1;
		_hatingItemSubType = tuple.Item2;
		sbyte favorabilityType = FavorabilityType.GetFavorabilityType(DomainManager.Character.GetFavorability(_id, DomainManager.Taiwu.GetTaiwuCharId()));
		if (favorabilityType < 3)
		{
			_lovingItemRevealed = false;
			_hatingItemRevealed = false;
		}
		else
		{
			_lovingItemRevealed = true;
			_hatingItemRevealed = true;
		}
		OfflineUpdateHobbyExpirationDate();
	}

	private void CommitChangingHobby(DataContext context)
	{
		SetLovingItemSubType(_lovingItemSubType, context);
		SetHatingItemSubType(_hatingItemSubType, context);
		SetLovingItemRevealed(_lovingItemRevealed, context);
		SetHatingItemRevealed(_hatingItemRevealed, context);
		SetHobbyExpirationDate(_hobbyExpirationDate, context);
		ChangeMerchantType(context, GetOrganizationInfo());
	}

	private bool IgnoreFeature(short featureId)
	{
		if (featureId == 483 && IsTaiwu())
		{
			sbyte stateTemplateIdByAreaId = DomainManager.Map.GetStateTemplateIdByAreaId(GetValidLocation().AreaId);
			if (stateTemplateIdByAreaId != 11)
			{
				return true;
			}
		}
		return false;
	}

	public bool IsMerchant(OrganizationInfo organizationInfo)
	{
		OrganizationItem organizationItem = Config.Organization.Instance[organizationInfo.OrgTemplateId];
		OrganizationMemberItem orgMemberConfig = OrganizationDomain.GetOrgMemberConfig(organizationInfo);
		List<sbyte> identityInteractConfig = orgMemberConfig.IdentityInteractConfig;
		if (organizationItem.IsCivilian && organizationInfo.Grade == 4 && identityInteractConfig != null && identityInteractConfig.Contains(4))
		{
			return true;
		}
		return false;
	}

	public void ChangeMerchantType(DataContext context, OrganizationInfo organizationInfo)
	{
		if (organizationInfo.OrgTemplateId == 16)
		{
			VillagerRoleBase villagerRole = DomainManager.Extra.GetVillagerRole(_id);
			if (villagerRole is VillagerRoleMerchant { CurrentMerchantType: var b } villagerRoleMerchant)
			{
				if (b == 7)
				{
					b = villagerRoleMerchant.SelfDecideMerchantType;
				}
				AddOrSetMerchantType(b, context);
			}
		}
		else if (IsMerchant(organizationInfo))
		{
			sbyte type = (sbyte)context.Random.Next(7);
			AddOrSetMerchantType(type, context);
		}
		else
		{
			RemoveMerchantType(context);
		}
	}

	public void AddOrSetMerchantType(sbyte type, DataContext context)
	{
		if (DomainManager.Extra.TryGetMerchantCharToType(_id, out var _))
		{
			DomainManager.Extra.SetMerchantCharToType(_id, type, context);
		}
		else
		{
			DomainManager.Extra.AddMerchantCharToType(_id, type, context);
		}
	}

	public void RemoveMerchantType(DataContext context)
	{
		if (DomainManager.Extra.TryGetMerchantCharToType(_id, out var _))
		{
			DomainManager.Extra.RemoveMerchantCharToType(_id, context);
		}
	}

	[Obsolete("This method is obsolete, and will be removed in future. Use ChangeCurrMainAttribute instead.")]
	public void ChangeCurrMainAttributeWithoutSpecialEffect(DataContext context, sbyte mainAttributeType, int delta)
	{
		ChangeCurrMainAttribute(context, mainAttributeType, delta);
	}

	[Obsolete("This method is obsolete, and will be removed in future. Use ChangeCurrMainAttributes instead.")]
	public void ChangeCurrMainAttributesWithoutSpecialEffect(DataContext context, MainAttributes delta)
	{
		ChangeCurrMainAttributes(context, delta);
	}

	public void OfflineInheritCrossArchiveCharacter(Character crossArchiveChar)
	{
		_templateId = crossArchiveChar._templateId;
		_creatingType = crossArchiveChar._creatingType;
		_genome = crossArchiveChar._genome;
		_gender = crossArchiveChar._gender;
		_transgender = crossArchiveChar._transgender;
		_bisexual = crossArchiveChar._bisexual;
		_actualAge = crossArchiveChar._actualAge;
		_currAge = crossArchiveChar._currAge;
		_birthMonth = crossArchiveChar._birthMonth;
		_avatar = crossArchiveChar._avatar;
		_fullName = crossArchiveChar._fullName;
		_monasticTitle = crossArchiveChar._monasticTitle;
		_monkType = crossArchiveChar._monkType;
		_baseMaxHealth = crossArchiveChar._baseMaxHealth;
		_health = crossArchiveChar._health;
		_baseMorality = crossArchiveChar._baseMorality;
		_happiness = crossArchiveChar._happiness;
		_baseMainAttributes = crossArchiveChar._baseMainAttributes;
		_currMainAttributes = crossArchiveChar._currMainAttributes;
		_featureIds = new List<short>(crossArchiveChar._featureIds);
		_featureIds.RemoveAll((short featureId) => !CharacterFeature.Instance[featureId].CanCrossArchive);
		_potentialFeatureIds = crossArchiveChar._potentialFeatureIds;
		_extraNeili = crossArchiveChar._extraNeili;
		_baseCombatSkillQualifications = crossArchiveChar._baseCombatSkillQualifications;
		_baseLifeSkillQualifications = crossArchiveChar._baseLifeSkillQualifications;
		_combatSkillQualificationGrowthType = crossArchiveChar._combatSkillQualificationGrowthType;
		_lifeSkillQualificationGrowthType = crossArchiveChar._lifeSkillQualificationGrowthType;
		_skillQualificationBonuses = new List<SkillQualificationBonus>(crossArchiveChar._skillQualificationBonuses);
		_mainAttributeInterest = crossArchiveChar._mainAttributeInterest;
		_combatSkillTypeInterest = crossArchiveChar._combatSkillTypeInterest;
		_lifeSkillTypeInterest = crossArchiveChar._lifeSkillTypeInterest;
		_idealSect = crossArchiveChar._idealSect;
		_lovingItemSubType = crossArchiveChar._lovingItemSubType;
		_hatingItemSubType = crossArchiveChar._hatingItemSubType;
		_lovingItemRevealed = crossArchiveChar._lovingItemRevealed;
		_hatingItemRevealed = crossArchiveChar._hatingItemRevealed;
		_hobbyExpirationDate = crossArchiveChar._hobbyExpirationDate;
		_baseNeiliProportionOfFiveElements = crossArchiveChar._baseNeiliProportionOfFiveElements;
		_preexistenceCharIds = crossArchiveChar._preexistenceCharIds;
		_injuries.Initialize();
		_poisoned.Initialize();
		_disorderOfQi = 0;
	}

	public void AddDarkAsh(DataContext context, LifeRecordCollection lifeRecordCollection = null, int baseTime = int.MinValue)
	{
		if (baseTime == int.MinValue)
		{
			int characterLifeSpanFactor = DomainManager.World.GetCharacterLifeSpanFactor();
			baseTime = (GlobalConfig.Instance.DarkAshDurationBase * characterLifeSpanFactor + context.Random.Next(characterLifeSpanFactor * GlobalConfig.Instance.DarkAshDurationRangeMax - (characterLifeSpanFactor - 100))) / 100;
		}
		int currDate = DomainManager.World.GetCurrDate();
		int num = currDate + _consummateLevel;
		int expireDate = num + baseTime;
		AddFeature(context, 758);
		DomainManager.Extra.RegisterCharacterTemporaryFeature(context, _id, 758, expireDate);
		if (DomainManager.Extra.TryGetDarkAshCounterData(_id, out var _))
		{
			AdaptableLog.Warning($"DarkAsh counter May duplicate for char {this}, please check the call stack:\n{new StackTrace()}");
			DomainManager.Extra.SetDarkAshCounterData(context, _id, new DarkAshCounterData(currDate, _consummateLevel));
		}
		else
		{
			DomainManager.Extra.AddDarkAshCounterData(context, _id, new DarkAshCounterData(currDate, _consummateLevel));
		}
		lifeRecordCollection?.AddGetInfected(_id, currDate, _location);
	}

	public void DirectlyChangeDarkAshDuration(DataContext context, int currDate, int baseTime, int consummateTime, int faithTime)
	{
		DomainManager.Extra.RegisterCharacterTemporaryFeature(context, _id, 758, currDate + baseTime + consummateTime + faithTime);
		if (DomainManager.Extra.TryGetDarkAshCounterData(_id, out var _))
		{
			DomainManager.Extra.SetDarkAshCounterData(context, _id, new DarkAshCounterData(currDate, consummateTime, faithTime));
		}
		else
		{
			DomainManager.Extra.AddDarkAshCounterData(context, _id, new DarkAshCounterData(currDate, consummateTime, faithTime));
		}
	}

	public void SavedFromInfected(DataContext context, bool batchMode = false)
	{
		int saveFromInfectedGainFaith = SaveFromInfectedGainFaith;
		DomainManager.Extra.AddFuyuFaith(context, saveFromInfectedGainFaith);
		if (!batchMode && saveFromInfectedGainFaith > 0)
		{
			DomainManager.World.GetInstantNotificationCollection().AddGainFuyuFaith1(_id, saveFromInfectedGainFaith);
		}
	}

	public void RemoveDarkAsh(DataContext context)
	{
		RemoveFeature(context, 758);
		DomainManager.Extra.UnregisterCharacterTemporaryFeature(context, _id, 758);
		DomainManager.Extra.RemoveDarkAshCounterData(context, _id);
	}

	public DarkAshCounter GetDarkAshCounter()
	{
		DarkAshCounterData data;
		return DomainManager.Extra.TryGetDarkAshCounterData(_id, out data) ? new DarkAshCounter(DarkAshDuration + DomainManager.World.GetCurrDate(), DomainManager.World.GetCurrDate(), data) : new DarkAshCounter(DarkAshDuration + DomainManager.World.GetCurrDate(), DomainManager.World.GetCurrDate());
	}

	public bool ShouldNotifyDarkAshInfected(out int duration)
	{
		int num = (duration = DarkAshDuration);
		return num <= 6 && num > 0;
	}

	public void ExtendDarkAshWithFuyuFaith(DataContext context, int delta, LifeRecordCollection lifeRecordCollection = null)
	{
		int currDate = DomainManager.World.GetCurrDate();
		ExtraDomain extra = DomainManager.Extra;
		int id = _id;
		int temporaryFeatureExpireDate = DomainManager.Extra.GetTemporaryFeatureExpireDate(_id, 758);
		extra.RegisterCharacterTemporaryFeature(context, id, 758, (temporaryFeatureExpireDate != -1) ? (temporaryFeatureExpireDate + delta) : (currDate + delta));
		if (DomainManager.Extra.TryGetDarkAshCounterData(_id, out var data))
		{
			DomainManager.Extra.SetDarkAshCounterData(context, _id, data.OfflineApplyFaithChangeToExtraData(currDate, delta));
		}
		else
		{
			DomainManager.Extra.AddDarkAshCounterData(context, _id, new DarkAshCounterData(currDate, 0, delta));
		}
		DomainManager.Extra.AddFuyuFaith(context, -delta);
		lifeRecordCollection?.AddExtendDarkAshTime(_id, currDate, _location);
	}

	public unsafe sbyte GetCurrMaxEatingSlotsCount()
	{
		MainAttributes maxMainAttributes = GetMaxMainAttributes();
		short maxVitality = maxMainAttributes.Items[3];
		return EatingItems.CalcMaxEatingSlotsCount(maxVitality);
	}

	public void AddEatingItem(DataContext context, ItemKey itemKey, IReadOnlyList<sbyte> targetBodyParts = null)
	{
		Tester.Assert(itemKey.IsValid());
		if (itemKey.ItemType == 8)
		{
			MedicineItem medicineItem = Config.Medicine.Instance[itemKey.TemplateId];
			if (medicineItem.InstantAffect)
			{
				ApplyEatingItemInstantEffects(context, itemKey.ItemType, itemKey.TemplateId, targetBodyParts);
				if (medicineItem.Duration == 0)
				{
					TryApplyAttachedPoison(context, itemKey);
				}
			}
			if (medicineItem.Duration > 0)
			{
				AddEatingItemWithoutInstantEffects(context, itemKey);
			}
			sbyte requiredMainAttributeType = medicineItem.RequiredMainAttributeType;
			if (requiredMainAttributeType >= 0)
			{
				sbyte requiredMainAttributeValue = medicineItem.RequiredMainAttributeValue;
				short num = GetCurrMainAttributes()[requiredMainAttributeType];
				if (num < requiredMainAttributeValue)
				{
					AdaptableLog.TagWarning($"Character {_id}", $"Current main attribute {requiredMainAttributeType} ({num}/{requiredMainAttributeValue}) is not enough to apply medicine {medicineItem.Name}.");
					return;
				}
				_currMainAttributes[requiredMainAttributeType] -= requiredMainAttributeValue;
				SetCurrMainAttributes(_currMainAttributes, context);
			}
		}
		else
		{
			ApplyEatingItemInstantEffects(context, itemKey.ItemType, itemKey.TemplateId, targetBodyParts);
			AddEatingItemWithoutInstantEffects(context, itemKey);
		}
		if (_id == DomainManager.Taiwu.GetTaiwuCharId() && itemKey.ItemType == 9)
		{
			ItemBase baseItem = DomainManager.Item.GetBaseItem(itemKey);
			int value = baseItem.GetValue();
			if (ItemTemplateHelper.GetItemSubType(itemKey.ItemType, itemKey.TemplateId) == 900)
			{
				ProfessionFormulaItem formulaCfg = ProfessionFormula.Instance[99];
				int baseDelta = formulaCfg.Calculate(value);
				DomainManager.Extra.ChangeProfessionSeniority(context, 16, baseDelta);
			}
			else
			{
				ProfessionFormulaItem formulaCfg2 = ProfessionFormula.Instance[48];
				int baseDelta2 = formulaCfg2.Calculate(value);
				DomainManager.Extra.ChangeProfessionSeniority(context, 7, baseDelta2);
			}
		}
		if (IsTaiwu() && itemKey.ItemType != 9)
		{
			int value2 = DomainManager.Item.GetValue(itemKey);
			int baseDelta3 = ProfessionFormulaImpl.Calculate(62, value2);
			DomainManager.Extra.ChangeProfessionSeniority(context, 9, baseDelta3);
		}
	}

	public void ApplyEatingItemInstantEffects(DataContext context, sbyte itemType, short templateId, IReadOnlyList<sbyte> targetBodyParts = null)
	{
		switch (itemType)
		{
		case 7:
		{
			FoodItem config = Config.Food.Instance[templateId];
			ApplyFoodInstantEffect(context, config);
			break;
		}
		case 8:
		{
			MedicineItem medicineItem = Config.Medicine.Instance[templateId];
			if (medicineItem.EffectType != EMedicineEffectType.Invalid)
			{
				MedicineEatingInstantEffect effect3 = new MedicineEatingInstantEffect(medicineItem, targetBodyParts);
				ApplyMedicineInstantEffect(context, ref effect3);
			}
			ApplySpecialMedicineEffect(context, templateId);
			break;
		}
		case 9:
		{
			TeaWineItem config2 = Config.TeaWine.Instance[templateId];
			ApplyTeaWineInstantEffect(context, config2);
			break;
		}
		case 12:
		{
			short neili = Config.Misc.Instance[templateId].Neili;
			ChangeCurrNeili(context, neili);
			break;
		}
		case 5:
		{
			MaterialItem materialItem = Config.Material.Instance[templateId];
			short baseMaxHealthDelta = materialItem.BaseMaxHealthDelta;
			if (baseMaxHealthDelta != 0)
			{
				ChangeBaseMaxHealth(context, baseMaxHealthDelta);
			}
			if (materialItem.PrimaryEffectType >= EMedicineEffectType.RecoverOuterInjury)
			{
				MedicineEatingInstantEffect effect = new MedicineEatingInstantEffect(materialItem, primary: true);
				ApplyMedicineInstantEffect(context, ref effect);
			}
			if (materialItem.SecondaryEffectType >= EMedicineEffectType.RecoverOuterInjury)
			{
				MedicineEatingInstantEffect effect2 = new MedicineEatingInstantEffect(materialItem, primary: false);
				ApplyMedicineInstantEffect(context, ref effect2);
			}
			break;
		}
		default:
			throw new Exception($"Invalid item type: {itemType}");
		}
	}

	public void AddEatingItemWithoutInstantEffects(DataContext context, ItemKey itemKey)
	{
		DomainManager.Item.SetPoisonsIdentified(context, itemKey, isIdentified: true);
		DomainManager.Item.SetOwner(itemKey, ItemOwnerType.CharacterEatingItem, _id);
		bool flag = OccupyAvailableEatingSlot(itemKey);
		sbyte currMaxEatingSlotsCount = GetCurrMaxEatingSlotsCount();
		int num = (flag ? _eatingItems.GetAvailableEatingSlot(currMaxEatingSlotsCount) : (-1));
		if (num < 0 && flag)
		{
			throw new Exception($"Character {_id}: EatingItems slots are full");
		}
		short itemSubType = ItemTemplateHelper.GetItemSubType(itemKey.ItemType, itemKey.TemplateId);
		short eatableItemDuration = ItemTemplateHelper.GetEatableItemDuration(itemKey.ItemType, itemKey.TemplateId);
		if (num >= 0)
		{
			_eatingItems.Set(num, itemKey, eatableItemDuration);
			if (_id != DomainManager.Taiwu.GetTaiwuCharId() || ((itemSubType != 900 || !DomainManager.Extra.IsProfessionalSkillUnlocked(16, 1)) && (itemSubType != 901 || !DomainManager.Extra.IsProfessionalSkillUnlocked(7, 1))))
			{
				short deltaDuration = TryApplyAttachedPoison(context, itemKey);
				_eatingItems.ChangeDuration(context, num, deltaDuration);
			}
			SetEatingItems(ref _eatingItems, context);
		}
		else if (itemKey.ItemType == 12)
		{
			TryApplyAttachedPoison(context, itemKey);
			DomainManager.Item.RemoveItem(context, itemKey);
		}
		else if (itemKey.ItemType == 8 && Config.Medicine.Instance[itemKey.TemplateId].WugGrowthType == 5)
		{
			AddWug(context, itemKey);
		}
		Events.RaiseEatingItem(context, this, itemKey);
		if (1 == 0)
		{
		}
		bool flag2 = itemSubType switch
		{
			701 => IsForbiddenToEatMeat(), 
			901 => IsForbiddenToDrinkingWines(), 
			_ => false, 
		};
		if (1 == 0)
		{
		}
		if (flag2)
		{
			LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
			int currDate = DomainManager.World.GetCurrDate();
			Location location = GetLocation();
			lifeRecordCollection.AddMonkBreakRule(_id, currDate, location, itemKey.ItemType, itemKey.TemplateId);
			SecretInformationCollection secretInformationCollection = DomainManager.Information.GetSecretInformationCollection();
			int dataOffset = secretInformationCollection.AddMonkBreakRule(_id, (ulong)itemKey);
			int num2 = DomainManager.Information.AddSecretInformationMetaData(context, dataOffset);
		}
	}

	public void ApplyTopicalMedicine(DataContext context, ItemKey itemKey)
	{
		Tester.Assert(itemKey.IsValid());
		if (itemKey.ItemType != 8)
		{
			throw new Exception($"Invalid item type: {itemKey.ItemType}");
		}
		MedicineItem medicineItem = Config.Medicine.Instance[itemKey.TemplateId];
		if (medicineItem.EffectType != EMedicineEffectType.RecoverOuterInjury && medicineItem.EffectType != EMedicineEffectType.RecoverInnerInjury)
		{
			throw new Exception($"Invalid EffectType: {medicineItem.EffectType}");
		}
		OfflineApplyTopicalMedicineInternal(context.Random, medicineItem);
		SetCurrMainAttributes(_currMainAttributes, context);
		SetInjuries(_injuries, context);
		TryApplyAttachedPoison(context, itemKey);
	}

	public void ApplySpecialMedicineEffect(DataContext context, short templateId)
	{
		switch (templateId)
		{
		case 78:
			_injuries.Initialize();
			SetInjuries(_injuries, context);
			break;
		case 79:
		{
			SetXiangshuInfection(0, context);
			short infectionFeatureIdThatShouldBe = XiangshuInfectionTypeHelper.GetInfectionFeatureIdThatShouldBe(_xiangshuInfection);
			if (!_featureIds.Contains(infectionFeatureIdThatShouldBe))
			{
				AddFeature(context, infectionFeatureIdThatShouldBe, removeMutexFeature: true);
				Events.RaiseXiangshuInfectionFeatureChanged(context, this, infectionFeatureIdThatShouldBe);
			}
			break;
		}
		case 81:
		{
			SetXiangshuInfection(100, context);
			short infectionFeatureIdThatShouldBe2 = XiangshuInfectionTypeHelper.GetInfectionFeatureIdThatShouldBe(_xiangshuInfection);
			if (!_featureIds.Contains(infectionFeatureIdThatShouldBe2))
			{
				AddFeature(context, infectionFeatureIdThatShouldBe2, removeMutexFeature: true);
				Events.RaiseXiangshuInfectionFeatureChanged(context, this, infectionFeatureIdThatShouldBe2);
			}
			break;
		}
		case 346:
			if (_currAge >= GlobalConfig.Instance.AgeBaby)
			{
				bool flag = GetAgeGroup() != 1;
				short currAge = _currAge;
				SetCurrAge((short)GlobalConfig.Instance.AgeBaby, context);
				Events.RaiseCharacterAgeChanged(context, this, currAge, _currAge);
				if (flag)
				{
					ItemKey itemKey = DomainManager.Item.CreateClothing(context, 65, _gender);
					AddInventoryItem(context, itemKey, 1);
					ChangeEquipment(context, -1, 4, itemKey);
				}
			}
			break;
		case 387:
			AddFeature(context, 337);
			break;
		}
	}

	public void AddWug(DataContext context, short medicineTemplateId)
	{
		ItemKey wugItemKey = new ItemKey(8, 0, medicineTemplateId, -1);
		AddWug(context, wugItemKey);
	}

	public void AddWug(DataContext context, ItemKey wugItemKey)
	{
		Tester.Assert(wugItemKey.ItemType == 8);
		MedicineItem medicineItem = Config.Medicine.Instance[wugItemKey.TemplateId];
		Tester.Assert(medicineItem.ItemSubType == 802);
		int num = _eatingItems.IndexOfWug(medicineItem);
		if ((num >= 0) ? OfflineReplaceWug(context, num, wugItemKey) : OfflineAddNewWug(context, wugItemKey))
		{
			SetEatingItems(ref _eatingItems, context);
		}
	}

	public void RemoveWug(DataContext context, short medicineTemplateId)
	{
		MedicineItem medicineItem = Config.Medicine.Instance[medicineTemplateId];
		int num = _eatingItems.IndexOfWug(medicineItem);
		if (num >= 0)
		{
			ItemKey itemKey = _eatingItems.Get(num);
			_eatingItems.Clear(num);
			_eatingItems.SortWugs();
			SetEatingItems(ref _eatingItems, context);
			if (itemKey.IsValid())
			{
				DomainManager.Item.RemoveItem(context, itemKey);
			}
			Events.RaiseRemoveWug(context, _id, medicineItem.TemplateId);
		}
	}

	public void OnDeathTransferWugKings(DataContext context, Location charLocation)
	{
		List<ItemKey> list = ObjectPool<List<ItemKey>>.Instance.Get();
		OnDeathTransferWugKings(context, list);
		InstantNotificationCollection instantNotificationCollection = DomainManager.World.GetInstantNotificationCollection();
		MonthlyNotificationCollection monthlyNotificationCollection = DomainManager.World.GetMonthlyNotificationCollection();
		foreach (ItemKey item in list)
		{
			_inventory.OfflineAdd(item, 1);
			if (DomainManager.World.GetAdvancingMonthState() != 0)
			{
				monthlyNotificationCollection.AddWugKingParasitiferDead(item.ItemType, item.TemplateId, charLocation, _id);
			}
			else
			{
				instantNotificationCollection.AddWugKingParasitiferDead(item.ItemType, item.TemplateId, charLocation, _id);
			}
		}
	}

	public void OnDeathTransferWugKings(DataContext context, List<ItemKey> wugKings)
	{
		for (int i = 0; i < 9; i++)
		{
			ItemKey itemKey = _eatingItems.Get(i);
			if (EatingItems.IsWugKing(itemKey))
			{
				_eatingItems.Clear(i);
				wugKings.Add(itemKey);
				Events.RaiseRemoveWug(context, _id, itemKey.TemplateId);
			}
		}
	}

	public unsafe void ClearEatingItems(DataContext context)
	{
		Character taiwu = DomainManager.Taiwu.GetTaiwu();
		Location validLocation = taiwu.GetValidLocation();
		MapBlockData block = DomainManager.Map.GetBlock(validLocation);
		InstantNotificationCollection instantNotificationCollection = DomainManager.World.GetInstantNotificationCollection();
		for (int num = 8; num >= 0; num--)
		{
			ItemKey itemKey = (ItemKey)_eatingItems.ItemKeys[num];
			if (EatingItems.IsWug(itemKey))
			{
				Events.RaiseRemoveWug(context, _id, itemKey.TemplateId);
			}
			if (EatingItems.IsWugKing(itemKey) && Template.AllowDropWugKing)
			{
				DomainManager.Map.AddBlockItem(context, block, itemKey, 1);
				if (DomainManager.Combat.GetIsPuppetCombat())
				{
					instantNotificationCollection.AddWugKingEscape2(itemKey.ItemType, itemKey.TemplateId, validLocation);
				}
				else
				{
					instantNotificationCollection.AddWugKingEscape1(itemKey.ItemType, itemKey.TemplateId, _templateId, validLocation);
				}
			}
			else if (itemKey.IsValid())
			{
				DomainManager.Item.RemoveItem(context, itemKey);
			}
			_eatingItems.Clear(num);
		}
		SetEatingItems(ref _eatingItems, context);
	}

	public unsafe short TryApplyAttachedPoison(DataContext context, ItemKey itemKey)
	{
		if (!ModificationStateHelper.IsActive(itemKey.ModificationState, 1) || !DomainManager.Item.PoisonEffects.TryGetValue(itemKey.Id, out var value))
		{
			return 0;
		}
		DomainManager.Item.SetPoisonsIdentified(context, itemKey, isIdentified: true);
		PoisonsAndLevels delta = value.GetAllPoisonsAndLevels();
		for (sbyte b = 0; b < 6; b++)
		{
			delta.Values[b] = (short)(delta.Values[b] * 10 * delta.Levels[b]);
		}
		ChangePoisoned(context, ref delta);
		short medicineTemplateId = value.GetMedicineTemplateId();
		ApplyMixedPoisonInstantEffects(context);
		return Config.Medicine.Instance[medicineTemplateId].Duration;
	}

	private void ApplyMixedPoisonInstantEffects(DataContext context)
	{
		for (sbyte b = 15; b <= 34; b++)
		{
			ApplyMixedPoisonInstantEffect(context, b);
		}
	}

	private void ApplyMixedPoisonInstantEffect(DataContext context, sbyte mixedPoisonType)
	{
		if (GetMixedPoisonTypeRelatedMarkCount(mixedPoisonType) == 0)
		{
			return;
		}
		short index = MixedPoisonType.ToMixPoisonEffectTemplateId(mixedPoisonType);
		MixPoisonEffectItem mixPoisonEffectItem = MixPoisonEffect.Instance[index];
		if (mixPoisonEffectItem.InstantEffect)
		{
			if (_creatingType == 1)
			{
				LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
				lifeRecordCollection.AddMixedPoisonEffectRecord(this, mixPoisonEffectItem);
			}
			sbyte b = MixedPoisonType.ToCombatSkillEquipType(mixedPoisonType);
			if (b != -1 && IsTaiwu())
			{
				DomainManager.Taiwu.ClearExceedCombatSkills(context);
			}
		}
	}

	private void MakeExistingInjuriesWorse(DataContext context, bool isInnerInjury, sbyte delta)
	{
		Injuries injuries = GetInjuries();
		for (sbyte b = 0; b < 7; b++)
		{
			sbyte b2 = injuries.Get(b, isInnerInjury);
			if (b2 > 0)
			{
				injuries.Change(b, isInnerInjury, delta);
			}
		}
		SetInjuries(injuries, context);
	}

	public unsafe int GetMixedPoisonTypeRelatedMarkCount(sbyte mixedPoisonType)
	{
		sbyte[] array = MixedPoisonType.ToPoisonTypes[mixedPoisonType];
		PoisonInts poisoned = _poisoned;
		int num = 0;
		foreach (sbyte b in array)
		{
			sbyte b2 = PoisonsAndLevels.CalcPoisonedLevel(poisoned.Items[b]);
			if (b2 == 0 || HasPoisonImmunity(b))
			{
				return 0;
			}
			num += b2;
		}
		return num;
	}

	public int GetRealMixedPoisonTypeRelatedMarkCount(sbyte mixedPoisonType)
	{
		sbyte[] array = MixedPoisonType.ToPoisonTypes[mixedPoisonType];
		PoisonInts poisoned = _poisoned;
		int num = 0;
		foreach (sbyte b in array)
		{
			sbyte b2 = PoisonsAndLevels.CalcPoisonedLevel(poisoned[b]);
			if (b2 == 0 || GetPoisonImmunities()[b] || DomainManager.Extra.HasPoisonImmunity(_id, b))
			{
				return 0;
			}
			num += b2;
		}
		return num;
	}

	private void ApplyMedicineInstantEffect(DataContext context, ref MedicineEatingInstantEffect effect)
	{
		switch (effect.EffectType)
		{
		case EMedicineEffectType.RecoverOuterInjury:
		{
			IReadOnlyList<sbyte> targetBodyParts = effect.TargetBodyParts;
			if (targetBodyParts != null && targetBodyParts.Count > 0)
			{
				CalcMedicineEffect_RecoverInjury(ref _injuries, effect.TargetBodyParts, inner: false, ref effect);
			}
			else
			{
				CalcMedicineEffect_RecoverInjury(ref _injuries, context.Random, inner: false, ref effect);
			}
			SetInjuries(_injuries, context);
			break;
		}
		case EMedicineEffectType.RecoverInnerInjury:
		{
			IReadOnlyList<sbyte> targetBodyParts = effect.TargetBodyParts;
			if (targetBodyParts != null && targetBodyParts.Count > 0)
			{
				CalcMedicineEffect_RecoverInjury(ref _injuries, effect.TargetBodyParts, inner: true, ref effect);
			}
			else
			{
				CalcMedicineEffect_RecoverInjury(ref _injuries, context.Random, inner: true, ref effect);
			}
			SetInjuries(_injuries, context);
			break;
		}
		case EMedicineEffectType.RecoverHealth:
			CalcMedicineEffect_RecoverHealth(ref _health, ref effect);
			SetHealth(_health, context);
			break;
		case EMedicineEffectType.ChangeDisorderOfQi:
			CalcMedicineEffect_RecoverDisorderOfQi(ref _disorderOfQi, ref effect);
			SetDisorderOfQi(_disorderOfQi, context);
			break;
		case EMedicineEffectType.DetoxPoison:
			CalcMedicineEffect_DetoxPoison(ref _poisoned, ref effect);
			SetPoisoned(ref _poisoned, context);
			break;
		case EMedicineEffectType.DetoxWug:
		{
			sbyte detoxWugType2 = effect.DetoxWugType;
			ApplyMedicineInstantEffect_DetoxWug(context, detoxWugType2, effect.Grade);
			break;
		}
		case EMedicineEffectType.ApplyPoison:
		{
			CalcMedicineEffect_ApplyPoison(ref _poisoned, ref effect);
			SetPoisoned(ref _poisoned, context);
			ApplyMixedPoisonInstantEffects(context);
			sbyte detoxWugType = effect.DetoxWugType;
			ApplyMedicineInstantEffect_DetoxWug(context, detoxWugType, effect.Grade);
			break;
		}
		}
	}

	public void CalcMedicineEffect_RecoverHealth(ref short health, ref MedicineEatingInstantEffect effect)
	{
		int delta = CalcMedicineEffectDelta(GetLeftMaxHealth(), effect.EffectValue, effect.EffectIsPercentage);
		health = CalcChangedHealth(health, delta);
	}

	public int CalcMedicineHealInjuryTimes(int effectValue)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		CValuePercent val = CValuePercent.op_Implicit(GetSpecialEffectModifiedMedicineEffectValue(100));
		return Math.Max(effectValue * val, 1);
	}

	public int GetSpecialEffectModifiedMedicineEffectValue(int effectValue)
	{
		return DomainManager.SpecialEffect.ModifyValue(_id, 261, effectValue);
	}

	public int CalcMedicineEffectDelta(int fullRangeValue, short effectValue, bool isPercentage)
	{
		return GetSpecialEffectModifiedMedicineEffectValue(EMedicineEffectSubTypeExtension.EffectValue(fullRangeValue, effectValue, isPercentage));
	}

	public int CalcMedicineEffectDelta_RecoverDisorderOfQi(short effectValue, bool isPercentage)
	{
		return -CalcMedicineEffectDelta(DisorderLevelOfQi.MaxValue, effectValue, isPercentage);
	}

	public void CalcMedicineEffect_RecoverDisorderOfQi(ref short disorderOfQi, ref MedicineEatingInstantEffect effect)
	{
		int delta = CalcMedicineEffectDelta_RecoverDisorderOfQi(effect.EffectValue, effect.EffectIsPercentage);
		disorderOfQi = CalcChangedDisorderOfQiWithoutEffect(disorderOfQi, delta);
	}

	public unsafe void CalcMedicineEffect_DetoxPoison(ref PoisonInts poison, ref MedicineEatingInstantEffect effect)
	{
		sbyte poisonType = effect.PoisonType;
		sbyte b = (sbyte)effect.EffectThresholdValue;
		if (b >= PoisonsAndLevels.CalcPoisonedLevel(poison.Items[poisonType]))
		{
			int delta = -CalcMedicineEffectDelta(poison.Items[poisonType], effect.EffectValue, effect.EffectIsPercentage);
			CalcChangedPoisoned(ref poison, poisonType, b, delta);
		}
	}

	public void CalcMedicineEffect_ApplyPoison(ref PoisonInts poison, ref MedicineEatingInstantEffect effect)
	{
		sbyte poisonType = effect.PoisonType;
		sbyte b = (sbyte)effect.EffectThresholdValue;
		short delta = PoisonsAndLevels.CalcApplyItemPoisonAmount(effect.EffectValue, b);
		CalcChangedPoisoned(ref poison, poisonType, b, delta);
	}

	private void ApplyTeaWineInstantEffect(DataContext context, TeaWineItem config)
	{
		if (_id == DomainManager.Taiwu.GetTaiwuCharId())
		{
			short directChangeOfQiDisorder = config.DirectChangeOfQiDisorder;
			if (config.ItemSubType == 900)
			{
				if (DomainManager.Extra.IsProfessionalSkillUnlockedAndEquipped(54))
				{
					int num = ProfessionSkillHandle.TeaTasterSkill_GetActionPointGained();
					int val = GlobalConfig.Instance.ProfessionSkillRecoverActionPointLimit - num;
					int actionPointRecover = config.ActionPointRecover;
					int num2 = Math.Min(actionPointRecover, val);
					ProfessionSkillHandle.TeaTasterSkill_SetActionPointGained(context, num + num2);
					DomainManager.Extra.ChangeActionPoint(context, num2);
					if (num2 > 0)
					{
						DomainManager.World.GetInstantNotificationCollection().AddDrinkTeaRecharge(_id, num2 / 10);
					}
				}
				ChangeDisorderOfQiRandomRecovery(context, directChangeOfQiDisorder);
			}
			else if (!DomainManager.Extra.IsProfessionalSkillUnlocked(7, 1))
			{
				ChangeDisorderOfQiRandomRecovery(context, directChangeOfQiDisorder);
			}
		}
		else
		{
			ChangeDisorderOfQiRandomRecovery(context, config.DirectChangeOfQiDisorder);
		}
	}

	private void ApplyFoodInstantEffect(DataContext context, FoodItem config)
	{
		ChangeCurrMainAttributes(context, config.MainAttributesRegen);
	}

	public void CalcMedicineEffect_RecoverInjury(ref Injuries injuries, IRandomSource random, bool inner, ref MedicineEatingInstantEffect config)
	{
		List<sbyte> list = ObjectPool<List<sbyte>>.Instance.Get();
		List<sbyte> list2 = ObjectPool<List<sbyte>>.Instance.Get();
		int num = CalcMedicineHealInjuryTimes(config.InjuryRecoveryTimes);
		while (num > 0)
		{
			list2.Clear();
			int num2 = 0;
			for (sbyte b = 0; b < 7; b++)
			{
				sbyte b2 = injuries.Get(b, inner);
				if (b2 <= config.EffectThresholdValue && b2 > 0 && !list.Contains(b))
				{
					if (b2 > num2)
					{
						num2 = b2;
						list2.Clear();
					}
					if (b2 == num2)
					{
						list2.Add(b);
					}
				}
			}
			if (list2.Count == 0)
			{
				break;
			}
			int num3 = Math.Min(num, list2.Count);
			num -= num3;
			foreach (sbyte item in RandomUtils.GetRandomUnrepeated(random, num3, list2))
			{
				list.Add(item);
			}
		}
		CalcMedicineEffect_RecoverInjury(ref injuries, (IReadOnlyList<sbyte>)list, inner, ref config);
		ObjectPool<List<sbyte>>.Instance.Return(list);
		ObjectPool<List<sbyte>>.Instance.Return(list2);
	}

	public void CalcMedicineEffect_RecoverInjury(ref Injuries injuries, IReadOnlyList<sbyte> bodyParts, bool inner, ref MedicineEatingInstantEffect config)
	{
		foreach (sbyte bodyPart in bodyParts)
		{
			injuries.Change(bodyPart, inner, -config.EffectValue);
		}
	}

	private void ApplyMedicineInstantEffect_DetoxWug(DataContext context, sbyte wugType, sbyte medicineGrade)
	{
		int num = _eatingItems.IndexOfWug(wugType);
		if (num >= 0)
		{
			short deltaWugDuration = GameData.Domains.Item.Medicine.GetDeltaWugDuration(medicineGrade);
			short templateId = _eatingItems.Get(num).TemplateId;
			_eatingItems.ChangeDuration(context, num, deltaWugDuration);
			SetEatingItems(ref _eatingItems, context);
			if (_eatingItems.Get(num) == ItemKey.Invalid)
			{
				Events.RaiseRemoveWug(context, _id, templateId);
			}
		}
	}

	private unsafe void OfflineApplyTopicalMedicineInternal(IRandomSource random, MedicineItem config)
	{
		MedicineEatingInstantEffect config2 = new MedicineEatingInstantEffect(config);
		bool inner = config2.EffectType == EMedicineEffectType.RecoverInnerInjury;
		CalcMedicineEffect_RecoverInjury(ref _injuries, random, inner, ref config2);
		sbyte requiredMainAttributeType = config.RequiredMainAttributeType;
		if (requiredMainAttributeType >= 0)
		{
			sbyte requiredMainAttributeValue = config.RequiredMainAttributeValue;
			MainAttributes currMainAttributes = GetCurrMainAttributes();
			short num = currMainAttributes.Items[requiredMainAttributeType];
			if (num < requiredMainAttributeValue)
			{
				AdaptableLog.TagWarning($"Character {_id}", $"Current main attribute {requiredMainAttributeType} ({num}/{requiredMainAttributeValue}) is not enough to apply topical medicine {config.Name}.");
			}
			else
			{
				ref short reference = ref _currMainAttributes.Items[requiredMainAttributeType];
				reference -= requiredMainAttributeValue;
			}
		}
	}

	private bool OfflineReplaceWug(DataContext context, int index, ItemKey wugItemKey)
	{
		ItemKey itemKey = _eatingItems.Get(index);
		if (!EatingItems.IsWug(itemKey) || !EatingItems.IsWug(wugItemKey))
		{
			throw new Exception($"cannot replace {itemKey} to {wugItemKey} as wug");
		}
		MedicineItem medicineItem = Config.Medicine.Instance[itemKey.TemplateId];
		MedicineItem medicineItem2 = Config.Medicine.Instance[wugItemKey.TemplateId];
		_eatingItems.Clear(index);
		_eatingItems.Set(index, wugItemKey, medicineItem2.Duration);
		Events.RaiseRemoveWug(context, _id, medicineItem.TemplateId);
		Events.RaiseAddWug(context, _id, medicineItem2.TemplateId, medicineItem.TemplateId);
		if (itemKey.IsValid())
		{
			DomainManager.Item.RemoveItem(context, itemKey);
		}
		return true;
	}

	private unsafe bool OfflineAddNewWug(DataContext context, ItemKey wugItemKey)
	{
		sbyte slotForNewWug = _eatingItems.GetSlotForNewWug();
		if (slotForNewWug < 0)
		{
			throw new Exception($"Character {_id}: There is no potential available eating slot");
		}
		MedicineItem medicineItem = Config.Medicine.Instance[wugItemKey.TemplateId];
		if (((ItemKey)_eatingItems.ItemKeys[slotForNewWug]).IsValid())
		{
			_eatingItems.Clear(slotForNewWug);
		}
		_eatingItems.Set(slotForNewWug, wugItemKey, medicineItem.Duration);
		Events.RaiseAddWug(context, _id, wugItemKey.TemplateId, -1);
		return true;
	}

	private static bool OccupyAvailableEatingSlot(ItemKey itemKey)
	{
		if (itemKey.ItemType == 12)
		{
			return false;
		}
		if (ItemTemplateHelper.GetItemSubType(itemKey.ItemType, itemKey.TemplateId) == 802)
		{
			return false;
		}
		return true;
	}

	public sbyte GetFameType()
	{
		sbyte fameType = FameType.GetFameType(GetFame());
		if (fameType != 3)
		{
			return fameType;
		}
		return (sbyte)(IsBothGoodAndBad() ? (-2) : 3);
	}

	public void RecordFameAction(DataContext context, short fameActionId, int targetCharId = -1, short fameMultiplier = 1, bool jumpAccordingToTargetFame = true)
	{
		FameActionItem fameActionItem = FameAction.Instance[fameActionId];
		if (targetCharId >= 0 && fameActionItem.HasJump && jumpAccordingToTargetFame)
		{
			sbyte targetFameType = 3;
			if (DomainManager.Character.TryGetElement_Objects(targetCharId, out var element))
			{
				targetFameType = element.GetFameType();
			}
			else
			{
				DeadCharacter deadCharacter = DomainManager.Character.TryGetDeadCharacter(targetCharId);
				if (deadCharacter != null)
				{
					targetFameType = deadCharacter.FameType;
				}
			}
			RecordFameAction(context, fameActionId, targetFameType, fameMultiplier);
		}
		else
		{
			RecordFameActionInternal(context, fameActionId, fameMultiplier);
		}
	}

	public void RecordFameAction(DataContext context, short fameActionId, sbyte targetFameType, short fameMultiplier = 1)
	{
		FameActionItem fameActionItem = FameAction.Instance[fameActionId];
		if (targetFameType >= 0 && fameActionItem.HasJump)
		{
			fameActionId = ((targetFameType == 3 || targetFameType == -2) ? fameActionItem.NormalJumpId : ((targetFameType >= 3) ? fameActionItem.GoodJumpId : fameActionItem.BadJumpId));
			if (fameActionId < 0)
			{
				return;
			}
		}
		RecordFameActionInternal(context, fameActionId, fameMultiplier);
	}

	private bool IsBothGoodAndBad()
	{
		CharacterItem characterItem = Config.Character.Instance[_templateId];
		if (_creatingType != 1)
		{
			return false;
		}
		int num = 0;
		bool flag = IsTaiwu();
		foreach (short featureId in _featureIds)
		{
			OrganizationItem organizationItem = Config.Organization.Instance.FirstOrDefault((OrganizationItem o) => o.MemberFeature == featureId);
			if (organizationItem != null)
			{
				CharacterFeatureItem characterFeatureItem = CharacterFeature.Instance[featureId];
				num = ((!flag) ? ((organizationItem.TemplateId != _organizationInfo.OrgTemplateId) ? (num + Math.Abs(characterFeatureItem.NotSectFameBonu)) : (num + Math.Abs(characterFeatureItem.SectFameBonus[_organizationInfo.Grade]))) : (num + Math.Abs(characterFeatureItem.TaiwuFameBonu)));
			}
		}
		int currDate = DomainManager.World.GetCurrDate();
		int num2 = 0;
		for (int count = _fameActionRecords.Count; num2 < count; num2++)
		{
			FameActionRecord fameActionRecord = _fameActionRecords[num2];
			if (fameActionRecord.EndDate >= currDate)
			{
				num += Math.Abs(fameActionRecord.Value);
			}
		}
		return num >= 50;
	}

	private void RecordFameActionInternal(DataContext context, short fameActionId, short fameMultiplier)
	{
		FameActionItem fameActionItem = FameAction.Instance[fameActionId];
		int currDate = DomainManager.World.GetCurrDate();
		int num = -1;
		int i = 0;
		for (int num2 = _fameActionRecords.Count; i < num2; i++)
		{
			FameActionRecord fameActionRecord = _fameActionRecords[i];
			if (fameActionRecord.EndDate <= currDate)
			{
				CollectionUtils.SwapAndRemove(_fameActionRecords, i);
				i--;
				num2--;
			}
			else if (fameActionRecord.Id == fameActionId)
			{
				num = i;
			}
		}
		if (num >= 0)
		{
			FameActionRecord value = _fameActionRecords[num];
			int num3 = value.Value + fameActionItem.Fame * fameMultiplier;
			int num4 = fameActionItem.Fame * fameActionItem.MaxStackCount;
			num3 = ((fameActionItem.Fame <= 0) ? ((num3 < num4) ? num4 : num3) : ((num3 > num4) ? num4 : num3));
			value.Value = (short)num3;
			if (fameActionItem.RepeatType == 0)
			{
				value.EndDate = currDate + fameActionItem.Duration;
			}
			else
			{
				value.EndDate += fameActionItem.Duration;
			}
			_fameActionRecords[num] = value;
		}
		else
		{
			short value2 = (short)(fameActionItem.Fame * Math.Min(fameMultiplier, fameActionItem.MaxStackCount));
			FameActionRecord item = new FameActionRecord(fameActionId, value2, currDate + fameActionItem.Duration);
			_fameActionRecords.Add(item);
		}
		SetFameActionRecords(_fameActionRecords, context);
	}

	public bool AddFeature(DataContext context, short featureId, bool removeMutexFeature = false)
	{
		bool flag = OfflineAddFeature(featureId, removeMutexFeature);
		if (flag)
		{
			SetFeatureIds(_featureIds, context);
			Events.RaiseCharacterFeatureAdded(context, this, featureId);
		}
		return flag;
	}

	public void RevertFeatures(DataContext context, List<ShortListModification> modifications)
	{
		for (int num = modifications.Count - 1; num >= 0; num--)
		{
			ShortListModification shortListModification = modifications[num];
			if (shortListModification.ModificationType == 0)
			{
				if (shortListModification.Index < _featureIds.Count && _featureIds[shortListModification.Index] == shortListModification.Element)
				{
					_featureIds.RemoveAt(shortListModification.Index);
				}
				else
				{
					_featureIds.Remove(shortListModification.Element);
				}
			}
			else if (shortListModification.Index <= _featureIds.Count)
			{
				_featureIds.Insert(shortListModification.Index, shortListModification.Element);
			}
			else
			{
				_featureIds.Add(shortListModification.Element);
			}
		}
		CheckForMutexFeatures();
		SetFeatureIds(_featureIds, context);
	}

	public void RemoveFeature(DataContext context, short featureId)
	{
		if (_featureIds.Remove(featureId))
		{
			SetFeatureIds(_featureIds, context);
			Events.RaiseCharacterFeatureRemoved(context, this, featureId);
		}
	}

	public void ClearGeneticFeatures(DataContext context)
	{
		for (int num = _featureIds.Count - 1; num >= 0; num--)
		{
			short num2 = _featureIds[num];
			CharacterFeatureItem characterFeatureItem = CharacterFeature.Instance[num2];
			if (characterFeatureItem.GeneticProb != 0)
			{
				_featureIds.RemoveAt(num);
				DomainManager.SpecialEffect.RemoveFeatureEffect(context, _id, num2);
			}
		}
		SetFeatureIds(_featureIds, context);
	}

	public bool RemoveFeatureGroup(DataContext context, short featureGroupId)
	{
		CharacterFeature instance = CharacterFeature.Instance;
		int i = 0;
		for (int count = _featureIds.Count; i < count; i++)
		{
			short index = _featureIds[i];
			CharacterFeatureItem characterFeatureItem = instance[index];
			if (characterFeatureItem.MutexGroupId == featureGroupId)
			{
				_featureIds.RemoveAt(i);
				SetFeatureIds(_featureIds, context);
				return true;
			}
		}
		return false;
	}

	public short GetGroupFeature(short featureGroupId)
	{
		CharacterFeature instance = CharacterFeature.Instance;
		int i = 0;
		for (int count = _featureIds.Count; i < count; i++)
		{
			short num = _featureIds[i];
			short mutexGroupId = instance[num].MutexGroupId;
			if (mutexGroupId == featureGroupId)
			{
				return num;
			}
		}
		return -1;
	}

	public void CalcGroupFeatures(Dictionary<short, short> groupToFeatures)
	{
		groupToFeatures.Clear();
		int i = 0;
		for (int count = _featureIds.Count; i < count; i++)
		{
			short num = _featureIds[i];
			short mutexGroupId = CharacterFeature.Instance[num].MutexGroupId;
			if (mutexGroupId >= 0)
			{
				groupToFeatures.Add(mutexGroupId, num);
			}
		}
	}

	public bool ChangeFeatureByWugKing(IRandomSource random)
	{
		Dictionary<short, short> dictionary = ObjectPool<Dictionary<short, short>>.Instance.Get();
		List<short> list = ObjectPool<List<short>>.Instance.Get();
		List<short> list2 = ObjectPool<List<short>>.Instance.Get();
		List<short> list3 = ObjectPool<List<short>>.Instance.Get();
		List<short> list4 = ObjectPool<List<short>>.Instance.Get();
		CalcGroupFeatures(dictionary);
		list.Clear();
		list2.Clear();
		list3.Clear();
		list4.Clear();
		foreach (CharacterFeatureItem item in (IEnumerable<CharacterFeatureItem>)CharacterFeature.Instance)
		{
			if (!item.IsNormal() || item.IsNeutral())
			{
				continue;
			}
			if (dictionary.TryGetValue(item.MutexGroupId, out var value))
			{
				if (value == item.TemplateId)
				{
					if (item.IsGood())
					{
						(item.IsLowest() ? list4 : list3).Add(item.TemplateId);
					}
					else if (item.IsBad() && !item.IsHighest())
					{
						list2.Add(item.TemplateId);
					}
				}
			}
			else if (item.IsBad() && item.IsLowest())
			{
				list.Add(item.TemplateId);
			}
		}
		bool flag = true;
		if (list.Count > 0)
		{
			_featureIds.Add(list.GetRandom(random));
		}
		else if (list2.Count > 0)
		{
			short random2 = list2.GetRandom(random);
			_featureIds.Remove(random2);
			_featureIds.Add(CharacterFeature.Instance[random2].Upgrade().TemplateId);
		}
		else if (list3.Count > 0)
		{
			short random3 = list3.GetRandom(random);
			_featureIds.Remove(random3);
			_featureIds.Add(CharacterFeature.Instance[random3].Degrade().TemplateId);
		}
		else if (list4.Count > 0)
		{
			_featureIds.Remove(list4.GetRandom(random));
		}
		else
		{
			flag = false;
		}
		ObjectPool<Dictionary<short, short>>.Instance.Return(dictionary);
		ObjectPool<List<short>>.Instance.Return(list);
		ObjectPool<List<short>>.Instance.Return(list2);
		ObjectPool<List<short>>.Instance.Return(list3);
		ObjectPool<List<short>>.Instance.Return(list4);
		if (flag)
		{
			_featureIds.Sort(CharacterFeatureHelper.FeatureComparer);
		}
		return flag;
	}

	public int GetFeatureMedalValue(sbyte medalType)
	{
		return SharedMethods.CalcFeatureMedalValue(_featureIds, medalType);
	}

	public bool IsCompletelyInfected()
	{
		return _organizationInfo.OrgTemplateId == 20;
	}

	public bool HasVirginity()
	{
		return _featureIds.Contains(195);
	}

	public void LoseVirginity(DataContext context)
	{
		AddFeature(context, 196, removeMutexFeature: true);
	}

	public void GetInscribableFeatureIds(List<short> featureIds)
	{
		int i = 0;
		for (int count = _featureIds.Count; i < count; i++)
		{
			short num = _featureIds[i];
			CharacterFeatureItem characterFeatureItem = CharacterFeature.Instance[num];
			if (characterFeatureItem.Inscribable)
			{
				featureIds.Add(num);
			}
		}
	}

	public List<short> GetChickenFeatures()
	{
		return _featureIds.Where((short f) => CharacterFeature.Instance[f].IsChickenFeature).ToList();
	}

	private bool OfflineAddFeature(short featureId, bool removeMutexFeature, bool removeLowerOnly = false)
	{
		CharacterFeatureItem characterFeatureItem = CharacterFeature.Instance[featureId];
		if (characterFeatureItem.Gender != -1 && characterFeatureItem.Gender != _gender)
		{
			return false;
		}
		short mutexGroupId = characterFeatureItem.MutexGroupId;
		bool flag = false;
		int i = 0;
		for (int count = _featureIds.Count; i < count; i++)
		{
			short index = _featureIds[i];
			CharacterFeatureItem characterFeatureItem2 = CharacterFeature.Instance[index];
			if (characterFeatureItem2.MutexGroupId == mutexGroupId)
			{
				if (!removeMutexFeature)
				{
					return false;
				}
				if (removeLowerOnly && characterFeatureItem2.Level >= characterFeatureItem.Level)
				{
					return false;
				}
				_featureIds[i] = featureId;
				flag = true;
				break;
			}
		}
		if (!flag)
		{
			int num = _featureIds.BinarySearch(featureId, CharacterFeatureHelper.FeatureComparer);
			if (num < 0)
			{
				num = ~num;
			}
			if (num >= _featureIds.Count)
			{
				_featureIds.Add(featureId);
			}
			else
			{
				_featureIds.Insert(num, featureId);
			}
		}
		return true;
	}

	private void CheckForMutexFeatures()
	{
		CharacterFeature instance = CharacterFeature.Instance;
		int count = _featureIds.Count;
		HashSet<short> hashSet = new HashSet<short>(count);
		for (int i = 0; i < count; i++)
		{
			short index = _featureIds[i];
			short mutexGroupId = instance[index].MutexGroupId;
			if (!hashSet.Add(mutexGroupId))
			{
				throw new Exception($"Character {_id}: contains mutex features: {mutexGroupId}");
			}
		}
	}

	public bool IsLoseConsummateBonusByFeature()
	{
		return _featureIds.Any((short f) => CharacterFeature.Instance[f].LoseConsummateBonus);
	}

	public bool NeedHealAction(EHealActionType type)
	{
		if (1 == 0)
		{
		}
		bool result = type switch
		{
			EHealActionType.Healing => _injuries.HasAnyInjury(), 
			EHealActionType.Detox => _poisoned.IsNonZero(), 
			EHealActionType.Breathing => _disorderOfQi > 0, 
			EHealActionType.Recover => _health < GetLeftMaxHealth(), 
			_ => throw new ArgumentOutOfRangeException("type", type, null), 
		};
		if (1 == 0)
		{
		}
		return result;
	}

	public int CalcHealCostHerb(EHealActionType type, bool isExpensiveHeal = false)
	{
		if (1 == 0)
		{
		}
		int num = type switch
		{
			EHealActionType.Healing => CombatDomain.GetHealInjuryCostHerb(_injuries), 
			EHealActionType.Detox => CombatDomain.GetHealPoisonCostHerb(_poisoned), 
			EHealActionType.Breathing => CombatDomain.GetHealQiDisorderCostHerb(_disorderOfQi), 
			EHealActionType.Recover => CombatDomain.GetHealHealthCostHerb(GetHealthType()), 
			_ => throw new ArgumentOutOfRangeException("type", type, null), 
		};
		if (1 == 0)
		{
		}
		int num2 = num;
		return isExpensiveHeal ? (2 * num2) : num2;
	}

	public int CalcHealCostMoney(EHealActionType type, sbyte doctorBehaviorType, bool isExpensiveHeal = false)
	{
		if (1 == 0)
		{
		}
		int num = type switch
		{
			EHealActionType.Healing => CombatDomain.GetHealInjuryCostMoney(_injuries, doctorBehaviorType), 
			EHealActionType.Detox => CombatDomain.GetHealPoisonCostMoney(_poisoned, doctorBehaviorType), 
			EHealActionType.Breathing => CombatDomain.GetHealQiDisorderCostMoney(_disorderOfQi, doctorBehaviorType), 
			EHealActionType.Recover => CombatDomain.GetHealHealthCostMoney(GetHealthType(), doctorBehaviorType), 
			_ => throw new ArgumentOutOfRangeException("type", type, null), 
		};
		if (1 == 0)
		{
		}
		int num2 = num;
		return isExpensiveHeal ? (2 * num2) : num2;
	}

	public int CalcHealCostSpiritualDebt(EHealActionType type)
	{
		if (1 == 0)
		{
		}
		int result = type switch
		{
			EHealActionType.Healing => CombatDomain.GetHealInjuryCostSpiritualDebt(_injuries), 
			EHealActionType.Detox => CombatDomain.GetHealPoisonCostSpiritualDebt(_poisoned), 
			EHealActionType.Breathing => CombatDomain.GetHealQiDisorderCostSpiritualDebt(_disorderOfQi), 
			EHealActionType.Recover => CombatDomain.GetHealHealthCostSpiritualDebt(GetHealthType()), 
			_ => throw new ArgumentOutOfRangeException("type", type, null), 
		};
		if (1 == 0)
		{
		}
		return result;
	}

	public int CalcHealAttainment(EHealActionType type)
	{
		LifeSkillShorts lifeSkillAttainments = GetLifeSkillAttainments();
		if (1 == 0)
		{
		}
		short result = type switch
		{
			EHealActionType.Healing => lifeSkillAttainments[8], 
			EHealActionType.Detox => lifeSkillAttainments[9], 
			EHealActionType.Breathing => Math.Max(lifeSkillAttainments[8], lifeSkillAttainments[9]), 
			EHealActionType.Recover => Math.Max(lifeSkillAttainments[8], lifeSkillAttainments[9]), 
			_ => throw new ArgumentOutOfRangeException("type", type, null), 
		};
		if (1 == 0)
		{
		}
		return result;
	}

	public int CalcHealEffect(EHealActionType type, Character patient, out int maxRequireAttainment, bool isExpensiveHeal = false)
	{
		maxRequireAttainment = 0;
		int id = patient.GetId();
		int maxHealMarkCount;
		switch (type)
		{
		case EHealActionType.Healing:
		{
			DomainManager.Combat.HealInjury(id, this, out var allHealMarkCount, out maxHealMarkCount, out maxRequireAttainment, canHealOld: true, getCost: true, checkHerb: false, null, null, isExpensiveHeal);
			return allHealMarkCount;
		}
		case EHealActionType.Detox:
		{
			DomainManager.Combat.HealPoison(id, this, out maxHealMarkCount, out var healPoisonValue, out maxRequireAttainment, canHealOld: true, getCost: true, checkHerb: false, isExpensiveHeal);
			return healPoisonValue;
		}
		case EHealActionType.Breathing:
		{
			short disorderOfQi = patient.GetDisorderOfQi();
			short num2 = DomainManager.Combat.HealQiDisorder(id, this, isExpensiveHeal);
			return disorderOfQi - num2;
		}
		case EHealActionType.Recover:
		{
			short health = patient.GetHealth();
			short num = DomainManager.Combat.HealHealth(id, this, isExpensiveHeal);
			return num - health;
		}
		default:
			throw new ArgumentOutOfRangeException("type", type, null);
		}
	}

	public bool DoHealAction(DataContext context, EHealActionType type, Character patient, bool canGetProfessionSeniority = false, bool isExpensiveHeal = false)
	{
		int id = patient.GetId();
		switch (type)
		{
		case EHealActionType.Healing:
		{
			Injuries injuries = patient.GetInjuries();
			Injuries injuries2 = DomainManager.Combat.HealInjury(id, this, isExpensiveHeal);
			patient.SetInjuries(injuries2, context);
			Injuries injuries3 = injuries.Subtract(injuries2);
			if (canGetProfessionSeniority)
			{
				ProfessionFormulaItem formulaCfg3 = ProfessionFormula.Instance[85];
				int num3 = 0;
				for (sbyte b3 = 0; b3 < 7; b3++)
				{
					sbyte b4 = injuries3.Get(b3, isInnerInjury: true);
					if (b4 > 0)
					{
						num3 += formulaCfg3.Calculate(b4 - 1);
					}
					b4 = injuries3.Get(b3, isInnerInjury: false);
					if (b4 > 0)
					{
						num3 += formulaCfg3.Calculate(b4 - 1);
					}
					if (num3 > 0)
					{
						DomainManager.Extra.ChangeProfessionSeniority(context, 13, num3);
					}
				}
			}
			return injuries3.GetSum() != 0;
		}
		case EHealActionType.Detox:
		{
			PoisonInts poisoned = patient.GetPoisoned();
			PoisonInts poisoned2 = DomainManager.Combat.HealPoison(id, this, isExpensiveHeal);
			patient.SetPoisoned(ref poisoned2, context);
			if (canGetProfessionSeniority)
			{
				ProfessionFormulaItem formulaCfg2 = ProfessionFormula.Instance[86];
				for (sbyte b = 0; b < 6; b++)
				{
					int poisoned3 = poisoned[b] - poisoned2[b];
					sbyte b2 = PoisonsAndLevels.CalcPoisonedLevel(poisoned3);
					if (b2 > 0)
					{
						DomainManager.Extra.ChangeProfessionSeniority(context, 13, formulaCfg2.Calculate(b2 - 1));
					}
				}
			}
			return poisoned.Subtract(ref poisoned2).IsNonZero();
		}
		case EHealActionType.Breathing:
		{
			short disorderOfQi = patient.GetDisorderOfQi();
			short num4 = DomainManager.Combat.HealQiDisorder(id, this, isExpensiveHeal);
			patient.SetDisorderOfQi(num4, context);
			int num5 = disorderOfQi - num4;
			if (canGetProfessionSeniority)
			{
				ProfessionFormulaItem formulaCfg4 = ProfessionFormula.Instance[87];
				int baseDelta2 = formulaCfg4.Calculate(num5);
				DomainManager.Extra.ChangeProfessionSeniority(context, 13, baseDelta2);
			}
			return num5 > 0;
		}
		case EHealActionType.Recover:
		{
			short health = patient.GetHealth();
			short num = DomainManager.Combat.HealHealth(id, this, isExpensiveHeal);
			patient.SetHealth(num, context);
			int num2 = num - health;
			if (canGetProfessionSeniority)
			{
				ProfessionFormulaItem formulaCfg = ProfessionFormula.Instance[88];
				int baseDelta = formulaCfg.Calculate(num2);
				DomainManager.Extra.ChangeProfessionSeniority(context, 13, baseDelta);
			}
			return num2 > 0;
		}
		default:
			throw new ArgumentOutOfRangeException("type", type, null);
		}
	}

	public short GetClothingDisplayId()
	{
		if (DomainManager.Extra.IsLegendaryBookConsumed(_id))
		{
			return 10003;
		}
		ItemKey itemKey = _equipment[4];
		if (itemKey.IsValid() && !DomainManager.Item.GetBaseItem(itemKey).IsDurabilityRunningOut())
		{
			return DomainManager.Extra.GetActualClothingDisplayId(itemKey);
		}
		return (short)((DomainManager.Character.GetSkeletonSourceGraveId(_id) >= 0) ? 20003 : 0);
	}

	private short GetClothingDisplayIdForCharm()
	{
		if (DomainManager.Extra.IsLegendaryBookConsumed(_id))
		{
			return 10003;
		}
		ItemKey itemKey = _equipment[4];
		if (itemKey.IsValid() && !DomainManager.Item.GetBaseItem(itemKey).IsDurabilityRunningOut())
		{
			return Config.Clothing.Instance[itemKey.TemplateId].DisplayId;
		}
		return (short)((DomainManager.Character.GetSkeletonSourceGraveId(_id) >= 0) ? 20003 : 0);
	}

	public void ChangeEquipment(DataContext context, sbyte srcSlot, sbyte destSlot, ItemKey srcItemKey)
	{
		if (srcSlot >= 0)
		{
			srcItemKey = _equipment[srcSlot];
			_equipment[srcSlot] = ItemKey.Invalid;
		}
		else
		{
			_inventory.OfflineRemove(srcItemKey, 1);
		}
		EquipmentBase baseEquipment = DomainManager.Item.GetBaseEquipment(srcItemKey);
		baseEquipment.ResetOwner();
		ItemKey itemKey;
		if (destSlot >= 0)
		{
			itemKey = _equipment[destSlot];
			_equipment[destSlot] = srcItemKey;
			baseEquipment.SetEquippedCharId(_id, context);
			baseEquipment.SetOwner(ItemOwnerType.CharacterEquipment, _id);
		}
		else
		{
			itemKey = ItemKey.Invalid;
			_inventory.OfflineAdd(srcItemKey, 1);
			baseEquipment.SetEquippedCharId(-1, context);
			baseEquipment.SetOwner(ItemOwnerType.CharacterInventory, _id);
		}
		if (itemKey.IsValid())
		{
			EquipmentBase baseEquipment2 = DomainManager.Item.GetBaseEquipment(itemKey);
			if (srcSlot >= 0)
			{
				_equipment[srcSlot] = itemKey;
				baseEquipment2.SetOwner(ItemOwnerType.CharacterEquipment, _id);
			}
			else
			{
				_inventory.OfflineAdd(itemKey, 1);
				baseEquipment2.SetEquippedCharId(-1, context);
				baseEquipment2.SetOwner(ItemOwnerType.CharacterInventory, _id);
			}
		}
		SetEquipment(_equipment, context);
		SetInventory(_inventory, context);
		if (_id == DomainManager.Taiwu.GetTaiwuCharId())
		{
			DomainManager.Taiwu.UpdateTaiwuWeaponInnerRatios(context, srcSlot, destSlot, srcItemKey);
		}
	}

	public void ChangeEquipment(DataContext context, ItemKey[] equipment)
	{
		for (int i = 0; i < 12; i++)
		{
			ItemKey itemKey = _equipment[i];
			if (itemKey.IsValid())
			{
				EquipmentBase baseEquipment = DomainManager.Item.GetBaseEquipment(itemKey);
				baseEquipment.RemoveOwner(ItemOwnerType.CharacterEquipment, _id);
				baseEquipment.SetOwner(ItemOwnerType.CharacterInventory, _id);
				baseEquipment.SetEquippedCharId(-1, context);
				_inventory.OfflineAdd(itemKey, 1);
			}
		}
		for (int j = 0; j < 12; j++)
		{
			ItemKey itemKey2 = equipment[j];
			if (itemKey2.IsValid())
			{
				EquipmentBase baseEquipment2 = DomainManager.Item.GetBaseEquipment(itemKey2);
				baseEquipment2.RemoveOwner(ItemOwnerType.CharacterInventory, _id);
				baseEquipment2.SetOwner(ItemOwnerType.CharacterEquipment, _id);
				baseEquipment2.SetEquippedCharId(_id, context);
				_inventory.OfflineRemove(itemKey2, 1);
			}
		}
		for (int k = 0; k < 12; k++)
		{
			_equipment[k] = equipment[k];
		}
		SetEquipment(_equipment, context);
		SetInventory(_inventory, context);
	}

	public int GetEquipmentCombatPowerValue()
	{
		return _equipment.Zip(GlobalConfig.EquipmentSlotCombatPower, (ItemKey key, int value) => ItemTemplateHelper.GetBaseCombatPowerValue(key.ItemType, key.TemplateId) * value / 100).Sum();
	}

	public void ForceReplaceClothing(DataContext context, short newClothingTemplateId)
	{
		ItemKey itemKey = _equipment[4];
		if (itemKey.IsValid())
		{
			DomainManager.Item.RemoveItem(context, itemKey);
		}
		if (newClothingTemplateId >= 0)
		{
			_equipment[4] = DomainManager.Item.CreateClothing(context, newClothingTemplateId, _gender);
			EquipmentBase baseEquipment = DomainManager.Item.GetBaseEquipment(_equipment[4]);
			baseEquipment.SetEquippedCharId(_id, context);
			baseEquipment.SetOwner(ItemOwnerType.CharacterEquipment, _id);
		}
		else
		{
			_equipment[4] = ItemKey.Invalid;
		}
		SetEquipment(_equipment, context);
	}

	public void CreateInventoryItem(DataContext context, sbyte itemType, short templateId, int amount)
	{
		OfflineCreateInventoryItem(context, itemType, templateId, amount);
		SetInventory(_inventory, context);
		if (itemType == 12 && Config.Misc.Instance[templateId].ItemSubType == 1202)
		{
			int num = templateId - 211;
			DomainManager.LegendaryBook.RegisterOwner(context, this, (sbyte)num);
		}
	}

	public void AddInventoryItem(DataContext context, ItemKey itemKey, int amount, bool offLine = false)
	{
		_inventory.OfflineAdd(itemKey, amount);
		if (!offLine)
		{
			SetInventory(_inventory, context);
		}
		if (itemKey.ItemType == 12 && Config.Misc.Instance[itemKey.TemplateId].ItemSubType == 1202)
		{
			int num = itemKey.TemplateId - 211;
			DomainManager.LegendaryBook.RegisterOwner(context, this, (sbyte)num);
			ItemBase baseItem = DomainManager.Item.GetBaseItem(itemKey);
			if (baseItem.Owner.OwnerType == ItemOwnerType.System)
			{
				baseItem.RemoveOwner(ItemOwnerType.System, 11);
			}
		}
		DomainManager.Item.SetOwner(itemKey, ItemOwnerType.CharacterInventory, _id);
		if (_id == DomainManager.Taiwu.GetTaiwuCharId() && ItemType.IsEquipmentItemType(itemKey.ItemType))
		{
			EquipmentBase baseEquipment = DomainManager.Item.GetBaseEquipment(itemKey);
			baseEquipment.SetModificationState(baseEquipment.GetModificationState(), context);
			if (itemKey.ItemType == 3)
			{
				DomainManager.Extra.RecordOwnedClothing(context, itemKey.TemplateId);
			}
		}
	}

	public void AddInventoryItemList(DataContext context, List<ItemKey> keyList)
	{
		Tester.Assert(keyList != null);
		Tester.Assert(keyList.Count > 0);
		foreach (ItemKey key in keyList)
		{
			AddInventoryItem(context, key, 1, offLine: true);
		}
		SetInventory(_inventory, context);
	}

	public void RemoveMultiInventoryItem(DataContext context, sbyte itemType, short templateId, int count)
	{
		if (count <= 0)
		{
			return;
		}
		List<ItemKey> list = ObjectPool<List<ItemKey>>.Instance.Get();
		while (count > 0)
		{
			ItemKey inventoryItemKey = _inventory.GetInventoryItemKey(itemType, templateId);
			int num = Math.Min(count, _inventory.Items[inventoryItemKey]);
			count -= num;
			_inventory.OfflineRemove(inventoryItemKey, num);
			if (!_inventory.Items.ContainsKey(inventoryItemKey))
			{
				list.Add(inventoryItemKey);
			}
			Events.RaiseItemRemovedFromInventory(context, this, inventoryItemKey, num);
		}
		SetInventory(_inventory, context);
		foreach (ItemKey item in list)
		{
			if (_id == DomainManager.Taiwu.GetTaiwuCharId())
			{
				DomainManager.Character.ClearItemUsingState(context, item, _id);
			}
			DomainManager.Item.RemoveItem(context, item);
		}
		ObjectPool<List<ItemKey>>.Instance.Return(list);
	}

	public void RemoveInventoryItem(DataContext context, ItemKey itemKey, int amount, bool deleteItem, bool offLine = false)
	{
		if (amount > 0)
		{
			if (_id == DomainManager.Taiwu.GetTaiwuCharId())
			{
				DomainManager.Character.ClearItemUsingState(context, itemKey, _id);
			}
			_inventory.OfflineRemove(itemKey, amount);
			Events.RaiseItemRemovedFromInventory(context, this, itemKey, amount);
			if (!offLine)
			{
				SetInventory(_inventory, context);
			}
			if (deleteItem)
			{
				DomainManager.Item.RemoveItem(context, itemKey);
			}
		}
	}

	public void RemoveInventoryItemList(DataContext context, List<ItemKey> keyList, bool deleteItem)
	{
		Tester.Assert(keyList != null);
		Tester.Assert(keyList.Count > 0);
		foreach (ItemKey key in keyList)
		{
			RemoveInventoryItem(context, key, 1, deleteItem, offLine: true);
		}
		SetInventory(_inventory, context);
	}

	public bool TryDetectAttachedPoisons(ItemKey itemKey)
	{
		if (!ModificationStateHelper.IsActive(itemKey.ModificationState, 1))
		{
			return false;
		}
		PoisonsAndLevels attachedPoisons = DomainManager.Item.GetAttachedPoisons(itemKey);
		short lifeSkillAttainment = GetLifeSkillAttainment(9);
		for (sbyte b = 0; b < 6; b++)
		{
			sbyte grade = attachedPoisons.GetGrade(b);
			if (grade >= 0)
			{
				short num = GlobalConfig.Instance.PoisonAttainments[grade];
				if (lifeSkillAttainment >= num)
				{
					return true;
				}
			}
		}
		return false;
	}

	public (ItemKey, bool KeyChanged) AttachPoisonsToInventoryItem(DataContext context, ItemKey targetItemKey, ItemKey[] poisonsToAdd)
	{
		bool flag = false;
		ItemBase itemBase = DomainManager.Item.GetBaseItem(targetItemKey);
		for (int i = 0; i < poisonsToAdd.Length; i++)
		{
			ItemKey itemKey = poisonsToAdd[i];
			if (itemKey.IsValid() && _inventory.Items.ContainsKey(itemKey))
			{
				var (itemBase2, flag2) = DomainManager.Item.SetAttachedPoisons(context, itemBase, itemKey.TemplateId, add: true);
				RemoveInventoryItem(context, itemKey, 1, deleteItem: true);
				if (flag2)
				{
					flag = true;
					itemBase = itemBase2;
				}
			}
		}
		if (flag)
		{
			ItemKey itemKey2 = itemBase.GetItemKey();
			_inventory.OfflineRemove(targetItemKey, 1);
			_inventory.OfflineAdd(itemKey2, 1);
			DomainManager.Item.SetOwner(itemKey2, ItemOwnerType.CharacterInventory, _id);
			SetInventory(_inventory, context);
			if (itemKey2.Id != targetItemKey.Id)
			{
				DomainManager.Item.RemoveItem(context, targetItemKey);
			}
		}
		return (itemBase.GetItemKey(), KeyChanged: flag);
	}

	private ItemKey SelectInventoryPoisonToAdd(sbyte requiredType)
	{
		short lifeSkillAttainment = GetLifeSkillAttainment(9);
		int num = 16129;
		ItemKey result = ItemKey.Invalid;
		foreach (ItemKey key in _inventory.Items.Keys)
		{
			if (key.ItemType != 8)
			{
				continue;
			}
			MedicineItem medicineItem = Config.Medicine.Instance[key.TemplateId];
			if (medicineItem.EffectType != EMedicineEffectType.ApplyPoison)
			{
				continue;
			}
			sbyte poisonType = medicineItem.PoisonType;
			short num2 = GlobalConfig.Instance.PoisonAttainments[medicineItem.Grade];
			if (poisonType == requiredType && lifeSkillAttainment >= num2)
			{
				int num3 = medicineItem.Grade - _organizationInfo.Grade;
				int num4 = num3 * num3;
				if (num4 <= num)
				{
					num = num4;
					result = key;
				}
			}
		}
		return result;
	}

	private ItemKey[] SelectInventoryPoisonsToAdd(IRandomSource random, ItemKey itemToAttachPoisonOn)
	{
		if (_id == DomainManager.Taiwu.GetTaiwuCharId())
		{
			return null;
		}
		Span<ItemKey> span = stackalloc ItemKey[6];
		SpanList<ItemKey> selectedPoisons = span;
		short lifeSkillAttainment = GetLifeSkillAttainment(9);
		_inventory.SelectPoisonsToAdd(random, lifeSkillAttainment, _organizationInfo.Grade, itemToAttachPoisonOn, ref selectedPoisons);
		return selectedPoisons.ToArray();
	}

	public void FindItems(Predicate<ItemBase> predicate, List<(ItemKey itemKey, int amount)> items, bool searchInventory, bool searchEquipment)
	{
		if (searchInventory)
		{
			foreach (KeyValuePair<ItemKey, int> item2 in _inventory.Items)
			{
				item2.Deconstruct(out var key, out var value);
				ItemKey itemKey = key;
				int item = value;
				ItemBase baseItem = DomainManager.Item.GetBaseItem(itemKey);
				if (predicate(baseItem))
				{
					items.Add((itemKey, item));
				}
			}
		}
		if (!searchEquipment)
		{
			return;
		}
		for (int i = 0; i < 12; i++)
		{
			ItemKey itemKey2 = _equipment[i];
			if (itemKey2.IsValid())
			{
				ItemBase baseItem2 = DomainManager.Item.GetBaseItem(itemKey2);
				if (predicate(baseItem2))
				{
					items.Add((itemKey2, 1));
				}
			}
		}
	}

	public void FindItems(List<Predicate<ItemBase>> predicates, List<(ItemKey itemKey, int amount)> items, bool searchInventory, bool searchEquipment)
	{
		if (searchInventory)
		{
			foreach (KeyValuePair<ItemKey, int> item2 in _inventory.Items)
			{
				item2.Deconstruct(out var key, out var value);
				ItemKey itemKey = key;
				int item = value;
				ItemBase baseItem = DomainManager.Item.GetBaseItem(itemKey);
				if (ItemMatchers.MatchAll(baseItem, predicates))
				{
					items.Add((itemKey, item));
				}
			}
		}
		if (!searchEquipment)
		{
			return;
		}
		for (int i = 0; i < 12; i++)
		{
			ItemKey itemKey2 = _equipment[i];
			if (itemKey2.IsValid())
			{
				ItemBase baseItem2 = DomainManager.Item.GetBaseItem(itemKey2);
				if (ItemMatchers.MatchAll(baseItem2, predicates))
				{
					items.Add((itemKey2, 1));
				}
			}
		}
	}

	public bool HasReadableBook()
	{
		foreach (var (itemKey2, _) in GetInventory().Items)
		{
			if (itemKey2.ItemType != 10 || !BookIsReadable(itemKey2))
			{
				continue;
			}
			return true;
		}
		return false;
	}

	public void GetReadableBookList(List<ItemKey> list)
	{
		list.Clear();
		foreach (var (itemKey2, _) in GetInventory().Items)
		{
			if (itemKey2.ItemType == 10 && BookIsReadable(itemKey2))
			{
				list.Add(itemKey2);
			}
		}
	}

	public bool BookIsReadable(ItemKey itemKey)
	{
		if (!DomainManager.Item.TryGetElement_SkillBooks(itemKey.Id, out var element))
		{
			return false;
		}
		if (TryDetectAttachedPoisons(itemKey))
		{
			return false;
		}
		byte item;
		if (element.IsCombatSkillBook())
		{
			item = GetCombatSkillBookCurrReadingInfo(element).readingPage;
			if (item == 6)
			{
				return false;
			}
		}
		else
		{
			item = GetLifeSkillBookCurrReadingInfo(element).readingPage;
			if (item == 5)
			{
				return false;
			}
		}
		if (SkillBookStateHelper.GetPageIncompleteState(element.GetPageIncompleteState(), item) == 2)
		{
			return false;
		}
		return true;
	}

	public bool IsBookRead(ItemKey itemKey)
	{
		if (!DomainManager.Item.TryGetElement_SkillBooks(itemKey.Id, out var element))
		{
			return false;
		}
		if (element.IsCombatSkillBook())
		{
			byte item = GetCombatSkillBookCurrReadingInfo(element).readingPage;
			if (item == 6)
			{
				return true;
			}
		}
		else
		{
			byte item = GetLifeSkillBookCurrReadingInfo(element).readingPage;
			if (item == 5)
			{
				return true;
			}
		}
		return false;
	}

	public void VillagerReturnNotReadableBooks(DataContext context)
	{
		short taiwuVillageSettlementId = DomainManager.Taiwu.GetTaiwuVillageSettlementId();
		if (_organizationInfo.SettlementId != taiwuVillageSettlementId)
		{
			return;
		}
		IReadOnlyDictionary<ItemKey, int> readOnlyDictionary = (DomainManager.Taiwu.IsInGroup(_id) ? DomainManager.Extra.GetTaiwuGiftItems(_id) : Inventory.Empty);
		List<ItemKey> list = ObjectPool<List<ItemKey>>.Instance.Get();
		foreach (var (itemKey2, _) in _inventory.Items)
		{
			if (itemKey2.ItemType == 10 && !readOnlyDictionary.ContainsKey(itemKey2) && !BookIsReadable(itemKey2))
			{
				list.Add(itemKey2);
			}
		}
		LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
		int currDate = DomainManager.World.GetCurrDate();
		using (List<ItemKey>.Enumerator enumerator2 = list.GetEnumerator())
		{
			if (enumerator2.MoveNext())
			{
				ItemKey current = enumerator2.Current;
				RemoveInventoryItem(context, current, 1, deleteItem: false);
				if (IsBookRead(current))
				{
					DomainManager.Taiwu.VillagerStoreItemInTreasury(context, this, current, 1, addLifeSkillRecord: false);
					lifeRecordCollection.AddTaiwuVillagerFinishedReading(_id, currDate, current.ItemType, current.TemplateId);
				}
				else
				{
					DomainManager.Taiwu.VillagerStoreItemInTreasury(context, this, current, 1);
				}
			}
		}
		ObjectPool<List<ItemKey>>.Instance.Return(list);
	}

	public void ReturnVillagerRoleClothing(DataContext context, bool forceReturn = true)
	{
		short idealClothingTemplateId = GetIdealClothingTemplateId();
		bool flag = false;
		if (forceReturn)
		{
			ItemKey equipment = GetEquipment()[4];
			if (equipment.IsValid())
			{
				bool flag2 = VillagerRole.Instance.Any((VillagerRoleItem r) => r.Clothing == equipment.TemplateId);
				bool flag3 = idealClothingTemplateId == equipment.TemplateId;
				if (flag2 && !flag3)
				{
					ChangeEquipment(context, 4, -1, equipment);
					flag = true;
				}
			}
		}
		else
		{
			EventHelper.SelectAndEquipClothing(_id);
		}
		IReadOnlyDictionary<ItemKey, int> readOnlyDictionary = (DomainManager.Taiwu.IsInGroup(_id) ? DomainManager.Extra.GetTaiwuGiftItems(_id) : Inventory.Empty);
		List<ItemKey> list = ObjectPool<List<ItemKey>>.Instance.Get();
		foreach (KeyValuePair<ItemKey, int> item in _inventory.Items)
		{
			var (itemKey2, _) = (KeyValuePair<ItemKey, int>)(ref item);
			if (itemKey2.ItemType == 3 && !readOnlyDictionary.ContainsKey(itemKey2))
			{
				bool flag4 = VillagerRole.Instance.Any((VillagerRoleItem r) => r.Clothing == itemKey2.TemplateId);
				bool flag5 = idealClothingTemplateId == itemKey2.TemplateId;
				if (flag4 && !flag5)
				{
					list.Add(itemKey2);
				}
			}
		}
		foreach (ItemKey item2 in list)
		{
			RemoveInventoryItem(context, item2, 1, deleteItem: false);
			DomainManager.Taiwu.VillagerStoreItemInTreasury(context, this, item2, 1);
		}
		if (flag)
		{
			EventHelper.SelectAndEquipClothing(_id);
		}
		ObjectPool<List<ItemKey>>.Instance.Return(list);
	}

	public unsafe ResourceInts GetResourcesAboveSatisfyingThreshold()
	{
		OrganizationMemberItem orgMemberConfig = OrganizationDomain.GetOrgMemberConfig(_organizationInfo);
		ResourceInts result = default(ResourceInts);
		for (sbyte b = 0; b < 8; b++)
		{
			int adjustedResourceSatisfyingAmount = orgMemberConfig.GetAdjustedResourceSatisfyingAmount(b);
			result.Items[b] = Math.Max(_resources.Items[b] - adjustedResourceSatisfyingAmount, 0);
		}
		return result;
	}

	public ItemKey GetMaxGradeItemToLose(IRandomSource random)
	{
		List<ItemKey> list = ObjectPool<List<ItemKey>>.Instance.Get();
		list.Clear();
		sbyte currMaxGrade = -1;
		bool flag = GetAgeGroup() != 2;
		for (int i = 0; i < _equipment.Length; i++)
		{
			ItemKey itemKey = _equipment[i];
			if (itemKey.IsValid() && (flag || i != 4))
			{
				currMaxGrade = AddItemToLose(itemKey, currMaxGrade, list);
			}
		}
		foreach (ItemKey key in _inventory.Items.Keys)
		{
			if (ItemDomain.CanItemBeLost(key))
			{
				currMaxGrade = AddItemToLose(key, currMaxGrade, list);
			}
		}
		ItemKey result = ((list.Count > 0) ? list.GetRandom(random) : ItemKey.Invalid);
		ObjectPool<List<ItemKey>>.Instance.Return(list);
		return result;
		static sbyte AddItemToLose(ItemKey item, sbyte b, List<ItemKey> itemsToLose)
		{
			sbyte grade = ItemTemplateHelper.GetGrade(item.ItemType, item.TemplateId);
			if (grade < b)
			{
				return b;
			}
			if (grade > b)
			{
				b = grade;
				itemsToLose.Clear();
			}
			itemsToLose.Add(item);
			return b;
		}
	}

	public void GetItemsToLose(List<ItemKey> result, sbyte minGrade = 0, sbyte maxGrade = 8)
	{
		result.Clear();
		bool flag = GetAgeGroup() == 2;
		for (int i = 0; i < _equipment.Length; i++)
		{
			ItemKey item = _equipment[i];
			if (item.IsValid() && (flag || i != 4))
			{
				sbyte grade = ItemTemplateHelper.GetGrade(item.ItemType, item.TemplateId);
				if (grade >= minGrade && grade <= maxGrade)
				{
					result.Add(item);
				}
			}
		}
		foreach (ItemKey key in _inventory.Items.Keys)
		{
			if (ItemDomain.CanItemBeLost(key))
			{
				sbyte grade2 = ItemTemplateHelper.GetGrade(key.ItemType, key.TemplateId);
				if (grade2 >= minGrade && grade2 <= maxGrade)
				{
					result.Add(key);
				}
			}
		}
	}

	private void OfflineCreateInventoryOnCharacterCreation(DataContext context, sbyte itemType, short templateId, int amount)
	{
		if (amount <= 1 || ItemTemplateHelper.IsStackable(itemType, templateId))
		{
			ItemKey itemKey = DomainManager.Item.CreateItem(context, itemType, templateId);
			_inventory.OfflineAdd(itemKey, amount);
			return;
		}
		for (int i = 0; i < amount; i++)
		{
			ItemKey key = DomainManager.Item.CreateItem(context, itemType, templateId);
			_inventory.Items.Add(key, 1);
		}
	}

	private void OfflineCreateInventoryItem(DataContext context, sbyte itemType, short templateId, int amount)
	{
		if (amount <= 1 || ItemTemplateHelper.IsStackable(itemType, templateId))
		{
			ItemKey itemKey = DomainManager.Item.CreateItem(context, itemType, templateId);
			DomainManager.Item.SetOwner(itemKey, ItemOwnerType.CharacterInventory, _id);
			_inventory.OfflineAdd(itemKey, amount);
			return;
		}
		for (int i = 0; i < amount; i++)
		{
			ItemKey itemKey2 = DomainManager.Item.CreateItem(context, itemType, templateId);
			DomainManager.Item.SetOwner(itemKey2, ItemOwnerType.CharacterInventory, _id);
			_inventory.Items.Add(itemKey2, 1);
		}
	}

	public ItemKey GetInventoryRope(DataContext context, sbyte grade)
	{
		if (context.Random.CheckPercentProb(50))
		{
			foreach (var (result, _) in _inventory.Items)
			{
				if (result.ItemType == 12 && ItemTemplateHelper.GetItemSubType(result.ItemType, result.TemplateId) == 1206 && ItemTemplateHelper.GetGrade(result.ItemType, result.TemplateId) >= grade)
				{
					return result;
				}
			}
		}
		sbyte b = ItemDomain.GenerateRandomItemGrade(context.Random, grade);
		ItemKey itemKey2 = DomainManager.Item.CreateMisc(context, (short)(73 + b));
		AddInventoryItem(context, itemKey2, 1);
		return itemKey2;
	}

	public int GetKidnapMaxSlotCount()
	{
		ItemKey itemKey = _equipment[11];
		if (!itemKey.IsValid())
		{
			return GlobalConfig.Instance.KidnapSlotBaseMaxCount;
		}
		return GlobalConfig.Instance.KidnapSlotBaseMaxCount + DomainManager.Item.GetElement_Carriers(itemKey.Id).GetMaxKidnapSlotCountBonus();
	}

	public LifeSkillItem LearnNewLifeSkill(DataContext context, short lifeSkillTemplateId, byte readingState)
	{
		LifeSkillItem lifeSkillItem = new LifeSkillItem(lifeSkillTemplateId);
		lifeSkillItem.ReadingState = readingState;
		_learnedLifeSkills.Add(lifeSkillItem);
		SetLearnedLifeSkills(_learnedLifeSkills, context);
		if (lifeSkillItem.IsAllPagesRead())
		{
			DomainManager.Information.GainLifeSkillInformationToCharacter(context, _id, LifeSkill.Instance[lifeSkillTemplateId].Type);
		}
		if (_id == DomainManager.Taiwu.GetTaiwuCharId())
		{
			DomainManager.Taiwu.RegisterLifeSkill(context, lifeSkillItem);
			DomainManager.Taiwu.AddLegacyPoint(context, 14);
		}
		return lifeSkillItem;
	}

	public void UpdateLifeSkillReadingState(DataContext context, int learnedSkillIndex, byte readingState)
	{
		LifeSkillItem value = _learnedLifeSkills[learnedSkillIndex];
		bool flag = value.IsAllPagesRead();
		value.ReadingState = readingState;
		_learnedLifeSkills[learnedSkillIndex] = value;
		SetLearnedLifeSkills(_learnedLifeSkills, context);
		sbyte type = LifeSkill.Instance[value.SkillTemplateId].Type;
		if (!flag && value.IsAllPagesRead())
		{
			DomainManager.Information.GainLifeSkillInformationToCharacter(context, _id, type);
		}
	}

	public void ReadLifeSkillPage(DataContext context, int learnedSkillIndex, byte pageId)
	{
		LifeSkillItem value = _learnedLifeSkills[learnedSkillIndex];
		bool flag = value.IsAllPagesRead();
		value.SetPageRead(pageId);
		_learnedLifeSkills[learnedSkillIndex] = value;
		SetLearnedLifeSkills(_learnedLifeSkills, context);
		sbyte type = LifeSkill.Instance[value.SkillTemplateId].Type;
		if (!flag && value.IsAllPagesRead())
		{
			DomainManager.Information.GainLifeSkillInformationToCharacter(context, _id, type);
		}
	}

	public unsafe short GetLifeSkillAttainment(sbyte lifeSkillType)
	{
		return GetLifeSkillAttainments().Items[lifeSkillType];
	}

	public unsafe short GetLifeSkillQualification(sbyte lifeSkillType)
	{
		return GetLifeSkillQualifications().Items[lifeSkillType];
	}

	public unsafe short GetMaxLifeSkillAttainment()
	{
		ref LifeSkillShorts lifeSkillAttainments = ref GetLifeSkillAttainments();
		short num = 0;
		for (int i = 0; i < 16; i++)
		{
			if (lifeSkillAttainments.Items[i] > num)
			{
				num = lifeSkillAttainments.Items[i];
			}
		}
		return num;
	}

	public short GetMaxCombatSkillAttainment()
	{
		return GetCombatSkillAttainments().GetMaxCombatSkillValue();
	}

	public unsafe (sbyte, short) GetMaxCombatSkillAttainmentType()
	{
		ref CombatSkillShorts combatSkillAttainments = ref GetCombatSkillAttainments();
		sbyte maxCombatSkillType = combatSkillAttainments.GetMaxCombatSkillType();
		return (maxCombatSkillType, combatSkillAttainments.Items[maxCombatSkillType]);
	}

	public int FindLearnedLifeSkillIndex(short skillTemplateId)
	{
		for (int i = 0; i < _learnedLifeSkills.Count; i++)
		{
			if (_learnedLifeSkills[i].SkillTemplateId == skillTemplateId)
			{
				return i;
			}
		}
		return -1;
	}

	public sbyte GetMaxLifeSkillAttainmentType(DataContext context)
	{
		return GetLifeSkillAttainments().GetMaxLifeSkillType(context.Random);
	}

	private unsafe LifeSkillShorts GetPredictLifeSkillAttainments(short skillTemplateId, int count)
	{
		LifeSkillShorts value = default(LifeSkillShorts);
		value.Initialize();
		GetLifeSkillBaseAttainment(ref value);
		Config.LifeSkillItem lifeSkillItem = LifeSkill.Instance[skillTemplateId];
		int num = GlobalConfig.Instance.AddAttainmentPerGrade[lifeSkillItem.Grade] / 5 * count;
		ref short reference = ref value.Items[lifeSkillItem.Type];
		reference += (short)num;
		GetLifeSkillAttainmentAddOns(ref value);
		return value;
	}

	public unsafe short GetPredictLifeSkillAttainment(short type, short skillTemplateId, int count)
	{
		LifeSkillShorts predictLifeSkillAttainments = GetPredictLifeSkillAttainments(skillTemplateId, count);
		return predictLifeSkillAttainments.Items[type];
	}

	public unsafe void GetLifeSkillBaseAttainment(ref LifeSkillShorts value)
	{
		int i = 0;
		for (int count = _learnedLifeSkills.Count; i < count; i++)
		{
			LifeSkillItem lifeSkillItem = _learnedLifeSkills[i];
			Config.LifeSkillItem lifeSkillItem2 = LifeSkill.Instance[lifeSkillItem.SkillTemplateId];
			int readPagesCount = lifeSkillItem.GetReadPagesCount();
			int num = GlobalConfig.Instance.AddAttainmentPerGrade[lifeSkillItem2.Grade] / 5 * readPagesCount;
			ref short reference = ref value.Items[lifeSkillItem2.Type];
			reference += (short)num;
		}
	}

	public sbyte GetLearnedLifeSkillMaxGradeByType(sbyte lifeSkillType)
	{
		sbyte b = 0;
		int i = 0;
		for (int count = _learnedLifeSkills.Count; i < count; i++)
		{
			LifeSkillItem lifeSkillItem = _learnedLifeSkills[i];
			Config.LifeSkillItem lifeSkillItem2 = LifeSkill.Instance[lifeSkillItem.SkillTemplateId];
			if (lifeSkillItem2.Type == lifeSkillType && lifeSkillItem2.Grade > b)
			{
				b = lifeSkillItem2.Grade;
			}
		}
		return b;
	}

	public int GetLearnedLifeSkillTotalValue(sbyte lifeSkillType)
	{
		int num = 0;
		int i = 0;
		for (int count = _learnedLifeSkills.Count; i < count; i++)
		{
			LifeSkillItem lifeSkillItem = _learnedLifeSkills[i];
			Config.LifeSkillItem lifeSkillItem2 = LifeSkill.Instance[lifeSkillItem.SkillTemplateId];
			if (lifeSkillItem2.Type == lifeSkillType)
			{
				int baseValue = ItemTemplateHelper.GetBaseValue(10, lifeSkillItem2.SkillBookId);
				int num2 = baseValue * lifeSkillItem.GetReadPagesCount() / 5;
				num += num2;
			}
		}
		return num;
	}

	public unsafe void GetLifeSkillAttainmentAddOns(ref LifeSkillShorts value)
	{
		ref LifeSkillShorts lifeSkillQualifications = ref GetLifeSkillQualifications();
		for (int i = 0; i < 16; i++)
		{
			int num = value.Items[i];
			value.Items[i] = (short)(lifeSkillQualifications.Items[i] * (100 + num) / 100 + num);
		}
		IBuildingEffectValue buildingBlockEffectObject = DomainManager.Building.GetBuildingBlockEffectObject(_location, EBuildingScaleEffect.LifeSkillAttainment);
		if (buildingBlockEffectObject != null)
		{
			for (int j = 0; j < 16; j++)
			{
				value[j] += (short)buildingBlockEffectObject.Get(j);
			}
		}
		for (int k = 0; k < 16; k++)
		{
			if (value.Items[k] < 0)
			{
				value.Items[k] = 0;
			}
		}
	}

	public List<short> GetUnlockedDebateStrategyList()
	{
		List<short> list = new List<short>();
		List<LifeSkillItem> learnedLifeSkills = GetLearnedLifeSkills();
		foreach (LifeSkillItem item in learnedLifeSkills)
		{
			if (CheckLearnedBookHasUnlockedDebateStrategy(item, out var strategyTemplateId, out var _) && !list.Contains(strategyTemplateId))
			{
				list.Add(strategyTemplateId);
			}
		}
		list.Sort();
		return list;
	}

	public Dictionary<sbyte, int> GetHasUnlockedDebateStrategyLearnedBookCountDict(int strategyLevel)
	{
		Dictionary<sbyte, int> dictionary = new Dictionary<sbyte, int>();
		List<LifeSkillItem> learnedLifeSkills = GetLearnedLifeSkills();
		foreach (LifeSkillItem item in learnedLifeSkills)
		{
			if (CheckLearnedBookHasUnlockedDebateStrategy(item, out var strategyTemplateId, out var lifeSkillType) && DebateStrategy.Instance[strategyTemplateId].Level == strategyLevel)
			{
				dictionary.TryGetValue(lifeSkillType, out var value);
				value = (dictionary[lifeSkillType] = value + 1);
			}
		}
		return dictionary;
	}

	public short GetUnlockedDebateStrategy(LifeSkillItem learnedLifeSkill, Dictionary<sbyte, int> oldUnlockCountDict)
	{
		if (!CheckLearnedBookHasUnlockedDebateStrategy(learnedLifeSkill, out var strategyTemplateId, out var lifeSkillType))
		{
			return -1;
		}
		DebateStrategyItem debateStrategyItem = DebateStrategy.Instance[strategyTemplateId];
		Dictionary<sbyte, int> hasUnlockedDebateStrategyLearnedBookCountDict = GetHasUnlockedDebateStrategyLearnedBookCountDict(debateStrategyItem.Level);
		oldUnlockCountDict.TryGetValue(lifeSkillType, out var value);
		hasUnlockedDebateStrategyLearnedBookCountDict.TryGetValue(lifeSkillType, out var value2);
		if (value == 0 && value2 == 1)
		{
			return strategyTemplateId;
		}
		return -1;
	}

	public bool CheckLearnedBookHasUnlockedDebateStrategy(LifeSkillItem learnedLifeSkill, out short strategyTemplateId, out sbyte lifeSkillType)
	{
		lifeSkillType = -1;
		strategyTemplateId = -1;
		Config.LifeSkillItem lifeSkillItem = LifeSkill.Instance[learnedLifeSkill.SkillTemplateId];
		SkillBookItem bookConfig = Config.SkillBook.Instance[lifeSkillItem.SkillBookId];
		sbyte grade = bookConfig.Grade;
		if (1 == 0)
		{
		}
		int num;
		switch (grade)
		{
		case 0:
		case 1:
		case 2:
			num = 1;
			break;
		case 3:
		case 4:
		case 5:
			num = 2;
			break;
		case 6:
		case 7:
		case 8:
			num = 3;
			break;
		default:
			throw new ArgumentOutOfRangeException();
		}
		if (1 == 0)
		{
		}
		int strategyLevel = num;
		strategyTemplateId = DebateStrategy.Instance.FirstOrDefault((DebateStrategyItem s) => s.LifeSkillType == bookConfig.LifeSkillType && s.Level == strategyLevel)?.TemplateId ?? (-1);
		lifeSkillType = bookConfig.LifeSkillType;
		if (!learnedLifeSkill.IsAllPagesRead())
		{
			return false;
		}
		return strategyTemplateId >= 0;
	}

	public int CalcMoveTimePercent()
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		return (IsOverweight ? CalcOverweightSanctionPercent() : 100) * CalcCarrierTimeBonus();
	}

	public int CalcOverweightSanctionPercent()
	{
		int currInventoryLoad = GetCurrInventoryLoad();
		int maxInventoryLoad = GetMaxInventoryLoad();
		return 300 + CValuePercent.ParseInt(currInventoryLoad - maxInventoryLoad, maxInventoryLoad);
	}

	public CValuePercentBonus CalcCarrierTimeBonus()
	{
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		ItemKey itemKey = GetEquipment()[11];
		if (itemKey.IsValid())
		{
			return CValuePercentBonus.op_Implicit(-DomainManager.Item.GetElement_Carriers(itemKey.Id).GetTravelTimeReduction());
		}
		return CValuePercentBonus.op_Implicit(0);
	}

	public void AddPersonalNeed(DataContext context, GameData.Domains.Character.Ai.PersonalNeed personalNeed)
	{
		if (OfflineAddPersonalNeed(personalNeed))
		{
			SetPersonalNeeds(_personalNeeds, context);
		}
	}

	public void RemovePersonalNeed(DataContext context, GameData.Domains.Character.Ai.PersonalNeed personalNeed)
	{
		int num = _personalNeeds.RemoveAll((GameData.Domains.Character.Ai.PersonalNeed need) => GameData.Domains.Character.Ai.PersonalNeed.MatchType(need, personalNeed));
		if (num > 0)
		{
			SetPersonalNeeds(_personalNeeds, context);
		}
	}

	public bool OfflineAddPersonalNeed(GameData.Domains.Character.Ai.PersonalNeed personalNeed)
	{
		PersonalNeedItem personalNeedItem = Config.PersonalNeed.Instance[personalNeed.TemplateId];
		for (int i = 0; i < _personalNeeds.Count; i++)
		{
			GameData.Domains.Character.Ai.PersonalNeed personalNeed2 = _personalNeeds[i];
			if (!GameData.Domains.Character.Ai.PersonalNeed.MatchType(personalNeed, personalNeed2))
			{
				continue;
			}
			if (personalNeedItem.Overwrite)
			{
				_personalNeeds[i] = personalNeed;
			}
			else if (personalNeedItem.Combine)
			{
				if (personalNeed2.Amount < personalNeed.Amount)
				{
					personalNeed2.Amount = personalNeed.Amount;
				}
				personalNeed2.RemainingMonths = personalNeed.RemainingMonths;
				_personalNeeds[i] = personalNeed2;
			}
			else
			{
				if (personalNeed2.Amount == personalNeed.Amount)
				{
					return false;
				}
				_personalNeeds.Add(personalNeed);
			}
			return true;
		}
		_personalNeeds.Add(personalNeed);
		return true;
	}

	public void OfflineUpdatePersonalNeedsDuration()
	{
		for (int num = _personalNeeds.Count - 1; num >= 0; num--)
		{
			GameData.Domains.Character.Ai.PersonalNeed value = _personalNeeds[num];
			if (value.RemainingMonths <= 0)
			{
				CollectionUtils.SwapAndRemove(_personalNeeds, num);
			}
			else
			{
				value.RemainingMonths--;
				_personalNeeds[num] = value;
			}
		}
	}

	public void OfflinePossession(DeadCharacter deadCharacter)
	{
		_happiness = deadCharacter.Happiness;
		_baseMorality = deadCharacter.Morality;
		_preexistenceCharIds = deadCharacter.PreexistenceCharIds;
		_baseLifeSkillQualifications = deadCharacter.BaseLifeSkillQualifications;
		_baseCombatSkillQualifications = deadCharacter.BaseCombatSkillQualifications;
		_baseMainAttributes = deadCharacter.BaseMainAttributes;
		bool flag = _gender == 1;
		short birthdayFeatureId = CharacterDomain.GetBirthdayFeatureId(GetBirthMonth());
		_featureIds.Clear();
		_featureIds.Add(birthdayFeatureId);
		if (GetFameType() >= 4)
		{
			_featureIds.Add(484);
		}
		else
		{
			sbyte fameType = GetFameType();
			if (fameType >= 0 && fameType < 3)
			{
				_featureIds.Add(485);
			}
		}
		foreach (short featureId in deadCharacter.FeatureIds)
		{
			if (CharacterFeature.Instance[featureId].MutexGroupId != 734)
			{
				if (featureId == 169 && flag)
				{
					_featureIds.Add(168);
				}
				else if (featureId == 168 && !flag)
				{
					_featureIds.Add(169);
				}
				else if (CharacterFeature.Instance[featureId].SoulTransform && CharacterFeature.Instance[featureId].MutexGroupId != 183)
				{
					_featureIds.Add(featureId);
				}
			}
		}
		_featureIds.Sort(CharacterFeatureHelper.CompareFeature);
	}

	public static RecruitCharacterData GenerateRecruitCharacterData(IRandomSource random, sbyte peopleLevel, BuildingBlockKey blockKey, BuildingBlockData blockData)
	{
		Character taiwu = DomainManager.Taiwu.GetTaiwu();
		Location taiwuVillageLocation = DomainManager.Taiwu.GetTaiwuVillageLocation();
		BuildingBlockItem configData = blockData.ConfigData;
		sbyte random2 = Gender.GetRandom(random);
		bool flag = false;
		short num = (short)random.Next(18, 25);
		sbyte stateTemplateIdByAreaId = DomainManager.Map.GetStateTemplateIdByAreaId(taiwuVillageLocation.AreaId);
		sbyte sectID = MapState.Instance[stateTemplateIdByAreaId].SectID;
		short characterTemplateId = OrganizationDomain.GetCharacterTemplateId(sectID, stateTemplateIdByAreaId, random2);
		short memberId = OrganizationDomain.GetMemberId(taiwu.GetOrganizationInfo().OrgTemplateId, peopleLevel);
		OrganizationMemberItem item = OrganizationMember.Instance.GetItem(memberId);
		CharacterItem characterItem = Config.Character.Instance[characterTemplateId];
		short num2 = ((blockData.TemplateId == 217) ? ((short)Math.Min(random.Next(500, 751) + 25 * peopleLevel, 900)) : GenerateRandomAttraction(random));
		short[] combatSkillsAdjust = configData.RecruitCombatSkillsAdjust;
		if (DomainManager.Building.IsDependKungfuPracticeRoom(configData))
		{
			List<short[]> adjusts = new List<short[]>();
			DomainManager.Building.BuildingBlockDependencies(blockKey, delegate(BuildingBlockData dependData, int _, BuildingBlockKey dependKey)
			{
				if (dependData.TemplateId == 52)
				{
					DomainManager.Building.BuildingBlockInfluences(dependKey, delegate(BuildingBlockData influenceData, int num6)
					{
						adjusts.Add(influenceData.ConfigData.RecruitCombatSkillsAdjust);
					});
				}
			});
			combatSkillsAdjust = CharacterCreation.MergeAdjusts(adjusts, 14);
		}
		FeatureCreationContext featureCreationContext = new FeatureCreationContext
		{
			FeatureIds = new List<short>(),
			PotentialFeatureIds = new List<short>(),
			Gender = random2,
			BirthMonth = (sbyte)random.Next(0, 11),
			CurrAge = num,
			RandomFeaturesAtCreating = true,
			PotentialFeaturesAge = -1,
			DestinyType = -1,
			AllGoodBasicFeature = false,
			IsProtagonist = false
		};
		CharacterCreation.CreateFeatures(random, ref featureCreationContext);
		short randomOrgMemberClothing = OrganizationDomain.GetRandomOrgMemberClothing(random, item);
		AvatarData randomAvatar;
		short charm;
		do
		{
			randomAvatar = AvatarManager.Instance.GetRandomAvatar(random, random2, flag, characterItem.PresetBodyType, num2);
			bool flag2 = item.MonkType != 130;
			bool flag3 = random2 == 1 && !flag && !featureCreationContext.FeatureIds.Contains(168);
			for (sbyte b = 0; b < 7; b++)
			{
				if (1 == 0)
				{
				}
				bool flag4 = b switch
				{
					0 => flag2, 
					1 => num >= GlobalConfig.Instance.AgeShowBeard1 && flag3, 
					2 => flag3, 
					3 => IsAbleToGrowWrinkle1(num), 
					4 => IsAbleToGrowWrinkle2(num), 
					5 => IsAbleToGrowWrinkle3(num), 
					6 => IsAbleToGrowEyebrow(), 
					_ => false, 
				};
				if (1 == 0)
				{
				}
				bool flag5 = flag4;
				randomAvatar.SetGrowableElementShowingAbility(b, flag5);
				randomAvatar.SetGrowableElementShowingState(b, flag5);
			}
			charm = randomAvatar.GetCharm(num, Config.Clothing.Instance[randomOrgMemberClothing].DisplayId);
		}
		while (Math.Abs(num2 - charm) > 100);
		num2 = charm;
		int num3 = 0;
		foreach (short featureId in featureCreationContext.FeatureIds)
		{
			int characterPropertyBonus = CharacterFeature.GetCharacterPropertyBonus(featureId, ECharacterPropertyReferencedType.Attraction);
			num3 = DataSumTypeHelper.Sum((EDataSumType)0, num3, characterPropertyBonus);
		}
		short num4 = (short)(num2 + num3);
		short num5 = 0;
		foreach (short featureId2 in featureCreationContext.FeatureIds)
		{
			num5 += CharacterFeature.Instance[featureId2].AttractionPercentBonus;
		}
		num4 = (short)(num4 * (100 + num5) / 100);
		List<sbyte> list = new List<sbyte>();
		ExtraDomain.GetRandomCmdTypes(random, list, CalcMedalCount);
		RecruitCharacterData recruitCharacterData = new RecruitCharacterData
		{
			TemplateId = characterTemplateId,
			PeopleLevel = peopleLevel,
			Age = num,
			BirthMonth = featureCreationContext.BirthMonth,
			FullName = CharacterDomain.GenerateRandomHanName(random, -1, -1, random2, 0),
			BaseAttraction = num2,
			FinalAttraction = num4,
			AvatarData = randomAvatar,
			MainAttributes = CharacterCreation.CreateMainAttributes(random, peopleLevel, item.MainAttributesAdjust),
			FeatureIds = featureCreationContext.FeatureIds,
			Gender = random2,
			Transgender = flag,
			CombatSkillQualifications = CharacterCreation.CreateCombatSkillQualifications(random, peopleLevel, combatSkillsAdjust),
			CombatSkillQualificationGrowthType = (sbyte)random.Next(3),
			LifeSkillQualifications = CharacterCreation.CreateLifeSkillQualifications(random, peopleLevel, configData.RecruitLifeSkillsAdjust),
			LifeSkillQualificationGrowthType = (sbyte)random.Next(3),
			ClothingTemplateId = randomOrgMemberClothing,
			TeammateCommands = list
		};
		recruitCharacterData.Recalculate();
		return recruitCharacterData;
		int CalcMedalCount(sbyte medalType)
		{
			return SharedMethods.CalcFeatureMedalValue(featureCreationContext.FeatureIds, medalType);
		}
	}

	public IEnumerable<SolarTermItem> GetInvokedSolarTerm()
	{
		List<sbyte> validSolarTypes = ObjectPool<List<sbyte>>.Instance.Get();
		validSolarTypes.Clear();
		for (int i = 0; i < 9; i++)
		{
			ItemKey itemKey = _eatingItems.Get(i);
			if (itemKey.ItemType == 9)
			{
				TeaWineItem config = Config.TeaWine.Instance[itemKey.TemplateId];
				if (config.ItemSubType == 901 && config.SolarTermType >= 0 && !validSolarTypes.Contains(config.SolarTermType))
				{
					validSolarTypes.Add(config.SolarTermType);
				}
			}
		}
		sbyte month = DomainManager.World.GetCurrMonthInYear();
		foreach (SolarTermItem solar in (IEnumerable<SolarTermItem>)SolarTerm.Instance)
		{
			if (solar.Month == month && validSolarTypes.Contains(solar.Type))
			{
				yield return solar;
			}
		}
		ObjectPool<List<sbyte>>.Instance.Return(validSolarTypes);
	}

	public CValuePercentBonus GetSolarTermBonus(int value)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		return CValuePercentBonus.op_Implicit(GetSolarTermValue(value));
	}

	public int GetSolarTermValue(int value)
	{
		//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
		if (DomainManager.Taiwu.GetTaiwuCharId() != _id)
		{
			return value;
		}
		if (!DomainManager.Extra.IsProfessionalSkillUnlockedAndEquipped(25))
		{
			return value;
		}
		List<short> list = ObjectPool<List<short>>.Instance.Get();
		for (int i = 0; i < 9; i++)
		{
			ItemKey itemKey = _eatingItems.Get(i);
			if (itemKey.ItemType == 9)
			{
				TeaWineItem teaWineItem = Config.TeaWine.Instance[itemKey.TemplateId];
				if (teaWineItem.ItemSubType == 901 && !list.Contains(itemKey.TemplateId))
				{
					list.Add(itemKey.TemplateId);
				}
			}
		}
		int count = list.Count;
		ObjectPool<List<short>>.Instance.Return(list);
		ProfessionData professionData = DomainManager.Extra.GetProfessionData(7);
		return value * professionData.GetSeniorityToWineTasterSolarTermBonus(count);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal void ActivateAdvanceMonthStatus(byte statusType)
	{
		_advanceMonthStatus |= statusType;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal void DeactivateAdvanceMonthStatus(byte statusType)
	{
		_advanceMonthStatus &= (byte)(~statusType);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal bool IsActiveAdvanceMonthStatus(byte statusType)
	{
		return (_advanceMonthStatus & statusType) != 0;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal void ResetAdvanceMonthStatus()
	{
		_advanceMonthStatus = 0;
	}

	public void PeriAdvanceMonth_UpdateStatus(DataContext context)
	{
		PeriAdvanceMonthUpdateStatusModification periAdvanceMonthUpdateStatusModification = new PeriAdvanceMonthUpdateStatusModification(this);
		OfflineChangeXiangshuInfection(periAdvanceMonthUpdateStatusModification);
		OfflineIncreaseAge(context, periAdvanceMonthUpdateStatusModification);
		if (!OfflineUpdatePregnantState(context, periAdvanceMonthUpdateStatusModification))
		{
			context.ParallelModificationsRecorder.RecordType(ParallelModificationType.PeriAdvanceMonthUpdateStatus);
			context.ParallelModificationsRecorder.RecordParameterClass(periAdvanceMonthUpdateStatusModification);
			return;
		}
		OfflineUpdateEatingItemEffect(context, periAdvanceMonthUpdateStatusModification);
		OfflineUpdateFeaturePoisons(context, periAdvanceMonthUpdateStatusModification);
		if (_xiangshuInfection < 200)
		{
			OfflineAutoRecoverCurrNeili(periAdvanceMonthUpdateStatusModification);
			OfflineChangeQiDisorder(periAdvanceMonthUpdateStatusModification);
			OfflineChangeInjuries(context.Random, periAdvanceMonthUpdateStatusModification);
			OfflineAutoRecoverPoisoned(periAdvanceMonthUpdateStatusModification);
			OfflineAutoRecoverMainAttributes();
		}
		else
		{
			OfflineRecoverAllForXiangshuInfected(periAdvanceMonthUpdateStatusModification);
		}
		OfflineUpdateHappiness(periAdvanceMonthUpdateStatusModification);
		if (_id != DomainManager.Taiwu.GetTaiwuCharId() && GetAgeGroup() != 0 && _kidnapperId < 0)
		{
			OfflineUseResourcesForHealingAndDetox(periAdvanceMonthUpdateStatusModification);
			context.AdvanceMonthRelatedData.CategorizedRegenItems(_inventory.Items);
			OfflineUseMedicineForHealingAndDetox(context, periAdvanceMonthUpdateStatusModification);
			OfflineUpdateHealth();
			OfflineUseResourcesForHealth(periAdvanceMonthUpdateStatusModification);
			OfflineUseMedicineForHealth(context, periAdvanceMonthUpdateStatusModification);
			OfflineUseMedicineForWug(context, periAdvanceMonthUpdateStatusModification);
			OfflineUseItemForNeili(context, periAdvanceMonthUpdateStatusModification);
			OfflineUseFoodForMainAttributes(context, periAdvanceMonthUpdateStatusModification);
			OfflineUseTeaWineForHappiness(context, periAdvanceMonthUpdateStatusModification);
			context.AdvanceMonthRelatedData.ReleaseCategorizedRegenItems();
		}
		else
		{
			OfflineUpdateHealth();
		}
		OfflineUpdateEatingItems(context, periAdvanceMonthUpdateStatusModification);
		ParallelModificationsRecorder parallelModificationsRecorder = context.ParallelModificationsRecorder;
		parallelModificationsRecorder.RecordType(ParallelModificationType.PeriAdvanceMonthUpdateStatus);
		parallelModificationsRecorder.RecordParameterClass(periAdvanceMonthUpdateStatusModification);
	}

	public static void ComplementPeriAdvanceMonth_UpdateStatus(DataContext context, PeriAdvanceMonthUpdateStatusModification mod)
	{
		Character character = mod.Character;
		if (mod.ResourcesChanged)
		{
			character.SetResources(ref character.GetResources(), context);
		}
		if (mod.EatingItemsChanged)
		{
			character.SetEatingItems(ref character.GetEatingItems(), context);
			MonthlyNotificationCollection monthlyNotificationCollection = DomainManager.World.GetMonthlyNotificationCollection();
			if (mod.RemovedWugKings != null)
			{
				foreach (ItemKey removedWugKing in mod.RemovedWugKings)
				{
					monthlyNotificationCollection.AddWugKingDead(character.GetId(), removedWugKing.ItemType, removedWugKing.TemplateId);
				}
			}
			if (mod.RemovedSafetyWugKings != null)
			{
				foreach (ItemKey removedSafetyWugKing in mod.RemovedSafetyWugKings)
				{
					monthlyNotificationCollection.AddWugKingDeadSpecial(character.GetId(), removedSafetyWugKing.ItemType, removedSafetyWugKing.TemplateId);
				}
			}
			if (mod.RemovedWugs != null)
			{
				foreach (short removedWug in mod.RemovedWugs)
				{
					Events.RaiseRemoveWug(context, character._id, removedWug);
				}
			}
		}
		character.SetCurrMainAttributes(character.GetCurrMainAttributes(), context);
		if (mod.CurrNeiliChanged)
		{
			character.SetCurrNeili(character.GetCurrNeili(), context);
		}
		if (mod.QiDisorderChanged)
		{
			character.SetDisorderOfQi(character.GetDisorderOfQi(), context);
		}
		if (mod.InjuriesChanged)
		{
			character.SetInjuries(character.GetInjuries(), context);
		}
		if (mod.PoisonedChanged)
		{
			character.SetPoisoned(ref character.GetPoisoned(), context);
		}
		if (mod.InventoryChanged)
		{
			character.SetInventory(character.GetInventory(), context);
		}
		if (mod.HappinessChanged)
		{
			character.SetHappiness(character.GetHappiness(), context);
		}
		if (mod.XiangshuInfectionChanged)
		{
			character.SetXiangshuInfection(character._xiangshuInfection, context);
		}
		if (mod.CurrAgeChanged)
		{
			character.SetCurrAge(character._currAge, context);
			AvatarData avatar = character.GetAvatar();
			if (avatar.UpdateGrowableElementsShowingAbilities(character))
			{
				character.SetAvatar(avatar, context);
			}
			Events.RaiseCharacterAgeChanged(context, character, character._currAge - 1, character._currAge);
		}
		if (mod.HealthChanged)
		{
			character.SetHealth(character._health, context);
		}
		if (mod.MaxHealthChanged)
		{
			character.SetBaseMaxHealth(character._baseMaxHealth, context);
		}
		if (mod.ActualAgeChanged)
		{
			character.SetActualAge(character._actualAge, context);
		}
		character.TryGrowAvatarElements(context);
		if (mod.HobbyChanged)
		{
			character.CommitChangingHobby(context);
			if (mod.FavorabilitiesOfRelatedChars != null)
			{
				DomainManager.Character.ApplyFavorabilitiesOfRelatedCharsWhenChangingHobby(context, character.GetId(), mod.FavorabilitiesOfRelatedChars);
			}
		}
		if (mod.PregnantStateModification != null)
		{
			DomainManager.Character.ApplyPregnantStateChange(context, character, mod.PregnantStateModification);
		}
		if (mod.ConsumedForbiddenFoodsOrWines != null)
		{
			int currDate = DomainManager.World.GetCurrDate();
			LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
			foreach (ItemKey consumedForbiddenFoodsOrWine in mod.ConsumedForbiddenFoodsOrWines)
			{
				lifeRecordCollection.AddMonkBreakRule(character._id, currDate, character._location, consumedForbiddenFoodsOrWine.ItemType, consumedForbiddenFoodsOrWine.TemplateId);
				SecretInformationCollection secretInformationCollection = DomainManager.Information.GetSecretInformationCollection();
				int dataOffset = secretInformationCollection.AddMonkBreakRule(character._id, (ulong)consumedForbiddenFoodsOrWine);
				DomainManager.Information.AddSecretInformationMetaData(context, dataOffset);
			}
		}
		if (mod.FeaturesChanged)
		{
			character.SetFeatureIds(character.GetFeatureIds(), context);
		}
		DomainManager.Extra.TryRegenerateTeammateCommand(context, character);
		if (character._id != DomainManager.Taiwu.GetTaiwuCharId() && character.GetAgeGroup() != 0)
		{
			DomainManager.Extra.UpdateCombatSkillProficiency(context, character);
		}
		if (mod.PersonalNeedsChanged)
		{
			character.SetPersonalNeeds(character._personalNeeds, context);
		}
		if (mod.NewClothingTemplateId != -1)
		{
			character.ForceReplaceClothing(context, mod.NewClothingTemplateId);
		}
		short health = character.GetHealth();
		character.SetHealth(health, context);
		if (mod.CurrMainAttributesChanged)
		{
			character.SetCurrMainAttributes(character._currMainAttributes, context);
		}
		if (mod.UsedHealingCount > 0)
		{
			DomainManager.Character.UseCombatResources(context, character._id, EHealActionType.Healing, mod.UsedHealingCount);
		}
		if (mod.UsedDetoxCount > 0)
		{
			DomainManager.Character.UseCombatResources(context, character._id, EHealActionType.Detox, mod.UsedDetoxCount);
		}
		if (mod.UsedBreathingCount > 0)
		{
			DomainManager.Character.UseCombatResources(context, character._id, EHealActionType.Breathing, mod.UsedBreathingCount);
		}
		if (mod.UsedRecoverCount > 0)
		{
			DomainManager.Character.UseCombatResources(context, character._id, EHealActionType.Recover, mod.UsedRecoverCount);
		}
		if (character.GetId() == DomainManager.Taiwu.GetTaiwuCharId())
		{
			if (mod.XiangshuInfectionFeatureChanged == 218)
			{
				DomainManager.World.SetTaiwuGettingCompletelyInfected();
			}
			else if (mod.XiangshuInfectionFeatureChanged == 217)
			{
				MonthlyEventCollection monthlyEventCollection = DomainManager.World.GetMonthlyEventCollection();
				monthlyEventCollection.AddTaiwuInfectedPartially(character.GetId(), character._location);
				Events.RaiseXiangshuInfectionFeatureChanged(context, character, mod.XiangshuInfectionFeatureChanged);
			}
			else if (character.GetXiangshuInfection() >= 200 && character._featureIds.Contains(415))
			{
				MonthlyEventCollection monthlyEventCollection2 = DomainManager.World.GetMonthlyEventCollection();
				monthlyEventCollection2.AddMirrorCreatedImpostureXiangshuInfected(character._id, character._location);
			}
			PregnantStateModification pregnantStateModification = mod.PregnantStateModification;
			if (pregnantStateModification != null && pregnantStateModification.LostMother)
			{
				DomainManager.World.SetTaiwuDying(isTaiwuDyingOfDystocia: true);
			}
		}
		else
		{
			if (mod.XiangshuInfectionFeatureChanged >= 0)
			{
				Events.RaiseXiangshuInfectionFeatureChanged(context, character, mod.XiangshuInfectionFeatureChanged);
			}
			PregnantStateModification pregnantStateModification = mod.PregnantStateModification;
			if (pregnantStateModification != null && pregnantStateModification.LostMother)
			{
				DomainManager.Character.MakeCharacterDead(context, character, 2);
			}
		}
	}

	private void ApplyItemLost(DataContext context, List<(MapBlockData block, ItemKey itemKey, int amount)> itemsToBeLost)
	{
		LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
		int currDate = DomainManager.World.GetCurrDate();
		SetInventory(_inventory, context);
		SetHappiness(_happiness, context);
		bool flag = false;
		ItemKey itemKey = ItemKey.Invalid;
		int i = 0;
		for (int count = itemsToBeLost.Count; i < count; i++)
		{
			var (mapBlockData, itemKey2, amount) = itemsToBeLost[i];
			DomainManager.Item.RemoveOwner(itemKey2, ItemOwnerType.CharacterInventory, _id);
			if (mapBlockData != null)
			{
				DomainManager.Map.AddBlockItem(context, mapBlockData, itemKey2, amount);
			}
			else
			{
				DomainManager.Item.RemoveItem(context, itemKey2);
			}
			lifeRecordCollection.AddLoseOverloadingItem(_id, currDate, _location, itemKey2.ItemType, itemKey2.TemplateId);
			if (!flag && ItemTemplateHelper.GetGrade(itemKey2.ItemType, itemKey2.TemplateId) >= 6)
			{
				flag = true;
				if (!itemKey.IsValid() || ItemTemplateHelper.GetBaseValue(itemKey.ItemType, itemKey.TemplateId) < ItemTemplateHelper.GetBaseValue(itemKey2.ItemType, itemKey2.TemplateId))
				{
					itemKey = itemKey2;
				}
			}
		}
		if (flag)
		{
			SecretInformationCollection secretInformationCollection = DomainManager.Information.GetSecretInformationCollection();
			int dataOffset = secretInformationCollection.AddLoseOverloadingItem(_id, (ulong)itemKey, _location);
			DomainManager.Information.AddSecretInformationMetaDataWithNecessity(context, dataOffset, withInitialDistribute: true, necessarily: false, delegate(ICollection<int> charIds)
			{
				foreach (int charId in charIds)
				{
					Character element_Objects = DomainManager.Character.GetElement_Objects(charId);
					Location location = element_Objects.GetLocation();
					GameData.Domains.Character.Ai.PersonalNeed personalNeed = GameData.Domains.Character.Ai.PersonalNeed.CreatePersonalNeed(24, location);
					element_Objects.AddPersonalNeed(context, personalNeed);
				}
			});
		}
		if (_id == DomainManager.Taiwu.GetTaiwuCharId())
		{
			MonthlyNotificationCollection monthlyNotificationCollection = DomainManager.World.GetMonthlyNotificationCollection();
			int num = 0;
			for (int num2 = Math.Min(100, itemsToBeLost.Count); num < num2; num++)
			{
				ItemKey item = itemsToBeLost[num].itemKey;
				monthlyNotificationCollection.AddLoseItemCausedByInventoryFull(_id, item.ItemType, item.TemplateId);
			}
		}
	}

	public void PeriAdvanceMonth_SelfImprovement(DataContext context)
	{
		if (GetAgeGroup() == 0)
		{
			return;
		}
		PeriAdvanceMonthSelfImprovementModification periAdvanceMonthSelfImprovementModification = new PeriAdvanceMonthSelfImprovementModification(this);
		sbyte maxConsummateLevel = GetMaxConsummateLevel();
		if (_consummateLevel < maxConsummateLevel)
		{
			int characterConsummateLevelProgress = DomainManager.Extra.GetCharacterConsummateLevelProgress(_id);
			characterConsummateLevelProgress += GlobalConfig.Instance.ConsummateLevelProgressSpeed[_organizationInfo.Grade];
			do
			{
				int num = GlobalConfig.Instance.ConsummateLevelProgressThreshold[_consummateLevel];
				if (characterConsummateLevelProgress >= num)
				{
					_consummateLevel++;
					characterConsummateLevelProgress -= num;
					continue;
				}
				break;
			}
			while (_consummateLevel < maxConsummateLevel);
			periAdvanceMonthSelfImprovementModification.ConsummateLevelChanged = true;
			periAdvanceMonthSelfImprovementModification.ConsummateLevelProgress = characterConsummateLevelProgress;
		}
		if (_loopingNeigong >= 0)
		{
			CombatSkillItem skillCfg = Config.CombatSkill.Instance[_loopingNeigong];
			(short neili, short qiDisorder, int[] extraNeiliAllocationProgress) tuple = CombatSkillDomain.CalcNeigongLoopingEffect(context.Random, this, skillCfg);
			short item = tuple.neili;
			short item2 = tuple.qiDisorder;
			int[] item3 = tuple.extraNeiliAllocationProgress;
			periAdvanceMonthSelfImprovementModification.LoopingNeigong = (combatSkillTemplateId: _loopingNeigong, neili: item, qiDisorder: item2);
			periAdvanceMonthSelfImprovementModification.ExtraNeiliAllocationProgress = item3;
		}
		short num2 = CalcExpectedNeili();
		if (_extraNeili < num2)
		{
			_extraNeili = num2;
			periAdvanceMonthSelfImprovementModification.ExtraNeiliChanged = true;
		}
		if (context.Random.CheckPercentProb(50 + GetPersonality(2)))
		{
			(GameData.Domains.Item.SkillBook, int, byte) currReadingBook = context.Equipping.GetCurrReadingBook(this);
			if (currReadingBook.Item1 != null)
			{
				OfflineUpdateReadingProgress(context, periAdvanceMonthSelfImprovementModification, currReadingBook.Item1, currReadingBook.Item2, currReadingBook.Item3);
			}
		}
		if (periAdvanceMonthSelfImprovementModification.IsChanged)
		{
			ParallelModificationsRecorder parallelModificationsRecorder = context.ParallelModificationsRecorder;
			parallelModificationsRecorder.RecordType(ParallelModificationType.PeriAdvanceMonthSelfImprovement);
			parallelModificationsRecorder.RecordParameterClass(periAdvanceMonthSelfImprovementModification);
		}
	}

	public void PeriAdvanceMonth_SelfImprovement_Taiwu(DataContext context)
	{
		PeriAdvanceMonthSelfImprovementModification periAdvanceMonthSelfImprovementModification = new PeriAdvanceMonthSelfImprovementModification(this);
		if (GenerateNeigongLoopingImprovementForTaiwu(context, out var neili, out var qiDisorder, out var extraNeiliAllocationProgress))
		{
			periAdvanceMonthSelfImprovementModification.LoopingNeigong = (combatSkillTemplateId: _loopingNeigong, neili: neili, qiDisorder: qiDisorder);
			periAdvanceMonthSelfImprovementModification.ExtraNeiliAllocationProgress = extraNeiliAllocationProgress;
		}
		if (periAdvanceMonthSelfImprovementModification.IsChanged)
		{
			ParallelModificationsRecorder parallelModificationsRecorder = context.ParallelModificationsRecorder;
			parallelModificationsRecorder.RecordType(ParallelModificationType.PeriAdvanceMonthSelfImprovement);
			parallelModificationsRecorder.RecordParameterClass(periAdvanceMonthSelfImprovementModification);
		}
	}

	public bool GenerateNeigongLoopingImprovementForTaiwu(DataContext context, out short neili, out short qiDisorder, out int[] extraNeiliAllocationProgress)
	{
		if (_loopingNeigong >= 0)
		{
			CombatSkillItem skillCfg = Config.CombatSkill.Instance[_loopingNeigong];
			(short, short, int[]) tuple = CombatSkillDomain.CalcNeigongLoopingEffect(context.Random, this, skillCfg);
			neili = tuple.Item1;
			qiDisorder = tuple.Item2;
			extraNeiliAllocationProgress = tuple.Item3;
			byte loopingDifficulty = DomainManager.Extra.GetLoopingDifficulty();
			short num = WorldCreation.Instance[(byte)10].InfluenceFactors[loopingDifficulty];
			neili = (short)(neili * num / 100);
			return true;
		}
		neili = 0;
		qiDisorder = 0;
		extraNeiliAllocationProgress = null;
		return false;
	}

	private short CalcExpectedNeili()
	{
		int num = _extraNeili;
		for (sbyte b = _organizationInfo.Grade; b >= 0; b--)
		{
			OrganizationInfo organizationInfo = _organizationInfo;
			organizationInfo.Grade = b;
			OrganizationMemberItem orgMemberConfig = OrganizationDomain.GetOrgMemberConfig(organizationInfo);
			if (orgMemberConfig.ConsummateLevel <= _consummateLevel)
			{
				if (orgMemberConfig.ConsummateLevel == _consummateLevel || b == _organizationInfo.Grade)
				{
					num = orgMemberConfig.Neili;
					break;
				}
				organizationInfo.Grade++;
				OrganizationMemberItem orgMemberConfig2 = OrganizationDomain.GetOrgMemberConfig(organizationInfo);
				num = _consummateLevel * (orgMemberConfig.Neili + orgMemberConfig2.Neili) / (orgMemberConfig.ConsummateLevel + orgMemberConfig2.ConsummateLevel);
				break;
			}
		}
		return (short)num;
	}

	public static void ComplementPeriAdvanceMonth_SelfImprovement(DataContext context, PeriAdvanceMonthSelfImprovementModification mod)
	{
		Character character = mod.Character;
		if (mod.LoopingNeigong.combatSkillTemplateId >= 0)
		{
			DomainManager.CombatSkill.ApplyNeigongLoopingEffect(context, character, mod.LoopingNeigong.combatSkillTemplateId, mod.LoopingNeigong.neili, mod.ExtraNeiliAllocationProgress);
			if (mod.LoopingNeigong.qiDisorder != 0)
			{
				character.ChangeDisorderOfQiRandomRecovery(context, mod.LoopingNeigong.qiDisorder);
			}
		}
		if (mod.ResourcesChanged)
		{
			character.SetResources(ref character.GetResources(), context);
		}
		if (mod.ConsummateLevelChanged)
		{
			character.SetConsummateLevel(character._consummateLevel, context);
			DomainManager.Extra.SetCharacterConsummateLevelProgress(context, character._id, mod.ConsummateLevelProgress);
		}
		if (mod.ExtraNeiliChanged)
		{
			character.SetExtraNeili(character.GetExtraNeili(), context);
		}
		if (mod.ReadingResult.readingBook != null)
		{
			character.ApplyBookReadingResult(context, mod.ReadingResult.readingBook, mod.ReadingResult.learnedSkillIndex, mod.ReadingResult.page, mod.ReadingResult.succeedPageCount);
		}
	}

	public unsafe void PeriAdvanceMonth_SelfImprovement_LearnNewSkills(DataContext context)
	{
		if (GetAgeGroup() == 0)
		{
			return;
		}
		OrganizationItem organizationItem = Config.Organization.Instance[_organizationInfo.OrgTemplateId];
		if (!organizationItem.IsSect && organizationItem.TemplateId != 16)
		{
			return;
		}
		Personalities personalities = GetPersonalities();
		sbyte cleverness = personalities.Items[1];
		(short, short, byte) combatSkillToLearn = GetCombatSkillToLearn();
		if (combatSkillToLearn.Item1 >= 0)
		{
			CombatSkillShorts combatSkillAttainments = GetCombatSkillAttainments();
			CombatSkillShorts combatSkillQualifications = GetCombatSkillQualifications();
			CombatSkillItem combatSkillItem = Config.CombatSkill.Instance[combatSkillToLearn.Item1];
			int taughtNewSkillSuccessRate = GetTaughtNewSkillSuccessRate(combatSkillItem.Grade, combatSkillQualifications.Items[combatSkillItem.Type], combatSkillAttainments.Items[combatSkillItem.Type], cleverness);
			if (!context.Random.CheckPercentProb(2 * taughtNewSkillSuccessRate))
			{
				combatSkillToLearn.Item1 = -1;
			}
		}
		if (combatSkillToLearn.Item2 >= 0)
		{
			CombatSkillShorts combatSkillAttainments2 = GetCombatSkillAttainments();
			CombatSkillItem combatSkillItem2 = Config.CombatSkill.Instance[combatSkillToLearn.Item2];
			int readingSuccessRate = GetReadingSuccessRate(combatSkillItem2.Grade, 0, combatSkillAttainments2.Items[combatSkillItem2.Type], cleverness);
			if (!context.Random.CheckPercentProb(readingSuccessRate))
			{
				combatSkillToLearn.Item2 = -1;
			}
		}
		(short, short, byte) lifeSkillToLearn = GetLifeSkillToLearn();
		if (lifeSkillToLearn.Item1 >= 0)
		{
			LifeSkillShorts lifeSkillAttainments = GetLifeSkillAttainments();
			LifeSkillShorts lifeSkillQualifications = GetLifeSkillQualifications();
			Config.LifeSkillItem lifeSkillItem = LifeSkill.Instance[lifeSkillToLearn.Item1];
			int taughtNewSkillSuccessRate2 = GetTaughtNewSkillSuccessRate(lifeSkillItem.Grade, lifeSkillQualifications.Items[lifeSkillItem.Type], lifeSkillAttainments.Items[lifeSkillItem.Type], cleverness);
			if (!context.Random.CheckPercentProb(2 * taughtNewSkillSuccessRate2))
			{
				lifeSkillToLearn.Item1 = -1;
			}
		}
		if (lifeSkillToLearn.Item2 >= 0)
		{
			short skillTemplateId = _learnedLifeSkills[lifeSkillToLearn.Item2].SkillTemplateId;
			LifeSkillShorts lifeSkillAttainments2 = GetLifeSkillAttainments();
			Config.LifeSkillItem lifeSkillItem2 = LifeSkill.Instance[skillTemplateId];
			int readingSuccessRate2 = GetReadingSuccessRate(lifeSkillItem2.Grade, 0, lifeSkillAttainments2.Items[lifeSkillItem2.Type], cleverness);
			if (!context.Random.CheckPercentProb(readingSuccessRate2))
			{
				lifeSkillToLearn.Item2 = -1;
			}
		}
		if (combatSkillToLearn.Item1 >= 0 || combatSkillToLearn.Item2 >= 0 || lifeSkillToLearn.Item1 >= 0 || lifeSkillToLearn.Item2 >= 0)
		{
			ParallelModificationsRecorder parallelModificationsRecorder = context.ParallelModificationsRecorder;
			parallelModificationsRecorder.RecordType(ParallelModificationType.PeriAdvanceMonthSelfImprovementLearnNewSkills);
			parallelModificationsRecorder.RecordParameterClass(this);
			parallelModificationsRecorder.RecordParameterUnmanaged<(short, short, byte)>(combatSkillToLearn);
			parallelModificationsRecorder.RecordParameterUnmanaged<(short, short, byte)>(lifeSkillToLearn);
		}
	}

	public static void ComplementPeriAdvanceMonth_SelfImprovement_LearnNewSkills(DataContext context, Character character, (short newSkillTemplateId, short newPageSkillTemplateId, byte pageId) combatSkillToLearn, (short newSkillTemplateId, short newPageSkillIndex, byte pageId) lifeSkillToLearn)
	{
		LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
		int currDate = DomainManager.World.GetCurrDate();
		if (combatSkillToLearn.newSkillTemplateId >= 0)
		{
			lifeRecordCollection.AddLearnCombatSkill(character._id, currDate, character.GetLocation(), combatSkillToLearn.newSkillTemplateId);
			character.LearnNewCombatSkill(context, combatSkillToLearn.newSkillTemplateId, 0);
			character.CreateInventoryItem(context, 10, Config.CombatSkill.Instance[combatSkillToLearn.newSkillTemplateId].BookId, 1);
		}
		if (combatSkillToLearn.newPageSkillTemplateId >= 0)
		{
			GameData.Domains.CombatSkill.CombatSkill element_CombatSkills = DomainManager.CombatSkill.GetElement_CombatSkills(new CombatSkillKey(character._id, combatSkillToLearn.newPageSkillTemplateId));
			sbyte randomDirection = CombatSkillDirection.GetRandomDirection(context.Random);
			byte normalPageInternalIndex = CombatSkillStateHelper.GetNormalPageInternalIndex(randomDirection, combatSkillToLearn.pageId);
			ushort readingState = CombatSkillStateHelper.SetPageRead(element_CombatSkills.GetReadingState(), normalPageInternalIndex);
			element_CombatSkills.SetReadingState(readingState, context);
			DomainManager.CombatSkill.TryActivateCombatSkillBookPageWhenSetReadingState(context, character.GetId(), element_CombatSkills.GetId().SkillTemplateId, normalPageInternalIndex);
		}
		if (lifeSkillToLearn.newSkillTemplateId >= 0)
		{
			lifeRecordCollection.AddLearnLifeSkill(character._id, currDate, character.GetLocation(), lifeSkillToLearn.newSkillTemplateId);
			character.LearnNewLifeSkill(context, lifeSkillToLearn.newSkillTemplateId, 1);
			character.CreateInventoryItem(context, 10, LifeSkill.Instance[lifeSkillToLearn.newSkillTemplateId].SkillBookId, 1);
		}
		if (lifeSkillToLearn.newPageSkillIndex >= 0)
		{
			character.ReadLifeSkillPage(context, lifeSkillToLearn.newPageSkillIndex, lifeSkillToLearn.pageId);
		}
	}

	private (short newSkillTemplateId, short newPageSkillIndex, byte pageId) GetLifeSkillToLearn()
	{
		OrganizationMemberItem orgMemberConfig = OrganizationDomain.GetOrgMemberConfig(_organizationInfo);
		if (_organizationInfo.OrgTemplateId == 16)
		{
			if (!DomainManager.Taiwu.GetVillagerLearnLifeSkillsFromSect())
			{
				return (newSkillTemplateId: -1, newPageSkillIndex: -1, pageId: 0);
			}
			Location taiwuVillageLocation = DomainManager.Taiwu.GetTaiwuVillageLocation();
			sbyte stateTemplateIdByAreaId = DomainManager.Map.GetStateTemplateIdByAreaId(taiwuVillageLocation.AreaId);
			MapStateItem mapStateItem = MapState.Instance[stateTemplateIdByAreaId];
			orgMemberConfig = OrganizationDomain.GetOrgMemberConfig(mapStateItem.SectID, 6);
		}
		short num = -1;
		short num2 = -1;
		byte b = 0;
		for (sbyte b2 = 0; b2 < orgMemberConfig.LifeSkillsAdjust.Length; b2++)
		{
			short num3 = orgMemberConfig.LifeSkillsAdjust[b2];
			if (num3 > 0)
			{
				for (int i = 0; i <= orgMemberConfig.LifeSkillGradeLimit; i++)
				{
					short num4 = Config.LifeSkillType.Instance[b2].SkillList[i];
					int num5 = FindLearnedLifeSkillIndex(num4);
					if (num5 < 0)
					{
						if (num < 0)
						{
							num = num4;
						}
					}
					else if (num2 < 0)
					{
						LifeSkillItem lifeSkillItem = _learnedLifeSkills[num5];
						if (!lifeSkillItem.IsAllPagesRead())
						{
							num2 = (short)num5;
							b = 0;
							while (b < 5 && lifeSkillItem.IsPageRead(b))
							{
								b++;
							}
						}
					}
					if (num >= 0 && num2 >= 0)
					{
						return (newSkillTemplateId: num, newPageSkillIndex: num2, pageId: b);
					}
				}
			}
		}
		return (newSkillTemplateId: num, newPageSkillIndex: num2, pageId: b);
	}

	private (short newSkillTemplateId, short newPageSkillTemplateId, byte pageId) GetCombatSkillToLearn()
	{
		OrganizationMemberItem orgMemberConfig = OrganizationDomain.GetOrgMemberConfig(_organizationInfo);
		if (_organizationInfo.OrgTemplateId == 16)
		{
			if (!DomainManager.Taiwu.GetVillagerLearnCombatSkillsFromSect())
			{
				return (newSkillTemplateId: -1, newPageSkillTemplateId: -1, pageId: 0);
			}
			Location taiwuVillageLocation = DomainManager.Taiwu.GetTaiwuVillageLocation();
			sbyte stateTemplateIdByAreaId = DomainManager.Map.GetStateTemplateIdByAreaId(taiwuVillageLocation.AreaId);
			MapStateItem mapStateItem = MapState.Instance[stateTemplateIdByAreaId];
			orgMemberConfig = OrganizationDomain.GetOrgMemberConfig(mapStateItem.SectID, 6);
		}
		Dictionary<short, GameData.Domains.CombatSkill.CombatSkill> charCombatSkills = DomainManager.CombatSkill.GetCharCombatSkills(_id);
		short num = -1;
		short num2 = -1;
		byte item = 0;
		foreach (PresetOrgMemberCombatSkill combatSkill in orgMemberConfig.CombatSkills)
		{
			CombatSkillItem combatSkillItem = Config.CombatSkill.Instance[combatSkill.SkillGroupId];
			IReadOnlyList<CombatSkillItem> learnableCombatSkills = CombatSkillDomain.GetLearnableCombatSkills(combatSkillItem.SectId, combatSkillItem.Type);
			int num3 = Math.Min(combatSkill.MaxGrade, learnableCombatSkills.Count - 1);
			for (int i = 0; i <= num3; i++)
			{
				short templateId = learnableCombatSkills[i].TemplateId;
				if (!charCombatSkills.TryGetValue(templateId, out var value))
				{
					if (num < 0)
					{
						num = templateId;
					}
				}
				else if (num2 < 0)
				{
					ushort readingState = value.GetReadingState();
					byte nextPageToRead = CombatSkillStateHelper.GetNextPageToRead(readingState);
					if (nextPageToRead < 15 && !value.GetRevoked())
					{
						num2 = value.GetId().SkillTemplateId;
						item = nextPageToRead;
					}
				}
				if (num >= 0 && num2 >= 0)
				{
					return (newSkillTemplateId: num, newPageSkillTemplateId: num2, pageId: item);
				}
			}
		}
		return (newSkillTemplateId: num, newPageSkillTemplateId: num2, pageId: item);
	}

	public void PeriAdvanceMonth_ActivePreparation_GetSupply(DataContext context)
	{
		sbyte ageGroup = GetAgeGroup();
		if (ageGroup == 0)
		{
			return;
		}
		PeriAdvanceMonthGetSupplyModification periAdvanceMonthGetSupplyModification = new PeriAdvanceMonthGetSupplyModification(this);
		OrganizationMemberItem orgMemberConfig = OrganizationDomain.GetOrgMemberConfig(_organizationInfo);
		context.AdvanceMonthRelatedData.SummarizeItemSubTypeStats(_inventory.Items, _equipment);
		periAdvanceMonthGetSupplyModification.ResourceChanged = OfflineGetResourcesSupply(orgMemberConfig);
		if (_location.IsValid() && _location.AreaId != 138)
		{
			OfflineGetBookSupply(context, periAdvanceMonthGetSupplyModification);
			if (_organizationInfo.OrgTemplateId == 16)
			{
				Location taiwuVillageLocation = DomainManager.Taiwu.GetTaiwuVillageLocation();
				sbyte stateTemplateIdByAreaId = DomainManager.Map.GetStateTemplateIdByAreaId(taiwuVillageLocation.AreaId);
				MapStateItem mapStateItem = MapState.Instance[stateTemplateIdByAreaId];
				OrganizationMemberItem orgMemberConfig2 = OrganizationDomain.GetOrgMemberConfig(mapStateItem.SectID, 6);
				if (DomainManager.Taiwu.GetVillagerLearnLifeSkillsFromSect())
				{
					OfflineGetItemSupply(context, periAdvanceMonthGetSupplyModification, orgMemberConfig2.ItemSatisfyingThreshold, orgMemberConfig2.Inventory);
				}
				if (DomainManager.Taiwu.GetVillagerLearnCombatSkillsFromSect())
				{
					OfflineGetEquipmentSupply(context, periAdvanceMonthGetSupplyModification, orgMemberConfig2.Equipment);
				}
			}
			else
			{
				OfflineGetItemSupply(context, periAdvanceMonthGetSupplyModification, orgMemberConfig.ItemSatisfyingThreshold, orgMemberConfig.Inventory);
				OfflineGetEquipmentSupply(context, periAdvanceMonthGetSupplyModification, orgMemberConfig.Equipment);
			}
		}
		if (ageGroup == 2)
		{
			OfflineGetClothingSupply(context, periAdvanceMonthGetSupplyModification, orgMemberConfig.Clothing.TemplateId);
		}
		context.AdvanceMonthRelatedData.ReleaseItemSubTypeStats();
		if (periAdvanceMonthGetSupplyModification.IsChanged)
		{
			ParallelModificationsRecorder parallelModificationsRecorder = context.ParallelModificationsRecorder;
			parallelModificationsRecorder.RecordType(ParallelModificationType.PeriAdvanceMonthActivePreparationGetSupply);
			parallelModificationsRecorder.RecordParameterClass(periAdvanceMonthGetSupplyModification);
		}
	}

	public static void ComplementPeriAdvanceMonth_ActivePreparation_GetSupply(DataContext context, PeriAdvanceMonthGetSupplyModification mod)
	{
		Character character = mod.Character;
		List<(sbyte, short, int)> itemsToCreate = mod.ItemsToCreate;
		if (itemsToCreate != null && itemsToCreate.Count > 0)
		{
			foreach (var item in mod.ItemsToCreate)
			{
				character.OfflineCreateInventoryItem(context, item.type, item.templateId, item.amount);
			}
			character.SetInventory(character.GetInventory(), context);
		}
		if (mod.ResourceChanged)
		{
			character.SetResources(ref character._resources, context);
		}
		if (mod.PersonalNeedChanged)
		{
			character.SetPersonalNeeds(character._personalNeeds, context);
		}
	}

	public void PeriAdvanceMonth_ActivePreparation(DataContext context)
	{
		if (GetAgeGroup() != 0)
		{
			PeriAdvanceMonthActivePreparationModification periAdvanceMonthActivePreparationModification = new PeriAdvanceMonthActivePreparationModification(this);
			OfflineRepairEquipments(context, periAdvanceMonthActivePreparationModification);
			OfflineFeedAnimalCarrier(context.Random, periAdvanceMonthActivePreparationModification);
			bool flag = !DomainManager.Taiwu.IsInGroup(_id);
			if (flag || !DomainManager.Character.IsCombatSkillAttainmentLocked(_id))
			{
				context.Equipping.ParallelSetInitialCombatSkillAttainmentPanels(context, this);
			}
			SelectEquipmentsModification selectEquipmentsModification = context.Equipping.ParallelSelectEquipments(context, this, flag);
			OfflineAddPoisonToEquipments(context, periAdvanceMonthActivePreparationModification, selectEquipmentsModification.EquippedItems);
			if (periAdvanceMonthActivePreparationModification.IsChanged)
			{
				ParallelModificationsRecorder parallelModificationsRecorder = context.ParallelModificationsRecorder;
				parallelModificationsRecorder.RecordType(ParallelModificationType.PeriAdvanceMonthActivePreparation);
				parallelModificationsRecorder.RecordParameterClass(periAdvanceMonthActivePreparationModification);
			}
		}
	}

	public static void ComplementPeriAdvanceMonth_ActivePreparation(DataContext context, PeriAdvanceMonthActivePreparationModification mod)
	{
		Character character = mod.Character;
		int id = character.GetId();
		Location location = character.GetLocation();
		if (!location.IsValid())
		{
			location = character.GetValidLocation();
		}
		LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
		int currDate = DomainManager.World.GetCurrDate();
		if (mod.ResourcesChanged)
		{
			character.SetResources(ref character._resources, context);
		}
		if (mod.ItemsFixed != null)
		{
			foreach (ItemBase item in mod.ItemsFixed)
			{
				item.SetCurrDurability(item.GetCurrDurability(), context);
				lifeRecordCollection.AddRepairItem(id, currDate, location, item.GetItemType(), item.GetTemplateId());
			}
		}
		if (mod.CraftToolsUsed != null)
		{
			foreach (int item2 in mod.CraftToolsUsed)
			{
				GameData.Domains.Item.CraftTool element_CraftTools = DomainManager.Item.GetElement_CraftTools(item2);
				element_CraftTools.SetCurrDurability(element_CraftTools.GetCurrDurability(), context);
			}
		}
		if (mod.PersonalNeedChanged)
		{
			character.SetPersonalNeeds(character._personalNeeds, context);
		}
		if (mod.FeedingCarrierKey.IsValid() && mod.FeedingFoodKey.IsValid())
		{
			DomainManager.Extra.FeedCarrier(context, character, mod.FeedingCarrierKey, mod.FeedingFoodKey);
			lifeRecordCollection.AddFeedTheAnimal(id, currDate, mod.FeedingFoodKey.ItemType, mod.FeedingFoodKey.TemplateId, mod.FeedingCarrierKey.ItemType, mod.FeedingCarrierKey.TemplateId);
		}
		if (mod.PoisonsToUse == null || mod.EquipmentSlotToAddPoison < 0)
		{
			return;
		}
		ItemKey itemKey = character._equipment[mod.EquipmentSlotToAddPoison];
		if (!itemKey.IsValid())
		{
			AdaptableLog.TagWarning($"ActivePreparation ({id})", $"Invalid item {itemKey} at slot {mod.EquipmentSlotToAddPoison} selected for adding poison.");
		}
		else
		{
			ItemKey itemKey2 = character.ApplyAddPoisonsToItem(context, itemKey, mod.PoisonsToUse);
			if (itemKey != itemKey2)
			{
				character._equipment[mod.EquipmentSlotToAddPoison] = itemKey2;
				character.SetEquipment(character._equipment, context);
			}
		}
	}

	private ItemKey ApplyAddPoisonsToItem(DataContext context, ItemKey itemKey, ItemKey[] poisonKeys)
	{
		LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
		int currDate = DomainManager.World.GetCurrDate();
		Location validLocation = GetValidLocation();
		ItemBase itemBase = DomainManager.Item.GetBaseItem(itemKey);
		if (poisonKeys.Length == 3 && poisonKeys[0].TemplateEquals(poisonKeys[1]) && poisonKeys[0].TemplateEquals(poisonKeys[2]))
		{
			ItemBase item = DomainManager.Item.SetAttachedPoisons(context, itemBase, poisonKeys[0].TemplateId, add: true, new short[2]
			{
				poisonKeys[1].TemplateId,
				poisonKeys[1].TemplateId
			}).item;
			itemBase = item;
			lifeRecordCollection.AddEnvenomedItemOverload(_id, currDate, validLocation, poisonKeys[0].ItemType, poisonKeys[0].TemplateId, poisonKeys[1].ItemType, poisonKeys[1].TemplateId, poisonKeys[2].ItemType, poisonKeys[2].TemplateId, itemKey.ItemType, itemKey.TemplateId);
			SecretInformationCollection secretInformationCollection = DomainManager.Information.GetSecretInformationCollection();
			int dataOffset = secretInformationCollection.AddAddPoisonToItem(_id, (ulong)item.GetItemKey(), (ulong)poisonKeys[0]);
			DomainManager.Information.AddSecretInformationMetaData(context, dataOffset);
		}
		else
		{
			for (int i = 0; i < poisonKeys.Length; i++)
			{
				ItemKey itemKey2 = poisonKeys[i];
				if (itemKey2.IsValid() && _inventory.Items.ContainsKey(itemKey2))
				{
					MedicineItem medicineItem = Config.Medicine.Instance[itemKey2.TemplateId];
					Tester.Assert(medicineItem.EffectType == EMedicineEffectType.ApplyPoison);
					ItemBase item2 = DomainManager.Item.SetAttachedPoisons(context, itemBase, itemKey2.TemplateId, add: true).item;
					itemBase = item2;
					lifeRecordCollection.AddAddPoisonToItem(_id, currDate, validLocation, itemKey2.ItemType, itemKey2.TemplateId, itemKey.ItemType, itemKey.TemplateId);
					SecretInformationCollection secretInformationCollection2 = DomainManager.Information.GetSecretInformationCollection();
					int dataOffset2 = secretInformationCollection2.AddAddPoisonToItem(_id, (ulong)item2.GetItemKey(), (ulong)itemKey2);
					DomainManager.Information.AddSecretInformationMetaData(context, dataOffset2);
				}
			}
		}
		foreach (ItemKey itemKey3 in poisonKeys)
		{
			RemoveInventoryItem(context, itemKey3, 1, deleteItem: true);
		}
		return itemBase.GetItemKey();
	}

	public unsafe void PeriAdvanceMonth_PassivePreparation(DataContext context)
	{
		PeriAdvanceMonthPassivePreparationModification periAdvanceMonthPassivePreparationModification = new PeriAdvanceMonthPassivePreparationModification(this);
		if (DomainManager.World.GetCurrMonthInYear() == GlobalConfig.Instance.CricketActiveStartMonth + 1 && DomainManager.Item.HasNewDeadCricket())
		{
			foreach (ItemKey key in _inventory.Items.Keys)
			{
				if (key.ItemType == 11 && DomainManager.Item.IsNewDeadCricket(key))
				{
					GameData.Domains.Character.Ai.PersonalNeed personalNeed = GameData.Domains.Character.Ai.PersonalNeed.CreatePersonalNeed(10, 11, key.TemplateId);
					OfflineAddPersonalNeed(personalNeed);
					periAdvanceMonthPassivePreparationModification.PersonalNeedsChanged = true;
				}
			}
		}
		OrganizationMemberItem orgMemberConfig = OrganizationDomain.GetOrgMemberConfig(_organizationInfo);
		for (sbyte b = 0; b < 7; b++)
		{
			int num = _resources.Items[b];
			int adjustedResourceSatisfyingAmount = orgMemberConfig.GetAdjustedResourceSatisfyingAmount(b);
			if (num > adjustedResourceSatisfyingAmount)
			{
				GameData.Domains.Character.Ai.PersonalNeed personalNeed2 = GameData.Domains.Character.Ai.PersonalNeed.CreatePersonalNeed(9, b, orgMemberConfig.ResourceSatisfyingThreshold);
				OfflineAddPersonalNeed(personalNeed2);
				periAdvanceMonthPassivePreparationModification.PersonalNeedsChanged = true;
			}
		}
		int num2 = OfflineLoseOverloadedItems(context, periAdvanceMonthPassivePreparationModification);
		_happiness = (sbyte)Math.Clamp(_happiness + num2, -119, 119);
		int inventoryTotalValue = GetInventoryTotalValue();
		if (inventoryTotalValue > orgMemberConfig.ItemSatisfyingThreshold)
		{
			GameData.Domains.Character.Ai.PersonalNeed personalNeed3 = GameData.Domains.Character.Ai.PersonalNeed.CreatePersonalNeed(13, orgMemberConfig.ItemSatisfyingThreshold);
			OfflineAddPersonalNeed(personalNeed3);
			periAdvanceMonthPassivePreparationModification.PersonalNeedsChanged = true;
		}
		ParallelModificationsRecorder parallelModificationsRecorder = context.ParallelModificationsRecorder;
		parallelModificationsRecorder.RecordType(ParallelModificationType.PeriAdvanceMonthPassivePreparation);
		parallelModificationsRecorder.RecordParameterClass(periAdvanceMonthPassivePreparationModification);
	}

	public void PeriAdvanceMonth_GearMateLoseOverLoadedItems(DataContext context)
	{
		int num = GetCurrInventoryLoad() - GetMaxInventoryLoad();
		if (num > 0)
		{
			PeriAdvanceMonthPassivePreparationModification periAdvanceMonthPassivePreparationModification = new PeriAdvanceMonthPassivePreparationModification(this);
			OfflineLoseOverloadedItems(context, periAdvanceMonthPassivePreparationModification);
			ParallelModificationsRecorder parallelModificationsRecorder = context.ParallelModificationsRecorder;
			parallelModificationsRecorder.RecordType(ParallelModificationType.PeriAdvanceMonthPassivePreparation);
			parallelModificationsRecorder.RecordParameterClass(periAdvanceMonthPassivePreparationModification);
		}
	}

	public static void ComplementPeriAdvanceMonth_PassivePreparation(DataContext context, PeriAdvanceMonthPassivePreparationModification mod)
	{
		Character character = mod.Character;
		if (mod.ItemsToBeLost != null)
		{
			character.ApplyItemLost(context, mod.ItemsToBeLost);
		}
		if (mod.PersonalNeedsChanged)
		{
			character.SetPersonalNeeds(character._personalNeeds, context);
		}
	}

	public static void ComplementPostAdvanceMonth_PersonalNeedsUpdate(DataContext context, Character character)
	{
		character.SetPersonalNeeds(character._personalNeeds, context);
	}

	public void PeriAdvanceMonth_PersonalNeedsProcessing(DataContext context)
	{
		if (_personalNeeds.Count == 0)
		{
			return;
		}
		for (int num = _personalNeeds.Count - 1; num >= 0; num--)
		{
			GameData.Domains.Character.Ai.PersonalNeed need = _personalNeeds[num];
			if (!need.CheckValid(this))
			{
				CollectionUtils.SwapAndRemove(_personalNeeds, num);
			}
		}
		ParallelModificationsRecorder parallelModificationsRecorder = context.ParallelModificationsRecorder;
		parallelModificationsRecorder.RecordType(ParallelModificationType.PeriAdvanceMonthPersonalNeedsProcessing);
		parallelModificationsRecorder.RecordParameterClass(this);
	}

	public static void ComplementPeriAdvanceMonth_PersonalNeedsProcessing(DataContext context, Character character)
	{
		character.SetPersonalNeeds(character._personalNeeds, context);
	}

	public unsafe MainAttributes GetMainAttributesRecoveries()
	{
		MainAttributes result = default(MainAttributes);
		MainAttributes maxMainAttributes = GetMaxMainAttributes();
		short physiologicalAge = GetPhysiologicalAge();
		int index = ((physiologicalAge <= 100) ? physiologicalAge : 100);
		MainAttributes mainAttributesRecoveries = AgeEffect.Instance[index].MainAttributesRecoveries;
		int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
		int num = ((_id == taiwuCharId) ? (DomainManager.Extra.GetActionPointPrevMonth() / 10) : 0);
		for (sbyte b = 0; b < 6; b++)
		{
			short num2 = maxMainAttributes.Items[b];
			int num3 = num2 / 5 * mainAttributesRecoveries.Items[b] / 100;
			num3 = Math.Max(1, num3 * (num + 100) / 100);
			if (_id == taiwuCharId)
			{
				ExtraDomain extra = DomainManager.Extra;
				int professionId = ProfessionRelatedConstants.MainAttributeRecoverProfessionIds[b];
				if (extra.IsProfessionalSkillUnlocked(professionId, 0))
				{
					ProfessionData professionData = extra.GetProfessionData(professionId);
					num3 = professionData.GetMainAttributesRecoveryBonusAppliedRate(b, num3);
				}
			}
			result.Items[b] = (short)num3;
		}
		return result;
	}

	public short GetHealthRecovery(bool isCompletelyInfected)
	{
		if (isCompletelyInfected)
		{
			return short.MaxValue;
		}
		int num = 0;
		num += GetHealthChangeDueToInjuries(ref _injuries);
		num += GetHealthChangeDueToPoisons(ref _poisoned, ref GetPoisonResists());
		num += GetHealthChangeDueToDisorderOfQi(_disorderOfQi);
		int num2 = 100;
		foreach (short featureId in _featureIds)
		{
			if (CharacterFeature.Instance[featureId].HealthDelta != 0)
			{
				num2 = num2 * (100 - CharacterFeature.Instance[featureId].HealthDelta) / 100;
			}
		}
		num -= _maxHealth * (100 - num2) / 100;
		num += DomainManager.Building.GetBuildingBlockEffect(_location, EBuildingScaleEffect.HealthRecovery);
		int totalMedicineEffectValue = _eatingItems.GetTotalMedicineEffectValue(EMedicineEffectSubType.RecoverHealthValue);
		num += GetSpecialEffectModifiedMedicineEffectValue(totalMedicineEffectValue);
		totalMedicineEffectValue = GetLeftMaxHealth() * _eatingItems.GetTotalMedicineEffectValue(EMedicineEffectSubType.RecoverHealthPercentage) / 100;
		num += GetSpecialEffectModifiedMedicineEffectValue(totalMedicineEffectValue);
		if (GetAgeGroup() == 0 && _leaderId < 0 && _location.IsValid())
		{
			MapBlockData block = DomainManager.Map.GetBlock(_location);
			int num3 = num;
			EMapBlockType blockType = block.BlockType;
			if (1 == 0)
			{
			}
			int num4 = blockType switch
			{
				EMapBlockType.City => 6, 
				EMapBlockType.Sect => 6, 
				EMapBlockType.Town => 6, 
				EMapBlockType.Station => 12, 
				EMapBlockType.Developed => 12, 
				EMapBlockType.Normal => 18, 
				_ => 24, 
			};
			if (1 == 0)
			{
			}
			num = num3 - num4;
		}
		num = DomainManager.SpecialEffect.ModifyValue(_id, 263, num);
		return (short)num;
	}

	public unsafe static int GetHealthChangeDueToInjuries(ref Injuries injuries)
	{
		int num = 0;
		for (int i = 0; i < 14; i++)
		{
			num += Math.Max(injuries.Items[i] / 2, 0);
		}
		return -num * 12;
	}

	public unsafe static int GetHealthChangeDueToPoisons(ref PoisonInts poisoned, ref PoisonInts poisonResists)
	{
		int num = 0;
		for (int i = 0; i < 6; i++)
		{
			num += ((poisonResists.Items[i] < 1000) ? PoisonsAndLevels.CalcPoisonedLevel(poisoned.Items[i]) : 0);
		}
		return -num * 12;
	}

	public int GetHealthChangeDueToDisorderOfQi(short disorderOfQi)
	{
		sbyte healthRecovery = DisorderLevelOfQi.GetDisorderLevelOfQiConfig(disorderOfQi).HealthRecovery;
		if (healthRecovery > 0 && !DomainManager.SpecialEffect.ModifyData(_id, -1, 295, dataValue: true))
		{
			return 0;
		}
		return healthRecovery;
	}

	public short GetChangeOfQiDisorder()
	{
		int num = -GlobalConfig.Instance.RecoveryOfQiDisorderUnitValue;
		num -= DomainManager.Building.GetBuildingBlockEffect(_location, EBuildingScaleEffect.QiDisorderRecovery);
		int num2 = 100;
		int num3 = 0;
		foreach (short featureId in _featureIds)
		{
			CharacterFeatureItem characterFeatureItem = CharacterFeature.Instance[featureId];
			num2 += characterFeatureItem.QiDisorderBuffPercent;
			if (characterFeatureItem.QiDisorderDelta > 0)
			{
				num3 += characterFeatureItem.QiDisorderDelta;
			}
			else
			{
				num += characterFeatureItem.QiDisorderDelta;
			}
		}
		num = num * num2 / 100;
		int num4 = ((_id == DomainManager.Taiwu.GetTaiwuCharId()) ? DomainManager.Extra.GetPreserveDay() : 0);
		int num5 = num * GetRecoveryOfQiDisorder() / 100;
		num5 = num5 * (num4 + 100) / 100;
		num += num5;
		for (int i = 0; i < 9; i++)
		{
			ItemKey itemKey = _eatingItems.Get(i);
			if (itemKey.IsValid() && itemKey.ItemType == 8)
			{
				MedicineItem configAs = itemKey.GetConfigAs<MedicineItem>();
				if (configAs.EffectType == EMedicineEffectType.ChangeDisorderOfQi)
				{
					num += CalcMedicineEffectDelta_RecoverDisorderOfQi(configAs.EffectValue, configAs.EffectIsPercentage);
				}
			}
		}
		return (short)Math.Clamp(num + num3, DisorderLevelOfQi.MinValue - DisorderLevelOfQi.MaxValue, DisorderLevelOfQi.MaxValue - DisorderLevelOfQi.MinValue);
	}

	private unsafe bool OfflineGetResourcesSupply(OrganizationMemberItem orgMemberCfg)
	{
		bool result = false;
		for (sbyte b = 0; b < 8; b++)
		{
			int num = orgMemberCfg.GetAdjustedResourceSatisfyingAmount(b) * DomainManager.World.GetGainResourcePercent(8) / 100;
			int num2 = num / 2;
			int num3 = num / 40;
			int num4 = _resources.Items[b];
			if (num4 < num2)
			{
				ref int reference = ref _resources.Items[b];
				reference += num3;
				result = true;
			}
		}
		return result;
	}

	private void OfflineGetBookSupply(DataContext context, PeriAdvanceMonthGetSupplyModification mod)
	{
		HashSet<short> obj = context.AdvanceMonthRelatedData.TemplateIdSet.Occupy();
		foreach (var (itemKey2, _) in _inventory.Items)
		{
			if (itemKey2.ItemType == 10)
			{
				obj.Add(itemKey2.TemplateId);
			}
		}
		OfflineGetCombatSkillBookSupply(context, obj, mod);
		OfflineGetLifeSkillBookSupply(context, obj, mod);
		context.AdvanceMonthRelatedData.TemplateIdSet.Release(ref obj);
	}

	private unsafe void OfflineGetCombatSkillBookSupply(DataContext context, HashSet<short> ownedBooks, PeriAdvanceMonthGetSupplyModification mod)
	{
		if (!OrganizationDomain.IsSect(_organizationInfo.OrgTemplateId))
		{
			return;
		}
		List<TemplateKey> obj = context.AdvanceMonthRelatedData.ItemTemplateKeys.Occupy();
		Dictionary<short, GameData.Domains.CombatSkill.CombatSkill> charCombatSkills = DomainManager.CombatSkill.GetCharCombatSkills(_id);
		foreach (KeyValuePair<short, GameData.Domains.CombatSkill.CombatSkill> item in charCombatSkills)
		{
			item.Deconstruct(out var key, out var value);
			short index = key;
			GameData.Domains.CombatSkill.CombatSkill combatSkill = value;
			ushort readingState = combatSkill.GetReadingState();
			if (!combatSkill.GetRevoked() && CombatSkillStateHelper.CalcPagesToBeReadForActivation(readingState) > 0)
			{
				CombatSkillItem combatSkillItem = Config.CombatSkill.Instance[index];
				if (combatSkillItem.SectId == _organizationInfo.OrgTemplateId && combatSkillItem.BookId >= 0 && _organizationInfo.Grade >= combatSkillItem.Grade && !ownedBooks.Contains(combatSkillItem.BookId) && context.Random.CheckPercentProb(_combatSkillQualifications.Items[combatSkillItem.Type] / 5))
				{
					obj.Add(new TemplateKey(10, combatSkillItem.BookId));
				}
			}
		}
		if (obj.Count == 0)
		{
			context.AdvanceMonthRelatedData.ItemTemplateKeys.Release(ref obj);
			return;
		}
		TemplateKey random = obj.GetRandom(context.Random);
		context.AdvanceMonthRelatedData.ItemTemplateKeys.Release(ref obj);
		mod.AddItemToCreate(random.ItemType, random.TemplateId, 1);
	}

	private unsafe void OfflineGetLifeSkillBookSupply(DataContext context, HashSet<short> ownedBooks, PeriAdvanceMonthGetSupplyModification mod)
	{
		OrganizationMemberItem orgMemberConfig = OrganizationDomain.GetOrgMemberConfig(_organizationInfo);
		List<TemplateKey> obj = context.AdvanceMonthRelatedData.ItemTemplateKeys.Occupy();
		foreach (LifeSkillItem learnedLifeSkill in _learnedLifeSkills)
		{
			if (!learnedLifeSkill.IsAllPagesRead())
			{
				Config.LifeSkillItem lifeSkillItem = LifeSkill.Instance[learnedLifeSkill.SkillTemplateId];
				short num = orgMemberConfig.LifeSkillsAdjust[lifeSkillItem.Type];
				if (num > 0 && orgMemberConfig.LifeSkillGradeLimit >= lifeSkillItem.Grade && !ownedBooks.Contains(lifeSkillItem.SkillBookId) && lifeSkillItem.SkillBookId >= 0 && context.Random.CheckPercentProb(_lifeSkillQualifications.Items[lifeSkillItem.Type] / 5))
				{
					obj.Add(new TemplateKey(10, lifeSkillItem.SkillBookId));
				}
			}
		}
		if (obj.Count == 0)
		{
			context.AdvanceMonthRelatedData.ItemTemplateKeys.Release(ref obj);
			return;
		}
		TemplateKey random = obj.GetRandom(context.Random);
		context.AdvanceMonthRelatedData.ItemTemplateKeys.Release(ref obj);
		mod.AddItemToCreate(random.ItemType, random.TemplateId, 1);
	}

	private void OfflineGetItemSupply(DataContext context, PeriAdvanceMonthGetSupplyModification mod, int satisfyingThreshold, List<PresetInventoryItem> presetInventory)
	{
		if (GetInventoryTotalValue() > satisfyingThreshold)
		{
			return;
		}
		Dictionary<short, (sbyte, int)> dictionary = context.AdvanceMonthRelatedData.ItemSubTypeStats.Get();
		foreach (PresetInventoryItem item in presetInventory)
		{
			short itemSubType = ItemTemplateHelper.GetItemSubType(item.Type, item.TemplateId);
			sbyte grade = _organizationInfo.Grade;
			sbyte b = (sbyte)Math.Max(0, grade - 3);
			if ((dictionary.TryGetValue(itemSubType, out var value) && value.Item1 >= b) || !context.Random.CheckPercentProb(item.SpawnChance / 2))
			{
				continue;
			}
			short templateIdInGroup = ItemTemplateHelper.GetTemplateIdInGroup(item.Type, item.TemplateId, grade);
			GameData.Domains.Character.Ai.PersonalNeed personalNeed = GameData.Domains.Character.Ai.PersonalNeed.CreatePersonalNeed(10, item.Type, templateIdInGroup);
			mod.PersonalNeedChanged = mod.PersonalNeedChanged || OfflineAddPersonalNeed(personalNeed);
			break;
		}
	}

	private void OfflineGetEquipmentSupply(DataContext context, PeriAdvanceMonthGetSupplyModification mod, PresetEquipmentItemWithProb[] presetEquipments)
	{
		Dictionary<short, (sbyte, int)> dictionary = context.AdvanceMonthRelatedData.ItemSubTypeStats.Get();
		sbyte grade = _organizationInfo.Grade;
		sbyte b = (sbyte)Math.Max(0, grade - 3);
		for (int i = 0; i < presetEquipments.Length; i++)
		{
			PresetEquipmentItemWithProb presetEquipmentItemWithProb = presetEquipments[i];
			if (presetEquipmentItemWithProb.TemplateId >= 0)
			{
				short itemSubType = ItemTemplateHelper.GetItemSubType(presetEquipmentItemWithProb.Type, presetEquipmentItemWithProb.TemplateId);
				if ((!dictionary.TryGetValue(itemSubType, out var value) || value.Item1 < b) && context.Random.CheckPercentProb(presetEquipmentItemWithProb.Prob / 2))
				{
					short templateIdInGroup = ItemTemplateHelper.GetTemplateIdInGroup(presetEquipmentItemWithProb.Type, presetEquipmentItemWithProb.TemplateId, grade);
					GameData.Domains.Character.Ai.PersonalNeed personalNeed = GameData.Domains.Character.Ai.PersonalNeed.CreatePersonalNeed(10, presetEquipmentItemWithProb.Type, templateIdInGroup);
					mod.PersonalNeedChanged = mod.PersonalNeedChanged || OfflineAddPersonalNeed(personalNeed);
					break;
				}
			}
		}
		ArraySegmentList<short> attack = GetCombatSkillEquipment().Attack;
		ArraySegmentList<short>.Enumerator enumerator = attack.GetEnumerator();
		while (enumerator.MoveNext())
		{
			short current = enumerator.Current;
			if (current < 0)
			{
				continue;
			}
			CombatSkillItem combatSkillItem = Config.CombatSkill.Instance[current];
			if (combatSkillItem.MostFittingWeaponID >= 0)
			{
				short itemSubType2 = Config.Weapon.Instance[combatSkillItem.MostFittingWeaponID].ItemSubType;
				if (!dictionary.TryGetValue(itemSubType2, out var value2) || value2.Item1 < b)
				{
					short templateId = ItemDomain.GenerateRandomItemTemplateId(context.Random, 0, combatSkillItem.MostFittingWeaponID, grade);
					mod.AddItemToCreate(0, templateId, 1);
					break;
				}
			}
		}
	}

	private void OfflineGetClothingSupply(DataContext context, PeriAdvanceMonthGetSupplyModification mod, short clothingId)
	{
		ClothingItem clothingItem = Config.Clothing.Instance[clothingId];
		if (clothingItem != null)
		{
			Dictionary<short, (sbyte, int)> dictionary = context.AdvanceMonthRelatedData.ItemSubTypeStats.Get();
			sbyte grade = clothingItem.Grade;
			if ((!dictionary.TryGetValue(clothingItem.ItemSubType, out var value) || value.Item1 < grade) && context.Random.CheckPercentProb(30))
			{
				mod.AddItemToCreate(3, clothingId, 1);
			}
		}
	}

	private unsafe void OfflineRepairEquipments(DataContext context, PeriAdvanceMonthActivePreparationModification mod)
	{
		List<ItemKey> list = ObjectPool<List<ItemKey>>.Instance.Get();
		List<ItemKey> list2 = ObjectPool<List<ItemKey>>.Instance.Get();
		list.Clear();
		list2.Clear();
		ItemKey[] equipment = _equipment;
		foreach (ItemKey itemKey in equipment)
		{
			if (DomainManager.Item.CheckItemNeedRepair(itemKey))
			{
				list.Add(itemKey);
			}
		}
		foreach (var (itemKey3, _) in _inventory.Items)
		{
			if (itemKey3.ItemType == 6)
			{
				ItemBase baseItem = DomainManager.Item.GetBaseItem(itemKey3);
				if (baseItem.GetCurrDurability() > 0)
				{
					list2.Add(itemKey3);
				}
			}
			else if (DomainManager.Item.CheckItemNeedRepair(itemKey3))
			{
				list.Add(itemKey3);
			}
		}
		if (list.Count > 0)
		{
			list2.Sort((ItemKey a, ItemKey itemKey4) => a.TemplateId.CompareTo(itemKey4.TemplateId));
			LifeSkillShorts lifeSkillAttainments = GetLifeSkillAttainments();
			foreach (ItemKey item in list)
			{
				ItemBase itemObj = DomainManager.Item.GetBaseItem(item);
				short currDurability = itemObj.GetCurrDurability();
				short maxDurability = itemObj.GetMaxDurability();
				sbyte resourceType = itemObj.GetResourceType();
				sbyte lifeSkillType = ItemTemplateHelper.GetCraftRequiredLifeSkillType(item.ItemType, item.TemplateId);
				short attainmentRequired = ItemTemplateHelper.GetRepairRequiredAttainment(item.ItemType, item.TemplateId, currDurability);
				EquipmentBase baseEquipment = DomainManager.Item.GetBaseEquipment(item);
				ResourceInts needResources = ItemTemplateHelper.GetRepairNeedResources(baseEquipment.GetMaterialResources(), item, currDurability);
				short charAttainment = lifeSkillAttainments.Items[lifeSkillType];
				int num2 = list2.FindIndex(delegate(ItemKey key)
				{
					CraftToolItem craftToolItem2 = Config.CraftTool.Instance[key.TemplateId];
					short num5 = craftToolItem2.DurabilityCost[itemObj.GetGrade()];
					GameData.Domains.Item.CraftTool element_CraftTools = DomainManager.Item.GetElement_CraftTools(key.Id);
					return element_CraftTools.GetCurrDurability() >= num5 && charAttainment + craftToolItem2.AttainmentBonus >= attainmentRequired && craftToolItem2.RequiredLifeSkillTypes.Contains(lifeSkillType);
				});
				GameData.Domains.Item.CraftTool craftTool = ((num2 >= 0) ? DomainManager.Item.GetElement_CraftTools(list2[num2].Id) : null);
				if (craftTool == null)
				{
					sbyte b;
					for (b = 0; b <= _organizationInfo.Grade; b++)
					{
						CraftToolItem gradeCraftTool = ItemTemplateHelper.GetGradeCraftTool(resourceType, b);
						if (lifeSkillAttainments.Items[lifeSkillType] + gradeCraftTool.AttainmentBonus >= attainmentRequired)
						{
							GameData.Domains.Character.Ai.PersonalNeed personalNeed = GameData.Domains.Character.Ai.PersonalNeed.CreatePersonalNeed(10, 6, gradeCraftTool.TemplateId);
							OfflineAddPersonalNeed(personalNeed);
							mod.PersonalNeedChanged = true;
							break;
						}
					}
					if (b > _organizationInfo.Grade)
					{
						GameData.Domains.Character.Ai.PersonalNeed personalNeed2 = GameData.Domains.Character.Ai.PersonalNeed.CreatePersonalNeed(11, item.ItemType, item.Id);
						OfflineAddPersonalNeed(personalNeed2);
						mod.PersonalNeedChanged = true;
					}
					continue;
				}
				if (!_resources.CheckIsMeet(ref needResources))
				{
					for (int num3 = 0; num3 < 8; num3++)
					{
						int num4 = needResources.Items[num3];
						if (num4 > 0)
						{
							GameData.Domains.Character.Ai.PersonalNeed personalNeed3 = GameData.Domains.Character.Ai.PersonalNeed.CreatePersonalNeed(8, resourceType, num4);
							OfflineAddPersonalNeed(personalNeed3);
							mod.PersonalNeedChanged = true;
						}
					}
					continue;
				}
				CraftToolItem craftToolItem = Config.CraftTool.Instance[craftTool.GetTemplateId()];
				short durabilityCost = craftToolItem.DurabilityCost[itemObj.GetGrade()];
				ItemBase.OfflineRepairItem(craftTool, itemObj, itemObj.GetMaxDurability(), durabilityCost);
				_resources = _resources.Subtract(ref needResources);
				mod.ResourcesChanged = true;
				PeriAdvanceMonthActivePreparationModification periAdvanceMonthActivePreparationModification = mod;
				if (periAdvanceMonthActivePreparationModification.ItemsFixed == null)
				{
					periAdvanceMonthActivePreparationModification.ItemsFixed = new List<ItemBase>();
				}
				mod.ItemsFixed.Add(itemObj);
				periAdvanceMonthActivePreparationModification = mod;
				if (periAdvanceMonthActivePreparationModification.CraftToolsUsed == null)
				{
					periAdvanceMonthActivePreparationModification.CraftToolsUsed = new HashSet<int>();
				}
				mod.CraftToolsUsed.Add(craftTool.GetId());
				if (craftTool.GetCurrDurability() <= 0)
				{
					list2.Remove(craftTool.GetItemKey());
				}
			}
		}
		ObjectPool<List<ItemKey>>.Instance.Return(list);
		ObjectPool<List<ItemKey>>.Instance.Return(list2);
	}

	private void OfflineFeedAnimalCarrier(IRandomSource random, PeriAdvanceMonthActivePreparationModification mod)
	{
		int num = 0;
		ItemKey itemKey = _equipment[7];
		if (itemKey.IsValid())
		{
			int num2 = GetNeedTamePoint(itemKey);
			if (num2 > 0)
			{
				mod.FeedingCarrierKey = itemKey;
				num = num2;
			}
		}
		ItemKey key;
		int value;
		foreach (KeyValuePair<ItemKey, int> item in _inventory.Items)
		{
			item.Deconstruct(out key, out value);
			ItemKey itemKey2 = key;
			int num3 = GetNeedTamePoint(itemKey2);
			if (num3 > num)
			{
				num = num3;
				mod.FeedingCarrierKey = itemKey2;
			}
		}
		if (!mod.FeedingCarrierKey.IsValid())
		{
			return;
		}
		int num4 = -128;
		foreach (KeyValuePair<ItemKey, int> item2 in _inventory.Items)
		{
			item2.Deconstruct(out key, out value);
			ItemKey itemKey3 = key;
			int num5 = GetFeedTamePoint(mod.FeedingCarrierKey, itemKey3);
			if (num5 > 0 && Math.Abs(num4 - num) >= Math.Abs(num5 - num))
			{
				num4 = num5;
				mod.FeedingFoodKey = itemKey3;
			}
		}
		if (!mod.FeedingFoodKey.IsValid())
		{
			CarrierItem carrierItem = Config.Carrier.Instance[mod.FeedingCarrierKey.TemplateId];
			if (carrierItem.LoveFoodType.CheckIndex(0))
			{
				short random2 = carrierItem.LoveFoodType.GetRandom(random);
				GameData.Domains.Character.Ai.PersonalNeed personalNeed = GameData.Domains.Character.Ai.PersonalNeed.CreatePersonalNeed(10, 5, random2);
				mod.PersonalNeedChanged = mod.PersonalNeedChanged || OfflineAddPersonalNeed(personalNeed);
			}
		}
		static int GetFeedTamePoint(ItemKey carrierKey, ItemKey foodKey)
		{
			if (!ItemTemplateHelper.IsFeedingAble(foodKey.ItemType, foodKey.TemplateId))
			{
				return 0;
			}
			return GameData.Domains.Extra.SharedMethods.GetFoodAddCarrierTamePoint(carrierKey.TemplateId, foodKey.TemplateId);
		}
		static int GetNeedTamePoint(ItemKey itemKey4)
		{
			if (!ItemTemplateHelper.HasCarrierTame(itemKey4.ItemType, itemKey4.TemplateId))
			{
				return 0;
			}
			int carrierTamePoint = DomainManager.Extra.GetCarrierTamePoint(itemKey4.Id);
			if (carrierTamePoint < 0)
			{
				return 0;
			}
			int carrierMaxTamePoint = DomainManager.Extra.GetCarrierMaxTamePoint(itemKey4.Id);
			if (carrierTamePoint >= carrierMaxTamePoint)
			{
				return 0;
			}
			return carrierMaxTamePoint - carrierTamePoint;
		}
	}

	private unsafe void OfflineAddPoisonToEquipments(DataContext context, PeriAdvanceMonthActivePreparationModification mod, ItemKey[] equipments = null)
	{
		if (!Config.Organization.Instance[_organizationInfo.OrgTemplateId].AllowPoisoning)
		{
			return;
		}
		sbyte behaviorType = GetBehaviorType();
		bool* ptr = stackalloc bool[6];
		GetAttackSkillPoisonTypes(ptr);
		bool flag = false;
		for (int i = 0; i < 6; i++)
		{
			if (ptr[i])
			{
				flag = true;
				break;
			}
		}
		if (!flag)
		{
			return;
		}
		int num = 10 + AiHelper.ActivePreparationConstants.AddPoisonBonusChance[behaviorType];
		foreach (GameData.Domains.Character.Ai.PersonalNeed personalNeed2 in _personalNeeds)
		{
			if (personalNeed2.TemplateId == 12 && ptr[personalNeed2.PoisonType])
			{
				num = 100;
			}
		}
		short aiActionRateAdjust = DomainManager.Extra.GetAiActionRateAdjust(_id, 9, -1);
		if (!context.Random.CheckPercentProb(num + aiActionRateAdjust))
		{
			return;
		}
		short lifeSkillAttainment = GetLifeSkillAttainment(9);
		ItemKey* ptr2 = stackalloc ItemKey[6];
		byte* intPtr = stackalloc byte[6];
		// IL initblk instruction
		Unsafe.InitBlock(intPtr, 255, 6);
		sbyte* ptr3 = (sbyte*)intPtr;
		int num2 = 0;
		int num3 = 0;
		foreach (var (itemKey2, _) in _inventory.Items)
		{
			if (itemKey2.ItemType != 8)
			{
				continue;
			}
			MedicineItem medicineItem = Config.Medicine.Instance[itemKey2.TemplateId];
			if (medicineItem.EffectType != EMedicineEffectType.ApplyPoison)
			{
				continue;
			}
			sbyte poisonType = medicineItem.PoisonType;
			short num5 = GlobalConfig.Instance.PoisonAttainments[medicineItem.Grade];
			if (lifeSkillAttainment < num5 || medicineItem.Grade <= ptr3[poisonType])
			{
				continue;
			}
			if (ptr3[poisonType] < 0)
			{
				if (ptr[poisonType])
				{
					num2++;
				}
				else
				{
					num3++;
				}
			}
			ptr2[poisonType] = itemKey2;
			ptr3[poisonType] = medicineItem.Grade;
		}
		if (num2 == 0)
		{
			(ItemKey, sbyte level, short value) leastPoisonedEquippedWeaponOrArmor = GetLeastPoisonedEquippedWeaponOrArmor();
			ItemKey item = leastPoisonedEquippedWeaponOrArmor.Item1;
			sbyte item2 = leastPoisonedEquippedWeaponOrArmor.level;
			short item3 = leastPoisonedEquippedWeaponOrArmor.value;
			sbyte b = AiHelper.ActivePreparationConstants.GradePoisonLevel[_organizationInfo.Grade];
			short num6 = AiHelper.ActivePreparationConstants.GradePoisonValue[_organizationInfo.Grade];
			if (b <= item2 && (b != item2 || num6 <= item3))
			{
				return;
			}
			for (sbyte b2 = 0; b2 < 6; b2++)
			{
				if (ptr[b2])
				{
					GameData.Domains.Character.Ai.PersonalNeed personalNeed = GameData.Domains.Character.Ai.PersonalNeed.CreatePersonalNeed(12, b2);
					OfflineAddPersonalNeed(personalNeed);
					mod.PersonalNeedChanged = true;
				}
			}
			return;
		}
		int num7 = num2 + num3;
		ItemKey* ptr4 = stackalloc ItemKey[num7];
		int num8 = 0;
		int num9 = num2;
		for (sbyte b3 = 0; b3 < 6; b3++)
		{
			if (ptr3[b3] >= 0)
			{
				ItemKey itemKey3 = ptr2[b3];
				if (ptr[b3])
				{
					ptr4[num8] = itemKey3;
					num8++;
				}
				else
				{
					ptr4[num9] = itemKey3;
					num9++;
				}
				Tester.Assert(itemKey3.ItemType == 8);
			}
		}
		Tester.Assert(num8 == num2);
		Tester.Assert(num9 == num7);
		if (num2 > 0)
		{
			CollectionUtils.Shuffle(context.Random, ptr4, num2);
		}
		if (num3 > 0)
		{
			CollectionUtils.Shuffle(context.Random, ptr4 + num2, num3);
		}
		sbyte* equipSlotArr = stackalloc sbyte[3] { 0, 1, 2 };
		(mod.EquipmentSlotToAddPoison, mod.PoisonsToUse) = SelectEquipmentSlotAndPoisonsToAdd(context.Random, ptr4, num7, equipSlotArr, 3, equipments);
		if (mod.EquipmentSlotToAddPoison >= 0)
		{
			RemovePersonalNeedOnAddPoisonToItem(mod);
			return;
		}
		sbyte* equipSlotArr2 = stackalloc sbyte[4] { 3, 5, 6, 7 };
		(mod.EquipmentSlotToAddPoison, mod.PoisonsToUse) = SelectEquipmentSlotAndPoisonsToAdd(context.Random, ptr4, num7, equipSlotArr2, 4, equipments);
		if (mod.EquipmentSlotToAddPoison >= 0)
		{
			RemovePersonalNeedOnAddPoisonToItem(mod);
			return;
		}
		sbyte* equipSlotArr3 = stackalloc sbyte[3] { 8, 9, 10 };
		(mod.EquipmentSlotToAddPoison, mod.PoisonsToUse) = SelectEquipmentSlotAndPoisonsToAdd(context.Random, ptr4, num7, equipSlotArr3, 3, equipments);
		if (mod.EquipmentSlotToAddPoison >= 0)
		{
			RemovePersonalNeedOnAddPoisonToItem(mod);
		}
	}

	private void RemovePersonalNeedOnAddPoisonToItem(PeriAdvanceMonthActivePreparationModification mod)
	{
		if (mod.PoisonsToUse == null)
		{
			return;
		}
		ItemKey[] poisonsToUse = mod.PoisonsToUse;
		for (int i = 0; i < poisonsToUse.Length; i++)
		{
			ItemKey itemKey = poisonsToUse[i];
			MedicineItem config = Config.Medicine.Instance[itemKey.TemplateId];
			int num = _personalNeeds.FindIndex((GameData.Domains.Character.Ai.PersonalNeed n) => n.TemplateId == 12 && n.PoisonType == config.PoisonType);
			if (num >= 0)
			{
				_personalNeeds.RemoveAt(num);
				mod.PersonalNeedChanged = true;
			}
		}
	}

	private unsafe (ItemKey, sbyte level, short value) GetLeastPoisonedEquippedWeaponOrArmor()
	{
		sbyte* ptr = stackalloc sbyte[7] { 0, 1, 2, 3, 5, 6, 7 };
		ItemKey item = ItemKey.Invalid;
		sbyte b = 3;
		short num = short.MaxValue;
		for (int i = 0; i < 7; i++)
		{
			sbyte b2 = ptr[i];
			ItemKey itemKey = _equipment[b2];
			if (!itemKey.IsValid())
			{
				continue;
			}
			if (!ModificationStateHelper.IsActive(itemKey.ModificationState, 1))
			{
				return (itemKey, level: 0, value: 0);
			}
			sbyte b3 = 0;
			short num2 = 0;
			PoisonsAndLevels attachedPoisons = DomainManager.Item.GetAttachedPoisons(itemKey);
			for (sbyte b4 = 0; b4 < 6; b4++)
			{
				if (attachedPoisons.Levels[b4] > 0)
				{
					b3 = attachedPoisons.Levels[b4];
					num2 = attachedPoisons.Values[b4];
					break;
				}
			}
			if (b3 <= b && (b3 != b || num2 <= num))
			{
				item = itemKey;
				b = b3;
				num = num2;
			}
		}
		return (item, level: b, value: num);
	}

	public unsafe void GetAttackSkillPoisonTypes(bool* poisonTypes)
	{
		for (sbyte b = 0; b < 6; b++)
		{
			poisonTypes[b] = false;
		}
		ArraySegmentList<short> attack = GetCombatSkillEquipment().Attack;
		ArraySegmentList<short>.Enumerator enumerator = attack.GetEnumerator();
		while (enumerator.MoveNext())
		{
			short current = enumerator.Current;
			if (current < 0)
			{
				continue;
			}
			CombatSkillItem combatSkillItem = Config.CombatSkill.Instance[current];
			if (!combatSkillItem.Poisons.IsNonZero())
			{
				continue;
			}
			for (sbyte b2 = 0; b2 < 6; b2++)
			{
				if (combatSkillItem.Poisons.Values[b2] > 0)
				{
					poisonTypes[b2] = true;
				}
			}
		}
	}

	private unsafe sbyte SelectEquipmentInArrayToAddPoisonOn(IRandomSource random, MedicineItem poisonConfig, sbyte* equipmentArr, int length, ItemKey[] equipments = null)
	{
		if (equipments == null)
		{
			equipments = _equipment;
		}
		CollectionUtils.Shuffle(random, equipmentArr, length);
		for (int i = 0; i < length; i++)
		{
			sbyte b = equipmentArr[i];
			ItemKey itemKey = equipments[b];
			if (itemKey.IsValid() && ItemTemplateHelper.IsPoisonable(itemKey.ItemType, itemKey.TemplateId))
			{
				EquipmentBase baseEquipment = DomainManager.Item.GetBaseEquipment(itemKey);
				byte modificationState = baseEquipment.GetModificationState();
				if (!ModificationStateHelper.IsActive(modificationState, 1))
				{
					return b;
				}
			}
		}
		return -1;
	}

	private unsafe (sbyte, ItemKey[]) SelectEquipmentSlotAndPoisonsToAdd(IRandomSource random, ItemKey* poisonArr, int poisonLength, sbyte* equipSlotArr, int equipSlotLength, ItemKey[] equipments = null)
	{
		if (equipments == null)
		{
			equipments = _equipment;
		}
		Span<ItemKey> span = stackalloc ItemKey[3];
		SpanList<ItemKey> spanList = span;
		CollectionUtils.Shuffle(random, equipSlotArr, equipSlotLength);
		for (int i = 0; i < equipSlotLength; i++)
		{
			sbyte b = equipSlotArr[i];
			ItemKey itemKey = equipments[b];
			if (!itemKey.IsValid() || !ItemTemplateHelper.IsPoisonable(itemKey.ItemType, itemKey.TemplateId))
			{
				continue;
			}
			EquipmentBase baseEquipment = DomainManager.Item.GetBaseEquipment(itemKey);
			byte modificationState = baseEquipment.GetModificationState();
			if (ModificationStateHelper.IsActive(modificationState, 1))
			{
				FullPoisonEffects poisonEffects = DomainManager.Item.GetPoisonEffects(itemKey);
				PoisonsAndLevels allPoisonsAndLevels = poisonEffects.GetAllPoisonsAndLevels();
				int num = (poisonEffects.IsTwoPoisonsMix() ? 2 : ((!poisonEffects.IsThreePoisonsMix()) ? 1 : 3));
				if (num >= 3)
				{
					continue;
				}
				for (int j = 0; j < poisonLength; j++)
				{
					ItemKey value = poisonArr[j];
					MedicineItem medicineItem = Config.Medicine.Instance[value.TemplateId];
					sbyte poisonType = medicineItem.PoisonType;
					sbyte b2 = allPoisonsAndLevels.Levels[poisonType];
					short num2 = allPoisonsAndLevels.Values[poisonType];
					if (num2 <= 0 && (b2 < medicineItem.EffectThresholdValue || (b2 == medicineItem.EffectThresholdValue && num2 < medicineItem.EffectValue)))
					{
						spanList.Add(value);
						if (spanList.Count + num >= 3)
						{
							break;
						}
					}
				}
				if (spanList.Count > 0)
				{
					return (b, spanList.ToArray());
				}
				continue;
			}
			if (random.CheckPercentProb(GetPersonality(1)))
			{
				for (int k = 0; k < poisonLength; k++)
				{
					ItemKey key = poisonArr[k];
					int num3 = _inventory.Items[key];
					if (num3 >= 3)
					{
						spanList.Add(poisonArr[k]);
						spanList.Add(poisonArr[k]);
						spanList.Add(poisonArr[k]);
						return (b, spanList.ToArray());
					}
				}
			}
			int num4 = Math.Min(poisonLength, 3);
			for (int l = 0; l < num4; l++)
			{
				spanList.Add(poisonArr[l]);
			}
			return (b, spanList.ToArray());
		}
		return (-1, null);
	}

	private int PoisonCompare(ItemKey itemKeyA, ItemKey itemKeyB)
	{
		return Config.Medicine.Instance[itemKeyA.TemplateId].Grade.CompareTo(Config.Medicine.Instance[itemKeyB.TemplateId].Grade);
	}

	private unsafe void OfflineLoseResources(PeriAdvanceMonthUpdateStatusModification mod)
	{
		int materialResourceMaxCount = DomainManager.Taiwu.GetMaterialResourceMaxCount();
		for (sbyte b = 0; b < 6; b++)
		{
			int num = _resources.Items[b];
			if (num > materialResourceMaxCount)
			{
				int num2 = Math.Max((num - materialResourceMaxCount) / 2, 1);
				_resources.Items[b] = num - num2;
				mod.ResourcesChanged = true;
			}
		}
	}

	private int OfflineLoseOverloadedItems(DataContext context, PeriAdvanceMonthPassivePreparationModification mod)
	{
		int num = GetCurrInventoryLoad() - GetMaxInventoryLoad();
		if (num <= 0)
		{
			return 0;
		}
		Location location = (_location.IsValid() ? _location : GetValidLocation());
		List<MapBlockData> list = ObjectPool<List<MapBlockData>>.Instance.Get();
		DomainManager.Map.GetRealNeighborBlocks(location.AreaId, location.BlockId, list, 2, includeCenter: true);
		List<(ItemBase, int)> obj = context.AdvanceMonthRelatedData.ItemsWithAmount.Occupy();
		CharacterDomain.GetLostItemsDueToOverload(context, num, _inventory.Items, obj, _id == DomainManager.Taiwu.GetTaiwuCharId());
		mod.ItemsToBeLost = new List<(MapBlockData, ItemKey, int)>();
		int num2 = 0;
		foreach (var item3 in obj)
		{
			ItemBase item = item3.Item1;
			int item2 = item3.Item2;
			ItemKey itemKey = item.GetItemKey();
			_inventory.OfflineRemove(itemKey, item2);
			MapBlockData random = list.GetRandom(context.Random);
			mod.ItemsToBeLost.Add((random, itemKey, item2));
			num2 += item.GetHappinessChange() * item2;
		}
		context.AdvanceMonthRelatedData.ItemsWithAmount.Release(ref obj);
		ObjectPool<List<MapBlockData>>.Instance.Return(list);
		return -num2;
	}

	[Obsolete]
	private void ApplyReadingResult(DataContext context, GameData.Domains.Item.SkillBook book, int learnedSkillIndex, byte readingPage, bool isSucceed)
	{
		ApplyBookReadingResult(context, book, learnedSkillIndex, readingPage, (sbyte)(isSucceed ? 1 : 0));
	}

	private void ApplyBookReadingResult(DataContext context, GameData.Domains.Item.SkillBook book, int learnedSkillIndex, byte readingPage, sbyte succeedPageCount)
	{
		int currDate = DomainManager.World.GetCurrDate();
		LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
		ItemKey itemKey = book.GetItemKey();
		TryApplyAttachedPoison(context, itemKey);
		book.SetCurrDurability((short)(book.GetCurrDurability() - 1), context);
		if (book.GetCurrDurability() <= 0)
		{
			RemoveInventoryItem(context, itemKey, 1, deleteItem: true);
			GameData.Domains.Character.Ai.PersonalNeed personalNeed = GameData.Domains.Character.Ai.PersonalNeed.CreatePersonalNeed(10, 10, book.GetTemplateId());
			AddPersonalNeed(context, personalNeed);
		}
		if (succeedPageCount > 0)
		{
			SetExp(_exp, context);
			byte item;
			if (book.IsCombatSkillBook())
			{
				short combatSkillTemplateId = book.GetCombatSkillTemplateId();
				byte pageTypes = book.GetPageTypes();
				if (learnedSkillIndex < 0)
				{
					learnedSkillIndex = _learnedCombatSkills.Count;
					byte pageInternalIndex = CombatSkillStateHelper.GetPageInternalIndex(SkillBookStateHelper.GetOutlinePageType(pageTypes), SkillBookStateHelper.GetNormalPageType(pageTypes, readingPage), readingPage);
					LearnNewCombatSkill(context, combatSkillTemplateId, (ushort)(1 << (int)pageInternalIndex));
					readingPage++;
					succeedPageCount--;
				}
				GameData.Domains.CombatSkill.CombatSkill element_CombatSkills = DomainManager.CombatSkill.GetElement_CombatSkills(new CombatSkillKey(_id, combatSkillTemplateId));
				while (succeedPageCount > 0 && readingPage < 6)
				{
					byte pageInternalIndex2 = CombatSkillStateHelper.GetPageInternalIndex(SkillBookStateHelper.GetOutlinePageType(pageTypes), SkillBookStateHelper.GetNormalPageType(pageTypes, readingPage), readingPage);
					element_CombatSkills.SetReadingState(CombatSkillStateHelper.SetPageRead(element_CombatSkills.GetReadingState(), pageInternalIndex2), context);
					DomainManager.CombatSkill.TryActivateCombatSkillBookPageWhenSetReadingState(context, _id, combatSkillTemplateId, pageInternalIndex2);
					readingPage++;
					succeedPageCount--;
				}
				item = GetCombatSkillBookCurrReadingInfo(book).readingPage;
			}
			else
			{
				short lifeSkillTemplateId = book.GetLifeSkillTemplateId();
				if (learnedSkillIndex < 0)
				{
					learnedSkillIndex = _learnedLifeSkills.Count;
					LearnNewLifeSkill(context, lifeSkillTemplateId, (byte)(1 << (int)readingPage));
					readingPage++;
					succeedPageCount--;
				}
				while (succeedPageCount > 0 && readingPage < 5)
				{
					ReadLifeSkillPage(context, learnedSkillIndex, readingPage);
					readingPage++;
					succeedPageCount--;
				}
				item = GetLifeSkillBookCurrReadingInfo(book).readingPage;
			}
			if (item < book.GetPageCount() && SkillBookStateHelper.GetPageIncompleteState(book.GetPageIncompleteState(), item) == 2)
			{
				GameData.Domains.Character.Ai.PersonalNeed personalNeed2 = GameData.Domains.Character.Ai.PersonalNeed.CreatePersonalNeed(17, book.GetId());
				AddPersonalNeed(context, personalNeed2);
			}
		}
		else
		{
			short readingAttainmentRequirement = SkillGradeData.Instance[book.GetGrade()].ReadingAttainmentRequirement;
			GameData.Domains.Character.Ai.PersonalNeed personalNeed3 = (book.IsCombatSkillBook() ? GameData.Domains.Character.Ai.PersonalNeed.CreatePersonalNeed(14, book.GetCombatSkillType(), readingAttainmentRequirement) : GameData.Domains.Character.Ai.PersonalNeed.CreatePersonalNeed(15, book.GetLifeSkillType(), readingAttainmentRequirement));
			AddPersonalNeed(context, personalNeed3);
			GameData.Domains.Character.Ai.PersonalNeed personalNeed4 = GameData.Domains.Character.Ai.PersonalNeed.CreatePersonalNeed(17, book.GetId());
			AddPersonalNeed(context, personalNeed4);
			SecretInformationCollection secretInformationCollection = DomainManager.Information.GetSecretInformationCollection();
			int dataOffset = secretInformationCollection.AddReadBookFail(_id, (ulong)itemKey);
			DomainManager.Information.AddSecretInformationMetaData(context, dataOffset);
		}
	}

	private void OfflineUpdateReadingProgress(DataContext context, PeriAdvanceMonthSelfImprovementModification mod, GameData.Domains.Item.SkillBook book, int learnedSkillIndex, byte readingPage)
	{
		sbyte lifeSkillType = book.GetLifeSkillType();
		sbyte combatSkillType = book.GetCombatSkillType();
		short attainment = ((lifeSkillType >= 0) ? GetLifeSkillAttainment(lifeSkillType) : GetCombatSkillAttainment(combatSkillType));
		sbyte personality = GetPersonality(1);
		byte b = readingPage;
		byte pageCount = book.GetPageCount();
		int num = GetTotalReadingPoint(book.GetGrade(), attainment, personality);
		if (_organizationInfo.SettlementId >= 0 && OrganizationDomain.IsSect(_organizationInfo.OrgTemplateId))
		{
			Settlement settlement = DomainManager.Organization.GetSettlement(_organizationInfo.SettlementId);
			num = num * settlement.GetMemberSelfImproveSpeedFactor() / 100;
		}
		if (DomainManager.Building.IsCharacterParticipantFeast(_id, out var value))
		{
			FeastItem feastItem = Config.Feast.Instance[value.Item1];
			int num2 = ((lifeSkillType >= 0) ? feastItem.ReadLifeSkillBook : feastItem.ReadCombatSkillBook);
			num += num * num2 / 100 * value.Item2 / 100;
		}
		if (DomainManager.SpecialEffect.ModifyData(_id, -1, 260, dataValue: true))
		{
			while (b < pageCount)
			{
				sbyte pageIncompleteState = SkillBookStateHelper.GetPageIncompleteState(book.GetPageIncompleteState(), readingPage);
				if (pageIncompleteState == 2)
				{
					break;
				}
				sbyte b2 = SkillBookPageIncompleteState.ReadingPointCost[pageIncompleteState];
				if (b2 > num)
				{
					break;
				}
				num -= b2;
				b++;
			}
		}
		sbyte item = (sbyte)(b - readingPage);
		mod.ReadingResult = (readingBook: book, learnedSkillIndex: learnedSkillIndex, page: readingPage, succeedPageCount: item);
		if (mod.ReadingResult.readingBook != null && mod.ReadingResult.succeedPageCount > 0)
		{
			_exp += SkillGradeData.Instance[book.GetGrade()].ReadingExpGainPerPage * mod.ReadingResult.succeedPageCount;
		}
	}

	public (int learnedSkillIndex, byte readingPage) GetCombatSkillBookCurrReadingInfo(GameData.Domains.Item.SkillBook book)
	{
		short combatSkillTemplateId = book.GetCombatSkillTemplateId();
		int num = _learnedCombatSkills.IndexOf(combatSkillTemplateId);
		if (num < 0)
		{
			return (learnedSkillIndex: num, readingPage: 0);
		}
		CombatSkillKey objectId = new CombatSkillKey(_id, _learnedCombatSkills[num]);
		GameData.Domains.CombatSkill.CombatSkill element_CombatSkills = DomainManager.CombatSkill.GetElement_CombatSkills(objectId);
		byte pageTypes = book.GetPageTypes();
		sbyte outlinePageType = SkillBookStateHelper.GetOutlinePageType(pageTypes);
		ushort readingState = element_CombatSkills.GetReadingState();
		for (byte b = 0; b < 6; b++)
		{
			byte pageInternalIndex = CombatSkillStateHelper.GetPageInternalIndex(outlinePageType, SkillBookStateHelper.GetNormalPageType(pageTypes, b), b);
			if (!CombatSkillStateHelper.IsPageRead(readingState, pageInternalIndex))
			{
				return (learnedSkillIndex: num, readingPage: b);
			}
		}
		return (learnedSkillIndex: num, readingPage: 6);
	}

	public (int learnedSkillIndex, byte readingPage) GetLifeSkillBookCurrReadingInfo(GameData.Domains.Item.SkillBook book)
	{
		short lifeSkillTemplateId = book.GetLifeSkillTemplateId();
		int num = FindLearnedLifeSkillIndex(lifeSkillTemplateId);
		if (num < 0)
		{
			return (learnedSkillIndex: num, readingPage: 0);
		}
		LifeSkillItem lifeSkillItem = _learnedLifeSkills[num];
		for (byte b = 0; b < 5; b++)
		{
			if (!lifeSkillItem.IsPageRead(b))
			{
				return (learnedSkillIndex: num, readingPage: b);
			}
		}
		return (learnedSkillIndex: num, readingPage: 5);
	}

	public static int GetTaughtNewSkillSuccessRate(sbyte grade, short qualification, short attainment, sbyte cleverness)
	{
		SkillGradeDataItem skillGradeDataItem = SkillGradeData.Instance[grade];
		if (attainment > skillGradeDataItem.ReadingAttainmentRequirement)
		{
			return 100;
		}
		if (qualification > skillGradeDataItem.PracticeQualificationRequirement)
		{
			return 100;
		}
		return Math.Max((cleverness + attainment * 100 / skillGradeDataItem.ReadingAttainmentRequirement) / 2, (cleverness + qualification * 100 / skillGradeDataItem.PracticeQualificationRequirement) / 4);
	}

	public static int GetReadingSuccessRate(sbyte grade, sbyte incompleteState, short attainment, sbyte cleverness)
	{
		short readingAttainmentRequirement = SkillGradeData.Instance[grade].ReadingAttainmentRequirement;
		sbyte b = SkillBookPageIncompleteState.BaseReadingSuccessRate[incompleteState];
		if (attainment >= readingAttainmentRequirement)
		{
			return b;
		}
		return b * (cleverness + attainment * 100 / readingAttainmentRequirement) / 200;
	}

	private static int GetTotalReadingPoint(sbyte grade, short attainment, sbyte cleverness)
	{
		short readingAttainmentRequirement = SkillGradeData.Instance[grade].ReadingAttainmentRequirement;
		return 100 + (100 + cleverness) * attainment / readingAttainmentRequirement / 2;
	}

	private bool OfflineUpdatePregnantState(DataContext context, PeriAdvanceMonthUpdateStatusModification mod)
	{
		if (!_featureIds.Contains(197) || DomainManager.Character.TryGetElement_CrossAreaMoveInfos((_leaderId >= 0) ? _leaderId : _id, out var _))
		{
			return true;
		}
		mod.PregnantStateModification = new PregnantStateModification();
		DomainManager.Character.ParallelUpdatePregnantState(context, this, mod.PregnantStateModification);
		if (mod.PregnantStateModification.State == PregnantStateModification.ChildState.AliveHuman)
		{
			DomainManager.Character.ParallelCreateNewbornChildren(context, this, mod.PregnantStateModification.Dystocia, mod.PregnantStateModification.LostMother);
		}
		else if (mod.PregnantStateModification.State == PregnantStateModification.ChildState.Dead)
		{
			sbyte b = AiHelper.UpdateStatusConstants.LostChildHappinessChange[GetBehaviorType()];
			_happiness = (sbyte)Math.Clamp(_happiness + b, -119, 119);
			_health += -72;
			mod.HappinessChanged = true;
		}
		if (mod.PregnantStateModification.State != PregnantStateModification.ChildState.Invalid)
		{
			_featureIds.Remove(197);
			mod.FeaturesChanged = true;
			mod.PersonalNeedsChanged = true;
		}
		if (!mod.PregnantStateModification.LostMother)
		{
			return true;
		}
		_health = 0;
		return false;
	}

	private sbyte GetXiangshuInfectionDelta()
	{
		int num = DomainManager.SpecialEffect.ModifyValue(_id, 262, 0);
		if (!CanBeXiangshuInfected())
		{
			return (sbyte)Math.Clamp(num, -128, 127);
		}
		int num2 = num;
		sbyte happinessType = HappinessType.GetHappinessType(_happiness);
		if (1 == 0)
		{
		}
		int num3 = happinessType switch
		{
			0 => GlobalConfig.Instance.XiangshuInfectionAddSpeed[0], 
			1 => GlobalConfig.Instance.XiangshuInfectionAddSpeed[1], 
			2 => GlobalConfig.Instance.XiangshuInfectionAddSpeed[2], 
			_ => 0, 
		};
		if (1 == 0)
		{
		}
		num = num2 + num3;
		foreach (short featureId in _featureIds)
		{
			if (!IgnoreFeature(featureId))
			{
				CharacterFeatureItem characterFeatureItem = CharacterFeature.Instance[featureId];
				num += characterFeatureItem.XiangshuInfectionChange;
			}
		}
		if (num > 127)
		{
			num = 127;
		}
		return (sbyte)num;
	}

	private void OfflineUpdateEatingItems(DataContext context, PeriAdvanceMonthUpdateStatusModification mod)
	{
		if (_eatingItems.UpdateDurations(context.AdvanceMonthRelatedData.WorldItemsToBeRemoved, ref mod.RemovedWugs, ref mod.RemovedWugKings))
		{
			mod.EatingItemsChanged = true;
		}
		OfflineUpdateWugKings(context, mod);
	}

	private void OfflineUpdateWugKings(DataContext context, PeriAdvanceMonthUpdateStatusModification mod)
	{
		List<ItemKey> removedWugKings = mod.RemovedWugKings;
		if (removedWugKings == null || removedWugKings.Count <= 0)
		{
			return;
		}
		for (int i = 0; i < mod.RemovedWugKings.Count; i++)
		{
			if (ChangeFeatureByWugKing(context.Random))
			{
				mod.FeaturesChanged = true;
				continue;
			}
			if (mod.RemovedSafetyWugKings == null)
			{
				mod.RemovedSafetyWugKings = new List<ItemKey>();
			}
			mod.RemovedSafetyWugKings.Add(mod.RemovedWugKings[i]);
		}
		if (mod.RemovedSafetyWugKings == null)
		{
			return;
		}
		foreach (ItemKey removedSafetyWugKing in mod.RemovedSafetyWugKings)
		{
			mod.RemovedWugKings.Remove(removedSafetyWugKing);
		}
	}

	private unsafe void OfflineUpdateEatingItemEffect(DataContext context, PeriAdvanceMonthUpdateStatusModification mod)
	{
		for (int i = 0; i < 9; i++)
		{
			ItemKey itemKey = (ItemKey)_eatingItems.ItemKeys[i];
			if (!itemKey.IsValid())
			{
				continue;
			}
			short num = _eatingItems.Durations[i];
			short eatableItemDuration = ItemTemplateHelper.GetEatableItemDuration(itemKey.ItemType, itemKey.TemplateId);
			if (ModificationStateHelper.IsActive(itemKey.ModificationState, 1))
			{
				if (!DomainManager.Item.PoisonEffects.TryGetValue(itemKey.Id, out var value))
				{
					continue;
				}
				PoisonsAndLevels delta = value.GetAllPoisonsAndLevels();
				short medicineTemplateId = value.GetMedicineTemplateId();
				if (medicineTemplateId >= 0)
				{
					eatableItemDuration += Config.Medicine.Instance[medicineTemplateId].Duration;
					for (sbyte b = 0; b < 6; b++)
					{
						int num2 = PoisonsAndLevels.CalcApplyItemPoisonAmount(delta.Values[b], delta.Levels[b]) / 2 * num / eatableItemDuration;
						delta.Values[b] = (short)num2;
					}
					OfflineChangePoisoned(ref delta);
					mod.PoisonedChanged = true;
				}
			}
			else
			{
				if (itemKey.ItemType != 8)
				{
					continue;
				}
				MedicineItem medicineItem = Config.Medicine.Instance[itemKey.TemplateId];
				MedicineEatingInstantEffect config = new MedicineEatingInstantEffect(medicineItem);
				switch (medicineItem.EffectType)
				{
				case EMedicineEffectType.RecoverOuterInjury:
					CalcMedicineEffect_RecoverInjury(ref _injuries, context.Random, inner: false, ref config);
					mod.InjuriesChanged = true;
					break;
				case EMedicineEffectType.RecoverInnerInjury:
					CalcMedicineEffect_RecoverInjury(ref _injuries, context.Random, inner: true, ref config);
					mod.InjuriesChanged = true;
					break;
				case EMedicineEffectType.ApplyPoison:
				{
					sbyte poisonType2 = medicineItem.PoisonType;
					sbyte b3 = (sbyte)medicineItem.EffectThresholdValue;
					int delta3 = PoisonsAndLevels.CalcApplyItemPoisonAmount(medicineItem.EffectValue, b3) / 2 * num / eatableItemDuration;
					if (OfflineChangePoisoned(poisonType2, b3, delta3))
					{
						mod.PoisonedChanged = true;
					}
					break;
				}
				case EMedicineEffectType.DetoxPoison:
				{
					sbyte poisonType = config.PoisonType;
					sbyte b2 = (sbyte)config.EffectThresholdValue;
					int num3 = GetPoisoned().Items[poisonType];
					if (b2 >= PoisonsAndLevels.CalcPoisonedLevel(num3))
					{
						int delta2 = -CalcMedicineEffectDelta(num3, config.EffectValue, config.EffectIsPercentage);
						if (OfflineChangePoisoned(poisonType, b2, delta2))
						{
							mod.PoisonedChanged = true;
						}
					}
					break;
				}
				}
			}
		}
	}

	private void OfflineUpdateFeaturePoisons(DataContext context, PeriAdvanceMonthUpdateStatusModification mod)
	{
		if (!_featureIds.Contains(637))
		{
			return;
		}
		int num = DomainManager.Extra.GetKongsangCharacterFeaturePoisonedProb();
		if (num < GlobalConfig.Instance.KongsangCharacterFeaturePoisonedProbParm[0])
		{
			num = GlobalConfig.Instance.KongsangCharacterFeaturePoisonedProbParm[0];
		}
		if (context.Random.CheckPercentProb(num))
		{
			MedicineItem medicineItem = Config.Medicine.Instance[(short)51];
			sbyte b = (sbyte)medicineItem.EffectThresholdValue;
			short num2 = PoisonsAndLevels.CalcApplyItemPoisonAmount(medicineItem.EffectValue, b);
			sbyte b2 = (sbyte)context.Random.Next(6);
			int num3 = context.Random.Next(1, 3);
			sbyte b3 = (sbyte)(b2 + num3);
			if (b3 >= 6)
			{
				b3 -= 6;
			}
			sbyte b4 = (sbyte)(b3 + num3);
			if (b4 >= 6)
			{
				b4 -= 6;
			}
			bool flag = OfflineChangePoisoned(b2, b, num2);
			bool flag2 = OfflineChangePoisoned(b3, b, num2);
			bool flag3 = OfflineChangePoisoned(b4, b, num2);
			if (flag || flag2 || flag3)
			{
				mod.PoisonedChanged = true;
				InstantNotificationCollection instantNotifications = DomainManager.World.GetInstantNotifications();
				if (flag)
				{
					instantNotifications.AddPoisonIncreased(_id, b2, num2);
				}
				if (flag2)
				{
					instantNotifications.AddPoisonIncreased(_id, b3, num2);
				}
				if (flag3)
				{
					instantNotifications.AddPoisonIncreased(_id, b4, num2);
				}
				DomainManager.LifeRecord.GetLifeRecordCollection().AddSpiritualDebtKongsangPoisoned(_id, DomainManager.World.GetCurrDate());
			}
			num = GlobalConfig.Instance.KongsangCharacterFeaturePoisonedProbParm[0];
		}
		else
		{
			num += GlobalConfig.Instance.KongsangCharacterFeaturePoisonedProbParm[1];
		}
		DomainManager.Extra.SetKongsangCharacterFeaturePoisonedProb(num, context);
	}

	private unsafe void OfflineAutoRecoverMainAttributes()
	{
		MainAttributes maxMainAttributes = GetMaxMainAttributes();
		short physiologicalAge = GetPhysiologicalAge();
		int index = ((physiologicalAge <= 100) ? physiologicalAge : 100);
		MainAttributes mainAttributesRecoveries = AgeEffect.Instance[index].MainAttributesRecoveries;
		int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
		int num = ((_id == taiwuCharId) ? (DomainManager.Extra.GetActionPointPrevMonth() / 10) : 0);
		for (sbyte b = 0; b < 6; b++)
		{
			short num2 = maxMainAttributes.Items[b];
			int num3 = num2 / 5 * mainAttributesRecoveries.Items[b] / 100;
			num3 = Math.Max(1, num3 * (num + 100) / 100);
			if (_id == taiwuCharId)
			{
				ExtraDomain extra = DomainManager.Extra;
				int professionId = ProfessionRelatedConstants.MainAttributeRecoverProfessionIds[b];
				if (extra.IsProfessionalSkillUnlocked(professionId, 0))
				{
					ProfessionData professionData = extra.GetProfessionData(professionId);
					num3 = professionData.GetMainAttributesRecoveryBonusAppliedRate(b, num3);
				}
			}
			int value = _currMainAttributes.Items[b] + num3;
			_currMainAttributes.Items[b] = (short)Math.Clamp(value, 0, num2);
		}
	}

	private void OfflineAutoRecoverCurrNeili(PeriAdvanceMonthUpdateStatusModification mod)
	{
		int currNeili = _currNeili;
		int maxNeili = GetMaxNeili();
		int currNeiliRecovery = GetCurrNeiliRecovery(maxNeili);
		currNeiliRecovery = DomainManager.SpecialEffect.ModifyData(_id, -1, 298, currNeiliRecovery);
		_currNeili = Math.Clamp(_currNeili + currNeiliRecovery, 0, maxNeili);
		if (_currNeili != currNeili)
		{
			mod.CurrNeiliChanged = true;
		}
	}

	private void OfflineChangeQiDisorder(PeriAdvanceMonthUpdateStatusModification mod)
	{
		short changeOfQiDisorder = GetChangeOfQiDisorder();
		int num = Math.Clamp(_disorderOfQi + changeOfQiDisorder, DisorderLevelOfQi.MinValue, DisorderLevelOfQi.MaxValue);
		if (num != _disorderOfQi)
		{
			_disorderOfQi = (short)num;
			mod.QiDisorderChanged = true;
		}
	}

	private unsafe void OfflineChangeInjuries(IRandomSource randomSource, PeriAdvanceMonthUpdateStatusModification mod)
	{
		if (!DomainManager.SpecialEffect.ModifyData(_id, -1, 269, dataValue: true))
		{
			return;
		}
		sbyte currMonthInYear = DomainManager.World.GetCurrMonthInYear();
		List<sbyte> recoverBodyParts = Month.Instance[currMonthInYear].RecoverBodyParts;
		for (int i = 0; i < recoverBodyParts.Count; i++)
		{
			sbyte b = recoverBodyParts[i];
			(sbyte outer, sbyte inner) tuple = _injuries.Get(b);
			sbyte item = tuple.outer;
			sbyte item2 = tuple.inner;
			int num = b * 2;
			if (item > 0)
			{
				_injuries.Items[num] = (sbyte)(item - 1);
				mod.InjuriesChanged = true;
			}
			if (item2 > 0)
			{
				_injuries.Items[num + 1] = (sbyte)(item2 - 1);
				mod.InjuriesChanged = true;
			}
		}
	}

	private unsafe void OfflineAutoRecoverPoisoned(PeriAdvanceMonthUpdateStatusModification mod)
	{
		ref PoisonInts poisonResists = ref GetPoisonResists();
		for (int i = 0; i < 6; i++)
		{
			if (_poisoned.Items[i] > 0)
			{
				int num = 100 + poisonResists.Items[i];
				_poisoned.Items[i] = Math.Max(_poisoned.Items[i] - num, 0);
				mod.PoisonedChanged = true;
			}
		}
	}

	private unsafe void OfflineRecoverAllForXiangshuInfected(PeriAdvanceMonthUpdateStatusModification mod)
	{
		int maxNeili = GetMaxNeili();
		if (_currNeili != maxNeili)
		{
			_currNeili = maxNeili;
			mod.CurrNeiliChanged = true;
		}
		if (_disorderOfQi > 0)
		{
			_disorderOfQi = 0;
			mod.QiDisorderChanged = true;
		}
		for (int i = 0; i < 14; i++)
		{
			if (_injuries.Items[i] > 0)
			{
				_injuries.Items[i] = 0;
				mod.InjuriesChanged = true;
			}
		}
		for (int j = 0; j < 6; j++)
		{
			if (_poisoned.Items[j] > 0)
			{
				_poisoned.Items[j] = 0;
				mod.PoisonedChanged = true;
			}
		}
		MainAttributes maxMainAttributes = GetMaxMainAttributes();
		for (int k = 0; k < 6; k++)
		{
			short num = maxMainAttributes.Items[k];
			_currMainAttributes.Items[k] = num;
		}
	}

	private void OfflineChangeXiangshuInfection(PeriAdvanceMonthUpdateStatusModification mod)
	{
		if (AgeGroup.GetAgeGroup(_actualAge) != 2)
		{
			return;
		}
		if (_id != DomainManager.Taiwu.GetTaiwuCharId())
		{
			sbyte xiangshuLevel = DomainManager.World.GetXiangshuLevel();
			sbyte b = GlobalConfig.Instance.XiangshuInfectionGradeUpperLimits[xiangshuLevel];
			if (b < _organizationInfo.Grade)
			{
				return;
			}
		}
		if (GetLegendaryBookOwnerState() != 3)
		{
			byte xiangshuInfection = _xiangshuInfection;
			sbyte xiangshuInfectionDelta = GetXiangshuInfectionDelta();
			byte b2 = (byte)Math.Clamp(xiangshuInfection + xiangshuInfectionDelta, 0, 200);
			if (b2 != xiangshuInfection)
			{
				_xiangshuInfection = b2;
				mod.XiangshuInfectionChanged = true;
			}
			short infectionFeatureIdThatShouldBe = XiangshuInfectionTypeHelper.GetInfectionFeatureIdThatShouldBe(b2);
			if (!_featureIds.Contains(infectionFeatureIdThatShouldBe) && (infectionFeatureIdThatShouldBe != 218 || !_featureIds.Contains(415)) && !IsActiveExternalRelationState(4))
			{
				OfflineAddFeature(infectionFeatureIdThatShouldBe, removeMutexFeature: true);
				mod.FeaturesChanged = true;
				mod.XiangshuInfectionFeatureChanged = infectionFeatureIdThatShouldBe;
			}
		}
	}

	private void OfflineIncreaseAge(DataContext context, PeriAdvanceMonthUpdateStatusModification mod)
	{
		if (_birthMonth != DomainManager.World.GetCurrMonthInYear())
		{
			return;
		}
		MainAttributes lackOfCurrMainAtributes = default(MainAttributes);
		RecordLacksOfCurrMainAttributes(ref lackOfCurrMainAtributes);
		_actualAge++;
		mod.ActualAgeChanged = true;
		if (!IsAgeIncreaseStopped())
		{
			_currAge++;
			mod.CurrAgeChanged = true;
			if (_id != DomainManager.Taiwu.GetTaiwuCharId() && _currAge == 1 && context.Random.CheckPercentProb(50))
			{
				short featureId = CharacterDomain.GenerateOneYearOldCatchFeature(context.Random);
				OfflineAddFeature(featureId, removeMutexFeature: true);
				mod.FeaturesChanged = true;
			}
			if (DomainManager.SpecialEffect.ModifyData(_id, -1, 266, dataValue: false))
			{
				_currAge++;
				mod.CurrAgeChanged = true;
			}
		}
		if (_hobbyExpirationDate <= DomainManager.World.GetCurrDate())
		{
			OfflineChangeHobby(context.Random);
			mod.HobbyChanged = true;
			mod.FavorabilitiesOfRelatedChars = DomainManager.Character.ChangeFavorabilitiesOfAllRelatedCharsWhenChangingHobby(context, this);
		}
		if (mod.CurrAgeChanged)
		{
			if (TryObtainPotentialFeatures())
			{
				mod.FeaturesChanged = true;
				mod.RecreateTeammateCommands = true;
			}
			mod.NewClothingTemplateId = TryGetNewClothingWhenAgeGroupChanges(context.Random, 0);
		}
		OfflineRestoreLacksOfCurrMainAttributes(context, lackOfCurrMainAtributes, mod);
	}

	private void OfflineUpdateHealth()
	{
		bool isCompletelyInfected = IsCompletelyInfected();
		int num = GetHealthRecovery(isCompletelyInfected) - 1;
		short leftMaxHealth = GetLeftMaxHealth();
		_health = (short)((leftMaxHealth >= 0) ? ((short)Math.Clamp(_health + num, 0, leftMaxHealth)) : 0);
	}

	private unsafe void RecordLacksOfCurrMainAttributes(ref MainAttributes lackOfCurrMainAtributes)
	{
		MainAttributes maxMainAttributes = GetMaxMainAttributes();
		for (int i = 0; i < 6; i++)
		{
			short num = (short)(maxMainAttributes.Items[i] - _currMainAttributes.Items[i]);
			lackOfCurrMainAtributes.Items[i] = num;
		}
	}

	private unsafe void OfflineRestoreLacksOfCurrMainAttributes(DataContext context, MainAttributes lacksOfCurrMainAttributes, PeriAdvanceMonthUpdateStatusModification mod)
	{
		MainAttributes maxMainAttributes = GetMaxMainAttributes();
		for (int i = 0; i < 6; i++)
		{
			short num = (short)(maxMainAttributes.Items[i] - lacksOfCurrMainAttributes.Items[i]);
			if (_currMainAttributes.Items[i] != num)
			{
				_currMainAttributes.Items[i] = num;
				mod.CurrMainAttributesChanged = true;
			}
		}
	}

	private bool TryObtainPotentialFeatures()
	{
		if (_currAge > 16)
		{
			return false;
		}
		int count = _potentialFeatureIds.Count;
		int num = count * (_currAge - 1) / 16;
		int num2 = count * _currAge / 16;
		if (num >= num2)
		{
			return false;
		}
		for (int i = num; i < num2; i++)
		{
			OfflineAddFeature(_potentialFeatureIds[i], removeMutexFeature: true, removeLowerOnly: true);
		}
		return true;
	}

	private short TryGetNewClothingWhenAgeGroupChanges(IRandomSource random, sbyte shufangAgeChange)
	{
		if ((_currAge == 16 && shufangAgeChange >= 0) || (shufangAgeChange == 1 && _currAge == 17))
		{
			if (_kidnapperId >= 0)
			{
				return -2;
			}
			OrganizationMemberItem orgMemberConfig = OrganizationDomain.GetOrgMemberConfig(_organizationInfo);
			return OrganizationDomain.GetRandomOrgMemberClothing(random, orgMemberConfig);
		}
		if (_currAge == GlobalConfig.Instance.AgeBaby)
		{
			return 65;
		}
		return -1;
	}

	private unsafe void TryGrowAvatarElements(DataContext context)
	{
		short physiologicalAge = GetPhysiologicalAge();
		bool flag = false;
		bool flag2 = false;
		AvatarElementsGrownDates growthDates = default(AvatarElementsGrownDates);
		for (sbyte b = 0; b < 7; b++)
		{
			if (!_avatar.GetGrowableElementShowingState(b) && IsAbleToGrowAvatarElement(b, physiologicalAge))
			{
				if (!flag)
				{
					if (!DomainManager.Character.TryGetAvatarElementGrowthProgress(_id, out growthDates))
					{
						break;
					}
					flag = true;
				}
				int num = growthDates.Items[b];
				if (DomainManager.World.GetCurrDate() >= num)
				{
					_avatar.SetGrowableElementShowingState(b);
					growthDates.Items[b] = -1;
					flag2 = true;
				}
			}
		}
		if (flag2)
		{
			SetAvatar(_avatar, context);
			DomainManager.Character.SetAvatarElementGrowthProgress(context, _id, ref growthDates);
		}
	}

	public bool IsAgeIncreaseStopped()
	{
		bool flag = _featureIds.Contains(637);
		if (_id == DomainManager.Taiwu.GetTaiwuCharId())
		{
			if (flag)
			{
				return true;
			}
			ProfessionData professionData = DomainManager.Extra.GetProfessionData(5);
			TaoistMonkSkillsData skillsData = professionData.GetSkillsData<TaoistMonkSkillsData>();
			if (!DomainManager.Extra.IsProfessionalSkillUnlockedAndEquipped(48) && skillsData.HasSurvivedAllTribulation() && !skillsData.ShouldIncreaseAge())
			{
				return true;
			}
		}
		else if (IsCompletelyInfected() || GetLegendaryBookOwnerState() > 0 || flag)
		{
			return true;
		}
		return false;
	}

	public void PeriAdvanceMonth_ExecuteFixedActions(DataContext context, HashSet<int> currBlockCharSet)
	{
		if (IsActiveAdvanceMonthStatus(4))
		{
			return;
		}
		PeriAdvanceMonthFixedActionModification periAdvanceMonthFixedActionModification = new PeriAdvanceMonthFixedActionModification(this);
		sbyte ageGroup = GetAgeGroup();
		if (ageGroup == 0)
		{
			if (!DomainManager.Taiwu.IsInGroup(_id))
			{
				OfflineExecuteFixedAction_Regroup(context.Random, currBlockCharSet, periAdvanceMonthFixedActionModification);
			}
		}
		else
		{
			if (ageGroup == 2)
			{
				OfflineExecuteFixedAction_MakeLove(context, currBlockCharSet, periAdvanceMonthFixedActionModification);
			}
			if (!DomainManager.Taiwu.IsInGroup(_id))
			{
				OfflineExecuteFixedAction_Regroup(context.Random, currBlockCharSet, periAdvanceMonthFixedActionModification);
			}
			OfflineExecuteFixedAction_ReleaseKidnappedCharacters(context.Random, periAdvanceMonthFixedActionModification);
			OfflineExecuteFixedAction_CollectResources(context.Random, periAdvanceMonthFixedActionModification);
		}
		if (periAdvanceMonthFixedActionModification.IsChanged)
		{
			ParallelModificationsRecorder parallelModificationsRecorder = context.ParallelModificationsRecorder;
			parallelModificationsRecorder.RecordType(ParallelModificationType.PeriAdvanceMonthExecuteFixedActions);
			parallelModificationsRecorder.RecordParameterClass(periAdvanceMonthFixedActionModification);
		}
	}

	public static void ComplementPeriAdvanceMonth_ExecuteFixedActions(DataContext context, PeriAdvanceMonthFixedActionModification mod)
	{
		Character character = mod.Character;
		LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
		MonthlyNotificationCollection monthlyNotificationCollection = DomainManager.World.GetMonthlyNotificationCollection();
		SecretInformationCollection secretInformationCollection = DomainManager.Information.GetSecretInformationCollection();
		int currDate = DomainManager.World.GetCurrDate();
		Location location = character.GetLocation();
		int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
		if (mod.MakeLoveTargetList != null)
		{
			character.SetFeatureIds(character._featureIds, context);
			foreach (var (character2, makeLoveState, flag) in mod.MakeLoveTargetList)
			{
				character2.SetFeatureIds(character2._featureIds, context);
				character2.SetHappiness(character2._happiness, context);
				switch (makeLoveState)
				{
				case PeriAdvanceMonthFixedActionModification.MakeLoveState.Legal:
					if (character2._id == taiwuCharId)
					{
						monthlyNotificationCollection.AddMakeLove(character._id, location, character2._id);
					}
					break;
				case PeriAdvanceMonthFixedActionModification.MakeLoveState.Illegal:
				case PeriAdvanceMonthFixedActionModification.MakeLoveState.Wug:
				{
					if (character2._id == taiwuCharId)
					{
						monthlyNotificationCollection.AddMakeLove(character._id, location, character2._id);
					}
					lifeRecordCollection.AddMakeLoveIllegal(character._id, currDate, character2._id, location);
					int dataOffset = secretInformationCollection.AddMakeLoveIllegal(character._id, character2._id);
					int num = DomainManager.Information.AddSecretInformationMetaData(context, dataOffset);
					break;
				}
				case PeriAdvanceMonthFixedActionModification.MakeLoveState.RapeSucceed:
				{
					lifeRecordCollection.AddRapeSucceed(character._id, currDate, character2._id, location);
					DomainManager.Character.AddRelation(context, character2._id, character._id, 32768, currDate);
					DomainManager.Character.ChangeFavorabilityOptionalMonthlyEvolution(context, character2, character, -30000);
					int dataOffset2 = secretInformationCollection.AddRape(character._id, character2._id);
					int num2 = DomainManager.Information.AddSecretInformationMetaData(context, dataOffset2);
					break;
				}
				case PeriAdvanceMonthFixedActionModification.MakeLoveState.RapeFail:
					if (character2._id == taiwuCharId)
					{
						monthlyNotificationCollection.AddRapeFailure(character._id, location, character2._id);
					}
					lifeRecordCollection.AddRapeFail(character._id, currDate, character2._id, location);
					DomainManager.Character.AddRelation(context, character2._id, character._id, 32768, currDate);
					DomainManager.Character.ChangeFavorabilityOptionalMonthlyEvolution(context, character2, character, -30000);
					break;
				}
				Events.RaiseMakeLove(context, character, character2, (sbyte)makeLoveState);
				if (flag)
				{
					Character father;
					Character mother;
					if (character._gender == 1)
					{
						father = character;
						mother = character2;
					}
					else
					{
						father = character2;
						mother = character;
					}
					DomainManager.Character.CreatePregnantState(context, mother, father, makeLoveState == PeriAdvanceMonthFixedActionModification.MakeLoveState.RapeSucceed);
				}
			}
		}
		if (mod.LeaveGroup)
		{
			DomainManager.Character.LeaveGroup(context, character);
		}
		if (mod.NewGroupLeader >= 0)
		{
			character.ApplyFixedAction_JoinGroup(context, mod.NewGroupLeader, mod.NewGroupActionTemplateId);
		}
		if (mod.TravelTargetsChanged)
		{
			character.SetNpcTravelTargets(character._npcTravelTargets, context);
		}
		if (mod.ReleaseKidnappedCharList != null)
		{
			KidnappedCharacterList kidnappedCharacters = DomainManager.Character.GetKidnappedCharacters(character._id);
			bool flag2 = DomainManager.Character.IsTaiwuPeople(character._id);
			foreach (int releaseKidnappedChar in mod.ReleaseKidnappedCharList)
			{
				int slotIndex = kidnappedCharacters.IndexOf(releaseKidnappedChar);
				lifeRecordCollection.AddReleaseKidnappedCharacter(character._id, currDate, releaseKidnappedChar, location);
				DomainManager.Character.RemoveKidnappedCharacter(context, character, kidnappedCharacters, slotIndex, isEscaped: false);
				if (flag2 || DomainManager.Character.IsTaiwuPeople(releaseKidnappedChar))
				{
					monthlyNotificationCollection.AddReleasePrisoner(character._id, location, releaseKidnappedChar);
				}
				int dataOffset3 = secretInformationCollection.AddReleaseKidnappedCharacter(character._id, releaseKidnappedChar);
				DomainManager.Information.AddSecretInformationMetaData(context, dataOffset3);
			}
		}
		if (mod.ModifiedMapBlocks == null)
		{
			return;
		}
		character.SetResources(ref character._resources, context);
		foreach (MapBlockData modifiedMapBlock in mod.ModifiedMapBlocks)
		{
			DomainManager.Map.SetBlockData(context, modifiedMapBlock);
		}
	}

	private void OfflineExecuteFixedAction_MakeLove(DataContext context, HashSet<int> currBlockCharSet, PeriAdvanceMonthFixedActionModification mod)
	{
		IRandomSource random = context.Random;
		if (SpecialEffectUtils.HasAzureMarrowMakeLoveEffect(_id))
		{
			List<int> obj = context.AdvanceMonthRelatedData.CharIdList.Occupy();
			foreach (int item4 in currBlockCharSet)
			{
				if (!DomainManager.Character.TryGetRelation(_id, item4, out var relation) || !DomainManager.Character.TryGetRelation(item4, _id, out var relation2) || relation.GetFavorabilityType() < 4 || relation2.GetFavorabilityType() < 4)
				{
					continue;
				}
				Character element_Objects = DomainManager.Character.GetElement_Objects(item4);
				if (_gender != element_Objects._gender)
				{
					int startRelationSuccessRate_BoyOrGirlFriend = AiHelper.Relation.GetStartRelationSuccessRate_BoyOrGirlFriend(this, element_Objects, relation, relation2);
					DomainManager.SpecialEffect.ModifyValue(_id, 268, startRelationSuccessRate_BoyOrGirlFriend);
					startRelationSuccessRate_BoyOrGirlFriend = startRelationSuccessRate_BoyOrGirlFriend * GetFertility() * element_Objects.GetFertility() / 10000;
					if (random.CheckPercentProb(startRelationSuccessRate_BoyOrGirlFriend))
					{
						obj.Add(item4);
					}
				}
			}
			int randomOrDefault = obj.GetRandomOrDefault(random, -1);
			context.AdvanceMonthRelatedData.CharIdList.Release(ref obj);
			if (randomOrDefault >= 0)
			{
				Character element_Objects2 = DomainManager.Character.GetElement_Objects(randomOrDefault);
				(Character father, Character mother) parentalInfoOnMakeLove = GetParentalInfoOnMakeLove(this, element_Objects2);
				Character item = parentalInfoOnMakeLove.father;
				Character item2 = parentalInfoOnMakeLove.mother;
				bool item3 = OfflineMakeLove(random, item, item2, isRape: false);
				if (mod.MakeLoveTargetList == null)
				{
					mod.MakeLoveTargetList = new List<(Character, PeriAdvanceMonthFixedActionModification.MakeLoveState, bool)>();
				}
				mod.MakeLoveTargetList.Add((element_Objects2, PeriAdvanceMonthFixedActionModification.MakeLoveState.Wug, item3));
				return;
			}
		}
		bool flag = false;
		for (int num = _personalNeeds.Count - 1; num >= 0; num--)
		{
			GameData.Domains.Character.Ai.PersonalNeed personalNeed = _personalNeeds[num];
			if (personalNeed.TemplateId == 23)
			{
				flag = true;
				if (currBlockCharSet.Contains(personalNeed.CharId))
				{
					OfflineExecuteFixedAction_MakeLove_Mutual(random, personalNeed.CharId, allowRape: true, mod);
				}
				else
				{
					NpcTravelTarget target = new NpcTravelTarget(personalNeed.CharId, 3);
					OfflineAddNpcTravelTarget(target);
					mod.TravelTargetsChanged = true;
				}
				CollectionUtils.SwapAndRemove(_personalNeeds, num);
			}
		}
		if (flag)
		{
			return;
		}
		foreach (int item5 in currBlockCharSet)
		{
			if ((DomainManager.Character.TryGetRelation(_id, item5, out var relation3) && RelationType.HasRelation(relation3.RelationType, 16384)) || RelationType.HasRelation(relation3.RelationType, 1024))
			{
				OfflineExecuteFixedAction_MakeLove_Mutual(random, item5, allowRape: false, mod);
			}
		}
	}

	private void OfflineExecuteFixedAction_MakeLove_Mutual(IRandomSource random, int targetCharId, bool allowRape, PeriAdvanceMonthFixedActionModification mod)
	{
		Character element_Objects = DomainManager.Character.GetElement_Objects(targetCharId);
		if (element_Objects.GetAgeGroup() != 2)
		{
			return;
		}
		RelatedCharacter relation = DomainManager.Character.GetRelation(targetCharId, _id);
		sbyte favorabilityType = FavorabilityType.GetFavorabilityType(DomainManager.Character.GetRelation(_id, targetCharId).Favorability);
		sbyte favorabilityType2 = FavorabilityType.GetFavorabilityType(relation.Favorability);
		var (father, mother) = GetParentalInfoOnMakeLove(this, element_Objects);
		if (RelationType.HasRelation(relation.RelationType, 1024))
		{
			int num = 50 + 50 * (favorabilityType + favorabilityType2) / 12;
			int num2 = num * GetFertility() * element_Objects.GetFertility() / 10000;
			DomainManager.SpecialEffect.ModifyValue(_id, 268, num2);
			if (random.CheckPercentProb(num2))
			{
				PeriAdvanceMonthFixedActionModification periAdvanceMonthFixedActionModification = mod;
				if (periAdvanceMonthFixedActionModification.MakeLoveTargetList == null)
				{
					periAdvanceMonthFixedActionModification.MakeLoveTargetList = new List<(Character, PeriAdvanceMonthFixedActionModification.MakeLoveState, bool)>();
				}
				mod.MakeLoveTargetList.Add((element_Objects, PeriAdvanceMonthFixedActionModification.MakeLoveState.Legal, OfflineMakeLove(random, father, mother, isRape: false)));
			}
		}
		else if (RelationType.HasRelation(relation.RelationType, 16384))
		{
			int num3 = 50 + 50 * (favorabilityType + favorabilityType2) / 12;
			num3 = num3 * AiHelper.FixedActionConstants.BoyAndGirlFriendMakeLoveBaseChance[GetBehaviorType()] / 100;
			int num4 = num3 * GetFertility() * element_Objects.GetFertility() / 10000;
			num4 += DomainManager.Extra.GetAiActionRateAdjust(_id, 4, 1);
			DomainManager.SpecialEffect.ModifyValue(_id, 268, num4);
			if (num4 > 0 && random.CheckPercentProb(num4))
			{
				PeriAdvanceMonthFixedActionModification periAdvanceMonthFixedActionModification = mod;
				if (periAdvanceMonthFixedActionModification.MakeLoveTargetList == null)
				{
					periAdvanceMonthFixedActionModification.MakeLoveTargetList = new List<(Character, PeriAdvanceMonthFixedActionModification.MakeLoveState, bool)>();
				}
				mod.MakeLoveTargetList.Add((element_Objects, PeriAdvanceMonthFixedActionModification.MakeLoveState.Illegal, OfflineMakeLove(random, father, mother, isRape: false)));
			}
		}
		else
		{
			if (!allowRape)
			{
				return;
			}
			short fertility = GetFertility();
			int num5 = AiHelper.FixedActionConstants.RapeBaseChance[GetBehaviorType()] * fertility / 100;
			num5 += DomainManager.Extra.GetAiActionRateAdjust(_id, 4, 3);
			DomainManager.SpecialEffect.ModifyValue(_id, 268, num5);
			if (num5 > 0 && random.CheckPercentProb(num5))
			{
				PeriAdvanceMonthFixedActionModification periAdvanceMonthFixedActionModification = mod;
				if (periAdvanceMonthFixedActionModification.MakeLoveTargetList == null)
				{
					periAdvanceMonthFixedActionModification.MakeLoveTargetList = new List<(Character, PeriAdvanceMonthFixedActionModification.MakeLoveState, bool)>();
				}
				if (targetCharId != DomainManager.Taiwu.GetTaiwuCharId() && GetCombatPower() > element_Objects.GetCombatPower() && fertility > 50)
				{
					mod.MakeLoveTargetList.Add((element_Objects, PeriAdvanceMonthFixedActionModification.MakeLoveState.RapeSucceed, OfflineMakeLove(random, father, mother, isRape: true)));
				}
				else
				{
					mod.MakeLoveTargetList.Add((element_Objects, PeriAdvanceMonthFixedActionModification.MakeLoveState.RapeFail, false));
				}
			}
		}
	}

	private static (Character father, Character mother) GetParentalInfoOnMakeLove(Character self, Character target)
	{
		return (self._gender == 1) ? (father: self, mother: target) : (father: target, mother: self);
	}

	private bool OfflineMakeLove(IRandomSource random, Character father, Character mother, bool isRape)
	{
		if (mother.GetGender() == father.GetGender())
		{
			return false;
		}
		mother.OfflineAddFeature(196, removeMutexFeature: true);
		father.OfflineAddFeature(196, removeMutexFeature: true);
		if (!PregnantState.CheckPregnant(random, father, mother, isRape))
		{
			return false;
		}
		mother.OfflineAddFeature(197, removeMutexFeature: true);
		return true;
	}

	private unsafe void OfflineExecuteFixedAction_Regroup(IRandomSource random, HashSet<int> currBlockCharSet, PeriAdvanceMonthFixedActionModification mod)
	{
		int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
		if (_leaderId == taiwuCharId)
		{
			return;
		}
		int num = _leaderId;
		if (num == _id)
		{
			if (DomainManager.Character.HasNonBabyMemberInGroup(num))
			{
				return;
			}
			num = -1;
		}
		sbyte behaviorType = GetBehaviorType();
		if (OfflineExecuteFixedAction_Regroup_PrioritizedAction(random, behaviorType, currBlockCharSet, mod))
		{
			return;
		}
		int num2 = -1;
		for (int num3 = _personalNeeds.Count - 1; num3 >= 0; num3--)
		{
			GameData.Domains.Character.Ai.PersonalNeed personalNeed = _personalNeeds[num3];
			if (personalNeed.TemplateId == 20 && DomainManager.Character.TryGetElement_Objects(personalNeed.CharId, out var element))
			{
				num2 = personalNeed.CharId;
				if (num < 0)
				{
					break;
				}
				if (element._leaderId >= 0)
				{
					num2 = element._leaderId;
				}
				if (num != num2)
				{
					mod.LeaveGroup = true;
				}
				return;
			}
		}
		Personalities personalities = GetPersonalities();
		sbyte ageGroup = AgeGroup.GetAgeGroup(_currAge);
		if (num >= 0)
		{
			RelatedCharacter relation = DomainManager.Character.GetRelation(_id, num);
			RelatedCharacter relation2 = DomainManager.Character.GetRelation(num, _id);
			sbyte joinGroupRelationType = AiHelper.JoinGroupRelationType.GetJoinGroupRelationType(relation.RelationType, relation2.RelationType);
			switch (ageGroup)
			{
			case 1:
				if ((uint)(joinGroupRelationType - 2) > 1u)
				{
					break;
				}
				return;
			case 0:
				return;
			}
			sbyte favorabilityType = FavorabilityType.GetFavorabilityType(relation.Favorability);
			if (joinGroupRelationType == -1 || favorabilityType < AiHelper.FixedActionConstants.JoinGroupFavorabilityReq[joinGroupRelationType])
			{
				mod.LeaveGroup = true;
				return;
			}
			int num4 = DomainManager.World.GetCurrDate() - DomainManager.Character.GetElement_JoinGroupDates(_id);
			if (num4 >= AiHelper.FixedActionConstants.StayInGroupMonth[joinGroupRelationType])
			{
				int percentProb = Math.Min(AiHelper.FixedActionConstants.MaxLeaveGroupChance[joinGroupRelationType], (num4 - AiHelper.FixedActionConstants.StayInGroupMonth[joinGroupRelationType]) * AiHelper.FixedActionConstants.LeaveGroupChancePerMonth[joinGroupRelationType]);
				if (random.CheckPercentProb(percentProb))
				{
					mod.LeaveGroup = true;
				}
			}
		}
		else if (num2 >= 0)
		{
			if (currBlockCharSet.Contains(num2) && !DomainManager.Taiwu.IsInGroup(num2))
			{
				mod.NewGroupLeader = num2;
				return;
			}
			NpcTravelTarget target = new NpcTravelTarget(num2, 3);
			OfflineAddNpcTravelTarget(target);
			mod.TravelTargetsChanged = true;
		}
		else
		{
			if (currBlockCharSet.Count <= 0)
			{
				return;
			}
			List<(Character, RelatedCharacter)>[] array = new List<(Character, RelatedCharacter)>[7];
			for (int i = 0; i < 7; i++)
			{
				array[i] = new List<(Character, RelatedCharacter)>();
			}
			foreach (int item8 in currBlockCharSet)
			{
				if (DomainManager.Taiwu.IsInGroup(item8) || !DomainManager.Character.TryGetRelation(_id, item8, out var relation3) || !DomainManager.Character.TryGetRelation(item8, _id, out var relation4))
				{
					continue;
				}
				sbyte joinGroupRelationType2 = AiHelper.JoinGroupRelationType.GetJoinGroupRelationType(relation3.RelationType, relation4.RelationType);
				if (joinGroupRelationType2 == -1)
				{
					continue;
				}
				Character element_Objects = DomainManager.Character.GetElement_Objects(item8);
				if (element_Objects.GetAgeGroup() == 0 || element_Objects.GetLegendaryBookOwnerState() >= 2)
				{
					continue;
				}
				switch (ageGroup)
				{
				case 0:
				{
					bool flag = (uint)(joinGroupRelationType2 - 1) <= 2u;
					if (flag && element_Objects.GetCurrAge() > _currAge)
					{
						array[joinGroupRelationType2].Add((element_Objects, relation3));
					}
					continue;
				}
				case 1:
					if ((uint)(joinGroupRelationType2 - 2) > 1u)
					{
						break;
					}
					if (element_Objects.GetCurrAge() > _currAge)
					{
						array[joinGroupRelationType2].Add((element_Objects, relation3));
					}
					continue;
				}
				sbyte favorabilityType2 = FavorabilityType.GetFavorabilityType(relation3.Favorability);
				if (favorabilityType2 >= AiHelper.FixedActionConstants.JoinGroupFavorabilityReq[joinGroupRelationType2])
				{
					Comparison<(Character, RelatedCharacter)> comparison = AiHelper.JoinGroupRelationType.Comparisons[joinGroupRelationType2];
					bool flag = (uint)(joinGroupRelationType2 - 1) <= 1u;
					if (flag || comparison((this, relation4), (element_Objects, relation3)) <= 0)
					{
						array[joinGroupRelationType2].Add((element_Objects, relation3));
					}
				}
			}
			switch (ageGroup)
			{
			case 0:
				if (array[2].Count > 0)
				{
					List<(Character, RelatedCharacter)> list3 = array[2];
					Comparison<(Character, RelatedCharacter)> comparison4 = AiHelper.JoinGroupRelationType.Comparisons[2];
					Character item3 = list3.Max(comparison4).Item1;
					mod.NewGroupLeader = item3._id;
				}
				else if (array[3].Count > 0)
				{
					List<(Character, RelatedCharacter)> list4 = array[3];
					Comparison<(Character, RelatedCharacter)> comparison5 = AiHelper.JoinGroupRelationType.Comparisons[3];
					Character item4 = list4.Max(comparison5).Item1;
					mod.NewGroupLeader = item4._id;
				}
				else if (array[1].Count > 0)
				{
					List<(Character, RelatedCharacter)> list5 = array[1];
					Comparison<(Character, RelatedCharacter)> comparison6 = AiHelper.JoinGroupRelationType.Comparisons[1];
					Character item5 = list5.Max(comparison6).Item1;
					mod.NewGroupLeader = item5._id;
				}
				return;
			case 1:
				if (array[2].Count > 0)
				{
					List<(Character, RelatedCharacter)> list = array[2];
					Comparison<(Character, RelatedCharacter)> comparison2 = AiHelper.JoinGroupRelationType.Comparisons[2];
					Character item = list.Max(comparison2).Item1;
					mod.NewGroupLeader = item._id;
					return;
				}
				if (array[3].Count > 0)
				{
					List<(Character, RelatedCharacter)> list2 = array[3];
					Comparison<(Character, RelatedCharacter)> comparison3 = AiHelper.JoinGroupRelationType.Comparisons[3];
					Character item2 = list2.Max(comparison3).Item1;
					mod.NewGroupLeader = item2._id;
					return;
				}
				break;
			}
			sbyte[] array2 = AiHelper.JoinGroupRelationType.Priorities[behaviorType];
			foreach (sbyte b in array2)
			{
				List<(Character, RelatedCharacter)> list6 = array[b];
				if (list6.Count == 0)
				{
					continue;
				}
				sbyte b2 = AiHelper.JoinGroupRelationType.ToPersonalityType[b];
				int percentProb2 = AiHelper.FixedActionConstants.JoinGroupBaseChance[behaviorType] + personalities.Items[b2];
				if (!random.CheckPercentProb(percentProb2))
				{
					if ((uint)(b - 1) <= 1u)
					{
						Comparison<(Character, RelatedCharacter)> comparison7 = AiHelper.JoinGroupRelationType.Comparisons[b];
						Character item6 = list6.Max(comparison7).Item1;
						mod.NewGroupLeader = item6._id;
					}
					else
					{
						Character item7 = list6.GetRandom(random).Item1;
						mod.NewGroupLeader = item7._id;
					}
					break;
				}
			}
		}
	}

	private bool OfflineExecuteFixedAction_Regroup_PrioritizedAction(IRandomSource random, sbyte behaviorType, HashSet<int> currBlockCharSet, PeriAdvanceMonthFixedActionModification mod)
	{
		if (!DomainManager.Character.TryGetCharacterPrioritizedAction(_id, out var action))
		{
			return false;
		}
		PrioritizedActionsItem prioritizedActionsItem = PrioritizedActions.Instance[action.ActionType];
		if (_leaderId >= 0)
		{
			if (DomainManager.Character.TryGetCharacterPrioritizedAction(_leaderId, out var action2) && action2.CanJointActionWith(action))
			{
				return true;
			}
			mod.LeaveGroup = true;
		}
		if (!random.CheckPercentProb(prioritizedActionsItem.ActionJointChance[behaviorType]))
		{
			return true;
		}
		foreach (int item in currBlockCharSet)
		{
			if (!DomainManager.Character.TryGetCharacterPrioritizedAction(item, out var action3) || !action3.CanJointActionWith(action) || DomainManager.Taiwu.IsInGroup(item) || !DomainManager.Character.TryGetRelation(_id, item, out var relation) || !DomainManager.Character.TryGetRelation(item, _id, out var relation2) || RelationType.HasRelation(relation.RelationType, 32768) || RelationType.HasRelation(relation2.RelationType, 32768))
			{
				continue;
			}
			Character element_Objects = DomainManager.Character.GetElement_Objects(item);
			int leaderId = element_Objects.GetLeaderId();
			if (leaderId < 0 || leaderId == item)
			{
				sbyte behaviorType2 = element_Objects.GetBehaviorType();
				if (!random.CheckPercentProb(prioritizedActionsItem.ActionJointChance[behaviorType2]))
				{
					mod.NewGroupLeader = item;
					mod.NewGroupActionTemplateId = action.ActionType;
					break;
				}
			}
		}
		return true;
	}

	private void OfflineExecuteFixedAction_ReleaseKidnappedCharacters(IRandomSource random, PeriAdvanceMonthFixedActionModification mod)
	{
		if (!IsActiveExternalRelationState(2))
		{
			return;
		}
		KidnappedCharacterList kidnappedCharacters = DomainManager.Character.GetKidnappedCharacters(_id);
		sbyte behaviorType = GetBehaviorType();
		sbyte b = AiHelper.FixedActionConstants.ReleaseKidnappedCharResistanceThreshold[behaviorType];
		sbyte b2 = AiHelper.FixedActionConstants.ReleaseKidnappedCharChance[behaviorType];
		Character element_Objects = DomainManager.Character.GetElement_Objects(_id);
		foreach (KidnappedCharacter item in kidnappedCharacters.GetCollection())
		{
			int num = DomainManager.Character.CalcKidnappedCharacterTotalResistance(element_Objects, item);
			if (num < b)
			{
				continue;
			}
			short favorability = DomainManager.Character.GetFavorability(_id, item.CharId);
			int percentProb = b2 + FavorabilityType.GetFavorabilityType(favorability) * 5;
			if (random.CheckPercentProb(percentProb))
			{
				if (mod.ReleaseKidnappedCharList == null)
				{
					mod.ReleaseKidnappedCharList = new List<int>();
				}
				mod.ReleaseKidnappedCharList.Add(item.CharId);
			}
		}
	}

	private unsafe void OfflineExecuteFixedAction_CollectResources(IRandomSource random, PeriAdvanceMonthFixedActionModification mod)
	{
		foreach (GameData.Domains.Character.Ai.PersonalNeed personalNeed in _personalNeeds)
		{
			if (personalNeed.TemplateId != 8 || personalNeed.ResourceType >= 6)
			{
				continue;
			}
			MapBlockData block = DomainManager.Map.GetBlock(_location);
			short num = block.CurrResources.Items[personalNeed.ResourceType];
			if (num >= block.MaxResources.Items[personalNeed.ResourceType] / 2)
			{
				int collectResourceAmount = DomainManager.Map.GetCollectResourceAmount(random, block, personalNeed.ResourceType);
				ref int reference = ref _resources.Items[personalNeed.ResourceType];
				reference += collectResourceAmount;
				ResourceTypeItem resourceTypeItem = Config.ResourceType.Instance[personalNeed.ResourceType];
				block.CurrResources.Items[personalNeed.ResourceType] = (short)Math.Max(num - resourceTypeItem.ResourceReducePerCollection, 0);
				block.Malice += 10;
				if (mod.ModifiedMapBlocks == null)
				{
					mod.ModifiedMapBlocks = new List<MapBlockData>();
				}
				mod.ModifiedMapBlocks.Add(block);
			}
		}
	}

	private void ApplyFixedAction_JoinGroup(DataContext context, int newGroupLeader, short actionTemplateId)
	{
		Character element_Objects = DomainManager.Character.GetElement_Objects(newGroupLeader);
		if (_leaderId == _id)
		{
			if (DomainManager.Character.HasNonBabyMemberInGroup(_leaderId))
			{
				return;
			}
			CharacterSet characterSet = DomainManager.Character.GetGroup(_leaderId);
			HashSet<int> obj = context.AdvanceMonthRelatedData.RelatedCharIds.Occupy();
			obj.UnionWith(characterSet.GetCollection());
			obj.Remove(_id);
			foreach (int item in obj)
			{
				Character element_Objects2 = DomainManager.Character.GetElement_Objects(item);
				Tester.Assert(element_Objects2.GetAgeGroup() == 0);
				DomainManager.Character.LeaveGroup(context, element_Objects2);
				DomainManager.Character.JoinGroup(context, element_Objects2, element_Objects);
			}
			context.AdvanceMonthRelatedData.RelatedCharIds.Release(ref obj);
		}
		DomainManager.LifeRecord.GetLifeRecordCollection().AddTeamUp(this, element_Objects, actionTemplateId, addBackwards: true);
		DomainManager.Character.JoinGroup(context, this, element_Objects);
	}

	public void PeriAdvanceMonth_ExecuteGeneralAction(DataContext context, HashSet<int> currBlockChars, HashSet<int> currBlockGraves)
	{
		if (GetAgeGroup() == 0 || (currBlockChars.Count <= 1 && currBlockGraves == null) || IsActiveAdvanceMonthStatus(1))
		{
			return;
		}
		PeriAdvanceMonthGeneralActionModification periAdvanceMonthGeneralActionModification = new PeriAdvanceMonthGeneralActionModification(this);
		bool flag = OfflineRecoverActionEnergies();
		HashSet<int> obj = context.AdvanceMonthRelatedData.CaringCharIds.Occupy();
		GetAllCaringCharsInSet(currBlockChars, obj);
		foreach (GameData.Domains.Character.Ai.PersonalNeed personalNeed in _personalNeeds)
		{
			if (!personalNeed.CheckValid(this))
			{
				continue;
			}
			switch (personalNeed.TemplateId)
			{
			case 4:
				if (currBlockChars.Count > 1)
				{
					OfflineCalcGeneralAction_HealInjury(context, periAdvanceMonthGeneralActionModification, currBlockChars, personalNeed);
				}
				break;
			case 5:
				if (currBlockChars.Count > 1)
				{
					OfflineCalcGeneralAction_DetoxPoison(context, periAdvanceMonthGeneralActionModification, currBlockChars, personalNeed);
				}
				break;
			case 1:
				if (currBlockChars.Count > 1)
				{
					OfflineCalcGeneralAction_IncreaseHealth(context, periAdvanceMonthGeneralActionModification, currBlockChars, personalNeed);
				}
				break;
			case 2:
				if (currBlockChars.Count > 1)
				{
					OfflineCalcGeneralAction_RestoreDisorderOfQi(context, periAdvanceMonthGeneralActionModification, currBlockChars, personalNeed);
				}
				break;
			case 3:
				if (currBlockChars.Count > 1)
				{
					OfflineCalcGeneralAction_IncreaseNeili(context, periAdvanceMonthGeneralActionModification, currBlockChars, personalNeed);
				}
				break;
			case 7:
				if (currBlockChars.Count > 1)
				{
					OfflineCalcGeneralAction_KillWug(context, periAdvanceMonthGeneralActionModification, currBlockChars, personalNeed);
				}
				break;
			case 6:
				if (currBlockChars.Count > 1)
				{
					OfflineCalcGeneralAction_RecoverMainAttribute(context, periAdvanceMonthGeneralActionModification, currBlockChars, personalNeed);
				}
				break;
			case 0:
				if (currBlockChars.Count > 1)
				{
					OfflineCalcGeneralAction_IncreaseHappiness(context, periAdvanceMonthGeneralActionModification, currBlockChars, personalNeed);
				}
				break;
			case 8:
				OfflineCalcGeneralAction_GainResource(context, periAdvanceMonthGeneralActionModification, currBlockChars, currBlockGraves, personalNeed);
				break;
			case 10:
				OfflineCalcGeneralAction_GainItem(context, periAdvanceMonthGeneralActionModification, currBlockChars, currBlockGraves, personalNeed);
				break;
			case 11:
				if (currBlockChars.Count > 1)
				{
					OfflineCalcGeneralAction_RepairItem(context, periAdvanceMonthGeneralActionModification, currBlockChars, personalNeed);
				}
				break;
			case 12:
				if (currBlockChars.Count > 1)
				{
					OfflineCalcGeneralAction_AddPoisonToItem(context, periAdvanceMonthGeneralActionModification, currBlockChars, personalNeed);
				}
				break;
			case 15:
				if (currBlockChars.Count > 1)
				{
					OfflineCalcGeneralAction_LearnLifeSkill(context, periAdvanceMonthGeneralActionModification, currBlockChars, personalNeed);
				}
				break;
			case 14:
				if (currBlockChars.Count > 1)
				{
					OfflineCalcGeneralAction_LearnCombatSkill(context, periAdvanceMonthGeneralActionModification, currBlockChars, personalNeed);
				}
				break;
			case 17:
				if (currBlockChars.Count > 1)
				{
					OfflineCalcGeneralAction_AskHelpOnReading(context, periAdvanceMonthGeneralActionModification, currBlockChars, personalNeed);
				}
				break;
			case 18:
				if (currBlockChars.Count > 1)
				{
					OfflineCalcGeneralAction_AskHelpOnBreaking(context, periAdvanceMonthGeneralActionModification, currBlockChars, personalNeed);
				}
				break;
			case 16:
				OfflineCalcGeneralAction_GainExp(context, periAdvanceMonthGeneralActionModification, currBlockChars, personalNeed);
				break;
			case 9:
				if (currBlockChars.Count > 1)
				{
					OfflineCalcGeneralAction_SpendResource(context, periAdvanceMonthGeneralActionModification, currBlockChars, obj, personalNeed);
				}
				break;
			case 13:
				if (currBlockChars.Count > 1)
				{
					OfflineCalcGeneralAction_SpendItem(context, periAdvanceMonthGeneralActionModification, currBlockChars, obj, personalNeed);
				}
				break;
			}
		}
		if (currBlockChars.Count > 1)
		{
			OfflineCalcGeneralAction_RandomActions(context, periAdvanceMonthGeneralActionModification, currBlockChars, obj);
		}
		context.AdvanceMonthRelatedData.CaringCharIds.Release(ref obj);
		if (periAdvanceMonthGeneralActionModification.IsChanged || flag)
		{
			ParallelModificationsRecorder parallelModificationsRecorder = context.ParallelModificationsRecorder;
			parallelModificationsRecorder.RecordType(ParallelModificationType.PeriAdvanceMonthExecuteGeneralActions);
			parallelModificationsRecorder.RecordParameterClass(periAdvanceMonthGeneralActionModification);
		}
	}

	public static void ComplementPeriAdvanceMonth_ExecuteGeneralActions(DataContext context, PeriAdvanceMonthGeneralActionModification mod)
	{
		Character character = mod.Character;
		int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
		foreach (var (character2, generalAction) in mod.PerformedActions)
		{
			if (!CanPerformAction(character))
			{
				return;
			}
			if ((character2 == null || CanPerformAction(character2)) && generalAction.CheckValid(character, character2))
			{
				if (character2 != null && character2.GetId() == taiwuCharId)
				{
					generalAction.ApplyInitialChangesForTaiwu(context, character, character2);
				}
				else
				{
					generalAction.ApplyChanges(context, character, character2);
				}
			}
			else
			{
				character._actionEnergies.Change(generalAction.ActionEnergyType, 100);
			}
		}
		character.SetActionEnergies(character._actionEnergies, context);
	}

	private static bool CanPerformAction(Character character)
	{
		return DomainManager.Character.IsCharacterAlive(character._id) && character._kidnapperId < 0;
	}

	private unsafe bool OfflineRecoverActionEnergies()
	{
		sbyte behaviorType = GetBehaviorType();
		Personalities personalities = GetPersonalities();
		byte[] array = AiHelper.GeneralActionConstants.EnergyGainSpeed[behaviorType];
		bool result = false;
		for (sbyte b = 0; b < 5; b++)
		{
			if (_actionEnergies.Items[b] < 200)
			{
				sbyte b2 = ActionEnergyType.ToPersonalityType[b];
				int num = array[b] + personalities.Items[b2];
				_actionEnergies.Items[b] = (byte)Math.Clamp(_actionEnergies.Items[b] + num, 0, 200);
				result = true;
			}
		}
		return result;
	}

	private void OfflineCalcGeneralAction_HealInjury(DataContext context, PeriAdvanceMonthGeneralActionModification mod, HashSet<int> currBlockChars, GameData.Domains.Character.Ai.PersonalNeed personalNeed)
	{
		if (!_actionEnergies.HasEnoughForAction(0))
		{
			return;
		}
		sbyte b = sbyte.MaxValue;
		sbyte b2 = 0;
		for (sbyte b3 = 0; b3 < 7; b3++)
		{
			sbyte b4 = _injuries.Get(b3, personalNeed.InjuryType == 1);
			b2 += b4;
			if (b4 > 0 && b4 < b)
			{
				b = b4;
			}
		}
		EMedicineEffectType requiredEffectType = ((personalNeed.InjuryType == 1) ? EMedicineEffectType.RecoverInnerInjury : EMedicineEffectType.RecoverOuterInjury);
		Character character = SelectMaxPriorityActionTarget(context, currBlockChars, delegate(Character character2)
		{
			foreach (ItemKey key in character2._inventory.Items.Keys)
			{
				if (key.ItemType == 8)
				{
					MedicineItem medicineItem = Config.Medicine.Instance[key.TemplateId];
					if (medicineItem.EffectType == requiredEffectType && !TryDetectAttachedPoisons(key) && (medicineItem.RequiredMainAttributeType < 0 || _currMainAttributes[medicineItem.RequiredMainAttributeType] >= medicineItem.RequiredMainAttributeValue) && (medicineItem.Duration <= 0 || _eatingItems.GetAvailableEatingSlot(GetCurrMaxEatingSlotsCount()) >= 0))
					{
						return true;
					}
				}
			}
			return false;
		});
		if (character == null)
		{
			return;
		}
		ItemKey itemUsed = ItemKey.Invalid;
		context.AdvanceMonthRelatedData.CategorizedRegenItems(character._inventory.Items);
		List<(GameData.Domains.Item.Medicine, int)>[] array = context.AdvanceMonthRelatedData.CategorizedMedicines.Get();
		List<(GameData.Domains.Item.Medicine, int)> list = ((personalNeed.InjuryType == 1) ? array[1] : array[0]);
		list.Sort(EatingItemComparer.MedicineInjury);
		int num = SelectTopicalMedicineIndex(list, b, ref _currMainAttributes);
		if (num >= 0)
		{
			GameData.Domains.Item.Medicine item = list[num].Item1;
			itemUsed = item.GetItemKey();
		}
		else
		{
			num = SelectMedicineIndexForInjury(list, b, b2);
			if (num >= 0)
			{
				GameData.Domains.Item.Medicine item2 = list[num].Item1;
				itemUsed = item2.GetItemKey();
			}
		}
		context.AdvanceMonthRelatedData.ReleaseCategorizedRegenItems();
		if (!itemUsed.IsValid())
		{
			throw new Exception($"Fail to find valid medicine for injury {personalNeed.InjuryType} in {character}'s inventory to cure {this}.");
		}
		sbyte behaviorType = character.GetBehaviorType();
		short favorability = DomainManager.Character.GetFavorability(character.GetId(), _id);
		sbyte favorabilityType = FavorabilityType.GetFavorabilityType(favorability);
		sbyte askForHelpRespondChance = AiHelper.GeneralActionConstants.GetAskForHelpRespondChance(behaviorType, favorabilityType);
		mod.PerformedActions.Add((character, new RequestHealInjuryAction
		{
			ItemUsed = itemUsed,
			IsInnerInjury = (personalNeed.InjuryType == 1),
			AgreeToRequest = context.Random.CheckPercentProb(askForHelpRespondChance)
		}));
		_actionEnergies.SpendEnergyOnAction(0);
	}

	private unsafe void OfflineCalcGeneralAction_DetoxPoison(DataContext context, PeriAdvanceMonthGeneralActionModification mod, HashSet<int> currBlockChars, GameData.Domains.Character.Ai.PersonalNeed personalNeed)
	{
		if (!_actionEnergies.HasEnoughForAction(0))
		{
			return;
		}
		int poisonedVal = _poisoned.Items[personalNeed.PoisonType];
		sbyte poisonLevel = PoisonsAndLevels.CalcPoisonedLevel(_poisoned.Items[personalNeed.PoisonType]);
		Character character = SelectMaxPriorityActionTarget(context, currBlockChars, delegate(Character character2)
		{
			foreach (ItemKey key in character2._inventory.Items.Keys)
			{
				if (key.ItemType == 8)
				{
					MedicineItem medicineItem = Config.Medicine.Instance[key.TemplateId];
					if (medicineItem.DetoxPoisonType == personalNeed.PoisonType)
					{
						short effectThresholdValue = medicineItem.EffectThresholdValue;
						if (effectThresholdValue >= poisonLevel && !TryDetectAttachedPoisons(key))
						{
							return true;
						}
					}
				}
			}
			return false;
		});
		if (character != null)
		{
			ItemKey itemUsed = ItemKey.Invalid;
			context.AdvanceMonthRelatedData.CategorizedRegenItems(character._inventory.Items);
			List<(GameData.Domains.Item.Medicine, int)>[] array = context.AdvanceMonthRelatedData.CategorizedMedicines.Get();
			List<(GameData.Domains.Item.Medicine, int)> list = array[4];
			list.Sort(CompareDetoxPoisonMedicines);
			int num = SelectMedicineIndexForDetoxPoison(list, personalNeed.PoisonType, poisonLevel, poisonedVal);
			if (num >= 0)
			{
				GameData.Domains.Item.Medicine item = list[num].Item1;
				itemUsed = item.GetItemKey();
			}
			context.AdvanceMonthRelatedData.ReleaseCategorizedRegenItems();
			if (!itemUsed.IsValid())
			{
				throw new Exception($"Fail to find valid medicine for detox {personalNeed.PoisonType} in {character}'s inventory to cure {this}.");
			}
			sbyte behaviorType = character.GetBehaviorType();
			short favorability = DomainManager.Character.GetFavorability(character.GetId(), _id);
			sbyte favorabilityType = FavorabilityType.GetFavorabilityType(favorability);
			sbyte askForHelpRespondChance = AiHelper.GeneralActionConstants.GetAskForHelpRespondChance(behaviorType, favorabilityType);
			mod.PerformedActions.Add((character, new RequestDetoxPoisonAction
			{
				ItemUsed = itemUsed,
				PoisonType = personalNeed.PoisonType,
				AgreeToRequest = context.Random.CheckPercentProb(askForHelpRespondChance)
			}));
			_actionEnergies.SpendEnergyOnAction(0);
		}
	}

	private void OfflineCalcGeneralAction_IncreaseHealth(DataContext context, PeriAdvanceMonthGeneralActionModification mod, HashSet<int> currBlockChars, GameData.Domains.Character.Ai.PersonalNeed personalNeed)
	{
		if (!_actionEnergies.HasEnoughForAction(0))
		{
			return;
		}
		short leftMaxHealth = GetLeftMaxHealth();
		Character character = SelectMaxPriorityActionTarget(context, currBlockChars, delegate(Character character2)
		{
			foreach (ItemKey key in character2._inventory.Items.Keys)
			{
				if (key.ItemType == 8)
				{
					MedicineItem medicineItem = Config.Medicine.Instance[key.TemplateId];
					if (medicineItem.EffectType == EMedicineEffectType.RecoverHealth && !TryDetectAttachedPoisons(key))
					{
						return true;
					}
				}
			}
			return false;
		});
		if (character != null)
		{
			ItemKey itemUsed = ItemKey.Invalid;
			context.AdvanceMonthRelatedData.CategorizedRegenItems(character._inventory.Items);
			List<(GameData.Domains.Item.Medicine, int)>[] array = context.AdvanceMonthRelatedData.CategorizedMedicines.Get();
			List<(GameData.Domains.Item.Medicine, int)> list = array[2];
			list.Sort(EatingItemComparer.MedicineEffect);
			int num = SelectMedicineIndexForHealth(list, _health, leftMaxHealth);
			if (num >= 0)
			{
				GameData.Domains.Item.Medicine item = list[num].Item1;
				itemUsed = item.GetItemKey();
			}
			context.AdvanceMonthRelatedData.ReleaseCategorizedRegenItems();
			if (!itemUsed.IsValid())
			{
				throw new Exception($"Fail to find valid medicine for health in {character}'s inventory to cure {this}.");
			}
			sbyte behaviorType = character.GetBehaviorType();
			short favorability = DomainManager.Character.GetFavorability(character.GetId(), _id);
			sbyte favorabilityType = FavorabilityType.GetFavorabilityType(favorability);
			sbyte askForHelpRespondChance = AiHelper.GeneralActionConstants.GetAskForHelpRespondChance(behaviorType, favorabilityType);
			mod.PerformedActions.Add((character, new RequestIncreaseHealthAction
			{
				ItemUsed = itemUsed,
				AgreeToRequest = context.Random.CheckPercentProb(askForHelpRespondChance)
			}));
			_actionEnergies.SpendEnergyOnAction(0);
		}
	}

	private void OfflineCalcGeneralAction_RestoreDisorderOfQi(DataContext context, PeriAdvanceMonthGeneralActionModification mod, HashSet<int> currBlockChars, GameData.Domains.Character.Ai.PersonalNeed personalNeed)
	{
		if (!_actionEnergies.HasEnoughForAction(0))
		{
			return;
		}
		Character character = SelectMaxPriorityActionTarget(context, currBlockChars, delegate(Character character2)
		{
			foreach (ItemKey key in character2._inventory.Items.Keys)
			{
				if (key.ItemType == 8)
				{
					MedicineItem medicineItem = Config.Medicine.Instance[key.TemplateId];
					if (medicineItem.EffectType == EMedicineEffectType.ChangeDisorderOfQi && !TryDetectAttachedPoisons(key))
					{
						return true;
					}
				}
			}
			return false;
		});
		if (character != null)
		{
			ItemKey itemUsed = ItemKey.Invalid;
			context.AdvanceMonthRelatedData.CategorizedRegenItems(character._inventory.Items);
			List<(GameData.Domains.Item.Medicine, int)>[] array = context.AdvanceMonthRelatedData.CategorizedMedicines.Get();
			List<(GameData.Domains.Item.Medicine, int)> list = array[3];
			list.Sort(EatingItemComparer.MedicineQiDisorder);
			int num = SelectMedicineIndexForQiDisorder(list, _disorderOfQi);
			if (num >= 0)
			{
				GameData.Domains.Item.Medicine item = list[num].Item1;
				itemUsed = item.GetItemKey();
			}
			context.AdvanceMonthRelatedData.ReleaseCategorizedRegenItems();
			if (!itemUsed.IsValid())
			{
				throw new Exception($"Fail to find valid medicine for disorder of Qi in {character}'s inventory to cure {this}.");
			}
			sbyte behaviorType = character.GetBehaviorType();
			short favorability = DomainManager.Character.GetFavorability(character.GetId(), _id);
			sbyte favorabilityType = FavorabilityType.GetFavorabilityType(favorability);
			sbyte askForHelpRespondChance = AiHelper.GeneralActionConstants.GetAskForHelpRespondChance(behaviorType, favorabilityType);
			mod.PerformedActions.Add((character, new RequestRestoreQiAction
			{
				ItemUsed = itemUsed,
				AgreeToRequest = context.Random.CheckPercentProb(askForHelpRespondChance)
			}));
			_actionEnergies.SpendEnergyOnAction(0);
		}
	}

	private void OfflineCalcGeneralAction_IncreaseNeili(DataContext context, PeriAdvanceMonthGeneralActionModification mod, HashSet<int> currBlockChars, GameData.Domains.Character.Ai.PersonalNeed personalNeed)
	{
		if (!_actionEnergies.HasEnoughForAction(0))
		{
			return;
		}
		Character character = SelectMaxPriorityActionTarget(context, currBlockChars, delegate(Character character2)
		{
			foreach (ItemKey key in character2._inventory.Items.Keys)
			{
				if (key.ItemType == 12)
				{
					MiscItem miscItem = Config.Misc.Instance[key.TemplateId];
					if (miscItem.Neili > 0 && !TryDetectAttachedPoisons(key))
					{
						return true;
					}
				}
			}
			return false;
		});
		if (character != null)
		{
			ItemKey itemUsed = ItemKey.Invalid;
			context.AdvanceMonthRelatedData.CategorizedRegenItems(character._inventory.Items);
			List<(GameData.Domains.Item.Misc, int)> list = context.AdvanceMonthRelatedData.ItemsForNeili.Get();
			list.Sort(EatingItemComparer.MiscNeili);
			int num = SelectItemIndexForNeili(list, _currNeili, GetMaxNeili());
			if (num >= 0)
			{
				GameData.Domains.Item.Misc item = list[num].Item1;
				itemUsed = item.GetItemKey();
			}
			context.AdvanceMonthRelatedData.ReleaseCategorizedRegenItems();
			if (!itemUsed.IsValid())
			{
				throw new Exception($"Fail to find valid item to restore Neili in {character}'s inventory to help {this}.");
			}
			sbyte behaviorType = character.GetBehaviorType();
			short favorability = DomainManager.Character.GetFavorability(character.GetId(), _id);
			sbyte favorabilityType = FavorabilityType.GetFavorabilityType(favorability);
			sbyte askForHelpRespondChance = AiHelper.GeneralActionConstants.GetAskForHelpRespondChance(behaviorType, favorabilityType);
			mod.PerformedActions.Add((character, new RequestIncreaseNeiliAction
			{
				ItemUsed = itemUsed,
				AgreeToRequest = context.Random.CheckPercentProb(askForHelpRespondChance)
			}));
			_actionEnergies.SpendEnergyOnAction(0);
		}
	}

	private unsafe void OfflineCalcGeneralAction_KillWug(DataContext context, PeriAdvanceMonthGeneralActionModification mod, HashSet<int> currBlockChars, GameData.Domains.Character.Ai.PersonalNeed personalNeed)
	{
		if (!_actionEnergies.HasEnoughForAction(0))
		{
			return;
		}
		Character character = SelectMaxPriorityActionTarget(context, currBlockChars, delegate(Character character2)
		{
			foreach (ItemKey key in character2._inventory.Items.Keys)
			{
				if (key.ItemType == 8)
				{
					MedicineItem medicineItem = Config.Medicine.Instance[key.TemplateId];
					if (medicineItem.DetoxWugType == personalNeed.WugType)
					{
						return true;
					}
				}
			}
			return false;
		});
		if (character == null)
		{
			return;
		}
		ItemKey itemUsed = ItemKey.Invalid;
		context.AdvanceMonthRelatedData.CategorizedRegenItems(character._inventory.Items);
		List<(GameData.Domains.Item.Medicine, int)>[] array = context.AdvanceMonthRelatedData.CategorizedMedicines.Get();
		List<(GameData.Domains.Item.Medicine, int)> list = array[5];
		List<(GameData.Domains.Item.Medicine, int)> list2 = array[6];
		list.Sort(EatingItemComparer.MedicineGrade);
		list2.Sort(EatingItemComparer.MedicineGrade);
		int num = _eatingItems.IndexOfWug(personalNeed.WugType);
		int num2 = SelectMedicineIndexForWug(list, personalNeed.WugType, _eatingItems.Durations[num]);
		if (num2 >= 0)
		{
			GameData.Domains.Item.Medicine item = list[num2].Item1;
			itemUsed = item.GetItemKey();
		}
		else
		{
			num2 = SelectPoisonIndexForWug(list2, personalNeed.WugType, _eatingItems.Durations[num]);
			if (num2 >= 0)
			{
				GameData.Domains.Item.Medicine item2 = list2[num2].Item1;
				itemUsed = item2.GetItemKey();
			}
		}
		context.AdvanceMonthRelatedData.ReleaseCategorizedRegenItems();
		if (!itemUsed.IsValid())
		{
			throw new Exception($"Fail to find valid medicine for killing wug in {character}'s inventory to help {this}.");
		}
		sbyte behaviorType = character.GetBehaviorType();
		short favorability = DomainManager.Character.GetFavorability(character.GetId(), _id);
		sbyte favorabilityType = FavorabilityType.GetFavorabilityType(favorability);
		sbyte askForHelpRespondChance = AiHelper.GeneralActionConstants.GetAskForHelpRespondChance(behaviorType, favorabilityType);
		mod.PerformedActions.Add((character, new RequestKillWugAction
		{
			ItemUsed = itemUsed,
			WugType = personalNeed.WugType,
			AgreeToRequest = context.Random.CheckPercentProb(askForHelpRespondChance)
		}));
		_actionEnergies.SpendEnergyOnAction(0);
	}

	private unsafe void OfflineCalcGeneralAction_RecoverMainAttribute(DataContext context, PeriAdvanceMonthGeneralActionModification mod, HashSet<int> currBlockChars, GameData.Domains.Character.Ai.PersonalNeed personalNeed)
	{
		if (!_actionEnergies.HasEnoughForAction(0))
		{
			return;
		}
		sbyte behaviorType = GetBehaviorType();
		short aiActionRateAdjust = DomainManager.Extra.GetAiActionRateAdjust(_id, 6, -1);
		sbyte b = AiHelper.UpdateStatusConstants.EatForbiddenFoodChance[behaviorType];
		bool flag = IsForbiddenToEatMeat();
		bool allowMeat = !flag || context.Random.CheckPercentProb(b + aiActionRateAdjust);
		Character character = SelectMaxPriorityActionTarget(context, currBlockChars, delegate(Character character2)
		{
			foreach (ItemKey key in character2._inventory.Items.Keys)
			{
				if (key.ItemType == 7)
				{
					FoodItem foodItem = Config.Food.Instance[key.TemplateId];
					if (foodItem.MainAttributesRegen.Items[personalNeed.MainAttributeType] > 0 && !TryDetectAttachedPoisons(key))
					{
						if (foodItem.ItemSubType != 701)
						{
							return true;
						}
						if (allowMeat)
						{
							return true;
						}
					}
				}
			}
			return false;
		});
		if (character != null)
		{
			ItemKey itemUsed = ItemKey.Invalid;
			context.AdvanceMonthRelatedData.CategorizedRegenItems(character._inventory.Items);
			List<(GameData.Domains.Item.Food, int)>[] array = context.AdvanceMonthRelatedData.FoodsForMainAttributes.Get();
			List<(GameData.Domains.Item.Food, int)> list = array[personalNeed.MainAttributeType];
			list.Sort(EatingItemComparer.FoodMainAttributes[personalNeed.MainAttributeType]);
			MainAttributes maxMainAttributes = GetMaxMainAttributes();
			int num = SelectFoodIndexForMainAttributes(list, personalNeed.MainAttributeType, _currMainAttributes.Items[personalNeed.MainAttributeType], maxMainAttributes.Items[personalNeed.MainAttributeType], allowMeat);
			if (num >= 0)
			{
				GameData.Domains.Item.Food item = list[num].Item1;
				itemUsed = item.GetItemKey();
			}
			context.AdvanceMonthRelatedData.ReleaseCategorizedRegenItems();
			if (!itemUsed.IsValid())
			{
				throw new Exception($"Fail to find valid medicine for main attribute recovery in {character}'s inventory to help {this}.");
			}
			sbyte behaviorType2 = character.GetBehaviorType();
			short favorability = DomainManager.Character.GetFavorability(character.GetId(), _id);
			sbyte favorabilityType = FavorabilityType.GetFavorabilityType(favorability);
			sbyte askForHelpRespondChance = AiHelper.GeneralActionConstants.GetAskForHelpRespondChance(behaviorType2, favorabilityType);
			mod.PerformedActions.Add((character, new RequestRecoverMainAttributeAction
			{
				ItemUsed = itemUsed,
				MainAttributeType = personalNeed.MainAttributeType,
				AgreeToRequest = context.Random.CheckPercentProb(askForHelpRespondChance)
			}));
			_actionEnergies.SpendEnergyOnAction(0);
		}
	}

	private void OfflineCalcGeneralAction_IncreaseHappiness(DataContext context, PeriAdvanceMonthGeneralActionModification mod, HashSet<int> currBlockChars, GameData.Domains.Character.Ai.PersonalNeed personalNeed)
	{
		if (!_actionEnergies.HasEnoughForAction(0) || GetAgeGroup() != 2)
		{
			return;
		}
		sbyte behaviorType = GetBehaviorType();
		short aiActionRateAdjust = DomainManager.Extra.GetAiActionRateAdjust(_id, 6, -1);
		sbyte b = AiHelper.UpdateStatusConstants.EatForbiddenFoodChance[behaviorType];
		bool flag = IsForbiddenToDrinkingWines();
		bool allowWines = !flag || context.Random.CheckPercentProb(b + aiActionRateAdjust);
		Character character = SelectMaxPriorityActionTarget(context, currBlockChars, delegate(Character character2)
		{
			foreach (ItemKey key in character2._inventory.Items.Keys)
			{
				if (key.ItemType == 9)
				{
					TeaWineItem teaWineItem = Config.TeaWine.Instance[key.TemplateId];
					if (DomainManager.Item.GetBaseItem(key).GetHappinessChange() > 0 && !TryDetectAttachedPoisons(key))
					{
						if (teaWineItem.ItemSubType == 900)
						{
							return true;
						}
						if (allowWines)
						{
							return true;
						}
					}
				}
			}
			return false;
		});
		if (character != null)
		{
			ItemKey itemUsed = ItemKey.Invalid;
			context.AdvanceMonthRelatedData.CategorizedRegenItems(character._inventory.Items);
			List<(GameData.Domains.Item.TeaWine, int)> list = context.AdvanceMonthRelatedData.TeaWinesForHappiness.Get();
			list.Sort(EatingItemComparer.TeaWineHappiness);
			int num = SelectTeaWineForHappiness(list, _happiness, HappinessType.Ranges[3].min, allowWines);
			if (num >= 0)
			{
				GameData.Domains.Item.TeaWine item = list[num].Item1;
				itemUsed = item.GetItemKey();
			}
			context.AdvanceMonthRelatedData.ReleaseCategorizedRegenItems();
			if (!itemUsed.IsValid())
			{
				throw new Exception($"Fail to find valid tea or wine for happiness in {character}'s inventory to help {this}.");
			}
			sbyte behaviorType2 = character.GetBehaviorType();
			short favorability = DomainManager.Character.GetFavorability(character.GetId(), _id);
			sbyte favorabilityType = FavorabilityType.GetFavorabilityType(favorability);
			sbyte askForHelpRespondChance = AiHelper.GeneralActionConstants.GetAskForHelpRespondChance(behaviorType2, favorabilityType);
			mod.PerformedActions.Add((character, new RequestIncreaseHappinessAction
			{
				ItemUsed = itemUsed,
				AgreeToRequest = context.Random.CheckPercentProb(askForHelpRespondChance)
			}));
			_actionEnergies.SpendEnergyOnAction(0);
		}
	}

	private unsafe void OfflineCalcGeneralAction_GainResource(DataContext context, PeriAdvanceMonthGeneralActionModification mod, HashSet<int> currBlockChars, HashSet<int> currBlockGraves, GameData.Domains.Character.Ai.PersonalNeed personalNeed)
	{
		if (!_actionEnergies.HasEnoughForAction(1) || OfflineCalcGeneralAction_TakeTreasuryResource(context.Random, mod, personalNeed.ResourceType, personalNeed.Amount))
		{
			return;
		}
		var (num, b) = SelectDemandActionTarget(context, currBlockChars, currBlockGraves, (Character character) => character._resources.Items[personalNeed.ResourceType] >= personalNeed.Amount, delegate(Grave grave)
		{
			ResourceInts resources = grave.GetResources();
			return resources.Items[personalNeed.ResourceType] >= personalNeed.Amount;
		}, 1);
		if (num < 0)
		{
			return;
		}
		sbyte b2 = AiHelper.DemandActionType.ToMainAttributeType(b, isSkill: false);
		if (b2 < 0 || GetCurrMainAttribute(b2) >= GlobalConfig.Instance.HarmfulActionCost)
		{
			short favorability = DomainManager.Character.GetFavorability(num, _id);
			sbyte favorabilityType = FavorabilityType.GetFavorabilityType(favorability);
			Character element;
			int alertFactor = (DomainManager.Character.TryGetElement_Objects(num, out element) ? element.GetResourceAlertFactor(personalNeed.ResourceType) : 0);
			switch (b)
			{
			case 0:
			{
				sbyte behaviorType = element.GetBehaviorType();
				sbyte askForHelpRespondChance = AiHelper.GeneralActionConstants.GetAskForHelpRespondChance(behaviorType, favorabilityType);
				mod.PerformedActions.Add((element, new RequestResourceAction
				{
					AgreeToRequest = context.Random.CheckPercentProb(askForHelpRespondChance),
					ResourceType = personalNeed.ResourceType,
					Amount = personalNeed.Amount
				}));
				break;
			}
			case 1:
				mod.PerformedActions.Add((element, new StealResourceAction
				{
					ResourceType = personalNeed.ResourceType,
					Amount = personalNeed.Amount,
					Phase = GetStealActionPhase(context.Random, element, alertFactor)
				}));
				break;
			case 2:
				mod.PerformedActions.Add((element, new ScamResourceAction
				{
					ResourceType = personalNeed.ResourceType,
					Amount = personalNeed.Amount,
					Phase = GetScamActionPhase(context.Random, element, alertFactor)
				}));
				break;
			case 3:
				mod.PerformedActions.Add((element, new RobResourceAction
				{
					ResourceType = personalNeed.ResourceType,
					Amount = personalNeed.Amount,
					Phase = GetRobActionPhase(context.Random, element, alertFactor)
				}));
				break;
			case 4:
			{
				Personalities personalities = GetPersonalities();
				int percentProb = 7 * (100 + personalities.Items[6] * 5) / 35;
				mod.PerformedActions.Add((null, new RobGraveResourceAction
				{
					ResourceType = personalNeed.ResourceType,
					Amount = personalNeed.Amount,
					Succeed = context.Random.CheckPercentProb(percentProb),
					TargetGraveId = num
				}));
				break;
			}
			}
			_actionEnergies.SpendEnergyOnAction(1);
		}
	}

	private unsafe void OfflineCalcGeneralAction_GainItem(DataContext context, PeriAdvanceMonthGeneralActionModification mod, HashSet<int> currBlockChars, HashSet<int> currBlockGraves, GameData.Domains.Character.Ai.PersonalNeed personalNeed)
	{
		IRandomSource random = context.Random;
		if (!_actionEnergies.HasEnoughForAction(1))
		{
			return;
		}
		ItemKey targetItem = DomainManager.Merchant.TryGetBuyBackItemForPersonalNeed(context, personalNeed);
		if (targetItem.IsValid())
		{
			mod.PerformedActions.Add((null, new PurchaseBuybackItemAction
			{
				TargetItem = targetItem
			}));
			_actionEnergies.SpendEnergyOnAction(1);
		}
		else
		{
			if (OfflineCalcGeneralAction_TakeTreasuryItem(context.Random, mod, personalNeed) || personalNeed.ItemType == 3 || ((context.Random.NextBool() || _resources.Items[6] > GetAdjustedResourceSatisfyingThreshold(6)) && personalNeed.ItemType != 11 && IsInRegularSettlementRange() && (OfflineCalcGeneralAction_MakeArtisanOrder(context, mod, personalNeed) || OfflineCalcGeneralAction_PurchaseItem(context, mod, personalNeed))))
			{
				return;
			}
			var (num, b) = SelectDemandActionTarget(context, currBlockChars, currBlockGraves, (Character character) => character._inventory.Items.Any((KeyValuePair<ItemKey, int> pair) => pair.Key.ItemType == personalNeed.ItemType && pair.Key.TemplateId == personalNeed.ItemTemplateId), (Grave grave) => grave.GetInventory().Items.Any((KeyValuePair<ItemKey, int> pair) => pair.Key.ItemType == personalNeed.ItemType && pair.Key.TemplateId == personalNeed.ItemTemplateId), 0);
			if (num < 0)
			{
				return;
			}
			ItemKey itemKey = ItemKey.Invalid;
			int amount = 1;
			ItemKey key;
			int value;
			if (DomainManager.Character.TryGetElement_Objects(num, out var element))
			{
				foreach (KeyValuePair<ItemKey, int> item in element._inventory.Items)
				{
					item.Deconstruct(out key, out value);
					ItemKey itemKey2 = key;
					int num2 = value;
					if (itemKey2.ItemType != personalNeed.ItemType || itemKey2.TemplateId != personalNeed.ItemTemplateId)
					{
						continue;
					}
					itemKey = itemKey2;
					amount = 1;
					break;
				}
			}
			else
			{
				Grave element_Graves = DomainManager.Character.GetElement_Graves(num);
				Inventory inventory = element_Graves.GetInventory();
				int num3 = context.Random.Next(inventory.Items.Count);
				foreach (KeyValuePair<ItemKey, int> item2 in inventory.Items)
				{
					item2.Deconstruct(out key, out value);
					ItemKey itemKey3 = key;
					int num4 = value;
					if (num3 <= 0)
					{
						itemKey = itemKey3;
						amount = num4;
						break;
					}
					num3--;
				}
			}
			if (!itemKey.IsValid())
			{
				throw new Exception($"Failed to find target item {personalNeed} in selected character {num}'s inventory for {this} to request.");
			}
			sbyte b2 = AiHelper.DemandActionType.ToMainAttributeType(b, isSkill: false);
			if (b2 >= 0 && GetCurrMainAttribute(b2) < GlobalConfig.Instance.HarmfulActionCost)
			{
				return;
			}
			short favorability = DomainManager.Character.GetFavorability(num, _id);
			sbyte favorabilityType = FavorabilityType.GetFavorabilityType(favorability);
			switch (b)
			{
			case 0:
			{
				sbyte behaviorType3 = element.GetBehaviorType();
				sbyte percentProb4 = AiHelper.GeneralActionConstants.AddPoisonOnTransferItemChance[behaviorType3];
				sbyte askForHelpRespondChance = AiHelper.GeneralActionConstants.GetAskForHelpRespondChance(behaviorType3, favorabilityType);
				bool flag = context.Random.CheckPercentProb(askForHelpRespondChance);
				ItemKey[] poisonsToAdd3 = ((flag && random.CheckPercentProb(percentProb4)) ? element.SelectInventoryPoisonsToAdd(random, itemKey) : null);
				mod.PerformedActions.Add((element, new RequestItemAction
				{
					TargetItem = itemKey,
					Amount = amount,
					AgreeToRequest = flag,
					PoisonsToAdd = poisonsToAdd3
				}));
				break;
			}
			case 1:
			{
				int itemAlertFactor2 = element.GetItemAlertFactor(itemKey, amount);
				sbyte stealActionPhase = GetStealActionPhase(random, element, itemAlertFactor2);
				ItemKey[] poisonsToAdd2 = null;
				if (stealActionPhase >= 5)
				{
					sbyte behaviorType2 = element.GetBehaviorType();
					sbyte percentProb3 = AiHelper.GeneralActionConstants.AddPoisonOnTransferItemChance[behaviorType2];
					if (random.CheckPercentProb(percentProb3))
					{
						poisonsToAdd2 = element.SelectInventoryPoisonsToAdd(random, itemKey);
					}
				}
				mod.PerformedActions.Add((element, new StealItemAction
				{
					TargetItem = itemKey,
					Amount = amount,
					Phase = stealActionPhase,
					PoisonsToAdd = poisonsToAdd2
				}));
				break;
			}
			case 2:
			{
				int itemAlertFactor3 = element.GetItemAlertFactor(itemKey, amount);
				sbyte scamActionPhase = GetScamActionPhase(context.Random, element, itemAlertFactor3);
				ItemKey[] poisonsToAdd4 = null;
				if (scamActionPhase >= 5)
				{
					sbyte behaviorType4 = element.GetBehaviorType();
					sbyte percentProb5 = AiHelper.GeneralActionConstants.AddPoisonOnTransferItemChance[behaviorType4];
					if (random.CheckPercentProb(percentProb5))
					{
						poisonsToAdd4 = element.SelectInventoryPoisonsToAdd(random, itemKey);
					}
				}
				mod.PerformedActions.Add((element, new ScamItemAction
				{
					TargetItem = itemKey,
					Amount = amount,
					Phase = scamActionPhase,
					PoisonsToAdd = poisonsToAdd4
				}));
				break;
			}
			case 3:
			{
				int itemAlertFactor = element.GetItemAlertFactor(itemKey, amount);
				sbyte robActionPhase = GetRobActionPhase(context.Random, element, itemAlertFactor);
				ItemKey[] poisonsToAdd = null;
				if (robActionPhase >= 5)
				{
					sbyte behaviorType = element.GetBehaviorType();
					sbyte percentProb2 = AiHelper.GeneralActionConstants.AddPoisonOnTransferItemChance[behaviorType];
					if (random.CheckPercentProb(percentProb2))
					{
						poisonsToAdd = element.SelectInventoryPoisonsToAdd(random, itemKey);
					}
				}
				mod.PerformedActions.Add((element, new RobItemAction
				{
					TargetItem = itemKey,
					Amount = amount,
					Phase = robActionPhase,
					PoisonsToAdd = poisonsToAdd
				}));
				break;
			}
			case 4:
			{
				Personalities personalities = GetPersonalities();
				Grave element_Graves2 = DomainManager.Character.GetElement_Graves(num);
				int percentProb = element_Graves2.GetInventory().Items.Count * (100 + personalities.Items[6] * 5) / 35;
				mod.PerformedActions.Add((element, new RobGraveItemAction
				{
					TargetItem = itemKey,
					Amount = amount,
					TargetGraveId = num,
					Succeed = context.Random.CheckPercentProb(percentProb)
				}));
				break;
			}
			}
			_actionEnergies.SpendEnergyOnAction(1);
		}
	}

	private void OfflineCalcGeneralAction_RepairItem(DataContext context, PeriAdvanceMonthGeneralActionModification mod, HashSet<int> currBlockChars, GameData.Domains.Character.Ai.PersonalNeed personalNeed)
	{
		if (!_actionEnergies.HasEnoughForAction(1))
		{
			return;
		}
		ItemBase baseItem = DomainManager.Item.GetBaseItem(new ItemKey(personalNeed.ItemType, 0, 0, personalNeed.ItemId));
		ItemKey itemKey = baseItem.GetItemKey();
		sbyte grade = baseItem.GetGrade();
		short currDurability = baseItem.GetCurrDurability();
		sbyte requiredLifeSkillType = ItemTemplateHelper.GetCraftRequiredLifeSkillType(itemKey.ItemType, itemKey.TemplateId);
		short attainmentRequired = ItemTemplateHelper.GetRepairRequiredAttainment(itemKey.ItemType, itemKey.TemplateId, currDurability);
		sbyte requiredResourceType = ItemTemplateHelper.GetCraftRequiredResourceType(itemKey.ItemType, itemKey.TemplateId);
		EquipmentBase baseEquipment = DomainManager.Item.GetBaseEquipment(itemKey);
		int requiredResourceAmount = ItemTemplateHelper.GetRepairNeedResourceCount(baseEquipment.GetMaterialResources(), itemKey, currDurability);
		Character character = SelectMaxPriorityActionTarget(context, currBlockChars, delegate(Character character2)
		{
			if (character2.GetResource(requiredResourceType) < requiredResourceAmount)
			{
				return false;
			}
			short lifeSkillAttainment2 = character2.GetLifeSkillAttainment(requiredLifeSkillType);
			foreach (ItemKey key in character2._inventory.Items.Keys)
			{
				if (key.ItemType == 6)
				{
					ItemBase baseItem2 = DomainManager.Item.GetBaseItem(key);
					CraftToolItem craftToolItem = Config.CraftTool.Instance[key.TemplateId];
					short num = craftToolItem.DurabilityCost[grade];
					if (baseItem2.GetCurrDurability() >= num && craftToolItem.RequiredLifeSkillTypes.Contains(requiredLifeSkillType) && lifeSkillAttainment2 + craftToolItem.AttainmentBonus >= attainmentRequired)
					{
						return true;
					}
				}
			}
			return false;
		});
		if (character != null)
		{
			short favorability = DomainManager.Character.GetFavorability(character.GetId(), _id);
			sbyte favorabilityType = FavorabilityType.GetFavorabilityType(favorability);
			sbyte behaviorType = character.GetBehaviorType();
			sbyte askForHelpRespondChance = AiHelper.GeneralActionConstants.GetAskForHelpRespondChance(behaviorType, favorabilityType);
			short lifeSkillAttainment = character.GetLifeSkillAttainment(requiredLifeSkillType);
			short durabilityCost;
			ItemKey worstUsableCraftTool = character._inventory.GetWorstUsableCraftTool(requiredLifeSkillType, attainmentRequired, lifeSkillAttainment, baseItem.GetGrade(), out durabilityCost);
			if (!worstUsableCraftTool.IsValid())
			{
				throw new Exception($"Failed to find target tool to repair {personalNeed} in selected character {character}'s inventory for {this}.");
			}
			mod.PerformedActions.Add((character, new RequestRepairItemAction
			{
				TargetItem = itemKey,
				ToolUsed = worstUsableCraftTool,
				ResourceType = requiredResourceType,
				ResourceAmount = requiredResourceAmount,
				AgreeToRequest = context.Random.CheckPercentProb(askForHelpRespondChance),
				ToolDurabilityCost = durabilityCost
			}));
			_actionEnergies.SpendEnergyOnAction(1);
		}
	}

	private bool OfflineCalcGeneralAction_PurchaseItem(DataContext context, PeriAdvanceMonthGeneralActionModification mod, GameData.Domains.Character.Ai.PersonalNeed personalNeed, Character targetChar = null)
	{
		int num = ItemTemplateHelper.GetBaseValue(personalNeed.ItemType, personalNeed.ItemTemplateId) * 2 * OrganizationDomain.GetOrgMemberConfig(_organizationInfo).PurchaseItemDiscount / 100;
		if (num <= 0 || _resources[6] < num)
		{
			return false;
		}
		mod.PerformedActions.Add((targetChar, new PurchaseItemAction
		{
			MoneyCost = num,
			PurchasedItem = new TemplateKey(personalNeed.ItemType, personalNeed.ItemTemplateId),
			ItemAmount = 1
		}));
		return true;
	}

	private bool OfflineCalcGeneralAction_MakeArtisanOrder(DataContext context, PeriAdvanceMonthGeneralActionModification mod, GameData.Domains.Character.Ai.PersonalNeed personalNeed, int giftTargetCharId = -1)
	{
		if (!ItemTemplateHelper.CanMakeArtisanOrder(personalNeed.ItemType, personalNeed.ItemTemplateId))
		{
			return false;
		}
		sbyte craftRequiredLifeSkillType = ItemTemplateHelper.GetCraftRequiredLifeSkillType(personalNeed.ItemType, personalNeed.ItemTemplateId);
		if (craftRequiredLifeSkillType < 0)
		{
			return false;
		}
		short itemSubType = ItemTemplateHelper.GetItemSubType(personalNeed.ItemType, personalNeed.ItemTemplateId);
		if (DomainManager.Extra.IsItemSubTypeSubscribed(_id, itemSubType))
		{
			return true;
		}
		List<Character> list = ObjectPool<List<Character>>.Instance.Get();
		MapCharacterFilter.Find((Character character) => character.IsInteractableAsIntelligentCharacter() && character.IdentifyCanCraftItem(personalNeed.ItemType, personalNeed.ItemTemplateId) && DomainManager.Extra.IsArtisanIdle(character.GetId()), list, _location.AreaId);
		Character randomOrDefault = list.GetRandomOrDefault(context.Random, null);
		ObjectPool<List<Character>>.Instance.Return(list);
		if (randomOrDefault == null)
		{
			return true;
		}
		mod.PerformedActions.Add((randomOrDefault, new MakeArtisanOrderAction
		{
			ItemSubType = itemSubType,
			LifeSkillType = craftRequiredLifeSkillType,
			GiftTargetCharId = giftTargetCharId
		}));
		if (giftTargetCharId >= 0)
		{
			_actionEnergies.SpendEnergyOnAction(3);
		}
		else
		{
			_actionEnergies.SpendEnergyOnAction(1);
		}
		return true;
	}

	private unsafe void OfflineCalcGeneralAction_AddPoisonToItem(DataContext context, PeriAdvanceMonthGeneralActionModification mod, HashSet<int> currBlockChars, GameData.Domains.Character.Ai.PersonalNeed personalNeed)
	{
		if (!_actionEnergies.HasEnoughForAction(1))
		{
			return;
		}
		Character character = SelectMaxPriorityActionTarget(context, currBlockChars, delegate(Character character2)
		{
			short lifeSkillAttainment2 = character2.GetLifeSkillAttainment(9);
			foreach (ItemKey key in character2._inventory.Items.Keys)
			{
				if (key.ItemType == 8)
				{
					MedicineItem medicineItem2 = Config.Medicine.Instance[key.TemplateId];
					if (medicineItem2.EffectType == EMedicineEffectType.ApplyPoison)
					{
						sbyte poisonType = medicineItem2.PoisonType;
						short num5 = GlobalConfig.Instance.PoisonAttainments[medicineItem2.Grade];
						if (poisonType == personalNeed.PoisonType && lifeSkillAttainment2 >= num5)
						{
							return true;
						}
					}
				}
			}
			return false;
		});
		if (character == null)
		{
			return;
		}
		short favorability = DomainManager.Character.GetFavorability(character.GetId(), _id);
		sbyte favorabilityType = FavorabilityType.GetFavorabilityType(favorability);
		sbyte behaviorType = character.GetBehaviorType();
		sbyte askForHelpRespondChance = AiHelper.GeneralActionConstants.GetAskForHelpRespondChance(behaviorType, favorabilityType);
		short lifeSkillAttainment = character.GetLifeSkillAttainment(9);
		int num = 16129;
		ItemKey poisonUsed = ItemKey.Invalid;
		foreach (ItemKey key2 in character._inventory.Items.Keys)
		{
			if (key2.ItemType != 8)
			{
				continue;
			}
			MedicineItem medicineItem = Config.Medicine.Instance[key2.TemplateId];
			if (medicineItem.EffectType != EMedicineEffectType.ApplyPoison)
			{
				continue;
			}
			sbyte applyPoisonType = medicineItem.ApplyPoisonType;
			short num2 = GlobalConfig.Instance.PoisonAttainments[medicineItem.Grade];
			if (applyPoisonType == personalNeed.PoisonType && lifeSkillAttainment >= num2)
			{
				int num3 = medicineItem.Grade - _organizationInfo.Grade;
				int num4 = num3 * num3;
				if (num4 <= num)
				{
					num = num4;
					poisonUsed = key2;
				}
			}
		}
		if (!poisonUsed.IsValid())
		{
			throw new Exception($"Failed to find target poison {personalNeed} in selected character {character}'s inventory for {this}.");
		}
		MedicineItem poisonConfig = Config.Medicine.Instance[poisonUsed.TemplateId];
		sbyte* equipmentArr = stackalloc sbyte[3] { 0, 1, 2 };
		sbyte b = SelectEquipmentInArrayToAddPoisonOn(context.Random, poisonConfig, equipmentArr, 3);
		if (b >= 0)
		{
			mod.PerformedActions.Add((character, new RequestAddPoisonToItemAction
			{
				TargetItem = _equipment[b],
				PoisonUsed = poisonUsed,
				AgreeToRequest = context.Random.CheckPercentProb(askForHelpRespondChance)
			}));
			_actionEnergies.SpendEnergyOnAction(1);
			return;
		}
		sbyte* equipmentArr2 = stackalloc sbyte[4] { 3, 5, 6, 7 };
		sbyte b2 = SelectEquipmentInArrayToAddPoisonOn(context.Random, poisonConfig, equipmentArr2, 4);
		if (b2 >= 0)
		{
			mod.PerformedActions.Add((character, new RequestAddPoisonToItemAction
			{
				TargetItem = _equipment[b2],
				PoisonUsed = poisonUsed,
				AgreeToRequest = context.Random.CheckPercentProb(askForHelpRespondChance)
			}));
			_actionEnergies.SpendEnergyOnAction(1);
			return;
		}
		sbyte* equipmentArr3 = stackalloc sbyte[3] { 8, 9, 10 };
		sbyte b3 = SelectEquipmentInArrayToAddPoisonOn(context.Random, poisonConfig, equipmentArr3, 3);
		if (b3 >= 0)
		{
			mod.PerformedActions.Add((character, new RequestAddPoisonToItemAction
			{
				TargetItem = _equipment[b3],
				PoisonUsed = poisonUsed,
				AgreeToRequest = context.Random.CheckPercentProb(askForHelpRespondChance)
			}));
			_actionEnergies.SpendEnergyOnAction(1);
		}
	}

	public bool CanLearnLifeSkillFrom(Character character, sbyte lifeSkillType = -1)
	{
		foreach (LifeSkillItem learnedLifeSkill in character._learnedLifeSkills)
		{
			Config.LifeSkillItem lifeSkillItem = LifeSkill.Instance[learnedLifeSkill.SkillTemplateId];
			if (lifeSkillItem.SkillBookId >= 0 && (lifeSkillItem.Type == lifeSkillType || lifeSkillType < 0))
			{
				int num = FindLearnedLifeSkillIndex(learnedLifeSkill.SkillTemplateId);
				if (num < 0 && learnedLifeSkill.ReadingState != 0)
				{
					return true;
				}
			}
		}
		return false;
	}

	public (short skillId, byte pageId) CalcLifeSkillToLearnFromCharacter(DataContext context, Character character, sbyte lifeSkillType = -1)
	{
		List<(short, short)> obj = context.AdvanceMonthRelatedData.WeightTable.Occupy();
		for (short num = 0; num < character._learnedLifeSkills.Count; num++)
		{
			LifeSkillItem lifeSkillItem = character._learnedLifeSkills[num];
			Config.LifeSkillItem lifeSkillItem2 = LifeSkill.Instance[lifeSkillItem.SkillTemplateId];
			if (lifeSkillItem2.SkillBookId >= 0 && (lifeSkillItem2.Type == lifeSkillType || lifeSkillType < 0) && FindLearnedLifeSkillIndex(lifeSkillItem.SkillTemplateId) < 0 && lifeSkillItem.ReadingState != 0)
			{
				obj.Add((num, (short)(3 << 8 - lifeSkillItem2.Grade)));
			}
		}
		if (obj.Count == 0)
		{
			context.AdvanceMonthRelatedData.WeightTable.Release(ref obj);
			return (skillId: -1, pageId: 0);
		}
		short randomResult = RandomUtils.GetRandomResult(obj, context.Random);
		context.AdvanceMonthRelatedData.WeightTable.Release(ref obj);
		LifeSkillItem lifeSkillItem3 = character._learnedLifeSkills[randomResult];
		short skillTemplateId = lifeSkillItem3.SkillTemplateId;
		byte b = 0;
		while (b < 5 && !lifeSkillItem3.IsPageRead(b))
		{
			b++;
		}
		return (skillId: skillTemplateId, pageId: b);
	}

	public bool CanLearnCombatSkillFrom(Character character, sbyte combatSkillType = -1)
	{
		int id = character.GetId();
		Dictionary<short, GameData.Domains.CombatSkill.CombatSkill> charCombatSkills = DomainManager.CombatSkill.GetCharCombatSkills(id);
		Dictionary<short, GameData.Domains.CombatSkill.CombatSkill> charCombatSkills2 = DomainManager.CombatSkill.GetCharCombatSkills(_id);
		foreach (KeyValuePair<short, GameData.Domains.CombatSkill.CombatSkill> item in charCombatSkills)
		{
			item.Deconstruct(out var key, out var value);
			short num = key;
			GameData.Domains.CombatSkill.CombatSkill combatSkill = value;
			CombatSkillItem combatSkillItem = Config.CombatSkill.Instance[num];
			if (combatSkillItem.BookId < 0 || (combatSkillItem.Type != combatSkillType && combatSkillType >= 0) || charCombatSkills2.ContainsKey(num) || combatSkill.GetReadingState() == 0)
			{
				continue;
			}
			return true;
		}
		return false;
	}

	public (short skillId, byte internalIndex, byte pageTypes) CalcCombatSkillToLearnFromCharacter(DataContext context, Character character, sbyte combatSkillType = -1)
	{
		Dictionary<short, GameData.Domains.CombatSkill.CombatSkill> charCombatSkills = DomainManager.CombatSkill.GetCharCombatSkills(character.GetId());
		Dictionary<short, GameData.Domains.CombatSkill.CombatSkill> charCombatSkills2 = DomainManager.CombatSkill.GetCharCombatSkills(_id);
		List<(short, short)> obj = context.AdvanceMonthRelatedData.WeightTable.Occupy();
		foreach (KeyValuePair<short, GameData.Domains.CombatSkill.CombatSkill> item2 in charCombatSkills)
		{
			item2.Deconstruct(out var key, out var value);
			short num = key;
			GameData.Domains.CombatSkill.CombatSkill combatSkill = value;
			CombatSkillItem combatSkillItem = Config.CombatSkill.Instance[num];
			if (combatSkillItem.BookId >= 0 && (combatSkillItem.Type == combatSkillType || combatSkillType < 0) && !charCombatSkills2.ContainsKey(num) && combatSkill.GetReadingState() != 0)
			{
				int num2 = 3 << 8 - combatSkillItem.Grade;
				if (combatSkillItem.IsNonPublic)
				{
					num2 /= 3;
				}
				obj.Add((num, (short)num2));
			}
		}
		if (obj.Count == 0)
		{
			context.AdvanceMonthRelatedData.WeightTable.Release(ref obj);
			return (skillId: -1, internalIndex: 0, pageTypes: 0);
		}
		short randomResult = RandomUtils.GetRandomResult(obj, context.Random);
		context.AdvanceMonthRelatedData.WeightTable.Release(ref obj);
		GameData.Domains.CombatSkill.CombatSkill combatSkill2 = charCombatSkills[randomResult];
		ushort readingState = combatSkill2.GetReadingState();
		byte b = 0;
		while (b < 15 && !CombatSkillStateHelper.IsPageRead(readingState, b))
		{
			b++;
		}
		byte item = CombatSkillStateHelper.GeneratePageTypesFromReadingState(context.Random, readingState);
		return (skillId: randomResult, internalIndex: b, pageTypes: item);
	}

	private void OfflineCalcGeneralAction_LearnLifeSkill(DataContext context, PeriAdvanceMonthGeneralActionModification mod, HashSet<int> currBlockChars, GameData.Domains.Character.Ai.PersonalNeed personalNeed)
	{
		if (!_actionEnergies.HasEnoughForAction(2))
		{
			return;
		}
		var (num, b) = SelectDemandActionTarget(context, currBlockChars, null, (Character character) => CanLearnLifeSkillFrom(character, personalNeed.LifeSkillType), null, 3);
		if (num < 0)
		{
			return;
		}
		sbyte b2 = AiHelper.DemandActionType.ToMainAttributeType(b, isSkill: true);
		if (b2 >= 0 && GetCurrMainAttribute(b2) < GlobalConfig.Instance.HarmfulActionCost)
		{
			return;
		}
		Character element_Objects = DomainManager.Character.GetElement_Objects(num);
		short favorability = DomainManager.Character.GetFavorability(num, _id);
		sbyte favorabilityType = FavorabilityType.GetFavorabilityType(favorability);
		(short skillId, byte pageId) tuple2 = CalcLifeSkillToLearnFromCharacter(context, element_Objects, personalNeed.LifeSkillType);
		short item = tuple2.skillId;
		byte item2 = tuple2.pageId;
		Config.LifeSkillItem lifeSkillItem = LifeSkill.Instance[item];
		short skillBookId = lifeSkillItem.SkillBookId;
		sbyte grade = LifeSkill.Instance[item].Grade;
		switch (b)
		{
		case 0:
		{
			int askToTeachSkillRespondChance = AiHelper.GeneralActionConstants.GetAskToTeachSkillRespondChance(this, element_Objects, favorabilityType, grade);
			bool flag = context.Random.CheckPercentProb(askToTeachSkillRespondChance);
			bool succeed = false;
			if (flag)
			{
				short lifeSkillAttainment = GetLifeSkillAttainment(personalNeed.LifeSkillType);
				short lifeSkillQualification = GetLifeSkillQualification(personalNeed.LifeSkillType);
				sbyte personality = GetPersonality(1);
				int taughtNewSkillSuccessRate = GetTaughtNewSkillSuccessRate(grade, lifeSkillQualification, lifeSkillAttainment, personality);
				succeed = context.Random.CheckPercentProb(taughtNewSkillSuccessRate);
			}
			mod.PerformedActions.Add((element_Objects, new RequestLifeSkillDemandAction
			{
				BookTemplateId = skillBookId,
				PageId = item2,
				AgreeToRequest = flag,
				Succeed = succeed
			}));
			break;
		}
		case 1:
			mod.PerformedActions.Add((element_Objects, new StealLifeSkillDemandAction
			{
				BookTemplateId = skillBookId,
				PageId = item2,
				Phase = GetStealLifeSkillActionPhase(context.Random, element_Objects, personalNeed.LifeSkillType, grade)
			}));
			break;
		case 2:
		{
			int gradeAlertFactor = element_Objects.GetGradeAlertFactor(grade, 1);
			mod.PerformedActions.Add((element_Objects, new ScamLifeSkillDemandAction
			{
				BookTemplateId = skillBookId,
				PageId = item2,
				Phase = GetScamActionPhase(context.Random, element_Objects, gradeAlertFactor)
			}));
			break;
		}
		default:
			throw new Exception($"Unrecognized request action type {b} for {this} to {num}");
		}
		_actionEnergies.SpendEnergyOnAction(2);
	}

	private void OfflineCalcGeneralAction_LearnCombatSkill(DataContext context, PeriAdvanceMonthGeneralActionModification mod, HashSet<int> currBlockChars, GameData.Domains.Character.Ai.PersonalNeed personalNeed)
	{
		if (!_actionEnergies.HasEnoughForAction(2))
		{
			return;
		}
		var (num, b) = SelectDemandActionTarget(context, currBlockChars, null, (Character character) => CanLearnCombatSkillFrom(character, personalNeed.CombatSkillType), null, 2);
		if (num < 0)
		{
			return;
		}
		sbyte b2 = AiHelper.DemandActionType.ToMainAttributeType(b, isSkill: true);
		if (b2 >= 0 && GetCurrMainAttribute(b2) < GlobalConfig.Instance.HarmfulActionCost)
		{
			return;
		}
		Character element_Objects = DomainManager.Character.GetElement_Objects(num);
		short favorability = DomainManager.Character.GetFavorability(num, _id);
		sbyte favorabilityType = FavorabilityType.GetFavorabilityType(favorability);
		(short skillId, byte internalIndex, byte pageTypes) tuple2 = CalcCombatSkillToLearnFromCharacter(context, element_Objects, personalNeed.CombatSkillType);
		short item = tuple2.skillId;
		byte item2 = tuple2.internalIndex;
		byte item3 = tuple2.pageTypes;
		CombatSkillItem combatSkillItem = Config.CombatSkill.Instance[item];
		short bookId = combatSkillItem.BookId;
		sbyte grade = Config.CombatSkill.Instance[item].Grade;
		switch (b)
		{
		case 0:
		{
			int askToTeachSkillRespondChance = AiHelper.GeneralActionConstants.GetAskToTeachSkillRespondChance(this, element_Objects, favorabilityType, combatSkillItem.Grade);
			bool flag = context.Random.CheckPercentProb(askToTeachSkillRespondChance);
			bool succeed = false;
			if (flag)
			{
				short combatSkillQualification = GetCombatSkillQualification(combatSkillItem.Type);
				short combatSkillAttainment = GetCombatSkillAttainment(combatSkillItem.Type);
				sbyte personality = GetPersonality(1);
				int taughtNewSkillSuccessRate = GetTaughtNewSkillSuccessRate(combatSkillItem.Grade, combatSkillQualification, combatSkillAttainment, personality);
				succeed = context.Random.CheckPercentProb(taughtNewSkillSuccessRate);
			}
			mod.PerformedActions.Add((element_Objects, new RequestCombatSkillDemandAction
			{
				BookTemplateId = bookId,
				InternalIndex = item2,
				GeneratedPageTypes = item3,
				AgreeToRequest = flag,
				Succeed = succeed
			}));
			break;
		}
		case 1:
			mod.PerformedActions.Add((element_Objects, new StealCombatSkillDemandAction
			{
				BookTemplateId = bookId,
				InternalIndex = item2,
				GeneratedPageTypes = item3,
				Phase = GetStealCombatSkillActionPhase(context.Random, element_Objects, personalNeed.CombatSkillType, grade)
			}));
			break;
		case 2:
		{
			int gradeAlertFactor = element_Objects.GetGradeAlertFactor(grade, 1);
			mod.PerformedActions.Add((element_Objects, new ScamCombatSkillDemandAction
			{
				BookTemplateId = bookId,
				InternalIndex = item2,
				GeneratedPageTypes = item3,
				Phase = GetScamActionPhase(context.Random, element_Objects, gradeAlertFactor)
			}));
			break;
		}
		default:
			throw new Exception($"Unrecognized request action type {b} for {this} to {num}");
		}
		_actionEnergies.SpendEnergyOnAction(2);
	}

	private void OfflineCalcGeneralAction_AskHelpOnCombatSkillReading(DataContext context, PeriAdvanceMonthGeneralActionModification mod, HashSet<int> currBlockChars, GameData.Domains.Item.SkillBook book)
	{
		short combatSkillTemplateId = book.GetCombatSkillTemplateId();
		var (num, b) = GetCombatSkillBookCurrReadingInfo(book);
		if (b >= 6)
		{
			throw new Exception($"{this} is trying to ask help on reading {Config.CombatSkill.Instance[combatSkillTemplateId].Name}, which has already been read.");
		}
		byte pageTypes = book.GetPageTypes();
		sbyte outlinePageType = SkillBookStateHelper.GetOutlinePageType(pageTypes);
		sbyte direction = ((b == 0) ? outlinePageType : SkillBookStateHelper.GetNormalPageType(pageTypes, b));
		byte pageInternalIndex = CombatSkillStateHelper.GetPageInternalIndex(outlinePageType, direction, b);
		Character character = SelectMaxPriorityActionTarget(context, currBlockChars, delegate(Character character2)
		{
			if (!DomainManager.CombatSkill.TryGetElement_CombatSkills(new CombatSkillKey(character2._id, combatSkillTemplateId), out var element))
			{
				return false;
			}
			ushort readingState = element.GetReadingState();
			return CombatSkillStateHelper.IsPageRead(readingState, pageInternalIndex);
		});
		if (character != null)
		{
			short favorability = DomainManager.Character.GetFavorability(character.GetId(), _id);
			sbyte favorabilityType = FavorabilityType.GetFavorabilityType(favorability);
			sbyte behaviorType = character.GetBehaviorType();
			sbyte askForHelpRespondChance = AiHelper.GeneralActionConstants.GetAskForHelpRespondChance(behaviorType, favorabilityType);
			mod.PerformedActions.Add((character, new CombatSkillReadingDemandAction
			{
				BookItemKey = book.GetItemKey(),
				InternalIndex = pageInternalIndex,
				AgreeToRequest = context.Random.CheckPercentProb(askForHelpRespondChance)
			}));
			_actionEnergies.SpendEnergyOnAction(2);
		}
	}

	private void OfflineCalcGeneralAction_AskHelpOnLifeSkillReading(DataContext context, PeriAdvanceMonthGeneralActionModification mod, HashSet<int> currBlockChars, GameData.Domains.Item.SkillBook book)
	{
		short lifeSkillTemplateId = book.GetLifeSkillTemplateId();
		var (num, needHelpPage) = GetLifeSkillBookCurrReadingInfo(book);
		if (needHelpPage >= 5)
		{
			throw new Exception($"{this} is trying to ask help on reading {LifeSkill.Instance[lifeSkillTemplateId].Name}, which has already been read.");
		}
		Character character = SelectMaxPriorityActionTarget(context, currBlockChars, delegate(Character character2)
		{
			int num2 = character2.FindLearnedLifeSkillIndex(lifeSkillTemplateId);
			return num2 >= 0 && character2._learnedLifeSkills[num2].IsPageRead(needHelpPage);
		});
		if (character != null)
		{
			short favorability = DomainManager.Character.GetFavorability(character.GetId(), _id);
			sbyte favorabilityType = FavorabilityType.GetFavorabilityType(favorability);
			sbyte behaviorType = character.GetBehaviorType();
			sbyte askForHelpRespondChance = AiHelper.GeneralActionConstants.GetAskForHelpRespondChance(behaviorType, favorabilityType);
			mod.PerformedActions.Add((character, new LifeSkillReadingDemandAction
			{
				BookItemKey = book.GetItemKey(),
				PageId = needHelpPage,
				AgreeToRequest = context.Random.CheckPercentProb(askForHelpRespondChance)
			}));
			_actionEnergies.SpendEnergyOnAction(2);
		}
	}

	private void OfflineCalcGeneralAction_AskHelpOnReading(DataContext context, PeriAdvanceMonthGeneralActionModification mod, HashSet<int> currBlockChars, GameData.Domains.Character.Ai.PersonalNeed personalNeed)
	{
		if (_actionEnergies.HasEnoughForAction(2))
		{
			GameData.Domains.Item.SkillBook element_SkillBooks = DomainManager.Item.GetElement_SkillBooks(personalNeed.ItemId);
			if (element_SkillBooks.IsCombatSkillBook())
			{
				OfflineCalcGeneralAction_AskHelpOnCombatSkillReading(context, mod, currBlockChars, element_SkillBooks);
			}
			else
			{
				OfflineCalcGeneralAction_AskHelpOnLifeSkillReading(context, mod, currBlockChars, element_SkillBooks);
			}
		}
	}

	private void OfflineCalcGeneralAction_AskHelpOnBreaking(DataContext context, PeriAdvanceMonthGeneralActionModification mod, HashSet<int> currBlockChars, GameData.Domains.Character.Ai.PersonalNeed personalNeed)
	{
		if (_actionEnergies.HasEnoughForAction(2))
		{
			GameData.Domains.CombatSkill.CombatSkill element_CombatSkills = DomainManager.CombatSkill.GetElement_CombatSkills(new CombatSkillKey(_id, personalNeed.CombatSkillTemplateId));
			GameData.Domains.CombatSkill.CombatSkill element;
			Character character = SelectMaxPriorityActionTarget(context, currBlockChars, (Character character2) => DomainManager.CombatSkill.TryGetElement_CombatSkills(new CombatSkillKey(character2._id, personalNeed.CombatSkillTemplateId), out element) && CombatSkillStateHelper.IsBrokenOut(element.GetActivationState()));
			if (character != null)
			{
				short favorability = DomainManager.Character.GetFavorability(character.GetId(), _id);
				sbyte favorabilityType = FavorabilityType.GetFavorabilityType(favorability);
				sbyte behaviorType = character.GetBehaviorType();
				sbyte askForHelpRespondChance = AiHelper.GeneralActionConstants.GetAskForHelpRespondChance(behaviorType, favorabilityType);
				mod.PerformedActions.Add((character, new BreakingDemandAction
				{
					CombatSkillTemplateId = personalNeed.CombatSkillTemplateId,
					AgreeToRequest = context.Random.CheckPercentProb(askForHelpRespondChance)
				}));
				_actionEnergies.SpendEnergyOnAction(2);
			}
		}
	}

	private unsafe void OfflineCalcGeneralAction_GainExp(DataContext context, PeriAdvanceMonthGeneralActionModification mod, HashSet<int> currBlockChars, GameData.Domains.Character.Ai.PersonalNeed personalNeed)
	{
		if (!_actionEnergies.HasEnoughForAction(3))
		{
			return;
		}
		sbyte behaviorType = GetBehaviorType();
		sbyte grade = _organizationInfo.Grade;
		short lifeSkillAttainment = GetLifeSkillAttainment(15);
		List<int>[] obj = context.AdvanceMonthRelatedData.DemandActionTargets.Occupy();
		List<int> list = obj[0];
		List<int> list2 = obj[1];
		List<int> list3 = obj[2];
		List<int> list4 = obj[3];
		bool flag = !NeedToAvoidCombat(CombatType.Play);
		bool flag2 = flag && !NeedToAvoidCombat(CombatType.Beat);
		sbyte[] array = AiHelper.GeneralActionConstants.CricketBattleGradeOffsets[behaviorType];
		foreach (int currBlockChar in currBlockChars)
		{
			if (!DomainManager.Character.TryGetElement_Objects(currBlockChar, out var element) || element.GetAgeGroup() != 2 || _consummateLevel > element._consummateLevel || _consummateLevel < element._consummateLevel - 4 || !DomainManager.Character.TryGetRelation(_id, currBlockChar, out var relation))
			{
				continue;
			}
			sbyte favorabilityType = FavorabilityType.GetFavorabilityType(relation.Favorability);
			bool flag3 = !element.NeedToAvoidCombat(CombatType.Play);
			bool flag4 = flag3 && !element.NeedToAvoidCombat(CombatType.Beat);
			if (favorabilityType <= 2 && flag && flag3)
			{
				list2.Add(currBlockChar);
			}
			if (favorabilityType >= 3 && flag2 && flag4)
			{
				list.Add(currBlockChar);
			}
			if (favorabilityType >= 2)
			{
				list3.Add(currBlockChar);
			}
			sbyte[] array2 = array;
			foreach (sbyte b in array2)
			{
				if (grade + b == element.GetInteractionGrade())
				{
					list4.Add(currBlockChar);
					break;
				}
			}
		}
		int num = 0;
		(sbyte, short)* ptr = stackalloc(sbyte, short)[6];
		sbyte[] weights = AiHelper.GainExpActionType.Weights;
		sbyte[] array3 = AiHelper.GainExpActionType.Priorities[behaviorType];
		for (int j = 0; j < array3.Length; j++)
		{
			sbyte b2 = array3[j];
			if (b2 == 3 && lifeSkillAttainment <= 150)
			{
				continue;
			}
			if (b2 < 4)
			{
				if (obj[b2].Count > 0)
				{
					Unsafe.Write(ptr + num, (b2, (short)weights[j]));
					num++;
				}
			}
			else if (b2 == 4)
			{
				if (HasBookToReadForExp())
				{
					Unsafe.Write(ptr + num, (b2, (short)weights[j]));
					num++;
				}
			}
			else
			{
				Unsafe.Write(ptr + num, (b2, (short)weights[j]));
				num++;
			}
		}
		if (num <= 0)
		{
			context.AdvanceMonthRelatedData.DemandActionTargets.Release(ref obj);
			return;
		}
		switch (CollectionUtils.GetRandomWeightedElement(context.Random, ptr, num))
		{
		case 0:
		{
			int random = list.GetRandom(context.Random);
			Character element_Objects = DomainManager.Character.GetElement_Objects(random);
			mod.PerformedActions.Add((element_Objects, new GainExpByCombatAction
			{
				CombatType = CombatType.Play
			}));
			break;
		}
		case 1:
		{
			int random2 = list2.GetRandom(context.Random);
			Character element_Objects2 = DomainManager.Character.GetElement_Objects(random2);
			mod.PerformedActions.Add((element_Objects2, new GainExpByCombatAction
			{
				CombatType = CombatType.Beat
			}));
			break;
		}
		case 2:
		{
			int random3 = list3.GetRandom(context.Random);
			Character element_Objects3 = DomainManager.Character.GetElement_Objects(random3);
			short num3 = Math.Max((short)1, GetLifeSkillAttainments().GetMaxLifeSkillValue());
			short num4 = Math.Max((short)1, element_Objects3.GetLifeSkillAttainments().GetMaxLifeSkillValue());
			bool flag5 = context.Random.CheckPercentProb(num3 * 50 / num4);
			int expGain3 = (flag5 ? DomainManager.Character.CalcExpGain(element_Objects3._organizationInfo.Grade, num4 * 50 / num3) : DomainManager.Character.CalcExpGain(_organizationInfo.Grade, num3 * 50 / num4));
			mod.PerformedActions.Add((element_Objects3, new GainExpByLifeSkillBattleAction
			{
				Succeed = flag5,
				ExpGain = expGain3
			}));
			break;
		}
		case 3:
		{
			int random4 = list4.GetRandom(context.Random);
			Character element_Objects4 = DomainManager.Character.GetElement_Objects(random4);
			short num5 = Math.Max((short)1, element_Objects4.GetLifeSkillAttainment(15));
			sbyte interactionGrade = element_Objects4.GetInteractionGrade();
			int percentProb = lifeSkillAttainment * 50 / num5 + (grade - interactionGrade) * 20;
			bool flag6 = context.Random.CheckPercentProb(percentProb);
			Wager wager = (flag6 ? DomainManager.Item.SelectCharacterValidWager(context, element_Objects4) : DomainManager.Item.SelectCharacterValidWager(context, this));
			int expGain4 = (flag6 ? DomainManager.Character.CalcExpGain(element_Objects4._organizationInfo.Grade, num5 * 50 / lifeSkillAttainment) : DomainManager.Character.CalcExpGain(_organizationInfo.Grade, lifeSkillAttainment * 50 / num5));
			mod.PerformedActions.Add((element_Objects4, new GainExpByCricketBattleAction
			{
				Wager = wager,
				Succeed = flag6,
				ExpGain = expGain4
			}));
			break;
		}
		case 4:
		{
			ItemKey itemKey = SelectBookToReadForExp(context);
			if (itemKey.IsValid())
			{
				sbyte grade2 = Config.SkillBook.Instance[itemKey.TemplateId].Grade;
				short num2 = 3;
				short currDurability = DomainManager.Item.GetElement_SkillBooks(itemKey.Id).GetCurrDurability();
				if (num2 > currDurability)
				{
					num2 = currDurability;
				}
				int expGain2 = SkillGradeData.Instance[grade2].ReadingExpGainPerPage * num2;
				mod.PerformedActions.Add((null, new GainExpByReadingAction
				{
					ExpGain = expGain2,
					DurabilityReduction = num2,
					ItemKey = itemKey
				}));
			}
			break;
		}
		case 5:
		{
			int expGain = DomainManager.Character.CalcExpGain(_organizationInfo.Grade, 100);
			mod.PerformedActions.Add((null, new GainExpByStrollAction
			{
				ExpGain = expGain
			}));
			break;
		}
		}
		_actionEnergies.SpendEnergyOnAction(3);
		context.AdvanceMonthRelatedData.DemandActionTargets.Release(ref obj);
	}

	private unsafe void OfflineCalcGeneralAction_SpendResource(DataContext context, PeriAdvanceMonthGeneralActionModification mod, HashSet<int> currBlockChars, HashSet<int> caringCharIds, GameData.Domains.Character.Ai.PersonalNeed personalNeed)
	{
		if (!_actionEnergies.HasEnoughForAction(3))
		{
			return;
		}
		int num = GetResource(personalNeed.ResourceType) - personalNeed.Amount;
		if (num <= 0)
		{
			return;
		}
		Span<int> weights = stackalloc int[8];
		weights.Fill(0);
		Span<sbyte> span = stackalloc sbyte[4];
		SpanList<sbyte> list = span;
		list.Add(0);
		if (IsInRegularSettlementRange())
		{
			list.Add(1);
			if (personalNeed.ResourceType == 6)
			{
				list.Add(2);
			}
		}
		if (CanInteractTreasury() && context.Random.CheckPercentProb(AiHelper.GeneralActionConstants.StoreInTreasuryChance[GetBehaviorType()]))
		{
			list.Add(3);
		}
		CollectionUtils.Shuffle(context.Random, list);
		switch (list.GetRandom(context.Random))
		{
		case 0:
		{
			HashSet<int> currBlockChars3 = ((caringCharIds.Count > 0) ? caringCharIds : currBlockChars);
			Character character2 = SelectMaxPriorityActionTarget(context, currBlockChars3, null, includeBabies: true);
			if (character2 != null)
			{
				mod.PerformedActions.Add((character2, new GiveResourceAction
				{
					ResourceType = personalNeed.ResourceType,
					Amount = num
				}));
				_actionEnergies.SpendEnergyOnAction(3);
			}
			break;
		}
		case 1:
		{
			bool flag = false;
			foreach (GameData.Domains.Character.Ai.PersonalNeed personalNeed2 in _personalNeeds)
			{
				if (personalNeed2.TemplateId == 8 && personalNeed2.ResourceType != personalNeed.ResourceType)
				{
					weights[personalNeed2.ResourceType] = Math.Max(0, personalNeed2.Amount - _resources.Items[personalNeed2.ResourceType]);
					if (weights[personalNeed2.ResourceType] > 0)
					{
						flag = true;
					}
				}
			}
			sbyte resourceType = personalNeed.ResourceType;
			sbyte b = (sbyte)(flag ? ((sbyte)RandomUtils.GetRandomIndex(weights, context.Random)) : 6);
			if (resourceType != b)
			{
				int num2 = weights[b];
				int num3 = num2 * GlobalConfig.ResourcesWorth[b];
				int num4 = num3 * 2 / GlobalConfig.ResourcesWorth[resourceType];
				if (num4 > num || !flag)
				{
					num4 = num;
					num2 = num4 * GlobalConfig.ResourcesWorth[resourceType] / (2 * GlobalConfig.ResourcesWorth[b]);
				}
				mod.PerformedActions.Add((null, new ExchangeResourceAction
				{
					SpentResourceType = personalNeed.ResourceType,
					SpentResourceAmount = num4,
					GainResourceType = b,
					GainResourceAmount = num2
				}));
				_actionEnergies.SpendEnergyOnAction(3);
			}
			break;
		}
		case 2:
		{
			HashSet<int> currBlockChars2 = ((caringCharIds.Count > 0) ? caringCharIds : currBlockChars);
			Character character = SelectMaxPriorityActionTarget(context, currBlockChars2, delegate(Character character3)
			{
				if (character3._id == DomainManager.Taiwu.GetTaiwuCharId())
				{
					return false;
				}
				foreach (GameData.Domains.Character.Ai.PersonalNeed personalNeed3 in character3._personalNeeds)
				{
					if (personalNeed3.TemplateId == 10 && personalNeed3.ItemType != 11)
					{
						return true;
					}
				}
				return false;
			}, includeBabies: true);
			if (character == null)
			{
				break;
			}
			{
				foreach (GameData.Domains.Character.Ai.PersonalNeed personalNeed4 in character._personalNeeds)
				{
					if (personalNeed4.TemplateId != 10 || personalNeed4.ItemType == 11 || personalNeed4.ItemType == 3 || (!OfflineCalcGeneralAction_MakeArtisanOrder(context, mod, personalNeed4, character._id) && !OfflineCalcGeneralAction_PurchaseItem(context, mod, personalNeed4, character)))
					{
						continue;
					}
					break;
				}
				break;
			}
		}
		case 3:
			mod.PerformedActions.Add((null, new StoreTreasuryResourceAction
			{
				ResourceType = personalNeed.ResourceType,
				Amount = num
			}));
			_actionEnergies.SpendEnergyOnAction(3);
			break;
		}
	}

	private void OfflineCalcGeneralAction_SpendItem(DataContext context, PeriAdvanceMonthGeneralActionModification mod, HashSet<int> currBlockChars, HashSet<int> caringCharIds, GameData.Domains.Character.Ai.PersonalNeed personalNeed)
	{
		if (!_actionEnergies.HasEnoughForAction(3))
		{
			return;
		}
		HashSet<int> currBlockChars2 = ((caringCharIds.Count > 0) ? caringCharIds : currBlockChars);
		bool flag = IsInRegularSettlementRange();
		Character character = ((flag && context.Random.NextBool()) ? null : SelectMaxPriorityActionTarget(context, currBlockChars2, null, includeBabies: true));
		sbyte targetGrade = character?.GetInteractionGrade() ?? 0;
		ItemBase itemBase = SelectSpareableItem(context, targetGrade, flag);
		if (itemBase == null)
		{
			return;
		}
		Span<sbyte> span = stackalloc sbyte[3];
		SpanList<sbyte> spanList = span;
		if (character != null && itemBase.GetCurrDurability() >= itemBase.GetMaxDurability())
		{
			spanList.Add(0);
		}
		if (IsInRegularSettlementRange())
		{
			spanList.Add(1);
		}
		if (CanInteractTreasury() && context.Random.CheckPercentProb(AiHelper.GeneralActionConstants.StoreInTreasuryChance[GetBehaviorType()]))
		{
			spanList.Add(2);
		}
		if (spanList.Count != 0)
		{
			ItemKey itemKey = itemBase.GetItemKey();
			switch (spanList.GetRandom(context.Random))
			{
			case 0:
				mod.PerformedActions.Add((character, new GiveItemAction
				{
					TargetItem = itemKey,
					Amount = 1
				}));
				break;
			case 1:
				mod.PerformedActions.Add((null, new SellItemAction
				{
					TargetItem = itemKey,
					Amount = Math.Max(1, _inventory.Items[itemKey] / 2)
				}));
				break;
			case 2:
				mod.PerformedActions.Add((null, new StoreTreasuryItemAction
				{
					TargetItem = itemBase.GetItemKey(),
					Amount = 1
				}));
				break;
			default:
				_actionEnergies.SpendEnergyOnAction(3);
				break;
			}
		}
	}

	private bool CanInteractTreasury()
	{
		if (_organizationInfo.SettlementId < 0)
		{
			return false;
		}
		Settlement settlement = DomainManager.Organization.GetSettlement(_organizationInfo.SettlementId);
		if (!settlement.HasTreasury() && _organizationInfo.OrgTemplateId != 16)
		{
			return false;
		}
		Location location = settlement.GetLocation();
		if (location.AreaId != _location.AreaId)
		{
			return false;
		}
		MapBlockData block = DomainManager.Map.GetBlock(_location);
		if (block.GetRootBlock().GetLocation() != location)
		{
			return false;
		}
		return true;
	}

	private bool OfflineCalcGeneralAction_TakeTreasuryItem(IRandomSource random, PeriAdvanceMonthGeneralActionModification mod, GameData.Domains.Character.Ai.PersonalNeed personalNeed)
	{
		if (!CanInteractTreasury())
		{
			return false;
		}
		if (!random.CheckPercentProb(AiHelper.GeneralActionConstants.TakeFromTreasuryChance[GetBehaviorType()]))
		{
			return false;
		}
		if (_organizationInfo.OrgTemplateId == 16 && personalNeed.ItemType == 3 && _equipment[4].IsValid() && personalNeed.TemplateId != GetIdealClothingTemplateId())
		{
			return false;
		}
		Settlement settlement = DomainManager.Organization.GetSettlement(_organizationInfo.SettlementId);
		SettlementTreasury treasury = DomainManager.Organization.GetTreasury(_organizationInfo);
		ItemKey itemInSameGroup = treasury.Inventory.GetItemInSameGroup(personalNeed.ItemType, personalNeed.ItemTemplateId, -2);
		if (!itemInSameGroup.IsValid())
		{
			return false;
		}
		int num = DomainManager.Organization.CalcItemContribution(settlement, itemInSameGroup, 1);
		if (_organizationInfo.OrgTemplateId != 16 && treasury.GetMemberContribution(this) < num)
		{
			return false;
		}
		mod.PerformedActions.Add((null, new TakeTreasuryItemAction
		{
			TargetItem = itemInSameGroup,
			Amount = 1
		}));
		_actionEnergies.SpendEnergyOnAction(1);
		return true;
	}

	[Obsolete]
	private bool OfflineCalcGeneralAction_TakeTreasuryItem(PeriAdvanceMonthGeneralActionModification mod, Predicate<ItemKey> predicate)
	{
		if (_organizationInfo.SettlementId < 0)
		{
			return false;
		}
		SettlementTreasury treasury = DomainManager.Organization.GetTreasury(_organizationInfo);
		ItemKey targetItem = ItemKey.Invalid;
		foreach (var (itemKey2, num2) in treasury.Inventory.Items)
		{
			if (!predicate(itemKey2))
			{
				continue;
			}
			targetItem = itemKey2;
			break;
		}
		if (!targetItem.IsValid())
		{
			return false;
		}
		mod.PerformedActions.Add((null, new TakeTreasuryItemAction
		{
			TargetItem = targetItem,
			Amount = 1
		}));
		_actionEnergies.SpendEnergyOnAction(1);
		return true;
	}

	private bool OfflineCalcGeneralAction_TakeTreasuryResource(IRandomSource random, PeriAdvanceMonthGeneralActionModification mod, sbyte resourceType, int amount)
	{
		if (!CanInteractTreasury())
		{
			return false;
		}
		if (!random.CheckPercentProb(AiHelper.GeneralActionConstants.TakeFromTreasuryChance[GetBehaviorType()]))
		{
			return false;
		}
		SettlementTreasury treasury = DomainManager.Organization.GetTreasury(_organizationInfo);
		if (treasury.Resources[resourceType] < amount)
		{
			return false;
		}
		int num = DomainManager.Organization.CalcResourceContribution(_organizationInfo.OrgTemplateId, resourceType, amount);
		if (treasury.GetMemberContribution(this) < num)
		{
			return false;
		}
		mod.PerformedActions.Add((null, new TakeTreasuryResourceAction
		{
			ResourceType = resourceType,
			Amount = amount
		}));
		_actionEnergies.SpendEnergyOnAction(1);
		return true;
	}

	private unsafe void OfflineCalcGeneralAction_RandomActions(DataContext context, PeriAdvanceMonthGeneralActionModification mod, HashSet<int> currBlockChars, HashSet<int> caringCharIds)
	{
		if (_organizationInfo.SettlementId >= 0)
		{
			Settlement settlement = DomainManager.Organization.GetSettlement(_organizationInfo.SettlementId);
			MapAreaData element_Areas = DomainManager.Map.GetElement_Areas(settlement.GetLocation().AreaId);
		}
		sbyte* ptr = stackalloc sbyte[3] { 0, 1, 2 };
		CollectionUtils.Shuffle(context.Random, ptr, 3);
		for (int i = 0; i < 3; i++)
		{
			if (!_actionEnergies.HasEnoughForAction(4))
			{
				break;
			}
			switch (ptr[i])
			{
			case 0:
				OfflineCalcGeneralAction_TeachSkill(context, mod, currBlockChars, caringCharIds);
				break;
			case 1:
				OfflineCalcGeneralAction_SocialStatus(context, mod, currBlockChars, caringCharIds);
				break;
			case 2:
				OfflineCalcGeneralAction_LifeSkill(context, mod, currBlockChars, caringCharIds);
				break;
			}
		}
	}

	public bool CanTeachCombatSkill(Character targetChar)
	{
		Dictionary<short, GameData.Domains.CombatSkill.CombatSkill> charCombatSkills = DomainManager.CombatSkill.GetCharCombatSkills(_id);
		foreach (var (num2, combatSkill2) in charCombatSkills)
		{
			if (combatSkill2.GetReadingState() != 0 && Config.CombatSkill.Instance[num2].BookId >= 0 && !targetChar._learnedCombatSkills.Contains(num2))
			{
				return true;
			}
		}
		return false;
	}

	public bool CanTeachLifeSkill(Character targetChar)
	{
		foreach (LifeSkillItem learnedLifeSkill in _learnedLifeSkills)
		{
			if (learnedLifeSkill.ReadingState != 0 && LifeSkill.Instance[learnedLifeSkill.SkillTemplateId].SkillBookId >= 0 && targetChar.FindLearnedLifeSkillIndex(learnedLifeSkill.SkillTemplateId) < 0)
			{
				return true;
			}
		}
		return false;
	}

	public void GetTeachableCombatSkillBookIds(Character targetChar, List<(short, short)> weightTable)
	{
		Dictionary<short, GameData.Domains.CombatSkill.CombatSkill> charCombatSkills = DomainManager.CombatSkill.GetCharCombatSkills(_id);
		foreach (short learnedCombatSkill in _learnedCombatSkills)
		{
			CombatSkillItem combatSkillItem = Config.CombatSkill.Instance[learnedCombatSkill];
			if (combatSkillItem.BookId >= 0 && charCombatSkills[learnedCombatSkill].GetReadingState() != 0 && !targetChar._learnedCombatSkills.Contains(learnedCombatSkill))
			{
				int num = 3 << 8 - combatSkillItem.Grade;
				if (combatSkillItem.IsNonPublic)
				{
					num /= 3;
				}
				weightTable.Add((combatSkillItem.BookId, (short)num));
			}
		}
	}

	public void GetTeachableLifeSkillBookIds(Character targetChar, List<(short, short)> weightTable)
	{
		foreach (LifeSkillItem learnedLifeSkill in _learnedLifeSkills)
		{
			Config.LifeSkillItem lifeSkillItem = LifeSkill.Instance[learnedLifeSkill.SkillTemplateId];
			byte readingState = learnedLifeSkill.ReadingState;
			if (lifeSkillItem.SkillBookId >= 0 && readingState != 0 && targetChar.FindLearnedLifeSkillIndex(learnedLifeSkill.SkillTemplateId) < 0)
			{
				int num = 3 << 8 - lifeSkillItem.Grade;
				weightTable.Add((lifeSkillItem.SkillBookId, (short)num));
			}
		}
	}

	private unsafe void OfflineCalcGeneralAction_TeachSkill(DataContext context, PeriAdvanceMonthGeneralActionModification mod, HashSet<int> currBlockChars, HashSet<int> caringCharIds)
	{
		if (!_actionEnergies.HasEnoughForAction(4))
		{
			return;
		}
		Character character = null;
		if (caringCharIds.Count > 0)
		{
			character = SelectMaxPriorityActionTarget(context, caringCharIds, IsValidTargetForTeachSkill);
		}
		if (character == null)
		{
			character = SelectMaxPriorityActionTarget(context, currBlockChars, IsValidTargetForTeachSkill);
		}
		if (character == null)
		{
			return;
		}
		List<(short, short)> obj = context.AdvanceMonthRelatedData.WeightTable.Occupy();
		GetTeachableCombatSkillBookIds(character, obj);
		GetTeachableLifeSkillBookIds(character, obj);
		short randomResult = RandomUtils.GetRandomResult(obj, context.Random);
		context.AdvanceMonthRelatedData.WeightTable.Release(ref obj);
		SkillBookItem skillBookItem = Config.SkillBook.Instance[randomResult];
		short favorability = DomainManager.Character.GetFavorability(character.GetId(), _id);
		sbyte favorabilityType = FavorabilityType.GetFavorabilityType(favorability);
		int askToTeachSkillRespondChance = AiHelper.GeneralActionConstants.GetAskToTeachSkillRespondChance(character, this, favorabilityType, skillBookItem.Grade);
		if (!context.Random.CheckPercentProb(askToTeachSkillRespondChance))
		{
			return;
		}
		if (skillBookItem.CombatSkillType >= 0)
		{
			Dictionary<short, GameData.Domains.CombatSkill.CombatSkill> charCombatSkills = DomainManager.CombatSkill.GetCharCombatSkills(_id);
			ushort readingState = charCombatSkills[skillBookItem.CombatSkillTemplateId].GetReadingState();
			byte b = 0;
			while (b < 15 && !CombatSkillStateHelper.IsPageRead(readingState, b))
			{
				b++;
			}
			byte generatedPageTypes = CombatSkillStateHelper.GeneratePageTypesFromReadingState(context.Random, readingState);
			CombatSkillShorts combatSkillAttainments = character.GetCombatSkillAttainments();
			CombatSkillShorts combatSkillQualifications = character.GetCombatSkillQualifications();
			Personalities personalities = character.GetPersonalities();
			int taughtNewSkillSuccessRate = GetTaughtNewSkillSuccessRate(skillBookItem.Grade, combatSkillQualifications.Items[skillBookItem.CombatSkillType], combatSkillAttainments.Items[skillBookItem.CombatSkillType], personalities.Items[1]);
			mod.PerformedActions.Add((character, new TeachCombatSkillAction
			{
				SkillTemplateId = skillBookItem.CombatSkillTemplateId,
				InternalIndex = b,
				GeneratedPageTypes = generatedPageTypes,
				Succeed = context.Random.CheckPercentProb(taughtNewSkillSuccessRate)
			}));
		}
		else
		{
			int index = FindLearnedLifeSkillIndex(skillBookItem.LifeSkillTemplateId);
			LifeSkillItem lifeSkillItem = _learnedLifeSkills[index];
			byte b2 = 0;
			while (b2 < 5 && !lifeSkillItem.IsPageRead(b2))
			{
				b2++;
			}
			LifeSkillShorts lifeSkillAttainments = character.GetLifeSkillAttainments();
			LifeSkillShorts lifeSkillQualifications = character.GetLifeSkillQualifications();
			Personalities personalities2 = character.GetPersonalities();
			int taughtNewSkillSuccessRate2 = GetTaughtNewSkillSuccessRate(skillBookItem.Grade, lifeSkillQualifications.Items[skillBookItem.LifeSkillType], lifeSkillAttainments.Items[skillBookItem.LifeSkillType], personalities2.Items[1]);
			mod.PerformedActions.Add((character, new TeachLifeSkillAction
			{
				SkillTemplateId = skillBookItem.LifeSkillTemplateId,
				PageId = b2,
				Succeed = context.Random.CheckPercentProb(taughtNewSkillSuccessRate2)
			}));
		}
		_actionEnergies.SpendEnergyOnAction(4);
		bool IsValidTargetForTeachSkill(Character targetChar)
		{
			return CanTeachCombatSkill(targetChar) || CanTeachLifeSkill(targetChar);
		}
	}

	private unsafe void OfflineCalcGeneralAction_SocialStatus(DataContext context, PeriAdvanceMonthGeneralActionModification mod, HashSet<int> currBlockChars, HashSet<int> caringCharIds)
	{
		OrganizationMemberItem orgMemberConfig = OrganizationDomain.GetOrgMemberConfig(_organizationInfo);
		if (orgMemberConfig.IdentityInteractConfig == null || orgMemberConfig.IdentityInteractConfig.Count == 0 || (orgMemberConfig.IdentityActiveAge >= 0 && _currAge < orgMemberConfig.IdentityActiveAge))
		{
			return;
		}
		sbyte* ptr = stackalloc sbyte[(int)(uint)orgMemberConfig.IdentityInteractConfig.Count];
		int num = 0;
		foreach (sbyte item in orgMemberConfig.IdentityInteractConfig)
		{
			ptr[num] = item;
			num++;
		}
		CollectionUtils.Shuffle(context.Random, ptr, num);
		for (int i = 0; i < num; i++)
		{
			if (!_actionEnergies.HasEnoughForAction(4))
			{
				break;
			}
			switch (ptr[i])
			{
			case 4:
				OfflineCalcGeneralAction_SocialStatus_SellItem(context, mod, currBlockChars, caringCharIds);
				break;
			case 5:
				OfflineCalcGeneralAction_SocialStatus_Heal(context, mod, currBlockChars, caringCharIds);
				break;
			case 6:
				OfflineCalcGeneralAction_SocialStatus_Repair(context, mod, currBlockChars, caringCharIds);
				break;
			case 7:
				OfflineCalcGeneralAction_SocialStatus_Barb(context, mod, currBlockChars, caringCharIds);
				break;
			case 8:
				OfflineCalcGeneralAction_SocialStatus_Beg(context, mod, currBlockChars, caringCharIds);
				break;
			}
		}
	}

	private void OfflineCalcGeneralAction_SocialStatus_ExtendFavor(DataContext context, PeriAdvanceMonthGeneralActionModification mod)
	{
		Character taiwu = DomainManager.Taiwu.GetTaiwu();
		if (_location.Equals(taiwu.GetLocation()))
		{
			Settlement settlement = DomainManager.Organization.GetSettlement(_organizationInfo.SettlementId);
			Location location = settlement.GetLocation();
			if (DomainManager.Extra.GetAreaSpiritualDebt(location.AreaId) >= 200)
			{
				mod.PerformedActions.Add((taiwu, new SocialStatusExtendFavorAction()));
			}
		}
	}

	private void OfflineCalcGeneralAction_SocialStatus_CultivateWill(DataContext context, PeriAdvanceMonthGeneralActionModification mod)
	{
		Character taiwu = DomainManager.Taiwu.GetTaiwu();
		if (_location.Equals(taiwu.GetLocation()))
		{
			int resource = taiwu.GetResource(6);
			if (resource >= 10000)
			{
				mod.PerformedActions.Add((taiwu, new SocialStatusCultivateWillAction()));
			}
		}
	}

	private void OfflineCalcGeneralAction_SocialStatus_MerchantPraise(DataContext context, PeriAdvanceMonthGeneralActionModification mod)
	{
		Character taiwu = DomainManager.Taiwu.GetTaiwu();
		if (_location.Equals(taiwu.GetLocation()))
		{
			Settlement settlement = DomainManager.Organization.GetSettlement(_organizationInfo.SettlementId);
			Location location = settlement.GetLocation();
			if (DomainManager.Extra.GetAreaSpiritualDebt(location.AreaId) >= 200)
			{
				mod.PerformedActions.Add((taiwu, new SocialStatusMerchantPraiseAction()));
			}
		}
	}

	private void OfflineCalcGeneralAction_SocialStatus_TeaWine(DataContext context, PeriAdvanceMonthGeneralActionModification mod, HashSet<int> currBlockChars, HashSet<int> caringCharIds)
	{
		sbyte currMaxEatingSlotsCount = GetCurrMaxEatingSlotsCount();
		sbyte availableEatingSlot = _eatingItems.GetAvailableEatingSlot(currMaxEatingSlotsCount);
		if (availableEatingSlot < 0 || GetAgeGroup() != 2)
		{
			return;
		}
		sbyte behaviorType = GetBehaviorType();
		short aiActionRateAdjust = DomainManager.Extra.GetAiActionRateAdjust(_id, 6, -1);
		sbyte b = AiHelper.UpdateStatusConstants.EatForbiddenFoodChance[behaviorType];
		bool flag = !IsForbiddenToDrinkingWines() || context.Random.CheckPercentProb(b + aiActionRateAdjust);
		List<ItemKey> obj = context.AdvanceMonthRelatedData.ItemKeys.Occupy();
		foreach (var (itemKey2, num2) in _inventory.Items)
		{
			if (itemKey2.ItemType == 9 && num2 >= 2)
			{
				short itemSubType = ItemTemplateHelper.GetItemSubType(itemKey2.ItemType, itemKey2.TemplateId);
				if ((itemSubType != 901 || flag) && !TryDetectAttachedPoisons(itemKey2))
				{
					obj.Add(itemKey2);
				}
			}
		}
		ItemKey randomOrDefault = obj.GetRandomOrDefault(context.Random, ItemKey.Invalid);
		context.AdvanceMonthRelatedData.ItemKeys.Release(ref obj);
		if (!randomOrDefault.IsValid())
		{
			return;
		}
		bool flag2 = ItemTemplateHelper.GetItemSubType(randomOrDefault.ItemType, randomOrDefault.TemplateId) == 901;
		Character character = SelectMaxPriorityActionTarget(context, currBlockChars, (Character character2) => character2.GetEatingItems().GetAvailableEatingSlotsCount(character2.GetCurrMaxEatingSlotsCount()) > 0);
		if (character == null)
		{
			return;
		}
		sbyte b2 = (flag2 ? AiHelper.GeneralActionConstants.SocialStatusDrinkWineChance[behaviorType] : AiHelper.GeneralActionConstants.SocialStatusDrinkTeaChance[behaviorType]);
		if (context.Random.CheckPercentProb(caringCharIds.Contains(character.GetId()) ? (b2 + 30) : b2))
		{
			bool succeed = true;
			if (flag2 && character.IsForbiddenToDrinkingWines())
			{
				sbyte behaviorType2 = character.GetBehaviorType();
				short aiActionRateAdjust2 = DomainManager.Extra.GetAiActionRateAdjust(character.GetId(), 6, -1);
				sbyte b3 = AiHelper.UpdateStatusConstants.EatForbiddenFoodChance[behaviorType2];
				succeed = context.Random.CheckPercentProb(b3 + aiActionRateAdjust2);
			}
			mod.PerformedActions.Add((character, new SocialStatusTeaWineAction
			{
				SelfTeaWineItem = randomOrDefault,
				TargetTeaWineItem = randomOrDefault,
				Succeed = succeed
			}));
			_actionEnergies.SpendEnergyOnAction(4);
		}
	}

	private void OfflineCalcGeneralAction_SocialStatus_SellItem(DataContext context, PeriAdvanceMonthGeneralActionModification mod, HashSet<int> currBlockChars, HashSet<int> caringCharIds)
	{
		if (!DomainManager.Merchant.TryGetMerchantData(_id, out var value))
		{
			return;
		}
		List<ItemKey> obj = context.AdvanceMonthRelatedData.ItemKeys.Occupy();
		if (value.GoodsList0 != null)
		{
			obj.AddRange(value.GoodsList0.Items.Keys);
		}
		if (value.GoodsList1 != null)
		{
			obj.AddRange(value.GoodsList1.Items.Keys);
		}
		if (value.GoodsList2 != null)
		{
			obj.AddRange(value.GoodsList2.Items.Keys);
		}
		if (value.GoodsList3 != null)
		{
			obj.AddRange(value.GoodsList3.Items.Keys);
		}
		if (value.GoodsList4 != null)
		{
			obj.AddRange(value.GoodsList4.Items.Keys);
		}
		if (value.GoodsList5 != null)
		{
			obj.AddRange(value.GoodsList5.Items.Keys);
		}
		if (value.GoodsList6 != null)
		{
			obj.AddRange(value.GoodsList6.Items.Keys);
		}
		ItemKey randomOrDefault = obj.GetRandomOrDefault(context.Random, ItemKey.Invalid);
		context.AdvanceMonthRelatedData.ItemKeys.Release(ref obj);
		if (!randomOrDefault.IsValid())
		{
			return;
		}
		int price = ItemTemplateHelper.GetBaseValue(randomOrDefault.ItemType, randomOrDefault.TemplateId) * 150 / 100;
		Character character = SelectMaxPriorityActionTarget(context, currBlockChars, (Character character2) => character2.GetResource(6) >= price);
		if (character != null)
		{
			sbyte behaviorType = GetBehaviorType();
			sbyte b = AiHelper.GeneralActionConstants.SocialStatusSellItemChance[behaviorType];
			if (context.Random.CheckPercentProb(caringCharIds.Contains(character.GetId()) ? (b + 30) : b))
			{
				int percentProb = 40 + GetLifeSkillAttainment(15) / 10;
				mod.PerformedActions.Add((character, new SocialStatusSellItemAction
				{
					ItemKey = randomOrDefault,
					Amount = 1,
					Price = price,
					Succeed = context.Random.CheckPercentProb(percentProb)
				}));
				_actionEnergies.SpendEnergyOnAction(4);
			}
		}
	}

	private unsafe void OfflineCalcGeneralAction_SocialStatus_Heal(DataContext context, PeriAdvanceMonthGeneralActionModification mod, HashSet<int> currBlockChars, HashSet<int> caringCharIds)
	{
		sbyte behaviorType = GetBehaviorType();
		sbyte b = AiHelper.GeneralActionConstants.SocialStatusHealChance[behaviorType];
		if (_resources.Items[5] < GlobalConfig.Instance.HealInjuryBaseHerb)
		{
			return;
		}
		CombatResources usableCombatResources = DomainManager.Character.GetUsableCombatResources(_id);
		Span<EHealActionType> span = stackalloc EHealActionType[4];
		SpanList<EHealActionType> list = span;
		foreach (EHealActionType allHealAction in AllHealActions)
		{
			if (CalcHealAttainment(allHealAction) >= 200 && usableCombatResources.Get(allHealAction) > 0)
			{
				list.Add(allHealAction);
			}
		}
		if (list.Count == 0)
		{
			return;
		}
		CollectionUtils.Shuffle(context.Random, list);
		SpanList<EHealActionType>.Enumerator enumerator2 = list.GetEnumerator();
		while (enumerator2.MoveNext())
		{
			EHealActionType type = enumerator2.Current;
			Character character = SelectMaxPriorityActionTarget(context, currBlockChars, (Character character2) => character2.NeedHealAction(type) && character2.CalcHealCostHerb(type) <= _resources.Items[5]);
			if (character == null || !context.Random.CheckPercentProb(caringCharIds.Contains(character.GetId()) ? (b + 30) : b))
			{
				continue;
			}
			mod.PerformedActions.Add((character, new SocialStatusHealAction
			{
				Type = type,
				HerbAmount = character.CalcHealCostHerb(type)
			}));
			_actionEnergies.SpendEnergyOnAction(4);
			break;
		}
	}

	private void OfflineCalcGeneralAction_SocialStatus_Repair(DataContext context, PeriAdvanceMonthGeneralActionModification mod, HashSet<int> currBlockChars, HashSet<int> caringCharIds)
	{
		Character character = SelectMaxPriorityActionTarget(context, currBlockChars, (Character character2) => character2._inventory.Items.Keys.Any(NeedRepair) || character2._equipment.Any(NeedRepair));
		if (character == null)
		{
			return;
		}
		sbyte behaviorType = GetBehaviorType();
		sbyte b = AiHelper.GeneralActionConstants.SocialStatusHealChance[behaviorType];
		if (!context.Random.CheckPercentProb(caringCharIds.Contains(character.GetId()) ? (b + 30) : b))
		{
			return;
		}
		List<ItemKey> obj = context.AdvanceMonthRelatedData.ItemKeys.Occupy();
		foreach (ItemKey key in character._inventory.Items.Keys)
		{
			if (NeedRepair(key))
			{
				obj.Add(key);
			}
		}
		ItemKey[] equipment = character._equipment;
		foreach (ItemKey itemKey in equipment)
		{
			if (NeedRepair(itemKey))
			{
				obj.Add(itemKey);
			}
		}
		ItemKey random = obj.GetRandom(context.Random);
		context.AdvanceMonthRelatedData.ItemKeys.Release(ref obj);
		ItemBase baseItem = DomainManager.Item.GetBaseItem(random);
		short currDurability = baseItem.GetCurrDurability();
		sbyte b2 = 6;
		EquipmentBase baseEquipment = DomainManager.Item.GetBaseEquipment(random);
		int repairNeedResourceCount = ItemTemplateHelper.GetRepairNeedResourceCount(baseEquipment.GetMaterialResources(), random, currDurability);
		if (!character.CheckResources(context, b2, repairNeedResourceCount))
		{
			return;
		}
		sbyte craftRequiredLifeSkillType = ItemTemplateHelper.GetCraftRequiredLifeSkillType(random.ItemType, random.TemplateId);
		short repairRequiredAttainment = ItemTemplateHelper.GetRepairRequiredAttainment(random.ItemType, random.TemplateId, currDurability);
		short lifeSkillAttainment = GetLifeSkillAttainment(craftRequiredLifeSkillType);
		obj = context.AdvanceMonthRelatedData.ItemKeys.Occupy();
		foreach (ItemKey key2 in _inventory.Items.Keys)
		{
			if (key2.ItemType != 6)
			{
				continue;
			}
			ItemBase baseItem2 = DomainManager.Item.GetBaseItem(key2);
			if (baseItem2.GetCurrDurability() > 0)
			{
				CraftToolItem craftToolItem = Config.CraftTool.Instance[key2.TemplateId];
				if (lifeSkillAttainment + craftToolItem.AttainmentBonus >= repairRequiredAttainment && craftToolItem.RequiredLifeSkillTypes.Contains(craftRequiredLifeSkillType))
				{
					obj.Add(key2);
				}
			}
		}
		ItemKey randomOrDefault = obj.GetRandomOrDefault(context.Random, ItemKey.Invalid);
		context.AdvanceMonthRelatedData.ItemKeys.Release(ref obj);
		if (random.IsValid())
		{
			mod.PerformedActions.Add((character, new SocialStatusRepairAction
			{
				RepairedItem = random,
				ToolUsed = randomOrDefault,
				ResourceType = b2,
				Amount = repairNeedResourceCount
			}));
			_actionEnergies.SpendEnergyOnAction(4);
		}
	}

	private static bool NeedRepair(ItemKey itemKey)
	{
		if (!itemKey.IsValid())
		{
			return false;
		}
		ItemBase baseItem = DomainManager.Item.GetBaseItem(itemKey);
		short currDurability = baseItem.GetCurrDurability();
		return baseItem.GetRepairable() && currDurability > 0 && currDurability < baseItem.GetMaxDurability() / 2;
	}

	private void OfflineCalcGeneralAction_SocialStatus_Barb(DataContext context, PeriAdvanceMonthGeneralActionModification mod, HashSet<int> currBlockChars, HashSet<int> caringCharIds)
	{
		Character character = SelectMaxPriorityActionTarget(context, currBlockChars, delegate(Character character2)
		{
			int id = character2.GetId();
			if (character2.GetAgeGroup() != 2)
			{
				return false;
			}
			if (!DomainManager.Character.TryGetRelation(_id, id, out var relation) || relation.GetFavorabilityType() < 3)
			{
				return false;
			}
			if (!character2._avatar.GetGrowableElementShowingState(0))
			{
				return false;
			}
			if (character2.GetGroupFeature(496) >= 0)
			{
				return false;
			}
			sbyte b = 10;
			if (caringCharIds.Contains(id))
			{
				b += 10;
			}
			return context.Random.CheckPercentProb(b);
		});
		if (character == null)
		{
			return;
		}
		short lifeSkillAttainment = GetLifeSkillAttainment(5);
		int percentProb = 80 + lifeSkillAttainment / 10;
		int percentProb2 = 20 + lifeSkillAttainment / 5;
		bool flag = context.Random.CheckPercentProb(percentProb);
		bool flag2 = context.Random.CheckPercentProb(percentProb2);
		AvatarData avatar = character.GetAvatar();
		short frontHairId = avatar.FrontHairId;
		short backHairId = avatar.BackHairId;
		short beard1Id = avatar.Beard1Id;
		short beard2Id = avatar.Beard2Id;
		if (flag)
		{
			AvatarGroup avatarGroup = AvatarManager.Instance.GetAvatarGroup(avatar.AvatarId);
			if (avatar.GetGrowableElementShowingState(1) && avatar.GetGrowableElementShowingAbility(1) && avatar.GetGrowableElementShowingState(2) && avatar.GetGrowableElementShowingAbility(2) && avatarGroup.Beard1Res.CheckIndex(0) && avatarGroup.Beard2Res.CheckIndex(0))
			{
				AvatarAsset avatarAsset = avatarGroup.Beard1Res.First((AvatarAsset beardRes) => beardRes.Id == beard1Id);
				AvatarAsset avatarAsset2 = avatarGroup.Beard2Res.First((AvatarAsset beardRes) => beardRes.Id == beard2Id);
				float beardCharmRate = Math.Min(avatarAsset.Config.CharmExtraArg, avatarAsset2.Config.CharmExtraArg);
				if (flag2)
				{
					(beard1Id, beard2Id) = avatarGroup.GetRandomBeardsWithCondition(context.Random, (AvatarAsset beardRes) => beardRes.Config.CharmExtraArg > beardCharmRate);
				}
				else
				{
					(beard1Id, beard2Id) = avatarGroup.GetRandomBeardsWithCondition(context.Random, (AvatarAsset beardRes) => beardRes.Config.CharmExtraArg < beardCharmRate);
				}
				if (avatarGroup.IsBeardless(beard1Id, beard2Id))
				{
					short beard1Id2 = avatar.Beard1Id;
					short beard2Id2 = avatar.Beard2Id;
					beard2Id = beard2Id2;
					beard1Id = beard1Id2;
				}
			}
			if (avatar.GetGrowableElementShowingState(0) && avatar.GetGrowableElementShowingAbility(0) && avatarGroup.Hair1Res.CheckIndex(0) && avatarGroup.Hair2Res.CheckIndex(0))
			{
				HairRes hairRes = avatarGroup.Hair1Res.First((HairRes hairRes3) => hairRes3.Id == frontHairId);
				HairRes hairRes2 = avatarGroup.Hair2Res.First((HairRes hairRes3) => hairRes3.Id == backHairId);
				float hairCharmRate = Math.Min(hairRes.Hair.Config.CharmExtraArg, hairRes2.Hair.Config.CharmExtraArg);
				if (flag2)
				{
					(frontHairId, backHairId) = avatarGroup.GetRandomHairsWithCondition(context.Random, (HairRes hairRes3) => hairRes3.Hair.Config.CharmExtraArg > hairCharmRate);
				}
				else
				{
					(frontHairId, backHairId) = avatarGroup.GetRandomHairsWithCondition(context.Random, (HairRes hairRes3) => hairRes3.Hair.Config.CharmExtraArg < hairCharmRate);
				}
				if (avatarGroup.IsHairless(frontHairId, backHairId))
				{
					short beard2Id2 = avatar.FrontHairId;
					short beard1Id2 = avatar.BackHairId;
					backHairId = beard1Id2;
					frontHairId = beard2Id2;
				}
			}
			if (avatar.FrontHairId == frontHairId && avatar.BackHairId == backHairId && avatar.Beard1Id == beard1Id && avatar.Beard2Id == beard2Id)
			{
				return;
			}
		}
		mod.PerformedActions.Add((character, new SocialStatusBarbAction
		{
			AttractionIncreased = flag2,
			Succeed = flag,
			TargetBeard1Id = beard1Id,
			TargetBeard2Id = beard2Id,
			TargetFrontHairId = frontHairId,
			TargetBackHairId = backHairId
		}));
		_actionEnergies.SpendEnergyOnAction(4);
	}

	private void OfflineCalcGeneralAction_SocialStatus_Beg(DataContext context, PeriAdvanceMonthGeneralActionModification mod, HashSet<int> currBlockChars, HashSet<int> caringCharIds)
	{
		sbyte behaviorType = GetBehaviorType();
		sbyte percentProb = AiHelper.GeneralActionConstants.StartBegChance[behaviorType];
		if (!context.Random.CheckPercentProb(percentProb))
		{
			return;
		}
		List<int> obj = context.AdvanceMonthRelatedData.CharIdList.Occupy();
		foreach (int currBlockChar in currBlockChars)
		{
			if (currBlockChar != _id)
			{
				Character element_Objects = DomainManager.Character.GetElement_Objects(currBlockChar);
				if (element_Objects.GetAgeGroup() != 0 && element_Objects.GetResource(6) >= 500)
				{
					obj.Add(currBlockChar);
				}
			}
		}
		int randomOrDefault = obj.GetRandomOrDefault(context.Random, -1);
		context.AdvanceMonthRelatedData.CharIdList.Release(ref obj);
		if (randomOrDefault >= 0)
		{
			Character element_Objects2 = DomainManager.Character.GetElement_Objects(randomOrDefault);
			sbyte behaviorType2 = element_Objects2.GetBehaviorType();
			sbyte percentProb2 = AiHelper.GeneralActionConstants.BegSuccessChance[behaviorType2];
			mod.PerformedActions.Add((element_Objects2, new SocialStatusBegAction
			{
				MoneyAmount = context.Random.Next(50, 101),
				Succeed = context.Random.CheckPercentProb(percentProb2)
			}));
			_actionEnergies.SpendEnergyOnAction(4);
		}
	}

	private unsafe void OfflineCalcGeneralAction_LifeSkill(DataContext context, PeriAdvanceMonthGeneralActionModification mod, HashSet<int> currBlockChars, HashSet<int> caringCharIds)
	{
		sbyte* ptr = stackalloc sbyte[6] { 0, 1, 2, 3, 4, 5 };
		CollectionUtils.Shuffle(context.Random, ptr, 6);
		for (int i = 0; i < 6; i++)
		{
			if (!_actionEnergies.HasEnoughForAction(4))
			{
				break;
			}
			switch (ptr[i])
			{
			case 0:
				OfflineCalcGeneralAction_Entertainment(context, mod, currBlockChars, caringCharIds);
				break;
			case 1:
				OfflineCalcGeneralAction_Crafting(context, mod);
				break;
			case 2:
				OfflineCalcGeneralAction_Awakening(context, mod, currBlockChars, caringCharIds);
				break;
			case 3:
				OfflineCalcGeneralAction_TeaWine(context, mod);
				break;
			case 4:
				OfflineCalcGeneralAction_CricketBattle(context, mod, currBlockChars, caringCharIds);
				break;
			case 5:
				OfflineCalcGeneralAction_Divination(context, mod, currBlockChars, caringCharIds);
				break;
			}
		}
	}

	private unsafe void OfflineCalcGeneralAction_Entertainment(DataContext context, PeriAdvanceMonthGeneralActionModification mod, HashSet<int> currBlockChars, HashSet<int> caringCharIds)
	{
		LifeSkillShorts lifeSkillAttainments = GetLifeSkillAttainments();
		Span<sbyte> span = stackalloc sbyte[LifeSkillType.EntertainingTypes.Length];
		int num = 0;
		sbyte[] entertainingTypes = LifeSkillType.EntertainingTypes;
		foreach (sbyte b in entertainingTypes)
		{
			if (lifeSkillAttainments.Items[b] >= 200)
			{
				span[num] = b;
				num++;
			}
		}
		if (num != 0)
		{
			sbyte lifeSkillType = span[context.Random.Next(num)];
			HashSet<int> currBlockChars2 = ((caringCharIds.Count > 0) ? caringCharIds : currBlockChars);
			Character character = SelectMaxPriorityActionTarget(context, currBlockChars2, null);
			if (character != null)
			{
				mod.PerformedActions.Add((character, new LifeSkillEntertainmentAction
				{
					LifeSkillType = lifeSkillType
				}));
				_actionEnergies.SpendEnergyOnAction(4);
			}
		}
	}

	private unsafe void OfflineCalcGeneralAction_Crafting(DataContext context, PeriAdvanceMonthGeneralActionModification mod)
	{
		if (_inventory.Items.Count <= 1 || !context.Random.CheckPercentProb(50 + GetPersonality(2)))
		{
			return;
		}
		LifeSkillShorts lifeSkillAttainments = GetLifeSkillAttainments();
		sbyte* ptr = stackalloc sbyte[(int)(uint)LifeSkillType.CraftingTypes.Length];
		int num = 0;
		sbyte[] craftingTypes = LifeSkillType.CraftingTypes;
		foreach (sbyte b in craftingTypes)
		{
			if (lifeSkillAttainments.Items[b] >= 200)
			{
				ptr[num] = b;
				num++;
			}
		}
		if (num == 0)
		{
			return;
		}
		CollectionUtils.Shuffle(context.Random, ptr, num);
		List<ItemKey> obj = context.AdvanceMonthRelatedData.ItemKeys.Occupy();
		for (int j = 0; j < num; j++)
		{
			sbyte b2 = ptr[j];
			_inventory.GetCraftMaterials(b2, obj);
			if (obj.Count == 0)
			{
				continue;
			}
			ItemKey random = obj.GetRandom(context.Random);
			MaterialItem materialItem = Config.Material.Instance[random.TemplateId];
			short durabilityCost;
			ItemKey bestCraftTool = _inventory.GetBestCraftTool(b2, materialItem.Grade, out durabilityCost);
			if (!bestCraftTool.IsValid())
			{
				continue;
			}
			short random2 = materialItem.CraftableItemTypes.GetRandom(context.Random);
			List<short> makeItemSubTypes = MakeItemType.Instance[random2].MakeItemSubTypes;
			short random3 = makeItemSubTypes.GetRandom(context.Random);
			MakeItemSubTypeItem makeItemSubTypeItem = MakeItemSubType.Instance[random3];
			int num2 = GetMakeItemRequiredResourceWorth(materialItem, makeItemSubTypeItem) * OrganizationDomain.GetOrgMemberConfig(_organizationInfo).PurchaseItemDiscount / 100;
			if (_resources.Items[6] >= num2)
			{
				short totalAttainment = (short)(lifeSkillAttainments.Items[b2] + Config.CraftTool.Instance[bestCraftTool.TemplateId].AttainmentBonus);
				(sbyte, short) makeResultTargetItemGradeAndTemplateId = DomainManager.Building.GetMakeResultTargetItemGradeAndTemplateId(random.TemplateId, totalAttainment, b2, makeItemSubTypes, random3, GetLearnedLifeSkills(), context.Random, upgradeMakeItem: false, 0);
				if (makeResultTargetItemGradeAndTemplateId.Item2 >= 0)
				{
					mod.PerformedActions.Add((null, new LifeSkillCraftingAction
					{
						ToolUsed = bestCraftTool,
						Material = random,
						RequiredMoney = num2,
						LifeSkillType = b2,
						TargetItemType = makeItemSubTypeItem.Result.ItemType,
						TargetItemTemplateId = makeResultTargetItemGradeAndTemplateId.Item2
					}));
					_actionEnergies.SpendEnergyOnAction(4);
					break;
				}
			}
		}
		context.AdvanceMonthRelatedData.ItemKeys.Release(ref obj);
		static int GetMakeItemRequiredResourceWorth(MaterialItem materialCfg, MakeItemSubTypeItem makeItemSubTypeCfg)
		{
			return GlobalConfig.ResourcesWorth[materialCfg.ResourceType] * makeItemSubTypeCfg.ResourceTotalCount * materialCfg.RequiredResourceAmount;
		}
	}

	private unsafe void OfflineCalcGeneralAction_Awakening(DataContext context, PeriAdvanceMonthGeneralActionModification mod, HashSet<int> currBlockChars, HashSet<int> caringCharIds)
	{
		LifeSkillShorts baseLifeSkillQualifications = GetBaseLifeSkillQualifications();
		LifeSkillShorts lifeSkillAttainments = GetLifeSkillAttainments();
		short num = lifeSkillAttainments.Items[13];
		short num2 = lifeSkillAttainments.Items[12];
		if (num < 200 && num2 < 200)
		{
			return;
		}
		Character character = null;
		if (caringCharIds.Count > 0)
		{
			character = SelectMaxPriorityActionTarget(context, caringCharIds, IsValidForLifeSkillAwakening);
		}
		if (character == null)
		{
			character = SelectMaxPriorityActionTarget(context, currBlockChars, IsValidForLifeSkillAwakening);
		}
		if (character == null)
		{
			return;
		}
		LifeSkillShorts baseLifeSkillQualifications2 = character.GetBaseLifeSkillQualifications();
		Span<sbyte> span = stackalloc sbyte[16];
		int num3 = 0;
		for (sbyte b = 0; b < 16; b++)
		{
			if (baseLifeSkillQualifications2.Items[b] < 90)
			{
				span[num3] = b;
				num3++;
			}
		}
		if (num3 == 0)
		{
			throw new Exception($"Selected character {character} does not have a life skill type that can be awakened by {this}.");
		}
		sbyte increasedLifeSkillType = span[context.Random.Next(num3)];
		Span<sbyte> span2 = stackalloc sbyte[LifeSkillType.ReligiousTypes.Length];
		int num4 = 0;
		sbyte[] religiousTypes = LifeSkillType.ReligiousTypes;
		foreach (sbyte b2 in religiousTypes)
		{
			if (baseLifeSkillQualifications.Items[b2] > baseLifeSkillQualifications2.Items[b2] && lifeSkillAttainments.Items[b2] >= 200)
			{
				span2[num4] = b2;
				num4++;
			}
		}
		if (num4 == 0)
		{
			throw new Exception($"{this} cannot perform awakening action to {character} due to insufficient buddhism or taoism attainment.");
		}
		sbyte awakeningLifeSkillType = span2[context.Random.Next(num4)];
		mod.PerformedActions.Add((character, new LifeSkillAwakeningAction
		{
			AwakeningLifeSkillType = awakeningLifeSkillType,
			IncreasedLifeSkillType = increasedLifeSkillType
		}));
		_actionEnergies.SpendEnergyOnAction(4);
	}

	private unsafe bool IsValidForLifeSkillAwakening(Character targetChar)
	{
		LifeSkillShorts baseLifeSkillQualifications = GetBaseLifeSkillQualifications();
		LifeSkillShorts lifeSkillAttainments = GetLifeSkillAttainments();
		LifeSkillShorts baseLifeSkillQualifications2 = targetChar.GetBaseLifeSkillQualifications();
		bool flag = false;
		for (sbyte b = 0; b < 16; b++)
		{
			if (baseLifeSkillQualifications2.Items[b] < 90)
			{
				flag = true;
				break;
			}
		}
		if (!flag)
		{
			return false;
		}
		if (baseLifeSkillQualifications.Items[13] > baseLifeSkillQualifications2.Items[13] && lifeSkillAttainments.Items[13] >= 200)
		{
			return true;
		}
		if (baseLifeSkillQualifications.Items[12] > baseLifeSkillQualifications2.Items[12] && lifeSkillAttainments.Items[12] >= 200)
		{
			return true;
		}
		return false;
	}

	private void OfflineCalcGeneralAction_TeaWine(DataContext context, PeriAdvanceMonthGeneralActionModification mod)
	{
		int percentProb = ((_happiness >= 0) ? _happiness : (-_happiness));
		if (!context.Random.CheckPercentProb(percentProb) || GetAgeGroup() != 2)
		{
			return;
		}
		short lifeSkillAttainment = GetLifeSkillAttainment(5);
		if (lifeSkillAttainment >= 150)
		{
			int num = lifeSkillAttainment / 75 - context.Random.Next(4);
			if (num > 8)
			{
				num = 8;
			}
			short teaWineTemplateId = -1;
			if (num >= 0)
			{
				sbyte behaviorType = GetBehaviorType();
				sbyte percentProb2 = AiHelper.UpdateStatusConstants.EatForbiddenFoodChance[behaviorType];
				bool flag = !IsForbiddenToDrinkingWines() || context.Random.CheckPercentProb(percentProb2);
				Span<short> span = stackalloc short[4] { 27, 18, 9, 0 };
				int num2 = (flag ? 4 : 2);
				teaWineTemplateId = (short)(span[context.Random.Next(num2)] + num);
			}
			mod.PerformedActions.Add((null, new LifeSkillTeaWineAction
			{
				TeaWineTemplateId = teaWineTemplateId,
				Amount = 1
			}));
			_actionEnergies.SpendEnergyOnAction(4);
		}
	}

	private void OfflineCalcGeneralAction_CricketBattle(DataContext context, PeriAdvanceMonthGeneralActionModification mod, HashSet<int> currBlockChars, HashSet<int> caringCharIds)
	{
		short lifeSkillAttainment = GetLifeSkillAttainment(15);
		if (lifeSkillAttainment < 150)
		{
			return;
		}
		sbyte selfGrade = _organizationInfo.Grade;
		sbyte behaviorType = GetBehaviorType();
		sbyte[] validGradeOffsets = AiHelper.GeneralActionConstants.CricketBattleGradeOffsets[behaviorType];
		HashSet<int> currBlockChars2 = ((caringCharIds.Count > 0) ? caringCharIds : currBlockChars);
		Character character = SelectMaxPriorityActionTarget(context, currBlockChars2, delegate(Character character2)
		{
			sbyte interactionGrade2 = character2.GetInteractionGrade();
			sbyte[] array = validGradeOffsets;
			foreach (sbyte b in array)
			{
				if (interactionGrade2 + b == selfGrade)
				{
					return true;
				}
			}
			return false;
		});
		if (character != null)
		{
			short num = character.GetLifeSkillAttainment(15);
			sbyte interactionGrade = character.GetInteractionGrade();
			if (num <= 0)
			{
				num = 1;
			}
			int percentProb = lifeSkillAttainment * 50 / num + (selfGrade - interactionGrade) * 20;
			bool flag = context.Random.CheckPercentProb(percentProb);
			Wager wager = (flag ? DomainManager.Item.SelectCharacterValidWager(context, character) : DomainManager.Item.SelectCharacterValidWager(context, this));
			mod.PerformedActions.Add((character, new LifeSkillCricketAction
			{
				Wager = wager,
				Succeed = flag
			}));
			_actionEnergies.SpendEnergyOnAction(4);
		}
	}

	private void OfflineCalcGeneralAction_Divination(DataContext context, PeriAdvanceMonthGeneralActionModification mod, HashSet<int> currBlockChars, HashSet<int> caringCharIds)
	{
		short lifeSkillAttainment = GetLifeSkillAttainment(4);
		if (lifeSkillAttainment < 150 || _health < GetLeftMaxHealth() || _health <= 12)
		{
			return;
		}
		DomainManager.Information.TryGetElement_CharacterSecretInformation(_id, out var selfSecretInfoCollection);
		HashSet<int> currBlockChars2 = ((caringCharIds.Count > 0) ? caringCharIds : currBlockChars);
		Character character = SelectMaxPriorityActionTarget(context, currBlockChars2, delegate(Character character2)
		{
			int id = character2.GetId();
			if (!DomainManager.Information.TryGetElement_CharacterSecretInformation(id, out var value))
			{
				return false;
			}
			if (value.Collection.Count == 0)
			{
				return false;
			}
			if (selfSecretInfoCollection == null)
			{
				return true;
			}
			foreach (int key in value.Collection.Keys)
			{
				if (!selfSecretInfoCollection.Collection.ContainsKey(key))
				{
					return true;
				}
			}
			return false;
		});
		if (character == null)
		{
			return;
		}
		SecretInformationCharacterDataCollection element_CharacterSecretInformation = DomainManager.Information.GetElement_CharacterSecretInformation(character.GetId());
		List<int> obj = context.AdvanceMonthRelatedData.IntList.Occupy();
		obj.Clear();
		if (selfSecretInfoCollection == null)
		{
			obj.AddRange(element_CharacterSecretInformation.Collection.Keys);
		}
		else
		{
			foreach (KeyValuePair<int, SecretInformationCharacterData> item in element_CharacterSecretInformation.Collection)
			{
				if (!selfSecretInfoCollection.Collection.ContainsKey(item.Key))
				{
					obj.Add(item.Key);
				}
			}
		}
		if (obj.Count == 0)
		{
			throw new Exception($"{this} cannot perform divination action to {character} because target has no secret information that {this} doesn't have.");
		}
		int random = obj.GetRandom(context.Random);
		context.AdvanceMonthRelatedData.IntList.Release(ref obj);
		sbyte interactionGrade = GetInteractionGrade();
		sbyte interactionGrade2 = character.GetInteractionGrade();
		short num = character.GetLifeSkillAttainment(4);
		if (num <= 0)
		{
			num = 1;
		}
		int percentProb = lifeSkillAttainment * 50 / num - (interactionGrade2 - interactionGrade) * 20;
		mod.PerformedActions.Add((character, new LifeSkillDivinationAction
		{
			SecretInfoMetaDataId = random,
			Succeed = context.Random.CheckPercentProb(percentProb)
		}));
		_actionEnergies.SpendEnergyOnAction(4);
	}

	public void ClassifyPrioritizedActionTargets(List<int>[] prioritizedActionTargets, HashSet<int> charSet, IRandomSource random)
	{
		foreach (List<int> list in prioritizedActionTargets)
		{
			list.Clear();
		}
		foreach (int item in charSet)
		{
			if (DomainManager.Character.TryGetRelation(_id, item, out var relation))
			{
				sbyte actionTargetType = AiHelper.ActionTargetType.GetActionTargetType(relation.RelationType);
				if (actionTargetType != -1)
				{
					prioritizedActionTargets[actionTargetType].Add(item);
				}
			}
		}
		int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
		if (DomainManager.Extra.IsProfessionalSkillUnlockedAndEquipped(28) && charSet.Contains(taiwuCharId) && DomainManager.Character.TryGetRelation(_id, taiwuCharId, out var relation2))
		{
			sbyte actionTargetType2 = AiHelper.ActionTargetType.GetActionTargetType(relation2.RelationType);
			if (actionTargetType2 != -1)
			{
				ProfessionSkillHandle.AristocratSkill_BoostTaiwuAsTargetInCollection(prioritizedActionTargets[actionTargetType2]);
				CollectionUtils.Shuffle(random, prioritizedActionTargets[actionTargetType2]);
			}
		}
	}

	public int SelectMaxPriorityActionTarget(List<int>[] prioritizedActionTargets, Predicate<int> condition)
	{
		sbyte behaviorType = GetBehaviorType();
		sbyte[] array = AiHelper.ActionTargetType.Priorities[behaviorType];
		sbyte[] array2 = array;
		foreach (sbyte b in array2)
		{
			List<int> list = prioritizedActionTargets[b];
			foreach (int item in list)
			{
				if (condition(item))
				{
					return item;
				}
			}
		}
		return -1;
	}

	public Character SelectRandomActionTarget(DataContext context, HashSet<int> currBlockChars, Predicate<Character> condition, bool includeBabies = false)
	{
		if (currBlockChars == null)
		{
			return null;
		}
		List<int> obj = context.AdvanceMonthRelatedData.CharIdList.Occupy();
		List<int> obj2 = context.AdvanceMonthRelatedData.TargetCharIdList.Occupy();
		obj2.Clear();
		obj.Clear();
		foreach (int currBlockChar in currBlockChars)
		{
			Character element_Objects = DomainManager.Character.GetElement_Objects(currBlockChar);
			if ((includeBabies || element_Objects.GetAgeGroup() != 0) && DomainManager.Character.TryGetRelation(_id, currBlockChar, out var _) && (condition == null || condition(element_Objects)))
			{
				obj.Add(currBlockChar);
				obj2.Add(currBlockChar);
			}
		}
		if (DomainManager.Extra.IsProfessionalSkillUnlockedAndEquipped(28) && obj2.Contains(DomainManager.Taiwu.GetTaiwuCharId()))
		{
			ProfessionSkillHandle.AristocratSkill_BoostTaiwuAsTargetInCollection(obj2);
			int random = obj2.GetRandom(context.Random);
			context.AdvanceMonthRelatedData.CharIdList.Release(ref obj);
			context.AdvanceMonthRelatedData.TargetCharIdList.Release(ref obj2);
			return DomainManager.Character.GetElement_Objects(random);
		}
		Character result = ((obj.Count == 0) ? null : DomainManager.Character.GetElement_Objects(obj.GetRandom(context.Random)));
		context.AdvanceMonthRelatedData.CharIdList.Release(ref obj);
		context.AdvanceMonthRelatedData.TargetCharIdList.Release(ref obj2);
		return result;
	}

	public Character SelectMaxPriorityActionTarget(DataContext context, HashSet<int> currBlockChars, Predicate<Character> condition, bool includeBabies = false)
	{
		if (currBlockChars == null)
		{
			return null;
		}
		sbyte b = -1;
		int num = 0;
		List<int> obj = context.AdvanceMonthRelatedData.CharIdList.Occupy();
		List<int> obj2 = context.AdvanceMonthRelatedData.TargetCharIdList.Occupy();
		sbyte behaviorType = GetBehaviorType();
		foreach (int currBlockChar in currBlockChars)
		{
			Character element_Objects = DomainManager.Character.GetElement_Objects(currBlockChar);
			if ((!includeBabies && element_Objects.GetAgeGroup() == 0) || !DomainManager.Character.TryGetRelation(_id, currBlockChar, out var relation) || (condition != null && !condition(element_Objects)))
			{
				continue;
			}
			sbyte actionTargetType = AiHelper.ActionTargetType.GetActionTargetType(relation.RelationType);
			if (actionTargetType != -1)
			{
				sbyte b2 = AiHelper.ActionTargetType.PriorityScores[behaviorType][actionTargetType];
				if (actionTargetType == b)
				{
					obj.Add(currBlockChar);
					obj2.Add(currBlockChar);
				}
				else if (num < b2)
				{
					b = actionTargetType;
					num = b2;
					obj.Clear();
					obj.Add(currBlockChar);
					obj2.Clear();
					obj2.Add(currBlockChar);
				}
			}
		}
		if (DomainManager.Extra.IsProfessionalSkillUnlockedAndEquipped(28) && obj2.Contains(DomainManager.Taiwu.GetTaiwuCharId()))
		{
			ProfessionSkillHandle.AristocratSkill_BoostTaiwuAsTargetInCollection(obj2);
			int random = obj2.GetRandom(context.Random);
			context.AdvanceMonthRelatedData.CharIdList.Release(ref obj);
			context.AdvanceMonthRelatedData.TargetCharIdList.Release(ref obj2);
			return DomainManager.Character.GetElement_Objects(random);
		}
		Character result = ((obj.Count == 0) ? null : DomainManager.Character.GetElement_Objects(obj.GetRandom(context.Random)));
		context.AdvanceMonthRelatedData.CharIdList.Release(ref obj);
		context.AdvanceMonthRelatedData.TargetCharIdList.Release(ref obj2);
		return result;
	}

	private unsafe (int charId, sbyte actionType) SelectDemandActionTarget(DataContext context, HashSet<int> currBlockChars, HashSet<int> currBlockGraves, Predicate<Character> characterFilter, Predicate<Grave> graveFilter, sbyte restrictActionType)
	{
		IRandomSource random = context.Random;
		sbyte behaviorType = GetBehaviorType();
		int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
		bool flag = DomainManager.Taiwu.CanTaiwuBeSneakyHarmfulActionTarget();
		List<int>[] obj = context.AdvanceMonthRelatedData.DemandActionTargets.Occupy();
		sbyte b = -1;
		int num = 0;
		short aiActionRateAdjust = DomainManager.Extra.GetAiActionRateAdjust(_id, restrictActionType, 2);
		short aiActionRateAdjust2 = DomainManager.Extra.GetAiActionRateAdjust(_id, restrictActionType, 1);
		short aiActionRateAdjust3 = DomainManager.Extra.GetAiActionRateAdjust(_id, restrictActionType, 3);
		if (currBlockChars != null)
		{
			foreach (int currBlockChar in currBlockChars)
			{
				Character element_Objects = DomainManager.Character.GetElement_Objects(currBlockChar);
				if (element_Objects.GetAgeGroup() == 0 || !DomainManager.Character.TryGetRelation(_id, element_Objects._id, out var relation) || !characterFilter(element_Objects))
				{
					continue;
				}
				sbyte targetRelationCategory = AiHelper.ActionTargetRelationCategory.GetTargetRelationCategory(relation.RelationType);
				if (graveFilter != null && random.CheckPercentProb(AiHelper.GeneralActionConstants.StartRobbingChance[behaviorType][targetRelationCategory] + aiActionRateAdjust3))
				{
					obj[3].Add(currBlockChar);
				}
				if (currBlockChar != taiwuCharId || flag)
				{
					if (random.CheckPercentProb(AiHelper.GeneralActionConstants.StartScammingChance[behaviorType][targetRelationCategory] + aiActionRateAdjust))
					{
						obj[2].Add(currBlockChar);
					}
					if (random.CheckPercentProb(AiHelper.GeneralActionConstants.StartStealingChance[behaviorType][targetRelationCategory] + aiActionRateAdjust2))
					{
						obj[1].Add(currBlockChar);
					}
				}
				sbyte actionTargetType = AiHelper.ActionTargetType.GetActionTargetType(relation.RelationType);
				if (actionTargetType != -1)
				{
					sbyte b2 = AiHelper.ActionTargetType.PriorityScores[behaviorType][actionTargetType];
					if (actionTargetType == b)
					{
						obj[0].Add(currBlockChar);
					}
					else if (num < b2)
					{
						b = actionTargetType;
						num = b2;
						obj[0].Clear();
						obj[0].Add(currBlockChar);
					}
				}
			}
		}
		if (graveFilter != null && currBlockGraves != null)
		{
			short aiActionRateAdjust4 = DomainManager.Extra.GetAiActionRateAdjust(_id, restrictActionType, 4);
			foreach (int currBlockGrafe in currBlockGraves)
			{
				Grave element_Graves = DomainManager.Character.GetElement_Graves(currBlockGrafe);
				if (DomainManager.Character.TryGetRelation(_id, currBlockGrafe, out var relation2) && graveFilter(element_Graves) && !DomainManager.Character.IsGraveProtected(element_Graves))
				{
					sbyte targetRelationCategory2 = AiHelper.ActionTargetRelationCategory.GetTargetRelationCategory(relation2.RelationType);
					if (random.CheckPercentProb(AiHelper.GeneralActionConstants.StartRobbingFromGraveChance[behaviorType][targetRelationCategory2] + aiActionRateAdjust4))
					{
						obj[4].Add(currBlockGrafe);
					}
				}
			}
		}
		sbyte* ptr = stackalloc sbyte[5];
		int num2 = 0;
		for (sbyte b3 = 0; b3 < 5; b3++)
		{
			if (obj[b3].Count > 0)
			{
				ptr[num2] = b3;
				num2++;
			}
		}
		if (num2 <= 0)
		{
			context.AdvanceMonthRelatedData.DemandActionTargets.Release(ref obj);
			return (charId: -1, actionType: -1);
		}
		sbyte b4 = ptr[random.Next(num2)];
		if (DomainManager.Extra.IsProfessionalSkillUnlockedAndEquipped(28) && obj[b4].Contains(DomainManager.Taiwu.GetTaiwuCharId()))
		{
			List<int> obj2 = context.AdvanceMonthRelatedData.TargetCharIdList.Occupy();
			foreach (int item in obj[b4])
			{
				obj2.Add(item);
			}
			ProfessionSkillHandle.AristocratSkill_BoostTaiwuAsTargetInCollection(obj2);
			int random2 = obj2.GetRandom(random);
			context.AdvanceMonthRelatedData.TargetCharIdList.Release(ref obj2);
			context.AdvanceMonthRelatedData.DemandActionTargets.Release(ref obj);
			return (charId: random2, actionType: b4);
		}
		int random3 = obj[b4].GetRandom(random);
		context.AdvanceMonthRelatedData.DemandActionTargets.Release(ref obj);
		return (charId: random3, actionType: b4);
	}

	private void GetAllCaringCharsInSet(HashSet<int> charSet, HashSet<int> caringCharIds)
	{
		caringCharIds.Clear();
		foreach (GameData.Domains.Character.Ai.PersonalNeed personalNeed in _personalNeeds)
		{
			if (personalNeed.TemplateId == 19 && charSet.Contains(personalNeed.CharId))
			{
				caringCharIds.Add(personalNeed.CharId);
			}
		}
	}

	public ItemBase SelectSpareableItem(DataContext context, sbyte targetGrade, bool allowUsed)
	{
		int num = 9;
		List<(ItemBase, int)> obj = context.AdvanceMonthRelatedData.ItemsWithAmount.Occupy();
		IReadOnlyDictionary<ItemKey, int> taiwuGiftItems = DomainManager.Extra.GetTaiwuGiftItems(_id);
		int villagerIdealClothing = ((_organizationInfo.OrgTemplateId == 16) ? GetIdealClothingTemplateId() : (-1));
		int num2 = ((villagerIdealClothing >= 0 && _equipment.Exist((ItemKey e) => e.IsValid() && e.ItemType == 3 && e.TemplateId == villagerIdealClothing)) ? 1 : 0);
		foreach (var (itemKey2, num4) in _inventory.Items)
		{
			if (!ItemTemplateHelper.IsTransferable(itemKey2.ItemType, itemKey2.TemplateId) || ItemTemplateHelper.GetBaseValue(itemKey2.ItemType, itemKey2.TemplateId) <= 0 || (taiwuGiftItems.TryGetValue(itemKey2, out var value) && value >= num4))
			{
				continue;
			}
			ItemBase baseItem = DomainManager.Item.GetBaseItem(itemKey2);
			if ((!allowUsed && baseItem.GetCurrDurability() < baseItem.GetMaxDurability()) || TryDetectAttachedPoisons(itemKey2))
			{
				continue;
			}
			if (itemKey2.ItemType == 10)
			{
				if (baseItem.GetItemSubType() == 1001)
				{
					(int, byte) combatSkillBookCurrReadingInfo = GetCombatSkillBookCurrReadingInfo((GameData.Domains.Item.SkillBook)baseItem);
					if (combatSkillBookCurrReadingInfo.Item1 < 0 || combatSkillBookCurrReadingInfo.Item2 < 6)
					{
						continue;
					}
				}
				else
				{
					(int, byte) lifeSkillBookCurrReadingInfo = GetLifeSkillBookCurrReadingInfo((GameData.Domains.Item.SkillBook)baseItem);
					if (lifeSkillBookCurrReadingInfo.Item1 < 0 || lifeSkillBookCurrReadingInfo.Item2 < 5)
					{
						continue;
					}
				}
			}
			else if (itemKey2.ItemType == 3 && num2 > 0)
			{
				num2--;
				continue;
			}
			if (itemKey2.ItemType == 12 && itemKey2.TemplateId == 225)
			{
				continue;
			}
			sbyte grade = baseItem.GetGrade();
			if (grade == num)
			{
				obj.Add((baseItem, num4));
			}
			else if (num < targetGrade)
			{
				if (grade >= num && grade <= targetGrade)
				{
					num = grade;
					obj.Clear();
					obj.Add((baseItem, num4));
				}
			}
			else if (num > targetGrade && grade < num)
			{
				num = grade;
				obj.Clear();
				obj.Add((baseItem, num4));
			}
		}
		ItemBase result = ((obj.Count == 0) ? null : obj.GetRandom(context.Random).Item1);
		context.AdvanceMonthRelatedData.ItemsWithAmount.Release(ref obj);
		return result;
	}

	private bool HasBookToReadForExp()
	{
		foreach (ItemKey key in _inventory.Items.Keys)
		{
			if (key.ItemType == 10)
			{
				GameData.Domains.Item.SkillBook element_SkillBooks = DomainManager.Item.GetElement_SkillBooks(key.Id);
				if (element_SkillBooks.GetCurrDurability() < element_SkillBooks.GetMaxDurability())
				{
					return true;
				}
			}
		}
		return false;
	}

	private ItemKey SelectBookToReadForExp(DataContext context)
	{
		Dictionary<short, GameData.Domains.CombatSkill.CombatSkill> charCombatSkills = DomainManager.CombatSkill.GetCharCombatSkills(_id);
		List<ItemKey> obj = context.AdvanceMonthRelatedData.ItemKeys.Occupy();
		foreach (ItemKey key in _inventory.Items.Keys)
		{
			if (key.ItemType != 10)
			{
				continue;
			}
			GameData.Domains.Item.SkillBook element_SkillBooks = DomainManager.Item.GetElement_SkillBooks(key.Id);
			if (element_SkillBooks.GetCurrDurability() >= element_SkillBooks.GetMaxDurability())
			{
				continue;
			}
			if (element_SkillBooks.IsCombatSkillBook())
			{
				if (charCombatSkills.TryGetValue(element_SkillBooks.GetCombatSkillTemplateId(), out var value) && CombatSkillStateHelper.IsReadNormalPagesMeetConditionOfBreakout(value.GetReadingState()))
				{
					obj.Add(key);
				}
				continue;
			}
			int num = FindLearnedLifeSkillIndex(element_SkillBooks.GetLifeSkillTemplateId());
			if (num >= 0 && _learnedLifeSkills[num].IsAllPagesRead())
			{
				obj.Add(key);
			}
		}
		ItemKey randomOrDefault = obj.GetRandomOrDefault(context.Random, ItemKey.Invalid);
		context.AdvanceMonthRelatedData.ItemKeys.Release(ref obj);
		return randomOrDefault;
	}

	[Obsolete("Use SelectSpareableItem instead.")]
	public ItemKey SelectItemToGive(DataContext context, sbyte targetGrade)
	{
		return SelectSpareableItem(context, targetGrade, allowUsed: true)?.GetItemKey() ?? ItemKey.Invalid;
	}

	public unsafe sbyte GetStealActionPhase(IRandomSource random, Character targetChar, int alertFactor, bool showCheckAnim = false)
	{
		MainAttributes maxMainAttributes = GetMaxMainAttributes();
		HitOrAvoidInts hitValues = GetHitValues();
		HitOrAvoidInts avoidValues = GetAvoidValues();
		CombatSkillShorts combatSkillAttainments = GetCombatSkillAttainments();
		short moveSpeed = GetMoveSpeed();
		HitOrAvoidInts hitValues2 = targetChar.GetHitValues();
		HitOrAvoidInts avoidValues2 = targetChar.GetAvoidValues();
		CombatSkillShorts combatSkillAttainments2 = targetChar.GetCombatSkillAttainments();
		short moveSpeed2 = targetChar.GetMoveSpeed();
		if (showCheckAnim)
		{
			DomainManager.TaiwuEvent.InteractCheckData = new EventInteractCheckData(1)
			{
				SelfMainAttributes = maxMainAttributes,
				SelfHitValues = hitValues,
				SelfAvoidValues = avoidValues,
				SelfCombatSkillAttainments = combatSkillAttainments,
				SelfMoveSpeed = moveSpeed,
				TargetHitValues = hitValues2,
				TargetAvoidValues = avoidValues2,
				TargetCombatSkillAttainments = combatSkillAttainments2,
				TargetMoveSpeed = moveSpeed2,
				TargetAlertFactor = alertFactor,
				SelfNameRelatedData = DomainManager.Character.GetNameRelatedData(_id),
				TargetNameRelatedData = DomainManager.Character.GetNameRelatedData(targetChar.GetId())
			};
			DomainManager.TaiwuEvent.ShowInteractCheckAnimation = true;
		}
		int baseSuccessRate = maxMainAttributes.Items[1];
		baseSuccessRate = GetPhaseAdjustedSuccessRate(baseSuccessRate, 0);
		if (showCheckAnim)
		{
			DomainManager.TaiwuEvent.InteractCheckData.PhaseProbList.Add(baseSuccessRate);
		}
		if (!random.CheckPercentProb(baseSuccessRate))
		{
			if (showCheckAnim)
			{
				DomainManager.TaiwuEvent.InteractCheckData.FailPhase = 0;
			}
			return 0;
		}
		baseSuccessRate = GetPhaseBaseSuccessRate(combatSkillAttainments.Items[1], combatSkillAttainments2.Items[1], alertFactor);
		baseSuccessRate = GetPhaseAdjustedSuccessRate(baseSuccessRate, 1);
		if (showCheckAnim)
		{
			DomainManager.TaiwuEvent.InteractCheckData.PhaseProbList.Add(baseSuccessRate);
		}
		if (!random.CheckPercentProb(baseSuccessRate))
		{
			if (showCheckAnim)
			{
				DomainManager.TaiwuEvent.InteractCheckData.FailPhase = 1;
			}
			return 1;
		}
		baseSuccessRate = GetPhaseBaseSuccessRate(hitValues.Items[2], avoidValues2.Items[2], alertFactor);
		baseSuccessRate = GetPhaseAdjustedSuccessRate(baseSuccessRate, 2);
		if (showCheckAnim)
		{
			DomainManager.TaiwuEvent.InteractCheckData.PhaseProbList.Add(baseSuccessRate);
		}
		if (!random.CheckPercentProb(baseSuccessRate))
		{
			if (showCheckAnim)
			{
				DomainManager.TaiwuEvent.InteractCheckData.FailPhase = 2;
			}
			return 2;
		}
		baseSuccessRate = GetPhaseBaseSuccessRate(moveSpeed, moveSpeed2, alertFactor) / 3;
		baseSuccessRate = GetPhaseAdjustedSuccessRate(baseSuccessRate, 3);
		if (showCheckAnim)
		{
			DomainManager.TaiwuEvent.InteractCheckData.PhaseProbList.Add(baseSuccessRate);
		}
		if (!random.CheckPercentProb(baseSuccessRate))
		{
			if (showCheckAnim)
			{
				DomainManager.TaiwuEvent.InteractCheckData.FailPhase = 3;
			}
			return 3;
		}
		if (!targetChar.NeedToAvoidCombat(CombatType.Beat))
		{
			baseSuccessRate = GetPhaseBaseSuccessRate(avoidValues.Items[2], hitValues2.Items[2], alertFactor);
			baseSuccessRate = GetPhaseAdjustedSuccessRate(baseSuccessRate, 4);
			if (showCheckAnim)
			{
				DomainManager.TaiwuEvent.InteractCheckData.PhaseProbList.Add(baseSuccessRate);
			}
			if (!random.CheckPercentProb(baseSuccessRate))
			{
				if (showCheckAnim)
				{
					DomainManager.TaiwuEvent.InteractCheckData.FailPhase = 4;
				}
				return 4;
			}
		}
		else if (showCheckAnim)
		{
			DomainManager.TaiwuEvent.InteractCheckData.PhaseProbList.Add(100);
		}
		return 5;
	}

	public unsafe sbyte GetScamActionPhase(IRandomSource random, Character targetChar, int alertFactor, bool showCheckAnim = false)
	{
		MainAttributes maxMainAttributes = GetMaxMainAttributes();
		HitOrAvoidInts hitValues = GetHitValues();
		HitOrAvoidInts avoidValues = GetAvoidValues();
		CombatSkillShorts combatSkillAttainments = GetCombatSkillAttainments();
		short maxLifeSkillValue = GetLifeSkillAttainments().GetMaxLifeSkillValue();
		HitOrAvoidInts hitValues2 = targetChar.GetHitValues();
		HitOrAvoidInts avoidValues2 = targetChar.GetAvoidValues();
		CombatSkillShorts combatSkillAttainments2 = targetChar.GetCombatSkillAttainments();
		short maxLifeSkillValue2 = targetChar.GetLifeSkillAttainments().GetMaxLifeSkillValue();
		if (showCheckAnim)
		{
			DomainManager.TaiwuEvent.InteractCheckData = new EventInteractCheckData(0)
			{
				SelfMainAttributes = maxMainAttributes,
				SelfHitValues = hitValues,
				SelfAvoidValues = avoidValues,
				SelfCombatSkillAttainments = combatSkillAttainments,
				TargetHitValues = hitValues2,
				TargetAvoidValues = avoidValues2,
				TargetCombatSkillAttainments = combatSkillAttainments2,
				TargetAlertFactor = alertFactor,
				SelfNameRelatedData = DomainManager.Character.GetNameRelatedData(_id),
				TargetNameRelatedData = DomainManager.Character.GetNameRelatedData(targetChar.GetId())
			};
			DomainManager.TaiwuEvent.ShowInteractCheckAnimation = true;
		}
		int baseSuccessRate = maxMainAttributes.Items[2];
		baseSuccessRate = GetPhaseAdjustedSuccessRate(baseSuccessRate, 0);
		if (showCheckAnim)
		{
			DomainManager.TaiwuEvent.InteractCheckData.PhaseProbList.Add(baseSuccessRate);
		}
		if (!random.CheckPercentProb(baseSuccessRate))
		{
			if (showCheckAnim)
			{
				DomainManager.TaiwuEvent.InteractCheckData.FailPhase = 0;
			}
			return 0;
		}
		baseSuccessRate = GetPhaseBaseSuccessRate(combatSkillAttainments.Items[1], combatSkillAttainments2.Items[1], alertFactor);
		baseSuccessRate = GetPhaseAdjustedSuccessRate(baseSuccessRate, 1);
		if (showCheckAnim)
		{
			DomainManager.TaiwuEvent.InteractCheckData.PhaseProbList.Add(baseSuccessRate);
		}
		if (!random.CheckPercentProb(baseSuccessRate))
		{
			if (showCheckAnim)
			{
				DomainManager.TaiwuEvent.InteractCheckData.FailPhase = 1;
			}
			return 1;
		}
		baseSuccessRate = GetPhaseBaseSuccessRate(hitValues.Items[3], avoidValues2.Items[3], alertFactor);
		baseSuccessRate = GetPhaseAdjustedSuccessRate(baseSuccessRate, 2);
		if (showCheckAnim)
		{
			DomainManager.TaiwuEvent.InteractCheckData.PhaseProbList.Add(baseSuccessRate);
		}
		if (!random.CheckPercentProb(baseSuccessRate))
		{
			if (showCheckAnim)
			{
				DomainManager.TaiwuEvent.InteractCheckData.FailPhase = 2;
			}
			return 2;
		}
		baseSuccessRate = GetPhaseBaseSuccessRate(maxLifeSkillValue, maxLifeSkillValue2, alertFactor) / 3;
		baseSuccessRate = GetPhaseAdjustedSuccessRate(baseSuccessRate, 3);
		if (showCheckAnim)
		{
			DomainManager.TaiwuEvent.InteractCheckData.PhaseProbList.Add(baseSuccessRate);
		}
		if (!showCheckAnim && !random.CheckPercentProb(baseSuccessRate))
		{
			return 3;
		}
		if (!targetChar.NeedToAvoidCombat(CombatType.Beat))
		{
			baseSuccessRate = GetPhaseBaseSuccessRate(avoidValues.Items[3], hitValues2.Items[3], alertFactor);
			baseSuccessRate = GetPhaseAdjustedSuccessRate(baseSuccessRate, 4);
			if (showCheckAnim)
			{
				DomainManager.TaiwuEvent.InteractCheckData.PhaseProbList.Add(baseSuccessRate);
			}
			if (!random.CheckPercentProb(baseSuccessRate))
			{
				if (showCheckAnim)
				{
					DomainManager.TaiwuEvent.InteractCheckData.FailPhase = 4;
				}
				return 4;
			}
		}
		else if (showCheckAnim)
		{
			DomainManager.TaiwuEvent.InteractCheckData.PhaseProbList.Add(100);
		}
		return 5;
	}

	public unsafe sbyte GetRobActionPhase(IRandomSource random, Character targetChar, int alertFactor, bool showCheckAnim = false)
	{
		MainAttributes maxMainAttributes = GetMaxMainAttributes();
		HitOrAvoidInts hitValues = GetHitValues();
		HitOrAvoidInts avoidValues = GetAvoidValues();
		CombatSkillShorts combatSkillAttainments = GetCombatSkillAttainments();
		int combatPower = GetCombatPower();
		HitOrAvoidInts hitValues2 = targetChar.GetHitValues();
		HitOrAvoidInts avoidValues2 = targetChar.GetAvoidValues();
		CombatSkillShorts combatSkillAttainments2 = targetChar.GetCombatSkillAttainments();
		int combatPower2 = targetChar.GetCombatPower();
		if (showCheckAnim)
		{
			DomainManager.TaiwuEvent.InteractCheckData = new EventInteractCheckData(2)
			{
				SelfMainAttributes = maxMainAttributes,
				SelfHitValues = hitValues,
				SelfAvoidValues = avoidValues,
				SelfCombatSkillAttainments = combatSkillAttainments,
				TargetHitValues = hitValues2,
				TargetAvoidValues = avoidValues2,
				TargetCombatSkillAttainments = combatSkillAttainments2,
				CombatPowerHigher = (combatPower > combatPower2),
				TargetAlertFactor = alertFactor,
				SelfNameRelatedData = DomainManager.Character.GetNameRelatedData(_id),
				TargetNameRelatedData = DomainManager.Character.GetNameRelatedData(targetChar.GetId())
			};
			DomainManager.TaiwuEvent.ShowInteractCheckAnimation = true;
		}
		int items = maxMainAttributes.Items[0];
		items = GetPhaseAdjustedSuccessRate(items, 0);
		if (showCheckAnim)
		{
			DomainManager.TaiwuEvent.InteractCheckData.PhaseProbList.Add(items);
		}
		if (!random.CheckPercentProb(items))
		{
			if (showCheckAnim)
			{
				DomainManager.TaiwuEvent.InteractCheckData.FailPhase = 0;
			}
			return 0;
		}
		items = GetPhaseBaseSuccessRate(combatSkillAttainments.Items[1], combatSkillAttainments2.Items[1], alertFactor);
		items = GetPhaseAdjustedSuccessRate(items, 1);
		if (showCheckAnim)
		{
			DomainManager.TaiwuEvent.InteractCheckData.PhaseProbList.Add(items);
		}
		if (!random.CheckPercentProb(items))
		{
			if (showCheckAnim)
			{
				DomainManager.TaiwuEvent.InteractCheckData.FailPhase = 1;
			}
			return 1;
		}
		items = GetPhaseBaseSuccessRate(hitValues.Items[0], avoidValues2.Items[0], alertFactor);
		items = GetPhaseAdjustedSuccessRate(items, 2);
		if (showCheckAnim)
		{
			DomainManager.TaiwuEvent.InteractCheckData.PhaseProbList.Add(items);
		}
		if (!random.CheckPercentProb(items))
		{
			if (showCheckAnim)
			{
				DomainManager.TaiwuEvent.InteractCheckData.FailPhase = 2;
			}
			return 2;
		}
		items = GetPhaseBaseSuccessRate(combatPower, combatPower2, alertFactor) / 3;
		items = GetPhaseAdjustedSuccessRate(items, 3);
		if (showCheckAnim)
		{
			DomainManager.TaiwuEvent.InteractCheckData.PhaseProbList.Add(items);
		}
		if (!random.CheckPercentProb(items))
		{
			if (showCheckAnim)
			{
				DomainManager.TaiwuEvent.InteractCheckData.FailPhase = 3;
			}
			return 3;
		}
		if (!targetChar.NeedToAvoidCombat(CombatType.Beat))
		{
			items = GetPhaseBaseSuccessRate(avoidValues.Items[0], hitValues2.Items[0], alertFactor);
			items = GetPhaseAdjustedSuccessRate(items, 4);
			if (showCheckAnim)
			{
				DomainManager.TaiwuEvent.InteractCheckData.PhaseProbList.Add(items);
			}
			if (!random.CheckPercentProb(items))
			{
				if (showCheckAnim)
				{
					DomainManager.TaiwuEvent.InteractCheckData.FailPhase = 4;
				}
				return 4;
			}
		}
		else if (showCheckAnim)
		{
			DomainManager.TaiwuEvent.InteractCheckData.PhaseProbList.Add(100);
		}
		return 5;
	}

	public unsafe sbyte GetStealLifeSkillActionPhase(IRandomSource random, Character targetChar, sbyte lifeSkillType, sbyte grade, bool showCheckAnim = false)
	{
		MainAttributes maxMainAttributes = GetMaxMainAttributes();
		HitOrAvoidInts hitValues = GetHitValues();
		HitOrAvoidInts avoidValues = GetAvoidValues();
		CombatSkillShorts combatSkillAttainments = GetCombatSkillAttainments();
		LifeSkillShorts lifeSkillQualifications = GetLifeSkillQualifications();
		HitOrAvoidInts hitValues2 = targetChar.GetHitValues();
		HitOrAvoidInts avoidValues2 = targetChar.GetAvoidValues();
		CombatSkillShorts combatSkillAttainments2 = targetChar.GetCombatSkillAttainments();
		short practiceQualificationRequirement = SkillGradeData.Instance[grade].PracticeQualificationRequirement;
		int gradeAlertFactor = targetChar.GetGradeAlertFactor(grade, 1);
		if (showCheckAnim)
		{
			DomainManager.TaiwuEvent.InteractCheckData = new EventInteractCheckData(6)
			{
				SelfMainAttributes = maxMainAttributes,
				SelfHitValues = hitValues,
				SelfAvoidValues = avoidValues,
				SelfCombatSkillAttainments = combatSkillAttainments,
				SelfLifeSkillQualities = lifeSkillQualifications,
				TargetHitValues = hitValues2,
				TargetAvoidValues = avoidValues2,
				TargetCombatSkillAttainments = combatSkillAttainments2,
				StealSkillGrade = grade,
				StealLifeSkillType = lifeSkillType,
				TargetAlertFactor = gradeAlertFactor,
				SelfNameRelatedData = DomainManager.Character.GetNameRelatedData(_id),
				TargetNameRelatedData = DomainManager.Character.GetNameRelatedData(targetChar.GetId())
			};
			DomainManager.TaiwuEvent.ShowInteractCheckAnimation = true;
		}
		int baseSuccessRate = maxMainAttributes.Items[5];
		baseSuccessRate = GetPhaseAdjustedSuccessRate(baseSuccessRate, 0);
		if (showCheckAnim)
		{
			DomainManager.TaiwuEvent.InteractCheckData.PhaseProbList.Add(baseSuccessRate);
		}
		if (!random.CheckPercentProb(baseSuccessRate))
		{
			if (showCheckAnim)
			{
				DomainManager.TaiwuEvent.InteractCheckData.FailPhase = 0;
			}
			return 0;
		}
		baseSuccessRate = GetPhaseBaseSuccessRate(combatSkillAttainments.Items[1], combatSkillAttainments2.Items[1], gradeAlertFactor);
		baseSuccessRate = GetPhaseAdjustedSuccessRate(baseSuccessRate, 1);
		if (showCheckAnim)
		{
			DomainManager.TaiwuEvent.InteractCheckData.PhaseProbList.Add(baseSuccessRate);
		}
		if (!random.CheckPercentProb(baseSuccessRate))
		{
			if (showCheckAnim)
			{
				DomainManager.TaiwuEvent.InteractCheckData.FailPhase = 1;
			}
			return 1;
		}
		baseSuccessRate = GetPhaseBaseSuccessRate(hitValues.Items[1], avoidValues2.Items[1], gradeAlertFactor);
		baseSuccessRate = GetPhaseAdjustedSuccessRate(baseSuccessRate, 2);
		if (showCheckAnim)
		{
			DomainManager.TaiwuEvent.InteractCheckData.PhaseProbList.Add(baseSuccessRate);
		}
		if (!random.CheckPercentProb(baseSuccessRate))
		{
			if (showCheckAnim)
			{
				DomainManager.TaiwuEvent.InteractCheckData.FailPhase = 2;
			}
			return 2;
		}
		baseSuccessRate = GetPhaseBaseSuccessRate(lifeSkillQualifications.Items[lifeSkillType], practiceQualificationRequirement, gradeAlertFactor) / 3;
		baseSuccessRate = GetPhaseAdjustedSuccessRate(baseSuccessRate, 3);
		if (showCheckAnim)
		{
			DomainManager.TaiwuEvent.InteractCheckData.PhaseProbList.Add(baseSuccessRate);
		}
		if (!random.CheckPercentProb(baseSuccessRate))
		{
			if (showCheckAnim)
			{
				DomainManager.TaiwuEvent.InteractCheckData.FailPhase = 3;
			}
			return 3;
		}
		if (!targetChar.NeedToAvoidCombat(CombatType.Beat))
		{
			baseSuccessRate = GetPhaseBaseSuccessRate(avoidValues.Items[1], hitValues2.Items[1], gradeAlertFactor);
			baseSuccessRate = GetPhaseAdjustedSuccessRate(baseSuccessRate, 4);
			if (showCheckAnim)
			{
				DomainManager.TaiwuEvent.InteractCheckData.PhaseProbList.Add(baseSuccessRate);
			}
			if (!random.CheckPercentProb(baseSuccessRate))
			{
				if (showCheckAnim)
				{
					DomainManager.TaiwuEvent.InteractCheckData.FailPhase = 4;
				}
				return 4;
			}
		}
		else if (showCheckAnim)
		{
			DomainManager.TaiwuEvent.InteractCheckData.PhaseProbList.Add(100);
		}
		return 5;
	}

	public unsafe sbyte GetStealCombatSkillActionPhase(IRandomSource random, Character targetChar, sbyte combatSkillType, sbyte grade, bool showCheckAnim = false)
	{
		MainAttributes maxMainAttributes = GetMaxMainAttributes();
		HitOrAvoidInts hitValues = GetHitValues();
		HitOrAvoidInts avoidValues = GetAvoidValues();
		CombatSkillShorts combatSkillAttainments = GetCombatSkillAttainments();
		CombatSkillShorts combatSkillQualifications = GetCombatSkillQualifications();
		HitOrAvoidInts hitValues2 = targetChar.GetHitValues();
		HitOrAvoidInts avoidValues2 = targetChar.GetAvoidValues();
		CombatSkillShorts combatSkillAttainments2 = targetChar.GetCombatSkillAttainments();
		short practiceQualificationRequirement = SkillGradeData.Instance[grade].PracticeQualificationRequirement;
		int gradeAlertFactor = targetChar.GetGradeAlertFactor(grade, 1);
		if (showCheckAnim)
		{
			DomainManager.TaiwuEvent.InteractCheckData = new EventInteractCheckData(7)
			{
				SelfMainAttributes = maxMainAttributes,
				SelfHitValues = hitValues,
				SelfAvoidValues = avoidValues,
				SelfCombatSkillAttainments = combatSkillAttainments,
				SelfCombatSkillQualities = combatSkillQualifications,
				TargetHitValues = hitValues2,
				TargetAvoidValues = avoidValues2,
				TargetCombatSkillAttainments = combatSkillAttainments2,
				StealSkillGrade = grade,
				StealCombatSkillType = combatSkillType,
				TargetAlertFactor = gradeAlertFactor,
				SelfNameRelatedData = DomainManager.Character.GetNameRelatedData(_id),
				TargetNameRelatedData = DomainManager.Character.GetNameRelatedData(targetChar.GetId())
			};
			DomainManager.TaiwuEvent.ShowInteractCheckAnimation = true;
		}
		int baseSuccessRate = maxMainAttributes.Items[5];
		baseSuccessRate = GetPhaseAdjustedSuccessRate(baseSuccessRate, 0);
		if (showCheckAnim)
		{
			DomainManager.TaiwuEvent.InteractCheckData.PhaseProbList.Add(baseSuccessRate);
		}
		if (!random.CheckPercentProb(baseSuccessRate))
		{
			if (showCheckAnim)
			{
				DomainManager.TaiwuEvent.InteractCheckData.FailPhase = 0;
			}
			return 0;
		}
		baseSuccessRate = GetPhaseBaseSuccessRate(combatSkillAttainments.Items[1], combatSkillAttainments2.Items[1], gradeAlertFactor);
		baseSuccessRate = GetPhaseAdjustedSuccessRate(baseSuccessRate, 1);
		if (showCheckAnim)
		{
			DomainManager.TaiwuEvent.InteractCheckData.PhaseProbList.Add(baseSuccessRate);
		}
		if (!random.CheckPercentProb(baseSuccessRate))
		{
			if (showCheckAnim)
			{
				DomainManager.TaiwuEvent.InteractCheckData.FailPhase = 1;
			}
			return 1;
		}
		baseSuccessRate = GetPhaseBaseSuccessRate(hitValues.Items[1], avoidValues2.Items[1], gradeAlertFactor);
		baseSuccessRate = GetPhaseAdjustedSuccessRate(baseSuccessRate, 2);
		if (showCheckAnim)
		{
			DomainManager.TaiwuEvent.InteractCheckData.PhaseProbList.Add(baseSuccessRate);
		}
		if (!random.CheckPercentProb(baseSuccessRate))
		{
			if (showCheckAnim)
			{
				DomainManager.TaiwuEvent.InteractCheckData.FailPhase = 2;
			}
			return 2;
		}
		baseSuccessRate = GetPhaseBaseSuccessRate(combatSkillQualifications.Items[combatSkillType], practiceQualificationRequirement, gradeAlertFactor) / 3;
		baseSuccessRate = GetPhaseAdjustedSuccessRate(baseSuccessRate, 3);
		if (showCheckAnim)
		{
			DomainManager.TaiwuEvent.InteractCheckData.PhaseProbList.Add(baseSuccessRate);
		}
		if (!random.CheckPercentProb(baseSuccessRate))
		{
			if (showCheckAnim)
			{
				DomainManager.TaiwuEvent.InteractCheckData.FailPhase = 3;
			}
			return 3;
		}
		if (!targetChar.NeedToAvoidCombat(CombatType.Beat))
		{
			baseSuccessRate = GetPhaseBaseSuccessRate(avoidValues.Items[1], hitValues2.Items[1], gradeAlertFactor);
			baseSuccessRate = GetPhaseAdjustedSuccessRate(baseSuccessRate, 4);
			if (showCheckAnim)
			{
				DomainManager.TaiwuEvent.InteractCheckData.PhaseProbList.Add(baseSuccessRate);
			}
			if (!random.CheckPercentProb(baseSuccessRate))
			{
				if (showCheckAnim)
				{
					DomainManager.TaiwuEvent.InteractCheckData.FailPhase = 4;
				}
				return 4;
			}
		}
		else if (showCheckAnim)
		{
			DomainManager.TaiwuEvent.InteractCheckData.PhaseProbList.Add(100);
		}
		return 5;
	}

	public unsafe sbyte GetPoisonActionPhase(IRandomSource random, Character targetChar, int alertFactor = 100, bool showCheckAnim = false)
	{
		CombatSkillShorts combatSkillAttainments = GetCombatSkillAttainments();
		LifeSkillShorts lifeSkillAttainments = GetLifeSkillAttainments();
		OuterAndInnerInts penetrations = GetPenetrations();
		OuterAndInnerInts penetrationResists = GetPenetrationResists();
		short castSpeed = GetCastSpeed();
		CombatSkillShorts combatSkillAttainments2 = targetChar.GetCombatSkillAttainments();
		LifeSkillShorts lifeSkillAttainments2 = targetChar.GetLifeSkillAttainments();
		OuterAndInnerInts penetrations2 = targetChar.GetPenetrations();
		OuterAndInnerInts penetrationResists2 = targetChar.GetPenetrationResists();
		short castSpeed2 = targetChar.GetCastSpeed();
		if (showCheckAnim)
		{
			DomainManager.TaiwuEvent.InteractCheckData = new EventInteractCheckData(3)
			{
				SelfCombatSkillAttainments = combatSkillAttainments,
				SelfLifeSkillAttainments = lifeSkillAttainments,
				SelfPenetrations = penetrations,
				SelfPenetrationResists = penetrationResists,
				SelfCastSpeed = castSpeed,
				TargetCombatSkillAttainments = combatSkillAttainments2,
				TargetLifeSkillAttainments = lifeSkillAttainments2,
				TargetPenetrations = penetrations2,
				TargetPenetrationResists = penetrationResists2,
				TargetCastSpeed = castSpeed2,
				TargetAlertFactor = alertFactor,
				SelfNameRelatedData = DomainManager.Character.GetNameRelatedData(_id),
				TargetNameRelatedData = DomainManager.Character.GetNameRelatedData(targetChar.GetId())
			};
			DomainManager.TaiwuEvent.ShowInteractCheckAnimation = true;
		}
		int phaseBaseSuccessRate = GetPhaseBaseSuccessRate(lifeSkillAttainments.Items[9], lifeSkillAttainments2.Items[9], alertFactor);
		phaseBaseSuccessRate = GetPhaseAdjustedSuccessRate(phaseBaseSuccessRate, 0);
		if (showCheckAnim)
		{
			DomainManager.TaiwuEvent.InteractCheckData.PhaseProbList.Add(phaseBaseSuccessRate);
		}
		if (!random.CheckPercentProb(phaseBaseSuccessRate))
		{
			if (showCheckAnim)
			{
				DomainManager.TaiwuEvent.InteractCheckData.FailPhase = 0;
			}
			return 0;
		}
		phaseBaseSuccessRate = GetPhaseBaseSuccessRate(combatSkillAttainments.Items[1], combatSkillAttainments2.Items[1], alertFactor);
		phaseBaseSuccessRate = GetPhaseAdjustedSuccessRate(phaseBaseSuccessRate, 1);
		if (showCheckAnim)
		{
			DomainManager.TaiwuEvent.InteractCheckData.PhaseProbList.Add(phaseBaseSuccessRate);
		}
		if (!random.CheckPercentProb(phaseBaseSuccessRate))
		{
			if (showCheckAnim)
			{
				DomainManager.TaiwuEvent.InteractCheckData.FailPhase = 1;
			}
			return 1;
		}
		phaseBaseSuccessRate = GetPhaseBaseSuccessRate(penetrations.Inner, penetrationResists2.Inner, alertFactor);
		phaseBaseSuccessRate = GetPhaseAdjustedSuccessRate(phaseBaseSuccessRate, 2);
		if (showCheckAnim)
		{
			DomainManager.TaiwuEvent.InteractCheckData.PhaseProbList.Add(phaseBaseSuccessRate);
		}
		if (!random.CheckPercentProb(phaseBaseSuccessRate))
		{
			if (showCheckAnim)
			{
				DomainManager.TaiwuEvent.InteractCheckData.FailPhase = 2;
			}
			return 2;
		}
		phaseBaseSuccessRate = GetPhaseBaseSuccessRate(castSpeed, castSpeed2, alertFactor) / 3;
		phaseBaseSuccessRate = GetPhaseAdjustedSuccessRate(phaseBaseSuccessRate, 3);
		if (showCheckAnim)
		{
			DomainManager.TaiwuEvent.InteractCheckData.PhaseProbList.Add(phaseBaseSuccessRate);
		}
		if (!random.CheckPercentProb(phaseBaseSuccessRate))
		{
			if (showCheckAnim)
			{
				DomainManager.TaiwuEvent.InteractCheckData.FailPhase = 3;
			}
			return 3;
		}
		if (!targetChar.NeedToAvoidCombat(CombatType.Beat))
		{
			phaseBaseSuccessRate = GetPhaseBaseSuccessRate(penetrationResists.Inner, penetrations2.Inner, alertFactor);
			phaseBaseSuccessRate = GetPhaseAdjustedSuccessRate(phaseBaseSuccessRate, 4);
			if (showCheckAnim)
			{
				DomainManager.TaiwuEvent.InteractCheckData.PhaseProbList.Add(phaseBaseSuccessRate);
			}
			if (!random.CheckPercentProb(phaseBaseSuccessRate))
			{
				if (showCheckAnim)
				{
					DomainManager.TaiwuEvent.InteractCheckData.FailPhase = 4;
				}
				return 4;
			}
		}
		else if (showCheckAnim)
		{
			DomainManager.TaiwuEvent.InteractCheckData.PhaseProbList.Add(100);
		}
		return 5;
	}

	public unsafe sbyte GetPlotHarmActionPhase(IRandomSource random, Character targetChar, int alertFactor = 100, bool showCheckAnim = false)
	{
		CombatSkillShorts combatSkillAttainments = GetCombatSkillAttainments();
		LifeSkillShorts lifeSkillAttainments = GetLifeSkillAttainments();
		OuterAndInnerInts penetrations = GetPenetrations();
		OuterAndInnerInts penetrationResists = GetPenetrationResists();
		short attackSpeed = GetAttackSpeed();
		CombatSkillShorts combatSkillAttainments2 = targetChar.GetCombatSkillAttainments();
		LifeSkillShorts lifeSkillAttainments2 = targetChar.GetLifeSkillAttainments();
		OuterAndInnerInts penetrations2 = targetChar.GetPenetrations();
		OuterAndInnerInts penetrationResists2 = targetChar.GetPenetrationResists();
		short attackSpeed2 = targetChar.GetAttackSpeed();
		if (showCheckAnim)
		{
			DomainManager.TaiwuEvent.InteractCheckData = new EventInteractCheckData(4)
			{
				SelfCombatSkillAttainments = combatSkillAttainments,
				SelfLifeSkillAttainments = lifeSkillAttainments,
				SelfPenetrations = penetrations,
				SelfPenetrationResists = penetrationResists,
				SelfAttackSpeed = attackSpeed,
				TargetCombatSkillAttainments = combatSkillAttainments2,
				TargetLifeSkillAttainments = lifeSkillAttainments2,
				TargetPenetrations = penetrations2,
				TargetPenetrationResists = penetrationResists2,
				TargetAttackSpeed = attackSpeed2,
				TargetAlertFactor = alertFactor,
				SelfNameRelatedData = DomainManager.Character.GetNameRelatedData(_id),
				TargetNameRelatedData = DomainManager.Character.GetNameRelatedData(targetChar.GetId())
			};
			DomainManager.TaiwuEvent.ShowInteractCheckAnimation = true;
		}
		int phaseBaseSuccessRate = GetPhaseBaseSuccessRate(lifeSkillAttainments.Items[8], lifeSkillAttainments2.Items[8], alertFactor);
		phaseBaseSuccessRate = GetPhaseAdjustedSuccessRate(phaseBaseSuccessRate, 0);
		if (showCheckAnim)
		{
			DomainManager.TaiwuEvent.InteractCheckData.PhaseProbList.Add(phaseBaseSuccessRate);
		}
		if (!random.CheckPercentProb(phaseBaseSuccessRate))
		{
			if (showCheckAnim)
			{
				DomainManager.TaiwuEvent.InteractCheckData.FailPhase = 0;
			}
			return 0;
		}
		phaseBaseSuccessRate = GetPhaseBaseSuccessRate(combatSkillAttainments.Items[1], combatSkillAttainments2.Items[1], alertFactor);
		phaseBaseSuccessRate = GetPhaseAdjustedSuccessRate(phaseBaseSuccessRate, 1);
		if (showCheckAnim)
		{
			DomainManager.TaiwuEvent.InteractCheckData.PhaseProbList.Add(phaseBaseSuccessRate);
		}
		if (!random.CheckPercentProb(phaseBaseSuccessRate))
		{
			if (showCheckAnim)
			{
				DomainManager.TaiwuEvent.InteractCheckData.FailPhase = 1;
			}
			return 1;
		}
		phaseBaseSuccessRate = GetPhaseBaseSuccessRate(penetrations.Outer, penetrationResists2.Outer, alertFactor);
		phaseBaseSuccessRate = GetPhaseAdjustedSuccessRate(phaseBaseSuccessRate, 2);
		if (showCheckAnim)
		{
			DomainManager.TaiwuEvent.InteractCheckData.PhaseProbList.Add(phaseBaseSuccessRate);
		}
		if (!random.CheckPercentProb(phaseBaseSuccessRate))
		{
			if (showCheckAnim)
			{
				DomainManager.TaiwuEvent.InteractCheckData.FailPhase = 2;
			}
			return 2;
		}
		phaseBaseSuccessRate = GetPhaseBaseSuccessRate(attackSpeed, attackSpeed2, alertFactor) / 3;
		phaseBaseSuccessRate = GetPhaseAdjustedSuccessRate(phaseBaseSuccessRate, 3);
		if (showCheckAnim)
		{
			DomainManager.TaiwuEvent.InteractCheckData.PhaseProbList.Add(phaseBaseSuccessRate);
		}
		if (!random.CheckPercentProb(phaseBaseSuccessRate))
		{
			if (showCheckAnim)
			{
				DomainManager.TaiwuEvent.InteractCheckData.FailPhase = 3;
			}
			return 3;
		}
		if (!targetChar.NeedToAvoidCombat(CombatType.Beat))
		{
			phaseBaseSuccessRate = GetPhaseBaseSuccessRate(penetrationResists.Outer, penetrations2.Outer, alertFactor);
			phaseBaseSuccessRate = GetPhaseAdjustedSuccessRate(phaseBaseSuccessRate, 4);
			if (showCheckAnim)
			{
				DomainManager.TaiwuEvent.InteractCheckData.PhaseProbList.Add(phaseBaseSuccessRate);
			}
			if (!random.CheckPercentProb(phaseBaseSuccessRate))
			{
				if (showCheckAnim)
				{
					DomainManager.TaiwuEvent.InteractCheckData.FailPhase = 4;
				}
				return 4;
			}
		}
		else if (showCheckAnim)
		{
			DomainManager.TaiwuEvent.InteractCheckData.PhaseProbList.Add(100);
		}
		return 5;
	}

	private static int GetPhaseBaseSuccessRate(int selfVal, int targetVal, int targetAlertFactor)
	{
		return Math.Max(selfVal, 1) * GlobalConfig.Instance.HarmfulActionPhaseBaseSuccessRate / Math.Max(targetVal * targetAlertFactor / 100, 1);
	}

	private unsafe int GetPhaseAdjustedSuccessRate(int baseSuccessRate, sbyte harmfulActionPhase)
	{
		Personalities personalities = GetPersonalities();
		sbyte b = personalities.Items[HarmfulActionPhase.ToPersonalityType[harmfulActionPhase]];
		return baseSuccessRate * (GlobalConfig.Instance.HarmfulActionSuccessGlobalFactor + b) / 100;
	}

	public Character GetGuardForCalculation(IRandomSource random)
	{
		if (_organizationInfo.SettlementId >= 0 && Config.Organization.Instance[_organizationInfo.OrgTemplateId].IsCivilian && DomainManager.Character.HasGuard(_id, this))
		{
			sbyte fameType = GetFameType();
			bool isHeretic = ((fameType == -2) ? random.NextBool() : (fameType < 3));
			Location location = DomainManager.Organization.GetSettlement(_organizationInfo.SettlementId).GetLocation();
			sbyte stateTemplateIdByAreaId = DomainManager.Map.GetStateTemplateIdByAreaId(location.AreaId);
			return DomainManager.Character.GetPregeneratedCityTownGuard(stateTemplateIdByAreaId, isHeretic, _organizationInfo.Grade);
		}
		return this;
	}

	public int GetItemAlertFactor(ItemKey itemKey, int amount)
	{
		sbyte grade = ItemTemplateHelper.GetGrade(itemKey.ItemType, itemKey.TemplateId);
		return GetGradeAlertFactor(grade, amount);
	}

	public int GetResourceAlertFactor(sbyte resourceType)
	{
		sbyte b = ResourceTypeHelper.ResourceAmountToGrade(resourceType, _resources[resourceType]);
		if (b < 0)
		{
			b = 0;
		}
		return GetGradeAlertFactor(b, 1);
	}

	public int GetGradeAlertFactor(sbyte grade, int amount)
	{
		sbyte b = GlobalConfig.Instance.CharacterGradeAlertness[_organizationInfo.Grade];
		return Math.Clamp(100 + (grade - b) * amount * 100, 100, 300);
	}

	public void PeriAdvanceMonth_MixedPoisonEffect(DataContext context)
	{
		List<(sbyte, int)> list = null;
		foreach (MixPoisonEffectItem item in (IEnumerable<MixPoisonEffectItem>)MixPoisonEffect.Instance)
		{
			sbyte b = MixedPoisonType.FromMedicineTemplateId(item.MedicineId);
			int mixedPoisonTypeRelatedMarkCount = GetMixedPoisonTypeRelatedMarkCount(b);
			if (mixedPoisonTypeRelatedMarkCount > 0 && (b != 22 || (_location.IsValid() && _kidnapperId < 0)))
			{
				if (list == null)
				{
					list = new List<(sbyte, int)>();
				}
				list.Add((b, mixedPoisonTypeRelatedMarkCount));
			}
		}
		if (list != null)
		{
			ParallelModificationsRecorder parallelModificationsRecorder = context.ParallelModificationsRecorder;
			parallelModificationsRecorder.RecordType(ParallelModificationType.PeriAdvanceMonthMixedPoisonEffect);
			parallelModificationsRecorder.RecordParameterClass(this);
			parallelModificationsRecorder.RecordParameterClass(list);
		}
	}

	public static void ComplementPeriAdvanceMonth_MixedPoisonEffect(DataContext context, Character character, List<(sbyte mixedPoisonType, int markCount)> mixedPoisonInfoList)
	{
		if (!DomainManager.Character.IsCharacterAlive(character._id))
		{
			return;
		}
		Character taiwu = DomainManager.Taiwu.GetTaiwu();
		MonthlyNotificationCollection monthlyNotificationCollection = DomainManager.World.GetMonthlyNotificationCollection();
		LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
		int currDate = DomainManager.World.GetCurrDate();
		foreach (var (b, num) in mixedPoisonInfoList)
		{
			switch (b)
			{
			case 15:
				character.ChangeHealth(context, -num * 12);
				lifeRecordCollection.AddMixPoisonHotRedRotten(character._id, currDate, character._location);
				break;
			case 24:
				character.MakeExistingInjuriesWorse(context, isInnerInjury: false, (sbyte)(num / 3));
				lifeRecordCollection.AddMixPoisonHotRedCold(character._id, currDate, character._location);
				break;
			case 33:
			{
				int delta = -character._currNeili * num * 5 / 100;
				character.ChangeCurrNeili(context, delta);
				lifeRecordCollection.AddMixPoisonHotGloomyIllusory(character._id, currDate, character._location);
				break;
			}
			case 26:
			{
				List<short> list = ObjectPool<List<short>>.Instance.Get();
				CombatSkillEquipment combatSkillEquipment = character.GetCombatSkillEquipment();
				combatSkillEquipment.GetValidSkills(list);
				if (list.Count > 0)
				{
					for (int j = 0; j < num; j++)
					{
						short random = list.GetRandom(context.Random);
						DomainManager.Combat.AddGoneMadInjuryOutOfCombat(context, character, random);
					}
				}
				ObjectPool<List<short>>.Instance.Return(list);
				lifeRecordCollection.AddMixPoisonRottenGloomyCold(character._id, currDate, character._location);
				break;
			}
			case 27:
				character.ChangeDisorderOfQiRandomRecovery(context, (short)Math.Clamp(num * 500, DisorderLevelOfQi.MinValue, DisorderLevelOfQi.MaxValue));
				lifeRecordCollection.AddMixPoisonHotGloomyCold(character._id, currDate, character._location);
				break;
			case 28:
				character.MakeExistingInjuriesWorse(context, isInnerInjury: true, (sbyte)(num / 3));
				lifeRecordCollection.AddMixPoisonRedGloomyCold(character._id, currDate, character._location);
				break;
			case 17:
			{
				List<(ItemBase, int)> obj2 = context.AdvanceMonthRelatedData.ItemsWithAmount.Occupy();
				CharacterDomain.GetLostItemsByAmount(context, num, character._inventory.Items, obj2, character._id == taiwu._id);
				Location validLocation = character.GetValidLocation();
				List<MapBlockData> list2 = ObjectPool<List<MapBlockData>>.Instance.Get();
				DomainManager.Map.GetRealNeighborBlocks(validLocation.AreaId, validLocation.BlockId, list2, 2, includeCenter: true);
				if (character._id == taiwu._id)
				{
					foreach (var item4 in obj2)
					{
						ItemBase item = item4.Item1;
						monthlyNotificationCollection.AddPoisonMakeLoss(taiwu._id, validLocation, item.GetItemType(), item.GetTemplateId());
					}
				}
				int num2 = 0;
				foreach (var item5 in obj2)
				{
					ItemBase item2 = item5.Item1;
					int item3 = item5.Item2;
					ItemKey itemKey = item2.GetItemKey();
					character.RemoveInventoryItem(context, itemKey, item3, deleteItem: false);
					lifeRecordCollection.AddMixPoisonHotRottenGloomy(character._id, currDate, validLocation, itemKey.ItemType, itemKey.TemplateId);
					MapBlockData random2 = list2.GetRandom(context.Random);
					DomainManager.Map.AddBlockItem(context, random2, itemKey, item3);
					num2 -= item2.GetHappinessChange() * item3;
				}
				context.AdvanceMonthRelatedData.ItemsWithAmount.Release(ref obj2);
				ObjectPool<List<MapBlockData>>.Instance.Return(list2);
				character.ChangeHappiness(context, num2);
				break;
			}
			case 21:
			{
				Location location2 = character._location;
				if (!location2.IsValid())
				{
					location2 = character.GetValidLocation();
				}
				MapBlockData block2 = DomainManager.Map.GetBlock(location2);
				if (block2.CharacterSet != null)
				{
					foreach (int item6 in block2.CharacterSet)
					{
						if (item6 != character._id)
						{
							Character element_Objects3 = DomainManager.Character.GetElement_Objects(item6);
							element_Objects3.ChangePoisoned(context, 4, 3, 100 + num * 20);
						}
					}
				}
				if (location2.Equals(taiwu._location))
				{
					HashSet<int> collection2 = DomainManager.Taiwu.GetGroupCharIds().GetCollection();
					foreach (int item7 in collection2)
					{
						if (item7 != character._id)
						{
							Character element_Objects4 = DomainManager.Character.GetElement_Objects(item7);
							element_Objects4.ChangePoisoned(context, 4, 3, 100 + num * 20);
						}
					}
				}
				if (character._id == taiwu._id)
				{
					monthlyNotificationCollection.AddRottenPoisonDiffuse(character._id, location2);
				}
				lifeRecordCollection.AddMixPoisonRedRottenCold(character._id, currDate, location2);
				break;
			}
			case 22:
			{
				if (character.GetAgeGroup() == 0)
				{
					break;
				}
				List<int> obj = context.AdvanceMonthRelatedData.CharIdList.Occupy();
				character.GetPotentialHarmfulActionTargets(obj);
				lifeRecordCollection.AddMixPoisonHotRedIllusory(character._id, currDate, character._location);
				character.ActivateAdvanceMonthStatus(7);
				for (int i = 0; i < num; i++)
				{
					Character element;
					while (true)
					{
						if (obj.Count == 0)
						{
							context.AdvanceMonthRelatedData.CharIdList.Release(ref obj);
							return;
						}
						int index = context.Random.Next(obj.Count);
						int objectId = obj[index];
						if (DomainManager.Character.TryGetElement_Objects(objectId, out element) && element._location.IsValid())
						{
							break;
						}
						CollectionUtils.SwapAndRemove(obj, index);
						bool flag = true;
					}
					character.PerformHarmfulActionToTarget(context, element);
					if (!DomainManager.Character.IsCharacterAlive(character._id) || !character._location.IsValid())
					{
						break;
					}
				}
				context.AdvanceMonthRelatedData.CharIdList.Release(ref obj);
				break;
			}
			case 23:
				if (character.GetAgeGroup() != 0)
				{
					DomainManager.Extra.TryAddCharacterAvatarSnapshot(context, character);
					character._avatar.AdjustToBaseCharm(context.Random, (short)Math.Max(0, character._avatar.BaseCharm - num * 50));
					character.SetAvatar(character._avatar, context);
					if (character._id == taiwu._id)
					{
						monthlyNotificationCollection.AddPoisonDestroyFace(character._id);
					}
					lifeRecordCollection.AddMixPoisonHotRedGloomy(character._id, currDate, character._location);
				}
				break;
			case 31:
			{
				Location location = character._location;
				if (!location.IsValid())
				{
					location = character.GetValidLocation();
				}
				MapBlockData block = DomainManager.Map.GetBlock(location);
				if (block.CharacterSet != null)
				{
					foreach (int item8 in block.CharacterSet)
					{
						if (item8 != character._id)
						{
							Character element_Objects = DomainManager.Character.GetElement_Objects(item8);
							element_Objects.ChangePoisoned(context, 5, 3, 100 + num * 20);
						}
					}
				}
				if (location.Equals(taiwu._location))
				{
					HashSet<int> collection = DomainManager.Taiwu.GetGroupCharIds().GetCollection();
					foreach (int item9 in collection)
					{
						if (item9 != character._id)
						{
							Character element_Objects2 = DomainManager.Character.GetElement_Objects(item9);
							element_Objects2.ChangePoisoned(context, 5, 3, 100 + num * 20);
						}
					}
				}
				if (character._id == taiwu._id)
				{
					monthlyNotificationCollection.AddIllusoryPoisonDiffuse(character._id, location);
				}
				lifeRecordCollection.AddMixPoisonRedColdIllusory(character._id, currDate, location);
				break;
			}
			}
		}
	}

	public void GetPotentialHarmfulActionTargets(List<int> charIdList)
	{
		charIdList.Clear();
		if (_location.IsValid())
		{
			MapBlockData block = DomainManager.Map.GetBlock(_location);
			if (block.CharacterSet != null)
			{
				GetPotentialHarmfulActionTargetsInSet(block.CharacterSet, charIdList);
			}
			if (_location.Equals(DomainManager.Taiwu.GetTaiwu()._location))
			{
				HashSet<int> collection = DomainManager.Taiwu.GetGroupCharIds().GetCollection();
				GetPotentialHarmfulActionTargetsInSet(collection, charIdList);
			}
		}
	}

	private void GetPotentialHarmfulActionTargetsInSet(HashSet<int> allCharSet, List<int> charIdList)
	{
		foreach (int item in allCharSet)
		{
			if (item != _id && DomainManager.Character.TryGetElement_Objects(item, out var element) && element.GetAgeGroup() != 0)
			{
				charIdList.Add(item);
			}
		}
	}

	private unsafe void PerformHarmfulActionToTarget(DataContext context, Character targetChar)
	{
		MonthlyNotificationCollection monthlyNotificationCollection = DomainManager.World.GetMonthlyNotificationCollection();
		int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
		IRandomSource random = context.Random;
		sbyte* ptr = stackalloc sbyte[4];
		for (sbyte b = 0; b < 4; b++)
		{
			ptr[b] = b;
		}
		CollectionUtils.Shuffle(context.Random, ptr, 4);
		for (int i = 0; i < 4; i++)
		{
			switch (ptr[i])
			{
			case 0:
			{
				bool flag2 = DomainManager.Character.HandleAttackAction(context, this, targetChar);
				if (_id == taiwuCharId)
				{
					if (flag2)
					{
						monthlyNotificationCollection.AddPoisonDisturbMindAttckSuccess(_id, _location, targetChar._id);
					}
					else
					{
						monthlyNotificationCollection.AddPoisonDisturbMindAttckFalse(_id, _location, targetChar._id);
					}
				}
				return;
			}
			case 1:
			{
				bool flag3 = DomainManager.Character.HandlePoisonAction(context, this, targetChar, ItemKey.Invalid, -1);
				if (_id == taiwuCharId)
				{
					if (flag3)
					{
						monthlyNotificationCollection.AddPoisonDisturbMindEmpoisonSuccess(_id, _location, targetChar._id);
					}
					else
					{
						monthlyNotificationCollection.AddPoisonDisturbMindEmpoisonFalse(_id, _location, targetChar._id);
					}
				}
				return;
			}
			case 2:
			{
				bool flag4 = DomainManager.Character.HandlePlotHarmAction(context, this, targetChar, ItemKey.Invalid, -1);
				if (_id == taiwuCharId)
				{
					if (flag4)
					{
						monthlyNotificationCollection.AddPoisonDisturbMindSneakAttckSuccess(_id, _location, targetChar._id);
					}
					else
					{
						monthlyNotificationCollection.AddPoisonDisturbMindSneakAttckFalse(_id, _location, targetChar._id);
					}
				}
				return;
			}
			case 3:
			{
				if (GetAgeGroup() != 2 || targetChar.GetAgeGroup() != 2)
				{
					break;
				}
				bool flag = DomainManager.Character.HandleRapeAction(context, this, targetChar);
				if (_id == taiwuCharId)
				{
					if (flag)
					{
						monthlyNotificationCollection.AddPoisonDisturbMindRapeSuccess(_id, _location, targetChar._id);
					}
					else
					{
						monthlyNotificationCollection.AddPoisonDisturbMindRapeFalse(_id, _location, targetChar._id);
					}
				}
				return;
			}
			}
		}
	}

	public bool IsOnRegularSettlement()
	{
		Location location = _location;
		if (!location.IsValid())
		{
			location = GetValidLocation();
		}
		if (location.AreaId >= 45)
		{
			return false;
		}
		MapBlockData blockData = DomainManager.Map.GetBlockData(location.AreaId, location.BlockId);
		return blockData.IsCityTown();
	}

	public bool IsInRegularSettlementRange()
	{
		Location location = _location;
		if (!location.IsValid())
		{
			location = GetValidLocation();
		}
		if (location.AreaId >= 45)
		{
			return false;
		}
		return DomainManager.Map.GetBelongSettlementBlock(location) != null;
	}

	public void AddTravelTarget(DataContext context, NpcTravelTarget target)
	{
		_npcTravelTargets.Add(target);
		SetNpcTravelTargets(_npcTravelTargets, context);
	}

	public void UpdateTravelTargetRemainingMonths(DataContext context)
	{
		for (int num = _npcTravelTargets.Count - 1; num >= 0; num--)
		{
			NpcTravelTarget npcTravelTarget = _npcTravelTargets[num];
			npcTravelTarget.RemainingMonth--;
			if (npcTravelTarget.RemainingMonth <= 0)
			{
				_npcTravelTargets.RemoveAt(num);
			}
		}
		SetNpcTravelTargets(_npcTravelTargets, context);
	}

	public void UpdateIntelligentCharacterMovement(DataContext context)
	{
		if (IsActiveExternalRelationState(60) || GetAgeGroup() == 0 || _kidnapperId >= 0 || (_leaderId >= 0 && _leaderId != _id) || (IsCompletelyInfected() && _location.IsValid()) || GetLegendaryBookOwnerState() >= 2)
		{
			return;
		}
		if (_location.IsValid())
		{
			HashSet<int> characterSet = DomainManager.Map.GetBlock(_location).CharacterSet;
			if (characterSet == null || !characterSet.Contains(_id))
			{
				return;
			}
		}
		if ((!IsActiveExternalRelationState(1) || !DomainManager.Taiwu.TryGetElement_VillagerWork(_id, out var value) || value.WorkType == 2) && !ContinueCurrCrossAreaTravel(context) && !TravelToPrioritizedActionTargetLocation(context) && !TravelToTargets(context))
		{
			TravelToRandomTarget(context);
		}
	}

	public void CompletelyInfectedCharacterMovement(DataContext context)
	{
		int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
		Location location = DomainManager.Taiwu.GetTaiwu().GetLocation();
		if (InfectedMoveCloserToLocationInSameArea(context, location))
		{
			return;
		}
		switch (GetBehaviorType())
		{
		case 0:
		case 1:
		{
			HashSet<int> obj3 = context.AdvanceMonthRelatedData.RelatedCharIds.Occupy();
			DomainManager.Character.GetRelatedCharacters(_id)?.GetAllPrioritizedCharIds(obj3);
			List<int>[] obj4 = context.AdvanceMonthRelatedData.PrioritizedTargets.Occupy();
			ClassifyPrioritizedActionTargets(obj4, obj3, context.Random);
			context.AdvanceMonthRelatedData.RelatedCharIds.Release(ref obj3);
			int num2 = SelectMaxPriorityActionTarget(obj4, delegate(int charId)
			{
				if (!DomainManager.Character.TryGetElement_Objects(charId, out var element))
				{
					return false;
				}
				if (element._location.IsValid())
				{
					return element._location.AreaId == _location.AreaId;
				}
				return element._leaderId == taiwuCharId && element.GetValidLocation().AreaId == _location.AreaId;
			});
			context.AdvanceMonthRelatedData.PrioritizedTargets.Release(ref obj4);
			if (num2 < 0)
			{
				InfectedMoveToRandomLocationInThreeBlocks(context, _location);
				break;
			}
			Character element_Objects2 = DomainManager.Character.GetElement_Objects(num2);
			Location moveAwayFrom = element_Objects2._location;
			if (!moveAwayFrom.IsValid())
			{
				moveAwayFrom = element_Objects2.GetValidLocation();
			}
			InfectedMoveAwayFromLocationInSameArea(context, moveAwayFrom);
			break;
		}
		case 2:
			InfectedMoveToRandomLocationInThreeBlocks(context, _location);
			break;
		case 3:
		case 4:
		{
			HashSet<int> obj = context.AdvanceMonthRelatedData.RelatedCharIds.Occupy();
			DomainManager.Character.GetRelatedCharacters(_id)?.GetAllPrioritizedCharIds(obj);
			List<int>[] obj2 = context.AdvanceMonthRelatedData.PrioritizedTargets.Occupy();
			ClassifyPrioritizedActionTargets(obj2, obj, context.Random);
			context.AdvanceMonthRelatedData.RelatedCharIds.Release(ref obj);
			int num = SelectMaxPriorityActionTarget(obj2, delegate(int charId)
			{
				if (!DomainManager.Character.TryGetElement_Objects(charId, out var element))
				{
					return false;
				}
				if (element._location.IsValid())
				{
					return element._location.AreaId == _location.AreaId;
				}
				return element._leaderId == taiwuCharId && element.GetValidLocation().AreaId == _location.AreaId;
			});
			context.AdvanceMonthRelatedData.PrioritizedTargets.Release(ref obj2);
			if (num < 0)
			{
				InfectedMoveToRandomLocationInThreeBlocks(context, _location);
				break;
			}
			Character element_Objects = DomainManager.Character.GetElement_Objects(num);
			Location destLocation = element_Objects._location;
			if (!destLocation.IsValid())
			{
				destLocation = element_Objects.GetValidLocation();
			}
			InfectedMoveCloserToLocationInSameArea(context, destLocation);
			break;
		}
		}
	}

	private bool InfectedMoveCloserToLocationInSameArea(DataContext context, Location destLocation)
	{
		if (_location.AreaId != destLocation.AreaId)
		{
			return false;
		}
		if (_location.BlockId == destLocation.BlockId)
		{
			return true;
		}
		byte areaSize = DomainManager.Map.GetAreaSize(_location.AreaId);
		ByteCoordinate byteCoordinate = ByteCoordinate.IndexToCoordinate(_location.BlockId, areaSize);
		ByteCoordinate byteCoordinate2 = ByteCoordinate.IndexToCoordinate(destLocation.BlockId, areaSize);
		if (byteCoordinate.GetManhattanDistance(byteCoordinate2) > 3)
		{
			InfectedMoveToRandomLocationInThreeBlocks(context, destLocation);
		}
		else
		{
			DomainManager.Character.GroupMove(context, this, destLocation);
		}
		return true;
	}

	private bool InfectedMoveAwayFromLocationInSameArea(DataContext context, Location moveAwayFrom)
	{
		if (_location.AreaId != moveAwayFrom.AreaId)
		{
			return false;
		}
		if (_location.BlockId == moveAwayFrom.BlockId && InfectedMoveToRandomLocationInThreeBlocks(context, moveAwayFrom))
		{
			return true;
		}
		byte areaSize = DomainManager.Map.GetAreaSize(_location.AreaId);
		ByteCoordinate byteCoordinate = ByteCoordinate.IndexToCoordinate(_location.BlockId, areaSize);
		ByteCoordinate byteCoordinate2 = ByteCoordinate.IndexToCoordinate(moveAwayFrom.BlockId, areaSize);
		if (byteCoordinate.GetManhattanDistance(byteCoordinate2) > 3)
		{
			InfectedMoveToRandomLocationInThreeBlocks(context, _location);
			return true;
		}
		Span<MapBlockData> areaBlocks = DomainManager.Map.GetAreaBlocks(_location.AreaId);
		int num = 0;
		Location targetLocation = Location.Invalid;
		for (int i = 0; i < 10; i++)
		{
			int index = context.Random.Next(areaBlocks.Length);
			MapBlockData mapBlockData = areaBlocks[index];
			Location location = new Location(mapBlockData.AreaId, mapBlockData.BlockId);
			MapBlockData block = DomainManager.Map.GetBlock(location);
			if (block.IsPassable())
			{
				ByteCoordinate byteCoordinate3 = ByteCoordinate.IndexToCoordinate(location.BlockId, areaSize);
				int manhattanDistance = byteCoordinate.GetManhattanDistance(byteCoordinate3);
				if (manhattanDistance > 3)
				{
					targetLocation = location;
					break;
				}
				if (manhattanDistance >= num)
				{
					num = manhattanDistance;
					targetLocation = location;
				}
			}
		}
		DomainManager.Character.GroupMove(context, this, targetLocation);
		return true;
	}

	private bool InfectedMoveToRandomLocationInThreeBlocks(DataContext context, Location srcLocation, bool includeCenter = false)
	{
		List<MapBlockData> obj = context.AdvanceMonthRelatedData.Blocks.Occupy();
		DomainManager.Adventure.GetValidBlocksForRandomEnemy(srcLocation.AreaId, srcLocation.BlockId, 3, onAdventureSite: true, onSettlement: false, nearTaiwu: true, obj);
		MapBlockData randomOrDefault = obj.GetRandomOrDefault(context.Random, null);
		context.AdvanceMonthRelatedData.Blocks.Release(ref obj);
		if (randomOrDefault == null)
		{
			return false;
		}
		Location targetLocation = new Location(randomOrDefault.AreaId, randomOrDefault.BlockId);
		DomainManager.Character.GroupMove(context, this, targetLocation);
		return true;
	}

	private bool ContinueCurrCrossAreaTravel(DataContext context)
	{
		if (!DomainManager.Character.TryGetElement_CrossAreaMoveInfos(_id, out var value))
		{
			return false;
		}
		if (_location.IsValid())
		{
			AdaptableLog.TagWarning($"Character {_id}", $"character is currently in valid location {_location} while cross area traveling.");
			return true;
		}
		if (DomainManager.Character.TryGetCharacterPrioritizedAction(_id, out var action))
		{
			Location realTargetLocation = action.Target.GetRealTargetLocation();
			List<short> areaList = value.Route.AreaList;
			if (areaList[areaList.Count - 1] != realTargetLocation.AreaId)
			{
				Location validLocation = GetValidLocation();
				Location validAndNotForbiddenByBeggarSkillLocation = GetValidAndNotForbiddenByBeggarSkillLocation(validLocation);
				if (!validAndNotForbiddenByBeggarSkillLocation.IsValid())
				{
					throw new Exception($"Character {this} cannot find a valid location near {validLocation}.");
				}
				DomainManager.Character.RemoveCrossAreaTravelInfo(context, _id);
				DomainManager.Character.GroupMove(context, this, validAndNotForbiddenByBeggarSkillLocation);
				return false;
			}
		}
		NpcCrossAreaTravel(context, value);
		return true;
	}

	public bool IsCrossAreaTraveling()
	{
		CrossAreaMoveInfo value;
		if (_leaderId >= 0)
		{
			return DomainManager.Character.TryGetElement_CrossAreaMoveInfos(_leaderId, out value);
		}
		return DomainManager.Character.TryGetElement_CrossAreaMoveInfos(_id, out value);
	}

	private bool TravelToPrioritizedActionTargetLocation(DataContext context)
	{
		if (!DomainManager.Character.TryGetCharacterPrioritizedAction(_id, out var action))
		{
			return false;
		}
		Location realTargetLocation = action.Target.GetRealTargetLocation();
		realTargetLocation = GetValidAndNotForbiddenByBeggarSkillLocation(realTargetLocation);
		if (!realTargetLocation.IsValid())
		{
			return false;
		}
		if (realTargetLocation.Equals(_location))
		{
			return true;
		}
		if (realTargetLocation.AreaId != _location.AreaId)
		{
			if (!DomainManager.Map.AllowCrossAreaTravel(_location.AreaId, realTargetLocation.AreaId))
			{
				return false;
			}
			CrossAreaMoveInfo crossAreaMoveInfo = DomainManager.Map.CalcAreaTravelRoute(this, _location.AreaId, _location.BlockId, realTargetLocation.AreaId);
			for (int i = 0; i < _npcTravelTargets.Count; i++)
			{
				NpcTravelTarget target = _npcTravelTargets[i];
				Location realTargetLocation2 = target.GetRealTargetLocation();
				realTargetLocation2 = GetValidAndNotForbiddenByBeggarSkillLocation(realTargetLocation2);
				if (!realTargetLocation2.IsValid() || !crossAreaMoveInfo.Route.AreaList.Contains(realTargetLocation2.AreaId))
				{
					continue;
				}
				if (realTargetLocation2.AreaId != _location.AreaId)
				{
					crossAreaMoveInfo.ToAreaId = realTargetLocation2.AreaId;
					if (!NpcCrossAreaTravel(context, crossAreaMoveInfo))
					{
						return true;
					}
				}
				Tester.Assert(realTargetLocation2.AreaId == _location.AreaId);
				DomainManager.Character.GroupMove(context, this, realTargetLocation2);
				_npcTravelTargets.RemoveAt(i);
				SetNpcTravelTargets(_npcTravelTargets, context);
				return true;
			}
			if (!NpcCrossAreaTravel(context, crossAreaMoveInfo))
			{
				return true;
			}
		}
		Tester.Assert(realTargetLocation.AreaId == _location.AreaId);
		DomainManager.Character.GroupMove(context, this, realTargetLocation);
		return true;
	}

	private bool TravelToTargets(DataContext context)
	{
		for (int i = 0; i < _npcTravelTargets.Count; i++)
		{
			NpcTravelTarget target = _npcTravelTargets[i];
			Location realTargetLocation = target.GetRealTargetLocation();
			realTargetLocation = GetValidAndNotForbiddenByBeggarSkillLocation(realTargetLocation);
			if (!realTargetLocation.IsValid())
			{
				continue;
			}
			if (realTargetLocation.AreaId != _location.AreaId)
			{
				if (!DomainManager.Map.AllowCrossAreaTravel(_location.AreaId, realTargetLocation.AreaId))
				{
					return false;
				}
				CrossAreaMoveInfo travelInfo = DomainManager.Map.CalcAreaTravelRoute(this, _location.AreaId, _location.BlockId, realTargetLocation.AreaId);
				if (!NpcCrossAreaTravel(context, travelInfo))
				{
					return true;
				}
			}
			Tester.Assert(realTargetLocation.AreaId == _location.AreaId);
			DomainManager.Character.GroupMove(context, this, realTargetLocation);
			_npcTravelTargets.RemoveAt(i);
			SetNpcTravelTargets(_npcTravelTargets, context);
			return true;
		}
		return false;
	}

	private void TravelToRandomTarget(DataContext context)
	{
		if (!context.Random.CheckPercentProb(30))
		{
			return;
		}
		Location location = OfflineCalcRandomMovementTarget(context);
		location = GetValidAndNotForbiddenByBeggarSkillLocation(location);
		if (!location.IsValid())
		{
			return;
		}
		if (location.AreaId != _location.AreaId)
		{
			if (!DomainManager.Map.AllowCrossAreaTravel(_location.AreaId, location.AreaId))
			{
				return;
			}
			CrossAreaMoveInfo travelInfo = DomainManager.Map.CalcAreaTravelRoute(this, _location.AreaId, _location.BlockId, location.AreaId);
			if (!NpcCrossAreaTravel(context, travelInfo))
			{
				return;
			}
		}
		Tester.Assert(location.AreaId == _location.AreaId);
		DomainManager.Character.GroupMove(context, this, location);
	}

	public bool NpcCrossAreaTravel(DataContext context, CrossAreaMoveInfo travelInfo)
	{
		travelInfo.CostedDays += 30;
		int num = 0;
		int num2 = 0;
		while (num2 < travelInfo.Route.CostList.Count)
		{
			short num3 = travelInfo.Route.AreaList[num2];
			num += travelInfo.Route.CostList[num2];
			if (num3 != travelInfo.ToAreaId)
			{
				List<short> areaList = travelInfo.Route.AreaList;
				if (num3 != areaList[areaList.Count - 1])
				{
					if (num >= travelInfo.CostedDays)
					{
						break;
					}
					num2++;
					continue;
				}
			}
			short stationBlockId = DomainManager.Map.GetElement_Areas(num3).StationBlockId;
			Location location = new Location(num3, stationBlockId);
			Location validAndNotForbiddenByBeggarSkillLocation = GetValidAndNotForbiddenByBeggarSkillLocation(location);
			if (!validAndNotForbiddenByBeggarSkillLocation.IsValid())
			{
				throw new Exception($"Character {this} cannot find a valid location near {location}.");
			}
			DomainManager.Character.RemoveCrossAreaTravelInfo(context, _id);
			DomainManager.Character.GroupMove(context, this, validAndNotForbiddenByBeggarSkillLocation);
			return true;
		}
		if (_location.IsValid())
		{
			DomainManager.Character.GroupMove(context, this, Location.Invalid);
		}
		DomainManager.Character.SetCrossAreaTravelInfo(context, _id, travelInfo);
		return false;
	}

	public unsafe Location OfflineCalcRandomMovementTarget(DataContext context)
	{
		Location location = _location;
		OrganizationMemberItem orgMemberConfig = OrganizationDomain.GetOrgMemberConfig(_organizationInfo);
		if (orgMemberConfig.CanStroll)
		{
			foreach (GameData.Domains.Character.Ai.PersonalNeed personalNeed in _personalNeeds)
			{
				if (personalNeed.TemplateId != 8 || personalNeed.ResourceType >= 6)
				{
					continue;
				}
				MapBlockData block = DomainManager.Map.GetBlock(location);
				short num = block.CurrResources.Items[personalNeed.ResourceType];
				if (num >= block.MaxResources.Items[personalNeed.ResourceType] / 2)
				{
					continue;
				}
				List<MapBlockData> list = ObjectPool<List<MapBlockData>>.Instance.Get();
				DomainManager.Map.GetRealNeighborBlocks(location.AreaId, location.BlockId, list, 3);
				Location result = location;
				foreach (MapBlockData item in list)
				{
					if (item.CurrResources.Items[personalNeed.ResourceType] < item.MaxResources.Items[personalNeed.ResourceType] / 2)
					{
						continue;
					}
					result = new Location(item.AreaId, item.BlockId);
					break;
				}
				ObjectPool<List<MapBlockData>>.Instance.Return(list);
				return result;
			}
		}
		if (_organizationInfo.SettlementId >= 0)
		{
			Settlement settlement = DomainManager.Organization.GetSettlement(_organizationInfo.SettlementId);
			Location location2 = settlement.GetLocation();
			List<short> obj = context.AdvanceMonthRelatedData.BlockIds.Occupy();
			DomainManager.Map.GetSettlementBlocksAndAffiliatedBlocks(location2.AreaId, location2.BlockId, obj);
			if (location.AreaId != location2.AreaId || !obj.Contains(location.BlockId))
			{
				context.AdvanceMonthRelatedData.BlockIds.Release(ref obj);
				return location2;
			}
			if (orgMemberConfig.CanStroll)
			{
				short random = obj.GetRandom(context.Random);
				context.AdvanceMonthRelatedData.BlockIds.Release(ref obj);
				return new Location(location2.AreaId, random);
			}
			obj.Clear();
			DomainManager.Map.GetSettlementBlocks(location2.AreaId, location2.BlockId, obj);
			short random2 = obj.GetRandom(context.Random);
			context.AdvanceMonthRelatedData.BlockIds.Release(ref obj);
			return new Location(location2.AreaId, random2);
		}
		return location;
	}

	private void OfflineAddNpcTravelTarget(NpcTravelTarget target)
	{
		for (int i = 0; i < _npcTravelTargets.Count; i++)
		{
			if (target.IsSameTargetWith(_npcTravelTargets[i]))
			{
				_npcTravelTargets[i] = target;
				return;
			}
		}
		_npcTravelTargets.Add(target);
	}

	private Location GetValidAndNotForbiddenByBeggarSkillLocation(Location location)
	{
		if (!location.IsValid())
		{
			return location;
		}
		if (!ProfessionSkillHandle.IsLocationForbiddenByBeggarSkill(location))
		{
			return location;
		}
		int maxSteps = 2;
		List<MapBlockData> list = ObjectPool<List<MapBlockData>>.Instance.Get();
		DomainManager.Map.GetRealNeighborBlocks(location.AreaId, location.BlockId, list, maxSteps);
		byte areaSize = DomainManager.Map.GetAreaSize(location.AreaId);
		ByteCoordinate centerCoordinate = ByteCoordinate.IndexToCoordinate(location.BlockId, areaSize);
		list.Sort(delegate(MapBlockData a, MapBlockData b)
		{
			byte areaSize2 = DomainManager.Map.GetAreaSize(a.AreaId);
			ByteCoordinate byteCoordinate = ByteCoordinate.IndexToCoordinate(a.BlockId, areaSize2);
			byte areaSize3 = DomainManager.Map.GetAreaSize(b.AreaId);
			ByteCoordinate byteCoordinate2 = ByteCoordinate.IndexToCoordinate(b.BlockId, areaSize3);
			int manhattanDistance = centerCoordinate.GetManhattanDistance(byteCoordinate);
			int manhattanDistance2 = centerCoordinate.GetManhattanDistance(byteCoordinate2);
			return manhattanDistance.CompareTo(manhattanDistance2);
		});
		Location result = Location.Invalid;
		foreach (MapBlockData item in list)
		{
			Location location2 = item.GetLocation();
			if (!location2.IsValid() || ProfessionSkillHandle.IsLocationForbiddenByBeggarSkill(location2))
			{
				continue;
			}
			result = location2;
			break;
		}
		ObjectPool<List<MapBlockData>>.Instance.Return(list);
		return result;
	}

	public void PeriAdvanceMonth_ExecutePrioritizedAction(DataContext context)
	{
		if (GetAgeGroup() == 0 || IsActiveAdvanceMonthStatus(2) || IsActiveExternalRelationState(60) || (IsActiveExternalRelationState(1) && DomainManager.Taiwu.TryGetElement_VillagerWork(_id, out var value) && value.WorkType != 2) || _kidnapperId >= 0 || DomainManager.LegendaryBook.IsCharacterActingCrazy(this))
		{
			return;
		}
		sbyte behaviorType = GetBehaviorType();
		BasePrioritizedAction action;
		bool flag = DomainManager.Character.TryGetCharacterPrioritizedAction(_id, out action);
		int num = ((!flag) ? (-1) : (PrioritizedActions.Instance[action.ActionType].BasePriority + PrioritizedActions.Instance[action.ActionType].MoralityPriority[behaviorType]));
		PrioritizedActionModification prioritizedActionModification = new PrioritizedActionModification(this)
		{
			Action = (flag ? action : null),
			IsNewAction = false
		};
		PrioritizedActionConditions generalConditions = new PrioritizedActionConditions
		{
			IsAdult = (GetAgeGroup() == 2),
			IsLeader = (_leaderId < 0 || _id == _leaderId),
			IsTaiwuTeammate = (_leaderId == DomainManager.Taiwu.GetTaiwuCharId()),
			IsAllowMarriage = OrgAndMonkTypeAllowMarriage(),
			CanStroll = OrganizationDomain.GetOrgMemberConfig(_organizationInfo).CanStroll,
			LoafDice = context.Random.Next(0, 100),
			OrgTemplateId = _organizationInfo.OrgTemplateId,
			OrgGrade = _organizationInfo.Grade
		};
		List<int>[] obj = context.AdvanceMonthRelatedData.PrioritizedTargets.Occupy();
		HashSet<int> obj2 = context.AdvanceMonthRelatedData.RelatedCharIds.Occupy();
		DomainManager.Character.GetRelatedCharacters(_id)?.GetAllPrioritizedCharIds(obj2);
		ClassifyPrioritizedActionTargets(obj, obj2, context.Random);
		context.AdvanceMonthRelatedData.RelatedCharIds.Release(ref obj2);
		for (short num2 = 0; num2 < PrioritizedActions.Instance.Count; num2++)
		{
			PrioritizedActionsItem prioritizedActionsItem = PrioritizedActions.Instance[num2];
			int num3 = prioritizedActionsItem.BasePriority + prioritizedActionsItem.MoralityPriority[behaviorType];
			if (num3 > num && (!flag || prioritizedActionsItem.IsPrevActionInterrupted) && !DomainManager.Extra.IsPrioritizedActionInCooldown(_id, num2))
			{
				BasePrioritizedAction basePrioritizedAction = PrioritizedActionTypeHelper.TryCreatePrioritizedAction(context, this, num2, ref generalConditions);
				if (basePrioritizedAction == null)
				{
					if (prioritizedActionsItem.FailToCreateActionCoolDown >= 0)
					{
						PrioritizedActionModification prioritizedActionModification2 = prioritizedActionModification;
						if (prioritizedActionModification2.FailToCreateActions == null)
						{
							prioritizedActionModification2.FailToCreateActions = new List<short>();
						}
						prioritizedActionModification.FailToCreateActions.Add(num2);
					}
				}
				else
				{
					prioritizedActionModification.Action = basePrioritizedAction;
					prioritizedActionModification.IsNewAction = true;
					num = num3;
				}
			}
		}
		context.AdvanceMonthRelatedData.PrioritizedTargets.Release(ref obj);
		if (prioritizedActionModification.Action is HuntTaiwuAction)
		{
			DomainManager.Taiwu.JieqingHuntTaiwu = true;
		}
		if (prioritizedActionModification.Action != null || prioritizedActionModification.FailToCreateActions != null)
		{
			ParallelModificationsRecorder parallelModificationsRecorder = context.ParallelModificationsRecorder;
			parallelModificationsRecorder.RecordType(ParallelModificationType.PeriAdvanceMonthExecutePrioritizedAction);
			parallelModificationsRecorder.RecordParameterClass(prioritizedActionModification);
		}
	}

	public static void ComplementPeriAdvanceMonth_ExecutePrioritizedAction(DataContext context, PrioritizedActionModification mod)
	{
		Character character = mod.Character;
		int id = character.GetId();
		BasePrioritizedAction action = mod.Action;
		if (!DomainManager.Character.IsCharacterAlive(id) || character.GetKidnapperId() >= 0)
		{
			return;
		}
		DomainManager.Extra.UpdatePrioritizedActionCooldown(context, id);
		if (mod.FailToCreateActions != null)
		{
			foreach (short failToCreateAction in mod.FailToCreateActions)
			{
				character.ResetPrioritizedActionCooldown(context, failToCreateAction, isFailToCreate: true);
			}
		}
		if (mod.Action == null)
		{
			return;
		}
		bool flag = action.CheckValid(character);
		if (mod.IsNewAction)
		{
			if (!flag)
			{
				return;
			}
			if (DomainManager.Character.TryGetCharacterPrioritizedAction(id, out var action2))
			{
				action2.OnInterrupt(context, character);
				DomainManager.Character.RemoveCharacterPrioritizedAction(context, id);
				character.ResetPrioritizedActionCooldown(context, mod.Action.ActionType, isFailToCreate: false);
			}
			DomainManager.Character.AddCharacterPrioritizedAction(context, id, mod.Action);
			action.OnStart(context, character);
		}
		else
		{
			action.Target.RemainingMonth--;
		}
		if (!DomainManager.Character.TryGetCharacterPrioritizedAction(id, out var _))
		{
			return;
		}
		if (flag)
		{
			if (mod.IsNewAction || !character._location.IsValid() || !action.Target.IsTargetInteractable() || !character._location.Equals(action.Target.GetRealTargetLocation()))
			{
				DomainManager.Character.SetCharacterPrioritizedAction(context, id);
				return;
			}
			if (!action.HasArrived)
			{
				action.OnArrival(context, character);
				if (!DomainManager.Character.IsCharacterAlive(id))
				{
					return;
				}
			}
			if (action.Execute(context, character))
			{
				if (DomainManager.Character.IsCharacterAlive(id))
				{
					DomainManager.Character.RemoveCharacterPrioritizedAction(context, id);
					character.ResetPrioritizedActionCooldown(context, mod.Action.ActionType, isFailToCreate: false);
				}
			}
			else
			{
				DomainManager.Character.SetCharacterPrioritizedAction(context, id);
			}
		}
		else
		{
			action.OnInterrupt(context, character);
			if (DomainManager.Character.IsCharacterAlive(id))
			{
				DomainManager.Character.RemoveCharacterPrioritizedAction(context, id);
				character.ResetPrioritizedActionCooldown(context, mod.Action.ActionType, isFailToCreate: false);
			}
		}
	}

	public void ResetPrioritizedActionCooldown(DataContext context, short actionType, bool isFailToCreate)
	{
		PrioritizedActionsItem prioritizedActionsItem = PrioritizedActions.Instance[actionType];
		short num = (isFailToCreate ? prioritizedActionsItem.FailToCreateActionCoolDown : prioritizedActionsItem.ActionCoolDown);
		if (num > 0)
		{
			DomainManager.Extra.SetPrioritizedActionCooldown(context, _id, actionType, num);
		}
	}

	public Location CalcFurthestEscapeDestination(IRandomSource random)
	{
		short areaId = GetValidLocation().AreaId;
		int num = -1;
		short num2 = -1;
		for (short num3 = 0; num3 < 135; num3++)
		{
			if (num3 != areaId)
			{
				int totalTimeCost = DomainManager.Map.GetTotalTimeCost(this, areaId, num3);
				if (totalTimeCost > num)
				{
					num = totalTimeCost;
					num2 = num3;
				}
			}
		}
		MapAreaData element_Areas = DomainManager.Map.GetElement_Areas(num2);
		short blockId = element_Areas.StationBlockId;
		if (num2 < 45)
		{
			Span<short> span = stackalloc short[element_Areas.SettlementInfos.Length];
			SpanList<short> spanList = span;
			SettlementInfo[] settlementInfos = element_Areas.SettlementInfos;
			for (int i = 0; i < settlementInfos.Length; i++)
			{
				SettlementInfo settlementInfo = settlementInfos[i];
				if (settlementInfo.SettlementId >= 0)
				{
					spanList.Add(settlementInfo.BlockId);
				}
			}
			blockId = spanList.GetRandom(random);
		}
		return new Location(num2, blockId);
	}

	private unsafe void OfflineUseResourcesForHealingAndDetox(PeriAdvanceMonthUpdateStatusModification mod)
	{
		CombatResources usableCombatResources = DomainManager.Character.GetUsableCombatResources(_id);
		if (usableCombatResources.HealingCount > 0)
		{
			for (int i = 0; i < usableCombatResources.HealingCount; i++)
			{
				if (!_injuries.HasAnyInjury())
				{
					break;
				}
				int healInjuryCostHerb = CombatDomain.GetHealInjuryCostHerb(_injuries);
				if (OfflineCheckHerbLackAndAddPersonalNeed(mod, healInjuryCostHerb))
				{
					break;
				}
				_injuries = DomainManager.Combat.HealInjury(_id, this);
				ref int reference = ref _resources.Items[5];
				reference -= healInjuryCostHerb;
				mod.InjuriesChanged = true;
				mod.ResourcesChanged = true;
				mod.UsedHealingCount++;
			}
		}
		if (usableCombatResources.DetoxCount > 0)
		{
			for (int j = 0; j < usableCombatResources.DetoxCount; j++)
			{
				if (!_poisoned.IsNonZero())
				{
					break;
				}
				int healPoisonCostHerb = CombatDomain.GetHealPoisonCostHerb(_poisoned);
				if (OfflineCheckHerbLackAndAddPersonalNeed(mod, healPoisonCostHerb))
				{
					break;
				}
				_poisoned = DomainManager.Combat.HealPoison(_id, this);
				ref int reference2 = ref _resources.Items[5];
				reference2 -= healPoisonCostHerb;
				mod.PoisonedChanged = true;
				mod.ResourcesChanged = true;
				mod.UsedDetoxCount++;
			}
		}
		if (usableCombatResources.BreathingCount <= 0)
		{
			return;
		}
		for (int k = 0; k < usableCombatResources.BreathingCount; k++)
		{
			if (_disorderOfQi <= 0)
			{
				break;
			}
			int healQiDisorderCostHerb = CombatDomain.GetHealQiDisorderCostHerb(_disorderOfQi);
			if (OfflineCheckHerbLackAndAddPersonalNeed(mod, healQiDisorderCostHerb))
			{
				break;
			}
			_disorderOfQi = DomainManager.Combat.HealQiDisorder(_id, this);
			ref int reference3 = ref _resources.Items[5];
			reference3 -= healQiDisorderCostHerb;
			mod.QiDisorderChanged = true;
			mod.ResourcesChanged = true;
			mod.UsedBreathingCount++;
		}
	}

	private unsafe void OfflineUseResourcesForHealth(PeriAdvanceMonthUpdateStatusModification mod)
	{
		CombatResources usableCombatResources = DomainManager.Character.GetUsableCombatResources(_id);
		if (usableCombatResources.RecoverCount <= 0)
		{
			return;
		}
		short leftMaxHealth = GetLeftMaxHealth();
		for (int i = 0; i < usableCombatResources.RecoverCount; i++)
		{
			if (_health == leftMaxHealth)
			{
				break;
			}
			EHealthType healthType = HealthTypeHelper.CalcType(_featureIds, _health, leftMaxHealth);
			int healHealthCostHerb = CombatDomain.GetHealHealthCostHerb(healthType);
			if (OfflineCheckHerbLackAndAddPersonalNeed(mod, healHealthCostHerb))
			{
				break;
			}
			_health = DomainManager.Combat.HealHealth(_id, this);
			ref int reference = ref _resources.Items[5];
			reference -= healHealthCostHerb;
			mod.ResourcesChanged = true;
			mod.UsedRecoverCount++;
		}
	}

	private unsafe bool OfflineCheckHerbLackAndAddPersonalNeed(PeriAdvanceMonthUpdateStatusModification mod, int costHerb)
	{
		if (_resources.Items[5] >= costHerb)
		{
			return false;
		}
		GameData.Domains.Character.Ai.PersonalNeed personalNeed = GameData.Domains.Character.Ai.PersonalNeed.CreatePersonalNeed(8, 5, costHerb);
		OfflineAddPersonalNeed(personalNeed);
		mod.PersonalNeedsChanged = true;
		return true;
	}

	private unsafe void OfflineUseMedicineForHealingAndDetox(DataContext context, PeriAdvanceMonthUpdateStatusModification mod)
	{
		sbyte currMaxEatingSlotsCount = GetCurrMaxEatingSlotsCount();
		int num = _eatingItems.GetAvailableEatingSlotsCount(currMaxEatingSlotsCount);
		PoisonInts poisonResists = GetPoisonResists();
		List<(GameData.Domains.Item.Medicine, int)>[] array = context.AdvanceMonthRelatedData.CategorizedMedicines.Get();
		List<(GameData.Domains.Item.Medicine, int)> list = array[0];
		List<(GameData.Domains.Item.Medicine, int)> list2 = array[1];
		list.Sort(EatingItemComparer.MedicineInjury);
		list2.Sort(EatingItemComparer.MedicineInjury);
		for (sbyte b = 0; b < 7; b++)
		{
			var (b2, b3) = _injuries.Get(b);
			if (b2 > 2 && list.Count > 0)
			{
				int num2 = SelectTopicalMedicineIndex(list, b2, ref _currMainAttributes);
				if (num2 >= 0)
				{
					(GameData.Domains.Item.Medicine, int) tuple2 = list[num2];
					GameData.Domains.Item.Medicine item = tuple2.Item1;
					int item2 = tuple2.Item2;
					MedicineItem config = Config.Medicine.Instance[item.GetTemplateId()];
					OfflineApplyTopicalMedicineInternal(context.Random, config);
					mod.InjuriesChanged = true;
					mod.CurrMainAttributesChanged = true;
					OfflineRemoveUsedMedicine(list, num2, -1, mod);
				}
			}
			if (b3 > 2 && list2.Count > 0)
			{
				int num3 = SelectTopicalMedicineIndex(list2, b3, ref _currMainAttributes);
				if (num3 >= 0)
				{
					(GameData.Domains.Item.Medicine, int) tuple3 = list2[num3];
					GameData.Domains.Item.Medicine item3 = tuple3.Item1;
					int item4 = tuple3.Item2;
					MedicineItem config2 = Config.Medicine.Instance[item3.GetTemplateId()];
					OfflineApplyTopicalMedicineInternal(context.Random, config2);
					mod.InjuriesChanged = true;
					mod.CurrMainAttributesChanged = true;
					OfflineRemoveUsedMedicine(list2, num3, -1, mod);
				}
			}
		}
		array[3].Sort(EatingItemComparer.MedicineQiDisorder);
		int* ptr = stackalloc int[3];
		while (num > 1)
		{
			*ptr = GetHealthChangeDueToInjuries(ref _injuries) << 8;
			ptr[1] = (GetHealthChangeDueToPoisons(ref _poisoned, ref poisonResists) << 8) + 1;
			ptr[2] = (GetHealthChangeDueToDisorderOfQi(_disorderOfQi) << 8) + 2;
			CollectionUtils.Sort(ptr, 3);
			if (*ptr >> 8 >= 0 || !OfflineUseMedicineForHealingAndDetoxHelper(context, ptr, ref poisonResists, mod))
			{
				break;
			}
			num--;
		}
		OfflineCheckPersonalNeedForHealingAndDetox(mod);
	}

	private unsafe bool OfflineUseMedicineForHealingAndDetoxHelper(DataContext context, int* healthChanges, ref PoisonInts poisonResists, PeriAdvanceMonthUpdateStatusModification mod)
	{
		sbyte currMaxEatingSlotsCount = GetCurrMaxEatingSlotsCount();
		List<(GameData.Domains.Item.Medicine, int)>[] array = context.AdvanceMonthRelatedData.CategorizedMedicines.Get();
		int* ptr = stackalloc int[6];
		for (int i = 0; i < 3; i++)
		{
			int num = healthChanges[i] & 0xFF;
			int num2 = healthChanges[i] >> 8;
			if (num2 >= 0)
			{
				break;
			}
			switch (num)
			{
			case 0:
			{
				List<(GameData.Domains.Item.Medicine, int)> list2 = array[0];
				List<(GameData.Domains.Item.Medicine, int)> list3 = array[1];
				sbyte b = sbyte.MaxValue;
				sbyte b2 = sbyte.MaxValue;
				sbyte b3 = 0;
				sbyte b4 = 0;
				for (sbyte b5 = 0; b5 < 7; b5++)
				{
					(sbyte outer, sbyte inner) tuple2 = _injuries.Get(b5);
					sbyte item3 = tuple2.outer;
					sbyte item4 = tuple2.inner;
					b3 += item3;
					b4 += item4;
					if (item3 > 0 && item3 < b2)
					{
						b2 = item3;
					}
					if (item4 > 0 && item4 < b)
					{
						b = item4;
					}
				}
				int num4 = -1;
				if (list2.Count > 0 && b2 > b && b2 <= 6)
				{
					num4 = SelectMedicineIndexForInjury(list2, b2, b3);
					if (num4 >= 0)
					{
						(GameData.Domains.Item.Medicine, int) tuple3 = list2[num4];
						GameData.Domains.Item.Medicine item5 = tuple3.Item1;
						int item6 = tuple3.Item2;
						MedicineItem cfg = Config.Medicine.Instance[item5.GetTemplateId()];
						MedicineEatingInstantEffect config = new MedicineEatingInstantEffect(cfg);
						CalcMedicineEffect_RecoverInjury(ref _injuries, context.Random, inner: false, ref config);
						sbyte availableEatingSlot2 = _eatingItems.GetAvailableEatingSlot(currMaxEatingSlotsCount);
						OfflineRemoveUsedMedicine(list2, num4, availableEatingSlot2, mod);
						mod.InjuriesChanged = true;
						return true;
					}
				}
				if (list3.Count > 0 && b <= 6)
				{
					num4 = SelectMedicineIndexForInjury(list3, b, b4);
					if (num4 >= 0)
					{
						(GameData.Domains.Item.Medicine, int) tuple4 = list3[num4];
						GameData.Domains.Item.Medicine item7 = tuple4.Item1;
						int item8 = tuple4.Item2;
						MedicineItem cfg2 = Config.Medicine.Instance[item7.GetTemplateId()];
						MedicineEatingInstantEffect config2 = new MedicineEatingInstantEffect(cfg2);
						CalcMedicineEffect_RecoverInjury(ref _injuries, context.Random, inner: true, ref config2);
						sbyte availableEatingSlot3 = _eatingItems.GetAvailableEatingSlot(currMaxEatingSlotsCount);
						OfflineRemoveUsedMedicine(list3, num4, availableEatingSlot3, mod);
						mod.InjuriesChanged = true;
						return true;
					}
				}
				break;
			}
			case 1:
			{
				List<(GameData.Domains.Item.Medicine, int)> list4 = array[4];
				if (list4.Count <= 0)
				{
					break;
				}
				list4.Sort(CompareDetoxPoisonMedicines);
				for (sbyte b6 = 0; b6 < 6; b6++)
				{
					int num5 = ((poisonResists.Items[b6] < 1000) ? PoisonsAndLevels.CalcPoisonedLevel(_poisoned.Items[b6]) : 0);
					ptr[b6] = (num5 << 8) + b6;
				}
				CollectionUtils.Sort(ptr, 6);
				for (sbyte b7 = 5; b7 >= 0; b7--)
				{
					sbyte b8 = (sbyte)(ptr[b7] & 0xFF);
					sbyte b9 = (sbyte)(ptr[b7] >> 8);
					if (b9 <= 0)
					{
						break;
					}
					int num6 = SelectMedicineIndexForDetoxPoison(list4, b8, b9, _poisoned.Items[b8]);
					if (num6 >= 0)
					{
						(GameData.Domains.Item.Medicine, int) tuple5 = list4[num6];
						MedicineItem medicineItem = Config.Medicine.Instance[tuple5.Item1.GetTemplateId()];
						int num7 = -CalcMedicineEffectDelta(_poisoned.Items[b8], medicineItem.EffectValue, medicineItem.EffectIsPercentage);
						_poisoned.Items[b8] = Math.Max(_poisoned.Items[b8] + num7, 0);
						sbyte availableEatingSlot4 = _eatingItems.GetAvailableEatingSlot(currMaxEatingSlotsCount);
						OfflineRemoveUsedMedicine(list4, num6, availableEatingSlot4, mod);
						mod.PoisonedChanged = true;
						return true;
					}
				}
				break;
			}
			case 2:
			{
				List<(GameData.Domains.Item.Medicine, int)> list = array[3];
				int num3 = SelectMedicineIndexForQiDisorder(list, _disorderOfQi);
				if (num3 >= 0)
				{
					(GameData.Domains.Item.Medicine, int) tuple = list[num3];
					GameData.Domains.Item.Medicine item = tuple.Item1;
					int item2 = tuple.Item2;
					short effectValue = item.GetEffectValue();
					_disorderOfQi = (short)Math.Clamp(_disorderOfQi + effectValue, DisorderLevelOfQi.MinValue, DisorderLevelOfQi.MaxValue);
					sbyte availableEatingSlot = _eatingItems.GetAvailableEatingSlot(currMaxEatingSlotsCount);
					OfflineRemoveUsedMedicine(list, num3, availableEatingSlot, mod);
					mod.QiDisorderChanged = true;
					return true;
				}
				break;
			}
			}
		}
		return false;
	}

	public unsafe int CompareDetoxPoisonMedicines((GameData.Domains.Item.Medicine item, int amount) a, (GameData.Domains.Item.Medicine item, int amount) b)
	{
		short effectValue = a.item.GetEffectValue();
		short effectValue2 = b.item.GetEffectValue();
		sbyte b2 = a.item.GetEffectSubType().PoisonType();
		sbyte b3 = b.item.GetEffectSubType().PoisonType();
		int fullRangeValue = ((b2 >= 0) ? _poisoned.Items[b2] : 0);
		int value = ((b2 < 0) ? int.MinValue : CalcMedicineEffectDelta(fullRangeValue, effectValue, a.item.GetEffectSubType().IsPercentage()));
		int fullRangeValue2 = ((b3 < 0) ? (-1) : _poisoned.Items[b3]);
		return ((b2 < 0) ? int.MinValue : CalcMedicineEffectDelta(fullRangeValue2, effectValue2, b.item.GetEffectSubType().IsPercentage())).CompareTo(value);
	}

	private unsafe void OfflineCheckPersonalNeedForHealingAndDetox(PeriAdvanceMonthUpdateStatusModification mod)
	{
		int num = 0;
		int num2 = 0;
		bool flag = false;
		bool flag2 = false;
		for (sbyte b = 0; b < 7; b++)
		{
			(sbyte outer, sbyte inner) tuple = _injuries.Get(b);
			sbyte item = tuple.outer;
			sbyte item2 = tuple.inner;
			num += item;
			num2 += item2;
			if (item > 2)
			{
				flag = true;
			}
			if (item2 > 2)
			{
				flag2 = true;
			}
		}
		if (flag)
		{
			GameData.Domains.Character.Ai.PersonalNeed personalNeed = GameData.Domains.Character.Ai.PersonalNeed.CreatePersonalNeed(4, 0, num);
			OfflineAddPersonalNeed(personalNeed);
			mod.PersonalNeedsChanged = true;
		}
		if (flag2)
		{
			GameData.Domains.Character.Ai.PersonalNeed personalNeed2 = GameData.Domains.Character.Ai.PersonalNeed.CreatePersonalNeed(4, 1, num2);
			OfflineAddPersonalNeed(personalNeed2);
			mod.PersonalNeedsChanged = true;
		}
		for (sbyte b2 = 0; b2 < 6; b2++)
		{
			int num3 = _poisoned.Items[b2];
			sbyte b3 = PoisonsAndLevels.CalcPoisonedLevel(num3);
			if (b3 > 0)
			{
				GameData.Domains.Character.Ai.PersonalNeed personalNeed3 = GameData.Domains.Character.Ai.PersonalNeed.CreatePersonalNeed(5, b2, num3);
				OfflineAddPersonalNeed(personalNeed3);
				mod.PersonalNeedsChanged = true;
			}
		}
		if (GetHealthChangeDueToDisorderOfQi(_disorderOfQi) < 0)
		{
			GameData.Domains.Character.Ai.PersonalNeed personalNeed4 = GameData.Domains.Character.Ai.PersonalNeed.CreatePersonalNeed(2, _disorderOfQi);
			OfflineAddPersonalNeed(personalNeed4);
			mod.PersonalNeedsChanged = true;
		}
	}

	private void OfflineUseMedicineForHealth(DataContext context, PeriAdvanceMonthUpdateStatusModification mod)
	{
		short leftMaxHealth = GetLeftMaxHealth();
		if (_health >= leftMaxHealth / 2)
		{
			return;
		}
		sbyte currMaxEatingSlotsCount = GetCurrMaxEatingSlotsCount();
		sbyte availableEatingSlot = _eatingItems.GetAvailableEatingSlot(currMaxEatingSlotsCount);
		List<(GameData.Domains.Item.Medicine, int)>[] array = context.AdvanceMonthRelatedData.CategorizedMedicines.Get();
		List<(GameData.Domains.Item.Medicine, int)> list = array[2];
		if (availableEatingSlot >= 0 && list.Count > 0)
		{
			list.Sort(EatingItemComparer.MedicineEffect);
			while (availableEatingSlot >= 0 && _health < leftMaxHealth)
			{
				int num = SelectMedicineIndexForHealth(list, _health, leftMaxHealth);
				if (num < 0)
				{
					break;
				}
				short effectValue = list[num].Item1.GetEffectValue();
				_health = (short)Math.Min(_health + effectValue, leftMaxHealth);
				OfflineRemoveUsedMedicine(list, num, availableEatingSlot, mod);
				availableEatingSlot = _eatingItems.GetAvailableEatingSlot(currMaxEatingSlotsCount);
			}
		}
		if (_health < leftMaxHealth)
		{
			GameData.Domains.Character.Ai.PersonalNeed personalNeed = GameData.Domains.Character.Ai.PersonalNeed.CreatePersonalNeed(1, leftMaxHealth - _health);
			OfflineAddPersonalNeed(personalNeed);
			mod.PersonalNeedsChanged = true;
		}
	}

	private unsafe void OfflineUseMedicineForWug(DataContext context, PeriAdvanceMonthUpdateStatusModification mod)
	{
		sbyte currMaxEatingSlotsCount = GetCurrMaxEatingSlotsCount();
		sbyte availableEatingSlot = _eatingItems.GetAvailableEatingSlot(currMaxEatingSlotsCount);
		List<(GameData.Domains.Item.Medicine, int)>[] array = context.AdvanceMonthRelatedData.CategorizedMedicines.Get();
		List<(GameData.Domains.Item.Medicine, int)> list = array[5];
		List<(GameData.Domains.Item.Medicine, int)> list2 = array[6];
		if (availableEatingSlot >= 0 && (list.Count > 0 || list2.Count > 0))
		{
			list.Sort(EatingItemComparer.MedicineGrade);
			list2.Sort(EatingItemComparer.MedicineGrade);
			for (sbyte b = 0; b < 9; b++)
			{
				ItemKey itemKey = (ItemKey)_eatingItems.ItemKeys[b];
				if (EatingItems.IsWug(itemKey) && !itemKey.IsValid())
				{
					MedicineItem medicineItem = Config.Medicine.Instance[itemKey.TemplateId];
					bool flag = EatingItems.IsWug(_eatingItems.Get(b)) && !_eatingItems.Get(b).IsValid();
					while (availableEatingSlot >= 0 && flag)
					{
						int num = SelectMedicineIndexForWug(list, medicineItem.WugType, _eatingItems.Durations[b]);
						if (num >= 0)
						{
							short deltaWugDuration = GameData.Domains.Item.Medicine.GetDeltaWugDuration(list[num].Item1.GetGrade());
							_eatingItems.ChangeDuration(context, b, deltaWugDuration, ref mod.RemovedWugs);
							OfflineRemoveUsedMedicine(list, num, availableEatingSlot, mod);
							availableEatingSlot = _eatingItems.GetAvailableEatingSlot(currMaxEatingSlotsCount);
							continue;
						}
						num = SelectPoisonIndexForWug(list2, medicineItem.WugType, _eatingItems.Durations[b]);
						if (num >= 0)
						{
							MedicineItem medicineItem2 = Config.Medicine.Instance[list2[num].Item1.GetTemplateId()];
							short deltaWugDuration2 = GameData.Domains.Item.Medicine.GetDeltaWugDuration(medicineItem2.Grade);
							_eatingItems.ChangeDuration(context, b, deltaWugDuration2, ref mod.RemovedWugs);
							sbyte poisonType = medicineItem2.PoisonType;
							sbyte b2 = (sbyte)medicineItem2.EffectThresholdValue;
							if (!HasPoisonImmunity(poisonType))
							{
								int max = 25000;
								int num2 = _poisoned.Items[poisonType];
								_poisoned.Items[poisonType] = Math.Clamp(num2 + medicineItem2.EffectValue, 0, max);
								mod.PoisonedChanged = true;
							}
							OfflineRemoveUsedMedicine(list2, num, availableEatingSlot, mod);
							availableEatingSlot = _eatingItems.GetAvailableEatingSlot(currMaxEatingSlotsCount);
							continue;
						}
						break;
					}
				}
			}
		}
		for (sbyte b3 = 0; b3 < 9; b3++)
		{
			ItemKey itemKey2 = (ItemKey)_eatingItems.ItemKeys[b3];
			if (EatingItems.IsWug(itemKey2) && !itemKey2.IsValid())
			{
				MedicineItem medicineItem3 = Config.Medicine.Instance[itemKey2.TemplateId];
				if (EatingItems.IsWug(_eatingItems.Get(b3)) && !_eatingItems.Get(b3).IsValid())
				{
					GameData.Domains.Character.Ai.PersonalNeed personalNeed = GameData.Domains.Character.Ai.PersonalNeed.CreatePersonalNeedKillWug(medicineItem3.WugType);
					OfflineAddPersonalNeed(personalNeed);
					mod.PersonalNeedsChanged = true;
				}
			}
		}
	}

	private void OfflineUseItemForNeili(DataContext context, PeriAdvanceMonthUpdateStatusModification mod)
	{
		int maxNeili = GetMaxNeili();
		if (_currNeili >= maxNeili)
		{
			return;
		}
		List<(GameData.Domains.Item.Misc, int)> list = context.AdvanceMonthRelatedData.ItemsForNeili.Get();
		if (list.Count > 0)
		{
			list.Sort(EatingItemComparer.MiscNeili);
			while (_currNeili < maxNeili)
			{
				int num = SelectItemIndexForNeili(list, _currNeili, maxNeili);
				if (num < 0)
				{
					break;
				}
				(GameData.Domains.Item.Misc, int) tuple = list[num];
				GameData.Domains.Item.Misc item = tuple.Item1;
				int item2 = tuple.Item2;
				short neili = item.GetNeili();
				_currNeili = (short)Math.Min(_currNeili + neili, maxNeili);
				mod.CurrNeiliChanged = true;
				if (item2 > 1)
				{
					list[num] = (item, item2 - 1);
				}
				else
				{
					list.RemoveAt(num);
				}
				ItemKey itemKey = item.GetItemKey();
				_inventory.OfflineRemove(itemKey, 1);
				mod.InventoryChanged = true;
			}
		}
		if (_currNeili < maxNeili)
		{
			GameData.Domains.Character.Ai.PersonalNeed personalNeed = GameData.Domains.Character.Ai.PersonalNeed.CreatePersonalNeed(3, maxNeili - _currNeili);
			OfflineAddPersonalNeed(personalNeed);
			mod.PersonalNeedsChanged = true;
		}
	}

	private unsafe void OfflineUseFoodForMainAttributes(DataContext context, PeriAdvanceMonthUpdateStatusModification mod)
	{
		MainAttributes maxMainAttributes = GetMaxMainAttributes();
		sbyte currMaxEatingSlotsCount = GetCurrMaxEatingSlotsCount();
		sbyte availableEatingSlot = _eatingItems.GetAvailableEatingSlot(currMaxEatingSlotsCount);
		List<(GameData.Domains.Item.Food, int)>[] array = context.AdvanceMonthRelatedData.FoodsForMainAttributes.Get();
		for (sbyte b = 0; b < 6; b++)
		{
			short num = maxMainAttributes.Items[b];
			if (_currMainAttributes.Items[b] * 2 < num)
			{
				List<(GameData.Domains.Item.Food, int)> list = array[b];
				if (availableEatingSlot >= 0 && list.Count > 0)
				{
					sbyte behaviorType = GetBehaviorType();
					sbyte b2 = AiHelper.UpdateStatusConstants.EatForbiddenFoodChance[behaviorType];
					bool flag = IsForbiddenToEatMeat();
					short aiActionRateAdjust = DomainManager.Extra.GetAiActionRateAdjust(_id, 6, -1);
					bool allowMeat = !flag || context.Random.CheckPercentProb(b2 + aiActionRateAdjust);
					list.Sort(EatingItemComparer.FoodMainAttributes[b]);
					while (availableEatingSlot >= 0 && _currMainAttributes.Items[b] < num)
					{
						int num2 = SelectFoodIndexForMainAttributes(list, b, _currMainAttributes.Items[b], num, allowMeat);
						if (num2 < 0)
						{
							break;
						}
						FoodItem foodItem = Config.Food.Instance[list[num2].Item1.GetTemplateId()];
						short num3 = foodItem.MainAttributesRegen.Items[b];
						_currMainAttributes.Items[b] = (short)Math.Min(_currMainAttributes.Items[b] + num3, num);
						mod.CurrMainAttributesChanged = true;
						if (flag && foodItem.ItemSubType == 701)
						{
							ItemKey itemKey = list[num2].Item1.GetItemKey();
							if (mod.ConsumedForbiddenFoodsOrWines == null)
							{
								mod.ConsumedForbiddenFoodsOrWines = new List<ItemKey>();
							}
							if (!mod.ConsumedForbiddenFoodsOrWines.Contains(itemKey))
							{
								mod.ConsumedForbiddenFoodsOrWines.Add(itemKey);
							}
							sbyte b3 = AiHelper.UpdateStatusConstants.EatForbiddenFoodHappinessChange[behaviorType];
							_happiness = (sbyte)Math.Clamp(_happiness + b3, -119, 119);
							mod.HappinessChanged = true;
						}
						OfflineRemoveUsedFood(list, num2, availableEatingSlot, mod);
						availableEatingSlot = _eatingItems.GetAvailableEatingSlot(currMaxEatingSlotsCount);
					}
				}
				if (_currMainAttributes.Items[b] < maxMainAttributes.Items[b])
				{
					GameData.Domains.Character.Ai.PersonalNeed personalNeed = GameData.Domains.Character.Ai.PersonalNeed.CreatePersonalNeed(6, b, maxMainAttributes.Items[b] - _currMainAttributes.Items[b]);
					OfflineAddPersonalNeed(personalNeed);
					mod.PersonalNeedsChanged = true;
				}
			}
		}
	}

	private void OfflineUpdateHappiness(PeriAdvanceMonthUpdateStatusModification mod)
	{
		int num = DomainManager.SpecialEffect.ModifyValue(_id, 270, 0);
		if (num != 0)
		{
			sbyte happiness = _happiness;
			_happiness = (sbyte)Math.Clamp(_happiness + num, -119, 119);
			if (happiness != _happiness)
			{
				mod.HappinessChanged = true;
			}
		}
	}

	private void OfflineUseTeaWineForHappiness(DataContext context, PeriAdvanceMonthUpdateStatusModification mod)
	{
		sbyte item = HappinessType.Ranges[3].min;
		if (_happiness >= item)
		{
			return;
		}
		sbyte currMaxEatingSlotsCount = GetCurrMaxEatingSlotsCount();
		sbyte availableEatingSlot = _eatingItems.GetAvailableEatingSlot(currMaxEatingSlotsCount);
		List<(GameData.Domains.Item.TeaWine, int)> list = context.AdvanceMonthRelatedData.TeaWinesForHappiness.Get();
		if (availableEatingSlot >= 0 && list.Count > 0)
		{
			sbyte behaviorType = GetBehaviorType();
			short aiActionRateAdjust = DomainManager.Extra.GetAiActionRateAdjust(_id, 6, -1);
			sbyte b = AiHelper.UpdateStatusConstants.EatForbiddenFoodChance[behaviorType];
			bool flag = IsForbiddenToDrinkingWines();
			bool allowWines = !flag || context.Random.CheckPercentProb(b + aiActionRateAdjust);
			list.Sort(EatingItemComparer.TeaWineHappiness);
			while (availableEatingSlot >= 0 && _happiness < item)
			{
				int num = SelectTeaWineForHappiness(list, _happiness, item, allowWines);
				if (num < 0)
				{
					break;
				}
				TeaWineItem teaWineItem = Config.TeaWine.Instance[list[num].Item1.GetTemplateId()];
				_happiness = (sbyte)Math.Clamp(_happiness + teaWineItem.EatHappinessChange, -119, 119);
				mod.HappinessChanged = true;
				if (flag && teaWineItem.ItemSubType == 901)
				{
					ItemKey itemKey = list[num].Item1.GetItemKey();
					if (mod.ConsumedForbiddenFoodsOrWines == null)
					{
						mod.ConsumedForbiddenFoodsOrWines = new List<ItemKey>();
					}
					if (!mod.ConsumedForbiddenFoodsOrWines.Contains(itemKey))
					{
						mod.ConsumedForbiddenFoodsOrWines.Add(itemKey);
					}
					sbyte b2 = AiHelper.UpdateStatusConstants.EatForbiddenFoodHappinessChange[behaviorType];
					_happiness = (sbyte)Math.Clamp(_happiness + b2, -119, 119);
				}
				OfflineRemoveUsedTeaWine(list, num, availableEatingSlot, mod);
				availableEatingSlot = _eatingItems.GetAvailableEatingSlot(currMaxEatingSlotsCount);
			}
		}
		if (_happiness < item)
		{
			GameData.Domains.Character.Ai.PersonalNeed personalNeed = GameData.Domains.Character.Ai.PersonalNeed.CreatePersonalNeed(0, item - _happiness);
			OfflineAddPersonalNeed(personalNeed);
			mod.PersonalNeedsChanged = true;
		}
	}

	private void OfflineRemoveUsedMedicine(List<(GameData.Domains.Item.Medicine item, int amount)> medicines, int selectedIndex, sbyte eatingSlot, PeriAdvanceMonthUpdateStatusModification mod)
	{
		var (medicine, num) = medicines[selectedIndex];
		if (num > 1)
		{
			medicines[selectedIndex] = (medicine, num - 1);
		}
		else
		{
			medicines.RemoveAt(selectedIndex);
		}
		ItemKey itemKey = medicine.GetItemKey();
		_inventory.OfflineRemove(itemKey, 1);
		mod.InventoryChanged = true;
		if (eatingSlot >= 0)
		{
			_eatingItems.Set(eatingSlot, itemKey, medicine.GetDuration());
			mod.EatingItemsChanged = true;
		}
	}

	private void OfflineRemoveUsedTeaWine(List<(GameData.Domains.Item.TeaWine item, int amount)> items, int selectedIndex, sbyte eatingSlot, PeriAdvanceMonthUpdateStatusModification mod)
	{
		var (teaWine, num) = items[selectedIndex];
		if (num > 1)
		{
			items[selectedIndex] = (teaWine, num - 1);
		}
		else
		{
			items.RemoveAt(selectedIndex);
		}
		ItemKey itemKey = teaWine.GetItemKey();
		_inventory.OfflineRemove(itemKey, 1);
		mod.InventoryChanged = true;
		short duration = Config.TeaWine.Instance[itemKey.TemplateId].Duration;
		_eatingItems.Set(eatingSlot, itemKey, duration);
		mod.EatingItemsChanged = true;
	}

	private void OfflineRemoveUsedFood(List<(GameData.Domains.Item.Food item, int amount)> items, int selectedIndex, sbyte eatingSlot, PeriAdvanceMonthUpdateStatusModification mod)
	{
		var (food, num) = items[selectedIndex];
		if (num > 1)
		{
			items[selectedIndex] = (food, num - 1);
		}
		else
		{
			items.RemoveAt(selectedIndex);
		}
		ItemKey itemKey = food.GetItemKey();
		_inventory.OfflineRemove(itemKey, 1);
		mod.InventoryChanged = true;
		short duration = Config.Food.Instance[itemKey.TemplateId].Duration;
		_eatingItems.Set(eatingSlot, itemKey, duration);
		mod.EatingItemsChanged = true;
	}

	public unsafe int SelectTopicalMedicineIndex(List<(GameData.Domains.Item.Medicine item, int amount)> medicines, sbyte level, ref MainAttributes mainAttributes, List<ItemKey> selections = null)
	{
		bool flag = selections != null;
		if (flag)
		{
			selections.Clear();
		}
		int result = -1;
		for (int i = 0; i < medicines.Count; i++)
		{
			(GameData.Domains.Item.Medicine, int) tuple = medicines[i];
			if (TryDetectAttachedPoisons(tuple.Item1.GetItemKey()))
			{
				continue;
			}
			sbyte requiredMainAttributeType = tuple.Item1.GetRequiredMainAttributeType();
			sbyte requiredMainAttributeValue = tuple.Item1.GetRequiredMainAttributeValue();
			if (mainAttributes.Items[requiredMainAttributeType] >= requiredMainAttributeValue)
			{
				if (flag)
				{
					selections.Add(tuple.Item1.GetItemKey());
				}
				int num = tuple.Item1.GetEffectValue() * tuple.Item1.GetInjuryRecoveryTimes() * tuple.Item1.GetDuration();
				if (num >= level)
				{
					return i;
				}
				result = i;
			}
		}
		return result;
	}

	public int SelectMedicineIndexForInjury(List<(GameData.Domains.Item.Medicine item, int amount)> medicines, sbyte minLevel, sbyte injuryTotalLevel, List<ItemKey> selections = null)
	{
		bool flag = selections != null;
		if (flag)
		{
			selections.Clear();
		}
		int result = -1;
		for (int i = 0; i < medicines.Count; i++)
		{
			(GameData.Domains.Item.Medicine, int) tuple = medicines[i];
			if (!TryDetectAttachedPoisons(tuple.Item1.GetItemKey()))
			{
				if (flag)
				{
					selections.Add(tuple.Item1.GetItemKey());
				}
				int num = tuple.Item1.GetEffectValue() * tuple.Item1.GetInjuryRecoveryTimes() * tuple.Item1.GetDuration();
				if (num >= injuryTotalLevel)
				{
					return i;
				}
				result = i;
			}
		}
		return result;
	}

	public int SelectMedicineIndexForDetoxPoison(List<(GameData.Domains.Item.Medicine item, int amount)> medicines, sbyte poisonType, sbyte level, int poisonedVal, List<ItemKey> selections = null)
	{
		bool flag = selections != null;
		if (flag)
		{
			selections.Clear();
		}
		int result = -1;
		for (int i = 0; i < medicines.Count; i++)
		{
			(GameData.Domains.Item.Medicine, int) tuple = medicines[i];
			if (tuple.Item1.GetEffectSubType().DetoxPoisonType() != poisonType)
			{
				continue;
			}
			MedicineItem medicineItem = Config.Medicine.Instance[tuple.Item1.GetTemplateId()];
			sbyte b = (sbyte)medicineItem.EffectThresholdValue;
			if (level <= b && !TryDetectAttachedPoisons(tuple.Item1.GetItemKey()))
			{
				if (flag)
				{
					selections.Add(tuple.Item1.GetItemKey());
				}
				int num = CalcMedicineEffectDelta(poisonedVal, medicineItem.EffectValue, medicineItem.EffectIsPercentage);
				if (num + poisonedVal <= 0)
				{
					return i;
				}
				result = i;
			}
		}
		return result;
	}

	public int SelectMedicineIndexForWug(List<(GameData.Domains.Item.Medicine item, int amount)> medicines, sbyte wugType, short remainingDuration, List<ItemKey> selections = null)
	{
		bool flag = selections != null;
		if (flag)
		{
			selections.Clear();
		}
		int result = -1;
		for (int i = 0; i < medicines.Count; i++)
		{
			(GameData.Domains.Item.Medicine, int) tuple = medicines[i];
			if (EMedicineEffectSubTypeExtension.DetoxWugType(tuple.Item1.GetEffectType(), tuple.Item1.GetSideEffectValue()) == wugType && !TryDetectAttachedPoisons(tuple.Item1.GetItemKey()))
			{
				if (flag)
				{
					selections.Add(tuple.Item1.GetItemKey());
				}
				short deltaWugDuration = GameData.Domains.Item.Medicine.GetDeltaWugDuration(tuple.Item1.GetGrade());
				if (remainingDuration + deltaWugDuration <= 0)
				{
					return i;
				}
				result = i;
			}
		}
		return result;
	}

	public static int SelectPoisonIndexForWug(List<(GameData.Domains.Item.Medicine item, int amount)> poisons, sbyte wugType, short remainingDuration, List<ItemKey> selections = null)
	{
		bool flag = selections != null;
		if (flag)
		{
			selections.Clear();
		}
		int result = -1;
		for (int i = 0; i < poisons.Count; i++)
		{
			(GameData.Domains.Item.Medicine, int) tuple = poisons[i];
			if (EMedicineEffectSubTypeExtension.DetoxWugType(tuple.Item1.GetEffectType(), tuple.Item1.GetSideEffectValue()) == wugType)
			{
				if (flag)
				{
					selections.Add(tuple.Item1.GetItemKey());
				}
				short deltaWugDuration = GameData.Domains.Item.Medicine.GetDeltaWugDuration(tuple.Item1.GetGrade());
				if (remainingDuration + deltaWugDuration <= 0)
				{
					return i;
				}
				result = i;
			}
		}
		return result;
	}

	public int SelectMedicineIndexForQiDisorder(List<(GameData.Domains.Item.Medicine item, int amount)> medicines, short disorderOfQi, List<ItemKey> selections = null)
	{
		bool flag = selections != null;
		if (flag)
		{
			selections.Clear();
		}
		int result = -1;
		for (int i = 0; i < medicines.Count; i++)
		{
			(GameData.Domains.Item.Medicine, int) tuple = medicines[i];
			if (!TryDetectAttachedPoisons(tuple.Item1.GetItemKey()))
			{
				if (flag)
				{
					selections.Add(tuple.Item1.GetItemKey());
				}
				int num = CalcMedicineEffectDelta(disorderOfQi, tuple.Item1.GetEffectValue(), tuple.Item1.GetEffectSubType().IsPercentage());
				if (disorderOfQi + num <= DisorderLevelOfQi.MinValue)
				{
					return i;
				}
				result = i;
			}
		}
		return result;
	}

	public int SelectMedicineIndexForHealth(List<(GameData.Domains.Item.Medicine item, int amount)> medicines, short health, short leftMaxHealth, List<ItemKey> selections = null)
	{
		bool flag = selections != null;
		if (flag)
		{
			selections.Clear();
		}
		int result = -1;
		for (int i = 0; i < medicines.Count; i++)
		{
			(GameData.Domains.Item.Medicine, int) tuple = medicines[i];
			if (!TryDetectAttachedPoisons(tuple.Item1.GetItemKey()))
			{
				if (flag)
				{
					selections.Add(tuple.Item1.GetItemKey());
				}
				int num = CalcMedicineEffectDelta(leftMaxHealth - health, tuple.Item1.GetEffectValue(), tuple.Item1.GetEffectSubType().IsPercentage());
				if (health + num >= leftMaxHealth)
				{
					return i;
				}
				result = i;
			}
		}
		return result;
	}

	public int SelectItemIndexForNeili(List<(GameData.Domains.Item.Misc item, int amount)> items, int currNeili, int maxNeili, List<ItemKey> selections = null)
	{
		bool flag = selections != null;
		if (flag)
		{
			selections.Clear();
		}
		int result = -1;
		for (int i = 0; i < items.Count; i++)
		{
			(GameData.Domains.Item.Misc, int) tuple = items[i];
			if (!TryDetectAttachedPoisons(tuple.Item1.GetItemKey()))
			{
				if (flag)
				{
					selections.Add(tuple.Item1.GetItemKey());
				}
				if (currNeili + tuple.Item1.GetNeili() >= maxNeili)
				{
					return i;
				}
				result = i;
			}
		}
		return result;
	}

	public unsafe int SelectFoodIndexForMainAttributes(List<(GameData.Domains.Item.Food item, int amount)> foods, sbyte attrType, short currAttr, short maxAttr, bool allowMeat, List<ItemKey> selections = null)
	{
		bool flag = selections != null;
		if (flag)
		{
			selections.Clear();
		}
		int result = -1;
		for (int i = 0; i < foods.Count; i++)
		{
			(GameData.Domains.Item.Food, int) tuple = foods[i];
			FoodItem foodItem = Config.Food.Instance[tuple.Item1.GetTemplateId()];
			if ((allowMeat || foodItem.ItemSubType != 701) && !TryDetectAttachedPoisons(tuple.Item1.GetItemKey()))
			{
				if (flag)
				{
					selections.Add(tuple.Item1.GetItemKey());
				}
				if (currAttr + foodItem.MainAttributesRegen.Items[attrType] >= maxAttr)
				{
					return i;
				}
				result = i;
			}
		}
		return result;
	}

	public int SelectTeaWineForHappiness(List<(GameData.Domains.Item.TeaWine item, int amount)> teaWines, int currHappiness, int targetHappiness, bool allowWines, List<ItemKey> selections = null)
	{
		bool flag = selections != null;
		if (flag)
		{
			selections.Clear();
		}
		int result = -1;
		for (int i = 0; i < teaWines.Count; i++)
		{
			(GameData.Domains.Item.TeaWine, int) tuple = teaWines[i];
			TeaWineItem teaWineItem = Config.TeaWine.Instance[tuple.Item1.GetTemplateId()];
			if ((allowWines || teaWineItem.ItemSubType != 901) && !TryDetectAttachedPoisons(tuple.Item1.GetItemKey()))
			{
				if (flag)
				{
					selections.Add(tuple.Item1.GetItemKey());
				}
				if (currHappiness + tuple.Item1.GetHappinessChange() >= targetHappiness)
				{
					return i;
				}
				result = i;
			}
		}
		return result;
	}

	private bool TripleStartRelationChance(short aiRelationsTemplateId)
	{
		Location tripleAdoreLocation = DomainManager.Character.TripleAdoreLocation;
		Location tripleHateLocation = DomainManager.Character.TripleHateLocation;
		if (aiRelationsTemplateId == 2 && tripleAdoreLocation.IsValid() && tripleAdoreLocation.AreaId == _location.AreaId)
		{
			byte areaSize = DomainManager.Map.GetAreaSize(_location.AreaId);
			ByteCoordinate byteCoordinate = ByteCoordinate.IndexToCoordinate(tripleAdoreLocation.BlockId, areaSize);
			ByteCoordinate byteCoordinate2 = ByteCoordinate.IndexToCoordinate(_location.BlockId, areaSize);
			return byteCoordinate.GetManhattanDistance(byteCoordinate2) <= 2;
		}
		if (aiRelationsTemplateId == 0 && tripleHateLocation.IsValid() && tripleHateLocation.AreaId == _location.AreaId)
		{
			byte areaSize2 = DomainManager.Map.GetAreaSize(_location.AreaId);
			ByteCoordinate byteCoordinate3 = ByteCoordinate.IndexToCoordinate(tripleHateLocation.BlockId, areaSize2);
			ByteCoordinate byteCoordinate4 = ByteCoordinate.IndexToCoordinate(_location.BlockId, areaSize2);
			return byteCoordinate3.GetManhattanDistance(byteCoordinate4) <= 2;
		}
		return false;
	}

	private Character GetStartOrEndRelationTarget(IRandomSource random, short aiRelationsTemplateId, List<int> selectableChars, ref Personalities selfPersonalities)
	{
		if (selectableChars.Count == 0)
		{
			return null;
		}
		int multiplier = ((!TripleStartRelationChance(aiRelationsTemplateId)) ? 1 : 3);
		AiRelationsItem aiRelationConfig = AiHelper.Relation.GetAiRelationConfig(aiRelationsTemplateId);
		if (!AiHelper.Relation.CheckChangeRelationTypeChance(random, ref selfPersonalities, aiRelationConfig.PersonalityType, multiplier))
		{
			return null;
		}
		int random2 = selectableChars.GetRandom(random);
		Character element_Objects = DomainManager.Character.GetElement_Objects(random2);
		RelatedCharacter relation = DomainManager.Character.GetRelation(_id, random2);
		sbyte sectFavorability = DomainManager.Organization.GetSectFavorability(_organizationInfo.OrgTemplateId, element_Objects.GetOrganizationInfo().OrgTemplateId);
		int startOrEndRelationChance = AiHelper.Relation.GetStartOrEndRelationChance(aiRelationConfig, this, element_Objects, relation.RelationType, sectFavorability, multiplier);
		return random.CheckProb(startOrEndRelationChance, 10000) ? element_Objects : null;
	}

	private Character GetStartAdoptiveParentRelationTarget(IRandomSource random, short aiRelationsTemplateId, List<int> selectableChars, ref Personalities selfPersonalities)
	{
		if (selectableChars.Count == 0)
		{
			return null;
		}
		AiRelationsItem aiRelationConfig = AiHelper.Relation.GetAiRelationConfig(aiRelationsTemplateId);
		if (!AiHelper.Relation.CheckChangeRelationTypeChance(random, ref selfPersonalities, aiRelationConfig.PersonalityType))
		{
			return null;
		}
		int random2 = selectableChars.GetRandom(random);
		Character element_Objects = DomainManager.Character.GetElement_Objects(random2);
		RelatedCharacter relation = DomainManager.Character.GetRelation(random2, _id);
		int num = element_Objects.GetCurrAge() - _currAge;
		num += (element_Objects.GetInteractionGrade() - GetInteractionGrade()) * 3;
		num += (FavorabilityType.GetFavorabilityType(relation.Favorability) - 4) * 5;
		return random.CheckPercentProb(num) ? element_Objects : null;
	}

	private Character GetStartAdoptiveChildRelationTarget(IRandomSource random, short aiRelationsTemplateId, List<int> selectableChars, ref Personalities selfPersonalities)
	{
		if (selectableChars.Count == 0)
		{
			return null;
		}
		AiRelationsItem aiRelationConfig = AiHelper.Relation.GetAiRelationConfig(aiRelationsTemplateId);
		if (!AiHelper.Relation.CheckChangeRelationTypeChance(random, ref selfPersonalities, aiRelationConfig.PersonalityType))
		{
			return null;
		}
		int random2 = selectableChars.GetRandom(random);
		Character element_Objects = DomainManager.Character.GetElement_Objects(random2);
		RelatedCharacter relation = DomainManager.Character.GetRelation(random2, _id);
		int num = _currAge - element_Objects.GetCurrAge();
		num += (GetInteractionGrade() - element_Objects.GetInteractionGrade()) * 3;
		num += (FavorabilityType.GetFavorabilityType(relation.Favorability) - 4) * 5;
		return random.CheckPercentProb(num) ? element_Objects : null;
	}

	public void PeriAdvanceMonth_RelationsUpdate(DataContext context, HashSet<int> charSet)
	{
		if (GetAgeGroup() == 0 || DomainManager.LegendaryBook.IsCharacterActingCrazy(this))
		{
			return;
		}
		PeriAdvanceMonthRelationsUpdateModification periAdvanceMonthRelationsUpdateModification = new PeriAdvanceMonthRelationsUpdateModification(this);
		bool flag = false;
		List<Character> list = new List<Character>();
		PotentialRelatedCharacters obj = context.AdvanceMonthRelatedData.CurrBlockCanStartRelationChars.Occupy();
		PotentialRelatedCharacters obj2 = context.AdvanceMonthRelatedData.CurrBlockCanEndRelationChars.Occupy();
		DomainManager.Character.GetPotentialRelatedCharactersInSet(obj, obj2, list, this, charSet);
		if (list.Count != 0)
		{
			periAdvanceMonthRelationsUpdateModification.NewlyMetCharacters = list;
			flag = true;
		}
		Personalities selfPersonalities = GetPersonalities();
		IRandomSource random = context.Random;
		Character startOrEndRelationTarget = GetStartOrEndRelationTarget(random, 0, obj.Enemies, ref selfPersonalities);
		if (startOrEndRelationTarget != null)
		{
			flag = true;
			sbyte behaviorType = startOrEndRelationTarget.GetBehaviorType();
			bool item = random.CheckPercentProb(AiHelper.RelationsRelatedConstants.SeverEnemyNotMutuallyChance[behaviorType]);
			PeriAdvanceMonthRelationsUpdateModification periAdvanceMonthRelationsUpdateModification2 = periAdvanceMonthRelationsUpdateModification;
			if (periAdvanceMonthRelationsUpdateModification2.NewRegularRelations == null)
			{
				periAdvanceMonthRelationsUpdateModification2.NewRegularRelations = new List<(Character, ushort, bool)>();
			}
			periAdvanceMonthRelationsUpdateModification.NewRegularRelations.Add((startOrEndRelationTarget, 32768, item));
		}
		startOrEndRelationTarget = GetStartOrEndRelationTarget(random, 1, obj2.Enemies, ref selfPersonalities);
		if (startOrEndRelationTarget != null)
		{
			flag = true;
			sbyte behaviorType2 = startOrEndRelationTarget.GetBehaviorType();
			bool item2 = random.CheckPercentProb(AiHelper.RelationsRelatedConstants.SeverEnemyNotMutuallyChance[behaviorType2]);
			PeriAdvanceMonthRelationsUpdateModification periAdvanceMonthRelationsUpdateModification2 = periAdvanceMonthRelationsUpdateModification;
			if (periAdvanceMonthRelationsUpdateModification2.RemovedRegularRelations == null)
			{
				periAdvanceMonthRelationsUpdateModification2.RemovedRegularRelations = new List<(Character, ushort, bool)>();
			}
			periAdvanceMonthRelationsUpdateModification.RemovedRegularRelations.Add((startOrEndRelationTarget, 32768, item2));
		}
		startOrEndRelationTarget = GetStartOrEndRelationTarget(random, 2, obj.Adored, ref selfPersonalities);
		if (startOrEndRelationTarget != null)
		{
			RelatedCharacter relation = DomainManager.Character.GetRelation(_id, startOrEndRelationTarget.GetId());
			RelatedCharacter relation2 = DomainManager.Character.GetRelation(startOrEndRelationTarget.GetId(), _id);
			int startRelationSuccessRate_Adored = AiHelper.Relation.GetStartRelationSuccessRate_Adored(this, startOrEndRelationTarget, relation, relation2);
			if (random.CheckProb(startRelationSuccessRate_Adored, 100))
			{
				flag = true;
				int startRelationSuccessRate_Adored2 = AiHelper.Relation.GetStartRelationSuccessRate_Adored(startOrEndRelationTarget, this, relation, relation2);
				PeriAdvanceMonthRelationsUpdateModification periAdvanceMonthRelationsUpdateModification2 = periAdvanceMonthRelationsUpdateModification;
				if (periAdvanceMonthRelationsUpdateModification2.NewRegularRelations == null)
				{
					periAdvanceMonthRelationsUpdateModification2.NewRegularRelations = new List<(Character, ushort, bool)>();
				}
				periAdvanceMonthRelationsUpdateModification.NewRegularRelations.Add((startOrEndRelationTarget, 16384, random.CheckPercentProb(startRelationSuccessRate_Adored2)));
			}
		}
		startOrEndRelationTarget = GetStartOrEndRelationTarget(random, 3, obj.BoyAndGirlFriends, ref selfPersonalities);
		if (startOrEndRelationTarget != null)
		{
			flag = true;
			RelatedCharacter relation3 = DomainManager.Character.GetRelation(_id, startOrEndRelationTarget.GetId());
			RelatedCharacter relation4 = DomainManager.Character.GetRelation(startOrEndRelationTarget.GetId(), _id);
			int startRelationSuccessRate_BoyOrGirlFriend = AiHelper.Relation.GetStartRelationSuccessRate_BoyOrGirlFriend(this, startOrEndRelationTarget, relation3, relation4);
			periAdvanceMonthRelationsUpdateModification.NewBoyOrGirlFriend = (targetChar: startOrEndRelationTarget, succeed: random.CheckPercentProb(startRelationSuccessRate_BoyOrGirlFriend));
		}
		startOrEndRelationTarget = GetStartOrEndRelationTarget(random, 4, obj2.BoyAndGirlFriends, ref selfPersonalities);
		if (startOrEndRelationTarget != null)
		{
			flag = true;
			sbyte behaviorType3 = startOrEndRelationTarget.GetBehaviorType();
			bool item3 = !random.CheckPercentProb(AiHelper.RelationsRelatedConstants.BreakupMutuallyChance[behaviorType3]);
			PeriAdvanceMonthRelationsUpdateModification periAdvanceMonthRelationsUpdateModification2 = periAdvanceMonthRelationsUpdateModification;
			if (periAdvanceMonthRelationsUpdateModification2.RemovedRegularRelations == null)
			{
				periAdvanceMonthRelationsUpdateModification2.RemovedRegularRelations = new List<(Character, ushort, bool)>();
			}
			periAdvanceMonthRelationsUpdateModification.RemovedRegularRelations.Add((startOrEndRelationTarget, 16384, item3));
		}
		startOrEndRelationTarget = GetStartOrEndRelationTarget(random, 5, obj.HusbandsAndWives, ref selfPersonalities);
		if (startOrEndRelationTarget != null)
		{
			flag = true;
			RelatedCharacter relation5 = DomainManager.Character.GetRelation(_id, startOrEndRelationTarget.GetId());
			RelatedCharacter relation6 = DomainManager.Character.GetRelation(startOrEndRelationTarget.GetId(), _id);
			int startRelationSuccessRate_HusbandOrWife = AiHelper.Relation.GetStartRelationSuccessRate_HusbandOrWife(this, startOrEndRelationTarget, relation5, relation6);
			PeriAdvanceMonthRelationsUpdateModification periAdvanceMonthRelationsUpdateModification2 = periAdvanceMonthRelationsUpdateModification;
			if (periAdvanceMonthRelationsUpdateModification2.NewRegularRelations == null)
			{
				periAdvanceMonthRelationsUpdateModification2.NewRegularRelations = new List<(Character, ushort, bool)>();
			}
			periAdvanceMonthRelationsUpdateModification.NewRegularRelations.Add((startOrEndRelationTarget, 1024, random.CheckProb(startRelationSuccessRate_HusbandOrWife, 100)));
		}
		startOrEndRelationTarget = GetStartOrEndRelationTarget(random, 12, obj2.HusbandsAndWives, ref selfPersonalities);
		if (startOrEndRelationTarget != null)
		{
			flag = true;
			PeriAdvanceMonthRelationsUpdateModification periAdvanceMonthRelationsUpdateModification2 = periAdvanceMonthRelationsUpdateModification;
			if (periAdvanceMonthRelationsUpdateModification2.RemovedRegularRelations == null)
			{
				periAdvanceMonthRelationsUpdateModification2.RemovedRegularRelations = new List<(Character, ushort, bool)>();
			}
			periAdvanceMonthRelationsUpdateModification.RemovedRegularRelations.Add((startOrEndRelationTarget, 1024, false));
		}
		startOrEndRelationTarget = GetStartOrEndRelationTarget(random, 6, obj.Friends, ref selfPersonalities);
		if (startOrEndRelationTarget != null)
		{
			flag = true;
			PeriAdvanceMonthRelationsUpdateModification periAdvanceMonthRelationsUpdateModification2 = periAdvanceMonthRelationsUpdateModification;
			if (periAdvanceMonthRelationsUpdateModification2.NewRegularRelations == null)
			{
				periAdvanceMonthRelationsUpdateModification2.NewRegularRelations = new List<(Character, ushort, bool)>();
			}
			periAdvanceMonthRelationsUpdateModification.NewRegularRelations.Add((startOrEndRelationTarget, 8192, true));
		}
		startOrEndRelationTarget = GetStartOrEndRelationTarget(random, 7, obj2.Friends, ref selfPersonalities);
		if (startOrEndRelationTarget != null)
		{
			flag = true;
			PeriAdvanceMonthRelationsUpdateModification periAdvanceMonthRelationsUpdateModification2 = periAdvanceMonthRelationsUpdateModification;
			if (periAdvanceMonthRelationsUpdateModification2.RemovedRegularRelations == null)
			{
				periAdvanceMonthRelationsUpdateModification2.RemovedRegularRelations = new List<(Character, ushort, bool)>();
			}
			periAdvanceMonthRelationsUpdateModification.RemovedRegularRelations.Add((startOrEndRelationTarget, 8192, false));
		}
		startOrEndRelationTarget = GetStartOrEndRelationTarget(random, 8, obj.SwornBrothersAndSisters, ref selfPersonalities);
		if (startOrEndRelationTarget != null)
		{
			flag = true;
			PeriAdvanceMonthRelationsUpdateModification periAdvanceMonthRelationsUpdateModification2 = periAdvanceMonthRelationsUpdateModification;
			if (periAdvanceMonthRelationsUpdateModification2.NewRegularRelations == null)
			{
				periAdvanceMonthRelationsUpdateModification2.NewRegularRelations = new List<(Character, ushort, bool)>();
			}
			periAdvanceMonthRelationsUpdateModification.NewRegularRelations.Add((startOrEndRelationTarget, 512, true));
		}
		startOrEndRelationTarget = GetStartOrEndRelationTarget(random, 9, obj2.SwornBrothersAndSisters, ref selfPersonalities);
		if (startOrEndRelationTarget != null)
		{
			flag = true;
			PeriAdvanceMonthRelationsUpdateModification periAdvanceMonthRelationsUpdateModification2 = periAdvanceMonthRelationsUpdateModification;
			if (periAdvanceMonthRelationsUpdateModification2.RemovedRegularRelations == null)
			{
				periAdvanceMonthRelationsUpdateModification2.RemovedRegularRelations = new List<(Character, ushort, bool)>();
			}
			periAdvanceMonthRelationsUpdateModification.RemovedRegularRelations.Add((startOrEndRelationTarget, 512, false));
		}
		startOrEndRelationTarget = GetStartAdoptiveParentRelationTarget(random, 10, obj.AdoptiveParents, ref selfPersonalities);
		if (startOrEndRelationTarget != null)
		{
			flag = true;
			PeriAdvanceMonthRelationsUpdateModification periAdvanceMonthRelationsUpdateModification2 = periAdvanceMonthRelationsUpdateModification;
			if (periAdvanceMonthRelationsUpdateModification2.NewRegularRelations == null)
			{
				periAdvanceMonthRelationsUpdateModification2.NewRegularRelations = new List<(Character, ushort, bool)>();
			}
			periAdvanceMonthRelationsUpdateModification.NewRegularRelations.Add((startOrEndRelationTarget, 64, true));
		}
		startOrEndRelationTarget = GetStartAdoptiveChildRelationTarget(random, 11, obj.AdoptiveChildren, ref selfPersonalities);
		if (startOrEndRelationTarget != null)
		{
			flag = true;
			PeriAdvanceMonthRelationsUpdateModification periAdvanceMonthRelationsUpdateModification2 = periAdvanceMonthRelationsUpdateModification;
			if (periAdvanceMonthRelationsUpdateModification2.NewRegularRelations == null)
			{
				periAdvanceMonthRelationsUpdateModification2.NewRegularRelations = new List<(Character, ushort, bool)>();
			}
			periAdvanceMonthRelationsUpdateModification.NewRegularRelations.Add((startOrEndRelationTarget, 128, true));
		}
		if (flag)
		{
			ParallelModificationsRecorder parallelModificationsRecorder = context.ParallelModificationsRecorder;
			parallelModificationsRecorder.RecordType(ParallelModificationType.PeriAdvanceMonthRelationsUpdate);
			parallelModificationsRecorder.RecordParameterClass(periAdvanceMonthRelationsUpdateModification);
		}
		context.AdvanceMonthRelatedData.CurrBlockCanStartRelationChars.Release(ref obj);
		context.AdvanceMonthRelatedData.CurrBlockCanEndRelationChars.Release(ref obj2);
	}

	public static void ComplementPeriAdvanceMonth_RelationsUpdate(DataContext context, PeriAdvanceMonthRelationsUpdateModification mod)
	{
		Character character = mod.Character;
		int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
		int id = character.GetId();
		sbyte behaviorType = character.GetBehaviorType();
		bool selfIsTaiwuPeople = DomainManager.Character.IsTaiwuPeople(id);
		MonthlyEventCollection monthlyEventCollection = DomainManager.World.GetMonthlyEventCollection();
		Location location = DomainManager.Taiwu.GetTaiwu().GetLocation();
		sbyte gender = DomainManager.Taiwu.GetTaiwu().GetGender();
		if (mod.NewlyMetCharacters != null)
		{
			foreach (Character newlyMetCharacter in mod.NewlyMetCharacters)
			{
				DomainManager.Character.TryCreateGeneralRelation(context, character, newlyMetCharacter);
			}
		}
		if (mod.NewRegularRelations != null)
		{
			foreach (var newRegularRelation in mod.NewRegularRelations)
			{
				Character item = newRegularRelation.targetChar;
				ushort item2 = newRegularRelation.relationType;
				bool item3 = newRegularRelation.succeed;
				bool flag = DomainManager.Character.IsTaiwuPeople(item._id);
				switch (item2)
				{
				case 64:
					if (item._id == taiwuCharId)
					{
						if (gender == 0)
						{
							monthlyEventCollection.AddGetAdoptedByMother(id, location, taiwuCharId);
						}
						else
						{
							monthlyEventCollection.AddGetAdoptedByFather(id, location, taiwuCharId);
						}
					}
					else
					{
						ApplyAddRelation_AdoptiveParent(context, character, item, behaviorType, selfIsTaiwuPeople, flag);
					}
					break;
				case 128:
					if (item._id == taiwuCharId)
					{
						if (gender == 0)
						{
							monthlyEventCollection.AddAdoptDaughter(id, location, taiwuCharId);
						}
						else
						{
							monthlyEventCollection.AddAdoptSon(id, location, taiwuCharId);
						}
					}
					else
					{
						ApplyAddRelation_AdoptiveChild(context, character, item, behaviorType, selfIsTaiwuPeople, flag);
					}
					break;
				case 512:
					if (item._id == taiwuCharId)
					{
						monthlyEventCollection.AddBecomeSwornBrotherOrSister(id, location, taiwuCharId);
					}
					else
					{
						ApplyBecomeSwornBrotherOrSister(context, character, item, behaviorType, selfIsTaiwuPeople, flag);
					}
					break;
				case 1024:
					if (item._id == taiwuCharId)
					{
						monthlyEventCollection.AddProposeMarriage(id, location, taiwuCharId);
					}
					else
					{
						ApplyBecomeHusbandOrWife(context, character, item, behaviorType, item3, selfIsTaiwuPeople, flag);
					}
					break;
				case 8192:
					if (item._id == taiwuCharId)
					{
						monthlyEventCollection.AddBecomeFriend(id, location, taiwuCharId);
					}
					else
					{
						ApplyBecomeFriend(context, character, item, behaviorType, selfIsTaiwuPeople, flag);
					}
					break;
				case 16384:
					if (item._id == taiwuCharId)
					{
						monthlyEventCollection.AddAdore(id, location, taiwuCharId);
					}
					else
					{
						ApplyAddRelation_Adore(context, character, item, behaviorType, item3, selfIsTaiwuPeople, flag);
					}
					break;
				case 32768:
					if (item._id == taiwuCharId)
					{
						monthlyEventCollection.AddMakeEnemy(id, location, taiwuCharId);
						break;
					}
					ApplyAddRelation_Enemy(context, character, item, selfIsTaiwuPeople, 0);
					if (item3)
					{
						ApplyAddRelation_Enemy(context, item, character, flag, 0);
					}
					break;
				default:
					throw new Exception($"Given relation type cannot be handled as a regular relation type {item2}");
				}
			}
		}
		if (mod.RemovedRegularRelations != null)
		{
			foreach (var removedRegularRelation in mod.RemovedRegularRelations)
			{
				Character item4 = removedRegularRelation.targetChar;
				ushort item5 = removedRegularRelation.relationType;
				bool item6 = removedRegularRelation.targetStillHasRelation;
				bool flag2 = DomainManager.Character.IsTaiwuPeople(item4._id);
				switch (item5)
				{
				case 512:
					if (item4._id == taiwuCharId)
					{
						monthlyEventCollection.AddSeverSwornBrotherhood(id, location, taiwuCharId);
					}
					else
					{
						ApplySeverSwornBrotherOrSister(context, character, item4, behaviorType, selfIsTaiwuPeople, flag2);
					}
					break;
				case 1024:
					if (item4._id != taiwuCharId)
					{
						ApplySeverHusbandOrWife(context, character, item4, behaviorType, selfIsTaiwuPeople, flag2);
					}
					break;
				case 8192:
					if (item4._id == taiwuCharId)
					{
						monthlyEventCollection.AddSeverFriendship(id, location, taiwuCharId);
					}
					else
					{
						ApplySeverFriend(context, character, item4, behaviorType, selfIsTaiwuPeople, flag2);
					}
					break;
				case 16384:
					if (item4._id == taiwuCharId)
					{
						monthlyEventCollection.AddBreakup(id, location, taiwuCharId);
					}
					else
					{
						ApplyBreakupWithBoyOrGirlFriend(context, character, item4, behaviorType, item6, selfIsTaiwuPeople, flag2);
					}
					break;
				case 32768:
					if (item4._id == taiwuCharId)
					{
						monthlyEventCollection.AddSeverEnemy(id, location, taiwuCharId);
						break;
					}
					ApplySeverEnemy(context, character, item4, behaviorType, selfIsTaiwuPeople);
					if (!item6)
					{
						ApplySeverEnemy(context, item4, character, item4.GetBehaviorType(), flag2);
					}
					break;
				default:
					throw new Exception($"Given relation type cannot be handled as a regular relation type {item5}");
				}
			}
		}
		if (mod.NewBoyOrGirlFriend.targetChar != null)
		{
			var (character2, succeed) = mod.NewBoyOrGirlFriend;
			if (character2._id == taiwuCharId)
			{
				monthlyEventCollection.AddConfess(id, location, taiwuCharId);
				return;
			}
			bool targetIsTaiwuPeople = DomainManager.Character.IsTaiwuPeople(character2._id);
			ApplyBecomeBoyOrGirlFriend(context, character, character2, behaviorType, succeed, selfIsTaiwuPeople, targetIsTaiwuPeople);
		}
	}

	public static void ApplyAddRelation_Enemy(DataContext context, Character selfChar, Character targetChar, bool selfIsTaiwuPeople, short becomeEnemyType)
	{
		CharacterBecomeEnemyInfo becomeEnemyInfo = new CharacterBecomeEnemyInfo(selfChar);
		ApplyAddRelation_Enemy(context, selfChar, targetChar, selfIsTaiwuPeople, becomeEnemyType, becomeEnemyInfo);
	}

	public static void ApplyAddRelation_Enemy(DataContext context, Character selfChar, Character targetChar, bool selfIsTaiwuPeople, short becomeEnemyType, CharacterBecomeEnemyInfo becomeEnemyInfo)
	{
		if (RelationTypeHelper.AllowAddingEnemyRelation(selfChar._id, targetChar._id))
		{
			int currDate = DomainManager.World.GetCurrDate();
			Location location = selfChar._location;
			if (!location.IsValid())
			{
				location = targetChar._location;
			}
			DomainManager.Character.AddRelation(context, selfChar._id, targetChar._id, 32768, currDate);
			sbyte behaviorType = selfChar.GetBehaviorType();
			selfChar.ChangeHappiness(context, AiHelper.RelationsRelatedConstants.BecomeEnemyHappinessChange[behaviorType]);
			BecomeEnemyTypeItem becomeEnemyType2 = BecomeEnemyType.Instance[becomeEnemyType];
			LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
			lifeRecordCollection.AddBecomeEnemyRecord(selfChar, targetChar, becomeEnemyType2, ref becomeEnemyInfo);
			SecretInformationCollection secretInformationCollection = DomainManager.Information.GetSecretInformationCollection();
			int dataOffset = secretInformationCollection.AddBecomeEnemy(selfChar._id, targetChar._id);
			int metaDataId = DomainManager.Information.AddSecretInformationMetaData(context, dataOffset);
			if (selfIsTaiwuPeople)
			{
				DomainManager.World.GetMonthlyNotificationCollection().AddCreateHatred(selfChar._id, location, targetChar._id);
				DomainManager.Information.ReceiveSecretInformation(context, metaDataId, DomainManager.Taiwu.GetTaiwuCharId(), selfChar._id);
			}
		}
	}

	public static void ApplySeverEnemy(DataContext context, Character selfChar, Character targetChar, sbyte charBehaviorType, bool selfIsTaiwuPeople)
	{
		int currDate = DomainManager.World.GetCurrDate();
		int id = selfChar._id;
		int id2 = targetChar._id;
		if (DomainManager.Character.HasRelation(id, id2, 32768))
		{
			Location location = selfChar._location;
			DomainManager.Character.ChangeRelationType(context, id, id2, 32768, 0);
			selfChar.ChangeHappiness(context, AiHelper.RelationsRelatedConstants.SeverEnemyHappinessChange[charBehaviorType]);
			DomainManager.Character.ChangeFavorabilityOptionalRepeatedEvent(context, selfChar, targetChar, 3000);
			DomainManager.Character.ChangeFavorabilityOptionalRepeatedEvent(context, targetChar, selfChar, 3000);
			DomainManager.LifeRecord.GetLifeRecordCollection().AddSeverEnemy(id, currDate, id2, location);
			SecretInformationCollection secretInformationCollection = DomainManager.Information.GetSecretInformationCollection();
			int dataOffset = secretInformationCollection.AddSeverEnemy(selfChar._id, targetChar._id);
			int metaDataId = DomainManager.Information.AddSecretInformationMetaData(context, dataOffset);
			if (selfIsTaiwuPeople)
			{
				DomainManager.World.GetMonthlyNotificationCollection().AddDecreaseHatred(id, location, id2);
				DomainManager.Information.ReceiveSecretInformation(context, metaDataId, DomainManager.Taiwu.GetTaiwuCharId(), selfChar._id);
			}
		}
	}

	public static void ApplyBecomeFriend(DataContext context, Character selfChar, Character targetChar, sbyte charBehaviorType, bool selfIsTaiwuPeople, bool targetIsTaiwuPeople)
	{
		int id = selfChar._id;
		int id2 = targetChar._id;
		if (RelationTypeHelper.AllowAddingFriendRelation(id2, id))
		{
			sbyte behaviorType = targetChar.GetBehaviorType();
			Location location = selfChar._location;
			int currDate = DomainManager.World.GetCurrDate();
			LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
			MonthlyNotificationCollection monthlyNotificationCollection = DomainManager.World.GetMonthlyNotificationCollection();
			DomainManager.Character.AddRelation(context, id, id2, 8192, currDate);
			DomainManager.Character.ChangeFavorabilityOptionalRepeatedEvent(context, selfChar, targetChar, 3000);
			DomainManager.Character.ChangeFavorabilityOptionalRepeatedEvent(context, targetChar, selfChar, 3000);
			selfChar.ChangeHappiness(context, AiHelper.RelationsRelatedConstants.BecomeFriendHappinessChange[charBehaviorType]);
			targetChar.ChangeHappiness(context, AiHelper.RelationsRelatedConstants.BecomeFriendHappinessChange[behaviorType]);
			lifeRecordCollection.AddBecomeFriend(id, currDate, id2, location);
			SecretInformationCollection secretInformationCollection = DomainManager.Information.GetSecretInformationCollection();
			int dataOffset = secretInformationCollection.AddBecomeFriend(selfChar._id, targetChar._id);
			int metaDataId = DomainManager.Information.AddSecretInformationMetaData(context, dataOffset);
			if (selfIsTaiwuPeople || targetIsTaiwuPeople)
			{
				monthlyNotificationCollection.AddBecomeFriend(id, location, id2);
				DomainManager.Information.ReceiveSecretInformation(context, metaDataId, DomainManager.Taiwu.GetTaiwuCharId(), selfIsTaiwuPeople ? selfChar._id : targetChar._id);
			}
			DomainManager.Character.AddFavorabilityChangeInstantNotification(targetChar, selfChar, isIncrease: true);
		}
	}

	public static void ApplySeverFriend(DataContext context, Character selfChar, Character targetChar, sbyte charBehaviorType, bool selfIsTaiwuPeople, bool targetIsTaiwuPeople)
	{
		int id = selfChar._id;
		int id2 = targetChar._id;
		if (DomainManager.Character.HasRelation(id2, id, 8192))
		{
			sbyte behaviorType = targetChar.GetBehaviorType();
			Location location = selfChar._location;
			int currDate = DomainManager.World.GetCurrDate();
			LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
			MonthlyNotificationCollection monthlyNotificationCollection = DomainManager.World.GetMonthlyNotificationCollection();
			DomainManager.Character.ChangeRelationType(context, id, id2, 8192, 0);
			DomainManager.Character.ChangeRelationType(context, id2, id, 8192, 0);
			DomainManager.Character.ChangeFavorabilityOptionalRepeatedEvent(context, selfChar, targetChar, -3000);
			DomainManager.Character.ChangeFavorabilityOptionalRepeatedEvent(context, targetChar, selfChar, -3000);
			selfChar.ChangeHappiness(context, AiHelper.RelationsRelatedConstants.SeverFriendHappinessChange[charBehaviorType]);
			targetChar.ChangeHappiness(context, AiHelper.RelationsRelatedConstants.SeverFriendHappinessChange[behaviorType]);
			lifeRecordCollection.AddSeverFriendship(id, currDate, id2, location);
			SecretInformationCollection secretInformationCollection = DomainManager.Information.GetSecretInformationCollection();
			int dataOffset = secretInformationCollection.AddSeverFriend(selfChar._id, targetChar._id);
			int metaDataId = DomainManager.Information.AddSecretInformationMetaData(context, dataOffset);
			if (selfIsTaiwuPeople || targetIsTaiwuPeople)
			{
				monthlyNotificationCollection.AddDecreaseFriendship(id, location, id2);
				DomainManager.Information.ReceiveSecretInformation(context, metaDataId, DomainManager.Taiwu.GetTaiwuCharId(), selfIsTaiwuPeople ? selfChar._id : targetChar._id);
			}
		}
	}

	public static void ApplyBecomeSwornBrotherOrSister(DataContext context, Character selfChar, Character targetChar, sbyte charBehaviorType, bool selfIsTaiwuPeople, bool targetIsTaiwuPeople)
	{
		int id = selfChar._id;
		int id2 = targetChar._id;
		if (RelationTypeHelper.AllowAddingSwornBrotherOrSisterRelation(id2, id))
		{
			sbyte behaviorType = targetChar.GetBehaviorType();
			Location location = selfChar._location;
			int currDate = DomainManager.World.GetCurrDate();
			LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
			MonthlyNotificationCollection monthlyNotificationCollection = DomainManager.World.GetMonthlyNotificationCollection();
			DomainManager.Character.AddRelation(context, id, id2, 512, currDate);
			DomainManager.Character.ChangeFavorabilityOptionalRepeatedEvent(context, selfChar, targetChar, 12000);
			DomainManager.Character.ChangeFavorabilityOptionalRepeatedEvent(context, targetChar, selfChar, 12000);
			selfChar.ChangeHappiness(context, AiHelper.RelationsRelatedConstants.BecomeSwornOrAdoptedFamilyHappinessChange[charBehaviorType]);
			targetChar.ChangeHappiness(context, AiHelper.RelationsRelatedConstants.BecomeSwornOrAdoptedFamilyHappinessChange[behaviorType]);
			lifeRecordCollection.AddBecomeSwornBrotherOrSister(id, currDate, id2, location);
			SecretInformationCollection secretInformationCollection = DomainManager.Information.GetSecretInformationCollection();
			int dataOffset = secretInformationCollection.AddBecomeSwornBrothersAndSisters(selfChar._id, targetChar._id);
			int metaDataId = DomainManager.Information.AddSecretInformationMetaData(context, dataOffset);
			if (selfIsTaiwuPeople || targetIsTaiwuPeople)
			{
				monthlyNotificationCollection.AddBecomeSwornBrotherOrSister(id, location, id2);
				DomainManager.Information.ReceiveSecretInformation(context, metaDataId, DomainManager.Taiwu.GetTaiwuCharId(), selfIsTaiwuPeople ? selfChar._id : targetChar._id);
			}
		}
	}

	public static void ApplySeverSwornBrotherOrSister(DataContext context, Character selfChar, Character targetChar, sbyte charBehaviorType, bool selfIsTaiwuPeople, bool targetIsTaiwuPeople)
	{
		int id = selfChar._id;
		int id2 = targetChar._id;
		if (DomainManager.Character.HasRelation(id2, id, 512))
		{
			sbyte behaviorType = targetChar.GetBehaviorType();
			Location location = selfChar._location;
			int currDate = DomainManager.World.GetCurrDate();
			LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
			MonthlyNotificationCollection monthlyNotificationCollection = DomainManager.World.GetMonthlyNotificationCollection();
			DomainManager.Character.ChangeRelationType(context, id, id2, 512, 0);
			DomainManager.Character.ChangeRelationType(context, id2, id, 512, 0);
			DomainManager.Character.ChangeFavorabilityOptionalRepeatedEvent(context, selfChar, targetChar, -12000);
			DomainManager.Character.ChangeFavorabilityOptionalRepeatedEvent(context, targetChar, selfChar, -12000);
			selfChar.ChangeHappiness(context, AiHelper.RelationsRelatedConstants.SeverSwornOrAdoptedFamilyHappinessChange[charBehaviorType]);
			targetChar.ChangeHappiness(context, AiHelper.RelationsRelatedConstants.SeverSwornOrAdoptedFamilyHappinessChange[behaviorType]);
			lifeRecordCollection.AddSeverSwornBrotherhood(id, currDate, id2, location);
			SecretInformationCollection secretInformationCollection = DomainManager.Information.GetSecretInformationCollection();
			int dataOffset = secretInformationCollection.AddSeverSwornBrothersAndSisters(selfChar._id, targetChar._id);
			int metaDataId = DomainManager.Information.AddSecretInformationMetaData(context, dataOffset);
			if (selfIsTaiwuPeople || targetIsTaiwuPeople)
			{
				monthlyNotificationCollection.AddSeverFriendship(id, location, id2);
				DomainManager.Information.ReceiveSecretInformation(context, metaDataId, DomainManager.Taiwu.GetTaiwuCharId(), selfIsTaiwuPeople ? selfChar._id : targetChar._id);
			}
		}
	}

	public static void ApplySeverHusbandOrWife(DataContext context, Character selfChar, Character targetChar, sbyte charBehaviorType, bool selfIsTaiwuPeople, bool targetIsTaiwuPeople)
	{
		int id = selfChar._id;
		int id2 = targetChar._id;
		if (DomainManager.Character.HasRelation(id2, id, 1024))
		{
			sbyte behaviorType = targetChar.GetBehaviorType();
			Location validLocation = selfChar.GetValidLocation();
			int currDate = DomainManager.World.GetCurrDate();
			LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
			MonthlyNotificationCollection monthlyNotificationCollection = DomainManager.World.GetMonthlyNotificationCollection();
			DomainManager.Character.ChangeRelationType(context, id, id2, 1024, 0);
			DomainManager.Character.ChangeRelationType(context, id2, id, 1024, 0);
			if (selfChar._organizationInfo.Principal)
			{
				DomainManager.Organization.TryDowngradeDeputySpouse(context, id, selfChar._organizationInfo, id2);
			}
			else
			{
				DomainManager.Organization.TryDowngradeDeputySpouse(context, id2, targetChar._organizationInfo, id);
			}
			DomainManager.Character.ChangeRelationType(context, id, id2, 16384, 0);
			DomainManager.Character.ChangeRelationType(context, id2, id, 16384, 0);
			DomainManager.Character.ChangeFavorabilityOptionalRepeatedEvent(context, selfChar, targetChar, -12000);
			DomainManager.Character.ChangeFavorabilityOptionalRepeatedEvent(context, targetChar, selfChar, -12000);
			selfChar.ChangeHappiness(context, AiHelper.RelationsRelatedConstants.SeverHusbandOrWifeHappinessChange[charBehaviorType]);
			targetChar.ChangeHappiness(context, AiHelper.RelationsRelatedConstants.SeverHusbandOrWifeHappinessChange[behaviorType]);
			lifeRecordCollection.AddDivorce(id, currDate, id2, validLocation);
		}
	}

	public static void ApplyAddRelation_Adore(DataContext context, Character selfChar, Character targetChar, sbyte charBehaviorType, bool targetLovesBack, bool selfIsTaiwuPeople, bool targetIsTaiwuPeople)
	{
		int id = selfChar._id;
		int id2 = targetChar._id;
		if (!RelationTypeHelper.AllowAddingAdoredRelation(id, id2))
		{
			return;
		}
		Location location = selfChar._location;
		LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
		MonthlyNotificationCollection monthlyNotificationCollection = DomainManager.World.GetMonthlyNotificationCollection();
		int currDate = DomainManager.World.GetCurrDate();
		DomainManager.Character.AddRelation(context, id, id2, 16384, currDate);
		lifeRecordCollection.AddAdore(id, currDate, id2, location);
		if (targetLovesBack && !DomainManager.Character.HasRelation(id2, id, 16384))
		{
			DomainManager.Character.AddRelation(context, id2, id, 16384);
			DomainManager.Character.ChangeFavorabilityOptionalRepeatedEvent(context, selfChar, targetChar, 3000);
			DomainManager.Character.ChangeFavorabilityOptionalRepeatedEvent(context, targetChar, selfChar, 3000);
			selfChar.ChangeHappiness(context, AiHelper.RelationsRelatedConstants.ConfessLoveSucceedHappinessChange[charBehaviorType]);
			targetChar.ChangeHappiness(context, AiHelper.RelationsRelatedConstants.ConfessLoveSucceedHappinessChange[targetChar.GetBehaviorType()]);
			lifeRecordCollection.AddAdore(id2, currDate, id, location);
			lifeRecordCollection.AddLoveAtFirstSight(id, currDate, id2, location);
			SecretInformationCollection secretInformationCollection = DomainManager.Information.GetSecretInformationCollection();
			int dataOffset = secretInformationCollection.AddBecomeLover(selfChar._id, targetChar._id);
			int metaDataId = DomainManager.Information.AddSecretInformationMetaData(context, dataOffset);
			if (selfIsTaiwuPeople || targetIsTaiwuPeople)
			{
				monthlyNotificationCollection.AddConfessLoveAndSucceed(id, location, id2);
				DomainManager.Information.ReceiveSecretInformation(context, metaDataId, DomainManager.Taiwu.GetTaiwuCharId(), selfIsTaiwuPeople ? selfChar._id : targetChar._id);
			}
			if (context.Random.CheckPercentProb(AiHelper.RelationsRelatedConstants.ConfessLoveSucceedNeedForSexChance[charBehaviorType]))
			{
				GameData.Domains.Character.Ai.PersonalNeed personalNeed = GameData.Domains.Character.Ai.PersonalNeed.CreatePersonalNeed(23, id2);
				selfChar.AddPersonalNeed(context, personalNeed);
			}
		}
	}

	public static void ApplySeverAdore(DataContext context, Character selfChar, Character targetChar, sbyte charBehaviorType, bool selfIsTaiwuPeople)
	{
		int currDate = DomainManager.World.GetCurrDate();
		int id = selfChar._id;
		int id2 = targetChar._id;
		if (DomainManager.Character.HasRelation(id, id2, 16384))
		{
			Location location = selfChar._location;
			DomainManager.Character.ChangeRelationType(context, id, id2, 16384, 0);
			selfChar.ChangeHappiness(context, AiHelper.RelationsRelatedConstants.ConfessLoveFailedHappinessChange[charBehaviorType]);
			DomainManager.Character.ChangeFavorabilityOptionalRepeatedEvent(context, selfChar, targetChar, -3000);
			DomainManager.LifeRecord.GetLifeRecordCollection().AddEndAdored(id, currDate, id2, location);
			if (context.Random.CheckPercentProb(AiHelper.RelationsRelatedConstants.BecomeSingleNeedNewLoveChance[charBehaviorType]))
			{
				GameData.Domains.Character.Ai.PersonalNeed personalNeed = GameData.Domains.Character.Ai.PersonalNeed.CreatePersonalNeed((sbyte)25, (ushort)16384);
				selfChar.AddPersonalNeed(context, personalNeed);
			}
		}
	}

	public static void ApplyBecomeBoyOrGirlFriend(DataContext context, Character selfChar, Character targetChar, sbyte charBehaviorType, bool succeed, bool selfIsTaiwuPeople, bool targetIsTaiwuPeople)
	{
		int id = selfChar._id;
		int id2 = targetChar._id;
		if (!RelationTypeHelper.AllowAddingAdoredRelation(id2, id))
		{
			return;
		}
		Location location = selfChar._location;
		LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
		MonthlyNotificationCollection monthlyNotificationCollection = DomainManager.World.GetMonthlyNotificationCollection();
		int currDate = DomainManager.World.GetCurrDate();
		if (succeed)
		{
			DomainManager.Character.AddRelation(context, id2, id, 16384);
			DomainManager.Character.ChangeFavorabilityOptionalRepeatedEvent(context, selfChar, targetChar, 3000);
			DomainManager.Character.ChangeFavorabilityOptionalRepeatedEvent(context, targetChar, selfChar, 3000);
			selfChar.ChangeHappiness(context, AiHelper.RelationsRelatedConstants.ConfessLoveSucceedHappinessChange[charBehaviorType]);
			lifeRecordCollection.AddConfessLoveSucceed(id, currDate, id2, location);
			SecretInformationCollection secretInformationCollection = DomainManager.Information.GetSecretInformationCollection();
			int dataOffset = secretInformationCollection.AddBecomeLover(selfChar._id, targetChar._id);
			int metaDataId = DomainManager.Information.AddSecretInformationMetaData(context, dataOffset);
			if (selfIsTaiwuPeople || targetIsTaiwuPeople)
			{
				monthlyNotificationCollection.AddConfessLoveAndSucceed(id, location, id2);
				DomainManager.Information.ReceiveSecretInformation(context, metaDataId, DomainManager.Taiwu.GetTaiwuCharId(), selfIsTaiwuPeople ? selfChar._id : targetChar._id);
			}
			if (context.Random.CheckPercentProb(AiHelper.RelationsRelatedConstants.ConfessLoveSucceedNeedForSexChance[charBehaviorType]))
			{
				GameData.Domains.Character.Ai.PersonalNeed personalNeed = GameData.Domains.Character.Ai.PersonalNeed.CreatePersonalNeed(23, id2);
				selfChar.AddPersonalNeed(context, personalNeed);
			}
		}
		else
		{
			DomainManager.Character.ChangeFavorabilityOptionalRepeatedEvent(context, selfChar, targetChar, -3000);
			DomainManager.Character.ChangeFavorabilityOptionalRepeatedEvent(context, targetChar, selfChar, -3000);
			selfChar.ChangeHappiness(context, AiHelper.RelationsRelatedConstants.ConfessLoveFailedHappinessChange[charBehaviorType]);
			lifeRecordCollection.AddConfessLoveFail(id, currDate, id2, location);
			if (context.Random.CheckPercentProb(AiHelper.RelationsRelatedConstants.BecomeSingleNeedNewLoveChance[charBehaviorType]))
			{
				GameData.Domains.Character.Ai.PersonalNeed personalNeed2 = GameData.Domains.Character.Ai.PersonalNeed.CreatePersonalNeed((sbyte)25, (ushort)16384);
				selfChar.AddPersonalNeed(context, personalNeed2);
			}
			if (context.Random.CheckPercentProb(AiHelper.RelationsRelatedConstants.ConfessLoveFailedNeedForRapeChance[charBehaviorType]))
			{
				GameData.Domains.Character.Ai.PersonalNeed personalNeed3 = GameData.Domains.Character.Ai.PersonalNeed.CreatePersonalNeed(23, id2);
				selfChar.AddPersonalNeed(context, personalNeed3);
			}
			if (context.Random.CheckPercentProb(AiHelper.RelationsRelatedConstants.ConfessLoveOrProposeFailedBecomeEnemyChance[charBehaviorType]))
			{
				ApplyAddRelation_Enemy(context, selfChar, targetChar, selfIsTaiwuPeople, 9);
			}
		}
	}

	public static void ApplyBreakupWithBoyOrGirlFriend(DataContext context, Character selfChar, Character targetChar, sbyte charBehaviorType, bool targetStillLoveSelf, bool selfIsTaiwuPeople, bool targetIsTaiwuPeople)
	{
		int id = selfChar._id;
		int id2 = targetChar._id;
		if (DomainManager.Character.HasRelation(id2, id, 16384))
		{
			Location location = selfChar._location;
			int currDate = DomainManager.World.GetCurrDate();
			sbyte behaviorType = targetChar.GetBehaviorType();
			DomainManager.Character.ChangeRelationType(context, id, id2, 16384, 0);
			DomainManager.Character.ChangeFavorabilityOptionalRepeatedEvent(context, selfChar, targetChar, -12000);
			DomainManager.Character.ChangeFavorabilityOptionalRepeatedEvent(context, targetChar, selfChar, -12000);
			selfChar.ChangeHappiness(context, AiHelper.RelationsRelatedConstants.ConfessLoveFailedHappinessChange[charBehaviorType]);
			SecretInformationCollection secretInformationCollection = DomainManager.Information.GetSecretInformationCollection();
			int dataOffset = secretInformationCollection.AddBreakupWithLover(selfChar._id, targetChar._id);
			int metaDataId = DomainManager.Information.AddSecretInformationMetaData(context, dataOffset);
			if (selfIsTaiwuPeople || targetIsTaiwuPeople)
			{
				DomainManager.World.GetMonthlyNotificationCollection().AddSeverLove(id, location, id2);
				DomainManager.Information.ReceiveSecretInformation(context, metaDataId, DomainManager.Taiwu.GetTaiwuCharId(), selfIsTaiwuPeople ? selfChar._id : targetChar._id);
			}
			if (targetStillLoveSelf)
			{
				DomainManager.LifeRecord.GetLifeRecordCollection().AddDumpLover(id, currDate, id2, location);
			}
			else
			{
				DomainManager.LifeRecord.GetLifeRecordCollection().AddBreakupMutually(id, currDate, id2, location);
				DomainManager.Character.ChangeRelationType(context, id2, id, 16384, 0);
				targetChar.ChangeHappiness(context, AiHelper.RelationsRelatedConstants.ConfessLoveFailedHappinessChange[behaviorType]);
			}
			if (context.Random.CheckPercentProb(AiHelper.RelationsRelatedConstants.BecomeSingleNeedNewLoveChance[charBehaviorType]))
			{
				GameData.Domains.Character.Ai.PersonalNeed personalNeed = GameData.Domains.Character.Ai.PersonalNeed.CreatePersonalNeed((sbyte)25, (ushort)16384);
				selfChar.AddPersonalNeed(context, personalNeed);
			}
			if (context.Random.CheckPercentProb(AiHelper.RelationsRelatedConstants.BreakupBecomeEnemyChance[behaviorType]))
			{
				ApplyAddRelation_Enemy(context, targetChar, selfChar, targetIsTaiwuPeople, 3);
			}
			if (selfIsTaiwuPeople)
			{
				InteractOfLove.LeaveLover(context, targetChar.GetId());
			}
			else if (targetIsTaiwuPeople)
			{
				InteractOfLove.LeaveLover(context, selfChar.GetId());
			}
		}
	}

	public static void ApplyBecomeHusbandOrWife(DataContext context, Character selfChar, Character targetChar, sbyte charBehaviorType, bool succeed, bool selfIsTaiwuPeople, bool targetIsTaiwuPeople)
	{
		int id = selfChar._id;
		int id2 = targetChar._id;
		if (!RelationTypeHelper.AllowAddingHusbandOrWifeRelation(id2, id))
		{
			return;
		}
		sbyte behaviorType = targetChar.GetBehaviorType();
		Location location = selfChar._location;
		int currDate = DomainManager.World.GetCurrDate();
		LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
		MonthlyNotificationCollection monthlyNotificationCollection = DomainManager.World.GetMonthlyNotificationCollection();
		if (succeed)
		{
			DomainManager.Character.AddHusbandOrWifeRelations(context, id, id2, currDate);
			DomainManager.Character.ChangeFavorabilityOptionalRepeatedEvent(context, selfChar, targetChar, 12000);
			DomainManager.Character.ChangeFavorabilityOptionalRepeatedEvent(context, targetChar, selfChar, 12000);
			selfChar.ChangeHappiness(context, AiHelper.RelationsRelatedConstants.ProposeMarriageSucceedHappinessChange[charBehaviorType]);
			lifeRecordCollection.AddProposeMarriageSucceed(id, currDate, id2, location);
			SecretInformationCollection secretInformationCollection = DomainManager.Information.GetSecretInformationCollection();
			int dataOffset = secretInformationCollection.AddBecomeHusbandAndWife(selfChar._id, targetChar._id);
			int metaDataId = DomainManager.Information.AddSecretInformationMetaData(context, dataOffset);
			if (selfIsTaiwuPeople || targetIsTaiwuPeople)
			{
				monthlyNotificationCollection.AddMarriage(id, location, id2);
				DomainManager.Information.ReceiveSecretInformation(context, metaDataId, DomainManager.Taiwu.GetTaiwuCharId(), selfIsTaiwuPeople ? selfChar._id : targetChar._id);
			}
			DomainManager.Organization.UpdateOrganizationAfterMarriage(context, selfChar, targetChar);
			GameData.Domains.Character.Ai.PersonalNeed personalNeed = GameData.Domains.Character.Ai.PersonalNeed.CreatePersonalNeed(23, id2);
			selfChar.AddPersonalNeed(context, personalNeed);
		}
		else
		{
			DomainManager.Character.ChangeFavorabilityOptionalRepeatedEvent(context, selfChar, targetChar, -3000);
			DomainManager.Character.ChangeFavorabilityOptionalRepeatedEvent(context, targetChar, selfChar, -3000);
			selfChar.ChangeHappiness(context, AiHelper.RelationsRelatedConstants.ProposeMarriageFailHappinessChange[charBehaviorType]);
			lifeRecordCollection.AddProposeMarriageFail(id, currDate, id2, location);
			if (context.Random.CheckPercentProb(AiHelper.RelationsRelatedConstants.BecomeSingleNeedNewLoveChance[charBehaviorType]))
			{
				GameData.Domains.Character.Ai.PersonalNeed personalNeed2 = GameData.Domains.Character.Ai.PersonalNeed.CreatePersonalNeed((sbyte)25, (ushort)16384);
				selfChar.AddPersonalNeed(context, personalNeed2);
			}
			if (context.Random.CheckPercentProb(AiHelper.RelationsRelatedConstants.ConfessLoveOrProposeFailedBecomeEnemyChance[charBehaviorType]))
			{
				ApplyAddRelation_Enemy(context, selfChar, targetChar, selfIsTaiwuPeople, 4);
			}
		}
	}

	public static void ApplyEndRelation_AdoptiveParent(DataContext context, Character selfChar, Character targetChar, sbyte charBehaviorType, bool selfIsTaiwuPeople, bool targetIsTaiwuPeople)
	{
		int id = selfChar._id;
		int id2 = targetChar._id;
		if (DomainManager.Character.HasRelation(id, id2, 64))
		{
			sbyte behaviorType = targetChar.GetBehaviorType();
			Location location = selfChar._location;
			int currDate = DomainManager.World.GetCurrDate();
			LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
			MonthlyNotificationCollection monthlyNotificationCollection = DomainManager.World.GetMonthlyNotificationCollection();
			DomainManager.Character.ChangeRelationType(context, id, id2, 64, 0);
			DomainManager.Character.ChangeRelationType(context, id2, id, 128, 0);
			DomainManager.Character.ChangeFavorabilityOptionalRepeatedEvent(context, selfChar, targetChar, -12000);
			DomainManager.Character.ChangeFavorabilityOptionalRepeatedEvent(context, targetChar, selfChar, -12000);
			selfChar.ChangeHappiness(context, AiHelper.RelationsRelatedConstants.SeverSwornOrAdoptedFamilyHappinessChange[charBehaviorType]);
			targetChar.ChangeHappiness(context, AiHelper.RelationsRelatedConstants.SeverSwornOrAdoptedFamilyHappinessChange[behaviorType]);
			lifeRecordCollection.AddSeverAdoptiveParent(id, currDate, id2, location);
			if (!(selfIsTaiwuPeople || targetIsTaiwuPeople))
			{
			}
		}
	}

	public static void ApplyEndRelation_AdoptiveChild(DataContext context, Character selfChar, Character targetChar, sbyte charBehaviorType, bool selfIsTaiwuPeople, bool targetIsTaiwuPeople)
	{
		ApplyEndRelation_AdoptiveParent(context, targetChar, selfChar, targetChar.GetBehaviorType(), targetIsTaiwuPeople, selfIsTaiwuPeople);
	}

	public static void ApplyAddRelation_AdoptiveParent(DataContext context, Character selfChar, Character targetChar, sbyte charBehaviorType, bool selfIsTaiwuPeople, bool targetIsTaiwuPeople)
	{
		int id = selfChar._id;
		int id2 = targetChar._id;
		if (!RelationTypeHelper.AllowAddingAdoptiveParentRelation(id, id2))
		{
			return;
		}
		sbyte behaviorType = targetChar.GetBehaviorType();
		Location location = selfChar._location;
		int currDate = DomainManager.World.GetCurrDate();
		LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
		MonthlyNotificationCollection monthlyNotificationCollection = DomainManager.World.GetMonthlyNotificationCollection();
		DomainManager.Character.AddAdoptiveParentRelations(context, id, id2, currDate);
		int aliveSpouse = DomainManager.Character.GetAliveSpouse(id2);
		if (aliveSpouse >= 0)
		{
			DomainManager.Character.AddAdoptiveParentRelations(context, id, aliveSpouse, currDate);
		}
		DomainManager.Character.ChangeFavorabilityOptionalRepeatedEvent(context, selfChar, targetChar, 12000);
		DomainManager.Character.ChangeFavorabilityOptionalRepeatedEvent(context, targetChar, selfChar, 12000);
		selfChar.ChangeHappiness(context, AiHelper.RelationsRelatedConstants.BecomeSwornOrAdoptedFamilyHappinessChange[charBehaviorType]);
		targetChar.ChangeHappiness(context, AiHelper.RelationsRelatedConstants.BecomeSwornOrAdoptedFamilyHappinessChange[behaviorType]);
		short relatedRecordId = (short)((targetChar.GetGender() == 1) ? 46 : 47);
		if (selfChar.GetGender() == 1)
		{
			lifeRecordCollection.AddAdoptSon(id2, currDate, id, location, relatedRecordId);
		}
		else
		{
			lifeRecordCollection.AddAdoptDaughter(id2, currDate, id, location, relatedRecordId);
		}
		SecretInformationCollection secretInformationCollection = DomainManager.Information.GetSecretInformationCollection();
		int dataOffset = secretInformationCollection.AddGetAdopted(selfChar._id, targetChar._id);
		int metaDataId = DomainManager.Information.AddSecretInformationMetaData(context, dataOffset);
		if (selfIsTaiwuPeople)
		{
			if (targetChar.GetGender() == 1)
			{
				monthlyNotificationCollection.AddRecognizeFather(id, location, id2);
			}
			else
			{
				monthlyNotificationCollection.AddRecognizeMother(id, location, id2);
			}
			DomainManager.Information.ReceiveSecretInformation(context, metaDataId, DomainManager.Taiwu.GetTaiwuCharId(), selfChar._id);
		}
		dataOffset = secretInformationCollection.AddAdoptChild(targetChar._id, selfChar._id);
		metaDataId = DomainManager.Information.AddSecretInformationMetaData(context, dataOffset);
		if (targetIsTaiwuPeople)
		{
			if (selfChar.GetGender() == 1)
			{
				monthlyNotificationCollection.AddAdoptBoy(id2, location, id);
			}
			else
			{
				monthlyNotificationCollection.AddAdoptGirl(id2, location, id);
			}
			DomainManager.Information.ReceiveSecretInformation(context, metaDataId, DomainManager.Taiwu.GetTaiwuCharId(), targetChar._id);
		}
	}

	public static void ApplyAddRelation_AdoptiveChild(DataContext context, Character selfChar, Character targetChar, sbyte charBehaviorType, bool selfIsTaiwuPeople, bool targetIsTaiwuPeople)
	{
		int id = selfChar._id;
		int id2 = targetChar._id;
		if (!RelationTypeHelper.AllowAddingAdoptiveChildRelation(id, id2))
		{
			return;
		}
		sbyte behaviorType = targetChar.GetBehaviorType();
		Location location = selfChar._location;
		int currDate = DomainManager.World.GetCurrDate();
		LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
		MonthlyNotificationCollection monthlyNotificationCollection = DomainManager.World.GetMonthlyNotificationCollection();
		DomainManager.Character.AddAdoptiveParentRelations(context, id2, id, currDate);
		int aliveSpouse = DomainManager.Character.GetAliveSpouse(id);
		if (aliveSpouse >= 0)
		{
			DomainManager.Character.AddAdoptiveParentRelations(context, id2, aliveSpouse, currDate);
		}
		DomainManager.Character.ChangeFavorabilityOptionalRepeatedEvent(context, selfChar, targetChar, 12000);
		DomainManager.Character.ChangeFavorabilityOptionalRepeatedEvent(context, targetChar, selfChar, 12000);
		selfChar.ChangeHappiness(context, AiHelper.RelationsRelatedConstants.BecomeSwornOrAdoptedFamilyHappinessChange[charBehaviorType]);
		targetChar.ChangeHappiness(context, AiHelper.RelationsRelatedConstants.BecomeSwornOrAdoptedFamilyHappinessChange[behaviorType]);
		short relatedRecordId = (short)((selfChar.GetGender() == 1) ? 46 : 47);
		if (targetChar.GetGender() == 1)
		{
			lifeRecordCollection.AddAdoptSon(id, currDate, id2, location, relatedRecordId);
		}
		else
		{
			lifeRecordCollection.AddAdoptDaughter(id, currDate, id2, location, relatedRecordId);
		}
		SecretInformationCollection secretInformationCollection = DomainManager.Information.GetSecretInformationCollection();
		int dataOffset = secretInformationCollection.AddAdoptChild(selfChar._id, targetChar._id);
		int metaDataId = DomainManager.Information.AddSecretInformationMetaData(context, dataOffset);
		if (selfIsTaiwuPeople)
		{
			if (targetChar.GetGender() == 1)
			{
				monthlyNotificationCollection.AddAdoptBoy(id, location, id2);
			}
			else
			{
				monthlyNotificationCollection.AddAdoptGirl(id, location, id2);
			}
			DomainManager.Information.ReceiveSecretInformation(context, metaDataId, DomainManager.Taiwu.GetTaiwuCharId(), selfChar._id);
		}
		dataOffset = secretInformationCollection.AddGetAdopted(targetChar._id, selfChar._id);
		metaDataId = DomainManager.Information.AddSecretInformationMetaData(context, dataOffset);
		if (targetIsTaiwuPeople)
		{
			if (selfChar.GetGender() == 1)
			{
				monthlyNotificationCollection.AddRecognizeFather(id2, location, id);
			}
			else
			{
				monthlyNotificationCollection.AddRecognizeMother(id2, location, id);
			}
			DomainManager.Information.ReceiveSecretInformation(context, metaDataId, DomainManager.Taiwu.GetTaiwuCharId(), targetChar._id);
		}
	}

	public static void ApplyAddRelation_Mentor(DataContext context, Character selfChar, Character targetChar, sbyte charBehaviorType, bool selfIsTaiwuPeople, bool targetIsTaiwuPeople)
	{
		int id = selfChar._id;
		int id2 = targetChar._id;
		if (RelationTypeHelper.AllowAddingMentorRelation(id, id2))
		{
			sbyte behaviorType = targetChar.GetBehaviorType();
			Location location = selfChar._location;
			int currDate = DomainManager.World.GetCurrDate();
			LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
			MonthlyNotificationCollection monthlyNotificationCollection = DomainManager.World.GetMonthlyNotificationCollection();
			DomainManager.Character.AddRelation(context, id, id2, 2048);
			DomainManager.Character.ChangeFavorabilityOptionalRepeatedEvent(context, selfChar, targetChar, 3000);
			DomainManager.Character.ChangeFavorabilityOptionalRepeatedEvent(context, targetChar, selfChar, 3000);
			selfChar.ChangeHappiness(context, AiHelper.RelationsRelatedConstants.BecomeMentorHappinessChange[charBehaviorType]);
			targetChar.ChangeHappiness(context, AiHelper.RelationsRelatedConstants.BecomeMentorHappinessChange[behaviorType]);
			lifeRecordCollection.AddGetMentor(id, currDate, id2, location);
			if (!(selfIsTaiwuPeople || targetIsTaiwuPeople))
			{
			}
		}
	}

	public static void ApplyEndRelation_Mentor(DataContext context, Character selfChar, Character targetChar, sbyte charBehaviorType, bool selfIsTaiwuPeople, bool targetIsTaiwuPeople)
	{
		int id = selfChar._id;
		int id2 = targetChar._id;
		if (DomainManager.Character.HasRelation(id, id2, 2048))
		{
			sbyte behaviorType = targetChar.GetBehaviorType();
			Location location = selfChar._location;
			int currDate = DomainManager.World.GetCurrDate();
			LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
			MonthlyNotificationCollection monthlyNotificationCollection = DomainManager.World.GetMonthlyNotificationCollection();
			DomainManager.Character.ChangeRelationType(context, id, id2, 2048, 0);
			DomainManager.Character.ChangeRelationType(context, id2, id, 4096, 0);
			DomainManager.Character.ChangeFavorabilityOptionalRepeatedEvent(context, selfChar, targetChar, -3000);
			DomainManager.Character.ChangeFavorabilityOptionalRepeatedEvent(context, targetChar, selfChar, -3000);
			selfChar.ChangeHappiness(context, -AiHelper.RelationsRelatedConstants.BecomeMentorHappinessChange[charBehaviorType]);
			targetChar.ChangeHappiness(context, -AiHelper.RelationsRelatedConstants.BecomeMentorHappinessChange[behaviorType]);
			lifeRecordCollection.AddSeverMentor(id, currDate, id2, location);
			if (!(selfIsTaiwuPeople || targetIsTaiwuPeople))
			{
			}
		}
	}

	public int GetId()
	{
		return _id;
	}

	public short GetTemplateId()
	{
		return _templateId;
	}

	public byte GetCreatingType()
	{
		return _creatingType;
	}

	public sbyte GetGender()
	{
		return _gender;
	}

	public short GetActualAge()
	{
		return _actualAge;
	}

	public unsafe void SetActualAge(short actualAge, DataContext context)
	{
		_actualAge = actualAge;
		SetModifiedAndInvalidateInfluencedCache(4, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 8u, 2);
			*(short*)ptr = _actualAge;
			ptr += 2;
		}
	}

	public sbyte GetBirthMonth()
	{
		return _birthMonth;
	}

	public sbyte GetHappiness()
	{
		return _happiness;
	}

	public unsafe void SetHappiness(sbyte happiness, DataContext context)
	{
		_happiness = happiness;
		SetModifiedAndInvalidateInfluencedCache(6, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 11u, 1);
			*ptr = (byte)_happiness;
			ptr++;
		}
	}

	public short GetBaseMorality()
	{
		return _baseMorality;
	}

	public unsafe void SetBaseMorality(short baseMorality, DataContext context)
	{
		_baseMorality = baseMorality;
		SetModifiedAndInvalidateInfluencedCache(7, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 12u, 2);
			*(short*)ptr = _baseMorality;
			ptr += 2;
		}
	}

	public OrganizationInfo GetOrganizationInfo()
	{
		return _organizationInfo;
	}

	public unsafe void SetOrganizationInfo(OrganizationInfo organizationInfo, DataContext context)
	{
		_organizationInfo = organizationInfo;
		SetModifiedAndInvalidateInfluencedCache(8, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 14u, 8);
			ptr += _organizationInfo.Serialize(ptr);
		}
	}

	public sbyte GetIdealSect()
	{
		return _idealSect;
	}

	public unsafe void SetIdealSect(sbyte idealSect, DataContext context)
	{
		_idealSect = idealSect;
		SetModifiedAndInvalidateInfluencedCache(9, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 22u, 1);
			*ptr = (byte)_idealSect;
			ptr++;
		}
	}

	public sbyte GetLifeSkillTypeInterest()
	{
		return _lifeSkillTypeInterest;
	}

	public unsafe void SetLifeSkillTypeInterest(sbyte lifeSkillTypeInterest, DataContext context)
	{
		_lifeSkillTypeInterest = lifeSkillTypeInterest;
		SetModifiedAndInvalidateInfluencedCache(10, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 23u, 1);
			*ptr = (byte)_lifeSkillTypeInterest;
			ptr++;
		}
	}

	public sbyte GetCombatSkillTypeInterest()
	{
		return _combatSkillTypeInterest;
	}

	public unsafe void SetCombatSkillTypeInterest(sbyte combatSkillTypeInterest, DataContext context)
	{
		_combatSkillTypeInterest = combatSkillTypeInterest;
		SetModifiedAndInvalidateInfluencedCache(11, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 24u, 1);
			*ptr = (byte)_combatSkillTypeInterest;
			ptr++;
		}
	}

	public sbyte GetMainAttributeInterest()
	{
		return _mainAttributeInterest;
	}

	public unsafe void SetMainAttributeInterest(sbyte mainAttributeInterest, DataContext context)
	{
		_mainAttributeInterest = mainAttributeInterest;
		SetModifiedAndInvalidateInfluencedCache(12, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 25u, 1);
			*ptr = (byte)_mainAttributeInterest;
			ptr++;
		}
	}

	public bool GetTransgender()
	{
		return _transgender;
	}

	public bool GetBisexual()
	{
		return _bisexual;
	}

	public sbyte GetXiangshuType()
	{
		return _xiangshuType;
	}

	public byte GetMonkType()
	{
		return _monkType;
	}

	public unsafe void SetMonkType(byte monkType, DataContext context)
	{
		_monkType = monkType;
		SetModifiedAndInvalidateInfluencedCache(16, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 29u, 1);
			*ptr = _monkType;
			ptr++;
		}
	}

	public List<short> GetFeatureIds()
	{
		return _featureIds;
	}

	public unsafe void SetFeatureIds(List<short> featureIds, DataContext context)
	{
		_featureIds = featureIds;
		SetModifiedAndInvalidateInfluencedCache(17, context);
		if (CollectionHelperData.IsArchive)
		{
			int count = _featureIds.Count;
			int num = 2 * count;
			int valueSize = 2 + num;
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 0, valueSize);
			*(ushort*)ptr = (ushort)count;
			ptr += 2;
			for (int i = 0; i < count; i++)
			{
				((short*)ptr)[i] = _featureIds[i];
			}
			ptr += num;
		}
	}

	public MainAttributes GetBaseMainAttributes()
	{
		return _baseMainAttributes;
	}

	public unsafe void SetBaseMainAttributes(MainAttributes baseMainAttributes, DataContext context)
	{
		_baseMainAttributes = baseMainAttributes;
		SetModifiedAndInvalidateInfluencedCache(18, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 30u, 12);
			ptr += _baseMainAttributes.Serialize(ptr);
		}
	}

	public short GetHealth()
	{
		return _health;
	}

	public unsafe void SetHealth(short health, DataContext context)
	{
		_health = health;
		SetModifiedAndInvalidateInfluencedCache(19, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 42u, 2);
			*(short*)ptr = _health;
			ptr += 2;
		}
	}

	public short GetBaseMaxHealth()
	{
		return _baseMaxHealth;
	}

	public unsafe void SetBaseMaxHealth(short baseMaxHealth, DataContext context)
	{
		_baseMaxHealth = baseMaxHealth;
		SetModifiedAndInvalidateInfluencedCache(20, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 44u, 2);
			*(short*)ptr = _baseMaxHealth;
			ptr += 2;
		}
	}

	public short GetDisorderOfQi()
	{
		return _disorderOfQi;
	}

	public unsafe void SetDisorderOfQi(short disorderOfQi, DataContext context)
	{
		_disorderOfQi = disorderOfQi;
		SetModifiedAndInvalidateInfluencedCache(21, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 46u, 2);
			*(short*)ptr = _disorderOfQi;
			ptr += 2;
		}
	}

	public bool GetHaveLeftArm()
	{
		return _haveLeftArm;
	}

	public unsafe void SetHaveLeftArm(bool haveLeftArm, DataContext context)
	{
		_haveLeftArm = haveLeftArm;
		SetModifiedAndInvalidateInfluencedCache(22, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 48u, 1);
			*ptr = (_haveLeftArm ? ((byte)1) : ((byte)0));
			ptr++;
		}
	}

	public bool GetHaveRightArm()
	{
		return _haveRightArm;
	}

	public unsafe void SetHaveRightArm(bool haveRightArm, DataContext context)
	{
		_haveRightArm = haveRightArm;
		SetModifiedAndInvalidateInfluencedCache(23, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 49u, 1);
			*ptr = (_haveRightArm ? ((byte)1) : ((byte)0));
			ptr++;
		}
	}

	public bool GetHaveLeftLeg()
	{
		return _haveLeftLeg;
	}

	public unsafe void SetHaveLeftLeg(bool haveLeftLeg, DataContext context)
	{
		_haveLeftLeg = haveLeftLeg;
		SetModifiedAndInvalidateInfluencedCache(24, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 50u, 1);
			*ptr = (_haveLeftLeg ? ((byte)1) : ((byte)0));
			ptr++;
		}
	}

	public bool GetHaveRightLeg()
	{
		return _haveRightLeg;
	}

	public unsafe void SetHaveRightLeg(bool haveRightLeg, DataContext context)
	{
		_haveRightLeg = haveRightLeg;
		SetModifiedAndInvalidateInfluencedCache(25, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 51u, 1);
			*ptr = (_haveRightLeg ? ((byte)1) : ((byte)0));
			ptr++;
		}
	}

	public Injuries GetInjuries()
	{
		return _injuries;
	}

	public unsafe void SetInjuries(Injuries injuries, DataContext context)
	{
		_injuries = injuries;
		SetModifiedAndInvalidateInfluencedCache(26, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 52u, 16);
			ptr += _injuries.Serialize(ptr);
		}
	}

	public int GetExtraNeili()
	{
		return _extraNeili;
	}

	public unsafe void SetExtraNeili(int extraNeili, DataContext context)
	{
		_extraNeili = extraNeili;
		SetModifiedAndInvalidateInfluencedCache(27, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 68u, 4);
			*(int*)ptr = _extraNeili;
			ptr += 4;
		}
	}

	public sbyte GetConsummateLevel()
	{
		return _consummateLevel;
	}

	public unsafe void SetConsummateLevel(sbyte consummateLevel, DataContext context)
	{
		_consummateLevel = consummateLevel;
		SetModifiedAndInvalidateInfluencedCache(28, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 72u, 1);
			*ptr = (byte)_consummateLevel;
			ptr++;
		}
	}

	public List<LifeSkillItem> GetLearnedLifeSkills()
	{
		return _learnedLifeSkills;
	}

	public unsafe void SetLearnedLifeSkills(List<LifeSkillItem> learnedLifeSkills, DataContext context)
	{
		_learnedLifeSkills = learnedLifeSkills;
		SetModifiedAndInvalidateInfluencedCache(29, context);
		if (CollectionHelperData.IsArchive)
		{
			int count = _learnedLifeSkills.Count;
			int num = 4 * count;
			int valueSize = 2 + num;
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 1, valueSize);
			*(ushort*)ptr = (ushort)count;
			ptr += 2;
			for (int i = 0; i < count; i++)
			{
				ptr += _learnedLifeSkills[i].Serialize(ptr);
			}
		}
	}

	public ref LifeSkillShorts GetBaseLifeSkillQualifications()
	{
		return ref _baseLifeSkillQualifications;
	}

	public unsafe void SetBaseLifeSkillQualifications(ref LifeSkillShorts baseLifeSkillQualifications, DataContext context)
	{
		_baseLifeSkillQualifications = baseLifeSkillQualifications;
		SetModifiedAndInvalidateInfluencedCache(30, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 73u, 32);
			ptr += _baseLifeSkillQualifications.Serialize(ptr);
		}
	}

	public sbyte GetLifeSkillQualificationGrowthType()
	{
		return _lifeSkillQualificationGrowthType;
	}

	public unsafe void SetLifeSkillQualificationGrowthType(sbyte lifeSkillQualificationGrowthType, DataContext context)
	{
		_lifeSkillQualificationGrowthType = lifeSkillQualificationGrowthType;
		SetModifiedAndInvalidateInfluencedCache(31, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 105u, 1);
			*ptr = (byte)_lifeSkillQualificationGrowthType;
			ptr++;
		}
	}

	public ref CombatSkillShorts GetBaseCombatSkillQualifications()
	{
		return ref _baseCombatSkillQualifications;
	}

	public unsafe void SetBaseCombatSkillQualifications(ref CombatSkillShorts baseCombatSkillQualifications, DataContext context)
	{
		_baseCombatSkillQualifications = baseCombatSkillQualifications;
		SetModifiedAndInvalidateInfluencedCache(32, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 106u, 28);
			ptr += _baseCombatSkillQualifications.Serialize(ptr);
		}
	}

	public sbyte GetCombatSkillQualificationGrowthType()
	{
		return _combatSkillQualificationGrowthType;
	}

	public unsafe void SetCombatSkillQualificationGrowthType(sbyte combatSkillQualificationGrowthType, DataContext context)
	{
		_combatSkillQualificationGrowthType = combatSkillQualificationGrowthType;
		SetModifiedAndInvalidateInfluencedCache(33, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 134u, 1);
			*ptr = (byte)_combatSkillQualificationGrowthType;
			ptr++;
		}
	}

	public ref ResourceInts GetResources()
	{
		return ref _resources;
	}

	public unsafe void SetResources(ref ResourceInts resources, DataContext context)
	{
		_resources = resources;
		SetModifiedAndInvalidateInfluencedCache(34, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 135u, 32);
			ptr += _resources.Serialize(ptr);
		}
	}

	public short GetLovingItemSubType()
	{
		return _lovingItemSubType;
	}

	public unsafe void SetLovingItemSubType(short lovingItemSubType, DataContext context)
	{
		_lovingItemSubType = lovingItemSubType;
		SetModifiedAndInvalidateInfluencedCache(35, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 167u, 2);
			*(short*)ptr = _lovingItemSubType;
			ptr += 2;
		}
	}

	public short GetHatingItemSubType()
	{
		return _hatingItemSubType;
	}

	public unsafe void SetHatingItemSubType(short hatingItemSubType, DataContext context)
	{
		_hatingItemSubType = hatingItemSubType;
		SetModifiedAndInvalidateInfluencedCache(36, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 169u, 2);
			*(short*)ptr = _hatingItemSubType;
			ptr += 2;
		}
	}

	public FullName GetFullName()
	{
		return _fullName;
	}

	public unsafe void SetFullName(FullName fullName, DataContext context)
	{
		_fullName = fullName;
		SetModifiedAndInvalidateInfluencedCache(37, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 171u, 10);
			ptr += _fullName.Serialize(ptr);
		}
	}

	public MonasticTitle GetMonasticTitle()
	{
		return _monasticTitle;
	}

	public unsafe void SetMonasticTitle(MonasticTitle monasticTitle, DataContext context)
	{
		_monasticTitle = monasticTitle;
		SetModifiedAndInvalidateInfluencedCache(38, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 181u, 4);
			ptr += _monasticTitle.Serialize(ptr);
		}
	}

	public AvatarData GetAvatar()
	{
		return _avatar;
	}

	public unsafe void SetAvatar(AvatarData avatar, DataContext context)
	{
		_avatar = avatar;
		SetModifiedAndInvalidateInfluencedCache(39, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 185u, 76);
			ptr += _avatar.Serialize(ptr);
		}
	}

	public List<short> GetPotentialFeatureIds()
	{
		return _potentialFeatureIds;
	}

	public unsafe void SetPotentialFeatureIds(List<short> potentialFeatureIds, DataContext context)
	{
		_potentialFeatureIds = potentialFeatureIds;
		SetModifiedAndInvalidateInfluencedCache(40, context);
		if (CollectionHelperData.IsArchive)
		{
			int count = _potentialFeatureIds.Count;
			int num = 2 * count;
			int valueSize = 2 + num;
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 2, valueSize);
			*(ushort*)ptr = (ushort)count;
			ptr += 2;
			for (int i = 0; i < count; i++)
			{
				((short*)ptr)[i] = _potentialFeatureIds[i];
			}
			ptr += num;
		}
	}

	public List<FameActionRecord> GetFameActionRecords()
	{
		return _fameActionRecords;
	}

	public unsafe void SetFameActionRecords(List<FameActionRecord> fameActionRecords, DataContext context)
	{
		_fameActionRecords = fameActionRecords;
		SetModifiedAndInvalidateInfluencedCache(41, context);
		if (CollectionHelperData.IsArchive)
		{
			int count = _fameActionRecords.Count;
			int num = 8 * count;
			int valueSize = 2 + num;
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 3, valueSize);
			*(ushort*)ptr = (ushort)count;
			ptr += 2;
			for (int i = 0; i < count; i++)
			{
				ptr += _fameActionRecords[i].Serialize(ptr);
			}
		}
	}

	public ref Genome GetGenome()
	{
		return ref _genome;
	}

	public MainAttributes GetCurrMainAttributes()
	{
		return _currMainAttributes;
	}

	public unsafe void SetCurrMainAttributes(MainAttributes currMainAttributes, DataContext context)
	{
		_currMainAttributes = currMainAttributes;
		SetModifiedAndInvalidateInfluencedCache(43, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 325u, 12);
			ptr += _currMainAttributes.Serialize(ptr);
		}
	}

	public ref PoisonInts GetPoisoned()
	{
		return ref _poisoned;
	}

	public unsafe void SetPoisoned(ref PoisonInts poisoned, DataContext context)
	{
		_poisoned = poisoned;
		SetModifiedAndInvalidateInfluencedCache(44, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 337u, 24);
			ptr += _poisoned.Serialize(ptr);
		}
	}

	public byte[] GetInjuriesRecoveryProgress()
	{
		return _injuriesRecoveryProgress;
	}

	public unsafe void SetInjuriesRecoveryProgress(byte[] injuriesRecoveryProgress, DataContext context)
	{
		_injuriesRecoveryProgress = injuriesRecoveryProgress;
		SetModifiedAndInvalidateInfluencedCache(45, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 361u, 14);
			for (int i = 0; i < 14; i++)
			{
				ptr[i] = _injuriesRecoveryProgress[i];
			}
			ptr += 14;
		}
	}

	public int GetCurrNeili()
	{
		return _currNeili;
	}

	public unsafe void SetCurrNeili(int currNeili, DataContext context)
	{
		_currNeili = currNeili;
		SetModifiedAndInvalidateInfluencedCache(46, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 375u, 4);
			*(int*)ptr = _currNeili;
			ptr += 4;
		}
	}

	public short GetLoopingNeigong()
	{
		return _loopingNeigong;
	}

	public unsafe void SetLoopingNeigong(short loopingNeigong, DataContext context)
	{
		_loopingNeigong = loopingNeigong;
		SetModifiedAndInvalidateInfluencedCache(47, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 379u, 2);
			*(short*)ptr = _loopingNeigong;
			ptr += 2;
		}
	}

	public NeiliAllocation GetBaseNeiliAllocation()
	{
		return _baseNeiliAllocation;
	}

	public unsafe void SetBaseNeiliAllocation(NeiliAllocation baseNeiliAllocation, DataContext context)
	{
		_baseNeiliAllocation = baseNeiliAllocation;
		SetModifiedAndInvalidateInfluencedCache(48, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 381u, 8);
			ptr += _baseNeiliAllocation.Serialize(ptr);
		}
	}

	public NeiliAllocation GetExtraNeiliAllocation()
	{
		return _extraNeiliAllocation;
	}

	public unsafe void SetExtraNeiliAllocation(NeiliAllocation extraNeiliAllocation, DataContext context)
	{
		_extraNeiliAllocation = extraNeiliAllocation;
		SetModifiedAndInvalidateInfluencedCache(49, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 389u, 8);
			ptr += _extraNeiliAllocation.Serialize(ptr);
		}
	}

	public NeiliProportionOfFiveElements GetBaseNeiliProportionOfFiveElements()
	{
		return _baseNeiliProportionOfFiveElements;
	}

	public unsafe void SetBaseNeiliProportionOfFiveElements(NeiliProportionOfFiveElements baseNeiliProportionOfFiveElements, DataContext context)
	{
		_baseNeiliProportionOfFiveElements = baseNeiliProportionOfFiveElements;
		SetModifiedAndInvalidateInfluencedCache(50, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 397u, 8);
			ptr += _baseNeiliProportionOfFiveElements.Serialize(ptr);
		}
	}

	public int GetHobbyExpirationDate()
	{
		return _hobbyExpirationDate;
	}

	public unsafe void SetHobbyExpirationDate(int hobbyExpirationDate, DataContext context)
	{
		_hobbyExpirationDate = hobbyExpirationDate;
		SetModifiedAndInvalidateInfluencedCache(51, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 405u, 4);
			*(int*)ptr = _hobbyExpirationDate;
			ptr += 4;
		}
	}

	public bool GetLovingItemRevealed()
	{
		return _lovingItemRevealed;
	}

	public unsafe void SetLovingItemRevealed(bool lovingItemRevealed, DataContext context)
	{
		_lovingItemRevealed = lovingItemRevealed;
		SetModifiedAndInvalidateInfluencedCache(52, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 409u, 1);
			*ptr = (_lovingItemRevealed ? ((byte)1) : ((byte)0));
			ptr++;
		}
	}

	public bool GetHatingItemRevealed()
	{
		return _hatingItemRevealed;
	}

	public unsafe void SetHatingItemRevealed(bool hatingItemRevealed, DataContext context)
	{
		_hatingItemRevealed = hatingItemRevealed;
		SetModifiedAndInvalidateInfluencedCache(53, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 410u, 1);
			*ptr = (_hatingItemRevealed ? ((byte)1) : ((byte)0));
			ptr++;
		}
	}

	public sbyte GetLegitimateBoysCount()
	{
		return _legitimateBoysCount;
	}

	public unsafe void SetLegitimateBoysCount(sbyte legitimateBoysCount, DataContext context)
	{
		_legitimateBoysCount = legitimateBoysCount;
		SetModifiedAndInvalidateInfluencedCache(54, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 411u, 1);
			*ptr = (byte)_legitimateBoysCount;
			ptr++;
		}
	}

	public Location GetBirthLocation()
	{
		return _birthLocation;
	}

	public Location GetLocation()
	{
		return _location;
	}

	public unsafe void SetLocation(Location location, DataContext context)
	{
		_location = location;
		SetModifiedAndInvalidateInfluencedCache(56, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 416u, 4);
			ptr += _location.Serialize(ptr);
		}
	}

	public ItemKey[] GetEquipment()
	{
		return _equipment;
	}

	public unsafe void SetEquipment(ItemKey[] equipment, DataContext context)
	{
		_equipment = equipment;
		SetModifiedAndInvalidateInfluencedCache(57, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 420u, 96);
			for (int i = 0; i < 12; i++)
			{
				ptr += _equipment[i].Serialize(ptr);
			}
		}
	}

	public Inventory GetInventory()
	{
		return _inventory;
	}

	public unsafe void SetInventory(Inventory inventory, DataContext context)
	{
		_inventory = inventory;
		SetModifiedAndInvalidateInfluencedCache(58, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _inventory.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 4, serializedSize);
			ptr += _inventory.Serialize(ptr);
		}
	}

	public ref EatingItems GetEatingItems()
	{
		return ref _eatingItems;
	}

	public unsafe void SetEatingItems(ref EatingItems eatingItems, DataContext context)
	{
		_eatingItems = eatingItems;
		SetModifiedAndInvalidateInfluencedCache(59, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 516u, 90);
			ptr += _eatingItems.Serialize(ptr);
		}
	}

	public List<short> GetLearnedCombatSkills()
	{
		return _learnedCombatSkills;
	}

	public unsafe void SetLearnedCombatSkills(List<short> learnedCombatSkills, DataContext context)
	{
		_learnedCombatSkills = learnedCombatSkills;
		SetModifiedAndInvalidateInfluencedCache(60, context);
		if (CollectionHelperData.IsArchive)
		{
			int count = _learnedCombatSkills.Count;
			int num = 2 * count;
			int valueSize = 2 + num;
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 5, valueSize);
			*(ushort*)ptr = (ushort)count;
			ptr += 2;
			for (int i = 0; i < count; i++)
			{
				((short*)ptr)[i] = _learnedCombatSkills[i];
			}
			ptr += num;
		}
	}

	public short[] GetEquippedCombatSkills()
	{
		return _equippedCombatSkills;
	}

	public unsafe void SetEquippedCombatSkills(short[] equippedCombatSkills, DataContext context)
	{
		_equippedCombatSkills = equippedCombatSkills;
		SetModifiedAndInvalidateInfluencedCache(61, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 606u, 96);
			for (int i = 0; i < 48; i++)
			{
				((short*)ptr)[i] = _equippedCombatSkills[i];
			}
			ptr += 96;
		}
	}

	public short[] GetCombatSkillAttainmentPanels()
	{
		return _combatSkillAttainmentPanels;
	}

	public unsafe void SetCombatSkillAttainmentPanels(short[] combatSkillAttainmentPanels, DataContext context)
	{
		_combatSkillAttainmentPanels = combatSkillAttainmentPanels;
		SetModifiedAndInvalidateInfluencedCache(62, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 702u, 252);
			for (int i = 0; i < 126; i++)
			{
				((short*)ptr)[i] = _combatSkillAttainmentPanels[i];
			}
			ptr += 252;
		}
	}

	public List<SkillQualificationBonus> GetSkillQualificationBonuses()
	{
		return _skillQualificationBonuses;
	}

	public unsafe void SetSkillQualificationBonuses(List<SkillQualificationBonus> skillQualificationBonuses, DataContext context)
	{
		_skillQualificationBonuses = skillQualificationBonuses;
		SetModifiedAndInvalidateInfluencedCache(63, context);
		if (CollectionHelperData.IsArchive)
		{
			int count = _skillQualificationBonuses.Count;
			int num = 4 * count;
			int valueSize = 2 + num;
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 6, valueSize);
			*(ushort*)ptr = (ushort)count;
			ptr += 2;
			for (int i = 0; i < count; i++)
			{
				ptr += _skillQualificationBonuses[i].Serialize(ptr);
			}
		}
	}

	public ref PreexistenceCharIds GetPreexistenceCharIds()
	{
		return ref _preexistenceCharIds;
	}

	public byte GetXiangshuInfection()
	{
		return _xiangshuInfection;
	}

	public unsafe void SetXiangshuInfection(byte xiangshuInfection, DataContext context)
	{
		_xiangshuInfection = xiangshuInfection;
		SetModifiedAndInvalidateInfluencedCache(65, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 1006u, 1);
			*ptr = _xiangshuInfection;
			ptr++;
		}
	}

	public short GetCurrAge()
	{
		return _currAge;
	}

	public unsafe void SetCurrAge(short currAge, DataContext context)
	{
		_currAge = currAge;
		SetModifiedAndInvalidateInfluencedCache(66, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 1007u, 2);
			*(short*)ptr = _currAge;
			ptr += 2;
		}
	}

	public int GetExp()
	{
		return _exp;
	}

	public unsafe void SetExp(int exp, DataContext context)
	{
		_exp = exp;
		SetModifiedAndInvalidateInfluencedCache(67, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 1009u, 4);
			*(int*)ptr = _exp;
			ptr += 4;
		}
	}

	public byte GetExternalRelationState()
	{
		return _externalRelationState;
	}

	public unsafe void SetExternalRelationState(byte externalRelationState, DataContext context)
	{
		_externalRelationState = externalRelationState;
		SetModifiedAndInvalidateInfluencedCache(68, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 1013u, 1);
			*ptr = _externalRelationState;
			ptr++;
		}
	}

	public int GetKidnapperId()
	{
		return _kidnapperId;
	}

	public unsafe void SetKidnapperId(int kidnapperId, DataContext context)
	{
		_kidnapperId = kidnapperId;
		SetModifiedAndInvalidateInfluencedCache(69, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 1014u, 4);
			*(int*)ptr = _kidnapperId;
			ptr += 4;
		}
	}

	public int GetLeaderId()
	{
		return _leaderId;
	}

	public unsafe void SetLeaderId(int leaderId, DataContext context)
	{
		_leaderId = leaderId;
		SetModifiedAndInvalidateInfluencedCache(70, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 1018u, 4);
			*(int*)ptr = _leaderId;
			ptr += 4;
		}
	}

	public int GetFactionId()
	{
		return _factionId;
	}

	public unsafe void SetFactionId(int factionId, DataContext context)
	{
		_factionId = factionId;
		SetModifiedAndInvalidateInfluencedCache(71, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 1022u, 4);
			*(int*)ptr = _factionId;
			ptr += 4;
		}
	}

	public List<GameData.Domains.Character.Ai.PersonalNeed> GetPersonalNeeds()
	{
		return _personalNeeds;
	}

	public unsafe void SetPersonalNeeds(List<GameData.Domains.Character.Ai.PersonalNeed> personalNeeds, DataContext context)
	{
		_personalNeeds = personalNeeds;
		SetModifiedAndInvalidateInfluencedCache(72, context);
		if (CollectionHelperData.IsArchive)
		{
			int count = _personalNeeds.Count;
			int num = 8 * count;
			int valueSize = 2 + num;
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 7, valueSize);
			*(ushort*)ptr = (ushort)count;
			ptr += 2;
			for (int i = 0; i < count; i++)
			{
				ptr += _personalNeeds[i].Serialize(ptr);
			}
		}
	}

	public ActionEnergySbytes GetActionEnergies()
	{
		return _actionEnergies;
	}

	public unsafe void SetActionEnergies(ActionEnergySbytes actionEnergies, DataContext context)
	{
		_actionEnergies = actionEnergies;
		SetModifiedAndInvalidateInfluencedCache(73, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 1026u, 5);
			ptr += _actionEnergies.Serialize(ptr);
		}
	}

	public List<NpcTravelTarget> GetNpcTravelTargets()
	{
		return _npcTravelTargets;
	}

	public unsafe void SetNpcTravelTargets(List<NpcTravelTarget> npcTravelTargets, DataContext context)
	{
		_npcTravelTargets = npcTravelTargets;
		SetModifiedAndInvalidateInfluencedCache(74, context);
		if (CollectionHelperData.IsArchive)
		{
			int count = _npcTravelTargets.Count;
			int num = 16 * count;
			int valueSize = 2 + num;
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 8, valueSize);
			*(ushort*)ptr = (ushort)count;
			ptr += 2;
			for (int i = 0; i < count; i++)
			{
				ptr += _npcTravelTargets[i].Serialize(ptr);
			}
		}
	}

	public PrioritizedActionCooldownSbytes GetPrioritizedActionCooldowns()
	{
		return _prioritizedActionCooldowns;
	}

	public unsafe void SetPrioritizedActionCooldowns(PrioritizedActionCooldownSbytes prioritizedActionCooldowns, DataContext context)
	{
		_prioritizedActionCooldowns = prioritizedActionCooldowns;
		SetModifiedAndInvalidateInfluencedCache(75, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 1031u, 9);
			ptr += _prioritizedActionCooldowns.Serialize(ptr);
		}
	}

	public short GetPhysiologicalAge()
	{
		ObjectCollectionDataStates dataStates = CollectionHelperData.DataStates;
		Thread.MemoryBarrier();
		if (dataStates.IsCached(DataStatesOffset, 76))
		{
			return _physiologicalAge;
		}
		short physiologicalAge = CalcPhysiologicalAge();
		bool lockTaken = false;
		try
		{
			_spinLock.Enter(ref lockTaken);
			_physiologicalAge = physiologicalAge;
			dataStates.SetCached(DataStatesOffset, 76);
		}
		finally
		{
			if (lockTaken)
			{
				_spinLock.Exit(useMemoryBarrier: false);
			}
		}
		Thread.MemoryBarrier();
		return _physiologicalAge;
	}

	public sbyte GetFame()
	{
		ObjectCollectionDataStates dataStates = CollectionHelperData.DataStates;
		Thread.MemoryBarrier();
		if (dataStates.IsCached(DataStatesOffset, 77))
		{
			return _fame;
		}
		sbyte fame = CalcFame();
		bool lockTaken = false;
		try
		{
			_spinLock.Enter(ref lockTaken);
			_fame = fame;
			dataStates.SetCached(DataStatesOffset, 77);
		}
		finally
		{
			if (lockTaken)
			{
				_spinLock.Exit(useMemoryBarrier: false);
			}
		}
		Thread.MemoryBarrier();
		return _fame;
	}

	public short GetMorality()
	{
		ObjectCollectionDataStates dataStates = CollectionHelperData.DataStates;
		Thread.MemoryBarrier();
		if (dataStates.IsCached(DataStatesOffset, 78))
		{
			return _morality;
		}
		short morality = CalcMorality();
		bool lockTaken = false;
		try
		{
			_spinLock.Enter(ref lockTaken);
			_morality = morality;
			dataStates.SetCached(DataStatesOffset, 78);
		}
		finally
		{
			if (lockTaken)
			{
				_spinLock.Exit(useMemoryBarrier: false);
			}
		}
		Thread.MemoryBarrier();
		return _morality;
	}

	public short GetAttraction()
	{
		ObjectCollectionDataStates dataStates = CollectionHelperData.DataStates;
		Thread.MemoryBarrier();
		if (dataStates.IsCached(DataStatesOffset, 79))
		{
			return _attraction;
		}
		short attraction = CalcAttraction();
		bool lockTaken = false;
		try
		{
			_spinLock.Enter(ref lockTaken);
			_attraction = attraction;
			dataStates.SetCached(DataStatesOffset, 79);
		}
		finally
		{
			if (lockTaken)
			{
				_spinLock.Exit(useMemoryBarrier: false);
			}
		}
		Thread.MemoryBarrier();
		return _attraction;
	}

	public MainAttributes GetMaxMainAttributes()
	{
		ObjectCollectionDataStates dataStates = CollectionHelperData.DataStates;
		Thread.MemoryBarrier();
		if (dataStates.IsCached(DataStatesOffset, 80))
		{
			return _maxMainAttributes;
		}
		MainAttributes maxMainAttributes = CalcMaxMainAttributes();
		bool lockTaken = false;
		try
		{
			_spinLock.Enter(ref lockTaken);
			_maxMainAttributes = maxMainAttributes;
			dataStates.SetCached(DataStatesOffset, 80);
		}
		finally
		{
			if (lockTaken)
			{
				_spinLock.Exit(useMemoryBarrier: false);
			}
		}
		Thread.MemoryBarrier();
		return _maxMainAttributes;
	}

	public HitOrAvoidInts GetHitValues()
	{
		ObjectCollectionDataStates dataStates = CollectionHelperData.DataStates;
		Thread.MemoryBarrier();
		if (dataStates.IsCached(DataStatesOffset, 81))
		{
			return _hitValues;
		}
		HitOrAvoidInts hitValues = CalcHitValues();
		bool lockTaken = false;
		try
		{
			_spinLock.Enter(ref lockTaken);
			_hitValues = hitValues;
			dataStates.SetCached(DataStatesOffset, 81);
		}
		finally
		{
			if (lockTaken)
			{
				_spinLock.Exit(useMemoryBarrier: false);
			}
		}
		Thread.MemoryBarrier();
		return _hitValues;
	}

	public OuterAndInnerInts GetPenetrations()
	{
		ObjectCollectionDataStates dataStates = CollectionHelperData.DataStates;
		Thread.MemoryBarrier();
		if (dataStates.IsCached(DataStatesOffset, 82))
		{
			return _penetrations;
		}
		OuterAndInnerInts penetrations = CalcPenetrations();
		bool lockTaken = false;
		try
		{
			_spinLock.Enter(ref lockTaken);
			_penetrations = penetrations;
			dataStates.SetCached(DataStatesOffset, 82);
		}
		finally
		{
			if (lockTaken)
			{
				_spinLock.Exit(useMemoryBarrier: false);
			}
		}
		Thread.MemoryBarrier();
		return _penetrations;
	}

	public HitOrAvoidInts GetAvoidValues()
	{
		ObjectCollectionDataStates dataStates = CollectionHelperData.DataStates;
		Thread.MemoryBarrier();
		if (dataStates.IsCached(DataStatesOffset, 83))
		{
			return _avoidValues;
		}
		HitOrAvoidInts avoidValues = CalcAvoidValues();
		bool lockTaken = false;
		try
		{
			_spinLock.Enter(ref lockTaken);
			_avoidValues = avoidValues;
			dataStates.SetCached(DataStatesOffset, 83);
		}
		finally
		{
			if (lockTaken)
			{
				_spinLock.Exit(useMemoryBarrier: false);
			}
		}
		Thread.MemoryBarrier();
		return _avoidValues;
	}

	public OuterAndInnerInts GetPenetrationResists()
	{
		ObjectCollectionDataStates dataStates = CollectionHelperData.DataStates;
		Thread.MemoryBarrier();
		if (dataStates.IsCached(DataStatesOffset, 84))
		{
			return _penetrationResists;
		}
		OuterAndInnerInts penetrationResists = CalcPenetrationResists();
		bool lockTaken = false;
		try
		{
			_spinLock.Enter(ref lockTaken);
			_penetrationResists = penetrationResists;
			dataStates.SetCached(DataStatesOffset, 84);
		}
		finally
		{
			if (lockTaken)
			{
				_spinLock.Exit(useMemoryBarrier: false);
			}
		}
		Thread.MemoryBarrier();
		return _penetrationResists;
	}

	public OuterAndInnerShorts GetRecoveryOfStanceAndBreath()
	{
		ObjectCollectionDataStates dataStates = CollectionHelperData.DataStates;
		Thread.MemoryBarrier();
		if (dataStates.IsCached(DataStatesOffset, 85))
		{
			return _recoveryOfStanceAndBreath;
		}
		OuterAndInnerShorts recoveryOfStanceAndBreath = CalcRecoveryOfStanceAndBreath();
		bool lockTaken = false;
		try
		{
			_spinLock.Enter(ref lockTaken);
			_recoveryOfStanceAndBreath = recoveryOfStanceAndBreath;
			dataStates.SetCached(DataStatesOffset, 85);
		}
		finally
		{
			if (lockTaken)
			{
				_spinLock.Exit(useMemoryBarrier: false);
			}
		}
		Thread.MemoryBarrier();
		return _recoveryOfStanceAndBreath;
	}

	public short GetMoveSpeed()
	{
		ObjectCollectionDataStates dataStates = CollectionHelperData.DataStates;
		Thread.MemoryBarrier();
		if (dataStates.IsCached(DataStatesOffset, 86))
		{
			return _moveSpeed;
		}
		short moveSpeed = CalcMoveSpeed();
		bool lockTaken = false;
		try
		{
			_spinLock.Enter(ref lockTaken);
			_moveSpeed = moveSpeed;
			dataStates.SetCached(DataStatesOffset, 86);
		}
		finally
		{
			if (lockTaken)
			{
				_spinLock.Exit(useMemoryBarrier: false);
			}
		}
		Thread.MemoryBarrier();
		return _moveSpeed;
	}

	public short GetRecoveryOfFlaw()
	{
		ObjectCollectionDataStates dataStates = CollectionHelperData.DataStates;
		Thread.MemoryBarrier();
		if (dataStates.IsCached(DataStatesOffset, 87))
		{
			return _recoveryOfFlaw;
		}
		short recoveryOfFlaw = CalcRecoveryOfFlaw();
		bool lockTaken = false;
		try
		{
			_spinLock.Enter(ref lockTaken);
			_recoveryOfFlaw = recoveryOfFlaw;
			dataStates.SetCached(DataStatesOffset, 87);
		}
		finally
		{
			if (lockTaken)
			{
				_spinLock.Exit(useMemoryBarrier: false);
			}
		}
		Thread.MemoryBarrier();
		return _recoveryOfFlaw;
	}

	public short GetCastSpeed()
	{
		ObjectCollectionDataStates dataStates = CollectionHelperData.DataStates;
		Thread.MemoryBarrier();
		if (dataStates.IsCached(DataStatesOffset, 88))
		{
			return _castSpeed;
		}
		short castSpeed = CalcCastSpeed();
		bool lockTaken = false;
		try
		{
			_spinLock.Enter(ref lockTaken);
			_castSpeed = castSpeed;
			dataStates.SetCached(DataStatesOffset, 88);
		}
		finally
		{
			if (lockTaken)
			{
				_spinLock.Exit(useMemoryBarrier: false);
			}
		}
		Thread.MemoryBarrier();
		return _castSpeed;
	}

	public short GetRecoveryOfBlockedAcupoint()
	{
		ObjectCollectionDataStates dataStates = CollectionHelperData.DataStates;
		Thread.MemoryBarrier();
		if (dataStates.IsCached(DataStatesOffset, 89))
		{
			return _recoveryOfBlockedAcupoint;
		}
		short recoveryOfBlockedAcupoint = CalcRecoveryOfBlockedAcupoint();
		bool lockTaken = false;
		try
		{
			_spinLock.Enter(ref lockTaken);
			_recoveryOfBlockedAcupoint = recoveryOfBlockedAcupoint;
			dataStates.SetCached(DataStatesOffset, 89);
		}
		finally
		{
			if (lockTaken)
			{
				_spinLock.Exit(useMemoryBarrier: false);
			}
		}
		Thread.MemoryBarrier();
		return _recoveryOfBlockedAcupoint;
	}

	public short GetWeaponSwitchSpeed()
	{
		ObjectCollectionDataStates dataStates = CollectionHelperData.DataStates;
		Thread.MemoryBarrier();
		if (dataStates.IsCached(DataStatesOffset, 90))
		{
			return _weaponSwitchSpeed;
		}
		short weaponSwitchSpeed = CalcWeaponSwitchSpeed();
		bool lockTaken = false;
		try
		{
			_spinLock.Enter(ref lockTaken);
			_weaponSwitchSpeed = weaponSwitchSpeed;
			dataStates.SetCached(DataStatesOffset, 90);
		}
		finally
		{
			if (lockTaken)
			{
				_spinLock.Exit(useMemoryBarrier: false);
			}
		}
		Thread.MemoryBarrier();
		return _weaponSwitchSpeed;
	}

	public short GetAttackSpeed()
	{
		ObjectCollectionDataStates dataStates = CollectionHelperData.DataStates;
		Thread.MemoryBarrier();
		if (dataStates.IsCached(DataStatesOffset, 91))
		{
			return _attackSpeed;
		}
		short attackSpeed = CalcAttackSpeed();
		bool lockTaken = false;
		try
		{
			_spinLock.Enter(ref lockTaken);
			_attackSpeed = attackSpeed;
			dataStates.SetCached(DataStatesOffset, 91);
		}
		finally
		{
			if (lockTaken)
			{
				_spinLock.Exit(useMemoryBarrier: false);
			}
		}
		Thread.MemoryBarrier();
		return _attackSpeed;
	}

	public short GetInnerRatio()
	{
		ObjectCollectionDataStates dataStates = CollectionHelperData.DataStates;
		Thread.MemoryBarrier();
		if (dataStates.IsCached(DataStatesOffset, 92))
		{
			return _innerRatio;
		}
		short innerRatio = CalcInnerRatio();
		bool lockTaken = false;
		try
		{
			_spinLock.Enter(ref lockTaken);
			_innerRatio = innerRatio;
			dataStates.SetCached(DataStatesOffset, 92);
		}
		finally
		{
			if (lockTaken)
			{
				_spinLock.Exit(useMemoryBarrier: false);
			}
		}
		Thread.MemoryBarrier();
		return _innerRatio;
	}

	public short GetRecoveryOfQiDisorder()
	{
		ObjectCollectionDataStates dataStates = CollectionHelperData.DataStates;
		Thread.MemoryBarrier();
		if (dataStates.IsCached(DataStatesOffset, 93))
		{
			return _recoveryOfQiDisorder;
		}
		short recoveryOfQiDisorder = CalcRecoveryOfQiDisorder();
		bool lockTaken = false;
		try
		{
			_spinLock.Enter(ref lockTaken);
			_recoveryOfQiDisorder = recoveryOfQiDisorder;
			dataStates.SetCached(DataStatesOffset, 93);
		}
		finally
		{
			if (lockTaken)
			{
				_spinLock.Exit(useMemoryBarrier: false);
			}
		}
		Thread.MemoryBarrier();
		return _recoveryOfQiDisorder;
	}

	public ref PoisonInts GetPoisonResists()
	{
		ObjectCollectionDataStates dataStates = CollectionHelperData.DataStates;
		Thread.MemoryBarrier();
		if (dataStates.IsCached(DataStatesOffset, 94))
		{
			return ref _poisonResists;
		}
		PoisonInts poisonResists = CalcPoisonResists();
		bool lockTaken = false;
		try
		{
			_spinLock.Enter(ref lockTaken);
			_poisonResists = poisonResists;
			dataStates.SetCached(DataStatesOffset, 94);
		}
		finally
		{
			if (lockTaken)
			{
				_spinLock.Exit(useMemoryBarrier: false);
			}
		}
		Thread.MemoryBarrier();
		return ref _poisonResists;
	}

	public short GetMaxHealth()
	{
		ObjectCollectionDataStates dataStates = CollectionHelperData.DataStates;
		Thread.MemoryBarrier();
		if (dataStates.IsCached(DataStatesOffset, 95))
		{
			return _maxHealth;
		}
		short maxHealth = CalcMaxHealth();
		bool lockTaken = false;
		try
		{
			_spinLock.Enter(ref lockTaken);
			_maxHealth = maxHealth;
			dataStates.SetCached(DataStatesOffset, 95);
		}
		finally
		{
			if (lockTaken)
			{
				_spinLock.Exit(useMemoryBarrier: false);
			}
		}
		Thread.MemoryBarrier();
		return _maxHealth;
	}

	public short GetFertility()
	{
		ObjectCollectionDataStates dataStates = CollectionHelperData.DataStates;
		Thread.MemoryBarrier();
		if (dataStates.IsCached(DataStatesOffset, 96))
		{
			return _fertility;
		}
		short fertility = CalcFertility();
		bool lockTaken = false;
		try
		{
			_spinLock.Enter(ref lockTaken);
			_fertility = fertility;
			dataStates.SetCached(DataStatesOffset, 96);
		}
		finally
		{
			if (lockTaken)
			{
				_spinLock.Exit(useMemoryBarrier: false);
			}
		}
		Thread.MemoryBarrier();
		return _fertility;
	}

	public ref LifeSkillShorts GetLifeSkillQualifications()
	{
		ObjectCollectionDataStates dataStates = CollectionHelperData.DataStates;
		Thread.MemoryBarrier();
		if (dataStates.IsCached(DataStatesOffset, 97))
		{
			return ref _lifeSkillQualifications;
		}
		LifeSkillShorts lifeSkillQualifications = CalcLifeSkillQualifications();
		bool lockTaken = false;
		try
		{
			_spinLock.Enter(ref lockTaken);
			_lifeSkillQualifications = lifeSkillQualifications;
			dataStates.SetCached(DataStatesOffset, 97);
		}
		finally
		{
			if (lockTaken)
			{
				_spinLock.Exit(useMemoryBarrier: false);
			}
		}
		Thread.MemoryBarrier();
		return ref _lifeSkillQualifications;
	}

	public ref LifeSkillShorts GetLifeSkillAttainments()
	{
		ObjectCollectionDataStates dataStates = CollectionHelperData.DataStates;
		Thread.MemoryBarrier();
		if (dataStates.IsCached(DataStatesOffset, 98))
		{
			return ref _lifeSkillAttainments;
		}
		LifeSkillShorts lifeSkillAttainments = CalcLifeSkillAttainments();
		bool lockTaken = false;
		try
		{
			_spinLock.Enter(ref lockTaken);
			_lifeSkillAttainments = lifeSkillAttainments;
			dataStates.SetCached(DataStatesOffset, 98);
		}
		finally
		{
			if (lockTaken)
			{
				_spinLock.Exit(useMemoryBarrier: false);
			}
		}
		Thread.MemoryBarrier();
		return ref _lifeSkillAttainments;
	}

	public ref CombatSkillShorts GetCombatSkillQualifications()
	{
		ObjectCollectionDataStates dataStates = CollectionHelperData.DataStates;
		Thread.MemoryBarrier();
		if (dataStates.IsCached(DataStatesOffset, 99))
		{
			return ref _combatSkillQualifications;
		}
		CombatSkillShorts combatSkillQualifications = CalcCombatSkillQualifications();
		bool lockTaken = false;
		try
		{
			_spinLock.Enter(ref lockTaken);
			_combatSkillQualifications = combatSkillQualifications;
			dataStates.SetCached(DataStatesOffset, 99);
		}
		finally
		{
			if (lockTaken)
			{
				_spinLock.Exit(useMemoryBarrier: false);
			}
		}
		Thread.MemoryBarrier();
		return ref _combatSkillQualifications;
	}

	public ref CombatSkillShorts GetCombatSkillAttainments()
	{
		ObjectCollectionDataStates dataStates = CollectionHelperData.DataStates;
		Thread.MemoryBarrier();
		if (dataStates.IsCached(DataStatesOffset, 100))
		{
			return ref _combatSkillAttainments;
		}
		CombatSkillShorts combatSkillAttainments = CalcCombatSkillAttainments();
		bool lockTaken = false;
		try
		{
			_spinLock.Enter(ref lockTaken);
			_combatSkillAttainments = combatSkillAttainments;
			dataStates.SetCached(DataStatesOffset, 100);
		}
		finally
		{
			if (lockTaken)
			{
				_spinLock.Exit(useMemoryBarrier: false);
			}
		}
		Thread.MemoryBarrier();
		return ref _combatSkillAttainments;
	}

	public Personalities GetPersonalities()
	{
		ObjectCollectionDataStates dataStates = CollectionHelperData.DataStates;
		Thread.MemoryBarrier();
		if (dataStates.IsCached(DataStatesOffset, 101))
		{
			return _personalities;
		}
		Personalities personalities = CalcPersonalities();
		bool lockTaken = false;
		try
		{
			_spinLock.Enter(ref lockTaken);
			_personalities = personalities;
			dataStates.SetCached(DataStatesOffset, 101);
		}
		finally
		{
			if (lockTaken)
			{
				_spinLock.Exit(useMemoryBarrier: false);
			}
		}
		Thread.MemoryBarrier();
		return _personalities;
	}

	public sbyte GetHobbyChangingPeriod()
	{
		ObjectCollectionDataStates dataStates = CollectionHelperData.DataStates;
		Thread.MemoryBarrier();
		if (dataStates.IsCached(DataStatesOffset, 102))
		{
			return _hobbyChangingPeriod;
		}
		sbyte hobbyChangingPeriod = CalcHobbyChangingPeriod();
		bool lockTaken = false;
		try
		{
			_spinLock.Enter(ref lockTaken);
			_hobbyChangingPeriod = hobbyChangingPeriod;
			dataStates.SetCached(DataStatesOffset, 102);
		}
		finally
		{
			if (lockTaken)
			{
				_spinLock.Exit(useMemoryBarrier: false);
			}
		}
		Thread.MemoryBarrier();
		return _hobbyChangingPeriod;
	}

	public OuterAndInnerShorts GetFavorabilityChangingFactor()
	{
		ObjectCollectionDataStates dataStates = CollectionHelperData.DataStates;
		Thread.MemoryBarrier();
		if (dataStates.IsCached(DataStatesOffset, 103))
		{
			return _favorabilityChangingFactor;
		}
		OuterAndInnerShorts favorabilityChangingFactor = CalcFavorabilityChangingFactor();
		bool lockTaken = false;
		try
		{
			_spinLock.Enter(ref lockTaken);
			_favorabilityChangingFactor = favorabilityChangingFactor;
			dataStates.SetCached(DataStatesOffset, 103);
		}
		finally
		{
			if (lockTaken)
			{
				_spinLock.Exit(useMemoryBarrier: false);
			}
		}
		Thread.MemoryBarrier();
		return _favorabilityChangingFactor;
	}

	public int GetMaxInventoryLoad()
	{
		ObjectCollectionDataStates dataStates = CollectionHelperData.DataStates;
		Thread.MemoryBarrier();
		if (dataStates.IsCached(DataStatesOffset, 104))
		{
			return _maxInventoryLoad;
		}
		int maxInventoryLoad = CalcMaxInventoryLoad();
		bool lockTaken = false;
		try
		{
			_spinLock.Enter(ref lockTaken);
			_maxInventoryLoad = maxInventoryLoad;
			dataStates.SetCached(DataStatesOffset, 104);
		}
		finally
		{
			if (lockTaken)
			{
				_spinLock.Exit(useMemoryBarrier: false);
			}
		}
		Thread.MemoryBarrier();
		return _maxInventoryLoad;
	}

	public int GetCurrInventoryLoad()
	{
		ObjectCollectionDataStates dataStates = CollectionHelperData.DataStates;
		Thread.MemoryBarrier();
		if (dataStates.IsCached(DataStatesOffset, 105))
		{
			return _currInventoryLoad;
		}
		int currInventoryLoad = CalcCurrInventoryLoad();
		bool lockTaken = false;
		try
		{
			_spinLock.Enter(ref lockTaken);
			_currInventoryLoad = currInventoryLoad;
			dataStates.SetCached(DataStatesOffset, 105);
		}
		finally
		{
			if (lockTaken)
			{
				_spinLock.Exit(useMemoryBarrier: false);
			}
		}
		Thread.MemoryBarrier();
		return _currInventoryLoad;
	}

	public int GetMaxEquipmentLoad()
	{
		ObjectCollectionDataStates dataStates = CollectionHelperData.DataStates;
		Thread.MemoryBarrier();
		if (dataStates.IsCached(DataStatesOffset, 106))
		{
			return _maxEquipmentLoad;
		}
		int maxEquipmentLoad = CalcMaxEquipmentLoad();
		bool lockTaken = false;
		try
		{
			_spinLock.Enter(ref lockTaken);
			_maxEquipmentLoad = maxEquipmentLoad;
			dataStates.SetCached(DataStatesOffset, 106);
		}
		finally
		{
			if (lockTaken)
			{
				_spinLock.Exit(useMemoryBarrier: false);
			}
		}
		Thread.MemoryBarrier();
		return _maxEquipmentLoad;
	}

	public int GetCurrEquipmentLoad()
	{
		ObjectCollectionDataStates dataStates = CollectionHelperData.DataStates;
		Thread.MemoryBarrier();
		if (dataStates.IsCached(DataStatesOffset, 107))
		{
			return _currEquipmentLoad;
		}
		int currEquipmentLoad = CalcCurrEquipmentLoad();
		bool lockTaken = false;
		try
		{
			_spinLock.Enter(ref lockTaken);
			_currEquipmentLoad = currEquipmentLoad;
			dataStates.SetCached(DataStatesOffset, 107);
		}
		finally
		{
			if (lockTaken)
			{
				_spinLock.Exit(useMemoryBarrier: false);
			}
		}
		Thread.MemoryBarrier();
		return _currEquipmentLoad;
	}

	public int GetInventoryTotalValue()
	{
		ObjectCollectionDataStates dataStates = CollectionHelperData.DataStates;
		Thread.MemoryBarrier();
		if (dataStates.IsCached(DataStatesOffset, 108))
		{
			return _inventoryTotalValue;
		}
		int inventoryTotalValue = CalcInventoryTotalValue();
		bool lockTaken = false;
		try
		{
			_spinLock.Enter(ref lockTaken);
			_inventoryTotalValue = inventoryTotalValue;
			dataStates.SetCached(DataStatesOffset, 108);
		}
		finally
		{
			if (lockTaken)
			{
				_spinLock.Exit(useMemoryBarrier: false);
			}
		}
		Thread.MemoryBarrier();
		return _inventoryTotalValue;
	}

	public int GetMaxNeili()
	{
		ObjectCollectionDataStates dataStates = CollectionHelperData.DataStates;
		Thread.MemoryBarrier();
		if (dataStates.IsCached(DataStatesOffset, 109))
		{
			return _maxNeili;
		}
		int maxNeili = CalcMaxNeili();
		bool lockTaken = false;
		try
		{
			_spinLock.Enter(ref lockTaken);
			_maxNeili = maxNeili;
			dataStates.SetCached(DataStatesOffset, 109);
		}
		finally
		{
			if (lockTaken)
			{
				_spinLock.Exit(useMemoryBarrier: false);
			}
		}
		Thread.MemoryBarrier();
		return _maxNeili;
	}

	public NeiliAllocation GetNeiliAllocation()
	{
		ObjectCollectionDataStates dataStates = CollectionHelperData.DataStates;
		Thread.MemoryBarrier();
		if (dataStates.IsCached(DataStatesOffset, 110))
		{
			return _neiliAllocation;
		}
		NeiliAllocation neiliAllocation = CalcNeiliAllocation();
		bool lockTaken = false;
		try
		{
			_spinLock.Enter(ref lockTaken);
			_neiliAllocation = neiliAllocation;
			dataStates.SetCached(DataStatesOffset, 110);
		}
		finally
		{
			if (lockTaken)
			{
				_spinLock.Exit(useMemoryBarrier: false);
			}
		}
		Thread.MemoryBarrier();
		return _neiliAllocation;
	}

	public NeiliProportionOfFiveElements GetNeiliProportionOfFiveElements()
	{
		ObjectCollectionDataStates dataStates = CollectionHelperData.DataStates;
		Thread.MemoryBarrier();
		if (dataStates.IsCached(DataStatesOffset, 111))
		{
			return _neiliProportionOfFiveElements;
		}
		NeiliProportionOfFiveElements neiliProportionOfFiveElements = CalcNeiliProportionOfFiveElements();
		bool lockTaken = false;
		try
		{
			_spinLock.Enter(ref lockTaken);
			_neiliProportionOfFiveElements = neiliProportionOfFiveElements;
			dataStates.SetCached(DataStatesOffset, 111);
		}
		finally
		{
			if (lockTaken)
			{
				_spinLock.Exit(useMemoryBarrier: false);
			}
		}
		Thread.MemoryBarrier();
		return _neiliProportionOfFiveElements;
	}

	public sbyte GetNeiliType()
	{
		ObjectCollectionDataStates dataStates = CollectionHelperData.DataStates;
		Thread.MemoryBarrier();
		if (dataStates.IsCached(DataStatesOffset, 112))
		{
			return _neiliType;
		}
		sbyte neiliType = CalcNeiliType();
		bool lockTaken = false;
		try
		{
			_spinLock.Enter(ref lockTaken);
			_neiliType = neiliType;
			dataStates.SetCached(DataStatesOffset, 112);
		}
		finally
		{
			if (lockTaken)
			{
				_spinLock.Exit(useMemoryBarrier: false);
			}
		}
		Thread.MemoryBarrier();
		return _neiliType;
	}

	public int GetCombatPower()
	{
		ObjectCollectionDataStates dataStates = CollectionHelperData.DataStates;
		Thread.MemoryBarrier();
		if (dataStates.IsCached(DataStatesOffset, 113))
		{
			return _combatPower;
		}
		int combatPower = CalcCombatPower();
		bool lockTaken = false;
		try
		{
			_spinLock.Enter(ref lockTaken);
			_combatPower = combatPower;
			dataStates.SetCached(DataStatesOffset, 113);
		}
		finally
		{
			if (lockTaken)
			{
				_spinLock.Exit(useMemoryBarrier: false);
			}
		}
		Thread.MemoryBarrier();
		return _combatPower;
	}

	public sbyte GetAttackTendencyOfInnerAndOuter()
	{
		ObjectCollectionDataStates dataStates = CollectionHelperData.DataStates;
		Thread.MemoryBarrier();
		if (dataStates.IsCached(DataStatesOffset, 114))
		{
			return _attackTendencyOfInnerAndOuter;
		}
		sbyte attackTendencyOfInnerAndOuter = CalcAttackTendencyOfInnerAndOuter();
		bool lockTaken = false;
		try
		{
			_spinLock.Enter(ref lockTaken);
			_attackTendencyOfInnerAndOuter = attackTendencyOfInnerAndOuter;
			dataStates.SetCached(DataStatesOffset, 114);
		}
		finally
		{
			if (lockTaken)
			{
				_spinLock.Exit(useMemoryBarrier: false);
			}
		}
		Thread.MemoryBarrier();
		return _attackTendencyOfInnerAndOuter;
	}

	public NeiliAllocation GetAllocatedNeiliEffects()
	{
		ObjectCollectionDataStates dataStates = CollectionHelperData.DataStates;
		Thread.MemoryBarrier();
		if (dataStates.IsCached(DataStatesOffset, 115))
		{
			return _allocatedNeiliEffects;
		}
		NeiliAllocation allocatedNeiliEffects = CalcAllocatedNeiliEffects();
		bool lockTaken = false;
		try
		{
			_spinLock.Enter(ref lockTaken);
			_allocatedNeiliEffects = allocatedNeiliEffects;
			dataStates.SetCached(DataStatesOffset, 115);
		}
		finally
		{
			if (lockTaken)
			{
				_spinLock.Exit(useMemoryBarrier: false);
			}
		}
		Thread.MemoryBarrier();
		return _allocatedNeiliEffects;
	}

	public sbyte GetMaxConsummateLevel()
	{
		ObjectCollectionDataStates dataStates = CollectionHelperData.DataStates;
		Thread.MemoryBarrier();
		if (dataStates.IsCached(DataStatesOffset, 116))
		{
			return _maxConsummateLevel;
		}
		sbyte maxConsummateLevel = CalcMaxConsummateLevel();
		bool lockTaken = false;
		try
		{
			_spinLock.Enter(ref lockTaken);
			_maxConsummateLevel = maxConsummateLevel;
			dataStates.SetCached(DataStatesOffset, 116);
		}
		finally
		{
			if (lockTaken)
			{
				_spinLock.Exit(useMemoryBarrier: false);
			}
		}
		Thread.MemoryBarrier();
		return _maxConsummateLevel;
	}

	public CombatSkillEquipment GetCombatSkillEquipment()
	{
		ObjectCollectionDataStates dataStates = CollectionHelperData.DataStates;
		Thread.MemoryBarrier();
		if (dataStates.IsCached(DataStatesOffset, 117))
		{
			return _combatSkillEquipment;
		}
		CombatSkillEquipment combatSkillEquipment = new CombatSkillEquipment();
		CalcCombatSkillEquipment(combatSkillEquipment);
		bool lockTaken = false;
		try
		{
			_spinLock.Enter(ref lockTaken);
			_combatSkillEquipment.Assign(combatSkillEquipment);
			dataStates.SetCached(DataStatesOffset, 117);
		}
		finally
		{
			if (lockTaken)
			{
				_spinLock.Exit(useMemoryBarrier: false);
			}
		}
		Thread.MemoryBarrier();
		return _combatSkillEquipment;
	}

	public uint GetDarkAshProtector()
	{
		ObjectCollectionDataStates dataStates = CollectionHelperData.DataStates;
		Thread.MemoryBarrier();
		if (dataStates.IsCached(DataStatesOffset, 118))
		{
			return _darkAshProtector;
		}
		uint darkAshProtector = CalcDarkAshProtector();
		bool lockTaken = false;
		try
		{
			_spinLock.Enter(ref lockTaken);
			_darkAshProtector = darkAshProtector;
			dataStates.SetCached(DataStatesOffset, 118);
		}
		finally
		{
			if (lockTaken)
			{
				_spinLock.Exit(useMemoryBarrier: false);
			}
		}
		Thread.MemoryBarrier();
		return _darkAshProtector;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public string GetSurname()
	{
		return Config.Character.Instance[_templateId].Surname;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public string GetGivenName()
	{
		return Config.Character.Instance[_templateId].GivenName;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public string GetAnonymousTitle()
	{
		return Config.Character.Instance[_templateId].AnonymousTitle;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public bool GetRandomFeaturesAtCreating()
	{
		return Config.Character.Instance[_templateId].RandomFeaturesAtCreating;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public bool GetAllowUseFreeWeapon()
	{
		return Config.Character.Instance[_templateId].AllowUseFreeWeapon;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public bool GetAllowEscape()
	{
		return Config.Character.Instance[_templateId].AllowEscape;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public bool GetAllowHeal()
	{
		return Config.Character.Instance[_templateId].AllowHeal;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public bool GetCanDefeat()
	{
		return Config.Character.Instance[_templateId].CanDefeat;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public short GetRandomEnemyId()
	{
		return Config.Character.Instance[_templateId].RandomEnemyId;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public short GetLeadingEnemyNestId()
	{
		return Config.Character.Instance[_templateId].LeadingEnemyNestId;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public string GetFixedAvatarName()
	{
		return Config.Character.Instance[_templateId].FixedAvatarName;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public sbyte GetPresetBodyType()
	{
		return Config.Character.Instance[_templateId].PresetBodyType;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public bool GetHideAge()
	{
		return Config.Character.Instance[_templateId].HideAge;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public sbyte GetRace()
	{
		return Config.Character.Instance[_templateId].Race;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public sbyte GetPresetFame()
	{
		return Config.Character.Instance[_templateId].PresetFame;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public short GetBaseAttraction()
	{
		return Config.Character.Instance[_templateId].BaseAttraction;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public bool GetCanBeKidnapped()
	{
		return Config.Character.Instance[_templateId].CanBeKidnapped;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public short GetFixWeaponPower()
	{
		return Config.Character.Instance[_templateId].FixWeaponPower;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public short GetFixArmorPower()
	{
		return Config.Character.Instance[_templateId].FixArmorPower;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public short GetFixCombatSkillPower()
	{
		return Config.Character.Instance[_templateId].FixCombatSkillPower;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public HitOrAvoidInts GetBaseHitValues()
	{
		return Config.Character.Instance[_templateId].BaseHitValues;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public OuterAndInnerInts GetBasePenetrations()
	{
		return Config.Character.Instance[_templateId].BasePenetrations;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public HitOrAvoidInts GetBaseAvoidValues()
	{
		return Config.Character.Instance[_templateId].BaseAvoidValues;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public OuterAndInnerInts GetBasePenetrationResists()
	{
		return Config.Character.Instance[_templateId].BasePenetrationResists;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public OuterAndInnerShorts GetBaseRecoveryOfStanceAndBreath()
	{
		return Config.Character.Instance[_templateId].BaseRecoveryOfStanceAndBreath;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public short GetBaseMoveSpeed()
	{
		return Config.Character.Instance[_templateId].BaseMoveSpeed;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public short GetBaseRecoveryOfFlaw()
	{
		return Config.Character.Instance[_templateId].BaseRecoveryOfFlaw;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public short GetBaseCastSpeed()
	{
		return Config.Character.Instance[_templateId].BaseCastSpeed;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public short GetBaseRecoveryOfBlockedAcupoint()
	{
		return Config.Character.Instance[_templateId].BaseRecoveryOfBlockedAcupoint;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public short GetBaseWeaponSwitchSpeed()
	{
		return Config.Character.Instance[_templateId].BaseWeaponSwitchSpeed;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public short GetBaseAttackSpeed()
	{
		return Config.Character.Instance[_templateId].BaseAttackSpeed;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public short GetBaseInnerRatio()
	{
		return Config.Character.Instance[_templateId].BaseInnerRatio;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public short GetBaseRecoveryOfQiDisorder()
	{
		return Config.Character.Instance[_templateId].BaseRecoveryOfQiDisorder;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public ref readonly PoisonInts GetBasePoisonResists()
	{
		return ref Config.Character.Instance[_templateId].BasePoisonResists;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public bool GetInnerInjuryImmunity()
	{
		return Config.Character.Instance[_templateId].InnerInjuryImmunity;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public bool GetOuterInjuryImmunity()
	{
		return Config.Character.Instance[_templateId].OuterInjuryImmunity;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public bool GetMindImmunity()
	{
		return Config.Character.Instance[_templateId].MindImmunity;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public bool GetFlawImmunity()
	{
		return Config.Character.Instance[_templateId].FlawImmunity;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public bool GetAcupointImmunity()
	{
		return Config.Character.Instance[_templateId].AcupointImmunity;
	}

	[CollectionObjectField(true, false, false, false, false, ArrayElementsCount = 6)]
	public bool[] GetPoisonImmunities()
	{
		return Config.Character.Instance[_templateId].PoisonImmunities;
	}

	[CollectionObjectField(true, false, false, false, false, ArrayElementsCount = 12)]
	public PresetEquipmentItem[] GetPresetEquipment()
	{
		return Config.Character.Instance[_templateId].PresetEquipment;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public List<PresetInventoryItem> GetPresetInventory()
	{
		return Config.Character.Instance[_templateId].PresetInventory;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public List<PresetCombatSkill> GetPresetCombatSkills()
	{
		return Config.Character.Instance[_templateId].PresetCombatSkills;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public NeiliProportionOfFiveElements GetPresetNeiliProportionOfFiveElements()
	{
		return Config.Character.Instance[_templateId].PresetNeiliProportionOfFiveElements;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public short GetMinionGroupId()
	{
		return Config.Character.Instance[_templateId].MinionGroupId;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public DamageStepCollection GetDamageSteps()
	{
		return Config.Character.Instance[_templateId].DamageSteps;
	}

	[CollectionObjectField(true, false, false, false, false, ArrayElementsCount = 4)]
	public sbyte[] GetIdeaAllocationProportion()
	{
		return Config.Character.Instance[_templateId].IdeaAllocationProportion;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public int GetExtraEquipmentLoad()
	{
		return Config.Character.Instance[_templateId].ExtraEquipmentLoad;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public short GetInitCurrAge()
	{
		return Config.Character.Instance[_templateId].InitCurrAge;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public List<sbyte> GetPresetTeammateCommands()
	{
		return Config.Character.Instance[_templateId].PresetTeammateCommands;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public bool GetIsFavorabilityDisplay()
	{
		return Config.Character.Instance[_templateId].IsFavorabilityDisplay;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public bool GetFixedCharacterShowNameOnMap()
	{
		return Config.Character.Instance[_templateId].FixedCharacterShowNameOnMap;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public sbyte GetSpecialCombatSkeleton()
	{
		return Config.Character.Instance[_templateId].SpecialCombatSkeleton;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public bool GetDieImmunity()
	{
		return Config.Character.Instance[_templateId].DieImmunity;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public bool GetFatalImmunity()
	{
		return Config.Character.Instance[_templateId].FatalImmunity;
	}

	[CollectionObjectField(true, false, false, false, false, ArrayElementsCount = 16)]
	public sbyte[] GetLearnedLifeSkillGrades()
	{
		return Config.Character.Instance[_templateId].LearnedLifeSkillGrades;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public int GetCombatAi()
	{
		return Config.Character.Instance[_templateId].CombatAi;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public bool GetCanMove()
	{
		return Config.Character.Instance[_templateId].CanMove;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public bool GetCanOpenCharacterMenu()
	{
		return Config.Character.Instance[_templateId].CanOpenCharacterMenu;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public bool GetRandomAnimalAttack()
	{
		return Config.Character.Instance[_templateId].RandomAnimalAttack;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public ref readonly ResourceInts GetDropResources()
	{
		return ref Config.Character.Instance[_templateId].DropResources;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public string GetSpecialGradeName()
	{
		return Config.Character.Instance[_templateId].SpecialGradeName;
	}

	[CollectionObjectField(true, false, false, false, false, ArrayElementsCount = 4)]
	public sbyte[] GetExtraNeiliAllocationProgress()
	{
		return Config.Character.Instance[_templateId].ExtraNeiliAllocationProgress;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public List<PresetItemTemplateId> GetPresetEatingItems()
	{
		return Config.Character.Instance[_templateId].PresetEatingItems;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public bool GetCanSpeak()
	{
		return Config.Character.Instance[_templateId].CanSpeak;
	}

	[CollectionObjectField(true, false, false, false, false, ArrayElementsCount = 4)]
	public sbyte[] GetRandomEnemyFavorability()
	{
		return Config.Character.Instance[_templateId].RandomEnemyFavorability;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public short GetGroupId()
	{
		return Config.Character.Instance[_templateId].GroupId;
	}

	[CollectionObjectField(true, false, false, false, false, ArrayElementsCount = 5)]
	public sbyte[] GetExtraCombatSkillGrids()
	{
		return Config.Character.Instance[_templateId].ExtraCombatSkillGrids;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public ECharacterSpecialTemmateType GetSpecialTemmateType()
	{
		return Config.Character.Instance[_templateId].SpecialTemmateType;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public List<sbyte> GetRandomIdealSects()
	{
		return Config.Character.Instance[_templateId].RandomIdealSects;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public bool GetAllowDropWugKing()
	{
		return Config.Character.Instance[_templateId].AllowDropWugKing;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public bool GetAllowFavorabilitySkipCd()
	{
		return Config.Character.Instance[_templateId].AllowFavorabilitySkipCd;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public string GetSpecialMuteBubbleEnemy()
	{
		return Config.Character.Instance[_templateId].SpecialMuteBubbleEnemy;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public string GetSpecialMuteBubbleSelf()
	{
		return Config.Character.Instance[_templateId].SpecialMuteBubbleSelf;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public int GetDropRatePercentAsTeammate()
	{
		return Config.Character.Instance[_templateId].DropRatePercentAsTeammate;
	}

	[CollectionObjectField(true, false, false, false, false)]
	public int GetDropRatePercentAsMainChar()
	{
		return Config.Character.Instance[_templateId].DropRatePercentAsMainChar;
	}

	public Character()
	{
		_featureIds = new List<short>();
		_learnedLifeSkills = new List<LifeSkillItem>();
		_avatar = new AvatarData();
		_potentialFeatureIds = new List<short>();
		_fameActionRecords = new List<FameActionRecord>();
		_injuriesRecoveryProgress = new byte[14];
		_equipment = new ItemKey[12];
		_inventory = new Inventory();
		_learnedCombatSkills = new List<short>();
		_equippedCombatSkills = new short[48];
		_combatSkillAttainmentPanels = new short[126];
		_skillQualificationBonuses = new List<SkillQualificationBonus>();
		_personalNeeds = new List<GameData.Domains.Character.Ai.PersonalNeed>();
		_npcTravelTargets = new List<NpcTravelTarget>();
		_combatSkillEquipment = new CombatSkillEquipment();
	}

	public Character(short templateId)
	{
		CharacterItem characterItem = Config.Character.Instance[templateId];
		_templateId = characterItem.TemplateId;
		_creatingType = characterItem.CreatingType;
		_gender = characterItem.Gender;
		_actualAge = characterItem.ActualAge;
		_birthMonth = characterItem.BirthMonth;
		_happiness = characterItem.Happiness;
		_baseMorality = characterItem.BaseMorality;
		_organizationInfo = characterItem.OrganizationInfo;
		_idealSect = characterItem.IdealSect;
		_lifeSkillTypeInterest = characterItem.LifeSkillTypeInterest;
		_combatSkillTypeInterest = characterItem.CombatSkillTypeInterest;
		_mainAttributeInterest = characterItem.MainAttributeInterest;
		_transgender = characterItem.Transgender;
		_bisexual = characterItem.Bisexual;
		_xiangshuType = characterItem.XiangshuType;
		_monkType = characterItem.MonkType;
		_featureIds = new List<short>(characterItem.FeatureIds);
		_baseMainAttributes = characterItem.BaseMainAttributes;
		_health = characterItem.Health;
		_baseMaxHealth = characterItem.BaseMaxHealth;
		_disorderOfQi = characterItem.DisorderOfQi;
		_haveLeftArm = characterItem.HaveLeftArm;
		_haveRightArm = characterItem.HaveRightArm;
		_haveLeftLeg = characterItem.HaveLeftLeg;
		_haveRightLeg = characterItem.HaveRightLeg;
		_injuries = characterItem.Injuries;
		_extraNeili = characterItem.ExtraNeili;
		_consummateLevel = characterItem.ConsummateLevel;
		_learnedLifeSkills = new List<LifeSkillItem>(characterItem.LearnedLifeSkills);
		_baseLifeSkillQualifications = characterItem.BaseLifeSkillQualifications;
		_lifeSkillQualificationGrowthType = characterItem.LifeSkillQualificationGrowthType;
		_baseCombatSkillQualifications = characterItem.BaseCombatSkillQualifications;
		_combatSkillQualificationGrowthType = characterItem.CombatSkillQualificationGrowthType;
		_resources = characterItem.Resources;
		_lovingItemSubType = characterItem.LovingItemSubType;
		_hatingItemSubType = characterItem.HatingItemSubType;
		_avatar = new AvatarData();
		_potentialFeatureIds = new List<short>();
		_fameActionRecords = new List<FameActionRecord>();
		_injuriesRecoveryProgress = new byte[14];
		_equipment = new ItemKey[12];
		_inventory = new Inventory();
		_learnedCombatSkills = new List<short>();
		_equippedCombatSkills = new short[48];
		_combatSkillAttainmentPanels = new short[126];
		_skillQualificationBonuses = new List<SkillQualificationBonus>();
		_personalNeeds = new List<GameData.Domains.Character.Ai.PersonalNeed>();
		_npcTravelTargets = new List<NpcTravelTarget>();
		_combatSkillEquipment = new CombatSkillEquipment();
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 1076;
		int count = _featureIds.Count;
		int num2 = 2 * count;
		int num3 = 2 + num2;
		num += num3;
		int count2 = _learnedLifeSkills.Count;
		int num4 = 4 * count2;
		int num5 = 2 + num4;
		num += num5;
		int count3 = _potentialFeatureIds.Count;
		int num6 = 2 * count3;
		int num7 = 2 + num6;
		num += num7;
		int count4 = _fameActionRecords.Count;
		int num8 = 8 * count4;
		int num9 = 2 + num8;
		num += num9;
		int serializedSize = _inventory.GetSerializedSize();
		num += serializedSize;
		int count5 = _learnedCombatSkills.Count;
		int num10 = 2 * count5;
		int num11 = 2 + num10;
		num += num11;
		int count6 = _skillQualificationBonuses.Count;
		int num12 = 4 * count6;
		int num13 = 2 + num12;
		num += num13;
		int count7 = _personalNeeds.Count;
		int num14 = 8 * count7;
		int num15 = 2 + num14;
		num += num15;
		int count8 = _npcTravelTargets.Count;
		int num16 = 16 * count8;
		int num17 = 2 + num16;
		return num + num17;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		*(int*)ptr = _id;
		ptr += 4;
		*(short*)ptr = _templateId;
		ptr += 2;
		*ptr = _creatingType;
		ptr++;
		*ptr = (byte)_gender;
		ptr++;
		*(short*)ptr = _actualAge;
		ptr += 2;
		*ptr = (byte)_birthMonth;
		ptr++;
		*ptr = (byte)_happiness;
		ptr++;
		*(short*)ptr = _baseMorality;
		ptr += 2;
		ptr += _organizationInfo.Serialize(ptr);
		*ptr = (byte)_idealSect;
		ptr++;
		*ptr = (byte)_lifeSkillTypeInterest;
		ptr++;
		*ptr = (byte)_combatSkillTypeInterest;
		ptr++;
		*ptr = (byte)_mainAttributeInterest;
		ptr++;
		*ptr = (_transgender ? ((byte)1) : ((byte)0));
		ptr++;
		*ptr = (_bisexual ? ((byte)1) : ((byte)0));
		ptr++;
		*ptr = (byte)_xiangshuType;
		ptr++;
		*ptr = _monkType;
		ptr++;
		ptr += _baseMainAttributes.Serialize(ptr);
		*(short*)ptr = _health;
		ptr += 2;
		*(short*)ptr = _baseMaxHealth;
		ptr += 2;
		*(short*)ptr = _disorderOfQi;
		ptr += 2;
		*ptr = (_haveLeftArm ? ((byte)1) : ((byte)0));
		ptr++;
		*ptr = (_haveRightArm ? ((byte)1) : ((byte)0));
		ptr++;
		*ptr = (_haveLeftLeg ? ((byte)1) : ((byte)0));
		ptr++;
		*ptr = (_haveRightLeg ? ((byte)1) : ((byte)0));
		ptr++;
		ptr += _injuries.Serialize(ptr);
		*(int*)ptr = _extraNeili;
		ptr += 4;
		*ptr = (byte)_consummateLevel;
		ptr++;
		ptr += _baseLifeSkillQualifications.Serialize(ptr);
		*ptr = (byte)_lifeSkillQualificationGrowthType;
		ptr++;
		ptr += _baseCombatSkillQualifications.Serialize(ptr);
		*ptr = (byte)_combatSkillQualificationGrowthType;
		ptr++;
		ptr += _resources.Serialize(ptr);
		*(short*)ptr = _lovingItemSubType;
		ptr += 2;
		*(short*)ptr = _hatingItemSubType;
		ptr += 2;
		ptr += _fullName.Serialize(ptr);
		ptr += _monasticTitle.Serialize(ptr);
		ptr += _avatar.Serialize(ptr);
		ptr += _genome.Serialize(ptr);
		ptr += _currMainAttributes.Serialize(ptr);
		ptr += _poisoned.Serialize(ptr);
		if (_injuriesRecoveryProgress.Length != 14)
		{
			throw new Exception("Elements count of field _injuriesRecoveryProgress is not equal to declaration");
		}
		for (int i = 0; i < 14; i++)
		{
			ptr[i] = _injuriesRecoveryProgress[i];
		}
		ptr += 14;
		*(int*)ptr = _currNeili;
		ptr += 4;
		*(short*)ptr = _loopingNeigong;
		ptr += 2;
		ptr += _baseNeiliAllocation.Serialize(ptr);
		ptr += _extraNeiliAllocation.Serialize(ptr);
		ptr += _baseNeiliProportionOfFiveElements.Serialize(ptr);
		*(int*)ptr = _hobbyExpirationDate;
		ptr += 4;
		*ptr = (_lovingItemRevealed ? ((byte)1) : ((byte)0));
		ptr++;
		*ptr = (_hatingItemRevealed ? ((byte)1) : ((byte)0));
		ptr++;
		*ptr = (byte)_legitimateBoysCount;
		ptr++;
		ptr += _birthLocation.Serialize(ptr);
		ptr += _location.Serialize(ptr);
		if (_equipment.Length != 12)
		{
			throw new Exception("Elements count of field _equipment is not equal to declaration");
		}
		for (int j = 0; j < 12; j++)
		{
			ptr += _equipment[j].Serialize(ptr);
		}
		ptr += _eatingItems.Serialize(ptr);
		if (_equippedCombatSkills.Length != 48)
		{
			throw new Exception("Elements count of field _equippedCombatSkills is not equal to declaration");
		}
		for (int k = 0; k < 48; k++)
		{
			((short*)ptr)[k] = _equippedCombatSkills[k];
		}
		ptr += 96;
		if (_combatSkillAttainmentPanels.Length != 126)
		{
			throw new Exception("Elements count of field _combatSkillAttainmentPanels is not equal to declaration");
		}
		for (int l = 0; l < 126; l++)
		{
			((short*)ptr)[l] = _combatSkillAttainmentPanels[l];
		}
		ptr += 252;
		ptr += _preexistenceCharIds.Serialize(ptr);
		*ptr = _xiangshuInfection;
		ptr++;
		*(short*)ptr = _currAge;
		ptr += 2;
		*(int*)ptr = _exp;
		ptr += 4;
		*ptr = _externalRelationState;
		ptr++;
		*(int*)ptr = _kidnapperId;
		ptr += 4;
		*(int*)ptr = _leaderId;
		ptr += 4;
		*(int*)ptr = _factionId;
		ptr += 4;
		ptr += _actionEnergies.Serialize(ptr);
		ptr += _prioritizedActionCooldowns.Serialize(ptr);
		int count = _featureIds.Count;
		int num = 2 * count;
		if (num > 4194300)
		{
			throw new Exception($"Size of field {"_featureIds"} must be less than {4096}KB");
		}
		*(int*)ptr = num + 2;
		ptr += 4;
		*(ushort*)ptr = (ushort)count;
		ptr += 2;
		for (int m = 0; m < count; m++)
		{
			((short*)ptr)[m] = _featureIds[m];
		}
		ptr += num;
		int count2 = _learnedLifeSkills.Count;
		int num2 = 4 * count2;
		if (num2 > 4194300)
		{
			throw new Exception($"Size of field {"_learnedLifeSkills"} must be less than {4096}KB");
		}
		*(int*)ptr = num2 + 2;
		ptr += 4;
		*(ushort*)ptr = (ushort)count2;
		ptr += 2;
		for (int n = 0; n < count2; n++)
		{
			ptr += _learnedLifeSkills[n].Serialize(ptr);
		}
		int count3 = _potentialFeatureIds.Count;
		int num3 = 2 * count3;
		if (num3 > 4194300)
		{
			throw new Exception($"Size of field {"_potentialFeatureIds"} must be less than {4096}KB");
		}
		*(int*)ptr = num3 + 2;
		ptr += 4;
		*(ushort*)ptr = (ushort)count3;
		ptr += 2;
		for (int num4 = 0; num4 < count3; num4++)
		{
			((short*)ptr)[num4] = _potentialFeatureIds[num4];
		}
		ptr += num3;
		int count4 = _fameActionRecords.Count;
		int num5 = 8 * count4;
		if (num5 > 4194300)
		{
			throw new Exception($"Size of field {"_fameActionRecords"} must be less than {4096}KB");
		}
		*(int*)ptr = num5 + 2;
		ptr += 4;
		*(ushort*)ptr = (ushort)count4;
		ptr += 2;
		for (int num6 = 0; num6 < count4; num6++)
		{
			ptr += _fameActionRecords[num6].Serialize(ptr);
		}
		byte* ptr2 = ptr;
		ptr += 4;
		ptr += _inventory.Serialize(ptr);
		int num7 = (int)(ptr - ptr2 - 4);
		if (num7 > 4194304)
		{
			throw new Exception($"Size of field {"_inventory"} must be less than {4096}KB");
		}
		*(int*)ptr2 = num7;
		int count5 = _learnedCombatSkills.Count;
		int num8 = 2 * count5;
		if (num8 > 4194300)
		{
			throw new Exception($"Size of field {"_learnedCombatSkills"} must be less than {4096}KB");
		}
		*(int*)ptr = num8 + 2;
		ptr += 4;
		*(ushort*)ptr = (ushort)count5;
		ptr += 2;
		for (int num9 = 0; num9 < count5; num9++)
		{
			((short*)ptr)[num9] = _learnedCombatSkills[num9];
		}
		ptr += num8;
		int count6 = _skillQualificationBonuses.Count;
		int num10 = 4 * count6;
		if (num10 > 4194300)
		{
			throw new Exception($"Size of field {"_skillQualificationBonuses"} must be less than {4096}KB");
		}
		*(int*)ptr = num10 + 2;
		ptr += 4;
		*(ushort*)ptr = (ushort)count6;
		ptr += 2;
		for (int num11 = 0; num11 < count6; num11++)
		{
			ptr += _skillQualificationBonuses[num11].Serialize(ptr);
		}
		int count7 = _personalNeeds.Count;
		int num12 = 8 * count7;
		if (num12 > 4194300)
		{
			throw new Exception($"Size of field {"_personalNeeds"} must be less than {4096}KB");
		}
		*(int*)ptr = num12 + 2;
		ptr += 4;
		*(ushort*)ptr = (ushort)count7;
		ptr += 2;
		for (int num13 = 0; num13 < count7; num13++)
		{
			ptr += _personalNeeds[num13].Serialize(ptr);
		}
		int count8 = _npcTravelTargets.Count;
		int num14 = 16 * count8;
		if (num14 > 4194300)
		{
			throw new Exception($"Size of field {"_npcTravelTargets"} must be less than {4096}KB");
		}
		*(int*)ptr = num14 + 2;
		ptr += 4;
		*(ushort*)ptr = (ushort)count8;
		ptr += 2;
		for (int num15 = 0; num15 < count8; num15++)
		{
			ptr += _npcTravelTargets[num15].Serialize(ptr);
		}
		return (int)(ptr - pData);
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		_id = *(int*)ptr;
		ptr += 4;
		_templateId = *(short*)ptr;
		ptr += 2;
		_creatingType = *ptr;
		ptr++;
		_gender = (sbyte)(*ptr);
		ptr++;
		_actualAge = *(short*)ptr;
		ptr += 2;
		_birthMonth = (sbyte)(*ptr);
		ptr++;
		_happiness = (sbyte)(*ptr);
		ptr++;
		_baseMorality = *(short*)ptr;
		ptr += 2;
		ptr += _organizationInfo.Deserialize(ptr);
		_idealSect = (sbyte)(*ptr);
		ptr++;
		_lifeSkillTypeInterest = (sbyte)(*ptr);
		ptr++;
		_combatSkillTypeInterest = (sbyte)(*ptr);
		ptr++;
		_mainAttributeInterest = (sbyte)(*ptr);
		ptr++;
		_transgender = *ptr != 0;
		ptr++;
		_bisexual = *ptr != 0;
		ptr++;
		_xiangshuType = (sbyte)(*ptr);
		ptr++;
		_monkType = *ptr;
		ptr++;
		ptr += _baseMainAttributes.Deserialize(ptr);
		_health = *(short*)ptr;
		ptr += 2;
		_baseMaxHealth = *(short*)ptr;
		ptr += 2;
		_disorderOfQi = *(short*)ptr;
		ptr += 2;
		_haveLeftArm = *ptr != 0;
		ptr++;
		_haveRightArm = *ptr != 0;
		ptr++;
		_haveLeftLeg = *ptr != 0;
		ptr++;
		_haveRightLeg = *ptr != 0;
		ptr++;
		ptr += _injuries.Deserialize(ptr);
		_extraNeili = *(int*)ptr;
		ptr += 4;
		_consummateLevel = (sbyte)(*ptr);
		ptr++;
		ptr += _baseLifeSkillQualifications.Deserialize(ptr);
		_lifeSkillQualificationGrowthType = (sbyte)(*ptr);
		ptr++;
		ptr += _baseCombatSkillQualifications.Deserialize(ptr);
		_combatSkillQualificationGrowthType = (sbyte)(*ptr);
		ptr++;
		ptr += _resources.Deserialize(ptr);
		_lovingItemSubType = *(short*)ptr;
		ptr += 2;
		_hatingItemSubType = *(short*)ptr;
		ptr += 2;
		ptr += _fullName.Deserialize(ptr);
		ptr += _monasticTitle.Deserialize(ptr);
		ptr += _avatar.Deserialize(ptr);
		ptr += _genome.Deserialize(ptr);
		ptr += _currMainAttributes.Deserialize(ptr);
		ptr += _poisoned.Deserialize(ptr);
		if (_injuriesRecoveryProgress.Length != 14)
		{
			throw new Exception("Elements count of field _injuriesRecoveryProgress is not equal to declaration");
		}
		for (int i = 0; i < 14; i++)
		{
			_injuriesRecoveryProgress[i] = ptr[i];
		}
		ptr += 14;
		_currNeili = *(int*)ptr;
		ptr += 4;
		_loopingNeigong = *(short*)ptr;
		ptr += 2;
		ptr += _baseNeiliAllocation.Deserialize(ptr);
		ptr += _extraNeiliAllocation.Deserialize(ptr);
		ptr += _baseNeiliProportionOfFiveElements.Deserialize(ptr);
		_hobbyExpirationDate = *(int*)ptr;
		ptr += 4;
		_lovingItemRevealed = *ptr != 0;
		ptr++;
		_hatingItemRevealed = *ptr != 0;
		ptr++;
		_legitimateBoysCount = (sbyte)(*ptr);
		ptr++;
		ptr += _birthLocation.Deserialize(ptr);
		ptr += _location.Deserialize(ptr);
		if (_equipment.Length != 12)
		{
			throw new Exception("Elements count of field _equipment is not equal to declaration");
		}
		for (int j = 0; j < 12; j++)
		{
			ItemKey itemKey = default(ItemKey);
			ptr += itemKey.Deserialize(ptr);
			_equipment[j] = itemKey;
		}
		ptr += _eatingItems.Deserialize(ptr);
		if (_equippedCombatSkills.Length != 48)
		{
			throw new Exception("Elements count of field _equippedCombatSkills is not equal to declaration");
		}
		for (int k = 0; k < 48; k++)
		{
			_equippedCombatSkills[k] = ((short*)ptr)[k];
		}
		ptr += 96;
		if (_combatSkillAttainmentPanels.Length != 126)
		{
			throw new Exception("Elements count of field _combatSkillAttainmentPanels is not equal to declaration");
		}
		for (int l = 0; l < 126; l++)
		{
			_combatSkillAttainmentPanels[l] = ((short*)ptr)[l];
		}
		ptr += 252;
		ptr += _preexistenceCharIds.Deserialize(ptr);
		_xiangshuInfection = *ptr;
		ptr++;
		_currAge = *(short*)ptr;
		ptr += 2;
		_exp = *(int*)ptr;
		ptr += 4;
		_externalRelationState = *ptr;
		ptr++;
		_kidnapperId = *(int*)ptr;
		ptr += 4;
		_leaderId = *(int*)ptr;
		ptr += 4;
		_factionId = *(int*)ptr;
		ptr += 4;
		ptr += _actionEnergies.Deserialize(ptr);
		ptr += _prioritizedActionCooldowns.Deserialize(ptr);
		ptr += 4;
		ushort num = *(ushort*)ptr;
		ptr += 2;
		_featureIds.Clear();
		for (int m = 0; m < num; m++)
		{
			_featureIds.Add(((short*)ptr)[m]);
		}
		ptr += 2 * num;
		ptr += 4;
		ushort num2 = *(ushort*)ptr;
		ptr += 2;
		_learnedLifeSkills.Clear();
		for (int n = 0; n < num2; n++)
		{
			LifeSkillItem item = default(LifeSkillItem);
			ptr += item.Deserialize(ptr);
			_learnedLifeSkills.Add(item);
		}
		ptr += 4;
		ushort num3 = *(ushort*)ptr;
		ptr += 2;
		_potentialFeatureIds.Clear();
		for (int num4 = 0; num4 < num3; num4++)
		{
			_potentialFeatureIds.Add(((short*)ptr)[num4]);
		}
		ptr += 2 * num3;
		ptr += 4;
		ushort num5 = *(ushort*)ptr;
		ptr += 2;
		_fameActionRecords.Clear();
		for (int num6 = 0; num6 < num5; num6++)
		{
			FameActionRecord item2 = default(FameActionRecord);
			ptr += item2.Deserialize(ptr);
			_fameActionRecords.Add(item2);
		}
		ptr += 4;
		ptr += _inventory.Deserialize(ptr);
		ptr += 4;
		ushort num7 = *(ushort*)ptr;
		ptr += 2;
		_learnedCombatSkills.Clear();
		for (int num8 = 0; num8 < num7; num8++)
		{
			_learnedCombatSkills.Add(((short*)ptr)[num8]);
		}
		ptr += 2 * num7;
		ptr += 4;
		ushort num9 = *(ushort*)ptr;
		ptr += 2;
		_skillQualificationBonuses.Clear();
		for (int num10 = 0; num10 < num9; num10++)
		{
			SkillQualificationBonus item3 = default(SkillQualificationBonus);
			ptr += item3.Deserialize(ptr);
			_skillQualificationBonuses.Add(item3);
		}
		ptr += 4;
		ushort num11 = *(ushort*)ptr;
		ptr += 2;
		_personalNeeds.Clear();
		for (int num12 = 0; num12 < num11; num12++)
		{
			GameData.Domains.Character.Ai.PersonalNeed item4 = default(GameData.Domains.Character.Ai.PersonalNeed);
			ptr += item4.Deserialize(ptr);
			_personalNeeds.Add(item4);
		}
		ptr += 4;
		ushort num13 = *(ushort*)ptr;
		ptr += 2;
		_npcTravelTargets.Clear();
		for (int num14 = 0; num14 < num13; num14++)
		{
			NpcTravelTarget item5 = default(NpcTravelTarget);
			ptr += item5.Deserialize(ptr);
			_npcTravelTargets.Add(item5);
		}
		return (int)(ptr - pData);
	}

	internal int GetSerializedSize_Legacy()
	{
		int num = 1058;
		int count = _featureIds.Count;
		int num2 = 2 * count;
		int num3 = 2 + num2;
		num += num3;
		int count2 = _learnedLifeSkills.Count;
		int num4 = 4 * count2;
		int num5 = 2 + num4;
		num += num5;
		int count3 = _potentialFeatureIds.Count;
		int num6 = 2 * count3;
		int num7 = 2 + num6;
		num += num7;
		int count4 = _fameActionRecords.Count;
		int num8 = 8 * count4;
		int num9 = 2 + num8;
		num += num9;
		int serializedSize = _inventory.GetSerializedSize();
		num += serializedSize;
		int count5 = _learnedCombatSkills.Count;
		int num10 = 2 * count5;
		int num11 = 2 + num10;
		num += num11;
		int count6 = _skillQualificationBonuses.Count;
		int num12 = 4 * count6;
		int num13 = 2 + num12;
		num += num13;
		int count7 = _personalNeeds.Count;
		int num14 = 8 * count7;
		int num15 = 2 + num14;
		num += num15;
		int count8 = _npcTravelTargets.Count;
		int num16 = 16 * count8;
		int num17 = 2 + num16;
		return num + num17;
	}

	internal unsafe int Serialize_Legacy(byte* pData)
	{
		byte* ptr = pData;
		*(int*)ptr = _id;
		ptr += 4;
		*(short*)ptr = _templateId;
		ptr += 2;
		*ptr = _creatingType;
		ptr++;
		*ptr = (byte)_gender;
		ptr++;
		*(short*)ptr = _actualAge;
		ptr += 2;
		*ptr = (byte)_birthMonth;
		ptr++;
		*ptr = (byte)_happiness;
		ptr++;
		*(short*)ptr = _baseMorality;
		ptr += 2;
		ptr += _organizationInfo.Serialize(ptr);
		*ptr = (byte)_idealSect;
		ptr++;
		*ptr = (byte)_lifeSkillTypeInterest;
		ptr++;
		*ptr = (byte)_combatSkillTypeInterest;
		ptr++;
		*ptr = (byte)_mainAttributeInterest;
		ptr++;
		*ptr = (_transgender ? ((byte)1) : ((byte)0));
		ptr++;
		*ptr = (_bisexual ? ((byte)1) : ((byte)0));
		ptr++;
		*ptr = (byte)_xiangshuType;
		ptr++;
		*ptr = _monkType;
		ptr++;
		ptr += _baseMainAttributes.Serialize(ptr);
		*(short*)ptr = _health;
		ptr += 2;
		*(short*)ptr = _baseMaxHealth;
		ptr += 2;
		*(short*)ptr = _disorderOfQi;
		ptr += 2;
		*ptr = (_haveLeftArm ? ((byte)1) : ((byte)0));
		ptr++;
		*ptr = (_haveRightArm ? ((byte)1) : ((byte)0));
		ptr++;
		*ptr = (_haveLeftLeg ? ((byte)1) : ((byte)0));
		ptr++;
		*ptr = (_haveRightLeg ? ((byte)1) : ((byte)0));
		ptr++;
		ptr += _injuries.Serialize(ptr);
		*(int*)ptr = _extraNeili;
		ptr += 4;
		*ptr = (byte)_consummateLevel;
		ptr++;
		ptr += _baseLifeSkillQualifications.Serialize(ptr);
		*ptr = (byte)_lifeSkillQualificationGrowthType;
		ptr++;
		ptr += _baseCombatSkillQualifications.Serialize(ptr);
		*ptr = (byte)_combatSkillQualificationGrowthType;
		ptr++;
		ptr += _resources.Serialize(ptr);
		*(short*)ptr = _lovingItemSubType;
		ptr += 2;
		*(short*)ptr = _hatingItemSubType;
		ptr += 2;
		ptr += _fullName.Serialize(ptr);
		ptr += _monasticTitle.Serialize(ptr);
		ptr += _avatar.Serialize(ptr);
		ptr += _genome.Serialize(ptr);
		ptr += _currMainAttributes.Serialize(ptr);
		ptr += _poisoned.Serialize(ptr);
		if (_injuriesRecoveryProgress.Length != 14)
		{
			throw new Exception("Elements count of field _injuriesRecoveryProgress is not equal to declaration");
		}
		for (int i = 0; i < 14; i++)
		{
			ptr[i] = _injuriesRecoveryProgress[i];
		}
		ptr += 14;
		*(int*)ptr = _currNeili;
		ptr += 4;
		*(short*)ptr = _loopingNeigong;
		ptr += 2;
		ptr += _baseNeiliAllocation.Serialize(ptr);
		ptr += _extraNeiliAllocation.Serialize(ptr);
		ptr += _baseNeiliProportionOfFiveElements.Serialize(ptr);
		*(int*)ptr = _hobbyExpirationDate;
		ptr += 4;
		*ptr = (_lovingItemRevealed ? ((byte)1) : ((byte)0));
		ptr++;
		*ptr = (_hatingItemRevealed ? ((byte)1) : ((byte)0));
		ptr++;
		*ptr = (byte)_legitimateBoysCount;
		ptr++;
		ptr += _birthLocation.Serialize(ptr);
		ptr += _location.Serialize(ptr);
		if (_equipment.Length != 12)
		{
			throw new Exception("Elements count of field _equipment is not equal to declaration");
		}
		for (int j = 0; j < 12; j++)
		{
			ptr += _equipment[j].Serialize(ptr);
		}
		ptr += _eatingItems.Serialize(ptr);
		if (_equippedCombatSkills.Length != 48)
		{
			throw new Exception("Elements count of field _equippedCombatSkills is not equal to declaration");
		}
		for (int k = 0; k < 48; k++)
		{
			((short*)ptr)[k] = _equippedCombatSkills[k];
		}
		ptr += 96;
		if (_combatSkillAttainmentPanels.Length != 126)
		{
			throw new Exception("Elements count of field _combatSkillAttainmentPanels is not equal to declaration");
		}
		for (int l = 0; l < 126; l++)
		{
			((short*)ptr)[l] = _combatSkillAttainmentPanels[l];
		}
		ptr += 252;
		ptr += _preexistenceCharIds.Serialize(ptr);
		*ptr = _xiangshuInfection;
		ptr++;
		*(short*)ptr = _currAge;
		ptr += 2;
		*(int*)ptr = _exp;
		ptr += 4;
		*ptr = _externalRelationState;
		ptr++;
		*(int*)ptr = _kidnapperId;
		ptr += 4;
		*(int*)ptr = _leaderId;
		ptr += 4;
		*(int*)ptr = _factionId;
		ptr += 4;
		ptr += _actionEnergies.Serialize(ptr);
		ptr += _prioritizedActionCooldowns.Serialize(ptr);
		int count = _featureIds.Count;
		int num = 2 * count;
		if (num > 65533)
		{
			throw new Exception("Size of field _featureIds must be less than 64KB");
		}
		*(ushort*)ptr = (ushort)(num + 2);
		ptr += 2;
		*(ushort*)ptr = (ushort)count;
		ptr += 2;
		for (int m = 0; m < count; m++)
		{
			((short*)ptr)[m] = _featureIds[m];
		}
		ptr += num;
		int count2 = _learnedLifeSkills.Count;
		int num2 = 4 * count2;
		if (num2 > 65533)
		{
			throw new Exception("Size of field _learnedLifeSkills must be less than 64KB");
		}
		*(ushort*)ptr = (ushort)(num2 + 2);
		ptr += 2;
		*(ushort*)ptr = (ushort)count2;
		ptr += 2;
		for (int n = 0; n < count2; n++)
		{
			ptr += _learnedLifeSkills[n].Serialize(ptr);
		}
		int count3 = _potentialFeatureIds.Count;
		int num3 = 2 * count3;
		if (num3 > 65533)
		{
			throw new Exception("Size of field _potentialFeatureIds must be less than 64KB");
		}
		*(ushort*)ptr = (ushort)(num3 + 2);
		ptr += 2;
		*(ushort*)ptr = (ushort)count3;
		ptr += 2;
		for (int num4 = 0; num4 < count3; num4++)
		{
			((short*)ptr)[num4] = _potentialFeatureIds[num4];
		}
		ptr += num3;
		int count4 = _fameActionRecords.Count;
		int num5 = 8 * count4;
		if (num5 > 65533)
		{
			throw new Exception("Size of field _fameActionRecords must be less than 64KB");
		}
		*(ushort*)ptr = (ushort)(num5 + 2);
		ptr += 2;
		*(ushort*)ptr = (ushort)count4;
		ptr += 2;
		for (int num6 = 0; num6 < count4; num6++)
		{
			ptr += _fameActionRecords[num6].Serialize(ptr);
		}
		byte* ptr2 = ptr;
		ptr += 2;
		ptr += _inventory.Serialize(ptr);
		int num7 = (int)(ptr - ptr2 - 2);
		if (num7 > 65535)
		{
			throw new Exception("Size of field _inventory must be less than 64KB");
		}
		*(ushort*)ptr2 = (ushort)num7;
		int count5 = _learnedCombatSkills.Count;
		int num8 = 2 * count5;
		if (num8 > 65533)
		{
			throw new Exception("Size of field _learnedCombatSkills must be less than 64KB");
		}
		*(ushort*)ptr = (ushort)(num8 + 2);
		ptr += 2;
		*(ushort*)ptr = (ushort)count5;
		ptr += 2;
		for (int num9 = 0; num9 < count5; num9++)
		{
			((short*)ptr)[num9] = _learnedCombatSkills[num9];
		}
		ptr += num8;
		int count6 = _skillQualificationBonuses.Count;
		int num10 = 4 * count6;
		if (num10 > 65533)
		{
			throw new Exception("Size of field _skillQualificationBonuses must be less than 64KB");
		}
		*(ushort*)ptr = (ushort)(num10 + 2);
		ptr += 2;
		*(ushort*)ptr = (ushort)count6;
		ptr += 2;
		for (int num11 = 0; num11 < count6; num11++)
		{
			ptr += _skillQualificationBonuses[num11].Serialize(ptr);
		}
		int count7 = _personalNeeds.Count;
		int num12 = 8 * count7;
		if (num12 > 65533)
		{
			throw new Exception("Size of field _personalNeeds must be less than 64KB");
		}
		*(ushort*)ptr = (ushort)(num12 + 2);
		ptr += 2;
		*(ushort*)ptr = (ushort)count7;
		ptr += 2;
		for (int num13 = 0; num13 < count7; num13++)
		{
			ptr += _personalNeeds[num13].Serialize(ptr);
		}
		int count8 = _npcTravelTargets.Count;
		int num14 = 16 * count8;
		if (num14 > 65533)
		{
			throw new Exception("Size of field _npcTravelTargets must be less than 64KB");
		}
		*(ushort*)ptr = (ushort)(num14 + 2);
		ptr += 2;
		*(ushort*)ptr = (ushort)count8;
		ptr += 2;
		for (int num15 = 0; num15 < count8; num15++)
		{
			ptr += _npcTravelTargets[num15].Serialize(ptr);
		}
		return (int)(ptr - pData);
	}

	internal unsafe int Deserialize_Legacy(byte* pData)
	{
		byte* ptr = pData;
		_id = *(int*)ptr;
		ptr += 4;
		_templateId = *(short*)ptr;
		ptr += 2;
		_creatingType = *ptr;
		ptr++;
		_gender = (sbyte)(*ptr);
		ptr++;
		_actualAge = *(short*)ptr;
		ptr += 2;
		_birthMonth = (sbyte)(*ptr);
		ptr++;
		_happiness = (sbyte)(*ptr);
		ptr++;
		_baseMorality = *(short*)ptr;
		ptr += 2;
		ptr += _organizationInfo.Deserialize(ptr);
		_idealSect = (sbyte)(*ptr);
		ptr++;
		_lifeSkillTypeInterest = (sbyte)(*ptr);
		ptr++;
		_combatSkillTypeInterest = (sbyte)(*ptr);
		ptr++;
		_mainAttributeInterest = (sbyte)(*ptr);
		ptr++;
		_transgender = *ptr != 0;
		ptr++;
		_bisexual = *ptr != 0;
		ptr++;
		_xiangshuType = (sbyte)(*ptr);
		ptr++;
		_monkType = *ptr;
		ptr++;
		ptr += _baseMainAttributes.Deserialize(ptr);
		_health = *(short*)ptr;
		ptr += 2;
		_baseMaxHealth = *(short*)ptr;
		ptr += 2;
		_disorderOfQi = *(short*)ptr;
		ptr += 2;
		_haveLeftArm = *ptr != 0;
		ptr++;
		_haveRightArm = *ptr != 0;
		ptr++;
		_haveLeftLeg = *ptr != 0;
		ptr++;
		_haveRightLeg = *ptr != 0;
		ptr++;
		ptr += _injuries.Deserialize(ptr);
		_extraNeili = *(int*)ptr;
		ptr += 4;
		_consummateLevel = (sbyte)(*ptr);
		ptr++;
		ptr += _baseLifeSkillQualifications.Deserialize(ptr);
		_lifeSkillQualificationGrowthType = (sbyte)(*ptr);
		ptr++;
		ptr += _baseCombatSkillQualifications.Deserialize(ptr);
		_combatSkillQualificationGrowthType = (sbyte)(*ptr);
		ptr++;
		ptr += _resources.Deserialize(ptr);
		_lovingItemSubType = *(short*)ptr;
		ptr += 2;
		_hatingItemSubType = *(short*)ptr;
		ptr += 2;
		ptr += _fullName.Deserialize(ptr);
		ptr += _monasticTitle.Deserialize(ptr);
		ptr += _avatar.Deserialize(ptr);
		ptr += _genome.Deserialize(ptr);
		ptr += _currMainAttributes.Deserialize(ptr);
		ptr += _poisoned.Deserialize(ptr);
		if (_injuriesRecoveryProgress.Length != 14)
		{
			throw new Exception("Elements count of field _injuriesRecoveryProgress is not equal to declaration");
		}
		for (int i = 0; i < 14; i++)
		{
			_injuriesRecoveryProgress[i] = ptr[i];
		}
		ptr += 14;
		_currNeili = *(int*)ptr;
		ptr += 4;
		_loopingNeigong = *(short*)ptr;
		ptr += 2;
		ptr += _baseNeiliAllocation.Deserialize(ptr);
		ptr += _extraNeiliAllocation.Deserialize(ptr);
		ptr += _baseNeiliProportionOfFiveElements.Deserialize(ptr);
		_hobbyExpirationDate = *(int*)ptr;
		ptr += 4;
		_lovingItemRevealed = *ptr != 0;
		ptr++;
		_hatingItemRevealed = *ptr != 0;
		ptr++;
		_legitimateBoysCount = (sbyte)(*ptr);
		ptr++;
		ptr += _birthLocation.Deserialize(ptr);
		ptr += _location.Deserialize(ptr);
		if (_equipment.Length != 12)
		{
			throw new Exception("Elements count of field _equipment is not equal to declaration");
		}
		for (int j = 0; j < 12; j++)
		{
			ItemKey itemKey = default(ItemKey);
			ptr += itemKey.Deserialize(ptr);
			_equipment[j] = itemKey;
		}
		ptr += _eatingItems.Deserialize(ptr);
		if (_equippedCombatSkills.Length != 48)
		{
			throw new Exception("Elements count of field _equippedCombatSkills is not equal to declaration");
		}
		for (int k = 0; k < 48; k++)
		{
			_equippedCombatSkills[k] = ((short*)ptr)[k];
		}
		ptr += 96;
		if (_combatSkillAttainmentPanels.Length != 126)
		{
			throw new Exception("Elements count of field _combatSkillAttainmentPanels is not equal to declaration");
		}
		for (int l = 0; l < 126; l++)
		{
			_combatSkillAttainmentPanels[l] = ((short*)ptr)[l];
		}
		ptr += 252;
		ptr += _preexistenceCharIds.Deserialize(ptr);
		_xiangshuInfection = *ptr;
		ptr++;
		_currAge = *(short*)ptr;
		ptr += 2;
		_exp = *(int*)ptr;
		ptr += 4;
		_externalRelationState = *ptr;
		ptr++;
		_kidnapperId = *(int*)ptr;
		ptr += 4;
		_leaderId = *(int*)ptr;
		ptr += 4;
		_factionId = *(int*)ptr;
		ptr += 4;
		ptr += _actionEnergies.Deserialize(ptr);
		ptr += _prioritizedActionCooldowns.Deserialize(ptr);
		ptr += 2;
		ushort num = *(ushort*)ptr;
		ptr += 2;
		_featureIds.Clear();
		for (int m = 0; m < num; m++)
		{
			_featureIds.Add(((short*)ptr)[m]);
		}
		ptr += 2 * num;
		ptr += 2;
		ushort num2 = *(ushort*)ptr;
		ptr += 2;
		_learnedLifeSkills.Clear();
		for (int n = 0; n < num2; n++)
		{
			LifeSkillItem item = default(LifeSkillItem);
			ptr += item.Deserialize(ptr);
			_learnedLifeSkills.Add(item);
		}
		ptr += 2;
		ushort num3 = *(ushort*)ptr;
		ptr += 2;
		_potentialFeatureIds.Clear();
		for (int num4 = 0; num4 < num3; num4++)
		{
			_potentialFeatureIds.Add(((short*)ptr)[num4]);
		}
		ptr += 2 * num3;
		ptr += 2;
		ushort num5 = *(ushort*)ptr;
		ptr += 2;
		_fameActionRecords.Clear();
		for (int num6 = 0; num6 < num5; num6++)
		{
			FameActionRecord item2 = default(FameActionRecord);
			ptr += item2.Deserialize(ptr);
			_fameActionRecords.Add(item2);
		}
		ptr += 2;
		ptr += _inventory.Deserialize(ptr);
		ptr += 2;
		ushort num7 = *(ushort*)ptr;
		ptr += 2;
		_learnedCombatSkills.Clear();
		for (int num8 = 0; num8 < num7; num8++)
		{
			_learnedCombatSkills.Add(((short*)ptr)[num8]);
		}
		ptr += 2 * num7;
		ptr += 2;
		ushort num9 = *(ushort*)ptr;
		ptr += 2;
		_skillQualificationBonuses.Clear();
		for (int num10 = 0; num10 < num9; num10++)
		{
			SkillQualificationBonus item3 = default(SkillQualificationBonus);
			ptr += item3.Deserialize(ptr);
			_skillQualificationBonuses.Add(item3);
		}
		ptr += 2;
		ushort num11 = *(ushort*)ptr;
		ptr += 2;
		_personalNeeds.Clear();
		for (int num12 = 0; num12 < num11; num12++)
		{
			GameData.Domains.Character.Ai.PersonalNeed item4 = default(GameData.Domains.Character.Ai.PersonalNeed);
			ptr += item4.Deserialize(ptr);
			_personalNeeds.Add(item4);
		}
		ptr += 2;
		ushort num13 = *(ushort*)ptr;
		ptr += 2;
		_npcTravelTargets.Clear();
		for (int num14 = 0; num14 < num13; num14++)
		{
			NpcTravelTarget item5 = default(NpcTravelTarget);
			ptr += item5.Deserialize(ptr);
			_npcTravelTargets.Add(item5);
		}
		return (int)(ptr - pData);
	}

	public unsafe ProtagonistFeatureRelatedStatus OfflineCreateProtagonist(short templateId, short orgMemberId, ProtagonistCreationInfo info, DataContext context)
	{
		IRandomSource random = context.Random;
		CharacterItem characterItem = Config.Character.Instance[templateId];
		if ((string.IsNullOrEmpty(info.Surname) || string.IsNullOrEmpty(info.GivenName)) && info.InscribedChar == null)
		{
			throw new Exception("Surname and given name can neither be null nor empty");
		}
		int num = ((info.Surname != null) ? DomainManager.World.RegisterCustomText(context, info.Surname) : (-1));
		int num2 = ((info.GivenName != null) ? DomainManager.World.RegisterCustomText(context, info.GivenName) : (-1));
		bool flag = num >= 0 && num2 >= 0;
		_fullName = (flag ? new FullName(num, num2, -1, -1, -1, -1) : new FullName((num2 >= 0) ? num2 : num, -1, -1));
		_monasticTitle = new MonasticTitle(-1, -1);
		_actualAge = info.Age;
		_currAge = _actualAge;
		_birthMonth = info.InscribedChar?.BirthMonth ?? info.BirthMonth;
		_health = short.MaxValue;
		_baseMaxHealth = info.InscribedChar?.BaseMaxHealth ?? GenerateProtagonistMaxHealth(context.Random);
		_happiness = 0;
		_baseMorality = info.InscribedChar?.Morality ?? info.Morality;
		_location = Location.Invalid;
		sbyte orgTemplateId = _organizationInfo.OrgTemplateId;
		short settlementIdByOrgTemplateId = DomainManager.Organization.GetSettlementIdByOrgTemplateId(orgTemplateId);
		_organizationInfo = new OrganizationInfo(orgTemplateId, _organizationInfo.Grade, principal: true, settlementIdByOrgTemplateId);
		_avatar = info.InscribedChar?.Avatar ?? info.Avatar;
		if (_gender != _avatar.GetGender())
		{
			_transgender = true;
		}
		if (random.CheckPercentProb(20))
		{
			_bisexual = true;
		}
		_loopingNeigong = -1;
		OfflineInitializeBaseNeiliProportionOfFiveElements();
		_resources.Initialize();
		_resources.Items[0] = 50;
		_resources.Items[6] = 990;
		OfflineCreateEquipmentAndInventoryItems(context, characterItem, AgeGroup.GetAgeGroup(_actualAge));
		OfflineReplaceProtagonistClothing(context, info.ClothingTemplateId);
		(TemplateKey, int)[] protagonistInitialItems = ProtagonistInitialItems;
		for (int i = 0; i < protagonistInitialItems.Length; i++)
		{
			(TemplateKey, int) tuple = protagonistInitialItems[i];
			OfflineCreateInventoryOnCharacterCreation(context, tuple.Item1.ItemType, tuple.Item1.TemplateId, tuple.Item2);
		}
		_eatingItems.Initialize();
		CombatSkillHelper.InitializeEquippedSkills(_equippedCombatSkills);
		CombatSkillAttainmentPanelsHelper.Initialize(_combatSkillAttainmentPanels);
		List<GameData.Domains.CombatSkill.CombatSkill> combatSkills = OfflineCreateCombatSkills(random, characterItem.PresetCombatSkills);
		if (info.InscribedChar != null)
		{
			_baseMainAttributes = info.InscribedChar.BaseMainAttributes;
			_baseLifeSkillQualifications = info.InscribedChar.BaseLifeSkillQualifications;
			_baseCombatSkillQualifications = info.InscribedChar.BaseCombatSkillQualifications;
			_lifeSkillQualificationGrowthType = info.InscribedChar.LifeSkillQualificationGrowthType;
			_combatSkillQualificationGrowthType = info.InscribedChar.CombatSkillQualificationGrowthType;
			_skillQualificationBonuses.Add(info.InscribedChar.InnateSkillQualificationBonuses[0]);
			_skillQualificationBonuses.Add(info.InscribedChar.InnateSkillQualificationBonuses[1]);
		}
		else
		{
			OrganizationMemberItem organizationMemberItem = OrganizationMember.Instance[orgMemberId];
			_baseMainAttributes = CharacterCreation.CreateMainAttributes(random, organizationMemberItem.Grade, organizationMemberItem.MainAttributesAdjust);
			_baseLifeSkillQualifications = CharacterCreation.CreateLifeSkillQualifications(random, organizationMemberItem.Grade, organizationMemberItem.LifeSkillsAdjust);
			_baseCombatSkillQualifications = CharacterCreation.CreateCombatSkillQualifications(random, organizationMemberItem.Grade, organizationMemberItem.CombatSkillsAdjust);
			_lifeSkillQualificationGrowthType = (sbyte)random.Next(3);
			_combatSkillQualificationGrowthType = (sbyte)random.Next(3);
			_skillQualificationBonuses.Add(GenerateRandomInnateSkillQualificationBonus(context));
			_skillQualificationBonuses.Add(GenerateRandomInnateSkillQualificationBonus(context));
		}
		short num3 = (short)(40 + random.Next(21));
		if (_baseLifeSkillQualifications.Items[7] < num3)
		{
			_baseLifeSkillQualifications.Items[7] = num3;
		}
		(_lovingItemSubType, _hatingItemSubType) = GenerateRandomHobby(random);
		OfflineGenerateRandomIdealSect(random);
		ProtagonistFeatureRelatedStatus result = OfflineApplyProtagonistFeaturesAndGenome(context, info, combatSkills);
		_actionEnergies.Initialize();
		_prioritizedActionCooldowns.Initialize();
		_kidnapperId = -1;
		_leaderId = -1;
		_factionId = -1;
		_srcCharId = -1;
		return result;
	}

	public void OfflineCreateCharacterFromInscription(DataContext context, CreateIntelligentCharacterModification mod, InscribedCharacter inscribedChar, OrganizationInfo targetOrgInfo)
	{
		OrganizationMemberItem orgMemberConfig = OrganizationDomain.GetOrgMemberConfig(targetOrgInfo);
		OrganizationItem orgConfig = Config.Organization.Instance[targetOrgInfo.OrgTemplateId];
		CharacterItem template = Template;
		int num = ((inscribedChar.Surname != null) ? DomainManager.World.RegisterCustomText(context, inscribedChar.Surname) : (-1));
		int num2 = ((inscribedChar.GivenName != null) ? DomainManager.World.RegisterCustomText(context, inscribedChar.GivenName) : (-1));
		_fullName = ((num >= 0 && num2 >= 0) ? new FullName(num, num2, -1, -1, -1, -1) : new FullName((num2 >= 0) ? num2 : num, -1, -1));
		_monasticTitle = new MonasticTitle(-1, -1);
		_organizationInfo = targetOrgInfo;
		_avatar = new AvatarData(inscribedChar.Avatar);
		_currAge = inscribedChar.CurrAge;
		_actualAge = inscribedChar.ActualAge;
		_birthMonth = inscribedChar.BirthMonth;
		_gender = inscribedChar.Gender;
		_baseMorality = inscribedChar.Morality;
		_baseMaxHealth = inscribedChar.BaseMaxHealth;
		_health = short.MaxValue;
		_baseMainAttributes = inscribedChar.BaseMainAttributes;
		_baseCombatSkillQualifications = inscribedChar.BaseCombatSkillQualifications;
		_baseLifeSkillQualifications = inscribedChar.BaseLifeSkillQualifications;
		_combatSkillQualificationGrowthType = inscribedChar.CombatSkillQualificationGrowthType;
		_lifeSkillQualificationGrowthType = inscribedChar.LifeSkillQualificationGrowthType;
		_skillQualificationBonuses.AddRange(inscribedChar.InnateSkillQualificationBonuses);
		_featureIds.Add(CharacterDomain.GetBirthdayFeatureId(_birthMonth));
		_featureIds.Add(216);
		_featureIds.Add(195);
		if (inscribedChar.FeatureIds != null)
		{
			_featureIds.AddRange(inscribedChar.FeatureIds);
		}
		_featureIds.Sort(CharacterFeatureHelper.FeatureComparer);
		IRandomSource random = context.Random;
		OfflineCreateGenome(random, null, null, null);
		(_lovingItemSubType, _hatingItemSubType) = GenerateRandomHobby(random);
		OfflineGenerateRandomIdealSect(random);
		_loopingNeigong = -1;
		OfflineInitializeBaseNeiliProportionOfFiveElements();
		int num3 = CalcAgeInfluence(random, this, orgMemberConfig);
		_extraNeili = orgMemberConfig.Neili * num3 / 100;
		_consummateLevel = (sbyte)(orgMemberConfig.ConsummateLevel * num3 / 100);
		sbyte ageGroup = AgeGroup.GetAgeGroup(_actualAge);
		OfflineCreateEquipmentAndInventoryItems(mod, Template, ageGroup);
		OfflineAddPresetOrgMemberEquipmentAndInventoryItems(context, mod, orgMemberConfig, ageGroup);
		OfflineCreateResources(random, orgMemberConfig.TemplateId, 20);
		_eatingItems.Initialize();
		CombatSkillHelper.InitializeEquippedSkills(_equippedCombatSkills);
		CombatSkillAttainmentPanelsHelper.Initialize(_combatSkillAttainmentPanels);
		OfflineCreateCombatSkills(random, template.PresetCombatSkills, mod);
		OfflineAddPresetOrgMemberCombatSkills(context, mod, orgConfig, orgMemberConfig, num3);
		OfflineCreateInitialLifeSkills(random, orgMemberConfig);
		_prioritizedActionCooldowns.Initialize();
		_actionEnergies.Initialize();
		OfflineGenerateRandomActionEnergies(random);
		_kidnapperId = -1;
		_leaderId = -1;
		_factionId = -1;
		_srcCharId = -1;
	}

	public List<GameData.Domains.CombatSkill.CombatSkill> OfflineCreatePresetCharacter(short templateId, DataContext context)
	{
		IRandomSource random = context.Random;
		CharacterItem characterItem = Config.Character.Instance[templateId];
		if (characterItem.CreatingType != 0 && characterItem.CreatingType != 3)
		{
			throw new Exception($"Not allow to create preset character by templateId: {templateId}");
		}
		_monasticTitle = new MonasticTitle(-1, -1);
		_actualAge = characterItem.ActualAge;
		_currAge = ((characterItem.InitCurrAge >= 0) ? characterItem.InitCurrAge : _actualAge);
		_birthMonth = ((characterItem.BirthMonth >= 0) ? characterItem.BirthMonth : ((sbyte)random.Next(12)));
		if (_gender == -1)
		{
			_gender = Gender.GetRandom(random);
		}
		sbyte orgTemplateId = _organizationInfo.OrgTemplateId;
		short settlementIdByOrgTemplateId = DomainManager.Organization.GetSettlementIdByOrgTemplateId(orgTemplateId);
		_organizationInfo = new OrganizationInfo(orgTemplateId, _organizationInfo.Grade, principal: true, settlementIdByOrgTemplateId);
		_avatar = AvatarManager.Instance.GetRandomAvatar(random, _gender, _transgender, characterItem.PresetBodyType, characterItem.BaseAttraction);
		CharacterCreation.CreateFeatures(context.Random, this);
		OfflineCreateGenome(context.Random, null, null, null);
		_loopingNeigong = -1;
		NeiliProportionOfFiveElements presetNeiliProportionOfFiveElements = characterItem.PresetNeiliProportionOfFiveElements;
		if (presetNeiliProportionOfFiveElements.SumCheck() > 0)
		{
			_baseNeiliProportionOfFiveElements = presetNeiliProportionOfFiveElements;
		}
		else
		{
			OfflineInitializeBaseNeiliProportionOfFiveElements();
		}
		CombatSkillHelper.InitializeEquippedSkills(_equippedCombatSkills);
		CombatSkillAttainmentPanelsHelper.Initialize(_combatSkillAttainmentPanels);
		OfflineCreateEquipmentAndInventoryItems(context, characterItem, AgeGroup.GetAgeGroup(_actualAge));
		List<GameData.Domains.CombatSkill.CombatSkill> result = OfflineCreateCombatSkills(random, characterItem.PresetCombatSkills);
		OfflineCreatePresetLifeSkillsByGradeConfig();
		_eatingItems.Initialize();
		if (_idealSect <= 0)
		{
			OfflineGenerateRandomIdealSect(random);
		}
		if (_lifeSkillTypeInterest < 0)
		{
			_lifeSkillTypeInterest = (sbyte)random.Next(16);
		}
		if (_combatSkillTypeInterest < 0)
		{
			_combatSkillTypeInterest = (sbyte)random.Next(14);
		}
		if (_mainAttributeInterest < 0)
		{
			_mainAttributeInterest = (sbyte)random.Next(6);
		}
		_birthLocation = Location.Invalid;
		_location = Location.Invalid;
		_skillQualificationBonuses.Add(GenerateRandomInnateSkillQualificationBonus(context));
		_skillQualificationBonuses.Add(GenerateRandomInnateSkillQualificationBonus(context));
		_actionEnergies.Initialize();
		_prioritizedActionCooldowns.Initialize();
		_kidnapperId = -1;
		_leaderId = -1;
		_factionId = -1;
		_srcCharId = -1;
		return result;
	}

	public void OfflineSetSrcCharId(int charId)
	{
		Tester.Assert(DomainManager.Character.IsTemporaryIntelligentCharacter(_id));
		_srcCharId = charId;
	}

	public void OfflineSetCopySource(int charId)
	{
		_eatingItems.Initialize();
		_injuries.Initialize();
		_poisoned.Initialize();
		_inventory.Items.Clear();
		_equipment = new ItemKey[12];
		for (int i = 0; i < 12; i++)
		{
			_equipment[i] = ItemKey.Invalid;
		}
		_kidnapperId = -1;
		_leaderId = -1;
		_factionId = -1;
		_srcCharId = charId;
	}

	public void OfflineCreateIntelligentCharacter(DataContext context, CreateIntelligentCharacterModification mod, ref IntelligentCharacterCreationInfo info)
	{
		IRandomSource random = context.Random;
		CharacterItem characterItem = Config.Character.Instance[info.CharTemplateId];
		if (characterItem.CreatingType != 1)
		{
			throw new Exception($"Not allow to create intelligent character by templateId: {info.CharTemplateId}");
		}
		sbyte b = info.OrgInfo.OrgTemplateId;
		bool flag = info.Location.AreaId == 138 && b == 38;
		if (flag)
		{
			sbyte taiwuVillageStateTemplateId = DomainManager.World.GetTaiwuVillageStateTemplateId();
			b = MapState.Instance[taiwuVillageStateTemplateId].SectID;
		}
		short memberId = OrganizationDomain.GetMemberId(b, info.OrgInfo.Grade);
		OrganizationItem organizationItem = Config.Organization.Instance[b];
		OrganizationMemberItem organizationMemberItem = OrganizationMember.Instance[memberId];
		_organizationInfo = info.OrgInfo;
		_monasticTitle = new MonasticTitle(-1, -1);
		OfflineCreateRandomName(context, ref info, mod, (info.Race >= 0) ? info.Race : characterItem.Race);
		_actualAge = ((info.Age >= 0) ? info.Age : GenerateRandomAge(random));
		_currAge = _actualAge;
		_birthMonth = ((info.BirthMonth >= 0) ? info.BirthMonth : ((characterItem.BirthMonth >= 0) ? characterItem.BirthMonth : ((sbyte)random.Next(12))));
		_health = short.MaxValue;
		_baseMaxHealth = GenerateRandomMaxHealth(random);
		int num = CalcAgeInfluence(random, this, organizationMemberItem);
		_happiness = (sbyte)random.Next(-59, 60);
		_birthLocation = info.Location;
		_location = info.Location;
		if (info.DestinyType >= 0)
		{
			DestinyTypeItem destinyTypeItem = DestinyType.Instance[info.DestinyType];
			_baseMorality = (short)random.Next((int)destinyTypeItem.MoralityRange[0], (int)destinyTypeItem.MoralityRange[1]);
			info.GrowingSectGrade = (sbyte)random.Next((int)destinyTypeItem.OrganizationGradeRange[0], (int)destinyTypeItem.OrganizationGradeRange[1]);
		}
		else if (organizationItem.MainMorality != short.MinValue && random.CheckPercentProb(60))
		{
			int value = organizationItem.MainMorality + random.Next(301) - 150;
			_baseMorality = (short)Math.Clamp(value, -500, 500);
		}
		else
		{
			_baseMorality = (short)(random.Next(1001) - 500);
		}
		if (info.Gender != -1)
		{
			_gender = info.Gender;
			_transgender = info.Transgender;
		}
		else if (random.CheckPercentProb(3))
		{
			_transgender = true;
		}
		if (random.CheckPercentProb(10))
		{
			_bisexual = true;
		}
		if (ProfessionSkillHandle.BuddhistMonkSkill_IsDirectedSamsaraCharacter(mod.ReincarnationCharId))
		{
			if (DomainManager.Character.TryGetDeadCharacter(mod.ReincarnationCharId, out var character))
			{
				_gender = character.Gender;
				_transgender = _gender != character.Avatar.Gender;
			}
			else
			{
				AdaptableLog.Warning($"[Debug] Creation Intelligent Character {mod.ReincarnationCharId}. Cannot find dead character.");
			}
		}
		OfflineCreateAttractionAndAvatar(context, characterItem.PresetBodyType, ref info, haveHair: false);
		CharacterCreation.CreateFeatures(random, this, ref info);
		if (info.SpecifyGenome)
		{
			_genome = info.Genome;
		}
		else
		{
			OfflineCreateGenome(random, info.Mother, info.PregnantState, info.ActualFather);
		}
		if (_lifeSkillTypeInterest < 0)
		{
			_lifeSkillTypeInterest = (sbyte)random.Next(16);
		}
		if (_combatSkillTypeInterest < 0)
		{
			_combatSkillTypeInterest = (sbyte)random.Next(14);
		}
		if (_mainAttributeInterest < 0)
		{
			_mainAttributeInterest = (sbyte)random.Next(6);
		}
		if (_idealSect <= 0)
		{
			OfflineGenerateRandomIdealSect(random);
		}
		(short, short) tuple = GenerateRandomHobby(random);
		_lovingItemSubType = tuple.Item1;
		_hatingItemSubType = tuple.Item2;
		sbyte growingSectGrade = CharacterCreation.CalcGrowingSectGradeAndAssignWeights(context.Random, ref info, _idealSect);
		if (info.GrowingSectGrade < 0)
		{
			info.GrowingSectGrade = growingSectGrade;
		}
		mod.GrowingSectGrade = info.GrowingSectGrade;
		_baseMainAttributes = CharacterCreation.CreateMainAttributes(random, ref info);
		_baseLifeSkillQualifications = CharacterCreation.CreateLifeSkillQualifications(random, ref info);
		_baseCombatSkillQualifications = CharacterCreation.CreateCombatSkillQualifications(random, ref info);
		_lifeSkillQualificationGrowthType = ((info.LifeSkillQualificationGrowthType < 0) ? ((sbyte)random.Next(3)) : info.LifeSkillQualificationGrowthType);
		_combatSkillQualificationGrowthType = ((info.CombatSkillQualificationGrowthType < 0) ? ((sbyte)random.Next(3)) : info.CombatSkillQualificationGrowthType);
		_skillQualificationBonuses.Add(GenerateRandomInnateSkillQualificationBonus(context));
		_skillQualificationBonuses.Add(GenerateRandomInnateSkillQualificationBonus(context));
		_loopingNeigong = -1;
		OfflineInitializeBaseNeiliProportionOfFiveElements();
		_extraNeili = organizationMemberItem.Neili * num / 100;
		_consummateLevel = (sbyte)(organizationMemberItem.ConsummateLevel * num / 100);
		sbyte ageGroup = AgeGroup.GetAgeGroup(_actualAge);
		OfflineCreateEquipmentAndInventoryItems(mod, characterItem, ageGroup);
		if (flag)
		{
			OfflineAddPresetOrgMemberEquipmentAndInventoryItems(context, mod, OrganizationDomain.GetOrgMemberConfig(b, (sbyte)(_organizationInfo.Grade / 2)), ageGroup);
		}
		else
		{
			OfflineAddPresetOrgMemberEquipmentAndInventoryItems(context, mod, organizationMemberItem, ageGroup);
		}
		OfflineCreateResources(random, memberId, 20);
		_eatingItems.Initialize();
		CombatSkillHelper.InitializeEquippedSkills(_equippedCombatSkills);
		CombatSkillAttainmentPanelsHelper.Initialize(_combatSkillAttainmentPanels);
		OfflineCreateCombatSkills(random, characterItem.PresetCombatSkills, mod);
		if (info.InitializeSectSkills)
		{
			OfflineAddPresetOrgMemberCombatSkills(context, mod, organizationItem, organizationMemberItem, num);
		}
		if (ageGroup != 0 && info.InitializeSectSkills)
		{
			OfflineCreateInitialLifeSkills(random, organizationMemberItem);
		}
		_prioritizedActionCooldowns.Initialize();
		_actionEnergies.Initialize();
		OfflineGenerateRandomActionEnergies(random);
		_kidnapperId = -1;
		_leaderId = -1;
		_factionId = -1;
		_srcCharId = -1;
	}

	public void OfflineCreateTemporaryIntelligentCharacter(DataContext context, CreateIntelligentCharacterModification mod, ref IntelligentCharacterCreationInfo info, ref TemporaryIntelligentCharacterCreationInfo tmpInfo)
	{
		IRandomSource random = context.Random;
		CharacterItem characterItem = Config.Character.Instance[info.CharTemplateId];
		if (characterItem.CreatingType != 1)
		{
			throw new Exception($"Not allow to create intelligent character by templateId: {info.CharTemplateId}");
		}
		sbyte orgTemplateId = info.OrgInfo.OrgTemplateId;
		short memberId = OrganizationDomain.GetMemberId(orgTemplateId, info.OrgInfo.Grade);
		OrganizationItem organizationItem = Config.Organization.Instance[orgTemplateId];
		OrganizationMemberItem organizationMemberItem = OrganizationMember.Instance[memberId];
		_organizationInfo = info.OrgInfo;
		_monasticTitle = new MonasticTitle(-1, -1);
		OfflineCreateRandomName(context, ref info, mod, characterItem.Race);
		_actualAge = ((info.Age >= 0) ? info.Age : GenerateRandomAge(random));
		_currAge = _actualAge;
		_birthMonth = ((characterItem.BirthMonth >= 0) ? characterItem.BirthMonth : ((sbyte)random.Next(12)));
		_happiness = tmpInfo.Happiness ?? ((sbyte)random.Next(-59, 60));
		_health = short.MaxValue;
		_baseMaxHealth = GenerateRandomMaxHealth(random);
		_location = info.Location;
		if (tmpInfo.Morality.HasValue)
		{
			_baseMorality = tmpInfo.Morality.Value;
		}
		else if (organizationItem.MainMorality != short.MinValue && random.CheckPercentProb(60))
		{
			int value = organizationItem.MainMorality + random.Next(301) - 150;
			_baseMorality = (short)Math.Clamp(value, -500, 500);
		}
		else
		{
			_baseMorality = (short)(random.Next(1001) - 500);
		}
		if (random.CheckPercentProb(3))
		{
			_transgender = true;
		}
		if (random.CheckPercentProb(20))
		{
			_bisexual = true;
		}
		OfflineCreateAttractionAndAvatar(context, characterItem.PresetBodyType, ref info, tmpInfo.HairNoSkinHead);
		CharacterCreation.CreateFeatures(random, this, ref info);
		if (tmpInfo.IsCompletelyInfected.HasValue && tmpInfo.IsCompletelyInfected.Value)
		{
			OfflineAddFeature(218, removeMutexFeature: true);
			_xiangshuInfection = 200;
			_organizationInfo = new OrganizationInfo(20, _organizationInfo.Grade, principal: true, -1);
		}
		if (info.SpecifyGenome)
		{
			_genome = info.Genome;
		}
		else
		{
			OfflineCreateGenome(random, info.Mother, info.PregnantState, info.ActualFather);
		}
		int num = Math.Clamp(CalcAgeInfluence(random, this, organizationMemberItem), 75, 100);
		if (_lifeSkillTypeInterest < 0)
		{
			_lifeSkillTypeInterest = (sbyte)random.Next(16);
		}
		if (_combatSkillTypeInterest < 0)
		{
			_combatSkillTypeInterest = (sbyte)random.Next(14);
		}
		if (_mainAttributeInterest < 0)
		{
			_mainAttributeInterest = (sbyte)random.Next(6);
		}
		if (_idealSect <= 0)
		{
			OfflineGenerateRandomIdealSect(random);
		}
		(short, short) tuple = GenerateRandomHobby(random);
		_lovingItemSubType = tuple.Item1;
		_hatingItemSubType = tuple.Item2;
		sbyte growingSectGrade = CharacterCreation.CalcGrowingSectGradeAndAssignWeights(context.Random, ref info, _idealSect);
		if (info.GrowingSectGrade < 0)
		{
			info.GrowingSectGrade = growingSectGrade;
		}
		mod.GrowingSectGrade = info.GrowingSectGrade;
		_baseMainAttributes = CharacterCreation.CreateMainAttributes(random, ref info);
		_baseLifeSkillQualifications = CharacterCreation.CreateLifeSkillQualifications(random, ref info);
		_baseCombatSkillQualifications = CharacterCreation.CreateCombatSkillQualifications(random, ref info);
		_lifeSkillQualificationGrowthType = ((info.LifeSkillQualificationGrowthType < 0) ? ((sbyte)random.Next(3)) : info.LifeSkillQualificationGrowthType);
		_combatSkillQualificationGrowthType = ((info.CombatSkillQualificationGrowthType < 0) ? ((sbyte)random.Next(3)) : info.CombatSkillQualificationGrowthType);
		_skillQualificationBonuses.Add(GenerateRandomInnateSkillQualificationBonus(context));
		_skillQualificationBonuses.Add(GenerateRandomInnateSkillQualificationBonus(context));
		_loopingNeigong = -1;
		OfflineInitializeBaseNeiliProportionOfFiveElements();
		_extraNeili = organizationMemberItem.Neili * num / 100;
		_consummateLevel = tmpInfo.ConsummateLevel ?? ((sbyte)(organizationMemberItem.ConsummateLevel * num / 100));
		sbyte ageGroup = AgeGroup.GetAgeGroup(_actualAge);
		OfflineCreateEquipmentAndInventoryItems(mod, characterItem, ageGroup);
		OfflineAddPresetOrgMemberEquipmentAndInventoryItems(context, mod, organizationMemberItem, ageGroup);
		OfflineCreateResources(random, memberId, 20);
		_eatingItems.Initialize();
		CombatSkillHelper.InitializeEquippedSkills(_equippedCombatSkills);
		CombatSkillAttainmentPanelsHelper.Initialize(_combatSkillAttainmentPanels);
		OfflineCreateCombatSkills(random, characterItem.PresetCombatSkills, mod);
		if (info.InitializeSectSkills)
		{
			OfflineAddPresetOrgMemberCombatSkills(context, mod, organizationItem, organizationMemberItem, num);
		}
		if (ageGroup != 0 && info.InitializeSectSkills)
		{
			OfflineCreateInitialLifeSkills(random, organizationMemberItem);
		}
		_prioritizedActionCooldowns.Initialize();
		_actionEnergies.Initialize();
		OfflineGenerateRandomActionEnergies(random);
		_kidnapperId = -1;
		_leaderId = -1;
		_factionId = -1;
		_srcCharId = -1;
	}

	public List<GameData.Domains.CombatSkill.CombatSkill> OfflineCreateRandomEnemy(short templateId, DataContext context)
	{
		IRandomSource random = context.Random;
		CharacterItem characterItem = Config.Character.Instance[templateId];
		if (characterItem.CreatingType != 2)
		{
			throw new Exception($"Not allow to create random enemy by templateId: {templateId}");
		}
		if (_gender == -1)
		{
			_gender = Gender.GetRandom(random);
		}
		_monasticTitle = new MonasticTitle(-1, -1);
		_actualAge = GenerateRandomAge(random, characterItem.ActualAge);
		_currAge = ((characterItem.InitCurrAge >= 0) ? characterItem.InitCurrAge : _actualAge);
		_birthMonth = ((characterItem.BirthMonth >= 0) ? characterItem.BirthMonth : ((sbyte)random.Next(12)));
		_health = short.MaxValue;
		_baseMaxHealth = GenerateRandomMaxHealth(random);
		sbyte orgTemplateId = _organizationInfo.OrgTemplateId;
		short settlementIdByOrgTemplateId = DomainManager.Organization.GetSettlementIdByOrgTemplateId(orgTemplateId);
		_organizationInfo = new OrganizationInfo(orgTemplateId, _organizationInfo.Grade, principal: true, settlementIdByOrgTemplateId);
		sbyte b = characterItem.PresetBodyType;
		if (b < 0)
		{
			b = BodyType.GetRandom(random);
		}
		short num = characterItem.BaseAttraction;
		if (num < 0)
		{
			num = GenerateRandomAttraction(random);
		}
		_avatar = AvatarManager.Instance.GetRandomAvatar(random, _gender, _transgender, b, num);
		CharacterCreation.CreateFeatures(random, this);
		OfflineCreateGenome(context.Random, null, null, null);
		RandomEnemyItem randomEnemyItem = RandomEnemy.Instance[characterItem.RandomEnemyId];
		List<(OrganizationItem, OrganizationMemberItem)> relatedSectAndMembers = GetRelatedSectAndMembers(random, randomEnemyItem, _organizationInfo.Grade);
		short[] randomEnemyMainAttributesAdjust = CharacterCreation.GetRandomEnemyMainAttributesAdjust(random, relatedSectAndMembers);
		short[] randomEnemyLifeSkillsAdjust = CharacterCreation.GetRandomEnemyLifeSkillsAdjust(random, relatedSectAndMembers);
		short[] randomEnemyCombatSkillsAdjust = CharacterCreation.GetRandomEnemyCombatSkillsAdjust(random, relatedSectAndMembers);
		_baseMainAttributes = CharacterCreation.CreateMainAttributes(random, _organizationInfo.Grade, randomEnemyMainAttributesAdjust);
		_baseLifeSkillQualifications = CharacterCreation.CreateLifeSkillQualifications(random, _organizationInfo.Grade, randomEnemyLifeSkillsAdjust);
		_baseCombatSkillQualifications = CharacterCreation.CreateCombatSkillQualifications(random, _organizationInfo.Grade, randomEnemyCombatSkillsAdjust);
		_lifeSkillQualificationGrowthType = (sbyte)random.Next(3);
		_combatSkillQualificationGrowthType = (sbyte)random.Next(3);
		_skillQualificationBonuses.Add(GenerateRandomInnateSkillQualificationBonus(context));
		_skillQualificationBonuses.Add(GenerateRandomInnateSkillQualificationBonus(context));
		_loopingNeigong = -1;
		OfflineInitializeBaseNeiliProportionOfFiveElements();
		sbyte ageGroup = AgeGroup.GetAgeGroup(_actualAge);
		OfflineCreateEquipmentAndInventoryItems(context, characterItem, ageGroup);
		OfflineAddRandomEnemyPresetOrgMemberEquipmentAndInventoryItems(context, randomEnemyItem, relatedSectAndMembers);
		_eatingItems.Initialize();
		CombatSkillHelper.InitializeEquippedSkills(_equippedCombatSkills);
		CombatSkillAttainmentPanelsHelper.Initialize(_combatSkillAttainmentPanels);
		List<GameData.Domains.CombatSkill.CombatSkill> list = OfflineCreateRandomEnemyCombatSkills(random, randomEnemyItem, characterItem.PresetCombatSkills);
		OfflineAddRandomEnemyOrgMemberCombatSkills(context, randomEnemyItem, relatedSectAndMembers, list);
		if (relatedSectAndMembers.Count > 0)
		{
			int num2 = 0;
			for (int i = 0; i < relatedSectAndMembers.Count; i++)
			{
				num2 += relatedSectAndMembers[i].Item2.LifeSkillGradeLimit;
			}
			sbyte skillGradeLimit = (sbyte)(num2 / relatedSectAndMembers.Count);
			OfflineCreateInitialLifeSkills(random, randomEnemyLifeSkillsAdjust, skillGradeLimit);
		}
		_location = Location.Invalid;
		if (_idealSect <= 0)
		{
			OfflineGenerateRandomIdealSect(random);
		}
		if (_lifeSkillTypeInterest < 0)
		{
			_lifeSkillTypeInterest = (sbyte)random.Next(16);
		}
		if (_combatSkillTypeInterest < 0)
		{
			_combatSkillTypeInterest = (sbyte)random.Next(14);
		}
		if (_mainAttributeInterest < 0)
		{
			_mainAttributeInterest = (sbyte)random.Next(6);
		}
		_actionEnergies.Initialize();
		_prioritizedActionCooldowns.Initialize();
		_kidnapperId = -1;
		_leaderId = -1;
		_factionId = -1;
		return list;
	}

	public void OfflineSetCloseFriendFields(DataContext context, short morality)
	{
		_organizationInfo.Grade = 0;
		_featureIds.Clear();
		_potentialFeatureIds.Clear();
		_featureIds.Add(164);
		_featureIds.Add(554);
		_bisexual = false;
		Genome.EraseAffectedRecessiveTraits(ref _genome);
		_baseMorality = morality;
	}

	public void RemoveUnequippedCombatSkills(DataContext context, sbyte retainingProb = 25)
	{
		HashSet<short> hashSet = new HashSet<short>();
		CombatSkillEquipment combatSkillEquipment = GetCombatSkillEquipment();
		combatSkillEquipment.GetValidSkills(hashSet);
		List<short> list = new List<short>(_learnedCombatSkills.Count);
		int i = 0;
		for (int count = _learnedCombatSkills.Count; i < count; i++)
		{
			short num = _learnedCombatSkills[i];
			if (hashSet.Contains(num))
			{
				list.Add(num);
				continue;
			}
			if (CollectionUtils.Contains(_combatSkillAttainmentPanels, num))
			{
				list.Add(num);
				continue;
			}
			CombatSkillKey objectId = new CombatSkillKey(_id, num);
			GameData.Domains.CombatSkill.CombatSkill element_CombatSkills = DomainManager.CombatSkill.GetElement_CombatSkills(objectId);
			if (element_CombatSkills.GetObtainedNeili() > 0 || context.Random.CheckPercentProb(retainingProb))
			{
				list.Add(num);
			}
			else
			{
				DomainManager.CombatSkill.RemoveCombatSkill(_id, num);
			}
		}
		_learnedCombatSkills.Clear();
		_learnedCombatSkills.AddRange(list);
		SetLearnedCombatSkills(_learnedCombatSkills, context);
	}

	public void AddEquipmentAndInventoryItems(DataContext context, PresetEquipmentItem[] equipment, List<PresetInventoryItem> inventory)
	{
		OfflineAddEquipmentAndInventoryItems(context, equipment, inventory);
		SetEquipment(_equipment, context);
		SetInventory(_inventory, context);
	}

	public void AddSkillBooksAndWeapons(DataContext context, List<InventoryCombatSkillBookParams> skillBooks, HashSet<short> skillWeaponIds)
	{
		OfflineAddSkillBooksAndWeapons(context, skillBooks, skillWeaponIds);
		SetInventory(_inventory, context);
	}

	public void RemoveUnequippedEquipment(DataContext context)
	{
		_itemsToBeDeleted.Clear();
		foreach (KeyValuePair<ItemKey, int> item in _inventory.Items)
		{
			item.Deconstruct(out var key, out var _);
			ItemKey itemKey = key;
			EquipmentBase equipmentBase = DomainManager.Item.TryGetBaseEquipment(itemKey);
			if (equipmentBase != null)
			{
				_itemsToBeDeleted.Add(itemKey);
			}
		}
		int i = 0;
		for (int count = _itemsToBeDeleted.Count; i < count; i++)
		{
			ItemKey itemKey2 = _itemsToBeDeleted[i];
			_inventory.OfflineRemove(itemKey2);
			DomainManager.Item.RemoveItem(context, itemKey2);
		}
		SetInventory(_inventory, context);
	}

	public void OfflineSetId(int id)
	{
		_id = id;
	}

	public void OfflineSetGenderInfo(sbyte gender, bool transgender)
	{
		_gender = gender;
		_transgender = transgender;
	}

	public void OfflineSetAvatar(AvatarData avatar)
	{
		_avatar = avatar;
	}

	public void OfflineSetTemplateId(short templateId)
	{
		_templateId = templateId;
	}

	public void OfflineSetCreatingType(byte creatingType)
	{
		_creatingType = creatingType;
	}

	public void OfflineSetXiangshuType(sbyte xiangshuType)
	{
		_xiangshuType = xiangshuType;
	}

	public void OfflineSetBirthLocation(Location location)
	{
		_birthLocation = location;
	}

	public unsafe void OfflineSetPreexistenceCharId(DataContext context, int reincarnationCharId)
	{
		DeadCharacter deadCharacter = DomainManager.Character.GetDeadCharacter(reincarnationCharId);
		ref PreexistenceCharIds preexistenceCharIds = ref deadCharacter.PreexistenceCharIds;
		int num = deadCharacter.FeatureIds.FindIndex(IsReincarnationBonusFeature);
		if (num >= 0)
		{
			OfflineAddFeature(deadCharacter.FeatureIds[num], removeMutexFeature: true);
		}
		OfflineInheritPreexistenceCharFeatures(context.Random, deadCharacter.FeatureIds);
		if (preexistenceCharIds.Count < 9)
		{
			PreexistenceCharIds preexistenceCharIds2 = preexistenceCharIds;
			preexistenceCharIds2.Add(context.Random, reincarnationCharId);
			_preexistenceCharIds = preexistenceCharIds2;
			return;
		}
		int num2 = 0;
		for (int i = 0; i < preexistenceCharIds.Count; i++)
		{
			int num3 = preexistenceCharIds.CharIds[i];
			if (num3 >= 0)
			{
				DeadCharacter deadCharacter2 = DomainManager.Character.TryGetDeadCharacter(num3);
				if (deadCharacter2 != null)
				{
					num2 = ((deadCharacter2.FameType != 3 && deadCharacter2.FameType != -2) ? (num2 + ((deadCharacter2.FameType > 3) ? 1 : (-1))) : (num2 + (context.Random.NextBool() ? 1 : (-1))));
				}
			}
		}
		short num4 = ((num2 >= 0) ? ((short)context.Random.Next(257, 262)) : ((short)context.Random.Next(394, 399)));
		AdaptableLog.Info($"Reincarnation bonus feature {CharacterFeature.Instance[num4].Name} is added to character {this} with diff {num2}");
		OfflineAddFeature(num4, removeMutexFeature: true);
		DomainManager.Character.RecordDeletedFromOthersPreexistence(context, ref preexistenceCharIds);
		_preexistenceCharIds.Reset();
		_preexistenceCharIds.Add(context.Random, reincarnationCharId);
	}

	public void OfflineInheritPreexistenceCharFeatures(IRandomSource randomSource, List<short> featureIds)
	{
		Span<short> span = stackalloc short[featureIds.Count];
		SpanList<short> spanList = span;
		foreach (short featureId2 in featureIds)
		{
			if (CharacterFeature.Instance[featureId2].InheritableThroughSamsara)
			{
				spanList.Add(featureId2);
			}
		}
		if (spanList.Count <= 0)
		{
			return;
		}
		int num = randomSource.Next(3) + 1;
		for (int i = 0; i < num; i++)
		{
			if (spanList.Count <= 0)
			{
				break;
			}
			int index = randomSource.Next(spanList.Count);
			short featureId = spanList[index];
			spanList.SwapRemove(index);
			OfflineAddFeature(featureId, removeMutexFeature: true, removeLowerOnly: true);
		}
	}

	internal unsafe void ClearPreexistenceCharIds(DataContext context)
	{
		_preexistenceCharIds.Reset();
		SetModifiedAndInvalidateInfluencedCache(64, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 954u, 52);
			ptr += _preexistenceCharIds.Serialize(ptr);
		}
	}

	public void OfflineCreateRandomName(IRandomSource random, int customSurnameId = -1, int customGivenNameId = -1)
	{
		if (Config.Character.Instance[_templateId].Race == 0)
		{
			if (_gender == 0)
			{
				_fullName = CharacterDomain.GenerateRandomHanName(random, customSurnameId, -1, _gender, -1);
			}
			else
			{
				_fullName = CharacterDomain.GenerateRandomHanName(random, customSurnameId, -1, _gender, 0);
			}
		}
		else
		{
			_fullName = CharacterDomain.GenerateRandomZangName(random, _gender);
		}
	}

	internal void SetCharacterUnamedStatus(DataContext context, bool unamed)
	{
		if (unamed)
		{
			_fullName.Type |= 16;
		}
		else
		{
			_fullName.Type &= -17;
		}
		SetFullName(_fullName, context);
	}

	public void OfflineRecreateByGrowingSect(IRandomSource random, sbyte growingOrgTemplateId, sbyte growingGrade)
	{
		OrganizationMemberItem orgMemberConfig = OrganizationDomain.GetOrgMemberConfig(growingOrgTemplateId, growingGrade);
		_baseMainAttributes = CharacterCreation.CreateMainAttributes(random, growingGrade, orgMemberConfig.MainAttributesAdjust);
		_baseLifeSkillQualifications = CharacterCreation.CreateLifeSkillQualifications(random, growingGrade, orgMemberConfig.LifeSkillsAdjust);
		_baseCombatSkillQualifications = CharacterCreation.CreateCombatSkillQualifications(random, growingGrade, orgMemberConfig.CombatSkillsAdjust);
		_lifeSkillQualificationGrowthType = (sbyte)random.Next(3);
		_combatSkillQualificationGrowthType = (sbyte)random.Next(3);
	}

	public void RecreateMainAttributes(DataContext context, sbyte growingOrgTemplateId, sbyte growingGrade)
	{
		IRandomSource random = context.Random;
		short index = ChooseGrowingSectMember(random, growingOrgTemplateId, growingGrade);
		OrganizationMemberItem organizationMemberItem = OrganizationMember.Instance[index];
		_baseMainAttributes = CharacterCreation.CreateMainAttributes(random, growingGrade, organizationMemberItem.MainAttributesAdjust);
		SetBaseMainAttributes(_baseMainAttributes, context);
	}

	public void RecreateCombatSkillQualifications(DataContext context, sbyte growingOrgTemplateId, sbyte growingGrade)
	{
		IRandomSource random = context.Random;
		short index = ChooseGrowingSectMember(random, growingOrgTemplateId, growingGrade);
		OrganizationMemberItem organizationMemberItem = OrganizationMember.Instance[index];
		_baseCombatSkillQualifications = CharacterCreation.CreateCombatSkillQualifications(random, growingGrade, organizationMemberItem.CombatSkillsAdjust);
		SetBaseCombatSkillQualifications(ref _baseCombatSkillQualifications, context);
	}

	public void RecreateLifeSkillQualifications(DataContext context, sbyte growingOrgTemplateId, sbyte growingGrade)
	{
		IRandomSource random = context.Random;
		short index = ChooseGrowingSectMember(random, growingOrgTemplateId, growingGrade);
		OrganizationMemberItem organizationMemberItem = OrganizationMember.Instance[index];
		_baseLifeSkillQualifications = CharacterCreation.CreateLifeSkillQualifications(random, growingGrade, organizationMemberItem.LifeSkillsAdjust);
		SetBaseLifeSkillQualifications(ref _baseLifeSkillQualifications, context);
	}

	public void RecreateFeatures(DataContext context, int positiveFeatureRate, sbyte featureMedalType)
	{
		for (int num = _featureIds.Count - 1; num >= 0; num--)
		{
			short num2 = _featureIds[num];
			CharacterFeatureItem characterFeatureItem = CharacterFeature.Instance[num2];
			if (characterFeatureItem.Basic && characterFeatureItem.MutexGroupId != 168)
			{
				_featureIds.Remove(num2);
			}
		}
		_potentialFeatureIds.Clear();
		int num3 = CharacterCreation.GenerateRandomBasicFeaturesCount(context.Random);
		if (num3 <= 0)
		{
			return;
		}
		bool isProtagonist = _id == DomainManager.Taiwu.GetTaiwuCharId();
		Dictionary<short, short> dictionary = new Dictionary<short, short>(16);
		IRandomSource random = context.Random;
		int num4 = positiveFeatureRate;
		dictionary.Add(168, -1);
		for (int i = 0; i < num3; i++)
		{
			if (random.CheckPercentProb(num4))
			{
				sbyte medalType = featureMedalType;
				if (featureMedalType != 3 && !context.Random.CheckPercentProb(60))
				{
					medalType = 3;
				}
				var (key, num5) = CharacterDomain.GetRandomBasicFeature(random, isProtagonist, _gender, isPositive: true, medalType, dictionary, 3);
				if (num5 >= 0)
				{
					dictionary.Add(key, num5);
					num4 -= 20;
				}
			}
			else
			{
				var (key2, num6) = CharacterDomain.GetRandomBasicFeature(random, isProtagonist, _gender, isPositive: false, dictionary);
				if (num6 >= 0)
				{
					dictionary.Add(key2, num6);
					num4 += 20;
				}
			}
		}
		int num7 = ((GetAgeGroup() == 2) ? (-1) : _actualAge);
		foreach (KeyValuePair<short, short> item in dictionary)
		{
			short value = item.Value;
			if (value >= 0)
			{
				if (num7 >= 0 && CharacterFeature.Instance[value].Mergeable)
				{
					_potentialFeatureIds.Add(value);
				}
				else
				{
					_featureIds.Add(value);
				}
			}
		}
		if (num7 >= 0)
		{
			int num8 = _potentialFeatureIds.Count * num7 / 16;
			for (int j = 0; j < num8; j++)
			{
				_featureIds.Add(_potentialFeatureIds[j]);
			}
		}
		SetFeatureIds(_featureIds, context);
		SetPotentialFeatureIds(_potentialFeatureIds, context);
	}

	public void RecreateFeaturesByUpgradeCount(DataContext context, int upgradeCount, sbyte featureMedalType)
	{
		Dictionary<short, short> dictionary = ObjectPool<Dictionary<short, short>>.Instance.Get();
		List<short> list = ObjectPool<List<short>>.Instance.Get();
		List<short> list2 = ObjectPool<List<short>>.Instance.Get();
		bool isProtagonist = _id == DomainManager.Taiwu.GetTaiwuCharId();
		for (int i = 0; i < upgradeCount; i++)
		{
			dictionary.Clear();
			list.Clear();
			list2.Clear();
			List<short> list3;
			if (featureMedalType != 3)
			{
				foreach (short featureId in _featureIds)
				{
					CharacterFeatureItem characterFeatureItem = CharacterFeature.Instance[featureId];
					if (!characterFeatureItem.Basic || characterFeatureItem.MutexGroupId == 168)
					{
						continue;
					}
					if (SharedMethods.CalcFeatureMedalValue(new short[1] { featureId }, featureMedalType) > 0)
					{
						if ((characterFeatureItem.IsGood() && !characterFeatureItem.IsHighest()) || characterFeatureItem.IsBad())
						{
							list.Add(featureId);
						}
					}
					else if ((characterFeatureItem.IsGood() && !characterFeatureItem.IsHighest()) || characterFeatureItem.IsBad())
					{
						list2.Add(featureId);
					}
				}
				list3 = ((list.Count == 0) ? list2 : ((list2.Count == 0) ? list : (context.Random.CheckPercentProb(60) ? list : list2)));
			}
			else
			{
				foreach (short featureId2 in _featureIds)
				{
					CharacterFeatureItem characterFeatureItem2 = CharacterFeature.Instance[featureId2];
					if (characterFeatureItem2.Basic && characterFeatureItem2.MutexGroupId != 168 && ((characterFeatureItem2.IsGood() && !characterFeatureItem2.IsHighest()) || characterFeatureItem2.IsBad()))
					{
						list.Add(featureId2);
					}
				}
				list3 = list;
			}
			if (list3.Count == 0)
			{
				sbyte medalType = featureMedalType;
				if (featureMedalType == 3)
				{
					medalType = (sbyte)context.Random.Next(0, 3);
				}
				foreach (short featureId3 in _featureIds)
				{
					CharacterFeatureItem characterFeatureItem3 = CharacterFeature.Instance[featureId3];
					short mutexGroupId = characterFeatureItem3.MutexGroupId;
					dictionary.Add(mutexGroupId, featureId3);
				}
				var (num, num2) = CharacterDomain.GetRandomBasicFeature(context.Random, isProtagonist, _gender, isPositive: true, medalType, dictionary, 1);
				if (num2 >= 0)
				{
					_featureIds.Add(num2);
				}
				continue;
			}
			short randomElement = context.Random.GetRandomElement(list3);
			CharacterFeatureItem config = CharacterFeature.Instance[randomElement];
			int index = _featureIds.IndexOf(randomElement);
			if (config.IsGood())
			{
				CharacterFeatureItem characterFeatureItem4 = config.Upgrade();
				if (characterFeatureItem4 != null)
				{
					_featureIds[index] = characterFeatureItem4.TemplateId;
				}
			}
			else
			{
				CharacterFeatureItem characterFeatureItem4 = config.Degrade();
				if (characterFeatureItem4 != null)
				{
					_featureIds[index] = characterFeatureItem4.TemplateId;
				}
				else
				{
					_featureIds.RemoveAt(index);
				}
			}
		}
		SetFeatureIds(_featureIds, context);
		ObjectPool<Dictionary<short, short>>.Instance.Return(dictionary);
		ObjectPool<List<short>>.Instance.Return(list);
		ObjectPool<List<short>>.Instance.Return(list2);
	}

	public static short GenerateRandomAge(IRandomSource random)
	{
		return (short)RedzenHelper.NormalDistribute(random, 30f, 6.667f, 10, 50);
	}

	public static short GenerateRandomAge(IRandomSource random, short baseAge)
	{
		return (short)Math.Max(16, random.Next(baseAge - 5, baseAge + 6));
	}

	public static bool IsXiangshuMinion(short templateId)
	{
		return templateId >= 298 && templateId <= 306;
	}

	public static bool IsRighteous(short templateId)
	{
		CharacterItem item = Config.Character.Instance.GetItem(templateId);
		return item != null && item.OrganizationInfo.OrgTemplateId == 18;
	}

	public static bool IsReincarnationBonusFeature(short featureId)
	{
		switch (featureId)
		{
		case 257:
		case 258:
		case 259:
		case 260:
		case 261:
		case 394:
		case 395:
		case 396:
		case 397:
		case 398:
			return true;
		default:
			return false;
		}
	}

	public static bool IsPositiveReincarnationBonusFeature(short featureId)
	{
		return featureId >= 257 && featureId <= 261;
	}

	public static bool IsNegativeReincarnationBonusFeature(short featureId)
	{
		return featureId >= 394 && featureId <= 398;
	}

	public static bool IsProfessionReincarnationBonusFeature(short featureId)
	{
		return featureId >= 605 && featureId <= 625;
	}

	public static bool IsProfessionPositiveReincarnationBonusFeature(short featureId)
	{
		switch (featureId)
		{
		case 605:
		case 606:
		case 607:
		case 608:
		case 609:
		case 616:
		case 617:
		case 618:
		case 619:
		case 620:
			return true;
		default:
			return false;
		}
	}

	public static bool IsProfessionNegativeReincarnationBonusFeature(short featureId)
	{
		switch (featureId)
		{
		case 610:
		case 611:
		case 612:
		case 613:
		case 614:
		case 621:
		case 622:
		case 623:
		case 624:
		case 625:
			return true;
		default:
			return false;
		}
	}

	private void OfflineCreateRandomName(DataContext context, ref IntelligentCharacterCreationInfo info, CreateIntelligentCharacterModification mod, sbyte race)
	{
		if (race == 0)
		{
			if (info.ReferenceFullName.Type != 0)
			{
				_fullName = CharacterDomain.GenerateRandomHanName(context.Random, info.ReferenceFullName.GetCustomSurnameId(), info.ReferenceFullName.GetSurnameId(), _gender, -1);
				return;
			}
			FullName mainParentFullName = GetMainParentFullName(ref info, mod.CreateMotherRelation, mod.CreateFatherRelation);
			int customSurnameId = mainParentFullName.GetCustomSurnameId();
			short surnameId = mainParentFullName.GetSurnameId();
			if (_gender == 0)
			{
				_fullName = CharacterDomain.GenerateRandomHanName(context.Random, customSurnameId, surnameId, _gender, -1);
				return;
			}
			int elderBrothersCount = GetElderBrothersCount(ref info, mod);
			_fullName = CharacterDomain.GenerateRandomHanName(context.Random, customSurnameId, surnameId, _gender, elderBrothersCount);
			if (elderBrothersCount < 0)
			{
				return;
			}
			if (elderBrothersCount == 0)
			{
				if (_fullName.GivenNameGroupId == 0)
				{
					mod.Father = info.Father;
					mod.FathersLegitimateBoysCount = 1;
				}
				else
				{
					mod.Father = info.Father;
					mod.FathersLegitimateBoysCount = -1;
				}
			}
			else
			{
				mod.Father = info.Father;
				mod.FathersLegitimateBoysCount = (sbyte)(elderBrothersCount + 1);
			}
		}
		else
		{
			_fullName = CharacterDomain.GenerateRandomZangName(context.Random, _gender);
		}
	}

	private static FullName GetMainParentFullName(ref IntelligentCharacterCreationInfo info, bool createMotherRelation, bool createFatherRelation)
	{
		if (createMotherRelation && !createFatherRelation)
		{
			return info.Mother.GetFullName();
		}
		if (info.Father != null)
		{
			return info.Father.GetFullName();
		}
		if (info.DeadFather != null)
		{
			return info.DeadFather.FullName;
		}
		return default(FullName);
	}

	private static int GetElderBrothersCount(ref IntelligentCharacterCreationInfo info, CreateIntelligentCharacterModification mod)
	{
		if (info.Father == null || info.Mother == null)
		{
			return -1;
		}
		HashSet<int> relatedCharIds = DomainManager.Character.GetRelatedCharIds(info.FatherCharId, 1024);
		if (!relatedCharIds.Contains(info.MotherCharId))
		{
			return -1;
		}
		if (info.MultipleBirthCount > 1)
		{
			mod.Father = info.Father;
			mod.FathersLegitimateBoysCount = -1;
			return -1;
		}
		sbyte legitimateBoysCount = info.Father.GetLegitimateBoysCount();
		if (legitimateBoysCount >= 5)
		{
			mod.Father = info.Father;
			mod.FathersLegitimateBoysCount = -1;
			return -1;
		}
		return legitimateBoysCount;
	}

	private void OfflineCreateAttractionAndAvatar(DataContext context, sbyte bodyType, ref IntelligentCharacterCreationInfo info, bool haveHair)
	{
		if (bodyType < 0)
		{
			bodyType = BodyType.GetRandom(context.Random);
		}
		if (info.BaseAttraction >= 0)
		{
			if (info.Avatar != null)
			{
				_avatar = info.Avatar;
				if (_transgender)
				{
					_avatar.ChangeGender(Gender.Flip(_gender));
				}
			}
			else
			{
				_avatar = AvatarManager.Instance.GetRandomAvatar(context.Random, _gender, _transgender, bodyType, info.BaseAttraction);
			}
			if (haveHair)
			{
				AvatarGroup avatarGroup = AvatarManager.Instance.GetAvatarGroup(_avatar.AvatarId);
				var (frontHairId, backHairId) = avatarGroup.GetRandomHairsNoSkinHead(context.Random);
				_avatar.FrontHairId = frontHairId;
				_avatar.BackHairId = backHairId;
			}
		}
		else if (info.MotherCharId >= 0 || info.ActualFatherCharId >= 0)
		{
			AvatarData mother = info.Mother?.GetAvatar();
			AvatarData father = info.ActualFather?.GetAvatar() ?? info.ActualDeadFather?.Avatar;
			_avatar = AvatarManager.Instance.GetRandomAvatar(context.Random, _gender, _transgender, bodyType, father, mother);
		}
		else
		{
			short baseAttraction = GenerateRandomAttraction(context.Random);
			_avatar = AvatarManager.Instance.GetRandomAvatar(context.Random, _gender, _transgender, bodyType, baseAttraction);
		}
	}

	private void OfflineCreateInitialLifeSkills(IRandomSource random, OrganizationMemberItem orgMemberCfg)
	{
		OfflineCreateInitialLifeSkills(random, orgMemberCfg.LifeSkillsAdjust, orgMemberCfg.LifeSkillGradeLimit);
	}

	private unsafe void OfflineCreateInitialLifeSkills(IRandomSource random, short[] lifeSkillsAdjust, sbyte skillGradeLimit)
	{
		for (sbyte b = 0; b < lifeSkillsAdjust.Length; b++)
		{
			short num = lifeSkillsAdjust[b];
			if (num > 0)
			{
				int num2 = 0;
				short num3 = _baseLifeSkillQualifications.Items[b];
				int num4 = num + 100;
				for (int i = 0; i < skillGradeLimit; i++)
				{
					short skillTemplateId = Config.LifeSkillType.Instance[b].SkillList[i];
					short readingAttainmentRequirement = SkillGradeData.Instance[i].ReadingAttainmentRequirement;
					num2 += 5 * (i + 1);
					int num5 = ((i > 8) ? (num2 * 2) : ((i > 5) ? (num2 / 2) : ((i > 2) ? (num2 / 4) : 0)));
					int num6 = num3 * (100 + num5) / 100;
					if (!random.CheckPercentProb(num6 * num4 / readingAttainmentRequirement))
					{
						break;
					}
					LifeSkillItem item = new LifeSkillItem(skillTemplateId);
					item.SetRandomPagesRead(random, 5);
					_learnedLifeSkills.Add(item);
				}
			}
		}
	}

	private void OfflineCreatePresetLifeSkillsByGradeConfig()
	{
		sbyte[] learnedLifeSkillGrades = Config.Character.Instance[_templateId].LearnedLifeSkillGrades;
		List<LifeSkillItem> list = ((_learnedLifeSkills.Count > 0) ? _learnedLifeSkills : null);
		if (list != null)
		{
			_learnedLifeSkills = new List<LifeSkillItem>();
		}
		for (sbyte b = 0; b < 16; b++)
		{
			sbyte b2 = learnedLifeSkillGrades[b];
			if (b2 >= 0)
			{
				for (sbyte b3 = 0; b3 <= b2; b3++)
				{
					short skillTemplateId = Config.LifeSkillType.Instance[b].SkillList[b3];
					_learnedLifeSkills.Add(new LifeSkillItem(skillTemplateId)
					{
						ReadingState = 31
					});
				}
			}
		}
		if (list == null)
		{
			return;
		}
		if (_learnedLifeSkills.Count != 0)
		{
			foreach (LifeSkillItem item in list)
			{
				if (FindLearnedLifeSkillIndex(item.SkillTemplateId) < 0)
				{
					_learnedLifeSkills.Add(item);
				}
			}
			return;
		}
		_learnedLifeSkills.AddRange(list);
	}

	private static int GetCurrAdjust(IRandomSource random, short[] adjusts, sbyte index, sbyte abnormalProb)
	{
		if (adjusts == null)
		{
			return 0;
		}
		if (abnormalProb > 0 && random.CheckPercentProb(abnormalProb))
		{
			return 0;
		}
		return adjusts[index];
	}

	private unsafe void OfflineCreateResources(IRandomSource random, short orgMemberId, sbyte abnormalOrgAttributesProb)
	{
		short[] adjusts = ((orgMemberId >= 0) ? OrganizationDomain.GetMemberResourcesAdjust(orgMemberId) : null);
		sbyte abnormalProb = (sbyte)(random.CheckPercentProb(abnormalOrgAttributesProb) ? random.Next(101) : 0);
		int gainResourcePercent = DomainManager.World.GetGainResourcePercent(8);
		for (sbyte b = 0; b < 8; b++)
		{
			int num = ((b == 7) ? 100 : 200);
			int normalDistributedRangedValue = RedzenHelper.GetNormalDistributedRangedValue(random, num, num * 2);
			int currAdjust = GetCurrAdjust(random, adjusts, b, abnormalProb);
			normalDistributedRangedValue = normalDistributedRangedValue * (100 + currAdjust) / 100;
			normalDistributedRangedValue = normalDistributedRangedValue * gainResourcePercent / 100;
			_resources.Items[b] = ((normalDistributedRangedValue > 0) ? normalDistributedRangedValue : 0);
		}
	}

	private static SkillQualificationBonus GenerateRandomInnateSkillQualificationBonus(DataContext context)
	{
		IRandomSource random = context.Random;
		int num = random.Next(2);
		int num2 = ((num == 0) ? random.Next(16) : random.Next(14));
		int num3 = RedzenHelper.SkewDistribute(context.Random, 7f, 1.5f, 2f, 3, 15);
		return new SkillQualificationBonus((sbyte)num, (sbyte)num2, (sbyte)num3, -1);
	}

	private static short GenerateRandomMaxHealth(IRandomSource random)
	{
		int characterLifeSpanFactor = DomainManager.World.GetCharacterLifeSpanFactor();
		int num = characterLifeSpanFactor / 2 * 12;
		int num2 = characterLifeSpanFactor * 12;
		float mean = (float)(num + num2) / 2f;
		float stdDev = (float)(num2 - num) / 6f;
		return (short)RedzenHelper.NormalDistribute(random, mean, stdDev, num, num2);
	}

	private static short GenerateProtagonistMaxHealth(IRandomSource random)
	{
		int characterLifeSpanFactor = DomainManager.World.GetCharacterLifeSpanFactor();
		int num = characterLifeSpanFactor * 3 / 4;
		int num2 = num + random.Next(5, 15) * characterLifeSpanFactor / 100;
		return (short)(num2 * 12 + random.Next(12));
	}

	private static short GenerateRandomAttraction(IRandomSource random)
	{
		return (short)RedzenHelper.SkewDistribute(random, 350f, 152.2f, 1.5714285f, 0, 900);
	}

	private static (short lovingItemSubType, short hatingItemSubType) GenerateRandomHobby(IRandomSource random)
	{
		short num = ItemSubType.GetRandom(random);
		short num2 = ItemSubType.GetRandom(random);
		if (!ItemSubType.IsHobbyType(num))
		{
			num = -1;
		}
		if (!ItemSubType.IsHobbyType(num2))
		{
			num2 = -1;
		}
		if (num == num2 && num != -1)
		{
			if (random.CheckPercentProb(50))
			{
				num = -1;
			}
			else
			{
				num2 = -1;
			}
		}
		return (lovingItemSubType: num, hatingItemSubType: num2);
	}

	private void OfflineGenerateRandomIdealSect(IRandomSource random)
	{
		if (Template.IdealSect == 0)
		{
			_idealSect = -1;
			return;
		}
		List<sbyte> randomIdealSects = Template.RandomIdealSects;
		_idealSect = ((randomIdealSects != null && randomIdealSects.Count > 0) ? randomIdealSects.GetRandom(random) : ((sbyte)((Template.IdealSect > 0) ? Template.IdealSect : (-1))));
	}

	private void OfflineCreateProtagonistRandomFeatures(DataContext context, List<short> inscribedFeatureIds, bool addFeatureDreamLover, bool addFeatureLifeSkillLearning, bool addFeatureCombatSkillLearning, bool addFeatureWhiteSnake, bool addFeatureLongevity)
	{
		if (inscribedFeatureIds != null)
		{
			int i = 0;
			for (int count = inscribedFeatureIds.Count; i < count; i++)
			{
				_featureIds.Add(inscribedFeatureIds[i]);
			}
		}
		_featureIds.Add(CharacterDomain.GetBirthdayFeatureId(_birthMonth));
		if (addFeatureDreamLover)
		{
			_featureIds.Add(203);
		}
		if (addFeatureLifeSkillLearning)
		{
			_featureIds.Add(201);
		}
		if (addFeatureCombatSkillLearning)
		{
			_featureIds.Add(202);
		}
		if (addFeatureWhiteSnake)
		{
			_featureIds.Add(256);
		}
		if (addFeatureLongevity)
		{
			_featureIds.Add(335);
		}
		if (inscribedFeatureIds != null)
		{
			_featureIds.Add(195);
			_featureIds.Add(216);
			_featureIds.Sort(CharacterFeatureHelper.FeatureComparer);
		}
		else
		{
			FeatureCreationContext featureCreationContext = new FeatureCreationContext(this);
			featureCreationContext.IsProtagonist = true;
			FeatureCreationContext creationContext = featureCreationContext;
			CharacterCreation.CreateFeatures(context.Random, ref creationContext);
		}
	}

	private void OfflineCreateGenome(IRandomSource randomSource, Character mother, PregnantState pregnantState, Character father)
	{
		if (pregnantState != null)
		{
			if (pregnantState.FatherId >= 0)
			{
				Genome.Inherit(randomSource, ref mother.GetGenome(), ref pregnantState.FatherGenome, ref _genome);
				return;
			}
			Genome genome = default(Genome);
			Genome.CreateRandom(randomSource, ref genome);
			Genome.Inherit(randomSource, ref mother.GetGenome(), ref genome, ref _genome);
		}
		else
		{
			GenomeHelper.Inherit(randomSource, mother, father, ref _genome);
		}
	}

	private List<GameData.Domains.CombatSkill.CombatSkill> OfflineCreateCombatSkills(IRandomSource random, List<PresetCombatSkill> presetCombatSkills)
	{
		int count = presetCombatSkills.Count;
		List<GameData.Domains.CombatSkill.CombatSkill> list = new List<GameData.Domains.CombatSkill.CombatSkill>(count);
		for (int i = 0; i < count; i++)
		{
			PresetCombatSkill presetSkill = presetCombatSkills[i];
			list.Add(new GameData.Domains.CombatSkill.CombatSkill(random, presetSkill));
			_learnedCombatSkills.Add(presetSkill.SkillTemplateId);
		}
		return list;
	}

	private List<GameData.Domains.CombatSkill.CombatSkill> OfflineCreateRandomEnemyCombatSkills(IRandomSource random, RandomEnemyItem randomEnemyCfg, List<PresetCombatSkill> presetCombatSkills)
	{
		int count = presetCombatSkills.Count;
		List<GameData.Domains.CombatSkill.CombatSkill> list = new List<GameData.Domains.CombatSkill.CombatSkill>(count);
		WorldCreationItem enemyPracticeLevelCfg = WorldCreation.Instance[(byte)11];
		sbyte behaviorType = BehaviorType.GetBehaviorType(_baseMorality);
		for (int i = 0; i < count; i++)
		{
			PresetCombatSkill presetCombatSkill = presetCombatSkills[i];
			GameData.Domains.CombatSkill.CombatSkill item = OfflineCreateRandomEnemyCombatSkill(random, presetCombatSkill.SkillTemplateId, behaviorType, randomEnemyCfg, enemyPracticeLevelCfg);
			list.Add(item);
			_learnedCombatSkills.Add(presetCombatSkill.SkillTemplateId);
		}
		return list;
	}

	private void OfflineAddRandomEnemyOrgMemberCombatSkills(DataContext context, RandomEnemyItem randomEnemyCfg, List<(OrganizationItem Org, OrganizationMemberItem Member)> relatedSectAndMembers, List<GameData.Domains.CombatSkill.CombatSkill> charCombatSkills)
	{
		IRandomSource random = context.Random;
		sbyte behaviorType = BehaviorType.GetBehaviorType(_baseMorality);
		List<InventoryCombatSkillBookParams> list = new List<InventoryCombatSkillBookParams>();
		HashSet<short> hashSet = new HashSet<short>();
		WorldCreationItem enemyPracticeLevelCfg = WorldCreation.Instance[(byte)11];
		int i = 0;
		for (int count = relatedSectAndMembers.Count; i < count; i++)
		{
			OrganizationMemberItem item = relatedSectAndMembers[i].Member;
			foreach (PresetOrgMemberCombatSkill combatSkill2 in item.CombatSkills)
			{
				CombatSkillItem combatSkillItem = Config.CombatSkill.Instance[combatSkill2.SkillGroupId];
				IReadOnlyList<CombatSkillItem> learnableCombatSkills = CombatSkillDomain.GetLearnableCombatSkills(combatSkillItem.SectId, combatSkillItem.Type);
				int num = Math.Min(combatSkill2.MaxGrade, learnableCombatSkills.Count - 1);
				for (int j = 0; j <= num; j++)
				{
					if (j == combatSkill2.MaxGrade || random.CheckPercentProb(50))
					{
						CombatSkillItem combatSkillItem2 = learnableCombatSkills[j];
						GameData.Domains.CombatSkill.CombatSkill combatSkill = OfflineCreateRandomEnemyCombatSkill(random, combatSkillItem2.TemplateId, behaviorType, randomEnemyCfg, enemyPracticeLevelCfg);
						byte pageTypes = CombatSkillStateHelper.GeneratePageTypesFromReadingState(random, combatSkill.GetReadingState());
						charCombatSkills.Add(combatSkill);
						_learnedCombatSkills.Add(combatSkillItem2.TemplateId);
						if (random.CheckPercentProb(11 - j * 2))
						{
							list.Add(new InventoryCombatSkillBookParams(combatSkillItem2.BookId, pageTypes));
						}
						short mostFittingWeaponID = combatSkillItem2.MostFittingWeaponID;
						sbyte b = ItemDomain.GenerateRandomItemGrade(random, _organizationInfo.Grade);
						if (mostFittingWeaponID >= 0)
						{
							hashSet.Add((short)(mostFittingWeaponID + b));
						}
					}
				}
			}
		}
		OfflineAddSkillBooksAndWeapons(context, list, hashSet);
	}

	private GameData.Domains.CombatSkill.CombatSkill OfflineCreateRandomEnemyCombatSkill(IRandomSource random, short skillTemplateId, sbyte behaviorType, RandomEnemyItem randomEnemyCfg, WorldCreationItem enemyPracticeLevelCfg)
	{
		byte enemyPracticeLevel = DomainManager.Extra.GetEnemyPracticeLevel();
		short num = enemyPracticeLevelCfg.InfluenceFactors[enemyPracticeLevel];
		int num2 = random.Next(0, num + 1);
		(int, int)[] pageCountRandomRange = randomEnemyCfg.PageCountRandomRange;
		sbyte directPagesReadCount = (sbyte)Math.Clamp(random.Next(pageCountRandomRange[0].Item1, pageCountRandomRange[0].Item2 + 1) + num2, 0, 5);
		sbyte reversePagesReadCount = (sbyte)Math.Clamp(random.Next(pageCountRandomRange[1].Item1, pageCountRandomRange[1].Item2 + 1) + num - num2, 0, 5);
		return new GameData.Domains.CombatSkill.CombatSkill(random, _id, skillTemplateId, behaviorType, directPagesReadCount, reversePagesReadCount);
	}

	private void OfflineCreateCombatSkills(IRandomSource random, List<PresetCombatSkill> presetCombatSkills, CreateIntelligentCharacterModification mod)
	{
		List<GameData.Domains.CombatSkill.CombatSkill> combatSkills = OfflineCreateCombatSkills(random, presetCombatSkills);
		mod.CombatSkills = combatSkills;
	}

	private void OfflineAddPresetOrgMemberCombatSkills(DataContext context, CreateIntelligentCharacterModification mod, OrganizationItem orgConfig, OrganizationMemberItem orgMemberConfig, int ageInfluence)
	{
		sbyte behaviorType = BehaviorType.GetBehaviorType(_baseMorality);
		sbyte teacherBehaviorType = (sbyte)((orgConfig.MainMorality == short.MinValue) ? (-1) : BehaviorType.GetBehaviorType(orgConfig.MainMorality));
		List<InventoryCombatSkillBookParams> inventorySkillBooks = new List<InventoryCombatSkillBookParams>();
		HashSet<short> hashSet = new HashSet<short>();
		OfflineAddPresetOrgMemberCombatSkillsInternal(context.Random, orgConfig.Goodness, teacherBehaviorType, behaviorType, ageInfluence, orgMemberConfig.CombatSkills, mod.CombatSkills, inventorySkillBooks, hashSet);
		mod.InventorySkillBooks = inventorySkillBooks;
		mod.SkillWeaponIds = hashSet;
	}

	private void OfflineAddSkillBooksAndWeapons(DataContext context, List<InventoryCombatSkillBookParams> skillBooks, HashSet<short> skillWeaponIds)
	{
		int i = 0;
		for (int count = skillBooks.Count; i < count; i++)
		{
			InventoryCombatSkillBookParams inventoryCombatSkillBookParams = skillBooks[i];
			ItemKey itemKey = DomainManager.Item.CreateSkillBook(context, inventoryCombatSkillBookParams.TemplateId, inventoryCombatSkillBookParams.PageTypes, -1, -1);
			_inventory.OfflineAdd(itemKey, 1);
		}
		foreach (short skillWeaponId in skillWeaponIds)
		{
			ItemKey itemKey2 = DomainManager.Item.CreateWeapon(context, skillWeaponId, 1);
			_inventory.OfflineAdd(itemKey2, 1);
		}
	}

	private void OfflineAddPresetOrgMemberCombatSkillsInternal(IRandomSource random, sbyte orgGoodness, sbyte teacherBehaviorType, sbyte selfBehaviorType, int ageInfluence, List<PresetOrgMemberCombatSkill> presetSkills, List<GameData.Domains.CombatSkill.CombatSkill> charCombatSkills, List<InventoryCombatSkillBookParams> inventorySkillBooks, HashSet<short> suitableWeaponIds)
	{
		sbyte itemGrade = ((_location.AreaId == 138 && _organizationInfo.OrgTemplateId == 38) ? ((sbyte)(_organizationInfo.Grade / 2)) : _organizationInfo.Grade);
		int i = 0;
		for (int count = presetSkills.Count; i < count; i++)
		{
			PresetOrgMemberCombatSkill presetOrgMemberCombatSkill = presetSkills[i];
			CombatSkillItem combatSkillItem = Config.CombatSkill.Instance[presetOrgMemberCombatSkill.SkillGroupId];
			IReadOnlyList<CombatSkillItem> learnableCombatSkills = CombatSkillDomain.GetLearnableCombatSkills(combatSkillItem.SectId, combatSkillItem.Type);
			int num = Math.Clamp(presetOrgMemberCombatSkill.MaxGrade * ageInfluence / 100, 0, learnableCombatSkills.Count - 1);
			for (int j = 0; j <= num; j++)
			{
				CombatSkillItem combatSkillItem2 = learnableCombatSkills[j];
				int gradeDiff = _organizationInfo.Grade - j;
				var (pageTypes, readingState) = GenerateCombatSkillBookReadingInfo(random, teacherBehaviorType, selfBehaviorType, orgGoodness, gradeDiff, ageInfluence, 0);
				charCombatSkills.Add(new GameData.Domains.CombatSkill.CombatSkill(-1, combatSkillItem2.TemplateId, readingState));
				_learnedCombatSkills.Add(combatSkillItem2.TemplateId);
				if (random.CheckPercentProb(11 - j * 2))
				{
					inventorySkillBooks.Add(new InventoryCombatSkillBookParams(combatSkillItem2.BookId, pageTypes));
				}
				short mostFittingWeaponID = combatSkillItem2.MostFittingWeaponID;
				sbyte b = ItemDomain.GenerateRandomItemGrade(random, itemGrade);
				if (mostFittingWeaponID >= 0)
				{
					suitableWeaponIds.Add((short)(mostFittingWeaponID + b));
				}
			}
		}
	}

	private static (byte bookPageTypes, ushort readingState) GenerateCombatSkillBookReadingInfo(IRandomSource random, sbyte teacherBehaviorType, sbyte selfBehaviorType, sbyte orgGoodness, int gradeDiff, int ageInfluence = 100, short pageBuff = 0)
	{
		sbyte outlinePageType = ((teacherBehaviorType == -1) ? selfBehaviorType : (random.CheckPercentProb(70) ? teacherBehaviorType : selfBehaviorType));
		if (1 == 0)
		{
		}
		sbyte b = orgGoodness switch
		{
			-1 => 30, 
			0 => 50, 
			1 => 70, 
			_ => throw new Exception($"Unsupported goodness {orgGoodness}"), 
		};
		if (1 == 0)
		{
		}
		sbyte normalPagesDirectProb = b;
		int num = 3 + gradeDiff + random.Next(4) + pageBuff;
		num = Math.Clamp(num * ageInfluence / 100, 0, 6);
		byte b2 = GameData.Domains.Item.SkillBook.GenerateCombatPageTypes(random, outlinePageType, normalPagesDirectProb);
		ushort item = GameData.Domains.CombatSkill.CombatSkill.GenerateRandomReadingState(random, b2, num);
		return (bookPageTypes: b2, readingState: item);
	}

	private void OfflineCreateEquipmentAndInventoryItems(DataContext context, CharacterItem template, sbyte ageGroup)
	{
		OfflineCreateEquipmentAndInventoryItemsInternal(template, ageGroup, out var equipment, out var inventory);
		OfflineAddEquipmentAndInventoryItems(context, equipment, inventory);
	}

	private void OfflineAddEquipmentAndInventoryItems(DataContext context, PresetEquipmentItem[] equipment, List<PresetInventoryItem> inventory)
	{
		for (int i = 0; i < 12; i++)
		{
			PresetEquipmentItem presetEquipmentItem = equipment[i];
			_equipment[i] = ((presetEquipmentItem.TemplateId < 0) ? ItemKey.Invalid : ((i != 4) ? DomainManager.Item.CreateItem(context, presetEquipmentItem.Type, presetEquipmentItem.TemplateId) : DomainManager.Item.CreateClothing(context, presetEquipmentItem.TemplateId, _gender)));
			if (_equipment[i].IsValid() && ItemTemplateHelper.HasCarrierTame(presetEquipmentItem.Type, presetEquipmentItem.TemplateId))
			{
				ExtraDomain extra = DomainManager.Extra;
				int carrierMaxTamePoint = extra.GetCarrierMaxTamePoint(_equipment[i].Id);
				extra.SetCarrierTamePoint(context, _equipment[i].Id, carrierMaxTamePoint);
			}
		}
		int j = 0;
		for (int count = inventory.Count; j < count; j++)
		{
			PresetInventoryItem presetInventoryItem = inventory[j];
			if (presetInventoryItem.SpawnChance >= 100 || context.Random.CheckPercentProb(presetInventoryItem.SpawnChance))
			{
				OfflineCreateInventoryOnCharacterCreation(context, presetInventoryItem.Type, presetInventoryItem.TemplateId, presetInventoryItem.Amount);
			}
		}
	}

	private void OfflineCreateEquipmentAndInventoryItems(CreateIntelligentCharacterModification mod, CharacterItem template, sbyte ageGroup)
	{
		OfflineCreateEquipmentAndInventoryItemsInternal(template, ageGroup, out var equipment, out var inventory);
		mod.Equipment = equipment;
		mod.Inventory = inventory;
	}

	private static void OfflineCreateEquipmentAndInventoryItemsInternal(CharacterItem template, sbyte ageGroup, out PresetEquipmentItem[] equipment, out List<PresetInventoryItem> inventory)
	{
		equipment = new PresetEquipmentItem[12];
		for (int i = 0; i < 12; i++)
		{
			equipment[i] = template.PresetEquipment[i];
		}
		if (ageGroup != 2)
		{
			equipment[4] = new PresetEquipmentItem(3, (short)((ageGroup == 1) ? 65 : 64));
		}
		inventory = new List<PresetInventoryItem>(template.PresetInventory);
	}

	private void OfflineAddPresetOrgMemberEquipmentAndInventoryItems(DataContext context, CreateIntelligentCharacterModification mod, OrganizationMemberItem orgMemberConfig, sbyte ageGroup)
	{
		if (ageGroup == 2)
		{
			short randomOrgMemberClothing = OrganizationDomain.GetRandomOrgMemberClothing(context.Random, orgMemberConfig);
			if (mod.Self._location.AreaId == 138 && mod.Self._organizationInfo.OrgTemplateId == 38)
			{
				randomOrgMemberClothing = OrganizationDomain.GetRandomOrgMemberClothing(context.Random, OrganizationDomain.GetOrgMemberConfig(mod.Self._organizationInfo));
			}
			mod.Equipment[4] = new PresetEquipmentItem(3, randomOrgMemberClothing);
		}
		OfflineAddPresetOrgMemberEquipmentsWithProb(context, orgMemberConfig.Equipment, orgMemberConfig.Grade, mod.Inventory);
		OfflineAddPresetOrgMemberInventoryItems(context, orgMemberConfig.Inventory, orgMemberConfig.Grade, mod.Inventory);
	}

	private void OfflineAddRandomEnemyPresetOrgMemberEquipmentAndInventoryItems(DataContext context, RandomEnemyItem randomEnemyCfg, List<(OrganizationItem Sect, OrganizationMemberItem Member)> relatedSectAndMembers)
	{
		List<PresetInventoryItem> list = new List<PresetInventoryItem>();
		int i = 0;
		for (int count = relatedSectAndMembers.Count; i < count; i++)
		{
			OrganizationMemberItem item = relatedSectAndMembers[i].Member;
			OfflineAddPresetOrgMemberEquipmentsWithProb(context, item.Equipment, randomEnemyCfg.ItemGrade, list);
			OfflineAddRandomEnemyPresetOrgMemberInventoryItems(context, item.Inventory, randomEnemyCfg.ItemGrade, list);
		}
		bool loadedAllArchiveData = DomainManager.Global.GetLoadedAllArchiveData();
		int j = 0;
		for (int count2 = list.Count; j < count2; j++)
		{
			PresetInventoryItem presetInventoryItem = list[j];
			if (loadedAllArchiveData)
			{
				for (int k = 0; k < presetInventoryItem.Amount; k++)
				{
					OfflineCreateRandomEnemyInventoryItem(context, randomEnemyCfg, presetInventoryItem.Type, presetInventoryItem.TemplateId);
				}
			}
			else
			{
				OfflineCreateInventoryOnCharacterCreation(context, presetInventoryItem.Type, presetInventoryItem.TemplateId, presetInventoryItem.Amount);
			}
		}
	}

	private void OfflineCreateRandomEnemyInventoryItem(DataContext context, RandomEnemyItem randomEnemyCfg, sbyte itemType, short itemTemplateId)
	{
		ItemKey itemKey = DomainManager.Item.CreateItem(context, itemType, itemTemplateId);
		if (randomEnemyCfg.PoisonsToAdd != null && ItemTemplateHelper.IsPoisonable(itemKey.ItemType, itemKey.TemplateId) && context.Random.CheckPercentProb(randomEnemyCfg.AddPoisonRate))
		{
			Span<short> span = stackalloc short[6];
			SpanList<short> spanList = span;
			spanList.Clear();
			spanList.AddRange(randomEnemyCfg.PoisonsToAdd);
			spanList.Shuffle(context.Random);
			int num = context.Random.Next((int)randomEnemyCfg.MaxAddPoisonCount) + 1;
			if (num > spanList.Count)
			{
				num = spanList.Count;
			}
			if (num > 3)
			{
				num = 3;
			}
			ItemBase itemBase = DomainManager.Item.GetBaseItem(itemKey);
			for (int i = 0; i < num; i++)
			{
				short medicineTemplateId = spanList[i];
				(ItemBase item, bool keyChanged) tuple = DomainManager.Item.SetAttachedPoisons(context, itemBase, medicineTemplateId, add: true);
				ItemBase item = tuple.item;
				bool item2 = tuple.keyChanged;
				itemBase = item;
			}
			itemKey = itemBase.GetItemKey();
		}
		_inventory.OfflineAdd(itemKey, 1);
	}

	private void OfflineAddPresetOrgMemberEquipmentsWithProb(DataContext context, PresetEquipmentItemWithProb[] items, sbyte itemGrade, List<PresetInventoryItem> itemsToBeCreated)
	{
		int i = 0;
		for (int num = items.Length; i < num; i++)
		{
			PresetEquipmentItemWithProb presetEquipmentItemWithProb = items[i];
			if (presetEquipmentItemWithProb.TemplateId >= 0 && context.Random.CheckPercentProb(presetEquipmentItemWithProb.Prob))
			{
				sbyte b = ItemDomain.GenerateRandomItemGrade(context.Random, itemGrade);
				short templateId = (short)(presetEquipmentItemWithProb.TemplateId + b);
				itemsToBeCreated.Add(new PresetInventoryItem(presetEquipmentItemWithProb.Type, templateId, 1, 100));
			}
		}
	}

	private void OfflineAddPresetOrgMemberInventoryItems(DataContext context, List<PresetInventoryItem> items, sbyte itemGrade, List<PresetInventoryItem> itemsToBeCreated)
	{
		IRandomSource random = context.Random;
		int i = 0;
		for (int count = items.Count; i < count; i++)
		{
			PresetInventoryItem presetInventoryItem = items[i];
			if (random.CheckPercentProb(presetInventoryItem.SpawnChance))
			{
				short templateId = ItemDomain.GenerateRandomItemTemplateId(random, presetInventoryItem.Type, presetInventoryItem.TemplateId, itemGrade);
				int amount = 1 + context.Random.Next(presetInventoryItem.Amount);
				itemsToBeCreated.Add(new PresetInventoryItem(presetInventoryItem.Type, templateId, amount, 100));
			}
		}
	}

	private void OfflineAddRandomEnemyPresetOrgMemberInventoryItems(DataContext context, List<PresetInventoryItem> items, sbyte itemGrade, List<PresetInventoryItem> itemsToBeCreated)
	{
		int i = 0;
		for (int count = items.Count; i < count; i++)
		{
			PresetInventoryItem presetInventoryItem = items[i];
			if (context.Random.CheckPercentProb(presetInventoryItem.SpawnChance))
			{
				short templateId = ItemDomain.GenerateRandomItemTemplateId(context.Random, presetInventoryItem.Type, presetInventoryItem.TemplateId, itemGrade);
				int amount = 1 + context.Random.Next(presetInventoryItem.Amount);
				itemsToBeCreated.Add(new PresetInventoryItem(presetInventoryItem.Type, templateId, amount, 100));
			}
		}
	}

	private void OfflineReplaceProtagonistClothing(DataContext context, short clothingTemplateId)
	{
		ItemKey itemKey = _equipment[4];
		if (itemKey.IsValid())
		{
			DomainManager.Item.RemoveItem(context, itemKey);
		}
		ItemKey itemKey2 = DomainManager.Item.CreateClothing(context, clothingTemplateId, _gender);
		_equipment[4] = itemKey2;
	}

	private static List<(OrganizationItem Sect, OrganizationMemberItem Member)> GetRelatedSectAndMembers(IRandomSource random, RandomEnemyItem randomEnemy, sbyte grade)
	{
		int count = randomEnemy.SectIds.Count;
		Span<short> span = stackalloc short[15];
		SpanList<short> spanList = span;
		spanList.AddRange(randomEnemy.SectIds);
		if (count > randomEnemy.SelectSectCount)
		{
			spanList.Shuffle(random);
			spanList.RemoveRange(randomEnemy.SelectSectCount, count - randomEnemy.SelectSectCount);
		}
		List<(OrganizationItem, OrganizationMemberItem)> list = new List<(OrganizationItem, OrganizationMemberItem)>();
		int i = 0;
		for (int count2 = spanList.Count; i < count2; i++)
		{
			short index = spanList[i];
			OrganizationItem organizationItem = Config.Organization.Instance[index];
			short index2 = organizationItem.Members[grade];
			OrganizationMemberItem item = OrganizationMember.Instance[index2];
			list.Add((organizationItem, item));
		}
		return list;
	}

	private short ChooseGrowingSectMember(IRandomSource random, sbyte orgTemplateId, sbyte growingSectGrade)
	{
		if (orgTemplateId < 0)
		{
			orgTemplateId = _organizationInfo.OrgTemplateId;
		}
		sbyte b = -1;
		if (OrganizationDomain.IsSect(orgTemplateId))
		{
			b = orgTemplateId;
		}
		else
		{
			sbyte idealSect = GetIdealSect();
			if (idealSect >= 0 && OrganizationDomain.MeetGenderRestriction(idealSect, _gender))
			{
				b = idealSect;
			}
		}
		if (b < 0)
		{
			b = OrganizationDomain.GetRandomSectOrgTemplateId(random, _gender);
		}
		if (growingSectGrade < 0)
		{
			growingSectGrade = _organizationInfo.Grade;
		}
		return OrganizationDomain.GetMemberId(b, growingSectGrade);
	}

	private unsafe void OfflineInitializeBaseNeiliProportionOfFiveElements()
	{
		for (int i = 0; i < 5; i++)
		{
			_baseNeiliProportionOfFiveElements.Items[i] = 15;
		}
		sbyte innateFiveElementsType = GetInnateFiveElementsType();
		ref sbyte reference = ref _baseNeiliProportionOfFiveElements.Items[innateFiveElementsType];
		reference += 25;
	}

	private unsafe void OfflineGenerateRandomActionEnergies(IRandomSource random)
	{
		for (sbyte b = 0; b < 5; b++)
		{
			_actionEnergies.Items[b] = (byte)random.Next(0, 201);
		}
	}

	private static int CalcAgeInfluence(IRandomSource random, Character character, OrganizationMemberItem orgMemberConfig)
	{
		short initialAge = OrganizationDomain.GetInitialAge(orgMemberConfig);
		return (character._actualAge < initialAge) ? Math.Clamp(character._actualAge * 100 / initialAge + RedzenHelper.NormalDistribute(random, 0f, 10f), 25, 100) : 100;
	}

	[Obsolete]
	private static int GenerateRandomAttributeValue(IRandomSource random, int adjustPercent)
	{
		float mean = 40f + 25f * ((float)adjustPercent / 100f);
		return RedzenHelper.NormalDistribute(random, mean, 10.746457f, 0, 100);
	}

	[Obsolete]
	private unsafe void OfflineGenerateRandomMainAttributes(IRandomSource random, short[] mainAttributesAdjust, bool abnormalOrgAttributes)
	{
		sbyte abnormalProb = (sbyte)(abnormalOrgAttributes ? random.Next(101) : 0);
		for (sbyte b = 0; b < 6; b++)
		{
			int currAdjust = GetCurrAdjust(random, mainAttributesAdjust, b, abnormalProb);
			int num = GenerateRandomAttributeValue(random, currAdjust);
			_baseMainAttributes.Items[b] = (short)num;
		}
	}

	[Obsolete]
	public void RecreateMainAttributesAndQualificationsByGrowingSect(DataContext context, sbyte growingOrgTemplateId, sbyte growingGrade)
	{
		OfflineRecreateByGrowingSect(context.Random, growingOrgTemplateId, growingGrade);
	}

	[Obsolete]
	private void OfflineCreateMainAttributes(IRandomSource random, ref IntelligentCharacterCreationInfo info, short orgMemberId, sbyte abnormalOrgAttributesProb)
	{
		if (info.MotherCharId >= 0 || info.ActualFatherCharId >= 0)
		{
			OfflineInheritMainAttributes(random, ref info);
		}
		else if (orgMemberId >= 0)
		{
			OfflineGenerateRandomMainAttributes(random, OrganizationDomain.GetMemberMainAttributesAdjust(orgMemberId), random.CheckPercentProb(abnormalOrgAttributesProb));
		}
		else
		{
			OfflineGenerateRandomMainAttributes(random, null, abnormalOrgAttributes: false);
		}
	}

	[Obsolete]
	private unsafe void OfflineInheritMainAttributes(IRandomSource random, ref IntelligentCharacterCreationInfo info)
	{
		if (info.MotherCharId >= 0 && info.ActualFatherCharId >= 0)
		{
			int* ptr = stackalloc int[6];
			MainAttributes baseMainAttributes = info.Mother.GetBaseMainAttributes();
			MainAttributes mainAttributes = info.ActualFather?.GetBaseMainAttributes() ?? info.ActualDeadFather.BaseMainAttributes;
			for (sbyte b = 0; b < 6; b++)
			{
				*(short*)(ptr + b) = baseMainAttributes.Items[b];
				((short*)(ptr + b))[1] = mainAttributes.Items[b];
			}
			InheritAttributes_SortAttributePairs(ptr, 6);
			fixed (short* items = _baseMainAttributes.Items)
			{
				InheritAttributes_CalcAttributes(random, ptr, items, 6);
			}
			return;
		}
		MainAttributes mainAttributes2 = info.Mother?.GetBaseMainAttributes() ?? info.ActualFather?.GetBaseMainAttributes() ?? info.ActualDeadFather.BaseMainAttributes;
		for (sbyte b2 = 0; b2 < 6; b2++)
		{
			int num = mainAttributes2.Items[b2];
			num += random.Next(21) - 10;
			if (num < 0)
			{
				num = 0;
			}
			_baseMainAttributes.Items[b2] = (short)num;
		}
	}

	[Obsolete]
	private unsafe static void InheritAttributes_SortAttributePairs(int* pAttrPairs, int count)
	{
		short* ptr = (short*)pAttrPairs;
		short* ptr2 = (short*)pAttrPairs + 1;
		for (int i = 0; i < count; i++)
		{
			if (ptr < ptr2)
			{
				short num = *ptr;
				*ptr = *ptr2;
				*ptr2 = num;
			}
			ptr += 2;
			ptr2 += 2;
		}
	}

	[Obsolete]
	private unsafe static void InheritAttributes_CalcAttributes(IRandomSource random, int* pAttrPairs, short* pResultAttributes, int count)
	{
		int num = random.Next(100);
		short* ptr = (short*)pAttrPairs;
		short* ptr2 = (short*)pAttrPairs + 1;
		for (int i = 0; i < count; i++)
		{
			int num2 = (random.CheckPercentProb(50) ? num : random.Next(100));
			int num3 = ((num2 < 20) ? (*ptr + *ptr2 / 10) : ((num2 >= 50) ? ((*ptr + *ptr2) / 2) : (*ptr)));
			num3 += random.Next(21) - 10;
			if (num3 < 0)
			{
				num3 = 0;
			}
			pResultAttributes[i] = (short)num3;
			ptr += 2;
			ptr2 += 2;
		}
	}

	[Obsolete]
	private void OfflineCreateRandomEnemyMainAttributes(IRandomSource random, List<(OrganizationItem Sect, OrganizationMemberItem Member)> relatedSectsAndMembers, sbyte abnormalOrgAttributesProb)
	{
		short num = (short)relatedSectsAndMembers.Count;
		short[] array;
		if (num > 1)
		{
			array = new short[6];
			for (int i = 0; i < num; i++)
			{
				short[] mainAttributesAdjust = relatedSectsAndMembers[i].Member.MainAttributesAdjust;
				for (sbyte b = 0; b < 6; b++)
				{
					array[b] += mainAttributesAdjust[b];
				}
			}
			for (sbyte b2 = 0; b2 < 6; b2++)
			{
				array[b2] /= num;
			}
		}
		else
		{
			array = relatedSectsAndMembers[0].Member.MainAttributesAdjust;
		}
		OfflineGenerateRandomMainAttributes(random, array, random.CheckPercentProb(abnormalOrgAttributesProb));
	}

	[Obsolete]
	private unsafe void OfflineCreateProtagonistMainAttributes(IRandomSource random, short orgMemberId)
	{
		short[] memberMainAttributesAdjust = OrganizationDomain.GetMemberMainAttributesAdjust(orgMemberId);
		for (sbyte b = 0; b < 6; b++)
		{
			int num = memberMainAttributesAdjust[b];
			int num2 = ((num > 0) ? 50 : ((num == 0) ? 35 : 20));
			int num3 = num2 + random.Next(16);
			_baseMainAttributes.Items[b] = (short)num3;
		}
	}

	[Obsolete]
	private void OfflineCreateLifeSkillsQualifications(IRandomSource random, short orgMemberId, sbyte abnormalOrgAttributesProb, short[] lifeSkillsAdjustBonus)
	{
		if (orgMemberId >= 0)
		{
			OfflineGenerateRandomLifeSkillsQualifications(random, OrganizationDomain.GetMemberLifeSkillsAdjust(orgMemberId), lifeSkillsAdjustBonus, random.CheckPercentProb(abnormalOrgAttributesProb));
		}
		else
		{
			OfflineGenerateRandomLifeSkillsQualifications(random, null, lifeSkillsAdjustBonus, abnormalOrgAttributes: false);
		}
	}

	[Obsolete]
	private unsafe void OfflineGenerateRandomLifeSkillsQualifications(IRandomSource random, short[] lifeSkillsAdjust, short[] lifeSkillsAdjustBonus, bool abnormalOrgAttributes)
	{
		sbyte abnormalProb = (sbyte)(abnormalOrgAttributes ? random.Next(101) : 0);
		for (sbyte b = 0; b < 16; b++)
		{
			int num = GetCurrAdjust(random, lifeSkillsAdjust, b, abnormalProb);
			if (lifeSkillsAdjustBonus != null)
			{
				num += lifeSkillsAdjustBonus[b];
			}
			int num2 = GenerateRandomAttributeValue(random, num);
			_baseLifeSkillQualifications.Items[b] = (short)num2;
		}
	}

	[Obsolete]
	private void OfflineCreateLifeSkillsQualifications(IRandomSource random, ref IntelligentCharacterCreationInfo info, short orgMemberId, sbyte abnormalOrgAttributesProb)
	{
		if (info.MotherCharId >= 0 || info.ActualFatherCharId >= 0)
		{
			OfflineInheritLifeSkillsQualifications(random, ref info);
		}
		else
		{
			OfflineCreateLifeSkillsQualifications(random, orgMemberId, abnormalOrgAttributesProb, info.LifeSkillsLowerBound);
		}
	}

	[Obsolete]
	private unsafe void OfflineInheritLifeSkillsQualifications(IRandomSource random, ref IntelligentCharacterCreationInfo info)
	{
		if (info.MotherCharId >= 0 && info.ActualFatherCharId >= 0)
		{
			int* ptr = stackalloc int[16];
			ref LifeSkillShorts baseLifeSkillQualifications = ref info.Mother.GetBaseLifeSkillQualifications();
			Character actualFather = info.ActualFather;
			LifeSkillShorts lifeSkillShorts = ((actualFather != null) ? actualFather.GetBaseLifeSkillQualifications() : info.ActualDeadFather.BaseLifeSkillQualifications);
			for (sbyte b = 0; b < 16; b++)
			{
				*(short*)(ptr + b) = baseLifeSkillQualifications.Items[b];
				((short*)(ptr + b))[1] = lifeSkillShorts.Items[b];
			}
			InheritAttributes_SortAttributePairs(ptr, 16);
			fixed (short* items = _baseLifeSkillQualifications.Items)
			{
				InheritAttributes_CalcAttributes(random, ptr, items, 16);
			}
			return;
		}
		Character mother = info.Mother;
		LifeSkillShorts obj;
		if (mother == null)
		{
			Character actualFather2 = info.ActualFather;
			obj = ((actualFather2 != null) ? actualFather2.GetBaseLifeSkillQualifications() : info.ActualDeadFather.BaseLifeSkillQualifications);
		}
		else
		{
			obj = mother.GetBaseLifeSkillQualifications();
		}
		LifeSkillShorts lifeSkillShorts2 = obj;
		for (sbyte b2 = 0; b2 < 16; b2++)
		{
			int num = lifeSkillShorts2.Items[b2];
			num += random.Next(21) - 10;
			if (num < 0)
			{
				num = 0;
			}
			_baseLifeSkillQualifications.Items[b2] = (short)num;
		}
	}

	[Obsolete]
	private unsafe void OfflineCreateRandomEnemyLifeSkillsQualifications(IRandomSource random, RandomEnemyItem randomEnemy, List<(OrganizationItem Sect, OrganizationMemberItem Member)> relatedSectsAndMembers, sbyte abnormalOrgAttributesProb)
	{
		short num = (short)relatedSectsAndMembers.Count;
		short[] array;
		if (num > 1)
		{
			array = new short[16];
			for (int i = 0; i < num; i++)
			{
				short[] lifeSkillsAdjust = relatedSectsAndMembers[i].Member.LifeSkillsAdjust;
				for (sbyte b = 0; b < 16; b++)
				{
					array[b] += lifeSkillsAdjust[b];
				}
			}
			for (sbyte b2 = 0; b2 < 16; b2++)
			{
				array[b2] /= num;
			}
		}
		else
		{
			array = relatedSectsAndMembers[0].Member.LifeSkillsAdjust;
		}
		OfflineGenerateRandomLifeSkillsQualifications(random, array, null, random.CheckPercentProb(abnormalOrgAttributesProb));
		for (sbyte b3 = 0; b3 < 16; b3++)
		{
			int num2 = _baseLifeSkillQualifications.Items[b3];
			num2 = num2 * (100 + randomEnemy.LifeSkillQualificationAdjust) / 100;
			if (num2 < 0)
			{
				num2 = 0;
			}
			_baseLifeSkillQualifications.Items[b3] = (short)num2;
		}
	}

	[Obsolete]
	private unsafe void OfflineGenerateRandomCombatSkillsQualifications(IRandomSource random, short[] combatSkillsAdjust, short[] combatSkillsAdjustBonus, bool abnormalOrgAttributes)
	{
		sbyte abnormalProb = (sbyte)(abnormalOrgAttributes ? random.Next(101) : 0);
		for (sbyte b = 0; b < 14; b++)
		{
			int num = GetCurrAdjust(random, combatSkillsAdjust, b, abnormalProb);
			if (combatSkillsAdjustBonus != null)
			{
				num += combatSkillsAdjustBonus[b];
			}
			int num2 = GenerateRandomAttributeValue(random, num);
			_baseCombatSkillQualifications.Items[b] = (short)num2;
		}
	}

	[Obsolete]
	private void OfflineCreateCombatSkillsQualifications(IRandomSource random, short orgMemberId, sbyte abnormalOrgAttributesProb, short[] combatSkillsAdjustBonus)
	{
		if (orgMemberId >= 0)
		{
			OfflineGenerateRandomCombatSkillsQualifications(random, OrganizationDomain.GetMemberCombatSkillsAdjust(orgMemberId), combatSkillsAdjustBonus, random.CheckPercentProb(abnormalOrgAttributesProb));
		}
		else
		{
			OfflineGenerateRandomCombatSkillsQualifications(random, null, combatSkillsAdjustBonus, abnormalOrgAttributes: false);
		}
	}

	[Obsolete]
	private void OfflineCreateCombatSkillsQualifications(IRandomSource random, ref IntelligentCharacterCreationInfo info, short orgMemberId, sbyte abnormalOrgAttributesProb)
	{
		if (info.MotherCharId >= 0 || info.ActualFatherCharId >= 0)
		{
			OfflineInheritCombatSkillsQualifications(random, ref info);
		}
		else
		{
			OfflineCreateCombatSkillsQualifications(random, orgMemberId, abnormalOrgAttributesProb, info.CombatSkillsLowerBound);
		}
	}

	[Obsolete]
	private unsafe void OfflineInheritCombatSkillsQualifications(IRandomSource random, ref IntelligentCharacterCreationInfo info)
	{
		if (info.MotherCharId >= 0 && info.ActualFatherCharId >= 0)
		{
			int* ptr = stackalloc int[14];
			ref CombatSkillShorts baseCombatSkillQualifications = ref info.Mother.GetBaseCombatSkillQualifications();
			Character actualFather = info.ActualFather;
			CombatSkillShorts combatSkillShorts = ((actualFather != null) ? actualFather.GetBaseCombatSkillQualifications() : info.ActualDeadFather.BaseCombatSkillQualifications);
			for (sbyte b = 0; b < 14; b++)
			{
				*(short*)(ptr + b) = baseCombatSkillQualifications.Items[b];
				((short*)(ptr + b))[1] = combatSkillShorts.Items[b];
			}
			InheritAttributes_SortAttributePairs(ptr, 14);
			fixed (short* items = _baseCombatSkillQualifications.Items)
			{
				InheritAttributes_CalcAttributes(random, ptr, items, 14);
			}
			return;
		}
		Character mother = info.Mother;
		CombatSkillShorts obj;
		if (mother == null)
		{
			Character actualFather2 = info.ActualFather;
			obj = ((actualFather2 != null) ? actualFather2.GetBaseCombatSkillQualifications() : info.ActualDeadFather.BaseCombatSkillQualifications);
		}
		else
		{
			obj = mother.GetBaseCombatSkillQualifications();
		}
		CombatSkillShorts combatSkillShorts2 = obj;
		for (sbyte b2 = 0; b2 < 14; b2++)
		{
			int num = combatSkillShorts2.Items[b2];
			num += random.Next(21) - 10;
			if (num < 0)
			{
				num = 0;
			}
			_baseCombatSkillQualifications.Items[b2] = (short)num;
		}
	}

	[Obsolete]
	private unsafe void OfflineCreateRandomEnemyCombatSkillsQualifications(IRandomSource random, RandomEnemyItem randomEnemy, List<(OrganizationItem Sect, OrganizationMemberItem Member)> relatedSectsAndMembers, sbyte abnormalOrgAttributesProb)
	{
		short num = (short)relatedSectsAndMembers.Count;
		short[] array;
		if (num > 1)
		{
			array = new short[14];
			for (int i = 0; i < num; i++)
			{
				short[] combatSkillsAdjust = relatedSectsAndMembers[i].Member.CombatSkillsAdjust;
				for (int j = 0; j < 14; j++)
				{
					array[j] += combatSkillsAdjust[j];
				}
			}
			for (int k = 0; k < 14; k++)
			{
				array[k] /= num;
			}
		}
		else
		{
			array = relatedSectsAndMembers[0].Member.CombatSkillsAdjust;
		}
		OfflineGenerateRandomCombatSkillsQualifications(random, array, null, random.CheckPercentProb(abnormalOrgAttributesProb));
		for (sbyte b = 0; b < 14; b++)
		{
			int num2 = _baseCombatSkillQualifications.Items[b];
			num2 = num2 * (100 + randomEnemy.CombatSkillQualificationAdjust) / 100;
			if (num2 < 0)
			{
				num2 = 0;
			}
			_baseCombatSkillQualifications.Items[b] = (short)num2;
		}
	}

	[Obsolete("replaced with GameData.Domains.Character.Character.OfflineAddRandomEnemyOrgMemberCombatSkills")]
	private void OfflineAddRandomEnemyPresetOrgMemberCombatSkills(DataContext context, List<(OrganizationItem Org, OrganizationMemberItem Member)> relatedSectAndMembers, List<GameData.Domains.CombatSkill.CombatSkill> charCombatSkills)
	{
		sbyte behaviorType = BehaviorType.GetBehaviorType(_baseMorality);
		List<InventoryCombatSkillBookParams> skillBooks = new List<InventoryCombatSkillBookParams>();
		HashSet<short> hashSet = new HashSet<short>();
		int i = 0;
		for (int count = relatedSectAndMembers.Count; i < count; i++)
		{
			OrganizationItem item = relatedSectAndMembers[i].Org;
			OrganizationMemberItem item2 = relatedSectAndMembers[i].Member;
			sbyte teacherBehaviorType = (sbyte)((item.MainMorality == short.MinValue) ? (-1) : BehaviorType.GetBehaviorType(item.MainMorality));
			OfflineAddRandomEnemyPresetOrgMemberCombatSkillsInternal(context.Random, item.Goodness, teacherBehaviorType, behaviorType, item2.CombatSkills, charCombatSkills, skillBooks, hashSet);
		}
		OfflineAddSkillBooksAndWeapons(context, skillBooks, hashSet);
	}

	[Obsolete("replaced with GameData.Domains.Character.Character.OfflineAddRandomEnemyOrgMemberCombatSkills")]
	private void OfflineAddRandomEnemyPresetOrgMemberCombatSkillsInternal(IRandomSource random, sbyte orgGoodness, sbyte teacherBehaviorType, sbyte selfBehaviorType, List<PresetOrgMemberCombatSkill> presetSkills, List<GameData.Domains.CombatSkill.CombatSkill> charCombatSkills, List<InventoryCombatSkillBookParams> skillBooks, HashSet<short> suitableWeaponIds)
	{
		byte enemyPracticeLevel = DomainManager.Extra.GetEnemyPracticeLevel();
		short pageBuff = WorldCreation.Instance[(byte)11].InfluenceFactors[enemyPracticeLevel];
		short num = WorldCreation.Instance[(byte)11].SecondaryInfluenceFactors[enemyPracticeLevel];
		int i = 0;
		for (int count = presetSkills.Count; i < count; i++)
		{
			PresetOrgMemberCombatSkill presetOrgMemberCombatSkill = presetSkills[i];
			for (int j = 0; j <= presetOrgMemberCombatSkill.MaxGrade; j++)
			{
				if (j == presetOrgMemberCombatSkill.MaxGrade || random.CheckPercentProb(50))
				{
					short num2 = (short)(presetOrgMemberCombatSkill.SkillGroupId + j);
					int gradeDiff = _organizationInfo.Grade - j;
					var (pageTypes, readingState) = GenerateCombatSkillBookReadingInfo(random, teacherBehaviorType, selfBehaviorType, orgGoodness, gradeDiff, 100, pageBuff);
					charCombatSkills.Add(new GameData.Domains.CombatSkill.CombatSkill(-1, num2, readingState));
					_learnedCombatSkills.Add(num2);
					CombatSkillItem combatSkillItem = Config.CombatSkill.Instance[num2];
					if (random.CheckPercentProb(11 - j * 2))
					{
						skillBooks.Add(new InventoryCombatSkillBookParams(combatSkillItem.BookId, pageTypes));
					}
					short mostFittingWeaponID = combatSkillItem.MostFittingWeaponID;
					sbyte b = ItemDomain.GenerateRandomItemGrade(random, _organizationInfo.Grade);
					if (mostFittingWeaponID >= 0)
					{
						suitableWeaponIds.Add((short)(mostFittingWeaponID + b));
					}
				}
			}
		}
	}

	[Obsolete]
	private void OfflineCreateFeatures(DataContext context, Character mother, PregnantState pregnantState, Character father, DeadCharacter deadFather, bool randomFeaturesAtCreating, short potentialFeaturesAge = -1, int destinyType = -1)
	{
		IRandomSource random = context.Random;
		Dictionary<short, short> featureGroup2Id = new Dictionary<short, short>(16);
		GenerateFixedFeatures(featureGroup2Id);
		if (destinyType >= 0)
		{
			AddFeature(featureGroup2Id, DestinyType.Instance[destinyType].Feature);
		}
		if (randomFeaturesAtCreating)
		{
			GenerateGeneticFeatures(random, featureGroup2Id, mother, pregnantState, father, deadFather);
			AddFeature(featureGroup2Id, CharacterDomain.GetBirthdayFeatureId(_birthMonth));
			if (_currAge >= 1 && random.CheckPercentProb(50))
			{
				AddFeature(featureGroup2Id, CharacterDomain.GenerateOneYearOldCatchFeature(random));
			}
			GenerateRandomBasicFeatures(context, featureGroup2Id);
		}
		OfflineApplyFeatureIds(featureGroup2Id, potentialFeaturesAge);
	}

	[Obsolete]
	private void OfflineCreateCloseFriendRandomFeatures(DataContext context)
	{
		IRandomSource random = context.Random;
		Dictionary<short, short> featureGroup2Id = new Dictionary<short, short>(16);
		GenerateFixedFeatures(featureGroup2Id);
		AddFeature(featureGroup2Id, CharacterDomain.GetBirthdayFeatureId(_birthMonth));
		if (random.CheckPercentProb(50))
		{
			AddFeature(featureGroup2Id, CharacterDomain.GenerateOneYearOldCatchFeature(random));
		}
		AddFeature(featureGroup2Id, 164);
		GenerateRandomBasicFeatures(context, featureGroup2Id, isProtagonist: true, allGoodBasicFeatures: true);
		OfflineApplyFeatureIds(featureGroup2Id, -1);
	}

	[Obsolete]
	private static void AddFeature(Dictionary<short, short> featureGroup2Id, short featureId)
	{
		short mutexGroupId = CharacterFeature.Instance[featureId].MutexGroupId;
		featureGroup2Id.TryAdd(mutexGroupId, featureId);
	}

	[Obsolete]
	private static void ApplyFeatureGroup(Dictionary<short, short> featureGroup2Id, List<short> featureIds, short groupId)
	{
		if (featureGroup2Id.TryGetValue(groupId, out var value))
		{
			featureIds.Add(value);
			featureGroup2Id.Remove(groupId);
		}
	}

	[Obsolete]
	private void OfflineApplyFeatureIds(Dictionary<short, short> featureGroup2Id, short potentialFeaturesAge = -1)
	{
		_featureIds.Clear();
		_potentialFeatureIds.Clear();
		ApplyFeatureGroup(featureGroup2Id, _featureIds, 216);
		ApplyFeatureGroup(featureGroup2Id, _featureIds, 195);
		ApplyFeatureGroup(featureGroup2Id, _featureIds, 183);
		ApplyFeatureGroup(featureGroup2Id, _featureIds, 171);
		foreach (KeyValuePair<short, short> item in featureGroup2Id)
		{
			short value = item.Value;
			if (potentialFeaturesAge >= 0 && CharacterFeature.Instance[value].Mergeable)
			{
				_potentialFeatureIds.Add(value);
			}
			else
			{
				_featureIds.Add(value);
			}
		}
		if (potentialFeaturesAge >= 0)
		{
			int num = _potentialFeatureIds.Count * potentialFeaturesAge / 16;
			for (int i = 0; i < num; i++)
			{
				_featureIds.Add(_potentialFeatureIds[i]);
			}
		}
	}

	[Obsolete]
	private void GenerateFixedFeatures(Dictionary<short, short> featureGroup2Id)
	{
		short featureId = 195;
		short featureId2 = 216;
		int count = _featureIds.Count;
		for (int i = 0; i < count; i++)
		{
			short num = _featureIds[i];
			switch (CharacterFeature.Instance[num].MutexGroupId)
			{
			case 195:
				featureId = num;
				break;
			case 216:
				featureId2 = num;
				break;
			}
		}
		AddFeature(featureGroup2Id, featureId);
		AddFeature(featureGroup2Id, featureId2);
		for (int j = 0; j < count; j++)
		{
			AddFeature(featureGroup2Id, _featureIds[j]);
		}
	}

	[Obsolete]
	private void GenerateGeneticFeatures(IRandomSource random, Dictionary<short, short> featureGroup2Id, Character mother, PregnantState pregnantState, Character father, DeadCharacter deadFather)
	{
		if (mother == null && father == null && deadFather == null)
		{
			return;
		}
		List<short> list = null;
		List<short> list2 = null;
		if (pregnantState != null)
		{
			list = pregnantState.MotherFeatureIds;
			list2 = pregnantState.FatherFeatureIds;
		}
		else
		{
			if (mother != null)
			{
				list = mother.GetFeatureIds();
			}
			if (father != null)
			{
				list2 = father.GetFeatureIds();
			}
			else if (deadFather != null)
			{
				list2 = deadFather.FeatureIds;
			}
		}
		Dictionary<short, (short, short)> dictionary = new Dictionary<short, (short, short)>();
		if (list2 != null)
		{
			int count = list2.Count;
			for (int i = 0; i < count; i++)
			{
				short num = list2[i];
				CharacterFeatureItem characterFeatureItem = CharacterFeature.Instance[num];
				short mutexGroupId = characterFeatureItem.MutexGroupId;
				sbyte geneticProb = characterFeatureItem.GeneticProb;
				if (characterFeatureItem.Mergeable)
				{
					dictionary[mutexGroupId] = (-1, num);
				}
				else if (geneticProb > 0 && !featureGroup2Id.ContainsKey(mutexGroupId) && random.CheckPercentProb(geneticProb))
				{
					featureGroup2Id.Add(mutexGroupId, num);
				}
			}
		}
		if (list != null)
		{
			int count2 = list.Count;
			for (int j = 0; j < count2; j++)
			{
				short num2 = list[j];
				CharacterFeatureItem characterFeatureItem2 = CharacterFeature.Instance[num2];
				short mutexGroupId2 = characterFeatureItem2.MutexGroupId;
				sbyte geneticProb2 = characterFeatureItem2.GeneticProb;
				if (characterFeatureItem2.Mergeable)
				{
					if (dictionary.TryGetValue(mutexGroupId2, out var value))
					{
						dictionary[mutexGroupId2] = (num2, value.Item2);
					}
				}
				else if (geneticProb2 > 0 && !featureGroup2Id.ContainsKey(mutexGroupId2) && random.CheckPercentProb(geneticProb2))
				{
					featureGroup2Id.Add(mutexGroupId2, num2);
				}
			}
		}
		foreach (KeyValuePair<short, (short, short)> item in dictionary)
		{
			short key = item.Key;
			var (num3, num4) = item.Value;
			if (num3 < 0 || num4 < 0 || featureGroup2Id.ContainsKey(key))
			{
				continue;
			}
			sbyte level = CharacterFeature.Instance[num3].Level;
			sbyte level2 = CharacterFeature.Instance[num4].Level;
			sbyte b = (sbyte)((level + level2) / 2);
			if (level > 0 && level2 > 0)
			{
				int percentProb = (3 - b) * 40;
				if (random.CheckPercentProb(percentProb))
				{
					b++;
				}
			}
			else if (level < 0 && level2 < 0)
			{
				int percentProb2 = (3 + b) * 40;
				if (random.CheckPercentProb(percentProb2))
				{
					b--;
				}
			}
			if (b != 0)
			{
				short mergeableFeatureIdByLevel = CharacterDomain.GetMergeableFeatureIdByLevel(key, b);
				featureGroup2Id.Add(key, mergeableFeatureIdByLevel);
			}
		}
	}

	[Obsolete]
	private void GenerateRandomBasicFeatures(DataContext context, Dictionary<short, short> featureGroup2Id, bool isProtagonist = false, bool allGoodBasicFeatures = false)
	{
		IRandomSource random = context.Random;
		int num = (allGoodBasicFeatures ? 5 : GenerateRandomBasicFeaturesCount(context.Random));
		foreach (KeyValuePair<short, short> item in featureGroup2Id)
		{
			short value = item.Value;
			if (CharacterFeature.Instance[value].Basic)
			{
				num--;
			}
		}
		if (num <= 0)
		{
			return;
		}
		int num2 = random.Next(101);
		for (int i = 0; i < num; i++)
		{
			if (allGoodBasicFeatures || random.CheckPercentProb(num2))
			{
				var (key, num3) = CharacterDomain.GetRandomBasicFeature(random, isProtagonist, _gender, isPositive: true, featureGroup2Id);
				if (num3 >= 0)
				{
					featureGroup2Id.Add(key, num3);
					num2 -= 20;
				}
			}
			else
			{
				var (key2, num4) = CharacterDomain.GetRandomBasicFeature(random, isProtagonist, _gender, isPositive: false, featureGroup2Id);
				if (num4 >= 0)
				{
					featureGroup2Id.Add(key2, num4);
					num2 += 20;
				}
			}
		}
	}

	[Obsolete]
	private static int GenerateRandomBasicFeaturesCount(IRandomSource random)
	{
		return RedzenHelper.SkewDistribute(random, 4f, 0.333f, 3f, 3, 7);
	}

	[Obsolete("Use GameData.Domains.Organization.OrganizationDomain.GetRandomOrgMemberClothing instead.")]
	private static short GetOrgClothing(IRandomSource random, OrganizationMemberItem orgMemberConfig)
	{
		return OrganizationDomain.GetRandomOrgMemberClothing(random, orgMemberConfig);
	}

	private ProtagonistFeatureRelatedStatus OfflineApplyProtagonistFeaturesAndGenome(DataContext context, ProtagonistCreationInfo info, List<GameData.Domains.CombatSkill.CombatSkill> combatSkills)
	{
		ProtagonistFeatureRelatedStatus protagonistFeatureRelatedStatus = new ProtagonistFeatureRelatedStatus(combatSkills);
		int i = 0;
		for (int count = info.ProtagonistFeatureIds.Count; i < count; i++)
		{
			short protagonistFeatureId = info.ProtagonistFeatureIds[i];
			OfflineApplyProtagonistFeature(context, protagonistFeatureId, info, protagonistFeatureRelatedStatus);
		}
		OfflineCreateProtagonistRandomFeatures(context, info.InscribedChar?.FeatureIds, protagonistFeatureRelatedStatus.AddFeatureDreamLover, protagonistFeatureRelatedStatus.AddFeatureLifeSkillLearning, protagonistFeatureRelatedStatus.AddFeatureCombatSkillLearning, protagonistFeatureRelatedStatus.AddFeatureWhiteSnake, protagonistFeatureRelatedStatus.AddFeatureLongevity);
		Tester.Assert(!IsCompletelyInfected());
		Genome.CreateRandom(context.Random, ref _genome);
		return protagonistFeatureRelatedStatus;
	}

	private unsafe void OfflineApplyProtagonistFeature(DataContext context, short protagonistFeatureId, ProtagonistCreationInfo info, ProtagonistFeatureRelatedStatus status)
	{
		IRandomSource random = context.Random;
		switch (protagonistFeatureId)
		{
		case 0:
			OfflineApplyProtagonistFeature_MainAttribute(random, 0);
			break;
		case 1:
			OfflineApplyProtagonistFeature_MainAttribute(random, 1);
			break;
		case 2:
			OfflineApplyProtagonistFeature_MainAttribute(random, 2);
			break;
		case 3:
			OfflineApplyProtagonistFeature_MainAttribute(random, 3);
			break;
		case 4:
			OfflineApplyProtagonistFeature_MainAttribute(random, 4);
			break;
		case 5:
			OfflineApplyProtagonistFeature_MainAttribute(random, 5);
			break;
		case 6:
			status.CreateCloseFriend = true;
			break;
		case 7:
			status.AddFeatureDreamLover = true;
			break;
		case 8:
			status.AddFeatureWhiteSnake = true;
			break;
		case 9:
			status.AddFeatureLongevity = true;
			break;
		case 10:
			OfflineApplyProtagonistFeature_MaterialResources();
			break;
		case 11:
		{
			ref int reference = ref _resources.Items[6];
			reference += 10000;
			break;
		}
		case 12:
			OfflineApplyProtagonistFeature_Clothing(context);
			break;
		case 13:
			OfflineApplyProtagonistFeature_Wines(context);
			break;
		case 14:
			OfflineApplyProtagonistFeature_Horse(context);
			break;
		case 15:
			OfflineApplyProtagonistFeature_CricketJar(context);
			break;
		case 16:
			OfflineApplyProtagonistFeature_PoisonMaterials(context);
			break;
		case 17:
			OfflineApplyProtagonistFeature_Medicines(context);
			break;
		case 18:
			OfflineApplyProtagonistFeature_Accessory(context);
			break;
		case 19:
			DomainManager.Taiwu.SetProsperousConstruction(value: true, context);
			break;
		case 20:
			OfflineApplyProtagonistFeature_Literature(random);
			break;
		case 21:
			OfflineApplyProtagonistFeature_Religion(random);
			break;
		case 22:
			OfflineApplyProtagonistFeature_WitchDoctor(random);
			break;
		case 23:
			OfflineApplyProtagonistFeature_Artisan(random);
			break;
		case 24:
			status.AddFeatureLifeSkillLearning = true;
			break;
		case 25:
			status.AddFeatureCombatSkillLearning = true;
			break;
		case 26:
			OfflineApplyProtagonistFeature_CraftMaterials(context);
			break;
		case 27:
			OfflineApplyProtagonistFeature_CraftTools(context);
			break;
		case 28:
			OfflineApplyProtagonistFeature_MartialArtist(random);
			break;
		case 29:
			OfflineApplyProtagonistFeature_SkillBooks(context, info, status);
			break;
		default:
			throw new Exception($"Unsupported ProtagonistFeatureId: {protagonistFeatureId}");
		}
	}

	private unsafe void OfflineApplyProtagonistFeature_MainAttribute(IRandomSource random, sbyte mainAttributeType)
	{
		short num = (short)(70 + random.Next(11));
		if (num > _baseMainAttributes.Items[mainAttributeType])
		{
			_baseMainAttributes.Items[mainAttributeType] = num;
		}
	}

	private unsafe void OfflineApplyProtagonistFeature_MaterialResources()
	{
		for (int i = 0; i < 6; i++)
		{
			ref int reference = ref _resources.Items[i];
			reference += 1000;
		}
	}

	private void OfflineApplyProtagonistFeature_Clothing(DataContext context)
	{
		ItemKey itemKey = DomainManager.Item.CreateClothing(context, 4, _gender);
		_inventory.OfflineAdd(itemKey, 1);
		itemKey = DomainManager.Item.CreateClothing(context, 13, _gender);
		_inventory.OfflineAdd(itemKey, 1);
	}

	private void OfflineApplyProtagonistFeature_Wines(DataContext context)
	{
		IRandomSource random = context.Random;
		ItemKey itemKey = DomainManager.Item.CreateItem(context, 9, 3);
		int amount = 1 + random.Next(2);
		_inventory.OfflineAdd(itemKey, amount);
		amount = random.Next(2);
		if (amount > 0)
		{
			itemKey = DomainManager.Item.CreateItem(context, 9, 4);
			_inventory.OfflineAdd(itemKey, amount);
		}
		itemKey = DomainManager.Item.CreateItem(context, 9, 12);
		amount = 1 + random.Next(2);
		_inventory.OfflineAdd(itemKey, amount);
		amount = random.Next(2);
		if (amount > 0)
		{
			itemKey = DomainManager.Item.CreateItem(context, 9, 13);
			_inventory.OfflineAdd(itemKey, amount);
		}
	}

	private void OfflineApplyProtagonistFeature_Horse(DataContext context)
	{
		ItemKey itemKey = DomainManager.Item.CreateCarrier(context, 22);
		_inventory.OfflineAdd(itemKey, 1);
	}

	private void OfflineApplyProtagonistFeature_CricketJar(DataContext context)
	{
		ItemKey itemKey = DomainManager.Item.CreateMisc(context, 87);
		_inventory.OfflineAdd(itemKey, 1);
	}

	private void OfflineApplyProtagonistFeature_PoisonMaterials(DataContext context)
	{
		IRandomSource random = context.Random;
		int num = random.Next(6);
		int num2 = random.Next((num == 0) ? 1 : 0, 3);
		if (num2 > 0)
		{
			ItemKey itemKey = DomainManager.Item.CreateMaterial(context, 240);
			_inventory.OfflineAdd(itemKey, num2);
		}
		num2 = random.Next((num == 1) ? 1 : 0, 3);
		if (num2 > 0)
		{
			ItemKey itemKey2 = DomainManager.Item.CreateMaterial(context, 247);
			_inventory.OfflineAdd(itemKey2, num2);
		}
		num2 = random.Next((num == 2) ? 1 : 0, 3);
		if (num2 > 0)
		{
			ItemKey itemKey3 = DomainManager.Item.CreateMaterial(context, 254);
			_inventory.OfflineAdd(itemKey3, num2);
		}
		num2 = random.Next((num == 3) ? 1 : 0, 3);
		if (num2 > 0)
		{
			ItemKey itemKey4 = DomainManager.Item.CreateMaterial(context, 261);
			_inventory.OfflineAdd(itemKey4, num2);
		}
		num2 = random.Next((num == 4) ? 1 : 0, 3);
		if (num2 > 0)
		{
			ItemKey itemKey5 = DomainManager.Item.CreateMaterial(context, 268);
			_inventory.OfflineAdd(itemKey5, num2);
		}
		num2 = random.Next((num == 5) ? 1 : 0, 3);
		if (num2 > 0)
		{
			ItemKey itemKey6 = DomainManager.Item.CreateMaterial(context, 275);
			_inventory.OfflineAdd(itemKey6, num2);
		}
	}

	private void OfflineApplyProtagonistFeature_Medicines(DataContext context)
	{
		ItemKey itemKey = DomainManager.Item.CreateItem(context, 8, 59);
		_inventory.OfflineAdd(itemKey, 1);
		itemKey = DomainManager.Item.CreateItem(context, 8, 71);
		_inventory.OfflineAdd(itemKey, 1);
		itemKey = DomainManager.Item.CreateItem(context, 8, 87);
		_inventory.OfflineAdd(itemKey, 1);
		itemKey = DomainManager.Item.CreateItem(context, 8, 99);
		_inventory.OfflineAdd(itemKey, 1);
	}

	private void OfflineApplyProtagonistFeature_Accessory(DataContext context)
	{
		short templateId = (short)context.Random.Next(225, 250);
		ItemKey itemKey = DomainManager.Item.CreateAccessory(context, templateId, 1);
		_inventory.OfflineAdd(itemKey, 1);
	}

	private unsafe void OfflineApplyProtagonistFeature_Literature(IRandomSource random)
	{
		sbyte* pLifeSkillTypes = stackalloc sbyte[4] { 0, 1, 2, 3 };
		OfflineBuffLifeSkills(random, pLifeSkillTypes, 4);
	}

	private unsafe void OfflineApplyProtagonistFeature_Religion(IRandomSource random)
	{
		sbyte* pLifeSkillTypes = stackalloc sbyte[4] { 13, 12, 5, 14 };
		OfflineBuffLifeSkills(random, pLifeSkillTypes, 4);
	}

	private unsafe void OfflineApplyProtagonistFeature_WitchDoctor(IRandomSource random)
	{
		sbyte* pLifeSkillTypes = stackalloc sbyte[4] { 8, 9, 4, 15 };
		OfflineBuffLifeSkills(random, pLifeSkillTypes, 4);
	}

	private unsafe void OfflineApplyProtagonistFeature_Artisan(IRandomSource random)
	{
		sbyte* pLifeSkillTypes = stackalloc sbyte[4] { 6, 7, 11, 10 };
		OfflineBuffLifeSkills(random, pLifeSkillTypes, 4);
	}

	private unsafe void OfflineBuffLifeSkills(IRandomSource random, sbyte* pLifeSkillTypes, int count)
	{
		CollectionUtils.Shuffle(random, pLifeSkillTypes, count);
		ref LifeSkillShorts baseLifeSkillQualifications = ref GetBaseLifeSkillQualifications();
		for (int i = 0; i < count; i++)
		{
			sbyte b = pLifeSkillTypes[i];
			short num = (short)(70 + i * 5 + random.Next(11));
			if (num > baseLifeSkillQualifications.Items[b])
			{
				baseLifeSkillQualifications.Items[b] = num;
			}
		}
	}

	private void OfflineApplyProtagonistFeature_CraftMaterials(DataContext context)
	{
		IRandomSource random = context.Random;
		ItemKey itemKey = DomainManager.Item.CreateItem(context, 5, 5);
		int amount = 1 + random.Next(2);
		_inventory.OfflineAdd(itemKey, amount);
		itemKey = DomainManager.Item.CreateItem(context, 5, 12);
		amount = 1 + random.Next(2);
		_inventory.OfflineAdd(itemKey, amount);
		itemKey = DomainManager.Item.CreateItem(context, 5, 19);
		amount = 1 + random.Next(2);
		_inventory.OfflineAdd(itemKey, amount);
		itemKey = DomainManager.Item.CreateItem(context, 5, 26);
		amount = 1 + random.Next(2);
		_inventory.OfflineAdd(itemKey, amount);
		itemKey = DomainManager.Item.CreateItem(context, 5, 33);
		amount = 1 + random.Next(2);
		_inventory.OfflineAdd(itemKey, amount);
		itemKey = DomainManager.Item.CreateItem(context, 5, 40);
		amount = 1 + random.Next(2);
		_inventory.OfflineAdd(itemKey, amount);
		itemKey = DomainManager.Item.CreateItem(context, 5, 47);
		amount = 1 + random.Next(2);
		_inventory.OfflineAdd(itemKey, amount);
		itemKey = DomainManager.Item.CreateItem(context, 5, 54);
		amount = 1 + random.Next(2);
		_inventory.OfflineAdd(itemKey, amount);
	}

	private unsafe void OfflineApplyProtagonistFeature_CraftTools(DataContext context)
	{
		IRandomSource random = context.Random;
		short* pArray = stackalloc short[6] { 5, 14, 23, 32, 41, 50 };
		short* pArray2 = stackalloc short[6] { 2, 16, 30, 44, 79, 142 };
		pArray = CollectionUtils.Shuffle(random, pArray, 6, 3);
		pArray2 = CollectionUtils.Shuffle(random, pArray2, 6, 3);
		for (int i = 0; i < 3; i++)
		{
			ItemKey itemKey = DomainManager.Item.CreateItem(context, 6, pArray[i]);
			ItemKey itemKey2 = DomainManager.Item.CreateItem(context, 5, pArray2[i]);
			_inventory.OfflineAdd(itemKey, 1);
			_inventory.OfflineAdd(itemKey2, 1);
		}
	}

	private unsafe void OfflineApplyProtagonistFeature_MartialArtist(IRandomSource random)
	{
		ref short items = ref _baseCombatSkillQualifications.Items[0];
		items = (short)(90 + random.Next(11));
		_baseCombatSkillQualifications.Items[1] = (short)(90 + random.Next(11));
		_baseCombatSkillQualifications.Items[2] = (short)(90 + random.Next(11));
	}

	private unsafe void OfflineApplyProtagonistFeature_SkillBooks(DataContext context, ProtagonistCreationInfo info, ProtagonistFeatureRelatedStatus status)
	{
		IRandomSource random = context.Random;
		short num = ProtagonistLifeSkillBookIds[random.Next(ProtagonistLifeSkillBookIds.Length)];
		ItemKey itemKey = DomainManager.Item.CreateSkillBook(context, num, 5, 0, -1, 50);
		_inventory.OfflineAdd(itemKey, 1);
		short lifeSkillTemplateIdFromSkillBook = ItemTemplateHelper.GetLifeSkillTemplateIdFromSkillBook(num);
		Config.LifeSkillItem lifeSkillItem = LifeSkill.Instance[lifeSkillTemplateIdFromSkillBook];
		LifeSkillItem item = new LifeSkillItem(lifeSkillTemplateIdFromSkillBook);
		_learnedLifeSkills.Add(item);
		status.ReadLifeSkillTemplateId = lifeSkillTemplateIdFromSkillBook;
		int num2 = 90 + random.Next(10);
		if (_baseLifeSkillQualifications.Items[lifeSkillItem.Type] < num2)
		{
			_baseLifeSkillQualifications.Items[lifeSkillItem.Type] = (short)num2;
		}
		sbyte sectOrgTemplateIdByStateTemplateId = MapDomain.GetSectOrgTemplateIdByStateTemplateId(info.TaiwuVillageStateTemplateId);
		sbyte largeSectIndex = OrganizationDomain.GetLargeSectIndex(sectOrgTemplateIdByStateTemplateId);
		short[] array = ProtagonistCombatSkillBookIds[largeSectIndex];
		short num3 = array[random.Next(array.Length)];
		itemKey = DomainManager.Item.CreateSkillBook(context, num3, 5, 0, -1, 50);
		_inventory.OfflineAdd(itemKey, 1);
		short combatSkillTemplateIdFromSkillBook = ItemTemplateHelper.GetCombatSkillTemplateIdFromSkillBook(num3);
		CombatSkillItem combatSkillItem = Config.CombatSkill.Instance[combatSkillTemplateIdFromSkillBook];
		GameData.Domains.CombatSkill.CombatSkill item2 = new GameData.Domains.CombatSkill.CombatSkill(-1, combatSkillTemplateIdFromSkillBook, 0);
		status.CombatSkills.Add(item2);
		_learnedCombatSkills.Add(combatSkillTemplateIdFromSkillBook);
		status.ReadCombatSkillTemplateId = combatSkillTemplateIdFromSkillBook;
		GameData.Domains.Item.SkillBook element_SkillBooks = DomainManager.Item.GetElement_SkillBooks(itemKey.Id);
		status.CombatSkillBookPageTypes = element_SkillBooks.GetPageTypes();
		int num4 = 90 + random.Next(10);
		if (_baseCombatSkillQualifications.Items[combatSkillItem.Type] < num4)
		{
			_baseCombatSkillQualifications.Items[combatSkillItem.Type] = (short)num4;
		}
	}

	ValueInfo IValueSelector.SelectValue(Evaluator evaluator, string identifier)
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		if (1 == 0)
		{
		}
		ValueInfo result = ((!(identifier == "MapBlock")) ? ValueInfo.Void : evaluator.PushEvaluationResult((object)DomainManager.Map.GetBlock(GetValidLocation())));
		if (1 == 0)
		{
		}
		return result;
	}
}
