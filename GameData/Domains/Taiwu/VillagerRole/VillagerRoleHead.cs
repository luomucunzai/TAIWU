using System;
using System.Collections.Generic;
using Config;
using GameData.Common;
using GameData.Domains.Character;
using GameData.Domains.Character.Ai;
using GameData.Domains.Character.Ai.PrioritizedAction;
using GameData.Domains.Character.Relation;
using GameData.Domains.LifeRecord;
using GameData.Domains.Map;
using GameData.Domains.Organization.Display;
using GameData.Domains.Taiwu.Display;
using GameData.Domains.Taiwu.Display.VillagerRoleArrangement;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Taiwu.VillagerRole
{
	// Token: 0x02000050 RID: 80
	[SerializableGameData(IsExtensible = true, NotForDisplayModule = true, NoCopyConstructors = true)]
	public class VillagerRoleHead : VillagerRoleBase, IVillagerRoleArrangementExecutor, IVillagerRoleSelectLocation
	{
		// Token: 0x17000041 RID: 65
		// (get) Token: 0x060013C5 RID: 5061 RVA: 0x0013A677 File Offset: 0x00138877
		public override short RoleTemplateId
		{
			get
			{
				return 6;
			}
		}

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x060013C6 RID: 5062 RVA: 0x0013A67A File Offset: 0x0013887A
		private int AutoActionAffectCount
		{
			get
			{
				return VillagerRoleFormulaImpl.Calculate(11, base.Personality);
			}
		}

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x060013C7 RID: 5063 RVA: 0x0013A689 File Offset: 0x00138889
		private int AutoActionFavorChange
		{
			get
			{
				return VillagerRoleFormulaImpl.Calculate(12, (int)this.Character.GetLifeSkillAttainment(4));
			}
		}

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x060013C8 RID: 5064 RVA: 0x0013A69E File Offset: 0x0013889E
		private int AutoActionFavorIncreaseRate
		{
			get
			{
				return VillagerRoleFormulaImpl.Calculate(13, (int)this.Character.GetBehaviorType());
			}
		}

		// Token: 0x060013C9 RID: 5065 RVA: 0x0013A6B2 File Offset: 0x001388B2
		private int ChickenUpgradeTotalCount(DataContext context)
		{
			return (base.HasChickenUpgradeEffect && context.Random.CheckPercentProb(VillagerRoleFormulaImpl.Calculate(12, base.Personality))) ? 2 : 1;
		}

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x060013CA RID: 5066 RVA: 0x0013A6DA File Offset: 0x001388DA
		public int ArrangementSpecialRuleCount
		{
			get
			{
				return VillagerRoleFormulaImpl.Calculate(15, base.Personality);
			}
		}

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x060013CB RID: 5067 RVA: 0x0013A6E9 File Offset: 0x001388E9
		public int ArrangementSpecialRuleRange
		{
			get
			{
				return VillagerRoleFormulaImpl.Calculate(14, (int)GlobalConfig.Instance.ModifySeverityDefaultRange);
			}
		}

		// Token: 0x060013CC RID: 5068 RVA: 0x0013A6FC File Offset: 0x001388FC
		public override void ExecuteFixedAction(DataContext context)
		{
			bool flag = !base.AutoActionStates[8];
			if (!flag)
			{
				int affectCount = this.AutoActionAffectCount;
				int favorChange = this.AutoActionFavorChange;
				int increaseRate = this.AutoActionFavorIncreaseRate;
				Location location = this.Character.GetLocation();
				List<MapBlockData> blocks = context.AdvanceMonthRelatedData.Blocks.Occupy();
				List<GameData.Domains.Character.Character> memberCharacters = ObjectPool<List<GameData.Domains.Character.Character>>.Instance.Get();
				memberCharacters.Clear();
				DomainManager.Map.GetRealNeighborBlocks(location.AreaId, location.BlockId, blocks, 2, true);
				foreach (MapBlockData block in blocks)
				{
					bool flag2 = block.CharacterSet == null;
					if (!flag2)
					{
						foreach (int charId in block.CharacterSet)
						{
							bool flag3 = this.Character.GetId() == charId;
							if (!flag3)
							{
								GameData.Domains.Character.Character character = DomainManager.Character.GetElement_Objects(charId);
								bool flag4 = character.IsInteractableAsIntelligentCharacter();
								if (flag4)
								{
									memberCharacters.Add(character);
								}
							}
						}
					}
				}
				context.AdvanceMonthRelatedData.Blocks.Release(ref blocks);
				LifeRecordCollection c = DomainManager.LifeRecord.GetLifeRecordCollection();
				int date = DomainManager.World.GetCurrDate();
				for (int i = this.ChickenUpgradeTotalCount(context); i > 0; i--)
				{
					int remainCount = affectCount;
					CollectionUtils.Shuffle<GameData.Domains.Character.Character>(context.Random, memberCharacters);
					foreach (ValueTuple<int, int> valueTuple in RandomUtils.GetRandomUnrepeatedIntPair(context.Random, memberCharacters.Count))
					{
						int idA = valueTuple.Item1;
						int idB = valueTuple.Item2;
						bool flag5 = --remainCount < 0;
						if (flag5)
						{
							break;
						}
						GameData.Domains.Character.Character charA = memberCharacters[idA];
						GameData.Domains.Character.Character charB = memberCharacters[idB];
						bool isIncrease = context.Random.CheckPercentProb(increaseRate);
						bool flag6 = isIncrease;
						if (flag6)
						{
							DomainManager.Character.ChangeFavorabilityOptional(context, charA, charB, favorChange, 5);
							c.AddVillagerFavorabilityUp(this.Character.GetId(), date, charA.GetId(), charB.GetId());
							c.AddVillagerFavorabilityUpPerson(charA.GetId(), date, this.Character.GetId(), charB.GetId());
						}
						else
						{
							DomainManager.Character.ChangeFavorabilityOptional(context, charA, charB, -favorChange, 5);
							c.AddVillagerFavorabilityDown(this.Character.GetId(), date, charA.GetId(), charB.GetId());
							c.AddVillagerFavorabilityDownPerson(charA.GetId(), date, this.Character.GetId(), charB.GetId());
						}
						this.TryCreateRelations(context, charA, charB, isIncrease);
					}
				}
				ObjectPool<List<GameData.Domains.Character.Character>>.Instance.Return(memberCharacters);
			}
		}

		// Token: 0x060013CD RID: 5069 RVA: 0x0013AA58 File Offset: 0x00138C58
		private bool TryCreateRelations(DataContext context, GameData.Domains.Character.Character character, GameData.Domains.Character.Character relatedChar, bool isIncrease)
		{
			return (isIncrease ? this.HandleMakeFriend(context, character, relatedChar) : this.HandleMakeEnemy(context, character, relatedChar)) || this.HandleAdore(context, character, relatedChar) || this.HandleMarriage(context, character, relatedChar) || this.HandleSwornBrotherAndSister(context, character, relatedChar) || this.HandleAdoption(context, character, relatedChar) || this.HandleAdoption(context, relatedChar, character);
		}

		// Token: 0x060013CE RID: 5070 RVA: 0x0013AABC File Offset: 0x00138CBC
		private bool CheckRelationShouldStart(DataContext context, AiRelationsItem relationCfg, GameData.Domains.Character.Character character, GameData.Domains.Character.Character relatedChar, bool useMax = true)
		{
			RelatedCharacter relation = DomainManager.Character.GetRelation(character.GetId(), relatedChar.GetId());
			sbyte sectFavorability = DomainManager.Organization.GetSectFavorability(character.GetOrganizationInfo().OrgTemplateId, relatedChar.GetOrganizationInfo().OrgTemplateId);
			int prob = AiHelper.Relation.GetStartOrEndRelationChance(relationCfg, character, relatedChar, relation.RelationType, sectFavorability, 1);
			relation = DomainManager.Character.GetRelation(relatedChar.GetId(), character.GetId());
			sectFavorability = DomainManager.Organization.GetSectFavorability(relatedChar.GetOrganizationInfo().OrgTemplateId, character.GetOrganizationInfo().OrgTemplateId);
			prob = (useMax ? new Func<int, int, int>(Math.Max) : new Func<int, int, int>(Math.Min))(prob, AiHelper.Relation.GetStartOrEndRelationChance(relationCfg, relatedChar, character, relation.RelationType, sectFavorability, 1));
			return context.Random.CheckPercentProb(prob);
		}

		// Token: 0x060013CF RID: 5071 RVA: 0x0013AB98 File Offset: 0x00138D98
		private bool HandleMakeFriend(DataContext context, GameData.Domains.Character.Character character, GameData.Domains.Character.Character relatedChar)
		{
			int currDate = DomainManager.World.GetCurrDate();
			Location location = this.Character.GetLocation();
			LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
			int villageHeadCharId = this.Character.GetId();
			int charId = character.GetId();
			int relatedCharId = relatedChar.GetId();
			bool flag = !AiHelper.Relation.CanStartRelation(character, relatedChar, 8192) || !AiHelper.Relation.CanStartRelation(relatedChar, character, 8192);
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				bool flag2 = !this.CheckRelationShouldStart(context, AiRelations.DefValue.StartFriendRelation, character, relatedChar, true);
				if (flag2)
				{
					result = false;
				}
				else
				{
					sbyte behaviorType = character.GetBehaviorType();
					GameData.Domains.Character.Character.ApplyBecomeFriend(context, character, relatedChar, behaviorType, true, true);
					lifeRecordCollection.AddVillagerMakeFriends(villageHeadCharId, currDate, charId, relatedCharId, location);
					result = true;
				}
			}
			return result;
		}

		// Token: 0x060013D0 RID: 5072 RVA: 0x0013AC58 File Offset: 0x00138E58
		private bool HandleMakeEnemy(DataContext context, GameData.Domains.Character.Character character, GameData.Domains.Character.Character relatedChar)
		{
			int currDate = DomainManager.World.GetCurrDate();
			Location location = this.Character.GetLocation();
			LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
			int villageHeadCharId = this.Character.GetId();
			int charId = character.GetId();
			int relatedCharId = relatedChar.GetId();
			bool flag = !AiHelper.Relation.CanStartRelation(character, relatedChar, 32768);
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				bool flag2 = !this.CheckRelationShouldStart(context, AiRelations.DefValue.StartEnemyRelation, character, relatedChar, true);
				if (flag2)
				{
					result = false;
				}
				else
				{
					GameData.Domains.Character.Character.ApplyAddRelation_Enemy(context, character, relatedChar, true, 0);
					lifeRecordCollection.AddVillagerMakeEnemy(villageHeadCharId, currDate, charId, relatedCharId, location);
					result = true;
				}
			}
			return result;
		}

		// Token: 0x060013D1 RID: 5073 RVA: 0x0013ACFC File Offset: 0x00138EFC
		private bool HandleAdore(DataContext context, GameData.Domains.Character.Character character, GameData.Domains.Character.Character relatedChar)
		{
			int currDate = DomainManager.World.GetCurrDate();
			Location location = this.Character.GetLocation();
			LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
			int villageHeadCharId = this.Character.GetId();
			sbyte behaviorType = character.GetBehaviorType();
			bool flag = character.GetAgeGroup() != 2;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				bool flag2 = relatedChar.GetAgeGroup() != 2;
				if (flag2)
				{
					result = false;
				}
				else
				{
					int charId = character.GetId();
					int relatedCharId = relatedChar.GetId();
					RelatedCharacter selfToTarget = DomainManager.Character.GetRelation(charId, relatedCharId);
					RelatedCharacter targetToSelf = DomainManager.Character.GetRelation(relatedCharId, charId);
					bool flag3 = !this.CheckRelationShouldStart(context, AiRelations.DefValue.StartAdoredRelation, character, relatedChar, false);
					if (flag3)
					{
						result = false;
					}
					else
					{
						bool selfToTargetHasRelation = RelationType.HasRelation(selfToTarget.RelationType, 16384);
						bool targetToSelfHasRelation = RelationType.HasRelation(targetToSelf.RelationType, 16384);
						bool flag4 = selfToTargetHasRelation && targetToSelfHasRelation;
						if (flag4)
						{
							result = false;
						}
						else
						{
							bool selfToTargetCanStartOrStartedRelation = selfToTargetHasRelation || AiHelper.Relation.CanStartRelation(character, relatedChar, 16384);
							bool targetToSelfCanStartOrStartedRelation = targetToSelfHasRelation || AiHelper.Relation.CanStartRelation(relatedChar, character, 16384);
							bool flag5 = !selfToTargetCanStartOrStartedRelation || !targetToSelfCanStartOrStartedRelation;
							if (flag5)
							{
								result = false;
							}
							else
							{
								int probToCheck = Math.Min(selfToTargetHasRelation ? 100 : AiHelper.Relation.GetStartRelationSuccessRate_Adored(character, relatedChar, selfToTarget, targetToSelf), targetToSelfHasRelation ? 100 : AiHelper.Relation.GetStartRelationSuccessRate_Adored(relatedChar, character, targetToSelf, selfToTarget));
								bool flag6 = !context.Random.CheckPercentProb(probToCheck);
								if (flag6)
								{
									result = false;
								}
								else
								{
									GameData.Domains.Character.Character.ApplyAddRelation_Adore(context, character, relatedChar, behaviorType, true, true, true);
									lifeRecordCollection.AddVillagerConfessLoveSucceed(villageHeadCharId, currDate, charId, relatedCharId, location);
									result = true;
								}
							}
						}
					}
				}
			}
			return result;
		}

		// Token: 0x060013D2 RID: 5074 RVA: 0x0013AEA8 File Offset: 0x001390A8
		private bool HandleMarriage(DataContext context, GameData.Domains.Character.Character character, GameData.Domains.Character.Character relatedChar)
		{
			int currDate = DomainManager.World.GetCurrDate();
			Location location = this.Character.GetLocation();
			LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
			int villageHeadCharId = this.Character.GetId();
			sbyte behaviorType = character.GetBehaviorType();
			bool flag = character.GetAgeGroup() != 2;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				bool flag2 = relatedChar.GetAgeGroup() != 2;
				if (flag2)
				{
					result = false;
				}
				else
				{
					bool flag3 = !character.OrgAndMonkTypeAllowMarriage();
					if (flag3)
					{
						result = false;
					}
					else
					{
						bool flag4 = !relatedChar.OrgAndMonkTypeAllowMarriage();
						if (flag4)
						{
							result = false;
						}
						else
						{
							int charId = character.GetId();
							int relatedCharId = relatedChar.GetId();
							bool flag5 = !AiHelper.Relation.CanStartRelation(character, relatedChar, 1024);
							if (flag5)
							{
								result = false;
							}
							else
							{
								bool flag6 = !this.CheckRelationShouldStart(context, AiRelations.DefValue.StartHusbandOrWifeRelation, character, relatedChar, true);
								if (flag6)
								{
									result = false;
								}
								else
								{
									RelatedCharacter selfToTarget = DomainManager.Character.GetRelation(charId, relatedCharId);
									RelatedCharacter targetToSelf = DomainManager.Character.GetRelation(relatedCharId, charId);
									int probToCheck = Math.Max(AiHelper.Relation.GetStartRelationSuccessRate_HusbandOrWife(character, relatedChar, selfToTarget, targetToSelf), AiHelper.Relation.GetStartRelationSuccessRate_HusbandOrWife(relatedChar, character, targetToSelf, selfToTarget));
									bool flag7 = !context.Random.CheckPercentProb(probToCheck);
									if (flag7)
									{
										result = false;
									}
									else
									{
										GameData.Domains.Character.Character.ApplyBecomeHusbandOrWife(context, character, relatedChar, behaviorType, true, true, true);
										lifeRecordCollection.AddVillagerGetMarried(villageHeadCharId, currDate, charId, relatedCharId, location);
										result = true;
									}
								}
							}
						}
					}
				}
			}
			return result;
		}

		// Token: 0x060013D3 RID: 5075 RVA: 0x0013B014 File Offset: 0x00139214
		private bool HandleSwornBrotherAndSister(DataContext context, GameData.Domains.Character.Character character, GameData.Domains.Character.Character relatedChar)
		{
			int currDate = DomainManager.World.GetCurrDate();
			Location location = this.Character.GetLocation();
			LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
			int villageHeadCharId = this.Character.GetId();
			int charId = character.GetId();
			int relatedCharId = relatedChar.GetId();
			bool flag = !AiHelper.Relation.CanStartRelation(character, relatedChar, 512);
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				bool flag2 = !this.CheckRelationShouldStart(context, AiRelations.DefValue.StartSwornBrotherOrSisterRelation, character, relatedChar, true);
				if (flag2)
				{
					result = false;
				}
				else
				{
					RelatedCharacter selfToTarget = DomainManager.Character.GetRelation(charId, relatedCharId);
					RelatedCharacter targetToSelf = DomainManager.Character.GetRelation(relatedCharId, charId);
					bool selfToTargetHasRelation = RelationType.HasRelation(selfToTarget.RelationType, 16384);
					bool targetToSelfHasRelation = RelationType.HasRelation(targetToSelf.RelationType, 16384);
					bool flag3 = selfToTargetHasRelation || targetToSelfHasRelation;
					if (flag3)
					{
						result = false;
					}
					else
					{
						int probToCheck = Math.Max(AiHelper.Relation.GetStartRelationSuccessRate_HusbandOrWife(character, relatedChar, selfToTarget, targetToSelf), AiHelper.Relation.GetStartRelationSuccessRate_HusbandOrWife(relatedChar, character, targetToSelf, selfToTarget));
						bool flag4 = !context.Random.CheckPercentProb(probToCheck);
						if (flag4)
						{
							result = false;
						}
						else
						{
							sbyte behaviorType = character.GetBehaviorType();
							GameData.Domains.Character.Character.ApplyBecomeSwornBrotherOrSister(context, character, relatedChar, behaviorType, true, true);
							lifeRecordCollection.AddVillagerBecomeBrothers(villageHeadCharId, currDate, charId, relatedCharId, location);
							result = true;
						}
					}
				}
			}
			return result;
		}

		// Token: 0x060013D4 RID: 5076 RVA: 0x0013B158 File Offset: 0x00139358
		private bool HandleAdoption(DataContext context, GameData.Domains.Character.Character character, GameData.Domains.Character.Character relatedChar)
		{
			int currDate = DomainManager.World.GetCurrDate();
			Location location = this.Character.GetLocation();
			LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
			int villageHeadCharId = this.Character.GetId();
			int charId = character.GetId();
			int relatedCharId = relatedChar.GetId();
			bool flag = !AiHelper.Relation.CanStartRelation(character, relatedChar, 128) && !AiHelper.Relation.CanStartRelation(relatedChar, character, 128);
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				bool flag2 = !this.CheckRelationShouldStart(context, AiRelations.DefValue.AdoptingRelation, character, relatedChar, true);
				if (flag2)
				{
					result = false;
				}
				else
				{
					sbyte behaviorType = character.GetBehaviorType();
					GameData.Domains.Character.Character.ApplyAddRelation_AdoptiveChild(context, character, relatedChar, behaviorType, true, true);
					lifeRecordCollection.AddVillagerAdopt(villageHeadCharId, currDate, charId, relatedCharId, location);
					result = true;
				}
			}
			return result;
		}

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x060013D5 RID: 5077 RVA: 0x0013B216 File Offset: 0x00139416
		[Obsolete]
		public int VillagerFavorabilityChange
		{
			get
			{
				return (int)this.Character.GetPersonality(6) * 300;
			}
		}

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x060013D6 RID: 5078 RVA: 0x0013B22A File Offset: 0x0013942A
		[Obsolete]
		public int PersonalityBonusPercent
		{
			get
			{
				return GameData.Domains.Taiwu.VillagerRole.SharedMethods.CalculateVillageHeadPersonalityBonusPercent(this.Character.GetPersonalities());
			}
		}

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x060013D7 RID: 5079 RVA: 0x0013B23C File Offset: 0x0013943C
		[Obsolete]
		public int AttainmentBonusPercent
		{
			get
			{
				return GameData.Domains.Taiwu.VillagerRole.SharedMethods.CalculateVillageHeadAttainmentBonusPercent(this.Character.GetPersonalities());
			}
		}

		// Token: 0x060013D8 RID: 5080 RVA: 0x0013B250 File Offset: 0x00139450
		void IVillagerRoleArrangementExecutor.ExecuteArrangementAction(DataContext context, VillagerRoleArrangementAction action)
		{
			short key;
			bool flag = !this.TryGetWorkingStateCustomizeKey(out key);
			if (!flag)
			{
				GameData.Domains.Character.Character taiwuChar = DomainManager.Taiwu.GetTaiwu();
				bool removedAny;
				int authorityCost = this.GetAuthorityCost(true, out removedAny);
				taiwuChar.ChangeResource(context, 7, -authorityCost);
			}
		}

		// Token: 0x060013D9 RID: 5081 RVA: 0x0013B294 File Offset: 0x00139494
		public int GetAuthorityCost(bool removeExceeded, out bool removedAny)
		{
			removedAny = false;
			short key;
			bool flag = !this.TryGetWorkingStateCustomizeKey(out key);
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				ValueTuple<sbyte, bool> valueTuple = PunishmentSeverityCustomizeData.DecodePunishmentSeverityCustomizeKey(key);
				sbyte stateTemplateId = valueTuple.Item1;
				bool isSect = valueTuple.Item2;
				GameData.Domains.Character.Character taiwuChar = DomainManager.Taiwu.GetTaiwu();
				int totalAuthority = taiwuChar.GetResource(7);
				int authorityCost = 0;
				SerializableList<PunishmentSeverityCustomizeData> punishmentSeverityCustomize;
				bool flag2;
				if (DomainManager.Extra.TryGetElement_CityPunishmentSeverityCustomizeDict(key, out punishmentSeverityCustomize))
				{
					List<PunishmentSeverityCustomizeData> items = punishmentSeverityCustomize.Items;
					flag2 = (items != null && items.Count > 0);
				}
				else
				{
					flag2 = false;
				}
				bool flag3 = flag2;
				if (flag3)
				{
					for (int index = punishmentSeverityCustomize.Items.Count - 1; index >= 0; index--)
					{
						PunishmentSeverityCustomizeData customSeverityData = punishmentSeverityCustomize.Items[index];
						PunishmentTypeItem punishmentType = PunishmentType.Instance[customSeverityData.PunishmentTypeTemplateId];
						sbyte originalSeverity = punishmentType.GetSeverity(stateTemplateId, isSect, false);
						int diff = Math.Abs((int)(originalSeverity - customSeverityData.CustomizedPunishmentSeverityTemplateId));
						bool flag4 = diff <= (int)GlobalConfig.Instance.ModifySeverityDefaultRange;
						if (!flag4)
						{
							int baseCost = (int)(originalSeverity + 1) * GlobalConfig.Instance.ModifySeverityCostFactor;
							int currCost = this.CalcModificationAuthorityCost(baseCost);
							bool flag5 = removeExceeded && currCost + authorityCost > totalAuthority;
							if (flag5)
							{
								punishmentSeverityCustomize.Items.RemoveAt(index);
								removedAny = true;
							}
							else
							{
								authorityCost += currCost;
							}
						}
					}
				}
				result = authorityCost;
			}
			return result;
		}

		// Token: 0x060013DA RID: 5082 RVA: 0x0013B3FC File Offset: 0x001395FC
		public bool TryGetWorkingStateCustomizeKey(out short key)
		{
			key = 0;
			bool flag = this.WorkData == null || this.WorkData.AreaId < 0;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				short areaTemplateId = DomainManager.Map.GetElement_Areas((int)this.WorkData.AreaId).GetTemplateId();
				sbyte stateTemplateId = DomainManager.Map.GetStateTemplateIdByAreaId(this.WorkData.AreaId);
				MapStateItem stateConfig = MapState.Instance[stateTemplateId];
				bool flag2 = (short)stateConfig.MainAreaID == areaTemplateId;
				if (flag2)
				{
					key = PunishmentSeverityCustomizeData.GetPunishmentSeverityCustomizeKey(stateTemplateId, false);
					result = true;
				}
				else
				{
					bool flag3 = (short)stateConfig.SectAreaID == areaTemplateId;
					if (flag3)
					{
						key = PunishmentSeverityCustomizeData.GetPunishmentSeverityCustomizeKey(stateTemplateId, true);
						result = true;
					}
					else
					{
						result = false;
					}
				}
			}
			return result;
		}

		// Token: 0x060013DB RID: 5083 RVA: 0x0013B4AF File Offset: 0x001396AF
		public int CalcModificationAuthorityCost(int baseCost)
		{
			return Math.Max(0, VillagerRoleFormulaImpl.Calculate(16, baseCost, (int)this.Character.GetLifeSkillAttainment(4)));
		}

		// Token: 0x060013DC RID: 5084 RVA: 0x0013B4CC File Offset: 0x001396CC
		public override IVillagerRoleArrangementDisplayData GetArrangementDisplayData()
		{
			return new TaiwuEnvoyDisplayData
			{
				SpecialRuleCount = this.ArrangementSpecialRuleCount,
				MonthlyAuthorityCost = this.CalcModificationAuthorityCost(100)
			};
		}
	}
}
