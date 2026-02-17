using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Config;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.Character.Ai.PrioritizedAction;
using GameData.Domains.Character.AvatarSystem;
using GameData.Domains.Character.Creation;
using GameData.Domains.Character.Filters;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.Extra;
using GameData.Domains.Information;
using GameData.Domains.Item;
using GameData.Domains.Item.Display;
using GameData.Domains.LifeRecord;
using GameData.Domains.Map;
using GameData.Domains.Organization;
using GameData.Domains.Organization.SettlementPrisonRecord;
using GameData.Domains.Taiwu.Profession.SkillsData;
using GameData.Domains.TaiwuEvent;
using GameData.Domains.TaiwuEvent.EventHelper;
using GameData.Domains.World.MonthlyEvent;
using GameData.Domains.World.Notification;
using GameData.GameDataBridge;
using GameData.Utilities;

namespace GameData.Domains.Taiwu.Profession
{
	// Token: 0x02000056 RID: 86
	public static class ProfessionSkillHandle
	{
		// Token: 0x06001456 RID: 5206 RVA: 0x0013EA3C File Offset: 0x0013CC3C
		public unsafe static bool CanExecuteSkill(ProfessionData professionData, int skillIndex)
		{
			ProfessionSkillItem skillCfg = professionData.GetSkillConfig(skillIndex);
			bool ignoreCanExecuteSkill = skillCfg.IgnoreCanExecuteSkill;
			bool result;
			if (ignoreCanExecuteSkill)
			{
				result = true;
			}
			else
			{
				bool flag = skillCfg.TriggerType > EProfessionSkillTriggerType.Active;
				if (flag)
				{
					result = false;
				}
				else
				{
					bool flag2 = DomainManager.Taiwu.GetTaiwu().GetExp() < skillCfg.ExpCost;
					if (flag2)
					{
						result = false;
					}
					else
					{
						bool flag3 = DomainManager.World.GetLeftDaysInCurrMonth() < (int)skillCfg.TimeCost;
						if (flag3)
						{
							result = false;
						}
						else
						{
							bool flag4 = professionData.IsSkillCooldown(DomainManager.World.GetCurrDate(), skillIndex);
							if (flag4)
							{
								result = false;
							}
							else
							{
								GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
								ResourceInts resources = *taiwu.GetResources();
								foreach (ResourceInfo resourceInfo in skillCfg.ResourcesCost)
								{
									bool flag5 = !resources.CheckIsMeet(resourceInfo.ResourceType, resourceInfo.ResourceCount);
									if (flag5)
									{
										return false;
									}
								}
								bool flag6 = !ProfessionSkillHandle.CheckSpecialCondition(professionData, skillIndex);
								result = !flag6;
							}
						}
					}
				}
			}
			return result;
		}

		// Token: 0x06001457 RID: 5207 RVA: 0x0013EB7C File Offset: 0x0013CD7C
		public static int GetSkillIndex(ProfessionSkillItem skillCfg)
		{
			ProfessionItem professionCfg = Profession.Instance[skillCfg.Profession];
			return (professionCfg.ExtraProfessionSkill == skillCfg.TemplateId) ? professionCfg.ProfessionSkills.Length : professionCfg.ProfessionSkills.IndexOf(skillCfg.TemplateId);
		}

		// Token: 0x06001458 RID: 5208 RVA: 0x0013EBC8 File Offset: 0x0013CDC8
		public static bool CheckSpecialCondition(int professionId, int skillIndex)
		{
			ProfessionData professionData = DomainManager.Extra.GetProfessionData(professionId);
			return ProfessionSkillHandle.CheckSpecialCondition(professionData, skillIndex);
		}

		// Token: 0x06001459 RID: 5209 RVA: 0x0013EBF0 File Offset: 0x0013CDF0
		public static bool CheckSpecialCondition(ProfessionData professionData, int skillIndex)
		{
			switch (professionData.TemplateId)
			{
			case 0:
				return ProfessionSkillHandle.CheckSpecialCondition_SavageSkill(professionData, skillIndex);
			case 1:
				return ProfessionSkillHandle.CheckSpecialCondition_HunterSkill(professionData, skillIndex);
			case 3:
				return ProfessionSkillHandle.CheckSpecialCondition_MartialArtistSkill(professionData, skillIndex);
			case 4:
				return ProfessionSkillHandle.CheckSpecialCondition_LiteratiSkill(professionData, skillIndex);
			case 5:
				return ProfessionSkillHandle.CheckSpecialCondition_TaoistMonkSkill(professionData, skillIndex);
			case 6:
				return ProfessionSkillHandle.CheckSpecialCondition_BuddhistMonkSkill(professionData, skillIndex);
			case 7:
				return ProfessionSkillHandle.CheckSpecialCondition_WineTasterSkill(professionData, skillIndex);
			case 8:
				return ProfessionSkillHandle.CheckSpecialCondition_AristocratSkill(professionData, skillIndex);
			case 9:
				return ProfessionSkillHandle.CheckSpecialCondition_BeggarSkill(professionData, skillIndex);
			case 10:
				return ProfessionSkillHandle.CheckSpecialCondition_CivilianSkill(professionData, skillIndex);
			case 12:
				return ProfessionSkillHandle.CheckSpecialCondition_TravelingBuddhistMonkSkill(professionData, skillIndex);
			case 13:
				return ProfessionSkillHandle.CheckSpecialCondition_DoctorSkill(professionData, skillIndex);
			case 14:
				return ProfessionSkillHandle.CheckSpecialCondition_TravelingTaoistMonkSkill(professionData, skillIndex);
			case 15:
				return ProfessionSkillHandle.CheckSpecialCondition_CapitalistSkill(professionData, skillIndex);
			case 16:
				return ProfessionSkillHandle.CheckSpecialCondition_TeaTasterSkill(professionData, skillIndex);
			case 17:
				return ProfessionSkillHandle.CheckSpecialCondition_DukeSkill(professionData, skillIndex);
			}
			return true;
		}

		// Token: 0x0600145A RID: 5210 RVA: 0x0013ED10 File Offset: 0x0013CF10
		public static void OnSkillExecuted(DataContext context, ref ProfessionSkillArg arg)
		{
			ProfessionData professionData = DomainManager.Extra.GetProfessionData(arg.ProfessionId);
			int skillIndex = professionData.GetSkillIndex(arg.SkillId);
			ProfessionSkillItem skillCfg = professionData.GetSkillConfig(skillIndex);
			bool flag = !DomainManager.Extra.NoProfessionSkillCost;
			if (flag)
			{
				bool flag2 = !skillCfg.CostTimeWhenFinished;
				if (flag2)
				{
					DomainManager.World.AdvanceDaysInMonth(context, (int)skillCfg.TimeCost);
				}
				GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
				foreach (ResourceInfo resourceInfo in skillCfg.ResourcesCost)
				{
					taiwu.ChangeResource(context, resourceInfo.ResourceType, -resourceInfo.ResourceCount);
				}
				DomainManager.Taiwu.GetTaiwu().ChangeExp(context, -skillCfg.ExpCost);
			}
			professionData.OfflineSkillCooldown(skillIndex);
			DomainManager.Extra.SetProfessionData(context, professionData);
			bool flag3 = skillCfg.Type == EProfessionSkillType.Interactive;
			if (flag3)
			{
				DomainManager.TaiwuEvent.SetIsSequential(true);
			}
		}

		// Token: 0x0600145B RID: 5211 RVA: 0x0013EE2C File Offset: 0x0013D02C
		public static void OnActiveSkillExecuted(DataContext context, ref ProfessionSkillArg arg)
		{
			ProfessionData professionData = DomainManager.Extra.GetProfessionData(arg.ProfessionId);
			int skillIndex = professionData.GetSkillIndex(arg.SkillId);
			ProfessionSkillItem skillCfg = professionData.GetSkillConfig(skillIndex);
			bool flag = skillCfg.Type > EProfessionSkillType.Active;
			if (!flag)
			{
				switch (professionData.TemplateId)
				{
				case 0:
					ProfessionSkillHandle.ExecuteOnClick_SavageSkill(context, professionData, skillIndex, ref arg);
					break;
				case 1:
					ProfessionSkillHandle.ExecuteOnClick_HunterSkill(context, professionData, skillIndex, ref arg);
					break;
				case 2:
					ProfessionSkillHandle.ExecuteOnClick_CraftSkill(context, professionData, skillIndex, ref arg);
					break;
				case 3:
					ProfessionSkillHandle.ExecuteOnClick_MartialArtist(context, professionData, skillIndex, ref arg);
					break;
				case 5:
					ProfessionSkillHandle.ExecuteOnClick_TaoistMonkSkill(context, professionData, skillIndex, ref arg);
					break;
				case 6:
					ProfessionSkillHandle.ExecuteOnClick_BuddhistMonkSkill(context, professionData, skillIndex, ref arg);
					break;
				case 7:
					ProfessionSkillHandle.ExecuteOnClick_WineTasterSkill(context, professionData, skillIndex, ref arg);
					break;
				case 8:
					ProfessionSkillHandle.ExecuteOnClick_AristocratSkill(context, professionData, skillIndex, ref arg);
					break;
				case 9:
					ProfessionSkillHandle.ExecuteOnClick_BeggarSkill(context, professionData, skillIndex, ref arg);
					break;
				case 10:
					ProfessionSkillHandle.ExecuteOnClick_CivilianSkill(context, professionData, skillIndex, ref arg);
					break;
				case 11:
					ProfessionSkillHandle.ExecuteOnClick_TravelerSkill(context, professionData, skillIndex, ref arg);
					break;
				case 12:
					ProfessionSkillHandle.ExecuteOnClick_TravelingBuddhistMonkSkill(context, professionData, skillIndex, ref arg);
					break;
				case 13:
					ProfessionSkillHandle.ExecuteOnClick_DoctorSkill(context, professionData, skillIndex, ref arg);
					break;
				case 14:
					ProfessionSkillHandle.ExecuteOnClick_TravelingTaoistMonkSkill(context, professionData, skillIndex, ref arg);
					break;
				case 16:
					ProfessionSkillHandle.ExecuteOnClick_TeaTasterSkill(context, professionData, skillIndex, ref arg);
					break;
				case 17:
					ProfessionSkillHandle.ExecuteOnClick_DukeSkill(context, professionData, skillIndex, ref arg);
					break;
				}
			}
		}

		// Token: 0x0600145C RID: 5212 RVA: 0x0013EFAA File Offset: 0x0013D1AA
		public static void ConfirmSkillExecute(ref ProfessionSkillArg professionSkillArg)
		{
			GameDataBridge.AddDisplayEvent<ProfessionSkillArg>(DisplayEventType.ConfirmProfessionSkillExecute, professionSkillArg);
		}

		// Token: 0x0600145D RID: 5213 RVA: 0x0013EFB8 File Offset: 0x0013D1B8
		public static void ConfirmSkillExecuteWithEvent(ProfessionSkillArg professionSkillArg, string afterEvent, EventArgBox argBox)
		{
			bool flag = !string.IsNullOrEmpty(afterEvent);
			if (flag)
			{
				EventHelper.AddEventInListenWithActionName(afterEvent, argBox, "ConfirmProfessionSkillExecuteAndAnimComplete");
			}
			int beggarMoneyCount = 0;
			bool flag2 = argBox.Contains<int>("BeggarMoneyCount");
			if (flag2)
			{
				beggarMoneyCount = argBox.GetInt("BeggarMoneyCount");
			}
			GameDataBridge.AddDisplayEvent<ProfessionSkillArg, int>(DisplayEventType.ConfirmProfessionSkillExecute, professionSkillArg, beggarMoneyCount);
		}

		// Token: 0x0600145E RID: 5214 RVA: 0x0013F00C File Offset: 0x0013D20C
		public static void ExecuteActiveProfessionSkill(int professionId, int skillIndex)
		{
			ProfessionData professionData = DomainManager.Extra.GetProfessionData(professionId);
			ProfessionSkillItem skillCfg = professionData.GetSkillConfig(skillIndex);
			DomainManager.TaiwuEvent.OnEvent_ProfessionSkillClicked(skillCfg.TemplateId);
			bool instant = skillCfg.Instant;
			if (instant)
			{
				ProfessionSkillArg professionSkillArg = new ProfessionSkillArg
				{
					ProfessionId = professionId,
					SkillId = skillCfg.TemplateId,
					IsSuccess = true,
					SkipAnimation = (skillCfg.TemplateId == 18 || skillCfg.TemplateId == 63)
				};
				ProfessionSkillHandle.ConfirmSkillExecute(ref professionSkillArg);
			}
			else
			{
				int templateId = skillCfg.TemplateId;
				int num = templateId;
				if (num <= 37)
				{
					if (num == 8)
					{
						ProfessionSkillHandle.OpenSetEquipmentEffect();
						goto IL_2BF;
					}
					if (num == 14)
					{
						SecretInformationDisplayPackage secrets = DomainManager.Information.GetSecretInformationDisplayPackageFromCharacter(DomainManager.Taiwu.GetTaiwuCharId());
						bool flag = secrets.SecretInformationDisplayDataList != null;
						if (flag)
						{
							foreach (SecretInformationDisplayData secret in secrets.SecretInformationDisplayDataList)
							{
								SecretInformationItem config = SecretInformation.Instance.GetItem(secret.SecretInformationTemplateId);
								secret.AuthorityCostWhenDisseminatingForBroadcast = ProfessionSkillHandle.CalcLiteratiSkill2AuthorityCost(config, secret.HolderCount);
							}
						}
						GameDataBridge.AddDisplayEvent<SecretInformationDisplayPackage>(DisplayEventType.OpenSelectSecretInformationLiteratiSkill2, secrets);
						goto IL_2BF;
					}
					if (num != 37)
					{
						goto IL_2BF;
					}
				}
				else if (num != 41)
				{
					if (num == 51)
					{
						ProfessionSkillHandle.OpenInvestCaravan();
						goto IL_2BF;
					}
					switch (num)
					{
					case 57:
					{
						List<ItemDisplayData> cricketList = new List<ItemDisplayData>();
						foreach (KeyValuePair<ItemKey, int> itemPair in DomainManager.Taiwu.GetTaiwu().GetInventory().Items)
						{
							bool flag2 = itemPair.Key.ItemType != 11;
							if (!flag2)
							{
								GameData.Domains.Item.Cricket cricket;
								bool flag3 = !DomainManager.Item.TryGetElement_Crickets(itemPair.Key.Id, out cricket);
								if (!flag3)
								{
									bool isAlive = cricket.IsAlive;
									if (!isAlive)
									{
										cricketList.Add(DomainManager.Item.GetItemDisplayData(itemPair.Key, -1));
									}
								}
							}
						}
						GameDataBridge.AddDisplayEvent<List<ItemDisplayData>>(DisplayEventType.OpenSelectCricketDukeSkill2, cricketList);
						goto IL_2BF;
					}
					case 58:
					case 65:
					case 67:
					case 68:
						break;
					case 59:
					case 61:
					case 62:
					case 63:
					case 66:
						goto IL_2BF;
					case 60:
						ProfessionSkillHandle.OpenItemSelectFromBlock();
						goto IL_2BF;
					case 64:
					{
						NormalInformationCollection information = DomainManager.Information.GetCharacterNormalInformation(DomainManager.Taiwu.GetTaiwuCharId());
						GameDataBridge.AddDisplayEvent<List<NormalInformation>>(DisplayEventType.OpenSelectNormalInformationLiteratiSkill3, information.GetList().Where(new Func<NormalInformation, bool>(ProfessionSkillHandle.CheckLiteratiSkillNormalInformationUsable)).ToList<NormalInformation>());
						goto IL_2BF;
					}
					default:
						goto IL_2BF;
					}
				}
				GameDataBridge.AddDisplayEvent<int>(DisplayEventType.OpenProfessionSkillSpecial, skillCfg.TemplateId);
				IL_2BF:;
			}
		}

		// Token: 0x0600145F RID: 5215 RVA: 0x0013F2F8 File Offset: 0x0013D4F8
		public static void OnPreAdvanceMonth(DataContext context)
		{
			ProfessionSkillHandle.TravelingTaoistMonkSkill_OnPreAdvanceMonth(context);
		}

		// Token: 0x06001460 RID: 5216 RVA: 0x0013F304 File Offset: 0x0013D504
		public static void OnPostAdvanceMonth(DataContext context)
		{
			GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
			ProfessionSkillHandle.TeaTasterSkill_UpdateVillagerLearnSkillInterval(context);
			ProfessionSkillHandle.TeaTasterSkill_SetActionPointGained(context, 0);
			ProfessionSkillHandle.WineTasterSkill_UpdateVillagerLearnSkillInterval(context);
			ProfessionSkillHandle.BeggarSkill_AdvanceMonth(context);
			ProfessionSkillHandle.TaoistMonkSkill_OnPostAdvanceMonth(context);
			ProfessionSkillHandle.UpdateSeniorityOnPostAdvanceMonth(context, taiwu);
			ProfessionSkillHandle.UpdateDukeMonthlyEvent(context);
			int currDate = DomainManager.World.GetCurrDate();
			TaiwuProfessionSkillSlots slots = DomainManager.Extra.GetTaiwuProfessionSkillSlots();
			foreach (IntList levelSlots in slots.Slots)
			{
				foreach (int skillId in levelSlots.Items)
				{
					bool flag = skillId < 0;
					if (!flag)
					{
						ProfessionSkillItem skillConfig = ProfessionSkill.Instance[skillId];
						bool flag2 = skillConfig.Type == EProfessionSkillType.Passive;
						if (!flag2)
						{
							ProfessionData professionData = DomainManager.Extra.GetProfessionData(skillConfig.Profession);
							int skillIndex = (int)(skillConfig.Level - 1);
							bool flag3 = !professionData.IsSkillUnlocked(skillIndex);
							if (!flag3)
							{
								int offCooldownDate = professionData.SkillOffCooldownDates[skillIndex];
								bool flag4 = offCooldownDate != 0 && currDate == offCooldownDate;
								if (flag4)
								{
									DomainManager.World.GetInstantNotificationCollection().AddProfessionSkillHasCoolDown(skillConfig.TemplateId);
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x06001461 RID: 5217 RVA: 0x0013F478 File Offset: 0x0013D678
		private static bool IsSkillUnlocked(int skillTemplateId)
		{
			return DomainManager.Extra.IsProfessionalSkillUnlockedAndEquipped(skillTemplateId);
		}

		// Token: 0x06001462 RID: 5218 RVA: 0x0013F488 File Offset: 0x0013D688
		private static void UpdateSeniorityOnPostAdvanceMonth(DataContext context, GameData.Domains.Character.Character character)
		{
			ItemKey clothingKey = character.GetEquipment()[4];
			bool flag = !clothingKey.IsValid() || DomainManager.Item.GetBaseItem(clothingKey).IsDurabilityRunningOut();
			if (!flag)
			{
				foreach (ProfessionItem professionCfg in ((IEnumerable<ProfessionItem>)Profession.Instance))
				{
					bool flag2 = professionCfg.BonusClothing != clothingKey.TemplateId;
					if (!flag2)
					{
						DomainManager.Extra.ChangeProfessionSeniority(context, professionCfg.TemplateId, GlobalConfig.Instance.ProfessionSeniorityPerMonth, true, false);
						break;
					}
				}
			}
		}

		// Token: 0x06001463 RID: 5219 RVA: 0x0013F53C File Offset: 0x0013D73C
		private static void UpdateDukeMonthlyEvent(DataContext context)
		{
			SeasonItem autumnConfig = Season.Instance[2];
			sbyte currMonth = DomainManager.World.GetCurrMonthInYear();
			bool flag = autumnConfig.Months.Contains(currMonth);
			if (flag)
			{
				ProfessionData professionData = DomainManager.Extra.GetProfessionData(17);
				DukeSkillsData duke = (DukeSkillsData)professionData.SkillsData;
				bool flag2 = duke.GetNotGivenCricketTitles(new Predicate<int>(DomainManager.Character.IsCharacterAlive)).Any<short>();
				if (flag2)
				{
					DomainManager.World.GetMonthlyEventCollection().AddProfessionDukeReceiveCricket(DomainManager.Taiwu.GetTaiwuCharId());
				}
			}
			else
			{
				bool flag3 = currMonth == 10;
				if (flag3)
				{
					ProfessionData professionData2 = DomainManager.Extra.GetProfessionData(17);
					DukeSkillsData duke2 = (DukeSkillsData)professionData2.SkillsData;
					duke2.ResetAllCricketGivenData();
					DomainManager.Extra.SetProfessionData(context, professionData2);
				}
			}
		}

		// Token: 0x06001464 RID: 5220 RVA: 0x0013F60C File Offset: 0x0013D80C
		public static void UnpackCrossArchiveProfession(DataContext context, int professionId)
		{
			ProfessionData professionData = DomainManager.Extra.GetProfessionData(professionId);
			ProfessionItem professionCfg = Profession.Instance[professionId];
			bool flag = professionData.SkillsData != null && professionCfg.ReinitOnCrossArchive;
			if (flag)
			{
				professionData.SkillsData.Initialize();
				DomainManager.Extra.SetProfessionData(context, professionData);
			}
			if (professionId != 5)
			{
				if (professionId == 6)
				{
					ProfessionSkillHandle.UnpackCrossArchiveProfession_BuddhistMonk(context);
				}
			}
			else
			{
				ProfessionSkillHandle.UnpackCrossArchiveProfession_TaoistMonk(context);
			}
		}

		// Token: 0x06001465 RID: 5221 RVA: 0x0013F686 File Offset: 0x0013D886
		public static void OnTaiwuDeath(DataContext context)
		{
			ProfessionSkillHandle.TaoistMonkSkill_ResetSurvivedTribulationCount(context);
			ProfessionSkillHandle.BuddhistMonkSkill_ClearSavedSoulCount(context);
			ProfessionSkillHandle.TravelingTaoistMonkSkill_ClearHealthBonus(context);
		}

		// Token: 0x06001466 RID: 5222 RVA: 0x0013F6A0 File Offset: 0x0013D8A0
		public static void AristocratSkill_ChangeInfluencePower(DataContext context, int charId, bool isAdd)
		{
			SettlementCharacter settlementCharacter = DomainManager.Organization.GetSettlementCharacter(charId);
			short currInfluencePower = settlementCharacter.GetInfluencePower();
			ProfessionData professionData = DomainManager.Extra.GetProfessionData(8);
			int percentage = 50 + 50 * professionData.GetSeniorityPercent() / 100;
			AristocratSkillsData skillsData = professionData.GetSkillsData<AristocratSkillsData>();
			int bonus = (int)currInfluencePower * percentage / 100;
			if (isAdd)
			{
				skillsData.OfflineAddRecommendedCharId(charId);
			}
			else
			{
				bonus = -bonus;
			}
			skillsData.OfflineSetInfluencePowerBonus(charId, (short)bonus);
			DomainManager.Extra.SetProfessionData(context, professionData);
			short influencePower = (short)Math.Clamp((int)currInfluencePower + bonus, 0, 32767);
			settlementCharacter.SetInfluencePower(influencePower, context);
			int delta = Math.Abs((int)(influencePower - currInfluencePower));
			Location taiwuLocation = DomainManager.Taiwu.GetTaiwu().GetLocation();
			bool flag = !DomainManager.Taiwu.GetTaiwu().GetLocation().IsValid();
			if (!flag)
			{
				MapBlockData blockData = DomainManager.Map.GetBlock(taiwuLocation);
				DomainManager.Map.SetBlockData(context, blockData);
				InstantNotificationCollection instantNotification = DomainManager.World.GetInstantNotificationCollection();
				if (isAdd)
				{
					instantNotification.AddRecommendFellowUp(charId, delta, (int)influencePower);
				}
				else
				{
					instantNotification.AddRecommendFellowDown(charId, delta, (int)influencePower);
				}
			}
		}

		// Token: 0x06001467 RID: 5223 RVA: 0x0013F7C4 File Offset: 0x0013D9C4
		public static void AristocratSkill_BoostTaiwuAsTargetInCollection(List<int> collection)
		{
			int taiwuId = DomainManager.Taiwu.GetTaiwuCharId();
			for (int i = 0; i < 4; i++)
			{
				collection.Add(taiwuId);
			}
		}

		// Token: 0x06001468 RID: 5224 RVA: 0x0013F7F8 File Offset: 0x0013D9F8
		private static void ExecuteOnClick_AristocratSkill4(DataContext context)
		{
			GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
			int taiwuId = taiwu.GetId();
			Location location = taiwu.GetLocation();
			MapBlockData currBlock = DomainManager.Map.GetBlock(location);
			MapBlockData blockData = currBlock.GetRootBlock();
			Settlement settlement = DomainManager.Organization.GetSettlementByLocation(blockData.GetLocation());
			short settlementId = settlement.GetId();
			sbyte orgTemplateId = settlement.GetOrgTemplateId();
			int currDate = DomainManager.World.GetCurrDate();
			LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
			SettlementPrisonRecordCollection prisonRecordCollection = DomainManager.Extra.GetSettlementPrisonRecordCollection(context, settlementId);
			Sect sect = settlement as Sect;
			bool flag = sect != null;
			if (flag)
			{
				SettlementPrison prison = sect.Prison;
				int count = prison.Prisoners.Count;
				int realCount = 0;
				for (int i = count - 1; i >= 0; i--)
				{
					SettlementPrisoner prisoner = prison.Prisoners[i];
					GameData.Domains.Character.Character character;
					bool flag2 = !DomainManager.Character.TryGetElement_Objects(prisoner.CharId, out character) || character.IsCompletelyInfected();
					if (!flag2)
					{
						sect.RemovePrisoner(context, prisoner.CharId);
						realCount++;
						OrganizationInfo orgInfo = character.GetOrganizationInfo();
						OrganizationMemberItem orgMemberCfg = OrganizationDomain.GetOrgMemberConfig(orgTemplateId, orgInfo.Grade);
						sbyte rejoinGrade = orgMemberCfg.GetRejoinGrade();
						OrganizationInfo newOrgInfo = new OrganizationInfo(orgTemplateId, rejoinGrade, true, settlementId);
						int remaining = prisoner.Duration - (currDate - prisoner.KidnapBeginDate);
						DomainManager.Character.ChangeFavorabilityOptional(context, character, taiwu, 2500 * (int)prisoner.PunishmentSeverity * remaining / prisoner.Duration, 3);
						lifeRecordCollection.AddAristocratReleasePrisoner(taiwuId, currDate, settlementId);
						lifeRecordCollection.AddPrisonerBeReleaseByAristocrat(prisoner.CharId, currDate, taiwuId, settlementId);
						DomainManager.Organization.ChangeOrganization(context, character, newOrgInfo);
						SectCharacter sectChar = DomainManager.Organization.GetElement_SectCharacters(prisoner.CharId);
						sectChar.SetApprovedTaiwu(context, true);
						prisonRecordCollection.AddPrisonerBeReleaseByAristocrat(currDate, settlementId, prisoner.CharId, taiwuId);
						DomainManager.Extra.SetSettlementPrisonRecordCollection(context, settlementId, prisonRecordCollection);
					}
				}
				bool flag3 = realCount > 0;
				if (flag3)
				{
					DomainManager.World.GetInstantNotificationCollection().AddReleasePrisoners(settlementId, realCount);
				}
			}
		}

		// Token: 0x06001469 RID: 5225 RVA: 0x0013FA24 File Offset: 0x0013DC24
		private static void ExecuteOnClick_AristocratSkill(DataContext context, ProfessionData professionData, int index, ref ProfessionSkillArg arg)
		{
			if (index != 3)
			{
				throw new Exception(professionData.GetSkillConfig(index).Name + " is not an executable skill.");
			}
			ProfessionSkillHandle.ExecuteOnClick_AristocratSkill4(context);
		}

		// Token: 0x0600146A RID: 5226 RVA: 0x0013FA64 File Offset: 0x0013DC64
		private static bool CheckSpecialCondition_AristocratSkill(ProfessionData professionData, int index)
		{
			return index != 3 || DomainManager.Extra.CheckAristocratUltimateSpecialCondition() == 0;
		}

		// Token: 0x0600146B RID: 5227 RVA: 0x0013FA94 File Offset: 0x0013DC94
		private static bool CheckSpecialCondition_BeggarSkill(ProfessionData professionData, int index)
		{
			switch (index)
			{
			case 0:
				return ProfessionSkillHandle.CheckSpecialCondition_BeggarSkill_1(professionData);
			case 1:
				return true;
			case 3:
				return ProfessionSkillHandle.CheckSpecialCondition_BeggarSkill_4(professionData);
			}
			return true;
		}

		// Token: 0x0600146C RID: 5228 RVA: 0x0013FADC File Offset: 0x0013DCDC
		private static bool CheckSpecialCondition_BeggarSkill_1(ProfessionData professionData)
		{
			sbyte settlementType = professionData.GetSeniorityBeggarMaxSettlementType();
			return ProfessionSkillHandle.CheckSettlementBlockTypeValid(settlementType);
		}

		// Token: 0x0600146D RID: 5229 RVA: 0x0013FAFC File Offset: 0x0013DCFC
		private static bool CheckSpecialCondition_BeggarSkill_4(ProfessionData professionData)
		{
			return DomainManager.Extra.CheckBeggarUltimateSpecialCondition() == 0;
		}

		// Token: 0x0600146E RID: 5230 RVA: 0x0013FB1C File Offset: 0x0013DD1C
		private static void ExecuteOnClick_BeggarSkill(DataContext context, ProfessionData professionData, int index, ref ProfessionSkillArg arg)
		{
			if (index != 1)
			{
				if (index != 3)
				{
					throw new Exception(professionData.GetSkillConfig(index).Name + " is not an executable skill.");
				}
				ProfessionSkillHandle.ExecuteOnClick_BeggarSkill4(context, arg.CharId, arg.ItemKey);
			}
			else
			{
				ProfessionSkillHandle.ExecuteOnClick_BeggarSkill2(context);
			}
		}

		// Token: 0x0600146F RID: 5231 RVA: 0x0013FB78 File Offset: 0x0013DD78
		public static int BeggarSkill_GetBeggingMoney(DataContext context, GameData.Domains.Character.Character character)
		{
			GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
			ProfessionData professionData = DomainManager.Extra.GetProfessionData(9);
			int baseValue = 10 + 90 * professionData.GetSeniorityPercent() / 100;
			int happinessBonusFactor = (int)(-(int)taiwu.GetHappiness());
			int injuriesBonusFactor = taiwu.GetInjuries().GetSum() * 3;
			int poisonedBonusFactor = taiwu.GetPoisonMarkCount() * 9;
			CValuePercentBonus factor = happinessBonusFactor + injuriesBonusFactor + poisonedBonusFactor;
			CValuePercent factor2 = (int)ProfessionRelatedConstants.BeggarMoneyBehaviorTypeFactors[(int)character.GetBehaviorType()];
			int value = baseValue * factor * factor2;
			return Math.Min(value, character.GetResource(6) * ProfessionRelatedConstants.BeggarMoneyMaxPercent);
		}

		// Token: 0x06001470 RID: 5232 RVA: 0x0013FC24 File Offset: 0x0013DE24
		public static int BeggarSkill_GetLocationBeggingMoney(DataContext context, Location location, bool isTransferMoney = false)
		{
			MapBlockData mapBlockData = DomainManager.Map.GetBlockData(location.AreaId, location.BlockId);
			HashSet<int> characterSet = mapBlockData.CharacterSet;
			int moneyTaiwuGet = 0;
			foreach (int charId in characterSet)
			{
				GameData.Domains.Character.Character character;
				bool flag = DomainManager.Character.TryGetElement_Objects(charId, out character);
				if (flag)
				{
					int moneyValue = ProfessionSkillHandle.BeggarSkill_GetBeggingMoney(context, character);
					if (isTransferMoney)
					{
						character.ChangeResource(context, 6, -moneyValue);
					}
					moneyTaiwuGet += moneyValue;
				}
			}
			return moneyTaiwuGet;
		}

		// Token: 0x06001471 RID: 5233 RVA: 0x0013FCD4 File Offset: 0x0013DED4
		public static void BeggarSkill_AddLookingForCharacter(DataContext context, string name)
		{
			ProfessionData professionData = DomainManager.Extra.GetProfessionData(9);
			BeggarSkillsData skillsData = (BeggarSkillsData)professionData.SkillsData;
			skillsData.LookingForCharName = name;
			skillsData.AlreadyFoundCharacters.Clear();
			DomainManager.Extra.SetProfessionData(context, professionData);
		}

		// Token: 0x06001472 RID: 5234 RVA: 0x0013FD1C File Offset: 0x0013DF1C
		public static void BeggarSkill_ClearLookingForCharacter(DataContext context)
		{
			ProfessionData professionData = DomainManager.Extra.GetProfessionData(9);
			BeggarSkillsData skillsData = (BeggarSkillsData)professionData.SkillsData;
			skillsData.LookingForCharName = null;
			skillsData.AlreadyFoundCharacters.Clear();
			DomainManager.Extra.SetProfessionData(context, professionData);
		}

		// Token: 0x06001473 RID: 5235 RVA: 0x0013FD64 File Offset: 0x0013DF64
		public static string BeggarSkill_LookingForCharacter()
		{
			ProfessionData professionData = DomainManager.Extra.GetProfessionData(9);
			BeggarSkillsData skillsData = (BeggarSkillsData)professionData.SkillsData;
			return skillsData.LookingForCharName;
		}

		// Token: 0x06001474 RID: 5236 RVA: 0x0013FD98 File Offset: 0x0013DF98
		public static bool BeggarSkill_FoundMoreAlive()
		{
			ProfessionData professionData = DomainManager.Extra.GetProfessionData(9);
			BeggarSkillsData skillsData = (BeggarSkillsData)professionData.SkillsData;
			return skillsData.FoundMoreAlive;
		}

		// Token: 0x06001475 RID: 5237 RVA: 0x0013FDCC File Offset: 0x0013DFCC
		public static bool BeggarSkill_FoundMoreDead()
		{
			ProfessionData professionData = DomainManager.Extra.GetProfessionData(9);
			BeggarSkillsData skillsData = (BeggarSkillsData)professionData.SkillsData;
			return skillsData.FoundMoreDead;
		}

		// Token: 0x06001476 RID: 5238 RVA: 0x0013FE00 File Offset: 0x0013E000
		public unsafe static void ExecuteOnClick_BeggarSkill2(DataContext context)
		{
			GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
			Location centerLocation = taiwu.GetLocation();
			int range = 3;
			List<MapBlockData> neighborMapBlockList = ObjectPool<List<MapBlockData>>.Instance.Get();
			DomainManager.Map.GetLocationByDistance(centerLocation, range, range, ref neighborMapBlockList);
			MapBlockData centerBlock = *DomainManager.Map.GetAreaBlocks(centerLocation.AreaId)[(int)centerLocation.BlockId];
			List<int> characters = new List<int>();
			bool flag = centerBlock.CharacterSet != null;
			if (flag)
			{
				characters.AddRange(centerBlock.CharacterSet);
			}
			bool flag2 = characters.Count > 0;
			if (flag2)
			{
				characters.ForEach(delegate(int charId)
				{
					GameData.Domains.Character.Character character = DomainManager.Character.GetElement_Objects(charId);
					bool flag3 = character.GetId() == taiwu.GetId();
					if (!flag3)
					{
						DomainManager.Character.GroupMove(context, character, base.<ExecuteOnClick_BeggarSkill2>g__SelectRandomValidTargetLocation|1(ref neighborMapBlockList));
					}
				});
			}
			ProfessionSkillHandle.BeggarSkill_AddCurrentLocationForbiddenMapBlock(context, centerLocation);
			DomainManager.World.GetInstantNotificationCollection().AddDriveAwayPeople(centerLocation);
			ObjectPool<List<MapBlockData>>.Instance.Return(neighborMapBlockList);
		}

		// Token: 0x06001477 RID: 5239 RVA: 0x0013FEFC File Offset: 0x0013E0FC
		public static void ExecuteOnClick_BeggarSkill4(DataContext context, int charId, ItemKey itemKey)
		{
			GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
			GameData.Domains.Character.Character target = DomainManager.Character.GetElement_Objects(charId);
			ItemBase baseItem = DomainManager.Item.GetBaseItem(itemKey);
			int favorChange = baseItem.GetFavorabilityChange();
			sbyte happinessChange = baseItem.GetHappinessChange();
			ItemKey eatItem = itemKey;
			int count = 1;
			bool flag = ItemTemplateHelper.IsTianJieFuLu(itemKey.ItemType, itemKey.TemplateId);
			if (flag)
			{
				count = ItemTemplateHelper.GetTianJieFuLuCountUnit();
				eatItem = DomainManager.Item.CreateItem(context, 8, 432);
				taiwu.AddInventoryItem(context, eatItem, 1, false);
			}
			target.RemoveInventoryItem(context, itemKey, count, false, false);
			taiwu.AddEatingItem(context, eatItem, null);
			DomainManager.Item.RemoveItem(context, itemKey);
			DomainManager.Character.ChangeFavorabilityOptional(context, target, taiwu, favorChange, 0);
			DomainManager.Character.AddFavorabilityChangeInstantNotification(target, taiwu, favorChange > 0);
			target.ChangeHappiness(context, (int)happinessChange);
			LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
			int currDate = DomainManager.World.GetCurrDate();
			lifeRecordCollection.AddBeggarEatSomeoneFood(taiwu.GetId(), currDate, target.GetId(), taiwu.GetLocation(), itemKey.ItemType, itemKey.TemplateId);
		}

		// Token: 0x06001478 RID: 5240 RVA: 0x00140014 File Offset: 0x0013E214
		public static bool IsLocationForbiddenByBeggarSkill(Location location)
		{
			ProfessionData professionData = DomainManager.Extra.GetProfessionData(9);
			BeggarSkillsData skillsData = (BeggarSkillsData)professionData.SkillsData;
			return skillsData.ForbiddenLocations != null && skillsData.ForbiddenLocations.Contains(location);
		}

		// Token: 0x06001479 RID: 5241 RVA: 0x00140058 File Offset: 0x0013E258
		private static void BeggarSkill_AddCurrentLocationForbiddenMapBlock(DataContext context, Location targetLocation)
		{
			ProfessionData professionData = DomainManager.Extra.GetProfessionData(9);
			BeggarSkillsData skillsData = (BeggarSkillsData)professionData.SkillsData;
			bool flag = skillsData.ForbiddenLocations == null;
			if (flag)
			{
				skillsData.ForbiddenLocations = new List<Location>();
			}
			skillsData.ForbiddenLocations.Add(targetLocation);
			DomainManager.Extra.SetProfessionData(context, professionData);
		}

		// Token: 0x0600147A RID: 5242 RVA: 0x001400B4 File Offset: 0x0013E2B4
		private static void BeggarSkill_AdvanceMonth(DataContext context)
		{
			ProfessionSkillHandle.<>c__DisplayClass36_0 CS$<>8__locals1 = new ProfessionSkillHandle.<>c__DisplayClass36_0();
			ProfessionData professionData = DomainManager.Extra.GetProfessionData(9);
			CS$<>8__locals1.skillsData = professionData.GetSkillsData<BeggarSkillsData>();
			BeggarSkillsData skillsData = CS$<>8__locals1.skillsData;
			if (skillsData != null)
			{
				List<Location> forbiddenLocations = skillsData.ForbiddenLocations;
				if (forbiddenLocations != null)
				{
					forbiddenLocations.Clear();
				}
			}
			DomainManager.Extra.SetProfessionData(context, professionData);
			bool flag = !ProfessionSkillHandle.IsSkillUnlocked(31);
			if (!flag)
			{
				CS$<>8__locals1.skillsData.FoundMoreAlive = false;
				CS$<>8__locals1.skillsData.FoundMoreDead = false;
				bool flag2 = string.IsNullOrEmpty(CS$<>8__locals1.skillsData.LookingForCharName);
				if (!flag2)
				{
					CS$<>8__locals1.exceptions = CS$<>8__locals1.skillsData.AlreadyFoundCharacters.GetCollection();
					MonthlyEventCollection monthlyEventCollection = DomainManager.World.GetMonthlyEventCollection();
					List<GameData.Domains.Character.Character> characterResults = new List<GameData.Domains.Character.Character>();
					MapCharacterFilter.ParallelFind(new Predicate<GameData.Domains.Character.Character>(CS$<>8__locals1.<BeggarSkill_AdvanceMonth>g__CharacterPredicate|0), characterResults, 0, 135, false);
					MapCharacterFilter.FindTraveling(new Predicate<GameData.Domains.Character.Character>(CS$<>8__locals1.<BeggarSkill_AdvanceMonth>g__CharacterPredicate|0), characterResults, false);
					Predicate<GameData.Domains.Character.Character> calledByAdventure = (GameData.Domains.Character.Character character) => character.IsActiveExternalRelationState(60);
					bool flag3 = characterResults.Count > 0;
					if (flag3)
					{
						bool flag4 = characterResults.TrueForAll(calledByAdventure);
						if (flag4)
						{
							GameData.Domains.Character.Character character3 = characterResults.GetRandom(context.Random);
							int foundCharId = character3.GetId();
							monthlyEventCollection.AddBeggerSkill2TargetUnavailable(foundCharId);
							CS$<>8__locals1.skillsData.AlreadyFoundCharacters.Add(foundCharId);
							DomainManager.Extra.SetProfessionData(context, professionData);
						}
						else
						{
							characterResults.RemoveAll(calledByAdventure);
							GameData.Domains.Character.Character foundCharacter = characterResults.GetRandom(context.Random);
							monthlyEventCollection.AddBeggarSkill2TargetBrought(foundCharacter.GetId(), foundCharacter.GetLocation());
							CS$<>8__locals1.skillsData.FoundMoreAlive = (characterResults.Count > 1);
							GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
							Location targetLocation = taiwu.GetLocation();
							bool flag5 = !targetLocation.IsValid();
							if (flag5)
							{
								targetLocation = taiwu.GetValidLocation();
							}
							DomainManager.Character.GroupMove(context, foundCharacter, targetLocation);
							CS$<>8__locals1.skillsData.AlreadyFoundCharacters.Add(foundCharacter.GetId());
							DomainManager.Extra.SetProfessionData(context, professionData);
						}
					}
					else
					{
						MapCharacterFilter.FindHiddenCharacters(new Predicate<GameData.Domains.Character.Character>(CS$<>8__locals1.<BeggarSkill_AdvanceMonth>g__CharacterPredicate|0), characterResults, false);
						MapCharacterFilter.FindKidnappedCharacters(new Predicate<GameData.Domains.Character.Character>(CS$<>8__locals1.<BeggarSkill_AdvanceMonth>g__CharacterPredicate|0), characterResults, false);
						bool flag6 = characterResults.Count > 0;
						if (flag6)
						{
							GameData.Domains.Character.Character character2 = characterResults.GetRandom(context.Random);
							int foundCharId2 = character2.GetId();
							monthlyEventCollection.AddBeggerSkill2TargetUnavailable(foundCharId2);
							CS$<>8__locals1.skillsData.AlreadyFoundCharacters.Add(foundCharId2);
							DomainManager.Extra.SetProfessionData(context, professionData);
						}
						else
						{
							List<Grave> foundGraves = new List<Grave>();
							DomainManager.Character.FindGrave(new Predicate<Grave>(CS$<>8__locals1.<BeggarSkill_AdvanceMonth>g__GravePredicate|2), foundGraves);
							bool flag7 = foundGraves.Count > 0;
							if (flag7)
							{
								Grave targetGrave = foundGraves.GetRandom(context.Random);
								int foundCharId3 = targetGrave.GetId();
								monthlyEventCollection.AddBeggarSkill2TargetDead(foundCharId3, targetGrave.GetLocation());
								CS$<>8__locals1.skillsData.FoundMoreDead = (foundGraves.Count > 0);
								CS$<>8__locals1.skillsData.AlreadyFoundCharacters.Add(foundCharId3);
								DomainManager.Extra.SetProfessionData(context, professionData);
							}
							else
							{
								monthlyEventCollection.AddBeggarSkill2TargetNoneExistent(CS$<>8__locals1.skillsData.LookingForCharName);
							}
						}
					}
				}
			}
		}

		// Token: 0x0600147B RID: 5243 RVA: 0x001403FC File Offset: 0x0013E5FC
		private static bool CheckSettlementBlockTypeValid(sbyte settlementType)
		{
			Location taiwuLocation = DomainManager.Taiwu.GetTaiwu().GetLocation();
			bool flag = !taiwuLocation.IsValid();
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				MapBlockData block = DomainManager.Map.GetBlock(taiwuLocation);
				MapBlockData rootBlock = block.GetRootBlock();
				bool flag2 = !rootBlock.IsCityTown();
				if (flag2)
				{
					result = false;
				}
				else
				{
					switch (settlementType)
					{
					case 0:
						result = (rootBlock.BlockSubType == EMapBlockSubType.Village || rootBlock.BlockSubType == EMapBlockSubType.TaiwuCun);
						break;
					case 1:
						result = (rootBlock.BlockType != EMapBlockType.Sect && rootBlock.BlockType != EMapBlockType.City && rootBlock.BlockSubType != EMapBlockSubType.Town);
						break;
					case 2:
						result = (rootBlock.BlockType == EMapBlockType.Town);
						break;
					case 3:
						result = true;
						break;
					default:
						result = false;
						break;
					}
				}
			}
			return result;
		}

		// Token: 0x0600147C RID: 5244 RVA: 0x001404D0 File Offset: 0x0013E6D0
		private static void ExecuteOnClick_BuddhistMonkSkill(DataContext context, ProfessionData professionData, int index, ref ProfessionSkillArg arg)
		{
			if (index != 3)
			{
				throw new Exception(professionData.GetSkillConfig(index).Name + " is not an executable skill.");
			}
			ProfessionSkillHandle.BuddhistMonkSkill_SetSamsaraFeature(context, DomainManager.Taiwu.GetTaiwu().GetId(), arg.EffectId);
		}

		// Token: 0x0600147D RID: 5245 RVA: 0x00140524 File Offset: 0x0013E724
		public static void BuddhistMonkSkill_SelectDirectedSamsara(DataContext context, int motherId, int reincarnatedCharId)
		{
			ProfessionData professionData = DomainManager.Extra.GetProfessionData(6);
			BuddhistMonkSkillsData skillsData = professionData.GetSkillsData<BuddhistMonkSkillsData>();
			skillsData.OfflineAddDirectedSamsara(motherId, reincarnatedCharId);
			GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
			DomainManager.Extra.SetProfessionData(context, professionData);
			GameData.Domains.Character.Character mother = DomainManager.Character.GetElement_Objects(motherId);
			PregnantState pregnantState;
			bool flag = !DomainManager.Character.TryGetPregnantState(motherId, out pregnantState);
			if (flag)
			{
				DomainManager.Character.RemovePregnantLock(context, motherId);
				DomainManager.Character.MakePregnantWithoutMale(context, mother);
			}
			LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
			int currDate = DomainManager.World.GetCurrDate();
			lifeRecordCollection.AddTaiwuReincarnationPregnancy(motherId, currDate, mother.GetLocation());
			lifeRecordCollection.AddTaiwuReincarnation(reincarnatedCharId, currDate, motherId, taiwu.GetLocation(), mother.GetOrganizationInfo().SettlementId);
		}

		// Token: 0x0600147E RID: 5246 RVA: 0x001405EC File Offset: 0x0013E7EC
		public static void BuddhistMonkSkill_TryRemoveDirectedSamsara(DataContext context, int motherId, bool addMonthlyEvent = true)
		{
			ProfessionSkillHandle.<>c__DisplayClass40_0 CS$<>8__locals1 = new ProfessionSkillHandle.<>c__DisplayClass40_0();
			ProfessionData professionData = DomainManager.Extra.GetProfessionData(6);
			BuddhistMonkSkillsData skillsData = professionData.GetSkillsData<BuddhistMonkSkillsData>();
			CS$<>8__locals1.charId = skillsData.GetDirectedSamsara(motherId);
			bool flag = skillsData.OfflineRemoveDirectedSamsara(motherId);
			if (flag)
			{
				DomainManager.Extra.SetProfessionData(context, professionData);
				bool flag2 = !addMonthlyEvent;
				if (!flag2)
				{
					bool flag3 = DomainManager.World.GetAdvancingMonthState() != 0;
					if (flag3)
					{
						MonthlyEventCollection monthlyEventCollection = DomainManager.World.GetMonthlyEventCollection();
						int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
						monthlyEventCollection.AddTaiwuComingDefeated(taiwuCharId, CS$<>8__locals1.charId);
					}
					else
					{
						Events.RegisterHandler_PostAdvanceMonthBegin(new Events.OnPostAdvanceMonthBegin(CS$<>8__locals1.<BuddhistMonkSkill_TryRemoveDirectedSamsara>g__AddTaiwuComingEvent|0));
					}
				}
			}
		}

		// Token: 0x0600147F RID: 5247 RVA: 0x00140698 File Offset: 0x0013E898
		public static bool BuddhistMonkSkill_IsDirectedSamsaraMother(int motherId)
		{
			ProfessionData professionData = DomainManager.Extra.GetProfessionData(6);
			BuddhistMonkSkillsData skillsData = professionData.GetSkillsData<BuddhistMonkSkillsData>();
			return skillsData.GetDirectedSamsara(motherId) >= 0;
		}

		// Token: 0x06001480 RID: 5248 RVA: 0x001406CC File Offset: 0x0013E8CC
		public static bool BuddhistMonkSkill_IsDirectedSamsaraCharacter(int reincarnatedCharId)
		{
			ProfessionData professionData = DomainManager.Extra.GetProfessionData(6);
			BuddhistMonkSkillsData skillsData = professionData.GetSkillsData<BuddhistMonkSkillsData>();
			return skillsData.IsDirectedSamsaraCharacter(reincarnatedCharId);
		}

		// Token: 0x06001481 RID: 5249 RVA: 0x001406F8 File Offset: 0x0013E8F8
		public static void BuddhistMonkSkill_SetSamsaraFeature(DataContext context, int reincarnatedCharId, short featureID)
		{
			bool flag = reincarnatedCharId == -1;
			if (!flag)
			{
				ProfessionData professionData = DomainManager.Extra.GetProfessionData(6);
				BuddhistMonkSkillsData skillsData = professionData.GetSkillsData<BuddhistMonkSkillsData>();
				skillsData.OfflineAddSamsaraFeature(reincarnatedCharId, featureID);
				DomainManager.Extra.SetProfessionData(context, professionData);
			}
		}

		// Token: 0x06001482 RID: 5250 RVA: 0x0014073C File Offset: 0x0013E93C
		public static bool BuddhistMonkSkill_TryGetSamsaraFeature(int reincarnatedCharId, out short featureID)
		{
			ProfessionData professionData = DomainManager.Extra.GetProfessionData(6);
			BuddhistMonkSkillsData skillsData = professionData.GetSkillsData<BuddhistMonkSkillsData>();
			return skillsData.TryGetSamaraFeature(reincarnatedCharId, out featureID);
		}

		// Token: 0x06001483 RID: 5251 RVA: 0x0014076C File Offset: 0x0013E96C
		public static bool BuddhistMonkSkill_RemoveSamsaraFeature(DataContext context, int reincarnatedCharId)
		{
			ProfessionData professionData = DomainManager.Extra.GetProfessionData(6);
			BuddhistMonkSkillsData skillsData = professionData.GetSkillsData<BuddhistMonkSkillsData>();
			bool flag = skillsData.OfflineRemoveSamsaraFeature(reincarnatedCharId);
			bool result;
			if (flag)
			{
				DomainManager.Extra.SetProfessionData(context, professionData);
				result = true;
			}
			else
			{
				result = false;
			}
			return result;
		}

		// Token: 0x06001484 RID: 5252 RVA: 0x001407B0 File Offset: 0x0013E9B0
		public static int BuddhistMonkSkill_GetDirectedSamsaraMother(int reincarnatedCharId)
		{
			ProfessionData professionData = DomainManager.Extra.GetProfessionData(6);
			BuddhistMonkSkillsData skillsData = professionData.GetSkillsData<BuddhistMonkSkillsData>();
			return skillsData.GetDirectedSamsaraMother(reincarnatedCharId);
		}

		// Token: 0x06001485 RID: 5253 RVA: 0x001407DC File Offset: 0x0013E9DC
		private static void BuddhistMonkSkill_ClearSavedSoulCount(DataContext context)
		{
			ProfessionData professionData = DomainManager.Extra.GetProfessionData(6);
			BuddhistMonkSkillsData skillsData = professionData.GetSkillsData<BuddhistMonkSkillsData>();
			skillsData.OfflineClearSavedSoulsCount();
			DomainManager.Extra.SetProfessionData(context, professionData);
		}

		// Token: 0x06001486 RID: 5254 RVA: 0x00140814 File Offset: 0x0013EA14
		private static void UnpackCrossArchiveProfession_BuddhistMonk(DataContext context)
		{
			ProfessionData professionData = DomainManager.Extra.GetProfessionData(6);
			BuddhistMonkSkillsData skillsData = professionData.GetSkillsData<BuddhistMonkSkillsData>();
			skillsData.OfflineClearDirectedSamsara();
			DomainManager.Extra.SetProfessionData(context, professionData);
		}

		// Token: 0x06001487 RID: 5255 RVA: 0x0014084C File Offset: 0x0013EA4C
		private static bool CheckSpecialCondition_BuddhistMonkSkill(ProfessionData professionData, int index)
		{
			return index != 3 || ProfessionSkillHandle.CheckSpecialCondition_BuddhistMonkSkill_3(professionData);
		}

		// Token: 0x06001488 RID: 5256 RVA: 0x00140874 File Offset: 0x0013EA74
		private static bool CheckSpecialCondition_BuddhistMonkSkill_3(ProfessionData professionData)
		{
			BuddhistMonkSkillsData skillsData = professionData.GetSkillsData<BuddhistMonkSkillsData>();
			return skillsData.GetSavedSoulsCount() >= 100;
		}

		// Token: 0x06001489 RID: 5257 RVA: 0x0014089C File Offset: 0x0013EA9C
		private static bool CheckSpecialCondition_CapitalistSkill(ProfessionData professionData, int index)
		{
			return true;
		}

		// Token: 0x0600148A RID: 5258 RVA: 0x001408AF File Offset: 0x0013EAAF
		public static void OpenInvestCaravan()
		{
			GameDataBridge.AddDisplayEvent(DisplayEventType.OpenInvestCaravan);
		}

		// Token: 0x0600148B RID: 5259 RVA: 0x001408BC File Offset: 0x0013EABC
		private static void ExecuteOnClick_CivilianSkill(DataContext context, ProfessionData professionData, int index, ref ProfessionSkillArg arg)
		{
			if (index != 1)
			{
				throw new Exception(professionData.GetSkillConfig(index).Name + " is not an executable skill.");
			}
			ProfessionSkillHandle.ExecuteOnClick_CivilianSkill_1(context, professionData);
		}

		// Token: 0x0600148C RID: 5260 RVA: 0x001408FC File Offset: 0x0013EAFC
		private static void ExecuteOnClick_CivilianSkill_1(DataContext context, ProfessionData professionData)
		{
			GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
			int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
			Location location = taiwu.GetLocation();
			bool flag = !location.IsValid();
			if (!flag)
			{
				LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
				int currDate = DomainManager.World.GetCurrDate();
				int count = 0;
				int limit = ProfessionData.GetSeniorityCivilianSeverHatredLimit(professionData.Seniority);
				List<int> availableCharIds = new List<int>();
				List<ValueTuple<int, int>> pairs = new List<ValueTuple<int, int>>();
				MapBlockData block = DomainManager.Map.GetBlock(location);
				bool flag2 = block.CharacterSet != null && block.CharacterSet.Count != 0;
				if (flag2)
				{
					foreach (int charId in block.CharacterSet)
					{
						availableCharIds.Add(charId);
					}
				}
				foreach (int charId2 in DomainManager.Taiwu.GetGroupCharIds().GetCollection())
				{
					bool flag3 = charId2 != taiwuCharId;
					if (flag3)
					{
						availableCharIds.Add(charId2);
					}
				}
				foreach (int charId3 in availableCharIds)
				{
					bool flag4 = !DomainManager.Character.GetRelatedCharIds(charId3, 32768).Contains(taiwuCharId);
					if (!flag4)
					{
						pairs.Add(new ValueTuple<int, int>(charId3, taiwuCharId));
						count++;
						bool flag5 = count >= limit;
						if (flag5)
						{
							break;
						}
					}
				}
				bool flag6 = count < limit;
				if (flag6)
				{
					foreach (int charId4 in availableCharIds)
					{
						HashSet<int> characterSet = DomainManager.Character.GetRelatedCharIds(charId4, 32768);
						foreach (int charIdB in availableCharIds)
						{
							HashSet<int> characterSetB = DomainManager.Character.GetRelatedCharIds(charIdB, 32768);
							bool flag7 = characterSet.Contains(charIdB) && !pairs.Contains(new ValueTuple<int, int>(charId4, charIdB)) && characterSetB.Contains(charId4) && !pairs.Contains(new ValueTuple<int, int>(charIdB, charId4));
							if (flag7)
							{
								pairs.Add(new ValueTuple<int, int>(charId4, charIdB));
								pairs.Add(new ValueTuple<int, int>(charIdB, charId4));
								count++;
								bool flag8 = count >= limit;
								if (flag8)
								{
									break;
								}
							}
						}
						bool flag9 = count >= limit;
						if (flag9)
						{
							break;
						}
					}
				}
				bool flag10 = count < limit;
				if (flag10)
				{
					foreach (int charId5 in availableCharIds)
					{
						HashSet<int> characterSet2 = DomainManager.Character.GetRelatedCharIds(charId5, 32768);
						foreach (int charIdB2 in availableCharIds)
						{
							bool flag11 = characterSet2.Contains(charIdB2) && !pairs.Contains(new ValueTuple<int, int>(charId5, charIdB2));
							if (flag11)
							{
								pairs.Add(new ValueTuple<int, int>(charId5, charIdB2));
								count++;
								bool flag12 = count >= limit;
								if (flag12)
								{
									break;
								}
							}
						}
						bool flag13 = count >= limit;
						if (flag13)
						{
							break;
						}
					}
				}
				foreach (ValueTuple<int, int> valueTuple in pairs)
				{
					int charIdA = valueTuple.Item1;
					int charIdB3 = valueTuple.Item2;
					GameData.Domains.Character.Character characterA = DomainManager.Character.GetElement_Objects(charIdA);
					GameData.Domains.Character.Character characterB = DomainManager.Character.GetElement_Objects(charIdB3);
					ProfessionFormulaItem formulaItem = ProfessionFormula.Instance[67];
					DomainManager.Extra.ChangeProfessionSeniority(context, 10, formulaItem.Calculate((int)(characterA.GetInteractionGrade() + characterB.GetInteractionGrade())), true, false);
					lifeRecordCollection.AddForgiveForCivilianSkill(charIdA, currDate, taiwuCharId, location, charIdB3);
					DomainManager.Character.ChangeRelationType(context, charIdA, charIdB3, 32768, 0);
					characterA.ChangeHappiness(context, 20);
					characterA.RecordFameAction(context, 4, charIdB3, 1, true);
					BasePrioritizedAction action;
					bool flag14 = DomainManager.Character.TryGetCharacterPrioritizedAction(charIdA, out action) && action.ActionType == 8;
					if (flag14)
					{
						DomainManager.Character.RemoveCharacterPrioritizedAction(context, charIdA);
					}
				}
				bool flag15 = count > 0;
				if (flag15)
				{
					taiwu.RecordFameAction(context, 12, taiwuCharId, (short)count, true);
					lifeRecordCollection.AddCivilianSkillDissolveResentment(taiwuCharId, currDate, location);
					DomainManager.World.GetInstantNotificationCollection().AddQuenchHatred(pairs.Count);
					DomainManager.Map.SetBlockData(context, block);
				}
			}
		}

		// Token: 0x0600148D RID: 5261 RVA: 0x00140EE8 File Offset: 0x0013F0E8
		public static void CivilianSkill_MakeCharacterLeaveSect(DataContext context, GameData.Domains.Character.Character character)
		{
			OrganizationInfo oriOrgInfo = character.GetOrganizationInfo();
			Tester.Assert(OrganizationDomain.IsSect(oriOrgInfo.OrgTemplateId), "$OrganizationDomain.IsSect({oriOrgInfo.OrgTemplateId})");
			Settlement oriSettlement = DomainManager.Organization.GetSettlement(oriOrgInfo.SettlementId);
			short oriAreaId = oriSettlement.GetLocation().AreaId;
			sbyte stateId = DomainManager.Map.GetStateIdByAreaId(oriAreaId);
			sbyte retireGrade = Organization.Instance[oriOrgInfo.OrgTemplateId].RetireGrade;
			List<short> settlementIds = new List<short>();
			DomainManager.Map.GetStateSettlementIds(stateId, settlementIds, true, false);
			settlementIds.Remove(DomainManager.Taiwu.GetTaiwuVillageSettlementId());
			bool flag = retireGrade == 1;
			if (flag)
			{
				settlementIds.RemoveAll((short settlementId) => DomainManager.Organization.GetSettlement(settlementId).GetOrgTemplateId() == 36);
			}
			short settlementId2 = settlementIds.GetRandom(context.Random);
			Settlement settlement = DomainManager.Organization.GetSettlement(settlementId2);
			OrganizationInfo orgInfo = new OrganizationInfo(settlement.GetOrgTemplateId(), retireGrade, true, settlementId2);
			int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
			int currDate = DomainManager.World.GetCurrDate();
			Location location = DomainManager.Taiwu.GetTaiwu().GetLocation();
			LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
			lifeRecordCollection.AddPersuadeWithdrawlFromOrganization(taiwuCharId, currDate, character.GetId(), location);
			DomainManager.Organization.ChangeOrganization(context, character, orgInfo);
			int taiwuId = DomainManager.Taiwu.GetTaiwuCharId();
			DomainManager.World.GetInstantNotificationCollection().AddProfessionCivilianSkill2(character.GetId(), taiwuId, orgInfo.OrgTemplateId, orgInfo.Grade, true, character.GetGender());
			bool flag2 = location.IsValid();
			if (flag2)
			{
				MapBlockData block = DomainManager.Map.GetBlock(location);
				DomainManager.Map.SetBlockData(context, block);
			}
			int limit = ProfessionData.GetSeniorityCivilianAddHatredLimit(DomainManager.Extra.GetProfessionData(10).Seniority);
			int count = 0;
			OrgMemberCollection members = oriSettlement.GetMembers();
			for (sbyte grade = 8; grade >= 0; grade -= 1)
			{
				foreach (int orgMemberId in members.GetMembers(grade))
				{
					DomainManager.Character.TryAddAndApplyOneWayRelation(context, orgMemberId, character.GetId(), 32768);
					count++;
					bool flag3 = count >= limit;
					if (flag3)
					{
						break;
					}
				}
				bool flag4 = count >= limit;
				if (flag4)
				{
					break;
				}
			}
		}

		// Token: 0x0600148E RID: 5262 RVA: 0x00141168 File Offset: 0x0013F368
		private static bool CheckSpecialCondition_CivilianSkill(ProfessionData professionData, int index)
		{
			return index != 1 || ProfessionSkillHandle.CheckSpecialCondition_CivilianSkill_1(professionData);
		}

		// Token: 0x0600148F RID: 5263 RVA: 0x00141190 File Offset: 0x0013F390
		private static bool CheckSpecialCondition_CivilianSkill_1(ProfessionData professionData)
		{
			GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
			int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
			Location location = taiwu.GetLocation();
			bool flag = !location.IsValid();
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				MapBlockData block = DomainManager.Map.GetBlock(location);
				List<int> availableCharIds = new List<int>();
				bool flag2 = block.CharacterSet != null && block.CharacterSet.Count != 0;
				if (flag2)
				{
					foreach (int charId in block.CharacterSet)
					{
						availableCharIds.Add(charId);
					}
				}
				foreach (int charId2 in DomainManager.Taiwu.GetGroupCharIds().GetCollection())
				{
					bool flag3 = charId2 != taiwuCharId;
					if (flag3)
					{
						availableCharIds.Add(charId2);
					}
				}
				foreach (int charId3 in availableCharIds)
				{
					HashSet<int> characterSet = DomainManager.Character.GetRelatedCharIds(charId3, 32768);
					bool flag4 = characterSet.Contains(taiwuCharId);
					if (flag4)
					{
						return true;
					}
					foreach (int charIdB in availableCharIds)
					{
						bool flag5 = characterSet.Contains(charIdB);
						if (flag5)
						{
							return true;
						}
					}
				}
				result = false;
			}
			return result;
		}

		// Token: 0x06001490 RID: 5264 RVA: 0x00141370 File Offset: 0x0013F570
		private static void ExecuteOnClick_CraftSkill(DataContext context, ProfessionData professionData, int index, ref ProfessionSkillArg arg)
		{
			if (index != 2)
			{
				throw new Exception(professionData.GetSkillConfig(index).Name + " is not an executable skill.");
			}
			ProfessionSkillHandle.ExecuteOnClick_CraftSkill_2(context, professionData, ref arg);
		}

		// Token: 0x06001491 RID: 5265 RVA: 0x001413B0 File Offset: 0x0013F5B0
		public static void ExecuteOnClick_CraftSkill_2(DataContext context, ProfessionData professionData, ref ProfessionSkillArg arg)
		{
			EquipmentBase baseEquipment = DomainManager.Item.GetBaseEquipment(arg.ItemKey);
			baseEquipment.ApplyDurabilityEquipmentEffectChange(context, (int)baseEquipment.GetEquipmentEffectId(), (int)arg.EffectId);
			baseEquipment.SetEquipmentEffectId(arg.EffectId, context);
			ItemDisplayData itemData = DomainManager.Item.GetItemDisplayData(baseEquipment.GetItemKey(), DomainManager.Taiwu.GetTaiwuCharId());
			List<ItemDisplayData> itemDisplayDataList = new List<ItemDisplayData>
			{
				itemData
			};
			GameDataBridge.AddDisplayEvent<List<ItemDisplayData>, bool, bool>(DisplayEventType.OpenGetItem_Item, itemDisplayDataList, false, false);
		}

		// Token: 0x06001492 RID: 5266 RVA: 0x00141428 File Offset: 0x0013F628
		public static void OpenSetEquipmentEffect()
		{
			GameDataBridge.AddDisplayEvent(DisplayEventType.OpenSetEquipmentEffect);
		}

		// Token: 0x06001493 RID: 5267 RVA: 0x00141434 File Offset: 0x0013F634
		public static int GetAddSeniority_CraftSkill_0(sbyte itemType, short templateId)
		{
			int value = ItemTemplateHelper.GetBaseValue(itemType, templateId);
			return value / 100;
		}

		// Token: 0x06001494 RID: 5268 RVA: 0x00141454 File Offset: 0x0013F654
		public static int GetRefineBonus_CraftSkill_2(int refineBonus, int equippedCharId)
		{
			GameData.Domains.Character.Character character;
			bool flag = (!DomainManager.Character.TryGetElement_Objects(equippedCharId, out character) || equippedCharId == DomainManager.Taiwu.GetTaiwuCharId()) && DomainManager.Extra.IsProfessionalSkillUnlockedAndEquipped(62);
			if (flag)
			{
				refineBonus = refineBonus * 150 / 100;
			}
			return refineBonus;
		}

		// Token: 0x06001495 RID: 5269 RVA: 0x001414A4 File Offset: 0x0013F6A4
		private static void ExecuteOnClick_DoctorSkill(DataContext context, ProfessionData professionData, int skillIndex, ref ProfessionSkillArg arg)
		{
			if (skillIndex != 1)
			{
				throw new Exception(professionData.GetSkillConfig(skillIndex).Name + " is not an executable skill.");
			}
			ProfessionSkillHandle.ExecuteOnClick_DoctorSkill_1(context, professionData);
		}

		// Token: 0x06001496 RID: 5270 RVA: 0x001414E4 File Offset: 0x0013F6E4
		private static void ExecuteOnClick_DoctorSkill_1(DataContext context, ProfessionData professionData)
		{
			ProfessionSkillHandle.<>c__DisplayClass64_0 CS$<>8__locals1;
			CS$<>8__locals1.context = context;
			CS$<>8__locals1.professionData = professionData;
			CS$<>8__locals1.taiwu = DomainManager.Taiwu.GetTaiwu();
			short medicineAttainment = CS$<>8__locals1.taiwu.GetLifeSkillAttainment(8);
			short toxicologyAttainment = CS$<>8__locals1.taiwu.GetLifeSkillAttainment(9);
			CS$<>8__locals1.disorderOfQiDelta = (short)(-(short)Math.Clamp((int)((medicineAttainment + toxicologyAttainment) * 5), (int)DisorderLevelOfQi.MinValue, (int)DisorderLevelOfQi.MaxValue));
			CS$<>8__locals1.location = CS$<>8__locals1.taiwu.GetLocation();
			MapBlockData settlementBlock = DomainManager.Map.GetBelongSettlementBlock(CS$<>8__locals1.location);
			int treatCount = 0;
			int maxGrade = CS$<>8__locals1.professionData.GetSeniorityOrgGrade();
			List<short> blockIds = new List<short>();
			DomainManager.Map.GetSettlementBlocks(settlementBlock.AreaId, settlementBlock.BlockId, blockIds);
			foreach (short blockId in blockIds)
			{
				MapBlockData block = DomainManager.Map.GetBlock(settlementBlock.AreaId, blockId);
				bool flag = block.CharacterSet == null;
				if (!flag)
				{
					foreach (int charId in block.CharacterSet)
					{
						GameData.Domains.Character.Character character = DomainManager.Character.GetElement_Objects(charId);
						bool flag2 = (int)character.GetOrganizationInfo().Grade > maxGrade;
						if (!flag2)
						{
							bool flag3 = character.GetLegendaryBookOwnerState() >= 2;
							if (!flag3)
							{
								bool flag4 = ProfessionSkillHandle.<ExecuteOnClick_DoctorSkill_1>g__TryTreatCharacter|64_0(character, ref CS$<>8__locals1);
								if (flag4)
								{
									treatCount++;
								}
							}
						}
					}
				}
			}
			HashSet<int> taiwuGroup = DomainManager.Taiwu.GetGroupCharIds().GetCollection();
			foreach (int charId2 in taiwuGroup)
			{
				bool flag5 = charId2 == CS$<>8__locals1.taiwu.GetId();
				if (!flag5)
				{
					GameData.Domains.Character.Character character2 = DomainManager.Character.GetElement_Objects(charId2);
					bool flag6 = ProfessionSkillHandle.<ExecuteOnClick_DoctorSkill_1>g__TryTreatCharacter|64_0(character2, ref CS$<>8__locals1);
					if (flag6)
					{
						treatCount++;
					}
				}
			}
			CS$<>8__locals1.taiwu.RecordFameAction(CS$<>8__locals1.context, 24, -1, 1, true);
			LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
			int currDate = DomainManager.World.GetCurrDate();
			lifeRecordCollection.AddFreeMedicalConsultation(CS$<>8__locals1.taiwu.GetId(), currDate, CS$<>8__locals1.location);
			int taiwuId = DomainManager.Taiwu.GetTaiwuCharId();
			DomainManager.World.GetInstantNotificationCollection().AddProfessionDoctorSkill1(taiwuId, CS$<>8__locals1.location, treatCount);
		}

		// Token: 0x06001497 RID: 5271 RVA: 0x001417A0 File Offset: 0x0013F9A0
		private static bool CheckSpecialCondition_DoctorSkill(ProfessionData professionData, int index)
		{
			return index != 1 || ProfessionSkillHandle.CheckSpecialCondition_DoctorSkill_1(professionData);
		}

		// Token: 0x06001498 RID: 5272 RVA: 0x001417C8 File Offset: 0x0013F9C8
		private static bool CheckSpecialCondition_DoctorSkill_1(ProfessionData professionData)
		{
			sbyte settlementType = professionData.GetSeniorityDoctorMaxSettlementType();
			return ProfessionSkillHandle.CheckSettlementBlockTypeValid(settlementType);
		}

		// Token: 0x06001499 RID: 5273 RVA: 0x001417E8 File Offset: 0x0013F9E8
		private static void ExecuteOnClick_DukeSkill(DataContext context, ProfessionData professionData, int index, ref ProfessionSkillArg arg)
		{
			if (index != 2)
			{
				if (index != 3)
				{
					throw new Exception(professionData.GetSkillConfig(index).Name + " is not an executable skill.");
				}
				ProfessionSkillHandle.ExecuteOnClick_DukeSkill_3(context, professionData, ref arg);
			}
		}

		// Token: 0x0600149A RID: 5274 RVA: 0x0014182F File Offset: 0x0013FA2F
		private static void ExecuteOnClick_DukeSkill_3(DataContext context, ProfessionData professionData, ref ProfessionSkillArg arg)
		{
			ProfessionSkillHandle.GetCricketBlocks(context, professionData);
		}

		// Token: 0x0600149B RID: 5275 RVA: 0x0014183C File Offset: 0x0013FA3C
		private static void GetCricketBlocks(DataContext context, ProfessionData professionData)
		{
			TaiwuDomain taiwuDomain = DomainManager.Taiwu;
			GameData.Domains.Character.Character taiwuChar;
			bool flag = DomainManager.Character.TryGetElement_Objects(taiwuDomain.GetTaiwuCharId(), out taiwuChar);
			if (flag)
			{
				List<MapBlockData> blocks = new List<MapBlockData>();
				Location location = taiwuChar.GetLocation();
				MapDomain mapDomain = DomainManager.Map;
				mapDomain.GetNeighborBlocks(location.AreaId, location.BlockId, blocks, 3);
				List<MapBlockData> effectBlocks = new List<MapBlockData>();
				bool flag2 = blocks.Count > 0;
				if (flag2)
				{
					CollectionUtils.Shuffle<MapBlockData>(context.Random, blocks);
					blocks.RemoveRange(0, blocks.Count / 2);
					foreach (MapBlockData block2 in blocks)
					{
						Location blockLocation = block2.GetLocation();
						bool flag3 = mapDomain.LocationHasCricket(context, blockLocation);
						if (!flag3)
						{
							effectBlocks.Add(block2);
						}
					}
					taiwuDomain.SetCricketLuckPoint(taiwuDomain.GetCricketLuckPoint() + 300, context);
				}
				ProfessionSkillArg professionSkillArg2 = new ProfessionSkillArg();
				professionSkillArg2.ProfessionId = 17;
				professionSkillArg2.SkillId = 70;
				professionSkillArg2.IsSuccess = true;
				professionSkillArg2.EffectBlocks = (from block in effectBlocks
				select block.BlockId).ToList<short>();
				ProfessionSkillArg professionSkillArg = professionSkillArg2;
				GameDataBridge.AddDisplayEvent<ProfessionSkillArg, bool>(DisplayEventType.ConfirmSkillExecuteAndPlayAnim, professionSkillArg, false);
			}
		}

		// Token: 0x0600149C RID: 5276 RVA: 0x001419A4 File Offset: 0x0013FBA4
		private static bool CheckSpecialCondition_DukeSkill(ProfessionData professionData, int skillIndex)
		{
			return skillIndex != 3 || ProfessionSkillHandle.CheckSpecialCondition_DukeSkill_3(professionData);
		}

		// Token: 0x0600149D RID: 5277 RVA: 0x001419CC File Offset: 0x0013FBCC
		private static bool CheckSpecialCondition_DukeSkill_3(ProfessionData professionData)
		{
			TaiwuDomain taiwuDomain = DomainManager.Taiwu;
			GameData.Domains.Character.Character taiwuChar;
			bool flag = DomainManager.Character.TryGetElement_Objects(taiwuDomain.GetTaiwuCharId(), out taiwuChar);
			return flag && taiwuChar.GetLocation().IsValid();
		}

		// Token: 0x0600149E RID: 5278 RVA: 0x00141A10 File Offset: 0x0013FC10
		public static bool DukeSkill_CheckCharacterHasTitle(int charId, ProfessionData professionData)
		{
			Tester.Assert(professionData.SkillsData is DukeSkillsData, "");
			DukeSkillsData duke = (DukeSkillsData)professionData.SkillsData;
			return duke.CharacterHasTitle(charId);
		}

		// Token: 0x0600149F RID: 5279 RVA: 0x00141A50 File Offset: 0x0013FC50
		public static short DukeSkill_GetTitleFromOwner(int charId, ProfessionData professionData)
		{
			Tester.Assert(professionData.SkillsData is DukeSkillsData, "");
			DukeSkillsData duke = (DukeSkillsData)professionData.SkillsData;
			return duke.GetTitleFromOwner(charId);
		}

		// Token: 0x060014A0 RID: 5280 RVA: 0x00141A90 File Offset: 0x0013FC90
		public static int DukeSkill_GetOwnerOfTitle(short templateId, ProfessionData professionData)
		{
			Tester.Assert(professionData.SkillsData is DukeSkillsData, "");
			DukeSkillsData duke = (DukeSkillsData)professionData.SkillsData;
			return duke.GetOwnerOfTitle(templateId);
		}

		// Token: 0x060014A1 RID: 5281 RVA: 0x00141AD0 File Offset: 0x0013FCD0
		public static void DukeSkill_AddCharacterTitle(DataContext context, ProfessionData professionData, int charId, short templateId)
		{
			Tester.Assert(professionData.SkillsData is DukeSkillsData, "");
			DukeSkillsData duke = (DukeSkillsData)professionData.SkillsData;
			duke.OfflineAssignTitleToCharacter(context.Random, templateId, charId);
			DomainManager.Extra.SetProfessionData(context, professionData);
			DomainManager.Extra.AddCharacterProfessionExtraTitle(context, charId, templateId);
			DomainManager.Organization.TryRemoveBounty(context, charId);
		}

		// Token: 0x060014A2 RID: 5282 RVA: 0x00141B3C File Offset: 0x0013FD3C
		public static bool DukeSkill_RemoveCharacterTitle(DataContext context, ProfessionData professionData, int charId)
		{
			Tester.Assert(professionData.SkillsData is DukeSkillsData, "");
			DukeSkillsData duke = (DukeSkillsData)professionData.SkillsData;
			short templateId = duke.OfflineRemoveTitleFromCharacter(charId);
			bool flag = templateId == -1;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				DomainManager.World.GetInstantNotifications().AddResignationPosition(charId);
				DomainManager.Extra.SetProfessionData(context, professionData);
				DomainManager.Extra.RemoveCharacterProfessionExtraTitle(context, charId, templateId);
				result = true;
			}
			return result;
		}

		// Token: 0x060014A3 RID: 5283 RVA: 0x00141BB4 File Offset: 0x0013FDB4
		public static void DukeSkill_ClearAllTitle(DataContext context, ProfessionData professionData)
		{
			Tester.Assert(professionData.SkillsData is DukeSkillsData, "");
			DukeSkillsData duke = (DukeSkillsData)professionData.SkillsData;
			foreach (ValueTuple<int, short> dso in duke.GetAllOwners())
			{
				DomainManager.Extra.RemoveCharacterProfessionExtraTitle(context, dso.Item1, dso.Item2);
			}
			duke.OfflineClearAllTitles();
			DomainManager.Extra.SetProfessionData(context, professionData);
		}

		// Token: 0x060014A4 RID: 5284 RVA: 0x00141C50 File Offset: 0x0013FE50
		public static ItemKey DukeSkill_GetNewCricket(DataContext context, int charId)
		{
			ProfessionData professionData = DomainManager.Extra.GetProfessionData(17);
			DukeSkillsData dukeSkillData = professionData.GetSkillsData<DukeSkillsData>();
			ItemKey itemKey = ProfessionSkillHandle.OfflineCreateCricketAndModifyLuckPoint(context, dukeSkillData, charId);
			DomainManager.Extra.SetProfessionData(context, professionData);
			DomainManager.Taiwu.AddItem(context, itemKey, 1, ItemSourceType.Inventory, false);
			GameData.Domains.Item.Cricket cricket = DomainManager.Item.GetElement_Crickets(itemKey.Id);
			DomainManager.Item.AddCatchCricketProfessionSeniority(context, cricket);
			return itemKey;
		}

		// Token: 0x060014A5 RID: 5285 RVA: 0x00141CC0 File Offset: 0x0013FEC0
		private static ItemKey OfflineCreateCricketAndModifyLuckPoint(DataContext context, DukeSkillsData data, int charId)
		{
			short title = data.GetTitleFromOwner(charId);
			bool flag = title < 0;
			ItemKey result;
			if (flag)
			{
				result = ItemKey.Invalid;
			}
			else
			{
				int formulaId = 112;
				int count = ProfessionFormula.Instance[formulaId].Calculate();
				int luckPoint = data.GetDukeLuckPointByTitle(title);
				ItemKey itemKey = DomainManager.Item.CreateCricketByLuckPoint(context, ref luckPoint, count);
				data.OfflineSetDukeLuckPointByTitle(title, luckPoint);
				result = itemKey;
			}
			return result;
		}

		// Token: 0x060014A6 RID: 5286 RVA: 0x00141D28 File Offset: 0x0013FF28
		internal static bool DukeSkill_CheckCharacterHasTitle(int charId)
		{
			bool flag = !DomainManager.Extra.IsProfessionalSkillUnlockedAndEquipped(56);
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				ProfessionData professionData = DomainManager.Extra.GetProfessionData(17);
				result = ProfessionSkillHandle.DukeSkill_CheckCharacterHasTitle(charId, professionData);
			}
			return result;
		}

		// Token: 0x060014A7 RID: 5287 RVA: 0x00141D68 File Offset: 0x0013FF68
		internal static void DukeSkill_RemoveCharacterTitle(DataContext context, int charId)
		{
			ProfessionData professionData = DomainManager.Extra.GetProfessionData(17);
			ProfessionSkillHandle.DukeSkill_RemoveCharacterTitle(context, professionData, charId);
		}

		// Token: 0x060014A8 RID: 5288 RVA: 0x00141D8C File Offset: 0x0013FF8C
		internal static void DukeSkill_ClearAllTitle(DataContext context)
		{
			ProfessionData professionData = DomainManager.Extra.GetProfessionData(17);
			ProfessionSkillHandle.DukeSkill_ClearAllTitle(context, professionData);
		}

		// Token: 0x060014A9 RID: 5289 RVA: 0x00141DB0 File Offset: 0x0013FFB0
		private static void ExecuteOnClick_HunterSkill(DataContext context, ProfessionData professionData, int index, ref ProfessionSkillArg arg)
		{
			if (index != 0)
			{
				if (index != 2)
				{
					throw new Exception(professionData.GetSkillConfig(index).Name + " is not an executable skill.");
				}
				ProfessionSkillHandle.ExecuteOnClick_HunterSkill_2(context, professionData);
			}
			else
			{
				ProfessionSkillHandle.ExecuteOnClick_HunterSkill_0(context, professionData);
			}
		}

		// Token: 0x060014AA RID: 5290 RVA: 0x00141E00 File Offset: 0x00140000
		public static CarrierItem GetCarrierByAnimal(short animalCharTemplateId)
		{
			sbyte animalId = GameData.Domains.Combat.SharedConstValue.CharId2AnimalId[animalCharTemplateId];
			AnimalItem animalItem = Config.Animal.Instance[animalId];
			return Config.Carrier.Instance[animalItem.CarrierId];
		}

		// Token: 0x060014AB RID: 5291 RVA: 0x00141E3C File Offset: 0x0014003C
		private static void ExecuteOnClick_HunterSkill_2(DataContext context, ProfessionData professionData)
		{
			Location taiwuLocation = DomainManager.Taiwu.GetTaiwu().GetLocation();
			Location centerLocation = DomainManager.Extra.GetAnimalGenerateCenterLocationByHunterSkill(context, taiwuLocation.AreaId);
			bool flag = !centerLocation.IsValid();
			if (flag)
			{
				DomainManager.World.GetInstantNotificationCollection().AddProfessionHunterSkill0None();
			}
			else
			{
				sbyte animalCount = professionData.GetSeniorityAnimalCount();
				int prob = GlobalConfig.Instance.HunterSkill2_OddFormulaFactorA + GlobalConfig.Instance.HunterSkill2_OddFormulaFactorB * professionData.GetSeniorityPercent() / 100;
				List<CharacterItem> animalConfigList = new List<CharacterItem>((int)animalCount);
				for (int i = 0; i < (int)animalCount; i++)
				{
					byte[] animalConsummateLevelList = GlobalConfig.Instance.HunterSkill2_AnimalCountIndexToAnimalConsummateLevelList[i];
					byte animalConsummateLevel = animalConsummateLevelList[0];
					for (int j = animalConsummateLevelList.Length - 1; j >= 1; j--)
					{
						bool flag2 = context.Random.CheckPercentProb(prob);
						if (flag2)
						{
							animalConsummateLevel = animalConsummateLevelList[j];
						}
					}
					CharacterItem[] templateIds = (from templateId in ProfessionSkillHandle.AnimalCharacterTemplateIds
					select Config.Character.Instance.GetItem(templateId) into template
					where template.ConsummateLevel <= (sbyte)animalConsummateLevel
					orderby template.ConsummateLevel descending
					select template).ToArray<CharacterItem>();
					CharacterItem animalConfig = templateIds[0];
					bool flag3 = templateIds.Length != 0;
					if (flag3)
					{
						for (int k = 1; k < templateIds.Length; k++)
						{
							CharacterItem template2 = templateIds[k];
							bool flag4 = template2.ConsummateLevel == animalConfig.ConsummateLevel && template2.TemplateId >= 219 && template2.TemplateId > animalConfig.TemplateId && context.Random.CheckPercentProb(33);
							if (flag4)
							{
								animalConfig = template2;
								break;
							}
						}
					}
					animalConfigList.Add(animalConfig);
				}
				List<Location> locationList = ObjectPool<List<Location>>.Instance.Get();
				DomainManager.Extra.GetAnimalGenerateLocationListByCenterLocation(context, centerLocation, (int)animalCount, ref locationList);
				for (int l = 0; l < animalConfigList.Count; l++)
				{
					bool flag5 = l == 0;
					if (flag5)
					{
						DomainManager.Extra.AnimalGenerateInAreaByHunterSkill(context, centerLocation, animalConfigList[l].TemplateId);
					}
					else
					{
						bool flag6 = l < locationList.Count;
						if (flag6)
						{
							DomainManager.Extra.AnimalGenerateInAreaByHunterSkill(context, locationList[l], animalConfigList[l].TemplateId);
						}
						else
						{
							ExtraDomain extra = DomainManager.Extra;
							List<Location> list = locationList;
							extra.AnimalGenerateInAreaByHunterSkill(context, list[list.Count - 1], animalConfigList[l].TemplateId);
						}
					}
					CharacterItem characterConfig = Config.Character.Instance.GetItem(animalConfigList[l].TemplateId);
					DomainManager.World.GetInstantNotificationCollection().AddProfessionHunterSkill0(characterConfig.OrganizationInfo.OrgTemplateId, characterConfig.OrganizationInfo.Grade, true, characterConfig.Gender);
				}
			}
		}

		// Token: 0x060014AC RID: 5292 RVA: 0x0014214A File Offset: 0x0014034A
		[Obsolete]
		private static void ExecuteOnClick_HunterSkill_0(DataContext context, ProfessionData professionData)
		{
		}

		// Token: 0x060014AD RID: 5293 RVA: 0x00142150 File Offset: 0x00140350
		public static GameData.Domains.Character.Character HunterSkill_ItemToAnimalCharacter(DataContext context, ItemKey animalItemKey, string displayName)
		{
			short animalCharTemplateId = Config.Carrier.Instance[animalItemKey.TemplateId].CharacterIdInCombat;
			bool flag = animalCharTemplateId < 0;
			if (flag)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(44, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Invalid carrier to become animal character ");
				defaultInterpolatedStringHandler.AppendFormatted<ItemKey>(animalItemKey);
				defaultInterpolatedStringHandler.AppendLiteral(".");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			ProfessionData professionData = DomainManager.Extra.GetProfessionData(1);
			HunterSkillsData skillsData = professionData.GetSkillsData<HunterSkillsData>();
			HunterSkillsData hunterSkillsData = skillsData;
			if (hunterSkillsData.AnimalItemKeyToGender == null)
			{
				hunterSkillsData.AnimalItemKeyToGender = new Dictionary<ItemKey, sbyte>();
			}
			hunterSkillsData = skillsData;
			if (hunterSkillsData.AnimalCharIdToItemKey == null)
			{
				hunterSkillsData.AnimalCharIdToItemKey = new Dictionary<int, ItemKey>();
			}
			hunterSkillsData = skillsData;
			if (hunterSkillsData.AnimalCharIdToAttraction == null)
			{
				hunterSkillsData.AnimalCharIdToAttraction = new Dictionary<ItemKey, short>();
			}
			sbyte gender = Config.Character.Instance[animalCharTemplateId].Gender;
			bool needSaveGender = false;
			bool flag2 = gender == -1;
			if (flag2)
			{
				sbyte savedGender;
				bool flag3 = skillsData.AnimalItemKeyToGender.TryGetValue(animalItemKey, out savedGender);
				if (flag3)
				{
					gender = savedGender;
				}
				else
				{
					gender = (sbyte)context.Random.Next(2);
					needSaveGender = true;
				}
			}
			FixedEnemyCreationInfo creationInfo = new FixedEnemyCreationInfo
			{
				Gender = gender
			};
			GameData.Domains.Character.Character character = DomainManager.Character.CreateFixedEnemyWithCreationInfo(context, animalCharTemplateId, ref creationInfo);
			FullName fullName = character.GetFullName();
			fullName.Type = 8;
			character.SetFullName(fullName, context);
			int charId = character.GetId();
			short savedAttraction;
			bool flag4 = !skillsData.AnimalCharIdToAttraction.TryGetValue(animalItemKey, out savedAttraction);
			if (flag4)
			{
				short attraction = (short)context.Random.Next(0, 901);
				skillsData.AnimalCharIdToAttraction.Add(animalItemKey, attraction);
			}
			DomainManager.Character.CompleteCreatingCharacter(charId);
			DomainManager.Extra.AssignCharacterCustomDisplayName(context, charId, displayName);
			DomainManager.Taiwu.JoinGroup(context, charId, true);
			skillsData.AnimalCharIdToItemKey.Add(charId, animalItemKey);
			bool flag5 = needSaveGender;
			if (flag5)
			{
				skillsData.AnimalItemKeyToGender.Add(animalItemKey, gender);
			}
			DomainManager.Extra.SetProfessionData(context, professionData);
			DomainManager.Taiwu.RemoveItem(context, animalItemKey, 1, 1, false, false);
			DomainManager.Item.SetOwner(animalItemKey, ItemOwnerType.SpecialGroupMember, DomainManager.Taiwu.GetTaiwuCharId());
			EventHelper.ShowGetItemPageForCharacters(new List<int>
			{
				charId
			}, false, "", null);
			DomainManager.World.GetInstantNotificationCollection().AddBeastUpgrade(animalItemKey.ItemType, animalItemKey.TemplateId);
			return character;
		}

		// Token: 0x060014AE RID: 5294 RVA: 0x001423AC File Offset: 0x001405AC
		public static ItemKey HunterSkill_AnimalCharacterToItem(DataContext context, GameData.Domains.Character.Character character)
		{
			int charId = character.GetId();
			ProfessionData professionData = DomainManager.Extra.GetProfessionData(1);
			HunterSkillsData skillsData = professionData.GetSkillsData<HunterSkillsData>();
			ItemKey itemKey = skillsData.AnimalCharIdToItemKey[charId];
			Dictionary<ItemKey, int> items = character.GetInventory().Items;
			List<ValueTuple<ItemKey, int>> toRemoveItems = new List<ValueTuple<ItemKey, int>>();
			foreach (KeyValuePair<ItemKey, int> item in items)
			{
				toRemoveItems.Add(new ValueTuple<ItemKey, int>(item.Key, item.Value));
			}
			int leader = character.GetLeaderId();
			GameData.Domains.Character.Character leaderChar = DomainManager.Character.GetElement_Objects(leader);
			Location location = leaderChar.GetLocation();
			MapBlockData blockData = DomainManager.Map.GetBlockData(location.AreaId, location.BlockId);
			foreach (ValueTuple<ItemKey, int> valueTuple in toRemoveItems)
			{
				ItemKey item2 = valueTuple.Item1;
				int count = valueTuple.Item2;
				character.RemoveInventoryItem(context, item2, count, false, false);
				DomainManager.Map.AddBlockItem(context, blockData, item2, count);
			}
			DomainManager.Character.LeaveGroup(context, character, true);
			DomainManager.World.GetInstantNotificationCollection().AddBeastDowngrade(itemKey.ItemType, itemKey.TemplateId);
			DomainManager.Character.RemoveNonIntelligentCharacter(context, character);
			GameData.Domains.Character.Character taiwuChar = DomainManager.Taiwu.GetTaiwu();
			taiwuChar.AddInventoryItem(context, itemKey, 1, false);
			return itemKey;
		}

		// Token: 0x060014AF RID: 5295 RVA: 0x0014254C File Offset: 0x0014074C
		private static bool CheckSpecialCondition_HunterSkill(ProfessionData professionData, int skillIndex)
		{
			bool result;
			switch (skillIndex)
			{
			case 1:
			{
				HashSet<ValueTuple<Location, int>> hashSet;
				result = DomainManager.Extra.TryGetAnimalAttackInRange(DomainManager.Taiwu.GetTaiwu().GetLocation(), 2, true, out hashSet);
				break;
			}
			case 2:
				result = DomainManager.Extra.CheckSpecialCondition_HunterSkill2(DataContextManager.GetCurrentThreadDataContext());
				break;
			case 3:
				result = (ProfessionSkillHandle.CheckGroupCountForConvertToAnimalCharacter() && ProfessionSkillHandle.<CheckSpecialCondition_HunterSkill>g__CheckItem|90_0());
				break;
			default:
				result = true;
				break;
			}
			return result;
		}

		// Token: 0x060014B0 RID: 5296 RVA: 0x001425C0 File Offset: 0x001407C0
		public static bool CheckGroupCountForConvertToAnimalCharacter()
		{
			return true;
		}

		// Token: 0x060014B1 RID: 5297 RVA: 0x001425D4 File Offset: 0x001407D4
		public static bool IsItemCanConvertToAnimalCharacter(ItemKey itemKey)
		{
			short subType = ItemTemplateHelper.GetItemSubType(itemKey.ItemType, itemKey.TemplateId);
			return subType == 402;
		}

		// Token: 0x060014B2 RID: 5298 RVA: 0x00142600 File Offset: 0x00140800
		internal static int CalcLiteratiSkill2AuthorityCost(SecretInformationItem config, int holderCount)
		{
			return (int)(5000 * config.SortValue) * (100 - holderCount * 50 / (int)config.MaxPersonAmount) / 100;
		}

		// Token: 0x060014B3 RID: 5299 RVA: 0x00142630 File Offset: 0x00140830
		public static void LiteratiSkill_BroadcastModifiedSecretInformation(DataContext context, int secretInformationId)
		{
			InformationDomain informationDomain = DomainManager.Information;
			SecretInformationItem config = informationDomain.GetSecretInformationConfig(secretInformationId);
			GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
			SecretInformationCharacterDataCollection collection;
			DomainManager.Information.TryGetElement_CharacterSecretInformation(taiwu.GetId(), out collection);
			taiwu.ChangeResource(context, 7, -ProfessionSkillHandle.CalcLiteratiSkill2AuthorityCost(config, informationDomain.GetSecretInformationHolderCount(secretInformationId)));
			DomainManager.Information.MakeSecretInformationBroadcastEffect(context, secretInformationId, DomainManager.Taiwu.GetTaiwuCharId());
			DomainManager.World.GetInstantNotificationCollection().AddDisseminateSecretInformation(config.TemplateId, secretInformationId);
		}

		// Token: 0x060014B4 RID: 5300 RVA: 0x001426B0 File Offset: 0x001408B0
		public unsafe static void LiteratiSkill_AreaBroadcastNormalInformation(DataContext context, NormalInformation normalInformation)
		{
			GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
			Location taiwuLocation = taiwu.GetLocation();
			bool flag = !taiwuLocation.IsValid();
			if (!flag)
			{
				int count = 0;
				Span<MapBlockData> areaBlocks = DomainManager.Map.GetAreaBlocks(taiwuLocation.AreaId);
				for (int i = 0; i < areaBlocks.Length; i++)
				{
					MapBlockData block = *areaBlocks[i];
					HashSet<int> blockTargets = block.CharacterSet;
					bool flag2 = blockTargets == null;
					if (!flag2)
					{
						EventArgBox argBox = new EventArgBox();
						InformationItem config = Information.Instance[normalInformation.TemplateId];
						foreach (int targetCharId in blockTargets)
						{
							argBox.Clear();
							GameData.Domains.Character.Character targetChar;
							bool flag3 = DomainManager.Character.TryGetElement_Objects(targetCharId, out targetChar);
							if (flag3)
							{
								sbyte useResultType = EventHelper.GetNormalInformationUseResultType(targetCharId, normalInformation);
								int rate = (int)config.EffectRate[(int)useResultType];
								EventHelper.ChangeFavorabilityOptionalShareInformation(targetChar, taiwu, EventHelper.ClampFavorabilityChangeValue((int)EventHelper.GetTaiwuFavorabilityHotChangeValue() * rate / 100 * 150 / 100));
							}
							EventHelper.ApplyNormalInformationWithEffectRate(targetCharId, argBox, normalInformation, string.Empty, string.Empty, string.Empty, string.Empty, 150);
							count++;
						}
					}
				}
				DomainManager.World.GetInstantNotificationCollection().AddDisseminateInformation(taiwuLocation, count);
			}
		}

		// Token: 0x060014B5 RID: 5301 RVA: 0x0014282C File Offset: 0x00140A2C
		private static bool CheckLiteratiSkillNormalInformationUsable(NormalInformation normalInformation)
		{
			InformationItem config = Information.Instance.GetItem(normalInformation.TemplateId);
			InformationInfoItem info = InformationInfo.Instance.GetItem(config.InfoIds[(int)normalInformation.Level]);
			sbyte type = config.Type;
			bool flag = type <= 3;
			bool flag2 = flag && config.IsGeneral;
			bool flag3 = flag2;
			if (flag3)
			{
				sbyte lifeSkillType = info.LifeSkillType;
				bool flag4 = lifeSkillType - 12 <= 1;
				flag3 = !flag4;
			}
			return flag3;
		}

		// Token: 0x060014B6 RID: 5302 RVA: 0x001428B8 File Offset: 0x00140AB8
		private static bool CheckSpecialCondition_LiteratiSkill(ProfessionData professionData, int index)
		{
			bool result;
			if (index != 2)
			{
				result = (index != 3 || ProfessionSkillHandle.CheckSpecialCondition_LiteratiSkill_3(professionData));
			}
			else
			{
				result = ProfessionSkillHandle.CheckSpecialCondition_LiteratiSkill_2(professionData);
			}
			return result;
		}

		// Token: 0x060014B7 RID: 5303 RVA: 0x001428F0 File Offset: 0x00140AF0
		private static bool CheckSpecialCondition_LiteratiSkill_2(ProfessionData professionData)
		{
			int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
			SecretInformationDisplayPackage secrets = DomainManager.Information.GetSecretInformationDisplayPackageFromCharacter(taiwuCharId);
			List<SecretInformationDisplayData> secretInformationDisplayDataList = secrets.SecretInformationDisplayDataList;
			return secretInformationDisplayDataList != null && secretInformationDisplayDataList.Count > 0;
		}

		// Token: 0x060014B8 RID: 5304 RVA: 0x00142930 File Offset: 0x00140B30
		private static bool CheckSpecialCondition_LiteratiSkill_3(ProfessionData professionData)
		{
			int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
			IList<NormalInformation> infoList = DomainManager.Information.GetCharacterNormalInformation(taiwuCharId).GetList() ?? new List<NormalInformation>();
			return infoList.Any(new Func<NormalInformation, bool>(ProfessionSkillHandle.CheckLiteratiSkillNormalInformationUsable));
		}

		// Token: 0x060014B9 RID: 5305 RVA: 0x0014297C File Offset: 0x00140B7C
		public unsafe static void MartialArtistSkill_MakeAreaLearnCombatSkill(DataContext context, ProfessionData professionData, sbyte combatSkillType)
		{
			ProfessionSkillHandle.<>c__DisplayClass100_0 CS$<>8__locals1;
			CS$<>8__locals1.combatSkillType = combatSkillType;
			CS$<>8__locals1.context = context;
			CS$<>8__locals1.taiwu = DomainManager.Taiwu.GetTaiwu();
			Location location = CS$<>8__locals1.taiwu.GetLocation();
			bool flag = !location.IsValid();
			if (!flag)
			{
				Span<MapBlockData> blocks = DomainManager.Map.GetAreaBlocks(location.AreaId);
				CS$<>8__locals1.selectableSkillIds = new List<short>();
				CS$<>8__locals1.lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
				int taiwuCharId = CS$<>8__locals1.taiwu.GetId();
				CS$<>8__locals1.currDate = DomainManager.World.GetCurrDate();
				CS$<>8__locals1.lifeRecordCollection.AddCombatSkillModel(taiwuCharId, CS$<>8__locals1.currDate, location, CS$<>8__locals1.combatSkillType);
				int charCount = 0;
				Span<MapBlockData> span = blocks;
				for (int i = 0; i < span.Length; i++)
				{
					MapBlockData block = *span[i];
					bool flag2 = block.CharacterSet == null;
					if (!flag2)
					{
						foreach (int charId in block.CharacterSet)
						{
							bool flag3 = ProfessionSkillHandle.<MartialArtistSkill_MakeAreaLearnCombatSkill>g__TryLearnCombatSkill|100_0(charId, ref CS$<>8__locals1);
							if (flag3)
							{
								charCount++;
							}
						}
					}
				}
				HashSet<int> taiwuGroupCharIds = DomainManager.Taiwu.GetGroupCharIds().GetCollection();
				foreach (int charId2 in taiwuGroupCharIds)
				{
					bool flag4 = charId2 == taiwuCharId;
					if (!flag4)
					{
						bool flag5 = ProfessionSkillHandle.<MartialArtistSkill_MakeAreaLearnCombatSkill>g__TryLearnCombatSkill|100_0(charId2, ref CS$<>8__locals1);
						if (flag5)
						{
							charCount++;
						}
					}
				}
				int authorityGain = 1000 + charCount * 100;
				CS$<>8__locals1.taiwu.ChangeResource(CS$<>8__locals1.context, 7, authorityGain);
				InstantNotificationCollection instantNotifications = DomainManager.World.GetInstantNotificationCollection();
				bool flag6 = authorityGain > 0;
				if (flag6)
				{
					instantNotifications.AddResourceIncreased(taiwuCharId, 7, authorityGain);
				}
			}
		}

		// Token: 0x060014BA RID: 5306 RVA: 0x00142B84 File Offset: 0x00140D84
		private static bool CheckSpecialCondition_MartialArtistSkill(ProfessionData professionData, int index)
		{
			bool result;
			if (index != 1)
			{
				result = (index != 2 || ProfessionSkillHandle.CheckSpecialCondition_MartialArtistSkill_2(professionData));
			}
			else
			{
				result = ProfessionSkillHandle.CheckSpecialCondition_MartialArtistSkill_1(professionData);
			}
			return result;
		}

		// Token: 0x060014BB RID: 5307 RVA: 0x00142BBC File Offset: 0x00140DBC
		private static bool CheckSpecialCondition_MartialArtistSkill_1(ProfessionData professionData)
		{
			Location taiwuLocation = DomainManager.Taiwu.GetTaiwu().GetLocation();
			bool flag = !taiwuLocation.IsValid();
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				MapBlockData blockData = DomainManager.Map.GetBlock(taiwuLocation);
				bool flag2 = !blockData.IsCityTown();
				if (flag2)
				{
					result = false;
				}
				else
				{
					MapBlockData settlementBlock = DomainManager.Map.GetBelongSettlementBlock(taiwuLocation);
					Settlement settlement = DomainManager.Organization.GetSettlementByLocation(settlementBlock.GetLocation());
					result = (settlement.GetSafety() < settlement.GetMaxSafety());
				}
			}
			return result;
		}

		// Token: 0x060014BC RID: 5308 RVA: 0x00142C40 File Offset: 0x00140E40
		private static bool CheckSpecialCondition_MartialArtistSkill_2(ProfessionData professionData)
		{
			Location taiwuLocation = DomainManager.Taiwu.GetTaiwu().GetLocation();
			bool flag = !taiwuLocation.IsValid();
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				MapBlockData blockData = DomainManager.Map.GetBlock(taiwuLocation);
				bool flag2 = !blockData.IsCityTown();
				if (flag2)
				{
					result = false;
				}
				else
				{
					MapBlockData settlementBlock = DomainManager.Map.GetBelongSettlementBlock(taiwuLocation);
					Settlement settlement = DomainManager.Organization.GetSettlementByLocation(settlementBlock.GetLocation());
					result = (settlement.GetSafety() >= 25);
				}
			}
			return result;
		}

		// Token: 0x060014BD RID: 5309 RVA: 0x00142CC4 File Offset: 0x00140EC4
		private static void ExecuteOnClick_MartialArtist(DataContext context, ProfessionData professionData, int index, ref ProfessionSkillArg arg)
		{
			if (index != 3)
			{
				throw new Exception(professionData.GetSkillConfig(index).Name + " is not an executable skill.");
			}
			ProfessionSkillHandle.ExecuteOnClick_MartialArtist_3(context, professionData, ref arg);
		}

		// Token: 0x060014BE RID: 5310 RVA: 0x00142D04 File Offset: 0x00140F04
		private static void ExecuteOnClick_MartialArtist_3(DataContext context, ProfessionData professionData, ref ProfessionSkillArg professionSkillArg)
		{
			bool flag = professionSkillArg.EffectBlocks == null;
			if (flag)
			{
				DomainManager.Extra.MartialArtistSkill3Execute(context, true);
			}
		}

		// Token: 0x060014BF RID: 5311 RVA: 0x00142D30 File Offset: 0x00140F30
		private static void ExecuteOnClick_SavageSkill(DataContext context, ProfessionData professionData, int index, ref ProfessionSkillArg arg)
		{
			if (index != 1)
			{
				if (index != 3)
				{
					throw new Exception(professionData.GetSkillConfig(index).Name + " is not an executable skill.");
				}
				ProfessionSkillHandle.ExecuteOnClick_SavageSkill_3(context, professionData, ref arg);
			}
			else
			{
				ProfessionSkillHandle.ExecuteOnClick_SavageSkill_1(context, professionData);
			}
		}

		// Token: 0x060014C0 RID: 5312 RVA: 0x00142D7F File Offset: 0x00140F7F
		internal static IEnumerable<Location> GetSavageSkill_1_EffectRange(Location location)
		{
			bool flag = !location.IsValid();
			if (flag)
			{
				yield break;
			}
			yield return location;
			MapDomain mapDomain = DomainManager.Map;
			byte areaSize = mapDomain.GetAreaSize(location.AreaId);
			ByteCoordinate origin = ByteCoordinate.IndexToCoordinate(location.BlockId, areaSize);
			bool flag2 = origin.X > 0;
			if (flag2)
			{
				yield return new Location(location.AreaId, ByteCoordinate.CoordinateToIndex(new ByteCoordinate(origin.X - 1, origin.Y), areaSize));
			}
			bool flag3 = origin.Y > 0;
			if (flag3)
			{
				yield return new Location(location.AreaId, ByteCoordinate.CoordinateToIndex(new ByteCoordinate(origin.X, origin.Y - 1), areaSize));
			}
			bool flag4 = origin.X < areaSize - 1;
			if (flag4)
			{
				yield return new Location(location.AreaId, ByteCoordinate.CoordinateToIndex(new ByteCoordinate(origin.X + 1, origin.Y), areaSize));
			}
			bool flag5 = origin.Y < areaSize - 1;
			if (flag5)
			{
				yield return new Location(location.AreaId, ByteCoordinate.CoordinateToIndex(new ByteCoordinate(origin.X, origin.Y + 1), areaSize));
			}
			yield break;
		}

		// Token: 0x060014C1 RID: 5313 RVA: 0x00142D90 File Offset: 0x00140F90
		private unsafe static void ExecuteOnClick_SavageSkill_1(DataContext context, ProfessionData professionData)
		{
			GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
			bool flag = !taiwu.GetLocation().IsValid();
			if (!flag)
			{
				MapDomain mapDomain = DomainManager.Map;
				int recoveryValue = 0;
				int recoveryFactor = professionData.GetSeniorityResourceRecoveryFactor();
				InstantNotificationCollection instantNotification = DomainManager.World.GetInstantNotificationCollection();
				instantNotification.AddBlockResourceRecovery(recoveryFactor);
				foreach (Location blockLocation in ProfessionSkillHandle.GetSavageSkill_1_EffectRange(taiwu.GetLocation()))
				{
					MapBlockData blockData = mapDomain.GetBlock(blockLocation);
					bool flag2 = 126 == blockData.TemplateId;
					if (!flag2)
					{
						SectStoryHeavenlyTreeExtendable tree;
						bool flag3 = DomainManager.Extra.TryGetHeavenlyTreeByLocation(blockData.GetLocation(), out tree);
						if (flag3)
						{
							int requirement = DomainManager.Extra.GetHeavenlyTreeRequiredGrowPointById(tree.Id);
							bool flag4 = requirement >= 0;
							if (flag4)
							{
								DomainManager.Extra.HeavenlyTreeGrewUp(context, tree.Id, requirement, false);
								instantNotification.AddShenTreeGrow();
							}
						}
						for (sbyte resourceType = 0; resourceType < 6; resourceType += 1)
						{
							short maxResource = *(ref blockData.MaxResources.Items.FixedElementField + (IntPtr)resourceType * 2);
							short currResource = *(ref blockData.CurrResources.Items.FixedElementField + (IntPtr)resourceType * 2);
							short newResource = (short)Math.Min((int)maxResource, (int)currResource + (int)maxResource * recoveryFactor / 100);
							recoveryValue += (int)(newResource - *(ref blockData.CurrResources.Items.FixedElementField + (IntPtr)resourceType * 2));
							*(ref blockData.CurrResources.Items.FixedElementField + (IntPtr)resourceType * 2) = newResource;
						}
						int currResourceSum = blockData.CurrResources.GetSum();
						int maxResourceSum = blockData.MaxResources.GetSum();
						bool flag5 = currResourceSum >= maxResourceSum / 2;
						if (flag5)
						{
							blockData.Destroyed = false;
						}
						DomainManager.Map.SetBlockData(context, blockData);
					}
				}
				ProfessionFormulaItem formula = ProfessionFormula.Instance[7];
				int addSeniority = formula.Calculate(recoveryValue);
				DomainManager.Extra.ChangeProfessionSeniority(context, 0, addSeniority, true, false);
			}
		}

		// Token: 0x060014C2 RID: 5314 RVA: 0x00142FCC File Offset: 0x001411CC
		private static void ExecuteOnClick_SavageSkill_3(DataContext context, ProfessionData professionData, ref ProfessionSkillArg arg)
		{
			ItemKey selectedItem = arg.ItemKey;
			GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
			Location location = taiwu.GetLocation();
			MapBlockData block = DomainManager.Map.GetBlock(location);
			IEnumerable<KeyValuePair<ItemKeyAndDate, int>> items = block.Items;
			Func<KeyValuePair<ItemKeyAndDate, int>, bool> <>9__0;
			Func<KeyValuePair<ItemKeyAndDate, int>, bool> predicate;
			if ((predicate = <>9__0) == null)
			{
				predicate = (<>9__0 = ((KeyValuePair<ItemKeyAndDate, int> item) => item.Key.ItemKey == selectedItem));
			}
			using (IEnumerator<KeyValuePair<ItemKeyAndDate, int>> enumerator = items.Where(predicate).GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					KeyValuePair<ItemKeyAndDate, int> item2 = enumerator.Current;
					ItemKeyAndDate itemKeyAndDate = item2.Key;
					block.RemoveItemByCount(itemKeyAndDate, 1);
					DomainManager.Item.RemoveOwner(itemKeyAndDate.ItemKey, ItemOwnerType.MapBlock, location.GetHashCode());
				}
			}
			DomainManager.Map.SetBlockData(context, block);
			taiwu.AddInventoryItem(context, selectedItem, 1, false);
			List<ItemDisplayData> itemDisplayDataList = new List<ItemDisplayData>
			{
				DomainManager.Item.GetItemDisplayData(selectedItem, DomainManager.Taiwu.GetTaiwu().GetId())
			};
			GameDataBridge.AddDisplayEvent<List<ItemDisplayData>, bool, bool>(DisplayEventType.OpenGetItem_Item, itemDisplayDataList, false, true);
		}

		// Token: 0x060014C3 RID: 5315 RVA: 0x00143100 File Offset: 0x00141300
		private static void OpenItemSelectFromBlock()
		{
			DomainManager.World.AdvanceDaysInMonth(DomainManager.TaiwuEvent.MainThreadDataContext, (int)GlobalConfig.Instance.SavageSkill3_OpenItemSelectTimeCost);
			List<ItemDisplayData> itemDisplayDataList = new List<ItemDisplayData>();
			GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
			Location location = taiwu.GetLocation();
			bool flag = !location.IsValid();
			if (!flag)
			{
				MapBlockData block = DomainManager.Map.GetBlock(location);
				Dictionary<ItemKey, int> itemKeyAndCountDict = new Dictionary<ItemKey, int>();
				foreach (KeyValuePair<ItemKeyAndDate, int> item in block.Items)
				{
					bool flag2 = itemKeyAndCountDict.ContainsKey(item.Key.ItemKey);
					if (flag2)
					{
						Dictionary<ItemKey, int> dictionary = itemKeyAndCountDict;
						ItemKey itemKey = item.Key.ItemKey;
						dictionary[itemKey] += item.Value;
					}
					else
					{
						itemKeyAndCountDict.Add(item.Key.ItemKey, item.Value);
					}
				}
				itemDisplayDataList = DomainManager.Item.GetItemDisplayDataListOptional((from v in itemKeyAndCountDict
				select v.Key).ToList<ItemKey>(), taiwu.GetId(), -1, false);
				foreach (ItemDisplayData item2 in itemDisplayDataList)
				{
					item2.Amount = itemKeyAndCountDict[item2.Key];
				}
				GameDataBridge.AddDisplayEvent<int, List<ItemDisplayData>>(DisplayEventType.OpenProfessionSkillSpecial, 60, itemDisplayDataList);
			}
		}

		// Token: 0x060014C4 RID: 5316 RVA: 0x001432AC File Offset: 0x001414AC
		private static bool CheckSpecialCondition_SavageSkill(ProfessionData professionData, int skillIndex)
		{
			bool result;
			if (skillIndex != 1)
			{
				result = (skillIndex != 3 || ProfessionSkillHandle.CheckSpecialCondition_SavageSkill_3(professionData));
			}
			else
			{
				result = DomainManager.Extra.CheckSpecialCondition_SavageSkill_1(professionData);
			}
			return result;
		}

		// Token: 0x060014C5 RID: 5317 RVA: 0x001432E8 File Offset: 0x001414E8
		private static bool CheckSpecialCondition_SavageSkill_3(ProfessionData professionData)
		{
			GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
			Location location = taiwu.GetLocation();
			bool flag = !location.IsValid();
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				MapBlockData block = DomainManager.Map.GetBlock(location);
				SortedList<ItemKeyAndDate, int> items = block.Items;
				result = (items != null && items.Count > 0);
			}
			return result;
		}

		// Token: 0x060014C6 RID: 5318 RVA: 0x00143348 File Offset: 0x00141548
		private static void ExecuteOnClick_TaoistMonkSkill(DataContext context, ProfessionData professionData, int index, ref ProfessionSkillArg arg)
		{
			if (index != 3)
			{
				throw new Exception(professionData.GetSkillConfig(index).Name + " is not an executable skill.");
			}
			ProfessionSkillHandle.ExecuteOnClick_TaoistMonkSkill_3(context, professionData);
		}

		// Token: 0x060014C7 RID: 5319 RVA: 0x00143388 File Offset: 0x00141588
		private static void ExecuteOnClick_TaoistMonkSkill_3(DataContext context, ProfessionData professionData)
		{
			TaoistMonkSkillsData skillsData = professionData.GetSkillsData<TaoistMonkSkillsData>();
			skillsData.IsTriggeringTribulation = true;
		}

		// Token: 0x060014C8 RID: 5320 RVA: 0x001433A4 File Offset: 0x001415A4
		public static bool TaoistMonkSkill_CheckTribulationSucceed()
		{
			GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
			Dictionary<ItemKey, int> inventoryItems = taiwu.GetInventory().Items;
			int totalAmount = 0;
			foreach (KeyValuePair<ItemKey, int> keyValuePair in inventoryItems)
			{
				ItemKey itemKey2;
				int num;
				keyValuePair.Deconstruct(out itemKey2, out num);
				ItemKey itemKey = itemKey2;
				int amount = num;
				bool flag = itemKey.ItemType != 12 || itemKey.TemplateId != 234;
				if (!flag)
				{
					totalAmount += amount;
				}
			}
			return totalAmount >= 99;
		}

		// Token: 0x060014C9 RID: 5321 RVA: 0x00143458 File Offset: 0x00141658
		public static void TaoistMonkSkill_ConfirmTribulationSucceed(DataContext context, ProfessionData professionData)
		{
			GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
			List<short> featureIds = taiwu.GetFeatureIds();
			TaoistMonkSkillsData skillsData = professionData.GetSkillsData<TaoistMonkSkillsData>();
			Dictionary<ItemKey, int> inventoryItems = taiwu.GetInventory().Items;
			foreach (KeyValuePair<ItemKey, int> keyValuePair in inventoryItems)
			{
				ItemKey itemKey2;
				int num;
				keyValuePair.Deconstruct(out itemKey2, out num);
				ItemKey itemKey = itemKey2;
				int amount = num;
				bool flag = itemKey.ItemType == 12 && itemKey.TemplateId == 234;
				if (flag)
				{
					taiwu.RemoveInventoryItem(context, itemKey, 99, amount == 99, false);
					break;
				}
			}
			bool flag2 = featureIds.Contains(629);
			if (flag2)
			{
				taiwu.RemoveFeature(context, 629);
				taiwu.RemoveFeature(context, 628);
				taiwu.RemoveFeature(context, 627);
				skillsData.SurvivedTribulationCount = 4;
				skillsData.LastAgeIncreaseDate = DomainManager.World.GetCurrDate() - 12 - (int)DomainManager.World.GetCurrMonthInYear() + (int)taiwu.GetBirthMonth();
				DomainManager.Extra.SetProfessionData(context, professionData);
				bool flag3 = taiwu.GetCurrAge() > 16;
				if (flag3)
				{
					short leftMaxHealth = taiwu.GetLeftMaxHealth(false);
					taiwu.SetCurrAge(16, context);
					short leftMaxHealthAfter = taiwu.GetLeftMaxHealth(false);
					taiwu.ChangeHealth(context, (int)((leftMaxHealthAfter - leftMaxHealth) * 12));
					AvatarData avatar = taiwu.GetAvatar();
					bool flag4 = avatar.UpdateGrowableElementsShowingAbilities(taiwu);
					if (flag4)
					{
						taiwu.SetAvatar(avatar, context);
					}
				}
				taiwu.AddFeature(context, 195, true);
			}
			else
			{
				bool flag5 = taiwu.AddFeature(context, 627, false);
				if (flag5)
				{
					skillsData.SurvivedTribulationCount = 1;
				}
				else
				{
					bool flag6 = taiwu.AddFeature(context, 628, false);
					if (flag6)
					{
						skillsData.SurvivedTribulationCount = 2;
					}
					else
					{
						bool flag7 = taiwu.AddFeature(context, 629, false);
						if (flag7)
						{
							skillsData.SurvivedTribulationCount = 3;
						}
					}
				}
				DomainManager.Extra.SetProfessionData(context, professionData);
			}
			LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
			int currDate = DomainManager.World.GetCurrDate();
			lifeRecordCollection.AddTribulationSucceeded(taiwu.GetId(), currDate, taiwu.GetLocation());
		}

		// Token: 0x060014CA RID: 5322 RVA: 0x0014368C File Offset: 0x0014188C
		[Obsolete]
		public static bool TaoistMonkSkill_CanTriggerTribulation()
		{
			bool flag = !DomainManager.Extra.IsProfessionalSkillUnlocked(5, 3);
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
				bool flag2 = taiwu.GetLeftMaxHealth(false) > 0;
				result = (!flag2 && DomainManager.Extra.IsOneShotEventHandled(39));
			}
			return result;
		}

		// Token: 0x060014CB RID: 5323 RVA: 0x001436E0 File Offset: 0x001418E0
		public static bool TaoistMonkSkill_HasSurvivedAllTribulation()
		{
			ProfessionData professionData = DomainManager.Extra.GetProfessionData(5);
			TaoistMonkSkillsData skillsData = professionData.GetSkillsData<TaoistMonkSkillsData>();
			return skillsData.HasSurvivedAllTribulation();
		}

		// Token: 0x060014CC RID: 5324 RVA: 0x0014370C File Offset: 0x0014190C
		public static bool TaoistMonkSkill_ShouldIncreaseAge()
		{
			ProfessionData professionData = DomainManager.Extra.GetProfessionData(5);
			TaoistMonkSkillsData skillsData = professionData.GetSkillsData<TaoistMonkSkillsData>();
			return skillsData.ShouldIncreaseAge();
		}

		// Token: 0x060014CD RID: 5325 RVA: 0x00143738 File Offset: 0x00141938
		public static void TaoistMonkSkill_UpdateAgeIncreaseDate(DataContext context)
		{
			ProfessionData professionData = DomainManager.Extra.GetProfessionData(5);
			TaoistMonkSkillsData skillsData = professionData.GetSkillsData<TaoistMonkSkillsData>();
			skillsData.LastAgeIncreaseDate = DomainManager.World.GetCurrDate();
			DomainManager.Extra.SetProfessionData(context, professionData);
		}

		// Token: 0x060014CE RID: 5326 RVA: 0x00143778 File Offset: 0x00141978
		private static void UnpackCrossArchiveProfession_TaoistMonk(DataContext context)
		{
			ProfessionData professionData = DomainManager.Extra.GetProfessionData(5);
			TaoistMonkSkillsData taoistMonkSkillsData = professionData.GetSkillsData<TaoistMonkSkillsData>();
			bool flag = taoistMonkSkillsData.SurvivedTribulationCount <= 0;
			if (!flag)
			{
				GameData.Domains.Character.Character taiwuChar = DomainManager.Taiwu.GetTaiwu();
				bool flag2 = taoistMonkSkillsData.HasSurvivedAllTribulation();
				if (flag2)
				{
					bool flag3 = taiwuChar.GetCurrAge() > 16;
					if (flag3)
					{
						taiwuChar.SetCurrAge(16, context);
					}
				}
				else
				{
					for (int i = 0; i < (int)taoistMonkSkillsData.SurvivedTribulationCount; i++)
					{
						short featureId = (short)(627 + i);
						taiwuChar.AddFeature(context, featureId, false);
					}
				}
			}
		}

		// Token: 0x060014CF RID: 5327 RVA: 0x00143818 File Offset: 0x00141A18
		private static void TaoistMonkSkill_ResetSurvivedTribulationCount(DataContext context)
		{
			ProfessionData professionData = DomainManager.Extra.GetProfessionData(5);
			TaoistMonkSkillsData skillsData = professionData.GetSkillsData<TaoistMonkSkillsData>();
			skillsData.SurvivedTribulationCount = 0;
			DomainManager.Extra.SetProfessionData(context, professionData);
		}

		// Token: 0x060014D0 RID: 5328 RVA: 0x00143850 File Offset: 0x00141A50
		private static void TaoistMonkSkill_OnPostAdvanceMonth(DataContext context)
		{
			ProfessionData professionData = DomainManager.Extra.GetProfessionData(5);
			TaoistMonkSkillsData skillData = professionData.GetSkillsData<TaoistMonkSkillsData>();
			GameData.Domains.Character.Character taiwuChar = DomainManager.Taiwu.GetTaiwu();
			bool flag = skillData.SurvivedTribulationCount > 0 && !skillData.HasSurvivedAllTribulation();
			if (flag)
			{
				taiwuChar.CreateInventoryItem(context, 12, 234, (int)(skillData.SurvivedTribulationCount * 3));
				DomainManager.World.GetInstantNotificationCollection().AddGetItem(taiwuChar.GetId(), 12, 234);
			}
			bool isTriggeringTribulation = skillData.IsTriggeringTribulation;
			if (isTriggeringTribulation)
			{
				skillData.IsTriggeringTribulation = false;
				MonthlyEventCollection monthlyEventCollection = DomainManager.World.GetMonthlyEventCollection();
				monthlyEventCollection.AddTaiwuTribulation(taiwuChar.GetId(), taiwuChar.GetLocation());
			}
			bool flag2 = skillData.ShouldIncreaseAge();
			if (flag2)
			{
				ProfessionSkillHandle.TaoistMonkSkill_UpdateAgeIncreaseDate(context);
			}
		}

		// Token: 0x060014D1 RID: 5329 RVA: 0x00143918 File Offset: 0x00141B18
		private static bool CheckSpecialCondition_TaoistMonkSkill(ProfessionData professionData, int index)
		{
			return index != 3 || ProfessionSkillHandle.CheckSpecialCondition_TaoistMonkSkill_3(professionData);
		}

		// Token: 0x060014D2 RID: 5330 RVA: 0x00143940 File Offset: 0x00141B40
		private static bool CheckSpecialCondition_TaoistMonkSkill_3(ProfessionData professionData)
		{
			return true;
		}

		// Token: 0x060014D3 RID: 5331 RVA: 0x00143954 File Offset: 0x00141B54
		private static void TeaTasterSkill_UpdateVillagerLearnSkillInterval(DataContext context)
		{
			bool flag = !DomainManager.Taiwu.GetVillagerLearnLifeSkillsFromSect();
			if (!flag)
			{
				ProfessionData professionData = DomainManager.Extra.GetProfessionData(16);
				int currDate = DomainManager.World.GetCurrDate();
				TeaTasterSkillsData skillsData = professionData.SkillsData as TeaTasterSkillsData;
				bool flag2 = skillsData != null && skillsData.VillagersLastLearnSkillDate + 3 < currDate;
				if (flag2)
				{
					skillsData.VillagersLastLearnSkillDate = currDate;
					DomainManager.Extra.SetProfessionData(context, professionData);
				}
			}
		}

		// Token: 0x060014D4 RID: 5332 RVA: 0x001439C8 File Offset: 0x00141BC8
		public static void TeaTasterSkill_SetActionPointGained(DataContext context, int value)
		{
			ProfessionData professionData = DomainManager.Extra.GetProfessionData(16);
			TeaTasterSkillsData skillsData = professionData.SkillsData as TeaTasterSkillsData;
			bool flag = skillsData != null;
			if (flag)
			{
				skillsData.ActionPointGained = value;
				DomainManager.Extra.SetProfessionData(context, professionData);
			}
		}

		// Token: 0x060014D5 RID: 5333 RVA: 0x00143A10 File Offset: 0x00141C10
		public static int TeaTasterSkill_GetActionPointGained()
		{
			ProfessionData professionData = DomainManager.Extra.GetProfessionData(16);
			int res = 0;
			TeaTasterSkillsData skillsData = professionData.SkillsData as TeaTasterSkillsData;
			bool flag = skillsData != null;
			if (flag)
			{
				res = skillsData.ActionPointGained;
			}
			return res;
		}

		// Token: 0x060014D6 RID: 5334 RVA: 0x00143A50 File Offset: 0x00141C50
		private static void ExecuteOnClick_TeaTasterSkill(DataContext context, ProfessionData professionData, int index, ref ProfessionSkillArg arg)
		{
			if (index != 3)
			{
				throw new Exception(professionData.GetSkillConfig(index).Name + " is not an executable skill.");
			}
			TasterUltimateResult result = DomainManager.Extra.CastTasterUltimateSkill(context, arg.CharIds, arg.BookIds, false);
			GameDataBridge.AddDisplayEvent<bool, TasterUltimateResult>(DisplayEventType.OpenTasterUltimateResult, false, result);
		}

		// Token: 0x060014D7 RID: 5335 RVA: 0x00143AAC File Offset: 0x00141CAC
		private static bool CheckSpecialCondition_TeaTasterSkill(ProfessionData professionData, int index)
		{
			return index != 3 || DomainManager.Extra.CheckTasterUltimateSpecialCondition(false) == 0;
		}

		// Token: 0x060014D8 RID: 5336 RVA: 0x00143ADC File Offset: 0x00141CDC
		private static void ExecuteOnClick_TravelerSkill(DataContext context, ProfessionData professionData, int index, ref ProfessionSkillArg arg)
		{
			if (index != 2)
			{
				if (index != 3)
				{
					throw new Exception(professionData.GetSkillConfig(index).Name + " is not an executable skill.");
				}
				ProfessionSkillHandle.ExecuteOnClick_TravelerSkill_3(context, professionData);
			}
			else
			{
				ProfessionSkillHandle.ExecuteOnClick_TravelerSkill_2(context, professionData, arg.ProfessionTravelerTargetLocation);
			}
		}

		// Token: 0x060014D9 RID: 5337 RVA: 0x00143B31 File Offset: 0x00141D31
		private static void ExecuteOnClick_TravelerSkill_2(DataContext context, ProfessionData professionData, Location targetLocation)
		{
			DomainManager.Map.TeleportByTraveler(context, targetLocation.BlockId);
		}

		// Token: 0x060014DA RID: 5338 RVA: 0x00143B48 File Offset: 0x00141D48
		private static void ExecuteOnClick_TravelerSkill_3(DataContext context, ProfessionData professionData)
		{
			Location location = DomainManager.Taiwu.GetTaiwu().GetLocation();
			DomainManager.Map.BuildTravelerPalace(context, location);
		}

		// Token: 0x060014DB RID: 5339 RVA: 0x00143B74 File Offset: 0x00141D74
		private static void ExecuteOnClick_TravelingBuddhistMonkSkill(DataContext context, ProfessionData professionData, int index, ref ProfessionSkillArg arg)
		{
			if (index != 3)
			{
				throw new Exception(professionData.GetSkillConfig(index).Name + " is not an executable skill.");
			}
			ProfessionSkillHandle.TravelingBuddhistMonkSkill3_SetFeature(context, arg.EffectId);
		}

		// Token: 0x060014DC RID: 5340 RVA: 0x00143BB8 File Offset: 0x00141DB8
		public static void TravelingBuddhistMonkSkill_SetTempleVisited(DataContext context, Location location)
		{
			ProfessionData professionData = DomainManager.Extra.GetProfessionData(12);
			TravelingBuddhistMonkSkillsData skillsData = professionData.GetSkillsData<TravelingBuddhistMonkSkillsData>();
			sbyte stateId = DomainManager.Map.GetStateIdByAreaId(location.AreaId);
			skillsData.OfflineSetStateTempleVisited(stateId);
			DomainManager.Extra.SetProfessionData(context, professionData);
			DomainManager.World.GetInstantNotificationCollection().AddVisitTemple(location, 5 - skillsData.GetVisitedTempleCount());
			int addSeniority = ProfessionFormulaImpl.Calculate(80);
			DomainManager.Extra.ChangeProfessionSeniority(context, 12, addSeniority, true, false);
		}

		// Token: 0x060014DD RID: 5341 RVA: 0x00143C34 File Offset: 0x00141E34
		public static bool TravelingBuddhistMonkSkill_CanVisitTemple(Location location)
		{
			bool flag = !ProfessionSkillHandle.IsSkillUnlocked(40);
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				ProfessionData professionData = DomainManager.Extra.GetProfessionData(12);
				TravelingBuddhistMonkSkillsData skillsData = professionData.GetSkillsData<TravelingBuddhistMonkSkillsData>();
				sbyte stateId = DomainManager.Map.GetStateIdByAreaId(location.AreaId);
				bool flag2 = skillsData.IsStateTempleVisited(stateId);
				if (flag2)
				{
					result = false;
				}
				else
				{
					Location stateTempleLocation = skillsData.GetStateTempleLocation(stateId);
					result = (stateTempleLocation.IsValid() && stateTempleLocation.Equals(location));
				}
			}
			return result;
		}

		// Token: 0x060014DE RID: 5342 RVA: 0x00143CB0 File Offset: 0x00141EB0
		public unsafe static void TravelingBuddhistMonkSkill_CreateTemples(DataContext context)
		{
			ProfessionData professionData = DomainManager.Extra.GetProfessionData(12);
			TravelingBuddhistMonkSkillsData skillsData = professionData.GetSkillsData<TravelingBuddhistMonkSkillsData>();
			skillsData.OfflineClearAllTampleState();
			short taiwuSettlementId = DomainManager.Taiwu.GetTaiwuVillageSettlementId();
			Span<short> span = new Span<short>(stackalloc byte[(UIntPtr)6], 3);
			Span<short> selectableAreas = span;
			span = new Span<short>(stackalloc byte[(UIntPtr)6], 3);
			Span<short> selectableBlocks = span;
			List<sbyte> indexList = new List<sbyte>(15);
			for (sbyte stateId = 0; stateId < 15; stateId += 1)
			{
				indexList.Add(stateId);
			}
			CollectionUtils.Shuffle<sbyte>(context.Random, indexList);
			for (int i = 0; i < 5; i++)
			{
				sbyte stateId2 = indexList[i];
				bool flag = skillsData.StateHasTemple(stateId2);
				if (!flag)
				{
					int selectableAreaCount = 0;
					for (int j = 0; j < 3; j++)
					{
						short areaId = (short)((int)(stateId2 * 3) + j);
						MapAreaData mapAreaData = DomainManager.Map.GetElement_Areas((int)areaId);
						bool flag2 = string.IsNullOrEmpty(mapAreaData.GetConfig().TempleName);
						if (!flag2)
						{
							*selectableAreas[selectableAreaCount] = areaId;
							selectableAreaCount++;
						}
					}
					int index = context.Random.Next(selectableAreaCount);
					short selectedAreaId = *selectableAreas[index];
					MapAreaData selectedAreaData = DomainManager.Map.GetElement_Areas((int)selectedAreaId);
					int selectableBlockCount = 0;
					foreach (SettlementInfo settlementInfo in selectedAreaData.SettlementInfos)
					{
						bool flag3 = settlementInfo.SettlementId < 0 || settlementInfo.SettlementId == taiwuSettlementId;
						if (!flag3)
						{
							*selectableBlocks[selectableBlockCount] = settlementInfo.BlockId;
							selectableBlockCount++;
						}
					}
					index = context.Random.Next(selectableBlockCount);
					short selectedBlockId = *selectableBlocks[index];
					Location location = new Location(selectedAreaId, selectedBlockId);
					skillsData.OfflineCreateTemple(stateId2, location);
					AdaptableLog.TagInfo("Profession", "Creating temple " + selectedAreaData.GetConfig().TempleName + " at " + location.ToString());
				}
			}
			DomainManager.Extra.SetProfessionData(context, professionData);
		}

		// Token: 0x060014DF RID: 5343 RVA: 0x00143ED8 File Offset: 0x001420D8
		public static List<string> TravelingBuddhistMonkSkill_GetTempleNames()
		{
			ProfessionData professionData = DomainManager.Extra.GetProfessionData(12);
			TravelingBuddhistMonkSkillsData skillsData = professionData.GetSkillsData<TravelingBuddhistMonkSkillsData>();
			List<string> templeNames = new List<string>();
			for (sbyte stateId = 0; stateId < 15; stateId += 1)
			{
				bool flag = skillsData.StateHasTemple(stateId);
				if (flag)
				{
					Location location = skillsData.GetStateTempleLocation(stateId);
					MapAreaData areaData = DomainManager.Map.GetElement_Areas((int)location.AreaId);
					templeNames.Add(areaData.GetConfig().TempleName);
				}
			}
			return templeNames;
		}

		// Token: 0x060014E0 RID: 5344 RVA: 0x00143F5C File Offset: 0x0014215C
		public static void TravelingBuddhistMonkSkill3_SetFeature(DataContext context, short featureId)
		{
			GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
			taiwu.AddFeature(context, featureId, true);
			ProfessionData professionData = DomainManager.Extra.GetProfessionData(12);
			TravelingBuddhistMonkSkillsData skillsData = professionData.GetSkillsData<TravelingBuddhistMonkSkillsData>();
			skillsData.OfflineSetSelectedSkill3FeatureId(featureId);
			DomainManager.Extra.SetProfessionData(context, professionData);
		}

		// Token: 0x060014E1 RID: 5345 RVA: 0x00143FA8 File Offset: 0x001421A8
		private static bool CheckSpecialCondition_TravelingBuddhistMonkSkill(ProfessionData professionData, int index)
		{
			return index != 3 || ProfessionSkillHandle.CheckSpecialCondition_TravelingBuddhistMonkSkill_3(professionData);
		}

		// Token: 0x060014E2 RID: 5346 RVA: 0x00143FD0 File Offset: 0x001421D0
		private static bool CheckSpecialCondition_TravelingBuddhistMonkSkill_3(ProfessionData professionData)
		{
			return true;
		}

		// Token: 0x060014E3 RID: 5347 RVA: 0x00143FE4 File Offset: 0x001421E4
		private static bool CheckSpecialCondition_TravelingTaoistMonkSkill(ProfessionData professionData, int index)
		{
			return index != 3 || ProfessionSkillHandle.CheckSpecialCondition_TravelingTaoistMonkSkill_3(professionData);
		}

		// Token: 0x060014E4 RID: 5348 RVA: 0x0014400C File Offset: 0x0014220C
		private static bool CheckSpecialCondition_TravelingTaoistMonkSkill_3(ProfessionData professionData)
		{
			return true;
		}

		// Token: 0x060014E5 RID: 5349 RVA: 0x00144020 File Offset: 0x00142220
		private static void ExecuteOnClick_TravelingTaoistMonkSkill(DataContext context, ProfessionData professionData, int index, ref ProfessionSkillArg arg)
		{
			if (index != 2)
			{
				throw new Exception(professionData.GetSkillConfig(index).Name + " is not an executable skill.");
			}
			ProfessionSkillHandle.TravelingTaoistMonkSkill2(context, professionData, arg);
		}

		// Token: 0x060014E6 RID: 5350 RVA: 0x00144060 File Offset: 0x00142260
		private static void TravelingTaoistMonkSkill2(DataContext context, ProfessionData professionData, ProfessionSkillArg arg)
		{
			int rightCharId = arg.CharIds.FirstOrDefault<int>();
			GameData.Domains.Character.Character targetChar = DomainManager.Character.GetElement_Objects(rightCharId);
			GameData.Domains.Character.Character leftChar = DomainManager.Character.GetElement_Objects(arg.CharId);
			GameData.Domains.Character.Character taiwuChar = DomainManager.Taiwu.GetTaiwu();
			List<int> leftFeatureIds = arg.BookIds;
			List<short> rightFeatureIds = arg.EffectBlocks;
			int preBadCount = EventHelper.GetBadFeatureCount(targetChar);
			foreach (int id in leftFeatureIds)
			{
				leftChar.RemoveFeature(context, (short)id);
			}
			foreach (short id2 in rightFeatureIds)
			{
				targetChar.RemoveFeature(context, id2);
			}
			foreach (short id3 in rightFeatureIds)
			{
				leftChar.AddFeature(context, id3, false);
			}
			foreach (int id4 in leftFeatureIds)
			{
				targetChar.AddFeature(context, (short)id4, false);
			}
			int curBadCount = EventHelper.GetBadFeatureCount(targetChar);
			int difference = curBadCount - preBadCount;
			EventHelper.ChangeFavorabilityOptionalRepeatedEvent(targetChar, taiwuChar, (short)(-difference * ProfessionRelatedConstants.TravelingTaoistMonkSkill2FavorValue));
			int levelSum = 0;
			foreach (short id5 in rightFeatureIds)
			{
				levelSum += (int)Math.Abs(CharacterFeature.Instance[id5].Level);
			}
			int curSeniority = professionData.Seniority;
			int maxSeniority = 3000000;
			int hpCost = levelSum * (9 - 6 * curSeniority / maxSeniority);
			short health = taiwuChar.GetBaseMaxHealth();
			health = (short)Math.Max((int)health - hpCost, 0);
			int trulyCost = (int)(taiwuChar.GetBaseMaxHealth() - health);
			taiwuChar.SetBaseMaxHealth(health, context);
			ProfessionFormulaItem formula = ProfessionFormula.Instance[93];
			int addSeniority = formula.Calculate(trulyCost);
			DomainManager.Extra.ChangeProfessionSeniority(context, 14, addSeniority, true, false);
			DomainManager.TaiwuEvent.SetListenerEventActionBoolArg("TravelingTaoistMonkSkill2Executed", "ConchShip_PresetKey_FinishSkillExecute", true);
			DomainManager.TaiwuEvent.TriggerListener("TravelingTaoistMonkSkill2Executed", true);
		}

		// Token: 0x060014E7 RID: 5351 RVA: 0x001442F4 File Offset: 0x001424F4
		public static int TravelingTaoistMonkSkill1_ExpCost(GameData.Domains.Character.Character targetChar)
		{
			sbyte grade = targetChar.GetOrganizationInfo().Grade;
			return (int)((grade + 1) * (grade + 1)) * 200;
		}

		// Token: 0x060014E8 RID: 5352 RVA: 0x00144320 File Offset: 0x00142520
		private static void TravelingTaoistMonkSkill_ClearHealthBonus(DataContext context)
		{
			ProfessionData professionData = DomainManager.Extra.GetProfessionData(14);
			TravelingTaoistMonkSkillsData skillsData = professionData.GetSkillsData<TravelingTaoistMonkSkillsData>();
			skillsData.BonusMaxHealth = 0;
			DomainManager.Extra.SetProfessionData(context, professionData);
		}

		// Token: 0x060014E9 RID: 5353 RVA: 0x00144358 File Offset: 0x00142558
		public static short TravelingTaoistMonkSkill_GetMaxHealthBonus()
		{
			ProfessionData professionData = DomainManager.Extra.GetProfessionData(14);
			bool flag = professionData == null;
			short result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				TravelingTaoistMonkSkillsData skillsData = professionData.GetSkillsData<TravelingTaoistMonkSkillsData>();
				result = skillsData.BonusMaxHealth;
			}
			return result;
		}

		// Token: 0x060014EA RID: 5354 RVA: 0x00144390 File Offset: 0x00142590
		private static void TravelingTaoistMonkSkill_OnPreAdvanceMonth(DataContext context)
		{
			bool flag = !ProfessionSkillHandle.IsSkillUnlocked(48);
			if (!flag)
			{
				ProfessionData professionData = DomainManager.Extra.GetProfessionData(14);
				bool flag2 = ProfessionSkillHandle.CheckSpecialCondition(professionData, 3);
				if (flag2)
				{
					GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
					bool flag3 = taiwu.GetBirthMonth() == DomainManager.World.GetCurrMonthInYear();
					if (flag3)
					{
						bool flag4 = taiwu.IsAgeIncreaseStopped();
						if (!flag4)
						{
							short age = taiwu.GetCurrAge();
							age += 2;
							taiwu.SetCurrAge(age, context);
							TravelingTaoistMonkSkillsData skillsData = professionData.GetSkillsData<TravelingTaoistMonkSkillsData>();
							TravelingTaoistMonkSkillsData travelingTaoistMonkSkillsData = skillsData;
							travelingTaoistMonkSkillsData.BonusMaxHealth += 36;
							DomainManager.Extra.SetProfessionData(context, professionData);
							short baseMaxHealth = taiwu.GetBaseMaxHealth();
							taiwu.SetBaseMaxHealth(baseMaxHealth, context);
						}
					}
				}
			}
		}

		// Token: 0x060014EB RID: 5355 RVA: 0x00144454 File Offset: 0x00142654
		private static void WineTasterSkill_UpdateVillagerLearnSkillInterval(DataContext context)
		{
			bool flag = !DomainManager.Taiwu.GetVillagerLearnCombatSkillsFromSect();
			if (!flag)
			{
				ProfessionData professionData = DomainManager.Extra.GetProfessionData(7);
				int currDate = DomainManager.World.GetCurrDate();
				WineTasterSkillsData skillsData = professionData.SkillsData as WineTasterSkillsData;
				bool flag2 = skillsData != null && skillsData.VillagersLastLearnSkillDate + 3 < currDate;
				if (flag2)
				{
					skillsData.VillagersLastLearnSkillDate = currDate;
					DomainManager.Extra.SetProfessionData(context, professionData);
				}
			}
		}

		// Token: 0x060014EC RID: 5356 RVA: 0x001444C8 File Offset: 0x001426C8
		private static void ExecuteOnClick_WineTasterSkill(DataContext context, ProfessionData professionData, int index, ref ProfessionSkillArg arg)
		{
			if (index != 3)
			{
				throw new Exception(professionData.GetSkillConfig(index).Name + " is not an executable skill.");
			}
			TasterUltimateResult result = DomainManager.Extra.CastTasterUltimateSkill(context, arg.CharIds, arg.BookIds, true);
			GameDataBridge.AddDisplayEvent<bool, TasterUltimateResult>(DisplayEventType.OpenTasterUltimateResult, true, result);
		}

		// Token: 0x060014ED RID: 5357 RVA: 0x00144524 File Offset: 0x00142724
		private static bool CheckSpecialCondition_WineTasterSkill(ProfessionData professionData, int index)
		{
			return index != 3 || DomainManager.Extra.CheckTasterUltimateSpecialCondition(true) == 0;
		}

		// Token: 0x060014EF RID: 5359 RVA: 0x0014456C File Offset: 0x0014276C
		[CompilerGenerated]
		internal unsafe static bool <ExecuteOnClick_DoctorSkill_1>g__TryTreatCharacter|64_0(GameData.Domains.Character.Character character, ref ProfessionSkillHandle.<>c__DisplayClass64_0 A_1)
		{
			Injuries injuries = character.GetInjuries();
			PoisonInts poisons = *character.GetPoisoned();
			short disorderOfQi = character.GetDisorderOfQi();
			bool flag = disorderOfQi <= 0 && !poisons.IsNonZero() && injuries.GetSum() <= 0;
			bool result2;
			if (flag)
			{
				result2 = false;
			}
			else
			{
				Injuries healedInjuries = DomainManager.Combat.HealInjury(character.GetId(), A_1.taiwu, false);
				character.SetInjuries(healedInjuries, A_1.context);
				PoisonInts result = DomainManager.Combat.HealPoison(character.GetId(), A_1.taiwu, false);
				character.SetPoisoned(ref result, A_1.context);
				character.ChangeDisorderOfQi(A_1.context, (int)A_1.disorderOfQiDelta);
				int spiritualDebt = (int)(character.GetOrganizationInfo().Grade + 1) * (10 + 10 * A_1.professionData.Seniority / 3000000);
				DomainManager.Extra.ChangeAreaSpiritualDebt(A_1.context, A_1.location.AreaId, spiritualDebt, true, true);
				DomainManager.Character.ChangeFavorabilityOptionalRepeatedEvent(A_1.context, character, A_1.taiwu, 6000);
				result2 = true;
			}
			return result2;
		}

		// Token: 0x060014F0 RID: 5360 RVA: 0x0014468C File Offset: 0x0014288C
		[CompilerGenerated]
		internal static bool <CheckSpecialCondition_HunterSkill>g__CheckItem|90_0()
		{
			GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
			List<ItemDisplayData> inventoryItems = DomainManager.Character.GetAllInventoryItems(taiwu.GetId());
			return (from d in inventoryItems
			select d.Key).Any(new Func<ItemKey, bool>(ProfessionSkillHandle.IsItemCanConvertToAnimalCharacter));
		}

		// Token: 0x060014F1 RID: 5361 RVA: 0x001446F0 File Offset: 0x001428F0
		[CompilerGenerated]
		internal unsafe static bool <MartialArtistSkill_MakeAreaLearnCombatSkill>g__TryLearnCombatSkill|100_0(int charId, ref ProfessionSkillHandle.<>c__DisplayClass100_0 A_1)
		{
			GameData.Domains.Character.Character character = DomainManager.Character.GetElement_Objects(charId);
			bool flag = character.GetAgeGroup() != 2;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				Dictionary<short, GameData.Domains.CombatSkill.CombatSkill> learnedCombatSkills = DomainManager.CombatSkill.GetCharCombatSkills(charId);
				sbyte grade = 8;
				A_1.selectableSkillIds.Clear();
				foreach (CombatSkillItem combatSkillCfg in ((IEnumerable<CombatSkillItem>)Config.CombatSkill.Instance))
				{
					bool flag2 = A_1.combatSkillType != combatSkillCfg.Type;
					if (!flag2)
					{
						bool flag3 = combatSkillCfg.SectId == 0;
						if (!flag3)
						{
							bool flag4 = combatSkillCfg.Grade > grade;
							if (!flag4)
							{
								bool flag5 = combatSkillCfg.BookId < 0;
								if (!flag5)
								{
									bool flag6 = learnedCombatSkills.ContainsKey(combatSkillCfg.TemplateId);
									if (!flag6)
									{
										bool flag7 = combatSkillCfg.Grade < grade;
										if (flag7)
										{
											grade = combatSkillCfg.Grade;
											A_1.selectableSkillIds.Clear();
										}
										A_1.selectableSkillIds.Add(combatSkillCfg.TemplateId);
									}
								}
							}
						}
					}
				}
				bool flag8 = A_1.selectableSkillIds.Count == 0;
				if (flag8)
				{
					result = false;
				}
				else
				{
					short selectedTemplateId = A_1.selectableSkillIds.GetRandom(A_1.context.Random);
					short bookTemplateId = Config.CombatSkill.Instance[selectedTemplateId].BookId;
					SkillBookItem bookCfg = Config.SkillBook.Instance[bookTemplateId];
					CombatSkillShorts combatSkillAttainments = *character.GetCombatSkillAttainments();
					CombatSkillShorts combatSkillQualifications = *character.GetCombatSkillQualifications();
					Personalities personalities = character.GetPersonalities();
					int successRate = GameData.Domains.Character.Character.GetTaughtNewSkillSuccessRate(bookCfg.Grade, *(ref combatSkillQualifications.Items.FixedElementField + (IntPtr)bookCfg.CombatSkillType * 2), *(ref combatSkillAttainments.Items.FixedElementField + (IntPtr)bookCfg.CombatSkillType * 2), *(ref personalities.Items.FixedElementField + 1));
					bool flag9 = !A_1.context.Random.CheckPercentProb(successRate);
					if (flag9)
					{
						result = false;
					}
					else
					{
						ItemKey itemKey = DomainManager.Item.CreateSkillBook(A_1.context, bookTemplateId, -1, -1, -1, 50, true);
						character.AddInventoryItem(A_1.context, itemKey, 1, false);
						character.LearnNewCombatSkill(A_1.context, selectedTemplateId, 0);
						int favorChange = (int)(grade + 1) * 1000;
						DomainManager.Character.ChangeFavorabilityOptionalRepeatedEvent(A_1.context, character, A_1.taiwu, favorChange);
						A_1.lifeRecordCollection.AddLearnCombatSkill(charId, A_1.currDate, character.GetLocation(), selectedTemplateId);
						result = true;
					}
				}
			}
			return result;
		}

		// Token: 0x0400032D RID: 813
		public static readonly IReadOnlyList<short> AnimalCharacterTemplateIds = new short[]
		{
			210,
			211,
			212,
			213,
			214,
			215,
			216,
			217,
			218,
			219,
			220,
			221,
			222,
			223,
			224,
			225,
			226,
			227
		};
	}
}
