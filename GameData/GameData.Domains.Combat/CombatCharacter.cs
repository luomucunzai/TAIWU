using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Config;
using GameData.ArchiveData;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Dependencies;
using GameData.DomainEvents;
using GameData.Domains.Building;
using GameData.Domains.Character;
using GameData.Domains.Character.Relation;
using GameData.Domains.Combat.Ai;
using GameData.Domains.Combat.MixPoison;
using GameData.Domains.CombatSkill;
using GameData.Domains.Item;
using GameData.Domains.SpecialEffect;
using GameData.Domains.SpecialEffect.SectStory.Yuanshan;
using GameData.Domains.SpecialEffect.SectStory.Zhujian;
using GameData.Domains.Taiwu.Profession.SkillsData;
using GameData.Domains.TaiwuEvent;
using GameData.GameDataBridge;
using GameData.Serializer;
using GameData.Utilities;
using Redzen.Random;

namespace GameData.Domains.Combat;

[SerializableGameData(NotForDisplayModule = true)]
public class CombatCharacter : BaseGameDataObject, IExpressionConverter, IAiParticipant, ISerializableGameData
{
	internal class FixedFieldInfos
	{
		public const uint Id_Offset = 0u;

		public const int Id_Size = 4;

		public const uint BreathValue_Offset = 4u;

		public const int BreathValue_Size = 4;

		public const uint StanceValue_Offset = 8u;

		public const int StanceValue_Size = 4;

		public const uint NeiliAllocation_Offset = 12u;

		public const int NeiliAllocation_Size = 8;

		public const uint OriginNeiliAllocation_Offset = 20u;

		public const int OriginNeiliAllocation_Size = 8;

		public const uint NeiliAllocationRecoverProgress_Offset = 28u;

		public const int NeiliAllocationRecoverProgress_Size = 8;

		public const uint OldDisorderOfQi_Offset = 36u;

		public const int OldDisorderOfQi_Size = 2;

		public const uint NeiliType_Offset = 38u;

		public const int NeiliType_Size = 1;

		public const uint AvoidToShow_Offset = 39u;

		public const int AvoidToShow_Size = 4;

		public const uint CurrentPosition_Offset = 43u;

		public const int CurrentPosition_Size = 4;

		public const uint DisplayPosition_Offset = 47u;

		public const int DisplayPosition_Size = 4;

		public const uint MobilityValue_Offset = 51u;

		public const int MobilityValue_Size = 4;

		public const uint JumpPrepareProgress_Offset = 55u;

		public const int JumpPrepareProgress_Size = 1;

		public const uint JumpPreparedDistance_Offset = 56u;

		public const int JumpPreparedDistance_Size = 2;

		public const uint MobilityLockEffectCount_Offset = 58u;

		public const int MobilityLockEffectCount_Size = 2;

		public const uint JumpChangeDistanceDuration_Offset = 60u;

		public const int JumpChangeDistanceDuration_Size = 4;

		public const uint UsingWeaponIndex_Offset = 64u;

		public const int UsingWeaponIndex_Size = 4;

		public const uint WeaponTricks_Offset = 68u;

		public const int WeaponTricks_Size = 6;

		public const uint WeaponTrickIndex_Offset = 74u;

		public const int WeaponTrickIndex_Size = 1;

		public const uint Weapons_Offset = 75u;

		public const int Weapons_Size = 56;

		public const uint AttackingTrickType_Offset = 131u;

		public const int AttackingTrickType_Size = 1;

		public const uint CanAttackOutRange_Offset = 132u;

		public const int CanAttackOutRange_Size = 1;

		public const uint ChangeTrickProgress_Offset = 133u;

		public const int ChangeTrickProgress_Size = 1;

		public const uint ChangeTrickCount_Offset = 134u;

		public const int ChangeTrickCount_Size = 2;

		public const uint CanChangeTrick_Offset = 136u;

		public const int CanChangeTrick_Size = 1;

		public const uint ChangingTrick_Offset = 137u;

		public const int ChangingTrick_Size = 1;

		public const uint ChangeTrickAttack_Offset = 138u;

		public const int ChangeTrickAttack_Size = 1;

		public const uint IsFightBack_Offset = 139u;

		public const int IsFightBack_Size = 1;

		public const uint Injuries_Offset = 140u;

		public const int Injuries_Size = 16;

		public const uint OldInjuries_Offset = 156u;

		public const int OldInjuries_Size = 16;

		public const uint DamageStepCollection_Offset = 172u;

		public const int DamageStepCollection_Size = 64;

		public const uint OuterDamageValue_Offset = 236u;

		public const int OuterDamageValue_Size = 28;

		public const uint InnerDamageValue_Offset = 264u;

		public const int InnerDamageValue_Size = 28;

		public const uint MindDamageValue_Offset = 292u;

		public const int MindDamageValue_Size = 4;

		public const uint FatalDamageValue_Offset = 296u;

		public const int FatalDamageValue_Size = 4;

		public const uint OuterDamageValueToShow_Offset = 300u;

		public const int OuterDamageValueToShow_Size = 56;

		public const uint InnerDamageValueToShow_Offset = 356u;

		public const int InnerDamageValueToShow_Size = 56;

		public const uint MindDamageValueToShow_Offset = 412u;

		public const int MindDamageValueToShow_Size = 4;

		public const uint FatalDamageValueToShow_Offset = 416u;

		public const int FatalDamageValueToShow_Size = 4;

		public const uint FlawCount_Offset = 420u;

		public const int FlawCount_Size = 7;

		public const uint AcupointCount_Offset = 427u;

		public const int AcupointCount_Size = 7;

		public const uint Poison_Offset = 434u;

		public const int Poison_Size = 24;

		public const uint OldPoison_Offset = 458u;

		public const int OldPoison_Size = 24;

		public const uint PoisonResist_Offset = 482u;

		public const int PoisonResist_Size = 24;

		public const uint NewPoisonsToShow_Offset = 506u;

		public const int NewPoisonsToShow_Size = 18;

		public const uint PreparingSkillId_Offset = 524u;

		public const int PreparingSkillId_Size = 2;

		public const uint SkillPreparePercent_Offset = 526u;

		public const int SkillPreparePercent_Size = 1;

		public const uint PerformingSkillId_Offset = 527u;

		public const int PerformingSkillId_Size = 2;

		public const uint AutoCastingSkill_Offset = 529u;

		public const int AutoCastingSkill_Size = 1;

		public const uint AttackSkillAttackIndex_Offset = 530u;

		public const int AttackSkillAttackIndex_Size = 1;

		public const uint AttackSkillPower_Offset = 531u;

		public const int AttackSkillPower_Size = 1;

		public const uint AffectingMoveSkillId_Offset = 532u;

		public const int AffectingMoveSkillId_Size = 2;

		public const uint AffectingDefendSkillId_Offset = 534u;

		public const int AffectingDefendSkillId_Size = 2;

		public const uint DefendSkillTimePercent_Offset = 536u;

		public const int DefendSkillTimePercent_Size = 1;

		public const uint WugCount_Offset = 537u;

		public const int WugCount_Size = 2;

		public const uint HealInjuryCount_Offset = 539u;

		public const int HealInjuryCount_Size = 1;

		public const uint HealPoisonCount_Offset = 540u;

		public const int HealPoisonCount_Size = 1;

		public const uint OtherActionCanUse_Offset = 541u;

		public const int OtherActionCanUse_Size = 5;

		public const uint PreparingOtherAction_Offset = 546u;

		public const int PreparingOtherAction_Size = 1;

		public const uint OtherActionPreparePercent_Offset = 547u;

		public const int OtherActionPreparePercent_Size = 1;

		public const uint CanSurrender_Offset = 548u;

		public const int CanSurrender_Size = 1;

		public const uint CanUseItem_Offset = 549u;

		public const int CanUseItem_Size = 1;

		public const uint PreparingItem_Offset = 550u;

		public const int PreparingItem_Size = 8;

		public const uint UseItemPreparePercent_Offset = 558u;

		public const int UseItemPreparePercent_Size = 1;

		public const uint CombatReserveData_Offset = 559u;

		public const int CombatReserveData_Size = 12;

		public const uint XiangshuEffectId_Offset = 571u;

		public const int XiangshuEffectId_Size = 2;

		public const uint HazardValue_Offset = 573u;

		public const int HazardValue_Size = 4;

		public const uint AnimationTimeScale_Offset = 577u;

		public const int AnimationTimeScale_Size = 4;

		public const uint AttackOutOfRange_Offset = 581u;

		public const int AttackOutOfRange_Size = 1;

		public const uint BossPhase_Offset = 582u;

		public const int BossPhase_Size = 1;

		public const uint AnimalAttackCount_Offset = 583u;

		public const int AnimalAttackCount_Size = 1;

		public const uint ShowTransferInjuryCommand_Offset = 584u;

		public const int ShowTransferInjuryCommand_Size = 1;

		public const uint ExecutingTeammateCommand_Offset = 585u;

		public const int ExecutingTeammateCommand_Size = 1;

		public const uint Visible_Offset = 586u;

		public const int Visible_Size = 1;

		public const uint TeammateCommandPreparePercent_Offset = 587u;

		public const int TeammateCommandPreparePercent_Size = 1;

		public const uint TeammateCommandTimePercent_Offset = 588u;

		public const int TeammateCommandTimePercent_Size = 1;

		public const uint AttackCommandWeaponKey_Offset = 589u;

		public const int AttackCommandWeaponKey_Size = 8;

		public const uint AttackCommandTrickType_Offset = 597u;

		public const int AttackCommandTrickType_Size = 1;

		public const uint DefendCommandSkillId_Offset = 598u;

		public const int DefendCommandSkillId_Size = 2;

		public const uint ShowEffectCommandIndex_Offset = 600u;

		public const int ShowEffectCommandIndex_Size = 1;

		public const uint AttackCommandSkillId_Offset = 601u;

		public const int AttackCommandSkillId_Size = 2;

		public const uint TargetDistance_Offset = 603u;

		public const int TargetDistance_Size = 2;

		public const uint NeiliAllocationCd_Offset = 605u;

		public const int NeiliAllocationCd_Size = 8;

		public const uint ProportionDelta_Offset = 613u;

		public const int ProportionDelta_Size = 8;

		public const uint MindMarkInfinityCount_Offset = 621u;

		public const int MindMarkInfinityCount_Size = 4;

		public const uint MindMarkInfinityProgress_Offset = 625u;

		public const int MindMarkInfinityProgress_Size = 4;

		public const uint NormalAttackRecovery_Offset = 629u;

		public const int NormalAttackRecovery_Size = 8;

		public const uint ReserveNormalAttack_Offset = 637u;

		public const int ReserveNormalAttack_Size = 1;

		public const uint Gangqi_Offset = 638u;

		public const int Gangqi_Size = 4;

		public const uint GangqiMax_Offset = 642u;

		public const int GangqiMax_Size = 4;
	}

	private GameData.Domains.Character.Character _character;

	private CombatDomain _combatDomain;

	public readonly CombatCharacterStateMachine StateMachine = new CombatCharacterStateMachine();

	public bool IsAlly;

	public bool IsTaiwu;

	[CollectionObjectField(false, true, false, true, false)]
	private int _id;

	public int OriginXiangshuInfection;

	public sbyte OriginNeiliType;

	[CollectionObjectField(false, true, false, false, false)]
	private short _oldDisorderOfQi;

	[CollectionObjectField(false, true, false, false, false)]
	private sbyte _neiliType;

	[CollectionObjectField(false, true, false, false, false)]
	private NeiliProportionOfFiveElements _proportionDelta;

	[CollectionObjectField(false, true, false, false, false)]
	private int _breathValue;

	[CollectionObjectField(false, true, false, false, false)]
	private int _stanceValue;

	public bool LockMaxBreath = false;

	public bool LockMaxStance = false;

	[CollectionObjectField(false, true, false, false, false)]
	private NeiliAllocation _neiliAllocation;

	[CollectionObjectField(false, true, false, false, false)]
	private NeiliAllocation _originNeiliAllocation;

	[CollectionObjectField(false, true, false, false, false)]
	private NeiliAllocation _neiliAllocationRecoverProgress;

	[CollectionObjectField(false, true, false, false, false)]
	private SilenceFrameData _neiliAllocationCd;

	private NeiliAllocation _originBaseNeiliAllocation;

	public int[] NeiliAllocationAutoRecoverProgress = new int[4];

	[CollectionObjectField(false, true, false, false, false)]
	private List<int> _unlockPrepareValue;

	[CollectionObjectField(false, true, false, false, false)]
	private sbyte _changeTrickProgress;

	[CollectionObjectField(false, true, false, false, false)]
	private short _changeTrickCount;

	[CollectionObjectField(false, false, true, false, false)]
	private short _moveCd;

	[CollectionObjectField(false, true, false, false, false)]
	private int _mobilityValue;

	[CollectionObjectField(false, false, true, false, false)]
	private byte _mobilityLevel;

	[CollectionObjectField(false, false, true, false, false)]
	private int _mobilityRecoverSpeed;

	[CollectionObjectField(false, true, false, false, false)]
	private sbyte _jumpPrepareProgress;

	[CollectionObjectField(false, true, false, false, false)]
	private short _jumpPreparedDistance;

	[CollectionObjectField(false, true, false, false, false)]
	private int _usingWeaponIndex;

	[CollectionObjectField(false, true, false, false, false, ArrayElementsCount = 6)]
	private sbyte[] _weaponTricks;

	[CollectionObjectField(false, true, false, false, false)]
	private byte _weaponTrickIndex;

	[CollectionObjectField(false, true, false, false, false, ArrayElementsCount = 7)]
	private ItemKey[] _weapons;

	[CollectionObjectField(false, true, false, false, false)]
	private List<int> _rawCreateEffects;

	[CollectionObjectField(false, true, false, false, false)]
	private RawCreateCollection _rawCreateCollection;

	public readonly ItemKey[] Armors = new ItemKey[7];

	public readonly Stack<ItemKey> ChangingDurabilityItems = new Stack<ItemKey>();

	[CollectionObjectField(false, false, true, false, false)]
	private List<ItemKey> _validItems = new List<ItemKey>();

	[CollectionObjectField(false, false, true, false, false)]
	private List<ItemKeyAndCount> _validItemAndCounts = new List<ItemKeyAndCount>();

	[CollectionObjectField(false, true, false, false, false)]
	private List<short> _neigongList;

	[CollectionObjectField(false, true, false, false, false)]
	private List<short> _attackSkillList;

	[CollectionObjectField(false, true, false, false, false)]
	private List<short> _agileSkillList;

	[CollectionObjectField(false, true, false, false, false)]
	private List<short> _defenceSkillList;

	[CollectionObjectField(false, true, false, false, false)]
	private List<short> _assistSkillList;

	[CollectionObjectField(false, true, false, false, false)]
	private TrickCollection _tricks;

	[CollectionObjectField(false, false, true, false, false)]
	private int _maxTrickCount;

	public readonly List<sbyte> InterchangeableTricks = new List<sbyte>();

	[CollectionObjectField(false, true, false, false, false)]
	private short _wugCount;

	[CollectionObjectField(false, true, false, false, false)]
	private Injuries _injuries;

	[CollectionObjectField(false, true, false, false, false)]
	private Injuries _oldInjuries;

	[CollectionObjectField(false, true, false, false, false)]
	private InjuryAutoHealCollection _injuryAutoHealCollection;

	[CollectionObjectField(false, true, false, false, false)]
	private InjuryAutoHealCollection _oldInjuryAutoHealCollection;

	[CollectionObjectField(false, false, true, false, false)]
	private HeavyOrBreakInjuryData _heavyOrBreakInjuryData;

	public readonly List<short> OuterInjuryAutoHealSpeeds = new List<short>();

	public readonly List<short> InnerInjuryAutoHealSpeeds = new List<short>();

	public readonly List<short> OuterOldInjuryAutoHealSpeeds = new List<short>();

	public readonly List<short> InnerOldInjuryAutoHealSpeeds = new List<short>();

	[CollectionObjectField(false, true, false, false, false)]
	private PoisonInts _poison;

	[CollectionObjectField(false, true, false, false, false)]
	private PoisonInts _oldPoison;

	[CollectionObjectField(false, true, false, false, false)]
	private PoisonInts _poisonResist;

	[CollectionObjectField(false, true, false, false, false)]
	private MixPoisonAffectedCountCollection _mixPoisonAffectedCount;

	private readonly short[] _poisonAffectAccumulator = new short[6];

	private DataUid _poisonResistUid;

	[CollectionObjectField(false, true, false, false, false, ArrayElementsCount = 7)]
	private byte[] _flawCount;

	[CollectionObjectField(false, true, false, false, false)]
	private FlawOrAcupointCollection _flawCollection;

	[CollectionObjectField(false, true, false, false, false, ArrayElementsCount = 7)]
	private byte[] _acupointCount;

	[CollectionObjectField(false, true, false, false, false)]
	private FlawOrAcupointCollection _acupointCollection;

	[CollectionObjectField(false, true, false, false, false)]
	private MindMarkList _mindMarkTime;

	[CollectionObjectField(false, true, false, false, false, ArrayElementsCount = 7)]
	private int[] _outerDamageValue;

	[CollectionObjectField(false, true, false, false, false, ArrayElementsCount = 7)]
	private int[] _innerDamageValue;

	[CollectionObjectField(false, true, false, false, false)]
	private int _mindDamageValue;

	[CollectionObjectField(false, true, false, false, false)]
	private int _fatalDamageValue;

	[CollectionObjectField(false, true, false, false, false)]
	private int _mindMarkInfinityCount;

	[CollectionObjectField(false, true, false, false, false)]
	private int _mindMarkInfinityProgress;

	[CollectionObjectField(false, true, false, false, false)]
	private DamageStepCollection _damageStepCollection;

	[CollectionObjectField(false, true, false, false, false)]
	private DefeatMarkCollection _defeatMarkCollection;

	private DataUid _defeatMarkUid;

	public bool ForceDefeat = false;

	public bool Immortal;

	[CollectionObjectField(false, true, false, false, false)]
	private int _gangqi;

	[CollectionObjectField(false, true, false, false, false)]
	private int _gangqiMax;

	public bool NeedReduceWeaponDurability;

	public bool NeedReduceArmorDurability;

	public bool BeCriticalDuringCalcAddInjury;

	public int BeCalcInjuryInnerRatio;

	[CollectionObjectField(false, true, false, false, false)]
	private short _targetDistance;

	[CollectionObjectField(false, true, false, false, false)]
	private short _mobilityLockEffectCount;

	[CollectionObjectField(false, false, true, false, false)]
	private float _changeDistanceDuration;

	[CollectionObjectField(false, true, false, false, false)]
	private float _jumpChangeDistanceDuration;

	public bool KeepMoving;

	public bool MoveForward;

	public bool PlayerControllingMove;

	public short AiTargetDistance;

	public short PlayerTargetDistance;

	public sbyte PlayerChangeTrickType;

	public sbyte PlayerChangeTrickBodyPart;

	public MoveData MoveData;

	public bool NeedPauseJumpMove;

	public short PauseJumpMoveSkillId;

	public int PauseJumpMoveDistance;

	[CollectionObjectField(false, true, false, false, false)]
	private SilenceFrameData _normalAttackRecovery;

	[CollectionObjectField(false, true, false, false, false)]
	private bool _canChangeTrick;

	[CollectionObjectField(false, true, false, false, false)]
	private bool _changingTrick;

	[CollectionObjectField(false, true, false, false, false)]
	private bool _changeTrickAttack;

	[CollectionObjectField(false, true, false, false, false)]
	private bool _isFightBack;

	[CollectionObjectField(false, true, false, false, false)]
	private sbyte _attackingTrickType;

	[CollectionObjectField(false, true, false, false, false)]
	private bool _canAttackOutRange;

	[CollectionObjectField(false, false, true, false, false)]
	private List<bool> _canUnlockAttack = new List<bool>();

	public byte PursueAttackCount;

	public sbyte NormalAttackHitType;

	public sbyte NormalAttackBodyPart;

	public sbyte ChangeTrickType;

	public sbyte ChangeTrickBodyPart;

	public bool NeedChangeTrickAttack;

	public EFlawOrAcupointType ChangeTrickFlawOrAcupointType;

	public int UnlockWeaponIndex;

	public sbyte FightBackHitType;

	public bool FightBackWithHit;

	public bool NeedUnlockAttack;

	public bool NeedNormalAttackImmediate;

	public int NeedNormalAttackSkipPrepare;

	public bool NeedBreakAttack;

	public bool IsBreakAttacking;

	public int ForbidNormalAttackEffectCount;

	public bool CanNormalAttackInPrepareSkill;

	public byte NormalAttackLeftRepeatTimes;

	public bool NormalAttackRepeatIsFightBack;

	public bool NextAttackNoPrepare;

	public bool NeedFreeAttack;

	public bool IsAutoNormalAttacking;

	public bool IsAutoNormalAttackingSpecial;

	[CollectionObjectField(false, true, false, false, false)]
	private short _preparingSkillId;

	[CollectionObjectField(false, true, false, false, false)]
	private byte _skillPreparePercent;

	[CollectionObjectField(false, true, false, false, false)]
	private short _performingSkillId;

	[CollectionObjectField(false, true, false, false, false)]
	private bool _autoCastingSkill;

	[CollectionObjectField(false, true, false, false, false)]
	private byte _attackSkillAttackIndex;

	[CollectionObjectField(false, true, false, false, false)]
	private byte _attackSkillPower;

	public int SkillPrepareTotalProgress;

	public int SkillPrepareCurrProgress;

	public sbyte SkillAttackBodyPart;

	public readonly sbyte[] SkillHitType = new sbyte[3];

	public readonly int[] SkillHitValue = new int[3];

	public readonly int[] SkillAvoidValue = new int[3];

	public int SkillFinalAttackHitIndex;

	[CollectionObjectField(false, true, false, false, false)]
	private short _affectingMoveSkillId;

	public short NeedAddEffectAgileSkillId;

	[CollectionObjectField(false, true, false, false, false)]
	private short _affectingDefendSkillId;

	[CollectionObjectField(false, true, false, false, false)]
	private byte _defendSkillTimePercent;

	public short DefendSkillTotalFrame;

	public short DefendSkillLeftFrame;

	[CollectionObjectField(false, true, false, false, false)]
	private ShowAvoidData _avoidToShow;

	[CollectionObjectField(false, true, false, false, false, ArrayElementsCount = 7)]
	private IntPair[] _outerDamageValueToShow;

	[CollectionObjectField(false, true, false, false, false, ArrayElementsCount = 7)]
	private IntPair[] _innerDamageValueToShow;

	[CollectionObjectField(false, true, false, false, false)]
	private PoisonsAndLevels _newPoisonsToShow;

	[CollectionObjectField(false, true, false, false, false)]
	private int _mindDamageValueToShow;

	[CollectionObjectField(false, true, false, false, false)]
	private int _fatalDamageValueToShow;

	[CollectionObjectField(false, true, false, false, false)]
	private int _currentPosition;

	[CollectionObjectField(false, true, false, false, false)]
	private int _displayPosition;

	[CollectionObjectField(false, true, false, false, false)]
	private short _xiangshuEffectId;

	[CollectionObjectField(false, true, false, false, false)]
	private ShowSpecialEffectCollection _showEffectList;

	[CollectionObjectField(false, true, false, false, false)]
	private List<TeammateCommandDisplayData> _showCommandList;

	[CollectionObjectField(false, true, false, false, false)]
	private string _animationToLoop;

	[CollectionObjectField(false, true, false, false, false)]
	private string _animationToPlayOnce;

	[CollectionObjectField(false, true, false, false, false)]
	private string _particleToPlay;

	[CollectionObjectField(false, true, false, false, false)]
	private string _particleToLoop;

	[CollectionObjectField(false, true, false, false, false)]
	private string _particleToLoopByCombatSkill;

	[CollectionObjectField(false, true, false, false, false)]
	private string _skillPetAnimation;

	[CollectionObjectField(false, true, false, false, false)]
	private string _petParticle;

	[CollectionObjectField(false, true, false, false, false)]
	private float _animationTimeScale;

	[CollectionObjectField(false, true, false, false, false)]
	private bool _attackOutOfRange;

	[CollectionObjectField(false, true, false, false, false)]
	private string _attackSoundToPlay;

	[CollectionObjectField(false, true, false, false, false)]
	private string _skillSoundToPlay;

	[CollectionObjectField(false, true, false, false, false)]
	private string _hitSoundToPlay;

	[CollectionObjectField(false, true, false, false, false)]
	private string _armorHitSoundToPlay;

	[CollectionObjectField(false, true, false, false, false)]
	private string _whooshSoundToPlay;

	[CollectionObjectField(false, true, false, false, false)]
	private string _shockSoundToPlay;

	[CollectionObjectField(false, true, false, false, false)]
	private string _stepSoundToPlay;

	[CollectionObjectField(false, true, false, false, false)]
	private string _dieSoundToPlay;

	[CollectionObjectField(false, true, false, false, false)]
	private string _soundToLoop;

	[CollectionObjectField(false, false, true, false, false)]
	private SilenceData _silenceData = new SilenceData();

	public readonly List<ShowSpecialEffectDisplayData> NeedShowEffectList = new List<ShowSpecialEffectDisplayData>();

	public readonly List<TeammateCommandDisplayData> NeedShowCommandList = new List<TeammateCommandDisplayData>();

	public string SpecialAnimationLoop;

	public bool NeedSelectMercyOption;

	public bool NeedDelaySettlement;

	public bool NeedEnterSpecialShow = false;

	[CollectionObjectField(false, true, false, false, false)]
	private sbyte _bossPhase;

	public bool NeedChangeBossPhase;

	public int ChangeBossPhaseEffectId;

	public bool CanCastSkillCostBreath;

	public bool CanCastSkillCostStance;

	public int PreventCastSkillEffectCount;

	public bool CanCastDirectSkill;

	public bool CanCastReverseSkill;

	public readonly List<short> CanCastDuringPrepareSkills = new List<short>();

	public readonly List<short> ForgetAfterCombatSkills = new List<short>();

	[CollectionObjectField(false, true, false, false, false)]
	private CombatReserveData _combatReserveData;

	[CollectionObjectField(false, true, false, false, false)]
	private bool _reserveNormalAttack;

	public readonly List<CastFreeData> CastFreeDataList = new List<CastFreeData>();

	public bool CanFleeOutOfRange;

	[CollectionObjectField(false, true, false, false, false)]
	private CombatStateCollection _buffCombatStateCollection;

	[CollectionObjectField(false, true, false, false, false)]
	private CombatStateCollection _debuffCombatStateCollection;

	[CollectionObjectField(false, true, false, false, false)]
	private CombatStateCollection _specialCombatStateCollection;

	[CollectionObjectField(false, false, true, false, false)]
	private int _combatStateTotalBuffPower;

	public short BuffCombatStatePowerExtraLimit;

	public short DebuffCombatStatePowerExtraLimit;

	[CollectionObjectField(false, true, false, false, false)]
	private SkillEffectCollection _skillEffectCollection;

	public sbyte ChangeHitTypeEffectCount;

	public sbyte ChangeAvoidTypeEffectCount;

	[CollectionObjectField(false, true, false, false, false)]
	private int _hazardValue;

	public AiController AiController;

	[CollectionObjectField(false, true, false, false, false)]
	private byte _healInjuryCount;

	[CollectionObjectField(false, true, false, false, false)]
	private byte _healPoisonCount;

	[CollectionObjectField(false, true, false, false, false)]
	private sbyte _animalAttackCount;

	[CollectionObjectField(false, true, false, false, false, ArrayElementsCount = 5)]
	private bool[] _otherActionCanUse;

	[CollectionObjectField(false, true, false, false, false)]
	private sbyte _preparingOtherAction;

	[CollectionObjectField(false, true, false, false, false)]
	private byte _otherActionPreparePercent;

	[CollectionObjectField(false, true, false, false, false)]
	private bool _canSurrender;

	[CollectionObjectField(false, true, false, false, false)]
	private bool _canUseItem;

	[CollectionObjectField(false, true, false, false, false)]
	private ItemKey _preparingItem;

	[CollectionObjectField(false, true, false, false, false)]
	private byte _useItemPreparePercent;

	public ItemKey UsingItem;

	public sbyte ItemUseType;

	public List<sbyte> ItemTargetBodyParts;

	public ItemKey NeedRepairItem;

	public ItemKey RepairingItem;

	public bool NeedInterruptSurrender;

	public bool NeedAnimalAttack;

	public int ChangeCharId;

	public string ChangeCharFailAni;

	public string ChangeCharFailParticle;

	public string ChangeCharFailSound;

	public readonly bool[] TeammateHasCommand = new bool[3];

	public int TeammateBeforeMainChar;

	public int TeammateAfterMainChar;

	public CombatCharacter ActingTeammateCommandChar;

	[CollectionObjectField(false, true, false, false, false)]
	private bool _showTransferInjuryCommand;

	public readonly int[] TeammateCommandCdTotalCount = new int[3];

	public readonly int[] TeammateCommandCdCurrentCount = new int[3];

	public int TeammateCommandCdSpeed;

	public int StopCommandEffectCount;

	public bool TransferInjuryCommandIsInner;

	[CollectionObjectField(false, true, false, false, false)]
	private List<sbyte> _currTeammateCommands;

	[CollectionObjectField(false, true, false, false, false)]
	private List<SByteList> _teammateCommandBanReasons;

	[CollectionObjectField(false, false, true, false, false)]
	private readonly List<bool> _teammateCommandCanUse = new List<bool>();

	[CollectionObjectField(false, true, false, false, false)]
	private List<byte> _teammateCommandCdPercent;

	[CollectionObjectField(false, true, false, false, false)]
	private sbyte _executingTeammateCommand;

	public bool NeedResetAdvanceTeammateCommandPushCd;

	public bool NeedResetAdvanceTeammateCommandPullCd;

	public long ExecutingTeammateCommandSpecialEffect;

	public int ExecutingTeammateCommandIndex;

	public int ExecutingTeammateCommandChangeDistance;

	public TeammateCommandItem ExecutingTeammateCommandConfig;

	[CollectionObjectField(false, true, false, false, false)]
	private bool _visible;

	public short TeammateCommandLeftPrepareFrame;

	public short TeammateCommandTotalPrepareFrame;

	[CollectionObjectField(false, true, false, false, false)]
	private byte _teammateCommandPreparePercent;

	public short TeammateCommandLeftFrame;

	public short TeammateCommandTotalFrame;

	[CollectionObjectField(false, true, false, false, false)]
	private byte _teammateCommandTimePercent;

	private short _teammateExitAniLeftFrame;

	[CollectionObjectField(false, true, false, false, false)]
	private ItemKey _attackCommandWeaponKey;

	[CollectionObjectField(false, true, false, false, false)]
	private sbyte _attackCommandTrickType;

	[CollectionObjectField(false, true, false, false, false)]
	private short _attackCommandSkillId;

	[CollectionObjectField(false, true, false, false, false)]
	private short _defendCommandSkillId;

	[CollectionObjectField(false, true, false, false, false)]
	private sbyte _showEffectCommandIndex;

	private readonly List<ITeammateCommandInvoker> _teammateCommandInvokers = new List<ITeammateCommandInvoker>();

	public bool CanRecoverMobility;

	public sbyte AttackForceMissCount;

	public sbyte AttackForceHitCount;

	public bool SkillForceHit;

	public bool OuterInjuryImmunity;

	public bool InnerInjuryImmunity;

	public bool MindImmunity;

	public bool FlawImmunity;

	public bool AcupointImmunity;

	public bool SkipOnFrameBegin;

	[CollectionObjectField(false, false, true, false, false)]
	private OuterAndInnerShorts _attackRange;

	[CollectionObjectField(false, false, true, false, false)]
	private sbyte _happiness;

	private readonly string[] WinAni = new string[2] { "C_017_female", "C_017" };

	private readonly string[] WinAniLoop = new string[2] { "C_018_female", "C_018" };

	private readonly List<DataUid> _markDataUids = new List<DataUid>();

	private static readonly Dictionary<ETeammateCommandImplement, Type> TeammateCommandEffects = new Dictionary<ETeammateCommandImplement, Type>
	{
		{
			ETeammateCommandImplement.GearMateC,
			typeof(GearMateC)
		},
		{
			ETeammateCommandImplement.VitalDemonA,
			typeof(VitalDemonA)
		},
		{
			ETeammateCommandImplement.VitalDemonB,
			typeof(VitalDemonB)
		},
		{
			ETeammateCommandImplement.VitalDemonC,
			typeof(VitalDemonC)
		}
	};

	private const int DirectCostDurability = 4;

	private const int ReverseCostDurability = 8;

	private const byte DirectCostJiTrick = 2;

	private const byte ReverseCostUsableTrick = 3;

	private readonly List<IExtraUnlockEffect> _invokedUnlockEffects = new List<IExtraUnlockEffect>();

	private readonly List<IExtraUnlockEffect> _costedUnlockEffects = new List<IExtraUnlockEffect>();

	public const int FixedSize = 646;

	public const int DynamicCount = 41;

	public BossItem BossConfig { get; private set; }

	public AnimalItem AnimalConfig { get; private set; }

	public bool IsActorSkeleton => BossConfig == null && AnimalConfig == null;

	public int MaxChangeTrickCount => DomainManager.SpecialEffect.ModifyValue(_id, 301, 12);

	public bool ChangeToMindMark => DomainManager.SpecialEffect.ModifyData(_id, -1, 288, dataValue: false);

	public bool UnyieldingFallen => DomainManager.SpecialEffect.ModifyData(_id, -1, 282, dataValue: false);

	public bool IsAnimal => (IsAlly && _id == DomainManager.Combat.GetCarrierAnimalCombatCharId()) || AnimalConfig != null;

	public bool IsMoving => MoveData.MoveCd > 0;

	public bool IsJumping => KeepMoving && (_jumpPrepareProgress > 0 || _jumpPreparedDistance > 0);

	public bool IsUnlockAttack => UnlockWeaponIndex >= 0;

	public GameData.Domains.Item.Weapon UnlockWeapon => DomainManager.Item.GetElement_Weapons(_weapons[UnlockWeaponIndex].Id);

	public int UnlockEffectId => Config.Weapon.Instance[UnlockWeapon.GetTemplateId()].UnlockEffect;

	public WeaponUnlockEffectItem UnlockEffect => WeaponUnlockEffect.Instance[UnlockEffectId];

	public short NeedUseSkillFreeId
	{
		get
		{
			int result;
			if (CastFreeDataList.Count <= 0)
			{
				result = -1;
			}
			else
			{
				List<CastFreeData> castFreeDataList = CastFreeDataList;
				result = castFreeDataList[castFreeDataList.Count - 1].SkillId;
			}
			return (short)result;
		}
	}

	public bool NeedChangeSkill => NeedUseSkillId >= 0 && (_preparingSkillId < 0 || CanCastDuringPrepareSkills.Contains(NeedUseSkillId));

	public short NeedUseSkillId => (NeedUseSkillFreeId >= 0) ? NeedUseSkillFreeId : _combatReserveData.NeedUseSkillId;

	public bool NeedShowChangeTrick => _combatReserveData.NeedShowChangeTrick && (_preparingSkillId < 0 || CanNormalAttackInPrepareSkill);

	public int NeedChangeWeaponIndex => _combatReserveData.NeedChangeWeaponIndex;

	public ItemKey NeedUseItem => _combatReserveData.NeedUseItem;

	public sbyte NeedUseOtherAction => _combatReserveData.NeedUseOtherAction;

	public OtherActionTypeItem PreparingOtherActionTypeConfig => (_preparingOtherAction < 0) ? null : Config.OtherActionType.Instance[_preparingOtherAction];

	public ETeammateCommandImplement ExecutingTeammateCommandImplement => (_executingTeammateCommand < 0) ? ETeammateCommandImplement.Invalid : TeammateCommand.Instance[_executingTeammateCommand].Implement;

	private string DataHandlerKey => $"CombatChar_{_id}";

	bool IAiParticipant.DisableAi => ExecutingTeammateCommandConfig?.DisableAi ?? false;

	public bool NoBlockAttack => GetChangeTrickAttack() || IsUnlockAttack;

	private WeaponItem UsingWeaponConfig => Config.Weapon.Instance[_weapons[_usingWeaponIndex].TemplateId];

	private sbyte UsingWeaponAction => UsingWeaponConfig.WeaponAction;

	private string AttackPostfix => BossConfig?.AttackEffectPostfix[_usingWeaponIndex] ?? string.Empty;

	public bool NeedNormalAttack
	{
		get
		{
			if (NeedNormalAttackSkipPrepare > 0 || NeedFreeAttack || NeedChangeTrickAttack)
			{
				return true;
			}
			if (CanNormalAttackImmediate)
			{
				return GetReserveNormalAttack() || NeedNormalAttackImmediate;
			}
			NeedNormalAttackImmediate = false;
			return false;
		}
	}

	public bool CanNormalAttackImmediate
	{
		get
		{
			if (NeedNormalAttackSkipPrepare > 0 || NeedFreeAttack || NeedChangeTrickAttack)
			{
				return false;
			}
			if (_normalAttackRecovery.Silencing || IsJumping)
			{
				return false;
			}
			return (StateMachine.GetCurrentStateType() == CombatCharacterStateType.Idle && !PreparingOrDoingTeammateCommand()) || (_preparingSkillId >= 0 && CanNormalAttackInPrepareSkill);
		}
	}

	private int AttackSpeed => _character.GetAttackSpeed();

	private CombatCharacter MainChar => DomainManager.Combat.GetMainCharacter(IsAlly);

	private bool IsMainChar => DomainManager.Combat.IsMainCharacter(this);

	public bool AnyRawCreate => _rawCreateEffects.Count > 0;

	public bool AnyUsableTrick => _tricks.Tricks.Values.Any(IsTrickUsable);

	public int UsableTrickCount => _tricks.Tricks.Values.Count(IsTrickUsable);

	public int UselessTrickCount => _tricks.Tricks.Values.Count(IsTrickUseless);

	public override string ToString()
	{
		return _character.ToString();
	}

	public unsafe void Init(CombatDomain combatDomain, int characterId, DataContext context)
	{
		_id = characterId;
		_character = DomainManager.Character.GetElement_Objects(characterId);
		_combatDomain = combatDomain;
		short templateId = _character.GetTemplateId();
		bool flag = CombatDomain.CharId2BossId.ContainsKey(templateId);
		bool flag2 = SharedConstValue.CharId2AnimalId.ContainsKey(templateId);
		BossConfig = (flag ? Boss.Instance[CombatDomain.CharId2BossId[templateId]] : null);
		_bossPhase = 0;
		ChangeBossPhaseEffectId = -1;
		AnimalConfig = (flag2 ? Config.Animal.Instance[SharedConstValue.CharId2AnimalId[templateId]] : null);
		_breathValue = 30000;
		_stanceValue = 4000;
		_oldDisorderOfQi = _character.GetDisorderOfQi();
		_neiliType = (OriginNeiliType = _character.GetNeiliType());
		_avoidToShow.HitType = -1;
		_currentPosition = (short)(combatDomain.GetCurrentDistance() / 2 * ((!IsAlly) ? 1 : (-1)));
		_displayPosition = int.MinValue;
		_mobilityValue = MoveSpecialConstants.MaxMobility;
		_mobilityLevel = 2;
		_targetDistance = -1;
		_mobilityLockEffectCount = 0;
		_jumpChangeDistanceDuration = -1f;
		KeepMoving = false;
		PlayerControllingMove = false;
		AiTargetDistance = -1;
		PlayerTargetDistance = -1;
		PlayerChangeTrickType = (PlayerChangeTrickBodyPart = -1);
		MoveData.Init(context, this);
		NeedPauseJumpMove = false;
		ItemKey[] equipment = _character.GetEquipment();
		sbyte[] array = EquipmentSlot.EquipmentType2Slots[0];
		if (flag && BossConfig.PhaseWeapons != null)
		{
			short[] array2 = BossConfig.PhaseWeapons[0];
			for (int i = 0; i < array.Length; i++)
			{
				if (equipment[array[i]].IsValid())
				{
					_character.ChangeEquipment(context, array[i], -1, ItemKey.Invalid);
				}
			}
			for (int j = 0; j < array2.Length; j++)
			{
				ItemKey itemKey = DomainManager.Item.CreateWeapon(context, array2[j], 0);
				_character.AddInventoryItem(context, itemKey, 1);
				_character.ChangeEquipment(context, -1, array[j], itemKey);
			}
		}
		for (int k = 0; k < array.Length; k++)
		{
			ItemKey itemKey2 = equipment[array[k]];
			_weapons[k] = itemKey2;
			if (itemKey2.IsValid())
			{
				DomainManager.SpecialEffect.AddEquipmentEffect(context, _id, itemKey2);
			}
		}
		bool allowUseFreeWeapon = _character.GetAllowUseFreeWeapon();
		_weapons[3] = (allowUseFreeWeapon ? DomainManager.Item.CreateWeapon(context, 0, 0) : ItemKey.Invalid);
		_weapons[4] = (allowUseFreeWeapon ? DomainManager.Item.CreateWeapon(context, 1, 0) : ItemKey.Invalid);
		_weapons[5] = (allowUseFreeWeapon ? DomainManager.Item.CreateWeapon(context, 2, 0) : ItemKey.Invalid);
		_weapons[6] = (allowUseFreeWeapon ? DomainManager.Item.CreateWeapon(context, 884, 0) : ItemKey.Invalid);
		for (sbyte b = 0; b < 7; b++)
		{
			ItemKey itemKey3 = equipment[EquipmentSlotHelper.GetSlotByBodyPartType(b)];
			Armors[b] = ((itemKey3.IsValid() && DomainManager.Item.GetElement_Armors(itemKey3.Id).GetCurrDurability() > 0) ? itemKey3 : ItemKey.Invalid);
			if (itemKey3.IsValid())
			{
				DomainManager.SpecialEffect.AddEquipmentEffect(context, _id, itemKey3);
			}
		}
		_usingWeaponIndex = -1;
		_weaponTrickIndex = 0;
		_changeTrickProgress = 0;
		_changeTrickCount = 0;
		_canChangeTrick = false;
		_attackingTrickType = -1;
		ForbidNormalAttackEffectCount = 0;
		CanNormalAttackInPrepareSkill = false;
		NeedBreakAttack = false;
		IsBreakAttacking = false;
		NeedNormalAttackImmediate = false;
		NeedNormalAttackSkipPrepare = 0;
		NormalAttackBodyPart = -1;
		NormalAttackHitType = -1;
		PursueAttackCount = 0;
		NormalAttackLeftRepeatTimes = 0;
		ChangeTrickType = -1;
		ChangeTrickBodyPart = -1;
		NeedChangeTrickAttack = false;
		UnlockWeaponIndex = -1;
		FightBackHitType = -1;
		IsAutoNormalAttackingSpecial = false;
		NeedReduceWeaponDurability = false;
		NeedReduceArmorDurability = false;
		_changingTrick = false;
		_changeTrickAttack = false;
		_unlockPrepareValue.Clear();
		for (int l = 0; l < 3; l++)
		{
			_unlockPrepareValue.Add(0);
		}
		_tricks.ClearTricks();
		_maxTrickCount = 0;
		_defeatMarkCollection = new DefeatMarkCollection();
		Immortal = !Config.Character.Instance[_character.GetTemplateId()].CanDefeat;
		_defeatMarkUid = new DataUid(8, 10, (ulong)_id, 50u);
		GameData.GameDataBridge.GameDataBridge.AddPostDataModificationHandler(_defeatMarkUid, DataHandlerKey, OnDefeatMarkChanged);
		RegisterMarkHandler();
		_injuries = (_oldInjuries = _character.GetInjuries());
		_injuryAutoHealCollection = new InjuryAutoHealCollection();
		_oldInjuryAutoHealCollection = new InjuryAutoHealCollection();
		_damageStepCollection = combatDomain.GetDamageStepCollection(_id);
		for (sbyte b2 = 0; b2 < 7; b2++)
		{
			_flawCount[b2] = 0;
			_acupointCount[b2] = 0;
		}
		_flawCollection = new FlawOrAcupointCollection();
		_acupointCollection = new FlawOrAcupointCollection();
		_mindMarkTime = new MindMarkList();
		for (sbyte b3 = 0; b3 < 7; b3++)
		{
			_outerDamageValueToShow[b3] = new IntPair(-1, -1);
			_innerDamageValueToShow[b3] = new IntPair(-1, -1);
		}
		_mindDamageValueToShow = -1;
		_fatalDamageValueToShow = -1;
		_poison = (_oldPoison = _character.GetPoisoned());
		_poisonResist = _character.GetPoisonResists();
		Array.Clear(_poisonAffectAccumulator, 0, 6);
		_poisonResistUid = new DataUid(4, 0, (ulong)_id, 94u);
		GameData.GameDataBridge.GameDataBridge.AddPostDataModificationHandler(_poisonResistUid, DataHandlerKey, OnPoisonResistChanged);
		_mixPoisonAffectedCount.Clear();
		_neiliAllocation = default(NeiliAllocation);
		_originNeiliAllocation = default(NeiliAllocation);
		_originBaseNeiliAllocation = default(NeiliAllocation);
		NeiliAllocation neiliAllocation = _character.GetNeiliAllocation();
		NeiliAllocation baseNeiliAllocation = _character.GetBaseNeiliAllocation();
		for (int m = 0; m < 4; m++)
		{
			_neiliAllocation.Items[m] = neiliAllocation.Items[m];
			_originNeiliAllocation.Items[m] = neiliAllocation.Items[m];
			_originBaseNeiliAllocation.Items[m] = baseNeiliAllocation.Items[m];
			_neiliAllocationRecoverProgress.Items[m] = 0;
			NeiliAllocationAutoRecoverProgress[m] = 0;
		}
		OriginXiangshuInfection = _character.GetXiangshuInfection();
		InitSkillList(0, _neigongList);
		InitSkillList(1, _attackSkillList, BossConfig);
		InitSkillList(2, _agileSkillList);
		InitSkillList(3, _defenceSkillList);
		InitSkillList(4, _assistSkillList);
		EnableEnterCombatSkillEffect(context, _neigongList);
		EnableEnterCombatSkillEffect(context, _attackSkillList);
		EnableEnterCombatSkillEffect(context, _agileSkillList);
		EnableEnterCombatSkillEffect(context, _defenceSkillList);
		EnableEnterCombatSkillEffect(context, _assistSkillList);
		CanCastSkillCostBreath = true;
		CanCastSkillCostStance = true;
		_preparingSkillId = -1;
		_skillPreparePercent = 0;
		_performingSkillId = -1;
		_attackSkillPower = 0;
		_affectingMoveSkillId = -1;
		_affectingDefendSkillId = -1;
		_defendSkillTimePercent = 0;
		CastFreeDataList.Clear();
		NeedAddEffectAgileSkillId = -1;
		DefendSkillTotalFrame = 0;
		DefendSkillLeftFrame = 0;
		PreventCastSkillEffectCount = 0;
		CanCastDirectSkill = true;
		CanCastReverseSkill = true;
		_wugCount = 0;
		CombatResources usableCombatResources = DomainManager.Character.GetUsableCombatResources(_id);
		sbyte healingCount = usableCombatResources.HealingCount;
		bool allowHeal = Config.Character.Instance[_character.GetTemplateId()].AllowHeal;
		_healInjuryCount = (byte)((healingCount > 0 && !flag2 && allowHeal) ? healingCount : 0);
		sbyte detoxCount = usableCombatResources.DetoxCount;
		_healPoisonCount = (byte)((detoxCount > 0 && !flag2 && allowHeal) ? detoxCount : 0);
		_preparingOtherAction = -1;
		_otherActionPreparePercent = 0;
		UsingItem = ItemKey.Invalid;
		_preparingItem = ItemKey.Invalid;
		_useItemPreparePercent = 0;
		BuffCombatStatePowerExtraLimit = 0;
		DebuffCombatStatePowerExtraLimit = 0;
		_xiangshuEffectId = -1;
		_hazardValue = 0;
		NeedSelectMercyOption = false;
		NeedDelaySettlement = false;
		ChangeHitTypeEffectCount = 0;
		ChangeAvoidTypeEffectCount = 0;
		SpecialAnimationLoop = null;
		_animationToLoop = null;
		_animationToPlayOnce = null;
		_particleToPlay = null;
		_skillPetAnimation = null;
		_petParticle = null;
		_animationTimeScale = 1f;
		if (IsAlly && _combatDomain.IsMainCharacter(this))
		{
			HunterSkillsData hunterSkillsData = (HunterSkillsData)DomainManager.Extra.GetProfessionData(1).SkillsData;
			_animalAttackCount = (sbyte)((hunterSkillsData != null) ? Math.Max(3 - hunterSkillsData.UsedCarrierAnimalAttackCount, 0) : 0);
		}
		else
		{
			_animalAttackCount = 0;
		}
		NeedAnimalAttack = false;
		ChangeCharId = -1;
		ChangeCharFailAni = null;
		Array.Clear(TeammateHasCommand, 0, TeammateHasCommand.Length);
		TeammateBeforeMainChar = -1;
		TeammateAfterMainChar = -1;
		ActingTeammateCommandChar = null;
		_showTransferInjuryCommand = false;
		_executingTeammateCommand = -1;
		NeedResetAdvanceTeammateCommandPushCd = false;
		NeedResetAdvanceTeammateCommandPullCd = false;
		ExecutingTeammateCommandSpecialEffect = -1L;
		ExecutingTeammateCommandIndex = -1;
		ExecutingTeammateCommandChangeDistance = 0;
		ExecutingTeammateCommandConfig = null;
		_visible = false;
		TeammateCommandLeftPrepareFrame = 0;
		_teammateCommandPreparePercent = 0;
		TeammateCommandLeftFrame = -1;
		_teammateCommandTimePercent = 0;
		_teammateExitAniLeftFrame = 0;
		_attackCommandWeaponKey = ItemKey.Invalid;
		_attackingTrickType = -1;
		_attackCommandSkillId = -1;
		_defendCommandSkillId = -1;
		_showEffectCommandIndex = -1;
		CanRecoverMobility = true;
		AttackForceMissCount = 0;
		AttackForceHitCount = 0;
		SkillForceHit = false;
		OuterInjuryImmunity = false;
		InnerInjuryImmunity = false;
		MindImmunity = false;
		FlawImmunity = false;
		AcupointImmunity = false;
		_combatReserveData = CombatReserveData.Invalid;
		_reserveNormalAttack = false;
		StateMachine.Init(combatDomain, this);
		StateMachine.TranslateState(CombatCharacterStateType.Idle);
	}

	private void InitSkillList(sbyte equipType, ICollection<short> skillList, BossItem bossConfig = null)
	{
		skillList.Clear();
		if (bossConfig == null)
		{
			ArraySegmentList<short>.Enumerator enumerator = _character.GetCombatSkillEquipment()[equipType].GetEnumerator();
			while (enumerator.MoveNext())
			{
				short current = enumerator.Current;
				if (current >= 0 && _character.GetCombatSkillCanAffect(current))
				{
					skillList.Add(current);
				}
			}
		}
		else
		{
			short[] array = bossConfig.PhaseAttackSkills[0];
			foreach (short item in array)
			{
				skillList.Add(item);
			}
		}
	}

	private void EnableEnterCombatSkillEffect(DataContext context, IEnumerable<short> skillListInCombat)
	{
		if (!_combatDomain.IsTeamCharacter(_id))
		{
			return;
		}
		foreach (short item in skillListInCombat)
		{
			DomainManager.SpecialEffect.Add(context, _id, item, 1, -1);
		}
	}

	public void OnFrameBegin()
	{
		DataContext dataContext = GetDataContext();
		if (NeedAddEffectAgileSkillId >= 0)
		{
			DomainManager.SpecialEffect.Add(dataContext, _id, NeedAddEffectAgileSkillId, 0, -1);
			NeedAddEffectAgileSkillId = -1;
		}
		if (SkipOnFrameBegin)
		{
			SkipOnFrameBegin = false;
			return;
		}
		if (_showEffectList.ShowEffectList.Count > 0)
		{
			_showEffectList.ShowEffectList.Clear();
		}
		if (NeedShowEffectList.Count > 0)
		{
			_showEffectList.ShowEffectList.AddRange(NeedShowEffectList);
			NeedShowEffectList.Clear();
		}
		if (_showCommandList.Count > 0)
		{
			_showCommandList.Clear();
		}
		if (NeedShowCommandList.Count > 0)
		{
			_showCommandList.AddRange(NeedShowCommandList);
			NeedShowCommandList.Clear();
		}
		for (sbyte b = 0; b < 7; b++)
		{
			_outerDamageValueToShow[b].First = -1;
			_outerDamageValueToShow[b].Second = -1;
			_innerDamageValueToShow[b].First = -1;
			_innerDamageValueToShow[b].Second = -1;
		}
		SetOuterDamageValueToShow(_outerDamageValueToShow, dataContext);
		SetInnerDamageValueToShow(_innerDamageValueToShow, dataContext);
		SetMindDamageValueToShow(-1, dataContext);
		SetFatalDamageValueToShow(-1, dataContext);
		if (_newPoisonsToShow.IsNonZero())
		{
			_newPoisonsToShow.Initialize();
			SetNewPoisonsToShow(ref _newPoisonsToShow, dataContext);
		}
	}

	public void OnFrameEnd()
	{
		if (_showEffectList.ShowEffectList.Count > 0)
		{
			SetShowEffectList(_showEffectList, GetDataContext());
		}
		if (_showCommandList.Count > 0)
		{
			SetShowCommandList(_showCommandList, GetDataContext());
		}
	}

	public GameData.Domains.Character.Character GetCharacter()
	{
		return _character;
	}

	public void OnCombatEnd(DataContext context)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_018d: Unknown result type (might be due to invalid IL or missing references)
		CValuePercent val = CValuePercent.op_Implicit(_combatDomain.CombatConfig.StayPercent);
		bool flag = _character.GetCreatingType() == 1 || _combatDomain.CombatConfig.AffectTemporaryCharacter || DomainManager.Taiwu.IsInGroup(_character.GetId());
		bool flag2 = flag && (IsAlly || !_combatDomain.CombatConfig.EnemyHealDamage);
		if (flag2)
		{
			for (sbyte b = 0; b < 7; b++)
			{
				(sbyte, sbyte) tuple = _injuries.Get(b);
				(sbyte, sbyte) tuple2 = _oldInjuries.Get(b);
				int num = tuple.Item1 - tuple2.Item1;
				int num2 = tuple.Item2 - tuple2.Item2;
				_injuries.Set(b, isInnerInjury: false, (sbyte)(tuple2.Item1 + num * val));
				_injuries.Set(b, isInnerInjury: true, (sbyte)(tuple2.Item2 + num2 * val));
			}
		}
		else
		{
			_injuries.Initialize();
		}
		_character.SetInjuries(_injuries, context);
		if (flag2)
		{
			for (sbyte b2 = 0; b2 < 6; b2++)
			{
				int num3 = _poison[b2] - _oldPoison[b2];
				_poison[b2] = Math.Clamp(_oldPoison[b2] + num3 * val, 0, 25000);
			}
		}
		else
		{
			_poison.Initialize();
		}
		_character.SetPoisoned(ref _poison, context);
		if (flag2)
		{
			int num4 = _character.GetDisorderOfQi() - _oldDisorderOfQi;
			int num5 = Math.Clamp(_oldDisorderOfQi + num4 * val, 0, DisorderLevelOfQi.MaxValue);
			_character.SetDisorderOfQi((short)num5, context);
		}
		else
		{
			_character.SetDisorderOfQi(0, context);
		}
		if (flag)
		{
			sbyte b3 = GlobalConfig.Instance.ReduceHealthPerFatalDamageMark[Math.Clamp(DomainManager.Combat.GetCombatType(), 0, GlobalConfig.Instance.ReduceHealthPerFatalDamageMark.Length - 1)];
			if (IsAlly ? _combatDomain.CombatConfig.SelfFatalDamageReduceHealth : _combatDomain.CombatConfig.EnemyFatalDamageReduceHealth)
			{
				_character.ChangeHealth(context, -b3 * _defeatMarkCollection.FatalDamageMarkCount);
			}
		}
		else if (!DomainManager.Combat.IsCharInLoot(_id))
		{
			_character.ClearEatingItems(context);
		}
		List<short> learnedCombatSkills = _character.GetLearnedCombatSkills();
		for (int i = 0; i < ForgetAfterCombatSkills.Count; i++)
		{
			short num6 = ForgetAfterCombatSkills[i];
			learnedCombatSkills.Remove(num6);
			DomainManager.CombatSkill.RemoveCombatSkill(_id, num6);
		}
		_character.SetLearnedCombatSkills(learnedCombatSkills, context);
		foreach (ITeammateCommandInvoker teammateCommandInvoker in _teammateCommandInvokers)
		{
			teammateCommandInvoker.Close();
		}
		_teammateCommandInvokers.Clear();
		if (flag)
		{
			NeiliAllocation extraNeiliAllocation = _character.GetExtraNeiliAllocation();
			int currNeili = _character.GetCurrNeili();
			for (int j = 0; j < 4; j++)
			{
				_neiliAllocation[j] = (short)Math.Clamp(_neiliAllocation[j] - extraNeiliAllocation[j], 0, _originBaseNeiliAllocation[j]);
			}
			_character.SpecifyBaseNeiliAllocation(context, _neiliAllocation);
			_character.SpecifyCurrNeili(context, currNeili);
			if (IsAlly && _id == DomainManager.Taiwu.GetTaiwuCharId())
			{
				DomainManager.Taiwu.UpdateTaiwuNeiliAllocation(context, isInCombat: true);
			}
		}
		else
		{
			_character.SpecifyBaseNeiliAllocation(context, _originBaseNeiliAllocation);
		}
		AiController?.UnInit();
		GameData.GameDataBridge.GameDataBridge.RemovePostDataModificationHandler(_poisonResistUid, DataHandlerKey);
		GameData.GameDataBridge.GameDataBridge.RemovePostDataModificationHandler(_defeatMarkUid, DataHandlerKey);
		UnRegisterMarkHandler();
		if (IsTaiwu && DomainManager.Combat.AiOptions.SaveMoveTarget)
		{
			DomainManager.Extra.SetLastTargetDistance(PlayerTargetDistance, context);
		}
		EventArgBox globalEventArgumentBox = DomainManager.TaiwuEvent.GetGlobalEventArgumentBox();
		globalEventArgumentBox.Set("IsGuardCombat", arg: false);
	}

	public void RemoveTempWeapons(DataContext context)
	{
		DomainManager.Item.RemoveItem(context, _weapons[3]);
		DomainManager.Item.RemoveItem(context, _weapons[4]);
		DomainManager.Item.RemoveItem(context, _weapons[5]);
		DomainManager.Item.RemoveItem(context, _weapons[6]);
	}

	public void OfflineChangeInjuries(sbyte bodyPart, bool isInner, sbyte delta)
	{
		_injuries.Change(bodyPart, isInner, delta);
	}

	public DataContext GetDataContext()
	{
		return _combatDomain.Context;
	}

	[ObjectCollectionDependency(8, 10, new ushort[] { 16, 18, 56, 58, 26, 44 }, Scope = InfluenceScope.Self)]
	[SingleValueDependency(8, new ushort[] { 19 }, Scope = InfluenceScope.AllCombatCharsInCombat)]
	[ObjectCollectionDependency(17, 2, new ushort[] { 145, 146, 273 }, Scope = InfluenceScope.CombatCharacterAffectedByTheSpecialEffects)]
	private OuterAndInnerShorts CalcAttackRange()
	{
		return CalcAttackRangeImmediate(-1);
	}

	[ObjectCollectionDependency(17, 2, new ushort[] { 52 }, Scope = InfluenceScope.CombatCharacterAffectedByTheSpecialEffects)]
	private sbyte CalcHappiness()
	{
		int happiness = _character.GetHappiness();
		happiness += DomainManager.SpecialEffect.GetModifyValue(_id, 52, (EDataModifyType)0, -1, -1, -1, (EDataSumType)0);
		happiness = Math.Clamp(happiness, -119, 119);
		return (sbyte)happiness;
	}

	[ObjectCollectionDependency(8, 10, new ushort[] { 132, 15 }, Scope = InfluenceScope.Self)]
	[ObjectCollectionDependency(4, 0, new ushort[] { 86 }, Scope = InfluenceScope.CombatCharOfTheChar)]
	private float CalcChangeDistanceDuration()
	{
		if (GetJumpChangeDistanceDuration() >= 0f)
		{
			return GetJumpChangeDistanceDuration();
		}
		short moveCd = GetMoveCd();
		return (float)moveCd / 60f;
	}

	[ObjectCollectionDependency(8, 10, new ushort[] { 114 }, Scope = InfluenceScope.Self)]
	private void CalcTeammateCommandCanUse(List<bool> teammateCommandCanUse)
	{
		for (int i = 0; i < _teammateCommandBanReasons.Count; i++)
		{
			teammateCommandCanUse[i] = _teammateCommandBanReasons[i].Items.Count == 0;
		}
	}

	[ObjectCollectionDependency(8, 10, new ushort[] { 11 }, Scope = InfluenceScope.Self)]
	private byte CalcMobilityLevel()
	{
		return CFormula.CalcMobilityLevel(_mobilityValue);
	}

	[ObjectCollectionDependency(8, 29, new ushort[] { 2, 3 }, Scope = InfluenceScope.CombatCharOfTheCombatSkillData)]
	[ObjectCollectionDependency(8, 30, new ushort[] { 7, 8 }, Scope = InfluenceScope.CombatCharOfTheCombatWeaponData)]
	private void CalcSilenceData(SilenceData silenceData)
	{
		SilenceData silenceData2 = silenceData;
		if (silenceData2.CombatSkill == null)
		{
			silenceData2.CombatSkill = new Dictionary<short, SilenceFrameData>();
		}
		silenceData.CombatSkill.Clear();
		foreach (CombatSkillKey combatSkillKey in GetCombatSkillKeys())
		{
			if (DomainManager.Combat.TryGetCombatSkillData(_id, combatSkillKey.SkillTemplateId, out var combatSkillData) && combatSkillData.GetLeftCdFrame() != 0)
			{
				SilenceFrameData value = SilenceFrameData.Create(combatSkillData.GetTotalCdFrame(), combatSkillData.GetLeftCdFrame());
				silenceData.CombatSkill[combatSkillKey.SkillTemplateId] = value;
			}
		}
		silenceData2 = silenceData;
		if (silenceData2.WeaponKeys == null)
		{
			silenceData2.WeaponKeys = new List<ItemKey>();
		}
		silenceData.WeaponKeys.Clear();
		silenceData2 = silenceData;
		if (silenceData2.WeaponFrames == null)
		{
			silenceData2.WeaponFrames = new List<SilenceFrameData>();
		}
		silenceData.WeaponFrames.Clear();
		ItemKey[] weapons = _weapons;
		for (int i = 0; i < weapons.Length; i++)
		{
			ItemKey item = weapons[i];
			if (DomainManager.Combat.TryGetElement_WeaponDataDict(item.Id, out var element) && element.GetFixedCdLeftFrame() != 0)
			{
				SilenceFrameData item2 = SilenceFrameData.Create(element.GetFixedCdTotalFrame(), element.GetFixedCdLeftFrame());
				silenceData.WeaponKeys.Add(item);
				silenceData.WeaponFrames.Add(item2);
			}
		}
	}

	[ObjectCollectionDependency(8, 10, new ushort[] { 76, 77 }, Scope = InfluenceScope.Self)]
	private int CalcCombatStateTotalBuffPower()
	{
		int num = _buffCombatStateCollection.StateDict.Values.Select(PowerSelector).Sum();
		int num2 = _debuffCombatStateCollection.StateDict.Values.Select(PowerSelector).Sum();
		return num - num2;
	}

	private static int PowerSelector((short power, bool reverse, int srcCharId) tuple)
	{
		return tuple.power;
	}

	[ObjectCollectionDependency(8, 10, new ushort[] { 29 }, Scope = InfluenceScope.Self)]
	[ObjectCollectionDependency(17, 2, new ushort[] { 168, 169 }, Scope = InfluenceScope.CombatCharacterAffectedByTheSpecialEffects)]
	private HeavyOrBreakInjuryData CalcHeavyOrBreakInjuryData()
	{
		HeavyOrBreakInjuryData result = default(HeavyOrBreakInjuryData);
		result.Initialize();
		for (sbyte b = 0; b < 7; b++)
		{
			if (DomainManager.Combat.CheckBodyPartInjury(this, b))
			{
				result[b] = EHeavyOrBreakType.Break;
			}
			else if (DomainManager.Combat.CheckBodyPartInjury(this, b, checkHeavyInjury: true))
			{
				result[b] = EHeavyOrBreakType.Heavy;
			}
		}
		return result;
	}

	[ObjectCollectionDependency(8, 10, new ushort[] { 44, 62 }, Scope = InfluenceScope.Self)]
	[ObjectCollectionDependency(4, 0, new ushort[] { 86 }, Scope = InfluenceScope.CombatCharOfTheChar)]
	private short CalcMoveCd()
	{
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		short moveSpeed = _character.GetMoveSpeed();
		int num = CFormula.CalcMoveCd(moveSpeed);
		if (_affectingMoveSkillId >= 0)
		{
			num *= CValuePercentBonus.op_Implicit((int)Config.CombatSkill.Instance[_affectingMoveSkillId].MoveCdBonus);
		}
		int num2 = _acupointCollection.CalcAcupointParam(5) + _acupointCollection.CalcAcupointParam(6);
		num += num * num2 / 100;
		return (short)num;
	}

	[ObjectCollectionDependency(17, 2, new ushort[] { 197 }, Scope = InfluenceScope.CombatCharacterAffectedByTheSpecialEffects)]
	private int CalcMobilityRecoverSpeed()
	{
		return DomainManager.SpecialEffect.ModifyValue(_id, 197, MoveSpecialConstants.MobilityRecoverSpeed);
	}

	[ObjectCollectionDependency(8, 10, new ushort[] { 124 }, Scope = InfluenceScope.Self)]
	[SingleValueDependency(8, new ushort[] { 4 }, Scope = InfluenceScope.AllCombatCharsInCombat)]
	[ObjectCollectionDependency(17, 2, new ushort[] { 145, 146, 273 }, Scope = InfluenceScope.CombatCharacterAffectedByTheSpecialEffects)]
	private void CalcCanUnlockAttack(List<bool> canUnlockAttack)
	{
		int needUnlockWeaponIndex = _combatReserveData.NeedUnlockWeaponIndex;
		canUnlockAttack.Clear();
		for (int i = 0; i < _unlockPrepareValue.Count; i++)
		{
			canUnlockAttack.Add(CalcCanUnlockAttackByWeaponIndex(i));
			if (needUnlockWeaponIndex == i && !canUnlockAttack[i])
			{
				SetCombatReserveData(CombatReserveData.Invalid, _combatDomain.Context);
			}
		}
	}

	private bool CalcCanUnlockAttackByWeaponIndex(int weaponIndex)
	{
		if (!_weapons[weaponIndex].IsValid())
		{
			return false;
		}
		if (_unlockPrepareValue[weaponIndex] < GlobalConfig.Instance.UnlockAttackUnit)
		{
			return false;
		}
		WeaponUnlockEffectItem unlockEffect = GetUnlockEffect(weaponIndex);
		if (unlockEffect == null)
		{
			return false;
		}
		if (unlockEffect.IgnoreAttackRange)
		{
			return true;
		}
		CalcAttackRangeImmediate(-1, weaponIndex).Deconstruct(out var outer, out var inner);
		short num = outer;
		short num2 = inner;
		short currentDistance = DomainManager.Combat.GetCurrentDistance();
		return num <= currentDistance && currentDistance <= num2;
	}

	[ObjectCollectionDependency(17, 2, new ushort[] { 170 }, Scope = InfluenceScope.CombatCharacterAffectedByTheSpecialEffects)]
	private int CalcMaxTrickCount()
	{
		int dataValue = 9;
		dataValue = DomainManager.SpecialEffect.ModifyData(_id, -1, 170, dataValue);
		return Math.Max(dataValue, 0);
	}

	[ObjectCollectionDependency(8, 10, new ushort[] { 144 }, Scope = InfluenceScope.Self)]
	private void CalcValidItems(List<ItemKey> validItems)
	{
		validItems.Clear();
		foreach (ItemKeyAndCount validItemAndCount in GetValidItemAndCounts())
		{
			validItems.Add(validItemAndCount.ItemKey);
		}
	}

	[ObjectCollectionDependency(4, 0, new ushort[] { 58 }, Scope = InfluenceScope.CombatCharOfTheChar)]
	[ObjectCollectionDependency(17, 2, new ushort[] { 325 }, Scope = InfluenceScope.CombatCharacterAffectedByTheSpecialEffects)]
	private void CalcValidItemAndCounts(List<ItemKeyAndCount> validItemAndCounts)
	{
		validItemAndCounts.Clear();
		if (IsAlly)
		{
			validItemAndCounts.Add(DomainManager.Extra.GetEmptyToolKey(DomainManager.Combat.Context));
		}
		foreach (var (itemKey2, count) in _character.GetInventory().Items)
		{
			if (ItemIsValid(itemKey2))
			{
				validItemAndCounts.Add(new ItemKeyAndCount(itemKey2, count));
			}
		}
		DomainManager.SpecialEffect.ModifyData(_id, -1, 325, validItemAndCounts);
	}

	private bool ItemIsValid(ItemKey itemKey)
	{
		if (itemKey.GetConsumedFeatureMedals() < 0)
		{
			return false;
		}
		if (itemKey.ItemType == 5)
		{
			return DomainManager.Extra.IsProfessionalSkillUnlockedAndEquipped(44);
		}
		if (itemKey.ItemType != 12)
		{
			return true;
		}
		MiscItem miscItem = Config.Misc.Instance[itemKey.TemplateId];
		List<short> requireCombatConfig = miscItem.RequireCombatConfig;
		return requireCombatConfig == null || requireCombatConfig.Count <= 0 || miscItem.RequireCombatConfig.Contains(DomainManager.Combat.CombatConfig.TemplateId);
	}

	private static bool SkillIdIsValid(short skillId)
	{
		return skillId >= 0;
	}

	public void SyncPoisonData(DataContext context)
	{
		_character.SetPoisoned(ref _poison, context);
	}

	public IEnumerable<short> GetCombatSkillIds()
	{
		IEnumerable<short> enumerable = Enumerable.Empty<short>();
		if (_neigongList != null)
		{
			enumerable = enumerable.Concat(_neigongList.Where(SkillIdIsValid));
		}
		if (_attackSkillList != null)
		{
			enumerable = enumerable.Concat(_attackSkillList.Where(SkillIdIsValid));
		}
		if (_agileSkillList != null)
		{
			enumerable = enumerable.Concat(_agileSkillList.Where(SkillIdIsValid));
		}
		if (_defenceSkillList != null)
		{
			enumerable = enumerable.Concat(_defenceSkillList.Where(SkillIdIsValid));
		}
		if (_assistSkillList != null)
		{
			enumerable = enumerable.Concat(_assistSkillList.Where(SkillIdIsValid));
		}
		return enumerable;
	}

	public IEnumerable<CombatSkillKey> GetCombatSkillKeys()
	{
		return GetCombatSkillIds().Select((Func<short, CombatSkillKey>)((short skillId) => (charId: _id, skillId: skillId)));
	}

	public IEnumerable<short> GetBannedSkillIds(bool requireNotInfinity = false)
	{
		foreach (CombatSkillKey key in GetCombatSkillKeys())
		{
			if (DomainManager.Combat.TryGetCombatSkillData(key.CharId, key.SkillTemplateId, out var data) && (!requireNotInfinity || data.GetLeftCdFrame() >= 0))
			{
				if (data.GetLeftCdFrame() != 0)
				{
					yield return key.SkillTemplateId;
				}
				data = null;
			}
		}
	}

	public IEnumerable<short> GetBanableSkillIds(sbyte specifyEquipType = -1, sbyte expectEquipType = -1)
	{
		foreach (CombatSkillKey key in GetCombatSkillKeys())
		{
			sbyte configEquipType = Config.CombatSkill.Instance[key.SkillTemplateId].EquipType;
			if (configEquipType != 0 && (specifyEquipType < 0 || configEquipType == specifyEquipType) && (expectEquipType < 0 || configEquipType != expectEquipType) && DomainManager.Combat.TryGetCombatSkillData(key.CharId, key.SkillTemplateId, out var data) && data.GetLeftCdFrame() >= 0)
			{
				yield return key.SkillTemplateId;
				data = null;
			}
		}
	}

	public short GetRandomBanableSkillId(IRandomSource random, Func<short, bool> predicate = null, sbyte specifyEquipType = -1)
	{
		using (IEnumerator<short> enumerator = GetRandomUnrepeatedBanableSkillIds(random, 1, predicate, specifyEquipType, -1).GetEnumerator())
		{
			if (enumerator.MoveNext())
			{
				return enumerator.Current;
			}
		}
		return -1;
	}

	public IEnumerable<short> GetRandomUnrepeatedBanableSkillIds(IRandomSource random, int maxCount, Func<short, bool> predicate = null, sbyte specifyEquipType = -1, sbyte expectEquipType = -1)
	{
		if (maxCount <= 0)
		{
			yield break;
		}
		List<short> prefer = ObjectPool<List<short>>.Instance.Get();
		List<short> normal = ObjectPool<List<short>>.Instance.Get();
		prefer.Clear();
		normal.Clear();
		foreach (short skillId in from arg in GetBanableSkillIds(specifyEquipType, expectEquipType)
			where predicate == null || predicate(arg)
			select arg)
		{
			CombatSkillData data = DomainManager.Combat.GetCombatSkillData(_id, skillId);
			if (data.GetLeftCdFrame() == 0)
			{
				prefer.Add(skillId);
			}
			else
			{
				normal.Add(skillId);
			}
		}
		foreach (short item in RandomUtils.GetRandomUnrepeated(random, maxCount, prefer, normal))
		{
			yield return item;
		}
		ObjectPool<List<short>>.Instance.Return(prefer);
		ObjectPool<List<short>>.Instance.Return(normal);
	}

	public IReadOnlyList<short> GetCombatSkillList(sbyte equipType)
	{
		if (1 == 0)
		{
		}
		List<short> result = equipType switch
		{
			0 => _neigongList, 
			1 => _attackSkillList, 
			2 => _agileSkillList, 
			3 => _defenceSkillList, 
			4 => _assistSkillList, 
			_ => throw new Exception($"Invalid skill equip type {equipType}"), 
		};
		if (1 == 0)
		{
		}
		return result;
	}

	public IEnumerable<sbyte> GetAvailableBodyParts()
	{
		for (sbyte i = 0; i < 7; i++)
		{
			if (ContainsBodyPart(i))
			{
				yield return i;
			}
		}
	}

	public bool ContainsBodyPart(sbyte bodyPart)
	{
		if (1 == 0)
		{
		}
		bool result = bodyPart >= 0 && bodyPart switch
		{
			3 => _character.GetHaveLeftArm(), 
			4 => _character.GetHaveRightArm(), 
			5 => _character.GetHaveLeftLeg(), 
			6 => _character.GetHaveRightLeg(), 
			_ => true, 
		};
		if (1 == 0)
		{
		}
		return result;
	}

	public int GetMaxBreathValue()
	{
		int num = 30000;
		int dataValue = 100;
		dataValue = DomainManager.SpecialEffect.ModifyData(_id, -1, 171, dataValue);
		return num * dataValue / 100;
	}

	public int GetMaxStanceValue()
	{
		int num = 4000;
		int dataValue = 100;
		dataValue = DomainManager.SpecialEffect.ModifyData(_id, -1, 172, dataValue);
		return num * dataValue / 100;
	}

	public int CalcBreathRecoverValue(int value)
	{
		value = DomainManager.SpecialEffect.ModifyValue(_id, 195, value);
		int num = _acupointCollection.CalcAcupointParam(0);
		value = value * (100 - num) / 100;
		return value;
	}

	public int CalcStanceRecoverValue(int value)
	{
		value = DomainManager.SpecialEffect.ModifyValue(_id, 196, value);
		int num = _acupointCollection.CalcAcupointParam(1);
		value = value * (100 - num) / 100;
		return value;
	}

	public int GetMaxMobility()
	{
		int maxMobility = MoveSpecialConstants.MaxMobility;
		int dataValue = 100;
		dataValue = DomainManager.SpecialEffect.ModifyData(_id, -1, 274, dataValue);
		return maxMobility * dataValue / 100;
	}

	public void CalcCostTrickStatus(List<NeedTrick> costTricks, Dictionary<sbyte, byte> costTrickDict, Dictionary<sbyte, byte> lackTrickDict)
	{
		costTrickDict.Clear();
		lackTrickDict.Clear();
		byte value;
		foreach (NeedTrick costTrick in costTricks)
		{
			sbyte trickType = costTrick.TrickType;
			value = (costTrickDict[costTrick.TrickType] = costTrick.NeedCount);
			lackTrickDict[trickType] = value;
		}
		TrickCollection tricks = GetTricks();
		sbyte key2;
		foreach (KeyValuePair<int, sbyte> trick in tricks.Tricks)
		{
			trick.Deconstruct(out var _, out key2);
			sbyte key3 = key2;
			if (lackTrickDict.TryGetValue(key3, out var value2) && value2 > 0)
			{
				lackTrickDict[key3] = (byte)(value2 - 1);
			}
		}
		List<sbyte> list = ObjectPool<List<sbyte>>.Instance.Get();
		list.Clear();
		foreach (KeyValuePair<sbyte, byte> item2 in lackTrickDict)
		{
			item2.Deconstruct(out key2, out value);
			sbyte item = key2;
			if (value == 0)
			{
				list.Add(item);
			}
		}
		foreach (sbyte item3 in list)
		{
			lackTrickDict.Remove(item3);
		}
		ObjectPool<List<sbyte>>.Instance.Return(list);
	}

	public void CalcInsteadTricks(Dictionary<sbyte, byte> insteadTrickDict, Func<sbyte, bool> insteadPredicate, Dictionary<sbyte, byte> costTrickDict, Dictionary<sbyte, byte> lackTrickDict, int maxInsteadCount = int.MaxValue, bool onlyInsteadLack = false)
	{
		insteadTrickDict.Clear();
		foreach (sbyte value2 in GetTricks().Tricks.Values)
		{
			if (!insteadPredicate(value2))
			{
				continue;
			}
			if (costTrickDict.Count == 0 || (onlyInsteadLack && lackTrickDict.Count == 0) || maxInsteadCount <= 0)
			{
				break;
			}
			insteadTrickDict[value2] = (byte)Math.Clamp(insteadTrickDict.GetOrDefault(value2) + 1, 0, 255);
			sbyte key = ((lackTrickDict.Count > 0) ? lackTrickDict.First().Key : costTrickDict.First().Key);
			if (lackTrickDict.TryGetValue(key, out var value))
			{
				if (value <= 1)
				{
					lackTrickDict.Remove(key);
				}
				else
				{
					lackTrickDict[key] = (byte)(value - 1);
				}
			}
			Tester.Assert(costTrickDict[key] > 0);
			costTrickDict[key]--;
			if (costTrickDict[key] == 0)
			{
				costTrickDict.Remove(key);
			}
			maxInsteadCount--;
		}
	}

	public void AddInfinityMindMarkProgress(DataContext context, int markCount)
	{
		int mindMarkAddInfinityProgress = GlobalConfig.Instance.MindMarkAddInfinityProgress;
		_mindMarkInfinityProgress += DomainManager.SpecialEffect.ModifyValue(_id, 305, markCount * mindMarkAddInfinityProgress);
		int num = 0;
		while (_mindMarkInfinityProgress >= CFormula.CalcInfinityMindMarkProgress(_mindMarkInfinityCount))
		{
			_mindMarkInfinityProgress -= CFormula.CalcInfinityMindMarkProgress(_mindMarkInfinityCount);
			_mindMarkInfinityCount++;
			num++;
		}
		if (num > 0)
		{
			DomainManager.Combat.AppendMindDefeatMark(context, this, num, -1, infinity: true);
		}
	}

	public short GetAttackSpeedPercent()
	{
		return _character.GetAttackSpeed();
	}

	public short GetSkillPrepareSpeed()
	{
		return _character.GetCastSpeed();
	}

	public bool ChangeWugCount(DataContext context, int delta)
	{
		short wugCount = GetWugCount();
		short num = (short)Math.Clamp(wugCount + delta, 0, GlobalConfig.Instance.MaxWugCount);
		if (num != wugCount)
		{
			SetWugCount(num, context);
		}
		return num - wugCount != 0;
	}

	public void ChangeToProportion(DataContext context, sbyte fiveElementsType, int maxChangeValue)
	{
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		NeiliProportionOfFiveElements neiliProportionOfFiveElements = _character.GetNeiliProportionOfFiveElements();
		int num = neiliProportionOfFiveElements[fiveElementsType];
		int num2 = 100 - num;
		if (maxChangeValue == 0 || (maxChangeValue < 0 && num == 0) || (maxChangeValue > 0 && num2 == 0))
		{
			return;
		}
		maxChangeValue = Math.Min(maxChangeValue, num2);
		NeiliProportionOfFiveElements proportionDelta = _proportionDelta;
		int num3 = 0;
		for (int i = 0; i < 5; i++)
		{
			if (i != fiveElementsType)
			{
				CValuePercent val = CValuePercent.Parse((int)neiliProportionOfFiveElements[i], num2);
				int num4 = maxChangeValue * val;
				if (num4 != 0)
				{
					num3 += num4;
					proportionDelta[i] = (sbyte)(proportionDelta[i] - num4);
				}
			}
		}
		proportionDelta[fiveElementsType] = (sbyte)(proportionDelta[fiveElementsType] + num3);
		SetProportionDelta(proportionDelta, context);
		sbyte neiliType = _character.GetNeiliType();
		if (neiliType != _neiliType)
		{
			SetNeiliType(neiliType, context);
		}
	}

	public void SilenceNeiliAllocationAutoRecover(DataContext context, int cdFrame)
	{
		if (_neiliAllocationCd.Cover(cdFrame))
		{
			SetNeiliAllocationCd(_neiliAllocationCd, context);
		}
	}

	public bool TickNeiliAllocationCd(DataContext context)
	{
		if (_neiliAllocationCd.Tick())
		{
			SetNeiliAllocationCd(_neiliAllocationCd, context);
		}
		return _neiliAllocationCd.Silencing;
	}

	public bool AnyLowerThanOriginNeiliAllocation()
	{
		for (byte b = 0; b < 4; b++)
		{
			if (_neiliAllocation[b] < _originNeiliAllocation[b])
			{
				return true;
			}
		}
		return false;
	}

	public short GetMaxNeiliAllocation(byte type)
	{
		return (short)(GetOriginNeiliAllocation()[type] * 3);
	}

	public int ApplySpecialEffectToNeiliAllocation(byte type, int addValue)
	{
		if (addValue == 0)
		{
			return 0;
		}
		bool flag = addValue > 0;
		if (flag)
		{
			float num = 100 + DomainManager.SpecialEffect.GetModifyValue(_id, 135, (EDataModifyType)1, type, -1, -1, (EDataSumType)0);
			num += (float)this.GetAddNeiliAllocationAddPercent(type);
			(int, int) totalPercentModifyValue = DomainManager.SpecialEffect.GetTotalPercentModifyValue(_id, -1, 135, type);
			addValue = (int)Math.Floor((float)addValue * num / 100f);
			addValue = (int)Math.Floor((float)addValue * (100f + (float)totalPercentModifyValue.Item1 + (float)totalPercentModifyValue.Item2) / 100f);
			addValue = DomainManager.SpecialEffect.ModifyData(_id, -1, 135, addValue);
		}
		else
		{
			float num2 = 100 + DomainManager.SpecialEffect.GetModifyValue(_id, 136, (EDataModifyType)1, type, -1, -1, (EDataSumType)0);
			num2 += (float)this.GetCostNeiliAllocationAddPercent(type);
			(int, int) totalPercentModifyValue2 = DomainManager.SpecialEffect.GetTotalPercentModifyValue(_id, -1, 136, type);
			addValue = (int)Math.Ceiling((float)addValue * num2 / 100f);
			addValue = (int)Math.Ceiling((float)addValue * (100f + (float)totalPercentModifyValue2.Item1 + (float)totalPercentModifyValue2.Item2) / 100f);
			addValue = DomainManager.SpecialEffect.ModifyData(_id, -1, 136, addValue);
		}
		if (addValue != 0)
		{
			flag = addValue > 0;
		}
		addValue = (flag ? Math.Max(addValue, 1) : Math.Min(addValue, -1));
		return addValue;
	}

	public bool ChangeNeiliAllocationRandom(DataContext context, int addValue, int count, bool applySpecialEffect = true)
	{
		bool flag = false;
		for (int i = 0; i < count; i++)
		{
			flag = ChangeNeiliAllocationRandom(context, addValue, applySpecialEffect) || flag;
		}
		return flag;
	}

	public bool ChangeNeiliAllocationRandom(DataContext context, int addValue, bool applySpecialEffect = true)
	{
		if (addValue == 0)
		{
			return false;
		}
		bool flag = addValue > 0;
		List<byte> list = ObjectPool<List<byte>>.Instance.Get();
		list.Clear();
		for (byte b = 0; b < 4; b++)
		{
			if (!(flag ? (_originNeiliAllocation[b] <= 0) : (_neiliAllocation[b] <= 0)))
			{
				list.Add(b);
			}
		}
		bool flag2 = list.Count > 0;
		if (flag2)
		{
			ChangeNeiliAllocation(context, list.GetRandom(context.Random), addValue, applySpecialEffect);
		}
		ObjectPool<List<byte>>.Instance.Return(list);
		return flag2;
	}

	public int ChangeNeiliAllocation(DataContext context, byte type, int addValue, bool applySpecialEffect = true, bool raiseEvent = true)
	{
		if (applySpecialEffect)
		{
			addValue = ApplySpecialEffectToNeiliAllocation(type, addValue);
			if (addValue == 0)
			{
				return 0;
			}
			if (!DomainManager.SpecialEffect.ModifyData(_id, -1, 137, dataValue: true, (addValue <= 0) ? 1 : 0))
			{
				return 0;
			}
		}
		short num = _neiliAllocation[type];
		short num2 = (short)MathUtils.Clamp(num + addValue, 0, _combatDomain.GetMaxNeiliAllocation(this, type));
		int num3 = num2 - num;
		_neiliAllocation[type] = num2;
		SetNeiliAllocation(_neiliAllocation, context);
		_defeatMarkCollection.SyncNeiliAllocationMark(context, this);
		_combatDomain.UpdateTeammateCommandUsable(context, this, _combatDomain.IsMainCharacter(this) ? ETeammateCommandImplement.ReduceNeiliAllocation : ETeammateCommandImplement.TransferNeiliAllocation);
		if (raiseEvent)
		{
			Events.RaiseNeiliAllocationChanged(context, _id, type, num3);
		}
		return num3;
	}

	public void ChangeAllNeiliAllocation(DataContext context, int addPercent, bool raiseEvent = true)
	{
		for (byte b = 0; b < 4; b++)
		{
			short num = _neiliAllocation[b];
			short num2 = (short)MathUtils.Clamp(num + num * addPercent / 100, 0, _combatDomain.GetMaxNeiliAllocation(this, b));
			int changeValue = num2 - num;
			_neiliAllocation[b] = num2;
			if (!_combatDomain.IsMainCharacter(this))
			{
				_combatDomain.UpdateTeammateCommandUsable(context, this, ETeammateCommandImplement.TransferNeiliAllocation);
			}
			if (raiseEvent)
			{
				Events.RaiseNeiliAllocationChanged(context, _id, b, changeValue);
			}
		}
		SetNeiliAllocation(_neiliAllocation, context);
		_defeatMarkCollection.SyncNeiliAllocationMark(context, this);
	}

	private byte RandomAbsorbNeiliAllocationType(IRandomSource random, CombatCharacter target)
	{
		List<byte> list = ObjectPool<List<byte>>.Instance.Get();
		List<byte> list2 = ObjectPool<List<byte>>.Instance.Get();
		for (byte b = 0; b < 4; b++)
		{
			if (target._neiliAllocation[b] > 0)
			{
				if (_neiliAllocation[b] < GetMaxNeiliAllocation(b))
				{
					list.Add(b);
				}
				else
				{
					list2.Add(b);
				}
			}
		}
		byte result = byte.MaxValue;
		if (list.Count > 0)
		{
			result = list.GetRandom(random);
		}
		else if (list2.Count > 0)
		{
			result = list2.GetRandom(random);
		}
		ObjectPool<List<byte>>.Instance.Return(list);
		ObjectPool<List<byte>>.Instance.Return(list2);
		return result;
	}

	public bool AbsorbNeiliAllocation(DataContext context, CombatCharacter target, byte type, int value)
	{
		value = Math.Min(value, target._neiliAllocation[type]);
		if (value <= 0)
		{
			return false;
		}
		value = -target.ChangeNeiliAllocation(context, type, -value);
		ChangeNeiliAllocation(context, type, value);
		GameData.GameDataBridge.GameDataBridge.AddDisplayEvent(DisplayEventType.CombatShowAbsorbNeiliAllocation, target._id, _id, type);
		return true;
	}

	public bool AbsorbNeiliAllocationRandom(DataContext context, CombatCharacter target, int value)
	{
		byte b = RandomAbsorbNeiliAllocationType(context.Random, target);
		return b != byte.MaxValue && AbsorbNeiliAllocation(context, target, b, value);
	}

	public void StealNeiliAllocationRandom(DataContext context, CombatCharacter target, CValuePercent percent)
	{
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		byte b = RandomAbsorbNeiliAllocationType(context.Random, target);
		if (b != byte.MaxValue)
		{
			int value = Math.Max((int)target._neiliAllocation[b] * percent, 1);
			AbsorbNeiliAllocation(context, target, b, value);
		}
	}

	public unsafe void SetNeiliAllocationRecoverProgress(DataContext context, byte type, short percent)
	{
		if (_neiliAllocationRecoverProgress.Items[(int)type] != percent)
		{
			_neiliAllocationRecoverProgress.Items[(int)type] = percent;
			SetNeiliAllocationRecoverProgress(_neiliAllocationRecoverProgress, context);
		}
	}

	public void AddOrUpdateFlawOrAcupoint(DataContext context, sbyte bodyPart, bool isFlaw, sbyte level, bool raiseEvent = true, int leftFrames = -1, int totalFrames = -1)
	{
		if (isFlaw ? GetFlawImmunity() : GetAcupointImmunity())
		{
			DomainManager.Combat.ShowImmunityEffectTips(context, _id, isFlaw ? EMarkType.Flaw : EMarkType.Acupoint);
			return;
		}
		FlawOrAcupointCollection flawOrAcupointCollection = (isFlaw ? _flawCollection : _acupointCollection);
		byte[] array = (isFlaw ? _flawCount : _acupointCount);
		List<(sbyte, int, int)> list = flawOrAcupointCollection.BodyPartDict[bodyPart];
		int num = (isFlaw ? GlobalConfig.Instance.FlawBaseKeepTime[level] : GlobalConfig.Instance.AcupointBaseKeepTime[level]);
		bool flag = list.Count < (isFlaw ? GetMaxFlawCount() : GetMaxAcupointCount());
		bool flag2 = DomainManager.SpecialEffect.ModifyData(_id, -1, (ushort)(isFlaw ? 126 : 131), dataValue: true, bodyPart);
		if (flag && !flag2)
		{
			return;
		}
		if (flag)
		{
			array[bodyPart]++;
			list.Add((level, (totalFrames > 0) ? totalFrames : num, (leftFrames > 0) ? leftFrames : num));
			if (isFlaw)
			{
				SetFlawCount(array, context);
				SetFlawCollection(flawOrAcupointCollection, context);
				if (raiseEvent)
				{
					Events.RaiseFlawAdded(context, this, bodyPart, level);
				}
			}
			else
			{
				SetAcupointCount(array, context);
				SetAcupointCollection(flawOrAcupointCollection, context);
				if (raiseEvent)
				{
					Events.RaiseAcuPointAdded(context, this, bodyPart, level);
				}
			}
			if (_combatDomain.IsMainCharacter(this))
			{
				_combatDomain.UpdateAllTeammateCommandUsable(context, IsAlly, isFlaw ? ETeammateCommandImplement.HealFlaw : ETeammateCommandImplement.HealAcupoint);
			}
		}
		else
		{
			int index = 0;
			int val = int.MaxValue;
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].Item1 < Math.Min(level, val))
				{
					index = i;
					val = list[i].Item1;
				}
			}
			(sbyte, int, int) item = list[index];
			item.Item1 = level;
			item.Item2 = (item.Item3 = num);
			list.RemoveAt(0);
			list.Add(item);
			if (isFlaw)
			{
				SetFlawCollection(flawOrAcupointCollection, context);
			}
			else
			{
				SetAcupointCollection(flawOrAcupointCollection, context);
			}
		}
		_combatDomain.UpdateBodyDefeatMark(context, this, bodyPart);
		if (!isFlaw && flag && list.Count >= 3)
		{
			_combatDomain.UpdateSkillNeedBodyPartCanUse(context, this);
		}
	}

	public int UpgradeRandomFlawOrAcupoint(DataContext context, bool isFlaw, int count = 1, sbyte bodyPart = -1)
	{
		if (isFlaw ? GetFlawImmunity() : GetAcupointImmunity())
		{
			DomainManager.Combat.ShowImmunityEffectTips(context, _id, isFlaw ? EMarkType.Flaw : EMarkType.Acupoint);
			return 0;
		}
		int[] array = (isFlaw ? GlobalConfig.Instance.FlawBaseKeepTime : GlobalConfig.Instance.AcupointBaseKeepTime);
		int num = array.Length - 1;
		FlawOrAcupointCollection flawOrAcupointCollection = (isFlaw ? _flawCollection : _acupointCollection);
		List<sbyte> list = ObjectPool<List<sbyte>>.Instance.Get();
		List<int> list2 = ObjectPool<List<int>>.Instance.Get();
		HashSet<sbyte> hashSet = ObjectPool<HashSet<sbyte>>.Instance.Get();
		list.Clear();
		list2.Clear();
		hashSet.Clear();
		GenerateFlawOrAcupointRandomPool(bodyPart, flawOrAcupointCollection, list, list2, num);
		int num2 = Math.Min(list.Count, count);
		for (int i = 0; i < num2; i++)
		{
			int index = context.Random.Next(list.Count);
			List<(sbyte, int, int)> list3 = flawOrAcupointCollection.BodyPartDict[list[index]];
			(sbyte, int, int) value = list3[list2[index]];
			value.Item1 = (sbyte)Math.Min(value.Item1 + 1, num);
			value.Item3 = (value.Item2 = array[value.Item1]);
			list3[list2[index]] = value;
			hashSet.Add(list[index]);
			CollectionUtils.SwapAndRemove(list, index);
			CollectionUtils.SwapAndRemove(list2, index);
		}
		if (num2 > 0)
		{
			if (isFlaw)
			{
				SetFlawCollection(flawOrAcupointCollection, context);
			}
			else
			{
				SetAcupointCollection(flawOrAcupointCollection, context);
			}
		}
		foreach (sbyte item in hashSet)
		{
			_combatDomain.UpdateBodyDefeatMark(context, this, item);
		}
		ObjectPool<List<sbyte>>.Instance.Return(list);
		ObjectPool<List<int>>.Instance.Return(list2);
		ObjectPool<HashSet<sbyte>>.Instance.Return(hashSet);
		return num2;
	}

	public void RemoveRandomFlawOrAcupoint(DataContext context, bool isFlaw, int count = 1)
	{
		FlawOrAcupointCollection flawOrAcupointCollection = (isFlaw ? _flawCollection : _acupointCollection);
		byte[] array = (isFlaw ? _flawCount : _acupointCount);
		List<sbyte> list = ObjectPool<List<sbyte>>.Instance.Get();
		List<(sbyte, sbyte)> list2 = new List<(sbyte, sbyte)>();
		list.Clear();
		for (sbyte b = 0; b < 7; b++)
		{
			for (int i = 0; i < array[b]; i++)
			{
				list.Add(b);
			}
		}
		int num = Math.Min(count, list.Count);
		for (int j = 0; j < num; j++)
		{
			int index = context.Random.Next(0, list.Count);
			sbyte b2 = list[index];
			int index2 = context.Random.Next(0, flawOrAcupointCollection.BodyPartDict[b2].Count);
			list.RemoveAt(index);
			list2.Add((b2, flawOrAcupointCollection.BodyPartDict[b2][index2].level));
			array[b2]--;
			flawOrAcupointCollection.BodyPartDict[b2].RemoveAt(index2);
		}
		if (isFlaw)
		{
			SetFlawCount(array, context);
			SetFlawCollection(flawOrAcupointCollection, context);
		}
		else
		{
			SetAcupointCount(array, context);
			SetAcupointCollection(flawOrAcupointCollection, context);
		}
		_combatDomain.UpdateBodyDefeatMark(context, this);
		if (_combatDomain.IsMainCharacter(this))
		{
			if (isFlaw)
			{
				_combatDomain.UpdateAllTeammateCommandUsable(context, IsAlly, ETeammateCommandImplement.HealFlaw);
			}
			else
			{
				_combatDomain.UpdateAllCommandAvailability(context, this);
			}
		}
		for (int k = 0; k < list2.Count; k++)
		{
			(sbyte, sbyte) tuple = list2[k];
			if (isFlaw)
			{
				Events.RaiseFlawRemoved(context, this, tuple.Item1, tuple.Item2);
			}
			else
			{
				Events.RaiseAcuPointRemoved(context, this, tuple.Item1, tuple.Item2);
			}
		}
		ObjectPool<List<sbyte>>.Instance.Return(list);
	}

	private static void GenerateFlawOrAcupointRandomPool(sbyte bodyPart, FlawOrAcupointCollection collection, ICollection<sbyte> bodyPartRandomPool, ICollection<int> indexRandomPool, int maxLevel, bool onlyMaxLevel = false)
	{
		if (bodyPart < 0)
		{
			for (sbyte b = 0; b < 7; b++)
			{
				AddToRandom(b);
			}
		}
		else
		{
			AddToRandom(bodyPart);
		}
		void AddToRandom(sbyte bodyPartKey)
		{
			if (collection.BodyPartDict.TryGetValue(bodyPartKey, out List<(sbyte, int, int)> value) && value != null && value.Count > 0)
			{
				for (int i = 0; i < value.Count; i++)
				{
					(sbyte, int, int) tuple = value[i];
					if (!(onlyMaxLevel ? (tuple.Item1 != maxLevel) : (tuple.Item1 >= maxLevel)))
					{
						bodyPartRandomPool.Add(bodyPartKey);
						indexRandomPool.Add(i);
					}
				}
			}
		}
	}

	public int GetMaxFlawCount()
	{
		int val = 3 + DomainManager.SpecialEffect.GetModifyValue(_id, 125, (EDataModifyType)0, -1, -1, -1, (EDataSumType)0);
		return Math.Max(val, 0);
	}

	public int GetMaxAcupointCount()
	{
		int val = 3 + DomainManager.SpecialEffect.GetModifyValue(_id, 130, (EDataModifyType)0, -1, -1, -1, (EDataSumType)0);
		return Math.Max(val, 0);
	}

	public short GetRecoveryOfFlaw()
	{
		int num = 0;
		if (_affectingDefendSkillId >= 0)
		{
			GameData.Domains.CombatSkill.CombatSkill element_CombatSkills = DomainManager.CombatSkill.GetElement_CombatSkills((charId: _id, skillId: _affectingDefendSkillId));
			num += element_CombatSkills.GetPageEffects().Sum((SkillBreakPageEffectImplementItem pageEffect) => pageEffect.FlawRecoverSpeed);
		}
		return (short)DomainManager.SpecialEffect.ModifyValue(_id, 185, _character.GetRecoveryOfFlaw(), -1, -1, -1, 0, num);
	}

	public short GetRecoveryOfAcupoint()
	{
		int num = 0;
		if (_affectingDefendSkillId >= 0)
		{
			GameData.Domains.CombatSkill.CombatSkill element_CombatSkills = DomainManager.CombatSkill.GetElement_CombatSkills((charId: _id, skillId: _affectingDefendSkillId));
			num += element_CombatSkills.GetPageEffects().Sum((SkillBreakPageEffectImplementItem pageEffect) => pageEffect.AcupointRecoverSpeed);
		}
		return (short)DomainManager.SpecialEffect.ModifyValue(_id, 186, _character.GetRecoveryOfBlockedAcupoint(), -1, -1, -1, 0, num);
	}

	public void ClearInjuryAutoHealProgress(DataContext context, bool inner)
	{
		InjuryAutoHealCollection injuryAutoHealCollection = GetInjuryAutoHealCollection();
		List<short>[] array = (inner ? injuryAutoHealCollection.InnerBodyPartList : injuryAutoHealCollection.OuterBodyPartList);
		foreach (List<short> list in array)
		{
			for (int j = 0; j < list.Count; j++)
			{
				list[j] = 0;
			}
		}
		SetInjuryAutoHealCollection(injuryAutoHealCollection, context);
	}

	public int GetDamageValue(sbyte bodyPart, bool inner)
	{
		return (inner ? GetInnerDamageValue() : GetOuterDamageValue())[bodyPart];
	}

	public void SetDamageValue(DataContext context, int leftDamage, sbyte bodyPart, bool inner)
	{
		int[] array = (inner ? GetInnerDamageValue() : GetOuterDamageValue());
		array[bodyPart] = leftDamage;
		if (inner)
		{
			SetInnerDamageValue(array, context);
		}
		else
		{
			SetOuterDamageValue(array, context);
		}
	}

	public void AddDamageToShow(DataContext context, int damage, int criticalPercent, sbyte bodyPart, bool inner)
	{
		IntPair intPair = (inner ? _innerDamageValueToShow[bodyPart] : _outerDamageValueToShow[bodyPart]);
		intPair.First = Math.Max(intPair.First, 0) + damage;
		intPair.Second = Math.Max(intPair.Second, criticalPercent);
		if (inner)
		{
			_innerDamageValueToShow[bodyPart] = intPair;
			SetInnerDamageValueToShow(_innerDamageValueToShow, context);
		}
		else
		{
			_outerDamageValueToShow[bodyPart] = intPair;
			SetOuterDamageValueToShow(_outerDamageValueToShow, context);
		}
	}

	public void AddMindDamageToShow(DataContext context, int mindDamage)
	{
		int mindDamageValueToShow = Math.Max(_mindDamageValueToShow, 0) + mindDamage;
		SetMindDamageValueToShow(mindDamageValueToShow, context);
	}

	public void AddFatalDamageToShow(DataContext context, int fatalDamage)
	{
		int fatalDamageValueToShow = Math.Max(_fatalDamageValueToShow, 0) + fatalDamage;
		SetFatalDamageValueToShow(fatalDamageValueToShow, context);
	}

	public CombatStateCollection GetCombatStateCollection(sbyte type)
	{
		if (1 == 0)
		{
		}
		CombatStateCollection result = type switch
		{
			0 => _specialCombatStateCollection, 
			1 => _buffCombatStateCollection, 
			2 => _debuffCombatStateCollection, 
			_ => throw new Exception($"Invalid combat state type: {type}"), 
		};
		if (1 == 0)
		{
		}
		return result;
	}

	public void SetCombatStateCollection(sbyte type, CombatStateCollection stateCollection, DataContext context)
	{
		switch (type)
		{
		case 0:
			SetSpecialCombatStateCollection(stateCollection, context);
			break;
		case 1:
			SetBuffCombatStateCollection(stateCollection, context);
			break;
		case 2:
			SetDebuffCombatStateCollection(stateCollection, context);
			break;
		}
	}

	public int GetCombatStatePower(sbyte stateType, short stateId)
	{
		CombatStateCollection combatStateCollection = GetCombatStateCollection(stateType);
		(short, bool, int) value;
		return combatStateCollection.StateDict.TryGetValue(stateId, out value) ? value.Item1 : 0;
	}

	public short GetCombatStatePowerLimit(sbyte type)
	{
		return type switch
		{
			0 => 500, 
			1 => (short)Math.Max(500 + BuffCombatStatePowerExtraLimit, 0), 
			2 => (short)Math.Max(500 + DebuffCombatStatePowerExtraLimit, 0), 
			_ => throw new Exception($"Invalid combat state type: {type}"), 
		};
	}

	public unsafe int GetFightBackPower(sbyte hitType)
	{
		GameData.Domains.CombatSkill.CombatSkill combatSkill = ((_affectingDefendSkillId >= 0) ? DomainManager.CombatSkill.GetElement_CombatSkills(new CombatSkillKey(_id, _affectingDefendSkillId)) : null);
		HitOrAvoidInts hitOrAvoidInts = combatSkill?.GetAddAvoidValueOnCast() ?? default(HitOrAvoidInts);
		int value = ((combatSkill != null && hitOrAvoidInts.Items[hitType] > 0) ? combatSkill.GetFightBackPower() : 0);
		return DomainManager.SpecialEffect.ModifyValue(_id, 112, value, hitType);
	}

	public OuterAndInnerInts GetBouncePower(sbyte attackInnerRatio = 50)
	{
		OuterAndInnerInts result = ((_affectingDefendSkillId > 0) ? DomainManager.CombatSkill.GetElement_CombatSkills(new CombatSkillKey(_id, _affectingDefendSkillId)).GetBouncePower() : new OuterAndInnerInts(0, 0));
		result.Outer = DomainManager.SpecialEffect.ModifyValue(_id, 111, result.Outer, 0, attackInnerRatio);
		result.Inner = DomainManager.SpecialEffect.ModifyValue(_id, 111, result.Inner, 1, attackInnerRatio);
		return result;
	}

	public unsafe bool PoisonOverflow(sbyte poisonType)
	{
		return PoisonsAndLevels.CalcPoisonedLevel(_poison.Items[poisonType]) > 0;
	}

	public void AddPoisonAffectValue(sbyte poisonType, short value, bool needLessThanThreshold = false)
	{
		if (!GetCharacter().HasPoisonImmunity(poisonType))
		{
			_poisonAffectAccumulator[poisonType] += value;
			short affectNeedValue = Poison.Instance[poisonType].AffectNeedValue;
			affectNeedValue += (short)DomainManager.SpecialEffect.GetModifyValue(_id, 243, (EDataModifyType)0, poisonType, -1, -1, (EDataSumType)0);
			affectNeedValue = Math.Max((short)1, affectNeedValue);
			if (needLessThanThreshold)
			{
				_poisonAffectAccumulator[poisonType] = (short)Math.Min(_poisonAffectAccumulator[poisonType], affectNeedValue - 1);
			}
			while (_poisonAffectAccumulator[poisonType] >= affectNeedValue)
			{
				_poisonAffectAccumulator[poisonType] -= affectNeedValue;
				_combatDomain.PoisonAffect(GetDataContext(), this, poisonType);
			}
		}
	}

	private void OnDefeatMarkChanged(DataContext context, DataUid dataUid)
	{
		if (!IsAlly)
		{
			_combatDomain.UpdateShowUseSpecialMisc(context);
		}
	}

	public int CalcAccessoryReducePoisonResist(sbyte poisonType, sbyte poisonLevel)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		int num = 0;
		CValuePercent val = CValuePercent.op_Implicit(GlobalConfig.Instance.AccessoryReducePoisonPercent);
		ItemKey[] equipment = _character.GetEquipment();
		for (int i = 8; i <= 10; i++)
		{
			if (DomainManager.Combat.CheckEquipmentPoison(equipment[i], out var attachedPoisons))
			{
				var (num2, b) = attachedPoisons.GetValueAndLevel(poisonType);
				if (b > poisonLevel)
				{
					num -= (int)num2 * val;
				}
			}
		}
		return num;
	}

	private void OnPoisonResistChanged(DataContext context, DataUid dataUid)
	{
		SetPoisonResist(ref _character.GetPoisonResists(), context);
		_combatDomain.UpdatePoisonDefeatMark(context, this);
	}

	public int GetFeatureMedalValue(sbyte medalType)
	{
		return _character.GetFeatureMedalValue(medalType);
	}

	public (int add, int reduce) GetFeatureSilenceFrameTotalPercent()
	{
		int num = 0;
		int num2 = 0;
		foreach (short featureId in _character.GetFeatureIds())
		{
			CharacterFeatureItem characterFeatureItem = CharacterFeature.Instance[featureId];
			num = Math.Max(num, characterFeatureItem.SilenceFramePercent);
			num2 = Math.Min(num2, characterFeatureItem.SilenceFramePercent);
		}
		return (add: num, reduce: num2);
	}

	public bool HasInfectedFeature(ECharacterFeatureInfectedType type)
	{
		if (1 == 0)
		{
		}
		bool result;
		switch (type)
		{
		case ECharacterFeatureInfectedType.NotInfected:
			result = !HasInfectedFeature(ECharacterFeatureInfectedType.PartlyInfected) && !HasInfectedFeature(ECharacterFeatureInfectedType.CompletelyInfected);
			break;
		case ECharacterFeatureInfectedType.PartlyInfected:
		case ECharacterFeatureInfectedType.CompletelyInfected:
			result = _character.GetFeatureIds().Any((short x) => CharacterFeature.Instance[x].InfectedType == type);
			break;
		default:
			result = false;
			break;
		}
		if (1 == 0)
		{
		}
		return result;
	}

	public unsafe sbyte GetPersonalityValue(sbyte type)
	{
		Personalities personalities = _character.GetPersonalities();
		return personalities.Items[type];
	}

	public bool ChangeToEmptyHand(DataContext context)
	{
		if (!Config.Character.Instance[_character.GetTemplateId()].AllowUseFreeWeapon)
		{
			return false;
		}
		DomainManager.Combat.ChangeWeapon(context, this, 3, init: false, force: true);
		if (StateMachine.GetCurrentStateType() == CombatCharacterStateType.PrepareSkill && _preparingSkillId >= 0 && !DomainManager.Combat.WeaponHasNeedTrick(this, _preparingSkillId, DomainManager.Combat.GetUsingWeaponData(this)))
		{
			DomainManager.Combat.InterruptSkill(context, this, -1);
		}
		return true;
	}

	public bool ChangeToEmptyHandOrOther(DataContext context)
	{
		if (ChangeToEmptyHand(context))
		{
			return true;
		}
		for (int i = 0; i < 3; i++)
		{
			if (i != _usingWeaponIndex && _weapons[i].IsValid())
			{
				CombatWeaponData element_WeaponDataDict = DomainManager.Combat.GetElement_WeaponDataDict(_weapons[i].Id);
				if (element_WeaponDataDict.GetCanChangeTo())
				{
					DomainManager.Combat.ChangeWeapon(context, this, i, init: false, force: true);
					return true;
				}
			}
		}
		return false;
	}

	public bool CanUnlockAttackByConfig(int index)
	{
		return GetUnlockEffect(index) != null;
	}

	public WeaponUnlockEffectItem GetUnlockEffect(int index)
	{
		if (!_weapons.CheckIndex(index))
		{
			return null;
		}
		ItemKey itemKey = _weapons[index];
		if (!itemKey.IsValid())
		{
			return null;
		}
		short templateId = itemKey.TemplateId;
		WeaponItem weaponItem = Config.Weapon.Instance[templateId];
		return WeaponUnlockEffect.Instance[weaponItem.UnlockEffect];
	}

	public CombatWeaponData GetWeaponData(int index = -1)
	{
		index = ((index < 0) ? _usingWeaponIndex : index);
		ItemKey itemKey = _weapons[index];
		return _combatDomain.GetElement_WeaponDataDict(itemKey.Id);
	}

	public sbyte GetConfigAttackPointCost()
	{
		GameData.Domains.Item.Weapon usingWeapon = DomainManager.Combat.GetUsingWeapon(this);
		return usingWeapon.GetAttackPreparePointCost();
	}

	public void ClearUnlockAttackValue(DataContext context, int index)
	{
		if (_unlockPrepareValue.CheckIndex(index) && _unlockPrepareValue[index] > 0)
		{
			_unlockPrepareValue[index] = 0;
			SetUnlockPrepareValue(_unlockPrepareValue, context);
		}
	}

	public void ChangeUnlockAttackValue(DataContext context, int index, int delta)
	{
		if (_unlockPrepareValue.CheckIndex(index) && CanUnlockAttackByConfig(index) && GetWeaponData(index).GetDurability() > 0)
		{
			if (delta > 0)
			{
				delta = DomainManager.SpecialEffect.ModifyValue(_id, 317, delta);
				delta = Math.Max(delta, 1);
			}
			int num = Math.Clamp(_unlockPrepareValue[index] + delta, 0, GlobalConfig.Instance.UnlockAttackUnit);
			if (num != _unlockPrepareValue[index])
			{
				_unlockPrepareValue[index] = num;
				SetUnlockPrepareValue(_unlockPrepareValue, context);
			}
		}
	}

	public void ChangeAllUnlockAttackValue(DataContext context, int delta)
	{
		for (int i = 0; i < 3; i++)
		{
			ChangeUnlockAttackValue(context, i, delta);
		}
	}

	public void ChangeAllUnlockAttackValue(DataContext context, CValuePercent deltaPercent)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		ChangeAllUnlockAttackValue(context, GlobalConfig.Instance.UnlockAttackUnit * deltaPercent);
	}

	public bool LegSkillUseShoes()
	{
		return DomainManager.SpecialEffect.ModifyData(_id, -1, 88, dataValue: true);
	}

	public bool HasDoingOrReserveCommand()
	{
		bool flag = StateMachine.GetCurrentStateType() != CombatCharacterStateType.Idle || _combatReserveData.AnyReserve || NeedNormalAttack || NeedChangeTrickAttack || NeedUnlockAttack || NeedBreakAttack || NeedUseSkillFreeId >= 0 || ChangeCharId >= 0;
		if (!flag && _combatDomain.IsMainCharacter(this))
		{
			int[] array = (IsAlly ? _combatDomain.GetSelfTeam() : _combatDomain.GetEnemyTeam());
			for (int i = 0; i < TeammateHasCommand.Length; i++)
			{
				if (TeammateHasCommand[i] && _combatDomain.GetElement_CombatCharacterDict(array[i + 1]).ExecutingTeammateCommandConfig.IntoCombatField)
				{
					flag = true;
					break;
				}
			}
		}
		return flag;
	}

	public void ClearAllDoingOrReserveCommand(DataContext context)
	{
		NeedChangeTrickAttack = (NeedUnlockAttack = (NeedBreakAttack = false));
		SetPreparingSkillId(-1, context);
		SetAffectingMoveSkillId(-1, context);
		SetAffectingDefendSkillId(-1, context);
		SetPreparingItem(ItemKey.Invalid, context);
		SetPreparingOtherAction(-1, context);
		SetCombatReserveData(CombatReserveData.Invalid, context);
		SetReserveNormalAttack(reserveNormalAttack: false, context);
		NeedNormalAttackImmediate = false;
		KeepMoving = false;
		MoveData.ResetJumpState(context, calcPreparedMove: false);
		SetAnimationToLoop(null, context);
		SetParticleToLoop(null, context);
	}

	public void SetNeedUseSkillId(DataContext context, short needUseSkillId)
	{
		SetCombatReserveData(CombatReserveData.CreateSkill(needUseSkillId), context);
	}

	public void SetNeedShowChangeTrick(DataContext context, bool needShowChangeTrick)
	{
		SetCombatReserveData(CombatReserveData.CreateChangeTrick(needShowChangeTrick), context);
	}

	public void SetNeedChangeWeaponIndex(DataContext context, int needChangeWeaponIndex)
	{
		SetCombatReserveData(CombatReserveData.CreateChangeWeapon(needChangeWeaponIndex), context);
	}

	public void SetNeedUnlockWeaponIndex(DataContext context, int needUnlockWeaponIndex)
	{
		SetCombatReserveData(CombatReserveData.CreateUnlockAttack(needUnlockWeaponIndex), context);
	}

	public void SetNeedUseOtherAction(DataContext context, sbyte needUseOtherAction)
	{
		SetCombatReserveData(CombatReserveData.CreateOtherAction(needUseOtherAction), context);
	}

	public void SetNeedUseItem(DataContext context, ItemKey needUseItem)
	{
		SetCombatReserveData(CombatReserveData.CreateUseItem(needUseItem), context);
	}

	public void SetNeedTeammateCommand(DataContext context, int teammateId, int index)
	{
		SetCombatReserveData(CombatReserveData.CreateTeammateCommand(teammateId, index), context);
	}

	public void NormalAttackFree()
	{
		NeedFreeAttack = true;
	}

	public void FinishFreeAttack()
	{
		IsAutoNormalAttacking = false;
	}

	public bool GetOuterInjuryImmunity()
	{
		return _character.GetOuterInjuryImmunity() || OuterInjuryImmunity;
	}

	public bool GetInnerInjuryImmunity()
	{
		return _character.GetInnerInjuryImmunity() || InnerInjuryImmunity;
	}

	public bool GetMindImmunity()
	{
		return _character.GetMindImmunity() || MindImmunity;
	}

	public bool GetFlawImmunity()
	{
		return _character.GetFlawImmunity() || FlawImmunity;
	}

	public bool GetAcupointImmunity()
	{
		return _character.GetAcupointImmunity() || AcupointImmunity;
	}

	public void ClearAllSound(DataContext context)
	{
		SetAttackSoundToPlay(null, context);
		SetSkillSoundToPlay(null, context);
		SetHitSoundToPlay(null, context);
		SetArmorHitSoundToPlay(null, context);
		SetWhooshSoundToPlay(null, context);
		SetShockSoundToPlay(null, context);
		SetStepSoundToPlay(null, context);
		SetDieSoundToPlay(null, context);
	}

	public void InitTeammateCommand(DataContext context, bool isFirstMove)
	{
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0106: Unknown result type (might be due to invalid IL or missing references)
		if (DomainManager.Combat.GetTeamCharacterIds().Contains(_id))
		{
			IReadOnlyList<sbyte> preRandomizedTeammateCommands = DomainManager.Combat.GetPreRandomizedTeammateCommands(context, _id);
			short favorability = DomainManager.Character.GetFavorability(_id, _combatDomain.GetMainCharacter(IsAlly).GetId());
			sbyte favorabilityType = FavorabilityType.GetFavorabilityType(favorability);
			byte b = (byte)(isFirstMove ? 75 : 50);
			CValuePercent val = CValuePercent.op_Implicit((int)b);
			for (int i = 0; i < 3; i++)
			{
				sbyte b2 = (sbyte)((preRandomizedTeammateCommands != null && i < preRandomizedTeammateCommands.Count) ? preRandomizedTeammateCommands[i] : (-1));
				_currTeammateCommands.Add(b2);
				_teammateCommandBanReasons.Add(SByteList.Create());
				_teammateCommandCanUse.Add(item: false);
				_teammateCommandCdPercent.Add(b);
				TeammateCommandCdTotalCount[i] = ((b2 >= 0) ? TeammateCommand.Instance[b2].CdCount : (-1));
				TeammateCommandCdCurrentCount[i] = TeammateCommandCdTotalCount[i] * val;
			}
			TeammateCommandCdSpeed = ((favorabilityType >= 5) ? 100 : ((favorabilityType >= 3) ? 85 : ((favorabilityType >= 1) ? 70 : ((favorabilityType >= 0) ? 55 : ((favorabilityType >= -2) ? 40 : ((favorabilityType >= -4) ? 25 : 10))))));
			StopCommandEffectCount = 0;
			UpdateTeammateCommandOnPrepared(context);
		}
	}

	public int GetTeammateCommandCdSpeed(sbyte cmdType)
	{
		if (_showTransferInjuryCommand)
		{
			cmdType = 13;
		}
		ETeammateCommandImplement implement = TeammateCommand.Instance[cmdType].Implement;
		int modifyValue = DomainManager.SpecialEffect.GetModifyValue(_combatDomain.GetMainCharacter(IsAlly).GetId(), 183, (EDataModifyType)1, (int)implement, -1, -1, (EDataSumType)0);
		return TeammateCommandCdSpeed * 100 / (100 + modifyValue);
	}

	public void ClearTeammateCommandCd(DataContext context, int index)
	{
		List<sbyte> currTeammateCommands = GetCurrTeammateCommands();
		if (!currTeammateCommands.CheckIndex(index) || currTeammateCommands[index] < 0)
		{
			return;
		}
		CombatCharacter mainCharacter = DomainManager.Combat.GetMainCharacter(IsAlly);
		if (TeammateCommandCdCurrentCount[index] < TeammateCommandCdTotalCount[index] && mainCharacter.TeammateBeforeMainChar != _id && mainCharacter.TeammateAfterMainChar != _id)
		{
			TeammateCommandCdCurrentCount[index] = TeammateCommandCdTotalCount[index];
			List<byte> teammateCommandCdPercent = GetTeammateCommandCdPercent();
			if (teammateCommandCdPercent[index] != 0)
			{
				teammateCommandCdPercent[index] = 0;
				SetTeammateCommandCdPercent(teammateCommandCdPercent, context);
			}
			DomainManager.Combat.UpdateTeammateCommandUsable(context, this, currTeammateCommands[index]);
		}
	}

	public void ResetTeammateCommandCd(DataContext context, int index, int cdCount = -1, bool checkEvent = false, bool displayEvent = false)
	{
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		sbyte b = _currTeammateCommands[index];
		if (cdCount < 0)
		{
			cdCount = TeammateCommand.Instance[b].CdCount;
		}
		CombatCharacter mainCharacter = DomainManager.Combat.GetMainCharacter(IsAlly);
		if (mainCharacter.GetCharacter().IsTreasuryGuard())
		{
			cdCount *= CValuePercentBonus.op_Implicit(GlobalConfig.Instance.TreasuryGuardTeammateCdBonus);
		}
		TeammateCommandCdTotalCount[index] = cdCount;
		bool flag = checkEvent && CheckInvokeSkipCd(context.Random, b);
		if (flag)
		{
			DomainManager.Combat.ShowTeammateCommand(_id, index, displayEvent);
		}
		TeammateCommandCdCurrentCount[index] = (flag ? TeammateCommandCdTotalCount[index] : 0);
		_teammateCommandCdPercent[index] = (byte)((!flag) ? 100u : 0u);
		SetTeammateCommandCdPercent(_teammateCommandCdPercent, context);
		_combatDomain.UpdateTeammateCommandUsable(context, this, b);
	}

	private bool CheckInvokeSkipCd(IRandomSource random, sbyte cmdType)
	{
		TeammateCommandItem teammateCommandItem = TeammateCommand.Instance[cmdType];
		if (teammateCommandItem.Type == ETeammateCommandType.Negative)
		{
			return false;
		}
		if (!_character.Template.AllowFavorabilitySkipCd)
		{
			return false;
		}
		CombatCharacter mainCharacter = DomainManager.Combat.GetMainCharacter(IsAlly);
		short favorability = DomainManager.Character.GetFavorability(_id, mainCharacter.GetId());
		if (favorability < teammateCommandItem.FavorLimit[0] || favorability > teammateCommandItem.FavorLimit[1])
		{
			return false;
		}
		return random.CheckPercentProb(teammateCommandItem.AutoProb);
	}

	public bool UpdateTeammateCommandState(DataContext context)
	{
		if (ExecutingTeammateCommandConfig.IntoCombatField)
		{
			if (!_visible)
			{
				sbyte b = -1;
				if (ExecutingTeammateCommandImplement.IsAttack() && _combatDomain.InAttackRange(this))
				{
					b = GetNormalAttackPosition(_attackCommandTrickType);
				}
				int displayPos = ((b > 0) ? _combatDomain.GetDisplayPosition(IsAlly, b) : int.MinValue);
				_combatDomain.SetDisplayPosition(context, IsAlly, displayPos);
				SetAnimationToLoop(_combatDomain.GetIdleAni(this), context);
				SetVisible(visible: true, context);
				SetTeammateCommandPreparePercent(0, context);
				if (TeammateCommandLeftPrepareFrame <= 0)
				{
					ResetTeammateCommandLeftTime(context);
				}
				return true;
			}
		}
		else if (TeammateCommandLeftFrame < 0)
		{
			ResetTeammateCommandLeftTime(context);
			if (ExecutingTeammateCommandImplement == ETeammateCommandImplement.StopEnemy)
			{
				int[] array = (IsAlly ? _combatDomain.GetEnemyTeam() : _combatDomain.GetSelfTeam());
				for (int i = 1; i < array.Length; i++)
				{
					if (array[i] >= 0)
					{
						_combatDomain.GetElement_CombatCharacterDict(array[i]).StopCommandEffectCount++;
					}
				}
				_combatDomain.UpdateAllTeammateCommandUsable(context, !IsAlly, -1);
			}
			if (ExecutingTeammateCommandImplement == ETeammateCommandImplement.AnimalEffect && ExecutingTeammateCommandSpecialEffect < 0)
			{
				string effectName = SharedConstValue.AnimalCarrier2Effect[AnimalConfig.CarrierId];
				int id = _combatDomain.GetMainCharacter(IsAlly).GetId();
				ExecutingTeammateCommandSpecialEffect = DomainManager.SpecialEffect.Add(context, id, effectName);
			}
			if (TeammateCommandEffects.TryGetValue(ExecutingTeammateCommandImplement, out var value) && ExecutingTeammateCommandSpecialEffect < 0)
			{
				SpecialEffectBase effect = (SpecialEffectBase)Activator.CreateInstance(value, _id);
				ExecutingTeammateCommandSpecialEffect = DomainManager.SpecialEffect.Add(context, effect);
			}
		}
		if (TeammateCommandLeftPrepareFrame > 0)
		{
			TeammateCommandLeftPrepareFrame--;
			byte b2 = (byte)((TeammateCommandLeftPrepareFrame != 0) ? ((uint)((TeammateCommandTotalPrepareFrame - TeammateCommandLeftPrepareFrame) * 100 / TeammateCommandTotalPrepareFrame)) : 0u);
			if (GetTeammateCommandPreparePercent() != b2)
			{
				SetTeammateCommandPreparePercent(b2, context);
			}
			if (TeammateCommandLeftPrepareFrame == 0)
			{
				ResetTeammateCommandLeftTime(context);
			}
			return TeammateCommandLeftPrepareFrame == 0;
		}
		if (TeammateCommandLeftFrame > 0)
		{
			ReduceTeammateCommandLeftTime(context);
			if (TeammateCommandLeftFrame == 0)
			{
				if (ExecutingTeammateCommandImplement == ETeammateCommandImplement.StopEnemy)
				{
					int[] array2 = (IsAlly ? _combatDomain.GetEnemyTeam() : _combatDomain.GetSelfTeam());
					for (int j = 1; j < array2.Length; j++)
					{
						if (array2[j] >= 0)
						{
							_combatDomain.GetElement_CombatCharacterDict(array2[j]).StopCommandEffectCount--;
						}
					}
					_combatDomain.UpdateAllTeammateCommandUsable(context, !IsAlly, -1);
				}
				if (ExecutingTeammateCommandSpecialEffect >= 0)
				{
					DomainManager.SpecialEffect.Remove(context, ExecutingTeammateCommandSpecialEffect);
					ExecutingTeammateCommandSpecialEffect = -1L;
				}
				ClearTeammateCommand(context);
			}
			return false;
		}
		if (_teammateExitAniLeftFrame > 0)
		{
			_teammateExitAniLeftFrame--;
		}
		if (_teammateExitAniLeftFrame <= 0)
		{
			ClearTeammateCommandData(context);
		}
		return false;
	}

	public void ResetTeammateCommandLeftTime(DataContext context)
	{
		if (ExecutingTeammateCommandConfig.AffectFrame > 0)
		{
			short num = ExecutingTeammateCommandConfig.AffectFrame;
			ETeammateCommandImplement implement = ExecutingTeammateCommandConfig.Implement;
			if ((implement == ETeammateCommandImplement.Fight || implement == ETeammateCommandImplement.StopEnemy) ? true : false)
			{
				int modifyValue = DomainManager.SpecialEffect.GetModifyValue(_combatDomain.GetMainCharacter(IsAlly).GetId(), 184, (EDataModifyType)0, (int)ExecutingTeammateCommandConfig.Implement, -1, -1, (EDataSumType)0);
				num = (short)(num * (100 + modifyValue) / 100);
			}
			TeammateCommandLeftFrame = (TeammateCommandTotalFrame = num);
			SetTeammateCommandTimePercent(100, context);
		}
	}

	public void ReduceTeammateCommandLeftTime(DataContext context)
	{
		TeammateCommandLeftFrame--;
		byte b = (byte)(TeammateCommandLeftFrame * 100 / TeammateCommandTotalFrame);
		if (GetTeammateCommandTimePercent() != b)
		{
			SetTeammateCommandTimePercent(b, context);
		}
	}

	public void ClearTeammateCommand(DataContext context, bool interrupt = false)
	{
		if (_executingTeammateCommand < 0)
		{
			return;
		}
		CombatCharacter mainCharacter = _combatDomain.GetMainCharacter(IsAlly);
		PartlyClearTeammateCommand(context, interrupt);
		if (ExecutingTeammateCommandConfig.IntoCombatField && (ExecutingTeammateCommandConfig.PrepareFrame > 0 || ExecutingTeammateCommandImplement.IsAttack() || ExecutingTeammateCommandImplement.IsDefend()))
		{
			TeammateCommandLeftPrepareFrame = 0;
			TeammateCommandLeftFrame = 0;
			_teammateExitAniLeftFrame = 1;
			SetTeammateCommandPreparePercent(0, context);
			if (interrupt)
			{
				if (mainCharacter.TeammateBeforeMainChar < 0)
				{
					mainCharacter.SpecialAnimationLoop = null;
				}
				_combatDomain.SetProperLoopAniAndParticle(context, mainCharacter);
				mainCharacter.SetTeammateCommandPreparePercent(0, context);
				if (!string.IsNullOrEmpty(_soundToLoop))
				{
					SetSoundToLoop(string.Empty, context);
				}
			}
		}
		else
		{
			if (!ExecutingTeammateCommandConfig.IntoCombatField)
			{
				ResetTeammateCommandCd(context, ExecutingTeammateCommandIndex, -1, checkEvent: true);
			}
			ClearTeammateCommandData(context);
		}
	}

	public void PartlyClearTeammateCommand(DataContext context, bool interrupt = false)
	{
		CombatCharacter mainCharacter = _combatDomain.GetMainCharacter(IsAlly);
		if (mainCharacter.TeammateBeforeMainChar == _id)
		{
			mainCharacter.TeammateBeforeMainChar = -1;
		}
		else if (mainCharacter.TeammateAfterMainChar == _id)
		{
			mainCharacter.TeammateAfterMainChar = -1;
		}
		mainCharacter.SetParticleToLoop(null, context);
		SetParticleToLoop(null, context);
		SetParticleToPlay(null, context);
		if (ExecutingTeammateCommandImplement.IsAttack())
		{
			_combatDomain.ClearDamageCompareData(context);
			UpdateAttackCommandWeaponAndTrick(context);
		}
		else if (ExecutingTeammateCommandImplement.IsDefend())
		{
			UpdateDefendCommandSkill(context);
		}
		else if (ExecutingTeammateCommandImplement == ETeammateCommandImplement.AttackSkill)
		{
			UpdateAttackCommandSkill(context);
		}
		if ((ExecutingTeammateCommandConfig.IntoCombatField && ExecutingTeammateCommandConfig.PrepareFrame > 0) || ExecutingTeammateCommandImplement.IsAttack() || ExecutingTeammateCommandImplement.IsDefend())
		{
			short posOffset = ExecutingTeammateCommandConfig.PosOffset;
			string animationToPlayOnce = ((posOffset < 0 && string.IsNullOrEmpty(ExecutingTeammateCommandConfig.BackCharExitAni) && !interrupt) ? ExecutingTeammateCommandConfig.BackCharExitAni : "M_004");
			_combatDomain.SetDisplayPosition(context, IsAlly, int.MinValue);
			SetAnimationToPlayOnce(animationToPlayOnce, context);
			SetAnimationToLoop(_combatDomain.GetIdleAni(this), context);
			SetDisplayPosition(int.MinValue, context);
			if (ExecutingTeammateCommandImplement.IsDefend())
			{
				_combatDomain.ClearAffectingDefenseSkill(context, this);
			}
		}
	}

	private void ClearTeammateCommandData(DataContext context)
	{
		CombatCharacter mainCharacter = _combatDomain.GetMainCharacter(IsAlly);
		mainCharacter.TeammateHasCommand[_combatDomain.GetCharacterList(IsAlly).IndexOf(_id) - 1] = false;
		SetExecutingTeammateCommand(-1, context);
		SetVisible(visible: false, context);
		TeammateCommandLeftFrame = -1;
		ExecutingTeammateCommandIndex = -1;
		_combatDomain.UpdateAllCommandAvailability(context, mainCharacter);
	}

	public OuterAndInnerShorts CalcAttackRangeImmediate(short skillId = -1, int weaponIndex = -1)
	{
		if (!_combatDomain.IsInCombat())
		{
			return _attackRange;
		}
		if (skillId < 0 && weaponIndex < 0)
		{
			skillId = (short)((_preparingSkillId > 0) ? _preparingSkillId : ((_performingSkillId > 0) ? _performingSkillId : (-1)));
		}
		GameData.Domains.CombatSkill.CombatSkill element = null;
		CombatSkillKey combatSkillKey = new CombatSkillKey(_id, skillId);
		if (skillId >= 0 && !DomainManager.CombatSkill.TryGetElement_CombatSkills(combatSkillKey, out element))
		{
			PredefinedLog.DefValue.CombatRuntimeException.Log($"{combatSkillKey} instance not found.");
			return _attackRange;
		}
		if (weaponIndex < 0)
		{
			weaponIndex = _usingWeaponIndex;
		}
		GameData.Domains.Item.Weapon element_Weapons = DomainManager.Item.GetElement_Weapons(_weapons[weaponIndex].Id);
		if (skillId >= 0 && DomainManager.CombatSkill.GetSkillType(_id, skillId) == 5)
		{
			element_Weapons = DomainManager.Item.GetElement_Weapons(_weapons[3].Id);
		}
		WeaponItem weaponItem = Config.Weapon.Instance[element_Weapons.GetTemplateId()];
		sbyte currTrick = (_changeTrickAttack ? ChangeTrickType : _weaponTricks[_weaponTrickIndex]);
		int num = (weaponItem.MinDistance + weaponItem.MaxDistance) / 2;
		byte attackRangeMidMinDistance = GlobalConfig.Instance.AttackRangeMidMinDistance;
		(int, int) weaponAttackRange = DomainManager.Item.GetWeaponAttackRange(_id, element_Weapons.GetItemKey());
		short num2 = (short)weaponAttackRange.Item1;
		short num3 = (short)weaponAttackRange.Item2;
		if (skillId >= 0)
		{
			if (weaponItem.TrickDistanceAdjusts.Count > 0)
			{
				List<NeedTrick> list = ObjectPool<List<NeedTrick>>.Instance.Get();
				DomainManager.CombatSkill.GetCombatSkillCostTrick(element, list);
				short num4 = 0;
				short num5 = 0;
				for (int i = 0; i < list.Count; i++)
				{
					sbyte skillTrick = list[i].TrickType;
					TrickDistanceAdjust trickDistanceAdjust = weaponItem.TrickDistanceAdjusts.Find((TrickDistanceAdjust adjust) => adjust.TrickTemplateId == skillTrick);
					if (trickDistanceAdjust != null)
					{
						num4 = Math.Max(num4, trickDistanceAdjust.MinDistance);
						num5 = Math.Max(num5, trickDistanceAdjust.MaxDistance);
					}
				}
				ObjectPool<List<NeedTrick>>.Instance.Return(list);
				num2 -= num4;
				num3 += num5;
			}
			num2 -= DomainManager.CombatSkill.GetCombatSkillAddAttackDistance(_id, skillId, forward: true);
			num3 += DomainManager.CombatSkill.GetCombatSkillAddAttackDistance(_id, skillId, forward: false);
		}
		else
		{
			TrickDistanceAdjust trickDistanceAdjust2 = weaponItem.TrickDistanceAdjusts.Find((TrickDistanceAdjust adjust) => adjust.TrickTemplateId == currTrick);
			if (trickDistanceAdjust2 != null)
			{
				num2 -= trickDistanceAdjust2.MinDistance;
				num3 += trickDistanceAdjust2.MaxDistance;
			}
		}
		num2 -= (short)DomainManager.SpecialEffect.GetModifyValue(_id, -1, 145, (EDataModifyType)0, -1, -1, -1, (EDataSumType)0);
		num3 += (short)DomainManager.SpecialEffect.GetModifyValue(_id, -1, 146, (EDataModifyType)0, -1, -1, -1, (EDataSumType)0);
		int modifyValue = DomainManager.SpecialEffect.GetModifyValue(_id, 273, (EDataModifyType)0, num2, num3, -1, (EDataSumType)0);
		int val = _acupointCollection.CalcAcupointParam(2);
		int num6 = Math.Max(modifyValue, val);
		num2 = (short)Math.Clamp(num2 + num6, 20, num - attackRangeMidMinDistance);
		num3 = (short)Math.Clamp(num3 - num6, num + attackRangeMidMinDistance, 120);
		return new OuterAndInnerShorts(num2, num3);
	}

	public void UpdateTeammateCommandOnPrepared(DataContext context)
	{
		List<sbyte> currTeammateCommands = GetCurrTeammateCommands();
		if (currTeammateCommands.Exists((sbyte x) => x >= 0 && TeammateCommand.Instance[x].RequireTrick))
		{
			UpdateAttackCommandWeaponAndTrick(context);
		}
		if (currTeammateCommands.Exists((sbyte x) => x >= 0 && TeammateCommand.Instance[x].RequireAttackSkill))
		{
			UpdateAttackCommandSkill(context);
		}
		if (currTeammateCommands.Exists((sbyte x) => x >= 0 && TeammateCommand.Instance[x].RequireDefendSkill))
		{
			UpdateDefendCommandSkill(context);
		}
		UpdateTeammateCommandInvokers();
	}

	public void UpdateAttackCommandWeaponAndTrick(DataContext context)
	{
		List<ItemKey> list = ObjectPool<List<ItemKey>>.Instance.Get();
		list.Clear();
		for (int i = 0; i < 3; i++)
		{
			ItemKey itemKey = _weapons[i];
			if (itemKey.IsValid() && DomainManager.Item.GetBaseItem(itemKey).GetCurrDurability() > 0)
			{
				list.Add(itemKey);
			}
		}
		if (list.Count == 0)
		{
			for (int j = 3; j < 7; j++)
			{
				ItemKey item = _weapons[j];
				if (item.IsValid())
				{
					list.Add(item);
				}
			}
		}
		ItemKey itemKey2 = list[context.Random.Next(0, list.Count)];
		CombatWeaponData element_WeaponDataDict = _combatDomain.GetElement_WeaponDataDict(itemKey2.Id);
		sbyte[] weaponTricks = element_WeaponDataDict.GetWeaponTricks();
		ObjectPool<List<ItemKey>>.Instance.Return(list);
		_combatDomain.ChangeWeapon(context, this, _weapons.IndexOf(itemKey2));
		SetUsingWeaponIndex(Array.IndexOf(_weapons, itemKey2), context);
		SetAttackCommandWeaponKey(itemKey2, context);
		SetAttackCommandTrickType(weaponTricks[context.Random.Next(0, weaponTricks.Length)], context);
		_combatDomain.UpdateTeammateCommandUsable(context, this, -1);
	}

	public void UpdateAttackCommandSkill(DataContext context)
	{
		int num = 0;
		List<short> list = ObjectPool<List<short>>.Instance.Get();
		list.Clear();
		for (int i = 0; i < _attackSkillList.Count; i++)
		{
			short num2 = _attackSkillList[i];
			if (num2 < 0)
			{
				continue;
			}
			int num3 = AiController.CalcAttackSkillScore(context.Random, num2);
			if (num3 >= num)
			{
				if (num3 > num)
				{
					num = num3;
					list.Clear();
				}
				list.Add(num2);
			}
		}
		SetAttackCommandSkillId((short)((list.Count > 0) ? list.GetRandom(context.Random) : (-1)), context);
		ObjectPool<List<short>>.Instance.Return(list);
		_combatDomain.UpdateTeammateCommandUsable(context, this, ETeammateCommandImplement.AttackSkill);
	}

	public void UpdateDefendCommandSkill(DataContext context)
	{
		List<short> list = ObjectPool<List<short>>.Instance.Get();
		list.Clear();
		for (int i = 0; i < _defenceSkillList.Count; i++)
		{
			short num = _defenceSkillList[i];
			if (num >= 0)
			{
				list.Add(num);
			}
		}
		SetDefendCommandSkillId((short)((list.Count > 0) ? list[context.Random.Next(0, list.Count)] : (-1)), context);
		ObjectPool<List<short>>.Instance.Return(list);
		_combatDomain.UpdateTeammateCommandUsable(context, this, -1);
	}

	public void UpdateTeammateCommandInvokers()
	{
		for (int i = 0; i < _currTeammateCommands.Count; i++)
		{
			sbyte b = _currTeammateCommands[i];
			if (b >= 0)
			{
				TeammateCommandItem cmdConfig = TeammateCommand.Instance[b];
				ITeammateCommandInvoker teammateCommandInvoker = TryCreateTeammateCommandInvoker(cmdConfig, i);
				if (teammateCommandInvoker != null)
				{
					teammateCommandInvoker.Setup();
					_teammateCommandInvokers.Add(teammateCommandInvoker);
				}
			}
		}
	}

	private ITeammateCommandInvoker TryCreateTeammateCommandInvoker(TeammateCommandItem cmdConfig, int i)
	{
		ETeammateCommandType type = cmdConfig.Type;
		if (1 == 0)
		{
		}
		ITeammateCommandInvoker result;
		switch (type)
		{
		case ETeammateCommandType.Negative:
		{
			ETeammateCommandImplement implement2 = cmdConfig.Implement;
			if (1 == 0)
			{
			}
			ITeammateCommandInvoker teammateCommandInvoker = implement2 switch
			{
				ETeammateCommandImplement.InterruptSkill => new TeammateCommandInvokerCombatSkillProgress(_id, i), 
				ETeammateCommandImplement.InterruptOtherAction => new TeammateCommandInvokerOtherActionProgress(_id, i), 
				_ => new TeammateCommandInvokerFrame(_id, i), 
			};
			if (1 == 0)
			{
			}
			result = teammateCommandInvoker;
			break;
		}
		case ETeammateCommandType.GearMate:
		{
			ETeammateCommandImplement implement = cmdConfig.Implement;
			if (1 == 0)
			{
			}
			ITeammateCommandInvoker teammateCommandInvoker = implement switch
			{
				ETeammateCommandImplement.GearMateA => new TeammateCommandInvokerCooldown(_id, i), 
				ETeammateCommandImplement.GearMateB => new TeammateCommandInvokerAutoDefend(_id, i), 
				_ => null, 
			};
			if (1 == 0)
			{
			}
			result = teammateCommandInvoker;
			break;
		}
		default:
			result = null;
			break;
		}
		if (1 == 0)
		{
		}
		return result;
	}

	public bool UpdateTeammateCharStatus(DataContext context)
	{
		if (!DomainManager.Combat.IsMainCharacter(this))
		{
			return false;
		}
		int[] characterList = DomainManager.Combat.GetCharacterList(IsAlly);
		for (int i = 1; i < characterList.Length; i++)
		{
			int num = characterList[i];
			if (num < 0)
			{
				continue;
			}
			CombatCharacter element_CombatCharacterDict = DomainManager.Combat.GetElement_CombatCharacterDict(num);
			if (element_CombatCharacterDict.GetExecutingTeammateCommand() < 0)
			{
				if (TeammateBeforeMainChar == num)
				{
					TeammateBeforeMainChar = -1;
				}
				if (TeammateAfterMainChar == num)
				{
					TeammateAfterMainChar = -1;
				}
				continue;
			}
			if (element_CombatCharacterDict.ExecutingTeammateCommandConfig.IntoCombatField && !element_CombatCharacterDict.GetVisible())
			{
				if (element_CombatCharacterDict.ExecutingTeammateCommandConfig.PosOffset > 0)
				{
					TeammateBeforeMainChar = num;
				}
				else
				{
					TeammateAfterMainChar = num;
				}
			}
			if (element_CombatCharacterDict.UpdateTeammateCommandState(context))
			{
				ActingTeammateCommandChar = element_CombatCharacterDict;
				StateMachine.TranslateState(CombatCharacterStateType.TeammateCommand);
				return true;
			}
		}
		return false;
	}

	public bool PreparingOrDoingTeammateCommand()
	{
		return TeammateAfterMainChar >= 0 || TeammateBeforeMainChar >= 0;
	}

	public bool PreparingTeammateCommand()
	{
		return (TeammateAfterMainChar >= 0 && _combatDomain.GetElement_CombatCharacterDict(TeammateAfterMainChar).TeammateCommandLeftPrepareFrame >= 0) || (TeammateBeforeMainChar >= 0 && _combatDomain.GetElement_CombatCharacterDict(TeammateBeforeMainChar).TeammateCommandLeftPrepareFrame >= 0);
	}

	public int CalcTeammateCommandRepairDurabilityValue(ItemKey equipKey)
	{
		EquipmentBase equipmentBase = (equipKey.IsValid() ? DomainManager.Item.TryGetBaseEquipment(equipKey) : null);
		if (equipmentBase == null)
		{
			return 0;
		}
		short currDurability = equipmentBase.GetCurrDurability();
		int num = DomainManager.Combat.EquipmentOldDurability.GetValueOrDefault(equipKey) - currDurability;
		if (num <= 0)
		{
			return 0;
		}
		sbyte craftRequiredLifeSkillType = ItemTemplateHelper.GetCraftRequiredLifeSkillType(equipKey.ItemType, equipKey.TemplateId);
		short lifeSkillAttainment = _character.GetLifeSkillAttainment(craftRequiredLifeSkillType);
		return CFormula.CalcPartRepairDurabilityValue(equipmentBase.GetGrade(), lifeSkillAttainment, currDurability, num);
	}

	public void ChangeAffectingDefenseSkillLeftFrame(DataContext context, CValuePercent delta)
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		if (_affectingDefendSkillId >= 0)
		{
			int num = (int)DefendSkillTotalFrame * delta;
			DefendSkillLeftFrame = (short)Math.Clamp(DefendSkillLeftFrame + num, 1, DefendSkillTotalFrame);
			if (DefendSkillLeftFrame <= 1)
			{
				SetAffectingDefendSkillId(-1, context);
				DomainManager.Combat.SetProperLoopAniAndParticle(context, this);
			}
		}
	}

	public bool AiCanCast(short skillId)
	{
		CombatSkillData combatSkillData;
		return DomainManager.Combat.TryGetCombatSkillData(_id, skillId, out combatSkillData) && combatSkillData.GetCanUse();
	}

	public sbyte AiGetCombatSkillRequireTrickType(short skillId)
	{
		if (skillId < 0)
		{
			return -1;
		}
		if (!DomainManager.CombatSkill.TryGetElement_CombatSkills((charId: _id, skillId: skillId), out var element))
		{
			return -1;
		}
		List<NeedTrick> list = ObjectPool<List<NeedTrick>>.Instance.Get();
		list.Clear();
		DomainManager.CombatSkill.GetCombatSkillCostTrick(element, list);
		list.RemoveAll((NeedTrick needTrick) => !_weaponTricks.Exist(needTrick.TrickType) || GetTrickCount(needTrick.TrickType) >= needTrick.NeedCount);
		sbyte result = (sbyte)((list.Count > 0) ? list[0].TrickType : (-1));
		ObjectPool<List<NeedTrick>>.Instance.Return(list);
		return result;
	}

	public bool AiCastCheckRange()
	{
		short currentDistance = DomainManager.Combat.GetCurrentDistance();
		OuterAndInnerShorts attackRange = GetAttackRange();
		int num = Math.Max(attackRange.Outer - currentDistance, currentDistance - attackRange.Inner);
		if (num < 0)
		{
			return false;
		}
		int num2 = GetCharacter().GetCombatSkillGridCost(GetPreparingSkillId()) * 5 + 5;
		return num2 >= num;
	}

	public int AiGetFirstChangeableWeaponIndex(int minIndex, int maxIndex)
	{
		short currentDistance = DomainManager.Combat.GetCurrentDistance();
		int num = Math.Max(minIndex, 0);
		int num2 = Math.Min(maxIndex + 1, _weapons.Length);
		for (int i = num; i < num2; i++)
		{
			ItemKey itemKey = _weapons[i];
			if (!itemKey.IsValid())
			{
				continue;
			}
			CombatWeaponData element_WeaponDataDict = DomainManager.Combat.GetElement_WeaponDataDict(itemKey.Id);
			if (element_WeaponDataDict.GetCanChangeTo())
			{
				OuterAndInnerShorts outerAndInnerShorts = CalcAttackRangeImmediate(-1, i);
				if (outerAndInnerShorts.Outer <= currentDistance && currentDistance <= outerAndInnerShorts.Inner)
				{
					return i;
				}
			}
		}
		return -1;
	}

	public bool AiCanRepair(ItemKey toolKey, ItemKey itemKey)
	{
		return DomainManager.Building.CheckRepairConditionIsMeet(_id, toolKey, itemKey, BuildingBlockKey.Invalid);
	}

	public bool AiCanRepair(IEnumerable<ItemKey> tools, ItemKey itemKey)
	{
		return tools.Any((ItemKey tool) => AiCanRepair(tool, itemKey));
	}

	public (ItemKey targetKey, ItemKey toolKey) AiSelectRepairTarget(IEnumerable<sbyte> equipmentSlots)
	{
		DataContext context = DomainManager.Combat.Context;
		ItemKey[] equipments = GetCharacter().GetEquipment();
		List<ItemKey> tools = ObjectPool<List<ItemKey>>.Instance.Get();
		tools.Clear();
		tools.AddRange(from itemKey in GetValidItems()
			where itemKey.ItemType == 6
			select itemKey);
		List<ItemKey> list = ObjectPool<List<ItemKey>>.Instance.Get();
		list.Clear();
		list.AddRange(from itemKey in equipmentSlots.Select((sbyte slot) => equipments[slot]).Where(delegate(ItemKey itemKey)
			{
				ItemKey itemKey2 = itemKey;
				return itemKey2.IsValid();
			})
			let baseItem = DomainManager.Item.GetBaseItem(itemKey)
			where baseItem.GetCurrDurability() <= 0
			where AiCanRepair(tools, itemKey)
			select itemKey);
		(ItemKey, ItemKey) result = (ItemKey.Invalid, ItemKey.Invalid);
		if (list.Count > 0)
		{
			ItemKey targetKey = list.GetRandom(context.Random);
			ItemKey item = tools.First((ItemKey toolKey) => AiCanRepair(toolKey, targetKey));
			result = (targetKey, item);
		}
		ObjectPool<List<ItemKey>>.Instance.Return(tools);
		ObjectPool<List<ItemKey>>.Instance.Return(list);
		return result;
	}

	public IEnumerable<(ItemKey weaponKey, int index)> AiCanChangeToWeapons()
	{
		for (int i = 0; i < _weapons.Length; i++)
		{
			if (i == _usingWeaponIndex)
			{
				continue;
			}
			ItemKey weaponKey = _weapons[i];
			if (weaponKey.IsValid())
			{
				CombatWeaponData weaponData = DomainManager.Combat.GetElement_WeaponDataDict(weaponKey.Id);
				if (weaponData.GetCanChangeTo())
				{
					yield return (weaponKey: weaponKey, index: i);
				}
			}
		}
	}

	int IExpressionConverter.GetPersonalityValue(int personalityType)
	{
		return GetPersonalityValue((sbyte)personalityType);
	}

	int IExpressionConverter.GetConsummateLevel()
	{
		return IsAlly ? GlobalConfig.Instance.MaxConsummateLevel : _character.GetConsummateLevel();
	}

	int IExpressionConverter.GetBehaviorType()
	{
		return _character.GetBehaviorType();
	}

	public sbyte GetNormalAttackPosition(sbyte trickType)
	{
		if (BossConfig != null)
		{
			return BossConfig.AttackDistances[_bossPhase][_usingWeaponIndex];
		}
		if (AnimalConfig != null)
		{
			return AnimalConfig.AttackDistances[_usingWeaponIndex];
		}
		return Config.TrickType.Instance[trickType].AttackDistance[UsingWeaponAction];
	}

	public string GetNormalAttackParticle(sbyte trickType)
	{
		if (BossConfig != null)
		{
			return $"{BossConfig.AttackParticles[_bossPhase]}_{PursueAttackCount}{AttackPostfix}";
		}
		if (AnimalConfig != null)
		{
			return AnimalConfig.AttackParticles[_usingWeaponIndex];
		}
		return $"{Config.TrickType.Instance[trickType].AttackParticles[UsingWeaponAction]}_{PursueAttackCount}";
	}

	public string GetNormalAttackSound(sbyte trickType)
	{
		if (BossConfig != null)
		{
			return $"{BossConfig.AttackSounds[_bossPhase]}_{PursueAttackCount}";
		}
		if (AnimalConfig != null)
		{
			return AnimalConfig.AttackSounds[_usingWeaponIndex];
		}
		return $"{Config.TrickType.Instance[trickType].SoundEffects[UsingWeaponAction]}_{PursueAttackCount}{UsingWeaponConfig.SwingSoundsSuffix}";
	}

	public string GetNormalAttackAnimation(sbyte trickType)
	{
		if (BossConfig != null)
		{
			return $"{BossConfig.AttackAnimation}_{PursueAttackCount}{AttackPostfix}";
		}
		if (AnimalConfig != null)
		{
			return Config.TrickType.Instance[trickType].AttackAnimations[UsingWeaponAction];
		}
		return $"{Config.TrickType.Instance[trickType].AttackAnimations[UsingWeaponAction]}_{PursueAttackCount}";
	}

	public string GetNormalAttackAnimationFull(sbyte trickType)
	{
		string normalAttackAnimation = GetNormalAttackAnimation(trickType);
		return GetNormalAttackAnimationFull(normalAttackAnimation);
	}

	public string GetNormalAttackAnimationFull(string animation)
	{
		if (BossConfig != null)
		{
			return BossConfig.AniPrefix[_bossPhase] + animation;
		}
		if (AnimalConfig != null)
		{
			return AnimalConfig.AniPrefix + animation;
		}
		return animation;
	}

	public void PlayBeHitSound(DataContext context, WeaponItem weapon, CombatCharacter attacker, bool critical)
	{
		if (attacker.NoBlockAttack || critical)
		{
			DomainManager.Combat.PlayHitSound(context, this, weapon);
		}
		else
		{
			DomainManager.Combat.PlayBlockSound(context, this);
		}
	}

	public void PlayWinAnimation(DataContext context)
	{
		if (IsActorSkeleton)
		{
			sbyte displayingGender = GetCharacter().GetDisplayingGender();
			int num = ((displayingGender < 0 || displayingGender >= 2) ? 1 : displayingGender);
			SetAnimationToPlayOnce(WinAni[num], context);
			SetAnimationToLoop(WinAniLoop[num], context);
		}
		else
		{
			SetAnimationToLoop(DomainManager.Combat.GetIdleAni(this), context);
		}
	}

	public bool ApplyChangeTrickFlawOrAcupoint(DataContext context, CombatCharacter defender, sbyte bodyPart)
	{
		if (PursueAttackCount != 0 || !GetChangeTrickAttack())
		{
			return false;
		}
		sbyte attackPreparePointCost = DomainManager.Combat.GetUsingWeapon(this).GetAttackPreparePointCost();
		if (ChangeTrickFlawOrAcupointType == EFlawOrAcupointType.Flaw)
		{
			DomainManager.Combat.AddFlaw(context, defender, attackPreparePointCost, (charId: -1, skillId: (short)(-1)), bodyPart);
		}
		else
		{
			if (ChangeTrickFlawOrAcupointType != EFlawOrAcupointType.Acupoint)
			{
				return false;
			}
			DomainManager.Combat.AddAcupoint(context, defender, attackPreparePointCost, (charId: -1, skillId: (short)(-1)), bodyPart);
		}
		return true;
	}

	public int MarkCountChangeToDamageValue(sbyte bodyPart, bool inner, int count)
	{
		int[] array = (inner ? _damageStepCollection.InnerDamageSteps : _damageStepCollection.OuterDamageSteps);
		int num = (inner ? _innerDamageValue : _outerDamageValue)[bodyPart];
		int val = 6 - _injuries.Get(bodyPart, inner);
		int num2 = Math.Min(val, count);
		int num3 = count - num2;
		int num4 = num2 * array[bodyPart] + num3 * _damageStepCollection.FatalDamageStep;
		return num4 - num - ((num3 > 0) ? _fatalDamageValue : 0);
	}

	public void RemoveInjury(DataContext context, sbyte bodyPart, bool inner, sbyte count = 1)
	{
		DomainManager.Combat.RemoveInjury(context, this, bodyPart, inner, count, updateDefeatMark: true);
	}

	public bool RemoveRandomInjury(DataContext context, sbyte count = 1)
	{
		Injuries injuries = _injuries.Subtract(_oldInjuries);
		bool flag = injuries.HasAnyInjury(isInnerInjury: true);
		bool flag2 = injuries.HasAnyInjury(isInnerInjury: false);
		if (!flag && !flag2)
		{
			return false;
		}
		bool flag3 = context.Random.RandomIsInner(flag, flag2);
		List<sbyte> list = ObjectPool<List<sbyte>>.Instance.Get();
		for (sbyte b = 0; b < 7; b++)
		{
			if (injuries.Get(b, flag3) > 0)
			{
				list.Add(b);
			}
		}
		RemoveInjury(context, list.GetRandom(context.Random), flag3, count);
		ObjectPool<List<sbyte>>.Instance.Return(list);
		return true;
	}

	private void ListenCharacterField(ushort fieldId, Action<DataContext, DataUid> handler)
	{
		DataUid dataUid = new DataUid(4, 0, (ulong)_id, fieldId);
		GameData.GameDataBridge.GameDataBridge.AddPostDataModificationHandler(dataUid, DataHandlerKey, handler);
		_markDataUids.Add(dataUid);
	}

	public void RegisterMarkHandler()
	{
		ListenCharacterField(59, _defeatMarkCollection.SyncWugMark);
		ListenCharacterField(21, _defeatMarkCollection.SyncQiDisorderMark);
		ListenCharacterField(19, _defeatMarkCollection.SyncHealthMark);
	}

	public void UnRegisterMarkHandler()
	{
		foreach (DataUid markDataUid in _markDataUids)
		{
			GameData.GameDataBridge.GameDataBridge.RemovePostDataModificationHandler(markDataUid, DataHandlerKey);
		}
		_markDataUids.Clear();
	}

	public int CalcNormalAttackStartupFrames()
	{
		GameData.Domains.Item.Weapon usingWeapon = DomainManager.Combat.GetUsingWeapon(this);
		return CalcNormalAttackStartupFrames(usingWeapon);
	}

	public int CalcNormalAttackStartupFrames(GameData.Domains.Item.Weapon weapon)
	{
		WeaponItem weaponItem = Config.Weapon.Instance[weapon.GetTemplateId()];
		int value = weapon.CalcAttackStartupOrRecoveryFrame(AttackSpeed, weaponItem.BaseStartupFrames);
		FlawOrAcupointCollection acupointCollection = _acupointCollection;
		int extraTotalPercentAdd = acupointCollection.CalcAcupointParam(3) + acupointCollection.CalcAcupointParam(4);
		value = DomainManager.SpecialEffect.ModifyValue(_id, 283, value, -1, -1, -1, 0, 0, extraTotalPercentAdd);
		return Math.Max(value, GlobalConfig.Instance.MinPrepareFrame);
	}

	public short CalcNormalAttackAnimationFrames(float animDuration)
	{
		FlawOrAcupointCollection acupointCollection = _acupointCollection;
		int num = acupointCollection.CalcAcupointParam(3) + acupointCollection.CalcAcupointParam(4);
		num /= 5;
		animDuration *= (float)(100 + num) / 100f;
		return (short)Math.Round(animDuration * 60f, MidpointRounding.AwayFromZero);
	}

	public int CalcNormalAttackRecoveryFrames(GameData.Domains.Item.Weapon weapon)
	{
		WeaponItem weaponItem = Config.Weapon.Instance[weapon.GetTemplateId()];
		int value = weapon.CalcAttackStartupOrRecoveryFrame(AttackSpeed, weaponItem.BaseRecoveryFrames);
		value = DomainManager.SpecialEffect.ModifyValue(_id, 321, value);
		return Math.Max(value, 1);
	}

	public void NormalAttackRecovery(DataContext context)
	{
		if (!IsAutoNormalAttacking && !GetChangeTrickAttack())
		{
			GameData.Domains.Item.Weapon usingWeapon = DomainManager.Combat.GetUsingWeapon(this);
			int newFrame = CalcNormalAttackRecoveryFrames(usingWeapon);
			if (_normalAttackRecovery.Cover(newFrame))
			{
				SetNormalAttackRecovery(_normalAttackRecovery, context);
			}
		}
	}

	public short GetOtherActionPrepareFrame(sbyte actionType)
	{
		int num = CombatDomain.OtherActionPrepareFrame[actionType];
		if (1 == 0)
		{
		}
		int num2 = actionType switch
		{
			0 => _character.GetLifeSkillAttainment(8), 
			1 => _character.GetLifeSkillAttainment(9), 
			_ => 0, 
		};
		if (1 == 0)
		{
		}
		int num3 = num2;
		if (1 == 0)
		{
		}
		num2 = actionType switch
		{
			0 => 120, 
			1 => 120, 
			_ => 0, 
		};
		if (1 == 0)
		{
		}
		int val = num2;
		if (num3 > 0)
		{
			num = Math.Max(num - num3 / 5 * 2, val);
		}
		int num4 = 100;
		if (1 == 0)
		{
		}
		ushort num5 = actionType switch
		{
			0 => 118, 
			1 => 121, 
			2 => 124, 
			_ => ushort.MaxValue, 
		};
		if (1 == 0)
		{
		}
		ushort num6 = num5;
		if (num6 != ushort.MaxValue)
		{
			num4 += DomainManager.SpecialEffect.GetModifyValue(_id, num6, (EDataModifyType)1, -1, -1, -1, (EDataSumType)0);
		}
		num = num * 100 / Math.Max(num4, GlobalConfig.Instance.HealInjuryPoisonSpeedMinPercent);
		return (short)Math.Clamp(num, 0, 32767);
	}

	private bool IsLegSkill(short skillId)
	{
		return skillId >= 0 && DomainManager.CombatSkill.GetSkillType(_id, skillId) == 5;
	}

	private bool IgnoreShoesOrIsNotLegSkill(short skillId)
	{
		return !IsLegSkill(skillId) || !LegSkillUseShoes();
	}

	private CValuePercent GetEquipmentPower(ItemKey key)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		return CValuePercent.op_Implicit((int)DomainManager.Character.GetItemPower(_id, key));
	}

	private GameData.Domains.Item.Weapon GetFinalWeapon(GameData.Domains.Item.Weapon weapon, short skillId, out ItemKey shoesWeaponKey)
	{
		shoesWeaponKey = ItemKey.Invalid;
		if (IgnoreShoesOrIsNotLegSkill(skillId))
		{
			return weapon;
		}
		ItemKey itemKey = Armors[5];
		GameData.Domains.Item.Armor armor = (itemKey.IsValid() ? DomainManager.Item.GetElement_Armors(itemKey.Id) : null);
		if (armor == null || armor.GetCurrDurability() <= 0)
		{
			return DomainManager.Item.GetElement_Weapons(_weapons[3].Id);
		}
		short relatedWeapon = Config.Armor.Instance[itemKey.TemplateId].RelatedWeapon;
		shoesWeaponKey = new ItemKey(0, 0, relatedWeapon, -1);
		return null;
	}

	private CValuePercentBonus CalcWeaponHitFactor(GameData.Domains.Item.Weapon weapon, sbyte hitType, short skillId)
	{
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		ItemKey shoesWeaponKey;
		GameData.Domains.Item.Weapon finalWeapon = GetFinalWeapon(weapon, skillId, out shoesWeaponKey);
		if (finalWeapon != null)
		{
			return CValuePercentBonus.op_Implicit((int)finalWeapon.GetHitFactors(_id)[hitType]);
		}
		short num = Config.Weapon.Instance[shoesWeaponKey.TemplateId].BaseHitFactors[hitType];
		if (num > 0)
		{
			return CValuePercentBonus.op_Implicit((int)num * GetEquipmentPower(shoesWeaponKey));
		}
		return CValuePercentBonus.op_Implicit((int)num);
	}

	private CValuePercent CalcWeaponPenetrateFactor(GameData.Domains.Item.Weapon weapon, bool inner, sbyte bodyPart, short skillId)
	{
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0140: Unknown result type (might be due to invalid IL or missing references)
		//IL_0137: Unknown result type (might be due to invalid IL or missing references)
		//IL_013c: Unknown result type (might be due to invalid IL or missing references)
		ItemKey shoesWeaponKey;
		GameData.Domains.Item.Weapon finalWeapon = GetFinalWeapon(weapon, skillId, out shoesWeaponKey);
		int num = finalWeapon?.GetPenetrationFactor() ?? Config.Weapon.Instance[shoesWeaponKey.TemplateId].BasePenetrationFactor;
		num *= GetEquipmentPower(finalWeapon?.GetItemKey() ?? shoesWeaponKey);
		sbyte b = ((finalWeapon != null) ? DomainManager.Combat.GetElement_WeaponDataDict(finalWeapon.GetId()).GetInnerRatio() : Config.Weapon.Instance[shoesWeaponKey.TemplateId].DefaultInnerRatio);
		CValuePercent val = CValuePercent.op_Implicit(inner ? b : (100 - b));
		num *= val;
		bool flag = DomainManager.SpecialEffect.ModifyData(_id, skillId, 281, dataValue: false);
		if (bodyPart < 0 || flag)
		{
			return CValuePercent.op_Implicit(num);
		}
		CombatCharacter combatCharacter = DomainManager.Combat.GetCombatCharacter(!IsAlly);
		ItemKey itemKey = combatCharacter.Armors[bodyPart];
		GameData.Domains.Item.Armor armor = (itemKey.IsValid() ? DomainManager.Item.GetElement_Armors(itemKey.Id) : null);
		int num2 = CombatDomain.CalcWeaponDefend(this, weapon, skillId);
		int num3 = CombatDomain.CalcArmorAttack(combatCharacter, armor);
		if (num3 > num2)
		{
			num = CFormula.FormulaCalcWeaponArmorFactor(num, num3, num2);
		}
		return CValuePercent.op_Implicit(num);
	}

	private CValuePercentBonus CalcArmorAvoidFactor(sbyte hitType, sbyte bodyPart, short skillId)
	{
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		CombatCharacter combatCharacter = DomainManager.Combat.GetCombatCharacter(!IsAlly);
		bool flag = DomainManager.SpecialEffect.ModifyData(combatCharacter.GetId(), skillId, 281, dataValue: false);
		if (bodyPart < 0 || flag)
		{
			return CValuePercentBonus.op_Implicit(0);
		}
		ItemKey itemKey = Armors[bodyPart];
		GameData.Domains.Item.Armor armor = (itemKey.IsValid() ? DomainManager.Item.GetElement_Armors(itemKey.Id) : null);
		if (armor == null)
		{
			return CValuePercentBonus.op_Implicit(0);
		}
		return CValuePercentBonus.op_Implicit((int)armor.GetAvoidFactors(_id)[hitType]);
	}

	private CValuePercentBonus CalcArmorPenetrateResistFactor(GameData.Domains.Item.Weapon weapon, bool inner, sbyte bodyPart, short skillId)
	{
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		CombatCharacter combatCharacter = DomainManager.Combat.GetCombatCharacter(!IsAlly);
		bool flag = DomainManager.SpecialEffect.ModifyData(combatCharacter.GetId(), skillId, 281, dataValue: false);
		if (bodyPart < 0 || flag)
		{
			return CValuePercentBonus.op_Implicit(0);
		}
		ItemKey key = Armors[bodyPart];
		GameData.Domains.Item.Armor armor = (key.IsValid() ? DomainManager.Item.GetElement_Armors(key.Id) : null);
		if (armor == null)
		{
			return CValuePercentBonus.op_Implicit(0);
		}
		int num = armor.GetPenetrationResistFactors().Get(inner) * GetEquipmentPower(key);
		int num2 = CombatDomain.CalcWeaponAttack(combatCharacter, weapon, skillId);
		int num3 = CombatDomain.CalcArmorDefend(this, armor);
		if (num2 > num3)
		{
			num = CFormula.FormulaCalcWeaponArmorFactor(num, num2, num3);
		}
		return CValuePercentBonus.op_Implicit(num);
	}

	private int CalcMoveSkillAddHitValue(sbyte hitType)
	{
		if (_affectingMoveSkillId < 0)
		{
			return 0;
		}
		GameData.Domains.CombatSkill.CombatSkill element_CombatSkills = DomainManager.CombatSkill.GetElement_CombatSkills((charId: _id, skillId: _affectingMoveSkillId));
		return element_CombatSkills.GetAddHitValueOnCast()[hitType];
	}

	private int CalcDefendSkillAddAvoidValue(sbyte hitType, bool ignoreDefendSkill)
	{
		if (_affectingDefendSkillId < 0 || ignoreDefendSkill)
		{
			return 0;
		}
		GameData.Domains.CombatSkill.CombatSkill element_CombatSkills = DomainManager.CombatSkill.GetElement_CombatSkills((charId: _id, skillId: _affectingDefendSkillId));
		return element_CombatSkills.GetAddAvoidValueOnCast()[hitType];
	}

	private int CalcDefendSkillAddPenetrateResistValue(bool inner, bool ignoreDefendSkill)
	{
		if (_affectingDefendSkillId < 0 || ignoreDefendSkill)
		{
			return 0;
		}
		GameData.Domains.CombatSkill.CombatSkill element_CombatSkills = DomainManager.CombatSkill.GetElement_CombatSkills((charId: _id, skillId: _affectingDefendSkillId));
		OuterAndInnerInts addPenetrateResist = element_CombatSkills.GetAddPenetrateResist();
		return inner ? addPenetrateResist.Inner : addPenetrateResist.Outer;
	}

	private CValueModify CalcTeammateHitModify(sbyte hitType, EDataSumType valueSumType)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0142: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_016b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0170: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0138: Unknown result type (might be due to invalid IL or missing references)
		//IL_013d: Unknown result type (might be due to invalid IL or missing references)
		CValueModify result = CValueModify.Zero;
		if (IsMainChar)
		{
			if (DataSumTypeHelper.ContainsAdd(valueSumType))
			{
				CValuePercentBonus bonus = CValuePercentBonus.op_Implicit(DomainManager.SpecialEffect.GetModifyValue(_id, 184, (EDataModifyType)0, 10, -1, -1, (EDataSumType)0));
				int num = (from teammateChar in DomainManager.Combat.GetTeammateCharacters(_id)
					where teammateChar.ExecutingTeammateCommandImplement == ETeammateCommandImplement.AddHit
					let baseValue = teammateChar.GetCharacter().GetHitValues()[hitType]
					select baseValue * CValuePercent.op_Implicit(teammateChar.ExecutingTeammateCommandConfig.IntArg) * bonus).Sum();
				result = ((CValueModify)(ref result)).ChangeA(num);
			}
			if (DataSumTypeHelper.ContainsReduce(valueSumType))
			{
				int num2 = (from teammateChar in DomainManager.Combat.GetTeammateCharacters(_id)
					where teammateChar.ExecutingTeammateCommandImplement == ETeammateCommandImplement.ReduceHitAndAvoid
					select teammateChar.ExecutingTeammateCommandConfig.IntArg).Sum();
				result = ((CValueModify)(ref result)).ChangeB(-num2);
			}
		}
		else if (DataSumTypeHelper.ContainsAdd(valueSumType))
		{
			if (ExecutingTeammateCommandImplement == ETeammateCommandImplement.Fight)
			{
				result = ((CValueModify)(ref result)).ChangeB(ExecutingTeammateCommandConfig.IntArg);
			}
			if (ExecutingTeammateCommandImplement == ETeammateCommandImplement.Attack)
			{
				result = ((CValueModify)(ref result)).ChangeC(DomainManager.SpecialEffect.GetModifyValue(MainChar.GetId(), 184, (EDataModifyType)0, 4, -1, -1, (EDataSumType)0));
			}
		}
		return result;
	}

	private CValueModify CalcTeammatePenetrateModify()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		CValueModify result = CValueModify.Zero;
		if (IsMainChar)
		{
			return result;
		}
		if (ExecutingTeammateCommandImplement == ETeammateCommandImplement.Fight)
		{
			result = ((CValueModify)(ref result)).ChangeB(ExecutingTeammateCommandConfig.IntArg);
		}
		if (ExecutingTeammateCommandImplement == ETeammateCommandImplement.Attack)
		{
			result = ((CValueModify)(ref result)).ChangeC(DomainManager.SpecialEffect.GetModifyValue(MainChar.GetId(), 184, (EDataModifyType)0, 4, -1, -1, (EDataSumType)0));
		}
		return result;
	}

	private CValueModify CalcTeammateAvoidModify(sbyte hitType, EDataSumType valueSumType)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0142: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0172: Unknown result type (might be due to invalid IL or missing references)
		//IL_0173: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_0177: Unknown result type (might be due to invalid IL or missing references)
		//IL_016b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0170: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0138: Unknown result type (might be due to invalid IL or missing references)
		//IL_013d: Unknown result type (might be due to invalid IL or missing references)
		CValueModify result = CValueModify.Zero;
		if (IsMainChar)
		{
			if (DataSumTypeHelper.ContainsAdd(valueSumType))
			{
				CValuePercentBonus bonus = CValuePercentBonus.op_Implicit(DomainManager.SpecialEffect.GetModifyValue(_id, 184, (EDataModifyType)0, 11, -1, -1, (EDataSumType)0));
				int num = (from teammateChar in DomainManager.Combat.GetTeammateCharacters(_id)
					where teammateChar.ExecutingTeammateCommandImplement == ETeammateCommandImplement.AddAvoid
					let baseValue = teammateChar.GetCharacter().GetAvoidValues()[hitType]
					select baseValue * CValuePercent.op_Implicit(teammateChar.ExecutingTeammateCommandConfig.IntArg) * bonus).Sum();
				result = ((CValueModify)(ref result)).ChangeA(num);
			}
			if (DataSumTypeHelper.ContainsReduce(valueSumType))
			{
				int num2 = (from teammateChar in DomainManager.Combat.GetTeammateCharacters(_id)
					where teammateChar.ExecutingTeammateCommandImplement == ETeammateCommandImplement.ReduceHitAndAvoid
					select teammateChar.ExecutingTeammateCommandConfig.IntArg).Sum();
				result = ((CValueModify)(ref result)).ChangeB(-num2);
			}
		}
		else if (DataSumTypeHelper.ContainsAdd(valueSumType) && ExecutingTeammateCommandImplement == ETeammateCommandImplement.Fight)
		{
			result = ((CValueModify)(ref result)).ChangeB(ExecutingTeammateCommandConfig.IntArg);
		}
		return result;
	}

	private CValueModify CalcTeammatePenetrateResistModify()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		CValueModify result = CValueModify.Zero;
		if (IsMainChar)
		{
			return result;
		}
		if (ExecutingTeammateCommandImplement == ETeammateCommandImplement.Fight)
		{
			result = ((CValueModify)(ref result)).ChangeB(ExecutingTeammateCommandConfig.IntArg);
		}
		return result;
	}

	public int GetHitValue(CombatContext context, sbyte hitType)
	{
		int skillAddPercent = context.Skill?.GetHitValue()[hitType] ?? 0;
		return GetHitValue(context.Weapon, hitType, context.BodyPart, skillAddPercent, context.SkillTemplateId);
	}

	public int GetHitValue(sbyte hitType, sbyte bodyPart = -1, int skillAddPercent = 0, short skillId = -1)
	{
		GameData.Domains.Item.Weapon usingWeapon = DomainManager.Combat.GetUsingWeapon(this);
		return GetHitValue(usingWeapon, hitType, bodyPart, skillAddPercent, skillId);
	}

	public int GetHitValue(GameData.Domains.Item.Weapon weapon, sbyte hitType, sbyte bodyPart, int skillAddPercent = 0, short skillId = -1)
	{
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_0100: Unknown result type (might be due to invalid IL or missing references)
		//IL_0105: Unknown result type (might be due to invalid IL or missing references)
		//IL_0107: Unknown result type (might be due to invalid IL or missing references)
		//IL_010c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0111: Unknown result type (might be due to invalid IL or missing references)
		//IL_0116: Unknown result type (might be due to invalid IL or missing references)
		//IL_0118: Unknown result type (might be due to invalid IL or missing references)
		//IL_012d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0132: Unknown result type (might be due to invalid IL or missing references)
		//IL_0133: Unknown result type (might be due to invalid IL or missing references)
		//IL_0138: Unknown result type (might be due to invalid IL or missing references)
		//IL_013d: Unknown result type (might be due to invalid IL or missing references)
		//IL_013f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0154: Unknown result type (might be due to invalid IL or missing references)
		//IL_0159: Unknown result type (might be due to invalid IL or missing references)
		//IL_015a: Unknown result type (might be due to invalid IL or missing references)
		//IL_015f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0164: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01da: Unknown result type (might be due to invalid IL or missing references)
		//IL_01df: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_016f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0174: Unknown result type (might be due to invalid IL or missing references)
		//IL_0179: Unknown result type (might be due to invalid IL or missing references)
		//IL_017e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0180: Unknown result type (might be due to invalid IL or missing references)
		//IL_0195: Unknown result type (might be due to invalid IL or missing references)
		//IL_019a: Unknown result type (might be due to invalid IL or missing references)
		//IL_019c: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_0227: Unknown result type (might be due to invalid IL or missing references)
		//IL_0207: Unknown result type (might be due to invalid IL or missing references)
		//IL_021a: Unknown result type (might be due to invalid IL or missing references)
		//IL_021f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0224: Unknown result type (might be due to invalid IL or missing references)
		long num = _character.GetHitValues()[hitType];
		num = DomainManager.SpecialEffect.ModifyData(_id, skillId, 158, (int)num, hitType);
		num *= CalcWeaponHitFactor(weapon, hitType, skillId);
		bool flag = DomainManager.SpecialEffect.ModifyData(_id, -1, 36, dataValue: true, hitType, 0);
		bool flag2 = DomainManager.SpecialEffect.ModifyData(_id, -1, 36, dataValue: true, hitType, 1);
		CValuePercentBonus val = CValuePercentBonus.op_Implicit(DomainManager.SpecialEffect.GetModifyValue(_id, 37, (EDataModifyType)0, hitType, 0, -1, (EDataSumType)0));
		CValuePercentBonus val2 = CValuePercentBonus.op_Implicit(DomainManager.SpecialEffect.GetModifyValue(_id, 37, (EDataModifyType)0, hitType, 1, -1, (EDataSumType)0));
		CombatCharacter combatCharacter = _combatDomain.GetCombatCharacter(!IsAlly, tryGetCoverCharacter: true);
		int id = _id;
		int id2 = combatCharacter.GetId();
		ushort fieldId = (ushort)(56 + hitType);
		ushort fieldId2 = (ushort)(90 + hitType);
		CValueModify val3 = CValueModify.Zero;
		if (flag)
		{
			val3 = ((CValueModify)(ref val3)).ChangeA(CalcMoveSkillAddHitValue(hitType));
			val3 += CalcTeammateHitModify(hitType, (EDataSumType)1);
			val3 += DomainManager.SpecialEffect.GetModify(id, fieldId, skillId, PursueAttackCount, bodyPart, (EDataSumType)1) * val;
			val3 += DomainManager.SpecialEffect.GetModify(id2, fieldId2, skillId, PursueAttackCount, bodyPart, (EDataSumType)1) * val;
		}
		if (flag2)
		{
			val3 += CalcTeammateHitModify(hitType, (EDataSumType)2);
			val3 += DomainManager.SpecialEffect.GetModify(id, fieldId, skillId, PursueAttackCount, bodyPart, (EDataSumType)2) * val2;
			val3 += DomainManager.SpecialEffect.GetModify(id2, fieldId2, skillId, PursueAttackCount, bodyPart, (EDataSumType)2) * val2;
		}
		val3 = ((CValueModify)(ref val3)).MaxB(CValuePercent.op_Implicit(33));
		num *= val3;
		CValuePercentBonus val4 = CValuePercentBonus.op_Implicit(skillAddPercent);
		if (skillId < 0 && GetChangeTrickAttack())
		{
			val4 += CValuePercentBonus.op_Implicit((int)GlobalConfig.Instance.AttackChangeTrickHitValueAddPercent[weapon.GetAttackPreparePointCost()]);
		}
		num *= val4;
		num = DomainManager.SpecialEffect.ModifyData(id, skillId, fieldId, num);
		num = DomainManager.SpecialEffect.ModifyData(id2, skillId, fieldId2, num);
		return (int)Math.Clamp(num, 0L, 2147483647L);
	}

	public int GetAvoidValue(CombatContext context, sbyte hitType)
	{
		return GetAvoidValue(hitType, context.BodyPart, context.SkillTemplateId);
	}

	public int GetAvoidValue(sbyte hitType, sbyte bodyPart = -1, short skillId = -1, bool ignoreDefendSkill = false)
	{
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0108: Unknown result type (might be due to invalid IL or missing references)
		//IL_010d: Unknown result type (might be due to invalid IL or missing references)
		//IL_010f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0114: Unknown result type (might be due to invalid IL or missing references)
		//IL_0119: Unknown result type (might be due to invalid IL or missing references)
		//IL_011b: Unknown result type (might be due to invalid IL or missing references)
		//IL_012a: Unknown result type (might be due to invalid IL or missing references)
		//IL_012f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0131: Unknown result type (might be due to invalid IL or missing references)
		//IL_0136: Unknown result type (might be due to invalid IL or missing references)
		//IL_013b: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_0146: Unknown result type (might be due to invalid IL or missing references)
		//IL_014b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0150: Unknown result type (might be due to invalid IL or missing references)
		//IL_0155: Unknown result type (might be due to invalid IL or missing references)
		//IL_0157: Unknown result type (might be due to invalid IL or missing references)
		//IL_0166: Unknown result type (might be due to invalid IL or missing references)
		//IL_016b: Unknown result type (might be due to invalid IL or missing references)
		//IL_016d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0172: Unknown result type (might be due to invalid IL or missing references)
		//IL_0177: Unknown result type (might be due to invalid IL or missing references)
		//IL_0179: Unknown result type (might be due to invalid IL or missing references)
		//IL_0188: Unknown result type (might be due to invalid IL or missing references)
		//IL_018d: Unknown result type (might be due to invalid IL or missing references)
		//IL_018f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0194: Unknown result type (might be due to invalid IL or missing references)
		//IL_0199: Unknown result type (might be due to invalid IL or missing references)
		int num = _character.GetAvoidValues()[hitType];
		CombatCharacter combatCharacter = _combatDomain.GetCombatCharacter(!IsAlly);
		num *= CalcArmorAvoidFactor(hitType, bodyPart, skillId);
		bool flag = DomainManager.SpecialEffect.ModifyData(_id, -1, 42, dataValue: true, hitType, 0);
		bool flag2 = DomainManager.SpecialEffect.ModifyData(_id, -1, 42, dataValue: true, hitType, 1);
		CValuePercentBonus val = CValuePercentBonus.op_Implicit(DomainManager.SpecialEffect.GetModifyValue(_id, 43, (EDataModifyType)0, hitType, 0, -1, (EDataSumType)0));
		CValuePercentBonus val2 = CValuePercentBonus.op_Implicit(DomainManager.SpecialEffect.GetModifyValue(_id, 43, (EDataModifyType)0, hitType, 1, -1, (EDataSumType)0));
		int id = combatCharacter.GetId();
		int id2 = _id;
		ushort fieldId = (ushort)(60 + hitType);
		ushort fieldId2 = (ushort)(94 + hitType);
		CValueModify val3 = CValueModify.Zero;
		if (flag)
		{
			val3 = ((CValueModify)(ref val3)).ChangeA(CalcDefendSkillAddAvoidValue(hitType, ignoreDefendSkill));
			val3 += CalcTeammateAvoidModify(hitType, (EDataSumType)1);
			val3 += DomainManager.SpecialEffect.GetModify(id, fieldId, skillId, -1, -1, (EDataSumType)1) * val;
			val3 += DomainManager.SpecialEffect.GetModify(id2, fieldId2, skillId, -1, -1, (EDataSumType)1) * val;
		}
		if (flag2)
		{
			val3 += CalcTeammateAvoidModify(hitType, (EDataSumType)2);
			val3 += DomainManager.SpecialEffect.GetModify(id, fieldId, skillId, -1, -1, (EDataSumType)2) * val2;
			val3 += DomainManager.SpecialEffect.GetModify(id2, fieldId2, skillId, -1, -1, (EDataSumType)2) * val2;
		}
		val3 = ((CValueModify)(ref val3)).MaxB(CValuePercent.op_Implicit(33));
		num *= val3;
		num = DomainManager.SpecialEffect.ModifyData(id, skillId, fieldId, num);
		num = DomainManager.SpecialEffect.ModifyData(id2, skillId, fieldId2, num);
		return Math.Max(num, 1);
	}

	public OuterAndInnerInts GetPenetrate(CombatContext context)
	{
		GameData.Domains.Item.Weapon weapon = context.Weapon;
		sbyte bodyPart = context.BodyPart;
		short skillTemplateId = context.SkillTemplateId;
		GameData.Domains.CombatSkill.CombatSkill skill = context.Skill;
		int penetrate = GetPenetrate(inner: false, weapon, bodyPart, skillTemplateId, skill?.GetPenetrations().Outer ?? 0);
		int penetrate2 = GetPenetrate(inner: true, weapon, bodyPart, skillTemplateId, skill?.GetPenetrations().Inner ?? 0);
		return new OuterAndInnerInts(penetrate, penetrate2);
	}

	public int GetPenetrate(bool inner, GameData.Domains.Item.Weapon weapon, sbyte bodyPart, short skillId = -1, int skillAddPercent = 0)
	{
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		long num = (inner ? _character.GetPenetrations().Inner : _character.GetPenetrations().Outer);
		num *= CalcWeaponPenetrateFactor(weapon, inner, bodyPart, skillId) + CValuePercent.op_Implicit(skillAddPercent);
		CombatCharacter combatCharacter = _combatDomain.GetCombatCharacter(!IsAlly, tryGetCoverCharacter: true);
		int id = _id;
		int id2 = combatCharacter.GetId();
		ushort fieldId = (ushort)(inner ? 65 : 64);
		ushort fieldId2 = (ushort)(inner ? 99 : 98);
		CValueModify val = CalcTeammatePenetrateModify();
		val += DomainManager.SpecialEffect.GetModify(id, fieldId, skillId, bodyPart, -1, (EDataSumType)0);
		val += DomainManager.SpecialEffect.GetModify(id2, fieldId2, skillId, bodyPart, -1, (EDataSumType)0);
		val = ((CValueModify)(ref val)).MaxB(CValuePercent.op_Implicit(33));
		num *= val;
		num = DomainManager.SpecialEffect.ModifyData(id, skillId, fieldId, num);
		num = DomainManager.SpecialEffect.ModifyData(id2, skillId, fieldId2, num);
		return (int)Math.Clamp(num, 0L, 2147483647L);
	}

	public OuterAndInnerInts GetPenetrateResist(CombatContext context)
	{
		GameData.Domains.Item.Weapon weapon = context.Weapon;
		sbyte bodyPart = context.BodyPart;
		short skillTemplateId = context.SkillTemplateId;
		int penetrateResist = GetPenetrateResist(inner: false, weapon, bodyPart, skillTemplateId);
		int penetrateResist2 = GetPenetrateResist(inner: true, weapon, bodyPart, skillTemplateId);
		return new OuterAndInnerInts(penetrateResist, penetrateResist2);
	}

	public int GetPenetrateResist(bool inner, GameData.Domains.Item.Weapon weapon, sbyte bodyPart, short skillId = -1, bool ignoreDefendSkill = false)
	{
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		long num = (inner ? _character.GetPenetrationResists().Inner : _character.GetPenetrationResists().Outer);
		num *= CalcArmorPenetrateResistFactor(weapon, inner, bodyPart, skillId);
		CombatCharacter combatCharacter = _combatDomain.GetCombatCharacter(!IsAlly);
		int id = combatCharacter.GetId();
		int id2 = _id;
		ushort fieldId = (ushort)(inner ? 67 : 66);
		ushort fieldId2 = (ushort)(inner ? 101 : 100);
		CValueModify val = CalcTeammatePenetrateResistModify();
		val = ((CValueModify)(ref val)).ChangeA(CalcDefendSkillAddPenetrateResistValue(inner, ignoreDefendSkill));
		val += DomainManager.SpecialEffect.GetModify(id, fieldId, skillId, bodyPart, -1, (EDataSumType)0);
		val += DomainManager.SpecialEffect.GetModify(id2, fieldId2, skillId, bodyPart, -1, (EDataSumType)0);
		val = ((CValueModify)(ref val)).MaxB(CValuePercent.op_Implicit(33));
		num *= val;
		num = DomainManager.SpecialEffect.ModifyData(id, skillId, fieldId, num);
		num = DomainManager.SpecialEffect.ModifyData(id2, skillId, fieldId2, num);
		return (int)Math.Clamp(num, 1L, 2147483647L);
	}

	public sbyte GetOrRandomChangeTrickType(IRandomSource random)
	{
		sbyte[] weaponTricks = GetWeaponTricks();
		if (PlayerChangeTrickType >= 0 && weaponTricks.Exist(PlayerChangeTrickType))
		{
			return PlayerChangeTrickType;
		}
		return weaponTricks.GetRandom(random);
	}

	public sbyte RandomChangeTrickBodyPart(IRandomSource random, sbyte trickType, short skillId = -1)
	{
		CombatCharacter combatCharacter = DomainManager.Combat.GetCombatCharacter(!IsAlly);
		CombatDomain combat = DomainManager.Combat;
		sbyte trickType2 = trickType;
		return combat.GetAttackBodyPart(this, combatCharacter, random, skillId, trickType2, -1);
	}

	public sbyte RandomChangeTrickBodyPartByNeiliType(IRandomSource random, sbyte trickType)
	{
		CombatCharacter combatCharacter = DomainManager.Combat.GetCombatCharacter(!IsAlly);
		Dictionary<SkillEffectKey, short> effectDict = GetSkillEffectCollection().EffectDict;
		if (effectDict != null && effectDict.Keys.Any((SkillEffectKey x) => x.EffectConfig.TransferProportion > 0))
		{
			byte fiveElements = NeiliType.Instance[combatCharacter.OriginNeiliType].FiveElements;
			if (fiveElements != 5)
			{
				return BodyPartType.TransferFromFiveElementsType(FiveElementsType.Countered[fiveElements]);
			}
			byte fiveElements2 = NeiliType.Instance[GetNeiliType()].FiveElements;
			if (fiveElements2 != 5)
			{
				return BodyPartType.TransferFromFiveElementsType(FiveElementsType.Countering[fiveElements2]);
			}
		}
		return DomainManager.Combat.GetAttackBodyPart(this, combatCharacter, random, -1, trickType, -1);
	}

	public sbyte RandomInjuryBodyPart(IRandomSource random, bool inner, IEnumerable<sbyte> partRange = null)
	{
		List<sbyte> list = ObjectPool<List<sbyte>>.Instance.Get();
		list.Clear();
		if (partRange == null)
		{
			partRange = GetAvailableBodyParts();
		}
		foreach (sbyte item in partRange)
		{
			if (_injuries.Get(item, inner) < 6)
			{
				list.Add(item);
			}
		}
		sbyte result = (sbyte)((list.Count > 0) ? list.GetRandom(random) : (-1));
		ObjectPool<List<sbyte>>.Instance.Return(list);
		return result;
	}

	public sbyte RandomInjuryBodyPartMustValid(IRandomSource random, bool inner, IEnumerable<sbyte> partRange = null)
	{
		List<sbyte> list = ObjectPool<List<sbyte>>.Instance.Get();
		if (partRange == null)
		{
			partRange = GetAvailableBodyParts();
		}
		list.AddRange(partRange);
		sbyte b = RandomInjuryBodyPart(random, inner, list);
		if (b < 0)
		{
			b = list.GetRandom(random);
		}
		ObjectPool<List<sbyte>>.Instance.Return(list);
		return b;
	}

	public bool AllRawCreateSlotsBlocked(int effectId)
	{
		SpecialEffectItem specialEffectItem = Config.SpecialEffect.Instance[effectId];
		return !GetAllCanRawCreateEquipmentSlots(specialEffectItem.RawCreateType).Any();
	}

	public IEnumerable<sbyte> GetAllCanRawCreateEquipmentSlots(ESpecialEffectRawCreateType type)
	{
		if (1 == 0)
		{
		}
		int num = type switch
		{
			ESpecialEffectRawCreateType.Sword => 0, 
			ESpecialEffectRawCreateType.Blade => 0, 
			ESpecialEffectRawCreateType.Polearm => 0, 
			ESpecialEffectRawCreateType.Armor => 1, 
			ESpecialEffectRawCreateType.Accessory => 2, 
			_ => -1, 
		};
		if (1 == 0)
		{
		}
		int itemType = num;
		if (1 == 0)
		{
		}
		num = type switch
		{
			ESpecialEffectRawCreateType.Sword => 8, 
			ESpecialEffectRawCreateType.Blade => 9, 
			ESpecialEffectRawCreateType.Polearm => 10, 
			ESpecialEffectRawCreateType.Armor => -1, 
			ESpecialEffectRawCreateType.Accessory => -1, 
			_ => -1, 
		};
		if (1 == 0)
		{
		}
		int itemSubType = num;
		ItemKey[] equipments = _character.GetEquipment();
		foreach (sbyte rawCreateSlot in SharedConstValue.AllRawCreateSlots)
		{
			ItemKey equipment = equipments[rawCreateSlot];
			if (!equipment.IsValid() || (itemType >= 0 && itemType != equipment.ItemType) || _rawCreateCollection.Contains(equipment))
			{
				continue;
			}
			short equipmentSubType = ItemTemplateHelper.GetItemSubType(equipment.ItemType, equipment.TemplateId);
			if (itemSubType < 0 || itemSubType == equipmentSubType)
			{
				ItemBase baseItem = DomainManager.Item.GetBaseItem(equipment);
				if (baseItem.GetCurrDurability() > 0)
				{
					yield return rawCreateSlot;
				}
			}
		}
	}

	public void InvokeRawCreate(DataContext context, int effectId)
	{
		if ((!IsAlly || !DomainManager.Combat.GetAutoCombat() || !DomainManager.Combat.AiOptions.AutoUnlock || !DomainManager.Combat.AiOptions.SkipRawCreate) && !_rawCreateEffects.Contains(effectId))
		{
			SpecialEffectItem specialEffectItem = Config.SpecialEffect.Instance[effectId];
			if (GetAllCanRawCreateEquipmentSlots(specialEffectItem.RawCreateType).Any())
			{
				_rawCreateEffects.Add(effectId);
				SetRawCreateEffects(_rawCreateEffects, context);
			}
		}
	}

	public void IgnoreRawCreate(DataContext context, int effectId)
	{
		if (_rawCreateEffects.Remove(effectId))
		{
			SetRawCreateEffects(_rawCreateEffects, context);
		}
	}

	public void IgnoreAllRawCreate(DataContext context)
	{
		if (_rawCreateEffects.Count != 0)
		{
			_rawCreateEffects.Clear();
			SetRawCreateEffects(_rawCreateEffects, context);
		}
	}

	public void AutoAllRawCreate(DataContext context)
	{
		if (_rawCreateEffects.Count == 0)
		{
			return;
		}
		ItemKey[] equipment = _character.GetEquipment();
		List<sbyte> list = ObjectPool<List<sbyte>>.Instance.Get();
		for (int num = _rawCreateEffects.Count - 1; num >= 0; num--)
		{
			int num2 = _rawCreateEffects[num];
			SpecialEffectItem specialEffectItem = Config.SpecialEffect.Instance[num2];
			list.Clear();
			foreach (sbyte allCanRawCreateEquipmentSlot in GetAllCanRawCreateEquipmentSlots(specialEffectItem.RawCreateType))
			{
				if (ItemTemplateHelper.GetAllowRawCreate(equipment[allCanRawCreateEquipmentSlot].ItemType, equipment[allCanRawCreateEquipmentSlot].TemplateId))
				{
					list.Add(allCanRawCreateEquipmentSlot);
				}
			}
			if (list.Count > 0)
			{
				sbyte random = list.GetRandom(context.Random);
				DoRawCreate(context, num2, random, equipment[random].TemplateId);
			}
			else
			{
				IgnoreRawCreate(context, num2);
			}
		}
		ObjectPool<List<sbyte>>.Instance.Return(list);
	}

	public bool DoRawCreate(DataContext context, int effectId, sbyte equipmentSlot, short newTemplateId)
	{
		if (!SharedConstValue.AllRawCreateSlots.Contains(equipmentSlot))
		{
			return false;
		}
		ItemKey[] equipment = _character.GetEquipment();
		ItemKey itemKey = equipment[equipmentSlot];
		if (!itemKey.IsValid() || _rawCreateCollection.Contains(itemKey))
		{
			return false;
		}
		sbyte itemType = itemKey.ItemType;
		if (1 == 0)
		{
		}
		int num = itemType switch
		{
			0 => Config.Weapon.Instance.Count, 
			1 => Config.Armor.Instance.Count, 
			2 => Config.Accessory.Instance.Count, 
			_ => -1, 
		};
		if (1 == 0)
		{
		}
		int num2 = num;
		if (newTemplateId < 0 || newTemplateId >= num2)
		{
			return false;
		}
		Inventory inventory = _character.GetInventory();
		short rawCreateMaterial = ItemTemplateHelper.GetRawCreateMaterial(itemKey.ItemType, itemKey.TemplateId, newTemplateId);
		int num3 = ((rawCreateMaterial >= 0) ? inventory.GetInventoryItemCount(5, rawCreateMaterial) : 0);
		int rawCreateRequireMaterialCount = Config.SpecialEffect.Instance[effectId].RawCreateRequireMaterialCount;
		if (rawCreateMaterial >= 0 && num3 < rawCreateRequireMaterialCount)
		{
			return false;
		}
		if (!_rawCreateEffects.Remove(effectId))
		{
			return false;
		}
		SetRawCreateEffects(_rawCreateEffects, context);
		if (rawCreateMaterial >= 0)
		{
			_character.RemoveMultiInventoryItem(context, 5, rawCreateMaterial, rawCreateRequireMaterialCount);
		}
		ItemKey newKey = DomainManager.Item.CreateItem(context, itemKey.ItemType, newTemplateId);
		newKey = (equipment[equipmentSlot] = CopyEquipmentData(context, itemKey, newKey));
		_character.SetEquipment(equipment, context);
		SpecialEffectItem specialEffectItem = Config.SpecialEffect.Instance[effectId];
		short rawCreateEffect = specialEffectItem.RawCreateEffect;
		DomainManager.Item.AddExternEquipmentEffect(context, newKey, rawCreateEffect);
		long specialEffectId = DomainManager.SpecialEffect.AddEquipmentEffect(context, _id, newKey, rawCreateEffect);
		ItemBase baseItem = DomainManager.Item.GetBaseItem(newKey);
		DomainManager.Combat.EquipmentOldDurability[newKey] = baseItem.GetCurrDurability();
		_rawCreateCollection.Add(newKey, itemKey, effectId, specialEffectId);
		SetRawCreateCollection(_rawCreateCollection, context);
		DomainManager.Combat.ShowSpecialEffectTips(_id, specialEffectItem.RawCreateTips, 0);
		switch (itemKey.ItemType)
		{
		case 0:
		{
			_weapons[equipmentSlot] = newKey;
			SetWeapons(_weapons, context);
			DomainManager.Combat.InitWeaponData(context, this, equipmentSlot);
			CombatWeaponData element_WeaponDataDict = DomainManager.Combat.GetElement_WeaponDataDict(newKey.Id);
			if (equipmentSlot == _usingWeaponIndex)
			{
				SetWeaponTricks(element_WeaponDataDict.GetWeaponTricks(), context);
			}
			return true;
		}
		case 1:
		{
			for (sbyte b = 0; b < 7; b++)
			{
				if (EquipmentSlotHelper.GetSlotByBodyPartType(b) == equipmentSlot)
				{
					Armors[b] = newKey;
				}
			}
			return true;
		}
		default:
			return true;
		}
	}

	private ItemKey CopyEquipmentData(DataContext context, ItemKey oldKey, ItemKey newKey)
	{
		EquipmentBase baseEquipment = DomainManager.Item.GetBaseEquipment(oldKey);
		EquipmentBase baseEquipment2 = DomainManager.Item.GetBaseEquipment(newKey);
		ItemBase itemBase = baseEquipment2;
		baseEquipment2.ApplyDurabilityEquipmentEffectChange(context, baseEquipment2.GetEquipmentEffectId(), baseEquipment.GetEquipmentEffectId());
		baseEquipment2.SetEquipmentEffectId(baseEquipment.GetEquipmentEffectId(), context);
		baseEquipment2.SetCurrDurability(baseEquipment2.GetMaxDurability(), context);
		if (ModificationStateHelper.IsActive(oldKey.ModificationState, 2))
		{
			RefiningEffects refinedEffects = DomainManager.Item.GetRefinedEffects(oldKey);
			ItemBase baseItem = DomainManager.Item.GetBaseItem(newKey);
			itemBase = DomainManager.Item.SetRefinedEffects(context, baseItem, refinedEffects);
			newKey = itemBase.GetItemKey();
		}
		if (ModificationStateHelper.IsActive(oldKey.ModificationState, 1))
		{
			FullPoisonEffects poisonEffects = DomainManager.Item.GetPoisonEffects(oldKey);
			itemBase = DomainManager.Item.SetAttachedPoisons(context, itemBase, poisonEffects);
			newKey = itemBase.GetItemKey();
		}
		return newKey;
	}

	public void RevertRawCreate(DataContext context, ItemKey newKey)
	{
		ItemKey[] equipment = _character.GetEquipment();
		int num = equipment.IndexOf(newKey);
		Tester.Assert(num >= 0, "equipmentSlot >= 0");
		int index = _rawCreateCollection.Effects[newKey];
		long effectId = _rawCreateCollection.SpecialEffects[newKey];
		short rawCreateEffect = Config.SpecialEffect.Instance[index].RawCreateEffect;
		_rawCreateCollection.Remove(newKey, out var oldKey);
		SetRawCreateCollection(_rawCreateCollection, context);
		equipment[num] = oldKey;
		_character.SetEquipment(equipment, context);
		DomainManager.Item.RemoveItem(context, newKey);
		DomainManager.Item.RemoveExternEquipmentEffect(context, newKey, rawCreateEffect);
		DomainManager.SpecialEffect.Remove(context, effectId);
		int num2 = _weapons.IndexOf(newKey);
		if (num2 >= 0)
		{
			DomainManager.Combat.RemoveWeaponData(newKey);
			_weapons[num2] = oldKey;
			CombatWeaponData element_WeaponDataDict = DomainManager.Combat.GetElement_WeaponDataDict(oldKey.Id);
			SetWeaponTricks(element_WeaponDataDict.GetWeaponTricks(), context);
			SetWeapons(_weapons, context);
		}
		for (sbyte b = 0; b < 7; b++)
		{
			if (EquipmentSlotHelper.GetSlotByBodyPartType(b) == num)
			{
				Armors[b] = oldKey;
			}
		}
	}

	public void RevertAllRawCreates(DataContext context)
	{
		ItemKey[] equipment = _character.GetEquipment();
		foreach (sbyte allRawCreateSlot in SharedConstValue.AllRawCreateSlots)
		{
			if (_rawCreateCollection.Contains(equipment[allRawCreateSlot]))
			{
				equipment[allRawCreateSlot] = _rawCreateCollection.Sources[equipment[allRawCreateSlot]];
			}
		}
		if (_rawCreateCollection.Any())
		{
			_character.SetEquipment(equipment, context);
		}
		foreach (ItemKey key in _rawCreateCollection.Effects.Keys)
		{
			DomainManager.Item.RemoveItem(context, key);
		}
		_rawCreateCollection.Clear();
	}

	public void CreateGangqi(DataContext context, int value)
	{
		if (value > _gangqiMax)
		{
			SetGangqi(value, context);
			SetGangqiMax(value, context);
		}
	}

	public void ChangeGangqi(DataContext context, int delta)
	{
		int gangqi = Math.Clamp(_gangqi + delta, 0, _gangqiMax);
		SetGangqi(gangqi, context);
	}

	public bool CanExecuteTeammateCommandImmediate(ETeammateCommandImplement implement)
	{
		if (DomainManager.Combat.Pause)
		{
			return false;
		}
		ECombatReserveType type = _combatReserveData.Type;
		if ((type != ECombatReserveType.Invalid && type != ECombatReserveType.TeammateCommand) || 1 == 0)
		{
			return false;
		}
		CombatReserveData combatReserveData = _combatReserveData;
		_combatReserveData = CombatReserveData.Invalid;
		bool flag = HasDoingOrReserveCommand();
		_combatReserveData = combatReserveData;
		if (!flag)
		{
			return true;
		}
		if (StateMachine.GetCurrentStateType() == CombatCharacterStateType.TeammateCommand)
		{
			return true;
		}
		bool flag2 = _preparingSkillId >= 0;
		bool flag3 = flag2;
		if (flag3)
		{
			bool flag4 = ((implement == ETeammateCommandImplement.AccelerateCast || implement == ETeammateCommandImplement.InterruptSkill) ? true : false);
			flag3 = flag4;
		}
		if (flag3)
		{
			return true;
		}
		if ((_preparingOtherAction >= 0 || _preparingItem.IsValid()) && implement == ETeammateCommandImplement.InterruptOtherAction)
		{
			return true;
		}
		return false;
	}

	public bool CanExecuteReserveTeammateCommand()
	{
		if (_combatReserveData.Type != ECombatReserveType.TeammateCommand)
		{
			return false;
		}
		CombatCharacter element_CombatCharacterDict = DomainManager.Combat.GetElement_CombatCharacterDict(_combatReserveData.TeammateCharId);
		sbyte index = (sbyte)(_showTransferInjuryCommand ? 13 : element_CombatCharacterDict.GetCurrTeammateCommands()[_combatReserveData.TeammateCmdIndex]);
		return CanExecuteTeammateCommandImmediate(TeammateCommand.Instance[index].Implement);
	}

	public bool ExecuteTeammateCommandImmediate(DataContext context, int charId, int index)
	{
		CombatCharacter element_CombatCharacterDict = DomainManager.Combat.GetElement_CombatCharacterDict(charId);
		if (element_CombatCharacterDict.IsAlly != IsAlly || !DomainManager.Combat.IsMainCharacter(this))
		{
			return false;
		}
		sbyte b = (sbyte)(_showTransferInjuryCommand ? 13 : element_CombatCharacterDict.GetCurrTeammateCommands()[index]);
		TeammateCommandItem teammateCommandItem = TeammateCommand.Instance[b];
		ETeammateCommandImplement implement = teammateCommandItem.Implement;
		element_CombatCharacterDict.ExecutingTeammateCommandConfig = teammateCommandItem;
		switch (implement)
		{
		case ETeammateCommandImplement.Fight:
			ChangeCharId = charId;
			element_CombatCharacterDict.ExecutingTeammateCommandIndex = index;
			DomainManager.Combat.ClearAllWeaponCd(context, element_CombatCharacterDict);
			break;
		case ETeammateCommandImplement.AttackSkill:
		{
			ChangeCharId = charId;
			element_CombatCharacterDict.ExecutingTeammateCommandIndex = index;
			DomainManager.Combat.CastSkillFree(context, element_CombatCharacterDict, element_CombatCharacterDict.GetAttackCommandSkillId());
			int bestWeaponIndex = element_CombatCharacterDict.AiController.GetBestWeaponIndex(context.Random, element_CombatCharacterDict.NeedUseSkillFreeId);
			if (element_CombatCharacterDict.GetUsingWeaponIndex() != bestWeaponIndex)
			{
				DomainManager.Combat.ChangeWeapon(context, element_CombatCharacterDict, bestWeaponIndex);
			}
			OuterAndInnerShorts attackRange = element_CombatCharacterDict.GetAttackRange();
			short num2 = (short)((attackRange.Outer + attackRange.Inner) / 2);
			int num3 = num2 - DomainManager.Combat.GetCurrentDistance();
			if (DomainManager.Combat.ChangeDistance(context, this, num3))
			{
				element_CombatCharacterDict.ExecutingTeammateCommandChangeDistance = -num3;
			}
			DomainManager.Combat.ClearAllWeaponCd(context, element_CombatCharacterDict);
			break;
		}
		default:
			element_CombatCharacterDict.SetExecutingTeammateCommand(b, context);
			element_CombatCharacterDict.TeammateCommandLeftPrepareFrame = (element_CombatCharacterDict.TeammateCommandTotalPrepareFrame = element_CombatCharacterDict.ExecutingTeammateCommandConfig.PrepareFrame);
			TeammateHasCommand[DomainManager.Combat.GetCharacterList(IsAlly).IndexOf(charId) - 1] = true;
			if (element_CombatCharacterDict.ExecutingTeammateCommandConfig.IntoCombatField)
			{
				switch (implement)
				{
				case ETeammateCommandImplement.HealInjury:
					element_CombatCharacterDict.TeammateCommandLeftPrepareFrame = (element_CombatCharacterDict.TeammateCommandTotalPrepareFrame = element_CombatCharacterDict.GetOtherActionPrepareFrame(0));
					break;
				case ETeammateCommandImplement.HealPoison:
					element_CombatCharacterDict.TeammateCommandLeftPrepareFrame = (element_CombatCharacterDict.TeammateCommandTotalPrepareFrame = element_CombatCharacterDict.GetOtherActionPrepareFrame(1));
					break;
				case ETeammateCommandImplement.Attack:
				{
					ItemKey attackCommandWeaponKey = element_CombatCharacterDict.GetAttackCommandWeaponKey();
					int num = element_CombatCharacterDict.GetWeapons().IndexOf(attackCommandWeaponKey);
					if (element_CombatCharacterDict.GetUsingWeaponIndex() != num)
					{
						DomainManager.Combat.ChangeWeapon(context, element_CombatCharacterDict, num);
					}
					break;
				}
				}
			}
			if (element_CombatCharacterDict.ExecutingTeammateCommandConfig.AffectFrame >= 0)
			{
				element_CombatCharacterDict.ExecutingTeammateCommandIndex = index;
			}
			else if (element_CombatCharacterDict.CheckResetTeammateCommandCd(teammateCommandItem))
			{
				element_CombatCharacterDict.ResetTeammateCommandCd(context, index, -1, checkEvent: true);
			}
			break;
		}
		element_CombatCharacterDict.SetShowEffectCommandIndex((sbyte)index, context);
		if ((implement == ETeammateCommandImplement.Fight || (uint)(implement - 2) <= 1u) ? true : false)
		{
			MoveData.ResetJumpState(context);
		}
		bool result = UpdateTeammateCharStatus(context);
		DomainManager.Combat.UpdateAllCommandAvailability(context, this);
		return result;
	}

	public bool ExecuteReserveTeammateCommand(DataContext context)
	{
		if (!CanExecuteReserveTeammateCommand())
		{
			return false;
		}
		int teammateCharId = _combatReserveData.TeammateCharId;
		int teammateCmdIndex = _combatReserveData.TeammateCmdIndex;
		SetCombatReserveData(CombatReserveData.Invalid, context);
		CombatCharacter element_CombatCharacterDict = DomainManager.Combat.GetElement_CombatCharacterDict(teammateCharId);
		if (!element_CombatCharacterDict.GetTeammateCommandCanUse()[teammateCmdIndex])
		{
			return false;
		}
		return ExecuteTeammateCommandImmediate(context, teammateCharId, teammateCmdIndex);
	}

	private bool CheckResetTeammateCommandCd(TeammateCommandItem commandConfig)
	{
		if (commandConfig.Type != ETeammateCommandType.Advance)
		{
			return true;
		}
		switch (commandConfig.Implement)
		{
		case ETeammateCommandImplement.Push:
			if (NeedResetAdvanceTeammateCommandPushCd)
			{
				NeedResetAdvanceTeammateCommandPushCd = false;
				return true;
			}
			NeedResetAdvanceTeammateCommandPushCd = true;
			return false;
		case ETeammateCommandImplement.Pull:
			if (NeedResetAdvanceTeammateCommandPullCd)
			{
				NeedResetAdvanceTeammateCommandPullCd = false;
				return true;
			}
			NeedResetAdvanceTeammateCommandPullCd = true;
			return false;
		default:
			return true;
		}
	}

	public bool TrickEquals(sbyte trick1, sbyte trick2)
	{
		if (trick1 == trick2)
		{
			return true;
		}
		return InterchangeableTricks.Contains(trick1) && InterchangeableTricks.Contains(trick2) && _weaponTricks.Exist(InterchangeableTricks.Contains);
	}

	public bool IsTrickUsable(sbyte trickType)
	{
		if (trickType == 21)
		{
			return true;
		}
		sbyte[] weaponTricks = _combatDomain.GetUsingWeaponData(this).GetWeaponTricks();
		if (weaponTricks.Contains(trickType))
		{
			return true;
		}
		return InterchangeableTricks.Contains(trickType) && InterchangeableTricks.Any(((IEnumerable<sbyte>)weaponTricks).Contains<sbyte>);
	}

	public bool IsTrickUseless(sbyte trickType)
	{
		return !IsTrickUsable(trickType);
	}

	public int ReplaceUsableTrick(DataContext context, sbyte trickType, int count = -1)
	{
		if (count == 0)
		{
			return 0;
		}
		IReadOnlyDictionary<int, sbyte> tricks = _tricks.Tricks;
		List<int> list = ObjectPool<List<int>>.Instance.Get();
		list.Clear();
		list.AddRange(tricks.Where(delegate(KeyValuePair<int, sbyte> kvp)
		{
			KeyValuePair<int, sbyte> keyValuePair = kvp;
			int result;
			if (keyValuePair.Value != trickType)
			{
				CombatCharacter combatCharacter = this;
				keyValuePair = kvp;
				result = (combatCharacter.IsTrickUsable(keyValuePair.Value) ? 1 : 0);
			}
			else
			{
				result = 0;
			}
			return (byte)result != 0;
		}).Select(delegate(KeyValuePair<int, sbyte> kvp)
		{
			KeyValuePair<int, sbyte> keyValuePair = kvp;
			return keyValuePair.Key;
		}));
		int num = ((count > 0) ? (list.Count - Math.Min(count, list.Count)) : 0);
		for (int num2 = 0; num2 < num; num2++)
		{
			CollectionUtils.SwapAndRemove(list, context.Random.Next(list.Count));
		}
		foreach (int item in list)
		{
			_tricks.ReplaceTrick(item, trickType);
		}
		int count2 = list.Count;
		ObjectPool<List<int>>.Instance.Return(list);
		SetTricks(_tricks, context);
		return count2;
	}

	public byte GetTrickCount(sbyte type)
	{
		return GetTrickCount(type, useTrickEquals: false);
	}

	public byte GetTrickCount(sbyte type, bool useTrickEquals)
	{
		byte b = 0;
		foreach (sbyte value in _tricks.Tricks.Values)
		{
			if (useTrickEquals ? TrickEquals(value, type) : (value == type))
			{
				b++;
			}
		}
		return b;
	}

	public sbyte GetTrickAtStart()
	{
		using (IEnumerator<sbyte> enumerator = _tricks.Tricks.Values.GetEnumerator())
		{
			if (enumerator.MoveNext())
			{
				return enumerator.Current;
			}
		}
		return -1;
	}

	public int GetContinueTricksAtStart(sbyte trickType)
	{
		int num = 0;
		foreach (sbyte value in _tricks.Tricks.Values)
		{
			if (value == trickType)
			{
				num++;
				continue;
			}
			break;
		}
		return (num > 1) ? num : 0;
	}

	public int GetContinueTricks(sbyte trickType, List<int> indexList = null)
	{
		IReadOnlyDictionary<int, sbyte> tricks = _tricks.Tricks;
		int num = 0;
		int num2 = 0;
		List<int> list = ObjectPool<List<int>>.Instance.Get();
		list.Clear();
		foreach (var (item, b2) in tricks)
		{
			if (b2 == trickType)
			{
				num2++;
				list.Add(item);
				if (num2 >= 2 && num2 > num)
				{
					num = num2;
					indexList?.Clear();
					indexList?.AddRange(list);
				}
			}
			else
			{
				num2 = 0;
				list.Clear();
			}
		}
		ObjectPool<List<int>>.Instance.Return(list);
		return num;
	}

	public void InvokeExtraUnlockEffect(IExtraUnlockEffect effect, int weaponIndex)
	{
		if (CanInvokeExtraUnlockEffect(effect, weaponIndex))
		{
			_invokedUnlockEffects.Add(effect);
		}
	}

	public void DoExtraUnlockEffect(DataContext context, int weaponIndex)
	{
		CollectionUtils.Shuffle(context.Random, _invokedUnlockEffects);
		foreach (IExtraUnlockEffect invokedUnlockEffect in _invokedUnlockEffects)
		{
			if (DoExtraUnlockEffectCost(context, invokedUnlockEffect, weaponIndex))
			{
				_costedUnlockEffects.Add(invokedUnlockEffect);
			}
		}
		_invokedUnlockEffects.Clear();
		foreach (IExtraUnlockEffect costedUnlockEffect in _costedUnlockEffects)
		{
			costedUnlockEffect.DoAffectAfterCost(context, weaponIndex);
		}
		_costedUnlockEffects.Clear();
	}

	private bool CanInvokeExtraUnlockEffect(IExtraUnlockEffect effect, int weaponIndex)
	{
		ItemKey itemKey = GetWeapons()[weaponIndex];
		if (!itemKey.IsValid())
		{
			return false;
		}
		short currDurability = DomainManager.Item.GetBaseItem(itemKey).GetCurrDurability();
		int num = (effect.IsDirect ? GetTrickCount(12) : UsableTrickCount);
		num += TryInsteadTrick(effect);
		if (effect.IsDirect)
		{
			return num >= 2 || currDurability > 4;
		}
		return num >= 3 && currDurability > 8;
	}

	private int TryInsteadTrick(IExtraUnlockEffect effect, DataContext context = null)
	{
		if (!effect.IsDirect && IsTrickUsable(12))
		{
			return 0;
		}
		int val = DomainManager.SpecialEffect.ModifyData(_id, -1, (ushort)(effect.IsDirect ? 314 : 313), 0);
		int val2 = (effect.IsDirect ? UselessTrickCount : GetTrickCount(12));
		byte val3 = (byte)(effect.IsDirect ? 2 : 3);
		int num = Math.Min(Math.Min(val, val2), val3);
		if (context == null)
		{
			return num;
		}
		if (effect.IsDirect)
		{
			List<sbyte> list = ObjectPool<List<sbyte>>.Instance.Get();
			list.AddRange(_tricks.Tricks.Values.Where(IsTrickUseless));
			IEnumerable<sbyte> randomUnrepeated = RandomUtils.GetRandomUnrepeated(context.Random, num, list);
			_combatDomain.RemoveTrick(context, this, randomUnrepeated);
			ObjectPool<List<sbyte>>.Instance.Return(list);
			Events.RaiseUselessTrickInsteadJiTricks(context, this, num);
		}
		else
		{
			_combatDomain.RemoveTrick(context, this, 12, (byte)num);
			Events.RaiseJiTrickInsteadCostTricks(context, this, num);
		}
		return num;
	}

	private bool DoExtraUnlockEffectCost(DataContext context, IExtraUnlockEffect effect, int weaponIndex)
	{
		if (!CanInvokeExtraUnlockEffect(effect, weaponIndex))
		{
			return false;
		}
		bool flag = effect.IsDirect && GetTrickCount(12) + TryInsteadTrick(effect) >= 2;
		bool flag2 = !flag || !effect.IsDirect;
		if (flag)
		{
			int num = 2 - TryInsteadTrick(effect, context);
			_combatDomain.RemoveTrick(context, this, 12, (byte)num);
		}
		if (!effect.IsDirect)
		{
			int maxCount = 3 - TryInsteadTrick(effect, context);
			List<sbyte> list = ObjectPool<List<sbyte>>.Instance.Get();
			list.AddRange(_tricks.Tricks.Values.Where(IsTrickUsable));
			IEnumerable<sbyte> randomUnrepeated = RandomUtils.GetRandomUnrepeated(context.Random, maxCount, list);
			_combatDomain.RemoveTrick(context, this, randomUnrepeated);
			ObjectPool<List<sbyte>>.Instance.Return(list);
		}
		ItemKey key = _weapons[weaponIndex];
		int num2 = (effect.IsDirect ? 4 : 8);
		if (flag2)
		{
			_combatDomain.ChangeDurability(context, this, key, -num2, EChangeDurabilitySourceType.Unlock);
		}
		return true;
	}

	public int GetRecoverUnlockAttackValue(ItemKey weaponKey)
	{
		int num = 0;
		foreach (short combatSkillId in GetCombatSkillIds())
		{
			if (!DomainManager.CombatSkill.TryGetElement_CombatSkills((charId: _id, skillId: combatSkillId), out var element))
			{
				continue;
			}
			SpecialEffectItem specialEffectItem = element.TryGetSpecialEffect();
			if (specialEffectItem != null)
			{
				short itemSubType = ItemTemplateHelper.GetItemSubType(weaponKey.ItemType, weaponKey.TemplateId);
				if (itemSubType == specialEffectItem.AddUnlockValueItemSubType)
				{
					num = Math.Max(num, specialEffectItem.AddUnlockValue);
				}
			}
		}
		return num;
	}

	public bool WorsenInjury(DataContext context, sbyte bodyPart, bool inner)
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		return WorsenInjury(context, bodyPart, inner, WorsenConstants.DefaultPercent);
	}

	public bool WorsenInjury(DataContext context, sbyte bodyPart, bool inner, CValuePercent percent)
	{
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		if ((bodyPart < 0 || bodyPart >= 7) ? true : false)
		{
			return false;
		}
		sbyte b = _injuries.Get(bodyPart, inner);
		if (b <= 0)
		{
			return false;
		}
		int num = (inner ? _damageStepCollection.InnerDamageSteps : _damageStepCollection.OuterDamageSteps)[bodyPart];
		int num2 = num * WorsenConstants.WorsenFatalPercent[b - 1] * percent;
		if (num2 > 0)
		{
			_combatDomain.AddFatalDamageValue(context, this, num2, inner ? 1 : 0, bodyPart, -1);
		}
		return num2 > 0;
	}

	public bool WorsenRandomInjury(DataContext context)
	{
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		return WorsenRandomInjury(context, WorsenConstants.DefaultPercent);
	}

	public bool WorsenRandomInjury(DataContext context, CValuePercent percent)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		return WorsenRandomInjury(context, RandomWorsenIsInner(context.Random), percent);
	}

	public bool WorsenRandomInjury(DataContext context, sbyte bodyPart)
	{
		//IL_0004: Unknown result type (might be due to invalid IL or missing references)
		return WorsenRandomInjury(context, bodyPart, WorsenConstants.DefaultPercent);
	}

	public bool WorsenRandomInjury(DataContext context, sbyte bodyPart, CValuePercent percent)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		return WorsenRandomInjury(context, RandomWorsenIsInner(context.Random, bodyPart), percent);
	}

	public bool WorsenRandomInjury(DataContext context, bool inner)
	{
		//IL_0004: Unknown result type (might be due to invalid IL or missing references)
		return WorsenRandomInjury(context, inner, WorsenConstants.DefaultPercent);
	}

	public bool WorsenRandomInjury(DataContext context, bool inner, CValuePercent percent)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		sbyte b = RandomWorsenBodyPart(context.Random, inner);
		return b >= 0 && WorsenInjury(context, b, inner, percent);
	}

	public void WorsenRepeatableInjury(DataContext context, int count)
	{
		//IL_0004: Unknown result type (might be due to invalid IL or missing references)
		WorsenRepeatableInjury(context, count, WorsenConstants.DefaultPercent);
	}

	public void WorsenRepeatableInjury(DataContext context, int count, CValuePercent percent)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		for (int i = 0; i < count; i++)
		{
			WorsenRandomInjury(context, percent);
		}
	}

	public void WorsenRepeatableInjury(DataContext context, bool inner, int count)
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		WorsenRepeatableInjury(context, inner, count, WorsenConstants.DefaultPercent);
	}

	public void WorsenRepeatableInjury(DataContext context, bool inner, int count, CValuePercent percent)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		for (int i = 0; i < count; i++)
		{
			WorsenRandomInjury(context, inner, percent);
		}
	}

	public void WorsenUnrepeatedInjury(DataContext context, int count)
	{
		//IL_0004: Unknown result type (might be due to invalid IL or missing references)
		WorsenUnrepeatedInjury(context, count, WorsenConstants.DefaultPercent);
	}

	public void WorsenUnrepeatedInjury(DataContext context, int count, CValuePercent percent)
	{
		//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
		List<sbyte> list = ObjectPool<List<sbyte>>.Instance.Get();
		List<sbyte> list2 = ObjectPool<List<sbyte>>.Instance.Get();
		for (sbyte b = 0; b < 7; b++)
		{
			if (_injuries.Get(b, isInnerInjury: true) > 0)
			{
				list.Add(b);
			}
			if (_injuries.Get(b, isInnerInjury: false) > 0)
			{
				list2.Add(b);
			}
		}
		for (int i = 0; i < count; i++)
		{
			bool flag = list.Count > 0;
			bool flag2 = list2.Count > 0;
			if (!flag && !flag2)
			{
				break;
			}
			bool flag3 = context.Random.RandomIsInner(flag, flag2);
			sbyte random = (flag3 ? list : list2).GetRandom(context.Random);
			if (flag3)
			{
				list.Remove(random);
			}
			else
			{
				list2.Remove(random);
			}
			WorsenInjury(context, random, flag3, percent);
		}
		ObjectPool<List<sbyte>>.Instance.Return(list);
		ObjectPool<List<sbyte>>.Instance.Return(list2);
	}

	public void WorsenUnrepeatedInjury(DataContext context, bool inner, int count)
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		WorsenUnrepeatedInjury(context, inner, count, WorsenConstants.DefaultPercent);
	}

	public void WorsenUnrepeatedInjury(DataContext context, bool inner, int count, CValuePercent percent)
	{
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		List<sbyte> list = ObjectPool<List<sbyte>>.Instance.Get();
		for (sbyte b = 0; b < 7; b++)
		{
			if (_injuries.Get(b, inner) > 0)
			{
				list.Add(b);
			}
		}
		foreach (sbyte item in RandomUtils.GetRandomUnrepeated(context.Random, count, list))
		{
			WorsenInjury(context, item, inner, percent);
		}
		ObjectPool<List<sbyte>>.Instance.Return(list);
	}

	public bool WorsenAllInjury(DataContext context)
	{
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		return WorsenAllInjury(context, WorsenConstants.DefaultPercent);
	}

	public bool WorsenAllInjury(DataContext context, CValuePercent percent)
	{
		//IL_0004: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		bool flag = WorsenAllInjury(context, inner: true, percent);
		return WorsenAllInjury(context, inner: false, percent) || flag;
	}

	public bool WorsenAllInjury(DataContext context, bool inner)
	{
		//IL_0004: Unknown result type (might be due to invalid IL or missing references)
		return WorsenAllInjury(context, inner, WorsenConstants.DefaultPercent);
	}

	public bool WorsenAllInjury(DataContext context, bool inner, CValuePercent percent)
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		bool flag = false;
		for (sbyte b = 0; b < 7; b++)
		{
			if (_injuries.Get(b, inner) > 0)
			{
				flag = WorsenInjury(context, b, inner, percent) || flag;
			}
		}
		return flag;
	}

	private bool RandomWorsenIsInner(IRandomSource random)
	{
		bool anyInner = _injuries.HasAnyInjury(isInnerInjury: true);
		bool anyOuter = _injuries.HasAnyInjury(isInnerInjury: false);
		return random.RandomIsInner(anyInner, anyOuter);
	}

	private bool RandomWorsenIsInner(IRandomSource random, sbyte bodyPart)
	{
		if ((bodyPart < 0 || bodyPart >= 7) ? true : false)
		{
			return RandomWorsenIsInner(random);
		}
		bool anyInner = _injuries.Get(bodyPart, isInnerInjury: true) > 0;
		bool anyOuter = _injuries.Get(bodyPart, isInnerInjury: false) > 0;
		return random.RandomIsInner(anyInner, anyOuter);
	}

	private sbyte RandomWorsenBodyPart(IRandomSource random, bool inner)
	{
		List<sbyte> list = ObjectPool<List<sbyte>>.Instance.Get();
		for (sbyte b = 0; b < 7; b++)
		{
			if (_injuries.Get(b, inner) > 0)
			{
				list.Add(b);
			}
		}
		sbyte result = (sbyte)((list.Count > 0) ? list.GetRandom(random) : (-1));
		ObjectPool<List<sbyte>>.Instance.Return(list);
		return result;
	}

	public int GetId()
	{
		return _id;
	}

	public int GetBreathValue()
	{
		return _breathValue;
	}

	public unsafe void SetBreathValue(int breathValue, DataContext context)
	{
		_breathValue = breathValue;
		SetModifiedAndInvalidateInfluencedCache(1, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 4u, 4);
			*(int*)ptr = _breathValue;
			ptr += 4;
		}
	}

	public int GetStanceValue()
	{
		return _stanceValue;
	}

	public unsafe void SetStanceValue(int stanceValue, DataContext context)
	{
		_stanceValue = stanceValue;
		SetModifiedAndInvalidateInfluencedCache(2, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 8u, 4);
			*(int*)ptr = _stanceValue;
			ptr += 4;
		}
	}

	public NeiliAllocation GetNeiliAllocation()
	{
		return _neiliAllocation;
	}

	public unsafe void SetNeiliAllocation(NeiliAllocation neiliAllocation, DataContext context)
	{
		_neiliAllocation = neiliAllocation;
		SetModifiedAndInvalidateInfluencedCache(3, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 12u, 8);
			ptr += _neiliAllocation.Serialize(ptr);
		}
	}

	public NeiliAllocation GetOriginNeiliAllocation()
	{
		return _originNeiliAllocation;
	}

	public unsafe void SetOriginNeiliAllocation(NeiliAllocation originNeiliAllocation, DataContext context)
	{
		_originNeiliAllocation = originNeiliAllocation;
		SetModifiedAndInvalidateInfluencedCache(4, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 20u, 8);
			ptr += _originNeiliAllocation.Serialize(ptr);
		}
	}

	public NeiliAllocation GetNeiliAllocationRecoverProgress()
	{
		return _neiliAllocationRecoverProgress;
	}

	public unsafe void SetNeiliAllocationRecoverProgress(NeiliAllocation neiliAllocationRecoverProgress, DataContext context)
	{
		_neiliAllocationRecoverProgress = neiliAllocationRecoverProgress;
		SetModifiedAndInvalidateInfluencedCache(5, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 28u, 8);
			ptr += _neiliAllocationRecoverProgress.Serialize(ptr);
		}
	}

	public short GetOldDisorderOfQi()
	{
		return _oldDisorderOfQi;
	}

	public unsafe void SetOldDisorderOfQi(short oldDisorderOfQi, DataContext context)
	{
		_oldDisorderOfQi = oldDisorderOfQi;
		SetModifiedAndInvalidateInfluencedCache(6, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 36u, 2);
			*(short*)ptr = _oldDisorderOfQi;
			ptr += 2;
		}
	}

	public sbyte GetNeiliType()
	{
		return _neiliType;
	}

	public unsafe void SetNeiliType(sbyte neiliType, DataContext context)
	{
		_neiliType = neiliType;
		SetModifiedAndInvalidateInfluencedCache(7, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 38u, 1);
			*ptr = (byte)_neiliType;
			ptr++;
		}
	}

	public ShowAvoidData GetAvoidToShow()
	{
		return _avoidToShow;
	}

	public unsafe void SetAvoidToShow(ShowAvoidData avoidToShow, DataContext context)
	{
		_avoidToShow = avoidToShow;
		SetModifiedAndInvalidateInfluencedCache(8, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 39u, 4);
			ptr += _avoidToShow.Serialize(ptr);
		}
	}

	public int GetCurrentPosition()
	{
		return _currentPosition;
	}

	public unsafe void SetCurrentPosition(int currentPosition, DataContext context)
	{
		_currentPosition = currentPosition;
		SetModifiedAndInvalidateInfluencedCache(9, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 43u, 4);
			*(int*)ptr = _currentPosition;
			ptr += 4;
		}
	}

	public int GetDisplayPosition()
	{
		return _displayPosition;
	}

	public unsafe void SetDisplayPosition(int displayPosition, DataContext context)
	{
		_displayPosition = displayPosition;
		SetModifiedAndInvalidateInfluencedCache(10, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 47u, 4);
			*(int*)ptr = _displayPosition;
			ptr += 4;
		}
	}

	public int GetMobilityValue()
	{
		return _mobilityValue;
	}

	public unsafe void SetMobilityValue(int mobilityValue, DataContext context)
	{
		_mobilityValue = mobilityValue;
		SetModifiedAndInvalidateInfluencedCache(11, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 51u, 4);
			*(int*)ptr = _mobilityValue;
			ptr += 4;
		}
	}

	public sbyte GetJumpPrepareProgress()
	{
		return _jumpPrepareProgress;
	}

	public unsafe void SetJumpPrepareProgress(sbyte jumpPrepareProgress, DataContext context)
	{
		_jumpPrepareProgress = jumpPrepareProgress;
		SetModifiedAndInvalidateInfluencedCache(12, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 55u, 1);
			*ptr = (byte)_jumpPrepareProgress;
			ptr++;
		}
	}

	public short GetJumpPreparedDistance()
	{
		return _jumpPreparedDistance;
	}

	public unsafe void SetJumpPreparedDistance(short jumpPreparedDistance, DataContext context)
	{
		_jumpPreparedDistance = jumpPreparedDistance;
		SetModifiedAndInvalidateInfluencedCache(13, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 56u, 2);
			*(short*)ptr = _jumpPreparedDistance;
			ptr += 2;
		}
	}

	public short GetMobilityLockEffectCount()
	{
		return _mobilityLockEffectCount;
	}

	public unsafe void SetMobilityLockEffectCount(short mobilityLockEffectCount, DataContext context)
	{
		_mobilityLockEffectCount = mobilityLockEffectCount;
		SetModifiedAndInvalidateInfluencedCache(14, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 58u, 2);
			*(short*)ptr = _mobilityLockEffectCount;
			ptr += 2;
		}
	}

	public float GetJumpChangeDistanceDuration()
	{
		return _jumpChangeDistanceDuration;
	}

	public unsafe void SetJumpChangeDistanceDuration(float jumpChangeDistanceDuration, DataContext context)
	{
		_jumpChangeDistanceDuration = jumpChangeDistanceDuration;
		SetModifiedAndInvalidateInfluencedCache(15, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 60u, 4);
			*(float*)ptr = _jumpChangeDistanceDuration;
			ptr += 4;
		}
	}

	public int GetUsingWeaponIndex()
	{
		return _usingWeaponIndex;
	}

	public unsafe void SetUsingWeaponIndex(int usingWeaponIndex, DataContext context)
	{
		_usingWeaponIndex = usingWeaponIndex;
		SetModifiedAndInvalidateInfluencedCache(16, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 64u, 4);
			*(int*)ptr = _usingWeaponIndex;
			ptr += 4;
		}
	}

	public sbyte[] GetWeaponTricks()
	{
		return _weaponTricks;
	}

	public unsafe void SetWeaponTricks(sbyte[] weaponTricks, DataContext context)
	{
		_weaponTricks = weaponTricks;
		SetModifiedAndInvalidateInfluencedCache(17, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 68u, 6);
			for (int i = 0; i < 6; i++)
			{
				ptr[i] = (byte)_weaponTricks[i];
			}
			ptr += 6;
		}
	}

	public byte GetWeaponTrickIndex()
	{
		return _weaponTrickIndex;
	}

	public unsafe void SetWeaponTrickIndex(byte weaponTrickIndex, DataContext context)
	{
		_weaponTrickIndex = weaponTrickIndex;
		SetModifiedAndInvalidateInfluencedCache(18, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 74u, 1);
			*ptr = _weaponTrickIndex;
			ptr++;
		}
	}

	public ItemKey[] GetWeapons()
	{
		return _weapons;
	}

	public unsafe void SetWeapons(ItemKey[] weapons, DataContext context)
	{
		_weapons = weapons;
		SetModifiedAndInvalidateInfluencedCache(19, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 75u, 56);
			for (int i = 0; i < 7; i++)
			{
				ptr += _weapons[i].Serialize(ptr);
			}
		}
	}

	public sbyte GetAttackingTrickType()
	{
		return _attackingTrickType;
	}

	public unsafe void SetAttackingTrickType(sbyte attackingTrickType, DataContext context)
	{
		_attackingTrickType = attackingTrickType;
		SetModifiedAndInvalidateInfluencedCache(20, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 131u, 1);
			*ptr = (byte)_attackingTrickType;
			ptr++;
		}
	}

	public bool GetCanAttackOutRange()
	{
		return _canAttackOutRange;
	}

	public unsafe void SetCanAttackOutRange(bool canAttackOutRange, DataContext context)
	{
		_canAttackOutRange = canAttackOutRange;
		SetModifiedAndInvalidateInfluencedCache(21, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 132u, 1);
			*ptr = (_canAttackOutRange ? ((byte)1) : ((byte)0));
			ptr++;
		}
	}

	public sbyte GetChangeTrickProgress()
	{
		return _changeTrickProgress;
	}

	public unsafe void SetChangeTrickProgress(sbyte changeTrickProgress, DataContext context)
	{
		_changeTrickProgress = changeTrickProgress;
		SetModifiedAndInvalidateInfluencedCache(22, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 133u, 1);
			*ptr = (byte)_changeTrickProgress;
			ptr++;
		}
	}

	public short GetChangeTrickCount()
	{
		return _changeTrickCount;
	}

	public unsafe void SetChangeTrickCount(short changeTrickCount, DataContext context)
	{
		_changeTrickCount = changeTrickCount;
		SetModifiedAndInvalidateInfluencedCache(23, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 134u, 2);
			*(short*)ptr = _changeTrickCount;
			ptr += 2;
		}
	}

	public bool GetCanChangeTrick()
	{
		return _canChangeTrick;
	}

	public unsafe void SetCanChangeTrick(bool canChangeTrick, DataContext context)
	{
		_canChangeTrick = canChangeTrick;
		SetModifiedAndInvalidateInfluencedCache(24, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 136u, 1);
			*ptr = (_canChangeTrick ? ((byte)1) : ((byte)0));
			ptr++;
		}
	}

	public bool GetChangingTrick()
	{
		return _changingTrick;
	}

	public unsafe void SetChangingTrick(bool changingTrick, DataContext context)
	{
		_changingTrick = changingTrick;
		SetModifiedAndInvalidateInfluencedCache(25, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 137u, 1);
			*ptr = (_changingTrick ? ((byte)1) : ((byte)0));
			ptr++;
		}
	}

	public bool GetChangeTrickAttack()
	{
		return _changeTrickAttack;
	}

	public unsafe void SetChangeTrickAttack(bool changeTrickAttack, DataContext context)
	{
		_changeTrickAttack = changeTrickAttack;
		SetModifiedAndInvalidateInfluencedCache(26, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 138u, 1);
			*ptr = (_changeTrickAttack ? ((byte)1) : ((byte)0));
			ptr++;
		}
	}

	public bool GetIsFightBack()
	{
		return _isFightBack;
	}

	public unsafe void SetIsFightBack(bool isFightBack, DataContext context)
	{
		_isFightBack = isFightBack;
		SetModifiedAndInvalidateInfluencedCache(27, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 139u, 1);
			*ptr = (_isFightBack ? ((byte)1) : ((byte)0));
			ptr++;
		}
	}

	public TrickCollection GetTricks()
	{
		return _tricks;
	}

	public unsafe void SetTricks(TrickCollection tricks, DataContext context)
	{
		_tricks = tricks;
		SetModifiedAndInvalidateInfluencedCache(28, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _tricks.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 0, serializedSize);
			ptr += _tricks.Serialize(ptr);
		}
	}

	public Injuries GetInjuries()
	{
		return _injuries;
	}

	public unsafe void SetInjuries(Injuries injuries, DataContext context)
	{
		_injuries = injuries;
		SetModifiedAndInvalidateInfluencedCache(29, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 140u, 16);
			ptr += _injuries.Serialize(ptr);
		}
	}

	public Injuries GetOldInjuries()
	{
		return _oldInjuries;
	}

	public unsafe void SetOldInjuries(Injuries oldInjuries, DataContext context)
	{
		_oldInjuries = oldInjuries;
		SetModifiedAndInvalidateInfluencedCache(30, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 156u, 16);
			ptr += _oldInjuries.Serialize(ptr);
		}
	}

	public InjuryAutoHealCollection GetInjuryAutoHealCollection()
	{
		return _injuryAutoHealCollection;
	}

	public unsafe void SetInjuryAutoHealCollection(InjuryAutoHealCollection injuryAutoHealCollection, DataContext context)
	{
		_injuryAutoHealCollection = injuryAutoHealCollection;
		SetModifiedAndInvalidateInfluencedCache(31, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _injuryAutoHealCollection.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 1, serializedSize);
			ptr += _injuryAutoHealCollection.Serialize(ptr);
		}
	}

	public DamageStepCollection GetDamageStepCollection()
	{
		return _damageStepCollection;
	}

	public unsafe void SetDamageStepCollection(DamageStepCollection damageStepCollection, DataContext context)
	{
		_damageStepCollection = damageStepCollection;
		SetModifiedAndInvalidateInfluencedCache(32, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 172u, 64);
			ptr += _damageStepCollection.Serialize(ptr);
		}
	}

	public int[] GetOuterDamageValue()
	{
		return _outerDamageValue;
	}

	public unsafe void SetOuterDamageValue(int[] outerDamageValue, DataContext context)
	{
		_outerDamageValue = outerDamageValue;
		SetModifiedAndInvalidateInfluencedCache(33, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 236u, 28);
			for (int i = 0; i < 7; i++)
			{
				((int*)ptr)[i] = _outerDamageValue[i];
			}
			ptr += 28;
		}
	}

	public int[] GetInnerDamageValue()
	{
		return _innerDamageValue;
	}

	public unsafe void SetInnerDamageValue(int[] innerDamageValue, DataContext context)
	{
		_innerDamageValue = innerDamageValue;
		SetModifiedAndInvalidateInfluencedCache(34, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 264u, 28);
			for (int i = 0; i < 7; i++)
			{
				((int*)ptr)[i] = _innerDamageValue[i];
			}
			ptr += 28;
		}
	}

	public int GetMindDamageValue()
	{
		return _mindDamageValue;
	}

	public unsafe void SetMindDamageValue(int mindDamageValue, DataContext context)
	{
		_mindDamageValue = mindDamageValue;
		SetModifiedAndInvalidateInfluencedCache(35, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 292u, 4);
			*(int*)ptr = _mindDamageValue;
			ptr += 4;
		}
	}

	public int GetFatalDamageValue()
	{
		return _fatalDamageValue;
	}

	public unsafe void SetFatalDamageValue(int fatalDamageValue, DataContext context)
	{
		_fatalDamageValue = fatalDamageValue;
		SetModifiedAndInvalidateInfluencedCache(36, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 296u, 4);
			*(int*)ptr = _fatalDamageValue;
			ptr += 4;
		}
	}

	public IntPair[] GetOuterDamageValueToShow()
	{
		return _outerDamageValueToShow;
	}

	public unsafe void SetOuterDamageValueToShow(IntPair[] outerDamageValueToShow, DataContext context)
	{
		_outerDamageValueToShow = outerDamageValueToShow;
		SetModifiedAndInvalidateInfluencedCache(37, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 300u, 56);
			for (int i = 0; i < 7; i++)
			{
				ptr += _outerDamageValueToShow[i].Serialize(ptr);
			}
		}
	}

	public IntPair[] GetInnerDamageValueToShow()
	{
		return _innerDamageValueToShow;
	}

	public unsafe void SetInnerDamageValueToShow(IntPair[] innerDamageValueToShow, DataContext context)
	{
		_innerDamageValueToShow = innerDamageValueToShow;
		SetModifiedAndInvalidateInfluencedCache(38, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 356u, 56);
			for (int i = 0; i < 7; i++)
			{
				ptr += _innerDamageValueToShow[i].Serialize(ptr);
			}
		}
	}

	public int GetMindDamageValueToShow()
	{
		return _mindDamageValueToShow;
	}

	public unsafe void SetMindDamageValueToShow(int mindDamageValueToShow, DataContext context)
	{
		_mindDamageValueToShow = mindDamageValueToShow;
		SetModifiedAndInvalidateInfluencedCache(39, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 412u, 4);
			*(int*)ptr = _mindDamageValueToShow;
			ptr += 4;
		}
	}

	public int GetFatalDamageValueToShow()
	{
		return _fatalDamageValueToShow;
	}

	public unsafe void SetFatalDamageValueToShow(int fatalDamageValueToShow, DataContext context)
	{
		_fatalDamageValueToShow = fatalDamageValueToShow;
		SetModifiedAndInvalidateInfluencedCache(40, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 416u, 4);
			*(int*)ptr = _fatalDamageValueToShow;
			ptr += 4;
		}
	}

	public byte[] GetFlawCount()
	{
		return _flawCount;
	}

	public unsafe void SetFlawCount(byte[] flawCount, DataContext context)
	{
		_flawCount = flawCount;
		SetModifiedAndInvalidateInfluencedCache(41, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 420u, 7);
			for (int i = 0; i < 7; i++)
			{
				ptr[i] = _flawCount[i];
			}
			ptr += 7;
		}
	}

	public FlawOrAcupointCollection GetFlawCollection()
	{
		return _flawCollection;
	}

	public unsafe void SetFlawCollection(FlawOrAcupointCollection flawCollection, DataContext context)
	{
		_flawCollection = flawCollection;
		SetModifiedAndInvalidateInfluencedCache(42, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _flawCollection.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 2, serializedSize);
			ptr += _flawCollection.Serialize(ptr);
		}
	}

	public byte[] GetAcupointCount()
	{
		return _acupointCount;
	}

	public unsafe void SetAcupointCount(byte[] acupointCount, DataContext context)
	{
		_acupointCount = acupointCount;
		SetModifiedAndInvalidateInfluencedCache(43, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 427u, 7);
			for (int i = 0; i < 7; i++)
			{
				ptr[i] = _acupointCount[i];
			}
			ptr += 7;
		}
	}

	public FlawOrAcupointCollection GetAcupointCollection()
	{
		return _acupointCollection;
	}

	public unsafe void SetAcupointCollection(FlawOrAcupointCollection acupointCollection, DataContext context)
	{
		_acupointCollection = acupointCollection;
		SetModifiedAndInvalidateInfluencedCache(44, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _acupointCollection.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 3, serializedSize);
			ptr += _acupointCollection.Serialize(ptr);
		}
	}

	public MindMarkList GetMindMarkTime()
	{
		return _mindMarkTime;
	}

	public unsafe void SetMindMarkTime(MindMarkList mindMarkTime, DataContext context)
	{
		_mindMarkTime = mindMarkTime;
		SetModifiedAndInvalidateInfluencedCache(45, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _mindMarkTime.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 4, serializedSize);
			ptr += _mindMarkTime.Serialize(ptr);
		}
	}

	public ref PoisonInts GetPoison()
	{
		return ref _poison;
	}

	public unsafe void SetPoison(ref PoisonInts poison, DataContext context)
	{
		_poison = poison;
		SetModifiedAndInvalidateInfluencedCache(46, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 434u, 24);
			ptr += _poison.Serialize(ptr);
		}
	}

	public ref PoisonInts GetOldPoison()
	{
		return ref _oldPoison;
	}

	public unsafe void SetOldPoison(ref PoisonInts oldPoison, DataContext context)
	{
		_oldPoison = oldPoison;
		SetModifiedAndInvalidateInfluencedCache(47, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 458u, 24);
			ptr += _oldPoison.Serialize(ptr);
		}
	}

	public ref PoisonInts GetPoisonResist()
	{
		return ref _poisonResist;
	}

	public unsafe void SetPoisonResist(ref PoisonInts poisonResist, DataContext context)
	{
		_poisonResist = poisonResist;
		SetModifiedAndInvalidateInfluencedCache(48, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 482u, 24);
			ptr += _poisonResist.Serialize(ptr);
		}
	}

	public ref PoisonsAndLevels GetNewPoisonsToShow()
	{
		return ref _newPoisonsToShow;
	}

	public unsafe void SetNewPoisonsToShow(ref PoisonsAndLevels newPoisonsToShow, DataContext context)
	{
		_newPoisonsToShow = newPoisonsToShow;
		SetModifiedAndInvalidateInfluencedCache(49, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 506u, 18);
			ptr += _newPoisonsToShow.Serialize(ptr);
		}
	}

	public DefeatMarkCollection GetDefeatMarkCollection()
	{
		return _defeatMarkCollection;
	}

	public unsafe void SetDefeatMarkCollection(DefeatMarkCollection defeatMarkCollection, DataContext context)
	{
		_defeatMarkCollection = defeatMarkCollection;
		SetModifiedAndInvalidateInfluencedCache(50, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _defeatMarkCollection.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 5, serializedSize);
			ptr += _defeatMarkCollection.Serialize(ptr);
		}
	}

	public List<short> GetNeigongList()
	{
		return _neigongList;
	}

	public unsafe void SetNeigongList(List<short> neigongList, DataContext context)
	{
		_neigongList = neigongList;
		SetModifiedAndInvalidateInfluencedCache(51, context);
		if (CollectionHelperData.IsArchive)
		{
			int count = _neigongList.Count;
			int num = 2 * count;
			int valueSize = 2 + num;
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 6, valueSize);
			*(ushort*)ptr = (ushort)count;
			ptr += 2;
			for (int i = 0; i < count; i++)
			{
				((short*)ptr)[i] = _neigongList[i];
			}
			ptr += num;
		}
	}

	public List<short> GetAttackSkillList()
	{
		return _attackSkillList;
	}

	public unsafe void SetAttackSkillList(List<short> attackSkillList, DataContext context)
	{
		_attackSkillList = attackSkillList;
		SetModifiedAndInvalidateInfluencedCache(52, context);
		if (CollectionHelperData.IsArchive)
		{
			int count = _attackSkillList.Count;
			int num = 2 * count;
			int valueSize = 2 + num;
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 7, valueSize);
			*(ushort*)ptr = (ushort)count;
			ptr += 2;
			for (int i = 0; i < count; i++)
			{
				((short*)ptr)[i] = _attackSkillList[i];
			}
			ptr += num;
		}
	}

	public List<short> GetAgileSkillList()
	{
		return _agileSkillList;
	}

	public unsafe void SetAgileSkillList(List<short> agileSkillList, DataContext context)
	{
		_agileSkillList = agileSkillList;
		SetModifiedAndInvalidateInfluencedCache(53, context);
		if (CollectionHelperData.IsArchive)
		{
			int count = _agileSkillList.Count;
			int num = 2 * count;
			int valueSize = 2 + num;
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 8, valueSize);
			*(ushort*)ptr = (ushort)count;
			ptr += 2;
			for (int i = 0; i < count; i++)
			{
				((short*)ptr)[i] = _agileSkillList[i];
			}
			ptr += num;
		}
	}

	public List<short> GetDefenceSkillList()
	{
		return _defenceSkillList;
	}

	public unsafe void SetDefenceSkillList(List<short> defenceSkillList, DataContext context)
	{
		_defenceSkillList = defenceSkillList;
		SetModifiedAndInvalidateInfluencedCache(54, context);
		if (CollectionHelperData.IsArchive)
		{
			int count = _defenceSkillList.Count;
			int num = 2 * count;
			int valueSize = 2 + num;
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 9, valueSize);
			*(ushort*)ptr = (ushort)count;
			ptr += 2;
			for (int i = 0; i < count; i++)
			{
				((short*)ptr)[i] = _defenceSkillList[i];
			}
			ptr += num;
		}
	}

	public List<short> GetAssistSkillList()
	{
		return _assistSkillList;
	}

	public unsafe void SetAssistSkillList(List<short> assistSkillList, DataContext context)
	{
		_assistSkillList = assistSkillList;
		SetModifiedAndInvalidateInfluencedCache(55, context);
		if (CollectionHelperData.IsArchive)
		{
			int count = _assistSkillList.Count;
			int num = 2 * count;
			int valueSize = 2 + num;
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 10, valueSize);
			*(ushort*)ptr = (ushort)count;
			ptr += 2;
			for (int i = 0; i < count; i++)
			{
				((short*)ptr)[i] = _assistSkillList[i];
			}
			ptr += num;
		}
	}

	public short GetPreparingSkillId()
	{
		return _preparingSkillId;
	}

	public unsafe void SetPreparingSkillId(short preparingSkillId, DataContext context)
	{
		_preparingSkillId = preparingSkillId;
		SetModifiedAndInvalidateInfluencedCache(56, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 524u, 2);
			*(short*)ptr = _preparingSkillId;
			ptr += 2;
		}
	}

	public byte GetSkillPreparePercent()
	{
		return _skillPreparePercent;
	}

	public unsafe void SetSkillPreparePercent(byte skillPreparePercent, DataContext context)
	{
		_skillPreparePercent = skillPreparePercent;
		SetModifiedAndInvalidateInfluencedCache(57, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 526u, 1);
			*ptr = _skillPreparePercent;
			ptr++;
		}
	}

	public short GetPerformingSkillId()
	{
		return _performingSkillId;
	}

	public unsafe void SetPerformingSkillId(short performingSkillId, DataContext context)
	{
		_performingSkillId = performingSkillId;
		SetModifiedAndInvalidateInfluencedCache(58, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 527u, 2);
			*(short*)ptr = _performingSkillId;
			ptr += 2;
		}
	}

	public bool GetAutoCastingSkill()
	{
		return _autoCastingSkill;
	}

	public unsafe void SetAutoCastingSkill(bool autoCastingSkill, DataContext context)
	{
		_autoCastingSkill = autoCastingSkill;
		SetModifiedAndInvalidateInfluencedCache(59, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 529u, 1);
			*ptr = (_autoCastingSkill ? ((byte)1) : ((byte)0));
			ptr++;
		}
	}

	public byte GetAttackSkillAttackIndex()
	{
		return _attackSkillAttackIndex;
	}

	public unsafe void SetAttackSkillAttackIndex(byte attackSkillAttackIndex, DataContext context)
	{
		_attackSkillAttackIndex = attackSkillAttackIndex;
		SetModifiedAndInvalidateInfluencedCache(60, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 530u, 1);
			*ptr = _attackSkillAttackIndex;
			ptr++;
		}
	}

	public byte GetAttackSkillPower()
	{
		return _attackSkillPower;
	}

	public unsafe void SetAttackSkillPower(byte attackSkillPower, DataContext context)
	{
		_attackSkillPower = attackSkillPower;
		SetModifiedAndInvalidateInfluencedCache(61, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 531u, 1);
			*ptr = _attackSkillPower;
			ptr++;
		}
	}

	public short GetAffectingMoveSkillId()
	{
		return _affectingMoveSkillId;
	}

	public unsafe void SetAffectingMoveSkillId(short affectingMoveSkillId, DataContext context)
	{
		_affectingMoveSkillId = affectingMoveSkillId;
		SetModifiedAndInvalidateInfluencedCache(62, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 532u, 2);
			*(short*)ptr = _affectingMoveSkillId;
			ptr += 2;
		}
	}

	public short GetAffectingDefendSkillId()
	{
		return _affectingDefendSkillId;
	}

	public unsafe void SetAffectingDefendSkillId(short affectingDefendSkillId, DataContext context)
	{
		_affectingDefendSkillId = affectingDefendSkillId;
		SetModifiedAndInvalidateInfluencedCache(63, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 534u, 2);
			*(short*)ptr = _affectingDefendSkillId;
			ptr += 2;
		}
	}

	public byte GetDefendSkillTimePercent()
	{
		return _defendSkillTimePercent;
	}

	public unsafe void SetDefendSkillTimePercent(byte defendSkillTimePercent, DataContext context)
	{
		_defendSkillTimePercent = defendSkillTimePercent;
		SetModifiedAndInvalidateInfluencedCache(64, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 536u, 1);
			*ptr = _defendSkillTimePercent;
			ptr++;
		}
	}

	public short GetWugCount()
	{
		return _wugCount;
	}

	public unsafe void SetWugCount(short wugCount, DataContext context)
	{
		_wugCount = wugCount;
		SetModifiedAndInvalidateInfluencedCache(65, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 537u, 2);
			*(short*)ptr = _wugCount;
			ptr += 2;
		}
	}

	public byte GetHealInjuryCount()
	{
		return _healInjuryCount;
	}

	public unsafe void SetHealInjuryCount(byte healInjuryCount, DataContext context)
	{
		_healInjuryCount = healInjuryCount;
		SetModifiedAndInvalidateInfluencedCache(66, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 539u, 1);
			*ptr = _healInjuryCount;
			ptr++;
		}
	}

	public byte GetHealPoisonCount()
	{
		return _healPoisonCount;
	}

	public unsafe void SetHealPoisonCount(byte healPoisonCount, DataContext context)
	{
		_healPoisonCount = healPoisonCount;
		SetModifiedAndInvalidateInfluencedCache(67, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 540u, 1);
			*ptr = _healPoisonCount;
			ptr++;
		}
	}

	public bool[] GetOtherActionCanUse()
	{
		return _otherActionCanUse;
	}

	public unsafe void SetOtherActionCanUse(bool[] otherActionCanUse, DataContext context)
	{
		_otherActionCanUse = otherActionCanUse;
		SetModifiedAndInvalidateInfluencedCache(68, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 541u, 5);
			for (int i = 0; i < 5; i++)
			{
				ptr[i] = (_otherActionCanUse[i] ? ((byte)1) : ((byte)0));
			}
			ptr += 5;
		}
	}

	public sbyte GetPreparingOtherAction()
	{
		return _preparingOtherAction;
	}

	public unsafe void SetPreparingOtherAction(sbyte preparingOtherAction, DataContext context)
	{
		_preparingOtherAction = preparingOtherAction;
		SetModifiedAndInvalidateInfluencedCache(69, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 546u, 1);
			*ptr = (byte)_preparingOtherAction;
			ptr++;
		}
	}

	public byte GetOtherActionPreparePercent()
	{
		return _otherActionPreparePercent;
	}

	public unsafe void SetOtherActionPreparePercent(byte otherActionPreparePercent, DataContext context)
	{
		_otherActionPreparePercent = otherActionPreparePercent;
		SetModifiedAndInvalidateInfluencedCache(70, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 547u, 1);
			*ptr = _otherActionPreparePercent;
			ptr++;
		}
	}

	public bool GetCanSurrender()
	{
		return _canSurrender;
	}

	public unsafe void SetCanSurrender(bool canSurrender, DataContext context)
	{
		_canSurrender = canSurrender;
		SetModifiedAndInvalidateInfluencedCache(71, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 548u, 1);
			*ptr = (_canSurrender ? ((byte)1) : ((byte)0));
			ptr++;
		}
	}

	public bool GetCanUseItem()
	{
		return _canUseItem;
	}

	public unsafe void SetCanUseItem(bool canUseItem, DataContext context)
	{
		_canUseItem = canUseItem;
		SetModifiedAndInvalidateInfluencedCache(72, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 549u, 1);
			*ptr = (_canUseItem ? ((byte)1) : ((byte)0));
			ptr++;
		}
	}

	public ItemKey GetPreparingItem()
	{
		return _preparingItem;
	}

	public unsafe void SetPreparingItem(ItemKey preparingItem, DataContext context)
	{
		_preparingItem = preparingItem;
		SetModifiedAndInvalidateInfluencedCache(73, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 550u, 8);
			ptr += _preparingItem.Serialize(ptr);
		}
	}

	public byte GetUseItemPreparePercent()
	{
		return _useItemPreparePercent;
	}

	public unsafe void SetUseItemPreparePercent(byte useItemPreparePercent, DataContext context)
	{
		_useItemPreparePercent = useItemPreparePercent;
		SetModifiedAndInvalidateInfluencedCache(74, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 558u, 1);
			*ptr = _useItemPreparePercent;
			ptr++;
		}
	}

	public CombatReserveData GetCombatReserveData()
	{
		return _combatReserveData;
	}

	public unsafe void SetCombatReserveData(CombatReserveData combatReserveData, DataContext context)
	{
		_combatReserveData = combatReserveData;
		SetModifiedAndInvalidateInfluencedCache(75, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 559u, 12);
			ptr += _combatReserveData.Serialize(ptr);
		}
	}

	public CombatStateCollection GetBuffCombatStateCollection()
	{
		return _buffCombatStateCollection;
	}

	public unsafe void SetBuffCombatStateCollection(CombatStateCollection buffCombatStateCollection, DataContext context)
	{
		_buffCombatStateCollection = buffCombatStateCollection;
		SetModifiedAndInvalidateInfluencedCache(76, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _buffCombatStateCollection.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 11, serializedSize);
			ptr += _buffCombatStateCollection.Serialize(ptr);
		}
	}

	public CombatStateCollection GetDebuffCombatStateCollection()
	{
		return _debuffCombatStateCollection;
	}

	public unsafe void SetDebuffCombatStateCollection(CombatStateCollection debuffCombatStateCollection, DataContext context)
	{
		_debuffCombatStateCollection = debuffCombatStateCollection;
		SetModifiedAndInvalidateInfluencedCache(77, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _debuffCombatStateCollection.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 12, serializedSize);
			ptr += _debuffCombatStateCollection.Serialize(ptr);
		}
	}

	public CombatStateCollection GetSpecialCombatStateCollection()
	{
		return _specialCombatStateCollection;
	}

	public unsafe void SetSpecialCombatStateCollection(CombatStateCollection specialCombatStateCollection, DataContext context)
	{
		_specialCombatStateCollection = specialCombatStateCollection;
		SetModifiedAndInvalidateInfluencedCache(78, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _specialCombatStateCollection.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 13, serializedSize);
			ptr += _specialCombatStateCollection.Serialize(ptr);
		}
	}

	public SkillEffectCollection GetSkillEffectCollection()
	{
		return _skillEffectCollection;
	}

	public unsafe void SetSkillEffectCollection(SkillEffectCollection skillEffectCollection, DataContext context)
	{
		_skillEffectCollection = skillEffectCollection;
		SetModifiedAndInvalidateInfluencedCache(79, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _skillEffectCollection.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 14, serializedSize);
			ptr += _skillEffectCollection.Serialize(ptr);
		}
	}

	public short GetXiangshuEffectId()
	{
		return _xiangshuEffectId;
	}

	public unsafe void SetXiangshuEffectId(short xiangshuEffectId, DataContext context)
	{
		_xiangshuEffectId = xiangshuEffectId;
		SetModifiedAndInvalidateInfluencedCache(80, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 571u, 2);
			*(short*)ptr = _xiangshuEffectId;
			ptr += 2;
		}
	}

	public int GetHazardValue()
	{
		return _hazardValue;
	}

	public unsafe void SetHazardValue(int hazardValue, DataContext context)
	{
		_hazardValue = hazardValue;
		SetModifiedAndInvalidateInfluencedCache(81, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 573u, 4);
			*(int*)ptr = _hazardValue;
			ptr += 4;
		}
	}

	public ShowSpecialEffectCollection GetShowEffectList()
	{
		return _showEffectList;
	}

	public unsafe void SetShowEffectList(ShowSpecialEffectCollection showEffectList, DataContext context)
	{
		_showEffectList = showEffectList;
		SetModifiedAndInvalidateInfluencedCache(82, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _showEffectList.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 15, serializedSize);
			ptr += _showEffectList.Serialize(ptr);
		}
	}

	public string GetAnimationToLoop()
	{
		return _animationToLoop;
	}

	public unsafe void SetAnimationToLoop(string animationToLoop, DataContext context)
	{
		_animationToLoop = animationToLoop;
		SetModifiedAndInvalidateInfluencedCache(83, context);
		if (!CollectionHelperData.IsArchive)
		{
			return;
		}
		int length = _animationToLoop.Length;
		int num = 2 * length;
		int valueSize = 4 + num;
		byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 16, valueSize);
		*(int*)ptr = num;
		ptr += 4;
		fixed (char* animationToLoop2 = _animationToLoop)
		{
			for (int i = 0; i < length; i++)
			{
				((short*)ptr)[i] = (short)animationToLoop2[i];
			}
		}
		ptr += num;
	}

	public string GetAnimationToPlayOnce()
	{
		return _animationToPlayOnce;
	}

	public unsafe void SetAnimationToPlayOnce(string animationToPlayOnce, DataContext context)
	{
		_animationToPlayOnce = animationToPlayOnce;
		SetModifiedAndInvalidateInfluencedCache(84, context);
		if (!CollectionHelperData.IsArchive)
		{
			return;
		}
		int length = _animationToPlayOnce.Length;
		int num = 2 * length;
		int valueSize = 4 + num;
		byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 17, valueSize);
		*(int*)ptr = num;
		ptr += 4;
		fixed (char* animationToPlayOnce2 = _animationToPlayOnce)
		{
			for (int i = 0; i < length; i++)
			{
				((short*)ptr)[i] = (short)animationToPlayOnce2[i];
			}
		}
		ptr += num;
	}

	public string GetParticleToPlay()
	{
		return _particleToPlay;
	}

	public unsafe void SetParticleToPlay(string particleToPlay, DataContext context)
	{
		_particleToPlay = particleToPlay;
		SetModifiedAndInvalidateInfluencedCache(85, context);
		if (!CollectionHelperData.IsArchive)
		{
			return;
		}
		int length = _particleToPlay.Length;
		int num = 2 * length;
		int valueSize = 4 + num;
		byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 18, valueSize);
		*(int*)ptr = num;
		ptr += 4;
		fixed (char* particleToPlay2 = _particleToPlay)
		{
			for (int i = 0; i < length; i++)
			{
				((short*)ptr)[i] = (short)particleToPlay2[i];
			}
		}
		ptr += num;
	}

	public string GetParticleToLoop()
	{
		return _particleToLoop;
	}

	public unsafe void SetParticleToLoop(string particleToLoop, DataContext context)
	{
		_particleToLoop = particleToLoop;
		SetModifiedAndInvalidateInfluencedCache(86, context);
		if (!CollectionHelperData.IsArchive)
		{
			return;
		}
		int length = _particleToLoop.Length;
		int num = 2 * length;
		int valueSize = 4 + num;
		byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 19, valueSize);
		*(int*)ptr = num;
		ptr += 4;
		fixed (char* particleToLoop2 = _particleToLoop)
		{
			for (int i = 0; i < length; i++)
			{
				((short*)ptr)[i] = (short)particleToLoop2[i];
			}
		}
		ptr += num;
	}

	public string GetSkillPetAnimation()
	{
		return _skillPetAnimation;
	}

	public unsafe void SetSkillPetAnimation(string skillPetAnimation, DataContext context)
	{
		_skillPetAnimation = skillPetAnimation;
		SetModifiedAndInvalidateInfluencedCache(87, context);
		if (!CollectionHelperData.IsArchive)
		{
			return;
		}
		int length = _skillPetAnimation.Length;
		int num = 2 * length;
		int valueSize = 4 + num;
		byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 20, valueSize);
		*(int*)ptr = num;
		ptr += 4;
		fixed (char* skillPetAnimation2 = _skillPetAnimation)
		{
			for (int i = 0; i < length; i++)
			{
				((short*)ptr)[i] = (short)skillPetAnimation2[i];
			}
		}
		ptr += num;
	}

	public string GetPetParticle()
	{
		return _petParticle;
	}

	public unsafe void SetPetParticle(string petParticle, DataContext context)
	{
		_petParticle = petParticle;
		SetModifiedAndInvalidateInfluencedCache(88, context);
		if (!CollectionHelperData.IsArchive)
		{
			return;
		}
		int length = _petParticle.Length;
		int num = 2 * length;
		int valueSize = 4 + num;
		byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 21, valueSize);
		*(int*)ptr = num;
		ptr += 4;
		fixed (char* petParticle2 = _petParticle)
		{
			for (int i = 0; i < length; i++)
			{
				((short*)ptr)[i] = (short)petParticle2[i];
			}
		}
		ptr += num;
	}

	public float GetAnimationTimeScale()
	{
		return _animationTimeScale;
	}

	public unsafe void SetAnimationTimeScale(float animationTimeScale, DataContext context)
	{
		_animationTimeScale = animationTimeScale;
		SetModifiedAndInvalidateInfluencedCache(89, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 577u, 4);
			*(float*)ptr = _animationTimeScale;
			ptr += 4;
		}
	}

	public bool GetAttackOutOfRange()
	{
		return _attackOutOfRange;
	}

	public unsafe void SetAttackOutOfRange(bool attackOutOfRange, DataContext context)
	{
		_attackOutOfRange = attackOutOfRange;
		SetModifiedAndInvalidateInfluencedCache(90, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 581u, 1);
			*ptr = (_attackOutOfRange ? ((byte)1) : ((byte)0));
			ptr++;
		}
	}

	public string GetAttackSoundToPlay()
	{
		return _attackSoundToPlay;
	}

	public unsafe void SetAttackSoundToPlay(string attackSoundToPlay, DataContext context)
	{
		_attackSoundToPlay = attackSoundToPlay;
		SetModifiedAndInvalidateInfluencedCache(91, context);
		if (!CollectionHelperData.IsArchive)
		{
			return;
		}
		int length = _attackSoundToPlay.Length;
		int num = 2 * length;
		int valueSize = 4 + num;
		byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 22, valueSize);
		*(int*)ptr = num;
		ptr += 4;
		fixed (char* attackSoundToPlay2 = _attackSoundToPlay)
		{
			for (int i = 0; i < length; i++)
			{
				((short*)ptr)[i] = (short)attackSoundToPlay2[i];
			}
		}
		ptr += num;
	}

	public string GetSkillSoundToPlay()
	{
		return _skillSoundToPlay;
	}

	public unsafe void SetSkillSoundToPlay(string skillSoundToPlay, DataContext context)
	{
		_skillSoundToPlay = skillSoundToPlay;
		SetModifiedAndInvalidateInfluencedCache(92, context);
		if (!CollectionHelperData.IsArchive)
		{
			return;
		}
		int length = _skillSoundToPlay.Length;
		int num = 2 * length;
		int valueSize = 4 + num;
		byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 23, valueSize);
		*(int*)ptr = num;
		ptr += 4;
		fixed (char* skillSoundToPlay2 = _skillSoundToPlay)
		{
			for (int i = 0; i < length; i++)
			{
				((short*)ptr)[i] = (short)skillSoundToPlay2[i];
			}
		}
		ptr += num;
	}

	public string GetHitSoundToPlay()
	{
		return _hitSoundToPlay;
	}

	public unsafe void SetHitSoundToPlay(string hitSoundToPlay, DataContext context)
	{
		_hitSoundToPlay = hitSoundToPlay;
		SetModifiedAndInvalidateInfluencedCache(93, context);
		if (!CollectionHelperData.IsArchive)
		{
			return;
		}
		int length = _hitSoundToPlay.Length;
		int num = 2 * length;
		int valueSize = 4 + num;
		byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 24, valueSize);
		*(int*)ptr = num;
		ptr += 4;
		fixed (char* hitSoundToPlay2 = _hitSoundToPlay)
		{
			for (int i = 0; i < length; i++)
			{
				((short*)ptr)[i] = (short)hitSoundToPlay2[i];
			}
		}
		ptr += num;
	}

	public string GetArmorHitSoundToPlay()
	{
		return _armorHitSoundToPlay;
	}

	public unsafe void SetArmorHitSoundToPlay(string armorHitSoundToPlay, DataContext context)
	{
		_armorHitSoundToPlay = armorHitSoundToPlay;
		SetModifiedAndInvalidateInfluencedCache(94, context);
		if (!CollectionHelperData.IsArchive)
		{
			return;
		}
		int length = _armorHitSoundToPlay.Length;
		int num = 2 * length;
		int valueSize = 4 + num;
		byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 25, valueSize);
		*(int*)ptr = num;
		ptr += 4;
		fixed (char* armorHitSoundToPlay2 = _armorHitSoundToPlay)
		{
			for (int i = 0; i < length; i++)
			{
				((short*)ptr)[i] = (short)armorHitSoundToPlay2[i];
			}
		}
		ptr += num;
	}

	public string GetWhooshSoundToPlay()
	{
		return _whooshSoundToPlay;
	}

	public unsafe void SetWhooshSoundToPlay(string whooshSoundToPlay, DataContext context)
	{
		_whooshSoundToPlay = whooshSoundToPlay;
		SetModifiedAndInvalidateInfluencedCache(95, context);
		if (!CollectionHelperData.IsArchive)
		{
			return;
		}
		int length = _whooshSoundToPlay.Length;
		int num = 2 * length;
		int valueSize = 4 + num;
		byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 26, valueSize);
		*(int*)ptr = num;
		ptr += 4;
		fixed (char* whooshSoundToPlay2 = _whooshSoundToPlay)
		{
			for (int i = 0; i < length; i++)
			{
				((short*)ptr)[i] = (short)whooshSoundToPlay2[i];
			}
		}
		ptr += num;
	}

	public string GetShockSoundToPlay()
	{
		return _shockSoundToPlay;
	}

	public unsafe void SetShockSoundToPlay(string shockSoundToPlay, DataContext context)
	{
		_shockSoundToPlay = shockSoundToPlay;
		SetModifiedAndInvalidateInfluencedCache(96, context);
		if (!CollectionHelperData.IsArchive)
		{
			return;
		}
		int length = _shockSoundToPlay.Length;
		int num = 2 * length;
		int valueSize = 4 + num;
		byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 27, valueSize);
		*(int*)ptr = num;
		ptr += 4;
		fixed (char* shockSoundToPlay2 = _shockSoundToPlay)
		{
			for (int i = 0; i < length; i++)
			{
				((short*)ptr)[i] = (short)shockSoundToPlay2[i];
			}
		}
		ptr += num;
	}

	public string GetStepSoundToPlay()
	{
		return _stepSoundToPlay;
	}

	public unsafe void SetStepSoundToPlay(string stepSoundToPlay, DataContext context)
	{
		_stepSoundToPlay = stepSoundToPlay;
		SetModifiedAndInvalidateInfluencedCache(97, context);
		if (!CollectionHelperData.IsArchive)
		{
			return;
		}
		int length = _stepSoundToPlay.Length;
		int num = 2 * length;
		int valueSize = 4 + num;
		byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 28, valueSize);
		*(int*)ptr = num;
		ptr += 4;
		fixed (char* stepSoundToPlay2 = _stepSoundToPlay)
		{
			for (int i = 0; i < length; i++)
			{
				((short*)ptr)[i] = (short)stepSoundToPlay2[i];
			}
		}
		ptr += num;
	}

	public string GetDieSoundToPlay()
	{
		return _dieSoundToPlay;
	}

	public unsafe void SetDieSoundToPlay(string dieSoundToPlay, DataContext context)
	{
		_dieSoundToPlay = dieSoundToPlay;
		SetModifiedAndInvalidateInfluencedCache(98, context);
		if (!CollectionHelperData.IsArchive)
		{
			return;
		}
		int length = _dieSoundToPlay.Length;
		int num = 2 * length;
		int valueSize = 4 + num;
		byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 29, valueSize);
		*(int*)ptr = num;
		ptr += 4;
		fixed (char* dieSoundToPlay2 = _dieSoundToPlay)
		{
			for (int i = 0; i < length; i++)
			{
				((short*)ptr)[i] = (short)dieSoundToPlay2[i];
			}
		}
		ptr += num;
	}

	public string GetSoundToLoop()
	{
		return _soundToLoop;
	}

	public unsafe void SetSoundToLoop(string soundToLoop, DataContext context)
	{
		_soundToLoop = soundToLoop;
		SetModifiedAndInvalidateInfluencedCache(99, context);
		if (!CollectionHelperData.IsArchive)
		{
			return;
		}
		int length = _soundToLoop.Length;
		int num = 2 * length;
		int valueSize = 4 + num;
		byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 30, valueSize);
		*(int*)ptr = num;
		ptr += 4;
		fixed (char* soundToLoop2 = _soundToLoop)
		{
			for (int i = 0; i < length; i++)
			{
				((short*)ptr)[i] = (short)soundToLoop2[i];
			}
		}
		ptr += num;
	}

	public sbyte GetBossPhase()
	{
		return _bossPhase;
	}

	public unsafe void SetBossPhase(sbyte bossPhase, DataContext context)
	{
		_bossPhase = bossPhase;
		SetModifiedAndInvalidateInfluencedCache(100, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 582u, 1);
			*ptr = (byte)_bossPhase;
			ptr++;
		}
	}

	public sbyte GetAnimalAttackCount()
	{
		return _animalAttackCount;
	}

	public unsafe void SetAnimalAttackCount(sbyte animalAttackCount, DataContext context)
	{
		_animalAttackCount = animalAttackCount;
		SetModifiedAndInvalidateInfluencedCache(101, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 583u, 1);
			*ptr = (byte)_animalAttackCount;
			ptr++;
		}
	}

	public bool GetShowTransferInjuryCommand()
	{
		return _showTransferInjuryCommand;
	}

	public unsafe void SetShowTransferInjuryCommand(bool showTransferInjuryCommand, DataContext context)
	{
		_showTransferInjuryCommand = showTransferInjuryCommand;
		SetModifiedAndInvalidateInfluencedCache(102, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 584u, 1);
			*ptr = (_showTransferInjuryCommand ? ((byte)1) : ((byte)0));
			ptr++;
		}
	}

	public List<sbyte> GetCurrTeammateCommands()
	{
		return _currTeammateCommands;
	}

	public unsafe void SetCurrTeammateCommands(List<sbyte> currTeammateCommands, DataContext context)
	{
		_currTeammateCommands = currTeammateCommands;
		SetModifiedAndInvalidateInfluencedCache(103, context);
		if (CollectionHelperData.IsArchive)
		{
			int count = _currTeammateCommands.Count;
			int num = count;
			int valueSize = 2 + num;
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 31, valueSize);
			*(ushort*)ptr = (ushort)count;
			ptr += 2;
			for (int i = 0; i < count; i++)
			{
				ptr[i] = (byte)_currTeammateCommands[i];
			}
			ptr += num;
		}
	}

	public List<byte> GetTeammateCommandCdPercent()
	{
		return _teammateCommandCdPercent;
	}

	public unsafe void SetTeammateCommandCdPercent(List<byte> teammateCommandCdPercent, DataContext context)
	{
		_teammateCommandCdPercent = teammateCommandCdPercent;
		SetModifiedAndInvalidateInfluencedCache(104, context);
		if (CollectionHelperData.IsArchive)
		{
			int count = _teammateCommandCdPercent.Count;
			int num = count;
			int valueSize = 2 + num;
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 32, valueSize);
			*(ushort*)ptr = (ushort)count;
			ptr += 2;
			for (int i = 0; i < count; i++)
			{
				ptr[i] = _teammateCommandCdPercent[i];
			}
			ptr += num;
		}
	}

	public sbyte GetExecutingTeammateCommand()
	{
		return _executingTeammateCommand;
	}

	public unsafe void SetExecutingTeammateCommand(sbyte executingTeammateCommand, DataContext context)
	{
		_executingTeammateCommand = executingTeammateCommand;
		SetModifiedAndInvalidateInfluencedCache(105, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 585u, 1);
			*ptr = (byte)_executingTeammateCommand;
			ptr++;
		}
	}

	public bool GetVisible()
	{
		return _visible;
	}

	public unsafe void SetVisible(bool visible, DataContext context)
	{
		_visible = visible;
		SetModifiedAndInvalidateInfluencedCache(106, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 586u, 1);
			*ptr = (_visible ? ((byte)1) : ((byte)0));
			ptr++;
		}
	}

	public byte GetTeammateCommandPreparePercent()
	{
		return _teammateCommandPreparePercent;
	}

	public unsafe void SetTeammateCommandPreparePercent(byte teammateCommandPreparePercent, DataContext context)
	{
		_teammateCommandPreparePercent = teammateCommandPreparePercent;
		SetModifiedAndInvalidateInfluencedCache(107, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 587u, 1);
			*ptr = _teammateCommandPreparePercent;
			ptr++;
		}
	}

	public byte GetTeammateCommandTimePercent()
	{
		return _teammateCommandTimePercent;
	}

	public unsafe void SetTeammateCommandTimePercent(byte teammateCommandTimePercent, DataContext context)
	{
		_teammateCommandTimePercent = teammateCommandTimePercent;
		SetModifiedAndInvalidateInfluencedCache(108, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 588u, 1);
			*ptr = _teammateCommandTimePercent;
			ptr++;
		}
	}

	public ItemKey GetAttackCommandWeaponKey()
	{
		return _attackCommandWeaponKey;
	}

	public unsafe void SetAttackCommandWeaponKey(ItemKey attackCommandWeaponKey, DataContext context)
	{
		_attackCommandWeaponKey = attackCommandWeaponKey;
		SetModifiedAndInvalidateInfluencedCache(109, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 589u, 8);
			ptr += _attackCommandWeaponKey.Serialize(ptr);
		}
	}

	public sbyte GetAttackCommandTrickType()
	{
		return _attackCommandTrickType;
	}

	public unsafe void SetAttackCommandTrickType(sbyte attackCommandTrickType, DataContext context)
	{
		_attackCommandTrickType = attackCommandTrickType;
		SetModifiedAndInvalidateInfluencedCache(110, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 597u, 1);
			*ptr = (byte)_attackCommandTrickType;
			ptr++;
		}
	}

	public short GetDefendCommandSkillId()
	{
		return _defendCommandSkillId;
	}

	public unsafe void SetDefendCommandSkillId(short defendCommandSkillId, DataContext context)
	{
		_defendCommandSkillId = defendCommandSkillId;
		SetModifiedAndInvalidateInfluencedCache(111, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 598u, 2);
			*(short*)ptr = _defendCommandSkillId;
			ptr += 2;
		}
	}

	public sbyte GetShowEffectCommandIndex()
	{
		return _showEffectCommandIndex;
	}

	public unsafe void SetShowEffectCommandIndex(sbyte showEffectCommandIndex, DataContext context)
	{
		_showEffectCommandIndex = showEffectCommandIndex;
		SetModifiedAndInvalidateInfluencedCache(112, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 600u, 1);
			*ptr = (byte)_showEffectCommandIndex;
			ptr++;
		}
	}

	public short GetAttackCommandSkillId()
	{
		return _attackCommandSkillId;
	}

	public unsafe void SetAttackCommandSkillId(short attackCommandSkillId, DataContext context)
	{
		_attackCommandSkillId = attackCommandSkillId;
		SetModifiedAndInvalidateInfluencedCache(113, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 601u, 2);
			*(short*)ptr = _attackCommandSkillId;
			ptr += 2;
		}
	}

	public List<SByteList> GetTeammateCommandBanReasons()
	{
		return _teammateCommandBanReasons;
	}

	public unsafe void SetTeammateCommandBanReasons(List<SByteList> teammateCommandBanReasons, DataContext context)
	{
		_teammateCommandBanReasons = teammateCommandBanReasons;
		SetModifiedAndInvalidateInfluencedCache(114, context);
		if (CollectionHelperData.IsArchive)
		{
			int num = 2;
			int count = _teammateCommandBanReasons.Count;
			for (int i = 0; i < count; i++)
			{
				num += _teammateCommandBanReasons[i].GetSerializedSize();
			}
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 33, num);
			*(ushort*)ptr = (ushort)count;
			ptr += 2;
			for (int j = 0; j < count; j++)
			{
				ptr += _teammateCommandBanReasons[j].Serialize(ptr);
			}
		}
	}

	public short GetTargetDistance()
	{
		return _targetDistance;
	}

	public unsafe void SetTargetDistance(short targetDistance, DataContext context)
	{
		_targetDistance = targetDistance;
		SetModifiedAndInvalidateInfluencedCache(115, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 603u, 2);
			*(short*)ptr = _targetDistance;
			ptr += 2;
		}
	}

	public InjuryAutoHealCollection GetOldInjuryAutoHealCollection()
	{
		return _oldInjuryAutoHealCollection;
	}

	public unsafe void SetOldInjuryAutoHealCollection(InjuryAutoHealCollection oldInjuryAutoHealCollection, DataContext context)
	{
		_oldInjuryAutoHealCollection = oldInjuryAutoHealCollection;
		SetModifiedAndInvalidateInfluencedCache(116, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _oldInjuryAutoHealCollection.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 34, serializedSize);
			ptr += _oldInjuryAutoHealCollection.Serialize(ptr);
		}
	}

	public MixPoisonAffectedCountCollection GetMixPoisonAffectedCount()
	{
		return _mixPoisonAffectedCount;
	}

	public unsafe void SetMixPoisonAffectedCount(MixPoisonAffectedCountCollection mixPoisonAffectedCount, DataContext context)
	{
		_mixPoisonAffectedCount = mixPoisonAffectedCount;
		SetModifiedAndInvalidateInfluencedCache(117, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _mixPoisonAffectedCount.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 35, serializedSize);
			ptr += _mixPoisonAffectedCount.Serialize(ptr);
		}
	}

	public string GetParticleToLoopByCombatSkill()
	{
		return _particleToLoopByCombatSkill;
	}

	public unsafe void SetParticleToLoopByCombatSkill(string particleToLoopByCombatSkill, DataContext context)
	{
		_particleToLoopByCombatSkill = particleToLoopByCombatSkill;
		SetModifiedAndInvalidateInfluencedCache(118, context);
		if (!CollectionHelperData.IsArchive)
		{
			return;
		}
		int length = _particleToLoopByCombatSkill.Length;
		int num = 2 * length;
		int valueSize = 4 + num;
		byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 36, valueSize);
		*(int*)ptr = num;
		ptr += 4;
		fixed (char* particleToLoopByCombatSkill2 = _particleToLoopByCombatSkill)
		{
			for (int i = 0; i < length; i++)
			{
				((short*)ptr)[i] = (short)particleToLoopByCombatSkill2[i];
			}
		}
		ptr += num;
	}

	public SilenceFrameData GetNeiliAllocationCd()
	{
		return _neiliAllocationCd;
	}

	public unsafe void SetNeiliAllocationCd(SilenceFrameData neiliAllocationCd, DataContext context)
	{
		_neiliAllocationCd = neiliAllocationCd;
		SetModifiedAndInvalidateInfluencedCache(119, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 605u, 8);
			ptr += _neiliAllocationCd.Serialize(ptr);
		}
	}

	public NeiliProportionOfFiveElements GetProportionDelta()
	{
		return _proportionDelta;
	}

	public unsafe void SetProportionDelta(NeiliProportionOfFiveElements proportionDelta, DataContext context)
	{
		_proportionDelta = proportionDelta;
		SetModifiedAndInvalidateInfluencedCache(120, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 613u, 8);
			ptr += _proportionDelta.Serialize(ptr);
		}
	}

	public int GetMindMarkInfinityCount()
	{
		return _mindMarkInfinityCount;
	}

	public unsafe void SetMindMarkInfinityCount(int mindMarkInfinityCount, DataContext context)
	{
		_mindMarkInfinityCount = mindMarkInfinityCount;
		SetModifiedAndInvalidateInfluencedCache(121, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 621u, 4);
			*(int*)ptr = _mindMarkInfinityCount;
			ptr += 4;
		}
	}

	public int GetMindMarkInfinityProgress()
	{
		return _mindMarkInfinityProgress;
	}

	public unsafe void SetMindMarkInfinityProgress(int mindMarkInfinityProgress, DataContext context)
	{
		_mindMarkInfinityProgress = mindMarkInfinityProgress;
		SetModifiedAndInvalidateInfluencedCache(122, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 625u, 4);
			*(int*)ptr = _mindMarkInfinityProgress;
			ptr += 4;
		}
	}

	public List<TeammateCommandDisplayData> GetShowCommandList()
	{
		return _showCommandList;
	}

	public unsafe void SetShowCommandList(List<TeammateCommandDisplayData> showCommandList, DataContext context)
	{
		_showCommandList = showCommandList;
		SetModifiedAndInvalidateInfluencedCache(123, context);
		if (CollectionHelperData.IsArchive)
		{
			int count = _showCommandList.Count;
			int num = 8 * count;
			int valueSize = 2 + num;
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 37, valueSize);
			*(ushort*)ptr = (ushort)count;
			ptr += 2;
			for (int i = 0; i < count; i++)
			{
				ptr += _showCommandList[i].Serialize(ptr);
			}
		}
	}

	public List<int> GetUnlockPrepareValue()
	{
		return _unlockPrepareValue;
	}

	public unsafe void SetUnlockPrepareValue(List<int> unlockPrepareValue, DataContext context)
	{
		_unlockPrepareValue = unlockPrepareValue;
		SetModifiedAndInvalidateInfluencedCache(124, context);
		if (CollectionHelperData.IsArchive)
		{
			int count = _unlockPrepareValue.Count;
			int num = 4 * count;
			int valueSize = 2 + num;
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 38, valueSize);
			*(ushort*)ptr = (ushort)count;
			ptr += 2;
			for (int i = 0; i < count; i++)
			{
				((int*)ptr)[i] = _unlockPrepareValue[i];
			}
			ptr += num;
		}
	}

	public List<int> GetRawCreateEffects()
	{
		return _rawCreateEffects;
	}

	public unsafe void SetRawCreateEffects(List<int> rawCreateEffects, DataContext context)
	{
		_rawCreateEffects = rawCreateEffects;
		SetModifiedAndInvalidateInfluencedCache(125, context);
		if (CollectionHelperData.IsArchive)
		{
			int count = _rawCreateEffects.Count;
			int num = 4 * count;
			int valueSize = 2 + num;
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 39, valueSize);
			*(ushort*)ptr = (ushort)count;
			ptr += 2;
			for (int i = 0; i < count; i++)
			{
				((int*)ptr)[i] = _rawCreateEffects[i];
			}
			ptr += num;
		}
	}

	public RawCreateCollection GetRawCreateCollection()
	{
		return _rawCreateCollection;
	}

	public unsafe void SetRawCreateCollection(RawCreateCollection rawCreateCollection, DataContext context)
	{
		_rawCreateCollection = rawCreateCollection;
		SetModifiedAndInvalidateInfluencedCache(126, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _rawCreateCollection.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 40, serializedSize);
			ptr += _rawCreateCollection.Serialize(ptr);
		}
	}

	public SilenceFrameData GetNormalAttackRecovery()
	{
		return _normalAttackRecovery;
	}

	public unsafe void SetNormalAttackRecovery(SilenceFrameData normalAttackRecovery, DataContext context)
	{
		_normalAttackRecovery = normalAttackRecovery;
		SetModifiedAndInvalidateInfluencedCache(127, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 629u, 8);
			ptr += _normalAttackRecovery.Serialize(ptr);
		}
	}

	public bool GetReserveNormalAttack()
	{
		return _reserveNormalAttack;
	}

	public unsafe void SetReserveNormalAttack(bool reserveNormalAttack, DataContext context)
	{
		_reserveNormalAttack = reserveNormalAttack;
		SetModifiedAndInvalidateInfluencedCache(128, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 637u, 1);
			*ptr = (_reserveNormalAttack ? ((byte)1) : ((byte)0));
			ptr++;
		}
	}

	public int GetGangqi()
	{
		return _gangqi;
	}

	public unsafe void SetGangqi(int gangqi, DataContext context)
	{
		_gangqi = gangqi;
		SetModifiedAndInvalidateInfluencedCache(129, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 638u, 4);
			*(int*)ptr = _gangqi;
			ptr += 4;
		}
	}

	public int GetGangqiMax()
	{
		return _gangqiMax;
	}

	public unsafe void SetGangqiMax(int gangqiMax, DataContext context)
	{
		_gangqiMax = gangqiMax;
		SetModifiedAndInvalidateInfluencedCache(130, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 642u, 4);
			*(int*)ptr = _gangqiMax;
			ptr += 4;
		}
	}

	public int GetMaxTrickCount()
	{
		ObjectCollectionDataStates dataStates = CollectionHelperData.DataStates;
		if (dataStates.IsCached(DataStatesOffset, 131))
		{
			return _maxTrickCount;
		}
		_maxTrickCount = CalcMaxTrickCount();
		dataStates.SetCached(DataStatesOffset, 131);
		return _maxTrickCount;
	}

	public byte GetMobilityLevel()
	{
		ObjectCollectionDataStates dataStates = CollectionHelperData.DataStates;
		if (dataStates.IsCached(DataStatesOffset, 132))
		{
			return _mobilityLevel;
		}
		_mobilityLevel = CalcMobilityLevel();
		dataStates.SetCached(DataStatesOffset, 132);
		return _mobilityLevel;
	}

	public List<bool> GetTeammateCommandCanUse()
	{
		ObjectCollectionDataStates dataStates = CollectionHelperData.DataStates;
		if (dataStates.IsCached(DataStatesOffset, 133))
		{
			return _teammateCommandCanUse;
		}
		CalcTeammateCommandCanUse(_teammateCommandCanUse);
		dataStates.SetCached(DataStatesOffset, 133);
		return _teammateCommandCanUse;
	}

	public float GetChangeDistanceDuration()
	{
		ObjectCollectionDataStates dataStates = CollectionHelperData.DataStates;
		if (dataStates.IsCached(DataStatesOffset, 134))
		{
			return _changeDistanceDuration;
		}
		_changeDistanceDuration = CalcChangeDistanceDuration();
		dataStates.SetCached(DataStatesOffset, 134);
		return _changeDistanceDuration;
	}

	public OuterAndInnerShorts GetAttackRange()
	{
		ObjectCollectionDataStates dataStates = CollectionHelperData.DataStates;
		if (dataStates.IsCached(DataStatesOffset, 135))
		{
			return _attackRange;
		}
		_attackRange = CalcAttackRange();
		dataStates.SetCached(DataStatesOffset, 135);
		return _attackRange;
	}

	public sbyte GetHappiness()
	{
		ObjectCollectionDataStates dataStates = CollectionHelperData.DataStates;
		if (dataStates.IsCached(DataStatesOffset, 136))
		{
			return _happiness;
		}
		_happiness = CalcHappiness();
		dataStates.SetCached(DataStatesOffset, 136);
		return _happiness;
	}

	public SilenceData GetSilenceData()
	{
		ObjectCollectionDataStates dataStates = CollectionHelperData.DataStates;
		if (dataStates.IsCached(DataStatesOffset, 137))
		{
			return _silenceData;
		}
		CalcSilenceData(_silenceData);
		dataStates.SetCached(DataStatesOffset, 137);
		return _silenceData;
	}

	public int GetCombatStateTotalBuffPower()
	{
		ObjectCollectionDataStates dataStates = CollectionHelperData.DataStates;
		if (dataStates.IsCached(DataStatesOffset, 138))
		{
			return _combatStateTotalBuffPower;
		}
		_combatStateTotalBuffPower = CalcCombatStateTotalBuffPower();
		dataStates.SetCached(DataStatesOffset, 138);
		return _combatStateTotalBuffPower;
	}

	public HeavyOrBreakInjuryData GetHeavyOrBreakInjuryData()
	{
		ObjectCollectionDataStates dataStates = CollectionHelperData.DataStates;
		if (dataStates.IsCached(DataStatesOffset, 139))
		{
			return _heavyOrBreakInjuryData;
		}
		_heavyOrBreakInjuryData = CalcHeavyOrBreakInjuryData();
		dataStates.SetCached(DataStatesOffset, 139);
		return _heavyOrBreakInjuryData;
	}

	public short GetMoveCd()
	{
		ObjectCollectionDataStates dataStates = CollectionHelperData.DataStates;
		if (dataStates.IsCached(DataStatesOffset, 140))
		{
			return _moveCd;
		}
		_moveCd = CalcMoveCd();
		dataStates.SetCached(DataStatesOffset, 140);
		return _moveCd;
	}

	public int GetMobilityRecoverSpeed()
	{
		ObjectCollectionDataStates dataStates = CollectionHelperData.DataStates;
		if (dataStates.IsCached(DataStatesOffset, 141))
		{
			return _mobilityRecoverSpeed;
		}
		_mobilityRecoverSpeed = CalcMobilityRecoverSpeed();
		dataStates.SetCached(DataStatesOffset, 141);
		return _mobilityRecoverSpeed;
	}

	public List<bool> GetCanUnlockAttack()
	{
		ObjectCollectionDataStates dataStates = CollectionHelperData.DataStates;
		if (dataStates.IsCached(DataStatesOffset, 142))
		{
			return _canUnlockAttack;
		}
		CalcCanUnlockAttack(_canUnlockAttack);
		dataStates.SetCached(DataStatesOffset, 142);
		return _canUnlockAttack;
	}

	public List<ItemKey> GetValidItems()
	{
		ObjectCollectionDataStates dataStates = CollectionHelperData.DataStates;
		if (dataStates.IsCached(DataStatesOffset, 143))
		{
			return _validItems;
		}
		CalcValidItems(_validItems);
		dataStates.SetCached(DataStatesOffset, 143);
		return _validItems;
	}

	public List<ItemKeyAndCount> GetValidItemAndCounts()
	{
		ObjectCollectionDataStates dataStates = CollectionHelperData.DataStates;
		if (dataStates.IsCached(DataStatesOffset, 144))
		{
			return _validItemAndCounts;
		}
		CalcValidItemAndCounts(_validItemAndCounts);
		dataStates.SetCached(DataStatesOffset, 144);
		return _validItemAndCounts;
	}

	public CombatCharacter()
	{
		_weaponTricks = new sbyte[6];
		_weapons = new ItemKey[7];
		_tricks = new TrickCollection();
		_injuryAutoHealCollection = new InjuryAutoHealCollection();
		_damageStepCollection = new DamageStepCollection();
		_outerDamageValue = new int[7];
		_innerDamageValue = new int[7];
		_outerDamageValueToShow = new IntPair[7];
		_innerDamageValueToShow = new IntPair[7];
		_flawCount = new byte[7];
		_flawCollection = new FlawOrAcupointCollection();
		_acupointCount = new byte[7];
		_acupointCollection = new FlawOrAcupointCollection();
		_mindMarkTime = new MindMarkList();
		_defeatMarkCollection = new DefeatMarkCollection();
		_neigongList = new List<short>();
		_attackSkillList = new List<short>();
		_agileSkillList = new List<short>();
		_defenceSkillList = new List<short>();
		_assistSkillList = new List<short>();
		_otherActionCanUse = new bool[5];
		_buffCombatStateCollection = new CombatStateCollection();
		_debuffCombatStateCollection = new CombatStateCollection();
		_specialCombatStateCollection = new CombatStateCollection();
		_skillEffectCollection = new SkillEffectCollection();
		_showEffectList = new ShowSpecialEffectCollection();
		_animationToLoop = string.Empty;
		_animationToPlayOnce = string.Empty;
		_particleToPlay = string.Empty;
		_particleToLoop = string.Empty;
		_skillPetAnimation = string.Empty;
		_petParticle = string.Empty;
		_attackSoundToPlay = string.Empty;
		_skillSoundToPlay = string.Empty;
		_hitSoundToPlay = string.Empty;
		_armorHitSoundToPlay = string.Empty;
		_whooshSoundToPlay = string.Empty;
		_shockSoundToPlay = string.Empty;
		_stepSoundToPlay = string.Empty;
		_dieSoundToPlay = string.Empty;
		_soundToLoop = string.Empty;
		_currTeammateCommands = new List<sbyte>();
		_teammateCommandCdPercent = new List<byte>();
		_teammateCommandBanReasons = new List<SByteList>();
		_oldInjuryAutoHealCollection = new InjuryAutoHealCollection();
		_mixPoisonAffectedCount = new MixPoisonAffectedCountCollection();
		_particleToLoopByCombatSkill = string.Empty;
		_showCommandList = new List<TeammateCommandDisplayData>();
		_unlockPrepareValue = new List<int>();
		_rawCreateEffects = new List<int>();
		_rawCreateCollection = new RawCreateCollection();
		_teammateCommandCanUse = new List<bool>();
		_silenceData = new SilenceData();
		_canUnlockAttack = new List<bool>();
		_validItems = new List<ItemKey>();
		_validItemAndCounts = new List<ItemKeyAndCount>();
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 810;
		int serializedSize = _tricks.GetSerializedSize();
		num += serializedSize;
		int serializedSize2 = _injuryAutoHealCollection.GetSerializedSize();
		num += serializedSize2;
		int serializedSize3 = _flawCollection.GetSerializedSize();
		num += serializedSize3;
		int serializedSize4 = _acupointCollection.GetSerializedSize();
		num += serializedSize4;
		int serializedSize5 = _mindMarkTime.GetSerializedSize();
		num += serializedSize5;
		int serializedSize6 = _defeatMarkCollection.GetSerializedSize();
		num += serializedSize6;
		int count = _neigongList.Count;
		int num2 = 2 * count;
		int num3 = 2 + num2;
		num += num3;
		int count2 = _attackSkillList.Count;
		int num4 = 2 * count2;
		int num5 = 2 + num4;
		num += num5;
		int count3 = _agileSkillList.Count;
		int num6 = 2 * count3;
		int num7 = 2 + num6;
		num += num7;
		int count4 = _defenceSkillList.Count;
		int num8 = 2 * count4;
		int num9 = 2 + num8;
		num += num9;
		int count5 = _assistSkillList.Count;
		int num10 = 2 * count5;
		int num11 = 2 + num10;
		num += num11;
		int serializedSize7 = _buffCombatStateCollection.GetSerializedSize();
		num += serializedSize7;
		int serializedSize8 = _debuffCombatStateCollection.GetSerializedSize();
		num += serializedSize8;
		int serializedSize9 = _specialCombatStateCollection.GetSerializedSize();
		num += serializedSize9;
		int serializedSize10 = _skillEffectCollection.GetSerializedSize();
		num += serializedSize10;
		int serializedSize11 = _showEffectList.GetSerializedSize();
		num += serializedSize11;
		int length = _animationToLoop.Length;
		int num12 = 2 * length;
		int num13 = 4 + num12;
		num += num13;
		int length2 = _animationToPlayOnce.Length;
		int num14 = 2 * length2;
		int num15 = 4 + num14;
		num += num15;
		int length3 = _particleToPlay.Length;
		int num16 = 2 * length3;
		int num17 = 4 + num16;
		num += num17;
		int length4 = _particleToLoop.Length;
		int num18 = 2 * length4;
		int num19 = 4 + num18;
		num += num19;
		int length5 = _skillPetAnimation.Length;
		int num20 = 2 * length5;
		int num21 = 4 + num20;
		num += num21;
		int length6 = _petParticle.Length;
		int num22 = 2 * length6;
		int num23 = 4 + num22;
		num += num23;
		int length7 = _attackSoundToPlay.Length;
		int num24 = 2 * length7;
		int num25 = 4 + num24;
		num += num25;
		int length8 = _skillSoundToPlay.Length;
		int num26 = 2 * length8;
		int num27 = 4 + num26;
		num += num27;
		int length9 = _hitSoundToPlay.Length;
		int num28 = 2 * length9;
		int num29 = 4 + num28;
		num += num29;
		int length10 = _armorHitSoundToPlay.Length;
		int num30 = 2 * length10;
		int num31 = 4 + num30;
		num += num31;
		int length11 = _whooshSoundToPlay.Length;
		int num32 = 2 * length11;
		int num33 = 4 + num32;
		num += num33;
		int length12 = _shockSoundToPlay.Length;
		int num34 = 2 * length12;
		int num35 = 4 + num34;
		num += num35;
		int length13 = _stepSoundToPlay.Length;
		int num36 = 2 * length13;
		int num37 = 4 + num36;
		num += num37;
		int length14 = _dieSoundToPlay.Length;
		int num38 = 2 * length14;
		int num39 = 4 + num38;
		num += num39;
		int length15 = _soundToLoop.Length;
		int num40 = 2 * length15;
		int num41 = 4 + num40;
		num += num41;
		int count6 = _currTeammateCommands.Count;
		int num42 = count6;
		int num43 = 2 + num42;
		num += num43;
		int count7 = _teammateCommandCdPercent.Count;
		int num44 = count7;
		int num45 = 2 + num44;
		num += num45;
		int num46 = 2;
		int count8 = _teammateCommandBanReasons.Count;
		for (int i = 0; i < count8; i++)
		{
			num46 += _teammateCommandBanReasons[i].GetSerializedSize();
		}
		num += num46;
		int serializedSize12 = _oldInjuryAutoHealCollection.GetSerializedSize();
		num += serializedSize12;
		int serializedSize13 = _mixPoisonAffectedCount.GetSerializedSize();
		num += serializedSize13;
		int length16 = _particleToLoopByCombatSkill.Length;
		int num47 = 2 * length16;
		int num48 = 4 + num47;
		num += num48;
		int count9 = _showCommandList.Count;
		int num49 = 8 * count9;
		int num50 = 2 + num49;
		num += num50;
		int count10 = _unlockPrepareValue.Count;
		int num51 = 4 * count10;
		int num52 = 2 + num51;
		num += num52;
		int count11 = _rawCreateEffects.Count;
		int num53 = 4 * count11;
		int num54 = 2 + num53;
		num += num54;
		int serializedSize14 = _rawCreateCollection.GetSerializedSize();
		return num + serializedSize14;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		*(int*)ptr = _id;
		ptr += 4;
		*(int*)ptr = _breathValue;
		ptr += 4;
		*(int*)ptr = _stanceValue;
		ptr += 4;
		ptr += _neiliAllocation.Serialize(ptr);
		ptr += _originNeiliAllocation.Serialize(ptr);
		ptr += _neiliAllocationRecoverProgress.Serialize(ptr);
		*(short*)ptr = _oldDisorderOfQi;
		ptr += 2;
		*ptr = (byte)_neiliType;
		ptr++;
		ptr += _avoidToShow.Serialize(ptr);
		*(int*)ptr = _currentPosition;
		ptr += 4;
		*(int*)ptr = _displayPosition;
		ptr += 4;
		*(int*)ptr = _mobilityValue;
		ptr += 4;
		*ptr = (byte)_jumpPrepareProgress;
		ptr++;
		*(short*)ptr = _jumpPreparedDistance;
		ptr += 2;
		*(short*)ptr = _mobilityLockEffectCount;
		ptr += 2;
		*(float*)ptr = _jumpChangeDistanceDuration;
		ptr += 4;
		*(int*)ptr = _usingWeaponIndex;
		ptr += 4;
		if (_weaponTricks.Length != 6)
		{
			throw new Exception("Elements count of field _weaponTricks is not equal to declaration");
		}
		for (int i = 0; i < 6; i++)
		{
			ptr[i] = (byte)_weaponTricks[i];
		}
		ptr += 6;
		*ptr = _weaponTrickIndex;
		ptr++;
		if (_weapons.Length != 7)
		{
			throw new Exception("Elements count of field _weapons is not equal to declaration");
		}
		for (int j = 0; j < 7; j++)
		{
			ptr += _weapons[j].Serialize(ptr);
		}
		*ptr = (byte)_attackingTrickType;
		ptr++;
		*ptr = (_canAttackOutRange ? ((byte)1) : ((byte)0));
		ptr++;
		*ptr = (byte)_changeTrickProgress;
		ptr++;
		*(short*)ptr = _changeTrickCount;
		ptr += 2;
		*ptr = (_canChangeTrick ? ((byte)1) : ((byte)0));
		ptr++;
		*ptr = (_changingTrick ? ((byte)1) : ((byte)0));
		ptr++;
		*ptr = (_changeTrickAttack ? ((byte)1) : ((byte)0));
		ptr++;
		*ptr = (_isFightBack ? ((byte)1) : ((byte)0));
		ptr++;
		ptr += _injuries.Serialize(ptr);
		ptr += _oldInjuries.Serialize(ptr);
		ptr += _damageStepCollection.Serialize(ptr);
		if (_outerDamageValue.Length != 7)
		{
			throw new Exception("Elements count of field _outerDamageValue is not equal to declaration");
		}
		for (int k = 0; k < 7; k++)
		{
			((int*)ptr)[k] = _outerDamageValue[k];
		}
		ptr += 28;
		if (_innerDamageValue.Length != 7)
		{
			throw new Exception("Elements count of field _innerDamageValue is not equal to declaration");
		}
		for (int l = 0; l < 7; l++)
		{
			((int*)ptr)[l] = _innerDamageValue[l];
		}
		ptr += 28;
		*(int*)ptr = _mindDamageValue;
		ptr += 4;
		*(int*)ptr = _fatalDamageValue;
		ptr += 4;
		if (_outerDamageValueToShow.Length != 7)
		{
			throw new Exception("Elements count of field _outerDamageValueToShow is not equal to declaration");
		}
		for (int m = 0; m < 7; m++)
		{
			ptr += _outerDamageValueToShow[m].Serialize(ptr);
		}
		if (_innerDamageValueToShow.Length != 7)
		{
			throw new Exception("Elements count of field _innerDamageValueToShow is not equal to declaration");
		}
		for (int n = 0; n < 7; n++)
		{
			ptr += _innerDamageValueToShow[n].Serialize(ptr);
		}
		*(int*)ptr = _mindDamageValueToShow;
		ptr += 4;
		*(int*)ptr = _fatalDamageValueToShow;
		ptr += 4;
		if (_flawCount.Length != 7)
		{
			throw new Exception("Elements count of field _flawCount is not equal to declaration");
		}
		for (int num = 0; num < 7; num++)
		{
			ptr[num] = _flawCount[num];
		}
		ptr += 7;
		if (_acupointCount.Length != 7)
		{
			throw new Exception("Elements count of field _acupointCount is not equal to declaration");
		}
		for (int num2 = 0; num2 < 7; num2++)
		{
			ptr[num2] = _acupointCount[num2];
		}
		ptr += 7;
		ptr += _poison.Serialize(ptr);
		ptr += _oldPoison.Serialize(ptr);
		ptr += _poisonResist.Serialize(ptr);
		ptr += _newPoisonsToShow.Serialize(ptr);
		*(short*)ptr = _preparingSkillId;
		ptr += 2;
		*ptr = _skillPreparePercent;
		ptr++;
		*(short*)ptr = _performingSkillId;
		ptr += 2;
		*ptr = (_autoCastingSkill ? ((byte)1) : ((byte)0));
		ptr++;
		*ptr = _attackSkillAttackIndex;
		ptr++;
		*ptr = _attackSkillPower;
		ptr++;
		*(short*)ptr = _affectingMoveSkillId;
		ptr += 2;
		*(short*)ptr = _affectingDefendSkillId;
		ptr += 2;
		*ptr = _defendSkillTimePercent;
		ptr++;
		*(short*)ptr = _wugCount;
		ptr += 2;
		*ptr = _healInjuryCount;
		ptr++;
		*ptr = _healPoisonCount;
		ptr++;
		if (_otherActionCanUse.Length != 5)
		{
			throw new Exception("Elements count of field _otherActionCanUse is not equal to declaration");
		}
		for (int num3 = 0; num3 < 5; num3++)
		{
			ptr[num3] = (_otherActionCanUse[num3] ? ((byte)1) : ((byte)0));
		}
		ptr += 5;
		*ptr = (byte)_preparingOtherAction;
		ptr++;
		*ptr = _otherActionPreparePercent;
		ptr++;
		*ptr = (_canSurrender ? ((byte)1) : ((byte)0));
		ptr++;
		*ptr = (_canUseItem ? ((byte)1) : ((byte)0));
		ptr++;
		ptr += _preparingItem.Serialize(ptr);
		*ptr = _useItemPreparePercent;
		ptr++;
		ptr += _combatReserveData.Serialize(ptr);
		*(short*)ptr = _xiangshuEffectId;
		ptr += 2;
		*(int*)ptr = _hazardValue;
		ptr += 4;
		*(float*)ptr = _animationTimeScale;
		ptr += 4;
		*ptr = (_attackOutOfRange ? ((byte)1) : ((byte)0));
		ptr++;
		*ptr = (byte)_bossPhase;
		ptr++;
		*ptr = (byte)_animalAttackCount;
		ptr++;
		*ptr = (_showTransferInjuryCommand ? ((byte)1) : ((byte)0));
		ptr++;
		*ptr = (byte)_executingTeammateCommand;
		ptr++;
		*ptr = (_visible ? ((byte)1) : ((byte)0));
		ptr++;
		*ptr = _teammateCommandPreparePercent;
		ptr++;
		*ptr = _teammateCommandTimePercent;
		ptr++;
		ptr += _attackCommandWeaponKey.Serialize(ptr);
		*ptr = (byte)_attackCommandTrickType;
		ptr++;
		*(short*)ptr = _defendCommandSkillId;
		ptr += 2;
		*ptr = (byte)_showEffectCommandIndex;
		ptr++;
		*(short*)ptr = _attackCommandSkillId;
		ptr += 2;
		*(short*)ptr = _targetDistance;
		ptr += 2;
		ptr += _neiliAllocationCd.Serialize(ptr);
		ptr += _proportionDelta.Serialize(ptr);
		*(int*)ptr = _mindMarkInfinityCount;
		ptr += 4;
		*(int*)ptr = _mindMarkInfinityProgress;
		ptr += 4;
		ptr += _normalAttackRecovery.Serialize(ptr);
		*ptr = (_reserveNormalAttack ? ((byte)1) : ((byte)0));
		ptr++;
		*(int*)ptr = _gangqi;
		ptr += 4;
		*(int*)ptr = _gangqiMax;
		ptr += 4;
		byte* ptr2 = ptr;
		ptr += 4;
		ptr += _tricks.Serialize(ptr);
		int num4 = (int)(ptr - ptr2 - 4);
		if (num4 > 4194304)
		{
			throw new Exception($"Size of field {"_tricks"} must be less than {4096}KB");
		}
		*(int*)ptr2 = num4;
		byte* ptr3 = ptr;
		ptr += 4;
		ptr += _injuryAutoHealCollection.Serialize(ptr);
		int num5 = (int)(ptr - ptr3 - 4);
		if (num5 > 4194304)
		{
			throw new Exception($"Size of field {"_injuryAutoHealCollection"} must be less than {4096}KB");
		}
		*(int*)ptr3 = num5;
		byte* ptr4 = ptr;
		ptr += 4;
		ptr += _flawCollection.Serialize(ptr);
		int num6 = (int)(ptr - ptr4 - 4);
		if (num6 > 4194304)
		{
			throw new Exception($"Size of field {"_flawCollection"} must be less than {4096}KB");
		}
		*(int*)ptr4 = num6;
		byte* ptr5 = ptr;
		ptr += 4;
		ptr += _acupointCollection.Serialize(ptr);
		int num7 = (int)(ptr - ptr5 - 4);
		if (num7 > 4194304)
		{
			throw new Exception($"Size of field {"_acupointCollection"} must be less than {4096}KB");
		}
		*(int*)ptr5 = num7;
		byte* ptr6 = ptr;
		ptr += 4;
		ptr += _mindMarkTime.Serialize(ptr);
		int num8 = (int)(ptr - ptr6 - 4);
		if (num8 > 4194304)
		{
			throw new Exception($"Size of field {"_mindMarkTime"} must be less than {4096}KB");
		}
		*(int*)ptr6 = num8;
		byte* ptr7 = ptr;
		ptr += 4;
		ptr += _defeatMarkCollection.Serialize(ptr);
		int num9 = (int)(ptr - ptr7 - 4);
		if (num9 > 4194304)
		{
			throw new Exception($"Size of field {"_defeatMarkCollection"} must be less than {4096}KB");
		}
		*(int*)ptr7 = num9;
		int count = _neigongList.Count;
		int num10 = 2 * count;
		if (num10 > 4194300)
		{
			throw new Exception($"Size of field {"_neigongList"} must be less than {4096}KB");
		}
		*(int*)ptr = num10 + 2;
		ptr += 4;
		*(ushort*)ptr = (ushort)count;
		ptr += 2;
		for (int num11 = 0; num11 < count; num11++)
		{
			((short*)ptr)[num11] = _neigongList[num11];
		}
		ptr += num10;
		int count2 = _attackSkillList.Count;
		int num12 = 2 * count2;
		if (num12 > 4194300)
		{
			throw new Exception($"Size of field {"_attackSkillList"} must be less than {4096}KB");
		}
		*(int*)ptr = num12 + 2;
		ptr += 4;
		*(ushort*)ptr = (ushort)count2;
		ptr += 2;
		for (int num13 = 0; num13 < count2; num13++)
		{
			((short*)ptr)[num13] = _attackSkillList[num13];
		}
		ptr += num12;
		int count3 = _agileSkillList.Count;
		int num14 = 2 * count3;
		if (num14 > 4194300)
		{
			throw new Exception($"Size of field {"_agileSkillList"} must be less than {4096}KB");
		}
		*(int*)ptr = num14 + 2;
		ptr += 4;
		*(ushort*)ptr = (ushort)count3;
		ptr += 2;
		for (int num15 = 0; num15 < count3; num15++)
		{
			((short*)ptr)[num15] = _agileSkillList[num15];
		}
		ptr += num14;
		int count4 = _defenceSkillList.Count;
		int num16 = 2 * count4;
		if (num16 > 4194300)
		{
			throw new Exception($"Size of field {"_defenceSkillList"} must be less than {4096}KB");
		}
		*(int*)ptr = num16 + 2;
		ptr += 4;
		*(ushort*)ptr = (ushort)count4;
		ptr += 2;
		for (int num17 = 0; num17 < count4; num17++)
		{
			((short*)ptr)[num17] = _defenceSkillList[num17];
		}
		ptr += num16;
		int count5 = _assistSkillList.Count;
		int num18 = 2 * count5;
		if (num18 > 4194300)
		{
			throw new Exception($"Size of field {"_assistSkillList"} must be less than {4096}KB");
		}
		*(int*)ptr = num18 + 2;
		ptr += 4;
		*(ushort*)ptr = (ushort)count5;
		ptr += 2;
		for (int num19 = 0; num19 < count5; num19++)
		{
			((short*)ptr)[num19] = _assistSkillList[num19];
		}
		ptr += num18;
		byte* ptr8 = ptr;
		ptr += 4;
		ptr += _buffCombatStateCollection.Serialize(ptr);
		int num20 = (int)(ptr - ptr8 - 4);
		if (num20 > 4194304)
		{
			throw new Exception($"Size of field {"_buffCombatStateCollection"} must be less than {4096}KB");
		}
		*(int*)ptr8 = num20;
		byte* ptr9 = ptr;
		ptr += 4;
		ptr += _debuffCombatStateCollection.Serialize(ptr);
		int num21 = (int)(ptr - ptr9 - 4);
		if (num21 > 4194304)
		{
			throw new Exception($"Size of field {"_debuffCombatStateCollection"} must be less than {4096}KB");
		}
		*(int*)ptr9 = num21;
		byte* ptr10 = ptr;
		ptr += 4;
		ptr += _specialCombatStateCollection.Serialize(ptr);
		int num22 = (int)(ptr - ptr10 - 4);
		if (num22 > 4194304)
		{
			throw new Exception($"Size of field {"_specialCombatStateCollection"} must be less than {4096}KB");
		}
		*(int*)ptr10 = num22;
		byte* ptr11 = ptr;
		ptr += 4;
		ptr += _skillEffectCollection.Serialize(ptr);
		int num23 = (int)(ptr - ptr11 - 4);
		if (num23 > 4194304)
		{
			throw new Exception($"Size of field {"_skillEffectCollection"} must be less than {4096}KB");
		}
		*(int*)ptr11 = num23;
		byte* ptr12 = ptr;
		ptr += 4;
		ptr += _showEffectList.Serialize(ptr);
		int num24 = (int)(ptr - ptr12 - 4);
		if (num24 > 4194304)
		{
			throw new Exception($"Size of field {"_showEffectList"} must be less than {4096}KB");
		}
		*(int*)ptr12 = num24;
		int length = _animationToLoop.Length;
		int num25 = 2 * length;
		if (num25 > 4194300)
		{
			throw new Exception($"Size of field {"_animationToLoop"} must be less than {4096}KB");
		}
		*(int*)ptr = num25 + 4;
		ptr += 4;
		*(int*)ptr = num25;
		ptr += 4;
		fixed (char* animationToLoop = _animationToLoop)
		{
			for (int num26 = 0; num26 < length; num26++)
			{
				((short*)ptr)[num26] = (short)animationToLoop[num26];
			}
		}
		ptr += num25;
		int length2 = _animationToPlayOnce.Length;
		int num27 = 2 * length2;
		if (num27 > 4194300)
		{
			throw new Exception($"Size of field {"_animationToPlayOnce"} must be less than {4096}KB");
		}
		*(int*)ptr = num27 + 4;
		ptr += 4;
		*(int*)ptr = num27;
		ptr += 4;
		fixed (char* animationToPlayOnce = _animationToPlayOnce)
		{
			for (int num28 = 0; num28 < length2; num28++)
			{
				((short*)ptr)[num28] = (short)animationToPlayOnce[num28];
			}
		}
		ptr += num27;
		int length3 = _particleToPlay.Length;
		int num29 = 2 * length3;
		if (num29 > 4194300)
		{
			throw new Exception($"Size of field {"_particleToPlay"} must be less than {4096}KB");
		}
		*(int*)ptr = num29 + 4;
		ptr += 4;
		*(int*)ptr = num29;
		ptr += 4;
		fixed (char* particleToPlay = _particleToPlay)
		{
			for (int num30 = 0; num30 < length3; num30++)
			{
				((short*)ptr)[num30] = (short)particleToPlay[num30];
			}
		}
		ptr += num29;
		int length4 = _particleToLoop.Length;
		int num31 = 2 * length4;
		if (num31 > 4194300)
		{
			throw new Exception($"Size of field {"_particleToLoop"} must be less than {4096}KB");
		}
		*(int*)ptr = num31 + 4;
		ptr += 4;
		*(int*)ptr = num31;
		ptr += 4;
		fixed (char* particleToLoop = _particleToLoop)
		{
			for (int num32 = 0; num32 < length4; num32++)
			{
				((short*)ptr)[num32] = (short)particleToLoop[num32];
			}
		}
		ptr += num31;
		int length5 = _skillPetAnimation.Length;
		int num33 = 2 * length5;
		if (num33 > 4194300)
		{
			throw new Exception($"Size of field {"_skillPetAnimation"} must be less than {4096}KB");
		}
		*(int*)ptr = num33 + 4;
		ptr += 4;
		*(int*)ptr = num33;
		ptr += 4;
		fixed (char* skillPetAnimation = _skillPetAnimation)
		{
			for (int num34 = 0; num34 < length5; num34++)
			{
				((short*)ptr)[num34] = (short)skillPetAnimation[num34];
			}
		}
		ptr += num33;
		int length6 = _petParticle.Length;
		int num35 = 2 * length6;
		if (num35 > 4194300)
		{
			throw new Exception($"Size of field {"_petParticle"} must be less than {4096}KB");
		}
		*(int*)ptr = num35 + 4;
		ptr += 4;
		*(int*)ptr = num35;
		ptr += 4;
		fixed (char* petParticle = _petParticle)
		{
			for (int num36 = 0; num36 < length6; num36++)
			{
				((short*)ptr)[num36] = (short)petParticle[num36];
			}
		}
		ptr += num35;
		int length7 = _attackSoundToPlay.Length;
		int num37 = 2 * length7;
		if (num37 > 4194300)
		{
			throw new Exception($"Size of field {"_attackSoundToPlay"} must be less than {4096}KB");
		}
		*(int*)ptr = num37 + 4;
		ptr += 4;
		*(int*)ptr = num37;
		ptr += 4;
		fixed (char* attackSoundToPlay = _attackSoundToPlay)
		{
			for (int num38 = 0; num38 < length7; num38++)
			{
				((short*)ptr)[num38] = (short)attackSoundToPlay[num38];
			}
		}
		ptr += num37;
		int length8 = _skillSoundToPlay.Length;
		int num39 = 2 * length8;
		if (num39 > 4194300)
		{
			throw new Exception($"Size of field {"_skillSoundToPlay"} must be less than {4096}KB");
		}
		*(int*)ptr = num39 + 4;
		ptr += 4;
		*(int*)ptr = num39;
		ptr += 4;
		fixed (char* skillSoundToPlay = _skillSoundToPlay)
		{
			for (int num40 = 0; num40 < length8; num40++)
			{
				((short*)ptr)[num40] = (short)skillSoundToPlay[num40];
			}
		}
		ptr += num39;
		int length9 = _hitSoundToPlay.Length;
		int num41 = 2 * length9;
		if (num41 > 4194300)
		{
			throw new Exception($"Size of field {"_hitSoundToPlay"} must be less than {4096}KB");
		}
		*(int*)ptr = num41 + 4;
		ptr += 4;
		*(int*)ptr = num41;
		ptr += 4;
		fixed (char* hitSoundToPlay = _hitSoundToPlay)
		{
			for (int num42 = 0; num42 < length9; num42++)
			{
				((short*)ptr)[num42] = (short)hitSoundToPlay[num42];
			}
		}
		ptr += num41;
		int length10 = _armorHitSoundToPlay.Length;
		int num43 = 2 * length10;
		if (num43 > 4194300)
		{
			throw new Exception($"Size of field {"_armorHitSoundToPlay"} must be less than {4096}KB");
		}
		*(int*)ptr = num43 + 4;
		ptr += 4;
		*(int*)ptr = num43;
		ptr += 4;
		fixed (char* armorHitSoundToPlay = _armorHitSoundToPlay)
		{
			for (int num44 = 0; num44 < length10; num44++)
			{
				((short*)ptr)[num44] = (short)armorHitSoundToPlay[num44];
			}
		}
		ptr += num43;
		int length11 = _whooshSoundToPlay.Length;
		int num45 = 2 * length11;
		if (num45 > 4194300)
		{
			throw new Exception($"Size of field {"_whooshSoundToPlay"} must be less than {4096}KB");
		}
		*(int*)ptr = num45 + 4;
		ptr += 4;
		*(int*)ptr = num45;
		ptr += 4;
		fixed (char* whooshSoundToPlay = _whooshSoundToPlay)
		{
			for (int num46 = 0; num46 < length11; num46++)
			{
				((short*)ptr)[num46] = (short)whooshSoundToPlay[num46];
			}
		}
		ptr += num45;
		int length12 = _shockSoundToPlay.Length;
		int num47 = 2 * length12;
		if (num47 > 4194300)
		{
			throw new Exception($"Size of field {"_shockSoundToPlay"} must be less than {4096}KB");
		}
		*(int*)ptr = num47 + 4;
		ptr += 4;
		*(int*)ptr = num47;
		ptr += 4;
		fixed (char* shockSoundToPlay = _shockSoundToPlay)
		{
			for (int num48 = 0; num48 < length12; num48++)
			{
				((short*)ptr)[num48] = (short)shockSoundToPlay[num48];
			}
		}
		ptr += num47;
		int length13 = _stepSoundToPlay.Length;
		int num49 = 2 * length13;
		if (num49 > 4194300)
		{
			throw new Exception($"Size of field {"_stepSoundToPlay"} must be less than {4096}KB");
		}
		*(int*)ptr = num49 + 4;
		ptr += 4;
		*(int*)ptr = num49;
		ptr += 4;
		fixed (char* stepSoundToPlay = _stepSoundToPlay)
		{
			for (int num50 = 0; num50 < length13; num50++)
			{
				((short*)ptr)[num50] = (short)stepSoundToPlay[num50];
			}
		}
		ptr += num49;
		int length14 = _dieSoundToPlay.Length;
		int num51 = 2 * length14;
		if (num51 > 4194300)
		{
			throw new Exception($"Size of field {"_dieSoundToPlay"} must be less than {4096}KB");
		}
		*(int*)ptr = num51 + 4;
		ptr += 4;
		*(int*)ptr = num51;
		ptr += 4;
		fixed (char* dieSoundToPlay = _dieSoundToPlay)
		{
			for (int num52 = 0; num52 < length14; num52++)
			{
				((short*)ptr)[num52] = (short)dieSoundToPlay[num52];
			}
		}
		ptr += num51;
		int length15 = _soundToLoop.Length;
		int num53 = 2 * length15;
		if (num53 > 4194300)
		{
			throw new Exception($"Size of field {"_soundToLoop"} must be less than {4096}KB");
		}
		*(int*)ptr = num53 + 4;
		ptr += 4;
		*(int*)ptr = num53;
		ptr += 4;
		fixed (char* soundToLoop = _soundToLoop)
		{
			for (int num54 = 0; num54 < length15; num54++)
			{
				((short*)ptr)[num54] = (short)soundToLoop[num54];
			}
		}
		ptr += num53;
		int count6 = _currTeammateCommands.Count;
		int num55 = count6;
		if (num55 > 4194300)
		{
			throw new Exception($"Size of field {"_currTeammateCommands"} must be less than {4096}KB");
		}
		*(int*)ptr = num55 + 2;
		ptr += 4;
		*(ushort*)ptr = (ushort)count6;
		ptr += 2;
		for (int num56 = 0; num56 < count6; num56++)
		{
			ptr[num56] = (byte)_currTeammateCommands[num56];
		}
		ptr += num55;
		int count7 = _teammateCommandCdPercent.Count;
		int num57 = count7;
		if (num57 > 4194300)
		{
			throw new Exception($"Size of field {"_teammateCommandCdPercent"} must be less than {4096}KB");
		}
		*(int*)ptr = num57 + 2;
		ptr += 4;
		*(ushort*)ptr = (ushort)count7;
		ptr += 2;
		for (int num58 = 0; num58 < count7; num58++)
		{
			ptr[num58] = _teammateCommandCdPercent[num58];
		}
		ptr += num57;
		int count8 = _teammateCommandBanReasons.Count;
		byte* ptr13 = ptr;
		ptr += 4;
		*(ushort*)ptr = (ushort)count8;
		ptr += 2;
		for (int num59 = 0; num59 < count8; num59++)
		{
			ptr += _teammateCommandBanReasons[num59].Serialize(ptr);
		}
		int num60 = (int)(ptr - ptr13 - 4);
		if (num60 > 4194304)
		{
			throw new Exception($"Size of field {"_teammateCommandBanReasons"} must be less than {4096}KB");
		}
		*(int*)ptr13 = num60;
		byte* ptr14 = ptr;
		ptr += 4;
		ptr += _oldInjuryAutoHealCollection.Serialize(ptr);
		int num61 = (int)(ptr - ptr14 - 4);
		if (num61 > 4194304)
		{
			throw new Exception($"Size of field {"_oldInjuryAutoHealCollection"} must be less than {4096}KB");
		}
		*(int*)ptr14 = num61;
		byte* ptr15 = ptr;
		ptr += 4;
		ptr += _mixPoisonAffectedCount.Serialize(ptr);
		int num62 = (int)(ptr - ptr15 - 4);
		if (num62 > 4194304)
		{
			throw new Exception($"Size of field {"_mixPoisonAffectedCount"} must be less than {4096}KB");
		}
		*(int*)ptr15 = num62;
		int length16 = _particleToLoopByCombatSkill.Length;
		int num63 = 2 * length16;
		if (num63 > 4194300)
		{
			throw new Exception($"Size of field {"_particleToLoopByCombatSkill"} must be less than {4096}KB");
		}
		*(int*)ptr = num63 + 4;
		ptr += 4;
		*(int*)ptr = num63;
		ptr += 4;
		fixed (char* particleToLoopByCombatSkill = _particleToLoopByCombatSkill)
		{
			for (int num64 = 0; num64 < length16; num64++)
			{
				((short*)ptr)[num64] = (short)particleToLoopByCombatSkill[num64];
			}
		}
		ptr += num63;
		int count9 = _showCommandList.Count;
		int num65 = 8 * count9;
		if (num65 > 4194300)
		{
			throw new Exception($"Size of field {"_showCommandList"} must be less than {4096}KB");
		}
		*(int*)ptr = num65 + 2;
		ptr += 4;
		*(ushort*)ptr = (ushort)count9;
		ptr += 2;
		for (int num66 = 0; num66 < count9; num66++)
		{
			ptr += _showCommandList[num66].Serialize(ptr);
		}
		int count10 = _unlockPrepareValue.Count;
		int num67 = 4 * count10;
		if (num67 > 4194300)
		{
			throw new Exception($"Size of field {"_unlockPrepareValue"} must be less than {4096}KB");
		}
		*(int*)ptr = num67 + 2;
		ptr += 4;
		*(ushort*)ptr = (ushort)count10;
		ptr += 2;
		for (int num68 = 0; num68 < count10; num68++)
		{
			((int*)ptr)[num68] = _unlockPrepareValue[num68];
		}
		ptr += num67;
		int count11 = _rawCreateEffects.Count;
		int num69 = 4 * count11;
		if (num69 > 4194300)
		{
			throw new Exception($"Size of field {"_rawCreateEffects"} must be less than {4096}KB");
		}
		*(int*)ptr = num69 + 2;
		ptr += 4;
		*(ushort*)ptr = (ushort)count11;
		ptr += 2;
		for (int num70 = 0; num70 < count11; num70++)
		{
			((int*)ptr)[num70] = _rawCreateEffects[num70];
		}
		ptr += num69;
		byte* ptr16 = ptr;
		ptr += 4;
		ptr += _rawCreateCollection.Serialize(ptr);
		int num71 = (int)(ptr - ptr16 - 4);
		if (num71 > 4194304)
		{
			throw new Exception($"Size of field {"_rawCreateCollection"} must be less than {4096}KB");
		}
		*(int*)ptr16 = num71;
		return (int)(ptr - pData);
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		_id = *(int*)ptr;
		ptr += 4;
		_breathValue = *(int*)ptr;
		ptr += 4;
		_stanceValue = *(int*)ptr;
		ptr += 4;
		ptr += _neiliAllocation.Deserialize(ptr);
		ptr += _originNeiliAllocation.Deserialize(ptr);
		ptr += _neiliAllocationRecoverProgress.Deserialize(ptr);
		_oldDisorderOfQi = *(short*)ptr;
		ptr += 2;
		_neiliType = (sbyte)(*ptr);
		ptr++;
		ptr += _avoidToShow.Deserialize(ptr);
		_currentPosition = *(int*)ptr;
		ptr += 4;
		_displayPosition = *(int*)ptr;
		ptr += 4;
		_mobilityValue = *(int*)ptr;
		ptr += 4;
		_jumpPrepareProgress = (sbyte)(*ptr);
		ptr++;
		_jumpPreparedDistance = *(short*)ptr;
		ptr += 2;
		_mobilityLockEffectCount = *(short*)ptr;
		ptr += 2;
		_jumpChangeDistanceDuration = *(float*)ptr;
		ptr += 4;
		_usingWeaponIndex = *(int*)ptr;
		ptr += 4;
		if (_weaponTricks.Length != 6)
		{
			throw new Exception("Elements count of field _weaponTricks is not equal to declaration");
		}
		for (int i = 0; i < 6; i++)
		{
			_weaponTricks[i] = (sbyte)ptr[i];
		}
		ptr += 6;
		_weaponTrickIndex = *ptr;
		ptr++;
		if (_weapons.Length != 7)
		{
			throw new Exception("Elements count of field _weapons is not equal to declaration");
		}
		for (int j = 0; j < 7; j++)
		{
			ItemKey itemKey = default(ItemKey);
			ptr += itemKey.Deserialize(ptr);
			_weapons[j] = itemKey;
		}
		_attackingTrickType = (sbyte)(*ptr);
		ptr++;
		_canAttackOutRange = *ptr != 0;
		ptr++;
		_changeTrickProgress = (sbyte)(*ptr);
		ptr++;
		_changeTrickCount = *(short*)ptr;
		ptr += 2;
		_canChangeTrick = *ptr != 0;
		ptr++;
		_changingTrick = *ptr != 0;
		ptr++;
		_changeTrickAttack = *ptr != 0;
		ptr++;
		_isFightBack = *ptr != 0;
		ptr++;
		ptr += _injuries.Deserialize(ptr);
		ptr += _oldInjuries.Deserialize(ptr);
		ptr += _damageStepCollection.Deserialize(ptr);
		if (_outerDamageValue.Length != 7)
		{
			throw new Exception("Elements count of field _outerDamageValue is not equal to declaration");
		}
		for (int k = 0; k < 7; k++)
		{
			_outerDamageValue[k] = ((int*)ptr)[k];
		}
		ptr += 28;
		if (_innerDamageValue.Length != 7)
		{
			throw new Exception("Elements count of field _innerDamageValue is not equal to declaration");
		}
		for (int l = 0; l < 7; l++)
		{
			_innerDamageValue[l] = ((int*)ptr)[l];
		}
		ptr += 28;
		_mindDamageValue = *(int*)ptr;
		ptr += 4;
		_fatalDamageValue = *(int*)ptr;
		ptr += 4;
		if (_outerDamageValueToShow.Length != 7)
		{
			throw new Exception("Elements count of field _outerDamageValueToShow is not equal to declaration");
		}
		for (int m = 0; m < 7; m++)
		{
			IntPair intPair = default(IntPair);
			ptr += intPair.Deserialize(ptr);
			_outerDamageValueToShow[m] = intPair;
		}
		if (_innerDamageValueToShow.Length != 7)
		{
			throw new Exception("Elements count of field _innerDamageValueToShow is not equal to declaration");
		}
		for (int n = 0; n < 7; n++)
		{
			IntPair intPair2 = default(IntPair);
			ptr += intPair2.Deserialize(ptr);
			_innerDamageValueToShow[n] = intPair2;
		}
		_mindDamageValueToShow = *(int*)ptr;
		ptr += 4;
		_fatalDamageValueToShow = *(int*)ptr;
		ptr += 4;
		if (_flawCount.Length != 7)
		{
			throw new Exception("Elements count of field _flawCount is not equal to declaration");
		}
		for (int num = 0; num < 7; num++)
		{
			_flawCount[num] = ptr[num];
		}
		ptr += 7;
		if (_acupointCount.Length != 7)
		{
			throw new Exception("Elements count of field _acupointCount is not equal to declaration");
		}
		for (int num2 = 0; num2 < 7; num2++)
		{
			_acupointCount[num2] = ptr[num2];
		}
		ptr += 7;
		ptr += _poison.Deserialize(ptr);
		ptr += _oldPoison.Deserialize(ptr);
		ptr += _poisonResist.Deserialize(ptr);
		ptr += _newPoisonsToShow.Deserialize(ptr);
		_preparingSkillId = *(short*)ptr;
		ptr += 2;
		_skillPreparePercent = *ptr;
		ptr++;
		_performingSkillId = *(short*)ptr;
		ptr += 2;
		_autoCastingSkill = *ptr != 0;
		ptr++;
		_attackSkillAttackIndex = *ptr;
		ptr++;
		_attackSkillPower = *ptr;
		ptr++;
		_affectingMoveSkillId = *(short*)ptr;
		ptr += 2;
		_affectingDefendSkillId = *(short*)ptr;
		ptr += 2;
		_defendSkillTimePercent = *ptr;
		ptr++;
		_wugCount = *(short*)ptr;
		ptr += 2;
		_healInjuryCount = *ptr;
		ptr++;
		_healPoisonCount = *ptr;
		ptr++;
		if (_otherActionCanUse.Length != 5)
		{
			throw new Exception("Elements count of field _otherActionCanUse is not equal to declaration");
		}
		for (int num3 = 0; num3 < 5; num3++)
		{
			_otherActionCanUse[num3] = ptr[num3] != 0;
		}
		ptr += 5;
		_preparingOtherAction = (sbyte)(*ptr);
		ptr++;
		_otherActionPreparePercent = *ptr;
		ptr++;
		_canSurrender = *ptr != 0;
		ptr++;
		_canUseItem = *ptr != 0;
		ptr++;
		ptr += _preparingItem.Deserialize(ptr);
		_useItemPreparePercent = *ptr;
		ptr++;
		ptr += _combatReserveData.Deserialize(ptr);
		_xiangshuEffectId = *(short*)ptr;
		ptr += 2;
		_hazardValue = *(int*)ptr;
		ptr += 4;
		_animationTimeScale = *(float*)ptr;
		ptr += 4;
		_attackOutOfRange = *ptr != 0;
		ptr++;
		_bossPhase = (sbyte)(*ptr);
		ptr++;
		_animalAttackCount = (sbyte)(*ptr);
		ptr++;
		_showTransferInjuryCommand = *ptr != 0;
		ptr++;
		_executingTeammateCommand = (sbyte)(*ptr);
		ptr++;
		_visible = *ptr != 0;
		ptr++;
		_teammateCommandPreparePercent = *ptr;
		ptr++;
		_teammateCommandTimePercent = *ptr;
		ptr++;
		ptr += _attackCommandWeaponKey.Deserialize(ptr);
		_attackCommandTrickType = (sbyte)(*ptr);
		ptr++;
		_defendCommandSkillId = *(short*)ptr;
		ptr += 2;
		_showEffectCommandIndex = (sbyte)(*ptr);
		ptr++;
		_attackCommandSkillId = *(short*)ptr;
		ptr += 2;
		_targetDistance = *(short*)ptr;
		ptr += 2;
		ptr += _neiliAllocationCd.Deserialize(ptr);
		ptr += _proportionDelta.Deserialize(ptr);
		_mindMarkInfinityCount = *(int*)ptr;
		ptr += 4;
		_mindMarkInfinityProgress = *(int*)ptr;
		ptr += 4;
		ptr += _normalAttackRecovery.Deserialize(ptr);
		_reserveNormalAttack = *ptr != 0;
		ptr++;
		_gangqi = *(int*)ptr;
		ptr += 4;
		_gangqiMax = *(int*)ptr;
		ptr += 4;
		ptr += 4;
		ptr += _tricks.Deserialize(ptr);
		ptr += 4;
		ptr += _injuryAutoHealCollection.Deserialize(ptr);
		ptr += 4;
		ptr += _flawCollection.Deserialize(ptr);
		ptr += 4;
		ptr += _acupointCollection.Deserialize(ptr);
		ptr += 4;
		ptr += _mindMarkTime.Deserialize(ptr);
		ptr += 4;
		ptr += _defeatMarkCollection.Deserialize(ptr);
		ptr += 4;
		ushort num4 = *(ushort*)ptr;
		ptr += 2;
		_neigongList.Clear();
		for (int num5 = 0; num5 < num4; num5++)
		{
			_neigongList.Add(((short*)ptr)[num5]);
		}
		ptr += 2 * num4;
		ptr += 4;
		ushort num6 = *(ushort*)ptr;
		ptr += 2;
		_attackSkillList.Clear();
		for (int num7 = 0; num7 < num6; num7++)
		{
			_attackSkillList.Add(((short*)ptr)[num7]);
		}
		ptr += 2 * num6;
		ptr += 4;
		ushort num8 = *(ushort*)ptr;
		ptr += 2;
		_agileSkillList.Clear();
		for (int num9 = 0; num9 < num8; num9++)
		{
			_agileSkillList.Add(((short*)ptr)[num9]);
		}
		ptr += 2 * num8;
		ptr += 4;
		ushort num10 = *(ushort*)ptr;
		ptr += 2;
		_defenceSkillList.Clear();
		for (int num11 = 0; num11 < num10; num11++)
		{
			_defenceSkillList.Add(((short*)ptr)[num11]);
		}
		ptr += 2 * num10;
		ptr += 4;
		ushort num12 = *(ushort*)ptr;
		ptr += 2;
		_assistSkillList.Clear();
		for (int num13 = 0; num13 < num12; num13++)
		{
			_assistSkillList.Add(((short*)ptr)[num13]);
		}
		ptr += 2 * num12;
		ptr += 4;
		ptr += _buffCombatStateCollection.Deserialize(ptr);
		ptr += 4;
		ptr += _debuffCombatStateCollection.Deserialize(ptr);
		ptr += 4;
		ptr += _specialCombatStateCollection.Deserialize(ptr);
		ptr += 4;
		ptr += _skillEffectCollection.Deserialize(ptr);
		ptr += 4;
		ptr += _showEffectList.Deserialize(ptr);
		ptr += 4;
		uint num14 = *(uint*)ptr;
		ptr += 4;
		_animationToLoop = Encoding.Unicode.GetString(ptr, (int)num14);
		ptr += num14;
		ptr += 4;
		uint num15 = *(uint*)ptr;
		ptr += 4;
		_animationToPlayOnce = Encoding.Unicode.GetString(ptr, (int)num15);
		ptr += num15;
		ptr += 4;
		uint num16 = *(uint*)ptr;
		ptr += 4;
		_particleToPlay = Encoding.Unicode.GetString(ptr, (int)num16);
		ptr += num16;
		ptr += 4;
		uint num17 = *(uint*)ptr;
		ptr += 4;
		_particleToLoop = Encoding.Unicode.GetString(ptr, (int)num17);
		ptr += num17;
		ptr += 4;
		uint num18 = *(uint*)ptr;
		ptr += 4;
		_skillPetAnimation = Encoding.Unicode.GetString(ptr, (int)num18);
		ptr += num18;
		ptr += 4;
		uint num19 = *(uint*)ptr;
		ptr += 4;
		_petParticle = Encoding.Unicode.GetString(ptr, (int)num19);
		ptr += num19;
		ptr += 4;
		uint num20 = *(uint*)ptr;
		ptr += 4;
		_attackSoundToPlay = Encoding.Unicode.GetString(ptr, (int)num20);
		ptr += num20;
		ptr += 4;
		uint num21 = *(uint*)ptr;
		ptr += 4;
		_skillSoundToPlay = Encoding.Unicode.GetString(ptr, (int)num21);
		ptr += num21;
		ptr += 4;
		uint num22 = *(uint*)ptr;
		ptr += 4;
		_hitSoundToPlay = Encoding.Unicode.GetString(ptr, (int)num22);
		ptr += num22;
		ptr += 4;
		uint num23 = *(uint*)ptr;
		ptr += 4;
		_armorHitSoundToPlay = Encoding.Unicode.GetString(ptr, (int)num23);
		ptr += num23;
		ptr += 4;
		uint num24 = *(uint*)ptr;
		ptr += 4;
		_whooshSoundToPlay = Encoding.Unicode.GetString(ptr, (int)num24);
		ptr += num24;
		ptr += 4;
		uint num25 = *(uint*)ptr;
		ptr += 4;
		_shockSoundToPlay = Encoding.Unicode.GetString(ptr, (int)num25);
		ptr += num25;
		ptr += 4;
		uint num26 = *(uint*)ptr;
		ptr += 4;
		_stepSoundToPlay = Encoding.Unicode.GetString(ptr, (int)num26);
		ptr += num26;
		ptr += 4;
		uint num27 = *(uint*)ptr;
		ptr += 4;
		_dieSoundToPlay = Encoding.Unicode.GetString(ptr, (int)num27);
		ptr += num27;
		ptr += 4;
		uint num28 = *(uint*)ptr;
		ptr += 4;
		_soundToLoop = Encoding.Unicode.GetString(ptr, (int)num28);
		ptr += num28;
		ptr += 4;
		ushort num29 = *(ushort*)ptr;
		ptr += 2;
		_currTeammateCommands.Clear();
		for (int num30 = 0; num30 < num29; num30++)
		{
			_currTeammateCommands.Add((sbyte)ptr[num30]);
		}
		ptr += (int)num29;
		ptr += 4;
		ushort num31 = *(ushort*)ptr;
		ptr += 2;
		_teammateCommandCdPercent.Clear();
		for (int num32 = 0; num32 < num31; num32++)
		{
			_teammateCommandCdPercent.Add(ptr[num32]);
		}
		ptr += (int)num31;
		ptr += 4;
		ushort num33 = *(ushort*)ptr;
		ptr += 2;
		_teammateCommandBanReasons.Clear();
		for (int num34 = 0; num34 < num33; num34++)
		{
			SByteList item = default(SByteList);
			ptr += item.Deserialize(ptr);
			_teammateCommandBanReasons.Add(item);
		}
		ptr += 4;
		ptr += _oldInjuryAutoHealCollection.Deserialize(ptr);
		ptr += 4;
		ptr += _mixPoisonAffectedCount.Deserialize(ptr);
		ptr += 4;
		uint num35 = *(uint*)ptr;
		ptr += 4;
		_particleToLoopByCombatSkill = Encoding.Unicode.GetString(ptr, (int)num35);
		ptr += num35;
		ptr += 4;
		ushort num36 = *(ushort*)ptr;
		ptr += 2;
		_showCommandList.Clear();
		for (int num37 = 0; num37 < num36; num37++)
		{
			TeammateCommandDisplayData item2 = default(TeammateCommandDisplayData);
			ptr += item2.Deserialize(ptr);
			_showCommandList.Add(item2);
		}
		ptr += 4;
		ushort num38 = *(ushort*)ptr;
		ptr += 2;
		_unlockPrepareValue.Clear();
		for (int num39 = 0; num39 < num38; num39++)
		{
			_unlockPrepareValue.Add(((int*)ptr)[num39]);
		}
		ptr += 4 * num38;
		ptr += 4;
		ushort num40 = *(ushort*)ptr;
		ptr += 2;
		_rawCreateEffects.Clear();
		for (int num41 = 0; num41 < num40; num41++)
		{
			_rawCreateEffects.Add(((int*)ptr)[num41]);
		}
		ptr += 4 * num40;
		ptr += 4;
		ptr += _rawCreateCollection.Deserialize(ptr);
		return (int)(ptr - pData);
	}
}
