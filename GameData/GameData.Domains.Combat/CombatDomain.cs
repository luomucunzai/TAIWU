using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using Config;
using GameData.ArchiveData;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Common.SingleValueCollection;
using GameData.Dependencies;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.Character.Ai;
using GameData.Domains.Character.Display;
using GameData.Domains.Character.Relation;
using GameData.Domains.Combat.Ai;
using GameData.Domains.Combat.MixPoison;
using GameData.Domains.Combat.Profession;
using GameData.Domains.CombatSkill;
using GameData.Domains.Extra;
using GameData.Domains.Global;
using GameData.Domains.Item;
using GameData.Domains.Item.Display;
using GameData.Domains.Map;
using GameData.Domains.Organization;
using GameData.Domains.SpecialEffect;
using GameData.Domains.SpecialEffect.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Tutorial;
using GameData.Domains.SpecialEffect.CombatSkill.Wuxianjiao.WugEffect;
using GameData.Domains.SpecialEffect.CombatSkill.Xuehoujiao.BreakBodyEffect;
using GameData.Domains.SpecialEffect.SectStory.Baihua;
using GameData.Domains.SpecialEffect.SectStory.Fulong;
using GameData.Domains.Taiwu;
using GameData.Domains.TaiwuEvent;
using GameData.Domains.World;
using GameData.Domains.World.Notification;
using GameData.Domains.World.SectMainStory;
using GameData.GameDataBridge;
using GameData.Serializer;
using GameData.Utilities;
using Redzen.Random;

namespace GameData.Domains.Combat;

[GameDataDomain(8)]
public class CombatDomain : BaseGameDataDomain
{
	public delegate void OnCombatCharAboutToFall(DataContext context, CombatCharacter combatChar, int eventIndex);

	[DomainData(DomainDataType.SingleValue, false, false, true, false)]
	private float _timeScale;

	[DomainData(DomainDataType.SingleValue, false, false, true, false)]
	private bool _autoCombat;

	private float _frameTimeAccumulator;

	[DomainData(DomainDataType.SingleValue, false, false, true, true)]
	private ulong _combatFrame;

	[DomainData(DomainDataType.SingleValue, false, false, true, true)]
	private sbyte _combatType;

	[DomainData(DomainDataType.SingleValue, false, false, true, true)]
	private bool _isPuppetCombat;

	[DomainData(DomainDataType.SingleValue, false, false, true, true)]
	private bool _isPlaygroundCombat;

	public CombatConfigItem CombatConfig;

	private bool _inBulletTime;

	private float _timeScaleSaveInBulletTime;

	public sbyte TestSkillCounter;

	private bool _saveDyingEffectTriggerd;

	public DataContext Context;

	private bool _isTutorialCombat;

	private bool _enableEnemyAi = true;

	private bool _enableSkillFreeCast = false;

	public AiOptions AiOptions = new AiOptions();

	public const short CombatStatePowerLimit = 500;

	public const sbyte MaxFlawCount = 3;

	public const sbyte MaxAcupointCount = 3;

	private const short NoArmorEquipAttack = 100;

	private const short NoArmorEquipDefense = 50;

	public const sbyte MinEffectPercent = 33;

	private readonly string[][] _avoidSound = new string[4][]
	{
		new string[3] { "se_combat_block_combat_1", "se_combat_block_combat_2", "se_combat_block_combat_3" },
		new string[3] { "se_combat_block_combat_1", "se_combat_block_combat_2", "se_combat_block_combat_3" },
		new string[3] { "se_combat_block_dodge_1", "se_combat_block_dodge_2", "se_combat_block_dodge_3" },
		new string[3] { "se_combat_block_dodge_1", "se_combat_block_dodge_2", "se_combat_block_dodge_3" }
	};

	private readonly string[] _selfAvoidParticle = new string[4] { "Particle_H_xieli", "Particle_H_chaizhao", "Particle_H_shanbi", "Particle_H_shouxin" };

	private readonly string[] _enemyAvoidParticle = new string[4] { "Particle_H_xieli_1", "Particle_H_chaizhao_1", "Particle_H_shanbi_1", "Particle_H_shouxin_1" };

	private const string NoDamageParticle = "Particle_D_qidun";

	private const sbyte AnimalAttackPosOffset = 8;

	public static readonly Dictionary<short, short> BossPuppet2BossId = new Dictionary<short, short>
	{
		[490] = 48,
		[491] = 57,
		[492] = 66,
		[493] = 75,
		[494] = 84,
		[495] = 93,
		[496] = 102,
		[497] = 111,
		[498] = 120
	};

	public static readonly Dictionary<short, sbyte> CharId2BossId = new Dictionary<short, sbyte>();

	public const string IdleAni = "C_000";

	public const string WeakIdleAni = "C_000";

	public const string WalkForwardAni = "M_001";

	public const string WalkBackwardAni = "M_002";

	public const string WalkForwardFastAni = "MR_001";

	public const string WalkBackwardFastAni = "MR_002";

	public const string SkillMoveForwardAni = "M_003";

	public const string SkillMoveBackwardAni = "M_004";

	public const string JumpMovePrepareAni = "C_007";

	public const string JumpMoveForwardAni = "M_003_fly";

	public const string JumpMoveBackwardAni = "M_004_fly";

	public const string SlowForwardAni = "M_014";

	public const string SlowBackwardAni = "M_015";

	[DomainData(DomainDataType.SingleValue, false, false, true, true)]
	private short _currentDistance;

	private readonly CombatResultDisplayData _combatResultData = new CombatResultDisplayData();

	private static readonly Dictionary<ECombatEvaluationExtraCheck, Func<bool>> EvaluationCheckers = new Dictionary<ECombatEvaluationExtraCheck, Func<bool>>
	{
		[ECombatEvaluationExtraCheck.Fail] = FailChecker,
		[ECombatEvaluationExtraCheck.Draw] = DrawChecker,
		[ECombatEvaluationExtraCheck.Flee] = FleeChecker,
		[ECombatEvaluationExtraCheck.Win] = WinChecker,
		[ECombatEvaluationExtraCheck.FightSameLevel] = FightSameLevelChecker,
		[ECombatEvaluationExtraCheck.FightLessLevel] = FightLessLevelChecker,
		[ECombatEvaluationExtraCheck.BeatXiangShu] = BeatXiangShuChecker,
		[ECombatEvaluationExtraCheck.WinLess] = WinLessChecker,
		[ECombatEvaluationExtraCheck.WinChild] = WinChildChecker,
		[ECombatEvaluationExtraCheck.WinWorseEquip] = WinWorseEquipChecker,
		[ECombatEvaluationExtraCheck.WinLessNeili] = WinLessNeiliChecker,
		[ECombatEvaluationExtraCheck.WinWorseSkill] = WinWorseSkillChecker,
		[ECombatEvaluationExtraCheck.WinLessConsummate] = WinLessConsummateChecker,
		[ECombatEvaluationExtraCheck.WinPregnant] = WinPregnantChecker,
		[ECombatEvaluationExtraCheck.WinMore] = WinMoreChecker,
		[ECombatEvaluationExtraCheck.WinOlder] = WinOlderChecker,
		[ECombatEvaluationExtraCheck.WinBetterEquip] = WinBetterEquipChecker,
		[ECombatEvaluationExtraCheck.WinMoreNeili] = WinMoreNeiliChecker,
		[ECombatEvaluationExtraCheck.WinBetterSkill] = WinBetterSkillChecker,
		[ECombatEvaluationExtraCheck.WinMoreConsummate] = WinMoreConsummateChecker,
		[ECombatEvaluationExtraCheck.WinInPregnant] = WinInPregnantChecker,
		[ECombatEvaluationExtraCheck.KillBad0] = KillBad0Checker,
		[ECombatEvaluationExtraCheck.KillBad1] = KillBad1Checker,
		[ECombatEvaluationExtraCheck.KillGood0] = KillGood0Checker,
		[ECombatEvaluationExtraCheck.KillGood1] = KillGood1Checker,
		[ECombatEvaluationExtraCheck.KillMinion0] = KillMinion0Checker,
		[ECombatEvaluationExtraCheck.KillMinion1] = KillMinion1Checker,
		[ECombatEvaluationExtraCheck.ShixiangBuff0] = ShixiangBuff0Checker,
		[ECombatEvaluationExtraCheck.ShixiangBuff1] = ShixiangBuff1Checker,
		[ECombatEvaluationExtraCheck.ShixiangBuff2] = ShixiangBuff2Checker,
		[ECombatEvaluationExtraCheck.PuppetCombat] = PuppetCombatChecker,
		[ECombatEvaluationExtraCheck.OutBossCombat] = OutBossCombatChecker,
		[ECombatEvaluationExtraCheck.WinLoong] = WinLoongChecker,
		[ECombatEvaluationExtraCheck.CombatHard] = CombatHardChecker,
		[ECombatEvaluationExtraCheck.CombatVeryHard] = CombatVeryHardChecker
	};

	private static readonly CValuePercentBonus WinNeiliMinDelta = CValuePercentBonus.op_Implicit(25);

	public sbyte SelfMaxSkillGrade;

	public sbyte EnemyMaxSkillGrade;

	private readonly List<int> _lootCharList = new List<int>();

	private const sbyte CombatCharAboutToFallEventSendTimes = 4;

	private static OnCombatCharAboutToFall _handlersCombatCharAboutToFall;

	[DomainData(DomainDataType.SingleValue, false, false, true, true)]
	private DamageCompareData _damageCompareData;

	[DomainData(DomainDataType.SingleValue, false, false, true, true)]
	private IntPair _skillAttackedIndexAndHit;

	public static readonly string[] InjuryAni = new string[3] { "H_003", "H_004", "H_005" };

	public static readonly string[] AvoidAni = new string[4] { "H_002", "H_001", "H_000", "H_002" };

	private static readonly List<(sbyte bodyPart, bool isInner)> CachedInjuryRandomPool = new List<(sbyte, bool)>();

	private static readonly sbyte[] AddCaptureRateEquipSlot = new sbyte[4] { 8, 9, 10, 11 };

	public const sbyte RopeEnsureHitConsummateGap = 6;

	[DomainData(DomainDataType.SingleValue, false, false, true, true)]
	private SpecialMiscData _showUseGoldenWire;

	public Dictionary<int, int> EquipmentPowerChangeInCombat = new Dictionary<int, int>();

	public Dictionary<ItemKey, int> EquipmentOldDurability = new Dictionary<ItemKey, int>();

	public const byte NormalWeaponCount = 3;

	public const byte MaxWeaponCount = 7;

	public const int WeaponIndexEmptyHand = 3;

	public const int WeaponIndexBranch = 4;

	public const int WeaponIndexStone = 5;

	public const int WeaponIndexVoice = 6;

	public const byte WeaponTrickCount = 6;

	public const byte MaxPursueAttack = 5;

	public const short NormalAttackMoveWaitFrame = 6;

	public static readonly List<string> SpecialCharBlockAni = new List<string> { "T_001_0_1", "T_001_0_2" };

	private static readonly Dictionary<sbyte, sbyte> GodTrickUseTrickType = new Dictionary<sbyte, sbyte>
	{
		[0] = 3,
		[1] = 5,
		[2] = 4,
		[3] = 9
	};

	[DomainData(DomainDataType.ObjectCollection, false, false, true, true)]
	private Dictionary<int, CombatWeaponData> _weaponDataDict;

	[DomainData(DomainDataType.SingleValue, false, false, true, true)]
	private WeaponExpectInnerRatioData _expectRatioData;

	public const string OtherActionPrepareAni = "C_007";

	public static readonly int[] OtherActionSpecialEffectId = new int[2] { 1457, 1459 };

	public static readonly short[] OtherActionPrepareFrame = new short[5] { 320, 320, 600, 120, -1 };

	public const short HealInjuryMinPrepareFrame = 120;

	public const short HealPoisonMinPrepareFrame = 120;

	private static readonly Dictionary<sbyte, MixPoisonEffectDelegate> MixPoisonEffectImplements = new Dictionary<sbyte, MixPoisonEffectDelegate>();

	public const string CommonPrepareAni = "C_007";

	public const string SkillFinalAvoidAni = "H_008";

	public const string SkillPrepareSound = "se_combat_preskill";

	[DomainData(DomainDataType.ObjectCollection, false, false, true, true)]
	private Dictionary<CombatSkillKey, CombatSkillData> _skillDataDict;

	[DomainData(DomainDataType.SingleValueCollection, false, false, true, true)]
	private Dictionary<CombatSkillKey, SkillPowerChangeCollection> _skillPowerAddInCombat;

	[DomainData(DomainDataType.SingleValueCollection, false, false, true, true)]
	private Dictionary<CombatSkillKey, SkillPowerChangeCollection> _skillPowerReduceInCombat;

	[DomainData(DomainDataType.SingleValueCollection, false, false, true, true)]
	private Dictionary<CombatSkillKey, CombatSkillKey> _skillPowerReplaceInCombat;

	private readonly Dictionary<CombatSkillKey, int> _skillCastTimes = new Dictionary<CombatSkillKey, int>();

	private const string NoResourceTypeWhooshSound = "se_combat_whoosh_empty";

	private static readonly string[] NoResourceTypeStepSound = new string[2] { "se_combat_foot_empty_1", "se_combat_foot_empty_2" };

	[DomainData(DomainDataType.SingleValue, false, false, true, true)]
	private sbyte _bgmIndex;

	private const byte TeamCapacity = 4;

	public const sbyte MaxTeammateCommandCount = 3;

	public const sbyte TeammateCommandBaseCdSpeed = 100;

	public const sbyte TeammateFightBaseBreathStancePercent = 50;

	public const sbyte TeammateFightBaseMobilityPercent = 50;

	[DomainData(DomainDataType.ObjectCollection, false, false, true, true)]
	private Dictionary<int, CombatCharacter> _combatCharacterDict;

	[DomainData(DomainDataType.SingleValue, false, false, true, true)]
	private List<int> _taiwuSpecialGroupCharIds;

	[DomainData(DomainDataType.SingleValue, false, false, true, true, ArrayElementsCount = 4)]
	private int[] _selfTeam;

	[DomainData(DomainDataType.SingleValue, false, false, true, true)]
	private int _selfCharId;

	private CombatCharacter _selfChar;

	[DomainData(DomainDataType.SingleValue, false, false, true, true)]
	private sbyte _selfTeamWisdomType;

	[DomainData(DomainDataType.SingleValue, false, false, true, true)]
	private short _selfTeamWisdomCount;

	[DomainData(DomainDataType.SingleValue, false, false, true, true, ArrayElementsCount = 4)]
	private int[] _enemyTeam;

	[DomainData(DomainDataType.SingleValue, false, false, true, true)]
	private int _enemyCharId;

	private CombatCharacter _enemyChar;

	[DomainData(DomainDataType.SingleValue, false, false, true, true)]
	private sbyte _enemyTeamWisdomType;

	[DomainData(DomainDataType.SingleValue, false, false, true, true)]
	private short _enemyTeamWisdomCount;

	[DomainData(DomainDataType.SingleValue, false, false, true, true)]
	private int _carrierAnimalCombatCharId;

	[DomainData(DomainDataType.SingleValue, false, false, true, true)]
	private int _specialShowCombatCharId;

	public TeammateCommandChangeData PreRandomizedTeammateCommandReplaceData;

	private Dictionary<SectStoryThreeVitalsCharacterType, int> _vitalTeammateData;

	public static readonly Dictionary<ETeammateCommandImplement, ETeammateCommandOption> TeammateCommandOptions = new Dictionary<ETeammateCommandImplement, ETeammateCommandOption>();

	private static readonly Dictionary<ETeammateCommandImplement, ITeammateCommandChecker> TeammateCommandCheckers = new Dictionary<ETeammateCommandImplement, ITeammateCommandChecker>
	{
		{
			ETeammateCommandImplement.Fight,
			new TeammateCommandCheckerFight()
		},
		{
			ETeammateCommandImplement.AccelerateCast,
			new TeammateCommandCheckerAccelerateCast()
		},
		{
			ETeammateCommandImplement.Push,
			new TeammateCommandCheckerPush()
		},
		{
			ETeammateCommandImplement.Pull,
			new TeammateCommandCheckerPull()
		},
		{
			ETeammateCommandImplement.Attack,
			new TeammateCommandCheckerAttack()
		},
		{
			ETeammateCommandImplement.AttackSkill,
			new TeammateCommandCheckerAttackSkill()
		},
		{
			ETeammateCommandImplement.Defend,
			new TeammateCommandCheckerDefendSkill()
		},
		{
			ETeammateCommandImplement.HealInjury,
			new TeammateCommandCheckerHealInjury()
		},
		{
			ETeammateCommandImplement.HealPoison,
			new TeammateCommandCheckerHealPoison()
		},
		{
			ETeammateCommandImplement.HealFlaw,
			new TeammateCommandCheckerHealFlaw()
		},
		{
			ETeammateCommandImplement.HealAcupoint,
			new TeammateCommandCheckerHealAcupoint()
		},
		{
			ETeammateCommandImplement.AddHit,
			new TeammateCommandCheckerAddHit()
		},
		{
			ETeammateCommandImplement.AddAvoid,
			new TeammateCommandCheckerAddAvoid()
		},
		{
			ETeammateCommandImplement.StopEnemy,
			new TeammateCommandCheckerStopEnemy()
		},
		{
			ETeammateCommandImplement.TransferNeiliAllocation,
			new TeammateCommandCheckerTransferNeiliAllocation()
		},
		{
			ETeammateCommandImplement.TransferInjury,
			new TeammateCommandCheckerTransferInjury()
		},
		{
			ETeammateCommandImplement.InterruptSkill,
			new TeammateCommandCheckerInterruptSkill()
		},
		{
			ETeammateCommandImplement.PushOrPullIntoDanger,
			new TeammateCommandCheckerPushOrPullIntoDanger()
		},
		{
			ETeammateCommandImplement.AttackFlawAndAcupoint,
			new TeammateCommandCheckerAttackFlawAndAcupoint()
		},
		{
			ETeammateCommandImplement.ClearAgileAndDefense,
			new TeammateCommandCheckerClearAgileAndDefense()
		},
		{
			ETeammateCommandImplement.AddInjuryAndPoison,
			new TeammateCommandCheckerAddInjuryAndPoison()
		},
		{
			ETeammateCommandImplement.ReduceHitAndAvoid,
			new TeammateCommandCheckerReduceHitAndAvoid()
		},
		{
			ETeammateCommandImplement.InterruptOtherAction,
			new TeammateCommandCheckerInterruptOtherAction()
		},
		{
			ETeammateCommandImplement.ReduceNeiliAllocation,
			new TeammateCommandCheckerReduceNeiliAllocation()
		},
		{
			ETeammateCommandImplement.AnimalEffect,
			new TeammateCommandCheckerAnimalEffect()
		},
		{
			ETeammateCommandImplement.GearMateA,
			new TeammateCommandCheckerGearMateA()
		},
		{
			ETeammateCommandImplement.GearMateB,
			new TeammateCommandCheckerGearMateB()
		},
		{
			ETeammateCommandImplement.GearMateC,
			new TeammateCommandCheckerGearMateC()
		},
		{
			ETeammateCommandImplement.AddUnlockAttackValue,
			new TeammateCommandCheckerAddUnlockAttackValue()
		},
		{
			ETeammateCommandImplement.TransferManyMark,
			new TeammateCommandCheckerTransferManyMark()
		},
		{
			ETeammateCommandImplement.RepairItem,
			new TeammateCommandCheckerRepairItem()
		},
		{
			ETeammateCommandImplement.VitalDemonA,
			new TeammateCommandCheckerVitalDemonA()
		},
		{
			ETeammateCommandImplement.VitalDemonB,
			new TeammateCommandCheckerVitalDemonB()
		},
		{
			ETeammateCommandImplement.VitalDemonC,
			new TeammateCommandCheckerVitalDemonC()
		}
	};

	private static readonly string[] FailAni = new string[4] { "C_005", "C_012", "C_011", "C_005" };

	private const string WaitMercyAni = "C_011_stun";

	private const string FallAni = "C_005";

	[DomainData(DomainDataType.SingleValue, false, false, true, true)]
	private sbyte _combatStatus;

	[DomainData(DomainDataType.SingleValue, false, false, true, true)]
	private bool _waitingDelaySettlement;

	[DomainData(DomainDataType.SingleValue, false, false, true, true)]
	private sbyte _showMercyOption;

	[DomainData(DomainDataType.SingleValue, false, false, true, true)]
	private sbyte _selectedMercyOption;

	private readonly HashSet<int> _needCheckFallenCharSet = new HashSet<int>();

	private bool _skipCombatLoop;

	private static readonly DataInfluence[][] CacheInfluences = new DataInfluence[33][];

	private SingleValueCollectionModificationCollection<CombatSkillKey> _modificationsSkillPowerAddInCombat = SingleValueCollectionModificationCollection<CombatSkillKey>.Create();

	private SingleValueCollectionModificationCollection<CombatSkillKey> _modificationsSkillPowerReduceInCombat = SingleValueCollectionModificationCollection<CombatSkillKey>.Create();

	private SingleValueCollectionModificationCollection<CombatSkillKey> _modificationsSkillPowerReplaceInCombat = SingleValueCollectionModificationCollection<CombatSkillKey>.Create();

	private static readonly DataInfluence[][] CacheInfluencesCombatCharacterDict = new DataInfluence[145][];

	private readonly ObjectCollectionDataStates _dataStatesCombatCharacterDict = new ObjectCollectionDataStates(145, 0);

	public readonly ObjectCollectionHelperData HelperDataCombatCharacterDict;

	private static readonly DataInfluence[][] CacheInfluencesSkillDataDict = new DataInfluence[10][];

	private readonly ObjectCollectionDataStates _dataStatesSkillDataDict = new ObjectCollectionDataStates(10, 0);

	public readonly ObjectCollectionHelperData HelperDataSkillDataDict;

	private static readonly DataInfluence[][] CacheInfluencesWeaponDataDict = new DataInfluence[10][];

	private readonly ObjectCollectionDataStates _dataStatesWeaponDataDict = new ObjectCollectionDataStates(10, 0);

	public readonly ObjectCollectionHelperData HelperDataWeaponDataDict;

	private Queue<uint> _pendingLoadingOperationIds;

	private bool EnemyUnyieldingFallen => GetIsPlaygroundCombat() && DomainManager.Extra.GetEnemyUnyieldingFallen();

	private bool EnemyEnableAi => _enableEnemyAi && (!GetIsPlaygroundCombat() || !DomainManager.Extra.GetDisableEnemyAi());

	public bool Started { get; private set; }

	public bool Pause { get; private set; }

	public bool IsAiMoving => _autoCombat && AiOptions.AutoMove;

	public float SelfAvgEquipGrade { get; private set; }

	public float EnemyAvgEquipGrade { get; private set; }

	public byte FleeNeedDistance => CombatConfig.FleeDistance;

	public byte InterruptFleeNeedDistance => CombatConfig.FleeInterruptDistance;

	private void OnInitializedDomainData()
	{
		_handlersCombatCharAboutToFall = null;
	}

	private void InitializeOnInitializeGameDataModule()
	{
		for (sbyte b = 0; b < Boss.Instance.Count; b++)
		{
			short[] characterIdList = Boss.Instance[b].CharacterIdList;
			short[] array = characterIdList;
			foreach (short key in array)
			{
				CharId2BossId[key] = b;
			}
		}
		foreach (TeammateCommandItem item in (IEnumerable<TeammateCommandItem>)TeammateCommand.Instance)
		{
			if (TeammateCommandOptions.TryGetValue(item.Implement, out var value) && value != item.Option)
			{
				PredefinedLog.Show(8, $"cmd implement mapping to multi option {value} {item.Option}");
			}
			else
			{
				TeammateCommandOptions[item.Implement] = item.Option;
			}
		}
		SharedConstValue.InitializeCharId2AnimalIdCache();
		BindMixPoisonEffectImplements();
		AiNodeFactory.Register(GetType().Assembly);
		AiActionFactory.Register(GetType().Assembly);
		AiConditionFactory.Register(GetType().Assembly);
	}

	private void InitializeOnEnterNewWorld()
	{
	}

	private void OnLoadedArchiveData()
	{
	}

	[DomainMethod]
	public int SimulatePrepareCombat(DataContext context, short combatConfigId, int[] enemyTeam)
	{
		CombatConfigItem combatConfigItem = Config.CombatConfig.Instance[combatConfigId];
		if (combatConfigItem == null)
		{
			return -1;
		}
		ProcessCombatTeam(context, combatConfigId, enemyTeam, out var selfTeam);
		return (int)CFormulaHelper.RandomPrepareResult(selfTeam, enemyTeam);
	}

	private void ProcessCombatTeam(DataContext context, short combatConfigId, int[] enemyTeam, out int[] selfTeam)
	{
		CombatConfigItem combatConfigItem = Config.CombatConfig.Instance[combatConfigId];
		selfTeam = new int[4];
		selfTeam[0] = DomainManager.Taiwu.GetTaiwuCharId();
		if (combatConfigItem.AllowGroupMember)
		{
			int num = 1;
			for (int i = 0; i < 3; i++)
			{
				int element_CombatGroupCharIds = DomainManager.Taiwu.GetElement_CombatGroupCharIds(i);
				if (element_CombatGroupCharIds >= 0 && !enemyTeam.Exist(element_CombatGroupCharIds))
				{
					selfTeam[num++] = element_CombatGroupCharIds;
				}
			}
			if (combatConfigItem.AllowVitalDemon)
			{
				Dictionary<SectStoryThreeVitalsCharacterType, int> vitalTeammateData = _vitalTeammateData;
				if (vitalTeammateData != null && vitalTeammateData.Count > 0)
				{
					foreach (var (type, num3) in _vitalTeammateData)
					{
						selfTeam[(num3 < num) ? num3 : num++] = DomainManager.Extra.GetVitalCharacterByType(context, type).GetId();
					}
				}
			}
			for (int j = num; j < selfTeam.Length; j++)
			{
				selfTeam[j] = -1;
			}
			return;
		}
		for (int k = 1; k < 4; k++)
		{
			selfTeam[k] = -1;
			if (enemyTeam.Length > k)
			{
				enemyTeam[k] = -1;
			}
		}
	}

	private static void ProcessTemporaryFavorability(DataContext context, IReadOnlyList<int> enemyTeam)
	{
		int charId = enemyTeam[0];
		for (int i = 1; i < enemyTeam.Count; i++)
		{
			int num = enemyTeam[i];
			if (num < 0)
			{
				continue;
			}
			GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(num);
			if (element_Objects.GetCreatingType() != 1)
			{
				short favorability = DomainManager.Character.GetFavorability(charId, num);
				if (favorability == short.MinValue)
				{
					short randomFavorability = element_Objects.GetRandomFavorability(context.Random);
					DomainManager.Character.DirectlySetFavorabilities(context, charId, num, randomFavorability, randomFavorability);
				}
			}
		}
	}

	[DomainMethod]
	public TeammateCommandChangeData ProcessCombatTeammateCommands(DataContext context, short combatConfigId, int[] enemyTeam)
	{
		ProcessCombatTeam(context, combatConfigId, enemyTeam, out var selfTeam);
		CombatConfigItem combatConfigItem = Config.CombatConfig.Instance[combatConfigId];
		if (combatConfigItem.AllowRandomFavorability)
		{
			ProcessTemporaryFavorability(context, enemyTeam);
		}
		PreRandomizedTeammateCommandReplaceData = new TeammateCommandChangeData
		{
			SelfTeam = CalcTeammateBetrayData(context, combatConfigId, selfTeam, isAlly: true),
			EnemyTeam = CalcTeammateBetrayData(context, combatConfigId, enemyTeam, isAlly: false)
		};
		if (combatConfigItem.AllowVitalDemonBetray)
		{
			ProcessVitalDemonBetray(context, PreRandomizedTeammateCommandReplaceData.EnemyTeam);
		}
		return PreRandomizedTeammateCommandReplaceData;
	}

	[DomainMethod]
	public void PrepareEnemyEquipments(DataContext context, short combatConfigId, List<int> enemyList)
	{
		GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(enemyList[0]);
		short templateId = element_Objects.GetTemplateId();
		bool flag = CharId2BossId.ContainsKey(templateId);
		bool flag2 = SharedConstValue.CharId2AnimalId.ContainsKey(templateId);
		if (!(flag || flag2) && !DomainManager.Taiwu.GetGroupCharIds().Contains(element_Objects.GetId()))
		{
			for (int i = 0; i < enemyList.Count; i++)
			{
				GameData.Domains.Character.Character element_Objects2 = DomainManager.Character.GetElement_Objects(enemyList[i]);
				context.Equipping.SelectEquipmentsByCombatConfig(context, element_Objects2, combatConfigId, isOutOfTaiwuGroup: true);
			}
		}
	}

	[DomainMethod]
	public CharacterDisplayData ApplyVitalOnTeammate(DataContext context, int typeInt, int index)
	{
		if ((index <= 0 || index >= 4) ? true : false)
		{
			return null;
		}
		if (PreRandomizedTeammateCommandReplaceData == null)
		{
			return null;
		}
		bool vitalIsDemon = DomainManager.Extra.AreVitalsDemon();
		SectStoryThreeVitalsCharacter vitalByType = DomainManager.Extra.GetVitalByType((SectStoryThreeVitalsCharacterType)typeInt);
		if (vitalByType == null || !vitalByType.AllowAsTeammate(vitalIsDemon))
		{
			return null;
		}
		Dictionary<SectStoryThreeVitalsCharacterType, int> vitalTeammateData = _vitalTeammateData;
		if (vitalTeammateData != null && vitalTeammateData.ContainsValue(index))
		{
			return null;
		}
		if (_vitalTeammateData == null)
		{
			_vitalTeammateData = new Dictionary<SectStoryThreeVitalsCharacterType, int>();
		}
		_vitalTeammateData[(SectStoryThreeVitalsCharacterType)typeInt] = index;
		GameData.Domains.Character.Character vitalCharacterByType = DomainManager.Extra.GetVitalCharacterByType(context, (SectStoryThreeVitalsCharacterType)typeInt);
		JoinSpecialGroup(context, vitalCharacterByType.GetId());
		return DomainManager.Character.GetCharacterDisplayData(vitalCharacterByType.GetId());
	}

	[DomainMethod]
	public int RevertVitalOnTeammate(DataContext context, int typeInt)
	{
		if (_vitalTeammateData == null || PreRandomizedTeammateCommandReplaceData == null)
		{
			return -1;
		}
		if (!_vitalTeammateData.Remove((SectStoryThreeVitalsCharacterType)typeInt, out var value))
		{
			return -1;
		}
		GameData.Domains.Character.Character vitalCharacterByType = DomainManager.Extra.GetVitalCharacterByType(context, (SectStoryThreeVitalsCharacterType)typeInt);
		ExitSpecialGroup(context, vitalCharacterByType.GetId());
		foreach (SectStoryThreeVitalsCharacterType key in _vitalTeammateData.Keys)
		{
			if (_vitalTeammateData[key] > value)
			{
				_vitalTeammateData[key]--;
			}
		}
		return value;
	}

	[DomainMethod]
	public sbyte PrepareCombat(DataContext context, short combatConfigId, int[] enemyTeam)
	{
		ProcessCombatTeam(context, combatConfigId, enemyTeam, out var selfTeam);
		return PrepareCombat(context, combatConfigId, selfTeam, enemyTeam);
	}

	public sbyte PrepareCombat(DataContext context, short combatConfigId, int[] selfTeam, int[] enemyTeam)
	{
		CombatConfig = Config.CombatConfig.Instance[combatConfigId];
		SetCombatStatus(CombatStatusType.NotInCombat, context);
		SetWaitingDelaySettlement(value: false, context);
		SetTimeScale(0f, context);
		SetAutoCombat(value: false, context);
		SetCombatFrame(0uL, context);
		SetCombatType(CombatConfig.CombatType, context);
		SetShowMercyOption(context, EShowMercyOption.Invalid);
		SetSelectedMercyOption(context, EShowMercySelect.Unselected);
		SetSkillAttackedIndexAndHit(new IntPair(-1, 0), context);
		if (CombatConfig.InitDistance < CombatConfig.MinDistance || CombatConfig.InitDistance > CombatConfig.MaxDistance)
		{
			(byte, byte) distanceRange = GetDistanceRange();
			short num = (short)((distanceRange.Item1 + distanceRange.Item2) / 2);
			List<short> list = ObjectPool<List<short>>.Instance.Get();
			list.Clear();
			for (int i = -2; i <= 2; i++)
			{
				short num2 = (short)(num + 10 * i);
				if (num2 < distanceRange.Item1)
				{
					num2 = distanceRange.Item1;
				}
				else if (num2 > distanceRange.Item2)
				{
					num2 = distanceRange.Item2;
				}
				if (!list.Contains(num2))
				{
					list.Add(num2);
				}
			}
			SetCurrentDistance(list.GetRandom(context.Random), context);
			ObjectPool<List<short>>.Instance.Return(list);
		}
		else
		{
			SetCurrentDistance(CombatConfig.InitDistance, context);
		}
		_frameTimeAccumulator = 0f;
		_bgmIndex = 0;
		Started = false;
		_inBulletTime = false;
		_showUseGoldenWire = 0;
		Context = context;
		_isTutorialCombat = DomainManager.Character.GetElement_Objects(selfTeam[0]).GetTemplateId() == 444;
		ClearCombatCharacterDict();
		for (int j = 0; j < 4; j++)
		{
			_selfTeam[j] = ((j < selfTeam.Length && selfTeam[j] >= 0) ? selfTeam[j] : (-1));
			_enemyTeam[j] = ((j < enemyTeam.Length && enemyTeam[j] >= 0) ? enemyTeam[j] : (-1));
		}
		int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
		for (int k = 0; k < 4; k++)
		{
			int num3 = _selfTeam[k];
			int num4 = _enemyTeam[k];
			if (num3 >= 0)
			{
				CombatCharacter combatCharacter = new CombatCharacter();
				combatCharacter.IsAlly = true;
				combatCharacter.IsTaiwu = taiwuCharId == num3;
				AddElement_CombatCharacterDict(num3, combatCharacter);
				combatCharacter.Init(this, num3, context);
			}
			if (num4 >= 0)
			{
				CombatCharacter combatCharacter2 = new CombatCharacter();
				combatCharacter2.IsAlly = false;
				combatCharacter2.IsTaiwu = taiwuCharId == num4;
				AddElement_CombatCharacterDict(num4, combatCharacter2);
				combatCharacter2.Init(this, num4, context);
			}
		}
		InitSkillData(context);
		InitWeaponData(context);
		_selfChar = (_enemyChar = null);
		SetCombatCharacter(context, isAlly: true, _selfTeam[0]);
		SetCombatCharacter(context, isAlly: false, _enemyTeam[0]);
		InitEquipmentDurability();
		for (sbyte b = 0; b < 14; b++)
		{
			int owner = DomainManager.LegendaryBook.GetOwner(b);
			if (owner >= 0 && owner != _selfTeam[0] && _combatCharacterDict.ContainsKey(owner))
			{
				AddCombatState(context, _combatCharacterDict[owner], 0, (short)(117 + b), 100, reverse: false, applyEffect: true, owner);
			}
		}
		foreach (CombatCharacter value in _combatCharacterDict.Values)
		{
			value.AiController = new AiController(value);
			value.AiController.Init();
		}
		PrepareCombatSpecial(context);
		PrepareCombatProfession(context);
		PrepareCombatSectStory(context, combatConfigId);
		int teamWisdomCount = GetTeamWisdomCount(isAlly: true);
		int teamWisdomCount2 = GetTeamWisdomCount(isAlly: false);
		SetSelfTeamWisdomType((sbyte)((teamWisdomCount <= 0) ? ((teamWisdomCount < 0) ? 1 : (-1)) : 0), context);
		SetSelfTeamWisdomCount((short)Math.Abs(teamWisdomCount), context);
		SetEnemyTeamWisdomType((sbyte)((teamWisdomCount2 <= 0) ? ((teamWisdomCount2 < 0) ? 1 : (-1)) : 0), context);
		SetEnemyTeamWisdomCount((short)Math.Abs(teamWisdomCount2), context);
		EPrepareCombatResult ePrepareCombatResult = CFormulaHelper.RandomPrepareResult(_selfTeam, _enemyTeam, context.Random);
		bool flag = ePrepareCombatResult == EPrepareCombatResult.SelfFirst;
		_selfChar.SetBreathValue(30000 * (flag ? 30 : 0) / 100, context);
		_selfChar.SetStanceValue(4000 * (flag ? 30 : 0) / 100, context);
		_enemyChar.SetBreathValue(30000 * ((!flag) ? 30 : 0) / 100, context);
		_enemyChar.SetStanceValue(4000 * ((!flag) ? 30 : 0) / 100, context);
		CombatCharacter character = (flag ? _enemyChar : _selfChar);
		ChangeMobilityValue(context, character, -MoveSpecialConstants.MaxMobility * 50 / 100);
		foreach (CombatCharacter value2 in _combatCharacterDict.Values)
		{
			if (!IsMainCharacter(value2))
			{
				value2.InitTeammateCommand(context, value2.IsAlly == flag);
			}
		}
		UpdateAllCommandAvailability(context, _selfChar);
		UpdateAllCommandAvailability(context, _enemyChar);
		_combatResultData.Reset();
		_skillCastTimes.Clear();
		_lootCharList.Clear();
		SelfMaxSkillGrade = -1;
		EnemyMaxSkillGrade = -1;
		SelfAvgEquipGrade = 0f;
		EnemyAvgEquipGrade = 0f;
		int num5 = 0;
		int num6 = 0;
		foreach (CombatCharacter value3 in _combatCharacterDict.Values)
		{
			ItemKey[] equipment = value3.GetCharacter().GetEquipment();
			for (sbyte b2 = 0; b2 < 12; b2++)
			{
				if (b2 != 4 && b2 != 11)
				{
					ItemKey itemKey = equipment[b2];
					if (itemKey.IsValid())
					{
						ItemBase baseItem = DomainManager.Item.GetBaseItem(itemKey);
						if (baseItem.GetMaxDurability() < 0 || baseItem.GetCurrDurability() > 0)
						{
							sbyte grade = ItemTemplateHelper.GetGrade(itemKey.ItemType, itemKey.TemplateId);
							if (value3.IsAlly)
							{
								SelfAvgEquipGrade += grade + 1;
								num5++;
							}
							else
							{
								EnemyAvgEquipGrade += grade + 1;
								num6++;
							}
						}
					}
				}
			}
		}
		SelfAvgEquipGrade /= Math.Max(1, num5);
		EnemyAvgEquipGrade /= Math.Max(1, num6);
		if (_isTutorialCombat)
		{
			_selfChar.SetBreathValue(30000, context);
			_selfChar.SetStanceValue(4000, context);
		}
		if (combatConfigId == 125)
		{
			ClearMobilityAndForbidRecover(context, _enemyCharId);
		}
		SetCombatStatus(CombatStatusType.InCombat, context);
		foreach (CombatCharacter value4 in _combatCharacterDict.Values)
		{
			UpdateBodyDefeatMark(context, value4);
			UpdatePoisonDefeatMark(context, value4);
			UpdateOtherDefeatMark(context, value4);
		}
		UpdateAllTeammateCommandUsable(context, isAlly: true, -1);
		UpdateAllTeammateCommandUsable(context, isAlly: false, -1);
		PreRandomizedTeammateCommandReplaceData = null;
		_vitalTeammateData = null;
		_expectRatioData.Clear();
		SetExpectRatioData(_expectRatioData, context);
		return (sbyte)ePrepareCombatResult;
	}

	private void PrepareCombatSpecial(DataContext context)
	{
		foreach (CombatCharacter value2 in _combatCharacterDict.Values)
		{
			ItemKey carrier = value2.GetCharacter().GetEquipment()[11];
			if (DomainManager.Extra.IsCarrierFullTamePoint(carrier) && DomainManager.Item.TryGetElement_Carriers(carrier.Id, out var element) && element.GetCurrDurability() > 0)
			{
				short templateId = carrier.TemplateId;
				if (SharedConstValue.AnimalCarrier2Effect.TryGetValue(templateId, out var value))
				{
					DomainManager.SpecialEffect.Add(context, value2.GetId(), value);
				}
			}
		}
	}

	private void PrepareCombatProfession(DataContext context)
	{
		if (DomainManager.Extra.IsProfessionalSkillUnlockedAndEquipped(3) && _enemyChar.IsAnimal)
		{
			DomainManager.SpecialEffect.Add(context, _selfChar.GetId(), "Profession.Hunter.HuntingBeasts");
		}
		ItemKey itemKey = _selfChar.GetCharacter().GetEquipment()[11];
		short templateId = itemKey.TemplateId;
		bool flag = itemKey.IsValid() && !DomainManager.Item.GetBaseItem(itemKey).IsDurabilityRunningOut();
		if (DomainManager.Extra.IsProfessionalSkillUnlockedAndEquipped(4) && templateId >= 0 && Config.Carrier.Instance[templateId].CharacterIdInCombat >= 0 && flag)
		{
			GameData.Domains.Character.Character character = DomainManager.Character.CreateFixedEnemy(context, Config.Carrier.Instance[templateId].CharacterIdInCombat);
			DomainManager.Character.CompleteCreatingCharacter(character.GetId());
			CombatCharacter combatCharacter = new CombatCharacter();
			combatCharacter.IsAlly = true;
			combatCharacter.IsTaiwu = false;
			AddElement_CombatCharacterDict(character.GetId(), combatCharacter);
			combatCharacter.Init(this, character.GetId(), context);
			combatCharacter.Immortal = true;
			combatCharacter.SetVisible(visible: false, context);
			combatCharacter.SetCanAttackOutRange(canAttackOutRange: true, context);
			combatCharacter.SetUsingWeaponIndex(0, context);
			combatCharacter.SetAnimationToLoop(GetIdleAni(combatCharacter), context);
			for (int i = 0; i < 3; i++)
			{
				ItemKey itemKey2 = combatCharacter.GetWeapons()[i];
				if (itemKey2.IsValid())
				{
					List<sbyte> weaponTricks = DomainManager.Item.GetWeaponTricks(itemKey2);
					CombatWeaponData combatWeaponData = new CombatWeaponData(itemKey2, combatCharacter);
					sbyte[] weaponTricks2 = combatWeaponData.GetWeaponTricks();
					AddElement_WeaponDataDict(itemKey2.Id, combatWeaponData);
					combatWeaponData.Init(context, i);
					for (int j = 0; j < weaponTricks.Count; j++)
					{
						weaponTricks2[j] = weaponTricks[j];
					}
				}
			}
			SetCarrierAnimalCombatCharId(combatCharacter.GetId(), context);
		}
		else
		{
			SetCarrierAnimalCombatCharId(-1, context);
		}
		Location location = _selfChar.GetCharacter().GetLocation();
		if (!DomainManager.Extra.IsProfessionalSkillUnlockedAndEquipped(2) || !location.IsValid())
		{
			return;
		}
		List<MapBlockData> list = ObjectPool<List<MapBlockData>>.Instance.Get();
		int maxSteps = 1;
		DomainManager.Map.GetRealNeighborBlocks(location.AreaId, location.BlockId, list, maxSteps, includeCenter: true);
		HashSet<string> hashSet = ObjectPool<HashSet<string>>.Instance.Get();
		hashSet.Clear();
		foreach (MapBlockData item in list)
		{
			if (SharedConstValue.MapBlockSubType2SavageEffect.TryGetValue(item.BlockSubType, out var value))
			{
				hashSet.Add(value);
			}
		}
		foreach (string item2 in hashSet)
		{
			DomainManager.SpecialEffect.Add(context, _selfChar.GetId(), item2);
		}
		ObjectPool<List<MapBlockData>>.Instance.Return(list);
		ObjectPool<HashSet<string>>.Instance.Return(hashSet);
	}

	private void PrepareCombatSectStory(DataContext context, short combatConfigId)
	{
		if (combatConfigId >= 164 && combatConfigId <= 166)
		{
			GameData.Domains.Character.Character character = DomainManager.Character.CreateFixedCharacter(context, 515);
			DomainManager.Character.CompleteCreatingCharacter(character.GetId());
			CombatCharacter combatCharacter = new CombatCharacter();
			combatCharacter.IsAlly = true;
			combatCharacter.IsTaiwu = false;
			AddElement_CombatCharacterDict(character.GetId(), combatCharacter);
			combatCharacter.Init(this, character.GetId(), context);
			combatCharacter.SetVisible(visible: false, context);
			SetSpecialShowCombatCharId(combatCharacter.GetId(), context);
			_selfChar.NeedEnterSpecialShow = true;
		}
		else
		{
			SetSpecialShowCombatCharId(-1, context);
		}
		short templateId = _enemyChar.GetCharacter().GetTemplateId();
		if (514 <= templateId && templateId <= 518)
		{
			AddCombatState(context, _enemyChar, 0, 145);
		}
		if (combatConfigId == 198)
		{
			int charId = _selfTeam[0];
			XiongZhongSiQi effect = new XiongZhongSiQi(charId);
			DomainManager.SpecialEffect.Add(context, effect);
		}
		if (combatConfigId == 203)
		{
			int charId2 = _selfTeam[0];
			SiQiDuoHun effect2 = new SiQiDuoHun(charId2);
			DomainManager.SpecialEffect.Add(context, effect2);
		}
		if (combatConfigId == 204)
		{
			EventArgBox sectMainStoryEventArgBox = DomainManager.Extra.GetSectMainStoryEventArgBox(3);
			sectMainStoryEventArgBox.Get<SByteList>("ConchShip_PresetKey_BaihuaAdventureFinialWinSect", out SByteList arg);
			List<sbyte> items = arg.Items;
			if (items != null && items.Count > 0)
			{
				foreach (sbyte item in arg.Items)
				{
					PrepareCombatSectStoryTryCreateBaihuaLegacyPower(context, item);
				}
			}
		}
		if (combatConfigId == 205)
		{
			int charId3 = _selfTeam[0];
			SoulWitheringBell effect3 = new SoulWitheringBell(charId3);
			DomainManager.SpecialEffect.Add(context, effect3);
		}
		SectShaolinDemonSlayerData sectShaolinDemonSlayerData = DomainManager.Extra.GetSectShaolinDemonSlayerData();
		List<SpecialEffectBase> trialingRestrictEffects = sectShaolinDemonSlayerData.TrialingRestrictEffects;
		if (trialingRestrictEffects != null && trialingRestrictEffects.Count > 0)
		{
			foreach (SpecialEffectBase trialingRestrictEffect in sectShaolinDemonSlayerData.TrialingRestrictEffects)
			{
				DomainManager.SpecialEffect.Add(context, trialingRestrictEffect);
			}
		}
		sectShaolinDemonSlayerData.TrialingRestrictEffects = null;
	}

	private void PrepareCombatSectStoryTryCreateBaihuaLegacyPower(DataContext context, sbyte orgTemplateId)
	{
		int charId = _selfTeam[0];
		if (1 == 0)
		{
		}
		SpecialEffectBase specialEffectBase = orgTemplateId switch
		{
			1 => new LegacyPowerShaolin(charId), 
			2 => new LegacyPowerEmei(charId), 
			3 => new LegacyPowerBaihua(charId), 
			4 => new LegacyPowerWudang(charId), 
			5 => new LegacyPowerYuanshan(charId), 
			6 => new LegacyPowerShixiang(charId), 
			7 => new LegacyPowerRanshan(charId), 
			8 => new LegacyPowerXuannv(charId), 
			9 => new LegacyPowerZhujian(charId), 
			10 => new LegacyPowerKongsang(charId), 
			11 => new LegacyPowerJingang(charId), 
			12 => new LegacyPowerWuxian(charId), 
			13 => new LegacyPowerJieqing(charId), 
			14 => new LegacyPowerFulong(charId), 
			15 => new LegacyPowerXuehou(charId), 
			_ => null, 
		};
		if (1 == 0)
		{
		}
		SpecialEffectBase specialEffectBase2 = specialEffectBase;
		if (specialEffectBase2 != null)
		{
			DomainManager.SpecialEffect.Add(context, specialEffectBase2);
		}
	}

	[DomainMethod]
	public bool StartCombat(DataContext context)
	{
		SetTimeScale(_timeScale, context);
		if (CombatConfig.StartInSecondPhase && _enemyChar.BossConfig != null)
		{
			RaiseCombatCharAboutToFall(context, _enemyChar, 3);
		}
		_needCheckFallenCharSet.Clear();
		AddToCheckFallenSet(_selfChar.GetId());
		AddToCheckFallenSet(_enemyChar.GetId());
		Events.RaiseCombatBegin(context);
		Events.RaiseChangeNeiliAllocationAfterCombatBegin(context, _selfChar, _selfChar.GetNeiliAllocation());
		Events.RaiseChangeNeiliAllocationAfterCombatBegin(context, _enemyChar, _enemyChar.GetNeiliAllocation());
		Events.RaiseCreateGangqiAfterChangeNeiliAllocation(context, _selfChar);
		Events.RaiseCreateGangqiAfterChangeNeiliAllocation(context, _enemyChar);
		DomainManager.TaiwuEvent.OnEvent_CombatOpening(_enemyCharId);
		EnsurePauseState();
		TestSkillCounter = 0;
		Started = true;
		if (AiOptions.SaveMoveTarget && DomainManager.Extra.GetLastTargetDistance() > 0)
		{
			SetTargetDistance(context, DomainManager.Extra.GetLastTargetDistance());
		}
		return true;
	}

	[DomainMethod]
	public void SetTimeScale(DataContext context, float timeScale)
	{
		if (IsInCombat() && !CombatAboutToOver())
		{
			if (_inBulletTime)
			{
				_timeScaleSaveInBulletTime = timeScale;
				SetTimeScale((timeScale == 0f) ? 0f : 0.2f, context);
			}
			else
			{
				SetTimeScale(timeScale, context);
			}
		}
	}

	[DomainMethod]
	public void SetPlayerAutoCombat(DataContext context, bool autoCombat)
	{
		SetAutoCombat(autoCombat, context);
		SetMoveState(0);
	}

	[DomainMethod]
	public void EnterBossPuppetCombat(DataContext context, short puppetCharTemplateId, sbyte consummateLevel, bool playground = false)
	{
		int num = consummateLevel / 2 - 1;
		if (BossPuppet2BossId.ContainsKey(puppetCharTemplateId) && num >= 0 && num <= 8)
		{
			short num2 = (short)(BossPuppet2BossId[puppetCharTemplateId] + num);
			if (DomainManager.Character.TryGetFixedCharacterByTemplateId(num2, out var character))
			{
				DomainManager.Character.RemoveNonIntelligentCharacter(context, character);
			}
			GameData.Domains.Character.Character character2 = DomainManager.Character.CreateFixedCharacter(context, num2);
			character2.OfflineSetXiangshuType(4);
			DomainManager.LegendaryBook.UpdateBossCharacterLegendaryBookFeatures(context, character2);
			DomainManager.Character.CompleteCreatingCharacter(character2.GetId());
			CombatEntry(context, new List<int> { character2.GetId() }, Boss.Instance[CharId2BossId[num2]].CombatConfig);
			SetIsPuppetCombat(value: true, context);
			SetIsPlaygroundCombat(playground, context);
		}
	}

	[DomainMethod]
	public void EnableBulletTime(DataContext context, bool enable)
	{
		if (_inBulletTime == enable)
		{
			return;
		}
		_inBulletTime = enable;
		if (enable)
		{
			_timeScaleSaveInBulletTime = _timeScale;
			if (_timeScale > 0f)
			{
				SetTimeScale(0.2f, context);
			}
		}
		else
		{
			SetTimeScale(_timeScaleSaveInBulletTime, context);
		}
	}

	public override void OnUpdate(DataContext context)
	{
		if (_timeScale <= 0f || !IsInCombat() || !Started)
		{
			return;
		}
		Context = context;
		_selfChar.OnFrameBegin();
		if (_selfChar.TeammateBeforeMainChar >= 0)
		{
			_combatCharacterDict[_selfChar.TeammateBeforeMainChar].OnFrameBegin();
		}
		_enemyChar.OnFrameBegin();
		if (_enemyChar.TeammateBeforeMainChar >= 0)
		{
			_combatCharacterDict[_enemyChar.TeammateBeforeMainChar].OnFrameBegin();
		}
		_frameTimeAccumulator += _timeScale;
		if (_frameTimeAccumulator >= 1f)
		{
			int num = (int)_frameTimeAccumulator;
			_frameTimeAccumulator %= 1f;
			for (int i = 0; i < num; i++)
			{
				if (IsInCombat())
				{
					CombatLoop(context);
				}
			}
		}
		_selfChar.OnFrameEnd();
		_enemyChar.OnFrameEnd();
	}

	private void CombatLoop(DataContext context)
	{
		if (_skipCombatLoop)
		{
			_skipCombatLoop = false;
			return;
		}
		_saveDyingEffectTriggerd = false;
		if (CheckFallen(context))
		{
			return;
		}
		if (Pause == _selfChar.StateMachine.GetCurrentState().IsUpdateOnPause)
		{
			if (!Pause)
			{
				if (_autoCombat && !_isTutorialCombat)
				{
					_selfChar.AiController.Update(context);
				}
				else
				{
					_selfChar.AiController.UpdateOnlyMove(context);
				}
			}
			_selfChar.StateMachine.OnUpdate();
		}
		if (IsInCombat() && Pause == _enemyChar.StateMachine.GetCurrentState().IsUpdateOnPause)
		{
			if (!Pause && EnemyEnableAi && (!_isTutorialCombat || DomainManager.TutorialChapter.IsInTutorialChapter(7)))
			{
				_enemyChar.AiController.Update(context);
			}
			_enemyChar.StateMachine.OnUpdate();
		}
		if (!Pause)
		{
			SetCombatFrame(_combatFrame + 1, context);
			if (CombatConfig.SelfForceDefeatFrame != 0 && _combatFrame >= CombatConfig.SelfForceDefeatFrame)
			{
				ForceDefeat(GetMainCharacter(isAlly: true).GetId());
			}
			if (CombatConfig.EnemyForceDefeatFrame != 0 && _combatFrame >= CombatConfig.EnemyForceDefeatFrame)
			{
				ForceDefeat(GetMainCharacter(isAlly: false).GetId());
			}
		}
	}

	private bool CheckFallen(DataContext context)
	{
		if (_selfChar.StateMachine.GetCurrentState().RequireDelayFallen || _enemyChar.StateMachine.GetCurrentState().RequireDelayFallen)
		{
			return false;
		}
		return CheckFallenImmediate(context);
	}

	public bool CheckFallenImmediate(DataContext context)
	{
		for (int i = 0; i < _selfTeam.Length; i++)
		{
			int num = _selfTeam[i];
			if (num >= 0 && _needCheckFallenCharSet.Contains(num) && CheckCurrCharDangerOrFallen(context, _combatCharacterDict[num]))
			{
				break;
			}
		}
		if (IsInCombat() && !CombatAboutToOver())
		{
			for (int j = 0; j < _enemyTeam.Length; j++)
			{
				int num2 = _enemyTeam[j];
				if (num2 >= 0 && _needCheckFallenCharSet.Contains(num2) && CheckCurrCharDangerOrFallen(context, _combatCharacterDict[num2]))
				{
					break;
				}
			}
		}
		_needCheckFallenCharSet.Clear();
		return !IsInCombat();
	}

	public void EnsurePauseState()
	{
		bool pause = Pause;
		Pause = _selfChar.StateMachine.GetCurrentState().IsUpdateOnPause || _enemyChar.StateMachine.GetCurrentState().IsUpdateOnPause;
		if (pause != Pause)
		{
			UpdateAllTeammateCommandUsable(Context, isAlly: true, -1);
			UpdateAllTeammateCommandUsable(Context, isAlly: false, -1);
		}
	}

	[DomainMethod]
	[Obsolete("This method is obsolete, and will be removed in future.")]
	public void RemoveTeammateCommand(DataContext context, int charId, int index)
	{
	}

	[Obsolete("This method is obsolete, use AddCombatResultLegacy or ClearCombatResultLegacies instead.")]
	public void SetCombatResultLegacy(short legacy)
	{
	}

	[Obsolete("This method is obsolete, and will be removed in future.")]
	public void ExecuteTeammateCommandImplement(DataContext context, bool isAlly, ETeammateCommandImplement implement, int charId)
	{
	}

	[Obsolete("This method is obsolete, and will be removed in future.")]
	public void GetUsableTeammateCommands(List<(int, int)> teammateId2CmdIndexes, CombatCharacter combatChar, ETeammateCommandImplement targetImplement)
	{
	}

	[Obsolete("This method is obsolete, and will be removed in future.")]
	public void ChangeNpcWeaponInnerRatio(DataContext context, int charId, int weaponIndex, sbyte innerRatio)
	{
	}

	[Obsolete("This method is obsolete, and will be removed in future. Use character.GetMaxTrickCount instead.")]
	public int GetMaxTrickCount(CombatCharacter character)
	{
		return character.GetMaxTrickCount();
	}

	[Obsolete("This method is obsolete, and will be removed in future. Use character.IsTrickUsable instead.")]
	public bool IsTrickUsable(CombatCharacter character, sbyte trickType)
	{
		return character.IsTrickUsable(trickType);
	}

	[Obsolete("This method is obsolete, and will be removed in future. Use character.AnyUsableTrick instead.")]
	public bool HasAnyUsableTrick(CombatCharacter character)
	{
		return character.AnyUsableTrick;
	}

	[Obsolete("This method is obsolete, and will be removed in future. Use character.UsableTrickCount instead.")]
	public int GetUsableTrickCount(CombatCharacter character)
	{
		return character.UsableTrickCount;
	}

	[Obsolete("This method is obsolete, and will be removed in future. Use character.ReplaceUsableTrick instead.")]
	public int ReplaceUsableTrick(DataContext context, CombatCharacter character, sbyte trickType, int count = -1)
	{
		return character.ReplaceUsableTrick(context, trickType, count);
	}

	[Obsolete("This method is obsolete, and will be removed in future. Use character.UselessTrickCount instead.")]
	public int GetUselessTrickCount(CombatCharacter character)
	{
		return character.UselessTrickCount;
	}

	[Obsolete("This method is obsolete, and will be removed in future. Use character.TrickEquals instead.")]
	public bool TrickEquals(CombatCharacter character, sbyte trick1, sbyte trick2)
	{
		return character.TrickEquals(trick1, trick2);
	}

	[Obsolete("This method is obsolete, and will be removed in future. Use character.GetContinueTricks instead.")]
	public void GetContinueTricks(CombatCharacter character, sbyte trickType, List<int> indexList = null)
	{
		character.GetContinueTricks(trickType, indexList);
	}

	[Obsolete("This method is obsolete, and will be removed in future. Use character.CalcNormalAttackStartupFrames instead.")]
	public int GetAttackPrepareFrame(CombatCharacter character)
	{
		return character.CalcNormalAttackStartupFrames();
	}

	[Obsolete("This method is obsolete, and will be removed in future. Use character.CalcNormalAttackStartupFrames instead.")]
	public int GetAttackPrepareFrame(CombatCharacter character, GameData.Domains.Item.Weapon weapon)
	{
		return character.CalcNormalAttackStartupFrames(weapon);
	}

	[Obsolete("This method is obsolete, and will be removed in future. Use character.CalcNormalAttackAnimationFrames instead.")]
	public short GetAttackAnimationFrame(CombatCharacter character, float animDuration)
	{
		return character.CalcNormalAttackAnimationFrames(animDuration);
	}

	[DomainMethod]
	public TestAiData GetAiTestData(bool isAlly)
	{
		return new TestAiData();
	}

	[DomainMethod]
	public void SetAiOptions(AiOptions aiOptions)
	{
		AiOptions = aiOptions;
	}

	public bool CanRecoverBreath(CombatCharacter character)
	{
		bool dataValue = true;
		return DomainManager.SpecialEffect.ModifyData(character.GetId(), -1, 189, dataValue);
	}

	public void RecoverBreathValue(DataContext context, CombatCharacter character)
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		if (CanRecoverBreath(character))
		{
			int value = CFormula.CalcBreathRecoverValue(CValuePercent.op_Implicit((int)character.GetCharacter().GetRecoveryOfStanceAndBreath().Inner));
			value = character.CalcBreathRecoverValue(value);
			ChangeBreathValue(context, character, value);
		}
	}

	public int ChangeBreathValue(DataContext context, CombatCharacter character, int addValue, bool changedByEffect = false, CombatCharacter changer = null)
	{
		if (changedByEffect && addValue < 0)
		{
			addValue = DomainManager.SpecialEffect.ModifyValue(character.GetId(), 256, addValue, changer?.GetId() ?? (-1));
		}
		int breathValue = character.GetBreathValue();
		int num = Math.Clamp(breathValue + addValue, 0, character.GetMaxBreathValue());
		addValue = num - breathValue;
		if (addValue < 0 && character.PoisonOverflow(2))
		{
			character.AddPoisonAffectValue(2, (short)(-addValue * 100 / 30000));
		}
		if (character.LockMaxBreath)
		{
			num = 30000;
		}
		character.SetBreathValue(num, context);
		return addValue;
	}

	public bool CanRecoverStance(CombatCharacter character)
	{
		bool dataValue = true;
		return DomainManager.SpecialEffect.ModifyData(character.GetId(), -1, 190, dataValue);
	}

	public void RecoverStanceValue(DataContext context, CombatCharacter character, int addValue, sbyte attackPreparePointCost, bool isPursue)
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		if (CanRecoverStance(character))
		{
			addValue = CFormula.CalcStanceRecoverValue(CValuePercent.op_Implicit((int)character.GetCharacter().GetRecoveryOfStanceAndBreath().Outer), addValue, attackPreparePointCost, isPursue);
			addValue = character.CalcStanceRecoverValue(addValue);
			ChangeStanceValue(context, character, addValue);
			UpdateSkillCostBreathStanceCanUse(context, character);
		}
	}

	public int ChangeStanceValue(DataContext context, CombatCharacter character, int addValue, bool changedByEffect = false, CombatCharacter changer = null)
	{
		if (changedByEffect && addValue < 0)
		{
			addValue = DomainManager.SpecialEffect.ModifyValue(character.GetId(), 255, addValue, changer?.GetId() ?? (-1));
		}
		int stanceValue = character.GetStanceValue();
		int num = Math.Clamp(stanceValue + addValue, 0, character.GetMaxStanceValue());
		addValue = num - stanceValue;
		if (addValue < 0 && character.PoisonOverflow(3))
		{
			character.AddPoisonAffectValue(3, (short)(-addValue * 100 / 4000));
		}
		if (character.LockMaxStance)
		{
			num = 4000;
		}
		character.SetStanceValue(num, context);
		return addValue;
	}

	public void CostBreathAndStance(DataContext context, CombatCharacter character, int costBreath, int costStance, short skillId = -1)
	{
		if (costBreath > 0)
		{
			costBreath = ChangeBreathValue(context, character, -costBreath);
		}
		if (costStance > 0)
		{
			costStance = ChangeStanceValue(context, character, -costStance);
		}
		Events.RaiseCostBreathAndStance(context, character.GetId(), character.IsAlly, -costBreath, -costStance, skillId);
	}

	public void AddCombatState(DataContext context, CombatCharacter character, sbyte stateType, short stateId)
	{
		AddCombatState(context, character, stateType, stateId, 100, reverse: false, applyEffect: true, -1);
	}

	public void AddCombatState(DataContext context, CombatCharacter character, sbyte stateType, short stateId, int power)
	{
		AddCombatState(context, character, stateType, stateId, power, reverse: false, applyEffect: true, -1);
	}

	public void AddCombatState(DataContext context, CombatCharacter character, sbyte stateType, short stateId, int power, bool reverse)
	{
		AddCombatState(context, character, stateType, stateId, power, reverse, applyEffect: true, -1);
	}

	public void AddCombatState(DataContext context, CombatCharacter character, sbyte stateType, short stateId, int power, bool reverse, bool applyEffect)
	{
		AddCombatState(context, character, stateType, stateId, power, reverse, applyEffect, -1);
	}

	public void AddCombatState(DataContext context, CombatCharacter character, sbyte stateType, short stateId, int power, bool reverse, bool applyEffect, int srcCharId)
	{
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		CombatStateCollection combatStateCollection = character.GetCombatStateCollection(stateType);
		short combatStatePowerLimit = character.GetCombatStatePowerLimit(stateType);
		if (applyEffect)
		{
			stateId = (short)DomainManager.SpecialEffect.ModifyData(character.GetId(), (short)(-1), (ushort)166, (int)stateId, (int)stateType, power, reverse ? 1 : 0);
			if (stateId < 0)
			{
				return;
			}
			SpecialEffectDomain specialEffect = DomainManager.SpecialEffect;
			int id = character.GetId();
			int value = power;
			short customParam = stateId;
			BoolArray8 val = default(BoolArray8);
			((BoolArray8)(ref val))[0] = power <= 0;
			((BoolArray8)(ref val))[1] = !combatStateCollection.StateDict.ContainsKey(stateId);
			power = specialEffect.ModifyValue(id, 167, value, stateType, customParam, BoolArray8.op_Implicit(val));
			if (power == 0)
			{
				return;
			}
		}
		if (combatStateCollection.StateDict.ContainsKey(stateId))
		{
			short item = combatStateCollection.StateDict[stateId].power;
			if (item + power != 0)
			{
				(short, bool, int) value2 = combatStateCollection.StateDict[stateId];
				value2.Item1 = (short)Math.Min(item + power, combatStatePowerLimit);
				combatStateCollection.StateDict[stateId] = value2;
				((CombatStateEffect)DomainManager.SpecialEffect.Get(combatStateCollection.State2EffectId[stateId])).ChangePower(context, combatStateCollection.StateDict[stateId].power);
			}
			else
			{
				RemoveCombatState(context, character, stateType, stateId);
			}
		}
		else if (power > 0)
		{
			combatStateCollection.StateDict.Add(stateId, ((short)Math.Min(power, combatStatePowerLimit), reverse, srcCharId));
			(short, bool, int) tuple = combatStateCollection.StateDict[stateId];
			long value3 = DomainManager.SpecialEffect.AddCombatStateEffect(context, character.GetId(), stateType, stateId, tuple.Item1, tuple.Item2);
			combatStateCollection.State2EffectId[stateId] = value3;
		}
		character.SetCombatStateCollection(stateType, combatStateCollection, context);
		character.GetDefeatMarkCollection().SyncCombatStateMark(context, character);
	}

	public void RemoveCombatState(DataContext context, CombatCharacter character, sbyte stateType, short stateId)
	{
		CombatStateCollection combatStateCollection = character.GetCombatStateCollection(stateType);
		if (combatStateCollection.StateDict.ContainsKey(stateId))
		{
			combatStateCollection.StateDict.Remove(stateId);
			character.SetCombatStateCollection(stateType, combatStateCollection, context);
			character.GetDefeatMarkCollection().SyncCombatStateMark(context, character);
			DomainManager.SpecialEffect.Remove(context, combatStateCollection.State2EffectId[stateId]);
			combatStateCollection.State2EffectId.Remove(stateId);
		}
	}

	public void ClearCombatState(DataContext context, CombatCharacter character)
	{
		List<short> list = ObjectPool<List<short>>.Instance.Get();
		for (sbyte b = 0; b < 3; b++)
		{
			list.Clear();
			list.AddRange(character.GetCombatStateCollection(b).StateDict.Keys);
			for (int i = 0; i < list.Count; i++)
			{
				RemoveCombatState(context, character, b, list[i]);
			}
		}
		ObjectPool<List<short>>.Instance.Return(list);
	}

	public static (short stateId, bool reverse) CalcReversedCombatState(short stateId, bool reverse)
	{
		CombatStateItem combatStateItem = CombatState.Instance[stateId];
		if (combatStateItem.ReverseState < 0)
		{
			reverse = !reverse;
		}
		else
		{
			stateId = combatStateItem.ReverseState;
		}
		return (stateId: stateId, reverse: reverse);
	}

	public void ReverseCombatState(DataContext context, CombatCharacter character, sbyte stateType, short stateId)
	{
		if ((uint)(stateType - 1) > 1u)
		{
			PredefinedLog.Show(8, $"cannot reverse special state {stateType} {stateId}");
			return;
		}
		CombatStateCollection combatStateCollection = character.GetCombatStateCollection(stateType);
		if (!combatStateCollection.StateDict.ContainsKey(stateId))
		{
			PredefinedLog.Show(8, $"cannot reverse state of not existing {stateType} {stateId}");
		}
		else
		{
			(short, bool, int) tuple = combatStateCollection.StateDict[stateId];
			sbyte stateType2 = (sbyte)((stateType != 1) ? 1 : 2);
			bool item = tuple.Item2;
			RemoveCombatState(context, character, stateType, stateId);
			(stateId, item) = CalcReversedCombatState(stateId, item);
			AddCombatState(context, character, stateType2, stateId, tuple.Item1, item, applyEffect: false);
		}
	}

	public void InvalidateCombatStateCache(DataContext context, CombatCharacter character, sbyte stateType)
	{
		CombatStateCollection combatStateCollection = character.GetCombatStateCollection(stateType);
		foreach (long value in combatStateCollection.State2EffectId.Values)
		{
			((CombatStateEffect)DomainManager.SpecialEffect.Get(value)).InvalidateCache(context);
		}
	}

	[DomainMethod]
	public void ClearAllReserveAction(DataContext context, bool isAlly = true)
	{
		if (IsInCombat())
		{
			CombatCharacter combatCharacter = (isAlly ? _selfChar : _enemyChar);
			combatCharacter.SetCombatReserveData(CombatReserveData.Invalid, context);
		}
	}

	[DomainMethod]
	public void ClearReserveNormalAttack(DataContext context, bool isAlly = true)
	{
		if (IsInCombat())
		{
			CombatCharacter combatCharacter = (isAlly ? _selfChar : _enemyChar);
			combatCharacter.SetReserveNormalAttack(reserveNormalAttack: false, context);
		}
	}

	[DomainMethod]
	public bool SetPuppetUnyieldingFallen(DataContext context, bool unyieldingFallen)
	{
		if (!IsInCombat() || !GetIsPlaygroundCombat())
		{
			return false;
		}
		DomainManager.Extra.SetEnemyUnyieldingFallen(unyieldingFallen, context);
		return true;
	}

	[DomainMethod]
	public bool SetPuppetDisableAi(DataContext context, bool disableAi)
	{
		if (!IsInCombat() || !GetIsPlaygroundCombat())
		{
			return false;
		}
		DomainManager.Extra.SetDisableEnemyAi(disableAi, context);
		if (disableAi)
		{
			SetMoveState(0, isAlly: false);
		}
		return true;
	}

	private bool IsGuardChar(CombatCharacter character)
	{
		if (character.GetCharacter().GetCreatingType() != 2)
		{
			return false;
		}
		for (int i = 0; i < _enemyTeam.Length; i++)
		{
			int num = _enemyTeam[i];
			if (num >= 0 && DomainManager.Character.GetElement_Objects(num).GetCreatingType() == 1)
			{
				return true;
			}
		}
		return false;
	}

	public bool CanAcceptCommand()
	{
		return IsInCombat() && !CombatAboutToOver() && _selfChar.ChangeCharId < 0 && _enemyChar.ChangeCharId < 0;
	}

	public void CombatEntry(DataContext context, List<int> enemyIds, short combatConfigTemplateId)
	{
		GameData.GameDataBridge.GameDataBridge.AddDisplayEvent(DisplayEventType.StartCombat, enemyIds, combatConfigTemplateId);
		Events.RaiseCombatEntry(context, enemyIds, combatConfigTemplateId);
	}

	public (string anim, bool anyChanged) SetProperLoopAniAndParticle(DataContext context, CombatCharacter character, bool getMoveAni = false)
	{
		bool item = false;
		string properLoopAni = GetProperLoopAni(character, getMoveAni);
		if (properLoopAni != character.GetAnimationToLoop())
		{
			character.SetAnimationToLoop(properLoopAni, context);
			item = true;
		}
		string properLoopParticle = GetProperLoopParticle(character, getMoveAni);
		if (properLoopParticle != character.GetParticleToLoop())
		{
			character.SetParticleToLoop(properLoopParticle, context);
		}
		return (anim: properLoopAni, anyChanged: item);
	}

	public string GetProperLoopAni(CombatCharacter character, bool getMoveAni = false)
	{
		if (IsCharacterFallen(character))
		{
			return null;
		}
		if (character.SpecialAnimationLoop != null)
		{
			return character.SpecialAnimationLoop;
		}
		if (character.GetAttackingTrickType() >= 0)
		{
			return null;
		}
		if (!Pause && character.IsMoving && (character.KeepMoving || getMoveAni))
		{
			return character.MoveForward ? GetWalkForwardAni(character) : GetWalkBackwardAni(character);
		}
		if (character.GetPreparingSkillId() >= 0)
		{
			CombatSkillItem combatSkillItem = Config.CombatSkill.Instance[character.GetPreparingSkillId()];
			bool flag = character.BossConfig != null;
			string text = ((combatSkillItem.Type != 13) ? "" : GetMusicWeaponNameFix(character.GetWeaponData()));
			string text2 = ((string.IsNullOrEmpty(combatSkillItem.PlayerCastBossSkillPrepareAni) || flag) ? (combatSkillItem.PrepareAnimation + text + "_1") : (combatSkillItem.PlayerCastBossSkillPrepareAni + text + "_1"));
			return (combatSkillItem.EquipType == 1 && !flag) ? text2 : "C_007";
		}
		if (character.GetPreparingOtherAction() >= 0)
		{
			return GetPrepareOtherActionAnim(character);
		}
		if (character.GetAffectingDefendSkillId() >= 0)
		{
			return Config.CombatSkill.Instance[character.GetAffectingDefendSkillId()].DefendAnimation;
		}
		return GetIdleAni(character);
	}

	private string GetProperLoopParticle(CombatCharacter character, bool getMoveAni)
	{
		if (IsCharacterFallen(character) || character.GetAttackingTrickType() >= 0 || character.GetPreparingOtherAction() < 0)
		{
			return null;
		}
		OtherActionTypeItem otherActionTypeItem = Config.OtherActionType.Instance[character.GetPreparingOtherAction()];
		if (Pause || !character.IsMoving || (!character.KeepMoving && !getMoveAni))
		{
			return otherActionTypeItem.PrepareParticle;
		}
		if (_currentDistance <= GlobalConfig.Instance.FastWalkDistance)
		{
			return character.MoveForward ? otherActionTypeItem.ForwardParticle : otherActionTypeItem.BackwardParticle;
		}
		return character.MoveForward ? otherActionTypeItem.ForwardFastParticle : otherActionTypeItem.BackwardFastParticle;
	}

	public string GetPrepareOtherActionAnim(CombatCharacter character)
	{
		if (!character.IsActorSkeleton)
		{
			return "C_007";
		}
		string prepareAnim = character.PreparingOtherActionTypeConfig.PrepareAnim;
		return string.IsNullOrEmpty(prepareAnim) ? "C_007" : prepareAnim;
	}

	public string GetIdleAni(CombatCharacter character)
	{
		int usingWeaponIndex = character.GetUsingWeaponIndex();
		if (usingWeaponIndex < 0)
		{
			return "C_000";
		}
		short templateId = character.GetWeapons()[usingWeaponIndex].TemplateId;
		if (templateId < 0)
		{
			return "C_000";
		}
		string idleAni = Config.Weapon.Instance[templateId].IdleAni;
		return (!string.IsNullOrEmpty(idleAni)) ? idleAni : ((character.GetMobilityLevel() == 0) ? "C_000" : "C_000");
	}

	private string GetWalkForwardAni(CombatCharacter character)
	{
		bool flag = _currentDistance <= GlobalConfig.Instance.FastWalkDistance;
		OtherActionTypeItem preparingOtherActionTypeConfig = character.PreparingOtherActionTypeConfig;
		if (character.IsActorSkeleton && preparingOtherActionTypeConfig != null && !string.IsNullOrEmpty(flag ? preparingOtherActionTypeConfig.ForwardAnim : preparingOtherActionTypeConfig.ForwardFastAnim))
		{
			return flag ? preparingOtherActionTypeConfig.ForwardAnim : preparingOtherActionTypeConfig.ForwardFastAnim;
		}
		string text = (flag ? "M_001" : "MR_001");
		int usingWeaponIndex = character.GetUsingWeaponIndex();
		if (usingWeaponIndex < 0)
		{
			return text;
		}
		short templateId = character.GetWeapons()[usingWeaponIndex].TemplateId;
		if (templateId < 0)
		{
			return text;
		}
		WeaponItem weaponItem = Config.Weapon.Instance[templateId];
		string text2 = ((_currentDistance <= GlobalConfig.Instance.FastWalkDistance) ? weaponItem.ForwardAni : weaponItem.FastForwardAni);
		return (!string.IsNullOrEmpty(text2)) ? text2 : text;
	}

	private string GetWalkBackwardAni(CombatCharacter character)
	{
		bool flag = _currentDistance <= GlobalConfig.Instance.FastWalkDistance;
		OtherActionTypeItem preparingOtherActionTypeConfig = character.PreparingOtherActionTypeConfig;
		if (character.IsActorSkeleton && preparingOtherActionTypeConfig != null && !string.IsNullOrEmpty(flag ? preparingOtherActionTypeConfig.BackwardAnim : preparingOtherActionTypeConfig.BackwardFastAnim))
		{
			return flag ? preparingOtherActionTypeConfig.BackwardAnim : preparingOtherActionTypeConfig.BackwardFastAnim;
		}
		string text = (flag ? "M_002" : "MR_002");
		int usingWeaponIndex = character.GetUsingWeaponIndex();
		if (usingWeaponIndex < 0)
		{
			return text;
		}
		short templateId = character.GetWeapons()[usingWeaponIndex].TemplateId;
		if (templateId < 0)
		{
			return text;
		}
		WeaponItem weaponItem = Config.Weapon.Instance[templateId];
		string text2 = ((_currentDistance <= GlobalConfig.Instance.FastWalkDistance) ? weaponItem.BackwardAni : weaponItem.FastBackwardAni);
		return (!string.IsNullOrEmpty(text2)) ? text2 : text;
	}

	public string GetAvoidAni(CombatCharacter character, sbyte hitType)
	{
		if (character.BossConfig != null || character.AnimalConfig != null)
		{
			return AvoidAni[hitType];
		}
		int usingWeaponIndex = character.GetUsingWeaponIndex();
		if (usingWeaponIndex < 0)
		{
			return AvoidAni[hitType];
		}
		short templateId = character.GetWeapons()[usingWeaponIndex].TemplateId;
		if (templateId < 0)
		{
			return AvoidAni[hitType];
		}
		string[] avoidAnis = Config.Weapon.Instance[templateId].AvoidAnis;
		return (avoidAnis != null) ? avoidAnis[hitType] : AvoidAni[hitType];
	}

	public string GetHittedAni(CombatCharacter character, int injuryLevel)
	{
		int usingWeaponIndex = character.GetUsingWeaponIndex();
		if (usingWeaponIndex < 0)
		{
			return InjuryAni[injuryLevel];
		}
		short templateId = character.GetWeapons()[usingWeaponIndex].TemplateId;
		if (templateId < 0)
		{
			return InjuryAni[injuryLevel];
		}
		string[] hittedAnis = Config.Weapon.Instance[templateId].HittedAnis;
		return (hittedAnis != null) ? hittedAnis[injuryLevel] : InjuryAni[injuryLevel];
	}

	public bool IsPlayingMoveAni(CombatCharacter character)
	{
		string animationToLoop = character.GetAnimationToLoop();
		if (animationToLoop == GetWalkForwardAni(character) || animationToLoop == GetWalkBackwardAni(character))
		{
			goto IL_005a;
		}
		switch (animationToLoop)
		{
		case "M_003":
		case "M_004":
		case "M_014":
			goto IL_005a;
		}
		int result = ((animationToLoop == "M_015") ? 1 : 0);
		goto IL_005b;
		IL_005b:
		return (byte)result != 0;
		IL_005a:
		result = 1;
		goto IL_005b;
	}

	public void ChangeChangeTrickProgress(DataContext context, CombatCharacter character, int changeValue)
	{
		if (character.GetChangeTrickCount() < character.MaxChangeTrickCount)
		{
			int num = 100 + DomainManager.SpecialEffect.GetModifyValue(character.GetId(), 198, (EDataModifyType)1, -1, -1, -1, (EDataSumType)0);
			changeValue = Math.Max(changeValue * num / 100, 0);
			int num2 = Math.Clamp(character.GetChangeTrickProgress() + changeValue, 0, GlobalConfig.Instance.MaxChangeTrickProgressOnce);
			if (num2 >= GlobalConfig.Instance.MaxChangeTrickProgress)
			{
				int num3 = num2 / GlobalConfig.Instance.MaxChangeTrickProgress;
				ChangeChangeTrickCount(context, character, num3);
				num2 -= GlobalConfig.Instance.MaxChangeTrickProgress * num3;
			}
			if (character.GetChangeTrickCount() == character.MaxChangeTrickCount)
			{
				num2 = 0;
			}
			sbyte changeTrickProgress = (sbyte)Math.Clamp(num2, 0, GlobalConfig.Instance.MaxChangeTrickProgress);
			character.SetChangeTrickProgress(changeTrickProgress, context);
		}
	}

	public short GetMaxNeiliAllocation(CombatCharacter character, byte type)
	{
		return character.GetMaxNeiliAllocation(type);
	}

	public sbyte GetAttackBodyPart(CombatCharacter attacker, CombatCharacter defender, IRandomSource random, short skillId = -1, sbyte trickType = -1, sbyte hitType = -1)
	{
		if (hitType < 0 && trickType >= 0)
		{
			hitType = GetAttackHitType(attacker, trickType);
		}
		sbyte b = ((trickType >= 0 && trickType != 21) ? Config.TrickType.Instance[trickType].AvoidType : hitType);
		if ((skillId >= 0 && CombatSkillTemplateHelper.IsMindHitSkill(skillId)) || (trickType >= 0 && b == 3))
		{
			return -1;
		}
		if (trickType == 21)
		{
			trickType = GodTrickUseTrickType[hitType];
		}
		DefeatMarkCollection defeatMarkCollection = defender.GetDefeatMarkCollection();
		List<int> list = ObjectPool<List<int>>.Instance.Get();
		list.Clear();
		if (skillId >= 0 && Config.CombatSkill.Instance[skillId].InjuryPartAtkRateDistribution != null)
		{
			list.AddRange(DomainManager.CombatSkill.GetElement_CombatSkills(new CombatSkillKey(attacker.GetId(), skillId)).GetBodyPartWeights());
		}
		else if (trickType >= 0 && Config.TrickType.Instance[trickType].InjuryPartAtkRateDistribution != null)
		{
			list.AddRange(((IEnumerable<sbyte>)Config.TrickType.Instance[trickType].InjuryPartAtkRateDistribution).Select((Func<sbyte, int>)((sbyte x) => x)));
		}
		else
		{
			PredefinedLog.Show(8, $"GetAttackBodyPart {skillId} {trickType}");
			for (int num = 0; num < 7; num++)
			{
				list.Add(0);
			}
		}
		List<sbyte> list2 = ObjectPool<List<sbyte>>.Instance.Get();
		list2.Clear();
		if (!defender.GetCharacter().GetHaveLeftArm())
		{
			list2.Add(3);
		}
		if (!defender.GetCharacter().GetHaveRightArm())
		{
			list2.Add(4);
		}
		if (!defender.GetCharacter().GetHaveLeftLeg())
		{
			list2.Add(5);
		}
		if (!defender.GetCharacter().GetHaveRightLeg())
		{
			list2.Add(6);
		}
		if (list2.Count > 0)
		{
			for (int num2 = 0; num2 < list2.Count; num2++)
			{
				list[list2[num2]] = 0;
			}
			if (!list.Exists((int odds) => odds > 0))
			{
				for (sbyte b2 = 0; b2 < 7; b2++)
				{
					if (!list2.Contains(b2))
					{
						list[b2] = 1;
					}
				}
			}
		}
		ObjectPool<List<sbyte>>.Instance.Return(list2);
		if (skillId < 0)
		{
			for (sbyte b3 = 0; b3 < 7; b3++)
			{
				byte b4 = defeatMarkCollection.OuterInjuryMarkList[b3];
				byte b5 = defeatMarkCollection.InnerInjuryMarkList[b3];
				if (list[b3] > 0 && (b4 >= 3 || b5 >= 3))
				{
					list[b3] = (sbyte)Math.Max(list[b3] / 10, 1);
				}
			}
		}
		for (sbyte b6 = 0; b6 < 7; b6++)
		{
			if (list[b6] > 0)
			{
				list[b6] = DomainManager.SpecialEffect.ModifyValue(attacker.GetId(), 308, list[b6], b6);
			}
		}
		sbyte dataValue = (sbyte)RandomUtils.GetRandomIndex(list, Context.Random);
		ObjectPool<List<int>>.Instance.Return(list);
		return (sbyte)DomainManager.SpecialEffect.ModifyData(attacker.GetId(), skillId, (ushort)140, (int)dataValue, -1, -1, -1);
	}

	private int ApplyHitOddsSpecialEffect(CombatCharacter attacker, CombatCharacter defender, int hitOdds, sbyte hitType, short skillId = -1)
	{
		long num = hitOdds;
		int num2 = 100;
		num2 += DomainManager.SpecialEffect.GetModifyValue(attacker.GetId(), skillId, 74, (EDataModifyType)1, hitType, -1, -1, (EDataSumType)0);
		num2 += DomainManager.SpecialEffect.GetModifyValue(defender.GetId(), skillId, 107, (EDataModifyType)1, hitType, -1, -1, (EDataSumType)0);
		num = Math.Max(num * num2 / 100, 0L);
		(int, int) totalPercentModifyValue = DomainManager.SpecialEffect.GetTotalPercentModifyValue(attacker.GetId(), skillId, 74, hitType);
		(int, int) totalPercentModifyValue2 = DomainManager.SpecialEffect.GetTotalPercentModifyValue(defender.GetId(), skillId, 107, hitType);
		if (attacker.GetIsFightBack())
		{
			(int, int) totalPercentModifyValue3 = DomainManager.SpecialEffect.GetTotalPercentModifyValue(attacker.GetId(), skillId, 75, hitType);
			totalPercentModifyValue.Item1 = Math.Max(totalPercentModifyValue.Item1, totalPercentModifyValue3.Item1);
			totalPercentModifyValue.Item2 = Math.Min(totalPercentModifyValue.Item2, totalPercentModifyValue3.Item2);
			(int, int) totalPercentModifyValue4 = DomainManager.SpecialEffect.GetTotalPercentModifyValue(defender.GetId(), skillId, 108, hitType);
			totalPercentModifyValue2.Item1 = Math.Max(totalPercentModifyValue2.Item1, totalPercentModifyValue4.Item1);
			totalPercentModifyValue2.Item2 = Math.Min(totalPercentModifyValue2.Item2, totalPercentModifyValue4.Item2);
		}
		num2 = 100 + Math.Max(totalPercentModifyValue.Item1, totalPercentModifyValue2.Item1) + Math.Min(totalPercentModifyValue.Item2, totalPercentModifyValue2.Item2);
		num = Math.Max(num * num2 / 100, 0L);
		hitOdds = (int)Math.Min(num, 2147483647L);
		hitOdds = DomainManager.SpecialEffect.ModifyData(attacker.GetId(), skillId, 74, hitOdds);
		hitOdds = DomainManager.SpecialEffect.ModifyData(defender.GetId(), skillId, 107, hitOdds);
		return hitOdds;
	}

	public void TransferDisorderOfQi(DataContext context, CombatCharacter srcChar, CombatCharacter dstChar, int delta)
	{
		delta = Math.Min(delta, srcChar.GetCharacter().GetDisorderOfQi() - srcChar.GetOldDisorderOfQi());
		if (delta > 0)
		{
			srcChar.GetCharacter().TransferDisorderOfQi(context, dstChar.GetCharacter(), delta);
		}
	}

	public void ChangeDisorderOfQiRandomRecovery(DataContext context, CombatCharacter combatChar, int delta, bool changeToOld = false)
	{
		delta = CFormula.RandomCalcDisorderOfQiDelta(context.Random, delta);
		if (delta >= 0 || combatChar.GetOldDisorderOfQi() < combatChar.GetCharacter().GetDisorderOfQi())
		{
			if (delta < 0)
			{
				delta = Math.Clamp(delta, combatChar.GetOldDisorderOfQi() - combatChar.GetCharacter().GetDisorderOfQi(), 0);
			}
			if (delta > 0 && changeToOld)
			{
				combatChar.SetOldDisorderOfQi((short)Math.Clamp(combatChar.GetOldDisorderOfQi() + delta, DisorderLevelOfQi.MinValue, DisorderLevelOfQi.MaxValue), context);
			}
			GameData.Domains.Character.Character character = combatChar.GetCharacter();
			character.ChangeDisorderOfQi(context, delta);
		}
	}

	public static int CalcWeaponAttack(CombatCharacter combatChar, GameData.Domains.Item.Weapon weapon, short skillId)
	{
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		int num = weapon.GetEquipmentAttack();
		if (skillId >= 0 && DomainManager.CombatSkill.GetSkillType(combatChar.GetId(), skillId) == 5 && combatChar.LegSkillUseShoes())
		{
			ItemKey itemKey = combatChar.Armors[5];
			GameData.Domains.Item.Armor armor = (itemKey.IsValid() ? DomainManager.Item.GetElement_Armors(itemKey.Id) : null);
			num = CalcArmorAttack(combatChar, armor);
		}
		return num * DomainManager.SpecialEffect.GetModify(combatChar.GetId(), skillId, 141, -1, -1, -1, (EDataSumType)0);
	}

	public static int CalcWeaponDefend(CombatCharacter combatChar, GameData.Domains.Item.Weapon weapon, short skillId)
	{
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		int num = weapon.GetEquipmentDefense();
		if (skillId >= 0 && DomainManager.CombatSkill.GetSkillType(combatChar.GetId(), skillId) == 5 && combatChar.LegSkillUseShoes())
		{
			ItemKey itemKey = combatChar.Armors[5];
			GameData.Domains.Item.Armor armor = (itemKey.IsValid() ? DomainManager.Item.GetElement_Armors(itemKey.Id) : null);
			num = CalcArmorDefend(combatChar, armor);
		}
		return num * DomainManager.SpecialEffect.GetModify(combatChar.GetId(), skillId, 142, weapon.GetId(), -1, -1, (EDataSumType)0);
	}

	public static int CalcArmorAttack(CombatCharacter combatChar, GameData.Domains.Item.Armor armor)
	{
		int value = ((armor != null && armor.GetCurrDurability() > 0) ? armor.GetEquipmentAttack() : 100);
		return DomainManager.SpecialEffect.ModifyValue(combatChar.GetId(), 143, value);
	}

	public static int CalcArmorDefend(CombatCharacter combatChar, GameData.Domains.Item.Armor armor)
	{
		int value = ((armor != null && armor.GetCurrDurability() > 0) ? armor.GetEquipmentDefense() : 50);
		return DomainManager.SpecialEffect.ModifyValue(combatChar.GetId(), 144, value, armor?.GetId() ?? (-1));
	}

	public static bool IsWeaponCanBreak(short weaponSubType)
	{
		bool flag = (uint)(weaponSubType - 16) <= 1u;
		return !flag;
	}

	private void CalculateWeaponArmorBreak(CombatContext context, sbyte breakOdds)
	{
		bool flag = DomainManager.SpecialEffect.ModifyData(context.AttackerId, context.SkillTemplateId, 281, dataValue: false);
		if (context.BodyPart < 0 || breakOdds == 0 || flag)
		{
			context.Attacker.NeedReduceWeaponDurability = false;
			context.Defender.NeedReduceArmorDurability = false;
		}
		else
		{
			context.CheckReduceWeaponDurability(breakOdds);
			context.CheckReduceArmorDurability(breakOdds);
		}
	}

	public void ReduceDurabilityByHit(DataContext context, CombatCharacter character, ItemKey key)
	{
		ChangeDurability(context, character, key, -1, EChangeDurabilitySourceType.Hit);
	}

	public void CostDurability(DataContext context, CombatCharacter character, ItemKey key, int cost)
	{
		ChangeDurability(context, character, key, -cost, EChangeDurabilitySourceType.Cost);
	}

	public void ChangeDurability(DataContext context, CombatCharacter character, ItemKey key, int delta, EChangeDurabilitySourceType sourceType)
	{
		if (!key.IsValid())
		{
			return;
		}
		ItemBase baseItem = DomainManager.Item.GetBaseItem(key);
		if (baseItem.GetMaxDurability() <= 0)
		{
			return;
		}
		character.ChangingDurabilityItems.Push(key);
		delta = DomainManager.SpecialEffect.ModifyValueCustom(character.GetId(), 309, delta, key.ItemType, (int)sourceType);
		character.ChangingDurabilityItems.Pop();
		short currDurability = baseItem.GetCurrDurability();
		short num = (short)Math.Clamp(currDurability + delta, 0, baseItem.GetMaxDurability());
		if (num == currDurability)
		{
			return;
		}
		baseItem.SetCurrDurability(num, context);
		if (delta > 0)
		{
			EnsureOldDurability(key);
		}
		Events.RaiseCombatChangeDurability(context, character, key, num - currDurability);
		if (key.ItemType != 0)
		{
			return;
		}
		CombatWeaponData element_WeaponDataDict = GetElement_WeaponDataDict(key.Id);
		element_WeaponDataDict.SetDurability(num, context);
		if (num == 0)
		{
			element_WeaponDataDict.SetCanChangeTo(canChangeTo: false, context);
			int num2 = character.GetWeapons().IndexOf(key);
			if (num2 >= 0 && num2 < 3)
			{
				character.ClearUnlockAttackValue(context, num2);
			}
			if (!(key != character.GetWeapons()[character.GetUsingWeaponIndex()]) && !character.GetRawCreateCollection().Contains(key))
			{
				ChangeWeapon(context, character, 3);
			}
		}
	}

	private bool CanPlayHitAnimation(CombatCharacter character)
	{
		return !IsCharacterFallen(character) && character.GetAttackingTrickType() < 0 && character.GetAffectingDefendSkillId() < 0 && character.GetPreparingSkillId() < 0 && character.GetPreparingOtherAction() < 0 && !character.GetPreparingItem().IsValid() && character.GetAnimationToLoop() != "C_007" && IsCurrentCombatCharacter(character);
	}

	public void UpdateAllCommandAvailability(DataContext context, CombatCharacter character)
	{
		UpdateSkillCanUse(context, character);
		UpdateWeaponCanChange(context, character);
		UpdateOtherActionCanUse(context, character, -1);
		UpdateAllTeammateCommandUsable(context, character.IsAlly, -1);
		UpdateCanUseItem(context, character);
		UpdateCanChangeTrick(context, character);
		UpdateCanSurrender(context, character);
	}

	public bool CheckHit(DataContext context, CombatCharacter character, sbyte hitType, int hitValuePercent = 100)
	{
		CombatCharacter combatCharacter = GetCombatCharacter(!character.IsAlly);
		sbyte bodyPart = (sbyte)context.Random.Next(0, 7);
		int num = character.GetHitValue(hitType, bodyPart, 0, -1) * hitValuePercent / 100;
		int avoidValue = combatCharacter.GetAvoidValue(hitType, bodyPart, -1);
		int hitOdds = num * 100 / avoidValue / ((num >= avoidValue) ? 1 : 2);
		hitOdds = ApplyHitOddsSpecialEffect(character, combatCharacter, hitOdds, hitType, -1);
		return hitOdds < 0 || context.Random.CheckPercentProb(hitOdds);
	}

	public bool CheckHealthImmunity(DataContext context, CombatCharacter character)
	{
		if (character.GetCharacter().GetFeatureIds().All((short x) => !CharacterFeature.Instance[x].IgnoreHealthMark))
		{
			return false;
		}
		ShowImmunityEffectTips(context, character.GetId(), EMarkType.Health);
		return true;
	}

	public string GetMusicWeaponNameFix(CombatWeaponData weaponData)
	{
		return (weaponData.Template.ItemSubType == 11) ? "_guqin" : ((weaponData.TemplateId == 884) ? "_sing" : "_flute");
	}

	public void ShowImmunityEffectTips(DataContext context, int charId, EMarkType markType)
	{
		if (1 == 0)
		{
		}
		int num = markType switch
		{
			EMarkType.Outer => 1700, 
			EMarkType.Inner => 1699, 
			EMarkType.Flaw => 1702, 
			EMarkType.Acupoint => 1703, 
			EMarkType.Mind => 1701, 
			EMarkType.Fatal => 1704, 
			EMarkType.Die => 1705, 
			EMarkType.Health => 1708, 
			_ => -1, 
		};
		if (1 == 0)
		{
		}
		int num2 = num;
		if (num2 >= 0)
		{
			ShowSpecialEffectTips(charId, num2, 0);
		}
	}

	public void ShowWugKingEffectTips(DataContext context, int srcCharId, int dstCharId)
	{
		int effectId;
		if (srcCharId == dstCharId)
		{
			effectId = 1707;
		}
		else
		{
			if (_combatCharacterDict[srcCharId].IsAlly == _combatCharacterDict[dstCharId].IsAlly)
			{
				PredefinedLog.Show(8, $"Unexpected wug king from {srcCharId} to {dstCharId}");
				return;
			}
			effectId = 1706;
		}
		ShowSpecialEffectTips(srcCharId, effectId, 0);
	}

	public void ShowSpecialEffectTips(int charId, int effectId, byte index = 0)
	{
		if (IsInCombat() && _combatCharacterDict.TryGetValue(charId, out var value) && IsCurrentCombatCharacter(value))
		{
			int num = ShowSpecialEffectDisplayData.CheckIndex(effectId, index);
			if (num >= 0)
			{
				GetCombatCharacter(_combatCharacterDict[charId].IsAlly).NeedShowEffectList.Add(new ShowSpecialEffectDisplayData(charId, effectId, num, ItemKey.Invalid));
			}
		}
	}

	public void ShowSpecialEffectTips(int charId, int effectId, ItemKey itemKey, byte index = 0)
	{
		if (IsInCombat() && _combatCharacterDict.TryGetValue(charId, out var value) && IsCurrentCombatCharacter(value))
		{
			int num = ShowSpecialEffectDisplayData.CheckIndex(effectId, index);
			if (num >= 0)
			{
				_combatCharacterDict[charId].NeedShowEffectList.Add(new ShowSpecialEffectDisplayData(charId, effectId, num, itemKey));
			}
		}
	}

	public void ShowSpecialEffectTipsByDisplayEvent(int charId, int effectId, byte index = 0)
	{
		if (IsInCombat() && _combatCharacterDict.TryGetValue(charId, out var value))
		{
			int index2 = ShowSpecialEffectDisplayData.CheckIndex(effectId, index);
			GameData.GameDataBridge.GameDataBridge.AddDisplayEvent(arg2: new ShowSpecialEffectDisplayData(charId, effectId, index2, ItemKey.Invalid), type: DisplayEventType.CombatShowSpecialEffect, arg1: value.IsAlly);
		}
	}

	public void ShowTeammateCommand(int teammateId, int index, bool displayEvent = false)
	{
		if (!IsInCombat() || !_combatCharacterDict.TryGetValue(teammateId, out var value))
		{
			return;
		}
		List<sbyte> currTeammateCommands = value.GetCurrTeammateCommands();
		if (!currTeammateCommands.CheckIndex(index))
		{
			return;
		}
		int[] characterList = GetCharacterList(value.IsAlly);
		sbyte b = 0;
		for (int i = 0; i < characterList.Length && characterList[i] != teammateId; i++)
		{
			if (characterList[i] >= 0)
			{
				b++;
			}
		}
		b--;
		TeammateCommandDisplayData teammateCommandDisplayData = new TeammateCommandDisplayData
		{
			CmdType = currTeammateCommands[index],
			IndexCharacter = (sbyte)(characterList.IndexOf(teammateId) - 1),
			ValidIndexCharacter = b,
			IndexCommand = (sbyte)index,
			IsAlly = value.IsAlly
		};
		if (displayEvent)
		{
			GameData.GameDataBridge.GameDataBridge.AddDisplayEvent(DisplayEventType.CombatShowCommand, teammateCommandDisplayData);
			return;
		}
		CombatCharacter mainCharacter = GetMainCharacter(value.IsAlly);
		mainCharacter.NeedShowCommandList.Add(teammateCommandDisplayData);
	}

	public void Reset(DataContext context, CombatCharacter character)
	{
		character.GetCharacter().SetInjuries(default(Injuries), context);
		character.SetInjuries(default(Injuries), context);
		character.SetOldInjuries(default(Injuries), context);
		int[] outerDamageValue = character.GetOuterDamageValue();
		int[] innerDamageValue = character.GetInnerDamageValue();
		Array.Clear(outerDamageValue, 0, outerDamageValue.Length);
		Array.Clear(innerDamageValue, 0, innerDamageValue.Length);
		character.SetOuterDamageValue(outerDamageValue, context);
		character.SetInnerDamageValue(innerDamageValue, context);
		character.SetMindDamageValue(0, context);
		character.SetFatalDamageValue(0, context);
		PoisonInts poisoned = default(PoisonInts);
		character.GetCharacter().SetPoisoned(ref poisoned, context);
		character.SetPoison(ref poisoned, context);
		character.SetOldPoison(ref poisoned, context);
		character.SetFlawCount(new byte[7], context);
		character.SetFlawCollection(new FlawOrAcupointCollection(), context);
		character.SetAcupointCount(new byte[7], context);
		character.SetAcupointCollection(new FlawOrAcupointCollection(), context);
		character.SetMindMarkTime(new MindMarkList(), context);
		ClearCombatState(context, character);
		character.UnRegisterMarkHandler();
		character.SetDefeatMarkCollection(new DefeatMarkCollection(), context);
		character.RegisterMarkHandler();
	}

	public void AddBossPhase(DataContext context, CombatCharacter character, int effectId)
	{
		int id = character.GetId();
		bool isAlly = character.IsAlly;
		sbyte index = (sbyte)(character.GetBossPhase() + 1);
		InterruptSkill(context, character, -1);
		InterruptOtherAction(context, character);
		character.NeedChangeBossPhase = true;
		character.ChangeBossPhaseEffectId = effectId;
		character.ClearAllDoingOrReserveCommand(context);
		character.SetMindMarkInfinityCount(0, context);
		character.SetMindMarkInfinityProgress(0, context);
		character.SetHazardValue(0, context);
		character.GetCharacter().SetHealth(character.GetCharacter().GetLeftMaxHealth(), context);
		character.GetCharacter().SetDisorderOfQi(character.GetOldDisorderOfQi(), context);
		character.SetMixPoisonAffectedCount(character.GetMixPoisonAffectedCount().Clear(), context);
		character.GetCharacter().ClearEatingItems(context);
		ClearSkillEffect(context, character);
		List<short> attackSkillList = character.GetAttackSkillList();
		short[] array = character.BossConfig.PhaseAttackSkills[index];
		for (int i = 0; i < attackSkillList.Count; i++)
		{
			short num = attackSkillList[i];
			if (num > 0)
			{
				CombatSkillKey objectId = new CombatSkillKey(id, num);
				DomainManager.SpecialEffect.Remove(context, id, num, 1);
				RemoveElement_SkillDataDict(objectId);
			}
		}
		for (sbyte b = 0; b < attackSkillList.Count; b++)
		{
			short num2 = (attackSkillList[b] = (short)((b < array.Length) ? array[b] : (-1)));
			if (num2 >= 0)
			{
				DomainManager.SpecialEffect.Add(context, id, num2, 1, -1);
				AddCombatSkillData(context, id, num2);
			}
		}
		character.SetAttackSkillList(attackSkillList, context);
		if (character.BossConfig.PhaseWeapons != null)
		{
			short[] array2 = character.BossConfig.PhaseWeapons[index];
			ItemKey[] weapons = character.GetWeapons();
			GameData.Domains.Character.Character character2 = character.GetCharacter();
			ItemKey[] equipment = character2.GetEquipment();
			sbyte[] array3 = EquipmentSlot.EquipmentType2Slots[0];
			for (int j = 0; j < array3.Length; j++)
			{
				ItemKey itemKey = equipment[array3[j]];
				if (itemKey.IsValid())
				{
					character2.ChangeEquipment(context, array3[j], -1, ItemKey.Invalid);
					character2.RemoveInventoryItem(context, itemKey, 1, deleteItem: true);
					RemoveElement_WeaponDataDict(itemKey.Id);
				}
			}
			for (int k = 0; k < array2.Length; k++)
			{
				ItemKey itemKey2 = DomainManager.Item.CreateWeapon(context, array2[k], 0);
				List<sbyte> weaponTricks = DomainManager.Item.GetWeaponTricks(itemKey2);
				CombatWeaponData combatWeaponData = new CombatWeaponData(itemKey2, character);
				sbyte[] weaponTricks2 = combatWeaponData.GetWeaponTricks();
				character2.AddInventoryItem(context, itemKey2, 1);
				character2.ChangeEquipment(context, -1, array3[k], itemKey2);
				weapons[k] = itemKey2;
				AddElement_WeaponDataDict(itemKey2.Id, combatWeaponData);
				combatWeaponData.Init(context, k);
				for (int l = 0; l < weaponTricks.Count; l++)
				{
					weaponTricks2[l] = weaponTricks[l];
				}
			}
			character.SetWeapons(weapons, context);
			character.SetUsingWeaponIndex(character.GetUsingWeaponIndex(), context);
			UpdateCanChangeTrick(context, character);
		}
		else
		{
			ItemKey[] weapons2 = character.GetWeapons();
			for (int m = 0; m < 3; m++)
			{
				if (weapons2[m].IsValid())
				{
					ClearWeaponCd(context, character, m);
				}
			}
		}
		if (character.BossConfig.FailPlayerAni != null && character.BossConfig.FailPlayerAni.Count > character.GetBossPhase())
		{
			DomainManager.Combat.ForceAllTeammateLeaveCombatField(context, !isAlly);
		}
		Events.RaiseChangeBossPhase(context);
	}

	[DomainMethod]
	public void SetMoveState(byte state, bool isAlly = true, bool settedByPlayer = false)
	{
		if (!CanAcceptCommand())
		{
			return;
		}
		CombatCharacter combatCharacter = (isAlly ? _selfChar : _enemyChar);
		DataContext dataContext = combatCharacter.GetDataContext();
		byte b = (byte)(combatCharacter.KeepMoving ? (combatCharacter.MoveForward ? 1 : 2) : 0);
		if (b != state)
		{
			combatCharacter.KeepMoving = state != 0;
			if (isAlly)
			{
				combatCharacter.PlayerControllingMove = settedByPlayer && combatCharacter.KeepMoving;
			}
			if (state != combatCharacter.MoveData.JumpPrepareDirection && (combatCharacter.KeepMoving || (combatCharacter.MoveData.CanPartlyJump && combatCharacter.GetJumpPreparedDistance() > 0)))
			{
				combatCharacter.MoveData.ResetJumpState(dataContext);
			}
			if (combatCharacter.KeepMoving)
			{
				combatCharacter.MoveForward = state == 1;
				combatCharacter.MoveData.JumpPrepareDirection = state;
			}
			else
			{
				SetProperLoopAniAndParticle(dataContext, combatCharacter);
			}
			Events.RaiseMoveStateChanged(dataContext, combatCharacter, state);
		}
	}

	[DomainMethod]
	public void SetTargetDistance(DataContext context, short targetDistance, bool isAlly = true)
	{
		if (IsInCombat() && !IsAiMoving)
		{
			CombatCharacter combatCharacter = GetCombatCharacter(isAlly);
			combatCharacter.PlayerTargetDistance = targetDistance;
			if (_timeScale <= 0f)
			{
				combatCharacter.SetTargetDistance(targetDistance, context);
			}
		}
	}

	[DomainMethod]
	public void ClearTargetDistance(DataContext context)
	{
		SetTargetDistance(context, -1);
	}

	[DomainMethod]
	public bool SetJumpThreshold(DataContext context, short combatSkillId, short jumpThreshold)
	{
		return DomainManager.Extra.SetJumpThreshold(context, combatSkillId, jumpThreshold);
	}

	public (byte min, byte max) GetDistanceRange()
	{
		return (min: CombatConfig.MinDistance, max: CombatConfig.MaxDistance);
	}

	public short GetMoveRangeOffsetCurrentDistance(int offset)
	{
		return GetMoveRangeDistance(_currentDistance + offset);
	}

	public short GetMoveRangeDistance(int distance)
	{
		var (min, max) = GetDistanceRange();
		return (short)Math.Clamp(distance, min, max);
	}

	public short GetNearlyOutDistance(OuterAndInnerShorts range)
	{
		(byte, byte) distanceRange = GetDistanceRange();
		int num = _currentDistance - range.Outer;
		int num2 = range.Inner - _currentDistance;
		bool flag = range.Outer - 1 >= distanceRange.Item1;
		bool flag2 = range.Inner + 1 <= distanceRange.Item2;
		if (!flag && !flag2)
		{
			return -1;
		}
		return (short)(((num < num2 || !flag2) && flag) ? (range.Outer - 1) : (range.Inner + 1));
	}

	public bool CanMove(CombatCharacter combatChar, bool forward)
	{
		if (!combatChar.GetCharacter().Template.CanMove)
		{
			return false;
		}
		if (combatChar.IsMoving)
		{
			return false;
		}
		(byte, byte) distanceRange = GetDistanceRange();
		if (forward ? (_currentDistance <= distanceRange.Item1) : (_currentDistance >= distanceRange.Item2))
		{
			return false;
		}
		if (combatChar.TeammateBeforeMainChar >= 0 || combatChar.TeammateAfterMainChar >= 0)
		{
			return false;
		}
		if (_isTutorialCombat && !combatChar.CanRecoverMobility)
		{
			return false;
		}
		CombatCharacterStateType currentStateType = combatChar.StateMachine.GetCurrentStateType();
		bool flag;
		if (currentStateType == CombatCharacterStateType.PrepareSkill)
		{
			flag = (forward ? combatChar.MoveData.CanMoveForwardInSkillPrepareDist : combatChar.MoveData.CanMoveBackwardInSkillPrepareDist) > 0;
		}
		else
		{
			bool flag2 = ((currentStateType == CombatCharacterStateType.Idle || currentStateType == CombatCharacterStateType.PrepareOtherAction) ? true : false);
			flag = flag2;
		}
		short templateId = DomainManager.Combat.CombatConfig.TemplateId;
		if ((uint)(templateId - 157) <= 1u)
		{
			flag = flag && combatChar.GetAffectingMoveSkillId() >= 0;
		}
		return flag;
	}

	public void RecoverMobilityValue(DataContext context, CombatCharacter character)
	{
		int mobilityValue = character.GetMobilityValue();
		int maxMobility = character.GetMaxMobility();
		if (mobilityValue < maxMobility && character.CanRecoverMobility)
		{
			ChangeMobilityValue(context, character, character.GetMobilityRecoverSpeed());
		}
	}

	public void ChangeMobilityValue(DataContext context, CombatCharacter character, int addValue, bool changedByEffect = false, CombatCharacter changer = null, bool costBySkill = false)
	{
		if (changedByEffect && addValue < 0)
		{
			if (!DomainManager.SpecialEffect.ModifyData(character.GetId(), -1, 149, dataValue: true, changer?.GetId() ?? (-1)))
			{
				return;
			}
			addValue = DomainManager.SpecialEffect.ModifyValue(character.GetId(), 150, addValue, changer?.GetId() ?? (-1));
		}
		int maxMobility = character.GetMaxMobility();
		int num = Math.Clamp(character.GetMobilityValue() + addValue, 0, maxMobility);
		if (num != character.GetMobilityValue())
		{
			character.SetMobilityValue(num, context);
			if (IsCurrentCombatCharacter(character))
			{
				UpdateSkillNeedMobilityCanUse(context, character);
			}
		}
	}

	public bool ChangeDistance(DataContext context, CombatCharacter mover, int addDistance)
	{
		return ChangeDistance(context, mover, addDistance, isForced: false, canStop: true);
	}

	public bool ChangeDistance(DataContext context, CombatCharacter mover, int addDistance, bool isForced)
	{
		return ChangeDistance(context, mover, addDistance, isForced, canStop: true);
	}

	public bool ChangeDistance(DataContext context, CombatCharacter mover, int addDistance, bool isForced, bool canStop)
	{
		bool flag = DomainManager.SpecialEffect.ModifyData(-1, -1, 244, dataValue: false);
		if (flag && canStop)
		{
			return false;
		}
		bool flag2 = DomainManager.SpecialEffect.ModifyData(mover.GetId(), -1, 157, dataValue: true);
		int num = addDistance;
		if (flag2)
		{
			num = DomainManager.SpecialEffect.ModifyData(mover.GetId(), -1, 151, addDistance, (addDistance < 0) ? 1 : 0);
			if (!DomainManager.SpecialEffect.ModifyData(mover.GetId(), -1, 147, dataValue: true))
			{
				num = ((addDistance > 0) ? Math.Max(addDistance, num) : Math.Min(addDistance, num));
			}
			if (isForced && !DomainManager.SpecialEffect.ModifyData(mover.GetId(), -1, 148, dataValue: true, addDistance))
			{
				Events.RaiseIgnoredForceChangeDistance(context, mover, addDistance);
				return false;
			}
		}
		(byte, byte) distanceRange = GetDistanceRange();
		byte b = (byte)Math.Clamp(_currentDistance + num, distanceRange.Item1, distanceRange.Item2);
		int num2 = b - _currentDistance;
		if (num2 == 0)
		{
			return true;
		}
		int num3 = mover.GetCurrentPosition() + num2 * ((!mover.IsAlly) ? 1 : (-1));
		bool flag3 = _currentDistance <= GlobalConfig.Instance.FastWalkDistance != b <= GlobalConfig.Instance.FastWalkDistance;
		mover.SetCurrentPosition((short)num3, context);
		SetCurrentDistance(b, context);
		UpdateSkillNeedDistanceCanUse(context, _selfChar);
		UpdateSkillNeedDistanceCanUse(context, _enemyChar);
		UpdateOtherActionCanUse(context, _selfChar, 2);
		UpdateOtherActionCanUse(context, _enemyChar, 2);
		UpdateAllTeammateCommandUsable(context, isAlly: true, -1);
		UpdateAllTeammateCommandUsable(context, isAlly: false, -1);
		UpdateShowUseSpecialMisc(context);
		if (flag3)
		{
			SetProperLoopAniAndParticle(context, _selfChar);
			SetProperLoopAniAndParticle(context, _enemyChar);
		}
		if (flag2 && mover.PoisonOverflow(1))
		{
			mover.AddPoisonAffectValue(1, (short)Math.Abs(num));
		}
		Events.RaiseDistanceChanged(context, mover, (short)num2, flag2, isForced);
		return true;
	}

	public void EnsureDistance(DataContext context, CombatCharacter checker)
	{
		ChangeDistance(context, checker, 0, isForced: false, canStop: false);
	}

	public int GetSkillCostMobilityPerFrame(CombatCharacter character, short skillId)
	{
		GameData.Domains.CombatSkill.CombatSkill element_CombatSkills = DomainManager.CombatSkill.GetElement_CombatSkills(new CombatSkillKey(character.GetId(), skillId));
		int num = element_CombatSkills.GetBreakoutGridCombatSkillPropertyBonus(16);
		foreach (SkillBreakPageEffectImplementItem pageEffect in element_CombatSkills.GetPageEffects())
		{
			num += pageEffect.CostMobilityByFrame;
		}
		foreach (SkillBreakPlateBonus breakBonuse in element_CombatSkills.GetBreakBonuses())
		{
			num += breakBonuse.CalcCostMobilityByFrame();
		}
		int mobilityReduceSpeed = Config.CombatSkill.Instance[skillId].MobilityReduceSpeed;
		mobilityReduceSpeed = DomainManager.SpecialEffect.ModifyValue(character.GetId(), skillId, 179, mobilityReduceSpeed, -1, -1, -1, 0, num);
		return Math.Max(mobilityReduceSpeed, 0);
	}

	public int GetSkillMoveCostMobility(CombatCharacter character, short skillId)
	{
		int id = character.GetId();
		GameData.Domains.CombatSkill.CombatSkill element_CombatSkills = DomainManager.CombatSkill.GetElement_CombatSkills(new CombatSkillKey(id, skillId));
		int num = element_CombatSkills.GetBreakoutGridCombatSkillPropertyBonus(17);
		foreach (SkillBreakPageEffectImplementItem pageEffect in element_CombatSkills.GetPageEffects())
		{
			num += pageEffect.CostMobilityByMove;
		}
		foreach (SkillBreakPlateBonus breakBonuse in element_CombatSkills.GetBreakBonuses())
		{
			num += breakBonuse.CalcCostMobilityByMove();
		}
		int moveCostMobility = Config.CombatSkill.Instance[skillId].MoveCostMobility;
		return DomainManager.SpecialEffect.ModifyValueCustom(id, skillId, 175, moveCostMobility, -1, -1, -1, 0, num);
	}

	public bool InAttackRange(CombatCharacter character)
	{
		if (character.GetCanAttackOutRange())
		{
			return true;
		}
		short currentDistance = GetCurrentDistance();
		OuterAndInnerShorts attackRange = character.GetAttackRange();
		return currentDistance >= attackRange.Outer && currentDistance <= attackRange.Inner;
	}

	public bool AnyAttackRangeEdge(CombatCharacter character)
	{
		if (character.GetCanAttackOutRange())
		{
			return false;
		}
		(byte, byte) distanceRange = GetDistanceRange();
		OuterAndInnerShorts attackRange = character.GetAttackRange();
		return distanceRange.Item1 < attackRange.Outer || distanceRange.Item2 > attackRange.Inner;
	}

	public bool IsMovedByTeammate(CombatCharacter character)
	{
		if (character.TeammateAfterMainChar < 0)
		{
			return false;
		}
		CombatCharacter combatCharacter = _combatCharacterDict[character.TeammateAfterMainChar];
		ETeammateCommandImplement executingTeammateCommandImplement = combatCharacter.ExecutingTeammateCommandImplement;
		return (uint)(executingTeammateCommandImplement - 2) <= 1u;
	}

	public int GetDisplayPosition(bool isAlly, short distance)
	{
		CombatCharacter combatCharacter = DomainManager.Combat.GetCombatCharacter(!isAlly, tryGetCoverCharacter: true);
		int num = combatCharacter.GetDisplayPosition();
		if (num == int.MinValue)
		{
			num = combatCharacter.GetCurrentPosition();
		}
		return (short)(isAlly ? (num - distance) : (num + distance));
	}

	public void SetDisplayPosition(DataContext context, bool isAlly, int displayPos)
	{
		CombatCharacter combatCharacter = GetCombatCharacter(isAlly);
		int num = ((displayPos != int.MinValue) ? displayPos : combatCharacter.GetCurrentPosition());
		if (combatCharacter.TeammateBeforeMainChar >= 0)
		{
			CombatCharacter combatCharacter2 = _combatCharacterDict[combatCharacter.TeammateBeforeMainChar];
			short posOffset = combatCharacter2.ExecutingTeammateCommandConfig.PosOffset;
			combatCharacter2.SetDisplayPosition(num, context);
			num = (displayPos = num + posOffset * ((!isAlly) ? 1 : (-1)));
		}
		combatCharacter.SetDisplayPosition(displayPos, context);
		if (combatCharacter.TeammateAfterMainChar >= 0)
		{
			CombatCharacter combatCharacter3 = _combatCharacterDict[combatCharacter.TeammateAfterMainChar];
			short posOffset2 = combatCharacter3.ExecutingTeammateCommandConfig.PosOffset;
			combatCharacter3.SetDisplayPosition(num + posOffset2 * (isAlly ? 1 : (-1)), context);
		}
	}

	public void EnableJumpMove(CombatCharacter character, short skillId)
	{
		CombatSkillItem combatSkillItem = Config.CombatSkill.Instance[skillId];
		EnableJumpMove(character, skillId, combatSkillItem.CanPartlyJump, combatSkillItem.MaxJumpDistance, combatSkillItem.MaxJumpDistance, combatSkillItem.JumpPrepareFrame);
	}

	public void EnableJumpMove(CombatCharacter character, short skillId, bool isForward)
	{
		CombatSkillItem combatSkillItem = Config.CombatSkill.Instance[skillId];
		EnableJumpMove(character, skillId, combatSkillItem.CanPartlyJump, (short)(isForward ? combatSkillItem.MaxJumpDistance : 0), (short)((!isForward) ? combatSkillItem.MaxJumpDistance : 0), combatSkillItem.JumpPrepareFrame);
	}

	public void EnableJumpMove(CombatCharacter character, short skillId, bool canPartlyJump, short maxForwardDist, short maxBackwardDist, int prepareFrame)
	{
		character.MoveData.JumpMoveSkillId = skillId;
		character.MoveData.CanPartlyJump = canPartlyJump;
		character.MoveData.MaxJumpForwardDist = maxForwardDist;
		character.MoveData.MaxJumpBackwardDist = maxBackwardDist;
		character.MoveData.PrepareProgressUnit = prepareFrame;
		if (skillId >= 0 && Config.CombatSkill.Instance[skillId].EquipType == 2)
		{
			character.PauseJumpMoveSkillId = skillId;
		}
	}

	public void DisableJumpMove(DataContext context, CombatCharacter character, short skillId)
	{
		if (character.MoveData.JumpMoveSkillId == skillId)
		{
			DisableJumpMove(context, character);
		}
	}

	public void DisableJumpMove(DataContext context, CombatCharacter character)
	{
		character.MoveData.JumpMoveSkillId = -1;
		character.MoveData.MaxJumpForwardDist = 0;
		character.MoveData.MaxJumpBackwardDist = 0;
		character.MoveData.PrepareProgressUnit = 0;
		character.MoveData.ResetJumpState(context);
	}

	[DomainMethod]
	public CombatResultDisplayData GetCombatResultDisplayData()
	{
		return _combatResultData;
	}

	[DomainMethod]
	public void SelectGetItem(DataContext context, List<ItemKey> acceptItems, List<int> acceptCounts)
	{
		if (acceptItems != null)
		{
			GameData.Domains.Character.Character character = _selfChar.GetCharacter();
			for (int i = 0; i < acceptItems.Count; i++)
			{
				ItemKey itemKey = acceptItems[i];
				if (!_combatResultData.ItemSrcCharDict.ContainsKey(itemKey))
				{
					continue;
				}
				if (_combatResultData.ItemSrcCharDict[itemKey] != _selfChar.GetId())
				{
					GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(_combatResultData.ItemSrcCharDict[itemKey]);
					int num = acceptCounts[i];
					int num2 = element_Objects.GetInventory().Items[itemKey] - num;
					DomainManager.Character.TransferInventoryItem(context, element_Objects, character, itemKey, num);
					if (num2 > 0)
					{
						element_Objects.RemoveInventoryItem(context, itemKey, num2, deleteItem: true);
					}
				}
				else
				{
					DomainManager.TaiwuEvent.SetListenerEventActionISerializableArg("CombatOver", "CarrierItemKeyGotInCombat", (ISerializableGameData)(object)itemKey);
				}
				_combatResultData.ItemSrcCharDict.Remove(itemKey);
			}
		}
		foreach (KeyValuePair<ItemKey, int> item in _combatResultData.ItemSrcCharDict)
		{
			GameData.Domains.Character.Character element_Objects2 = DomainManager.Character.GetElement_Objects(item.Value);
			element_Objects2.RemoveInventoryItem(context, item.Key, element_Objects2.GetInventory().Items[item.Key], deleteItem: true);
		}
	}

	public void UpdateMaxSkillGrade(bool isAlly, short skillId)
	{
		if (isAlly)
		{
			SelfMaxSkillGrade = Math.Max(SelfMaxSkillGrade, Config.CombatSkill.Instance[skillId].Grade);
		}
		else
		{
			EnemyMaxSkillGrade = Math.Max(EnemyMaxSkillGrade, Config.CombatSkill.Instance[skillId].Grade);
		}
	}

	public bool IsCharInLoot(int charId)
	{
		return _lootCharList.Contains(charId);
	}

	public void AppendGetChar(int charId)
	{
		_lootCharList.Add(charId);
	}

	public void AppendGetItem(ItemKey itemKey)
	{
		_combatResultData.ItemList.Add(DomainManager.Item.GetItemDisplayData(itemKey, _selfCharId));
		_combatResultData.ItemSrcCharDict.Add(itemKey, _selfCharId);
	}

	public void AppendEvaluation(sbyte evaluationId)
	{
		if (!_combatResultData.EvaluationList.Contains(evaluationId))
		{
			_combatResultData.EvaluationList.Add(evaluationId);
		}
	}

	public bool CheckEvaluation(sbyte evaluationTemplateId)
	{
		CombatEvaluationItem combatEvaluationItem = CombatEvaluation.Instance[evaluationTemplateId];
		if (combatEvaluationItem.AvailableInPlayground != _isPlaygroundCombat)
		{
			return false;
		}
		List<short> requireCombatConfigs = combatEvaluationItem.RequireCombatConfigs;
		if (requireCombatConfigs != null && requireCombatConfigs.Count > 0 && !combatEvaluationItem.RequireCombatConfigs.Contains(CombatConfig.TemplateId))
		{
			return false;
		}
		return combatEvaluationItem.CombatTypes.Exist(_combatType);
	}

	private void CalcEvaluationList(DataContext context)
	{
		foreach (CombatEvaluationItem item in (IEnumerable<CombatEvaluationItem>)CombatEvaluation.Instance)
		{
			ECombatEvaluationExtraCheck extraCheck = item.ExtraCheck;
			bool flag = (uint)(extraCheck - -1) <= 1u;
			if (flag || (item.NeedWin && !IsWin(isAlly: true)) || !CheckEvaluation(item.TemplateId))
			{
				continue;
			}
			if (item.ExtraCheck != ECombatEvaluationExtraCheck.None)
			{
				if (!EvaluationCheckers.TryGetValue(item.ExtraCheck, out var value))
				{
					PredefinedLog.Show(8, $"Unexpect checker type {item.ExtraCheck}, evaluation will always be true.");
				}
				else if (!value())
				{
					continue;
				}
			}
			AppendEvaluation(item.TemplateId);
		}
		_combatResultData.EvaluationList.Sort();
	}

	private void CalcReadInCombat(DataContext context)
	{
		_combatResultData.ShowReadingEvent = false;
		sbyte readInCombatCount = DomainManager.Taiwu.GetReadInCombatCount();
		ItemKey curReadingBook = DomainManager.Taiwu.GetCurReadingBook();
		if (readInCombatCount <= 0 || !curReadingBook.IsValid() || Config.SkillBook.Instance[curReadingBook.TemplateId].CombatSkillTemplateId < 0)
		{
			return;
		}
		int percentProb = _combatResultData.SelectEvaluations((CombatEvaluationItem x) => x.ReadInCombatRate).Sum((int x) => x);
		if (context.Random.CheckPercentProb(percentProb))
		{
			DomainManager.Taiwu.SetReadInCombatCount((sbyte)(readInCombatCount - 1), context);
			_combatResultData.EvaluationList.Add(33);
			_combatResultData.ShowReadingEvent = DomainManager.Taiwu.UpdateReadingProgressInCombat(context);
			if (_combatResultData.ShowReadingEvent)
			{
				DomainManager.Extra.AddReadingEventBookId(context, curReadingBook.Id);
			}
		}
	}

	private void CalcQiQrtInCombat(DataContext context)
	{
		sbyte loopInCombatCount = DomainManager.Extra.GetLoopInCombatCount();
		_combatResultData.ShowLoopingEvent = false;
		if (loopInCombatCount <= 0)
		{
			return;
		}
		GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
		short loopingNeigong = taiwu.GetLoopingNeigong();
		if (loopingNeigong >= 0)
		{
			int num = _combatResultData.SelectEvaluations((CombatEvaluationItem x) => x.QiArtCombatRate).Sum((int x) => x);
			if (context.Random.CheckPercentProb(num))
			{
				DomainManager.Extra.SetLoopInCombatCount((sbyte)(loopInCombatCount - 1), context);
				DomainManager.Taiwu.ApplyNeigongLoopingImprovementOnce(context);
				_combatResultData.EvaluationList.Add(43);
				InstantNotificationCollection instantNotificationCollection = DomainManager.World.GetInstantNotificationCollection();
				instantNotificationCollection.AddQiArtInCombatNoChance(loopingNeigong);
				_combatResultData.ShowLoopingEvent = DomainManager.Taiwu.TryAddLoopingEvent(context, num);
			}
		}
	}

	private void CalcAddLegacyPoint(DataContext context)
	{
		bool flag = CombatResultType.IsPlayerWin(_combatResultData.CombatStatus);
		Dictionary<short, int> dictionary = ObjectPool<Dictionary<short, int>>.Instance.Get();
		dictionary.Clear();
		foreach (CombatEvaluationItem evaluation in _combatResultData.Evaluations)
		{
			foreach (LegacyPointReference item in evaluation.AddLegacyPoint)
			{
				int num = (flag ? item.WinPercent : item.FailPercent);
				dictionary[item.TemplateId] = dictionary.GetOrDefault(item.TemplateId) + num;
			}
		}
		foreach (var (templateId, num4) in dictionary)
		{
			if (num4 > 0)
			{
				DomainManager.Taiwu.AddLegacyPoint(context, templateId, num4);
			}
		}
		ObjectPool<Dictionary<short, int>>.Instance.Return(dictionary);
	}

	private void CalcAndAddFameAction(DataContext context)
	{
		GameData.Domains.Character.Character character = _selfChar.GetCharacter();
		foreach (short item in from x in _combatResultData.SelectEvaluations((CombatEvaluationItem x) => x.FameAction)
			where x >= 0
			select x)
		{
			character.RecordFameAction(context, item, -1, 1);
		}
	}

	private void CalcAndAddAreaSpiritualDebt(DataContext context)
	{
		int num = 0;
		num += _combatResultData.SelectEvaluations((CombatEvaluationItem x) => x.AreaSpiritualDebt).Sum((short x) => x);
		if (IsWin(isAlly: true))
		{
			num += GetAddAreaSpiritualDebt(_enemyTeam);
		}
		_combatResultData.AreaSpiritualDebt = num;
		if (num != 0)
		{
			Location location = _selfChar.GetCharacter().GetLocation();
			if (!location.IsValid())
			{
				location = _selfChar.GetCharacter().GetValidLocation();
			}
			MapAreaData element_Areas = DomainManager.Map.GetElement_Areas(location.AreaId);
			DomainManager.Extra.ChangeAreaSpiritualDebt(context, location.AreaId, _combatResultData.AreaSpiritualDebt);
		}
	}

	private void CalcAndAddExp(DataContext context)
	{
		if (!CombatConfig.DropResource || _isPlaygroundCombat)
		{
			return;
		}
		int baseValue = CalcAddBase(_enemyTeam, GlobalConfig.Instance.CombatGetExpBase);
		GameData.Domains.Character.Character character = _selfChar.GetCharacter();
		int num = ((!character.IsLoseConsummateBonusByFeature()) ? character.GetConsummateLevel() : 0);
		int expBonus = ConsummateLevel.Instance[num].ExpBonus;
		baseValue = _combatResultData.ModifyValue(baseValue, (CombatEvaluationItem x) => x.ExpAddPercent, (CombatEvaluationItem x) => x.ExpTotalPercent, expBonus);
		baseValue = Math.Max(baseValue, 0);
		_combatResultData.Exp = baseValue;
		DomainManager.Taiwu.GetTaiwu().ChangeExp(context, _combatResultData.Exp);
		Events.RaiseEvaluationAddExp(context, _combatResultData.Exp);
		if (_enemyChar.GetCharacter().GetConsummateLevel() < num || !IsWin(isAlly: true))
		{
			return;
		}
		bool flag = _enemyChar.GetCharacter().GetCreatingType() == 1;
		sbyte orgTemplateId = _enemyChar.GetCharacter().GetOrganizationInfo().OrgTemplateId;
		foreach (ExpAddProfessionSeniorityData allDatum in ExpAddProfessionSeniorityData.AllData)
		{
			if ((allDatum.RequireCombatType < 0 || allDatum.RequireCombatType == _combatType) && (!allDatum.RequireIntelligent || flag) && (allDatum.RequireOrganization < 0 || allDatum.RequireOrganization == orgTemplateId))
			{
				allDatum.DoAddSeniority(context, baseValue);
			}
		}
	}

	private void CalcAndAddResource(DataContext context)
	{
		if (!CombatConfig.DropResource || _isPlaygroundCombat)
		{
			return;
		}
		for (sbyte b = 0; b < 8; b++)
		{
			int num = CalcAddResource(_enemyTeam, b);
			if (b == 7)
			{
				num += CalcAddBase(_enemyTeam, GlobalConfig.Instance.CombatGetAuthorityBase);
			}
			num = num * DomainManager.World.GetGainResourcePercent(8) / 100;
			if (b == 7)
			{
				num = _combatResultData.ModifyValue(num, (CombatEvaluationItem x) => x.AuthorityAddPercent, (CombatEvaluationItem x) => x.AuthorityTotalPercent);
			}
			else if (!_combatResultData.IsWin)
			{
				num = 0;
			}
			num = Math.Max(num, 0);
			_combatResultData.Resource[b] = num;
			_selfChar.GetCharacter().ChangeResource(context, b, num);
		}
	}

	private void CalcAndAddProficiency(DataContext context)
	{
		if (!CombatConfig.DropResource || _isPlaygroundCombat || _combatResultData.Evaluations.Any((CombatEvaluationItem evaluation) => !evaluation.AllowProficiency))
		{
			return;
		}
		foreach (short combatSkillId in _selfChar.GetCombatSkillIds())
		{
			CombatSkillKey combatSkillKey = new CombatSkillKey(_selfChar.GetId(), combatSkillId);
			int delta = CalcProficiencyDeltaTarget(context.Random, combatSkillKey);
			int num = DomainManager.Extra.ChangeCombatSkillProficiency(context, combatSkillKey, delta);
			if (num != 0)
			{
				CombatResultDisplayData combatResultData = _combatResultData;
				if (combatResultData.ChangedProficiencies == null)
				{
					combatResultData.ChangedProficiencies = new Dictionary<short, int>();
				}
				_combatResultData.ChangedProficiencies[combatSkillId] = DomainManager.Extra.GetElement_CombatSkillProficiencies(combatSkillKey);
				combatResultData = _combatResultData;
				if (combatResultData.ChangedProficienciesDelta == null)
				{
					combatResultData.ChangedProficienciesDelta = new Dictionary<short, int>();
				}
				_combatResultData.ChangedProficienciesDelta[combatSkillId] = num;
			}
		}
		if (_enemyChar.GetCharacter().GetCreatingType() != 1)
		{
			return;
		}
		foreach (short combatSkillId2 in _enemyChar.GetCombatSkillIds())
		{
			CombatSkillKey key = new CombatSkillKey(_enemyChar.GetId(), combatSkillId2);
			int delta2 = CalcProficiencyDeltaTarget(context.Random, key);
			DomainManager.Extra.ChangeCombatSkillProficiency(context, key, delta2);
		}
	}

	private int CalcProficiencyDeltaTarget(IRandomSource random, CombatSkillKey key)
	{
		sbyte equipType = Config.CombatSkill.Instance[key.SkillTemplateId].EquipType;
		if (GameData.Domains.Character.CombatSkillHelper.IsProactiveSkill(equipType))
		{
			return _skillCastTimes.GetOrDefault(key) * GlobalConfig.ProactiveProficiencyFactor[_combatType];
		}
		int num = 1;
		int num2 = 3;
		return random.Next(num, num2 + 1);
	}

	public static short GetWorldLootRatePercent()
	{
		short lootYield = DomainManager.Extra.GetLootYield();
		WorldCreationItem worldCreationItem = WorldCreation.Instance[(byte)14];
		return worldCreationItem.InfluenceFactors[lootYield];
	}

	private unsafe void CalcLootItem(DataContext context)
	{
		int lootItemRate = CombatConfig.LootItemRate;
		lootItemRate = lootItemRate * GetWorldLootRatePercent() / 100;
		sbyte combatType = _combatType;
		bool flag = ((combatType == 0 || combatType == 3) ? true : false);
		if (flag || !CombatConfig.AllowDropItem || _combatStatus != CombatStatusType.EnemyFail || (lootItemRate <= 0 && !CombatConfig.LootAllInventory) || _isPuppetCombat)
		{
			return;
		}
		GameData.Domains.Character.Character character = _selfChar.GetCharacter();
		if (!CombatConfig.LootAllInventory)
		{
			ItemKey[] equipment = character.GetEquipment();
			Personalities personalities = character.GetPersonalities();
			ItemKey itemKey = equipment[11];
			int num = 100 + personalities.Items[5];
			if (itemKey.IsValid())
			{
				num += DomainManager.Item.GetElement_Carriers(itemKey.Id).GetDropRateBonus();
			}
			for (sbyte b = 8; b <= 10; b++)
			{
				ItemKey itemKey2 = equipment[b];
				if (itemKey2.IsValid())
				{
					num += Config.Accessory.Instance[itemKey2.TemplateId].DropRateBonus;
				}
			}
			List<ItemKey> list = ObjectPool<List<ItemKey>>.Instance.Get();
			List<int> list2 = ObjectPool<List<int>>.Instance.Get();
			for (int i = 0; i < _enemyTeam.Length; i++)
			{
				int num2 = _enemyTeam[i];
				if (num2 < 0 || _lootCharList.Contains(num2))
				{
					continue;
				}
				GameData.Domains.Character.Character character2 = _combatCharacterDict[num2].GetCharacter();
				int num3 = ((i == 0) ? character2.Template.DropRatePercentAsMainChar : character2.Template.DropRatePercentAsTeammate);
				ItemKey[] equipment2 = character2.GetEquipment();
				Inventory inventory = character2.GetInventory();
				list.Clear();
				list2.Clear();
				for (sbyte b2 = 0; b2 < 12; b2++)
				{
					if (b2 != 4)
					{
						ItemKey item = equipment2[b2];
						if (item.IsValid() && ItemTemplateHelper.IsTransferable(item.ItemType, item.TemplateId))
						{
							int percentProb = ItemTemplateHelper.GetDropRate(item.ItemType, item.TemplateId) * num / 100 * lootItemRate / 100 * num3 / 100;
							if (context.Random.CheckPercentProb(percentProb))
							{
								list.Add(item);
								list2.Add(1);
							}
						}
					}
				}
				foreach (KeyValuePair<ItemKey, int> item2 in inventory.Items)
				{
					ItemKey key = item2.Key;
					if (ItemTemplateHelper.IsTransferable(key.ItemType, key.TemplateId))
					{
						int percentProb2 = ItemTemplateHelper.GetDropRate(key.ItemType, key.TemplateId) * num / 100 * lootItemRate / 100 * num3 / 100;
						if (context.Random.CheckPercentProb(percentProb2))
						{
							list.Add(key);
							list2.Add(item2.Value);
						}
					}
				}
				int num4 = Math.Min(list.Count, (i != 0) ? 1 : 3);
				for (int j = 0; j < num4; j++)
				{
					if (context.Random.CheckPercentProb(100 - 20 * j))
					{
						int index = context.Random.Next(list.Count);
						ItemKey key2 = list[index];
						int num5 = list2[index];
						sbyte b3 = (sbyte)equipment2.IndexOf(key2);
						list.RemoveAt(index);
						list2.RemoveAt(index);
						if (b3 >= 0)
						{
							character2.ChangeEquipment(context, b3, -1, ItemKey.Invalid);
						}
						int num6 = _combatResultData.ItemList.FindIndex((ItemDisplayData data) => data.Key.Equals(key2));
						if (num6 < 0)
						{
							ItemDisplayData itemDisplayData = DomainManager.Item.GetItemDisplayData(key2, _selfCharId);
							itemDisplayData.Amount = num5;
							_combatResultData.ItemList.Add(itemDisplayData);
							_combatResultData.ItemSrcCharDict[key2] = character2.GetId();
						}
						else
						{
							_combatResultData.ItemList[num6].Amount += num5;
							DomainManager.Character.TransferInventoryItem(context, character2, DomainManager.Character.GetElement_Objects(_combatResultData.ItemSrcCharDict[key2]), key2, num5);
						}
					}
				}
			}
			ObjectPool<List<ItemKey>>.Instance.Return(list);
			ObjectPool<List<int>>.Instance.Return(list2);
			return;
		}
		GameData.Domains.Character.Character character3 = _combatCharacterDict[_enemyTeam[0]].GetCharacter();
		Inventory inventory2 = character3.GetInventory();
		foreach (KeyValuePair<ItemKey, int> item3 in inventory2.Items)
		{
			ItemKey key3 = item3.Key;
			int num7 = _combatResultData.ItemList.FindIndex((ItemDisplayData data) => data.Key.Equals(key3));
			if (num7 < 0)
			{
				ItemDisplayData itemDisplayData2 = DomainManager.Item.GetItemDisplayData(key3, _selfCharId);
				itemDisplayData2.Amount = item3.Value;
				_combatResultData.ItemList.Add(itemDisplayData2);
				_combatResultData.ItemSrcCharDict[key3] = character3.GetId();
			}
			else
			{
				_combatResultData.ItemList[num7].Amount += item3.Value;
				DomainManager.Character.TransferInventoryItem(context, character3, DomainManager.Character.GetElement_Objects(_combatResultData.ItemSrcCharDict[key3]), key3, item3.Value);
			}
		}
	}

	private void GetLootCharDisplayData()
	{
		_combatResultData.CharList = DomainManager.Character.GetCharacterDisplayDataList(_lootCharList);
	}

	private void CalcSnapshotAfterCombat(DataContext context)
	{
		_combatResultData.SnapshotAfterCombat = CombatResultHelper.CreateSnapshot();
	}

	private static int SumAddValue(IEnumerable<int> addValues)
	{
		int num = 0;
		bool flag = true;
		foreach (int addValue in addValues)
		{
			int num2 = addValue;
			if (flag)
			{
				flag = false;
			}
			else
			{
				num2 = num2 * GlobalConfig.Instance.CombatGetNonMainPercent / 100;
			}
			num += num2;
		}
		return num;
	}

	public static int CalcAddBase(IEnumerable<int> charIds, IReadOnlyList<short> baseValues, bool useTemplate = false)
	{
		return SumAddValue(charIds.Select((int charId) => CalcAddBase(charId, baseValues, useTemplate)));
	}

	private static int CalcAddBase(int charId, IReadOnlyList<short> baseValues, bool useTemplate)
	{
		if (charId < 0)
		{
			return 0;
		}
		int consummateLevel;
		if (useTemplate)
		{
			CharacterItem characterItem = Config.Character.Instance[charId];
			if (characterItem.RandomEnemyId < 0)
			{
				return 0;
			}
			consummateLevel = Config.Character.Instance[charId].ConsummateLevel;
		}
		else
		{
			GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(charId);
			consummateLevel = element_Objects.GetConsummateLevel();
		}
		consummateLevel = Math.Clamp(consummateLevel, 0, baseValues.Count - 1);
		return baseValues[consummateLevel];
	}

	private static int CalcAddResource(IEnumerable<int> charIds, sbyte resourceType, bool useTemplate = false)
	{
		return SumAddValue(charIds.Select((int charId) => CalcAddResource(charId, resourceType, useTemplate)));
	}

	private static int CalcAddResource(int charId, sbyte resourceType, bool useTemplate)
	{
		if (charId < 0)
		{
			return 0;
		}
		if (useTemplate)
		{
			return Config.Character.Instance[charId].DropResources[resourceType];
		}
		GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(charId);
		CharacterItem characterItem = Config.Character.Instance[element_Objects.GetTemplateId()];
		if (characterItem.CreatingType != 1)
		{
			return characterItem.DropResources[resourceType];
		}
		return OrganizationDomain.GetOrgMemberConfig(element_Objects.GetOrganizationInfo()).DropResources[resourceType];
	}

	public void WipeOut(DataContext context, List<short> enemyList)
	{
		(WipeOutType, short) wipeOutType = GetWipeOutType(enemyList);
		WipeOutType item = wipeOutType.Item1;
		short item2 = wipeOutType.Item2;
		InstantNotificationCollection instantNotifications = DomainManager.World.GetInstantNotifications();
		switch (item)
		{
		case WipeOutType.Heretic:
			instantNotifications.AddExpelEnemy(item2);
			break;
		case WipeOutType.Righteous:
			instantNotifications.AddExpelRighteous(item2);
			DomainManager.Taiwu.GetTaiwu().RecordFameAction(context, 60, -1, 1);
			instantNotifications.AddFameDecreased(DomainManager.Taiwu.GetTaiwuCharId());
			break;
		case WipeOutType.Xiangshu:
			instantNotifications.AddExpelXiangshuMinion(item2);
			break;
		case WipeOutType.Beast:
			instantNotifications.AddExpelBeast(item2);
			break;
		}
		GetExpAndAuthorityAndAreaSpiritualDebtOutOfCombat(context, enemyList);
	}

	public bool CanWipeOut(short templateId)
	{
		if (templateId < 0)
		{
			return false;
		}
		GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
		if (TryGetWipeOutType(templateId, out var type))
		{
			if (type == WipeOutType.Righteous && taiwu.GetFameType() > 1)
			{
				return false;
			}
			if (type == WipeOutType.Beast && DomainManager.Extra.TryGetAnimalIdsByLocation(taiwu.GetLocation(), out var animals))
			{
				foreach (int item in animals)
				{
					if (DomainManager.Extra.TryGetAnimal(item, out var animal) && animal.CharacterTemplateId == templateId && animal.ItemKey.IsValid())
					{
						return false;
					}
				}
			}
		}
		return taiwu.GetConsummateLevel() >= Config.Character.Instance[templateId].ConsummateLevel + GlobalConfig.Instance.RandomEnemyEscapeConsummateLevelGap;
	}

	public bool TryGetWipeOutType(short templateId, out WipeOutType type)
	{
		type = WipeOutType.Invalid;
		if (templateId < 0)
		{
			return false;
		}
		switch (Config.Character.Instance[templateId].OrganizationInfo.OrgTemplateId)
		{
		case 17:
			type = WipeOutType.Heretic;
			return true;
		case 18:
			type = WipeOutType.Righteous;
			return true;
		case 19:
			type = WipeOutType.Xiangshu;
			return true;
		case 40:
			type = WipeOutType.Beast;
			return true;
		default:
			return false;
		}
	}

	private (WipeOutType, short) GetWipeOutType(List<short> enemyList)
	{
		foreach (short enemy in enemyList)
		{
			if (TryGetWipeOutType(enemy, out var type))
			{
				return (type, enemy);
			}
		}
		return (WipeOutType.Invalid, -1);
	}

	private int GetAddExp(int[] enemyList)
	{
		int num = 0;
		foreach (int num2 in enemyList)
		{
			if (num2 >= 0)
			{
				int num3 = Math.Clamp(DomainManager.Character.GetElement_Objects(num2).GetConsummateLevel(), 0, GlobalConfig.Instance.CombatGetExpBase.Length - 1);
				num += GlobalConfig.Instance.CombatGetExpBase[num3];
			}
		}
		return num;
	}

	private int GetAddExp(List<short> enemyList)
	{
		int num = 0;
		for (int i = 0; i < enemyList.Count; i++)
		{
			int num2 = enemyList[i];
			if (num2 >= 0)
			{
				CharacterItem characterItem = Config.Character.Instance[num2];
				short randomEnemyId = characterItem.RandomEnemyId;
				if (randomEnemyId >= 0)
				{
					int num3 = Math.Clamp(characterItem.ConsummateLevel, 0, GlobalConfig.Instance.CombatGetExpBase.Length - 1);
					num += GlobalConfig.Instance.CombatGetExpBase[num3];
				}
			}
		}
		return num;
	}

	private int GetAddAuthority(int[] enemyList)
	{
		int num = 0;
		foreach (int num2 in enemyList)
		{
			if (num2 >= 0)
			{
				int num3 = Math.Clamp(DomainManager.Character.GetElement_Objects(num2).GetConsummateLevel(), 0, GlobalConfig.Instance.CombatGetAuthorityBase.Length - 1);
				num += GlobalConfig.Instance.CombatGetAuthorityBase[num3];
			}
		}
		return num;
	}

	private int GetAddAuthority(List<short> enemyList)
	{
		int num = 0;
		for (int i = 0; i < enemyList.Count; i++)
		{
			int num2 = enemyList[i];
			if (num2 >= 0)
			{
				CharacterItem characterItem = Config.Character.Instance[num2];
				short randomEnemyId = characterItem.RandomEnemyId;
				if (randomEnemyId >= 0)
				{
					int num3 = Math.Clamp(characterItem.ConsummateLevel, 0, GlobalConfig.Instance.CombatGetAuthorityBase.Length - 1);
					num += GlobalConfig.Instance.CombatGetAuthorityBase[num3];
				}
			}
		}
		return num;
	}

	private int GetAddAreaSpiritualDebt(int[] enemyList)
	{
		int num = 0;
		foreach (int num2 in enemyList)
		{
			if (num2 >= 0 && !IsGuardChar(_combatCharacterDict[num2]))
			{
				short randomEnemyId = Config.Character.Instance[DomainManager.Character.GetElement_Objects(num2).GetTemplateId()].RandomEnemyId;
				if (randomEnemyId >= 0)
				{
					num += RandomEnemy.Instance[randomEnemyId].SpiritualDebt;
				}
			}
		}
		return num;
	}

	private int GetAddAreaSpiritualDebt(List<short> enemyList)
	{
		int num = 0;
		for (int i = 0; i < enemyList.Count; i++)
		{
			int num2 = enemyList[i];
			if (num2 >= 0)
			{
				short randomEnemyId = Config.Character.Instance[num2].RandomEnemyId;
				if (randomEnemyId >= 0)
				{
					num += RandomEnemy.Instance[randomEnemyId].SpiritualDebt;
				}
			}
		}
		return num;
	}

	public void GetExpAndAuthorityAndAreaSpiritualDebtOutOfCombat(DataContext context, List<int> enemyIdList)
	{
		GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
		Location location = taiwu.GetLocation();
		if (!location.IsValid())
		{
			location = taiwu.GetValidLocation();
		}
		short areaId = location.AreaId;
		int[] enemyList = enemyIdList.ToArray();
		taiwu.ChangeExp(context, GetAddExp(enemyList));
		taiwu.ChangeResource(context, 7, GetAddAuthority(enemyList));
		DomainManager.Extra.ChangeAreaSpiritualDebt(context, areaId, GetAddAreaSpiritualDebt(enemyList));
	}

	public void GetExpAndAuthorityAndAreaSpiritualDebtOutOfCombat(DataContext context, List<short> enemyTemplateIdList, int rewardTimes = 1)
	{
		GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
		Location location = taiwu.GetLocation();
		if (!location.IsValid())
		{
			location = taiwu.GetValidLocation();
		}
		short areaId = location.AreaId;
		int num = GetAddExp(enemyTemplateIdList) * rewardTimes;
		int num2 = GetAddAuthority(enemyTemplateIdList) * rewardTimes;
		int num3 = GetAddAreaSpiritualDebt(enemyTemplateIdList) * rewardTimes;
		InstantNotificationCollection instantNotifications = DomainManager.World.GetInstantNotifications();
		taiwu.ChangeExp(context, num);
		taiwu.ChangeResource(context, 7, num2);
		DomainManager.Extra.ChangeAreaSpiritualDebt(context, areaId, num3, getProfessionSeniority: true, addInstantNotification: false);
		if (num > 0)
		{
			instantNotifications.AddExpIncreased(taiwu.GetId(), num);
		}
		if (num2 > 0)
		{
			instantNotifications.AddResourceIncreased(taiwu.GetId(), 7, num2);
		}
		if (num3 > 0)
		{
			instantNotifications.AddGraceUp(new Location(areaId, -1), num3);
		}
		else if (num3 < 0)
		{
			instantNotifications.AddGraceDown(new Location(areaId, -1), -num3);
		}
	}

	private static bool FailChecker()
	{
		return DomainManager.Combat.GetCombatStatus() == CombatStatusType.SelfFail;
	}

	private static bool DrawChecker()
	{
		return false;
	}

	private static bool FleeChecker()
	{
		return DomainManager.Combat.GetCombatStatus() == CombatStatusType.SelfFlee;
	}

	private static bool WinChecker()
	{
		return DomainManager.Combat.IsWin(isAlly: true);
	}

	private static bool FightSameLevelChecker()
	{
		CombatDomain combat = DomainManager.Combat;
		return combat._selfChar.GetCharacter().GetConsummateLevel() <= combat._enemyChar.GetCharacter().GetConsummateLevel();
	}

	private static bool FightLessLevelChecker()
	{
		CombatDomain combat = DomainManager.Combat;
		return combat._selfChar.GetCharacter().GetConsummateLevel() > combat._enemyChar.GetCharacter().GetConsummateLevel();
	}

	private static bool BeatXiangShuChecker()
	{
		return DomainManager.Combat.GetMainCharacter(isAlly: false).BossConfig?.TemplateId < 9 && DomainManager.Combat.CombatConfig.Scene >= 0 && !DomainManager.Combat._isPuppetCombat;
	}

	private static bool WinLessChecker()
	{
		int[] characterList = DomainManager.Combat.GetCharacterList(isAlly: true);
		int[] characterList2 = DomainManager.Combat.GetCharacterList(isAlly: false);
		return characterList.FindAll((int id) => id >= 0).Count > characterList2.FindAll((int id) => id >= 0).Count;
	}

	private static bool WinMoreChecker()
	{
		int[] characterList = DomainManager.Combat.GetCharacterList(isAlly: true);
		int[] characterList2 = DomainManager.Combat.GetCharacterList(isAlly: false);
		return characterList.FindAll((int id) => id >= 0).Count < characterList2.FindAll((int id) => id >= 0).Count;
	}

	private static bool WinChildChecker()
	{
		short currAge = DomainManager.Combat.GetMainCharacter(isAlly: true).GetCharacter().GetCurrAge();
		short currAge2 = DomainManager.Combat.GetMainCharacter(isAlly: false).GetCharacter().GetCurrAge();
		return currAge2 < 16 && currAge > currAge2;
	}

	private static bool WinOlderChecker()
	{
		short currAge = DomainManager.Combat.GetMainCharacter(isAlly: true).GetCharacter().GetCurrAge();
		short currAge2 = DomainManager.Combat.GetMainCharacter(isAlly: false).GetCharacter().GetCurrAge();
		return currAge < 16 && currAge < currAge2;
	}

	private static bool WinWorseEquipChecker()
	{
		return DomainManager.Combat.SelfAvgEquipGrade - DomainManager.Combat.EnemyAvgEquipGrade >= 3f;
	}

	private static bool WinBetterEquipChecker()
	{
		return DomainManager.Combat.EnemyAvgEquipGrade - DomainManager.Combat.SelfAvgEquipGrade >= 3f;
	}

	private static bool WinLessNeiliChecker()
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		int maxOriginNeiliAllocationSum = DomainManager.Combat.GetMaxOriginNeiliAllocationSum(isAlly: true);
		int maxOriginNeiliAllocationSum2 = DomainManager.Combat.GetMaxOriginNeiliAllocationSum(isAlly: false);
		return maxOriginNeiliAllocationSum >= maxOriginNeiliAllocationSum2 * WinNeiliMinDelta;
	}

	private static bool WinMoreNeiliChecker()
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		int maxOriginNeiliAllocationSum = DomainManager.Combat.GetMaxOriginNeiliAllocationSum(isAlly: true);
		int maxOriginNeiliAllocationSum2 = DomainManager.Combat.GetMaxOriginNeiliAllocationSum(isAlly: false);
		return maxOriginNeiliAllocationSum2 >= maxOriginNeiliAllocationSum * WinNeiliMinDelta;
	}

	private static bool WinWorseSkillChecker()
	{
		return DomainManager.Combat.SelfMaxSkillGrade > DomainManager.Combat.EnemyMaxSkillGrade;
	}

	private static bool WinBetterSkillChecker()
	{
		return DomainManager.Combat.SelfMaxSkillGrade < DomainManager.Combat.EnemyMaxSkillGrade;
	}

	private static bool WinLessConsummateChecker()
	{
		return DomainManager.Combat.GetMainCharacter(isAlly: true).GetCharacter().GetConsummateLevel() - DomainManager.Combat.GetMainCharacter(isAlly: false).GetCharacter().GetConsummateLevel() >= 3;
	}

	private static bool WinMoreConsummateChecker()
	{
		return DomainManager.Combat.GetMainCharacter(isAlly: false).GetCharacter().GetConsummateLevel() - DomainManager.Combat.GetMainCharacter(isAlly: true).GetCharacter().GetConsummateLevel() >= 3;
	}

	private static bool WinPregnantChecker()
	{
		return DomainManager.Combat.GetMainCharacter(isAlly: false).GetCharacter().GetFeatureIds()
			.Contains(197);
	}

	private static bool WinInPregnantChecker()
	{
		return DomainManager.Combat.GetMainCharacter(isAlly: true).GetCharacter().GetFeatureIds()
			.Contains(197);
	}

	private static bool KillBad0Checker()
	{
		CombatCharacter mainCharacter = DomainManager.Combat.GetMainCharacter(isAlly: true);
		CombatCharacter mainCharacter2 = DomainManager.Combat.GetMainCharacter(isAlly: false);
		return !DomainManager.Combat.IsGuardChar(mainCharacter2) && mainCharacter2.GetCharacter().GetOrganizationInfo().OrgTemplateId == 17 && mainCharacter.GetCharacter().GetConsummateLevel() > mainCharacter2.GetCharacter().GetConsummateLevel();
	}

	private static bool KillBad1Checker()
	{
		CombatCharacter mainCharacter = DomainManager.Combat.GetMainCharacter(isAlly: true);
		CombatCharacter mainCharacter2 = DomainManager.Combat.GetMainCharacter(isAlly: false);
		return !DomainManager.Combat.IsGuardChar(mainCharacter2) && mainCharacter2.GetCharacter().GetOrganizationInfo().OrgTemplateId == 17 && mainCharacter.GetCharacter().GetConsummateLevel() <= mainCharacter2.GetCharacter().GetConsummateLevel();
	}

	private static bool KillGood0Checker()
	{
		CombatCharacter mainCharacter = DomainManager.Combat.GetMainCharacter(isAlly: true);
		CombatCharacter mainCharacter2 = DomainManager.Combat.GetMainCharacter(isAlly: false);
		return !DomainManager.Combat.IsGuardChar(mainCharacter2) && mainCharacter2.GetCharacter().GetOrganizationInfo().OrgTemplateId == 18 && mainCharacter.GetCharacter().GetConsummateLevel() > mainCharacter2.GetCharacter().GetConsummateLevel();
	}

	private static bool KillGood1Checker()
	{
		CombatCharacter mainCharacter = DomainManager.Combat.GetMainCharacter(isAlly: true);
		CombatCharacter mainCharacter2 = DomainManager.Combat.GetMainCharacter(isAlly: false);
		return !DomainManager.Combat.IsGuardChar(mainCharacter2) && mainCharacter2.GetCharacter().GetOrganizationInfo().OrgTemplateId == 18 && mainCharacter.GetCharacter().GetConsummateLevel() <= mainCharacter2.GetCharacter().GetConsummateLevel();
	}

	private static bool KillMinion0Checker()
	{
		CombatCharacter mainCharacter = DomainManager.Combat.GetMainCharacter(isAlly: true);
		CombatCharacter mainCharacter2 = DomainManager.Combat.GetMainCharacter(isAlly: false);
		return !DomainManager.Combat.IsGuardChar(mainCharacter2) && mainCharacter2.GetCharacter().GetOrganizationInfo().OrgTemplateId == 19 && mainCharacter.GetCharacter().GetConsummateLevel() > mainCharacter2.GetCharacter().GetConsummateLevel();
	}

	private static bool KillMinion1Checker()
	{
		CombatCharacter mainCharacter = DomainManager.Combat.GetMainCharacter(isAlly: true);
		CombatCharacter mainCharacter2 = DomainManager.Combat.GetMainCharacter(isAlly: false);
		return !DomainManager.Combat.IsGuardChar(mainCharacter2) && mainCharacter2.GetCharacter().GetOrganizationInfo().OrgTemplateId == 19 && mainCharacter.GetCharacter().GetConsummateLevel() <= mainCharacter2.GetCharacter().GetConsummateLevel();
	}

	private static bool ShixiangBuff0Checker()
	{
		return DomainManager.Combat.GetMainCharacter(isAlly: true).GetCharacter().GetFeatureIds()
			.Contains(243);
	}

	private static bool ShixiangBuff1Checker()
	{
		return DomainManager.Combat.GetMainCharacter(isAlly: true).GetCharacter().GetFeatureIds()
			.Contains(244);
	}

	private static bool ShixiangBuff2Checker()
	{
		return DomainManager.Combat.GetMainCharacter(isAlly: true).GetCharacter().GetFeatureIds()
			.Contains(245);
	}

	private static bool PuppetCombatChecker()
	{
		return DomainManager.Combat._isPuppetCombat;
	}

	private static bool OutBossCombatChecker()
	{
		return DomainManager.Combat.CombatConfig.IsOutBoss;
	}

	private static bool WinLoongChecker()
	{
		short templateId = DomainManager.Combat.CombatConfig.TemplateId;
		if ((uint)(templateId - 182) <= 4u)
		{
			return true;
		}
		return false;
	}

	private static bool CombatHardChecker()
	{
		return DomainManager.World.GetCombatDifficulty() == 2;
	}

	private static bool CombatVeryHardChecker()
	{
		return DomainManager.World.GetCombatDifficulty() == 3;
	}

	public void AddCombatResultLegacy(short legacy)
	{
		CombatResultDisplayData combatResultData = _combatResultData;
		if (combatResultData.LegacyTemplateIds == null)
		{
			combatResultData.LegacyTemplateIds = new List<short>();
		}
		_combatResultData.LegacyTemplateIds.Add(legacy);
	}

	private void ClearCombatResultLegacies()
	{
		_combatResultData.LegacyTemplateIds?.Clear();
	}

	[DomainMethod]
	public void ApplyCombatResultDataEffect(DataContext context, CombatResultDisplayData combatResultData, List<ItemDisplayData> selectedLootItem)
	{
		GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
		taiwu.ChangeExp(context, combatResultData.Exp);
		Events.RaiseEvaluationAddExp(context, combatResultData.Exp);
		for (sbyte b = 0; b < 8; b++)
		{
			taiwu.ChangeResource(context, b, combatResultData.Resource.Get(b));
		}
		int areaSpiritualDebt = combatResultData.AreaSpiritualDebt;
		if (areaSpiritualDebt != 0)
		{
			Location location = taiwu.GetLocation();
			if (!location.IsValid())
			{
				location = taiwu.GetValidLocation();
			}
			DomainManager.Extra.ChangeAreaSpiritualDebt(context, location.AreaId, areaSpiritualDebt);
		}
		ItemDomain item = DomainManager.Item;
		HashSet<ItemDisplayData> hashSet = new HashSet<ItemDisplayData>();
		if (selectedLootItem != null)
		{
			hashSet.UnionWith(selectedLootItem);
		}
		foreach (ItemDisplayData item2 in combatResultData.ItemList)
		{
			if (hashSet.Any((ItemDisplayData selectedItem) => selectedItem.ContainsItemKey(item2.Key)))
			{
				taiwu.AddInventoryItem(context, item2.Key, item2.Amount);
			}
			else
			{
				item.RemoveItem(context, item2.Key);
			}
		}
	}

	internal static void ResultCalcExp(CombatConfigItem combatConfig, bool isPlaygroundCombat, GameData.Domains.Character.Character selfChar, ICollection<int> enemyTeam, CombatResultDisplayData combatResultData)
	{
		if (!(!combatConfig.DropResource || isPlaygroundCombat))
		{
			int baseValue = CalcAddBase(enemyTeam, GlobalConfig.Instance.CombatGetExpBase);
			int index = ((!selfChar.IsLoseConsummateBonusByFeature()) ? selfChar.GetConsummateLevel() : 0);
			int expBonus = ConsummateLevel.Instance[index].ExpBonus;
			baseValue = combatResultData.ModifyValue(baseValue, (CombatEvaluationItem x) => x.ExpAddPercent, (CombatEvaluationItem x) => x.ExpTotalPercent, expBonus);
			baseValue = Math.Max(baseValue, 0);
			combatResultData.Exp += baseValue;
		}
	}

	internal static void ResultCalcResource(CombatConfigItem combatConfig, bool isPlaygroundCombat, GameData.Domains.Character.Character selfChar, ICollection<int> enemyTeam, CombatResultDisplayData combatResultData)
	{
		if (!combatConfig.DropResource || isPlaygroundCombat)
		{
			return;
		}
		for (sbyte b = 0; b < 8; b++)
		{
			int num = CalcAddResource(enemyTeam, b);
			if (b == 7)
			{
				num += CalcAddBase(enemyTeam, GlobalConfig.Instance.CombatGetAuthorityBase);
			}
			num = num * DomainManager.World.GetGainResourcePercent(8) / 100;
			if (b == 7)
			{
				num = combatResultData.ModifyValue(num, (CombatEvaluationItem x) => x.AuthorityAddPercent, (CombatEvaluationItem x) => x.AuthorityTotalPercent);
			}
			else if (!combatResultData.IsWin)
			{
				num = 0;
			}
			num = Math.Max(num, 0);
			combatResultData.Resource[b] += num;
		}
	}

	internal static void ResultCalcAreaSpiritualDebt(bool isWin, GameData.Domains.Character.Character selfChar, int[] enemyTeam, CombatResultDisplayData combatResultData)
	{
		int num = 0;
		num += combatResultData.SelectEvaluations((CombatEvaluationItem x) => x.AreaSpiritualDebt).Sum((short x) => x);
		if (isWin)
		{
			num += CalcAddAreaSpiritualDebt();
		}
		combatResultData.AreaSpiritualDebt += num;
		int CalcAddAreaSpiritualDebt()
		{
			int num2 = 0;
			foreach (int num3 in enemyTeam)
			{
				if (num3 >= 0 && !IsGuardChar(num3))
				{
					short randomEnemyId = Config.Character.Instance[DomainManager.Character.GetElement_Objects(num3).GetTemplateId()].RandomEnemyId;
					if (randomEnemyId >= 0)
					{
						num2 += RandomEnemy.Instance[randomEnemyId].SpiritualDebt;
					}
				}
			}
			return num2;
		}
		bool IsGuardChar(int chId)
		{
			GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(chId);
			if (element_Objects.GetCreatingType() != 2)
			{
				return false;
			}
			foreach (int num2 in enemyTeam)
			{
				if (num2 >= 0 && DomainManager.Character.GetElement_Objects(num2).GetCreatingType() == 1)
				{
					return true;
				}
			}
			return false;
		}
	}

	internal unsafe static void ResultCalcLootItem(IRandomSource random, int lootRatePercentFactor, sbyte combatType, sbyte combatStatus, bool isPuppetCombat, CombatConfigItem combatConfig, GameData.Domains.Character.Character charObj, int[] enemyTeam, ICollection<int> lootCharList, CombatResultDisplayData combatResultData)
	{
		int lootItemRate = combatConfig.LootItemRate;
		lootItemRate = lootItemRate * GetWorldLootRatePercent() / 100;
		lootItemRate = lootItemRate * lootRatePercentFactor / 100;
		bool flag = ((combatType == 0 || combatType == 3) ? true : false);
		if (flag || !combatConfig.AllowDropItem || combatStatus != CombatStatusType.EnemyFail || (lootItemRate <= 0 && !combatConfig.LootAllInventory) || isPuppetCombat)
		{
			return;
		}
		if (!combatConfig.LootAllInventory)
		{
			ItemKey[] equipment = charObj.GetEquipment();
			Personalities personalities = charObj.GetPersonalities();
			ItemKey itemKey = equipment[11];
			int num = 100 + personalities.Items[5];
			if (itemKey.IsValid())
			{
				num += DomainManager.Item.GetElement_Carriers(itemKey.Id).GetDropRateBonus();
			}
			for (sbyte b = 8; b <= 10; b++)
			{
				ItemKey itemKey2 = equipment[b];
				if (itemKey2.IsValid())
				{
					num += Config.Accessory.Instance[itemKey2.TemplateId].DropRateBonus;
				}
			}
			List<ItemKey> list = ObjectPool<List<ItemKey>>.Instance.Get();
			List<int> list2 = ObjectPool<List<int>>.Instance.Get();
			for (int i = 0; i < enemyTeam.Length; i++)
			{
				int num2 = enemyTeam[i];
				if (num2 < 0 || lootCharList.Contains(num2))
				{
					continue;
				}
				int num3 = ((i == 0) ? 100 : 25);
				GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(num2);
				ItemKey[] equipment2 = element_Objects.GetEquipment();
				Inventory inventory = element_Objects.GetInventory();
				list.Clear();
				list2.Clear();
				for (sbyte b2 = 0; b2 < 12; b2++)
				{
					if (b2 != 4)
					{
						ItemKey item = equipment2[b2];
						if (item.IsValid() && ItemTemplateHelper.IsTransferable(item.ItemType, item.TemplateId))
						{
							int percentProb = ItemTemplateHelper.GetDropRate(item.ItemType, item.TemplateId) * num / 100 * lootItemRate / 100 * num3 / 100;
							if (random.CheckPercentProb(percentProb))
							{
								list.Add(item);
								list2.Add(1);
							}
						}
					}
				}
				foreach (KeyValuePair<ItemKey, int> item3 in inventory.Items)
				{
					ItemKey key = item3.Key;
					if (ItemTemplateHelper.IsTransferable(key.ItemType, key.TemplateId))
					{
						int percentProb2 = ItemTemplateHelper.GetDropRate(key.ItemType, key.TemplateId) * num / 100 * lootItemRate / 100 * num3 / 100;
						if (random.CheckPercentProb(percentProb2))
						{
							list.Add(key);
							list2.Add(item3.Value);
						}
					}
				}
				List<ItemKey> list3 = new List<ItemKey>();
				int num4 = Math.Min(list.Count, (i != 0) ? 1 : 3);
				for (int j = 0; j < num4; j++)
				{
					if (random.CheckPercentProb(100 - 20 * j))
					{
						int index = random.Next(list.Count);
						ItemKey item2 = list[index];
						int num5 = list2[index];
						list.RemoveAt(index);
						list2.RemoveAt(index);
						list3.Add(item2);
					}
				}
				List<ItemDisplayData> itemDisplayDataListOptional = DomainManager.Item.GetItemDisplayDataListOptional(list3, charObj.GetId(), 1, merge: true);
				if (itemDisplayDataListOptional != null)
				{
					combatResultData.ItemList.AddRange(itemDisplayDataListOptional);
				}
				foreach (ItemKey item4 in list3)
				{
					combatResultData.ItemSrcCharDict[item4] = element_Objects.GetId();
				}
			}
			ObjectPool<List<ItemKey>>.Instance.Return(list);
			ObjectPool<List<int>>.Instance.Return(list2);
			return;
		}
		GameData.Domains.Character.Character element_Objects2 = DomainManager.Character.GetElement_Objects(enemyTeam[0]);
		Inventory inventory2 = element_Objects2.GetInventory();
		List<ItemKey> list4 = inventory2.Items.Select((KeyValuePair<ItemKey, int> itemEntry) => itemEntry.Key).ToList();
		List<ItemDisplayData> itemDisplayDataListOptional2 = DomainManager.Item.GetItemDisplayDataListOptional(list4, charObj.GetId(), 1, merge: true);
		if (itemDisplayDataListOptional2 != null)
		{
			combatResultData.ItemList.AddRange(itemDisplayDataListOptional2);
		}
		foreach (ItemKey item5 in list4)
		{
			combatResultData.ItemSrcCharDict[item5] = element_Objects2.GetId();
		}
	}

	public static void RegisterHandler_CombatCharAboutToFall(OnCombatCharAboutToFall handler)
	{
		_handlersCombatCharAboutToFall = (OnCombatCharAboutToFall)Delegate.Combine(_handlersCombatCharAboutToFall, handler);
	}

	public static void UnRegisterHandler_CombatCharAboutToFall(OnCombatCharAboutToFall handler)
	{
		_handlersCombatCharAboutToFall = (OnCombatCharAboutToFall)Delegate.Remove(_handlersCombatCharAboutToFall, handler);
	}

	public static void RaiseCombatCharAboutToFall(DataContext context, CombatCharacter combatChar, int eventIndex)
	{
		_handlersCombatCharAboutToFall?.Invoke(context, combatChar, eventIndex);
	}

	[DomainMethod]
	public void GmCmd_ForceRecoverBreathAndStance(DataContext context)
	{
		ChangeBreathValue(context, _selfChar, _selfChar.GetMaxBreathValue());
		ChangeStanceValue(context, _selfChar, _selfChar.GetMaxStanceValue());
		UpdateSkillCostBreathStanceCanUse(context, _selfChar);
		GmCmd_ForceRecoverMobilityValue(context);
		GmCmd_ForceRecoverTeammateCommand(context);
	}

	[DomainMethod]
	public void GmCmd_ForceRecoverTeammateCommand(DataContext context)
	{
		CombatCharacter mainCharacter = GetMainCharacter(isAlly: true);
		foreach (CombatCharacter teammateCharacter in GetTeammateCharacters(mainCharacter.GetId()))
		{
			List<sbyte> currTeammateCommands = teammateCharacter.GetCurrTeammateCommands();
			for (int i = 0; i < currTeammateCommands.Count; i++)
			{
				teammateCharacter.ClearTeammateCommandCd(context, i);
			}
		}
	}

	[DomainMethod]
	public void GmCmd_AddTrick(DataContext context, bool isAlly, sbyte trickType)
	{
		AddTrick(context, GetCombatCharacter(isAlly), trickType);
	}

	[DomainMethod]
	public void GmCmd_AddInjury(DataContext context, bool isAlly, sbyte bodyPart, bool isInner, int count = 1, bool changeToOld = false)
	{
		CombatCharacter combatCharacter = (isAlly ? _selfChar : _enemyChar);
		foreach (sbyte item in CRandom.IterBodyPart(bodyPart))
		{
			AddInjury(context, combatCharacter, item, isInner, (sbyte)count, updateDefeatMark: true, changeToOld);
		}
		AddToCheckFallenSet(combatCharacter.GetId());
		if (IsCharacterFallen(combatCharacter))
		{
			_skipCombatLoop = true;
			GetMainCharacter(isAlly).SkipOnFrameBegin = true;
		}
	}

	[DomainMethod]
	public void GmCmd_ForceHealAllInjury(DataContext context, bool isAlly = true)
	{
		CombatCharacter combatCharacter = (isAlly ? _selfChar : _enemyChar);
		Injuries injuries = combatCharacter.GetInjuries();
		injuries.Initialize();
		SetInjuries(context, combatCharacter, injuries);
	}

	[DomainMethod]
	public void GmCmd_HealInjury(DataContext context, bool isAlly, bool isInner)
	{
		CombatCharacter combatCharacter = (isAlly ? _selfChar : _enemyChar);
		Injuries injuries = combatCharacter.GetInjuries();
		for (sbyte b = 0; b < 7; b++)
		{
			injuries.Change(b, isInner, -6);
		}
		SetInjuries(context, combatCharacter, injuries);
	}

	[DomainMethod]
	public void GmCmd_AddPoison(DataContext context, bool isAlly, sbyte poisonType, int count = 1, bool changeToOld = false)
	{
		CombatCharacter combatCharacter = (isAlly ? _selfChar : _enemyChar);
		short[] poisonLevelThresholds = GlobalConfig.Instance.PoisonLevelThresholds;
		int addValue = poisonLevelThresholds[Math.Clamp(count - 1, 0, poisonLevelThresholds.Length - 1)] * (1 + (count - 1) / 3);
		foreach (sbyte item in CRandom.IterPoisonType(poisonType))
		{
			bool forceChangeToOld = changeToOld;
			AddPoison(context, null, combatCharacter, item, 3, addValue, -1, applySpecialEffect: false, canBounce: true, default(ItemKey), isDirectPoison: false, ignorePositiveResist: false, forceChangeToOld);
		}
		AddToCheckFallenSet(combatCharacter.GetId());
		if (IsCharacterFallen(combatCharacter))
		{
			_skipCombatLoop = true;
			GetMainCharacter(isAlly).SkipOnFrameBegin = true;
		}
	}

	[DomainMethod]
	public unsafe void GmCmd_ForceHealAllPoison(DataContext context, bool isAlly = true)
	{
		CombatCharacter combatCharacter = (isAlly ? _selfChar : _enemyChar);
		PoisonInts poison = combatCharacter.GetPoison();
		for (int i = 0; i < 6; i++)
		{
			poison.Items[i] = 0;
		}
		SetPoisons(context, combatCharacter, poison);
	}

	[DomainMethod]
	public void GmCmd_ForceEnemyUseSkill(DataContext context, short skillId)
	{
		if (_skillDataDict.ContainsKey(new CombatSkillKey(_enemyChar.GetId(), skillId)) && _enemyChar.NeedUseSkillId < 0 && _enemyChar.GetPreparingSkillId() < 0)
		{
			CastSkillFree(context, _enemyChar, skillId, ECombatCastFreePriority.Gm);
			_enemyChar.MoveData.ResetJumpState(context);
			UpdateAllCommandAvailability(context, _enemyChar);
		}
	}

	[DomainMethod]
	public void GmCmd_ForceEnemyUseOtherAction(DataContext context, sbyte actionType)
	{
		switch (actionType)
		{
		case 0:
			_enemyChar.SetHealInjuryCount((byte)(_enemyChar.GetHealInjuryCount() + 1), context);
			break;
		case 1:
			_enemyChar.SetHealPoisonCount((byte)(_enemyChar.GetHealPoisonCount() + 1), context);
			break;
		}
		_enemyChar.SetNeedUseOtherAction(context, actionType);
		_enemyChar.MoveData.ResetJumpState(context);
		UpdateAllCommandAvailability(context, _enemyChar);
	}

	[DomainMethod]
	public void GmCmd_ForceEnemyDefeat(DataContext context)
	{
		if (IsInCombat())
		{
			CombatCharacter combatCharacter = GetCombatCharacter(isAlly: false, tryGetCoverCharacter: true);
			if (combatCharacter.StateMachine.GetCurrentStateType() != CombatCharacterStateType.ChangeBossPhase && !combatCharacter.NeedChangeBossPhase)
			{
				AppendMindDefeatMark(context, combatCharacter, GlobalConfig.NeedDefeatMarkCount[2], -1);
			}
		}
	}

	[DomainMethod]
	public void GmCmd_ForceSelfDefeat(DataContext context)
	{
		if (IsInCombat())
		{
			CombatCharacter combatCharacter = ((_selfChar.TeammateBeforeMainChar >= 0) ? GetElement_CombatCharacterDict(_selfChar.TeammateBeforeMainChar) : _selfChar);
			AppendMindDefeatMark(context, combatCharacter, GlobalConfig.NeedDefeatMarkCount[2], -1);
			AddToCheckFallenSet(combatCharacter.GetId());
			if (DomainManager.Combat.IsCharacterFallen(combatCharacter))
			{
				_skipCombatLoop = true;
				GetMainCharacter(isAlly: true).SkipOnFrameBegin = true;
				SetTimeScale(1f, context);
			}
		}
	}

	[DomainMethod]
	public unsafe void GmCmd_SetNeiliAllocation(DataContext context, bool isAlly, short[] neiliAllocation)
	{
		CombatCharacter combatCharacter = (isAlly ? _selfChar : _enemyChar);
		NeiliAllocation neiliAllocation2 = combatCharacter.GetNeiliAllocation();
		for (byte b = 0; b < 4; b++)
		{
			combatCharacter.ChangeNeiliAllocation(context, b, neiliAllocation[b] - neiliAllocation2.Items[(int)b], applySpecialEffect: false);
		}
	}

	[DomainMethod]
	public void GmCmd_AddFlaw(DataContext context, bool isAlly, sbyte bodyPart, int count = 1)
	{
		CombatCharacter character = (isAlly ? _selfChar : _enemyChar);
		foreach (sbyte item in CRandom.IterBodyPart(bodyPart))
		{
			AddFlaw(context, character, 3, new CombatSkillKey(-1, -1), item, count);
		}
		if (IsCharacterFallen(character))
		{
			_skipCombatLoop = true;
			GetMainCharacter(isAlly).SkipOnFrameBegin = true;
		}
	}

	[DomainMethod]
	public void GmCmd_HealAllFlaw(DataContext context, bool isAlly)
	{
		RemoveAllFlaw(context, isAlly ? _selfChar : _enemyChar);
	}

	[DomainMethod]
	public void GmCmd_AddAcupoint(DataContext context, bool isAlly, sbyte bodyPart, int count = 1)
	{
		CombatCharacter character = (isAlly ? _selfChar : _enemyChar);
		foreach (sbyte item in CRandom.IterBodyPart(bodyPart))
		{
			AddAcupoint(context, character, 3, new CombatSkillKey(-1, -1), item, count);
		}
		if (IsCharacterFallen(character))
		{
			_skipCombatLoop = true;
			GetMainCharacter(isAlly).SkipOnFrameBegin = true;
		}
	}

	[DomainMethod]
	public void GmCmd_HealAllAcupoint(DataContext context, bool isAlly)
	{
		RemoveAllAcupoint(context, isAlly ? _selfChar : _enemyChar);
	}

	[DomainMethod]
	public void GmCmd_AddMind(DataContext context, bool isAlly, int count)
	{
		AppendMindDefeatMark(context, isAlly ? _selfChar : _enemyChar, count, -1);
	}

	[DomainMethod]
	public void GmCmd_HealAllMind(DataContext context, bool isAlly)
	{
		RemoveMindDefeatMark(context, isAlly ? _selfChar : _enemyChar, int.MaxValue, random: false);
	}

	[DomainMethod]
	public void GmCmd_AddDie(DataContext context, bool isAlly, int count)
	{
		AppendDieDefeatMark(context, isAlly ? _selfChar : _enemyChar, new CombatSkillKey(-1, -1), count);
	}

	[DomainMethod]
	public void GmCmd_HealAllDie(DataContext context, bool isAlly)
	{
		CombatCharacter combatCharacter = (isAlly ? _selfChar : _enemyChar);
		combatCharacter.GetDefeatMarkCollection().DieMarkList.Clear();
		combatCharacter.SetDefeatMarkCollection(combatCharacter.GetDefeatMarkCollection(), context);
	}

	[DomainMethod]
	public void GmCmd_AddFatal(DataContext context, bool isAlly, int count)
	{
		CombatCharacter character = (isAlly ? _selfChar : _enemyChar);
		AppendFatalDamageMark(context, character, count, -1, -1);
	}

	[DomainMethod]
	public void GmCmd_HealAllFatal(DataContext context, bool isAlly)
	{
		CombatCharacter combatCharacter = (isAlly ? _selfChar : _enemyChar);
		combatCharacter.GetDefeatMarkCollection().FatalDamageMarkCount = 0;
		combatCharacter.SetDefeatMarkCollection(combatCharacter.GetDefeatMarkCollection(), context);
	}

	[DomainMethod]
	public void GmCmd_AddAllDefeatMark(DataContext context, bool isAlly, int count = 1)
	{
		for (sbyte b = 0; b < 7; b++)
		{
			GmCmd_AddInjury(context, isAlly, b, isInner: true, count);
			GmCmd_AddInjury(context, isAlly, b, isInner: false, count);
			GmCmd_AddFlaw(context, isAlly, b, count);
			GmCmd_AddAcupoint(context, isAlly, b, count);
		}
		for (sbyte b2 = 0; b2 < 6; b2++)
		{
			GmCmd_AddPoison(context, isAlly, b2, count);
		}
		GmCmd_AddDie(context, isAlly, count);
		GmCmd_AddMind(context, isAlly, count);
		GmCmd_AddFatal(context, isAlly, count);
	}

	[DomainMethod]
	public void GmCmd_HealAllDefeatMark(DataContext context, bool isAlly)
	{
		GmCmd_ForceHealAllInjury(context, isAlly);
		GmCmd_ForceHealAllPoison(context, isAlly);
		GmCmd_HealAllFlaw(context, isAlly);
		GmCmd_HealAllAcupoint(context, isAlly);
		GmCmd_HealAllDie(context, isAlly);
		GmCmd_HealAllMind(context, isAlly);
		GmCmd_HealAllFatal(context, isAlly);
	}

	[DomainMethod]
	public void GmCmd_FightBoss(DataContext context, short charTemplateId)
	{
		if (Config.Character.Instance[charTemplateId].XiangshuType == 3)
		{
			sbyte xiangshuAvatarIdByCharacterTemplateId = XiangshuAvatarIds.GetXiangshuAvatarIdByCharacterTemplateId(charTemplateId);
			int num = DomainManager.Character.CreateJuniorXiangshuCombatImage(context, xiangshuAvatarIdByCharacterTemplateId);
			GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(num);
			CombatEntry(context, new List<int> { num }, (short)(Boss.Instance[CharId2BossId[element_Objects.GetTemplateId()]].CombatConfig + 9));
			return;
		}
		if (DomainManager.Character.TryGetFixedCharacterByTemplateId(charTemplateId, out var character))
		{
			DomainManager.Character.RemoveNonIntelligentCharacter(context, character);
		}
		GameData.Domains.Character.Character character2 = DomainManager.Character.CreateFixedCharacter(context, charTemplateId);
		DomainManager.Character.CompleteCreatingCharacter(character2.GetId());
		CombatEntry(context, new List<int> { character2.GetId() }, Boss.Instance[CharId2BossId[charTemplateId]].CombatConfig);
	}

	[DomainMethod]
	public void GmCmd_FightAnimal(DataContext context, short charTemplateId)
	{
		GameData.Domains.Character.Character character;
		switch (Config.Character.Instance[charTemplateId].CreatingType)
		{
		default:
			return;
		case 0:
			character = DomainManager.Character.GetOrCreateFixedCharacterByTemplateId(context, charTemplateId);
			break;
		case 3:
			character = DomainManager.Character.CreateFixedEnemy(context, charTemplateId);
			DomainManager.Character.CompleteCreatingCharacter(character.GetId());
			break;
		}
		CombatEntry(context, new List<int> { character.GetId() }, 2);
	}

	[DomainMethod]
	public void GmCmd_FightTestOrgMember(DataContext context, short charTemplateId, int testCount)
	{
		GameData.Domains.Character.Character character = DomainManager.Character.CreateRandomEnemy(context, charTemplateId);
		DomainManager.Character.CompleteCreatingCharacter(character.GetId());
		testCount = Math.Clamp(testCount - 1, 0, 8);
		short combatConfigTemplateId = (short)(136 + testCount);
		CombatEntry(context, new List<int> { character.GetId() }, combatConfigTemplateId);
	}

	[DomainMethod]
	public void GmCmd_FightRandomEnemy(DataContext context, short charTemplateId, sbyte combatTypeAsSbyte)
	{
		GameData.Domains.Character.Character character = DomainManager.Character.CreateRandomEnemy(context, charTemplateId);
		DomainManager.Character.CompleteCreatingCharacter(character.GetId());
		CombatType combatType = (CombatType)combatTypeAsSbyte;
		if (1 == 0)
		{
		}
		short num = combatType switch
		{
			CombatType.Play => 0, 
			CombatType.Beat => 1, 
			CombatType.Die => 2, 
			_ => 3, 
		};
		if (1 == 0)
		{
		}
		short combatConfigTemplateId = num;
		CombatEntry(context, new List<int> { character.GetId() }, combatConfigTemplateId);
	}

	[DomainMethod]
	public void GmCmd_FightCharacter(DataContext context, int charId, short combatConfig)
	{
		if (charId != DomainManager.Taiwu.GetTaiwuCharId())
		{
			CombatEntry(context, new List<int> { charId }, combatConfig);
		}
	}

	[DomainMethod]
	public void GmCmd_EnableEnemyAi(DataContext context, bool on)
	{
		_enableEnemyAi = on;
		if (!on)
		{
			SetMoveState(0, isAlly: false);
		}
	}

	[DomainMethod]
	public void GmCmd_EnableSkillFreeCast(DataContext context, bool on)
	{
		_enableSkillFreeCast = on;
		UpdateSkillCanUse(context, _selfChar);
	}

	[DomainMethod]
	public void GmCmd_SetImmortal(DataContext context, bool isAlly, bool on)
	{
		CombatCharacter combatCharacter = (isAlly ? _selfChar : _enemyChar);
		combatCharacter.Immortal = on;
		if (!on)
		{
			AddToCheckFallenSet(combatCharacter.GetId());
		}
	}

	[DomainMethod]
	public void GmCmd_UnitTestPrepare(DataContext context, bool testing = true)
	{
	}

	[DomainMethod]
	public void GmCmd_UnitTestClearAllEquipSkill(DataContext context, int charId)
	{
	}

	[DomainMethod]
	public bool GmCmd_UnitTestEquipSkill(DataContext context, int charId, short skillTemplateId, bool isDirect)
	{
		return false;
	}

	[DomainMethod]
	public void GmCmd_UnitTestSetDistanceToTarget(DataContext context, bool isAlly)
	{
	}

	[DomainMethod]
	public void GmCmd_ForceRecoverMobilityValue(DataContext context)
	{
		ChangeMobilityValue(context, _selfChar, _selfChar.GetMaxMobility());
	}

	[DomainMethod]
	public void GmCmd_ForceRecoverWugCount(DataContext context, bool isAlly, short wugCount)
	{
		CombatCharacter combatCharacter = (isAlly ? _selfChar : _enemyChar);
		combatCharacter.ChangeWugCount(context, wugCount);
	}

	[DomainMethod]
	public OuterAndInnerDamageStepDisplayData GetBodyPartDamageStepDisplayData(int charId, sbyte bodyPart)
	{
		return new OuterAndInnerDamageStepDisplayData
		{
			Outer = CalcDamageDisplayData(charId, bodyPart),
			Inner = CalcDamageDisplayData(charId, bodyPart, inner: true)
		};
	}

	[DomainMethod]
	public DamageStepDisplayData GetMindDamageStepDisplayData(int charId)
	{
		return CalcDamageDisplayData(charId, -1, inner: false, mind: true);
	}

	[DomainMethod]
	public DamageStepDisplayData GetFatalDamageStepDisplayData(int charId)
	{
		return CalcDamageDisplayData(charId, -1, inner: false, mind: false, fatal: true);
	}

	[DomainMethod]
	public CompleteDamageStepDisplayData GetCompleteDamageStepDisplayData(int charId)
	{
		GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(charId);
		bool flag = element_Objects.IsLoseConsummateBonusByFeature();
		CompleteDamageStepDisplayData completeDamageStepDisplayData = new CompleteDamageStepDisplayData
		{
			Fatal = GetFatalDamageStepDisplayData(charId),
			Mind = GetMindDamageStepDisplayData(charId),
			CharacterTemplateId = element_Objects.GetTemplateId(),
			CharacterConsummateLevel = (sbyte)((!flag) ? element_Objects.GetConsummateLevel() : 0)
		};
		for (sbyte b = 0; b < 7; b++)
		{
			completeDamageStepDisplayData.BodyPart[b] = GetBodyPartDamageStepDisplayData(charId, b);
		}
		return completeDamageStepDisplayData;
	}

	private DamageStepDisplayData CalcDamageDisplayData(int charId, sbyte bodyPart = -1, bool inner = false, bool mind = false, bool fatal = false)
	{
		//IL_018a: Unknown result type (might be due to invalid IL or missing references)
		DamageStepDisplayData invalid = DamageStepDisplayData.Invalid;
		int num = 0;
		Dictionary<short, GameData.Domains.CombatSkill.CombatSkill> charCombatSkills = DomainManager.CombatSkill.GetCharCombatSkills(charId);
		foreach (GameData.Domains.CombatSkill.CombatSkill value in charCombatSkills.Values)
		{
			short skillTemplateId = value.GetId().SkillTemplateId;
			CombatSkillItem combatSkillItem = Config.CombatSkill.Instance[skillTemplateId];
			int num2;
			if (mind)
			{
				num2 = value.CalcMindDamageStep();
			}
			else if (!fatal)
			{
				bool flag = ((bodyPart < 0 || bodyPart >= 7) ? true : false);
				num2 = ((!flag) ? value.CalcInjuryDamageStep(inner, bodyPart) : 0);
			}
			else
			{
				num2 = value.CalcFatalDamageStep();
			}
			if (num2 > num)
			{
				num = num2;
				invalid.ActivateSkillTemplateId = skillTemplateId;
				invalid.ActivateSkillBonusData = value.CalcStepBonusDisplayData();
			}
			else if (num2 == num && invalid.ActivateSkillTemplateId >= 0)
			{
				CombatSkillItem combatSkillItem2 = Config.CombatSkill.Instance[invalid.ActivateSkillTemplateId];
				if (combatSkillItem2.Grade < combatSkillItem.Grade || combatSkillItem2.TemplateId > combatSkillItem.TemplateId)
				{
					invalid.ActivateSkillTemplateId = skillTemplateId;
					invalid.ActivateSkillBonusData = value.CalcStepBonusDisplayData();
				}
			}
		}
		EMarkType markType = (fatal ? EMarkType.Fatal : (mind ? EMarkType.Mind : (inner ? EMarkType.Inner : EMarkType.Outer)));
		GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(charId);
		invalid.EatingBonusData = (int)element_Objects.GetEatingItems().CalcDamageStepBonus(markType);
		return invalid;
	}

	public DamageStepCollection GetDamageStepCollection(int charId)
	{
		//IL_01c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0202: Unknown result type (might be due to invalid IL or missing references)
		//IL_0207: Unknown result type (might be due to invalid IL or missing references)
		//IL_021d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0239: Unknown result type (might be due to invalid IL or missing references)
		//IL_023e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0254: Unknown result type (might be due to invalid IL or missing references)
		//IL_0277: Unknown result type (might be due to invalid IL or missing references)
		//IL_0290: Unknown result type (might be due to invalid IL or missing references)
		GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(charId);
		short templateId = element_Objects.GetTemplateId();
		DamageStepCollection damageStepCollection = new DamageStepCollection(Config.Character.Instance[templateId].DamageSteps);
		Span<int> span = stackalloc int[7];
		Span<int> span2 = stackalloc int[7];
		span.Fill(0);
		span2.Fill(0);
		int num = 0;
		int num2 = 0;
		Dictionary<short, GameData.Domains.CombatSkill.CombatSkill> charCombatSkills = DomainManager.CombatSkill.GetCharCombatSkills(charId);
		foreach (GameData.Domains.CombatSkill.CombatSkill value in charCombatSkills.Values)
		{
			for (sbyte b = 0; b < 7; b++)
			{
				span[b] = Math.Max(span[b], value.CalcInjuryDamageStep(inner: false, b));
				span2[b] = Math.Max(span2[b], value.CalcInjuryDamageStep(inner: true, b));
			}
			num = Math.Max(num, value.CalcFatalDamageStep());
			num2 = Math.Max(num2, value.CalcMindDamageStep());
		}
		for (int i = 0; i < 7; i++)
		{
			damageStepCollection.OuterDamageSteps[i] += span[i];
			damageStepCollection.InnerDamageSteps[i] += span2[i];
		}
		damageStepCollection.FatalDamageStep += num;
		damageStepCollection.MindDamageStep += num2;
		int index = ((!element_Objects.IsLoseConsummateBonusByFeature()) ? element_Objects.GetConsummateLevel() : 0);
		ConsummateLevelItem consummateLevelItem = ConsummateLevel.Instance[index];
		damageStepCollection.FatalDamageStep *= CValuePercentBonus.op_Implicit(consummateLevelItem.FatalDamageStepAddPercent);
		damageStepCollection.MindDamageStep *= CValuePercentBonus.op_Implicit(consummateLevelItem.MindDamageStepAddPercent);
		EatingItems eatingItems = element_Objects.GetEatingItems();
		CValuePercentBonus val = eatingItems.CalcDamageStepBonus(EMarkType.Outer);
		for (int j = 0; j < 7; j++)
		{
			ref int reference = ref damageStepCollection.OuterDamageSteps[j];
			reference *= val;
		}
		CValuePercentBonus val2 = eatingItems.CalcDamageStepBonus(EMarkType.Inner);
		for (int k = 0; k < 7; k++)
		{
			ref int reference2 = ref damageStepCollection.InnerDamageSteps[k];
			reference2 *= val2;
		}
		damageStepCollection.FatalDamageStep *= eatingItems.CalcDamageStepBonus(EMarkType.Fatal);
		damageStepCollection.MindDamageStep *= eatingItems.CalcDamageStepBonus(EMarkType.Mind);
		return damageStepCollection;
	}

	public void UpdateDamageCompareData(CombatContext context)
	{
		CombatCharacter attacker = context.Attacker;
		CombatCharacter defender = context.Defender;
		GameData.Domains.Item.Weapon weapon = context.Weapon;
		sbyte bodyPart = context.BodyPart;
		short skillTemplateId = context.SkillTemplateId;
		_damageCompareData.IsAlly = attacker.IsAlly;
		_damageCompareData.SkillId = skillTemplateId;
		if (skillTemplateId >= 0)
		{
			GameData.Domains.CombatSkill.CombatSkill skill = context.Skill;
			for (int i = 0; i < 3; i++)
			{
				_damageCompareData.HitType[i] = attacker.SkillHitType[i];
				_damageCompareData.HitValue[i] = attacker.SkillHitValue[i];
				_damageCompareData.AvoidValue[i] = attacker.SkillAvoidValue[i];
			}
			_damageCompareData.OuterAttackValue = attacker.GetPenetrate(inner: false, weapon, bodyPart, skillTemplateId, skill.GetPenetrations().Outer);
			_damageCompareData.InnerAttackValue = attacker.GetPenetrate(inner: true, weapon, bodyPart, skillTemplateId, skill.GetPenetrations().Inner);
		}
		else
		{
			sbyte normalAttackHitType = attacker.NormalAttackHitType;
			_damageCompareData.HitType[0] = normalAttackHitType;
			_damageCompareData.HitType[1] = (_damageCompareData.HitType[2] = -1);
			_damageCompareData.HitValue[0] = attacker.GetHitValue(weapon, normalAttackHitType, bodyPart, 0, -1);
			_damageCompareData.AvoidValue[0] = defender.GetAvoidValue(normalAttackHitType, bodyPart, -1);
			_damageCompareData.OuterAttackValue = attacker.GetPenetrate(inner: false, weapon, bodyPart, -1);
			_damageCompareData.InnerAttackValue = attacker.GetPenetrate(inner: true, weapon, bodyPart, -1);
			if (attacker.GetId() == _carrierAnimalCombatCharId)
			{
				sbyte consummateLevel = _selfChar.GetCharacter().GetConsummateLevel();
				_damageCompareData.HitValue[0] = _damageCompareData.HitValue[0] * (100 + consummateLevel * 50) / 100;
				_damageCompareData.OuterAttackValue = _damageCompareData.OuterAttackValue * (200 + consummateLevel * 100) / 100;
				_damageCompareData.InnerAttackValue = _damageCompareData.InnerAttackValue * (200 + consummateLevel * 100) / 100;
			}
			if (attacker.IsBreakAttacking)
			{
				sbyte attackPreparePointCost = weapon.GetAttackPreparePointCost();
				short num = GlobalConfig.Instance.BreakAttackHitBasePercent[attackPreparePointCost];
				_damageCompareData.HitValue[0] = _damageCompareData.HitValue[0] * num / 100;
			}
		}
		_damageCompareData.OuterDefendValue = defender.GetPenetrateResist(inner: false, weapon, bodyPart, skillTemplateId);
		_damageCompareData.InnerDefendValue = defender.GetPenetrateResist(inner: true, weapon, bodyPart, skillTemplateId);
		Events.RaiseCompareDataCalcFinished(context, _damageCompareData);
		SetDamageCompareData(_damageCompareData, context);
	}

	public int GetFinalCriticalOdds(CombatCharacter combatChar)
	{
		int skillFinalAttackHitIndex = combatChar.SkillFinalAttackHitIndex;
		int hitValue = _damageCompareData.HitValue[skillFinalAttackHitIndex];
		int avoidValue = _damageCompareData.AvoidValue[skillFinalAttackHitIndex];
		int hitOdds = CFormula.FormulaCalcHitOdds(hitValue, avoidValue);
		return CFormula.FormulaCalcCriticalOdds(hitOdds);
	}

	public void ClearDamageCompareData(DataContext context)
	{
		_damageCompareData.Clear();
		SetDamageCompareData(_damageCompareData, context);
	}

	private OuterAndInnerInts CalcAndAddInjury(CombatContext context, sbyte hitType, out int finalDamage, out bool critical, int power = 100, int outerPower = 100, int innerPower = 100)
	{
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		OuterAndInnerInts result = new OuterAndInnerInts(0, 0);
		critical = context.CheckCritical(hitType);
		if (context.BodyPart >= 0)
		{
			CalcMixedInjuryBegin(context, critical);
			CombatDamageResultMixed result2 = CalcMixedInjury(context, hitType, critical, CValuePercent.op_Implicit(power), CValuePercent.op_Implicit(outerPower), CValuePercent.op_Implicit(innerPower));
			result = result2.MarkCounts;
			finalDamage = result2.TotalDamage;
			ApplyMixedInjury(context, result2);
			CalcMixedInjuryEnd(context);
		}
		else
		{
			CombatDamageResult result3 = CalcMindInjury(context);
			result.Outer = result3.MarkCount;
			finalDamage = result3.TotalDamage;
			ApplyMindInjury(context, result3);
		}
		return result;
	}

	public void AddBounceDamage(CombatContext context, sbyte hitType)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		AddBounceDamage(context, hitType, -1, CValuePercent.op_Implicit(100));
	}

	public void AddBounceDamage(CombatContext context, sbyte hitType, short skillId, CValuePercent bouncePercent)
	{
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		if (!DomainManager.SpecialEffect.ModifyData(context.AttackerId, skillId, 85, dataValue: true))
		{
			return;
		}
		OuterAndInnerInts bouncePower = context.Defender.GetBouncePower(context.InnerRatio);
		ref int outer = ref bouncePower.Outer;
		outer *= bouncePercent;
		ref int inner = ref bouncePower.Inner;
		inner *= bouncePercent;
		if (bouncePower.Outer > 0 || bouncePower.Inner > 0)
		{
			short affectingDefendSkillId = context.Defender.GetAffectingDefendSkillId();
			OuterAndInnerInts dataValue = ((affectingDefendSkillId >= 0) ? new OuterAndInnerInts(CombatConfig.MinDistance, DomainManager.CombatSkill.GetElement_CombatSkills((charId: context.DefenderId, skillId: affectingDefendSkillId)).GetBounceDistance()) : new OuterAndInnerInts(CombatConfig.MaxDistance, CombatConfig.MinDistance));
			dataValue = DomainManager.SpecialEffect.ModifyData(context.DefenderId, -1, 177, dataValue);
			if (dataValue.Outer <= _currentDistance && _currentDistance <= dataValue.Inner)
			{
				CalcAndAddInjury(context.Bounce(), hitType, out var _, out var _, 100, bouncePower.Outer, bouncePower.Inner);
			}
		}
	}

	public int AddInjuryDamageValue(CombatCharacter attacker, CombatCharacter defender, sbyte bodyPart, int outerDamage, int innerDamage, short combatSkillId, bool updateDefeatMark = true)
	{
		DamageStepCollection damageStepCollection = defender.GetDamageStepCollection();
		CombatContext combatContext = CombatContext.Create(attacker, defender, bodyPart, -1);
		int num = 0;
		if (outerDamage > 0)
		{
			int injuryStep = damageStepCollection.OuterDamageSteps[bodyPart];
			int[] outerDamageValue = defender.GetOuterDamageValue();
			(int, int, int) tuple = CalcSingleInjury(combatContext, outerDamage, injuryStep, inner: false, EDamageType.None, outerDamageValue[bodyPart], combatSkillId);
			num += tuple.Item1;
			if (tuple.Item1 > 0)
			{
				AddInjury(combatContext, defender, bodyPart, isInner: false, (sbyte)tuple.Item1);
			}
			if (tuple.Item2 > 0)
			{
				if (defender.GetInjuries().Get(bodyPart, isInnerInjury: false) < 6)
				{
					outerDamageValue[bodyPart] = tuple.Item2;
				}
				else
				{
					outerDamageValue[bodyPart] = 0;
					tuple.Item3 -= tuple.Item2;
					num += AddFatalDamageValue(combatContext, defender, tuple.Item2, 0, bodyPart, combatSkillId);
				}
				defender.SetOuterDamageValue(outerDamageValue, combatContext);
			}
			if (tuple.Item3 >= 0)
			{
				IntPair[] outerDamageValueToShow = defender.GetOuterDamageValueToShow();
				outerDamageValueToShow[bodyPart].First = Math.Max(outerDamageValueToShow[bodyPart].First, 0);
				outerDamageValueToShow[bodyPart].First += tuple.Item3;
				outerDamageValueToShow[bodyPart].Second = -1;
				defender.SetOuterDamageValueToShow(outerDamageValueToShow, combatContext);
			}
		}
		if (innerDamage > 0)
		{
			int injuryStep2 = damageStepCollection.InnerDamageSteps[bodyPart];
			int[] innerDamageValue = defender.GetInnerDamageValue();
			int originDamageValue = innerDamageValue[bodyPart];
			(int, int, int) tuple2 = CalcSingleInjury(combatContext, innerDamage, injuryStep2, inner: true, EDamageType.None, originDamageValue, combatSkillId);
			num += tuple2.Item1;
			if (tuple2.Item1 > 0)
			{
				AddInjury(combatContext, defender, bodyPart, isInner: true, (sbyte)tuple2.Item1);
			}
			if (tuple2.Item2 > 0)
			{
				if (defender.GetInjuries().Get(bodyPart, isInnerInjury: true) < 6)
				{
					innerDamageValue[bodyPart] = tuple2.Item2;
				}
				else
				{
					innerDamageValue[bodyPart] = 0;
					tuple2.Item3 -= tuple2.Item2;
					num += AddFatalDamageValue(combatContext, defender, tuple2.Item2, 1, bodyPart, -1);
				}
				defender.SetInnerDamageValue(innerDamageValue, combatContext);
			}
			if (tuple2.Item3 >= 0)
			{
				IntPair[] innerDamageValueToShow = defender.GetInnerDamageValueToShow();
				innerDamageValueToShow[bodyPart].First = Math.Max(innerDamageValueToShow[bodyPart].First, 0);
				innerDamageValueToShow[bodyPart].First += tuple2.Item3;
				innerDamageValueToShow[bodyPart].Second = -1;
				defender.SetInnerDamageValueToShow(innerDamageValueToShow, combatContext);
			}
		}
		if (updateDefeatMark)
		{
			UpdateBodyDefeatMark(combatContext, defender, bodyPart);
			AddToCheckFallenSet(defender.GetId());
		}
		return num;
	}

	private static (int markCount, int leftDamage, int finalDamageValue) CalcSingleInjury(CombatContext context, long damage, int injuryStep, bool inner, EDamageType damageType, int originDamageValue, short combatSkillId = -1, int criticalPercent = 100, int armorReducePercent = 0)
	{
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_013e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0143: Unknown result type (might be due to invalid IL or missing references)
		//IL_0149: Unknown result type (might be due to invalid IL or missing references)
		//IL_014e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0172: Unknown result type (might be due to invalid IL or missing references)
		//IL_0177: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0266: Unknown result type (might be due to invalid IL or missing references)
		//IL_026b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0270: Unknown result type (might be due to invalid IL or missing references)
		//IL_0273: Unknown result type (might be due to invalid IL or missing references)
		//IL_0235: Unknown result type (might be due to invalid IL or missing references)
		//IL_0215: Unknown result type (might be due to invalid IL or missing references)
		//IL_0217: Unknown result type (might be due to invalid IL or missing references)
		//IL_021c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0221: Unknown result type (might be due to invalid IL or missing references)
		//IL_0254: Unknown result type (might be due to invalid IL or missing references)
		//IL_0256: Unknown result type (might be due to invalid IL or missing references)
		//IL_025b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0260: Unknown result type (might be due to invalid IL or missing references)
		CombatCharacter attacker = context.Attacker;
		CombatCharacter defender = context.Defender;
		sbyte bodyPart = context.BodyPart;
		if (inner ? defender.GetInnerInjuryImmunity() : defender.GetOuterInjuryImmunity())
		{
			return (markCount: 0, leftDamage: 0, finalDamageValue: -1);
		}
		if (inner ? DomainManager.SpecialEffect.ModifyData(defender.GetId(), combatSkillId, 241, dataValue: false, bodyPart, (int)damageType) : DomainManager.SpecialEffect.ModifyData(defender.GetId(), combatSkillId, 242, dataValue: false, bodyPart, (int)damageType))
		{
			return (markCount: 0, leftDamage: 0, finalDamageValue: -1);
		}
		damage *= CValuePercent.op_Implicit(criticalPercent);
		int num = ((damageType == EDamageType.Bounce) ? context.BounceSourceId : context.AttackerId);
		int defenderId = context.DefenderId;
		if (1 == 0)
		{
		}
		ushort num2 = damageType switch
		{
			EDamageType.Direct => 69, 
			EDamageType.Bounce => 70, 
			EDamageType.FightBack => 71, 
			_ => ushort.MaxValue, 
		};
		if (1 == 0)
		{
		}
		ushort num3 = num2;
		if (1 == 0)
		{
		}
		num2 = damageType switch
		{
			EDamageType.Direct => 102, 
			EDamageType.Bounce => 103, 
			EDamageType.FightBack => 104, 
			_ => ushort.MaxValue, 
		};
		if (1 == 0)
		{
		}
		ushort num4 = num2;
		CValueModify zero = CValueModify.Zero;
		CValueModify val = ((CValueModify)(ref zero)).ChangeB(armorReducePercent);
		if (DomainManager.CombatSkill.TryGetElement_CombatSkills(context.SkillKey, out var element))
		{
			val = ((CValueModify)(ref val)).ChangeB(element.GetMakeDamageBreakBonus());
		}
		CombatSkillKey objectId = new CombatSkillKey(context.DefenderId, context.Defender.GetAffectingDefendSkillId());
		bool anyFatal = context.Defender.GetDefeatMarkCollection().FatalDamageMarkCount > 0;
		if (DomainManager.CombatSkill.TryGetElement_CombatSkills(objectId, out var element2))
		{
			val = ((CValueModify)(ref val)).ChangeB(element2.GetAcceptDirectDamageBreakBonus(anyFatal));
		}
		EDataSumType valueSumType = (inner ? context.InnerSumType : context.OuterSumType);
		if (num3 != ushort.MaxValue)
		{
			val += DomainManager.SpecialEffect.GetModify(num, combatSkillId, num3, inner ? 1 : 0, bodyPart, (criticalPercent > 100) ? 1 : 0, valueSumType);
		}
		if (num4 != ushort.MaxValue)
		{
			val += DomainManager.SpecialEffect.GetModify(defenderId, combatSkillId, num4, inner ? 1 : 0, bodyPart, (criticalPercent > 100) ? 1 : 0, valueSumType);
		}
		val = ((CValueModify)(ref val)).MaxB(CValuePercent.op_Implicit(20));
		damage *= val;
		if (damageType == EDamageType.Direct)
		{
			damage = DomainManager.SpecialEffect.ModifyData(attacker.GetId(), combatSkillId, 323, damage, defenderId);
		}
		damage = DomainManager.SpecialEffect.ModifyData(attacker.GetId(), combatSkillId, 89, damage, (int)damageType, inner ? 1 : 0, bodyPart);
		if (damageType == EDamageType.Direct)
		{
			damage = DomainManager.SpecialEffect.ModifyData(defender.GetId(), combatSkillId, 320, damage, num, (criticalPercent > 100) ? 1 : 0);
		}
		damage = DomainManager.SpecialEffect.ModifyData(defender.GetId(), combatSkillId, 114, damage, (int)damageType, inner ? 1 : 0, bodyPart);
		(int, int) tuple = CalcInjuryMarkCount((int)Math.Min(originDamageValue + damage, 2147483647L), injuryStep, 6 - defender.GetInjuries().Get(bodyPart, inner));
		(int, int, int) result = (tuple.Item1, tuple.Item2, (int)damage);
		if (damageType == EDamageType.Direct)
		{
			Events.RaiseAddDirectDamageValue(attacker.GetDataContext(), attacker.GetId(), defender.GetId(), bodyPart, inner, (int)damage, combatSkillId);
		}
		if (result.Item1 > 0 && defender.GetInjuries().Get(bodyPart, inner) == 0 && !DomainManager.SpecialEffect.ModifyData(attacker.GetId(), combatSkillId, 80, dataValue: true, inner ? 1 : 0))
		{
			result.Item1 = 0;
		}
		return result;
	}

	public static (int markCount, int leftDamage) CalcInjuryMarkCount(int damage, int injuryStep, int maxMarkCount = -1)
	{
		injuryStep = Math.Max(injuryStep, 1);
		int num = damage / injuryStep;
		num = ((maxMarkCount < 0) ? num : Math.Min(num, maxMarkCount));
		int item = damage - num * injuryStep;
		return (markCount: num, leftDamage: item);
	}

	private static CombatDamageResultMixed CalcMixedInjury(CombatContext context, sbyte hitType, bool critical, CValuePercent power, CValuePercent outerPower, CValuePercent innerPower)
	{
		//IL_0004: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		context.CalcMixedDamage(hitType, power).Deconstruct(out var outer, out var inner);
		int num = outer;
		int num2 = inner;
		num *= outerPower;
		num2 *= innerPower;
		EDamageType outerDamageType = context.OuterDamageType;
		EDamageType innerDamageType = context.InnerDamageType;
		if (outerDamageType == EDamageType.Direct)
		{
			num *= context.ConsummateBonus;
		}
		if (innerDamageType == EDamageType.Direct)
		{
			num2 *= context.ConsummateBonus;
		}
		bool flag = DomainManager.SpecialEffect.ModifyData(context.AttackerId, context.SkillTemplateId, 281, dataValue: false);
		ItemKey itemKey = context.Defender.Armors[context.BodyPart];
		int armorReducePercent = 0;
		int armorReducePercent2 = 0;
		if (!flag && itemKey.IsValid())
		{
			GameData.Domains.Item.Armor element_Armors = DomainManager.Item.GetElement_Armors(itemKey.Id);
			int attack = CalcWeaponAttack(context.Attacker, context.Weapon, context.SkillTemplateId);
			int defense = CalcArmorDefend(context.Defender, element_Armors);
			int num3 = CFormula.FormulaCalcWeaponArmorFactor(100, attack, defense);
			if (num3 > 0)
			{
				OuterAndInnerShorts injuryFactor = element_Armors.GetInjuryFactor();
				armorReducePercent = -injuryFactor.Outer * num3 / 100;
				armorReducePercent2 = -injuryFactor.Inner * num3 / 100;
			}
		}
		int criticalPercent = (critical ? context.CalcProperty(hitType).CriticalPercent : 100);
		(int, int, int) tuple = CalcSingleInjury(context, num, context.OuterStep, inner: false, outerDamageType, context.OuterOrigin, context.SkillTemplateId, criticalPercent, armorReducePercent);
		(int, int, int) tuple2 = CalcSingleInjury(context, num2, context.InnerStep, inner: true, innerDamageType, context.InnerOrigin, context.SkillTemplateId, criticalPercent, armorReducePercent2);
		ref int item = ref tuple.Item1;
		ref int item2 = ref tuple2.Item1;
		CalcMixedInjuryMark(context, new OuterAndInnerInts(tuple.Item1, tuple2.Item1)).Deconstruct(out inner, out outer);
		item = inner;
		item2 = outer;
		CalcMixedInjuryRefill(context, inner: false, ref tuple.Item1, ref tuple.Item2);
		CalcMixedInjuryRefill(context, inner: true, ref tuple2.Item1, ref tuple2.Item2);
		return new CombatDamageResultMixed
		{
			Outer = new CombatDamageResult
			{
				TotalDamage = tuple.Item1 * context.OuterStep + tuple.Item2 - context.OuterOrigin,
				LeftDamage = tuple.Item2,
				MarkCount = tuple.Item1
			},
			Inner = new CombatDamageResult
			{
				TotalDamage = tuple2.Item1 * context.InnerStep + tuple2.Item2 - context.InnerOrigin,
				LeftDamage = tuple2.Item2,
				MarkCount = tuple2.Item1
			},
			CriticalPercent = criticalPercent
		};
	}

	private static void CalcMixedInjuryBegin(CombatContext context, bool critical)
	{
		context.Defender.BeCriticalDuringCalcAddInjury = critical;
		context.Defender.BeCalcInjuryInnerRatio = context.InnerRatio;
	}

	private static void CalcMixedInjuryEnd(CombatContext context)
	{
		context.Defender.BeCriticalDuringCalcAddInjury = false;
		context.Defender.BeCalcInjuryInnerRatio = -1;
	}

	private static OuterAndInnerInts CalcMixedInjuryMark(CombatContext context, OuterAndInnerInts originMarks)
	{
		originMarks.Outer = ModifyMarkCount(inner: false);
		originMarks.Inner = ModifyMarkCount(inner: true);
		if (context.OuterDamageType == EDamageType.Direct && context.InnerDamageType == EDamageType.Direct)
		{
			originMarks = DomainManager.SpecialEffect.ModifyData(context.DefenderId, context.SkillTemplateId, 116, originMarks, context.BodyPart);
		}
		originMarks.Outer = Math.Max(originMarks.Outer, 0);
		originMarks.Inner = Math.Max(originMarks.Inner, 0);
		return originMarks;
		int ModifyMarkCount(bool inner)
		{
			EDamageType eDamageType = (inner ? context.InnerDamageType : context.OuterDamageType);
			int num = (inner ? originMarks.Inner : originMarks.Outer);
			if (num <= 0)
			{
				return 0;
			}
			sbyte bodyPart = context.BodyPart;
			short skillTemplateId = context.SkillTemplateId;
			if (1 == 0)
			{
			}
			int num2 = eDamageType switch
			{
				EDamageType.Direct => DomainManager.SpecialEffect.ModifyValue(context.DefenderId, skillTemplateId, 116, num, bodyPart, inner ? 1 : 0, num), 
				EDamageType.FightBack => DomainManager.SpecialEffect.ModifyValue(context.AttackerId, skillTemplateId, 87, num, bodyPart, inner ? 1 : 0, num), 
				_ => num, 
			};
			if (1 == 0)
			{
			}
			int val = num2;
			return Math.Max(val, 0);
		}
	}

	private static void CalcMixedInjuryRefill(CombatContext context, bool inner, ref int markCount, ref int leftDamage)
	{
		int num = Math.Max(inner ? context.InnerStep : context.OuterStep, 1);
		sbyte b = context.Defender.GetInjuries().Get(context.BodyPart, inner);
		int num2 = b + markCount;
		if (num2 != 6 && (num2 >= 6 || leftDamage >= num))
		{
			if (num2 > 6)
			{
				int num3 = num2 - 6;
				markCount -= num3;
				leftDamage += num3 * num;
			}
			else
			{
				(int, int) tuple = CalcInjuryMarkCount(leftDamage, num, 6 - num2);
				markCount += tuple.Item1;
				leftDamage = tuple.Item2;
			}
		}
	}

	private void ApplyMixedInjury(CombatContext context, CombatDamageResultMixed result)
	{
		CombatDamageResultMixed combatDamageResultMixed = result;
		var (outerResult, innerResult) = (CombatDamageResultMixed)(ref combatDamageResultMixed);
		if (context.InnerRatio > 0 && context.Defender.GetInnerInjuryImmunity())
		{
			ShowImmunityEffectTips(context, context.DefenderId, EMarkType.Inner);
		}
		if (context.OuterRatio > 0 && context.Defender.GetOuterInjuryImmunity())
		{
			ShowImmunityEffectTips(context, context.DefenderId, EMarkType.Outer);
		}
		OuterAndInnerInts outerAndInnerInts = new OuterAndInnerInts(CalcLeftFatalDamage(inner: false), CalcLeftFatalDamage(inner: true));
		if (outerResult.MarkCount > 0)
		{
			AddInjury(context, context.Defender, context.BodyPart, isInner: false, (sbyte)outerResult.MarkCount, updateDefeatMark: false, context.OuterInjuryChangeToOld);
		}
		if (innerResult.MarkCount > 0)
		{
			AddInjury(context, context.Defender, context.BodyPart, isInner: true, (sbyte)innerResult.MarkCount, updateDefeatMark: false, context.InnerInjuryChangeToOld);
		}
		if (context.OuterRatio > 0 || outerResult.TotalDamage > 0)
		{
			context.Defender.AddDamageToShow(context, outerResult.TotalDamage - outerAndInnerInts.Outer, result.CriticalPercent, context.BodyPart, inner: false);
		}
		if (context.InnerRatio > 0 || innerResult.TotalDamage > 0)
		{
			context.Defender.AddDamageToShow(context, innerResult.TotalDamage - outerAndInnerInts.Inner, result.CriticalPercent, context.BodyPart, inner: true);
		}
		OuterAndInnerInts outerAndInnerInts2 = new OuterAndInnerInts(0, 0);
		if (outerAndInnerInts.Outer > 0)
		{
			outerAndInnerInts2.Outer = AddFatalDamageValue(context, context.Defender, outerAndInnerInts.Outer, 0, context.BodyPart, context.SkillTemplateId, context.OuterDamageType);
		}
		if (outerAndInnerInts.Inner > 0)
		{
			outerAndInnerInts2.Inner = AddFatalDamageValue(context, context.Defender, outerAndInnerInts.Inner, 1, context.BodyPart, context.SkillTemplateId, context.InnerDamageType);
		}
		Events.RaiseAddDirectFatalDamage(context, outerAndInnerInts.Outer, outerAndInnerInts.Inner);
		Events.RaiseAddDirectFatalDamageMark(context, context.AttackerId, context.DefenderId, context.Attacker.IsAlly, context.BodyPart, outerAndInnerInts2.Outer, outerAndInnerInts2.Inner, context.SkillTemplateId);
		if (context.DamageType == EDamageType.Bounce)
		{
			Events.RaiseBounceInjury(context, context.BounceSourceId, context.DefenderId, context.Attacker.IsAlly, context.BodyPart, (sbyte)outerResult.MarkCount, (sbyte)innerResult.MarkCount);
		}
		else if (result.MarkCounts.IsNonZero)
		{
			Events.RaiseAddDirectInjury(context, context.AttackerId, context.DefenderId, context.Attacker.IsAlly, context.BodyPart, (sbyte)outerResult.MarkCount, (sbyte)innerResult.MarkCount, context.SkillTemplateId);
		}
		if (result.MarkCounts.IsNonZero)
		{
			UpdateBodyDefeatMark(context, context.Defender);
		}
		int CalcLeftFatalDamage(bool inner)
		{
			int num = (inner ? innerResult.LeftDamage : outerResult.LeftDamage);
			sbyte b = context.Defender.GetInjuries().Get(context.BodyPart, inner);
			if (b + (inner ? innerResult.MarkCount : outerResult.MarkCount) < 6)
			{
				context.Defender.SetDamageValue(context, num, context.BodyPart, inner);
				return 0;
			}
			if (context.Defender.GetDamageValue(context.BodyPart, inner) != 0)
			{
				context.Defender.SetDamageValue(context, 0, context.BodyPart, inner);
			}
			return num;
		}
	}

	private static CombatDamageResult CalcMindInjury(CombatContext context)
	{
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		int hitOdds = context.CalcProperty(3).HitOdds;
		int num = CFormula.FormulaCalcDamageValue(context.BaseDamage, hitOdds, 100L, context.AttackOdds);
		num *= context.ConsummateBonus;
		int num2 = 0;
		if (DomainManager.CombatSkill.TryGetElement_CombatSkills(context.SkillKey, out var element))
		{
			num2 += element.GetMakeDamageBreakBonus();
		}
		num = DomainManager.SpecialEffect.ModifyValueCustom(context.AttackerId, context.SkillTemplateId, 275, num, -1, -1, -1, 0, num2);
		num = DomainManager.SpecialEffect.ModifyValueCustom(context.DefenderId, context.SkillTemplateId, 276, num);
		int mindDamageValue = context.Defender.GetMindDamageValue();
		int mindDamageStep = context.DamageStepCollection.MindDamageStep;
		(int, int) tuple = CalcInjuryMarkCount(num + mindDamageValue, mindDamageStep);
		CombatDamageResult result = new CombatDamageResult
		{
			TotalDamage = tuple.Item1 * mindDamageStep + tuple.Item2 - mindDamageValue,
			LeftDamage = tuple.Item2
		};
		(result.MarkCount, _) = tuple;
		return result;
	}

	private void ApplyMindInjury(CombatContext context, CombatDamageResult result)
	{
		CombatCharacter defender = context.Defender;
		defender.SetMindDamageValue(result.LeftDamage, context);
		AppendMindDefeatMark(context, defender, result.MarkCount, context.SkillTemplateId);
		Events.RaiseAddMindDamage(Context, context.AttackerId, context.DefenderId, result.TotalDamage, result.MarkCount, context.SkillTemplateId);
		if (result.TotalDamage >= 0)
		{
			defender.AddMindDamageToShow(context, result.TotalDamage);
		}
	}

	public static bool CheckCritical(IRandomSource random, int charId, int hitOdds, bool certainCritical = false)
	{
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		int value = CFormula.FormulaCalcCriticalOdds(hitOdds);
		value = DomainManager.SpecialEffect.ModifyValue(charId, 254, value);
		if (DomainManager.Combat.TryGetElement_CombatCharacterDict(charId, out var element) && element.ExecutingTeammateCommandImplement == ETeammateCommandImplement.Attack)
		{
			value *= CValuePercentBonus.op_Implicit(element.ExecutingTeammateCommandConfig.IntArg);
		}
		return certainCritical || random.CheckPercentProb(value);
	}

	public void AddInjury(DataContext context, CombatCharacter character, sbyte bodyPart, bool isInner, sbyte value, bool updateDefeatMark = false, bool changeToOld = false)
	{
		if (value <= 0 || (isInner ? character.GetInnerInjuryImmunity() : character.GetOuterInjuryImmunity()))
		{
			ShowImmunityEffectTips(context, character.GetId(), isInner ? EMarkType.Inner : EMarkType.Outer);
			return;
		}
		if (character.ChangeToMindMark)
		{
			AppendMindDefeatMark(context, character, value, -1);
			return;
		}
		Injuries injuries = character.GetInjuries();
		sbyte b = injuries.Get(bodyPart, isInner);
		if (b < 6)
		{
			injuries.Change(bodyPart, isInner, value);
			SetInjuries(context, character, injuries, updateDefeatMark);
		}
		if (changeToOld)
		{
			ChangeToOldInjury(context, character, bodyPart, isInner, value);
		}
		Events.RaiseAddInjury(context, character, bodyPart, isInner, value, changeToOld);
	}

	public void RemoveHalfInjury(DataContext context, CombatCharacter character, bool isInner)
	{
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		Injuries injuries = character.GetInjuries();
		Injuries injuries2 = injuries.Subtract(character.GetOldInjuries());
		List<sbyte> list = ObjectPool<List<sbyte>>.Instance.Get();
		list.Clear();
		CRandom.GenerateRemoveInjuryRandomPool(list, injuries2, isInner, -1);
		int maxCount = list.Count * CValueHalf.RoundUp;
		foreach (sbyte item in RandomUtils.GetRandomUnrepeated(context.Random, maxCount, list))
		{
			injuries.Change(item, isInner, -1);
		}
		ObjectPool<List<sbyte>>.Instance.Return(list);
		SetInjuries(context, character, injuries);
	}

	public void RemoveInjury(DataContext context, CombatCharacter character, sbyte bodyPart, bool isInner, sbyte removeCount, bool updateDefeatMark = false, bool removeOldInjury = false)
	{
		Injuries injuries = character.GetInjuries();
		injuries.Change(bodyPart, isInner, (sbyte)(-removeCount));
		if (removeOldInjury)
		{
			Injuries oldInjuries = character.GetOldInjuries();
			oldInjuries.Change(bodyPart, isInner, (sbyte)(-removeCount));
			character.SetOldInjuries(oldInjuries, context);
		}
		SetInjuries(context, character, injuries, updateDefeatMark);
	}

	public void AddRandomInjury(DataContext context, CombatCharacter character, bool inner, int count = 1, sbyte value = 1, bool changeToOld = false, sbyte bodyPartType = -1)
	{
		if (inner ? character.GetInnerInjuryImmunity() : character.GetOuterInjuryImmunity())
		{
			ShowImmunityEffectTips(context, character.GetId(), inner ? EMarkType.Inner : EMarkType.Outer);
			return;
		}
		if (character.ChangeToMindMark)
		{
			AppendMindDefeatMark(context, character, count * value, -1);
			return;
		}
		Injuries injuries = character.GetInjuries();
		List<sbyte> list = ObjectPool<List<sbyte>>.Instance.Get();
		List<sbyte> list2 = ObjectPool<List<sbyte>>.Instance.Get();
		List<sbyte> list3 = ObjectPool<List<sbyte>>.Instance.Get();
		list.Clear();
		CRandom.GenerateAddRandomInjuryPool(list2, list3, injuries, inner, bodyPartType);
		foreach (sbyte item in RandomUtils.GetRandomUnrepeated(context.Random, count, list2, list3))
		{
			sbyte b = injuries.Get(item, inner);
			if (b < 6)
			{
				injuries.Change(item, inner, value);
				list.Add(item);
			}
		}
		SetInjuries(context, character, injuries);
		for (int i = 0; i < list.Count; i++)
		{
			if (changeToOld)
			{
				ChangeToOldInjury(context, character, list[i], inner, value);
			}
			Events.RaiseAddInjury(context, character, list[i], inner, value, changeToOld: false);
		}
		ObjectPool<List<sbyte>>.Instance.Return(list);
		ObjectPool<List<sbyte>>.Instance.Return(list2);
		ObjectPool<List<sbyte>>.Instance.Return(list3);
	}

	public void SetInjuries(DataContext context, CombatCharacter character, Injuries injuries, bool updateDefeatMark = true, bool syncAutoHealProgress = true)
	{
		Injuries oldInjuries = character.GetOldInjuries();
		Injuries injuries2 = injuries.Subtract(oldInjuries);
		bool flag = false;
		if (injuries2.HasAnyInjury())
		{
			for (sbyte b = 0; b < 7; b++)
			{
				(sbyte, sbyte) tuple = injuries.Get(b);
				(sbyte, sbyte) tuple2 = oldInjuries.Get(b);
				if (tuple.Item1 < tuple2.Item1)
				{
					oldInjuries.Change(b, isInnerInjury: false, (sbyte)(tuple.Item1 - tuple2.Item1));
					flag = true;
				}
				if (tuple.Item2 < tuple2.Item2)
				{
					oldInjuries.Change(b, isInnerInjury: true, (sbyte)(tuple.Item2 - tuple2.Item2));
					flag = true;
				}
			}
		}
		if (flag)
		{
			character.SetOldInjuries(oldInjuries, context);
		}
		character.SetInjuries(injuries, context);
		character.GetCharacter().SetInjuries(injuries, context);
		if (syncAutoHealProgress)
		{
			SyncInjuryAutoHealCollection(context, character);
		}
		if (updateDefeatMark)
		{
			UpdateBodyDefeatMark(context, character);
		}
		UpdateOtherActionCanUse(context, character, 0);
		if (IsMainCharacter(character))
		{
			UpdateAllTeammateCommandUsable(context, character.IsAlly, ETeammateCommandImplement.HealInjury);
			UpdateAllTeammateCommandUsable(context, character.IsAlly, ETeammateCommandImplement.TransferInjury);
		}
	}

	public bool CheckBodyPartInjury(CombatCharacter character, sbyte bodyPartType, bool checkHeavyInjury = false)
	{
		Injuries injuries = character.GetInjuries();
		int num = DomainManager.SpecialEffect.ModifyData(character.GetId(), -1, 168, 6, bodyPartType, 0);
		int num2 = DomainManager.SpecialEffect.ModifyData(character.GetId(), -1, 168, 6, bodyPartType, 1);
		if (checkHeavyInjury)
		{
			num = Math.Min(num, 5);
			num2 = Math.Min(num2, 5);
		}
		bool dataValue = injuries.Get(bodyPartType, isInnerInjury: false) >= num || injuries.Get(bodyPartType, isInnerInjury: true) >= num2;
		return DomainManager.SpecialEffect.ModifyData(character.GetId(), -1, 169, dataValue, bodyPartType);
	}

	public void AddFlaw(DataContext context, CombatCharacter character, sbyte level, CombatSkillKey skillKey, sbyte bodyPart = -1, int count = 1, bool raiseEvent = true)
	{
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		if (character.GetFlawImmunity())
		{
			ShowImmunityEffectTips(context, character.GetId(), EMarkType.Flaw);
			return;
		}
		if (character.ChangeToMindMark)
		{
			AppendMindDefeatMark(context, character, count, -1);
			return;
		}
		short skillTemplateId = skillKey.SkillTemplateId;
		if (skillTemplateId >= 0)
		{
			GameData.Domains.CombatSkill.CombatSkill element_CombatSkills = DomainManager.CombatSkill.GetElement_CombatSkills(skillKey);
			level = (sbyte)(level + element_CombatSkills.GetBreakoutGridCombatSkillPropertyBonus(41));
		}
		bool flag = DomainManager.SpecialEffect.ModifyData(character.GetId(), skillTemplateId, 128, dataValue: true);
		EDataSumType valueSumType = DataSumTypeHelper.CalcSumType(true, flag);
		int max = GlobalConfig.Instance.FlawBaseKeepTime.Length - 1;
		level = (sbyte)Math.Clamp((int)level * DomainManager.SpecialEffect.GetModify(character.GetId(), skillTemplateId, 127, -1, -1, -1, valueSumType), 0, max);
		count = (sbyte)(count + DomainManager.SpecialEffect.GetModifyValue(character.GetId(), skillTemplateId, 129, (EDataModifyType)0, -1, -1, -1, (EDataSumType)0));
		count = (sbyte)DomainManager.SpecialEffect.ModifyData(character.GetId(), skillTemplateId, 129, count, bodyPart, level);
		if (count <= 0)
		{
			return;
		}
		List<sbyte> list = ObjectPool<List<sbyte>>.Instance.Get();
		for (int i = 0; i < count; i++)
		{
			if (bodyPart < 0)
			{
				list.Clear();
				foreach (sbyte availableBodyPart in character.GetAvailableBodyParts())
				{
					if (character.GetFlawCount()[availableBodyPart] < character.GetMaxFlawCount())
					{
						list.Add(availableBodyPart);
					}
				}
				if (list.Count == 0)
				{
					foreach (sbyte availableBodyPart2 in character.GetAvailableBodyParts())
					{
						list.Add(availableBodyPart2);
					}
				}
				bodyPart = list[context.Random.Next(list.Count)];
			}
			character.AddOrUpdateFlawOrAcupoint(context, bodyPart, isFlaw: true, level, raiseEvent);
		}
		ObjectPool<List<sbyte>>.Instance.Return(list);
	}

	public void RemoveFlaw(DataContext context, CombatCharacter character, sbyte bodyPart, int index, bool raiseEvent = true, bool updateMark = true)
	{
		FlawOrAcupointCollection flawCollection = character.GetFlawCollection();
		byte[] flawCount = character.GetFlawCount();
		sbyte item = flawCollection.BodyPartDict[bodyPart][index].level;
		flawCollection.BodyPartDict[bodyPart].RemoveAt(index);
		flawCount[bodyPart]--;
		character.SetFlawCollection(flawCollection, context);
		character.SetFlawCount(flawCount, context);
		if (updateMark)
		{
			UpdateBodyDefeatMark(context, character, bodyPart);
		}
		if (raiseEvent)
		{
			Events.RaiseFlawRemoved(context, character, bodyPart, item);
		}
	}

	public void ReduceFlawKeepTimePercent(DataContext context, CombatCharacter combatChar, int reducePercent, bool raiseEvent = true)
	{
		FlawOrAcupointCollection flawCollection = combatChar.GetFlawCollection();
		FlawOrAcupointCollection.ReduceKeepTimeResult reduceResult = flawCollection.ReduceKeepTimePercent(combatChar, reducePercent, combatChar.GetFlawCount(), isFlaw: true);
		ApplyReduceFlawResult(context, combatChar, raiseEvent, reduceResult);
	}

	public void RemoveAllFlaw(DataContext context, CombatCharacter combatChar)
	{
		FlawOrAcupointCollection flawCollection = combatChar.GetFlawCollection();
		FlawOrAcupointCollection.ReduceKeepTimeResult reduceResult = flawCollection.ReduceKeepTime(combatChar, int.MaxValue, combatChar.GetFlawCount(), isFlaw: true);
		ApplyReduceFlawResult(context, combatChar, raiseEvent: false, reduceResult);
	}

	private void ApplyReduceFlawResult(DataContext context, CombatCharacter combatChar, bool raiseEvent, FlawOrAcupointCollection.ReduceKeepTimeResult reduceResult)
	{
		FlawOrAcupointCollection flawCollection = combatChar.GetFlawCollection();
		if (reduceResult.DataChanged)
		{
			combatChar.SetFlawCollection(flawCollection, context);
		}
		if (reduceResult.CountChanged)
		{
			combatChar.SetFlawCount(combatChar.GetFlawCount(), context);
			UpdateBodyDefeatMark(context, combatChar);
			if (IsMainCharacter(combatChar))
			{
				UpdateAllTeammateCommandUsable(context, combatChar.IsAlly, ETeammateCommandImplement.HealFlaw);
			}
		}
		if (!raiseEvent)
		{
			return;
		}
		foreach (var (bodyPart, level) in reduceResult.RemovedList)
		{
			Events.RaiseFlawRemoved(context, combatChar, bodyPart, level);
		}
	}

	public void TransferRandomFlaw(DataContext context, CombatCharacter src, CombatCharacter dst)
	{
		DefeatMarkCollection defeatMarkCollection = src.GetDefeatMarkCollection();
		List<sbyte> list = ObjectPool<List<sbyte>>.Instance.Get();
		for (sbyte b = 0; b < 7; b++)
		{
			if (defeatMarkCollection.FlawMarkList[b].Count > 0)
			{
				list.Add(b);
			}
		}
		if (list.Count > 0)
		{
			sbyte random = list.GetRandom(context.Random);
			int index = context.Random.Next(defeatMarkCollection.FlawMarkList[random].Count);
			TransferFlaw(context, src, dst, random, index);
		}
		ObjectPool<List<sbyte>>.Instance.Return(list);
	}

	public void TransferFlaw(DataContext context, CombatCharacter srcChar, CombatCharacter destChar, sbyte bodyPart, int index)
	{
		(sbyte, int, int) tuple = srcChar.GetFlawCollection().BodyPartDict[bodyPart][index];
		RemoveFlaw(context, srcChar, bodyPart, index, raiseEvent: false);
		destChar.AddOrUpdateFlawOrAcupoint(context, bodyPart, isFlaw: true, tuple.Item1, raiseEvent: true, tuple.Item3, tuple.Item2);
	}

	public void AddAcupoint(DataContext context, CombatCharacter character, sbyte level, CombatSkillKey skillKey, sbyte bodyPart = -1, int count = 1, bool raiseEvent = true)
	{
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		if (character.GetAcupointImmunity())
		{
			ShowImmunityEffectTips(context, character.GetId(), EMarkType.Acupoint);
			return;
		}
		if (character.ChangeToMindMark)
		{
			AppendMindDefeatMark(context, character, count, -1);
			return;
		}
		short skillTemplateId = skillKey.SkillTemplateId;
		if (skillTemplateId >= 0)
		{
			GameData.Domains.CombatSkill.CombatSkill element_CombatSkills = DomainManager.CombatSkill.GetElement_CombatSkills(skillKey);
			level = (sbyte)(level + element_CombatSkills.GetBreakoutGridCombatSkillPropertyBonus(40));
		}
		bool flag = DomainManager.SpecialEffect.ModifyData(character.GetId(), skillTemplateId, 133, dataValue: true);
		EDataSumType valueSumType = DataSumTypeHelper.CalcSumType(true, flag);
		int max = GlobalConfig.Instance.AcupointBaseKeepTime.Length - 1;
		level = (sbyte)Math.Clamp((int)level * DomainManager.SpecialEffect.GetModify(character.GetId(), skillTemplateId, 132, -1, -1, -1, valueSumType), 0, max);
		count = (sbyte)(count + DomainManager.SpecialEffect.GetModifyValue(character.GetId(), skillTemplateId, 134, (EDataModifyType)0, -1, -1, -1, (EDataSumType)0));
		count = (sbyte)DomainManager.SpecialEffect.ModifyData(character.GetId(), skillTemplateId, 134, count, bodyPart, level);
		if (count <= 0)
		{
			return;
		}
		List<sbyte> list = ObjectPool<List<sbyte>>.Instance.Get();
		for (int i = 0; i < count; i++)
		{
			if (bodyPart < 0)
			{
				list.Clear();
				foreach (sbyte availableBodyPart in character.GetAvailableBodyParts())
				{
					if (character.GetAcupointCount()[availableBodyPart] < character.GetMaxAcupointCount())
					{
						list.Add(availableBodyPart);
					}
				}
				if (list.Count == 0)
				{
					foreach (sbyte availableBodyPart2 in character.GetAvailableBodyParts())
					{
						list.Add(availableBodyPart2);
					}
				}
				bodyPart = list[context.Random.Next(list.Count)];
			}
			character.AddOrUpdateFlawOrAcupoint(context, bodyPart, isFlaw: false, level, raiseEvent);
		}
		ObjectPool<List<sbyte>>.Instance.Return(list);
	}

	public void RemoveAcupoint(DataContext context, CombatCharacter character, sbyte bodyPart, int index, bool raiseEvent = true, bool updateMark = true)
	{
		FlawOrAcupointCollection acupointCollection = character.GetAcupointCollection();
		byte[] acupointCount = character.GetAcupointCount();
		sbyte item = acupointCollection.BodyPartDict[bodyPart][index].level;
		acupointCollection.BodyPartDict[bodyPart].RemoveAt(index);
		acupointCount[bodyPart]--;
		character.SetAcupointCollection(acupointCollection, context);
		character.SetAcupointCount(acupointCount, context);
		if (updateMark)
		{
			UpdateBodyDefeatMark(context, character, bodyPart);
		}
		if (raiseEvent)
		{
			Events.RaiseAcuPointRemoved(context, character, bodyPart, item);
		}
	}

	public void ReduceAcupointKeepTimePercent(DataContext context, CombatCharacter combatChar, int reducePercent, bool raiseEvent = true)
	{
		FlawOrAcupointCollection acupointCollection = combatChar.GetAcupointCollection();
		FlawOrAcupointCollection.ReduceKeepTimeResult reduceResult = acupointCollection.ReduceKeepTimePercent(combatChar, reducePercent, combatChar.GetAcupointCount(), isFlaw: false);
		ApplyReduceAcupointResult(context, combatChar, raiseEvent, reduceResult);
	}

	public void RemoveAllAcupoint(DataContext context, CombatCharacter combatChar)
	{
		FlawOrAcupointCollection acupointCollection = combatChar.GetAcupointCollection();
		FlawOrAcupointCollection.ReduceKeepTimeResult reduceResult = acupointCollection.ReduceKeepTime(combatChar, int.MaxValue, combatChar.GetAcupointCount(), isFlaw: false);
		ApplyReduceAcupointResult(context, combatChar, raiseEvent: false, reduceResult);
	}

	private void ApplyReduceAcupointResult(DataContext context, CombatCharacter combatChar, bool raiseEvent, FlawOrAcupointCollection.ReduceKeepTimeResult reduceResult)
	{
		FlawOrAcupointCollection acupointCollection = combatChar.GetAcupointCollection();
		if (reduceResult.DataChanged)
		{
			combatChar.SetAcupointCollection(acupointCollection, context);
		}
		if (reduceResult.CountChanged)
		{
			combatChar.SetAcupointCount(combatChar.GetAcupointCount(), context);
			UpdateBodyDefeatMark(context, combatChar);
			if (IsMainCharacter(combatChar))
			{
				UpdateAllTeammateCommandUsable(context, combatChar.IsAlly, ETeammateCommandImplement.HealAcupoint);
			}
		}
		if (!raiseEvent)
		{
			return;
		}
		foreach (var (bodyPart, level) in reduceResult.RemovedList)
		{
			Events.RaiseAcuPointRemoved(context, combatChar, bodyPart, level);
		}
	}

	public void TransferRandomAcupoint(DataContext context, CombatCharacter src, CombatCharacter dst)
	{
		DefeatMarkCollection defeatMarkCollection = src.GetDefeatMarkCollection();
		List<sbyte> list = ObjectPool<List<sbyte>>.Instance.Get();
		for (sbyte b = 0; b < 7; b++)
		{
			if (defeatMarkCollection.AcupointMarkList[b].Count > 0)
			{
				list.Add(b);
			}
		}
		if (list.Count > 0)
		{
			sbyte random = list.GetRandom(context.Random);
			int index = context.Random.Next(defeatMarkCollection.AcupointMarkList[random].Count);
			TransferAcupoint(context, src, dst, random, index);
		}
		ObjectPool<List<sbyte>>.Instance.Return(list);
	}

	public void TransferAcupoint(DataContext context, CombatCharacter srcChar, CombatCharacter destChar, sbyte bodyPart, int index)
	{
		(sbyte, int, int) tuple = srcChar.GetAcupointCollection().BodyPartDict[bodyPart][index];
		RemoveAcupoint(context, srcChar, bodyPart, index, raiseEvent: false);
		destChar.AddOrUpdateFlawOrAcupoint(context, bodyPart, isFlaw: false, tuple.Item1, raiseEvent: true, tuple.Item3, tuple.Item2);
	}

	public void RemoveHalfFlawOrAcupoint(DataContext context, CombatCharacter combatChar, bool isFlaw)
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		DefeatMarkCollection defeatMarkCollection = combatChar.GetDefeatMarkCollection();
		int num = (isFlaw ? defeatMarkCollection.GetTotalFlawCount() : defeatMarkCollection.GetTotalAcupointCount());
		int count = num * CValueHalf.RoundUp;
		combatChar.RemoveRandomFlawOrAcupoint(context, isFlaw, count);
	}

	public int GetBrokenBodyPartCount(CombatCharacter character)
	{
		int num = 0;
		for (sbyte b = 0; b < 7; b++)
		{
			if (CheckBodyPartInjury(character, b))
			{
				num++;
			}
		}
		return num;
	}

	public void ChangeToOldInjury(DataContext context, CombatCharacter character, sbyte bodyPart, bool isInner, int count)
	{
		Injuries oldInjuries = character.GetOldInjuries();
		count = Math.Min(count, character.GetInjuries().Get(bodyPart, isInner) - oldInjuries.Get(bodyPart, isInner));
		oldInjuries.Change(bodyPart, isInner, (sbyte)count);
		character.SetOldInjuries(oldInjuries, context);
		SyncInjuryAutoHealCollection(context, character);
	}

	public int ChangeToOldInjury(DataContext context, CombatCharacter character, int count, Func<sbyte, bool> bodyPartFilter = null)
	{
		Injuries injuries = character.GetInjuries().Subtract(character.GetOldInjuries());
		List<(sbyte, bool)> cachedInjuryRandomPool = CachedInjuryRandomPool;
		for (sbyte b = 0; b < 7; b++)
		{
			if (bodyPartFilter == null || bodyPartFilter(b))
			{
				(sbyte, sbyte) tuple = injuries.Get(b);
				for (int i = 0; i < tuple.Item1; i++)
				{
					cachedInjuryRandomPool.Add((b, false));
				}
				for (int j = 0; j < tuple.Item2; j++)
				{
					cachedInjuryRandomPool.Add((b, true));
				}
			}
		}
		int num = Math.Min(cachedInjuryRandomPool.Count, count);
		for (int k = 0; k < num; k++)
		{
			int index = context.Random.Next(0, cachedInjuryRandomPool.Count);
			(sbyte, bool) tuple2 = cachedInjuryRandomPool[index];
			ChangeToOldInjury(context, character, tuple2.Item1, tuple2.Item2, 1);
			cachedInjuryRandomPool.RemoveAt(index);
		}
		cachedInjuryRandomPool.Clear();
		return num;
	}

	private void SyncInjuryAutoHealCollection(DataContext context, CombatCharacter character)
	{
		InjuryAutoHealCollection oldInjuryAutoHealCollection = character.GetOldInjuryAutoHealCollection();
		InjuryAutoHealCollection injuryAutoHealCollection = character.GetInjuryAutoHealCollection();
		Injuries injuries = character.GetOldInjuries();
		Injuries injuries2 = character.GetInjuries().Subtract(injuries);
		oldInjuryAutoHealCollection.SyncInjuries(ref injuries);
		injuryAutoHealCollection.SyncInjuries(ref injuries2);
		character.SetOldInjuryAutoHealCollection(oldInjuryAutoHealCollection, context);
		character.SetInjuryAutoHealCollection(injuryAutoHealCollection, context);
	}

	[DomainMethod]
	public uint GetHealInjuryBanReason(int doctorCharId, int patientCharId)
	{
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		if (!DomainManager.Character.TryGetElement_Objects(doctorCharId, out var element))
		{
			return 0u;
		}
		if (!DomainManager.Character.TryGetElement_Objects(patientCharId, out var element2))
		{
			return 0u;
		}
		return BoolArray32.op_Implicit(GetHealInjuryBanReason(element, element2));
	}

	[DomainMethod]
	public uint GetHealPoisonBanReason(int doctorCharId, int patientCharId)
	{
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		if (!DomainManager.Character.TryGetElement_Objects(doctorCharId, out var element))
		{
			return 0u;
		}
		if (!DomainManager.Character.TryGetElement_Objects(patientCharId, out var element2))
		{
			return 0u;
		}
		return BoolArray32.op_Implicit(GetHealPoisonBanReason(element, element2));
	}

	public BoolArray32 GetHealInjuryBanReason(CombatCharacter doctor, CombatCharacter patient)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		return GetHealInjuryBanReason(doctor.GetCharacter(), patient.GetCharacter());
	}

	public BoolArray32 GetHealInjuryBanReason(GameData.Domains.Character.Character doctor, GameData.Domains.Character.Character patient)
	{
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		BoolArray32 result = default(BoolArray32);
		if (patient.GetInjuries().HasAnyInjury())
		{
			int resource = doctor.GetResource(5);
			int num = patient.CalcHealCostHerb(EHealActionType.Healing);
			HealInjury(patient.GetId(), doctor, out var allHealMarkCount, out var maxHealMarkCount, out var maxRequireAttainment, canHealOld: true, getCost: true, checkHerb: true);
			((BoolArray32)(ref result))[2] = resource < num && allHealMarkCount <= 0;
			HealInjury(patient.GetId(), doctor, out var allHealMarkCount2, out maxRequireAttainment, out maxHealMarkCount, canHealOld: true, getCost: true);
			((BoolArray32)(ref result))[3] = allHealMarkCount2 <= 0;
		}
		else
		{
			((BoolArray32)(ref result))[0] = true;
		}
		return result;
	}

	public BoolArray32 GetHealPoisonBanReason(CombatCharacter doctor, CombatCharacter patient)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		return GetHealPoisonBanReason(doctor.GetCharacter(), patient.GetCharacter());
	}

	public BoolArray32 GetHealPoisonBanReason(GameData.Domains.Character.Character doctor, GameData.Domains.Character.Character patient)
	{
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		BoolArray32 result = default(BoolArray32);
		if (patient.GetPoisoned().IsNonZero())
		{
			int resource = doctor.GetResource(5);
			int num = patient.CalcHealCostHerb(EHealActionType.Detox);
			HealPoison(patient.GetId(), doctor, out var healMarkCount, out var healPoisonValue, out var maxRequireAttainment, canHealOld: true, getCost: true, checkHerb: true);
			((BoolArray32)(ref result))[2] = resource < num && healPoisonValue <= 0;
			HealPoison(patient.GetId(), doctor, out maxRequireAttainment, out var healPoisonValue2, out healMarkCount, canHealOld: true, getCost: true);
			((BoolArray32)(ref result))[3] = healPoisonValue2 <= 0;
		}
		else
		{
			((BoolArray32)(ref result))[0] = true;
		}
		return result;
	}

	public int GetMaxCanHealInjuryCount(int doctorCharId, int patientCharId)
	{
		if (!DomainManager.Character.TryGetElement_Objects(doctorCharId, out var element))
		{
			return 0;
		}
		HealInjury(patientCharId, element, out var _, out var maxHealMarkCount, out var _, canHealOld: true, getCost: true, checkHerb: true);
		return maxHealMarkCount;
	}

	public static int GetHealInjuryCostHerb(Injuries injuries)
	{
		int num = GlobalConfig.Instance.HealInjuryBaseHerb;
		for (sbyte b = 0; b < 7; b++)
		{
			(sbyte, sbyte) tuple = injuries.Get(b);
			if (tuple.Item1 > 0)
			{
				num += GlobalConfig.Instance.HealInjuryExtraHerb[tuple.Item1 - 1];
			}
			if (tuple.Item2 > 0)
			{
				num += GlobalConfig.Instance.HealInjuryExtraHerb[tuple.Item2 - 1];
			}
		}
		return num;
	}

	public static int GetHealInjuryCostMoney(Injuries injuries, sbyte doctorBehaviorType)
	{
		int num = GlobalConfig.Instance.HealInjuryBaseMoney;
		for (sbyte b = 0; b < 7; b++)
		{
			(sbyte, sbyte) tuple = injuries.Get(b);
			if (tuple.Item1 > 0)
			{
				num += GlobalConfig.Instance.HealInjuryExtraMoney[tuple.Item1 - 1];
			}
			if (tuple.Item2 > 0)
			{
				num += GlobalConfig.Instance.HealInjuryExtraMoney[tuple.Item2 - 1];
			}
		}
		return num * GlobalConfig.Instance.HealMoneyPercent[doctorBehaviorType] / 100;
	}

	public static int GetHealInjuryCostSpiritualDebt(Injuries injuries)
	{
		int num = 0;
		for (sbyte b = 0; b < 7; b++)
		{
			(sbyte, sbyte) tuple = injuries.Get(b);
			if (tuple.Item1 > 0)
			{
				num += GlobalConfig.Instance.HealInjuryCostSpiritualDebt[tuple.Item1 - 1];
			}
			if (tuple.Item2 > 0)
			{
				num += GlobalConfig.Instance.HealInjuryCostSpiritualDebt[tuple.Item2 - 1];
			}
		}
		return num;
	}

	public unsafe static int GetHealPoisonCostHerb(PoisonInts poisons)
	{
		int num = GlobalConfig.Instance.HealPoisonBaseHerb;
		for (sbyte b = 0; b < 6; b++)
		{
			int poisoned = poisons.Items[b];
			int num2 = PoisonsAndLevels.CalcPoisonedLevel(poisoned);
			if (num2 > 0)
			{
				num += GlobalConfig.Instance.HealPoisonExtraHerb[num2 - 1];
			}
		}
		return num;
	}

	public unsafe static int GetHealPoisonCostMoney(PoisonInts poisons, sbyte doctorBehaviorType)
	{
		int num = GlobalConfig.Instance.HealPoisonBaseMoney;
		for (sbyte b = 0; b < 6; b++)
		{
			int poisoned = poisons.Items[b];
			int num2 = PoisonsAndLevels.CalcPoisonedLevel(poisoned);
			if (num2 > 0)
			{
				num += GlobalConfig.Instance.HealPoisonExtraMoney[num2 - 1];
			}
		}
		return num * GlobalConfig.Instance.HealMoneyPercent[doctorBehaviorType] / 100;
	}

	public unsafe static int GetHealPoisonCostSpiritualDebt(PoisonInts poisons)
	{
		int num = 0;
		for (sbyte b = 0; b < 6; b++)
		{
			int poisoned = poisons.Items[b];
			int num2 = PoisonsAndLevels.CalcPoisonedLevel(poisoned);
			if (num2 > 0)
			{
				num += GlobalConfig.Instance.HealPoisonExtraSpiritualDebt[num2 - 1];
			}
		}
		return num;
	}

	public static int GetHealQiDisorderCostHerb(short qiDisorder)
	{
		return GlobalConfig.Instance.HealQiDisorderHerb[DisorderLevelOfQi.GetDisorderLevelOfQi(qiDisorder)];
	}

	public static int GetHealQiDisorderCostMoney(short qiDisorder, sbyte doctorBehaviorType)
	{
		return GlobalConfig.Instance.HealQiDisorderMoney[DisorderLevelOfQi.GetDisorderLevelOfQi(qiDisorder)] * GlobalConfig.Instance.HealMoneyPercent[doctorBehaviorType];
	}

	public static int GetHealQiDisorderCostSpiritualDebt(short qiDisorder)
	{
		return GlobalConfig.Instance.HealQiDisorderCostSpiritualDebt[DisorderLevelOfQi.GetDisorderLevelOfQi(qiDisorder)];
	}

	public static int GetHealHealthCostHerb(EHealthType healthType)
	{
		int index = healthType.ToCommonIndex();
		return GlobalConfig.Instance.HealHealthHerb.GetOrDefault(index);
	}

	public static int GetHealHealthCostMoney(EHealthType healthType, sbyte doctorBehaviorType)
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		int index = healthType.ToCommonIndex();
		CValuePercent val = CValuePercent.op_Implicit((int)GlobalConfig.Instance.HealMoneyPercent[doctorBehaviorType]);
		return (int)GlobalConfig.Instance.HealHealthMoney.GetOrDefault(index) * val;
	}

	public static int GetHealHealthCostSpiritualDebt(EHealthType healthType)
	{
		int index = healthType.ToCommonIndex();
		return GlobalConfig.Instance.HealHealthCostSpiritualDebt.GetOrDefault(index);
	}

	public sbyte HealInjuryInCombat(DataContext context, CombatCharacter patient, CombatCharacter doctor, bool canHealOld = true)
	{
		int healInjuryCostHerb = GetHealInjuryCostHerb(patient.GetInjuries());
		int resource = doctor.GetCharacter().GetResource(5);
		if (healInjuryCostHerb > resource)
		{
			ShowSpecialEffectTips(patient.GetId(), 1458, 0);
		}
		Dictionary<int, int> dictionary = ObjectPool<Dictionary<int, int>>.Instance.Get();
		Dictionary<int, int> dictionary2 = ObjectPool<Dictionary<int, int>>.Instance.Get();
		dictionary.Clear();
		dictionary2.Clear();
		int allHealMarkCount;
		int maxHealMarkCount;
		int maxRequireAttainment;
		Injuries injuries = HealInjury(patient.GetId(), doctor.GetCharacter(), out allHealMarkCount, out maxHealMarkCount, out maxRequireAttainment, canHealOld, getCost: false, checkHerb: true, dictionary, dictionary2);
		doctor.GetCharacter().ChangeResource(context, 5, -Math.Min(healInjuryCostHerb, resource));
		if (allHealMarkCount > 0)
		{
			SetInjuries(context, patient, injuries);
		}
		if (dictionary.Count > 0)
		{
			int[] innerDamageValue = patient.GetInnerDamageValue();
			foreach (KeyValuePair<int, int> item in dictionary)
			{
				item.Deconstruct(out maxRequireAttainment, out maxHealMarkCount);
				int num = maxRequireAttainment;
				int num2 = maxHealMarkCount;
				innerDamageValue[num] = num2;
			}
			patient.SetInnerDamageValue(innerDamageValue, context);
		}
		if (dictionary2.Count > 0)
		{
			int[] outerDamageValue = patient.GetOuterDamageValue();
			foreach (KeyValuePair<int, int> item2 in dictionary2)
			{
				item2.Deconstruct(out maxHealMarkCount, out maxRequireAttainment);
				int num3 = maxHealMarkCount;
				int num4 = maxRequireAttainment;
				outerDamageValue[num3] = num4;
			}
			patient.SetOuterDamageValue(outerDamageValue, context);
		}
		Events.RaiseHealedInjury(context, doctor.GetId(), patient.GetId(), patient.IsAlly, (sbyte)allHealMarkCount);
		return (sbyte)allHealMarkCount;
	}

	public sbyte HealPoisonInCombat(DataContext context, CombatCharacter patient, CombatCharacter doctor, bool canHealOld = true)
	{
		int healPoisonCostHerb = GetHealPoisonCostHerb(patient.GetPoison());
		int resource = doctor.GetCharacter().GetResource(5);
		if (healPoisonCostHerb > resource)
		{
			ShowSpecialEffectTips(patient.GetId(), 1460, 0);
		}
		int healMarkCount;
		int healPoisonValue;
		int maxRequireAttainment;
		PoisonInts poisons = HealPoison(patient.GetId(), doctor.GetCharacter(), out healMarkCount, out healPoisonValue, out maxRequireAttainment, canHealOld, getCost: false, checkHerb: true);
		doctor.GetCharacter().ChangeResource(context, 5, -Math.Min(healPoisonCostHerb, resource));
		SetPoisons(context, patient, poisons);
		Events.RaiseHealedPoison(context, doctor.GetId(), patient.GetId(), patient.IsAlly, (sbyte)healMarkCount);
		return (sbyte)healMarkCount;
	}

	public Injuries HealInjury(int patientId, GameData.Domains.Character.Character doctor, bool isExpensiveHeal = false)
	{
		int allHealMarkCount;
		int maxHealMarkCount;
		int maxRequireAttainment;
		return HealInjury(patientId, doctor, out allHealMarkCount, out maxHealMarkCount, out maxRequireAttainment, canHealOld: true, getCost: false, checkHerb: false, null, null, isExpensiveHeal);
	}

	public Injuries HealInjury(int patientId, GameData.Domains.Character.Character doctor, out int allHealMarkCount, out int maxHealMarkCount, out int maxRequireAttainment, bool canHealOld = true, bool getCost = false, bool checkHerb = false, Dictionary<int, int> changedInnerDamageValue = null, Dictionary<int, int> changedOuterDamageValue = null, bool isExpensiveHeal = false)
	{
		HealInjuryCalcOriginal(patientId, out var injuries, out var oldInjuries, out var damageSteps);
		HealInjuryCalcExtraAddPercent(patientId, doctor, checkHerb, injuries, out var extraBuffAddPercent, out var extraInnerDebuffAddPercent, out var extraOuterDebuffAddPercent);
		CombatCharacter patient;
		bool inCombat = _combatCharacterDict.TryGetValue(patientId, out patient) && IsInCombat();
		int doctorAttainment = doctor.CalcHealAttainment(EHealActionType.Healing);
		if (isExpensiveHeal)
		{
			doctorAttainment *= 2;
		}
		allHealMarkCount = 0;
		maxHealMarkCount = 0;
		maxRequireAttainment = 0;
		for (sbyte b = 0; b < 7; b++)
		{
			HealPartInjury(b, isInner: true, ref allHealMarkCount, ref maxHealMarkCount, ref maxRequireAttainment);
			HealPartInjury(b, isInner: false, ref allHealMarkCount, ref maxHealMarkCount, ref maxRequireAttainment);
		}
		return injuries;
		void HealPartInjury(sbyte bodyPart, bool isInner, ref int reference2, ref int reference3, ref int reference)
		{
			int num = ((patient != null) ? (isInner ? patient.GetInnerDamageValue() : patient.GetOuterDamageValue())[bodyPart] : 0);
			int num2 = (isInner ? damageSteps.InnerDamageSteps : damageSteps.OuterDamageSteps)[bodyPart];
			sbyte b2 = injuries.Get(bodyPart, isInner);
			int num3 = b2 * num2 + num;
			if (num3 > 0)
			{
				int num4 = CFormula.CalcHealInjuryValue(doctorAttainment, num2, b2, out var requireAttainment);
				reference = Math.Max(reference, requireAttainment);
				if (num4 > 0)
				{
					int extraAddPercent = (isInner ? extraInnerDebuffAddPercent : extraOuterDebuffAddPercent);
					num4 = DomainManager.SpecialEffect.ModifyValue(doctor.GetId(), 120, num4, getCost ? 1 : 0, -1, -1, 0, extraAddPercent);
					int num5 = oldInjuries.Get(bodyPart, isInner) * num2;
					int num6 = num3 - num5;
					if (num4 < num6)
					{
						num4 = Math.Min(DomainManager.SpecialEffect.ModifyValue(doctor.GetId(), 119, num4, getCost ? 1 : 0, -1, -1, 0, extraBuffAddPercent), num6);
					}
					int num7 = num4;
					num4 = Math.Min(num4, canHealOld ? num3 : (num3 - num5));
					int num8 = num4 / num2;
					Dictionary<int, int> dictionary = (isInner ? changedInnerDamageValue : changedOuterDamageValue);
					if (inCombat && dictionary != null)
					{
						int num9 = num;
						int num10 = num4 % num2;
						num9 -= num10;
						if (num9 < 0)
						{
							num8++;
							num9 += num2;
						}
						if (num9 != num)
						{
							dictionary[bodyPart] = num9;
						}
					}
					injuries.Change(bodyPart, isInner, (sbyte)(-num8));
					reference2 += num8;
					reference3 += num7 / num2;
				}
			}
		}
	}

	private void HealInjuryCalcOriginal(int patientId, out Injuries injuries, out Injuries oldInjuries, out DamageStepCollection damageSteps)
	{
		if (IsCharInCombat(patientId))
		{
			CombatCharacter combatCharacter = _combatCharacterDict[patientId];
			injuries = combatCharacter.GetInjuries();
			oldInjuries = combatCharacter.GetOldInjuries();
			damageSteps = combatCharacter.GetDamageStepCollection();
		}
		else
		{
			GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(patientId);
			injuries = (oldInjuries = element_Objects.GetInjuries());
			damageSteps = GetDamageStepCollection(patientId);
		}
	}

	private void HealInjuryCalcExtraAddPercent(int patientId, GameData.Domains.Character.Character doctor, bool checkHerb, Injuries injuries, out int extraBuffAddPercent, out int extraInnerDebuffAddPercent, out int extraOuterDebuffAddPercent)
	{
		//IL_0104: Unknown result type (might be due to invalid IL or missing references)
		extraBuffAddPercent = 0;
		extraInnerDebuffAddPercent = 0;
		extraOuterDebuffAddPercent = 0;
		if (IsCharInCombat(patientId))
		{
			CombatCharacter combatCharacter = _combatCharacterDict[doctor.GetId()];
			if (combatCharacter.ExecutingTeammateCommandImplement == ETeammateCommandImplement.HealInjury)
			{
				extraBuffAddPercent += DomainManager.SpecialEffect.GetModifyValue(patientId, 184, (EDataModifyType)0, 6, -1, -1, (EDataSumType)0);
			}
		}
		foreach (SolarTermItem item in doctor.GetInvokedSolarTerm())
		{
			if (item.InnerHealingBuff)
			{
				extraInnerDebuffAddPercent += doctor.GetSolarTermValue(GlobalConfig.Instance.SolarTermAddHealInnerInjury);
			}
			if (item.OuterHealingBuff)
			{
				extraOuterDebuffAddPercent += doctor.GetSolarTermValue(GlobalConfig.Instance.SolarTermAddHealOuterInjury);
			}
		}
		if (checkHerb)
		{
			int healInjuryCostHerb = GetHealInjuryCostHerb(injuries);
			int resource = doctor.GetResource(5);
			if (healInjuryCostHerb > resource)
			{
				int num = -(int)CValuePercent.Parse(healInjuryCostHerb - resource, healInjuryCostHerb);
				extraInnerDebuffAddPercent += num;
				extraOuterDebuffAddPercent += num;
			}
		}
	}

	public PoisonInts HealPoison(int patientId, GameData.Domains.Character.Character doctor, bool isExpensiveHeal = false)
	{
		int healMarkCount;
		int healPoisonValue;
		int maxRequireAttainment;
		return HealPoison(patientId, doctor, out healMarkCount, out healPoisonValue, out maxRequireAttainment, canHealOld: true, getCost: false, checkHerb: false, isExpensiveHeal);
	}

	public PoisonInts HealPoison(int patientId, GameData.Domains.Character.Character doctor, out int healMarkCount, out int healPoisonValue, out int maxRequireAttainment, bool canHealOld = true, bool getCost = false, bool checkHerb = false, bool isExpensiveHeal = false)
	{
		HealPoisonCalcOriginal(patientId, out var poisons, out var oldPoisons);
		HealPoisonCalcExtraAddPercent(patientId, doctor, checkHerb, poisons, out var extraBuffAddPercent, out var extraDebuffAddPercent);
		int num = doctor.CalcHealAttainment(EHealActionType.Detox);
		if (isExpensiveHeal)
		{
			num *= 2;
		}
		healMarkCount = 0;
		healPoisonValue = 0;
		maxRequireAttainment = 0;
		for (sbyte b = 0; b < 6; b++)
		{
			int num2 = poisons[b];
			if (num2 > 0)
			{
				sbyte b2 = PoisonsAndLevels.CalcPoisonedLevel(num2);
				int num3 = CFormula.CalcHealPoisonValue(num, b2, out var requireAttainment);
				maxRequireAttainment = Math.Max(maxRequireAttainment, requireAttainment);
				if (num3 > 0)
				{
					int num4 = extraDebuffAddPercent;
					foreach (SolarTermItem item in doctor.GetInvokedSolarTerm())
					{
						if (item.DetoxBuffType == b)
						{
							num4 += doctor.GetSolarTermValue(GlobalConfig.Instance.SolarTermAddHealPoison);
						}
					}
					num3 = DomainManager.SpecialEffect.ModifyValue(doctor.GetId(), 123, num3, getCost ? 1 : 0, -1, -1, 0, num4);
					int num5 = oldPoisons[b];
					int num6 = num2 - num5;
					if (num3 < num6)
					{
						num3 = Math.Min(DomainManager.SpecialEffect.ModifyValue(doctor.GetId(), 122, num3, getCost ? 1 : 0, -1, -1, 0, extraBuffAddPercent), num6);
					}
					num3 = ApplyReducePoisonEffect(patientId, b, num3, getCost);
					num3 = Math.Min(num3, canHealOld ? num2 : (num2 - num5));
					poisons[b] -= num3;
					healMarkCount += b2 - PoisonsAndLevels.CalcPoisonedLevel(num2 - num3);
					healPoisonValue += num3;
				}
			}
		}
		return poisons;
	}

	private void HealPoisonCalcOriginal(int patientId, out PoisonInts poisons, out PoisonInts oldPoisons)
	{
		if (IsCharInCombat(patientId))
		{
			CombatCharacter combatCharacter = _combatCharacterDict[patientId];
			poisons = combatCharacter.GetPoison();
			oldPoisons = combatCharacter.GetOldPoison();
		}
		else
		{
			GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(patientId);
			poisons = (oldPoisons = element_Objects.GetPoisoned());
		}
	}

	private void HealPoisonCalcExtraAddPercent(int patientId, GameData.Domains.Character.Character doctor, bool checkHerb, PoisonInts poisons, out int extraBuffAddPercent, out int extraDebuffAddPercent)
	{
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		extraBuffAddPercent = 0;
		extraDebuffAddPercent = 0;
		if (IsCharInCombat(patientId))
		{
			CombatCharacter combatCharacter = _combatCharacterDict[doctor.GetId()];
			if (combatCharacter.ExecutingTeammateCommandImplement == ETeammateCommandImplement.HealPoison)
			{
				extraBuffAddPercent += DomainManager.SpecialEffect.GetModifyValue(patientId, 184, (EDataModifyType)0, 7, -1, -1, (EDataSumType)0);
			}
		}
		if (checkHerb)
		{
			int healPoisonCostHerb = GetHealPoisonCostHerb(poisons);
			int resource = doctor.GetResource(5);
			if (healPoisonCostHerb > resource)
			{
				extraDebuffAddPercent = -(int)CValuePercent.Parse(healPoisonCostHerb - resource, healPoisonCostHerb);
			}
		}
	}

	public short HealQiDisorder(int patientId, GameData.Domains.Character.Character doctor, bool isExpensiveHeal = false)
	{
		short disorderOfQi = DomainManager.Character.GetElement_Objects(patientId).GetDisorderOfQi();
		int num = doctor.CalcHealAttainment(EHealActionType.Breathing);
		if (isExpensiveHeal)
		{
			num *= 2;
		}
		sbyte disorderLevelOfQi = DisorderLevelOfQi.GetDisorderLevelOfQi(disorderOfQi);
		int num2 = CFormula.CalcHealQiDisorderValue(num, disorderLevelOfQi);
		return (short)Math.Clamp(disorderOfQi - num2, DisorderLevelOfQi.MinValue, DisorderLevelOfQi.MaxValue);
	}

	public short HealHealth(int patientId, GameData.Domains.Character.Character doctor, bool isExpensiveHeal = false)
	{
		GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(patientId);
		short health = element_Objects.GetHealth();
		int num = doctor.CalcHealAttainment(EHealActionType.Recover);
		if (isExpensiveHeal)
		{
			num *= 2;
		}
		EHealthType healthType = element_Objects.GetHealthType();
		int num2 = CFormula.CalcHealHealthValue(num, healthType);
		return (short)Math.Clamp(health + num2, 0, Math.Max((int)element_Objects.GetLeftMaxHealth(), 0));
	}

	private static bool IsSwordFragment(ItemKey itemKey)
	{
		return itemKey.ItemType == 12 && SharedConstValue.SwordFragment2BossId.ContainsKey(itemKey.TemplateId);
	}

	[DomainMethod]
	public List<short> RequestSwordFragmentSkillIds()
	{
		GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
		List<short> list = null;
		foreach (ItemKey item in taiwu.GetInventory().Items.Keys.Where(IsSwordFragment))
		{
			short swordFragmentCurrSkill = DomainManager.Item.GetSwordFragmentCurrSkill(item);
			if (swordFragmentCurrSkill >= 0)
			{
				if (list == null)
				{
					list = new List<short>();
				}
				list.Add(swordFragmentCurrSkill);
			}
		}
		return list;
	}

	[DomainMethod]
	public List<ItemDisplayData> RequestValidItemsInCombat(int charId)
	{
		if (!IsCharInCombat(charId))
		{
			return null;
		}
		CombatCharacter element_CombatCharacterDict = GetElement_CombatCharacterDict(charId);
		List<ItemDisplayData> list = null;
		foreach (var (itemKey2, amount) in element_CombatCharacterDict.GetValidItemAndCounts())
		{
			if (list == null)
			{
				list = new List<ItemDisplayData>();
			}
			ItemDisplayData itemDisplayData = (itemKey2.IsValid() ? DomainManager.Item.GetItemDisplayData(itemKey2, charId) : new ItemDisplayData(itemKey2.ItemType, itemKey2.TemplateId));
			itemDisplayData.Amount = amount;
			list.Add(itemDisplayData);
		}
		return list;
	}

	[DomainMethod]
	public void UseItem(DataContext context, ItemKey itemKey, sbyte useType = -1, bool isAlly = true, List<sbyte> targetBodyParts = null)
	{
		if (!CanAcceptCommand())
		{
			return;
		}
		CombatCharacter combatCharacter = (isAlly ? _selfChar : _enemyChar);
		if (!combatCharacter.GetCanUseItem() || !combatCharacter.GetValidItems().Contains(itemKey))
		{
			return;
		}
		int consumedFeatureMedals = itemKey.GetConsumedFeatureMedals();
		if (consumedFeatureMedals <= (isAlly ? _selfTeamWisdomCount : _enemyTeamWisdomCount))
		{
			bool flag = !combatCharacter.HasDoingOrReserveCommand();
			combatCharacter.SetNeedUseItem(context, itemKey);
			combatCharacter.ItemUseType = useType;
			combatCharacter.ItemTargetBodyParts = targetBodyParts;
			combatCharacter.MoveData.ResetJumpState(context);
			if (flag)
			{
				UpdateAllCommandAvailability(context, combatCharacter);
			}
			else
			{
				UpdateCanUseItem(context, combatCharacter);
			}
		}
	}

	[DomainMethod]
	public void RepairItem(DataContext context, ItemKey toolKey, ItemKey targetKey, bool isAlly = true)
	{
		if (!CanAcceptCommand())
		{
			return;
		}
		CombatCharacter combatCharacter = (isAlly ? _selfChar : _enemyChar);
		if (combatCharacter.GetCanUseItem())
		{
			bool flag = !combatCharacter.HasDoingOrReserveCommand();
			combatCharacter.SetNeedUseItem(context, toolKey);
			combatCharacter.NeedRepairItem = targetKey;
			combatCharacter.MoveData.ResetJumpState(context);
			if (flag)
			{
				UpdateAllCommandAvailability(context, combatCharacter);
			}
			else
			{
				UpdateCanUseItem(context, combatCharacter);
			}
		}
	}

	[DomainMethod]
	public void UseSpecialItem(DataContext context, sbyte itemType, short templateId, bool isAlly = true)
	{
		if (!CanAcceptCommand())
		{
			return;
		}
		CombatCharacter combatCharacter = (isAlly ? _selfChar : _enemyChar);
		ItemKey? itemKey = null;
		foreach (ItemKey validItem in combatCharacter.GetValidItems())
		{
			if (validItem.TemplateEquals(itemType, templateId))
			{
				itemKey = validItem;
			}
		}
		if (itemKey.HasValue)
		{
			UseItem(context, itemKey.Value, -1);
		}
	}

	public bool IsInfectedCombat()
	{
		GameData.Domains.Character.Character character = GetMainCharacter(isAlly: false).GetCharacter();
		if (character.GetCreatingType() != 1)
		{
			return false;
		}
		if (!character.GetFeatureIds().Contains(218))
		{
			return false;
		}
		return CombatConfig.TemplateId == 193;
	}

	public void ChangeWisdom(DataContext context, bool isAlly, int delta)
	{
		short num = (isAlly ? _selfTeamWisdomCount : _enemyTeamWisdomCount);
		short num2 = (short)Math.Clamp(num + delta, 0, 32767);
		if (num2 != num)
		{
			if (isAlly)
			{
				SetSelfTeamWisdomCount(num2, context);
			}
			else
			{
				SetEnemyTeamWisdomCount(num2, context);
			}
		}
	}

	public bool CostWisdom(DataContext context, bool isAlly, int costValue)
	{
		short num = (isAlly ? _selfTeamWisdomCount : _enemyTeamWisdomCount);
		if (num < costValue || costValue <= 0)
		{
			return false;
		}
		short value = (short)Math.Clamp(num - costValue, 0, 32767);
		if (isAlly)
		{
			SetSelfTeamWisdomCount(value, context);
		}
		else
		{
			SetEnemyTeamWisdomCount(value, context);
		}
		Events.RaiseWisdomCosted(context, isAlly, costValue);
		return true;
	}

	public void InitEquipmentDurability()
	{
		EquipmentOldDurability.Clear();
		foreach (CombatCharacter value in _combatCharacterDict.Values)
		{
			foreach (ItemKey item in from x in value.GetCharacter().GetEquipment()
				where x.IsValid()
				select x)
			{
				EquipmentOldDurability.Add(item, DomainManager.Item.GetBaseItem(item).GetCurrDurability());
			}
		}
	}

	public void EnsureOldDurability(ItemKey key)
	{
		if (key.IsValid())
		{
			short currDurability = DomainManager.Item.GetBaseItem(key).GetCurrDurability();
			if (EquipmentOldDurability.ContainsKey(key))
			{
				EquipmentOldDurability[key] = Math.Max(EquipmentOldDurability[key], currDurability);
			}
		}
	}

	private void UpdateCanUseItem(DataContext context, CombatCharacter character)
	{
		if (!character.GetCanUseItem())
		{
			character.SetCanUseItem(canUseItem: true, context);
			if (character.IsAlly)
			{
				UpdateShowUseSpecialMisc(context);
			}
		}
	}

	public void ClearShowUseSpecialMisc(DataContext context)
	{
		SetShowUseGoldenWire(0, context);
	}

	public void UpdateShowUseSpecialMisc(DataContext context)
	{
		UpdateShowUseGoldenWire(context);
	}

	private bool CalcShowUseSpecialMiscCommon(short miscTemplateId)
	{
		if (!IsMainCharacter(_selfChar) || !IsMainCharacter(_enemyChar))
		{
			return false;
		}
		if (!_selfChar.GetCanUseItem())
		{
			return false;
		}
		if (_selfChar.GetCharacter().GetInventory().GetInventoryItemCount(12, miscTemplateId) == 0)
		{
			return false;
		}
		if (_enemyChar.TeammateBeforeMainChar >= 0)
		{
			return false;
		}
		MiscItem miscItem = Config.Misc.Instance[miscTemplateId];
		List<short> requireCombatConfig = miscItem.RequireCombatConfig;
		if (requireCombatConfig != null && requireCombatConfig.Count > 0 && !miscItem.RequireCombatConfig.Contains(CombatConfig.TemplateId))
		{
			return false;
		}
		return _currentDistance <= miscItem.MaxUseDistance;
	}

	private void UpdateShowUseGoldenWire(DataContext context)
	{
		if (_selfChar != null && _enemyChar != null)
		{
			short num = 285;
			int num2 = (CalcShowUseSpecialMiscCommon(num) ? CalcRopeHitOdds(Config.Misc.Instance[num].Grade) : 0);
			if (_showUseGoldenWire != num2)
			{
				SetShowUseGoldenWire(num2, context);
			}
		}
	}

	private static int CalcCaptureRateBonus(ItemKey equipKey)
	{
		return (equipKey.ItemType == 2) ? Config.Accessory.Instance[equipKey.TemplateId].BaseCaptureRateBonus : DomainManager.Item.GetElement_Carriers(equipKey.Id).GetCaptureRateBonus();
	}

	public bool CheckRopeHit(IRandomSource random, int ropeGrade)
	{
		int percentProb = CalcRopeHitOdds(ropeGrade);
		return random.CheckPercentProb(percentProb);
	}

	private int CalcRopeHitOdds(int ropeGrade)
	{
		CombatCharacter combatCharacter = GetCombatCharacter(isAlly: false, tryGetCoverCharacter: true);
		int totalCount = combatCharacter.GetDefeatMarkCollection().GetTotalCount();
		sbyte b = CFormula.CalcRopeRequireMinMarkCount((CombatType)_combatType);
		if (!IsMainCharacter(combatCharacter) || combatCharacter.TeammateBeforeMainChar >= 0 || combatCharacter.BossConfig != null)
		{
			return 0;
		}
		GameData.Domains.Character.Character character = combatCharacter.GetCharacter();
		if (!Config.Character.Instance[character.GetTemplateId()].CanBeKidnapped || CombatConfig.CaptureRate <= 0)
		{
			return 0;
		}
		if (_selfChar.GetCharacter().GetConsummateLevel() >= combatCharacter.GetCharacter().GetConsummateLevel() + 6)
		{
			return 100;
		}
		if (totalCount < b)
		{
			return 0;
		}
		HitOrAvoidInts hitValues = _selfChar.GetCharacter().GetHitValues();
		HitOrAvoidInts avoidValues = combatCharacter.GetCharacter().GetAvoidValues();
		ItemKey[] equipment = _selfChar.GetCharacter().GetEquipment();
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		for (sbyte b2 = 0; b2 < 4; b2++)
		{
			num += hitValues[b2];
			num2 += avoidValues[b2];
		}
		num2 = Math.Max(num2, 1);
		for (int i = 0; i < AddCaptureRateEquipSlot.Length; i++)
		{
			ItemKey itemKey = equipment[AddCaptureRateEquipSlot[i]];
			if (itemKey.IsValid())
			{
				EquipmentBase baseEquipment = DomainManager.Item.GetBaseEquipment(itemKey);
				if (baseEquipment.GetCurrDurability() > 0)
				{
					num3 += CalcCaptureRateBonus(itemKey);
				}
			}
		}
		if (ropeGrade >= 0)
		{
			num3 += GlobalConfig.Instance.CaptureRatePerRopeGrade * (ropeGrade + 1);
		}
		sbyte baseHitOdds = CFormula.CalcRopeBaseHitOdds((CombatType)_combatType);
		int num4 = CFormula.FormulaCalcRopeHitOdds(baseHitOdds, b, num, num2, num3, totalCount);
		if (ropeGrade >= 0)
		{
			num4 = num4 * CombatConfig.CaptureRate / 100;
		}
		return num4;
	}

	public unsafe static bool CheckRopeHitOutOfCombat(IRandomSource random, GameData.Domains.Character.Character useChar, GameData.Domains.Character.Character targetChar, sbyte combatType, bool useMaxMarkCount = true, int ropeGrade = -1)
	{
		int num = (useMaxMarkCount ? GlobalConfig.NeedDefeatMarkCount[combatType] : GetDefeatMarksCountOutOfCombat(targetChar));
		sbyte b = CFormula.CalcRopeRequireMinMarkCount((CombatType)combatType);
		if (num < b)
		{
			return false;
		}
		if (!Config.Character.Instance[targetChar.GetTemplateId()].CanBeKidnapped)
		{
			return false;
		}
		HitOrAvoidInts hitValues = useChar.GetHitValues();
		HitOrAvoidInts avoidValues = targetChar.GetAvoidValues();
		ItemKey[] equipment = useChar.GetEquipment();
		sbyte baseHitOdds = CFormula.CalcRopeBaseHitOdds((CombatType)combatType);
		int num2 = 0;
		int num3 = 0;
		int num4 = 0;
		for (sbyte b2 = 0; b2 < 4; b2++)
		{
			num2 += hitValues.Items[b2];
			num3 += avoidValues.Items[b2];
		}
		num3 = Math.Max(num3, 1);
		for (int i = 0; i < AddCaptureRateEquipSlot.Length; i++)
		{
			ItemKey itemKey = equipment[AddCaptureRateEquipSlot[i]];
			if (itemKey.IsValid())
			{
				EquipmentBase baseEquipment = DomainManager.Item.GetBaseEquipment(itemKey);
				if (baseEquipment.GetCurrDurability() > 0)
				{
					num4 += CalcCaptureRateBonus(itemKey);
				}
			}
		}
		if (ropeGrade >= 0)
		{
			num4 += GlobalConfig.Instance.CaptureRatePerRopeGrade * (ropeGrade + 1);
		}
		int percentProb = CFormula.FormulaCalcRopeHitOdds(baseHitOdds, b, num2, num3, num4, num);
		return random.CheckPercentProb(percentProb);
	}

	public void ChangeEquipmentPowerInCombat(int charId, int delta)
	{
		EquipmentPowerChangeInCombat[charId] = EquipmentPowerChangeInCombat.GetOrDefault(charId) + delta;
	}

	[DomainMethod]
	public void ChangeWeapon(DataContext context, int weaponIndex, bool isAlly = true, bool forceChange = false)
	{
		if (!CanAcceptCommand())
		{
			return;
		}
		CombatCharacter combatCharacter = (isAlly ? _selfChar : _enemyChar);
		ItemKey itemKey = combatCharacter.GetWeapons()[weaponIndex];
		if (TryGetElement_WeaponDataDict(itemKey.Id, out var element) && (element.GetCanChangeTo() || forceChange))
		{
			bool flag = !combatCharacter.HasDoingOrReserveCommand();
			combatCharacter.SetNeedChangeWeaponIndex(context, weaponIndex);
			if (flag)
			{
				UpdateAllCommandAvailability(context, combatCharacter);
			}
		}
	}

	[DomainMethod]
	public void NormalAttack(DataContext context, bool isAlly = true)
	{
		if (CanAcceptCommand() && CanNormalAttack(isAlly))
		{
			CombatCharacter combatCharacter = (isAlly ? _selfChar : _enemyChar);
			combatCharacter.SetReserveNormalAttack(reserveNormalAttack: true, context);
			UpdateAllCommandAvailability(context, combatCharacter);
		}
	}

	[DomainMethod]
	public void NormalAttackImmediate(DataContext context, bool isAlly = true)
	{
		if (CanAcceptCommand() && CanNormalAttack(isAlly))
		{
			CombatCharacter combatCharacter = (isAlly ? _selfChar : _enemyChar);
			if (combatCharacter.CanNormalAttackImmediate)
			{
				combatCharacter.NeedNormalAttackImmediate = true;
				UpdateAllCommandAvailability(context, combatCharacter);
			}
		}
	}

	[DomainMethod]
	public void UnlockAttack(DataContext context, int index, bool isAlly = true)
	{
		if (CanAcceptCommand() && CanUnlockAttack(isAlly, index))
		{
			(isAlly ? _selfChar : _enemyChar).SetNeedUnlockWeaponIndex(context, index);
		}
	}

	[DomainMethod]
	public ChangeTrickDisplayData GetChangeTrickDisplayData(bool isAlly = true)
	{
		CombatCharacter combatCharacter = (isAlly ? _selfChar : _enemyChar);
		GameData.Domains.Item.Weapon element_Weapons = DomainManager.Item.GetElement_Weapons(GetUsingWeaponKey(combatCharacter).Id);
		sbyte attackPreparePointCost = element_Weapons.GetAttackPreparePointCost();
		return new ChangeTrickDisplayData
		{
			CanChangeTrick = (combatCharacter.GetChangeTrickCount() > attackPreparePointCost),
			CostCount = (sbyte)(attackPreparePointCost + 1),
			AddHitRate = (short)(100 + GlobalConfig.Instance.AttackChangeTrickHitValueAddPercent[attackPreparePointCost]),
			AddBreakBlock = GlobalConfig.Instance.AttackChangeTrickCostBlockBasePercent[attackPreparePointCost]
		};
	}

	[DomainMethod]
	public void StartChangeTrick(DataContext context, bool isAlly = true)
	{
		if (CanAcceptCommand())
		{
			CombatCharacter combatCharacter = (isAlly ? _selfChar : _enemyChar);
			if (combatCharacter.GetCanChangeTrick())
			{
				combatCharacter.SetNeedShowChangeTrick(context, needShowChangeTrick: true);
				UpdateAllCommandAvailability(context, combatCharacter);
			}
		}
	}

	[DomainMethod]
	public void SelectChangeTrick(DataContext context, sbyte trickType, sbyte bodyPart, int flawOrAcupointType)
	{
		_selfChar.PlayerChangeTrickType = trickType;
		_selfChar.PlayerChangeTrickBodyPart = bodyPart;
		SelectChangeTrick(context, trickType, bodyPart, isAlly: true, (EFlawOrAcupointType)flawOrAcupointType);
	}

	public void SelectChangeTrick(DataContext context, sbyte trickType, sbyte bodyPart, bool isAlly = true, EFlawOrAcupointType flawOrAcupointType = EFlawOrAcupointType.None)
	{
		CombatCharacter combatCharacter = (isAlly ? _selfChar : _enemyChar);
		combatCharacter.SetNeedShowChangeTrick(context, needShowChangeTrick: false);
		int num = CFormulaHelper.CalcCostChangeTrickCount(combatCharacter, flawOrAcupointType);
		if (combatCharacter.GetChangeTrickCount() >= num)
		{
			ChangeChangeTrickCount(context, combatCharacter, -num, bySelectChangeTrick: true);
			combatCharacter.NeedChangeTrickAttack = true;
			combatCharacter.ChangeTrickType = trickType;
			combatCharacter.ChangeTrickBodyPart = bodyPart;
			combatCharacter.ChangeTrickFlawOrAcupointType = flawOrAcupointType;
			combatCharacter.MoveData.ResetJumpState(context);
			UpdateAllCommandAvailability(context, combatCharacter);
		}
	}

	[DomainMethod]
	public void CancelChangeTrick(DataContext context, bool isAlly = true)
	{
		CombatCharacter combatCharacter = (isAlly ? _selfChar : _enemyChar);
		combatCharacter.SetNeedShowChangeTrick(context, needShowChangeTrick: false);
		if (combatCharacter.StateMachine.GetCurrentStateType() != CombatCharacterStateType.SelectChangeTrick)
		{
			UpdateAllCommandAvailability(context, combatCharacter);
		}
	}

	[DomainMethod]
	public void ChangeTaiwuWeaponInnerRatio(DataContext context, int index, sbyte expectInnerRatio)
	{
		if (IsCharInCombat(DomainManager.Taiwu.GetTaiwuCharId()))
		{
			if (_selfChar.IsTaiwu)
			{
				DomainManager.Taiwu.SetWeaponInnerRatio(context, index, expectInnerRatio);
				return;
			}
			_expectRatioData.SetValue(_selfChar.GetId(), index, expectInnerRatio);
			SetExpectRatioData(_expectRatioData, context);
		}
	}

	[DomainMethod]
	public sbyte GetWeaponInnerRatio(DataContext context, ItemKey weaponKey)
	{
		int num = (IsCharInCombat(_selfTeam[0]) ? _combatCharacterDict[_selfTeam[0]].GetWeapons().IndexOf(weaponKey) : (-1));
		return (num >= 0) ? DomainManager.Taiwu.GetWeaponCurrInnerRatios()[num] : Config.Weapon.Instance[weaponKey.TemplateId].DefaultInnerRatio;
	}

	[DomainMethod]
	public WeaponEffectDisplayData[] GetWeaponEffects(ItemKey weaponKey)
	{
		int charId = -1;
		CombatWeaponData value = null;
		if (IsInCombat() && _weaponDataDict.TryGetValue(weaponKey.Id, out value))
		{
			charId = value.Character.GetId();
		}
		SkillEffectKey effectKey = value?.GetAutoAttackEffect() ?? new SkillEffectKey(-1, isDirect: false);
		SkillEffectKey effectKey2 = value?.GetPestleEffect() ?? new SkillEffectKey(-1, isDirect: false);
		return new WeaponEffectDisplayData[2]
		{
			new WeaponEffectDisplayData(effectKey, charId),
			new WeaponEffectDisplayData(effectKey2, charId)
		};
	}

	private void InitWeaponData(DataContext context)
	{
		ClearWeaponDataDict();
		foreach (CombatCharacter value in _combatCharacterDict.Values)
		{
			for (int i = 0; i < 7; i++)
			{
				InitWeaponData(context, value, i);
			}
			ChangeToFirstAvailableWeapon(context, value, init: true);
			value.SetAnimationToLoop(GetIdleAni(value), context);
		}
	}

	public void InitWeaponData(DataContext context, CombatCharacter character, int index)
	{
		ItemKey itemKey = character.GetWeapons()[index];
		if (itemKey.IsValid())
		{
			List<sbyte> weaponTricks = DomainManager.Item.GetWeaponTricks(itemKey);
			CombatWeaponData combatWeaponData = new CombatWeaponData(itemKey, character);
			sbyte[] weaponTricks2 = combatWeaponData.GetWeaponTricks();
			AddElement_WeaponDataDict(itemKey.Id, combatWeaponData);
			combatWeaponData.Init(context, index);
			for (int i = 0; i < weaponTricks.Count; i++)
			{
				weaponTricks2[i] = weaponTricks[i];
			}
		}
	}

	public void RemoveWeaponData(ItemKey weaponKey)
	{
		RemoveElement_WeaponDataDict(weaponKey.Id);
	}

	public void ChangeWeapon(DataContext context, CombatCharacter character, int weaponIndex, bool init = false, bool force = false)
	{
		ItemKey[] weapons = character.GetWeapons();
		int usingWeaponIndex = character.GetUsingWeaponIndex();
		int customParam = (weapons.CheckIndex(usingWeaponIndex) ? weapons[usingWeaponIndex].Id : (-1));
		if ((!force || !DomainManager.SpecialEffect.ModifyData(character.GetId(), -1, 181, dataValue: false, customParam)) && weaponIndex >= 0 && weapons[weaponIndex].IsValid())
		{
			CombatWeaponData weaponData = character.GetWeaponData(weaponIndex);
			if (usingWeaponIndex >= 0 && usingWeaponIndex != 3)
			{
				character.GetWeaponData(usingWeaponIndex).SetCdFrame(30000, context);
			}
			character.SetNeedChangeWeaponIndex(context, -1);
			character.SetUsingWeaponIndex(weaponIndex, context);
			character.SetWeaponTricks(weaponData.GetWeaponTricks(), context);
			character.SetChangeTrickAttack(changeTrickAttack: false, context);
			character.SetReserveNormalAttack(reserveNormalAttack: false, context);
			if (!init)
			{
				UpdateAllCommandAvailability(context, character);
				SetProperLoopAniAndParticle(context, character);
				Events.RaiseChangeWeapon(context, character.GetId(), character.IsAlly, weaponData, (usingWeaponIndex >= 0) ? character.GetWeaponData(usingWeaponIndex) : null);
			}
		}
	}

	private void ChangeToFirstAvailableWeapon(DataContext context, CombatCharacter character, bool init = false)
	{
		ItemKey[] weapons = character.GetWeapons();
		for (int i = 0; i < weapons.Length; i++)
		{
			if (TryGetElement_WeaponDataDict(weapons[i].Id, out var element) && element.GetCanChangeTo())
			{
				ChangeWeapon(context, character, i, init);
				break;
			}
		}
	}

	public void UpdateWeaponCanChange(DataContext context, CombatCharacter character)
	{
		ItemKey[] weapons = character.GetWeapons();
		for (int i = 0; i < weapons.Length; i++)
		{
			if (weapons[i].IsValid())
			{
				UpdateWeaponCanChange(context, character, i);
			}
		}
	}

	private void UpdateWeaponCanChange(DataContext context, CombatCharacter character, int index)
	{
		bool allowUseFreeWeapon = Config.Character.Instance[character.GetCharacter().GetTemplateId()].AllowUseFreeWeapon;
		CombatWeaponData weaponData = character.GetWeaponData(index);
		bool flag = index != character.GetUsingWeaponIndex() && !character.PreparingTeammateCommand() && ((index >= 3 && allowUseFreeWeapon) || weaponData.GetDurability() > 0) && weaponData.NotInAnyCd;
		if (weaponData.GetCanChangeTo() != flag)
		{
			weaponData.SetCanChangeTo(flag, context);
		}
	}

	public int GetWeaponCharId(int itemId)
	{
		CombatWeaponData value;
		return _weaponDataDict.TryGetValue(itemId, out value) ? value.Character.GetId() : (-1);
	}

	public CombatWeaponData GetUsingWeaponData(CombatCharacter character)
	{
		return _weaponDataDict[GetUsingWeaponKey(character).Id];
	}

	public ItemKey GetUsingWeaponKey(CombatCharacter character)
	{
		int usingWeaponIndex = character.GetUsingWeaponIndex();
		usingWeaponIndex = DomainManager.SpecialEffect.ModifyData(character.GetId(), -1, 82, usingWeaponIndex);
		return character.GetWeapons()[usingWeaponIndex];
	}

	public GameData.Domains.Item.Weapon GetUsingWeapon(CombatCharacter combatChar)
	{
		return DomainManager.Item.GetElement_Weapons(GetUsingWeaponKey(combatChar).Id);
	}

	public bool CanNormalAttack(bool isAlly)
	{
		CombatCharacter combatCharacter = (isAlly ? _selfChar : _enemyChar);
		return !combatCharacter.GetReserveNormalAttack() && CanNormalAttackWithoutCommandPrepareValueCheck(isAlly);
	}

	private bool CanNormalAttackWithoutCommandPrepareValueCheck(bool isAlly)
	{
		CombatCharacter combatCharacter = (isAlly ? _selfChar : _enemyChar);
		if (combatCharacter.ForbidNormalAttackEffectCount > 0)
		{
			return false;
		}
		GameData.Domains.Item.Weapon element_Weapons = DomainManager.Item.GetElement_Weapons(GetUsingWeaponKey(combatCharacter).Id);
		return element_Weapons.GetMaxDurability() <= 0 || element_Weapons.GetCurrDurability() != 0;
	}

	private bool CanUnlockAttack(bool isAlly, int index)
	{
		CombatCharacter combatCharacter = (isAlly ? _selfChar : _enemyChar);
		List<bool> canUnlockAttack = combatCharacter.GetCanUnlockAttack();
		return canUnlockAttack.CheckIndex(index) && canUnlockAttack[index];
	}

	public void UnlockAttack(DataContext context, CombatCharacter combatChar, int index)
	{
		combatChar.ChangeUnlockAttackValue(context, index, -GlobalConfig.Instance.UnlockAttackUnit);
		combatChar.NeedUnlockAttack = true;
		combatChar.UnlockWeaponIndex = index;
		combatChar.MoveData.ResetJumpState(context);
		UpdateAllCommandAvailability(context, combatChar);
		Events.RaiseUnlockAttack(context, combatChar, index);
		combatChar.DoExtraUnlockEffect(context, index);
	}

	public sbyte GetAttackHitType(CombatCharacter attacker, sbyte trickType)
	{
		return GetAttackHitType(attacker.GetCharacter(), trickType);
	}

	public unsafe sbyte GetAttackHitType(GameData.Domains.Character.Character attacker, sbyte trickType)
	{
		if (trickType != 21)
		{
			return Config.TrickType.Instance[trickType].AvoidType;
		}
		HitOrAvoidInts hitValues = attacker.GetHitValues();
		List<sbyte> list = ObjectPool<List<sbyte>>.Instance.Get();
		int num = int.MinValue;
		list.Clear();
		for (sbyte b = 0; b < 4; b++)
		{
			int num2 = hitValues.Items[b];
			if (num2 > num)
			{
				num = num2;
				list.Clear();
				list.Add(b);
			}
			else if (num2 == num)
			{
				list.Add(b);
			}
		}
		sbyte result = list[Context.Random.Next(list.Count)];
		ObjectPool<List<sbyte>>.Instance.Return(list);
		return result;
	}

	public (string aniName, string fullAniName) GetPrepareAttackAni(CombatCharacter character, sbyte trickType, int aniIndex)
	{
		TrickTypeItem trickTypeItem = Config.TrickType.Instance[trickType];
		string text;
		string item;
		if (character.IsBreakAttacking)
		{
			if (character.BossConfig == null && character.AnimalConfig == null)
			{
				text = (item = trickTypeItem.AttackAnimations[aniIndex] + "_B0");
			}
			else if (character.BossConfig != null)
			{
				string text2 = character.BossConfig.AttackEffectPostfix[character.GetUsingWeaponIndex()];
				text = character.BossConfig.AttackAnimation + "_B0" + text2;
				item = character.BossConfig.AniPrefix[character.GetBossPhase()] + text;
			}
			else
			{
				text = (item = null);
			}
			return (aniName: text, fullAniName: item);
		}
		if (character.BossConfig == null && character.AnimalConfig == null)
		{
			text = (item = trickTypeItem.AttackAnimations[aniIndex] + "_7");
		}
		else if (character.BossConfig != null)
		{
			string text3 = character.BossConfig.AttackEffectPostfix[character.GetUsingWeaponIndex()];
			text = character.BossConfig.AttackAnimation + "_7" + text3;
			item = character.BossConfig.AniPrefix[character.GetBossPhase()] + text;
		}
		else
		{
			text = trickTypeItem.AttackAnimations[aniIndex] + "_7";
			item = character.AnimalConfig.AniPrefix + text;
		}
		return (aniName: text, fullAniName: item);
	}

	public (string aniName, string fullAniName, string particle, string sound) GetAttackEffect(CombatCharacter character, GameData.Domains.Item.Weapon weapon, sbyte trickType)
	{
		(string, string, string, string) result = default((string, string, string, string));
		TrickTypeItem trickTypeItem = Config.TrickType.Instance[trickType];
		sbyte weaponAction = weapon.GetWeaponAction();
		short affectingDefendSkillId = character.GetAffectingDefendSkillId();
		if (character.BossConfig == null && character.AnimalConfig == null)
		{
			if (character.GetIsFightBack() && affectingDefendSkillId >= 0 && !string.IsNullOrEmpty(Config.CombatSkill.Instance[affectingDefendSkillId].FightBackAnimation))
			{
				CombatSkillItem combatSkillItem = Config.CombatSkill.Instance[affectingDefendSkillId];
				result.Item2 = (result.Item1 = combatSkillItem.FightBackAnimation);
				result.Item3 = combatSkillItem.FightBackParticle;
				result.Item4 = combatSkillItem.FightBackSound;
			}
			else
			{
				result.Item2 = (result.Item1 = $"{trickTypeItem.AttackAnimations[weaponAction]}_{character.PursueAttackCount}");
				result.Item3 = $"{trickTypeItem.AttackParticles[weaponAction]}_{character.PursueAttackCount}";
				result.Item4 = $"{trickTypeItem.SoundEffects[weaponAction]}_{character.PursueAttackCount}";
				result.Item4 += Config.Weapon.Instance[weapon.GetTemplateId()].SwingSoundsSuffix;
			}
		}
		else
		{
			int usingWeaponIndex = character.GetUsingWeaponIndex();
			if (character.BossConfig != null)
			{
				int bossPhase = character.GetBossPhase();
				if (character.GetIsFightBack() && affectingDefendSkillId >= 0)
				{
					CombatSkillItem combatSkillItem2 = Config.CombatSkill.Instance[affectingDefendSkillId];
					result.Item1 = combatSkillItem2.FightBackAnimation;
					result.Item2 = character.BossConfig.AniPrefix[bossPhase] + result.Item1;
					result.Item3 = character.BossConfig.DefendSkillParticlePrefix[bossPhase] + combatSkillItem2.FightBackParticle;
					result.Item4 = character.BossConfig.DefendSkillSoundPrefix[bossPhase] + combatSkillItem2.FightBackSound;
				}
				else
				{
					string value = character.BossConfig.AttackEffectPostfix[usingWeaponIndex];
					result.Item1 = $"{character.BossConfig.AttackAnimation}_{character.PursueAttackCount}{value}";
					result.Item2 = character.BossConfig.AniPrefix[bossPhase] + result.Item1;
					result.Item3 = $"{character.BossConfig.AttackParticles[bossPhase]}_{character.PursueAttackCount}{value}";
					result.Item4 = $"{character.BossConfig.AttackSounds[bossPhase]}_{character.PursueAttackCount}";
				}
			}
			else
			{
				result.Item1 = trickTypeItem.AttackAnimations.GetClampedIndexValueWithWarning(weaponAction, "GetAttackEffect");
				result.Item3 = character.AnimalConfig.AttackParticles[usingWeaponIndex];
				result.Item4 = character.AnimalConfig.AttackSounds[usingWeaponIndex];
				result.Item2 = character.AnimalConfig.AniPrefix + result.Item1;
			}
		}
		return result;
	}

	public (string aniName, string particle) GetBlockEffect(CombatCharacter character, IRandomSource random)
	{
		if (character.BossConfig == null && character.AnimalConfig == null)
		{
			WeaponItem template = character.GetWeaponData().Template;
			int index = random.Next(template.BlockAnis.Count);
			return (aniName: template.BlockAnis[index], particle: template.BlockParticles[index]);
		}
		string text = SpecialCharBlockAni[random.Next(SpecialCharBlockAni.Count)];
		string text2 = ((character.BossConfig != null) ? character.BossConfig.AniPrefix[character.GetBossPhase()] : character.AnimalConfig.AniPrefix);
		return (aniName: text, particle: "Particle_" + text2 + text);
	}

	public void UpdateWeaponCd(DataContext context, CombatCharacter character)
	{
		ItemKey[] weapons = character.GetWeapons();
		for (int i = 0; i < weapons.Length; i++)
		{
			if (!weapons[i].IsValid())
			{
				continue;
			}
			CombatWeaponData weaponData = character.GetWeaponData(i);
			if (!weaponData.NotInAnyCd)
			{
				if (weaponData.GetFixedCdLeftFrame() > 0)
				{
					weaponData.SetFixedCdLeftFrame((short)Math.Max(weaponData.GetFixedCdLeftFrame() - 1, 0), context);
				}
				else if (weaponData.GetCdFrame() > 0)
				{
					int weight = weaponData.Item.GetWeight();
					int weaponSwitchSpeed = character.GetCharacter().GetWeaponSwitchSpeed();
					int num = CFormula.CalcWeaponCdFrameSpeed(weaponSwitchSpeed, weight);
					weaponData.SetCdFrame((short)Math.Max(weaponData.GetCdFrame() - num, 0), context);
				}
				if (weaponData.NotInAnyCd)
				{
					UpdateWeaponCanChange(context, character, i);
					Events.RaiseWeaponCdEnd(context, character.GetId(), character.IsAlly, weaponData);
				}
			}
		}
	}

	public void ChangeWeaponCd(DataContext context, CombatCharacter character, int index, CValuePercent addPercent)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		CombatWeaponData weaponData = character.GetWeaponData(index);
		int num = 30000 * addPercent;
		short cdFrame = (short)Math.Clamp(weaponData.GetCdFrame() + num, 0, 30000);
		bool flag = weaponData.GetCdFrame() > 0;
		weaponData.SetCdFrame(cdFrame, context);
		UpdateWeaponCanChange(context, character, index);
		if (flag && weaponData.NotInAnyCd)
		{
			Events.RaiseWeaponCdEnd(context, character.GetId(), character.IsAlly, weaponData);
		}
	}

	public void ClearAllWeaponCd(DataContext context, CombatCharacter character)
	{
		foreach (CombatWeaponData value in _weaponDataDict.Values)
		{
			if (value.Character == character && !value.NotInAnyCd)
			{
				value.SetCdFrame(0, context);
				value.SetFixedCdLeftFrame(0, context);
				if (value.NotInAnyCd)
				{
					Events.RaiseWeaponCdEnd(context, character.GetId(), character.IsAlly, value);
				}
			}
		}
	}

	public void ClearWeaponCd(DataContext context, CombatCharacter character, int index)
	{
		CombatWeaponData weaponData = character.GetWeaponData(index);
		if (!weaponData.NotInAnyCd)
		{
			weaponData.SetCdFrame(0, context);
			weaponData.SetFixedCdLeftFrame(0, context);
			Events.RaiseWeaponCdEnd(context, character.GetId(), character.IsAlly, weaponData);
		}
	}

	public void SilenceWeapon(DataContext context, CombatCharacter combatChar, int weaponIndex, int cdFrame)
	{
		ItemKey itemKey = combatChar.GetWeapons()[weaponIndex];
		if (itemKey.IsValid())
		{
			if (cdFrame > 0)
			{
				(int, int) featureSilenceFrameTotalPercent = combatChar.GetFeatureSilenceFrameTotalPercent();
				cdFrame = DomainManager.SpecialEffect.ModifyValue(combatChar.GetId(), 264, cdFrame, -1, -1, -1, 0, 0, featureSilenceFrameTotalPercent.Item1, featureSilenceFrameTotalPercent.Item2);
			}
			short num = (short)Math.Clamp(cdFrame, -1, 32767);
			if (num != 0)
			{
				CombatWeaponData combatWeaponData = _weaponDataDict[itemKey.Id];
				combatWeaponData.SetFixedCdTotalFrame(num, context);
				combatWeaponData.SetFixedCdLeftFrame(num, context);
				UpdateWeaponCanChange(context, combatChar, weaponIndex);
			}
		}
	}

	public void CalcNormalAttack(CombatContext context, sbyte trickType)
	{
		//IL_054c: Unknown result type (might be due to invalid IL or missing references)
		CombatCharacter attacker = context.Attacker;
		CombatCharacter defender = context.Defender;
		Events.RaiseNormalAttackBegin(context, attacker, defender, trickType, attacker.PursueAttackCount);
		ItemKey weaponKey = context.WeaponKey;
		GameData.Domains.Item.Weapon weapon = context.Weapon;
		WeaponItem weaponConfig = context.WeaponConfig;
		sbyte normalAttackHitType = attacker.NormalAttackHitType;
		sbyte b = ((trickType != 21) ? Config.TrickType.Instance[trickType].AvoidType : normalAttackHitType);
		bool flag = attacker.GetId() == _carrierAnimalCombatCharId;
		CombatProperty property = _damageCompareData.GetProperty();
		bool critical = context.CheckCritical(normalAttackHitType);
		context = context.Property(property).Critical(critical);
		int hitOdds = property.HitOdds;
		hitOdds = ApplyHitOddsSpecialEffect(attacker, defender, hitOdds, normalAttackHitType, -1);
		if (hitOdds > 0)
		{
			hitOdds += GlobalConfig.Instance.NormalAttackExtraHitOdds;
		}
		bool flag2 = DomainManager.SpecialEffect.ModifyData(attacker.GetId(), -1, 251, dataValue: false);
		bool flag3 = !DomainManager.SpecialEffect.ModifyData(defender.GetId(), -1, 291, dataValue: false, critical ? 1 : 0, context.BodyPart, attacker.GetId()) && (flag2 || hitOdds < 0 || context.Random.CheckPercentProb(hitOdds));
		bool isFightBack = context.IsFightBack;
		if (attacker.AttackForceHitCount > 0)
		{
			flag3 = true;
		}
		else if (attacker.AttackForceMissCount > 0)
		{
			flag3 = false;
		}
		Events.RaiseNormalAttackCalcHitEnd(context, attacker, defender, attacker.PursueAttackCount, flag3, isFightBack, b == 3);
		Events.RaiseNormalAttackCalcCriticalEnd(context, attacker, defender, critical);
		Events.RaiseCalcLeveragingValue(context, b, flag3, attacker.PursueAttackCount);
		if (flag3)
		{
			sbyte index = ((trickType != 21) ? trickType : GodTrickUseTrickType[normalAttackHitType]);
			sbyte breakOdds = (sbyte)((attacker.NormalAttackBodyPart >= 0) ? Config.TrickType.Instance[index].EquipmentBreakOdds : 0);
			CalculateWeaponArmorBreak(context, breakOdds);
			int finalDamage = 0;
			bool flag4 = false;
			if (!TrickType.NoBodyDamageTrickType.Exist(trickType))
			{
				OuterAndInnerInts outerAndInnerInts = CalcAndAddInjury(context, normalAttackHitType, out finalDamage, out critical, isFightBack ? attacker.GetFightBackPower(attacker.FightBackHitType) : 100);
				flag4 = attacker.ApplyChangeTrickFlawOrAcupoint(context, defender, attacker.ChangeTrickBodyPart);
				if (b != 3)
				{
					context.ApplyWeaponAndArmorPoison();
				}
				if (!attacker.IsAutoNormalAttackingSpecial && b != 3 && !flag)
				{
					AddBounceDamage(context, normalAttackHitType);
				}
				if (CanPlayHitAnimation(defender))
				{
					int num = outerAndInnerInts.Outer + outerAndInnerInts.Inner;
					if (!attacker.NoBlockAttack && !critical)
					{
						(string, string) blockEffect = GetBlockEffect(defender, context.Random);
						defender.SetAnimationToPlayOnce(blockEffect.Item1, context);
						defender.SetParticleToPlay(blockEffect.Item2, context);
					}
					else if (num > 0 || flag4)
					{
						defender.SetAnimationToPlayOnce(GetHittedAni(defender, Math.Clamp(num - 1, 0, 2)), context);
					}
				}
				defender.PlayBeHitSound(context, weaponConfig, attacker, critical);
			}
			if (attacker.NeedReduceWeaponDurability)
			{
				ReduceDurabilityByHit(context, attacker, GetUsingWeaponKey(attacker));
				attacker.NeedReduceWeaponDurability = false;
			}
			if (defender.NeedReduceArmorDurability)
			{
				ReduceDurabilityByHit(context, defender, defender.Armors[attacker.NormalAttackBodyPart]);
				defender.NeedReduceArmorDurability = false;
			}
			if (!defender.GetNewPoisonsToShow().IsNonZero() && !flag4 && finalDamage <= 0)
			{
				defender.SetParticleToPlay("Particle_D_qidun", context);
			}
			if (defender.GetPreparingOtherAction() == 2 && _currentDistance <= InterruptFleeNeedDistance)
			{
				InterruptOtherAction(context, defender);
			}
			AddToCheckFallenSet(attacker.GetId());
			AddToCheckFallenSet(defender.GetId());
		}
		else
		{
			if (CanPlayHitAnimation(defender))
			{
				defender.SetAnimationToPlayOnce(GetAvoidAni(defender, normalAttackHitType), context);
			}
			defender.SetParticleToPlay((defender.IsAlly ? _selfAvoidParticle : _enemyAvoidParticle)[normalAttackHitType], context);
			string[] array = _avoidSound[normalAttackHitType];
			defender.SetHitSoundToPlay(array[context.Random.Next(array.Length)], context);
		}
		int percentProb = GlobalConfig.Instance.AvoidAddTrickBaseOdds + hitOdds / GlobalConfig.Instance.AvoidAddTrickHitOddsDivisor;
		bool flag5 = flag3 || (context.Random.CheckPercentProb(percentProb) && attacker.AttackForceMissCount <= 0);
		bool flag6 = !attacker.IsAutoNormalAttackingSpecial && attacker.GetId() != _carrierAnimalCombatCharId;
		bool flag7 = flag6;
		if (flag7)
		{
			byte pursueAttackCount = attacker.PursueAttackCount;
			bool flag8 = ((pursueAttackCount == 0 || pursueAttackCount == 2 || pursueAttackCount == 5) ? true : false);
			flag7 = flag8;
		}
		if (flag7 && flag5)
		{
			int count = 1 * DomainManager.SpecialEffect.GetModify(attacker.GetId(), 328, -1, -1, -1, (EDataSumType)0);
			AddTrick(context, GetCombatCharacter(attacker.IsAlly), trickType, count, addedByAlly: true, !flag3);
		}
		bool flag9 = DomainManager.SpecialEffect.ModifyData(attacker.GetId(), -1, 86, dataValue: true);
		bool flag10 = DomainManager.SpecialEffect.ModifyData(defender.GetId(), -1, 250, dataValue: false, critical ? 1 : 0);
		if (!isFightBack && !flag && flag9 && CanFightBack(defender, normalAttackHitType) && (flag10 || !flag3))
		{
			defender.FightBackWithHit = flag3;
			defender.FightBackHitType = normalAttackHitType;
			defender.SetIsFightBack(isFightBack: true, context);
		}
		if (InAttackRange(attacker) && !attacker.IsAutoNormalAttackingSpecial)
		{
			bool flag11 = attacker.PursueAttackCount > 0;
			int num2 = ((!flag11) ? weaponConfig.StanceIncrement : (weaponConfig.StanceIncrement * 25 / 100));
			sbyte attackPreparePointCost = weapon.GetAttackPreparePointCost();
			if (attacker.GetStanceValue() < attacker.GetMaxStanceValue())
			{
				RecoverStanceValue(context, attacker, num2, attackPreparePointCost, flag11);
			}
			if (defender.GetStanceValue() < defender.GetMaxStanceValue())
			{
				RecoverStanceValue(context, defender, num2 / 3, attackPreparePointCost, flag11);
			}
		}
		if (weapon.GetCanChangeTrick() && attacker.AttackForceHitCount <= 0 && attacker.AttackForceMissCount <= 0 && !attacker.GetChangeTrickAttack())
		{
			int changeValue = DomainManager.Character.CalcWeaponChangeTrickValue(attacker.GetId(), weaponKey, attacker.PursueAttackCount == 0, flag3);
			ChangeChangeTrickProgress(context, attacker, changeValue);
		}
		if (!attacker.IsAutoNormalAttackingSpecial && attacker.PursueAttackCount == 0 && attacker.PoisonOverflow(0))
		{
			attacker.AddPoisonAffectValue(0, 1);
		}
		Events.RaiseNormalAttackEnd(context, attacker, defender, trickType, attacker.PursueAttackCount, flag3, isFightBack);
	}

	public void CalcUnlockAttack(CombatCharacter attacker, int index)
	{
		//IL_0275: Unknown result type (might be due to invalid IL or missing references)
		IRandomSource random = Context.Random;
		CombatCharacter combatCharacter = GetCombatCharacter(!attacker.IsAlly, tryGetCoverCharacter: true);
		sbyte random2 = attacker.UnlockWeapon.GetTricks().GetRandom(random);
		sbyte attackHitType = GetAttackHitType(attacker, random2);
		attackHitType = (sbyte)DomainManager.SpecialEffect.ModifyData(attacker.GetId(), (short)(-1), (ushort)68, (int)attackHitType, -1, -1, -1);
		sbyte attackBodyPart = GetAttackBodyPart(attacker, combatCharacter, random, -1, random2, attackHitType);
		CombatContext combatContext = CombatContext.Create(attacker, combatCharacter, attackBodyPart, -1, attacker.UnlockWeaponIndex).Critical(critical: true);
		attacker.NormalAttackHitType = attackHitType;
		attacker.NormalAttackBodyPart = attackBodyPart;
		Events.RaiseNormalAttackBegin(combatContext, attacker, combatCharacter, random2, 0);
		Events.RaiseNormalAttackCalcHitEnd(combatContext, attacker, combatCharacter, 0, hit: true, isFightBack: false, attackHitType == 3);
		Events.RaiseNormalAttackCalcCriticalEnd(combatContext, attacker, combatCharacter, critical: true);
		Events.RaiseCalcLeveragingValue(combatContext, attackHitType, hit: true, index);
		if (attackBodyPart >= 0)
		{
			combatContext.ApplyWeaponAndArmorPoison(attacker.UnlockEffect.PoisonRatio);
			if (attacker.UnlockEffect.FlawLevels != null && attacker.UnlockEffect.FlawLevels.Length > index)
			{
				AddFlaw(Context, combatCharacter, attacker.UnlockEffect.FlawLevels[index], (charId: -1, skillId: (short)(-1)), attackBodyPart);
			}
			if (attacker.UnlockEffect.AcupointLevels != null && attacker.UnlockEffect.AcupointLevels.Length > index)
			{
				AddAcupoint(Context, combatCharacter, attacker.UnlockEffect.AcupointLevels[index], (charId: -1, skillId: (short)(-1)), attackBodyPart);
			}
		}
		sbyte equipmentBreakOdds = Config.TrickType.Instance[random2].EquipmentBreakOdds;
		combatContext.CheckReduceArmorDurability(equipmentBreakOdds);
		int power = 100 + DomainManager.Character.GetItemPower(combatContext.AttackerId, combatContext.WeaponKey) / 5;
		UpdateDamageCompareData(combatContext);
		combatContext = combatContext.Property(_damageCompareData.GetProperty());
		CalcAndAddInjury(combatContext, attackHitType, out var _, out var _, power);
		if (attackHitType != 3)
		{
			AddBounceDamage(combatContext, attackHitType);
		}
		if (combatCharacter.NeedReduceArmorDurability)
		{
			combatCharacter.NeedReduceArmorDurability = false;
			ReduceDurabilityByHit(combatContext, combatCharacter, combatContext.ArmorKey);
		}
		if (attacker.UnlockEffect.StealNeiliAllocationPercent > 0)
		{
			attacker.StealNeiliAllocationRandom(Context, combatCharacter, CValuePercent.op_Implicit(attacker.UnlockEffect.StealNeiliAllocationPercent));
		}
		short randomBanableSkillId = combatCharacter.GetRandomBanableSkillId(Context.Random, null, -1);
		if (randomBanableSkillId >= 0 && attacker.UnlockEffect.SilenceSkillFrame > 0)
		{
			SilenceSkill(Context, combatCharacter, randomBanableSkillId, attacker.UnlockEffect.SilenceSkillFrame);
		}
		if (attacker.UnlockEffect.AddQiDisorder > 0)
		{
			ChangeDisorderOfQiRandomRecovery(Context, combatCharacter, attacker.UnlockEffect.AddQiDisorder);
		}
		AddToCheckFallenSet(combatCharacter.GetId());
		if (combatCharacter.GetPreparingOtherAction() == 2 && _currentDistance <= InterruptFleeNeedDistance)
		{
			InterruptOtherAction(combatContext, combatCharacter);
		}
		Events.RaiseNormalAttackEnd(combatContext, attacker, combatCharacter, random2, 0, hit: true, isFightBack: false);
		attacker.NormalAttackHitType = -1;
		attacker.NormalAttackBodyPart = -1;
	}

	public bool CalcSpiritAttack(CombatCharacter attacker, int index)
	{
		IRandomSource random = Context.Random;
		CombatCharacter combatCharacter = GetCombatCharacter(!attacker.IsAlly, tryGetCoverCharacter: true);
		CombatWeaponData weaponData = attacker.GetWeaponData(index);
		sbyte random2 = weaponData.GetWeaponTricks().GetRandom(random);
		sbyte attackHitType = DomainManager.Combat.GetAttackHitType(attacker, random2);
		sbyte attackBodyPart = DomainManager.Combat.GetAttackBodyPart(attacker, combatCharacter, random, -1, random2, attackHitType);
		CombatContext combatContext = CombatContext.Create(attacker, combatCharacter, attackBodyPart, -1, index);
		bool changeTrickAttack = attacker.GetChangeTrickAttack();
		attacker.SetChangeTrickAttack(changeTrickAttack: true, combatContext);
		attacker.IsAutoNormalAttackingSpecial = true;
		attacker.NormalAttackHitType = attackHitType;
		sbyte attackPreparePointCost = weaponData.Item.GetAttackPreparePointCost();
		bool flag = random.CheckPercentProb(50);
		if (flag)
		{
			if (random.CheckPercentProb(75))
			{
				AddFlaw(combatContext, combatCharacter, attackPreparePointCost, (charId: -1, skillId: (short)(-1)), attackBodyPart);
			}
			else
			{
				AddAcupoint(combatContext, combatCharacter, attackPreparePointCost, (charId: -1, skillId: (short)(-1)), attackBodyPart);
			}
		}
		combatContext.ApplyWeaponAndArmorPoison();
		combatContext = combatContext.Critical(critical: true);
		UpdateDamageCompareData(combatContext);
		combatContext = combatContext.Property(_damageCompareData.GetProperty());
		CalcAndAddInjury(combatContext, attackHitType, out var _, out var critical);
		combatCharacter.PlayBeHitSound(combatContext, weaponData.Template, attacker, critical);
		attacker.IsAutoNormalAttackingSpecial = false;
		attacker.NormalAttackHitType = -1;
		attacker.SetChangeTrickAttack(changeTrickAttack, combatContext);
		return flag;
	}

	public void AddWeaponAttackSelfInjury(DataContext context, CombatCharacter character, int weaponIndex)
	{
		CombatWeaponData weaponData = character.GetWeaponData(weaponIndex);
		GameData.Domains.Item.Weapon item = weaponData.Item;
		List<sbyte> tricks = item.GetTricks();
		sbyte trickType = tricks[context.Random.Next(0, tricks.Count)];
		sbyte attackHitType = GetAttackHitType(character, trickType);
		sbyte attackBodyPart = GetAttackBodyPart(character, character, context.Random, -1, trickType, attackHitType);
		CombatContext context2 = CombatContext.Create(character, character, attackBodyPart, -1, weaponIndex);
		CalcAndAddInjury(context2, attackHitType, out var _, out var _);
	}

	public void ChangeChangeTrickCount(DataContext context, CombatCharacter character, int addValue, bool bySelectChangeTrick = false)
	{
		character.SetChangeTrickCount((short)Math.Clamp(character.GetChangeTrickCount() + addValue, 0, character.MaxChangeTrickCount), context);
		if (character.GetChangeTrickCount() == character.MaxChangeTrickCount && character.GetChangeTrickProgress() > 0)
		{
			character.SetChangeTrickProgress(0, context);
		}
		UpdateCanChangeTrick(context, character);
		Events.RaiseChangeTrickCountChanged(context, character, addValue, bySelectChangeTrick);
	}

	public void UpdateCanChangeTrick(DataContext context, CombatCharacter character)
	{
		GameData.Domains.Item.Weapon element_Weapons = DomainManager.Item.GetElement_Weapons(GetUsingWeaponKey(character).Id);
		bool flag = character.GetChangeTrickCount() > element_Weapons.GetAttackPreparePointCost() && IsCurrentCombatCharacter(character) && element_Weapons.GetCanChangeTrick() && CanNormalAttackWithoutCommandPrepareValueCheck(character.IsAlly);
		if (character.GetCanChangeTrick() != flag)
		{
			character.SetCanChangeTrick(flag, context);
		}
	}

	public bool CanPursue(CombatCharacter character, bool critical)
	{
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		CombatCharacter combatCharacter = GetCombatCharacter(!character.IsAlly, tryGetCoverCharacter: true);
		if (!IsInCombat() || !DomainManager.SpecialEffect.ModifyData(character.GetId(), -1, 252, dataValue: true) || character.PursueAttackCount >= 5 || character.GetIsFightBack() || combatCharacter.GetIsFightBack() || combatCharacter.ChangeCharId >= 0 || character.ChangeCharId >= 0 || combatCharacter.NeedChangeBossPhase || IsCharacterFallen(combatCharacter) || IsCharacterFallen(character) || _saveDyingEffectTriggerd || character.AttackForceHitCount > 0 || character.AttackForceMissCount > 0)
		{
			return false;
		}
		if (character.IsAutoNormalAttackingSpecial || character.IsBreakAttacking)
		{
			return true;
		}
		GameData.Domains.Item.Weapon element_Weapons = DomainManager.Item.GetElement_Weapons(GetUsingWeaponKey(character).Id);
		short itemPower = DomainManager.Character.GetItemPower(character.GetId(), element_Weapons.GetItemKey());
		int num = CFormula.FormulaCalcPursueOdds(element_Weapons.GetPursueAttackFactor(), CValuePercent.op_Implicit((int)itemPower), character.PursueAttackCount);
		int modifyValue = DomainManager.SpecialEffect.GetModifyValue(character.GetId(), 76, (EDataModifyType)0, character.PursueAttackCount, -1, -1, (EDataSumType)0);
		int modifyValue2 = DomainManager.SpecialEffect.GetModifyValue(combatCharacter.GetId(), 109, (EDataModifyType)0, character.PursueAttackCount, -1, -1, (EDataSumType)0);
		num = Math.Max(num + modifyValue + modifyValue2, 0);
		int modifyValue3 = DomainManager.SpecialEffect.GetModifyValue(character.GetId(), 76, (EDataModifyType)1, character.PursueAttackCount, critical ? 1 : 0, -1, (EDataSumType)0);
		int modifyValue4 = DomainManager.SpecialEffect.GetModifyValue(combatCharacter.GetId(), 109, (EDataModifyType)1, character.PursueAttackCount, critical ? 1 : 0, -1, (EDataSumType)0);
		int num2 = 100 + modifyValue3 + modifyValue4;
		num = num * num2 / 100;
		(int, int) totalPercentModifyValue = DomainManager.SpecialEffect.GetTotalPercentModifyValue(character.GetId(), -1, 76);
		(int, int) totalPercentModifyValue2 = DomainManager.SpecialEffect.GetTotalPercentModifyValue(combatCharacter.GetId(), -1, 109);
		num2 = 100 + Math.Max(totalPercentModifyValue.Item1, totalPercentModifyValue2.Item1) + Math.Min(totalPercentModifyValue.Item2, totalPercentModifyValue2.Item2);
		num = num * num2 / 100;
		num = DomainManager.SpecialEffect.ModifyData(character.GetId(), -1, 76, num);
		return Context.Random.CheckPercentProb(num);
	}

	private bool CanFightBack(CombatCharacter defender, sbyte hitType)
	{
		bool flag = DomainManager.SpecialEffect.ModifyData(defender.GetId(), -1, 193, dataValue: false);
		if (!IsMainCharacter(defender) || IsCharacterFallen(defender) || !InAttackRange(defender) || hitType == 3 || (defender.GetPreparingSkillId() >= 0 && !flag) || defender.GetPreparingOtherAction() >= 0 || defender.GetPreparingItem().IsValid() || defender.GetFightBackPower(hitType) <= 0)
		{
			return false;
		}
		return true;
	}

	public void AddRandomTrick(DataContext context, CombatCharacter combatChar, int count)
	{
		List<NeedTrick> list = ObjectPool<List<NeedTrick>>.Instance.Get();
		sbyte[] weaponTricks = combatChar.GetWeaponTricks();
		for (int i = 0; i < count; i++)
		{
			list.Add(new NeedTrick(weaponTricks.GetRandom(context.Random), 1));
		}
		AddTrick(context, combatChar, list);
		ObjectPool<List<NeedTrick>>.Instance.Return(list);
	}

	public void AddTrick(DataContext context, CombatCharacter combatChar, sbyte trickType, bool addedByAlly = true)
	{
		AddTrick(context, combatChar, trickType, 1, addedByAlly);
	}

	public void AddTrick(DataContext context, CombatCharacter combatChar, sbyte trickType, int count, bool addedByAlly = true, bool addByAvoid = false)
	{
		List<NeedTrick> list = ObjectPool<List<NeedTrick>>.Instance.Get();
		list.Add(new NeedTrick(trickType, (byte)Math.Clamp(count, 0, 255)));
		AddTrick(context, combatChar, list, addedByAlly, addByAvoid);
		ObjectPool<List<NeedTrick>>.Instance.Return(list);
	}

	public void AddTrick(DataContext context, CombatCharacter combatChar, IEnumerable<sbyte> trickTypes)
	{
		List<NeedTrick> list = ObjectPool<List<NeedTrick>>.Instance.Get();
		ConvertTricks(list, trickTypes);
		AddTrick(context, combatChar, list);
		ObjectPool<List<NeedTrick>>.Instance.Return(list);
	}

	public void AddTrick(DataContext context, CombatCharacter character, List<NeedTrick> tricks, bool addedByAlly = true, bool addByAvoid = false)
	{
		TrickCollection tricks2 = character.GetTricks();
		int num = 0;
		foreach (NeedTrick trick in tricks)
		{
			for (int i = 0; i < trick.NeedCount; i++)
			{
				sbyte trickType = trick.TrickType;
				if (DomainManager.SpecialEffect.ModifyData(character.GetId(), -1, 138, dataValue: true, trickType, addedByAlly ? 1 : 0, 1))
				{
					trickType = (sbyte)DomainManager.SpecialEffect.ModifyData(character.GetId(), (short)(-1), (ushort)139, (int)trickType, -1, -1, -1);
					tricks2.AppendTrick(trickType, addByAvoid);
					if (tricks2.Tricks.Count > character.GetMaxTrickCount())
					{
						RemoveOverflowTrick(context, character);
					}
					character.SetTricks(tricks2, context);
					UpdateSkillCostTrickCanUse(context, character);
					Events.RaiseGetTrick(context, character.GetId(), character.IsAlly, trickType, character.IsTrickUsable(trickType));
					if (trickType == 19)
					{
						num++;
					}
				}
			}
		}
		for (int j = 0; j < num; j++)
		{
			Events.RaiseGetShaTrick(context, character.GetId(), character.IsAlly, real: true);
		}
	}

	public void RemoveOverflowTrick(DataContext context, CombatCharacter character, bool updateFieldAndSkill = false)
	{
		TrickCollection tricks = character.GetTricks();
		List<int> list = ObjectPool<List<int>>.Instance.Get();
		list.Clear();
		list.AddRange(tricks.Tricks.Keys);
		int num = 0;
		while (tricks.Tricks.Count > character.GetMaxTrickCount())
		{
			tricks.RemoveTrick(list[0]);
			list.RemoveAt(0);
			num++;
		}
		ObjectPool<List<int>>.Instance.Return(list);
		Events.RaiseOverflowTrickRemoved(context, character.GetId(), character.IsAlly, num);
		if (updateFieldAndSkill)
		{
			character.SetTricks(tricks, context);
			UpdateSkillCostTrickCanUse(context, character);
		}
	}

	public void RemoveUsableTrickInsteadCostTrick([DisallowNull] CombatCharacter character, short skillId, [DisallowNull] List<NeedTrick> costTricks, [AllowNull] List<NeedTrick> costEnemyTricks = null)
	{
		costEnemyTricks?.Clear();
		bool flag = DomainManager.SpecialEffect.ModifyData(character.GetId(), skillId, 280, dataValue: false, (costEnemyTricks != null) ? 1 : 0);
		CombatCharacter combatCharacter = GetCombatCharacter(!character.IsAlly);
		if (flag && combatCharacter != null)
		{
			Dictionary<sbyte, byte> dictionary = ObjectPool<Dictionary<sbyte, byte>>.Instance.Get();
			Dictionary<sbyte, byte> dictionary2 = ObjectPool<Dictionary<sbyte, byte>>.Instance.Get();
			character.CalcCostTrickStatus(costTricks, dictionary, dictionary2);
			costTricks.Clear();
			Dictionary<sbyte, byte> dictionary3 = ObjectPool<Dictionary<sbyte, byte>>.Instance.Get();
			combatCharacter.CalcInsteadTricks(dictionary3, combatCharacter.IsTrickUsable, dictionary, dictionary2);
			costTricks.AddRange(dictionary.Select((KeyValuePair<sbyte, byte> tup) => new NeedTrick(tup.Key, tup.Value)));
			costEnemyTricks?.AddRange(dictionary3.Select((KeyValuePair<sbyte, byte> tup) => new NeedTrick(tup.Key, tup.Value)));
			ObjectPool<Dictionary<sbyte, byte>>.Instance.Return(dictionary);
			ObjectPool<Dictionary<sbyte, byte>>.Instance.Return(dictionary2);
			ObjectPool<Dictionary<sbyte, byte>>.Instance.Return(dictionary3);
		}
	}

	public void RemoveCostTrickInsteadUselessTrick(CombatCharacter character, short skillId, List<NeedTrick> costTricks, bool trulyCost)
	{
		if (DomainManager.SpecialEffect.ModifyData(character.GetId(), skillId, 284, dataValue: false, trulyCost ? 1 : 0))
		{
			Dictionary<sbyte, byte> dictionary = ObjectPool<Dictionary<sbyte, byte>>.Instance.Get();
			Dictionary<sbyte, byte> dictionary2 = ObjectPool<Dictionary<sbyte, byte>>.Instance.Get();
			character.CalcCostTrickStatus(costTricks, dictionary, dictionary2);
			costTricks.Clear();
			Dictionary<sbyte, byte> dictionary3 = ObjectPool<Dictionary<sbyte, byte>>.Instance.Get();
			character.CalcInsteadTricks(dictionary3, character.IsTrickUseless, dictionary, dictionary2);
			costTricks.AddRange(dictionary.Select((KeyValuePair<sbyte, byte> tup) => new NeedTrick(tup.Key, tup.Value)));
			costTricks.AddRange(dictionary3.Select((KeyValuePair<sbyte, byte> tup) => new NeedTrick(tup.Key, tup.Value)));
			ObjectPool<Dictionary<sbyte, byte>>.Instance.Return(dictionary);
			ObjectPool<Dictionary<sbyte, byte>>.Instance.Return(dictionary2);
			ObjectPool<Dictionary<sbyte, byte>>.Instance.Return(dictionary3);
		}
	}

	public void RemoveCostTrickBySelfShaTrick(DataContext context, CombatCharacter character, short skillId, List<NeedTrick> costTricks, bool trulyCost)
	{
		if (!DomainManager.SpecialEffect.ModifyData(character.GetId(), skillId, 319, dataValue: false, 1))
		{
			return;
		}
		Dictionary<sbyte, byte> dictionary = ObjectPool<Dictionary<sbyte, byte>>.Instance.Get();
		Dictionary<sbyte, byte> dictionary2 = ObjectPool<Dictionary<sbyte, byte>>.Instance.Get();
		character.CalcCostTrickStatus(costTricks, dictionary, dictionary2);
		costTricks.Clear();
		Dictionary<sbyte, byte> dictionary3 = ObjectPool<Dictionary<sbyte, byte>>.Instance.Get();
		if (dictionary.Keys.All((sbyte x) => x != 19))
		{
			character.CalcInsteadTricks(dictionary3, (sbyte x) => x == 19, dictionary, dictionary2, int.MaxValue, onlyInsteadLack: true);
		}
		if (trulyCost && dictionary3.Values.Sum((byte x) => x) > 0)
		{
			Events.RaiseShaTrickInsteadCostTricks(context, character, skillId);
		}
		costTricks.AddRange(dictionary.Select((KeyValuePair<sbyte, byte> tup) => new NeedTrick(tup.Key, tup.Value)));
		costTricks.AddRange(dictionary3.Select((KeyValuePair<sbyte, byte> tup) => new NeedTrick(tup.Key, tup.Value)));
		ObjectPool<Dictionary<sbyte, byte>>.Instance.Return(dictionary);
		ObjectPool<Dictionary<sbyte, byte>>.Instance.Return(dictionary2);
		ObjectPool<Dictionary<sbyte, byte>>.Instance.Return(dictionary3);
	}

	public void RemoveCostTrickByEnemyShaTrick(DataContext context, CombatCharacter character, short skillId, List<NeedTrick> costTricks, List<NeedTrick> costEnemyTricks = null)
	{
		bool flag = DomainManager.SpecialEffect.ModifyData(character.GetId(), skillId, 319, dataValue: false, 0);
		CombatCharacter combatCharacter = GetCombatCharacter(!character.IsAlly);
		if (!flag || combatCharacter == null)
		{
			return;
		}
		Dictionary<sbyte, byte> dictionary = ObjectPool<Dictionary<sbyte, byte>>.Instance.Get();
		Dictionary<sbyte, byte> dictionary2 = ObjectPool<Dictionary<sbyte, byte>>.Instance.Get();
		character.CalcCostTrickStatus(costTricks, dictionary, dictionary2);
		costTricks.Clear();
		Dictionary<sbyte, byte> dictionary3 = ObjectPool<Dictionary<sbyte, byte>>.Instance.Get();
		if (dictionary.Keys.All((sbyte x) => x != 19))
		{
			combatCharacter.CalcInsteadTricks(dictionary3, (sbyte x) => x == 19, dictionary, dictionary2, int.MaxValue, onlyInsteadLack: true);
		}
		if (costEnemyTricks != null && dictionary3.Values.Sum((byte x) => x) > 0)
		{
			Events.RaiseShaTrickInsteadCostTricks(context, character, skillId);
		}
		costTricks.AddRange(dictionary.Select((KeyValuePair<sbyte, byte> tup) => new NeedTrick(tup.Key, tup.Value)));
		costEnemyTricks?.AddRange(dictionary3.Select((KeyValuePair<sbyte, byte> tup) => new NeedTrick(tup.Key, tup.Value)));
		ObjectPool<Dictionary<sbyte, byte>>.Instance.Return(dictionary);
		ObjectPool<Dictionary<sbyte, byte>>.Instance.Return(dictionary2);
		ObjectPool<Dictionary<sbyte, byte>>.Instance.Return(dictionary3);
	}

	public void RemoveCostTrickByJiTrick(DataContext context, CombatCharacter character, short skillId, List<NeedTrick> costTricks, bool trulyCost)
	{
		int num = DomainManager.SpecialEffect.ModifyData(character.GetId(), skillId, 313, 0);
		if (num <= 0)
		{
			return;
		}
		Dictionary<sbyte, byte> dictionary = ObjectPool<Dictionary<sbyte, byte>>.Instance.Get();
		Dictionary<sbyte, byte> dictionary2 = ObjectPool<Dictionary<sbyte, byte>>.Instance.Get();
		character.CalcCostTrickStatus(costTricks, dictionary, dictionary2);
		costTricks.Clear();
		Dictionary<sbyte, byte> dictionary3 = ObjectPool<Dictionary<sbyte, byte>>.Instance.Get();
		if (dictionary.Keys.All((sbyte x) => x != 12))
		{
			character.CalcInsteadTricks(dictionary3, (sbyte x) => x == 12, dictionary, dictionary2, num);
		}
		if (trulyCost)
		{
			Events.RaiseJiTrickInsteadCostTricks(context, character, dictionary3.Values.Sum((byte x) => x));
		}
		costTricks.AddRange(dictionary.Select((KeyValuePair<sbyte, byte> tup) => new NeedTrick(tup.Key, tup.Value)));
		costTricks.AddRange(dictionary3.Select((KeyValuePair<sbyte, byte> tup) => new NeedTrick(tup.Key, tup.Value)));
		ObjectPool<Dictionary<sbyte, byte>>.Instance.Return(dictionary);
		ObjectPool<Dictionary<sbyte, byte>>.Instance.Return(dictionary2);
		ObjectPool<Dictionary<sbyte, byte>>.Instance.Return(dictionary3);
	}

	public void RemoveJiTrickByUselessTrick(DataContext context, CombatCharacter character, short skillId, List<NeedTrick> costTricks, bool trulyCost)
	{
		int val = DomainManager.SpecialEffect.ModifyData(character.GetId(), skillId, 314, 0);
		val = Math.Min(val, costTricks.Sum((NeedTrick x) => (x.TrickType == 12) ? x.NeedCount : 0));
		if (val <= 0)
		{
			return;
		}
		Dictionary<sbyte, byte> dictionary = ObjectPool<Dictionary<sbyte, byte>>.Instance.Get();
		foreach (NeedTrick costTrick in costTricks)
		{
			dictionary[costTrick.TrickType] = costTrick.NeedCount;
		}
		costTricks.Clear();
		Dictionary<sbyte, byte> dictionary2 = ObjectPool<Dictionary<sbyte, byte>>.Instance.Get();
		foreach (sbyte value in character.GetTricks().Tricks.Values)
		{
			if (!character.IsTrickUsable(value))
			{
				dictionary2[value] = (byte)Math.Min(dictionary2.GetOrDefault(value) + 1, 255);
				dictionary[12]--;
				if (dictionary[12] <= 0)
				{
					dictionary.Remove(12);
					break;
				}
			}
		}
		if (trulyCost)
		{
			Events.RaiseUselessTrickInsteadJiTricks(context, character, dictionary2.Values.Sum((byte x) => x));
		}
		costTricks.AddRange(dictionary.Select((KeyValuePair<sbyte, byte> tup) => new NeedTrick(tup.Key, tup.Value)));
		costTricks.AddRange(dictionary2.Select((KeyValuePair<sbyte, byte> tup) => new NeedTrick(tup.Key, tup.Value)));
		ObjectPool<Dictionary<sbyte, byte>>.Instance.Return(dictionary);
		ObjectPool<Dictionary<sbyte, byte>>.Instance.Return(dictionary2);
	}

	public bool RemoveTrick(DataContext context, CombatCharacter character, sbyte trickType, byte count = 1, bool removedByAlly = true, int preferIndex = -1)
	{
		List<NeedTrick> list = ObjectPool<List<NeedTrick>>.Instance.Get();
		list.Clear();
		list.Add(new NeedTrick(trickType, count));
		bool result = RemoveTrick(context, character, list, removedByAlly, skillCost: false, preferIndex);
		ObjectPool<List<NeedTrick>>.Instance.Return(list);
		return result;
	}

	public void RemoveTrick(DataContext context, CombatCharacter combatChar, IEnumerable<sbyte> trickTypes, bool removedByAlly = true)
	{
		List<NeedTrick> list = ObjectPool<List<NeedTrick>>.Instance.Get();
		ConvertTricks(list, trickTypes);
		RemoveTrick(context, combatChar, list, removedByAlly);
		ObjectPool<List<NeedTrick>>.Instance.Return(list);
	}

	public bool RemoveTrick(DataContext context, CombatCharacter character, List<NeedTrick> tricks, bool removedByAlly = true, bool skillCost = false, int preferIndex = -1)
	{
		TrickCollection tricks2 = character.GetTricks();
		List<int> list = ObjectPool<List<int>>.Instance.Get();
		bool result = false;
		tricks = DomainManager.SpecialEffect.ModifyData(character.GetId(), -1, 164, tricks, skillCost ? 1 : 0, removedByAlly ? 1 : 0);
		int num = 0;
		foreach (NeedTrick trick in tricks)
		{
			if (trick.NeedCount <= 0)
			{
				continue;
			}
			int num2 = 0;
			list.Clear();
			list.AddRange(tricks2.Tricks.Keys);
			if (list.Contains(preferIndex))
			{
				list.MoveIndexToFirst(list.IndexOf(preferIndex));
			}
			for (int i = 0; i < list.Count; i++)
			{
				if (character.TrickEquals(tricks2.Tricks[list[i]], trick.TrickType))
				{
					tricks2.RemoveTrick(list[i]);
					num2++;
					if (trick.TrickType == 19)
					{
						num++;
					}
					if (num2 == trick.NeedCount)
					{
						break;
					}
				}
			}
			if (num2 > 0)
			{
				result = true;
			}
		}
		ObjectPool<List<int>>.Instance.Return(list);
		character.SetTricks(tricks2, context);
		for (int j = 0; j < num; j++)
		{
			Events.RaiseRemoveShaTrick(context, character.GetId());
		}
		return result;
	}

	public bool StealTrick(DataContext context, CombatCharacter thief, CombatCharacter victim, sbyte trickType, byte count = 1)
	{
		List<NeedTrick> list = ObjectPool<List<NeedTrick>>.Instance.Get();
		list.Add(new NeedTrick(trickType, count));
		bool result = StealTrick(context, thief, victim, list);
		ObjectPool<List<NeedTrick>>.Instance.Return(list);
		return result;
	}

	public bool StealTrick(DataContext context, CombatCharacter thief, CombatCharacter victim, IEnumerable<sbyte> trickTypes)
	{
		List<NeedTrick> list = ObjectPool<List<NeedTrick>>.Instance.Get();
		ConvertTricks(list, trickTypes);
		bool result = StealTrick(context, thief, victim, list);
		ObjectPool<List<NeedTrick>>.Instance.Return(list);
		return result;
	}

	public bool StealTrick(DataContext context, CombatCharacter thief, CombatCharacter victim, List<NeedTrick> tricks)
	{
		tricks = DomainManager.SpecialEffect.ModifyData(victim.GetId(), -1, 164, tricks);
		List<NeedTrick> list = ObjectPool<List<NeedTrick>>.Instance.Get();
		TrickCollection tricks2 = victim.GetTricks();
		List<int> list2 = ObjectPool<List<int>>.Instance.Get();
		foreach (NeedTrick trick in tricks)
		{
			if (trick.NeedCount <= 0)
			{
				continue;
			}
			byte b = 0;
			list2.Clear();
			list2.AddRange(tricks2.Tricks.Keys);
			for (int i = 0; i < list2.Count; i++)
			{
				if (tricks2.Tricks[list2[i]] == trick.TrickType)
				{
					tricks2.RemoveTrick(list2[i]);
					b++;
					if (b == trick.NeedCount)
					{
						break;
					}
				}
			}
			if (b > 0)
			{
				list.Add(new NeedTrick(trick.TrickType, b));
			}
		}
		victim.SetTricks(tricks2, context);
		bool flag = list.Count > 0;
		if (flag)
		{
			AddTrick(context, thief, list);
		}
		ObjectPool<List<int>>.Instance.Return(list2);
		ObjectPool<List<NeedTrick>>.Instance.Return(list);
		return flag;
	}

	private static void ConvertTricks(ICollection<NeedTrick> needTricks, IEnumerable<sbyte> trickTypes)
	{
		Dictionary<sbyte, byte> dictionary = ObjectPool<Dictionary<sbyte, byte>>.Instance.Get();
		dictionary.Clear();
		foreach (sbyte trickType in trickTypes)
		{
			dictionary[trickType] = (byte)Math.Clamp(dictionary.GetOrDefault(trickType) + 1, 0, 255);
		}
		needTricks.Clear();
		foreach (var (type, count) in dictionary)
		{
			needTricks.Add(new NeedTrick(type, count));
		}
		ObjectPool<Dictionary<sbyte, byte>>.Instance.Return(dictionary);
	}

	public bool WeaponHasNeedTrick(CombatCharacter character, short skillTemplateId, CombatWeaponData weaponData)
	{
		sbyte[] weaponTricks = weaponData.GetWeaponTricks();
		List<NeedTrick> list = ObjectPool<List<NeedTrick>>.Instance.Get();
		list.Clear();
		if (DomainManager.CombatSkill.TryGetElement_CombatSkills(new CombatSkillKey(character.GetId(), skillTemplateId), out var element))
		{
			DomainManager.CombatSkill.GetCombatSkillCostTrick(element, list, applySpecialEffect: false);
		}
		else
		{
			list.AddRange(Config.CombatSkill.Instance[skillTemplateId].TrickCost);
		}
		bool result = list.All((NeedTrick x) => weaponTricks.Exist(x.TrickType));
		ObjectPool<List<NeedTrick>>.Instance.Return(list);
		return result;
	}

	public bool HasNeedTrick(CombatCharacter character, GameData.Domains.CombatSkill.CombatSkill skill, bool useConfigValue = false)
	{
		TrickCollection tricks = character.GetTricks();
		List<NeedTrick> list = ObjectPool<List<NeedTrick>>.Instance.Get();
		List<int> list2 = ObjectPool<List<int>>.Instance.Get();
		bool result = true;
		list.Clear();
		if (useConfigValue)
		{
			list.AddRange(Config.CombatSkill.Instance[skill.GetId().SkillTemplateId].TrickCost);
		}
		else
		{
			DomainManager.CombatSkill.GetCombatSkillCostTrick(skill, list);
		}
		RemoveUsableTrickInsteadCostTrick(character, skill.GetId().SkillTemplateId, list);
		RemoveCostTrickInsteadUselessTrick(character, skill.GetId().SkillTemplateId, list, trulyCost: false);
		RemoveCostTrickBySelfShaTrick(Context, character, skill.GetId().SkillTemplateId, list, trulyCost: false);
		RemoveCostTrickByEnemyShaTrick(Context, character, skill.GetId().SkillTemplateId, list);
		RemoveCostTrickByJiTrick(Context, character, skill.GetId().SkillTemplateId, list, trulyCost: false);
		RemoveJiTrickByUselessTrick(Context, character, skill.GetId().SkillTemplateId, list, trulyCost: false);
		list2.Clear();
		list2.AddRange(tricks.Tricks.Keys);
		for (int i = 0; i < list.Count; i++)
		{
			NeedTrick needTrick = list[i];
			if (needTrick.NeedCount <= 0)
			{
				continue;
			}
			int num = 0;
			for (int j = 0; j < list2.Count; j++)
			{
				if (character.TrickEquals(tricks.Tricks[list2[j]], needTrick.TrickType))
				{
					num++;
					if (num == needTrick.NeedCount)
					{
						break;
					}
				}
			}
			if (num < needTrick.NeedCount)
			{
				result = false;
				break;
			}
		}
		ObjectPool<List<int>>.Instance.Return(list2);
		ObjectPool<List<NeedTrick>>.Instance.Return(list);
		return result;
	}

	public void NpcCombatAttack(DataContext context, int attackerId, int defenderId, ItemKey weaponKey, short attackSkillId, short defendSkillId)
	{
		CombatCharacter combatCharacter = _combatCharacterDict[attackerId];
		CombatCharacter combatCharacter2 = _combatCharacterDict[defenderId];
		SetCombatCharacter(context, combatCharacter.IsAlly, attackerId);
		SetCombatCharacter(context, combatCharacter2.IsAlly, defenderId);
		ItemKey[] weapons = combatCharacter.GetWeapons();
		if (!weaponKey.IsValid())
		{
			weaponKey = weapons[3];
		}
		ChangeWeapon(context, combatCharacter, weapons.IndexOf(weaponKey));
		if (defendSkillId >= 0)
		{
			ApplyAgileOrDefenseSkill(combatCharacter2, Config.CombatSkill.Instance[defendSkillId]);
		}
		if (attackSkillId >= 0)
		{
			CombatSkillItem skillConfig = Config.CombatSkill.Instance[attackSkillId];
			CalcSkillQiDisorderAndInjury(combatCharacter, skillConfig);
			combatCharacter.SkillAttackBodyPart = GetAttackBodyPart(combatCharacter, combatCharacter2, context.Random, attackSkillId, -1, -1);
			Events.RaisePrepareSkillEnd(context, attackerId, combatCharacter.IsAlly, attackSkillId);
			CalcAttackSkillDataCompare(CombatContext.Create(combatCharacter, null, -1, attackSkillId));
			Events.RaiseCastAttackSkillBegin(context, combatCharacter, combatCharacter2, attackSkillId);
			for (int i = 0; i < 4; i++)
			{
				if (i == 3 || combatCharacter.SkillHitType[i] >= 0)
				{
					CalcSkillAttack(CombatContext.Create(combatCharacter, null, -1, -1), i);
				}
				if (i < 3)
				{
					combatCharacter.SetAttackSkillAttackIndex((byte)(i + 1), context);
				}
			}
			sbyte power = (sbyte)combatCharacter.GetAttackSkillPower();
			combatCharacter.SetPerformingSkillId(-1, context);
			combatCharacter.SetAttackSkillPower(0, context);
			ClearDamageCompareData(context);
			DomainManager.Combat.RaiseCastSkillEnd(context, attackerId, combatCharacter.IsAlly, attackSkillId, power);
		}
		else
		{
			sbyte[] weaponTricks = combatCharacter.GetWeaponTricks();
			sbyte trickType = weaponTricks[context.Random.Next(0, weaponTricks.Length)];
			combatCharacter.NormalAttackHitType = GetAttackHitType(combatCharacter, trickType);
			combatCharacter.NormalAttackBodyPart = GetAttackBodyPart(combatCharacter, _enemyChar, context.Random, -1, trickType, combatCharacter.NormalAttackHitType);
			UpdateDamageCompareData(CombatContext.Create(combatCharacter, null, -1, -1));
			CalcNormalAttack(CombatContext.Create(combatCharacter, null, -1, -1), trickType);
			combatCharacter2.SetIsFightBack(isFightBack: false, context);
		}
		ClearAffectingDefenseSkill(context, combatCharacter2);
	}

	[Obsolete]
	public void EndNpcCombat(DataContext context)
	{
	}

	public unsafe void NpcSimplifiedAttack(DataContext context, GameData.Domains.Character.Character attacker, GameData.Domains.Character.Character defender, GameData.Domains.CombatSkill.CombatSkill combatSkill, ItemKey weaponKey, CombatType combatType)
	{
		sbyte b = AiHelper.NpcCombat.CombatTypeWeaponAttackInjuryCount[(int)combatType];
		sbyte b2 = AiHelper.NpcCombat.CombatTypePoisonScale[(int)combatType];
		sbyte b3 = Config.Weapon.Instance[weaponKey.TemplateId].DefaultInnerRatio;
		PoisonsAndLevels delta = default(PoisonsAndLevels);
		if (combatSkill != null)
		{
			delta = combatSkill.GetPoisons();
			b = AiHelper.NpcCombat.CombatTypeSkillAttackInjuryCount[(int)combatType];
			b3 = combatSkill.GetCurrInnerRatio();
		}
		if (ModificationStateHelper.IsActive(weaponKey.ModificationState, 1))
		{
			delta.Add(DomainManager.Item.GetAttachedPoisons(weaponKey));
		}
		int num = b * b3 / 100;
		int damage = b - num;
		defender.TakeRandomDamage(context, num, isInnerInjury: true);
		defender.TakeRandomDamage(context, damage, isInnerInjury: false);
		if (b2 != 100)
		{
			for (sbyte b4 = 0; b4 < 6; b4++)
			{
				if (delta.Values[b4] > 0)
				{
					delta.Values[b4] = (short)(delta.Values[b4] * b2 / 100);
				}
			}
		}
		defender.ChangePoisoned(context, ref delta);
		if (combatType == CombatType.Play || combatSkill == null)
		{
			return;
		}
		sbyte direction = combatSkill.GetDirection();
		if (direction == -1)
		{
			return;
		}
		CombatSkillItem combatSkillItem = Config.CombatSkill.Instance[combatSkill.GetId().SkillTemplateId];
		bool flag = direction == 0;
		if (combatSkillItem.AddWugType >= 0)
		{
			GameData.Domains.Character.Character character = (flag ? defender : attacker);
			sbyte npcCombatAddGrowthType = WugEffectBase.GetNpcCombatAddGrowthType(character, combatSkillItem.AddWugType, flag);
			short wugTemplateId = ItemDomain.GetWugTemplateId(combatSkillItem.AddWugType, npcCombatAddGrowthType);
			if (character.GetCreatingType() == 1 && character.GetEatingItems().IndexOfWug(Config.Medicine.Instance[wugTemplateId]) < 0)
			{
				character.AddWug(context, wugTemplateId);
			}
		}
		if (combatSkillItem.AddBreakBodyFeature == null)
		{
			return;
		}
		short num2 = combatSkillItem.AddBreakBodyFeature[(!flag) ? 1u : 0u];
		Injuries injuries = defender.GetInjuries();
		sbyte[] array = BreakFeatureHelper.Feature2BodyPart[num2];
		bool flag2 = false;
		for (int i = 0; i < array.Length; i++)
		{
			if (injuries.Get(array[i], !flag) > 0)
			{
				flag2 = true;
				break;
			}
		}
		if (flag2 && !defender.GetFeatureIds().Contains(num2))
		{
			defender.AddFeature(context, num2);
			DomainManager.SpecialEffect.Add(context, defender.GetId(), SpecialEffectDomain.BreakBodyFeatureEffectClassName[num2]);
		}
	}

	[Obsolete]
	public void NpcAttackQuick(DataContext context, int attackerId, int defenderId, ItemKey weaponKey, ItemKey emptyHandWeaponKey, short attackSkillId, short defendSkillId, sbyte skillPower = 100)
	{
		if (!emptyHandWeaponKey.IsValid())
		{
			throw new Exception("emptyHandWeaponKey cannot be invalid");
		}
		if (!weaponKey.IsValid())
		{
			weaponKey = emptyHandWeaponKey;
		}
		GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(attackerId);
		GameData.Domains.Character.Character element_Objects2 = DomainManager.Character.GetElement_Objects(defenderId);
		GameData.Domains.Item.Weapon element_Weapons = DomainManager.Item.GetElement_Weapons(weaponKey.Id);
		List<sbyte> tricks = element_Weapons.GetTricks();
		sbyte b = tricks[context.Random.Next(0, tricks.Count)];
		sbyte attackHitType = GetAttackHitType(element_Objects, b);
		sbyte npcAttackBodyPart = GetNpcAttackBodyPart(element_Objects, element_Objects2, context.Random, element_Weapons, attackSkillId, (sbyte)((attackSkillId >= 0) ? (-1) : b), attackHitType);
		if (attackSkillId >= 0)
		{
			int num = CalcNpcSkillAttack(context, element_Objects, element_Objects2, npcAttackBodyPart, element_Weapons, emptyHandWeaponKey, attackSkillId, defendSkillId, skillPower);
			if (num < 100)
			{
				return;
			}
			sbyte skillDirection = DomainManager.CombatSkill.GetSkillDirection(attackerId, attackSkillId);
			if (skillDirection == -1)
			{
				return;
			}
			CombatSkillItem combatSkillItem = Config.CombatSkill.Instance[attackSkillId];
			bool flag = skillDirection == 0;
			if (combatSkillItem.AddWugType >= 0)
			{
				GameData.Domains.Character.Character character = (flag ? element_Objects2 : element_Objects);
				sbyte npcCombatAddGrowthType = WugEffectBase.GetNpcCombatAddGrowthType(character, combatSkillItem.AddWugType, flag);
				short wugTemplateId = ItemDomain.GetWugTemplateId(combatSkillItem.AddWugType, npcCombatAddGrowthType);
				if (character.GetEatingItems().IndexOfWug(Config.Medicine.Instance[wugTemplateId]) < 0)
				{
					character.AddWug(context, wugTemplateId);
				}
			}
			if (combatSkillItem.AddBreakBodyFeature == null)
			{
				return;
			}
			short num2 = combatSkillItem.AddBreakBodyFeature[(!flag) ? 1u : 0u];
			Injuries injuries = element_Objects2.GetInjuries();
			sbyte[] array = BreakFeatureHelper.Feature2BodyPart[num2];
			bool flag2 = false;
			for (int i = 0; i < array.Length; i++)
			{
				if (injuries.Get(array[i], !flag) > 0)
				{
					flag2 = true;
					break;
				}
			}
			if (flag2 && !element_Objects2.GetFeatureIds().Contains(num2))
			{
				element_Objects2.AddFeature(context, num2);
				DomainManager.SpecialEffect.Add(context, defenderId, SpecialEffectDomain.BreakBodyFeatureEffectClassName[num2]);
			}
		}
		else
		{
			CalcNpcNormalAttack(context, element_Objects, element_Objects2, b, attackHitType, npcAttackBodyPart, element_Weapons, emptyHandWeaponKey, defendSkillId, skillPower);
		}
	}

	private sbyte GetNpcAttackBodyPart(GameData.Domains.Character.Character attacker, GameData.Domains.Character.Character defender, IRandomSource random, GameData.Domains.Item.Weapon weapon, short skillId = -1, sbyte trickType = -1, sbyte hitType = -1)
	{
		if ((skillId >= 0 && CombatSkillTemplateHelper.IsMindHitSkill(skillId)) || (trickType >= 0 && hitType == 3))
		{
			return -1;
		}
		if (trickType == 21)
		{
			trickType = GodTrickUseTrickType[hitType];
		}
		Injuries injuries = defender.GetInjuries();
		sbyte b = ((skillId >= 0) ? DomainManager.CombatSkill.GetElement_CombatSkills(new CombatSkillKey(attacker.GetId(), skillId)).GetCurrInnerRatio() : Config.Weapon.Instance[weapon.GetTemplateId()].DefaultInnerRatio);
		sbyte result = -1;
		List<sbyte> list = ObjectPool<List<sbyte>>.Instance.Get();
		sbyte[] collection = null;
		if (skillId >= 0 && Config.CombatSkill.Instance[skillId].InjuryPartAtkRateDistribution != null)
		{
			collection = Config.CombatSkill.Instance[skillId].InjuryPartAtkRateDistribution;
		}
		else if (trickType >= 0 && Config.TrickType.Instance[trickType].InjuryPartAtkRateDistribution != null)
		{
			collection = Config.TrickType.Instance[trickType].InjuryPartAtkRateDistribution;
		}
		list.Clear();
		list.AddRange(collection);
		if (list.Count > 0)
		{
			for (sbyte b2 = 0; b2 < 7; b2++)
			{
				if ((b == 100 || injuries.Get(b2, isInnerInjury: false) >= 6) && (b == 0 || injuries.Get(b2, isInnerInjury: true) >= 6))
				{
					list[b2] = 0;
				}
			}
			int num = list.ToArray().Sum();
			if (num == 0)
			{
				list.Clear();
				list.AddRange(collection);
				num = list.ToArray().Sum();
			}
			int num2 = random.Next(num);
			int num3 = 0;
			for (sbyte b3 = 0; b3 < 7; b3++)
			{
				num3 += list[b3];
				if (num3 > num2)
				{
					result = b3;
					break;
				}
			}
		}
		ObjectPool<List<sbyte>>.Instance.Return(list);
		return result;
	}

	private unsafe int GetNpcAttackHitValue(GameData.Domains.Character.Character attacker, GameData.Domains.Item.Weapon weapon, ItemKey emptyHandWeaponKey, sbyte hitType, short attackSkillId = -1, int skillAddPercent = 0)
	{
		HitOrAvoidInts hitValues = attacker.GetHitValues();
		int id = attacker.GetId();
		int num = hitValues.Items[hitType];
		int num2 = 100;
		if (attackSkillId < 0 || Config.CombatSkill.Instance[attackSkillId].Type != 5)
		{
			HitOrAvoidShorts hitFactors = weapon.GetHitFactors(id);
			num2 += hitFactors.Items[hitType];
		}
		else
		{
			ItemKey itemKey = attacker.GetEquipment()[7];
			GameData.Domains.Item.Armor armor = (itemKey.IsValid() ? DomainManager.Item.GetElement_Armors(itemKey.Id) : null);
			if (armor != null && armor.GetCurrDurability() > 0)
			{
				short relatedWeapon = Config.Armor.Instance[itemKey.TemplateId].RelatedWeapon;
				ItemKey itemKey2 = new ItemKey(0, 0, relatedWeapon, -1);
				HitOrAvoidShorts baseHitFactors = Config.Weapon.Instance[relatedWeapon].BaseHitFactors;
				num2 += baseHitFactors.Items[hitType] * DomainManager.Character.GetItemPower(id, itemKey2) / 100;
			}
			else
			{
				GameData.Domains.Item.Weapon element_Weapons = DomainManager.Item.GetElement_Weapons(emptyHandWeaponKey.Id);
				HitOrAvoidShorts hitFactors2 = element_Weapons.GetHitFactors(id);
				num2 += hitFactors2.Items[hitType];
			}
		}
		num2 = Math.Max(num2, 33);
		num = num * num2 / 100;
		num = num * (100 + skillAddPercent) / 100;
		return Math.Max(num, 0);
	}

	private unsafe int GetNpcAttackAvoidValue(GameData.Domains.Character.Character defender, sbyte hitType, sbyte bodyPart, short defendSkillId, sbyte skillPower)
	{
		HitOrAvoidInts avoidValues = defender.GetAvoidValues();
		int id = defender.GetId();
		int num = avoidValues.Items[hitType];
		if (defendSkillId >= 0)
		{
			GameData.Domains.CombatSkill.CombatSkill element_CombatSkills = DomainManager.CombatSkill.GetElement_CombatSkills(new CombatSkillKey(id, defendSkillId));
			HitOrAvoidInts addAvoidValueOnCast = element_CombatSkills.GetAddAvoidValueOnCast();
			num += GlobalConfig.Instance.DefendSkillBaseAddAvoid * addAvoidValueOnCast.Items[hitType] / 100;
		}
		int num2 = 100;
		if (bodyPart >= 0)
		{
			ItemKey itemKey = defender.GetEquipment()[EquipmentSlotHelper.GetSlotByBodyPartType(bodyPart)];
			if (itemKey.IsValid())
			{
				GameData.Domains.Item.Armor element_Armors = DomainManager.Item.GetElement_Armors(itemKey.Id);
				if (element_Armors.GetCurrDurability() > 0)
				{
					HitOrAvoidShorts avoidFactors = element_Armors.GetAvoidFactors(id);
					num2 += avoidFactors.Items[hitType];
				}
			}
		}
		num2 = Math.Max(num2, 33);
		num = num * num2 / 100;
		return Math.Max(num, 1);
	}

	private int GetNpcAttackPenetrate(GameData.Domains.Character.Character attacker, GameData.Domains.Character.Character defender, bool inner, GameData.Domains.Item.Weapon weapon, ItemKey emptyHandWeaponKey, sbyte bodyPart, short attackSkillId = -1, int skillAddPercent = 0)
	{
		int id = attacker.GetId();
		int num = (inner ? attacker.GetPenetrations().Inner : attacker.GetPenetrations().Outer);
		bool flag = attackSkillId >= 0 && Config.CombatSkill.Instance[attackSkillId].Type == 5;
		ItemKey itemKey = attacker.GetEquipment()[7];
		GameData.Domains.Item.Armor armor = (itemKey.IsValid() ? DomainManager.Item.GetElement_Armors(itemKey.Id) : null);
		int num2;
		int num3;
		if (!flag)
		{
			num2 = weapon.GetPenetrationFactor() * DomainManager.Character.GetItemPower(id, weapon.GetItemKey()) / 100;
			num3 = num2 * Config.Weapon.Instance[weapon.GetTemplateId()].DefaultInnerRatio / 100;
		}
		else if (armor != null && armor.GetCurrDurability() > 0)
		{
			short relatedWeapon = Config.Armor.Instance[itemKey.TemplateId].RelatedWeapon;
			ItemKey itemKey2 = new ItemKey(0, 0, relatedWeapon, -1);
			WeaponItem weaponItem = Config.Weapon.Instance[relatedWeapon];
			num2 = weaponItem.BasePenetrationFactor * DomainManager.Character.GetItemPower(id, itemKey2) / 100;
			num3 = num2 * weaponItem.DefaultInnerRatio / 100;
		}
		else
		{
			GameData.Domains.Item.Weapon element_Weapons = DomainManager.Item.GetElement_Weapons(emptyHandWeaponKey.Id);
			num2 = element_Weapons.GetPenetrationFactor() * DomainManager.Character.GetItemPower(id, emptyHandWeaponKey) / 100;
			num3 = num2 * Config.Weapon.Instance[emptyHandWeaponKey.TemplateId].DefaultInnerRatio / 100;
		}
		int num4 = (inner ? num3 : (num2 - num3));
		if (bodyPart >= 0)
		{
			ItemKey itemKey3 = defender.GetEquipment()[EquipmentSlotHelper.GetSlotByBodyPartType(bodyPart)];
			GameData.Domains.Item.Armor armor2 = (itemKey3.IsValid() ? DomainManager.Item.GetElement_Armors(itemKey3.Id) : null);
			int num5 = ((!flag) ? weapon.GetEquipmentDefense() : ((armor != null && armor.GetCurrDurability() > 0) ? armor.GetEquipmentDefense() : 50));
			int num6 = ((armor2 != null && armor2.GetCurrDurability() > 0) ? armor2.GetEquipmentAttack() : 100);
			if (num6 > num5)
			{
				num4 -= num4 * Math.Min(20 + 10 * num6 / Math.Max(num5, 1), 100) / 100;
			}
		}
		num += num4;
		num = num * (100 + skillAddPercent) / 100;
		return Math.Max(num, 0);
	}

	private unsafe int GetNpcAttackPenetrateResist(GameData.Domains.Character.Character attacker, GameData.Domains.Character.Character defender, bool inner, GameData.Domains.Item.Weapon weapon, sbyte bodyPart, short defendSkillId, sbyte skillPower, short attackSkillId = -1)
	{
		int id = defender.GetId();
		int num = (inner ? defender.GetPenetrationResists().Inner : defender.GetPenetrationResists().Outer);
		PoisonInts poisoned = defender.GetPoisoned();
		int num2 = 0;
		if (bodyPart >= 0)
		{
			ItemKey itemKey = defender.GetEquipment()[EquipmentSlotHelper.GetSlotByBodyPartType(bodyPart)];
			GameData.Domains.Item.Armor armor = (itemKey.IsValid() ? DomainManager.Item.GetElement_Armors(itemKey.Id) : null);
			if (armor != null)
			{
				OuterAndInnerShorts penetrationResistFactors = armor.GetPenetrationResistFactors();
				num2 = (inner ? penetrationResistFactors.Inner : penetrationResistFactors.Outer) * DomainManager.Character.GetItemPower(id, itemKey) / 100;
			}
			else
			{
				num2 = 0;
			}
			int num3 = weapon.GetEquipmentAttack();
			int num4 = ((armor != null && armor.GetCurrDurability() > 0) ? armor.GetEquipmentDefense() : 50);
			if (attackSkillId >= 0 && Config.CombatSkill.Instance[attackSkillId].Type == 5)
			{
				ItemKey itemKey2 = attacker.GetEquipment()[7];
				GameData.Domains.Item.Armor armor2 = (itemKey2.IsValid() ? DomainManager.Item.GetElement_Armors(itemKey2.Id) : null);
				num3 = ((armor2 != null && armor2.GetCurrDurability() > 0) ? armor2.GetEquipmentAttack() : 100);
			}
			if (num3 > num4)
			{
				num2 -= num2 * Math.Min(20 + 10 * num3 / Math.Max(num4, 1), 100) / 100;
			}
		}
		num += num2;
		if (defendSkillId >= 0)
		{
			GameData.Domains.CombatSkill.CombatSkill element_CombatSkills = DomainManager.CombatSkill.GetElement_CombatSkills(new CombatSkillKey(id, defendSkillId));
			OuterAndInnerInts addPenetrateResist = element_CombatSkills.GetAddPenetrateResist();
			num += (inner ? addPenetrateResist.Inner : addPenetrateResist.Outer);
		}
		int num5 = 100;
		sbyte b = (sbyte)(inner ? 5 : 4);
		sbyte b2 = PoisonsAndLevels.CalcPoisonedLevel(poisoned.Items[b]);
		if (b2 > 0)
		{
			num5 = ((!inner) ? (num5 - b2 * Poison.Instance[b].ReduceOuterResist) : (num5 - b2 * Poison.Instance[b].ReduceInnerResist));
		}
		num5 = Math.Max(num5, 33);
		num = num * num5 / 100;
		return Math.Max(num, 1);
	}

	private unsafe void CalcNpcNormalAttack(DataContext context, GameData.Domains.Character.Character attacker, GameData.Domains.Character.Character defender, sbyte trickType, sbyte hitType, sbyte bodyPart, GameData.Domains.Item.Weapon weapon, ItemKey emptyHandWeaponKey, short defendSkillId, sbyte skillPower)
	{
		int npcAttackHitValue = GetNpcAttackHitValue(attacker, weapon, emptyHandWeaponKey, hitType, -1);
		int npcAttackAvoidValue = GetNpcAttackAvoidValue(defender, hitType, bodyPart, defendSkillId, skillPower);
		int percentProb = npcAttackHitValue * 100 / npcAttackAvoidValue / ((npcAttackHitValue >= npcAttackAvoidValue) ? 1 : 2);
		if (!context.Random.CheckPercentProb(percentProb) || TrickType.NoBodyDamageTrickType.Exist(trickType))
		{
			return;
		}
		if (hitType != 3)
		{
			CalcNpcAttackInjury(context, attacker, defender, weapon, bodyPart, npcAttackHitValue, npcAttackAvoidValue, Config.Weapon.Instance[weapon.GetTemplateId()].DefaultInnerRatio, GetNpcAttackPenetrate(attacker, defender, inner: false, weapon, emptyHandWeaponKey, bodyPart, -1), GetNpcAttackPenetrateResist(attacker, defender, inner: false, weapon, bodyPart, defendSkillId, skillPower, -1), GetNpcAttackPenetrate(attacker, defender, inner: true, weapon, emptyHandWeaponKey, bodyPart, -1), GetNpcAttackPenetrateResist(attacker, defender, inner: true, weapon, bodyPart, defendSkillId, skillPower, -1), -1);
		}
		PoisonsAndLevels innatePoisons = weapon.GetInnatePoisons();
		for (sbyte b = 0; b < 6; b++)
		{
			if (innatePoisons.Levels[b] > 0)
			{
				defender.ChangePoisoned(context, b, innatePoisons.Levels[b], innatePoisons.Values[b]);
			}
		}
	}

	private unsafe int CalcNpcSkillAttack(DataContext context, GameData.Domains.Character.Character attacker, GameData.Domains.Character.Character defender, sbyte bodyPart, GameData.Domains.Item.Weapon weapon, ItemKey emptyHandWeaponKey, short attackSkillId, short defendSkillId, sbyte skillPower)
	{
		GameData.Domains.CombatSkill.CombatSkill element_CombatSkills = DomainManager.CombatSkill.GetElement_CombatSkills(new CombatSkillKey(attacker.GetId(), attackSkillId));
		HitOrAvoidInts hitValue = element_CombatSkills.GetHitValue();
		int num = 0;
		if (!CombatSkillTemplateHelper.IsMindHitSkill(attackSkillId))
		{
			HitOrAvoidInts hitDistribution = element_CombatSkills.GetHitDistribution();
			List<int> list = ObjectPool<List<int>>.Instance.Get();
			List<int> list2 = ObjectPool<List<int>>.Instance.Get();
			int num2 = int.MinValue;
			int num3 = int.MinValue;
			list.Clear();
			list2.Clear();
			for (sbyte b = 2; b >= 0; b--)
			{
				int npcAttackHitValue = GetNpcAttackHitValue(attacker, weapon, emptyHandWeaponKey, b, attackSkillId, hitValue.Items[b] * skillPower / 100);
				int npcAttackAvoidValue = GetNpcAttackAvoidValue(defender, b, bodyPart, defendSkillId, skillPower);
				if (npcAttackHitValue > num2 || (npcAttackHitValue == num2 && npcAttackAvoidValue > num3))
				{
					num2 = npcAttackHitValue;
					num3 = npcAttackAvoidValue;
				}
				list.Add(npcAttackHitValue);
				list2.Add(npcAttackAvoidValue);
			}
			for (int i = 0; i < 3; i++)
			{
				int num4 = list[i];
				int num5 = list2[i];
				int percentProb = num4 * 100 / num5 / ((num4 >= num5) ? 1 : 2);
				if (context.Random.CheckPercentProb(percentProb))
				{
					num += hitDistribution.Items[i];
				}
			}
			ObjectPool<List<int>>.Instance.Return(list);
			ObjectPool<List<int>>.Instance.Return(list2);
			if (num > 0)
			{
				OuterAndInnerInts penetrations = element_CombatSkills.GetPenetrations();
				CalcNpcAttackInjury(context, attacker, defender, weapon, bodyPart, num2, num3, element_CombatSkills.GetCurrInnerRatio(), GetNpcAttackPenetrate(attacker, defender, inner: false, weapon, emptyHandWeaponKey, bodyPart, attackSkillId, penetrations.Outer * skillPower / 100), GetNpcAttackPenetrateResist(attacker, defender, inner: false, weapon, bodyPart, defendSkillId, skillPower, attackSkillId), GetNpcAttackPenetrate(attacker, defender, inner: true, weapon, emptyHandWeaponKey, bodyPart, attackSkillId, penetrations.Inner * skillPower / 100), GetNpcAttackPenetrateResist(attacker, defender, inner: true, weapon, bodyPart, defendSkillId, skillPower, attackSkillId), attackSkillId, num);
			}
		}
		else
		{
			int npcAttackHitValue2 = GetNpcAttackHitValue(attacker, weapon, emptyHandWeaponKey, 3, attackSkillId, hitValue.Items[3] * skillPower / 100);
			int npcAttackAvoidValue2 = GetNpcAttackAvoidValue(defender, 3, bodyPart, defendSkillId, skillPower);
			int percentProb2 = npcAttackHitValue2 * 100 / npcAttackAvoidValue2 / ((npcAttackHitValue2 >= npcAttackAvoidValue2) ? 1 : 2);
			num = (context.Random.CheckPercentProb(percentProb2) ? 100 : 0);
		}
		if (num > 0)
		{
			PoisonsAndLevels poisons = element_CombatSkills.GetPoisons();
			for (sbyte b2 = 0; b2 < 6; b2++)
			{
				if (poisons.Levels[b2] > 0)
				{
					defender.ChangePoisoned(context, b2, poisons.Levels[b2], poisons.Values[b2] * num / 100);
				}
			}
		}
		return num;
	}

	private void CalcNpcAttackInjury(DataContext context, GameData.Domains.Character.Character attacker, GameData.Domains.Character.Character defender, GameData.Domains.Item.Weapon weapon, sbyte bodyPart, int hitValue, int avoidValue, sbyte innerRatio, int outerPenetrate, int outerPenetrateResist, int innerPenetrate, int innerPenetrateResist, short attackSkillId = -1, int attackSkillPower = 100)
	{
		if (bodyPart < 0)
		{
			return;
		}
		Injuries injuries = defender.GetInjuries();
		DamageStepCollection damageStepCollection = GetDamageStepCollection(defender.GetId());
		int num = outerPenetrate * attackSkillPower / outerPenetrateResist / ((outerPenetrate >= outerPenetrateResist) ? 1 : 2);
		int num2 = innerPenetrate * attackSkillPower / innerPenetrateResist / ((innerPenetrate >= innerPenetrateResist) ? 1 : 2);
		int hitOdds = CFormula.FormulaCalcHitOdds(hitValue, avoidValue);
		int num3 = (CheckCritical(context.Random, attacker.GetId(), hitOdds) ? CFormula.FormulaCalcCriticalPercent(hitOdds) : 100);
		int num4 = CFormula.CalcBaseDamageValue((attackSkillId >= 0) ? CFormula.EAttackType.Skill : CFormula.EAttackType.Normal, weapon.GetAttackPreparePointCost());
		int num5 = num4 * (60 + 80 * Math.Abs(innerRatio - 50) / 100) / 100;
		int damage = ((innerRatio < 100) ? (num5 * (num4 / 2 + num / 4) / 100 * num3 / 100) : 0);
		int damage2 = ((innerRatio > 0) ? (num5 * (num4 / 2 + num2 / 4) / 100 * num3 / 100) : 0);
		int num6 = ((innerRatio < 100) ? CalcInjuryMarkCount(damage, damageStepCollection.OuterDamageSteps[bodyPart], 6 - injuries.Get(bodyPart, isInnerInjury: false)).markCount : 0);
		int num7 = ((innerRatio > 0) ? CalcInjuryMarkCount(damage2, damageStepCollection.InnerDamageSteps[bodyPart], 6 - injuries.Get(bodyPart, isInnerInjury: true)).markCount : 0);
		ItemKey itemKey = defender.GetEquipment()[EquipmentSlotHelper.GetSlotByBodyPartType(bodyPart)];
		GameData.Domains.Item.Armor armor = (itemKey.IsValid() ? DomainManager.Item.GetElement_Armors(itemKey.Id) : null);
		if (armor != null && armor.GetCurrDurability() > 0)
		{
			int num8 = weapon.GetEquipmentAttack();
			if (attackSkillId >= 0 && Config.CombatSkill.Instance[attackSkillId].Type == 5)
			{
				ItemKey itemKey2 = attacker.GetEquipment()[7];
				GameData.Domains.Item.Armor armor2 = (itemKey2.IsValid() ? DomainManager.Item.GetElement_Armors(itemKey2.Id) : null);
				num8 = ((armor2 != null && armor2.GetCurrDurability() > 0) ? armor2.GetEquipmentAttack() : 100);
			}
			if (num8 <= armor.GetEquipmentDefense())
			{
				OuterAndInnerShorts injuryFactor = armor.GetInjuryFactor();
				num6 -= injuryFactor.Outer;
				num7 -= injuryFactor.Inner;
			}
		}
		if (num6 > 0 || num7 > 0)
		{
			if (num6 > 0)
			{
				injuries.Change(bodyPart, isInnerInjury: false, (sbyte)num6);
			}
			if (num7 > 0)
			{
				injuries.Change(bodyPart, isInnerInjury: true, (sbyte)num7);
			}
			defender.SetInjuries(injuries, context);
		}
	}

	[DomainMethod]
	public void StartPrepareOtherAction(DataContext context, sbyte actionType, bool isAlly = true)
	{
		if (!CanAcceptCommand())
		{
			return;
		}
		CombatCharacter combatCharacter = (isAlly ? _selfChar : _enemyChar);
		if (combatCharacter.GetOtherActionCanUse()[actionType])
		{
			bool flag = !combatCharacter.HasDoingOrReserveCommand();
			combatCharacter.SetNeedUseOtherAction(context, actionType);
			combatCharacter.MoveData.ResetJumpState(context);
			if (flag)
			{
				UpdateAllCommandAvailability(context, combatCharacter);
			}
			else
			{
				UpdateOtherActionCanUse(context, combatCharacter, -1);
			}
		}
	}

	[DomainMethod]
	public void InterruptSurrender()
	{
		if (IsInCombat() && _selfChar.GetPreparingOtherAction() == 4)
		{
			_selfChar.NeedInterruptSurrender = true;
		}
	}

	private void UpdateOtherActionCanUse(DataContext context, CombatCharacter character, sbyte actionType = -1)
	{
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		bool[] otherActionCanUse = character.GetOtherActionCanUse();
		bool flag = false;
		BoolArray32 val;
		if (actionType < 0 || actionType == 0)
		{
			int num;
			if (character.GetHealInjuryCount() > 0 && !character.PreparingTeammateCommand())
			{
				if (character.GetInjuries().HasAnyInjury())
				{
					val = GetHealInjuryBanReason(character, character);
					num = ((!((BoolArray32)(ref val)).Any()) ? 1 : 0);
				}
				else
				{
					num = 1;
				}
			}
			else
			{
				num = 0;
			}
			bool flag2 = (byte)num != 0;
			if (otherActionCanUse[0] != flag2)
			{
				otherActionCanUse[0] = flag2;
				flag = true;
			}
		}
		if (actionType < 0 || actionType == 1)
		{
			int num2;
			if (character.GetHealPoisonCount() > 0 && !character.PreparingTeammateCommand())
			{
				if (character.GetPoison().IsNonZero())
				{
					val = GetHealPoisonBanReason(character, character);
					num2 = ((!((BoolArray32)(ref val)).Any()) ? 1 : 0);
				}
				else
				{
					num2 = 1;
				}
			}
			else
			{
				num2 = 0;
			}
			bool flag2 = (byte)num2 != 0;
			if (otherActionCanUse[1] != flag2)
			{
				otherActionCanUse[1] = flag2;
				flag = true;
			}
		}
		if (actionType < 0 || actionType == 2)
		{
			bool flag2 = CanFlee(character.IsAlly) && !character.PreparingTeammateCommand() && GetCurrentDistance() >= FleeNeedDistance;
			if (otherActionCanUse[2] != flag2)
			{
				otherActionCanUse[2] = flag2;
				flag = true;
			}
		}
		if (actionType < 0 || actionType == 3)
		{
			bool flag2 = _carrierAnimalCombatCharId >= 0 && !character.PreparingTeammateCommand() && character.GetAnimalAttackCount() > 0;
			if (otherActionCanUse[3] != flag2)
			{
				otherActionCanUse[3] = flag2;
				flag = true;
			}
		}
		if (actionType < 0 || actionType == 4)
		{
			bool flag2 = character.GetCanSurrender();
			if (otherActionCanUse[4] != flag2)
			{
				otherActionCanUse[4] = flag2;
				flag = true;
			}
		}
		if (flag)
		{
			character.SetOtherActionCanUse(otherActionCanUse, context);
		}
	}

	public void InterruptOtherAction(DataContext context, CombatCharacter character)
	{
		if (character.GetPreparingOtherAction() == 2)
		{
			DomainManager.Combat.ShowSpecialEffectTips(character.GetId(), 1456, 0);
		}
		Events.RaiseInterruptOtherAction(context, character, character.GetPreparingOtherAction());
		character.SetPreparingOtherAction(-1, context);
		SetProperLoopAniAndParticle(context, character);
	}

	public bool CanFlee(bool isAlly)
	{
		return isAlly ? CombatConfig.SelfCanFlee : CombatConfig.EnemyCanFlee;
	}

	public unsafe void SetPoisons(DataContext context, CombatCharacter character, PoisonInts poisons, bool updateDefeatMark = true)
	{
		PoisonInts oldPoison = character.GetOldPoison();
		bool flag = false;
		for (sbyte b = 0; b < 6; b++)
		{
			if (poisons.Items[b] < oldPoison.Items[b])
			{
				oldPoison.Items[b] = poisons.Items[b];
				flag = true;
			}
		}
		if (flag)
		{
			character.SetOldPoison(ref oldPoison, context);
		}
		character.SetPoison(ref poisons, context);
		character.SyncPoisonData(context);
		if (updateDefeatMark)
		{
			UpdatePoisonDefeatMark(context, character);
		}
		UpdateOtherActionCanUse(context, character, 1);
		if (IsMainCharacter(character))
		{
			UpdateAllTeammateCommandUsable(context, character.IsAlly, ETeammateCommandImplement.HealPoison);
		}
	}

	public unsafe void AddPoison(DataContext context, CombatCharacter attacker, CombatCharacter defender, sbyte poisonType, sbyte level, int addValue, short skillId = -1, bool applySpecialEffect = true, bool canBounce = true, ItemKey equipKey = default(ItemKey), bool isDirectPoison = false, bool ignorePositiveResist = false, bool forceChangeToOld = false)
	{
		//IL_011c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0129: Unknown result type (might be due to invalid IL or missing references)
		//IL_012e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0130: Unknown result type (might be due to invalid IL or missing references)
		//IL_0153: Unknown result type (might be due to invalid IL or missing references)
		//IL_0158: Unknown result type (might be due to invalid IL or missing references)
		//IL_015d: Unknown result type (might be due to invalid IL or missing references)
		//IL_015f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0187: Unknown result type (might be due to invalid IL or missing references)
		//IL_018c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0191: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01de: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0307: Unknown result type (might be due to invalid IL or missing references)
		//IL_030c: Unknown result type (might be due to invalid IL or missing references)
		//IL_030e: Unknown result type (might be due to invalid IL or missing references)
		//IL_032d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0332: Unknown result type (might be due to invalid IL or missing references)
		//IL_0337: Unknown result type (might be due to invalid IL or missing references)
		//IL_0339: Unknown result type (might be due to invalid IL or missing references)
		//IL_0358: Unknown result type (might be due to invalid IL or missing references)
		//IL_035d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0362: Unknown result type (might be due to invalid IL or missing references)
		//IL_0365: Unknown result type (might be due to invalid IL or missing references)
		int num = attacker?.GetId() ?? (-1);
		defender = DomainManager.SpecialEffect.ModifyData(num, skillId, 246, defender);
		defender = DomainManager.SpecialEffect.ModifyData(defender.GetId(), skillId, 247, defender);
		int id = defender.GetId();
		if (defender.GetCharacter().GetPoisonImmunities()[poisonType] || DomainManager.Extra.HasPoisonImmunity(id, poisonType))
		{
			return;
		}
		PoisonInts poison = defender.GetPoison();
		int num2 = defender.GetPoisonResist()[poisonType];
		if (ignorePositiveResist && num2 > 0 && num2 < 1000)
		{
			num2 = 0;
		}
		if (applySpecialEffect)
		{
			poisonType = (sbyte)DomainManager.SpecialEffect.ModifyData(num, skillId, (ushort)81, (int)poisonType, -1, -1, -1);
			level = (sbyte)(level + DomainManager.SpecialEffect.GetModifyValue(num, skillId, 72, (EDataModifyType)0, poisonType, -1, -1, (EDataSumType)0));
			level = (sbyte)(level + DomainManager.SpecialEffect.GetModifyValue(defender.GetId(), skillId, 105, (EDataModifyType)0, poisonType, -1, -1, (EDataSumType)0));
			level = (sbyte)Math.Clamp((int)level, 0, 3);
			addValue *= CFormulaHelper.CalcConsummateChangeDamagePercent(attacker, defender);
			CValuePercentBonus val = CValuePercentBonus.op_Implicit(0);
			val += CValuePercentBonus.op_Implicit(DomainManager.SpecialEffect.GetModifyValue(num, skillId, 73, (EDataModifyType)1, poisonType, equipKey.ItemType, equipKey.Id, (EDataSumType)0));
			val += CValuePercentBonus.op_Implicit(DomainManager.SpecialEffect.GetModifyValue(defender.GetId(), skillId, 106, (EDataModifyType)1, poisonType, equipKey.ItemType, equipKey.Id, (EDataSumType)0));
			GameData.Domains.Character.Character character = attacker.GetCharacter();
			foreach (SolarTermItem item in character.GetInvokedSolarTerm())
			{
				if (item.PoisonBuffType == poisonType)
				{
					val += character.GetSolarTermBonus(GlobalConfig.Instance.SolarTermAddPoisonEffect);
				}
			}
			addValue *= val;
			(int, int) totalPercentModifyValue = DomainManager.SpecialEffect.GetTotalPercentModifyValue(num, -1, 73, poisonType);
			(int, int) totalPercentModifyValue2 = DomainManager.SpecialEffect.GetTotalPercentModifyValue(defender.GetId(), -1, 106, poisonType);
			addValue = addValue * (100 + totalPercentModifyValue.Item1 + totalPercentModifyValue.Item2 + totalPercentModifyValue2.Item1 + totalPercentModifyValue2.Item2) / 100;
			addValue = DomainManager.SpecialEffect.ModifyData(defender.GetId(), skillId, 106, addValue, poisonType);
			if (!DomainManager.SpecialEffect.ModifyData(defender.GetId(), skillId, 159, dataValue: true, poisonType))
			{
				return;
			}
			sbyte poisonLevel = PoisonsAndLevels.CalcPoisonedLevel(defender.GetPoison()[poisonType]);
			num2 += attacker.CalcAccessoryReducePoisonResist(poisonType, poisonLevel);
			num2 += DomainManager.SpecialEffect.GetModifyValue(attacker.GetId(), skillId, 233, (EDataModifyType)0, poisonType, num2, -1, (EDataSumType)0);
			num2 += DomainManager.SpecialEffect.GetModifyValue(defender.GetId(), skillId, 232, (EDataModifyType)0, poisonType, num2, -1, (EDataSumType)0);
			val = CValuePercentBonus.op_Implicit(0);
			val += CValuePercentBonus.op_Implicit(DomainManager.SpecialEffect.GetModifyValue(attacker.GetId(), skillId, 233, (EDataModifyType)1, poisonType, num2, -1, (EDataSumType)0));
			val += CValuePercentBonus.op_Implicit(DomainManager.SpecialEffect.GetModifyValue(defender.GetId(), skillId, 232, (EDataModifyType)1, poisonType, num2, -1, (EDataSumType)0));
			num2 *= val;
		}
		addValue = PoisonsAndLevels.CalcPoisonDelta(addValue, level, poison.Items[poisonType], num2);
		if (addValue > 0 && poison.Items[poisonType] < 25000 && num2 < 1000)
		{
			PoisonsAndLevels newPoisonsToShow = defender.GetNewPoisonsToShow();
			poison.Items[poisonType] = Math.Clamp(poison.Items[poisonType] + addValue, 0, 25000);
			newPoisonsToShow.Levels[poisonType] = Math.Max(newPoisonsToShow.Levels[poisonType], level);
			newPoisonsToShow.Values[poisonType] = (short)(newPoisonsToShow.Values[poisonType] + addValue);
			SetPoisons(context, defender, poison, updateDefeatMark: false);
			bool flag = num >= 0 && DomainManager.SpecialEffect.ModifyData(num, skillId, 78, dataValue: false, poisonType);
			if (flag || forceChangeToOld)
			{
				ChangeToOldPoison(context, defender, poisonType, addValue);
			}
			Events.RaiseAddPoison(context, num, defender.GetId(), poisonType, level, addValue, skillId, canBounce);
			int markCount = UpdatePoisonDefeatMark(context, defender, poisonType);
			if (isDirectPoison)
			{
				Events.RaiseAddDirectPoisonMark(context, attacker, defender, poisonType, skillId, markCount);
			}
			defender.SetNewPoisonsToShow(ref newPoisonsToShow, context);
		}
	}

	public int ReducePoison(DataContext context, CombatCharacter character, sbyte poisonType, int reduceValue, bool applySpecialEffect = true, bool canReduceOld = false)
	{
		PoisonInts poison = character.GetPoison();
		PoisonInts oldPoison = character.GetOldPoison();
		if (applySpecialEffect)
		{
			reduceValue = ApplyReducePoisonEffect(character.GetId(), poisonType, reduceValue);
		}
		if (reduceValue > 0)
		{
			int num = poison[poisonType];
			int val = ((!canReduceOld) ? oldPoison[poisonType] : 0);
			int num2 = (poison[poisonType] = Math.Max(poison[poisonType] - reduceValue, val));
			SetPoisons(context, character, poison);
			reduceValue = num - num2;
		}
		return reduceValue;
	}

	private void AddDirectPoison(DataContext context, CombatCharacter attacker, CombatCharacter defender, PoisonsAndLevels poisons, CValuePercent ratio, short skillId = -1, ItemKey weaponKey = default(ItemKey))
	{
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		for (sbyte b = 0; b < 6; b++)
		{
			var (num, level) = poisons.GetValueAndLevel(b);
			if (num > 0)
			{
				AddPoison(context, attacker, defender, b, level, (int)num * ratio, skillId, applySpecialEffect: true, canBounce: true, weaponKey, isDirectPoison: true);
			}
		}
	}

	private int ApplyReducePoisonEffect(int charId, sbyte poisonType, int reduceValue, bool getCost = false)
	{
		reduceValue = DomainManager.SpecialEffect.ModifyData(charId, -1, 161, reduceValue, poisonType, getCost ? 1 : 0);
		if (!DomainManager.SpecialEffect.ModifyData(charId, -1, 160, dataValue: true, poisonType))
		{
			reduceValue = 0;
		}
		return reduceValue;
	}

	public void PoisonAffect(DataContext context, CombatCharacter character, sbyte poisonType)
	{
		int num = 1 + DomainManager.SpecialEffect.GetModifyValue(character.GetId(), 163, (EDataModifyType)0, -1, -1, -1, (EDataSumType)0);
		for (int i = 0; i < num; i++)
		{
			PoisonAffectInternal(context, character, poisonType);
		}
	}

	private unsafe void PoisonAffectInternal(DataContext context, CombatCharacter character, sbyte poisonType)
	{
		//IL_0134: Unknown result type (might be due to invalid IL or missing references)
		//IL_0139: Unknown result type (might be due to invalid IL or missing references)
		if (!DomainManager.SpecialEffect.ModifyData(character.GetId(), -1, 162, dataValue: true, poisonType))
		{
			return;
		}
		int poisoned = character.GetPoison().Items[poisonType];
		sbyte b = PoisonsAndLevels.CalcPoisonedLevel(poisoned);
		if (b == 0)
		{
			return;
		}
		switch (poisonType)
		{
		case 0:
		case 1:
		{
			List<sbyte> list = ObjectPool<List<sbyte>>.Instance.Get();
			Injuries injuries = character.GetInjuries();
			bool flag = poisonType == 1;
			list.Clear();
			for (sbyte b2 = 0; b2 < 7; b2++)
			{
				if (injuries.Get(b2, flag) < 6)
				{
					list.Add(b2);
				}
			}
			if (list.Count > 0 && b > 0)
			{
				sbyte random = list.GetRandom(context.Random);
				AddInjury(context, character, random, flag, (sbyte)Math.Min(b, 6 - injuries.Get(random, flag)));
				UpdateBodyDefeatMark(context, character, random);
			}
			ObjectPool<List<sbyte>>.Instance.Return(list);
			break;
		}
		case 2:
		case 3:
			character.WorsenRandomInjury(context, poisonType == 2, WorsenConstants.CalcPoisonPercent(CValueMultiplier.op_Implicit((int)b)));
			break;
		}
		CalcMixPoisonEffects(context, character, poisonType);
		Events.RaisePoisonAffected(context, character.GetId(), poisonType);
		PoisonProduce(context, character, poisonType);
		PoisonProduceWeaken(context, character, poisonType);
		ShowSpecialEffectTips(character.GetId(), 1466 + poisonType, 0);
	}

	public void PoisonProduce(DataContext context, CombatCharacter combatChar, sbyte poisonType, int multiplier = 1)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		PoisonItem poisonItem = Poison.Instance[poisonType];
		CValuePercent val = CValuePercent.op_Implicit((int)poisonItem.ProducePercent);
		int num = combatChar.GetPoison()[poisonType];
		sbyte level = PoisonsAndLevels.CalcPoisonedLevel(num);
		int addValue = DomainManager.SpecialEffect.ModifyValue(combatChar.GetId(), 259, num * multiplier * val);
		AddPoison(context, null, combatChar, poisonItem.ProduceType, level, addValue, -1, applySpecialEffect: false);
	}

	public void PoisonProduceWeaken(DataContext context, CombatCharacter combatChar, sbyte poisonType)
	{
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		PoisonItem poisonItem = Poison.Instance[poisonType];
		int num = combatChar.GetPoison()[poisonType];
		sbyte b = PoisonsAndLevels.CalcPoisonedLevel(num);
		int num2 = combatChar.GetOldPoison()[poisonType];
		CValuePercent val = CValuePercent.op_Implicit((int)poisonItem.AffectCostPercent[Math.Clamp(b - 1, 0, poisonItem.AffectCostPercent.Length)]);
		int reduceValue = (num - num2) * val;
		ReducePoison(context, combatChar, poisonType, reduceValue, applySpecialEffect: false);
	}

	public unsafe void ChangeToOldPoison(DataContext context, CombatCharacter character, sbyte poisonType, int poisonValue)
	{
		PoisonInts oldPoison = character.GetOldPoison();
		PoisonInts poison = character.GetPoison();
		oldPoison.Items[poisonType] = Math.Min(oldPoison.Items[poisonType] + poisonValue, poison.Items[poisonType]);
		character.SetOldPoison(ref oldPoison, context);
	}

	public bool CheckSkillPoison(short skillId, sbyte poisonType)
	{
		return Config.CombatSkill.Instance[skillId].Poisons.GetValueAndLevel(poisonType).value > 0;
	}

	public bool CheckEquipmentPoison(ItemKey itemKey, out PoisonsAndLevels attachedPoisons)
	{
		PoisonsAndLevels innatePoisons;
		bool result = CheckEquipmentPoison(itemKey, out attachedPoisons, out innatePoisons);
		if (!attachedPoisons.IsNonZero())
		{
			attachedPoisons = innatePoisons;
		}
		return result;
	}

	public bool CheckEquipmentPoison(ItemKey itemKey, out PoisonsAndLevels attachedPoisons, out PoisonsAndLevels innatePoisons)
	{
		ItemBase itemBase = (itemKey.IsValid() ? DomainManager.Item.GetBaseItem(itemKey) : null);
		if (itemBase == null || itemBase.GetCurrDurability() <= 0)
		{
			return false;
		}
		if (ModificationStateHelper.IsActive(itemKey.ModificationState, 1))
		{
			attachedPoisons = DomainManager.Item.GetAttachedPoisons(itemKey);
		}
		if (itemBase is GameData.Domains.Item.Weapon weapon)
		{
			innatePoisons = weapon.GetInnatePoisons();
		}
		return attachedPoisons.IsNonZero() || innatePoisons.IsNonZero();
	}

	public void ApplyEquipmentPoison(DataContext context, CombatCharacter poisoner, CombatCharacter victim, ItemKey itemKey, int valueMultiplier = 1)
	{
		if (victim.GetId() == _carrierAnimalCombatCharId || !CheckEquipmentPoison(itemKey, out var attachedPoisons, out var innatePoisons))
		{
			return;
		}
		List<sbyte> list = ObjectPool<List<sbyte>>.Instance.Get();
		List<int> list2 = ObjectPool<List<int>>.Instance.Get();
		list.Clear();
		list2.Clear();
		for (sbyte b = 0; b < 6; b++)
		{
			int num = attachedPoisons.GetValueAndLevel(b).value + innatePoisons.GetValueAndLevel(b).value;
			if (num > 0)
			{
				list.Add(b);
				list2.Add(num);
			}
		}
		sbyte poisonType = list[RandomUtils.GetRandomIndex(list2, context.Random)];
		ObjectPool<List<sbyte>>.Instance.Return(list);
		ObjectPool<List<int>>.Instance.Return(list2);
		AddPoisonByTuple(attachedPoisons.GetValueAndLevel(poisonType));
		AddPoisonByTuple(innatePoisons.GetValueAndLevel(poisonType));
		void AddPoisonByTuple((short value, sbyte level) tuple)
		{
			if (tuple.value > 0)
			{
				AddPoison(context, poisoner, victim, poisonType, tuple.level, tuple.value * valueMultiplier, -1, applySpecialEffect: true, canBounce: true, itemKey, isDirectPoison: true);
			}
		}
	}

	private static void BindMixPoisonEffectImplements()
	{
		if (MixPoisonEffectImplements.Count > 0)
		{
			return;
		}
		Type typeFromHandle = typeof(MixPoisonEffectImplements);
		Type typeFromHandle2 = typeof(MixPoisonEffectAttribute);
		MethodInfo[] methods = typeFromHandle.GetMethods();
		foreach (MethodInfo methodInfo in methods)
		{
			object[] customAttributes = methodInfo.GetCustomAttributes(typeFromHandle2, inherit: false);
			if (customAttributes.Length != 0)
			{
				MixPoisonEffectAttribute mixPoisonEffectAttribute = (MixPoisonEffectAttribute)customAttributes[0];
				MixPoisonEffectDelegate value = methodInfo.CreateDelegate<MixPoisonEffectDelegate>();
				MixPoisonEffectImplements.Add(mixPoisonEffectAttribute.TemplateId, value);
			}
		}
	}

	private void CalcMixPoisonEffects(DataContext context, CombatCharacter combatChar, sbyte affectPoisonType)
	{
		byte[] poisonMarkList = combatChar.GetDefeatMarkCollection().PoisonMarkList;
		if (poisonMarkList.CountAll((byte count) => count > 0) < 3)
		{
			return;
		}
		List<sbyte> typeList = ObjectPool<List<sbyte>>.Instance.Get();
		typeList.Clear();
		for (sbyte b = 0; b < 6; b++)
		{
			if (poisonMarkList[b] > 0)
			{
				typeList.Add(b);
			}
		}
		foreach (MixPoisonEffectItem item in (IEnumerable<MixPoisonEffectItem>)MixPoisonEffect.Instance)
		{
			if (!item.AffectPoisonTypes.Exist(affectPoisonType) || item.HasPoisonTypes.Exist((sbyte type) => !typeList.Contains(type)))
			{
				continue;
			}
			if (!MixPoisonEffectImplements.TryGetValue(item.TemplateId, out var value))
			{
				PredefinedLog.Show(8, $"Not implement mixed poison type {item.TemplateId}");
				continue;
			}
			bool flag = DomainManager.SpecialEffect.ModifyData(combatChar.GetId(), -1, 272, dataValue: false);
			int poisonMarkCount = item.HasPoisonTypes.Sum((sbyte x) => poisonMarkList[x]);
			int num = CFormula.CalcMixPoisonAffectCount(poisonMarkCount);
			int affectedCount = combatChar.GetMixPoisonAffectedCount().GetAffectedCount(item.TemplateId);
			if ((flag || affectedCount < num) && value(context, combatChar, poisonMarkList))
			{
				combatChar.SetMixPoisonAffectedCount(combatChar.GetMixPoisonAffectedCount().AddAffectedCount(item.TemplateId), context);
			}
		}
		ObjectPool<List<sbyte>>.Instance.Return(typeList);
	}

	[DomainMethod]
	public bool DoRawCreate(DataContext context, int effectId, sbyte equipmentSlot, short newTemplateId)
	{
		if (!_selfChar.IsAlly)
		{
			return false;
		}
		return _selfChar.DoRawCreate(context, effectId, equipmentSlot, newTemplateId);
	}

	[DomainMethod]
	public bool IgnoreRawCreate(DataContext context, int effectId)
	{
		if (!_selfChar.IsAlly)
		{
			return false;
		}
		_selfChar.IgnoreRawCreate(context, effectId);
		return true;
	}

	[DomainMethod]
	public bool IgnoreAllRawCreate(DataContext context)
	{
		if (!_selfChar.IsAlly)
		{
			return false;
		}
		_selfChar.IgnoreAllRawCreate(context);
		return true;
	}

	[DomainMethod]
	public List<sbyte> GetAllCanRawCreateEquipmentSlots(int effectId)
	{
		SpecialEffectItem specialEffectItem = Config.SpecialEffect.Instance[effectId];
		return new List<sbyte>(_selfChar.GetAllCanRawCreateEquipmentSlots(specialEffectItem.RawCreateType));
	}

	[DomainMethod]
	public UnlockSimulateResult GetUnlockSimulateResult(int index, bool isAlly = true)
	{
		if (!IsInCombat())
		{
			return null;
		}
		CombatCharacter combatCharacter = (isAlly ? _selfChar : _enemyChar);
		List<bool> canUnlockAttack = combatCharacter.GetCanUnlockAttack();
		if (!canUnlockAttack.CheckIndex(index) || !canUnlockAttack[index])
		{
			return null;
		}
		List<int> list = ObjectPool<List<int>>.Instance.Get();
		list.Clear();
		list = DomainManager.SpecialEffect.ModifyData(combatCharacter.GetId(), -1, 312, list, index);
		UnlockSimulateResult result = new UnlockSimulateResult(list, combatCharacter.AllRawCreateSlotsBlocked);
		ObjectPool<List<int>>.Instance.Return(list);
		return result;
	}

	[DomainMethod]
	public List<CombatSkillDisplayData> GetProactiveSkillList(int charId)
	{
		if (!IsCharInCombat(charId))
		{
			return null;
		}
		CombatCharacter combatCharacter = _combatCharacterDict[charId];
		List<short> list = ObjectPool<List<short>>.Instance.Get();
		list.Clear();
		list.AddRange(combatCharacter.GetAttackSkillList());
		list.AddRange(combatCharacter.GetAgileSkillList());
		list.AddRange(combatCharacter.GetDefenceSkillList());
		list.RemoveAll((short id) => id < 0);
		List<CombatSkillDisplayData> combatSkillDisplayData = DomainManager.CombatSkill.GetCombatSkillDisplayData(charId, list);
		ObjectPool<List<short>>.Instance.Return(list);
		return combatSkillDisplayData;
	}

	[DomainMethod]
	public OuterAndInnerShorts GetPreviewAttackRange(int charId, short skillId, int weaponIndex = -1)
	{
		if (!IsCharInCombat(charId) || !TryGetElement_CombatCharacterDict(charId, out var element))
		{
			return default(OuterAndInnerShorts);
		}
		return element.CalcAttackRangeImmediate(skillId, weaponIndex);
	}

	[DomainMethod]
	public void StartPrepareSkill(DataContext context, short skillId, bool isAlly = true)
	{
		if (!CanAcceptCommand())
		{
			return;
		}
		CombatCharacter combatCharacter = (isAlly ? _selfChar : _enemyChar);
		if (_skillDataDict.TryGetValue((charId: combatCharacter.GetId(), skillId: skillId), out var value) && value.GetCanUse())
		{
			combatCharacter.SetNeedUseSkillId(context, skillId);
			bool flag = Config.CombatSkill.Instance[combatCharacter.NeedUseSkillId].EquipType == 1 || combatCharacter.GetPreparingSkillId() < 0 || !combatCharacter.CanCastDuringPrepareSkills.Contains(skillId);
			if (!flag)
			{
				CastAgileOrDefenseWithoutPrepare(combatCharacter, combatCharacter.NeedUseSkillId);
				combatCharacter.SetNeedUseSkillId(context, -1);
			}
			if (flag)
			{
				combatCharacter.MoveData.ResetJumpState(context);
			}
			UpdateMaxSkillGrade(isAlly, skillId);
			UpdateAllCommandAvailability(context, combatCharacter);
		}
	}

	[DomainMethod]
	public bool ClearDefendInBlockAttackSkill(DataContext context, bool isAlly = true)
	{
		CombatCharacter combatCharacter = (isAlly ? _selfChar : _enemyChar);
		if (combatCharacter.NeedUseSkillId < 0 || combatCharacter.GetAffectingDefendSkillId() < 0)
		{
			return false;
		}
		if (DomainManager.SpecialEffect.ModifyData(combatCharacter.GetId(), combatCharacter.NeedUseSkillId, 223, dataValue: false))
		{
			return false;
		}
		ClearAffectingDefenseSkillManual(context, isAlly);
		return true;
	}

	private void InitSkillData(DataContext context)
	{
		ClearSkillDataDict();
		foreach (CombatCharacter value in _combatCharacterDict.Values)
		{
			GameData.Domains.Character.Character character = value.GetCharacter();
			int id = character.GetId();
			foreach (short item in value.GetCharacter().GetCombatSkillEquipment())
			{
				if ((value.BossConfig == null || !CombatSkillTemplateHelper.IsAttack(item)) && character.GetCombatSkillCanAffect(item))
				{
					AddCombatSkillData(context, id, item);
				}
			}
			if (value.BossConfig != null)
			{
				short[] array = value.BossConfig.PhaseAttackSkills[0];
				foreach (short skillId in array)
				{
					AddCombatSkillData(context, id, skillId);
				}
			}
		}
	}

	private void AddCombatSkillData(DataContext context, int charId, short skillId)
	{
		CombatSkillKey combatSkillKey = new CombatSkillKey(charId, skillId);
		if (_skillDataDict.ContainsKey(combatSkillKey))
		{
			PredefinedLog.Show(8, "AddCombatSkillData already exist key " + combatSkillKey.ToString());
			return;
		}
		CombatSkillData combatSkillData = new CombatSkillData(combatSkillKey);
		AddElement_SkillDataDict(combatSkillKey, combatSkillData);
		combatSkillData.SetLeftCdFrame(0, context);
	}

	public CombatSkillData GetCombatSkillData(int charId, short skillId)
	{
		return _skillDataDict[new CombatSkillKey(charId, skillId)];
	}

	public bool TryGetCombatSkillData(int charId, short skillId, out CombatSkillData combatSkillData)
	{
		return _skillDataDict.TryGetValue((charId: charId, skillId: skillId), out combatSkillData);
	}

	public bool IsCombatSkillSilenceInfinity(CombatSkillKey skillKey)
	{
		CombatSkillData combatSkillData;
		return TryGetCombatSkillData(skillKey.CharId, skillKey.SkillTemplateId, out combatSkillData) && combatSkillData.GetTotalCdFrame() < 0;
	}

	public bool CombatSkillDataExist(CombatSkillKey skillKey)
	{
		return _skillDataDict.ContainsKey(skillKey);
	}

	public void UpdateSkillCanUse(DataContext context, CombatCharacter character)
	{
		List<short> list = ObjectPool<List<short>>.Instance.Get();
		list.Clear();
		list.AddRange(character.GetAttackSkillList());
		list.AddRange(character.GetAgileSkillList());
		list.AddRange(character.GetDefenceSkillList());
		list.AddRange(character.GetAssistSkillList());
		list.RemoveAll((short id) => id <= 0);
		for (int num = 0; num < list.Count; num++)
		{
			UpdateSkillCanUse(context, character, list[num]);
		}
		ObjectPool<List<short>>.Instance.Return(list);
	}

	public void UpdateSkillCanUse(DataContext context, CombatCharacter character, short skillId)
	{
		CombatSkillKey key = new CombatSkillKey(character.GetId(), skillId);
		CombatSkillData combatSkillData = _skillDataDict[key];
		CombatSkillItem combatSkillItem = Config.CombatSkill.Instance[skillId];
		bool flag = ((combatSkillItem.EquipType == 4) ? (combatSkillData.GetLeftCdFrame() == 0) : CanCastSkill(character, skillId));
		if (combatSkillData.GetCanUse() != flag)
		{
			combatSkillData.SetCanUse(flag, context);
		}
		else if (!flag)
		{
			combatSkillData.InvalidateSelfAndInfluencedCache(7, context);
		}
		if (!flag && character.GetCombatReserveData().NeedUseSkillId == skillId)
		{
			character.SetCombatReserveData(CombatReserveData.Invalid, context);
		}
	}

	public void UpdateSkillCostBreathStanceCanUse(DataContext context, CombatCharacter character)
	{
		foreach (CombatSkillKey key in _skillDataDict.Keys)
		{
			if (key.CharId == character.GetId() && DomainManager.CombatSkill.GetElement_CombatSkills(key).GetCostBreathAndStancePercent() > 0)
			{
				UpdateSkillCanUse(context, character, key.SkillTemplateId);
			}
		}
	}

	public void UpdateSkillCostTrickCanUse(DataContext context, CombatCharacter character)
	{
		foreach (CombatSkillKey key in _skillDataDict.Keys)
		{
			if (key.CharId == character.GetId() && Config.CombatSkill.Instance[key.SkillTemplateId].TrickCost.Count > 0)
			{
				UpdateSkillCanUse(context, character, key.SkillTemplateId);
			}
		}
		CombatCharacter combatCharacter = GetCombatCharacter(!character.IsAlly);
		foreach (CombatSkillKey key2 in _skillDataDict.Keys)
		{
			if (key2.CharId == combatCharacter.GetId() && DomainManager.SpecialEffect.ModifyData(key2.CharId, key2.SkillTemplateId, 280, dataValue: false))
			{
				UpdateSkillCanUse(context, combatCharacter, key2.SkillTemplateId);
			}
		}
	}

	private void UpdateSkillNeedMobilityCanUse(DataContext context, CombatCharacter character)
	{
		foreach (CombatSkillKey key in _skillDataDict.Keys)
		{
			GameData.Domains.CombatSkill.CombatSkill element_CombatSkills = DomainManager.CombatSkill.GetElement_CombatSkills(key);
			CombatSkillItem combatSkillItem = Config.CombatSkill.Instance[key.SkillTemplateId];
			bool flag = DomainManager.SpecialEffect.ModifyData(character.GetId(), key.SkillTemplateId, 229, dataValue: false);
			bool flag2 = DomainManager.SpecialEffect.ModifyData(character.GetId(), key.SkillTemplateId, 230, dataValue: false);
			if (key.CharId == character.GetId() && (combatSkillItem.EquipType == 2 || element_CombatSkills.GetCostMobilityPercent() > 0 || flag || flag2))
			{
				UpdateSkillCanUse(context, character, key.SkillTemplateId);
			}
		}
	}

	private void UpdateSkillNeedDistanceCanUse(DataContext context, CombatCharacter character)
	{
		foreach (CombatSkillKey key in _skillDataDict.Keys)
		{
			if (key.CharId == character.GetId() && Config.CombatSkill.Instance[key.SkillTemplateId].EquipType == 1)
			{
				UpdateSkillCanUse(context, character, key.SkillTemplateId);
			}
		}
	}

	public void UpdateSkillNeedBodyPartCanUse(DataContext context, CombatCharacter character)
	{
		foreach (CombatSkillKey key in _skillDataDict.Keys)
		{
			if (key.CharId == character.GetId() && Config.CombatSkill.Instance[key.SkillTemplateId].NeedBodyPartTypes.Count > 0)
			{
				UpdateSkillCanUse(context, character, key.SkillTemplateId);
			}
		}
	}

	public OuterAndInnerInts GetSkillCostBreathStance(int charId, GameData.Domains.CombatSkill.CombatSkill skill)
	{
		int costBreathPercent = skill.GetCostBreathPercent();
		int costStancePercent = skill.GetCostStancePercent();
		int num = DomainManager.SpecialEffect.ModifyValue(charId, skill.GetId().SkillTemplateId, 302, 0);
		if (1 == 0)
		{
		}
		(int, int) tuple = ((num > 0) ? (costBreathPercent + costStancePercent, 0) : ((num >= 0) ? (costBreathPercent, costStancePercent) : (0, costBreathPercent + costStancePercent)));
		if (1 == 0)
		{
		}
		(int, int) tuple2 = tuple;
		costBreathPercent = tuple2.Item1;
		costStancePercent = tuple2.Item2;
		costBreathPercent = DomainManager.SpecialEffect.ModifyData(charId, skill.GetId().SkillTemplateId, 227, costBreathPercent);
		costStancePercent = DomainManager.SpecialEffect.ModifyData(charId, skill.GetId().SkillTemplateId, 228, costStancePercent);
		return new OuterAndInnerInts(costStancePercent, costBreathPercent);
	}

	public bool SkillCostEnough(CombatCharacter character, short skillId)
	{
		if (_enableSkillFreeCast)
		{
			return true;
		}
		if (!DomainManager.SpecialEffect.ModifyData(character.GetId(), skillId, 154, dataValue: true))
		{
			return true;
		}
		foreach (ECombatSkillBanReasonType item in CalcSkillCostEnoughBanReasons(character, skillId))
		{
			if (item != ECombatSkillBanReasonType.None)
			{
				return false;
			}
		}
		return true;
	}

	public IEnumerable<ECombatSkillBanReasonType> CalcSkillCostEnoughBanReasons(CombatCharacter character, short skillId)
	{
		CombatSkillItem configData = Config.CombatSkill.Instance[skillId];
		CombatSkillKey skillKey = new CombatSkillKey(character.GetId(), skillId);
		GameData.Domains.CombatSkill.CombatSkill skill = DomainManager.CombatSkill.GetElement_CombatSkills(skillKey);
		if (!DomainManager.SpecialEffect.ModifyData(character.GetId(), skillId, 290, dataValue: true))
		{
			yield return ECombatSkillBanReasonType.SpecialEffectBan;
		}
		if ((!character.CanCastSkillCostBreath && skill.GetCostBreathPercent() > 0) || (!character.CanCastSkillCostStance && skill.GetCostStancePercent() > 0))
		{
			yield return ECombatSkillBanReasonType.SpecialEffectBan;
		}
		else
		{
			int mobilityPercent = CValuePercent.ParseInt(character.GetMobilityValue(), MoveSpecialConstants.MaxMobility);
			OuterAndInnerInts costBreathStance = GetSkillCostBreathStance(character.GetId(), skill);
			CValuePercent innerRatio = CValuePercent.op_Implicit((int)skill.GetCurrInnerRatio());
			int breathExtra = DomainManager.SpecialEffect.GetModifyValue(character.GetId(), skillId, 173, (EDataModifyType)0, -1, -1, -1, (EDataSumType)0);
			int breathCostPercent = Math.Max(costBreathStance.Inner - breathExtra, 0);
			bool breathCanUseMobility = DomainManager.SpecialEffect.ModifyData(character.GetId(), skillKey.SkillTemplateId, 229, dataValue: false);
			int breathUseMobility = 0;
			if (breathCanUseMobility && breathCostPercent > 0)
			{
				breathUseMobility = Math.Min(breathCostPercent, mobilityPercent * 2 * innerRatio);
			}
			breathCostPercent -= breathUseMobility;
			int breathCost = 30000 * breathCostPercent / 100;
			bool breathEnough = character.GetBreathValue() >= breathCost;
			bool breathCanCastOnLack = DomainManager.SpecialEffect.ModifyData(character.GetId(), skillId, 225, dataValue: false, breathCost);
			if (!breathEnough && !breathCanCastOnLack)
			{
				yield return ECombatSkillBanReasonType.BreathNotEnough;
			}
			int stanceExtra = DomainManager.SpecialEffect.GetModifyValue(character.GetId(), skillId, 174, (EDataModifyType)0, -1, -1, -1, (EDataSumType)0);
			int stanceCostPercent = Math.Max(costBreathStance.Outer - stanceExtra, 0);
			bool stanceCanUseMobility = DomainManager.SpecialEffect.ModifyData(character.GetId(), skillKey.SkillTemplateId, 230, dataValue: false);
			int stanceUseMobility = 0;
			if (stanceCanUseMobility && stanceCostPercent > 0)
			{
				stanceUseMobility = Math.Min(stanceCostPercent, mobilityPercent * 2 - breathUseMobility);
			}
			stanceCostPercent -= stanceUseMobility;
			int stanceCost = 4000 * stanceCostPercent / 100;
			bool stanceEnough = character.GetStanceValue() >= stanceCost;
			bool stanceCanCastOnLack = DomainManager.SpecialEffect.ModifyData(character.GetId(), skillId, 226, dataValue: false, stanceCost);
			if (!stanceEnough && !stanceCanCastOnLack)
			{
				yield return ECombatSkillBanReasonType.StanceNotEnough;
			}
		}
		if (!HasNeedTrick(character, skill))
		{
			yield return ECombatSkillBanReasonType.TrickNotEnough;
		}
		if (configData.WeaponDurableCost > 0 && character.GetUsingWeaponIndex() < 3 && DomainManager.Item.GetElement_Weapons(GetUsingWeaponKey(character).Id).GetCurrDurability() < configData.WeaponDurableCost)
		{
			yield return ECombatSkillBanReasonType.WeaponDestroyed;
		}
		CValuePercent costMobilityPercent = CValuePercent.op_Implicit((int)skill.GetCostMobilityPercent());
		if (costMobilityPercent > 0)
		{
			int costMobility = MoveSpecialConstants.MaxMobility * costMobilityPercent;
			if (character.GetMobilityValue() < costMobility)
			{
				yield return ECombatSkillBanReasonType.MobilityNotEnough;
			}
		}
		if (character.GetWugCount() < configData.WugCost)
		{
			yield return ECombatSkillBanReasonType.WugNotEnough;
		}
		if (!HasNeedNeiliAllocation(character, skill))
		{
			yield return ECombatSkillBanReasonType.NeiliAllocationNotEnough;
		}
	}

	public bool SkillInCastRange(CombatCharacter character, short skillId)
	{
		OuterAndInnerInts skillAttackRange = GetSkillAttackRange(character, skillId);
		return _currentDistance >= skillAttackRange.Outer && _currentDistance <= skillAttackRange.Inner;
	}

	public OuterAndInnerInts GetSkillAttackRange(CombatCharacter character, short skillId)
	{
		OuterAndInnerShorts outerAndInnerShorts = character.CalcAttackRangeImmediate(skillId);
		return new OuterAndInnerInts(outerAndInnerShorts.Outer, outerAndInnerShorts.Inner);
	}

	public bool HasSkillNeedBodyPart(CombatCharacter character, short skillId, bool applyEffect = true)
	{
		CombatSkillItem combatSkillItem = Config.CombatSkill.Instance[skillId];
		bool flag = applyEffect && DomainManager.SpecialEffect.ModifyData(character.GetId(), skillId, 219, dataValue: false);
		byte[] acupointCount = character.GetAcupointCount();
		for (int i = 0; i < combatSkillItem.NeedBodyPartTypes.Count; i++)
		{
			switch (combatSkillItem.NeedBodyPartTypes[i])
			{
			case 0:
				if ((!flag && CheckBodyPartInjury(character, 2)) || acupointCount[2] >= 3)
				{
					return false;
				}
				break;
			case 1:
				if ((!flag && CheckBodyPartInjury(character, 0)) || acupointCount[0] >= 3)
				{
					return false;
				}
				break;
			case 2:
				if ((!flag && CheckBodyPartInjury(character, 1)) || acupointCount[1] >= 3)
				{
					return false;
				}
				break;
			case 3:
				if ((!flag && CheckBodyPartInjury(character, 3)) || acupointCount[3] >= 3 || (!flag && CheckBodyPartInjury(character, 4)) || acupointCount[4] >= 3)
				{
					return false;
				}
				break;
			case 4:
				if (((!flag && CheckBodyPartInjury(character, 3)) || acupointCount[3] >= 3) && ((!flag && CheckBodyPartInjury(character, 4)) || acupointCount[4] >= 3))
				{
					return false;
				}
				break;
			case 5:
				if ((!flag && CheckBodyPartInjury(character, 5)) || acupointCount[5] >= 3 || (!flag && CheckBodyPartInjury(character, 6)) || acupointCount[6] >= 3)
				{
					return false;
				}
				break;
			case 6:
				if (((!flag && CheckBodyPartInjury(character, 5)) || acupointCount[5] >= 3) && ((!flag && CheckBodyPartInjury(character, 6)) || acupointCount[6] >= 3))
				{
					return false;
				}
				break;
			}
		}
		return true;
	}

	public bool SkillBodyPartHasHeavyInjury(CombatCharacter character, short skillId)
	{
		CombatSkillItem combatSkillItem = Config.CombatSkill.Instance[skillId];
		for (int i = 0; i < combatSkillItem.NeedBodyPartTypes.Count; i++)
		{
			switch (combatSkillItem.NeedBodyPartTypes[i])
			{
			case 0:
				if (CheckBodyPartInjury(character, 2, checkHeavyInjury: true))
				{
					return true;
				}
				break;
			case 1:
				if (CheckBodyPartInjury(character, 0, checkHeavyInjury: true))
				{
					return true;
				}
				break;
			case 2:
				if (CheckBodyPartInjury(character, 1, checkHeavyInjury: true))
				{
					return true;
				}
				break;
			case 3:
				if (CheckBodyPartInjury(character, 3, checkHeavyInjury: true) || CheckBodyPartInjury(character, 4, checkHeavyInjury: true))
				{
					return true;
				}
				break;
			case 4:
				if (CheckBodyPartInjury(character, 3, checkHeavyInjury: true) && CheckBodyPartInjury(character, 4, checkHeavyInjury: true))
				{
					return true;
				}
				break;
			case 5:
				if (CheckBodyPartInjury(character, 5, checkHeavyInjury: true) || CheckBodyPartInjury(character, 6, checkHeavyInjury: true))
				{
					return true;
				}
				break;
			case 6:
				if (CheckBodyPartInjury(character, 5, checkHeavyInjury: true) && CheckBodyPartInjury(character, 6, checkHeavyInjury: true))
				{
					return true;
				}
				break;
			}
		}
		return false;
	}

	public bool SkillCanUseInCurrCombat(int charId, CombatSkillItem configData)
	{
		List<sbyte> combatSkillType = CombatConfig.CombatSkillType;
		return CombatSkillDomain.FiveElementMatch(charId, configData, CombatConfig.FiveElementsOfSkill) && (combatSkillType == null || combatSkillType.Count == 0 || combatSkillType.Contains(DomainManager.CombatSkill.GetSkillType(charId, configData.TemplateId))) && (CombatConfig.Sect < 0 || CombatConfig.Sect == configData.SectId);
	}

	public bool SkillDirectionCanCast(CombatCharacter character, short skillId)
	{
		sbyte skillDirection = DomainManager.CombatSkill.GetSkillDirection(character.GetId(), skillId);
		return (skillDirection != 0 || character.CanCastDirectSkill) && (skillDirection != 1 || character.CanCastReverseSkill);
	}

	public unsafe bool HasNeedNeiliAllocation(CombatCharacter character, GameData.Domains.CombatSkill.CombatSkill skill)
	{
		(sbyte, sbyte) costNeiliAllocation = skill.GetCostNeiliAllocation();
		NeiliAllocation neiliAllocation = character.GetNeiliAllocation();
		return costNeiliAllocation.Item1 < 0 || neiliAllocation.Items[costNeiliAllocation.Item1] >= costNeiliAllocation.Item2;
	}

	public void DoCombatSkillCost(DataContext context, CombatCharacter character, short skillId)
	{
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0107: Unknown result type (might be due to invalid IL or missing references)
		//IL_021c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0221: Unknown result type (might be due to invalid IL or missing references)
		//IL_0223: Unknown result type (might be due to invalid IL or missing references)
		//IL_023a: Unknown result type (might be due to invalid IL or missing references)
		if (_enableSkillFreeCast || !DomainManager.SpecialEffect.ModifyData(character.GetId(), skillId, 154, dataValue: true))
		{
			return;
		}
		CombatSkillItem combatSkillItem = Config.CombatSkill.Instance[skillId];
		CombatSkillKey objectId = new CombatSkillKey(character.GetId(), skillId);
		GameData.Domains.CombatSkill.CombatSkill element_CombatSkills = DomainManager.CombatSkill.GetElement_CombatSkills(objectId);
		int num = CValuePercent.ParseInt(character.GetMobilityValue(), MoveSpecialConstants.MaxMobility);
		OuterAndInnerInts skillCostBreathStance = GetSkillCostBreathStance(character.GetId(), element_CombatSkills);
		CValuePercent val = CValuePercent.op_Implicit((int)element_CombatSkills.GetCurrInnerRatio());
		int modifyValue = DomainManager.SpecialEffect.GetModifyValue(character.GetId(), skillId, 173, (EDataModifyType)0, -1, -1, -1, (EDataSumType)0);
		int num2 = Math.Max(skillCostBreathStance.Inner - modifyValue, 0);
		int extraBreath = skillCostBreathStance.Inner - num2;
		bool flag = DomainManager.SpecialEffect.ModifyData(character.GetId(), objectId.SkillTemplateId, 229, dataValue: false);
		int num3 = 0;
		if (flag && num2 > 0)
		{
			num3 = Math.Min(num2, num * 2 * val);
		}
		num2 -= num3;
		int num4 = 30000 * num2 / 100;
		int modifyValue2 = DomainManager.SpecialEffect.GetModifyValue(character.GetId(), skillId, 174, (EDataModifyType)0, -1, -1, -1, (EDataSumType)0);
		int num5 = Math.Max(skillCostBreathStance.Outer - modifyValue2, 0);
		int extraStance = skillCostBreathStance.Outer - num5;
		bool flag2 = DomainManager.SpecialEffect.ModifyData(character.GetId(), objectId.SkillTemplateId, 230, dataValue: false);
		int num6 = 0;
		if (flag2 && num5 > 0)
		{
			num6 = Math.Min(num5, num * 2 - num3);
		}
		num5 -= num6;
		int num7 = 4000 * num5 / 100;
		Events.RaiseCastSkillUseExtraBreathOrStance(context, character.GetId(), skillId, extraBreath, extraStance);
		if (num3 > 0)
		{
			Events.RaiseCastSkillUseMobilityAsBreathOrStance(context, character.GetId(), skillId, asBreath: true);
		}
		if (num6 > 0)
		{
			Events.RaiseCastSkillUseMobilityAsBreathOrStance(context, character.GetId(), skillId, asBreath: false);
		}
		CValuePercent val2 = CValuePercent.op_Implicit((num3 + num6 > 0) ? Math.Max((num3 + num6) / 2, 1) : 0);
		if (val2 > 0)
		{
			ChangeMobilityValue(context, character, -MoveSpecialConstants.MaxMobility * val2, changedByEffect: false, null, costBySkill: true);
		}
		if (num4 > 0 || num7 > 0)
		{
			bool flag3 = DomainManager.SpecialEffect.ModifyData(character.GetId(), skillId, 225, dataValue: false, num4);
			bool flag4 = DomainManager.SpecialEffect.ModifyData(character.GetId(), skillId, 226, dataValue: false, num7);
			if (flag3 || flag4)
			{
				int breathValue = character.GetBreathValue();
				int stanceValue = character.GetStanceValue();
				if (breathValue < num4 || stanceValue < num7)
				{
					Events.RaiseCastSkillOnLackBreathStance(context, character, skillId, breathValue - num4, stanceValue - num7, num4, num7);
				}
			}
		}
		CostBreathAndStance(context, character, num4, num7, skillId);
		List<NeedTrick> list = ObjectPool<List<NeedTrick>>.Instance.Get();
		List<NeedTrick> list2 = ObjectPool<List<NeedTrick>>.Instance.Get();
		DomainManager.CombatSkill.GetCombatSkillCostTrick(element_CombatSkills, list);
		RemoveUsableTrickInsteadCostTrick(character, skillId, list, list2);
		RemoveCostTrickInsteadUselessTrick(character, skillId, list, trulyCost: true);
		RemoveCostTrickBySelfShaTrick(Context, character, element_CombatSkills.GetId().SkillTemplateId, list, trulyCost: true);
		RemoveCostTrickByEnemyShaTrick(Context, character, element_CombatSkills.GetId().SkillTemplateId, list, list2);
		RemoveCostTrickByJiTrick(Context, character, element_CombatSkills.GetId().SkillTemplateId, list, trulyCost: true);
		RemoveJiTrickByUselessTrick(Context, character, element_CombatSkills.GetId().SkillTemplateId, list, trulyCost: true);
		RemoveTrick(context, character, list, removedByAlly: true, skillCost: true);
		if (list2.Count > 0)
		{
			RemoveTrick(context, GetCombatCharacter(!character.IsAlly), list2, removedByAlly: false);
		}
		Events.RaiseCastSkillTrickCosted(context, character, skillId, list);
		ObjectPool<List<NeedTrick>>.Instance.Return(list);
		ObjectPool<List<NeedTrick>>.Instance.Return(list2);
		int costMobilityPercent = element_CombatSkills.GetCostMobilityPercent();
		if (costMobilityPercent > 0)
		{
			int num8 = MoveSpecialConstants.MaxMobility * costMobilityPercent / 100;
			ChangeMobilityValue(context, character, -num8, changedByEffect: false, null, costBySkill: true);
		}
		if (combatSkillItem.WugCost > 0)
		{
			character.ChangeWugCount(context, -combatSkillItem.WugCost);
		}
		(sbyte, sbyte) costNeiliAllocation = element_CombatSkills.GetCostNeiliAllocation();
		if (costNeiliAllocation.Item1 >= 0)
		{
			character.ChangeNeiliAllocation(context, (byte)costNeiliAllocation.Item1, -costNeiliAllocation.Item2);
		}
		Events.RaiseCastSkillCosted(context, character, skillId);
		UpdateAllCommandAvailability(context, character);
	}

	public int GetSkillPrepareSpeed(CombatCharacter character)
	{
		int skillPrepareSpeed = DomainManager.SpecialEffect.ModifyValue(character.GetId(), character.GetPreparingSkillId(), 194, character.GetSkillPrepareSpeed());
		return CFormula.CalcSkillPrepareSpeed(skillPrepareSpeed);
	}

	public void CalcSkillQiDisorderAndInjury(CombatCharacter character, CombatSkillItem skillConfig)
	{
		NeiliTypeItem neiliTypeItem = NeiliType.Instance[character.GetNeiliType()];
		if (CombatSkillDomain.FiveElementEquals(character.GetId(), skillConfig, neiliTypeItem.InjuryOnUseType))
		{
			AddGoneMadInjury(Context, character, skillConfig.TemplateId);
			ShowSpecialEffectTips(character.GetId(), 1462, 0);
		}
		if (Context.Random.CheckPercentProb(character.GetInjuredRate(skillConfig)))
		{
			AddGoneMadInjury(Context, character, skillConfig.TemplateId);
			ShowSpecialEffectTips(character.GetId(), 1487, 0);
		}
	}

	public void ApplyAgileOrDefenseSkill(CombatCharacter character, CombatSkillItem skillConfig)
	{
		GameData.Domains.CombatSkill.CombatSkill element_CombatSkills = DomainManager.CombatSkill.GetElement_CombatSkills(new CombatSkillKey(character.GetId(), skillConfig.TemplateId));
		if (skillConfig.EquipType == 2)
		{
			character.SetAffectingMoveSkillId(skillConfig.TemplateId, Context);
			character.MoveData.ResetJumpState(Context);
			character.NeedAddEffectAgileSkillId = skillConfig.TemplateId;
			UpdateTeammateCommandUsable(Context, character, ETeammateCommandImplement.ClearAgileAndDefense);
		}
		else if (skillConfig.EquipType == 3)
		{
			short defendSkillTotalFrame = CombatSkillDomain.CalcContinuousFrames(element_CombatSkills);
			character.SetAffectingDefendSkillId(skillConfig.TemplateId, Context);
			character.DefendSkillLeftFrame = (character.DefendSkillTotalFrame = defendSkillTotalFrame);
			DomainManager.SpecialEffect.Add(Context, character.GetId(), skillConfig.TemplateId, 0, -1);
			UpdateTeammateCommandUsable(Context, character, ETeammateCommandImplement.ClearAgileAndDefense);
		}
		if (character.GetCharacter().IsCombatSkillEquipped(skillConfig.TemplateId))
		{
			CombatSkillKey key = new CombatSkillKey(character.GetId(), skillConfig.TemplateId);
			_skillCastTimes[key] = _skillCastTimes.GetOrDefault(key) + 1;
		}
	}

	public void CastAgileOrDefenseWithoutPrepare(CombatCharacter character, short skillId)
	{
		Events.RaiseCastAgileOrDefenseWithoutPrepareBegin(Context, character.GetId(), skillId);
		CombatSkillItem skillConfig = Config.CombatSkill.Instance[skillId];
		CalcSkillQiDisorderAndInjury(character, skillConfig);
		ApplyAgileOrDefenseSkill(character, skillConfig);
		AddToCheckFallenSet(character.GetId());
		Events.RaiseCastAgileOrDefenseWithoutPrepareEnd(Context, character.GetId(), skillId);
	}

	public unsafe void CalcAttackSkillDataCompare(CombatContext context)
	{
		CombatCharacter attacker = context.Attacker;
		CombatCharacter defender = context.Defender;
		short skillTemplateId = context.SkillTemplateId;
		GameData.Domains.CombatSkill.CombatSkill element_CombatSkills = DomainManager.CombatSkill.GetElement_CombatSkills(new CombatSkillKey(attacker.GetId(), skillTemplateId));
		HitOrAvoidInts hitValue = element_CombatSkills.GetHitValue();
		if (!CombatSkillTemplateHelper.IsMindHitSkill(skillTemplateId))
		{
			HitOrAvoidInts hitDistribution = element_CombatSkills.GetHitDistribution();
			attacker.SkillHitType[0] = 2;
			attacker.SkillHitType[1] = 1;
			attacker.SkillHitType[2] = 0;
			attacker.SkillHitValue[0] = ((hitDistribution.Items[2] > 0) ? attacker.GetHitValue(2, attacker.SkillAttackBodyPart, hitValue.Items[2], skillTemplateId) : (-1));
			attacker.SkillAvoidValue[0] = ((hitDistribution.Items[2] > 0) ? defender.GetAvoidValue(2, attacker.SkillAttackBodyPart, skillTemplateId) : (-1));
			attacker.SkillHitValue[1] = ((hitDistribution.Items[1] > 0) ? attacker.GetHitValue(1, attacker.SkillAttackBodyPart, hitValue.Items[1], skillTemplateId) : (-1));
			attacker.SkillAvoidValue[1] = ((hitDistribution.Items[1] > 0) ? defender.GetAvoidValue(1, attacker.SkillAttackBodyPart, skillTemplateId) : (-1));
			attacker.SkillHitValue[2] = ((hitDistribution.Items[0] > 0) ? attacker.GetHitValue(0, attacker.SkillAttackBodyPart, hitValue.Items[0], skillTemplateId) : (-1));
			attacker.SkillAvoidValue[2] = ((hitDistribution.Items[0] > 0) ? defender.GetAvoidValue(0, attacker.SkillAttackBodyPart, skillTemplateId) : (-1));
			attacker.SkillFinalAttackHitIndex = 0;
			bool flag = CanHit(0);
			int num = hitDistribution.Items[2];
			int num2 = CalcHitOdds(0);
			for (int i = 1; i < 3; i++)
			{
				bool flag2 = CanHit(i);
				int num3 = hitDistribution.Items[2 - i];
				int num4 = CalcHitOdds(i);
				if ((!flag || flag2) && (num3 > num || (num3 == num && num4 > num2)))
				{
					attacker.SkillFinalAttackHitIndex = i;
					flag = flag2;
					num = num3;
					num2 = num4;
				}
			}
		}
		else
		{
			attacker.SkillHitType[0] = 3;
			attacker.SkillHitType[1] = (attacker.SkillHitType[2] = -1);
			attacker.SkillHitValue[0] = attacker.GetHitValue(3, attacker.SkillAttackBodyPart, hitValue.Items[3], skillTemplateId);
			attacker.SkillAvoidValue[0] = defender.GetAvoidValue(3, attacker.SkillAttackBodyPart, skillTemplateId);
			attacker.SkillFinalAttackHitIndex = 0;
		}
		attacker.SetAttackSkillAttackIndex(0, context);
		attacker.SetPerformingSkillId(skillTemplateId, context);
		UpdateDamageCompareData(context);
		int CalcHitOdds(int index)
		{
			return attacker.SkillHitValue[index] * 100 / Math.Max(attacker.SkillAvoidValue[index], 1);
		}
		bool CanHit(int index)
		{
			return attacker.SkillHitValue[index] >= 0 && attacker.SkillHitValue[index] >= attacker.SkillAvoidValue[index];
		}
	}

	public unsafe void CalcSkillAttack(CombatContext context, int attackIndex)
	{
		CombatCharacter attacker = context.Attacker;
		CombatCharacter defender = context.Defender;
		short performingSkillId = attacker.GetPerformingSkillId();
		GameData.Domains.Item.Weapon weapon = context.Weapon;
		GameData.Domains.CombatSkill.CombatSkill element_CombatSkills = DomainManager.CombatSkill.GetElement_CombatSkills(new CombatSkillKey(attacker.GetId(), performingSkillId));
		HitOrAvoidInts hitDistribution = element_CombatSkills.GetHitDistribution();
		bool flag = CombatSkillTemplateHelper.IsMindHitSkill(performingSkillId);
		int num = (flag ? ((attackIndex == 0) ? 100 : 0) : ((attackIndex < 3) ? hitDistribution.Items[2 - attackIndex] : (-1)));
		int num2 = ((attackIndex < 3) ? attackIndex : attacker.SkillFinalAttackHitIndex);
		sbyte b = _damageCompareData.HitType[num2];
		CombatProperty property = _damageCompareData.GetProperty(num2);
		bool critical = context.CheckCritical(b);
		context = context.Property(property).Critical(critical);
		int num3 = property.HitOdds;
		if (attackIndex < 3)
		{
			num3 = ApplyHitOddsSpecialEffect(attacker, defender, num3, attacker.SkillHitType[attackIndex], performingSkillId);
		}
		bool flag2 = DomainManager.SpecialEffect.ModifyData(attacker.GetId(), performingSkillId, 251, dataValue: false);
		bool flag3 = DomainManager.SpecialEffect.ModifyData(defender.GetId(), performingSkillId, 291, dataValue: false, critical ? 1 : 0, context.BodyPart, attacker.GetId());
		bool flag4 = !flag3;
		bool flag5 = flag4;
		if (flag5)
		{
			bool flag8;
			if (attackIndex < 3)
			{
				bool isValid = property.IsValid;
				bool flag6 = isValid;
				if (flag6)
				{
					bool flag7 = ((num3 < 0 || num3 >= 100) ? true : false);
					flag6 = flag7 || attacker.SkillForceHit || flag2;
				}
				flag8 = flag6;
			}
			else
			{
				flag8 = attacker.GetAttackSkillPower() > 0;
			}
			flag5 = flag8;
		}
		bool flag9 = flag5;
		SetSkillAttackedIndexAndHit(new IntPair(attackIndex, flag9 ? 1 : 0), context);
		if (property.IsValid)
		{
			Events.RaiseAttackSkillAttackBegin(context, attacker, defender, performingSkillId, attackIndex, flag9);
		}
		if (flag9)
		{
			bool flag10 = DomainManager.CombatSkill.GetSkillType(attacker.GetId(), performingSkillId) == 5;
			WeaponItem weaponData = Config.Weapon.Instance[weapon.GetTemplateId()];
			if (flag10 && attacker.LegSkillUseShoes())
			{
				ItemKey itemKey = attacker.Armors[5];
				GameData.Domains.Item.Armor armor = (itemKey.IsValid() ? DomainManager.Item.GetElement_Armors(itemKey.Id) : null);
				weaponData = ((armor == null || armor.GetCurrDurability() <= 0) ? Config.Weapon.Instance[(short)0] : Config.Weapon.Instance[Config.Armor.Instance[itemKey.TemplateId].RelatedWeapon]);
			}
			PlayHitSound(context, defender, weaponData);
			if (attackIndex < 3)
			{
				if (num > 0)
				{
					attacker.SetAttackSkillPower((byte)(attacker.GetAttackSkillPower() + num), context);
					if (CanPlayHitAnimation(defender))
					{
						defender.SetAnimationToPlayOnce(GetHittedAni(defender, (num > 30) ? ((num <= 60) ? 1 : 2) : 0), context);
					}
					if (!flag)
					{
						int num4 = num / 2;
						if (num4 > 0)
						{
							CalcSkillDamage(context, b, num4, out var _, out critical, num4);
						}
					}
					else
					{
						int num5 = num3 / 5;
						if (num5 > 0)
						{
							AddCombatState(context, defender, 2, 116, num5);
						}
					}
				}
			}
			else
			{
				int finalDamage2;
				OuterAndInnerInts outerAndInnerInts = CalcSkillHit(context, b, out finalDamage2, out critical);
				if (CanPlayHitAnimation(defender))
				{
					defender.SetAnimationToPlayOnce(GetHittedAni(defender, Math.Clamp(outerAndInnerInts.Outer + outerAndInnerInts.Inner - 1, 0, 2)), context);
				}
				if (!defender.GetNewPoisonsToShow().IsNonZero() && finalDamage2 <= 0)
				{
					defender.SetParticleToPlay("Particle_D_qidun", context);
				}
				if (defender.GetPreparingOtherAction() == 2 && _currentDistance <= InterruptFleeNeedDistance)
				{
					InterruptOtherAction(context, defender);
				}
			}
		}
		else if (attackIndex < 3 && property.IsValid)
		{
			if (CanPlayHitAnimation(defender))
			{
				defender.SetAnimationToPlayOnce(GetAvoidAni(defender, b), context);
			}
			defender.SetParticleToPlay((defender.IsAlly ? _selfAvoidParticle : _enemyAvoidParticle)[b], context);
			string[] array = _avoidSound[b];
			defender.SetHitSoundToPlay(array[context.Random.Next(array.Length)], context);
		}
		else if (attackIndex == 3 && CanPlayHitAnimation(defender))
		{
			defender.SetAnimationToPlayOnce((defender.AnimalConfig == null) ? "H_008" : AvoidAni[2], context);
		}
		if (flag9)
		{
			Events.RaiseAttackSkillAttackHit(context, attacker, defender, performingSkillId, attackIndex, critical);
		}
		if (attackIndex == 3 && (!IsInCombat() || _selectedMercyOption >= 0))
		{
			attacker.SetPerformingSkillId(-1, context);
			attacker.SetAttackSkillPower(0, context);
			ClearDamageCompareData(context);
			SetSkillAttackedIndexAndHit(new IntPair(-1, 0), context);
		}
		if (attackIndex == 3 && attacker.GetCharacter().IsCombatSkillEquipped(performingSkillId))
		{
			CombatSkillKey key = new CombatSkillKey(attacker.GetId(), performingSkillId);
			_skillCastTimes[key] = _skillCastTimes.GetOrDefault(key) + 1;
		}
		if (attackIndex == 3 && attacker.GetAttackSkillPower() >= 100 && attacker.GetId() == DomainManager.Taiwu.GetTaiwuCharId() && attacker.GetCharacter().GetConsummateLevel() <= defender.GetCharacter().GetConsummateLevel())
		{
			DomainManager.Taiwu.AddFullPowerCastTimes(context, performingSkillId);
		}
		if (property.IsValid)
		{
			Events.RaiseAttackSkillAttackEnd(context, b, flag9, attackIndex);
		}
	}

	private OuterAndInnerInts CalcSkillDamage(CombatContext context, sbyte hitType, int skillPower, out int finalDamage, out bool critical, int bouncePercent = 100)
	{
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		CombatCharacter attacker = context.Attacker;
		CombatCharacter defender = context.Defender;
		short skillTemplateId = context.SkillTemplateId;
		bool flag = CombatSkillTemplateHelper.IsMindHitSkill(skillTemplateId);
		GameData.Domains.CombatSkill.CombatSkill skill = context.Skill;
		OuterAndInnerInts result = CalcAndAddInjury(context, hitType, out finalDamage, out critical, skillPower);
		CValuePercent ratio = CValuePercent.op_Implicit((int)skill.GetPower()) * CValuePercent.op_Implicit(skillPower);
		PoisonsAndLevels poisons = skill.GetPoisons();
		if (poisons.IsNonZero())
		{
			AddDirectPoison(context, attacker, defender, poisons, ratio, skillTemplateId, context.WeaponKey);
		}
		if (flag)
		{
			return result;
		}
		context.ApplyWeaponAndArmorPoison(3 * skillPower / 100);
		AddBounceDamage(context, hitType, skillTemplateId, CValuePercent.op_Implicit(bouncePercent));
		return result;
	}

	private OuterAndInnerInts CalcSkillHit(CombatContext context, sbyte hitType, out int finalDamage, out bool critical)
	{
		CombatCharacter attacker = context.Attacker;
		CombatCharacter defender = context.Defender;
		sbyte bodyPart = context.BodyPart;
		short skillTemplateId = context.SkillTemplateId;
		byte attackSkillPower = attacker.GetAttackSkillPower();
		CombatSkillItem combatSkillItem = Config.CombatSkill.Instance[skillTemplateId];
		GameData.Domains.CombatSkill.CombatSkill element_CombatSkills = DomainManager.CombatSkill.GetElement_CombatSkills(new CombatSkillKey(attacker.GetId(), skillTemplateId));
		CalculateWeaponArmorBreak(context, combatSkillItem.EquipmentBreakOdds);
		sbyte level = (sbyte)(combatSkillItem.GridCost - 1);
		if (combatSkillItem.HasAtkAcupointEffect && attackSkillPower >= element_CombatSkills.GetSumMax2HitDistribution())
		{
			AddAcupoint(context, defender, level, element_CombatSkills.GetId(), bodyPart);
		}
		if (combatSkillItem.HasAtkFlawEffect && attackSkillPower >= element_CombatSkills.GetSumMax2HitDistribution())
		{
			AddFlaw(context, defender, level, element_CombatSkills.GetId(), bodyPart);
		}
		OuterAndInnerInts result = CalcSkillDamage(context, hitType, attackSkillPower, out finalDamage, out critical);
		if (attacker.NeedReduceWeaponDurability)
		{
			if (DomainManager.CombatSkill.GetSkillType(attacker.GetId(), skillTemplateId) != 5 || !attacker.LegSkillUseShoes())
			{
				ReduceDurabilityByHit(context, attacker, context.WeaponKey);
			}
			else
			{
				ItemKey key = attacker.Armors[5];
				if (key.IsValid())
				{
					ReduceDurabilityByHit(context, attacker, key);
				}
			}
			attacker.NeedReduceWeaponDurability = false;
		}
		if (defender.NeedReduceArmorDurability)
		{
			ReduceDurabilityByHit(context, defender, defender.Armors[bodyPart]);
			defender.NeedReduceArmorDurability = false;
		}
		return result;
	}

	public void DoSkillHit(CombatCharacter attacker, CombatCharacter defender, short skillId, sbyte bodyPart, sbyte hitType)
	{
		CombatContext context = CombatContext.Create(attacker, defender, bodyPart, skillId);
		CalcSkillHit(context, hitType, out var _, out var _);
	}

	public void UpdateSkillCd(DataContext context, CombatCharacter character)
	{
		foreach (CombatSkillData value in _skillDataDict.Values)
		{
			if (value.GetId().CharId == character.GetId() && value.GetLeftCdFrame() > 0)
			{
				value.SetLeftCdFrame((short)(value.GetLeftCdFrame() - 1), context);
				if (value.GetLeftCdFrame() == 0)
				{
					value.RaiseSkillSilenceEnd(context);
					UpdateSkillCanUse(context, character, value.GetId().SkillTemplateId);
				}
			}
		}
	}

	public void AddGoneMadInjury(DataContext context, CombatCharacter character, short skillId, int factor = 0)
	{
		CombatSkillItem combatSkillItem = Config.CombatSkill.Instance[skillId];
		int extraTotalPercent = character.GetGoneMadInjuryTotalPercent(combatSkillItem);
		sbyte goneMadInjuryValue = combatSkillItem.GoneMadInjuryValue;
		bool inner = combatSkillItem.GoneMadInnerInjury;
		DamageStepCollection damageStepCollection = character.GetDamageStepCollection();
		bool addingDisorderOfQi = false;
		sbyte part = character.RandomInjuryBodyPart(context.Random, inner, combatSkillItem.GoneMadInjuredPart);
		int num = 0;
		if (part < 0)
		{
			num = ModifyValue(goneMadInjuryValue * damageStepCollection.FatalDamageStep);
		}
		else
		{
			int[] array = (inner ? character.GetInnerDamageValue() : character.GetOuterDamageValue());
			int num2 = (inner ? damageStepCollection.InnerDamageSteps : damageStepCollection.OuterDamageSteps)[part];
			int num3 = 6 - character.GetInjuries().Get(part, inner);
			int damage = ModifyValue(goneMadInjuryValue * num2) + array[part];
			var (num4, num5) = CalcInjuryMarkCount(damage, num2, num3);
			if (num4 > 0)
			{
				AddInjury(context, character, part, inner, (sbyte)num4, updateDefeatMark: true);
			}
			if (num4 == num3)
			{
				num = num5 * damageStepCollection.FatalDamageStep / num2;
			}
			else
			{
				array[part] = num5;
				if (inner)
				{
					character.SetInnerDamageValue(array, context);
				}
				else
				{
					character.SetOuterDamageValue(array, context);
				}
			}
		}
		if (num > 0)
		{
			AddFatalDamageValue(context, character, num, combatSkillItem.GoneMadInnerInjury ? 1 : 0, -1, -1);
		}
		addingDisorderOfQi = true;
		character.GetCharacter().ChangeDisorderOfQiRandomRecovery(Context, ModifyValue(combatSkillItem.GoneMadQiDisorder));
		int ModifyValue(int value)
		{
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			value = DomainManager.SpecialEffect.ModifyValueCustom(character.GetId(), skillId, 117, value, -1, -1, -1, 0, 0, extraTotalPercent, extraTotalPercent) * CValuePercentBonus.op_Implicit(factor);
			return DomainManager.SpecialEffect.ModifyData(character.GetId(), skillId, 322, value, inner ? 1 : 0, part, addingDisorderOfQi ? 1 : 0);
		}
	}

	public void AddGoneMadInjuryOutOfCombat(DataContext context, GameData.Domains.Character.Character character, short skillId)
	{
		CombatSkillItem combatSkillItem = Config.CombatSkill.Instance[skillId];
		Injuries injuries = character.GetInjuries();
		List<sbyte> list = ObjectPool<List<sbyte>>.Instance.Get();
		list.Clear();
		foreach (sbyte item in combatSkillItem.GoneMadInjuredPart)
		{
			if (injuries.Get(item, combatSkillItem.GoneMadInnerInjury) < 6)
			{
				list.Add(item);
			}
		}
		if (list.Count > 0)
		{
			sbyte bodyPartType = list[context.Random.Next(0, list.Count)];
			injuries.Change(bodyPartType, combatSkillItem.GoneMadInnerInjury, combatSkillItem.GoneMadInjuryValue);
			character.SetInjuries(injuries, context);
		}
		ObjectPool<List<sbyte>>.Instance.Return(list);
	}

	[DomainMethod]
	public bool InterruptSkillManual(DataContext context, bool isAlly = true)
	{
		return InterruptSkill(context, isAlly ? _selfChar : _enemyChar, -1);
	}

	public bool InterruptSkill(DataContext context, CombatCharacter character, int odds = 100)
	{
		short preparingSkillId = character.GetPreparingSkillId();
		if (preparingSkillId < 0)
		{
			return false;
		}
		if (odds > 0)
		{
			odds = (DomainManager.SpecialEffect.ModifyData(character.GetId(), preparingSkillId, 215, dataValue: true) ? (odds + DomainManager.SpecialEffect.GetModifyValue(character.GetId(), preparingSkillId, 216, (EDataModifyType)0, -1, -1, -1, (EDataSumType)0)) : 0);
			odds = Math.Max(odds, 0);
		}
		if (odds < 0 || context.Random.CheckPercentProb(odds))
		{
			character.SetPreparingSkillId(-1, context);
			DomainManager.Combat.RaiseCastSkillEndByInterrupt(context, character.GetId(), character.IsAlly, preparingSkillId);
			return true;
		}
		return false;
	}

	public int GetInterruptSkillOdds(CombatSkillKey skillKey, CombatCharacter castingChar)
	{
		short preparingSkillId = castingChar.GetPreparingSkillId();
		if (preparingSkillId < 0)
		{
			return 0;
		}
		sbyte skillDirection = DomainManager.CombatSkill.GetSkillDirection(skillKey.CharId, skillKey.SkillTemplateId);
		if (skillDirection == -1)
		{
			return 0;
		}
		if (!DomainManager.SpecialEffect.ModifyData(castingChar.GetId(), preparingSkillId, 215, dataValue: true))
		{
			return 0;
		}
		Dictionary<short, Func<CombatSkillKey, bool, CombatSkillKey, int>> calcInterruptOddsFuncDict = CombatSkillEffectBase.CalcInterruptOddsFuncDict;
		int num = (calcInterruptOddsFuncDict.ContainsKey(skillKey.SkillTemplateId) ? calcInterruptOddsFuncDict[skillKey.SkillTemplateId](skillKey, skillDirection == 0, new CombatSkillKey(castingChar.GetId(), preparingSkillId)) : 0);
		num += DomainManager.SpecialEffect.GetModifyValue(castingChar.GetId(), preparingSkillId, 216, (EDataModifyType)0, -1, -1, -1, (EDataSumType)0);
		return Math.Clamp(num, 0, 100);
	}

	public bool SilenceSkill(DataContext context, CombatCharacter character, short skillId, int silenceFrame, int odds = 100)
	{
		CombatSkillKey combatSkillKey = new CombatSkillKey(character.GetId(), skillId);
		GameData.Domains.CombatSkill.CombatSkill element_CombatSkills = DomainManager.CombatSkill.GetElement_CombatSkills(combatSkillKey);
		if (odds > 0)
		{
			if (!DomainManager.SpecialEffect.ModifyData(character.GetId(), skillId, 217, dataValue: true))
			{
				return false;
			}
			int extraAddPercent = element_CombatSkills.GetPageEffects().Sum((SkillBreakPageEffectImplementItem x) => x.SilenceRate);
			odds = DomainManager.SpecialEffect.ModifyValue(character.GetId(), skillId, 218, odds, -1, -1, -1, 0, extraAddPercent);
			odds = Math.Max(odds, 0);
		}
		if (odds >= 0 && !context.Random.CheckPercentProb(odds))
		{
			return false;
		}
		if (silenceFrame > 0)
		{
			int extraAddPercent2 = element_CombatSkills.GetPageEffects().Sum((SkillBreakPageEffectImplementItem x) => x.SilenceFrame);
			(int, int) featureSilenceFrameTotalPercent = character.GetFeatureSilenceFrameTotalPercent();
			silenceFrame = DomainManager.SpecialEffect.ModifyValue(character.GetId(), skillId, 265, silenceFrame, -1, -1, -1, 0, extraAddPercent2, featureSilenceFrameTotalPercent.Item1, featureSilenceFrameTotalPercent.Item2);
		}
		short num = (short)Math.Clamp(silenceFrame, -1, 32767);
		if (num == 0)
		{
			return false;
		}
		CombatSkillData combatSkillData = _skillDataDict[combatSkillKey];
		if (combatSkillData.GetLeftCdFrame() < 0 || combatSkillData.GetTotalCdFrame() < 0)
		{
			return false;
		}
		if (num < 0 || num > combatSkillData.GetTotalCdFrame() || combatSkillData.GetLeftCdFrame() == 0)
		{
			combatSkillData.SetTotalCdFrame(num, context);
		}
		if (num < 0 || num > combatSkillData.GetLeftCdFrame())
		{
			combatSkillData.SetLeftCdFrame(num, context);
		}
		combatSkillData.RaiseSkillSilence(context);
		UpdateSkillCanUse(context, character, skillId);
		if (character.GetAffectingMoveSkillId() == skillId)
		{
			ClearAffectingAgileSkill(context, character);
		}
		if (character.GetAffectingDefendSkillId() == skillId)
		{
			ClearAffectingDefenseSkill(context, character);
		}
		if (character.GetPreparingSkillId() == skillId)
		{
			InterruptSkill(context, character);
		}
		return true;
	}

	public void DoubleSkillCd(DataContext context, CombatCharacter character, short skillId)
	{
		CombatSkillKey key = new CombatSkillKey(character.GetId(), skillId);
		CombatSkillData combatSkillData = _skillDataDict[key];
		if (combatSkillData.GetLeftCdFrame() > 0)
		{
			short num = (short)Math.Min(combatSkillData.GetTotalCdFrame() * 2, 32767);
			short leftCdFrame = (short)Math.Min(combatSkillData.GetLeftCdFrame() * 2, num);
			combatSkillData.SetTotalCdFrame(num, context);
			combatSkillData.SetLeftCdFrame(leftCdFrame, context);
		}
	}

	public void ResetSkillCd(DataContext context, CombatCharacter character, short skillId)
	{
		CombatSkillKey key = new CombatSkillKey(character.GetId(), skillId);
		CombatSkillData combatSkillData = _skillDataDict[key];
		combatSkillData.SetLeftCdFrame(combatSkillData.GetTotalCdFrame(), context);
	}

	public void ClearSkillCd(DataContext context, CombatCharacter character, short skillId)
	{
		CombatSkillKey key = new CombatSkillKey(character.GetId(), skillId);
		CombatSkillData combatSkillData = _skillDataDict[key];
		combatSkillData.SetTotalCdFrame(0, context);
		combatSkillData.SetLeftCdFrame(0, context);
		combatSkillData.RaiseSkillSilenceEnd(context);
		UpdateSkillCanUse(context, character, skillId);
	}

	public void RaiseCastSkillEndByInterrupt(DataContext context, int charId, bool isAlly, short skillId)
	{
		RaiseCastSkillEnd(context, charId, isAlly, skillId, 0, interrupt: true);
	}

	public void RaiseCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power = 0, bool interrupt = false, int finalCriticalOdds = 0)
	{
		Events.RaiseCastSkillEnd(context, charId, isAlly, skillId, power, interrupt);
		if (IsInCombat() && _combatCharacterDict.TryGetValue(charId, out var value))
		{
			DomainManager.Combat.OnCastSkillEndEffect(context, value, skillId, power, finalCriticalOdds);
		}
		Events.RaiseCastSkillAllEnd(context, charId, skillId);
	}

	public void OnCastSkillEndEffect(DataContext context, CombatCharacter combatChar, short skillId, int power = 0, int finalCriticalOdds = 0)
	{
		OnCastSkillEndBreakBonus(context, combatChar, skillId);
		OnCastSkillEndFeature(context, combatChar, skillId, power, finalCriticalOdds);
	}

	private void OnCastSkillEndBreakBonus(DataContext context, CombatCharacter combatChar, short skillId)
	{
		if (DomainManager.CombatSkill.TryGetElement_CombatSkills((charId: combatChar.GetId(), skillId: skillId), out var element))
		{
			int breakoutGridCombatSkillPropertyBonus = element.GetBreakoutGridCombatSkillPropertyBonus(70);
			if (breakoutGridCombatSkillPropertyBonus > 0)
			{
				int breakoutGridCombatSkillPropertyBonus2 = element.GetBreakoutGridCombatSkillPropertyBonus(71);
				SilenceSkill(context, combatChar, skillId, (short)Math.Clamp(breakoutGridCombatSkillPropertyBonus2, 0, 32767), breakoutGridCombatSkillPropertyBonus);
			}
		}
	}

	private void OnCastSkillEndFeature(DataContext context, CombatCharacter attacker, short skillId, int power, int finalCriticalOdds)
	{
		if (power < 100)
		{
			return;
		}
		CombatCharacter combatCharacter = GetCombatCharacter(!attacker.IsAlly, tryGetCoverCharacter: true);
		List<short> featureIds = attacker.GetCharacter().GetFeatureIds();
		List<short> featureIds2 = combatCharacter.GetCharacter().GetFeatureIds();
		byte fiveElements = NeiliType.Instance[combatCharacter.GetNeiliType()].FiveElements;
		foreach (LifeLinkFeatureEffectItem item in (IEnumerable<LifeLinkFeatureEffectItem>)LifeLinkFeatureEffect.Instance)
		{
			LifeLinkFeatureEffectItem config = item;
			bool flag = featureIds.Contains(config.FeatureId);
			bool flag2 = featureIds2.Contains(config.FeatureId);
			if (!flag && !flag2)
			{
				continue;
			}
			if (config.CriticalProbPercent > 0)
			{
				if (CheckProb() && flag && FiveElementEquals(config.FiveElements) && fiveElements == FiveElementsType.Countering[config.FiveElements])
				{
					AddGoneMadInjury(context, combatCharacter, skillId);
					ShowSpecialEffectTips(attacker.GetId(), 1713, 0);
				}
				if (CheckProb() && flag2 && FiveElementEquals(FiveElementsType.Countered[config.FiveElements]))
				{
					AddGoneMadInjury(context, combatCharacter, skillId);
					ShowSpecialEffectTips(combatCharacter.GetId(), 1714, 0);
				}
			}
			if (config.CriticalProbPercent < 0 && flag2 && CheckProb() && (FiveElementEquals(config.FiveElements) || FiveElementEquals(FiveElementsType.Countered[config.FiveElements])))
			{
				AddGoneMadInjury(context, combatCharacter, skillId);
				ShowSpecialEffectTips(combatCharacter.GetId(), 1715, 0);
			}
			bool CheckProb()
			{
				//IL_0021: Unknown result type (might be due to invalid IL or missing references)
				return context.Random.CheckPercentProb(finalCriticalOdds * CValuePercent.op_Implicit(Math.Abs(config.CriticalProbPercent)));
			}
		}
		bool FiveElementEquals(int fiveElementType)
		{
			return CombatSkillDomain.FiveElementEquals(attacker.GetId(), skillId, (sbyte)fiveElementType);
		}
	}

	public short GetRandomAttackSkill(CombatCharacter combatChar, sbyte skillType, sbyte targetGrade, IRandomSource random, bool descSearch = true, short expectSkillId = -1)
	{
		List<short> list = ObjectPool<List<short>>.Instance.Get();
		List<short> list2 = ObjectPool<List<short>>.Instance.Get();
		list.Clear();
		list.AddRange(combatChar.GetAttackSkillList());
		list.RemoveAll((short id) => id < 0 || id == expectSkillId);
		list2.Clear();
		targetGrade = Math.Clamp(targetGrade, 0, 8);
		for (int num = targetGrade; num != (descSearch ? (-1) : 9); num += ((!descSearch) ? 1 : (-1)))
		{
			for (int num2 = 0; num2 < list.Count; num2++)
			{
				short num3 = list[num2];
				if (Config.CombatSkill.Instance[num3].Grade == num && DomainManager.CombatSkill.GetSkillType(combatChar.GetId(), num3) == skillType && DomainManager.Combat.CanCastSkill(combatChar, num3, costFree: true))
				{
					list2.Add(num3);
				}
			}
			if (list2.Count > 0)
			{
				break;
			}
		}
		short result = (short)((list2.Count > 0) ? list2[random.Next(0, list2.Count)] : (-1));
		ObjectPool<List<short>>.Instance.Return(list);
		ObjectPool<List<short>>.Instance.Return(list2);
		return result;
	}

	[DomainMethod]
	public void ClearAffectingMoveSkillManual(DataContext context, bool isAlly)
	{
		CombatCharacter character = (isAlly ? _selfChar : _enemyChar);
		ClearAffectingAgileSkill(context, character);
	}

	public bool ClearAffectingAgileSkillByEffect(DataContext context, CombatCharacter character, CombatCharacter changer = null)
	{
		if (!DomainManager.SpecialEffect.ModifyData(character.GetId(), -1, 149, dataValue: true, changer?.GetId() ?? (-1)))
		{
			return false;
		}
		return ClearAffectingAgileSkill(context, character);
	}

	public bool ClearAffectingAgileSkill(DataContext context, CombatCharacter character)
	{
		if (character.GetAffectingMoveSkillId() < 0)
		{
			return false;
		}
		if (character.NeedAddEffectAgileSkillId == character.GetAffectingMoveSkillId())
		{
			character.NeedAddEffectAgileSkillId = -1;
		}
		character.SetAffectingMoveSkillId(-1, context);
		return true;
	}

	[DomainMethod]
	public void ClearAffectingDefenseSkillManual(DataContext context, bool isAlly)
	{
		CombatCharacter character = (isAlly ? _selfChar : _enemyChar);
		ClearAffectingDefenseSkill(context, character);
	}

	public void ClearAffectingDefenseSkill(DataContext context, CombatCharacter character)
	{
		if (character.GetAffectingDefendSkillId() >= 0)
		{
			character.DefendSkillLeftFrame = 1;
			character.SetAffectingDefendSkillId(-1, context);
			SetProperLoopAniAndParticle(context, character);
		}
	}

	public bool CanCastSkill(CombatCharacter character, short skillId, bool costFree = false, bool checkRange = false)
	{
		_skillDataDict.TryGetValue(new CombatSkillKey(character.GetId(), skillId), out var value);
		CombatSkillItem combatSkillItem = Config.CombatSkill.Instance[skillId];
		bool flag = combatSkillItem.EquipType == 1;
		return character.PreventCastSkillEffectCount == 0 && !character.PreparingTeammateCommand() && (value == null || value.GetLeftCdFrame() == 0) && (costFree || SkillCostEnough(character, skillId)) && HasSkillNeedBodyPart(character, skillId) && (!flag || WeaponHasNeedTrick(character, skillId, GetUsingWeaponData(character))) && (!character.IsAlly || costFree || SkillCanUseInCurrCombat(character.GetId(), combatSkillItem)) && SkillDirectionCanCast(character, skillId) && (!flag || !checkRange || SkillInCastRange(character, skillId));
	}

	public void CastSkillFree(DataContext context, CombatCharacter character, short skillId, ECombatCastFreePriority priority = ECombatCastFreePriority.Normal)
	{
		if (!character.CastFreeDataList.Contains((skillId: skillId, priority: priority)))
		{
			character.CastFreeDataList.Add((skillId: skillId, priority: priority));
			character.CastFreeDataList.Sort();
			UpdateAllCommandAvailability(context, character);
		}
	}

	public void ChangeSkillPrepareProgress(CombatCharacter character, int progress)
	{
		if (character.GetPreparingSkillId() >= 0)
		{
			character.SkillPrepareCurrProgress = Math.Max(character.SkillPrepareCurrProgress, progress);
		}
	}

	public void AddSkillPowerInCombat(DataContext context, CombatSkillKey skillKey, SkillEffectKey effectKey, int power)
	{
		if (power > 0)
		{
			SkillPowerChangeCollection value;
			bool flag = _skillPowerAddInCombat.TryGetValue(skillKey, out value);
			if (!flag)
			{
				value = new SkillPowerChangeCollection();
			}
			value.Add(effectKey, power);
			if (!flag)
			{
				AddElement_SkillPowerAddInCombat(skillKey, value, context);
			}
			else
			{
				SetElement_SkillPowerAddInCombat(skillKey, value, context);
			}
		}
	}

	public SkillPowerChangeCollection RemoveSkillPowerAddInCombat(DataContext context, CombatSkillKey skillKey)
	{
		if (!_skillPowerAddInCombat.TryGetValue(skillKey, out var value))
		{
			return null;
		}
		if (!DomainManager.SpecialEffect.ModifyData(skillKey.CharId, skillKey.SkillTemplateId, 220, dataValue: true))
		{
			return null;
		}
		RemoveElement_SkillPowerAddInCombat(skillKey, context);
		return value;
	}

	public int RemoveSkillPowerAddInCombat(DataContext context, CombatSkillKey skillKey, SkillEffectKey source)
	{
		if (!_skillPowerAddInCombat.TryGetValue(skillKey, out var value))
		{
			return 0;
		}
		if (!DomainManager.SpecialEffect.ModifyData(skillKey.CharId, skillKey.SkillTemplateId, 220, dataValue: true))
		{
			return 0;
		}
		if (value.EffectDict.ContainsKey(source))
		{
			int result = value.EffectDict[source];
			value.EffectDict.Remove(source);
			SetElement_SkillPowerAddInCombat(skillKey, value, context);
			return result;
		}
		return 0;
	}

	public Dictionary<CombatSkillKey, SkillPowerChangeCollection> GetAllSkillPowerAddInCombat()
	{
		return _skillPowerAddInCombat;
	}

	public int GetReduceSkillPowerInCombat(CombatSkillKey skillKey, SkillEffectKey effectKey)
	{
		if (!_skillPowerReduceInCombat.TryGetValue(skillKey, out var value))
		{
			return 0;
		}
		int value2;
		return value.EffectDict.TryGetValue(effectKey, out value2) ? value2 : 0;
	}

	public void ReduceSkillPowerInCombat(DataContext context, CombatSkillKey skillKey, SkillEffectKey effectKey, int power)
	{
		if (power < 0)
		{
			SkillPowerChangeCollection value;
			bool flag = _skillPowerReduceInCombat.TryGetValue(skillKey, out value);
			if (!flag)
			{
				value = new SkillPowerChangeCollection();
			}
			value.Add(effectKey, power);
			if (!flag)
			{
				AddElement_SkillPowerReduceInCombat(skillKey, value, context);
			}
			else
			{
				SetElement_SkillPowerReduceInCombat(skillKey, value, context);
			}
		}
	}

	public SkillPowerChangeCollection RemoveSkillPowerReduceInCombat(DataContext context, CombatSkillKey skillKey)
	{
		if (!_skillPowerReduceInCombat.TryGetValue(skillKey, out var value))
		{
			return null;
		}
		RemoveElement_SkillPowerReduceInCombat(skillKey, context);
		return value;
	}

	public int RemoveSkillPowerReduceInCombat(DataContext context, CombatSkillKey skillKey, SkillEffectKey source)
	{
		if (!_skillPowerReduceInCombat.TryGetValue(skillKey, out var value))
		{
			return 0;
		}
		if (value.EffectDict.ContainsKey(source))
		{
			int result = value.EffectDict[source];
			value.EffectDict.Remove(source);
			SetElement_SkillPowerReduceInCombat(skillKey, value, context);
			return result;
		}
		return 0;
	}

	public Dictionary<CombatSkillKey, SkillPowerChangeCollection> GetAllSkillPowerReduceInCombat()
	{
		return _skillPowerReduceInCombat;
	}

	public void SetSkillPowerReplaceInCombat(DataContext context, CombatSkillKey targetSkillKey, CombatSkillKey powerSkillKey)
	{
		if (!_skillPowerReplaceInCombat.ContainsKey(targetSkillKey))
		{
			AddElement_SkillPowerReplaceInCombat(targetSkillKey, powerSkillKey, context);
		}
		else
		{
			SetElement_SkillPowerReplaceInCombat(targetSkillKey, powerSkillKey, context);
		}
	}

	public void RemoveSkillPowerReplaceInCombat(DataContext context, CombatSkillKey targetSkillKey)
	{
		if (_skillPowerReplaceInCombat.ContainsKey(targetSkillKey))
		{
			RemoveElement_SkillPowerReplaceInCombat(targetSkillKey, context);
		}
	}

	public Dictionary<CombatSkillKey, CombatSkillKey> GetAllSkillPowerReplaceInCombat()
	{
		return _skillPowerReplaceInCombat;
	}

	public void AddMoveDistInSkillPrepare(CombatCharacter character, short dist, bool forward)
	{
		if (forward)
		{
			character.MoveData.CanMoveForwardInSkillPrepareDist += dist;
		}
		else
		{
			character.MoveData.CanMoveBackwardInSkillPrepareDist += dist;
		}
	}

	public void AddSkillEffect(DataContext context, CombatCharacter combatChar, SkillEffectKey key, short count, short maxCount, bool autoRemoveOnNoCount)
	{
		SkillEffectCollection skillEffectCollection = combatChar.GetSkillEffectCollection();
		SkillEffectCollection skillEffectCollection2 = skillEffectCollection;
		if (skillEffectCollection2.EffectDict == null)
		{
			skillEffectCollection2.EffectDict = new Dictionary<SkillEffectKey, short>();
		}
		skillEffectCollection2 = skillEffectCollection;
		if (skillEffectCollection2.EffectDescriptionDict == null)
		{
			skillEffectCollection2.EffectDescriptionDict = new Dictionary<SkillEffectKey, CombatSkillEffectDescriptionDisplayData>();
		}
		skillEffectCollection.EffectDict[key] = count;
		skillEffectCollection.EffectDescriptionDict[key] = DomainManager.CombatSkill.GetEffectDisplayData(combatChar.GetId(), key.SkillId);
		skillEffectCollection.MaxEffectCountDict[key] = maxCount;
		skillEffectCollection.AutoRemoveOnNoCountDict[key] = autoRemoveOnNoCount;
		combatChar.SetSkillEffectCollection(skillEffectCollection, context);
		Events.RaiseSkillEffectChange(context, combatChar.GetId(), key, 0, count, removed: false);
	}

	public void ChangeSkillEffectCount(DataContext context, CombatCharacter combatChar, SkillEffectKey key, short addValue, bool raiseEvent = true, bool forceChange = false)
	{
		if (!DomainManager.SpecialEffect.ModifyData(combatChar.GetId(), key.SkillId, 222, dataValue: true) && !forceChange)
		{
			return;
		}
		SkillEffectCollection skillEffectCollection = combatChar.GetSkillEffectCollection();
		if (skillEffectCollection.EffectDict == null || !skillEffectCollection.EffectDict.ContainsKey(key))
		{
			return;
		}
		short num = skillEffectCollection.EffectDict[key];
		short max = skillEffectCollection.MaxEffectCountDict[key];
		short num2 = (short)Math.Clamp(num + addValue, 0, max);
		if (num2 == 0 && skillEffectCollection.AutoRemoveOnNoCountDict[key])
		{
			RemoveSkillEffect(context, combatChar, key);
			return;
		}
		skillEffectCollection.EffectDict[key] = num2;
		combatChar.SetSkillEffectCollection(skillEffectCollection, context);
		if (raiseEvent)
		{
			Events.RaiseSkillEffectChange(context, combatChar.GetId(), key, num, num2, removed: false);
		}
	}

	public bool IsSkillEffectExist(CombatCharacter combatChar, SkillEffectKey key)
	{
		SkillEffectCollection skillEffectCollection = combatChar.GetSkillEffectCollection();
		return skillEffectCollection.EffectDict != null && skillEffectCollection.EffectDict.ContainsKey(key);
	}

	public short GetSkillEffectCount(CombatCharacter combatChar, SkillEffectKey key)
	{
		SkillEffectCollection skillEffectCollection = combatChar.GetSkillEffectCollection();
		if (skillEffectCollection.EffectDict != null && skillEffectCollection.EffectDict.ContainsKey(key))
		{
			return skillEffectCollection.EffectDict[key];
		}
		return 0;
	}

	public void ClearSkillEffect(DataContext context, CombatCharacter combatChar)
	{
		SkillEffectCollection skillEffectCollection = combatChar.GetSkillEffectCollection();
		if (skillEffectCollection.EffectDict != null)
		{
			List<SkillEffectKey> list = ObjectPool<List<SkillEffectKey>>.Instance.Get();
			List<short> list2 = ObjectPool<List<short>>.Instance.Get();
			list.Clear();
			list2.Clear();
			list.AddRange(skillEffectCollection.EffectDict.Keys);
			list2.AddRange(skillEffectCollection.EffectDict.Values);
			skillEffectCollection.EffectDict.Clear();
			skillEffectCollection.EffectDescriptionDict.Clear();
			skillEffectCollection.MaxEffectCountDict.Clear();
			skillEffectCollection.AutoRemoveOnNoCountDict.Clear();
			combatChar.SetSkillEffectCollection(skillEffectCollection, context);
			for (int i = 0; i < list.Count; i++)
			{
				Events.RaiseSkillEffectChange(context, combatChar.GetId(), list[i], list2[i], 0, removed: true);
			}
			ObjectPool<List<SkillEffectKey>>.Instance.Return(list);
			ObjectPool<List<short>>.Instance.Return(list2);
		}
	}

	public void RemoveSkillEffect(DataContext context, CombatCharacter combatChar, SkillEffectKey key)
	{
		SkillEffectCollection skillEffectCollection = combatChar.GetSkillEffectCollection();
		if (skillEffectCollection.EffectDict != null && skillEffectCollection.EffectDict.ContainsKey(key))
		{
			short oldCount = skillEffectCollection.EffectDict[key];
			skillEffectCollection.EffectDict.Remove(key);
			skillEffectCollection.EffectDescriptionDict.Remove(key);
			skillEffectCollection.MaxEffectCountDict.Remove(key);
			skillEffectCollection.AutoRemoveOnNoCountDict.Remove(key);
			combatChar.SetSkillEffectCollection(skillEffectCollection, context);
			Events.RaiseSkillEffectChange(context, combatChar.GetId(), key, oldCount, 0, removed: true);
		}
	}

	public bool ChangeSkillEffectRandom(DataContext context, CombatCharacter target, CValuePercent percent, int maxChangeCount = 1, sbyte requireEquipType = -1)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_017b: Unknown result type (might be due to invalid IL or missing references)
		if (percent == 0)
		{
			return false;
		}
		SkillEffectCollection skillEffectCollection = target.GetSkillEffectCollection();
		if (skillEffectCollection?.EffectDict == null)
		{
			return false;
		}
		bool result = false;
		bool flag = percent > 0;
		int num = 0;
		List<SkillEffectKey> list = ObjectPool<List<SkillEffectKey>>.Instance.Get();
		List<SkillEffectKey> list2 = ObjectPool<List<SkillEffectKey>>.Instance.Get();
		foreach (KeyValuePair<SkillEffectKey, short> item in skillEffectCollection.EffectDict)
		{
			item.Deconstruct(out var key, out var value);
			SkillEffectKey skillEffectKey = key;
			short num2 = value;
			short num3 = skillEffectCollection.MaxEffectCountDict[skillEffectKey];
			int num4 = (flag ? (num3 - num2) : num2);
			if (num4 != 0 && (requireEquipType < 0 || requireEquipType == Config.CombatSkill.Instance[skillEffectKey.SkillId].EquipType))
			{
				if (num4 > num)
				{
					list2.AddRange(list);
					list.Clear();
					num = num4;
				}
				if (num4 == num)
				{
					list.Add(skillEffectKey);
				}
				else
				{
					list2.Add(skillEffectKey);
				}
			}
		}
		foreach (SkillEffectKey item2 in RandomUtils.GetRandomUnrepeated(context.Random, maxChangeCount, list, list2))
		{
			result = true;
			int num5 = (int)skillEffectCollection.MaxEffectCountDict[item2] * percent;
			if (num5 == 0)
			{
				num5 = (flag ? 1 : (-1));
			}
			ChangeSkillEffectCount(context, target, item2, (short)num5);
		}
		ObjectPool<List<SkillEffectKey>>.Instance.Return(list);
		ObjectPool<List<SkillEffectKey>>.Instance.Return(list2);
		return result;
	}

	public void ChangeSkillEffectToMinCount(DataContext context, CombatCharacter combatChar, SkillEffectKey key)
	{
		SkillEffectCollection skillEffectCollection = combatChar.GetSkillEffectCollection();
		if (skillEffectCollection.EffectDict != null && skillEffectCollection.EffectDict.ContainsKey(key))
		{
			CombatSkillItem combatSkillItem = Config.CombatSkill.Instance[key.SkillId];
			int index = (key.IsDirect ? combatSkillItem.DirectEffectID : combatSkillItem.ReverseEffectID);
			short minEffectCount = Config.SpecialEffect.Instance[index].MinEffectCount;
			if (skillEffectCollection.EffectDict[key] > minEffectCount)
			{
				ChangeSkillEffectCount(context, combatChar, key, (short)(minEffectCount - skillEffectCollection.EffectDict[key]));
			}
		}
	}

	public void ChangeSkillEffectToMaxCount(DataContext context, CombatCharacter combatChar, SkillEffectKey key)
	{
		SkillEffectCollection skillEffectCollection = combatChar.GetSkillEffectCollection();
		if (skillEffectCollection.EffectDict != null && skillEffectCollection.EffectDict.ContainsKey(key))
		{
			short num = skillEffectCollection.MaxEffectCountDict[key];
			if (skillEffectCollection.EffectDict[key] < num)
			{
				ChangeSkillEffectCount(context, combatChar, key, (short)(num - skillEffectCollection.EffectDict[key]));
			}
		}
	}

	public void ChangeSkillEffectDirection(DataContext context, CombatCharacter combatChar, SkillEffectKey key, bool isDirect)
	{
		SkillEffectCollection skillEffectCollection = combatChar.GetSkillEffectCollection();
		if (skillEffectCollection.EffectDict != null && skillEffectCollection.EffectDict.ContainsKey(key))
		{
			SkillEffectKey key2 = new SkillEffectKey(key.SkillId, isDirect);
			skillEffectCollection.EffectDict.Add(key2, skillEffectCollection.EffectDict[key]);
			skillEffectCollection.EffectDescriptionDict.Add(key2, skillEffectCollection.EffectDescriptionDict[key]);
			skillEffectCollection.MaxEffectCountDict.Add(key2, skillEffectCollection.MaxEffectCountDict[key]);
			skillEffectCollection.AutoRemoveOnNoCountDict.Add(key2, skillEffectCollection.AutoRemoveOnNoCountDict[key]);
			skillEffectCollection.EffectDict.Remove(key);
			skillEffectCollection.EffectDescriptionDict.Remove(key);
			skillEffectCollection.MaxEffectCountDict.Remove(key);
			skillEffectCollection.AutoRemoveOnNoCountDict.Remove(key);
			combatChar.SetSkillEffectCollection(skillEffectCollection, context);
		}
	}

	[DomainMethod]
	public void PlayMoveStepSound(DataContext context, bool isAlly)
	{
		if (IsInCombat())
		{
			CombatCharacter character = (isAlly ? _selfChar : _enemyChar);
			PlayStepSound(context, character);
			PlayShockSound(context, character);
		}
	}

	public void PlayHitSound(DataContext context, CombatCharacter character, WeaponItem weaponData)
	{
		List<string> hitSounds = weaponData.HitSounds;
		ItemKey itemKey = character.Armors[0];
		if (hitSounds != null && hitSounds.Count > 0)
		{
			character.SetHitSoundToPlay(hitSounds[context.Random.Next(hitSounds.Count)], context);
		}
		if (!itemKey.IsValid())
		{
			return;
		}
		sbyte resourceType = DomainManager.Item.GetElement_Armors(itemKey.Id).GetResourceType();
		if (resourceType >= 0)
		{
			string[] hitSound = Config.ResourceType.Instance[resourceType].HitSound;
			string[] shockSound = Config.ResourceType.Instance[resourceType].ShockSound;
			if (weaponData.PlayArmorHitSound && hitSound != null && hitSound.Length != 0)
			{
				character.SetArmorHitSoundToPlay(hitSound[context.Random.Next(hitSound.Length)], context);
			}
			if (shockSound != null && shockSound.Length != 0)
			{
				character.SetShockSoundToPlay(shockSound[context.Random.Next(shockSound.Length)], context);
			}
		}
	}

	public void PlayBlockSound(DataContext context, CombatCharacter character)
	{
		string text = null;
		if (character.AnimalConfig == null)
		{
			List<string> blockSounds = Config.Weapon.Instance[GetUsingWeaponKey(character).TemplateId].BlockSounds;
			if (blockSounds != null && blockSounds.Count > 0)
			{
				text = blockSounds[context.Random.Next(blockSounds.Count)];
			}
		}
		else
		{
			text = character.AnimalConfig.BlockSound;
		}
		if (text != null)
		{
			character.SetHitSoundToPlay(text, context);
		}
	}

	public void PlayWhooshSound(DataContext context, CombatCharacter character)
	{
		ItemKey itemKey = character.Armors[0];
		if (itemKey.IsValid())
		{
			sbyte resourceType = DomainManager.Item.GetElement_Armors(itemKey.Id).GetResourceType();
			if (resourceType >= 0)
			{
				string[] whooshSound = Config.ResourceType.Instance[resourceType].WhooshSound;
				if (whooshSound != null && whooshSound.Length != 0)
				{
					character.SetWhooshSoundToPlay(whooshSound[context.Random.Next(whooshSound.Length)], context);
				}
			}
		}
		else
		{
			character.SetWhooshSoundToPlay("se_combat_whoosh_empty", context);
		}
	}

	public void PlayShockSound(DataContext context, CombatCharacter character)
	{
		ItemKey itemKey = character.Armors[0];
		if (!itemKey.IsValid())
		{
			return;
		}
		sbyte resourceType = DomainManager.Item.GetElement_Armors(itemKey.Id).GetResourceType();
		if (resourceType >= 0)
		{
			string[] shockSound = Config.ResourceType.Instance[resourceType].ShockSound;
			if (shockSound != null && shockSound.Length != 0)
			{
				character.SetShockSoundToPlay(shockSound[context.Random.Next(shockSound.Length)], context);
			}
		}
	}

	public void PlayStepSound(DataContext context, CombatCharacter character)
	{
		ItemKey itemKey = character.Armors[5];
		IList<string> list;
		if (character.IsAnimal)
		{
			list = character.AnimalConfig.StepSound;
		}
		else if (itemKey.IsValid())
		{
			sbyte resourceType = DomainManager.Item.GetElement_Armors(itemKey.Id).GetResourceType();
			list = ((resourceType >= 0) ? Config.ResourceType.Instance[resourceType].StepSound : NoResourceTypeStepSound);
		}
		else
		{
			list = NoResourceTypeStepSound;
		}
		if (list != null && list.Count > 0)
		{
			character.SetStepSoundToPlay(list.GetRandom(context.Random), context);
		}
	}

	[DomainMethod]
	public bool ExecuteTeammateCommand(DataContext context, bool isAlly, int index, int charId)
	{
		if (!IsCharInCombat(charId))
		{
			return false;
		}
		CombatCharacter element_CombatCharacterDict = GetElement_CombatCharacterDict(charId);
		if (element_CombatCharacterDict.IsAlly != isAlly)
		{
			return false;
		}
		CombatCharacter combatCharacter = GetCombatCharacter(isAlly);
		List<sbyte> currTeammateCommands = element_CombatCharacterDict.GetCurrTeammateCommands();
		if (currTeammateCommands.Count <= index || !element_CombatCharacterDict.GetTeammateCommandCanUse()[index] || element_CombatCharacterDict.GetExecutingTeammateCommand() >= 0 || combatCharacter == element_CombatCharacterDict)
		{
			return false;
		}
		sbyte index2 = (sbyte)(GetMainCharacter(isAlly).GetShowTransferInjuryCommand() ? 13 : element_CombatCharacterDict.GetCurrTeammateCommands()[index]);
		TeammateCommandItem teammateCommandItem = TeammateCommand.Instance[index2];
		ETeammateCommandImplement implement = teammateCommandItem.Implement;
		if (implement == ETeammateCommandImplement.TransferInjury && index != 0)
		{
			return false;
		}
		combatCharacter.SetNeedTeammateCommand(context, charId, index);
		return true;
	}

	[DomainMethod]
	public CombatCharacterDisplayData GetCombatCharDisplayData(DataContext context, int charId)
	{
		if (!TryGetElement_CombatCharacterDict(charId, out var element))
		{
			return null;
		}
		CombatCharacterDisplayData combatCharacterDisplayData = new CombatCharacterDisplayData();
		combatCharacterDisplayData.DefeatMarks = element.GetDefeatMarkCollection();
		combatCharacterDisplayData.OldInjuries = element.GetOldInjuries();
		combatCharacterDisplayData.OldPoisons = element.GetOldPoison();
		combatCharacterDisplayData.OldDisorderOfQi = element.GetOldDisorderOfQi();
		combatCharacterDisplayData.Happiness = element.GetHappiness();
		return combatCharacterDisplayData;
	}

	private int GetTeamWisdomCount(bool isAlly)
	{
		return CFormulaHelper.CalcTeamWisdomCount(isAlly ? _selfTeam : _enemyTeam);
	}

	public void GetAllCharInCombat(List<int> charIdList)
	{
		charIdList.Clear();
		charIdList.AddRange(_combatCharacterDict.Keys);
	}

	public bool IsTeamCharacter(int charId)
	{
		return _selfTeam.Exist(charId) || _enemyTeam.Exist(charId);
	}

	public bool IsMainCharacter(CombatCharacter character)
	{
		return character.GetId() == (character.IsAlly ? _selfTeam : _enemyTeam)[0];
	}

	public bool AnyTeammateChar(bool isAlly)
	{
		int[] array = (isAlly ? _selfTeam : _enemyTeam);
		for (int i = 1; i < array.Length; i++)
		{
			if (array[i] >= 0)
			{
				return true;
			}
		}
		return false;
	}

	public CombatCharacter GetMainCharacter(bool isAlly)
	{
		return GetElement_CombatCharacterDict((isAlly ? _selfTeam : _enemyTeam)[0]);
	}

	public IEnumerable<int> GetTeamCharacterIds()
	{
		int[] selfTeam = _selfTeam;
		foreach (int teamCharId in selfTeam)
		{
			if (teamCharId >= 0)
			{
				yield return teamCharId;
			}
		}
		int[] enemyTeam = _enemyTeam;
		foreach (int teamCharId2 in enemyTeam)
		{
			if (teamCharId2 >= 0)
			{
				yield return teamCharId2;
			}
		}
	}

	public int[] GetCharacterList(bool isAlly)
	{
		return isAlly ? _selfTeam : _enemyTeam;
	}

	public IEnumerable<CombatCharacter> GetCharacters(bool isAlly)
	{
		int[] team = GetCharacterList(isAlly);
		for (int i = 0; i < team.Length; i++)
		{
			if (team[i] >= 0)
			{
				yield return _combatCharacterDict[team[i]];
			}
		}
	}

	public IEnumerable<CombatCharacter> GetTeammateCharacters(int charId)
	{
		if (!IsCharInCombat(charId))
		{
			yield break;
		}
		CombatCharacter combatChar = _combatCharacterDict[charId];
		if (!IsMainCharacter(combatChar))
		{
			yield break;
		}
		int[] team = GetCharacterList(combatChar.IsAlly);
		for (int i = 1; i < team.Length; i++)
		{
			if (team[i] >= 0)
			{
				yield return _combatCharacterDict[team[i]];
			}
		}
	}

	public int GetMaxOriginNeiliAllocationSum(bool isAlly)
	{
		NeiliAllocation neiliAllocation = default(NeiliAllocation);
		neiliAllocation.Initialize();
		foreach (CombatCharacter character in GetCharacters(isAlly))
		{
			NeiliAllocation originNeiliAllocation = character.GetOriginNeiliAllocation();
			for (int i = 0; i < 4; i++)
			{
				neiliAllocation[i] = Math.Max(neiliAllocation[i], originNeiliAllocation[i]);
			}
		}
		return neiliAllocation.Sum();
	}

	public bool IsCurrentCombatCharacter(CombatCharacter character)
	{
		return GetCombatCharacter(character.IsAlly) == character;
	}

	public CombatCharacter GetCombatCharacter(bool isAlly, bool tryGetCoverCharacter = false)
	{
		CombatCharacter combatCharacter = (isAlly ? _selfChar : _enemyChar);
		if (tryGetCoverCharacter && combatCharacter.TeammateBeforeMainChar >= 0)
		{
			CombatCharacter combatCharacter2 = _combatCharacterDict[combatCharacter.TeammateBeforeMainChar];
			if (combatCharacter2.GetVisible())
			{
				return combatCharacter2;
			}
		}
		return combatCharacter;
	}

	public void SetCombatCharacter(DataContext context, bool isAlly, int charId)
	{
		CombatCharacter combatCharacter = _combatCharacterDict[charId];
		combatCharacter.SetVisible(visible: true, context);
		if (!IsMainCharacter(combatCharacter))
		{
			TrickCollection tricks = combatCharacter.GetTricks();
			tricks.ClearTricks();
			combatCharacter.SetTricks(tricks, context);
		}
		if (isAlly)
		{
			if (!_selfTeam.Exist(charId))
			{
				throw new Exception("Character " + charId + " is not in self team");
			}
			SetSelfCharId(charId, context);
			_selfChar = combatCharacter;
		}
		else
		{
			if (!_enemyTeam.Exist(charId))
			{
				throw new Exception("Character " + charId + " is not in enemy team");
			}
			SetEnemyCharId(charId, context);
			_enemyChar = combatCharacter;
		}
		UpdateAllCommandAvailability(context, combatCharacter);
	}

	public bool IsCharInCombat(int charId, bool checkCombatStatus = true)
	{
		return (!checkCombatStatus || IsInCombat()) && _combatCharacterDict.ContainsKey(charId);
	}

	public bool IsAlly(int charId1, int charId2)
	{
		return _combatCharacterDict[charId1].IsAlly == _combatCharacterDict[charId2].IsAlly;
	}

	public void UpdateAllTeammateCommandUsable(DataContext context, bool isAlly, sbyte type = -1)
	{
		ETeammateCommandImplement implement = ((type < 0) ? ETeammateCommandImplement.Invalid : TeammateCommand.Instance[type].Implement);
		UpdateAllTeammateCommandUsable(context, isAlly, implement);
	}

	public void UpdateAllTeammateCommandUsable(DataContext context, bool isAlly, ETeammateCommandImplement implement)
	{
		int[] array = (isAlly ? _selfTeam : _enemyTeam);
		for (int i = 1; i < array.Length; i++)
		{
			int num = array[i];
			if (num >= 0)
			{
				UpdateTeammateCommandUsable(context, GetElement_CombatCharacterDict(num), implement);
			}
		}
	}

	public void UpdateTeammateCommandUsable(DataContext context, CombatCharacter teammateChar, sbyte type = -1)
	{
		ETeammateCommandImplement implement = ((type < 0) ? ETeammateCommandImplement.Invalid : TeammateCommand.Instance[type].Implement);
		UpdateTeammateCommandUsable(context, teammateChar, implement);
	}

	public void UpdateTeammateCommandUsable(DataContext context, CombatCharacter teammateChar, ETeammateCommandImplement implement)
	{
		CombatCharacter combatCharacter = GetCombatCharacter(teammateChar.IsAlly);
		List<sbyte> currTeammateCommands = teammateChar.GetCurrTeammateCommands();
		List<SByteList> cmdBanReasonList = teammateChar.GetTeammateCommandBanReasons();
		TeammateCommandCheckerContext checkerContext = new TeammateCommandCheckerContext
		{
			CurrChar = combatCharacter,
			TeammateChar = teammateChar
		};
		checkerContext.InitExtraFields();
		List<sbyte> tempBanReasons = ObjectPool<List<sbyte>>.Instance.Get();
		bool changed = false;
		if (GetMainCharacter(teammateChar.IsAlly).GetShowTransferInjuryCommand())
		{
			for (int i = 0; i < currTeammateCommands.Count; i++)
			{
				UpdateBanReasons(ETeammateCommandImplement.TransferInjury, i);
			}
		}
		else
		{
			List<int> list = ObjectPool<List<int>>.Instance.Get();
			list.Clear();
			for (int j = 0; j < currTeammateCommands.Count; j++)
			{
				sbyte b = currTeammateCommands[j];
				ETeammateCommandImplement eTeammateCommandImplement = ((b < 0) ? ETeammateCommandImplement.Invalid : TeammateCommand.Instance[b].Implement);
				bool flag = implement == ETeammateCommandImplement.Invalid || eTeammateCommandImplement == implement;
				bool flag2 = flag;
				if (!flag2)
				{
					bool flag3 = (uint)(eTeammateCommandImplement - 2) <= 1u;
					bool flag4 = flag3;
					bool flag5 = flag4;
					if (flag5)
					{
						bool flag6 = (uint)(implement - 2) <= 1u;
						flag5 = flag6;
					}
					flag2 = flag5;
				}
				if (flag2)
				{
					list.Add(j);
				}
			}
			foreach (int item in list)
			{
				sbyte b2 = currTeammateCommands[item];
				ETeammateCommandImplement cmdImplement = ((b2 < 0) ? ETeammateCommandImplement.Invalid : TeammateCommand.Instance[b2].Implement);
				UpdateBanReasons(cmdImplement, item);
			}
			ObjectPool<List<int>>.Instance.Return(list);
		}
		ObjectPool<List<sbyte>>.Instance.Return(tempBanReasons);
		if (changed)
		{
			teammateChar.SetTeammateCommandBanReasons(cmdBanReasonList, context);
		}
		void UpdateBanReasons(ETeammateCommandImplement key, int index)
		{
			tempBanReasons.Clear();
			if (TeammateCommandCheckers.TryGetValue(key, out var value))
			{
				tempBanReasons.AddRange(from x in value.Check(index, checkerContext)
					select (sbyte)x);
			}
			else
			{
				tempBanReasons.Add(-1);
			}
			SByteList sByteList = cmdBanReasonList[index];
			if (!sByteList.Items.SequenceEqual(tempBanReasons))
			{
				sByteList.Items.Clear();
				sByteList.Items.AddRange(tempBanReasons);
				changed = true;
			}
		}
	}

	public void ForceAllTeammateLeaveCombatField(DataContext context, bool isAlly)
	{
		CombatCharacter combatCharacter = GetCombatCharacter(isAlly);
		if (!IsMainCharacter(combatCharacter))
		{
			combatCharacter.ChangeCharId = GetCharacterList(isAlly)[0];
		}
		if (combatCharacter.TeammateBeforeMainChar >= 0)
		{
			GetElement_CombatCharacterDict(combatCharacter.TeammateBeforeMainChar).ClearTeammateCommand(context, interrupt: true);
		}
		if (combatCharacter.TeammateAfterMainChar >= 0)
		{
			GetElement_CombatCharacterDict(combatCharacter.TeammateAfterMainChar).ClearTeammateCommand(context, interrupt: true);
		}
	}

	public void TryUpdatePreRandomizedTeammateCommands(DataContext context, int teammateId)
	{
		IReadOnlyList<sbyte> readOnlyList = PreRandomizedTeammateCommandReplaceData?.GetCharTeammateCommands(teammateId);
		if (readOnlyList != null)
		{
			List<sbyte> list = ObjectPool<List<sbyte>>.Instance.Get();
			list.Clear();
			list.AddRange(DomainManager.Extra.GetCharUsableTeammateCommands(context, teammateId));
			if (!list.SequenceEqual(readOnlyList))
			{
				PreRandomizedTeammateCommandReplaceData.SetCharTeammateCommands(teammateId, list);
				GameData.GameDataBridge.GameDataBridge.AddDisplayEvent(DisplayEventType.ChangeTeammateCommandOnCombatBegin, teammateId, list);
			}
			ObjectPool<List<sbyte>>.Instance.Return(list);
		}
	}

	private static TeammateCommandChangeDataPart CalcTeammateBetrayData(DataContext context, short combatConfigId, IReadOnlyList<int> charIds, bool isAlly)
	{
		CombatConfigItem combatConfigItem = Config.CombatConfig.Instance[combatConfigId];
		TeammateCommandChangeDataPart teammateCommandChangeDataPart = new TeammateCommandChangeDataPart();
		int relatedCharId = charIds[0];
		for (int i = 1; i < charIds.Count; i++)
		{
			int num = charIds[i];
			if (num < 0)
			{
				continue;
			}
			teammateCommandChangeDataPart.TeammateCharIds.Add(num);
			IEnumerable<sbyte> charUsableTeammateCommands = DomainManager.Extra.GetCharUsableTeammateCommands(context, num);
			SByteList item = new SByteList(charUsableTeammateCommands);
			List<sbyte> items = item.Items;
			teammateCommandChangeDataPart.OriginTeammateCommands.Add(item);
			SByteList item2 = SByteList.Create();
			List<sbyte> items2 = item2.Items;
			if (combatConfigItem.SpecialTeammateCommands.Count > i - 1 && !isAlly)
			{
				List<sbyte> list = combatConfigItem.SpecialTeammateCommands[i - 1];
				if (list != null && list.Count > 0)
				{
					items.Clear();
					items2.AddRange(combatConfigItem.SpecialTeammateCommands[i - 1]);
					goto IL_0125;
				}
			}
			items2.AddRange(items);
			short favorability = DomainManager.Character.GetFavorability(num, relatedCharId);
			int count = CalcNegativeTeammateCommandCount(favorability, items);
			DomainManager.Extra.FillNegativeTeammateCommands(context.Random, num, count, items2);
			goto IL_0125;
			IL_0125:
			teammateCommandChangeDataPart.ReplaceTeammateCommands.Add(item2);
		}
		return teammateCommandChangeDataPart;
	}

	private static int CalcNegativeTeammateCommandCount(short favor, IReadOnlyList<sbyte> cmdTypes)
	{
		sbyte favorabilityType = FavorabilityType.GetFavorabilityType(favor);
		if (1 == 0)
		{
		}
		int num = ((favorabilityType <= -3) ? ((favorabilityType > -5) ? 2 : 3) : ((favorabilityType <= -1) ? 1 : 0));
		if (1 == 0)
		{
		}
		int val = num;
		int num2 = cmdTypes.Count((sbyte x) => TeammateCommand.Instance[x].Type == ETeammateCommandType.Advance);
		return Math.Min(val, 3 - num2);
	}

	public IReadOnlyList<sbyte> GetPreRandomizedTeammateCommands(DataContext context, int teammateId)
	{
		IReadOnlyList<sbyte> readOnlyList = PreRandomizedTeammateCommandReplaceData?.GetCharTeammateCommands(teammateId);
		return readOnlyList ?? DomainManager.Extra.GetCharOriginalTeammateCommands(context, teammateId);
	}

	private void ProcessVitalDemonBetray(DataContext context, TeammateCommandChangeDataPart enemyTeam)
	{
		bool flag = DomainManager.Extra.AreVitalsDemon();
		List<SectStoryThreeVitalsCharacterType> list = null;
		SectStoryThreeVitalsCharacterType[] values = Enum.GetValues<SectStoryThreeVitalsCharacterType>();
		foreach (SectStoryThreeVitalsCharacterType sectStoryThreeVitalsCharacterType in values)
		{
			SectStoryThreeVitalsCharacter vitalByType = DomainManager.Extra.GetVitalByType(sectStoryThreeVitalsCharacterType);
			if (vitalByType == null)
			{
				continue;
			}
			int percentProb = vitalByType.CalcBetrayOdds(flag);
			if (context.Random.CheckPercentProb(percentProb))
			{
				if (list == null)
				{
					list = new List<SectStoryThreeVitalsCharacterType>();
				}
				list.Add(sectStoryThreeVitalsCharacterType);
			}
		}
		if (list != null)
		{
			int val = 3 - enemyTeam.TeammateCharIds.Count;
			val = Math.Min(val, list.Count);
			list.Sort();
			if (val < list.Count)
			{
				list.MoveLastToFirst(val);
			}
			int num = 0;
			bool vitalIsDemon = !flag;
			for (int j = 0; j < val; j++)
			{
				SectStoryThreeVitalsCharacterType type = list[num++];
				short vitalTemplateId = type.GetVitalTemplateId(vitalIsDemon);
				GameData.Domains.Character.Character orCreateFixedCharacterByTemplateId = DomainManager.Character.GetOrCreateFixedCharacterByTemplateId(context, vitalTemplateId);
				int id = orCreateFixedCharacterByTemplateId.GetId();
				SByteList charTeammateCommandsSByteList = DomainManager.Extra.GetCharTeammateCommandsSByteList(context, id);
				enemyTeam.TeammateCharIds.Add(id);
				enemyTeam.OriginTeammateCommands.Add(charTeammateCommandsSByteList);
				enemyTeam.ReplaceTeammateCommands.Add(charTeammateCommandsSByteList);
			}
			int num2 = Math.Min(3, list.Count) - val;
			for (int k = 0; k < num2; k++)
			{
				SectStoryThreeVitalsCharacterType type2 = list[num++];
				short vitalTemplateId2 = type2.GetVitalTemplateId(vitalIsDemon);
				GameData.Domains.Character.Character orCreateFixedCharacterByTemplateId2 = DomainManager.Character.GetOrCreateFixedCharacterByTemplateId(context, vitalTemplateId2);
				int id2 = orCreateFixedCharacterByTemplateId2.GetId();
				SByteList charTeammateCommandsSByteList2 = DomainManager.Extra.GetCharTeammateCommandsSByteList(context, id2);
				enemyTeam.BetrayedCharIds[k] = enemyTeam.TeammateCharIds[k];
				enemyTeam.TeammateCharIds[k] = id2;
				List<SByteList> originTeammateCommands = enemyTeam.OriginTeammateCommands;
				int index = k;
				SByteList value = (enemyTeam.ReplaceTeammateCommands[k] = charTeammateCommandsSByteList2);
				originTeammateCommands[index] = value;
			}
		}
	}

	public void JoinSpecialGroup(DataContext context, int charId)
	{
		if (!_taiwuSpecialGroupCharIds.Contains(charId))
		{
			_taiwuSpecialGroupCharIds.Add(charId);
			SetTaiwuSpecialGroupCharIds(_taiwuSpecialGroupCharIds, context);
			SpecialGroupInvalidateAllCaches(context, charId);
		}
	}

	public void ExitSpecialGroup(DataContext context, int charId)
	{
		if (_taiwuSpecialGroupCharIds.Remove(charId))
		{
			SetTaiwuSpecialGroupCharIds(_taiwuSpecialGroupCharIds, context);
			SpecialGroupInvalidateAllCaches(context, charId);
		}
	}

	public void ClearSpecialGroup(DataContext context)
	{
		List<int> taiwuSpecialGroupCharIds = _taiwuSpecialGroupCharIds;
		if (taiwuSpecialGroupCharIds == null || taiwuSpecialGroupCharIds.Count <= 0)
		{
			return;
		}
		List<int> list = ObjectPool<List<int>>.Instance.Get();
		list.AddRange(_taiwuSpecialGroupCharIds);
		_taiwuSpecialGroupCharIds.Clear();
		SetTaiwuSpecialGroupCharIds(_taiwuSpecialGroupCharIds, context);
		foreach (int item in list)
		{
			SpecialGroupInvalidateAllCaches(context, item);
		}
		ObjectPool<List<int>>.Instance.Return(list);
	}

	private void SpecialGroupInvalidateAllCaches(DataContext context, int charId)
	{
		if (DomainManager.Character.TryGetElement_Objects(charId, out var element))
		{
			element.InvalidateSelfAndInfluencedCache(112, context);
		}
	}

	public void ForbidNormalAttackInTutorial(int charId)
	{
		if (_isTutorialCombat)
		{
			CombatCharacter combatCharacter = _combatCharacterDict[charId];
			combatCharacter.ForbidNormalAttackEffectCount = int.MaxValue;
		}
	}

	public void PermitNormalAttackInTutorial(int charId)
	{
		if (_isTutorialCombat)
		{
			CombatCharacter combatCharacter = _combatCharacterDict[charId];
			combatCharacter.ForbidNormalAttackEffectCount = 0;
		}
	}

	public void ClearMobilityAndForbidRecover(DataContext context, int charId)
	{
		CombatCharacter combatCharacter = _combatCharacterDict[charId];
		combatCharacter.CanRecoverMobility = false;
		ChangeMobilityValue(context, combatCharacter, -MoveSpecialConstants.MaxMobility);
	}

	public void SetAttackForceMiss(int charId, sbyte count)
	{
		_combatCharacterDict[charId].AttackForceMissCount = count;
	}

	public void SetAttackForceHit(int charId, sbyte count)
	{
		_combatCharacterDict[charId].AttackForceHitCount = count;
	}

	public void SetSkillAttackForceHit(int charId, bool forceHit)
	{
		_combatCharacterDict[charId].SkillForceHit = forceHit;
	}

	public void SetSkillToMaxRange(DataContext context, int charId, short skillId)
	{
		CombatSkillKey skillKey = new CombatSkillKey(charId, skillId);
		SpecialEffectBase effect = new MaxAttackRange(skillKey);
		DomainManager.SpecialEffect.Add(context, effect);
	}

	public void AddSkillPower(DataContext context, int charId, short skillId, int power)
	{
		AddSkillPowerInCombat(effectKey: new SkillEffectKey(-1, isDirect: false), context: context, skillKey: new CombatSkillKey(charId, skillId), power: power);
	}

	public void AddInjury(DataContext context, int charId, sbyte bodyPart, bool isInner, sbyte count)
	{
		CombatCharacter combatCharacter = _combatCharacterDict[charId];
		AddInjury(context, combatCharacter, bodyPart, isInner, count, updateDefeatMark: true);
		AddToCheckFallenSet(combatCharacter.GetId());
	}

	public void SetDefeatMarkImmunity(int charId, bool outerInjuryImmunity, bool innerInjuryImmunity, bool mindImmunity, bool flawImmunity, bool acupointImmunity)
	{
		CombatCharacter combatCharacter = _combatCharacterDict[charId];
		combatCharacter.OuterInjuryImmunity = outerInjuryImmunity;
		combatCharacter.InnerInjuryImmunity = innerInjuryImmunity;
		combatCharacter.MindImmunity = mindImmunity;
		combatCharacter.FlawImmunity = flawImmunity;
		combatCharacter.AcupointImmunity = acupointImmunity;
	}

	public void AppendEquipAttackSkill(DataContext context, int charId, short skillId)
	{
		if (!DomainManager.CombatSkill.TryGetElement_CombatSkills(new CombatSkillKey(charId, skillId), out var _))
		{
			DomainManager.Character.LearnCombatSkill(context, charId, skillId, 0);
		}
		CombatCharacter combatCharacter = _combatCharacterDict[charId];
		List<short> attackSkillList = combatCharacter.GetAttackSkillList();
		attackSkillList.Add(skillId);
		combatCharacter.SetAttackSkillList(attackSkillList, context);
		AddCombatSkillData(context, charId, skillId);
		UpdateSkillCanUse(context, combatCharacter, skillId);
	}

	public void ForceDefeat(int charId)
	{
		_combatCharacterDict[charId].ForceDefeat = true;
		DomainManager.Combat.AddToCheckFallenSet(charId);
	}

	[DomainMethod]
	public void SelectMercyOption(DataContext context, bool isAlly, bool mercy)
	{
		if (_combatStatus == CombatStatusType.InCombat)
		{
			EShowMercySelect selected = ((!mercy) ? EShowMercySelect.Confirm : EShowMercySelect.Cancel);
			SetSelectedMercyOption(context, selected);
		}
	}

	[DomainMethod]
	public void Surrender(DataContext context, bool isAlly = true)
	{
		if (!CanAcceptCommand())
		{
			return;
		}
		CombatCharacter mainCharacter = GetMainCharacter(isAlly);
		mainCharacter.ForceDefeat = true;
		AddToCheckFallenSet(mainCharacter.GetId());
		UpdateCanSurrender(context, mainCharacter);
		if (!CheckEvaluation(45))
		{
			return;
		}
		AppendEvaluation(45);
		byte b = GlobalConfig.SurrenderInjuryCount[_combatType];
		Injuries injuries = mainCharacter.GetInjuries();
		Injuries oldInjuries = mainCharacter.GetOldInjuries();
		DefeatMarkCollection defeatMarkCollection = mainCharacter.GetDefeatMarkCollection();
		List<int> list = ObjectPool<List<int>>.Instance.Get();
		list.Clear();
		for (int i = 0; i < b; i++)
		{
			bool flag = context.Random.NextBool();
			sbyte b2 = (sbyte)context.Random.Next(7);
			if (injuries.Get(b2, flag) < 6)
			{
				injuries.Change(b2, flag, 1);
				oldInjuries.Change(b2, flag, 1);
				list.Add(new DefeatMarkKey(flag ? EMarkType.Inner : EMarkType.Outer, b2, 1));
			}
			else
			{
				defeatMarkCollection.FatalDamageMarkCount = CMath.ClampFatalMarkCount(defeatMarkCollection.FatalDamageMarkCount + 1);
				list.Add(new DefeatMarkKey(EMarkType.Fatal));
			}
		}
		mainCharacter.SetInjuries(injuries, context);
		mainCharacter.SetOldInjuries(injuries, context);
		UpdateBodyDefeatMark(context, mainCharacter);
		mainCharacter.SetDefeatMarkCollection(defeatMarkCollection, context);
		GameData.GameDataBridge.GameDataBridge.AddDisplayEvent(DisplayEventType.CombatShowSurrenderMark, list);
		ObjectPool<List<int>>.Instance.Return(list);
	}

	[DomainMethod]
	public bool IsInCombat()
	{
		return _combatStatus == CombatStatusType.InCombat;
	}

	public bool CombatAboutToOver()
	{
		return _showMercyOption >= 0 || _selfChar.NeedDelaySettlement || _enemyChar.NeedDelaySettlement || _selfChar.StateMachine.GetCurrentStateType() == CombatCharacterStateType.DelaySettlement || _enemyChar.StateMachine.GetCurrentStateType() == CombatCharacterStateType.DelaySettlement;
	}

	public bool IsWin(bool isAlly)
	{
		return (!isAlly) ? (_combatStatus == CombatStatusType.SelfFail || _combatStatus == CombatStatusType.SelfFlee) : (_combatStatus == CombatStatusType.EnemyFail || _combatStatus == CombatStatusType.EnemyFlee);
	}

	private void UpdateCanSurrender(DataContext context, CombatCharacter character)
	{
		bool flag = IsMainCharacter(character) && !character.ForceDefeat && !_isTutorialCombat;
		if (character.GetCanSurrender() != flag)
		{
			character.SetCanSurrender(flag, context);
		}
		UpdateOtherActionCanUse(context, character, 4);
	}

	public void ShowMercyOption(DataContext context, CombatCharacter winChar)
	{
		CombatCharacter combatCharacter = GetCombatCharacter(!winChar.IsAlly);
		combatCharacter.SetAnimationToPlayOnce("C_011_stun", context);
		winChar.NeedSelectMercyOption = true;
	}

	public void SetShowMercyOption(DataContext context, EShowMercyOption option)
	{
		SetShowMercyOption((sbyte)option, context);
	}

	public void SetSelectedMercyOption(DataContext context, EShowMercySelect selected)
	{
		SetSelectedMercyOption((sbyte)selected, context);
	}

	public void UpdateBodyDefeatMark(DataContext context, CombatCharacter character)
	{
		DefeatMarkCollection defeatMarkCollection = character.GetDefeatMarkCollection();
		Injuries injuries = character.GetInjuries();
		SortedDictionary<sbyte, List<(sbyte, int, int)>> bodyPartDict = character.GetFlawCollection().BodyPartDict;
		SortedDictionary<sbyte, List<(sbyte, int, int)>> bodyPartDict2 = character.GetAcupointCollection().BodyPartDict;
		List<byte> list = ObjectPool<List<byte>>.Instance.Get();
		bool flag = false;
		for (sbyte b = 0; b < 7; b++)
		{
			(sbyte, sbyte) tuple = injuries.Get(b);
			if (defeatMarkCollection.OuterInjuryMarkList[b] != tuple.Item1)
			{
				defeatMarkCollection.OuterInjuryMarkList[b] = (byte)tuple.Item1;
				flag = true;
			}
			if (defeatMarkCollection.InnerInjuryMarkList[b] != tuple.Item2)
			{
				defeatMarkCollection.InnerInjuryMarkList[b] = (byte)tuple.Item2;
				flag = true;
			}
			list.Clear();
			for (int i = 0; i < bodyPartDict[b].Count; i++)
			{
				list.Add((byte)bodyPartDict[b][i].Item1);
			}
			if (!defeatMarkCollection.FlawMarkList[b].SequenceEqual(list))
			{
				defeatMarkCollection.FlawMarkList[b].Clear();
				defeatMarkCollection.FlawMarkList[b].AddRange(list);
				flag = true;
			}
			list.Clear();
			for (int j = 0; j < bodyPartDict2[b].Count; j++)
			{
				list.Add((byte)bodyPartDict2[b][j].Item1);
			}
			if (!defeatMarkCollection.AcupointMarkList[b].SequenceEqual(list))
			{
				defeatMarkCollection.AcupointMarkList[b].Clear();
				defeatMarkCollection.AcupointMarkList[b].AddRange(list);
				flag = true;
			}
		}
		ObjectPool<List<byte>>.Instance.Return(list);
		if (flag)
		{
			character.SetDefeatMarkCollection(defeatMarkCollection, context);
			AddToCheckFallenSet(character.GetId());
			if (IsMainCharacter(character))
			{
				UpdateSkillNeedBodyPartCanUse(context, character);
			}
		}
	}

	public void UpdateBodyDefeatMark(DataContext context, CombatCharacter character, sbyte bodyPart)
	{
		DefeatMarkCollection defeatMarkCollection = character.GetDefeatMarkCollection();
		Injuries injuries = character.GetInjuries();
		bool flag = false;
		if (bodyPart != -1)
		{
			(sbyte, sbyte) tuple = injuries.Get(bodyPart);
			List<(sbyte, int, int)> list = character.GetFlawCollection().BodyPartDict[bodyPart];
			List<(sbyte, int, int)> list2 = character.GetAcupointCollection().BodyPartDict[bodyPart];
			List<byte> list3 = ObjectPool<List<byte>>.Instance.Get();
			if (defeatMarkCollection.OuterInjuryMarkList[bodyPart] != tuple.Item1)
			{
				defeatMarkCollection.OuterInjuryMarkList[bodyPart] = (byte)tuple.Item1;
				flag = true;
			}
			if (defeatMarkCollection.InnerInjuryMarkList[bodyPart] != tuple.Item2)
			{
				defeatMarkCollection.InnerInjuryMarkList[bodyPart] = (byte)tuple.Item2;
				flag = true;
			}
			list3.Clear();
			for (int i = 0; i < list.Count; i++)
			{
				list3.Add((byte)list[i].Item1);
			}
			if (!defeatMarkCollection.FlawMarkList[bodyPart].SequenceEqual(list3))
			{
				defeatMarkCollection.FlawMarkList[bodyPart].Clear();
				defeatMarkCollection.FlawMarkList[bodyPart].AddRange(list3);
				flag = true;
			}
			list3.Clear();
			for (int j = 0; j < list2.Count; j++)
			{
				list3.Add((byte)list2[j].Item1);
			}
			if (!defeatMarkCollection.AcupointMarkList[bodyPart].SequenceEqual(list3))
			{
				defeatMarkCollection.AcupointMarkList[bodyPart].Clear();
				defeatMarkCollection.AcupointMarkList[bodyPart].AddRange(list3);
				flag = true;
			}
			ObjectPool<List<byte>>.Instance.Return(list3);
		}
		if (flag)
		{
			character.SetDefeatMarkCollection(defeatMarkCollection, context);
			if (IsMainCharacter(character))
			{
				UpdateSkillNeedBodyPartCanUse(context, character);
			}
			if (bodyPart == 5 || bodyPart == 6)
			{
				ChangeMobilityValue(context, character, 0);
			}
			AddToCheckFallenSet(character.GetId());
		}
	}

	public unsafe void UpdatePoisonDefeatMark(DataContext context, CombatCharacter character)
	{
		DefeatMarkCollection defeatMarkCollection = character.GetDefeatMarkCollection();
		PoisonInts poison = character.GetPoison();
		bool flag = false;
		for (sbyte b = 0; b < 6; b++)
		{
			sbyte b2 = (sbyte)((!character.GetCharacter().HasPoisonImmunity(b)) ? PoisonsAndLevels.CalcPoisonedLevel(poison.Items[b]) : 0);
			if (b2 != defeatMarkCollection.PoisonMarkList[b])
			{
				defeatMarkCollection.PoisonMarkList[b] = (byte)b2;
				flag = true;
			}
		}
		if (flag)
		{
			AddToCheckFallenSet(character.GetId());
			character.SetDefeatMarkCollection(defeatMarkCollection, context);
		}
	}

	public unsafe int UpdatePoisonDefeatMark(DataContext context, CombatCharacter character, sbyte poisonType)
	{
		DefeatMarkCollection defeatMarkCollection = character.GetDefeatMarkCollection();
		int poisoned = character.GetPoison().Items[poisonType];
		sbyte b = (sbyte)((!character.GetCharacter().HasPoisonImmunity(poisonType)) ? PoisonsAndLevels.CalcPoisonedLevel(poisoned) : 0);
		byte b2 = defeatMarkCollection.PoisonMarkList[poisonType];
		if (b != b2)
		{
			defeatMarkCollection.PoisonMarkList[poisonType] = (byte)b;
			character.SetDefeatMarkCollection(defeatMarkCollection, context);
			AddToCheckFallenSet(character.GetId());
		}
		return b - b2;
	}

	public void UpdateOtherDefeatMark(DataContext context, CombatCharacter character)
	{
		character.GetDefeatMarkCollection().SyncOtherMark(context, character);
	}

	public void AddMindDamage(CombatContext context, int damageValue)
	{
		if (damageValue > 0)
		{
			int mindDamageStep = context.Defender.GetDamageStepCollection().MindDamageStep;
			var (count, mindDamageValue) = CalcInjuryMarkCount(damageValue + context.Defender.GetMindDamageValue(), mindDamageStep);
			context.Defender.SetMindDamageValue(mindDamageValue, context);
			context.Defender.AddMindDamageToShow(context, damageValue);
			AppendMindDefeatMark(context, context.Defender, count, context.SkillTemplateId);
		}
	}

	public void AppendMindDefeatMark(DataContext context, CombatCharacter character, int count, short skillId = -1, bool infinity = false)
	{
		if (character.GetMindImmunity())
		{
			ShowImmunityEffectTips(context, character.GetId(), EMarkType.Mind);
			return;
		}
		count = DomainManager.SpecialEffect.ModifyData(character.GetId(), -1, 249, count);
		if (count <= 0)
		{
			return;
		}
		bool flag = DomainManager.SpecialEffect.ModifyData(character.GetId(), skillId, 289, dataValue: false);
		if (!character.ChangeToMindMark && flag)
		{
			AppendFatalDamageMark(context, character, count, -1, -1);
			return;
		}
		MindMarkList mindMarkTime = character.GetMindMarkTime();
		int total = (infinity ? (-1) : DomainManager.SpecialEffect.ModifyValue(character.GetId(), 178, GlobalConfig.Instance.MindMarkBaseKeepTime));
		for (int i = 0; i < count; i++)
		{
			MindMarkList mindMarkList = mindMarkTime;
			if (mindMarkList.MarkList == null)
			{
				mindMarkList.MarkList = new List<SilenceFrameData>();
			}
			mindMarkTime.MarkList.Add(SilenceFrameData.Create(total));
		}
		character.GetDefeatMarkCollection().SyncMindMark(context, character);
		character.SetMindMarkTime(mindMarkTime, context);
		Events.RaiseAddMindMark(context, character, count);
	}

	public void RemoveHalfMindDefeatMark(DataContext context, CombatCharacter character)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		int count = character.GetDefeatMarkCollection().MindMarkList.Count * CValueHalf.RoundUp;
		DomainManager.Combat.RemoveMindDefeatMark(context, character, count, random: true);
	}

	public void RemoveMindDefeatMark(DataContext context, CombatCharacter character, int count, bool random, int index = 0)
	{
		MindMarkList mindMarkTime = character.GetMindMarkTime();
		int num = Math.Min(count, mindMarkTime.MarkList.Count);
		for (int i = 0; i < num; i++)
		{
			mindMarkTime.MarkList.RemoveAt(random ? context.Random.Next(0, mindMarkTime.MarkList.Count) : index);
		}
		character.GetDefeatMarkCollection().SyncMindMark(context, character);
		character.SetMindMarkTime(mindMarkTime, context);
	}

	public void TransferRandomMindDefeatMark(DataContext context, CombatCharacter srcChar, CombatCharacter dstChar)
	{
		List<bool> mindMarkList = srcChar.GetDefeatMarkCollection().MindMarkList;
		TransferMindDefeatMark(context, srcChar, dstChar, context.Random.Next(mindMarkList.Count));
	}

	public void TransferMindDefeatMark(DataContext context, CombatCharacter srcChar, CombatCharacter dstChar, int index)
	{
		MindMarkList mindMarkTime = srcChar.GetMindMarkTime();
		MindMarkList mindMarkTime2 = dstChar.GetMindMarkTime();
		SilenceFrameData silenceFrameData = mindMarkTime.MarkList[index];
		short mindMarkBaseKeepTime = GlobalConfig.Instance.MindMarkBaseKeepTime;
		mindMarkTime.MarkList.RemoveAt(index);
		MindMarkList mindMarkList = mindMarkTime2;
		if (mindMarkList.MarkList == null)
		{
			mindMarkList.MarkList = new List<SilenceFrameData>();
		}
		mindMarkTime2.MarkList.Add(silenceFrameData.Infinity ? SilenceFrameData.Create(mindMarkBaseKeepTime) : silenceFrameData);
		srcChar.SetMindMarkTime(mindMarkTime, context);
		dstChar.SetMindMarkTime(mindMarkTime2, context);
		srcChar.GetDefeatMarkCollection().SyncMindMark(context, srcChar);
		dstChar.GetDefeatMarkCollection().SyncMindMark(context, dstChar);
	}

	public void AppendDieDefeatMark(DataContext context, CombatCharacter character, CombatSkillKey skillKey, int count)
	{
		if (character.GetCharacter().GetDieImmunity())
		{
			ShowImmunityEffectTips(context, character.GetId(), EMarkType.Die);
			return;
		}
		if (character.ChangeToMindMark)
		{
			AppendMindDefeatMark(context, character, count, -1);
			return;
		}
		DefeatMarkCollection defeatMarkCollection = character.GetDefeatMarkCollection();
		for (int i = 0; i < count; i++)
		{
			defeatMarkCollection.DieMarkList.Add(skillKey);
		}
		character.SetDefeatMarkCollection(defeatMarkCollection, context);
		AddToCheckFallenSet(character.GetId());
	}

	public int AddFatalDamageValue(DataContext context, CombatCharacter combatChar, int damageValue, int type = -1, sbyte bodyPart = -1, short skillId = -1, EDamageType damageType = EDamageType.None)
	{
		damageValue = DomainManager.SpecialEffect.ModifyValueCustom(combatChar.GetId(), skillId, 191, damageValue, type, (int)damageType);
		damageValue = DomainManager.SpecialEffect.ModifyData(combatChar.GetId(), skillId, 294, damageValue);
		if (damageValue <= 0)
		{
			return 0;
		}
		int fatalDamageValue = combatChar.GetFatalDamageValue();
		int fatalDamageStep = combatChar.GetDamageStepCollection().FatalDamageStep;
		(int, int) tuple = CalcInjuryMarkCount(damageValue + fatalDamageValue, fatalDamageStep);
		combatChar.SetFatalDamageValue(tuple.Item2, context);
		int num = tuple.Item1 * fatalDamageStep + tuple.Item2 - fatalDamageValue;
		if (num > 0)
		{
			combatChar.AddFatalDamageToShow(context, num);
		}
		if (tuple.Item1 > 0)
		{
			return AppendFatalDamageMark(context, combatChar, tuple.Item1, type, bodyPart, addByValue: true, damageType);
		}
		return 0;
	}

	public int AppendFatalDamageMark(DataContext context, CombatCharacter character, int count, int type = -1, sbyte bodyPart = -1, bool addByValue = false, EDamageType damageType = EDamageType.None)
	{
		if (character.GetCharacter().GetFatalImmunity())
		{
			ShowImmunityEffectTips(context, character.GetId(), EMarkType.Fatal);
			return 0;
		}
		count = DomainManager.SpecialEffect.ModifyData(character.GetId(), -1, 192, count, type, bodyPart, addByValue ? 1 : 0);
		count = DomainManager.SpecialEffect.ModifyData(character.GetId(), -1, 304, count, (int)damageType);
		if (count <= 0)
		{
			return 0;
		}
		if (character.ChangeToMindMark)
		{
			AppendMindDefeatMark(context, character, count, -1);
			return 0;
		}
		AppendFatalDamageMarkImmediate(context, character, count);
		return count;
	}

	public void AppendFatalDamageMarkImmediate(DataContext context, CombatCharacter character, int count)
	{
		DefeatMarkCollection defeatMarkCollection = character.GetDefeatMarkCollection();
		defeatMarkCollection.FatalDamageMarkCount = CMath.ClampFatalMarkCount(defeatMarkCollection.FatalDamageMarkCount + count);
		character.SetDefeatMarkCollection(defeatMarkCollection, context);
		Events.RaiseAddFatalDamageMark(context, character, count);
		AddToCheckFallenSet(character.GetId());
	}

	public void RemoveHalfFatalDamageMark(DataContext context, CombatCharacter character)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		int count = character.GetDefeatMarkCollection().FatalDamageMarkCount * CValueHalf.RoundUp;
		DomainManager.Combat.RemoveFatalDamageMark(context, character, count);
	}

	public void RemoveFatalDamageMark(DataContext context, CombatCharacter character, int count)
	{
		DefeatMarkCollection defeatMarkCollection = character.GetDefeatMarkCollection();
		defeatMarkCollection.FatalDamageMarkCount = CMath.ClampFatalMarkCount(defeatMarkCollection.FatalDamageMarkCount - count);
		character.SetDefeatMarkCollection(defeatMarkCollection, context);
	}

	public void TransferFatalDamageMark(DataContext context, CombatCharacter srcChar, CombatCharacter destChar, int count)
	{
		DefeatMarkCollection defeatMarkCollection = srcChar.GetDefeatMarkCollection();
		DefeatMarkCollection defeatMarkCollection2 = destChar.GetDefeatMarkCollection();
		count = Math.Min(count, defeatMarkCollection.FatalDamageMarkCount);
		defeatMarkCollection.FatalDamageMarkCount = CMath.ClampFatalMarkCount(defeatMarkCollection.FatalDamageMarkCount - count);
		defeatMarkCollection2.FatalDamageMarkCount = CMath.ClampFatalMarkCount(defeatMarkCollection2.FatalDamageMarkCount + count);
		srcChar.SetDefeatMarkCollection(defeatMarkCollection, context);
		destChar.SetDefeatMarkCollection(defeatMarkCollection2, context);
	}

	public void AddToCheckFallenSet(int charId)
	{
		_needCheckFallenCharSet.Add(charId);
	}

	private bool CheckCurrCharDangerOrFallen(DataContext context, CombatCharacter character)
	{
		if (!IsInCombat() || CombatAboutToOver() || (!IsCharacterFallen(character) && !DefeatMarkReachFailCount(character)))
		{
			return false;
		}
		if (EnemyUnyieldingFallen && !character.IsAlly)
		{
			GmCmd_HealAllDefeatMark(context, character.IsAlly);
		}
		for (int i = 0; i < 4; i++)
		{
			RaiseCombatCharAboutToFall(context, character, i);
			if (!IsCharacterFallen(character) && !DefeatMarkReachFailCount(character))
			{
				_saveDyingEffectTriggerd = true;
				return false;
			}
		}
		if (!IsCharacterFallen(character))
		{
			return false;
		}
		Events.RaiseCombatCharFallen(context, character);
		CombatCharacter combatCharacter = GetCombatCharacter(character.IsAlly);
		bool flag = combatCharacter == character;
		if (IsMainCharacter(character))
		{
			CombatCharacter combatCharacter2 = GetCombatCharacter(!character.IsAlly);
			combatCharacter.ClearAllDoingOrReserveCommand(context);
			combatCharacter2.ClearAllDoingOrReserveCommand(context);
			if (combatCharacter.NeedShowChangeTrick && combatCharacter.IsAlly)
			{
				CancelChangeTrick(context, combatCharacter.IsAlly);
			}
			if (combatCharacter2.NeedShowChangeTrick && combatCharacter2.IsAlly)
			{
				CancelChangeTrick(context, combatCharacter2.IsAlly);
			}
			DomainManager.Combat.ForceAllTeammateLeaveCombatField(context, isAlly: true);
			DomainManager.Combat.ForceAllTeammateLeaveCombatField(context, isAlly: false);
			combatCharacter.ChangeCharId = (flag ? (-1) : (character.IsAlly ? _selfTeam : _enemyTeam)[0]);
			combatCharacter2.ChangeCharId = (IsMainCharacter(combatCharacter2) ? (-1) : ((!character.IsAlly) ? _selfTeam : _enemyTeam)[0]);
			if (character.ChangeCharId == -1 && combatCharacter2.ChangeCharId == -1)
			{
				EndCombat(context, character);
			}
			else if (character.BossConfig == null && character.AnimalConfig == null)
			{
				character.SetAnimationToPlayOnce(NeedShowMercy(character) ? "C_011_stun" : FailAni[_combatType], context);
			}
		}
		else if (flag)
		{
			character.ChangeCharId = (character.IsAlly ? _selfTeam : _enemyTeam)[0];
			character.ClearAllDoingOrReserveCommand(context);
			if (character.NeedShowChangeTrick && character.IsAlly)
			{
				CancelChangeTrick(context, character.IsAlly);
			}
			if (NeedShowMercy(character))
			{
				ShowMercyOption(context, GetCombatCharacter(!character.IsAlly));
			}
			else
			{
				character.ChangeCharFailAni = "C_005";
			}
		}
		else
		{
			character.ClearTeammateCommand(context, interrupt: true);
		}
		return true;
	}

	public bool IsCharacterFallen(CombatCharacter character)
	{
		if (character.Immortal)
		{
			return false;
		}
		if (character.ForceDefeat)
		{
			return true;
		}
		DefeatMarkCollection defeatMarkCollection = character.GetDefeatMarkCollection();
		return (DefeatMarkReachFailCount(character) && !character.UnyieldingFallen) || defeatMarkCollection.DieMarkList.Count >= SharedConstValue.DefeatNeedDieMarkCount;
	}

	public bool DefeatMarkReachFailCount(CombatCharacter character)
	{
		return character.GetDefeatMarkCollection().GetTotalCount() >= GlobalConfig.NeedDefeatMarkCount[_combatType];
	}

	public bool IsCharacterHalfFallen(CombatCharacter character)
	{
		return character.GetDefeatMarkCollection().GetTotalCount() > GlobalConfig.NeedDefeatMarkCount[_combatType] / 2;
	}

	public (string, string, string) GetFailAnimationAndSound(DataContext context, CombatCharacter attacker, bool flee = false, bool kill = false)
	{
		if (flee)
		{
			return ("M_004", "", "");
		}
		if (!kill)
		{
			CombatCharacter combatCharacter = GetCombatCharacter(!attacker.IsAlly);
			return (combatCharacter.BossConfig != null) ? (combatCharacter.BossConfig.FailAnimation, combatCharacter.BossConfig.FailParticles[(CombatConfig.Scene < 0) ? 1 : combatCharacter.GetBossPhase()], combatCharacter.BossConfig.FailSounds[(CombatConfig.Scene < 0) ? 1 : combatCharacter.GetBossPhase()]) : ((combatCharacter.AnimalConfig != null) ? (FailAni[2], combatCharacter.AnimalConfig.FailParticle, combatCharacter.AnimalConfig.FailSound) : (FailAni[_combatType], "", ""));
		}
		GameData.Domains.Item.Weapon element_Weapons = DomainManager.Item.GetElement_Weapons(GetUsingWeaponKey(attacker).Id);
		sbyte index = attacker.GetWeaponTricks()[attacker.GetWeaponTrickIndex()];
		TrickTypeItem trickTypeItem = Config.TrickType.Instance[index];
		int weaponAction = element_Weapons.GetWeaponAction();
		List<string> list = trickTypeItem.ExecuteAni[weaponAction];
		List<string> list2 = trickTypeItem.ExecuteParticle[weaponAction];
		List<string> list3 = trickTypeItem.ExecuteSound[weaponAction];
		int index2 = context.Random.Next(list.Count);
		return (list[index2], list2[index2], list3[index2]);
	}

	public void ClearBurstBodyPartFlawAndAcupoint(DataContext context, CombatCharacter combatChar, string aniName)
	{
		if (aniName.Contains("burst"))
		{
			sbyte b = (sbyte)(aniName.Contains("head") ? 2 : (aniName.Contains("arm") ? 4 : (aniName.Contains("leg") ? 5 : (-1))));
			if (b >= 0)
			{
				byte[] flawCount = combatChar.GetFlawCount();
				FlawOrAcupointCollection flawCollection = combatChar.GetFlawCollection();
				byte[] acupointCount = combatChar.GetAcupointCount();
				FlawOrAcupointCollection acupointCollection = combatChar.GetAcupointCollection();
				flawCount[b] = 0;
				flawCollection.BodyPartDict[b].Clear();
				acupointCount[b] = 0;
				acupointCollection.BodyPartDict[b].Clear();
				combatChar.SetFlawCount(flawCount, context);
				combatChar.SetFlawCollection(flawCollection, context);
				combatChar.SetAcupointCount(acupointCount, context);
				combatChar.SetAcupointCollection(acupointCollection, context);
			}
		}
	}

	public bool NeedShowMercy(CombatCharacter failChar)
	{
		if (!IsMainCharacter(failChar))
		{
			return false;
		}
		CombatCharacter combatCharacter = GetCombatCharacter(!failChar.IsAlly);
		CombatCharacterStateType currentStateType = combatCharacter.StateMachine.GetCurrentStateType();
		if (currentStateType == CombatCharacterStateType.SelectMercy)
		{
			return false;
		}
		if (IsInfectedCombat() && !failChar.IsAlly)
		{
			return true;
		}
		bool flag = DomainManager.World.GetAllowExecute() && _combatType == 2 && CombatConfig.AllowShowMercy && failChar.GetDefeatMarkCollection().GetTotalCount() > GlobalConfig.NeedDefeatMarkCount[2];
		bool flag2 = flag;
		if (flag2)
		{
			bool flag3 = ((currentStateType == CombatCharacterStateType.Idle || currentStateType == CombatCharacterStateType.Attack || currentStateType == CombatCharacterStateType.CastSkill) ? true : false);
			flag2 = flag3;
		}
		return flag2 && combatCharacter.IsActorSkeleton && failChar.IsActorSkeleton && failChar.GetCharacter().GetAgeGroup() == 2 && failChar.GetId() != DomainManager.Character.GetAvoidDeathCharId() && !_isTutorialCombat && Context.Random.CheckPercentProb(50);
	}

	public void EndCombat(DataContext context, CombatCharacter failChar, bool flee = false, bool playAni = true)
	{
		ClearCombatResultLegacies();
		CombatCharacter combatCharacter = GetCombatCharacter(!failChar.IsAlly);
		failChar.ClearAllDoingOrReserveCommand(context);
		if (!flee && NeedShowMercy(failChar))
		{
			ShowMercyOption(context, combatCharacter);
		}
		else
		{
			if (playAni)
			{
				if (!GetIsPuppetCombat())
				{
					(string, string, string) failAnimationAndSound = GetFailAnimationAndSound(context, combatCharacter, flee);
					if ((failChar.GetAnimationToPlayOnce() != failAnimationAndSound.Item1 || failChar.BossConfig != null) && (!flee || failChar.AnimalConfig == null))
					{
						failChar.SetAnimationToPlayOnce(failAnimationAndSound.Item1, context);
					}
					if (failAnimationAndSound.Item2 != "" && failChar.GetParticleToPlay() != failAnimationAndSound.Item2)
					{
						failChar.SetParticleToPlay(failAnimationAndSound.Item2, context);
					}
					if (failAnimationAndSound.Item3 != "" && failChar.GetDieSoundToPlay() != failAnimationAndSound.Item3)
					{
						failChar.SetDieSoundToPlay(failAnimationAndSound.Item3, context);
					}
					BossItem bossConfig = failChar.BossConfig;
					if (bossConfig != null)
					{
						string[] failPetParticles = bossConfig.FailPetParticles;
						if (failPetParticles != null && failPetParticles.Length > 0 && !string.IsNullOrEmpty(failChar.BossConfig.FailPetParticles[0]))
						{
							failChar.SetPetParticle(failChar.BossConfig.FailPetParticles[1], context);
						}
					}
				}
				else
				{
					failChar.SetAnimationToLoop("C_000", context);
					failChar.SetAnimationToPlayOnce(null, context);
					failChar.SetParticleToPlay(null, context);
					failChar.SetDieSoundToPlay(null, context);
					BossItem bossConfig = failChar.BossConfig;
					if (bossConfig != null)
					{
						string[] failPetParticles = bossConfig.FailPetParticles;
						if (failPetParticles != null && failPetParticles.Length > 0)
						{
							failChar.SetPetParticle(null, context);
						}
					}
				}
				combatCharacter.PlayWinAnimation(context);
			}
			if (!flee && playAni)
			{
				SetWaitingDelaySettlement(value: true, context);
				combatCharacter.NeedDelaySettlement = true;
				combatCharacter.StateMachine.TranslateState();
			}
			else
			{
				CombatSettlement(context, (!flee) ? (failChar.IsAlly ? CombatStatusType.SelfFail : CombatStatusType.EnemyFail) : (failChar.IsAlly ? CombatStatusType.SelfFlee : CombatStatusType.EnemyFlee));
			}
		}
		if ((_combatType == 1 || _combatType == 2) && combatCharacter.IsAlly)
		{
			for (int i = 0; i < _enemyTeam.Length; i++)
			{
				int num = _enemyTeam[i];
				if (num >= 0)
				{
					GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(num);
					if (element_Objects.GetCreatingType() == 1)
					{
						DomainManager.Character.LoseGuard(context, num, element_Objects);
					}
				}
			}
		}
		if ((36 > CombatConfig.TemplateId || CombatConfig.TemplateId > 44) && (54 > CombatConfig.TemplateId || CombatConfig.TemplateId > 62))
		{
			return;
		}
		if (failChar.BossConfig != null)
		{
			sbyte xiangshuAvatarIdByCharacterTemplateId = XiangshuAvatarIds.GetXiangshuAvatarIdByCharacterTemplateId(failChar.GetCharacter().GetTemplateId());
			if (xiangshuAvatarIdByCharacterTemplateId >= 0)
			{
				DomainManager.World.SetSwordTombStatus(context, xiangshuAvatarIdByCharacterTemplateId, 2);
			}
		}
		else if (combatCharacter.BossConfig != null && combatCharacter.GetBossPhase() > 0)
		{
			sbyte xiangshuAvatarIdByCharacterTemplateId2 = XiangshuAvatarIds.GetXiangshuAvatarIdByCharacterTemplateId(combatCharacter.GetCharacter().GetTemplateId());
			if (xiangshuAvatarIdByCharacterTemplateId2 >= 0)
			{
				DomainManager.World.SetSwordTombStatus(context, xiangshuAvatarIdByCharacterTemplateId2, 1);
			}
		}
	}

	public void Flee(DataContext context, CombatCharacter character)
	{
		EndCombat(context, character, flee: true);
	}

	public void CombatSettlement(DataContext context, sbyte statusType)
	{
		Started = false;
		_combatResultData.CombatStatus = statusType;
		Events.RaiseCombatSettlement(context, statusType);
		SetCombatStatus(statusType, context);
		CalcEvaluationList(context);
		CalcReadInCombat(context);
		CalcQiQrtInCombat(context);
		CalcAddLegacyPoint(context);
		CalcAndAddFameAction(context);
		CalcAndAddAreaSpiritualDebt(context);
		foreach (CombatCharacter value in _combatCharacterDict.Values)
		{
			value.RevertAllRawCreates(context);
		}
		CalcAndAddExp(context);
		CalcAndAddResource(context);
		CalcAndAddProficiency(context);
		CalcLootItem(context);
		GetLootCharDisplayData();
		_selfChar.OnFrameBegin();
		_selfChar.OnFrameEnd();
		_enemyChar.OnFrameBegin();
		_enemyChar.OnFrameEnd();
		DomainManager.SpecialEffect.RemoveAllEffectsInCombat(context);
		ClearSkillPowerAddInCombat(context);
		ClearSkillPowerReduceInCombat(context);
		ClearSkillPowerReplaceInCombat(context);
		EquipmentPowerChangeInCombat.Clear();
		foreach (CombatCharacter value2 in _combatCharacterDict.Values)
		{
			value2.OnCombatEnd(context);
		}
		CombatCharacter combatCharacter = (IsWin(isAlly: true) ? _enemyChar : _selfChar);
		if (combatCharacter.GetId() == DomainManager.Character.GetAvoidDeathCharId())
		{
			DomainManager.Character.TransferAvoidDeathCharInjuries(context);
		}
		if (_isPuppetCombat)
		{
			DomainManager.Character.RemoveNonIntelligentCharacter(context, _enemyChar.GetCharacter());
			SetIsPuppetCombat(value: false, context);
			SetIsPlaygroundCombat(value: false, context);
		}
		if (_carrierAnimalCombatCharId >= 0)
		{
			DomainManager.Character.RemoveNonIntelligentCharacter(context, _combatCharacterDict[_carrierAnimalCombatCharId].GetCharacter());
		}
		if (_specialShowCombatCharId >= 0)
		{
			DomainManager.Character.RemoveNonIntelligentCharacter(context, _combatCharacterDict[_specialShowCombatCharId].GetCharacter());
		}
		ClearSpecialGroup(context);
		foreach (CombatCharacter value3 in _combatCharacterDict.Values)
		{
			if (value3.GetCharacter().GetAllowUseFreeWeapon())
			{
				value3.RemoveTempWeapons(context);
			}
		}
		List<int> list = ObjectPool<List<int>>.Instance.Get();
		list.Clear();
		list.AddRange(_combatCharacterDict.Keys);
		foreach (int item in list)
		{
			RemoveElement_CombatCharacterDict(item);
		}
		ObjectPool<List<int>>.Instance.Return(list);
		CalcSnapshotAfterCombat(context);
		Events.RaiseCombatEnd(context);
	}

	public static int GetDefeatMarksCountOutOfCombat(GameData.Domains.Character.Character character)
	{
		int num = 0;
		num += character.GetInjuries().GetSum();
		num += character.GetPoisonMarkCount();
		num += character.GetEatingItems().CountOfWugMark();
		num += DefeatMarkCollection.CalcQiDisorderMarkCount(character.GetDisorderOfQi());
		return num + DefeatMarkCollection.GetHealthMarkCount(character);
	}

	[DomainMethod]
	public static DefeatMarksCountOutOfCombatData GetDefeatMarksCountOutOfCombat(DataContext context, int charId)
	{
		DefeatMarksCountOutOfCombatData defeatMarksCountOutOfCombatData = new DefeatMarksCountOutOfCombatData();
		GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(charId);
		int sum = element_Objects.GetInjuries().GetSum();
		int poisonMarkCount = element_Objects.GetPoisonMarkCount();
		sbyte b = element_Objects.GetEatingItems().CountOfWugMark();
		sbyte b2 = DefeatMarkCollection.CalcQiDisorderMarkCount(element_Objects.GetDisorderOfQi());
		sbyte healthMarkCount = DefeatMarkCollection.GetHealthMarkCount(element_Objects);
		if (sum > 0)
		{
			defeatMarksCountOutOfCombatData.DefeatMarksDict.Add(0, sum);
		}
		if (poisonMarkCount > 0)
		{
			defeatMarksCountOutOfCombatData.DefeatMarksDict.Add(2, poisonMarkCount);
		}
		if (b > 0)
		{
			defeatMarksCountOutOfCombatData.DefeatMarksDict.Add(5, b);
		}
		if (b2 > 0)
		{
			defeatMarksCountOutOfCombatData.DefeatMarksDict.Add(6, b2);
		}
		if (healthMarkCount > 0)
		{
			defeatMarksCountOutOfCombatData.DefeatMarksDict.Add(9, healthMarkCount);
		}
		return defeatMarksCountOutOfCombatData;
	}

	public bool AddWug(DataContext context, CombatCharacter affectChar, sbyte wugType, bool isBad, int srcCharId)
	{
		short wugTemplateId = ItemDomain.GetWugTemplateId(wugType, (sbyte)(isBad ? 2 : 0));
		return AddWug(context, affectChar, wugTemplateId, srcCharId, EWugReplaceType.CombatOnly);
	}

	public bool AddWug(DataContext context, CombatCharacter affectChar, short wugTemplateId, int srcCharId, EWugReplaceType replaceType = EWugReplaceType.None)
	{
		if (!DomainManager.SpecialEffect.ModifyData(affectChar.GetId(), -1, 180, dataValue: true, srcCharId))
		{
			return false;
		}
		MedicineItem wugConfig = Config.Medicine.Instance[wugTemplateId];
		EatingItems eatingItems = affectChar.GetCharacter().GetEatingItems();
		int num = eatingItems.IndexOfWug(wugConfig);
		if (num >= 0 && !replaceType.IsMatchWug(eatingItems.Get(num).TemplateId, wugTemplateId))
		{
			return false;
		}
		affectChar.GetCharacter().AddWug(context, wugTemplateId);
		return true;
	}

	public void AddWugIrresistibly(DataContext context, CombatCharacter affectChar, ItemKey wugItemKey)
	{
		affectChar.GetCharacter().AddEatingItem(context, wugItemKey);
	}

	public short RemoveRandomWug(DataContext context, CombatCharacter combatChar, EWugReplaceType replaceType = EWugReplaceType.CombatOnly)
	{
		List<short> list = ObjectPool<List<short>>.Instance.Get();
		list.Clear();
		EatingItems eatingItems = combatChar.GetCharacter().GetEatingItems();
		for (sbyte b = 0; b < 8; b++)
		{
			int num = eatingItems.IndexOfWug(b);
			if (num >= 0)
			{
				short templateId = eatingItems.Get(num).TemplateId;
				if (replaceType.IsMatchWug(templateId, -1))
				{
					list.Add(templateId);
				}
			}
		}
		short num2 = (short)((list.Count > 0) ? list.GetRandom(context.Random) : (-1));
		ObjectPool<List<short>>.Instance.Return(list);
		if (num2 >= 0)
		{
			combatChar.GetCharacter().RemoveWug(context, num2);
		}
		return num2;
	}

	public CombatDomain()
		: base(33)
	{
		_timeScale = 0f;
		_autoCombat = false;
		_combatFrame = 0uL;
		_combatType = 0;
		_currentDistance = 0;
		_damageCompareData = new DamageCompareData();
		_skillPowerAddInCombat = new Dictionary<CombatSkillKey, SkillPowerChangeCollection>(0);
		_skillPowerReduceInCombat = new Dictionary<CombatSkillKey, SkillPowerChangeCollection>(0);
		_skillPowerReplaceInCombat = new Dictionary<CombatSkillKey, CombatSkillKey>(0);
		_bgmIndex = 0;
		_combatCharacterDict = new Dictionary<int, CombatCharacter>(0);
		_selfTeam = new int[4];
		_selfCharId = 0;
		_selfTeamWisdomType = 0;
		_selfTeamWisdomCount = 0;
		_enemyTeam = new int[4];
		_enemyCharId = 0;
		_enemyTeamWisdomType = 0;
		_enemyTeamWisdomCount = 0;
		_combatStatus = 0;
		_showMercyOption = 0;
		_selectedMercyOption = 0;
		_carrierAnimalCombatCharId = 0;
		_specialShowCombatCharId = 0;
		_skillAttackedIndexAndHit = default(IntPair);
		_waitingDelaySettlement = false;
		_showUseGoldenWire = default(SpecialMiscData);
		_isPuppetCombat = false;
		_isPlaygroundCombat = false;
		_skillDataDict = new Dictionary<CombatSkillKey, CombatSkillData>(0);
		_weaponDataDict = new Dictionary<int, CombatWeaponData>(0);
		_expectRatioData = new WeaponExpectInnerRatioData();
		_taiwuSpecialGroupCharIds = new List<int>();
		HelperDataCombatCharacterDict = new ObjectCollectionHelperData(8, 10, CacheInfluencesCombatCharacterDict, _dataStatesCombatCharacterDict, isArchive: false);
		HelperDataSkillDataDict = new ObjectCollectionHelperData(8, 29, CacheInfluencesSkillDataDict, _dataStatesSkillDataDict, isArchive: false);
		HelperDataWeaponDataDict = new ObjectCollectionHelperData(8, 30, CacheInfluencesWeaponDataDict, _dataStatesWeaponDataDict, isArchive: false);
		OnInitializedDomainData();
	}

	public float GetTimeScale()
	{
		return _timeScale;
	}

	private void SetTimeScale(float value, DataContext context)
	{
		_timeScale = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(0, DataStates, CacheInfluences, context);
	}

	public bool GetAutoCombat()
	{
		return _autoCombat;
	}

	private void SetAutoCombat(bool value, DataContext context)
	{
		_autoCombat = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(1, DataStates, CacheInfluences, context);
	}

	public ulong GetCombatFrame()
	{
		return _combatFrame;
	}

	public void SetCombatFrame(ulong value, DataContext context)
	{
		_combatFrame = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(2, DataStates, CacheInfluences, context);
	}

	public sbyte GetCombatType()
	{
		return _combatType;
	}

	public void SetCombatType(sbyte value, DataContext context)
	{
		_combatType = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(3, DataStates, CacheInfluences, context);
	}

	public short GetCurrentDistance()
	{
		return _currentDistance;
	}

	public void SetCurrentDistance(short value, DataContext context)
	{
		_currentDistance = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(4, DataStates, CacheInfluences, context);
	}

	public DamageCompareData GetDamageCompareData()
	{
		return _damageCompareData;
	}

	public void SetDamageCompareData(DamageCompareData value, DataContext context)
	{
		_damageCompareData = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(5, DataStates, CacheInfluences, context);
	}

	public SkillPowerChangeCollection GetElement_SkillPowerAddInCombat(CombatSkillKey elementId)
	{
		return _skillPowerAddInCombat[elementId];
	}

	public bool TryGetElement_SkillPowerAddInCombat(CombatSkillKey elementId, out SkillPowerChangeCollection value)
	{
		return _skillPowerAddInCombat.TryGetValue(elementId, out value);
	}

	private void AddElement_SkillPowerAddInCombat(CombatSkillKey elementId, SkillPowerChangeCollection value, DataContext context)
	{
		_skillPowerAddInCombat.Add(elementId, value);
		_modificationsSkillPowerAddInCombat.RecordAdding(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(6, DataStates, CacheInfluences, context);
	}

	private void SetElement_SkillPowerAddInCombat(CombatSkillKey elementId, SkillPowerChangeCollection value, DataContext context)
	{
		_skillPowerAddInCombat[elementId] = value;
		_modificationsSkillPowerAddInCombat.RecordSetting(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(6, DataStates, CacheInfluences, context);
	}

	private void RemoveElement_SkillPowerAddInCombat(CombatSkillKey elementId, DataContext context)
	{
		_skillPowerAddInCombat.Remove(elementId);
		_modificationsSkillPowerAddInCombat.RecordRemoving(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(6, DataStates, CacheInfluences, context);
	}

	private void ClearSkillPowerAddInCombat(DataContext context)
	{
		_skillPowerAddInCombat.Clear();
		_modificationsSkillPowerAddInCombat.RecordClearing();
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(6, DataStates, CacheInfluences, context);
	}

	public SkillPowerChangeCollection GetElement_SkillPowerReduceInCombat(CombatSkillKey elementId)
	{
		return _skillPowerReduceInCombat[elementId];
	}

	public bool TryGetElement_SkillPowerReduceInCombat(CombatSkillKey elementId, out SkillPowerChangeCollection value)
	{
		return _skillPowerReduceInCombat.TryGetValue(elementId, out value);
	}

	private void AddElement_SkillPowerReduceInCombat(CombatSkillKey elementId, SkillPowerChangeCollection value, DataContext context)
	{
		_skillPowerReduceInCombat.Add(elementId, value);
		_modificationsSkillPowerReduceInCombat.RecordAdding(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(7, DataStates, CacheInfluences, context);
	}

	private void SetElement_SkillPowerReduceInCombat(CombatSkillKey elementId, SkillPowerChangeCollection value, DataContext context)
	{
		_skillPowerReduceInCombat[elementId] = value;
		_modificationsSkillPowerReduceInCombat.RecordSetting(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(7, DataStates, CacheInfluences, context);
	}

	private void RemoveElement_SkillPowerReduceInCombat(CombatSkillKey elementId, DataContext context)
	{
		_skillPowerReduceInCombat.Remove(elementId);
		_modificationsSkillPowerReduceInCombat.RecordRemoving(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(7, DataStates, CacheInfluences, context);
	}

	private void ClearSkillPowerReduceInCombat(DataContext context)
	{
		_skillPowerReduceInCombat.Clear();
		_modificationsSkillPowerReduceInCombat.RecordClearing();
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(7, DataStates, CacheInfluences, context);
	}

	public CombatSkillKey GetElement_SkillPowerReplaceInCombat(CombatSkillKey elementId)
	{
		return _skillPowerReplaceInCombat[elementId];
	}

	public bool TryGetElement_SkillPowerReplaceInCombat(CombatSkillKey elementId, out CombatSkillKey value)
	{
		return _skillPowerReplaceInCombat.TryGetValue(elementId, out value);
	}

	private void AddElement_SkillPowerReplaceInCombat(CombatSkillKey elementId, CombatSkillKey value, DataContext context)
	{
		_skillPowerReplaceInCombat.Add(elementId, value);
		_modificationsSkillPowerReplaceInCombat.RecordAdding(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(8, DataStates, CacheInfluences, context);
	}

	private void SetElement_SkillPowerReplaceInCombat(CombatSkillKey elementId, CombatSkillKey value, DataContext context)
	{
		_skillPowerReplaceInCombat[elementId] = value;
		_modificationsSkillPowerReplaceInCombat.RecordSetting(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(8, DataStates, CacheInfluences, context);
	}

	private void RemoveElement_SkillPowerReplaceInCombat(CombatSkillKey elementId, DataContext context)
	{
		_skillPowerReplaceInCombat.Remove(elementId);
		_modificationsSkillPowerReplaceInCombat.RecordRemoving(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(8, DataStates, CacheInfluences, context);
	}

	private void ClearSkillPowerReplaceInCombat(DataContext context)
	{
		_skillPowerReplaceInCombat.Clear();
		_modificationsSkillPowerReplaceInCombat.RecordClearing();
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(8, DataStates, CacheInfluences, context);
	}

	public sbyte GetBgmIndex()
	{
		return _bgmIndex;
	}

	public void SetBgmIndex(sbyte value, DataContext context)
	{
		_bgmIndex = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(9, DataStates, CacheInfluences, context);
	}

	public CombatCharacter GetElement_CombatCharacterDict(int objectId)
	{
		return _combatCharacterDict[objectId];
	}

	public bool TryGetElement_CombatCharacterDict(int objectId, out CombatCharacter element)
	{
		return _combatCharacterDict.TryGetValue(objectId, out element);
	}

	private void AddElement_CombatCharacterDict(int objectId, CombatCharacter instance)
	{
		instance.CollectionHelperData = HelperDataCombatCharacterDict;
		instance.DataStatesOffset = _dataStatesCombatCharacterDict.Create();
		_combatCharacterDict.Add(objectId, instance);
	}

	private void RemoveElement_CombatCharacterDict(int objectId)
	{
		if (_combatCharacterDict.TryGetValue(objectId, out var value))
		{
			_dataStatesCombatCharacterDict.Remove(value.DataStatesOffset);
			_combatCharacterDict.Remove(objectId);
		}
	}

	private void ClearCombatCharacterDict()
	{
		_dataStatesCombatCharacterDict.Clear();
		_combatCharacterDict.Clear();
	}

	public int GetElementField_CombatCharacterDict(int objectId, ushort fieldId, RawDataPool dataPool, bool resetModified)
	{
		if (!_combatCharacterDict.TryGetValue(objectId, out var value))
		{
			AdaptableLog.TagWarning("GetElementField_CombatCharacterDict", $"Failed to find element {objectId} with field {fieldId}");
			return -1;
		}
		if (resetModified)
		{
			_dataStatesCombatCharacterDict.ResetModified(value.DataStatesOffset, fieldId);
		}
		switch (fieldId)
		{
		case 0:
			return GameData.Serializer.Serializer.Serialize(value.GetId(), dataPool);
		case 1:
			return GameData.Serializer.Serializer.Serialize(value.GetBreathValue(), dataPool);
		case 2:
			return GameData.Serializer.Serializer.Serialize(value.GetStanceValue(), dataPool);
		case 3:
			return GameData.Serializer.Serializer.Serialize(value.GetNeiliAllocation(), dataPool);
		case 4:
			return GameData.Serializer.Serializer.Serialize(value.GetOriginNeiliAllocation(), dataPool);
		case 5:
			return GameData.Serializer.Serializer.Serialize(value.GetNeiliAllocationRecoverProgress(), dataPool);
		case 6:
			return GameData.Serializer.Serializer.Serialize(value.GetOldDisorderOfQi(), dataPool);
		case 7:
			return GameData.Serializer.Serializer.Serialize(value.GetNeiliType(), dataPool);
		case 8:
			return GameData.Serializer.Serializer.Serialize(value.GetAvoidToShow(), dataPool);
		case 9:
			return GameData.Serializer.Serializer.Serialize(value.GetCurrentPosition(), dataPool);
		case 10:
			return GameData.Serializer.Serializer.Serialize(value.GetDisplayPosition(), dataPool);
		case 11:
			return GameData.Serializer.Serializer.Serialize(value.GetMobilityValue(), dataPool);
		case 12:
			return GameData.Serializer.Serializer.Serialize(value.GetJumpPrepareProgress(), dataPool);
		case 13:
			return GameData.Serializer.Serializer.Serialize(value.GetJumpPreparedDistance(), dataPool);
		case 14:
			return GameData.Serializer.Serializer.Serialize(value.GetMobilityLockEffectCount(), dataPool);
		case 15:
			return GameData.Serializer.Serializer.Serialize(value.GetJumpChangeDistanceDuration(), dataPool);
		case 16:
			return GameData.Serializer.Serializer.Serialize(value.GetUsingWeaponIndex(), dataPool);
		case 17:
			return GameData.Serializer.Serializer.Serialize(value.GetWeaponTricks(), dataPool);
		case 18:
			return GameData.Serializer.Serializer.Serialize(value.GetWeaponTrickIndex(), dataPool);
		case 19:
			return GameData.Serializer.Serializer.Serialize(value.GetWeapons(), dataPool);
		case 20:
			return GameData.Serializer.Serializer.Serialize(value.GetAttackingTrickType(), dataPool);
		case 21:
			return GameData.Serializer.Serializer.Serialize(value.GetCanAttackOutRange(), dataPool);
		case 22:
			return GameData.Serializer.Serializer.Serialize(value.GetChangeTrickProgress(), dataPool);
		case 23:
			return GameData.Serializer.Serializer.Serialize(value.GetChangeTrickCount(), dataPool);
		case 24:
			return GameData.Serializer.Serializer.Serialize(value.GetCanChangeTrick(), dataPool);
		case 25:
			return GameData.Serializer.Serializer.Serialize(value.GetChangingTrick(), dataPool);
		case 26:
			return GameData.Serializer.Serializer.Serialize(value.GetChangeTrickAttack(), dataPool);
		case 27:
			return GameData.Serializer.Serializer.Serialize(value.GetIsFightBack(), dataPool);
		case 28:
			return GameData.Serializer.Serializer.Serialize(value.GetTricks(), dataPool);
		case 29:
			return GameData.Serializer.Serializer.Serialize(value.GetInjuries(), dataPool);
		case 30:
			return GameData.Serializer.Serializer.Serialize(value.GetOldInjuries(), dataPool);
		case 31:
			return GameData.Serializer.Serializer.Serialize(value.GetInjuryAutoHealCollection(), dataPool);
		case 32:
			return GameData.Serializer.Serializer.Serialize(value.GetDamageStepCollection(), dataPool);
		case 33:
			return GameData.Serializer.Serializer.Serialize(value.GetOuterDamageValue(), dataPool);
		case 34:
			return GameData.Serializer.Serializer.Serialize(value.GetInnerDamageValue(), dataPool);
		case 35:
			return GameData.Serializer.Serializer.Serialize(value.GetMindDamageValue(), dataPool);
		case 36:
			return GameData.Serializer.Serializer.Serialize(value.GetFatalDamageValue(), dataPool);
		case 37:
			return GameData.Serializer.Serializer.Serialize(value.GetOuterDamageValueToShow(), dataPool);
		case 38:
			return GameData.Serializer.Serializer.Serialize(value.GetInnerDamageValueToShow(), dataPool);
		case 39:
			return GameData.Serializer.Serializer.Serialize(value.GetMindDamageValueToShow(), dataPool);
		case 40:
			return GameData.Serializer.Serializer.Serialize(value.GetFatalDamageValueToShow(), dataPool);
		case 41:
			return GameData.Serializer.Serializer.Serialize(value.GetFlawCount(), dataPool);
		case 42:
			return GameData.Serializer.Serializer.Serialize(value.GetFlawCollection(), dataPool);
		case 43:
			return GameData.Serializer.Serializer.Serialize(value.GetAcupointCount(), dataPool);
		case 44:
			return GameData.Serializer.Serializer.Serialize(value.GetAcupointCollection(), dataPool);
		case 45:
			return GameData.Serializer.Serializer.Serialize(value.GetMindMarkTime(), dataPool);
		case 46:
			return GameData.Serializer.Serializer.Serialize(value.GetPoison(), dataPool);
		case 47:
			return GameData.Serializer.Serializer.Serialize(value.GetOldPoison(), dataPool);
		case 48:
			return GameData.Serializer.Serializer.Serialize(value.GetPoisonResist(), dataPool);
		case 49:
			return GameData.Serializer.Serializer.Serialize(value.GetNewPoisonsToShow(), dataPool);
		case 50:
			return GameData.Serializer.Serializer.Serialize(value.GetDefeatMarkCollection(), dataPool);
		case 51:
			return GameData.Serializer.Serializer.Serialize(value.GetNeigongList(), dataPool);
		case 52:
			return GameData.Serializer.Serializer.Serialize(value.GetAttackSkillList(), dataPool);
		case 53:
			return GameData.Serializer.Serializer.Serialize(value.GetAgileSkillList(), dataPool);
		case 54:
			return GameData.Serializer.Serializer.Serialize(value.GetDefenceSkillList(), dataPool);
		case 55:
			return GameData.Serializer.Serializer.Serialize(value.GetAssistSkillList(), dataPool);
		case 56:
			return GameData.Serializer.Serializer.Serialize(value.GetPreparingSkillId(), dataPool);
		case 57:
			return GameData.Serializer.Serializer.Serialize(value.GetSkillPreparePercent(), dataPool);
		case 58:
			return GameData.Serializer.Serializer.Serialize(value.GetPerformingSkillId(), dataPool);
		case 59:
			return GameData.Serializer.Serializer.Serialize(value.GetAutoCastingSkill(), dataPool);
		case 60:
			return GameData.Serializer.Serializer.Serialize(value.GetAttackSkillAttackIndex(), dataPool);
		case 61:
			return GameData.Serializer.Serializer.Serialize(value.GetAttackSkillPower(), dataPool);
		case 62:
			return GameData.Serializer.Serializer.Serialize(value.GetAffectingMoveSkillId(), dataPool);
		case 63:
			return GameData.Serializer.Serializer.Serialize(value.GetAffectingDefendSkillId(), dataPool);
		case 64:
			return GameData.Serializer.Serializer.Serialize(value.GetDefendSkillTimePercent(), dataPool);
		case 65:
			return GameData.Serializer.Serializer.Serialize(value.GetWugCount(), dataPool);
		case 66:
			return GameData.Serializer.Serializer.Serialize(value.GetHealInjuryCount(), dataPool);
		case 67:
			return GameData.Serializer.Serializer.Serialize(value.GetHealPoisonCount(), dataPool);
		case 68:
			return GameData.Serializer.Serializer.Serialize(value.GetOtherActionCanUse(), dataPool);
		case 69:
			return GameData.Serializer.Serializer.Serialize(value.GetPreparingOtherAction(), dataPool);
		case 70:
			return GameData.Serializer.Serializer.Serialize(value.GetOtherActionPreparePercent(), dataPool);
		case 71:
			return GameData.Serializer.Serializer.Serialize(value.GetCanSurrender(), dataPool);
		case 72:
			return GameData.Serializer.Serializer.Serialize(value.GetCanUseItem(), dataPool);
		case 73:
			return GameData.Serializer.Serializer.Serialize(value.GetPreparingItem(), dataPool);
		case 74:
			return GameData.Serializer.Serializer.Serialize(value.GetUseItemPreparePercent(), dataPool);
		case 75:
			return GameData.Serializer.Serializer.Serialize(value.GetCombatReserveData(), dataPool);
		case 76:
			return GameData.Serializer.Serializer.Serialize(value.GetBuffCombatStateCollection(), dataPool);
		case 77:
			return GameData.Serializer.Serializer.Serialize(value.GetDebuffCombatStateCollection(), dataPool);
		case 78:
			return GameData.Serializer.Serializer.Serialize(value.GetSpecialCombatStateCollection(), dataPool);
		case 79:
			return GameData.Serializer.Serializer.Serialize(value.GetSkillEffectCollection(), dataPool);
		case 80:
			return GameData.Serializer.Serializer.Serialize(value.GetXiangshuEffectId(), dataPool);
		case 81:
			return GameData.Serializer.Serializer.Serialize(value.GetHazardValue(), dataPool);
		case 82:
			return GameData.Serializer.Serializer.Serialize(value.GetShowEffectList(), dataPool);
		case 83:
			return GameData.Serializer.Serializer.Serialize(value.GetAnimationToLoop(), dataPool);
		case 84:
			return GameData.Serializer.Serializer.Serialize(value.GetAnimationToPlayOnce(), dataPool);
		case 85:
			return GameData.Serializer.Serializer.Serialize(value.GetParticleToPlay(), dataPool);
		case 86:
			return GameData.Serializer.Serializer.Serialize(value.GetParticleToLoop(), dataPool);
		case 87:
			return GameData.Serializer.Serializer.Serialize(value.GetSkillPetAnimation(), dataPool);
		case 88:
			return GameData.Serializer.Serializer.Serialize(value.GetPetParticle(), dataPool);
		case 89:
			return GameData.Serializer.Serializer.Serialize(value.GetAnimationTimeScale(), dataPool);
		case 90:
			return GameData.Serializer.Serializer.Serialize(value.GetAttackOutOfRange(), dataPool);
		case 91:
			return GameData.Serializer.Serializer.Serialize(value.GetAttackSoundToPlay(), dataPool);
		case 92:
			return GameData.Serializer.Serializer.Serialize(value.GetSkillSoundToPlay(), dataPool);
		case 93:
			return GameData.Serializer.Serializer.Serialize(value.GetHitSoundToPlay(), dataPool);
		case 94:
			return GameData.Serializer.Serializer.Serialize(value.GetArmorHitSoundToPlay(), dataPool);
		case 95:
			return GameData.Serializer.Serializer.Serialize(value.GetWhooshSoundToPlay(), dataPool);
		case 96:
			return GameData.Serializer.Serializer.Serialize(value.GetShockSoundToPlay(), dataPool);
		case 97:
			return GameData.Serializer.Serializer.Serialize(value.GetStepSoundToPlay(), dataPool);
		case 98:
			return GameData.Serializer.Serializer.Serialize(value.GetDieSoundToPlay(), dataPool);
		case 99:
			return GameData.Serializer.Serializer.Serialize(value.GetSoundToLoop(), dataPool);
		case 100:
			return GameData.Serializer.Serializer.Serialize(value.GetBossPhase(), dataPool);
		case 101:
			return GameData.Serializer.Serializer.Serialize(value.GetAnimalAttackCount(), dataPool);
		case 102:
			return GameData.Serializer.Serializer.Serialize(value.GetShowTransferInjuryCommand(), dataPool);
		case 103:
			return GameData.Serializer.Serializer.Serialize(value.GetCurrTeammateCommands(), dataPool);
		case 104:
			return GameData.Serializer.Serializer.Serialize(value.GetTeammateCommandCdPercent(), dataPool);
		case 105:
			return GameData.Serializer.Serializer.Serialize(value.GetExecutingTeammateCommand(), dataPool);
		case 106:
			return GameData.Serializer.Serializer.Serialize(value.GetVisible(), dataPool);
		case 107:
			return GameData.Serializer.Serializer.Serialize(value.GetTeammateCommandPreparePercent(), dataPool);
		case 108:
			return GameData.Serializer.Serializer.Serialize(value.GetTeammateCommandTimePercent(), dataPool);
		case 109:
			return GameData.Serializer.Serializer.Serialize(value.GetAttackCommandWeaponKey(), dataPool);
		case 110:
			return GameData.Serializer.Serializer.Serialize(value.GetAttackCommandTrickType(), dataPool);
		case 111:
			return GameData.Serializer.Serializer.Serialize(value.GetDefendCommandSkillId(), dataPool);
		case 112:
			return GameData.Serializer.Serializer.Serialize(value.GetShowEffectCommandIndex(), dataPool);
		case 113:
			return GameData.Serializer.Serializer.Serialize(value.GetAttackCommandSkillId(), dataPool);
		case 114:
			return GameData.Serializer.Serializer.Serialize(value.GetTeammateCommandBanReasons(), dataPool);
		case 115:
			return GameData.Serializer.Serializer.Serialize(value.GetTargetDistance(), dataPool);
		case 116:
			return GameData.Serializer.Serializer.Serialize(value.GetOldInjuryAutoHealCollection(), dataPool);
		case 117:
			return GameData.Serializer.Serializer.Serialize(value.GetMixPoisonAffectedCount(), dataPool);
		case 118:
			return GameData.Serializer.Serializer.Serialize(value.GetParticleToLoopByCombatSkill(), dataPool);
		case 119:
			return GameData.Serializer.Serializer.Serialize(value.GetNeiliAllocationCd(), dataPool);
		case 120:
			return GameData.Serializer.Serializer.Serialize(value.GetProportionDelta(), dataPool);
		case 121:
			return GameData.Serializer.Serializer.Serialize(value.GetMindMarkInfinityCount(), dataPool);
		case 122:
			return GameData.Serializer.Serializer.Serialize(value.GetMindMarkInfinityProgress(), dataPool);
		case 123:
			return GameData.Serializer.Serializer.Serialize(value.GetShowCommandList(), dataPool);
		case 124:
			return GameData.Serializer.Serializer.Serialize(value.GetUnlockPrepareValue(), dataPool);
		case 125:
			return GameData.Serializer.Serializer.Serialize(value.GetRawCreateEffects(), dataPool);
		case 126:
			return GameData.Serializer.Serializer.Serialize(value.GetRawCreateCollection(), dataPool);
		case 127:
			return GameData.Serializer.Serializer.Serialize(value.GetNormalAttackRecovery(), dataPool);
		case 128:
			return GameData.Serializer.Serializer.Serialize(value.GetReserveNormalAttack(), dataPool);
		case 129:
			return GameData.Serializer.Serializer.Serialize(value.GetGangqi(), dataPool);
		case 130:
			return GameData.Serializer.Serializer.Serialize(value.GetGangqiMax(), dataPool);
		case 131:
			return GameData.Serializer.Serializer.Serialize(value.GetMaxTrickCount(), dataPool);
		case 132:
			return GameData.Serializer.Serializer.Serialize(value.GetMobilityLevel(), dataPool);
		case 133:
			return GameData.Serializer.Serializer.Serialize(value.GetTeammateCommandCanUse(), dataPool);
		case 134:
			return GameData.Serializer.Serializer.Serialize(value.GetChangeDistanceDuration(), dataPool);
		case 135:
			return GameData.Serializer.Serializer.Serialize(value.GetAttackRange(), dataPool);
		case 136:
			return GameData.Serializer.Serializer.Serialize(value.GetHappiness(), dataPool);
		case 137:
			return GameData.Serializer.Serializer.Serialize(value.GetSilenceData(), dataPool);
		case 138:
			return GameData.Serializer.Serializer.Serialize(value.GetCombatStateTotalBuffPower(), dataPool);
		case 139:
			return GameData.Serializer.Serializer.Serialize(value.GetHeavyOrBreakInjuryData(), dataPool);
		case 140:
			return GameData.Serializer.Serializer.Serialize(value.GetMoveCd(), dataPool);
		case 141:
			return GameData.Serializer.Serializer.Serialize(value.GetMobilityRecoverSpeed(), dataPool);
		case 142:
			return GameData.Serializer.Serializer.Serialize(value.GetCanUnlockAttack(), dataPool);
		case 143:
			return GameData.Serializer.Serializer.Serialize(value.GetValidItems(), dataPool);
		case 144:
			return GameData.Serializer.Serializer.Serialize(value.GetValidItemAndCounts(), dataPool);
		default:
			if (fieldId >= 145)
			{
				throw new Exception($"Unsupported fieldId {fieldId}");
			}
			throw new Exception($"Not allow to get readonly field data: {fieldId}");
		}
	}

	public void SetElementField_CombatCharacterDict(int objectId, ushort fieldId, int valueOffset, RawDataPool dataPool, DataContext context)
	{
		if (!_combatCharacterDict.TryGetValue(objectId, out var value))
		{
			throw new Exception($"Failed to find element {objectId} with field {fieldId}");
		}
		switch (fieldId)
		{
		case 0:
			throw new Exception($"Not allow to set readonly field data: {fieldId}");
		case 1:
		{
			int item3 = 0;
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item3);
			value.SetBreathValue(item3, context);
			return;
		}
		case 2:
		{
			int item2 = 0;
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item2);
			value.SetStanceValue(item2, context);
			return;
		}
		case 3:
		{
			NeiliAllocation item = default(NeiliAllocation);
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item);
			value.SetNeiliAllocation(item, context);
			return;
		}
		case 4:
		{
			NeiliAllocation item130 = default(NeiliAllocation);
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item130);
			value.SetOriginNeiliAllocation(item130, context);
			return;
		}
		case 5:
		{
			NeiliAllocation item129 = default(NeiliAllocation);
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item129);
			value.SetNeiliAllocationRecoverProgress(item129, context);
			return;
		}
		case 6:
		{
			short item128 = 0;
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item128);
			value.SetOldDisorderOfQi(item128, context);
			return;
		}
		case 7:
		{
			sbyte item127 = 0;
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item127);
			value.SetNeiliType(item127, context);
			return;
		}
		case 8:
		{
			ShowAvoidData item126 = default(ShowAvoidData);
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item126);
			value.SetAvoidToShow(item126, context);
			return;
		}
		case 9:
		{
			int item125 = 0;
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item125);
			value.SetCurrentPosition(item125, context);
			return;
		}
		case 10:
		{
			int item124 = 0;
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item124);
			value.SetDisplayPosition(item124, context);
			return;
		}
		case 11:
		{
			int item123 = 0;
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item123);
			value.SetMobilityValue(item123, context);
			return;
		}
		case 12:
		{
			sbyte item122 = 0;
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item122);
			value.SetJumpPrepareProgress(item122, context);
			return;
		}
		case 13:
		{
			short item121 = 0;
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item121);
			value.SetJumpPreparedDistance(item121, context);
			return;
		}
		case 14:
		{
			short item120 = 0;
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item120);
			value.SetMobilityLockEffectCount(item120, context);
			return;
		}
		case 15:
		{
			float item119 = 0f;
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item119);
			value.SetJumpChangeDistanceDuration(item119, context);
			return;
		}
		case 16:
		{
			int item118 = 0;
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item118);
			value.SetUsingWeaponIndex(item118, context);
			return;
		}
		case 17:
		{
			sbyte[] item117 = value.GetWeaponTricks();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item117);
			value.SetWeaponTricks(item117, context);
			return;
		}
		case 18:
		{
			byte item116 = 0;
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item116);
			value.SetWeaponTrickIndex(item116, context);
			return;
		}
		case 19:
		{
			ItemKey[] item115 = value.GetWeapons();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item115);
			value.SetWeapons(item115, context);
			return;
		}
		case 20:
		{
			sbyte item114 = 0;
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item114);
			value.SetAttackingTrickType(item114, context);
			return;
		}
		case 21:
		{
			bool item113 = false;
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item113);
			value.SetCanAttackOutRange(item113, context);
			return;
		}
		case 22:
		{
			sbyte item112 = 0;
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item112);
			value.SetChangeTrickProgress(item112, context);
			return;
		}
		case 23:
		{
			short item111 = 0;
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item111);
			value.SetChangeTrickCount(item111, context);
			return;
		}
		case 24:
		{
			bool item110 = false;
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item110);
			value.SetCanChangeTrick(item110, context);
			return;
		}
		case 25:
		{
			bool item109 = false;
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item109);
			value.SetChangingTrick(item109, context);
			return;
		}
		case 26:
		{
			bool item108 = false;
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item108);
			value.SetChangeTrickAttack(item108, context);
			return;
		}
		case 27:
		{
			bool item107 = false;
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item107);
			value.SetIsFightBack(item107, context);
			return;
		}
		case 28:
		{
			TrickCollection item106 = value.GetTricks();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item106);
			value.SetTricks(item106, context);
			return;
		}
		case 29:
		{
			Injuries item105 = default(Injuries);
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item105);
			value.SetInjuries(item105, context);
			return;
		}
		case 30:
		{
			Injuries item104 = default(Injuries);
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item104);
			value.SetOldInjuries(item104, context);
			return;
		}
		case 31:
		{
			InjuryAutoHealCollection item103 = value.GetInjuryAutoHealCollection();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item103);
			value.SetInjuryAutoHealCollection(item103, context);
			return;
		}
		case 32:
		{
			DamageStepCollection item102 = value.GetDamageStepCollection();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item102);
			value.SetDamageStepCollection(item102, context);
			return;
		}
		case 33:
		{
			int[] item101 = value.GetOuterDamageValue();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item101);
			value.SetOuterDamageValue(item101, context);
			return;
		}
		case 34:
		{
			int[] item100 = value.GetInnerDamageValue();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item100);
			value.SetInnerDamageValue(item100, context);
			return;
		}
		case 35:
		{
			int item99 = 0;
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item99);
			value.SetMindDamageValue(item99, context);
			return;
		}
		case 36:
		{
			int item98 = 0;
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item98);
			value.SetFatalDamageValue(item98, context);
			return;
		}
		case 37:
		{
			IntPair[] item97 = value.GetOuterDamageValueToShow();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item97);
			value.SetOuterDamageValueToShow(item97, context);
			return;
		}
		case 38:
		{
			IntPair[] item96 = value.GetInnerDamageValueToShow();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item96);
			value.SetInnerDamageValueToShow(item96, context);
			return;
		}
		case 39:
		{
			int item95 = 0;
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item95);
			value.SetMindDamageValueToShow(item95, context);
			return;
		}
		case 40:
		{
			int item94 = 0;
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item94);
			value.SetFatalDamageValueToShow(item94, context);
			return;
		}
		case 41:
		{
			byte[] item93 = value.GetFlawCount();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item93);
			value.SetFlawCount(item93, context);
			return;
		}
		case 42:
		{
			FlawOrAcupointCollection item92 = value.GetFlawCollection();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item92);
			value.SetFlawCollection(item92, context);
			return;
		}
		case 43:
		{
			byte[] item91 = value.GetAcupointCount();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item91);
			value.SetAcupointCount(item91, context);
			return;
		}
		case 44:
		{
			FlawOrAcupointCollection item90 = value.GetAcupointCollection();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item90);
			value.SetAcupointCollection(item90, context);
			return;
		}
		case 45:
		{
			MindMarkList item89 = value.GetMindMarkTime();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item89);
			value.SetMindMarkTime(item89, context);
			return;
		}
		case 46:
		{
			PoisonInts item88 = default(PoisonInts);
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item88);
			value.SetPoison(ref item88, context);
			return;
		}
		case 47:
		{
			PoisonInts item87 = default(PoisonInts);
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item87);
			value.SetOldPoison(ref item87, context);
			return;
		}
		case 48:
		{
			PoisonInts item86 = default(PoisonInts);
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item86);
			value.SetPoisonResist(ref item86, context);
			return;
		}
		case 49:
		{
			PoisonsAndLevels item85 = default(PoisonsAndLevels);
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item85);
			value.SetNewPoisonsToShow(ref item85, context);
			return;
		}
		case 50:
		{
			DefeatMarkCollection item84 = value.GetDefeatMarkCollection();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item84);
			value.SetDefeatMarkCollection(item84, context);
			return;
		}
		case 51:
		{
			List<short> item83 = value.GetNeigongList();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item83);
			value.SetNeigongList(item83, context);
			return;
		}
		case 52:
		{
			List<short> item82 = value.GetAttackSkillList();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item82);
			value.SetAttackSkillList(item82, context);
			return;
		}
		case 53:
		{
			List<short> item81 = value.GetAgileSkillList();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item81);
			value.SetAgileSkillList(item81, context);
			return;
		}
		case 54:
		{
			List<short> item80 = value.GetDefenceSkillList();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item80);
			value.SetDefenceSkillList(item80, context);
			return;
		}
		case 55:
		{
			List<short> item79 = value.GetAssistSkillList();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item79);
			value.SetAssistSkillList(item79, context);
			return;
		}
		case 56:
		{
			short item78 = 0;
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item78);
			value.SetPreparingSkillId(item78, context);
			return;
		}
		case 57:
		{
			byte item77 = 0;
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item77);
			value.SetSkillPreparePercent(item77, context);
			return;
		}
		case 58:
		{
			short item76 = 0;
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item76);
			value.SetPerformingSkillId(item76, context);
			return;
		}
		case 59:
		{
			bool item75 = false;
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item75);
			value.SetAutoCastingSkill(item75, context);
			return;
		}
		case 60:
		{
			byte item74 = 0;
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item74);
			value.SetAttackSkillAttackIndex(item74, context);
			return;
		}
		case 61:
		{
			byte item73 = 0;
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item73);
			value.SetAttackSkillPower(item73, context);
			return;
		}
		case 62:
		{
			short item72 = 0;
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item72);
			value.SetAffectingMoveSkillId(item72, context);
			return;
		}
		case 63:
		{
			short item71 = 0;
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item71);
			value.SetAffectingDefendSkillId(item71, context);
			return;
		}
		case 64:
		{
			byte item70 = 0;
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item70);
			value.SetDefendSkillTimePercent(item70, context);
			return;
		}
		case 65:
		{
			short item69 = 0;
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item69);
			value.SetWugCount(item69, context);
			return;
		}
		case 66:
		{
			byte item68 = 0;
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item68);
			value.SetHealInjuryCount(item68, context);
			return;
		}
		case 67:
		{
			byte item67 = 0;
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item67);
			value.SetHealPoisonCount(item67, context);
			return;
		}
		case 68:
		{
			bool[] item66 = value.GetOtherActionCanUse();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item66);
			value.SetOtherActionCanUse(item66, context);
			return;
		}
		case 69:
		{
			sbyte item65 = 0;
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item65);
			value.SetPreparingOtherAction(item65, context);
			return;
		}
		case 70:
		{
			byte item64 = 0;
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item64);
			value.SetOtherActionPreparePercent(item64, context);
			return;
		}
		case 71:
		{
			bool item63 = false;
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item63);
			value.SetCanSurrender(item63, context);
			return;
		}
		case 72:
		{
			bool item62 = false;
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item62);
			value.SetCanUseItem(item62, context);
			return;
		}
		case 73:
		{
			ItemKey item61 = default(ItemKey);
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item61);
			value.SetPreparingItem(item61, context);
			return;
		}
		case 74:
		{
			byte item60 = 0;
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item60);
			value.SetUseItemPreparePercent(item60, context);
			return;
		}
		case 75:
		{
			CombatReserveData item59 = default(CombatReserveData);
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item59);
			value.SetCombatReserveData(item59, context);
			return;
		}
		case 76:
		{
			CombatStateCollection item58 = value.GetBuffCombatStateCollection();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item58);
			value.SetBuffCombatStateCollection(item58, context);
			return;
		}
		case 77:
		{
			CombatStateCollection item57 = value.GetDebuffCombatStateCollection();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item57);
			value.SetDebuffCombatStateCollection(item57, context);
			return;
		}
		case 78:
		{
			CombatStateCollection item56 = value.GetSpecialCombatStateCollection();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item56);
			value.SetSpecialCombatStateCollection(item56, context);
			return;
		}
		case 79:
		{
			SkillEffectCollection item55 = value.GetSkillEffectCollection();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item55);
			value.SetSkillEffectCollection(item55, context);
			return;
		}
		case 80:
		{
			short item54 = 0;
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item54);
			value.SetXiangshuEffectId(item54, context);
			return;
		}
		case 81:
		{
			int item53 = 0;
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item53);
			value.SetHazardValue(item53, context);
			return;
		}
		case 82:
		{
			ShowSpecialEffectCollection item52 = value.GetShowEffectList();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item52);
			value.SetShowEffectList(item52, context);
			return;
		}
		case 83:
		{
			string item51 = null;
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item51);
			value.SetAnimationToLoop(item51, context);
			return;
		}
		case 84:
		{
			string item50 = null;
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item50);
			value.SetAnimationToPlayOnce(item50, context);
			return;
		}
		case 85:
		{
			string item49 = null;
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item49);
			value.SetParticleToPlay(item49, context);
			return;
		}
		case 86:
		{
			string item48 = null;
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item48);
			value.SetParticleToLoop(item48, context);
			return;
		}
		case 87:
		{
			string item47 = null;
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item47);
			value.SetSkillPetAnimation(item47, context);
			return;
		}
		case 88:
		{
			string item46 = null;
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item46);
			value.SetPetParticle(item46, context);
			return;
		}
		case 89:
		{
			float item45 = 0f;
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item45);
			value.SetAnimationTimeScale(item45, context);
			return;
		}
		case 90:
		{
			bool item44 = false;
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item44);
			value.SetAttackOutOfRange(item44, context);
			return;
		}
		case 91:
		{
			string item43 = null;
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item43);
			value.SetAttackSoundToPlay(item43, context);
			return;
		}
		case 92:
		{
			string item42 = null;
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item42);
			value.SetSkillSoundToPlay(item42, context);
			return;
		}
		case 93:
		{
			string item41 = null;
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item41);
			value.SetHitSoundToPlay(item41, context);
			return;
		}
		case 94:
		{
			string item40 = null;
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item40);
			value.SetArmorHitSoundToPlay(item40, context);
			return;
		}
		case 95:
		{
			string item39 = null;
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item39);
			value.SetWhooshSoundToPlay(item39, context);
			return;
		}
		case 96:
		{
			string item38 = null;
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item38);
			value.SetShockSoundToPlay(item38, context);
			return;
		}
		case 97:
		{
			string item37 = null;
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item37);
			value.SetStepSoundToPlay(item37, context);
			return;
		}
		case 98:
		{
			string item36 = null;
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item36);
			value.SetDieSoundToPlay(item36, context);
			return;
		}
		case 99:
		{
			string item35 = null;
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item35);
			value.SetSoundToLoop(item35, context);
			return;
		}
		case 100:
		{
			sbyte item34 = 0;
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item34);
			value.SetBossPhase(item34, context);
			return;
		}
		case 101:
		{
			sbyte item33 = 0;
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item33);
			value.SetAnimalAttackCount(item33, context);
			return;
		}
		case 102:
		{
			bool item32 = false;
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item32);
			value.SetShowTransferInjuryCommand(item32, context);
			return;
		}
		case 103:
		{
			List<sbyte> item31 = value.GetCurrTeammateCommands();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item31);
			value.SetCurrTeammateCommands(item31, context);
			return;
		}
		case 104:
		{
			List<byte> item30 = value.GetTeammateCommandCdPercent();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item30);
			value.SetTeammateCommandCdPercent(item30, context);
			return;
		}
		case 105:
		{
			sbyte item29 = 0;
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item29);
			value.SetExecutingTeammateCommand(item29, context);
			return;
		}
		case 106:
		{
			bool item28 = false;
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item28);
			value.SetVisible(item28, context);
			return;
		}
		case 107:
		{
			byte item27 = 0;
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item27);
			value.SetTeammateCommandPreparePercent(item27, context);
			return;
		}
		case 108:
		{
			byte item26 = 0;
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item26);
			value.SetTeammateCommandTimePercent(item26, context);
			return;
		}
		case 109:
		{
			ItemKey item25 = default(ItemKey);
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item25);
			value.SetAttackCommandWeaponKey(item25, context);
			return;
		}
		case 110:
		{
			sbyte item24 = 0;
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item24);
			value.SetAttackCommandTrickType(item24, context);
			return;
		}
		case 111:
		{
			short item23 = 0;
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item23);
			value.SetDefendCommandSkillId(item23, context);
			return;
		}
		case 112:
		{
			sbyte item22 = 0;
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item22);
			value.SetShowEffectCommandIndex(item22, context);
			return;
		}
		case 113:
		{
			short item21 = 0;
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item21);
			value.SetAttackCommandSkillId(item21, context);
			return;
		}
		case 114:
		{
			List<SByteList> item20 = value.GetTeammateCommandBanReasons();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item20);
			value.SetTeammateCommandBanReasons(item20, context);
			return;
		}
		case 115:
		{
			short item19 = 0;
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item19);
			value.SetTargetDistance(item19, context);
			return;
		}
		case 116:
		{
			InjuryAutoHealCollection item18 = value.GetOldInjuryAutoHealCollection();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item18);
			value.SetOldInjuryAutoHealCollection(item18, context);
			return;
		}
		case 117:
		{
			MixPoisonAffectedCountCollection item17 = value.GetMixPoisonAffectedCount();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item17);
			value.SetMixPoisonAffectedCount(item17, context);
			return;
		}
		case 118:
		{
			string item16 = null;
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item16);
			value.SetParticleToLoopByCombatSkill(item16, context);
			return;
		}
		case 119:
		{
			SilenceFrameData item15 = default(SilenceFrameData);
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item15);
			value.SetNeiliAllocationCd(item15, context);
			return;
		}
		case 120:
		{
			NeiliProportionOfFiveElements item14 = default(NeiliProportionOfFiveElements);
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item14);
			value.SetProportionDelta(item14, context);
			return;
		}
		case 121:
		{
			int item13 = 0;
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item13);
			value.SetMindMarkInfinityCount(item13, context);
			return;
		}
		case 122:
		{
			int item12 = 0;
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item12);
			value.SetMindMarkInfinityProgress(item12, context);
			return;
		}
		case 123:
		{
			List<TeammateCommandDisplayData> item11 = value.GetShowCommandList();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item11);
			value.SetShowCommandList(item11, context);
			return;
		}
		case 124:
		{
			List<int> item10 = value.GetUnlockPrepareValue();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item10);
			value.SetUnlockPrepareValue(item10, context);
			return;
		}
		case 125:
		{
			List<int> item9 = value.GetRawCreateEffects();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item9);
			value.SetRawCreateEffects(item9, context);
			return;
		}
		case 126:
		{
			RawCreateCollection item8 = value.GetRawCreateCollection();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item8);
			value.SetRawCreateCollection(item8, context);
			return;
		}
		case 127:
		{
			SilenceFrameData item7 = default(SilenceFrameData);
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item7);
			value.SetNormalAttackRecovery(item7, context);
			return;
		}
		case 128:
		{
			bool item6 = false;
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item6);
			value.SetReserveNormalAttack(item6, context);
			return;
		}
		case 129:
		{
			int item5 = 0;
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item5);
			value.SetGangqi(item5, context);
			return;
		}
		case 130:
		{
			int item4 = 0;
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item4);
			value.SetGangqiMax(item4, context);
			return;
		}
		}
		if (fieldId >= 145)
		{
			throw new Exception($"Unsupported fieldId {fieldId}");
		}
		if (fieldId >= 145)
		{
			throw new Exception($"Not allow to set readonly field data: {fieldId}");
		}
		throw new Exception($"Not allow to set cache field data: {fieldId}");
	}

	private int CheckModified_CombatCharacterDict(int objectId, ushort fieldId, RawDataPool dataPool)
	{
		if (!_combatCharacterDict.TryGetValue(objectId, out var value))
		{
			return -1;
		}
		if (fieldId >= 145)
		{
			throw new Exception($"Not allow to check readonly field data: {fieldId}");
		}
		if (!_dataStatesCombatCharacterDict.IsModified(value.DataStatesOffset, fieldId))
		{
			return -1;
		}
		_dataStatesCombatCharacterDict.ResetModified(value.DataStatesOffset, fieldId);
		return fieldId switch
		{
			0 => GameData.Serializer.Serializer.Serialize(value.GetId(), dataPool), 
			1 => GameData.Serializer.Serializer.Serialize(value.GetBreathValue(), dataPool), 
			2 => GameData.Serializer.Serializer.Serialize(value.GetStanceValue(), dataPool), 
			3 => GameData.Serializer.Serializer.Serialize(value.GetNeiliAllocation(), dataPool), 
			4 => GameData.Serializer.Serializer.Serialize(value.GetOriginNeiliAllocation(), dataPool), 
			5 => GameData.Serializer.Serializer.Serialize(value.GetNeiliAllocationRecoverProgress(), dataPool), 
			6 => GameData.Serializer.Serializer.Serialize(value.GetOldDisorderOfQi(), dataPool), 
			7 => GameData.Serializer.Serializer.Serialize(value.GetNeiliType(), dataPool), 
			8 => GameData.Serializer.Serializer.Serialize(value.GetAvoidToShow(), dataPool), 
			9 => GameData.Serializer.Serializer.Serialize(value.GetCurrentPosition(), dataPool), 
			10 => GameData.Serializer.Serializer.Serialize(value.GetDisplayPosition(), dataPool), 
			11 => GameData.Serializer.Serializer.Serialize(value.GetMobilityValue(), dataPool), 
			12 => GameData.Serializer.Serializer.Serialize(value.GetJumpPrepareProgress(), dataPool), 
			13 => GameData.Serializer.Serializer.Serialize(value.GetJumpPreparedDistance(), dataPool), 
			14 => GameData.Serializer.Serializer.Serialize(value.GetMobilityLockEffectCount(), dataPool), 
			15 => GameData.Serializer.Serializer.Serialize(value.GetJumpChangeDistanceDuration(), dataPool), 
			16 => GameData.Serializer.Serializer.Serialize(value.GetUsingWeaponIndex(), dataPool), 
			17 => GameData.Serializer.Serializer.Serialize(value.GetWeaponTricks(), dataPool), 
			18 => GameData.Serializer.Serializer.Serialize(value.GetWeaponTrickIndex(), dataPool), 
			19 => GameData.Serializer.Serializer.Serialize(value.GetWeapons(), dataPool), 
			20 => GameData.Serializer.Serializer.Serialize(value.GetAttackingTrickType(), dataPool), 
			21 => GameData.Serializer.Serializer.Serialize(value.GetCanAttackOutRange(), dataPool), 
			22 => GameData.Serializer.Serializer.Serialize(value.GetChangeTrickProgress(), dataPool), 
			23 => GameData.Serializer.Serializer.Serialize(value.GetChangeTrickCount(), dataPool), 
			24 => GameData.Serializer.Serializer.Serialize(value.GetCanChangeTrick(), dataPool), 
			25 => GameData.Serializer.Serializer.Serialize(value.GetChangingTrick(), dataPool), 
			26 => GameData.Serializer.Serializer.Serialize(value.GetChangeTrickAttack(), dataPool), 
			27 => GameData.Serializer.Serializer.Serialize(value.GetIsFightBack(), dataPool), 
			28 => GameData.Serializer.Serializer.Serialize(value.GetTricks(), dataPool), 
			29 => GameData.Serializer.Serializer.Serialize(value.GetInjuries(), dataPool), 
			30 => GameData.Serializer.Serializer.Serialize(value.GetOldInjuries(), dataPool), 
			31 => GameData.Serializer.Serializer.Serialize(value.GetInjuryAutoHealCollection(), dataPool), 
			32 => GameData.Serializer.Serializer.Serialize(value.GetDamageStepCollection(), dataPool), 
			33 => GameData.Serializer.Serializer.Serialize(value.GetOuterDamageValue(), dataPool), 
			34 => GameData.Serializer.Serializer.Serialize(value.GetInnerDamageValue(), dataPool), 
			35 => GameData.Serializer.Serializer.Serialize(value.GetMindDamageValue(), dataPool), 
			36 => GameData.Serializer.Serializer.Serialize(value.GetFatalDamageValue(), dataPool), 
			37 => GameData.Serializer.Serializer.Serialize(value.GetOuterDamageValueToShow(), dataPool), 
			38 => GameData.Serializer.Serializer.Serialize(value.GetInnerDamageValueToShow(), dataPool), 
			39 => GameData.Serializer.Serializer.Serialize(value.GetMindDamageValueToShow(), dataPool), 
			40 => GameData.Serializer.Serializer.Serialize(value.GetFatalDamageValueToShow(), dataPool), 
			41 => GameData.Serializer.Serializer.Serialize(value.GetFlawCount(), dataPool), 
			42 => GameData.Serializer.Serializer.Serialize(value.GetFlawCollection(), dataPool), 
			43 => GameData.Serializer.Serializer.Serialize(value.GetAcupointCount(), dataPool), 
			44 => GameData.Serializer.Serializer.Serialize(value.GetAcupointCollection(), dataPool), 
			45 => GameData.Serializer.Serializer.Serialize(value.GetMindMarkTime(), dataPool), 
			46 => GameData.Serializer.Serializer.Serialize(value.GetPoison(), dataPool), 
			47 => GameData.Serializer.Serializer.Serialize(value.GetOldPoison(), dataPool), 
			48 => GameData.Serializer.Serializer.Serialize(value.GetPoisonResist(), dataPool), 
			49 => GameData.Serializer.Serializer.Serialize(value.GetNewPoisonsToShow(), dataPool), 
			50 => GameData.Serializer.Serializer.Serialize(value.GetDefeatMarkCollection(), dataPool), 
			51 => GameData.Serializer.Serializer.Serialize(value.GetNeigongList(), dataPool), 
			52 => GameData.Serializer.Serializer.Serialize(value.GetAttackSkillList(), dataPool), 
			53 => GameData.Serializer.Serializer.Serialize(value.GetAgileSkillList(), dataPool), 
			54 => GameData.Serializer.Serializer.Serialize(value.GetDefenceSkillList(), dataPool), 
			55 => GameData.Serializer.Serializer.Serialize(value.GetAssistSkillList(), dataPool), 
			56 => GameData.Serializer.Serializer.Serialize(value.GetPreparingSkillId(), dataPool), 
			57 => GameData.Serializer.Serializer.Serialize(value.GetSkillPreparePercent(), dataPool), 
			58 => GameData.Serializer.Serializer.Serialize(value.GetPerformingSkillId(), dataPool), 
			59 => GameData.Serializer.Serializer.Serialize(value.GetAutoCastingSkill(), dataPool), 
			60 => GameData.Serializer.Serializer.Serialize(value.GetAttackSkillAttackIndex(), dataPool), 
			61 => GameData.Serializer.Serializer.Serialize(value.GetAttackSkillPower(), dataPool), 
			62 => GameData.Serializer.Serializer.Serialize(value.GetAffectingMoveSkillId(), dataPool), 
			63 => GameData.Serializer.Serializer.Serialize(value.GetAffectingDefendSkillId(), dataPool), 
			64 => GameData.Serializer.Serializer.Serialize(value.GetDefendSkillTimePercent(), dataPool), 
			65 => GameData.Serializer.Serializer.Serialize(value.GetWugCount(), dataPool), 
			66 => GameData.Serializer.Serializer.Serialize(value.GetHealInjuryCount(), dataPool), 
			67 => GameData.Serializer.Serializer.Serialize(value.GetHealPoisonCount(), dataPool), 
			68 => GameData.Serializer.Serializer.Serialize(value.GetOtherActionCanUse(), dataPool), 
			69 => GameData.Serializer.Serializer.Serialize(value.GetPreparingOtherAction(), dataPool), 
			70 => GameData.Serializer.Serializer.Serialize(value.GetOtherActionPreparePercent(), dataPool), 
			71 => GameData.Serializer.Serializer.Serialize(value.GetCanSurrender(), dataPool), 
			72 => GameData.Serializer.Serializer.Serialize(value.GetCanUseItem(), dataPool), 
			73 => GameData.Serializer.Serializer.Serialize(value.GetPreparingItem(), dataPool), 
			74 => GameData.Serializer.Serializer.Serialize(value.GetUseItemPreparePercent(), dataPool), 
			75 => GameData.Serializer.Serializer.Serialize(value.GetCombatReserveData(), dataPool), 
			76 => GameData.Serializer.Serializer.Serialize(value.GetBuffCombatStateCollection(), dataPool), 
			77 => GameData.Serializer.Serializer.Serialize(value.GetDebuffCombatStateCollection(), dataPool), 
			78 => GameData.Serializer.Serializer.Serialize(value.GetSpecialCombatStateCollection(), dataPool), 
			79 => GameData.Serializer.Serializer.Serialize(value.GetSkillEffectCollection(), dataPool), 
			80 => GameData.Serializer.Serializer.Serialize(value.GetXiangshuEffectId(), dataPool), 
			81 => GameData.Serializer.Serializer.Serialize(value.GetHazardValue(), dataPool), 
			82 => GameData.Serializer.Serializer.Serialize(value.GetShowEffectList(), dataPool), 
			83 => GameData.Serializer.Serializer.Serialize(value.GetAnimationToLoop(), dataPool), 
			84 => GameData.Serializer.Serializer.Serialize(value.GetAnimationToPlayOnce(), dataPool), 
			85 => GameData.Serializer.Serializer.Serialize(value.GetParticleToPlay(), dataPool), 
			86 => GameData.Serializer.Serializer.Serialize(value.GetParticleToLoop(), dataPool), 
			87 => GameData.Serializer.Serializer.Serialize(value.GetSkillPetAnimation(), dataPool), 
			88 => GameData.Serializer.Serializer.Serialize(value.GetPetParticle(), dataPool), 
			89 => GameData.Serializer.Serializer.Serialize(value.GetAnimationTimeScale(), dataPool), 
			90 => GameData.Serializer.Serializer.Serialize(value.GetAttackOutOfRange(), dataPool), 
			91 => GameData.Serializer.Serializer.Serialize(value.GetAttackSoundToPlay(), dataPool), 
			92 => GameData.Serializer.Serializer.Serialize(value.GetSkillSoundToPlay(), dataPool), 
			93 => GameData.Serializer.Serializer.Serialize(value.GetHitSoundToPlay(), dataPool), 
			94 => GameData.Serializer.Serializer.Serialize(value.GetArmorHitSoundToPlay(), dataPool), 
			95 => GameData.Serializer.Serializer.Serialize(value.GetWhooshSoundToPlay(), dataPool), 
			96 => GameData.Serializer.Serializer.Serialize(value.GetShockSoundToPlay(), dataPool), 
			97 => GameData.Serializer.Serializer.Serialize(value.GetStepSoundToPlay(), dataPool), 
			98 => GameData.Serializer.Serializer.Serialize(value.GetDieSoundToPlay(), dataPool), 
			99 => GameData.Serializer.Serializer.Serialize(value.GetSoundToLoop(), dataPool), 
			100 => GameData.Serializer.Serializer.Serialize(value.GetBossPhase(), dataPool), 
			101 => GameData.Serializer.Serializer.Serialize(value.GetAnimalAttackCount(), dataPool), 
			102 => GameData.Serializer.Serializer.Serialize(value.GetShowTransferInjuryCommand(), dataPool), 
			103 => GameData.Serializer.Serializer.Serialize(value.GetCurrTeammateCommands(), dataPool), 
			104 => GameData.Serializer.Serializer.Serialize(value.GetTeammateCommandCdPercent(), dataPool), 
			105 => GameData.Serializer.Serializer.Serialize(value.GetExecutingTeammateCommand(), dataPool), 
			106 => GameData.Serializer.Serializer.Serialize(value.GetVisible(), dataPool), 
			107 => GameData.Serializer.Serializer.Serialize(value.GetTeammateCommandPreparePercent(), dataPool), 
			108 => GameData.Serializer.Serializer.Serialize(value.GetTeammateCommandTimePercent(), dataPool), 
			109 => GameData.Serializer.Serializer.Serialize(value.GetAttackCommandWeaponKey(), dataPool), 
			110 => GameData.Serializer.Serializer.Serialize(value.GetAttackCommandTrickType(), dataPool), 
			111 => GameData.Serializer.Serializer.Serialize(value.GetDefendCommandSkillId(), dataPool), 
			112 => GameData.Serializer.Serializer.Serialize(value.GetShowEffectCommandIndex(), dataPool), 
			113 => GameData.Serializer.Serializer.Serialize(value.GetAttackCommandSkillId(), dataPool), 
			114 => GameData.Serializer.Serializer.Serialize(value.GetTeammateCommandBanReasons(), dataPool), 
			115 => GameData.Serializer.Serializer.Serialize(value.GetTargetDistance(), dataPool), 
			116 => GameData.Serializer.Serializer.Serialize(value.GetOldInjuryAutoHealCollection(), dataPool), 
			117 => GameData.Serializer.Serializer.Serialize(value.GetMixPoisonAffectedCount(), dataPool), 
			118 => GameData.Serializer.Serializer.Serialize(value.GetParticleToLoopByCombatSkill(), dataPool), 
			119 => GameData.Serializer.Serializer.Serialize(value.GetNeiliAllocationCd(), dataPool), 
			120 => GameData.Serializer.Serializer.Serialize(value.GetProportionDelta(), dataPool), 
			121 => GameData.Serializer.Serializer.Serialize(value.GetMindMarkInfinityCount(), dataPool), 
			122 => GameData.Serializer.Serializer.Serialize(value.GetMindMarkInfinityProgress(), dataPool), 
			123 => GameData.Serializer.Serializer.Serialize(value.GetShowCommandList(), dataPool), 
			124 => GameData.Serializer.Serializer.Serialize(value.GetUnlockPrepareValue(), dataPool), 
			125 => GameData.Serializer.Serializer.Serialize(value.GetRawCreateEffects(), dataPool), 
			126 => GameData.Serializer.Serializer.Serialize(value.GetRawCreateCollection(), dataPool), 
			127 => GameData.Serializer.Serializer.Serialize(value.GetNormalAttackRecovery(), dataPool), 
			128 => GameData.Serializer.Serializer.Serialize(value.GetReserveNormalAttack(), dataPool), 
			129 => GameData.Serializer.Serializer.Serialize(value.GetGangqi(), dataPool), 
			130 => GameData.Serializer.Serializer.Serialize(value.GetGangqiMax(), dataPool), 
			131 => GameData.Serializer.Serializer.Serialize(value.GetMaxTrickCount(), dataPool), 
			132 => GameData.Serializer.Serializer.Serialize(value.GetMobilityLevel(), dataPool), 
			133 => GameData.Serializer.Serializer.Serialize(value.GetTeammateCommandCanUse(), dataPool), 
			134 => GameData.Serializer.Serializer.Serialize(value.GetChangeDistanceDuration(), dataPool), 
			135 => GameData.Serializer.Serializer.Serialize(value.GetAttackRange(), dataPool), 
			136 => GameData.Serializer.Serializer.Serialize(value.GetHappiness(), dataPool), 
			137 => GameData.Serializer.Serializer.Serialize(value.GetSilenceData(), dataPool), 
			138 => GameData.Serializer.Serializer.Serialize(value.GetCombatStateTotalBuffPower(), dataPool), 
			139 => GameData.Serializer.Serializer.Serialize(value.GetHeavyOrBreakInjuryData(), dataPool), 
			140 => GameData.Serializer.Serializer.Serialize(value.GetMoveCd(), dataPool), 
			141 => GameData.Serializer.Serializer.Serialize(value.GetMobilityRecoverSpeed(), dataPool), 
			142 => GameData.Serializer.Serializer.Serialize(value.GetCanUnlockAttack(), dataPool), 
			143 => GameData.Serializer.Serializer.Serialize(value.GetValidItems(), dataPool), 
			144 => GameData.Serializer.Serializer.Serialize(value.GetValidItemAndCounts(), dataPool), 
			_ => throw new Exception($"Unsupported fieldId {fieldId}"), 
		};
	}

	private void ResetModifiedWrapper_CombatCharacterDict(int objectId, ushort fieldId)
	{
		if (_combatCharacterDict.TryGetValue(objectId, out var value))
		{
			if (fieldId >= 145)
			{
				throw new Exception($"Not allow to reset modification state of readonly field data: {fieldId}");
			}
			if (_dataStatesCombatCharacterDict.IsModified(value.DataStatesOffset, fieldId))
			{
				_dataStatesCombatCharacterDict.ResetModified(value.DataStatesOffset, fieldId);
			}
		}
	}

	private bool IsModifiedWrapper_CombatCharacterDict(int objectId, ushort fieldId)
	{
		if (!_combatCharacterDict.TryGetValue(objectId, out var value))
		{
			return false;
		}
		if (fieldId >= 145)
		{
			throw new Exception($"Not allow to check modification state of readonly field data: {fieldId}");
		}
		return _dataStatesCombatCharacterDict.IsModified(value.DataStatesOffset, fieldId);
	}

	public int[] GetSelfTeam()
	{
		return _selfTeam;
	}

	public void SetSelfTeam(int[] value, DataContext context)
	{
		_selfTeam = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(11, DataStates, CacheInfluences, context);
	}

	public int GetSelfCharId()
	{
		return _selfCharId;
	}

	public void SetSelfCharId(int value, DataContext context)
	{
		_selfCharId = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(12, DataStates, CacheInfluences, context);
	}

	public sbyte GetSelfTeamWisdomType()
	{
		return _selfTeamWisdomType;
	}

	public void SetSelfTeamWisdomType(sbyte value, DataContext context)
	{
		_selfTeamWisdomType = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(13, DataStates, CacheInfluences, context);
	}

	public short GetSelfTeamWisdomCount()
	{
		return _selfTeamWisdomCount;
	}

	public void SetSelfTeamWisdomCount(short value, DataContext context)
	{
		_selfTeamWisdomCount = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(14, DataStates, CacheInfluences, context);
	}

	public int[] GetEnemyTeam()
	{
		return _enemyTeam;
	}

	public void SetEnemyTeam(int[] value, DataContext context)
	{
		_enemyTeam = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(15, DataStates, CacheInfluences, context);
	}

	public int GetEnemyCharId()
	{
		return _enemyCharId;
	}

	public void SetEnemyCharId(int value, DataContext context)
	{
		_enemyCharId = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(16, DataStates, CacheInfluences, context);
	}

	public sbyte GetEnemyTeamWisdomType()
	{
		return _enemyTeamWisdomType;
	}

	public void SetEnemyTeamWisdomType(sbyte value, DataContext context)
	{
		_enemyTeamWisdomType = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(17, DataStates, CacheInfluences, context);
	}

	public short GetEnemyTeamWisdomCount()
	{
		return _enemyTeamWisdomCount;
	}

	public void SetEnemyTeamWisdomCount(short value, DataContext context)
	{
		_enemyTeamWisdomCount = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(18, DataStates, CacheInfluences, context);
	}

	public sbyte GetCombatStatus()
	{
		return _combatStatus;
	}

	public void SetCombatStatus(sbyte value, DataContext context)
	{
		_combatStatus = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(19, DataStates, CacheInfluences, context);
	}

	public sbyte GetShowMercyOption()
	{
		return _showMercyOption;
	}

	public void SetShowMercyOption(sbyte value, DataContext context)
	{
		_showMercyOption = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(20, DataStates, CacheInfluences, context);
	}

	public sbyte GetSelectedMercyOption()
	{
		return _selectedMercyOption;
	}

	public void SetSelectedMercyOption(sbyte value, DataContext context)
	{
		_selectedMercyOption = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(21, DataStates, CacheInfluences, context);
	}

	public int GetCarrierAnimalCombatCharId()
	{
		return _carrierAnimalCombatCharId;
	}

	public void SetCarrierAnimalCombatCharId(int value, DataContext context)
	{
		_carrierAnimalCombatCharId = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(22, DataStates, CacheInfluences, context);
	}

	public int GetSpecialShowCombatCharId()
	{
		return _specialShowCombatCharId;
	}

	public void SetSpecialShowCombatCharId(int value, DataContext context)
	{
		_specialShowCombatCharId = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(23, DataStates, CacheInfluences, context);
	}

	public IntPair GetSkillAttackedIndexAndHit()
	{
		return _skillAttackedIndexAndHit;
	}

	public void SetSkillAttackedIndexAndHit(IntPair value, DataContext context)
	{
		_skillAttackedIndexAndHit = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(24, DataStates, CacheInfluences, context);
	}

	public bool GetWaitingDelaySettlement()
	{
		return _waitingDelaySettlement;
	}

	public void SetWaitingDelaySettlement(bool value, DataContext context)
	{
		_waitingDelaySettlement = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(25, DataStates, CacheInfluences, context);
	}

	public SpecialMiscData GetShowUseGoldenWire()
	{
		return _showUseGoldenWire;
	}

	public void SetShowUseGoldenWire(SpecialMiscData value, DataContext context)
	{
		_showUseGoldenWire = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(26, DataStates, CacheInfluences, context);
	}

	public bool GetIsPuppetCombat()
	{
		return _isPuppetCombat;
	}

	public void SetIsPuppetCombat(bool value, DataContext context)
	{
		_isPuppetCombat = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(27, DataStates, CacheInfluences, context);
	}

	public bool GetIsPlaygroundCombat()
	{
		return _isPlaygroundCombat;
	}

	public void SetIsPlaygroundCombat(bool value, DataContext context)
	{
		_isPlaygroundCombat = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(28, DataStates, CacheInfluences, context);
	}

	public CombatSkillData GetElement_SkillDataDict(CombatSkillKey objectId)
	{
		return _skillDataDict[objectId];
	}

	public bool TryGetElement_SkillDataDict(CombatSkillKey objectId, out CombatSkillData element)
	{
		return _skillDataDict.TryGetValue(objectId, out element);
	}

	private void AddElement_SkillDataDict(CombatSkillKey objectId, CombatSkillData instance)
	{
		instance.CollectionHelperData = HelperDataSkillDataDict;
		instance.DataStatesOffset = _dataStatesSkillDataDict.Create();
		_skillDataDict.Add(objectId, instance);
	}

	private void RemoveElement_SkillDataDict(CombatSkillKey objectId)
	{
		if (_skillDataDict.TryGetValue(objectId, out var value))
		{
			_dataStatesSkillDataDict.Remove(value.DataStatesOffset);
			_skillDataDict.Remove(objectId);
		}
	}

	private void ClearSkillDataDict()
	{
		_dataStatesSkillDataDict.Clear();
		_skillDataDict.Clear();
	}

	public int GetElementField_SkillDataDict(CombatSkillKey objectId, ushort fieldId, RawDataPool dataPool, bool resetModified)
	{
		if (!_skillDataDict.TryGetValue(objectId, out var value))
		{
			AdaptableLog.TagWarning("GetElementField_SkillDataDict", $"Failed to find element {objectId} with field {fieldId}");
			return -1;
		}
		if (resetModified)
		{
			_dataStatesSkillDataDict.ResetModified(value.DataStatesOffset, fieldId);
		}
		switch (fieldId)
		{
		case 0:
			return GameData.Serializer.Serializer.Serialize(value.GetId(), dataPool);
		case 1:
			return GameData.Serializer.Serializer.Serialize(value.GetCanUse(), dataPool);
		case 2:
			return GameData.Serializer.Serializer.Serialize(value.GetLeftCdFrame(), dataPool);
		case 3:
			return GameData.Serializer.Serializer.Serialize(value.GetTotalCdFrame(), dataPool);
		case 4:
			return GameData.Serializer.Serializer.Serialize(value.GetConstAffecting(), dataPool);
		case 5:
			return GameData.Serializer.Serializer.Serialize(value.GetShowAffectTips(), dataPool);
		case 6:
			return GameData.Serializer.Serializer.Serialize(value.GetSilencing(), dataPool);
		case 7:
			return GameData.Serializer.Serializer.Serialize(value.GetBanReason(), dataPool);
		case 8:
			return GameData.Serializer.Serializer.Serialize(value.GetEffectData(), dataPool);
		case 9:
			return GameData.Serializer.Serializer.Serialize(value.GetCanAffect(), dataPool);
		default:
			if (fieldId >= 10)
			{
				throw new Exception($"Unsupported fieldId {fieldId}");
			}
			throw new Exception($"Not allow to get readonly field data: {fieldId}");
		}
	}

	public void SetElementField_SkillDataDict(CombatSkillKey objectId, ushort fieldId, int valueOffset, RawDataPool dataPool, DataContext context)
	{
		if (!_skillDataDict.TryGetValue(objectId, out var value))
		{
			throw new Exception($"Failed to find element {objectId} with field {fieldId}");
		}
		switch (fieldId)
		{
		case 0:
		{
			CombatSkillKey item4 = default(CombatSkillKey);
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item4);
			value.SetId(item4, context);
			return;
		}
		case 1:
		{
			bool item3 = false;
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item3);
			value.SetCanUse(item3, context);
			return;
		}
		case 2:
		{
			short item2 = 0;
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item2);
			value.SetLeftCdFrame(item2, context);
			return;
		}
		case 3:
		{
			short item = 0;
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item);
			value.SetTotalCdFrame(item, context);
			return;
		}
		case 4:
		{
			bool item7 = false;
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item7);
			value.SetConstAffecting(item7, context);
			return;
		}
		case 5:
		{
			bool item6 = false;
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item6);
			value.SetShowAffectTips(item6, context);
			return;
		}
		case 6:
		{
			bool item5 = false;
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item5);
			value.SetSilencing(item5, context);
			return;
		}
		}
		if (fieldId >= 10)
		{
			throw new Exception($"Unsupported fieldId {fieldId}");
		}
		if (fieldId >= 10)
		{
			throw new Exception($"Not allow to set readonly field data: {fieldId}");
		}
		throw new Exception($"Not allow to set cache field data: {fieldId}");
	}

	private int CheckModified_SkillDataDict(CombatSkillKey objectId, ushort fieldId, RawDataPool dataPool)
	{
		if (!_skillDataDict.TryGetValue(objectId, out var value))
		{
			return -1;
		}
		if (fieldId >= 10)
		{
			throw new Exception($"Not allow to check readonly field data: {fieldId}");
		}
		if (!_dataStatesSkillDataDict.IsModified(value.DataStatesOffset, fieldId))
		{
			return -1;
		}
		_dataStatesSkillDataDict.ResetModified(value.DataStatesOffset, fieldId);
		return fieldId switch
		{
			0 => GameData.Serializer.Serializer.Serialize(value.GetId(), dataPool), 
			1 => GameData.Serializer.Serializer.Serialize(value.GetCanUse(), dataPool), 
			2 => GameData.Serializer.Serializer.Serialize(value.GetLeftCdFrame(), dataPool), 
			3 => GameData.Serializer.Serializer.Serialize(value.GetTotalCdFrame(), dataPool), 
			4 => GameData.Serializer.Serializer.Serialize(value.GetConstAffecting(), dataPool), 
			5 => GameData.Serializer.Serializer.Serialize(value.GetShowAffectTips(), dataPool), 
			6 => GameData.Serializer.Serializer.Serialize(value.GetSilencing(), dataPool), 
			7 => GameData.Serializer.Serializer.Serialize(value.GetBanReason(), dataPool), 
			8 => GameData.Serializer.Serializer.Serialize(value.GetEffectData(), dataPool), 
			9 => GameData.Serializer.Serializer.Serialize(value.GetCanAffect(), dataPool), 
			_ => throw new Exception($"Unsupported fieldId {fieldId}"), 
		};
	}

	private void ResetModifiedWrapper_SkillDataDict(CombatSkillKey objectId, ushort fieldId)
	{
		if (_skillDataDict.TryGetValue(objectId, out var value))
		{
			if (fieldId >= 10)
			{
				throw new Exception($"Not allow to reset modification state of readonly field data: {fieldId}");
			}
			if (_dataStatesSkillDataDict.IsModified(value.DataStatesOffset, fieldId))
			{
				_dataStatesSkillDataDict.ResetModified(value.DataStatesOffset, fieldId);
			}
		}
	}

	private bool IsModifiedWrapper_SkillDataDict(CombatSkillKey objectId, ushort fieldId)
	{
		if (!_skillDataDict.TryGetValue(objectId, out var value))
		{
			return false;
		}
		if (fieldId >= 10)
		{
			throw new Exception($"Not allow to check modification state of readonly field data: {fieldId}");
		}
		return _dataStatesSkillDataDict.IsModified(value.DataStatesOffset, fieldId);
	}

	public CombatWeaponData GetElement_WeaponDataDict(int objectId)
	{
		return _weaponDataDict[objectId];
	}

	public bool TryGetElement_WeaponDataDict(int objectId, out CombatWeaponData element)
	{
		return _weaponDataDict.TryGetValue(objectId, out element);
	}

	private void AddElement_WeaponDataDict(int objectId, CombatWeaponData instance)
	{
		instance.CollectionHelperData = HelperDataWeaponDataDict;
		instance.DataStatesOffset = _dataStatesWeaponDataDict.Create();
		_weaponDataDict.Add(objectId, instance);
	}

	private void RemoveElement_WeaponDataDict(int objectId)
	{
		if (_weaponDataDict.TryGetValue(objectId, out var value))
		{
			_dataStatesWeaponDataDict.Remove(value.DataStatesOffset);
			_weaponDataDict.Remove(objectId);
		}
	}

	private void ClearWeaponDataDict()
	{
		_dataStatesWeaponDataDict.Clear();
		_weaponDataDict.Clear();
	}

	public int GetElementField_WeaponDataDict(int objectId, ushort fieldId, RawDataPool dataPool, bool resetModified)
	{
		if (!_weaponDataDict.TryGetValue(objectId, out var value))
		{
			AdaptableLog.TagWarning("GetElementField_WeaponDataDict", $"Failed to find element {objectId} with field {fieldId}");
			return -1;
		}
		if (resetModified)
		{
			_dataStatesWeaponDataDict.ResetModified(value.DataStatesOffset, fieldId);
		}
		switch (fieldId)
		{
		case 0:
			return GameData.Serializer.Serializer.Serialize(value.GetId(), dataPool);
		case 1:
			return GameData.Serializer.Serializer.Serialize(value.GetWeaponTricks(), dataPool);
		case 2:
			return GameData.Serializer.Serializer.Serialize(value.GetCanChangeTo(), dataPool);
		case 3:
			return GameData.Serializer.Serializer.Serialize(value.GetDurability(), dataPool);
		case 4:
			return GameData.Serializer.Serializer.Serialize(value.GetCdFrame(), dataPool);
		case 5:
			return GameData.Serializer.Serializer.Serialize(value.GetAutoAttackEffect(), dataPool);
		case 6:
			return GameData.Serializer.Serializer.Serialize(value.GetPestleEffect(), dataPool);
		case 7:
			return GameData.Serializer.Serializer.Serialize(value.GetFixedCdLeftFrame(), dataPool);
		case 8:
			return GameData.Serializer.Serializer.Serialize(value.GetFixedCdTotalFrame(), dataPool);
		case 9:
			return GameData.Serializer.Serializer.Serialize(value.GetInnerRatio(), dataPool);
		default:
			if (fieldId >= 10)
			{
				throw new Exception($"Unsupported fieldId {fieldId}");
			}
			throw new Exception($"Not allow to get readonly field data: {fieldId}");
		}
	}

	public void SetElementField_WeaponDataDict(int objectId, ushort fieldId, int valueOffset, RawDataPool dataPool, DataContext context)
	{
		if (!_weaponDataDict.TryGetValue(objectId, out var value))
		{
			throw new Exception($"Failed to find element {objectId} with field {fieldId}");
		}
		switch (fieldId)
		{
		case 0:
		{
			int item4 = 0;
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item4);
			value.SetId(item4, context);
			return;
		}
		case 1:
		{
			sbyte[] item3 = value.GetWeaponTricks();
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item3);
			value.SetWeaponTricks(item3, context);
			return;
		}
		case 2:
		{
			bool item2 = false;
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item2);
			value.SetCanChangeTo(item2, context);
			return;
		}
		case 3:
		{
			short item = 0;
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item);
			value.SetDurability(item, context);
			return;
		}
		case 4:
		{
			short item9 = 0;
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item9);
			value.SetCdFrame(item9, context);
			return;
		}
		case 5:
		{
			SkillEffectKey item8 = default(SkillEffectKey);
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item8);
			value.SetAutoAttackEffect(item8, context);
			return;
		}
		case 6:
		{
			SkillEffectKey item7 = default(SkillEffectKey);
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item7);
			value.SetPestleEffect(item7, context);
			return;
		}
		case 7:
		{
			short item6 = 0;
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item6);
			value.SetFixedCdLeftFrame(item6, context);
			return;
		}
		case 8:
		{
			short item5 = 0;
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item5);
			value.SetFixedCdTotalFrame(item5, context);
			return;
		}
		}
		if (fieldId >= 10)
		{
			throw new Exception($"Unsupported fieldId {fieldId}");
		}
		if (fieldId >= 10)
		{
			throw new Exception($"Not allow to set readonly field data: {fieldId}");
		}
		throw new Exception($"Not allow to set cache field data: {fieldId}");
	}

	private int CheckModified_WeaponDataDict(int objectId, ushort fieldId, RawDataPool dataPool)
	{
		if (!_weaponDataDict.TryGetValue(objectId, out var value))
		{
			return -1;
		}
		if (fieldId >= 10)
		{
			throw new Exception($"Not allow to check readonly field data: {fieldId}");
		}
		if (!_dataStatesWeaponDataDict.IsModified(value.DataStatesOffset, fieldId))
		{
			return -1;
		}
		_dataStatesWeaponDataDict.ResetModified(value.DataStatesOffset, fieldId);
		return fieldId switch
		{
			0 => GameData.Serializer.Serializer.Serialize(value.GetId(), dataPool), 
			1 => GameData.Serializer.Serializer.Serialize(value.GetWeaponTricks(), dataPool), 
			2 => GameData.Serializer.Serializer.Serialize(value.GetCanChangeTo(), dataPool), 
			3 => GameData.Serializer.Serializer.Serialize(value.GetDurability(), dataPool), 
			4 => GameData.Serializer.Serializer.Serialize(value.GetCdFrame(), dataPool), 
			5 => GameData.Serializer.Serializer.Serialize(value.GetAutoAttackEffect(), dataPool), 
			6 => GameData.Serializer.Serializer.Serialize(value.GetPestleEffect(), dataPool), 
			7 => GameData.Serializer.Serializer.Serialize(value.GetFixedCdLeftFrame(), dataPool), 
			8 => GameData.Serializer.Serializer.Serialize(value.GetFixedCdTotalFrame(), dataPool), 
			9 => GameData.Serializer.Serializer.Serialize(value.GetInnerRatio(), dataPool), 
			_ => throw new Exception($"Unsupported fieldId {fieldId}"), 
		};
	}

	private void ResetModifiedWrapper_WeaponDataDict(int objectId, ushort fieldId)
	{
		if (_weaponDataDict.TryGetValue(objectId, out var value))
		{
			if (fieldId >= 10)
			{
				throw new Exception($"Not allow to reset modification state of readonly field data: {fieldId}");
			}
			if (_dataStatesWeaponDataDict.IsModified(value.DataStatesOffset, fieldId))
			{
				_dataStatesWeaponDataDict.ResetModified(value.DataStatesOffset, fieldId);
			}
		}
	}

	private bool IsModifiedWrapper_WeaponDataDict(int objectId, ushort fieldId)
	{
		if (!_weaponDataDict.TryGetValue(objectId, out var value))
		{
			return false;
		}
		if (fieldId >= 10)
		{
			throw new Exception($"Not allow to check modification state of readonly field data: {fieldId}");
		}
		return _dataStatesWeaponDataDict.IsModified(value.DataStatesOffset, fieldId);
	}

	public WeaponExpectInnerRatioData GetExpectRatioData()
	{
		return _expectRatioData;
	}

	public void SetExpectRatioData(WeaponExpectInnerRatioData value, DataContext context)
	{
		_expectRatioData = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(31, DataStates, CacheInfluences, context);
	}

	public List<int> GetTaiwuSpecialGroupCharIds()
	{
		return _taiwuSpecialGroupCharIds;
	}

	public void SetTaiwuSpecialGroupCharIds(List<int> value, DataContext context)
	{
		_taiwuSpecialGroupCharIds = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(32, DataStates, CacheInfluences, context);
	}

	public override void OnInitializeGameDataModule()
	{
		InitializeOnInitializeGameDataModule();
	}

	public override void OnEnterNewWorld()
	{
		InitializeOnEnterNewWorld();
		InitializeInternalDataOfCollections();
	}

	public override void OnLoadWorld()
	{
		InitializeInternalDataOfCollections();
		OnLoadedArchiveData();
		DomainManager.Global.CompleteLoading(8);
	}

	public override int GetData(ushort dataId, ulong subId0, uint subId1, RawDataPool dataPool, bool resetModified)
	{
		switch (dataId)
		{
		case 0:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 0);
			}
			return GameData.Serializer.Serializer.Serialize(_timeScale, dataPool);
		case 1:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 1);
			}
			return GameData.Serializer.Serializer.Serialize(_autoCombat, dataPool);
		case 2:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 2);
			}
			return GameData.Serializer.Serializer.Serialize(_combatFrame, dataPool);
		case 3:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 3);
			}
			return GameData.Serializer.Serializer.Serialize(_combatType, dataPool);
		case 4:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 4);
			}
			return GameData.Serializer.Serializer.Serialize(_currentDistance, dataPool);
		case 5:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 5);
			}
			return GameData.Serializer.Serializer.Serialize(_damageCompareData, dataPool);
		case 6:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 6);
				_modificationsSkillPowerAddInCombat.Reset();
			}
			return GameData.Serializer.Serializer.SerializeModifications((IDictionary<CombatSkillKey, SkillPowerChangeCollection>)_skillPowerAddInCombat, dataPool);
		case 7:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 7);
				_modificationsSkillPowerReduceInCombat.Reset();
			}
			return GameData.Serializer.Serializer.SerializeModifications((IDictionary<CombatSkillKey, SkillPowerChangeCollection>)_skillPowerReduceInCombat, dataPool);
		case 8:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 8);
				_modificationsSkillPowerReplaceInCombat.Reset();
			}
			return GameData.Serializer.Serializer.SerializeModifications((IDictionary<CombatSkillKey, CombatSkillKey>)_skillPowerReplaceInCombat, dataPool);
		case 9:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 9);
			}
			return GameData.Serializer.Serializer.Serialize(_bgmIndex, dataPool);
		case 10:
			return GetElementField_CombatCharacterDict((int)subId0, (ushort)subId1, dataPool, resetModified);
		case 11:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 11);
			}
			return GameData.Serializer.Serializer.Serialize(_selfTeam, dataPool);
		case 12:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 12);
			}
			return GameData.Serializer.Serializer.Serialize(_selfCharId, dataPool);
		case 13:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 13);
			}
			return GameData.Serializer.Serializer.Serialize(_selfTeamWisdomType, dataPool);
		case 14:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 14);
			}
			return GameData.Serializer.Serializer.Serialize(_selfTeamWisdomCount, dataPool);
		case 15:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 15);
			}
			return GameData.Serializer.Serializer.Serialize(_enemyTeam, dataPool);
		case 16:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 16);
			}
			return GameData.Serializer.Serializer.Serialize(_enemyCharId, dataPool);
		case 17:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 17);
			}
			return GameData.Serializer.Serializer.Serialize(_enemyTeamWisdomType, dataPool);
		case 18:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 18);
			}
			return GameData.Serializer.Serializer.Serialize(_enemyTeamWisdomCount, dataPool);
		case 19:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 19);
			}
			return GameData.Serializer.Serializer.Serialize(_combatStatus, dataPool);
		case 20:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 20);
			}
			return GameData.Serializer.Serializer.Serialize(_showMercyOption, dataPool);
		case 21:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 21);
			}
			return GameData.Serializer.Serializer.Serialize(_selectedMercyOption, dataPool);
		case 22:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 22);
			}
			return GameData.Serializer.Serializer.Serialize(_carrierAnimalCombatCharId, dataPool);
		case 23:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 23);
			}
			return GameData.Serializer.Serializer.Serialize(_specialShowCombatCharId, dataPool);
		case 24:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 24);
			}
			return GameData.Serializer.Serializer.Serialize(_skillAttackedIndexAndHit, dataPool);
		case 25:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 25);
			}
			return GameData.Serializer.Serializer.Serialize(_waitingDelaySettlement, dataPool);
		case 26:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 26);
			}
			return GameData.Serializer.Serializer.Serialize(_showUseGoldenWire, dataPool);
		case 27:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 27);
			}
			return GameData.Serializer.Serializer.Serialize(_isPuppetCombat, dataPool);
		case 28:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 28);
			}
			return GameData.Serializer.Serializer.Serialize(_isPlaygroundCombat, dataPool);
		case 29:
			return GetElementField_SkillDataDict((CombatSkillKey)subId0, (ushort)subId1, dataPool, resetModified);
		case 30:
			return GetElementField_WeaponDataDict((int)subId0, (ushort)subId1, dataPool, resetModified);
		case 31:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 31);
			}
			return GameData.Serializer.Serializer.Serialize(_expectRatioData, dataPool);
		case 32:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 32);
			}
			return GameData.Serializer.Serializer.Serialize(_taiwuSpecialGroupCharIds, dataPool);
		default:
			throw new Exception($"Unsupported dataId {dataId}");
		}
	}

	public override void SetData(ushort dataId, ulong subId0, uint subId1, int valueOffset, RawDataPool dataPool, DataContext context)
	{
		switch (dataId)
		{
		case 0:
			throw new Exception($"Not allow to set value of dataId {dataId}");
		case 1:
			throw new Exception($"Not allow to set value of dataId {dataId}");
		case 2:
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref _combatFrame);
			SetCombatFrame(_combatFrame, context);
			break;
		case 3:
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref _combatType);
			SetCombatType(_combatType, context);
			break;
		case 4:
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref _currentDistance);
			SetCurrentDistance(_currentDistance, context);
			break;
		case 5:
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref _damageCompareData);
			SetDamageCompareData(_damageCompareData, context);
			break;
		case 6:
			throw new Exception($"Not allow to set value of dataId {dataId}");
		case 7:
			throw new Exception($"Not allow to set value of dataId {dataId}");
		case 8:
			throw new Exception($"Not allow to set value of dataId {dataId}");
		case 9:
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref _bgmIndex);
			SetBgmIndex(_bgmIndex, context);
			break;
		case 10:
			SetElementField_CombatCharacterDict((int)subId0, (ushort)subId1, valueOffset, dataPool, context);
			break;
		case 11:
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref _selfTeam);
			SetSelfTeam(_selfTeam, context);
			break;
		case 12:
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref _selfCharId);
			SetSelfCharId(_selfCharId, context);
			break;
		case 13:
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref _selfTeamWisdomType);
			SetSelfTeamWisdomType(_selfTeamWisdomType, context);
			break;
		case 14:
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref _selfTeamWisdomCount);
			SetSelfTeamWisdomCount(_selfTeamWisdomCount, context);
			break;
		case 15:
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref _enemyTeam);
			SetEnemyTeam(_enemyTeam, context);
			break;
		case 16:
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref _enemyCharId);
			SetEnemyCharId(_enemyCharId, context);
			break;
		case 17:
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref _enemyTeamWisdomType);
			SetEnemyTeamWisdomType(_enemyTeamWisdomType, context);
			break;
		case 18:
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref _enemyTeamWisdomCount);
			SetEnemyTeamWisdomCount(_enemyTeamWisdomCount, context);
			break;
		case 19:
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref _combatStatus);
			SetCombatStatus(_combatStatus, context);
			break;
		case 20:
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref _showMercyOption);
			SetShowMercyOption(_showMercyOption, context);
			break;
		case 21:
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref _selectedMercyOption);
			SetSelectedMercyOption(_selectedMercyOption, context);
			break;
		case 22:
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref _carrierAnimalCombatCharId);
			SetCarrierAnimalCombatCharId(_carrierAnimalCombatCharId, context);
			break;
		case 23:
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref _specialShowCombatCharId);
			SetSpecialShowCombatCharId(_specialShowCombatCharId, context);
			break;
		case 24:
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref _skillAttackedIndexAndHit);
			SetSkillAttackedIndexAndHit(_skillAttackedIndexAndHit, context);
			break;
		case 25:
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref _waitingDelaySettlement);
			SetWaitingDelaySettlement(_waitingDelaySettlement, context);
			break;
		case 26:
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref _showUseGoldenWire);
			SetShowUseGoldenWire(_showUseGoldenWire, context);
			break;
		case 27:
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref _isPuppetCombat);
			SetIsPuppetCombat(_isPuppetCombat, context);
			break;
		case 28:
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref _isPlaygroundCombat);
			SetIsPlaygroundCombat(_isPlaygroundCombat, context);
			break;
		case 29:
			SetElementField_SkillDataDict((CombatSkillKey)subId0, (ushort)subId1, valueOffset, dataPool, context);
			break;
		case 30:
			SetElementField_WeaponDataDict((int)subId0, (ushort)subId1, valueOffset, dataPool, context);
			break;
		case 31:
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref _expectRatioData);
			SetExpectRatioData(_expectRatioData, context);
			break;
		case 32:
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref _taiwuSpecialGroupCharIds);
			SetTaiwuSpecialGroupCharIds(_taiwuSpecialGroupCharIds, context);
			break;
		default:
			throw new Exception($"Unsupported dataId {dataId}");
		}
	}

	public override int CallMethod(Operation operation, RawDataPool argDataPool, RawDataPool returnDataPool, DataContext context)
	{
		int argsOffset = operation.ArgsOffset;
		switch (operation.MethodId)
		{
		case 0:
		{
			int argsCount36 = operation.ArgsCount;
			int num36 = argsCount36;
			if (num36 == 1)
			{
				bool item137 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item137);
				PlayMoveStepSound(context, item137);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 1:
		{
			int argsCount62 = operation.ArgsCount;
			int num62 = argsCount62;
			if (num62 == 3)
			{
				bool item212 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item212);
				int item213 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item213);
				int item214 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item214);
				bool item215 = ExecuteTeammateCommand(context, item212, item213, item214);
				return GameData.Serializer.Serializer.Serialize(item215, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 2:
		{
			int argsCount44 = operation.ArgsCount;
			int num44 = argsCount44;
			if (num44 == 2)
			{
				int item161 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item161);
				int item162 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item162);
				RemoveTeammateCommand(context, item161, item162);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 3:
		{
			int argsCount59 = operation.ArgsCount;
			int num59 = argsCount59;
			if (num59 == 1)
			{
				int item209 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item209);
				CombatCharacterDisplayData combatCharDisplayData = GetCombatCharDisplayData(context, item209);
				return GameData.Serializer.Serializer.Serialize(combatCharDisplayData, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 4:
		{
			int argsCount21 = operation.ArgsCount;
			int num21 = argsCount21;
			if (num21 == 2)
			{
				bool item68 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item68);
				bool item69 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item69);
				SelectMercyOption(context, item68, item69);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 5:
			switch (operation.ArgsCount)
			{
			case 1:
			{
				int item58 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item58);
				ChangeWeapon(context, item58);
				return -1;
			}
			case 2:
			{
				int item56 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item56);
				bool item57 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item57);
				ChangeWeapon(context, item56, item57);
				return -1;
			}
			case 3:
			{
				int item53 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item53);
				bool item54 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item54);
				bool item55 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item55);
				ChangeWeapon(context, item53, item54, item55);
				return -1;
			}
			default:
				throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
			}
		case 6:
			switch (operation.ArgsCount)
			{
			case 0:
				NormalAttack(context);
				return -1;
			case 1:
			{
				bool item39 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item39);
				NormalAttack(context, item39);
				return -1;
			}
			default:
				throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
			}
		case 7:
			switch (operation.ArgsCount)
			{
			case 0:
				StartChangeTrick(context);
				return -1;
			case 1:
			{
				bool item14 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item14);
				StartChangeTrick(context, item14);
				return -1;
			}
			default:
				throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
			}
		case 8:
		{
			int argsCount48 = operation.ArgsCount;
			int num48 = argsCount48;
			if (num48 == 3)
			{
				sbyte item172 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item172);
				sbyte item173 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item173);
				int item174 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item174);
				SelectChangeTrick(context, item172, item173, item174);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 9:
		{
			int argsCount35 = operation.ArgsCount;
			int num35 = argsCount35;
			if (num35 == 2)
			{
				int item135 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item135);
				sbyte item136 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item136);
				ChangeTaiwuWeaponInnerRatio(context, item135, item136);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 10:
		{
			int argsCount12 = operation.ArgsCount;
			int num12 = argsCount12;
			if (num12 == 1)
			{
				ItemKey item44 = default(ItemKey);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item44);
				sbyte weaponInnerRatio = GetWeaponInnerRatio(context, item44);
				return GameData.Serializer.Serializer.Serialize(weaponInnerRatio, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 11:
		{
			int argsCount65 = operation.ArgsCount;
			int num65 = argsCount65;
			if (num65 == 1)
			{
				ItemKey item223 = default(ItemKey);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item223);
				WeaponEffectDisplayData[] weaponEffects = GetWeaponEffects(item223);
				return GameData.Serializer.Serializer.Serialize(weaponEffects, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 12:
			switch (operation.ArgsCount)
			{
			case 1:
			{
				sbyte item220 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item220);
				StartPrepareOtherAction(context, item220);
				return -1;
			}
			case 2:
			{
				sbyte item218 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item218);
				bool item219 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item219);
				StartPrepareOtherAction(context, item218, item219);
				return -1;
			}
			default:
				throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
			}
		case 13:
		{
			int argsCount56 = operation.ArgsCount;
			int num56 = argsCount56;
			if (num56 == 1)
			{
				int item205 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item205);
				List<CombatSkillDisplayData> proactiveSkillList = GetProactiveSkillList(item205);
				return GameData.Serializer.Serializer.Serialize(proactiveSkillList, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 14:
			switch (operation.ArgsCount)
			{
			case 1:
			{
				short item189 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item189);
				StartPrepareSkill(context, item189);
				return -1;
			}
			case 2:
			{
				short item187 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item187);
				bool item188 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item188);
				StartPrepareSkill(context, item187, item188);
				return -1;
			}
			default:
				throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
			}
		case 15:
			if (operation.ArgsCount == 0)
			{
				GmCmd_ForceRecoverBreathAndStance(context);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		case 16:
		{
			int argsCount31 = operation.ArgsCount;
			int num31 = argsCount31;
			if (num31 == 2)
			{
				bool item125 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item125);
				sbyte item126 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item126);
				GmCmd_AddTrick(context, item125, item126);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 17:
			switch (operation.ArgsCount)
			{
			case 3:
			{
				bool item108 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item108);
				sbyte item109 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item109);
				bool item110 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item110);
				GmCmd_AddInjury(context, item108, item109, item110);
				return -1;
			}
			case 4:
			{
				bool item104 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item104);
				sbyte item105 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item105);
				bool item106 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item106);
				int item107 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item107);
				GmCmd_AddInjury(context, item104, item105, item106, item107);
				return -1;
			}
			case 5:
			{
				bool item99 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item99);
				sbyte item100 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item100);
				bool item101 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item101);
				int item102 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item102);
				bool item103 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item103);
				GmCmd_AddInjury(context, item99, item100, item101, item102, item103);
				return -1;
			}
			default:
				throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
			}
		case 18:
			switch (operation.ArgsCount)
			{
			case 0:
				GmCmd_ForceHealAllInjury(context);
				return -1;
			case 1:
			{
				bool item92 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item92);
				GmCmd_ForceHealAllInjury(context, item92);
				return -1;
			}
			default:
				throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
			}
		case 19:
			switch (operation.ArgsCount)
			{
			case 2:
			{
				bool item87 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item87);
				sbyte item88 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item88);
				GmCmd_AddPoison(context, item87, item88);
				return -1;
			}
			case 3:
			{
				bool item84 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item84);
				sbyte item85 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item85);
				int item86 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item86);
				GmCmd_AddPoison(context, item84, item85, item86);
				return -1;
			}
			case 4:
			{
				bool item80 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item80);
				sbyte item81 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item81);
				int item82 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item82);
				bool item83 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item83);
				GmCmd_AddPoison(context, item80, item81, item82, item83);
				return -1;
			}
			default:
				throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
			}
		case 20:
			switch (operation.ArgsCount)
			{
			case 0:
				GmCmd_ForceHealAllPoison(context);
				return -1;
			case 1:
			{
				bool item73 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item73);
				GmCmd_ForceHealAllPoison(context, item73);
				return -1;
			}
			default:
				throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
			}
		case 21:
		{
			int argsCount16 = operation.ArgsCount;
			int num16 = argsCount16;
			if (num16 == 1)
			{
				short item52 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item52);
				GmCmd_ForceEnemyUseSkill(context, item52);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 22:
		{
			int argsCount5 = operation.ArgsCount;
			int num5 = argsCount5;
			if (num5 == 1)
			{
				sbyte item16 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item16);
				GmCmd_ForceEnemyUseOtherAction(context, item16);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 23:
			if (operation.ArgsCount == 0)
			{
				GmCmd_ForceEnemyDefeat(context);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		case 24:
			if (operation.ArgsCount == 0)
			{
				GmCmd_ForceSelfDefeat(context);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		case 25:
		{
			int argsCount54 = operation.ArgsCount;
			int num54 = argsCount54;
			if (num54 == 2)
			{
				bool item200 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item200);
				short[] item201 = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item201);
				GmCmd_SetNeiliAllocation(context, item200, item201);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 26:
			switch (operation.ArgsCount)
			{
			case 2:
			{
				bool item193 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item193);
				sbyte item194 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item194);
				GmCmd_AddFlaw(context, item193, item194);
				return -1;
			}
			case 3:
			{
				bool item190 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item190);
				sbyte item191 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item191);
				int item192 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item192);
				GmCmd_AddFlaw(context, item190, item191, item192);
				return -1;
			}
			default:
				throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
			}
		case 27:
		{
			int argsCount47 = operation.ArgsCount;
			int num47 = argsCount47;
			if (num47 == 1)
			{
				bool item171 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item171);
				GmCmd_HealAllFlaw(context, item171);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 28:
			switch (operation.ArgsCount)
			{
			case 2:
			{
				bool item167 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item167);
				sbyte item168 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item168);
				GmCmd_AddAcupoint(context, item167, item168);
				return -1;
			}
			case 3:
			{
				bool item164 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item164);
				sbyte item165 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item165);
				int item166 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item166);
				GmCmd_AddAcupoint(context, item164, item165, item166);
				return -1;
			}
			default:
				throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
			}
		case 29:
		{
			int argsCount39 = operation.ArgsCount;
			int num39 = argsCount39;
			if (num39 == 1)
			{
				bool item142 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item142);
				GmCmd_HealAllAcupoint(context, item142);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 30:
		{
			int argsCount32 = operation.ArgsCount;
			int num32 = argsCount32;
			if (num32 == 1)
			{
				short item127 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item127);
				GmCmd_FightBoss(context, item127);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 31:
		{
			int argsCount27 = operation.ArgsCount;
			int num27 = argsCount27;
			if (num27 == 1)
			{
				short item98 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item98);
				GmCmd_FightAnimal(context, item98);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 32:
		{
			int argsCount23 = operation.ArgsCount;
			int num23 = argsCount23;
			if (num23 == 1)
			{
				bool item74 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item74);
				GmCmd_EnableEnemyAi(context, item74);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 33:
		{
			int argsCount19 = operation.ArgsCount;
			int num19 = argsCount19;
			if (num19 == 1)
			{
				bool item65 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item65);
				GmCmd_EnableSkillFreeCast(context, item65);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 34:
		{
			int argsCount14 = operation.ArgsCount;
			int num14 = argsCount14;
			if (num14 == 2)
			{
				int item46 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item46);
				int item47 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item47);
				uint healInjuryBanReason = GetHealInjuryBanReason(item46, item47);
				return GameData.Serializer.Serializer.Serialize(healInjuryBanReason, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 35:
		{
			int argsCount9 = operation.ArgsCount;
			int num9 = argsCount9;
			if (num9 == 2)
			{
				int item37 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item37);
				int item38 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item38);
				uint healPoisonBanReason = GetHealPoisonBanReason(item37, item38);
				return GameData.Serializer.Serializer.Serialize(healPoisonBanReason, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 36:
			switch (operation.ArgsCount)
			{
			case 1:
			{
				ItemKey item26 = default(ItemKey);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item26);
				UseItem(context, item26, -1);
				return -1;
			}
			case 2:
			{
				ItemKey item24 = default(ItemKey);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item24);
				sbyte item25 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item25);
				UseItem(context, item24, item25);
				return -1;
			}
			case 3:
			{
				ItemKey item21 = default(ItemKey);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item21);
				sbyte item22 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item22);
				bool item23 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item23);
				UseItem(context, item21, item22, item23);
				return -1;
			}
			case 4:
			{
				ItemKey item17 = default(ItemKey);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item17);
				sbyte item18 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item18);
				bool item19 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item19);
				List<sbyte> item20 = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item20);
				UseItem(context, item17, item18, item19, item20);
				return -1;
			}
			default:
				throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
			}
		case 37:
		{
			int argsCount2 = operation.ArgsCount;
			int num2 = argsCount2;
			if (num2 == 2)
			{
				short item9 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item9);
				int[] item10 = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item10);
				sbyte item11 = PrepareCombat(context, item9, item10);
				return GameData.Serializer.Serializer.Serialize(item11, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 38:
			if (operation.ArgsCount == 0)
			{
				bool item221 = StartCombat(context);
				return GameData.Serializer.Serializer.Serialize(item221, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		case 39:
		{
			int argsCount61 = operation.ArgsCount;
			int num61 = argsCount61;
			if (num61 == 1)
			{
				float item211 = 0f;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item211);
				SetTimeScale(context, item211);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 40:
		{
			int argsCount57 = operation.ArgsCount;
			int num57 = argsCount57;
			if (num57 == 1)
			{
				bool item206 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item206);
				SetPlayerAutoCombat(context, item206);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 41:
		{
			int argsCount53 = operation.ArgsCount;
			int num53 = argsCount53;
			if (num53 == 1)
			{
				bool item199 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item199);
				TestAiData aiTestData = GetAiTestData(item199);
				return GameData.Serializer.Serializer.Serialize(aiTestData, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 42:
		{
			int argsCount50 = operation.ArgsCount;
			int num50 = argsCount50;
			if (num50 == 1)
			{
				AiOptions item185 = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item185);
				SetAiOptions(item185);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 43:
			switch (operation.ArgsCount)
			{
			case 1:
			{
				byte item184 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item184);
				SetMoveState(item184);
				return -1;
			}
			case 2:
			{
				byte item182 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item182);
				bool item183 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item183);
				SetMoveState(item182, item183);
				return -1;
			}
			case 3:
			{
				byte item179 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item179);
				bool item180 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item180);
				bool item181 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item181);
				SetMoveState(item179, item180, item181);
				return -1;
			}
			default:
				throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
			}
		case 44:
			if (operation.ArgsCount == 0)
			{
				CombatResultDisplayData combatResultDisplayData = GetCombatResultDisplayData();
				return GameData.Serializer.Serializer.Serialize(combatResultDisplayData, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		case 45:
		{
			int argsCount43 = operation.ArgsCount;
			int num43 = argsCount43;
			if (num43 == 2)
			{
				List<ItemKey> item159 = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item159);
				List<int> item160 = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item160);
				SelectGetItem(context, item159, item160);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 46:
			switch (operation.ArgsCount)
			{
			case 0:
				Surrender(context);
				return -1;
			case 1:
			{
				bool item156 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item156);
				Surrender(context, item156);
				return -1;
			}
			default:
				throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
			}
		case 47:
			switch (operation.ArgsCount)
			{
			case 2:
			{
				short item154 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item154);
				sbyte item155 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item155);
				EnterBossPuppetCombat(context, item154, item155);
				return -1;
			}
			case 3:
			{
				short item151 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item151);
				sbyte item152 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item152);
				bool item153 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item153);
				EnterBossPuppetCombat(context, item151, item152, item153);
				return -1;
			}
			default:
				throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
			}
		case 48:
			switch (operation.ArgsCount)
			{
			case 2:
			{
				ItemKey item148 = default(ItemKey);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item148);
				ItemKey item149 = default(ItemKey);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item149);
				RepairItem(context, item148, item149);
				return -1;
			}
			case 3:
			{
				ItemKey item145 = default(ItemKey);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item145);
				ItemKey item146 = default(ItemKey);
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item146);
				bool item147 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item147);
				RepairItem(context, item145, item146, item147);
				return -1;
			}
			default:
				throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
			}
		case 49:
		{
			int argsCount38 = operation.ArgsCount;
			int num38 = argsCount38;
			if (num38 == 2)
			{
				short item140 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item140);
				List<int> item141 = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item141);
				PrepareEnemyEquipments(context, item140, item141);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 50:
		{
			int argsCount33 = operation.ArgsCount;
			int num33 = argsCount33;
			if (num33 == 1)
			{
				bool item128 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item128);
				EnableBulletTime(context, item128);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 51:
		{
			int argsCount30 = operation.ArgsCount;
			int num30 = argsCount30;
			if (num30 == 2)
			{
				bool item123 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item123);
				bool item124 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item124);
				GmCmd_SetImmortal(context, item123, item124);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 52:
			switch (operation.ArgsCount)
			{
			case 0:
				CancelChangeTrick(context);
				return -1;
			case 1:
			{
				bool item114 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item114);
				CancelChangeTrick(context, item114);
				return -1;
			}
			default:
				throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
			}
		case 53:
			switch (operation.ArgsCount)
			{
			case 0:
				ClearAllReserveAction(context);
				return -1;
			case 1:
			{
				bool item113 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item113);
				ClearAllReserveAction(context, item113);
				return -1;
			}
			default:
				throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
			}
		case 54:
			if (operation.ArgsCount == 0)
			{
				InterruptSurrender();
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		case 55:
			if (operation.ArgsCount == 0)
			{
				bool item79 = IsInCombat();
				return GameData.Serializer.Serializer.Serialize(item79, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		case 56:
		{
			int argsCount22 = operation.ArgsCount;
			int num22 = argsCount22;
			if (num22 == 2)
			{
				short item70 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item70);
				int item71 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item71);
				GmCmd_FightTestOrgMember(context, item70, item71);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 57:
		{
			int argsCount18 = operation.ArgsCount;
			int num18 = argsCount18;
			if (num18 == 2)
			{
				short item63 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item63);
				sbyte item64 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item64);
				GmCmd_FightRandomEnemy(context, item63, item64);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 58:
			if (operation.ArgsCount == 0)
			{
				GmCmd_ForceRecoverMobilityValue(context);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		case 59:
		{
			int argsCount11 = operation.ArgsCount;
			int num11 = argsCount11;
			if (num11 == 1)
			{
				bool item43 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item43);
				GmCmd_UnitTestSetDistanceToTarget(context, item43);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 60:
		{
			int argsCount7 = operation.ArgsCount;
			int num7 = argsCount7;
			if (num7 == 3)
			{
				int item30 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item30);
				short item31 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item31);
				bool item32 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item32);
				bool item33 = GmCmd_UnitTestEquipSkill(context, item30, item31, item32);
				return GameData.Serializer.Serializer.Serialize(item33, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 61:
			switch (operation.ArgsCount)
			{
			case 0:
				GmCmd_UnitTestPrepare(context);
				return -1;
			case 1:
			{
				bool item29 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item29);
				GmCmd_UnitTestPrepare(context, item29);
				return -1;
			}
			default:
				throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
			}
		case 62:
		{
			int argsCount4 = operation.ArgsCount;
			int num4 = argsCount4;
			if (num4 == 1)
			{
				int item15 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item15);
				GmCmd_UnitTestClearAllEquipSkill(context, item15);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 63:
		{
			int argsCount = operation.ArgsCount;
			int num = argsCount;
			if (num == 1)
			{
				int item8 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item8);
				DamageStepDisplayData fatalDamageStepDisplayData = GetFatalDamageStepDisplayData(item8);
				return GameData.Serializer.Serializer.Serialize(fatalDamageStepDisplayData, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 64:
		{
			int argsCount64 = operation.ArgsCount;
			int num64 = argsCount64;
			if (num64 == 1)
			{
				int item222 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item222);
				DamageStepDisplayData mindDamageStepDisplayData = GetMindDamageStepDisplayData(item222);
				return GameData.Serializer.Serializer.Serialize(mindDamageStepDisplayData, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 65:
		{
			int argsCount63 = operation.ArgsCount;
			int num63 = argsCount63;
			if (num63 == 2)
			{
				int item216 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item216);
				sbyte item217 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item217);
				OuterAndInnerDamageStepDisplayData bodyPartDamageStepDisplayData = GetBodyPartDamageStepDisplayData(item216, item217);
				return GameData.Serializer.Serializer.Serialize(bodyPartDamageStepDisplayData, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 66:
		{
			int argsCount60 = operation.ArgsCount;
			int num60 = argsCount60;
			if (num60 == 1)
			{
				int item210 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item210);
				CompleteDamageStepDisplayData completeDamageStepDisplayData = GetCompleteDamageStepDisplayData(item210);
				return GameData.Serializer.Serializer.Serialize(completeDamageStepDisplayData, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 67:
		{
			int argsCount58 = operation.ArgsCount;
			int num58 = argsCount58;
			if (num58 == 2)
			{
				bool item207 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item207);
				short item208 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item208);
				GmCmd_ForceRecoverWugCount(context, item207, item208);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 68:
		{
			int argsCount55 = operation.ArgsCount;
			int num55 = argsCount55;
			if (num55 == 2)
			{
				int item203 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item203);
				short item204 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item204);
				GmCmd_FightCharacter(context, item203, item204);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 69:
			switch (operation.ArgsCount)
			{
			case 0:
			{
				ChangeTrickDisplayData changeTrickDisplayData2 = GetChangeTrickDisplayData();
				return GameData.Serializer.Serializer.Serialize(changeTrickDisplayData2, returnDataPool);
			}
			case 1:
			{
				bool item202 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item202);
				ChangeTrickDisplayData changeTrickDisplayData = GetChangeTrickDisplayData(item202);
				return GameData.Serializer.Serializer.Serialize(changeTrickDisplayData, returnDataPool);
			}
			default:
				throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
			}
		case 70:
		{
			int argsCount52 = operation.ArgsCount;
			int num52 = argsCount52;
			if (num52 == 1)
			{
				bool item198 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item198);
				ClearAffectingDefenseSkillManual(context, item198);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 71:
			switch (operation.ArgsCount)
			{
			case 0:
			{
				bool item197 = ClearDefendInBlockAttackSkill(context);
				return GameData.Serializer.Serializer.Serialize(item197, returnDataPool);
			}
			case 1:
			{
				bool item195 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item195);
				bool item196 = ClearDefendInBlockAttackSkill(context, item195);
				return GameData.Serializer.Serializer.Serialize(item196, returnDataPool);
			}
			default:
				throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
			}
		case 72:
		{
			int argsCount51 = operation.ArgsCount;
			int num51 = argsCount51;
			if (num51 == 1)
			{
				bool item186 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item186);
				GmCmd_HealAllFatal(context, item186);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 73:
		{
			int argsCount49 = operation.ArgsCount;
			int num49 = argsCount49;
			if (num49 == 1)
			{
				bool item178 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item178);
				GmCmd_HealAllDefeatMark(context, item178);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 74:
			switch (operation.ArgsCount)
			{
			case 1:
			{
				bool item177 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item177);
				GmCmd_AddAllDefeatMark(context, item177);
				return -1;
			}
			case 2:
			{
				bool item175 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item175);
				int item176 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item176);
				GmCmd_AddAllDefeatMark(context, item175, item176);
				return -1;
			}
			default:
				throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
			}
		case 75:
		{
			int argsCount46 = operation.ArgsCount;
			int num46 = argsCount46;
			if (num46 == 2)
			{
				bool item169 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item169);
				int item170 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item170);
				GmCmd_AddFatal(context, item169, item170);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 76:
		{
			int argsCount45 = operation.ArgsCount;
			int num45 = argsCount45;
			if (num45 == 1)
			{
				bool item163 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item163);
				GmCmd_HealAllDie(context, item163);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 77:
		{
			int argsCount42 = operation.ArgsCount;
			int num42 = argsCount42;
			if (num42 == 2)
			{
				bool item157 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item157);
				int item158 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item158);
				GmCmd_AddDie(context, item157, item158);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 78:
		{
			int argsCount41 = operation.ArgsCount;
			int num41 = argsCount41;
			if (num41 == 1)
			{
				bool item150 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item150);
				GmCmd_HealAllMind(context, item150);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 79:
		{
			int argsCount40 = operation.ArgsCount;
			int num40 = argsCount40;
			if (num40 == 2)
			{
				bool item143 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item143);
				bool item144 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item144);
				GmCmd_HealInjury(context, item143, item144);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 80:
		{
			int argsCount37 = operation.ArgsCount;
			int num37 = argsCount37;
			if (num37 == 2)
			{
				bool item138 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item138);
				int item139 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item139);
				GmCmd_AddMind(context, item138, item139);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 81:
		{
			int argsCount34 = operation.ArgsCount;
			int num34 = argsCount34;
			if (num34 == 2)
			{
				short item132 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item132);
				int[] item133 = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item133);
				int item134 = SimulatePrepareCombat(context, item132, item133);
				return GameData.Serializer.Serializer.Serialize(item134, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 82:
			switch (operation.ArgsCount)
			{
			case 1:
			{
				short item131 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item131);
				SetTargetDistance(context, item131);
				return -1;
			}
			case 2:
			{
				short item129 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item129);
				bool item130 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item130);
				SetTargetDistance(context, item129, item130);
				return -1;
			}
			default:
				throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
			}
		case 83:
			if (operation.ArgsCount == 0)
			{
				ClearTargetDistance(context);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		case 84:
		{
			int argsCount29 = operation.ArgsCount;
			int num29 = argsCount29;
			if (num29 == 2)
			{
				short item120 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item120);
				short item121 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item121);
				bool item122 = SetJumpThreshold(context, item120, item121);
				return GameData.Serializer.Serializer.Serialize(item122, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 85:
			switch (operation.ArgsCount)
			{
			case 2:
			{
				int item118 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item118);
				short item119 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item119);
				OuterAndInnerShorts previewAttackRange2 = GetPreviewAttackRange(item118, item119);
				return GameData.Serializer.Serializer.Serialize(previewAttackRange2, returnDataPool);
			}
			case 3:
			{
				int item115 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item115);
				short item116 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item116);
				int item117 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item117);
				OuterAndInnerShorts previewAttackRange = GetPreviewAttackRange(item115, item116, item117);
				return GameData.Serializer.Serializer.Serialize(previewAttackRange, returnDataPool);
			}
			default:
				throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
			}
		case 86:
		{
			int argsCount28 = operation.ArgsCount;
			int num28 = argsCount28;
			if (num28 == 1)
			{
				bool item111 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item111);
				bool item112 = SetPuppetUnyieldingFallen(context, item111);
				return GameData.Serializer.Serializer.Serialize(item112, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 87:
		{
			int argsCount26 = operation.ArgsCount;
			int num26 = argsCount26;
			if (num26 == 1)
			{
				bool item96 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item96);
				bool item97 = SetPuppetDisableAi(context, item96);
				return GameData.Serializer.Serializer.Serialize(item97, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 88:
			switch (operation.ArgsCount)
			{
			case 0:
			{
				bool item95 = InterruptSkillManual(context);
				return GameData.Serializer.Serializer.Serialize(item95, returnDataPool);
			}
			case 1:
			{
				bool item93 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item93);
				bool item94 = InterruptSkillManual(context, item93);
				return GameData.Serializer.Serializer.Serialize(item94, returnDataPool);
			}
			default:
				throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
			}
		case 89:
		{
			int argsCount25 = operation.ArgsCount;
			int num25 = argsCount25;
			if (num25 == 2)
			{
				short item89 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item89);
				int[] item90 = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item90);
				TeammateCommandChangeData item91 = ProcessCombatTeammateCommands(context, item89, item90);
				return GameData.Serializer.Serializer.Serialize(item91, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 90:
		{
			int argsCount24 = operation.ArgsCount;
			int num24 = argsCount24;
			if (num24 == 1)
			{
				bool item78 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item78);
				ClearAffectingMoveSkillManual(context, item78);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 91:
			switch (operation.ArgsCount)
			{
			case 1:
			{
				int item77 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item77);
				UnlockAttack(context, item77);
				return -1;
			}
			case 2:
			{
				int item75 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item75);
				bool item76 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item76);
				UnlockAttack(context, item75, item76);
				return -1;
			}
			default:
				throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
			}
		case 92:
			if (operation.ArgsCount == 0)
			{
				bool item72 = IgnoreAllRawCreate(context);
				return GameData.Serializer.Serializer.Serialize(item72, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		case 93:
		{
			int argsCount20 = operation.ArgsCount;
			int num20 = argsCount20;
			if (num20 == 1)
			{
				int item66 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item66);
				bool item67 = IgnoreRawCreate(context, item66);
				return GameData.Serializer.Serializer.Serialize(item67, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 94:
		{
			int argsCount17 = operation.ArgsCount;
			int num17 = argsCount17;
			if (num17 == 3)
			{
				int item59 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item59);
				sbyte item60 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item60);
				short item61 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item61);
				bool item62 = DoRawCreate(context, item59, item60, item61);
				return GameData.Serializer.Serializer.Serialize(item62, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 95:
		{
			int argsCount15 = operation.ArgsCount;
			int num15 = argsCount15;
			if (num15 == 1)
			{
				int item51 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item51);
				List<sbyte> allCanRawCreateEquipmentSlots = GetAllCanRawCreateEquipmentSlots(item51);
				return GameData.Serializer.Serializer.Serialize(allCanRawCreateEquipmentSlots, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 96:
			switch (operation.ArgsCount)
			{
			case 1:
			{
				int item50 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item50);
				UnlockSimulateResult unlockSimulateResult2 = GetUnlockSimulateResult(item50);
				return GameData.Serializer.Serializer.Serialize(unlockSimulateResult2, returnDataPool);
			}
			case 2:
			{
				int item48 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item48);
				bool item49 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item49);
				UnlockSimulateResult unlockSimulateResult = GetUnlockSimulateResult(item48, item49);
				return GameData.Serializer.Serializer.Serialize(unlockSimulateResult, returnDataPool);
			}
			default:
				throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
			}
		case 97:
		{
			int argsCount13 = operation.ArgsCount;
			int num13 = argsCount13;
			if (num13 == 1)
			{
				int item45 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item45);
				DefeatMarksCountOutOfCombatData defeatMarksCountOutOfCombat = GetDefeatMarksCountOutOfCombat(context, item45);
				return GameData.Serializer.Serializer.Serialize(defeatMarksCountOutOfCombat, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 98:
		{
			int argsCount10 = operation.ArgsCount;
			int num10 = argsCount10;
			if (num10 == 2)
			{
				CombatResultDisplayData item41 = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item41);
				List<ItemDisplayData> item42 = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item42);
				ApplyCombatResultDataEffect(context, item41, item42);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 99:
			switch (operation.ArgsCount)
			{
			case 0:
				ClearReserveNormalAttack(context);
				return -1;
			case 1:
			{
				bool item40 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item40);
				ClearReserveNormalAttack(context, item40);
				return -1;
			}
			default:
				throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
			}
		case 100:
		{
			int argsCount8 = operation.ArgsCount;
			int num8 = argsCount8;
			if (num8 == 2)
			{
				int item34 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item34);
				int item35 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item35);
				CharacterDisplayData item36 = ApplyVitalOnTeammate(context, item34, item35);
				return GameData.Serializer.Serializer.Serialize(item36, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 101:
		{
			int argsCount6 = operation.ArgsCount;
			int num6 = argsCount6;
			if (num6 == 1)
			{
				int item27 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item27);
				int item28 = RevertVitalOnTeammate(context, item27);
				return GameData.Serializer.Serializer.Serialize(item28, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 102:
			if (operation.ArgsCount == 0)
			{
				GmCmd_ForceRecoverTeammateCommand(context);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		case 103:
		{
			int argsCount3 = operation.ArgsCount;
			int num3 = argsCount3;
			if (num3 == 1)
			{
				int item12 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item12);
				List<ItemDisplayData> item13 = RequestValidItemsInCombat(item12);
				return GameData.Serializer.Serializer.Serialize(item13, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 104:
			if (operation.ArgsCount == 0)
			{
				List<short> item7 = RequestSwordFragmentSkillIds();
				return GameData.Serializer.Serializer.Serialize(item7, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		case 105:
			switch (operation.ArgsCount)
			{
			case 2:
			{
				sbyte item5 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item5);
				short item6 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item6);
				UseSpecialItem(context, item5, item6);
				return -1;
			}
			case 3:
			{
				sbyte item2 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item2);
				short item3 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item3);
				bool item4 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item4);
				UseSpecialItem(context, item2, item3, item4);
				return -1;
			}
			default:
				throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
			}
		case 106:
			switch (operation.ArgsCount)
			{
			case 0:
				NormalAttackImmediate(context);
				return -1;
			case 1:
			{
				bool item = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item);
				NormalAttackImmediate(context, item);
				return -1;
			}
			default:
				throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
			}
		default:
			throw new Exception($"Unsupported methodId {operation.MethodId}");
		}
	}

	public override void OnMonitorData(ushort dataId, ulong subId0, uint subId1, bool monitoring)
	{
		switch (dataId)
		{
		case 0:
			break;
		case 1:
			break;
		case 2:
			break;
		case 3:
			break;
		case 4:
			break;
		case 5:
			break;
		case 6:
			_modificationsSkillPowerAddInCombat.ChangeRecording(monitoring);
			break;
		case 7:
			_modificationsSkillPowerReduceInCombat.ChangeRecording(monitoring);
			break;
		case 8:
			_modificationsSkillPowerReplaceInCombat.ChangeRecording(monitoring);
			break;
		case 9:
			break;
		case 10:
			break;
		case 11:
			break;
		case 12:
			break;
		case 13:
			break;
		case 14:
			break;
		case 15:
			break;
		case 16:
			break;
		case 17:
			break;
		case 18:
			break;
		case 19:
			break;
		case 20:
			break;
		case 21:
			break;
		case 22:
			break;
		case 23:
			break;
		case 24:
			break;
		case 25:
			break;
		case 26:
			break;
		case 27:
			break;
		case 28:
			break;
		case 29:
			break;
		case 30:
			break;
		case 31:
			break;
		case 32:
			break;
		default:
			throw new Exception($"Unsupported dataId {dataId}");
		}
	}

	public override int CheckModified(ushort dataId, ulong subId0, uint subId1, RawDataPool dataPool)
	{
		switch (dataId)
		{
		case 0:
			if (!BaseGameDataDomain.IsModified(DataStates, 0))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 0);
			return GameData.Serializer.Serializer.Serialize(_timeScale, dataPool);
		case 1:
			if (!BaseGameDataDomain.IsModified(DataStates, 1))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 1);
			return GameData.Serializer.Serializer.Serialize(_autoCombat, dataPool);
		case 2:
			if (!BaseGameDataDomain.IsModified(DataStates, 2))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 2);
			return GameData.Serializer.Serializer.Serialize(_combatFrame, dataPool);
		case 3:
			if (!BaseGameDataDomain.IsModified(DataStates, 3))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 3);
			return GameData.Serializer.Serializer.Serialize(_combatType, dataPool);
		case 4:
			if (!BaseGameDataDomain.IsModified(DataStates, 4))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 4);
			return GameData.Serializer.Serializer.Serialize(_currentDistance, dataPool);
		case 5:
			if (!BaseGameDataDomain.IsModified(DataStates, 5))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 5);
			return GameData.Serializer.Serializer.Serialize(_damageCompareData, dataPool);
		case 6:
		{
			if (!BaseGameDataDomain.IsModified(DataStates, 6))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 6);
			int result2 = GameData.Serializer.Serializer.SerializeModifications(_skillPowerAddInCombat, dataPool, _modificationsSkillPowerAddInCombat);
			_modificationsSkillPowerAddInCombat.Reset();
			return result2;
		}
		case 7:
		{
			if (!BaseGameDataDomain.IsModified(DataStates, 7))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 7);
			int result = GameData.Serializer.Serializer.SerializeModifications(_skillPowerReduceInCombat, dataPool, _modificationsSkillPowerReduceInCombat);
			_modificationsSkillPowerReduceInCombat.Reset();
			return result;
		}
		case 8:
		{
			if (!BaseGameDataDomain.IsModified(DataStates, 8))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 8);
			int result3 = GameData.Serializer.Serializer.SerializeModifications(_skillPowerReplaceInCombat, dataPool, _modificationsSkillPowerReplaceInCombat);
			_modificationsSkillPowerReplaceInCombat.Reset();
			return result3;
		}
		case 9:
			if (!BaseGameDataDomain.IsModified(DataStates, 9))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 9);
			return GameData.Serializer.Serializer.Serialize(_bgmIndex, dataPool);
		case 10:
			return CheckModified_CombatCharacterDict((int)subId0, (ushort)subId1, dataPool);
		case 11:
			if (!BaseGameDataDomain.IsModified(DataStates, 11))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 11);
			return GameData.Serializer.Serializer.Serialize(_selfTeam, dataPool);
		case 12:
			if (!BaseGameDataDomain.IsModified(DataStates, 12))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 12);
			return GameData.Serializer.Serializer.Serialize(_selfCharId, dataPool);
		case 13:
			if (!BaseGameDataDomain.IsModified(DataStates, 13))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 13);
			return GameData.Serializer.Serializer.Serialize(_selfTeamWisdomType, dataPool);
		case 14:
			if (!BaseGameDataDomain.IsModified(DataStates, 14))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 14);
			return GameData.Serializer.Serializer.Serialize(_selfTeamWisdomCount, dataPool);
		case 15:
			if (!BaseGameDataDomain.IsModified(DataStates, 15))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 15);
			return GameData.Serializer.Serializer.Serialize(_enemyTeam, dataPool);
		case 16:
			if (!BaseGameDataDomain.IsModified(DataStates, 16))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 16);
			return GameData.Serializer.Serializer.Serialize(_enemyCharId, dataPool);
		case 17:
			if (!BaseGameDataDomain.IsModified(DataStates, 17))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 17);
			return GameData.Serializer.Serializer.Serialize(_enemyTeamWisdomType, dataPool);
		case 18:
			if (!BaseGameDataDomain.IsModified(DataStates, 18))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 18);
			return GameData.Serializer.Serializer.Serialize(_enemyTeamWisdomCount, dataPool);
		case 19:
			if (!BaseGameDataDomain.IsModified(DataStates, 19))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 19);
			return GameData.Serializer.Serializer.Serialize(_combatStatus, dataPool);
		case 20:
			if (!BaseGameDataDomain.IsModified(DataStates, 20))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 20);
			return GameData.Serializer.Serializer.Serialize(_showMercyOption, dataPool);
		case 21:
			if (!BaseGameDataDomain.IsModified(DataStates, 21))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 21);
			return GameData.Serializer.Serializer.Serialize(_selectedMercyOption, dataPool);
		case 22:
			if (!BaseGameDataDomain.IsModified(DataStates, 22))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 22);
			return GameData.Serializer.Serializer.Serialize(_carrierAnimalCombatCharId, dataPool);
		case 23:
			if (!BaseGameDataDomain.IsModified(DataStates, 23))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 23);
			return GameData.Serializer.Serializer.Serialize(_specialShowCombatCharId, dataPool);
		case 24:
			if (!BaseGameDataDomain.IsModified(DataStates, 24))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 24);
			return GameData.Serializer.Serializer.Serialize(_skillAttackedIndexAndHit, dataPool);
		case 25:
			if (!BaseGameDataDomain.IsModified(DataStates, 25))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 25);
			return GameData.Serializer.Serializer.Serialize(_waitingDelaySettlement, dataPool);
		case 26:
			if (!BaseGameDataDomain.IsModified(DataStates, 26))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 26);
			return GameData.Serializer.Serializer.Serialize(_showUseGoldenWire, dataPool);
		case 27:
			if (!BaseGameDataDomain.IsModified(DataStates, 27))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 27);
			return GameData.Serializer.Serializer.Serialize(_isPuppetCombat, dataPool);
		case 28:
			if (!BaseGameDataDomain.IsModified(DataStates, 28))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 28);
			return GameData.Serializer.Serializer.Serialize(_isPlaygroundCombat, dataPool);
		case 29:
			return CheckModified_SkillDataDict((CombatSkillKey)subId0, (ushort)subId1, dataPool);
		case 30:
			return CheckModified_WeaponDataDict((int)subId0, (ushort)subId1, dataPool);
		case 31:
			if (!BaseGameDataDomain.IsModified(DataStates, 31))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 31);
			return GameData.Serializer.Serializer.Serialize(_expectRatioData, dataPool);
		case 32:
			if (!BaseGameDataDomain.IsModified(DataStates, 32))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 32);
			return GameData.Serializer.Serializer.Serialize(_taiwuSpecialGroupCharIds, dataPool);
		default:
			throw new Exception($"Unsupported dataId {dataId}");
		}
	}

	public override void ResetModifiedWrapper(ushort dataId, ulong subId0, uint subId1)
	{
		switch (dataId)
		{
		case 0:
			if (BaseGameDataDomain.IsModified(DataStates, 0))
			{
				BaseGameDataDomain.ResetModified(DataStates, 0);
			}
			break;
		case 1:
			if (BaseGameDataDomain.IsModified(DataStates, 1))
			{
				BaseGameDataDomain.ResetModified(DataStates, 1);
			}
			break;
		case 2:
			if (BaseGameDataDomain.IsModified(DataStates, 2))
			{
				BaseGameDataDomain.ResetModified(DataStates, 2);
			}
			break;
		case 3:
			if (BaseGameDataDomain.IsModified(DataStates, 3))
			{
				BaseGameDataDomain.ResetModified(DataStates, 3);
			}
			break;
		case 4:
			if (BaseGameDataDomain.IsModified(DataStates, 4))
			{
				BaseGameDataDomain.ResetModified(DataStates, 4);
			}
			break;
		case 5:
			if (BaseGameDataDomain.IsModified(DataStates, 5))
			{
				BaseGameDataDomain.ResetModified(DataStates, 5);
			}
			break;
		case 6:
			if (BaseGameDataDomain.IsModified(DataStates, 6))
			{
				BaseGameDataDomain.ResetModified(DataStates, 6);
				_modificationsSkillPowerAddInCombat.Reset();
			}
			break;
		case 7:
			if (BaseGameDataDomain.IsModified(DataStates, 7))
			{
				BaseGameDataDomain.ResetModified(DataStates, 7);
				_modificationsSkillPowerReduceInCombat.Reset();
			}
			break;
		case 8:
			if (BaseGameDataDomain.IsModified(DataStates, 8))
			{
				BaseGameDataDomain.ResetModified(DataStates, 8);
				_modificationsSkillPowerReplaceInCombat.Reset();
			}
			break;
		case 9:
			if (BaseGameDataDomain.IsModified(DataStates, 9))
			{
				BaseGameDataDomain.ResetModified(DataStates, 9);
			}
			break;
		case 10:
			ResetModifiedWrapper_CombatCharacterDict((int)subId0, (ushort)subId1);
			break;
		case 11:
			if (BaseGameDataDomain.IsModified(DataStates, 11))
			{
				BaseGameDataDomain.ResetModified(DataStates, 11);
			}
			break;
		case 12:
			if (BaseGameDataDomain.IsModified(DataStates, 12))
			{
				BaseGameDataDomain.ResetModified(DataStates, 12);
			}
			break;
		case 13:
			if (BaseGameDataDomain.IsModified(DataStates, 13))
			{
				BaseGameDataDomain.ResetModified(DataStates, 13);
			}
			break;
		case 14:
			if (BaseGameDataDomain.IsModified(DataStates, 14))
			{
				BaseGameDataDomain.ResetModified(DataStates, 14);
			}
			break;
		case 15:
			if (BaseGameDataDomain.IsModified(DataStates, 15))
			{
				BaseGameDataDomain.ResetModified(DataStates, 15);
			}
			break;
		case 16:
			if (BaseGameDataDomain.IsModified(DataStates, 16))
			{
				BaseGameDataDomain.ResetModified(DataStates, 16);
			}
			break;
		case 17:
			if (BaseGameDataDomain.IsModified(DataStates, 17))
			{
				BaseGameDataDomain.ResetModified(DataStates, 17);
			}
			break;
		case 18:
			if (BaseGameDataDomain.IsModified(DataStates, 18))
			{
				BaseGameDataDomain.ResetModified(DataStates, 18);
			}
			break;
		case 19:
			if (BaseGameDataDomain.IsModified(DataStates, 19))
			{
				BaseGameDataDomain.ResetModified(DataStates, 19);
			}
			break;
		case 20:
			if (BaseGameDataDomain.IsModified(DataStates, 20))
			{
				BaseGameDataDomain.ResetModified(DataStates, 20);
			}
			break;
		case 21:
			if (BaseGameDataDomain.IsModified(DataStates, 21))
			{
				BaseGameDataDomain.ResetModified(DataStates, 21);
			}
			break;
		case 22:
			if (BaseGameDataDomain.IsModified(DataStates, 22))
			{
				BaseGameDataDomain.ResetModified(DataStates, 22);
			}
			break;
		case 23:
			if (BaseGameDataDomain.IsModified(DataStates, 23))
			{
				BaseGameDataDomain.ResetModified(DataStates, 23);
			}
			break;
		case 24:
			if (BaseGameDataDomain.IsModified(DataStates, 24))
			{
				BaseGameDataDomain.ResetModified(DataStates, 24);
			}
			break;
		case 25:
			if (BaseGameDataDomain.IsModified(DataStates, 25))
			{
				BaseGameDataDomain.ResetModified(DataStates, 25);
			}
			break;
		case 26:
			if (BaseGameDataDomain.IsModified(DataStates, 26))
			{
				BaseGameDataDomain.ResetModified(DataStates, 26);
			}
			break;
		case 27:
			if (BaseGameDataDomain.IsModified(DataStates, 27))
			{
				BaseGameDataDomain.ResetModified(DataStates, 27);
			}
			break;
		case 28:
			if (BaseGameDataDomain.IsModified(DataStates, 28))
			{
				BaseGameDataDomain.ResetModified(DataStates, 28);
			}
			break;
		case 29:
			ResetModifiedWrapper_SkillDataDict((CombatSkillKey)subId0, (ushort)subId1);
			break;
		case 30:
			ResetModifiedWrapper_WeaponDataDict((int)subId0, (ushort)subId1);
			break;
		case 31:
			if (BaseGameDataDomain.IsModified(DataStates, 31))
			{
				BaseGameDataDomain.ResetModified(DataStates, 31);
			}
			break;
		case 32:
			if (BaseGameDataDomain.IsModified(DataStates, 32))
			{
				BaseGameDataDomain.ResetModified(DataStates, 32);
			}
			break;
		default:
			throw new Exception($"Unsupported dataId {dataId}");
		}
	}

	public override bool IsModifiedWrapper(ushort dataId, ulong subId0, uint subId1)
	{
		return dataId switch
		{
			0 => BaseGameDataDomain.IsModified(DataStates, 0), 
			1 => BaseGameDataDomain.IsModified(DataStates, 1), 
			2 => BaseGameDataDomain.IsModified(DataStates, 2), 
			3 => BaseGameDataDomain.IsModified(DataStates, 3), 
			4 => BaseGameDataDomain.IsModified(DataStates, 4), 
			5 => BaseGameDataDomain.IsModified(DataStates, 5), 
			6 => BaseGameDataDomain.IsModified(DataStates, 6), 
			7 => BaseGameDataDomain.IsModified(DataStates, 7), 
			8 => BaseGameDataDomain.IsModified(DataStates, 8), 
			9 => BaseGameDataDomain.IsModified(DataStates, 9), 
			10 => IsModifiedWrapper_CombatCharacterDict((int)subId0, (ushort)subId1), 
			11 => BaseGameDataDomain.IsModified(DataStates, 11), 
			12 => BaseGameDataDomain.IsModified(DataStates, 12), 
			13 => BaseGameDataDomain.IsModified(DataStates, 13), 
			14 => BaseGameDataDomain.IsModified(DataStates, 14), 
			15 => BaseGameDataDomain.IsModified(DataStates, 15), 
			16 => BaseGameDataDomain.IsModified(DataStates, 16), 
			17 => BaseGameDataDomain.IsModified(DataStates, 17), 
			18 => BaseGameDataDomain.IsModified(DataStates, 18), 
			19 => BaseGameDataDomain.IsModified(DataStates, 19), 
			20 => BaseGameDataDomain.IsModified(DataStates, 20), 
			21 => BaseGameDataDomain.IsModified(DataStates, 21), 
			22 => BaseGameDataDomain.IsModified(DataStates, 22), 
			23 => BaseGameDataDomain.IsModified(DataStates, 23), 
			24 => BaseGameDataDomain.IsModified(DataStates, 24), 
			25 => BaseGameDataDomain.IsModified(DataStates, 25), 
			26 => BaseGameDataDomain.IsModified(DataStates, 26), 
			27 => BaseGameDataDomain.IsModified(DataStates, 27), 
			28 => BaseGameDataDomain.IsModified(DataStates, 28), 
			29 => IsModifiedWrapper_SkillDataDict((CombatSkillKey)subId0, (ushort)subId1), 
			30 => IsModifiedWrapper_WeaponDataDict((int)subId0, (ushort)subId1), 
			31 => BaseGameDataDomain.IsModified(DataStates, 31), 
			32 => BaseGameDataDomain.IsModified(DataStates, 32), 
			_ => throw new Exception($"Unsupported dataId {dataId}"), 
		};
	}

	public override void InvalidateCache(BaseGameDataObject sourceObject, DataInfluence influence, DataContext context, bool unconditionallyInfluenceAll)
	{
		switch (influence.TargetIndicator.DataId)
		{
		case 10:
			if (!unconditionallyInfluenceAll)
			{
				List<BaseGameDataObject> list2 = InfluenceChecker.InfluencedObjectsPool.Get();
				if (!InfluenceChecker.GetScope(context, sourceObject, influence.Scope, _combatCharacterDict, list2))
				{
					int count3 = list2.Count;
					for (int k = 0; k < count3; k++)
					{
						BaseGameDataObject baseGameDataObject2 = list2[k];
						List<DataUid> targetUids2 = influence.TargetUids;
						int count4 = targetUids2.Count;
						for (int l = 0; l < count4; l++)
						{
							baseGameDataObject2.InvalidateSelfAndInfluencedCache((ushort)targetUids2[l].SubId1, context);
						}
					}
				}
				else
				{
					BaseGameDataDomain.InvalidateAllAndInfluencedCaches(CacheInfluencesCombatCharacterDict, _dataStatesCombatCharacterDict, influence, context);
				}
				list2.Clear();
				InfluenceChecker.InfluencedObjectsPool.Return(list2);
			}
			else
			{
				BaseGameDataDomain.InvalidateAllAndInfluencedCaches(CacheInfluencesCombatCharacterDict, _dataStatesCombatCharacterDict, influence, context);
			}
			break;
		case 29:
			if (!unconditionallyInfluenceAll)
			{
				List<BaseGameDataObject> list3 = InfluenceChecker.InfluencedObjectsPool.Get();
				if (!InfluenceChecker.GetScope(context, sourceObject, influence.Scope, _skillDataDict, list3))
				{
					int count5 = list3.Count;
					for (int m = 0; m < count5; m++)
					{
						BaseGameDataObject baseGameDataObject3 = list3[m];
						List<DataUid> targetUids3 = influence.TargetUids;
						int count6 = targetUids3.Count;
						for (int n = 0; n < count6; n++)
						{
							baseGameDataObject3.InvalidateSelfAndInfluencedCache((ushort)targetUids3[n].SubId1, context);
						}
					}
				}
				else
				{
					BaseGameDataDomain.InvalidateAllAndInfluencedCaches(CacheInfluencesSkillDataDict, _dataStatesSkillDataDict, influence, context);
				}
				list3.Clear();
				InfluenceChecker.InfluencedObjectsPool.Return(list3);
			}
			else
			{
				BaseGameDataDomain.InvalidateAllAndInfluencedCaches(CacheInfluencesSkillDataDict, _dataStatesSkillDataDict, influence, context);
			}
			break;
		case 30:
			if (!unconditionallyInfluenceAll)
			{
				List<BaseGameDataObject> list = InfluenceChecker.InfluencedObjectsPool.Get();
				if (!InfluenceChecker.GetScope(context, sourceObject, influence.Scope, _weaponDataDict, list))
				{
					int count = list.Count;
					for (int i = 0; i < count; i++)
					{
						BaseGameDataObject baseGameDataObject = list[i];
						List<DataUid> targetUids = influence.TargetUids;
						int count2 = targetUids.Count;
						for (int j = 0; j < count2; j++)
						{
							baseGameDataObject.InvalidateSelfAndInfluencedCache((ushort)targetUids[j].SubId1, context);
						}
					}
				}
				else
				{
					BaseGameDataDomain.InvalidateAllAndInfluencedCaches(CacheInfluencesWeaponDataDict, _dataStatesWeaponDataDict, influence, context);
				}
				list.Clear();
				InfluenceChecker.InfluencedObjectsPool.Return(list);
			}
			else
			{
				BaseGameDataDomain.InvalidateAllAndInfluencedCaches(CacheInfluencesWeaponDataDict, _dataStatesWeaponDataDict, influence, context);
			}
			break;
		default:
			throw new Exception($"Unsupported dataId {influence.TargetIndicator.DataId}");
		case 0:
		case 1:
		case 2:
		case 3:
		case 4:
		case 5:
		case 6:
		case 7:
		case 8:
		case 9:
		case 11:
		case 12:
		case 13:
		case 14:
		case 15:
		case 16:
		case 17:
		case 18:
		case 19:
		case 20:
		case 21:
		case 22:
		case 23:
		case 24:
		case 25:
		case 26:
		case 27:
		case 28:
		case 31:
		case 32:
			throw new Exception($"Cannot invalidate cache state of non-cache data {influence.TargetIndicator.DataId}");
		}
	}

	public unsafe override void ProcessArchiveResponse(OperationWrapper operation, byte* pResult)
	{
		throw new Exception($"Cannot process archive response of non-archive data {operation.DataId}");
	}

	private void InitializeInternalDataOfCollections()
	{
		foreach (KeyValuePair<int, CombatCharacter> item in _combatCharacterDict)
		{
			CombatCharacter value = item.Value;
			value.CollectionHelperData = HelperDataCombatCharacterDict;
			value.DataStatesOffset = _dataStatesCombatCharacterDict.Create();
		}
		foreach (KeyValuePair<CombatSkillKey, CombatSkillData> item2 in _skillDataDict)
		{
			CombatSkillData value2 = item2.Value;
			value2.CollectionHelperData = HelperDataSkillDataDict;
			value2.DataStatesOffset = _dataStatesSkillDataDict.Create();
		}
		foreach (KeyValuePair<int, CombatWeaponData> item3 in _weaponDataDict)
		{
			CombatWeaponData value3 = item3.Value;
			value3.CollectionHelperData = HelperDataWeaponDataDict;
			value3.DataStatesOffset = _dataStatesWeaponDataDict.Create();
		}
	}
}
