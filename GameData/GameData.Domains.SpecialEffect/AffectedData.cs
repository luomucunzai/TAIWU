using System;
using GameData.ArchiveData;
using GameData.Common;
using GameData.Serializer;

namespace GameData.Domains.SpecialEffect;

[SerializableGameData(NotForDisplayModule = true)]
public class AffectedData : BaseGameDataObject, ISerializableGameData
{
	internal class FixedFieldInfos
	{
		public const uint Id_Offset = 0u;

		public const int Id_Size = 4;
	}

	[CollectionObjectField(false, true, false, false, false)]
	private int _id;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _maxStrength;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _maxDexterity;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _maxConcentration;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _maxVitality;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _maxEnergy;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _maxIntelligence;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _recoveryOfStance;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _recoveryOfBreath;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _moveSpeed;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _recoveryOfFlaw;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _castSpeed;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _recoveryOfBlockedAcupoint;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _weaponSwitchSpeed;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _attackSpeed;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _innerRatio;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _recoveryOfQiDisorder;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _minorAttributeFixMaxValue;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _minorAttributeFixMinValue;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _resistOfHotPoison;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _resistOfGloomyPoison;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _resistOfColdPoison;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _resistOfRedPoison;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _resistOfRottenPoison;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _resistOfIllusoryPoison;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _resistOfAllPoison;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _personalitiesAll;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _displayAge;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _neiliProportionOfFiveElements;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _skillAlsoAsFiveElements;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _weaponMaxPower;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _weaponUseRequirement;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _weaponAttackRange;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _armorMaxPower;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _armorUseRequirement;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _equipmentPower;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _ignoreEquipmentOverload;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _equipmentBonus;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _hitStrength;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _hitTechnique;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _hitSpeed;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _hitMind;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _hitAddByTempValue;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _hitCanChange;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _hitChangeEffectPercent;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _avoidStrength;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _avoidTechnique;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _avoidSpeed;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _avoidMind;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _avoidAddByTempValue;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _avoidCanChange;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _avoidChangeEffectPercent;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _penetrateOuter;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _penetrateInner;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _penetrateResistOuter;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _penetrateResistInner;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _consummateLevelBonus;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _consummateLevelRelatedMainAttributesHitValues;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _consummateLevelRelatedMainAttributesAvoidValues;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _consummateLevelRelatedMainAttributesPenetrations;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _consummateLevelRelatedMainAttributesPenetrationResists;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _neiliAllocationAttack;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _neiliAllocationAgile;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _neiliAllocationDefense;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _neiliAllocationAssist;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _happiness;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _maxHealth;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _healthCost;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _moveSpeedCanChange;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _goneMadInAllBreak;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _xiangshuInfectionDelta;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _healthDelta;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _happinessDelta;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _currAgeDelta;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _neiliDelta;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _makeLoveRateOnMonthChange;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _takeRevengeRateOnMonthChange;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _canMakeLoveSpecialOnMonthChange;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _canReadingOnMonthChange;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _canAutoHealOnMonthChange;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _canRecoverHealthOnMonthChange;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _featureBonusReverse;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _attackerHitStrength;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _attackerHitTechnique;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _attackerHitSpeed;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _attackerHitMind;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _attackerAvoidStrength;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _attackerAvoidTechnique;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _attackerAvoidSpeed;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _attackerAvoidMind;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _attackerPenetrateOuter;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _attackerPenetrateInner;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _attackerPenetrateResistOuter;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _attackerPenetrateResistInner;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _attackHitType;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _makeDirectDamage;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _makeMindDamage;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _makeBounceDamage;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _makeFightBackDamage;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _makePoisonLevel;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _makePoisonValue;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _makePoisonResist;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _makePoisonTarget;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _attackerHitOdds;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _attackerFightBackHitOdds;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _attackerPursueOdds;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _makedInjuryChangeToOld;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _makedPoisonChangeToOld;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _allMarkChangeToMind;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _mindMarkChangeToFatal;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _infinityMindMarkProgress;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _makeDamageCanReduce;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _ignoreArmor;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _makeDamageType;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _canMakeInjuryToNoInjuryPart;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _makePoisonType;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _normalAttackWeapon;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _normalAttackTrick;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _normalAttackGetTrickCount;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _normalAttackPrepareFrame;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _normalAttackRecoveryFrame;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _unlockSpeed;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _normalAttackChangeToUnlockAttack;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _rawCreateEffectList;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _extraFlawCount;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _flawBonusFactor;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _attackCanBounce;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _attackCanFightBack;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _attackCanPursue;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _makeFightBackInjuryMark;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _legSkillUseShoes;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _attackerDirectFinalDamageValue;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _attackerFinalDamageValue;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _defenderHitStrength;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _defenderHitTechnique;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _defenderHitSpeed;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _defenderHitMind;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _defenderAvoidStrength;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _defenderAvoidTechnique;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _defenderAvoidSpeed;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _defenderAvoidMind;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _defenderPenetrateOuter;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _defenderPenetrateInner;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _defenderPenetrateResistOuter;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _defenderPenetrateResistInner;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _acceptDirectDamage;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _acceptMindDamage;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _acceptBounceDamage;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _acceptFightBackDamage;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _acceptPoisonLevel;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _acceptPoisonValue;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _acceptPoisonResist;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _acceptPoisonTarget;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _defenderHitOdds;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _defenderFightBackHitOdds;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _defenderPursueOdds;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _acceptDamageCanAdd;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _acceptMaxInjuryCount;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _bouncePower;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _fightBackPower;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _directDamageInnerRatio;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _defenderDirectFinalDamageValue;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _defenderFinalDamageValue;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _unyieldingFallen;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _outerInjuryImmunity;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _innerInjuryImmunity;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _directDamageValue;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _directInjuryMark;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _goneMadInjury;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _finalGoneMadInjury;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _healInjurySpeed;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _healInjuryBuff;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _healInjuryDebuff;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _healPoisonSpeed;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _healPoisonBuff;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _healPoisonDebuff;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _medicineEffect;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _healFlawSpeed;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _healAcupointSpeed;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _fleeSpeed;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _maxFlawCount;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _canAddFlaw;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _flawLevel;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _flawLevelCanReduce;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _flawCount;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _maxAcupointCount;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _canAddAcupoint;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _acupointLevel;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _acupointLevelCanReduce;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _acupointCount;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _addNeiliAllocation;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _costNeiliAllocation;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _canChangeNeiliAllocation;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _canGetTrick;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _getTrickType;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _attackBodyPart;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _attackBodyPartOdds;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _weaponEquipAttack;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _weaponEquipDefense;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _armorEquipAttack;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _armorEquipDefense;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _equipmentWeight;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _attackRangeForward;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _attackRangeBackward;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _attackRangeMaxAcupoint;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _moveCanBeStopped;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _canForcedMove;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _mobilityCanBeRemoved;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _mobilityCostByEffect;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _stanceCostByEffect;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _breathCostByEffect;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _moveDistance;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _lockDistance;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _jumpPrepareFrame;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _bounceInjuryMark;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _skillHasCost;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _combatStateEffect;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _changeNeedUseSkill;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _changeDistanceIsMove;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _replaceCharHit;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _canAddPoison;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _canReducePoison;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _reducePoisonValue;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _poisonCanAffect;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _poisonAffectCount;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _poisonAffectThreshold;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _poisonAffectProduceValue;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _mixPoisonInfinityAffect;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _costTricks;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _jiTrickAsWeaponTrickCount;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _uselessTrickAsJiTrickCount;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _changeDurability;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _jumpMoveDistance;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _combatStateToAdd;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _combatStatePower;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _breakBodyPartInjuryCount;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _bodyPartIsBroken;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _maxTrickCount;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _maxBreathPercent;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _maxStancePercent;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _maxMobilityPercent;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _extraBreathPercent;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _extraStancePercent;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _moveCostMobility;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _defendSkillKeepTime;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _bounceRange;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _mindMarkKeepTime;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _mindMarkCount;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _skillMobilityCostPerFrame;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _canAddWug;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _hasGodWeaponBuff;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _hasGodArmorBuff;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _teammateCmdRequireGenerateValue;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _teammateCmdEffect;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _teammateCmdCanUse;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _flawRecoverSpeed;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _acupointRecoverSpeed;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _mindMarkRecoverSpeed;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _injuryAutoHealSpeed;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _canRecoverBreath;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _canRecoverStance;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _fatalDamageValue;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _wugFatalDamageValue;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _fatalDamageMarkCount;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _finalFatalDamageMarkCount;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _canFightBackDuringPrepareSkill;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _canFightBackWithHit;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _skillPrepareSpeed;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _breathRecoverSpeed;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _stanceRecoverSpeed;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _mobilityRecoverSpeed;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _maxChangeTrickCount;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _changeTrickProgressAddValue;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _agileSkillCanAffect;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _defendSkillCanAffect;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _assistSkillCanAffect;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _power;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _maxPower;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _powerCanReduce;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _powerAddRatio;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _powerReduceRatio;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _powerEffectReverse;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _useRequirement;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _currInnerRatio;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _costBreathAndStance;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _costBreath;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _costStance;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _costMobility;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _skillCostTricks;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _canCostEnemyUsableTricks;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _canCostUselessTricks;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _canCostShaTricks;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _effectDirection;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _effectDirectionCanChange;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _gridCost;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _prepareTotalProgress;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _specificGridCount;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _genericGridCount;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _canCriticalHit;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _criticalOdds;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _certainCriticalHit;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _inevitableHit;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _inevitableAvoid;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _canInterrupt;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _interruptOdds;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _canSilence;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _silenceOdds;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _silenceFrame;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _weaponSilenceFrame;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _canCastWithBrokenBodyPart;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _addPowerCanBeRemoved;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _skillType;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _effectCountCanChange;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _canCast;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _canCastInDefend;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _hitDistribution;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _canCastOnLackBreath;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _canCastOnLackStance;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _convertCostBreathAndStance;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _costBreathOnCast;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _costStanceOnCast;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _canUseMobilityAsBreath;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _canUseMobilityAsStance;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _castCostNeiliAllocation;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _canCostNeiliAllocationEffect;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _canCostTrickDuringPreparingSkill;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _validItemList;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _combatSkillDataEffectList;

	[CollectionObjectField(false, true, false, false, false)]
	private SpecialEffectList _combatSkillAiScorePower;

	public const int FixedSize = 4;

	public const int DynamicCount = 328;

	public AffectedData(int charId)
	{
		_id = charId;
	}

	public SpecialEffectList GetEffectList(ushort fieldId, bool createIfNull = false)
	{
		switch (fieldId)
		{
		case 1:
			if (_maxStrength == null && createIfNull)
			{
				_maxStrength = new SpecialEffectList();
			}
			return _maxStrength;
		case 2:
			if (_maxDexterity == null && createIfNull)
			{
				_maxDexterity = new SpecialEffectList();
			}
			return _maxDexterity;
		case 3:
			if (_maxConcentration == null && createIfNull)
			{
				_maxConcentration = new SpecialEffectList();
			}
			return _maxConcentration;
		case 4:
			if (_maxVitality == null && createIfNull)
			{
				_maxVitality = new SpecialEffectList();
			}
			return _maxVitality;
		case 5:
			if (_maxEnergy == null && createIfNull)
			{
				_maxEnergy = new SpecialEffectList();
			}
			return _maxEnergy;
		case 6:
			if (_maxIntelligence == null && createIfNull)
			{
				_maxIntelligence = new SpecialEffectList();
			}
			return _maxIntelligence;
		case 7:
			if (_recoveryOfStance == null && createIfNull)
			{
				_recoveryOfStance = new SpecialEffectList();
			}
			return _recoveryOfStance;
		case 8:
			if (_recoveryOfBreath == null && createIfNull)
			{
				_recoveryOfBreath = new SpecialEffectList();
			}
			return _recoveryOfBreath;
		case 9:
			if (_moveSpeed == null && createIfNull)
			{
				_moveSpeed = new SpecialEffectList();
			}
			return _moveSpeed;
		case 10:
			if (_recoveryOfFlaw == null && createIfNull)
			{
				_recoveryOfFlaw = new SpecialEffectList();
			}
			return _recoveryOfFlaw;
		case 11:
			if (_castSpeed == null && createIfNull)
			{
				_castSpeed = new SpecialEffectList();
			}
			return _castSpeed;
		case 12:
			if (_recoveryOfBlockedAcupoint == null && createIfNull)
			{
				_recoveryOfBlockedAcupoint = new SpecialEffectList();
			}
			return _recoveryOfBlockedAcupoint;
		case 13:
			if (_weaponSwitchSpeed == null && createIfNull)
			{
				_weaponSwitchSpeed = new SpecialEffectList();
			}
			return _weaponSwitchSpeed;
		case 14:
			if (_attackSpeed == null && createIfNull)
			{
				_attackSpeed = new SpecialEffectList();
			}
			return _attackSpeed;
		case 15:
			if (_innerRatio == null && createIfNull)
			{
				_innerRatio = new SpecialEffectList();
			}
			return _innerRatio;
		case 16:
			if (_recoveryOfQiDisorder == null && createIfNull)
			{
				_recoveryOfQiDisorder = new SpecialEffectList();
			}
			return _recoveryOfQiDisorder;
		case 17:
			if (_minorAttributeFixMaxValue == null && createIfNull)
			{
				_minorAttributeFixMaxValue = new SpecialEffectList();
			}
			return _minorAttributeFixMaxValue;
		case 18:
			if (_minorAttributeFixMinValue == null && createIfNull)
			{
				_minorAttributeFixMinValue = new SpecialEffectList();
			}
			return _minorAttributeFixMinValue;
		case 19:
			if (_resistOfHotPoison == null && createIfNull)
			{
				_resistOfHotPoison = new SpecialEffectList();
			}
			return _resistOfHotPoison;
		case 20:
			if (_resistOfGloomyPoison == null && createIfNull)
			{
				_resistOfGloomyPoison = new SpecialEffectList();
			}
			return _resistOfGloomyPoison;
		case 21:
			if (_resistOfColdPoison == null && createIfNull)
			{
				_resistOfColdPoison = new SpecialEffectList();
			}
			return _resistOfColdPoison;
		case 22:
			if (_resistOfRedPoison == null && createIfNull)
			{
				_resistOfRedPoison = new SpecialEffectList();
			}
			return _resistOfRedPoison;
		case 23:
			if (_resistOfRottenPoison == null && createIfNull)
			{
				_resistOfRottenPoison = new SpecialEffectList();
			}
			return _resistOfRottenPoison;
		case 24:
			if (_resistOfIllusoryPoison == null && createIfNull)
			{
				_resistOfIllusoryPoison = new SpecialEffectList();
			}
			return _resistOfIllusoryPoison;
		case 245:
			if (_resistOfAllPoison == null && createIfNull)
			{
				_resistOfAllPoison = new SpecialEffectList();
			}
			return _resistOfAllPoison;
		case 303:
			if (_personalitiesAll == null && createIfNull)
			{
				_personalitiesAll = new SpecialEffectList();
			}
			return _personalitiesAll;
		case 25:
			if (_displayAge == null && createIfNull)
			{
				_displayAge = new SpecialEffectList();
			}
			return _displayAge;
		case 26:
			if (_neiliProportionOfFiveElements == null && createIfNull)
			{
				_neiliProportionOfFiveElements = new SpecialEffectList();
			}
			return _neiliProportionOfFiveElements;
		case 240:
			if (_skillAlsoAsFiveElements == null && createIfNull)
			{
				_skillAlsoAsFiveElements = new SpecialEffectList();
			}
			return _skillAlsoAsFiveElements;
		case 27:
			if (_weaponMaxPower == null && createIfNull)
			{
				_weaponMaxPower = new SpecialEffectList();
			}
			return _weaponMaxPower;
		case 28:
			if (_weaponUseRequirement == null && createIfNull)
			{
				_weaponUseRequirement = new SpecialEffectList();
			}
			return _weaponUseRequirement;
		case 29:
			if (_weaponAttackRange == null && createIfNull)
			{
				_weaponAttackRange = new SpecialEffectList();
			}
			return _weaponAttackRange;
		case 30:
			if (_armorMaxPower == null && createIfNull)
			{
				_armorMaxPower = new SpecialEffectList();
			}
			return _armorMaxPower;
		case 31:
			if (_armorUseRequirement == null && createIfNull)
			{
				_armorUseRequirement = new SpecialEffectList();
			}
			return _armorUseRequirement;
		case 315:
			if (_equipmentPower == null && createIfNull)
			{
				_equipmentPower = new SpecialEffectList();
			}
			return _equipmentPower;
		case 279:
			if (_ignoreEquipmentOverload == null && createIfNull)
			{
				_ignoreEquipmentOverload = new SpecialEffectList();
			}
			return _ignoreEquipmentOverload;
		case 310:
			if (_equipmentBonus == null && createIfNull)
			{
				_equipmentBonus = new SpecialEffectList();
			}
			return _equipmentBonus;
		case 32:
			if (_hitStrength == null && createIfNull)
			{
				_hitStrength = new SpecialEffectList();
			}
			return _hitStrength;
		case 33:
			if (_hitTechnique == null && createIfNull)
			{
				_hitTechnique = new SpecialEffectList();
			}
			return _hitTechnique;
		case 34:
			if (_hitSpeed == null && createIfNull)
			{
				_hitSpeed = new SpecialEffectList();
			}
			return _hitSpeed;
		case 35:
			if (_hitMind == null && createIfNull)
			{
				_hitMind = new SpecialEffectList();
			}
			return _hitMind;
		case 277:
			if (_hitAddByTempValue == null && createIfNull)
			{
				_hitAddByTempValue = new SpecialEffectList();
			}
			return _hitAddByTempValue;
		case 36:
			if (_hitCanChange == null && createIfNull)
			{
				_hitCanChange = new SpecialEffectList();
			}
			return _hitCanChange;
		case 37:
			if (_hitChangeEffectPercent == null && createIfNull)
			{
				_hitChangeEffectPercent = new SpecialEffectList();
			}
			return _hitChangeEffectPercent;
		case 38:
			if (_avoidStrength == null && createIfNull)
			{
				_avoidStrength = new SpecialEffectList();
			}
			return _avoidStrength;
		case 39:
			if (_avoidTechnique == null && createIfNull)
			{
				_avoidTechnique = new SpecialEffectList();
			}
			return _avoidTechnique;
		case 40:
			if (_avoidSpeed == null && createIfNull)
			{
				_avoidSpeed = new SpecialEffectList();
			}
			return _avoidSpeed;
		case 41:
			if (_avoidMind == null && createIfNull)
			{
				_avoidMind = new SpecialEffectList();
			}
			return _avoidMind;
		case 278:
			if (_avoidAddByTempValue == null && createIfNull)
			{
				_avoidAddByTempValue = new SpecialEffectList();
			}
			return _avoidAddByTempValue;
		case 42:
			if (_avoidCanChange == null && createIfNull)
			{
				_avoidCanChange = new SpecialEffectList();
			}
			return _avoidCanChange;
		case 43:
			if (_avoidChangeEffectPercent == null && createIfNull)
			{
				_avoidChangeEffectPercent = new SpecialEffectList();
			}
			return _avoidChangeEffectPercent;
		case 44:
			if (_penetrateOuter == null && createIfNull)
			{
				_penetrateOuter = new SpecialEffectList();
			}
			return _penetrateOuter;
		case 45:
			if (_penetrateInner == null && createIfNull)
			{
				_penetrateInner = new SpecialEffectList();
			}
			return _penetrateInner;
		case 46:
			if (_penetrateResistOuter == null && createIfNull)
			{
				_penetrateResistOuter = new SpecialEffectList();
			}
			return _penetrateResistOuter;
		case 47:
			if (_penetrateResistInner == null && createIfNull)
			{
				_penetrateResistInner = new SpecialEffectList();
			}
			return _penetrateResistInner;
		case 297:
			if (_consummateLevelBonus == null && createIfNull)
			{
				_consummateLevelBonus = new SpecialEffectList();
			}
			return _consummateLevelBonus;
		case 236:
			if (_consummateLevelRelatedMainAttributesHitValues == null && createIfNull)
			{
				_consummateLevelRelatedMainAttributesHitValues = new SpecialEffectList();
			}
			return _consummateLevelRelatedMainAttributesHitValues;
		case 237:
			if (_consummateLevelRelatedMainAttributesAvoidValues == null && createIfNull)
			{
				_consummateLevelRelatedMainAttributesAvoidValues = new SpecialEffectList();
			}
			return _consummateLevelRelatedMainAttributesAvoidValues;
		case 238:
			if (_consummateLevelRelatedMainAttributesPenetrations == null && createIfNull)
			{
				_consummateLevelRelatedMainAttributesPenetrations = new SpecialEffectList();
			}
			return _consummateLevelRelatedMainAttributesPenetrations;
		case 239:
			if (_consummateLevelRelatedMainAttributesPenetrationResists == null && createIfNull)
			{
				_consummateLevelRelatedMainAttributesPenetrationResists = new SpecialEffectList();
			}
			return _consummateLevelRelatedMainAttributesPenetrationResists;
		case 48:
			if (_neiliAllocationAttack == null && createIfNull)
			{
				_neiliAllocationAttack = new SpecialEffectList();
			}
			return _neiliAllocationAttack;
		case 49:
			if (_neiliAllocationAgile == null && createIfNull)
			{
				_neiliAllocationAgile = new SpecialEffectList();
			}
			return _neiliAllocationAgile;
		case 50:
			if (_neiliAllocationDefense == null && createIfNull)
			{
				_neiliAllocationDefense = new SpecialEffectList();
			}
			return _neiliAllocationDefense;
		case 51:
			if (_neiliAllocationAssist == null && createIfNull)
			{
				_neiliAllocationAssist = new SpecialEffectList();
			}
			return _neiliAllocationAssist;
		case 52:
			if (_happiness == null && createIfNull)
			{
				_happiness = new SpecialEffectList();
			}
			return _happiness;
		case 53:
			if (_maxHealth == null && createIfNull)
			{
				_maxHealth = new SpecialEffectList();
			}
			return _maxHealth;
		case 54:
			if (_healthCost == null && createIfNull)
			{
				_healthCost = new SpecialEffectList();
			}
			return _healthCost;
		case 55:
			if (_moveSpeedCanChange == null && createIfNull)
			{
				_moveSpeedCanChange = new SpecialEffectList();
			}
			return _moveSpeedCanChange;
		case 267:
			if (_goneMadInAllBreak == null && createIfNull)
			{
				_goneMadInAllBreak = new SpecialEffectList();
			}
			return _goneMadInAllBreak;
		case 262:
			if (_xiangshuInfectionDelta == null && createIfNull)
			{
				_xiangshuInfectionDelta = new SpecialEffectList();
			}
			return _xiangshuInfectionDelta;
		case 263:
			if (_healthDelta == null && createIfNull)
			{
				_healthDelta = new SpecialEffectList();
			}
			return _healthDelta;
		case 270:
			if (_happinessDelta == null && createIfNull)
			{
				_happinessDelta = new SpecialEffectList();
			}
			return _happinessDelta;
		case 266:
			if (_currAgeDelta == null && createIfNull)
			{
				_currAgeDelta = new SpecialEffectList();
			}
			return _currAgeDelta;
		case 298:
			if (_neiliDelta == null && createIfNull)
			{
				_neiliDelta = new SpecialEffectList();
			}
			return _neiliDelta;
		case 268:
			if (_makeLoveRateOnMonthChange == null && createIfNull)
			{
				_makeLoveRateOnMonthChange = new SpecialEffectList();
			}
			return _makeLoveRateOnMonthChange;
		case 296:
			if (_takeRevengeRateOnMonthChange == null && createIfNull)
			{
				_takeRevengeRateOnMonthChange = new SpecialEffectList();
			}
			return _takeRevengeRateOnMonthChange;
		case 299:
			if (_canMakeLoveSpecialOnMonthChange == null && createIfNull)
			{
				_canMakeLoveSpecialOnMonthChange = new SpecialEffectList();
			}
			return _canMakeLoveSpecialOnMonthChange;
		case 260:
			if (_canReadingOnMonthChange == null && createIfNull)
			{
				_canReadingOnMonthChange = new SpecialEffectList();
			}
			return _canReadingOnMonthChange;
		case 269:
			if (_canAutoHealOnMonthChange == null && createIfNull)
			{
				_canAutoHealOnMonthChange = new SpecialEffectList();
			}
			return _canAutoHealOnMonthChange;
		case 295:
			if (_canRecoverHealthOnMonthChange == null && createIfNull)
			{
				_canRecoverHealthOnMonthChange = new SpecialEffectList();
			}
			return _canRecoverHealthOnMonthChange;
		case 293:
			if (_featureBonusReverse == null && createIfNull)
			{
				_featureBonusReverse = new SpecialEffectList();
			}
			return _featureBonusReverse;
		case 56:
			if (_attackerHitStrength == null && createIfNull)
			{
				_attackerHitStrength = new SpecialEffectList();
			}
			return _attackerHitStrength;
		case 57:
			if (_attackerHitTechnique == null && createIfNull)
			{
				_attackerHitTechnique = new SpecialEffectList();
			}
			return _attackerHitTechnique;
		case 58:
			if (_attackerHitSpeed == null && createIfNull)
			{
				_attackerHitSpeed = new SpecialEffectList();
			}
			return _attackerHitSpeed;
		case 59:
			if (_attackerHitMind == null && createIfNull)
			{
				_attackerHitMind = new SpecialEffectList();
			}
			return _attackerHitMind;
		case 60:
			if (_attackerAvoidStrength == null && createIfNull)
			{
				_attackerAvoidStrength = new SpecialEffectList();
			}
			return _attackerAvoidStrength;
		case 61:
			if (_attackerAvoidTechnique == null && createIfNull)
			{
				_attackerAvoidTechnique = new SpecialEffectList();
			}
			return _attackerAvoidTechnique;
		case 62:
			if (_attackerAvoidSpeed == null && createIfNull)
			{
				_attackerAvoidSpeed = new SpecialEffectList();
			}
			return _attackerAvoidSpeed;
		case 63:
			if (_attackerAvoidMind == null && createIfNull)
			{
				_attackerAvoidMind = new SpecialEffectList();
			}
			return _attackerAvoidMind;
		case 64:
			if (_attackerPenetrateOuter == null && createIfNull)
			{
				_attackerPenetrateOuter = new SpecialEffectList();
			}
			return _attackerPenetrateOuter;
		case 65:
			if (_attackerPenetrateInner == null && createIfNull)
			{
				_attackerPenetrateInner = new SpecialEffectList();
			}
			return _attackerPenetrateInner;
		case 66:
			if (_attackerPenetrateResistOuter == null && createIfNull)
			{
				_attackerPenetrateResistOuter = new SpecialEffectList();
			}
			return _attackerPenetrateResistOuter;
		case 67:
			if (_attackerPenetrateResistInner == null && createIfNull)
			{
				_attackerPenetrateResistInner = new SpecialEffectList();
			}
			return _attackerPenetrateResistInner;
		case 68:
			if (_attackHitType == null && createIfNull)
			{
				_attackHitType = new SpecialEffectList();
			}
			return _attackHitType;
		case 69:
			if (_makeDirectDamage == null && createIfNull)
			{
				_makeDirectDamage = new SpecialEffectList();
			}
			return _makeDirectDamage;
		case 275:
			if (_makeMindDamage == null && createIfNull)
			{
				_makeMindDamage = new SpecialEffectList();
			}
			return _makeMindDamage;
		case 70:
			if (_makeBounceDamage == null && createIfNull)
			{
				_makeBounceDamage = new SpecialEffectList();
			}
			return _makeBounceDamage;
		case 71:
			if (_makeFightBackDamage == null && createIfNull)
			{
				_makeFightBackDamage = new SpecialEffectList();
			}
			return _makeFightBackDamage;
		case 72:
			if (_makePoisonLevel == null && createIfNull)
			{
				_makePoisonLevel = new SpecialEffectList();
			}
			return _makePoisonLevel;
		case 73:
			if (_makePoisonValue == null && createIfNull)
			{
				_makePoisonValue = new SpecialEffectList();
			}
			return _makePoisonValue;
		case 233:
			if (_makePoisonResist == null && createIfNull)
			{
				_makePoisonResist = new SpecialEffectList();
			}
			return _makePoisonResist;
		case 246:
			if (_makePoisonTarget == null && createIfNull)
			{
				_makePoisonTarget = new SpecialEffectList();
			}
			return _makePoisonTarget;
		case 74:
			if (_attackerHitOdds == null && createIfNull)
			{
				_attackerHitOdds = new SpecialEffectList();
			}
			return _attackerHitOdds;
		case 75:
			if (_attackerFightBackHitOdds == null && createIfNull)
			{
				_attackerFightBackHitOdds = new SpecialEffectList();
			}
			return _attackerFightBackHitOdds;
		case 76:
			if (_attackerPursueOdds == null && createIfNull)
			{
				_attackerPursueOdds = new SpecialEffectList();
			}
			return _attackerPursueOdds;
		case 77:
			if (_makedInjuryChangeToOld == null && createIfNull)
			{
				_makedInjuryChangeToOld = new SpecialEffectList();
			}
			return _makedInjuryChangeToOld;
		case 78:
			if (_makedPoisonChangeToOld == null && createIfNull)
			{
				_makedPoisonChangeToOld = new SpecialEffectList();
			}
			return _makedPoisonChangeToOld;
		case 288:
			if (_allMarkChangeToMind == null && createIfNull)
			{
				_allMarkChangeToMind = new SpecialEffectList();
			}
			return _allMarkChangeToMind;
		case 289:
			if (_mindMarkChangeToFatal == null && createIfNull)
			{
				_mindMarkChangeToFatal = new SpecialEffectList();
			}
			return _mindMarkChangeToFatal;
		case 305:
			if (_infinityMindMarkProgress == null && createIfNull)
			{
				_infinityMindMarkProgress = new SpecialEffectList();
			}
			return _infinityMindMarkProgress;
		case 327:
			if (_makeDamageCanReduce == null && createIfNull)
			{
				_makeDamageCanReduce = new SpecialEffectList();
			}
			return _makeDamageCanReduce;
		case 281:
			if (_ignoreArmor == null && createIfNull)
			{
				_ignoreArmor = new SpecialEffectList();
			}
			return _ignoreArmor;
		case 79:
			if (_makeDamageType == null && createIfNull)
			{
				_makeDamageType = new SpecialEffectList();
			}
			return _makeDamageType;
		case 80:
			if (_canMakeInjuryToNoInjuryPart == null && createIfNull)
			{
				_canMakeInjuryToNoInjuryPart = new SpecialEffectList();
			}
			return _canMakeInjuryToNoInjuryPart;
		case 81:
			if (_makePoisonType == null && createIfNull)
			{
				_makePoisonType = new SpecialEffectList();
			}
			return _makePoisonType;
		case 82:
			if (_normalAttackWeapon == null && createIfNull)
			{
				_normalAttackWeapon = new SpecialEffectList();
			}
			return _normalAttackWeapon;
		case 83:
			if (_normalAttackTrick == null && createIfNull)
			{
				_normalAttackTrick = new SpecialEffectList();
			}
			return _normalAttackTrick;
		case 328:
			if (_normalAttackGetTrickCount == null && createIfNull)
			{
				_normalAttackGetTrickCount = new SpecialEffectList();
			}
			return _normalAttackGetTrickCount;
		case 283:
			if (_normalAttackPrepareFrame == null && createIfNull)
			{
				_normalAttackPrepareFrame = new SpecialEffectList();
			}
			return _normalAttackPrepareFrame;
		case 321:
			if (_normalAttackRecoveryFrame == null && createIfNull)
			{
				_normalAttackRecoveryFrame = new SpecialEffectList();
			}
			return _normalAttackRecoveryFrame;
		case 317:
			if (_unlockSpeed == null && createIfNull)
			{
				_unlockSpeed = new SpecialEffectList();
			}
			return _unlockSpeed;
		case 307:
			if (_normalAttackChangeToUnlockAttack == null && createIfNull)
			{
				_normalAttackChangeToUnlockAttack = new SpecialEffectList();
			}
			return _normalAttackChangeToUnlockAttack;
		case 312:
			if (_rawCreateEffectList == null && createIfNull)
			{
				_rawCreateEffectList = new SpecialEffectList();
			}
			return _rawCreateEffectList;
		case 84:
			if (_extraFlawCount == null && createIfNull)
			{
				_extraFlawCount = new SpecialEffectList();
			}
			return _extraFlawCount;
		case 318:
			if (_flawBonusFactor == null && createIfNull)
			{
				_flawBonusFactor = new SpecialEffectList();
			}
			return _flawBonusFactor;
		case 85:
			if (_attackCanBounce == null && createIfNull)
			{
				_attackCanBounce = new SpecialEffectList();
			}
			return _attackCanBounce;
		case 86:
			if (_attackCanFightBack == null && createIfNull)
			{
				_attackCanFightBack = new SpecialEffectList();
			}
			return _attackCanFightBack;
		case 252:
			if (_attackCanPursue == null && createIfNull)
			{
				_attackCanPursue = new SpecialEffectList();
			}
			return _attackCanPursue;
		case 87:
			if (_makeFightBackInjuryMark == null && createIfNull)
			{
				_makeFightBackInjuryMark = new SpecialEffectList();
			}
			return _makeFightBackInjuryMark;
		case 88:
			if (_legSkillUseShoes == null && createIfNull)
			{
				_legSkillUseShoes = new SpecialEffectList();
			}
			return _legSkillUseShoes;
		case 323:
			if (_attackerDirectFinalDamageValue == null && createIfNull)
			{
				_attackerDirectFinalDamageValue = new SpecialEffectList();
			}
			return _attackerDirectFinalDamageValue;
		case 89:
			if (_attackerFinalDamageValue == null && createIfNull)
			{
				_attackerFinalDamageValue = new SpecialEffectList();
			}
			return _attackerFinalDamageValue;
		case 90:
			if (_defenderHitStrength == null && createIfNull)
			{
				_defenderHitStrength = new SpecialEffectList();
			}
			return _defenderHitStrength;
		case 91:
			if (_defenderHitTechnique == null && createIfNull)
			{
				_defenderHitTechnique = new SpecialEffectList();
			}
			return _defenderHitTechnique;
		case 92:
			if (_defenderHitSpeed == null && createIfNull)
			{
				_defenderHitSpeed = new SpecialEffectList();
			}
			return _defenderHitSpeed;
		case 93:
			if (_defenderHitMind == null && createIfNull)
			{
				_defenderHitMind = new SpecialEffectList();
			}
			return _defenderHitMind;
		case 94:
			if (_defenderAvoidStrength == null && createIfNull)
			{
				_defenderAvoidStrength = new SpecialEffectList();
			}
			return _defenderAvoidStrength;
		case 95:
			if (_defenderAvoidTechnique == null && createIfNull)
			{
				_defenderAvoidTechnique = new SpecialEffectList();
			}
			return _defenderAvoidTechnique;
		case 96:
			if (_defenderAvoidSpeed == null && createIfNull)
			{
				_defenderAvoidSpeed = new SpecialEffectList();
			}
			return _defenderAvoidSpeed;
		case 97:
			if (_defenderAvoidMind == null && createIfNull)
			{
				_defenderAvoidMind = new SpecialEffectList();
			}
			return _defenderAvoidMind;
		case 98:
			if (_defenderPenetrateOuter == null && createIfNull)
			{
				_defenderPenetrateOuter = new SpecialEffectList();
			}
			return _defenderPenetrateOuter;
		case 99:
			if (_defenderPenetrateInner == null && createIfNull)
			{
				_defenderPenetrateInner = new SpecialEffectList();
			}
			return _defenderPenetrateInner;
		case 100:
			if (_defenderPenetrateResistOuter == null && createIfNull)
			{
				_defenderPenetrateResistOuter = new SpecialEffectList();
			}
			return _defenderPenetrateResistOuter;
		case 101:
			if (_defenderPenetrateResistInner == null && createIfNull)
			{
				_defenderPenetrateResistInner = new SpecialEffectList();
			}
			return _defenderPenetrateResistInner;
		case 102:
			if (_acceptDirectDamage == null && createIfNull)
			{
				_acceptDirectDamage = new SpecialEffectList();
			}
			return _acceptDirectDamage;
		case 276:
			if (_acceptMindDamage == null && createIfNull)
			{
				_acceptMindDamage = new SpecialEffectList();
			}
			return _acceptMindDamage;
		case 103:
			if (_acceptBounceDamage == null && createIfNull)
			{
				_acceptBounceDamage = new SpecialEffectList();
			}
			return _acceptBounceDamage;
		case 104:
			if (_acceptFightBackDamage == null && createIfNull)
			{
				_acceptFightBackDamage = new SpecialEffectList();
			}
			return _acceptFightBackDamage;
		case 105:
			if (_acceptPoisonLevel == null && createIfNull)
			{
				_acceptPoisonLevel = new SpecialEffectList();
			}
			return _acceptPoisonLevel;
		case 106:
			if (_acceptPoisonValue == null && createIfNull)
			{
				_acceptPoisonValue = new SpecialEffectList();
			}
			return _acceptPoisonValue;
		case 232:
			if (_acceptPoisonResist == null && createIfNull)
			{
				_acceptPoisonResist = new SpecialEffectList();
			}
			return _acceptPoisonResist;
		case 247:
			if (_acceptPoisonTarget == null && createIfNull)
			{
				_acceptPoisonTarget = new SpecialEffectList();
			}
			return _acceptPoisonTarget;
		case 107:
			if (_defenderHitOdds == null && createIfNull)
			{
				_defenderHitOdds = new SpecialEffectList();
			}
			return _defenderHitOdds;
		case 108:
			if (_defenderFightBackHitOdds == null && createIfNull)
			{
				_defenderFightBackHitOdds = new SpecialEffectList();
			}
			return _defenderFightBackHitOdds;
		case 109:
			if (_defenderPursueOdds == null && createIfNull)
			{
				_defenderPursueOdds = new SpecialEffectList();
			}
			return _defenderPursueOdds;
		case 326:
			if (_acceptDamageCanAdd == null && createIfNull)
			{
				_acceptDamageCanAdd = new SpecialEffectList();
			}
			return _acceptDamageCanAdd;
		case 110:
			if (_acceptMaxInjuryCount == null && createIfNull)
			{
				_acceptMaxInjuryCount = new SpecialEffectList();
			}
			return _acceptMaxInjuryCount;
		case 111:
			if (_bouncePower == null && createIfNull)
			{
				_bouncePower = new SpecialEffectList();
			}
			return _bouncePower;
		case 112:
			if (_fightBackPower == null && createIfNull)
			{
				_fightBackPower = new SpecialEffectList();
			}
			return _fightBackPower;
		case 113:
			if (_directDamageInnerRatio == null && createIfNull)
			{
				_directDamageInnerRatio = new SpecialEffectList();
			}
			return _directDamageInnerRatio;
		case 320:
			if (_defenderDirectFinalDamageValue == null && createIfNull)
			{
				_defenderDirectFinalDamageValue = new SpecialEffectList();
			}
			return _defenderDirectFinalDamageValue;
		case 114:
			if (_defenderFinalDamageValue == null && createIfNull)
			{
				_defenderFinalDamageValue = new SpecialEffectList();
			}
			return _defenderFinalDamageValue;
		case 282:
			if (_unyieldingFallen == null && createIfNull)
			{
				_unyieldingFallen = new SpecialEffectList();
			}
			return _unyieldingFallen;
		case 242:
			if (_outerInjuryImmunity == null && createIfNull)
			{
				_outerInjuryImmunity = new SpecialEffectList();
			}
			return _outerInjuryImmunity;
		case 241:
			if (_innerInjuryImmunity == null && createIfNull)
			{
				_innerInjuryImmunity = new SpecialEffectList();
			}
			return _innerInjuryImmunity;
		case 115:
			if (_directDamageValue == null && createIfNull)
			{
				_directDamageValue = new SpecialEffectList();
			}
			return _directDamageValue;
		case 116:
			if (_directInjuryMark == null && createIfNull)
			{
				_directInjuryMark = new SpecialEffectList();
			}
			return _directInjuryMark;
		case 117:
			if (_goneMadInjury == null && createIfNull)
			{
				_goneMadInjury = new SpecialEffectList();
			}
			return _goneMadInjury;
		case 322:
			if (_finalGoneMadInjury == null && createIfNull)
			{
				_finalGoneMadInjury = new SpecialEffectList();
			}
			return _finalGoneMadInjury;
		case 118:
			if (_healInjurySpeed == null && createIfNull)
			{
				_healInjurySpeed = new SpecialEffectList();
			}
			return _healInjurySpeed;
		case 119:
			if (_healInjuryBuff == null && createIfNull)
			{
				_healInjuryBuff = new SpecialEffectList();
			}
			return _healInjuryBuff;
		case 120:
			if (_healInjuryDebuff == null && createIfNull)
			{
				_healInjuryDebuff = new SpecialEffectList();
			}
			return _healInjuryDebuff;
		case 121:
			if (_healPoisonSpeed == null && createIfNull)
			{
				_healPoisonSpeed = new SpecialEffectList();
			}
			return _healPoisonSpeed;
		case 122:
			if (_healPoisonBuff == null && createIfNull)
			{
				_healPoisonBuff = new SpecialEffectList();
			}
			return _healPoisonBuff;
		case 123:
			if (_healPoisonDebuff == null && createIfNull)
			{
				_healPoisonDebuff = new SpecialEffectList();
			}
			return _healPoisonDebuff;
		case 261:
			if (_medicineEffect == null && createIfNull)
			{
				_medicineEffect = new SpecialEffectList();
			}
			return _medicineEffect;
		case 316:
			if (_healFlawSpeed == null && createIfNull)
			{
				_healFlawSpeed = new SpecialEffectList();
			}
			return _healFlawSpeed;
		case 300:
			if (_healAcupointSpeed == null && createIfNull)
			{
				_healAcupointSpeed = new SpecialEffectList();
			}
			return _healAcupointSpeed;
		case 124:
			if (_fleeSpeed == null && createIfNull)
			{
				_fleeSpeed = new SpecialEffectList();
			}
			return _fleeSpeed;
		case 125:
			if (_maxFlawCount == null && createIfNull)
			{
				_maxFlawCount = new SpecialEffectList();
			}
			return _maxFlawCount;
		case 126:
			if (_canAddFlaw == null && createIfNull)
			{
				_canAddFlaw = new SpecialEffectList();
			}
			return _canAddFlaw;
		case 127:
			if (_flawLevel == null && createIfNull)
			{
				_flawLevel = new SpecialEffectList();
			}
			return _flawLevel;
		case 128:
			if (_flawLevelCanReduce == null && createIfNull)
			{
				_flawLevelCanReduce = new SpecialEffectList();
			}
			return _flawLevelCanReduce;
		case 129:
			if (_flawCount == null && createIfNull)
			{
				_flawCount = new SpecialEffectList();
			}
			return _flawCount;
		case 130:
			if (_maxAcupointCount == null && createIfNull)
			{
				_maxAcupointCount = new SpecialEffectList();
			}
			return _maxAcupointCount;
		case 131:
			if (_canAddAcupoint == null && createIfNull)
			{
				_canAddAcupoint = new SpecialEffectList();
			}
			return _canAddAcupoint;
		case 132:
			if (_acupointLevel == null && createIfNull)
			{
				_acupointLevel = new SpecialEffectList();
			}
			return _acupointLevel;
		case 133:
			if (_acupointLevelCanReduce == null && createIfNull)
			{
				_acupointLevelCanReduce = new SpecialEffectList();
			}
			return _acupointLevelCanReduce;
		case 134:
			if (_acupointCount == null && createIfNull)
			{
				_acupointCount = new SpecialEffectList();
			}
			return _acupointCount;
		case 135:
			if (_addNeiliAllocation == null && createIfNull)
			{
				_addNeiliAllocation = new SpecialEffectList();
			}
			return _addNeiliAllocation;
		case 136:
			if (_costNeiliAllocation == null && createIfNull)
			{
				_costNeiliAllocation = new SpecialEffectList();
			}
			return _costNeiliAllocation;
		case 137:
			if (_canChangeNeiliAllocation == null && createIfNull)
			{
				_canChangeNeiliAllocation = new SpecialEffectList();
			}
			return _canChangeNeiliAllocation;
		case 138:
			if (_canGetTrick == null && createIfNull)
			{
				_canGetTrick = new SpecialEffectList();
			}
			return _canGetTrick;
		case 139:
			if (_getTrickType == null && createIfNull)
			{
				_getTrickType = new SpecialEffectList();
			}
			return _getTrickType;
		case 140:
			if (_attackBodyPart == null && createIfNull)
			{
				_attackBodyPart = new SpecialEffectList();
			}
			return _attackBodyPart;
		case 308:
			if (_attackBodyPartOdds == null && createIfNull)
			{
				_attackBodyPartOdds = new SpecialEffectList();
			}
			return _attackBodyPartOdds;
		case 141:
			if (_weaponEquipAttack == null && createIfNull)
			{
				_weaponEquipAttack = new SpecialEffectList();
			}
			return _weaponEquipAttack;
		case 142:
			if (_weaponEquipDefense == null && createIfNull)
			{
				_weaponEquipDefense = new SpecialEffectList();
			}
			return _weaponEquipDefense;
		case 143:
			if (_armorEquipAttack == null && createIfNull)
			{
				_armorEquipAttack = new SpecialEffectList();
			}
			return _armorEquipAttack;
		case 144:
			if (_armorEquipDefense == null && createIfNull)
			{
				_armorEquipDefense = new SpecialEffectList();
			}
			return _armorEquipDefense;
		case 311:
			if (_equipmentWeight == null && createIfNull)
			{
				_equipmentWeight = new SpecialEffectList();
			}
			return _equipmentWeight;
		case 145:
			if (_attackRangeForward == null && createIfNull)
			{
				_attackRangeForward = new SpecialEffectList();
			}
			return _attackRangeForward;
		case 146:
			if (_attackRangeBackward == null && createIfNull)
			{
				_attackRangeBackward = new SpecialEffectList();
			}
			return _attackRangeBackward;
		case 273:
			if (_attackRangeMaxAcupoint == null && createIfNull)
			{
				_attackRangeMaxAcupoint = new SpecialEffectList();
			}
			return _attackRangeMaxAcupoint;
		case 148:
			if (_canForcedMove == null && createIfNull)
			{
				_canForcedMove = new SpecialEffectList();
			}
			return _canForcedMove;
		case 149:
			if (_mobilityCanBeRemoved == null && createIfNull)
			{
				_mobilityCanBeRemoved = new SpecialEffectList();
			}
			return _mobilityCanBeRemoved;
		case 150:
			if (_mobilityCostByEffect == null && createIfNull)
			{
				_mobilityCostByEffect = new SpecialEffectList();
			}
			return _mobilityCostByEffect;
		case 255:
			if (_stanceCostByEffect == null && createIfNull)
			{
				_stanceCostByEffect = new SpecialEffectList();
			}
			return _stanceCostByEffect;
		case 256:
			if (_breathCostByEffect == null && createIfNull)
			{
				_breathCostByEffect = new SpecialEffectList();
			}
			return _breathCostByEffect;
		case 147:
			if (_moveCanBeStopped == null && createIfNull)
			{
				_moveCanBeStopped = new SpecialEffectList();
			}
			return _moveCanBeStopped;
		case 151:
			if (_moveDistance == null && createIfNull)
			{
				_moveDistance = new SpecialEffectList();
			}
			return _moveDistance;
		case 244:
			if (_lockDistance == null && createIfNull)
			{
				_lockDistance = new SpecialEffectList();
			}
			return _lockDistance;
		case 152:
			if (_jumpPrepareFrame == null && createIfNull)
			{
				_jumpPrepareFrame = new SpecialEffectList();
			}
			return _jumpPrepareFrame;
		case 153:
			if (_bounceInjuryMark == null && createIfNull)
			{
				_bounceInjuryMark = new SpecialEffectList();
			}
			return _bounceInjuryMark;
		case 154:
			if (_skillHasCost == null && createIfNull)
			{
				_skillHasCost = new SpecialEffectList();
			}
			return _skillHasCost;
		case 155:
			if (_combatStateEffect == null && createIfNull)
			{
				_combatStateEffect = new SpecialEffectList();
			}
			return _combatStateEffect;
		case 156:
			if (_changeNeedUseSkill == null && createIfNull)
			{
				_changeNeedUseSkill = new SpecialEffectList();
			}
			return _changeNeedUseSkill;
		case 157:
			if (_changeDistanceIsMove == null && createIfNull)
			{
				_changeDistanceIsMove = new SpecialEffectList();
			}
			return _changeDistanceIsMove;
		case 158:
			if (_replaceCharHit == null && createIfNull)
			{
				_replaceCharHit = new SpecialEffectList();
			}
			return _replaceCharHit;
		case 159:
			if (_canAddPoison == null && createIfNull)
			{
				_canAddPoison = new SpecialEffectList();
			}
			return _canAddPoison;
		case 160:
			if (_canReducePoison == null && createIfNull)
			{
				_canReducePoison = new SpecialEffectList();
			}
			return _canReducePoison;
		case 161:
			if (_reducePoisonValue == null && createIfNull)
			{
				_reducePoisonValue = new SpecialEffectList();
			}
			return _reducePoisonValue;
		case 162:
			if (_poisonCanAffect == null && createIfNull)
			{
				_poisonCanAffect = new SpecialEffectList();
			}
			return _poisonCanAffect;
		case 163:
			if (_poisonAffectCount == null && createIfNull)
			{
				_poisonAffectCount = new SpecialEffectList();
			}
			return _poisonAffectCount;
		case 243:
			if (_poisonAffectThreshold == null && createIfNull)
			{
				_poisonAffectThreshold = new SpecialEffectList();
			}
			return _poisonAffectThreshold;
		case 259:
			if (_poisonAffectProduceValue == null && createIfNull)
			{
				_poisonAffectProduceValue = new SpecialEffectList();
			}
			return _poisonAffectProduceValue;
		case 272:
			if (_mixPoisonInfinityAffect == null && createIfNull)
			{
				_mixPoisonInfinityAffect = new SpecialEffectList();
			}
			return _mixPoisonInfinityAffect;
		case 164:
			if (_costTricks == null && createIfNull)
			{
				_costTricks = new SpecialEffectList();
			}
			return _costTricks;
		case 313:
			if (_jiTrickAsWeaponTrickCount == null && createIfNull)
			{
				_jiTrickAsWeaponTrickCount = new SpecialEffectList();
			}
			return _jiTrickAsWeaponTrickCount;
		case 314:
			if (_uselessTrickAsJiTrickCount == null && createIfNull)
			{
				_uselessTrickAsJiTrickCount = new SpecialEffectList();
			}
			return _uselessTrickAsJiTrickCount;
		case 309:
			if (_changeDurability == null && createIfNull)
			{
				_changeDurability = new SpecialEffectList();
			}
			return _changeDurability;
		case 165:
			if (_jumpMoveDistance == null && createIfNull)
			{
				_jumpMoveDistance = new SpecialEffectList();
			}
			return _jumpMoveDistance;
		case 166:
			if (_combatStateToAdd == null && createIfNull)
			{
				_combatStateToAdd = new SpecialEffectList();
			}
			return _combatStateToAdd;
		case 167:
			if (_combatStatePower == null && createIfNull)
			{
				_combatStatePower = new SpecialEffectList();
			}
			return _combatStatePower;
		case 168:
			if (_breakBodyPartInjuryCount == null && createIfNull)
			{
				_breakBodyPartInjuryCount = new SpecialEffectList();
			}
			return _breakBodyPartInjuryCount;
		case 169:
			if (_bodyPartIsBroken == null && createIfNull)
			{
				_bodyPartIsBroken = new SpecialEffectList();
			}
			return _bodyPartIsBroken;
		case 170:
			if (_maxTrickCount == null && createIfNull)
			{
				_maxTrickCount = new SpecialEffectList();
			}
			return _maxTrickCount;
		case 171:
			if (_maxBreathPercent == null && createIfNull)
			{
				_maxBreathPercent = new SpecialEffectList();
			}
			return _maxBreathPercent;
		case 172:
			if (_maxStancePercent == null && createIfNull)
			{
				_maxStancePercent = new SpecialEffectList();
			}
			return _maxStancePercent;
		case 274:
			if (_maxMobilityPercent == null && createIfNull)
			{
				_maxMobilityPercent = new SpecialEffectList();
			}
			return _maxMobilityPercent;
		case 173:
			if (_extraBreathPercent == null && createIfNull)
			{
				_extraBreathPercent = new SpecialEffectList();
			}
			return _extraBreathPercent;
		case 174:
			if (_extraStancePercent == null && createIfNull)
			{
				_extraStancePercent = new SpecialEffectList();
			}
			return _extraStancePercent;
		case 175:
			if (_moveCostMobility == null && createIfNull)
			{
				_moveCostMobility = new SpecialEffectList();
			}
			return _moveCostMobility;
		case 176:
			if (_defendSkillKeepTime == null && createIfNull)
			{
				_defendSkillKeepTime = new SpecialEffectList();
			}
			return _defendSkillKeepTime;
		case 177:
			if (_bounceRange == null && createIfNull)
			{
				_bounceRange = new SpecialEffectList();
			}
			return _bounceRange;
		case 178:
			if (_mindMarkKeepTime == null && createIfNull)
			{
				_mindMarkKeepTime = new SpecialEffectList();
			}
			return _mindMarkKeepTime;
		case 249:
			if (_mindMarkCount == null && createIfNull)
			{
				_mindMarkCount = new SpecialEffectList();
			}
			return _mindMarkCount;
		case 179:
			if (_skillMobilityCostPerFrame == null && createIfNull)
			{
				_skillMobilityCostPerFrame = new SpecialEffectList();
			}
			return _skillMobilityCostPerFrame;
		case 180:
			if (_canAddWug == null && createIfNull)
			{
				_canAddWug = new SpecialEffectList();
			}
			return _canAddWug;
		case 181:
			if (_hasGodWeaponBuff == null && createIfNull)
			{
				_hasGodWeaponBuff = new SpecialEffectList();
			}
			return _hasGodWeaponBuff;
		case 182:
			if (_hasGodArmorBuff == null && createIfNull)
			{
				_hasGodArmorBuff = new SpecialEffectList();
			}
			return _hasGodArmorBuff;
		case 183:
			if (_teammateCmdRequireGenerateValue == null && createIfNull)
			{
				_teammateCmdRequireGenerateValue = new SpecialEffectList();
			}
			return _teammateCmdRequireGenerateValue;
		case 184:
			if (_teammateCmdEffect == null && createIfNull)
			{
				_teammateCmdEffect = new SpecialEffectList();
			}
			return _teammateCmdEffect;
		case 271:
			if (_teammateCmdCanUse == null && createIfNull)
			{
				_teammateCmdCanUse = new SpecialEffectList();
			}
			return _teammateCmdCanUse;
		case 185:
			if (_flawRecoverSpeed == null && createIfNull)
			{
				_flawRecoverSpeed = new SpecialEffectList();
			}
			return _flawRecoverSpeed;
		case 186:
			if (_acupointRecoverSpeed == null && createIfNull)
			{
				_acupointRecoverSpeed = new SpecialEffectList();
			}
			return _acupointRecoverSpeed;
		case 187:
			if (_mindMarkRecoverSpeed == null && createIfNull)
			{
				_mindMarkRecoverSpeed = new SpecialEffectList();
			}
			return _mindMarkRecoverSpeed;
		case 188:
			if (_injuryAutoHealSpeed == null && createIfNull)
			{
				_injuryAutoHealSpeed = new SpecialEffectList();
			}
			return _injuryAutoHealSpeed;
		case 189:
			if (_canRecoverBreath == null && createIfNull)
			{
				_canRecoverBreath = new SpecialEffectList();
			}
			return _canRecoverBreath;
		case 190:
			if (_canRecoverStance == null && createIfNull)
			{
				_canRecoverStance = new SpecialEffectList();
			}
			return _canRecoverStance;
		case 191:
			if (_fatalDamageValue == null && createIfNull)
			{
				_fatalDamageValue = new SpecialEffectList();
			}
			return _fatalDamageValue;
		case 294:
			if (_wugFatalDamageValue == null && createIfNull)
			{
				_wugFatalDamageValue = new SpecialEffectList();
			}
			return _wugFatalDamageValue;
		case 192:
			if (_fatalDamageMarkCount == null && createIfNull)
			{
				_fatalDamageMarkCount = new SpecialEffectList();
			}
			return _fatalDamageMarkCount;
		case 304:
			if (_finalFatalDamageMarkCount == null && createIfNull)
			{
				_finalFatalDamageMarkCount = new SpecialEffectList();
			}
			return _finalFatalDamageMarkCount;
		case 193:
			if (_canFightBackDuringPrepareSkill == null && createIfNull)
			{
				_canFightBackDuringPrepareSkill = new SpecialEffectList();
			}
			return _canFightBackDuringPrepareSkill;
		case 250:
			if (_canFightBackWithHit == null && createIfNull)
			{
				_canFightBackWithHit = new SpecialEffectList();
			}
			return _canFightBackWithHit;
		case 194:
			if (_skillPrepareSpeed == null && createIfNull)
			{
				_skillPrepareSpeed = new SpecialEffectList();
			}
			return _skillPrepareSpeed;
		case 195:
			if (_breathRecoverSpeed == null && createIfNull)
			{
				_breathRecoverSpeed = new SpecialEffectList();
			}
			return _breathRecoverSpeed;
		case 196:
			if (_stanceRecoverSpeed == null && createIfNull)
			{
				_stanceRecoverSpeed = new SpecialEffectList();
			}
			return _stanceRecoverSpeed;
		case 197:
			if (_mobilityRecoverSpeed == null && createIfNull)
			{
				_mobilityRecoverSpeed = new SpecialEffectList();
			}
			return _mobilityRecoverSpeed;
		case 301:
			if (_maxChangeTrickCount == null && createIfNull)
			{
				_maxChangeTrickCount = new SpecialEffectList();
			}
			return _maxChangeTrickCount;
		case 198:
			if (_changeTrickProgressAddValue == null && createIfNull)
			{
				_changeTrickProgressAddValue = new SpecialEffectList();
			}
			return _changeTrickProgressAddValue;
		case 287:
			if (_agileSkillCanAffect == null && createIfNull)
			{
				_agileSkillCanAffect = new SpecialEffectList();
			}
			return _agileSkillCanAffect;
		case 285:
			if (_defendSkillCanAffect == null && createIfNull)
			{
				_defendSkillCanAffect = new SpecialEffectList();
			}
			return _defendSkillCanAffect;
		case 286:
			if (_assistSkillCanAffect == null && createIfNull)
			{
				_assistSkillCanAffect = new SpecialEffectList();
			}
			return _assistSkillCanAffect;
		case 199:
			if (_power == null && createIfNull)
			{
				_power = new SpecialEffectList();
			}
			return _power;
		case 200:
			if (_maxPower == null && createIfNull)
			{
				_maxPower = new SpecialEffectList();
			}
			return _maxPower;
		case 201:
			if (_powerCanReduce == null && createIfNull)
			{
				_powerCanReduce = new SpecialEffectList();
			}
			return _powerCanReduce;
		case 257:
			if (_powerAddRatio == null && createIfNull)
			{
				_powerAddRatio = new SpecialEffectList();
			}
			return _powerAddRatio;
		case 258:
			if (_powerReduceRatio == null && createIfNull)
			{
				_powerReduceRatio = new SpecialEffectList();
			}
			return _powerReduceRatio;
		case 292:
			if (_powerEffectReverse == null && createIfNull)
			{
				_powerEffectReverse = new SpecialEffectList();
			}
			return _powerEffectReverse;
		case 202:
			if (_useRequirement == null && createIfNull)
			{
				_useRequirement = new SpecialEffectList();
			}
			return _useRequirement;
		case 203:
			if (_currInnerRatio == null && createIfNull)
			{
				_currInnerRatio = new SpecialEffectList();
			}
			return _currInnerRatio;
		case 204:
			if (_costBreathAndStance == null && createIfNull)
			{
				_costBreathAndStance = new SpecialEffectList();
			}
			return _costBreathAndStance;
		case 205:
			if (_costBreath == null && createIfNull)
			{
				_costBreath = new SpecialEffectList();
			}
			return _costBreath;
		case 206:
			if (_costStance == null && createIfNull)
			{
				_costStance = new SpecialEffectList();
			}
			return _costStance;
		case 207:
			if (_costMobility == null && createIfNull)
			{
				_costMobility = new SpecialEffectList();
			}
			return _costMobility;
		case 208:
			if (_skillCostTricks == null && createIfNull)
			{
				_skillCostTricks = new SpecialEffectList();
			}
			return _skillCostTricks;
		case 280:
			if (_canCostEnemyUsableTricks == null && createIfNull)
			{
				_canCostEnemyUsableTricks = new SpecialEffectList();
			}
			return _canCostEnemyUsableTricks;
		case 284:
			if (_canCostUselessTricks == null && createIfNull)
			{
				_canCostUselessTricks = new SpecialEffectList();
			}
			return _canCostUselessTricks;
		case 319:
			if (_canCostShaTricks == null && createIfNull)
			{
				_canCostShaTricks = new SpecialEffectList();
			}
			return _canCostShaTricks;
		case 209:
			if (_effectDirection == null && createIfNull)
			{
				_effectDirection = new SpecialEffectList();
			}
			return _effectDirection;
		case 210:
			if (_effectDirectionCanChange == null && createIfNull)
			{
				_effectDirectionCanChange = new SpecialEffectList();
			}
			return _effectDirectionCanChange;
		case 211:
			if (_gridCost == null && createIfNull)
			{
				_gridCost = new SpecialEffectList();
			}
			return _gridCost;
		case 212:
			if (_prepareTotalProgress == null && createIfNull)
			{
				_prepareTotalProgress = new SpecialEffectList();
			}
			return _prepareTotalProgress;
		case 213:
			if (_specificGridCount == null && createIfNull)
			{
				_specificGridCount = new SpecialEffectList();
			}
			return _specificGridCount;
		case 214:
			if (_genericGridCount == null && createIfNull)
			{
				_genericGridCount = new SpecialEffectList();
			}
			return _genericGridCount;
		case 234:
			if (_canCriticalHit == null && createIfNull)
			{
				_canCriticalHit = new SpecialEffectList();
			}
			return _canCriticalHit;
		case 254:
			if (_criticalOdds == null && createIfNull)
			{
				_criticalOdds = new SpecialEffectList();
			}
			return _criticalOdds;
		case 248:
			if (_certainCriticalHit == null && createIfNull)
			{
				_certainCriticalHit = new SpecialEffectList();
			}
			return _certainCriticalHit;
		case 251:
			if (_inevitableHit == null && createIfNull)
			{
				_inevitableHit = new SpecialEffectList();
			}
			return _inevitableHit;
		case 291:
			if (_inevitableAvoid == null && createIfNull)
			{
				_inevitableAvoid = new SpecialEffectList();
			}
			return _inevitableAvoid;
		case 215:
			if (_canInterrupt == null && createIfNull)
			{
				_canInterrupt = new SpecialEffectList();
			}
			return _canInterrupt;
		case 216:
			if (_interruptOdds == null && createIfNull)
			{
				_interruptOdds = new SpecialEffectList();
			}
			return _interruptOdds;
		case 217:
			if (_canSilence == null && createIfNull)
			{
				_canSilence = new SpecialEffectList();
			}
			return _canSilence;
		case 218:
			if (_silenceOdds == null && createIfNull)
			{
				_silenceOdds = new SpecialEffectList();
			}
			return _silenceOdds;
		case 265:
			if (_silenceFrame == null && createIfNull)
			{
				_silenceFrame = new SpecialEffectList();
			}
			return _silenceFrame;
		case 264:
			if (_weaponSilenceFrame == null && createIfNull)
			{
				_weaponSilenceFrame = new SpecialEffectList();
			}
			return _weaponSilenceFrame;
		case 219:
			if (_canCastWithBrokenBodyPart == null && createIfNull)
			{
				_canCastWithBrokenBodyPart = new SpecialEffectList();
			}
			return _canCastWithBrokenBodyPart;
		case 220:
			if (_addPowerCanBeRemoved == null && createIfNull)
			{
				_addPowerCanBeRemoved = new SpecialEffectList();
			}
			return _addPowerCanBeRemoved;
		case 221:
			if (_skillType == null && createIfNull)
			{
				_skillType = new SpecialEffectList();
			}
			return _skillType;
		case 222:
			if (_effectCountCanChange == null && createIfNull)
			{
				_effectCountCanChange = new SpecialEffectList();
			}
			return _effectCountCanChange;
		case 290:
			if (_canCast == null && createIfNull)
			{
				_canCast = new SpecialEffectList();
			}
			return _canCast;
		case 223:
			if (_canCastInDefend == null && createIfNull)
			{
				_canCastInDefend = new SpecialEffectList();
			}
			return _canCastInDefend;
		case 224:
			if (_hitDistribution == null && createIfNull)
			{
				_hitDistribution = new SpecialEffectList();
			}
			return _hitDistribution;
		case 225:
			if (_canCastOnLackBreath == null && createIfNull)
			{
				_canCastOnLackBreath = new SpecialEffectList();
			}
			return _canCastOnLackBreath;
		case 226:
			if (_canCastOnLackStance == null && createIfNull)
			{
				_canCastOnLackStance = new SpecialEffectList();
			}
			return _canCastOnLackStance;
		case 302:
			if (_convertCostBreathAndStance == null && createIfNull)
			{
				_convertCostBreathAndStance = new SpecialEffectList();
			}
			return _convertCostBreathAndStance;
		case 227:
			if (_costBreathOnCast == null && createIfNull)
			{
				_costBreathOnCast = new SpecialEffectList();
			}
			return _costBreathOnCast;
		case 228:
			if (_costStanceOnCast == null && createIfNull)
			{
				_costStanceOnCast = new SpecialEffectList();
			}
			return _costStanceOnCast;
		case 229:
			if (_canUseMobilityAsBreath == null && createIfNull)
			{
				_canUseMobilityAsBreath = new SpecialEffectList();
			}
			return _canUseMobilityAsBreath;
		case 230:
			if (_canUseMobilityAsStance == null && createIfNull)
			{
				_canUseMobilityAsStance = new SpecialEffectList();
			}
			return _canUseMobilityAsStance;
		case 231:
			if (_castCostNeiliAllocation == null && createIfNull)
			{
				_castCostNeiliAllocation = new SpecialEffectList();
			}
			return _castCostNeiliAllocation;
		case 235:
			if (_canCostNeiliAllocationEffect == null && createIfNull)
			{
				_canCostNeiliAllocationEffect = new SpecialEffectList();
			}
			return _canCostNeiliAllocationEffect;
		case 324:
			if (_canCostTrickDuringPreparingSkill == null && createIfNull)
			{
				_canCostTrickDuringPreparingSkill = new SpecialEffectList();
			}
			return _canCostTrickDuringPreparingSkill;
		case 325:
			if (_validItemList == null && createIfNull)
			{
				_validItemList = new SpecialEffectList();
			}
			return _validItemList;
		case 253:
			if (_combatSkillDataEffectList == null && createIfNull)
			{
				_combatSkillDataEffectList = new SpecialEffectList();
			}
			return _combatSkillDataEffectList;
		case 306:
			if (_combatSkillAiScorePower == null && createIfNull)
			{
				_combatSkillAiScorePower = new SpecialEffectList();
			}
			return _combatSkillAiScorePower;
		default:
			throw new Exception($"AffectedData filed with id {fieldId} not found");
		}
	}

	public void SetEffectList(DataContext context, ushort fieldId, SpecialEffectList effectList)
	{
		switch (fieldId)
		{
		case 1:
			SetMaxStrength(effectList, context);
			return;
		case 2:
			SetMaxDexterity(effectList, context);
			return;
		case 3:
			SetMaxConcentration(effectList, context);
			return;
		case 4:
			SetMaxVitality(effectList, context);
			return;
		case 5:
			SetMaxEnergy(effectList, context);
			return;
		case 6:
			SetMaxIntelligence(effectList, context);
			return;
		case 7:
			SetRecoveryOfStance(effectList, context);
			return;
		case 8:
			SetRecoveryOfBreath(effectList, context);
			return;
		case 9:
			SetMoveSpeed(effectList, context);
			return;
		case 10:
			SetRecoveryOfFlaw(effectList, context);
			return;
		case 11:
			SetCastSpeed(effectList, context);
			return;
		case 12:
			SetRecoveryOfBlockedAcupoint(effectList, context);
			return;
		case 13:
			SetWeaponSwitchSpeed(effectList, context);
			return;
		case 14:
			SetAttackSpeed(effectList, context);
			return;
		case 15:
			SetInnerRatio(effectList, context);
			return;
		case 16:
			SetRecoveryOfQiDisorder(effectList, context);
			return;
		case 17:
			SetMinorAttributeFixMaxValue(effectList, context);
			return;
		case 18:
			SetMinorAttributeFixMinValue(effectList, context);
			return;
		case 19:
			SetResistOfHotPoison(effectList, context);
			return;
		case 20:
			SetResistOfGloomyPoison(effectList, context);
			return;
		case 21:
			SetResistOfColdPoison(effectList, context);
			return;
		case 22:
			SetResistOfRedPoison(effectList, context);
			return;
		case 23:
			SetResistOfRottenPoison(effectList, context);
			return;
		case 24:
			SetResistOfIllusoryPoison(effectList, context);
			return;
		case 245:
			SetResistOfAllPoison(effectList, context);
			return;
		case 303:
			SetPersonalitiesAll(effectList, context);
			return;
		case 25:
			SetDisplayAge(effectList, context);
			return;
		case 26:
			SetNeiliProportionOfFiveElements(effectList, context);
			return;
		case 240:
			SetSkillAlsoAsFiveElements(effectList, context);
			return;
		case 27:
			SetWeaponMaxPower(effectList, context);
			return;
		case 28:
			SetWeaponUseRequirement(effectList, context);
			return;
		case 29:
			SetWeaponAttackRange(effectList, context);
			return;
		case 30:
			SetArmorMaxPower(effectList, context);
			return;
		case 31:
			SetArmorUseRequirement(effectList, context);
			return;
		case 315:
			SetEquipmentPower(effectList, context);
			return;
		case 279:
			SetIgnoreEquipmentOverload(effectList, context);
			return;
		case 310:
			SetEquipmentBonus(effectList, context);
			return;
		case 32:
			SetHitStrength(effectList, context);
			return;
		case 33:
			SetHitTechnique(effectList, context);
			return;
		case 34:
			SetHitSpeed(effectList, context);
			return;
		case 35:
			SetHitMind(effectList, context);
			return;
		case 277:
			SetHitAddByTempValue(effectList, context);
			return;
		case 36:
			SetHitCanChange(effectList, context);
			return;
		case 37:
			SetHitChangeEffectPercent(effectList, context);
			return;
		case 38:
			SetAvoidStrength(effectList, context);
			return;
		case 39:
			SetAvoidTechnique(effectList, context);
			return;
		case 40:
			SetAvoidSpeed(effectList, context);
			return;
		case 41:
			SetAvoidMind(effectList, context);
			return;
		case 278:
			SetAvoidAddByTempValue(effectList, context);
			return;
		case 42:
			SetAvoidCanChange(effectList, context);
			return;
		case 43:
			SetAvoidChangeEffectPercent(effectList, context);
			return;
		case 44:
			SetPenetrateOuter(effectList, context);
			return;
		case 45:
			SetPenetrateInner(effectList, context);
			return;
		case 46:
			SetPenetrateResistOuter(effectList, context);
			return;
		case 47:
			SetPenetrateResistInner(effectList, context);
			return;
		case 297:
			SetConsummateLevelBonus(effectList, context);
			return;
		case 236:
			SetConsummateLevelRelatedMainAttributesHitValues(effectList, context);
			return;
		case 237:
			SetConsummateLevelRelatedMainAttributesAvoidValues(effectList, context);
			return;
		case 238:
			SetConsummateLevelRelatedMainAttributesPenetrations(effectList, context);
			return;
		case 239:
			SetConsummateLevelRelatedMainAttributesPenetrationResists(effectList, context);
			return;
		case 48:
			SetNeiliAllocationAttack(effectList, context);
			return;
		case 49:
			SetNeiliAllocationAgile(effectList, context);
			return;
		case 50:
			SetNeiliAllocationDefense(effectList, context);
			return;
		case 51:
			SetNeiliAllocationAssist(effectList, context);
			return;
		case 52:
			SetHappiness(effectList, context);
			return;
		case 53:
			SetMaxHealth(effectList, context);
			return;
		case 54:
			SetHealthCost(effectList, context);
			return;
		case 55:
			SetMoveSpeedCanChange(effectList, context);
			return;
		case 267:
			SetGoneMadInAllBreak(effectList, context);
			return;
		case 262:
			SetXiangshuInfectionDelta(effectList, context);
			return;
		case 263:
			SetHealthDelta(effectList, context);
			return;
		case 270:
			SetHappinessDelta(effectList, context);
			return;
		case 266:
			SetCurrAgeDelta(effectList, context);
			return;
		case 298:
			SetNeiliDelta(effectList, context);
			return;
		case 268:
			SetMakeLoveRateOnMonthChange(effectList, context);
			return;
		case 296:
			SetTakeRevengeRateOnMonthChange(effectList, context);
			return;
		case 299:
			SetCanMakeLoveSpecialOnMonthChange(effectList, context);
			return;
		case 260:
			SetCanReadingOnMonthChange(effectList, context);
			return;
		case 269:
			SetCanAutoHealOnMonthChange(effectList, context);
			return;
		case 295:
			SetCanRecoverHealthOnMonthChange(effectList, context);
			return;
		case 293:
			SetFeatureBonusReverse(effectList, context);
			return;
		case 56:
			SetAttackerHitStrength(effectList, context);
			return;
		case 57:
			SetAttackerHitTechnique(effectList, context);
			return;
		case 58:
			SetAttackerHitSpeed(effectList, context);
			return;
		case 59:
			SetAttackerHitMind(effectList, context);
			return;
		case 60:
			SetAttackerAvoidStrength(effectList, context);
			return;
		case 61:
			SetAttackerAvoidTechnique(effectList, context);
			return;
		case 62:
			SetAttackerAvoidSpeed(effectList, context);
			return;
		case 63:
			SetAttackerAvoidMind(effectList, context);
			return;
		case 64:
			SetAttackerPenetrateOuter(effectList, context);
			return;
		case 65:
			SetAttackerPenetrateInner(effectList, context);
			return;
		case 66:
			SetAttackerPenetrateResistOuter(effectList, context);
			return;
		case 67:
			SetAttackerPenetrateResistInner(effectList, context);
			return;
		case 68:
			SetAttackHitType(effectList, context);
			return;
		case 69:
			SetMakeDirectDamage(effectList, context);
			return;
		case 275:
			SetMakeMindDamage(effectList, context);
			return;
		case 70:
			SetMakeBounceDamage(effectList, context);
			return;
		case 71:
			SetMakeFightBackDamage(effectList, context);
			return;
		case 72:
			SetMakePoisonLevel(effectList, context);
			return;
		case 73:
			SetMakePoisonValue(effectList, context);
			return;
		case 233:
			SetMakePoisonResist(effectList, context);
			return;
		case 246:
			SetMakePoisonTarget(effectList, context);
			return;
		case 74:
			SetAttackerHitOdds(effectList, context);
			return;
		case 75:
			SetAttackerFightBackHitOdds(effectList, context);
			return;
		case 76:
			SetAttackerPursueOdds(effectList, context);
			return;
		case 77:
			SetMakedInjuryChangeToOld(effectList, context);
			return;
		case 78:
			SetMakedPoisonChangeToOld(effectList, context);
			return;
		case 288:
			SetAllMarkChangeToMind(effectList, context);
			return;
		case 289:
			SetMindMarkChangeToFatal(effectList, context);
			return;
		case 305:
			SetInfinityMindMarkProgress(effectList, context);
			return;
		case 327:
			SetMakeDamageCanReduce(effectList, context);
			return;
		case 281:
			SetIgnoreArmor(effectList, context);
			return;
		case 79:
			SetMakeDamageType(effectList, context);
			return;
		case 80:
			SetCanMakeInjuryToNoInjuryPart(effectList, context);
			return;
		case 81:
			SetMakePoisonType(effectList, context);
			return;
		case 82:
			SetNormalAttackWeapon(effectList, context);
			return;
		case 83:
			SetNormalAttackTrick(effectList, context);
			return;
		case 328:
			SetNormalAttackGetTrickCount(effectList, context);
			return;
		case 283:
			SetNormalAttackPrepareFrame(effectList, context);
			return;
		case 321:
			SetNormalAttackRecoveryFrame(effectList, context);
			return;
		case 317:
			SetUnlockSpeed(effectList, context);
			return;
		case 307:
			SetNormalAttackChangeToUnlockAttack(effectList, context);
			return;
		case 312:
			SetRawCreateEffectList(effectList, context);
			return;
		case 84:
			SetExtraFlawCount(effectList, context);
			return;
		case 318:
			SetFlawBonusFactor(effectList, context);
			return;
		case 85:
			SetAttackCanBounce(effectList, context);
			return;
		case 86:
			SetAttackCanFightBack(effectList, context);
			return;
		case 252:
			SetAttackCanPursue(effectList, context);
			return;
		case 87:
			SetMakeFightBackInjuryMark(effectList, context);
			return;
		case 88:
			SetLegSkillUseShoes(effectList, context);
			return;
		case 323:
			SetAttackerDirectFinalDamageValue(effectList, context);
			return;
		case 89:
			SetAttackerFinalDamageValue(effectList, context);
			return;
		case 90:
			SetDefenderHitStrength(effectList, context);
			return;
		case 91:
			SetDefenderHitTechnique(effectList, context);
			return;
		case 92:
			SetDefenderHitSpeed(effectList, context);
			return;
		case 93:
			SetDefenderHitMind(effectList, context);
			return;
		case 94:
			SetDefenderAvoidStrength(effectList, context);
			return;
		case 95:
			SetDefenderAvoidTechnique(effectList, context);
			return;
		case 96:
			SetDefenderAvoidSpeed(effectList, context);
			return;
		case 97:
			SetDefenderAvoidMind(effectList, context);
			return;
		case 98:
			SetDefenderPenetrateOuter(effectList, context);
			return;
		case 99:
			SetDefenderPenetrateInner(effectList, context);
			return;
		case 100:
			SetDefenderPenetrateResistOuter(effectList, context);
			return;
		case 101:
			SetDefenderPenetrateResistInner(effectList, context);
			return;
		case 102:
			SetAcceptDirectDamage(effectList, context);
			return;
		case 276:
			SetAcceptMindDamage(effectList, context);
			return;
		case 103:
			SetAcceptBounceDamage(effectList, context);
			return;
		case 104:
			SetAcceptFightBackDamage(effectList, context);
			return;
		case 105:
			SetAcceptPoisonLevel(effectList, context);
			return;
		case 106:
			SetAcceptPoisonValue(effectList, context);
			return;
		case 232:
			SetAcceptPoisonResist(effectList, context);
			return;
		case 247:
			SetAcceptPoisonTarget(effectList, context);
			return;
		case 107:
			SetDefenderHitOdds(effectList, context);
			return;
		case 108:
			SetDefenderFightBackHitOdds(effectList, context);
			return;
		case 109:
			SetDefenderPursueOdds(effectList, context);
			return;
		case 326:
			SetAcceptDamageCanAdd(effectList, context);
			return;
		case 110:
			SetAcceptMaxInjuryCount(effectList, context);
			return;
		case 111:
			SetBouncePower(effectList, context);
			return;
		case 112:
			SetFightBackPower(effectList, context);
			return;
		case 113:
			SetDirectDamageInnerRatio(effectList, context);
			return;
		case 320:
			SetDefenderDirectFinalDamageValue(effectList, context);
			return;
		case 114:
			SetDefenderFinalDamageValue(effectList, context);
			return;
		case 282:
			SetUnyieldingFallen(effectList, context);
			return;
		case 242:
			SetOuterInjuryImmunity(effectList, context);
			return;
		case 241:
			SetInnerInjuryImmunity(effectList, context);
			return;
		case 115:
			SetDirectDamageValue(effectList, context);
			return;
		case 116:
			SetDirectInjuryMark(effectList, context);
			return;
		case 117:
			SetGoneMadInjury(effectList, context);
			return;
		case 322:
			SetFinalGoneMadInjury(effectList, context);
			return;
		case 118:
			SetHealInjurySpeed(effectList, context);
			return;
		case 119:
			SetHealInjuryBuff(effectList, context);
			return;
		case 120:
			SetHealInjuryDebuff(effectList, context);
			return;
		case 121:
			SetHealPoisonSpeed(effectList, context);
			return;
		case 122:
			SetHealPoisonBuff(effectList, context);
			return;
		case 123:
			SetHealPoisonDebuff(effectList, context);
			return;
		case 261:
			SetMedicineEffect(effectList, context);
			return;
		case 316:
			SetHealFlawSpeed(effectList, context);
			return;
		case 300:
			SetHealAcupointSpeed(effectList, context);
			return;
		case 124:
			SetFleeSpeed(effectList, context);
			return;
		case 126:
			SetCanAddFlaw(effectList, context);
			return;
		case 127:
			SetFlawLevel(effectList, context);
			return;
		case 128:
			SetFlawLevelCanReduce(effectList, context);
			return;
		case 129:
			SetFlawCount(effectList, context);
			return;
		case 125:
			SetMaxFlawCount(effectList, context);
			return;
		case 130:
			SetMaxAcupointCount(effectList, context);
			return;
		case 131:
			SetCanAddAcupoint(effectList, context);
			return;
		case 132:
			SetAcupointLevel(effectList, context);
			return;
		case 133:
			SetAcupointLevelCanReduce(effectList, context);
			return;
		case 134:
			SetAcupointCount(effectList, context);
			return;
		case 135:
			SetAddNeiliAllocation(effectList, context);
			return;
		case 136:
			SetCostNeiliAllocation(effectList, context);
			return;
		case 137:
			SetCanChangeNeiliAllocation(effectList, context);
			return;
		case 138:
			SetCanGetTrick(effectList, context);
			return;
		case 139:
			SetGetTrickType(effectList, context);
			return;
		case 140:
			SetAttackBodyPart(effectList, context);
			return;
		case 308:
			SetAttackBodyPartOdds(effectList, context);
			return;
		case 141:
			SetWeaponEquipAttack(effectList, context);
			return;
		case 142:
			SetWeaponEquipDefense(effectList, context);
			return;
		case 143:
			SetArmorEquipAttack(effectList, context);
			return;
		case 144:
			SetArmorEquipDefense(effectList, context);
			return;
		case 311:
			SetEquipmentWeight(effectList, context);
			return;
		case 145:
			SetAttackRangeForward(effectList, context);
			return;
		case 146:
			SetAttackRangeBackward(effectList, context);
			return;
		case 273:
			SetAttackRangeMaxAcupoint(effectList, context);
			return;
		case 148:
			SetCanForcedMove(effectList, context);
			return;
		case 149:
			SetMobilityCanBeRemoved(effectList, context);
			return;
		case 150:
			SetMobilityCostByEffect(effectList, context);
			return;
		case 255:
			SetStanceCostByEffect(effectList, context);
			return;
		case 256:
			SetBreathCostByEffect(effectList, context);
			return;
		case 147:
			SetMoveCanBeStopped(effectList, context);
			return;
		case 151:
			SetMoveDistance(effectList, context);
			return;
		case 244:
			SetLockDistance(effectList, context);
			return;
		case 152:
			SetJumpPrepareFrame(effectList, context);
			return;
		case 153:
			SetBounceInjuryMark(effectList, context);
			return;
		case 154:
			SetSkillHasCost(effectList, context);
			return;
		case 155:
			SetCombatStateEffect(effectList, context);
			return;
		case 156:
			SetChangeNeedUseSkill(effectList, context);
			return;
		case 157:
			SetChangeDistanceIsMove(effectList, context);
			return;
		case 158:
			SetReplaceCharHit(effectList, context);
			return;
		case 159:
			SetCanAddPoison(effectList, context);
			return;
		case 160:
			SetCanReducePoison(effectList, context);
			return;
		case 161:
			SetReducePoisonValue(effectList, context);
			return;
		case 162:
			SetPoisonCanAffect(effectList, context);
			return;
		case 163:
			SetPoisonAffectCount(effectList, context);
			return;
		case 243:
			SetPoisonAffectThreshold(effectList, context);
			return;
		case 259:
			SetPoisonAffectProduceValue(effectList, context);
			return;
		case 272:
			SetMixPoisonInfinityAffect(effectList, context);
			return;
		case 164:
			SetCostTricks(effectList, context);
			return;
		case 313:
			SetJiTrickAsWeaponTrickCount(effectList, context);
			return;
		case 314:
			SetUselessTrickAsJiTrickCount(effectList, context);
			return;
		case 309:
			SetChangeDurability(effectList, context);
			return;
		case 165:
			SetJumpMoveDistance(effectList, context);
			return;
		case 166:
			SetCombatStateToAdd(effectList, context);
			return;
		case 167:
			SetCombatStatePower(effectList, context);
			return;
		case 168:
			SetBreakBodyPartInjuryCount(effectList, context);
			return;
		case 169:
			SetBodyPartIsBroken(effectList, context);
			return;
		case 170:
			SetMaxTrickCount(effectList, context);
			return;
		case 171:
			SetMaxBreathPercent(effectList, context);
			return;
		case 172:
			SetMaxStancePercent(effectList, context);
			return;
		case 274:
			SetMaxMobilityPercent(effectList, context);
			return;
		case 173:
			SetExtraBreathPercent(effectList, context);
			return;
		case 174:
			SetExtraStancePercent(effectList, context);
			return;
		case 175:
			SetMoveCostMobility(effectList, context);
			return;
		case 176:
			SetDefendSkillKeepTime(effectList, context);
			return;
		case 177:
			SetBounceRange(effectList, context);
			return;
		case 178:
			SetMindMarkKeepTime(effectList, context);
			return;
		case 249:
			SetMindMarkCount(effectList, context);
			return;
		case 179:
			SetSkillMobilityCostPerFrame(effectList, context);
			return;
		case 180:
			SetCanAddWug(effectList, context);
			return;
		case 181:
			SetHasGodWeaponBuff(effectList, context);
			return;
		case 182:
			SetHasGodArmorBuff(effectList, context);
			return;
		case 183:
			SetTeammateCmdRequireGenerateValue(effectList, context);
			return;
		case 184:
			SetTeammateCmdEffect(effectList, context);
			return;
		case 271:
			SetTeammateCmdCanUse(effectList, context);
			return;
		case 185:
			SetFlawRecoverSpeed(effectList, context);
			return;
		case 186:
			SetAcupointRecoverSpeed(effectList, context);
			return;
		case 187:
			SetMindMarkRecoverSpeed(effectList, context);
			return;
		case 188:
			SetInjuryAutoHealSpeed(effectList, context);
			return;
		case 189:
			SetCanRecoverBreath(effectList, context);
			return;
		case 190:
			SetCanRecoverStance(effectList, context);
			return;
		case 191:
			SetFatalDamageValue(effectList, context);
			return;
		case 294:
			SetWugFatalDamageValue(effectList, context);
			return;
		case 192:
			SetFatalDamageMarkCount(effectList, context);
			return;
		case 304:
			SetFinalFatalDamageMarkCount(effectList, context);
			return;
		case 193:
			SetCanFightBackDuringPrepareSkill(effectList, context);
			return;
		case 250:
			SetCanFightBackWithHit(effectList, context);
			return;
		case 194:
			SetSkillPrepareSpeed(effectList, context);
			return;
		case 195:
			SetBreathRecoverSpeed(effectList, context);
			return;
		case 196:
			SetStanceRecoverSpeed(effectList, context);
			return;
		case 197:
			SetMobilityRecoverSpeed(effectList, context);
			return;
		case 301:
			SetMaxChangeTrickCount(effectList, context);
			return;
		case 198:
			SetChangeTrickProgressAddValue(effectList, context);
			return;
		case 287:
			SetAgileSkillCanAffect(effectList, context);
			return;
		case 285:
			SetDefendSkillCanAffect(effectList, context);
			return;
		case 286:
			SetAssistSkillCanAffect(effectList, context);
			return;
		case 199:
			SetPower(effectList, context);
			return;
		case 200:
			SetMaxPower(effectList, context);
			return;
		case 201:
			SetPowerCanReduce(effectList, context);
			return;
		case 257:
			SetPowerAddRatio(effectList, context);
			return;
		case 258:
			SetPowerReduceRatio(effectList, context);
			return;
		case 292:
			SetPowerEffectReverse(effectList, context);
			return;
		case 202:
			SetUseRequirement(effectList, context);
			return;
		case 203:
			SetCurrInnerRatio(effectList, context);
			return;
		case 204:
			SetCostBreathAndStance(effectList, context);
			return;
		case 205:
			SetCostBreath(effectList, context);
			return;
		case 206:
			SetCostStance(effectList, context);
			return;
		case 207:
			SetCostMobility(effectList, context);
			return;
		case 208:
			SetSkillCostTricks(effectList, context);
			return;
		case 280:
			SetCanCostEnemyUsableTricks(effectList, context);
			return;
		case 284:
			SetCanCostUselessTricks(effectList, context);
			return;
		case 319:
			SetCanCostShaTricks(effectList, context);
			return;
		case 209:
			SetEffectDirection(effectList, context);
			return;
		case 210:
			SetEffectDirectionCanChange(effectList, context);
			return;
		case 211:
			SetGridCost(effectList, context);
			return;
		case 212:
			SetPrepareTotalProgress(effectList, context);
			return;
		case 213:
			SetSpecificGridCount(effectList, context);
			return;
		case 214:
			SetGenericGridCount(effectList, context);
			return;
		case 234:
			SetCanCriticalHit(effectList, context);
			return;
		case 254:
			SetCriticalOdds(effectList, context);
			return;
		case 248:
			SetCertainCriticalHit(effectList, context);
			return;
		case 251:
			SetInevitableHit(effectList, context);
			return;
		case 291:
			SetInevitableAvoid(effectList, context);
			return;
		case 215:
			SetCanInterrupt(effectList, context);
			return;
		case 216:
			SetInterruptOdds(effectList, context);
			return;
		case 217:
			SetCanSilence(effectList, context);
			return;
		case 218:
			SetSilenceOdds(effectList, context);
			return;
		case 265:
			SetSilenceFrame(effectList, context);
			return;
		case 264:
			SetWeaponSilenceFrame(effectList, context);
			return;
		case 219:
			SetCanCastWithBrokenBodyPart(effectList, context);
			return;
		case 220:
			SetAddPowerCanBeRemoved(effectList, context);
			return;
		case 221:
			SetSkillType(effectList, context);
			return;
		case 222:
			SetEffectCountCanChange(effectList, context);
			return;
		case 290:
			SetCanCast(effectList, context);
			return;
		case 223:
			SetCanCastInDefend(effectList, context);
			return;
		case 224:
			SetHitDistribution(effectList, context);
			return;
		case 225:
			SetCanCastOnLackBreath(effectList, context);
			return;
		case 226:
			SetCanCastOnLackStance(effectList, context);
			return;
		case 302:
			SetConvertCostBreathAndStance(effectList, context);
			return;
		case 227:
			SetCostBreathOnCast(effectList, context);
			return;
		case 228:
			SetCostStanceOnCast(effectList, context);
			return;
		case 229:
			SetCanUseMobilityAsBreath(effectList, context);
			return;
		case 230:
			SetCanUseMobilityAsStance(effectList, context);
			return;
		case 231:
			SetCastCostNeiliAllocation(effectList, context);
			return;
		case 235:
			SetCanCostNeiliAllocationEffect(effectList, context);
			return;
		case 324:
			SetCanCostTrickDuringPreparingSkill(effectList, context);
			return;
		case 325:
			SetValidItemList(effectList, context);
			return;
		case 253:
			SetCombatSkillDataEffectList(effectList, context);
			return;
		case 306:
			SetCombatSkillAiScorePower(effectList, context);
			return;
		}
		throw new Exception($"Effect list of fieldId {fieldId} not found");
	}

	public int GetId()
	{
		return _id;
	}

	public unsafe void SetId(int id, DataContext context)
	{
		_id = id;
		SetModifiedAndInvalidateInfluencedCache(0, context);
		if (CollectionHelperData.IsArchive)
		{
			byte* ptr = OperationAdder.DynamicObjectCollection_SetFixedField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 0u, 4);
			*(int*)ptr = _id;
			ptr += 4;
		}
	}

	public SpecialEffectList GetMaxStrength()
	{
		return _maxStrength;
	}

	public unsafe void SetMaxStrength(SpecialEffectList maxStrength, DataContext context)
	{
		_maxStrength = maxStrength;
		SetModifiedAndInvalidateInfluencedCache(1, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _maxStrength.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 0, serializedSize);
			ptr += _maxStrength.Serialize(ptr);
		}
	}

	public SpecialEffectList GetMaxDexterity()
	{
		return _maxDexterity;
	}

	public unsafe void SetMaxDexterity(SpecialEffectList maxDexterity, DataContext context)
	{
		_maxDexterity = maxDexterity;
		SetModifiedAndInvalidateInfluencedCache(2, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _maxDexterity.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 1, serializedSize);
			ptr += _maxDexterity.Serialize(ptr);
		}
	}

	public SpecialEffectList GetMaxConcentration()
	{
		return _maxConcentration;
	}

	public unsafe void SetMaxConcentration(SpecialEffectList maxConcentration, DataContext context)
	{
		_maxConcentration = maxConcentration;
		SetModifiedAndInvalidateInfluencedCache(3, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _maxConcentration.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 2, serializedSize);
			ptr += _maxConcentration.Serialize(ptr);
		}
	}

	public SpecialEffectList GetMaxVitality()
	{
		return _maxVitality;
	}

	public unsafe void SetMaxVitality(SpecialEffectList maxVitality, DataContext context)
	{
		_maxVitality = maxVitality;
		SetModifiedAndInvalidateInfluencedCache(4, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _maxVitality.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 3, serializedSize);
			ptr += _maxVitality.Serialize(ptr);
		}
	}

	public SpecialEffectList GetMaxEnergy()
	{
		return _maxEnergy;
	}

	public unsafe void SetMaxEnergy(SpecialEffectList maxEnergy, DataContext context)
	{
		_maxEnergy = maxEnergy;
		SetModifiedAndInvalidateInfluencedCache(5, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _maxEnergy.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 4, serializedSize);
			ptr += _maxEnergy.Serialize(ptr);
		}
	}

	public SpecialEffectList GetMaxIntelligence()
	{
		return _maxIntelligence;
	}

	public unsafe void SetMaxIntelligence(SpecialEffectList maxIntelligence, DataContext context)
	{
		_maxIntelligence = maxIntelligence;
		SetModifiedAndInvalidateInfluencedCache(6, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _maxIntelligence.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 5, serializedSize);
			ptr += _maxIntelligence.Serialize(ptr);
		}
	}

	public SpecialEffectList GetRecoveryOfStance()
	{
		return _recoveryOfStance;
	}

	public unsafe void SetRecoveryOfStance(SpecialEffectList recoveryOfStance, DataContext context)
	{
		_recoveryOfStance = recoveryOfStance;
		SetModifiedAndInvalidateInfluencedCache(7, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _recoveryOfStance.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 6, serializedSize);
			ptr += _recoveryOfStance.Serialize(ptr);
		}
	}

	public SpecialEffectList GetRecoveryOfBreath()
	{
		return _recoveryOfBreath;
	}

	public unsafe void SetRecoveryOfBreath(SpecialEffectList recoveryOfBreath, DataContext context)
	{
		_recoveryOfBreath = recoveryOfBreath;
		SetModifiedAndInvalidateInfluencedCache(8, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _recoveryOfBreath.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 7, serializedSize);
			ptr += _recoveryOfBreath.Serialize(ptr);
		}
	}

	public SpecialEffectList GetMoveSpeed()
	{
		return _moveSpeed;
	}

	public unsafe void SetMoveSpeed(SpecialEffectList moveSpeed, DataContext context)
	{
		_moveSpeed = moveSpeed;
		SetModifiedAndInvalidateInfluencedCache(9, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _moveSpeed.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 8, serializedSize);
			ptr += _moveSpeed.Serialize(ptr);
		}
	}

	public SpecialEffectList GetRecoveryOfFlaw()
	{
		return _recoveryOfFlaw;
	}

	public unsafe void SetRecoveryOfFlaw(SpecialEffectList recoveryOfFlaw, DataContext context)
	{
		_recoveryOfFlaw = recoveryOfFlaw;
		SetModifiedAndInvalidateInfluencedCache(10, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _recoveryOfFlaw.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 9, serializedSize);
			ptr += _recoveryOfFlaw.Serialize(ptr);
		}
	}

	public SpecialEffectList GetCastSpeed()
	{
		return _castSpeed;
	}

	public unsafe void SetCastSpeed(SpecialEffectList castSpeed, DataContext context)
	{
		_castSpeed = castSpeed;
		SetModifiedAndInvalidateInfluencedCache(11, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _castSpeed.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 10, serializedSize);
			ptr += _castSpeed.Serialize(ptr);
		}
	}

	public SpecialEffectList GetRecoveryOfBlockedAcupoint()
	{
		return _recoveryOfBlockedAcupoint;
	}

	public unsafe void SetRecoveryOfBlockedAcupoint(SpecialEffectList recoveryOfBlockedAcupoint, DataContext context)
	{
		_recoveryOfBlockedAcupoint = recoveryOfBlockedAcupoint;
		SetModifiedAndInvalidateInfluencedCache(12, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _recoveryOfBlockedAcupoint.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 11, serializedSize);
			ptr += _recoveryOfBlockedAcupoint.Serialize(ptr);
		}
	}

	public SpecialEffectList GetWeaponSwitchSpeed()
	{
		return _weaponSwitchSpeed;
	}

	public unsafe void SetWeaponSwitchSpeed(SpecialEffectList weaponSwitchSpeed, DataContext context)
	{
		_weaponSwitchSpeed = weaponSwitchSpeed;
		SetModifiedAndInvalidateInfluencedCache(13, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _weaponSwitchSpeed.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 12, serializedSize);
			ptr += _weaponSwitchSpeed.Serialize(ptr);
		}
	}

	public SpecialEffectList GetAttackSpeed()
	{
		return _attackSpeed;
	}

	public unsafe void SetAttackSpeed(SpecialEffectList attackSpeed, DataContext context)
	{
		_attackSpeed = attackSpeed;
		SetModifiedAndInvalidateInfluencedCache(14, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _attackSpeed.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 13, serializedSize);
			ptr += _attackSpeed.Serialize(ptr);
		}
	}

	public SpecialEffectList GetInnerRatio()
	{
		return _innerRatio;
	}

	public unsafe void SetInnerRatio(SpecialEffectList innerRatio, DataContext context)
	{
		_innerRatio = innerRatio;
		SetModifiedAndInvalidateInfluencedCache(15, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _innerRatio.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 14, serializedSize);
			ptr += _innerRatio.Serialize(ptr);
		}
	}

	public SpecialEffectList GetRecoveryOfQiDisorder()
	{
		return _recoveryOfQiDisorder;
	}

	public unsafe void SetRecoveryOfQiDisorder(SpecialEffectList recoveryOfQiDisorder, DataContext context)
	{
		_recoveryOfQiDisorder = recoveryOfQiDisorder;
		SetModifiedAndInvalidateInfluencedCache(16, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _recoveryOfQiDisorder.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 15, serializedSize);
			ptr += _recoveryOfQiDisorder.Serialize(ptr);
		}
	}

	public SpecialEffectList GetMinorAttributeFixMaxValue()
	{
		return _minorAttributeFixMaxValue;
	}

	public unsafe void SetMinorAttributeFixMaxValue(SpecialEffectList minorAttributeFixMaxValue, DataContext context)
	{
		_minorAttributeFixMaxValue = minorAttributeFixMaxValue;
		SetModifiedAndInvalidateInfluencedCache(17, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _minorAttributeFixMaxValue.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 16, serializedSize);
			ptr += _minorAttributeFixMaxValue.Serialize(ptr);
		}
	}

	public SpecialEffectList GetMinorAttributeFixMinValue()
	{
		return _minorAttributeFixMinValue;
	}

	public unsafe void SetMinorAttributeFixMinValue(SpecialEffectList minorAttributeFixMinValue, DataContext context)
	{
		_minorAttributeFixMinValue = minorAttributeFixMinValue;
		SetModifiedAndInvalidateInfluencedCache(18, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _minorAttributeFixMinValue.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 17, serializedSize);
			ptr += _minorAttributeFixMinValue.Serialize(ptr);
		}
	}

	public SpecialEffectList GetResistOfHotPoison()
	{
		return _resistOfHotPoison;
	}

	public unsafe void SetResistOfHotPoison(SpecialEffectList resistOfHotPoison, DataContext context)
	{
		_resistOfHotPoison = resistOfHotPoison;
		SetModifiedAndInvalidateInfluencedCache(19, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _resistOfHotPoison.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 18, serializedSize);
			ptr += _resistOfHotPoison.Serialize(ptr);
		}
	}

	public SpecialEffectList GetResistOfGloomyPoison()
	{
		return _resistOfGloomyPoison;
	}

	public unsafe void SetResistOfGloomyPoison(SpecialEffectList resistOfGloomyPoison, DataContext context)
	{
		_resistOfGloomyPoison = resistOfGloomyPoison;
		SetModifiedAndInvalidateInfluencedCache(20, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _resistOfGloomyPoison.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 19, serializedSize);
			ptr += _resistOfGloomyPoison.Serialize(ptr);
		}
	}

	public SpecialEffectList GetResistOfColdPoison()
	{
		return _resistOfColdPoison;
	}

	public unsafe void SetResistOfColdPoison(SpecialEffectList resistOfColdPoison, DataContext context)
	{
		_resistOfColdPoison = resistOfColdPoison;
		SetModifiedAndInvalidateInfluencedCache(21, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _resistOfColdPoison.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 20, serializedSize);
			ptr += _resistOfColdPoison.Serialize(ptr);
		}
	}

	public SpecialEffectList GetResistOfRedPoison()
	{
		return _resistOfRedPoison;
	}

	public unsafe void SetResistOfRedPoison(SpecialEffectList resistOfRedPoison, DataContext context)
	{
		_resistOfRedPoison = resistOfRedPoison;
		SetModifiedAndInvalidateInfluencedCache(22, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _resistOfRedPoison.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 21, serializedSize);
			ptr += _resistOfRedPoison.Serialize(ptr);
		}
	}

	public SpecialEffectList GetResistOfRottenPoison()
	{
		return _resistOfRottenPoison;
	}

	public unsafe void SetResistOfRottenPoison(SpecialEffectList resistOfRottenPoison, DataContext context)
	{
		_resistOfRottenPoison = resistOfRottenPoison;
		SetModifiedAndInvalidateInfluencedCache(23, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _resistOfRottenPoison.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 22, serializedSize);
			ptr += _resistOfRottenPoison.Serialize(ptr);
		}
	}

	public SpecialEffectList GetResistOfIllusoryPoison()
	{
		return _resistOfIllusoryPoison;
	}

	public unsafe void SetResistOfIllusoryPoison(SpecialEffectList resistOfIllusoryPoison, DataContext context)
	{
		_resistOfIllusoryPoison = resistOfIllusoryPoison;
		SetModifiedAndInvalidateInfluencedCache(24, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _resistOfIllusoryPoison.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 23, serializedSize);
			ptr += _resistOfIllusoryPoison.Serialize(ptr);
		}
	}

	public SpecialEffectList GetDisplayAge()
	{
		return _displayAge;
	}

	public unsafe void SetDisplayAge(SpecialEffectList displayAge, DataContext context)
	{
		_displayAge = displayAge;
		SetModifiedAndInvalidateInfluencedCache(25, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _displayAge.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 24, serializedSize);
			ptr += _displayAge.Serialize(ptr);
		}
	}

	public SpecialEffectList GetNeiliProportionOfFiveElements()
	{
		return _neiliProportionOfFiveElements;
	}

	public unsafe void SetNeiliProportionOfFiveElements(SpecialEffectList neiliProportionOfFiveElements, DataContext context)
	{
		_neiliProportionOfFiveElements = neiliProportionOfFiveElements;
		SetModifiedAndInvalidateInfluencedCache(26, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _neiliProportionOfFiveElements.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 25, serializedSize);
			ptr += _neiliProportionOfFiveElements.Serialize(ptr);
		}
	}

	public SpecialEffectList GetWeaponMaxPower()
	{
		return _weaponMaxPower;
	}

	public unsafe void SetWeaponMaxPower(SpecialEffectList weaponMaxPower, DataContext context)
	{
		_weaponMaxPower = weaponMaxPower;
		SetModifiedAndInvalidateInfluencedCache(27, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _weaponMaxPower.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 26, serializedSize);
			ptr += _weaponMaxPower.Serialize(ptr);
		}
	}

	public SpecialEffectList GetWeaponUseRequirement()
	{
		return _weaponUseRequirement;
	}

	public unsafe void SetWeaponUseRequirement(SpecialEffectList weaponUseRequirement, DataContext context)
	{
		_weaponUseRequirement = weaponUseRequirement;
		SetModifiedAndInvalidateInfluencedCache(28, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _weaponUseRequirement.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 27, serializedSize);
			ptr += _weaponUseRequirement.Serialize(ptr);
		}
	}

	public SpecialEffectList GetWeaponAttackRange()
	{
		return _weaponAttackRange;
	}

	public unsafe void SetWeaponAttackRange(SpecialEffectList weaponAttackRange, DataContext context)
	{
		_weaponAttackRange = weaponAttackRange;
		SetModifiedAndInvalidateInfluencedCache(29, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _weaponAttackRange.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 28, serializedSize);
			ptr += _weaponAttackRange.Serialize(ptr);
		}
	}

	public SpecialEffectList GetArmorMaxPower()
	{
		return _armorMaxPower;
	}

	public unsafe void SetArmorMaxPower(SpecialEffectList armorMaxPower, DataContext context)
	{
		_armorMaxPower = armorMaxPower;
		SetModifiedAndInvalidateInfluencedCache(30, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _armorMaxPower.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 29, serializedSize);
			ptr += _armorMaxPower.Serialize(ptr);
		}
	}

	public SpecialEffectList GetArmorUseRequirement()
	{
		return _armorUseRequirement;
	}

	public unsafe void SetArmorUseRequirement(SpecialEffectList armorUseRequirement, DataContext context)
	{
		_armorUseRequirement = armorUseRequirement;
		SetModifiedAndInvalidateInfluencedCache(31, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _armorUseRequirement.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 30, serializedSize);
			ptr += _armorUseRequirement.Serialize(ptr);
		}
	}

	public SpecialEffectList GetHitStrength()
	{
		return _hitStrength;
	}

	public unsafe void SetHitStrength(SpecialEffectList hitStrength, DataContext context)
	{
		_hitStrength = hitStrength;
		SetModifiedAndInvalidateInfluencedCache(32, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _hitStrength.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 31, serializedSize);
			ptr += _hitStrength.Serialize(ptr);
		}
	}

	public SpecialEffectList GetHitTechnique()
	{
		return _hitTechnique;
	}

	public unsafe void SetHitTechnique(SpecialEffectList hitTechnique, DataContext context)
	{
		_hitTechnique = hitTechnique;
		SetModifiedAndInvalidateInfluencedCache(33, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _hitTechnique.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 32, serializedSize);
			ptr += _hitTechnique.Serialize(ptr);
		}
	}

	public SpecialEffectList GetHitSpeed()
	{
		return _hitSpeed;
	}

	public unsafe void SetHitSpeed(SpecialEffectList hitSpeed, DataContext context)
	{
		_hitSpeed = hitSpeed;
		SetModifiedAndInvalidateInfluencedCache(34, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _hitSpeed.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 33, serializedSize);
			ptr += _hitSpeed.Serialize(ptr);
		}
	}

	public SpecialEffectList GetHitMind()
	{
		return _hitMind;
	}

	public unsafe void SetHitMind(SpecialEffectList hitMind, DataContext context)
	{
		_hitMind = hitMind;
		SetModifiedAndInvalidateInfluencedCache(35, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _hitMind.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 34, serializedSize);
			ptr += _hitMind.Serialize(ptr);
		}
	}

	public SpecialEffectList GetHitCanChange()
	{
		return _hitCanChange;
	}

	public unsafe void SetHitCanChange(SpecialEffectList hitCanChange, DataContext context)
	{
		_hitCanChange = hitCanChange;
		SetModifiedAndInvalidateInfluencedCache(36, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _hitCanChange.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 35, serializedSize);
			ptr += _hitCanChange.Serialize(ptr);
		}
	}

	public SpecialEffectList GetHitChangeEffectPercent()
	{
		return _hitChangeEffectPercent;
	}

	public unsafe void SetHitChangeEffectPercent(SpecialEffectList hitChangeEffectPercent, DataContext context)
	{
		_hitChangeEffectPercent = hitChangeEffectPercent;
		SetModifiedAndInvalidateInfluencedCache(37, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _hitChangeEffectPercent.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 36, serializedSize);
			ptr += _hitChangeEffectPercent.Serialize(ptr);
		}
	}

	public SpecialEffectList GetAvoidStrength()
	{
		return _avoidStrength;
	}

	public unsafe void SetAvoidStrength(SpecialEffectList avoidStrength, DataContext context)
	{
		_avoidStrength = avoidStrength;
		SetModifiedAndInvalidateInfluencedCache(38, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _avoidStrength.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 37, serializedSize);
			ptr += _avoidStrength.Serialize(ptr);
		}
	}

	public SpecialEffectList GetAvoidTechnique()
	{
		return _avoidTechnique;
	}

	public unsafe void SetAvoidTechnique(SpecialEffectList avoidTechnique, DataContext context)
	{
		_avoidTechnique = avoidTechnique;
		SetModifiedAndInvalidateInfluencedCache(39, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _avoidTechnique.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 38, serializedSize);
			ptr += _avoidTechnique.Serialize(ptr);
		}
	}

	public SpecialEffectList GetAvoidSpeed()
	{
		return _avoidSpeed;
	}

	public unsafe void SetAvoidSpeed(SpecialEffectList avoidSpeed, DataContext context)
	{
		_avoidSpeed = avoidSpeed;
		SetModifiedAndInvalidateInfluencedCache(40, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _avoidSpeed.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 39, serializedSize);
			ptr += _avoidSpeed.Serialize(ptr);
		}
	}

	public SpecialEffectList GetAvoidMind()
	{
		return _avoidMind;
	}

	public unsafe void SetAvoidMind(SpecialEffectList avoidMind, DataContext context)
	{
		_avoidMind = avoidMind;
		SetModifiedAndInvalidateInfluencedCache(41, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _avoidMind.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 40, serializedSize);
			ptr += _avoidMind.Serialize(ptr);
		}
	}

	public SpecialEffectList GetAvoidCanChange()
	{
		return _avoidCanChange;
	}

	public unsafe void SetAvoidCanChange(SpecialEffectList avoidCanChange, DataContext context)
	{
		_avoidCanChange = avoidCanChange;
		SetModifiedAndInvalidateInfluencedCache(42, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _avoidCanChange.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 41, serializedSize);
			ptr += _avoidCanChange.Serialize(ptr);
		}
	}

	public SpecialEffectList GetAvoidChangeEffectPercent()
	{
		return _avoidChangeEffectPercent;
	}

	public unsafe void SetAvoidChangeEffectPercent(SpecialEffectList avoidChangeEffectPercent, DataContext context)
	{
		_avoidChangeEffectPercent = avoidChangeEffectPercent;
		SetModifiedAndInvalidateInfluencedCache(43, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _avoidChangeEffectPercent.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 42, serializedSize);
			ptr += _avoidChangeEffectPercent.Serialize(ptr);
		}
	}

	public SpecialEffectList GetPenetrateOuter()
	{
		return _penetrateOuter;
	}

	public unsafe void SetPenetrateOuter(SpecialEffectList penetrateOuter, DataContext context)
	{
		_penetrateOuter = penetrateOuter;
		SetModifiedAndInvalidateInfluencedCache(44, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _penetrateOuter.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 43, serializedSize);
			ptr += _penetrateOuter.Serialize(ptr);
		}
	}

	public SpecialEffectList GetPenetrateInner()
	{
		return _penetrateInner;
	}

	public unsafe void SetPenetrateInner(SpecialEffectList penetrateInner, DataContext context)
	{
		_penetrateInner = penetrateInner;
		SetModifiedAndInvalidateInfluencedCache(45, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _penetrateInner.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 44, serializedSize);
			ptr += _penetrateInner.Serialize(ptr);
		}
	}

	public SpecialEffectList GetPenetrateResistOuter()
	{
		return _penetrateResistOuter;
	}

	public unsafe void SetPenetrateResistOuter(SpecialEffectList penetrateResistOuter, DataContext context)
	{
		_penetrateResistOuter = penetrateResistOuter;
		SetModifiedAndInvalidateInfluencedCache(46, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _penetrateResistOuter.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 45, serializedSize);
			ptr += _penetrateResistOuter.Serialize(ptr);
		}
	}

	public SpecialEffectList GetPenetrateResistInner()
	{
		return _penetrateResistInner;
	}

	public unsafe void SetPenetrateResistInner(SpecialEffectList penetrateResistInner, DataContext context)
	{
		_penetrateResistInner = penetrateResistInner;
		SetModifiedAndInvalidateInfluencedCache(47, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _penetrateResistInner.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 46, serializedSize);
			ptr += _penetrateResistInner.Serialize(ptr);
		}
	}

	public SpecialEffectList GetNeiliAllocationAttack()
	{
		return _neiliAllocationAttack;
	}

	public unsafe void SetNeiliAllocationAttack(SpecialEffectList neiliAllocationAttack, DataContext context)
	{
		_neiliAllocationAttack = neiliAllocationAttack;
		SetModifiedAndInvalidateInfluencedCache(48, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _neiliAllocationAttack.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 47, serializedSize);
			ptr += _neiliAllocationAttack.Serialize(ptr);
		}
	}

	public SpecialEffectList GetNeiliAllocationAgile()
	{
		return _neiliAllocationAgile;
	}

	public unsafe void SetNeiliAllocationAgile(SpecialEffectList neiliAllocationAgile, DataContext context)
	{
		_neiliAllocationAgile = neiliAllocationAgile;
		SetModifiedAndInvalidateInfluencedCache(49, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _neiliAllocationAgile.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 48, serializedSize);
			ptr += _neiliAllocationAgile.Serialize(ptr);
		}
	}

	public SpecialEffectList GetNeiliAllocationDefense()
	{
		return _neiliAllocationDefense;
	}

	public unsafe void SetNeiliAllocationDefense(SpecialEffectList neiliAllocationDefense, DataContext context)
	{
		_neiliAllocationDefense = neiliAllocationDefense;
		SetModifiedAndInvalidateInfluencedCache(50, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _neiliAllocationDefense.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 49, serializedSize);
			ptr += _neiliAllocationDefense.Serialize(ptr);
		}
	}

	public SpecialEffectList GetNeiliAllocationAssist()
	{
		return _neiliAllocationAssist;
	}

	public unsafe void SetNeiliAllocationAssist(SpecialEffectList neiliAllocationAssist, DataContext context)
	{
		_neiliAllocationAssist = neiliAllocationAssist;
		SetModifiedAndInvalidateInfluencedCache(51, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _neiliAllocationAssist.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 50, serializedSize);
			ptr += _neiliAllocationAssist.Serialize(ptr);
		}
	}

	public SpecialEffectList GetHappiness()
	{
		return _happiness;
	}

	public unsafe void SetHappiness(SpecialEffectList happiness, DataContext context)
	{
		_happiness = happiness;
		SetModifiedAndInvalidateInfluencedCache(52, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _happiness.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 51, serializedSize);
			ptr += _happiness.Serialize(ptr);
		}
	}

	public SpecialEffectList GetMaxHealth()
	{
		return _maxHealth;
	}

	public unsafe void SetMaxHealth(SpecialEffectList maxHealth, DataContext context)
	{
		_maxHealth = maxHealth;
		SetModifiedAndInvalidateInfluencedCache(53, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _maxHealth.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 52, serializedSize);
			ptr += _maxHealth.Serialize(ptr);
		}
	}

	public SpecialEffectList GetHealthCost()
	{
		return _healthCost;
	}

	public unsafe void SetHealthCost(SpecialEffectList healthCost, DataContext context)
	{
		_healthCost = healthCost;
		SetModifiedAndInvalidateInfluencedCache(54, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _healthCost.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 53, serializedSize);
			ptr += _healthCost.Serialize(ptr);
		}
	}

	public SpecialEffectList GetMoveSpeedCanChange()
	{
		return _moveSpeedCanChange;
	}

	public unsafe void SetMoveSpeedCanChange(SpecialEffectList moveSpeedCanChange, DataContext context)
	{
		_moveSpeedCanChange = moveSpeedCanChange;
		SetModifiedAndInvalidateInfluencedCache(55, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _moveSpeedCanChange.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 54, serializedSize);
			ptr += _moveSpeedCanChange.Serialize(ptr);
		}
	}

	public SpecialEffectList GetAttackerHitStrength()
	{
		return _attackerHitStrength;
	}

	public unsafe void SetAttackerHitStrength(SpecialEffectList attackerHitStrength, DataContext context)
	{
		_attackerHitStrength = attackerHitStrength;
		SetModifiedAndInvalidateInfluencedCache(56, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _attackerHitStrength.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 55, serializedSize);
			ptr += _attackerHitStrength.Serialize(ptr);
		}
	}

	public SpecialEffectList GetAttackerHitTechnique()
	{
		return _attackerHitTechnique;
	}

	public unsafe void SetAttackerHitTechnique(SpecialEffectList attackerHitTechnique, DataContext context)
	{
		_attackerHitTechnique = attackerHitTechnique;
		SetModifiedAndInvalidateInfluencedCache(57, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _attackerHitTechnique.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 56, serializedSize);
			ptr += _attackerHitTechnique.Serialize(ptr);
		}
	}

	public SpecialEffectList GetAttackerHitSpeed()
	{
		return _attackerHitSpeed;
	}

	public unsafe void SetAttackerHitSpeed(SpecialEffectList attackerHitSpeed, DataContext context)
	{
		_attackerHitSpeed = attackerHitSpeed;
		SetModifiedAndInvalidateInfluencedCache(58, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _attackerHitSpeed.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 57, serializedSize);
			ptr += _attackerHitSpeed.Serialize(ptr);
		}
	}

	public SpecialEffectList GetAttackerHitMind()
	{
		return _attackerHitMind;
	}

	public unsafe void SetAttackerHitMind(SpecialEffectList attackerHitMind, DataContext context)
	{
		_attackerHitMind = attackerHitMind;
		SetModifiedAndInvalidateInfluencedCache(59, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _attackerHitMind.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 58, serializedSize);
			ptr += _attackerHitMind.Serialize(ptr);
		}
	}

	public SpecialEffectList GetAttackerAvoidStrength()
	{
		return _attackerAvoidStrength;
	}

	public unsafe void SetAttackerAvoidStrength(SpecialEffectList attackerAvoidStrength, DataContext context)
	{
		_attackerAvoidStrength = attackerAvoidStrength;
		SetModifiedAndInvalidateInfluencedCache(60, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _attackerAvoidStrength.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 59, serializedSize);
			ptr += _attackerAvoidStrength.Serialize(ptr);
		}
	}

	public SpecialEffectList GetAttackerAvoidTechnique()
	{
		return _attackerAvoidTechnique;
	}

	public unsafe void SetAttackerAvoidTechnique(SpecialEffectList attackerAvoidTechnique, DataContext context)
	{
		_attackerAvoidTechnique = attackerAvoidTechnique;
		SetModifiedAndInvalidateInfluencedCache(61, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _attackerAvoidTechnique.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 60, serializedSize);
			ptr += _attackerAvoidTechnique.Serialize(ptr);
		}
	}

	public SpecialEffectList GetAttackerAvoidSpeed()
	{
		return _attackerAvoidSpeed;
	}

	public unsafe void SetAttackerAvoidSpeed(SpecialEffectList attackerAvoidSpeed, DataContext context)
	{
		_attackerAvoidSpeed = attackerAvoidSpeed;
		SetModifiedAndInvalidateInfluencedCache(62, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _attackerAvoidSpeed.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 61, serializedSize);
			ptr += _attackerAvoidSpeed.Serialize(ptr);
		}
	}

	public SpecialEffectList GetAttackerAvoidMind()
	{
		return _attackerAvoidMind;
	}

	public unsafe void SetAttackerAvoidMind(SpecialEffectList attackerAvoidMind, DataContext context)
	{
		_attackerAvoidMind = attackerAvoidMind;
		SetModifiedAndInvalidateInfluencedCache(63, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _attackerAvoidMind.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 62, serializedSize);
			ptr += _attackerAvoidMind.Serialize(ptr);
		}
	}

	public SpecialEffectList GetAttackerPenetrateOuter()
	{
		return _attackerPenetrateOuter;
	}

	public unsafe void SetAttackerPenetrateOuter(SpecialEffectList attackerPenetrateOuter, DataContext context)
	{
		_attackerPenetrateOuter = attackerPenetrateOuter;
		SetModifiedAndInvalidateInfluencedCache(64, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _attackerPenetrateOuter.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 63, serializedSize);
			ptr += _attackerPenetrateOuter.Serialize(ptr);
		}
	}

	public SpecialEffectList GetAttackerPenetrateInner()
	{
		return _attackerPenetrateInner;
	}

	public unsafe void SetAttackerPenetrateInner(SpecialEffectList attackerPenetrateInner, DataContext context)
	{
		_attackerPenetrateInner = attackerPenetrateInner;
		SetModifiedAndInvalidateInfluencedCache(65, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _attackerPenetrateInner.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 64, serializedSize);
			ptr += _attackerPenetrateInner.Serialize(ptr);
		}
	}

	public SpecialEffectList GetAttackerPenetrateResistOuter()
	{
		return _attackerPenetrateResistOuter;
	}

	public unsafe void SetAttackerPenetrateResistOuter(SpecialEffectList attackerPenetrateResistOuter, DataContext context)
	{
		_attackerPenetrateResistOuter = attackerPenetrateResistOuter;
		SetModifiedAndInvalidateInfluencedCache(66, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _attackerPenetrateResistOuter.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 65, serializedSize);
			ptr += _attackerPenetrateResistOuter.Serialize(ptr);
		}
	}

	public SpecialEffectList GetAttackerPenetrateResistInner()
	{
		return _attackerPenetrateResistInner;
	}

	public unsafe void SetAttackerPenetrateResistInner(SpecialEffectList attackerPenetrateResistInner, DataContext context)
	{
		_attackerPenetrateResistInner = attackerPenetrateResistInner;
		SetModifiedAndInvalidateInfluencedCache(67, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _attackerPenetrateResistInner.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 66, serializedSize);
			ptr += _attackerPenetrateResistInner.Serialize(ptr);
		}
	}

	public SpecialEffectList GetAttackHitType()
	{
		return _attackHitType;
	}

	public unsafe void SetAttackHitType(SpecialEffectList attackHitType, DataContext context)
	{
		_attackHitType = attackHitType;
		SetModifiedAndInvalidateInfluencedCache(68, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _attackHitType.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 67, serializedSize);
			ptr += _attackHitType.Serialize(ptr);
		}
	}

	public SpecialEffectList GetMakeDirectDamage()
	{
		return _makeDirectDamage;
	}

	public unsafe void SetMakeDirectDamage(SpecialEffectList makeDirectDamage, DataContext context)
	{
		_makeDirectDamage = makeDirectDamage;
		SetModifiedAndInvalidateInfluencedCache(69, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _makeDirectDamage.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 68, serializedSize);
			ptr += _makeDirectDamage.Serialize(ptr);
		}
	}

	public SpecialEffectList GetMakeBounceDamage()
	{
		return _makeBounceDamage;
	}

	public unsafe void SetMakeBounceDamage(SpecialEffectList makeBounceDamage, DataContext context)
	{
		_makeBounceDamage = makeBounceDamage;
		SetModifiedAndInvalidateInfluencedCache(70, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _makeBounceDamage.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 69, serializedSize);
			ptr += _makeBounceDamage.Serialize(ptr);
		}
	}

	public SpecialEffectList GetMakeFightBackDamage()
	{
		return _makeFightBackDamage;
	}

	public unsafe void SetMakeFightBackDamage(SpecialEffectList makeFightBackDamage, DataContext context)
	{
		_makeFightBackDamage = makeFightBackDamage;
		SetModifiedAndInvalidateInfluencedCache(71, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _makeFightBackDamage.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 70, serializedSize);
			ptr += _makeFightBackDamage.Serialize(ptr);
		}
	}

	public SpecialEffectList GetMakePoisonLevel()
	{
		return _makePoisonLevel;
	}

	public unsafe void SetMakePoisonLevel(SpecialEffectList makePoisonLevel, DataContext context)
	{
		_makePoisonLevel = makePoisonLevel;
		SetModifiedAndInvalidateInfluencedCache(72, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _makePoisonLevel.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 71, serializedSize);
			ptr += _makePoisonLevel.Serialize(ptr);
		}
	}

	public SpecialEffectList GetMakePoisonValue()
	{
		return _makePoisonValue;
	}

	public unsafe void SetMakePoisonValue(SpecialEffectList makePoisonValue, DataContext context)
	{
		_makePoisonValue = makePoisonValue;
		SetModifiedAndInvalidateInfluencedCache(73, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _makePoisonValue.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 72, serializedSize);
			ptr += _makePoisonValue.Serialize(ptr);
		}
	}

	public SpecialEffectList GetAttackerHitOdds()
	{
		return _attackerHitOdds;
	}

	public unsafe void SetAttackerHitOdds(SpecialEffectList attackerHitOdds, DataContext context)
	{
		_attackerHitOdds = attackerHitOdds;
		SetModifiedAndInvalidateInfluencedCache(74, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _attackerHitOdds.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 73, serializedSize);
			ptr += _attackerHitOdds.Serialize(ptr);
		}
	}

	public SpecialEffectList GetAttackerFightBackHitOdds()
	{
		return _attackerFightBackHitOdds;
	}

	public unsafe void SetAttackerFightBackHitOdds(SpecialEffectList attackerFightBackHitOdds, DataContext context)
	{
		_attackerFightBackHitOdds = attackerFightBackHitOdds;
		SetModifiedAndInvalidateInfluencedCache(75, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _attackerFightBackHitOdds.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 74, serializedSize);
			ptr += _attackerFightBackHitOdds.Serialize(ptr);
		}
	}

	public SpecialEffectList GetAttackerPursueOdds()
	{
		return _attackerPursueOdds;
	}

	public unsafe void SetAttackerPursueOdds(SpecialEffectList attackerPursueOdds, DataContext context)
	{
		_attackerPursueOdds = attackerPursueOdds;
		SetModifiedAndInvalidateInfluencedCache(76, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _attackerPursueOdds.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 75, serializedSize);
			ptr += _attackerPursueOdds.Serialize(ptr);
		}
	}

	public SpecialEffectList GetMakedInjuryChangeToOld()
	{
		return _makedInjuryChangeToOld;
	}

	public unsafe void SetMakedInjuryChangeToOld(SpecialEffectList makedInjuryChangeToOld, DataContext context)
	{
		_makedInjuryChangeToOld = makedInjuryChangeToOld;
		SetModifiedAndInvalidateInfluencedCache(77, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _makedInjuryChangeToOld.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 76, serializedSize);
			ptr += _makedInjuryChangeToOld.Serialize(ptr);
		}
	}

	public SpecialEffectList GetMakedPoisonChangeToOld()
	{
		return _makedPoisonChangeToOld;
	}

	public unsafe void SetMakedPoisonChangeToOld(SpecialEffectList makedPoisonChangeToOld, DataContext context)
	{
		_makedPoisonChangeToOld = makedPoisonChangeToOld;
		SetModifiedAndInvalidateInfluencedCache(78, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _makedPoisonChangeToOld.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 77, serializedSize);
			ptr += _makedPoisonChangeToOld.Serialize(ptr);
		}
	}

	public SpecialEffectList GetMakeDamageType()
	{
		return _makeDamageType;
	}

	public unsafe void SetMakeDamageType(SpecialEffectList makeDamageType, DataContext context)
	{
		_makeDamageType = makeDamageType;
		SetModifiedAndInvalidateInfluencedCache(79, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _makeDamageType.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 78, serializedSize);
			ptr += _makeDamageType.Serialize(ptr);
		}
	}

	public SpecialEffectList GetCanMakeInjuryToNoInjuryPart()
	{
		return _canMakeInjuryToNoInjuryPart;
	}

	public unsafe void SetCanMakeInjuryToNoInjuryPart(SpecialEffectList canMakeInjuryToNoInjuryPart, DataContext context)
	{
		_canMakeInjuryToNoInjuryPart = canMakeInjuryToNoInjuryPart;
		SetModifiedAndInvalidateInfluencedCache(80, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _canMakeInjuryToNoInjuryPart.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 79, serializedSize);
			ptr += _canMakeInjuryToNoInjuryPart.Serialize(ptr);
		}
	}

	public SpecialEffectList GetMakePoisonType()
	{
		return _makePoisonType;
	}

	public unsafe void SetMakePoisonType(SpecialEffectList makePoisonType, DataContext context)
	{
		_makePoisonType = makePoisonType;
		SetModifiedAndInvalidateInfluencedCache(81, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _makePoisonType.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 80, serializedSize);
			ptr += _makePoisonType.Serialize(ptr);
		}
	}

	public SpecialEffectList GetNormalAttackWeapon()
	{
		return _normalAttackWeapon;
	}

	public unsafe void SetNormalAttackWeapon(SpecialEffectList normalAttackWeapon, DataContext context)
	{
		_normalAttackWeapon = normalAttackWeapon;
		SetModifiedAndInvalidateInfluencedCache(82, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _normalAttackWeapon.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 81, serializedSize);
			ptr += _normalAttackWeapon.Serialize(ptr);
		}
	}

	public SpecialEffectList GetNormalAttackTrick()
	{
		return _normalAttackTrick;
	}

	public unsafe void SetNormalAttackTrick(SpecialEffectList normalAttackTrick, DataContext context)
	{
		_normalAttackTrick = normalAttackTrick;
		SetModifiedAndInvalidateInfluencedCache(83, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _normalAttackTrick.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 82, serializedSize);
			ptr += _normalAttackTrick.Serialize(ptr);
		}
	}

	public SpecialEffectList GetExtraFlawCount()
	{
		return _extraFlawCount;
	}

	public unsafe void SetExtraFlawCount(SpecialEffectList extraFlawCount, DataContext context)
	{
		_extraFlawCount = extraFlawCount;
		SetModifiedAndInvalidateInfluencedCache(84, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _extraFlawCount.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 83, serializedSize);
			ptr += _extraFlawCount.Serialize(ptr);
		}
	}

	public SpecialEffectList GetAttackCanBounce()
	{
		return _attackCanBounce;
	}

	public unsafe void SetAttackCanBounce(SpecialEffectList attackCanBounce, DataContext context)
	{
		_attackCanBounce = attackCanBounce;
		SetModifiedAndInvalidateInfluencedCache(85, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _attackCanBounce.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 84, serializedSize);
			ptr += _attackCanBounce.Serialize(ptr);
		}
	}

	public SpecialEffectList GetAttackCanFightBack()
	{
		return _attackCanFightBack;
	}

	public unsafe void SetAttackCanFightBack(SpecialEffectList attackCanFightBack, DataContext context)
	{
		_attackCanFightBack = attackCanFightBack;
		SetModifiedAndInvalidateInfluencedCache(86, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _attackCanFightBack.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 85, serializedSize);
			ptr += _attackCanFightBack.Serialize(ptr);
		}
	}

	public SpecialEffectList GetMakeFightBackInjuryMark()
	{
		return _makeFightBackInjuryMark;
	}

	public unsafe void SetMakeFightBackInjuryMark(SpecialEffectList makeFightBackInjuryMark, DataContext context)
	{
		_makeFightBackInjuryMark = makeFightBackInjuryMark;
		SetModifiedAndInvalidateInfluencedCache(87, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _makeFightBackInjuryMark.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 86, serializedSize);
			ptr += _makeFightBackInjuryMark.Serialize(ptr);
		}
	}

	public SpecialEffectList GetLegSkillUseShoes()
	{
		return _legSkillUseShoes;
	}

	public unsafe void SetLegSkillUseShoes(SpecialEffectList legSkillUseShoes, DataContext context)
	{
		_legSkillUseShoes = legSkillUseShoes;
		SetModifiedAndInvalidateInfluencedCache(88, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _legSkillUseShoes.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 87, serializedSize);
			ptr += _legSkillUseShoes.Serialize(ptr);
		}
	}

	public SpecialEffectList GetAttackerFinalDamageValue()
	{
		return _attackerFinalDamageValue;
	}

	public unsafe void SetAttackerFinalDamageValue(SpecialEffectList attackerFinalDamageValue, DataContext context)
	{
		_attackerFinalDamageValue = attackerFinalDamageValue;
		SetModifiedAndInvalidateInfluencedCache(89, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _attackerFinalDamageValue.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 88, serializedSize);
			ptr += _attackerFinalDamageValue.Serialize(ptr);
		}
	}

	public SpecialEffectList GetDefenderHitStrength()
	{
		return _defenderHitStrength;
	}

	public unsafe void SetDefenderHitStrength(SpecialEffectList defenderHitStrength, DataContext context)
	{
		_defenderHitStrength = defenderHitStrength;
		SetModifiedAndInvalidateInfluencedCache(90, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _defenderHitStrength.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 89, serializedSize);
			ptr += _defenderHitStrength.Serialize(ptr);
		}
	}

	public SpecialEffectList GetDefenderHitTechnique()
	{
		return _defenderHitTechnique;
	}

	public unsafe void SetDefenderHitTechnique(SpecialEffectList defenderHitTechnique, DataContext context)
	{
		_defenderHitTechnique = defenderHitTechnique;
		SetModifiedAndInvalidateInfluencedCache(91, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _defenderHitTechnique.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 90, serializedSize);
			ptr += _defenderHitTechnique.Serialize(ptr);
		}
	}

	public SpecialEffectList GetDefenderHitSpeed()
	{
		return _defenderHitSpeed;
	}

	public unsafe void SetDefenderHitSpeed(SpecialEffectList defenderHitSpeed, DataContext context)
	{
		_defenderHitSpeed = defenderHitSpeed;
		SetModifiedAndInvalidateInfluencedCache(92, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _defenderHitSpeed.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 91, serializedSize);
			ptr += _defenderHitSpeed.Serialize(ptr);
		}
	}

	public SpecialEffectList GetDefenderHitMind()
	{
		return _defenderHitMind;
	}

	public unsafe void SetDefenderHitMind(SpecialEffectList defenderHitMind, DataContext context)
	{
		_defenderHitMind = defenderHitMind;
		SetModifiedAndInvalidateInfluencedCache(93, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _defenderHitMind.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 92, serializedSize);
			ptr += _defenderHitMind.Serialize(ptr);
		}
	}

	public SpecialEffectList GetDefenderAvoidStrength()
	{
		return _defenderAvoidStrength;
	}

	public unsafe void SetDefenderAvoidStrength(SpecialEffectList defenderAvoidStrength, DataContext context)
	{
		_defenderAvoidStrength = defenderAvoidStrength;
		SetModifiedAndInvalidateInfluencedCache(94, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _defenderAvoidStrength.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 93, serializedSize);
			ptr += _defenderAvoidStrength.Serialize(ptr);
		}
	}

	public SpecialEffectList GetDefenderAvoidTechnique()
	{
		return _defenderAvoidTechnique;
	}

	public unsafe void SetDefenderAvoidTechnique(SpecialEffectList defenderAvoidTechnique, DataContext context)
	{
		_defenderAvoidTechnique = defenderAvoidTechnique;
		SetModifiedAndInvalidateInfluencedCache(95, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _defenderAvoidTechnique.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 94, serializedSize);
			ptr += _defenderAvoidTechnique.Serialize(ptr);
		}
	}

	public SpecialEffectList GetDefenderAvoidSpeed()
	{
		return _defenderAvoidSpeed;
	}

	public unsafe void SetDefenderAvoidSpeed(SpecialEffectList defenderAvoidSpeed, DataContext context)
	{
		_defenderAvoidSpeed = defenderAvoidSpeed;
		SetModifiedAndInvalidateInfluencedCache(96, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _defenderAvoidSpeed.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 95, serializedSize);
			ptr += _defenderAvoidSpeed.Serialize(ptr);
		}
	}

	public SpecialEffectList GetDefenderAvoidMind()
	{
		return _defenderAvoidMind;
	}

	public unsafe void SetDefenderAvoidMind(SpecialEffectList defenderAvoidMind, DataContext context)
	{
		_defenderAvoidMind = defenderAvoidMind;
		SetModifiedAndInvalidateInfluencedCache(97, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _defenderAvoidMind.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 96, serializedSize);
			ptr += _defenderAvoidMind.Serialize(ptr);
		}
	}

	public SpecialEffectList GetDefenderPenetrateOuter()
	{
		return _defenderPenetrateOuter;
	}

	public unsafe void SetDefenderPenetrateOuter(SpecialEffectList defenderPenetrateOuter, DataContext context)
	{
		_defenderPenetrateOuter = defenderPenetrateOuter;
		SetModifiedAndInvalidateInfluencedCache(98, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _defenderPenetrateOuter.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 97, serializedSize);
			ptr += _defenderPenetrateOuter.Serialize(ptr);
		}
	}

	public SpecialEffectList GetDefenderPenetrateInner()
	{
		return _defenderPenetrateInner;
	}

	public unsafe void SetDefenderPenetrateInner(SpecialEffectList defenderPenetrateInner, DataContext context)
	{
		_defenderPenetrateInner = defenderPenetrateInner;
		SetModifiedAndInvalidateInfluencedCache(99, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _defenderPenetrateInner.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 98, serializedSize);
			ptr += _defenderPenetrateInner.Serialize(ptr);
		}
	}

	public SpecialEffectList GetDefenderPenetrateResistOuter()
	{
		return _defenderPenetrateResistOuter;
	}

	public unsafe void SetDefenderPenetrateResistOuter(SpecialEffectList defenderPenetrateResistOuter, DataContext context)
	{
		_defenderPenetrateResistOuter = defenderPenetrateResistOuter;
		SetModifiedAndInvalidateInfluencedCache(100, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _defenderPenetrateResistOuter.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 99, serializedSize);
			ptr += _defenderPenetrateResistOuter.Serialize(ptr);
		}
	}

	public SpecialEffectList GetDefenderPenetrateResistInner()
	{
		return _defenderPenetrateResistInner;
	}

	public unsafe void SetDefenderPenetrateResistInner(SpecialEffectList defenderPenetrateResistInner, DataContext context)
	{
		_defenderPenetrateResistInner = defenderPenetrateResistInner;
		SetModifiedAndInvalidateInfluencedCache(101, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _defenderPenetrateResistInner.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 100, serializedSize);
			ptr += _defenderPenetrateResistInner.Serialize(ptr);
		}
	}

	public SpecialEffectList GetAcceptDirectDamage()
	{
		return _acceptDirectDamage;
	}

	public unsafe void SetAcceptDirectDamage(SpecialEffectList acceptDirectDamage, DataContext context)
	{
		_acceptDirectDamage = acceptDirectDamage;
		SetModifiedAndInvalidateInfluencedCache(102, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _acceptDirectDamage.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 101, serializedSize);
			ptr += _acceptDirectDamage.Serialize(ptr);
		}
	}

	public SpecialEffectList GetAcceptBounceDamage()
	{
		return _acceptBounceDamage;
	}

	public unsafe void SetAcceptBounceDamage(SpecialEffectList acceptBounceDamage, DataContext context)
	{
		_acceptBounceDamage = acceptBounceDamage;
		SetModifiedAndInvalidateInfluencedCache(103, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _acceptBounceDamage.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 102, serializedSize);
			ptr += _acceptBounceDamage.Serialize(ptr);
		}
	}

	public SpecialEffectList GetAcceptFightBackDamage()
	{
		return _acceptFightBackDamage;
	}

	public unsafe void SetAcceptFightBackDamage(SpecialEffectList acceptFightBackDamage, DataContext context)
	{
		_acceptFightBackDamage = acceptFightBackDamage;
		SetModifiedAndInvalidateInfluencedCache(104, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _acceptFightBackDamage.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 103, serializedSize);
			ptr += _acceptFightBackDamage.Serialize(ptr);
		}
	}

	public SpecialEffectList GetAcceptPoisonLevel()
	{
		return _acceptPoisonLevel;
	}

	public unsafe void SetAcceptPoisonLevel(SpecialEffectList acceptPoisonLevel, DataContext context)
	{
		_acceptPoisonLevel = acceptPoisonLevel;
		SetModifiedAndInvalidateInfluencedCache(105, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _acceptPoisonLevel.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 104, serializedSize);
			ptr += _acceptPoisonLevel.Serialize(ptr);
		}
	}

	public SpecialEffectList GetAcceptPoisonValue()
	{
		return _acceptPoisonValue;
	}

	public unsafe void SetAcceptPoisonValue(SpecialEffectList acceptPoisonValue, DataContext context)
	{
		_acceptPoisonValue = acceptPoisonValue;
		SetModifiedAndInvalidateInfluencedCache(106, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _acceptPoisonValue.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 105, serializedSize);
			ptr += _acceptPoisonValue.Serialize(ptr);
		}
	}

	public SpecialEffectList GetDefenderHitOdds()
	{
		return _defenderHitOdds;
	}

	public unsafe void SetDefenderHitOdds(SpecialEffectList defenderHitOdds, DataContext context)
	{
		_defenderHitOdds = defenderHitOdds;
		SetModifiedAndInvalidateInfluencedCache(107, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _defenderHitOdds.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 106, serializedSize);
			ptr += _defenderHitOdds.Serialize(ptr);
		}
	}

	public SpecialEffectList GetDefenderFightBackHitOdds()
	{
		return _defenderFightBackHitOdds;
	}

	public unsafe void SetDefenderFightBackHitOdds(SpecialEffectList defenderFightBackHitOdds, DataContext context)
	{
		_defenderFightBackHitOdds = defenderFightBackHitOdds;
		SetModifiedAndInvalidateInfluencedCache(108, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _defenderFightBackHitOdds.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 107, serializedSize);
			ptr += _defenderFightBackHitOdds.Serialize(ptr);
		}
	}

	public SpecialEffectList GetDefenderPursueOdds()
	{
		return _defenderPursueOdds;
	}

	public unsafe void SetDefenderPursueOdds(SpecialEffectList defenderPursueOdds, DataContext context)
	{
		_defenderPursueOdds = defenderPursueOdds;
		SetModifiedAndInvalidateInfluencedCache(109, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _defenderPursueOdds.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 108, serializedSize);
			ptr += _defenderPursueOdds.Serialize(ptr);
		}
	}

	public SpecialEffectList GetAcceptMaxInjuryCount()
	{
		return _acceptMaxInjuryCount;
	}

	public unsafe void SetAcceptMaxInjuryCount(SpecialEffectList acceptMaxInjuryCount, DataContext context)
	{
		_acceptMaxInjuryCount = acceptMaxInjuryCount;
		SetModifiedAndInvalidateInfluencedCache(110, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _acceptMaxInjuryCount.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 109, serializedSize);
			ptr += _acceptMaxInjuryCount.Serialize(ptr);
		}
	}

	public SpecialEffectList GetBouncePower()
	{
		return _bouncePower;
	}

	public unsafe void SetBouncePower(SpecialEffectList bouncePower, DataContext context)
	{
		_bouncePower = bouncePower;
		SetModifiedAndInvalidateInfluencedCache(111, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _bouncePower.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 110, serializedSize);
			ptr += _bouncePower.Serialize(ptr);
		}
	}

	public SpecialEffectList GetFightBackPower()
	{
		return _fightBackPower;
	}

	public unsafe void SetFightBackPower(SpecialEffectList fightBackPower, DataContext context)
	{
		_fightBackPower = fightBackPower;
		SetModifiedAndInvalidateInfluencedCache(112, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _fightBackPower.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 111, serializedSize);
			ptr += _fightBackPower.Serialize(ptr);
		}
	}

	public SpecialEffectList GetDirectDamageInnerRatio()
	{
		return _directDamageInnerRatio;
	}

	public unsafe void SetDirectDamageInnerRatio(SpecialEffectList directDamageInnerRatio, DataContext context)
	{
		_directDamageInnerRatio = directDamageInnerRatio;
		SetModifiedAndInvalidateInfluencedCache(113, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _directDamageInnerRatio.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 112, serializedSize);
			ptr += _directDamageInnerRatio.Serialize(ptr);
		}
	}

	public SpecialEffectList GetDefenderFinalDamageValue()
	{
		return _defenderFinalDamageValue;
	}

	public unsafe void SetDefenderFinalDamageValue(SpecialEffectList defenderFinalDamageValue, DataContext context)
	{
		_defenderFinalDamageValue = defenderFinalDamageValue;
		SetModifiedAndInvalidateInfluencedCache(114, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _defenderFinalDamageValue.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 113, serializedSize);
			ptr += _defenderFinalDamageValue.Serialize(ptr);
		}
	}

	public SpecialEffectList GetDirectDamageValue()
	{
		return _directDamageValue;
	}

	public unsafe void SetDirectDamageValue(SpecialEffectList directDamageValue, DataContext context)
	{
		_directDamageValue = directDamageValue;
		SetModifiedAndInvalidateInfluencedCache(115, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _directDamageValue.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 114, serializedSize);
			ptr += _directDamageValue.Serialize(ptr);
		}
	}

	public SpecialEffectList GetDirectInjuryMark()
	{
		return _directInjuryMark;
	}

	public unsafe void SetDirectInjuryMark(SpecialEffectList directInjuryMark, DataContext context)
	{
		_directInjuryMark = directInjuryMark;
		SetModifiedAndInvalidateInfluencedCache(116, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _directInjuryMark.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 115, serializedSize);
			ptr += _directInjuryMark.Serialize(ptr);
		}
	}

	public SpecialEffectList GetGoneMadInjury()
	{
		return _goneMadInjury;
	}

	public unsafe void SetGoneMadInjury(SpecialEffectList goneMadInjury, DataContext context)
	{
		_goneMadInjury = goneMadInjury;
		SetModifiedAndInvalidateInfluencedCache(117, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _goneMadInjury.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 116, serializedSize);
			ptr += _goneMadInjury.Serialize(ptr);
		}
	}

	public SpecialEffectList GetHealInjurySpeed()
	{
		return _healInjurySpeed;
	}

	public unsafe void SetHealInjurySpeed(SpecialEffectList healInjurySpeed, DataContext context)
	{
		_healInjurySpeed = healInjurySpeed;
		SetModifiedAndInvalidateInfluencedCache(118, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _healInjurySpeed.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 117, serializedSize);
			ptr += _healInjurySpeed.Serialize(ptr);
		}
	}

	public SpecialEffectList GetHealInjuryBuff()
	{
		return _healInjuryBuff;
	}

	public unsafe void SetHealInjuryBuff(SpecialEffectList healInjuryBuff, DataContext context)
	{
		_healInjuryBuff = healInjuryBuff;
		SetModifiedAndInvalidateInfluencedCache(119, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _healInjuryBuff.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 118, serializedSize);
			ptr += _healInjuryBuff.Serialize(ptr);
		}
	}

	public SpecialEffectList GetHealInjuryDebuff()
	{
		return _healInjuryDebuff;
	}

	public unsafe void SetHealInjuryDebuff(SpecialEffectList healInjuryDebuff, DataContext context)
	{
		_healInjuryDebuff = healInjuryDebuff;
		SetModifiedAndInvalidateInfluencedCache(120, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _healInjuryDebuff.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 119, serializedSize);
			ptr += _healInjuryDebuff.Serialize(ptr);
		}
	}

	public SpecialEffectList GetHealPoisonSpeed()
	{
		return _healPoisonSpeed;
	}

	public unsafe void SetHealPoisonSpeed(SpecialEffectList healPoisonSpeed, DataContext context)
	{
		_healPoisonSpeed = healPoisonSpeed;
		SetModifiedAndInvalidateInfluencedCache(121, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _healPoisonSpeed.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 120, serializedSize);
			ptr += _healPoisonSpeed.Serialize(ptr);
		}
	}

	public SpecialEffectList GetHealPoisonBuff()
	{
		return _healPoisonBuff;
	}

	public unsafe void SetHealPoisonBuff(SpecialEffectList healPoisonBuff, DataContext context)
	{
		_healPoisonBuff = healPoisonBuff;
		SetModifiedAndInvalidateInfluencedCache(122, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _healPoisonBuff.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 121, serializedSize);
			ptr += _healPoisonBuff.Serialize(ptr);
		}
	}

	public SpecialEffectList GetHealPoisonDebuff()
	{
		return _healPoisonDebuff;
	}

	public unsafe void SetHealPoisonDebuff(SpecialEffectList healPoisonDebuff, DataContext context)
	{
		_healPoisonDebuff = healPoisonDebuff;
		SetModifiedAndInvalidateInfluencedCache(123, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _healPoisonDebuff.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 122, serializedSize);
			ptr += _healPoisonDebuff.Serialize(ptr);
		}
	}

	public SpecialEffectList GetFleeSpeed()
	{
		return _fleeSpeed;
	}

	public unsafe void SetFleeSpeed(SpecialEffectList fleeSpeed, DataContext context)
	{
		_fleeSpeed = fleeSpeed;
		SetModifiedAndInvalidateInfluencedCache(124, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _fleeSpeed.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 123, serializedSize);
			ptr += _fleeSpeed.Serialize(ptr);
		}
	}

	public SpecialEffectList GetMaxFlawCount()
	{
		return _maxFlawCount;
	}

	public unsafe void SetMaxFlawCount(SpecialEffectList maxFlawCount, DataContext context)
	{
		_maxFlawCount = maxFlawCount;
		SetModifiedAndInvalidateInfluencedCache(125, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _maxFlawCount.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 124, serializedSize);
			ptr += _maxFlawCount.Serialize(ptr);
		}
	}

	public SpecialEffectList GetCanAddFlaw()
	{
		return _canAddFlaw;
	}

	public unsafe void SetCanAddFlaw(SpecialEffectList canAddFlaw, DataContext context)
	{
		_canAddFlaw = canAddFlaw;
		SetModifiedAndInvalidateInfluencedCache(126, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _canAddFlaw.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 125, serializedSize);
			ptr += _canAddFlaw.Serialize(ptr);
		}
	}

	public SpecialEffectList GetFlawLevel()
	{
		return _flawLevel;
	}

	public unsafe void SetFlawLevel(SpecialEffectList flawLevel, DataContext context)
	{
		_flawLevel = flawLevel;
		SetModifiedAndInvalidateInfluencedCache(127, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _flawLevel.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 126, serializedSize);
			ptr += _flawLevel.Serialize(ptr);
		}
	}

	public SpecialEffectList GetFlawLevelCanReduce()
	{
		return _flawLevelCanReduce;
	}

	public unsafe void SetFlawLevelCanReduce(SpecialEffectList flawLevelCanReduce, DataContext context)
	{
		_flawLevelCanReduce = flawLevelCanReduce;
		SetModifiedAndInvalidateInfluencedCache(128, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _flawLevelCanReduce.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 127, serializedSize);
			ptr += _flawLevelCanReduce.Serialize(ptr);
		}
	}

	public SpecialEffectList GetFlawCount()
	{
		return _flawCount;
	}

	public unsafe void SetFlawCount(SpecialEffectList flawCount, DataContext context)
	{
		_flawCount = flawCount;
		SetModifiedAndInvalidateInfluencedCache(129, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _flawCount.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 128, serializedSize);
			ptr += _flawCount.Serialize(ptr);
		}
	}

	public SpecialEffectList GetMaxAcupointCount()
	{
		return _maxAcupointCount;
	}

	public unsafe void SetMaxAcupointCount(SpecialEffectList maxAcupointCount, DataContext context)
	{
		_maxAcupointCount = maxAcupointCount;
		SetModifiedAndInvalidateInfluencedCache(130, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _maxAcupointCount.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 129, serializedSize);
			ptr += _maxAcupointCount.Serialize(ptr);
		}
	}

	public SpecialEffectList GetCanAddAcupoint()
	{
		return _canAddAcupoint;
	}

	public unsafe void SetCanAddAcupoint(SpecialEffectList canAddAcupoint, DataContext context)
	{
		_canAddAcupoint = canAddAcupoint;
		SetModifiedAndInvalidateInfluencedCache(131, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _canAddAcupoint.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 130, serializedSize);
			ptr += _canAddAcupoint.Serialize(ptr);
		}
	}

	public SpecialEffectList GetAcupointLevel()
	{
		return _acupointLevel;
	}

	public unsafe void SetAcupointLevel(SpecialEffectList acupointLevel, DataContext context)
	{
		_acupointLevel = acupointLevel;
		SetModifiedAndInvalidateInfluencedCache(132, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _acupointLevel.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 131, serializedSize);
			ptr += _acupointLevel.Serialize(ptr);
		}
	}

	public SpecialEffectList GetAcupointLevelCanReduce()
	{
		return _acupointLevelCanReduce;
	}

	public unsafe void SetAcupointLevelCanReduce(SpecialEffectList acupointLevelCanReduce, DataContext context)
	{
		_acupointLevelCanReduce = acupointLevelCanReduce;
		SetModifiedAndInvalidateInfluencedCache(133, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _acupointLevelCanReduce.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 132, serializedSize);
			ptr += _acupointLevelCanReduce.Serialize(ptr);
		}
	}

	public SpecialEffectList GetAcupointCount()
	{
		return _acupointCount;
	}

	public unsafe void SetAcupointCount(SpecialEffectList acupointCount, DataContext context)
	{
		_acupointCount = acupointCount;
		SetModifiedAndInvalidateInfluencedCache(134, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _acupointCount.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 133, serializedSize);
			ptr += _acupointCount.Serialize(ptr);
		}
	}

	public SpecialEffectList GetAddNeiliAllocation()
	{
		return _addNeiliAllocation;
	}

	public unsafe void SetAddNeiliAllocation(SpecialEffectList addNeiliAllocation, DataContext context)
	{
		_addNeiliAllocation = addNeiliAllocation;
		SetModifiedAndInvalidateInfluencedCache(135, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _addNeiliAllocation.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 134, serializedSize);
			ptr += _addNeiliAllocation.Serialize(ptr);
		}
	}

	public SpecialEffectList GetCostNeiliAllocation()
	{
		return _costNeiliAllocation;
	}

	public unsafe void SetCostNeiliAllocation(SpecialEffectList costNeiliAllocation, DataContext context)
	{
		_costNeiliAllocation = costNeiliAllocation;
		SetModifiedAndInvalidateInfluencedCache(136, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _costNeiliAllocation.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 135, serializedSize);
			ptr += _costNeiliAllocation.Serialize(ptr);
		}
	}

	public SpecialEffectList GetCanChangeNeiliAllocation()
	{
		return _canChangeNeiliAllocation;
	}

	public unsafe void SetCanChangeNeiliAllocation(SpecialEffectList canChangeNeiliAllocation, DataContext context)
	{
		_canChangeNeiliAllocation = canChangeNeiliAllocation;
		SetModifiedAndInvalidateInfluencedCache(137, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _canChangeNeiliAllocation.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 136, serializedSize);
			ptr += _canChangeNeiliAllocation.Serialize(ptr);
		}
	}

	public SpecialEffectList GetCanGetTrick()
	{
		return _canGetTrick;
	}

	public unsafe void SetCanGetTrick(SpecialEffectList canGetTrick, DataContext context)
	{
		_canGetTrick = canGetTrick;
		SetModifiedAndInvalidateInfluencedCache(138, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _canGetTrick.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 137, serializedSize);
			ptr += _canGetTrick.Serialize(ptr);
		}
	}

	public SpecialEffectList GetGetTrickType()
	{
		return _getTrickType;
	}

	public unsafe void SetGetTrickType(SpecialEffectList getTrickType, DataContext context)
	{
		_getTrickType = getTrickType;
		SetModifiedAndInvalidateInfluencedCache(139, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _getTrickType.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 138, serializedSize);
			ptr += _getTrickType.Serialize(ptr);
		}
	}

	public SpecialEffectList GetAttackBodyPart()
	{
		return _attackBodyPart;
	}

	public unsafe void SetAttackBodyPart(SpecialEffectList attackBodyPart, DataContext context)
	{
		_attackBodyPart = attackBodyPart;
		SetModifiedAndInvalidateInfluencedCache(140, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _attackBodyPart.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 139, serializedSize);
			ptr += _attackBodyPart.Serialize(ptr);
		}
	}

	public SpecialEffectList GetWeaponEquipAttack()
	{
		return _weaponEquipAttack;
	}

	public unsafe void SetWeaponEquipAttack(SpecialEffectList weaponEquipAttack, DataContext context)
	{
		_weaponEquipAttack = weaponEquipAttack;
		SetModifiedAndInvalidateInfluencedCache(141, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _weaponEquipAttack.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 140, serializedSize);
			ptr += _weaponEquipAttack.Serialize(ptr);
		}
	}

	public SpecialEffectList GetWeaponEquipDefense()
	{
		return _weaponEquipDefense;
	}

	public unsafe void SetWeaponEquipDefense(SpecialEffectList weaponEquipDefense, DataContext context)
	{
		_weaponEquipDefense = weaponEquipDefense;
		SetModifiedAndInvalidateInfluencedCache(142, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _weaponEquipDefense.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 141, serializedSize);
			ptr += _weaponEquipDefense.Serialize(ptr);
		}
	}

	public SpecialEffectList GetArmorEquipAttack()
	{
		return _armorEquipAttack;
	}

	public unsafe void SetArmorEquipAttack(SpecialEffectList armorEquipAttack, DataContext context)
	{
		_armorEquipAttack = armorEquipAttack;
		SetModifiedAndInvalidateInfluencedCache(143, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _armorEquipAttack.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 142, serializedSize);
			ptr += _armorEquipAttack.Serialize(ptr);
		}
	}

	public SpecialEffectList GetArmorEquipDefense()
	{
		return _armorEquipDefense;
	}

	public unsafe void SetArmorEquipDefense(SpecialEffectList armorEquipDefense, DataContext context)
	{
		_armorEquipDefense = armorEquipDefense;
		SetModifiedAndInvalidateInfluencedCache(144, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _armorEquipDefense.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 143, serializedSize);
			ptr += _armorEquipDefense.Serialize(ptr);
		}
	}

	public SpecialEffectList GetAttackRangeForward()
	{
		return _attackRangeForward;
	}

	public unsafe void SetAttackRangeForward(SpecialEffectList attackRangeForward, DataContext context)
	{
		_attackRangeForward = attackRangeForward;
		SetModifiedAndInvalidateInfluencedCache(145, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _attackRangeForward.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 144, serializedSize);
			ptr += _attackRangeForward.Serialize(ptr);
		}
	}

	public SpecialEffectList GetAttackRangeBackward()
	{
		return _attackRangeBackward;
	}

	public unsafe void SetAttackRangeBackward(SpecialEffectList attackRangeBackward, DataContext context)
	{
		_attackRangeBackward = attackRangeBackward;
		SetModifiedAndInvalidateInfluencedCache(146, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _attackRangeBackward.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 145, serializedSize);
			ptr += _attackRangeBackward.Serialize(ptr);
		}
	}

	public SpecialEffectList GetMoveCanBeStopped()
	{
		return _moveCanBeStopped;
	}

	public unsafe void SetMoveCanBeStopped(SpecialEffectList moveCanBeStopped, DataContext context)
	{
		_moveCanBeStopped = moveCanBeStopped;
		SetModifiedAndInvalidateInfluencedCache(147, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _moveCanBeStopped.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 146, serializedSize);
			ptr += _moveCanBeStopped.Serialize(ptr);
		}
	}

	public SpecialEffectList GetCanForcedMove()
	{
		return _canForcedMove;
	}

	public unsafe void SetCanForcedMove(SpecialEffectList canForcedMove, DataContext context)
	{
		_canForcedMove = canForcedMove;
		SetModifiedAndInvalidateInfluencedCache(148, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _canForcedMove.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 147, serializedSize);
			ptr += _canForcedMove.Serialize(ptr);
		}
	}

	public SpecialEffectList GetMobilityCanBeRemoved()
	{
		return _mobilityCanBeRemoved;
	}

	public unsafe void SetMobilityCanBeRemoved(SpecialEffectList mobilityCanBeRemoved, DataContext context)
	{
		_mobilityCanBeRemoved = mobilityCanBeRemoved;
		SetModifiedAndInvalidateInfluencedCache(149, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _mobilityCanBeRemoved.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 148, serializedSize);
			ptr += _mobilityCanBeRemoved.Serialize(ptr);
		}
	}

	public SpecialEffectList GetMobilityCostByEffect()
	{
		return _mobilityCostByEffect;
	}

	public unsafe void SetMobilityCostByEffect(SpecialEffectList mobilityCostByEffect, DataContext context)
	{
		_mobilityCostByEffect = mobilityCostByEffect;
		SetModifiedAndInvalidateInfluencedCache(150, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _mobilityCostByEffect.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 149, serializedSize);
			ptr += _mobilityCostByEffect.Serialize(ptr);
		}
	}

	public SpecialEffectList GetMoveDistance()
	{
		return _moveDistance;
	}

	public unsafe void SetMoveDistance(SpecialEffectList moveDistance, DataContext context)
	{
		_moveDistance = moveDistance;
		SetModifiedAndInvalidateInfluencedCache(151, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _moveDistance.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 150, serializedSize);
			ptr += _moveDistance.Serialize(ptr);
		}
	}

	public SpecialEffectList GetJumpPrepareFrame()
	{
		return _jumpPrepareFrame;
	}

	public unsafe void SetJumpPrepareFrame(SpecialEffectList jumpPrepareFrame, DataContext context)
	{
		_jumpPrepareFrame = jumpPrepareFrame;
		SetModifiedAndInvalidateInfluencedCache(152, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _jumpPrepareFrame.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 151, serializedSize);
			ptr += _jumpPrepareFrame.Serialize(ptr);
		}
	}

	public SpecialEffectList GetBounceInjuryMark()
	{
		return _bounceInjuryMark;
	}

	public unsafe void SetBounceInjuryMark(SpecialEffectList bounceInjuryMark, DataContext context)
	{
		_bounceInjuryMark = bounceInjuryMark;
		SetModifiedAndInvalidateInfluencedCache(153, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _bounceInjuryMark.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 152, serializedSize);
			ptr += _bounceInjuryMark.Serialize(ptr);
		}
	}

	public SpecialEffectList GetSkillHasCost()
	{
		return _skillHasCost;
	}

	public unsafe void SetSkillHasCost(SpecialEffectList skillHasCost, DataContext context)
	{
		_skillHasCost = skillHasCost;
		SetModifiedAndInvalidateInfluencedCache(154, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _skillHasCost.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 153, serializedSize);
			ptr += _skillHasCost.Serialize(ptr);
		}
	}

	public SpecialEffectList GetCombatStateEffect()
	{
		return _combatStateEffect;
	}

	public unsafe void SetCombatStateEffect(SpecialEffectList combatStateEffect, DataContext context)
	{
		_combatStateEffect = combatStateEffect;
		SetModifiedAndInvalidateInfluencedCache(155, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _combatStateEffect.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 154, serializedSize);
			ptr += _combatStateEffect.Serialize(ptr);
		}
	}

	public SpecialEffectList GetChangeNeedUseSkill()
	{
		return _changeNeedUseSkill;
	}

	public unsafe void SetChangeNeedUseSkill(SpecialEffectList changeNeedUseSkill, DataContext context)
	{
		_changeNeedUseSkill = changeNeedUseSkill;
		SetModifiedAndInvalidateInfluencedCache(156, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _changeNeedUseSkill.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 155, serializedSize);
			ptr += _changeNeedUseSkill.Serialize(ptr);
		}
	}

	public SpecialEffectList GetChangeDistanceIsMove()
	{
		return _changeDistanceIsMove;
	}

	public unsafe void SetChangeDistanceIsMove(SpecialEffectList changeDistanceIsMove, DataContext context)
	{
		_changeDistanceIsMove = changeDistanceIsMove;
		SetModifiedAndInvalidateInfluencedCache(157, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _changeDistanceIsMove.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 156, serializedSize);
			ptr += _changeDistanceIsMove.Serialize(ptr);
		}
	}

	public SpecialEffectList GetReplaceCharHit()
	{
		return _replaceCharHit;
	}

	public unsafe void SetReplaceCharHit(SpecialEffectList replaceCharHit, DataContext context)
	{
		_replaceCharHit = replaceCharHit;
		SetModifiedAndInvalidateInfluencedCache(158, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _replaceCharHit.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 157, serializedSize);
			ptr += _replaceCharHit.Serialize(ptr);
		}
	}

	public SpecialEffectList GetCanAddPoison()
	{
		return _canAddPoison;
	}

	public unsafe void SetCanAddPoison(SpecialEffectList canAddPoison, DataContext context)
	{
		_canAddPoison = canAddPoison;
		SetModifiedAndInvalidateInfluencedCache(159, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _canAddPoison.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 158, serializedSize);
			ptr += _canAddPoison.Serialize(ptr);
		}
	}

	public SpecialEffectList GetCanReducePoison()
	{
		return _canReducePoison;
	}

	public unsafe void SetCanReducePoison(SpecialEffectList canReducePoison, DataContext context)
	{
		_canReducePoison = canReducePoison;
		SetModifiedAndInvalidateInfluencedCache(160, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _canReducePoison.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 159, serializedSize);
			ptr += _canReducePoison.Serialize(ptr);
		}
	}

	public SpecialEffectList GetReducePoisonValue()
	{
		return _reducePoisonValue;
	}

	public unsafe void SetReducePoisonValue(SpecialEffectList reducePoisonValue, DataContext context)
	{
		_reducePoisonValue = reducePoisonValue;
		SetModifiedAndInvalidateInfluencedCache(161, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _reducePoisonValue.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 160, serializedSize);
			ptr += _reducePoisonValue.Serialize(ptr);
		}
	}

	public SpecialEffectList GetPoisonCanAffect()
	{
		return _poisonCanAffect;
	}

	public unsafe void SetPoisonCanAffect(SpecialEffectList poisonCanAffect, DataContext context)
	{
		_poisonCanAffect = poisonCanAffect;
		SetModifiedAndInvalidateInfluencedCache(162, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _poisonCanAffect.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 161, serializedSize);
			ptr += _poisonCanAffect.Serialize(ptr);
		}
	}

	public SpecialEffectList GetPoisonAffectCount()
	{
		return _poisonAffectCount;
	}

	public unsafe void SetPoisonAffectCount(SpecialEffectList poisonAffectCount, DataContext context)
	{
		_poisonAffectCount = poisonAffectCount;
		SetModifiedAndInvalidateInfluencedCache(163, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _poisonAffectCount.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 162, serializedSize);
			ptr += _poisonAffectCount.Serialize(ptr);
		}
	}

	public SpecialEffectList GetCostTricks()
	{
		return _costTricks;
	}

	public unsafe void SetCostTricks(SpecialEffectList costTricks, DataContext context)
	{
		_costTricks = costTricks;
		SetModifiedAndInvalidateInfluencedCache(164, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _costTricks.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 163, serializedSize);
			ptr += _costTricks.Serialize(ptr);
		}
	}

	public SpecialEffectList GetJumpMoveDistance()
	{
		return _jumpMoveDistance;
	}

	public unsafe void SetJumpMoveDistance(SpecialEffectList jumpMoveDistance, DataContext context)
	{
		_jumpMoveDistance = jumpMoveDistance;
		SetModifiedAndInvalidateInfluencedCache(165, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _jumpMoveDistance.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 164, serializedSize);
			ptr += _jumpMoveDistance.Serialize(ptr);
		}
	}

	public SpecialEffectList GetCombatStateToAdd()
	{
		return _combatStateToAdd;
	}

	public unsafe void SetCombatStateToAdd(SpecialEffectList combatStateToAdd, DataContext context)
	{
		_combatStateToAdd = combatStateToAdd;
		SetModifiedAndInvalidateInfluencedCache(166, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _combatStateToAdd.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 165, serializedSize);
			ptr += _combatStateToAdd.Serialize(ptr);
		}
	}

	public SpecialEffectList GetCombatStatePower()
	{
		return _combatStatePower;
	}

	public unsafe void SetCombatStatePower(SpecialEffectList combatStatePower, DataContext context)
	{
		_combatStatePower = combatStatePower;
		SetModifiedAndInvalidateInfluencedCache(167, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _combatStatePower.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 166, serializedSize);
			ptr += _combatStatePower.Serialize(ptr);
		}
	}

	public SpecialEffectList GetBreakBodyPartInjuryCount()
	{
		return _breakBodyPartInjuryCount;
	}

	public unsafe void SetBreakBodyPartInjuryCount(SpecialEffectList breakBodyPartInjuryCount, DataContext context)
	{
		_breakBodyPartInjuryCount = breakBodyPartInjuryCount;
		SetModifiedAndInvalidateInfluencedCache(168, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _breakBodyPartInjuryCount.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 167, serializedSize);
			ptr += _breakBodyPartInjuryCount.Serialize(ptr);
		}
	}

	public SpecialEffectList GetBodyPartIsBroken()
	{
		return _bodyPartIsBroken;
	}

	public unsafe void SetBodyPartIsBroken(SpecialEffectList bodyPartIsBroken, DataContext context)
	{
		_bodyPartIsBroken = bodyPartIsBroken;
		SetModifiedAndInvalidateInfluencedCache(169, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _bodyPartIsBroken.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 168, serializedSize);
			ptr += _bodyPartIsBroken.Serialize(ptr);
		}
	}

	public SpecialEffectList GetMaxTrickCount()
	{
		return _maxTrickCount;
	}

	public unsafe void SetMaxTrickCount(SpecialEffectList maxTrickCount, DataContext context)
	{
		_maxTrickCount = maxTrickCount;
		SetModifiedAndInvalidateInfluencedCache(170, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _maxTrickCount.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 169, serializedSize);
			ptr += _maxTrickCount.Serialize(ptr);
		}
	}

	public SpecialEffectList GetMaxBreathPercent()
	{
		return _maxBreathPercent;
	}

	public unsafe void SetMaxBreathPercent(SpecialEffectList maxBreathPercent, DataContext context)
	{
		_maxBreathPercent = maxBreathPercent;
		SetModifiedAndInvalidateInfluencedCache(171, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _maxBreathPercent.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 170, serializedSize);
			ptr += _maxBreathPercent.Serialize(ptr);
		}
	}

	public SpecialEffectList GetMaxStancePercent()
	{
		return _maxStancePercent;
	}

	public unsafe void SetMaxStancePercent(SpecialEffectList maxStancePercent, DataContext context)
	{
		_maxStancePercent = maxStancePercent;
		SetModifiedAndInvalidateInfluencedCache(172, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _maxStancePercent.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 171, serializedSize);
			ptr += _maxStancePercent.Serialize(ptr);
		}
	}

	public SpecialEffectList GetExtraBreathPercent()
	{
		return _extraBreathPercent;
	}

	public unsafe void SetExtraBreathPercent(SpecialEffectList extraBreathPercent, DataContext context)
	{
		_extraBreathPercent = extraBreathPercent;
		SetModifiedAndInvalidateInfluencedCache(173, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _extraBreathPercent.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 172, serializedSize);
			ptr += _extraBreathPercent.Serialize(ptr);
		}
	}

	public SpecialEffectList GetExtraStancePercent()
	{
		return _extraStancePercent;
	}

	public unsafe void SetExtraStancePercent(SpecialEffectList extraStancePercent, DataContext context)
	{
		_extraStancePercent = extraStancePercent;
		SetModifiedAndInvalidateInfluencedCache(174, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _extraStancePercent.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 173, serializedSize);
			ptr += _extraStancePercent.Serialize(ptr);
		}
	}

	public SpecialEffectList GetMoveCostMobility()
	{
		return _moveCostMobility;
	}

	public unsafe void SetMoveCostMobility(SpecialEffectList moveCostMobility, DataContext context)
	{
		_moveCostMobility = moveCostMobility;
		SetModifiedAndInvalidateInfluencedCache(175, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _moveCostMobility.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 174, serializedSize);
			ptr += _moveCostMobility.Serialize(ptr);
		}
	}

	public SpecialEffectList GetDefendSkillKeepTime()
	{
		return _defendSkillKeepTime;
	}

	public unsafe void SetDefendSkillKeepTime(SpecialEffectList defendSkillKeepTime, DataContext context)
	{
		_defendSkillKeepTime = defendSkillKeepTime;
		SetModifiedAndInvalidateInfluencedCache(176, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _defendSkillKeepTime.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 175, serializedSize);
			ptr += _defendSkillKeepTime.Serialize(ptr);
		}
	}

	public SpecialEffectList GetBounceRange()
	{
		return _bounceRange;
	}

	public unsafe void SetBounceRange(SpecialEffectList bounceRange, DataContext context)
	{
		_bounceRange = bounceRange;
		SetModifiedAndInvalidateInfluencedCache(177, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _bounceRange.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 176, serializedSize);
			ptr += _bounceRange.Serialize(ptr);
		}
	}

	public SpecialEffectList GetMindMarkKeepTime()
	{
		return _mindMarkKeepTime;
	}

	public unsafe void SetMindMarkKeepTime(SpecialEffectList mindMarkKeepTime, DataContext context)
	{
		_mindMarkKeepTime = mindMarkKeepTime;
		SetModifiedAndInvalidateInfluencedCache(178, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _mindMarkKeepTime.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 177, serializedSize);
			ptr += _mindMarkKeepTime.Serialize(ptr);
		}
	}

	public SpecialEffectList GetSkillMobilityCostPerFrame()
	{
		return _skillMobilityCostPerFrame;
	}

	public unsafe void SetSkillMobilityCostPerFrame(SpecialEffectList skillMobilityCostPerFrame, DataContext context)
	{
		_skillMobilityCostPerFrame = skillMobilityCostPerFrame;
		SetModifiedAndInvalidateInfluencedCache(179, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _skillMobilityCostPerFrame.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 178, serializedSize);
			ptr += _skillMobilityCostPerFrame.Serialize(ptr);
		}
	}

	public SpecialEffectList GetCanAddWug()
	{
		return _canAddWug;
	}

	public unsafe void SetCanAddWug(SpecialEffectList canAddWug, DataContext context)
	{
		_canAddWug = canAddWug;
		SetModifiedAndInvalidateInfluencedCache(180, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _canAddWug.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 179, serializedSize);
			ptr += _canAddWug.Serialize(ptr);
		}
	}

	public SpecialEffectList GetHasGodWeaponBuff()
	{
		return _hasGodWeaponBuff;
	}

	public unsafe void SetHasGodWeaponBuff(SpecialEffectList hasGodWeaponBuff, DataContext context)
	{
		_hasGodWeaponBuff = hasGodWeaponBuff;
		SetModifiedAndInvalidateInfluencedCache(181, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _hasGodWeaponBuff.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 180, serializedSize);
			ptr += _hasGodWeaponBuff.Serialize(ptr);
		}
	}

	public SpecialEffectList GetHasGodArmorBuff()
	{
		return _hasGodArmorBuff;
	}

	public unsafe void SetHasGodArmorBuff(SpecialEffectList hasGodArmorBuff, DataContext context)
	{
		_hasGodArmorBuff = hasGodArmorBuff;
		SetModifiedAndInvalidateInfluencedCache(182, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _hasGodArmorBuff.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 181, serializedSize);
			ptr += _hasGodArmorBuff.Serialize(ptr);
		}
	}

	public SpecialEffectList GetTeammateCmdRequireGenerateValue()
	{
		return _teammateCmdRequireGenerateValue;
	}

	public unsafe void SetTeammateCmdRequireGenerateValue(SpecialEffectList teammateCmdRequireGenerateValue, DataContext context)
	{
		_teammateCmdRequireGenerateValue = teammateCmdRequireGenerateValue;
		SetModifiedAndInvalidateInfluencedCache(183, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _teammateCmdRequireGenerateValue.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 182, serializedSize);
			ptr += _teammateCmdRequireGenerateValue.Serialize(ptr);
		}
	}

	public SpecialEffectList GetTeammateCmdEffect()
	{
		return _teammateCmdEffect;
	}

	public unsafe void SetTeammateCmdEffect(SpecialEffectList teammateCmdEffect, DataContext context)
	{
		_teammateCmdEffect = teammateCmdEffect;
		SetModifiedAndInvalidateInfluencedCache(184, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _teammateCmdEffect.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 183, serializedSize);
			ptr += _teammateCmdEffect.Serialize(ptr);
		}
	}

	public SpecialEffectList GetFlawRecoverSpeed()
	{
		return _flawRecoverSpeed;
	}

	public unsafe void SetFlawRecoverSpeed(SpecialEffectList flawRecoverSpeed, DataContext context)
	{
		_flawRecoverSpeed = flawRecoverSpeed;
		SetModifiedAndInvalidateInfluencedCache(185, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _flawRecoverSpeed.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 184, serializedSize);
			ptr += _flawRecoverSpeed.Serialize(ptr);
		}
	}

	public SpecialEffectList GetAcupointRecoverSpeed()
	{
		return _acupointRecoverSpeed;
	}

	public unsafe void SetAcupointRecoverSpeed(SpecialEffectList acupointRecoverSpeed, DataContext context)
	{
		_acupointRecoverSpeed = acupointRecoverSpeed;
		SetModifiedAndInvalidateInfluencedCache(186, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _acupointRecoverSpeed.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 185, serializedSize);
			ptr += _acupointRecoverSpeed.Serialize(ptr);
		}
	}

	public SpecialEffectList GetMindMarkRecoverSpeed()
	{
		return _mindMarkRecoverSpeed;
	}

	public unsafe void SetMindMarkRecoverSpeed(SpecialEffectList mindMarkRecoverSpeed, DataContext context)
	{
		_mindMarkRecoverSpeed = mindMarkRecoverSpeed;
		SetModifiedAndInvalidateInfluencedCache(187, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _mindMarkRecoverSpeed.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 186, serializedSize);
			ptr += _mindMarkRecoverSpeed.Serialize(ptr);
		}
	}

	public SpecialEffectList GetInjuryAutoHealSpeed()
	{
		return _injuryAutoHealSpeed;
	}

	public unsafe void SetInjuryAutoHealSpeed(SpecialEffectList injuryAutoHealSpeed, DataContext context)
	{
		_injuryAutoHealSpeed = injuryAutoHealSpeed;
		SetModifiedAndInvalidateInfluencedCache(188, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _injuryAutoHealSpeed.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 187, serializedSize);
			ptr += _injuryAutoHealSpeed.Serialize(ptr);
		}
	}

	public SpecialEffectList GetCanRecoverBreath()
	{
		return _canRecoverBreath;
	}

	public unsafe void SetCanRecoverBreath(SpecialEffectList canRecoverBreath, DataContext context)
	{
		_canRecoverBreath = canRecoverBreath;
		SetModifiedAndInvalidateInfluencedCache(189, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _canRecoverBreath.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 188, serializedSize);
			ptr += _canRecoverBreath.Serialize(ptr);
		}
	}

	public SpecialEffectList GetCanRecoverStance()
	{
		return _canRecoverStance;
	}

	public unsafe void SetCanRecoverStance(SpecialEffectList canRecoverStance, DataContext context)
	{
		_canRecoverStance = canRecoverStance;
		SetModifiedAndInvalidateInfluencedCache(190, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _canRecoverStance.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 189, serializedSize);
			ptr += _canRecoverStance.Serialize(ptr);
		}
	}

	public SpecialEffectList GetFatalDamageValue()
	{
		return _fatalDamageValue;
	}

	public unsafe void SetFatalDamageValue(SpecialEffectList fatalDamageValue, DataContext context)
	{
		_fatalDamageValue = fatalDamageValue;
		SetModifiedAndInvalidateInfluencedCache(191, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _fatalDamageValue.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 190, serializedSize);
			ptr += _fatalDamageValue.Serialize(ptr);
		}
	}

	public SpecialEffectList GetFatalDamageMarkCount()
	{
		return _fatalDamageMarkCount;
	}

	public unsafe void SetFatalDamageMarkCount(SpecialEffectList fatalDamageMarkCount, DataContext context)
	{
		_fatalDamageMarkCount = fatalDamageMarkCount;
		SetModifiedAndInvalidateInfluencedCache(192, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _fatalDamageMarkCount.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 191, serializedSize);
			ptr += _fatalDamageMarkCount.Serialize(ptr);
		}
	}

	public SpecialEffectList GetCanFightBackDuringPrepareSkill()
	{
		return _canFightBackDuringPrepareSkill;
	}

	public unsafe void SetCanFightBackDuringPrepareSkill(SpecialEffectList canFightBackDuringPrepareSkill, DataContext context)
	{
		_canFightBackDuringPrepareSkill = canFightBackDuringPrepareSkill;
		SetModifiedAndInvalidateInfluencedCache(193, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _canFightBackDuringPrepareSkill.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 192, serializedSize);
			ptr += _canFightBackDuringPrepareSkill.Serialize(ptr);
		}
	}

	public SpecialEffectList GetSkillPrepareSpeed()
	{
		return _skillPrepareSpeed;
	}

	public unsafe void SetSkillPrepareSpeed(SpecialEffectList skillPrepareSpeed, DataContext context)
	{
		_skillPrepareSpeed = skillPrepareSpeed;
		SetModifiedAndInvalidateInfluencedCache(194, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _skillPrepareSpeed.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 193, serializedSize);
			ptr += _skillPrepareSpeed.Serialize(ptr);
		}
	}

	public SpecialEffectList GetBreathRecoverSpeed()
	{
		return _breathRecoverSpeed;
	}

	public unsafe void SetBreathRecoverSpeed(SpecialEffectList breathRecoverSpeed, DataContext context)
	{
		_breathRecoverSpeed = breathRecoverSpeed;
		SetModifiedAndInvalidateInfluencedCache(195, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _breathRecoverSpeed.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 194, serializedSize);
			ptr += _breathRecoverSpeed.Serialize(ptr);
		}
	}

	public SpecialEffectList GetStanceRecoverSpeed()
	{
		return _stanceRecoverSpeed;
	}

	public unsafe void SetStanceRecoverSpeed(SpecialEffectList stanceRecoverSpeed, DataContext context)
	{
		_stanceRecoverSpeed = stanceRecoverSpeed;
		SetModifiedAndInvalidateInfluencedCache(196, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _stanceRecoverSpeed.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 195, serializedSize);
			ptr += _stanceRecoverSpeed.Serialize(ptr);
		}
	}

	public SpecialEffectList GetMobilityRecoverSpeed()
	{
		return _mobilityRecoverSpeed;
	}

	public unsafe void SetMobilityRecoverSpeed(SpecialEffectList mobilityRecoverSpeed, DataContext context)
	{
		_mobilityRecoverSpeed = mobilityRecoverSpeed;
		SetModifiedAndInvalidateInfluencedCache(197, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _mobilityRecoverSpeed.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 196, serializedSize);
			ptr += _mobilityRecoverSpeed.Serialize(ptr);
		}
	}

	public SpecialEffectList GetChangeTrickProgressAddValue()
	{
		return _changeTrickProgressAddValue;
	}

	public unsafe void SetChangeTrickProgressAddValue(SpecialEffectList changeTrickProgressAddValue, DataContext context)
	{
		_changeTrickProgressAddValue = changeTrickProgressAddValue;
		SetModifiedAndInvalidateInfluencedCache(198, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _changeTrickProgressAddValue.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 197, serializedSize);
			ptr += _changeTrickProgressAddValue.Serialize(ptr);
		}
	}

	public SpecialEffectList GetPower()
	{
		return _power;
	}

	public unsafe void SetPower(SpecialEffectList power, DataContext context)
	{
		_power = power;
		SetModifiedAndInvalidateInfluencedCache(199, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _power.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 198, serializedSize);
			ptr += _power.Serialize(ptr);
		}
	}

	public SpecialEffectList GetMaxPower()
	{
		return _maxPower;
	}

	public unsafe void SetMaxPower(SpecialEffectList maxPower, DataContext context)
	{
		_maxPower = maxPower;
		SetModifiedAndInvalidateInfluencedCache(200, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _maxPower.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 199, serializedSize);
			ptr += _maxPower.Serialize(ptr);
		}
	}

	public SpecialEffectList GetPowerCanReduce()
	{
		return _powerCanReduce;
	}

	public unsafe void SetPowerCanReduce(SpecialEffectList powerCanReduce, DataContext context)
	{
		_powerCanReduce = powerCanReduce;
		SetModifiedAndInvalidateInfluencedCache(201, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _powerCanReduce.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 200, serializedSize);
			ptr += _powerCanReduce.Serialize(ptr);
		}
	}

	public SpecialEffectList GetUseRequirement()
	{
		return _useRequirement;
	}

	public unsafe void SetUseRequirement(SpecialEffectList useRequirement, DataContext context)
	{
		_useRequirement = useRequirement;
		SetModifiedAndInvalidateInfluencedCache(202, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _useRequirement.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 201, serializedSize);
			ptr += _useRequirement.Serialize(ptr);
		}
	}

	public SpecialEffectList GetCurrInnerRatio()
	{
		return _currInnerRatio;
	}

	public unsafe void SetCurrInnerRatio(SpecialEffectList currInnerRatio, DataContext context)
	{
		_currInnerRatio = currInnerRatio;
		SetModifiedAndInvalidateInfluencedCache(203, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _currInnerRatio.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 202, serializedSize);
			ptr += _currInnerRatio.Serialize(ptr);
		}
	}

	public SpecialEffectList GetCostBreathAndStance()
	{
		return _costBreathAndStance;
	}

	public unsafe void SetCostBreathAndStance(SpecialEffectList costBreathAndStance, DataContext context)
	{
		_costBreathAndStance = costBreathAndStance;
		SetModifiedAndInvalidateInfluencedCache(204, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _costBreathAndStance.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 203, serializedSize);
			ptr += _costBreathAndStance.Serialize(ptr);
		}
	}

	public SpecialEffectList GetCostBreath()
	{
		return _costBreath;
	}

	public unsafe void SetCostBreath(SpecialEffectList costBreath, DataContext context)
	{
		_costBreath = costBreath;
		SetModifiedAndInvalidateInfluencedCache(205, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _costBreath.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 204, serializedSize);
			ptr += _costBreath.Serialize(ptr);
		}
	}

	public SpecialEffectList GetCostStance()
	{
		return _costStance;
	}

	public unsafe void SetCostStance(SpecialEffectList costStance, DataContext context)
	{
		_costStance = costStance;
		SetModifiedAndInvalidateInfluencedCache(206, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _costStance.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 205, serializedSize);
			ptr += _costStance.Serialize(ptr);
		}
	}

	public SpecialEffectList GetCostMobility()
	{
		return _costMobility;
	}

	public unsafe void SetCostMobility(SpecialEffectList costMobility, DataContext context)
	{
		_costMobility = costMobility;
		SetModifiedAndInvalidateInfluencedCache(207, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _costMobility.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 206, serializedSize);
			ptr += _costMobility.Serialize(ptr);
		}
	}

	public SpecialEffectList GetSkillCostTricks()
	{
		return _skillCostTricks;
	}

	public unsafe void SetSkillCostTricks(SpecialEffectList skillCostTricks, DataContext context)
	{
		_skillCostTricks = skillCostTricks;
		SetModifiedAndInvalidateInfluencedCache(208, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _skillCostTricks.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 207, serializedSize);
			ptr += _skillCostTricks.Serialize(ptr);
		}
	}

	public SpecialEffectList GetEffectDirection()
	{
		return _effectDirection;
	}

	public unsafe void SetEffectDirection(SpecialEffectList effectDirection, DataContext context)
	{
		_effectDirection = effectDirection;
		SetModifiedAndInvalidateInfluencedCache(209, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _effectDirection.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 208, serializedSize);
			ptr += _effectDirection.Serialize(ptr);
		}
	}

	public SpecialEffectList GetEffectDirectionCanChange()
	{
		return _effectDirectionCanChange;
	}

	public unsafe void SetEffectDirectionCanChange(SpecialEffectList effectDirectionCanChange, DataContext context)
	{
		_effectDirectionCanChange = effectDirectionCanChange;
		SetModifiedAndInvalidateInfluencedCache(210, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _effectDirectionCanChange.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 209, serializedSize);
			ptr += _effectDirectionCanChange.Serialize(ptr);
		}
	}

	public SpecialEffectList GetGridCost()
	{
		return _gridCost;
	}

	public unsafe void SetGridCost(SpecialEffectList gridCost, DataContext context)
	{
		_gridCost = gridCost;
		SetModifiedAndInvalidateInfluencedCache(211, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _gridCost.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 210, serializedSize);
			ptr += _gridCost.Serialize(ptr);
		}
	}

	public SpecialEffectList GetPrepareTotalProgress()
	{
		return _prepareTotalProgress;
	}

	public unsafe void SetPrepareTotalProgress(SpecialEffectList prepareTotalProgress, DataContext context)
	{
		_prepareTotalProgress = prepareTotalProgress;
		SetModifiedAndInvalidateInfluencedCache(212, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _prepareTotalProgress.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 211, serializedSize);
			ptr += _prepareTotalProgress.Serialize(ptr);
		}
	}

	public SpecialEffectList GetSpecificGridCount()
	{
		return _specificGridCount;
	}

	public unsafe void SetSpecificGridCount(SpecialEffectList specificGridCount, DataContext context)
	{
		_specificGridCount = specificGridCount;
		SetModifiedAndInvalidateInfluencedCache(213, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _specificGridCount.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 212, serializedSize);
			ptr += _specificGridCount.Serialize(ptr);
		}
	}

	public SpecialEffectList GetGenericGridCount()
	{
		return _genericGridCount;
	}

	public unsafe void SetGenericGridCount(SpecialEffectList genericGridCount, DataContext context)
	{
		_genericGridCount = genericGridCount;
		SetModifiedAndInvalidateInfluencedCache(214, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _genericGridCount.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 213, serializedSize);
			ptr += _genericGridCount.Serialize(ptr);
		}
	}

	public SpecialEffectList GetCanInterrupt()
	{
		return _canInterrupt;
	}

	public unsafe void SetCanInterrupt(SpecialEffectList canInterrupt, DataContext context)
	{
		_canInterrupt = canInterrupt;
		SetModifiedAndInvalidateInfluencedCache(215, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _canInterrupt.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 214, serializedSize);
			ptr += _canInterrupt.Serialize(ptr);
		}
	}

	public SpecialEffectList GetInterruptOdds()
	{
		return _interruptOdds;
	}

	public unsafe void SetInterruptOdds(SpecialEffectList interruptOdds, DataContext context)
	{
		_interruptOdds = interruptOdds;
		SetModifiedAndInvalidateInfluencedCache(216, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _interruptOdds.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 215, serializedSize);
			ptr += _interruptOdds.Serialize(ptr);
		}
	}

	public SpecialEffectList GetCanSilence()
	{
		return _canSilence;
	}

	public unsafe void SetCanSilence(SpecialEffectList canSilence, DataContext context)
	{
		_canSilence = canSilence;
		SetModifiedAndInvalidateInfluencedCache(217, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _canSilence.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 216, serializedSize);
			ptr += _canSilence.Serialize(ptr);
		}
	}

	public SpecialEffectList GetSilenceOdds()
	{
		return _silenceOdds;
	}

	public unsafe void SetSilenceOdds(SpecialEffectList silenceOdds, DataContext context)
	{
		_silenceOdds = silenceOdds;
		SetModifiedAndInvalidateInfluencedCache(218, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _silenceOdds.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 217, serializedSize);
			ptr += _silenceOdds.Serialize(ptr);
		}
	}

	public SpecialEffectList GetCanCastWithBrokenBodyPart()
	{
		return _canCastWithBrokenBodyPart;
	}

	public unsafe void SetCanCastWithBrokenBodyPart(SpecialEffectList canCastWithBrokenBodyPart, DataContext context)
	{
		_canCastWithBrokenBodyPart = canCastWithBrokenBodyPart;
		SetModifiedAndInvalidateInfluencedCache(219, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _canCastWithBrokenBodyPart.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 218, serializedSize);
			ptr += _canCastWithBrokenBodyPart.Serialize(ptr);
		}
	}

	public SpecialEffectList GetAddPowerCanBeRemoved()
	{
		return _addPowerCanBeRemoved;
	}

	public unsafe void SetAddPowerCanBeRemoved(SpecialEffectList addPowerCanBeRemoved, DataContext context)
	{
		_addPowerCanBeRemoved = addPowerCanBeRemoved;
		SetModifiedAndInvalidateInfluencedCache(220, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _addPowerCanBeRemoved.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 219, serializedSize);
			ptr += _addPowerCanBeRemoved.Serialize(ptr);
		}
	}

	public SpecialEffectList GetSkillType()
	{
		return _skillType;
	}

	public unsafe void SetSkillType(SpecialEffectList skillType, DataContext context)
	{
		_skillType = skillType;
		SetModifiedAndInvalidateInfluencedCache(221, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _skillType.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 220, serializedSize);
			ptr += _skillType.Serialize(ptr);
		}
	}

	public SpecialEffectList GetEffectCountCanChange()
	{
		return _effectCountCanChange;
	}

	public unsafe void SetEffectCountCanChange(SpecialEffectList effectCountCanChange, DataContext context)
	{
		_effectCountCanChange = effectCountCanChange;
		SetModifiedAndInvalidateInfluencedCache(222, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _effectCountCanChange.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 221, serializedSize);
			ptr += _effectCountCanChange.Serialize(ptr);
		}
	}

	public SpecialEffectList GetCanCastInDefend()
	{
		return _canCastInDefend;
	}

	public unsafe void SetCanCastInDefend(SpecialEffectList canCastInDefend, DataContext context)
	{
		_canCastInDefend = canCastInDefend;
		SetModifiedAndInvalidateInfluencedCache(223, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _canCastInDefend.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 222, serializedSize);
			ptr += _canCastInDefend.Serialize(ptr);
		}
	}

	public SpecialEffectList GetHitDistribution()
	{
		return _hitDistribution;
	}

	public unsafe void SetHitDistribution(SpecialEffectList hitDistribution, DataContext context)
	{
		_hitDistribution = hitDistribution;
		SetModifiedAndInvalidateInfluencedCache(224, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _hitDistribution.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 223, serializedSize);
			ptr += _hitDistribution.Serialize(ptr);
		}
	}

	public SpecialEffectList GetCanCastOnLackBreath()
	{
		return _canCastOnLackBreath;
	}

	public unsafe void SetCanCastOnLackBreath(SpecialEffectList canCastOnLackBreath, DataContext context)
	{
		_canCastOnLackBreath = canCastOnLackBreath;
		SetModifiedAndInvalidateInfluencedCache(225, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _canCastOnLackBreath.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 224, serializedSize);
			ptr += _canCastOnLackBreath.Serialize(ptr);
		}
	}

	public SpecialEffectList GetCanCastOnLackStance()
	{
		return _canCastOnLackStance;
	}

	public unsafe void SetCanCastOnLackStance(SpecialEffectList canCastOnLackStance, DataContext context)
	{
		_canCastOnLackStance = canCastOnLackStance;
		SetModifiedAndInvalidateInfluencedCache(226, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _canCastOnLackStance.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 225, serializedSize);
			ptr += _canCastOnLackStance.Serialize(ptr);
		}
	}

	public SpecialEffectList GetCostBreathOnCast()
	{
		return _costBreathOnCast;
	}

	public unsafe void SetCostBreathOnCast(SpecialEffectList costBreathOnCast, DataContext context)
	{
		_costBreathOnCast = costBreathOnCast;
		SetModifiedAndInvalidateInfluencedCache(227, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _costBreathOnCast.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 226, serializedSize);
			ptr += _costBreathOnCast.Serialize(ptr);
		}
	}

	public SpecialEffectList GetCostStanceOnCast()
	{
		return _costStanceOnCast;
	}

	public unsafe void SetCostStanceOnCast(SpecialEffectList costStanceOnCast, DataContext context)
	{
		_costStanceOnCast = costStanceOnCast;
		SetModifiedAndInvalidateInfluencedCache(228, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _costStanceOnCast.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 227, serializedSize);
			ptr += _costStanceOnCast.Serialize(ptr);
		}
	}

	public SpecialEffectList GetCanUseMobilityAsBreath()
	{
		return _canUseMobilityAsBreath;
	}

	public unsafe void SetCanUseMobilityAsBreath(SpecialEffectList canUseMobilityAsBreath, DataContext context)
	{
		_canUseMobilityAsBreath = canUseMobilityAsBreath;
		SetModifiedAndInvalidateInfluencedCache(229, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _canUseMobilityAsBreath.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 228, serializedSize);
			ptr += _canUseMobilityAsBreath.Serialize(ptr);
		}
	}

	public SpecialEffectList GetCanUseMobilityAsStance()
	{
		return _canUseMobilityAsStance;
	}

	public unsafe void SetCanUseMobilityAsStance(SpecialEffectList canUseMobilityAsStance, DataContext context)
	{
		_canUseMobilityAsStance = canUseMobilityAsStance;
		SetModifiedAndInvalidateInfluencedCache(230, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _canUseMobilityAsStance.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 229, serializedSize);
			ptr += _canUseMobilityAsStance.Serialize(ptr);
		}
	}

	public SpecialEffectList GetCastCostNeiliAllocation()
	{
		return _castCostNeiliAllocation;
	}

	public unsafe void SetCastCostNeiliAllocation(SpecialEffectList castCostNeiliAllocation, DataContext context)
	{
		_castCostNeiliAllocation = castCostNeiliAllocation;
		SetModifiedAndInvalidateInfluencedCache(231, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _castCostNeiliAllocation.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 230, serializedSize);
			ptr += _castCostNeiliAllocation.Serialize(ptr);
		}
	}

	public SpecialEffectList GetAcceptPoisonResist()
	{
		return _acceptPoisonResist;
	}

	public unsafe void SetAcceptPoisonResist(SpecialEffectList acceptPoisonResist, DataContext context)
	{
		_acceptPoisonResist = acceptPoisonResist;
		SetModifiedAndInvalidateInfluencedCache(232, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _acceptPoisonResist.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 231, serializedSize);
			ptr += _acceptPoisonResist.Serialize(ptr);
		}
	}

	public SpecialEffectList GetMakePoisonResist()
	{
		return _makePoisonResist;
	}

	public unsafe void SetMakePoisonResist(SpecialEffectList makePoisonResist, DataContext context)
	{
		_makePoisonResist = makePoisonResist;
		SetModifiedAndInvalidateInfluencedCache(233, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _makePoisonResist.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 232, serializedSize);
			ptr += _makePoisonResist.Serialize(ptr);
		}
	}

	public SpecialEffectList GetCanCriticalHit()
	{
		return _canCriticalHit;
	}

	public unsafe void SetCanCriticalHit(SpecialEffectList canCriticalHit, DataContext context)
	{
		_canCriticalHit = canCriticalHit;
		SetModifiedAndInvalidateInfluencedCache(234, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _canCriticalHit.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 233, serializedSize);
			ptr += _canCriticalHit.Serialize(ptr);
		}
	}

	public SpecialEffectList GetCanCostNeiliAllocationEffect()
	{
		return _canCostNeiliAllocationEffect;
	}

	public unsafe void SetCanCostNeiliAllocationEffect(SpecialEffectList canCostNeiliAllocationEffect, DataContext context)
	{
		_canCostNeiliAllocationEffect = canCostNeiliAllocationEffect;
		SetModifiedAndInvalidateInfluencedCache(235, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _canCostNeiliAllocationEffect.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 234, serializedSize);
			ptr += _canCostNeiliAllocationEffect.Serialize(ptr);
		}
	}

	public SpecialEffectList GetConsummateLevelRelatedMainAttributesHitValues()
	{
		return _consummateLevelRelatedMainAttributesHitValues;
	}

	public unsafe void SetConsummateLevelRelatedMainAttributesHitValues(SpecialEffectList consummateLevelRelatedMainAttributesHitValues, DataContext context)
	{
		_consummateLevelRelatedMainAttributesHitValues = consummateLevelRelatedMainAttributesHitValues;
		SetModifiedAndInvalidateInfluencedCache(236, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _consummateLevelRelatedMainAttributesHitValues.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 235, serializedSize);
			ptr += _consummateLevelRelatedMainAttributesHitValues.Serialize(ptr);
		}
	}

	public SpecialEffectList GetConsummateLevelRelatedMainAttributesAvoidValues()
	{
		return _consummateLevelRelatedMainAttributesAvoidValues;
	}

	public unsafe void SetConsummateLevelRelatedMainAttributesAvoidValues(SpecialEffectList consummateLevelRelatedMainAttributesAvoidValues, DataContext context)
	{
		_consummateLevelRelatedMainAttributesAvoidValues = consummateLevelRelatedMainAttributesAvoidValues;
		SetModifiedAndInvalidateInfluencedCache(237, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _consummateLevelRelatedMainAttributesAvoidValues.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 236, serializedSize);
			ptr += _consummateLevelRelatedMainAttributesAvoidValues.Serialize(ptr);
		}
	}

	public SpecialEffectList GetConsummateLevelRelatedMainAttributesPenetrations()
	{
		return _consummateLevelRelatedMainAttributesPenetrations;
	}

	public unsafe void SetConsummateLevelRelatedMainAttributesPenetrations(SpecialEffectList consummateLevelRelatedMainAttributesPenetrations, DataContext context)
	{
		_consummateLevelRelatedMainAttributesPenetrations = consummateLevelRelatedMainAttributesPenetrations;
		SetModifiedAndInvalidateInfluencedCache(238, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _consummateLevelRelatedMainAttributesPenetrations.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 237, serializedSize);
			ptr += _consummateLevelRelatedMainAttributesPenetrations.Serialize(ptr);
		}
	}

	public SpecialEffectList GetConsummateLevelRelatedMainAttributesPenetrationResists()
	{
		return _consummateLevelRelatedMainAttributesPenetrationResists;
	}

	public unsafe void SetConsummateLevelRelatedMainAttributesPenetrationResists(SpecialEffectList consummateLevelRelatedMainAttributesPenetrationResists, DataContext context)
	{
		_consummateLevelRelatedMainAttributesPenetrationResists = consummateLevelRelatedMainAttributesPenetrationResists;
		SetModifiedAndInvalidateInfluencedCache(239, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _consummateLevelRelatedMainAttributesPenetrationResists.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 238, serializedSize);
			ptr += _consummateLevelRelatedMainAttributesPenetrationResists.Serialize(ptr);
		}
	}

	public SpecialEffectList GetSkillAlsoAsFiveElements()
	{
		return _skillAlsoAsFiveElements;
	}

	public unsafe void SetSkillAlsoAsFiveElements(SpecialEffectList skillAlsoAsFiveElements, DataContext context)
	{
		_skillAlsoAsFiveElements = skillAlsoAsFiveElements;
		SetModifiedAndInvalidateInfluencedCache(240, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _skillAlsoAsFiveElements.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 239, serializedSize);
			ptr += _skillAlsoAsFiveElements.Serialize(ptr);
		}
	}

	public SpecialEffectList GetInnerInjuryImmunity()
	{
		return _innerInjuryImmunity;
	}

	public unsafe void SetInnerInjuryImmunity(SpecialEffectList innerInjuryImmunity, DataContext context)
	{
		_innerInjuryImmunity = innerInjuryImmunity;
		SetModifiedAndInvalidateInfluencedCache(241, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _innerInjuryImmunity.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 240, serializedSize);
			ptr += _innerInjuryImmunity.Serialize(ptr);
		}
	}

	public SpecialEffectList GetOuterInjuryImmunity()
	{
		return _outerInjuryImmunity;
	}

	public unsafe void SetOuterInjuryImmunity(SpecialEffectList outerInjuryImmunity, DataContext context)
	{
		_outerInjuryImmunity = outerInjuryImmunity;
		SetModifiedAndInvalidateInfluencedCache(242, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _outerInjuryImmunity.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 241, serializedSize);
			ptr += _outerInjuryImmunity.Serialize(ptr);
		}
	}

	public SpecialEffectList GetPoisonAffectThreshold()
	{
		return _poisonAffectThreshold;
	}

	public unsafe void SetPoisonAffectThreshold(SpecialEffectList poisonAffectThreshold, DataContext context)
	{
		_poisonAffectThreshold = poisonAffectThreshold;
		SetModifiedAndInvalidateInfluencedCache(243, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _poisonAffectThreshold.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 242, serializedSize);
			ptr += _poisonAffectThreshold.Serialize(ptr);
		}
	}

	public SpecialEffectList GetLockDistance()
	{
		return _lockDistance;
	}

	public unsafe void SetLockDistance(SpecialEffectList lockDistance, DataContext context)
	{
		_lockDistance = lockDistance;
		SetModifiedAndInvalidateInfluencedCache(244, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _lockDistance.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 243, serializedSize);
			ptr += _lockDistance.Serialize(ptr);
		}
	}

	public SpecialEffectList GetResistOfAllPoison()
	{
		return _resistOfAllPoison;
	}

	public unsafe void SetResistOfAllPoison(SpecialEffectList resistOfAllPoison, DataContext context)
	{
		_resistOfAllPoison = resistOfAllPoison;
		SetModifiedAndInvalidateInfluencedCache(245, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _resistOfAllPoison.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 244, serializedSize);
			ptr += _resistOfAllPoison.Serialize(ptr);
		}
	}

	public SpecialEffectList GetMakePoisonTarget()
	{
		return _makePoisonTarget;
	}

	public unsafe void SetMakePoisonTarget(SpecialEffectList makePoisonTarget, DataContext context)
	{
		_makePoisonTarget = makePoisonTarget;
		SetModifiedAndInvalidateInfluencedCache(246, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _makePoisonTarget.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 245, serializedSize);
			ptr += _makePoisonTarget.Serialize(ptr);
		}
	}

	public SpecialEffectList GetAcceptPoisonTarget()
	{
		return _acceptPoisonTarget;
	}

	public unsafe void SetAcceptPoisonTarget(SpecialEffectList acceptPoisonTarget, DataContext context)
	{
		_acceptPoisonTarget = acceptPoisonTarget;
		SetModifiedAndInvalidateInfluencedCache(247, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _acceptPoisonTarget.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 246, serializedSize);
			ptr += _acceptPoisonTarget.Serialize(ptr);
		}
	}

	public SpecialEffectList GetCertainCriticalHit()
	{
		return _certainCriticalHit;
	}

	public unsafe void SetCertainCriticalHit(SpecialEffectList certainCriticalHit, DataContext context)
	{
		_certainCriticalHit = certainCriticalHit;
		SetModifiedAndInvalidateInfluencedCache(248, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _certainCriticalHit.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 247, serializedSize);
			ptr += _certainCriticalHit.Serialize(ptr);
		}
	}

	public SpecialEffectList GetMindMarkCount()
	{
		return _mindMarkCount;
	}

	public unsafe void SetMindMarkCount(SpecialEffectList mindMarkCount, DataContext context)
	{
		_mindMarkCount = mindMarkCount;
		SetModifiedAndInvalidateInfluencedCache(249, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _mindMarkCount.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 248, serializedSize);
			ptr += _mindMarkCount.Serialize(ptr);
		}
	}

	public SpecialEffectList GetCanFightBackWithHit()
	{
		return _canFightBackWithHit;
	}

	public unsafe void SetCanFightBackWithHit(SpecialEffectList canFightBackWithHit, DataContext context)
	{
		_canFightBackWithHit = canFightBackWithHit;
		SetModifiedAndInvalidateInfluencedCache(250, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _canFightBackWithHit.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 249, serializedSize);
			ptr += _canFightBackWithHit.Serialize(ptr);
		}
	}

	public SpecialEffectList GetInevitableHit()
	{
		return _inevitableHit;
	}

	public unsafe void SetInevitableHit(SpecialEffectList inevitableHit, DataContext context)
	{
		_inevitableHit = inevitableHit;
		SetModifiedAndInvalidateInfluencedCache(251, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _inevitableHit.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 250, serializedSize);
			ptr += _inevitableHit.Serialize(ptr);
		}
	}

	public SpecialEffectList GetAttackCanPursue()
	{
		return _attackCanPursue;
	}

	public unsafe void SetAttackCanPursue(SpecialEffectList attackCanPursue, DataContext context)
	{
		_attackCanPursue = attackCanPursue;
		SetModifiedAndInvalidateInfluencedCache(252, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _attackCanPursue.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 251, serializedSize);
			ptr += _attackCanPursue.Serialize(ptr);
		}
	}

	public SpecialEffectList GetCombatSkillDataEffectList()
	{
		return _combatSkillDataEffectList;
	}

	public unsafe void SetCombatSkillDataEffectList(SpecialEffectList combatSkillDataEffectList, DataContext context)
	{
		_combatSkillDataEffectList = combatSkillDataEffectList;
		SetModifiedAndInvalidateInfluencedCache(253, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _combatSkillDataEffectList.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 252, serializedSize);
			ptr += _combatSkillDataEffectList.Serialize(ptr);
		}
	}

	public SpecialEffectList GetCriticalOdds()
	{
		return _criticalOdds;
	}

	public unsafe void SetCriticalOdds(SpecialEffectList criticalOdds, DataContext context)
	{
		_criticalOdds = criticalOdds;
		SetModifiedAndInvalidateInfluencedCache(254, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _criticalOdds.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 253, serializedSize);
			ptr += _criticalOdds.Serialize(ptr);
		}
	}

	public SpecialEffectList GetStanceCostByEffect()
	{
		return _stanceCostByEffect;
	}

	public unsafe void SetStanceCostByEffect(SpecialEffectList stanceCostByEffect, DataContext context)
	{
		_stanceCostByEffect = stanceCostByEffect;
		SetModifiedAndInvalidateInfluencedCache(255, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _stanceCostByEffect.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 254, serializedSize);
			ptr += _stanceCostByEffect.Serialize(ptr);
		}
	}

	public SpecialEffectList GetBreathCostByEffect()
	{
		return _breathCostByEffect;
	}

	public unsafe void SetBreathCostByEffect(SpecialEffectList breathCostByEffect, DataContext context)
	{
		_breathCostByEffect = breathCostByEffect;
		SetModifiedAndInvalidateInfluencedCache(256, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _breathCostByEffect.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 255, serializedSize);
			ptr += _breathCostByEffect.Serialize(ptr);
		}
	}

	public SpecialEffectList GetPowerAddRatio()
	{
		return _powerAddRatio;
	}

	public unsafe void SetPowerAddRatio(SpecialEffectList powerAddRatio, DataContext context)
	{
		_powerAddRatio = powerAddRatio;
		SetModifiedAndInvalidateInfluencedCache(257, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _powerAddRatio.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 256, serializedSize);
			ptr += _powerAddRatio.Serialize(ptr);
		}
	}

	public SpecialEffectList GetPowerReduceRatio()
	{
		return _powerReduceRatio;
	}

	public unsafe void SetPowerReduceRatio(SpecialEffectList powerReduceRatio, DataContext context)
	{
		_powerReduceRatio = powerReduceRatio;
		SetModifiedAndInvalidateInfluencedCache(258, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _powerReduceRatio.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 257, serializedSize);
			ptr += _powerReduceRatio.Serialize(ptr);
		}
	}

	public SpecialEffectList GetPoisonAffectProduceValue()
	{
		return _poisonAffectProduceValue;
	}

	public unsafe void SetPoisonAffectProduceValue(SpecialEffectList poisonAffectProduceValue, DataContext context)
	{
		_poisonAffectProduceValue = poisonAffectProduceValue;
		SetModifiedAndInvalidateInfluencedCache(259, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _poisonAffectProduceValue.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 258, serializedSize);
			ptr += _poisonAffectProduceValue.Serialize(ptr);
		}
	}

	public SpecialEffectList GetCanReadingOnMonthChange()
	{
		return _canReadingOnMonthChange;
	}

	public unsafe void SetCanReadingOnMonthChange(SpecialEffectList canReadingOnMonthChange, DataContext context)
	{
		_canReadingOnMonthChange = canReadingOnMonthChange;
		SetModifiedAndInvalidateInfluencedCache(260, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _canReadingOnMonthChange.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 259, serializedSize);
			ptr += _canReadingOnMonthChange.Serialize(ptr);
		}
	}

	public SpecialEffectList GetMedicineEffect()
	{
		return _medicineEffect;
	}

	public unsafe void SetMedicineEffect(SpecialEffectList medicineEffect, DataContext context)
	{
		_medicineEffect = medicineEffect;
		SetModifiedAndInvalidateInfluencedCache(261, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _medicineEffect.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 260, serializedSize);
			ptr += _medicineEffect.Serialize(ptr);
		}
	}

	public SpecialEffectList GetXiangshuInfectionDelta()
	{
		return _xiangshuInfectionDelta;
	}

	public unsafe void SetXiangshuInfectionDelta(SpecialEffectList xiangshuInfectionDelta, DataContext context)
	{
		_xiangshuInfectionDelta = xiangshuInfectionDelta;
		SetModifiedAndInvalidateInfluencedCache(262, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _xiangshuInfectionDelta.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 261, serializedSize);
			ptr += _xiangshuInfectionDelta.Serialize(ptr);
		}
	}

	public SpecialEffectList GetHealthDelta()
	{
		return _healthDelta;
	}

	public unsafe void SetHealthDelta(SpecialEffectList healthDelta, DataContext context)
	{
		_healthDelta = healthDelta;
		SetModifiedAndInvalidateInfluencedCache(263, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _healthDelta.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 262, serializedSize);
			ptr += _healthDelta.Serialize(ptr);
		}
	}

	public SpecialEffectList GetWeaponSilenceFrame()
	{
		return _weaponSilenceFrame;
	}

	public unsafe void SetWeaponSilenceFrame(SpecialEffectList weaponSilenceFrame, DataContext context)
	{
		_weaponSilenceFrame = weaponSilenceFrame;
		SetModifiedAndInvalidateInfluencedCache(264, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _weaponSilenceFrame.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 263, serializedSize);
			ptr += _weaponSilenceFrame.Serialize(ptr);
		}
	}

	public SpecialEffectList GetSilenceFrame()
	{
		return _silenceFrame;
	}

	public unsafe void SetSilenceFrame(SpecialEffectList silenceFrame, DataContext context)
	{
		_silenceFrame = silenceFrame;
		SetModifiedAndInvalidateInfluencedCache(265, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _silenceFrame.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 264, serializedSize);
			ptr += _silenceFrame.Serialize(ptr);
		}
	}

	public SpecialEffectList GetCurrAgeDelta()
	{
		return _currAgeDelta;
	}

	public unsafe void SetCurrAgeDelta(SpecialEffectList currAgeDelta, DataContext context)
	{
		_currAgeDelta = currAgeDelta;
		SetModifiedAndInvalidateInfluencedCache(266, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _currAgeDelta.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 265, serializedSize);
			ptr += _currAgeDelta.Serialize(ptr);
		}
	}

	public SpecialEffectList GetGoneMadInAllBreak()
	{
		return _goneMadInAllBreak;
	}

	public unsafe void SetGoneMadInAllBreak(SpecialEffectList goneMadInAllBreak, DataContext context)
	{
		_goneMadInAllBreak = goneMadInAllBreak;
		SetModifiedAndInvalidateInfluencedCache(267, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _goneMadInAllBreak.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 266, serializedSize);
			ptr += _goneMadInAllBreak.Serialize(ptr);
		}
	}

	public SpecialEffectList GetMakeLoveRateOnMonthChange()
	{
		return _makeLoveRateOnMonthChange;
	}

	public unsafe void SetMakeLoveRateOnMonthChange(SpecialEffectList makeLoveRateOnMonthChange, DataContext context)
	{
		_makeLoveRateOnMonthChange = makeLoveRateOnMonthChange;
		SetModifiedAndInvalidateInfluencedCache(268, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _makeLoveRateOnMonthChange.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 267, serializedSize);
			ptr += _makeLoveRateOnMonthChange.Serialize(ptr);
		}
	}

	public SpecialEffectList GetCanAutoHealOnMonthChange()
	{
		return _canAutoHealOnMonthChange;
	}

	public unsafe void SetCanAutoHealOnMonthChange(SpecialEffectList canAutoHealOnMonthChange, DataContext context)
	{
		_canAutoHealOnMonthChange = canAutoHealOnMonthChange;
		SetModifiedAndInvalidateInfluencedCache(269, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _canAutoHealOnMonthChange.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 268, serializedSize);
			ptr += _canAutoHealOnMonthChange.Serialize(ptr);
		}
	}

	public SpecialEffectList GetHappinessDelta()
	{
		return _happinessDelta;
	}

	public unsafe void SetHappinessDelta(SpecialEffectList happinessDelta, DataContext context)
	{
		_happinessDelta = happinessDelta;
		SetModifiedAndInvalidateInfluencedCache(270, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _happinessDelta.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 269, serializedSize);
			ptr += _happinessDelta.Serialize(ptr);
		}
	}

	public SpecialEffectList GetTeammateCmdCanUse()
	{
		return _teammateCmdCanUse;
	}

	public unsafe void SetTeammateCmdCanUse(SpecialEffectList teammateCmdCanUse, DataContext context)
	{
		_teammateCmdCanUse = teammateCmdCanUse;
		SetModifiedAndInvalidateInfluencedCache(271, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _teammateCmdCanUse.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 270, serializedSize);
			ptr += _teammateCmdCanUse.Serialize(ptr);
		}
	}

	public SpecialEffectList GetMixPoisonInfinityAffect()
	{
		return _mixPoisonInfinityAffect;
	}

	public unsafe void SetMixPoisonInfinityAffect(SpecialEffectList mixPoisonInfinityAffect, DataContext context)
	{
		_mixPoisonInfinityAffect = mixPoisonInfinityAffect;
		SetModifiedAndInvalidateInfluencedCache(272, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _mixPoisonInfinityAffect.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 271, serializedSize);
			ptr += _mixPoisonInfinityAffect.Serialize(ptr);
		}
	}

	public SpecialEffectList GetAttackRangeMaxAcupoint()
	{
		return _attackRangeMaxAcupoint;
	}

	public unsafe void SetAttackRangeMaxAcupoint(SpecialEffectList attackRangeMaxAcupoint, DataContext context)
	{
		_attackRangeMaxAcupoint = attackRangeMaxAcupoint;
		SetModifiedAndInvalidateInfluencedCache(273, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _attackRangeMaxAcupoint.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 272, serializedSize);
			ptr += _attackRangeMaxAcupoint.Serialize(ptr);
		}
	}

	public SpecialEffectList GetMaxMobilityPercent()
	{
		return _maxMobilityPercent;
	}

	public unsafe void SetMaxMobilityPercent(SpecialEffectList maxMobilityPercent, DataContext context)
	{
		_maxMobilityPercent = maxMobilityPercent;
		SetModifiedAndInvalidateInfluencedCache(274, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _maxMobilityPercent.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 273, serializedSize);
			ptr += _maxMobilityPercent.Serialize(ptr);
		}
	}

	public SpecialEffectList GetMakeMindDamage()
	{
		return _makeMindDamage;
	}

	public unsafe void SetMakeMindDamage(SpecialEffectList makeMindDamage, DataContext context)
	{
		_makeMindDamage = makeMindDamage;
		SetModifiedAndInvalidateInfluencedCache(275, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _makeMindDamage.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 274, serializedSize);
			ptr += _makeMindDamage.Serialize(ptr);
		}
	}

	public SpecialEffectList GetAcceptMindDamage()
	{
		return _acceptMindDamage;
	}

	public unsafe void SetAcceptMindDamage(SpecialEffectList acceptMindDamage, DataContext context)
	{
		_acceptMindDamage = acceptMindDamage;
		SetModifiedAndInvalidateInfluencedCache(276, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _acceptMindDamage.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 275, serializedSize);
			ptr += _acceptMindDamage.Serialize(ptr);
		}
	}

	public SpecialEffectList GetHitAddByTempValue()
	{
		return _hitAddByTempValue;
	}

	public unsafe void SetHitAddByTempValue(SpecialEffectList hitAddByTempValue, DataContext context)
	{
		_hitAddByTempValue = hitAddByTempValue;
		SetModifiedAndInvalidateInfluencedCache(277, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _hitAddByTempValue.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 276, serializedSize);
			ptr += _hitAddByTempValue.Serialize(ptr);
		}
	}

	public SpecialEffectList GetAvoidAddByTempValue()
	{
		return _avoidAddByTempValue;
	}

	public unsafe void SetAvoidAddByTempValue(SpecialEffectList avoidAddByTempValue, DataContext context)
	{
		_avoidAddByTempValue = avoidAddByTempValue;
		SetModifiedAndInvalidateInfluencedCache(278, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _avoidAddByTempValue.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 277, serializedSize);
			ptr += _avoidAddByTempValue.Serialize(ptr);
		}
	}

	public SpecialEffectList GetIgnoreEquipmentOverload()
	{
		return _ignoreEquipmentOverload;
	}

	public unsafe void SetIgnoreEquipmentOverload(SpecialEffectList ignoreEquipmentOverload, DataContext context)
	{
		_ignoreEquipmentOverload = ignoreEquipmentOverload;
		SetModifiedAndInvalidateInfluencedCache(279, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _ignoreEquipmentOverload.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 278, serializedSize);
			ptr += _ignoreEquipmentOverload.Serialize(ptr);
		}
	}

	public SpecialEffectList GetCanCostEnemyUsableTricks()
	{
		return _canCostEnemyUsableTricks;
	}

	public unsafe void SetCanCostEnemyUsableTricks(SpecialEffectList canCostEnemyUsableTricks, DataContext context)
	{
		_canCostEnemyUsableTricks = canCostEnemyUsableTricks;
		SetModifiedAndInvalidateInfluencedCache(280, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _canCostEnemyUsableTricks.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 279, serializedSize);
			ptr += _canCostEnemyUsableTricks.Serialize(ptr);
		}
	}

	public SpecialEffectList GetIgnoreArmor()
	{
		return _ignoreArmor;
	}

	public unsafe void SetIgnoreArmor(SpecialEffectList ignoreArmor, DataContext context)
	{
		_ignoreArmor = ignoreArmor;
		SetModifiedAndInvalidateInfluencedCache(281, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _ignoreArmor.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 280, serializedSize);
			ptr += _ignoreArmor.Serialize(ptr);
		}
	}

	public SpecialEffectList GetUnyieldingFallen()
	{
		return _unyieldingFallen;
	}

	public unsafe void SetUnyieldingFallen(SpecialEffectList unyieldingFallen, DataContext context)
	{
		_unyieldingFallen = unyieldingFallen;
		SetModifiedAndInvalidateInfluencedCache(282, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _unyieldingFallen.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 281, serializedSize);
			ptr += _unyieldingFallen.Serialize(ptr);
		}
	}

	public SpecialEffectList GetNormalAttackPrepareFrame()
	{
		return _normalAttackPrepareFrame;
	}

	public unsafe void SetNormalAttackPrepareFrame(SpecialEffectList normalAttackPrepareFrame, DataContext context)
	{
		_normalAttackPrepareFrame = normalAttackPrepareFrame;
		SetModifiedAndInvalidateInfluencedCache(283, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _normalAttackPrepareFrame.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 282, serializedSize);
			ptr += _normalAttackPrepareFrame.Serialize(ptr);
		}
	}

	public SpecialEffectList GetCanCostUselessTricks()
	{
		return _canCostUselessTricks;
	}

	public unsafe void SetCanCostUselessTricks(SpecialEffectList canCostUselessTricks, DataContext context)
	{
		_canCostUselessTricks = canCostUselessTricks;
		SetModifiedAndInvalidateInfluencedCache(284, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _canCostUselessTricks.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 283, serializedSize);
			ptr += _canCostUselessTricks.Serialize(ptr);
		}
	}

	public SpecialEffectList GetDefendSkillCanAffect()
	{
		return _defendSkillCanAffect;
	}

	public unsafe void SetDefendSkillCanAffect(SpecialEffectList defendSkillCanAffect, DataContext context)
	{
		_defendSkillCanAffect = defendSkillCanAffect;
		SetModifiedAndInvalidateInfluencedCache(285, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _defendSkillCanAffect.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 284, serializedSize);
			ptr += _defendSkillCanAffect.Serialize(ptr);
		}
	}

	public SpecialEffectList GetAssistSkillCanAffect()
	{
		return _assistSkillCanAffect;
	}

	public unsafe void SetAssistSkillCanAffect(SpecialEffectList assistSkillCanAffect, DataContext context)
	{
		_assistSkillCanAffect = assistSkillCanAffect;
		SetModifiedAndInvalidateInfluencedCache(286, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _assistSkillCanAffect.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 285, serializedSize);
			ptr += _assistSkillCanAffect.Serialize(ptr);
		}
	}

	public SpecialEffectList GetAgileSkillCanAffect()
	{
		return _agileSkillCanAffect;
	}

	public unsafe void SetAgileSkillCanAffect(SpecialEffectList agileSkillCanAffect, DataContext context)
	{
		_agileSkillCanAffect = agileSkillCanAffect;
		SetModifiedAndInvalidateInfluencedCache(287, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _agileSkillCanAffect.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 286, serializedSize);
			ptr += _agileSkillCanAffect.Serialize(ptr);
		}
	}

	public SpecialEffectList GetAllMarkChangeToMind()
	{
		return _allMarkChangeToMind;
	}

	public unsafe void SetAllMarkChangeToMind(SpecialEffectList allMarkChangeToMind, DataContext context)
	{
		_allMarkChangeToMind = allMarkChangeToMind;
		SetModifiedAndInvalidateInfluencedCache(288, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _allMarkChangeToMind.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 287, serializedSize);
			ptr += _allMarkChangeToMind.Serialize(ptr);
		}
	}

	public SpecialEffectList GetMindMarkChangeToFatal()
	{
		return _mindMarkChangeToFatal;
	}

	public unsafe void SetMindMarkChangeToFatal(SpecialEffectList mindMarkChangeToFatal, DataContext context)
	{
		_mindMarkChangeToFatal = mindMarkChangeToFatal;
		SetModifiedAndInvalidateInfluencedCache(289, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _mindMarkChangeToFatal.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 288, serializedSize);
			ptr += _mindMarkChangeToFatal.Serialize(ptr);
		}
	}

	public SpecialEffectList GetCanCast()
	{
		return _canCast;
	}

	public unsafe void SetCanCast(SpecialEffectList canCast, DataContext context)
	{
		_canCast = canCast;
		SetModifiedAndInvalidateInfluencedCache(290, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _canCast.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 289, serializedSize);
			ptr += _canCast.Serialize(ptr);
		}
	}

	public SpecialEffectList GetInevitableAvoid()
	{
		return _inevitableAvoid;
	}

	public unsafe void SetInevitableAvoid(SpecialEffectList inevitableAvoid, DataContext context)
	{
		_inevitableAvoid = inevitableAvoid;
		SetModifiedAndInvalidateInfluencedCache(291, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _inevitableAvoid.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 290, serializedSize);
			ptr += _inevitableAvoid.Serialize(ptr);
		}
	}

	public SpecialEffectList GetPowerEffectReverse()
	{
		return _powerEffectReverse;
	}

	public unsafe void SetPowerEffectReverse(SpecialEffectList powerEffectReverse, DataContext context)
	{
		_powerEffectReverse = powerEffectReverse;
		SetModifiedAndInvalidateInfluencedCache(292, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _powerEffectReverse.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 291, serializedSize);
			ptr += _powerEffectReverse.Serialize(ptr);
		}
	}

	public SpecialEffectList GetFeatureBonusReverse()
	{
		return _featureBonusReverse;
	}

	public unsafe void SetFeatureBonusReverse(SpecialEffectList featureBonusReverse, DataContext context)
	{
		_featureBonusReverse = featureBonusReverse;
		SetModifiedAndInvalidateInfluencedCache(293, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _featureBonusReverse.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 292, serializedSize);
			ptr += _featureBonusReverse.Serialize(ptr);
		}
	}

	public SpecialEffectList GetWugFatalDamageValue()
	{
		return _wugFatalDamageValue;
	}

	public unsafe void SetWugFatalDamageValue(SpecialEffectList wugFatalDamageValue, DataContext context)
	{
		_wugFatalDamageValue = wugFatalDamageValue;
		SetModifiedAndInvalidateInfluencedCache(294, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _wugFatalDamageValue.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 293, serializedSize);
			ptr += _wugFatalDamageValue.Serialize(ptr);
		}
	}

	public SpecialEffectList GetCanRecoverHealthOnMonthChange()
	{
		return _canRecoverHealthOnMonthChange;
	}

	public unsafe void SetCanRecoverHealthOnMonthChange(SpecialEffectList canRecoverHealthOnMonthChange, DataContext context)
	{
		_canRecoverHealthOnMonthChange = canRecoverHealthOnMonthChange;
		SetModifiedAndInvalidateInfluencedCache(295, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _canRecoverHealthOnMonthChange.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 294, serializedSize);
			ptr += _canRecoverHealthOnMonthChange.Serialize(ptr);
		}
	}

	public SpecialEffectList GetTakeRevengeRateOnMonthChange()
	{
		return _takeRevengeRateOnMonthChange;
	}

	public unsafe void SetTakeRevengeRateOnMonthChange(SpecialEffectList takeRevengeRateOnMonthChange, DataContext context)
	{
		_takeRevengeRateOnMonthChange = takeRevengeRateOnMonthChange;
		SetModifiedAndInvalidateInfluencedCache(296, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _takeRevengeRateOnMonthChange.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 295, serializedSize);
			ptr += _takeRevengeRateOnMonthChange.Serialize(ptr);
		}
	}

	public SpecialEffectList GetConsummateLevelBonus()
	{
		return _consummateLevelBonus;
	}

	public unsafe void SetConsummateLevelBonus(SpecialEffectList consummateLevelBonus, DataContext context)
	{
		_consummateLevelBonus = consummateLevelBonus;
		SetModifiedAndInvalidateInfluencedCache(297, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _consummateLevelBonus.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 296, serializedSize);
			ptr += _consummateLevelBonus.Serialize(ptr);
		}
	}

	public SpecialEffectList GetNeiliDelta()
	{
		return _neiliDelta;
	}

	public unsafe void SetNeiliDelta(SpecialEffectList neiliDelta, DataContext context)
	{
		_neiliDelta = neiliDelta;
		SetModifiedAndInvalidateInfluencedCache(298, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _neiliDelta.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 297, serializedSize);
			ptr += _neiliDelta.Serialize(ptr);
		}
	}

	public SpecialEffectList GetCanMakeLoveSpecialOnMonthChange()
	{
		return _canMakeLoveSpecialOnMonthChange;
	}

	public unsafe void SetCanMakeLoveSpecialOnMonthChange(SpecialEffectList canMakeLoveSpecialOnMonthChange, DataContext context)
	{
		_canMakeLoveSpecialOnMonthChange = canMakeLoveSpecialOnMonthChange;
		SetModifiedAndInvalidateInfluencedCache(299, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _canMakeLoveSpecialOnMonthChange.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 298, serializedSize);
			ptr += _canMakeLoveSpecialOnMonthChange.Serialize(ptr);
		}
	}

	public SpecialEffectList GetHealAcupointSpeed()
	{
		return _healAcupointSpeed;
	}

	public unsafe void SetHealAcupointSpeed(SpecialEffectList healAcupointSpeed, DataContext context)
	{
		_healAcupointSpeed = healAcupointSpeed;
		SetModifiedAndInvalidateInfluencedCache(300, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _healAcupointSpeed.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 299, serializedSize);
			ptr += _healAcupointSpeed.Serialize(ptr);
		}
	}

	public SpecialEffectList GetMaxChangeTrickCount()
	{
		return _maxChangeTrickCount;
	}

	public unsafe void SetMaxChangeTrickCount(SpecialEffectList maxChangeTrickCount, DataContext context)
	{
		_maxChangeTrickCount = maxChangeTrickCount;
		SetModifiedAndInvalidateInfluencedCache(301, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _maxChangeTrickCount.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 300, serializedSize);
			ptr += _maxChangeTrickCount.Serialize(ptr);
		}
	}

	public SpecialEffectList GetConvertCostBreathAndStance()
	{
		return _convertCostBreathAndStance;
	}

	public unsafe void SetConvertCostBreathAndStance(SpecialEffectList convertCostBreathAndStance, DataContext context)
	{
		_convertCostBreathAndStance = convertCostBreathAndStance;
		SetModifiedAndInvalidateInfluencedCache(302, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _convertCostBreathAndStance.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 301, serializedSize);
			ptr += _convertCostBreathAndStance.Serialize(ptr);
		}
	}

	public SpecialEffectList GetPersonalitiesAll()
	{
		return _personalitiesAll;
	}

	public unsafe void SetPersonalitiesAll(SpecialEffectList personalitiesAll, DataContext context)
	{
		_personalitiesAll = personalitiesAll;
		SetModifiedAndInvalidateInfluencedCache(303, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _personalitiesAll.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 302, serializedSize);
			ptr += _personalitiesAll.Serialize(ptr);
		}
	}

	public SpecialEffectList GetFinalFatalDamageMarkCount()
	{
		return _finalFatalDamageMarkCount;
	}

	public unsafe void SetFinalFatalDamageMarkCount(SpecialEffectList finalFatalDamageMarkCount, DataContext context)
	{
		_finalFatalDamageMarkCount = finalFatalDamageMarkCount;
		SetModifiedAndInvalidateInfluencedCache(304, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _finalFatalDamageMarkCount.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 303, serializedSize);
			ptr += _finalFatalDamageMarkCount.Serialize(ptr);
		}
	}

	public SpecialEffectList GetInfinityMindMarkProgress()
	{
		return _infinityMindMarkProgress;
	}

	public unsafe void SetInfinityMindMarkProgress(SpecialEffectList infinityMindMarkProgress, DataContext context)
	{
		_infinityMindMarkProgress = infinityMindMarkProgress;
		SetModifiedAndInvalidateInfluencedCache(305, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _infinityMindMarkProgress.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 304, serializedSize);
			ptr += _infinityMindMarkProgress.Serialize(ptr);
		}
	}

	public SpecialEffectList GetCombatSkillAiScorePower()
	{
		return _combatSkillAiScorePower;
	}

	public unsafe void SetCombatSkillAiScorePower(SpecialEffectList combatSkillAiScorePower, DataContext context)
	{
		_combatSkillAiScorePower = combatSkillAiScorePower;
		SetModifiedAndInvalidateInfluencedCache(306, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _combatSkillAiScorePower.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 305, serializedSize);
			ptr += _combatSkillAiScorePower.Serialize(ptr);
		}
	}

	public SpecialEffectList GetNormalAttackChangeToUnlockAttack()
	{
		return _normalAttackChangeToUnlockAttack;
	}

	public unsafe void SetNormalAttackChangeToUnlockAttack(SpecialEffectList normalAttackChangeToUnlockAttack, DataContext context)
	{
		_normalAttackChangeToUnlockAttack = normalAttackChangeToUnlockAttack;
		SetModifiedAndInvalidateInfluencedCache(307, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _normalAttackChangeToUnlockAttack.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 306, serializedSize);
			ptr += _normalAttackChangeToUnlockAttack.Serialize(ptr);
		}
	}

	public SpecialEffectList GetAttackBodyPartOdds()
	{
		return _attackBodyPartOdds;
	}

	public unsafe void SetAttackBodyPartOdds(SpecialEffectList attackBodyPartOdds, DataContext context)
	{
		_attackBodyPartOdds = attackBodyPartOdds;
		SetModifiedAndInvalidateInfluencedCache(308, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _attackBodyPartOdds.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 307, serializedSize);
			ptr += _attackBodyPartOdds.Serialize(ptr);
		}
	}

	public SpecialEffectList GetChangeDurability()
	{
		return _changeDurability;
	}

	public unsafe void SetChangeDurability(SpecialEffectList changeDurability, DataContext context)
	{
		_changeDurability = changeDurability;
		SetModifiedAndInvalidateInfluencedCache(309, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _changeDurability.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 308, serializedSize);
			ptr += _changeDurability.Serialize(ptr);
		}
	}

	public SpecialEffectList GetEquipmentBonus()
	{
		return _equipmentBonus;
	}

	public unsafe void SetEquipmentBonus(SpecialEffectList equipmentBonus, DataContext context)
	{
		_equipmentBonus = equipmentBonus;
		SetModifiedAndInvalidateInfluencedCache(310, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _equipmentBonus.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 309, serializedSize);
			ptr += _equipmentBonus.Serialize(ptr);
		}
	}

	public SpecialEffectList GetEquipmentWeight()
	{
		return _equipmentWeight;
	}

	public unsafe void SetEquipmentWeight(SpecialEffectList equipmentWeight, DataContext context)
	{
		_equipmentWeight = equipmentWeight;
		SetModifiedAndInvalidateInfluencedCache(311, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _equipmentWeight.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 310, serializedSize);
			ptr += _equipmentWeight.Serialize(ptr);
		}
	}

	public SpecialEffectList GetRawCreateEffectList()
	{
		return _rawCreateEffectList;
	}

	public unsafe void SetRawCreateEffectList(SpecialEffectList rawCreateEffectList, DataContext context)
	{
		_rawCreateEffectList = rawCreateEffectList;
		SetModifiedAndInvalidateInfluencedCache(312, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _rawCreateEffectList.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 311, serializedSize);
			ptr += _rawCreateEffectList.Serialize(ptr);
		}
	}

	public SpecialEffectList GetJiTrickAsWeaponTrickCount()
	{
		return _jiTrickAsWeaponTrickCount;
	}

	public unsafe void SetJiTrickAsWeaponTrickCount(SpecialEffectList jiTrickAsWeaponTrickCount, DataContext context)
	{
		_jiTrickAsWeaponTrickCount = jiTrickAsWeaponTrickCount;
		SetModifiedAndInvalidateInfluencedCache(313, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _jiTrickAsWeaponTrickCount.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 312, serializedSize);
			ptr += _jiTrickAsWeaponTrickCount.Serialize(ptr);
		}
	}

	public SpecialEffectList GetUselessTrickAsJiTrickCount()
	{
		return _uselessTrickAsJiTrickCount;
	}

	public unsafe void SetUselessTrickAsJiTrickCount(SpecialEffectList uselessTrickAsJiTrickCount, DataContext context)
	{
		_uselessTrickAsJiTrickCount = uselessTrickAsJiTrickCount;
		SetModifiedAndInvalidateInfluencedCache(314, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _uselessTrickAsJiTrickCount.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 313, serializedSize);
			ptr += _uselessTrickAsJiTrickCount.Serialize(ptr);
		}
	}

	public SpecialEffectList GetEquipmentPower()
	{
		return _equipmentPower;
	}

	public unsafe void SetEquipmentPower(SpecialEffectList equipmentPower, DataContext context)
	{
		_equipmentPower = equipmentPower;
		SetModifiedAndInvalidateInfluencedCache(315, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _equipmentPower.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 314, serializedSize);
			ptr += _equipmentPower.Serialize(ptr);
		}
	}

	public SpecialEffectList GetHealFlawSpeed()
	{
		return _healFlawSpeed;
	}

	public unsafe void SetHealFlawSpeed(SpecialEffectList healFlawSpeed, DataContext context)
	{
		_healFlawSpeed = healFlawSpeed;
		SetModifiedAndInvalidateInfluencedCache(316, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _healFlawSpeed.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 315, serializedSize);
			ptr += _healFlawSpeed.Serialize(ptr);
		}
	}

	public SpecialEffectList GetUnlockSpeed()
	{
		return _unlockSpeed;
	}

	public unsafe void SetUnlockSpeed(SpecialEffectList unlockSpeed, DataContext context)
	{
		_unlockSpeed = unlockSpeed;
		SetModifiedAndInvalidateInfluencedCache(317, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _unlockSpeed.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 316, serializedSize);
			ptr += _unlockSpeed.Serialize(ptr);
		}
	}

	public SpecialEffectList GetFlawBonusFactor()
	{
		return _flawBonusFactor;
	}

	public unsafe void SetFlawBonusFactor(SpecialEffectList flawBonusFactor, DataContext context)
	{
		_flawBonusFactor = flawBonusFactor;
		SetModifiedAndInvalidateInfluencedCache(318, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _flawBonusFactor.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 317, serializedSize);
			ptr += _flawBonusFactor.Serialize(ptr);
		}
	}

	public SpecialEffectList GetCanCostShaTricks()
	{
		return _canCostShaTricks;
	}

	public unsafe void SetCanCostShaTricks(SpecialEffectList canCostShaTricks, DataContext context)
	{
		_canCostShaTricks = canCostShaTricks;
		SetModifiedAndInvalidateInfluencedCache(319, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _canCostShaTricks.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 318, serializedSize);
			ptr += _canCostShaTricks.Serialize(ptr);
		}
	}

	public SpecialEffectList GetDefenderDirectFinalDamageValue()
	{
		return _defenderDirectFinalDamageValue;
	}

	public unsafe void SetDefenderDirectFinalDamageValue(SpecialEffectList defenderDirectFinalDamageValue, DataContext context)
	{
		_defenderDirectFinalDamageValue = defenderDirectFinalDamageValue;
		SetModifiedAndInvalidateInfluencedCache(320, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _defenderDirectFinalDamageValue.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 319, serializedSize);
			ptr += _defenderDirectFinalDamageValue.Serialize(ptr);
		}
	}

	public SpecialEffectList GetNormalAttackRecoveryFrame()
	{
		return _normalAttackRecoveryFrame;
	}

	public unsafe void SetNormalAttackRecoveryFrame(SpecialEffectList normalAttackRecoveryFrame, DataContext context)
	{
		_normalAttackRecoveryFrame = normalAttackRecoveryFrame;
		SetModifiedAndInvalidateInfluencedCache(321, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _normalAttackRecoveryFrame.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 320, serializedSize);
			ptr += _normalAttackRecoveryFrame.Serialize(ptr);
		}
	}

	public SpecialEffectList GetFinalGoneMadInjury()
	{
		return _finalGoneMadInjury;
	}

	public unsafe void SetFinalGoneMadInjury(SpecialEffectList finalGoneMadInjury, DataContext context)
	{
		_finalGoneMadInjury = finalGoneMadInjury;
		SetModifiedAndInvalidateInfluencedCache(322, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _finalGoneMadInjury.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 321, serializedSize);
			ptr += _finalGoneMadInjury.Serialize(ptr);
		}
	}

	public SpecialEffectList GetAttackerDirectFinalDamageValue()
	{
		return _attackerDirectFinalDamageValue;
	}

	public unsafe void SetAttackerDirectFinalDamageValue(SpecialEffectList attackerDirectFinalDamageValue, DataContext context)
	{
		_attackerDirectFinalDamageValue = attackerDirectFinalDamageValue;
		SetModifiedAndInvalidateInfluencedCache(323, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _attackerDirectFinalDamageValue.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 322, serializedSize);
			ptr += _attackerDirectFinalDamageValue.Serialize(ptr);
		}
	}

	public SpecialEffectList GetCanCostTrickDuringPreparingSkill()
	{
		return _canCostTrickDuringPreparingSkill;
	}

	public unsafe void SetCanCostTrickDuringPreparingSkill(SpecialEffectList canCostTrickDuringPreparingSkill, DataContext context)
	{
		_canCostTrickDuringPreparingSkill = canCostTrickDuringPreparingSkill;
		SetModifiedAndInvalidateInfluencedCache(324, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _canCostTrickDuringPreparingSkill.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 323, serializedSize);
			ptr += _canCostTrickDuringPreparingSkill.Serialize(ptr);
		}
	}

	public SpecialEffectList GetValidItemList()
	{
		return _validItemList;
	}

	public unsafe void SetValidItemList(SpecialEffectList validItemList, DataContext context)
	{
		_validItemList = validItemList;
		SetModifiedAndInvalidateInfluencedCache(325, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _validItemList.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 324, serializedSize);
			ptr += _validItemList.Serialize(ptr);
		}
	}

	public SpecialEffectList GetAcceptDamageCanAdd()
	{
		return _acceptDamageCanAdd;
	}

	public unsafe void SetAcceptDamageCanAdd(SpecialEffectList acceptDamageCanAdd, DataContext context)
	{
		_acceptDamageCanAdd = acceptDamageCanAdd;
		SetModifiedAndInvalidateInfluencedCache(326, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _acceptDamageCanAdd.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 325, serializedSize);
			ptr += _acceptDamageCanAdd.Serialize(ptr);
		}
	}

	public SpecialEffectList GetMakeDamageCanReduce()
	{
		return _makeDamageCanReduce;
	}

	public unsafe void SetMakeDamageCanReduce(SpecialEffectList makeDamageCanReduce, DataContext context)
	{
		_makeDamageCanReduce = makeDamageCanReduce;
		SetModifiedAndInvalidateInfluencedCache(327, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _makeDamageCanReduce.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 326, serializedSize);
			ptr += _makeDamageCanReduce.Serialize(ptr);
		}
	}

	public SpecialEffectList GetNormalAttackGetTrickCount()
	{
		return _normalAttackGetTrickCount;
	}

	public unsafe void SetNormalAttackGetTrickCount(SpecialEffectList normalAttackGetTrickCount, DataContext context)
	{
		_normalAttackGetTrickCount = normalAttackGetTrickCount;
		SetModifiedAndInvalidateInfluencedCache(328, context);
		if (CollectionHelperData.IsArchive)
		{
			int serializedSize = _normalAttackGetTrickCount.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicObjectCollection_SetDynamicField(CollectionHelperData.DomainId, CollectionHelperData.DataId, _id, 327, serializedSize);
			ptr += _normalAttackGetTrickCount.Serialize(ptr);
		}
	}

	public AffectedData()
	{
		_maxStrength = new SpecialEffectList();
		_maxDexterity = new SpecialEffectList();
		_maxConcentration = new SpecialEffectList();
		_maxVitality = new SpecialEffectList();
		_maxEnergy = new SpecialEffectList();
		_maxIntelligence = new SpecialEffectList();
		_recoveryOfStance = new SpecialEffectList();
		_recoveryOfBreath = new SpecialEffectList();
		_moveSpeed = new SpecialEffectList();
		_recoveryOfFlaw = new SpecialEffectList();
		_castSpeed = new SpecialEffectList();
		_recoveryOfBlockedAcupoint = new SpecialEffectList();
		_weaponSwitchSpeed = new SpecialEffectList();
		_attackSpeed = new SpecialEffectList();
		_innerRatio = new SpecialEffectList();
		_recoveryOfQiDisorder = new SpecialEffectList();
		_minorAttributeFixMaxValue = new SpecialEffectList();
		_minorAttributeFixMinValue = new SpecialEffectList();
		_resistOfHotPoison = new SpecialEffectList();
		_resistOfGloomyPoison = new SpecialEffectList();
		_resistOfColdPoison = new SpecialEffectList();
		_resistOfRedPoison = new SpecialEffectList();
		_resistOfRottenPoison = new SpecialEffectList();
		_resistOfIllusoryPoison = new SpecialEffectList();
		_displayAge = new SpecialEffectList();
		_neiliProportionOfFiveElements = new SpecialEffectList();
		_weaponMaxPower = new SpecialEffectList();
		_weaponUseRequirement = new SpecialEffectList();
		_weaponAttackRange = new SpecialEffectList();
		_armorMaxPower = new SpecialEffectList();
		_armorUseRequirement = new SpecialEffectList();
		_hitStrength = new SpecialEffectList();
		_hitTechnique = new SpecialEffectList();
		_hitSpeed = new SpecialEffectList();
		_hitMind = new SpecialEffectList();
		_hitCanChange = new SpecialEffectList();
		_hitChangeEffectPercent = new SpecialEffectList();
		_avoidStrength = new SpecialEffectList();
		_avoidTechnique = new SpecialEffectList();
		_avoidSpeed = new SpecialEffectList();
		_avoidMind = new SpecialEffectList();
		_avoidCanChange = new SpecialEffectList();
		_avoidChangeEffectPercent = new SpecialEffectList();
		_penetrateOuter = new SpecialEffectList();
		_penetrateInner = new SpecialEffectList();
		_penetrateResistOuter = new SpecialEffectList();
		_penetrateResistInner = new SpecialEffectList();
		_neiliAllocationAttack = new SpecialEffectList();
		_neiliAllocationAgile = new SpecialEffectList();
		_neiliAllocationDefense = new SpecialEffectList();
		_neiliAllocationAssist = new SpecialEffectList();
		_happiness = new SpecialEffectList();
		_maxHealth = new SpecialEffectList();
		_healthCost = new SpecialEffectList();
		_moveSpeedCanChange = new SpecialEffectList();
		_attackerHitStrength = new SpecialEffectList();
		_attackerHitTechnique = new SpecialEffectList();
		_attackerHitSpeed = new SpecialEffectList();
		_attackerHitMind = new SpecialEffectList();
		_attackerAvoidStrength = new SpecialEffectList();
		_attackerAvoidTechnique = new SpecialEffectList();
		_attackerAvoidSpeed = new SpecialEffectList();
		_attackerAvoidMind = new SpecialEffectList();
		_attackerPenetrateOuter = new SpecialEffectList();
		_attackerPenetrateInner = new SpecialEffectList();
		_attackerPenetrateResistOuter = new SpecialEffectList();
		_attackerPenetrateResistInner = new SpecialEffectList();
		_attackHitType = new SpecialEffectList();
		_makeDirectDamage = new SpecialEffectList();
		_makeBounceDamage = new SpecialEffectList();
		_makeFightBackDamage = new SpecialEffectList();
		_makePoisonLevel = new SpecialEffectList();
		_makePoisonValue = new SpecialEffectList();
		_attackerHitOdds = new SpecialEffectList();
		_attackerFightBackHitOdds = new SpecialEffectList();
		_attackerPursueOdds = new SpecialEffectList();
		_makedInjuryChangeToOld = new SpecialEffectList();
		_makedPoisonChangeToOld = new SpecialEffectList();
		_makeDamageType = new SpecialEffectList();
		_canMakeInjuryToNoInjuryPart = new SpecialEffectList();
		_makePoisonType = new SpecialEffectList();
		_normalAttackWeapon = new SpecialEffectList();
		_normalAttackTrick = new SpecialEffectList();
		_extraFlawCount = new SpecialEffectList();
		_attackCanBounce = new SpecialEffectList();
		_attackCanFightBack = new SpecialEffectList();
		_makeFightBackInjuryMark = new SpecialEffectList();
		_legSkillUseShoes = new SpecialEffectList();
		_attackerFinalDamageValue = new SpecialEffectList();
		_defenderHitStrength = new SpecialEffectList();
		_defenderHitTechnique = new SpecialEffectList();
		_defenderHitSpeed = new SpecialEffectList();
		_defenderHitMind = new SpecialEffectList();
		_defenderAvoidStrength = new SpecialEffectList();
		_defenderAvoidTechnique = new SpecialEffectList();
		_defenderAvoidSpeed = new SpecialEffectList();
		_defenderAvoidMind = new SpecialEffectList();
		_defenderPenetrateOuter = new SpecialEffectList();
		_defenderPenetrateInner = new SpecialEffectList();
		_defenderPenetrateResistOuter = new SpecialEffectList();
		_defenderPenetrateResistInner = new SpecialEffectList();
		_acceptDirectDamage = new SpecialEffectList();
		_acceptBounceDamage = new SpecialEffectList();
		_acceptFightBackDamage = new SpecialEffectList();
		_acceptPoisonLevel = new SpecialEffectList();
		_acceptPoisonValue = new SpecialEffectList();
		_defenderHitOdds = new SpecialEffectList();
		_defenderFightBackHitOdds = new SpecialEffectList();
		_defenderPursueOdds = new SpecialEffectList();
		_acceptMaxInjuryCount = new SpecialEffectList();
		_bouncePower = new SpecialEffectList();
		_fightBackPower = new SpecialEffectList();
		_directDamageInnerRatio = new SpecialEffectList();
		_defenderFinalDamageValue = new SpecialEffectList();
		_directDamageValue = new SpecialEffectList();
		_directInjuryMark = new SpecialEffectList();
		_goneMadInjury = new SpecialEffectList();
		_healInjurySpeed = new SpecialEffectList();
		_healInjuryBuff = new SpecialEffectList();
		_healInjuryDebuff = new SpecialEffectList();
		_healPoisonSpeed = new SpecialEffectList();
		_healPoisonBuff = new SpecialEffectList();
		_healPoisonDebuff = new SpecialEffectList();
		_fleeSpeed = new SpecialEffectList();
		_maxFlawCount = new SpecialEffectList();
		_canAddFlaw = new SpecialEffectList();
		_flawLevel = new SpecialEffectList();
		_flawLevelCanReduce = new SpecialEffectList();
		_flawCount = new SpecialEffectList();
		_maxAcupointCount = new SpecialEffectList();
		_canAddAcupoint = new SpecialEffectList();
		_acupointLevel = new SpecialEffectList();
		_acupointLevelCanReduce = new SpecialEffectList();
		_acupointCount = new SpecialEffectList();
		_addNeiliAllocation = new SpecialEffectList();
		_costNeiliAllocation = new SpecialEffectList();
		_canChangeNeiliAllocation = new SpecialEffectList();
		_canGetTrick = new SpecialEffectList();
		_getTrickType = new SpecialEffectList();
		_attackBodyPart = new SpecialEffectList();
		_weaponEquipAttack = new SpecialEffectList();
		_weaponEquipDefense = new SpecialEffectList();
		_armorEquipAttack = new SpecialEffectList();
		_armorEquipDefense = new SpecialEffectList();
		_attackRangeForward = new SpecialEffectList();
		_attackRangeBackward = new SpecialEffectList();
		_moveCanBeStopped = new SpecialEffectList();
		_canForcedMove = new SpecialEffectList();
		_mobilityCanBeRemoved = new SpecialEffectList();
		_mobilityCostByEffect = new SpecialEffectList();
		_moveDistance = new SpecialEffectList();
		_jumpPrepareFrame = new SpecialEffectList();
		_bounceInjuryMark = new SpecialEffectList();
		_skillHasCost = new SpecialEffectList();
		_combatStateEffect = new SpecialEffectList();
		_changeNeedUseSkill = new SpecialEffectList();
		_changeDistanceIsMove = new SpecialEffectList();
		_replaceCharHit = new SpecialEffectList();
		_canAddPoison = new SpecialEffectList();
		_canReducePoison = new SpecialEffectList();
		_reducePoisonValue = new SpecialEffectList();
		_poisonCanAffect = new SpecialEffectList();
		_poisonAffectCount = new SpecialEffectList();
		_costTricks = new SpecialEffectList();
		_jumpMoveDistance = new SpecialEffectList();
		_combatStateToAdd = new SpecialEffectList();
		_combatStatePower = new SpecialEffectList();
		_breakBodyPartInjuryCount = new SpecialEffectList();
		_bodyPartIsBroken = new SpecialEffectList();
		_maxTrickCount = new SpecialEffectList();
		_maxBreathPercent = new SpecialEffectList();
		_maxStancePercent = new SpecialEffectList();
		_extraBreathPercent = new SpecialEffectList();
		_extraStancePercent = new SpecialEffectList();
		_moveCostMobility = new SpecialEffectList();
		_defendSkillKeepTime = new SpecialEffectList();
		_bounceRange = new SpecialEffectList();
		_mindMarkKeepTime = new SpecialEffectList();
		_skillMobilityCostPerFrame = new SpecialEffectList();
		_canAddWug = new SpecialEffectList();
		_hasGodWeaponBuff = new SpecialEffectList();
		_hasGodArmorBuff = new SpecialEffectList();
		_teammateCmdRequireGenerateValue = new SpecialEffectList();
		_teammateCmdEffect = new SpecialEffectList();
		_flawRecoverSpeed = new SpecialEffectList();
		_acupointRecoverSpeed = new SpecialEffectList();
		_mindMarkRecoverSpeed = new SpecialEffectList();
		_injuryAutoHealSpeed = new SpecialEffectList();
		_canRecoverBreath = new SpecialEffectList();
		_canRecoverStance = new SpecialEffectList();
		_fatalDamageValue = new SpecialEffectList();
		_fatalDamageMarkCount = new SpecialEffectList();
		_canFightBackDuringPrepareSkill = new SpecialEffectList();
		_skillPrepareSpeed = new SpecialEffectList();
		_breathRecoverSpeed = new SpecialEffectList();
		_stanceRecoverSpeed = new SpecialEffectList();
		_mobilityRecoverSpeed = new SpecialEffectList();
		_changeTrickProgressAddValue = new SpecialEffectList();
		_power = new SpecialEffectList();
		_maxPower = new SpecialEffectList();
		_powerCanReduce = new SpecialEffectList();
		_useRequirement = new SpecialEffectList();
		_currInnerRatio = new SpecialEffectList();
		_costBreathAndStance = new SpecialEffectList();
		_costBreath = new SpecialEffectList();
		_costStance = new SpecialEffectList();
		_costMobility = new SpecialEffectList();
		_skillCostTricks = new SpecialEffectList();
		_effectDirection = new SpecialEffectList();
		_effectDirectionCanChange = new SpecialEffectList();
		_gridCost = new SpecialEffectList();
		_prepareTotalProgress = new SpecialEffectList();
		_specificGridCount = new SpecialEffectList();
		_genericGridCount = new SpecialEffectList();
		_canInterrupt = new SpecialEffectList();
		_interruptOdds = new SpecialEffectList();
		_canSilence = new SpecialEffectList();
		_silenceOdds = new SpecialEffectList();
		_canCastWithBrokenBodyPart = new SpecialEffectList();
		_addPowerCanBeRemoved = new SpecialEffectList();
		_skillType = new SpecialEffectList();
		_effectCountCanChange = new SpecialEffectList();
		_canCastInDefend = new SpecialEffectList();
		_hitDistribution = new SpecialEffectList();
		_canCastOnLackBreath = new SpecialEffectList();
		_canCastOnLackStance = new SpecialEffectList();
		_costBreathOnCast = new SpecialEffectList();
		_costStanceOnCast = new SpecialEffectList();
		_canUseMobilityAsBreath = new SpecialEffectList();
		_canUseMobilityAsStance = new SpecialEffectList();
		_castCostNeiliAllocation = new SpecialEffectList();
		_acceptPoisonResist = new SpecialEffectList();
		_makePoisonResist = new SpecialEffectList();
		_canCriticalHit = new SpecialEffectList();
		_canCostNeiliAllocationEffect = new SpecialEffectList();
		_consummateLevelRelatedMainAttributesHitValues = new SpecialEffectList();
		_consummateLevelRelatedMainAttributesAvoidValues = new SpecialEffectList();
		_consummateLevelRelatedMainAttributesPenetrations = new SpecialEffectList();
		_consummateLevelRelatedMainAttributesPenetrationResists = new SpecialEffectList();
		_skillAlsoAsFiveElements = new SpecialEffectList();
		_innerInjuryImmunity = new SpecialEffectList();
		_outerInjuryImmunity = new SpecialEffectList();
		_poisonAffectThreshold = new SpecialEffectList();
		_lockDistance = new SpecialEffectList();
		_resistOfAllPoison = new SpecialEffectList();
		_makePoisonTarget = new SpecialEffectList();
		_acceptPoisonTarget = new SpecialEffectList();
		_certainCriticalHit = new SpecialEffectList();
		_mindMarkCount = new SpecialEffectList();
		_canFightBackWithHit = new SpecialEffectList();
		_inevitableHit = new SpecialEffectList();
		_attackCanPursue = new SpecialEffectList();
		_combatSkillDataEffectList = new SpecialEffectList();
		_criticalOdds = new SpecialEffectList();
		_stanceCostByEffect = new SpecialEffectList();
		_breathCostByEffect = new SpecialEffectList();
		_powerAddRatio = new SpecialEffectList();
		_powerReduceRatio = new SpecialEffectList();
		_poisonAffectProduceValue = new SpecialEffectList();
		_canReadingOnMonthChange = new SpecialEffectList();
		_medicineEffect = new SpecialEffectList();
		_xiangshuInfectionDelta = new SpecialEffectList();
		_healthDelta = new SpecialEffectList();
		_weaponSilenceFrame = new SpecialEffectList();
		_silenceFrame = new SpecialEffectList();
		_currAgeDelta = new SpecialEffectList();
		_goneMadInAllBreak = new SpecialEffectList();
		_makeLoveRateOnMonthChange = new SpecialEffectList();
		_canAutoHealOnMonthChange = new SpecialEffectList();
		_happinessDelta = new SpecialEffectList();
		_teammateCmdCanUse = new SpecialEffectList();
		_mixPoisonInfinityAffect = new SpecialEffectList();
		_attackRangeMaxAcupoint = new SpecialEffectList();
		_maxMobilityPercent = new SpecialEffectList();
		_makeMindDamage = new SpecialEffectList();
		_acceptMindDamage = new SpecialEffectList();
		_hitAddByTempValue = new SpecialEffectList();
		_avoidAddByTempValue = new SpecialEffectList();
		_ignoreEquipmentOverload = new SpecialEffectList();
		_canCostEnemyUsableTricks = new SpecialEffectList();
		_ignoreArmor = new SpecialEffectList();
		_unyieldingFallen = new SpecialEffectList();
		_normalAttackPrepareFrame = new SpecialEffectList();
		_canCostUselessTricks = new SpecialEffectList();
		_defendSkillCanAffect = new SpecialEffectList();
		_assistSkillCanAffect = new SpecialEffectList();
		_agileSkillCanAffect = new SpecialEffectList();
		_allMarkChangeToMind = new SpecialEffectList();
		_mindMarkChangeToFatal = new SpecialEffectList();
		_canCast = new SpecialEffectList();
		_inevitableAvoid = new SpecialEffectList();
		_powerEffectReverse = new SpecialEffectList();
		_featureBonusReverse = new SpecialEffectList();
		_wugFatalDamageValue = new SpecialEffectList();
		_canRecoverHealthOnMonthChange = new SpecialEffectList();
		_takeRevengeRateOnMonthChange = new SpecialEffectList();
		_consummateLevelBonus = new SpecialEffectList();
		_neiliDelta = new SpecialEffectList();
		_canMakeLoveSpecialOnMonthChange = new SpecialEffectList();
		_healAcupointSpeed = new SpecialEffectList();
		_maxChangeTrickCount = new SpecialEffectList();
		_convertCostBreathAndStance = new SpecialEffectList();
		_personalitiesAll = new SpecialEffectList();
		_finalFatalDamageMarkCount = new SpecialEffectList();
		_infinityMindMarkProgress = new SpecialEffectList();
		_combatSkillAiScorePower = new SpecialEffectList();
		_normalAttackChangeToUnlockAttack = new SpecialEffectList();
		_attackBodyPartOdds = new SpecialEffectList();
		_changeDurability = new SpecialEffectList();
		_equipmentBonus = new SpecialEffectList();
		_equipmentWeight = new SpecialEffectList();
		_rawCreateEffectList = new SpecialEffectList();
		_jiTrickAsWeaponTrickCount = new SpecialEffectList();
		_uselessTrickAsJiTrickCount = new SpecialEffectList();
		_equipmentPower = new SpecialEffectList();
		_healFlawSpeed = new SpecialEffectList();
		_unlockSpeed = new SpecialEffectList();
		_flawBonusFactor = new SpecialEffectList();
		_canCostShaTricks = new SpecialEffectList();
		_defenderDirectFinalDamageValue = new SpecialEffectList();
		_normalAttackRecoveryFrame = new SpecialEffectList();
		_finalGoneMadInjury = new SpecialEffectList();
		_attackerDirectFinalDamageValue = new SpecialEffectList();
		_canCostTrickDuringPreparingSkill = new SpecialEffectList();
		_validItemList = new SpecialEffectList();
		_acceptDamageCanAdd = new SpecialEffectList();
		_makeDamageCanReduce = new SpecialEffectList();
		_normalAttackGetTrickCount = new SpecialEffectList();
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 1316;
		int serializedSize = _maxStrength.GetSerializedSize();
		num += serializedSize;
		int serializedSize2 = _maxDexterity.GetSerializedSize();
		num += serializedSize2;
		int serializedSize3 = _maxConcentration.GetSerializedSize();
		num += serializedSize3;
		int serializedSize4 = _maxVitality.GetSerializedSize();
		num += serializedSize4;
		int serializedSize5 = _maxEnergy.GetSerializedSize();
		num += serializedSize5;
		int serializedSize6 = _maxIntelligence.GetSerializedSize();
		num += serializedSize6;
		int serializedSize7 = _recoveryOfStance.GetSerializedSize();
		num += serializedSize7;
		int serializedSize8 = _recoveryOfBreath.GetSerializedSize();
		num += serializedSize8;
		int serializedSize9 = _moveSpeed.GetSerializedSize();
		num += serializedSize9;
		int serializedSize10 = _recoveryOfFlaw.GetSerializedSize();
		num += serializedSize10;
		int serializedSize11 = _castSpeed.GetSerializedSize();
		num += serializedSize11;
		int serializedSize12 = _recoveryOfBlockedAcupoint.GetSerializedSize();
		num += serializedSize12;
		int serializedSize13 = _weaponSwitchSpeed.GetSerializedSize();
		num += serializedSize13;
		int serializedSize14 = _attackSpeed.GetSerializedSize();
		num += serializedSize14;
		int serializedSize15 = _innerRatio.GetSerializedSize();
		num += serializedSize15;
		int serializedSize16 = _recoveryOfQiDisorder.GetSerializedSize();
		num += serializedSize16;
		int serializedSize17 = _minorAttributeFixMaxValue.GetSerializedSize();
		num += serializedSize17;
		int serializedSize18 = _minorAttributeFixMinValue.GetSerializedSize();
		num += serializedSize18;
		int serializedSize19 = _resistOfHotPoison.GetSerializedSize();
		num += serializedSize19;
		int serializedSize20 = _resistOfGloomyPoison.GetSerializedSize();
		num += serializedSize20;
		int serializedSize21 = _resistOfColdPoison.GetSerializedSize();
		num += serializedSize21;
		int serializedSize22 = _resistOfRedPoison.GetSerializedSize();
		num += serializedSize22;
		int serializedSize23 = _resistOfRottenPoison.GetSerializedSize();
		num += serializedSize23;
		int serializedSize24 = _resistOfIllusoryPoison.GetSerializedSize();
		num += serializedSize24;
		int serializedSize25 = _displayAge.GetSerializedSize();
		num += serializedSize25;
		int serializedSize26 = _neiliProportionOfFiveElements.GetSerializedSize();
		num += serializedSize26;
		int serializedSize27 = _weaponMaxPower.GetSerializedSize();
		num += serializedSize27;
		int serializedSize28 = _weaponUseRequirement.GetSerializedSize();
		num += serializedSize28;
		int serializedSize29 = _weaponAttackRange.GetSerializedSize();
		num += serializedSize29;
		int serializedSize30 = _armorMaxPower.GetSerializedSize();
		num += serializedSize30;
		int serializedSize31 = _armorUseRequirement.GetSerializedSize();
		num += serializedSize31;
		int serializedSize32 = _hitStrength.GetSerializedSize();
		num += serializedSize32;
		int serializedSize33 = _hitTechnique.GetSerializedSize();
		num += serializedSize33;
		int serializedSize34 = _hitSpeed.GetSerializedSize();
		num += serializedSize34;
		int serializedSize35 = _hitMind.GetSerializedSize();
		num += serializedSize35;
		int serializedSize36 = _hitCanChange.GetSerializedSize();
		num += serializedSize36;
		int serializedSize37 = _hitChangeEffectPercent.GetSerializedSize();
		num += serializedSize37;
		int serializedSize38 = _avoidStrength.GetSerializedSize();
		num += serializedSize38;
		int serializedSize39 = _avoidTechnique.GetSerializedSize();
		num += serializedSize39;
		int serializedSize40 = _avoidSpeed.GetSerializedSize();
		num += serializedSize40;
		int serializedSize41 = _avoidMind.GetSerializedSize();
		num += serializedSize41;
		int serializedSize42 = _avoidCanChange.GetSerializedSize();
		num += serializedSize42;
		int serializedSize43 = _avoidChangeEffectPercent.GetSerializedSize();
		num += serializedSize43;
		int serializedSize44 = _penetrateOuter.GetSerializedSize();
		num += serializedSize44;
		int serializedSize45 = _penetrateInner.GetSerializedSize();
		num += serializedSize45;
		int serializedSize46 = _penetrateResistOuter.GetSerializedSize();
		num += serializedSize46;
		int serializedSize47 = _penetrateResistInner.GetSerializedSize();
		num += serializedSize47;
		int serializedSize48 = _neiliAllocationAttack.GetSerializedSize();
		num += serializedSize48;
		int serializedSize49 = _neiliAllocationAgile.GetSerializedSize();
		num += serializedSize49;
		int serializedSize50 = _neiliAllocationDefense.GetSerializedSize();
		num += serializedSize50;
		int serializedSize51 = _neiliAllocationAssist.GetSerializedSize();
		num += serializedSize51;
		int serializedSize52 = _happiness.GetSerializedSize();
		num += serializedSize52;
		int serializedSize53 = _maxHealth.GetSerializedSize();
		num += serializedSize53;
		int serializedSize54 = _healthCost.GetSerializedSize();
		num += serializedSize54;
		int serializedSize55 = _moveSpeedCanChange.GetSerializedSize();
		num += serializedSize55;
		int serializedSize56 = _attackerHitStrength.GetSerializedSize();
		num += serializedSize56;
		int serializedSize57 = _attackerHitTechnique.GetSerializedSize();
		num += serializedSize57;
		int serializedSize58 = _attackerHitSpeed.GetSerializedSize();
		num += serializedSize58;
		int serializedSize59 = _attackerHitMind.GetSerializedSize();
		num += serializedSize59;
		int serializedSize60 = _attackerAvoidStrength.GetSerializedSize();
		num += serializedSize60;
		int serializedSize61 = _attackerAvoidTechnique.GetSerializedSize();
		num += serializedSize61;
		int serializedSize62 = _attackerAvoidSpeed.GetSerializedSize();
		num += serializedSize62;
		int serializedSize63 = _attackerAvoidMind.GetSerializedSize();
		num += serializedSize63;
		int serializedSize64 = _attackerPenetrateOuter.GetSerializedSize();
		num += serializedSize64;
		int serializedSize65 = _attackerPenetrateInner.GetSerializedSize();
		num += serializedSize65;
		int serializedSize66 = _attackerPenetrateResistOuter.GetSerializedSize();
		num += serializedSize66;
		int serializedSize67 = _attackerPenetrateResistInner.GetSerializedSize();
		num += serializedSize67;
		int serializedSize68 = _attackHitType.GetSerializedSize();
		num += serializedSize68;
		int serializedSize69 = _makeDirectDamage.GetSerializedSize();
		num += serializedSize69;
		int serializedSize70 = _makeBounceDamage.GetSerializedSize();
		num += serializedSize70;
		int serializedSize71 = _makeFightBackDamage.GetSerializedSize();
		num += serializedSize71;
		int serializedSize72 = _makePoisonLevel.GetSerializedSize();
		num += serializedSize72;
		int serializedSize73 = _makePoisonValue.GetSerializedSize();
		num += serializedSize73;
		int serializedSize74 = _attackerHitOdds.GetSerializedSize();
		num += serializedSize74;
		int serializedSize75 = _attackerFightBackHitOdds.GetSerializedSize();
		num += serializedSize75;
		int serializedSize76 = _attackerPursueOdds.GetSerializedSize();
		num += serializedSize76;
		int serializedSize77 = _makedInjuryChangeToOld.GetSerializedSize();
		num += serializedSize77;
		int serializedSize78 = _makedPoisonChangeToOld.GetSerializedSize();
		num += serializedSize78;
		int serializedSize79 = _makeDamageType.GetSerializedSize();
		num += serializedSize79;
		int serializedSize80 = _canMakeInjuryToNoInjuryPart.GetSerializedSize();
		num += serializedSize80;
		int serializedSize81 = _makePoisonType.GetSerializedSize();
		num += serializedSize81;
		int serializedSize82 = _normalAttackWeapon.GetSerializedSize();
		num += serializedSize82;
		int serializedSize83 = _normalAttackTrick.GetSerializedSize();
		num += serializedSize83;
		int serializedSize84 = _extraFlawCount.GetSerializedSize();
		num += serializedSize84;
		int serializedSize85 = _attackCanBounce.GetSerializedSize();
		num += serializedSize85;
		int serializedSize86 = _attackCanFightBack.GetSerializedSize();
		num += serializedSize86;
		int serializedSize87 = _makeFightBackInjuryMark.GetSerializedSize();
		num += serializedSize87;
		int serializedSize88 = _legSkillUseShoes.GetSerializedSize();
		num += serializedSize88;
		int serializedSize89 = _attackerFinalDamageValue.GetSerializedSize();
		num += serializedSize89;
		int serializedSize90 = _defenderHitStrength.GetSerializedSize();
		num += serializedSize90;
		int serializedSize91 = _defenderHitTechnique.GetSerializedSize();
		num += serializedSize91;
		int serializedSize92 = _defenderHitSpeed.GetSerializedSize();
		num += serializedSize92;
		int serializedSize93 = _defenderHitMind.GetSerializedSize();
		num += serializedSize93;
		int serializedSize94 = _defenderAvoidStrength.GetSerializedSize();
		num += serializedSize94;
		int serializedSize95 = _defenderAvoidTechnique.GetSerializedSize();
		num += serializedSize95;
		int serializedSize96 = _defenderAvoidSpeed.GetSerializedSize();
		num += serializedSize96;
		int serializedSize97 = _defenderAvoidMind.GetSerializedSize();
		num += serializedSize97;
		int serializedSize98 = _defenderPenetrateOuter.GetSerializedSize();
		num += serializedSize98;
		int serializedSize99 = _defenderPenetrateInner.GetSerializedSize();
		num += serializedSize99;
		int serializedSize100 = _defenderPenetrateResistOuter.GetSerializedSize();
		num += serializedSize100;
		int serializedSize101 = _defenderPenetrateResistInner.GetSerializedSize();
		num += serializedSize101;
		int serializedSize102 = _acceptDirectDamage.GetSerializedSize();
		num += serializedSize102;
		int serializedSize103 = _acceptBounceDamage.GetSerializedSize();
		num += serializedSize103;
		int serializedSize104 = _acceptFightBackDamage.GetSerializedSize();
		num += serializedSize104;
		int serializedSize105 = _acceptPoisonLevel.GetSerializedSize();
		num += serializedSize105;
		int serializedSize106 = _acceptPoisonValue.GetSerializedSize();
		num += serializedSize106;
		int serializedSize107 = _defenderHitOdds.GetSerializedSize();
		num += serializedSize107;
		int serializedSize108 = _defenderFightBackHitOdds.GetSerializedSize();
		num += serializedSize108;
		int serializedSize109 = _defenderPursueOdds.GetSerializedSize();
		num += serializedSize109;
		int serializedSize110 = _acceptMaxInjuryCount.GetSerializedSize();
		num += serializedSize110;
		int serializedSize111 = _bouncePower.GetSerializedSize();
		num += serializedSize111;
		int serializedSize112 = _fightBackPower.GetSerializedSize();
		num += serializedSize112;
		int serializedSize113 = _directDamageInnerRatio.GetSerializedSize();
		num += serializedSize113;
		int serializedSize114 = _defenderFinalDamageValue.GetSerializedSize();
		num += serializedSize114;
		int serializedSize115 = _directDamageValue.GetSerializedSize();
		num += serializedSize115;
		int serializedSize116 = _directInjuryMark.GetSerializedSize();
		num += serializedSize116;
		int serializedSize117 = _goneMadInjury.GetSerializedSize();
		num += serializedSize117;
		int serializedSize118 = _healInjurySpeed.GetSerializedSize();
		num += serializedSize118;
		int serializedSize119 = _healInjuryBuff.GetSerializedSize();
		num += serializedSize119;
		int serializedSize120 = _healInjuryDebuff.GetSerializedSize();
		num += serializedSize120;
		int serializedSize121 = _healPoisonSpeed.GetSerializedSize();
		num += serializedSize121;
		int serializedSize122 = _healPoisonBuff.GetSerializedSize();
		num += serializedSize122;
		int serializedSize123 = _healPoisonDebuff.GetSerializedSize();
		num += serializedSize123;
		int serializedSize124 = _fleeSpeed.GetSerializedSize();
		num += serializedSize124;
		int serializedSize125 = _maxFlawCount.GetSerializedSize();
		num += serializedSize125;
		int serializedSize126 = _canAddFlaw.GetSerializedSize();
		num += serializedSize126;
		int serializedSize127 = _flawLevel.GetSerializedSize();
		num += serializedSize127;
		int serializedSize128 = _flawLevelCanReduce.GetSerializedSize();
		num += serializedSize128;
		int serializedSize129 = _flawCount.GetSerializedSize();
		num += serializedSize129;
		int serializedSize130 = _maxAcupointCount.GetSerializedSize();
		num += serializedSize130;
		int serializedSize131 = _canAddAcupoint.GetSerializedSize();
		num += serializedSize131;
		int serializedSize132 = _acupointLevel.GetSerializedSize();
		num += serializedSize132;
		int serializedSize133 = _acupointLevelCanReduce.GetSerializedSize();
		num += serializedSize133;
		int serializedSize134 = _acupointCount.GetSerializedSize();
		num += serializedSize134;
		int serializedSize135 = _addNeiliAllocation.GetSerializedSize();
		num += serializedSize135;
		int serializedSize136 = _costNeiliAllocation.GetSerializedSize();
		num += serializedSize136;
		int serializedSize137 = _canChangeNeiliAllocation.GetSerializedSize();
		num += serializedSize137;
		int serializedSize138 = _canGetTrick.GetSerializedSize();
		num += serializedSize138;
		int serializedSize139 = _getTrickType.GetSerializedSize();
		num += serializedSize139;
		int serializedSize140 = _attackBodyPart.GetSerializedSize();
		num += serializedSize140;
		int serializedSize141 = _weaponEquipAttack.GetSerializedSize();
		num += serializedSize141;
		int serializedSize142 = _weaponEquipDefense.GetSerializedSize();
		num += serializedSize142;
		int serializedSize143 = _armorEquipAttack.GetSerializedSize();
		num += serializedSize143;
		int serializedSize144 = _armorEquipDefense.GetSerializedSize();
		num += serializedSize144;
		int serializedSize145 = _attackRangeForward.GetSerializedSize();
		num += serializedSize145;
		int serializedSize146 = _attackRangeBackward.GetSerializedSize();
		num += serializedSize146;
		int serializedSize147 = _moveCanBeStopped.GetSerializedSize();
		num += serializedSize147;
		int serializedSize148 = _canForcedMove.GetSerializedSize();
		num += serializedSize148;
		int serializedSize149 = _mobilityCanBeRemoved.GetSerializedSize();
		num += serializedSize149;
		int serializedSize150 = _mobilityCostByEffect.GetSerializedSize();
		num += serializedSize150;
		int serializedSize151 = _moveDistance.GetSerializedSize();
		num += serializedSize151;
		int serializedSize152 = _jumpPrepareFrame.GetSerializedSize();
		num += serializedSize152;
		int serializedSize153 = _bounceInjuryMark.GetSerializedSize();
		num += serializedSize153;
		int serializedSize154 = _skillHasCost.GetSerializedSize();
		num += serializedSize154;
		int serializedSize155 = _combatStateEffect.GetSerializedSize();
		num += serializedSize155;
		int serializedSize156 = _changeNeedUseSkill.GetSerializedSize();
		num += serializedSize156;
		int serializedSize157 = _changeDistanceIsMove.GetSerializedSize();
		num += serializedSize157;
		int serializedSize158 = _replaceCharHit.GetSerializedSize();
		num += serializedSize158;
		int serializedSize159 = _canAddPoison.GetSerializedSize();
		num += serializedSize159;
		int serializedSize160 = _canReducePoison.GetSerializedSize();
		num += serializedSize160;
		int serializedSize161 = _reducePoisonValue.GetSerializedSize();
		num += serializedSize161;
		int serializedSize162 = _poisonCanAffect.GetSerializedSize();
		num += serializedSize162;
		int serializedSize163 = _poisonAffectCount.GetSerializedSize();
		num += serializedSize163;
		int serializedSize164 = _costTricks.GetSerializedSize();
		num += serializedSize164;
		int serializedSize165 = _jumpMoveDistance.GetSerializedSize();
		num += serializedSize165;
		int serializedSize166 = _combatStateToAdd.GetSerializedSize();
		num += serializedSize166;
		int serializedSize167 = _combatStatePower.GetSerializedSize();
		num += serializedSize167;
		int serializedSize168 = _breakBodyPartInjuryCount.GetSerializedSize();
		num += serializedSize168;
		int serializedSize169 = _bodyPartIsBroken.GetSerializedSize();
		num += serializedSize169;
		int serializedSize170 = _maxTrickCount.GetSerializedSize();
		num += serializedSize170;
		int serializedSize171 = _maxBreathPercent.GetSerializedSize();
		num += serializedSize171;
		int serializedSize172 = _maxStancePercent.GetSerializedSize();
		num += serializedSize172;
		int serializedSize173 = _extraBreathPercent.GetSerializedSize();
		num += serializedSize173;
		int serializedSize174 = _extraStancePercent.GetSerializedSize();
		num += serializedSize174;
		int serializedSize175 = _moveCostMobility.GetSerializedSize();
		num += serializedSize175;
		int serializedSize176 = _defendSkillKeepTime.GetSerializedSize();
		num += serializedSize176;
		int serializedSize177 = _bounceRange.GetSerializedSize();
		num += serializedSize177;
		int serializedSize178 = _mindMarkKeepTime.GetSerializedSize();
		num += serializedSize178;
		int serializedSize179 = _skillMobilityCostPerFrame.GetSerializedSize();
		num += serializedSize179;
		int serializedSize180 = _canAddWug.GetSerializedSize();
		num += serializedSize180;
		int serializedSize181 = _hasGodWeaponBuff.GetSerializedSize();
		num += serializedSize181;
		int serializedSize182 = _hasGodArmorBuff.GetSerializedSize();
		num += serializedSize182;
		int serializedSize183 = _teammateCmdRequireGenerateValue.GetSerializedSize();
		num += serializedSize183;
		int serializedSize184 = _teammateCmdEffect.GetSerializedSize();
		num += serializedSize184;
		int serializedSize185 = _flawRecoverSpeed.GetSerializedSize();
		num += serializedSize185;
		int serializedSize186 = _acupointRecoverSpeed.GetSerializedSize();
		num += serializedSize186;
		int serializedSize187 = _mindMarkRecoverSpeed.GetSerializedSize();
		num += serializedSize187;
		int serializedSize188 = _injuryAutoHealSpeed.GetSerializedSize();
		num += serializedSize188;
		int serializedSize189 = _canRecoverBreath.GetSerializedSize();
		num += serializedSize189;
		int serializedSize190 = _canRecoverStance.GetSerializedSize();
		num += serializedSize190;
		int serializedSize191 = _fatalDamageValue.GetSerializedSize();
		num += serializedSize191;
		int serializedSize192 = _fatalDamageMarkCount.GetSerializedSize();
		num += serializedSize192;
		int serializedSize193 = _canFightBackDuringPrepareSkill.GetSerializedSize();
		num += serializedSize193;
		int serializedSize194 = _skillPrepareSpeed.GetSerializedSize();
		num += serializedSize194;
		int serializedSize195 = _breathRecoverSpeed.GetSerializedSize();
		num += serializedSize195;
		int serializedSize196 = _stanceRecoverSpeed.GetSerializedSize();
		num += serializedSize196;
		int serializedSize197 = _mobilityRecoverSpeed.GetSerializedSize();
		num += serializedSize197;
		int serializedSize198 = _changeTrickProgressAddValue.GetSerializedSize();
		num += serializedSize198;
		int serializedSize199 = _power.GetSerializedSize();
		num += serializedSize199;
		int serializedSize200 = _maxPower.GetSerializedSize();
		num += serializedSize200;
		int serializedSize201 = _powerCanReduce.GetSerializedSize();
		num += serializedSize201;
		int serializedSize202 = _useRequirement.GetSerializedSize();
		num += serializedSize202;
		int serializedSize203 = _currInnerRatio.GetSerializedSize();
		num += serializedSize203;
		int serializedSize204 = _costBreathAndStance.GetSerializedSize();
		num += serializedSize204;
		int serializedSize205 = _costBreath.GetSerializedSize();
		num += serializedSize205;
		int serializedSize206 = _costStance.GetSerializedSize();
		num += serializedSize206;
		int serializedSize207 = _costMobility.GetSerializedSize();
		num += serializedSize207;
		int serializedSize208 = _skillCostTricks.GetSerializedSize();
		num += serializedSize208;
		int serializedSize209 = _effectDirection.GetSerializedSize();
		num += serializedSize209;
		int serializedSize210 = _effectDirectionCanChange.GetSerializedSize();
		num += serializedSize210;
		int serializedSize211 = _gridCost.GetSerializedSize();
		num += serializedSize211;
		int serializedSize212 = _prepareTotalProgress.GetSerializedSize();
		num += serializedSize212;
		int serializedSize213 = _specificGridCount.GetSerializedSize();
		num += serializedSize213;
		int serializedSize214 = _genericGridCount.GetSerializedSize();
		num += serializedSize214;
		int serializedSize215 = _canInterrupt.GetSerializedSize();
		num += serializedSize215;
		int serializedSize216 = _interruptOdds.GetSerializedSize();
		num += serializedSize216;
		int serializedSize217 = _canSilence.GetSerializedSize();
		num += serializedSize217;
		int serializedSize218 = _silenceOdds.GetSerializedSize();
		num += serializedSize218;
		int serializedSize219 = _canCastWithBrokenBodyPart.GetSerializedSize();
		num += serializedSize219;
		int serializedSize220 = _addPowerCanBeRemoved.GetSerializedSize();
		num += serializedSize220;
		int serializedSize221 = _skillType.GetSerializedSize();
		num += serializedSize221;
		int serializedSize222 = _effectCountCanChange.GetSerializedSize();
		num += serializedSize222;
		int serializedSize223 = _canCastInDefend.GetSerializedSize();
		num += serializedSize223;
		int serializedSize224 = _hitDistribution.GetSerializedSize();
		num += serializedSize224;
		int serializedSize225 = _canCastOnLackBreath.GetSerializedSize();
		num += serializedSize225;
		int serializedSize226 = _canCastOnLackStance.GetSerializedSize();
		num += serializedSize226;
		int serializedSize227 = _costBreathOnCast.GetSerializedSize();
		num += serializedSize227;
		int serializedSize228 = _costStanceOnCast.GetSerializedSize();
		num += serializedSize228;
		int serializedSize229 = _canUseMobilityAsBreath.GetSerializedSize();
		num += serializedSize229;
		int serializedSize230 = _canUseMobilityAsStance.GetSerializedSize();
		num += serializedSize230;
		int serializedSize231 = _castCostNeiliAllocation.GetSerializedSize();
		num += serializedSize231;
		int serializedSize232 = _acceptPoisonResist.GetSerializedSize();
		num += serializedSize232;
		int serializedSize233 = _makePoisonResist.GetSerializedSize();
		num += serializedSize233;
		int serializedSize234 = _canCriticalHit.GetSerializedSize();
		num += serializedSize234;
		int serializedSize235 = _canCostNeiliAllocationEffect.GetSerializedSize();
		num += serializedSize235;
		int serializedSize236 = _consummateLevelRelatedMainAttributesHitValues.GetSerializedSize();
		num += serializedSize236;
		int serializedSize237 = _consummateLevelRelatedMainAttributesAvoidValues.GetSerializedSize();
		num += serializedSize237;
		int serializedSize238 = _consummateLevelRelatedMainAttributesPenetrations.GetSerializedSize();
		num += serializedSize238;
		int serializedSize239 = _consummateLevelRelatedMainAttributesPenetrationResists.GetSerializedSize();
		num += serializedSize239;
		int serializedSize240 = _skillAlsoAsFiveElements.GetSerializedSize();
		num += serializedSize240;
		int serializedSize241 = _innerInjuryImmunity.GetSerializedSize();
		num += serializedSize241;
		int serializedSize242 = _outerInjuryImmunity.GetSerializedSize();
		num += serializedSize242;
		int serializedSize243 = _poisonAffectThreshold.GetSerializedSize();
		num += serializedSize243;
		int serializedSize244 = _lockDistance.GetSerializedSize();
		num += serializedSize244;
		int serializedSize245 = _resistOfAllPoison.GetSerializedSize();
		num += serializedSize245;
		int serializedSize246 = _makePoisonTarget.GetSerializedSize();
		num += serializedSize246;
		int serializedSize247 = _acceptPoisonTarget.GetSerializedSize();
		num += serializedSize247;
		int serializedSize248 = _certainCriticalHit.GetSerializedSize();
		num += serializedSize248;
		int serializedSize249 = _mindMarkCount.GetSerializedSize();
		num += serializedSize249;
		int serializedSize250 = _canFightBackWithHit.GetSerializedSize();
		num += serializedSize250;
		int serializedSize251 = _inevitableHit.GetSerializedSize();
		num += serializedSize251;
		int serializedSize252 = _attackCanPursue.GetSerializedSize();
		num += serializedSize252;
		int serializedSize253 = _combatSkillDataEffectList.GetSerializedSize();
		num += serializedSize253;
		int serializedSize254 = _criticalOdds.GetSerializedSize();
		num += serializedSize254;
		int serializedSize255 = _stanceCostByEffect.GetSerializedSize();
		num += serializedSize255;
		int serializedSize256 = _breathCostByEffect.GetSerializedSize();
		num += serializedSize256;
		int serializedSize257 = _powerAddRatio.GetSerializedSize();
		num += serializedSize257;
		int serializedSize258 = _powerReduceRatio.GetSerializedSize();
		num += serializedSize258;
		int serializedSize259 = _poisonAffectProduceValue.GetSerializedSize();
		num += serializedSize259;
		int serializedSize260 = _canReadingOnMonthChange.GetSerializedSize();
		num += serializedSize260;
		int serializedSize261 = _medicineEffect.GetSerializedSize();
		num += serializedSize261;
		int serializedSize262 = _xiangshuInfectionDelta.GetSerializedSize();
		num += serializedSize262;
		int serializedSize263 = _healthDelta.GetSerializedSize();
		num += serializedSize263;
		int serializedSize264 = _weaponSilenceFrame.GetSerializedSize();
		num += serializedSize264;
		int serializedSize265 = _silenceFrame.GetSerializedSize();
		num += serializedSize265;
		int serializedSize266 = _currAgeDelta.GetSerializedSize();
		num += serializedSize266;
		int serializedSize267 = _goneMadInAllBreak.GetSerializedSize();
		num += serializedSize267;
		int serializedSize268 = _makeLoveRateOnMonthChange.GetSerializedSize();
		num += serializedSize268;
		int serializedSize269 = _canAutoHealOnMonthChange.GetSerializedSize();
		num += serializedSize269;
		int serializedSize270 = _happinessDelta.GetSerializedSize();
		num += serializedSize270;
		int serializedSize271 = _teammateCmdCanUse.GetSerializedSize();
		num += serializedSize271;
		int serializedSize272 = _mixPoisonInfinityAffect.GetSerializedSize();
		num += serializedSize272;
		int serializedSize273 = _attackRangeMaxAcupoint.GetSerializedSize();
		num += serializedSize273;
		int serializedSize274 = _maxMobilityPercent.GetSerializedSize();
		num += serializedSize274;
		int serializedSize275 = _makeMindDamage.GetSerializedSize();
		num += serializedSize275;
		int serializedSize276 = _acceptMindDamage.GetSerializedSize();
		num += serializedSize276;
		int serializedSize277 = _hitAddByTempValue.GetSerializedSize();
		num += serializedSize277;
		int serializedSize278 = _avoidAddByTempValue.GetSerializedSize();
		num += serializedSize278;
		int serializedSize279 = _ignoreEquipmentOverload.GetSerializedSize();
		num += serializedSize279;
		int serializedSize280 = _canCostEnemyUsableTricks.GetSerializedSize();
		num += serializedSize280;
		int serializedSize281 = _ignoreArmor.GetSerializedSize();
		num += serializedSize281;
		int serializedSize282 = _unyieldingFallen.GetSerializedSize();
		num += serializedSize282;
		int serializedSize283 = _normalAttackPrepareFrame.GetSerializedSize();
		num += serializedSize283;
		int serializedSize284 = _canCostUselessTricks.GetSerializedSize();
		num += serializedSize284;
		int serializedSize285 = _defendSkillCanAffect.GetSerializedSize();
		num += serializedSize285;
		int serializedSize286 = _assistSkillCanAffect.GetSerializedSize();
		num += serializedSize286;
		int serializedSize287 = _agileSkillCanAffect.GetSerializedSize();
		num += serializedSize287;
		int serializedSize288 = _allMarkChangeToMind.GetSerializedSize();
		num += serializedSize288;
		int serializedSize289 = _mindMarkChangeToFatal.GetSerializedSize();
		num += serializedSize289;
		int serializedSize290 = _canCast.GetSerializedSize();
		num += serializedSize290;
		int serializedSize291 = _inevitableAvoid.GetSerializedSize();
		num += serializedSize291;
		int serializedSize292 = _powerEffectReverse.GetSerializedSize();
		num += serializedSize292;
		int serializedSize293 = _featureBonusReverse.GetSerializedSize();
		num += serializedSize293;
		int serializedSize294 = _wugFatalDamageValue.GetSerializedSize();
		num += serializedSize294;
		int serializedSize295 = _canRecoverHealthOnMonthChange.GetSerializedSize();
		num += serializedSize295;
		int serializedSize296 = _takeRevengeRateOnMonthChange.GetSerializedSize();
		num += serializedSize296;
		int serializedSize297 = _consummateLevelBonus.GetSerializedSize();
		num += serializedSize297;
		int serializedSize298 = _neiliDelta.GetSerializedSize();
		num += serializedSize298;
		int serializedSize299 = _canMakeLoveSpecialOnMonthChange.GetSerializedSize();
		num += serializedSize299;
		int serializedSize300 = _healAcupointSpeed.GetSerializedSize();
		num += serializedSize300;
		int serializedSize301 = _maxChangeTrickCount.GetSerializedSize();
		num += serializedSize301;
		int serializedSize302 = _convertCostBreathAndStance.GetSerializedSize();
		num += serializedSize302;
		int serializedSize303 = _personalitiesAll.GetSerializedSize();
		num += serializedSize303;
		int serializedSize304 = _finalFatalDamageMarkCount.GetSerializedSize();
		num += serializedSize304;
		int serializedSize305 = _infinityMindMarkProgress.GetSerializedSize();
		num += serializedSize305;
		int serializedSize306 = _combatSkillAiScorePower.GetSerializedSize();
		num += serializedSize306;
		int serializedSize307 = _normalAttackChangeToUnlockAttack.GetSerializedSize();
		num += serializedSize307;
		int serializedSize308 = _attackBodyPartOdds.GetSerializedSize();
		num += serializedSize308;
		int serializedSize309 = _changeDurability.GetSerializedSize();
		num += serializedSize309;
		int serializedSize310 = _equipmentBonus.GetSerializedSize();
		num += serializedSize310;
		int serializedSize311 = _equipmentWeight.GetSerializedSize();
		num += serializedSize311;
		int serializedSize312 = _rawCreateEffectList.GetSerializedSize();
		num += serializedSize312;
		int serializedSize313 = _jiTrickAsWeaponTrickCount.GetSerializedSize();
		num += serializedSize313;
		int serializedSize314 = _uselessTrickAsJiTrickCount.GetSerializedSize();
		num += serializedSize314;
		int serializedSize315 = _equipmentPower.GetSerializedSize();
		num += serializedSize315;
		int serializedSize316 = _healFlawSpeed.GetSerializedSize();
		num += serializedSize316;
		int serializedSize317 = _unlockSpeed.GetSerializedSize();
		num += serializedSize317;
		int serializedSize318 = _flawBonusFactor.GetSerializedSize();
		num += serializedSize318;
		int serializedSize319 = _canCostShaTricks.GetSerializedSize();
		num += serializedSize319;
		int serializedSize320 = _defenderDirectFinalDamageValue.GetSerializedSize();
		num += serializedSize320;
		int serializedSize321 = _normalAttackRecoveryFrame.GetSerializedSize();
		num += serializedSize321;
		int serializedSize322 = _finalGoneMadInjury.GetSerializedSize();
		num += serializedSize322;
		int serializedSize323 = _attackerDirectFinalDamageValue.GetSerializedSize();
		num += serializedSize323;
		int serializedSize324 = _canCostTrickDuringPreparingSkill.GetSerializedSize();
		num += serializedSize324;
		int serializedSize325 = _validItemList.GetSerializedSize();
		num += serializedSize325;
		int serializedSize326 = _acceptDamageCanAdd.GetSerializedSize();
		num += serializedSize326;
		int serializedSize327 = _makeDamageCanReduce.GetSerializedSize();
		num += serializedSize327;
		int serializedSize328 = _normalAttackGetTrickCount.GetSerializedSize();
		return num + serializedSize328;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		*(int*)ptr = _id;
		ptr += 4;
		byte* ptr2 = ptr;
		ptr += 4;
		ptr += _maxStrength.Serialize(ptr);
		int num = (int)(ptr - ptr2 - 4);
		if (num > 4194304)
		{
			throw new Exception($"Size of field {"_maxStrength"} must be less than {4096}KB");
		}
		*(int*)ptr2 = num;
		byte* ptr3 = ptr;
		ptr += 4;
		ptr += _maxDexterity.Serialize(ptr);
		int num2 = (int)(ptr - ptr3 - 4);
		if (num2 > 4194304)
		{
			throw new Exception($"Size of field {"_maxDexterity"} must be less than {4096}KB");
		}
		*(int*)ptr3 = num2;
		byte* ptr4 = ptr;
		ptr += 4;
		ptr += _maxConcentration.Serialize(ptr);
		int num3 = (int)(ptr - ptr4 - 4);
		if (num3 > 4194304)
		{
			throw new Exception($"Size of field {"_maxConcentration"} must be less than {4096}KB");
		}
		*(int*)ptr4 = num3;
		byte* ptr5 = ptr;
		ptr += 4;
		ptr += _maxVitality.Serialize(ptr);
		int num4 = (int)(ptr - ptr5 - 4);
		if (num4 > 4194304)
		{
			throw new Exception($"Size of field {"_maxVitality"} must be less than {4096}KB");
		}
		*(int*)ptr5 = num4;
		byte* ptr6 = ptr;
		ptr += 4;
		ptr += _maxEnergy.Serialize(ptr);
		int num5 = (int)(ptr - ptr6 - 4);
		if (num5 > 4194304)
		{
			throw new Exception($"Size of field {"_maxEnergy"} must be less than {4096}KB");
		}
		*(int*)ptr6 = num5;
		byte* ptr7 = ptr;
		ptr += 4;
		ptr += _maxIntelligence.Serialize(ptr);
		int num6 = (int)(ptr - ptr7 - 4);
		if (num6 > 4194304)
		{
			throw new Exception($"Size of field {"_maxIntelligence"} must be less than {4096}KB");
		}
		*(int*)ptr7 = num6;
		byte* ptr8 = ptr;
		ptr += 4;
		ptr += _recoveryOfStance.Serialize(ptr);
		int num7 = (int)(ptr - ptr8 - 4);
		if (num7 > 4194304)
		{
			throw new Exception($"Size of field {"_recoveryOfStance"} must be less than {4096}KB");
		}
		*(int*)ptr8 = num7;
		byte* ptr9 = ptr;
		ptr += 4;
		ptr += _recoveryOfBreath.Serialize(ptr);
		int num8 = (int)(ptr - ptr9 - 4);
		if (num8 > 4194304)
		{
			throw new Exception($"Size of field {"_recoveryOfBreath"} must be less than {4096}KB");
		}
		*(int*)ptr9 = num8;
		byte* ptr10 = ptr;
		ptr += 4;
		ptr += _moveSpeed.Serialize(ptr);
		int num9 = (int)(ptr - ptr10 - 4);
		if (num9 > 4194304)
		{
			throw new Exception($"Size of field {"_moveSpeed"} must be less than {4096}KB");
		}
		*(int*)ptr10 = num9;
		byte* ptr11 = ptr;
		ptr += 4;
		ptr += _recoveryOfFlaw.Serialize(ptr);
		int num10 = (int)(ptr - ptr11 - 4);
		if (num10 > 4194304)
		{
			throw new Exception($"Size of field {"_recoveryOfFlaw"} must be less than {4096}KB");
		}
		*(int*)ptr11 = num10;
		byte* ptr12 = ptr;
		ptr += 4;
		ptr += _castSpeed.Serialize(ptr);
		int num11 = (int)(ptr - ptr12 - 4);
		if (num11 > 4194304)
		{
			throw new Exception($"Size of field {"_castSpeed"} must be less than {4096}KB");
		}
		*(int*)ptr12 = num11;
		byte* ptr13 = ptr;
		ptr += 4;
		ptr += _recoveryOfBlockedAcupoint.Serialize(ptr);
		int num12 = (int)(ptr - ptr13 - 4);
		if (num12 > 4194304)
		{
			throw new Exception($"Size of field {"_recoveryOfBlockedAcupoint"} must be less than {4096}KB");
		}
		*(int*)ptr13 = num12;
		byte* ptr14 = ptr;
		ptr += 4;
		ptr += _weaponSwitchSpeed.Serialize(ptr);
		int num13 = (int)(ptr - ptr14 - 4);
		if (num13 > 4194304)
		{
			throw new Exception($"Size of field {"_weaponSwitchSpeed"} must be less than {4096}KB");
		}
		*(int*)ptr14 = num13;
		byte* ptr15 = ptr;
		ptr += 4;
		ptr += _attackSpeed.Serialize(ptr);
		int num14 = (int)(ptr - ptr15 - 4);
		if (num14 > 4194304)
		{
			throw new Exception($"Size of field {"_attackSpeed"} must be less than {4096}KB");
		}
		*(int*)ptr15 = num14;
		byte* ptr16 = ptr;
		ptr += 4;
		ptr += _innerRatio.Serialize(ptr);
		int num15 = (int)(ptr - ptr16 - 4);
		if (num15 > 4194304)
		{
			throw new Exception($"Size of field {"_innerRatio"} must be less than {4096}KB");
		}
		*(int*)ptr16 = num15;
		byte* ptr17 = ptr;
		ptr += 4;
		ptr += _recoveryOfQiDisorder.Serialize(ptr);
		int num16 = (int)(ptr - ptr17 - 4);
		if (num16 > 4194304)
		{
			throw new Exception($"Size of field {"_recoveryOfQiDisorder"} must be less than {4096}KB");
		}
		*(int*)ptr17 = num16;
		byte* ptr18 = ptr;
		ptr += 4;
		ptr += _minorAttributeFixMaxValue.Serialize(ptr);
		int num17 = (int)(ptr - ptr18 - 4);
		if (num17 > 4194304)
		{
			throw new Exception($"Size of field {"_minorAttributeFixMaxValue"} must be less than {4096}KB");
		}
		*(int*)ptr18 = num17;
		byte* ptr19 = ptr;
		ptr += 4;
		ptr += _minorAttributeFixMinValue.Serialize(ptr);
		int num18 = (int)(ptr - ptr19 - 4);
		if (num18 > 4194304)
		{
			throw new Exception($"Size of field {"_minorAttributeFixMinValue"} must be less than {4096}KB");
		}
		*(int*)ptr19 = num18;
		byte* ptr20 = ptr;
		ptr += 4;
		ptr += _resistOfHotPoison.Serialize(ptr);
		int num19 = (int)(ptr - ptr20 - 4);
		if (num19 > 4194304)
		{
			throw new Exception($"Size of field {"_resistOfHotPoison"} must be less than {4096}KB");
		}
		*(int*)ptr20 = num19;
		byte* ptr21 = ptr;
		ptr += 4;
		ptr += _resistOfGloomyPoison.Serialize(ptr);
		int num20 = (int)(ptr - ptr21 - 4);
		if (num20 > 4194304)
		{
			throw new Exception($"Size of field {"_resistOfGloomyPoison"} must be less than {4096}KB");
		}
		*(int*)ptr21 = num20;
		byte* ptr22 = ptr;
		ptr += 4;
		ptr += _resistOfColdPoison.Serialize(ptr);
		int num21 = (int)(ptr - ptr22 - 4);
		if (num21 > 4194304)
		{
			throw new Exception($"Size of field {"_resistOfColdPoison"} must be less than {4096}KB");
		}
		*(int*)ptr22 = num21;
		byte* ptr23 = ptr;
		ptr += 4;
		ptr += _resistOfRedPoison.Serialize(ptr);
		int num22 = (int)(ptr - ptr23 - 4);
		if (num22 > 4194304)
		{
			throw new Exception($"Size of field {"_resistOfRedPoison"} must be less than {4096}KB");
		}
		*(int*)ptr23 = num22;
		byte* ptr24 = ptr;
		ptr += 4;
		ptr += _resistOfRottenPoison.Serialize(ptr);
		int num23 = (int)(ptr - ptr24 - 4);
		if (num23 > 4194304)
		{
			throw new Exception($"Size of field {"_resistOfRottenPoison"} must be less than {4096}KB");
		}
		*(int*)ptr24 = num23;
		byte* ptr25 = ptr;
		ptr += 4;
		ptr += _resistOfIllusoryPoison.Serialize(ptr);
		int num24 = (int)(ptr - ptr25 - 4);
		if (num24 > 4194304)
		{
			throw new Exception($"Size of field {"_resistOfIllusoryPoison"} must be less than {4096}KB");
		}
		*(int*)ptr25 = num24;
		byte* ptr26 = ptr;
		ptr += 4;
		ptr += _displayAge.Serialize(ptr);
		int num25 = (int)(ptr - ptr26 - 4);
		if (num25 > 4194304)
		{
			throw new Exception($"Size of field {"_displayAge"} must be less than {4096}KB");
		}
		*(int*)ptr26 = num25;
		byte* ptr27 = ptr;
		ptr += 4;
		ptr += _neiliProportionOfFiveElements.Serialize(ptr);
		int num26 = (int)(ptr - ptr27 - 4);
		if (num26 > 4194304)
		{
			throw new Exception($"Size of field {"_neiliProportionOfFiveElements"} must be less than {4096}KB");
		}
		*(int*)ptr27 = num26;
		byte* ptr28 = ptr;
		ptr += 4;
		ptr += _weaponMaxPower.Serialize(ptr);
		int num27 = (int)(ptr - ptr28 - 4);
		if (num27 > 4194304)
		{
			throw new Exception($"Size of field {"_weaponMaxPower"} must be less than {4096}KB");
		}
		*(int*)ptr28 = num27;
		byte* ptr29 = ptr;
		ptr += 4;
		ptr += _weaponUseRequirement.Serialize(ptr);
		int num28 = (int)(ptr - ptr29 - 4);
		if (num28 > 4194304)
		{
			throw new Exception($"Size of field {"_weaponUseRequirement"} must be less than {4096}KB");
		}
		*(int*)ptr29 = num28;
		byte* ptr30 = ptr;
		ptr += 4;
		ptr += _weaponAttackRange.Serialize(ptr);
		int num29 = (int)(ptr - ptr30 - 4);
		if (num29 > 4194304)
		{
			throw new Exception($"Size of field {"_weaponAttackRange"} must be less than {4096}KB");
		}
		*(int*)ptr30 = num29;
		byte* ptr31 = ptr;
		ptr += 4;
		ptr += _armorMaxPower.Serialize(ptr);
		int num30 = (int)(ptr - ptr31 - 4);
		if (num30 > 4194304)
		{
			throw new Exception($"Size of field {"_armorMaxPower"} must be less than {4096}KB");
		}
		*(int*)ptr31 = num30;
		byte* ptr32 = ptr;
		ptr += 4;
		ptr += _armorUseRequirement.Serialize(ptr);
		int num31 = (int)(ptr - ptr32 - 4);
		if (num31 > 4194304)
		{
			throw new Exception($"Size of field {"_armorUseRequirement"} must be less than {4096}KB");
		}
		*(int*)ptr32 = num31;
		byte* ptr33 = ptr;
		ptr += 4;
		ptr += _hitStrength.Serialize(ptr);
		int num32 = (int)(ptr - ptr33 - 4);
		if (num32 > 4194304)
		{
			throw new Exception($"Size of field {"_hitStrength"} must be less than {4096}KB");
		}
		*(int*)ptr33 = num32;
		byte* ptr34 = ptr;
		ptr += 4;
		ptr += _hitTechnique.Serialize(ptr);
		int num33 = (int)(ptr - ptr34 - 4);
		if (num33 > 4194304)
		{
			throw new Exception($"Size of field {"_hitTechnique"} must be less than {4096}KB");
		}
		*(int*)ptr34 = num33;
		byte* ptr35 = ptr;
		ptr += 4;
		ptr += _hitSpeed.Serialize(ptr);
		int num34 = (int)(ptr - ptr35 - 4);
		if (num34 > 4194304)
		{
			throw new Exception($"Size of field {"_hitSpeed"} must be less than {4096}KB");
		}
		*(int*)ptr35 = num34;
		byte* ptr36 = ptr;
		ptr += 4;
		ptr += _hitMind.Serialize(ptr);
		int num35 = (int)(ptr - ptr36 - 4);
		if (num35 > 4194304)
		{
			throw new Exception($"Size of field {"_hitMind"} must be less than {4096}KB");
		}
		*(int*)ptr36 = num35;
		byte* ptr37 = ptr;
		ptr += 4;
		ptr += _hitCanChange.Serialize(ptr);
		int num36 = (int)(ptr - ptr37 - 4);
		if (num36 > 4194304)
		{
			throw new Exception($"Size of field {"_hitCanChange"} must be less than {4096}KB");
		}
		*(int*)ptr37 = num36;
		byte* ptr38 = ptr;
		ptr += 4;
		ptr += _hitChangeEffectPercent.Serialize(ptr);
		int num37 = (int)(ptr - ptr38 - 4);
		if (num37 > 4194304)
		{
			throw new Exception($"Size of field {"_hitChangeEffectPercent"} must be less than {4096}KB");
		}
		*(int*)ptr38 = num37;
		byte* ptr39 = ptr;
		ptr += 4;
		ptr += _avoidStrength.Serialize(ptr);
		int num38 = (int)(ptr - ptr39 - 4);
		if (num38 > 4194304)
		{
			throw new Exception($"Size of field {"_avoidStrength"} must be less than {4096}KB");
		}
		*(int*)ptr39 = num38;
		byte* ptr40 = ptr;
		ptr += 4;
		ptr += _avoidTechnique.Serialize(ptr);
		int num39 = (int)(ptr - ptr40 - 4);
		if (num39 > 4194304)
		{
			throw new Exception($"Size of field {"_avoidTechnique"} must be less than {4096}KB");
		}
		*(int*)ptr40 = num39;
		byte* ptr41 = ptr;
		ptr += 4;
		ptr += _avoidSpeed.Serialize(ptr);
		int num40 = (int)(ptr - ptr41 - 4);
		if (num40 > 4194304)
		{
			throw new Exception($"Size of field {"_avoidSpeed"} must be less than {4096}KB");
		}
		*(int*)ptr41 = num40;
		byte* ptr42 = ptr;
		ptr += 4;
		ptr += _avoidMind.Serialize(ptr);
		int num41 = (int)(ptr - ptr42 - 4);
		if (num41 > 4194304)
		{
			throw new Exception($"Size of field {"_avoidMind"} must be less than {4096}KB");
		}
		*(int*)ptr42 = num41;
		byte* ptr43 = ptr;
		ptr += 4;
		ptr += _avoidCanChange.Serialize(ptr);
		int num42 = (int)(ptr - ptr43 - 4);
		if (num42 > 4194304)
		{
			throw new Exception($"Size of field {"_avoidCanChange"} must be less than {4096}KB");
		}
		*(int*)ptr43 = num42;
		byte* ptr44 = ptr;
		ptr += 4;
		ptr += _avoidChangeEffectPercent.Serialize(ptr);
		int num43 = (int)(ptr - ptr44 - 4);
		if (num43 > 4194304)
		{
			throw new Exception($"Size of field {"_avoidChangeEffectPercent"} must be less than {4096}KB");
		}
		*(int*)ptr44 = num43;
		byte* ptr45 = ptr;
		ptr += 4;
		ptr += _penetrateOuter.Serialize(ptr);
		int num44 = (int)(ptr - ptr45 - 4);
		if (num44 > 4194304)
		{
			throw new Exception($"Size of field {"_penetrateOuter"} must be less than {4096}KB");
		}
		*(int*)ptr45 = num44;
		byte* ptr46 = ptr;
		ptr += 4;
		ptr += _penetrateInner.Serialize(ptr);
		int num45 = (int)(ptr - ptr46 - 4);
		if (num45 > 4194304)
		{
			throw new Exception($"Size of field {"_penetrateInner"} must be less than {4096}KB");
		}
		*(int*)ptr46 = num45;
		byte* ptr47 = ptr;
		ptr += 4;
		ptr += _penetrateResistOuter.Serialize(ptr);
		int num46 = (int)(ptr - ptr47 - 4);
		if (num46 > 4194304)
		{
			throw new Exception($"Size of field {"_penetrateResistOuter"} must be less than {4096}KB");
		}
		*(int*)ptr47 = num46;
		byte* ptr48 = ptr;
		ptr += 4;
		ptr += _penetrateResistInner.Serialize(ptr);
		int num47 = (int)(ptr - ptr48 - 4);
		if (num47 > 4194304)
		{
			throw new Exception($"Size of field {"_penetrateResistInner"} must be less than {4096}KB");
		}
		*(int*)ptr48 = num47;
		byte* ptr49 = ptr;
		ptr += 4;
		ptr += _neiliAllocationAttack.Serialize(ptr);
		int num48 = (int)(ptr - ptr49 - 4);
		if (num48 > 4194304)
		{
			throw new Exception($"Size of field {"_neiliAllocationAttack"} must be less than {4096}KB");
		}
		*(int*)ptr49 = num48;
		byte* ptr50 = ptr;
		ptr += 4;
		ptr += _neiliAllocationAgile.Serialize(ptr);
		int num49 = (int)(ptr - ptr50 - 4);
		if (num49 > 4194304)
		{
			throw new Exception($"Size of field {"_neiliAllocationAgile"} must be less than {4096}KB");
		}
		*(int*)ptr50 = num49;
		byte* ptr51 = ptr;
		ptr += 4;
		ptr += _neiliAllocationDefense.Serialize(ptr);
		int num50 = (int)(ptr - ptr51 - 4);
		if (num50 > 4194304)
		{
			throw new Exception($"Size of field {"_neiliAllocationDefense"} must be less than {4096}KB");
		}
		*(int*)ptr51 = num50;
		byte* ptr52 = ptr;
		ptr += 4;
		ptr += _neiliAllocationAssist.Serialize(ptr);
		int num51 = (int)(ptr - ptr52 - 4);
		if (num51 > 4194304)
		{
			throw new Exception($"Size of field {"_neiliAllocationAssist"} must be less than {4096}KB");
		}
		*(int*)ptr52 = num51;
		byte* ptr53 = ptr;
		ptr += 4;
		ptr += _happiness.Serialize(ptr);
		int num52 = (int)(ptr - ptr53 - 4);
		if (num52 > 4194304)
		{
			throw new Exception($"Size of field {"_happiness"} must be less than {4096}KB");
		}
		*(int*)ptr53 = num52;
		byte* ptr54 = ptr;
		ptr += 4;
		ptr += _maxHealth.Serialize(ptr);
		int num53 = (int)(ptr - ptr54 - 4);
		if (num53 > 4194304)
		{
			throw new Exception($"Size of field {"_maxHealth"} must be less than {4096}KB");
		}
		*(int*)ptr54 = num53;
		byte* ptr55 = ptr;
		ptr += 4;
		ptr += _healthCost.Serialize(ptr);
		int num54 = (int)(ptr - ptr55 - 4);
		if (num54 > 4194304)
		{
			throw new Exception($"Size of field {"_healthCost"} must be less than {4096}KB");
		}
		*(int*)ptr55 = num54;
		byte* ptr56 = ptr;
		ptr += 4;
		ptr += _moveSpeedCanChange.Serialize(ptr);
		int num55 = (int)(ptr - ptr56 - 4);
		if (num55 > 4194304)
		{
			throw new Exception($"Size of field {"_moveSpeedCanChange"} must be less than {4096}KB");
		}
		*(int*)ptr56 = num55;
		byte* ptr57 = ptr;
		ptr += 4;
		ptr += _attackerHitStrength.Serialize(ptr);
		int num56 = (int)(ptr - ptr57 - 4);
		if (num56 > 4194304)
		{
			throw new Exception($"Size of field {"_attackerHitStrength"} must be less than {4096}KB");
		}
		*(int*)ptr57 = num56;
		byte* ptr58 = ptr;
		ptr += 4;
		ptr += _attackerHitTechnique.Serialize(ptr);
		int num57 = (int)(ptr - ptr58 - 4);
		if (num57 > 4194304)
		{
			throw new Exception($"Size of field {"_attackerHitTechnique"} must be less than {4096}KB");
		}
		*(int*)ptr58 = num57;
		byte* ptr59 = ptr;
		ptr += 4;
		ptr += _attackerHitSpeed.Serialize(ptr);
		int num58 = (int)(ptr - ptr59 - 4);
		if (num58 > 4194304)
		{
			throw new Exception($"Size of field {"_attackerHitSpeed"} must be less than {4096}KB");
		}
		*(int*)ptr59 = num58;
		byte* ptr60 = ptr;
		ptr += 4;
		ptr += _attackerHitMind.Serialize(ptr);
		int num59 = (int)(ptr - ptr60 - 4);
		if (num59 > 4194304)
		{
			throw new Exception($"Size of field {"_attackerHitMind"} must be less than {4096}KB");
		}
		*(int*)ptr60 = num59;
		byte* ptr61 = ptr;
		ptr += 4;
		ptr += _attackerAvoidStrength.Serialize(ptr);
		int num60 = (int)(ptr - ptr61 - 4);
		if (num60 > 4194304)
		{
			throw new Exception($"Size of field {"_attackerAvoidStrength"} must be less than {4096}KB");
		}
		*(int*)ptr61 = num60;
		byte* ptr62 = ptr;
		ptr += 4;
		ptr += _attackerAvoidTechnique.Serialize(ptr);
		int num61 = (int)(ptr - ptr62 - 4);
		if (num61 > 4194304)
		{
			throw new Exception($"Size of field {"_attackerAvoidTechnique"} must be less than {4096}KB");
		}
		*(int*)ptr62 = num61;
		byte* ptr63 = ptr;
		ptr += 4;
		ptr += _attackerAvoidSpeed.Serialize(ptr);
		int num62 = (int)(ptr - ptr63 - 4);
		if (num62 > 4194304)
		{
			throw new Exception($"Size of field {"_attackerAvoidSpeed"} must be less than {4096}KB");
		}
		*(int*)ptr63 = num62;
		byte* ptr64 = ptr;
		ptr += 4;
		ptr += _attackerAvoidMind.Serialize(ptr);
		int num63 = (int)(ptr - ptr64 - 4);
		if (num63 > 4194304)
		{
			throw new Exception($"Size of field {"_attackerAvoidMind"} must be less than {4096}KB");
		}
		*(int*)ptr64 = num63;
		byte* ptr65 = ptr;
		ptr += 4;
		ptr += _attackerPenetrateOuter.Serialize(ptr);
		int num64 = (int)(ptr - ptr65 - 4);
		if (num64 > 4194304)
		{
			throw new Exception($"Size of field {"_attackerPenetrateOuter"} must be less than {4096}KB");
		}
		*(int*)ptr65 = num64;
		byte* ptr66 = ptr;
		ptr += 4;
		ptr += _attackerPenetrateInner.Serialize(ptr);
		int num65 = (int)(ptr - ptr66 - 4);
		if (num65 > 4194304)
		{
			throw new Exception($"Size of field {"_attackerPenetrateInner"} must be less than {4096}KB");
		}
		*(int*)ptr66 = num65;
		byte* ptr67 = ptr;
		ptr += 4;
		ptr += _attackerPenetrateResistOuter.Serialize(ptr);
		int num66 = (int)(ptr - ptr67 - 4);
		if (num66 > 4194304)
		{
			throw new Exception($"Size of field {"_attackerPenetrateResistOuter"} must be less than {4096}KB");
		}
		*(int*)ptr67 = num66;
		byte* ptr68 = ptr;
		ptr += 4;
		ptr += _attackerPenetrateResistInner.Serialize(ptr);
		int num67 = (int)(ptr - ptr68 - 4);
		if (num67 > 4194304)
		{
			throw new Exception($"Size of field {"_attackerPenetrateResistInner"} must be less than {4096}KB");
		}
		*(int*)ptr68 = num67;
		byte* ptr69 = ptr;
		ptr += 4;
		ptr += _attackHitType.Serialize(ptr);
		int num68 = (int)(ptr - ptr69 - 4);
		if (num68 > 4194304)
		{
			throw new Exception($"Size of field {"_attackHitType"} must be less than {4096}KB");
		}
		*(int*)ptr69 = num68;
		byte* ptr70 = ptr;
		ptr += 4;
		ptr += _makeDirectDamage.Serialize(ptr);
		int num69 = (int)(ptr - ptr70 - 4);
		if (num69 > 4194304)
		{
			throw new Exception($"Size of field {"_makeDirectDamage"} must be less than {4096}KB");
		}
		*(int*)ptr70 = num69;
		byte* ptr71 = ptr;
		ptr += 4;
		ptr += _makeBounceDamage.Serialize(ptr);
		int num70 = (int)(ptr - ptr71 - 4);
		if (num70 > 4194304)
		{
			throw new Exception($"Size of field {"_makeBounceDamage"} must be less than {4096}KB");
		}
		*(int*)ptr71 = num70;
		byte* ptr72 = ptr;
		ptr += 4;
		ptr += _makeFightBackDamage.Serialize(ptr);
		int num71 = (int)(ptr - ptr72 - 4);
		if (num71 > 4194304)
		{
			throw new Exception($"Size of field {"_makeFightBackDamage"} must be less than {4096}KB");
		}
		*(int*)ptr72 = num71;
		byte* ptr73 = ptr;
		ptr += 4;
		ptr += _makePoisonLevel.Serialize(ptr);
		int num72 = (int)(ptr - ptr73 - 4);
		if (num72 > 4194304)
		{
			throw new Exception($"Size of field {"_makePoisonLevel"} must be less than {4096}KB");
		}
		*(int*)ptr73 = num72;
		byte* ptr74 = ptr;
		ptr += 4;
		ptr += _makePoisonValue.Serialize(ptr);
		int num73 = (int)(ptr - ptr74 - 4);
		if (num73 > 4194304)
		{
			throw new Exception($"Size of field {"_makePoisonValue"} must be less than {4096}KB");
		}
		*(int*)ptr74 = num73;
		byte* ptr75 = ptr;
		ptr += 4;
		ptr += _attackerHitOdds.Serialize(ptr);
		int num74 = (int)(ptr - ptr75 - 4);
		if (num74 > 4194304)
		{
			throw new Exception($"Size of field {"_attackerHitOdds"} must be less than {4096}KB");
		}
		*(int*)ptr75 = num74;
		byte* ptr76 = ptr;
		ptr += 4;
		ptr += _attackerFightBackHitOdds.Serialize(ptr);
		int num75 = (int)(ptr - ptr76 - 4);
		if (num75 > 4194304)
		{
			throw new Exception($"Size of field {"_attackerFightBackHitOdds"} must be less than {4096}KB");
		}
		*(int*)ptr76 = num75;
		byte* ptr77 = ptr;
		ptr += 4;
		ptr += _attackerPursueOdds.Serialize(ptr);
		int num76 = (int)(ptr - ptr77 - 4);
		if (num76 > 4194304)
		{
			throw new Exception($"Size of field {"_attackerPursueOdds"} must be less than {4096}KB");
		}
		*(int*)ptr77 = num76;
		byte* ptr78 = ptr;
		ptr += 4;
		ptr += _makedInjuryChangeToOld.Serialize(ptr);
		int num77 = (int)(ptr - ptr78 - 4);
		if (num77 > 4194304)
		{
			throw new Exception($"Size of field {"_makedInjuryChangeToOld"} must be less than {4096}KB");
		}
		*(int*)ptr78 = num77;
		byte* ptr79 = ptr;
		ptr += 4;
		ptr += _makedPoisonChangeToOld.Serialize(ptr);
		int num78 = (int)(ptr - ptr79 - 4);
		if (num78 > 4194304)
		{
			throw new Exception($"Size of field {"_makedPoisonChangeToOld"} must be less than {4096}KB");
		}
		*(int*)ptr79 = num78;
		byte* ptr80 = ptr;
		ptr += 4;
		ptr += _makeDamageType.Serialize(ptr);
		int num79 = (int)(ptr - ptr80 - 4);
		if (num79 > 4194304)
		{
			throw new Exception($"Size of field {"_makeDamageType"} must be less than {4096}KB");
		}
		*(int*)ptr80 = num79;
		byte* ptr81 = ptr;
		ptr += 4;
		ptr += _canMakeInjuryToNoInjuryPart.Serialize(ptr);
		int num80 = (int)(ptr - ptr81 - 4);
		if (num80 > 4194304)
		{
			throw new Exception($"Size of field {"_canMakeInjuryToNoInjuryPart"} must be less than {4096}KB");
		}
		*(int*)ptr81 = num80;
		byte* ptr82 = ptr;
		ptr += 4;
		ptr += _makePoisonType.Serialize(ptr);
		int num81 = (int)(ptr - ptr82 - 4);
		if (num81 > 4194304)
		{
			throw new Exception($"Size of field {"_makePoisonType"} must be less than {4096}KB");
		}
		*(int*)ptr82 = num81;
		byte* ptr83 = ptr;
		ptr += 4;
		ptr += _normalAttackWeapon.Serialize(ptr);
		int num82 = (int)(ptr - ptr83 - 4);
		if (num82 > 4194304)
		{
			throw new Exception($"Size of field {"_normalAttackWeapon"} must be less than {4096}KB");
		}
		*(int*)ptr83 = num82;
		byte* ptr84 = ptr;
		ptr += 4;
		ptr += _normalAttackTrick.Serialize(ptr);
		int num83 = (int)(ptr - ptr84 - 4);
		if (num83 > 4194304)
		{
			throw new Exception($"Size of field {"_normalAttackTrick"} must be less than {4096}KB");
		}
		*(int*)ptr84 = num83;
		byte* ptr85 = ptr;
		ptr += 4;
		ptr += _extraFlawCount.Serialize(ptr);
		int num84 = (int)(ptr - ptr85 - 4);
		if (num84 > 4194304)
		{
			throw new Exception($"Size of field {"_extraFlawCount"} must be less than {4096}KB");
		}
		*(int*)ptr85 = num84;
		byte* ptr86 = ptr;
		ptr += 4;
		ptr += _attackCanBounce.Serialize(ptr);
		int num85 = (int)(ptr - ptr86 - 4);
		if (num85 > 4194304)
		{
			throw new Exception($"Size of field {"_attackCanBounce"} must be less than {4096}KB");
		}
		*(int*)ptr86 = num85;
		byte* ptr87 = ptr;
		ptr += 4;
		ptr += _attackCanFightBack.Serialize(ptr);
		int num86 = (int)(ptr - ptr87 - 4);
		if (num86 > 4194304)
		{
			throw new Exception($"Size of field {"_attackCanFightBack"} must be less than {4096}KB");
		}
		*(int*)ptr87 = num86;
		byte* ptr88 = ptr;
		ptr += 4;
		ptr += _makeFightBackInjuryMark.Serialize(ptr);
		int num87 = (int)(ptr - ptr88 - 4);
		if (num87 > 4194304)
		{
			throw new Exception($"Size of field {"_makeFightBackInjuryMark"} must be less than {4096}KB");
		}
		*(int*)ptr88 = num87;
		byte* ptr89 = ptr;
		ptr += 4;
		ptr += _legSkillUseShoes.Serialize(ptr);
		int num88 = (int)(ptr - ptr89 - 4);
		if (num88 > 4194304)
		{
			throw new Exception($"Size of field {"_legSkillUseShoes"} must be less than {4096}KB");
		}
		*(int*)ptr89 = num88;
		byte* ptr90 = ptr;
		ptr += 4;
		ptr += _attackerFinalDamageValue.Serialize(ptr);
		int num89 = (int)(ptr - ptr90 - 4);
		if (num89 > 4194304)
		{
			throw new Exception($"Size of field {"_attackerFinalDamageValue"} must be less than {4096}KB");
		}
		*(int*)ptr90 = num89;
		byte* ptr91 = ptr;
		ptr += 4;
		ptr += _defenderHitStrength.Serialize(ptr);
		int num90 = (int)(ptr - ptr91 - 4);
		if (num90 > 4194304)
		{
			throw new Exception($"Size of field {"_defenderHitStrength"} must be less than {4096}KB");
		}
		*(int*)ptr91 = num90;
		byte* ptr92 = ptr;
		ptr += 4;
		ptr += _defenderHitTechnique.Serialize(ptr);
		int num91 = (int)(ptr - ptr92 - 4);
		if (num91 > 4194304)
		{
			throw new Exception($"Size of field {"_defenderHitTechnique"} must be less than {4096}KB");
		}
		*(int*)ptr92 = num91;
		byte* ptr93 = ptr;
		ptr += 4;
		ptr += _defenderHitSpeed.Serialize(ptr);
		int num92 = (int)(ptr - ptr93 - 4);
		if (num92 > 4194304)
		{
			throw new Exception($"Size of field {"_defenderHitSpeed"} must be less than {4096}KB");
		}
		*(int*)ptr93 = num92;
		byte* ptr94 = ptr;
		ptr += 4;
		ptr += _defenderHitMind.Serialize(ptr);
		int num93 = (int)(ptr - ptr94 - 4);
		if (num93 > 4194304)
		{
			throw new Exception($"Size of field {"_defenderHitMind"} must be less than {4096}KB");
		}
		*(int*)ptr94 = num93;
		byte* ptr95 = ptr;
		ptr += 4;
		ptr += _defenderAvoidStrength.Serialize(ptr);
		int num94 = (int)(ptr - ptr95 - 4);
		if (num94 > 4194304)
		{
			throw new Exception($"Size of field {"_defenderAvoidStrength"} must be less than {4096}KB");
		}
		*(int*)ptr95 = num94;
		byte* ptr96 = ptr;
		ptr += 4;
		ptr += _defenderAvoidTechnique.Serialize(ptr);
		int num95 = (int)(ptr - ptr96 - 4);
		if (num95 > 4194304)
		{
			throw new Exception($"Size of field {"_defenderAvoidTechnique"} must be less than {4096}KB");
		}
		*(int*)ptr96 = num95;
		byte* ptr97 = ptr;
		ptr += 4;
		ptr += _defenderAvoidSpeed.Serialize(ptr);
		int num96 = (int)(ptr - ptr97 - 4);
		if (num96 > 4194304)
		{
			throw new Exception($"Size of field {"_defenderAvoidSpeed"} must be less than {4096}KB");
		}
		*(int*)ptr97 = num96;
		byte* ptr98 = ptr;
		ptr += 4;
		ptr += _defenderAvoidMind.Serialize(ptr);
		int num97 = (int)(ptr - ptr98 - 4);
		if (num97 > 4194304)
		{
			throw new Exception($"Size of field {"_defenderAvoidMind"} must be less than {4096}KB");
		}
		*(int*)ptr98 = num97;
		byte* ptr99 = ptr;
		ptr += 4;
		ptr += _defenderPenetrateOuter.Serialize(ptr);
		int num98 = (int)(ptr - ptr99 - 4);
		if (num98 > 4194304)
		{
			throw new Exception($"Size of field {"_defenderPenetrateOuter"} must be less than {4096}KB");
		}
		*(int*)ptr99 = num98;
		byte* ptr100 = ptr;
		ptr += 4;
		ptr += _defenderPenetrateInner.Serialize(ptr);
		int num99 = (int)(ptr - ptr100 - 4);
		if (num99 > 4194304)
		{
			throw new Exception($"Size of field {"_defenderPenetrateInner"} must be less than {4096}KB");
		}
		*(int*)ptr100 = num99;
		byte* ptr101 = ptr;
		ptr += 4;
		ptr += _defenderPenetrateResistOuter.Serialize(ptr);
		int num100 = (int)(ptr - ptr101 - 4);
		if (num100 > 4194304)
		{
			throw new Exception($"Size of field {"_defenderPenetrateResistOuter"} must be less than {4096}KB");
		}
		*(int*)ptr101 = num100;
		byte* ptr102 = ptr;
		ptr += 4;
		ptr += _defenderPenetrateResistInner.Serialize(ptr);
		int num101 = (int)(ptr - ptr102 - 4);
		if (num101 > 4194304)
		{
			throw new Exception($"Size of field {"_defenderPenetrateResistInner"} must be less than {4096}KB");
		}
		*(int*)ptr102 = num101;
		byte* ptr103 = ptr;
		ptr += 4;
		ptr += _acceptDirectDamage.Serialize(ptr);
		int num102 = (int)(ptr - ptr103 - 4);
		if (num102 > 4194304)
		{
			throw new Exception($"Size of field {"_acceptDirectDamage"} must be less than {4096}KB");
		}
		*(int*)ptr103 = num102;
		byte* ptr104 = ptr;
		ptr += 4;
		ptr += _acceptBounceDamage.Serialize(ptr);
		int num103 = (int)(ptr - ptr104 - 4);
		if (num103 > 4194304)
		{
			throw new Exception($"Size of field {"_acceptBounceDamage"} must be less than {4096}KB");
		}
		*(int*)ptr104 = num103;
		byte* ptr105 = ptr;
		ptr += 4;
		ptr += _acceptFightBackDamage.Serialize(ptr);
		int num104 = (int)(ptr - ptr105 - 4);
		if (num104 > 4194304)
		{
			throw new Exception($"Size of field {"_acceptFightBackDamage"} must be less than {4096}KB");
		}
		*(int*)ptr105 = num104;
		byte* ptr106 = ptr;
		ptr += 4;
		ptr += _acceptPoisonLevel.Serialize(ptr);
		int num105 = (int)(ptr - ptr106 - 4);
		if (num105 > 4194304)
		{
			throw new Exception($"Size of field {"_acceptPoisonLevel"} must be less than {4096}KB");
		}
		*(int*)ptr106 = num105;
		byte* ptr107 = ptr;
		ptr += 4;
		ptr += _acceptPoisonValue.Serialize(ptr);
		int num106 = (int)(ptr - ptr107 - 4);
		if (num106 > 4194304)
		{
			throw new Exception($"Size of field {"_acceptPoisonValue"} must be less than {4096}KB");
		}
		*(int*)ptr107 = num106;
		byte* ptr108 = ptr;
		ptr += 4;
		ptr += _defenderHitOdds.Serialize(ptr);
		int num107 = (int)(ptr - ptr108 - 4);
		if (num107 > 4194304)
		{
			throw new Exception($"Size of field {"_defenderHitOdds"} must be less than {4096}KB");
		}
		*(int*)ptr108 = num107;
		byte* ptr109 = ptr;
		ptr += 4;
		ptr += _defenderFightBackHitOdds.Serialize(ptr);
		int num108 = (int)(ptr - ptr109 - 4);
		if (num108 > 4194304)
		{
			throw new Exception($"Size of field {"_defenderFightBackHitOdds"} must be less than {4096}KB");
		}
		*(int*)ptr109 = num108;
		byte* ptr110 = ptr;
		ptr += 4;
		ptr += _defenderPursueOdds.Serialize(ptr);
		int num109 = (int)(ptr - ptr110 - 4);
		if (num109 > 4194304)
		{
			throw new Exception($"Size of field {"_defenderPursueOdds"} must be less than {4096}KB");
		}
		*(int*)ptr110 = num109;
		byte* ptr111 = ptr;
		ptr += 4;
		ptr += _acceptMaxInjuryCount.Serialize(ptr);
		int num110 = (int)(ptr - ptr111 - 4);
		if (num110 > 4194304)
		{
			throw new Exception($"Size of field {"_acceptMaxInjuryCount"} must be less than {4096}KB");
		}
		*(int*)ptr111 = num110;
		byte* ptr112 = ptr;
		ptr += 4;
		ptr += _bouncePower.Serialize(ptr);
		int num111 = (int)(ptr - ptr112 - 4);
		if (num111 > 4194304)
		{
			throw new Exception($"Size of field {"_bouncePower"} must be less than {4096}KB");
		}
		*(int*)ptr112 = num111;
		byte* ptr113 = ptr;
		ptr += 4;
		ptr += _fightBackPower.Serialize(ptr);
		int num112 = (int)(ptr - ptr113 - 4);
		if (num112 > 4194304)
		{
			throw new Exception($"Size of field {"_fightBackPower"} must be less than {4096}KB");
		}
		*(int*)ptr113 = num112;
		byte* ptr114 = ptr;
		ptr += 4;
		ptr += _directDamageInnerRatio.Serialize(ptr);
		int num113 = (int)(ptr - ptr114 - 4);
		if (num113 > 4194304)
		{
			throw new Exception($"Size of field {"_directDamageInnerRatio"} must be less than {4096}KB");
		}
		*(int*)ptr114 = num113;
		byte* ptr115 = ptr;
		ptr += 4;
		ptr += _defenderFinalDamageValue.Serialize(ptr);
		int num114 = (int)(ptr - ptr115 - 4);
		if (num114 > 4194304)
		{
			throw new Exception($"Size of field {"_defenderFinalDamageValue"} must be less than {4096}KB");
		}
		*(int*)ptr115 = num114;
		byte* ptr116 = ptr;
		ptr += 4;
		ptr += _directDamageValue.Serialize(ptr);
		int num115 = (int)(ptr - ptr116 - 4);
		if (num115 > 4194304)
		{
			throw new Exception($"Size of field {"_directDamageValue"} must be less than {4096}KB");
		}
		*(int*)ptr116 = num115;
		byte* ptr117 = ptr;
		ptr += 4;
		ptr += _directInjuryMark.Serialize(ptr);
		int num116 = (int)(ptr - ptr117 - 4);
		if (num116 > 4194304)
		{
			throw new Exception($"Size of field {"_directInjuryMark"} must be less than {4096}KB");
		}
		*(int*)ptr117 = num116;
		byte* ptr118 = ptr;
		ptr += 4;
		ptr += _goneMadInjury.Serialize(ptr);
		int num117 = (int)(ptr - ptr118 - 4);
		if (num117 > 4194304)
		{
			throw new Exception($"Size of field {"_goneMadInjury"} must be less than {4096}KB");
		}
		*(int*)ptr118 = num117;
		byte* ptr119 = ptr;
		ptr += 4;
		ptr += _healInjurySpeed.Serialize(ptr);
		int num118 = (int)(ptr - ptr119 - 4);
		if (num118 > 4194304)
		{
			throw new Exception($"Size of field {"_healInjurySpeed"} must be less than {4096}KB");
		}
		*(int*)ptr119 = num118;
		byte* ptr120 = ptr;
		ptr += 4;
		ptr += _healInjuryBuff.Serialize(ptr);
		int num119 = (int)(ptr - ptr120 - 4);
		if (num119 > 4194304)
		{
			throw new Exception($"Size of field {"_healInjuryBuff"} must be less than {4096}KB");
		}
		*(int*)ptr120 = num119;
		byte* ptr121 = ptr;
		ptr += 4;
		ptr += _healInjuryDebuff.Serialize(ptr);
		int num120 = (int)(ptr - ptr121 - 4);
		if (num120 > 4194304)
		{
			throw new Exception($"Size of field {"_healInjuryDebuff"} must be less than {4096}KB");
		}
		*(int*)ptr121 = num120;
		byte* ptr122 = ptr;
		ptr += 4;
		ptr += _healPoisonSpeed.Serialize(ptr);
		int num121 = (int)(ptr - ptr122 - 4);
		if (num121 > 4194304)
		{
			throw new Exception($"Size of field {"_healPoisonSpeed"} must be less than {4096}KB");
		}
		*(int*)ptr122 = num121;
		byte* ptr123 = ptr;
		ptr += 4;
		ptr += _healPoisonBuff.Serialize(ptr);
		int num122 = (int)(ptr - ptr123 - 4);
		if (num122 > 4194304)
		{
			throw new Exception($"Size of field {"_healPoisonBuff"} must be less than {4096}KB");
		}
		*(int*)ptr123 = num122;
		byte* ptr124 = ptr;
		ptr += 4;
		ptr += _healPoisonDebuff.Serialize(ptr);
		int num123 = (int)(ptr - ptr124 - 4);
		if (num123 > 4194304)
		{
			throw new Exception($"Size of field {"_healPoisonDebuff"} must be less than {4096}KB");
		}
		*(int*)ptr124 = num123;
		byte* ptr125 = ptr;
		ptr += 4;
		ptr += _fleeSpeed.Serialize(ptr);
		int num124 = (int)(ptr - ptr125 - 4);
		if (num124 > 4194304)
		{
			throw new Exception($"Size of field {"_fleeSpeed"} must be less than {4096}KB");
		}
		*(int*)ptr125 = num124;
		byte* ptr126 = ptr;
		ptr += 4;
		ptr += _maxFlawCount.Serialize(ptr);
		int num125 = (int)(ptr - ptr126 - 4);
		if (num125 > 4194304)
		{
			throw new Exception($"Size of field {"_maxFlawCount"} must be less than {4096}KB");
		}
		*(int*)ptr126 = num125;
		byte* ptr127 = ptr;
		ptr += 4;
		ptr += _canAddFlaw.Serialize(ptr);
		int num126 = (int)(ptr - ptr127 - 4);
		if (num126 > 4194304)
		{
			throw new Exception($"Size of field {"_canAddFlaw"} must be less than {4096}KB");
		}
		*(int*)ptr127 = num126;
		byte* ptr128 = ptr;
		ptr += 4;
		ptr += _flawLevel.Serialize(ptr);
		int num127 = (int)(ptr - ptr128 - 4);
		if (num127 > 4194304)
		{
			throw new Exception($"Size of field {"_flawLevel"} must be less than {4096}KB");
		}
		*(int*)ptr128 = num127;
		byte* ptr129 = ptr;
		ptr += 4;
		ptr += _flawLevelCanReduce.Serialize(ptr);
		int num128 = (int)(ptr - ptr129 - 4);
		if (num128 > 4194304)
		{
			throw new Exception($"Size of field {"_flawLevelCanReduce"} must be less than {4096}KB");
		}
		*(int*)ptr129 = num128;
		byte* ptr130 = ptr;
		ptr += 4;
		ptr += _flawCount.Serialize(ptr);
		int num129 = (int)(ptr - ptr130 - 4);
		if (num129 > 4194304)
		{
			throw new Exception($"Size of field {"_flawCount"} must be less than {4096}KB");
		}
		*(int*)ptr130 = num129;
		byte* ptr131 = ptr;
		ptr += 4;
		ptr += _maxAcupointCount.Serialize(ptr);
		int num130 = (int)(ptr - ptr131 - 4);
		if (num130 > 4194304)
		{
			throw new Exception($"Size of field {"_maxAcupointCount"} must be less than {4096}KB");
		}
		*(int*)ptr131 = num130;
		byte* ptr132 = ptr;
		ptr += 4;
		ptr += _canAddAcupoint.Serialize(ptr);
		int num131 = (int)(ptr - ptr132 - 4);
		if (num131 > 4194304)
		{
			throw new Exception($"Size of field {"_canAddAcupoint"} must be less than {4096}KB");
		}
		*(int*)ptr132 = num131;
		byte* ptr133 = ptr;
		ptr += 4;
		ptr += _acupointLevel.Serialize(ptr);
		int num132 = (int)(ptr - ptr133 - 4);
		if (num132 > 4194304)
		{
			throw new Exception($"Size of field {"_acupointLevel"} must be less than {4096}KB");
		}
		*(int*)ptr133 = num132;
		byte* ptr134 = ptr;
		ptr += 4;
		ptr += _acupointLevelCanReduce.Serialize(ptr);
		int num133 = (int)(ptr - ptr134 - 4);
		if (num133 > 4194304)
		{
			throw new Exception($"Size of field {"_acupointLevelCanReduce"} must be less than {4096}KB");
		}
		*(int*)ptr134 = num133;
		byte* ptr135 = ptr;
		ptr += 4;
		ptr += _acupointCount.Serialize(ptr);
		int num134 = (int)(ptr - ptr135 - 4);
		if (num134 > 4194304)
		{
			throw new Exception($"Size of field {"_acupointCount"} must be less than {4096}KB");
		}
		*(int*)ptr135 = num134;
		byte* ptr136 = ptr;
		ptr += 4;
		ptr += _addNeiliAllocation.Serialize(ptr);
		int num135 = (int)(ptr - ptr136 - 4);
		if (num135 > 4194304)
		{
			throw new Exception($"Size of field {"_addNeiliAllocation"} must be less than {4096}KB");
		}
		*(int*)ptr136 = num135;
		byte* ptr137 = ptr;
		ptr += 4;
		ptr += _costNeiliAllocation.Serialize(ptr);
		int num136 = (int)(ptr - ptr137 - 4);
		if (num136 > 4194304)
		{
			throw new Exception($"Size of field {"_costNeiliAllocation"} must be less than {4096}KB");
		}
		*(int*)ptr137 = num136;
		byte* ptr138 = ptr;
		ptr += 4;
		ptr += _canChangeNeiliAllocation.Serialize(ptr);
		int num137 = (int)(ptr - ptr138 - 4);
		if (num137 > 4194304)
		{
			throw new Exception($"Size of field {"_canChangeNeiliAllocation"} must be less than {4096}KB");
		}
		*(int*)ptr138 = num137;
		byte* ptr139 = ptr;
		ptr += 4;
		ptr += _canGetTrick.Serialize(ptr);
		int num138 = (int)(ptr - ptr139 - 4);
		if (num138 > 4194304)
		{
			throw new Exception($"Size of field {"_canGetTrick"} must be less than {4096}KB");
		}
		*(int*)ptr139 = num138;
		byte* ptr140 = ptr;
		ptr += 4;
		ptr += _getTrickType.Serialize(ptr);
		int num139 = (int)(ptr - ptr140 - 4);
		if (num139 > 4194304)
		{
			throw new Exception($"Size of field {"_getTrickType"} must be less than {4096}KB");
		}
		*(int*)ptr140 = num139;
		byte* ptr141 = ptr;
		ptr += 4;
		ptr += _attackBodyPart.Serialize(ptr);
		int num140 = (int)(ptr - ptr141 - 4);
		if (num140 > 4194304)
		{
			throw new Exception($"Size of field {"_attackBodyPart"} must be less than {4096}KB");
		}
		*(int*)ptr141 = num140;
		byte* ptr142 = ptr;
		ptr += 4;
		ptr += _weaponEquipAttack.Serialize(ptr);
		int num141 = (int)(ptr - ptr142 - 4);
		if (num141 > 4194304)
		{
			throw new Exception($"Size of field {"_weaponEquipAttack"} must be less than {4096}KB");
		}
		*(int*)ptr142 = num141;
		byte* ptr143 = ptr;
		ptr += 4;
		ptr += _weaponEquipDefense.Serialize(ptr);
		int num142 = (int)(ptr - ptr143 - 4);
		if (num142 > 4194304)
		{
			throw new Exception($"Size of field {"_weaponEquipDefense"} must be less than {4096}KB");
		}
		*(int*)ptr143 = num142;
		byte* ptr144 = ptr;
		ptr += 4;
		ptr += _armorEquipAttack.Serialize(ptr);
		int num143 = (int)(ptr - ptr144 - 4);
		if (num143 > 4194304)
		{
			throw new Exception($"Size of field {"_armorEquipAttack"} must be less than {4096}KB");
		}
		*(int*)ptr144 = num143;
		byte* ptr145 = ptr;
		ptr += 4;
		ptr += _armorEquipDefense.Serialize(ptr);
		int num144 = (int)(ptr - ptr145 - 4);
		if (num144 > 4194304)
		{
			throw new Exception($"Size of field {"_armorEquipDefense"} must be less than {4096}KB");
		}
		*(int*)ptr145 = num144;
		byte* ptr146 = ptr;
		ptr += 4;
		ptr += _attackRangeForward.Serialize(ptr);
		int num145 = (int)(ptr - ptr146 - 4);
		if (num145 > 4194304)
		{
			throw new Exception($"Size of field {"_attackRangeForward"} must be less than {4096}KB");
		}
		*(int*)ptr146 = num145;
		byte* ptr147 = ptr;
		ptr += 4;
		ptr += _attackRangeBackward.Serialize(ptr);
		int num146 = (int)(ptr - ptr147 - 4);
		if (num146 > 4194304)
		{
			throw new Exception($"Size of field {"_attackRangeBackward"} must be less than {4096}KB");
		}
		*(int*)ptr147 = num146;
		byte* ptr148 = ptr;
		ptr += 4;
		ptr += _moveCanBeStopped.Serialize(ptr);
		int num147 = (int)(ptr - ptr148 - 4);
		if (num147 > 4194304)
		{
			throw new Exception($"Size of field {"_moveCanBeStopped"} must be less than {4096}KB");
		}
		*(int*)ptr148 = num147;
		byte* ptr149 = ptr;
		ptr += 4;
		ptr += _canForcedMove.Serialize(ptr);
		int num148 = (int)(ptr - ptr149 - 4);
		if (num148 > 4194304)
		{
			throw new Exception($"Size of field {"_canForcedMove"} must be less than {4096}KB");
		}
		*(int*)ptr149 = num148;
		byte* ptr150 = ptr;
		ptr += 4;
		ptr += _mobilityCanBeRemoved.Serialize(ptr);
		int num149 = (int)(ptr - ptr150 - 4);
		if (num149 > 4194304)
		{
			throw new Exception($"Size of field {"_mobilityCanBeRemoved"} must be less than {4096}KB");
		}
		*(int*)ptr150 = num149;
		byte* ptr151 = ptr;
		ptr += 4;
		ptr += _mobilityCostByEffect.Serialize(ptr);
		int num150 = (int)(ptr - ptr151 - 4);
		if (num150 > 4194304)
		{
			throw new Exception($"Size of field {"_mobilityCostByEffect"} must be less than {4096}KB");
		}
		*(int*)ptr151 = num150;
		byte* ptr152 = ptr;
		ptr += 4;
		ptr += _moveDistance.Serialize(ptr);
		int num151 = (int)(ptr - ptr152 - 4);
		if (num151 > 4194304)
		{
			throw new Exception($"Size of field {"_moveDistance"} must be less than {4096}KB");
		}
		*(int*)ptr152 = num151;
		byte* ptr153 = ptr;
		ptr += 4;
		ptr += _jumpPrepareFrame.Serialize(ptr);
		int num152 = (int)(ptr - ptr153 - 4);
		if (num152 > 4194304)
		{
			throw new Exception($"Size of field {"_jumpPrepareFrame"} must be less than {4096}KB");
		}
		*(int*)ptr153 = num152;
		byte* ptr154 = ptr;
		ptr += 4;
		ptr += _bounceInjuryMark.Serialize(ptr);
		int num153 = (int)(ptr - ptr154 - 4);
		if (num153 > 4194304)
		{
			throw new Exception($"Size of field {"_bounceInjuryMark"} must be less than {4096}KB");
		}
		*(int*)ptr154 = num153;
		byte* ptr155 = ptr;
		ptr += 4;
		ptr += _skillHasCost.Serialize(ptr);
		int num154 = (int)(ptr - ptr155 - 4);
		if (num154 > 4194304)
		{
			throw new Exception($"Size of field {"_skillHasCost"} must be less than {4096}KB");
		}
		*(int*)ptr155 = num154;
		byte* ptr156 = ptr;
		ptr += 4;
		ptr += _combatStateEffect.Serialize(ptr);
		int num155 = (int)(ptr - ptr156 - 4);
		if (num155 > 4194304)
		{
			throw new Exception($"Size of field {"_combatStateEffect"} must be less than {4096}KB");
		}
		*(int*)ptr156 = num155;
		byte* ptr157 = ptr;
		ptr += 4;
		ptr += _changeNeedUseSkill.Serialize(ptr);
		int num156 = (int)(ptr - ptr157 - 4);
		if (num156 > 4194304)
		{
			throw new Exception($"Size of field {"_changeNeedUseSkill"} must be less than {4096}KB");
		}
		*(int*)ptr157 = num156;
		byte* ptr158 = ptr;
		ptr += 4;
		ptr += _changeDistanceIsMove.Serialize(ptr);
		int num157 = (int)(ptr - ptr158 - 4);
		if (num157 > 4194304)
		{
			throw new Exception($"Size of field {"_changeDistanceIsMove"} must be less than {4096}KB");
		}
		*(int*)ptr158 = num157;
		byte* ptr159 = ptr;
		ptr += 4;
		ptr += _replaceCharHit.Serialize(ptr);
		int num158 = (int)(ptr - ptr159 - 4);
		if (num158 > 4194304)
		{
			throw new Exception($"Size of field {"_replaceCharHit"} must be less than {4096}KB");
		}
		*(int*)ptr159 = num158;
		byte* ptr160 = ptr;
		ptr += 4;
		ptr += _canAddPoison.Serialize(ptr);
		int num159 = (int)(ptr - ptr160 - 4);
		if (num159 > 4194304)
		{
			throw new Exception($"Size of field {"_canAddPoison"} must be less than {4096}KB");
		}
		*(int*)ptr160 = num159;
		byte* ptr161 = ptr;
		ptr += 4;
		ptr += _canReducePoison.Serialize(ptr);
		int num160 = (int)(ptr - ptr161 - 4);
		if (num160 > 4194304)
		{
			throw new Exception($"Size of field {"_canReducePoison"} must be less than {4096}KB");
		}
		*(int*)ptr161 = num160;
		byte* ptr162 = ptr;
		ptr += 4;
		ptr += _reducePoisonValue.Serialize(ptr);
		int num161 = (int)(ptr - ptr162 - 4);
		if (num161 > 4194304)
		{
			throw new Exception($"Size of field {"_reducePoisonValue"} must be less than {4096}KB");
		}
		*(int*)ptr162 = num161;
		byte* ptr163 = ptr;
		ptr += 4;
		ptr += _poisonCanAffect.Serialize(ptr);
		int num162 = (int)(ptr - ptr163 - 4);
		if (num162 > 4194304)
		{
			throw new Exception($"Size of field {"_poisonCanAffect"} must be less than {4096}KB");
		}
		*(int*)ptr163 = num162;
		byte* ptr164 = ptr;
		ptr += 4;
		ptr += _poisonAffectCount.Serialize(ptr);
		int num163 = (int)(ptr - ptr164 - 4);
		if (num163 > 4194304)
		{
			throw new Exception($"Size of field {"_poisonAffectCount"} must be less than {4096}KB");
		}
		*(int*)ptr164 = num163;
		byte* ptr165 = ptr;
		ptr += 4;
		ptr += _costTricks.Serialize(ptr);
		int num164 = (int)(ptr - ptr165 - 4);
		if (num164 > 4194304)
		{
			throw new Exception($"Size of field {"_costTricks"} must be less than {4096}KB");
		}
		*(int*)ptr165 = num164;
		byte* ptr166 = ptr;
		ptr += 4;
		ptr += _jumpMoveDistance.Serialize(ptr);
		int num165 = (int)(ptr - ptr166 - 4);
		if (num165 > 4194304)
		{
			throw new Exception($"Size of field {"_jumpMoveDistance"} must be less than {4096}KB");
		}
		*(int*)ptr166 = num165;
		byte* ptr167 = ptr;
		ptr += 4;
		ptr += _combatStateToAdd.Serialize(ptr);
		int num166 = (int)(ptr - ptr167 - 4);
		if (num166 > 4194304)
		{
			throw new Exception($"Size of field {"_combatStateToAdd"} must be less than {4096}KB");
		}
		*(int*)ptr167 = num166;
		byte* ptr168 = ptr;
		ptr += 4;
		ptr += _combatStatePower.Serialize(ptr);
		int num167 = (int)(ptr - ptr168 - 4);
		if (num167 > 4194304)
		{
			throw new Exception($"Size of field {"_combatStatePower"} must be less than {4096}KB");
		}
		*(int*)ptr168 = num167;
		byte* ptr169 = ptr;
		ptr += 4;
		ptr += _breakBodyPartInjuryCount.Serialize(ptr);
		int num168 = (int)(ptr - ptr169 - 4);
		if (num168 > 4194304)
		{
			throw new Exception($"Size of field {"_breakBodyPartInjuryCount"} must be less than {4096}KB");
		}
		*(int*)ptr169 = num168;
		byte* ptr170 = ptr;
		ptr += 4;
		ptr += _bodyPartIsBroken.Serialize(ptr);
		int num169 = (int)(ptr - ptr170 - 4);
		if (num169 > 4194304)
		{
			throw new Exception($"Size of field {"_bodyPartIsBroken"} must be less than {4096}KB");
		}
		*(int*)ptr170 = num169;
		byte* ptr171 = ptr;
		ptr += 4;
		ptr += _maxTrickCount.Serialize(ptr);
		int num170 = (int)(ptr - ptr171 - 4);
		if (num170 > 4194304)
		{
			throw new Exception($"Size of field {"_maxTrickCount"} must be less than {4096}KB");
		}
		*(int*)ptr171 = num170;
		byte* ptr172 = ptr;
		ptr += 4;
		ptr += _maxBreathPercent.Serialize(ptr);
		int num171 = (int)(ptr - ptr172 - 4);
		if (num171 > 4194304)
		{
			throw new Exception($"Size of field {"_maxBreathPercent"} must be less than {4096}KB");
		}
		*(int*)ptr172 = num171;
		byte* ptr173 = ptr;
		ptr += 4;
		ptr += _maxStancePercent.Serialize(ptr);
		int num172 = (int)(ptr - ptr173 - 4);
		if (num172 > 4194304)
		{
			throw new Exception($"Size of field {"_maxStancePercent"} must be less than {4096}KB");
		}
		*(int*)ptr173 = num172;
		byte* ptr174 = ptr;
		ptr += 4;
		ptr += _extraBreathPercent.Serialize(ptr);
		int num173 = (int)(ptr - ptr174 - 4);
		if (num173 > 4194304)
		{
			throw new Exception($"Size of field {"_extraBreathPercent"} must be less than {4096}KB");
		}
		*(int*)ptr174 = num173;
		byte* ptr175 = ptr;
		ptr += 4;
		ptr += _extraStancePercent.Serialize(ptr);
		int num174 = (int)(ptr - ptr175 - 4);
		if (num174 > 4194304)
		{
			throw new Exception($"Size of field {"_extraStancePercent"} must be less than {4096}KB");
		}
		*(int*)ptr175 = num174;
		byte* ptr176 = ptr;
		ptr += 4;
		ptr += _moveCostMobility.Serialize(ptr);
		int num175 = (int)(ptr - ptr176 - 4);
		if (num175 > 4194304)
		{
			throw new Exception($"Size of field {"_moveCostMobility"} must be less than {4096}KB");
		}
		*(int*)ptr176 = num175;
		byte* ptr177 = ptr;
		ptr += 4;
		ptr += _defendSkillKeepTime.Serialize(ptr);
		int num176 = (int)(ptr - ptr177 - 4);
		if (num176 > 4194304)
		{
			throw new Exception($"Size of field {"_defendSkillKeepTime"} must be less than {4096}KB");
		}
		*(int*)ptr177 = num176;
		byte* ptr178 = ptr;
		ptr += 4;
		ptr += _bounceRange.Serialize(ptr);
		int num177 = (int)(ptr - ptr178 - 4);
		if (num177 > 4194304)
		{
			throw new Exception($"Size of field {"_bounceRange"} must be less than {4096}KB");
		}
		*(int*)ptr178 = num177;
		byte* ptr179 = ptr;
		ptr += 4;
		ptr += _mindMarkKeepTime.Serialize(ptr);
		int num178 = (int)(ptr - ptr179 - 4);
		if (num178 > 4194304)
		{
			throw new Exception($"Size of field {"_mindMarkKeepTime"} must be less than {4096}KB");
		}
		*(int*)ptr179 = num178;
		byte* ptr180 = ptr;
		ptr += 4;
		ptr += _skillMobilityCostPerFrame.Serialize(ptr);
		int num179 = (int)(ptr - ptr180 - 4);
		if (num179 > 4194304)
		{
			throw new Exception($"Size of field {"_skillMobilityCostPerFrame"} must be less than {4096}KB");
		}
		*(int*)ptr180 = num179;
		byte* ptr181 = ptr;
		ptr += 4;
		ptr += _canAddWug.Serialize(ptr);
		int num180 = (int)(ptr - ptr181 - 4);
		if (num180 > 4194304)
		{
			throw new Exception($"Size of field {"_canAddWug"} must be less than {4096}KB");
		}
		*(int*)ptr181 = num180;
		byte* ptr182 = ptr;
		ptr += 4;
		ptr += _hasGodWeaponBuff.Serialize(ptr);
		int num181 = (int)(ptr - ptr182 - 4);
		if (num181 > 4194304)
		{
			throw new Exception($"Size of field {"_hasGodWeaponBuff"} must be less than {4096}KB");
		}
		*(int*)ptr182 = num181;
		byte* ptr183 = ptr;
		ptr += 4;
		ptr += _hasGodArmorBuff.Serialize(ptr);
		int num182 = (int)(ptr - ptr183 - 4);
		if (num182 > 4194304)
		{
			throw new Exception($"Size of field {"_hasGodArmorBuff"} must be less than {4096}KB");
		}
		*(int*)ptr183 = num182;
		byte* ptr184 = ptr;
		ptr += 4;
		ptr += _teammateCmdRequireGenerateValue.Serialize(ptr);
		int num183 = (int)(ptr - ptr184 - 4);
		if (num183 > 4194304)
		{
			throw new Exception($"Size of field {"_teammateCmdRequireGenerateValue"} must be less than {4096}KB");
		}
		*(int*)ptr184 = num183;
		byte* ptr185 = ptr;
		ptr += 4;
		ptr += _teammateCmdEffect.Serialize(ptr);
		int num184 = (int)(ptr - ptr185 - 4);
		if (num184 > 4194304)
		{
			throw new Exception($"Size of field {"_teammateCmdEffect"} must be less than {4096}KB");
		}
		*(int*)ptr185 = num184;
		byte* ptr186 = ptr;
		ptr += 4;
		ptr += _flawRecoverSpeed.Serialize(ptr);
		int num185 = (int)(ptr - ptr186 - 4);
		if (num185 > 4194304)
		{
			throw new Exception($"Size of field {"_flawRecoverSpeed"} must be less than {4096}KB");
		}
		*(int*)ptr186 = num185;
		byte* ptr187 = ptr;
		ptr += 4;
		ptr += _acupointRecoverSpeed.Serialize(ptr);
		int num186 = (int)(ptr - ptr187 - 4);
		if (num186 > 4194304)
		{
			throw new Exception($"Size of field {"_acupointRecoverSpeed"} must be less than {4096}KB");
		}
		*(int*)ptr187 = num186;
		byte* ptr188 = ptr;
		ptr += 4;
		ptr += _mindMarkRecoverSpeed.Serialize(ptr);
		int num187 = (int)(ptr - ptr188 - 4);
		if (num187 > 4194304)
		{
			throw new Exception($"Size of field {"_mindMarkRecoverSpeed"} must be less than {4096}KB");
		}
		*(int*)ptr188 = num187;
		byte* ptr189 = ptr;
		ptr += 4;
		ptr += _injuryAutoHealSpeed.Serialize(ptr);
		int num188 = (int)(ptr - ptr189 - 4);
		if (num188 > 4194304)
		{
			throw new Exception($"Size of field {"_injuryAutoHealSpeed"} must be less than {4096}KB");
		}
		*(int*)ptr189 = num188;
		byte* ptr190 = ptr;
		ptr += 4;
		ptr += _canRecoverBreath.Serialize(ptr);
		int num189 = (int)(ptr - ptr190 - 4);
		if (num189 > 4194304)
		{
			throw new Exception($"Size of field {"_canRecoverBreath"} must be less than {4096}KB");
		}
		*(int*)ptr190 = num189;
		byte* ptr191 = ptr;
		ptr += 4;
		ptr += _canRecoverStance.Serialize(ptr);
		int num190 = (int)(ptr - ptr191 - 4);
		if (num190 > 4194304)
		{
			throw new Exception($"Size of field {"_canRecoverStance"} must be less than {4096}KB");
		}
		*(int*)ptr191 = num190;
		byte* ptr192 = ptr;
		ptr += 4;
		ptr += _fatalDamageValue.Serialize(ptr);
		int num191 = (int)(ptr - ptr192 - 4);
		if (num191 > 4194304)
		{
			throw new Exception($"Size of field {"_fatalDamageValue"} must be less than {4096}KB");
		}
		*(int*)ptr192 = num191;
		byte* ptr193 = ptr;
		ptr += 4;
		ptr += _fatalDamageMarkCount.Serialize(ptr);
		int num192 = (int)(ptr - ptr193 - 4);
		if (num192 > 4194304)
		{
			throw new Exception($"Size of field {"_fatalDamageMarkCount"} must be less than {4096}KB");
		}
		*(int*)ptr193 = num192;
		byte* ptr194 = ptr;
		ptr += 4;
		ptr += _canFightBackDuringPrepareSkill.Serialize(ptr);
		int num193 = (int)(ptr - ptr194 - 4);
		if (num193 > 4194304)
		{
			throw new Exception($"Size of field {"_canFightBackDuringPrepareSkill"} must be less than {4096}KB");
		}
		*(int*)ptr194 = num193;
		byte* ptr195 = ptr;
		ptr += 4;
		ptr += _skillPrepareSpeed.Serialize(ptr);
		int num194 = (int)(ptr - ptr195 - 4);
		if (num194 > 4194304)
		{
			throw new Exception($"Size of field {"_skillPrepareSpeed"} must be less than {4096}KB");
		}
		*(int*)ptr195 = num194;
		byte* ptr196 = ptr;
		ptr += 4;
		ptr += _breathRecoverSpeed.Serialize(ptr);
		int num195 = (int)(ptr - ptr196 - 4);
		if (num195 > 4194304)
		{
			throw new Exception($"Size of field {"_breathRecoverSpeed"} must be less than {4096}KB");
		}
		*(int*)ptr196 = num195;
		byte* ptr197 = ptr;
		ptr += 4;
		ptr += _stanceRecoverSpeed.Serialize(ptr);
		int num196 = (int)(ptr - ptr197 - 4);
		if (num196 > 4194304)
		{
			throw new Exception($"Size of field {"_stanceRecoverSpeed"} must be less than {4096}KB");
		}
		*(int*)ptr197 = num196;
		byte* ptr198 = ptr;
		ptr += 4;
		ptr += _mobilityRecoverSpeed.Serialize(ptr);
		int num197 = (int)(ptr - ptr198 - 4);
		if (num197 > 4194304)
		{
			throw new Exception($"Size of field {"_mobilityRecoverSpeed"} must be less than {4096}KB");
		}
		*(int*)ptr198 = num197;
		byte* ptr199 = ptr;
		ptr += 4;
		ptr += _changeTrickProgressAddValue.Serialize(ptr);
		int num198 = (int)(ptr - ptr199 - 4);
		if (num198 > 4194304)
		{
			throw new Exception($"Size of field {"_changeTrickProgressAddValue"} must be less than {4096}KB");
		}
		*(int*)ptr199 = num198;
		byte* ptr200 = ptr;
		ptr += 4;
		ptr += _power.Serialize(ptr);
		int num199 = (int)(ptr - ptr200 - 4);
		if (num199 > 4194304)
		{
			throw new Exception($"Size of field {"_power"} must be less than {4096}KB");
		}
		*(int*)ptr200 = num199;
		byte* ptr201 = ptr;
		ptr += 4;
		ptr += _maxPower.Serialize(ptr);
		int num200 = (int)(ptr - ptr201 - 4);
		if (num200 > 4194304)
		{
			throw new Exception($"Size of field {"_maxPower"} must be less than {4096}KB");
		}
		*(int*)ptr201 = num200;
		byte* ptr202 = ptr;
		ptr += 4;
		ptr += _powerCanReduce.Serialize(ptr);
		int num201 = (int)(ptr - ptr202 - 4);
		if (num201 > 4194304)
		{
			throw new Exception($"Size of field {"_powerCanReduce"} must be less than {4096}KB");
		}
		*(int*)ptr202 = num201;
		byte* ptr203 = ptr;
		ptr += 4;
		ptr += _useRequirement.Serialize(ptr);
		int num202 = (int)(ptr - ptr203 - 4);
		if (num202 > 4194304)
		{
			throw new Exception($"Size of field {"_useRequirement"} must be less than {4096}KB");
		}
		*(int*)ptr203 = num202;
		byte* ptr204 = ptr;
		ptr += 4;
		ptr += _currInnerRatio.Serialize(ptr);
		int num203 = (int)(ptr - ptr204 - 4);
		if (num203 > 4194304)
		{
			throw new Exception($"Size of field {"_currInnerRatio"} must be less than {4096}KB");
		}
		*(int*)ptr204 = num203;
		byte* ptr205 = ptr;
		ptr += 4;
		ptr += _costBreathAndStance.Serialize(ptr);
		int num204 = (int)(ptr - ptr205 - 4);
		if (num204 > 4194304)
		{
			throw new Exception($"Size of field {"_costBreathAndStance"} must be less than {4096}KB");
		}
		*(int*)ptr205 = num204;
		byte* ptr206 = ptr;
		ptr += 4;
		ptr += _costBreath.Serialize(ptr);
		int num205 = (int)(ptr - ptr206 - 4);
		if (num205 > 4194304)
		{
			throw new Exception($"Size of field {"_costBreath"} must be less than {4096}KB");
		}
		*(int*)ptr206 = num205;
		byte* ptr207 = ptr;
		ptr += 4;
		ptr += _costStance.Serialize(ptr);
		int num206 = (int)(ptr - ptr207 - 4);
		if (num206 > 4194304)
		{
			throw new Exception($"Size of field {"_costStance"} must be less than {4096}KB");
		}
		*(int*)ptr207 = num206;
		byte* ptr208 = ptr;
		ptr += 4;
		ptr += _costMobility.Serialize(ptr);
		int num207 = (int)(ptr - ptr208 - 4);
		if (num207 > 4194304)
		{
			throw new Exception($"Size of field {"_costMobility"} must be less than {4096}KB");
		}
		*(int*)ptr208 = num207;
		byte* ptr209 = ptr;
		ptr += 4;
		ptr += _skillCostTricks.Serialize(ptr);
		int num208 = (int)(ptr - ptr209 - 4);
		if (num208 > 4194304)
		{
			throw new Exception($"Size of field {"_skillCostTricks"} must be less than {4096}KB");
		}
		*(int*)ptr209 = num208;
		byte* ptr210 = ptr;
		ptr += 4;
		ptr += _effectDirection.Serialize(ptr);
		int num209 = (int)(ptr - ptr210 - 4);
		if (num209 > 4194304)
		{
			throw new Exception($"Size of field {"_effectDirection"} must be less than {4096}KB");
		}
		*(int*)ptr210 = num209;
		byte* ptr211 = ptr;
		ptr += 4;
		ptr += _effectDirectionCanChange.Serialize(ptr);
		int num210 = (int)(ptr - ptr211 - 4);
		if (num210 > 4194304)
		{
			throw new Exception($"Size of field {"_effectDirectionCanChange"} must be less than {4096}KB");
		}
		*(int*)ptr211 = num210;
		byte* ptr212 = ptr;
		ptr += 4;
		ptr += _gridCost.Serialize(ptr);
		int num211 = (int)(ptr - ptr212 - 4);
		if (num211 > 4194304)
		{
			throw new Exception($"Size of field {"_gridCost"} must be less than {4096}KB");
		}
		*(int*)ptr212 = num211;
		byte* ptr213 = ptr;
		ptr += 4;
		ptr += _prepareTotalProgress.Serialize(ptr);
		int num212 = (int)(ptr - ptr213 - 4);
		if (num212 > 4194304)
		{
			throw new Exception($"Size of field {"_prepareTotalProgress"} must be less than {4096}KB");
		}
		*(int*)ptr213 = num212;
		byte* ptr214 = ptr;
		ptr += 4;
		ptr += _specificGridCount.Serialize(ptr);
		int num213 = (int)(ptr - ptr214 - 4);
		if (num213 > 4194304)
		{
			throw new Exception($"Size of field {"_specificGridCount"} must be less than {4096}KB");
		}
		*(int*)ptr214 = num213;
		byte* ptr215 = ptr;
		ptr += 4;
		ptr += _genericGridCount.Serialize(ptr);
		int num214 = (int)(ptr - ptr215 - 4);
		if (num214 > 4194304)
		{
			throw new Exception($"Size of field {"_genericGridCount"} must be less than {4096}KB");
		}
		*(int*)ptr215 = num214;
		byte* ptr216 = ptr;
		ptr += 4;
		ptr += _canInterrupt.Serialize(ptr);
		int num215 = (int)(ptr - ptr216 - 4);
		if (num215 > 4194304)
		{
			throw new Exception($"Size of field {"_canInterrupt"} must be less than {4096}KB");
		}
		*(int*)ptr216 = num215;
		byte* ptr217 = ptr;
		ptr += 4;
		ptr += _interruptOdds.Serialize(ptr);
		int num216 = (int)(ptr - ptr217 - 4);
		if (num216 > 4194304)
		{
			throw new Exception($"Size of field {"_interruptOdds"} must be less than {4096}KB");
		}
		*(int*)ptr217 = num216;
		byte* ptr218 = ptr;
		ptr += 4;
		ptr += _canSilence.Serialize(ptr);
		int num217 = (int)(ptr - ptr218 - 4);
		if (num217 > 4194304)
		{
			throw new Exception($"Size of field {"_canSilence"} must be less than {4096}KB");
		}
		*(int*)ptr218 = num217;
		byte* ptr219 = ptr;
		ptr += 4;
		ptr += _silenceOdds.Serialize(ptr);
		int num218 = (int)(ptr - ptr219 - 4);
		if (num218 > 4194304)
		{
			throw new Exception($"Size of field {"_silenceOdds"} must be less than {4096}KB");
		}
		*(int*)ptr219 = num218;
		byte* ptr220 = ptr;
		ptr += 4;
		ptr += _canCastWithBrokenBodyPart.Serialize(ptr);
		int num219 = (int)(ptr - ptr220 - 4);
		if (num219 > 4194304)
		{
			throw new Exception($"Size of field {"_canCastWithBrokenBodyPart"} must be less than {4096}KB");
		}
		*(int*)ptr220 = num219;
		byte* ptr221 = ptr;
		ptr += 4;
		ptr += _addPowerCanBeRemoved.Serialize(ptr);
		int num220 = (int)(ptr - ptr221 - 4);
		if (num220 > 4194304)
		{
			throw new Exception($"Size of field {"_addPowerCanBeRemoved"} must be less than {4096}KB");
		}
		*(int*)ptr221 = num220;
		byte* ptr222 = ptr;
		ptr += 4;
		ptr += _skillType.Serialize(ptr);
		int num221 = (int)(ptr - ptr222 - 4);
		if (num221 > 4194304)
		{
			throw new Exception($"Size of field {"_skillType"} must be less than {4096}KB");
		}
		*(int*)ptr222 = num221;
		byte* ptr223 = ptr;
		ptr += 4;
		ptr += _effectCountCanChange.Serialize(ptr);
		int num222 = (int)(ptr - ptr223 - 4);
		if (num222 > 4194304)
		{
			throw new Exception($"Size of field {"_effectCountCanChange"} must be less than {4096}KB");
		}
		*(int*)ptr223 = num222;
		byte* ptr224 = ptr;
		ptr += 4;
		ptr += _canCastInDefend.Serialize(ptr);
		int num223 = (int)(ptr - ptr224 - 4);
		if (num223 > 4194304)
		{
			throw new Exception($"Size of field {"_canCastInDefend"} must be less than {4096}KB");
		}
		*(int*)ptr224 = num223;
		byte* ptr225 = ptr;
		ptr += 4;
		ptr += _hitDistribution.Serialize(ptr);
		int num224 = (int)(ptr - ptr225 - 4);
		if (num224 > 4194304)
		{
			throw new Exception($"Size of field {"_hitDistribution"} must be less than {4096}KB");
		}
		*(int*)ptr225 = num224;
		byte* ptr226 = ptr;
		ptr += 4;
		ptr += _canCastOnLackBreath.Serialize(ptr);
		int num225 = (int)(ptr - ptr226 - 4);
		if (num225 > 4194304)
		{
			throw new Exception($"Size of field {"_canCastOnLackBreath"} must be less than {4096}KB");
		}
		*(int*)ptr226 = num225;
		byte* ptr227 = ptr;
		ptr += 4;
		ptr += _canCastOnLackStance.Serialize(ptr);
		int num226 = (int)(ptr - ptr227 - 4);
		if (num226 > 4194304)
		{
			throw new Exception($"Size of field {"_canCastOnLackStance"} must be less than {4096}KB");
		}
		*(int*)ptr227 = num226;
		byte* ptr228 = ptr;
		ptr += 4;
		ptr += _costBreathOnCast.Serialize(ptr);
		int num227 = (int)(ptr - ptr228 - 4);
		if (num227 > 4194304)
		{
			throw new Exception($"Size of field {"_costBreathOnCast"} must be less than {4096}KB");
		}
		*(int*)ptr228 = num227;
		byte* ptr229 = ptr;
		ptr += 4;
		ptr += _costStanceOnCast.Serialize(ptr);
		int num228 = (int)(ptr - ptr229 - 4);
		if (num228 > 4194304)
		{
			throw new Exception($"Size of field {"_costStanceOnCast"} must be less than {4096}KB");
		}
		*(int*)ptr229 = num228;
		byte* ptr230 = ptr;
		ptr += 4;
		ptr += _canUseMobilityAsBreath.Serialize(ptr);
		int num229 = (int)(ptr - ptr230 - 4);
		if (num229 > 4194304)
		{
			throw new Exception($"Size of field {"_canUseMobilityAsBreath"} must be less than {4096}KB");
		}
		*(int*)ptr230 = num229;
		byte* ptr231 = ptr;
		ptr += 4;
		ptr += _canUseMobilityAsStance.Serialize(ptr);
		int num230 = (int)(ptr - ptr231 - 4);
		if (num230 > 4194304)
		{
			throw new Exception($"Size of field {"_canUseMobilityAsStance"} must be less than {4096}KB");
		}
		*(int*)ptr231 = num230;
		byte* ptr232 = ptr;
		ptr += 4;
		ptr += _castCostNeiliAllocation.Serialize(ptr);
		int num231 = (int)(ptr - ptr232 - 4);
		if (num231 > 4194304)
		{
			throw new Exception($"Size of field {"_castCostNeiliAllocation"} must be less than {4096}KB");
		}
		*(int*)ptr232 = num231;
		byte* ptr233 = ptr;
		ptr += 4;
		ptr += _acceptPoisonResist.Serialize(ptr);
		int num232 = (int)(ptr - ptr233 - 4);
		if (num232 > 4194304)
		{
			throw new Exception($"Size of field {"_acceptPoisonResist"} must be less than {4096}KB");
		}
		*(int*)ptr233 = num232;
		byte* ptr234 = ptr;
		ptr += 4;
		ptr += _makePoisonResist.Serialize(ptr);
		int num233 = (int)(ptr - ptr234 - 4);
		if (num233 > 4194304)
		{
			throw new Exception($"Size of field {"_makePoisonResist"} must be less than {4096}KB");
		}
		*(int*)ptr234 = num233;
		byte* ptr235 = ptr;
		ptr += 4;
		ptr += _canCriticalHit.Serialize(ptr);
		int num234 = (int)(ptr - ptr235 - 4);
		if (num234 > 4194304)
		{
			throw new Exception($"Size of field {"_canCriticalHit"} must be less than {4096}KB");
		}
		*(int*)ptr235 = num234;
		byte* ptr236 = ptr;
		ptr += 4;
		ptr += _canCostNeiliAllocationEffect.Serialize(ptr);
		int num235 = (int)(ptr - ptr236 - 4);
		if (num235 > 4194304)
		{
			throw new Exception($"Size of field {"_canCostNeiliAllocationEffect"} must be less than {4096}KB");
		}
		*(int*)ptr236 = num235;
		byte* ptr237 = ptr;
		ptr += 4;
		ptr += _consummateLevelRelatedMainAttributesHitValues.Serialize(ptr);
		int num236 = (int)(ptr - ptr237 - 4);
		if (num236 > 4194304)
		{
			throw new Exception($"Size of field {"_consummateLevelRelatedMainAttributesHitValues"} must be less than {4096}KB");
		}
		*(int*)ptr237 = num236;
		byte* ptr238 = ptr;
		ptr += 4;
		ptr += _consummateLevelRelatedMainAttributesAvoidValues.Serialize(ptr);
		int num237 = (int)(ptr - ptr238 - 4);
		if (num237 > 4194304)
		{
			throw new Exception($"Size of field {"_consummateLevelRelatedMainAttributesAvoidValues"} must be less than {4096}KB");
		}
		*(int*)ptr238 = num237;
		byte* ptr239 = ptr;
		ptr += 4;
		ptr += _consummateLevelRelatedMainAttributesPenetrations.Serialize(ptr);
		int num238 = (int)(ptr - ptr239 - 4);
		if (num238 > 4194304)
		{
			throw new Exception($"Size of field {"_consummateLevelRelatedMainAttributesPenetrations"} must be less than {4096}KB");
		}
		*(int*)ptr239 = num238;
		byte* ptr240 = ptr;
		ptr += 4;
		ptr += _consummateLevelRelatedMainAttributesPenetrationResists.Serialize(ptr);
		int num239 = (int)(ptr - ptr240 - 4);
		if (num239 > 4194304)
		{
			throw new Exception($"Size of field {"_consummateLevelRelatedMainAttributesPenetrationResists"} must be less than {4096}KB");
		}
		*(int*)ptr240 = num239;
		byte* ptr241 = ptr;
		ptr += 4;
		ptr += _skillAlsoAsFiveElements.Serialize(ptr);
		int num240 = (int)(ptr - ptr241 - 4);
		if (num240 > 4194304)
		{
			throw new Exception($"Size of field {"_skillAlsoAsFiveElements"} must be less than {4096}KB");
		}
		*(int*)ptr241 = num240;
		byte* ptr242 = ptr;
		ptr += 4;
		ptr += _innerInjuryImmunity.Serialize(ptr);
		int num241 = (int)(ptr - ptr242 - 4);
		if (num241 > 4194304)
		{
			throw new Exception($"Size of field {"_innerInjuryImmunity"} must be less than {4096}KB");
		}
		*(int*)ptr242 = num241;
		byte* ptr243 = ptr;
		ptr += 4;
		ptr += _outerInjuryImmunity.Serialize(ptr);
		int num242 = (int)(ptr - ptr243 - 4);
		if (num242 > 4194304)
		{
			throw new Exception($"Size of field {"_outerInjuryImmunity"} must be less than {4096}KB");
		}
		*(int*)ptr243 = num242;
		byte* ptr244 = ptr;
		ptr += 4;
		ptr += _poisonAffectThreshold.Serialize(ptr);
		int num243 = (int)(ptr - ptr244 - 4);
		if (num243 > 4194304)
		{
			throw new Exception($"Size of field {"_poisonAffectThreshold"} must be less than {4096}KB");
		}
		*(int*)ptr244 = num243;
		byte* ptr245 = ptr;
		ptr += 4;
		ptr += _lockDistance.Serialize(ptr);
		int num244 = (int)(ptr - ptr245 - 4);
		if (num244 > 4194304)
		{
			throw new Exception($"Size of field {"_lockDistance"} must be less than {4096}KB");
		}
		*(int*)ptr245 = num244;
		byte* ptr246 = ptr;
		ptr += 4;
		ptr += _resistOfAllPoison.Serialize(ptr);
		int num245 = (int)(ptr - ptr246 - 4);
		if (num245 > 4194304)
		{
			throw new Exception($"Size of field {"_resistOfAllPoison"} must be less than {4096}KB");
		}
		*(int*)ptr246 = num245;
		byte* ptr247 = ptr;
		ptr += 4;
		ptr += _makePoisonTarget.Serialize(ptr);
		int num246 = (int)(ptr - ptr247 - 4);
		if (num246 > 4194304)
		{
			throw new Exception($"Size of field {"_makePoisonTarget"} must be less than {4096}KB");
		}
		*(int*)ptr247 = num246;
		byte* ptr248 = ptr;
		ptr += 4;
		ptr += _acceptPoisonTarget.Serialize(ptr);
		int num247 = (int)(ptr - ptr248 - 4);
		if (num247 > 4194304)
		{
			throw new Exception($"Size of field {"_acceptPoisonTarget"} must be less than {4096}KB");
		}
		*(int*)ptr248 = num247;
		byte* ptr249 = ptr;
		ptr += 4;
		ptr += _certainCriticalHit.Serialize(ptr);
		int num248 = (int)(ptr - ptr249 - 4);
		if (num248 > 4194304)
		{
			throw new Exception($"Size of field {"_certainCriticalHit"} must be less than {4096}KB");
		}
		*(int*)ptr249 = num248;
		byte* ptr250 = ptr;
		ptr += 4;
		ptr += _mindMarkCount.Serialize(ptr);
		int num249 = (int)(ptr - ptr250 - 4);
		if (num249 > 4194304)
		{
			throw new Exception($"Size of field {"_mindMarkCount"} must be less than {4096}KB");
		}
		*(int*)ptr250 = num249;
		byte* ptr251 = ptr;
		ptr += 4;
		ptr += _canFightBackWithHit.Serialize(ptr);
		int num250 = (int)(ptr - ptr251 - 4);
		if (num250 > 4194304)
		{
			throw new Exception($"Size of field {"_canFightBackWithHit"} must be less than {4096}KB");
		}
		*(int*)ptr251 = num250;
		byte* ptr252 = ptr;
		ptr += 4;
		ptr += _inevitableHit.Serialize(ptr);
		int num251 = (int)(ptr - ptr252 - 4);
		if (num251 > 4194304)
		{
			throw new Exception($"Size of field {"_inevitableHit"} must be less than {4096}KB");
		}
		*(int*)ptr252 = num251;
		byte* ptr253 = ptr;
		ptr += 4;
		ptr += _attackCanPursue.Serialize(ptr);
		int num252 = (int)(ptr - ptr253 - 4);
		if (num252 > 4194304)
		{
			throw new Exception($"Size of field {"_attackCanPursue"} must be less than {4096}KB");
		}
		*(int*)ptr253 = num252;
		byte* ptr254 = ptr;
		ptr += 4;
		ptr += _combatSkillDataEffectList.Serialize(ptr);
		int num253 = (int)(ptr - ptr254 - 4);
		if (num253 > 4194304)
		{
			throw new Exception($"Size of field {"_combatSkillDataEffectList"} must be less than {4096}KB");
		}
		*(int*)ptr254 = num253;
		byte* ptr255 = ptr;
		ptr += 4;
		ptr += _criticalOdds.Serialize(ptr);
		int num254 = (int)(ptr - ptr255 - 4);
		if (num254 > 4194304)
		{
			throw new Exception($"Size of field {"_criticalOdds"} must be less than {4096}KB");
		}
		*(int*)ptr255 = num254;
		byte* ptr256 = ptr;
		ptr += 4;
		ptr += _stanceCostByEffect.Serialize(ptr);
		int num255 = (int)(ptr - ptr256 - 4);
		if (num255 > 4194304)
		{
			throw new Exception($"Size of field {"_stanceCostByEffect"} must be less than {4096}KB");
		}
		*(int*)ptr256 = num255;
		byte* ptr257 = ptr;
		ptr += 4;
		ptr += _breathCostByEffect.Serialize(ptr);
		int num256 = (int)(ptr - ptr257 - 4);
		if (num256 > 4194304)
		{
			throw new Exception($"Size of field {"_breathCostByEffect"} must be less than {4096}KB");
		}
		*(int*)ptr257 = num256;
		byte* ptr258 = ptr;
		ptr += 4;
		ptr += _powerAddRatio.Serialize(ptr);
		int num257 = (int)(ptr - ptr258 - 4);
		if (num257 > 4194304)
		{
			throw new Exception($"Size of field {"_powerAddRatio"} must be less than {4096}KB");
		}
		*(int*)ptr258 = num257;
		byte* ptr259 = ptr;
		ptr += 4;
		ptr += _powerReduceRatio.Serialize(ptr);
		int num258 = (int)(ptr - ptr259 - 4);
		if (num258 > 4194304)
		{
			throw new Exception($"Size of field {"_powerReduceRatio"} must be less than {4096}KB");
		}
		*(int*)ptr259 = num258;
		byte* ptr260 = ptr;
		ptr += 4;
		ptr += _poisonAffectProduceValue.Serialize(ptr);
		int num259 = (int)(ptr - ptr260 - 4);
		if (num259 > 4194304)
		{
			throw new Exception($"Size of field {"_poisonAffectProduceValue"} must be less than {4096}KB");
		}
		*(int*)ptr260 = num259;
		byte* ptr261 = ptr;
		ptr += 4;
		ptr += _canReadingOnMonthChange.Serialize(ptr);
		int num260 = (int)(ptr - ptr261 - 4);
		if (num260 > 4194304)
		{
			throw new Exception($"Size of field {"_canReadingOnMonthChange"} must be less than {4096}KB");
		}
		*(int*)ptr261 = num260;
		byte* ptr262 = ptr;
		ptr += 4;
		ptr += _medicineEffect.Serialize(ptr);
		int num261 = (int)(ptr - ptr262 - 4);
		if (num261 > 4194304)
		{
			throw new Exception($"Size of field {"_medicineEffect"} must be less than {4096}KB");
		}
		*(int*)ptr262 = num261;
		byte* ptr263 = ptr;
		ptr += 4;
		ptr += _xiangshuInfectionDelta.Serialize(ptr);
		int num262 = (int)(ptr - ptr263 - 4);
		if (num262 > 4194304)
		{
			throw new Exception($"Size of field {"_xiangshuInfectionDelta"} must be less than {4096}KB");
		}
		*(int*)ptr263 = num262;
		byte* ptr264 = ptr;
		ptr += 4;
		ptr += _healthDelta.Serialize(ptr);
		int num263 = (int)(ptr - ptr264 - 4);
		if (num263 > 4194304)
		{
			throw new Exception($"Size of field {"_healthDelta"} must be less than {4096}KB");
		}
		*(int*)ptr264 = num263;
		byte* ptr265 = ptr;
		ptr += 4;
		ptr += _weaponSilenceFrame.Serialize(ptr);
		int num264 = (int)(ptr - ptr265 - 4);
		if (num264 > 4194304)
		{
			throw new Exception($"Size of field {"_weaponSilenceFrame"} must be less than {4096}KB");
		}
		*(int*)ptr265 = num264;
		byte* ptr266 = ptr;
		ptr += 4;
		ptr += _silenceFrame.Serialize(ptr);
		int num265 = (int)(ptr - ptr266 - 4);
		if (num265 > 4194304)
		{
			throw new Exception($"Size of field {"_silenceFrame"} must be less than {4096}KB");
		}
		*(int*)ptr266 = num265;
		byte* ptr267 = ptr;
		ptr += 4;
		ptr += _currAgeDelta.Serialize(ptr);
		int num266 = (int)(ptr - ptr267 - 4);
		if (num266 > 4194304)
		{
			throw new Exception($"Size of field {"_currAgeDelta"} must be less than {4096}KB");
		}
		*(int*)ptr267 = num266;
		byte* ptr268 = ptr;
		ptr += 4;
		ptr += _goneMadInAllBreak.Serialize(ptr);
		int num267 = (int)(ptr - ptr268 - 4);
		if (num267 > 4194304)
		{
			throw new Exception($"Size of field {"_goneMadInAllBreak"} must be less than {4096}KB");
		}
		*(int*)ptr268 = num267;
		byte* ptr269 = ptr;
		ptr += 4;
		ptr += _makeLoveRateOnMonthChange.Serialize(ptr);
		int num268 = (int)(ptr - ptr269 - 4);
		if (num268 > 4194304)
		{
			throw new Exception($"Size of field {"_makeLoveRateOnMonthChange"} must be less than {4096}KB");
		}
		*(int*)ptr269 = num268;
		byte* ptr270 = ptr;
		ptr += 4;
		ptr += _canAutoHealOnMonthChange.Serialize(ptr);
		int num269 = (int)(ptr - ptr270 - 4);
		if (num269 > 4194304)
		{
			throw new Exception($"Size of field {"_canAutoHealOnMonthChange"} must be less than {4096}KB");
		}
		*(int*)ptr270 = num269;
		byte* ptr271 = ptr;
		ptr += 4;
		ptr += _happinessDelta.Serialize(ptr);
		int num270 = (int)(ptr - ptr271 - 4);
		if (num270 > 4194304)
		{
			throw new Exception($"Size of field {"_happinessDelta"} must be less than {4096}KB");
		}
		*(int*)ptr271 = num270;
		byte* ptr272 = ptr;
		ptr += 4;
		ptr += _teammateCmdCanUse.Serialize(ptr);
		int num271 = (int)(ptr - ptr272 - 4);
		if (num271 > 4194304)
		{
			throw new Exception($"Size of field {"_teammateCmdCanUse"} must be less than {4096}KB");
		}
		*(int*)ptr272 = num271;
		byte* ptr273 = ptr;
		ptr += 4;
		ptr += _mixPoisonInfinityAffect.Serialize(ptr);
		int num272 = (int)(ptr - ptr273 - 4);
		if (num272 > 4194304)
		{
			throw new Exception($"Size of field {"_mixPoisonInfinityAffect"} must be less than {4096}KB");
		}
		*(int*)ptr273 = num272;
		byte* ptr274 = ptr;
		ptr += 4;
		ptr += _attackRangeMaxAcupoint.Serialize(ptr);
		int num273 = (int)(ptr - ptr274 - 4);
		if (num273 > 4194304)
		{
			throw new Exception($"Size of field {"_attackRangeMaxAcupoint"} must be less than {4096}KB");
		}
		*(int*)ptr274 = num273;
		byte* ptr275 = ptr;
		ptr += 4;
		ptr += _maxMobilityPercent.Serialize(ptr);
		int num274 = (int)(ptr - ptr275 - 4);
		if (num274 > 4194304)
		{
			throw new Exception($"Size of field {"_maxMobilityPercent"} must be less than {4096}KB");
		}
		*(int*)ptr275 = num274;
		byte* ptr276 = ptr;
		ptr += 4;
		ptr += _makeMindDamage.Serialize(ptr);
		int num275 = (int)(ptr - ptr276 - 4);
		if (num275 > 4194304)
		{
			throw new Exception($"Size of field {"_makeMindDamage"} must be less than {4096}KB");
		}
		*(int*)ptr276 = num275;
		byte* ptr277 = ptr;
		ptr += 4;
		ptr += _acceptMindDamage.Serialize(ptr);
		int num276 = (int)(ptr - ptr277 - 4);
		if (num276 > 4194304)
		{
			throw new Exception($"Size of field {"_acceptMindDamage"} must be less than {4096}KB");
		}
		*(int*)ptr277 = num276;
		byte* ptr278 = ptr;
		ptr += 4;
		ptr += _hitAddByTempValue.Serialize(ptr);
		int num277 = (int)(ptr - ptr278 - 4);
		if (num277 > 4194304)
		{
			throw new Exception($"Size of field {"_hitAddByTempValue"} must be less than {4096}KB");
		}
		*(int*)ptr278 = num277;
		byte* ptr279 = ptr;
		ptr += 4;
		ptr += _avoidAddByTempValue.Serialize(ptr);
		int num278 = (int)(ptr - ptr279 - 4);
		if (num278 > 4194304)
		{
			throw new Exception($"Size of field {"_avoidAddByTempValue"} must be less than {4096}KB");
		}
		*(int*)ptr279 = num278;
		byte* ptr280 = ptr;
		ptr += 4;
		ptr += _ignoreEquipmentOverload.Serialize(ptr);
		int num279 = (int)(ptr - ptr280 - 4);
		if (num279 > 4194304)
		{
			throw new Exception($"Size of field {"_ignoreEquipmentOverload"} must be less than {4096}KB");
		}
		*(int*)ptr280 = num279;
		byte* ptr281 = ptr;
		ptr += 4;
		ptr += _canCostEnemyUsableTricks.Serialize(ptr);
		int num280 = (int)(ptr - ptr281 - 4);
		if (num280 > 4194304)
		{
			throw new Exception($"Size of field {"_canCostEnemyUsableTricks"} must be less than {4096}KB");
		}
		*(int*)ptr281 = num280;
		byte* ptr282 = ptr;
		ptr += 4;
		ptr += _ignoreArmor.Serialize(ptr);
		int num281 = (int)(ptr - ptr282 - 4);
		if (num281 > 4194304)
		{
			throw new Exception($"Size of field {"_ignoreArmor"} must be less than {4096}KB");
		}
		*(int*)ptr282 = num281;
		byte* ptr283 = ptr;
		ptr += 4;
		ptr += _unyieldingFallen.Serialize(ptr);
		int num282 = (int)(ptr - ptr283 - 4);
		if (num282 > 4194304)
		{
			throw new Exception($"Size of field {"_unyieldingFallen"} must be less than {4096}KB");
		}
		*(int*)ptr283 = num282;
		byte* ptr284 = ptr;
		ptr += 4;
		ptr += _normalAttackPrepareFrame.Serialize(ptr);
		int num283 = (int)(ptr - ptr284 - 4);
		if (num283 > 4194304)
		{
			throw new Exception($"Size of field {"_normalAttackPrepareFrame"} must be less than {4096}KB");
		}
		*(int*)ptr284 = num283;
		byte* ptr285 = ptr;
		ptr += 4;
		ptr += _canCostUselessTricks.Serialize(ptr);
		int num284 = (int)(ptr - ptr285 - 4);
		if (num284 > 4194304)
		{
			throw new Exception($"Size of field {"_canCostUselessTricks"} must be less than {4096}KB");
		}
		*(int*)ptr285 = num284;
		byte* ptr286 = ptr;
		ptr += 4;
		ptr += _defendSkillCanAffect.Serialize(ptr);
		int num285 = (int)(ptr - ptr286 - 4);
		if (num285 > 4194304)
		{
			throw new Exception($"Size of field {"_defendSkillCanAffect"} must be less than {4096}KB");
		}
		*(int*)ptr286 = num285;
		byte* ptr287 = ptr;
		ptr += 4;
		ptr += _assistSkillCanAffect.Serialize(ptr);
		int num286 = (int)(ptr - ptr287 - 4);
		if (num286 > 4194304)
		{
			throw new Exception($"Size of field {"_assistSkillCanAffect"} must be less than {4096}KB");
		}
		*(int*)ptr287 = num286;
		byte* ptr288 = ptr;
		ptr += 4;
		ptr += _agileSkillCanAffect.Serialize(ptr);
		int num287 = (int)(ptr - ptr288 - 4);
		if (num287 > 4194304)
		{
			throw new Exception($"Size of field {"_agileSkillCanAffect"} must be less than {4096}KB");
		}
		*(int*)ptr288 = num287;
		byte* ptr289 = ptr;
		ptr += 4;
		ptr += _allMarkChangeToMind.Serialize(ptr);
		int num288 = (int)(ptr - ptr289 - 4);
		if (num288 > 4194304)
		{
			throw new Exception($"Size of field {"_allMarkChangeToMind"} must be less than {4096}KB");
		}
		*(int*)ptr289 = num288;
		byte* ptr290 = ptr;
		ptr += 4;
		ptr += _mindMarkChangeToFatal.Serialize(ptr);
		int num289 = (int)(ptr - ptr290 - 4);
		if (num289 > 4194304)
		{
			throw new Exception($"Size of field {"_mindMarkChangeToFatal"} must be less than {4096}KB");
		}
		*(int*)ptr290 = num289;
		byte* ptr291 = ptr;
		ptr += 4;
		ptr += _canCast.Serialize(ptr);
		int num290 = (int)(ptr - ptr291 - 4);
		if (num290 > 4194304)
		{
			throw new Exception($"Size of field {"_canCast"} must be less than {4096}KB");
		}
		*(int*)ptr291 = num290;
		byte* ptr292 = ptr;
		ptr += 4;
		ptr += _inevitableAvoid.Serialize(ptr);
		int num291 = (int)(ptr - ptr292 - 4);
		if (num291 > 4194304)
		{
			throw new Exception($"Size of field {"_inevitableAvoid"} must be less than {4096}KB");
		}
		*(int*)ptr292 = num291;
		byte* ptr293 = ptr;
		ptr += 4;
		ptr += _powerEffectReverse.Serialize(ptr);
		int num292 = (int)(ptr - ptr293 - 4);
		if (num292 > 4194304)
		{
			throw new Exception($"Size of field {"_powerEffectReverse"} must be less than {4096}KB");
		}
		*(int*)ptr293 = num292;
		byte* ptr294 = ptr;
		ptr += 4;
		ptr += _featureBonusReverse.Serialize(ptr);
		int num293 = (int)(ptr - ptr294 - 4);
		if (num293 > 4194304)
		{
			throw new Exception($"Size of field {"_featureBonusReverse"} must be less than {4096}KB");
		}
		*(int*)ptr294 = num293;
		byte* ptr295 = ptr;
		ptr += 4;
		ptr += _wugFatalDamageValue.Serialize(ptr);
		int num294 = (int)(ptr - ptr295 - 4);
		if (num294 > 4194304)
		{
			throw new Exception($"Size of field {"_wugFatalDamageValue"} must be less than {4096}KB");
		}
		*(int*)ptr295 = num294;
		byte* ptr296 = ptr;
		ptr += 4;
		ptr += _canRecoverHealthOnMonthChange.Serialize(ptr);
		int num295 = (int)(ptr - ptr296 - 4);
		if (num295 > 4194304)
		{
			throw new Exception($"Size of field {"_canRecoverHealthOnMonthChange"} must be less than {4096}KB");
		}
		*(int*)ptr296 = num295;
		byte* ptr297 = ptr;
		ptr += 4;
		ptr += _takeRevengeRateOnMonthChange.Serialize(ptr);
		int num296 = (int)(ptr - ptr297 - 4);
		if (num296 > 4194304)
		{
			throw new Exception($"Size of field {"_takeRevengeRateOnMonthChange"} must be less than {4096}KB");
		}
		*(int*)ptr297 = num296;
		byte* ptr298 = ptr;
		ptr += 4;
		ptr += _consummateLevelBonus.Serialize(ptr);
		int num297 = (int)(ptr - ptr298 - 4);
		if (num297 > 4194304)
		{
			throw new Exception($"Size of field {"_consummateLevelBonus"} must be less than {4096}KB");
		}
		*(int*)ptr298 = num297;
		byte* ptr299 = ptr;
		ptr += 4;
		ptr += _neiliDelta.Serialize(ptr);
		int num298 = (int)(ptr - ptr299 - 4);
		if (num298 > 4194304)
		{
			throw new Exception($"Size of field {"_neiliDelta"} must be less than {4096}KB");
		}
		*(int*)ptr299 = num298;
		byte* ptr300 = ptr;
		ptr += 4;
		ptr += _canMakeLoveSpecialOnMonthChange.Serialize(ptr);
		int num299 = (int)(ptr - ptr300 - 4);
		if (num299 > 4194304)
		{
			throw new Exception($"Size of field {"_canMakeLoveSpecialOnMonthChange"} must be less than {4096}KB");
		}
		*(int*)ptr300 = num299;
		byte* ptr301 = ptr;
		ptr += 4;
		ptr += _healAcupointSpeed.Serialize(ptr);
		int num300 = (int)(ptr - ptr301 - 4);
		if (num300 > 4194304)
		{
			throw new Exception($"Size of field {"_healAcupointSpeed"} must be less than {4096}KB");
		}
		*(int*)ptr301 = num300;
		byte* ptr302 = ptr;
		ptr += 4;
		ptr += _maxChangeTrickCount.Serialize(ptr);
		int num301 = (int)(ptr - ptr302 - 4);
		if (num301 > 4194304)
		{
			throw new Exception($"Size of field {"_maxChangeTrickCount"} must be less than {4096}KB");
		}
		*(int*)ptr302 = num301;
		byte* ptr303 = ptr;
		ptr += 4;
		ptr += _convertCostBreathAndStance.Serialize(ptr);
		int num302 = (int)(ptr - ptr303 - 4);
		if (num302 > 4194304)
		{
			throw new Exception($"Size of field {"_convertCostBreathAndStance"} must be less than {4096}KB");
		}
		*(int*)ptr303 = num302;
		byte* ptr304 = ptr;
		ptr += 4;
		ptr += _personalitiesAll.Serialize(ptr);
		int num303 = (int)(ptr - ptr304 - 4);
		if (num303 > 4194304)
		{
			throw new Exception($"Size of field {"_personalitiesAll"} must be less than {4096}KB");
		}
		*(int*)ptr304 = num303;
		byte* ptr305 = ptr;
		ptr += 4;
		ptr += _finalFatalDamageMarkCount.Serialize(ptr);
		int num304 = (int)(ptr - ptr305 - 4);
		if (num304 > 4194304)
		{
			throw new Exception($"Size of field {"_finalFatalDamageMarkCount"} must be less than {4096}KB");
		}
		*(int*)ptr305 = num304;
		byte* ptr306 = ptr;
		ptr += 4;
		ptr += _infinityMindMarkProgress.Serialize(ptr);
		int num305 = (int)(ptr - ptr306 - 4);
		if (num305 > 4194304)
		{
			throw new Exception($"Size of field {"_infinityMindMarkProgress"} must be less than {4096}KB");
		}
		*(int*)ptr306 = num305;
		byte* ptr307 = ptr;
		ptr += 4;
		ptr += _combatSkillAiScorePower.Serialize(ptr);
		int num306 = (int)(ptr - ptr307 - 4);
		if (num306 > 4194304)
		{
			throw new Exception($"Size of field {"_combatSkillAiScorePower"} must be less than {4096}KB");
		}
		*(int*)ptr307 = num306;
		byte* ptr308 = ptr;
		ptr += 4;
		ptr += _normalAttackChangeToUnlockAttack.Serialize(ptr);
		int num307 = (int)(ptr - ptr308 - 4);
		if (num307 > 4194304)
		{
			throw new Exception($"Size of field {"_normalAttackChangeToUnlockAttack"} must be less than {4096}KB");
		}
		*(int*)ptr308 = num307;
		byte* ptr309 = ptr;
		ptr += 4;
		ptr += _attackBodyPartOdds.Serialize(ptr);
		int num308 = (int)(ptr - ptr309 - 4);
		if (num308 > 4194304)
		{
			throw new Exception($"Size of field {"_attackBodyPartOdds"} must be less than {4096}KB");
		}
		*(int*)ptr309 = num308;
		byte* ptr310 = ptr;
		ptr += 4;
		ptr += _changeDurability.Serialize(ptr);
		int num309 = (int)(ptr - ptr310 - 4);
		if (num309 > 4194304)
		{
			throw new Exception($"Size of field {"_changeDurability"} must be less than {4096}KB");
		}
		*(int*)ptr310 = num309;
		byte* ptr311 = ptr;
		ptr += 4;
		ptr += _equipmentBonus.Serialize(ptr);
		int num310 = (int)(ptr - ptr311 - 4);
		if (num310 > 4194304)
		{
			throw new Exception($"Size of field {"_equipmentBonus"} must be less than {4096}KB");
		}
		*(int*)ptr311 = num310;
		byte* ptr312 = ptr;
		ptr += 4;
		ptr += _equipmentWeight.Serialize(ptr);
		int num311 = (int)(ptr - ptr312 - 4);
		if (num311 > 4194304)
		{
			throw new Exception($"Size of field {"_equipmentWeight"} must be less than {4096}KB");
		}
		*(int*)ptr312 = num311;
		byte* ptr313 = ptr;
		ptr += 4;
		ptr += _rawCreateEffectList.Serialize(ptr);
		int num312 = (int)(ptr - ptr313 - 4);
		if (num312 > 4194304)
		{
			throw new Exception($"Size of field {"_rawCreateEffectList"} must be less than {4096}KB");
		}
		*(int*)ptr313 = num312;
		byte* ptr314 = ptr;
		ptr += 4;
		ptr += _jiTrickAsWeaponTrickCount.Serialize(ptr);
		int num313 = (int)(ptr - ptr314 - 4);
		if (num313 > 4194304)
		{
			throw new Exception($"Size of field {"_jiTrickAsWeaponTrickCount"} must be less than {4096}KB");
		}
		*(int*)ptr314 = num313;
		byte* ptr315 = ptr;
		ptr += 4;
		ptr += _uselessTrickAsJiTrickCount.Serialize(ptr);
		int num314 = (int)(ptr - ptr315 - 4);
		if (num314 > 4194304)
		{
			throw new Exception($"Size of field {"_uselessTrickAsJiTrickCount"} must be less than {4096}KB");
		}
		*(int*)ptr315 = num314;
		byte* ptr316 = ptr;
		ptr += 4;
		ptr += _equipmentPower.Serialize(ptr);
		int num315 = (int)(ptr - ptr316 - 4);
		if (num315 > 4194304)
		{
			throw new Exception($"Size of field {"_equipmentPower"} must be less than {4096}KB");
		}
		*(int*)ptr316 = num315;
		byte* ptr317 = ptr;
		ptr += 4;
		ptr += _healFlawSpeed.Serialize(ptr);
		int num316 = (int)(ptr - ptr317 - 4);
		if (num316 > 4194304)
		{
			throw new Exception($"Size of field {"_healFlawSpeed"} must be less than {4096}KB");
		}
		*(int*)ptr317 = num316;
		byte* ptr318 = ptr;
		ptr += 4;
		ptr += _unlockSpeed.Serialize(ptr);
		int num317 = (int)(ptr - ptr318 - 4);
		if (num317 > 4194304)
		{
			throw new Exception($"Size of field {"_unlockSpeed"} must be less than {4096}KB");
		}
		*(int*)ptr318 = num317;
		byte* ptr319 = ptr;
		ptr += 4;
		ptr += _flawBonusFactor.Serialize(ptr);
		int num318 = (int)(ptr - ptr319 - 4);
		if (num318 > 4194304)
		{
			throw new Exception($"Size of field {"_flawBonusFactor"} must be less than {4096}KB");
		}
		*(int*)ptr319 = num318;
		byte* ptr320 = ptr;
		ptr += 4;
		ptr += _canCostShaTricks.Serialize(ptr);
		int num319 = (int)(ptr - ptr320 - 4);
		if (num319 > 4194304)
		{
			throw new Exception($"Size of field {"_canCostShaTricks"} must be less than {4096}KB");
		}
		*(int*)ptr320 = num319;
		byte* ptr321 = ptr;
		ptr += 4;
		ptr += _defenderDirectFinalDamageValue.Serialize(ptr);
		int num320 = (int)(ptr - ptr321 - 4);
		if (num320 > 4194304)
		{
			throw new Exception($"Size of field {"_defenderDirectFinalDamageValue"} must be less than {4096}KB");
		}
		*(int*)ptr321 = num320;
		byte* ptr322 = ptr;
		ptr += 4;
		ptr += _normalAttackRecoveryFrame.Serialize(ptr);
		int num321 = (int)(ptr - ptr322 - 4);
		if (num321 > 4194304)
		{
			throw new Exception($"Size of field {"_normalAttackRecoveryFrame"} must be less than {4096}KB");
		}
		*(int*)ptr322 = num321;
		byte* ptr323 = ptr;
		ptr += 4;
		ptr += _finalGoneMadInjury.Serialize(ptr);
		int num322 = (int)(ptr - ptr323 - 4);
		if (num322 > 4194304)
		{
			throw new Exception($"Size of field {"_finalGoneMadInjury"} must be less than {4096}KB");
		}
		*(int*)ptr323 = num322;
		byte* ptr324 = ptr;
		ptr += 4;
		ptr += _attackerDirectFinalDamageValue.Serialize(ptr);
		int num323 = (int)(ptr - ptr324 - 4);
		if (num323 > 4194304)
		{
			throw new Exception($"Size of field {"_attackerDirectFinalDamageValue"} must be less than {4096}KB");
		}
		*(int*)ptr324 = num323;
		byte* ptr325 = ptr;
		ptr += 4;
		ptr += _canCostTrickDuringPreparingSkill.Serialize(ptr);
		int num324 = (int)(ptr - ptr325 - 4);
		if (num324 > 4194304)
		{
			throw new Exception($"Size of field {"_canCostTrickDuringPreparingSkill"} must be less than {4096}KB");
		}
		*(int*)ptr325 = num324;
		byte* ptr326 = ptr;
		ptr += 4;
		ptr += _validItemList.Serialize(ptr);
		int num325 = (int)(ptr - ptr326 - 4);
		if (num325 > 4194304)
		{
			throw new Exception($"Size of field {"_validItemList"} must be less than {4096}KB");
		}
		*(int*)ptr326 = num325;
		byte* ptr327 = ptr;
		ptr += 4;
		ptr += _acceptDamageCanAdd.Serialize(ptr);
		int num326 = (int)(ptr - ptr327 - 4);
		if (num326 > 4194304)
		{
			throw new Exception($"Size of field {"_acceptDamageCanAdd"} must be less than {4096}KB");
		}
		*(int*)ptr327 = num326;
		byte* ptr328 = ptr;
		ptr += 4;
		ptr += _makeDamageCanReduce.Serialize(ptr);
		int num327 = (int)(ptr - ptr328 - 4);
		if (num327 > 4194304)
		{
			throw new Exception($"Size of field {"_makeDamageCanReduce"} must be less than {4096}KB");
		}
		*(int*)ptr328 = num327;
		byte* ptr329 = ptr;
		ptr += 4;
		ptr += _normalAttackGetTrickCount.Serialize(ptr);
		int num328 = (int)(ptr - ptr329 - 4);
		if (num328 > 4194304)
		{
			throw new Exception($"Size of field {"_normalAttackGetTrickCount"} must be less than {4096}KB");
		}
		*(int*)ptr329 = num328;
		return (int)(ptr - pData);
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		_id = *(int*)ptr;
		ptr += 4;
		ptr += 4;
		ptr += _maxStrength.Deserialize(ptr);
		ptr += 4;
		ptr += _maxDexterity.Deserialize(ptr);
		ptr += 4;
		ptr += _maxConcentration.Deserialize(ptr);
		ptr += 4;
		ptr += _maxVitality.Deserialize(ptr);
		ptr += 4;
		ptr += _maxEnergy.Deserialize(ptr);
		ptr += 4;
		ptr += _maxIntelligence.Deserialize(ptr);
		ptr += 4;
		ptr += _recoveryOfStance.Deserialize(ptr);
		ptr += 4;
		ptr += _recoveryOfBreath.Deserialize(ptr);
		ptr += 4;
		ptr += _moveSpeed.Deserialize(ptr);
		ptr += 4;
		ptr += _recoveryOfFlaw.Deserialize(ptr);
		ptr += 4;
		ptr += _castSpeed.Deserialize(ptr);
		ptr += 4;
		ptr += _recoveryOfBlockedAcupoint.Deserialize(ptr);
		ptr += 4;
		ptr += _weaponSwitchSpeed.Deserialize(ptr);
		ptr += 4;
		ptr += _attackSpeed.Deserialize(ptr);
		ptr += 4;
		ptr += _innerRatio.Deserialize(ptr);
		ptr += 4;
		ptr += _recoveryOfQiDisorder.Deserialize(ptr);
		ptr += 4;
		ptr += _minorAttributeFixMaxValue.Deserialize(ptr);
		ptr += 4;
		ptr += _minorAttributeFixMinValue.Deserialize(ptr);
		ptr += 4;
		ptr += _resistOfHotPoison.Deserialize(ptr);
		ptr += 4;
		ptr += _resistOfGloomyPoison.Deserialize(ptr);
		ptr += 4;
		ptr += _resistOfColdPoison.Deserialize(ptr);
		ptr += 4;
		ptr += _resistOfRedPoison.Deserialize(ptr);
		ptr += 4;
		ptr += _resistOfRottenPoison.Deserialize(ptr);
		ptr += 4;
		ptr += _resistOfIllusoryPoison.Deserialize(ptr);
		ptr += 4;
		ptr += _displayAge.Deserialize(ptr);
		ptr += 4;
		ptr += _neiliProportionOfFiveElements.Deserialize(ptr);
		ptr += 4;
		ptr += _weaponMaxPower.Deserialize(ptr);
		ptr += 4;
		ptr += _weaponUseRequirement.Deserialize(ptr);
		ptr += 4;
		ptr += _weaponAttackRange.Deserialize(ptr);
		ptr += 4;
		ptr += _armorMaxPower.Deserialize(ptr);
		ptr += 4;
		ptr += _armorUseRequirement.Deserialize(ptr);
		ptr += 4;
		ptr += _hitStrength.Deserialize(ptr);
		ptr += 4;
		ptr += _hitTechnique.Deserialize(ptr);
		ptr += 4;
		ptr += _hitSpeed.Deserialize(ptr);
		ptr += 4;
		ptr += _hitMind.Deserialize(ptr);
		ptr += 4;
		ptr += _hitCanChange.Deserialize(ptr);
		ptr += 4;
		ptr += _hitChangeEffectPercent.Deserialize(ptr);
		ptr += 4;
		ptr += _avoidStrength.Deserialize(ptr);
		ptr += 4;
		ptr += _avoidTechnique.Deserialize(ptr);
		ptr += 4;
		ptr += _avoidSpeed.Deserialize(ptr);
		ptr += 4;
		ptr += _avoidMind.Deserialize(ptr);
		ptr += 4;
		ptr += _avoidCanChange.Deserialize(ptr);
		ptr += 4;
		ptr += _avoidChangeEffectPercent.Deserialize(ptr);
		ptr += 4;
		ptr += _penetrateOuter.Deserialize(ptr);
		ptr += 4;
		ptr += _penetrateInner.Deserialize(ptr);
		ptr += 4;
		ptr += _penetrateResistOuter.Deserialize(ptr);
		ptr += 4;
		ptr += _penetrateResistInner.Deserialize(ptr);
		ptr += 4;
		ptr += _neiliAllocationAttack.Deserialize(ptr);
		ptr += 4;
		ptr += _neiliAllocationAgile.Deserialize(ptr);
		ptr += 4;
		ptr += _neiliAllocationDefense.Deserialize(ptr);
		ptr += 4;
		ptr += _neiliAllocationAssist.Deserialize(ptr);
		ptr += 4;
		ptr += _happiness.Deserialize(ptr);
		ptr += 4;
		ptr += _maxHealth.Deserialize(ptr);
		ptr += 4;
		ptr += _healthCost.Deserialize(ptr);
		ptr += 4;
		ptr += _moveSpeedCanChange.Deserialize(ptr);
		ptr += 4;
		ptr += _attackerHitStrength.Deserialize(ptr);
		ptr += 4;
		ptr += _attackerHitTechnique.Deserialize(ptr);
		ptr += 4;
		ptr += _attackerHitSpeed.Deserialize(ptr);
		ptr += 4;
		ptr += _attackerHitMind.Deserialize(ptr);
		ptr += 4;
		ptr += _attackerAvoidStrength.Deserialize(ptr);
		ptr += 4;
		ptr += _attackerAvoidTechnique.Deserialize(ptr);
		ptr += 4;
		ptr += _attackerAvoidSpeed.Deserialize(ptr);
		ptr += 4;
		ptr += _attackerAvoidMind.Deserialize(ptr);
		ptr += 4;
		ptr += _attackerPenetrateOuter.Deserialize(ptr);
		ptr += 4;
		ptr += _attackerPenetrateInner.Deserialize(ptr);
		ptr += 4;
		ptr += _attackerPenetrateResistOuter.Deserialize(ptr);
		ptr += 4;
		ptr += _attackerPenetrateResistInner.Deserialize(ptr);
		ptr += 4;
		ptr += _attackHitType.Deserialize(ptr);
		ptr += 4;
		ptr += _makeDirectDamage.Deserialize(ptr);
		ptr += 4;
		ptr += _makeBounceDamage.Deserialize(ptr);
		ptr += 4;
		ptr += _makeFightBackDamage.Deserialize(ptr);
		ptr += 4;
		ptr += _makePoisonLevel.Deserialize(ptr);
		ptr += 4;
		ptr += _makePoisonValue.Deserialize(ptr);
		ptr += 4;
		ptr += _attackerHitOdds.Deserialize(ptr);
		ptr += 4;
		ptr += _attackerFightBackHitOdds.Deserialize(ptr);
		ptr += 4;
		ptr += _attackerPursueOdds.Deserialize(ptr);
		ptr += 4;
		ptr += _makedInjuryChangeToOld.Deserialize(ptr);
		ptr += 4;
		ptr += _makedPoisonChangeToOld.Deserialize(ptr);
		ptr += 4;
		ptr += _makeDamageType.Deserialize(ptr);
		ptr += 4;
		ptr += _canMakeInjuryToNoInjuryPart.Deserialize(ptr);
		ptr += 4;
		ptr += _makePoisonType.Deserialize(ptr);
		ptr += 4;
		ptr += _normalAttackWeapon.Deserialize(ptr);
		ptr += 4;
		ptr += _normalAttackTrick.Deserialize(ptr);
		ptr += 4;
		ptr += _extraFlawCount.Deserialize(ptr);
		ptr += 4;
		ptr += _attackCanBounce.Deserialize(ptr);
		ptr += 4;
		ptr += _attackCanFightBack.Deserialize(ptr);
		ptr += 4;
		ptr += _makeFightBackInjuryMark.Deserialize(ptr);
		ptr += 4;
		ptr += _legSkillUseShoes.Deserialize(ptr);
		ptr += 4;
		ptr += _attackerFinalDamageValue.Deserialize(ptr);
		ptr += 4;
		ptr += _defenderHitStrength.Deserialize(ptr);
		ptr += 4;
		ptr += _defenderHitTechnique.Deserialize(ptr);
		ptr += 4;
		ptr += _defenderHitSpeed.Deserialize(ptr);
		ptr += 4;
		ptr += _defenderHitMind.Deserialize(ptr);
		ptr += 4;
		ptr += _defenderAvoidStrength.Deserialize(ptr);
		ptr += 4;
		ptr += _defenderAvoidTechnique.Deserialize(ptr);
		ptr += 4;
		ptr += _defenderAvoidSpeed.Deserialize(ptr);
		ptr += 4;
		ptr += _defenderAvoidMind.Deserialize(ptr);
		ptr += 4;
		ptr += _defenderPenetrateOuter.Deserialize(ptr);
		ptr += 4;
		ptr += _defenderPenetrateInner.Deserialize(ptr);
		ptr += 4;
		ptr += _defenderPenetrateResistOuter.Deserialize(ptr);
		ptr += 4;
		ptr += _defenderPenetrateResistInner.Deserialize(ptr);
		ptr += 4;
		ptr += _acceptDirectDamage.Deserialize(ptr);
		ptr += 4;
		ptr += _acceptBounceDamage.Deserialize(ptr);
		ptr += 4;
		ptr += _acceptFightBackDamage.Deserialize(ptr);
		ptr += 4;
		ptr += _acceptPoisonLevel.Deserialize(ptr);
		ptr += 4;
		ptr += _acceptPoisonValue.Deserialize(ptr);
		ptr += 4;
		ptr += _defenderHitOdds.Deserialize(ptr);
		ptr += 4;
		ptr += _defenderFightBackHitOdds.Deserialize(ptr);
		ptr += 4;
		ptr += _defenderPursueOdds.Deserialize(ptr);
		ptr += 4;
		ptr += _acceptMaxInjuryCount.Deserialize(ptr);
		ptr += 4;
		ptr += _bouncePower.Deserialize(ptr);
		ptr += 4;
		ptr += _fightBackPower.Deserialize(ptr);
		ptr += 4;
		ptr += _directDamageInnerRatio.Deserialize(ptr);
		ptr += 4;
		ptr += _defenderFinalDamageValue.Deserialize(ptr);
		ptr += 4;
		ptr += _directDamageValue.Deserialize(ptr);
		ptr += 4;
		ptr += _directInjuryMark.Deserialize(ptr);
		ptr += 4;
		ptr += _goneMadInjury.Deserialize(ptr);
		ptr += 4;
		ptr += _healInjurySpeed.Deserialize(ptr);
		ptr += 4;
		ptr += _healInjuryBuff.Deserialize(ptr);
		ptr += 4;
		ptr += _healInjuryDebuff.Deserialize(ptr);
		ptr += 4;
		ptr += _healPoisonSpeed.Deserialize(ptr);
		ptr += 4;
		ptr += _healPoisonBuff.Deserialize(ptr);
		ptr += 4;
		ptr += _healPoisonDebuff.Deserialize(ptr);
		ptr += 4;
		ptr += _fleeSpeed.Deserialize(ptr);
		ptr += 4;
		ptr += _maxFlawCount.Deserialize(ptr);
		ptr += 4;
		ptr += _canAddFlaw.Deserialize(ptr);
		ptr += 4;
		ptr += _flawLevel.Deserialize(ptr);
		ptr += 4;
		ptr += _flawLevelCanReduce.Deserialize(ptr);
		ptr += 4;
		ptr += _flawCount.Deserialize(ptr);
		ptr += 4;
		ptr += _maxAcupointCount.Deserialize(ptr);
		ptr += 4;
		ptr += _canAddAcupoint.Deserialize(ptr);
		ptr += 4;
		ptr += _acupointLevel.Deserialize(ptr);
		ptr += 4;
		ptr += _acupointLevelCanReduce.Deserialize(ptr);
		ptr += 4;
		ptr += _acupointCount.Deserialize(ptr);
		ptr += 4;
		ptr += _addNeiliAllocation.Deserialize(ptr);
		ptr += 4;
		ptr += _costNeiliAllocation.Deserialize(ptr);
		ptr += 4;
		ptr += _canChangeNeiliAllocation.Deserialize(ptr);
		ptr += 4;
		ptr += _canGetTrick.Deserialize(ptr);
		ptr += 4;
		ptr += _getTrickType.Deserialize(ptr);
		ptr += 4;
		ptr += _attackBodyPart.Deserialize(ptr);
		ptr += 4;
		ptr += _weaponEquipAttack.Deserialize(ptr);
		ptr += 4;
		ptr += _weaponEquipDefense.Deserialize(ptr);
		ptr += 4;
		ptr += _armorEquipAttack.Deserialize(ptr);
		ptr += 4;
		ptr += _armorEquipDefense.Deserialize(ptr);
		ptr += 4;
		ptr += _attackRangeForward.Deserialize(ptr);
		ptr += 4;
		ptr += _attackRangeBackward.Deserialize(ptr);
		ptr += 4;
		ptr += _moveCanBeStopped.Deserialize(ptr);
		ptr += 4;
		ptr += _canForcedMove.Deserialize(ptr);
		ptr += 4;
		ptr += _mobilityCanBeRemoved.Deserialize(ptr);
		ptr += 4;
		ptr += _mobilityCostByEffect.Deserialize(ptr);
		ptr += 4;
		ptr += _moveDistance.Deserialize(ptr);
		ptr += 4;
		ptr += _jumpPrepareFrame.Deserialize(ptr);
		ptr += 4;
		ptr += _bounceInjuryMark.Deserialize(ptr);
		ptr += 4;
		ptr += _skillHasCost.Deserialize(ptr);
		ptr += 4;
		ptr += _combatStateEffect.Deserialize(ptr);
		ptr += 4;
		ptr += _changeNeedUseSkill.Deserialize(ptr);
		ptr += 4;
		ptr += _changeDistanceIsMove.Deserialize(ptr);
		ptr += 4;
		ptr += _replaceCharHit.Deserialize(ptr);
		ptr += 4;
		ptr += _canAddPoison.Deserialize(ptr);
		ptr += 4;
		ptr += _canReducePoison.Deserialize(ptr);
		ptr += 4;
		ptr += _reducePoisonValue.Deserialize(ptr);
		ptr += 4;
		ptr += _poisonCanAffect.Deserialize(ptr);
		ptr += 4;
		ptr += _poisonAffectCount.Deserialize(ptr);
		ptr += 4;
		ptr += _costTricks.Deserialize(ptr);
		ptr += 4;
		ptr += _jumpMoveDistance.Deserialize(ptr);
		ptr += 4;
		ptr += _combatStateToAdd.Deserialize(ptr);
		ptr += 4;
		ptr += _combatStatePower.Deserialize(ptr);
		ptr += 4;
		ptr += _breakBodyPartInjuryCount.Deserialize(ptr);
		ptr += 4;
		ptr += _bodyPartIsBroken.Deserialize(ptr);
		ptr += 4;
		ptr += _maxTrickCount.Deserialize(ptr);
		ptr += 4;
		ptr += _maxBreathPercent.Deserialize(ptr);
		ptr += 4;
		ptr += _maxStancePercent.Deserialize(ptr);
		ptr += 4;
		ptr += _extraBreathPercent.Deserialize(ptr);
		ptr += 4;
		ptr += _extraStancePercent.Deserialize(ptr);
		ptr += 4;
		ptr += _moveCostMobility.Deserialize(ptr);
		ptr += 4;
		ptr += _defendSkillKeepTime.Deserialize(ptr);
		ptr += 4;
		ptr += _bounceRange.Deserialize(ptr);
		ptr += 4;
		ptr += _mindMarkKeepTime.Deserialize(ptr);
		ptr += 4;
		ptr += _skillMobilityCostPerFrame.Deserialize(ptr);
		ptr += 4;
		ptr += _canAddWug.Deserialize(ptr);
		ptr += 4;
		ptr += _hasGodWeaponBuff.Deserialize(ptr);
		ptr += 4;
		ptr += _hasGodArmorBuff.Deserialize(ptr);
		ptr += 4;
		ptr += _teammateCmdRequireGenerateValue.Deserialize(ptr);
		ptr += 4;
		ptr += _teammateCmdEffect.Deserialize(ptr);
		ptr += 4;
		ptr += _flawRecoverSpeed.Deserialize(ptr);
		ptr += 4;
		ptr += _acupointRecoverSpeed.Deserialize(ptr);
		ptr += 4;
		ptr += _mindMarkRecoverSpeed.Deserialize(ptr);
		ptr += 4;
		ptr += _injuryAutoHealSpeed.Deserialize(ptr);
		ptr += 4;
		ptr += _canRecoverBreath.Deserialize(ptr);
		ptr += 4;
		ptr += _canRecoverStance.Deserialize(ptr);
		ptr += 4;
		ptr += _fatalDamageValue.Deserialize(ptr);
		ptr += 4;
		ptr += _fatalDamageMarkCount.Deserialize(ptr);
		ptr += 4;
		ptr += _canFightBackDuringPrepareSkill.Deserialize(ptr);
		ptr += 4;
		ptr += _skillPrepareSpeed.Deserialize(ptr);
		ptr += 4;
		ptr += _breathRecoverSpeed.Deserialize(ptr);
		ptr += 4;
		ptr += _stanceRecoverSpeed.Deserialize(ptr);
		ptr += 4;
		ptr += _mobilityRecoverSpeed.Deserialize(ptr);
		ptr += 4;
		ptr += _changeTrickProgressAddValue.Deserialize(ptr);
		ptr += 4;
		ptr += _power.Deserialize(ptr);
		ptr += 4;
		ptr += _maxPower.Deserialize(ptr);
		ptr += 4;
		ptr += _powerCanReduce.Deserialize(ptr);
		ptr += 4;
		ptr += _useRequirement.Deserialize(ptr);
		ptr += 4;
		ptr += _currInnerRatio.Deserialize(ptr);
		ptr += 4;
		ptr += _costBreathAndStance.Deserialize(ptr);
		ptr += 4;
		ptr += _costBreath.Deserialize(ptr);
		ptr += 4;
		ptr += _costStance.Deserialize(ptr);
		ptr += 4;
		ptr += _costMobility.Deserialize(ptr);
		ptr += 4;
		ptr += _skillCostTricks.Deserialize(ptr);
		ptr += 4;
		ptr += _effectDirection.Deserialize(ptr);
		ptr += 4;
		ptr += _effectDirectionCanChange.Deserialize(ptr);
		ptr += 4;
		ptr += _gridCost.Deserialize(ptr);
		ptr += 4;
		ptr += _prepareTotalProgress.Deserialize(ptr);
		ptr += 4;
		ptr += _specificGridCount.Deserialize(ptr);
		ptr += 4;
		ptr += _genericGridCount.Deserialize(ptr);
		ptr += 4;
		ptr += _canInterrupt.Deserialize(ptr);
		ptr += 4;
		ptr += _interruptOdds.Deserialize(ptr);
		ptr += 4;
		ptr += _canSilence.Deserialize(ptr);
		ptr += 4;
		ptr += _silenceOdds.Deserialize(ptr);
		ptr += 4;
		ptr += _canCastWithBrokenBodyPart.Deserialize(ptr);
		ptr += 4;
		ptr += _addPowerCanBeRemoved.Deserialize(ptr);
		ptr += 4;
		ptr += _skillType.Deserialize(ptr);
		ptr += 4;
		ptr += _effectCountCanChange.Deserialize(ptr);
		ptr += 4;
		ptr += _canCastInDefend.Deserialize(ptr);
		ptr += 4;
		ptr += _hitDistribution.Deserialize(ptr);
		ptr += 4;
		ptr += _canCastOnLackBreath.Deserialize(ptr);
		ptr += 4;
		ptr += _canCastOnLackStance.Deserialize(ptr);
		ptr += 4;
		ptr += _costBreathOnCast.Deserialize(ptr);
		ptr += 4;
		ptr += _costStanceOnCast.Deserialize(ptr);
		ptr += 4;
		ptr += _canUseMobilityAsBreath.Deserialize(ptr);
		ptr += 4;
		ptr += _canUseMobilityAsStance.Deserialize(ptr);
		ptr += 4;
		ptr += _castCostNeiliAllocation.Deserialize(ptr);
		ptr += 4;
		ptr += _acceptPoisonResist.Deserialize(ptr);
		ptr += 4;
		ptr += _makePoisonResist.Deserialize(ptr);
		ptr += 4;
		ptr += _canCriticalHit.Deserialize(ptr);
		ptr += 4;
		ptr += _canCostNeiliAllocationEffect.Deserialize(ptr);
		ptr += 4;
		ptr += _consummateLevelRelatedMainAttributesHitValues.Deserialize(ptr);
		ptr += 4;
		ptr += _consummateLevelRelatedMainAttributesAvoidValues.Deserialize(ptr);
		ptr += 4;
		ptr += _consummateLevelRelatedMainAttributesPenetrations.Deserialize(ptr);
		ptr += 4;
		ptr += _consummateLevelRelatedMainAttributesPenetrationResists.Deserialize(ptr);
		ptr += 4;
		ptr += _skillAlsoAsFiveElements.Deserialize(ptr);
		ptr += 4;
		ptr += _innerInjuryImmunity.Deserialize(ptr);
		ptr += 4;
		ptr += _outerInjuryImmunity.Deserialize(ptr);
		ptr += 4;
		ptr += _poisonAffectThreshold.Deserialize(ptr);
		ptr += 4;
		ptr += _lockDistance.Deserialize(ptr);
		ptr += 4;
		ptr += _resistOfAllPoison.Deserialize(ptr);
		ptr += 4;
		ptr += _makePoisonTarget.Deserialize(ptr);
		ptr += 4;
		ptr += _acceptPoisonTarget.Deserialize(ptr);
		ptr += 4;
		ptr += _certainCriticalHit.Deserialize(ptr);
		ptr += 4;
		ptr += _mindMarkCount.Deserialize(ptr);
		ptr += 4;
		ptr += _canFightBackWithHit.Deserialize(ptr);
		ptr += 4;
		ptr += _inevitableHit.Deserialize(ptr);
		ptr += 4;
		ptr += _attackCanPursue.Deserialize(ptr);
		ptr += 4;
		ptr += _combatSkillDataEffectList.Deserialize(ptr);
		ptr += 4;
		ptr += _criticalOdds.Deserialize(ptr);
		ptr += 4;
		ptr += _stanceCostByEffect.Deserialize(ptr);
		ptr += 4;
		ptr += _breathCostByEffect.Deserialize(ptr);
		ptr += 4;
		ptr += _powerAddRatio.Deserialize(ptr);
		ptr += 4;
		ptr += _powerReduceRatio.Deserialize(ptr);
		ptr += 4;
		ptr += _poisonAffectProduceValue.Deserialize(ptr);
		ptr += 4;
		ptr += _canReadingOnMonthChange.Deserialize(ptr);
		ptr += 4;
		ptr += _medicineEffect.Deserialize(ptr);
		ptr += 4;
		ptr += _xiangshuInfectionDelta.Deserialize(ptr);
		ptr += 4;
		ptr += _healthDelta.Deserialize(ptr);
		ptr += 4;
		ptr += _weaponSilenceFrame.Deserialize(ptr);
		ptr += 4;
		ptr += _silenceFrame.Deserialize(ptr);
		ptr += 4;
		ptr += _currAgeDelta.Deserialize(ptr);
		ptr += 4;
		ptr += _goneMadInAllBreak.Deserialize(ptr);
		ptr += 4;
		ptr += _makeLoveRateOnMonthChange.Deserialize(ptr);
		ptr += 4;
		ptr += _canAutoHealOnMonthChange.Deserialize(ptr);
		ptr += 4;
		ptr += _happinessDelta.Deserialize(ptr);
		ptr += 4;
		ptr += _teammateCmdCanUse.Deserialize(ptr);
		ptr += 4;
		ptr += _mixPoisonInfinityAffect.Deserialize(ptr);
		ptr += 4;
		ptr += _attackRangeMaxAcupoint.Deserialize(ptr);
		ptr += 4;
		ptr += _maxMobilityPercent.Deserialize(ptr);
		ptr += 4;
		ptr += _makeMindDamage.Deserialize(ptr);
		ptr += 4;
		ptr += _acceptMindDamage.Deserialize(ptr);
		ptr += 4;
		ptr += _hitAddByTempValue.Deserialize(ptr);
		ptr += 4;
		ptr += _avoidAddByTempValue.Deserialize(ptr);
		ptr += 4;
		ptr += _ignoreEquipmentOverload.Deserialize(ptr);
		ptr += 4;
		ptr += _canCostEnemyUsableTricks.Deserialize(ptr);
		ptr += 4;
		ptr += _ignoreArmor.Deserialize(ptr);
		ptr += 4;
		ptr += _unyieldingFallen.Deserialize(ptr);
		ptr += 4;
		ptr += _normalAttackPrepareFrame.Deserialize(ptr);
		ptr += 4;
		ptr += _canCostUselessTricks.Deserialize(ptr);
		ptr += 4;
		ptr += _defendSkillCanAffect.Deserialize(ptr);
		ptr += 4;
		ptr += _assistSkillCanAffect.Deserialize(ptr);
		ptr += 4;
		ptr += _agileSkillCanAffect.Deserialize(ptr);
		ptr += 4;
		ptr += _allMarkChangeToMind.Deserialize(ptr);
		ptr += 4;
		ptr += _mindMarkChangeToFatal.Deserialize(ptr);
		ptr += 4;
		ptr += _canCast.Deserialize(ptr);
		ptr += 4;
		ptr += _inevitableAvoid.Deserialize(ptr);
		ptr += 4;
		ptr += _powerEffectReverse.Deserialize(ptr);
		ptr += 4;
		ptr += _featureBonusReverse.Deserialize(ptr);
		ptr += 4;
		ptr += _wugFatalDamageValue.Deserialize(ptr);
		ptr += 4;
		ptr += _canRecoverHealthOnMonthChange.Deserialize(ptr);
		ptr += 4;
		ptr += _takeRevengeRateOnMonthChange.Deserialize(ptr);
		ptr += 4;
		ptr += _consummateLevelBonus.Deserialize(ptr);
		ptr += 4;
		ptr += _neiliDelta.Deserialize(ptr);
		ptr += 4;
		ptr += _canMakeLoveSpecialOnMonthChange.Deserialize(ptr);
		ptr += 4;
		ptr += _healAcupointSpeed.Deserialize(ptr);
		ptr += 4;
		ptr += _maxChangeTrickCount.Deserialize(ptr);
		ptr += 4;
		ptr += _convertCostBreathAndStance.Deserialize(ptr);
		ptr += 4;
		ptr += _personalitiesAll.Deserialize(ptr);
		ptr += 4;
		ptr += _finalFatalDamageMarkCount.Deserialize(ptr);
		ptr += 4;
		ptr += _infinityMindMarkProgress.Deserialize(ptr);
		ptr += 4;
		ptr += _combatSkillAiScorePower.Deserialize(ptr);
		ptr += 4;
		ptr += _normalAttackChangeToUnlockAttack.Deserialize(ptr);
		ptr += 4;
		ptr += _attackBodyPartOdds.Deserialize(ptr);
		ptr += 4;
		ptr += _changeDurability.Deserialize(ptr);
		ptr += 4;
		ptr += _equipmentBonus.Deserialize(ptr);
		ptr += 4;
		ptr += _equipmentWeight.Deserialize(ptr);
		ptr += 4;
		ptr += _rawCreateEffectList.Deserialize(ptr);
		ptr += 4;
		ptr += _jiTrickAsWeaponTrickCount.Deserialize(ptr);
		ptr += 4;
		ptr += _uselessTrickAsJiTrickCount.Deserialize(ptr);
		ptr += 4;
		ptr += _equipmentPower.Deserialize(ptr);
		ptr += 4;
		ptr += _healFlawSpeed.Deserialize(ptr);
		ptr += 4;
		ptr += _unlockSpeed.Deserialize(ptr);
		ptr += 4;
		ptr += _flawBonusFactor.Deserialize(ptr);
		ptr += 4;
		ptr += _canCostShaTricks.Deserialize(ptr);
		ptr += 4;
		ptr += _defenderDirectFinalDamageValue.Deserialize(ptr);
		ptr += 4;
		ptr += _normalAttackRecoveryFrame.Deserialize(ptr);
		ptr += 4;
		ptr += _finalGoneMadInjury.Deserialize(ptr);
		ptr += 4;
		ptr += _attackerDirectFinalDamageValue.Deserialize(ptr);
		ptr += 4;
		ptr += _canCostTrickDuringPreparingSkill.Deserialize(ptr);
		ptr += 4;
		ptr += _validItemList.Deserialize(ptr);
		ptr += 4;
		ptr += _acceptDamageCanAdd.Deserialize(ptr);
		ptr += 4;
		ptr += _makeDamageCanReduce.Deserialize(ptr);
		ptr += 4;
		ptr += _normalAttackGetTrickCount.Deserialize(ptr);
		return (int)(ptr - pData);
	}
}
