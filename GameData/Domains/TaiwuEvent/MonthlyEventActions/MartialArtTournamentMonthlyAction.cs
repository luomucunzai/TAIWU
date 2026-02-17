using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Config;
using Config.ConfigCells.Character;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Adventure;
using GameData.Domains.Character;
using GameData.Domains.Character.Creation;
using GameData.Domains.CombatSkill;
using GameData.Domains.Item;
using GameData.Domains.Map;
using GameData.Domains.Organization;
using GameData.Domains.TaiwuEvent.EventManager;
using GameData.Domains.World.MonthlyEvent;
using GameData.Domains.World.Notification;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.TaiwuEvent.MonthlyEventActions
{
	// Token: 0x02000091 RID: 145
	[SerializableGameData(NotForDisplayModule = true)]
	public class MartialArtTournamentMonthlyAction : MonthlyActionBase, ISerializableGameData
	{
		// Token: 0x170000CC RID: 204
		// (get) Token: 0x06001941 RID: 6465 RVA: 0x0016BA71 File Offset: 0x00169C71
		public MonthlyActionsItem ConfigData
		{
			get
			{
				return MonthlyActions.Instance[30];
			}
		}

		// Token: 0x06001942 RID: 6466 RVA: 0x0016BA7F File Offset: 0x00169C7F
		public MartialArtTournamentMonthlyAction()
		{
			this.CurrentHost = -1;
			this.LastFinishDate = int.MinValue;
			this.MajorCharacterSets = new List<CharacterSet>();
			this.ParticipatingCharacterSets = new List<CharacterSet>();
		}

		// Token: 0x06001943 RID: 6467 RVA: 0x0016BAB4 File Offset: 0x00169CB4
		public override void MonthlyHandler()
		{
			bool tmpSkipMonth = this._tmpSkipMonth;
			if (tmpSkipMonth)
			{
				this._tmpSkipMonth = false;
			}
			else
			{
				DataContext context = DomainManager.TaiwuEvent.MainThreadDataContext;
				bool flag = DomainManager.World.GetMainStoryLineProgress() >= 27;
				if (flag)
				{
					bool flag2 = this.State > 0;
					if (flag2)
					{
						this.State = 0;
						this.ClearCalledCharacters();
						this.CurrentHost = -1;
						this.Month = 0;
						bool flag3 = this.Location.IsValid();
						if (flag3)
						{
							DomainManager.Adventure.RemoveAdventureSite(context, this.Location.AreaId, this.Location.BlockId, false, false);
						}
						this.Location = Location.Invalid;
					}
				}
				else
				{
					bool flag4 = this.State == 0;
					if (flag4)
					{
						bool flag5 = !DomainManager.World.GetWorldFunctionsStatus(25);
						if (flag5)
						{
							return;
						}
						bool flag6 = this.LastFinishDate + 108 > DomainManager.World.GetCurrDate();
						if (flag6)
						{
							return;
						}
						this.State = 1;
						this.Month = 0;
						DomainManager.World.GetMonthlyNotificationCollection().AddWulinConferenceInPreparing();
					}
					bool flag7 = this.State == 1;
					if (flag7)
					{
						bool flag8 = this.Month >= 12;
						if (flag8)
						{
							bool flag9 = this.Month == 12 || !this.Location.IsValid();
							if (flag9)
							{
								this.FinishPreparation();
							}
							else
							{
								this.CallMajorCharacters();
								this.CallParticipateCharacters();
							}
						}
						else
						{
							this.SectAskForHelp();
						}
					}
					bool flag10 = this.State == 2;
					if (flag10)
					{
						this.CallParticipateCharacters();
					}
					bool flag11 = this.State == 2 || this.State == 3;
					if (flag11)
					{
						bool isTimeUp = this.Month >= 18;
						bool flag12 = (CallCharacterHelper.IsAllCharactersAtLocation(this.Location, this.ConfigData.MajorTargetFilterList, this.MajorCharacterSets) && CallCharacterHelper.IsAllCharactersAtLocation(this.Location, this.ConfigData.ParticipateTargetFilterList, this.ParticipatingCharacterSets)) || isTimeUp;
						if (flag12)
						{
							this.State = 5;
							this.Month = 0;
							this.Activate();
						}
					}
					bool flag13 = this.State != 0;
					if (flag13)
					{
						this.Month++;
					}
				}
			}
		}

		// Token: 0x06001944 RID: 6468 RVA: 0x0016BCFC File Offset: 0x00169EFC
		private void FinishPreparation()
		{
			DataContext context = DomainManager.TaiwuEvent.MainThreadDataContext;
			List<MartialArtTournamentPreparationInfo> preparationInfoList = DomainManager.Organization.GetMartialArtTournamentPreparationInfoList();
			MartialArtTournamentPreparationInfo selectedSect = preparationInfoList.Max<MartialArtTournamentPreparationInfo>();
			this.CurrentHost = selectedSect.SettlementId;
			Sect sect = DomainManager.Organization.GetElement_Sects(this.CurrentHost);
			Location location = sect.GetLocation();
			this.Location = Location.Invalid;
			List<short> validBlockList = ObjectPool<List<short>>.Instance.Get();
			validBlockList.Clear();
			DomainManager.Map.GetSettlementBlocks(location.AreaId, location.BlockId, validBlockList);
			AreaAdventureData adventuresInArea = DomainManager.Adventure.GetAdventuresInArea(location.AreaId);
			validBlockList.RemoveAll((short blockId) => adventuresInArea.AdventureSites.ContainsKey(blockId));
			bool flag = validBlockList.Count == 0;
			if (flag)
			{
				DomainManager.Map.GetSettlementBlocks(location.AreaId, location.BlockId, validBlockList);
			}
			CollectionUtils.Shuffle<short>(context.Random, validBlockList);
			foreach (short blockId2 in validBlockList)
			{
				bool flag2 = DomainManager.Adventure.TryCreateAdventureSite(context, location.AreaId, blockId2, this.ConfigData.AdventureId, this.Key);
				if (flag2)
				{
					this.Location = new Location(location.AreaId, blockId2);
					break;
				}
			}
			bool flag3 = !this.Location.IsValid();
			if (flag3)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(32, 1);
				defaultInterpolatedStringHandler.AppendLiteral("No valid location for adventure ");
				defaultInterpolatedStringHandler.AppendFormatted<AdventureItem>(Adventure.Instance[this.ConfigData.AdventureId]);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			this.CallMajorCharacters();
			this.CallParticipateCharacters();
		}

		// Token: 0x06001945 RID: 6469 RVA: 0x0016BED8 File Offset: 0x0016A0D8
		private void SectAskForHelp()
		{
			MonthlyEventCollection monthlyEventCollection = DomainManager.World.GetMonthlyEventCollection();
			DataContext context = DomainManager.TaiwuEvent.MainThreadDataContext;
			int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
			List<MartialArtTournamentPreparationInfo> preparingForMartialArtTournamentSects = DomainManager.Organization.GetMartialArtTournamentPreparationInfoList();
			List<short> availableSects = ObjectPool<List<short>>.Instance.Get();
			availableSects.Clear();
			foreach (MartialArtTournamentPreparationInfo preparationInfo in preparingForMartialArtTournamentSects)
			{
				short settlementId = preparationInfo.SettlementId;
				Sect sect = DomainManager.Organization.GetElement_Sects(settlementId);
				short approvingRate = sect.CalcApprovingRate();
				bool flag = approvingRate >= 500;
				if (flag)
				{
					availableSects.Add(settlementId);
				}
			}
			bool flag2 = availableSects.Count == 0;
			if (flag2)
			{
				ObjectPool<List<short>>.Instance.Return(availableSects);
			}
			else
			{
				short selectedSettlementId = availableSects.GetRandom(context.Random);
				monthlyEventCollection.AddWulinConferenceAskForHelp(selectedSettlementId, taiwuCharId);
				ObjectPool<List<short>>.Instance.Return(availableSects);
			}
		}

		// Token: 0x06001946 RID: 6470 RVA: 0x0016BFE8 File Offset: 0x0016A1E8
		public override void Activate()
		{
			DataContext context = DomainManager.TaiwuEvent.MainThreadDataContext;
			Sect sect = DomainManager.Organization.GetElement_Sects(this.CurrentHost);
			MonthlyEventActionsManager.NewlyActivated++;
			DomainManager.Adventure.ActivateAdventureSite(context, this.Location.AreaId, this.Location.BlockId);
			MonthlyNotificationCollection monthlyNotificationCollection = DomainManager.World.GetMonthlyNotificationCollection();
			monthlyNotificationCollection.AddWulinConferenceInProgress(sect.GetLocation());
		}

		// Token: 0x06001947 RID: 6471 RVA: 0x0016C058 File Offset: 0x0016A258
		public override void Deactivate(bool isComplete)
		{
			DataContext context = DomainManager.TaiwuEvent.MainThreadDataContext;
			this.ClearCalledCharacters();
			this.Location = Location.Invalid;
			this.State = 0;
			bool flag = this.CurrentHost < 0;
			if (!flag)
			{
				List<short> previousHosts = DomainManager.Organization.GetPreviousMartialArtTournamentHosts();
				if (isComplete)
				{
					this.LastFinishDate = DomainManager.World.GetCurrDate();
					previousHosts.Add(this.CurrentHost);
					DomainManager.Organization.SetPreviousMartialArtTournamentHosts(previousHosts, context);
					this.CurrentHost = -1;
				}
				else
				{
					bool flag2 = previousHosts.Count == 0;
					if (flag2)
					{
						this.CurrentHost = -1;
						MonthlyEventCollection monthlyEventCollection = DomainManager.World.GetMonthlyEventCollection();
						monthlyEventCollection.AddWulinConferenceTaiwuAbsent();
						this._tmpSkipMonth = true;
					}
					else
					{
						sbyte winnerSectId = this.SelectWinner();
						MartialArtTournamentMonthlyAction.AddMartialArtTournamentWinnerPrize(context, winnerSectId);
						MonthlyNotificationCollection monthlyNotifications = DomainManager.World.GetMonthlyNotificationCollection();
						short settlementId = DomainManager.Organization.GetSettlementIdByOrgTemplateId(winnerSectId);
						monthlyNotifications.AddWulinConferenceWinner(settlementId);
						this.LastFinishDate = DomainManager.World.GetCurrDate();
						previousHosts.Add(this.CurrentHost);
						DomainManager.Organization.SetPreviousMartialArtTournamentHosts(previousHosts, context);
						this.CurrentHost = -1;
					}
				}
			}
		}

		// Token: 0x06001948 RID: 6472 RVA: 0x0016C184 File Offset: 0x0016A384
		private unsafe sbyte SelectWinner()
		{
			Span<int> span = new Span<int>(stackalloc byte[(UIntPtr)60], 15);
			Span<int> sectMaxPowers = span;
			span = new Span<int>(stackalloc byte[(UIntPtr)60], 15);
			Span<int> powerSums = span;
			span = new Span<int>(stackalloc byte[(UIntPtr)60], 15);
			Span<int> charCounts = span;
			sectMaxPowers.Fill(0);
			foreach (CharacterSet charSet in this.ParticipatingCharacterSets)
			{
				foreach (int charId in charSet.GetCollection())
				{
					GameData.Domains.Character.Character character;
					bool flag = !DomainManager.Character.TryGetElement_Objects(charId, out character);
					if (!flag)
					{
						sbyte sectId = character.GetOrganizationInfo().OrgTemplateId;
						bool flag2 = !OrganizationDomain.IsSect(sectId);
						if (!flag2)
						{
							int stateId = (int)(sectId - 1);
							int combatPower = character.GetCombatPower();
							bool flag3 = combatPower > *sectMaxPowers[stateId];
							if (flag3)
							{
								*sectMaxPowers[stateId] = combatPower;
							}
							*powerSums[stateId] += combatPower;
							(*charCounts[stateId])++;
						}
					}
				}
			}
			for (int i = 0; i < powerSums.Length; i++)
			{
				bool flag4 = *charCounts[i] > 0;
				if (flag4)
				{
					*sectMaxPowers[i] += *powerSums[i] / *charCounts[i];
				}
			}
			int maxPowerIndex = CollectionUtils.GetMaxIndex(sectMaxPowers);
			int winnerSectId = 1 + maxPowerIndex;
			return (sbyte)winnerSectId;
		}

		// Token: 0x06001949 RID: 6473 RVA: 0x0016C350 File Offset: 0x0016A550
		public unsafe static void AddMartialArtTournamentWinnerPrize(DataContext context, sbyte sectTemplateId)
		{
			DomainManager.Extra.RegisterMartialArtTournamentWinner(context, (short)sectTemplateId);
			int gainEquipmentCount = 3;
			List<TemplateKey> presetItems = new List<TemplateKey>();
			Settlement settlement = DomainManager.Organization.GetSettlementByOrgTemplateId(sectTemplateId);
			OrgMemberCollection members = settlement.GetMembers();
			for (sbyte grade = 0; grade <= 8; grade += 1)
			{
				HashSet<int> gradeMembers = members.GetMembers(grade);
				foreach (int charId in gradeMembers)
				{
					GameData.Domains.Character.Character character;
					bool flag = !DomainManager.Character.TryGetElement_Objects(charId, out character);
					if (!flag)
					{
						OrganizationInfo orgInfo = character.GetOrganizationInfo();
						OrganizationMemberItem orgMemberCfg = OrganizationDomain.GetOrgMemberConfig(orgInfo);
						ArraySegmentList<short> attackSkills = character.GetCombatSkillEquipment().Attack;
						presetItems.Clear();
						for (int index = 0; index < attackSkills.Count; index++)
						{
							short skillTemplateId = *attackSkills[index];
							bool flag2 = skillTemplateId < 0;
							if (!flag2)
							{
								CombatSkillItem skillCfg = Config.CombatSkill.Instance[skillTemplateId];
								bool flag3 = skillCfg.MostFittingWeaponID < 0;
								if (!flag3)
								{
									presetItems.Add(new TemplateKey(0, skillCfg.MostFittingWeaponID));
								}
							}
						}
						for (int i = 0; i < orgMemberCfg.Equipment.Length; i++)
						{
							PresetEquipmentItemWithProb presetItem = orgMemberCfg.Equipment[i];
							bool flag4 = presetItem.Type < 0 || presetItem.TemplateId < 0;
							if (!flag4)
							{
								presetItems.Add(new TemplateKey(presetItem.Type, presetItem.TemplateId));
							}
						}
						CollectionUtils.Shuffle<TemplateKey>(context.Random, presetItems);
						bool flag5 = gainEquipmentCount > presetItems.Count;
						if (flag5)
						{
							gainEquipmentCount = presetItems.Count;
						}
						for (int j = 0; j < gainEquipmentCount; j++)
						{
							TemplateKey presetItem2 = presetItems[j];
							short templateId = presetItem2.TemplateId + (short)orgInfo.Grade;
							character.CreateInventoryItem(context, presetItem2.ItemType, templateId, 1);
						}
						MartialArtTournamentMonthlyAction.WinnerLearnCombatSkills(context, character);
						MartialArtTournamentMonthlyAction.WinnerLearnLifeSkills(context, character);
					}
				}
			}
		}

		// Token: 0x0600194A RID: 6474 RVA: 0x0016C598 File Offset: 0x0016A798
		private static void WinnerLearnCombatSkills(DataContext context, GameData.Domains.Character.Character character)
		{
			OrganizationInfo orgInfo = character.GetOrganizationInfo();
			sbyte behaviorType = character.GetBehaviorType();
			Dictionary<short, GameData.Domains.CombatSkill.CombatSkill> learnedSkills = DomainManager.CombatSkill.GetCharCombatSkills(character.GetId());
			List<sbyte> skillTypes = Organization.Instance[orgInfo.OrgTemplateId].CombatSkillTypes;
			List<short> skillsToLearn = ObjectPool<List<short>>.Instance.Get();
			skillsToLearn.Clear();
			foreach (sbyte skillType in skillTypes)
			{
				IReadOnlyList<CombatSkillItem> learnableSkills = CombatSkillDomain.GetLearnableCombatSkills(orgInfo.OrgTemplateId, skillType);
				int index = 0;
				while (index < learnableSkills.Count)
				{
					CombatSkillItem skillCfg = learnableSkills[index];
					GameData.Domains.CombatSkill.CombatSkill skill;
					bool flag = learnedSkills.TryGetValue(skillCfg.TemplateId, out skill);
					if (flag)
					{
						bool flag2 = !skill.CanBreakout() || CombatSkillStateHelper.IsBrokenOut(skill.GetActivationState());
						if (!flag2)
						{
							skillsToLearn.Add(skillCfg.TemplateId);
						}
					}
					else
					{
						skillsToLearn.Add(skillCfg.TemplateId);
					}
					IL_DA:
					index++;
					continue;
					goto IL_DA;
				}
			}
			CollectionUtils.Shuffle<short>(context.Random, skillsToLearn);
			int learnSkillCount = (int)MartialArtTournamentMonthlyAction.WinnerLearnCombatSkillCounts[(int)orgInfo.Grade];
			bool flag3 = learnSkillCount > skillsToLearn.Count;
			if (flag3)
			{
				learnSkillCount = skillsToLearn.Count;
			}
			for (int i = 0; i < learnSkillCount; i++)
			{
				short skillTemplateId = skillsToLearn[i];
				GameData.Domains.CombatSkill.CombatSkill skill2;
				bool flag4 = learnedSkills.TryGetValue(skillTemplateId, out skill2);
				if (flag4)
				{
					skill2.SetPracticeLevel(100, context);
					skill2.SetReadingState(32767, context);
					ushort activationState = CombatSkillStateHelper.GenerateRandomActivatedNormalPages(context.Random, 32767, 0);
					activationState = CombatSkillStateHelper.GenerateRandomActivatedOutlinePage(context.Random, 32767, activationState, behaviorType);
					skill2.SetActivationState(activationState, context);
					skill2.SetBreakoutStepsCount(GlobalConfig.Instance.BreakoutSpecialNpcStepsCount, context);
					skill2.SetForcedBreakoutStepsCount(0, context);
				}
				else
				{
					character.LearnNewCombatSkill(context, skillTemplateId, 32767);
				}
			}
		}

		// Token: 0x0600194B RID: 6475 RVA: 0x0016C7B4 File Offset: 0x0016A9B4
		private static void WinnerLearnLifeSkills(DataContext context, GameData.Domains.Character.Character character)
		{
			OrganizationInfo orgInfo = character.GetOrganizationInfo();
			OrganizationMemberItem orgMemberCfg = OrganizationDomain.GetOrgMemberConfig(orgInfo);
			List<GameData.Domains.Character.LifeSkillItem> learnedLifeSkills = character.GetLearnedLifeSkills();
			List<short> skillsToLearn = ObjectPool<List<short>>.Instance.Get();
			skillsToLearn.Clear();
			sbyte lifeSkillType = 0;
			while ((int)lifeSkillType < orgMemberCfg.LifeSkillsAdjust.Length)
			{
				short adjust = orgMemberCfg.LifeSkillsAdjust[(int)lifeSkillType];
				bool flag = adjust <= 0;
				if (!flag)
				{
					for (int i = 0; i <= (int)orgMemberCfg.LifeSkillGradeLimit; i++)
					{
						short lifeSkillTemplateId = Config.LifeSkillType.Instance[lifeSkillType].SkillList[i];
						int index = character.FindLearnedLifeSkillIndex(lifeSkillTemplateId);
						bool flag2 = index < 0;
						if (flag2)
						{
							skillsToLearn.Add(lifeSkillTemplateId);
						}
						else
						{
							GameData.Domains.Character.LifeSkillItem learnedLifeSkill = learnedLifeSkills[index];
							bool flag3 = !learnedLifeSkill.IsAllPagesRead();
							if (flag3)
							{
								skillsToLearn.Add(learnedLifeSkill.SkillTemplateId);
							}
						}
					}
				}
				lifeSkillType += 1;
			}
			CollectionUtils.Shuffle<short>(context.Random, skillsToLearn);
			int learnSkillCount = (int)MartialArtTournamentMonthlyAction.WinnerLearnLifeSkillCounts[(int)orgInfo.Grade];
			bool flag4 = skillsToLearn.Count < learnSkillCount;
			if (flag4)
			{
				learnSkillCount = skillsToLearn.Count;
			}
			for (int j = 0; j < learnSkillCount; j++)
			{
				short skillTemplateId = skillsToLearn[j];
				int index2 = character.FindLearnedLifeSkillIndex(skillTemplateId);
				bool flag5 = index2 < 0;
				if (flag5)
				{
					character.LearnNewLifeSkill(context, skillTemplateId, 31);
				}
				else
				{
					character.UpdateLifeSkillReadingState(context, index2, 31);
				}
			}
		}

		// Token: 0x0600194C RID: 6476 RVA: 0x0016C934 File Offset: 0x0016AB34
		public override void CollectCalledCharacters(HashSet<int> calledCharacters)
		{
			bool flag = this.ConfigData.MinInterval <= 0 && this.Key.ActionType == 0;
			if (flag)
			{
				this.MajorCharacterSets.ForEach(delegate(CharacterSet charSet)
				{
					charSet.Clear();
				});
				this.ParticipatingCharacterSets.ForEach(delegate(CharacterSet charSet)
				{
					charSet.Clear();
				});
				this.MajorCharacterSets.Clear();
				this.ParticipatingCharacterSets.Clear();
			}
			else
			{
				foreach (CharacterSet charSet3 in this.MajorCharacterSets)
				{
					calledCharacters.UnionWith(charSet3.GetCollection());
				}
				foreach (CharacterSet charSet2 in this.ParticipatingCharacterSets)
				{
					calledCharacters.UnionWith(charSet2.GetCollection());
				}
			}
		}

		// Token: 0x0600194D RID: 6477 RVA: 0x0016CA74 File Offset: 0x0016AC74
		public override MonthlyActionBase CreateCopy()
		{
			return Serializer.CreateCopy<MartialArtTournamentMonthlyAction>(this);
		}

		// Token: 0x0600194E RID: 6478 RVA: 0x0016CA8C File Offset: 0x0016AC8C
		public void ClearCalledCharacters()
		{
			CallCharacterHelper.ClearCalledCharacters(this.MajorCharacterSets, !this.ConfigData.MajorTargetMoveVisible, true);
			CallCharacterHelper.ClearCalledCharacters(this.ParticipatingCharacterSets, false, true);
		}

		// Token: 0x0600194F RID: 6479 RVA: 0x0016CAB8 File Offset: 0x0016ACB8
		public override void FillEventArgBox(EventArgBox eventArgBox)
		{
			AdaptableLog.Info("Adding major characters to adventure.");
			for (int majorCharSetIndex = 0; majorCharSetIndex < this.ConfigData.MajorTargetFilterList.Length; majorCharSetIndex++)
			{
				CharacterFilterRequirement filterReq = this.ConfigData.MajorTargetFilterList[majorCharSetIndex];
				CharacterSet majorCharSet = (majorCharSetIndex < this.MajorCharacterSets.Count) ? this.MajorCharacterSets[majorCharSetIndex] : default(CharacterSet);
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(15, 1);
				defaultInterpolatedStringHandler.AppendLiteral("MajorCharacter_");
				defaultInterpolatedStringHandler.AppendFormatted<int>(majorCharSetIndex);
				this.FillIntelligentCharactersToArgBox(eventArgBox, defaultInterpolatedStringHandler.ToStringAndClear(), majorCharSet, filterReq, this.ConfigData.AllowTemporaryMajorCharacter);
				AdventureCharacterSortUtils.Sort(eventArgBox, true, majorCharSetIndex, CharacterSortType.CombatPower, false);
			}
			AdaptableLog.Info("Adding participating characters to adventure.");
			for (int participateCharSetIndex = 0; participateCharSetIndex < this.ConfigData.ParticipateTargetFilterList.Length; participateCharSetIndex++)
			{
				CharacterFilterRequirement filterReq2 = this.ConfigData.ParticipateTargetFilterList[participateCharSetIndex];
				CharacterSet participateCharSet = (participateCharSetIndex < this.ParticipatingCharacterSets.Count) ? this.ParticipatingCharacterSets[participateCharSetIndex] : default(CharacterSet);
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(21, 1);
				defaultInterpolatedStringHandler.AppendLiteral("ParticipateCharacter_");
				defaultInterpolatedStringHandler.AppendFormatted<int>(participateCharSetIndex);
				this.FillIntelligentCharactersToArgBox(eventArgBox, defaultInterpolatedStringHandler.ToStringAndClear(), participateCharSet, filterReq2, true);
				AdventureCharacterSortUtils.Sort(eventArgBox, false, participateCharSetIndex, CharacterSortType.CombatPower, false);
			}
		}

		// Token: 0x06001950 RID: 6480 RVA: 0x0016CC24 File Offset: 0x0016AE24
		private unsafe void FillIntelligentCharactersToArgBox(EventArgBox eventArgBox, string keyPrefix, CharacterSet charSet, CharacterFilterRequirement filterReq, bool allowTempChars)
		{
			int amountAdded = 0;
			DataContext context = DomainManager.TaiwuEvent.MainThreadDataContext;
			int count = charSet.GetCount();
			Span<int> span = new Span<int>(stackalloc byte[checked(unchecked((UIntPtr)count) * 4)], count);
			SpanList<int> enteredCharIds = span;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			foreach (int charId in charSet.GetCollection())
			{
				GameData.Domains.Character.Character character;
				bool flag = !DomainManager.Character.TryGetElement_Objects(charId, out character);
				if (!flag)
				{
					bool flag2 = character.IsCrossAreaTraveling();
					if (flag2)
					{
						DomainManager.Character.GroupMove(context, character, this.Location);
					}
					bool flag3 = !character.GetLocation().Equals(this.Location);
					if (!flag3)
					{
						bool flag4 = filterReq.HasMaximum() && amountAdded >= filterReq.MaxCharactersRequired;
						if (flag4)
						{
							Events.RaiseCharacterLocationChanged(context, charId, character.GetLocation(), this.Location);
							character.SetLocation(this.Location, context);
						}
						else
						{
							defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(1, 2);
							defaultInterpolatedStringHandler.AppendFormatted(keyPrefix);
							defaultInterpolatedStringHandler.AppendLiteral("_");
							defaultInterpolatedStringHandler.AppendFormatted<int>(amountAdded);
							eventArgBox.Set(defaultInterpolatedStringHandler.ToStringAndClear(), charId);
							amountAdded++;
							enteredCharIds.Add(charId);
						}
					}
				}
			}
			defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(37, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Adding ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(amountAdded);
			defaultInterpolatedStringHandler.AppendLiteral(" real characters to adventure.");
			AdaptableLog.Info(defaultInterpolatedStringHandler.ToStringAndClear());
			bool flag5 = allowTempChars && filterReq.CharacterFilterRuleIds[0] == 16;
			if (flag5)
			{
				defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(31, 1);
				defaultInterpolatedStringHandler.AppendLiteral("creating ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(Math.Max(0, filterReq.MinCharactersRequired - amountAdded));
				defaultInterpolatedStringHandler.AppendLiteral(" temporary characters.");
				AdaptableLog.Info(defaultInterpolatedStringHandler.ToStringAndClear());
				foreach (OrganizationItem orgTemplate in ((IEnumerable<OrganizationItem>)Organization.Instance))
				{
					bool flag6 = !orgTemplate.IsSect;
					if (!flag6)
					{
						Settlement settlement = DomainManager.Organization.GetSettlementByOrgTemplateId(orgTemplate.TemplateId);
						GameData.Domains.Character.Character leader = settlement.GetLeader();
						bool flag7 = leader == null || !enteredCharIds.Contains(leader.GetId());
						if (flag7)
						{
							sbyte gender = orgTemplate.GenderRestriction;
							bool flag8 = gender < 0;
							if (flag8)
							{
								gender = Gender.GetRandom(context.Random);
							}
							sbyte stateTemplateId = DomainManager.Map.GetStateTemplateIdByAreaId(settlement.GetLocation().AreaId);
							short charTemplateId = OrganizationDomain.GetCharacterTemplateId(orgTemplate.TemplateId, stateTemplateId, gender);
							TemporaryIntelligentCharacterCreationInfo tempCreationInfo = new TemporaryIntelligentCharacterCreationInfo
							{
								Location = this.Location,
								CharTemplateId = charTemplateId,
								OrgInfo = new OrganizationInfo(orgTemplate.TemplateId, 8, true, settlement.GetId())
							};
							GameData.Domains.Character.Character character2 = DomainManager.Character.CreateTemporaryIntelligentCharacter(context, ref tempCreationInfo);
							OrganizationMemberItem orgMemberCfg = OrganizationDomain.GetOrgMemberConfig(tempCreationInfo.OrgInfo);
							ValueTuple<string, string> realName = CharacterDomain.GetRealName(character2);
							string surname = realName.Item1;
							string givenName = realName.Item2;
							string tag = "MartialArtTournamentMonthlyAction";
							defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(33, 5);
							defaultInterpolatedStringHandler.AppendLiteral("Creating temporary character ");
							defaultInterpolatedStringHandler.AppendFormatted(orgTemplate.Name);
							defaultInterpolatedStringHandler.AppendLiteral("-");
							defaultInterpolatedStringHandler.AppendFormatted(orgMemberCfg.GradeName);
							defaultInterpolatedStringHandler.AppendLiteral(" ");
							defaultInterpolatedStringHandler.AppendFormatted(surname);
							defaultInterpolatedStringHandler.AppendFormatted(givenName);
							defaultInterpolatedStringHandler.AppendLiteral("(");
							defaultInterpolatedStringHandler.AppendFormatted<int>(character2.GetId());
							defaultInterpolatedStringHandler.AppendLiteral(")");
							AdaptableLog.TagInfo(tag, defaultInterpolatedStringHandler.ToStringAndClear());
							int charId2 = character2.GetId();
							DomainManager.Adventure.AddTemporaryIntelligentCharacter(charId2);
							defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(1, 2);
							defaultInterpolatedStringHandler.AppendFormatted(keyPrefix);
							defaultInterpolatedStringHandler.AppendLiteral("_");
							defaultInterpolatedStringHandler.AppendFormatted<int>(amountAdded);
							eventArgBox.Set(defaultInterpolatedStringHandler.ToStringAndClear(), charId2);
							amountAdded++;
						}
					}
				}
			}
			eventArgBox.Set(keyPrefix + "_Count", amountAdded);
		}

		// Token: 0x06001951 RID: 6481 RVA: 0x0016D0A8 File Offset: 0x0016B2A8
		public override void EnsurePrerequisites()
		{
			sbyte curStateTemplateId = DomainManager.Map.GetStateTemplateIdByAreaId(this.Location.AreaId);
			int removedMajorCharCount = CallCharacterHelper.RemoveInvalidCharacters(this.ConfigData.MajorTargetFilterList, this.MajorCharacterSets, this.Location, curStateTemplateId);
			bool flag = removedMajorCharCount > 0;
			if (flag)
			{
				string tag = "ConfigMonthlyAction";
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(43, 3);
				defaultInterpolatedStringHandler.AppendFormatted(this.ConfigData.Name);
				defaultInterpolatedStringHandler.AppendFormatted<MonthlyActionKey>(this.Key);
				defaultInterpolatedStringHandler.AppendLiteral(" removed ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(removedMajorCharCount);
				defaultInterpolatedStringHandler.AppendLiteral(" major characters that are invalid");
				AdaptableLog.TagWarning(tag, defaultInterpolatedStringHandler.ToStringAndClear(), false);
			}
			int removedParticipateCharCount = CallCharacterHelper.RemoveInvalidCharacters(this.ConfigData.ParticipateTargetFilterList, this.ParticipatingCharacterSets, this.Location, curStateTemplateId);
			bool flag2 = removedParticipateCharCount > 0;
			if (flag2)
			{
				string tag2 = "ConfigMonthlyAction";
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(49, 3);
				defaultInterpolatedStringHandler.AppendFormatted(this.ConfigData.Name);
				defaultInterpolatedStringHandler.AppendFormatted<MonthlyActionKey>(this.Key);
				defaultInterpolatedStringHandler.AppendLiteral(" removed ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(removedParticipateCharCount);
				defaultInterpolatedStringHandler.AppendLiteral(" participate characters that are invalid");
				AdaptableLog.TagWarning(tag2, defaultInterpolatedStringHandler.ToStringAndClear(), false);
			}
			bool flag3 = !this.ConfigData.AllowTemporaryMajorCharacter && !CallCharacterHelper.IsAllCharactersAtLocation(this.Location, this.ConfigData.MajorTargetFilterList, this.MajorCharacterSets);
			if (flag3)
			{
				this.CallMajorCharacters();
			}
			bool flag4 = !this.ConfigData.AllowTemporaryParticipateCharacter && !CallCharacterHelper.IsAllCharactersAtLocation(this.Location, this.ConfigData.ParticipateTargetFilterList, this.ParticipatingCharacterSets);
			if (flag4)
			{
				this.CallParticipateCharacters();
			}
			this.State = 5;
		}

		// Token: 0x06001952 RID: 6482 RVA: 0x0016D25C File Offset: 0x0016B45C
		private void CallMajorCharacters()
		{
			bool flag = !this.Location.IsValid();
			if (!flag)
			{
				bool flag2 = !CallCharacterHelper.CallCharacters(this.Location, this.ConfigData.CharacterSearchRange, this.ConfigData.MajorTargetFilterList, this.MajorCharacterSets, this.ConfigData.AllowTemporaryMajorCharacter, true, !this.ConfigData.MajorTargetMoveVisible);
				if (!flag2)
				{
					this.State = 2;
				}
			}
		}

		// Token: 0x06001953 RID: 6483 RVA: 0x0016D2D0 File Offset: 0x0016B4D0
		private void CallParticipateCharacters()
		{
			bool flag = !this.Location.IsValid();
			if (!flag)
			{
				this.ConfigData.ParticipateTargetFilterList[0].MinCharactersRequired = 0;
				bool flag2 = !CallCharacterHelper.CallCharacters(this.Location, this.ConfigData.CharacterSearchRange, this.ConfigData.ParticipateTargetFilterList, this.ParticipatingCharacterSets, this.ConfigData.AllowTemporaryParticipateCharacter, true, false);
				if (!flag2)
				{
					this.State = 3;
				}
			}
		}

		// Token: 0x06001954 RID: 6484 RVA: 0x0016D34C File Offset: 0x0016B54C
		public override bool IsSerializedSizeFixed()
		{
			return false;
		}

		// Token: 0x06001955 RID: 6485 RVA: 0x0016D360 File Offset: 0x0016B560
		public override int GetSerializedSize()
		{
			int totalSize = 18;
			bool flag = this.MajorCharacterSets != null;
			if (flag)
			{
				totalSize += 2;
				int elementsCount = this.MajorCharacterSets.Count;
				for (int i = 0; i < elementsCount; i++)
				{
					totalSize += this.MajorCharacterSets[i].GetSerializedSize();
				}
			}
			else
			{
				totalSize += 2;
			}
			bool flag2 = this.ParticipatingCharacterSets != null;
			if (flag2)
			{
				totalSize += 2;
				int elementsCount2 = this.ParticipatingCharacterSets.Count;
				for (int j = 0; j < elementsCount2; j++)
				{
					totalSize += this.ParticipatingCharacterSets[j].GetSerializedSize();
				}
			}
			else
			{
				totalSize += 2;
			}
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x06001956 RID: 6486 RVA: 0x0016D434 File Offset: 0x0016B634
		public unsafe override int Serialize(byte* pData)
		{
			*(short*)pData = this.CurrentHost;
			byte* pCurrData = pData + 2;
			bool flag = this.MajorCharacterSets != null;
			if (flag)
			{
				int elementsCount = this.MajorCharacterSets.Count;
				Tester.Assert(elementsCount <= 65535, "");
				*(short*)pCurrData = (short)((ushort)elementsCount);
				pCurrData += 2;
				for (int i = 0; i < elementsCount; i++)
				{
					int subDataSize = this.MajorCharacterSets[i].Serialize(pCurrData);
					pCurrData += subDataSize;
					Tester.Assert(subDataSize <= 65535, "");
				}
			}
			else
			{
				*(short*)pCurrData = 0;
				pCurrData += 2;
			}
			bool flag2 = this.ParticipatingCharacterSets != null;
			if (flag2)
			{
				int elementsCount2 = this.ParticipatingCharacterSets.Count;
				Tester.Assert(elementsCount2 <= 65535, "");
				*(short*)pCurrData = (short)((ushort)elementsCount2);
				pCurrData += 2;
				for (int j = 0; j < elementsCount2; j++)
				{
					int subDataSize2 = this.ParticipatingCharacterSets[j].Serialize(pCurrData);
					pCurrData += subDataSize2;
					Tester.Assert(subDataSize2 <= 65535, "");
				}
			}
			else
			{
				*(short*)pCurrData = 0;
				pCurrData += 2;
			}
			pCurrData += this.Location.Serialize(pCurrData);
			pCurrData += this.Key.Serialize(pCurrData);
			*pCurrData = (byte)this.State;
			pCurrData++;
			*(int*)pCurrData = this.Month;
			pCurrData += 4;
			*(int*)pCurrData = this.LastFinishDate;
			pCurrData += 4;
			int totalSize = (int)((long)(pCurrData - pData));
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x06001957 RID: 6487 RVA: 0x0016D5DC File Offset: 0x0016B7DC
		public unsafe override int Deserialize(byte* pData)
		{
			this.CurrentHost = *(short*)pData;
			byte* pCurrData = pData + 2;
			ushort elementsCount = *(ushort*)pCurrData;
			pCurrData += 2;
			bool flag = elementsCount > 0;
			if (flag)
			{
				bool flag2 = this.MajorCharacterSets == null;
				if (flag2)
				{
					this.MajorCharacterSets = new List<CharacterSet>((int)elementsCount);
				}
				else
				{
					this.MajorCharacterSets.Clear();
				}
				for (int i = 0; i < (int)elementsCount; i++)
				{
					CharacterSet element = default(CharacterSet);
					pCurrData += element.Deserialize(pCurrData);
					this.MajorCharacterSets.Add(element);
				}
			}
			else
			{
				List<CharacterSet> majorCharacterSets = this.MajorCharacterSets;
				if (majorCharacterSets != null)
				{
					majorCharacterSets.Clear();
				}
			}
			ushort elementsCount2 = *(ushort*)pCurrData;
			pCurrData += 2;
			bool flag3 = elementsCount2 > 0;
			if (flag3)
			{
				bool flag4 = this.ParticipatingCharacterSets == null;
				if (flag4)
				{
					this.ParticipatingCharacterSets = new List<CharacterSet>((int)elementsCount2);
				}
				else
				{
					this.ParticipatingCharacterSets.Clear();
				}
				for (int j = 0; j < (int)elementsCount2; j++)
				{
					CharacterSet element2 = default(CharacterSet);
					pCurrData += element2.Deserialize(pCurrData);
					this.ParticipatingCharacterSets.Add(element2);
				}
			}
			else
			{
				List<CharacterSet> participatingCharacterSets = this.ParticipatingCharacterSets;
				if (participatingCharacterSets != null)
				{
					participatingCharacterSets.Clear();
				}
			}
			pCurrData += this.Location.Deserialize(pCurrData);
			pCurrData += this.Key.Deserialize(pCurrData);
			this.State = *(sbyte*)pCurrData;
			pCurrData++;
			this.Month = *(int*)pCurrData;
			pCurrData += 4;
			this.LastFinishDate = *(int*)pCurrData;
			pCurrData += 4;
			int totalSize = (int)((long)(pCurrData - pData));
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x040005A7 RID: 1447
		[SerializableGameDataField]
		public short CurrentHost;

		// Token: 0x040005A8 RID: 1448
		[SerializableGameDataField]
		public List<CharacterSet> MajorCharacterSets;

		// Token: 0x040005A9 RID: 1449
		[SerializableGameDataField]
		public List<CharacterSet> ParticipatingCharacterSets;

		// Token: 0x040005AA RID: 1450
		[SerializableGameDataField]
		public Location Location;

		// Token: 0x040005AB RID: 1451
		private static readonly sbyte[] WinnerLearnCombatSkillCounts = new sbyte[]
		{
			1,
			1,
			2,
			3,
			3,
			4,
			5,
			5,
			6
		};

		// Token: 0x040005AC RID: 1452
		private static readonly sbyte[] WinnerLearnLifeSkillCounts = new sbyte[]
		{
			1,
			1,
			1,
			2,
			2,
			2,
			3,
			3,
			3
		};

		// Token: 0x040005AD RID: 1453
		private bool _tmpSkipMonth;
	}
}
