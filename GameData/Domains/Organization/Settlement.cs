using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using CompDevLib.Interpreter;
using Config;
using Config.ConfigCells.Character;
using GameData.Common;
using GameData.Dependencies;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.Character.Display;
using GameData.Domains.Character.Relation;
using GameData.Domains.CombatSkill;
using GameData.Domains.Item;
using GameData.Domains.LifeRecord;
using GameData.Domains.Map;
using GameData.Domains.Organization.Display;
using GameData.Domains.Organization.SettlementTreasuryRecord;
using GameData.Domains.Taiwu.Profession;
using GameData.Domains.Taiwu.Profession.SkillsData;
using GameData.Domains.World;
using GameData.Domains.World.Notification;
using GameData.Utilities;
using Redzen.Random;

namespace GameData.Domains.Organization
{
	// Token: 0x02000647 RID: 1607
	public abstract class Settlement : BaseGameDataObject, IValueSelector
	{
		// Token: 0x060047D5 RID: 18389 RVA: 0x002888C8 File Offset: 0x00286AC8
		[SingleValueDependency(19, new ushort[]
		{
			30
		})]
		protected short CalcApprovingRateUpperLimitTempBonus()
		{
			return DomainManager.Extra.GetMaxApprovingRateTempBonus(this.Id);
		}

		// Token: 0x17000323 RID: 803
		// (get) Token: 0x060047D6 RID: 18390 RVA: 0x002888EA File Offset: 0x00286AEA
		public OrganizationItem OrganizationConfig
		{
			get
			{
				return Organization.Instance[this.OrgTemplateId];
			}
		}

		// Token: 0x060047D7 RID: 18391 RVA: 0x002888FC File Offset: 0x00286AFC
		protected Settlement()
		{
		}

		// Token: 0x060047D8 RID: 18392 RVA: 0x00288954 File Offset: 0x00286B54
		protected Settlement(short id, Location location, sbyte orgTemplateId, IRandomSource random)
		{
			this.Id = id;
			this.OrgTemplateId = orgTemplateId;
			this.Location = location;
			OrganizationItem orgConfig = Organization.Instance[orgTemplateId];
			ValueTuple<short, short> valueTuple = Settlement.CalcCultureAndSafety(orgConfig.Culture, random);
			this.Culture = valueTuple.Item1;
			this.MaxCulture = valueTuple.Item2;
			valueTuple = Settlement.CalcCultureAndSafety(orgConfig.Safety, random);
			this.Safety = valueTuple.Item1;
			this.MaxSafety = valueTuple.Item2;
			this.Population = orgConfig.Population;
			this.MaxPopulation = orgConfig.Population;
			this.Members = new OrgMemberCollection();
			this.LackingCoreMembers = new OrgMemberCollection();
		}

		// Token: 0x060047D9 RID: 18393 RVA: 0x00288A44 File Offset: 0x00286C44
		public void ChangeSafety(DataContext context, int delta)
		{
			this.Safety = (short)Math.Clamp((int)this.Safety + delta, 0, (int)this.MaxSafety);
			this.SetSafety(this.Safety, context);
			Events.RaiseSettlementInfoChanged(context, this);
		}

		// Token: 0x060047DA RID: 18394 RVA: 0x00288A78 File Offset: 0x00286C78
		public void ChangeCulture(DataContext context, int delta)
		{
			this.Culture = (short)Math.Clamp((int)this.Culture + delta, 0, (int)this.MaxCulture);
			this.SetCulture(this.Culture, context);
			Events.RaiseSettlementInfoChanged(context, this);
		}

		// Token: 0x060047DB RID: 18395 RVA: 0x00288AAC File Offset: 0x00286CAC
		public GameData.Domains.Character.Character GetLeader()
		{
			HashSet<int> maxGradeMembers = this.Members.GetMembers(8);
			foreach (int charId in maxGradeMembers)
			{
				GameData.Domains.Character.Character character;
				bool flag = DomainManager.Character.TryGetElement_Objects(charId, out character) && character.GetOrganizationInfo().Principal;
				if (flag)
				{
					return character;
				}
			}
			return null;
		}

		// Token: 0x060047DC RID: 18396 RVA: 0x00288B34 File Offset: 0x00286D34
		public int GetMaxSupportingBlockCount()
		{
			return (1 + this.Culture / 50 + this.Culture % 50 > 0) ? 1 : 0;
		}

		// Token: 0x060047DD RID: 18397 RVA: 0x00288B64 File Offset: 0x00286D64
		public sbyte GetPunishmentTypeSeverity(PunishmentTypeItem punishmentTypeCfg, bool includeDefault = false)
		{
			sbyte severity = -1;
			sbyte stateTemplateId = DomainManager.Map.GetStateTemplateIdByAreaId(this.Location.AreaId);
			return DomainManager.Extra.TryGetCustomizedCityPunishmentSeverity(stateTemplateId, this is Sect, punishmentTypeCfg.TemplateId, ref severity) ? severity : punishmentTypeCfg.GetSeverity(stateTemplateId, this is Sect, includeDefault);
		}

		// Token: 0x060047DE RID: 18398 RVA: 0x00288BC0 File Offset: 0x00286DC0
		public GameData.Domains.Character.Character GetAvailableHighMember(sbyte startHighGrade, sbyte endLowGrade, bool needAdult = true)
		{
			for (sbyte i = startHighGrade; i >= endLowGrade; i -= 1)
			{
				HashSet<int> maxGradeMembers = this.Members.GetMembers(i);
				foreach (int charId in maxGradeMembers)
				{
					GameData.Domains.Character.Character character;
					bool flag = DomainManager.Character.TryGetElement_Objects(charId, out character);
					if (flag)
					{
						bool flag2 = needAdult && character.GetAgeGroup() < 2;
						if (!flag2)
						{
							bool flag3 = character.IsActiveExternalRelationState(60);
							if (!flag3)
							{
								bool flag4 = character.GetKidnapperId() >= 0;
								if (!flag4)
								{
									return character;
								}
							}
						}
					}
				}
			}
			return null;
		}

		// Token: 0x060047DF RID: 18399 RVA: 0x00288C94 File Offset: 0x00286E94
		public void SortMembersByCombatPower()
		{
			this._membersSortedByCombatPower.Clear();
			for (sbyte grade = 0; grade < 9; grade += 1)
			{
				HashSet<int> members = this.Members.GetMembers(grade);
				foreach (int memberCharId in members)
				{
					GameData.Domains.Character.Character character = DomainManager.Character.GetElement_Objects(memberCharId);
					int combatPower = character.GetCombatPower();
					long key = ((long)combatPower << 32) + (long)memberCharId;
					this._membersSortedByCombatPower.Add(key, memberCharId);
				}
			}
		}

		// Token: 0x060047E0 RID: 18400 RVA: 0x00288D40 File Offset: 0x00286F40
		public long GetRankingInfo(int ranking)
		{
			bool flag = this._membersSortedByCombatPower.Count <= ranking;
			long result;
			if (flag)
			{
				result = 0L;
			}
			else
			{
				int index = this._membersSortedByCombatPower.Count - ranking;
				result = this._membersSortedByCombatPower.Keys[index];
			}
			return result;
		}

		// Token: 0x060047E1 RID: 18401 RVA: 0x00288D8C File Offset: 0x00286F8C
		public int GetRankingCombatPower(int ranking)
		{
			bool flag = this._membersSortedByCombatPower.Count <= ranking;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				int index = this._membersSortedByCombatPower.Count - ranking;
				long key = this._membersSortedByCombatPower.Keys[index];
				result = (int)(key >> 32);
			}
			return result;
		}

		// Token: 0x060047E2 RID: 18402 RVA: 0x00288DDC File Offset: 0x00286FDC
		public int GetCharacterRanking(int charId)
		{
			int index = this._membersSortedByCombatPower.IndexOfValue(charId);
			return this._membersSortedByCombatPower.Count - index;
		}

		// Token: 0x060047E3 RID: 18403
		public abstract SettlementNameRelatedData GetNameRelatedData();

		// Token: 0x060047E4 RID: 18404 RVA: 0x00288E08 File Offset: 0x00287008
		public override string ToString()
		{
			return this.GetNameRelatedData().GetName();
		}

		// Token: 0x060047E5 RID: 18405 RVA: 0x00288E24 File Offset: 0x00287024
		public int CalcInfluencePower()
		{
			int value = 0;
			for (sbyte grade = 0; grade < 9; grade += 1)
			{
				HashSet<int> gradeMembers = this.Members.GetMembers(grade);
				foreach (int memberCharId in gradeMembers)
				{
					SettlementCharacter settlementCharacter = DomainManager.Organization.GetSettlementCharacter(memberCharId);
					value += (int)settlementCharacter.GetInfluencePower();
				}
			}
			return value;
		}

		// Token: 0x060047E6 RID: 18406 RVA: 0x00288EB8 File Offset: 0x002870B8
		public short CalcApprovingRate()
		{
			int value = 0;
			for (sbyte grade = 0; grade < 9; grade += 1)
			{
				HashSet<int> gradeMembers = this.Members.GetMembers(grade);
				foreach (int memberCharId in gradeMembers)
				{
					SettlementCharacter settlementCharacter = DomainManager.Organization.GetSettlementCharacter(memberCharId);
					value += (int)settlementCharacter.GetApprovingRate();
				}
			}
			int upperLimit = (int)(OrganizationDomain.GetApprovingRateUpperLimit() + this.ApprovingRateUpperLimitBonus + this.GetApprovingRateUpperLimitTempBonus());
			upperLimit = Math.Min(upperLimit, 1000);
			return (upperLimit >= 0) ? ((short)Math.Clamp(value, 0, upperLimit)) : 0;
		}

		// Token: 0x060047E7 RID: 18407 RVA: 0x00288F7C File Offset: 0x0028717C
		public short CalcApprovingRateTotal()
		{
			int value = 0;
			for (sbyte grade = 0; grade < 9; grade += 1)
			{
				HashSet<int> gradeMembers = this.Members.GetMembers(grade);
				foreach (int memberCharId in gradeMembers)
				{
					SettlementCharacter settlementCharacter = DomainManager.Organization.GetSettlementCharacter(memberCharId);
					value += (int)settlementCharacter.GetApprovingRate();
				}
			}
			return (short)Math.Clamp(value, 0, 1000);
		}

		// Token: 0x060047E8 RID: 18408 RVA: 0x0028901C File Offset: 0x0028721C
		public void UpdateMemberGrades(DataContext context)
		{
			bool flag = this.OrgTemplateId == 16;
			if (!flag)
			{
				bool flag2 = this.OrgTemplateId == 12;
				if (flag2)
				{
					this.UpdateWuxianMemberGrades(context);
				}
				else
				{
					ProfessionData professionData = DomainManager.Extra.GetProfessionData(8);
					AristocratSkillsData skillsData = professionData.GetSkillsData<AristocratSkillsData>();
					OrganizationItem orgConfig = Organization.Instance[this.OrgTemplateId];
					List<int> potentialSuccessors = ObjectPool<List<int>>.Instance.Get();
					LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
					int worldPopulationFactor = DomainManager.World.GetWorldPopulationFactor();
					bool hereditary = orgConfig.Hereditary;
					if (hereditary)
					{
						HashSet<int> handled = ObjectPool<HashSet<int>>.Instance.Get();
						for (sbyte grade = 8; grade > 0; grade -= 1)
						{
							HashSet<int> lackingMembers = this.LackingCoreMembers.GetMembers(grade);
							handled.Clear();
							OrganizationInfo orgInfo = new OrganizationInfo(this.OrgTemplateId, grade, true, this.Id);
							OrganizationMemberItem orgMemberCfg = OrganizationDomain.GetOrgMemberConfig(orgInfo);
							HashSet<int> gradeMembers = this.Members.GetMembers(grade);
							int currAmount = (orgMemberCfg.DeputySpouseDowngrade >= 0) ? this.GetPrincipalAmount(grade) : gradeMembers.Count;
							int requiredAmount = this.GetExpectedCoreMemberAmount(orgMemberCfg);
							bool flag3 = !orgMemberCfg.RestrictPrincipalAmount;
							if (flag3)
							{
								requiredAmount = requiredAmount * worldPopulationFactor / 100;
							}
							foreach (int charId in lackingMembers)
							{
								bool flag4 = orgMemberCfg.Amount > 0 && currAmount >= requiredAmount;
								if (flag4)
								{
									handled.UnionWith(lackingMembers);
									break;
								}
								this.GetOrganizationMemberPotentialSuccessors(charId, orgInfo, potentialSuccessors);
								bool flag5 = potentialSuccessors.Count <= 0;
								if (!flag5)
								{
									int successorId = skillsData.GetRecommendedCharIdInList(potentialSuccessors);
									bool flag6 = successorId >= 0;
									if (flag6)
									{
										skillsData.OfflineRemoveRecommendedCharId(successorId);
										DomainManager.Extra.SetProfessionData(context, professionData);
									}
									else
									{
										successorId = potentialSuccessors.GetRandom(context.Random);
									}
									GameData.Domains.Character.Character successor = DomainManager.Character.GetElement_Objects(successorId);
									DomainManager.Organization.ChangeGrade(context, successor, grade, true);
									int successorSpouseId = DomainManager.Character.GetAliveSpouse(successorId);
									bool flag7 = successorSpouseId >= 0;
									if (flag7)
									{
										GameData.Domains.Character.Character spouse = DomainManager.Character.GetElement_Objects(successorSpouseId);
										DomainManager.Organization.UpdateGradeAccordingToSpouse(context, spouse, successor);
									}
									handled.Add(charId);
									bool flag8 = grade == 8;
									if (flag8)
									{
										this.OnOrganizationLeaderChange(context, successorId, successor.GetGender());
									}
									currAmount++;
								}
							}
							lackingMembers.ExceptWith(handled);
							this.SetLackingCoreMembers(this.LackingCoreMembers, context);
						}
						ObjectPool<HashSet<int>>.Instance.Return(handled);
					}
					else
					{
						for (sbyte grade2 = 8; grade2 > 0; grade2 -= 1)
						{
							OrganizationMemberItem orgMemberCfg2 = OrganizationDomain.GetOrgMemberConfig(this.OrgTemplateId, grade2);
							OrganizationInfo targetOrgInfo = new OrganizationInfo(this.OrgTemplateId, grade2, true, this.Id);
							HashSet<int> gradeMembers2 = this.Members.GetMembers(grade2);
							int currAmount2 = (orgMemberCfg2.DeputySpouseDowngrade >= 0) ? this.GetPrincipalAmount(grade2) : gradeMembers2.Count;
							int requiredAmount2 = this.GetExpectedCoreMemberAmount(orgMemberCfg2);
							bool flag9 = !orgMemberCfg2.RestrictPrincipalAmount;
							if (flag9)
							{
								requiredAmount2 = requiredAmount2 * worldPopulationFactor / 100;
							}
							bool flag10 = currAmount2 >= requiredAmount2;
							if (!flag10)
							{
								int upgradeAmount = requiredAmount2 - currAmount2;
								potentialSuccessors.Clear();
								for (int i = 0; i < upgradeAmount; i++)
								{
									bool flag11 = potentialSuccessors.Count <= 0;
									if (flag11)
									{
										this.GetNonHereditaryPotentialSuccessors(targetOrgInfo, potentialSuccessors);
									}
									bool flag12 = potentialSuccessors.Count <= 0;
									if (flag12)
									{
										break;
									}
									int successorId2 = skillsData.GetRecommendedCharIdInList(potentialSuccessors);
									bool flag13 = successorId2 >= 0;
									if (flag13)
									{
										skillsData.OfflineRemoveRecommendedCharId(successorId2);
										DomainManager.Extra.SetProfessionData(context, professionData);
										potentialSuccessors.Remove(successorId2);
									}
									else
									{
										int index = context.Random.Next(potentialSuccessors.Count);
										successorId2 = potentialSuccessors[index];
										potentialSuccessors.RemoveAt(index);
									}
									GameData.Domains.Character.Character successor2 = DomainManager.Character.GetElement_Objects(successorId2);
									DomainManager.Organization.ChangeGrade(context, successor2, grade2, true);
									int successorSpouseId2 = DomainManager.Character.GetAliveSpouse(successorId2);
									bool flag14 = successorSpouseId2 >= 0;
									if (flag14)
									{
										GameData.Domains.Character.Character spouse2 = DomainManager.Character.GetElement_Objects(successorSpouseId2);
										DomainManager.Organization.UpdateGradeAccordingToSpouse(context, spouse2, successor2);
									}
									bool flag15 = grade2 == 8;
									if (flag15)
									{
										this.OnOrganizationLeaderChange(context, successorId2, successor2.GetGender());
									}
								}
							}
						}
					}
					this.RecruitOrCreateLackingMembers(context);
					ObjectPool<List<int>>.Instance.Return(potentialSuccessors);
				}
			}
		}

		// Token: 0x060047E9 RID: 18409 RVA: 0x002894F4 File Offset: 0x002876F4
		private void UpdateWuxianMemberGrades(DataContext context)
		{
			ProfessionData professionData = DomainManager.Extra.GetProfessionData(8);
			AristocratSkillsData skillsData = professionData.GetSkillsData<AristocratSkillsData>();
			List<int> potentialSuccessors = ObjectPool<List<int>>.Instance.Get();
			List<int> potentialSaintesses = ObjectPool<List<int>>.Instance.Get();
			HashSet<int> handled = ObjectPool<HashSet<int>>.Instance.Get();
			potentialSuccessors.Clear();
			potentialSaintesses.Clear();
			handled.Clear();
			int worldPopulationFactor = DomainManager.World.GetWorldPopulationFactor();
			this.GetWuxianPotentialSaintessesByHereditary(potentialSaintesses);
			HashSet<int> lackingMembers = this.LackingCoreMembers.GetMembers(8);
			OrganizationInfo orgInfo = new OrganizationInfo(this.OrgTemplateId, 8, true, this.Id);
			OrganizationMemberItem orgMemberCfg = OrganizationDomain.GetOrgMemberConfig(orgInfo);
			HashSet<int> gradeMembers = this.Members.GetMembers(8);
			HashSet<int> saintesses = this.Members.GetMembers(7);
			int expectedAmount = this.GetExpectedCoreMemberAmount(orgMemberCfg);
			bool flag = gradeMembers.Count < expectedAmount;
			if (flag)
			{
				foreach (int charId in lackingMembers)
				{
					bool flag2 = potentialSuccessors.Count <= 0;
					if (flag2)
					{
						this.GetWuxianLeaderPotentialSuccessors(orgInfo, potentialSaintesses, potentialSuccessors);
					}
					bool flag3 = potentialSuccessors.Count <= 0;
					if (flag3)
					{
						break;
					}
					int successorId = skillsData.GetRecommendedCharIdInList(potentialSuccessors);
					bool flag4 = successorId >= 0;
					if (flag4)
					{
						skillsData.OfflineRemoveRecommendedCharId(successorId);
						DomainManager.Extra.SetProfessionData(context, professionData);
						potentialSuccessors.Remove(successorId);
					}
					else
					{
						int index = context.Random.Next(potentialSuccessors.Count);
						successorId = potentialSuccessors[index];
						potentialSuccessors.RemoveAt(index);
					}
					GameData.Domains.Character.Character successor = DomainManager.Character.GetElement_Objects(successorId);
					DomainManager.Organization.ChangeGrade(context, successor, 8, true);
					this.OnOrganizationLeaderChange(context, successorId, successor.GetGender());
					handled.Add(charId);
				}
				lackingMembers.ExceptWith(handled);
				handled.Clear();
			}
			else
			{
				lackingMembers.Clear();
			}
			orgInfo = new OrganizationInfo(this.OrgTemplateId, 7, true, this.Id);
			orgMemberCfg = OrganizationDomain.GetOrgMemberConfig(orgInfo);
			lackingMembers = this.LackingCoreMembers.GetMembers(7);
			gradeMembers = this.Members.GetMembers(7);
			expectedAmount = this.GetExpectedCoreMemberAmount(orgMemberCfg);
			bool flag5 = gradeMembers.Count < expectedAmount;
			if (flag5)
			{
				potentialSuccessors.Clear();
				foreach (int charId2 in lackingMembers)
				{
					bool flag6 = potentialSuccessors.Count <= 0;
					if (flag6)
					{
						this.GetWuxianSaintessesPotentialSuccessors(orgInfo, potentialSaintesses, potentialSuccessors);
					}
					bool flag7 = potentialSuccessors.Count <= 0;
					if (flag7)
					{
						break;
					}
					int successorId2 = skillsData.GetRecommendedCharIdInList(potentialSuccessors);
					bool flag8 = successorId2 >= 0;
					if (flag8)
					{
						skillsData.OfflineRemoveRecommendedCharId(successorId2);
						DomainManager.Extra.SetProfessionData(context, professionData);
						potentialSuccessors.Remove(successorId2);
					}
					else
					{
						int index2 = context.Random.Next(potentialSuccessors.Count);
						successorId2 = potentialSuccessors[index2];
						potentialSuccessors.RemoveAt(index2);
					}
					potentialSaintesses.Remove(successorId2);
					GameData.Domains.Character.Character successor2 = DomainManager.Character.GetElement_Objects(successorId2);
					DomainManager.Organization.ChangeGrade(context, successor2, 7, true);
					handled.Add(charId2);
				}
				lackingMembers.ExceptWith(handled);
				handled.Clear();
			}
			else
			{
				lackingMembers.Clear();
			}
			for (sbyte grade = 6; grade > 0; grade -= 1)
			{
				lackingMembers = this.LackingCoreMembers.GetMembers(grade);
				orgInfo = new OrganizationInfo(this.OrgTemplateId, grade, true, this.Id);
				orgMemberCfg = OrganizationDomain.GetOrgMemberConfig(orgInfo);
				gradeMembers = this.Members.GetMembers(grade);
				int currAmount = (orgMemberCfg.DeputySpouseDowngrade >= 0) ? this.GetPrincipalAmount(grade) : gradeMembers.Count;
				int requiredAmount = this.GetExpectedCoreMemberAmount(orgMemberCfg);
				bool flag9 = !orgMemberCfg.RestrictPrincipalAmount;
				if (flag9)
				{
					requiredAmount = requiredAmount * worldPopulationFactor / 100;
				}
				handled.Clear();
				foreach (int charId3 in lackingMembers)
				{
					bool flag10 = orgMemberCfg.Amount > 0 && currAmount >= requiredAmount;
					if (flag10)
					{
						handled.UnionWith(lackingMembers);
						break;
					}
					this.GetOrganizationMemberPotentialSuccessors(charId3, orgInfo, potentialSuccessors);
					bool flag11 = potentialSuccessors.Count <= 0;
					if (!flag11)
					{
						int successorId3 = skillsData.GetRecommendedCharIdInList(potentialSuccessors);
						bool flag12 = successorId3 >= 0;
						if (flag12)
						{
							skillsData.OfflineRemoveRecommendedCharId(successorId3);
							DomainManager.Extra.SetProfessionData(context, professionData);
						}
						else
						{
							successorId3 = potentialSuccessors.GetRandom(context.Random);
						}
						GameData.Domains.Character.Character successor3 = DomainManager.Character.GetElement_Objects(successorId3);
						DomainManager.Organization.ChangeGrade(context, successor3, grade, true);
						handled.Add(charId3);
						currAmount++;
					}
				}
				lackingMembers.ExceptWith(handled);
			}
			this.RecruitOrCreateLackingMembers(context);
			ObjectPool<List<int>>.Instance.Return(potentialSuccessors);
			ObjectPool<List<int>>.Instance.Return(potentialSaintesses);
			ObjectPool<HashSet<int>>.Instance.Return(handled);
			this.SetLackingCoreMembers(this.LackingCoreMembers, context);
		}

		// Token: 0x060047EA RID: 18410 RVA: 0x00289A6C File Offset: 0x00287C6C
		protected virtual void RecruitOrCreateLackingMembers(DataContext context)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060047EB RID: 18411 RVA: 0x00289A74 File Offset: 0x00287C74
		public int GetExpectedCoreMemberAmount(OrganizationMemberItem orgMemberCfg)
		{
			OrganizationItem orgCfg = Organization.Instance[this.OrgTemplateId];
			bool flag = !orgCfg.IsSect || orgMemberCfg.RestrictPrincipalAmount;
			int result;
			if (flag)
			{
				result = (int)orgMemberCfg.Amount;
			}
			else
			{
				sbyte sectMainStoryTaskStatus = DomainManager.World.GetSectMainStoryTaskStatus(this.OrgTemplateId);
				if (!true)
				{
				}
				sbyte b;
				if (sectMainStoryTaskStatus != 1)
				{
					if (sectMainStoryTaskStatus != 2)
					{
						b = orgMemberCfg.Amount;
					}
					else
					{
						b = orgMemberCfg.DownAmount;
					}
				}
				else
				{
					b = orgMemberCfg.UpAmount;
				}
				if (!true)
				{
				}
				result = (int)b;
			}
			return result;
		}

		// Token: 0x060047EC RID: 18412 RVA: 0x00289AFC File Offset: 0x00287CFC
		public int GetPrincipalAmount(sbyte grade)
		{
			int principalAmount = 0;
			HashSet<int> gradeMembers = this.Members.GetMembers(grade);
			foreach (int charId in gradeMembers)
			{
				GameData.Domains.Character.Character character;
				bool flag = DomainManager.Character.TryGetElement_Objects(charId, out character) && character.GetOrganizationInfo().Principal;
				if (flag)
				{
					principalAmount++;
				}
			}
			return principalAmount;
		}

		// Token: 0x060047ED RID: 18413 RVA: 0x00289B84 File Offset: 0x00287D84
		private void GetWuxianPotentialSaintessesByHereditary(List<int> potentialSaintesses)
		{
			HashSet<int> clanLeaders = this.Members.GetMembers(5);
			foreach (int clanLeaderId in clanLeaders)
			{
				RelatedCharacters clanLeaderRelatedCharIds = DomainManager.Character.GetRelatedCharacters(clanLeaderId);
				bool flag = clanLeaderRelatedCharIds == null;
				if (!flag)
				{
					this.GetSaintessCandidates(clanLeaderRelatedCharIds.BloodChildren.GetCollection(), potentialSaintesses);
					this.GetSaintessCandidates(clanLeaderRelatedCharIds.StepChildren.GetCollection(), potentialSaintesses);
					this.GetSaintessCandidates(clanLeaderRelatedCharIds.AdoptiveChildren.GetCollection(), potentialSaintesses);
				}
			}
		}

		// Token: 0x060047EE RID: 18414 RVA: 0x00289C30 File Offset: 0x00287E30
		private void GetWuxianLeaderPotentialSuccessors(OrganizationInfo orgInfo, List<int> potentialSaintesses, List<int> potentialSuccessors)
		{
			HashSet<int> saintesses = this.Members.GetMembers(7);
			bool flag = saintesses.Count > 0;
			if (flag)
			{
				Settlement.GetOrganizationMemberPotentialSuccessorsInSet(orgInfo, saintesses, potentialSuccessors);
			}
			bool flag2 = potentialSuccessors.Count <= 0;
			if (flag2)
			{
				this.GetWuxianSaintessesPotentialSuccessors(orgInfo, potentialSaintesses, potentialSuccessors);
			}
		}

		// Token: 0x060047EF RID: 18415 RVA: 0x00289C7C File Offset: 0x00287E7C
		private void GetWuxianSaintessesPotentialSuccessors(OrganizationInfo orgInfo, List<int> potentialSaintesses, List<int> potentialSuccessors)
		{
			Settlement.GetOrganizationMemberPotentialSuccessorsInSet(orgInfo, potentialSaintesses, potentialSuccessors);
			bool flag = potentialSuccessors.Count <= 0;
			if (flag)
			{
				for (sbyte successorGrade = 4; successorGrade >= 0; successorGrade -= 1)
				{
					HashSet<int> sourceMembers = this.Members.GetMembers(successorGrade);
					Settlement.GetOrganizationMemberPotentialSuccessorsInSet(orgInfo, sourceMembers, potentialSuccessors);
					bool flag2 = potentialSuccessors.Count > 0;
					if (flag2)
					{
						break;
					}
				}
			}
		}

		// Token: 0x060047F0 RID: 18416 RVA: 0x00289CE0 File Offset: 0x00287EE0
		private void GetNonHereditaryPotentialSuccessors(OrganizationInfo orgInfo, List<int> potentialSuccessors)
		{
			for (sbyte successorGrade = orgInfo.Grade - 1; successorGrade >= 0; successorGrade -= 1)
			{
				HashSet<int> sourceMembers = this.Members.GetMembers(successorGrade);
				Settlement.GetOrganizationMemberPotentialSuccessorsInSet(orgInfo, sourceMembers, potentialSuccessors);
				bool flag = potentialSuccessors.Count > 0;
				if (flag)
				{
					break;
				}
			}
		}

		// Token: 0x060047F1 RID: 18417 RVA: 0x00289D34 File Offset: 0x00287F34
		private void GetOrganizationMemberPotentialSuccessors(int charId, OrganizationInfo orgInfo, List<int> potentialSuccessors)
		{
			potentialSuccessors.Clear();
			RelatedCharacters relatedCharacters = DomainManager.Character.GetRelatedCharacters(charId);
			bool flag = relatedCharacters == null;
			if (!flag)
			{
				Settlement.GetOrganizationMemberPotentialSuccessorsInSet(orgInfo, relatedCharacters.BloodChildren.GetCollection(), potentialSuccessors);
				bool flag2 = potentialSuccessors.Count > 0;
				if (!flag2)
				{
					Settlement.GetOrganizationMemberPotentialSuccessorsInSet(orgInfo, relatedCharacters.StepChildren.GetCollection(), potentialSuccessors);
					bool flag3 = potentialSuccessors.Count > 0;
					if (!flag3)
					{
						Settlement.GetOrganizationMemberPotentialSuccessorsInSet(orgInfo, relatedCharacters.AdoptiveChildren.GetCollection(), potentialSuccessors);
						bool flag4 = potentialSuccessors.Count > 0;
						if (!flag4)
						{
							bool flag5 = relatedCharacters.HusbandsAndWives.GetCount() > 0;
							if (flag5)
							{
								foreach (int spouseId in relatedCharacters.HusbandsAndWives.GetCollection())
								{
									GameData.Domains.Character.Character spouse;
									bool flag6 = !DomainManager.Character.TryGetElement_Objects(spouseId, out spouse);
									if (!flag6)
									{
										OrganizationInfo spouseOrgInfo = spouse.GetOrganizationInfo();
										bool flag7 = spouseOrgInfo.OrgTemplateId != orgInfo.OrgTemplateId;
										if (flag7)
										{
											break;
										}
										bool flag8 = spouseOrgInfo.SettlementId != orgInfo.SettlementId;
										if (flag8)
										{
											break;
										}
										bool flag9 = spouseOrgInfo.Grade > orgInfo.Grade;
										if (flag9)
										{
											break;
										}
										bool flag10 = spouseOrgInfo.Grade == orgInfo.Grade && spouseOrgInfo.Principal;
										if (flag10)
										{
											break;
										}
										potentialSuccessors.Add(spouseId);
									}
								}
								bool flag11 = potentialSuccessors.Count > 0;
								if (flag11)
								{
									return;
								}
							}
							Settlement.GetOrganizationMemberPotentialSuccessorsInSet(orgInfo, relatedCharacters.BloodBrothersAndSisters.GetCollection(), potentialSuccessors);
							bool flag12 = potentialSuccessors.Count > 0;
							if (!flag12)
							{
								Settlement.GetOrganizationMemberPotentialSuccessorsInSet(orgInfo, relatedCharacters.StepBrothersAndSisters.GetCollection(), potentialSuccessors);
								bool flag13 = potentialSuccessors.Count > 0;
								if (!flag13)
								{
									Settlement.GetOrganizationMemberPotentialSuccessorsInSet(orgInfo, relatedCharacters.AdoptiveBrothersAndSisters.GetCollection(), potentialSuccessors);
									bool flag14 = potentialSuccessors.Count > 0;
									if (!flag14)
									{
										Settlement.GetOrganizationMemberPotentialSuccessorsInSet(orgInfo, relatedCharacters.BloodParents.GetCollection(), potentialSuccessors);
										bool flag15 = potentialSuccessors.Count > 0;
										if (!flag15)
										{
											Settlement.GetOrganizationMemberPotentialSuccessorsInSet(orgInfo, relatedCharacters.StepParents.GetCollection(), potentialSuccessors);
											bool flag16 = potentialSuccessors.Count > 0;
											if (!flag16)
											{
												Settlement.GetOrganizationMemberPotentialSuccessorsInSet(orgInfo, relatedCharacters.AdoptiveParents.GetCollection(), potentialSuccessors);
												bool flag17 = potentialSuccessors.Count > 0;
												if (!flag17)
												{
													this.GetNonHereditaryPotentialSuccessors(orgInfo, potentialSuccessors);
												}
											}
										}
									}
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x060047F2 RID: 18418 RVA: 0x00289FD4 File Offset: 0x002881D4
		internal void GetOrganizationMemberPotentialSuccessorsForDisplay(int charId, OrganizationInfo orgInfo, List<int> potentialSuccessors)
		{
			OrganizationMemberItem memberConfig = OrganizationDomain.GetOrgMemberConfig(orgInfo);
			bool flag = memberConfig.TemplateId == 145;
			if (flag)
			{
				List<int> potentialSaintesses = ObjectPool<List<int>>.Instance.Get();
				potentialSaintesses.Clear();
				this.GetWuxianPotentialSaintessesByHereditary(potentialSaintesses);
				this.GetWuxianLeaderPotentialSuccessors(orgInfo, potentialSaintesses, potentialSuccessors);
			}
			else
			{
				bool flag2 = memberConfig.TemplateId == 146;
				if (flag2)
				{
					List<int> potentialSaintesses2 = ObjectPool<List<int>>.Instance.Get();
					potentialSaintesses2.Clear();
					this.GetWuxianPotentialSaintessesByHereditary(potentialSaintesses2);
					this.GetWuxianSaintessesPotentialSuccessors(orgInfo, potentialSaintesses2, potentialSuccessors);
				}
				else
				{
					OrganizationItem item = Organization.Instance.GetItem(orgInfo.OrgTemplateId);
					bool flag3 = item != null && item.Hereditary;
					if (flag3)
					{
						this.GetOrganizationMemberPotentialSuccessors(charId, orgInfo, potentialSuccessors);
					}
					else
					{
						this.GetNonHereditaryPotentialSuccessors(orgInfo, potentialSuccessors);
					}
				}
			}
		}

		// Token: 0x060047F3 RID: 18419 RVA: 0x0028A09C File Offset: 0x0028829C
		private static void GetOrganizationMemberPotentialSuccessorsInSet(OrganizationInfo orgInfo, IEnumerable<int> charIds, List<int> result)
		{
			result.Clear();
			int currMaxInfluencePower = -1;
			sbyte requiredGender = OrganizationDomain.GetOrgMemberConfig(orgInfo).Gender;
			foreach (int relatedCharId in charIds)
			{
				GameData.Domains.Character.Character relatedChar;
				bool flag = !DomainManager.Character.TryGetElement_Objects(relatedCharId, out relatedChar);
				if (!flag)
				{
					bool flag2 = !relatedChar.IsInteractableAsIntelligentCharacter();
					if (!flag2)
					{
						OrganizationInfo relatedCharOrgInfo = relatedChar.GetOrganizationInfo();
						bool flag3 = orgInfo.SettlementId != relatedCharOrgInfo.SettlementId || orgInfo.Grade <= relatedCharOrgInfo.Grade || !relatedCharOrgInfo.Principal || (requiredGender != -1 && relatedChar.GetGender() != requiredGender);
						if (!flag3)
						{
							SettlementCharacter settlementCharacter = DomainManager.Organization.GetSettlementCharacter(relatedCharId);
							short influencePower = settlementCharacter.GetInfluencePower();
							bool flag4 = (int)influencePower > currMaxInfluencePower;
							if (flag4)
							{
								result.Clear();
								currMaxInfluencePower = (int)influencePower;
								result.Add(relatedCharId);
							}
							else
							{
								bool flag5 = (int)influencePower == currMaxInfluencePower;
								if (flag5)
								{
									result.Add(relatedCharId);
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x060047F4 RID: 18420 RVA: 0x0028A1C8 File Offset: 0x002883C8
		private void GetSaintessCandidates(HashSet<int> charSet, List<int> result)
		{
			foreach (int charId in charSet)
			{
				GameData.Domains.Character.Character character;
				bool flag = !DomainManager.Character.TryGetElement_Objects(charId, out character);
				if (!flag)
				{
					bool flag2 = character.GetGender() != 0;
					if (!flag2)
					{
						RelatedCharacters relatedChars = DomainManager.Character.GetRelatedCharacters(charId);
						bool flag3 = relatedChars.AdoptiveChildren.GetCount() > 0 || relatedChars.BloodChildren.GetCount() > 0 || relatedChars.StepChildren.GetCount() > 0 || relatedChars.HusbandsAndWives.GetCount() > 0;
						if (!flag3)
						{
							result.Add(charId);
						}
					}
				}
			}
		}

		// Token: 0x060047F5 RID: 18421 RVA: 0x0028A29C File Offset: 0x0028849C
		public bool RemoveSettlementFeatures(DataContext context, GameData.Domains.Character.Character character)
		{
			IReadOnlyList<SettlementMemberFeature> settlementMemberFeatures = DomainManager.Extra.GetSettlementMemberFeatures(this.Id);
			bool flag = settlementMemberFeatures == null;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				bool modified = false;
				List<short> featureIds = character.GetFeatureIds();
				foreach (SettlementMemberFeature featureInfo in settlementMemberFeatures)
				{
					int index = featureIds.IndexOf(featureInfo.FeatureId);
					bool flag2 = index < 0;
					if (!flag2)
					{
						character.RemoveFeature(context, featureInfo.FeatureId);
						modified = true;
					}
				}
				result = modified;
			}
			return result;
		}

		// Token: 0x060047F6 RID: 18422 RVA: 0x0028A344 File Offset: 0x00288544
		public bool AddSettlementFeatures(DataContext context, GameData.Domains.Character.Character character)
		{
			IReadOnlyList<SettlementMemberFeature> settlementMemberFeatures = DomainManager.Extra.GetSettlementMemberFeatures(this.Id);
			bool flag = settlementMemberFeatures == null;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				bool modified = false;
				sbyte grade = character.GetOrganizationInfo().Grade;
				foreach (SettlementMemberFeature featureInfo in settlementMemberFeatures)
				{
					bool flag2 = grade < featureInfo.MinGrade || grade > featureInfo.MaxGrade;
					if (!flag2)
					{
						modified |= character.AddFeature(context, featureInfo.FeatureId, false);
					}
				}
				result = modified;
			}
			return result;
		}

		// Token: 0x060047F7 RID: 18423 RVA: 0x0028A3F4 File Offset: 0x002885F4
		private void OnOrganizationLeaderChange(DataContext context, int charId, sbyte gender)
		{
			Dictionary<int, ValueTuple<GameData.Domains.Character.Character, short>> baseInfluencePowers = new Dictionary<int, ValueTuple<GameData.Domains.Character.Character, short>>();
			HashSet<int> relatedCharIds = new HashSet<int>();
			short influencePowerUpdateInterval = Organization.Instance[this.OrgTemplateId].InfluencePowerUpdateInterval;
			bool flag = influencePowerUpdateInterval > 0;
			if (flag)
			{
				this.UpdateInfluencePowers(context, baseInfluencePowers, relatedCharIds);
				this.SetInfluencePowerUpdateDate(DomainManager.World.GetCurrDate() + (int)influencePowerUpdateInterval, context);
			}
			MonthlyNotificationCollection monthlyNotificationCollection = DomainManager.World.GetMonthlyNotificationCollection();
			bool flag2 = OrganizationDomain.IsSect(this.OrgTemplateId);
			if (flag2)
			{
				monthlyNotificationCollection.AddSectUpgrade(charId, this.Id, this.OrgTemplateId, 8, true, gender);
			}
			else
			{
				monthlyNotificationCollection.AddCivilianSettlementUpgrade(charId, this.Id, this.OrgTemplateId, 8, true, gender);
			}
		}

		// Token: 0x060047F8 RID: 18424 RVA: 0x0028A49C File Offset: 0x0028869C
		public void UpdateInfluencePowers(DataContext context, [TupleElementNames(new string[]
		{
			"character",
			"baseInfluencePower"
		})] Dictionary<int, ValueTuple<GameData.Domains.Character.Character, short>> baseInfluencePowers, HashSet<int> relatedCharIds)
		{
			CivilianSettlement cs = this as CivilianSettlement;
			short mainMorality = (cs != null) ? cs.UpdateMainMorality(context) : Organization.Instance[this.OrgTemplateId].MainMorality;
			sbyte mainBehaviorType = GameData.Domains.Character.BehaviorType.GetBehaviorType(mainMorality);
			int normalInfluenceFactor;
			int combatInfluenceFactor;
			int combatFactorUnit;
			if (!OrganizationDomain.IsSect(this.OrgTemplateId))
			{
				normalInfluenceFactor = 20;
				combatInfluenceFactor = 80;
				combatFactorUnit = 4000;
			}
			else
			{
				normalInfluenceFactor = 20;
				combatInfluenceFactor = 80;
				combatFactorUnit = 2000;
			}
			baseInfluencePowers.Clear();
			short[] gradeInfluencePowers = GlobalConfig.Instance.OrgCharBaseInfluencePowers;
			for (sbyte grade = 0; grade <= 8; grade += 1)
			{
				HashSet<int> gradeMembers = this.Members.GetMembers(grade);
				SettlementTreasury treasury = this.GetTreasury(grade);
				foreach (int charId in gradeMembers)
				{
					short gradeInfluencePower = gradeInfluencePowers[(int)grade];
					GameData.Domains.Character.Character character = DomainManager.Character.GetElement_Objects(charId);
					sbyte behaviorType = GameData.Domains.Character.BehaviorType.GetBehaviorType(character.GetMorality());
					bool principal = character.GetOrganizationInfo().Principal;
					int baseInfluencePower = (int)Settlement.CalcBaseInfluencePower(gradeInfluencePower, mainBehaviorType, behaviorType, principal);
					baseInfluencePower = baseInfluencePower * treasury.CalcBonusInfluencePower(charId) / 100;
					baseInfluencePowers.Add(charId, new ValueTuple<GameData.Domains.Character.Character, short>(character, (short)((baseInfluencePower * normalInfluenceFactor + character.GetCombatPower() * combatInfluenceFactor / combatFactorUnit) / 100)));
				}
			}
			ProfessionData professionData = DomainManager.Extra.GetProfessionData(8);
			AristocratSkillsData skillsData = professionData.GetSkillsData<AristocratSkillsData>();
			bool skillsDataChanged = false;
			short approvingRate = this.CalcApprovingRate();
			foreach (KeyValuePair<int, ValueTuple<GameData.Domains.Character.Character, short>> entry in baseInfluencePowers)
			{
				int charId2 = entry.Key;
				ValueTuple<GameData.Domains.Character.Character, short> value = entry.Value;
				GameData.Domains.Character.Character character2 = value.Item1;
				short baseInfluencePower2 = value.Item2;
				SettlementCharacter settlementCharacter = DomainManager.Organization.GetSettlementCharacter(charId2);
				int influencePower = (int)settlementCharacter.CalcInfluencePower(character2, baseInfluencePower2, baseInfluencePowers, relatedCharIds);
				int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
				RelatedCharacter selfToTaiwu;
				bool flag = approvingRate >= 600 && DomainManager.Character.TryGetRelation(charId2, taiwuCharId, out selfToTaiwu);
				if (flag)
				{
					sbyte favorType = FavorabilityType.GetFavorabilityType(selfToTaiwu.Favorability);
					influencePower += influencePower * (int)favorType * 5 / 100;
				}
				settlementCharacter.SetInfluencePower((short)Math.Clamp(influencePower, 0, 32767), context);
				skillsDataChanged = (skillsData.OfflineRemoveInfluencePowerBonus(charId2) || skillsDataChanged);
			}
			this.UpdateTreasury(context);
			bool flag2 = skillsDataChanged;
			if (flag2)
			{
				DomainManager.Extra.SetProfessionData(context, professionData);
			}
		}

		// Token: 0x060047F9 RID: 18425 RVA: 0x0028A740 File Offset: 0x00288940
		public void UpdateTaiwuVillagerInfluencePowers(DataContext context, [TupleElementNames(new string[]
		{
			"character",
			"baseInfluencePower"
		})] Dictionary<int, ValueTuple<GameData.Domains.Character.Character, short>> baseInfluencePowers, HashSet<int> relatedCharIds)
		{
			baseInfluencePowers.Clear();
			short[] gradeInfluencePowers = GlobalConfig.Instance.OrgCharBaseInfluencePowers;
			for (sbyte grade = 0; grade <= 8; grade += 1)
			{
				HashSet<int> gradeMembers = this.Members.GetMembers(grade);
				foreach (int charId in gradeMembers)
				{
					short baseInfluencePower = gradeInfluencePowers[(int)grade];
					GameData.Domains.Character.Character character = DomainManager.Character.GetElement_Objects(charId);
					baseInfluencePowers.Add(charId, new ValueTuple<GameData.Domains.Character.Character, short>(character, (short)(((int)(baseInfluencePower * 20) + character.GetCombatPower() * 80 / 4000) / 100)));
				}
			}
			foreach (KeyValuePair<int, ValueTuple<GameData.Domains.Character.Character, short>> entry in baseInfluencePowers)
			{
				int charId2 = entry.Key;
				ValueTuple<GameData.Domains.Character.Character, short> value = entry.Value;
				GameData.Domains.Character.Character character2 = value.Item1;
				short baseInfluencePower2 = value.Item2;
				SettlementCharacter settlementCharacter = DomainManager.Organization.GetSettlementCharacter(charId2);
				int influencePower = (int)settlementCharacter.CalcInfluencePower(character2, baseInfluencePower2, baseInfluencePowers, relatedCharIds);
				settlementCharacter.SetInfluencePower((short)Math.Clamp(influencePower, 0, 32767), context);
			}
			DomainManager.Taiwu.UpdateTaiwuTreasury(context);
		}

		// Token: 0x060047FA RID: 18426 RVA: 0x0028A8A4 File Offset: 0x00288AA4
		private static short CalcBaseInfluencePower(short gradeInfluencePower, sbyte mainBehaviorType, sbyte behaviorType, bool principal)
		{
			int delta = (int)(mainBehaviorType - behaviorType);
			int percent = (delta == 0) ? 100 : ((delta == 1 || delta == -1) ? 75 : 50);
			int influencePower = (int)gradeInfluencePower * percent / 100;
			bool flag = !principal;
			if (flag)
			{
				influencePower /= 2;
			}
			return (short)influencePower;
		}

		// Token: 0x060047FB RID: 18427 RVA: 0x0028A8E8 File Offset: 0x00288AE8
		[return: TupleElementNames(new string[]
		{
			"currValue",
			"maxValue"
		})]
		private static ValueTuple<short, short> CalcCultureAndSafety(short configValue, IRandomSource random)
		{
			bool flag = configValue < 0;
			int maxValue;
			if (flag)
			{
				int value = (int)(-(int)configValue);
				maxValue = random.Next(value / 2, value + 1) * 5;
			}
			else
			{
				bool flag2 = configValue != 0 && random.CheckPercentProb(50);
				if (flag2)
				{
					int variation = (1 + random.Next(5)) * 5;
					maxValue = (int)configValue + (random.CheckPercentProb(35) ? variation : (-variation));
					bool flag3 = maxValue < 0;
					if (flag3)
					{
						maxValue = 0;
					}
				}
				else
				{
					maxValue = (int)configValue;
				}
			}
			return new ValueTuple<short, short>((short)(maxValue / 2), (short)maxValue);
		}

		// Token: 0x060047FC RID: 18428 RVA: 0x0028A96C File Offset: 0x00288B6C
		[return: TupleElementNames(new string[]
		{
			"Curr",
			"Max"
		})]
		public ValueTuple<int, int> GetPopulationInfo()
		{
			short factor = WorldCreation.DefValue.WorldPopulation.InfluenceFactors[(int)DomainManager.World.GetWorldPopulationType()];
			int curr = 0;
			int extra = 0;
			foreach (int charId in this.Members)
			{
				GameData.Domains.Character.Character character;
				bool flag = DomainManager.Character.TryGetElement_Objects(charId, out character) && character.GetCreatingType() == 1;
				if (flag)
				{
					bool flag2 = (character.GetDarkAshProtector() & 4294967263U) == 0U;
					if (flag2)
					{
						curr++;
					}
					else
					{
						extra++;
					}
				}
			}
			return new ValueTuple<int, int>(curr + extra, (this.OrganizationConfig.PopulationThreshold != -1) ? ((int)factor * this.OrganizationConfig.PopulationThreshold / 100 + extra) : -1);
		}

		// Token: 0x17000324 RID: 804
		// (get) Token: 0x060047FD RID: 18429 RVA: 0x0028AA44 File Offset: 0x00288C44
		public SettlementLayeredTreasuries Treasuries
		{
			get
			{
				if (this._treasuries == null)
				{
					SettlementLayeredTreasuries treasuries;
					this._treasuries = (DomainManager.Extra.TryGetElement_SettlementLayeredTreasuries(this.Id, out treasuries) ? treasuries : new SettlementLayeredTreasuries());
				}
				return this._treasuries;
			}
		}

		// Token: 0x060047FE RID: 18430 RVA: 0x0028AA88 File Offset: 0x00288C88
		public bool HasTreasury()
		{
			return this.OrgTemplateId != 0 && this.OrgTemplateId != 16 && this.Location.IsValid();
		}

		// Token: 0x060047FF RID: 18431 RVA: 0x0028AABA File Offset: 0x00288CBA
		public SettlementTreasury GetTreasury(sbyte grade)
		{
			return this.Treasuries.GetTreasury(this.GetLayer(grade));
		}

		// Token: 0x06004800 RID: 18432 RVA: 0x0028AACE File Offset: 0x00288CCE
		public int GetMemberSelfImproveSpeedFactor()
		{
			return GlobalConfig.Instance.MemberSelfImproveSpeedFactor[(int)this.Treasuries.GetTreasuryResourceStatus()];
		}

		// Token: 0x06004801 RID: 18433 RVA: 0x0028AAEC File Offset: 0x00288CEC
		public int CalcItemContribution(ItemKey itemKey, int amount)
		{
			short itemSubType = ItemTemplateHelper.GetItemSubType(itemKey.ItemType, itemKey.TemplateId);
			int value = DomainManager.Item.GetValue(itemKey);
			sbyte grade = ItemTemplateHelper.GetGrade(itemKey.ItemType, itemKey.TemplateId);
			SettlementTreasury treasury = this.GetTreasury(this.Treasuries, grade);
			return treasury.CalcAdjustedWorth(itemSubType, value) * amount * GlobalConfig.Instance.ItemContributionPercent / 100;
		}

		// Token: 0x06004802 RID: 18434 RVA: 0x0028AB58 File Offset: 0x00288D58
		public void InitializeTreasurySupplyRequirements()
		{
			SettlementLayeredTreasuries treasuries = this.Treasuries;
			foreach (SettlementTreasury treasury in treasuries.SettlementTreasuries)
			{
				ValueTuple<sbyte, sbyte> groupGradeRange = Grade.GetGroupGradeRange(treasury.LayerIndex);
				sbyte minGrade = groupGradeRange.Item1;
				sbyte maxGrade = groupGradeRange.Item2;
				OrganizationItem orgConfig = Organization.Instance[this.OrgTemplateId];
				for (sbyte grade = minGrade; grade <= maxGrade; grade += 1)
				{
					short memberTemplateId = orgConfig.Members[(int)grade];
					OrganizationMemberItem memberConfig = OrganizationMember.Instance[memberTemplateId];
					foreach (PresetInventoryItem presetItem in memberConfig.Inventory)
					{
						short templateId;
						bool flag = !this.CanItemBeSupplied(presetItem.Type, presetItem.TemplateId, grade, out templateId);
						if (!flag)
						{
							ShortPair key = new ShortPair((short)presetItem.Type, (short)grade);
							this._supplyItems.TryAdd(key, new List<short>());
							this._supplyItems[key].Add(templateId);
						}
					}
					foreach (PresetEquipmentItemWithProb presetItem2 in memberConfig.Equipment)
					{
						short templateId2;
						bool flag2 = !this.CanItemBeSupplied(presetItem2.Type, presetItem2.TemplateId, grade, out templateId2);
						if (!flag2)
						{
							ShortPair key2 = new ShortPair((short)presetItem2.Type, (short)grade);
							this._supplyItems.TryAdd(key2, new List<short>());
							this._supplyItems[key2].Add(templateId2);
						}
					}
				}
			}
			foreach (PresetOrgMemberCombatSkill presetSkill in OrganizationMember.Instance[Organization.Instance[this.OrgTemplateId].Members[8]].CombatSkills)
			{
				CombatSkillItem presetSkillCfg = Config.CombatSkill.Instance[presetSkill.SkillGroupId];
				IReadOnlyList<CombatSkillItem> group = CombatSkillDomain.GetLearnableCombatSkills(presetSkillCfg.SectId, presetSkillCfg.Type);
				foreach (CombatSkillItem config in group)
				{
					this._supplyBooks.TryAdd(config.Grade, new List<short>());
					this._supplyBooks[config.Grade].Add(config.BookId);
				}
			}
		}

		// Token: 0x06004803 RID: 18435 RVA: 0x0028AE20 File Offset: 0x00289020
		public void UpdateTreasuryOnAdvanceMonth(DataContext context)
		{
			bool flag = !this.HasTreasury();
			if (!flag)
			{
				SettlementLayeredTreasuries treasuries = this.Treasuries;
				treasuries.AlertTime = (byte)Math.Max(0, (int)(treasuries.AlertTime - 1));
				foreach (SettlementTreasury treasury3 in treasuries.SettlementTreasuries)
				{
					treasury3.ClearMemberUsedPresetContribution();
				}
				foreach (SettlementTreasury treasury2 in treasuries.SettlementTreasuries)
				{
					foreach (int id in treasury2.GuardIds.GetCollection().Where(delegate(int x)
					{
						GameData.Domains.Character.Character character;
						return !DomainManager.Character.TryGetElement_Objects(x, out character) || character.GetCreatingType() != 1;
					}).ToArray<int>())
					{
						treasury2.GuardIds.Remove(id);
					}
				}
				int id;
				bool flag2 = treasuries.SettlementTreasuries.Any(delegate(SettlementTreasury treasury)
				{
					bool result;
					if (treasury.GuardIds.GetCount() >= GlobalConfig.Instance.TreasuryGuardCount)
					{
						result = treasury.GuardIds.GetCollection().Any((int id) => !Sect.CanBeGuard(id));
					}
					else
					{
						result = true;
					}
					return result;
				});
				if (flag2)
				{
					this.ForceUpdateTreasuryGuards(context);
				}
				Array.Fill<bool>(this.HasTriggeredAllowEntryEvent, false);
				DomainManager.Extra.SetTreasuries(context, this.Id, treasuries, false);
			}
		}

		// Token: 0x06004804 RID: 18436 RVA: 0x0028AF5C File Offset: 0x0028915C
		public void UpdateTreasury(DataContext context)
		{
			bool flag = !this.HasTreasury();
			if (!flag)
			{
				SettlementLayeredTreasuries treasuries = this.Treasuries;
				int currDate = DomainManager.World.GetCurrDate();
				SettlementTreasuryRecordCollection settlementTreasuryRecordCollection = DomainManager.Organization.GetSettlementTreasuryRecordCollection(context, this.Id);
				settlementTreasuryRecordCollection.Clear();
				settlementTreasuryRecordCollection.AddClearRecord(currDate, this.Id);
				settlementTreasuryRecordCollection.AddSupplementResource(currDate, this.Id);
				settlementTreasuryRecordCollection.AddSupplementItem(currDate, this.Id);
				SettlementTreasuryLayers templateLayer = this.GetLayer(0);
				SettlementTreasury hobbyTemplate = this.GetTreasury(treasuries, templateLayer);
				this.OfflineUpdateTreasuryHobbies(context.Random, hobbyTemplate);
				int prevTotalWorth = 0;
				foreach (SettlementTreasuryLayers layer in Enum.GetValues<SettlementTreasuryLayers>())
				{
					SettlementTreasury treasury = this.GetTreasury(treasuries, layer);
					treasury.Contributions.Clear();
					bool flag2 = layer != templateLayer;
					if (flag2)
					{
						treasury.LovingItemSubTypes.Clear();
						treasury.HatingItemSubTypes.Clear();
						treasury.LovingItemSubTypes.AddRange(hobbyTemplate.LovingItemSubTypes);
						treasury.HatingItemSubTypes.AddRange(hobbyTemplate.HatingItemSubTypes);
					}
					prevTotalWorth += this.OfflineClearTreasury(context, treasury);
				}
				treasuries.SupplyLevelAddOn = 0;
				treasuries.ResupplyTotalValue = this.OfflineResupplyTreasury(context);
				bool flag3 = treasuries.ResupplyTotalValue * GlobalConfig.Instance.TreasurySupplyLevelUpPercent / 100 < prevTotalWorth;
				if (flag3)
				{
					treasuries.SupplyLevelAddOn = 1;
					treasuries.ResupplyTotalValue = this.OfflineResupplyTreasury(context);
				}
				this.OfflineUpdateTreasuryGuards(context, treasuries);
				DomainManager.Extra.SetTreasuries(context, this.Id, treasuries, true);
				DomainManager.Organization.SetSettlementTreasuryRecordCollection(context, this.Id, settlementTreasuryRecordCollection);
			}
		}

		// Token: 0x06004805 RID: 18437 RVA: 0x0028B114 File Offset: 0x00289314
		private void OfflineUpdateTreasuryHobbies(IRandomSource random, SettlementTreasury treasury)
		{
			treasury.LovingItemSubTypes.Clear();
			treasury.HatingItemSubTypes.Clear();
			MapAreaData mapAreaData = DomainManager.Map.GetElement_Areas((int)this.Location.AreaId);
			MapAreaItem mapAreaCfg = mapAreaData.GetConfig();
			treasury.LovingItemSubTypes.AddRange(mapAreaCfg.LovingItemSubTypes);
			treasury.HatingItemSubTypes.AddRange(mapAreaCfg.HatingItemSubTypes);
			List<int> maxInfluenceCharIds = ObjectPool<List<int>>.Instance.Get();
			sbyte grade = 9;
			for (;;)
			{
				sbyte b = grade;
				grade = b - 1;
				if (b <= 6)
				{
					break;
				}
				HashSet<int> gradeMembers = this.Members.GetMembers(grade);
				int maxInfluencePower = int.MinValue;
				maxInfluenceCharIds.Clear();
				foreach (int charId in gradeMembers)
				{
					SettlementCharacter settlementCharacter = DomainManager.Organization.GetSettlementCharacter(charId);
					short influencePower = settlementCharacter.GetInfluencePower();
					bool flag = (int)influencePower > maxInfluencePower;
					if (flag)
					{
						maxInfluencePower = (int)influencePower;
						maxInfluenceCharIds.Clear();
						maxInfluenceCharIds.Add(charId);
					}
					else
					{
						bool flag2 = (int)influencePower == maxInfluencePower;
						if (flag2)
						{
							maxInfluenceCharIds.Add(charId);
						}
					}
				}
				bool flag3 = maxInfluenceCharIds.Count <= 0;
				if (!flag3)
				{
					int maxInfluenceCharId = maxInfluenceCharIds.GetRandom(random);
					GameData.Domains.Character.Character character = DomainManager.Character.GetElement_Objects(maxInfluenceCharId);
					short lovingItemSubType = character.GetLovingItemSubType();
					short hatingItemSubType = character.GetHatingItemSubType();
					bool flag4 = !treasury.LovingItemSubTypes.Contains(lovingItemSubType) && !treasury.HatingItemSubTypes.Contains(lovingItemSubType);
					if (flag4)
					{
						treasury.LovingItemSubTypes.Add(lovingItemSubType);
					}
					bool flag5 = !treasury.LovingItemSubTypes.Contains(hatingItemSubType) && !treasury.HatingItemSubTypes.Contains(hatingItemSubType);
					if (flag5)
					{
						treasury.HatingItemSubTypes.Add(hatingItemSubType);
					}
				}
			}
			ObjectPool<List<int>>.Instance.Return(maxInfluenceCharIds);
		}

		// Token: 0x06004806 RID: 18438 RVA: 0x0028B308 File Offset: 0x00289508
		private unsafe int OfflineClearTreasury(DataContext context, SettlementTreasury treasury)
		{
			int res = 0;
			for (sbyte resourceType = 0; resourceType < 7; resourceType += 1)
			{
				res += ResourceTypeHelper.ResourceAmountToWorth(resourceType, *treasury.Resources[(int)resourceType]);
				treasury.Resources.Set((int)resourceType, 0);
			}
			res += treasury.Inventory.GetTotalValue();
			foreach (ItemKey itemKey in treasury.Inventory.Items.Keys)
			{
				DomainManager.Item.RemoveItem(context, itemKey);
			}
			treasury.Inventory.Items.Clear();
			return res;
		}

		// Token: 0x06004807 RID: 18439 RVA: 0x0028B3D0 File Offset: 0x002895D0
		private int OfflineResupplyTreasury(DataContext context)
		{
			int res = 0;
			SettlementLayeredTreasuries treasuries = this.Treasuries;
			foreach (SettlementTreasuryLayers layer in Enum.GetValues<SettlementTreasuryLayers>())
			{
				res += this.OfflineResupplyTreasury(context, this.GetTreasury(treasuries, layer));
			}
			return res;
		}

		// Token: 0x06004808 RID: 18440 RVA: 0x0028B41C File Offset: 0x0028961C
		private int OfflineResupplyTreasury(DataContext context, SettlementTreasury treasury)
		{
			int res = 0;
			ValueTuple<sbyte, sbyte> groupGradeRange = Grade.GetGroupGradeRange(treasury.LayerIndex);
			sbyte minGrade = groupGradeRange.Item1;
			sbyte maxGrade = groupGradeRange.Item2;
			int supplyLevel = this.GetSupplyLevel();
			int addOn = this.Treasuries.SupplyLevelAddOn;
			bool flag = addOn > 0;
			short[] range;
			sbyte[] supplyCounts;
			if (flag)
			{
				short[] rangeMax = GlobalConfig.Instance.TreasuryResourceSupplyRanges[supplyLevel];
				short[] rangeMin = GlobalConfig.Instance.TreasuryResourceSupplyRanges[supplyLevel - addOn];
				range = new short[rangeMax.Length];
				for (int i = 0; i < rangeMax.Length; i++)
				{
					range[i] = rangeMax[i] - rangeMin[i];
				}
				sbyte[] countsMax = GlobalConfig.Instance.TreasuryItemSupplyCounts[supplyLevel];
				sbyte[] countsMin = GlobalConfig.Instance.TreasuryItemSupplyCounts[supplyLevel - addOn];
				supplyCounts = new sbyte[countsMax.Length];
				for (int j = 0; j < countsMax.Length; j++)
				{
					supplyCounts[j] = countsMax[j] - countsMin[j];
				}
			}
			else
			{
				range = GlobalConfig.Instance.TreasuryResourceSupplyRanges[supplyLevel];
				supplyCounts = GlobalConfig.Instance.TreasuryItemSupplyCounts[supplyLevel];
			}
			for (sbyte resourceType = 0; resourceType < 7; resourceType += 1)
			{
				int satisfyingThreshold = this.GetResourceSupplyThreshold(resourceType, maxGrade) * (int)GameData.Domains.World.SharedMethods.GetGainResourcePercent(13) / 100;
				int supplyWorth = satisfyingThreshold * context.Random.Next((int)range[0], (int)(range[1] + 1)) / 100;
				int supplyAmount = ResourceTypeHelper.WorthToResourceAmount(resourceType, supplyWorth);
				treasury.Resources.Add(resourceType, supplyAmount);
				res += ResourceTypeHelper.ResourceAmountToWorth(resourceType, supplyAmount);
			}
			for (sbyte grade = minGrade; grade <= maxGrade; grade += 1)
			{
				sbyte supplyCount = supplyCounts[(int)grade];
				for (sbyte itemType = 0; itemType < 13; itemType += 1)
				{
					ShortPair key = new ShortPair((short)itemType, (short)grade);
					List<short> templateIds;
					bool flag2 = !this._supplyItems.TryGetValue(key, out templateIds);
					if (!flag2)
					{
						bool flag3 = supplyCount <= 0;
						if (!flag3)
						{
							for (int k = 0; k < (int)supplyCount; k++)
							{
								short templateId = templateIds.GetRandom(context.Random);
								ItemKey itemKey = DomainManager.Item.CreateItem(context, itemType, templateId);
								treasury.Inventory.OfflineAdd(itemKey, 1);
								DomainManager.Item.SetOwner(itemKey, ItemOwnerType.Treasury, (int)this.Id);
								res += DomainManager.Item.GetValue(itemKey);
							}
						}
					}
				}
				List<short> books;
				bool flag4 = supplyCounts[(int)grade] <= 0 || !this._supplyBooks.TryGetValue(grade, out books) || books.Count <= 0;
				if (!flag4)
				{
					for (int l = 0; l < (int)supplyCounts[(int)grade]; l++)
					{
						short bookId = books.GetRandom(context.Random);
						ItemKey itemKey2 = DomainManager.Item.CreateItem(context, 10, bookId);
						treasury.Inventory.OfflineAdd(itemKey2, 1);
						DomainManager.Item.SetOwner(itemKey2, ItemOwnerType.Treasury, (int)this.Id);
						res += DomainManager.Item.GetValue(itemKey2);
					}
				}
			}
			return res;
		}

		// Token: 0x06004809 RID: 18441 RVA: 0x0028B74C File Offset: 0x0028994C
		public void ConfiscateItem(DataContext context, GameData.Domains.Character.Character character, List<ItemKey> itemKeys)
		{
			string tag = this.ToString();
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(26, 2);
			defaultInterpolatedStringHandler.AppendLiteral("Confiscating ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(itemKeys.Count);
			defaultInterpolatedStringHandler.AppendLiteral(" items from ");
			defaultInterpolatedStringHandler.AppendFormatted<GameData.Domains.Character.Character>(character);
			defaultInterpolatedStringHandler.AppendLiteral(".");
			AdaptableLog.TagInfo(tag, defaultInterpolatedStringHandler.ToStringAndClear());
			Inventory inventory = character.GetInventory();
			SettlementLayeredTreasuries treasuries = this.Treasuries;
			foreach (ItemKey itemKey in itemKeys)
			{
				bool flag = !itemKey.IsValid();
				if (!flag)
				{
					sbyte grade = ItemTemplateHelper.GetGrade(itemKey.ItemType, itemKey.TemplateId);
					int amount;
					bool flag2 = !inventory.Items.TryGetValue(itemKey, out amount);
					if (flag2)
					{
						int equipmentIndex = character.GetEquipment().IndexOf(itemKey);
						Tester.Assert(equipmentIndex >= 0, "");
						character.ChangeEquipment(context, (sbyte)equipmentIndex, -1, itemKey);
						amount = 1;
					}
					inventory.OfflineRemove(itemKey, amount);
					this.GetTreasury(treasuries, grade).Inventory.OfflineAdd(itemKey, amount);
					Events.RaiseItemRemovedFromInventory(context, character, itemKey, amount);
					DomainManager.Item.SetOwner(itemKey, ItemOwnerType.Treasury, (int)this.Id);
					SettlementTreasuryRecordCollection settlementTreasuryRecordCollection = DomainManager.Organization.GetSettlementTreasuryRecordCollection(context, this.Id);
					int currDate = DomainManager.World.GetCurrDate();
					settlementTreasuryRecordCollection.AddConfiscateItem(currDate, this.Id, character.GetId(), itemKey.ItemType, itemKey.TemplateId);
					DomainManager.Organization.SetSettlementTreasuryRecordCollection(context, this.Id, settlementTreasuryRecordCollection);
				}
			}
			character.SetInventory(inventory, context);
			DomainManager.Extra.SetTreasuries(context, this.Id, treasuries, true);
		}

		// Token: 0x0600480A RID: 18442 RVA: 0x0028B944 File Offset: 0x00289B44
		public void ConfiscateResources(DataContext context, GameData.Domains.Character.Character character, ref ResourceInts resources)
		{
			int totalWorth = resources.GetTotalWorth();
			bool flag = totalWorth <= 0;
			if (!flag)
			{
				string tag = this.ToString();
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(39, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Confiscating ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(totalWorth);
				defaultInterpolatedStringHandler.AppendLiteral(" worth of resources from ");
				defaultInterpolatedStringHandler.AppendFormatted<GameData.Domains.Character.Character>(character);
				defaultInterpolatedStringHandler.AppendLiteral(".");
				AdaptableLog.TagInfo(tag, defaultInterpolatedStringHandler.ToStringAndClear());
				sbyte grade = character.GetOrganizationInfo().Grade;
				ref ResourceInts charResources = ref character.GetResources();
				charResources = charResources.Subtract(ref resources);
				character.SetResources(ref charResources, context);
				SettlementLayeredTreasuries treasuries = this.Treasuries;
				this.GetTreasury(treasuries, grade).Resources.Add(ref resources);
				DomainManager.Extra.SetTreasuries(context, this.Id, treasuries, true);
			}
		}

		// Token: 0x0600480B RID: 18443 RVA: 0x0028BA20 File Offset: 0x00289C20
		public void StoreItemInTreasury(DataContext context, GameData.Domains.Character.Character character, ItemKey itemKey, int amount, sbyte layerIndex)
		{
			SettlementLayeredTreasuries treasuries = this.Treasuries;
			SettlementTreasury treasury = (layerIndex >= 0) ? treasuries.GetTreasury(layerIndex) : this.GetTreasury(treasuries, ItemTemplateHelper.GetGrade(itemKey.ItemType, itemKey.TemplateId));
			LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
			int charId = character.GetId();
			bool isTaiwu = charId == DomainManager.Taiwu.GetTaiwuCharId();
			int currDate = DomainManager.World.GetCurrDate();
			SettlementTreasuryRecordCollection settlementTreasuryRecordCollection = DomainManager.Organization.GetSettlementTreasuryRecordCollection(context, this.Id);
			ItemBase item = DomainManager.Item.GetBaseItem(itemKey);
			int worth = treasury.CalcAdjustedWorth(item.GetItemSubType(), item.GetValue()) * amount;
			item.SetOwner(ItemOwnerType.Treasury, (int)this.Id);
			treasury.Inventory.OfflineAdd(itemKey, amount);
			treasury.OfflineChangeContribution(character, worth);
			bool flag = worth > 0;
			if (flag)
			{
				bool flag2 = isTaiwu;
				if (flag2)
				{
					lifeRecordCollection.AddTaiwuStorageItemToTreasury(charId, currDate, this.Id, itemKey.ItemType, itemKey.TemplateId);
				}
				else
				{
					lifeRecordCollection.AddStorageItemToTreasury(charId, currDate, this.Id, itemKey.ItemType, itemKey.TemplateId, worth);
				}
			}
			bool flag3 = isTaiwu;
			if (flag3)
			{
				settlementTreasuryRecordCollection.AddTaiwuStorageItem(currDate, this.Id, charId, itemKey.ItemType, itemKey.TemplateId);
			}
			else
			{
				settlementTreasuryRecordCollection.AddStorageItem(currDate, this.Id, charId, itemKey.ItemType, itemKey.TemplateId, worth);
			}
			DomainManager.Organization.SetSettlementTreasuryRecordCollection(context, this.Id, settlementTreasuryRecordCollection);
			DomainManager.Extra.SetTreasuries(context, this.Id, treasuries, true);
		}

		// Token: 0x0600480C RID: 18444 RVA: 0x0028BBAC File Offset: 0x00289DAC
		public void TakeItemFromTreasury(DataContext context, GameData.Domains.Character.Character character, ItemKey itemKey, int amount)
		{
			SettlementLayeredTreasuries treasuries = this.Treasuries;
			SettlementTreasury treasury = this.GetTreasury(treasuries, ItemTemplateHelper.GetGrade(itemKey.ItemType, itemKey.TemplateId));
			LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
			int charId = character.GetId();
			bool isTaiwu = charId == DomainManager.Taiwu.GetTaiwuCharId();
			int currDate = DomainManager.World.GetCurrDate();
			SettlementTreasuryRecordCollection settlementTreasuryRecordCollection = DomainManager.Organization.GetSettlementTreasuryRecordCollection(context, this.Id);
			ItemBase item = DomainManager.Item.GetBaseItem(itemKey);
			int worth = treasury.CalcAdjustedWorth(item.GetItemSubType(), item.GetValue()) * amount;
			bool flag = !treasury.Inventory.Items.ContainsKey(itemKey);
			if (flag)
			{
				foreach (SettlementTreasury t in treasuries.SettlementTreasuries)
				{
					bool flag2 = t.Inventory.Items.ContainsKey(itemKey);
					if (flag2)
					{
						treasury = t;
					}
				}
			}
			item.RemoveOwner(ItemOwnerType.Treasury, (int)this.Id);
			treasury.Inventory.OfflineRemove(itemKey, amount);
			treasury.OfflineChangeContribution(character, -worth);
			bool flag3 = worth > 0;
			if (flag3)
			{
				bool flag4 = isTaiwu;
				if (flag4)
				{
					lifeRecordCollection.AddTaiwuTakeItemFromTreasury(charId, currDate, this.Id, itemKey.ItemType, itemKey.TemplateId);
				}
				else
				{
					lifeRecordCollection.AddTakeItemFromTreasury(charId, currDate, this.Id, itemKey.ItemType, itemKey.TemplateId, worth);
				}
			}
			bool flag5 = isTaiwu;
			if (flag5)
			{
				settlementTreasuryRecordCollection.AddTaiwuTakeOutItem(currDate, this.Id, charId, itemKey.ItemType, itemKey.TemplateId);
			}
			else
			{
				settlementTreasuryRecordCollection.AddTakeOutItem(currDate, this.Id, charId, itemKey.ItemType, itemKey.TemplateId, worth);
			}
			DomainManager.Organization.SetSettlementTreasuryRecordCollection(context, this.Id, settlementTreasuryRecordCollection);
			DomainManager.Extra.SetTreasuries(context, this.Id, treasuries, true);
		}

		// Token: 0x0600480D RID: 18445 RVA: 0x0028BD84 File Offset: 0x00289F84
		public void StoreResourceInTreasury(DataContext context, GameData.Domains.Character.Character character, sbyte resourceType, int amount, sbyte layerIndex)
		{
			int worth = DomainManager.Organization.CalcResourceContribution(this.OrgTemplateId, resourceType, amount);
			SettlementLayeredTreasuries treasuries = this.Treasuries;
			SettlementTreasury treasury = (layerIndex >= 0) ? treasuries.GetTreasury(layerIndex) : this.GetTreasury(treasuries, character.GetOrganizationInfo().Grade);
			LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
			int charId = character.GetId();
			bool isTaiwu = charId == DomainManager.Taiwu.GetTaiwuCharId();
			int currDate = DomainManager.World.GetCurrDate();
			SettlementTreasuryRecordCollection settlementTreasuryRecordCollection = DomainManager.Organization.GetSettlementTreasuryRecordCollection(context, this.Id);
			treasury.Resources.Add(resourceType, amount);
			treasury.OfflineChangeContribution(character, worth);
			bool flag = worth > 0;
			if (flag)
			{
				bool flag2 = isTaiwu;
				if (flag2)
				{
					lifeRecordCollection.AddTaiwuStorageResourceToTreasury(charId, currDate, this.Id, resourceType, amount);
				}
				else
				{
					lifeRecordCollection.AddStorageResourceToTreasury(charId, currDate, this.Id, resourceType, amount, worth);
				}
			}
			bool flag3 = isTaiwu;
			if (flag3)
			{
				settlementTreasuryRecordCollection.AddTaiwuStorageResource(currDate, this.Id, charId, resourceType, amount);
			}
			else
			{
				settlementTreasuryRecordCollection.AddStorageResource(currDate, this.Id, charId, resourceType, amount, worth);
			}
			DomainManager.Extra.SetTreasuries(context, this.Id, treasuries, true);
			DomainManager.Organization.SetSettlementTreasuryRecordCollection(context, this.Id, settlementTreasuryRecordCollection);
		}

		// Token: 0x0600480E RID: 18446 RVA: 0x0028BEC8 File Offset: 0x0028A0C8
		public void TakeResourceFromTreasury(DataContext context, GameData.Domains.Character.Character character, sbyte resourceType, int amount, sbyte layerIndex)
		{
			int worth = DomainManager.Organization.CalcResourceContribution(this.OrgTemplateId, resourceType, amount);
			SettlementLayeredTreasuries treasuries = this.Treasuries;
			SettlementTreasury treasury = (layerIndex >= 0) ? treasuries.GetTreasury(layerIndex) : this.GetTreasury(treasuries, character.GetOrganizationInfo().Grade);
			LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
			int charId = character.GetId();
			bool isTaiwu = charId == DomainManager.Taiwu.GetTaiwuCharId();
			int currDate = DomainManager.World.GetCurrDate();
			SettlementTreasuryRecordCollection settlementTreasuryRecordCollection = DomainManager.Organization.GetSettlementTreasuryRecordCollection(context, this.Id);
			treasury.Resources.Subtract(resourceType, amount);
			treasury.OfflineChangeContribution(character, -worth);
			bool flag = worth > 0;
			if (flag)
			{
				bool flag2 = isTaiwu;
				if (flag2)
				{
					lifeRecordCollection.AddTaiwuTakeResourceFromTreasury(charId, currDate, this.Id, resourceType, amount);
				}
				else
				{
					lifeRecordCollection.AddTakeResourceFromTreasury(charId, currDate, this.Id, resourceType, amount, worth);
				}
			}
			bool flag3 = isTaiwu;
			if (flag3)
			{
				settlementTreasuryRecordCollection.AddTaiwuTakeOutResource(currDate, this.Id, charId, resourceType, amount);
			}
			else
			{
				settlementTreasuryRecordCollection.AddTakeOutResource(currDate, this.Id, charId, resourceType, amount, worth);
			}
			DomainManager.Extra.SetTreasuries(context, this.Id, treasuries, true);
			DomainManager.Organization.SetSettlementTreasuryRecordCollection(context, this.Id, settlementTreasuryRecordCollection);
		}

		// Token: 0x0600480F RID: 18447 RVA: 0x0028C00C File Offset: 0x0028A20C
		public static bool IsGuarding(int charId, bool includeNonIntelligentCharacter = false)
		{
			GameData.Domains.Character.Character guard;
			return DomainManager.Character.TryGetElement_Objects(charId, out guard) && ((guard.GetCreatingType() == 1) ? CharacterMatcher.DefValue.InSettlement.Match(guard) : includeNonIntelligentCharacter);
		}

		// Token: 0x06004810 RID: 18448 RVA: 0x0028C044 File Offset: 0x0028A244
		public IEnumerable<int> GetPrisonerRelatedGuards(SettlementPrisoner prisoner)
		{
			return from charId in this.Treasuries.GetTreasury((SettlementTreasuryLayers)Math.Clamp((int)prisoner.GetPrisonType(), 0, 2)).GuardIds.GetCollection()
			where Settlement.IsGuarding(charId, false)
			select charId;
		}

		// Token: 0x06004811 RID: 18449 RVA: 0x0028C098 File Offset: 0x0028A298
		public void RefreshGuards(DataContext context)
		{
			for (sbyte i = 0; i < 3; i += 1)
			{
				foreach (int _ in this.GetGuardsUnsorted(context, i))
				{
				}
			}
		}

		// Token: 0x06004812 RID: 18450 RVA: 0x0028C0F8 File Offset: 0x0028A2F8
		public IEnumerable<int> GetGuardsUnsorted(DataContext context, sbyte treasuryGrade)
		{
			SettlementTreasury treasury = this.Treasuries.GetTreasury(treasuryGrade);
			int count = 0;
			foreach (int guardId in treasury.GuardIds.GetCollection())
			{
				bool flag = Settlement.IsGuarding(guardId, true);
				if (flag)
				{
					yield return guardId;
					int num = count;
					count = num + 1;
				}
			}
			HashSet<int>.Enumerator enumerator = default(HashSet<int>.Enumerator);
			bool modified = false;
			for (;;)
			{
				int num = count;
				count = num + 1;
				if (num >= GlobalConfig.Instance.TreasuryGuardCount)
				{
					break;
				}
				modified = true;
				int charId = this.CreateNonIntelligentGuard(context, treasuryGrade, 0);
				treasury.GuardIds.Add(charId);
				yield return charId;
			}
			bool flag2 = modified;
			if (flag2)
			{
				DomainManager.Extra.SetTreasuries(context, this.Id, this.Treasuries, false);
			}
			yield break;
			yield break;
		}

		// Token: 0x06004813 RID: 18451 RVA: 0x0028C118 File Offset: 0x0028A318
		[return: TupleElementNames(new string[]
		{
			"Character",
			"Favor"
		})]
		public IEnumerable<ValueTuple<GameData.Domains.Character.Character, short>> GetGuardsAndFavors(DataContext context, sbyte treasuryGrade)
		{
			int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
			return (from data in this.GetGuardsUnsorted(context, treasuryGrade).Select(delegate(int charId)
			{
				GameData.Domains.Character.Character character = DomainManager.Character.GetElement_Objects(charId);
				return new ValueTuple<GameData.Domains.Character.Character, short>(character, DomainManager.Character.GetFavorability(charId, taiwuCharId));
			})
			orderby new ValueTuple<bool, int>(data.Item1.GetCreatingType() != 1, -data.Item1.GetCombatPower())
			select data).Take(GlobalConfig.Instance.TreasuryGuardCount);
		}

		// Token: 0x06004814 RID: 18452 RVA: 0x0028C18C File Offset: 0x0028A38C
		public IEnumerable<int> GetGuards(DataContext context, sbyte treasuryGrade)
		{
			return from characterAndFavor in this.GetGuardsAndFavors(context, treasuryGrade)
			select characterAndFavor.Item1.GetId();
		}

		// Token: 0x06004815 RID: 18453 RVA: 0x0028C1BA File Offset: 0x0028A3BA
		public CharacterDisplayData[] GetGuardsDisplayData(DataContext context, sbyte treasuryGrade)
		{
			return this.GetGuardsAndFavors(context, treasuryGrade).Select(delegate([TupleElementNames(new string[]
			{
				"Character",
				"Favor"
			})] ValueTuple<GameData.Domains.Character.Character, short> characterAndFavor)
			{
				CharacterDisplayData ret = DomainManager.Character.GetCharacterDisplayData(characterAndFavor.Item1.GetId());
				ret.FavorabilityToTaiwu = characterAndFavor.Item2;
				return ret;
			}).ToArray<CharacterDisplayData>();
		}

		// Token: 0x06004816 RID: 18454 RVA: 0x0028C1F0 File Offset: 0x0028A3F0
		public int CreateNonIntelligentGuard(DataContext context, sbyte treasuryGrade, sbyte offset = 0)
		{
			int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
			SettlementTreasury settlementTreasury = this.Treasuries.GetTreasury(treasuryGrade);
			sbyte orgTemplateId = this.GetOrgTemplateId();
			sbyte grade = (sbyte)Math.Clamp((int)(((this is Sect) ? GlobalConfig.Instance.SectTreasuryGuardMaxGrade[(int)treasuryGrade] : GlobalConfig.Instance.TreasuryGuardMaxGrade[(int)treasuryGrade]) + offset), 0, 8);
			short templateId = (this is Sect) ? GameData.Domains.Character.Character.GetSectRandomEnemyTemplateIdByGrade(orgTemplateId, grade) : CivilianSettlement.GetTreasuryGuardTemplateId(grade);
			int charId = DomainManager.Character.CreateNonIntelligentCharacter(context, templateId);
			GameData.Domains.Character.Character guard = DomainManager.Character.GetElement_Objects(charId);
			guard.AddFeature(context, this.GetTreasuryGuardFeatureId(settlementTreasury.LayerIndex), false);
			DomainManager.Character.DirectlySetFavorabilities(context, charId, taiwuCharId, DomainManager.Character.CalcInitialFavorability(context.Random, charId, taiwuCharId), DomainManager.Character.CalcInitialFavorability(context.Random, taiwuCharId, charId));
			return charId;
		}

		// Token: 0x06004817 RID: 18455 RVA: 0x0028C2DC File Offset: 0x0028A4DC
		public void ForceUpdateTreasuryGuards(DataContext context)
		{
			SettlementLayeredTreasuries treasuries = this.Treasuries;
			this.OfflineUpdateTreasuryGuards(context, treasuries);
			DomainManager.Extra.SetTreasuries(context, this.Id, treasuries, false);
		}

		// Token: 0x06004818 RID: 18456 RVA: 0x0028C310 File Offset: 0x0028A510
		public void SetAlterTime(DataContext context, byte time)
		{
			SettlementLayeredTreasuries treasuries = this.Treasuries;
			this.Treasuries.AlertTime = time;
			DomainManager.Extra.SetTreasuries(context, this.Id, treasuries, false);
		}

		// Token: 0x06004819 RID: 18457 RVA: 0x0028C348 File Offset: 0x0028A548
		public byte GetAlterTime(DataContext context)
		{
			return this.Treasuries.AlertTime;
		}

		// Token: 0x0600481A RID: 18458 RVA: 0x0028C365 File Offset: 0x0028A565
		private SettlementTreasuryLayers GetLayer(sbyte grade)
		{
			return (SettlementTreasuryLayers)Grade.GetGroup(grade);
		}

		// Token: 0x0600481B RID: 18459 RVA: 0x0028C36D File Offset: 0x0028A56D
		private SettlementTreasury GetTreasury(SettlementLayeredTreasuries treasuries, sbyte grade)
		{
			return treasuries.GetTreasury(this.GetLayer(grade));
		}

		// Token: 0x0600481C RID: 18460 RVA: 0x0028C37C File Offset: 0x0028A57C
		private SettlementTreasury GetTreasury(SettlementLayeredTreasuries treasuries, SettlementTreasuryLayers layer)
		{
			return treasuries.GetTreasury(layer);
		}

		// Token: 0x0600481D RID: 18461 RVA: 0x0028C388 File Offset: 0x0028A588
		private int GetResourceSupplyThreshold(sbyte resourceType, sbyte grade)
		{
			OrganizationMemberItem memberConfig = OrganizationDomain.GetOrgMemberConfig(this.OrgTemplateId, grade);
			return memberConfig.GetAdjustedResourceSatisfyingThreshold(resourceType);
		}

		// Token: 0x0600481E RID: 18462 RVA: 0x0028C3B0 File Offset: 0x0028A5B0
		private bool CanItemBeSupplied(sbyte itemType, short templateId, sbyte targetGrade, out short targetTemplateId)
		{
			targetTemplateId = templateId;
			bool flag = !ItemTemplateHelper.CheckTemplateValid(itemType, templateId);
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				bool flag2 = ItemTemplateHelper.GetItemSubType(itemType, targetTemplateId) == 1204;
				if (flag2)
				{
					result = true;
				}
				else
				{
					short groupId = ItemTemplateHelper.GetGroupId(itemType, templateId);
					bool flag3 = groupId < 0;
					if (flag3)
					{
						result = false;
					}
					else
					{
						for (int i = 0; i <= 8; i++)
						{
							targetTemplateId = (short)((int)groupId + i);
							bool flag4 = !ItemTemplateHelper.CheckTemplateValid(itemType, targetTemplateId);
							if (flag4)
							{
								return false;
							}
							bool flag5 = ItemTemplateHelper.GetGrade(itemType, targetTemplateId) != targetGrade;
							if (!flag5)
							{
								short targetGroupId = ItemTemplateHelper.GetGroupId(itemType, targetTemplateId);
								return targetGroupId >= 0 && targetGroupId == groupId;
							}
						}
						result = false;
					}
				}
			}
			return result;
		}

		// Token: 0x0600481F RID: 18463 RVA: 0x0028C478 File Offset: 0x0028A678
		public Inventory GetSupplyItems()
		{
			Inventory inventory = new Inventory();
			foreach (KeyValuePair<ShortPair, List<short>> keyValuePair in this._supplyItems)
			{
				ShortPair shortPair2;
				List<short> list3;
				keyValuePair.Deconstruct(out shortPair2, out list3);
				ShortPair shortPair = shortPair2;
				List<short> list = list3;
				sbyte itemType = (sbyte)shortPair.First;
				foreach (short templateId in list)
				{
					ItemKey itemKey = new ItemKey(itemType, 0, templateId, 0);
					inventory.OfflineAdd(itemKey, 1);
				}
			}
			foreach (List<short> list2 in this._supplyBooks.Values)
			{
				foreach (short templateId2 in list2)
				{
					ItemKey itemKey2 = new ItemKey(10, 0, templateId2, 0);
					inventory.OfflineAdd(itemKey2, 1);
				}
			}
			return inventory;
		}

		// Token: 0x06004820 RID: 18464 RVA: 0x0028C5E0 File Offset: 0x0028A7E0
		public virtual int GetSupplyLevel()
		{
			return DomainManager.Organization.IsCreatingSettlements() ? 2 : (1 + this.Treasuries.SupplyLevelAddOn);
		}

		// Token: 0x06004821 RID: 18465 RVA: 0x0028C610 File Offset: 0x0028A810
		public virtual short GetTreasuryGuardFeatureId(sbyte layerIndex)
		{
			int offset = (int)(layerIndex + 1);
			return (short)(536 + offset);
		}

		// Token: 0x06004822 RID: 18466
		protected abstract void OfflineUpdateTreasuryGuards(DataContext context, SettlementLayeredTreasuries treasuries);

		// Token: 0x06004823 RID: 18467 RVA: 0x0028C630 File Offset: 0x0028A830
		public short GetId()
		{
			return this.Id;
		}

		// Token: 0x06004824 RID: 18468 RVA: 0x0028C648 File Offset: 0x0028A848
		public sbyte GetOrgTemplateId()
		{
			return this.OrgTemplateId;
		}

		// Token: 0x06004825 RID: 18469 RVA: 0x0028C660 File Offset: 0x0028A860
		public Location GetLocation()
		{
			return this.Location;
		}

		// Token: 0x06004826 RID: 18470 RVA: 0x0028C678 File Offset: 0x0028A878
		public short GetCulture()
		{
			return this.Culture;
		}

		// Token: 0x06004827 RID: 18471
		public abstract void SetCulture(short culture, DataContext context);

		// Token: 0x06004828 RID: 18472 RVA: 0x0028C690 File Offset: 0x0028A890
		public short GetMaxCulture()
		{
			return this.MaxCulture;
		}

		// Token: 0x06004829 RID: 18473
		public abstract void SetMaxCulture(short maxCulture, DataContext context);

		// Token: 0x0600482A RID: 18474 RVA: 0x0028C6A8 File Offset: 0x0028A8A8
		public short GetSafety()
		{
			return this.Safety;
		}

		// Token: 0x0600482B RID: 18475
		public abstract void SetSafety(short safety, DataContext context);

		// Token: 0x0600482C RID: 18476 RVA: 0x0028C6C0 File Offset: 0x0028A8C0
		public short GetMaxSafety()
		{
			return this.MaxSafety;
		}

		// Token: 0x0600482D RID: 18477
		public abstract void SetMaxSafety(short maxSafety, DataContext context);

		// Token: 0x0600482E RID: 18478 RVA: 0x0028C6D8 File Offset: 0x0028A8D8
		public int GetPopulation()
		{
			return this.Population;
		}

		// Token: 0x0600482F RID: 18479
		public abstract void SetPopulation(int population, DataContext context);

		// Token: 0x06004830 RID: 18480 RVA: 0x0028C6F0 File Offset: 0x0028A8F0
		public int GetMaxPopulation()
		{
			return this.MaxPopulation;
		}

		// Token: 0x06004831 RID: 18481
		public abstract void SetMaxPopulation(int maxPopulation, DataContext context);

		// Token: 0x06004832 RID: 18482 RVA: 0x0028C708 File Offset: 0x0028A908
		public int GetStandardOnStagePopulation()
		{
			return this.StandardOnStagePopulation;
		}

		// Token: 0x06004833 RID: 18483
		public abstract void SetStandardOnStagePopulation(int standardOnStagePopulation, DataContext context);

		// Token: 0x06004834 RID: 18484 RVA: 0x0028C720 File Offset: 0x0028A920
		public OrgMemberCollection GetMembers()
		{
			return this.Members;
		}

		// Token: 0x06004835 RID: 18485
		public abstract void SetMembers(OrgMemberCollection members, DataContext context);

		// Token: 0x06004836 RID: 18486 RVA: 0x0028C738 File Offset: 0x0028A938
		public OrgMemberCollection GetLackingCoreMembers()
		{
			return this.LackingCoreMembers;
		}

		// Token: 0x06004837 RID: 18487
		public abstract void SetLackingCoreMembers(OrgMemberCollection lackingCoreMembers, DataContext context);

		// Token: 0x06004838 RID: 18488 RVA: 0x0028C750 File Offset: 0x0028A950
		public short GetApprovingRateUpperLimitBonus()
		{
			return this.ApprovingRateUpperLimitBonus;
		}

		// Token: 0x06004839 RID: 18489
		public abstract void SetApprovingRateUpperLimitBonus(short approvingRateUpperLimitBonus, DataContext context);

		// Token: 0x0600483A RID: 18490 RVA: 0x0028C768 File Offset: 0x0028A968
		public int GetInfluencePowerUpdateDate()
		{
			return this.InfluencePowerUpdateDate;
		}

		// Token: 0x0600483B RID: 18491
		public abstract void SetInfluencePowerUpdateDate(int influencePowerUpdateDate, DataContext context);

		// Token: 0x0600483C RID: 18492
		public abstract short GetApprovingRateUpperLimitTempBonus();

		// Token: 0x0600483D RID: 18493 RVA: 0x0028C780 File Offset: 0x0028A980
		public ValueInfo SelectValue(Evaluator evaluator, string identifier)
		{
			if (!true)
			{
			}
			ValueInfo result;
			if (!(identifier == "MapBlock"))
			{
				if (!(identifier == "ArgBox"))
				{
					result = ValueInfo.Void;
				}
				else
				{
					result = evaluator.PushEvaluationResult(DomainManager.Extra.GetSectMainStoryEventArgBox(this.OrgTemplateId));
				}
			}
			else
			{
				result = evaluator.PushEvaluationResult(DomainManager.Map.GetBlock(this.Location));
			}
			if (!true)
			{
			}
			return result;
		}

		// Token: 0x040014F7 RID: 5367
		[CollectionObjectField(false, true, false, true, false)]
		protected short Id;

		// Token: 0x040014F8 RID: 5368
		[CollectionObjectField(false, true, false, true, false)]
		protected sbyte OrgTemplateId;

		// Token: 0x040014F9 RID: 5369
		[CollectionObjectField(false, true, false, true, false)]
		protected Location Location;

		// Token: 0x040014FA RID: 5370
		[CollectionObjectField(false, true, false, false, false)]
		protected short Culture;

		// Token: 0x040014FB RID: 5371
		[CollectionObjectField(false, true, false, false, false)]
		protected short MaxCulture;

		// Token: 0x040014FC RID: 5372
		[CollectionObjectField(false, true, false, false, false)]
		protected short Safety;

		// Token: 0x040014FD RID: 5373
		[CollectionObjectField(false, true, false, false, false)]
		protected short MaxSafety;

		// Token: 0x040014FE RID: 5374
		[CollectionObjectField(false, true, false, false, false)]
		protected int Population;

		// Token: 0x040014FF RID: 5375
		[CollectionObjectField(false, true, false, false, false)]
		protected int MaxPopulation;

		// Token: 0x04001500 RID: 5376
		[CollectionObjectField(false, true, false, false, false)]
		protected int StandardOnStagePopulation;

		// Token: 0x04001501 RID: 5377
		[CollectionObjectField(false, true, false, false, false)]
		protected OrgMemberCollection Members;

		// Token: 0x04001502 RID: 5378
		[CollectionObjectField(false, true, false, false, false)]
		protected OrgMemberCollection LackingCoreMembers;

		// Token: 0x04001503 RID: 5379
		[CollectionObjectField(false, true, false, false, false)]
		protected short ApprovingRateUpperLimitBonus;

		// Token: 0x04001504 RID: 5380
		[CollectionObjectField(false, true, false, false, false)]
		protected int InfluencePowerUpdateDate;

		// Token: 0x04001505 RID: 5381
		[CollectionObjectField(false, false, true, false, false)]
		protected short ApprovingRateUpperLimitTempBonus;

		// Token: 0x04001506 RID: 5382
		protected SortedList<long, int> _membersSortedByCombatPower = new SortedList<long, int>();

		// Token: 0x04001507 RID: 5383
		private SettlementLayeredTreasuries _treasuries;

		// Token: 0x04001508 RID: 5384
		private readonly Dictionary<ShortPair, List<short>> _supplyItems = new Dictionary<ShortPair, List<short>>();

		// Token: 0x04001509 RID: 5385
		private readonly Dictionary<sbyte, List<short>> _supplyBooks = new Dictionary<sbyte, List<short>>();

		// Token: 0x0400150A RID: 5386
		public readonly bool[] HasTriggeredAllowEntryEvent = new bool[Enum.GetValues(typeof(SettlementTreasuryLayers)).Length];
	}
}
