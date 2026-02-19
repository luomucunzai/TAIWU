using Config.EventConfig;
using GameData.Domains.Adventure;
using GameData.Domains.Building;
using GameData.Domains.Extra;
using GameData.Domains.Item;
using GameData.Domains.Item.Display;
using GameData.Domains.Map;

namespace GameData.Domains.TaiwuEvent.EventManager;

public abstract class EventManagerBase
{
	public abstract TaiwuEvent GetEvent(string eventGuid);

	public abstract void HandleEventPackage(EventPackage package);

	public abstract void ClearExtendOptions();

	public abstract void Reset();

	public abstract void UnloadPackage(EventPackage package);

	public virtual void OnEventTrigger_TaiwuBlockChanged(Location arg0, Location arg1)
	{
	}

	public virtual void OnEventTrigger_CharacterClicked(int arg0)
	{
	}

	public virtual void OnEventTrigger_AdventureReachStartNode(short arg0)
	{
	}

	public virtual void OnEventTrigger_AdventureReachTransferNode(short arg0)
	{
	}

	public virtual void OnEventTrigger_AdventureReachEndNode(short arg0)
	{
	}

	public virtual void OnEventTrigger_AdventureEnterNode(AdventureMapPoint arg0)
	{
	}

	public virtual void OnEventTrigger_EnterTutorialChapter(short arg0)
	{
	}

	public virtual void OnEventTrigger_LetTeammateLeaveGroup(int arg0)
	{
	}

	public virtual void OnEventTrigger_NeedToPassLegacy(bool arg0, string arg1)
	{
	}

	public virtual void OnEventTrigger_CaravanClicked(int arg0)
	{
	}

	public virtual void OnEventTrigger_KidnappedCharacterClicked(int arg0)
	{
	}

	public virtual void OnEventTrigger_TeammateMonthAdvance(int arg0)
	{
	}

	public virtual void OnEventTrigger_SameBlockWithTaiwuWhenMonthAdvance(int arg0)
	{
	}

	public virtual void OnEventTrigger_SameBlockWithRandomEnemyOnNewMonth()
	{
	}

	public virtual void OnEventTrigger_SameBlockWithTaiwuOnNewMonthSpecial(int arg0)
	{
	}

	public virtual void OnEventTrigger_SectBuildingClicked(short arg0)
	{
	}

	public virtual void OnEventTrigger_SecretInformationBroadcast(int arg0)
	{
	}

	public virtual void OnEventTrigger_RecordEnterGame(short arg0)
	{
	}

	public virtual void OnEventTrigger_NewGameMonth()
	{
	}

	public virtual void OnEventTrigger_CombatWithXiangshuMinionComplete(short arg0)
	{
	}

	public virtual void OnEventTrigger_BlackMaskAnimationComplete(bool arg0)
	{
	}

	public virtual void OnEventTrigger_ConstructComplete(BuildingBlockKey arg0, short arg1, sbyte arg2)
	{
	}

	public virtual void OnEventTrigger_CombatOpening(int arg0)
	{
	}

	public virtual void OnEventTrigger_MakingSystemOpened(BuildingBlockKey arg0, short arg1)
	{
	}

	public virtual void OnEventTrigger_CollectedMakingSystemItem(BuildingBlockKey arg0, short arg1, bool arg2)
	{
	}

	public virtual void OnEventTrigger_TaiwuVillageDestroyed()
	{
	}

	public virtual void OnEventTrigger_OnSectSpecialBuildingClicked(short arg0)
	{
	}

	public virtual void OnEventTrigger_AnimalAvatarClicked(int arg0)
	{
	}

	public virtual void OnEventTrigger_MainStoryFinishCatchCricket(bool arg0)
	{
	}

	public virtual void OnEventTrigger_PurpleBambooAvatarClicked(int arg0, sbyte arg1)
	{
	}

	public virtual void OnEventTrigger_UserLoadDreamBackArchive()
	{
	}

	public virtual void OnEventTrigger_NpcTombClicked(int arg0)
	{
	}

	public virtual void OnEventTrigger_LifeSkillCombatForceSilent(int arg0, sbyte arg1, sbyte arg2)
	{
	}

	public virtual void OnEventTrigger_TryMoveWhenMoveDisabled()
	{
	}

	public virtual void OnEventTrigger_TryMoveToInvalidLocationInTutorial()
	{
	}

	public virtual void OnEventTrigger_ProfessionExperienceChange(int arg0, int arg1, int arg2)
	{
	}

	public virtual void OnEventTrigger_ProfessionSkillClicked(int arg0)
	{
	}

	public virtual void OnEventTrigger_TaiwuGotTianjieFulu(int arg0, ItemKey arg1, int arg2)
	{
	}

	public virtual void OnEventTrigger_TaiwuSaveCountChange(int arg0)
	{
	}

	public virtual void OnEventTrigger_CharacterTemplateClicked(short arg0)
	{
	}

	public virtual void OnEventTrigger_CloseUI(string arg0, bool arg1, int arg2)
	{
	}

	public virtual void OnEventTrigger_TaiwuFindMaterial(int arg0, TreasureFindResult arg1)
	{
	}

	public virtual void OnEventTrigger_TaiwuFindExtraTreasure(TreasureFindResult arg0)
	{
	}

	public virtual void OnEventTrigger_TaiwuCollectWudangHeavenlyTreeSeed(sbyte arg0)
	{
	}

	public virtual void OnEventTrigger_TaiwuVillagerExpelled(int arg0)
	{
	}

	public virtual void OnEventTrigger_TaiwuCrossArchive()
	{
	}

	public virtual void OnEventTrigger_TaiwuCrossArchiveFindMemory(sbyte arg0)
	{
	}

	public virtual void OnEventTrigger_DlcLoongPutJiaoEggs(int arg0, ItemKey arg1)
	{
	}

	public virtual void OnEventTrigger_DlcLoongInteractJiao(int arg0)
	{
	}

	public virtual void OnEventTrigger_DlcLoongPetJiao(int arg0)
	{
	}

	public virtual void OnEventTrigger_JingangSectMainStoryReborn()
	{
	}

	public virtual void OnEventTrigger_JingangSectMainStoryMonkSoul()
	{
	}

	public virtual void OnEventTrigger_OperateInventoryItem(int arg0, sbyte arg1, ItemDisplayData arg2)
	{
	}

	public virtual void OnEventTrigger_OnSettlementTreasuryBuildingClicked(short arg0, byte arg1, sbyte arg2)
	{
	}

	public virtual void OnEventTrigger_OnShixiangDrumClickedManyTimes()
	{
	}

	public virtual void OnEventTrigger_OnClickedPrisonBtn(short arg0)
	{
	}

	public virtual void OnEventTrigger_OnClickedSendPrisonBtn()
	{
	}

	public virtual void OnEventTrigger_InteractPrisoner(int arg0, int arg1)
	{
	}

	public virtual void OnEventTrigger_ClickChicken(int arg0, short arg1)
	{
	}

	public virtual void OnEventTrigger_SoulWitheringBellTransfer()
	{
	}

	public virtual void OnEventTrigger_CatchThief(sbyte arg0, bool arg1)
	{
	}

	public virtual void OnEventTrigger_ConfirmEnterSwordTomb()
	{
	}

	public virtual void OnEventTrigger_TaiwuBeHuntedArrivedSect(int arg0)
	{
	}

	public virtual void OnEventTrigger_TaiwuBeHuntedHunterDie(int arg0)
	{
	}

	public virtual void OnEventTrigger_StartSectShaolinDemonSlayer(int arg0)
	{
	}

	public virtual void OnEventTrigger_TriggerMapPickupEvent(Location arg0, bool arg1)
	{
	}

	public virtual void OnEventTrigger_FixedCharacterClicked(int arg0, short arg1)
	{
	}

	public virtual void OnEventTrigger_FixedEnemyClicked(int arg0, short arg1)
	{
	}

	public virtual void OnEventTrigger_AdventureRemoved(short arg0, Location arg1, bool arg2)
	{
	}

	public virtual void OnEventTrigger_TaiwuDeportVitals(int arg0, bool arg1)
	{
	}

	public virtual void OnEventTrigger_SwitchToGuardedPage(byte arg0, sbyte arg1)
	{
	}
}
