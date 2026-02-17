using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using Config;
using GameData.ArchiveData;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.Character.Ai;
using GameData.Domains.Character.Ai.GeneralAction.TeachRandom;
using GameData.Domains.Character.Ai.PrioritizedAction;
using GameData.Domains.Character.Filters;
using GameData.Domains.Character.Relation;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.Information;
using GameData.Domains.Information.Collection;
using GameData.Domains.Item;
using GameData.Domains.LifeRecord;
using GameData.Domains.Map;
using GameData.Domains.Organization.Display;
using GameData.Domains.Organization.SettlementPrisonRecord;
using GameData.Domains.SpecialEffect;
using GameData.Domains.SpecialEffect.CombatSkill.Xuehoujiao.BreakBodyEffect;
using GameData.Domains.World.MonthlyEvent;
using GameData.Serializer;
using GameData.Utilities;
using Redzen.Random;

namespace GameData.Domains.Organization
{
	// Token: 0x02000645 RID: 1605
	[SerializableGameData(NotForDisplayModule = true)]
	public class Sect : Settlement, ISerializableGameData
	{
		// Token: 0x17000320 RID: 800
		// (get) Token: 0x0600478F RID: 18319 RVA: 0x0028463F File Offset: 0x0028283F
		public sbyte TaskStatus
		{
			get
			{
				return DomainManager.World.GetSectMainStoryTaskStatus(this.OrgTemplateId);
			}
		}

		// Token: 0x17000321 RID: 801
		// (get) Token: 0x06004790 RID: 18320 RVA: 0x00284651 File Offset: 0x00282851
		private bool IsPreviousMartialArtTournamentWinner
		{
			get
			{
				return (short)this.OrgTemplateId == DomainManager.Extra.GetLastMartialArtTournamentWinner();
			}
		}

		// Token: 0x06004791 RID: 18321 RVA: 0x00284668 File Offset: 0x00282868
		public Sect(short id, Location location, sbyte orgTemplateId, IRandomSource random) : base(id, location, orgTemplateId, random)
		{
			OrganizationItem orgConfig = Organization.Instance[orgTemplateId];
			bool flag = orgConfig.SeniorityGroupId >= 0;
			if (flag)
			{
				ValueTuple<short, short> seniorityRange = OrganizationDomain.GetSeniorityRange(orgConfig.SeniorityGroupId);
				short seniorityFirst = seniorityRange.Item1;
				short seniorityLast = seniorityRange.Item2;
				this._minSeniorityId = (short)random.Next((int)seniorityFirst, (int)(seniorityLast + 1));
				ValueTuple<short, short> monasticTitleSuffixRange = OrganizationDomain.GetMonasticTitleSuffixRange(orgConfig.SeniorityGroupId);
				short suffixFirst = monasticTitleSuffixRange.Item1;
				short suffixLast = monasticTitleSuffixRange.Item2;
				int suffixesCount = (int)(suffixLast - suffixFirst + 1);
				this._availableMonasticTitleSuffixIds = new List<short>(suffixesCount);
				for (short i = suffixFirst; i <= suffixLast; i += 1)
				{
					this._availableMonasticTitleSuffixIds.Add(i);
				}
			}
			else
			{
				this._minSeniorityId = -1;
				this._availableMonasticTitleSuffixIds = new List<short>();
			}
			sbyte largeSectIndex = OrganizationDomain.GetLargeSectIndex(orgTemplateId);
			bool flag2 = largeSectIndex >= 0;
			if (flag2)
			{
				DomainManager.Organization.OfflineInitializeLargeSectFavorabilities(largeSectIndex, orgConfig.LargeSectFavorabilities);
			}
			this._taiwuExploreStatus = 0;
			this._taiwuInvestmentForMartialArtTournament = new int[3];
		}

		// Token: 0x06004792 RID: 18322 RVA: 0x0028479C File Offset: 0x0028299C
		public int[] GetMartialArtTournamentPreparations()
		{
			return this._martialArtTournamentPreparations;
		}

		// Token: 0x06004793 RID: 18323 RVA: 0x002847B4 File Offset: 0x002829B4
		public static bool CanBeGuard(int charId)
		{
			GameData.Domains.Character.Character character;
			return DomainManager.Character.TryGetElement_Objects(charId, out character) && character.IsInteractableAsIntelligentCharacter() && character.GetAgeGroup() == 2 && !DomainManager.LegendaryBook.IsCharacterLegendaryBookOwnerOrContest(charId);
		}

		// Token: 0x06004794 RID: 18324 RVA: 0x002847F4 File Offset: 0x002829F4
		public unsafe void UpdateMartialArtTournamentPreparations()
		{
			this._membersSortedByAuthority.Clear();
			for (sbyte grade = 0; grade < 9; grade += 1)
			{
				HashSet<int> members = this.Members.GetMembers(grade);
				foreach (int memberCharId in members)
				{
					GameData.Domains.Character.Character character = DomainManager.Character.GetElement_Objects(memberCharId);
					ResourceInts resources = *character.GetResources();
					int authority = *(ref resources.Items.FixedElementField + (IntPtr)7 * 4);
					this._membersSortedByAuthority.Add(((long)authority << 32) + (long)memberCharId, memberCharId);
				}
			}
			int topTenCombatPower = 0;
			int rankingIndex = 0;
			while (rankingIndex < 10 && rankingIndex < this._membersSortedByCombatPower.Count)
			{
				int index = this._membersSortedByCombatPower.Count - rankingIndex - 1;
				int combatPower = (int)(this._membersSortedByCombatPower.Keys[index] >> 32);
				topTenCombatPower += combatPower;
				rankingIndex++;
			}
			this._martialArtTournamentPreparations[0] = topTenCombatPower / 10000;
			int topTenAuthority = 0;
			int rankingIndex2 = 0;
			while (rankingIndex2 < 10 && rankingIndex2 < this._membersSortedByAuthority.Count)
			{
				int index2 = this._membersSortedByAuthority.Count - rankingIndex2 - 1;
				int authority2 = (int)(this._membersSortedByAuthority.Keys[index2] >> 32);
				topTenAuthority += authority2;
				rankingIndex2++;
			}
			this._martialArtTournamentPreparations[1] = topTenAuthority * (int)GlobalConfig.ResourcesWorth[7] / 10000;
			SettlementLayeredTreasuries treasuries = base.Treasuries;
			this._martialArtTournamentPreparations[2] = 0;
			foreach (SettlementTreasury treasury in treasuries.SettlementTreasuries)
			{
				this._martialArtTournamentPreparations[2] += (treasury.Inventory.GetTotalValue() + treasury.Resources.GetTotalWorth()) / GlobalConfig.Instance.MartialArtTournamentPreparationValueDivider;
			}
		}

		// Token: 0x06004795 RID: 18325 RVA: 0x00284A08 File Offset: 0x00282C08
		public void UpdateApprovalOfTaiwu(DataContext context)
		{
			short approvingRate = base.CalcApprovingRate();
			bool flag = approvingRate < 900;
			if (!flag)
			{
				List<int> potentialApprovingCharIds = ObjectPool<List<int>>.Instance.Get();
				potentialApprovingCharIds.Clear();
				for (sbyte grade = 0; grade < 9; grade += 1)
				{
					IEnumerable<int> gradeMembers = DomainManager.Character.ExcludeInfant(this.Members.GetMembers(grade));
					foreach (int memberCharId in gradeMembers)
					{
						SettlementCharacter settlementCharacter = DomainManager.Organization.GetSettlementCharacter(memberCharId);
						bool flag2 = !settlementCharacter.GetApprovedTaiwu();
						if (flag2)
						{
							potentialApprovingCharIds.Add(memberCharId);
						}
					}
				}
				bool flag3 = potentialApprovingCharIds.Count <= 0;
				if (flag3)
				{
					ObjectPool<List<int>>.Instance.Return(potentialApprovingCharIds);
				}
				else
				{
					int newApprovingCharId = potentialApprovingCharIds.GetRandom(context.Random);
					SectCharacter sectChar = DomainManager.Organization.GetElement_SectCharacters(newApprovingCharId);
					sectChar.SetApprovedTaiwu(context, true);
					DomainManager.Character.TryCreateRelation(context, newApprovingCharId, DomainManager.Taiwu.GetTaiwuCharId());
					ObjectPool<List<int>>.Instance.Return(potentialApprovingCharIds);
				}
			}
		}

		// Token: 0x06004796 RID: 18326 RVA: 0x00284B44 File Offset: 0x00282D44
		protected override void RecruitOrCreateLackingMembers(DataContext context)
		{
			List<ValueTuple<int, short>> weightTable = new List<ValueTuple<int, short>>();
			List<short> blockIds = ObjectPool<List<short>>.Instance.Get();
			blockIds.Clear();
			DomainManager.Map.GetSettlementBlocks(this.Location.AreaId, this.Location.BlockId, blockIds);
			List<short> nearbyBlockIds = ObjectPool<List<short>>.Instance.Get();
			nearbyBlockIds.Clear();
			DomainManager.Map.GetSettlementBlocksAndAffiliatedBlocks(this.Location.AreaId, this.Location.BlockId, nearbyBlockIds);
			LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
			int currDate = DomainManager.World.GetCurrDate();
			sbyte mapStateTemplateId = DomainManager.Map.GetStateTemplateIdByAreaId(this.Location.AreaId);
			OrganizationItem organizationCfg = Organization.Instance[this.OrgTemplateId];
			int worldPopulationFactor = DomainManager.World.GetWorldPopulationFactor();
			for (sbyte grade = 8; grade >= 0; grade -= 1)
			{
				OrganizationMemberItem orgMemberCfg = OrganizationMember.Instance[organizationCfg.Members[(int)grade]];
				OrganizationInfo orgInfo = new OrganizationInfo(this.OrgTemplateId, grade, true, this.Id);
				int principalAmount = base.GetPrincipalAmount(grade);
				int expectedAmount = base.GetExpectedCoreMemberAmount(orgMemberCfg);
				bool flag = !orgMemberCfg.RestrictPrincipalAmount;
				if (flag)
				{
					expectedAmount = expectedAmount * worldPopulationFactor / 100;
				}
				int recruitCount = expectedAmount - principalAmount;
				bool flag2 = recruitCount <= 0;
				if (!flag2)
				{
					this.GetRecruitableCharacters(weightTable, grade);
					for (int i = 0; i < recruitCount; i++)
					{
						bool flag3 = weightTable.Count > 0;
						if (flag3)
						{
							int index = RandomUtils.GetRandomIndex<int>(weightTable, context.Random);
							int charId = weightTable[index].Item1;
							GameData.Domains.Character.Character character = DomainManager.Character.GetElement_Objects(charId);
							DomainManager.Organization.JoinSect(context, character, orgInfo);
							CollectionUtils.SwapAndRemove<ValueTuple<int, short>>(weightTable, index);
						}
						else
						{
							SettlementMembersCreationInfo info = new SettlementMembersCreationInfo(this.OrgTemplateId, this.Id, mapStateTemplateId, this.Location.AreaId, blockIds, nearbyBlockIds);
							info.CoreMemberConfig = orgMemberCfg;
							OrganizationDomain.CreateCoreCharacter(context, info);
							info.CompleteCreatingCharacters();
						}
					}
					bool flag4 = recruitCount > 0;
					if (flag4)
					{
						string tag = "RecruitOrCreateLackingMembers";
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(30, 3);
						defaultInterpolatedStringHandler.AppendLiteral("Recruited Count for ");
						defaultInterpolatedStringHandler.AppendFormatted(organizationCfg.Name);
						defaultInterpolatedStringHandler.AppendLiteral(", grade ");
						defaultInterpolatedStringHandler.AppendFormatted<sbyte>(grade);
						defaultInterpolatedStringHandler.AppendLiteral(": ");
						defaultInterpolatedStringHandler.AppendFormatted<int>(recruitCount);
						AdaptableLog.TagInfo(tag, defaultInterpolatedStringHandler.ToStringAndClear());
					}
				}
			}
			ObjectPool<List<short>>.Instance.Return(blockIds);
			ObjectPool<List<short>>.Instance.Return(nearbyBlockIds);
		}

		// Token: 0x06004797 RID: 18327 RVA: 0x00284DEC File Offset: 0x00282FEC
		private void GetRecruitableCharacters(List<ValueTuple<int, short>> weightTable, sbyte grade = 0)
		{
			weightTable.Clear();
			MapAreaData areaData = DomainManager.Map.GetElement_Areas((int)this.Location.AreaId);
			foreach (SettlementInfo settlementInfo in areaData.SettlementInfos)
			{
				bool flag = settlementInfo.SettlementId < 0 || settlementInfo.SettlementId == this.Id;
				if (!flag)
				{
					Settlement settlement = DomainManager.Organization.GetSettlement(settlementInfo.SettlementId);
					OrganizationItem organizationCfg = Organization.Instance[this.OrgTemplateId];
					OrgMemberCollection members = settlement.GetMembers();
					int weight = (int)(100 + AiHelper.PrioritizedActionConstants.CivilianGradeJoinSectChance[(int)grade]);
					bool flag2 = weight <= 0;
					if (!flag2)
					{
						HashSet<int> gradeMembers = members.GetMembers(grade);
						OrganizationMemberItem orgMemberCfg = OrganizationMember.Instance[organizationCfg.Members[(int)grade]];
						bool flag3 = orgMemberCfg.Gender == -1;
						if (flag3)
						{
							foreach (int member in gradeMembers)
							{
								weightTable.Add(new ValueTuple<int, short>(member, (short)weight));
							}
						}
						else
						{
							foreach (int member2 in gradeMembers)
							{
								GameData.Domains.Character.Character character = DomainManager.Character.GetElement_Objects(member2);
								bool flag4 = character.GetGender() != orgMemberCfg.Gender;
								if (!flag4)
								{
									bool flag5 = orgMemberCfg.ChildGrade < 0;
									if (flag5)
									{
										RelatedCharacters relatedChars = DomainManager.Character.GetRelatedCharacters(member2);
										bool flag6 = relatedChars.HusbandsAndWives.GetCount() > 0;
										if (flag6)
										{
											continue;
										}
										bool flag7 = relatedChars.AdoptiveChildren.GetCount() > 0;
										if (flag7)
										{
											continue;
										}
										bool flag8 = relatedChars.BloodChildren.GetCount() > 0;
										if (flag8)
										{
											continue;
										}
										bool flag9 = relatedChars.StepChildren.GetCount() > 0;
										if (flag9)
										{
											continue;
										}
									}
									weightTable.Add(new ValueTuple<int, short>(member2, (short)weight));
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x06004798 RID: 18328 RVA: 0x00285030 File Offset: 0x00283230
		protected unsafe override void OfflineUpdateTreasuryGuards(DataContext context, SettlementLayeredTreasuries treasuries)
		{
			int count = GlobalConfig.Instance.TreasuryGuardCount;
			int num = count;
			Span<ValueTuple<int, int>> span = new Span<ValueTuple<int, int>>(stackalloc byte[checked(unchecked((UIntPtr)num) * (UIntPtr)sizeof(ValueTuple<int, int>))], num);
			SpanList<ValueTuple<int, int>> topK = span;
			HashSet<int> picked = ObjectPool<HashSet<int>>.Instance.Get();
			picked.Clear();
			foreach (SettlementTreasury treasury in treasuries.SettlementTreasuries)
			{
				foreach (int prevGuardId in treasury.GuardIds.GetCollection())
				{
					GameData.Domains.Character.Character character;
					bool flag = !DomainManager.Character.TryGetElement_Objects(prevGuardId, out character);
					if (!flag)
					{
						character.RemoveFeatureGroup(context, 536);
						character.AddFeature(context, 553, false);
					}
				}
			}
			foreach (SettlementTreasury treasury2 in treasuries.SettlementTreasuries)
			{
				sbyte currGrade = GlobalConfig.Instance.SectTreasuryGuardMaxGrade[(int)treasury2.LayerIndex];
				topK.Clear();
				HashSet<int> gradeMembers = this.Members.GetMembers(currGrade);
				foreach (int charId in gradeMembers)
				{
					bool flag2 = picked.Contains(charId);
					if (!flag2)
					{
						bool flag3 = !Sect.CanBeGuard(charId);
						if (!flag3)
						{
							GameData.Domains.Character.Character character2 = DomainManager.Character.GetElement_Objects(charId);
							int combatPower = character2.GetCombatPower();
							bool flag4 = character2.GetFeatureIds().Contains(553);
							if (flag4)
							{
								combatPower = combatPower * 2 / 3;
							}
							ref topK.TryInsertTopK(count, charId, combatPower);
						}
					}
				}
				treasury2.GuardIds.Clear();
				for (int i = 0; i < topK.Count; i++)
				{
					int charId2 = topK[i].Item1;
					treasury2.GuardIds.Add(charId2);
					GameData.Domains.Character.Character character3 = DomainManager.Character.GetElement_Objects(charId2);
					character3.RemoveFeature(context, 553);
					short guardFeatureId = this.GetTreasuryGuardFeatureId(treasury2.LayerIndex);
					character3.AddFeature(context, guardFeatureId, false);
					BasePrioritizedAction previousAction;
					bool flag5 = !DomainManager.Character.TryGetCharacterPrioritizedAction(charId2, out previousAction) || previousAction.ActionType != 15;
					if (flag5)
					{
						DomainManager.Character.StartCharacterPrioritizedAction(context, character3, new GuardTreasuryAction
						{
							Target = new NpcTravelTarget(this.Location, PrioritizedActionTypeHelper.GetMaxDurationByPrioritizedActionTemplateId(15))
						});
					}
					picked.Add(charId2);
				}
			}
			ObjectPool<HashSet<int>>.Instance.Return(picked);
		}

		// Token: 0x06004799 RID: 18329 RVA: 0x00285310 File Offset: 0x00283510
		public override int GetSupplyLevel()
		{
			bool flag = DomainManager.Organization.IsCreatingSettlements();
			int result;
			if (flag)
			{
				result = 2;
			}
			else
			{
				sbyte taskStatus = this.TaskStatus;
				bool isPrevWinner = this.IsPreviousMartialArtTournamentWinner;
				if (!true)
				{
				}
				int num;
				switch (taskStatus)
				{
				case 0:
					num = (isPrevWinner ? 2 : 1);
					break;
				case 1:
					num = (isPrevWinner ? 3 : 2);
					break;
				case 2:
					num = (isPrevWinner ? 1 : 0);
					break;
				default:
					num = 1;
					break;
				}
				if (!true)
				{
				}
				result = num + base.Treasuries.SupplyLevelAddOn;
			}
			return result;
		}

		// Token: 0x0600479A RID: 18330 RVA: 0x00285394 File Offset: 0x00283594
		public override SettlementNameRelatedData GetNameRelatedData()
		{
			MapBlockData block = DomainManager.Map.GetBlock(this.Location).GetRootBlock();
			return new SettlementNameRelatedData(-1, block.TemplateId);
		}

		// Token: 0x17000322 RID: 802
		// (get) Token: 0x0600479B RID: 18331 RVA: 0x002853C8 File Offset: 0x002835C8
		public SettlementPrison Prison
		{
			get
			{
				bool flag = this._prison != null;
				SettlementPrison prison;
				if (flag)
				{
					prison = this._prison;
				}
				else
				{
					bool flag2 = !DomainManager.Extra.TryGetElement_SettlementPrisons(this.Id, out this._prison);
					if (flag2)
					{
						this._prison = new SettlementPrison();
					}
					prison = this._prison;
				}
				return prison;
			}
		}

		// Token: 0x0600479C RID: 18332 RVA: 0x00285420 File Offset: 0x00283620
		public int CalcBountyAmount(sbyte grade)
		{
			sbyte goodness = Organization.Instance[this.OrgTemplateId].Goodness;
			if (!true)
			{
			}
			int result;
			switch (goodness)
			{
			case -1:
				result = (int)((grade + 1) * (grade + 1)) * 1200;
				break;
			case 0:
				result = (int)((grade + 1) * (grade + 1)) * 600;
				break;
			case 1:
				result = (int)((grade + 1) * (grade + 1) * 120);
				break;
			default:
				result = 0;
				break;
			}
			if (!true)
			{
			}
			return result;
		}

		// Token: 0x0600479D RID: 18333 RVA: 0x00285498 File Offset: 0x00283698
		public void GetXiangshuInfectedBounties(List<SettlementBounty> bounties)
		{
			List<Predicate<GameData.Domains.Character.Character>> predicates = ObjectPool<List<Predicate<GameData.Domains.Character.Character>>>.Instance.Get();
			List<GameData.Domains.Character.Character> charList = ObjectPool<List<GameData.Domains.Character.Character>>.Instance.Get();
			List<short> areaList = ObjectPool<List<short>>.Instance.Get();
			predicates.Clear();
			charList.Clear();
			areaList.Clear();
			sbyte curStateTemplateId = DomainManager.Map.GetStateTemplateIdByAreaId(this.Location.AreaId);
			sbyte curStateId = curStateTemplateId - 1;
			DomainManager.Map.GetAllAreaInState(curStateId, areaList);
			MapCharacterFilter.ParallelFindInfected(predicates, charList, areaList);
			short punishmentType = 39;
			PunishmentTypeItem punishmentTypeCfg = PunishmentType.Instance[punishmentType];
			sbyte punishmentSeverity = base.GetPunishmentTypeSeverity(punishmentTypeCfg, true);
			PunishmentSeverityItem punishmentSeverityCfg = PunishmentSeverity.Instance[punishmentSeverity];
			GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
			bool flag = taiwu.IsActiveExternalRelationState(2) && taiwu.GetLocation().AreaId == this.Location.AreaId;
			if (flag)
			{
				KidnappedCharacterList kidnappedCharacters = DomainManager.Character.GetKidnappedCharacters(taiwu.GetId());
				foreach (KidnappedCharacter kidnappedChar in kidnappedCharacters.GetCollection())
				{
					GameData.Domains.Character.Character character = DomainManager.Character.GetElement_Objects(kidnappedChar.CharId);
					bool flag2 = character.IsCompletelyInfected();
					if (flag2)
					{
						bounties.Add(new SettlementBounty
						{
							CharId = kidnappedChar.CharId,
							PunishmentSeverity = punishmentSeverity,
							PunishmentType = punishmentType,
							ExpireDate = DomainManager.World.GetCurrDate() + punishmentSeverityCfg.BountyDuration,
							BountyAmount = this.CalcBountyAmount(character.GetOrganizationInfo().Grade)
						});
					}
				}
			}
			foreach (GameData.Domains.Character.Character character2 in charList)
			{
				int charId = character2.GetId();
				bool flag3 = DomainManager.Organization.GetPrisonerSect(charId) >= 0;
				if (!flag3)
				{
					bounties.Add(new SettlementBounty
					{
						CharId = charId,
						PunishmentSeverity = punishmentSeverity,
						PunishmentType = punishmentType,
						ExpireDate = DomainManager.World.GetCurrDate() + punishmentSeverityCfg.BountyDuration,
						BountyAmount = this.CalcBountyAmount(character2.GetOrganizationInfo().Grade)
					});
				}
			}
			ObjectPool<List<Predicate<GameData.Domains.Character.Character>>>.Instance.Return(predicates);
			ObjectPool<List<GameData.Domains.Character.Character>>.Instance.Return(charList);
			ObjectPool<List<short>>.Instance.Return(areaList);
		}

		// Token: 0x0600479E RID: 18334 RVA: 0x0028572C File Offset: 0x0028392C
		public bool HasCriminalBounty(int charId)
		{
			return this.Prison.GetBounty(charId) != null;
		}

		// Token: 0x0600479F RID: 18335 RVA: 0x00285750 File Offset: 0x00283950
		public bool HasEnemySectBounty(int charId)
		{
			GameData.Domains.Character.Character character;
			bool flag = !DomainManager.Character.TryGetElement_Objects(charId, out character);
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				bool flag2 = !this.CanHaveBounty(character);
				if (flag2)
				{
					result = false;
				}
				else
				{
					sbyte charOrgTemplateId = character.GetOrganizationInfo().OrgTemplateId;
					bool flag3 = !OrganizationDomain.IsLargeSect((short)charOrgTemplateId);
					result = (!flag3 && DomainManager.Organization.GetSectFavorability(this.OrgTemplateId, charOrgTemplateId) == -1);
				}
			}
			return result;
		}

		// Token: 0x060047A0 RID: 18336 RVA: 0x002857C4 File Offset: 0x002839C4
		public bool HasEnemyRelationBounty(int charId)
		{
			GameData.Domains.Character.Character character;
			bool flag = !DomainManager.Character.TryGetElement_Objects(charId, out character);
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				bool flag2 = !this.CanHaveBounty(character);
				if (flag2)
				{
					result = false;
				}
				else
				{
					bool flag3 = character.GetOrganizationInfo().SettlementId == this.Id;
					if (flag3)
					{
						result = false;
					}
					else
					{
						sbyte stateId = DomainManager.Map.GetStateIdByAreaId(this.Location.AreaId);
						List<short> settlementIds = ObjectPool<List<short>>.Instance.Get();
						DomainManager.Map.GetStateSettlementIds(stateId, settlementIds, true, true);
						for (int i = 0; i < settlementIds.Count; i++)
						{
							short settlementId = settlementIds[i];
							Settlement settlement = DomainManager.Organization.GetSettlement(settlementId);
							OrgMemberCollection members = settlement.GetMembers();
							for (sbyte grade = 3; grade < 9; grade += 1)
							{
								HashSet<int> gradeMembers = members.GetMembers(grade);
								foreach (int gradeMemberId in gradeMembers)
								{
									bool flag4 = DomainManager.Organization.GetPrisonerSect(gradeMemberId) == this.OrgTemplateId;
									if (!flag4)
									{
										RelatedCharacters relatedCharacters = DomainManager.Character.GetRelatedCharacters(gradeMemberId);
										HashSet<int> enemies = relatedCharacters.Enemies.GetCollection();
										bool flag5 = !enemies.Contains(charId);
										if (!flag5)
										{
											ObjectPool<List<short>>.Instance.Return(settlementIds);
											return true;
										}
									}
								}
							}
						}
						ObjectPool<List<short>>.Instance.Return(settlementIds);
						result = false;
					}
				}
			}
			return result;
		}

		// Token: 0x060047A1 RID: 18337 RVA: 0x00285978 File Offset: 0x00283B78
		public unsafe void GetEnemySectBounties(List<SettlementBounty> bounties)
		{
			short punishmentType = 41;
			PunishmentTypeItem punishmentTypeCfg = PunishmentType.Instance[punishmentType];
			sbyte punishmentSeverity = base.GetPunishmentTypeSeverity(punishmentTypeCfg, true);
			int duration = PunishmentSeverity.Instance[punishmentSeverity].BountyDuration;
			Span<sbyte> span = new Span<sbyte>(stackalloc byte[(UIntPtr)15], 15);
			SpanList<sbyte> hostileSects = span;
			DomainManager.Organization.GetSectTemplateIdsByFavorability(this.OrgTemplateId, -1, ref hostileSects);
			foreach (sbyte ptr in hostileSects)
			{
				sbyte hostileSectTemplateId = ptr;
				Sect hostileSect = (Sect)DomainManager.Organization.GetSettlementByOrgTemplateId(hostileSectTemplateId);
				for (sbyte grade = 0; grade < 9; grade += 1)
				{
					HashSet<int> gradeMembers = hostileSect.Members.GetMembers(grade);
					foreach (int enemySectMemberId in gradeMembers)
					{
						GameData.Domains.Character.Character enemy;
						bool flag = !DomainManager.Character.TryGetElement_Objects(enemySectMemberId, out enemy);
						if (!flag)
						{
							bool flag2 = !this.CanHaveBounty(enemy);
							if (!flag2)
							{
								bounties.Add(new SettlementBounty
								{
									CharId = enemySectMemberId,
									PunishmentSeverity = punishmentSeverity,
									PunishmentType = punishmentType,
									ExpireDate = DomainManager.World.GetCurrDate() + duration,
									BountyAmount = this.CalcBountyAmount(grade)
								});
							}
						}
					}
				}
			}
		}

		// Token: 0x060047A2 RID: 18338 RVA: 0x00285AF8 File Offset: 0x00283CF8
		public void GetEnemyRelationBounties(List<SettlementBounty> bounties)
		{
			short punishmentType = 42;
			PunishmentTypeItem punishmentTypeCfg = PunishmentType.Instance[punishmentType];
			sbyte punishmentSeverity = base.GetPunishmentTypeSeverity(punishmentTypeCfg, true);
			int duration = PunishmentSeverity.Instance[punishmentSeverity].BountyDuration;
			for (sbyte grade = 3; grade < 9; grade += 1)
			{
				HashSet<int> gradeMembers = this.Members.GetMembers(grade);
				foreach (int gradeMemberId in gradeMembers)
				{
					bool flag = DomainManager.Organization.GetPrisonerSect(gradeMemberId) == this.OrgTemplateId;
					if (!flag)
					{
						RelatedCharacters relatedCharacters = DomainManager.Character.GetRelatedCharacters(gradeMemberId);
						HashSet<int> enemies = relatedCharacters.Enemies.GetCollection();
						foreach (int enemyId in enemies)
						{
							GameData.Domains.Character.Character enemy;
							bool flag2 = !DomainManager.Character.TryGetElement_Objects(enemyId, out enemy);
							if (!flag2)
							{
								bool flag3 = !this.CanHaveBounty(enemy);
								if (!flag3)
								{
									bool flag4 = enemy.GetOrganizationInfo().SettlementId == this.Id;
									if (!flag4)
									{
										bounties.Add(new SettlementBounty
										{
											CharId = enemyId,
											PunishmentSeverity = punishmentSeverity,
											PunishmentType = punishmentType,
											ExpireDate = DomainManager.World.GetCurrDate() + duration,
											BountyAmount = this.CalcBountyAmount(enemy.GetOrganizationInfo().Grade)
										});
									}
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x060047A3 RID: 18339 RVA: 0x00285CD0 File Offset: 0x00283ED0
		private bool CanHaveBounty(GameData.Domains.Character.Character character)
		{
			return CharacterMatcher.DefValue.CanHaveBounty.Match(character);
		}

		// Token: 0x060047A4 RID: 18340 RVA: 0x00285CE0 File Offset: 0x00283EE0
		public void AddBounty(DataContext context, GameData.Domains.Character.Character character, sbyte punishmentSeverity, short punishmentType, int duration = -1)
		{
			bool flag = duration < 0;
			if (flag)
			{
				duration = PunishmentSeverity.Instance[punishmentSeverity].BountyDuration;
			}
			string tag = this.ToString();
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(27, 2);
			defaultInterpolatedStringHandler.AppendLiteral("add bounty on ");
			defaultInterpolatedStringHandler.AppendFormatted<GameData.Domains.Character.Character>(character);
			defaultInterpolatedStringHandler.AppendLiteral(" for ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(duration);
			defaultInterpolatedStringHandler.AppendLiteral(" months.");
			AdaptableLog.TagInfo(tag, defaultInterpolatedStringHandler.ToStringAndClear());
			bool flag2 = punishmentType < 0;
			if (flag2)
			{
				short predefinedLogId = 33;
				PunishmentSeverityItem item = PunishmentSeverity.Instance.GetItem(punishmentSeverity);
				PredefinedLog.Show(predefinedLogId, character, (item != null) ? item.Name : null);
			}
			int charId = character.GetId();
			SettlementPrison prison = this.Prison;
			sbyte grade = character.GetInteractionGrade();
			SettlementBounty bounty = prison.GetBounty(charId);
			bool flag3 = bounty != null;
			if (flag3)
			{
				bool flag4 = bounty.PunishmentSeverity >= punishmentSeverity;
				if (flag4)
				{
					return;
				}
				bounty.PunishmentSeverity = punishmentSeverity;
				bounty.PunishmentType = punishmentType;
				bounty.ExpireDate = DomainManager.World.GetCurrDate() + duration;
				bool flag5 = character.GetId() == DomainManager.Taiwu.GetTaiwuCharId();
				if (flag5)
				{
					DomainManager.Extra.TaiwuWantedResetSectInteracted(context, this.OrgTemplateId);
				}
			}
			else
			{
				DomainManager.Organization.RegisterSectFugitive(charId, this.OrgTemplateId);
				prison.Bounties.Add(new SettlementBounty
				{
					CharId = charId,
					PunishmentSeverity = punishmentSeverity,
					PunishmentType = punishmentType,
					ExpireDate = DomainManager.World.GetCurrDate() + duration,
					BountyAmount = this.CalcBountyAmount(grade)
				});
				bool flag6 = character.GetId() == DomainManager.Taiwu.GetTaiwuCharId();
				if (flag6)
				{
					DomainManager.Extra.TaiwuWantedResetSectInteracted(context, this.OrgTemplateId);
				}
			}
			DomainManager.Extra.SetSettlementPrison(context, this.Id, prison);
		}

		// Token: 0x060047A5 RID: 18341 RVA: 0x00285EBC File Offset: 0x002840BC
		public bool RemoveBounty(DataContext context, int charId)
		{
			SettlementPrison prison = this.Prison;
			SettlementBounty bounty = prison.OfflineRemoveBounty(charId);
			bool flag = bounty == null;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				this.OnSettlementBountyRemoved(context, bounty);
				DomainManager.Extra.SetSettlementPrison(context, this.Id, prison);
				result = true;
			}
			return result;
		}

		// Token: 0x060047A6 RID: 18342 RVA: 0x00285F08 File Offset: 0x00284108
		public void AddPrisoner(DataContext context, GameData.Domains.Character.Character character, short punishmentType)
		{
			PunishmentTypeItem punishmentTypeCfg = PunishmentType.Instance[punishmentType];
			sbyte punishmentSeverity = base.GetPunishmentTypeSeverity(punishmentTypeCfg, true);
			PunishmentSeverityItem punishmentSeverityCfg = PunishmentSeverity.Instance[punishmentSeverity];
			this.AddPrisoner(context, character, punishmentSeverity, punishmentType, punishmentSeverityCfg.PrisonTime);
		}

		// Token: 0x060047A7 RID: 18343 RVA: 0x00285F48 File Offset: 0x00284148
		public void AddPrisoner(DataContext context, GameData.Domains.Character.Character character, sbyte punishmentSeverity, short punishmentType, int duration = -1)
		{
			Tester.Assert(character.GetKidnapperId() < 0, "");
			Tester.Assert(character.GetId() != DomainManager.Taiwu.GetTaiwuCharId(), "");
			int charId = character.GetId();
			SettlementPrison prison = this.Prison;
			bool flag = duration < 0;
			if (flag)
			{
				PunishmentSeverityItem punishmentSeverityCfg = PunishmentSeverity.Instance[punishmentSeverity];
				duration = punishmentSeverityCfg.PrisonTime;
			}
			string tag = this.ToString();
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(26, 2);
			defaultInterpolatedStringHandler.AppendLiteral("add prisoner ");
			defaultInterpolatedStringHandler.AppendFormatted<GameData.Domains.Character.Character>(character);
			defaultInterpolatedStringHandler.AppendLiteral(" for ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(duration);
			defaultInterpolatedStringHandler.AppendLiteral(" months.");
			AdaptableLog.TagInfo(tag, defaultInterpolatedStringHandler.ToStringAndClear());
			SettlementPrisoner prisoner = prison.GetPrisoner(charId);
			bool flag2 = prisoner != null;
			if (flag2)
			{
				prisoner.PunishmentSeverity = punishmentSeverity;
				prisoner.PunishmentType = punishmentType;
				prisoner.Duration = duration;
				prisoner.KidnapBeginDate = DomainManager.World.GetCurrDate();
				prisoner.RopeItemKey = new ItemKey(12, 0, this.GetPrisonRopeTemplateId(punishmentSeverity), -1);
				prisoner.InitialMorality = character.GetBaseMorality();
				DomainManager.Extra.SetSettlementPrison(context, this.Id, prison);
			}
			else
			{
				prison.Prisoners.Add(new SettlementPrisoner
				{
					CharId = charId,
					PunishmentType = punishmentType,
					PunishmentSeverity = punishmentSeverity,
					Duration = duration,
					KidnapBeginDate = DomainManager.World.GetCurrDate(),
					RopeItemKey = new ItemKey(12, 0, this.GetPrisonRopeTemplateId(punishmentSeverity), -1),
					InitialMorality = character.GetBaseMorality(),
					SpouseCharId = DomainManager.Character.GetAliveSpouse(charId)
				});
				BasePrioritizedAction action;
				bool flag3 = DomainManager.Character.TryGetCharacterPrioritizedAction(charId, out action);
				if (flag3)
				{
					DomainManager.Character.RemoveCharacterPrioritizedAction(context, charId);
				}
				DomainManager.Character.LeaveGroup(context, character, false);
				DomainManager.Character.GroupMove(context, character, this.Location);
				DomainManager.Character.RemoveAllKidnappedChars(context, character, true);
				character.DeactivateExternalRelationState(context, 2);
				character.ActiveExternalRelationState(context, 32);
				bool flag4 = character.IsCompletelyInfected();
				if (flag4)
				{
					Events.RaiseInfectedCharacterLocationChanged(context, charId, character.GetLocation(), Location.Invalid);
				}
				else
				{
					Events.RaiseCharacterLocationChanged(context, charId, character.GetLocation(), Location.Invalid);
				}
				character.SetLocation(Location.Invalid, context);
				DomainManager.Organization.RegisterSectPrisoner(charId, this.OrgTemplateId);
				defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(19, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Prisoner ");
				defaultInterpolatedStringHandler.AppendFormatted<GameData.Domains.Character.Character>(character);
				defaultInterpolatedStringHandler.AppendLiteral(" added to ");
				defaultInterpolatedStringHandler.AppendFormatted(this.ToString());
				AdaptableLog.Info(defaultInterpolatedStringHandler.ToStringAndClear());
				bool flag5 = !this.RemoveBounty(context, charId);
				if (flag5)
				{
					DomainManager.Extra.SetSettlementPrison(context, this.Id, prison);
				}
			}
		}

		// Token: 0x060047A8 RID: 18344 RVA: 0x0028621C File Offset: 0x0028441C
		public bool RemovePrisoner(DataContext context, int charId)
		{
			SettlementPrison prison = this.Prison;
			SettlementPrisoner prisoner = prison.OfflineRemovePrisoner(charId);
			bool flag = prisoner == null;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				this.OnSettlementPrisonerRemoved(context, prisoner);
				DomainManager.Extra.SetSettlementPrison(context, this.Id, prison);
				result = true;
			}
			return result;
		}

		// Token: 0x060047A9 RID: 18345 RVA: 0x00286268 File Offset: 0x00284468
		private void OnSettlementPrisonerRemoved(DataContext context, SettlementPrisoner prisoner)
		{
			GameData.Domains.Character.Character character;
			bool flag = !DomainManager.Character.TryGetElement_Objects(prisoner.CharId, out character);
			if (flag)
			{
				DomainManager.Organization.UnregisterSectPrisoner(prisoner.CharId);
			}
			else
			{
				Location location = character.GetValidLocation();
				character.DeactivateExternalRelationState(context, 32);
				bool flag2 = character.IsCompletelyInfected();
				if (flag2)
				{
					Events.RaiseInfectedCharacterLocationChanged(context, prisoner.CharId, Location.Invalid, location);
				}
				else
				{
					Events.RaiseCharacterLocationChanged(context, prisoner.CharId, Location.Invalid, location);
				}
				character.SetLocation(location, context);
				DomainManager.Organization.UnregisterSectPrisoner(prisoner.CharId);
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(24, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Prisoner ");
				defaultInterpolatedStringHandler.AppendFormatted<GameData.Domains.Character.Character>(character);
				defaultInterpolatedStringHandler.AppendLiteral(" removed from ");
				defaultInterpolatedStringHandler.AppendFormatted(this.ToString());
				defaultInterpolatedStringHandler.AppendLiteral(".");
				AdaptableLog.Info(defaultInterpolatedStringHandler.ToStringAndClear());
			}
		}

		// Token: 0x060047AA RID: 18346 RVA: 0x00286358 File Offset: 0x00284558
		private void OnSettlementBountyRemoved(DataContext context, SettlementBounty bounty)
		{
			DomainManager.Organization.UnregisterSectFugitive(bounty.CharId, this.OrgTemplateId);
			bool flag = bounty.CharId == DomainManager.Taiwu.GetTaiwuCharId();
			if (flag)
			{
				DomainManager.Extra.TaiwuWantedResetSectInteracted(context, this.OrgTemplateId);
			}
		}

		// Token: 0x060047AB RID: 18347 RVA: 0x002863A8 File Offset: 0x002845A8
		public short GetPrisonRopeTemplateId(sbyte punishmentSeverity)
		{
			int grade = Math.Clamp((int)((punishmentSeverity - 1) * 2), 0, 8);
			return (short)(73 + grade);
		}

		// Token: 0x060047AC RID: 18348 RVA: 0x002863CC File Offset: 0x002845CC
		public void UpdatePrisonOnAdvanceMonth(DataContext context)
		{
			LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
			SettlementPrisonRecordCollection settlementPrisonRecordCollection = DomainManager.Organization.GetSettlementPrisonRecordCollection(context, this.Id);
			int currDate = DomainManager.World.GetCurrDate();
			SettlementPrison prison = this.Prison;
			bool isChanged = prison.Prisoners.Count > 0;
			int taiwuId = DomainManager.Taiwu.GetTaiwuCharId();
			KidnappedTravelData kidnappedData = DomainManager.Extra.GetKidnappedTravelData();
			for (int i = prison.Bounties.Count - 1; i >= 0; i--)
			{
				SettlementBounty bounty = prison.Bounties[i];
				bool flag = bounty.CharId == taiwuId && kidnappedData.Valid;
				if (!flag)
				{
					bool flag2 = bounty.ExpireDate <= currDate;
					if (flag2)
					{
						prison.Bounties.RemoveAt(i);
						this.OnSettlementBountyRemoved(context, bounty);
						isChanged = true;
					}
					else
					{
						GameData.Domains.Character.Character hunter;
						bool flag3 = bounty.CurrentHunterId >= 0 && (!DomainManager.Character.TryGetElement_Objects(bounty.CurrentHunterId, out hunter) || hunter.IsActiveExternalRelationState(32) || DomainManager.Organization.GetFugitiveBountySect(bounty.CurrentHunterId) >= 0);
						if (flag3)
						{
							bounty.CurrentHunterId = -1;
							isChanged = true;
						}
					}
				}
			}
			int j = prison.Prisoners.Count - 1;
			while (j >= 0)
			{
				SettlementPrisoner prisoner = prison.Prisoners[j];
				GameData.Domains.Character.Character character = DomainManager.Character.GetElement_Objects(prisoner.CharId);
				bool flag4 = !character.IsCompletelyInfected();
				if (!flag4)
				{
					goto IL_285;
				}
				this.ApplyPrisonerPunishmentOnAdvanceMonth(context, prisoner);
				bool flag5 = prisoner.KidnapBeginDate + prisoner.Duration <= currDate;
				if (!flag5)
				{
					goto IL_285;
				}
				short punishmentType = prisoner.PunishmentType;
				bool flag6 = punishmentType - 41 <= 1;
				bool flag7 = flag6;
				if (flag7)
				{
					MonthlyEventCollection monthlyEventCollection = DomainManager.World.GetMonthlyEventCollection();
					monthlyEventCollection.AddSentenceCompleted(prisoner.CharId, this.Id);
				}
				else
				{
					prison.Prisoners.RemoveAt(j);
					this.OnSettlementPrisonerRemoved(context, prisoner);
					PunishmentSeverityItem punishmentSeverity = PunishmentSeverity.Instance[prisoner.PunishmentSeverity];
					bool expel = punishmentSeverity.Expel;
					if (expel)
					{
						this.ExpelSectMember(context, this, character, prisoner.PunishmentType, prisoner.SpouseCharId);
					}
					else
					{
						lifeRecordCollection.AddBeReleasedUponCompletionOfASentence(prisoner.CharId, currDate, this.Id);
						settlementPrisonRecordCollection.AddBeReleasedUponCompletionOfASentence(currDate, this.Id, prisoner.CharId);
					}
				}
				IL_36A:
				j--;
				continue;
				IL_285:
				int resistance = this.CalcKidnappedCharacterResistance(prisoner);
				int escapeRate = prisoner.CalcEscapeRate(resistance, 0);
				bool isEscaped = context.Random.CheckPercentProb(escapeRate);
				string escapeText = isEscaped ? "√成功" : "×失败";
				AdaptableLog.Info("逃跑" + escapeText);
				AdaptableLog.Info("");
				bool flag8 = isEscaped;
				if (flag8)
				{
					prison.Prisoners.RemoveAt(j);
					this.OnSettlementPrisonerRemoved(context, prisoner);
					lifeRecordCollection.AddPrisonBreak(prisoner.CharId, currDate, this.Id);
					settlementPrisonRecordCollection.AddPrisonBreak(currDate, this.Id, prisoner.CharId);
					SecretInformationCollection secretInformationCollection = DomainManager.Information.GetSecretInformationCollection();
					int secretInfoOffset = secretInformationCollection.AddPrisonBreak(prisoner.CharId, this.Location);
					int secretInfoId = DomainManager.Information.AddSecretInformationMetaData(context, secretInfoOffset, true);
					DomainManager.Information.MakeSecretInformationBroadcastEffect(context, secretInfoId, -1);
				}
				goto IL_36A;
			}
			bool flag9 = isChanged;
			if (flag9)
			{
				DomainManager.Extra.SetSettlementPrison(context, this.Id, prison);
			}
			DomainManager.Organization.SetSettlementPrisonRecordCollection(context, this.Id, settlementPrisonRecordCollection);
		}

		// Token: 0x060047AD RID: 18349 RVA: 0x00286788 File Offset: 0x00284988
		public int CalcKidnappedCharacterResistance(SettlementPrisoner prisoner)
		{
			GameData.Domains.Character.Character kidnapped = DomainManager.Character.GetElement_Objects(prisoner.CharId);
			int behaviorFactor = (int)(kidnapped.GetBehaviorType() + 1);
			int severity = (int)(prisoner.PunishmentSeverity + 1);
			sbyte grade = kidnapped.GetOrganizationInfo().Grade;
			int baseValue = behaviorFactor * (severity * 5 - (int)grade);
			int consummateLevel = (from id in base.GetPrisonerRelatedGuards(prisoner)
			select (int)DomainManager.Character.GetElement_Objects(id).GetConsummateLevel()).Prepend(-1).Max();
			bool flag = consummateLevel == -1;
			if (flag)
			{
				consummateLevel = GlobalConfig.Instance.GuardConsummateLevel[Math.Clamp((int)prisoner.GetPrisonType(), 0, 2)];
			}
			int levelEffect = (int)(GlobalConfig.Instance.MaxConsummateLevel + kidnapped.GetConsummateLevel()) - consummateLevel;
			int currDate = DomainManager.World.GetCurrDate();
			int passTime = currDate - prisoner.KidnapBeginDate;
			int durationEffect = prisoner.Duration - 2 * passTime;
			int value = baseValue + levelEffect + durationEffect;
			AdaptableLog.Info("");
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(85, 14);
			defaultInterpolatedStringHandler.AppendFormatted(base.OrganizationConfig.Name);
			defaultInterpolatedStringHandler.AppendLiteral("关押");
			defaultInterpolatedStringHandler.AppendFormatted<GameData.Domains.Character.Character>(kidnapped);
			defaultInterpolatedStringHandler.AppendLiteral("，初始值：立场");
			defaultInterpolatedStringHandler.AppendFormatted<int>(behaviorFactor);
			defaultInterpolatedStringHandler.AppendLiteral(" * (罪行");
			defaultInterpolatedStringHandler.AppendFormatted<int>(severity);
			defaultInterpolatedStringHandler.AppendLiteral(" * 5 - 身份");
			defaultInterpolatedStringHandler.AppendFormatted<sbyte>(grade);
			defaultInterpolatedStringHandler.AppendLiteral(") = ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(baseValue);
			defaultInterpolatedStringHandler.AppendLiteral("，");
			defaultInterpolatedStringHandler.AppendLiteral("精纯影响：精纯上限");
			defaultInterpolatedStringHandler.AppendFormatted<sbyte>(GlobalConfig.Instance.MaxConsummateLevel);
			defaultInterpolatedStringHandler.AppendLiteral(" + 囚犯精纯");
			defaultInterpolatedStringHandler.AppendFormatted<sbyte>(kidnapped.GetConsummateLevel());
			defaultInterpolatedStringHandler.AppendLiteral(" - 守卫平均精纯");
			defaultInterpolatedStringHandler.AppendFormatted<int>(consummateLevel);
			defaultInterpolatedStringHandler.AppendLiteral(" = ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(levelEffect);
			defaultInterpolatedStringHandler.AppendLiteral("，");
			defaultInterpolatedStringHandler.AppendLiteral("时长影响：总时长");
			defaultInterpolatedStringHandler.AppendFormatted<int>(prisoner.Duration);
			defaultInterpolatedStringHandler.AppendLiteral(" - 2 * 已关押时长");
			defaultInterpolatedStringHandler.AppendFormatted<int>(passTime);
			defaultInterpolatedStringHandler.AppendLiteral(" = ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(durationEffect);
			defaultInterpolatedStringHandler.AppendLiteral("，");
			defaultInterpolatedStringHandler.AppendLiteral("共计：");
			defaultInterpolatedStringHandler.AppendFormatted<int>(value);
			AdaptableLog.Info(defaultInterpolatedStringHandler.ToStringAndClear());
			return value;
		}

		// Token: 0x060047AE RID: 18350 RVA: 0x00286A0C File Offset: 0x00284C0C
		public void ExpelSectMember(DataContext context, Sect sect, GameData.Domains.Character.Character character, short punishmentType, int spouseCharId)
		{
			int charId = character.GetId();
			OrganizationInfo orgInfo = character.GetOrganizationInfo();
			sbyte sectTemplateId = sect.GetOrgTemplateId();
			short sectSettlementId = sect.GetId();
			bool flag = orgInfo.OrgTemplateId == sectTemplateId;
			if (flag)
			{
				HashSet<int> mentorSet = DomainManager.Character.GetRelatedCharIds(charId, 2048);
				HashSet<int> menteeSet = DomainManager.Character.GetRelatedCharIds(charId, 4096);
				HashSet<int> hashSet = ObjectPool<HashSet<int>>.Instance.Get();
				hashSet.Clear();
				hashSet.UnionWith(mentorSet);
				foreach (int relatedCharId in hashSet)
				{
					DomainManager.Character.ChangeRelationType(context, charId, relatedCharId, 2048, 0);
					DomainManager.Character.ChangeRelationType(context, relatedCharId, charId, 4096, 0);
				}
				hashSet.Clear();
				hashSet.UnionWith(menteeSet);
				foreach (int relatedCharId2 in hashSet)
				{
					DomainManager.Character.ChangeRelationType(context, charId, relatedCharId2, 4096, 0);
					DomainManager.Character.ChangeRelationType(context, relatedCharId2, charId, 2048, 0);
				}
			}
			LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
			int date = DomainManager.World.GetCurrDate();
			bool flag2 = punishmentType == 20 && spouseCharId >= 0;
			if (flag2)
			{
				lifeRecordCollection.AddBeImplicatedSectPunishLevel5Expel(character.GetId(), date, spouseCharId, sectSettlementId);
			}
			else
			{
				lifeRecordCollection.AddSectPunishLevel5Expel(character.GetId(), date, punishmentType, sectSettlementId);
			}
			DomainManager.Organization.JoinNearbyVillageTownAsBeggar(context, character, -1);
		}

		// Token: 0x060047AF RID: 18351 RVA: 0x00286BD8 File Offset: 0x00284DD8
		private unsafe void ApplyPrisonerPunishmentOnAdvanceMonth(DataContext context, SettlementPrisoner prisoner)
		{
			int charId = prisoner.CharId;
			GameData.Domains.Character.Character character;
			bool flag = !DomainManager.Character.TryGetElement_Objects(charId, out character);
			if (!flag)
			{
				bool flag2 = character.GetAgeGroup() != 2;
				if (!flag2)
				{
					int currDate = DomainManager.World.GetCurrDate();
					LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
					switch (this.OrgTemplateId)
					{
					case 1:
					{
						HashSet<int> relatedCharIds = context.AdvanceMonthRelatedData.RelatedCharIds.Occupy();
						DomainManager.Character.GetAllRelatedCharIds(charId, relatedCharIds, true);
						foreach (int relatedCharId in relatedCharIds)
						{
							GameData.Domains.Character.Character relatedChar;
							bool flag3 = !DomainManager.Character.TryGetElement_Objects(relatedCharId, out relatedChar);
							if (!flag3)
							{
								RelatedCharacter targetToSelf;
								bool flag4 = DomainManager.Character.TryGetRelation(relatedCharId, charId, out targetToSelf) && targetToSelf.Favorability > 0;
								if (flag4)
								{
									DomainManager.Character.ChangeFavorabilityOptional(context, relatedChar, character, -1000, 5);
								}
								RelatedCharacter selfToTarget;
								bool flag5 = DomainManager.Character.TryGetRelation(charId, relatedCharId, out selfToTarget) && selfToTarget.Favorability > 0;
								if (flag5)
								{
									DomainManager.Character.ChangeFavorabilityOptional(context, character, relatedChar, -1000, 5);
								}
							}
						}
						context.AdvanceMonthRelatedData.RelatedCharIds.Release(ref relatedCharIds);
						lifeRecordCollection.AddImprisonedShaoLin(charId, currDate, this.Id);
						break;
					}
					case 2:
					{
						List<int> targetCharIds = context.AdvanceMonthRelatedData.TargetCharIdList.Occupy();
						for (sbyte grade = 0; grade < 9; grade += 1)
						{
							HashSet<int> gradeMembers = this.Members.GetMembers(grade);
							foreach (int gradeMemberId in gradeMembers)
							{
								GameData.Domains.Character.Character gradeMember;
								bool flag6 = !DomainManager.Character.TryGetElement_Objects(gradeMemberId, out gradeMember);
								if (!flag6)
								{
									bool flag7 = character.CanTeachCombatSkill(gradeMember);
									if (flag7)
									{
										targetCharIds.Add(gradeMemberId);
									}
								}
							}
						}
						int targetCharId = targetCharIds.GetRandomOrDefault(context.Random, -1);
						context.AdvanceMonthRelatedData.TargetCharIdList.Release(ref targetCharIds);
						bool flag8 = targetCharId < 0;
						if (!flag8)
						{
							GameData.Domains.Character.Character targetChar = DomainManager.Character.GetElement_Objects(targetCharId);
							List<ValueTuple<short, short>> weightTable = context.AdvanceMonthRelatedData.WeightTable.Occupy();
							character.GetTeachableCombatSkillBookIds(targetChar, weightTable);
							short skillBookTemplateId = RandomUtils.GetRandomResult<short>(weightTable, context.Random);
							context.AdvanceMonthRelatedData.WeightTable.Release(ref weightTable);
							SkillBookItem bookCfg = Config.SkillBook.Instance[skillBookTemplateId];
							Dictionary<short, GameData.Domains.CombatSkill.CombatSkill> selfCombatSkills = DomainManager.CombatSkill.GetCharCombatSkills(charId);
							ushort readingState = selfCombatSkills[bookCfg.CombatSkillTemplateId].GetReadingState();
							byte currInternalIndex;
							for (currInternalIndex = 0; currInternalIndex < 15; currInternalIndex += 1)
							{
								bool flag9 = CombatSkillStateHelper.IsPageRead(readingState, currInternalIndex);
								if (flag9)
								{
									break;
								}
							}
							CombatSkillShorts combatSkillAttainments = *targetChar.GetCombatSkillAttainments();
							CombatSkillShorts combatSkillQualifications = *targetChar.GetCombatSkillQualifications();
							Personalities personalities = targetChar.GetPersonalities();
							int successRate = GameData.Domains.Character.Character.GetTaughtNewSkillSuccessRate(bookCfg.Grade, *combatSkillQualifications[(int)bookCfg.CombatSkillType], *combatSkillAttainments[(int)bookCfg.CombatSkillType], *personalities[1]);
							byte pageTypes = CombatSkillStateHelper.GeneratePageTypesFromReadingState(context.Random, readingState);
							TeachCombatSkillAction action = new TeachCombatSkillAction
							{
								GeneratedPageTypes = pageTypes,
								InternalIndex = currInternalIndex,
								SkillTemplateId = bookCfg.CombatSkillTemplateId,
								Succeed = context.Random.CheckPercentProb(successRate)
							};
							action.ApplyChanges(context, character, targetChar);
							bool succeed = action.Succeed;
							if (succeed)
							{
								prisoner.Duration = Math.Max(0, prisoner.Duration - 1);
								lifeRecordCollection.AddImprisonedEmei1(charId, currDate, this.Id);
							}
							else
							{
								prisoner.Duration++;
								lifeRecordCollection.AddImprisonedEmei2(charId, currDate, this.Id);
							}
						}
						break;
					}
					case 3:
					{
						bool flag10 = character.GetCurrNeili() > 0;
						if (flag10)
						{
							character.ChangeCurrNeiliWithoutChecking(context, -character.GetMaxNeili() * 3 / 10);
						}
						else
						{
							NeiliAllocation baseNeiliAllocation = character.GetBaseNeiliAllocation();
							for (int i = 0; i < 30; i++)
							{
								byte maxType = baseNeiliAllocation.GetMaxType();
								bool flag11 = *baseNeiliAllocation[(int)maxType] == 0;
								if (flag11)
								{
									break;
								}
								ref short ptr = ref baseNeiliAllocation[(int)maxType];
								ptr -= 1;
							}
							character.SetBaseNeiliAllocation(baseNeiliAllocation, context);
						}
						lifeRecordCollection.AddImprisonedBaihua(charId, currDate, this.Id);
						break;
					}
					case 4:
						character.ChangeDisorderOfQi(context, 500);
						lifeRecordCollection.AddImprisonedWudang(charId, currDate, this.Id);
						break;
					case 5:
						character.ChangeXiangshuInfection(context, 10);
						lifeRecordCollection.AddImprisonedYuanshan(charId, currDate, this.Id);
						break;
					case 6:
					{
						List<int> availableSkillIndices = context.AdvanceMonthRelatedData.IntList.Occupy();
						List<GameData.Domains.Character.LifeSkillItem> learnedLifeSkills = character.GetLearnedLifeSkills();
						for (int j = learnedLifeSkills.Count - 1; j >= 0; j--)
						{
							GameData.Domains.Character.LifeSkillItem skill = learnedLifeSkills[j];
							bool flag12 = skill.ReadingState > 0;
							if (flag12)
							{
								availableSkillIndices.Add(j);
							}
						}
						Span<byte> span = new Span<byte>(stackalloc byte[(UIntPtr)5], 5);
						SpanList<byte> pages = span;
						int k = 0;
						while (k < 3 && availableSkillIndices.Count > 0)
						{
							int index = availableSkillIndices.GetRandom(context.Random);
							GameData.Domains.Character.LifeSkillItem skill2 = learnedLifeSkills[index];
							bool isAllPagedRead = skill2.IsAllPagesRead();
							pages.Clear();
							for (byte page = 0; page < 5; page += 1)
							{
								bool flag13 = skill2.IsPageRead(page);
								if (flag13)
								{
									pages.Add(page);
								}
							}
							byte selectedPage = pages.GetRandom(context.Random);
							skill2.SetPageUnread(selectedPage);
							learnedLifeSkills[index] = skill2;
							bool flag14 = skill2.GetReadPagesCount() == 0;
							if (flag14)
							{
								availableSkillIndices.Remove(index);
							}
							bool flag15 = skill2.IsAllPagesRead() != isAllPagedRead && isAllPagedRead;
							if (flag15)
							{
								Config.LifeSkillItem cfg = LifeSkill.Instance.GetItem(skill2.SkillTemplateId);
								short templateId = Config.LifeSkillType.Instance[cfg.Type].InformationTemplateId;
								DomainManager.Information.DiscardNormalInformation(context, charId, new NormalInformation(templateId, cfg.Grade));
							}
							k++;
						}
						character.SetLearnedLifeSkills(learnedLifeSkills, context);
						context.AdvanceMonthRelatedData.IntList.Release(ref availableSkillIndices);
						lifeRecordCollection.AddImprisonedShingXiang(charId, currDate, this.Id);
						break;
					}
					case 7:
					{
						bool flag16 = prisoner.InitialMorality == 0 || character.GetFixedMorality() != short.MaxValue;
						if (!flag16)
						{
							sbyte initBehaviorType = GameData.Domains.Character.BehaviorType.GetBehaviorType(prisoner.InitialMorality);
							sbyte currBehaviorType = character.GetBehaviorType();
							switch (initBehaviorType)
							{
							case 0:
							{
								bool flag17 = currBehaviorType >= 4;
								if (flag17)
								{
									return;
								}
								character.ChangeBaseMorality(context, -25);
								break;
							}
							case 1:
							{
								bool flag18 = currBehaviorType >= 3;
								if (flag18)
								{
									return;
								}
								character.ChangeBaseMorality(context, -25);
								break;
							}
							case 2:
							{
								bool flag19 = prisoner.InitialMorality > 0;
								if (flag19)
								{
									character.ChangeBaseMorality(context, -25);
								}
								else
								{
									character.ChangeBaseMorality(context, 25);
								}
								break;
							}
							case 3:
							{
								bool flag20 = currBehaviorType <= 1;
								if (flag20)
								{
									return;
								}
								character.ChangeBaseMorality(context, 25);
								break;
							}
							case 4:
							{
								bool flag21 = currBehaviorType <= 0;
								if (flag21)
								{
									return;
								}
								character.ChangeBaseMorality(context, 25);
								break;
							}
							}
							lifeRecordCollection.AddImprisonedRanShan(charId, currDate, this.Id);
						}
						break;
					}
					case 8:
					{
						bool flag22 = !DomainManager.World.CheckDateInterval(prisoner.KidnapBeginDate, 6);
						if (!flag22)
						{
							List<short> featureIds = character.GetFeatureIds();
							List<short> downgradeFeatures = ObjectPool<List<short>>.Instance.Get();
							foreach (short featureId in featureIds)
							{
								CharacterFeatureItem featureCfg = CharacterFeature.Instance[featureId];
								bool flag23 = featureCfg.Degrade() == null;
								if (!flag23)
								{
									downgradeFeatures.Add(featureId);
								}
							}
							bool flag24 = downgradeFeatures.Count > 0;
							if (flag24)
							{
								short selectedFeatureId = downgradeFeatures.GetRandom(context.Random);
								CharacterFeatureItem newFeatureCfg = CharacterFeature.Instance[selectedFeatureId].Degrade();
								character.AddFeature(context, newFeatureCfg.TemplateId, true);
								lifeRecordCollection.AddImprisonedXuanNv(charId, currDate, this.Id);
							}
							ObjectPool<List<short>>.Instance.Return(downgradeFeatures);
						}
						break;
					}
					case 9:
					{
						bool flag25 = !DomainManager.World.CheckDateInterval(prisoner.KidnapBeginDate, 3);
						if (!flag25)
						{
							character.ChangeCurrAge(context, 1);
							lifeRecordCollection.AddImprisonedZhuJian(charId, currDate, this.Id);
						}
						break;
					}
					case 10:
					{
						IntPtr intPtr = stackalloc byte[(UIntPtr)12];
						*intPtr = 8;
						*(intPtr + 2) = 17;
						*(intPtr + (IntPtr)2 * 2) = 26;
						*(intPtr + (IntPtr)3 * 2) = 35;
						*(intPtr + (IntPtr)4 * 2) = 44;
						*(intPtr + (IntPtr)5 * 2) = 53;
						Span<short> span2 = new Span<short>(intPtr, 6);
						Span<short> poisons = span2;
						sbyte poisonType = (sbyte)context.Random.Next(6);
						short templateId2 = *poisons[(int)poisonType];
						character.ApplyEatingItemInstantEffects(context, 8, templateId2, null);
						lifeRecordCollection.AddImprisonedKongSang(charId, currDate, this.Id, 8, templateId2);
						break;
					}
					case 11:
					{
						NeiliAllocation extraNeiliAllocation = character.GetExtraNeiliAllocation();
						byte maxType2 = extraNeiliAllocation.GetMaxType();
						int maxValue = (int)(*extraNeiliAllocation[(int)maxType2]);
						bool flag26 = maxValue > 0;
						if (flag26)
						{
							short delta = (short)(-(short)Math.Min(maxValue, 5));
							character.ChangeExtraNeiliAllocation(context, maxType2, delta);
							lifeRecordCollection.AddImprisonedJinGang(charId, currDate, this.Id);
						}
						break;
					}
					case 12:
					{
						int beginDate = prisoner.KidnapBeginDate;
						bool flag27 = (currDate - beginDate) % 6 != 0;
						if (!flag27)
						{
							EatingItems eatingItems = *character.GetEatingItems();
							Span<sbyte> span3 = new Span<sbyte>(stackalloc byte[(UIntPtr)8], 8);
							SpanList<sbyte> wugs = span3;
							for (sbyte wugType = 0; wugType < 8; wugType += 1)
							{
								wugs.Add(wugType);
							}
							for (int l = 0; l < 9; l++)
							{
								ItemKey itemKey = (ItemKey)(*(ref eatingItems.ItemKeys.FixedElementField + (IntPtr)l * 8));
								bool flag28 = EatingItems.IsWug(itemKey);
								if (flag28)
								{
									ref wugs.Remove(Config.Medicine.Instance[itemKey.TemplateId].WugType);
								}
							}
							bool flag29 = wugs.Count == 0;
							if (!flag29)
							{
								sbyte randWug = wugs.GetRandom(context.Random);
								short wugTemplateId = ItemDomain.GetWugTemplateId(randWug, 4);
								character.AddWug(context, wugTemplateId);
								lifeRecordCollection.AddImprisonedWuXian(charId, currDate, this.Id, 8, wugTemplateId);
							}
						}
						break;
					}
					case 13:
					{
						OrgMemberCollection members = base.GetMembers();
						List<int> targets = context.AdvanceMonthRelatedData.TargetCharIdList.Occupy();
						HashSet<int> gradeMembers2 = members.GetMembers(character.GetInteractionGrade());
						foreach (int memberId in gradeMembers2)
						{
							GameData.Domains.Character.Character member = DomainManager.Character.GetElement_Objects(memberId);
							bool flag30 = !member.IsInteractableAsIntelligentCharacter();
							if (!flag30)
							{
								targets.Add(memberId);
							}
						}
						bool flag31 = targets.Count > 0;
						if (flag31)
						{
							int targetCharId2 = targets.GetRandom(context.Random);
							GameData.Domains.Character.Character targetChar2 = DomainManager.Character.GetElement_Objects(targetCharId2);
							AiHelper.NpcCombatResultType resultType = DomainManager.Character.SimulateCharacterCombat(context, character, targetChar2, CombatType.Beat, false, 1);
							bool flag32 = resultType <= AiHelper.NpcCombatResultType.MinorVictory;
							bool flag33 = flag32;
							if (flag33)
							{
								prisoner.Duration = Math.Max(0, prisoner.Duration - 1);
								lifeRecordCollection.AddImprisonedJieQing1(charId, currDate, this.Id);
							}
							else
							{
								prisoner.Duration++;
								lifeRecordCollection.AddImprisonedJieQing2(charId, currDate, this.Id);
							}
						}
						context.AdvanceMonthRelatedData.TargetCharIdList.Release(ref targets);
						break;
					}
					case 14:
						character.ChangeHealth(context, -24);
						lifeRecordCollection.AddImprisonedFuLong(charId, currDate, this.Id);
						break;
					case 15:
					{
						sbyte bodyPart = (sbyte)context.Random.Next(7);
						bool isInner = context.Random.NextBool();
						character.ChangeInjury(context, bodyPart, isInner, 1);
						bool flag34 = context.Random.NextBool();
						if (flag34)
						{
							short featureId2 = isInner ? BreakFeatureHelper.BodyPart2HurtFeature[bodyPart] : BreakFeatureHelper.BodyPart2CrashFeature[bodyPart];
							bool flag35 = !character.GetFeatureIds().Contains(featureId2);
							if (flag35)
							{
								character.AddFeature(context, featureId2, false);
								DomainManager.SpecialEffect.Add(context, charId, SpecialEffectDomain.BreakBodyFeatureEffectClassName[featureId2]);
							}
						}
						lifeRecordCollection.AddImprisonedXueHou(charId, currDate, this.Id, bodyPart);
						break;
					}
					}
				}
			}
		}

		// Token: 0x060047B0 RID: 18352 RVA: 0x00287928 File Offset: 0x00285B28
		public unsafe override void SetCulture(short culture, DataContext context)
		{
			this.Culture = culture;
			base.SetModifiedAndInvalidateInfluencedCache(3, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				byte* pData = OperationAdder.DynamicObjectCollection_SetFixedField<short>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this.Id, 7U, 2);
				*(short*)pData = this.Culture;
				pData += 2;
			}
		}

		// Token: 0x060047B1 RID: 18353 RVA: 0x00287988 File Offset: 0x00285B88
		public unsafe override void SetMaxCulture(short maxCulture, DataContext context)
		{
			this.MaxCulture = maxCulture;
			base.SetModifiedAndInvalidateInfluencedCache(4, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				byte* pData = OperationAdder.DynamicObjectCollection_SetFixedField<short>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this.Id, 9U, 2);
				*(short*)pData = this.MaxCulture;
				pData += 2;
			}
		}

		// Token: 0x060047B2 RID: 18354 RVA: 0x002879E8 File Offset: 0x00285BE8
		public unsafe override void SetSafety(short safety, DataContext context)
		{
			this.Safety = safety;
			base.SetModifiedAndInvalidateInfluencedCache(5, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				byte* pData = OperationAdder.DynamicObjectCollection_SetFixedField<short>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this.Id, 11U, 2);
				*(short*)pData = this.Safety;
				pData += 2;
			}
		}

		// Token: 0x060047B3 RID: 18355 RVA: 0x00287A48 File Offset: 0x00285C48
		public unsafe override void SetMaxSafety(short maxSafety, DataContext context)
		{
			this.MaxSafety = maxSafety;
			base.SetModifiedAndInvalidateInfluencedCache(6, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				byte* pData = OperationAdder.DynamicObjectCollection_SetFixedField<short>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this.Id, 13U, 2);
				*(short*)pData = this.MaxSafety;
				pData += 2;
			}
		}

		// Token: 0x060047B4 RID: 18356 RVA: 0x00287AA8 File Offset: 0x00285CA8
		public unsafe override void SetPopulation(int population, DataContext context)
		{
			this.Population = population;
			base.SetModifiedAndInvalidateInfluencedCache(7, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				byte* pData = OperationAdder.DynamicObjectCollection_SetFixedField<short>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this.Id, 15U, 4);
				*(int*)pData = this.Population;
				pData += 4;
			}
		}

		// Token: 0x060047B5 RID: 18357 RVA: 0x00287B08 File Offset: 0x00285D08
		public unsafe override void SetMaxPopulation(int maxPopulation, DataContext context)
		{
			this.MaxPopulation = maxPopulation;
			base.SetModifiedAndInvalidateInfluencedCache(8, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				byte* pData = OperationAdder.DynamicObjectCollection_SetFixedField<short>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this.Id, 19U, 4);
				*(int*)pData = this.MaxPopulation;
				pData += 4;
			}
		}

		// Token: 0x060047B6 RID: 18358 RVA: 0x00287B68 File Offset: 0x00285D68
		public unsafe override void SetStandardOnStagePopulation(int standardOnStagePopulation, DataContext context)
		{
			this.StandardOnStagePopulation = standardOnStagePopulation;
			base.SetModifiedAndInvalidateInfluencedCache(9, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				byte* pData = OperationAdder.DynamicObjectCollection_SetFixedField<short>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this.Id, 23U, 4);
				*(int*)pData = this.StandardOnStagePopulation;
				pData += 4;
			}
		}

		// Token: 0x060047B7 RID: 18359 RVA: 0x00287BCC File Offset: 0x00285DCC
		public unsafe override void SetMembers(OrgMemberCollection members, DataContext context)
		{
			this.Members = members;
			base.SetModifiedAndInvalidateInfluencedCache(10, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this.Members.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<short>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this.Id, 0, dataSize);
				pData += this.Members.Serialize(pData);
			}
		}

		// Token: 0x060047B8 RID: 18360 RVA: 0x00287C3C File Offset: 0x00285E3C
		public unsafe override void SetLackingCoreMembers(OrgMemberCollection lackingCoreMembers, DataContext context)
		{
			this.LackingCoreMembers = lackingCoreMembers;
			base.SetModifiedAndInvalidateInfluencedCache(11, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this.LackingCoreMembers.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<short>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this.Id, 1, dataSize);
				pData += this.LackingCoreMembers.Serialize(pData);
			}
		}

		// Token: 0x060047B9 RID: 18361 RVA: 0x00287CAC File Offset: 0x00285EAC
		public unsafe override void SetApprovingRateUpperLimitBonus(short approvingRateUpperLimitBonus, DataContext context)
		{
			this.ApprovingRateUpperLimitBonus = approvingRateUpperLimitBonus;
			base.SetModifiedAndInvalidateInfluencedCache(12, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				byte* pData = OperationAdder.DynamicObjectCollection_SetFixedField<short>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this.Id, 27U, 2);
				*(short*)pData = this.ApprovingRateUpperLimitBonus;
				pData += 2;
			}
		}

		// Token: 0x060047BA RID: 18362 RVA: 0x00287D10 File Offset: 0x00285F10
		public unsafe override void SetInfluencePowerUpdateDate(int influencePowerUpdateDate, DataContext context)
		{
			this.InfluencePowerUpdateDate = influencePowerUpdateDate;
			base.SetModifiedAndInvalidateInfluencedCache(13, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				byte* pData = OperationAdder.DynamicObjectCollection_SetFixedField<short>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this.Id, 29U, 4);
				*(int*)pData = this.InfluencePowerUpdateDate;
				pData += 4;
			}
		}

		// Token: 0x060047BB RID: 18363 RVA: 0x00287D74 File Offset: 0x00285F74
		public short GetMinSeniorityId()
		{
			return this._minSeniorityId;
		}

		// Token: 0x060047BC RID: 18364 RVA: 0x00287D8C File Offset: 0x00285F8C
		public unsafe void SetMinSeniorityId(short minSeniorityId, DataContext context)
		{
			this._minSeniorityId = minSeniorityId;
			base.SetModifiedAndInvalidateInfluencedCache(14, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				byte* pData = OperationAdder.DynamicObjectCollection_SetFixedField<short>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this.Id, 33U, 2);
				*(short*)pData = this._minSeniorityId;
				pData += 2;
			}
		}

		// Token: 0x060047BD RID: 18365 RVA: 0x00287DF0 File Offset: 0x00285FF0
		public List<short> GetAvailableMonasticTitleSuffixIds()
		{
			return this._availableMonasticTitleSuffixIds;
		}

		// Token: 0x060047BE RID: 18366 RVA: 0x00287E08 File Offset: 0x00286008
		public unsafe void SetAvailableMonasticTitleSuffixIds(List<short> availableMonasticTitleSuffixIds, DataContext context)
		{
			this._availableMonasticTitleSuffixIds = availableMonasticTitleSuffixIds;
			base.SetModifiedAndInvalidateInfluencedCache(15, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int elementsCount = this._availableMonasticTitleSuffixIds.Count;
				int contentSize = 2 * elementsCount;
				int dataSize = 2 + contentSize;
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<short>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this.Id, 2, dataSize);
				*(short*)pData = (short)((ushort)elementsCount);
				pData += 2;
				for (int i = 0; i < elementsCount; i++)
				{
					*(short*)(pData + (IntPtr)i * 2) = this._availableMonasticTitleSuffixIds[i];
				}
				pData += contentSize;
			}
		}

		// Token: 0x060047BF RID: 18367 RVA: 0x00287EB0 File Offset: 0x002860B0
		public byte GetTaiwuExploreStatus()
		{
			return this._taiwuExploreStatus;
		}

		// Token: 0x060047C0 RID: 18368 RVA: 0x00287EC8 File Offset: 0x002860C8
		public unsafe void SetTaiwuExploreStatus(byte taiwuExploreStatus, DataContext context)
		{
			this._taiwuExploreStatus = taiwuExploreStatus;
			base.SetModifiedAndInvalidateInfluencedCache(16, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				byte* pData = OperationAdder.DynamicObjectCollection_SetFixedField<short>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this.Id, 35U, 1);
				*pData = this._taiwuExploreStatus;
				pData++;
			}
		}

		// Token: 0x060047C1 RID: 18369 RVA: 0x00287F2C File Offset: 0x0028612C
		public bool GetSpiritualDebtInteractionOccurred()
		{
			return this._spiritualDebtInteractionOccurred;
		}

		// Token: 0x060047C2 RID: 18370 RVA: 0x00287F44 File Offset: 0x00286144
		public unsafe void SetSpiritualDebtInteractionOccurred(bool spiritualDebtInteractionOccurred, DataContext context)
		{
			this._spiritualDebtInteractionOccurred = spiritualDebtInteractionOccurred;
			base.SetModifiedAndInvalidateInfluencedCache(17, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				byte* pData = OperationAdder.DynamicObjectCollection_SetFixedField<short>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this.Id, 36U, 1);
				*pData = (this._spiritualDebtInteractionOccurred ? 1 : 0);
				pData++;
			}
		}

		// Token: 0x060047C3 RID: 18371 RVA: 0x00287FA8 File Offset: 0x002861A8
		public int[] GetTaiwuInvestmentForMartialArtTournament()
		{
			return this._taiwuInvestmentForMartialArtTournament;
		}

		// Token: 0x060047C4 RID: 18372 RVA: 0x00287FC0 File Offset: 0x002861C0
		public unsafe void SetTaiwuInvestmentForMartialArtTournament(int[] taiwuInvestmentForMartialArtTournament, DataContext context)
		{
			this._taiwuInvestmentForMartialArtTournament = taiwuInvestmentForMartialArtTournament;
			base.SetModifiedAndInvalidateInfluencedCache(18, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				byte* pData = OperationAdder.DynamicObjectCollection_SetFixedField<short>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this.Id, 37U, 12);
				for (int i = 0; i < 3; i++)
				{
					*(int*)(pData + (IntPtr)i * 4) = this._taiwuInvestmentForMartialArtTournament[i];
				}
				pData += 12;
			}
		}

		// Token: 0x060047C5 RID: 18373 RVA: 0x0028803C File Offset: 0x0028623C
		public override short GetApprovingRateUpperLimitTempBonus()
		{
			ObjectCollectionDataStates dataStates = this.CollectionHelperData.DataStates;
			Thread.MemoryBarrier();
			bool flag = dataStates.IsCached(this.DataStatesOffset, 19);
			short approvingRateUpperLimitTempBonus;
			if (flag)
			{
				approvingRateUpperLimitTempBonus = this.ApprovingRateUpperLimitTempBonus;
			}
			else
			{
				short value = base.CalcApprovingRateUpperLimitTempBonus();
				bool lockTaken = false;
				try
				{
					this._spinLock.Enter(ref lockTaken);
					this.ApprovingRateUpperLimitTempBonus = value;
					dataStates.SetCached(this.DataStatesOffset, 19);
				}
				finally
				{
					bool flag2 = lockTaken;
					if (flag2)
					{
						this._spinLock.Exit(false);
					}
				}
				Thread.MemoryBarrier();
				approvingRateUpperLimitTempBonus = this.ApprovingRateUpperLimitTempBonus;
			}
			return approvingRateUpperLimitTempBonus;
		}

		// Token: 0x060047C6 RID: 18374 RVA: 0x002880E4 File Offset: 0x002862E4
		public Sect()
		{
			this.Members = new OrgMemberCollection();
			this.LackingCoreMembers = new OrgMemberCollection();
			this._availableMonasticTitleSuffixIds = new List<short>();
			this._taiwuInvestmentForMartialArtTournament = new int[3];
		}

		// Token: 0x060047C7 RID: 18375 RVA: 0x0028814C File Offset: 0x0028634C
		public bool IsSerializedSizeFixed()
		{
			return false;
		}

		// Token: 0x060047C8 RID: 18376 RVA: 0x00288160 File Offset: 0x00286360
		public int GetSerializedSize()
		{
			int totalSize = 61;
			int dataSize = this.Members.GetSerializedSize();
			totalSize += dataSize;
			int dataSize2 = this.LackingCoreMembers.GetSerializedSize();
			totalSize += dataSize2;
			int elementsCount = this._availableMonasticTitleSuffixIds.Count;
			int contentSize = 2 * elementsCount;
			int dataSize3 = 2 + contentSize;
			return totalSize + dataSize3;
		}

		// Token: 0x060047C9 RID: 18377 RVA: 0x002881BC File Offset: 0x002863BC
		public unsafe int Serialize(byte* pData)
		{
			*(short*)pData = this.Id;
			byte* pCurrData = pData + 2;
			*pCurrData = (byte)this.OrgTemplateId;
			pCurrData++;
			pCurrData += this.Location.Serialize(pCurrData);
			*(short*)pCurrData = this.Culture;
			pCurrData += 2;
			*(short*)pCurrData = this.MaxCulture;
			pCurrData += 2;
			*(short*)pCurrData = this.Safety;
			pCurrData += 2;
			*(short*)pCurrData = this.MaxSafety;
			pCurrData += 2;
			*(int*)pCurrData = this.Population;
			pCurrData += 4;
			*(int*)pCurrData = this.MaxPopulation;
			pCurrData += 4;
			*(int*)pCurrData = this.StandardOnStagePopulation;
			pCurrData += 4;
			*(short*)pCurrData = this.ApprovingRateUpperLimitBonus;
			pCurrData += 2;
			*(int*)pCurrData = this.InfluencePowerUpdateDate;
			pCurrData += 4;
			*(short*)pCurrData = this._minSeniorityId;
			pCurrData += 2;
			*pCurrData = this._taiwuExploreStatus;
			pCurrData++;
			*pCurrData = (this._spiritualDebtInteractionOccurred ? 1 : 0);
			pCurrData++;
			bool flag = this._taiwuInvestmentForMartialArtTournament.Length != 3;
			if (flag)
			{
				throw new Exception("Elements count of field _taiwuInvestmentForMartialArtTournament is not equal to declaration");
			}
			for (int i = 0; i < 3; i++)
			{
				*(int*)(pCurrData + (IntPtr)i * 4) = this._taiwuInvestmentForMartialArtTournament[i];
			}
			pCurrData += 12;
			byte* pBegin = pCurrData;
			pCurrData += 4;
			pCurrData += this.Members.Serialize(pCurrData);
			int fieldSize = (int)((long)(pCurrData - pBegin) - 4L);
			bool flag2 = fieldSize > 4194304;
			if (flag2)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("Members");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin = fieldSize;
			byte* pBegin2 = pCurrData;
			pCurrData += 4;
			pCurrData += this.LackingCoreMembers.Serialize(pCurrData);
			int fieldSize2 = (int)((long)(pCurrData - pBegin2) - 4L);
			bool flag3 = fieldSize2 > 4194304;
			if (flag3)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("LackingCoreMembers");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin2 = fieldSize2;
			int elementsCount = this._availableMonasticTitleSuffixIds.Count;
			int contentSize = 2 * elementsCount;
			bool flag4 = contentSize > 4194300;
			if (flag4)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_availableMonasticTitleSuffixIds");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pCurrData = contentSize + 2;
			pCurrData += 4;
			*(short*)pCurrData = (short)((ushort)elementsCount);
			pCurrData += 2;
			for (int j = 0; j < elementsCount; j++)
			{
				*(short*)(pCurrData + (IntPtr)j * 2) = this._availableMonasticTitleSuffixIds[j];
			}
			pCurrData += contentSize;
			return (int)((long)(pCurrData - pData));
		}

		// Token: 0x060047CA RID: 18378 RVA: 0x002884B4 File Offset: 0x002866B4
		public unsafe int Deserialize(byte* pData)
		{
			this.Id = *(short*)pData;
			byte* pCurrData = pData + 2;
			this.OrgTemplateId = *(sbyte*)pCurrData;
			pCurrData++;
			pCurrData += this.Location.Deserialize(pCurrData);
			this.Culture = *(short*)pCurrData;
			pCurrData += 2;
			this.MaxCulture = *(short*)pCurrData;
			pCurrData += 2;
			this.Safety = *(short*)pCurrData;
			pCurrData += 2;
			this.MaxSafety = *(short*)pCurrData;
			pCurrData += 2;
			this.Population = *(int*)pCurrData;
			pCurrData += 4;
			this.MaxPopulation = *(int*)pCurrData;
			pCurrData += 4;
			this.StandardOnStagePopulation = *(int*)pCurrData;
			pCurrData += 4;
			this.ApprovingRateUpperLimitBonus = *(short*)pCurrData;
			pCurrData += 2;
			this.InfluencePowerUpdateDate = *(int*)pCurrData;
			pCurrData += 4;
			this._minSeniorityId = *(short*)pCurrData;
			pCurrData += 2;
			this._taiwuExploreStatus = *pCurrData;
			pCurrData++;
			this._spiritualDebtInteractionOccurred = (*pCurrData != 0);
			pCurrData++;
			bool flag = this._taiwuInvestmentForMartialArtTournament.Length != 3;
			if (flag)
			{
				throw new Exception("Elements count of field _taiwuInvestmentForMartialArtTournament is not equal to declaration");
			}
			for (int i = 0; i < 3; i++)
			{
				this._taiwuInvestmentForMartialArtTournament[i] = *(int*)(pCurrData + (IntPtr)i * 4);
			}
			pCurrData += 12;
			pCurrData += 4;
			pCurrData += this.Members.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this.LackingCoreMembers.Deserialize(pCurrData);
			pCurrData += 4;
			ushort elementsCount = *(ushort*)pCurrData;
			pCurrData += 2;
			this._availableMonasticTitleSuffixIds.Clear();
			for (int j = 0; j < (int)elementsCount; j++)
			{
				this._availableMonasticTitleSuffixIds.Add(*(short*)(pCurrData + (IntPtr)j * 2));
			}
			pCurrData += 2 * elementsCount;
			return (int)((long)(pCurrData - pData));
		}

		// Token: 0x040014E9 RID: 5353
		[CollectionObjectField(false, true, false, false, false)]
		private short _minSeniorityId;

		// Token: 0x040014EA RID: 5354
		[CollectionObjectField(false, true, false, false, false)]
		private List<short> _availableMonasticTitleSuffixIds;

		// Token: 0x040014EB RID: 5355
		[CollectionObjectField(false, true, false, false, false)]
		private byte _taiwuExploreStatus;

		// Token: 0x040014EC RID: 5356
		[CollectionObjectField(false, true, false, false, false)]
		private bool _spiritualDebtInteractionOccurred;

		// Token: 0x040014ED RID: 5357
		[CollectionObjectField(false, true, false, false, false, ArrayElementsCount = 3)]
		private int[] _taiwuInvestmentForMartialArtTournament;

		// Token: 0x040014EE RID: 5358
		private int[] _martialArtTournamentPreparations = new int[3];

		// Token: 0x040014EF RID: 5359
		public byte PrisonEnteredStatus;

		// Token: 0x040014F0 RID: 5360
		private SortedList<long, int> _membersSortedByAuthority = new SortedList<long, int>();

		// Token: 0x040014F1 RID: 5361
		private SettlementPrison _prison;

		// Token: 0x040014F2 RID: 5362
		public const int FixedSize = 49;

		// Token: 0x040014F3 RID: 5363
		public const int DynamicCount = 3;

		// Token: 0x040014F4 RID: 5364
		private SpinLock _spinLock = new SpinLock(false);

		// Token: 0x02000A84 RID: 2692
		internal class FixedFieldInfos
		{
			// Token: 0x04002B13 RID: 11027
			public const uint Id_Offset = 0U;

			// Token: 0x04002B14 RID: 11028
			public const int Id_Size = 2;

			// Token: 0x04002B15 RID: 11029
			public const uint OrgTemplateId_Offset = 2U;

			// Token: 0x04002B16 RID: 11030
			public const int OrgTemplateId_Size = 1;

			// Token: 0x04002B17 RID: 11031
			public const uint Location_Offset = 3U;

			// Token: 0x04002B18 RID: 11032
			public const int Location_Size = 4;

			// Token: 0x04002B19 RID: 11033
			public const uint Culture_Offset = 7U;

			// Token: 0x04002B1A RID: 11034
			public const int Culture_Size = 2;

			// Token: 0x04002B1B RID: 11035
			public const uint MaxCulture_Offset = 9U;

			// Token: 0x04002B1C RID: 11036
			public const int MaxCulture_Size = 2;

			// Token: 0x04002B1D RID: 11037
			public const uint Safety_Offset = 11U;

			// Token: 0x04002B1E RID: 11038
			public const int Safety_Size = 2;

			// Token: 0x04002B1F RID: 11039
			public const uint MaxSafety_Offset = 13U;

			// Token: 0x04002B20 RID: 11040
			public const int MaxSafety_Size = 2;

			// Token: 0x04002B21 RID: 11041
			public const uint Population_Offset = 15U;

			// Token: 0x04002B22 RID: 11042
			public const int Population_Size = 4;

			// Token: 0x04002B23 RID: 11043
			public const uint MaxPopulation_Offset = 19U;

			// Token: 0x04002B24 RID: 11044
			public const int MaxPopulation_Size = 4;

			// Token: 0x04002B25 RID: 11045
			public const uint StandardOnStagePopulation_Offset = 23U;

			// Token: 0x04002B26 RID: 11046
			public const int StandardOnStagePopulation_Size = 4;

			// Token: 0x04002B27 RID: 11047
			public const uint ApprovingRateUpperLimitBonus_Offset = 27U;

			// Token: 0x04002B28 RID: 11048
			public const int ApprovingRateUpperLimitBonus_Size = 2;

			// Token: 0x04002B29 RID: 11049
			public const uint InfluencePowerUpdateDate_Offset = 29U;

			// Token: 0x04002B2A RID: 11050
			public const int InfluencePowerUpdateDate_Size = 4;

			// Token: 0x04002B2B RID: 11051
			public const uint MinSeniorityId_Offset = 33U;

			// Token: 0x04002B2C RID: 11052
			public const int MinSeniorityId_Size = 2;

			// Token: 0x04002B2D RID: 11053
			public const uint TaiwuExploreStatus_Offset = 35U;

			// Token: 0x04002B2E RID: 11054
			public const int TaiwuExploreStatus_Size = 1;

			// Token: 0x04002B2F RID: 11055
			public const uint SpiritualDebtInteractionOccurred_Offset = 36U;

			// Token: 0x04002B30 RID: 11056
			public const int SpiritualDebtInteractionOccurred_Size = 1;

			// Token: 0x04002B31 RID: 11057
			public const uint TaiwuInvestmentForMartialArtTournament_Offset = 37U;

			// Token: 0x04002B32 RID: 11058
			public const int TaiwuInvestmentForMartialArtTournament_Size = 12;
		}
	}
}
