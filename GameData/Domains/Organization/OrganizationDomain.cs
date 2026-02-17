using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Config;
using Config.ConfigCells.Character;
using GameData.ArchiveData;
using GameData.Common;
using GameData.Common.SingleValueCollection;
using GameData.Dependencies;
using GameData.DomainEvents;
using GameData.Domains.Building;
using GameData.Domains.Character;
using GameData.Domains.Character.Ai;
using GameData.Domains.Character.Ai.PrioritizedAction;
using GameData.Domains.Character.AvatarSystem;
using GameData.Domains.Character.Creation;
using GameData.Domains.Character.Display;
using GameData.Domains.Character.Filters;
using GameData.Domains.Character.Relation;
using GameData.Domains.Global.Inscription;
using GameData.Domains.Item;
using GameData.Domains.LifeRecord;
using GameData.Domains.Map;
using GameData.Domains.Organization.Display;
using GameData.Domains.Organization.SettlementPrisonRecord;
using GameData.Domains.Organization.SettlementTreasuryRecord;
using GameData.Domains.Taiwu;
using GameData.Domains.Taiwu.Profession;
using GameData.Domains.Taiwu.Profession.SkillsData;
using GameData.Domains.TaiwuEvent;
using GameData.Domains.TaiwuEvent.MonthlyEventActions;
using GameData.Domains.World;
using GameData.Domains.World.Notification;
using GameData.GameDataBridge;
using GameData.Serializer;
using GameData.Utilities;
using NLog;
using Redzen.Random;

namespace GameData.Domains.Organization
{
	// Token: 0x02000644 RID: 1604
	[GameDataDomain(3)]
	public class OrganizationDomain : BaseGameDataDomain
	{
		// Token: 0x06004684 RID: 18052 RVA: 0x002755D8 File Offset: 0x002737D8
		private void OnInitializedDomainData()
		{
			this.InitializeSettlementTreasury();
		}

		// Token: 0x06004685 RID: 18053 RVA: 0x002755E2 File Offset: 0x002737E2
		private void InitializeOnInitializeGameDataModule()
		{
			OrganizationDomain.InitializeSectOrgTemplateIds();
		}

		// Token: 0x06004686 RID: 18054 RVA: 0x002755EB File Offset: 0x002737EB
		private void InitializeOnEnterNewWorld()
		{
			this.InitializeSettlementsCache();
			this.InitializeSettlementCharactersCache();
			OrganizationDomain._orgInscribedCharIdMap = new Dictionary<sbyte, List<InscribedCharacter>>();
		}

		// Token: 0x06004687 RID: 18055 RVA: 0x00275608 File Offset: 0x00273808
		private void OnLoadedArchiveData()
		{
			this.InitializeSettlementsCache();
			this.InitializeSettlementCharactersCache();
			DataUid dataUid = new DataUid(0, 1, ulong.MaxValue, uint.MaxValue);
			GameDataBridge.AddPostDataModificationHandler(dataUid, "InitializeSortedMembersCache", new Action<DataContext, DataUid>(this.InitializeSortedMembersCache));
		}

		// Token: 0x06004688 RID: 18056 RVA: 0x00275648 File Offset: 0x00273848
		public override void OnCurrWorldArchiveDataReady(DataContext context, bool isNewWorld)
		{
			this.InitializePrisonCache();
		}

		// Token: 0x06004689 RID: 18057 RVA: 0x00275654 File Offset: 0x00273854
		public override void FixAbnormalDomainArchiveData(DataContext context)
		{
			Settlement taiwuVillage = this.GetSettlementByOrgTemplateId(16);
			bool flag = taiwuVillage.GetMaxCulture() < 25;
			if (flag)
			{
				Logger logger = OrganizationDomain.Logger;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(42, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Fixing taiwu village's max culture ");
				defaultInterpolatedStringHandler.AppendFormatted<short>(taiwuVillage.GetMaxCulture());
				defaultInterpolatedStringHandler.AppendLiteral(" => 25.");
				logger.Warn(defaultInterpolatedStringHandler.ToStringAndClear());
				taiwuVillage.SetMaxCulture(25, context);
			}
			bool initTreasury = DomainManager.World.IsCurrWorldBeforeVersion(0, 0, 71, 26);
			bool flag2 = initTreasury;
			if (flag2)
			{
				OrganizationDomain.Logger.Warn("Initializing settlement treasuries.");
			}
			foreach (Settlement settlement in this._settlements.Values)
			{
				this.FixAbnormalSettlementMembers(context, settlement);
				bool flag3 = initTreasury;
				if (flag3)
				{
					this.FixComplementSettlementTreasuryBuilding(context, settlement);
				}
			}
			bool initPrison = DomainManager.World.IsCurrWorldBeforeVersion(0, 0, 73, 27);
			bool flag4 = initPrison;
			if (flag4)
			{
				OrganizationDomain.Logger.Warn("Initializing settlement prisons");
			}
			foreach (Sect sect in this._sects.Values)
			{
				bool flag5 = initPrison;
				if (flag5)
				{
					this.FixComplementSectPrisonBuilding(context, sect);
				}
				this.FixInvalidSectPrisoner(context, sect);
				GameData.Domains.Character.Character leader = sect.GetLeader();
				bool flag6 = leader == null;
				if (!flag6)
				{
					bool flag7 = leader.AddFeature(context, 405, false);
					if (flag7)
					{
						Logger logger2 = OrganizationDomain.Logger;
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(31, 1);
						defaultInterpolatedStringHandler.AppendLiteral("Adding sect leader feature to ");
						defaultInterpolatedStringHandler.AppendFormatted<GameData.Domains.Character.Character>(leader);
						defaultInterpolatedStringHandler.AppendLiteral(".");
						logger2.Info(defaultInterpolatedStringHandler.ToStringAndClear());
					}
				}
			}
			bool fixTreasuryHobbies = DomainManager.World.IsCurrWorldBeforeVersion(0, 0, 79, 21);
			bool flag8 = fixTreasuryHobbies;
			if (flag8)
			{
				OrganizationDomain.Logger.Warn("Fixing settlement treasury hobbies.");
				foreach (Settlement settlement2 in this._settlements.Values)
				{
					SettlementTreasury[] treasuries = settlement2.Treasuries.SettlementTreasuries;
					int i = treasuries.Length - 1;
					while (i-- > 0)
					{
						treasuries[i].LovingItemSubTypes = treasuries[treasuries.Length - 1].LovingItemSubTypes.ToList<short>();
						treasuries[i].HatingItemSubTypes = treasuries[treasuries.Length - 1].HatingItemSubTypes.ToList<short>();
					}
					DomainManager.Extra.SetTreasuries(context, settlement2.GetId(), settlement2.Treasuries, false);
				}
			}
		}

		// Token: 0x0600468A RID: 18058 RVA: 0x0027594C File Offset: 0x00273B4C
		private void FixComplementSettlementTreasuryBuilding(DataContext context, Settlement settlement)
		{
			bool flag = settlement.GetLocation().AreaId >= 45;
			if (!flag)
			{
				Location location = settlement.GetLocation();
				sbyte orgTemplateId = settlement.GetOrgTemplateId();
				if (!true)
				{
				}
				short num;
				if (orgTemplateId >= 21)
				{
					if (orgTemplateId <= 35)
					{
						num = 284;
						goto IL_91;
					}
					switch (orgTemplateId)
					{
					case 36:
						num = 286;
						goto IL_91;
					case 37:
						num = 285;
						goto IL_91;
					case 38:
						num = 287;
						goto IL_91;
					}
				}
				else if (orgTemplateId >= 1)
				{
					if (orgTemplateId <= 15)
					{
						num = (short)((int)(orgTemplateId - 1) + 288);
						goto IL_91;
					}
				}
				num = -1;
				IL_91:
				if (!true)
				{
				}
				short buildingTemplateId = num;
				bool flag2 = buildingTemplateId < 0;
				if (!flag2)
				{
					DomainManager.Building.PlaceBuildingAtBlock(context, location.AreaId, location.BlockId, buildingTemplateId, true, false);
				}
			}
		}

		// Token: 0x0600468B RID: 18059 RVA: 0x00275A18 File Offset: 0x00273C18
		private void FixComplementSectPrisonBuilding(DataContext context, Sect sect)
		{
			Location location = sect.GetLocation();
			sbyte orgTemplateId = sect.GetOrgTemplateId();
			sbyte sectIndex = OrganizationDomain.GetLargeSectIndex(orgTemplateId);
			bool flag = sectIndex < 0;
			if (!flag)
			{
				short buildingTemplateId = (short)((int)sectIndex + 303);
				DomainManager.Building.PlaceBuildingAtBlock(context, location.AreaId, location.BlockId, buildingTemplateId, true, false);
			}
		}

		// Token: 0x0600468C RID: 18060 RVA: 0x00275A6C File Offset: 0x00273C6C
		private void FixInvalidSectPrisoner(DataContext context, Sect sect)
		{
			SettlementPrison prison = sect.Prison;
			bool isChanged = false;
			for (int i = prison.Prisoners.Count - 1; i >= 0; i--)
			{
				SettlementPrisoner prisoner = prison.Prisoners[i];
				int charId = prisoner.CharId;
				GameData.Domains.Character.Character character;
				bool flag = !DomainManager.Character.TryGetElement_Objects(charId, out character);
				if (flag)
				{
					prison.Prisoners.RemoveAt(i);
					isChanged = true;
					Logger logger = OrganizationDomain.Logger;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(41, 2);
					defaultInterpolatedStringHandler.AppendLiteral("Fixing dead character ");
					defaultInterpolatedStringHandler.AppendFormatted<int>(charId);
					defaultInterpolatedStringHandler.AppendLiteral(" in sect ");
					defaultInterpolatedStringHandler.AppendFormatted<Sect>(sect);
					defaultInterpolatedStringHandler.AppendLiteral("'s prison.");
					logger.Warn(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				else
				{
					bool flag2 = character.IsActiveExternalRelationState(8);
					if (flag2)
					{
						prison.Prisoners.RemoveAt(i);
						DomainManager.Organization.UnregisterSectPrisoner(prisoner.CharId);
						character.SetLocation(sect.GetLocation(), context);
						character.DeactivateExternalRelationState(context, 32);
						isChanged = true;
						Logger logger2 = OrganizationDomain.Logger;
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(71, 2);
						defaultInterpolatedStringHandler.AppendLiteral("Fixing infected character ");
						defaultInterpolatedStringHandler.AppendFormatted<GameData.Domains.Character.Character>(character);
						defaultInterpolatedStringHandler.AppendLiteral(" exist in both stone room and sect ");
						defaultInterpolatedStringHandler.AppendFormatted<Sect>(sect);
						defaultInterpolatedStringHandler.AppendLiteral("'s prison.");
						logger2.Warn(defaultInterpolatedStringHandler.ToStringAndClear());
					}
				}
			}
			for (int j = prison.Bounties.Count - 1; j >= 0; j--)
			{
				SettlementBounty bounty = prison.Bounties[j];
				bool flag3 = bounty.PunishmentType < 0;
				if (flag3)
				{
					short predefinedLogId = 33;
					GameData.Domains.Character.Character ch;
					object arg = DomainManager.Character.TryGetElement_Objects(bounty.CharId, out ch) ? ch.ToString() : bounty.CharId.ToString();
					PunishmentSeverityItem item = PunishmentSeverity.Instance.GetItem(bounty.PunishmentSeverity);
					PredefinedLog.Show(predefinedLogId, arg, (item != null) ? item.Name : null);
					prison.Bounties.RemoveAt(j);
				}
			}
			bool flag4 = isChanged;
			if (flag4)
			{
				DomainManager.Extra.SetSettlementPrison(context, sect.GetId(), prison);
			}
		}

		// Token: 0x0600468D RID: 18061 RVA: 0x00275CAC File Offset: 0x00273EAC
		private BuildingBlockData GetAvailableBlockInSettlementBuildingArea(Location location)
		{
			BuildingAreaData buildingAreaData = DomainManager.Building.GetElement_BuildingAreas(location);
			int blockCount = (int)(buildingAreaData.Width * buildingAreaData.Width);
			ValueTuple<int, int> centerPos = new ValueTuple<int, int>((int)(buildingAreaData.Width / 2), (int)(buildingAreaData.Width / 2));
			BuildingBlockData bestBlock = null;
			int bestBlockPriority = int.MinValue;
			int bestDistance = int.MaxValue;
			short index = 0;
			while ((int)index < blockCount)
			{
				BuildingBlockKey blockKey = new BuildingBlockKey(location.AreaId, location.BlockId, index);
				BuildingBlockData block;
				bool flag = !DomainManager.Building.TryGetElement_BuildingBlocks(blockKey, out block);
				if (!flag)
				{
					bool flag2 = block.RootBlockIndex >= 0;
					if (!flag2)
					{
						BuildingBlockItem configData = block.ConfigData;
						EBuildingBlockType type = configData.Type;
						bool flag3 = type - EBuildingBlockType.Building <= 1;
						bool flag4 = flag3;
						if (!flag4)
						{
							ValueTuple<int, int> pos = buildingAreaData.GetBlockPos(index);
							EBuildingBlockType type2 = configData.Type;
							if (!true)
							{
							}
							int num;
							switch (type2)
							{
							case EBuildingBlockType.NormalResource:
								num = 1;
								break;
							case EBuildingBlockType.SpecialResource:
								num = 0;
								break;
							case EBuildingBlockType.UselessResource:
								num = 2;
								break;
							case EBuildingBlockType.Building:
							case EBuildingBlockType.MainBuilding:
								goto IL_10F;
							case EBuildingBlockType.Empty:
								num = 3;
								break;
							default:
								goto IL_10F;
							}
							IL_118:
							if (!true)
							{
							}
							int priority = num;
							int distance = MathUtils.GetManhattanDistance(centerPos.Item1, centerPos.Item2, pos.Item1, pos.Item2, 1);
							bool flag5 = bestBlock == null || bestBlockPriority > priority || (bestBlockPriority == priority && distance < bestDistance);
							if (flag5)
							{
								bestBlock = block;
								bestBlockPriority = priority;
								bestDistance = distance;
							}
							goto IL_171;
							IL_10F:
							num = int.MinValue;
							goto IL_118;
						}
					}
				}
				IL_171:
				index += 1;
			}
			return bestBlock;
		}

		// Token: 0x0600468E RID: 18062 RVA: 0x00275E48 File Offset: 0x00274048
		private void FixAbnormalSettlementMembers(DataContext context, Settlement settlement)
		{
			short settlementId = settlement.GetId();
			sbyte orgTemplateId = settlement.GetOrgTemplateId();
			OrgMemberCollection memberCollection = DomainManager.Organization.GetSettlement(settlementId).GetMembers();
			List<ValueTuple<int, sbyte>> wrongSettlementCharIds = new List<ValueTuple<int, sbyte>>();
			for (sbyte grade = 0; grade < 9; grade += 1)
			{
				HashSet<int> gradeMembers = memberCollection.GetMembers(grade);
				foreach (int charId in gradeMembers)
				{
					GameData.Domains.Character.Character character;
					bool flag = !DomainManager.Character.TryGetElement_Objects(charId, out character);
					if (!flag)
					{
						OrganizationInfo orgInfo = character.GetOrganizationInfo();
						bool flag2 = orgInfo.OrgTemplateId == 20;
						if (flag2)
						{
							wrongSettlementCharIds.Add(new ValueTuple<int, sbyte>(charId, grade));
						}
					}
				}
			}
			int i = 0;
			int max = wrongSettlementCharIds.Count;
			while (i < max)
			{
				ValueTuple<int, sbyte> valueTuple = wrongSettlementCharIds[i];
				int charId2 = valueTuple.Item1;
				sbyte grade2 = valueTuple.Item2;
				GameData.Domains.Character.Character character2 = DomainManager.Character.GetElement_Objects(charId2);
				OrganizationInfo orgInfo2 = character2.GetOrganizationInfo();
				orgInfo2.OrgTemplateId = orgTemplateId;
				orgInfo2.Grade = grade2;
				orgInfo2.SettlementId = settlementId;
				character2.SetOrganizationInfo(orgInfo2, context);
				Events.RaiseXiangshuInfectionFeatureChanged(context, character2, 218);
				string orgName = Organization.Instance[orgInfo2.OrgTemplateId].Name;
				string gradeName = OrganizationDomain.GetOrgMemberConfig(orgInfo2).GradeName;
				Logger logger = OrganizationDomain.Logger;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(66, 3);
				defaultInterpolatedStringHandler.AppendLiteral("Removing infected character ");
				defaultInterpolatedStringHandler.AppendFormatted<GameData.Domains.Character.Character>(character2);
				defaultInterpolatedStringHandler.AppendLiteral(" from his/her original settlement as ");
				defaultInterpolatedStringHandler.AppendFormatted(orgName);
				defaultInterpolatedStringHandler.AppendFormatted(gradeName);
				defaultInterpolatedStringHandler.AppendLiteral(".");
				logger.Warn(defaultInterpolatedStringHandler.ToStringAndClear());
				i++;
			}
		}

		// Token: 0x0600468F RID: 18063 RVA: 0x00276038 File Offset: 0x00274238
		[SingleValueDependency(12, new ushort[]
		{
			1
		})]
		[SingleValueDependency(3, new ushort[]
		{
			8
		})]
		[ObjectCollectionDependency(3, 0, new ushort[]
		{
			18
		})]
		private unsafe void CalcMartialArtTournamentPreparationInfoList(List<MartialArtTournamentPreparationInfo> value)
		{
			value.Clear();
			MonthlyActionKey actionKey = new MonthlyActionKey(2, 0);
			MonthlyActionBase monthlyAction = DomainManager.TaiwuEvent.GetMonthlyAction(actionKey);
			bool flag = monthlyAction == null || monthlyAction.State != 1;
			if (!flag)
			{
				long* sortedByCombatPower;
				long* sortedByAuthority;
				long* sortedByResource;
				int count;
				checked
				{
					sortedByCombatPower = stackalloc long[unchecked((UIntPtr)this._sects.Count) * 8];
					sortedByAuthority = stackalloc long[unchecked((UIntPtr)this._sects.Count) * 8];
					sortedByResource = stackalloc long[unchecked((UIntPtr)this._sects.Count) * 8];
					count = 0;
				}
				int indexThreshold = Math.Max(0, this._previousMartialArtTournamentHosts.Count - 3);
				foreach (Sect sect in this._sects.Values)
				{
					short settlementId2 = sect.GetId();
					int lastIndex = this._previousMartialArtTournamentHosts.LastIndexOf(settlementId2);
					bool flag2 = lastIndex >= indexThreshold;
					if (!flag2)
					{
						int[] taiwuInvestment = sect.GetTaiwuInvestmentForMartialArtTournament();
						int[] preparations = sect.GetMartialArtTournamentPreparations();
						int totalCombatPowerPreparation = preparations[0] + taiwuInvestment[0];
						int totalAuthorityPreparation = preparations[1] + taiwuInvestment[1];
						int totalResourcePreparation = preparations[2] + taiwuInvestment[2];
						sortedByCombatPower[count] = ((long)totalCombatPowerPreparation << 32) + (long)settlementId2;
						sortedByAuthority[count] = ((long)totalAuthorityPreparation << 32) + (long)settlementId2;
						sortedByResource[count] = ((long)totalResourcePreparation << 32) + (long)settlementId2;
						value.Add(new MartialArtTournamentPreparationInfo
						{
							SettlementId = settlementId2,
							CombatPowerPreparation = totalCombatPowerPreparation,
							AuthorityPreparation = totalAuthorityPreparation,
							ResourcePreparation = totalResourcePreparation,
							TotalScore = 0
						});
						count++;
					}
				}
				CollectionUtils.Sort(sortedByCombatPower, count);
				CollectionUtils.Sort(sortedByAuthority, count);
				CollectionUtils.Sort(sortedByResource, count);
				int num = count;
				Span<long> span = new Span<long>(stackalloc byte[checked(unchecked((UIntPtr)num) * 8)], num);
				Span<long> scores = span;
				scores.Fill(0L);
				for (int i = 0; i < count; i++)
				{
					int rankIndex = count - i - 1;
					short settlementId3 = (short)(sortedByCombatPower[rankIndex] & 65535L);
					int score = OrganizationDomain.<CalcMartialArtTournamentPreparationInfoList>g__GetScore|21_0(sortedByCombatPower, count, rankIndex);
					int index = value.FindIndex((MartialArtTournamentPreparationInfo info) => info.SettlementId == settlementId3);
					MartialArtTournamentPreparationInfo info4 = value[index];
					info4.TotalScore += score;
					value[index] = info4;
					short settlementId4 = (short)(sortedByAuthority[rankIndex] & 65535L);
					int score2 = OrganizationDomain.<CalcMartialArtTournamentPreparationInfoList>g__GetScore|21_0(sortedByAuthority, count, rankIndex);
					int index2 = value.FindIndex((MartialArtTournamentPreparationInfo info) => info.SettlementId == settlementId4);
					MartialArtTournamentPreparationInfo info2 = value[index2];
					info2.TotalScore += score2;
					value[index2] = info2;
					short settlementId = (short)(sortedByResource[rankIndex] & 65535L);
					int score3 = OrganizationDomain.<CalcMartialArtTournamentPreparationInfoList>g__GetScore|21_0(sortedByResource, count, rankIndex);
					int index3 = value.FindIndex((MartialArtTournamentPreparationInfo info) => info.SettlementId == settlementId);
					MartialArtTournamentPreparationInfo info3 = value[index3];
					info3.TotalScore += score3;
					value[index3] = info3;
				}
				value.Sort();
			}
		}

		// Token: 0x06004690 RID: 18064 RVA: 0x00276390 File Offset: 0x00274590
		public void MakeNoneOrgCharactersBecomeBeggar(DataContext context)
		{
			List<GameData.Domains.Character.Character> noneOrgCharacters = new List<GameData.Domains.Character.Character>();
			MapCharacterFilter.ParallelFind((GameData.Domains.Character.Character character) => character.GetOrganizationInfo().OrgTemplateId == 0 && character.IsInteractableAsIntelligentCharacter() && DomainManager.Organization.GetFugitiveBountySect(character.GetId()) < 0 && DomainManager.Organization.GetPrisonerSect(character.GetId()) < 0, noneOrgCharacters, 0, 135, false);
			foreach (GameData.Domains.Character.Character character2 in noneOrgCharacters)
			{
				this.JoinNearbyVillageTownAsBeggar(context, character2, -1);
			}
		}

		// Token: 0x06004691 RID: 18065 RVA: 0x00276418 File Offset: 0x00274618
		public void UpdateApprovingRateEffectOnAdvanceMonth(DataContext context)
		{
			GameData.Domains.Character.Character taiwuChar = DomainManager.Taiwu.GetTaiwu();
			int goodSectTotalApprovingRate = 0;
			int evilSectTotalApprovingRate = 0;
			int neutralTotalApprovingRate = 0;
			foreach (KeyValuePair<short, Sect> keyValuePair in this._sects)
			{
				short num;
				Sect sect2;
				keyValuePair.Deconstruct(out num, out sect2);
				Sect sect = sect2;
				short approvingRate = sect.CalcApprovingRate();
				bool flag = approvingRate >= 200;
				if (flag)
				{
					switch (Organization.Instance[sect.GetOrgTemplateId()].Goodness)
					{
					case -1:
						evilSectTotalApprovingRate += (int)approvingRate;
						break;
					case 0:
						neutralTotalApprovingRate += (int)approvingRate;
						break;
					case 1:
						goodSectTotalApprovingRate += (int)approvingRate;
						break;
					}
				}
			}
			bool flag2 = evilSectTotalApprovingRate >= 100;
			if (flag2)
			{
				taiwuChar.RecordFameAction(context, 72, -1, (short)(evilSectTotalApprovingRate / 100), true);
			}
			bool flag3 = goodSectTotalApprovingRate >= 100;
			if (flag3)
			{
				taiwuChar.RecordFameAction(context, 71, -1, (short)(goodSectTotalApprovingRate / 100), true);
			}
			bool flag4 = neutralTotalApprovingRate >= 100;
			if (flag4)
			{
				taiwuChar.RecordFameAction(context, 73, -1, (short)(neutralTotalApprovingRate / 100), true);
				taiwuChar.RecordFameAction(context, 74, -1, (short)(neutralTotalApprovingRate / 100), true);
			}
			taiwuChar.ChangeResource(context, 7, this.CalcApprovingRateEffectAuthorityGain());
		}

		// Token: 0x06004692 RID: 18066 RVA: 0x00276584 File Offset: 0x00274784
		[DomainMethod]
		public int CalcApprovingRateEffectAuthorityGain()
		{
			int totalAuthorityGain = 0;
			foreach (KeyValuePair<short, Sect> keyValuePair in this._sects)
			{
				short num;
				Sect sect2;
				keyValuePair.Deconstruct(out num, out sect2);
				Sect sect = sect2;
				short approvingRate = sect.CalcApprovingRate();
				bool flag = approvingRate >= 300;
				if (flag)
				{
					totalAuthorityGain += (int)approvingRate;
				}
			}
			return totalAuthorityGain / 10;
		}

		// Token: 0x06004693 RID: 18067 RVA: 0x00276614 File Offset: 0x00274814
		public Settlement GetSettlement(short settlementId)
		{
			return this._settlements[settlementId];
		}

		// Token: 0x06004694 RID: 18068 RVA: 0x00276634 File Offset: 0x00274834
		public Settlement GetSettlementByOrgTemplateId(sbyte orgTemplateId)
		{
			List<Settlement> settlements = this._orgTemplateId2Settlements[(int)orgTemplateId];
			bool flag = settlements == null;
			bool flag2 = flag;
			if (!flag2)
			{
				int count = settlements.Count;
				bool flag3 = count > 1 || count == 0;
				flag2 = flag3;
			}
			bool flag4 = flag2;
			Settlement result;
			if (flag4)
			{
				result = null;
			}
			else
			{
				result = settlements[0];
			}
			return result;
		}

		// Token: 0x06004695 RID: 18069 RVA: 0x0027668C File Offset: 0x0027488C
		public Settlement GetSettlementByLocation(Location location)
		{
			Settlement settlement;
			bool flag = this._locationSettlements.TryGetValue(location, out settlement);
			Settlement result;
			if (flag)
			{
				result = settlement;
			}
			else
			{
				result = null;
			}
			return result;
		}

		// Token: 0x06004696 RID: 18070 RVA: 0x002766B5 File Offset: 0x002748B5
		public void GetAllSettlements(List<Settlement> settlements)
		{
			settlements.Clear();
			settlements.AddRange(this._sects.Values);
			settlements.AddRange(this._civilianSettlements.Values);
		}

		// Token: 0x06004697 RID: 18071 RVA: 0x002766E3 File Offset: 0x002748E3
		public void GetAllCivilianSettlements(List<Settlement> settlements)
		{
			settlements.Clear();
			settlements.AddRange(this._civilianSettlements.Values);
		}

		// Token: 0x06004698 RID: 18072 RVA: 0x00276700 File Offset: 0x00274900
		public short GetSettlementIdByOrgTemplateId(sbyte orgTemplateId)
		{
			Settlement settlement = this.GetSettlementByOrgTemplateId(orgTemplateId);
			return (settlement != null) ? settlement.GetId() : -1;
		}

		// Token: 0x06004699 RID: 18073 RVA: 0x00276728 File Offset: 0x00274928
		public SettlementCharacter GetSettlementCharacter(int charId)
		{
			return this._settlementCharacters[charId];
		}

		// Token: 0x0600469A RID: 18074 RVA: 0x00276748 File Offset: 0x00274948
		public bool TryGetSettlementCharacter(int charId, out SettlementCharacter settlementChar)
		{
			return this._settlementCharacters.TryGetValue(charId, out settlementChar);
		}

		// Token: 0x0600469B RID: 18075 RVA: 0x00276768 File Offset: 0x00274968
		public bool IsInAnySect(int charId)
		{
			return this._sectCharacters.ContainsKey(charId);
		}

		// Token: 0x0600469C RID: 18076 RVA: 0x00276788 File Offset: 0x00274988
		public bool IsInAnyCivilianSettlement(int charId)
		{
			return this._civilianSettlementCharacters.ContainsKey(charId);
		}

		// Token: 0x0600469D RID: 18077 RVA: 0x002767A8 File Offset: 0x002749A8
		public void JoinSect(DataContext context, GameData.Domains.Character.Character character, OrganizationInfo destOrgInfo)
		{
			this.LeaveOrganization(context, character, false);
			this.JoinOrganization(context, character, destOrgInfo);
			OrganizationInfo srcOrgInfo = character.GetOrganizationInfo();
			character.SetOrganizationInfo(destOrgInfo, context);
			LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
			int currDate = DomainManager.World.GetCurrDate();
			int selfCharId = character.GetId();
			Location currLocation = character.GetLocation();
			sbyte gender = character.GetGender();
			lifeRecordCollection.AddJoinSectSucceed(selfCharId, currDate, currLocation, destOrgInfo.SettlementId, destOrgInfo.OrgTemplateId, destOrgInfo.Grade, true, gender);
			Events.RaiseCharacterOrganizationChanged(context, character, srcOrgInfo, destOrgInfo);
		}

		// Token: 0x0600469E RID: 18078 RVA: 0x00276834 File Offset: 0x00274A34
		public void JoinOrganization(DataContext context, GameData.Domains.Character.Character character, OrganizationInfo destOrgInfo)
		{
			bool flag = destOrgInfo.SettlementId < 0;
			if (!flag)
			{
				int charId = character.GetId();
				bool flag2 = OrganizationDomain.IsSect(destOrgInfo.OrgTemplateId);
				SettlementCharacter settlementCharacter;
				OrgMemberCollection members;
				if (flag2)
				{
					SectCharacter sectChar = new SectCharacter(charId, destOrgInfo.OrgTemplateId, destOrgInfo.SettlementId);
					this.AddElement_SectCharacters(charId, sectChar);
					settlementCharacter = sectChar;
					Sect sect = this._sects[destOrgInfo.SettlementId];
					members = sect.GetMembers();
					this.CreateRelationWithAllSettlementMembers(context, character, members);
					members.Add(charId, destOrgInfo.Grade);
					sect.SetMembers(members, context);
					this.TryAddSectMemberFeature(context, character, destOrgInfo);
					bool flag3 = destOrgInfo.Grade == 8 && destOrgInfo.Principal;
					if (flag3)
					{
						character.AddFeature(context, 405, false);
					}
					OrganizationMemberItem orgMemberConfig = OrganizationDomain.GetOrgMemberConfig(destOrgInfo);
					short mentorSeniorityId = this.SetRandomSectMentor(context, charId, destOrgInfo, members, orgMemberConfig.TeacherGrade);
					OrganizationDomain.TryBecomeSectMonk(context, character, sect, orgMemberConfig, mentorSeniorityId);
				}
				else
				{
					CivilianSettlementCharacter civilianSettlementChar = new CivilianSettlementCharacter(charId, destOrgInfo.OrgTemplateId, destOrgInfo.SettlementId);
					this.AddElement_CivilianSettlementCharacters(charId, civilianSettlementChar);
					settlementCharacter = civilianSettlementChar;
					CivilianSettlement civilianSettlement = this._civilianSettlements[destOrgInfo.SettlementId];
					members = civilianSettlement.GetMembers();
					this.CreateRelationWithAllSettlementMembers(context, character, members);
					members.Add(charId, destOrgInfo.Grade);
					civilianSettlement.SetMembers(members, context);
					bool flag4 = destOrgInfo.OrgTemplateId == 16;
					if (flag4)
					{
						bool flag5 = !DomainManager.Taiwu.IsInGroup(charId);
						if (flag5)
						{
							DomainManager.Taiwu.AddTaiwuVillageResident(context, charId);
						}
						bool flag6 = DomainManager.Extra.TryTriggerAddSeniorityPoint(context, 65, charId);
						if (flag6)
						{
							int sum = character.GetMaxMainAttributes().GetSum() + character.GetCombatSkillQualifications().GetSum() + character.GetLifeSkillQualifications().GetSum();
							int delta = ProfessionFormulaImpl.Calculate(66, sum);
							DomainManager.Extra.ChangeProfessionSeniority(context, 10, delta, true, false);
						}
					}
				}
				this._settlementCharacters.Add(charId, settlementCharacter);
				bool principal = destOrgInfo.Principal;
				if (principal)
				{
					OrganizationDomain.CheckPrincipalMembersAmount(destOrgInfo.OrgTemplateId, destOrgInfo.Grade, members);
				}
				character.ChangeMerchantType(context, destOrgInfo);
			}
		}

		// Token: 0x0600469F RID: 18079 RVA: 0x00276A54 File Offset: 0x00274C54
		public void LeaveOrganization(DataContext context, GameData.Domains.Character.Character character, bool charIsDead)
		{
			OrganizationInfo orgInfo = character.GetOrganizationInfo();
			bool flag = orgInfo.SettlementId < 0;
			if (!flag)
			{
				int charId = character.GetId();
				OrganizationItem orgConfig = Organization.Instance[orgInfo.OrgTemplateId];
				bool flag2 = OrganizationDomain.IsSect(orgInfo.OrgTemplateId);
				if (flag2)
				{
					Sect sect = this._sects[orgInfo.SettlementId];
					bool flag3 = orgConfig.Hereditary && orgInfo.Principal && orgInfo.Grade > 0;
					if (flag3)
					{
						OrgMemberCollection lackingMembers = sect.GetLackingCoreMembers();
						lackingMembers.Add(charId, orgInfo.Grade);
						sect.SetLackingCoreMembers(lackingMembers, context);
					}
					OrgMemberCollection members = sect.GetMembers();
					members.Remove(charId, orgInfo.Grade);
					sect.SetMembers(members, context);
					this.RemoveElement_SectCharacters(charId);
					bool flag4 = !charIsDead && orgInfo.Grade == 8 && orgInfo.Principal;
					if (flag4)
					{
						character.RemoveFeature(context, 405);
					}
					int factionId = character.GetFactionId();
					bool flag5 = factionId == charId;
					if (flag5)
					{
						this.RemoveFaction(context, character, charIsDead);
					}
					else
					{
						bool flag6 = factionId >= 0;
						if (flag6)
						{
							this.LeaveFaction(context, character, charIsDead);
						}
					}
					bool flag7 = !charIsDead;
					if (flag7)
					{
						OrganizationDomain.TrySecularize(context, character);
					}
					SettlementLayeredTreasuries treasuries = sect.Treasuries;
					sbyte b;
					bool isModified = treasuries.TryRemoveGuard(charId, out b);
					bool flag8 = isModified;
					if (flag8)
					{
						bool flag9 = !charIsDead;
						if (flag9)
						{
							character.RemoveFeatureGroup(context, 536);
						}
						Logger logger = OrganizationDomain.Logger;
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(28, 2);
						defaultInterpolatedStringHandler.AppendFormatted<GameData.Domains.Character.Character>(character);
						defaultInterpolatedStringHandler.AppendLiteral(" is no longer guarding sect ");
						defaultInterpolatedStringHandler.AppendFormatted(sect.GetNameRelatedData().GetName());
						logger.Info(defaultInterpolatedStringHandler.ToStringAndClear());
						DomainManager.Extra.SetTreasuries(context, sect.GetId(), treasuries, false);
					}
				}
				else
				{
					CivilianSettlement civilianSettlement = this._civilianSettlements[orgInfo.SettlementId];
					bool flag10 = orgConfig.Hereditary && orgInfo.Principal && orgInfo.Grade > 0;
					if (flag10)
					{
						OrgMemberCollection lackingMembers2 = civilianSettlement.GetLackingCoreMembers();
						lackingMembers2.Add(charId, orgInfo.Grade);
						civilianSettlement.SetLackingCoreMembers(lackingMembers2, context);
					}
					OrgMemberCollection members2 = civilianSettlement.GetMembers();
					members2.Remove(charId, orgInfo.Grade);
					civilianSettlement.SetMembers(members2, context);
					this.RemoveElement_CivilianSettlementCharacters(charId);
					bool flag11 = orgInfo.OrgTemplateId == 16;
					if (flag11)
					{
						if (charIsDead)
						{
							bool flag12 = DomainManager.Extra.GetVillagerRole(charId) != null;
							if (flag12)
							{
								DomainManager.World.GetMonthlyNotificationCollection().AddTaiwuVillagerDied(charId, orgInfo.OrgTemplateId, orgInfo.Grade, orgInfo.Principal, character.GetGender(), character.GetLocation());
							}
						}
						else
						{
							character.RemoveFeatureGroup(context, 734);
						}
						DomainManager.Taiwu.TryRemoveTaiwuVillageResident(context, charId);
						DomainManager.Extra.UnregisterVillagerRole(context, charId);
					}
				}
				this._settlementCharacters.Remove(charId);
				this.TryDowngradeDeputySpouses(context, charId, orgInfo);
			}
		}

		// Token: 0x060046A0 RID: 18080 RVA: 0x00276D68 File Offset: 0x00274F68
		public void ChangeOrganization(DataContext context, GameData.Domains.Character.Character character, OrganizationInfo destOrgInfo)
		{
			this.LeaveOrganization(context, character, false);
			this.JoinOrganization(context, character, destOrgInfo);
			OrganizationInfo srcOrgInfo = character.GetOrganizationInfo();
			character.SetOrganizationInfo(destOrgInfo, context);
			bool flag = srcOrgInfo.OrgTemplateId != 20 && destOrgInfo.OrgTemplateId != 20 && srcOrgInfo.OrgTemplateId != destOrgInfo.OrgTemplateId;
			if (flag)
			{
				LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
				int selfCharId = character.GetId();
				int currDate = DomainManager.World.GetCurrDate();
				sbyte gender = character.GetGender();
				bool flag2 = srcOrgInfo.OrgTemplateId == 0;
				if (flag2)
				{
					lifeRecordCollection.AddJoinOrganization(selfCharId, currDate, destOrgInfo.SettlementId, destOrgInfo.OrgTemplateId, destOrgInfo.Grade, destOrgInfo.Principal, gender);
				}
				else
				{
					bool flag3 = destOrgInfo.OrgTemplateId == 0;
					if (flag3)
					{
						lifeRecordCollection.AddBreakAwayOrganization(selfCharId, currDate, srcOrgInfo.SettlementId);
					}
					else
					{
						lifeRecordCollection.AddChangeOrganization(selfCharId, currDate, srcOrgInfo.SettlementId, destOrgInfo.SettlementId, destOrgInfo.OrgTemplateId, destOrgInfo.Grade, destOrgInfo.Principal, gender);
					}
				}
			}
			Events.RaiseCharacterOrganizationChanged(context, character, srcOrgInfo, destOrgInfo);
		}

		// Token: 0x060046A1 RID: 18081 RVA: 0x00276E7C File Offset: 0x0027507C
		public void ChangeGrade(DataContext context, GameData.Domains.Character.Character character, sbyte destGrade, bool destPrincipal)
		{
			int charId = character.GetId();
			OrganizationInfo oriOrgInfo = character.GetOrganizationInfo();
			OrganizationInfo destOrgInfo = new OrganizationInfo(oriOrgInfo.OrgTemplateId, destGrade, destPrincipal, oriOrgInfo.SettlementId);
			character.SetOrganizationInfo(destOrgInfo, context);
			bool flag = destGrade > oriOrgInfo.Grade || (destGrade == oriOrgInfo.Grade && destPrincipal && !oriOrgInfo.Principal);
			if (flag)
			{
				LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
				Location location = character.GetLocation();
				int currDate = DomainManager.World.GetCurrDate();
				sbyte gender = character.GetGender();
				lifeRecordCollection.AddChangeGrade(charId, currDate, location, destOrgInfo.OrgTemplateId, destGrade, destPrincipal, gender);
			}
			else
			{
				bool flag2 = destGrade < oriOrgInfo.Grade || (destGrade == oriOrgInfo.Grade && destPrincipal && !oriOrgInfo.Principal);
				if (flag2)
				{
					LifeRecordCollection lifeRecordCollection2 = DomainManager.LifeRecord.GetLifeRecordCollection();
					Location location2 = character.GetLocation();
					int currDate2 = DomainManager.World.GetCurrDate();
					sbyte gender2 = character.GetGender();
					lifeRecordCollection2.AddChangeGradeDrop(charId, currDate2, location2, destOrgInfo.OrgTemplateId, destGrade, destPrincipal, gender2);
				}
			}
			bool flag3 = oriOrgInfo.SettlementId < 0;
			if (!flag3)
			{
				Settlement settlement = this._settlements[oriOrgInfo.SettlementId];
				OrganizationItem orgConfig = Organization.Instance[oriOrgInfo.OrgTemplateId];
				bool flag4 = orgConfig.Hereditary && oriOrgInfo.Principal && oriOrgInfo.Grade > 0;
				if (flag4)
				{
					OrgMemberCollection lackingMembers = settlement.GetLackingCoreMembers();
					lackingMembers.Add(charId, oriOrgInfo.Grade);
					settlement.SetLackingCoreMembers(lackingMembers, context);
				}
				OrgMemberCollection members = settlement.GetMembers();
				members.OnChangeGrade(charId, oriOrgInfo.Grade, destGrade);
				settlement.SetMembers(members, context);
				bool isSect = orgConfig.IsSect;
				if (isSect)
				{
					this.TryAddSectMemberFeature(context, character, destOrgInfo);
					bool flag5 = destGrade == 8 && destPrincipal;
					if (flag5)
					{
						character.AddFeature(context, 405, false);
					}
					else
					{
						bool flag6 = oriOrgInfo.Grade == 8 && oriOrgInfo.Principal;
						if (flag6)
						{
							character.RemoveFeature(context, 405);
						}
					}
				}
				settlement.RemoveSettlementFeatures(context, character);
				settlement.AddSettlementFeatures(context, character);
				int factionId = character.GetFactionId();
				bool flag7 = factionId == charId;
				if (flag7)
				{
					this.RemoveFaction(context, character, false);
				}
				else
				{
					bool flag8 = factionId >= 0;
					if (flag8)
					{
						this.LeaveFaction(context, character, false);
					}
				}
				if (destPrincipal)
				{
					OrganizationDomain.CheckPrincipalMembersAmount(oriOrgInfo.OrgTemplateId, destGrade, members);
				}
				bool flag9 = destGrade > oriOrgInfo.Grade && settlement is Sect;
				if (flag9)
				{
					OrganizationMemberItem orgMemberConfig = OrganizationDomain.GetOrgMemberConfig(destOrgInfo);
					this.SetRandomSectMentor(context, charId, destOrgInfo, members, orgMemberConfig.TeacherGrade);
				}
				character.ChangeMerchantType(context, destOrgInfo);
				ProfessionData professionData = DomainManager.Extra.GetProfessionData(8);
				AristocratSkillsData skillsData = professionData.GetSkillsData<AristocratSkillsData>();
				bool flag10 = skillsData.IsCharacterRecommended(charId) && destPrincipal;
				if (flag10)
				{
					short influencePower = DomainManager.Organization.GetSettlementCharacter(charId).GetInfluencePower();
					bool flag11 = destGrade > oriOrgInfo.Grade;
					if (flag11)
					{
						int gradeChange = (int)(destGrade - oriOrgInfo.Grade);
						ProfessionFormulaItem seniorityFormula = ProfessionFormula.Instance[56];
						int addSeniority = seniorityFormula.Calculate((int)influencePower, gradeChange);
						DomainManager.Extra.ChangeProfessionSeniority(context, 8, addSeniority, true, false);
					}
					bool flag12 = destGrade == 8 && orgConfig.IsSect;
					if (flag12)
					{
						ProfessionFormulaItem seniorityFormula2 = ProfessionFormula.Instance[57];
						int addSeniority2 = seniorityFormula2.Calculate((int)influencePower);
						DomainManager.Extra.ChangeProfessionSeniority(context, 8, addSeniority2, true, false);
					}
				}
				bool flag13 = orgConfig.TemplateId == 16;
				if (flag13)
				{
					DomainManager.Taiwu.OnTaiwuVillagerGradeChanged(context, character, destGrade);
				}
			}
		}

		// Token: 0x060046A2 RID: 18082 RVA: 0x0027721C File Offset: 0x0027541C
		public void JoinNearbyVillageTownAsBeggar(DataContext context, GameData.Domains.Character.Character character, short settlementId = -1)
		{
			bool flag = settlementId < 0;
			if (flag)
			{
				Location location = character.GetLocation();
				bool flag2 = !location.IsValid();
				if (flag2)
				{
					location = character.GetValidLocation();
				}
				bool flag3 = location.AreaId == 138;
				if (flag3)
				{
					MapAreaData areaData = DomainManager.Map.GetElement_Areas((int)location.AreaId);
					settlementId = areaData.SettlementInfos[0].SettlementId;
				}
				else
				{
					bool flag4 = location.AreaId < 135;
					if (flag4)
					{
						sbyte stateId = DomainManager.Map.GetStateIdByAreaId(location.AreaId);
						List<short> settlementIds = ObjectPool<List<short>>.Instance.Get();
						DomainManager.Map.GetStateSettlementIds(stateId, settlementIds, false, false);
						settlementIds.Remove(character.GetOrganizationInfo().SettlementId);
						bool flag5 = settlementIds.Count > 0;
						if (flag5)
						{
							settlementId = settlementIds.GetRandom(context.Random);
						}
						else
						{
							settlementId = -1;
						}
						ObjectPool<List<short>>.Instance.Return(settlementIds);
					}
					else
					{
						settlementId = -1;
					}
				}
			}
			bool flag6 = settlementId >= 0;
			if (flag6)
			{
				Settlement settlement = this.GetSettlement(settlementId);
				OrganizationInfo orgInfo = new OrganizationInfo(settlement.GetOrgTemplateId(), 0, true, settlementId);
				this.ChangeOrganization(context, character, orgInfo);
			}
			else
			{
				this.ChangeOrganization(context, character, OrganizationInfo.None);
			}
		}

		// Token: 0x060046A3 RID: 18083 RVA: 0x00277364 File Offset: 0x00275564
		public void UpdateOrganizationAfterMarriage(DataContext context, GameData.Domains.Character.Character selfChar, GameData.Domains.Character.Character targetChar)
		{
			int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
			bool flag = selfChar.GetId() == taiwuCharId || targetChar.GetId() == taiwuCharId;
			if (!flag)
			{
				OrganizationInfo selfOrgInfo = selfChar.GetOrganizationInfo();
				bool selfIsSectMember = Organization.Instance[selfOrgInfo.OrgTemplateId].IsSect;
				sbyte selfCharGrade = selfOrgInfo.Grade;
				int selfAuthority = selfChar.GetResource(7);
				OrganizationInfo targetOrgInfo = targetChar.GetOrganizationInfo();
				bool targetIsSectMember = Organization.Instance[targetOrgInfo.OrgTemplateId].IsSect;
				sbyte targetCharGrade = targetOrgInfo.Grade;
				int targetAuthority = targetChar.GetResource(7);
				bool flag2 = selfIsSectMember && !targetIsSectMember;
				if (flag2)
				{
					DomainManager.Organization.JoinSpouseOrganization(context, targetChar, selfChar);
				}
				else
				{
					bool flag3 = !selfIsSectMember && targetIsSectMember;
					if (flag3)
					{
						DomainManager.Organization.JoinSpouseOrganization(context, selfChar, targetChar);
					}
					else
					{
						bool flag4 = selfCharGrade > targetCharGrade;
						if (flag4)
						{
							DomainManager.Organization.JoinSpouseOrganization(context, targetChar, selfChar);
						}
						else
						{
							bool flag5 = selfCharGrade < targetCharGrade;
							if (flag5)
							{
								DomainManager.Organization.JoinSpouseOrganization(context, selfChar, targetChar);
							}
							else
							{
								bool flag6 = selfAuthority > targetAuthority;
								if (flag6)
								{
									DomainManager.Organization.JoinSpouseOrganization(context, targetChar, selfChar);
								}
								else
								{
									bool flag7 = selfAuthority < targetAuthority;
									if (flag7)
									{
										DomainManager.Organization.JoinSpouseOrganization(context, selfChar, targetChar);
									}
									else
									{
										bool flag8 = selfChar.GetId() < targetChar.GetId();
										if (flag8)
										{
											DomainManager.Organization.JoinSpouseOrganization(context, targetChar, selfChar);
										}
										else
										{
											DomainManager.Organization.JoinSpouseOrganization(context, selfChar, targetChar);
										}
									}
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x060046A4 RID: 18084 RVA: 0x002774E4 File Offset: 0x002756E4
		public void JoinSpouseOrganization(DataContext context, GameData.Domains.Character.Character selfChar, GameData.Domains.Character.Character spouseChar)
		{
			OrganizationInfo selfOrgInfo = selfChar.GetOrganizationInfo();
			OrganizationInfo spouseOrgInfo = spouseChar.GetOrganizationInfo();
			OrganizationMemberItem spouseOrgMemberCfg = OrganizationDomain.GetOrgMemberConfig(spouseOrgInfo);
			bool condition = spouseOrgInfo.Principal && spouseOrgMemberCfg.ChildGrade >= 0;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(23, 4);
			defaultInterpolatedStringHandler.AppendFormatted<GameData.Domains.Character.Character>(selfChar);
			defaultInterpolatedStringHandler.AppendLiteral(" x ");
			defaultInterpolatedStringHandler.AppendFormatted<GameData.Domains.Character.Character>(spouseChar);
			defaultInterpolatedStringHandler.AppendLiteral(" Marriage: ");
			defaultInterpolatedStringHandler.AppendFormatted<bool>(spouseOrgInfo.Principal);
			defaultInterpolatedStringHandler.AppendLiteral(" && ");
			defaultInterpolatedStringHandler.AppendFormatted<sbyte>(spouseOrgMemberCfg.ChildGrade);
			defaultInterpolatedStringHandler.AppendLiteral(" >= 0");
			Tester.Assert(condition, defaultInterpolatedStringHandler.ToStringAndClear());
			bool flag = selfOrgInfo.OrgTemplateId == spouseOrgInfo.OrgTemplateId && selfOrgInfo.SettlementId == spouseOrgInfo.SettlementId;
			if (flag)
			{
				this.UpdateGradeAccordingToSpouse(context, selfChar, spouseChar);
			}
			else
			{
				bool flag2 = spouseOrgInfo.OrgTemplateId == 16;
				if (flag2)
				{
					OrganizationInfo selfNewOrgInfo = new OrganizationInfo(spouseOrgInfo.OrgTemplateId, 0, true, spouseOrgInfo.SettlementId);
					DomainManager.Organization.ChangeOrganization(context, selfChar, selfNewOrgInfo);
				}
				else
				{
					OrganizationInfo selfNewOrgInfo2 = new OrganizationInfo(spouseOrgInfo.OrgTemplateId, (!spouseOrgMemberCfg.RestrictPrincipalAmount || spouseOrgMemberCfg.DeputySpouseDowngrade >= 0) ? spouseOrgInfo.Grade : 0, spouseOrgMemberCfg.DeputySpouseDowngrade < 0, spouseOrgInfo.SettlementId);
					DomainManager.Organization.ChangeOrganization(context, selfChar, selfNewOrgInfo2);
				}
			}
		}

		// Token: 0x060046A5 RID: 18085 RVA: 0x0027764C File Offset: 0x0027584C
		public void UpdateGradeAccordingToSpouse(DataContext context, GameData.Domains.Character.Character selfChar, GameData.Domains.Character.Character spouseChar)
		{
			OrganizationInfo selfOrgInfo = selfChar.GetOrganizationInfo();
			OrganizationInfo spouseOrgInfo = spouseChar.GetOrganizationInfo();
			bool flag = selfOrgInfo.OrgTemplateId != spouseOrgInfo.OrgTemplateId || selfOrgInfo.SettlementId != spouseOrgInfo.SettlementId;
			if (!flag)
			{
				bool flag2 = selfOrgInfo.Grade >= spouseOrgInfo.Grade && selfOrgInfo.Principal;
				if (!flag2)
				{
					bool flag3 = selfOrgInfo.OrgTemplateId == 16;
					if (!flag3)
					{
						OrganizationMemberItem spouseOrgMemberCfg = OrganizationDomain.GetOrgMemberConfig(spouseOrgInfo);
						Tester.Assert(spouseOrgInfo.Principal, "");
						bool flag4 = spouseOrgMemberCfg.DeputySpouseDowngrade < 0;
						if (flag4)
						{
							bool flag5 = !selfOrgInfo.Principal && !spouseOrgMemberCfg.RestrictPrincipalAmount;
							if (flag5)
							{
								this.ChangeGrade(context, selfChar, spouseOrgInfo.Grade, true);
							}
						}
						else
						{
							this.ChangeGrade(context, selfChar, spouseOrgInfo.Grade, false);
						}
					}
				}
			}
		}

		// Token: 0x060046A6 RID: 18086 RVA: 0x00277728 File Offset: 0x00275928
		private void UpdateAllMentorsAndMenteesInSect(DataContext context, Sect sect)
		{
			OrgMemberCollection sectMembers = sect.GetMembers();
			for (sbyte grade = 0; grade < 8; grade += 1)
			{
				HashSet<int> members = sectMembers.GetMembers(grade);
				foreach (int charId in members)
				{
					GameData.Domains.Character.Character mentee;
					bool flag = !DomainManager.Character.TryGetElement_Objects(charId, out mentee);
					if (!flag)
					{
						OrganizationInfo menteeOrgInfo = mentee.GetOrganizationInfo();
						OrganizationMemberItem orgMemberConfig = OrganizationDomain.GetOrgMemberConfig(menteeOrgInfo);
						DomainManager.Organization.SetRandomSectMentor(context, charId, menteeOrgInfo, sectMembers, orgMemberConfig.TeacherGrade);
					}
				}
			}
		}

		// Token: 0x060046A7 RID: 18087 RVA: 0x002777E4 File Offset: 0x002759E4
		public void GetCharactersFromSettlement(short settlementId, sbyte minGrade, sbyte maxGrade, List<GameData.Domains.Character.Character> result)
		{
			this.GetCharactersFromSettlementWithInfantFilter(settlementId, minGrade, maxGrade, result, true);
		}

		// Token: 0x060046A8 RID: 18088 RVA: 0x002777F4 File Offset: 0x002759F4
		public void GetCharactersFromSettlementWithInfantFilter(short settlementId, sbyte minGrade, sbyte maxGrade, List<GameData.Domains.Character.Character> result, bool includeInfant = false)
		{
			result.Clear();
			Settlement sect = DomainManager.Organization.GetSettlement(settlementId);
			OrgMemberCollection sectMembers = sect.GetMembers();
			for (sbyte grade = minGrade; grade <= maxGrade; grade += 1)
			{
				IEnumerable<GameData.Domains.Character.Character> gradeMembers = from memberId in sectMembers.GetMembers(grade)
				select DomainManager.Character.GetElement_Objects(memberId);
				bool flag = !includeInfant;
				if (flag)
				{
					gradeMembers = DomainManager.Character.ExcludeInfant(gradeMembers);
				}
				result.AddRange(gradeMembers);
			}
		}

		// Token: 0x060046A9 RID: 18089 RVA: 0x00277884 File Offset: 0x00275A84
		public void OnCharacterDead(DataContext context, GameData.Domains.Character.Character character)
		{
			this.LeaveOrganization(context, character, true);
			int charId = character.GetId();
			sbyte fugitiveSectId = this.GetFugitiveBountySect(charId);
			bool flag = fugitiveSectId >= 0;
			if (flag)
			{
				Sect sect = (Sect)this.GetSettlementByOrgTemplateId(fugitiveSectId);
				sect.RemoveBounty(context, charId);
			}
			sbyte prisonerSectId = this.GetPrisonerSect(charId);
			bool flag2 = prisonerSectId >= 0;
			if (flag2)
			{
				Sect sect2 = (Sect)this.GetSettlementByOrgTemplateId(prisonerSectId);
				sect2.RemovePrisoner(context, charId);
			}
			DomainManager.Building.TryRemoveFeastCustomer(context, charId);
		}

		// Token: 0x060046AA RID: 18090 RVA: 0x0027790C File Offset: 0x00275B0C
		public void OnSectMemberCrimeMadePublic(DataContext context, GameData.Domains.Character.Character character, OrganizationInfo orgInfoOnCommit, sbyte punishmentSeverity, short punishmentType)
		{
			bool flag = punishmentSeverity < 0 || punishmentType < 0 || orgInfoOnCommit.SettlementId != character.GetOrganizationInfo().SettlementId;
			if (!flag)
			{
				bool flag2 = character.IsCompletelyInfected();
				if (!flag2)
				{
					Sect sect;
					bool flag3 = !DomainManager.Organization.TryGetElement_Sects(orgInfoOnCommit.SettlementId, out sect);
					if (flag3)
					{
						bool flag4 = orgInfoOnCommit.SettlementId < 0;
						if (flag4)
						{
							return;
						}
						Location settlementLocation = DomainManager.Organization.GetSettlement(orgInfoOnCommit.SettlementId).GetLocation();
						bool flag5 = !settlementLocation.IsValid();
						if (flag5)
						{
							return;
						}
						sbyte stateTemplateId = DomainManager.Map.GetStateTemplateIdByAreaId(settlementLocation.AreaId);
						MapStateItem stateCfg = MapState.Instance[stateTemplateId];
						bool flag6 = stateCfg.SectID < 0;
						if (flag6)
						{
							return;
						}
						sect = (Sect)DomainManager.Organization.GetSettlementByOrgTemplateId(stateCfg.SectID);
					}
					PunishmentSeverityItem punishSeverityCfg = PunishmentSeverity.Instance[punishmentSeverity];
					sbyte behaviorType = character.GetBehaviorType();
					sbyte escapeChance = punishSeverityCfg.EscapePunishmentChance[(int)behaviorType];
					bool flag7 = punishmentType == 40;
					if (flag7)
					{
						escapeChance = 100;
					}
					bool flag8 = character.IsActiveExternalRelationState(32);
					if (flag8)
					{
						DomainManager.Organization.PunishSectMember(context, sect, character, punishmentSeverity, punishmentType, true);
					}
					else
					{
						bool flag9 = !context.Random.CheckPercentProb((int)escapeChance) && character.IsInteractableAsIntelligentCharacter();
						if (flag9)
						{
							DomainManager.Character.LeaveGroup(context, character, true);
							DomainManager.Character.GroupMove(context, character, sect.GetLocation());
							DomainManager.Organization.PunishSectMember(context, sect, character, punishmentSeverity, punishmentType, false);
						}
						else
						{
							OrganizationInfo currOrgInfo = character.GetOrganizationInfo();
							bool flag10 = currOrgInfo.OrgTemplateId == orgInfoOnCommit.OrgTemplateId;
							if (flag10)
							{
								DomainManager.Organization.ChangeOrganization(context, character, new OrganizationInfo(0, currOrgInfo.Grade, true, -1));
							}
							sect.AddBounty(context, character, punishmentSeverity, punishmentType, -1);
						}
					}
				}
			}
		}

		// Token: 0x060046AB RID: 18091 RVA: 0x00277AF8 File Offset: 0x00275CF8
		public unsafe void PunishSectMember(DataContext context, Sect sect, GameData.Domains.Character.Character character, sbyte punishmentSeverity = -1, short punishmentType = -1, bool isArrested = false)
		{
			int charId = character.GetId();
			OrganizationInfo orgInfo = character.GetOrganizationInfo();
			sbyte sectTemplateId = sect.GetOrgTemplateId();
			short sectSettlementId = sect.GetId();
			PunishmentTypeItem punishmentTypeCfg = PunishmentType.Instance[punishmentType];
			bool flag = punishmentTypeCfg == null;
			if (flag)
			{
				short predefinedLogId = 33;
				PunishmentSeverityItem item = PunishmentSeverity.Instance.GetItem(punishmentSeverity);
				PredefinedLog.Show(predefinedLogId, character, (item != null) ? item.Name : null);
			}
			else
			{
				bool flag2 = punishmentSeverity < 0;
				if (flag2)
				{
					punishmentSeverity = sect.GetPunishmentTypeSeverity(punishmentTypeCfg, true);
				}
				PunishmentSeverityItem punishmentSeverityCfg = PunishmentSeverity.Instance[punishmentSeverity];
				bool flag3 = punishmentSeverityCfg.ResourceConfiscation > 0;
				if (flag3)
				{
					ResourceInts resources = *character.GetResources();
					bool flag4 = punishmentSeverityCfg.ResourceConfiscation == 1;
					if (flag4)
					{
						for (sbyte resourceType = 0; resourceType < 8; resourceType += 1)
						{
							*resources[(int)resourceType] /= 2;
						}
					}
					sect.ConfiscateResources(context, character, ref resources);
				}
				bool flag5 = punishmentSeverityCfg.ItemConfiscation > 0;
				if (flag5)
				{
					List<ItemKey> itemKeys = ObjectPool<List<ItemKey>>.Instance.Get();
					character.GetItemsToLose(itemKeys, 0, 8);
					itemKeys.Sort(ItemTemplateHelper.ItemGradeComparer);
					bool flag6 = punishmentSeverityCfg.ItemConfiscation == 1;
					if (flag6)
					{
						int removeIndex = itemKeys.Count / 2;
						itemKeys.RemoveRange(removeIndex, itemKeys.Count - removeIndex);
					}
					sect.ConfiscateItem(context, character, itemKeys);
					ObjectPool<List<ItemKey>>.Instance.Return(itemKeys);
				}
				bool flag7 = punishmentSeverityCfg.CombatSkillRevoke > 0;
				if (flag7)
				{
					List<short> skillsToRevoke = ObjectPool<List<short>>.Instance.Get();
					character.GetLearnedCombatSkillsFromSect(skillsToRevoke, sectTemplateId, 0, 8);
					int removeCount = skillsToRevoke.Count;
					bool flag8 = punishmentSeverityCfg.CombatSkillRevoke == 1;
					if (flag8)
					{
						removeCount /= 2;
						skillsToRevoke.Sort(CombatSkillHelper.CombatSkillGradeComparer);
					}
					for (int index = skillsToRevoke.Count - removeCount; index < skillsToRevoke.Count; index++)
					{
						short skillId = skillsToRevoke[index];
						DomainManager.Character.RevokeCombatSkill(context, character, skillId);
					}
					ObjectPool<List<short>>.Instance.Return(skillsToRevoke);
				}
				bool flag9 = character.GetId() == DomainManager.Taiwu.GetTaiwuCharId();
				if (!flag9)
				{
					bool flag10 = punishmentSeverityCfg.PrisonTime > 0;
					if (flag10)
					{
						sect.AddPrisoner(context, character, punishmentSeverity, punishmentType, -1);
						SettlementPrisonRecordCollection prisonRecord = DomainManager.Organization.GetSettlementPrisonRecordCollection(context, sectSettlementId);
						int currDate = DomainManager.World.GetCurrDate();
						if (isArrested)
						{
							prisonRecord.AddImprisonedByArrested(currDate, sectSettlementId, charId, punishmentType);
						}
						else
						{
							prisonRecord.AddImprisonedVoluntarily(currDate, sectSettlementId, charId, punishmentType);
						}
					}
					LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
					bool expel = punishmentSeverityCfg.Expel;
					if (expel)
					{
						int spouseId = DomainManager.Character.GetAliveSpouse(charId);
						GameData.Domains.Character.Character spouseChar = (spouseId >= 0) ? DomainManager.Character.GetElement_Objects(spouseId) : null;
						OrganizationInfo spouseOrgInfo = default(OrganizationInfo);
						bool flag11 = spouseChar != null;
						if (flag11)
						{
							spouseOrgInfo = spouseChar.GetOrganizationInfo();
							bool flag12 = spouseOrgInfo.OrgTemplateId != orgInfo.OrgTemplateId || (spouseOrgInfo.Principal && orgInfo.Principal);
							if (flag12)
							{
								spouseChar = null;
								spouseId = -1;
							}
						}
						lifeRecordCollection.AddSectPunishmentRecord(punishmentTypeCfg, punishmentSeverityCfg, sectSettlementId, isArrested, character, spouseId);
						bool flag13 = spouseChar != null;
						if (flag13)
						{
							bool principal = spouseOrgInfo.Principal;
							if (principal)
							{
								GameData.Domains.Character.Character.ApplySeverHusbandOrWife(context, character, spouseChar, character.GetBehaviorType(), false, false);
							}
							else
							{
								this.PunishSectMember(context, sect, spouseChar, punishmentSeverity, 20, false);
							}
						}
					}
					else
					{
						lifeRecordCollection.AddSectPunishmentRecord(punishmentTypeCfg, punishmentSeverityCfg, sectSettlementId, isArrested, character, -1);
						bool flag14 = !punishmentSeverityCfg.Expel && orgInfo.OrgTemplateId == 0;
						if (flag14)
						{
							sbyte stateId = DomainManager.Map.GetStateIdByAreaId(sect.GetLocation().AreaId);
							short targetSettlementId = DomainManager.Map.GetRandomStateSettlementId(context.Random, stateId, true, false);
							bool flag15 = targetSettlementId < 0;
							if (flag15)
							{
								targetSettlementId = sectSettlementId;
							}
							Settlement targetSettlement = DomainManager.Organization.GetSettlement(targetSettlementId);
							OrganizationMemberItem orgMemberCfg = OrganizationDomain.GetOrgMemberConfig(targetSettlement.GetOrgTemplateId(), orgInfo.Grade);
							sbyte rejoinGrade = orgMemberCfg.GetRejoinGrade();
							OrganizationInfo newOrgInfo = new OrganizationInfo(targetSettlement.GetOrgTemplateId(), rejoinGrade, true, targetSettlementId);
							DomainManager.Organization.ChangeOrganization(context, character, newOrgInfo);
						}
					}
				}
			}
		}

		// Token: 0x060046AC RID: 18092 RVA: 0x00277F30 File Offset: 0x00276130
		public void TryAddSectMemberFeature(DataContext context, GameData.Domains.Character.Character character, OrganizationInfo dstOrgInfo)
		{
			bool flag = character.GetAgeGroup() != 2;
			if (!flag)
			{
				short featureId = Organization.Instance[dstOrgInfo.OrgTemplateId].MemberFeature;
				bool flag2 = featureId < 0;
				if (!flag2)
				{
					bool flag3 = (int)dstOrgInfo.Grade < GlobalConfig.Instance.AddMemberFeatureMinGrade;
					if (!flag3)
					{
						character.AddFeature(context, featureId, false);
					}
				}
			}
		}

		// Token: 0x060046AD RID: 18093 RVA: 0x00277F94 File Offset: 0x00276194
		public static short GetApprovingRateUpperLimit()
		{
			int xiangshuLevel = Math.Clamp((int)DomainManager.World.GetXiangshuLevel(), 0, GlobalConfig.Instance.SectApprovingRateUpperLimits.Length);
			int upperLimit = (int)(GlobalConfig.Instance.SectApprovingRateUpperLimits[xiangshuLevel] * 10);
			return (short)Math.Min(upperLimit, 1000);
		}

		// Token: 0x060046AE RID: 18094 RVA: 0x00277FE0 File Offset: 0x002761E0
		public static sbyte GetHighestGradeOfTeachableCombatSkill(short approvingRate)
		{
			bool flag = approvingRate < 300;
			sbyte result;
			if (flag)
			{
				result = 1;
			}
			else
			{
				int grade = (int)(2 + (approvingRate - 300) / 100);
				result = (sbyte)Math.Clamp(grade, 0, 8);
			}
			return result;
		}

		// Token: 0x060046AF RID: 18095 RVA: 0x00278018 File Offset: 0x00276218
		public static bool IsLargeSect(short orgTemplateId)
		{
			return orgTemplateId >= 1 && orgTemplateId <= 15;
		}

		// Token: 0x060046B0 RID: 18096 RVA: 0x0027803C File Offset: 0x0027623C
		public static sbyte GetLargeSectIndex(sbyte orgTemplateId)
		{
			return (orgTemplateId >= 1 && orgTemplateId <= 15) ? (orgTemplateId - 1) : -1;
		}

		// Token: 0x060046B1 RID: 18097 RVA: 0x00278060 File Offset: 0x00276260
		public static sbyte GetLargeSectTemplateId(sbyte index)
		{
			return index + 1;
		}

		// Token: 0x060046B2 RID: 18098 RVA: 0x00278078 File Offset: 0x00276278
		public void UpdateSectPrisonersOnAdvanceMonth(DataContext context)
		{
			foreach (KeyValuePair<short, Sect> keyValuePair in this._sects)
			{
				short num;
				Sect sect2;
				keyValuePair.Deconstruct(out num, out sect2);
				Sect sect = sect2;
				sect.UpdatePrisonOnAdvanceMonth(context);
			}
		}

		// Token: 0x060046B3 RID: 18099 RVA: 0x002780E0 File Offset: 0x002762E0
		public void UpdateFugitiveGroupsOnAdvanceMonth(DataContext context)
		{
			Dictionary<IntPair, List<GameData.Domains.Character.Character>> groups = new Dictionary<IntPair, List<GameData.Domains.Character.Character>>();
			GameData.Domains.Character.Character taiwuChar = DomainManager.Taiwu.GetTaiwu();
			foreach (KeyValuePair<short, Sect> keyValuePair in this._sects)
			{
				short num;
				Sect sect2;
				keyValuePair.Deconstruct(out num, out sect2);
				Sect sect = sect2;
				groups.Clear();
				List<SettlementBounty> bounties = sect.Prison.Bounties;
				foreach (SettlementBounty bounty in bounties)
				{
					GameData.Domains.Character.Character character;
					bool flag = !DomainManager.Character.TryGetElement_Objects(bounty.CharId, out character);
					if (!flag)
					{
						bool flag2 = !character.IsInteractableAsIntelligentCharacter();
						if (!flag2)
						{
							bool flag3 = character == taiwuChar;
							if (!flag3)
							{
								Location location = character.GetLocation();
								bool flag4 = location.AreaId < 0;
								if (!flag4)
								{
									BasePrioritizedAction action;
									bool flag5 = !DomainManager.Character.TryGetCharacterPrioritizedAction(bounty.CharId, out action) || (action.ActionType != 18 && action.ActionType != 19);
									if (!flag5)
									{
										int leaderId = character.GetLeaderId();
										bool flag6 = leaderId >= 0;
										if (!flag6)
										{
											PrioritizedActionsItem actionCfg = PrioritizedActions.Instance[action.ActionType];
											sbyte behaviorType = character.GetBehaviorType();
											bool flag7 = !context.Random.CheckPercentProb((int)actionCfg.ActionJointChance[(int)behaviorType]);
											if (!flag7)
											{
												IntPair key = new IntPair((int)location.AreaId, (int)action.Target.GetRealTargetLocation().AreaId);
												List<GameData.Domains.Character.Character> group;
												bool flag8 = !groups.TryGetValue(key, out group);
												if (flag8)
												{
													group = new List<GameData.Domains.Character.Character>();
													groups.Add(key, group);
												}
												group.Add(character);
											}
										}
									}
								}
							}
						}
					}
				}
				foreach (KeyValuePair<IntPair, List<GameData.Domains.Character.Character>> keyValuePair2 in groups)
				{
					IntPair intPair;
					List<GameData.Domains.Character.Character> list;
					keyValuePair2.Deconstruct(out intPair, out list);
					List<GameData.Domains.Character.Character> group2 = list;
					bool flag9 = group2.Count <= 1;
					if (!flag9)
					{
						GameData.Domains.Character.Character leader = group2.GetRandom(context.Random);
						foreach (GameData.Domains.Character.Character groupChar in group2)
						{
							bool flag10 = leader == groupChar;
							if (!flag10)
							{
								DomainManager.Character.JoinGroup(context, groupChar, leader);
							}
						}
					}
				}
			}
		}

		// Token: 0x060046B4 RID: 18100 RVA: 0x00278400 File Offset: 0x00276600
		public void UpdateOrganizationMembers(DataContext context)
		{
			int currDate = DomainManager.World.GetCurrDate();
			Dictionary<int, ValueTuple<GameData.Domains.Character.Character, short>> baseInfluencePowers = new Dictionary<int, ValueTuple<GameData.Domains.Character.Character, short>>();
			HashSet<int> relatedCharIds = new HashSet<int>();
			foreach (KeyValuePair<short, Sect> keyValuePair in this._sects)
			{
				short num;
				Sect sect2;
				keyValuePair.Deconstruct(out num, out sect2);
				Sect sect = sect2;
				sbyte orgTemplateId = sect.GetOrgTemplateId();
				sect.UpdateMemberGrades(context);
				short influencePowerUpdateInterval = Organization.Instance[orgTemplateId].InfluencePowerUpdateInterval;
				int influencePowerUpdateDate = sect.GetInfluencePowerUpdateDate();
				bool flag = influencePowerUpdateInterval > 0 && currDate >= influencePowerUpdateDate;
				if (flag)
				{
					sect.UpdateInfluencePowers(context, baseInfluencePowers, relatedCharIds);
					sect.SetInfluencePowerUpdateDate(currDate + (int)influencePowerUpdateInterval, context);
					this.UpdateFactions(context, sect);
				}
				bool flag2 = currDate % 3 == 0;
				if (flag2)
				{
					sect.UpdateApprovalOfTaiwu(context);
				}
				this.UpdateAllMentorsAndMenteesInSect(context, sect);
				sect.UpdateTreasuryOnAdvanceMonth(context);
				sect.PrisonEnteredStatus = 0;
			}
			foreach (KeyValuePair<short, CivilianSettlement> keyValuePair2 in this._civilianSettlements)
			{
				short num;
				CivilianSettlement civilianSettlement;
				keyValuePair2.Deconstruct(out num, out civilianSettlement);
				CivilianSettlement settlement = civilianSettlement;
				sbyte orgTemplateId2 = settlement.GetOrgTemplateId();
				settlement.UpdateMemberGrades(context);
				short influencePowerUpdateInterval2 = Organization.Instance[orgTemplateId2].InfluencePowerUpdateInterval;
				int influencePowerUpdateDate2 = settlement.GetInfluencePowerUpdateDate();
				bool flag3 = influencePowerUpdateInterval2 > 0 && currDate >= influencePowerUpdateDate2;
				if (flag3)
				{
					settlement.UpdateInfluencePowers(context, baseInfluencePowers, relatedCharIds);
					settlement.SetInfluencePowerUpdateDate(currDate + (int)influencePowerUpdateInterval2, context);
				}
				settlement.UpdateTreasuryOnAdvanceMonth(context);
			}
			short taiwuVillageSettlementId = DomainManager.Taiwu.GetTaiwuVillageSettlementId();
			Settlement taiwuVillageSettlement = DomainManager.Organization.GetSettlement(taiwuVillageSettlementId);
			sbyte orgTemplateId3 = taiwuVillageSettlement.GetOrgTemplateId();
			short influencePowerUpdateInterval3 = Organization.Instance[orgTemplateId3].InfluencePowerUpdateInterval;
			int influencePowerUpdateDate3 = taiwuVillageSettlement.GetInfluencePowerUpdateDate();
			bool flag4 = influencePowerUpdateInterval3 > 0 && currDate >= influencePowerUpdateDate3;
			if (flag4)
			{
				taiwuVillageSettlement.UpdateTaiwuVillagerInfluencePowers(context, baseInfluencePowers, relatedCharIds);
				taiwuVillageSettlement.SetInfluencePowerUpdateDate(currDate + (int)influencePowerUpdateInterval3, context);
			}
			this.UpdateSettlementCacheInfo();
		}

		// Token: 0x060046B5 RID: 18101 RVA: 0x00278650 File Offset: 0x00276850
		[DomainMethod]
		public void ForceUpdateTaiwuVillager(DataContext context)
		{
			Dictionary<int, ValueTuple<GameData.Domains.Character.Character, short>> baseInfluencePowers = new Dictionary<int, ValueTuple<GameData.Domains.Character.Character, short>>();
			HashSet<int> relatedCharIds = new HashSet<int>();
			short taiwuVillageSettlementId = DomainManager.Taiwu.GetTaiwuVillageSettlementId();
			Settlement taiwuVillageSettlement = DomainManager.Organization.GetSettlement(taiwuVillageSettlementId);
			taiwuVillageSettlement.UpdateTaiwuVillagerInfluencePowers(context, baseInfluencePowers, relatedCharIds);
		}

		// Token: 0x060046B6 RID: 18102 RVA: 0x0027868C File Offset: 0x0027688C
		private void UpdateSettlementCacheInfo()
		{
			MonthlyActionKey martialArtTournamentKey = MonthlyEventActionsManager.PredefinedKeys["MartialArtTournamentDefault"];
			MonthlyActionBase monthlyAction = DomainManager.TaiwuEvent.GetMonthlyAction(martialArtTournamentKey);
			bool isPreparing = monthlyAction.State != 0;
			bool parallelUpdateOrganizationMembers = this.ParallelUpdateOrganizationMembers;
			if (parallelUpdateOrganizationMembers)
			{
				Parallel.ForEach<KeyValuePair<short, Settlement>>(this._settlements, delegate(KeyValuePair<short, Settlement> pair)
				{
					Settlement settlement2 = pair.Value;
					settlement2.SortMembersByCombatPower();
					Sect sect2;
					bool flag3;
					if (isPreparing)
					{
						sect2 = (settlement2 as Sect);
						flag3 = (sect2 != null);
					}
					else
					{
						flag3 = false;
					}
					bool flag4 = flag3;
					if (flag4)
					{
						sect2.UpdateMartialArtTournamentPreparations();
					}
				});
			}
			else
			{
				foreach (KeyValuePair<short, Settlement> pair2 in this._settlements)
				{
					Settlement settlement = pair2.Value;
					settlement.SortMembersByCombatPower();
					Sect sect;
					bool flag;
					if (isPreparing)
					{
						sect = (settlement as Sect);
						flag = (sect != null);
					}
					else
					{
						flag = false;
					}
					bool flag2 = flag;
					if (flag2)
					{
						sect.UpdateMartialArtTournamentPreparations();
					}
				}
			}
			DomainManager.Character.UpdateTopThousandCharRanking();
		}

		// Token: 0x060046B7 RID: 18103 RVA: 0x00278778 File Offset: 0x00276978
		public void ForceUpdateInfluencePowers(DataContext context)
		{
			Dictionary<int, ValueTuple<GameData.Domains.Character.Character, short>> baseInfluencePowers = new Dictionary<int, ValueTuple<GameData.Domains.Character.Character, short>>();
			HashSet<int> relatedCharIds = new HashSet<int>();
			int currDate = DomainManager.World.GetCurrDate();
			DomainManager.Extra.InitTreasurySupplies();
			foreach (KeyValuePair<short, Settlement> entry in this._settlements)
			{
				Settlement settlement = entry.Value;
				short influencePowerUpdateInterval = Organization.Instance[settlement.GetOrgTemplateId()].InfluencePowerUpdateInterval;
				settlement.UpdateInfluencePowers(context, baseInfluencePowers, relatedCharIds);
				bool flag = influencePowerUpdateInterval > 0;
				if (flag)
				{
					settlement.SetInfluencePowerUpdateDate(currDate + (int)influencePowerUpdateInterval, context);
				}
				Sect sect = settlement as Sect;
				bool flag2 = sect != null;
				if (flag2)
				{
					this.UpdateFactions(context, sect);
				}
			}
		}

		// Token: 0x060046B8 RID: 18104 RVA: 0x00278850 File Offset: 0x00276A50
		public void ResetSectExploreStatuses(DataContext context)
		{
			foreach (KeyValuePair<short, Sect> keyValuePair in this._sects)
			{
				short num;
				Sect sect2;
				keyValuePair.Deconstruct(out num, out sect2);
				Sect sect = sect2;
				sect.SetTaiwuExploreStatus(0, context);
				sect.SetSpiritualDebtInteractionOccurred(false, context);
			}
		}

		// Token: 0x060046B9 RID: 18105 RVA: 0x002788C4 File Offset: 0x00276AC4
		public void BecomeSectMonk(DataContext context, GameData.Domains.Character.Character character, short mentorSeniorityId)
		{
			OrganizationInfo orgInfo = character.GetOrganizationInfo();
			OrganizationMemberItem orgMemberCfg = OrganizationDomain.GetOrgMemberConfig(orgInfo);
			Sect sect = this._sects[orgInfo.SettlementId];
			OrganizationDomain.BecomeSectMonkInternal(context, character, sect, orgMemberCfg, mentorSeniorityId);
		}

		// Token: 0x060046BA RID: 18106 RVA: 0x00278900 File Offset: 0x00276B00
		public static void TryBecomeSectMonk(DataContext context, GameData.Domains.Character.Character character, Sect sect, OrganizationMemberItem orgMemberCfg, short mentorSeniorityId)
		{
			bool flag = !OrganizationDomain.CheckConditionOfBecomingSectMonk(context, character, orgMemberCfg);
			if (!flag)
			{
				bool flag2 = orgMemberCfg.ProbOfBecomingMonk <= 0;
				if (!flag2)
				{
					bool flag3 = !context.Random.CheckPercentProb((int)orgMemberCfg.ProbOfBecomingMonk);
					if (!flag3)
					{
						OrganizationDomain.BecomeSectMonkInternal(context, character, sect, orgMemberCfg, mentorSeniorityId);
					}
				}
			}
		}

		// Token: 0x060046BB RID: 18107 RVA: 0x00278958 File Offset: 0x00276B58
		public void SelectRandomSectCharacterToApproveTaiwu(DataContext context, sbyte orgTemplateId, sbyte grade)
		{
			Tester.Assert(OrganizationDomain.IsSect(orgTemplateId), "");
			Settlement organization = this.GetSettlementByOrgTemplateId(orgTemplateId);
			OrgMemberCollection orgMembers = organization.GetMembers();
			List<int> validMembers = ObjectPool<List<int>>.Instance.Get();
			for (sbyte actualGrade = grade; actualGrade >= 0; actualGrade -= 1)
			{
				IEnumerable<int> gradeMembers = DomainManager.Character.ExcludeInfant(orgMembers.GetMembers(actualGrade));
				validMembers.Clear();
				foreach (int gradeMember in gradeMembers)
				{
					SectCharacter sectChar = this._sectCharacters[gradeMember];
					bool flag = !sectChar.GetApprovedTaiwu();
					if (flag)
					{
						validMembers.Add(gradeMember);
					}
				}
				bool flag2 = validMembers.Count <= 0;
				if (!flag2)
				{
					break;
				}
			}
			bool flag3 = validMembers.Count > 0;
			if (flag3)
			{
				int targetGradeSectCharId = validMembers.GetRandom(context.Random);
				SectCharacter sectChar2 = this._sectCharacters[targetGradeSectCharId];
				sectChar2.SetApprovedTaiwu(context, true);
			}
			ObjectPool<List<int>>.Instance.Return(validMembers);
		}

		// Token: 0x060046BC RID: 18108 RVA: 0x00278A8C File Offset: 0x00276C8C
		public void ShowCharactersStats()
		{
			int worldBabyCount = 0;
			int worldChildCount = 0;
			int worldAdultCount = 0;
			List<int> ages = new List<int>();
			StringBuilder message = new StringBuilder();
			message.AppendLine("Organization characters:");
			StringBuilder stringBuilder;
			StringBuilder.AppendInterpolatedStringHandler appendInterpolatedStringHandler;
			foreach (KeyValuePair<short, Sect> keyValuePair in this._sects)
			{
				short num;
				Sect sect2;
				keyValuePair.Deconstruct(out num, out sect2);
				Sect sect = sect2;
				MapBlockData block = DomainManager.Map.GetBlock(sect.GetLocation()).GetRootBlock();
				string name = MapBlock.Instance[block.TemplateId].Name;
				ValueTuple<int, int, int> ageStats = OrganizationDomain.GetAgeStats(sect.GetMembers(), ages);
				int babyCount = ageStats.Item1;
				int childCount = ageStats.Item2;
				int adultCount = ageStats.Item3;
				worldBabyCount += babyCount;
				worldChildCount += childCount;
				worldAdultCount += adultCount;
				stringBuilder = message;
				StringBuilder stringBuilder2 = stringBuilder;
				appendInterpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(8, 4, stringBuilder);
				appendInterpolatedStringHandler.AppendLiteral("  ");
				appendInterpolatedStringHandler.AppendFormatted<string>(name, 5);
				appendInterpolatedStringHandler.AppendLiteral(": ");
				appendInterpolatedStringHandler.AppendFormatted<int>(babyCount, 3);
				appendInterpolatedStringHandler.AppendLiteral(", ");
				appendInterpolatedStringHandler.AppendFormatted<int>(childCount, 3);
				appendInterpolatedStringHandler.AppendLiteral(", ");
				appendInterpolatedStringHandler.AppendFormatted<int>(adultCount, 3);
				stringBuilder2.AppendLine(ref appendInterpolatedStringHandler);
			}
			foreach (KeyValuePair<short, CivilianSettlement> keyValuePair2 in this._civilianSettlements)
			{
				short num;
				CivilianSettlement civilianSettlement2;
				keyValuePair2.Deconstruct(out num, out civilianSettlement2);
				CivilianSettlement civilianSettlement = civilianSettlement2;
				short randomNameId = civilianSettlement.GetRandomNameId();
				bool flag = randomNameId >= 0;
				string name2;
				if (flag)
				{
					name2 = LocalTownNames.Instance.TownNameCore[(int)randomNameId].Name;
				}
				else
				{
					MapBlockData block2 = DomainManager.Map.GetBlock(civilianSettlement.GetLocation()).GetRootBlock();
					name2 = MapBlock.Instance[block2.TemplateId].Name;
				}
				ValueTuple<int, int, int> ageStats2 = OrganizationDomain.GetAgeStats(civilianSettlement.GetMembers(), ages);
				int babyCount2 = ageStats2.Item1;
				int childCount2 = ageStats2.Item2;
				int adultCount2 = ageStats2.Item3;
				worldBabyCount += babyCount2;
				worldChildCount += childCount2;
				worldAdultCount += adultCount2;
				stringBuilder = message;
				StringBuilder stringBuilder3 = stringBuilder;
				appendInterpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(8, 4, stringBuilder);
				appendInterpolatedStringHandler.AppendLiteral("  ");
				appendInterpolatedStringHandler.AppendFormatted<string>(name2, 5);
				appendInterpolatedStringHandler.AppendLiteral(": ");
				appendInterpolatedStringHandler.AppendFormatted<int>(babyCount2, 3);
				appendInterpolatedStringHandler.AppendLiteral(", ");
				appendInterpolatedStringHandler.AppendFormatted<int>(childCount2, 3);
				appendInterpolatedStringHandler.AppendLiteral(", ");
				appendInterpolatedStringHandler.AppendFormatted<int>(adultCount2, 3);
				stringBuilder3.AppendLine(ref appendInterpolatedStringHandler);
			}
			int totalCount = worldBabyCount + worldChildCount + worldAdultCount;
			float babyPercent = (float)worldBabyCount * 100f / (float)totalCount;
			float childPercent = (float)worldChildCount * 100f / (float)totalCount;
			float adultPercent = (float)worldAdultCount * 100f / (float)totalCount;
			stringBuilder = message;
			StringBuilder stringBuilder4 = stringBuilder;
			appendInterpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(23, 6, stringBuilder);
			appendInterpolatedStringHandler.AppendLiteral("Total: ");
			appendInterpolatedStringHandler.AppendFormatted<int>(worldBabyCount);
			appendInterpolatedStringHandler.AppendLiteral(" (");
			appendInterpolatedStringHandler.AppendFormatted<float>(babyPercent, "N1");
			appendInterpolatedStringHandler.AppendLiteral("%)");
			appendInterpolatedStringHandler.AppendLiteral(", ");
			appendInterpolatedStringHandler.AppendFormatted<int>(worldChildCount);
			appendInterpolatedStringHandler.AppendLiteral(" (");
			appendInterpolatedStringHandler.AppendFormatted<float>(childPercent, "N1");
			appendInterpolatedStringHandler.AppendLiteral("%)");
			appendInterpolatedStringHandler.AppendLiteral(", ");
			appendInterpolatedStringHandler.AppendFormatted<int>(worldAdultCount);
			appendInterpolatedStringHandler.AppendLiteral(" (");
			appendInterpolatedStringHandler.AppendFormatted<float>(adultPercent, "N1");
			appendInterpolatedStringHandler.AppendLiteral("%)");
			stringBuilder4.AppendLine(ref appendInterpolatedStringHandler);
			OrganizationDomain.Logger.Info<StringBuilder>(message);
			Histogram histogram = new Histogram(0, 60, 20);
			histogram.Record(ages);
			Logger logger = OrganizationDomain.Logger;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(15, 2);
			defaultInterpolatedStringHandler.AppendLiteral("World ages (");
			defaultInterpolatedStringHandler.AppendFormatted<int>(ages.Count);
			defaultInterpolatedStringHandler.AppendLiteral("):\n");
			defaultInterpolatedStringHandler.AppendFormatted(histogram.GetTextGraph(100));
			logger.Info(defaultInterpolatedStringHandler.ToStringAndClear());
		}

		// Token: 0x060046BD RID: 18109 RVA: 0x00278EEC File Offset: 0x002770EC
		[DomainMethod]
		public short GetOrganizationTemplateIdOfTaiwuLocation()
		{
			Location taiwuLocation = DomainManager.Taiwu.GetTaiwu().GetLocation();
			bool flag = !taiwuLocation.IsValid();
			short result;
			if (flag)
			{
				result = -1;
			}
			else
			{
				MapBlockData taiwuBlockData = DomainManager.Map.GetBlockData(taiwuLocation.AreaId, taiwuLocation.BlockId);
				Settlement settlement = this.GetSettlementByLocation(taiwuBlockData.GetRootBlock().GetLocation());
				bool flag2 = settlement == null;
				if (flag2)
				{
					result = -1;
				}
				else
				{
					result = (short)settlement.GetOrgTemplateId();
				}
			}
			return result;
		}

		// Token: 0x060046BE RID: 18110 RVA: 0x00278F64 File Offset: 0x00277164
		private void InitializeSettlementsCache()
		{
			this._settlements = new Dictionary<short, Settlement>();
			this._locationSettlements = new Dictionary<Location, Settlement>();
			int orgTemplateCount = OrganizationDomain.CalcOrgTemplateCount();
			this._orgTemplateId2Settlements = new List<Settlement>[orgTemplateCount];
			foreach (KeyValuePair<short, Sect> keyValuePair in this._sects)
			{
				short num;
				Sect sect2;
				keyValuePair.Deconstruct(out num, out sect2);
				Sect sect = sect2;
				this.AddSettlementCache(sect);
			}
			foreach (KeyValuePair<short, CivilianSettlement> keyValuePair2 in this._civilianSettlements)
			{
				short num;
				CivilianSettlement civilianSettlement2;
				keyValuePair2.Deconstruct(out num, out civilianSettlement2);
				CivilianSettlement civilianSettlement = civilianSettlement2;
				this.AddSettlementCache(civilianSettlement);
			}
		}

		// Token: 0x060046BF RID: 18111 RVA: 0x0027904C File Offset: 0x0027724C
		private void InitializeSortedMembersCache(DataContext context, DataUid dataUid)
		{
			this.UpdateSettlementCacheInfo();
			GameDataBridge.RemovePostDataModificationHandler(dataUid, "InitializeSortedMembersCache");
		}

		// Token: 0x060046C0 RID: 18112 RVA: 0x00279064 File Offset: 0x00277264
		private void CreateRelationWithAllSettlementMembers(DataContext context, GameData.Domains.Character.Character character, OrgMemberCollection members)
		{
			for (sbyte grade = 0; grade < 9; grade += 1)
			{
				foreach (int orgMemberId in members.GetMembers(grade))
				{
					GameData.Domains.Character.Character orgMember = DomainManager.Character.GetElement_Objects(orgMemberId);
					DomainManager.Character.TryCreateGeneralRelation(context, character, orgMember);
				}
			}
		}

		// Token: 0x060046C1 RID: 18113 RVA: 0x002790E8 File Offset: 0x002772E8
		private static int CalcOrgTemplateCount()
		{
			sbyte maxOrgTemplateId = -1;
			foreach (OrganizationItem item in ((IEnumerable<OrganizationItem>)Organization.Instance))
			{
				bool flag = item.TemplateId > maxOrgTemplateId;
				if (flag)
				{
					maxOrgTemplateId = item.TemplateId;
				}
			}
			return (int)(maxOrgTemplateId + 1);
		}

		// Token: 0x060046C2 RID: 18114 RVA: 0x00279150 File Offset: 0x00277350
		private void AddSettlementCache(Settlement settlement)
		{
			this._locationSettlements.Add(settlement.GetLocation(), settlement);
			this._settlements.Add(settlement.GetId(), settlement);
			sbyte orgTemplateId = settlement.GetOrgTemplateId();
			List<Settlement> settlements = this._orgTemplateId2Settlements[(int)orgTemplateId];
			bool flag = settlements == null;
			if (flag)
			{
				settlements = new List<Settlement>();
				this._orgTemplateId2Settlements[(int)orgTemplateId] = settlements;
			}
			settlements.Add(settlement);
		}

		// Token: 0x060046C3 RID: 18115 RVA: 0x002791B8 File Offset: 0x002773B8
		private void RemoveSettlementCache(Settlement settlement)
		{
			this._settlements.Remove(settlement.GetId());
			List<Settlement> settlements = this._orgTemplateId2Settlements[(int)settlement.GetOrgTemplateId()];
			if (settlements != null)
			{
				settlements.Remove(settlement);
			}
		}

		// Token: 0x060046C4 RID: 18116 RVA: 0x002791F4 File Offset: 0x002773F4
		private void InitializeSettlementCharactersCache()
		{
			this._settlementCharacters = new Dictionary<int, SettlementCharacter>();
			foreach (KeyValuePair<int, SectCharacter> keyValuePair in this._sectCharacters)
			{
				int num;
				SectCharacter sectCharacter;
				keyValuePair.Deconstruct(out num, out sectCharacter);
				int charId = num;
				SectCharacter settlementChar = sectCharacter;
				this._settlementCharacters.Add(charId, settlementChar);
			}
			foreach (KeyValuePair<int, CivilianSettlementCharacter> keyValuePair2 in this._civilianSettlementCharacters)
			{
				int num;
				CivilianSettlementCharacter civilianSettlementCharacter;
				keyValuePair2.Deconstruct(out num, out civilianSettlementCharacter);
				int charId2 = num;
				CivilianSettlementCharacter settlementChar2 = civilianSettlementCharacter;
				this._settlementCharacters.Add(charId2, settlementChar2);
			}
		}

		// Token: 0x060046C5 RID: 18117 RVA: 0x002792D4 File Offset: 0x002774D4
		private static void CheckPrincipalMembersAmount(sbyte orgTemplateId, sbyte grade, OrgMemberCollection members)
		{
			OrganizationMemberItem config = OrganizationDomain.GetOrgMemberConfig(orgTemplateId, grade);
			bool flag = !config.RestrictPrincipalAmount;
			if (!flag)
			{
				int principalAmount = 0;
				HashSet<int> gradeMembers = members.GetMembers(grade);
				foreach (int charId in gradeMembers)
				{
					GameData.Domains.Character.Character character;
					bool flag2 = DomainManager.Character.TryGetElement_Objects(charId, out character) && character.GetOrganizationInfo().Principal;
					if (flag2)
					{
						principalAmount++;
					}
				}
				bool flag3 = principalAmount > (int)config.Amount;
				if (flag3)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(55, 1);
					defaultInterpolatedStringHandler.AppendLiteral("The number of principal members exceeds the max limit: ");
					defaultInterpolatedStringHandler.AppendFormatted<short>(config.TemplateId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
			}
		}

		// Token: 0x060046C6 RID: 18118 RVA: 0x002793B0 File Offset: 0x002775B0
		private short SetRandomSectMentor(DataContext context, int charId, OrganizationInfo orgInfo, OrgMemberCollection sectMembers, sbyte mentorGrade)
		{
			bool flag = mentorGrade < 0;
			short result;
			if (flag)
			{
				result = -1;
			}
			else
			{
				HashSet<int> mentorIds = DomainManager.Character.GetRelatedCharIds(charId, 2048);
				foreach (int mentorId in mentorIds)
				{
					GameData.Domains.Character.Character mentor;
					bool flag2 = !DomainManager.Character.TryGetElement_Objects(mentorId, out mentor);
					if (!flag2)
					{
						OrganizationInfo mentorOrgInfo = mentor.GetOrganizationInfo();
						bool flag3 = mentorOrgInfo.SettlementId == orgInfo.SettlementId && mentorOrgInfo.Grade >= mentorGrade;
						if (flag3)
						{
							MonasticTitle monasticTitle = mentor.GetMonasticTitle();
							return monasticTitle.SeniorityId;
						}
					}
				}
				while (mentorGrade < 9)
				{
					HashSet<int> gradeCharIds = sectMembers.GetMembers(mentorGrade);
					bool flag4 = gradeCharIds.Count <= 0;
					if (!flag4)
					{
						short maxInfluencePower = short.MinValue;
						int mentorId2 = -1;
						foreach (int gradeCharId in gradeCharIds)
						{
							GameData.Domains.Character.Character character = DomainManager.Character.GetElement_Objects(gradeCharId);
							bool flag5 = character.GetKidnapperId() >= 0;
							if (!flag5)
							{
								bool flag6 = character.GetAgeGroup() != 2;
								if (!flag6)
								{
									SettlementCharacter settlementCharacter = this.GetSettlementCharacter(gradeCharId);
									short influencePower = settlementCharacter.GetInfluencePower();
									bool flag7 = maxInfluencePower < influencePower;
									if (flag7)
									{
										maxInfluencePower = influencePower;
										mentorId2 = gradeCharId;
									}
								}
							}
						}
						bool flag8 = mentorId2 < 0;
						if (!flag8)
						{
							bool flag9 = !RelationTypeHelper.AllowAddingMentorRelation(charId, mentorId2);
							if (!flag9)
							{
								DomainManager.Character.AddRelation(context, charId, mentorId2, 2048, int.MinValue);
								GameData.Domains.Character.Character mentor2 = DomainManager.Character.GetElement_Objects(mentorId2);
								MonasticTitle monasticTitle2 = mentor2.GetMonasticTitle();
								return monasticTitle2.SeniorityId;
							}
						}
					}
					mentorGrade += 1;
				}
				result = -1;
			}
			return result;
		}

		// Token: 0x060046C7 RID: 18119 RVA: 0x002795C4 File Offset: 0x002777C4
		private static bool CheckConditionOfBecomingSectMonk(DataContext context, GameData.Domains.Character.Character character, OrganizationMemberItem orgMemberCfg)
		{
			byte monkType = character.GetMonkType();
			bool flag = character.GetTemplateId() == 625;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				bool flag2 = monkType == 0;
				if (flag2)
				{
					result = true;
				}
				else
				{
					bool flag3 = (monkType & 128) == 0;
					if (flag3)
					{
						character.SetMonkType(0, context);
						result = true;
					}
					else
					{
						bool flag4 = monkType != orgMemberCfg.MonkType;
						if (flag4)
						{
							DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(62, 2);
							defaultInterpolatedStringHandler.AppendLiteral("Monk type of character is not compatible with organization: ");
							defaultInterpolatedStringHandler.AppendFormatted<byte>(monkType);
							defaultInterpolatedStringHandler.AppendLiteral(", ");
							defaultInterpolatedStringHandler.AppendFormatted<short>(orgMemberCfg.TemplateId);
							throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
						}
						bool flag5 = !DomainManager.Character.IsTemporaryIntelligentCharacter(character.GetId());
						if (flag5)
						{
							DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(58, 1);
							defaultInterpolatedStringHandler.AppendLiteral("Character.Character is not TemporaryIntelligentCharacter: ");
							defaultInterpolatedStringHandler.AppendFormatted<short>(character.GetTemplateId());
							throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
						}
						result = false;
					}
				}
			}
			return result;
		}

		// Token: 0x060046C8 RID: 18120 RVA: 0x002796CC File Offset: 0x002778CC
		private static void BecomeSectMonkInternal(DataContext context, GameData.Domains.Character.Character character, Sect sect, OrganizationMemberItem orgMemberCfg, short mentorSeniorityId)
		{
			bool flag = orgMemberCfg.MonkType == 0;
			if (flag)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(31, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Sect member ");
				defaultInterpolatedStringHandler.AppendFormatted<short>(orgMemberCfg.TemplateId);
				defaultInterpolatedStringHandler.AppendLiteral(" cannot become monk");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			MonasticTitle monasticTitle = CharacterDomain.CreateSectMemberMonasticTitle(context, sect, mentorSeniorityId);
			character.SetMonasticTitle(monasticTitle, context);
			character.SetMonkType(orgMemberCfg.MonkType, context);
			bool flag2 = orgMemberCfg.MonkType == 130;
			if (flag2)
			{
				AvatarData avatar = character.GetAvatar();
				avatar.ResetGrowableElementShowingAbility(0);
				character.SetAvatar(avatar, context);
			}
		}

		// Token: 0x060046C9 RID: 18121 RVA: 0x00279778 File Offset: 0x00277978
		private static void TrySecularize(DataContext context, GameData.Domains.Character.Character character)
		{
			byte monkType = character.GetMonkType();
			bool flag = (monkType & 128) > 0;
			if (flag)
			{
				bool flag2 = character.GetMonkType() == 130;
				if (flag2)
				{
					AvatarData avatar = character.GetAvatar();
					avatar.SetGrowableElementShowingAbility(0);
					avatar.ResetGrowableElementShowingState(0);
					character.SetAvatar(avatar, context);
					DomainManager.Character.InitializeAvatarElementGrowthProgress(context, character.GetId(), 0);
				}
				character.SetMonkType(0, context);
				character.SetMonasticTitle(new MonasticTitle(-1, -1), context);
			}
		}

		// Token: 0x060046CA RID: 18122 RVA: 0x002797FC File Offset: 0x002779FC
		public void TryDowngradeDeputySpouses(DataContext context, int charId, OrganizationInfo orgInfo)
		{
			OrganizationMemberItem orgMemberConfig = OrganizationDomain.GetOrgMemberConfig(orgInfo);
			bool flag = orgMemberConfig.DeputySpouseDowngrade < 0;
			if (!flag)
			{
				HashSet<int> spouseIds = DomainManager.Character.GetRelatedCharIds(charId, 1024);
				foreach (int spouseId in spouseIds)
				{
					GameData.Domains.Character.Character spouseChar;
					bool flag2 = !DomainManager.Character.TryGetElement_Objects(spouseId, out spouseChar);
					if (!flag2)
					{
						OrganizationInfo spouseOrgInfo = spouseChar.GetOrganizationInfo();
						bool principal = spouseOrgInfo.Principal;
						if (!principal)
						{
							bool flag3 = spouseOrgInfo.SettlementId != orgInfo.SettlementId;
							if (!flag3)
							{
								bool flag4 = spouseOrgInfo.Grade <= orgMemberConfig.DeputySpouseDowngrade;
								if (!flag4)
								{
									sbyte targetGrade = orgMemberConfig.DeputySpouseDowngrade;
									bool flag5 = targetGrade < 0;
									if (flag5)
									{
										targetGrade = orgMemberConfig.GetRejoinGrade();
									}
									this.ChangeGrade(context, spouseChar, targetGrade, true);
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x060046CB RID: 18123 RVA: 0x00279908 File Offset: 0x00277B08
		public void TryDowngradeDeputySpouse(DataContext context, int charId, OrganizationInfo charOrgInfo, int spouseId)
		{
			GameData.Domains.Character.Character spouseChar;
			bool flag = !DomainManager.Character.TryGetElement_Objects(spouseId, out spouseChar);
			if (!flag)
			{
				OrganizationInfo spouseOrgInfo = spouseChar.GetOrganizationInfo();
				bool principal = spouseOrgInfo.Principal;
				if (!principal)
				{
					OrganizationMemberItem orgMemberConfig = OrganizationDomain.GetOrgMemberConfig(charOrgInfo);
					bool flag2 = spouseOrgInfo.SettlementId != charOrgInfo.SettlementId;
					if (!flag2)
					{
						bool flag3 = spouseOrgInfo.Grade <= orgMemberConfig.DeputySpouseDowngrade;
						if (!flag3)
						{
							sbyte targetGrade = orgMemberConfig.DeputySpouseDowngrade;
							bool flag4 = targetGrade < 0;
							if (flag4)
							{
								targetGrade = orgMemberConfig.GetRejoinGrade();
							}
							this.ChangeGrade(context, spouseChar, targetGrade, true);
						}
					}
				}
			}
		}

		// Token: 0x060046CC RID: 18124 RVA: 0x002799A0 File Offset: 0x00277BA0
		private unsafe static ValueTuple<int, int, int> GetAgeStats(OrgMemberCollection members, List<int> ages)
		{
			IntPtr intPtr = stackalloc byte[(UIntPtr)12];
			initblk(intPtr, 0, 12);
			int* counts = intPtr;
			List<int> charIds = new List<int>();
			members.GetAllMembers(charIds);
			int i = 0;
			int count = charIds.Count;
			while (i < count)
			{
				int charId = charIds[i];
				GameData.Domains.Character.Character character = DomainManager.Character.GetElement_Objects(charId);
				short age = character.GetCurrAge();
				sbyte ageGroup = AgeGroup.GetAgeGroup(age);
				counts[ageGroup]++;
				ages.Add((int)age);
				i++;
			}
			return new ValueTuple<int, int, int>(*counts, counts[1], counts[2]);
		}

		// Token: 0x060046CD RID: 18125 RVA: 0x00279A3C File Offset: 0x00277C3C
		public void BeginCreatingSettlements(IRandomSource randomSource)
		{
			List<short> villageNameIds = new List<short>();
			List<short> townNameIds = new List<short>();
			List<short> walledTownNameIds = new List<short>();
			LocalTownNames nameCollection = LocalTownNames.Instance;
			for (short i = nameCollection.VillageStart; i <= nameCollection.VillageEnd; i += 1)
			{
				villageNameIds.Add(nameCollection.TownNameCore[(int)i].TemplateId);
			}
			for (short j = nameCollection.TownStart; j <= nameCollection.TownEnd; j += 1)
			{
				townNameIds.Add(nameCollection.TownNameCore[(int)j].TemplateId);
			}
			for (short k = nameCollection.WalledTownStart; k <= nameCollection.WalledTownEnd; k += 1)
			{
				walledTownNameIds.Add(nameCollection.TownNameCore[(int)k].TemplateId);
			}
			CollectionUtils.Shuffle<short>(randomSource, villageNameIds);
			CollectionUtils.Shuffle<short>(randomSource, townNameIds);
			CollectionUtils.Shuffle<short>(randomSource, walledTownNameIds);
			this._settlementCreatingInfo = new SettlementCreatingInfo(villageNameIds, townNameIds, walledTownNameIds);
		}

		// Token: 0x060046CE RID: 18126 RVA: 0x00279B33 File Offset: 0x00277D33
		public void EndCreatingSettlements(DataContext context)
		{
			this.ForceUpdateInfluencePowers(context);
			this.RecordSettlementStandardPopulations(context);
			DomainManager.World.RecordWorldStandardPopulation(context);
			DomainManager.World.UpdatePopulationRelatedData();
			this._settlementCreatingInfo = null;
			OrganizationDomain._orgInscribedCharIdMap = null;
		}

		// Token: 0x060046CF RID: 18127 RVA: 0x00279B6A File Offset: 0x00277D6A
		public bool IsCreatingSettlements()
		{
			return this._settlementCreatingInfo != null;
		}

		// Token: 0x060046D0 RID: 18128 RVA: 0x00279B78 File Offset: 0x00277D78
		public void CreateEmptySects(DataContext context)
		{
			short index = 0;
			foreach (OrganizationItem orgCfg in ((IEnumerable<OrganizationItem>)Organization.Instance))
			{
				bool isSect = orgCfg.IsSect;
				if (isSect)
				{
					short settlementId = this.GenerateNextSettlementId(context);
					Sect sect = new Sect(settlementId, new Location(-1, index), orgCfg.TemplateId, context.Random);
					this.AddElement_Sects(settlementId, sect);
					this.AddSettlementCache(sect);
					index += 1;
				}
			}
		}

		// Token: 0x060046D1 RID: 18129 RVA: 0x00279C10 File Offset: 0x00277E10
		public short CreateSettlement(DataContext context, Location location, sbyte orgTemplateId)
		{
			short settlementId = this.GenerateNextSettlementId(context);
			bool flag = OrganizationDomain.IsSect(orgTemplateId);
			if (flag)
			{
				Sect sect = new Sect(settlementId, location, orgTemplateId, context.Random);
				this.AddElement_Sects(settlementId, sect);
				this.SetLargeSectFavorabilities(this._largeSectFavorabilities, context);
				this.AddSettlementCache(sect);
				OrganizationDomain.CreateSettlementMembers(context, sect);
			}
			else
			{
				CivilianSettlement civilianSettlement = new CivilianSettlement(settlementId, location, orgTemplateId, this._settlementCreatingInfo, context.Random);
				this.AddElement_CivilianSettlements(settlementId, civilianSettlement);
				this.AddSettlementCache(civilianSettlement);
				OrganizationDomain.CreateSettlementMembers(context, civilianSettlement);
			}
			return settlementId;
		}

		// Token: 0x060046D2 RID: 18130 RVA: 0x00279CA4 File Offset: 0x00277EA4
		[DomainMethod]
		public void SetInscribedCharactersForCreation(DataContext context, List<InscribedCharacterKey> inscribedCharList)
		{
			OrganizationDomain._orgInscribedCharIdMap = new Dictionary<sbyte, List<InscribedCharacter>>();
			bool flag = inscribedCharList == null || inscribedCharList.Count <= 0;
			if (!flag)
			{
				IRandomSource random = context.Random;
				int totalCount = inscribedCharList.Count;
				if (!true)
				{
				}
				int num;
				if (totalCount != 1)
				{
					if (totalCount != 2)
					{
						num = totalCount / 3;
					}
					else
					{
						num = 1;
					}
				}
				else
				{
					num = random.Next(2);
				}
				if (!true)
				{
				}
				int civilianCount = num;
				CollectionUtils.Shuffle<InscribedCharacterKey>(random, inscribedCharList);
				Logger logger = OrganizationDomain.Logger;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(62, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Inscribed Character to settlements: ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(civilianCount);
				defaultInterpolatedStringHandler.AppendLiteral(" civilians / ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(totalCount - civilianCount);
				defaultInterpolatedStringHandler.AppendLiteral(" sect members");
				logger.Info(defaultInterpolatedStringHandler.ToStringAndClear());
				for (int i = 0; i < civilianCount; i++)
				{
					InscribedCharacterKey inscribedCharKey = inscribedCharList[i];
					InscribedCharacter inscribedChar = DomainManager.Global.GetElement_InscribedCharacters(inscribedCharKey);
					sbyte orgTemplateId = (sbyte)(21 + random.Next(15));
					OrganizationDomain.<SetInscribedCharactersForCreation>g__AddOrgInscribedCharacter|97_0(orgTemplateId, inscribedChar);
				}
				for (int j = civilianCount; j < totalCount; j++)
				{
					InscribedCharacterKey inscribedCharKey2 = inscribedCharList[j];
					InscribedCharacter inscribedChar2 = DomainManager.Global.GetElement_InscribedCharacters(inscribedCharKey2);
					sbyte orgTemplateId2 = this.GetBestMatchingOrgTemplateId(random, inscribedChar2.Gender, inscribedChar2.BaseCombatSkillQualifications, inscribedChar2.BaseLifeSkillQualifications);
					OrganizationDomain.<SetInscribedCharactersForCreation>g__AddOrgInscribedCharacter|97_0(orgTemplateId2, inscribedChar2);
				}
			}
		}

		// Token: 0x060046D3 RID: 18131 RVA: 0x00279E14 File Offset: 0x00278014
		private unsafe sbyte GetBestMatchingOrgTemplateId(IRandomSource random, sbyte gender, CombatSkillShorts combatSkillQualifications, LifeSkillShorts lifeSkillQualifications)
		{
			int bestScore = int.MinValue;
			Span<sbyte> span = new Span<sbyte>(stackalloc byte[(UIntPtr)15], 15);
			SpanList<sbyte> bestScoreSectIds = span;
			foreach (OrganizationItem orgConfig in ((IEnumerable<OrganizationItem>)Organization.Instance))
			{
				bool flag = !orgConfig.IsSect;
				if (!flag)
				{
					bool flag2 = orgConfig.GenderRestriction != -1 && orgConfig.GenderRestriction != gender;
					if (!flag2)
					{
						OrganizationMemberItem highestOrgMemberConfig = OrganizationDomain.GetOrgMemberConfig(orgConfig.TemplateId, 8);
						int score = 0;
						for (sbyte combatSkillType = 0; combatSkillType < 14; combatSkillType += 1)
						{
							short adjust = highestOrgMemberConfig.CombatSkillsAdjust[(int)combatSkillType];
							bool flag3 = adjust < 0;
							if (!flag3)
							{
								score += (int)(*combatSkillQualifications[(int)combatSkillType] * adjust * 3);
							}
						}
						for (sbyte lifeSkillType = 0; lifeSkillType < 16; lifeSkillType += 1)
						{
							short adjust2 = highestOrgMemberConfig.LifeSkillsAdjust[(int)lifeSkillType];
							bool flag4 = adjust2 < 0;
							if (!flag4)
							{
								score += (int)(*lifeSkillQualifications[(int)lifeSkillType] * adjust2);
							}
						}
						bool flag5 = score > bestScore;
						if (flag5)
						{
							bestScoreSectIds.Clear();
							bestScoreSectIds.Add(orgConfig.TemplateId);
							bestScore = score;
						}
						else
						{
							bool flag6 = score == bestScore;
							if (flag6)
							{
								bestScoreSectIds.Add(orgConfig.TemplateId);
							}
						}
					}
				}
			}
			return bestScoreSectIds.GetRandom(random);
		}

		// Token: 0x060046D4 RID: 18132 RVA: 0x00279FB4 File Offset: 0x002781B4
		private short GenerateNextSettlementId(DataContext context)
		{
			short settlementId = this._nextSettlementId;
			this._nextSettlementId += 1;
			bool flag = (ushort)this._nextSettlementId > 32767;
			if (flag)
			{
				this._nextSettlementId = 0;
			}
			this.SetNextSettlementId(this._nextSettlementId, context);
			return settlementId;
		}

		// Token: 0x060046D5 RID: 18133 RVA: 0x0027A004 File Offset: 0x00278204
		private static void CreateSettlementMembers(DataContext context, Settlement settlement)
		{
			sbyte orgTemplateId = settlement.GetOrgTemplateId();
			short settlementId = settlement.GetId();
			Location location = settlement.GetLocation();
			OrgMemberCollection members = settlement.GetMembers();
			List<InscribedCharacter> charList;
			bool flag = OrganizationDomain._orgInscribedCharIdMap.TryGetValue(orgTemplateId, out charList);
			if (flag)
			{
				if (OrganizationDomain._stringBuilder == null)
				{
					OrganizationDomain._stringBuilder = new StringBuilder();
				}
				OrganizationDomain._stringBuilder.Clear();
				OrganizationDomain._stringBuilder.Append("Creating inscribed characters at ");
				OrganizationDomain._stringBuilder.AppendLine(settlement.GetNameRelatedData().GetName());
				foreach (InscribedCharacter inscribedChar in charList)
				{
					OrganizationInfo targetOrgInfo = OrganizationDomain.GetInscribedCharTargetOrgInfo(context.Random, inscribedChar, settlement);
					GameData.Domains.Character.Character character = DomainManager.Character.CreateIntelligentCharacterFromInscription(context, inscribedChar, targetOrgInfo);
					DomainManager.Character.CompleteCreatingCharacter(character.GetId());
					OrganizationDomain._stringBuilder.Append('\t');
					OrganizationDomain._stringBuilder.AppendLine(character.ToString());
				}
				OrganizationDomain.Logger.Info(OrganizationDomain._stringBuilder.ToString());
			}
			OrganizationItem orgConfig = Organization.Instance[orgTemplateId];
			bool flag2 = orgConfig.Population <= 0;
			if (!flag2)
			{
				sbyte mapStateTemplateId = DomainManager.Map.GetStateTemplateIdByAreaId(location.AreaId);
				bool flag3 = mapStateTemplateId <= 0;
				if (flag3)
				{
					mapStateTemplateId = DomainManager.World.GetTaiwuVillageStateTemplateId();
				}
				List<short> blockIds = new List<short>();
				DomainManager.Map.GetSettlementBlocks(location.AreaId, location.BlockId, blockIds);
				List<short> nearbyBlockIds = new List<short>();
				DomainManager.Map.GetSettlementBlocksAndAffiliatedBlocks(location.AreaId, location.BlockId, nearbyBlockIds);
				SettlementMembersCreationInfo info = new SettlementMembersCreationInfo(orgTemplateId, settlementId, mapStateTemplateId, location.AreaId, blockIds, nearbyBlockIds);
				int worldPopulationFactor = DomainManager.World.GetWorldPopulationFactor();
				sbyte maxGrade = (orgTemplateId != 16) ? 8 : 7;
				for (sbyte grade = maxGrade; grade >= 0; grade -= 1)
				{
					short orgMemberId = orgConfig.Members[(int)grade];
					OrganizationMemberItem orgMemberConfig = OrganizationMember.Instance[orgMemberId];
					bool flag4 = orgMemberConfig.Amount <= 0;
					if (!flag4)
					{
						info.CoreMemberConfig = orgMemberConfig;
						HashSet<int> existingMembers = members.GetMembers(grade);
						int coreMembersAmount = (int)orgMemberConfig.Amount;
						bool flag5 = !orgMemberConfig.RestrictPrincipalAmount;
						if (flag5)
						{
							coreMembersAmount = Math.Max(1, coreMembersAmount * worldPopulationFactor / 125);
						}
						else
						{
							bool flag6 = existingMembers.Count > 0;
							if (flag6)
							{
								coreMembersAmount -= existingMembers.Count;
							}
						}
						for (int i = 0; i < coreMembersAmount; i++)
						{
							OrganizationDomain.CreateCoreCharacter(context, info);
							OrganizationDomain.CreateBrothersAndSisters(context, info);
							OrganizationDomain.CreateSpouseAndChildren(context, info);
							info.CompleteCreatingCharacters();
						}
					}
				}
			}
		}

		// Token: 0x060046D6 RID: 18134 RVA: 0x0027A2DC File Offset: 0x002784DC
		private static OrganizationInfo GetInscribedCharTargetOrgInfo(IRandomSource random, InscribedCharacter inscribedChar, Settlement settlement)
		{
			OrgMemberCollection members = settlement.GetMembers();
			sbyte orgTemplateId = settlement.GetOrgTemplateId();
			bool isSect = settlement is Sect;
			sbyte mainAttrGrade = CharacterCreation.GetMainAttributeGrade(inscribedChar.BaseMainAttributes.GetSum());
			sbyte combatSkillGrade = CharacterCreation.GetCombatSkillQualificationGrade(inscribedChar.BaseCombatSkillQualifications.GetSum());
			sbyte lifeSkillGrade = CharacterCreation.GetLifeSkillQualificationGrade(inscribedChar.BaseLifeSkillQualifications.GetSum());
			sbyte grade = (sbyte)Math.Clamp((int)((mainAttrGrade + combatSkillGrade + lifeSkillGrade) / 3), 0, 8);
			bool flag = isSect;
			if (flag)
			{
				for (grade = (sbyte)Math.Clamp(random.Next((int)(grade - 2), (int)(grade + 1)), 0, 8); grade >= 0; grade -= 1)
				{
					OrganizationMemberItem orgMemberCfg = OrganizationDomain.GetOrgMemberConfig(orgTemplateId, grade);
					bool flag2 = orgMemberCfg.RestrictPrincipalAmount && members.GetMembers(grade).Count >= (int)orgMemberCfg.Amount;
					if (!flag2)
					{
						bool flag3 = orgMemberCfg.Gender != -1 && orgMemberCfg.Gender != inscribedChar.Gender;
						if (!flag3)
						{
							break;
						}
					}
				}
			}
			else
			{
				OrganizationMemberItem orgMemberCfg2 = OrganizationDomain.GetOrgMemberConfig(orgTemplateId, grade);
				bool flag4 = orgMemberCfg2.RestrictPrincipalAmount && members.GetMembers(grade).Count >= (int)orgMemberCfg2.Amount;
				if (flag4)
				{
					grade = Math.Max(0, orgMemberCfg2.ChildGrade);
				}
				else
				{
					bool flag5 = orgMemberCfg2.Gender != -1 && orgMemberCfg2.Gender != inscribedChar.Gender;
					if (flag5)
					{
						grade = 0;
					}
				}
			}
			return new OrganizationInfo(orgTemplateId, grade, true, settlement.GetId());
		}

		// Token: 0x060046D7 RID: 18135 RVA: 0x0027A470 File Offset: 0x00278670
		public static void CreateCoreCharacter(DataContext context, SettlementMembersCreationInfo info)
		{
			IRandomSource random = context.Random;
			Genome.CreateRandom(random, ref info.CoreMotherGenome);
			Genome.CreateRandom(random, ref info.CoreFatherGenome);
			sbyte gender = (info.CoreMemberConfig.Gender == -1) ? Gender.GetRandom(random) : info.CoreMemberConfig.Gender;
			short charTemplateId = OrganizationDomain.GetCharacterTemplateId(info.OrgTemplateId, info.MapStateTemplateId, gender);
			short initialAge = OrganizationDomain.GetInitialAge(info.CoreMemberConfig);
			bool flag = initialAge >= 0;
			short age;
			if (flag)
			{
				int variationRange = (int)(initialAge / 4);
				age = (short)((int)initialAge + random.Next(-variationRange, variationRange + 1));
			}
			else
			{
				age = GameData.Domains.Character.Character.GenerateRandomAge(random);
			}
			short mapBlockId = info.CoreMemberConfig.CanStroll ? info.NearbyBlockIds[random.Next(info.NearbyBlockIds.Count)] : info.BlockIds[random.Next(info.BlockIds.Count)];
			sbyte grade = (info.OrgTemplateId != 16) ? info.CoreMemberConfig.Grade : 0;
			OrganizationInfo orgInfo = new OrganizationInfo(info.OrgTemplateId, grade, true, info.SettlementId);
			IntelligentCharacterCreationInfo charCreationInfo = new IntelligentCharacterCreationInfo(new Location(info.AreaId, mapBlockId), orgInfo, charTemplateId)
			{
				Age = age,
				SpecifyGenome = true
			};
			Genome.Inherit(random, ref info.CoreMotherGenome, ref info.CoreFatherGenome, ref charCreationInfo.Genome);
			GameData.Domains.Character.Character coreChar = DomainManager.Character.CreateIntelligentCharacter(context, ref charCreationInfo);
			int charId = coreChar.GetId();
			info.CreatedCharIds.Add(charId);
			bool isInfertile = coreChar.GetFertility() <= 0;
			bool flag2 = age >= 16 && !isInfertile && random.CheckPercentProb(10);
			if (flag2)
			{
				coreChar.LoseVirginity(context);
			}
			info.CoreCharId = charId;
			info.CoreChar = coreChar;
			info.IsCoreCharInfertile = isInfertile;
			bool flag3 = coreChar.GetGender() == 0;
			if (flag3)
			{
				info.CoreMotherAvatar = coreChar.GetAvatar();
				info.CoreFatherAvatar = new AvatarData(info.CoreMotherAvatar);
				info.CoreFatherAvatar.ChangeGender(1);
				info.CoreFatherAvatar.ChangeBodyType(BodyType.GetRandom(random));
			}
			else
			{
				info.CoreFatherAvatar = coreChar.GetAvatar();
				info.CoreMotherAvatar = new AvatarData(info.CoreFatherAvatar);
				info.CoreMotherAvatar.ChangeGender(0);
				info.CoreMotherAvatar.ChangeBodyType(BodyType.GetRandom(random));
			}
		}

		// Token: 0x060046D8 RID: 18136 RVA: 0x0027A6D0 File Offset: 0x002788D0
		private unsafe static void CreateBrothersAndSisters(DataContext context, SettlementMembersCreationInfo info)
		{
			IRandomSource random = context.Random;
			int brothersAndSistersCount = RedzenHelper.NormalDistribute(random, 1f, 1f, 1, 3);
			bool flag = info.OrgTemplateId == 16;
			if (flag)
			{
				brothersAndSistersCount = Math.Min(brothersAndSistersCount, 2);
			}
			sbyte brotherGrade = info.CoreMemberConfig.BrotherGrade;
			short orgMemberId = Organization.Instance[info.OrgTemplateId].Members[(int)brotherGrade];
			OrganizationMemberItem orgMemberConfig = OrganizationMember.Instance[orgMemberId];
			ValueTuple<int, int, ushort>* pBrothersAndSistersInfo = stackalloc ValueTuple<int, int, ushort>[checked(unchecked((UIntPtr)brothersAndSistersCount) * (UIntPtr)sizeof(ValueTuple<int, int, ushort>))];
			*pBrothersAndSistersInfo = new ValueTuple<int, int, ushort>(info.CoreCharId, info.CoreChar.GetBirthDate(), 4);
			FullName coreCharFullName = info.CoreChar.GetFullName();
			short currAge = info.CoreChar.GetCurrAge();
			for (int i = 1; i < brothersAndSistersCount; i++)
			{
				currAge += (short)(1 + random.Next(2));
				sbyte gender = (orgMemberConfig.Gender == -1) ? Gender.GetRandom(random) : orgMemberConfig.Gender;
				short charTemplateId = OrganizationDomain.GetCharacterTemplateId(info.OrgTemplateId, info.MapStateTemplateId, gender);
				ushort relationType = random.CheckPercentProb(75) ? 4 : 512;
				short mapBlockId = orgMemberConfig.CanStroll ? info.NearbyBlockIds[random.Next(info.NearbyBlockIds.Count)] : info.BlockIds[random.Next(info.BlockIds.Count)];
				OrganizationInfo orgInfo = new OrganizationInfo(info.OrgTemplateId, brotherGrade, true, info.SettlementId);
				IntelligentCharacterCreationInfo charCreationInfo = new IntelligentCharacterCreationInfo(new Location(info.AreaId, mapBlockId), orgInfo, charTemplateId)
				{
					Age = currAge
				};
				bool flag2 = relationType == 4;
				if (flag2)
				{
					charCreationInfo.Avatar = AvatarManager.Instance.GetRandomAvatar(random, gender, false, -1, info.CoreFatherAvatar, info.CoreMotherAvatar);
					charCreationInfo.BaseAttraction = charCreationInfo.Avatar.GetBaseCharm();
					charCreationInfo.SpecifyGenome = true;
					Genome.Inherit(random, ref info.CoreMotherGenome, ref info.CoreFatherGenome, ref charCreationInfo.Genome);
					charCreationInfo.ReferenceFullName = coreCharFullName;
				}
				GameData.Domains.Character.Character character = DomainManager.Character.CreateIntelligentCharacter(context, ref charCreationInfo);
				int charId = character.GetId();
				info.CreatedCharIds.Add(charId);
				bool flag3 = currAge >= 16 && random.CheckPercentProb(10) && character.GetFertility() > 0;
				if (flag3)
				{
					character.LoseVirginity(context);
				}
				pBrothersAndSistersInfo[(IntPtr)i * (IntPtr)sizeof(ValueTuple<int, int, ushort>) / (IntPtr)sizeof(ValueTuple<int, int, ushort>)] = new ValueTuple<int, int, ushort>(charId, character.GetBirthDate(), relationType);
			}
			for (int j = 0; j < brothersAndSistersCount; j++)
			{
				ValueTuple<int, int, ushort> valueTuple = pBrothersAndSistersInfo[(IntPtr)j * (IntPtr)sizeof(ValueTuple<int, int, ushort>) / (IntPtr)sizeof(ValueTuple<int, int, ushort>)];
				int youngerCharId = valueTuple.Item1;
				int youngerBirthDate = valueTuple.Item2;
				ushort youngerRelationType = valueTuple.Item3;
				for (int k = j + 1; k < brothersAndSistersCount; k++)
				{
					ValueTuple<int, int, ushort> valueTuple2 = pBrothersAndSistersInfo[(IntPtr)k * (IntPtr)sizeof(ValueTuple<int, int, ushort>) / (IntPtr)sizeof(ValueTuple<int, int, ushort>)];
					int elderCharId = valueTuple2.Item1;
					ushort elderRelationType = valueTuple2.Item3;
					ushort relationType2 = (youngerRelationType == 4 && elderRelationType == 4) ? 4 : 512;
					DomainManager.Character.AddRelation(context, youngerCharId, elderCharId, relationType2, youngerBirthDate);
				}
			}
		}

		// Token: 0x060046D9 RID: 18137 RVA: 0x0027AA04 File Offset: 0x00278C04
		private static void CreateSpouseAndChildren(DataContext context, SettlementMembersCreationInfo info)
		{
			bool flag = info.CoreMemberConfig.ChildGrade < 0 || info.CoreChar.GetMonkType() > 0;
			if (!flag)
			{
				IRandomSource random = context.Random;
				int marriageRate = Math.Min((int)((info.CoreChar.GetCurrAge() - 20) * 10), 90);
				bool flag2 = random.CheckPercentProb(marriageRate);
				if (flag2)
				{
					bool isTaiwuVillage = info.OrgTemplateId == 16;
					OrganizationDomain.CreateSpouse(context, info);
					bool flag3 = !isTaiwuVillage && random.CheckPercentProb(10);
					if (flag3)
					{
						OrganizationDomain.CreateLover(context, info);
					}
					short coreCharFertility = info.CoreChar.GetFertility();
					short spouseCharFertility = info.SpouseChar.GetFertility();
					bool flag4 = coreCharFertility > 0 && spouseCharFertility > 0 && (int)(coreCharFertility * spouseCharFertility) > random.Next(20000);
					if (flag4)
					{
						info.CoreChar.LoseVirginity(context);
						info.SpouseChar.LoseVirginity(context);
						OrganizationDomain.CreateChildren(context, info, true);
					}
					else
					{
						bool flag5 = !isTaiwuVillage && random.CheckPercentProb((int)((info.CoreChar.GetCurrAge() - 40) * 2));
						if (flag5)
						{
							OrganizationDomain.CreateChildren(context, info, false);
						}
					}
				}
				else
				{
					bool flag6 = random.CheckPercentProb(25);
					if (flag6)
					{
						OrganizationDomain.CreateLover(context, info);
					}
				}
			}
		}

		// Token: 0x060046DA RID: 18138 RVA: 0x0027AB48 File Offset: 0x00278D48
		private static void CreateSpouse(DataContext context, SettlementMembersCreationInfo info)
		{
			IRandomSource random = context.Random;
			sbyte grade = (info.OrgTemplateId != 16) ? info.CoreChar.GetOrganizationInfo().Grade : 0;
			sbyte gender = Gender.Flip(info.CoreChar.GetGender());
			short charTemplateId = OrganizationDomain.GetCharacterTemplateId(info.OrgTemplateId, info.MapStateTemplateId, gender);
			short age = (short)Math.Max((int)info.CoreChar.GetCurrAge() + ((gender == 0) ? -1 : 1) * random.Next(16), 16);
			short mapBlockId = info.CoreMemberConfig.CanStroll ? info.NearbyBlockIds[random.Next(info.NearbyBlockIds.Count)] : info.BlockIds[random.Next(info.BlockIds.Count)];
			OrganizationInfo orgInfo = new OrganizationInfo(info.OrgTemplateId, grade, info.CoreMemberConfig.DeputySpouseDowngrade < 0, info.SettlementId);
			IntelligentCharacterCreationInfo charCreationInfo = new IntelligentCharacterCreationInfo(new Location(info.AreaId, mapBlockId), orgInfo, charTemplateId)
			{
				Age = age
			};
			GameData.Domains.Character.Character character = DomainManager.Character.CreateIntelligentCharacter(context, ref charCreationInfo);
			int charId = character.GetId();
			info.CreatedCharIds.Add(charId);
			GameData.Domains.Character.Character wife = (gender == 0) ? character : info.CoreChar;
			int marriageDate = wife.GetBirthDate() + 192;
			bool flag = !RelationTypeHelper.AllowAddingHusbandOrWifeRelation(info.CoreCharId, charId);
			if (flag)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(43, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Failed to add husband or wife relation: ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(info.CoreCharId);
				defaultInterpolatedStringHandler.AppendLiteral(" - ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(charId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			DomainManager.Character.AddRelation(context, info.CoreCharId, charId, 1024, marriageDate);
			bool flag2 = context.Random.NextBool();
			if (flag2)
			{
				DomainManager.Character.ChangeRelationType(context, info.CoreCharId, charId, 0, 16384);
			}
			bool flag3 = context.Random.NextBool();
			if (flag3)
			{
				DomainManager.Character.ChangeRelationType(context, charId, info.CoreCharId, 0, 16384);
			}
			bool flag4 = !info.IsCoreCharInfertile && random.CheckPercentProb(75) && character.GetFertility() > 0;
			if (flag4)
			{
				info.CoreChar.LoseVirginity(context);
				character.LoseVirginity(context);
				bool flag5 = random.CheckPercentProb(10);
				if (flag5)
				{
					GameData.Domains.Character.Character husband = (gender == 1) ? character : info.CoreChar;
					wife.AddFeature(context, 197, false);
					DomainManager.Character.CreatePregnantState(context, wife, husband, false);
				}
			}
			info.SpouseCharId = charId;
			info.SpouseChar = character;
		}

		// Token: 0x060046DB RID: 18139 RVA: 0x0027ADF8 File Offset: 0x00278FF8
		private static void CreateLover(DataContext context, SettlementMembersCreationInfo info)
		{
			IRandomSource random = context.Random;
			sbyte grade = info.CoreMemberConfig.BrotherGrade;
			sbyte gender = Gender.Flip(info.CoreChar.GetGender());
			short charTemplateId = OrganizationDomain.GetCharacterTemplateId(info.OrgTemplateId, info.MapStateTemplateId, gender);
			short age = (short)Math.Max((int)info.CoreChar.GetCurrAge() + ((gender == 0) ? -1 : 1) * random.Next(16), 16);
			short orgMemberId = Organization.Instance[info.OrgTemplateId].Members[(int)grade];
			OrganizationMemberItem orgMemberConfig = OrganizationMember.Instance[orgMemberId];
			short mapBlockId = orgMemberConfig.CanStroll ? info.NearbyBlockIds[random.Next(info.NearbyBlockIds.Count)] : info.BlockIds[random.Next(info.BlockIds.Count)];
			OrganizationInfo orgInfo = new OrganizationInfo(info.OrgTemplateId, grade, true, info.SettlementId);
			IntelligentCharacterCreationInfo charCreationInfo = new IntelligentCharacterCreationInfo(new Location(info.AreaId, mapBlockId), orgInfo, charTemplateId)
			{
				Age = age
			};
			GameData.Domains.Character.Character character = DomainManager.Character.CreateIntelligentCharacter(context, ref charCreationInfo);
			int charId = character.GetId();
			info.CreatedCharIds.Add(charId);
			bool flag = random.CheckPercentProb(50);
			if (flag)
			{
				bool flag2 = !RelationTypeHelper.AllowAddingAdoredRelation(info.CoreCharId, charId);
				if (flag2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(34, 2);
					defaultInterpolatedStringHandler.AppendLiteral("Failed to add adored relation: ");
					defaultInterpolatedStringHandler.AppendFormatted<int>(info.CoreCharId);
					defaultInterpolatedStringHandler.AppendLiteral(" - ");
					defaultInterpolatedStringHandler.AppendFormatted<int>(charId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				DomainManager.Character.AddRelation(context, info.CoreCharId, charId, 16384, int.MinValue);
				bool flag3 = !RelationTypeHelper.AllowAddingAdoredRelation(charId, info.CoreCharId);
				if (flag3)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(34, 2);
					defaultInterpolatedStringHandler.AppendLiteral("Failed to add adored relation: ");
					defaultInterpolatedStringHandler.AppendFormatted<int>(charId);
					defaultInterpolatedStringHandler.AppendLiteral(" - ");
					defaultInterpolatedStringHandler.AppendFormatted<int>(info.CoreCharId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				DomainManager.Character.AddRelation(context, charId, info.CoreCharId, 16384, int.MinValue);
				bool flag4 = !info.IsCoreCharInfertile && random.CheckPercentProb(20) && character.GetFertility() > 0;
				if (flag4)
				{
					info.CoreChar.LoseVirginity(context);
					character.LoseVirginity(context);
				}
			}
			else
			{
				bool flag5 = !RelationTypeHelper.AllowAddingAdoredRelation(info.CoreCharId, charId);
				if (flag5)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(34, 2);
					defaultInterpolatedStringHandler.AppendLiteral("Failed to add adored relation: ");
					defaultInterpolatedStringHandler.AppendFormatted<int>(info.CoreCharId);
					defaultInterpolatedStringHandler.AppendLiteral(" - ");
					defaultInterpolatedStringHandler.AppendFormatted<int>(charId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				DomainManager.Character.AddRelation(context, info.CoreCharId, charId, 16384, int.MinValue);
			}
		}

		// Token: 0x060046DC RID: 18140 RVA: 0x0027B0FC File Offset: 0x002792FC
		private static void CreateChildren(DataContext context, SettlementMembersCreationInfo info, bool isBloodChildren)
		{
			IRandomSource random = context.Random;
			bool flag = info.CoreChar.GetGender() == 1;
			int fatherId;
			GameData.Domains.Character.Character father;
			int motherId;
			GameData.Domains.Character.Character mother;
			short motherAge;
			if (flag)
			{
				fatherId = info.CoreCharId;
				father = info.CoreChar;
				motherId = info.SpouseCharId;
				mother = info.SpouseChar;
				motherAge = info.SpouseChar.GetCurrAge();
			}
			else
			{
				fatherId = info.SpouseCharId;
				father = info.SpouseChar;
				motherId = info.CoreCharId;
				mother = info.CoreChar;
				motherAge = info.CoreChar.GetCurrAge();
			}
			sbyte grade = info.CoreMemberConfig.ChildGrade;
			short orgMemberId = Organization.Instance[info.OrgTemplateId].Members[(int)grade];
			OrganizationMemberItem orgMemberConfig = OrganizationMember.Instance[orgMemberId];
			sbyte orgMemberGender = orgMemberConfig.Gender;
			int motherAgeAtFirstChildBirthMin = 17;
			int motherAgeAtFirstChildBirthMax = Math.Min(GlobalConfig.Instance.MaxAgeOfCreatingChar + 1, (int)motherAge);
			bool flag2 = motherAgeAtFirstChildBirthMin > motherAgeAtFirstChildBirthMax;
			if (!flag2)
			{
				int childAge = (int)motherAge - random.Next(motherAgeAtFirstChildBirthMin, motherAgeAtFirstChildBirthMax + 1);
				int childrenCount = RedzenHelper.NormalDistribute(random, 1f, 1f, 1, 3);
				for (int i = 0; i < childrenCount; i++)
				{
					int motherAgeAtChildBirth = (int)motherAge - childAge;
					bool flag3 = motherAgeAtChildBirth >= motherAgeAtFirstChildBirthMin && childAge >= 0;
					if (flag3)
					{
						sbyte gender = (orgMemberGender == -1) ? Gender.GetRandom(random) : orgMemberGender;
						short charTemplateId = OrganizationDomain.GetCharacterTemplateId(info.OrgTemplateId, info.MapStateTemplateId, gender);
						OrganizationInfo orgInfo = new OrganizationInfo(info.OrgTemplateId, grade, true, info.SettlementId);
						IntelligentCharacterCreationInfo charCreationInfo = new IntelligentCharacterCreationInfo(mother.GetLocation(), orgInfo, charTemplateId)
						{
							GrowingSectGrade = info.CoreMemberConfig.Grade,
							Age = (short)childAge
						};
						if (isBloodChildren)
						{
							charCreationInfo.MotherCharId = motherId;
							charCreationInfo.Mother = mother;
							charCreationInfo.FatherCharId = fatherId;
							charCreationInfo.ActualFatherCharId = fatherId;
							charCreationInfo.Father = father;
							charCreationInfo.ActualFather = father;
						}
						GameData.Domains.Character.Character character = DomainManager.Character.CreateIntelligentCharacter(context, ref charCreationInfo);
						int charId = character.GetId();
						info.CreatedCharIds.Add(charId);
						int birthDate = character.GetBirthDate();
						if (isBloodChildren)
						{
							bool flag4 = !RelationTypeHelper.AllowAddingBloodParentRelation(charId, fatherId);
							if (flag4)
							{
								DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(40, 2);
								defaultInterpolatedStringHandler.AppendLiteral("Failed to add blood parent relation: ");
								defaultInterpolatedStringHandler.AppendFormatted<int>(charId);
								defaultInterpolatedStringHandler.AppendLiteral(" - ");
								defaultInterpolatedStringHandler.AppendFormatted<int>(fatherId);
								throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
							}
							bool flag5 = !RelationTypeHelper.AllowAddingBloodParentRelation(charId, motherId);
							if (flag5)
							{
								DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(40, 2);
								defaultInterpolatedStringHandler.AppendLiteral("Failed to add blood parent relation: ");
								defaultInterpolatedStringHandler.AppendFormatted<int>(charId);
								defaultInterpolatedStringHandler.AppendLiteral(" - ");
								defaultInterpolatedStringHandler.AppendFormatted<int>(motherId);
								throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
							}
							DomainManager.Character.AddBloodParentRelations(context, charId, fatherId, birthDate);
							DomainManager.Character.AddBloodParentRelations(context, charId, motherId, birthDate);
						}
						else
						{
							bool flag6 = !RelationTypeHelper.AllowAddingAdoptiveParentRelation(charId, fatherId);
							if (flag6)
							{
								DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(43, 2);
								defaultInterpolatedStringHandler.AppendLiteral("Failed to add adoptive parent relation: ");
								defaultInterpolatedStringHandler.AppendFormatted<int>(charId);
								defaultInterpolatedStringHandler.AppendLiteral(" - ");
								defaultInterpolatedStringHandler.AppendFormatted<int>(fatherId);
								throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
							}
							bool flag7 = !RelationTypeHelper.AllowAddingAdoptiveParentRelation(charId, motherId);
							if (flag7)
							{
								DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(43, 2);
								defaultInterpolatedStringHandler.AppendLiteral("Failed to add adoptive parent relation: ");
								defaultInterpolatedStringHandler.AppendFormatted<int>(charId);
								defaultInterpolatedStringHandler.AppendLiteral(" - ");
								defaultInterpolatedStringHandler.AppendFormatted<int>(motherId);
								throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
							}
							DomainManager.Character.AddAdoptiveParentRelations(context, charId, fatherId, birthDate);
							DomainManager.Character.AddAdoptiveParentRelations(context, charId, motherId, birthDate);
						}
					}
					childAge -= 1 + random.Next(2);
				}
			}
		}

		// Token: 0x060046DD RID: 18141 RVA: 0x0027B4E0 File Offset: 0x002796E0
		[DomainMethod]
		public SettlementDisplayData GetDisplayData(short settlementId)
		{
			Settlement settlement = this._settlements[settlementId];
			SettlementDisplayData data = default(SettlementDisplayData);
			ValueTuple<int, int> population = settlement.GetPopulationInfo();
			data.SettlementId = (int)settlementId;
			data.Culture = settlement.GetCulture();
			data.MaxCulture = settlement.GetMaxCulture();
			data.Safety = settlement.GetSafety();
			data.MaxSafety = settlement.GetMaxSafety();
			data.Population = population.Item1;
			data.MaxPopulation = population.Item2;
			CivilianSettlement cs = settlement as CivilianSettlement;
			data.RandomNameId = ((cs != null) ? cs.GetRandomNameId() : -1);
			data.OrgTemplateId = settlement.GetOrgTemplateId();
			data.AreaTemplateId = DomainManager.Map.GetElement_Areas((int)settlement.GetLocation().AreaId).GetTemplateId();
			data.InfluencePowerUpdateDate = settlement.GetInfluencePowerUpdateDate();
			return data;
		}

		// Token: 0x060046DE RID: 18142 RVA: 0x0027B5BC File Offset: 0x002797BC
		[DomainMethod]
		public List<SettlementNameRelatedData> GetSettlementNameRelatedData(List<short> settlementIds)
		{
			int settlementsCount = settlementIds.Count;
			List<SettlementNameRelatedData> data = new List<SettlementNameRelatedData>(settlementsCount);
			for (int i = 0; i < settlementsCount; i++)
			{
				bool flag = settlementIds[i] < 0;
				if (flag)
				{
					data.Add(new SettlementNameRelatedData(-1, -1));
				}
				else
				{
					Settlement settlement = this._settlements[settlementIds[i]];
					data.Add(settlement.GetNameRelatedData());
				}
			}
			return data;
		}

		// Token: 0x060046DF RID: 18143 RVA: 0x0027B634 File Offset: 0x00279834
		[DomainMethod]
		public List<CharacterDisplayData> GetSettlementMembers(short settlementId)
		{
			Settlement settlement = this._settlements[settlementId];
			List<int> chars = new List<int>();
			settlement.GetMembers().GetAllMembers(chars);
			return DomainManager.Character.GetCharacterDisplayDataList(chars);
		}

		// Token: 0x060046E0 RID: 18144 RVA: 0x0027B674 File Offset: 0x00279874
		[DomainMethod]
		public OrganizationCombatSkillsDisplayData GetOrganizationCombatSkillsDisplayData(sbyte organizationTemplateId)
		{
			OrganizationCombatSkillsDisplayData data = new OrganizationCombatSkillsDisplayData();
			data.OrganizationTemplateId = organizationTemplateId;
			Settlement settlement = this.GetSettlementByOrgTemplateId(organizationTemplateId);
			data.ApprovingRate = settlement.CalcApprovingRate();
			data.ApprovingRateTotal = settlement.CalcApprovingRateTotal();
			data.ApprovingRateUpperLimit = OrganizationDomain.GetApprovingRateUpperLimit();
			data.ApprovingRateUpperLimitBonus = settlement.GetApprovingRateUpperLimitBonus() + settlement.GetApprovingRateUpperLimitTempBonus();
			List<short> taiwuLearnedCombatSkillIdList = DomainManager.Taiwu.GetTaiwu().GetLearnedCombatSkills();
			List<short> organizationLearnedSkillIdList = new List<short>();
			for (int i = 0; i < taiwuLearnedCombatSkillIdList.Count; i++)
			{
				CombatSkillItem config = CombatSkill.Instance[taiwuLearnedCombatSkillIdList[i]];
				bool flag = config != null && config.SectId == organizationTemplateId;
				if (flag)
				{
					organizationLearnedSkillIdList.Add(taiwuLearnedCombatSkillIdList[i]);
				}
			}
			int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
			data.LearnedSkills = DomainManager.CombatSkill.GetCombatSkillDisplayData(taiwuCharId, organizationLearnedSkillIdList);
			return data;
		}

		// Token: 0x060046E1 RID: 18145 RVA: 0x0027B764 File Offset: 0x00279964
		[DomainMethod]
		public int[] GetSectPreparationForMartialArtTournament(sbyte orgTemplateId)
		{
			Sect sect = (Sect)this.GetSettlementByOrgTemplateId(orgTemplateId);
			return sect.GetMartialArtTournamentPreparations();
		}

		// Token: 0x060046E2 RID: 18146 RVA: 0x0027B78C File Offset: 0x0027998C
		[DomainMethod]
		public short GetMartialArtTournamentCurrentHostSettlementId()
		{
			MartialArtTournamentMonthlyAction action = (MartialArtTournamentMonthlyAction)DomainManager.TaiwuEvent.GetMonthlyAction(new MonthlyActionKey(2, 0));
			return action.CurrentHost;
		}

		// Token: 0x060046E3 RID: 18147 RVA: 0x0027B7BC File Offset: 0x002799BC
		[DomainMethod]
		public short GetSettlementIdByAreaIdAndBlockId(short areaId, short blockId)
		{
			return DomainManager.Organization.GetSettlementByLocation(new Location(areaId, blockId)).GetId();
		}

		// Token: 0x060046E4 RID: 18148 RVA: 0x0027B7E8 File Offset: 0x002799E8
		[DomainMethod]
		public ShortPair GetCultureByAreaIdAndBlockId(short areaId, short blockId)
		{
			short settlementId = DomainManager.Organization.GetSettlementByLocation(new Location(areaId, blockId)).GetId();
			Settlement settlement = this._settlements[settlementId];
			ShortPair shortPair = new ShortPair(settlement.GetCulture(), settlement.GetMaxCulture());
			return shortPair;
		}

		// Token: 0x060046E5 RID: 18149 RVA: 0x0027B834 File Offset: 0x00279A34
		private void UpdateFactions(DataContext context, Settlement settlement)
		{
			int currDate = DomainManager.World.GetCurrDate();
			LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
			OrgMemberCollection orgMemberCollection = settlement.GetMembers();
			List<GameData.Domains.Character.Character> charsWithoutFaction = new List<GameData.Domains.Character.Character>();
			List<GameData.Domains.Character.Character> factionLeaders = new List<GameData.Domains.Character.Character>();
			for (sbyte grade = 0; grade < 9; grade += 1)
			{
				OrganizationMemberItem memberConfig = OrganizationDomain.GetOrgMemberConfig(settlement.GetOrgTemplateId(), grade);
				bool flag = memberConfig.RestrictPrincipalAmount && memberConfig.Amount < 2;
				if (!flag)
				{
					factionLeaders.Clear();
					charsWithoutFaction.Clear();
					GameData.Domains.Character.Character newFactionLeader = null;
					int maxInfluencePower = -1;
					HashSet<int> gradeMember = orgMemberCollection.GetMembers(grade);
					foreach (int charId in gradeMember)
					{
						GameData.Domains.Character.Character character = DomainManager.Character.GetElement_Objects(charId);
						bool flag2 = !character.IsInteractableAsIntelligentCharacter();
						if (!flag2)
						{
							int factionId = character.GetFactionId();
							bool flag3 = factionId < 0;
							if (flag3)
							{
								charsWithoutFaction.Add(character);
							}
							else
							{
								bool flag4 = factionId == charId;
								if (flag4)
								{
									factionLeaders.Add(character);
								}
								else
								{
									RelatedCharacter relation = DomainManager.Character.GetRelation(charId, factionId);
									sbyte favorabilityType = FavorabilityType.GetFavorabilityType(relation.Favorability);
									sbyte priorityType = FactionLeaderPriorityType.GetFactionLeaderPriorityType(relation.RelationType);
									sbyte favorabilityReq = OrganizationDomain.JoinFactionFavorabilityReq[(int)priorityType];
									bool flag5 = favorabilityType < favorabilityReq;
									if (flag5)
									{
										this.LeaveFaction(context, character, false);
									}
								}
							}
							SettlementCharacter settlementCharacter = DomainManager.Organization.GetSettlementCharacter(charId);
							short influencePower = settlementCharacter.GetInfluencePower();
							bool flag6 = (int)influencePower > maxInfluencePower;
							if (flag6)
							{
								maxInfluencePower = (int)influencePower;
								newFactionLeader = character;
							}
						}
					}
					bool flag7 = newFactionLeader != null;
					if (flag7)
					{
						bool flag8 = this.TryCreateFaction(context, newFactionLeader);
						if (flag8)
						{
							charsWithoutFaction.Remove(newFactionLeader);
							factionLeaders.Add(newFactionLeader);
						}
					}
					foreach (GameData.Domains.Character.Character character2 in charsWithoutFaction)
					{
						ValueTuple<int, bool> valueTuple = this.OfflineJoinFaction(context, character2, factionLeaders);
						int factionId2 = valueTuple.Item1;
						bool succeed = valueTuple.Item2;
						bool flag9 = succeed;
						if (flag9)
						{
							lifeRecordCollection.AddJoinFaction(character2.GetId(), currDate, factionId2, character2.GetLocation());
							CharacterSet members = this._factions[factionId2];
							character2.SetFactionId(factionId2, context);
							members.Add(character2.GetId());
							this.SetElement_Factions(factionId2, members, context);
						}
					}
				}
			}
		}

		// Token: 0x060046E6 RID: 18150 RVA: 0x0027BAD8 File Offset: 0x00279CD8
		public bool TryCreateFaction(DataContext context, GameData.Domains.Character.Character factionLeader)
		{
			int charId = factionLeader.GetId();
			int initialFactionId = factionLeader.GetFactionId();
			bool flag = charId == initialFactionId;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				bool flag2 = !context.Random.CheckPercentProb((int)OrganizationDomain.CreateFactionChance[(int)factionLeader.GetBehaviorType()]);
				if (flag2)
				{
					result = false;
				}
				else
				{
					bool flag3 = initialFactionId >= 0;
					if (flag3)
					{
						this.LeaveFaction(context, factionLeader, false);
					}
					MonthlyNotificationCollection monthlyNotifications = DomainManager.World.GetMonthlyNotificationCollection();
					monthlyNotifications.AddFactionUpgrade(charId, factionLeader.GetOrganizationInfo().SettlementId);
					LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
					int currDate = DomainManager.World.GetCurrDate();
					lifeRecordCollection.AddCreateFaction(charId, currDate, factionLeader.GetLocation());
					this.AddElement_Factions(charId, default(CharacterSet), context);
					factionLeader.SetFactionId(charId, context);
					result = true;
				}
			}
			return result;
		}

		// Token: 0x060046E7 RID: 18151 RVA: 0x0027BBAC File Offset: 0x00279DAC
		public void LeaveFaction(DataContext context, GameData.Domains.Character.Character character, bool charIsDead)
		{
			int factionId = character.GetFactionId();
			int currDate = DomainManager.World.GetCurrDate();
			DomainManager.LifeRecord.GetLifeRecordCollection().AddLeaveFaction(character.GetId(), currDate, factionId, character.GetLocation());
			CharacterSet factionMembers = this._factions[factionId];
			factionMembers.Remove(character.GetId());
			bool flag = !charIsDead;
			if (flag)
			{
				character.SetFactionId(-1, context);
			}
			this.SetElement_Factions(factionId, factionMembers, context);
		}

		// Token: 0x060046E8 RID: 18152 RVA: 0x0027BC20 File Offset: 0x00279E20
		public void RemoveFaction(DataContext context, GameData.Domains.Character.Character factionLeader, bool leaderIsDead)
		{
			int factionId = factionLeader.GetId();
			bool flag = !leaderIsDead;
			if (flag)
			{
				factionLeader.SetFactionId(-1, context);
			}
			foreach (int charId in this._factions[factionId].GetCollection())
			{
				GameData.Domains.Character.Character character = DomainManager.Character.GetElement_Objects(charId);
				character.SetFactionId(-1, context);
			}
			this.RemoveElement_Factions(factionId, context);
		}

		// Token: 0x060046E9 RID: 18153 RVA: 0x0027BCBC File Offset: 0x00279EBC
		public void ExpandAllFactions(DataContext context)
		{
			LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
			int currDate = DomainManager.World.GetCurrDate();
			foreach (int factionId in this._factions.Keys)
			{
				GameData.Domains.Character.Character leader = DomainManager.Character.GetElement_Objects(factionId);
				bool flag = !leader.IsInteractableAsIntelligentCharacter();
				if (!flag)
				{
					ValueTuple<int, bool> valueTuple = this.OfflineExpandFaction(context, leader);
					int newMemberId = valueTuple.Item1;
					bool succeed = valueTuple.Item2;
					bool flag2 = newMemberId < 0;
					if (!flag2)
					{
						bool flag3 = succeed;
						if (flag3)
						{
							lifeRecordCollection.AddFactionRecruitSucceed(factionId, currDate, newMemberId, leader.GetLocation());
							CharacterSet members = this._factions[factionId];
							members.Add(newMemberId);
							DomainManager.Character.GetElement_Objects(newMemberId).SetFactionId(factionId, context);
							this.SetElement_Factions(factionId, members, context);
						}
						else
						{
							lifeRecordCollection.AddFactionRecruitFail(factionId, currDate, newMemberId, leader.GetLocation());
							GameData.Domains.Character.Ai.PersonalNeed need = GameData.Domains.Character.Ai.PersonalNeed.CreatePersonalNeed(19, newMemberId);
							leader.AddPersonalNeed(context, need);
						}
					}
				}
			}
		}

		// Token: 0x060046EA RID: 18154 RVA: 0x0027BDF8 File Offset: 0x00279FF8
		[return: TupleElementNames(new string[]
		{
			"newMemberId",
			"succeed"
		})]
		public ValueTuple<int, bool> OfflineExpandFaction(DataContext context, GameData.Domains.Character.Character factionLeader)
		{
			int leaderId = factionLeader.GetId();
			OrganizationInfo orgInfo = factionLeader.GetOrganizationInfo();
			Sect sect = this._sects[orgInfo.SettlementId];
			OrgMemberCollection members = sect.GetMembers();
			HashSet<int> gradeMembers = members.GetMembers(orgInfo.Grade);
			bool flag = gradeMembers.Count == 0;
			ValueTuple<int, bool> result;
			if (flag)
			{
				result = new ValueTuple<int, bool>(-1, false);
			}
			else
			{
				bool flag2 = !context.Random.CheckPercentProb((int)OrganizationDomain.ExpandFactionChance[(int)factionLeader.GetBehaviorType()]);
				if (flag2)
				{
					result = new ValueTuple<int, bool>(-1, false);
				}
				else
				{
					int maxPriority = int.MaxValue;
					int maxSuccessRate = int.MinValue;
					int selectedCharId = -1;
					foreach (int charId in gradeMembers)
					{
						GameData.Domains.Character.Character character = DomainManager.Character.GetElement_Objects(charId);
						bool flag3 = character.GetFactionId() >= 0;
						if (!flag3)
						{
							bool flag4 = character.GetAgeGroup() == 0;
							if (!flag4)
							{
								sbyte behaviorType = character.GetBehaviorType();
								RelatedCharacter relation = DomainManager.Character.GetRelation(charId, leaderId);
								ushort relationType = relation.RelationType;
								sbyte favorabilityType = FavorabilityType.GetFavorabilityType(relation.Favorability);
								sbyte priorityType = FactionLeaderPriorityType.GetFactionLeaderPriorityType(relationType);
								bool flag5 = priorityType == -1;
								if (!flag5)
								{
									sbyte favorabilityReq = OrganizationDomain.JoinFactionFavorabilityReq[(int)priorityType];
									bool flag6 = favorabilityType < favorabilityReq;
									if (!flag6)
									{
										sbyte priority = OrganizationDomain.JoinFactionPriorities[(int)behaviorType][(int)priorityType];
										bool flag7 = (int)priority > maxPriority;
										if (!flag7)
										{
											int successRate = (int)(OrganizationDomain.JoinFactionChance[(int)behaviorType] + OrganizationDomain.JoinFactionFavorabilityBonus[(int)behaviorType] * (favorabilityType - favorabilityReq));
											bool flag8 = successRate == 0;
											if (!flag8)
											{
												bool flag9 = (int)priority < maxPriority || maxSuccessRate < successRate;
												if (flag9)
												{
													maxPriority = (int)priority;
													maxSuccessRate = successRate;
													selectedCharId = charId;
												}
											}
										}
									}
								}
							}
						}
					}
					bool flag10 = selectedCharId >= 0;
					if (flag10)
					{
						result = new ValueTuple<int, bool>(selectedCharId, context.Random.CheckPercentProb(maxSuccessRate));
					}
					else
					{
						result = new ValueTuple<int, bool>(-1, false);
					}
				}
			}
			return result;
		}

		// Token: 0x060046EB RID: 18155 RVA: 0x0027C01C File Offset: 0x0027A21C
		[return: TupleElementNames(new string[]
		{
			"factionId",
			"succeed"
		})]
		public ValueTuple<int, bool> OfflineJoinFaction(DataContext context, GameData.Domains.Character.Character character, List<GameData.Domains.Character.Character> factionLeaders)
		{
			int charId = character.GetId();
			sbyte behaviorType = character.GetBehaviorType();
			int maxPriority = int.MaxValue;
			int maxSuccessRate = int.MinValue;
			int selectedCharId = -1;
			foreach (GameData.Domains.Character.Character factionLeader in factionLeaders)
			{
				int leaderId = factionLeader.GetId();
				RelatedCharacter relation = DomainManager.Character.GetRelation(charId, leaderId);
				ushort relationType = relation.RelationType;
				sbyte favorabilityType = FavorabilityType.GetFavorabilityType(relation.Favorability);
				sbyte priorityType = FactionLeaderPriorityType.GetFactionLeaderPriorityType(relationType);
				bool flag = priorityType == -1;
				if (!flag)
				{
					sbyte favorabilityReq = OrganizationDomain.JoinFactionFavorabilityReq[(int)priorityType];
					bool flag2 = favorabilityType < favorabilityReq;
					if (!flag2)
					{
						sbyte priority = OrganizationDomain.JoinFactionPriorities[(int)behaviorType][(int)priorityType];
						bool flag3 = (int)priority > maxPriority;
						if (!flag3)
						{
							int successRate = (int)(OrganizationDomain.JoinFactionChance[(int)behaviorType] + OrganizationDomain.JoinFactionFavorabilityBonus[(int)behaviorType] * (favorabilityType - favorabilityReq));
							bool flag4 = successRate == 0;
							if (!flag4)
							{
								bool flag5 = (int)priority < maxPriority || maxSuccessRate < successRate;
								if (flag5)
								{
									maxPriority = (int)priority;
									maxSuccessRate = successRate;
									selectedCharId = leaderId;
								}
							}
						}
					}
				}
			}
			bool flag6 = selectedCharId >= 0;
			ValueTuple<int, bool> result;
			if (flag6)
			{
				result = new ValueTuple<int, bool>(selectedCharId, context.Random.CheckPercentProb(maxSuccessRate));
			}
			else
			{
				result = new ValueTuple<int, bool>(-1, false);
			}
			return result;
		}

		// Token: 0x060046EC RID: 18156 RVA: 0x0027C178 File Offset: 0x0027A378
		public void Test_CheckFactions()
		{
			int totalMemberCount = 0;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			foreach (KeyValuePair<int, CharacterSet> keyValuePair in this._factions)
			{
				int num;
				CharacterSet characterSet;
				keyValuePair.Deconstruct(out num, out characterSet);
				int factionId = num;
				CharacterSet members = characterSet;
				GameData.Domains.Character.Character leader = DomainManager.Character.GetElement_Objects(factionId);
				bool flag = AgeGroup.GetAgeGroup(leader.GetCurrAge()) == 0;
				if (flag)
				{
					defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(39, 1);
					defaultInterpolatedStringHandler.AppendLiteral("(Test) Faction leader ");
					defaultInterpolatedStringHandler.AppendFormatted<int>(factionId);
					defaultInterpolatedStringHandler.AppendLiteral(" cannot be a baby");
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				OrganizationInfo leaderOrgInfo = leader.GetOrganizationInfo();
				bool flag2 = !OrganizationDomain.IsSect(leaderOrgInfo.OrgTemplateId);
				if (flag2)
				{
					defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(50, 2);
					defaultInterpolatedStringHandler.AppendLiteral("(Test) Faction ");
					defaultInterpolatedStringHandler.AppendFormatted<int>(factionId);
					defaultInterpolatedStringHandler.AppendLiteral(" appeared in non-sect organization ");
					defaultInterpolatedStringHandler.AppendFormatted(Organization.Instance[leaderOrgInfo.OrgTemplateId].Name);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				foreach (int member in members.GetCollection())
				{
					GameData.Domains.Character.Character character = DomainManager.Character.GetElement_Objects(member);
					bool flag3 = character.GetAgeGroup() == 0;
					if (flag3)
					{
						defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(40, 1);
						defaultInterpolatedStringHandler.AppendLiteral("(Test) Faction member ");
						defaultInterpolatedStringHandler.AppendFormatted<int>(member);
						defaultInterpolatedStringHandler.AppendLiteral(" cannot be a baby.");
						throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
					}
					OrganizationInfo memberOrgInfo = character.GetOrganizationInfo();
					bool flag4 = memberOrgInfo.OrgTemplateId != leaderOrgInfo.OrgTemplateId;
					if (flag4)
					{
						defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(63, 2);
						defaultInterpolatedStringHandler.AppendLiteral("(Test) Faction member ");
						defaultInterpolatedStringHandler.AppendFormatted<int>(member);
						defaultInterpolatedStringHandler.AppendLiteral(" is in different faction from the leader ");
						defaultInterpolatedStringHandler.AppendFormatted<int>(factionId);
						throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
					}
					bool flag5 = memberOrgInfo.Grade != leaderOrgInfo.Grade;
					if (flag5)
					{
						defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(61, 2);
						defaultInterpolatedStringHandler.AppendLiteral("(Test) Faction member ");
						defaultInterpolatedStringHandler.AppendFormatted<int>(member);
						defaultInterpolatedStringHandler.AppendLiteral(" is in different grade from the leader ");
						defaultInterpolatedStringHandler.AppendFormatted<int>(factionId);
						throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
					}
				}
				totalMemberCount += members.GetCount();
			}
			Logger logger = OrganizationDomain.Logger;
			defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(52, 2);
			defaultInterpolatedStringHandler.AppendLiteral("Faction Total Count: ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(this._factions.Count);
			defaultInterpolatedStringHandler.AppendLiteral("   Faction Member Total Count: ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(totalMemberCount);
			logger.Info(defaultInterpolatedStringHandler.ToStringAndClear());
		}

		// Token: 0x060046ED RID: 18157 RVA: 0x0027C490 File Offset: 0x0027A690
		[DomainMethod]
		public void GmCmd_SetAllSettlementInformationVisited(DataContext context)
		{
			TaiwuDomain taiwuDomain = DomainManager.Taiwu;
			HashSet<short> visited = taiwuDomain.GetVisitedSettlements().ToHashSet<short>();
			foreach (short sectId in this._sects.Keys)
			{
				visited.Add(sectId);
			}
			foreach (short townId in this._civilianSettlements.Keys)
			{
				visited.Add(townId);
			}
			taiwuDomain.SetVisitedSettlements(visited.ToList<short>(), context);
		}

		// Token: 0x060046EE RID: 18158 RVA: 0x0027C560 File Offset: 0x0027A760
		[DomainMethod]
		public List<CharacterDisplayData> GmCmd_GetSettlementPrisoner(DataContext context, int prisonType)
		{
			Location taiwuLocation = DomainManager.Taiwu.GetTaiwu().GetLocation();
			bool flag = !taiwuLocation.IsValid();
			List<CharacterDisplayData> result;
			if (flag)
			{
				result = null;
			}
			else
			{
				MapBlockData taiwuBlockData = DomainManager.Map.GetBlockData(taiwuLocation.AreaId, taiwuLocation.BlockId);
				bool flag2 = taiwuBlockData == null;
				if (flag2)
				{
					result = null;
				}
				else
				{
					Settlement settlement = this.GetSettlementByLocation(taiwuBlockData.GetRootBlock().GetLocation());
					Sect sect = settlement as Sect;
					bool flag3 = sect == null;
					if (flag3)
					{
						result = null;
					}
					else
					{
						bool flag4 = sect == null;
						if (flag4)
						{
							result = null;
						}
						else
						{
							switch (prisonType)
							{
							case 0:
								result = DomainManager.Character.GetCharacterDisplayDataList((from x in sect.Prison.GetPrisonLow()
								select x.CharId).ToList<int>());
								break;
							case 1:
								result = DomainManager.Character.GetCharacterDisplayDataList((from x in sect.Prison.GetPrisonMid()
								select x.CharId).ToList<int>());
								break;
							case 2:
								result = DomainManager.Character.GetCharacterDisplayDataList((from x in sect.Prison.GetPrisonHigh()
								select x.CharId).ToList<int>());
								break;
							case 3:
								result = DomainManager.Character.GetCharacterDisplayDataList((from x in sect.Prison.GetPrisonInfected()
								select x.CharId).ToList<int>());
								break;
							default:
								result = null;
								break;
							}
						}
					}
				}
			}
			return result;
		}

		// Token: 0x060046EF RID: 18159 RVA: 0x0027C738 File Offset: 0x0027A938
		[DomainMethod]
		public List<List<CharacterDisplayData>> GmCmd_GetAllFactionMembers()
		{
			CharacterDomain characterDomain = DomainManager.Character;
			List<int> memberList = new List<int>();
			List<List<CharacterDisplayData>> result = new List<List<CharacterDisplayData>>();
			foreach (KeyValuePair<int, CharacterSet> faction in this._factions)
			{
				memberList.Clear();
				memberList.Add(faction.Key);
				memberList.AddRange(faction.Value.GetCollection());
				result.Add(characterDomain.GetCharacterDisplayDataList(memberList));
			}
			return result;
		}

		// Token: 0x060046F0 RID: 18160 RVA: 0x0027C7E0 File Offset: 0x0027A9E0
		[DomainMethod]
		public void GmCmd_ForceUpdateTreasuryGuards(DataContext context, short settlementId)
		{
			Settlement settlement = this.GetSettlement(settlementId);
			settlement.ForceUpdateTreasuryGuards(context);
		}

		// Token: 0x060046F1 RID: 18161 RVA: 0x0027C800 File Offset: 0x0027AA00
		[DomainMethod]
		public void GmCmd_ForceUpdateInfluencePower(DataContext context, short settlementId)
		{
			Settlement settlement = this.GetSettlement(settlementId);
			Dictionary<int, ValueTuple<GameData.Domains.Character.Character, short>> baseInfluencePowers = new Dictionary<int, ValueTuple<GameData.Domains.Character.Character, short>>();
			HashSet<int> relatedCharIds = new HashSet<int>();
			settlement.UpdateInfluencePowers(context, baseInfluencePowers, relatedCharIds);
		}

		// Token: 0x060046F2 RID: 18162 RVA: 0x0027C82C File Offset: 0x0027AA2C
		[DomainMethod]
		public void AddSectBounty(DataContext context, sbyte orgTemplateId, int charId, sbyte punishmentSeverity, short punishmentType, int duration)
		{
			GameData.Domains.Character.Character character;
			bool flag = !DomainManager.Character.TryGetElement_Objects(charId, out character);
			if (!flag)
			{
				bool flag2 = !OrganizationDomain.IsSect(orgTemplateId);
				if (!flag2)
				{
					Sect sect = (Sect)this.GetSettlementByOrgTemplateId(orgTemplateId);
					sect.AddBounty(context, character, punishmentSeverity, punishmentType, duration);
					bool flag3 = punishmentType == 42;
					if (flag3)
					{
						Settlement settlement = DomainManager.Organization.GetSettlementByOrgTemplateId(orgTemplateId);
						List<int> members = new List<int>();
						settlement.GetMembers().GetAllMembers(members);
						using (List<int>.Enumerator enumerator = members.GetEnumerator())
						{
							if (enumerator.MoveNext())
							{
								int memberId = enumerator.Current;
								DomainManager.Character.AddRelation(context, charId, memberId, 32768, int.MinValue);
							}
						}
					}
				}
			}
		}

		// Token: 0x060046F3 RID: 18163 RVA: 0x0027C910 File Offset: 0x0027AB10
		[DomainMethod]
		public void AddSectPrisoner(DataContext context, sbyte orgTemplateId, int charId, sbyte punishmentSeverity, short punishmentType, int duration)
		{
			GameData.Domains.Character.Character character;
			bool flag = !DomainManager.Character.TryGetElement_Objects(charId, out character);
			if (!flag)
			{
				bool flag2 = !OrganizationDomain.IsSect(orgTemplateId);
				if (!flag2)
				{
					Sect sect = (Sect)this.GetSettlementByOrgTemplateId(orgTemplateId);
					sect.AddPrisoner(context, character, punishmentSeverity, punishmentType, -1);
				}
			}
		}

		// Token: 0x060046F4 RID: 18164 RVA: 0x0027C960 File Offset: 0x0027AB60
		private void RecordSettlementStandardPopulations(DataContext context)
		{
			foreach (KeyValuePair<short, Settlement> keyValuePair in this._settlements)
			{
				short num;
				Settlement settlement2;
				keyValuePair.Deconstruct(out num, out settlement2);
				Settlement settlement = settlement2;
				OrgMemberCollection members = settlement.GetMembers();
				int count = members.GetCount();
				settlement.SetStandardOnStagePopulation(count, context);
			}
		}

		// Token: 0x060046F5 RID: 18165 RVA: 0x0027C9DC File Offset: 0x0027ABDC
		public void ChangeSettlementStandardPopulations(DataContext context, byte oriWorldPopulationType)
		{
			int oriFactor = WorldDomain.GetWorldPopulationFactor(oriWorldPopulationType);
			int currFactor = DomainManager.World.GetWorldPopulationFactor();
			foreach (KeyValuePair<short, Settlement> keyValuePair in this._settlements)
			{
				short num;
				Settlement settlement2;
				keyValuePair.Deconstruct(out num, out settlement2);
				Settlement settlement = settlement2;
				int oriPopulation = settlement.GetStandardOnStagePopulation();
				int basicPopulation = oriPopulation * 100 / oriFactor;
				int currPopulation = basicPopulation * currFactor / 100;
				settlement.SetStandardOnStagePopulation(currPopulation, context);
			}
		}

		// Token: 0x060046F6 RID: 18166 RVA: 0x0027CA74 File Offset: 0x0027AC74
		private void InitializePrisonCache()
		{
			this._sectFugitives.Clear();
			this._sectPrisoners.Clear();
			foreach (Sect sect in this._sects.Values)
			{
				sbyte orgTemplateId = sect.GetOrgTemplateId();
				SettlementPrison prison = sect.Prison;
				foreach (SettlementBounty bounty in prison.Bounties)
				{
					this.RegisterSectFugitive(bounty.CharId, orgTemplateId);
				}
				foreach (SettlementPrisoner prisoner in prison.Prisoners)
				{
					this.RegisterSectPrisoner(prisoner.CharId, orgTemplateId);
				}
			}
		}

		// Token: 0x060046F7 RID: 18167 RVA: 0x0027CB94 File Offset: 0x0027AD94
		internal void RegisterSectFugitive(int charId, sbyte orgTemplateId)
		{
			List<sbyte> sectSet;
			bool flag = !this._sectFugitives.TryGetValue(charId, out sectSet);
			if (flag)
			{
				sectSet = new List<sbyte>(1);
				this._sectFugitives.Add(charId, sectSet);
			}
			else
			{
				bool flag2 = sectSet.Contains(orgTemplateId);
				if (flag2)
				{
					return;
				}
			}
			sectSet.Add(orgTemplateId);
		}

		// Token: 0x060046F8 RID: 18168 RVA: 0x0027CBE8 File Offset: 0x0027ADE8
		internal void UnregisterSectFugitive(int charId, sbyte orgTemplateId)
		{
			List<sbyte> sectSet;
			bool flag = !this._sectFugitives.TryGetValue(charId, out sectSet);
			if (!flag)
			{
				sectSet.Remove(orgTemplateId);
				bool flag2 = sectSet.Count == 0;
				if (flag2)
				{
					this._sectFugitives.Remove(charId);
				}
			}
		}

		// Token: 0x060046F9 RID: 18169 RVA: 0x0027CC30 File Offset: 0x0027AE30
		public sbyte GetFugitiveBountySect(int charId)
		{
			List<sbyte> sects;
			bool flag = this._sectFugitives.TryGetValue(charId, out sects);
			sbyte result;
			if (flag)
			{
				result = sects[0];
			}
			else
			{
				result = -1;
			}
			return result;
		}

		// Token: 0x060046FA RID: 18170 RVA: 0x0027CC5F File Offset: 0x0027AE5F
		public IEnumerable<sbyte> GetFugitiveBountySects(int charId)
		{
			List<sbyte> sects;
			bool flag = this._sectFugitives.TryGetValue(charId, out sects);
			if (flag)
			{
				foreach (sbyte item in sects)
				{
					yield return item;
				}
				List<sbyte>.Enumerator enumerator = default(List<sbyte>.Enumerator);
			}
			yield break;
			yield break;
		}

		// Token: 0x060046FB RID: 18171 RVA: 0x0027CC78 File Offset: 0x0027AE78
		public List<sbyte> GetTaiwuFugitiveBountySect()
		{
			int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
			List<sbyte> sects;
			bool flag = this._sectFugitives.TryGetValue(taiwuCharId, out sects);
			List<sbyte> result;
			if (flag)
			{
				result = sects;
			}
			else
			{
				result = null;
			}
			return result;
		}

		// Token: 0x060046FC RID: 18172 RVA: 0x0027CCAC File Offset: 0x0027AEAC
		public bool IsSectFugitive(int charId, sbyte orgTemplateId)
		{
			List<sbyte> sectSet;
			return this._sectFugitives.TryGetValue(charId, out sectSet) && sectSet.Contains(orgTemplateId);
		}

		// Token: 0x060046FD RID: 18173 RVA: 0x0027CCD8 File Offset: 0x0027AED8
		public SettlementBounty GetBounty(int charId, out sbyte sectOrgTemplateId)
		{
			sectOrgTemplateId = this.GetFugitiveBountySect(charId);
			bool flag = sectOrgTemplateId < 0;
			SettlementBounty result;
			if (flag)
			{
				result = null;
			}
			else
			{
				Sect sect = (Sect)this.GetSettlementByOrgTemplateId(sectOrgTemplateId);
				result = sect.Prison.GetBounty(charId);
			}
			return result;
		}

		// Token: 0x060046FE RID: 18174 RVA: 0x0027CD1C File Offset: 0x0027AF1C
		public bool TryRemoveBounty(DataContext context, int charId)
		{
			Tester.Assert(charId != DomainManager.Taiwu.GetTaiwuCharId(), "使用TryRemoveTaiwuBounty移除太吾的悬赏");
			sbyte bountySectId = DomainManager.Organization.GetFugitiveBountySect(charId);
			bool flag = bountySectId < 0;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				short settlementId = DomainManager.Organization.GetSettlementIdByOrgTemplateId(bountySectId);
				Sect sect = this._sects[settlementId];
				result = sect.RemoveBounty(context, charId);
			}
			return result;
		}

		// Token: 0x060046FF RID: 18175 RVA: 0x0027CD88 File Offset: 0x0027AF88
		public void TryRemoveTaiwuBounty(DataContext context)
		{
			List<sbyte> bountySectIds = DomainManager.Organization.GetTaiwuFugitiveBountySect();
			bool flag = bountySectIds == null;
			if (!flag)
			{
				for (int i = 0; i < bountySectIds.Count; i++)
				{
					sbyte sectTemplateId = bountySectIds[i];
					short settlementId = DomainManager.Organization.GetSettlementIdByOrgTemplateId(sectTemplateId);
					Sect sect = this._sects[settlementId];
					sect.RemoveBounty(context, DomainManager.Taiwu.GetTaiwuCharId());
				}
			}
		}

		// Token: 0x06004700 RID: 18176 RVA: 0x0027CDFC File Offset: 0x0027AFFC
		public void TryRemoveTaiwuGroupBountyAndPunishment(DataContext context)
		{
			GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
			int taiwuId = taiwu.GetId();
			HashSet<int> taiwuGroup = DomainManager.Taiwu.GetGroupCharIds().GetCollection();
			InstantNotificationCollection instantCollection = DomainManager.World.GetInstantNotificationCollection();
			List<short> featureIds = taiwu.GetFeatureIds();
			foreach (int charId in taiwuGroup)
			{
				bool flag = charId == taiwuId;
				if (!flag)
				{
					sbyte bountySectId = DomainManager.Organization.GetFugitiveBountySect(charId);
					bool flag2 = bountySectId < 0;
					if (!flag2)
					{
						short settlementId = DomainManager.Organization.GetSettlementIdByOrgTemplateId(bountySectId);
						Sect sect = this._sects[settlementId];
						bool flag3 = sect.RemoveBounty(context, charId);
						if (flag3)
						{
							instantCollection.AddSectPunishmentWarrantRelieved(charId, settlementId);
						}
					}
				}
			}
			List<int> toRemoveList = new List<int>();
			List<sbyte> sects;
			bool flag4 = this._sectFugitives.TryGetValue(taiwuId, out sects);
			if (flag4)
			{
				foreach (sbyte sectTemplateId in sects)
				{
					toRemoveList.Add((int)sectTemplateId);
				}
			}
			foreach (int sectTemplateId2 in toRemoveList)
			{
				short settlementId2 = DomainManager.Organization.GetSettlementIdByOrgTemplateId((sbyte)sectTemplateId2);
				Sect sect2 = this._sects[settlementId2];
				bool flag5 = sect2.RemoveBounty(context, taiwuId);
				if (flag5)
				{
					instantCollection.AddSectPunishmentWarrantRelieved(taiwuId, settlementId2);
				}
			}
			toRemoveList.Clear();
			for (sbyte orgTemplateId = 1; orgTemplateId <= 15; orgTemplateId += 1)
			{
				List<short> punishments = Organization.Instance[orgTemplateId].TaiwuPunishementFeature;
				bool flag6 = punishments == null;
				if (!flag6)
				{
					foreach (short punishment in punishments)
					{
						bool flag7 = featureIds.Contains(punishment);
						if (flag7)
						{
							short settlementId3 = DomainManager.Organization.GetSettlementIdByOrgTemplateId(orgTemplateId);
							toRemoveList.Add((int)punishment);
							instantCollection.AddSectPunishmentCharacterFeatureRelieved(taiwuId, settlementId3);
							break;
						}
					}
				}
			}
			foreach (int featureId in toRemoveList)
			{
				taiwu.RemoveFeature(context, (short)featureId);
				DomainManager.Extra.UnregisterCharacterTemporaryFeature(context, taiwuId, (short)featureId);
			}
		}

		// Token: 0x06004701 RID: 18177 RVA: 0x0027D0CC File Offset: 0x0027B2CC
		internal void RegisterSectPrisoner(int charId, sbyte orgTemplateId)
		{
			bool flag = !this._sectPrisoners.TryAdd(charId, orgTemplateId);
			if (flag)
			{
				Logger logger = OrganizationDomain.Logger;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(43, 1);
				defaultInterpolatedStringHandler.AppendLiteral("character ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(charId);
				defaultInterpolatedStringHandler.AppendLiteral(" is imprisoned by multiple sects.");
				logger.AppendWarning(defaultInterpolatedStringHandler.ToStringAndClear());
			}
		}

		// Token: 0x06004702 RID: 18178 RVA: 0x0027D12D File Offset: 0x0027B32D
		internal void UnregisterSectPrisoner(int charId)
		{
			this._sectPrisoners.Remove(charId);
		}

		// Token: 0x06004703 RID: 18179 RVA: 0x0027D140 File Offset: 0x0027B340
		public sbyte GetPrisonerSect(int charId)
		{
			return this._sectPrisoners.GetValueOrDefault(charId, -1);
		}

		// Token: 0x06004704 RID: 18180 RVA: 0x0027D15F File Offset: 0x0027B35F
		[Obsolete]
		public void SetSettlementPrisonGuardCharId(int charId)
		{
			this._prisonGuardCharId = charId;
		}

		// Token: 0x06004705 RID: 18181 RVA: 0x0027D16C File Offset: 0x0027B36C
		[DomainMethod]
		public SettlementPrisonDisplayData GetSettlementPrisonDisplayData(DataContext context, short settlementId)
		{
			Sect sect = this.GetSettlement(settlementId) as Sect;
			SettlementPrison prison = sect.Prison;
			CharacterDisplayData[] guards = (from prisonGuardCharId in sect.Treasuries.GetGuardIds()
			select DomainManager.Character.GetCharacterDisplayData(prisonGuardCharId)).ToArray<CharacterDisplayData>();
			SettlementPrisonDisplayData data = new SettlementPrisonDisplayData
			{
				OrgTemplateId = (int)sect.GetOrgTemplateId(),
				DebtOrSupport = (int)sect.CalcApprovingRate(),
				GuardianCharacterDisplayDataLow = sect.GetGuardsDisplayData(context, 0),
				GuardianCharacterDisplayDataMid = sect.GetGuardsDisplayData(context, 1),
				GuardianCharacterDisplayDataHigh = sect.GetGuardsDisplayData(context, 2),
				PrisonerCharacterDisplayDataDict = new Dictionary<int, CharacterDisplayDataForSettlementPrisoner>()
			};
			int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
			foreach (SettlementPrisoner prisoner in prison.Prisoners)
			{
				GameData.Domains.Character.Character character = DomainManager.Character.GetElement_Objects(prisoner.CharId);
				CharacterDisplayDataForSettlementPrisoner charDisplayData = new CharacterDisplayDataForSettlementPrisoner();
				charDisplayData.Resistance = sect.CalcKidnappedCharacterResistance(prisoner);
				charDisplayData.EscapeRate = prisoner.CalcEscapeRate(charDisplayData.Resistance, 0);
				charDisplayData.SettlementPrisoner = new SettlementPrisoner(prisoner);
				charDisplayData.CurrAge = character.GetCurrAge();
				charDisplayData.AvatarRelatedData = character.GenerateAvatarRelatedData();
				charDisplayData.NameRelatedData = DomainManager.Character.GetNameRelatedData(prisoner.CharId);
				charDisplayData.Gender = character.GetGender();
				charDisplayData.Health = character.GetHealth();
				charDisplayData.LeftMaxHealth = character.GetLeftMaxHealth(false);
				charDisplayData.OrgInfo = character.GetOrganizationInfo();
				charDisplayData.CompletelyInfected = character.IsCompletelyInfected();
				charDisplayData.Happiness = (short)character.GetHappiness();
				charDisplayData.FavorabilityToTaiwu = DomainManager.Character.GetFavorability(prisoner.CharId, taiwuCharId);
				charDisplayData.RandomNameId = ((charDisplayData.OrgInfo.SettlementId >= 0) ? DomainManager.Organization.GetSettlement(charDisplayData.OrgInfo.SettlementId).GetNameRelatedData().RandomNameId : -1);
				data.PrisonerCharacterDisplayDataDict[prisoner.CharId] = charDisplayData;
			}
			return data;
		}

		// Token: 0x06004706 RID: 18182 RVA: 0x0027D3C0 File Offset: 0x0027B5C0
		[DomainMethod]
		public SettlementBountyDisplayData GetSettlementBountyDisplayData(short settlementId)
		{
			SettlementBountyDisplayData data = new SettlementBountyDisplayData();
			Sect sect = (Sect)this.GetSettlement(settlementId);
			this._calculatedBountiesCache.Clear();
			sect.GetEnemyRelationBounties(this._calculatedBountiesCache);
			sect.GetEnemySectBounties(this._calculatedBountiesCache);
			sect.GetXiangshuInfectedBounties(this._calculatedBountiesCache);
			SettlementPrison prison = sect.Prison;
			data.BountyCharacterDisplayDataDict = new Dictionary<int, CharacterDisplayDataForSettlementBounty>();
			data.OrgTemplateId = (int)sect.GetOrgTemplateId();
			this.GetBountyCharacterDisplayDataFromList(data, prison.Bounties);
			this.GetBountyCharacterDisplayDataFromList(data, this._calculatedBountiesCache);
			return data;
		}

		// Token: 0x06004707 RID: 18183 RVA: 0x0027D454 File Offset: 0x0027B654
		internal void FillBountyCharacterDisplayDataFromInfo(CharacterDisplayDataForSettlementBounty charDisplayData, GameData.Domains.Character.Character character, SettlementBounty bounty)
		{
			charDisplayData.CurrAge = character.GetCurrAge();
			charDisplayData.AvatarRelatedData = character.GenerateAvatarRelatedData();
			charDisplayData.NameRelatedData = DomainManager.Character.GetNameRelatedData(character.GetId());
			charDisplayData.Gender = character.GetGender();
			charDisplayData.Health = character.GetHealth();
			charDisplayData.LeftMaxHealth = character.GetLeftMaxHealth(false);
			charDisplayData.OrgInfo = character.GetOrganizationInfo();
			charDisplayData.FullBlockName = DomainManager.Map.GetBlockFullName(character.GetLocation());
			charDisplayData.RandomNameId = ((charDisplayData.OrgInfo.SettlementId >= 0) ? DomainManager.Organization.GetSettlement(charDisplayData.OrgInfo.SettlementId).GetNameRelatedData().RandomNameId : -1);
			bool flag = bounty != null;
			if (flag)
			{
				charDisplayData.SettlementBounty = new SettlementBounty(bounty);
				charDisplayData.HunterState = this.GetHunterState(bounty, character);
			}
		}

		// Token: 0x06004708 RID: 18184 RVA: 0x0027D534 File Offset: 0x0027B734
		private void GetBountyCharacterDisplayDataFromList(SettlementBountyDisplayData data, List<SettlementBounty> source)
		{
			foreach (SettlementBounty bounty in source)
			{
				bool flag = data.BountyCharacterDisplayDataDict.ContainsKey(bounty.CharId);
				if (!flag)
				{
					CharacterDisplayDataForSettlementBounty charDisplayData = new CharacterDisplayDataForSettlementBounty();
					this.FillBountyCharacterDisplayDataFromInfo(charDisplayData, DomainManager.Character.GetElement_Objects(bounty.CharId), bounty);
					data.BountyCharacterDisplayDataDict[bounty.CharId] = charDisplayData;
				}
			}
		}

		// Token: 0x06004709 RID: 18185 RVA: 0x0027D5CC File Offset: 0x0027B7CC
		[DomainMethod]
		public SettlementBountyDisplayData GetBountyCharacterDisplayDataFromCharacterList(List<int> characterIds)
		{
			SettlementBountyDisplayData result = new SettlementBountyDisplayData
			{
				BountyCharacterDisplayDataDict = new Dictionary<int, CharacterDisplayDataForSettlementBounty>(),
				OrgTemplateId = -1
			};
			OrganizationDomain orgDomain = DomainManager.Organization;
			int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
			List<int> sourceIds = new List<int>();
			bool flag = characterIds != null;
			if (flag)
			{
				sourceIds.AddRange(characterIds);
			}
			for (int i = 0; i < sourceIds.Count; i++)
			{
				int charId = sourceIds[i];
				GameData.Domains.Character.Character character;
				bool flag2 = !DomainManager.Character.TryGetElement_Objects(charId, out character);
				if (!flag2)
				{
					bool flag3 = charId == taiwuCharId;
					if (flag3)
					{
						int index = -1;
						foreach (OrganizationItem orgConfig in ((IEnumerable<OrganizationItem>)Organization.Instance))
						{
							Sect sect;
							bool flag4;
							if (orgConfig.IsSect && orgDomain.IsSectFugitive(charId, orgConfig.TemplateId))
							{
								sect = (orgDomain.GetSettlementByOrgTemplateId(orgConfig.TemplateId) as Sect);
								flag4 = (sect == null);
							}
							else
							{
								flag4 = true;
							}
							bool flag5 = flag4;
							if (!flag5)
							{
								SettlementBounty bounty = sect.Prison.GetBounty(charId);
								bool flag6 = bounty == null;
								if (!flag6)
								{
									CharacterDisplayDataForSettlementBounty data = new CharacterDisplayDataForSettlementBounty();
									this.FillBountyCharacterDisplayDataFromInfo(data, character, bounty);
									data.OrgInfo.OrgTemplateId = orgConfig.TemplateId;
									bool flag7 = !sourceIds.Contains(bounty.CurrentHunterId);
									if (flag7)
									{
										sourceIds.Add(bounty.CurrentHunterId);
									}
									result.BountyCharacterDisplayDataDict[index] = data;
									index--;
								}
							}
						}
					}
					else
					{
						CharacterDisplayDataForSettlementBounty data2 = new CharacterDisplayDataForSettlementBounty();
						sbyte sectOrgTemplateId;
						SettlementBounty bounty2 = this.GetBounty(charId, out sectOrgTemplateId);
						this.FillBountyCharacterDisplayDataFromInfo(data2, character, bounty2);
						data2.OrgInfo.OrgTemplateId = sectOrgTemplateId;
						bool flag8 = bounty2 != null;
						if (flag8)
						{
							bool flag9 = !sourceIds.Contains(bounty2.CurrentHunterId);
							if (flag9)
							{
								sourceIds.Add(bounty2.CurrentHunterId);
							}
						}
						result.BountyCharacterDisplayDataDict[charId] = data2;
					}
				}
			}
			return result;
		}

		// Token: 0x0600470A RID: 18186 RVA: 0x0027D7F8 File Offset: 0x0027B9F8
		[DomainMethod]
		public SettlementPrisonRecordCollection GetSettlementPrisonRecordCollection(DataContext context, short settlementId)
		{
			return DomainManager.Extra.GetSettlementPrisonRecordCollection(context, settlementId);
		}

		// Token: 0x0600470B RID: 18187 RVA: 0x0027D808 File Offset: 0x0027BA08
		public bool IsCharacterSectFugitive(int charId, sbyte orgTemplateId)
		{
			sbyte bountyOrgTemplateId = DomainManager.Organization.GetFugitiveBountySect(charId);
			GameData.Domains.Character.Character character = DomainManager.Character.GetElement_Objects(charId);
			return bountyOrgTemplateId == orgTemplateId || character.IsCompletelyInfected();
		}

		// Token: 0x0600470C RID: 18188 RVA: 0x0027D83F File Offset: 0x0027BA3F
		public void SetSettlementPrisonRecordCollection(DataContext context, short settlementId, SettlementPrisonRecordCollection collection)
		{
			DomainManager.Extra.SetSettlementPrisonRecordCollection(context, settlementId, collection);
		}

		// Token: 0x0600470D RID: 18189 RVA: 0x0027D850 File Offset: 0x0027BA50
		private sbyte GetHunterState(SettlementBounty bounty, GameData.Domains.Character.Character character)
		{
			bool flag = bounty.CurrentHunterId < 0;
			sbyte result;
			if (flag)
			{
				result = ((bounty.RequiredConsummateLevel >= 0) ? 2 : 0);
			}
			else
			{
				result = ((character.GetKidnapperId() == bounty.CurrentHunterId) ? 3 : 1);
			}
			return result;
		}

		// Token: 0x0600470E RID: 18190 RVA: 0x0027D894 File Offset: 0x0027BA94
		[return: TupleElementNames(new string[]
		{
			"changeFavorCharIds",
			"becomeEnemyCharIds",
			"approveCharIds",
			"disapproveCharIds"
		})]
		public ValueTuple<CharacterSet, CharacterSet, CharacterSet, CharacterSet> ApplySettlementPrisonEventEffect(DataContext context, SettlementPrisonEventEffectItem effectCfg, short settlementId)
		{
			Settlement settlement = this.GetSettlement(settlementId);
			Sect sect = settlement as Sect;
			bool flag = sect == null;
			if (flag)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(23, 1);
				defaultInterpolatedStringHandler.AppendLiteral("settlement ");
				defaultInterpolatedStringHandler.AppendFormatted<Settlement>(settlement);
				defaultInterpolatedStringHandler.AppendLiteral(" is not Sect");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			SettlementLayeredTreasuries treasuries = settlement.Treasuries;
			OrgMemberCollection members = settlement.GetMembers();
			PunishmentTypeItem punishment;
			bool flag2;
			if (effectCfg.TaiwuBounty >= 0)
			{
				punishment = PunishmentType.Instance[effectCfg.TaiwuBounty];
				flag2 = (punishment != null);
			}
			else
			{
				flag2 = false;
			}
			bool flag3 = flag2;
			if (flag3)
			{
				sect.AddBounty(context, DomainManager.Taiwu.GetTaiwu(), punishment.Severity, effectCfg.TaiwuBounty, -1);
			}
			bool flag4 = effectCfg.AlterTime > 0;
			if (flag4)
			{
				sect.SetAlterTime(context, (byte)effectCfg.AlterTime);
			}
			HashSet<int> guardIds = ObjectPool<HashSet<int>>.Instance.Get();
			guardIds.Clear();
			treasuries.GetGuardIds(guardIds);
			SettlementTreasuryEventEffectHelper.EffectArgs guardArgs = new SettlementTreasuryEventEffectHelper.EffectArgs(effectCfg, guardIds.Count, true);
			this.ApplySettlementTreasuryEventEffect(context, guardIds, ref guardArgs);
			List<int> charIdList = ObjectPool<List<int>>.Instance.Get();
			charIdList.Clear();
			for (sbyte grade = 0; grade <= 8; grade += 1)
			{
				IEnumerable<int> gradeMembers = DomainManager.Character.ExcludeInfant(members.GetMembers(grade));
				foreach (int charId in gradeMembers)
				{
					bool flag5 = guardIds.Contains(charId);
					if (!flag5)
					{
						charIdList.Add(charId);
					}
				}
			}
			CollectionUtils.Shuffle<int>(context.Random, charIdList);
			SettlementTreasuryEventEffectHelper.EffectArgs memberArgs = new SettlementTreasuryEventEffectHelper.EffectArgs(effectCfg, charIdList.Count, false)
			{
				ChangeFavorCharIds = guardArgs.ChangeFavorCharIds,
				ApproveCharIds = guardArgs.ApproveCharIds,
				DisapproveCharIds = guardArgs.DisapproveCharIds,
				BecomeEnemyCharIds = guardArgs.BecomeEnemyCharIds
			};
			this.ApplySettlementTreasuryEventEffect(context, charIdList, ref memberArgs);
			ObjectPool<HashSet<int>>.Instance.Return(guardIds);
			ObjectPool<List<int>>.Instance.Return(charIdList);
			return new ValueTuple<CharacterSet, CharacterSet, CharacterSet, CharacterSet>(memberArgs.ChangeFavorCharIds, memberArgs.BecomeEnemyCharIds, memberArgs.ApproveCharIds, memberArgs.DisapproveCharIds);
		}

		// Token: 0x0600470F RID: 18191 RVA: 0x0027DAEC File Offset: 0x0027BCEC
		[DomainMethod]
		public bool IsTaiwuSectFugitive(sbyte orgTemplateId)
		{
			List<sbyte> sectList = this.GetTaiwuFugitiveBountySect();
			return sectList != null && sectList.Contains(orgTemplateId);
		}

		// Token: 0x06004710 RID: 18192 RVA: 0x0027DB14 File Offset: 0x0027BD14
		public sbyte GetSectFavorability(sbyte orgTemplateId, sbyte relatedOrgTemplateId)
		{
			sbyte largeSectIndex = OrganizationDomain.GetLargeSectIndex(orgTemplateId);
			sbyte relatedLargeSectIndex = OrganizationDomain.GetLargeSectIndex(relatedOrgTemplateId);
			bool flag = largeSectIndex >= 0 && relatedLargeSectIndex >= 0;
			sbyte result;
			if (flag)
			{
				result = Organization.Instance[orgTemplateId].LargeSectFavorabilities[(int)relatedLargeSectIndex];
			}
			else
			{
				result = 0;
			}
			return result;
		}

		// Token: 0x06004711 RID: 18193 RVA: 0x0027DB5C File Offset: 0x0027BD5C
		public void GetSectTemplateIdsByFavorability(sbyte orgTemplateId, sbyte sectFavorability, ref SpanList<sbyte> result)
		{
			for (sbyte i = 0; i < 15; i += 1)
			{
				sbyte relatedSectTemplateId = OrganizationDomain.GetLargeSectTemplateId(i);
				bool flag = this.GetSectFavorability(orgTemplateId, relatedSectTemplateId) != sectFavorability;
				if (!flag)
				{
					result.Add(relatedSectTemplateId);
				}
			}
		}

		// Token: 0x06004712 RID: 18194 RVA: 0x0027DBA4 File Offset: 0x0027BDA4
		[Obsolete]
		public void SetSectFavorability(DataContext context, sbyte orgTemplateId, sbyte relatedOrgTemplateId, sbyte favorability)
		{
			sbyte largeSectIndex = OrganizationDomain.GetLargeSectIndex(orgTemplateId);
			sbyte relatedLargeSectIndex = OrganizationDomain.GetLargeSectIndex(relatedOrgTemplateId);
			bool flag = largeSectIndex >= 0 && relatedLargeSectIndex >= 0;
			if (flag)
			{
				this.SetLargeSectFavorability(context, largeSectIndex, relatedLargeSectIndex, favorability);
				return;
			}
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(43, 2);
			defaultInterpolatedStringHandler.AppendLiteral("Not support favorability of small sects: ");
			defaultInterpolatedStringHandler.AppendFormatted<sbyte>(orgTemplateId);
			defaultInterpolatedStringHandler.AppendLiteral(", ");
			defaultInterpolatedStringHandler.AppendFormatted<sbyte>(relatedOrgTemplateId);
			throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
		}

		// Token: 0x06004713 RID: 18195 RVA: 0x0027DC24 File Offset: 0x0027BE24
		public unsafe void OfflineInitializeLargeSectFavorabilities(sbyte largeSectIndex, sbyte[] sectFavorabilities)
		{
			uint favorabilities = 0U;
			for (int i = 0; i < 15; i++)
			{
				uint favorability = (uint)sectFavorabilities[i];
				favorabilities |= favorability << i * 2;
			}
			sbyte[] array;
			sbyte* pLargeSectFavorabilities;
			if ((array = this._largeSectFavorabilities) == null || array.Length == 0)
			{
				pLargeSectFavorabilities = null;
			}
			else
			{
				pLargeSectFavorabilities = &array[0];
			}
			sbyte* pFavorabilities = pLargeSectFavorabilities + largeSectIndex * 4;
			*(int*)pFavorabilities = (int)favorabilities;
			array = null;
		}

		// Token: 0x06004714 RID: 18196 RVA: 0x0027DC8C File Offset: 0x0027BE8C
		[Obsolete]
		private sbyte GetLargeSectFavorability(sbyte largeSectIndex, sbyte relatedLargeSectIndex)
		{
			int index0 = (int)(largeSectIndex * 4 + relatedLargeSectIndex / 4);
			uint favorabilities = (uint)this._largeSectFavorabilities[index0];
			int index = (int)(relatedLargeSectIndex % 4 * 2);
			return (sbyte)(favorabilities >> index & 3U);
		}

		// Token: 0x06004715 RID: 18197 RVA: 0x0027DCC0 File Offset: 0x0027BEC0
		[Obsolete]
		private void SetLargeSectFavorability(DataContext context, sbyte largeSectIndex, sbyte relatedLargeSectIndex, sbyte favorability)
		{
			int index0 = (int)(largeSectIndex * 4 + relatedLargeSectIndex / 4);
			uint favorabilities = (uint)this._largeSectFavorabilities[index0];
			int index = (int)(relatedLargeSectIndex % 4 * 2);
			favorabilities &= ~(3U << index);
			favorabilities |= (uint)((uint)favorability << index);
			this._largeSectFavorabilities[index0] = (sbyte)favorabilities;
			this.SetLargeSectFavorabilities(this._largeSectFavorabilities, context);
		}

		// Token: 0x06004716 RID: 18198 RVA: 0x0027DD14 File Offset: 0x0027BF14
		public static sbyte GetRandomSectOrgTemplateId(IRandomSource random, sbyte gender = -1)
		{
			if (!true)
			{
			}
			sbyte result;
			switch (gender)
			{
			case -1:
				result = OrganizationDomain._allSectOrgTemplateIds[random.Next(OrganizationDomain._allSectOrgTemplateIds.Length)];
				break;
			case 0:
				result = OrganizationDomain._femaleSectOrgTemplateIds[random.Next(OrganizationDomain._femaleSectOrgTemplateIds.Length)];
				break;
			case 1:
				result = OrganizationDomain._maleSectOrgTemplateIds[random.Next(OrganizationDomain._maleSectOrgTemplateIds.Length)];
				break;
			default:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(19, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Unsupported gender ");
				defaultInterpolatedStringHandler.AppendFormatted<sbyte>(gender);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			}
			if (!true)
			{
			}
			return result;
		}

		// Token: 0x06004717 RID: 18199 RVA: 0x0027DDB4 File Offset: 0x0027BFB4
		public static short GetRandomOrgMemberClothing(IRandomSource random, OrganizationMemberItem orgMemberConfig)
		{
			PresetEquipmentItem clothing = orgMemberConfig.Clothing;
			bool flag = clothing.TemplateId >= 0;
			short result;
			if (flag)
			{
				result = clothing.TemplateId;
			}
			else
			{
				result = (random.NextBool() ? 0 : 9);
			}
			return result;
		}

		// Token: 0x06004718 RID: 18200 RVA: 0x0027DDF4 File Offset: 0x0027BFF4
		public static sbyte GetRandomOrgMemberGender(IRandomSource random, sbyte orgTemplateId)
		{
			sbyte genderRestriction = Organization.Instance[orgTemplateId].GenderRestriction;
			return (genderRestriction != -1) ? genderRestriction : (random.NextBool() ? 1 : 0);
		}

		// Token: 0x06004719 RID: 18201 RVA: 0x0027DE2C File Offset: 0x0027C02C
		public static bool MeetGenderRestriction(sbyte orgTemplateId, sbyte gender)
		{
			sbyte genderRestriction = Organization.Instance[orgTemplateId].GenderRestriction;
			return genderRestriction == -1 || genderRestriction == gender;
		}

		// Token: 0x0600471A RID: 18202 RVA: 0x0027DE5C File Offset: 0x0027C05C
		public static bool IsSect(sbyte orgTemplateId)
		{
			return Organization.Instance[orgTemplateId].IsSect;
		}

		// Token: 0x0600471B RID: 18203 RVA: 0x0027DE80 File Offset: 0x0027C080
		public static short GetMemberId(sbyte orgTemplateId, sbyte grade)
		{
			return Organization.Instance[orgTemplateId].Members[(int)grade];
		}

		// Token: 0x0600471C RID: 18204 RVA: 0x0027DEA4 File Offset: 0x0027C0A4
		public static short[] GetMemberResourcesAdjust(short orgMemberId)
		{
			return OrganizationMember.Instance[orgMemberId].ResourcesAdjust;
		}

		// Token: 0x0600471D RID: 18205 RVA: 0x0027DEC8 File Offset: 0x0027C0C8
		public static short[] GetMemberMainAttributesAdjust(short orgMemberId)
		{
			return OrganizationMember.Instance[orgMemberId].MainAttributesAdjust;
		}

		// Token: 0x0600471E RID: 18206 RVA: 0x0027DEEC File Offset: 0x0027C0EC
		public static short[] GetMemberLifeSkillsAdjust(short orgMemberId)
		{
			return OrganizationMember.Instance[orgMemberId].LifeSkillsAdjust;
		}

		// Token: 0x0600471F RID: 18207 RVA: 0x0027DF10 File Offset: 0x0027C110
		public static short[] GetMemberCombatSkillsAdjust(short orgMemberId)
		{
			return OrganizationMember.Instance[orgMemberId].CombatSkillsAdjust;
		}

		// Token: 0x06004720 RID: 18208 RVA: 0x0027DF34 File Offset: 0x0027C134
		public static string GetMonasticTitleSuffix(sbyte orgTemplateId, sbyte grade, sbyte gender)
		{
			OrganizationItem orgConfig = Organization.Instance[orgTemplateId];
			short orgMemberId = orgConfig.Members[(int)grade];
			OrganizationMemberItem orgMemberConfig = OrganizationMember.Instance[orgMemberId];
			return orgMemberConfig.MonasticTitleSuffixes[(int)gender];
		}

		// Token: 0x06004721 RID: 18209 RVA: 0x0027DF70 File Offset: 0x0027C170
		[return: TupleElementNames(new string[]
		{
			"first",
			"last"
		})]
		public static ValueTuple<short, short> GetSeniorityRange(sbyte seniorityGroupId)
		{
			LocalMonasticTitles config = LocalMonasticTitles.Instance;
			if (!true)
			{
			}
			ValueTuple<short, short> result;
			switch (seniorityGroupId)
			{
			case 0:
				result = new ValueTuple<short, short>(config.SeniorityShaolinStart, config.SeniorityShaolinEnd);
				break;
			case 1:
				result = new ValueTuple<short, short>(config.SeniorityEmeiStart, config.SeniorityEmeiEnd);
				break;
			case 2:
				result = new ValueTuple<short, short>(config.SeniorityWudangStart, config.SeniorityWudangEnd);
				break;
			case 3:
				result = new ValueTuple<short, short>(config.SeniorityRanshanStart, config.SeniorityRanshanEnd);
				break;
			default:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(29, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Unsupported seniorityGroupId ");
				defaultInterpolatedStringHandler.AppendFormatted<sbyte>(seniorityGroupId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			}
			if (!true)
			{
			}
			return result;
		}

		// Token: 0x06004722 RID: 18210 RVA: 0x0027E028 File Offset: 0x0027C228
		[return: TupleElementNames(new string[]
		{
			"first",
			"last"
		})]
		public static ValueTuple<short, short> GetMonasticTitleSuffixRange(sbyte seniorityGroupId)
		{
			LocalMonasticTitles config = LocalMonasticTitles.Instance;
			if (!true)
			{
			}
			ValueTuple<short, short> result;
			switch (seniorityGroupId)
			{
			case 0:
				result = new ValueTuple<short, short>(config.SuffixBuddhistStart, config.SuffixBuddhistEnd);
				break;
			case 1:
				result = new ValueTuple<short, short>(config.SuffixBuddhistStart, config.SuffixBuddhistEnd);
				break;
			case 2:
				result = new ValueTuple<short, short>(config.SuffixTaoistStart, config.SuffixTaoistEnd);
				break;
			case 3:
				result = new ValueTuple<short, short>(config.SuffixTaoistStart, config.SuffixTaoistEnd);
				break;
			default:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(29, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Unsupported seniorityGroupId ");
				defaultInterpolatedStringHandler.AppendFormatted<sbyte>(seniorityGroupId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			}
			if (!true)
			{
			}
			return result;
		}

		// Token: 0x06004723 RID: 18211 RVA: 0x0027E0E0 File Offset: 0x0027C2E0
		public static short GetNextSeniorityId(sbyte seniorityGroupId, short currSeniorityId)
		{
			ValueTuple<short, short> seniorityRange = OrganizationDomain.GetSeniorityRange(seniorityGroupId);
			short firstId = seniorityRange.Item1;
			short lastId = seniorityRange.Item2;
			short nextId = currSeniorityId + 1;
			return (nextId > lastId) ? firstId : nextId;
		}

		// Token: 0x06004724 RID: 18212 RVA: 0x0027E112 File Offset: 0x0027C312
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static OrganizationMemberItem GetOrgMemberConfig(OrganizationInfo orgInfo)
		{
			return orgInfo.GetOrgMemberConfig();
		}

		// Token: 0x06004725 RID: 18213 RVA: 0x0027E11C File Offset: 0x0027C31C
		public static OrganizationMemberItem GetOrgMemberConfig(sbyte orgTemplateId, sbyte grade)
		{
			OrganizationItem orgConfig = Organization.Instance[orgTemplateId];
			short orgMemberId = orgConfig.Members[(int)grade];
			return OrganizationMember.Instance[orgMemberId];
		}

		// Token: 0x06004726 RID: 18214 RVA: 0x0027E150 File Offset: 0x0027C350
		public static short GetInitialAge(OrganizationMemberItem orgMemberCfg)
		{
			byte lifespanType = DomainManager.World.GetCharacterLifespanType();
			return orgMemberCfg.InitialAges[(int)lifespanType];
		}

		// Token: 0x06004727 RID: 18215 RVA: 0x0027E178 File Offset: 0x0027C378
		public static short GetCharacterTemplateId(sbyte orgTemplateId, sbyte mapStateTemplateId, sbyte gender)
		{
			short charTemplateId = Organization.Instance[orgTemplateId].CharTemplateIds[(int)gender];
			return (charTemplateId >= 0) ? charTemplateId : MapDomain.GetCharacterTemplateId(mapStateTemplateId, gender);
		}

		// Token: 0x06004728 RID: 18216 RVA: 0x0027E1AC File Offset: 0x0027C3AC
		public static bool CanInteractWithType(GameData.Domains.Character.Character character, sbyte type)
		{
			OrganizationInfo orgInfo = character.GetOrganizationInfo();
			OrganizationMemberItem config = OrganizationDomain.GetOrgMemberConfig(orgInfo);
			bool flag = config == null;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				short currAge = character.GetCurrAge();
				bool flag2 = currAge < config.IdentityActiveAge;
				result = (!flag2 && config.IdentityInteractConfig.Contains(type));
			}
			return result;
		}

		// Token: 0x06004729 RID: 18217 RVA: 0x0027E204 File Offset: 0x0027C404
		private static void InitializeSectOrgTemplateIds()
		{
			List<sbyte> allSectIds = new List<sbyte>();
			List<sbyte> femaleSectIds = new List<sbyte>();
			List<sbyte> maleSectIds = new List<sbyte>();
			foreach (OrganizationItem item in ((IEnumerable<OrganizationItem>)Organization.Instance))
			{
				bool flag = !item.IsSect;
				if (!flag)
				{
					allSectIds.Add(item.TemplateId);
					switch (item.GenderRestriction)
					{
					case -1:
						maleSectIds.Add(item.TemplateId);
						femaleSectIds.Add(item.TemplateId);
						break;
					case 0:
						femaleSectIds.Add(item.TemplateId);
						break;
					case 1:
						maleSectIds.Add(item.TemplateId);
						break;
					}
				}
			}
			OrganizationDomain._allSectOrgTemplateIds = allSectIds.ToArray();
			OrganizationDomain._femaleSectOrgTemplateIds = femaleSectIds.ToArray();
			OrganizationDomain._maleSectOrgTemplateIds = maleSectIds.ToArray();
		}

		// Token: 0x0600472A RID: 18218 RVA: 0x0027E30C File Offset: 0x0027C50C
		public void Test_ContributionInfluencePowerBonus()
		{
			SettlementTreasury treasury = new SettlementTreasury();
			treasury.Contributions.Add(0, Config.Accessory.Instance[8].BaseValue);
			treasury.Contributions.Add(1, Config.Accessory.Instance[8].BaseValue * 10);
			Tester.Assert(treasury.CalcBonusInfluencePower(0) == 110, "");
			Tester.Assert(treasury.CalcBonusInfluencePower(1) == 200, "");
			Tester.Assert(treasury.CalcBonusInfluencePower(2) == 100, "");
		}

		// Token: 0x0600472B RID: 18219 RVA: 0x0027E3A4 File Offset: 0x0027C5A4
		public SettlementTreasury GetTreasury(OrganizationInfo info)
		{
			bool flag = info.OrgTemplateId == 16;
			SettlementTreasury result;
			if (flag)
			{
				result = DomainManager.Taiwu.GetTaiwuTreasury();
			}
			else
			{
				Settlement settlement = DomainManager.Organization.GetSettlement(info.SettlementId);
				result = settlement.GetTreasury(info.Grade);
			}
			return result;
		}

		// Token: 0x0600472C RID: 18220 RVA: 0x0027E3F0 File Offset: 0x0027C5F0
		public SettlementTreasury GetTreasury(short settlementId, sbyte layerIndex)
		{
			Settlement settlement = DomainManager.Organization.GetSettlement(settlementId);
			return settlement.Treasuries.SettlementTreasuries[(int)layerIndex];
		}

		// Token: 0x0600472D RID: 18221 RVA: 0x0027E41C File Offset: 0x0027C61C
		public void StoreItemInTreasury(DataContext context, short settlementId, GameData.Domains.Character.Character character, ItemKey itemKey, int amount, sbyte layerIndex = -1)
		{
			Settlement settlement = DomainManager.Organization.GetSettlement(settlementId);
			sbyte orgTemplateId = settlement.GetOrgTemplateId();
			bool flag = orgTemplateId == 16;
			if (flag)
			{
				bool flag2 = character.GetId() == DomainManager.Taiwu.GetTaiwuCharId();
				if (flag2)
				{
					DomainManager.Taiwu.StoreItemInTreasury(context, itemKey, amount, false);
				}
				else
				{
					DomainManager.Taiwu.VillagerStoreItemInTreasury(context, character, itemKey, amount, true);
				}
			}
			else
			{
				settlement.StoreItemInTreasury(context, character, itemKey, amount, layerIndex);
			}
		}

		// Token: 0x0600472E RID: 18222 RVA: 0x0027E494 File Offset: 0x0027C694
		public void TakeItemFromTreasury(DataContext context, short settlementId, GameData.Domains.Character.Character character, ItemKey itemKey, int amount, bool deleteItem = false)
		{
			Settlement settlement = DomainManager.Organization.GetSettlement(settlementId);
			sbyte orgTemplateId = settlement.GetOrgTemplateId();
			bool flag = orgTemplateId == 16;
			if (flag)
			{
				bool flag2 = character.GetId() == DomainManager.Taiwu.GetTaiwuCharId();
				if (flag2)
				{
					DomainManager.Taiwu.TakeItemFromTreasury(context, itemKey, amount, deleteItem, false);
				}
				else
				{
					DomainManager.Taiwu.VillagerTakeItemFromTreasury(context, character, itemKey, amount);
				}
			}
			else
			{
				settlement.TakeItemFromTreasury(context, character, itemKey, amount);
			}
		}

		// Token: 0x0600472F RID: 18223 RVA: 0x0027E50C File Offset: 0x0027C70C
		public void StoreResourceInTreasury(DataContext context, short settlementId, GameData.Domains.Character.Character character, sbyte resourceType, int amount, sbyte layerIndex = -1)
		{
			Settlement settlement = DomainManager.Organization.GetSettlement(settlementId);
			sbyte orgTemplateId = settlement.GetOrgTemplateId();
			bool flag = orgTemplateId == 16;
			if (flag)
			{
				bool flag2 = character.GetId() == DomainManager.Taiwu.GetTaiwuCharId();
				if (flag2)
				{
					DomainManager.Taiwu.StoreResourceInTreasury(context, resourceType, amount);
				}
				else
				{
					DomainManager.Taiwu.VillagerStoreResourceInTreasury(context, character, resourceType, amount);
				}
			}
			else
			{
				settlement.StoreResourceInTreasury(context, character, resourceType, amount, layerIndex);
			}
		}

		// Token: 0x06004730 RID: 18224 RVA: 0x0027E584 File Offset: 0x0027C784
		public void TakeResourceFromTreasury(DataContext context, short settlementId, GameData.Domains.Character.Character character, sbyte resourceType, int amount, sbyte layerIndex = -1)
		{
			Settlement settlement = DomainManager.Organization.GetSettlement(settlementId);
			sbyte orgTemplateId = settlement.GetOrgTemplateId();
			bool flag = orgTemplateId == 16;
			if (flag)
			{
				bool flag2 = character.GetId() == DomainManager.Taiwu.GetTaiwuCharId();
				if (flag2)
				{
					DomainManager.Taiwu.TakeResourceFromTreasury(context, resourceType, amount);
				}
				else
				{
					DomainManager.Taiwu.VillagerTakeResourceFromTreasury(context, character, resourceType, amount);
				}
			}
			else
			{
				settlement.TakeResourceFromTreasury(context, character, resourceType, amount, layerIndex);
			}
		}

		// Token: 0x06004731 RID: 18225 RVA: 0x0027E5FC File Offset: 0x0027C7FC
		public int CalcResourceContribution(sbyte orgTemplateId, sbyte resourceType, int amount)
		{
			OrganizationMemberItem memberConfig = OrganizationDomain.GetOrgMemberConfig(orgTemplateId, 8);
			return memberConfig.AdjustResourceValue(resourceType, amount) * GlobalConfig.Instance.ResourceContributionPercent / 100;
		}

		// Token: 0x06004732 RID: 18226 RVA: 0x0027E62C File Offset: 0x0027C82C
		public int CalcItemContribution(Settlement settlement, ItemKey itemKey, int amount)
		{
			return (settlement.GetOrgTemplateId() != 16) ? settlement.CalcItemContribution(itemKey, amount) : DomainManager.Taiwu.CalcItemContribution(itemKey, amount);
		}

		// Token: 0x06004733 RID: 18227 RVA: 0x0027E660 File Offset: 0x0027C860
		public bool IsCharacterTreasuryGuard(int charId)
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
				bool flag2 = character.GetOrganizationInfo().SettlementId < 0;
				result = (!flag2 && this.GetSettlement(character.GetOrganizationInfo().SettlementId).Treasuries.IsGuard(charId));
			}
			return result;
		}

		// Token: 0x06004734 RID: 18228 RVA: 0x0027E6BC File Offset: 0x0027C8BC
		public byte CharacterTreasuryGuardInfo(int charId)
		{
			GameData.Domains.Character.Character character;
			bool flag = !DomainManager.Character.TryGetElement_Objects(charId, out character);
			byte result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				bool flag2 = character.GetOrganizationInfo().SettlementId < 0;
				if (flag2)
				{
					result = 0;
				}
				else
				{
					byte res = this.GetSettlement(character.GetOrganizationInfo().SettlementId).Treasuries.GuardLevel(charId);
					bool flag3 = res != 0 && Settlement.IsGuarding(charId, false);
					if (flag3)
					{
						result = (res | 4);
					}
					else
					{
						result = res;
					}
				}
			}
			return result;
		}

		// Token: 0x06004735 RID: 18229 RVA: 0x0027E738 File Offset: 0x0027C938
		public void InitializeOwnedItems()
		{
			short taiwuSettlementId = DomainManager.Taiwu.GetTaiwuVillageSettlementId();
			foreach (KeyValuePair<short, Settlement> keyValuePair in this._settlements)
			{
				short num;
				Settlement settlement;
				keyValuePair.Deconstruct(out num, out settlement);
				short settlementId = num;
				bool flag = settlementId == taiwuSettlementId;
				if (flag)
				{
					SettlementTreasury treasury;
					bool flag2 = !DomainManager.Extra.TryGetElement_SettlementTreasuries(taiwuSettlementId, out treasury);
					if (!flag2)
					{
						foreach (KeyValuePair<ItemKey, int> keyValuePair2 in treasury.Inventory.Items)
						{
							ItemKey itemKey3;
							int num2;
							keyValuePair2.Deconstruct(out itemKey3, out num2);
							ItemKey itemKey = itemKey3;
							DomainManager.Item.SetOwner(itemKey, ItemOwnerType.Treasury, (int)settlementId);
						}
					}
				}
				else
				{
					SettlementLayeredTreasuries treasuries;
					bool flag3 = !DomainManager.Extra.TryGetElement_SettlementLayeredTreasuries(settlementId, out treasuries);
					if (!flag3)
					{
						foreach (SettlementTreasury treasury2 in treasuries.SettlementTreasuries)
						{
							foreach (KeyValuePair<ItemKey, int> keyValuePair2 in treasury2.Inventory.Items)
							{
								ItemKey itemKey3;
								int num2;
								keyValuePair2.Deconstruct(out itemKey3, out num2);
								ItemKey itemKey2 = itemKey3;
								DomainManager.Item.SetOwner(itemKey2, ItemOwnerType.Treasury, (int)settlementId);
							}
						}
					}
				}
			}
		}

		// Token: 0x06004736 RID: 18230 RVA: 0x0027E904 File Offset: 0x0027CB04
		public void InitializeSettlementTreasury()
		{
			this._firstGuardCharId = -1;
			this._itemSourceChanges = null;
			this._getTreasuryInventory = null;
			this._stealTreasuryInventory = null;
			this._exchangeTreasuryInventory = null;
			this._storeTreasuryInventory = null;
			this._currentTreasuryLayer = SettlementTreasuryLayers.Shallow;
		}

		// Token: 0x06004737 RID: 18231 RVA: 0x0027E938 File Offset: 0x0027CB38
		public void SetCurrentTreasuryLayer(sbyte layerIndex)
		{
			this._currentTreasuryLayer = (SettlementTreasuryLayers)layerIndex;
		}

		// Token: 0x06004738 RID: 18232 RVA: 0x0027E944 File Offset: 0x0027CB44
		public sbyte GetCurrentTreasuryLayer()
		{
			return (sbyte)this._currentTreasuryLayer;
		}

		// Token: 0x06004739 RID: 18233 RVA: 0x0027E95D File Offset: 0x0027CB5D
		public void SetSettlementTreasuryFirstGuardChar(int charId)
		{
			this._firstGuardCharId = charId;
		}

		// Token: 0x0600473A RID: 18234 RVA: 0x0027E968 File Offset: 0x0027CB68
		public void SetSettlementTreasuryAlterTime(DataContext context, short settlementId, byte time)
		{
			Settlement settlement = this._settlements[settlementId];
			settlement.SetAlterTime(context, time);
		}

		// Token: 0x0600473B RID: 18235 RVA: 0x0027E98C File Offset: 0x0027CB8C
		public byte GetSettlementTreasuryAlterTime(DataContext context, short settlementId)
		{
			Settlement settlement = this._settlements[settlementId];
			return settlement.GetAlterTime(context);
		}

		// Token: 0x0600473C RID: 18236 RVA: 0x0027E9B4 File Offset: 0x0027CBB4
		[DomainMethod]
		public SettlementTreasuryDisplayData GetSettlementTreasuryDisplayData(DataContext context, short settlementId, sbyte layerIndex)
		{
			Settlement settlement = DomainManager.Organization.GetSettlement(settlementId);
			sbyte orgTemplateId = settlement.GetOrgTemplateId();
			OrganizationItem orgConfig = Organization.Instance[orgTemplateId];
			SettlementTreasury settlementTreasury = settlement.Treasuries.GetTreasury(layerIndex);
			SettlementTreasuryDisplayData settlementTreasuryDisplayData = new SettlementTreasuryDisplayData
			{
				SettlementTreasury = settlementTreasury,
				AlertTime = settlement.Treasuries.AlertTime,
				SupplyLevel = settlement.GetSupplyLevel(),
				DebtOrSupport = (int)settlement.CalcApprovingRate(),
				GuardianCharacterDisplayDataLow = settlement.GetGuardsDisplayData(context, 0),
				GuardianCharacterDisplayDataMid = settlement.GetGuardsDisplayData(context, 1),
				GuardianCharacterDisplayDataHigh = settlement.GetGuardsDisplayData(context, 2),
				OrgTemplateId = (int)orgTemplateId,
				SupplyItems = settlement.GetSupplyItems(),
				InfluenceRefreshTime = (byte)Math.Clamp(settlement.GetInfluencePowerUpdateDate() - DomainManager.World.GetCurrDate(), 0, 256),
				SettlementNameRelatedData = settlement.GetNameRelatedData(),
				ResourceStatus = settlement.Treasuries.GetTreasuryResourceStatus()
			};
			bool isSect = orgConfig.IsSect;
			if (isSect)
			{
				sbyte sectMainStoryTaskStatus = DomainManager.World.GetSectMainStoryTaskStatus(orgTemplateId);
				settlementTreasuryDisplayData.SectStoryEnding = sectMainStoryTaskStatus;
				short winner = DomainManager.Extra.GetLastMartialArtTournamentWinner();
				settlementTreasuryDisplayData.MartialArtTournamentResult = ((short)orgTemplateId == winner);
			}
			return settlementTreasuryDisplayData;
		}

		// Token: 0x0600473D RID: 18237 RVA: 0x0027EAF8 File Offset: 0x0027CCF8
		[DomainMethod]
		public static bool[] CheckSettlementGuardFavorabilityType(DataContext context, short settlementId)
		{
			Settlement settlement = DomainManager.Organization.GetSettlement(settlementId);
			int layerCount = Enum.GetValues(typeof(SettlementTreasuryLayers)).Length;
			bool[] res = new bool[layerCount];
			res[0] = true;
			sbyte layerIndex = 1;
			while ((int)layerIndex < layerCount)
			{
				short favor = settlement.GetGuardsAndFavors(context, layerIndex).First<ValueTuple<GameData.Domains.Character.Character, short>>().Item2;
				sbyte favorabilityType = FavorabilityType.GetFavorabilityType(favor);
				res[(int)layerIndex] = (favorabilityType >= 4);
				layerIndex += 1;
			}
			return res;
		}

		// Token: 0x0600473E RID: 18238 RVA: 0x0027EB76 File Offset: 0x0027CD76
		[DomainMethod]
		public SettlementTreasuryRecordCollection GetSettlementTreasuryRecordCollection(DataContext context, short settlementId)
		{
			return DomainManager.Extra.GetSettlementTreasuryRecordCollection(context, settlementId);
		}

		// Token: 0x0600473F RID: 18239 RVA: 0x0027EB84 File Offset: 0x0027CD84
		public void SetSettlementTreasuryRecordCollection(DataContext context, short settlementId, SettlementTreasuryRecordCollection collection)
		{
			DomainManager.Extra.SetSettlementTreasuryRecordCollection(context, settlementId, collection);
		}

		// Token: 0x06004740 RID: 18240 RVA: 0x0027EB94 File Offset: 0x0027CD94
		[DomainMethod]
		public void ConfirmSettlementTreasuryOperation(DataContext context, int needAuthority, List<ItemSourceChange> itemSourceChanges, Inventory getTreasuryInventory, Inventory stealTreasuryInventory, Inventory exchangeTreasuryInventory, Inventory storeTreasuryInventory)
		{
			this._itemSourceChanges = itemSourceChanges;
			this._getTreasuryInventory = getTreasuryInventory;
			this._stealTreasuryInventory = stealTreasuryInventory;
			this._exchangeTreasuryInventory = exchangeTreasuryInventory;
			this._storeTreasuryInventory = storeTreasuryInventory;
			bool flag = stealTreasuryInventory != null && stealTreasuryInventory.InventoryItemTotalCount > 0;
			OrganizationDomain.ESettlementTreasuryOperationResult operationResult;
			if (flag)
			{
				operationResult = OrganizationDomain.ESettlementTreasuryOperationResult.Steal;
			}
			else
			{
				bool flag2 = getTreasuryInventory != null && getTreasuryInventory.InventoryItemTotalCount > 0 && exchangeTreasuryInventory != null && exchangeTreasuryInventory.InventoryItemTotalCount > 0;
				if (flag2)
				{
					int count = 0;
					foreach (ItemSourceChange itemSourceChange in this._itemSourceChanges)
					{
						foreach (ItemKeyAndCount itemKeyAndCount in itemSourceChange.Items)
						{
							ItemKey itemKey3;
							int num;
							itemKeyAndCount.Deconstruct(out itemKey3, out num);
							ItemKey itemKey = itemKey3;
							int countDelta = num;
							bool flag3 = ItemTemplateHelper.IsMiscResource(itemKey.ItemType, itemKey.TemplateId);
							if (flag3)
							{
								count += ((countDelta >= 0) ? 1 : -1);
							}
							else
							{
								count += countDelta;
							}
						}
					}
					if (!true)
					{
					}
					OrganizationDomain.ESettlementTreasuryOperationResult esettlementTreasuryOperationResult;
					if (count <= 0)
					{
						if (count != 0)
						{
							esettlementTreasuryOperationResult = OrganizationDomain.ESettlementTreasuryOperationResult.Store;
						}
						else
						{
							esettlementTreasuryOperationResult = OrganizationDomain.ESettlementTreasuryOperationResult.Exchange;
						}
					}
					else
					{
						esettlementTreasuryOperationResult = OrganizationDomain.ESettlementTreasuryOperationResult.Steal;
					}
					if (!true)
					{
					}
					operationResult = esettlementTreasuryOperationResult;
				}
				else
				{
					bool flag4 = storeTreasuryInventory != null && storeTreasuryInventory.InventoryItemTotalCount > 0;
					if (flag4)
					{
						operationResult = OrganizationDomain.ESettlementTreasuryOperationResult.Store;
					}
					else
					{
						operationResult = OrganizationDomain.ESettlementTreasuryOperationResult.None;
					}
				}
			}
			DomainManager.TaiwuEvent.SetListenerEventActionIntArg("ShopActionComplete", "OperationResult", (int)operationResult);
			DomainManager.TaiwuEvent.SetListenerEventActionIntArg("ShopActionComplete", "NeedAuthority", needAuthority);
			DomainManager.TaiwuEvent.SetListenerEventActionISerializableArg("ShopActionComplete", "StealTreasuryInventory", stealTreasuryInventory);
			DomainManager.TaiwuEvent.SetListenerEventActionISerializableArg("ShopActionComplete", "StoreTreasuryInventory", storeTreasuryInventory);
			bool flag5 = itemSourceChanges == null || itemSourceChanges.Count == 0;
			if (!flag5)
			{
				foreach (ItemSourceChange itemSourceChange2 in this._itemSourceChanges)
				{
					foreach (ItemKeyAndCount itemKeyAndCount in itemSourceChange2.Items)
					{
						ItemKey itemKey3;
						int num;
						itemKeyAndCount.Deconstruct(out itemKey3, out num);
						ItemKey itemKey2 = itemKey3;
						int countDelta2 = num;
						bool flag6 = countDelta2 < 0;
						if (flag6)
						{
							bool flag7 = ItemTemplateHelper.IsMiscResource(itemKey2.ItemType, itemKey2.TemplateId);
							if (flag7)
							{
								sbyte resourceType = ItemTemplateHelper.GetMiscResourceType(itemKey2.ItemType, itemKey2.TemplateId);
								DomainManager.Taiwu.GetTaiwu().ChangeResource(context, resourceType, countDelta2);
							}
							else
							{
								DomainManager.Taiwu.RemoveItem(context, itemKey2, -countDelta2, itemSourceChange2.ItemSourceType, false, false);
							}
						}
					}
				}
			}
		}

		// Token: 0x06004741 RID: 18241 RVA: 0x0027EEAC File Offset: 0x0027D0AC
		[DomainMethod]
		public List<ItemSourceChange> GetLastSettlementTreasuryOperationData()
		{
			return this._itemSourceChanges;
		}

		// Token: 0x06004742 RID: 18242 RVA: 0x0027EEC4 File Offset: 0x0027D0C4
		[return: TupleElementNames(new string[]
		{
			"changeFavorCharIds",
			"becomeEnemyCharIds",
			"approveCharIds",
			"disapproveCharIds"
		})]
		public ValueTuple<CharacterSet, CharacterSet, CharacterSet, CharacterSet> ApplySettlementTreasuryEventEffect(DataContext context, SettlementTreasuryEventEffectItem effectCfg, short settlementId, int totalWorth, sbyte layerIndex)
		{
			Settlement settlement = this.GetSettlement(settlementId);
			OrgMemberCollection members = settlement.GetMembers();
			PunishmentTypeItem punishment;
			bool flag;
			if (effectCfg.TaiwuBounty >= 0)
			{
				punishment = PunishmentType.Instance[effectCfg.TaiwuBounty];
				flag = (punishment != null);
			}
			else
			{
				flag = false;
			}
			bool flag2 = flag;
			if (flag2)
			{
				sbyte orgTemplateId = MapDomain.GetSectOrgTemplateIdByStateTemplateId(DomainManager.Map.GetStateTemplateIdByAreaId(settlement.GetLocation().AreaId));
				Sect sect = DomainManager.Organization.GetSettlementByOrgTemplateId(orgTemplateId) as Sect;
				sect.AddBounty(context, DomainManager.Taiwu.GetTaiwu(), punishment.Severity, effectCfg.TaiwuBounty, -1);
			}
			bool flag3 = effectCfg.AlterTime > 0;
			if (flag3)
			{
				settlement.SetAlterTime(context, (byte)effectCfg.AlterTime);
			}
			HashSet<int> guardIds = settlement.Treasuries.GetTreasury(layerIndex).GuardIds.GetCollection();
			SettlementTreasuryEventEffectHelper.EffectArgs guardArgs = new SettlementTreasuryEventEffectHelper.EffectArgs(effectCfg, guardIds.Count, totalWorth, true);
			this.ApplySettlementTreasuryEventEffect(context, guardIds, ref guardArgs);
			List<int> charIdList = ObjectPool<List<int>>.Instance.Get();
			charIdList.Clear();
			for (sbyte grade = 0; grade <= 8; grade += 1)
			{
				IEnumerable<int> gradeMembers = DomainManager.Character.ExcludeInfant(members.GetMembers(grade));
				foreach (int charId in gradeMembers)
				{
					bool flag4 = guardIds.Contains(charId);
					if (!flag4)
					{
						charIdList.Add(charId);
					}
				}
			}
			CollectionUtils.Shuffle<int>(context.Random, charIdList);
			SettlementTreasuryEventEffectHelper.EffectArgs memberArgs = new SettlementTreasuryEventEffectHelper.EffectArgs(effectCfg, charIdList.Count, totalWorth, false)
			{
				ChangeFavorCharIds = guardArgs.ChangeFavorCharIds,
				ApproveCharIds = guardArgs.ApproveCharIds,
				DisapproveCharIds = guardArgs.DisapproveCharIds,
				BecomeEnemyCharIds = guardArgs.BecomeEnemyCharIds
			};
			this.ApplySettlementTreasuryEventEffect(context, charIdList, ref memberArgs);
			Location location = settlement.GetLocation();
			int spiritualDebtChange = effectCfg.CalcSpiritualDebtChange(totalWorth);
			bool flag5 = spiritualDebtChange != 0;
			if (flag5)
			{
				DomainManager.Extra.ChangeAreaSpiritualDebt(context, location.AreaId, spiritualDebtChange, true, true);
			}
			ObjectPool<List<int>>.Instance.Return(charIdList);
			return new ValueTuple<CharacterSet, CharacterSet, CharacterSet, CharacterSet>(memberArgs.ChangeFavorCharIds, memberArgs.BecomeEnemyCharIds, memberArgs.ApproveCharIds, memberArgs.DisapproveCharIds);
		}

		// Token: 0x06004743 RID: 18243 RVA: 0x0027F118 File Offset: 0x0027D318
		public void ApplySettlementTreasuryEventEffect(DataContext context, IEnumerable<int> charIds, ref SettlementTreasuryEventEffectHelper.EffectArgs args)
		{
			GameData.Domains.Character.Character taiwuChar = DomainManager.Taiwu.GetTaiwu();
			int taiwuCharId = taiwuChar.GetId();
			int currChangeFavorCount = 0;
			int currDisapproveCount = 0;
			int currApproveCount = 0;
			int currBecomeEnemyCount = 0;
			foreach (int charId in charIds)
			{
				bool flag = currChangeFavorCount >= args.ChangeFavorCount;
				if (flag)
				{
					break;
				}
				GameData.Domains.Character.Character guardChar;
				bool flag2 = !DomainManager.Character.TryGetElement_Objects(charId, out guardChar);
				if (!flag2)
				{
					bool flag3 = guardChar.GetCreatingType() != 1;
					if (!flag3)
					{
						bool flag4 = DomainManager.Organization.GetPrisonerSect(charId) >= 0;
						if (!flag4)
						{
							short prevFavorability = DomainManager.Character.GetFavorability(guardChar.GetId(), taiwuChar.GetId());
							bool flag5 = prevFavorability == short.MinValue;
							if (flag5)
							{
								DomainManager.Character.TryCreateGeneralRelation(context, guardChar, taiwuChar);
								prevFavorability = DomainManager.Character.GetFavorability(guardChar.GetId(), taiwuChar.GetId());
							}
							int delta = DomainManager.Character.CalcFavorabilityDelta(charId, taiwuCharId, args.FavorChange, -1);
							bool flag6 = delta == 0;
							if (!flag6)
							{
								DomainManager.Character.DirectlyChangeFavorabilityOptional(context, guardChar, taiwuChar, args.FavorChange, 3);
								DomainManager.Character.AddFavorabilityChangeInstantNotification(guardChar, taiwuChar, delta > 0);
								args.ChangeFavorCharIds.Add(charId);
								bool flag7 = currBecomeEnemyCount < args.BecomeEnemyCount && !DomainManager.Character.HasRelation(charId, taiwuCharId, 32768);
								if (flag7)
								{
									DomainManager.Character.AddRelation(context, charId, taiwuCharId, 32768, int.MinValue);
									args.BecomeEnemyCharIds.Add(charId);
									currBecomeEnemyCount++;
								}
								SettlementCharacter settlementChar = this.GetSettlementCharacter(charId);
								bool approvedTaiwu = settlementChar.GetApprovedTaiwu();
								if (approvedTaiwu)
								{
									bool flag8 = currDisapproveCount < args.DisapproveCount;
									if (flag8)
									{
										settlementChar.SetApprovedTaiwu(context, false);
										args.DisapproveCharIds.Add(charId);
										currDisapproveCount++;
									}
								}
								else
								{
									bool flag9 = currApproveCount < args.ApproveCount;
									if (flag9)
									{
										settlementChar.SetApprovedTaiwu(context, true);
										args.ApproveCharIds.Add(charId);
										currApproveCount++;
									}
								}
								currChangeFavorCount++;
							}
						}
					}
				}
			}
		}

		// Token: 0x06004744 RID: 18244 RVA: 0x0027F380 File Offset: 0x0027D580
		public void SettleTreasuryOperate(DataContext context, short settlementId, bool takeEffect, bool startCombat, bool isSect, bool restart)
		{
			if (startCombat)
			{
				DomainManager.Organization.SetSettlementTreasuryAlterTime(context, settlementId, (byte)GlobalConfig.Instance.SettlementAlterTime);
			}
			bool flag = this._itemSourceChanges == null;
			if (flag)
			{
				this.InitializeSettlementTreasury();
			}
			else
			{
				int charId = DomainManager.Taiwu.GetTaiwuCharId();
				foreach (ItemSourceChange itemSourceChange in this._itemSourceChanges)
				{
					foreach (ItemKeyAndCount itemKeyAndCount in itemSourceChange.Items)
					{
						ItemKey itemKey3;
						int num;
						itemKeyAndCount.Deconstruct(out itemKey3, out num);
						ItemKey itemKey = itemKey3;
						int countDelta = num;
						bool flag2 = countDelta < 0;
						if (flag2)
						{
							bool flag3 = ItemTemplateHelper.IsMiscResource(itemKey.ItemType, itemKey.TemplateId);
							if (flag3)
							{
								sbyte resourceType = ItemTemplateHelper.GetMiscResourceType(itemKey.ItemType, itemKey.TemplateId);
								DomainManager.Taiwu.GetTaiwu().ChangeResource(context, resourceType, -countDelta);
							}
							else
							{
								DomainManager.Taiwu.AddItem(context, itemKey, -countDelta, itemSourceChange.ItemSourceType, false);
							}
						}
					}
				}
				if (!restart)
				{
					if (takeEffect)
					{
						foreach (ItemSourceChange itemSourceChange2 in this._itemSourceChanges)
						{
							foreach (ItemKeyAndCount itemKeyAndCount in itemSourceChange2.Items)
							{
								ItemKey itemKey3;
								int num;
								itemKeyAndCount.Deconstruct(out itemKey3, out num);
								ItemKey itemKey2 = itemKey3;
								int countDelta2 = num;
								bool flag4 = countDelta2 > 0;
								if (flag4)
								{
									bool flag5 = this._stealTreasuryInventory.InventoryItemTotalCount > 0;
									if (flag5)
									{
										SettlementTreasuryRecordCollection settlementTreasuryRecordCollection = DomainManager.Organization.GetSettlementTreasuryRecordCollection(context, settlementId);
										int currDate = DomainManager.World.GetCurrDate();
										if (isSect)
										{
											settlementTreasuryRecordCollection.AddPlunderSectTreasurySuccess(currDate, settlementId, charId);
										}
										else
										{
											settlementTreasuryRecordCollection.AddPlunderTownTreasurySuccess(currDate, settlementId, charId);
										}
										DomainManager.Organization.SetSettlementTreasuryRecordCollection(context, settlementId, settlementTreasuryRecordCollection);
									}
									bool flag6 = ItemTemplateHelper.IsMiscResource(itemKey2.ItemType, itemKey2.TemplateId);
									if (flag6)
									{
										GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
										sbyte resourceType2 = ItemTemplateHelper.GetMiscResourceType(itemKey2.ItemType, itemKey2.TemplateId);
										DomainManager.Organization.TakeResourceFromTreasury(context, settlementId, taiwu, resourceType2, countDelta2, -1);
										DomainManager.Taiwu.GetTaiwu().ChangeResource(context, resourceType2, countDelta2);
									}
									else
									{
										DomainManager.Organization.TakeItemFromTreasury(context, settlementId, DomainManager.Taiwu.GetTaiwu(), itemKey2, countDelta2, false);
										DomainManager.Taiwu.AddItem(context, itemKey2, countDelta2, itemSourceChange2.ItemSourceType, false);
									}
								}
								else
								{
									bool flag7 = countDelta2 < 0;
									if (flag7)
									{
										bool flag8 = ItemTemplateHelper.IsMiscResource(itemKey2.ItemType, itemKey2.TemplateId);
										if (flag8)
										{
											GameData.Domains.Character.Character taiwu2 = DomainManager.Taiwu.GetTaiwu();
											sbyte resourceType3 = ItemTemplateHelper.GetMiscResourceType(itemKey2.ItemType, itemKey2.TemplateId);
											taiwu2.ChangeResource(context, resourceType3, countDelta2);
											DomainManager.Organization.StoreResourceInTreasury(context, settlementId, taiwu2, resourceType3, -countDelta2, this.GetCurrentTreasuryLayer());
										}
										else
										{
											DomainManager.Taiwu.RemoveItem(context, itemKey2, -countDelta2, itemSourceChange2.ItemSourceType, false, false);
											DomainManager.Organization.StoreItemInTreasury(context, settlementId, DomainManager.Taiwu.GetTaiwu(), itemKey2, -countDelta2, this.GetCurrentTreasuryLayer());
										}
									}
								}
							}
						}
					}
					else if (startCombat)
					{
						bool flag9 = this._stealTreasuryInventory.InventoryItemTotalCount > 0;
						if (flag9)
						{
							SettlementTreasuryRecordCollection settlementTreasuryRecordCollection2 = DomainManager.Organization.GetSettlementTreasuryRecordCollection(context, settlementId);
							int currDate2 = DomainManager.World.GetCurrDate();
							if (isSect)
							{
								settlementTreasuryRecordCollection2.AddPlunderSectTreasuryFail(currDate2, settlementId, charId);
							}
							else
							{
								settlementTreasuryRecordCollection2.AddPlunderTownTreasuryFail(currDate2, settlementId, charId);
							}
							DomainManager.Organization.SetSettlementTreasuryRecordCollection(context, settlementId, settlementTreasuryRecordCollection2);
						}
					}
					this.InitializeSettlementTreasury();
				}
			}
		}

		// Token: 0x06004745 RID: 18245 RVA: 0x0027F810 File Offset: 0x0027DA10
		public sbyte CheckTreasuryLayerItemGradeRange(EventArgBox argBox)
		{
			return 1;
		}

		// Token: 0x06004746 RID: 18246 RVA: 0x0027F824 File Offset: 0x0027DA24
		[DomainMethod]
		public void GmCmd_ClearSettlementTreasuryAlertTime(DataContext context, short settlementId)
		{
			bool flag = settlementId < 0;
			if (!flag)
			{
				SettlementLayeredTreasuries treasuries = this.GetSettlement(settlementId).Treasuries;
				treasuries.AlertTime = 0;
				DomainManager.Extra.SetTreasuries(context, settlementId, treasuries, false);
			}
		}

		// Token: 0x06004747 RID: 18247 RVA: 0x0027F860 File Offset: 0x0027DA60
		[DomainMethod]
		public void GmCmd_ClearSettlementTreasuryItemAndResource(DataContext context, short settlementId)
		{
			bool flag = settlementId < 0;
			if (!flag)
			{
				SettlementLayeredTreasuries treasuries = this.GetSettlement(settlementId).Treasuries;
				foreach (SettlementTreasury settlementTreasury in treasuries.SettlementTreasuries)
				{
					if (settlementTreasury != null)
					{
						Inventory inventory = settlementTreasury.Inventory;
						if (inventory != null)
						{
							Dictionary<ItemKey, int> items = inventory.Items;
							if (items != null)
							{
								items.Clear();
							}
						}
					}
					if (settlementTreasury != null)
					{
						settlementTreasury.Resources.Initialize();
					}
				}
				DomainManager.Extra.SetTreasuries(context, settlementId, treasuries, true);
			}
		}

		// Token: 0x06004748 RID: 18248 RVA: 0x0027F8E8 File Offset: 0x0027DAE8
		[DomainMethod]
		public void GmCmd_UpdateSettlementTreasury(DataContext context, short settlementId)
		{
			Settlement settlement;
			bool flag = this._settlements.TryGetValue(settlementId, out settlement);
			if (flag)
			{
				settlement.UpdateTreasury(context);
			}
		}

		// Token: 0x06004749 RID: 18249 RVA: 0x0027F914 File Offset: 0x0027DB14
		public OrganizationDomain() : base(9)
		{
			this._sects = new Dictionary<short, Sect>(0);
			this._civilianSettlements = new Dictionary<short, CivilianSettlement>(0);
			this._nextSettlementId = 0;
			this._sectCharacters = new Dictionary<int, SectCharacter>(0);
			this._civilianSettlementCharacters = new Dictionary<int, CivilianSettlementCharacter>(0);
			this._factions = new Dictionary<int, CharacterSet>(0);
			this._largeSectFavorabilities = new sbyte[64];
			this._martialArtTournamentPreparationInfoList = new List<MartialArtTournamentPreparationInfo>();
			this._previousMartialArtTournamentHosts = new List<short>();
			this.HelperDataSects = new ObjectCollectionHelperData(3, 0, OrganizationDomain.CacheInfluencesSects, this._dataStatesSects, true);
			this.HelperDataCivilianSettlements = new ObjectCollectionHelperData(3, 1, OrganizationDomain.CacheInfluencesCivilianSettlements, this._dataStatesCivilianSettlements, true);
			this.HelperDataSectCharacters = new ObjectCollectionHelperData(3, 3, OrganizationDomain.CacheInfluencesSectCharacters, this._dataStatesSectCharacters, true);
			this.HelperDataCivilianSettlementCharacters = new ObjectCollectionHelperData(3, 4, OrganizationDomain.CacheInfluencesCivilianSettlementCharacters, this._dataStatesCivilianSettlementCharacters, true);
			this.OnInitializedDomainData();
		}

		// Token: 0x0600474A RID: 18250 RVA: 0x0027FA80 File Offset: 0x0027DC80
		public Sect GetElement_Sects(short objectId)
		{
			return this._sects[objectId];
		}

		// Token: 0x0600474B RID: 18251 RVA: 0x0027FAA0 File Offset: 0x0027DCA0
		public bool TryGetElement_Sects(short objectId, out Sect element)
		{
			return this._sects.TryGetValue(objectId, out element);
		}

		// Token: 0x0600474C RID: 18252 RVA: 0x0027FAC0 File Offset: 0x0027DCC0
		private unsafe void AddElement_Sects(short objectId, Sect instance)
		{
			instance.CollectionHelperData = this.HelperDataSects;
			instance.DataStatesOffset = this._dataStatesSects.Create();
			this._sects.Add(objectId, instance);
			byte* pData = OperationAdder.DynamicObjectCollection_Add<short>(3, 0, objectId, instance.GetSerializedSize());
			instance.Serialize(pData);
		}

		// Token: 0x0600474D RID: 18253 RVA: 0x0027FB10 File Offset: 0x0027DD10
		private void RemoveElement_Sects(short objectId)
		{
			Sect instance;
			bool flag = !this._sects.TryGetValue(objectId, out instance);
			if (!flag)
			{
				this._dataStatesSects.Remove(instance.DataStatesOffset);
				this._sects.Remove(objectId);
				OperationAdder.DynamicObjectCollection_Remove<short>(3, 0, objectId);
			}
		}

		// Token: 0x0600474E RID: 18254 RVA: 0x0027FB5D File Offset: 0x0027DD5D
		private void ClearSects()
		{
			this._dataStatesSects.Clear();
			this._sects.Clear();
			OperationAdder.DynamicObjectCollection_Clear(3, 0);
		}

		// Token: 0x0600474F RID: 18255 RVA: 0x0027FB80 File Offset: 0x0027DD80
		public int GetElementField_Sects(short objectId, ushort fieldId, RawDataPool dataPool, bool resetModified)
		{
			Sect instance;
			bool flag = !this._sects.TryGetValue(objectId, out instance);
			int result;
			if (flag)
			{
				string tag = "GetElementField_Sects";
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Failed to find element ");
				defaultInterpolatedStringHandler.AppendFormatted<short>(objectId);
				defaultInterpolatedStringHandler.AppendLiteral(" with field ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(fieldId);
				AdaptableLog.TagWarning(tag, defaultInterpolatedStringHandler.ToStringAndClear(), false);
				result = -1;
			}
			else
			{
				if (resetModified)
				{
					this._dataStatesSects.ResetModified(instance.DataStatesOffset, (int)fieldId);
				}
				switch (fieldId)
				{
				case 0:
					result = Serializer.Serialize(instance.GetId(), dataPool);
					break;
				case 1:
					result = Serializer.Serialize(instance.GetOrgTemplateId(), dataPool);
					break;
				case 2:
					result = Serializer.Serialize(instance.GetLocation(), dataPool);
					break;
				case 3:
					result = Serializer.Serialize(instance.GetCulture(), dataPool);
					break;
				case 4:
					result = Serializer.Serialize(instance.GetMaxCulture(), dataPool);
					break;
				case 5:
					result = Serializer.Serialize(instance.GetSafety(), dataPool);
					break;
				case 6:
					result = Serializer.Serialize(instance.GetMaxSafety(), dataPool);
					break;
				case 7:
					result = Serializer.Serialize(instance.GetPopulation(), dataPool);
					break;
				case 8:
					result = Serializer.Serialize(instance.GetMaxPopulation(), dataPool);
					break;
				case 9:
					result = Serializer.Serialize(instance.GetStandardOnStagePopulation(), dataPool);
					break;
				case 10:
					result = Serializer.Serialize(instance.GetMembers(), dataPool);
					break;
				case 11:
					result = Serializer.Serialize(instance.GetLackingCoreMembers(), dataPool);
					break;
				case 12:
					result = Serializer.Serialize(instance.GetApprovingRateUpperLimitBonus(), dataPool);
					break;
				case 13:
					result = Serializer.Serialize(instance.GetInfluencePowerUpdateDate(), dataPool);
					break;
				case 14:
					result = Serializer.Serialize(instance.GetMinSeniorityId(), dataPool);
					break;
				case 15:
					result = Serializer.Serialize(instance.GetAvailableMonasticTitleSuffixIds(), dataPool);
					break;
				case 16:
					result = Serializer.Serialize(instance.GetTaiwuExploreStatus(), dataPool);
					break;
				case 17:
					result = Serializer.Serialize(instance.GetSpiritualDebtInteractionOccurred(), dataPool);
					break;
				case 18:
					result = Serializer.Serialize(instance.GetTaiwuInvestmentForMartialArtTournament(), dataPool);
					break;
				case 19:
					result = Serializer.Serialize(instance.GetApprovingRateUpperLimitTempBonus(), dataPool);
					break;
				default:
				{
					bool flag2 = fieldId >= 20;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
					if (flag2)
					{
						defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(20, 1);
						defaultInterpolatedStringHandler.AppendLiteral("Unsupported fieldId ");
						defaultInterpolatedStringHandler.AppendFormatted<ushort>(fieldId);
						throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
					}
					defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(38, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Not allow to get readonly field data: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(fieldId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				}
			}
			return result;
		}

		// Token: 0x06004750 RID: 18256 RVA: 0x0027FE40 File Offset: 0x0027E040
		public void SetElementField_Sects(short objectId, ushort fieldId, int valueOffset, RawDataPool dataPool, DataContext context)
		{
			Sect instance;
			bool flag = !this._sects.TryGetValue(objectId, out instance);
			if (flag)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Failed to find element ");
				defaultInterpolatedStringHandler.AppendFormatted<short>(objectId);
				defaultInterpolatedStringHandler.AppendLiteral(" with field ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(fieldId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			switch (fieldId)
			{
			case 0:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(38, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to set readonly field data: ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(fieldId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 1:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(38, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to set readonly field data: ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(fieldId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 2:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(38, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to set readonly field data: ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(fieldId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 3:
			{
				short value = 0;
				Serializer.Deserialize(dataPool, valueOffset, ref value);
				instance.SetCulture(value, context);
				break;
			}
			case 4:
			{
				short value2 = 0;
				Serializer.Deserialize(dataPool, valueOffset, ref value2);
				instance.SetMaxCulture(value2, context);
				break;
			}
			case 5:
			{
				short value3 = 0;
				Serializer.Deserialize(dataPool, valueOffset, ref value3);
				instance.SetSafety(value3, context);
				break;
			}
			case 6:
			{
				short value4 = 0;
				Serializer.Deserialize(dataPool, valueOffset, ref value4);
				instance.SetMaxSafety(value4, context);
				break;
			}
			case 7:
			{
				int value5 = 0;
				Serializer.Deserialize(dataPool, valueOffset, ref value5);
				instance.SetPopulation(value5, context);
				break;
			}
			case 8:
			{
				int value6 = 0;
				Serializer.Deserialize(dataPool, valueOffset, ref value6);
				instance.SetMaxPopulation(value6, context);
				break;
			}
			case 9:
			{
				int value7 = 0;
				Serializer.Deserialize(dataPool, valueOffset, ref value7);
				instance.SetStandardOnStagePopulation(value7, context);
				break;
			}
			case 10:
			{
				OrgMemberCollection value8 = instance.GetMembers();
				Serializer.Deserialize(dataPool, valueOffset, ref value8);
				instance.SetMembers(value8, context);
				break;
			}
			case 11:
			{
				OrgMemberCollection value9 = instance.GetLackingCoreMembers();
				Serializer.Deserialize(dataPool, valueOffset, ref value9);
				instance.SetLackingCoreMembers(value9, context);
				break;
			}
			case 12:
			{
				short value10 = 0;
				Serializer.Deserialize(dataPool, valueOffset, ref value10);
				instance.SetApprovingRateUpperLimitBonus(value10, context);
				break;
			}
			case 13:
			{
				int value11 = 0;
				Serializer.Deserialize(dataPool, valueOffset, ref value11);
				instance.SetInfluencePowerUpdateDate(value11, context);
				break;
			}
			case 14:
			{
				short value12 = 0;
				Serializer.Deserialize(dataPool, valueOffset, ref value12);
				instance.SetMinSeniorityId(value12, context);
				break;
			}
			case 15:
			{
				List<short> value13 = instance.GetAvailableMonasticTitleSuffixIds();
				Serializer.Deserialize(dataPool, valueOffset, ref value13);
				instance.SetAvailableMonasticTitleSuffixIds(value13, context);
				break;
			}
			case 16:
			{
				byte value14 = 0;
				Serializer.Deserialize(dataPool, valueOffset, ref value14);
				instance.SetTaiwuExploreStatus(value14, context);
				break;
			}
			case 17:
			{
				bool value15 = false;
				Serializer.Deserialize(dataPool, valueOffset, ref value15);
				instance.SetSpiritualDebtInteractionOccurred(value15, context);
				break;
			}
			case 18:
			{
				int[] value16 = instance.GetTaiwuInvestmentForMartialArtTournament();
				Serializer.Deserialize(dataPool, valueOffset, ref value16);
				instance.SetTaiwuInvestmentForMartialArtTournament(value16, context);
				break;
			}
			default:
			{
				bool flag2 = fieldId >= 20;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				if (flag2)
				{
					defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(20, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported fieldId ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(fieldId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				bool flag3 = fieldId >= 20;
				if (flag3)
				{
					defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(38, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Not allow to set readonly field data: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(fieldId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to set cache field data: ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(fieldId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			}
		}

		// Token: 0x06004751 RID: 18257 RVA: 0x00280234 File Offset: 0x0027E434
		private int CheckModified_Sects(short objectId, ushort fieldId, RawDataPool dataPool)
		{
			Sect instance;
			bool flag = !this._sects.TryGetValue(objectId, out instance);
			int result;
			if (flag)
			{
				result = -1;
			}
			else
			{
				bool flag2 = fieldId >= 20;
				if (flag2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(40, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Not allow to check readonly field data: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(fieldId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				bool flag3 = !this._dataStatesSects.IsModified(instance.DataStatesOffset, (int)fieldId);
				if (flag3)
				{
					result = -1;
				}
				else
				{
					this._dataStatesSects.ResetModified(instance.DataStatesOffset, (int)fieldId);
					switch (fieldId)
					{
					case 0:
						result = Serializer.Serialize(instance.GetId(), dataPool);
						break;
					case 1:
						result = Serializer.Serialize(instance.GetOrgTemplateId(), dataPool);
						break;
					case 2:
						result = Serializer.Serialize(instance.GetLocation(), dataPool);
						break;
					case 3:
						result = Serializer.Serialize(instance.GetCulture(), dataPool);
						break;
					case 4:
						result = Serializer.Serialize(instance.GetMaxCulture(), dataPool);
						break;
					case 5:
						result = Serializer.Serialize(instance.GetSafety(), dataPool);
						break;
					case 6:
						result = Serializer.Serialize(instance.GetMaxSafety(), dataPool);
						break;
					case 7:
						result = Serializer.Serialize(instance.GetPopulation(), dataPool);
						break;
					case 8:
						result = Serializer.Serialize(instance.GetMaxPopulation(), dataPool);
						break;
					case 9:
						result = Serializer.Serialize(instance.GetStandardOnStagePopulation(), dataPool);
						break;
					case 10:
						result = Serializer.Serialize(instance.GetMembers(), dataPool);
						break;
					case 11:
						result = Serializer.Serialize(instance.GetLackingCoreMembers(), dataPool);
						break;
					case 12:
						result = Serializer.Serialize(instance.GetApprovingRateUpperLimitBonus(), dataPool);
						break;
					case 13:
						result = Serializer.Serialize(instance.GetInfluencePowerUpdateDate(), dataPool);
						break;
					case 14:
						result = Serializer.Serialize(instance.GetMinSeniorityId(), dataPool);
						break;
					case 15:
						result = Serializer.Serialize(instance.GetAvailableMonasticTitleSuffixIds(), dataPool);
						break;
					case 16:
						result = Serializer.Serialize(instance.GetTaiwuExploreStatus(), dataPool);
						break;
					case 17:
						result = Serializer.Serialize(instance.GetSpiritualDebtInteractionOccurred(), dataPool);
						break;
					case 18:
						result = Serializer.Serialize(instance.GetTaiwuInvestmentForMartialArtTournament(), dataPool);
						break;
					case 19:
						result = Serializer.Serialize(instance.GetApprovingRateUpperLimitTempBonus(), dataPool);
						break;
					default:
					{
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(20, 1);
						defaultInterpolatedStringHandler.AppendLiteral("Unsupported fieldId ");
						defaultInterpolatedStringHandler.AppendFormatted<ushort>(fieldId);
						throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
					}
					}
				}
			}
			return result;
		}

		// Token: 0x06004752 RID: 18258 RVA: 0x002804B4 File Offset: 0x0027E6B4
		private void ResetModifiedWrapper_Sects(short objectId, ushort fieldId)
		{
			Sect instance;
			bool flag = !this._sects.TryGetValue(objectId, out instance);
			if (!flag)
			{
				bool flag2 = fieldId >= 20;
				if (flag2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(62, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Not allow to reset modification state of readonly field data: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(fieldId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				bool flag3 = !this._dataStatesSects.IsModified(instance.DataStatesOffset, (int)fieldId);
				if (!flag3)
				{
					this._dataStatesSects.ResetModified(instance.DataStatesOffset, (int)fieldId);
				}
			}
		}

		// Token: 0x06004753 RID: 18259 RVA: 0x00280544 File Offset: 0x0027E744
		private bool IsModifiedWrapper_Sects(short objectId, ushort fieldId)
		{
			Sect instance;
			bool flag = !this._sects.TryGetValue(objectId, out instance);
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				bool flag2 = fieldId >= 20;
				if (flag2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(62, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Not allow to check modification state of readonly field data: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(fieldId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				result = this._dataStatesSects.IsModified(instance.DataStatesOffset, (int)fieldId);
			}
			return result;
		}

		// Token: 0x06004754 RID: 18260 RVA: 0x002805BC File Offset: 0x0027E7BC
		public CivilianSettlement GetElement_CivilianSettlements(short objectId)
		{
			return this._civilianSettlements[objectId];
		}

		// Token: 0x06004755 RID: 18261 RVA: 0x002805DC File Offset: 0x0027E7DC
		public bool TryGetElement_CivilianSettlements(short objectId, out CivilianSettlement element)
		{
			return this._civilianSettlements.TryGetValue(objectId, out element);
		}

		// Token: 0x06004756 RID: 18262 RVA: 0x002805FC File Offset: 0x0027E7FC
		private unsafe void AddElement_CivilianSettlements(short objectId, CivilianSettlement instance)
		{
			instance.CollectionHelperData = this.HelperDataCivilianSettlements;
			instance.DataStatesOffset = this._dataStatesCivilianSettlements.Create();
			this._civilianSettlements.Add(objectId, instance);
			byte* pData = OperationAdder.DynamicObjectCollection_Add<short>(3, 1, objectId, instance.GetSerializedSize());
			instance.Serialize(pData);
		}

		// Token: 0x06004757 RID: 18263 RVA: 0x0028064C File Offset: 0x0027E84C
		private void RemoveElement_CivilianSettlements(short objectId)
		{
			CivilianSettlement instance;
			bool flag = !this._civilianSettlements.TryGetValue(objectId, out instance);
			if (!flag)
			{
				this._dataStatesCivilianSettlements.Remove(instance.DataStatesOffset);
				this._civilianSettlements.Remove(objectId);
				OperationAdder.DynamicObjectCollection_Remove<short>(3, 1, objectId);
			}
		}

		// Token: 0x06004758 RID: 18264 RVA: 0x00280699 File Offset: 0x0027E899
		private void ClearCivilianSettlements()
		{
			this._dataStatesCivilianSettlements.Clear();
			this._civilianSettlements.Clear();
			OperationAdder.DynamicObjectCollection_Clear(3, 1);
		}

		// Token: 0x06004759 RID: 18265 RVA: 0x002806BC File Offset: 0x0027E8BC
		public int GetElementField_CivilianSettlements(short objectId, ushort fieldId, RawDataPool dataPool, bool resetModified)
		{
			CivilianSettlement instance;
			bool flag = !this._civilianSettlements.TryGetValue(objectId, out instance);
			int result;
			if (flag)
			{
				string tag = "GetElementField_CivilianSettlements";
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Failed to find element ");
				defaultInterpolatedStringHandler.AppendFormatted<short>(objectId);
				defaultInterpolatedStringHandler.AppendLiteral(" with field ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(fieldId);
				AdaptableLog.TagWarning(tag, defaultInterpolatedStringHandler.ToStringAndClear(), false);
				result = -1;
			}
			else
			{
				if (resetModified)
				{
					this._dataStatesCivilianSettlements.ResetModified(instance.DataStatesOffset, (int)fieldId);
				}
				switch (fieldId)
				{
				case 0:
					result = Serializer.Serialize(instance.GetId(), dataPool);
					break;
				case 1:
					result = Serializer.Serialize(instance.GetOrgTemplateId(), dataPool);
					break;
				case 2:
					result = Serializer.Serialize(instance.GetLocation(), dataPool);
					break;
				case 3:
					result = Serializer.Serialize(instance.GetCulture(), dataPool);
					break;
				case 4:
					result = Serializer.Serialize(instance.GetMaxCulture(), dataPool);
					break;
				case 5:
					result = Serializer.Serialize(instance.GetSafety(), dataPool);
					break;
				case 6:
					result = Serializer.Serialize(instance.GetMaxSafety(), dataPool);
					break;
				case 7:
					result = Serializer.Serialize(instance.GetPopulation(), dataPool);
					break;
				case 8:
					result = Serializer.Serialize(instance.GetMaxPopulation(), dataPool);
					break;
				case 9:
					result = Serializer.Serialize(instance.GetStandardOnStagePopulation(), dataPool);
					break;
				case 10:
					result = Serializer.Serialize(instance.GetMembers(), dataPool);
					break;
				case 11:
					result = Serializer.Serialize(instance.GetLackingCoreMembers(), dataPool);
					break;
				case 12:
					result = Serializer.Serialize(instance.GetApprovingRateUpperLimitBonus(), dataPool);
					break;
				case 13:
					result = Serializer.Serialize(instance.GetInfluencePowerUpdateDate(), dataPool);
					break;
				case 14:
					result = Serializer.Serialize(instance.GetRandomNameId(), dataPool);
					break;
				case 15:
					result = Serializer.Serialize(instance.GetMainMorality(), dataPool);
					break;
				case 16:
					result = Serializer.Serialize(instance.GetApprovingRateUpperLimitTempBonus(), dataPool);
					break;
				default:
				{
					bool flag2 = fieldId >= 17;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
					if (flag2)
					{
						defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(20, 1);
						defaultInterpolatedStringHandler.AppendLiteral("Unsupported fieldId ");
						defaultInterpolatedStringHandler.AppendFormatted<ushort>(fieldId);
						throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
					}
					defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(38, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Not allow to get readonly field data: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(fieldId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				}
			}
			return result;
		}

		// Token: 0x0600475A RID: 18266 RVA: 0x00280938 File Offset: 0x0027EB38
		public void SetElementField_CivilianSettlements(short objectId, ushort fieldId, int valueOffset, RawDataPool dataPool, DataContext context)
		{
			CivilianSettlement instance;
			bool flag = !this._civilianSettlements.TryGetValue(objectId, out instance);
			if (flag)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Failed to find element ");
				defaultInterpolatedStringHandler.AppendFormatted<short>(objectId);
				defaultInterpolatedStringHandler.AppendLiteral(" with field ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(fieldId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			switch (fieldId)
			{
			case 0:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(38, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to set readonly field data: ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(fieldId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 1:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(38, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to set readonly field data: ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(fieldId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 2:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(38, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to set readonly field data: ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(fieldId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 3:
			{
				short value = 0;
				Serializer.Deserialize(dataPool, valueOffset, ref value);
				instance.SetCulture(value, context);
				break;
			}
			case 4:
			{
				short value2 = 0;
				Serializer.Deserialize(dataPool, valueOffset, ref value2);
				instance.SetMaxCulture(value2, context);
				break;
			}
			case 5:
			{
				short value3 = 0;
				Serializer.Deserialize(dataPool, valueOffset, ref value3);
				instance.SetSafety(value3, context);
				break;
			}
			case 6:
			{
				short value4 = 0;
				Serializer.Deserialize(dataPool, valueOffset, ref value4);
				instance.SetMaxSafety(value4, context);
				break;
			}
			case 7:
			{
				int value5 = 0;
				Serializer.Deserialize(dataPool, valueOffset, ref value5);
				instance.SetPopulation(value5, context);
				break;
			}
			case 8:
			{
				int value6 = 0;
				Serializer.Deserialize(dataPool, valueOffset, ref value6);
				instance.SetMaxPopulation(value6, context);
				break;
			}
			case 9:
			{
				int value7 = 0;
				Serializer.Deserialize(dataPool, valueOffset, ref value7);
				instance.SetStandardOnStagePopulation(value7, context);
				break;
			}
			case 10:
			{
				OrgMemberCollection value8 = instance.GetMembers();
				Serializer.Deserialize(dataPool, valueOffset, ref value8);
				instance.SetMembers(value8, context);
				break;
			}
			case 11:
			{
				OrgMemberCollection value9 = instance.GetLackingCoreMembers();
				Serializer.Deserialize(dataPool, valueOffset, ref value9);
				instance.SetLackingCoreMembers(value9, context);
				break;
			}
			case 12:
			{
				short value10 = 0;
				Serializer.Deserialize(dataPool, valueOffset, ref value10);
				instance.SetApprovingRateUpperLimitBonus(value10, context);
				break;
			}
			case 13:
			{
				int value11 = 0;
				Serializer.Deserialize(dataPool, valueOffset, ref value11);
				instance.SetInfluencePowerUpdateDate(value11, context);
				break;
			}
			case 14:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(38, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to set readonly field data: ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(fieldId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 15:
			{
				short value12 = 0;
				Serializer.Deserialize(dataPool, valueOffset, ref value12);
				instance.SetMainMorality(value12, context);
				break;
			}
			default:
			{
				bool flag2 = fieldId >= 17;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				if (flag2)
				{
					defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(20, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported fieldId ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(fieldId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				bool flag3 = fieldId >= 17;
				if (flag3)
				{
					defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(38, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Not allow to set readonly field data: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(fieldId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to set cache field data: ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(fieldId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			}
		}

		// Token: 0x0600475B RID: 18267 RVA: 0x00280CC4 File Offset: 0x0027EEC4
		private int CheckModified_CivilianSettlements(short objectId, ushort fieldId, RawDataPool dataPool)
		{
			CivilianSettlement instance;
			bool flag = !this._civilianSettlements.TryGetValue(objectId, out instance);
			int result;
			if (flag)
			{
				result = -1;
			}
			else
			{
				bool flag2 = fieldId >= 17;
				if (flag2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(40, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Not allow to check readonly field data: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(fieldId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				bool flag3 = !this._dataStatesCivilianSettlements.IsModified(instance.DataStatesOffset, (int)fieldId);
				if (flag3)
				{
					result = -1;
				}
				else
				{
					this._dataStatesCivilianSettlements.ResetModified(instance.DataStatesOffset, (int)fieldId);
					switch (fieldId)
					{
					case 0:
						result = Serializer.Serialize(instance.GetId(), dataPool);
						break;
					case 1:
						result = Serializer.Serialize(instance.GetOrgTemplateId(), dataPool);
						break;
					case 2:
						result = Serializer.Serialize(instance.GetLocation(), dataPool);
						break;
					case 3:
						result = Serializer.Serialize(instance.GetCulture(), dataPool);
						break;
					case 4:
						result = Serializer.Serialize(instance.GetMaxCulture(), dataPool);
						break;
					case 5:
						result = Serializer.Serialize(instance.GetSafety(), dataPool);
						break;
					case 6:
						result = Serializer.Serialize(instance.GetMaxSafety(), dataPool);
						break;
					case 7:
						result = Serializer.Serialize(instance.GetPopulation(), dataPool);
						break;
					case 8:
						result = Serializer.Serialize(instance.GetMaxPopulation(), dataPool);
						break;
					case 9:
						result = Serializer.Serialize(instance.GetStandardOnStagePopulation(), dataPool);
						break;
					case 10:
						result = Serializer.Serialize(instance.GetMembers(), dataPool);
						break;
					case 11:
						result = Serializer.Serialize(instance.GetLackingCoreMembers(), dataPool);
						break;
					case 12:
						result = Serializer.Serialize(instance.GetApprovingRateUpperLimitBonus(), dataPool);
						break;
					case 13:
						result = Serializer.Serialize(instance.GetInfluencePowerUpdateDate(), dataPool);
						break;
					case 14:
						result = Serializer.Serialize(instance.GetRandomNameId(), dataPool);
						break;
					case 15:
						result = Serializer.Serialize(instance.GetMainMorality(), dataPool);
						break;
					case 16:
						result = Serializer.Serialize(instance.GetApprovingRateUpperLimitTempBonus(), dataPool);
						break;
					default:
					{
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(20, 1);
						defaultInterpolatedStringHandler.AppendLiteral("Unsupported fieldId ");
						defaultInterpolatedStringHandler.AppendFormatted<ushort>(fieldId);
						throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
					}
					}
				}
			}
			return result;
		}

		// Token: 0x0600475C RID: 18268 RVA: 0x00280F04 File Offset: 0x0027F104
		private void ResetModifiedWrapper_CivilianSettlements(short objectId, ushort fieldId)
		{
			CivilianSettlement instance;
			bool flag = !this._civilianSettlements.TryGetValue(objectId, out instance);
			if (!flag)
			{
				bool flag2 = fieldId >= 17;
				if (flag2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(62, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Not allow to reset modification state of readonly field data: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(fieldId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				bool flag3 = !this._dataStatesCivilianSettlements.IsModified(instance.DataStatesOffset, (int)fieldId);
				if (!flag3)
				{
					this._dataStatesCivilianSettlements.ResetModified(instance.DataStatesOffset, (int)fieldId);
				}
			}
		}

		// Token: 0x0600475D RID: 18269 RVA: 0x00280F94 File Offset: 0x0027F194
		private bool IsModifiedWrapper_CivilianSettlements(short objectId, ushort fieldId)
		{
			CivilianSettlement instance;
			bool flag = !this._civilianSettlements.TryGetValue(objectId, out instance);
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				bool flag2 = fieldId >= 17;
				if (flag2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(62, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Not allow to check modification state of readonly field data: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(fieldId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				result = this._dataStatesCivilianSettlements.IsModified(instance.DataStatesOffset, (int)fieldId);
			}
			return result;
		}

		// Token: 0x0600475E RID: 18270 RVA: 0x0028100C File Offset: 0x0027F20C
		private short GetNextSettlementId()
		{
			return this._nextSettlementId;
		}

		// Token: 0x0600475F RID: 18271 RVA: 0x00281024 File Offset: 0x0027F224
		private unsafe void SetNextSettlementId(short value, DataContext context)
		{
			this._nextSettlementId = value;
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(2, this.DataStates, OrganizationDomain.CacheInfluences, context);
			byte* pData = OperationAdder.FixedSingleValue_Set(3, 2, 2);
			*(short*)pData = this._nextSettlementId;
			pData += 2;
		}

		// Token: 0x06004760 RID: 18272 RVA: 0x00281064 File Offset: 0x0027F264
		public SectCharacter GetElement_SectCharacters(int objectId)
		{
			return this._sectCharacters[objectId];
		}

		// Token: 0x06004761 RID: 18273 RVA: 0x00281084 File Offset: 0x0027F284
		public bool TryGetElement_SectCharacters(int objectId, out SectCharacter element)
		{
			return this._sectCharacters.TryGetValue(objectId, out element);
		}

		// Token: 0x06004762 RID: 18274 RVA: 0x002810A4 File Offset: 0x0027F2A4
		private unsafe void AddElement_SectCharacters(int objectId, SectCharacter instance)
		{
			instance.CollectionHelperData = this.HelperDataSectCharacters;
			instance.DataStatesOffset = this._dataStatesSectCharacters.Create();
			this._sectCharacters.Add(objectId, instance);
			byte* pData = OperationAdder.FixedObjectCollection_Add<int>(3, 3, objectId, 12);
			instance.Serialize(pData);
		}

		// Token: 0x06004763 RID: 18275 RVA: 0x002810F0 File Offset: 0x0027F2F0
		private void RemoveElement_SectCharacters(int objectId)
		{
			SectCharacter instance;
			bool flag = !this._sectCharacters.TryGetValue(objectId, out instance);
			if (!flag)
			{
				this._dataStatesSectCharacters.Remove(instance.DataStatesOffset);
				this._sectCharacters.Remove(objectId);
				OperationAdder.FixedObjectCollection_Remove<int>(3, 3, objectId);
			}
		}

		// Token: 0x06004764 RID: 18276 RVA: 0x0028113D File Offset: 0x0027F33D
		private void ClearSectCharacters()
		{
			this._dataStatesSectCharacters.Clear();
			this._sectCharacters.Clear();
			OperationAdder.FixedObjectCollection_Clear(3, 3);
		}

		// Token: 0x06004765 RID: 18277 RVA: 0x00281160 File Offset: 0x0027F360
		public int GetElementField_SectCharacters(int objectId, ushort fieldId, RawDataPool dataPool, bool resetModified)
		{
			SectCharacter instance;
			bool flag = !this._sectCharacters.TryGetValue(objectId, out instance);
			int result;
			if (flag)
			{
				string tag = "GetElementField_SectCharacters";
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Failed to find element ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(objectId);
				defaultInterpolatedStringHandler.AppendLiteral(" with field ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(fieldId);
				AdaptableLog.TagWarning(tag, defaultInterpolatedStringHandler.ToStringAndClear(), false);
				result = -1;
			}
			else
			{
				if (resetModified)
				{
					this._dataStatesSectCharacters.ResetModified(instance.DataStatesOffset, (int)fieldId);
				}
				switch (fieldId)
				{
				case 0:
					result = Serializer.Serialize(instance.GetId(), dataPool);
					break;
				case 1:
					result = Serializer.Serialize(instance.GetOrgTemplateId(), dataPool);
					break;
				case 2:
					result = Serializer.Serialize(instance.GetSettlementId(), dataPool);
					break;
				case 3:
					result = Serializer.Serialize(instance.GetApprovedTaiwu(), dataPool);
					break;
				case 4:
					result = Serializer.Serialize(instance.GetInfluencePower(), dataPool);
					break;
				case 5:
					result = Serializer.Serialize(instance.GetInfluencePowerBonus(), dataPool);
					break;
				default:
				{
					bool flag2 = fieldId >= 6;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
					if (flag2)
					{
						defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(20, 1);
						defaultInterpolatedStringHandler.AppendLiteral("Unsupported fieldId ");
						defaultInterpolatedStringHandler.AppendFormatted<ushort>(fieldId);
						throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
					}
					defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(38, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Not allow to get readonly field data: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(fieldId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				}
			}
			return result;
		}

		// Token: 0x06004766 RID: 18278 RVA: 0x002812E8 File Offset: 0x0027F4E8
		public void SetElementField_SectCharacters(int objectId, ushort fieldId, int valueOffset, RawDataPool dataPool, DataContext context)
		{
			SectCharacter instance;
			bool flag = !this._sectCharacters.TryGetValue(objectId, out instance);
			if (flag)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Failed to find element ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(objectId);
				defaultInterpolatedStringHandler.AppendLiteral(" with field ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(fieldId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			switch (fieldId)
			{
			case 0:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(38, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to set readonly field data: ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(fieldId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 1:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(38, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to set readonly field data: ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(fieldId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 2:
			{
				short value = 0;
				Serializer.Deserialize(dataPool, valueOffset, ref value);
				instance.SetSettlementId(value, context);
				break;
			}
			case 3:
			{
				bool value2 = false;
				Serializer.Deserialize(dataPool, valueOffset, ref value2);
				instance.SetApprovedTaiwu(value2, context);
				break;
			}
			case 4:
			{
				short value3 = 0;
				Serializer.Deserialize(dataPool, valueOffset, ref value3);
				instance.SetInfluencePower(value3, context);
				break;
			}
			case 5:
			{
				short value4 = 0;
				Serializer.Deserialize(dataPool, valueOffset, ref value4);
				instance.SetInfluencePowerBonus(value4, context);
				break;
			}
			default:
			{
				bool flag2 = fieldId >= 6;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				if (flag2)
				{
					defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(20, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported fieldId ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(fieldId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				bool flag3 = fieldId >= 6;
				if (flag3)
				{
					defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(38, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Not allow to set readonly field data: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(fieldId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to set cache field data: ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(fieldId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			}
		}

		// Token: 0x06004767 RID: 18279 RVA: 0x002814F0 File Offset: 0x0027F6F0
		private int CheckModified_SectCharacters(int objectId, ushort fieldId, RawDataPool dataPool)
		{
			SectCharacter instance;
			bool flag = !this._sectCharacters.TryGetValue(objectId, out instance);
			int result;
			if (flag)
			{
				result = -1;
			}
			else
			{
				bool flag2 = fieldId >= 6;
				if (flag2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(40, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Not allow to check readonly field data: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(fieldId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				bool flag3 = !this._dataStatesSectCharacters.IsModified(instance.DataStatesOffset, (int)fieldId);
				if (flag3)
				{
					result = -1;
				}
				else
				{
					this._dataStatesSectCharacters.ResetModified(instance.DataStatesOffset, (int)fieldId);
					switch (fieldId)
					{
					case 0:
						result = Serializer.Serialize(instance.GetId(), dataPool);
						break;
					case 1:
						result = Serializer.Serialize(instance.GetOrgTemplateId(), dataPool);
						break;
					case 2:
						result = Serializer.Serialize(instance.GetSettlementId(), dataPool);
						break;
					case 3:
						result = Serializer.Serialize(instance.GetApprovedTaiwu(), dataPool);
						break;
					case 4:
						result = Serializer.Serialize(instance.GetInfluencePower(), dataPool);
						break;
					case 5:
						result = Serializer.Serialize(instance.GetInfluencePowerBonus(), dataPool);
						break;
					default:
					{
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(20, 1);
						defaultInterpolatedStringHandler.AppendLiteral("Unsupported fieldId ");
						defaultInterpolatedStringHandler.AppendFormatted<ushort>(fieldId);
						throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
					}
					}
				}
			}
			return result;
		}

		// Token: 0x06004768 RID: 18280 RVA: 0x00281638 File Offset: 0x0027F838
		private void ResetModifiedWrapper_SectCharacters(int objectId, ushort fieldId)
		{
			SectCharacter instance;
			bool flag = !this._sectCharacters.TryGetValue(objectId, out instance);
			if (!flag)
			{
				bool flag2 = fieldId >= 6;
				if (flag2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(62, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Not allow to reset modification state of readonly field data: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(fieldId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				bool flag3 = !this._dataStatesSectCharacters.IsModified(instance.DataStatesOffset, (int)fieldId);
				if (!flag3)
				{
					this._dataStatesSectCharacters.ResetModified(instance.DataStatesOffset, (int)fieldId);
				}
			}
		}

		// Token: 0x06004769 RID: 18281 RVA: 0x002816C8 File Offset: 0x0027F8C8
		private bool IsModifiedWrapper_SectCharacters(int objectId, ushort fieldId)
		{
			SectCharacter instance;
			bool flag = !this._sectCharacters.TryGetValue(objectId, out instance);
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				bool flag2 = fieldId >= 6;
				if (flag2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(62, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Not allow to check modification state of readonly field data: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(fieldId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				result = this._dataStatesSectCharacters.IsModified(instance.DataStatesOffset, (int)fieldId);
			}
			return result;
		}

		// Token: 0x0600476A RID: 18282 RVA: 0x00281740 File Offset: 0x0027F940
		public CivilianSettlementCharacter GetElement_CivilianSettlementCharacters(int objectId)
		{
			return this._civilianSettlementCharacters[objectId];
		}

		// Token: 0x0600476B RID: 18283 RVA: 0x00281760 File Offset: 0x0027F960
		public bool TryGetElement_CivilianSettlementCharacters(int objectId, out CivilianSettlementCharacter element)
		{
			return this._civilianSettlementCharacters.TryGetValue(objectId, out element);
		}

		// Token: 0x0600476C RID: 18284 RVA: 0x00281780 File Offset: 0x0027F980
		private unsafe void AddElement_CivilianSettlementCharacters(int objectId, CivilianSettlementCharacter instance)
		{
			instance.CollectionHelperData = this.HelperDataCivilianSettlementCharacters;
			instance.DataStatesOffset = this._dataStatesCivilianSettlementCharacters.Create();
			this._civilianSettlementCharacters.Add(objectId, instance);
			byte* pData = OperationAdder.FixedObjectCollection_Add<int>(3, 4, objectId, 12);
			instance.Serialize(pData);
		}

		// Token: 0x0600476D RID: 18285 RVA: 0x002817CC File Offset: 0x0027F9CC
		private void RemoveElement_CivilianSettlementCharacters(int objectId)
		{
			CivilianSettlementCharacter instance;
			bool flag = !this._civilianSettlementCharacters.TryGetValue(objectId, out instance);
			if (!flag)
			{
				this._dataStatesCivilianSettlementCharacters.Remove(instance.DataStatesOffset);
				this._civilianSettlementCharacters.Remove(objectId);
				OperationAdder.FixedObjectCollection_Remove<int>(3, 4, objectId);
			}
		}

		// Token: 0x0600476E RID: 18286 RVA: 0x00281819 File Offset: 0x0027FA19
		private void ClearCivilianSettlementCharacters()
		{
			this._dataStatesCivilianSettlementCharacters.Clear();
			this._civilianSettlementCharacters.Clear();
			OperationAdder.FixedObjectCollection_Clear(3, 4);
		}

		// Token: 0x0600476F RID: 18287 RVA: 0x0028183C File Offset: 0x0027FA3C
		public int GetElementField_CivilianSettlementCharacters(int objectId, ushort fieldId, RawDataPool dataPool, bool resetModified)
		{
			CivilianSettlementCharacter instance;
			bool flag = !this._civilianSettlementCharacters.TryGetValue(objectId, out instance);
			int result;
			if (flag)
			{
				string tag = "GetElementField_CivilianSettlementCharacters";
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Failed to find element ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(objectId);
				defaultInterpolatedStringHandler.AppendLiteral(" with field ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(fieldId);
				AdaptableLog.TagWarning(tag, defaultInterpolatedStringHandler.ToStringAndClear(), false);
				result = -1;
			}
			else
			{
				if (resetModified)
				{
					this._dataStatesCivilianSettlementCharacters.ResetModified(instance.DataStatesOffset, (int)fieldId);
				}
				switch (fieldId)
				{
				case 0:
					result = Serializer.Serialize(instance.GetId(), dataPool);
					break;
				case 1:
					result = Serializer.Serialize(instance.GetOrgTemplateId(), dataPool);
					break;
				case 2:
					result = Serializer.Serialize(instance.GetSettlementId(), dataPool);
					break;
				case 3:
					result = Serializer.Serialize(instance.GetApprovedTaiwu(), dataPool);
					break;
				case 4:
					result = Serializer.Serialize(instance.GetInfluencePower(), dataPool);
					break;
				case 5:
					result = Serializer.Serialize(instance.GetInfluencePowerBonus(), dataPool);
					break;
				default:
				{
					bool flag2 = fieldId >= 6;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
					if (flag2)
					{
						defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(20, 1);
						defaultInterpolatedStringHandler.AppendLiteral("Unsupported fieldId ");
						defaultInterpolatedStringHandler.AppendFormatted<ushort>(fieldId);
						throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
					}
					defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(38, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Not allow to get readonly field data: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(fieldId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				}
			}
			return result;
		}

		// Token: 0x06004770 RID: 18288 RVA: 0x002819C4 File Offset: 0x0027FBC4
		public void SetElementField_CivilianSettlementCharacters(int objectId, ushort fieldId, int valueOffset, RawDataPool dataPool, DataContext context)
		{
			CivilianSettlementCharacter instance;
			bool flag = !this._civilianSettlementCharacters.TryGetValue(objectId, out instance);
			if (flag)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Failed to find element ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(objectId);
				defaultInterpolatedStringHandler.AppendLiteral(" with field ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(fieldId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			switch (fieldId)
			{
			case 0:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(38, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to set readonly field data: ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(fieldId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 1:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(38, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to set readonly field data: ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(fieldId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 2:
			{
				short value = 0;
				Serializer.Deserialize(dataPool, valueOffset, ref value);
				instance.SetSettlementId(value, context);
				break;
			}
			case 3:
			{
				bool value2 = false;
				Serializer.Deserialize(dataPool, valueOffset, ref value2);
				instance.SetApprovedTaiwu(value2, context);
				break;
			}
			case 4:
			{
				short value3 = 0;
				Serializer.Deserialize(dataPool, valueOffset, ref value3);
				instance.SetInfluencePower(value3, context);
				break;
			}
			case 5:
			{
				short value4 = 0;
				Serializer.Deserialize(dataPool, valueOffset, ref value4);
				instance.SetInfluencePowerBonus(value4, context);
				break;
			}
			default:
			{
				bool flag2 = fieldId >= 6;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				if (flag2)
				{
					defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(20, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported fieldId ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(fieldId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				bool flag3 = fieldId >= 6;
				if (flag3)
				{
					defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(38, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Not allow to set readonly field data: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(fieldId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to set cache field data: ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(fieldId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			}
		}

		// Token: 0x06004771 RID: 18289 RVA: 0x00281BCC File Offset: 0x0027FDCC
		private int CheckModified_CivilianSettlementCharacters(int objectId, ushort fieldId, RawDataPool dataPool)
		{
			CivilianSettlementCharacter instance;
			bool flag = !this._civilianSettlementCharacters.TryGetValue(objectId, out instance);
			int result;
			if (flag)
			{
				result = -1;
			}
			else
			{
				bool flag2 = fieldId >= 6;
				if (flag2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(40, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Not allow to check readonly field data: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(fieldId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				bool flag3 = !this._dataStatesCivilianSettlementCharacters.IsModified(instance.DataStatesOffset, (int)fieldId);
				if (flag3)
				{
					result = -1;
				}
				else
				{
					this._dataStatesCivilianSettlementCharacters.ResetModified(instance.DataStatesOffset, (int)fieldId);
					switch (fieldId)
					{
					case 0:
						result = Serializer.Serialize(instance.GetId(), dataPool);
						break;
					case 1:
						result = Serializer.Serialize(instance.GetOrgTemplateId(), dataPool);
						break;
					case 2:
						result = Serializer.Serialize(instance.GetSettlementId(), dataPool);
						break;
					case 3:
						result = Serializer.Serialize(instance.GetApprovedTaiwu(), dataPool);
						break;
					case 4:
						result = Serializer.Serialize(instance.GetInfluencePower(), dataPool);
						break;
					case 5:
						result = Serializer.Serialize(instance.GetInfluencePowerBonus(), dataPool);
						break;
					default:
					{
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(20, 1);
						defaultInterpolatedStringHandler.AppendLiteral("Unsupported fieldId ");
						defaultInterpolatedStringHandler.AppendFormatted<ushort>(fieldId);
						throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
					}
					}
				}
			}
			return result;
		}

		// Token: 0x06004772 RID: 18290 RVA: 0x00281D14 File Offset: 0x0027FF14
		private void ResetModifiedWrapper_CivilianSettlementCharacters(int objectId, ushort fieldId)
		{
			CivilianSettlementCharacter instance;
			bool flag = !this._civilianSettlementCharacters.TryGetValue(objectId, out instance);
			if (!flag)
			{
				bool flag2 = fieldId >= 6;
				if (flag2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(62, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Not allow to reset modification state of readonly field data: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(fieldId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				bool flag3 = !this._dataStatesCivilianSettlementCharacters.IsModified(instance.DataStatesOffset, (int)fieldId);
				if (!flag3)
				{
					this._dataStatesCivilianSettlementCharacters.ResetModified(instance.DataStatesOffset, (int)fieldId);
				}
			}
		}

		// Token: 0x06004773 RID: 18291 RVA: 0x00281DA4 File Offset: 0x0027FFA4
		private bool IsModifiedWrapper_CivilianSettlementCharacters(int objectId, ushort fieldId)
		{
			CivilianSettlementCharacter instance;
			bool flag = !this._civilianSettlementCharacters.TryGetValue(objectId, out instance);
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				bool flag2 = fieldId >= 6;
				if (flag2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(62, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Not allow to check modification state of readonly field data: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(fieldId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				result = this._dataStatesCivilianSettlementCharacters.IsModified(instance.DataStatesOffset, (int)fieldId);
			}
			return result;
		}

		// Token: 0x06004774 RID: 18292 RVA: 0x00281E1C File Offset: 0x0028001C
		public CharacterSet GetElement_Factions(int elementId)
		{
			return this._factions[elementId];
		}

		// Token: 0x06004775 RID: 18293 RVA: 0x00281E3C File Offset: 0x0028003C
		public bool TryGetElement_Factions(int elementId, out CharacterSet value)
		{
			return this._factions.TryGetValue(elementId, out value);
		}

		// Token: 0x06004776 RID: 18294 RVA: 0x00281E5C File Offset: 0x0028005C
		private unsafe void AddElement_Factions(int elementId, CharacterSet value, DataContext context)
		{
			this._factions.Add(elementId, value);
			this._modificationsFactions.RecordAdding(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(5, this.DataStates, OrganizationDomain.CacheInfluences, context);
			int dataSize = value.GetSerializedSize();
			byte* pData = OperationAdder.DynamicSingleValueCollection_Add<int>(3, 5, elementId, dataSize);
			pData += value.Serialize(pData);
		}

		// Token: 0x06004777 RID: 18295 RVA: 0x00281EB8 File Offset: 0x002800B8
		private unsafe void SetElement_Factions(int elementId, CharacterSet value, DataContext context)
		{
			this._factions[elementId] = value;
			this._modificationsFactions.RecordSetting(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(5, this.DataStates, OrganizationDomain.CacheInfluences, context);
			int dataSize = value.GetSerializedSize();
			byte* pData = OperationAdder.DynamicSingleValueCollection_Set<int>(3, 5, elementId, dataSize);
			pData += value.Serialize(pData);
		}

		// Token: 0x06004778 RID: 18296 RVA: 0x00281F11 File Offset: 0x00280111
		private void RemoveElement_Factions(int elementId, DataContext context)
		{
			this._factions.Remove(elementId);
			this._modificationsFactions.RecordRemoving(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(5, this.DataStates, OrganizationDomain.CacheInfluences, context);
			OperationAdder.DynamicSingleValueCollection_Remove<int>(3, 5, elementId);
		}

		// Token: 0x06004779 RID: 18297 RVA: 0x00281F4A File Offset: 0x0028014A
		private void ClearFactions(DataContext context)
		{
			this._factions.Clear();
			this._modificationsFactions.RecordClearing();
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(5, this.DataStates, OrganizationDomain.CacheInfluences, context);
			OperationAdder.DynamicSingleValueCollection_Clear(3, 5);
		}

		// Token: 0x0600477A RID: 18298 RVA: 0x00281F80 File Offset: 0x00280180
		private sbyte[] GetLargeSectFavorabilities()
		{
			return this._largeSectFavorabilities;
		}

		// Token: 0x0600477B RID: 18299 RVA: 0x00281F98 File Offset: 0x00280198
		private unsafe void SetLargeSectFavorabilities(sbyte[] value, DataContext context)
		{
			this._largeSectFavorabilities = value;
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(6, this.DataStates, OrganizationDomain.CacheInfluences, context);
			byte* pData = OperationAdder.FixedSingleValue_Set(3, 6, 64);
			for (int i = 0; i < 64; i++)
			{
				pData[i] = (byte)this._largeSectFavorabilities[i];
			}
			pData += 64;
		}

		// Token: 0x0600477C RID: 18300 RVA: 0x00281FEC File Offset: 0x002801EC
		public List<MartialArtTournamentPreparationInfo> GetMartialArtTournamentPreparationInfoList()
		{
			Thread.MemoryBarrier();
			bool flag = BaseGameDataDomain.IsCached(this.DataStates, 7);
			List<MartialArtTournamentPreparationInfo> martialArtTournamentPreparationInfoList;
			if (flag)
			{
				martialArtTournamentPreparationInfoList = this._martialArtTournamentPreparationInfoList;
			}
			else
			{
				List<MartialArtTournamentPreparationInfo> value = new List<MartialArtTournamentPreparationInfo>();
				this.CalcMartialArtTournamentPreparationInfoList(value);
				bool lockTaken = false;
				try
				{
					this._spinLockMartialArtTournamentPreparationInfoList.Enter(ref lockTaken);
					this._martialArtTournamentPreparationInfoList.Assign(value);
					BaseGameDataDomain.SetCached(this.DataStates, 7);
				}
				finally
				{
					bool flag2 = lockTaken;
					if (flag2)
					{
						this._spinLockMartialArtTournamentPreparationInfoList.Exit(false);
					}
				}
				Thread.MemoryBarrier();
				martialArtTournamentPreparationInfoList = this._martialArtTournamentPreparationInfoList;
			}
			return martialArtTournamentPreparationInfoList;
		}

		// Token: 0x0600477D RID: 18301 RVA: 0x0028208C File Offset: 0x0028028C
		public List<short> GetPreviousMartialArtTournamentHosts()
		{
			return this._previousMartialArtTournamentHosts;
		}

		// Token: 0x0600477E RID: 18302 RVA: 0x002820A4 File Offset: 0x002802A4
		public unsafe void SetPreviousMartialArtTournamentHosts(List<short> value, DataContext context)
		{
			this._previousMartialArtTournamentHosts = value;
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(8, this.DataStates, OrganizationDomain.CacheInfluences, context);
			int elementsCount = this._previousMartialArtTournamentHosts.Count;
			int contentSize = 2 * elementsCount;
			int dataSize = 2 + contentSize;
			byte* pData = OperationAdder.DynamicSingleValue_Set(3, 8, dataSize);
			*(short*)pData = (short)((ushort)elementsCount);
			pData += 2;
			for (int i = 0; i < elementsCount; i++)
			{
				*(short*)(pData + (IntPtr)i * 2) = this._previousMartialArtTournamentHosts[i];
			}
			pData += contentSize;
		}

		// Token: 0x0600477F RID: 18303 RVA: 0x00282120 File Offset: 0x00280320
		public override void OnInitializeGameDataModule()
		{
			this.InitializeOnInitializeGameDataModule();
		}

		// Token: 0x06004780 RID: 18304 RVA: 0x0028212C File Offset: 0x0028032C
		public unsafe override void OnEnterNewWorld()
		{
			this.InitializeOnEnterNewWorld();
			this.InitializeInternalDataOfCollections();
			foreach (KeyValuePair<short, Sect> entry in this._sects)
			{
				short objectId = entry.Key;
				Sect instance = entry.Value;
				byte* pData = OperationAdder.DynamicObjectCollection_Add<short>(3, 0, objectId, instance.GetSerializedSize());
				instance.Serialize(pData);
			}
			foreach (KeyValuePair<short, CivilianSettlement> entry2 in this._civilianSettlements)
			{
				short objectId2 = entry2.Key;
				CivilianSettlement instance2 = entry2.Value;
				byte* pData2 = OperationAdder.DynamicObjectCollection_Add<short>(3, 1, objectId2, instance2.GetSerializedSize());
				instance2.Serialize(pData2);
			}
			byte* pData3 = OperationAdder.FixedSingleValue_Set(3, 2, 2);
			*(short*)pData3 = this._nextSettlementId;
			pData3 += 2;
			foreach (KeyValuePair<int, SectCharacter> entry3 in this._sectCharacters)
			{
				int objectId3 = entry3.Key;
				SectCharacter instance3 = entry3.Value;
				byte* pData4 = OperationAdder.FixedObjectCollection_Add<int>(3, 3, objectId3, 12);
				instance3.Serialize(pData4);
			}
			foreach (KeyValuePair<int, CivilianSettlementCharacter> entry4 in this._civilianSettlementCharacters)
			{
				int objectId4 = entry4.Key;
				CivilianSettlementCharacter instance4 = entry4.Value;
				byte* pData5 = OperationAdder.FixedObjectCollection_Add<int>(3, 4, objectId4, 12);
				instance4.Serialize(pData5);
			}
			foreach (KeyValuePair<int, CharacterSet> entry5 in this._factions)
			{
				int elementId = entry5.Key;
				CharacterSet value = entry5.Value;
				int dataSize = value.GetSerializedSize();
				byte* pData6 = OperationAdder.DynamicSingleValueCollection_Add<int>(3, 5, elementId, dataSize);
				pData6 += value.Serialize(pData6);
			}
			byte* pData7 = OperationAdder.FixedSingleValue_Set(3, 6, 64);
			for (int i = 0; i < 64; i++)
			{
				pData7[i] = (byte)this._largeSectFavorabilities[i];
			}
			pData7 += 64;
			int elementsCount = this._previousMartialArtTournamentHosts.Count;
			int contentSize = 2 * elementsCount;
			int dataSize2 = 2 + contentSize;
			byte* pData8 = OperationAdder.DynamicSingleValue_Set(3, 8, dataSize2);
			*(short*)pData8 = (short)((ushort)elementsCount);
			pData8 += 2;
			for (int j = 0; j < elementsCount; j++)
			{
				*(short*)(pData8 + (IntPtr)j * 2) = this._previousMartialArtTournamentHosts[j];
			}
			pData8 += contentSize;
		}

		// Token: 0x06004781 RID: 18305 RVA: 0x0028242C File Offset: 0x0028062C
		public override void OnLoadWorld()
		{
			this._pendingLoadingOperationIds = new Queue<uint>();
			this._pendingLoadingOperationIds.Enqueue(OperationAdder.DynamicObjectCollection_GetAllObjects(3, 0));
			this._pendingLoadingOperationIds.Enqueue(OperationAdder.DynamicObjectCollection_GetAllObjects(3, 1));
			this._pendingLoadingOperationIds.Enqueue(OperationAdder.FixedSingleValue_Get(3, 2));
			this._pendingLoadingOperationIds.Enqueue(OperationAdder.FixedObjectCollection_GetAllObjects(3, 3));
			this._pendingLoadingOperationIds.Enqueue(OperationAdder.FixedObjectCollection_GetAllObjects(3, 4));
			this._pendingLoadingOperationIds.Enqueue(OperationAdder.DynamicSingleValueCollection_GetAll(3, 5));
			this._pendingLoadingOperationIds.Enqueue(OperationAdder.FixedSingleValue_Get(3, 6));
			this._pendingLoadingOperationIds.Enqueue(OperationAdder.DynamicSingleValue_Get(3, 8));
		}

		// Token: 0x06004782 RID: 18306 RVA: 0x002824E0 File Offset: 0x002806E0
		public override int GetData(ushort dataId, ulong subId0, uint subId1, RawDataPool dataPool, bool resetModified)
		{
			int result;
			switch (dataId)
			{
			case 0:
				result = this.GetElementField_Sects((short)subId0, (ushort)subId1, dataPool, resetModified);
				break;
			case 1:
				result = this.GetElementField_CivilianSettlements((short)subId0, (ushort)subId1, dataPool, resetModified);
				break;
			case 2:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(34, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to get value of dataId: ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 3:
				result = this.GetElementField_SectCharacters((int)subId0, (ushort)subId1, dataPool, resetModified);
				break;
			case 4:
				result = this.GetElementField_CivilianSettlementCharacters((int)subId0, (ushort)subId1, dataPool, resetModified);
				break;
			case 5:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 5);
					this._modificationsFactions.Reset();
				}
				result = Serializer.SerializeModifications<int>(this._factions, dataPool);
				break;
			case 6:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(34, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to get value of dataId: ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 7:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 7);
				}
				result = Serializer.Serialize(this.GetMartialArtTournamentPreparationInfoList(), dataPool);
				break;
			case 8:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 8);
				}
				result = Serializer.Serialize(this._previousMartialArtTournamentHosts, dataPool);
				break;
			default:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(19, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Unsupported dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			}
			return result;
		}

		// Token: 0x06004783 RID: 18307 RVA: 0x0028267C File Offset: 0x0028087C
		public override void SetData(ushort dataId, ulong subId0, uint subId1, int valueOffset, RawDataPool dataPool, DataContext context)
		{
			switch (dataId)
			{
			case 0:
				this.SetElementField_Sects((short)subId0, (ushort)subId1, valueOffset, dataPool, context);
				break;
			case 1:
				this.SetElementField_CivilianSettlements((short)subId0, (ushort)subId1, valueOffset, dataPool, context);
				break;
			case 2:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(33, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to set value of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 3:
				this.SetElementField_SectCharacters((int)subId0, (ushort)subId1, valueOffset, dataPool, context);
				break;
			case 4:
				this.SetElementField_CivilianSettlementCharacters((int)subId0, (ushort)subId1, valueOffset, dataPool, context);
				break;
			case 5:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(33, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to set value of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 6:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(33, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to set value of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 7:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(33, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to set value of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 8:
				Serializer.Deserialize(dataPool, valueOffset, ref this._previousMartialArtTournamentHosts);
				this.SetPreviousMartialArtTournamentHosts(this._previousMartialArtTournamentHosts, context);
				break;
			default:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(19, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Unsupported dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			}
		}

		// Token: 0x06004784 RID: 18308 RVA: 0x00282818 File Offset: 0x00280A18
		public override int CallMethod(Operation operation, RawDataPool argDataPool, RawDataPool returnDataPool, DataContext context)
		{
			int argsOffset = operation.ArgsOffset;
			int result;
			switch (operation.MethodId)
			{
			case 0:
			{
				int argsCount = operation.ArgsCount;
				int num = argsCount;
				if (num != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				short settlementId = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref settlementId);
				SettlementDisplayData returnValue = this.GetDisplayData(settlementId);
				result = Serializer.Serialize(returnValue, returnDataPool);
				break;
			}
			case 1:
			{
				int argsCount2 = operation.ArgsCount;
				int num2 = argsCount2;
				if (num2 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				List<short> settlementIds = null;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref settlementIds);
				List<SettlementNameRelatedData> returnValue2 = this.GetSettlementNameRelatedData(settlementIds);
				result = Serializer.Serialize(returnValue2, returnDataPool);
				break;
			}
			case 2:
			{
				int argsCount3 = operation.ArgsCount;
				int num3 = argsCount3;
				if (num3 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				short settlementId2 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref settlementId2);
				List<CharacterDisplayData> returnValue3 = this.GetSettlementMembers(settlementId2);
				result = Serializer.Serialize(returnValue3, returnDataPool);
				break;
			}
			case 3:
			{
				int argsCount4 = operation.ArgsCount;
				int num4 = argsCount4;
				if (num4 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				sbyte organizationTemplateId = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref organizationTemplateId);
				OrganizationCombatSkillsDisplayData returnValue4 = this.GetOrganizationCombatSkillsDisplayData(organizationTemplateId);
				result = Serializer.Serialize(returnValue4, returnDataPool);
				break;
			}
			case 4:
			{
				int argsCount5 = operation.ArgsCount;
				int num5 = argsCount5;
				if (num5 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				sbyte orgTemplateId = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref orgTemplateId);
				int[] returnValue5 = this.GetSectPreparationForMartialArtTournament(orgTemplateId);
				result = Serializer.Serialize(returnValue5, returnDataPool);
				break;
			}
			case 5:
			{
				int argsCount6 = operation.ArgsCount;
				int num6 = argsCount6;
				if (num6 != 0)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				short returnValue6 = this.GetMartialArtTournamentCurrentHostSettlementId();
				result = Serializer.Serialize(returnValue6, returnDataPool);
				break;
			}
			case 6:
			{
				int argsCount7 = operation.ArgsCount;
				int num7 = argsCount7;
				if (num7 != 0)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				this.GmCmd_SetAllSettlementInformationVisited(context);
				result = -1;
				break;
			}
			case 7:
			{
				int argsCount8 = operation.ArgsCount;
				int num8 = argsCount8;
				if (num8 != 0)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				List<List<CharacterDisplayData>> returnValue7 = this.GmCmd_GetAllFactionMembers();
				result = Serializer.Serialize(returnValue7, returnDataPool);
				break;
			}
			case 8:
			{
				int argsCount9 = operation.ArgsCount;
				int num9 = argsCount9;
				if (num9 != 2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				short areaId = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref areaId);
				short blockId = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref blockId);
				short returnValue8 = this.GetSettlementIdByAreaIdAndBlockId(areaId, blockId);
				result = Serializer.Serialize(returnValue8, returnDataPool);
				break;
			}
			case 9:
			{
				int argsCount10 = operation.ArgsCount;
				int num10 = argsCount10;
				if (num10 != 2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				short areaId2 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref areaId2);
				short blockId2 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref blockId2);
				ShortPair returnValue9 = this.GetCultureByAreaIdAndBlockId(areaId2, blockId2);
				result = Serializer.Serialize(returnValue9, returnDataPool);
				break;
			}
			case 10:
			{
				int argsCount11 = operation.ArgsCount;
				int num11 = argsCount11;
				if (num11 != 0)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				int returnValue10 = this.CalcApprovingRateEffectAuthorityGain();
				result = Serializer.Serialize(returnValue10, returnDataPool);
				break;
			}
			case 11:
			{
				int argsCount12 = operation.ArgsCount;
				int num12 = argsCount12;
				if (num12 != 2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				short settlementId3 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref settlementId3);
				sbyte layerIndex = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref layerIndex);
				SettlementTreasuryDisplayData returnValue11 = this.GetSettlementTreasuryDisplayData(context, settlementId3, layerIndex);
				result = Serializer.Serialize(returnValue11, returnDataPool);
				break;
			}
			case 12:
			{
				int argsCount13 = operation.ArgsCount;
				int num13 = argsCount13;
				if (num13 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				short settlementId4 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref settlementId4);
				SettlementTreasuryRecordCollection returnValue12 = this.GetSettlementTreasuryRecordCollection(context, settlementId4);
				result = Serializer.Serialize(returnValue12, returnDataPool);
				break;
			}
			case 13:
			{
				int argsCount14 = operation.ArgsCount;
				int num14 = argsCount14;
				if (num14 != 6)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				int needAuthority = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref needAuthority);
				List<ItemSourceChange> itemSourceChanges = null;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref itemSourceChanges);
				Inventory getTreasuryInventory = null;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref getTreasuryInventory);
				Inventory stealTreasuryInventory = null;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref stealTreasuryInventory);
				Inventory exchangeTreasuryInventory = null;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref exchangeTreasuryInventory);
				Inventory storeTreasuryInventory = null;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref storeTreasuryInventory);
				this.ConfirmSettlementTreasuryOperation(context, needAuthority, itemSourceChanges, getTreasuryInventory, stealTreasuryInventory, exchangeTreasuryInventory, storeTreasuryInventory);
				result = -1;
				break;
			}
			case 14:
			{
				int argsCount15 = operation.ArgsCount;
				int num15 = argsCount15;
				if (num15 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				List<InscribedCharacterKey> inscribedCharList = null;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref inscribedCharList);
				this.SetInscribedCharactersForCreation(context, inscribedCharList);
				result = -1;
				break;
			}
			case 15:
			{
				int argsCount16 = operation.ArgsCount;
				int num16 = argsCount16;
				if (num16 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				short settlementId5 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref settlementId5);
				this.GmCmd_UpdateSettlementTreasury(context, settlementId5);
				result = -1;
				break;
			}
			case 16:
			{
				int argsCount17 = operation.ArgsCount;
				int num17 = argsCount17;
				if (num17 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				short settlementId6 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref settlementId6);
				this.GmCmd_ClearSettlementTreasuryAlertTime(context, settlementId6);
				result = -1;
				break;
			}
			case 17:
			{
				int argsCount18 = operation.ArgsCount;
				int num18 = argsCount18;
				if (num18 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				short settlementId7 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref settlementId7);
				this.GmCmd_ClearSettlementTreasuryItemAndResource(context, settlementId7);
				result = -1;
				break;
			}
			case 18:
			{
				int argsCount19 = operation.ArgsCount;
				int num19 = argsCount19;
				if (num19 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				short settlementId8 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref settlementId8);
				this.GmCmd_ForceUpdateTreasuryGuards(context, settlementId8);
				result = -1;
				break;
			}
			case 19:
			{
				int argsCount20 = operation.ArgsCount;
				int num20 = argsCount20;
				if (num20 != 5)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				sbyte orgTemplateId2 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref orgTemplateId2);
				int charId = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref charId);
				sbyte punishmentSeverity = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref punishmentSeverity);
				short punishmentType = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref punishmentType);
				int duration = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref duration);
				this.AddSectBounty(context, orgTemplateId2, charId, punishmentSeverity, punishmentType, duration);
				result = -1;
				break;
			}
			case 20:
			{
				int argsCount21 = operation.ArgsCount;
				int num21 = argsCount21;
				if (num21 != 5)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				sbyte orgTemplateId3 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref orgTemplateId3);
				int charId2 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref charId2);
				sbyte punishmentSeverity2 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref punishmentSeverity2);
				short punishmentType2 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref punishmentType2);
				int duration2 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref duration2);
				this.AddSectPrisoner(context, orgTemplateId3, charId2, punishmentSeverity2, punishmentType2, duration2);
				result = -1;
				break;
			}
			case 21:
			{
				int argsCount22 = operation.ArgsCount;
				int num22 = argsCount22;
				if (num22 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				short settlementId9 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref settlementId9);
				SettlementPrisonDisplayData returnValue13 = this.GetSettlementPrisonDisplayData(context, settlementId9);
				result = Serializer.Serialize(returnValue13, returnDataPool);
				break;
			}
			case 22:
			{
				int argsCount23 = operation.ArgsCount;
				int num23 = argsCount23;
				if (num23 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				short settlementId10 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref settlementId10);
				SettlementBountyDisplayData returnValue14 = this.GetSettlementBountyDisplayData(settlementId10);
				result = Serializer.Serialize(returnValue14, returnDataPool);
				break;
			}
			case 23:
			{
				int argsCount24 = operation.ArgsCount;
				int num24 = argsCount24;
				if (num24 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				short settlementId11 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref settlementId11);
				SettlementPrisonRecordCollection returnValue15 = this.GetSettlementPrisonRecordCollection(context, settlementId11);
				result = Serializer.Serialize(returnValue15, returnDataPool);
				break;
			}
			case 24:
			{
				int argsCount25 = operation.ArgsCount;
				int num25 = argsCount25;
				if (num25 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				short settlementId12 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref settlementId12);
				this.GmCmd_ForceUpdateInfluencePower(context, settlementId12);
				result = -1;
				break;
			}
			case 25:
			{
				int argsCount26 = operation.ArgsCount;
				int num26 = argsCount26;
				if (num26 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				List<int> characterIds = null;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref characterIds);
				SettlementBountyDisplayData returnValue16 = this.GetBountyCharacterDisplayDataFromCharacterList(characterIds);
				result = Serializer.Serialize(returnValue16, returnDataPool);
				break;
			}
			case 26:
			{
				int argsCount27 = operation.ArgsCount;
				int num27 = argsCount27;
				if (num27 != 0)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				this.ForceUpdateTaiwuVillager(context);
				result = -1;
				break;
			}
			case 27:
			{
				int argsCount28 = operation.ArgsCount;
				int num28 = argsCount28;
				if (num28 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				sbyte orgTemplateId4 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref orgTemplateId4);
				bool returnValue17 = this.IsTaiwuSectFugitive(orgTemplateId4);
				result = Serializer.Serialize(returnValue17, returnDataPool);
				break;
			}
			case 28:
			{
				int argsCount29 = operation.ArgsCount;
				int num29 = argsCount29;
				if (num29 != 0)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				short returnValue18 = this.GetOrganizationTemplateIdOfTaiwuLocation();
				result = Serializer.Serialize(returnValue18, returnDataPool);
				break;
			}
			case 29:
			{
				int argsCount30 = operation.ArgsCount;
				int num30 = argsCount30;
				if (num30 != 0)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				List<ItemSourceChange> returnValue19 = this.GetLastSettlementTreasuryOperationData();
				result = Serializer.Serialize(returnValue19, returnDataPool);
				break;
			}
			case 30:
			{
				int argsCount31 = operation.ArgsCount;
				int num31 = argsCount31;
				if (num31 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				int prisonType = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref prisonType);
				List<CharacterDisplayData> returnValue20 = this.GmCmd_GetSettlementPrisoner(context, prisonType);
				result = Serializer.Serialize(returnValue20, returnDataPool);
				break;
			}
			case 31:
			{
				int argsCount32 = operation.ArgsCount;
				int num32 = argsCount32;
				if (num32 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				short settlementId13 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref settlementId13);
				bool[] returnValue21 = OrganizationDomain.CheckSettlementGuardFavorabilityType(context, settlementId13);
				result = Serializer.Serialize(returnValue21, returnDataPool);
				break;
			}
			default:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(21, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Unsupported methodId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			}
			return result;
		}

		// Token: 0x06004785 RID: 18309 RVA: 0x0028372C File Offset: 0x0028192C
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
				this._modificationsFactions.ChangeRecording(monitoring);
				break;
			case 6:
				break;
			case 7:
				break;
			case 8:
				break;
			default:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(19, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Unsupported dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			}
		}

		// Token: 0x06004786 RID: 18310 RVA: 0x002837B8 File Offset: 0x002819B8
		public override int CheckModified(ushort dataId, ulong subId0, uint subId1, RawDataPool dataPool)
		{
			int result;
			switch (dataId)
			{
			case 0:
				result = this.CheckModified_Sects((short)subId0, (ushort)subId1, dataPool);
				break;
			case 1:
				result = this.CheckModified_CivilianSettlements((short)subId0, (ushort)subId1, dataPool);
				break;
			case 2:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(42, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to check modification of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 3:
				result = this.CheckModified_SectCharacters((int)subId0, (ushort)subId1, dataPool);
				break;
			case 4:
				result = this.CheckModified_CivilianSettlementCharacters((int)subId0, (ushort)subId1, dataPool);
				break;
			case 5:
			{
				bool flag = !BaseGameDataDomain.IsModified(this.DataStates, 5);
				if (flag)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 5);
					int offset = Serializer.SerializeModifications<int>(this._factions, dataPool, this._modificationsFactions);
					this._modificationsFactions.Reset();
					result = offset;
				}
				break;
			}
			case 6:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(42, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to check modification of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 7:
			{
				bool flag2 = !BaseGameDataDomain.IsModified(this.DataStates, 7);
				if (flag2)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 7);
					result = Serializer.Serialize(this.GetMartialArtTournamentPreparationInfoList(), dataPool);
				}
				break;
			}
			case 8:
			{
				bool flag3 = !BaseGameDataDomain.IsModified(this.DataStates, 8);
				if (flag3)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 8);
					result = Serializer.Serialize(this._previousMartialArtTournamentHosts, dataPool);
				}
				break;
			}
			default:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(19, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Unsupported dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			}
			return result;
		}

		// Token: 0x06004787 RID: 18311 RVA: 0x00283990 File Offset: 0x00281B90
		public override void ResetModifiedWrapper(ushort dataId, ulong subId0, uint subId1)
		{
			switch (dataId)
			{
			case 0:
				this.ResetModifiedWrapper_Sects((short)subId0, (ushort)subId1);
				break;
			case 1:
				this.ResetModifiedWrapper_CivilianSettlements((short)subId0, (ushort)subId1);
				break;
			case 2:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(48, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to reset modification state of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 3:
				this.ResetModifiedWrapper_SectCharacters((int)subId0, (ushort)subId1);
				break;
			case 4:
				this.ResetModifiedWrapper_CivilianSettlementCharacters((int)subId0, (ushort)subId1);
				break;
			case 5:
			{
				bool flag = !BaseGameDataDomain.IsModified(this.DataStates, 5);
				if (!flag)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 5);
					this._modificationsFactions.Reset();
				}
				break;
			}
			case 6:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(48, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to reset modification state of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 7:
			{
				bool flag2 = !BaseGameDataDomain.IsModified(this.DataStates, 7);
				if (!flag2)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 7);
				}
				break;
			}
			case 8:
			{
				bool flag3 = !BaseGameDataDomain.IsModified(this.DataStates, 8);
				if (!flag3)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 8);
				}
				break;
			}
			default:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(19, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Unsupported dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			}
		}

		// Token: 0x06004788 RID: 18312 RVA: 0x00283B20 File Offset: 0x00281D20
		public override bool IsModifiedWrapper(ushort dataId, ulong subId0, uint subId1)
		{
			bool result;
			switch (dataId)
			{
			case 0:
				result = this.IsModifiedWrapper_Sects((short)subId0, (ushort)subId1);
				break;
			case 1:
				result = this.IsModifiedWrapper_CivilianSettlements((short)subId0, (ushort)subId1);
				break;
			case 2:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(49, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to verify modification state of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 3:
				result = this.IsModifiedWrapper_SectCharacters((int)subId0, (ushort)subId1);
				break;
			case 4:
				result = this.IsModifiedWrapper_CivilianSettlementCharacters((int)subId0, (ushort)subId1);
				break;
			case 5:
				result = BaseGameDataDomain.IsModified(this.DataStates, 5);
				break;
			case 6:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(49, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to verify modification state of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 7:
				result = BaseGameDataDomain.IsModified(this.DataStates, 7);
				break;
			case 8:
				result = BaseGameDataDomain.IsModified(this.DataStates, 8);
				break;
			default:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(19, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Unsupported dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			}
			return result;
		}

		// Token: 0x06004789 RID: 18313 RVA: 0x00283C58 File Offset: 0x00281E58
		public override void InvalidateCache(BaseGameDataObject sourceObject, DataInfluence influence, DataContext context, bool unconditionallyInfluenceAll)
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			switch (influence.TargetIndicator.DataId)
			{
			case 0:
			{
				bool flag = !unconditionallyInfluenceAll;
				if (flag)
				{
					List<BaseGameDataObject> influencedObjects = InfluenceChecker.InfluencedObjectsPool.Get();
					bool influenceAll = InfluenceChecker.GetScope<short, Sect>(context, sourceObject, influence.Scope, this._sects, influencedObjects);
					bool flag2 = !influenceAll;
					if (flag2)
					{
						int influencedObjectsCount = influencedObjects.Count;
						for (int i = 0; i < influencedObjectsCount; i++)
						{
							BaseGameDataObject targetObject = influencedObjects[i];
							List<DataUid> targetUids = influence.TargetUids;
							int targetUidsCount = targetUids.Count;
							for (int j = 0; j < targetUidsCount; j++)
							{
								DataUid targetUid = targetUids[j];
								targetObject.InvalidateSelfAndInfluencedCache((ushort)targetUid.SubId1, context);
							}
						}
					}
					else
					{
						BaseGameDataDomain.InvalidateAllAndInfluencedCaches(OrganizationDomain.CacheInfluencesSects, this._dataStatesSects, influence, context);
					}
					influencedObjects.Clear();
					InfluenceChecker.InfluencedObjectsPool.Return(influencedObjects);
				}
				else
				{
					BaseGameDataDomain.InvalidateAllAndInfluencedCaches(OrganizationDomain.CacheInfluencesSects, this._dataStatesSects, influence, context);
				}
				return;
			}
			case 1:
			{
				bool flag3 = !unconditionallyInfluenceAll;
				if (flag3)
				{
					List<BaseGameDataObject> influencedObjects2 = InfluenceChecker.InfluencedObjectsPool.Get();
					bool influenceAll2 = InfluenceChecker.GetScope<short, CivilianSettlement>(context, sourceObject, influence.Scope, this._civilianSettlements, influencedObjects2);
					bool flag4 = !influenceAll2;
					if (flag4)
					{
						int influencedObjectsCount2 = influencedObjects2.Count;
						for (int k = 0; k < influencedObjectsCount2; k++)
						{
							BaseGameDataObject targetObject2 = influencedObjects2[k];
							List<DataUid> targetUids2 = influence.TargetUids;
							int targetUidsCount2 = targetUids2.Count;
							for (int l = 0; l < targetUidsCount2; l++)
							{
								DataUid targetUid2 = targetUids2[l];
								targetObject2.InvalidateSelfAndInfluencedCache((ushort)targetUid2.SubId1, context);
							}
						}
					}
					else
					{
						BaseGameDataDomain.InvalidateAllAndInfluencedCaches(OrganizationDomain.CacheInfluencesCivilianSettlements, this._dataStatesCivilianSettlements, influence, context);
					}
					influencedObjects2.Clear();
					InfluenceChecker.InfluencedObjectsPool.Return(influencedObjects2);
				}
				else
				{
					BaseGameDataDomain.InvalidateAllAndInfluencedCaches(OrganizationDomain.CacheInfluencesCivilianSettlements, this._dataStatesCivilianSettlements, influence, context);
				}
				return;
			}
			case 2:
				break;
			case 3:
			{
				bool flag5 = !unconditionallyInfluenceAll;
				if (flag5)
				{
					List<BaseGameDataObject> influencedObjects3 = InfluenceChecker.InfluencedObjectsPool.Get();
					bool influenceAll3 = InfluenceChecker.GetScope<int, SectCharacter>(context, sourceObject, influence.Scope, this._sectCharacters, influencedObjects3);
					bool flag6 = !influenceAll3;
					if (flag6)
					{
						int influencedObjectsCount3 = influencedObjects3.Count;
						for (int m = 0; m < influencedObjectsCount3; m++)
						{
							BaseGameDataObject targetObject3 = influencedObjects3[m];
							List<DataUid> targetUids3 = influence.TargetUids;
							int targetUidsCount3 = targetUids3.Count;
							for (int n = 0; n < targetUidsCount3; n++)
							{
								DataUid targetUid3 = targetUids3[n];
								targetObject3.InvalidateSelfAndInfluencedCache((ushort)targetUid3.SubId1, context);
							}
						}
					}
					else
					{
						BaseGameDataDomain.InvalidateAllAndInfluencedCaches(OrganizationDomain.CacheInfluencesSectCharacters, this._dataStatesSectCharacters, influence, context);
					}
					influencedObjects3.Clear();
					InfluenceChecker.InfluencedObjectsPool.Return(influencedObjects3);
				}
				else
				{
					BaseGameDataDomain.InvalidateAllAndInfluencedCaches(OrganizationDomain.CacheInfluencesSectCharacters, this._dataStatesSectCharacters, influence, context);
				}
				return;
			}
			case 4:
			{
				bool flag7 = !unconditionallyInfluenceAll;
				if (flag7)
				{
					List<BaseGameDataObject> influencedObjects4 = InfluenceChecker.InfluencedObjectsPool.Get();
					bool influenceAll4 = InfluenceChecker.GetScope<int, CivilianSettlementCharacter>(context, sourceObject, influence.Scope, this._civilianSettlementCharacters, influencedObjects4);
					bool flag8 = !influenceAll4;
					if (flag8)
					{
						int influencedObjectsCount4 = influencedObjects4.Count;
						for (int i2 = 0; i2 < influencedObjectsCount4; i2++)
						{
							BaseGameDataObject targetObject4 = influencedObjects4[i2];
							List<DataUid> targetUids4 = influence.TargetUids;
							int targetUidsCount4 = targetUids4.Count;
							for (int j2 = 0; j2 < targetUidsCount4; j2++)
							{
								DataUid targetUid4 = targetUids4[j2];
								targetObject4.InvalidateSelfAndInfluencedCache((ushort)targetUid4.SubId1, context);
							}
						}
					}
					else
					{
						BaseGameDataDomain.InvalidateAllAndInfluencedCaches(OrganizationDomain.CacheInfluencesCivilianSettlementCharacters, this._dataStatesCivilianSettlementCharacters, influence, context);
					}
					influencedObjects4.Clear();
					InfluenceChecker.InfluencedObjectsPool.Return(influencedObjects4);
				}
				else
				{
					BaseGameDataDomain.InvalidateAllAndInfluencedCaches(OrganizationDomain.CacheInfluencesCivilianSettlementCharacters, this._dataStatesCivilianSettlementCharacters, influence, context);
				}
				return;
			}
			case 5:
				break;
			case 6:
				break;
			case 7:
				BaseGameDataDomain.InvalidateSelfAndInfluencedCache(7, this.DataStates, OrganizationDomain.CacheInfluences, context);
				return;
			case 8:
				break;
			default:
				defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(19, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Unsupported dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(influence.TargetIndicator.DataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(48, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Cannot invalidate cache state of non-cache data ");
			defaultInterpolatedStringHandler.AppendFormatted<ushort>(influence.TargetIndicator.DataId);
			throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
		}

		// Token: 0x0600478A RID: 18314 RVA: 0x0028410C File Offset: 0x0028230C
		public unsafe override void ProcessArchiveResponse(OperationWrapper operation, byte* pResult)
		{
			switch (operation.DataId)
			{
			case 0:
				ResponseProcessor.ProcessDynamicObjectCollection<short, Sect>(operation, pResult, this._sects);
				break;
			case 1:
				ResponseProcessor.ProcessDynamicObjectCollection<short, CivilianSettlement>(operation, pResult, this._civilianSettlements);
				break;
			case 2:
				ResponseProcessor.ProcessSingleValue_BasicType_Fixed_Value_Single<short>(operation, pResult, ref this._nextSettlementId);
				break;
			case 3:
				ResponseProcessor.ProcessFixedObjectCollection<int, SectCharacter>(operation, pResult, this._sectCharacters);
				break;
			case 4:
				ResponseProcessor.ProcessFixedObjectCollection<int, CivilianSettlementCharacter>(operation, pResult, this._civilianSettlementCharacters);
				break;
			case 5:
				ResponseProcessor.ProcessSingleValueCollection_CustomType_Dynamic_Value<int, CharacterSet>(operation, pResult, this._factions);
				break;
			case 6:
				ResponseProcessor.ProcessSingleValue_BasicType_Fixed_Value_Array<sbyte>(operation, pResult, this._largeSectFavorabilities, 64);
				break;
			case 7:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(52, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Cannot process archive response of non-archive data ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.DataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 8:
				ResponseProcessor.ProcessSingleValue_BasicType_Fixed_Value_List<short>(operation, pResult, this._previousMartialArtTournamentHosts);
				break;
			default:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(19, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Unsupported dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.DataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			}
			bool flag = this._pendingLoadingOperationIds != null;
			if (flag)
			{
				uint currPendingOperationId = this._pendingLoadingOperationIds.Peek();
				bool flag2 = currPendingOperationId == operation.Id;
				if (flag2)
				{
					this._pendingLoadingOperationIds.Dequeue();
					bool flag3 = this._pendingLoadingOperationIds.Count <= 0;
					if (flag3)
					{
						this._pendingLoadingOperationIds = null;
						this.InitializeInternalDataOfCollections();
						this.OnLoadedArchiveData();
						DomainManager.Global.CompleteLoading(3);
					}
				}
			}
		}

		// Token: 0x0600478B RID: 18315 RVA: 0x002842C4 File Offset: 0x002824C4
		private void InitializeInternalDataOfCollections()
		{
			foreach (KeyValuePair<short, Sect> entry in this._sects)
			{
				Sect instance = entry.Value;
				instance.CollectionHelperData = this.HelperDataSects;
				instance.DataStatesOffset = this._dataStatesSects.Create();
			}
			foreach (KeyValuePair<short, CivilianSettlement> entry2 in this._civilianSettlements)
			{
				CivilianSettlement instance2 = entry2.Value;
				instance2.CollectionHelperData = this.HelperDataCivilianSettlements;
				instance2.DataStatesOffset = this._dataStatesCivilianSettlements.Create();
			}
			foreach (KeyValuePair<int, SectCharacter> entry3 in this._sectCharacters)
			{
				SectCharacter instance3 = entry3.Value;
				instance3.CollectionHelperData = this.HelperDataSectCharacters;
				instance3.DataStatesOffset = this._dataStatesSectCharacters.Create();
			}
			foreach (KeyValuePair<int, CivilianSettlementCharacter> entry4 in this._civilianSettlementCharacters)
			{
				CivilianSettlementCharacter instance4 = entry4.Value;
				instance4.CollectionHelperData = this.HelperDataCivilianSettlementCharacters;
				instance4.DataStatesOffset = this._dataStatesCivilianSettlementCharacters.Create();
			}
		}

		// Token: 0x0600478D RID: 18317 RVA: 0x002845A4 File Offset: 0x002827A4
		[CompilerGenerated]
		internal unsafe static int <CalcMartialArtTournamentPreparationInfoList>g__GetScore|21_0(long* rank, int length, int rankIndex)
		{
			int actualRankIndex = rankIndex;
			long score = rank[rankIndex] >> 32;
			for (int i = rankIndex + 1; i < length; i++)
			{
				long currScore = rank[i] >> 32;
				bool flag = currScore == score;
				if (!flag)
				{
					break;
				}
				actualRankIndex = i;
			}
			return 15 - (length - actualRankIndex - 1);
		}

		// Token: 0x0600478E RID: 18318 RVA: 0x00284600 File Offset: 0x00282800
		[CompilerGenerated]
		internal static void <SetInscribedCharactersForCreation>g__AddOrgInscribedCharacter|97_0(sbyte orgTemplateId, InscribedCharacter inscribedCharacter)
		{
			List<InscribedCharacter> charList;
			bool flag = !OrganizationDomain._orgInscribedCharIdMap.TryGetValue(orgTemplateId, out charList);
			if (flag)
			{
				charList = new List<InscribedCharacter>();
				OrganizationDomain._orgInscribedCharIdMap.Add(orgTemplateId, charList);
			}
			charList.Add(inscribedCharacter);
		}

		// Token: 0x040014B2 RID: 5298
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		// Token: 0x040014B3 RID: 5299
		[DomainData(DomainDataType.ObjectCollection, true, false, true, true)]
		private readonly Dictionary<short, Sect> _sects;

		// Token: 0x040014B4 RID: 5300
		[DomainData(DomainDataType.ObjectCollection, true, false, true, true)]
		private readonly Dictionary<short, CivilianSettlement> _civilianSettlements;

		// Token: 0x040014B5 RID: 5301
		[DomainData(DomainDataType.SingleValue, true, false, false, false)]
		private short _nextSettlementId;

		// Token: 0x040014B6 RID: 5302
		[DomainData(DomainDataType.ObjectCollection, true, false, true, true)]
		private readonly Dictionary<int, SectCharacter> _sectCharacters;

		// Token: 0x040014B7 RID: 5303
		[DomainData(DomainDataType.ObjectCollection, true, false, true, true)]
		private readonly Dictionary<int, CivilianSettlementCharacter> _civilianSettlementCharacters;

		// Token: 0x040014B8 RID: 5304
		[DomainData(DomainDataType.SingleValueCollection, true, false, true, true)]
		private readonly Dictionary<int, CharacterSet> _factions;

		// Token: 0x040014B9 RID: 5305
		[DomainData(DomainDataType.SingleValue, true, false, false, false, ArrayElementsCount = 64)]
		private sbyte[] _largeSectFavorabilities;

		// Token: 0x040014BA RID: 5306
		[DomainData(DomainDataType.SingleValue, false, true, true, true)]
		private List<MartialArtTournamentPreparationInfo> _martialArtTournamentPreparationInfoList;

		// Token: 0x040014BB RID: 5307
		[DomainData(DomainDataType.SingleValue, true, false, true, true)]
		private List<short> _previousMartialArtTournamentHosts;

		// Token: 0x040014BC RID: 5308
		private Dictionary<Location, Settlement> _locationSettlements;

		// Token: 0x040014BD RID: 5309
		private Dictionary<short, Settlement> _settlements;

		// Token: 0x040014BE RID: 5310
		private List<Settlement>[] _orgTemplateId2Settlements;

		// Token: 0x040014BF RID: 5311
		private Dictionary<int, SettlementCharacter> _settlementCharacters;

		// Token: 0x040014C0 RID: 5312
		public const sbyte MerchantGrade = 4;

		// Token: 0x040014C1 RID: 5313
		public bool ParallelUpdateOrganizationMembers = true;

		// Token: 0x040014C2 RID: 5314
		private SettlementCreatingInfo _settlementCreatingInfo;

		// Token: 0x040014C3 RID: 5315
		private static Dictionary<sbyte, List<InscribedCharacter>> _orgInscribedCharIdMap;

		// Token: 0x040014C4 RID: 5316
		private static StringBuilder _stringBuilder;

		// Token: 0x040014C5 RID: 5317
		private static readonly sbyte[] CreateFactionChance = new sbyte[]
		{
			30,
			10,
			40,
			20,
			50
		};

		// Token: 0x040014C6 RID: 5318
		private static readonly sbyte[] ExpandFactionChance = new sbyte[]
		{
			20,
			40,
			50,
			10,
			30
		};

		// Token: 0x040014C7 RID: 5319
		private static readonly sbyte[] JoinFactionChance = new sbyte[]
		{
			30,
			40,
			50,
			10,
			20
		};

		// Token: 0x040014C8 RID: 5320
		private static readonly sbyte[] JoinFactionFavorabilityBonus = new sbyte[]
		{
			0,
			5,
			10,
			15,
			0
		};

		// Token: 0x040014C9 RID: 5321
		private static readonly sbyte[] JoinFactionFavorabilityReq = new sbyte[]
		{
			1,
			1,
			2,
			1,
			2,
			3
		};

		// Token: 0x040014CA RID: 5322
		private static readonly sbyte[][] JoinFactionPriorities = new sbyte[][]
		{
			new sbyte[]
			{
				2,
				1,
				4,
				3,
				0,
				5
			},
			new sbyte[]
			{
				0,
				2,
				1,
				3,
				4,
				5
			},
			new sbyte[]
			{
				1,
				0,
				4,
				2,
				3,
				5
			},
			new sbyte[]
			{
				4,
				2,
				0,
				3,
				1,
				5
			},
			new sbyte[]
			{
				0,
				1,
				4,
				3,
				2,
				5
			}
		};

		// Token: 0x040014CB RID: 5323
		private readonly Dictionary<int, List<sbyte>> _sectFugitives = new Dictionary<int, List<sbyte>>();

		// Token: 0x040014CC RID: 5324
		private readonly Dictionary<int, sbyte> _sectPrisoners = new Dictionary<int, sbyte>();

		// Token: 0x040014CD RID: 5325
		private readonly List<SettlementBounty> _calculatedBountiesCache = new List<SettlementBounty>();

		// Token: 0x040014CE RID: 5326
		[Obsolete]
		private int _prisonGuardCharId = -1;

		// Token: 0x040014CF RID: 5327
		private static sbyte[] _allSectOrgTemplateIds;

		// Token: 0x040014D0 RID: 5328
		private static sbyte[] _maleSectOrgTemplateIds;

		// Token: 0x040014D1 RID: 5329
		private static sbyte[] _femaleSectOrgTemplateIds;

		// Token: 0x040014D2 RID: 5330
		private SettlementTreasuryLayers _currentTreasuryLayer;

		// Token: 0x040014D3 RID: 5331
		private int _firstGuardCharId = -1;

		// Token: 0x040014D4 RID: 5332
		private List<ItemSourceChange> _itemSourceChanges;

		// Token: 0x040014D5 RID: 5333
		private Inventory _getTreasuryInventory;

		// Token: 0x040014D6 RID: 5334
		private Inventory _stealTreasuryInventory;

		// Token: 0x040014D7 RID: 5335
		private Inventory _exchangeTreasuryInventory;

		// Token: 0x040014D8 RID: 5336
		private Inventory _storeTreasuryInventory;

		// Token: 0x040014D9 RID: 5337
		private static readonly DataInfluence[][] CacheInfluences = new DataInfluence[9][];

		// Token: 0x040014DA RID: 5338
		private static readonly DataInfluence[][] CacheInfluencesSects = new DataInfluence[20][];

		// Token: 0x040014DB RID: 5339
		private readonly ObjectCollectionDataStates _dataStatesSects = new ObjectCollectionDataStates(20, 0);

		// Token: 0x040014DC RID: 5340
		public readonly ObjectCollectionHelperData HelperDataSects;

		// Token: 0x040014DD RID: 5341
		private static readonly DataInfluence[][] CacheInfluencesCivilianSettlements = new DataInfluence[17][];

		// Token: 0x040014DE RID: 5342
		private readonly ObjectCollectionDataStates _dataStatesCivilianSettlements = new ObjectCollectionDataStates(17, 0);

		// Token: 0x040014DF RID: 5343
		public readonly ObjectCollectionHelperData HelperDataCivilianSettlements;

		// Token: 0x040014E0 RID: 5344
		private static readonly DataInfluence[][] CacheInfluencesSectCharacters = new DataInfluence[6][];

		// Token: 0x040014E1 RID: 5345
		private readonly ObjectCollectionDataStates _dataStatesSectCharacters = new ObjectCollectionDataStates(6, 0);

		// Token: 0x040014E2 RID: 5346
		public readonly ObjectCollectionHelperData HelperDataSectCharacters;

		// Token: 0x040014E3 RID: 5347
		private static readonly DataInfluence[][] CacheInfluencesCivilianSettlementCharacters = new DataInfluence[6][];

		// Token: 0x040014E4 RID: 5348
		private readonly ObjectCollectionDataStates _dataStatesCivilianSettlementCharacters = new ObjectCollectionDataStates(6, 0);

		// Token: 0x040014E5 RID: 5349
		public readonly ObjectCollectionHelperData HelperDataCivilianSettlementCharacters;

		// Token: 0x040014E6 RID: 5350
		private SingleValueCollectionModificationCollection<int> _modificationsFactions = SingleValueCollectionModificationCollection<int>.Create();

		// Token: 0x040014E7 RID: 5351
		private SpinLock _spinLockMartialArtTournamentPreparationInfoList = new SpinLock(false);

		// Token: 0x040014E8 RID: 5352
		private Queue<uint> _pendingLoadingOperationIds;

		// Token: 0x02000A7D RID: 2685
		public enum ESettlementTreasuryOperationResult
		{
			// Token: 0x04002AFA RID: 11002
			None,
			// Token: 0x04002AFB RID: 11003
			Exchange,
			// Token: 0x04002AFC RID: 11004
			Steal,
			// Token: 0x04002AFD RID: 11005
			Store
		}
	}
}
