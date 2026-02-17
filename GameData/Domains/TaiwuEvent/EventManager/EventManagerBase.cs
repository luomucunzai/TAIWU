using System;
using Config.EventConfig;
using GameData.Domains.Adventure;
using GameData.Domains.Building;
using GameData.Domains.Extra;
using GameData.Domains.Item;
using GameData.Domains.Item.Display;
using GameData.Domains.Map;

namespace GameData.Domains.TaiwuEvent.EventManager
{
	// Token: 0x020000CB RID: 203
	public abstract class EventManagerBase
	{
		// Token: 0x06001BFB RID: 7163
		public abstract TaiwuEvent GetEvent(string eventGuid);

		// Token: 0x06001BFC RID: 7164
		public abstract void HandleEventPackage(EventPackage package);

		// Token: 0x06001BFD RID: 7165
		public abstract void ClearExtendOptions();

		// Token: 0x06001BFE RID: 7166
		public abstract void Reset();

		// Token: 0x06001BFF RID: 7167
		public abstract void UnloadPackage(EventPackage package);

		// Token: 0x06001C00 RID: 7168 RVA: 0x0017FDE4 File Offset: 0x0017DFE4
		public virtual void OnEventTrigger_TaiwuBlockChanged(Location arg0, Location arg1)
		{
		}

		// Token: 0x06001C01 RID: 7169 RVA: 0x0017FDE7 File Offset: 0x0017DFE7
		public virtual void OnEventTrigger_CharacterClicked(int arg0)
		{
		}

		// Token: 0x06001C02 RID: 7170 RVA: 0x0017FDEA File Offset: 0x0017DFEA
		public virtual void OnEventTrigger_AdventureReachStartNode(short arg0)
		{
		}

		// Token: 0x06001C03 RID: 7171 RVA: 0x0017FDED File Offset: 0x0017DFED
		public virtual void OnEventTrigger_AdventureReachTransferNode(short arg0)
		{
		}

		// Token: 0x06001C04 RID: 7172 RVA: 0x0017FDF0 File Offset: 0x0017DFF0
		public virtual void OnEventTrigger_AdventureReachEndNode(short arg0)
		{
		}

		// Token: 0x06001C05 RID: 7173 RVA: 0x0017FDF3 File Offset: 0x0017DFF3
		public virtual void OnEventTrigger_AdventureEnterNode(AdventureMapPoint arg0)
		{
		}

		// Token: 0x06001C06 RID: 7174 RVA: 0x0017FDF6 File Offset: 0x0017DFF6
		public virtual void OnEventTrigger_EnterTutorialChapter(short arg0)
		{
		}

		// Token: 0x06001C07 RID: 7175 RVA: 0x0017FDF9 File Offset: 0x0017DFF9
		public virtual void OnEventTrigger_LetTeammateLeaveGroup(int arg0)
		{
		}

		// Token: 0x06001C08 RID: 7176 RVA: 0x0017FDFC File Offset: 0x0017DFFC
		public virtual void OnEventTrigger_NeedToPassLegacy(bool arg0, string arg1)
		{
		}

		// Token: 0x06001C09 RID: 7177 RVA: 0x0017FDFF File Offset: 0x0017DFFF
		public virtual void OnEventTrigger_CaravanClicked(int arg0)
		{
		}

		// Token: 0x06001C0A RID: 7178 RVA: 0x0017FE02 File Offset: 0x0017E002
		public virtual void OnEventTrigger_KidnappedCharacterClicked(int arg0)
		{
		}

		// Token: 0x06001C0B RID: 7179 RVA: 0x0017FE05 File Offset: 0x0017E005
		public virtual void OnEventTrigger_TeammateMonthAdvance(int arg0)
		{
		}

		// Token: 0x06001C0C RID: 7180 RVA: 0x0017FE08 File Offset: 0x0017E008
		public virtual void OnEventTrigger_SameBlockWithTaiwuWhenMonthAdvance(int arg0)
		{
		}

		// Token: 0x06001C0D RID: 7181 RVA: 0x0017FE0B File Offset: 0x0017E00B
		public virtual void OnEventTrigger_SameBlockWithRandomEnemyOnNewMonth()
		{
		}

		// Token: 0x06001C0E RID: 7182 RVA: 0x0017FE0E File Offset: 0x0017E00E
		public virtual void OnEventTrigger_SameBlockWithTaiwuOnNewMonthSpecial(int arg0)
		{
		}

		// Token: 0x06001C0F RID: 7183 RVA: 0x0017FE11 File Offset: 0x0017E011
		public virtual void OnEventTrigger_SectBuildingClicked(short arg0)
		{
		}

		// Token: 0x06001C10 RID: 7184 RVA: 0x0017FE14 File Offset: 0x0017E014
		public virtual void OnEventTrigger_SecretInformationBroadcast(int arg0)
		{
		}

		// Token: 0x06001C11 RID: 7185 RVA: 0x0017FE17 File Offset: 0x0017E017
		public virtual void OnEventTrigger_RecordEnterGame(short arg0)
		{
		}

		// Token: 0x06001C12 RID: 7186 RVA: 0x0017FE1A File Offset: 0x0017E01A
		public virtual void OnEventTrigger_NewGameMonth()
		{
		}

		// Token: 0x06001C13 RID: 7187 RVA: 0x0017FE1D File Offset: 0x0017E01D
		public virtual void OnEventTrigger_CombatWithXiangshuMinionComplete(short arg0)
		{
		}

		// Token: 0x06001C14 RID: 7188 RVA: 0x0017FE20 File Offset: 0x0017E020
		public virtual void OnEventTrigger_BlackMaskAnimationComplete(bool arg0)
		{
		}

		// Token: 0x06001C15 RID: 7189 RVA: 0x0017FE23 File Offset: 0x0017E023
		public virtual void OnEventTrigger_ConstructComplete(BuildingBlockKey arg0, short arg1, sbyte arg2)
		{
		}

		// Token: 0x06001C16 RID: 7190 RVA: 0x0017FE26 File Offset: 0x0017E026
		public virtual void OnEventTrigger_CombatOpening(int arg0)
		{
		}

		// Token: 0x06001C17 RID: 7191 RVA: 0x0017FE29 File Offset: 0x0017E029
		public virtual void OnEventTrigger_MakingSystemOpened(BuildingBlockKey arg0, short arg1)
		{
		}

		// Token: 0x06001C18 RID: 7192 RVA: 0x0017FE2C File Offset: 0x0017E02C
		public virtual void OnEventTrigger_CollectedMakingSystemItem(BuildingBlockKey arg0, short arg1, bool arg2)
		{
		}

		// Token: 0x06001C19 RID: 7193 RVA: 0x0017FE2F File Offset: 0x0017E02F
		public virtual void OnEventTrigger_TaiwuVillageDestroyed()
		{
		}

		// Token: 0x06001C1A RID: 7194 RVA: 0x0017FE32 File Offset: 0x0017E032
		public virtual void OnEventTrigger_OnSectSpecialBuildingClicked(short arg0)
		{
		}

		// Token: 0x06001C1B RID: 7195 RVA: 0x0017FE35 File Offset: 0x0017E035
		public virtual void OnEventTrigger_AnimalAvatarClicked(int arg0)
		{
		}

		// Token: 0x06001C1C RID: 7196 RVA: 0x0017FE38 File Offset: 0x0017E038
		public virtual void OnEventTrigger_MainStoryFinishCatchCricket(bool arg0)
		{
		}

		// Token: 0x06001C1D RID: 7197 RVA: 0x0017FE3B File Offset: 0x0017E03B
		public virtual void OnEventTrigger_PurpleBambooAvatarClicked(int arg0, sbyte arg1)
		{
		}

		// Token: 0x06001C1E RID: 7198 RVA: 0x0017FE3E File Offset: 0x0017E03E
		public virtual void OnEventTrigger_UserLoadDreamBackArchive()
		{
		}

		// Token: 0x06001C1F RID: 7199 RVA: 0x0017FE41 File Offset: 0x0017E041
		public virtual void OnEventTrigger_NpcTombClicked(int arg0)
		{
		}

		// Token: 0x06001C20 RID: 7200 RVA: 0x0017FE44 File Offset: 0x0017E044
		public virtual void OnEventTrigger_LifeSkillCombatForceSilent(int arg0, sbyte arg1, sbyte arg2)
		{
		}

		// Token: 0x06001C21 RID: 7201 RVA: 0x0017FE47 File Offset: 0x0017E047
		public virtual void OnEventTrigger_TryMoveWhenMoveDisabled()
		{
		}

		// Token: 0x06001C22 RID: 7202 RVA: 0x0017FE4A File Offset: 0x0017E04A
		public virtual void OnEventTrigger_TryMoveToInvalidLocationInTutorial()
		{
		}

		// Token: 0x06001C23 RID: 7203 RVA: 0x0017FE4D File Offset: 0x0017E04D
		public virtual void OnEventTrigger_ProfessionExperienceChange(int arg0, int arg1, int arg2)
		{
		}

		// Token: 0x06001C24 RID: 7204 RVA: 0x0017FE50 File Offset: 0x0017E050
		public virtual void OnEventTrigger_ProfessionSkillClicked(int arg0)
		{
		}

		// Token: 0x06001C25 RID: 7205 RVA: 0x0017FE53 File Offset: 0x0017E053
		public virtual void OnEventTrigger_TaiwuGotTianjieFulu(int arg0, ItemKey arg1, int arg2)
		{
		}

		// Token: 0x06001C26 RID: 7206 RVA: 0x0017FE56 File Offset: 0x0017E056
		public virtual void OnEventTrigger_TaiwuSaveCountChange(int arg0)
		{
		}

		// Token: 0x06001C27 RID: 7207 RVA: 0x0017FE59 File Offset: 0x0017E059
		public virtual void OnEventTrigger_CharacterTemplateClicked(short arg0)
		{
		}

		// Token: 0x06001C28 RID: 7208 RVA: 0x0017FE5C File Offset: 0x0017E05C
		public virtual void OnEventTrigger_CloseUI(string arg0, bool arg1, int arg2)
		{
		}

		// Token: 0x06001C29 RID: 7209 RVA: 0x0017FE5F File Offset: 0x0017E05F
		public virtual void OnEventTrigger_TaiwuFindMaterial(int arg0, TreasureFindResult arg1)
		{
		}

		// Token: 0x06001C2A RID: 7210 RVA: 0x0017FE62 File Offset: 0x0017E062
		public virtual void OnEventTrigger_TaiwuFindExtraTreasure(TreasureFindResult arg0)
		{
		}

		// Token: 0x06001C2B RID: 7211 RVA: 0x0017FE65 File Offset: 0x0017E065
		public virtual void OnEventTrigger_TaiwuCollectWudangHeavenlyTreeSeed(sbyte arg0)
		{
		}

		// Token: 0x06001C2C RID: 7212 RVA: 0x0017FE68 File Offset: 0x0017E068
		public virtual void OnEventTrigger_TaiwuVillagerExpelled(int arg0)
		{
		}

		// Token: 0x06001C2D RID: 7213 RVA: 0x0017FE6B File Offset: 0x0017E06B
		public virtual void OnEventTrigger_TaiwuCrossArchive()
		{
		}

		// Token: 0x06001C2E RID: 7214 RVA: 0x0017FE6E File Offset: 0x0017E06E
		public virtual void OnEventTrigger_TaiwuCrossArchiveFindMemory(sbyte arg0)
		{
		}

		// Token: 0x06001C2F RID: 7215 RVA: 0x0017FE71 File Offset: 0x0017E071
		public virtual void OnEventTrigger_DlcLoongPutJiaoEggs(int arg0, ItemKey arg1)
		{
		}

		// Token: 0x06001C30 RID: 7216 RVA: 0x0017FE74 File Offset: 0x0017E074
		public virtual void OnEventTrigger_DlcLoongInteractJiao(int arg0)
		{
		}

		// Token: 0x06001C31 RID: 7217 RVA: 0x0017FE77 File Offset: 0x0017E077
		public virtual void OnEventTrigger_DlcLoongPetJiao(int arg0)
		{
		}

		// Token: 0x06001C32 RID: 7218 RVA: 0x0017FE7A File Offset: 0x0017E07A
		public virtual void OnEventTrigger_JingangSectMainStoryReborn()
		{
		}

		// Token: 0x06001C33 RID: 7219 RVA: 0x0017FE7D File Offset: 0x0017E07D
		public virtual void OnEventTrigger_JingangSectMainStoryMonkSoul()
		{
		}

		// Token: 0x06001C34 RID: 7220 RVA: 0x0017FE80 File Offset: 0x0017E080
		public virtual void OnEventTrigger_OperateInventoryItem(int arg0, sbyte arg1, ItemDisplayData arg2)
		{
		}

		// Token: 0x06001C35 RID: 7221 RVA: 0x0017FE83 File Offset: 0x0017E083
		public virtual void OnEventTrigger_OnSettlementTreasuryBuildingClicked(short arg0, byte arg1, sbyte arg2)
		{
		}

		// Token: 0x06001C36 RID: 7222 RVA: 0x0017FE86 File Offset: 0x0017E086
		public virtual void OnEventTrigger_OnShixiangDrumClickedManyTimes()
		{
		}

		// Token: 0x06001C37 RID: 7223 RVA: 0x0017FE89 File Offset: 0x0017E089
		public virtual void OnEventTrigger_OnClickedPrisonBtn(short arg0)
		{
		}

		// Token: 0x06001C38 RID: 7224 RVA: 0x0017FE8C File Offset: 0x0017E08C
		public virtual void OnEventTrigger_OnClickedSendPrisonBtn()
		{
		}

		// Token: 0x06001C39 RID: 7225 RVA: 0x0017FE8F File Offset: 0x0017E08F
		public virtual void OnEventTrigger_InteractPrisoner(int arg0, int arg1)
		{
		}

		// Token: 0x06001C3A RID: 7226 RVA: 0x0017FE92 File Offset: 0x0017E092
		public virtual void OnEventTrigger_ClickChicken(int arg0, short arg1)
		{
		}

		// Token: 0x06001C3B RID: 7227 RVA: 0x0017FE95 File Offset: 0x0017E095
		public virtual void OnEventTrigger_SoulWitheringBellTransfer()
		{
		}

		// Token: 0x06001C3C RID: 7228 RVA: 0x0017FE98 File Offset: 0x0017E098
		public virtual void OnEventTrigger_CatchThief(sbyte arg0, bool arg1)
		{
		}

		// Token: 0x06001C3D RID: 7229 RVA: 0x0017FE9B File Offset: 0x0017E09B
		public virtual void OnEventTrigger_ConfirmEnterSwordTomb()
		{
		}

		// Token: 0x06001C3E RID: 7230 RVA: 0x0017FE9E File Offset: 0x0017E09E
		public virtual void OnEventTrigger_TaiwuBeHuntedArrivedSect(int arg0)
		{
		}

		// Token: 0x06001C3F RID: 7231 RVA: 0x0017FEA1 File Offset: 0x0017E0A1
		public virtual void OnEventTrigger_TaiwuBeHuntedHunterDie(int arg0)
		{
		}

		// Token: 0x06001C40 RID: 7232 RVA: 0x0017FEA4 File Offset: 0x0017E0A4
		public virtual void OnEventTrigger_StartSectShaolinDemonSlayer(int arg0)
		{
		}

		// Token: 0x06001C41 RID: 7233 RVA: 0x0017FEA7 File Offset: 0x0017E0A7
		public virtual void OnEventTrigger_TriggerMapPickupEvent(Location arg0, bool arg1)
		{
		}

		// Token: 0x06001C42 RID: 7234 RVA: 0x0017FEAA File Offset: 0x0017E0AA
		public virtual void OnEventTrigger_FixedCharacterClicked(int arg0, short arg1)
		{
		}

		// Token: 0x06001C43 RID: 7235 RVA: 0x0017FEAD File Offset: 0x0017E0AD
		public virtual void OnEventTrigger_FixedEnemyClicked(int arg0, short arg1)
		{
		}

		// Token: 0x06001C44 RID: 7236 RVA: 0x0017FEB0 File Offset: 0x0017E0B0
		public virtual void OnEventTrigger_AdventureRemoved(short arg0, Location arg1, bool arg2)
		{
		}

		// Token: 0x06001C45 RID: 7237 RVA: 0x0017FEB3 File Offset: 0x0017E0B3
		public virtual void OnEventTrigger_TaiwuDeportVitals(int arg0, bool arg1)
		{
		}

		// Token: 0x06001C46 RID: 7238 RVA: 0x0017FEB6 File Offset: 0x0017E0B6
		public virtual void OnEventTrigger_SwitchToGuardedPage(byte arg0, sbyte arg1)
		{
		}
	}
}
