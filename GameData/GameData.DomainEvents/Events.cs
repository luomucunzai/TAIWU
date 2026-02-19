using System;
using System.Collections.Generic;
using Config;
using GameData.Common;
using GameData.DLC;
using GameData.Domains;
using GameData.Domains.Adventure;
using GameData.Domains.Character;
using GameData.Domains.Character.AvatarSystem;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.Item;
using GameData.Domains.LifeRecord;
using GameData.Domains.Map;
using GameData.Domains.Organization;
using GameData.Domains.Taiwu.Profession;
using GameData.Domains.Taiwu.VillagerRole;
using GameData.Domains.TaiwuEvent;
using GameData.Domains.TaiwuEvent.EventHelper;
using GameData.Domains.TaiwuEvent.MonthlyEventActions;
using GameData.Domains.World;
using GameData.Domains.World.MonthlyEvent;
using GameData.Domains.World.Notification;
using GameData.GameDataBridge;
using GameData.Utilities;

namespace GameData.DomainEvents;

public static class Events
{
	public delegate void OnCharacterLocationChanged(DataContext context, int charId, Location srcLocation, Location destLocation);

	public delegate void OnMakeLove(DataContext context, GameData.Domains.Character.Character character, GameData.Domains.Character.Character target, sbyte makeLoveState);

	public delegate void OnEatingItem(DataContext context, GameData.Domains.Character.Character character, ItemKey itemKey);

	public delegate void OnXiangshuInfectionFeatureChangedEnd(DataContext context, GameData.Domains.Character.Character character, short featureId);

	public delegate void OnCombatBegin(DataContext context);

	public delegate void OnCombatSettlement(DataContext context, sbyte combatStatus);

	public delegate void OnCombatEnd(DataContext context);

	public delegate void OnChangeNeiliAllocationAfterCombatBegin(DataContext context, CombatCharacter character, NeiliAllocation allocationAfterBegin);

	public delegate void OnCreateGangqiAfterChangeNeiliAllocation(DataContext context, CombatCharacter character);

	public delegate void OnChangeBossPhase(DataContext context);

	[DomainEvent(MaxReenterCount = 1)]
	public delegate void OnGetTrick(DataContext context, int charId, bool isAlly, sbyte trickType, bool usable);

	public delegate void OnRearrangeTrick(DataContext context, int charId, bool isAlly);

	[DomainEvent(MaxReenterCount = 2)]
	public delegate void OnGetShaTrick(DataContext context, int charId, bool isAlly, bool real);

	public delegate void OnRemoveShaTrick(DataContext context, int charId);

	public delegate void OnOverflowTrickRemoved(DataContext context, int charId, bool isAlly, int removedCount);

	public delegate void OnCostBreathAndStance(DataContext context, int charId, bool isAlly, int costBreath, int costStance, short skillId);

	public delegate void OnChangeWeapon(DataContext context, int charId, bool isAlly, CombatWeaponData newWeapon, CombatWeaponData oldWeapon);

	public delegate void OnWeaponCdEnd(DataContext context, int charId, bool isAlly, CombatWeaponData weapon);

	public delegate void OnChangeTrickCountChanged(DataContext context, CombatCharacter character, int addValue, bool bySelectChangeTrick);

	public delegate void OnUnlockAttack(DataContext context, CombatCharacter combatChar, int weaponIndex);

	public delegate void OnUnlockAttackEnd(DataContext context, CombatCharacter attacker);

	public delegate void OnNormalAttackPrepareEnd(DataContext context, int charId, bool isAlly);

	public delegate void OnNormalAttackOutOfRange(DataContext context, int charId, bool isAlly);

	public delegate void OnNormalAttackBegin(DataContext context, CombatCharacter attacker, CombatCharacter defender, sbyte trickType, int pursueIndex);

	public delegate void OnNormalAttackCalcHitEnd(DataContext context, CombatCharacter attacker, CombatCharacter defender, int pursueIndex, bool hit, bool isFightBack, bool isMind);

	public delegate void OnNormalAttackCalcCriticalEnd(DataContext context, CombatCharacter attacker, CombatCharacter defender, bool critical);

	public delegate void OnNormalAttackEnd(DataContext context, CombatCharacter attacker, CombatCharacter defender, sbyte trickType, int pursueIndex, bool hit, bool isFightBack);

	public delegate void OnNormalAttackAllEnd(DataContext context, CombatCharacter attacker, CombatCharacter defender);

	public delegate void OnCastSkillUseExtraBreathOrStance(DataContext context, int charId, short skillId, int extraBreath, int extraStance);

	public delegate void OnCastSkillUseMobilityAsBreathOrStance(DataContext context, int charId, short skillId, bool asBreath);

	public delegate void OnCastLegSkillWithAgile(DataContext context, CombatCharacter combatChar, short legSkillId);

	public delegate void OnCastSkillOnLackBreathStance(DataContext context, CombatCharacter combatChar, short skillId, int lackBreath, int lackStance, int costBreath, int costStance);

	public delegate void OnCastSkillTrickCosted(DataContext context, CombatCharacter combatChar, short skillId, List<NeedTrick> costTricks);

	public delegate void OnJiTrickInsteadCostTricks(DataContext context, CombatCharacter character, int count);

	public delegate void OnUselessTrickInsteadJiTricks(DataContext context, CombatCharacter character, int count);

	public delegate void OnShaTrickInsteadCostTricks(DataContext context, CombatCharacter character, short skillId);

	public delegate void OnCastSkillCosted(DataContext context, CombatCharacter combatChar, short skillId);

	public delegate void OnChangePreparingSkillBegin(DataContext context, int charId, short prevSkillId, short currSkillId);

	public delegate void OnCastAgileOrDefenseWithoutPrepareBegin(DataContext context, int charId, short skillId);

	public delegate void OnCastAgileOrDefenseWithoutPrepareEnd(DataContext context, int charId, short skillId);

	public delegate void OnPrepareSkillEffectNotYetCreated(DataContext context, CombatCharacter character, short skillId);

	public delegate void OnPrepareSkillBegin(DataContext context, int charId, bool isAlly, short skillId);

	public delegate void OnPrepareSkillProgressChange(DataContext context, int charId, bool isAlly, short skillId, sbyte preparePercent);

	public delegate void OnPrepareSkillChangeDistance(DataContext context, CombatCharacter attacker, CombatCharacter defender, short skillId);

	public delegate void OnPrepareSkillEnd(DataContext context, int charId, bool isAlly, short skillId);

	public delegate void OnCastAttackSkillBegin(DataContext context, CombatCharacter attacker, CombatCharacter defender, short skillId);

	public delegate void OnAttackSkillAttackBegin(DataContext context, CombatCharacter attacker, CombatCharacter defender, short skillId, int index, bool hit);

	public delegate void OnAttackSkillAttackHit(DataContext context, CombatCharacter attacker, CombatCharacter defender, short skillId, int index, bool critical);

	public delegate void OnAttackSkillAttackEnd(CombatContext context, sbyte hitType, bool hit, int index);

	public delegate void OnAttackSkillAttackEndOfAll(DataContext context, CombatCharacter character, int index);

	[DomainEvent(MaxReenterCount = 1)]
	public delegate void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted);

	public delegate void OnCastSkillAllEnd(DataContext context, int charId, short skillId);

	public delegate void OnCalcLeveragingValue(CombatContext context, sbyte hitType, bool hit, int index);

	public delegate void OnWisdomCosted(DataContext context, bool isAlly, int value);

	public delegate void OnHealedInjury(DataContext context, int doctorId, int patientId, bool isAlly, sbyte healMarkCount);

	public delegate void OnHealedPoison(DataContext context, int doctorId, int patientId, bool isAlly, sbyte healMarkCount);

	public delegate void OnUsedMedicine(DataContext context, int charId, ItemKey itemKey);

	public delegate void OnUsedCustomItem(DataContext context, int charId, ItemKey itemKey);

	public delegate void OnInterruptOtherAction(DataContext context, CombatCharacter combatChar, sbyte otherActionType);

	public delegate void OnFlawAdded(DataContext context, CombatCharacter combatChar, sbyte bodyPart, sbyte level);

	public delegate void OnFlawRemoved(DataContext context, CombatCharacter combatChar, sbyte bodyPart, sbyte level);

	[DomainEvent(MaxReenterCount = 2)]
	public delegate void OnAcuPointAdded(DataContext context, CombatCharacter combatChar, sbyte bodyPart, sbyte level);

	public delegate void OnAcuPointRemoved(DataContext context, CombatCharacter combatChar, sbyte bodyPart, sbyte level);

	public delegate void OnCombatCharChanged(DataContext context, bool isAlly);

	public delegate void OnAddInjury(DataContext context, CombatCharacter character, sbyte bodyPart, bool isInner, sbyte value, bool changeToOld);

	public delegate void OnAddDirectDamageValue(DataContext context, int attackerId, int defenderId, sbyte bodyPart, bool isInner, int damageValue, short combatSkillId);

	public delegate void OnAddDirectInjury(DataContext context, int attackerId, int defenderId, bool isAlly, sbyte bodyPart, sbyte outerMarkCount, sbyte innerMarkCount, short combatSkillId);

	public delegate void OnBounceInjury(DataContext context, int attackerId, int defenderId, bool isAlly, sbyte bodyPart, sbyte outerMarkCount, sbyte innerMarkCount);

	[DomainEvent(MaxReenterCount = 1)]
	public delegate void OnAddMindMark(DataContext context, CombatCharacter character, int count);

	public delegate void OnAddMindDamage(DataContext context, int attackerId, int defenderId, int damageValue, int markCount, short combatSkillId);

	public delegate void OnAddFatalDamageMark(DataContext context, CombatCharacter combatChar, int count);

	public delegate void OnAddDirectFatalDamageMark(DataContext context, int attackerId, int defenderId, bool isAlly, sbyte bodyPart, int outerMarkCount, int innerMarkCount, short combatSkillId);

	public delegate void OnAddDirectFatalDamage(CombatContext context, int outer, int inner);

	public delegate void OnAddDirectPoisonMark(DataContext context, CombatCharacter attacker, CombatCharacter defender, sbyte poisonType, short skillId, int markCount);

	public delegate void OnMoveStateChanged(DataContext context, CombatCharacter character, byte moveState);

	public delegate void OnMoveBegin(DataContext context, CombatCharacter mover, int distance, bool isJump);

	public delegate void OnMoveEnd(DataContext context, CombatCharacter mover, int distance, bool isJump);

	public delegate void OnIgnoredForceChangeDistance(DataContext context, CombatCharacter mover, int distance);

	[DomainEvent(MaxReenterCount = 7)]
	public delegate void OnDistanceChanged(DataContext context, CombatCharacter mover, short distance, bool isMove, bool isForced);

	[DomainEvent(MaxReenterCount = 1)]
	public delegate void OnSkillEffectChange(DataContext context, int charId, SkillEffectKey key, short oldCount, short newCount, bool removed);

	public delegate void OnSkillSilence(DataContext context, CombatSkillKey skillKey);

	public delegate void OnSkillSilenceEnd(DataContext context, CombatSkillKey skillKey);

	[DomainEvent(MaxReenterCount = 1)]
	public delegate void OnNeiliAllocationChanged(DataContext context, int charId, byte type, int changeValue);

	public delegate void OnAddPoison(DataContext context, int attackerId, int defenderId, sbyte poisonType, sbyte level, int addValue, short skillId, bool canBounce);

	public delegate void OnPoisonAffected(DataContext context, int charId, sbyte poisonType);

	[DomainEvent(MaxReenterCount = 1)]
	public delegate void OnAddWug(DataContext context, int charId, short wugTemplateId, short replacedWug);

	public delegate void OnRemoveWug(DataContext context, int charId, short wugTemplateId);

	public delegate void OnCompareDataCalcFinished(CombatContext context, DamageCompareData compareData);

	public delegate void OnCombatStateMachineUpdateEnd(DataContext context, CombatCharacter combatChar);

	public delegate void OnCombatCharFallen(DataContext context, CombatCharacter combatChar);

	public delegate void OnCombatCostNeiliConfirm(DataContext context, int charId, short skillId, short effectId);

	public delegate void OnCostTrickDuringPreparingSkill(DataContext context, int charId);

	public delegate void OnCombatChangeDurability(DataContext context, CombatCharacter character, ItemKey itemKey, int delta);

	public delegate void OnPassingLegacyWhileAdvancingMonth(DataContext context);

	public delegate void OnAdvanceMonthBegin(DataContext context);

	public delegate void OnPostAdvanceMonthBegin(DataContext context);

	public delegate void OnAdvanceMonthFinish(DataContext context);

	public delegate void OnTaiwuMove(DataContext context, MapBlockData fromBlock, MapBlockData toBlock, int actionPointCost);

	private static OnCharacterLocationChanged _handlersCharacterLocationChanged;

	private static OnMakeLove _handlersMakeLove;

	private static OnEatingItem _handlersEatingItem;

	private static OnXiangshuInfectionFeatureChangedEnd _handlersXiangshuInfectionFeatureChangedEnd;

	private static OnCombatBegin _handlersCombatBegin;

	private static OnCombatSettlement _handlersCombatSettlement;

	private static OnCombatEnd _handlersCombatEnd;

	private static OnChangeNeiliAllocationAfterCombatBegin _handlersChangeNeiliAllocationAfterCombatBegin;

	private static OnCreateGangqiAfterChangeNeiliAllocation _handlersCreateGangqiAfterChangeNeiliAllocation;

	private static OnChangeBossPhase _handlersChangeBossPhase;

	private static OnGetTrick _handlersGetTrick;

	private static OnRearrangeTrick _handlersRearrangeTrick;

	private static OnGetShaTrick _handlersGetShaTrick;

	private static OnRemoveShaTrick _handlersRemoveShaTrick;

	private static OnOverflowTrickRemoved _handlersOverflowTrickRemoved;

	private static OnCostBreathAndStance _handlersCostBreathAndStance;

	private static OnChangeWeapon _handlersChangeWeapon;

	private static OnWeaponCdEnd _handlersWeaponCdEnd;

	private static OnChangeTrickCountChanged _handlersChangeTrickCountChanged;

	private static OnUnlockAttack _handlersUnlockAttack;

	private static OnUnlockAttackEnd _handlersUnlockAttackEnd;

	private static OnNormalAttackPrepareEnd _handlersNormalAttackPrepareEnd;

	private static OnNormalAttackOutOfRange _handlersNormalAttackOutOfRange;

	private static OnNormalAttackBegin _handlersNormalAttackBegin;

	private static OnNormalAttackCalcHitEnd _handlersNormalAttackCalcHitEnd;

	private static OnNormalAttackCalcCriticalEnd _handlersNormalAttackCalcCriticalEnd;

	private static OnNormalAttackEnd _handlersNormalAttackEnd;

	private static OnNormalAttackAllEnd _handlersNormalAttackAllEnd;

	private static OnCastSkillUseExtraBreathOrStance _handlersCastSkillUseExtraBreathOrStance;

	private static OnCastSkillUseMobilityAsBreathOrStance _handlersCastSkillUseMobilityAsBreathOrStance;

	private static OnCastLegSkillWithAgile _handlersCastLegSkillWithAgile;

	private static OnCastSkillOnLackBreathStance _handlersCastSkillOnLackBreathStance;

	private static OnCastSkillTrickCosted _handlersCastSkillTrickCosted;

	private static OnJiTrickInsteadCostTricks _handlersJiTrickInsteadCostTricks;

	private static OnUselessTrickInsteadJiTricks _handlersUselessTrickInsteadJiTricks;

	private static OnShaTrickInsteadCostTricks _handlersShaTrickInsteadCostTricks;

	private static OnCastSkillCosted _handlersCastSkillCosted;

	private static OnChangePreparingSkillBegin _handlersChangePreparingSkillBegin;

	private static OnCastAgileOrDefenseWithoutPrepareBegin _handlersCastAgileOrDefenseWithoutPrepareBegin;

	private static OnCastAgileOrDefenseWithoutPrepareEnd _handlersCastAgileOrDefenseWithoutPrepareEnd;

	private static OnPrepareSkillEffectNotYetCreated _handlersPrepareSkillEffectNotYetCreated;

	private static OnPrepareSkillBegin _handlersPrepareSkillBegin;

	private static OnPrepareSkillProgressChange _handlersPrepareSkillProgressChange;

	private static OnPrepareSkillChangeDistance _handlersPrepareSkillChangeDistance;

	private static OnPrepareSkillEnd _handlersPrepareSkillEnd;

	private static OnCastAttackSkillBegin _handlersCastAttackSkillBegin;

	private static OnAttackSkillAttackBegin _handlersAttackSkillAttackBegin;

	private static OnAttackSkillAttackHit _handlersAttackSkillAttackHit;

	private static OnAttackSkillAttackEnd _handlersAttackSkillAttackEnd;

	private static OnAttackSkillAttackEndOfAll _handlersAttackSkillAttackEndOfAll;

	private static OnCastSkillEnd _handlersCastSkillEnd;

	private static OnCastSkillAllEnd _handlersCastSkillAllEnd;

	private static OnCalcLeveragingValue _handlersCalcLeveragingValue;

	private static OnWisdomCosted _handlersWisdomCosted;

	private static OnHealedInjury _handlersHealedInjury;

	private static OnHealedPoison _handlersHealedPoison;

	private static OnUsedMedicine _handlersUsedMedicine;

	private static OnUsedCustomItem _handlersUsedCustomItem;

	private static OnInterruptOtherAction _handlersInterruptOtherAction;

	private static OnFlawAdded _handlersFlawAdded;

	private static OnFlawRemoved _handlersFlawRemoved;

	private static OnAcuPointAdded _handlersAcuPointAdded;

	private static OnAcuPointRemoved _handlersAcuPointRemoved;

	private static OnCombatCharChanged _handlersCombatCharChanged;

	private static OnAddInjury _handlersAddInjury;

	private static OnAddDirectDamageValue _handlersAddDirectDamageValue;

	private static OnAddDirectInjury _handlersAddDirectInjury;

	private static OnBounceInjury _handlersBounceInjury;

	private static OnAddMindMark _handlersAddMindMark;

	private static OnAddMindDamage _handlersAddMindDamage;

	private static OnAddFatalDamageMark _handlersAddFatalDamageMark;

	private static OnAddDirectFatalDamageMark _handlersAddDirectFatalDamageMark;

	private static OnAddDirectFatalDamage _handlersAddDirectFatalDamage;

	private static OnAddDirectPoisonMark _handlersAddDirectPoisonMark;

	private static OnMoveStateChanged _handlersMoveStateChanged;

	private static OnMoveBegin _handlersMoveBegin;

	private static OnMoveEnd _handlersMoveEnd;

	private static OnIgnoredForceChangeDistance _handlersIgnoredForceChangeDistance;

	private static OnDistanceChanged _handlersDistanceChanged;

	private static OnSkillEffectChange _handlersSkillEffectChange;

	private static OnSkillSilence _handlersSkillSilence;

	private static OnSkillSilenceEnd _handlersSkillSilenceEnd;

	private static OnNeiliAllocationChanged _handlersNeiliAllocationChanged;

	private static OnAddPoison _handlersAddPoison;

	private static OnPoisonAffected _handlersPoisonAffected;

	private static OnAddWug _handlersAddWug;

	private static OnRemoveWug _handlersRemoveWug;

	private static OnCompareDataCalcFinished _handlersCompareDataCalcFinished;

	private static OnCombatStateMachineUpdateEnd _handlersCombatStateMachineUpdateEnd;

	private static OnCombatCharFallen _handlersCombatCharFallen;

	private static OnCombatCostNeiliConfirm _handlersCombatCostNeiliConfirm;

	private static OnCostTrickDuringPreparingSkill _handlersCostTrickDuringPreparingSkill;

	private static OnCombatChangeDurability _handlersCombatChangeDurability;

	private static OnPassingLegacyWhileAdvancingMonth _handlersPassingLegacyWhileAdvancingMonth;

	private static OnAdvanceMonthBegin _handlersAdvanceMonthBegin;

	private static OnPostAdvanceMonthBegin _handlersPostAdvanceMonthBegin;

	private static OnAdvanceMonthFinish _handlersAdvanceMonthFinish;

	private static OnTaiwuMove _handlersTaiwuMove;

	public static void RaiseBeforeSendRequestToArchiveModule(DataContext context)
	{
		DomainManager.LifeRecord.CommitCurrLifeRecords();
		DomainManager.World.CommitInstantNotifications(context);
		DomainManager.Extra.CommitTravelingEvents(context);
		DomainManager.Extra.CommitTaiwuVillageStorages(context);
		DomainManager.Taiwu.CommitTaiwuSettlementTreasury(context);
	}

	public static void RaiseBeforeSaveWorld(DataContext context)
	{
		DomainManager.LifeRecord.CommitCurrLifeRecords();
		DomainManager.World.CommitInstantNotifications(context);
		DomainManager.Extra.CommitTravelingEvents(context);
		DomainManager.Extra.CommitTaiwuVillageStorages(context);
		DomainManager.Taiwu.CommitTaiwuSettlementTreasury(context);
	}

	public static void RaiseTaiwuItemModified(DataContext context, ItemKey itemKey)
	{
		if (ItemType.IsEquipmentItemType(itemKey.ItemType))
		{
			EquipmentBase baseEquipment = DomainManager.Item.GetBaseEquipment(itemKey);
			baseEquipment.SetModificationState(baseEquipment.GetModificationState(), context);
			if (itemKey.ItemType == 3)
			{
				DomainManager.Extra.RecordOwnedClothing(context, itemKey.TemplateId);
			}
		}
	}

	public static void RaiseBossInvasionSpeedTypeChanged(DataContext context, byte prevInvasionSpeedType)
	{
		byte bossInvasionSpeedType = DomainManager.World.GetBossInvasionSpeedType();
		if (GlobalConfig.Instance.SwordTombAdventureLastMonthCount[bossInvasionSpeedType] < 0 && GlobalConfig.Instance.SwordTombAdventureLastMonthCount[prevInvasionSpeedType] > 0)
		{
			RemoveAllAttackingXiangshu();
			DomainManager.Adventure.StopAllSwordTombAdventureCountDown(context);
		}
		else if (GlobalConfig.Instance.SwordTombAdventureLastMonthCount[bossInvasionSpeedType] > 0 && GlobalConfig.Instance.SwordTombAdventureLastMonthCount[prevInvasionSpeedType] < 0)
		{
			DomainManager.TaiwuEvent.ActivateNextSwordTomb();
		}
		else if (DomainManager.Taiwu.GetIsTaiwuDieOfCombatWithXiangshu())
		{
			RemoveAllAttackingXiangshu();
		}
		void RemoveAllAttackingXiangshu()
		{
			List<short> attackingSwordTombs = DomainManager.Adventure.GetAttackingSwordTombs();
			sbyte xiangshuLevel = DomainManager.World.GetXiangshuLevel();
			for (int i = 0; i < attackingSwordTombs.Count; i++)
			{
				short adventureId = attackingSwordTombs[i];
				sbyte xiangshuAvatarIdBySwordTomb = XiangshuAvatarIds.GetXiangshuAvatarIdBySwordTomb(adventureId);
				if (xiangshuAvatarIdBySwordTomb >= 0)
				{
					short currentLevelXiangshuTemplateId = XiangshuAvatarIds.GetCurrentLevelXiangshuTemplateId(xiangshuAvatarIdBySwordTomb, xiangshuLevel, isWeakened: true);
					if (DomainManager.Character.TryGetFixedCharacterByTemplateId(currentLevelXiangshuTemplateId, out var character))
					{
						RaiseFixedCharacterLocationChanged(context, character.GetId(), character.GetLocation(), Location.Invalid);
						character.SetLocation(Location.Invalid, context);
						DomainManager.Character.RemoveNonIntelligentCharacter(context, character);
					}
				}
			}
		}
	}

	public static void RaiseCharacterFeatureAdded(DataContext context, GameData.Domains.Character.Character character, short featureId)
	{
		int id = character.GetId();
		DomainManager.SpecialEffect.AddFeatureEffect(context, id, featureId);
		DomainManager.Extra.RegisterCharacterTemporaryFeature(context, id, featureId);
		if (id == DomainManager.Taiwu.GetTaiwuCharId())
		{
			DomainManager.Taiwu.TaiwuAddFeatureTryInterruptHuntTaiwuAction(context, featureId);
		}
	}

	public static void RaiseCharacterFeatureRemoved(DataContext context, GameData.Domains.Character.Character character, short featureId)
	{
		int id = character.GetId();
		DomainManager.SpecialEffect.RemoveFeatureEffect(context, id, featureId);
		if (id == DomainManager.Taiwu.GetTaiwuCharId())
		{
			DomainManager.Taiwu.TaiwuRemoveFeatureTryInterruptHuntTaiwuAction(context, featureId, character.GetFeatureIds());
		}
	}

	public static void RaiseItemRemovedFromInventory(DataContext context, GameData.Domains.Character.Character character, ItemKey itemKey, int amount)
	{
		int id = character.GetId();
		DomainManager.Item.RemoveOwner(itemKey, ItemOwnerType.CharacterInventory, id);
		short itemSubType = ItemTemplateHelper.GetItemSubType(itemKey.ItemType, itemKey.TemplateId);
		if (itemSubType == 1202)
		{
			int num = itemKey.TemplateId - 211;
			DomainManager.LegendaryBook.UnregisterOwner(context, character, (sbyte)num);
		}
		if (id == DomainManager.Taiwu.GetTaiwuCharId())
		{
			if (itemKey.ItemType == 0)
			{
				DomainManager.Extra.ClearLegendaryBookWeaponSlot(context, itemKey);
			}
			if (ItemType.IsEquipmentItemType(itemKey.ItemType))
			{
				EquipmentBase baseEquipment = DomainManager.Item.GetBaseEquipment(itemKey);
				baseEquipment.SetModificationState(baseEquipment.GetModificationState(), context);
			}
		}
		IReadOnlyDictionary<ItemKey, int> taiwuGiftItems = DomainManager.Extra.GetTaiwuGiftItems(id);
		if (taiwuGiftItems.Count > 0 && taiwuGiftItems.TryGetValue(itemKey, out var value))
		{
			int valueOrDefault = character.GetInventory().Items.GetValueOrDefault(itemKey, 0);
			if (valueOrDefault < value)
			{
				DomainManager.Extra.SetTaiwuGiftItemAmount(context, id, itemKey, amount);
			}
		}
	}

	public static void RaiseCharacterCreated(DataContext context, GameData.Domains.Character.Character character)
	{
		context.Equipping.SetInitialCombatSkillBreakouts(context, character);
		context.Equipping.SetInitialCombatSkillAttainmentPanels(context, character);
		DomainManager.Character.OnCharacterCreated(context, character);
		short settlementId = character.GetOrganizationInfo().SettlementId;
		if (settlementId >= 0)
		{
			DomainManager.Organization.GetSettlement(settlementId).AddSettlementFeatures(context, character);
		}
		character.SpecifyCurrNeili(context, int.MaxValue);
		if (character != DomainManager.Taiwu.GetTaiwu())
		{
			context.Equipping.SelectEquipments(context, character, isOutOfTaiwuGroup: true, removeUnequippedEquipment: true);
		}
		character.SetCurrMainAttributes(character.GetMaxMainAttributes(), context);
		if (character.GetActualAge() >= 0)
		{
			character.AdjustLifespan(context);
		}
		AvatarData avatar = character.GetAvatar();
		avatar.InitializeGrowableElementsShowingAbilitiesAndStates(character);
		character.SetAvatar(avatar, context);
		DomainManager.SpecialEffect.OnCharacterCreated(context, character);
	}

	public static void RaiseCharacterReincarnated(DataContext context, GameData.Domains.Character.Character character, int reincarnatedCharId)
	{
		int id = character.GetId();
		InteractOfLove.OnLoverReincarnate(context, character, id);
		short settlementId = character.GetOrganizationInfo().SettlementId;
		DomainManager.Character.TryEnterTaiwuDreamWhenReincarnated(reincarnatedCharId, settlementId);
		if (ProfessionSkillHandle.BuddhistMonkSkill_IsDirectedSamsaraCharacter(reincarnatedCharId))
		{
			int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
			Location location = character.GetLocation();
			MonthlyEventCollection monthlyEventCollection = DomainManager.World.GetMonthlyEventCollection();
			monthlyEventCollection.AddTaiwuComingSuccess(taiwuCharId, reincarnatedCharId, location, id);
			DeadCharacter deadCharacter = DomainManager.Character.GetDeadCharacter(reincarnatedCharId);
			AvatarData avatarData = new AvatarData(deadCharacter.Avatar);
			avatarData.InitializeGrowableElementsShowingAbilitiesAndStates(character);
			character.SetAvatar(avatarData, context);
			DomainManager.Character.TryCreateRelation(context, id, taiwuCharId);
			DomainManager.Character.DirectlySetFavorabilities(context, id, taiwuCharId, 30000, 30000);
			if (ProfessionSkillHandle.BuddhistMonkSkill_TryGetSamsaraFeature(reincarnatedCharId, out var featureID))
			{
				character.AddFeature(context, featureID);
				ProfessionSkillHandle.BuddhistMonkSkill_RemoveSamsaraFeature(context, reincarnatedCharId);
			}
			int motherId = ProfessionSkillHandle.BuddhistMonkSkill_GetDirectedSamsaraMother(reincarnatedCharId);
			ProfessionSkillHandle.BuddhistMonkSkill_TryRemoveDirectedSamsara(context, motherId, addMonthlyEvent: false);
		}
	}

	public static void RaiseRelationAdded(DataContext context, int charId, int relatedCharId, ushort relationType)
	{
		if (relatedCharId == DomainManager.Taiwu.GetTaiwuCharId() && DomainManager.Character.TryGetElement_Objects(charId, out var element))
		{
			switch (relationType)
			{
			case 8192:
			{
				OrganizationInfo organizationInfo2 = element.GetOrganizationInfo();
				TryAddMartialArtist7Seniority(organizationInfo2);
				TryAddBeggar4Seniority(organizationInfo2);
				TryAddCivilian6Seniority(organizationInfo2);
				break;
			}
			case 32768:
			{
				OrganizationInfo organizationInfo = element.GetOrganizationInfo();
				TryAddBeggar5Seniority(organizationInfo);
				break;
			}
			}
		}
		void TryAddBeggar4Seniority(OrganizationInfo orgInfo)
		{
			if (orgInfo.Grade <= 2 && DomainManager.Extra.TryTriggerAddSeniorityPoint(context, 63, charId))
			{
				int baseDelta = ProfessionFormulaImpl.Calculate(63, orgInfo.Grade);
				DomainManager.Extra.ChangeProfessionSeniority(context, 9, baseDelta);
			}
		}
		void TryAddBeggar5Seniority(OrganizationInfo orgInfo)
		{
			if (orgInfo.Grade >= 6 && DomainManager.Extra.TryTriggerAddSeniorityPoint(context, 64, charId))
			{
				int baseDelta = ProfessionFormulaImpl.Calculate(64, orgInfo.Grade);
				DomainManager.Extra.ChangeProfessionSeniority(context, 9, baseDelta);
			}
		}
		void TryAddCivilian6Seniority(OrganizationInfo orgInfo)
		{
			if (!OrganizationDomain.IsSect(orgInfo.OrgTemplateId) && orgInfo.Grade >= 3 && DomainManager.Extra.TryTriggerAddSeniorityPoint(context, 70, charId))
			{
				int baseDelta = ProfessionFormulaImpl.Calculate(70, orgInfo.Grade);
				DomainManager.Extra.ChangeProfessionSeniority(context, 10, baseDelta);
			}
		}
		void TryAddMartialArtist7Seniority(OrganizationInfo orgInfo)
		{
			if (OrganizationDomain.IsSect(orgInfo.OrgTemplateId) && orgInfo.Grade >= 3 && DomainManager.Extra.TryTriggerAddSeniorityPoint(context, 29, charId))
			{
				int baseDelta = ProfessionFormulaImpl.Calculate(29, orgInfo.Grade);
				DomainManager.Extra.ChangeProfessionSeniority(context, 3, baseDelta);
			}
		}
	}

	public static void RaiseCharacterAgeChanged(DataContext context, GameData.Domains.Character.Character character, int fromCurrAge, int toCurrAge)
	{
		if (character.GetCreatingType() == 1)
		{
			int id = character.GetId();
			if (character.GetAgeGroup() != 0)
			{
				DomainManager.Character.RemoveInfant(id);
			}
			else if (!DomainManager.Character.IsInfantInMap(id))
			{
				DomainManager.Character.AddInfant(id);
			}
			if (fromCurrAge < 16 && toCurrAge >= 16)
			{
				DomainManager.Character.GenerateCharacterProfession(context, character);
			}
			OrganizationInfo organizationInfo = character.GetOrganizationInfo();
			DomainManager.Organization.TryAddSectMemberFeature(context, character, organizationInfo);
		}
	}

	public static void RaiseIntelligentCharacterDead(DataContext context, GameData.Domains.Character.Character character, CharacterDeathTypeItem deathType, ref CharacterDeathInfo deathInfo)
	{
		int id = character.GetId();
		Location location = character.GetLocation();
		DomainManager.SpecialEffect.OnCharacterRemoved(context, character);
		DomainManager.Organization.OnCharacterDead(context, character);
		DomainManager.LegendaryBook.OnCharacterDead(context, character);
		DomainManager.Map.OnCharacterLocationChanged(context, id, location, Location.Invalid);
		DomainManager.Map.OnInfectedCharacterLocationChanged(context, id, location, Location.Invalid);
		DomainManager.Taiwu.ClearTeachTaiwuLifeSkillList(context, id);
		DomainManager.Taiwu.ClearTeachTaiwuCombatSkillList(context, id);
		DomainManager.CombatSkill.RemoveAllCombatSkills(id);
		DomainManager.Extra.RemoveCharacterEquippedCombatSkills(context, id);
		DomainManager.Extra.RemoveCharacterCombatSkillConfiguration(context, id);
		DomainManager.Extra.ClearCharacterMasteredCombatSkills(context, id);
		DomainManager.Extra.ClearCharTeammateCommands(context, id);
		DomainManager.Extra.RemovePoisonImmunities(context, id);
		DomainManager.Extra.RemoveCharacterProfessions(context, id);
		IntPair directedSamsaraInfo = DomainManager.Character.GetDirectedSamsaraInfo(id);
		if (directedSamsaraInfo.First >= 0)
		{
			MonthlyNotificationCollection monthlyNotificationCollection = DomainManager.World.GetMonthlyNotificationCollection();
			monthlyNotificationCollection.AddMiscarriageAndReincarnationMotherKilled(id, location, directedSamsaraInfo.First);
			DomainManager.Building.TryRemoveSamsaraPlatformBornData(context, id);
			ProfessionSkillHandle.BuddhistMonkSkill_TryRemoveDirectedSamsara(context, id);
		}
		DomainManager.World.TryRemoveLifeLinkCharacter(context, character);
		DomainManager.Information.RemoveCharacterAllInformation(context, id);
		LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
		lifeRecordCollection.AddDeathRecord(character, deathType, ref deathInfo);
		DomainManager.LifeRecord.GenerateDead(id);
		DomainManager.TaiwuEvent.OnCharacterDie(id);
		DomainManager.Extra.TryRemoveStoneRoomCharacter(context, character, isCharacterAlive: false);
		ProfessionSkillHandle.DukeSkill_RemoveCharacterTitle(context, id);
		DomainManager.Extra.RemoveCharacterRevealedHobbies(context, id);
		DomainManager.Extra.RemoveTaiwuGiftItemsForCharacter(context, id);
		DomainManager.Extra.RemoveVillageSkillLegacy(context, id);
	}

	public static void RaiseNonIntelligentCharacterRemoved(DataContext context, GameData.Domains.Character.Character character)
	{
		int id = character.GetId();
		DomainManager.SpecialEffect.OnCharacterRemoved(context, character);
		byte creatingType = character.GetCreatingType();
		Location location = character.GetLocation();
		if (location.IsValid())
		{
			switch (creatingType)
			{
			case 0:
				RaiseFixedCharacterLocationChanged(context, id, location, Location.Invalid);
				break;
			case 2:
			case 3:
				RaiseEnemyCharacterLocationChanged(context, id, location, Location.Invalid);
				break;
			}
		}
		DomainManager.CombatSkill.RemoveAllCombatSkills(id);
		DomainManager.Extra.ClearCharacterMasteredCombatSkills(context, id);
		DomainManager.Extra.RemovePoisonImmunities(context, id);
		DomainManager.Extra.RemoveCharacterCustomDisplayName(context, id);
	}

	public static void RaiseTemporaryIntelligentCharacterRemoved(DataContext context, GameData.Domains.Character.Character character)
	{
		int id = character.GetId();
		DomainManager.SpecialEffect.OnCharacterRemoved(context, character);
		Location location = character.GetLocation();
		Tester.Assert(!location.IsValid() || !DomainManager.Map.ContainsCharacter(location, id));
		Tester.Assert(!DomainManager.Organization.TryGetSettlementCharacter(id, out var _));
		Tester.Assert(!DomainManager.Taiwu.VillagerHasWork(id));
		Tester.Assert(!DomainManager.Taiwu.IsInGroup(id));
		DomainManager.CombatSkill.RemoveAllCombatSkills(id);
		Tester.Assert(DomainManager.LegendaryBook.GetCharOwnedBookTypes(id) == null);
		DomainManager.Extra.ClearCharacterMasteredCombatSkills(context, id);
		DomainManager.Extra.RemovePoisonImmunities(context, id);
		DomainManager.LifeRecord.Remove(id);
		DomainManager.TaiwuEvent.OnTemporaryIntelligentCharacterRemoved(id);
	}

	public static void RaiseXiangshuInfectionFeatureChanged(DataContext context, GameData.Domains.Character.Character character, short featureId)
	{
		int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
		int id = character.GetId();
		if (featureId == 218)
		{
			int leaderId = character.GetLeaderId();
			int kidnapperId = character.GetKidnapperId();
			if (!character.GetLocation().IsValid())
			{
				if ((leaderId < 0 || id == leaderId) && DomainManager.Character.TryGetElement_CrossAreaMoveInfos(id, out var value))
				{
					Location targetLocation = DomainManager.Map.CrossAreaTravelInfoToLocation(value);
					DomainManager.Character.RemoveCrossAreaTravelInfo(context, id);
					DomainManager.Character.GroupMove(context, character, targetLocation);
				}
				else if (kidnapperId >= 0)
				{
					DomainManager.Character.RemoveKidnappedCharacter(context, id, kidnapperId, isEscaped: true);
				}
				else if (character.IsActiveExternalRelationState(32))
				{
					sbyte prisonerSect = DomainManager.Organization.GetPrisonerSect(id);
					Sect sect = (Sect)DomainManager.Organization.GetSettlementByOrgTemplateId(prisonerSect);
					sect.AddPrisoner(context, character, 39);
				}
				else
				{
					Location validLocation = character.GetValidLocation();
					character.SetLocation(validLocation, context);
				}
			}
			DomainManager.Character.LeaveGroup(context, character, bringWards: false);
			OrganizationInfo organizationInfo = character.GetOrganizationInfo();
			OrganizationInfo destOrgInfo = new OrganizationInfo(20, organizationInfo.Grade, principal: true, -1);
			DomainManager.Organization.ChangeOrganization(context, character, destOrgInfo);
			DomainManager.Character.AddInfectedCharToSet(id);
			DomainManager.LegendaryBook.LoseAllLegendaryBooks(context, character, createAdventures: true);
			DomainManager.World.TryRemoveLifeLinkCharacter(context, character);
			Location location = character.GetLocation();
			int currDate = DomainManager.World.GetCurrDate();
			if (GameData.Domains.World.SharedMethods.SmallVillageXiangshuProgress())
			{
				DomainManager.LifeRecord.GetLifeRecordCollection().AddSmallVillagerXiangshuCompletelyInfected(id, currDate, location);
			}
			else
			{
				DomainManager.LifeRecord.GetLifeRecordCollection().AddXiangshuCompletelyInfected(id, currDate, location);
			}
			if (organizationInfo.OrgTemplateId == 16 || DomainManager.Character.IsTaiwuPeople(id))
			{
				DomainManager.World.GetMonthlyNotificationCollection().AddInfectXiangshuCompletely(id, location);
			}
			RaiseCharacterLocationChanged(context, id, location, Location.Invalid);
			if (!character.IsActiveExternalRelationState(60))
			{
				RaiseInfectedCharacterLocationChanged(context, id, Location.Invalid, location);
			}
		}
		else if (character.GetOrganizationInfo().OrgTemplateId == 20)
		{
			Location location2 = character.GetLocation();
			if (!location2.IsValid())
			{
				location2 = character.GetValidLocation();
			}
			int currDate2 = DomainManager.World.GetCurrDate();
			LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
			if (character.IsActiveExternalRelationState(8))
			{
				lifeRecordCollection.AddSavedFromInfection(id, currDate2, taiwuCharId, DomainManager.Taiwu.GetTaiwuVillageLocation());
				DomainManager.Extra.TryRemoveStoneRoomCharacter(context, character);
			}
			else if (GameData.Domains.World.SharedMethods.SmallVillageXiangshuProgress())
			{
				lifeRecordCollection.AddSmallVillagerSavedFromInfection(id, currDate2, taiwuCharId, location2);
			}
			else
			{
				lifeRecordCollection.AddSavedFromInfection(id, currDate2, taiwuCharId, location2);
			}
			DomainManager.Organization.JoinNearbyVillageTownAsBeggar(context, character, -1);
			DomainManager.Character.RemoveInfectedCharFromSet(id);
			RaiseInfectedCharacterLocationChanged(context, id, location2, Location.Invalid);
			RaiseCharacterLocationChanged(context, id, Location.Invalid, location2);
		}
		else if (featureId == 217)
		{
			Location location3 = character.GetLocation();
			int currDate3 = DomainManager.World.GetCurrDate();
			DomainManager.LifeRecord.GetLifeRecordCollection().AddXiangshuPartiallyInfected(id, currDate3, location3);
			if (DomainManager.Character.IsTaiwuPeople(id))
			{
				DomainManager.World.GetMonthlyNotificationCollection().AddInfectXiangshuPartially(id, location3);
			}
		}
		RaiseXiangshuInfectionFeatureChangedEnd(context, character, featureId);
	}

	public static void RaiseCharacterOrganizationChanged(DataContext context, GameData.Domains.Character.Character character, OrganizationInfo srcOrgInfo, OrganizationInfo dstOrgInfo)
	{
		if (srcOrgInfo.OrgTemplateId == dstOrgInfo.OrgTemplateId)
		{
			return;
		}
		if (srcOrgInfo.SettlementId >= 0)
		{
			DomainManager.Organization.GetSettlement(srcOrgInfo.SettlementId).RemoveSettlementFeatures(context, character);
		}
		if (dstOrgInfo.SettlementId >= 0)
		{
			DomainManager.Organization.GetSettlement(dstOrgInfo.SettlementId).AddSettlementFeatures(context, character);
		}
		List<short> featureIds = character.GetFeatureIds();
		for (int num = featureIds.Count - 1; num >= 0; num--)
		{
			short num2 = featureIds[num];
			CharacterFeatureItem characterFeatureItem = CharacterFeature.Instance[num2];
			if (!characterFeatureItem.IsAllowedForOrganization(dstOrgInfo.OrgTemplateId))
			{
				character.RemoveFeature(context, num2);
				AdaptableLog.TagInfo("RaiseCharacterOrganizationChanged", $"Removing feature {characterFeatureItem.Name} for {character} on organization changed.");
			}
		}
		OrganizationItem organizationItem = Organization.Instance[srcOrgInfo.OrgTemplateId];
		if (organizationItem.IsSect && DomainManager.Extra.TaiwuWantedInteracted(character.GetId()))
		{
			DomainManager.Extra.TaiwuWantedRemoveInteracted(context, character.GetId());
		}
		bool flag = srcOrgInfo.OrgTemplateId == 16 && dstOrgInfo.OrgTemplateId != 20;
		bool flag2 = srcOrgInfo.OrgTemplateId == 20 && dstOrgInfo.OrgTemplateId != 16;
		if (flag || flag2)
		{
			character.ReturnVillagerRoleClothing(context);
		}
		if (dstOrgInfo.OrgTemplateId == 16)
		{
			InstantNotificationCollection instantNotificationCollection = DomainManager.World.GetInstantNotificationCollection();
			instantNotificationCollection.AddJoinTaiwuVillage(character.GetId());
			DomainManager.Extra.CalcSingleVillagerSkillLegacy(context, character.GetId());
			DomainManager.Extra.RegisterVillagerSkillLegacyPoint(character.GetId());
		}
		else if (srcOrgInfo.OrgTemplateId == 16)
		{
			DomainManager.Extra.UnRegisterVillagerSkillLegacyPoint(character.GetId());
		}
		DomainManager.Building.TryRemoveFeastCustomer(context, character.GetId());
	}

	public static void RaiseLegendaryBookOwnerStateChanged(DataContext context, GameData.Domains.Character.Character character, sbyte ownerState)
	{
		int id = character.GetId();
		LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
		MonthlyNotificationCollection monthlyNotificationCollection = DomainManager.World.GetMonthlyNotificationCollection();
		MonthlyEventCollection monthlyEventCollection = DomainManager.World.GetMonthlyEventCollection();
		int currDate = DomainManager.World.GetCurrDate();
		Location location = character.GetLocation();
		List<sbyte> charOwnedBookTypes = DomainManager.LegendaryBook.GetCharOwnedBookTypes(id);
		character.TryRetireTreasuryGuard(context);
		switch (ownerState)
		{
		case 1:
			AdaptableLog.TagInfo("LegendaryBook", $"{character} => 因奇书执迷入邪");
			if (location.IsValid() && location.AreaId < 45)
			{
				DomainManager.LegendaryBook.UpgradeEnemyNestsByLegendaryBookOwner(context, location.AreaId, 4);
			}
			{
				foreach (sbyte item in charOwnedBookTypes)
				{
					ItemKey legendaryBookItem4 = DomainManager.LegendaryBook.GetLegendaryBookItem(item);
					lifeRecordCollection.AddLegendaryBookShocked(id, currDate, location, legendaryBookItem4.ItemType, legendaryBookItem4.TemplateId);
					monthlyNotificationCollection.AddLegendaryBookShocked(id, location, legendaryBookItem4.ItemType, legendaryBookItem4.TemplateId);
				}
				break;
			}
		case 2:
		{
			AdaptableLog.TagInfo("LegendaryBook", $"{character} => 因奇书执迷化魔");
			OrganizationInfo organizationInfo = character.GetOrganizationInfo();
			OrganizationMemberItem orgMemberConfig = OrganizationDomain.GetOrgMemberConfig(organizationInfo);
			if (organizationInfo.Principal && orgMemberConfig.RestrictPrincipalAmount)
			{
				for (sbyte b = (sbyte)(organizationInfo.Grade - 1); b >= 0; b--)
				{
					if (!OrganizationDomain.GetOrgMemberConfig(organizationInfo.OrgTemplateId, b).RestrictPrincipalAmount)
					{
						ItemKey legendaryBookItem2 = DomainManager.LegendaryBook.GetLegendaryBookItem(charOwnedBookTypes[0]);
						lifeRecordCollection.AddResignPositionToStudyLegendaryBook(id, currDate, location, legendaryBookItem2.ItemType, legendaryBookItem2.TemplateId, organizationInfo.SettlementId, organizationInfo.OrgTemplateId, organizationInfo.Grade, orgPrincipal: true, character.GetGender());
						DomainManager.Organization.ChangeGrade(context, character, b, destPrincipal: true);
						int aliveSpouse = DomainManager.Character.GetAliveSpouse(id);
						if (aliveSpouse >= 0)
						{
							GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(aliveSpouse);
							DomainManager.Organization.UpdateGradeAccordingToSpouse(context, element_Objects, character);
						}
						break;
					}
				}
			}
			DomainManager.World.TryRemoveLifeLinkCharacter(context, character);
			if (location.IsValid() && location.AreaId < 45)
			{
				DomainManager.LegendaryBook.UpgradeEnemyNestsByLegendaryBookOwner(context, location.AreaId, 7);
			}
			{
				foreach (sbyte item2 in charOwnedBookTypes)
				{
					short legendaryBookFeature = Config.CombatSkillType.Instance[item2].LegendaryBookFeature;
					DomainManager.Character.RegisterFeatureForAllXiangshuAvatars(context, legendaryBookFeature);
					ItemKey legendaryBookItem3 = DomainManager.LegendaryBook.GetLegendaryBookItem(item2);
					lifeRecordCollection.AddLegendaryBookInsane(id, currDate, location, legendaryBookItem3.ItemType, legendaryBookItem3.TemplateId);
					monthlyNotificationCollection.AddLegendaryBookInsane(id, location, legendaryBookItem3.ItemType, legendaryBookItem3.TemplateId);
					monthlyEventCollection.AddSwordTombGetStronger((ulong)legendaryBookItem3);
				}
				break;
			}
		}
		case 3:
			DomainManager.World.TryRemoveLifeLinkCharacter(context, character);
			lifeRecordCollection.AddLegendaryBookConsumed(id, currDate, location);
			foreach (sbyte item3 in charOwnedBookTypes)
			{
				ItemKey legendaryBookItem = DomainManager.LegendaryBook.GetLegendaryBookItem(item3);
				monthlyNotificationCollection.AddLegendaryBookConsumed(id, location, legendaryBookItem.ItemType, legendaryBookItem.TemplateId);
			}
			monthlyNotificationCollection.AddXiangshuGetStrengthened();
			DomainManager.Organization.ChangeOrganization(context, character, OrganizationInfo.None);
			if (location.IsValid() && location.AreaId < 45)
			{
				List<short> list = ObjectPool<List<short>>.Instance.Get();
				DomainManager.Map.GetAllBrokenAreaInState(DomainManager.Map.GetStateIdByAreaId(location.AreaId), list);
				if (list.Count > 0)
				{
					CollectionUtils.Shuffle(context.Random, list);
					List<MapBlockData> list2 = ObjectPool<List<MapBlockData>>.Instance.Get();
					short areaId = list[0];
					DomainManager.Adventure.GetValidBlocksForRandomEnemy(areaId, -1, -1, onAdventureSite: true, onSettlement: true, nearTaiwu: true, list2);
					if (list2.Count > 0)
					{
						CollectionUtils.Shuffle(context.Random, list2);
						Location location2 = list2[0].GetLocation();
						DomainManager.Character.LeaveGroup(context, character, bringWards: false);
						character.SetLocation(location2, context);
						RaiseCharacterLocationChanged(context, id, location, location2);
					}
					ObjectPool<List<MapBlockData>>.Instance.Return(list2);
				}
				ObjectPool<List<short>>.Instance.Return(list);
			}
			AdaptableLog.TagInfo("LegendaryBook", $"{character} => 因奇书被相枢吞噬");
			break;
		}
	}

	public static void RaiseKidnappedStatusChanged(DataContext context, GameData.Domains.Character.Character character, bool isKidnapped)
	{
		int id = character.GetId();
		if (isKidnapped)
		{
			DomainManager.Taiwu.RemoveVillagerWork(context, id);
			DomainManager.Building.MakeTargetHomeless(context, id);
			DomainManager.Building.TryRemoveFeastCustomer(context, id);
		}
	}

	public static void RaiseGraveLocationChanged(DataContext context, int graveId, Location srcLocation, Location destLocation)
	{
		DomainManager.Map.OnGraveLocationChanged(context, graveId, srcLocation, destLocation);
	}

	public static void RaiseTemplateEnemyLocationChanged(DataContext context, MapTemplateEnemyInfo templateEnemyInfo, Location srcLocation, Location destLocation)
	{
		DomainManager.Map.OnTemplateEnemyLocationChanged(context, templateEnemyInfo, srcLocation, destLocation);
		DomainManager.Adventure.OnRandomEnemyLocationChange(context, templateEnemyInfo, srcLocation, destLocation);
	}

	public static void RaiseInfectedCharacterLocationChanged(DataContext context, int charId, Location srcLocation, Location destLocation)
	{
		DomainManager.Map.OnInfectedCharacterLocationChanged(context, charId, srcLocation, destLocation);
	}

	public static void RaiseFixedCharacterLocationChanged(DataContext context, int charId, Location srcLocation, Location destLocation)
	{
		DomainManager.Map.OnFixedCharacterLocationChanged(context, charId, srcLocation, destLocation);
		DomainManager.Character.OnFixedCharacterLocationChanged(context, charId, srcLocation, destLocation);
	}

	public static void RaiseEnemyCharacterLocationChanged(DataContext context, int charId, Location srcLocation, Location destLocation)
	{
		DomainManager.Map.OnEnemyCharacterLocationChanged(context, charId, srcLocation, destLocation);
	}

	public static void RaiseCharacterCustomDisplayNameChanged(DataContext context, int charId)
	{
		DomainManager.TaiwuEvent.UpdateEventLogCharacterDisplayData(charId);
	}

	public static void RaiseCombatEntry(DataContext context, List<int> enemyIds, short combatConfigTemplateId)
	{
		DomainManager.World.StatSectMainStoryCombatTimes(combatConfigTemplateId);
	}

	public static void RaiseEventWindowFocusStateChanged(DataContext context, bool focusState)
	{
		DomainManager.Adventure.SetOperationBlock(focusState, context);
		if (!focusState)
		{
			DomainManager.TaiwuEvent.BlockEventLogStatusCheck();
		}
	}

	public static void RaiseOneShotEventHandled(DataContext context, int oneShotEventType)
	{
		DomainManager.Extra.UpdateProfessionExtraSkillUnlocked(context, oneShotEventType);
	}

	public static void RaiseEventHandleComplete(DataContext context)
	{
		GameData.GameDataBridge.GameDataBridge.AddDisplayEvent(DisplayEventType.EventHandleComplete);
	}

	public static void RaiseEventOnLegacyPassingStateChange(DataContext context, sbyte targetState)
	{
		DomainManager.TaiwuEvent.OnPassingLegacyStateChange(targetState);
	}

	public static void ClearPassingLegacyWhileAdvancingMonthHandlers(DataContext context)
	{
		_handlersPassingLegacyWhileAdvancingMonth = null;
	}

	public static void RaiseCharacterApproveTaiwuStatusChanged(DataContext context, SettlementCharacter settlementChar, bool approve, bool updateBlock = true)
	{
		if (approve)
		{
			DomainManager.Taiwu.AddLegacyPoint(context, 38);
			int baseDelta = ProfessionFormulaImpl.Calculate(55, settlementChar.GetInfluencePower());
			DomainManager.Extra.ChangeProfessionSeniority(context, 8, baseDelta);
		}
		else
		{
			ProfessionData professionData = DomainManager.Extra.GetProfessionData(17);
			if (professionData != null && professionData.SkillsData != null)
			{
				ProfessionSkillHandle.DukeSkill_RemoveCharacterTitle(context, professionData, settlementChar.GetId());
			}
		}
		if (updateBlock)
		{
			Location location = DomainManager.Taiwu.GetTaiwu().GetLocation();
			if (DomainManager.Taiwu.GetTaiwu().GetLocation().IsValid())
			{
				MapBlockData block = DomainManager.Map.GetBlock(location);
				DomainManager.Map.SetBlockData(context, block);
			}
		}
	}

	public static void RaiseEvaluationAddExp(DataContext context, int exp)
	{
		DomainManager.World.AddEmeiExp(context, exp);
	}

	public static void RaiseCricketCombatStarted(DataContext context)
	{
		DomainManager.TaiwuEvent.RecordCharacterEnterCricketCombat();
	}

	public static void RaiseCricketCombatFinished(DataContext context, bool isTaiwuWin)
	{
		DomainManager.TaiwuEvent.RecordCombatResult(isTaiwuWin);
	}

	public static void RaiseLifeSkillCombatStarted(DataContext context)
	{
		DomainManager.TaiwuEvent.RecordCharacterEnterLifeCombat();
	}

	public static void RaiseTaiwuOrTeammatePregnant(DataContext context, int charId, Location location)
	{
		DomainManager.World.GetMonthlyEventCollection().AddPregnant(charId, location);
	}

	public static void RaiseAdventureSiteStateChanged(DataContext context, short areaId, short blockId, AdventureSiteData siteData)
	{
		sbyte siteState = siteData.SiteState;
		sbyte b = siteState;
		if (b != 1)
		{
			return;
		}
		GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
		Location location = taiwu.GetLocation();
		if (location.AreaId == areaId)
		{
			Location location2 = new Location(areaId, blockId);
			MapBlockData block = DomainManager.Map.GetBlock(location2);
			InstantNotificationCollection instantNotifications = DomainManager.World.GetInstantNotifications();
			if (location.AreaId == areaId && block.Visible)
			{
				instantNotifications.AddBeginAdventure(location2, siteData.TemplateId);
			}
		}
	}

	public static void RaiseAdventureRemoved(DataContext context, short areaId, short blockId, bool isTimeout, bool isComplete, AdventureSiteData siteData)
	{
		MonthlyActionBase monthlyAction = DomainManager.TaiwuEvent.GetMonthlyAction(siteData.MonthlyActionKey);
		if (monthlyAction != null)
		{
			if (monthlyAction is IMonthlyActionGroup monthlyActionGroup)
			{
				monthlyActionGroup.DeactivateSubAction(areaId, blockId, isComplete);
			}
			else
			{
				monthlyAction.Deactivate(isComplete);
			}
			if (siteData.MonthlyActionKey.ActionType == 6)
			{
				DomainManager.TaiwuEvent.RemoveTempDynamicAction(DomainManager.TaiwuEvent.MainThreadDataContext, siteData.MonthlyActionKey);
			}
		}
		MonthlyNotificationCollection monthlyNotificationCollection = DomainManager.World.GetMonthlyNotificationCollection();
		Location location = new Location(areaId, blockId);
		if (siteData.SiteState >= 2)
		{
			monthlyNotificationCollection.AddEnemyNestDemise(location, siteData.TemplateId);
		}
		sbyte type = siteData.GetConfig().Type;
		if (type >= 9 && type <= 14)
		{
			DomainManager.Extra.OnBuildingAreaEffectLocationChanged(context, 2, location, Location.Invalid);
		}
		switch (siteData.TemplateId)
		{
		case 28:
		{
			EventArgBox globalEventArgumentBox = DomainManager.TaiwuEvent.GetGlobalEventArgumentBox();
			int arg = -1;
			if (!globalEventArgumentBox.Get("StoryForeverLoverId", ref arg))
			{
				break;
			}
			if (arg >= 0 && DomainManager.Character.TryGetElement_Objects(arg, out var element))
			{
				OrganizationInfo organizationInfo = element.GetOrganizationInfo();
				OrganizationItem organizationItem = Organization.Instance[organizationInfo.OrgTemplateId];
				if (organizationItem.IsSect)
				{
					DomainManager.Character.UnhideCharacterOnMap(context, element, 16);
					short punishmentFeature = organizationItem.PunishmentFeature;
					if (punishmentFeature >= 0)
					{
						element.AddFeature(context, punishmentFeature);
					}
					if (DomainManager.Organization.TryGetElement_Sects(organizationInfo.SettlementId, out var element2))
					{
						DomainManager.Organization.PunishSectMember(context, element2, element, 3, 43, isArrested: true);
						LifeRecordCollection lifeRecordCollection = EventHelper.GetLifeRecordCollection();
						lifeRecordCollection.AddSectPunishElope(arg, EventHelper.GetGameDate());
					}
				}
			}
			globalEventArgumentBox.Remove<int>("StoryForeverLoverId");
			break;
		}
		case 170:
		case 184:
			if (isTimeout)
			{
				DomainManager.Extra.TriggerExtraTask(context, 40, 300);
			}
			break;
		case 193:
			if (isTimeout)
			{
				MonthlyEventCollection monthlyEventCollection = DomainManager.World.GetMonthlyEventCollection();
				monthlyEventCollection.AddSectMainStoryZhujianFailing();
			}
			break;
		}
		sbyte xiangshuAvatarIdBySwordTomb = XiangshuAvatarIds.GetXiangshuAvatarIdBySwordTomb(siteData.TemplateId);
		if (xiangshuAvatarIdBySwordTomb >= 0)
		{
			for (short num = XiangshuAvatarIds.WeakenedXiangshuBossBeginIds[xiangshuAvatarIdBySwordTomb]; num <= XiangshuAvatarIds.WeakenedXiangshuBossEndIds[xiangshuAvatarIdBySwordTomb]; num++)
			{
				if (DomainManager.Character.TryGetFixedCharacterByTemplateId(num, out var character))
				{
					DomainManager.Character.RemoveNonIntelligentCharacter(context, character);
				}
			}
			IReadOnlySet<int> villagerRoleSet = DomainManager.Taiwu.GetVillagerRoleSet(5);
			foreach (int item in villagerRoleSet)
			{
				VillagerRoleBase villagerRole = DomainManager.Extra.GetVillagerRole(item);
				if (villagerRole is VillagerRoleSwordTombKeeper { ArrangementTemplateId: 3 } villagerRoleSwordTombKeeper && villagerRoleSwordTombKeeper.XiangshuAvatarId == xiangshuAvatarIdBySwordTomb)
				{
					DomainManager.Taiwu.RemoveVillagerWork(context, item);
				}
			}
		}
		if (DomainManager.World.GetAdvancingMonthState() == 0)
		{
			DomainManager.TaiwuEvent.OnEvent_AdventureRemoved(siteData.TemplateId, location, isComplete);
		}
	}

	public static void RaiseCricketCreated(DataContext context, ItemKey cricketKey)
	{
		DomainManager.Extra.OnCricketCreated(context, cricketKey);
	}

	public static void RaiseCricketRemoved(DataContext context, ItemKey cricketKey)
	{
		DomainManager.Extra.OnCricketRemoved(context, cricketKey);
	}

	public static void RaiseCarrierCreated(DataContext context, ItemKey carrierKey)
	{
		DomainManager.Extra.OnCarrierCreated(context, carrierKey);
	}

	public static void RaiseCarrierRemoved(DataContext context, ItemKey carrierKey)
	{
		DomainManager.Extra.OnCarrierRemoved(context, carrierKey);
	}

	public static void RaiseSettlementInfoChanged(DataContext context, Settlement settlement)
	{
		List<short> list = new List<short>();
		Location location = settlement.GetLocation();
		DomainManager.Map.GetSettlementBlocks(location.AreaId, location.BlockId, list);
		foreach (short item in list)
		{
			MapBlockData blockData = DomainManager.Map.GetBlockData(location.AreaId, item);
			DomainManager.Map.SetBlockData(context, blockData);
		}
	}

	public static void ClearAllHandlers()
	{
		_handlersCharacterLocationChanged = null;
		_handlersMakeLove = null;
		_handlersEatingItem = null;
		_handlersXiangshuInfectionFeatureChangedEnd = null;
		_handlersCombatBegin = null;
		_handlersCombatSettlement = null;
		_handlersCombatEnd = null;
		_handlersChangeNeiliAllocationAfterCombatBegin = null;
		_handlersCreateGangqiAfterChangeNeiliAllocation = null;
		_handlersChangeBossPhase = null;
		_handlersGetTrick = null;
		_handlersRearrangeTrick = null;
		_handlersGetShaTrick = null;
		_handlersRemoveShaTrick = null;
		_handlersOverflowTrickRemoved = null;
		_handlersCostBreathAndStance = null;
		_handlersChangeWeapon = null;
		_handlersWeaponCdEnd = null;
		_handlersChangeTrickCountChanged = null;
		_handlersUnlockAttack = null;
		_handlersUnlockAttackEnd = null;
		_handlersNormalAttackPrepareEnd = null;
		_handlersNormalAttackOutOfRange = null;
		_handlersNormalAttackBegin = null;
		_handlersNormalAttackCalcHitEnd = null;
		_handlersNormalAttackCalcCriticalEnd = null;
		_handlersNormalAttackEnd = null;
		_handlersNormalAttackAllEnd = null;
		_handlersCastSkillUseExtraBreathOrStance = null;
		_handlersCastSkillUseMobilityAsBreathOrStance = null;
		_handlersCastLegSkillWithAgile = null;
		_handlersCastSkillOnLackBreathStance = null;
		_handlersCastSkillTrickCosted = null;
		_handlersJiTrickInsteadCostTricks = null;
		_handlersUselessTrickInsteadJiTricks = null;
		_handlersShaTrickInsteadCostTricks = null;
		_handlersCastSkillCosted = null;
		_handlersChangePreparingSkillBegin = null;
		_handlersCastAgileOrDefenseWithoutPrepareBegin = null;
		_handlersCastAgileOrDefenseWithoutPrepareEnd = null;
		_handlersPrepareSkillEffectNotYetCreated = null;
		_handlersPrepareSkillBegin = null;
		_handlersPrepareSkillProgressChange = null;
		_handlersPrepareSkillChangeDistance = null;
		_handlersPrepareSkillEnd = null;
		_handlersCastAttackSkillBegin = null;
		_handlersAttackSkillAttackBegin = null;
		_handlersAttackSkillAttackHit = null;
		_handlersAttackSkillAttackEnd = null;
		_handlersAttackSkillAttackEndOfAll = null;
		_handlersCastSkillEnd = null;
		_handlersCastSkillAllEnd = null;
		_handlersCalcLeveragingValue = null;
		_handlersWisdomCosted = null;
		_handlersHealedInjury = null;
		_handlersHealedPoison = null;
		_handlersUsedMedicine = null;
		_handlersUsedCustomItem = null;
		_handlersInterruptOtherAction = null;
		_handlersFlawAdded = null;
		_handlersFlawRemoved = null;
		_handlersAcuPointAdded = null;
		_handlersAcuPointRemoved = null;
		_handlersCombatCharChanged = null;
		_handlersAddInjury = null;
		_handlersAddDirectDamageValue = null;
		_handlersAddDirectInjury = null;
		_handlersBounceInjury = null;
		_handlersAddMindMark = null;
		_handlersAddMindDamage = null;
		_handlersAddFatalDamageMark = null;
		_handlersAddDirectFatalDamageMark = null;
		_handlersAddDirectFatalDamage = null;
		_handlersAddDirectPoisonMark = null;
		_handlersMoveStateChanged = null;
		_handlersMoveBegin = null;
		_handlersMoveEnd = null;
		_handlersIgnoredForceChangeDistance = null;
		_handlersDistanceChanged = null;
		_handlersSkillEffectChange = null;
		_handlersSkillSilence = null;
		_handlersSkillSilenceEnd = null;
		_handlersNeiliAllocationChanged = null;
		_handlersAddPoison = null;
		_handlersPoisonAffected = null;
		_handlersAddWug = null;
		_handlersRemoveWug = null;
		_handlersCompareDataCalcFinished = null;
		_handlersCombatStateMachineUpdateEnd = null;
		_handlersCombatCharFallen = null;
		_handlersCombatCostNeiliConfirm = null;
		_handlersCostTrickDuringPreparingSkill = null;
		_handlersCombatChangeDurability = null;
		_handlersPassingLegacyWhileAdvancingMonth = null;
		_handlersAdvanceMonthBegin = null;
		_handlersPostAdvanceMonthBegin = null;
		_handlersAdvanceMonthFinish = null;
		_handlersTaiwuMove = null;
	}

	public static void RegisterHandler_CharacterLocationChanged(OnCharacterLocationChanged handler)
	{
		_handlersCharacterLocationChanged = (OnCharacterLocationChanged)Delegate.Combine(_handlersCharacterLocationChanged, handler);
	}

	public static void UnRegisterHandler_CharacterLocationChanged(OnCharacterLocationChanged handler)
	{
		_handlersCharacterLocationChanged = (OnCharacterLocationChanged)Delegate.Remove(_handlersCharacterLocationChanged, handler);
	}

	public static void RaiseCharacterLocationChanged(DataContext context, int charId, Location srcLocation, Location destLocation)
	{
		_handlersCharacterLocationChanged?.Invoke(context, charId, srcLocation, destLocation);
	}

	public static void RegisterHandler_MakeLove(OnMakeLove handler)
	{
		_handlersMakeLove = (OnMakeLove)Delegate.Combine(_handlersMakeLove, handler);
	}

	public static void UnRegisterHandler_MakeLove(OnMakeLove handler)
	{
		_handlersMakeLove = (OnMakeLove)Delegate.Remove(_handlersMakeLove, handler);
	}

	public static void RaiseMakeLove(DataContext context, GameData.Domains.Character.Character character, GameData.Domains.Character.Character target, sbyte makeLoveState)
	{
		_handlersMakeLove?.Invoke(context, character, target, makeLoveState);
	}

	public static void RegisterHandler_EatingItem(OnEatingItem handler)
	{
		_handlersEatingItem = (OnEatingItem)Delegate.Combine(_handlersEatingItem, handler);
	}

	public static void UnRegisterHandler_EatingItem(OnEatingItem handler)
	{
		_handlersEatingItem = (OnEatingItem)Delegate.Remove(_handlersEatingItem, handler);
	}

	public static void RaiseEatingItem(DataContext context, GameData.Domains.Character.Character character, ItemKey itemKey)
	{
		_handlersEatingItem?.Invoke(context, character, itemKey);
	}

	public static void RegisterHandler_XiangshuInfectionFeatureChangedEnd(OnXiangshuInfectionFeatureChangedEnd handler)
	{
		_handlersXiangshuInfectionFeatureChangedEnd = (OnXiangshuInfectionFeatureChangedEnd)Delegate.Combine(_handlersXiangshuInfectionFeatureChangedEnd, handler);
	}

	public static void UnRegisterHandler_XiangshuInfectionFeatureChangedEnd(OnXiangshuInfectionFeatureChangedEnd handler)
	{
		_handlersXiangshuInfectionFeatureChangedEnd = (OnXiangshuInfectionFeatureChangedEnd)Delegate.Remove(_handlersXiangshuInfectionFeatureChangedEnd, handler);
	}

	public static void RaiseXiangshuInfectionFeatureChangedEnd(DataContext context, GameData.Domains.Character.Character character, short featureId)
	{
		_handlersXiangshuInfectionFeatureChangedEnd?.Invoke(context, character, featureId);
	}

	public static void RegisterHandler_CombatBegin(OnCombatBegin handler)
	{
		_handlersCombatBegin = (OnCombatBegin)Delegate.Combine(_handlersCombatBegin, handler);
	}

	public static void UnRegisterHandler_CombatBegin(OnCombatBegin handler)
	{
		_handlersCombatBegin = (OnCombatBegin)Delegate.Remove(_handlersCombatBegin, handler);
	}

	public static void RaiseCombatBegin(DataContext context)
	{
		_handlersCombatBegin?.Invoke(context);
	}

	public static void RegisterHandler_CombatSettlement(OnCombatSettlement handler)
	{
		_handlersCombatSettlement = (OnCombatSettlement)Delegate.Combine(_handlersCombatSettlement, handler);
	}

	public static void UnRegisterHandler_CombatSettlement(OnCombatSettlement handler)
	{
		_handlersCombatSettlement = (OnCombatSettlement)Delegate.Remove(_handlersCombatSettlement, handler);
	}

	public static void RaiseCombatSettlement(DataContext context, sbyte combatStatus)
	{
		_handlersCombatSettlement?.Invoke(context, combatStatus);
	}

	public static void RegisterHandler_CombatEnd(OnCombatEnd handler)
	{
		_handlersCombatEnd = (OnCombatEnd)Delegate.Combine(_handlersCombatEnd, handler);
	}

	public static void UnRegisterHandler_CombatEnd(OnCombatEnd handler)
	{
		_handlersCombatEnd = (OnCombatEnd)Delegate.Remove(_handlersCombatEnd, handler);
	}

	public static void RaiseCombatEnd(DataContext context)
	{
		_handlersCombatEnd?.Invoke(context);
	}

	public static void RegisterHandler_ChangeNeiliAllocationAfterCombatBegin(OnChangeNeiliAllocationAfterCombatBegin handler)
	{
		_handlersChangeNeiliAllocationAfterCombatBegin = (OnChangeNeiliAllocationAfterCombatBegin)Delegate.Combine(_handlersChangeNeiliAllocationAfterCombatBegin, handler);
	}

	public static void UnRegisterHandler_ChangeNeiliAllocationAfterCombatBegin(OnChangeNeiliAllocationAfterCombatBegin handler)
	{
		_handlersChangeNeiliAllocationAfterCombatBegin = (OnChangeNeiliAllocationAfterCombatBegin)Delegate.Remove(_handlersChangeNeiliAllocationAfterCombatBegin, handler);
	}

	public static void RaiseChangeNeiliAllocationAfterCombatBegin(DataContext context, CombatCharacter character, NeiliAllocation allocationAfterBegin)
	{
		_handlersChangeNeiliAllocationAfterCombatBegin?.Invoke(context, character, allocationAfterBegin);
	}

	public static void RegisterHandler_CreateGangqiAfterChangeNeiliAllocation(OnCreateGangqiAfterChangeNeiliAllocation handler)
	{
		_handlersCreateGangqiAfterChangeNeiliAllocation = (OnCreateGangqiAfterChangeNeiliAllocation)Delegate.Combine(_handlersCreateGangqiAfterChangeNeiliAllocation, handler);
	}

	public static void UnRegisterHandler_CreateGangqiAfterChangeNeiliAllocation(OnCreateGangqiAfterChangeNeiliAllocation handler)
	{
		_handlersCreateGangqiAfterChangeNeiliAllocation = (OnCreateGangqiAfterChangeNeiliAllocation)Delegate.Remove(_handlersCreateGangqiAfterChangeNeiliAllocation, handler);
	}

	public static void RaiseCreateGangqiAfterChangeNeiliAllocation(DataContext context, CombatCharacter character)
	{
		_handlersCreateGangqiAfterChangeNeiliAllocation?.Invoke(context, character);
	}

	public static void RegisterHandler_ChangeBossPhase(OnChangeBossPhase handler)
	{
		_handlersChangeBossPhase = (OnChangeBossPhase)Delegate.Combine(_handlersChangeBossPhase, handler);
	}

	public static void UnRegisterHandler_ChangeBossPhase(OnChangeBossPhase handler)
	{
		_handlersChangeBossPhase = (OnChangeBossPhase)Delegate.Remove(_handlersChangeBossPhase, handler);
	}

	public static void RaiseChangeBossPhase(DataContext context)
	{
		_handlersChangeBossPhase?.Invoke(context);
	}

	public static void RegisterHandler_GetTrick(OnGetTrick handler)
	{
		_handlersGetTrick = (OnGetTrick)Delegate.Combine(_handlersGetTrick, handler);
	}

	public static void UnRegisterHandler_GetTrick(OnGetTrick handler)
	{
		_handlersGetTrick = (OnGetTrick)Delegate.Remove(_handlersGetTrick, handler);
	}

	public static void RaiseGetTrick(DataContext context, int charId, bool isAlly, sbyte trickType, bool usable)
	{
		_handlersGetTrick?.Invoke(context, charId, isAlly, trickType, usable);
	}

	public static void RegisterHandler_RearrangeTrick(OnRearrangeTrick handler)
	{
		_handlersRearrangeTrick = (OnRearrangeTrick)Delegate.Combine(_handlersRearrangeTrick, handler);
	}

	public static void UnRegisterHandler_RearrangeTrick(OnRearrangeTrick handler)
	{
		_handlersRearrangeTrick = (OnRearrangeTrick)Delegate.Remove(_handlersRearrangeTrick, handler);
	}

	public static void RaiseRearrangeTrick(DataContext context, int charId, bool isAlly)
	{
		_handlersRearrangeTrick?.Invoke(context, charId, isAlly);
	}

	public static void RegisterHandler_GetShaTrick(OnGetShaTrick handler)
	{
		_handlersGetShaTrick = (OnGetShaTrick)Delegate.Combine(_handlersGetShaTrick, handler);
	}

	public static void UnRegisterHandler_GetShaTrick(OnGetShaTrick handler)
	{
		_handlersGetShaTrick = (OnGetShaTrick)Delegate.Remove(_handlersGetShaTrick, handler);
	}

	public static void RaiseGetShaTrick(DataContext context, int charId, bool isAlly, bool real)
	{
		_handlersGetShaTrick?.Invoke(context, charId, isAlly, real);
	}

	public static void RegisterHandler_RemoveShaTrick(OnRemoveShaTrick handler)
	{
		_handlersRemoveShaTrick = (OnRemoveShaTrick)Delegate.Combine(_handlersRemoveShaTrick, handler);
	}

	public static void UnRegisterHandler_RemoveShaTrick(OnRemoveShaTrick handler)
	{
		_handlersRemoveShaTrick = (OnRemoveShaTrick)Delegate.Remove(_handlersRemoveShaTrick, handler);
	}

	public static void RaiseRemoveShaTrick(DataContext context, int charId)
	{
		_handlersRemoveShaTrick?.Invoke(context, charId);
	}

	public static void RegisterHandler_OverflowTrickRemoved(OnOverflowTrickRemoved handler)
	{
		_handlersOverflowTrickRemoved = (OnOverflowTrickRemoved)Delegate.Combine(_handlersOverflowTrickRemoved, handler);
	}

	public static void UnRegisterHandler_OverflowTrickRemoved(OnOverflowTrickRemoved handler)
	{
		_handlersOverflowTrickRemoved = (OnOverflowTrickRemoved)Delegate.Remove(_handlersOverflowTrickRemoved, handler);
	}

	public static void RaiseOverflowTrickRemoved(DataContext context, int charId, bool isAlly, int removedCount)
	{
		_handlersOverflowTrickRemoved?.Invoke(context, charId, isAlly, removedCount);
	}

	public static void RegisterHandler_CostBreathAndStance(OnCostBreathAndStance handler)
	{
		_handlersCostBreathAndStance = (OnCostBreathAndStance)Delegate.Combine(_handlersCostBreathAndStance, handler);
	}

	public static void UnRegisterHandler_CostBreathAndStance(OnCostBreathAndStance handler)
	{
		_handlersCostBreathAndStance = (OnCostBreathAndStance)Delegate.Remove(_handlersCostBreathAndStance, handler);
	}

	public static void RaiseCostBreathAndStance(DataContext context, int charId, bool isAlly, int costBreath, int costStance, short skillId)
	{
		_handlersCostBreathAndStance?.Invoke(context, charId, isAlly, costBreath, costStance, skillId);
	}

	public static void RegisterHandler_ChangeWeapon(OnChangeWeapon handler)
	{
		_handlersChangeWeapon = (OnChangeWeapon)Delegate.Combine(_handlersChangeWeapon, handler);
	}

	public static void UnRegisterHandler_ChangeWeapon(OnChangeWeapon handler)
	{
		_handlersChangeWeapon = (OnChangeWeapon)Delegate.Remove(_handlersChangeWeapon, handler);
	}

	public static void RaiseChangeWeapon(DataContext context, int charId, bool isAlly, CombatWeaponData newWeapon, CombatWeaponData oldWeapon)
	{
		_handlersChangeWeapon?.Invoke(context, charId, isAlly, newWeapon, oldWeapon);
	}

	public static void RegisterHandler_WeaponCdEnd(OnWeaponCdEnd handler)
	{
		_handlersWeaponCdEnd = (OnWeaponCdEnd)Delegate.Combine(_handlersWeaponCdEnd, handler);
	}

	public static void UnRegisterHandler_WeaponCdEnd(OnWeaponCdEnd handler)
	{
		_handlersWeaponCdEnd = (OnWeaponCdEnd)Delegate.Remove(_handlersWeaponCdEnd, handler);
	}

	public static void RaiseWeaponCdEnd(DataContext context, int charId, bool isAlly, CombatWeaponData weapon)
	{
		_handlersWeaponCdEnd?.Invoke(context, charId, isAlly, weapon);
	}

	public static void RegisterHandler_ChangeTrickCountChanged(OnChangeTrickCountChanged handler)
	{
		_handlersChangeTrickCountChanged = (OnChangeTrickCountChanged)Delegate.Combine(_handlersChangeTrickCountChanged, handler);
	}

	public static void UnRegisterHandler_ChangeTrickCountChanged(OnChangeTrickCountChanged handler)
	{
		_handlersChangeTrickCountChanged = (OnChangeTrickCountChanged)Delegate.Remove(_handlersChangeTrickCountChanged, handler);
	}

	public static void RaiseChangeTrickCountChanged(DataContext context, CombatCharacter character, int addValue, bool bySelectChangeTrick)
	{
		_handlersChangeTrickCountChanged?.Invoke(context, character, addValue, bySelectChangeTrick);
	}

	public static void RegisterHandler_UnlockAttack(OnUnlockAttack handler)
	{
		_handlersUnlockAttack = (OnUnlockAttack)Delegate.Combine(_handlersUnlockAttack, handler);
	}

	public static void UnRegisterHandler_UnlockAttack(OnUnlockAttack handler)
	{
		_handlersUnlockAttack = (OnUnlockAttack)Delegate.Remove(_handlersUnlockAttack, handler);
	}

	public static void RaiseUnlockAttack(DataContext context, CombatCharacter combatChar, int weaponIndex)
	{
		_handlersUnlockAttack?.Invoke(context, combatChar, weaponIndex);
	}

	public static void RegisterHandler_UnlockAttackEnd(OnUnlockAttackEnd handler)
	{
		_handlersUnlockAttackEnd = (OnUnlockAttackEnd)Delegate.Combine(_handlersUnlockAttackEnd, handler);
	}

	public static void UnRegisterHandler_UnlockAttackEnd(OnUnlockAttackEnd handler)
	{
		_handlersUnlockAttackEnd = (OnUnlockAttackEnd)Delegate.Remove(_handlersUnlockAttackEnd, handler);
	}

	public static void RaiseUnlockAttackEnd(DataContext context, CombatCharacter attacker)
	{
		_handlersUnlockAttackEnd?.Invoke(context, attacker);
	}

	public static void RegisterHandler_NormalAttackPrepareEnd(OnNormalAttackPrepareEnd handler)
	{
		_handlersNormalAttackPrepareEnd = (OnNormalAttackPrepareEnd)Delegate.Combine(_handlersNormalAttackPrepareEnd, handler);
	}

	public static void UnRegisterHandler_NormalAttackPrepareEnd(OnNormalAttackPrepareEnd handler)
	{
		_handlersNormalAttackPrepareEnd = (OnNormalAttackPrepareEnd)Delegate.Remove(_handlersNormalAttackPrepareEnd, handler);
	}

	public static void RaiseNormalAttackPrepareEnd(DataContext context, int charId, bool isAlly)
	{
		_handlersNormalAttackPrepareEnd?.Invoke(context, charId, isAlly);
	}

	public static void RegisterHandler_NormalAttackOutOfRange(OnNormalAttackOutOfRange handler)
	{
		_handlersNormalAttackOutOfRange = (OnNormalAttackOutOfRange)Delegate.Combine(_handlersNormalAttackOutOfRange, handler);
	}

	public static void UnRegisterHandler_NormalAttackOutOfRange(OnNormalAttackOutOfRange handler)
	{
		_handlersNormalAttackOutOfRange = (OnNormalAttackOutOfRange)Delegate.Remove(_handlersNormalAttackOutOfRange, handler);
	}

	public static void RaiseNormalAttackOutOfRange(DataContext context, int charId, bool isAlly)
	{
		_handlersNormalAttackOutOfRange?.Invoke(context, charId, isAlly);
	}

	public static void RegisterHandler_NormalAttackBegin(OnNormalAttackBegin handler)
	{
		_handlersNormalAttackBegin = (OnNormalAttackBegin)Delegate.Combine(_handlersNormalAttackBegin, handler);
	}

	public static void UnRegisterHandler_NormalAttackBegin(OnNormalAttackBegin handler)
	{
		_handlersNormalAttackBegin = (OnNormalAttackBegin)Delegate.Remove(_handlersNormalAttackBegin, handler);
	}

	public static void RaiseNormalAttackBegin(DataContext context, CombatCharacter attacker, CombatCharacter defender, sbyte trickType, int pursueIndex)
	{
		_handlersNormalAttackBegin?.Invoke(context, attacker, defender, trickType, pursueIndex);
	}

	public static void RegisterHandler_NormalAttackCalcHitEnd(OnNormalAttackCalcHitEnd handler)
	{
		_handlersNormalAttackCalcHitEnd = (OnNormalAttackCalcHitEnd)Delegate.Combine(_handlersNormalAttackCalcHitEnd, handler);
	}

	public static void UnRegisterHandler_NormalAttackCalcHitEnd(OnNormalAttackCalcHitEnd handler)
	{
		_handlersNormalAttackCalcHitEnd = (OnNormalAttackCalcHitEnd)Delegate.Remove(_handlersNormalAttackCalcHitEnd, handler);
	}

	public static void RaiseNormalAttackCalcHitEnd(DataContext context, CombatCharacter attacker, CombatCharacter defender, int pursueIndex, bool hit, bool isFightBack, bool isMind)
	{
		_handlersNormalAttackCalcHitEnd?.Invoke(context, attacker, defender, pursueIndex, hit, isFightBack, isMind);
	}

	public static void RegisterHandler_NormalAttackCalcCriticalEnd(OnNormalAttackCalcCriticalEnd handler)
	{
		_handlersNormalAttackCalcCriticalEnd = (OnNormalAttackCalcCriticalEnd)Delegate.Combine(_handlersNormalAttackCalcCriticalEnd, handler);
	}

	public static void UnRegisterHandler_NormalAttackCalcCriticalEnd(OnNormalAttackCalcCriticalEnd handler)
	{
		_handlersNormalAttackCalcCriticalEnd = (OnNormalAttackCalcCriticalEnd)Delegate.Remove(_handlersNormalAttackCalcCriticalEnd, handler);
	}

	public static void RaiseNormalAttackCalcCriticalEnd(DataContext context, CombatCharacter attacker, CombatCharacter defender, bool critical)
	{
		_handlersNormalAttackCalcCriticalEnd?.Invoke(context, attacker, defender, critical);
	}

	public static void RegisterHandler_NormalAttackEnd(OnNormalAttackEnd handler)
	{
		_handlersNormalAttackEnd = (OnNormalAttackEnd)Delegate.Combine(_handlersNormalAttackEnd, handler);
	}

	public static void UnRegisterHandler_NormalAttackEnd(OnNormalAttackEnd handler)
	{
		_handlersNormalAttackEnd = (OnNormalAttackEnd)Delegate.Remove(_handlersNormalAttackEnd, handler);
	}

	public static void RaiseNormalAttackEnd(DataContext context, CombatCharacter attacker, CombatCharacter defender, sbyte trickType, int pursueIndex, bool hit, bool isFightBack)
	{
		_handlersNormalAttackEnd?.Invoke(context, attacker, defender, trickType, pursueIndex, hit, isFightBack);
	}

	public static void RegisterHandler_NormalAttackAllEnd(OnNormalAttackAllEnd handler)
	{
		_handlersNormalAttackAllEnd = (OnNormalAttackAllEnd)Delegate.Combine(_handlersNormalAttackAllEnd, handler);
	}

	public static void UnRegisterHandler_NormalAttackAllEnd(OnNormalAttackAllEnd handler)
	{
		_handlersNormalAttackAllEnd = (OnNormalAttackAllEnd)Delegate.Remove(_handlersNormalAttackAllEnd, handler);
	}

	public static void RaiseNormalAttackAllEnd(DataContext context, CombatCharacter attacker, CombatCharacter defender)
	{
		_handlersNormalAttackAllEnd?.Invoke(context, attacker, defender);
	}

	public static void RegisterHandler_CastSkillUseExtraBreathOrStance(OnCastSkillUseExtraBreathOrStance handler)
	{
		_handlersCastSkillUseExtraBreathOrStance = (OnCastSkillUseExtraBreathOrStance)Delegate.Combine(_handlersCastSkillUseExtraBreathOrStance, handler);
	}

	public static void UnRegisterHandler_CastSkillUseExtraBreathOrStance(OnCastSkillUseExtraBreathOrStance handler)
	{
		_handlersCastSkillUseExtraBreathOrStance = (OnCastSkillUseExtraBreathOrStance)Delegate.Remove(_handlersCastSkillUseExtraBreathOrStance, handler);
	}

	public static void RaiseCastSkillUseExtraBreathOrStance(DataContext context, int charId, short skillId, int extraBreath, int extraStance)
	{
		_handlersCastSkillUseExtraBreathOrStance?.Invoke(context, charId, skillId, extraBreath, extraStance);
	}

	public static void RegisterHandler_CastSkillUseMobilityAsBreathOrStance(OnCastSkillUseMobilityAsBreathOrStance handler)
	{
		_handlersCastSkillUseMobilityAsBreathOrStance = (OnCastSkillUseMobilityAsBreathOrStance)Delegate.Combine(_handlersCastSkillUseMobilityAsBreathOrStance, handler);
	}

	public static void UnRegisterHandler_CastSkillUseMobilityAsBreathOrStance(OnCastSkillUseMobilityAsBreathOrStance handler)
	{
		_handlersCastSkillUseMobilityAsBreathOrStance = (OnCastSkillUseMobilityAsBreathOrStance)Delegate.Remove(_handlersCastSkillUseMobilityAsBreathOrStance, handler);
	}

	public static void RaiseCastSkillUseMobilityAsBreathOrStance(DataContext context, int charId, short skillId, bool asBreath)
	{
		_handlersCastSkillUseMobilityAsBreathOrStance?.Invoke(context, charId, skillId, asBreath);
	}

	public static void RegisterHandler_CastLegSkillWithAgile(OnCastLegSkillWithAgile handler)
	{
		_handlersCastLegSkillWithAgile = (OnCastLegSkillWithAgile)Delegate.Combine(_handlersCastLegSkillWithAgile, handler);
	}

	public static void UnRegisterHandler_CastLegSkillWithAgile(OnCastLegSkillWithAgile handler)
	{
		_handlersCastLegSkillWithAgile = (OnCastLegSkillWithAgile)Delegate.Remove(_handlersCastLegSkillWithAgile, handler);
	}

	public static void RaiseCastLegSkillWithAgile(DataContext context, CombatCharacter combatChar, short legSkillId)
	{
		_handlersCastLegSkillWithAgile?.Invoke(context, combatChar, legSkillId);
	}

	public static void RegisterHandler_CastSkillOnLackBreathStance(OnCastSkillOnLackBreathStance handler)
	{
		_handlersCastSkillOnLackBreathStance = (OnCastSkillOnLackBreathStance)Delegate.Combine(_handlersCastSkillOnLackBreathStance, handler);
	}

	public static void UnRegisterHandler_CastSkillOnLackBreathStance(OnCastSkillOnLackBreathStance handler)
	{
		_handlersCastSkillOnLackBreathStance = (OnCastSkillOnLackBreathStance)Delegate.Remove(_handlersCastSkillOnLackBreathStance, handler);
	}

	public static void RaiseCastSkillOnLackBreathStance(DataContext context, CombatCharacter combatChar, short skillId, int lackBreath, int lackStance, int costBreath, int costStance)
	{
		_handlersCastSkillOnLackBreathStance?.Invoke(context, combatChar, skillId, lackBreath, lackStance, costBreath, costStance);
	}

	public static void RegisterHandler_CastSkillTrickCosted(OnCastSkillTrickCosted handler)
	{
		_handlersCastSkillTrickCosted = (OnCastSkillTrickCosted)Delegate.Combine(_handlersCastSkillTrickCosted, handler);
	}

	public static void UnRegisterHandler_CastSkillTrickCosted(OnCastSkillTrickCosted handler)
	{
		_handlersCastSkillTrickCosted = (OnCastSkillTrickCosted)Delegate.Remove(_handlersCastSkillTrickCosted, handler);
	}

	public static void RaiseCastSkillTrickCosted(DataContext context, CombatCharacter combatChar, short skillId, List<NeedTrick> costTricks)
	{
		_handlersCastSkillTrickCosted?.Invoke(context, combatChar, skillId, costTricks);
	}

	public static void RegisterHandler_JiTrickInsteadCostTricks(OnJiTrickInsteadCostTricks handler)
	{
		_handlersJiTrickInsteadCostTricks = (OnJiTrickInsteadCostTricks)Delegate.Combine(_handlersJiTrickInsteadCostTricks, handler);
	}

	public static void UnRegisterHandler_JiTrickInsteadCostTricks(OnJiTrickInsteadCostTricks handler)
	{
		_handlersJiTrickInsteadCostTricks = (OnJiTrickInsteadCostTricks)Delegate.Remove(_handlersJiTrickInsteadCostTricks, handler);
	}

	public static void RaiseJiTrickInsteadCostTricks(DataContext context, CombatCharacter character, int count)
	{
		_handlersJiTrickInsteadCostTricks?.Invoke(context, character, count);
	}

	public static void RegisterHandler_UselessTrickInsteadJiTricks(OnUselessTrickInsteadJiTricks handler)
	{
		_handlersUselessTrickInsteadJiTricks = (OnUselessTrickInsteadJiTricks)Delegate.Combine(_handlersUselessTrickInsteadJiTricks, handler);
	}

	public static void UnRegisterHandler_UselessTrickInsteadJiTricks(OnUselessTrickInsteadJiTricks handler)
	{
		_handlersUselessTrickInsteadJiTricks = (OnUselessTrickInsteadJiTricks)Delegate.Remove(_handlersUselessTrickInsteadJiTricks, handler);
	}

	public static void RaiseUselessTrickInsteadJiTricks(DataContext context, CombatCharacter character, int count)
	{
		_handlersUselessTrickInsteadJiTricks?.Invoke(context, character, count);
	}

	public static void RegisterHandler_ShaTrickInsteadCostTricks(OnShaTrickInsteadCostTricks handler)
	{
		_handlersShaTrickInsteadCostTricks = (OnShaTrickInsteadCostTricks)Delegate.Combine(_handlersShaTrickInsteadCostTricks, handler);
	}

	public static void UnRegisterHandler_ShaTrickInsteadCostTricks(OnShaTrickInsteadCostTricks handler)
	{
		_handlersShaTrickInsteadCostTricks = (OnShaTrickInsteadCostTricks)Delegate.Remove(_handlersShaTrickInsteadCostTricks, handler);
	}

	public static void RaiseShaTrickInsteadCostTricks(DataContext context, CombatCharacter character, short skillId)
	{
		_handlersShaTrickInsteadCostTricks?.Invoke(context, character, skillId);
	}

	public static void RegisterHandler_CastSkillCosted(OnCastSkillCosted handler)
	{
		_handlersCastSkillCosted = (OnCastSkillCosted)Delegate.Combine(_handlersCastSkillCosted, handler);
	}

	public static void UnRegisterHandler_CastSkillCosted(OnCastSkillCosted handler)
	{
		_handlersCastSkillCosted = (OnCastSkillCosted)Delegate.Remove(_handlersCastSkillCosted, handler);
	}

	public static void RaiseCastSkillCosted(DataContext context, CombatCharacter combatChar, short skillId)
	{
		_handlersCastSkillCosted?.Invoke(context, combatChar, skillId);
	}

	public static void RegisterHandler_ChangePreparingSkillBegin(OnChangePreparingSkillBegin handler)
	{
		_handlersChangePreparingSkillBegin = (OnChangePreparingSkillBegin)Delegate.Combine(_handlersChangePreparingSkillBegin, handler);
	}

	public static void UnRegisterHandler_ChangePreparingSkillBegin(OnChangePreparingSkillBegin handler)
	{
		_handlersChangePreparingSkillBegin = (OnChangePreparingSkillBegin)Delegate.Remove(_handlersChangePreparingSkillBegin, handler);
	}

	public static void RaiseChangePreparingSkillBegin(DataContext context, int charId, short prevSkillId, short currSkillId)
	{
		_handlersChangePreparingSkillBegin?.Invoke(context, charId, prevSkillId, currSkillId);
	}

	public static void RegisterHandler_CastAgileOrDefenseWithoutPrepareBegin(OnCastAgileOrDefenseWithoutPrepareBegin handler)
	{
		_handlersCastAgileOrDefenseWithoutPrepareBegin = (OnCastAgileOrDefenseWithoutPrepareBegin)Delegate.Combine(_handlersCastAgileOrDefenseWithoutPrepareBegin, handler);
	}

	public static void UnRegisterHandler_CastAgileOrDefenseWithoutPrepareBegin(OnCastAgileOrDefenseWithoutPrepareBegin handler)
	{
		_handlersCastAgileOrDefenseWithoutPrepareBegin = (OnCastAgileOrDefenseWithoutPrepareBegin)Delegate.Remove(_handlersCastAgileOrDefenseWithoutPrepareBegin, handler);
	}

	public static void RaiseCastAgileOrDefenseWithoutPrepareBegin(DataContext context, int charId, short skillId)
	{
		_handlersCastAgileOrDefenseWithoutPrepareBegin?.Invoke(context, charId, skillId);
	}

	public static void RegisterHandler_CastAgileOrDefenseWithoutPrepareEnd(OnCastAgileOrDefenseWithoutPrepareEnd handler)
	{
		_handlersCastAgileOrDefenseWithoutPrepareEnd = (OnCastAgileOrDefenseWithoutPrepareEnd)Delegate.Combine(_handlersCastAgileOrDefenseWithoutPrepareEnd, handler);
	}

	public static void UnRegisterHandler_CastAgileOrDefenseWithoutPrepareEnd(OnCastAgileOrDefenseWithoutPrepareEnd handler)
	{
		_handlersCastAgileOrDefenseWithoutPrepareEnd = (OnCastAgileOrDefenseWithoutPrepareEnd)Delegate.Remove(_handlersCastAgileOrDefenseWithoutPrepareEnd, handler);
	}

	public static void RaiseCastAgileOrDefenseWithoutPrepareEnd(DataContext context, int charId, short skillId)
	{
		_handlersCastAgileOrDefenseWithoutPrepareEnd?.Invoke(context, charId, skillId);
	}

	public static void RegisterHandler_PrepareSkillEffectNotYetCreated(OnPrepareSkillEffectNotYetCreated handler)
	{
		_handlersPrepareSkillEffectNotYetCreated = (OnPrepareSkillEffectNotYetCreated)Delegate.Combine(_handlersPrepareSkillEffectNotYetCreated, handler);
	}

	public static void UnRegisterHandler_PrepareSkillEffectNotYetCreated(OnPrepareSkillEffectNotYetCreated handler)
	{
		_handlersPrepareSkillEffectNotYetCreated = (OnPrepareSkillEffectNotYetCreated)Delegate.Remove(_handlersPrepareSkillEffectNotYetCreated, handler);
	}

	public static void RaisePrepareSkillEffectNotYetCreated(DataContext context, CombatCharacter character, short skillId)
	{
		_handlersPrepareSkillEffectNotYetCreated?.Invoke(context, character, skillId);
	}

	public static void RegisterHandler_PrepareSkillBegin(OnPrepareSkillBegin handler)
	{
		_handlersPrepareSkillBegin = (OnPrepareSkillBegin)Delegate.Combine(_handlersPrepareSkillBegin, handler);
	}

	public static void UnRegisterHandler_PrepareSkillBegin(OnPrepareSkillBegin handler)
	{
		_handlersPrepareSkillBegin = (OnPrepareSkillBegin)Delegate.Remove(_handlersPrepareSkillBegin, handler);
	}

	public static void RaisePrepareSkillBegin(DataContext context, int charId, bool isAlly, short skillId)
	{
		_handlersPrepareSkillBegin?.Invoke(context, charId, isAlly, skillId);
	}

	public static void RegisterHandler_PrepareSkillProgressChange(OnPrepareSkillProgressChange handler)
	{
		_handlersPrepareSkillProgressChange = (OnPrepareSkillProgressChange)Delegate.Combine(_handlersPrepareSkillProgressChange, handler);
	}

	public static void UnRegisterHandler_PrepareSkillProgressChange(OnPrepareSkillProgressChange handler)
	{
		_handlersPrepareSkillProgressChange = (OnPrepareSkillProgressChange)Delegate.Remove(_handlersPrepareSkillProgressChange, handler);
	}

	public static void RaisePrepareSkillProgressChange(DataContext context, int charId, bool isAlly, short skillId, sbyte preparePercent)
	{
		_handlersPrepareSkillProgressChange?.Invoke(context, charId, isAlly, skillId, preparePercent);
	}

	public static void RegisterHandler_PrepareSkillChangeDistance(OnPrepareSkillChangeDistance handler)
	{
		_handlersPrepareSkillChangeDistance = (OnPrepareSkillChangeDistance)Delegate.Combine(_handlersPrepareSkillChangeDistance, handler);
	}

	public static void UnRegisterHandler_PrepareSkillChangeDistance(OnPrepareSkillChangeDistance handler)
	{
		_handlersPrepareSkillChangeDistance = (OnPrepareSkillChangeDistance)Delegate.Remove(_handlersPrepareSkillChangeDistance, handler);
	}

	public static void RaisePrepareSkillChangeDistance(DataContext context, CombatCharacter attacker, CombatCharacter defender, short skillId)
	{
		_handlersPrepareSkillChangeDistance?.Invoke(context, attacker, defender, skillId);
	}

	public static void RegisterHandler_PrepareSkillEnd(OnPrepareSkillEnd handler)
	{
		_handlersPrepareSkillEnd = (OnPrepareSkillEnd)Delegate.Combine(_handlersPrepareSkillEnd, handler);
	}

	public static void UnRegisterHandler_PrepareSkillEnd(OnPrepareSkillEnd handler)
	{
		_handlersPrepareSkillEnd = (OnPrepareSkillEnd)Delegate.Remove(_handlersPrepareSkillEnd, handler);
	}

	public static void RaisePrepareSkillEnd(DataContext context, int charId, bool isAlly, short skillId)
	{
		_handlersPrepareSkillEnd?.Invoke(context, charId, isAlly, skillId);
	}

	public static void RegisterHandler_CastAttackSkillBegin(OnCastAttackSkillBegin handler)
	{
		_handlersCastAttackSkillBegin = (OnCastAttackSkillBegin)Delegate.Combine(_handlersCastAttackSkillBegin, handler);
	}

	public static void UnRegisterHandler_CastAttackSkillBegin(OnCastAttackSkillBegin handler)
	{
		_handlersCastAttackSkillBegin = (OnCastAttackSkillBegin)Delegate.Remove(_handlersCastAttackSkillBegin, handler);
	}

	public static void RaiseCastAttackSkillBegin(DataContext context, CombatCharacter attacker, CombatCharacter defender, short skillId)
	{
		_handlersCastAttackSkillBegin?.Invoke(context, attacker, defender, skillId);
	}

	public static void RegisterHandler_AttackSkillAttackBegin(OnAttackSkillAttackBegin handler)
	{
		_handlersAttackSkillAttackBegin = (OnAttackSkillAttackBegin)Delegate.Combine(_handlersAttackSkillAttackBegin, handler);
	}

	public static void UnRegisterHandler_AttackSkillAttackBegin(OnAttackSkillAttackBegin handler)
	{
		_handlersAttackSkillAttackBegin = (OnAttackSkillAttackBegin)Delegate.Remove(_handlersAttackSkillAttackBegin, handler);
	}

	public static void RaiseAttackSkillAttackBegin(DataContext context, CombatCharacter attacker, CombatCharacter defender, short skillId, int index, bool hit)
	{
		_handlersAttackSkillAttackBegin?.Invoke(context, attacker, defender, skillId, index, hit);
	}

	public static void RegisterHandler_AttackSkillAttackHit(OnAttackSkillAttackHit handler)
	{
		_handlersAttackSkillAttackHit = (OnAttackSkillAttackHit)Delegate.Combine(_handlersAttackSkillAttackHit, handler);
	}

	public static void UnRegisterHandler_AttackSkillAttackHit(OnAttackSkillAttackHit handler)
	{
		_handlersAttackSkillAttackHit = (OnAttackSkillAttackHit)Delegate.Remove(_handlersAttackSkillAttackHit, handler);
	}

	public static void RaiseAttackSkillAttackHit(DataContext context, CombatCharacter attacker, CombatCharacter defender, short skillId, int index, bool critical)
	{
		_handlersAttackSkillAttackHit?.Invoke(context, attacker, defender, skillId, index, critical);
	}

	public static void RegisterHandler_AttackSkillAttackEnd(OnAttackSkillAttackEnd handler)
	{
		_handlersAttackSkillAttackEnd = (OnAttackSkillAttackEnd)Delegate.Combine(_handlersAttackSkillAttackEnd, handler);
	}

	public static void UnRegisterHandler_AttackSkillAttackEnd(OnAttackSkillAttackEnd handler)
	{
		_handlersAttackSkillAttackEnd = (OnAttackSkillAttackEnd)Delegate.Remove(_handlersAttackSkillAttackEnd, handler);
	}

	public static void RaiseAttackSkillAttackEnd(CombatContext context, sbyte hitType, bool hit, int index)
	{
		_handlersAttackSkillAttackEnd?.Invoke(context, hitType, hit, index);
	}

	public static void RegisterHandler_AttackSkillAttackEndOfAll(OnAttackSkillAttackEndOfAll handler)
	{
		_handlersAttackSkillAttackEndOfAll = (OnAttackSkillAttackEndOfAll)Delegate.Combine(_handlersAttackSkillAttackEndOfAll, handler);
	}

	public static void UnRegisterHandler_AttackSkillAttackEndOfAll(OnAttackSkillAttackEndOfAll handler)
	{
		_handlersAttackSkillAttackEndOfAll = (OnAttackSkillAttackEndOfAll)Delegate.Remove(_handlersAttackSkillAttackEndOfAll, handler);
	}

	public static void RaiseAttackSkillAttackEndOfAll(DataContext context, CombatCharacter character, int index)
	{
		_handlersAttackSkillAttackEndOfAll?.Invoke(context, character, index);
	}

	public static void RegisterHandler_CastSkillEnd(OnCastSkillEnd handler)
	{
		_handlersCastSkillEnd = (OnCastSkillEnd)Delegate.Combine(_handlersCastSkillEnd, handler);
	}

	public static void UnRegisterHandler_CastSkillEnd(OnCastSkillEnd handler)
	{
		_handlersCastSkillEnd = (OnCastSkillEnd)Delegate.Remove(_handlersCastSkillEnd, handler);
	}

	public static void RaiseCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
	{
		_handlersCastSkillEnd?.Invoke(context, charId, isAlly, skillId, power, interrupted);
	}

	public static void RegisterHandler_CastSkillAllEnd(OnCastSkillAllEnd handler)
	{
		_handlersCastSkillAllEnd = (OnCastSkillAllEnd)Delegate.Combine(_handlersCastSkillAllEnd, handler);
	}

	public static void UnRegisterHandler_CastSkillAllEnd(OnCastSkillAllEnd handler)
	{
		_handlersCastSkillAllEnd = (OnCastSkillAllEnd)Delegate.Remove(_handlersCastSkillAllEnd, handler);
	}

	public static void RaiseCastSkillAllEnd(DataContext context, int charId, short skillId)
	{
		_handlersCastSkillAllEnd?.Invoke(context, charId, skillId);
	}

	public static void RegisterHandler_CalcLeveragingValue(OnCalcLeveragingValue handler)
	{
		_handlersCalcLeveragingValue = (OnCalcLeveragingValue)Delegate.Combine(_handlersCalcLeveragingValue, handler);
	}

	public static void UnRegisterHandler_CalcLeveragingValue(OnCalcLeveragingValue handler)
	{
		_handlersCalcLeveragingValue = (OnCalcLeveragingValue)Delegate.Remove(_handlersCalcLeveragingValue, handler);
	}

	public static void RaiseCalcLeveragingValue(CombatContext context, sbyte hitType, bool hit, int index)
	{
		_handlersCalcLeveragingValue?.Invoke(context, hitType, hit, index);
	}

	public static void RegisterHandler_WisdomCosted(OnWisdomCosted handler)
	{
		_handlersWisdomCosted = (OnWisdomCosted)Delegate.Combine(_handlersWisdomCosted, handler);
	}

	public static void UnRegisterHandler_WisdomCosted(OnWisdomCosted handler)
	{
		_handlersWisdomCosted = (OnWisdomCosted)Delegate.Remove(_handlersWisdomCosted, handler);
	}

	public static void RaiseWisdomCosted(DataContext context, bool isAlly, int value)
	{
		_handlersWisdomCosted?.Invoke(context, isAlly, value);
	}

	public static void RegisterHandler_HealedInjury(OnHealedInjury handler)
	{
		_handlersHealedInjury = (OnHealedInjury)Delegate.Combine(_handlersHealedInjury, handler);
	}

	public static void UnRegisterHandler_HealedInjury(OnHealedInjury handler)
	{
		_handlersHealedInjury = (OnHealedInjury)Delegate.Remove(_handlersHealedInjury, handler);
	}

	public static void RaiseHealedInjury(DataContext context, int doctorId, int patientId, bool isAlly, sbyte healMarkCount)
	{
		_handlersHealedInjury?.Invoke(context, doctorId, patientId, isAlly, healMarkCount);
	}

	public static void RegisterHandler_HealedPoison(OnHealedPoison handler)
	{
		_handlersHealedPoison = (OnHealedPoison)Delegate.Combine(_handlersHealedPoison, handler);
	}

	public static void UnRegisterHandler_HealedPoison(OnHealedPoison handler)
	{
		_handlersHealedPoison = (OnHealedPoison)Delegate.Remove(_handlersHealedPoison, handler);
	}

	public static void RaiseHealedPoison(DataContext context, int doctorId, int patientId, bool isAlly, sbyte healMarkCount)
	{
		_handlersHealedPoison?.Invoke(context, doctorId, patientId, isAlly, healMarkCount);
	}

	public static void RegisterHandler_UsedMedicine(OnUsedMedicine handler)
	{
		_handlersUsedMedicine = (OnUsedMedicine)Delegate.Combine(_handlersUsedMedicine, handler);
	}

	public static void UnRegisterHandler_UsedMedicine(OnUsedMedicine handler)
	{
		_handlersUsedMedicine = (OnUsedMedicine)Delegate.Remove(_handlersUsedMedicine, handler);
	}

	public static void RaiseUsedMedicine(DataContext context, int charId, ItemKey itemKey)
	{
		_handlersUsedMedicine?.Invoke(context, charId, itemKey);
	}

	public static void RegisterHandler_UsedCustomItem(OnUsedCustomItem handler)
	{
		_handlersUsedCustomItem = (OnUsedCustomItem)Delegate.Combine(_handlersUsedCustomItem, handler);
	}

	public static void UnRegisterHandler_UsedCustomItem(OnUsedCustomItem handler)
	{
		_handlersUsedCustomItem = (OnUsedCustomItem)Delegate.Remove(_handlersUsedCustomItem, handler);
	}

	public static void RaiseUsedCustomItem(DataContext context, int charId, ItemKey itemKey)
	{
		_handlersUsedCustomItem?.Invoke(context, charId, itemKey);
	}

	public static void RegisterHandler_InterruptOtherAction(OnInterruptOtherAction handler)
	{
		_handlersInterruptOtherAction = (OnInterruptOtherAction)Delegate.Combine(_handlersInterruptOtherAction, handler);
	}

	public static void UnRegisterHandler_InterruptOtherAction(OnInterruptOtherAction handler)
	{
		_handlersInterruptOtherAction = (OnInterruptOtherAction)Delegate.Remove(_handlersInterruptOtherAction, handler);
	}

	public static void RaiseInterruptOtherAction(DataContext context, CombatCharacter combatChar, sbyte otherActionType)
	{
		_handlersInterruptOtherAction?.Invoke(context, combatChar, otherActionType);
	}

	public static void RegisterHandler_FlawAdded(OnFlawAdded handler)
	{
		_handlersFlawAdded = (OnFlawAdded)Delegate.Combine(_handlersFlawAdded, handler);
	}

	public static void UnRegisterHandler_FlawAdded(OnFlawAdded handler)
	{
		_handlersFlawAdded = (OnFlawAdded)Delegate.Remove(_handlersFlawAdded, handler);
	}

	public static void RaiseFlawAdded(DataContext context, CombatCharacter combatChar, sbyte bodyPart, sbyte level)
	{
		_handlersFlawAdded?.Invoke(context, combatChar, bodyPart, level);
	}

	public static void RegisterHandler_FlawRemoved(OnFlawRemoved handler)
	{
		_handlersFlawRemoved = (OnFlawRemoved)Delegate.Combine(_handlersFlawRemoved, handler);
	}

	public static void UnRegisterHandler_FlawRemoved(OnFlawRemoved handler)
	{
		_handlersFlawRemoved = (OnFlawRemoved)Delegate.Remove(_handlersFlawRemoved, handler);
	}

	public static void RaiseFlawRemoved(DataContext context, CombatCharacter combatChar, sbyte bodyPart, sbyte level)
	{
		_handlersFlawRemoved?.Invoke(context, combatChar, bodyPart, level);
	}

	public static void RegisterHandler_AcuPointAdded(OnAcuPointAdded handler)
	{
		_handlersAcuPointAdded = (OnAcuPointAdded)Delegate.Combine(_handlersAcuPointAdded, handler);
	}

	public static void UnRegisterHandler_AcuPointAdded(OnAcuPointAdded handler)
	{
		_handlersAcuPointAdded = (OnAcuPointAdded)Delegate.Remove(_handlersAcuPointAdded, handler);
	}

	public static void RaiseAcuPointAdded(DataContext context, CombatCharacter combatChar, sbyte bodyPart, sbyte level)
	{
		_handlersAcuPointAdded?.Invoke(context, combatChar, bodyPart, level);
	}

	public static void RegisterHandler_AcuPointRemoved(OnAcuPointRemoved handler)
	{
		_handlersAcuPointRemoved = (OnAcuPointRemoved)Delegate.Combine(_handlersAcuPointRemoved, handler);
	}

	public static void UnRegisterHandler_AcuPointRemoved(OnAcuPointRemoved handler)
	{
		_handlersAcuPointRemoved = (OnAcuPointRemoved)Delegate.Remove(_handlersAcuPointRemoved, handler);
	}

	public static void RaiseAcuPointRemoved(DataContext context, CombatCharacter combatChar, sbyte bodyPart, sbyte level)
	{
		_handlersAcuPointRemoved?.Invoke(context, combatChar, bodyPart, level);
	}

	public static void RegisterHandler_CombatCharChanged(OnCombatCharChanged handler)
	{
		_handlersCombatCharChanged = (OnCombatCharChanged)Delegate.Combine(_handlersCombatCharChanged, handler);
	}

	public static void UnRegisterHandler_CombatCharChanged(OnCombatCharChanged handler)
	{
		_handlersCombatCharChanged = (OnCombatCharChanged)Delegate.Remove(_handlersCombatCharChanged, handler);
	}

	public static void RaiseCombatCharChanged(DataContext context, bool isAlly)
	{
		_handlersCombatCharChanged?.Invoke(context, isAlly);
	}

	public static void RegisterHandler_AddInjury(OnAddInjury handler)
	{
		_handlersAddInjury = (OnAddInjury)Delegate.Combine(_handlersAddInjury, handler);
	}

	public static void UnRegisterHandler_AddInjury(OnAddInjury handler)
	{
		_handlersAddInjury = (OnAddInjury)Delegate.Remove(_handlersAddInjury, handler);
	}

	public static void RaiseAddInjury(DataContext context, CombatCharacter character, sbyte bodyPart, bool isInner, sbyte value, bool changeToOld)
	{
		_handlersAddInjury?.Invoke(context, character, bodyPart, isInner, value, changeToOld);
	}

	public static void RegisterHandler_AddDirectDamageValue(OnAddDirectDamageValue handler)
	{
		_handlersAddDirectDamageValue = (OnAddDirectDamageValue)Delegate.Combine(_handlersAddDirectDamageValue, handler);
	}

	public static void UnRegisterHandler_AddDirectDamageValue(OnAddDirectDamageValue handler)
	{
		_handlersAddDirectDamageValue = (OnAddDirectDamageValue)Delegate.Remove(_handlersAddDirectDamageValue, handler);
	}

	public static void RaiseAddDirectDamageValue(DataContext context, int attackerId, int defenderId, sbyte bodyPart, bool isInner, int damageValue, short combatSkillId)
	{
		_handlersAddDirectDamageValue?.Invoke(context, attackerId, defenderId, bodyPart, isInner, damageValue, combatSkillId);
	}

	public static void RegisterHandler_AddDirectInjury(OnAddDirectInjury handler)
	{
		_handlersAddDirectInjury = (OnAddDirectInjury)Delegate.Combine(_handlersAddDirectInjury, handler);
	}

	public static void UnRegisterHandler_AddDirectInjury(OnAddDirectInjury handler)
	{
		_handlersAddDirectInjury = (OnAddDirectInjury)Delegate.Remove(_handlersAddDirectInjury, handler);
	}

	public static void RaiseAddDirectInjury(DataContext context, int attackerId, int defenderId, bool isAlly, sbyte bodyPart, sbyte outerMarkCount, sbyte innerMarkCount, short combatSkillId)
	{
		_handlersAddDirectInjury?.Invoke(context, attackerId, defenderId, isAlly, bodyPart, outerMarkCount, innerMarkCount, combatSkillId);
	}

	public static void RegisterHandler_BounceInjury(OnBounceInjury handler)
	{
		_handlersBounceInjury = (OnBounceInjury)Delegate.Combine(_handlersBounceInjury, handler);
	}

	public static void UnRegisterHandler_BounceInjury(OnBounceInjury handler)
	{
		_handlersBounceInjury = (OnBounceInjury)Delegate.Remove(_handlersBounceInjury, handler);
	}

	public static void RaiseBounceInjury(DataContext context, int attackerId, int defenderId, bool isAlly, sbyte bodyPart, sbyte outerMarkCount, sbyte innerMarkCount)
	{
		_handlersBounceInjury?.Invoke(context, attackerId, defenderId, isAlly, bodyPart, outerMarkCount, innerMarkCount);
	}

	public static void RegisterHandler_AddMindMark(OnAddMindMark handler)
	{
		_handlersAddMindMark = (OnAddMindMark)Delegate.Combine(_handlersAddMindMark, handler);
	}

	public static void UnRegisterHandler_AddMindMark(OnAddMindMark handler)
	{
		_handlersAddMindMark = (OnAddMindMark)Delegate.Remove(_handlersAddMindMark, handler);
	}

	public static void RaiseAddMindMark(DataContext context, CombatCharacter character, int count)
	{
		_handlersAddMindMark?.Invoke(context, character, count);
	}

	public static void RegisterHandler_AddMindDamage(OnAddMindDamage handler)
	{
		_handlersAddMindDamage = (OnAddMindDamage)Delegate.Combine(_handlersAddMindDamage, handler);
	}

	public static void UnRegisterHandler_AddMindDamage(OnAddMindDamage handler)
	{
		_handlersAddMindDamage = (OnAddMindDamage)Delegate.Remove(_handlersAddMindDamage, handler);
	}

	public static void RaiseAddMindDamage(DataContext context, int attackerId, int defenderId, int damageValue, int markCount, short combatSkillId)
	{
		_handlersAddMindDamage?.Invoke(context, attackerId, defenderId, damageValue, markCount, combatSkillId);
	}

	public static void RegisterHandler_AddFatalDamageMark(OnAddFatalDamageMark handler)
	{
		_handlersAddFatalDamageMark = (OnAddFatalDamageMark)Delegate.Combine(_handlersAddFatalDamageMark, handler);
	}

	public static void UnRegisterHandler_AddFatalDamageMark(OnAddFatalDamageMark handler)
	{
		_handlersAddFatalDamageMark = (OnAddFatalDamageMark)Delegate.Remove(_handlersAddFatalDamageMark, handler);
	}

	public static void RaiseAddFatalDamageMark(DataContext context, CombatCharacter combatChar, int count)
	{
		_handlersAddFatalDamageMark?.Invoke(context, combatChar, count);
	}

	public static void RegisterHandler_AddDirectFatalDamageMark(OnAddDirectFatalDamageMark handler)
	{
		_handlersAddDirectFatalDamageMark = (OnAddDirectFatalDamageMark)Delegate.Combine(_handlersAddDirectFatalDamageMark, handler);
	}

	public static void UnRegisterHandler_AddDirectFatalDamageMark(OnAddDirectFatalDamageMark handler)
	{
		_handlersAddDirectFatalDamageMark = (OnAddDirectFatalDamageMark)Delegate.Remove(_handlersAddDirectFatalDamageMark, handler);
	}

	public static void RaiseAddDirectFatalDamageMark(DataContext context, int attackerId, int defenderId, bool isAlly, sbyte bodyPart, int outerMarkCount, int innerMarkCount, short combatSkillId)
	{
		_handlersAddDirectFatalDamageMark?.Invoke(context, attackerId, defenderId, isAlly, bodyPart, outerMarkCount, innerMarkCount, combatSkillId);
	}

	public static void RegisterHandler_AddDirectFatalDamage(OnAddDirectFatalDamage handler)
	{
		_handlersAddDirectFatalDamage = (OnAddDirectFatalDamage)Delegate.Combine(_handlersAddDirectFatalDamage, handler);
	}

	public static void UnRegisterHandler_AddDirectFatalDamage(OnAddDirectFatalDamage handler)
	{
		_handlersAddDirectFatalDamage = (OnAddDirectFatalDamage)Delegate.Remove(_handlersAddDirectFatalDamage, handler);
	}

	public static void RaiseAddDirectFatalDamage(CombatContext context, int outer, int inner)
	{
		_handlersAddDirectFatalDamage?.Invoke(context, outer, inner);
	}

	public static void RegisterHandler_AddDirectPoisonMark(OnAddDirectPoisonMark handler)
	{
		_handlersAddDirectPoisonMark = (OnAddDirectPoisonMark)Delegate.Combine(_handlersAddDirectPoisonMark, handler);
	}

	public static void UnRegisterHandler_AddDirectPoisonMark(OnAddDirectPoisonMark handler)
	{
		_handlersAddDirectPoisonMark = (OnAddDirectPoisonMark)Delegate.Remove(_handlersAddDirectPoisonMark, handler);
	}

	public static void RaiseAddDirectPoisonMark(DataContext context, CombatCharacter attacker, CombatCharacter defender, sbyte poisonType, short skillId, int markCount)
	{
		_handlersAddDirectPoisonMark?.Invoke(context, attacker, defender, poisonType, skillId, markCount);
	}

	public static void RegisterHandler_MoveStateChanged(OnMoveStateChanged handler)
	{
		_handlersMoveStateChanged = (OnMoveStateChanged)Delegate.Combine(_handlersMoveStateChanged, handler);
	}

	public static void UnRegisterHandler_MoveStateChanged(OnMoveStateChanged handler)
	{
		_handlersMoveStateChanged = (OnMoveStateChanged)Delegate.Remove(_handlersMoveStateChanged, handler);
	}

	public static void RaiseMoveStateChanged(DataContext context, CombatCharacter character, byte moveState)
	{
		_handlersMoveStateChanged?.Invoke(context, character, moveState);
	}

	public static void RegisterHandler_MoveBegin(OnMoveBegin handler)
	{
		_handlersMoveBegin = (OnMoveBegin)Delegate.Combine(_handlersMoveBegin, handler);
	}

	public static void UnRegisterHandler_MoveBegin(OnMoveBegin handler)
	{
		_handlersMoveBegin = (OnMoveBegin)Delegate.Remove(_handlersMoveBegin, handler);
	}

	public static void RaiseMoveBegin(DataContext context, CombatCharacter mover, int distance, bool isJump)
	{
		_handlersMoveBegin?.Invoke(context, mover, distance, isJump);
	}

	public static void RegisterHandler_MoveEnd(OnMoveEnd handler)
	{
		_handlersMoveEnd = (OnMoveEnd)Delegate.Combine(_handlersMoveEnd, handler);
	}

	public static void UnRegisterHandler_MoveEnd(OnMoveEnd handler)
	{
		_handlersMoveEnd = (OnMoveEnd)Delegate.Remove(_handlersMoveEnd, handler);
	}

	public static void RaiseMoveEnd(DataContext context, CombatCharacter mover, int distance, bool isJump)
	{
		_handlersMoveEnd?.Invoke(context, mover, distance, isJump);
	}

	public static void RegisterHandler_IgnoredForceChangeDistance(OnIgnoredForceChangeDistance handler)
	{
		_handlersIgnoredForceChangeDistance = (OnIgnoredForceChangeDistance)Delegate.Combine(_handlersIgnoredForceChangeDistance, handler);
	}

	public static void UnRegisterHandler_IgnoredForceChangeDistance(OnIgnoredForceChangeDistance handler)
	{
		_handlersIgnoredForceChangeDistance = (OnIgnoredForceChangeDistance)Delegate.Remove(_handlersIgnoredForceChangeDistance, handler);
	}

	public static void RaiseIgnoredForceChangeDistance(DataContext context, CombatCharacter mover, int distance)
	{
		_handlersIgnoredForceChangeDistance?.Invoke(context, mover, distance);
	}

	public static void RegisterHandler_DistanceChanged(OnDistanceChanged handler)
	{
		_handlersDistanceChanged = (OnDistanceChanged)Delegate.Combine(_handlersDistanceChanged, handler);
	}

	public static void UnRegisterHandler_DistanceChanged(OnDistanceChanged handler)
	{
		_handlersDistanceChanged = (OnDistanceChanged)Delegate.Remove(_handlersDistanceChanged, handler);
	}

	public static void RaiseDistanceChanged(DataContext context, CombatCharacter mover, short distance, bool isMove, bool isForced)
	{
		_handlersDistanceChanged?.Invoke(context, mover, distance, isMove, isForced);
	}

	public static void RegisterHandler_SkillEffectChange(OnSkillEffectChange handler)
	{
		_handlersSkillEffectChange = (OnSkillEffectChange)Delegate.Combine(_handlersSkillEffectChange, handler);
	}

	public static void UnRegisterHandler_SkillEffectChange(OnSkillEffectChange handler)
	{
		_handlersSkillEffectChange = (OnSkillEffectChange)Delegate.Remove(_handlersSkillEffectChange, handler);
	}

	public static void RaiseSkillEffectChange(DataContext context, int charId, SkillEffectKey key, short oldCount, short newCount, bool removed)
	{
		_handlersSkillEffectChange?.Invoke(context, charId, key, oldCount, newCount, removed);
	}

	public static void RegisterHandler_SkillSilence(OnSkillSilence handler)
	{
		_handlersSkillSilence = (OnSkillSilence)Delegate.Combine(_handlersSkillSilence, handler);
	}

	public static void UnRegisterHandler_SkillSilence(OnSkillSilence handler)
	{
		_handlersSkillSilence = (OnSkillSilence)Delegate.Remove(_handlersSkillSilence, handler);
	}

	public static void RaiseSkillSilence(DataContext context, CombatSkillKey skillKey)
	{
		_handlersSkillSilence?.Invoke(context, skillKey);
	}

	public static void RegisterHandler_SkillSilenceEnd(OnSkillSilenceEnd handler)
	{
		_handlersSkillSilenceEnd = (OnSkillSilenceEnd)Delegate.Combine(_handlersSkillSilenceEnd, handler);
	}

	public static void UnRegisterHandler_SkillSilenceEnd(OnSkillSilenceEnd handler)
	{
		_handlersSkillSilenceEnd = (OnSkillSilenceEnd)Delegate.Remove(_handlersSkillSilenceEnd, handler);
	}

	public static void RaiseSkillSilenceEnd(DataContext context, CombatSkillKey skillKey)
	{
		_handlersSkillSilenceEnd?.Invoke(context, skillKey);
	}

	public static void RegisterHandler_NeiliAllocationChanged(OnNeiliAllocationChanged handler)
	{
		_handlersNeiliAllocationChanged = (OnNeiliAllocationChanged)Delegate.Combine(_handlersNeiliAllocationChanged, handler);
	}

	public static void UnRegisterHandler_NeiliAllocationChanged(OnNeiliAllocationChanged handler)
	{
		_handlersNeiliAllocationChanged = (OnNeiliAllocationChanged)Delegate.Remove(_handlersNeiliAllocationChanged, handler);
	}

	public static void RaiseNeiliAllocationChanged(DataContext context, int charId, byte type, int changeValue)
	{
		_handlersNeiliAllocationChanged?.Invoke(context, charId, type, changeValue);
	}

	public static void RegisterHandler_AddPoison(OnAddPoison handler)
	{
		_handlersAddPoison = (OnAddPoison)Delegate.Combine(_handlersAddPoison, handler);
	}

	public static void UnRegisterHandler_AddPoison(OnAddPoison handler)
	{
		_handlersAddPoison = (OnAddPoison)Delegate.Remove(_handlersAddPoison, handler);
	}

	public static void RaiseAddPoison(DataContext context, int attackerId, int defenderId, sbyte poisonType, sbyte level, int addValue, short skillId, bool canBounce)
	{
		_handlersAddPoison?.Invoke(context, attackerId, defenderId, poisonType, level, addValue, skillId, canBounce);
	}

	public static void RegisterHandler_PoisonAffected(OnPoisonAffected handler)
	{
		_handlersPoisonAffected = (OnPoisonAffected)Delegate.Combine(_handlersPoisonAffected, handler);
	}

	public static void UnRegisterHandler_PoisonAffected(OnPoisonAffected handler)
	{
		_handlersPoisonAffected = (OnPoisonAffected)Delegate.Remove(_handlersPoisonAffected, handler);
	}

	public static void RaisePoisonAffected(DataContext context, int charId, sbyte poisonType)
	{
		_handlersPoisonAffected?.Invoke(context, charId, poisonType);
	}

	public static void RegisterHandler_AddWug(OnAddWug handler)
	{
		_handlersAddWug = (OnAddWug)Delegate.Combine(_handlersAddWug, handler);
	}

	public static void UnRegisterHandler_AddWug(OnAddWug handler)
	{
		_handlersAddWug = (OnAddWug)Delegate.Remove(_handlersAddWug, handler);
	}

	public static void RaiseAddWug(DataContext context, int charId, short wugTemplateId, short replacedWug)
	{
		_handlersAddWug?.Invoke(context, charId, wugTemplateId, replacedWug);
	}

	public static void RegisterHandler_RemoveWug(OnRemoveWug handler)
	{
		_handlersRemoveWug = (OnRemoveWug)Delegate.Combine(_handlersRemoveWug, handler);
	}

	public static void UnRegisterHandler_RemoveWug(OnRemoveWug handler)
	{
		_handlersRemoveWug = (OnRemoveWug)Delegate.Remove(_handlersRemoveWug, handler);
	}

	public static void RaiseRemoveWug(DataContext context, int charId, short wugTemplateId)
	{
		_handlersRemoveWug?.Invoke(context, charId, wugTemplateId);
	}

	public static void RegisterHandler_CompareDataCalcFinished(OnCompareDataCalcFinished handler)
	{
		_handlersCompareDataCalcFinished = (OnCompareDataCalcFinished)Delegate.Combine(_handlersCompareDataCalcFinished, handler);
	}

	public static void UnRegisterHandler_CompareDataCalcFinished(OnCompareDataCalcFinished handler)
	{
		_handlersCompareDataCalcFinished = (OnCompareDataCalcFinished)Delegate.Remove(_handlersCompareDataCalcFinished, handler);
	}

	public static void RaiseCompareDataCalcFinished(CombatContext context, DamageCompareData compareData)
	{
		_handlersCompareDataCalcFinished?.Invoke(context, compareData);
	}

	public static void RegisterHandler_CombatStateMachineUpdateEnd(OnCombatStateMachineUpdateEnd handler)
	{
		_handlersCombatStateMachineUpdateEnd = (OnCombatStateMachineUpdateEnd)Delegate.Combine(_handlersCombatStateMachineUpdateEnd, handler);
	}

	public static void UnRegisterHandler_CombatStateMachineUpdateEnd(OnCombatStateMachineUpdateEnd handler)
	{
		_handlersCombatStateMachineUpdateEnd = (OnCombatStateMachineUpdateEnd)Delegate.Remove(_handlersCombatStateMachineUpdateEnd, handler);
	}

	public static void RaiseCombatStateMachineUpdateEnd(DataContext context, CombatCharacter combatChar)
	{
		_handlersCombatStateMachineUpdateEnd?.Invoke(context, combatChar);
	}

	public static void RegisterHandler_CombatCharFallen(OnCombatCharFallen handler)
	{
		_handlersCombatCharFallen = (OnCombatCharFallen)Delegate.Combine(_handlersCombatCharFallen, handler);
	}

	public static void UnRegisterHandler_CombatCharFallen(OnCombatCharFallen handler)
	{
		_handlersCombatCharFallen = (OnCombatCharFallen)Delegate.Remove(_handlersCombatCharFallen, handler);
	}

	public static void RaiseCombatCharFallen(DataContext context, CombatCharacter combatChar)
	{
		_handlersCombatCharFallen?.Invoke(context, combatChar);
	}

	public static void RegisterHandler_CombatCostNeiliConfirm(OnCombatCostNeiliConfirm handler)
	{
		_handlersCombatCostNeiliConfirm = (OnCombatCostNeiliConfirm)Delegate.Combine(_handlersCombatCostNeiliConfirm, handler);
	}

	public static void UnRegisterHandler_CombatCostNeiliConfirm(OnCombatCostNeiliConfirm handler)
	{
		_handlersCombatCostNeiliConfirm = (OnCombatCostNeiliConfirm)Delegate.Remove(_handlersCombatCostNeiliConfirm, handler);
	}

	public static void RaiseCombatCostNeiliConfirm(DataContext context, int charId, short skillId, short effectId)
	{
		_handlersCombatCostNeiliConfirm?.Invoke(context, charId, skillId, effectId);
	}

	public static void RegisterHandler_CostTrickDuringPreparingSkill(OnCostTrickDuringPreparingSkill handler)
	{
		_handlersCostTrickDuringPreparingSkill = (OnCostTrickDuringPreparingSkill)Delegate.Combine(_handlersCostTrickDuringPreparingSkill, handler);
	}

	public static void UnRegisterHandler_CostTrickDuringPreparingSkill(OnCostTrickDuringPreparingSkill handler)
	{
		_handlersCostTrickDuringPreparingSkill = (OnCostTrickDuringPreparingSkill)Delegate.Remove(_handlersCostTrickDuringPreparingSkill, handler);
	}

	public static void RaiseCostTrickDuringPreparingSkill(DataContext context, int charId)
	{
		_handlersCostTrickDuringPreparingSkill?.Invoke(context, charId);
	}

	public static void RegisterHandler_CombatChangeDurability(OnCombatChangeDurability handler)
	{
		_handlersCombatChangeDurability = (OnCombatChangeDurability)Delegate.Combine(_handlersCombatChangeDurability, handler);
	}

	public static void UnRegisterHandler_CombatChangeDurability(OnCombatChangeDurability handler)
	{
		_handlersCombatChangeDurability = (OnCombatChangeDurability)Delegate.Remove(_handlersCombatChangeDurability, handler);
	}

	public static void RaiseCombatChangeDurability(DataContext context, CombatCharacter character, ItemKey itemKey, int delta)
	{
		_handlersCombatChangeDurability?.Invoke(context, character, itemKey, delta);
	}

	public static void RegisterHandler_PassingLegacyWhileAdvancingMonth(OnPassingLegacyWhileAdvancingMonth handler)
	{
		_handlersPassingLegacyWhileAdvancingMonth = (OnPassingLegacyWhileAdvancingMonth)Delegate.Combine(_handlersPassingLegacyWhileAdvancingMonth, handler);
	}

	public static void UnRegisterHandler_PassingLegacyWhileAdvancingMonth(OnPassingLegacyWhileAdvancingMonth handler)
	{
		_handlersPassingLegacyWhileAdvancingMonth = (OnPassingLegacyWhileAdvancingMonth)Delegate.Remove(_handlersPassingLegacyWhileAdvancingMonth, handler);
	}

	public static void RaisePassingLegacyWhileAdvancingMonth(DataContext context)
	{
		_handlersPassingLegacyWhileAdvancingMonth?.Invoke(context);
	}

	public static void RegisterHandler_AdvanceMonthBegin(OnAdvanceMonthBegin handler)
	{
		_handlersAdvanceMonthBegin = (OnAdvanceMonthBegin)Delegate.Combine(_handlersAdvanceMonthBegin, handler);
	}

	public static void UnRegisterHandler_AdvanceMonthBegin(OnAdvanceMonthBegin handler)
	{
		_handlersAdvanceMonthBegin = (OnAdvanceMonthBegin)Delegate.Remove(_handlersAdvanceMonthBegin, handler);
	}

	public static void RaiseAdvanceMonthBegin(DataContext context)
	{
		_handlersAdvanceMonthBegin?.Invoke(context);
	}

	public static void RegisterHandler_PostAdvanceMonthBegin(OnPostAdvanceMonthBegin handler)
	{
		_handlersPostAdvanceMonthBegin = (OnPostAdvanceMonthBegin)Delegate.Combine(_handlersPostAdvanceMonthBegin, handler);
	}

	public static void UnRegisterHandler_PostAdvanceMonthBegin(OnPostAdvanceMonthBegin handler)
	{
		_handlersPostAdvanceMonthBegin = (OnPostAdvanceMonthBegin)Delegate.Remove(_handlersPostAdvanceMonthBegin, handler);
	}

	public static void RaisePostAdvanceMonthBegin(DataContext context)
	{
		_handlersPostAdvanceMonthBegin?.Invoke(context);
	}

	public static void RegisterHandler_AdvanceMonthFinish(OnAdvanceMonthFinish handler)
	{
		_handlersAdvanceMonthFinish = (OnAdvanceMonthFinish)Delegate.Combine(_handlersAdvanceMonthFinish, handler);
	}

	public static void UnRegisterHandler_AdvanceMonthFinish(OnAdvanceMonthFinish handler)
	{
		_handlersAdvanceMonthFinish = (OnAdvanceMonthFinish)Delegate.Remove(_handlersAdvanceMonthFinish, handler);
	}

	public static void RaiseAdvanceMonthFinish(DataContext context)
	{
		_handlersAdvanceMonthFinish?.Invoke(context);
	}

	public static void RegisterHandler_TaiwuMove(OnTaiwuMove handler)
	{
		_handlersTaiwuMove = (OnTaiwuMove)Delegate.Combine(_handlersTaiwuMove, handler);
	}

	public static void UnRegisterHandler_TaiwuMove(OnTaiwuMove handler)
	{
		_handlersTaiwuMove = (OnTaiwuMove)Delegate.Remove(_handlersTaiwuMove, handler);
	}

	public static void RaiseTaiwuMove(DataContext context, MapBlockData fromBlock, MapBlockData toBlock, int actionPointCost)
	{
		_handlersTaiwuMove?.Invoke(context, fromBlock, toBlock, actionPointCost);
	}
}
