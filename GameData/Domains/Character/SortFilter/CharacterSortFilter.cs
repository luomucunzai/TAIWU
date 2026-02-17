using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Config;
using GameData.Domains.Character.Relation;
using GameData.Domains.Map;
using GameData.Domains.Taiwu.Profession;
using GameData.Domains.World.SectMainStory;
using GameData.Utilities;

namespace GameData.Domains.Character.SortFilter
{
	// Token: 0x0200081E RID: 2078
	public class CharacterSortFilter
	{
		// Token: 0x17000488 RID: 1160
		// (get) Token: 0x0600750E RID: 29966 RVA: 0x00447A1D File Offset: 0x00445C1D
		public CharacterSortFilterSettings Settings
		{
			get
			{
				return this._settings;
			}
		}

		// Token: 0x0600750F RID: 29967 RVA: 0x00447A28 File Offset: 0x00445C28
		public CharacterSortFilter(CharacterSortFilterSettings settings, string language)
		{
			this._characters = new List<CharacterSortingInfo>();
			this._addedCharIdSet = new HashSet<int>();
			bool flag = settings.FilterSubType >= 0;
			if (flag)
			{
				this._filteredCharacters = new List<CharacterSortingInfo>[CharacterFilterSubType.SubTypeToFilterCount[(int)settings.FilterSubType]];
				for (int i = 0; i < this._filteredCharacters.Length; i++)
				{
					this._filteredCharacters[i] = new List<CharacterSortingInfo>();
				}
			}
			this._currEncoding = CharacterSortFilter.LanguageEncodingDict[language];
			this._settings = settings;
			this.FindByFilter();
		}

		// Token: 0x06007510 RID: 29968 RVA: 0x00447AC1 File Offset: 0x00445CC1
		public List<int> GetCounts()
		{
			return this._characterCounts;
		}

		// Token: 0x06007511 RID: 29969 RVA: 0x00447ACC File Offset: 0x00445CCC
		public void UpdateCounts()
		{
			if (this._characterCounts == null)
			{
				this._characterCounts = new List<int>();
			}
			this._characterCounts.Clear();
			this._characterCounts.Add(this._characters.Count);
			bool flag = this._filteredCharacters != null;
			if (flag)
			{
				for (int i = 0; i < this._filteredCharacters.Length; i++)
				{
					this._characterCounts.Add(this._filteredCharacters[i].Count);
				}
			}
		}

		// Token: 0x06007512 RID: 29970 RVA: 0x00447B50 File Offset: 0x00445D50
		public List<int> GetMaxSortingTypeCharIds(List<int> sortingTypes, sbyte filterSubId)
		{
			if (this._characterCounts == null)
			{
				this._characterCounts = new List<int>();
			}
			this._characterCounts.Clear();
			for (int index = 0; index < sortingTypes.Count; index++)
			{
				this._characterCounts.Add(-1);
			}
			List<CharacterSortingInfo> characters = (filterSubId < 0) ? this._characters : this._filteredCharacters[(int)filterSubId];
			bool flag = characters.Count == 0;
			List<int> characterCounts;
			if (flag)
			{
				characterCounts = this._characterCounts;
			}
			else
			{
				Parallel.For(0, sortingTypes.Count, delegate(int i)
				{
					int sortingType = sortingTypes[i];
					CharacterSortingInfo result = characters.Max((CharacterSortingInfo a, CharacterSortingInfo b) => CharacterSortFilter.CompareValue(a, b, sortingType));
					this._characterCounts[i] = result.Character.GetId();
				});
				characterCounts = this._characterCounts;
			}
			return characterCounts;
		}

		// Token: 0x06007513 RID: 29971 RVA: 0x00447C18 File Offset: 0x00445E18
		public CharacterList FindByName(string name)
		{
			CharacterList charList = default(CharacterList);
			List<CharacterSortingInfo> characters = (this._settings.FilterSubId < 0) ? this._characters : this._filteredCharacters[this._settings.FilterSubId];
			foreach (CharacterSortingInfo charInfo in characters)
			{
				string str = this._currEncoding.GetString(charInfo.NameBytes);
				bool flag = str.Contains(name);
				if (flag)
				{
					charList.Add(charInfo.Character.GetId());
				}
			}
			return charList;
		}

		// Token: 0x06007514 RID: 29972 RVA: 0x00447CD0 File Offset: 0x00445ED0
		public CharacterList GetCharacterList(int filterIndex)
		{
			CharacterList characterList = default(CharacterList);
			bool flag = filterIndex < 0;
			if (flag)
			{
				foreach (CharacterSortingInfo charInfo in this._characters)
				{
					characterList.Add(charInfo.Character.GetId());
				}
			}
			else
			{
				List<CharacterSortingInfo> characters = this._filteredCharacters[filterIndex];
				foreach (CharacterSortingInfo charInfo2 in characters)
				{
					characterList.Add(charInfo2.Character.GetId());
				}
			}
			return characterList;
		}

		// Token: 0x06007515 RID: 29973 RVA: 0x00447DA8 File Offset: 0x00445FA8
		public void Sort(CharacterSortFilterSettings sortFilterSettings)
		{
			bool flag = this._settings.FilterType != sortFilterSettings.FilterType;
			if (flag)
			{
				throw new Exception("Cannot change filter type when the current sort filter is already initialized.");
			}
			this._settings = sortFilterSettings;
			bool flag2 = sortFilterSettings.FilterSubId < 0;
			if (flag2)
			{
				this._characters.Sort(new Comparison<CharacterSortingInfo>(this.Compare));
			}
			else
			{
				this._filteredCharacters[sortFilterSettings.FilterSubId].Sort(new Comparison<CharacterSortingInfo>(this.Compare));
			}
		}

		// Token: 0x06007516 RID: 29974 RVA: 0x00447E28 File Offset: 0x00446028
		public bool CheckFilterSettingsMatch(CharacterSortFilterSettings settings)
		{
			bool flag = settings.FilterType != this._settings.FilterType;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				switch (settings.FilterType)
				{
				case 1:
					return settings.TargetLocation.Equals(this._settings.TargetLocation);
				case 2:
					return settings.TargetCharId == this._settings.TargetCharId;
				case 4:
					return settings.VillagerNeededItem.Equals(this._settings.VillagerNeededItem);
				}
				result = true;
			}
			return result;
		}

		// Token: 0x06007517 RID: 29975 RVA: 0x00447EC4 File Offset: 0x004460C4
		public void FindByFilter()
		{
			this._characters.Clear();
			this._addedCharIdSet.Clear();
			bool canProduceDuplicate = false;
			bool flag = this._settings.FilterType == 1;
			if (flag)
			{
				DomainManager.Adventure.GetCharactersInAdventure(this._settings.TargetLocation, this._addedCharIdSet);
				foreach (int charId in this._addedCharIdSet)
				{
					Character character = DomainManager.Character.GetElement_Objects(charId);
					this._characters.Add(new CharacterSortingInfo(character, this._currEncoding, null));
				}
			}
			else
			{
				switch (this._settings.FilterSubType)
				{
				case 0:
					Parallel.For(0, this._filteredCharacters.Length, new Action<int>(this.FindInState));
					break;
				case 1:
					Parallel.For(0, RelationDisplayType.Instance.Count, new Action<int>(this.FindInRelationDisplayType));
					canProduceDuplicate = true;
					break;
				case 2:
					Parallel.For(0, this._filteredCharacters.Length, new Action<int>(this.FindInVillagerRole));
					break;
				}
			}
			bool flag2 = this._filteredCharacters != null;
			if (flag2)
			{
				bool flag3 = canProduceDuplicate;
				if (flag3)
				{
					foreach (List<CharacterSortingInfo> filteredCharacters in this._filteredCharacters)
					{
						for (int i = 0; i < filteredCharacters.Count; i++)
						{
							CharacterSortingInfo filteredChar = filteredCharacters[i];
							int charId2 = filteredChar.GetId();
							bool flag4 = this._addedCharIdSet.Add(charId2);
							if (flag4)
							{
								this._characters.Add(filteredChar);
							}
						}
					}
				}
				else
				{
					foreach (List<CharacterSortingInfo> filteredCharacters2 in this._filteredCharacters)
					{
						this._characters.AddRange(filteredCharacters2);
					}
				}
			}
			this._characters.Sort(new Comparison<CharacterSortingInfo>(this.Compare));
			this.UpdateCounts();
		}

		// Token: 0x06007518 RID: 29976 RVA: 0x004480F4 File Offset: 0x004462F4
		private sbyte FilterSubIdToStateId(int filterSubId)
		{
			return (sbyte)(filterSubId - 1);
		}

		// Token: 0x06007519 RID: 29977 RVA: 0x0044810C File Offset: 0x0044630C
		private unsafe void FindInState(int filterSubId)
		{
			List<CharacterSortingInfo> resultCollection = this._filteredCharacters[filterSubId];
			bool flag = filterSubId == 0;
			if (flag)
			{
				HashSet<int> taiwuGroup = DomainManager.Taiwu.GetGroupCharIds().GetCollection();
				foreach (int charId in taiwuGroup)
				{
					Character character = DomainManager.Character.GetElement_Objects(charId);
					bool flag2 = !CharacterSortFilter.Match(character, (int)this._settings.FilterType);
					if (!flag2)
					{
						resultCollection.Add(new CharacterSortingInfo(character, this._currEncoding, null));
					}
				}
				resultCollection.Sort(new Comparison<CharacterSortingInfo>(this.Compare));
			}
			else
			{
				sbyte stateId = this.FilterSubIdToStateId(filterSubId);
				List<short> areaIds = ObjectPool<List<short>>.Instance.Get();
				DomainManager.Map.GetAllAreaInState(stateId, areaIds);
				bool includeInfected = false;
				foreach (short areaId in areaIds)
				{
					Span<MapBlockData> areaBlocks = DomainManager.Map.GetAreaBlocks(areaId);
					Span<MapBlockData> span = areaBlocks;
					for (int i = 0; i < span.Length; i++)
					{
						MapBlockData blockData = *span[i];
						bool flag3 = blockData.CharacterSet == null;
						if (!flag3)
						{
							foreach (int charId2 in blockData.CharacterSet)
							{
								Character character2 = DomainManager.Character.GetElement_Objects(charId2);
								bool flag4 = !CharacterSortFilter.Match(character2, (int)this._settings.FilterType);
								if (!flag4)
								{
									resultCollection.Add(new CharacterSortingInfo(character2, this._currEncoding, null));
								}
							}
							bool flag5 = includeInfected && blockData.InfectedCharacterSet != null;
							if (flag5)
							{
								foreach (int charId3 in blockData.InfectedCharacterSet)
								{
									Character character3 = DomainManager.Character.GetElement_Objects(charId3);
									bool flag6 = CharacterSortFilter.Match(character3, (int)this._settings.FilterType);
									if (flag6)
									{
										resultCollection.Add(new CharacterSortingInfo(character3, this._currEncoding, null));
									}
								}
							}
						}
					}
				}
				resultCollection.Sort(new Comparison<CharacterSortingInfo>(this.Compare));
			}
		}

		// Token: 0x0600751A RID: 29978 RVA: 0x004483F0 File Offset: 0x004465F0
		private void FindInRelationDisplayType(int filterSubId)
		{
			RelationDisplayTypeItem displayTypeCfg = RelationDisplayType.Instance[filterSubId];
			RelatedCharacters relatedCharacters = DomainManager.Character.GetRelatedCharacters(this._settings.TargetCharId);
			bool flag = relatedCharacters == null;
			if (!flag)
			{
				bool flag2 = !this._filteredCharacters.CheckIndex(filterSubId);
				if (!flag2)
				{
					List<CharacterSortingInfo> resultCollection = this._filteredCharacters[filterSubId];
					bool flag3 = displayTypeCfg.RelationTypeIds != null;
					if (flag3)
					{
						foreach (sbyte relationTypeId in displayTypeCfg.RelationTypeIds)
						{
							ushort relationType = RelationType.GetRelationType(relationTypeId);
							HashSet<int> charSet = relatedCharacters.GetCharacterSet(relationType).GetCollection();
							foreach (int charId in charSet)
							{
								Character relatedChar;
								bool flag4 = !DomainManager.Character.TryGetElement_Objects(charId, out relatedChar);
								if (!flag4)
								{
									bool flag5 = !CharacterSortFilter.Match(relatedChar, (int)this._settings.FilterType);
									if (!flag5)
									{
										resultCollection.Add(new CharacterSortingInfo(relatedChar, this._currEncoding, null));
									}
								}
							}
							bool flag6 = RelationType.IsOneWayRelation(relationType);
							if (flag6)
							{
								HashSet<int> currCharIds = ObjectPool<HashSet<int>>.Instance.Get();
								currCharIds.Clear();
								relatedCharacters.GetAllRelatedCharIds(currCharIds, true);
								foreach (int charId2 in currCharIds)
								{
									bool flag7 = !DomainManager.Character.HasRelation(charId2, this._settings.TargetCharId, relationType);
									if (!flag7)
									{
										Character relatedChar2;
										bool flag8 = !DomainManager.Character.TryGetElement_Objects(charId2, out relatedChar2);
										if (!flag8)
										{
											bool flag9 = !CharacterSortFilter.Match(relatedChar2, (int)this._settings.FilterType);
											if (!flag9)
											{
												bool flag10 = charSet.Contains(charId2);
												if (!flag10)
												{
													resultCollection.Add(new CharacterSortingInfo(relatedChar2, this._currEncoding, null));
												}
											}
										}
									}
								}
								ObjectPool<HashSet<int>>.Instance.Return(currCharIds);
							}
						}
					}
					else
					{
						bool flag11 = displayTypeCfg.TemplateId == 4;
						if (flag11)
						{
							Character character = DomainManager.Character.GetElement_Objects(this._settings.TargetCharId);
							int factionId = character.GetFactionId();
							bool flag12 = factionId >= 0;
							if (flag12)
							{
								HashSet<int> faction = DomainManager.Organization.GetElement_Factions(factionId).GetCollection();
								foreach (int charId3 in faction)
								{
									Character relatedChar3;
									bool flag13 = !DomainManager.Character.TryGetElement_Objects(charId3, out relatedChar3);
									if (!flag13)
									{
										resultCollection.Add(new CharacterSortingInfo(relatedChar3, this._currEncoding, null));
									}
								}
							}
						}
					}
					resultCollection.Sort(new Comparison<CharacterSortingInfo>(this.Compare));
				}
			}
		}

		// Token: 0x0600751B RID: 29979 RVA: 0x00448718 File Offset: 0x00446918
		private void FindInVillagerRole(int filterSubId)
		{
			List<CharacterSortingInfo> resultCollection = this._filteredCharacters[filterSubId];
			Dictionary<int, sbyte> charDict = DomainManager.Taiwu.GetTreasuryItemNeededCharDict(this._settings.VillagerNeededItem);
			bool flag = filterSubId == 0;
			if (flag)
			{
				foreach (int charId in charDict.Keys)
				{
					Character character;
					bool flag2 = !DomainManager.Character.TryGetElement_Objects(charId, out character);
					if (!flag2)
					{
						bool flag3 = !CharacterSortFilter.Match(character, (int)this._settings.FilterType);
						if (!flag3)
						{
							bool noRole = character.GetOrganizationInfo().Grade == 0;
							bool flag4 = !noRole;
							if (!flag4)
							{
								resultCollection.Add(new CharacterSortingInfo(character, this._currEncoding, this));
							}
						}
					}
				}
			}
			else
			{
				int roleTemplateId = filterSubId - 1;
				IReadOnlySet<int> roleSet = DomainManager.Taiwu.GetVillagerRoleSet((short)roleTemplateId);
				foreach (int charId2 in charDict.Keys)
				{
					bool flag5 = !roleSet.Contains(charId2);
					if (!flag5)
					{
						Character character2;
						bool flag6 = !DomainManager.Character.TryGetElement_Objects(charId2, out character2);
						if (!flag6)
						{
							bool flag7 = !CharacterSortFilter.Match(character2, (int)this._settings.FilterType);
							if (!flag7)
							{
								resultCollection.Add(new CharacterSortingInfo(character2, this._currEncoding, this));
							}
						}
					}
				}
			}
			resultCollection.Sort(new Comparison<CharacterSortingInfo>(this.Compare));
		}

		// Token: 0x0600751C RID: 29980 RVA: 0x004488D0 File Offset: 0x00446AD0
		private static bool Match(Character character, int filterType)
		{
			if (!true)
			{
			}
			bool result;
			if (filterType != 0)
			{
				result = (filterType != 3 || CharacterSortFilter.MatchCanLinkInLifeDeathGate(character));
			}
			else
			{
				result = CharacterSortFilter.MatchDirectedSamsaraMother(character);
			}
			if (!true)
			{
			}
			return result;
		}

		// Token: 0x0600751D RID: 29981 RVA: 0x0044890C File Offset: 0x00446B0C
		private static bool MatchDirectedSamsaraMother(Character character)
		{
			bool flag = character.GetGender() != 0;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				bool flag2 = character.GetAgeGroup() != 2;
				if (flag2)
				{
					result = false;
				}
				else
				{
					bool flag3 = character.GetLegendaryBookOwnerState() > 1;
					if (flag3)
					{
						result = false;
					}
					else
					{
						bool flag4 = character.GetFertility() <= 0;
						if (flag4)
						{
							result = false;
						}
						else
						{
							int charId = character.GetId();
							bool flag5 = charId == DomainManager.Taiwu.GetTaiwuCharId();
							if (flag5)
							{
								result = false;
							}
							else
							{
								bool flag6 = ProfessionSkillHandle.BuddhistMonkSkill_IsDirectedSamsaraMother(charId);
								result = !flag6;
							}
						}
					}
				}
			}
			return result;
		}

		// Token: 0x0600751E RID: 29982 RVA: 0x0044899C File Offset: 0x00446B9C
		private static bool MatchCanLinkInLifeDeathGate(Character character)
		{
			int charId = character.GetId();
			int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
			RelatedCharacter relatedCharacter;
			bool flag = !DomainManager.Character.TryGetRelation(charId, taiwuCharId, out relatedCharacter) && charId != taiwuCharId;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				bool flag2 = character.IsCompletelyInfected();
				if (flag2)
				{
					result = false;
				}
				else
				{
					bool flag3 = character.GetLegendaryBookOwnerState() >= 2;
					if (flag3)
					{
						result = false;
					}
					else
					{
						bool flag4 = character.GetAgeGroup() != 2;
						if (flag4)
						{
							result = false;
						}
						else
						{
							SectBaihuaLifeLinkData lifeDeathGate = DomainManager.Extra.GetSectBaihuaLifeLinkData();
							bool flag5 = lifeDeathGate.LifeGateCharIds.IndexOf(charId) >= 0;
							if (flag5)
							{
								result = false;
							}
							else
							{
								bool flag6 = lifeDeathGate.DeathGateCharIds.IndexOf(charId) >= 0;
								result = !flag6;
							}
						}
					}
				}
			}
			return result;
		}

		// Token: 0x0600751F RID: 29983 RVA: 0x00448A70 File Offset: 0x00446C70
		private int Compare(CharacterSortingInfo charA, CharacterSortingInfo charB)
		{
			for (int i = 0; i < this._settings.SortOrder.Count - 1; i++)
			{
				ValueTuple<int, bool> order = this._settings.SortOrder[i];
				int result = order.Item2 ? CharacterSortFilter.Compare(charB, charA, order.Item1) : CharacterSortFilter.Compare(charA, charB, order.Item1);
				bool flag = result != 0;
				if (flag)
				{
					return result;
				}
			}
			bool flag2 = this._settings.SortOrder.Count > 0;
			if (flag2)
			{
				List<ValueTuple<int, bool>> sortOrder = this._settings.SortOrder;
				ValueTuple<int, bool> order2 = sortOrder[sortOrder.Count - 1];
				int result2 = order2.Item2 ? CharacterSortFilter.CompareValue(charB, charA, order2.Item1) : CharacterSortFilter.CompareValue(charA, charB, order2.Item1);
				bool flag3 = result2 != 0;
				if (flag3)
				{
					return result2;
				}
			}
			bool flag4 = this._settings.FilterType == 4 && charA.VillagerNeedWaitTime != charB.VillagerNeedWaitTime;
			if (flag4)
			{
				return charA.VillagerNeedWaitTime.CompareTo(charB.VillagerNeedWaitTime);
			}
			return charA.Character.GetId().CompareTo(charB.Character.GetId());
		}

		// Token: 0x06007520 RID: 29984 RVA: 0x00448BBC File Offset: 0x00446DBC
		private static int CompareValue(CharacterSortingInfo charA, CharacterSortingInfo charB, int sortingType)
		{
			if (!true)
			{
			}
			int result;
			switch (sortingType)
			{
			case 5:
				result = CharacterSortFilter.CompareMorality(charA, charB);
				break;
			case 6:
				result = CharacterSortFilter.CompareHappiness(charA, charB);
				break;
			case 7:
				result = CharacterSortFilter.CompareFavorability(charA, charB);
				break;
			case 8:
				result = CharacterSortFilter.CompareFame(charA, charB);
				break;
			default:
				if (sortingType != 201)
				{
					result = CharacterSortFilter.Compare(charA, charB, sortingType);
				}
				else
				{
					result = CharacterSortFilter.CompareAttraction(charA, charB);
				}
				break;
			}
			if (!true)
			{
			}
			return result;
		}

		// Token: 0x06007521 RID: 29985 RVA: 0x00448C38 File Offset: 0x00446E38
		private static int Compare(CharacterSortingInfo charA, CharacterSortingInfo charB, int sortingType)
		{
			if (!true)
			{
			}
			int result;
			if (sortingType <= 33)
			{
				if (sortingType >= 26)
				{
					result = CharacterSortFilter.CompareResource(charA, charB, (sbyte)(sortingType - 26));
					goto IL_1C4;
				}
				switch (sortingType)
				{
				case 0:
					result = CharacterSortFilter.CompareName(charA, charB);
					goto IL_1C4;
				case 1:
					result = CharacterSortFilter.CompareGrade(charA, charB);
					goto IL_1C4;
				case 2:
					result = CharacterSortFilter.CompareAge(charA, charB);
					goto IL_1C4;
				case 3:
					result = CharacterSortFilter.CompareHealth(charA, charB);
					goto IL_1C4;
				case 4:
					result = CharacterSortFilter.CompareGender(charA, charB);
					goto IL_1C4;
				case 5:
					result = CharacterSortFilter.CompareBehaviorType(charA, charB);
					goto IL_1C4;
				case 6:
					result = CharacterSortFilter.CompareHappinessType(charA, charB);
					goto IL_1C4;
				case 7:
					result = CharacterSortFilter.CompareFavorType(charA, charB);
					goto IL_1C4;
				case 8:
					result = CharacterSortFilter.CompareFameType(charA, charB);
					goto IL_1C4;
				case 9:
					result = CharacterSortFilter.CompareLivingStatus(charA, charB);
					goto IL_1C4;
				case 10:
					result = CharacterSortFilter.CompareWorkStatus(charA, charB);
					goto IL_1C4;
				case 11:
					result = CharacterSortFilter.CompareAttackMedal(charA, charB);
					goto IL_1C4;
				case 12:
					result = CharacterSortFilter.CompareDefenseMedal(charA, charB);
					goto IL_1C4;
				case 13:
					result = CharacterSortFilter.CompareWisdomMedal(charA, charB);
					goto IL_1C4;
				case 17:
					result = CharacterSortFilter.CompareCombatSkillAgeAdjust(charA, charB);
					goto IL_1C4;
				case 18:
					result = CharacterSortFilter.CompareLifeSkillAgeAdjust(charA, charB);
					goto IL_1C4;
				case 19:
					result = CharacterSortFilter.CompareDefeatMarkCount(charA, charB);
					goto IL_1C4;
				case 20:
					result = CharacterSortFilter.CompareDisorderOfQi(charA, charB);
					goto IL_1C4;
				case 21:
					result = CharacterSortFilter.ComparePreexistenceCharCount(charA, charB);
					goto IL_1C4;
				case 22:
					result = CharacterSortFilter.CompareCurrInventoryLoad(charA, charB);
					goto IL_1C4;
				case 23:
					result = CharacterSortFilter.CompareKidnappedCharacterCount(charA, charB);
					goto IL_1C4;
				case 24:
					result = CharacterSortFilter.CompareOrganization(charA, charB);
					goto IL_1C4;
				}
			}
			else
			{
				if (sortingType >= 100)
				{
					result = CharacterSortFilter.CompareByReferencedType(charA, charB, (ECharacterPropertyReferencedType)(sortingType - 100));
					goto IL_1C4;
				}
				if (sortingType == 34)
				{
					result = CharacterSortFilter.CompareConsummateLevel(charA, charB);
					goto IL_1C4;
				}
			}
			result = 0;
			IL_1C4:
			if (!true)
			{
			}
			return result;
		}

		// Token: 0x06007522 RID: 29986 RVA: 0x00448E14 File Offset: 0x00447014
		private static int CompareByReferencedType(CharacterSortingInfo charA, CharacterSortingInfo charB, ECharacterPropertyReferencedType referencedType)
		{
			if (!true)
			{
			}
			int result;
			if (referencedType <= ECharacterPropertyReferencedType.AttainmentEclectic)
			{
				if (referencedType <= ECharacterPropertyReferencedType.HitRateMind)
				{
					if (referencedType >= ECharacterPropertyReferencedType.Strength)
					{
						if (referencedType > ECharacterPropertyReferencedType.Intelligence)
						{
							result = CharacterSortFilter.CompareHitValue(charA, charB, (sbyte)(referencedType - ECharacterPropertyReferencedType.HitRateStrength));
							goto IL_179;
						}
						result = CharacterSortFilter.CompareMaxMainAttribute(charA, charB, (sbyte)(referencedType - ECharacterPropertyReferencedType.Strength));
						goto IL_179;
					}
				}
				else if (referencedType >= ECharacterPropertyReferencedType.QualificationMusic)
				{
					if (referencedType > ECharacterPropertyReferencedType.QualificationEclectic)
					{
						result = CharacterSortFilter.CompareLifeSkillAttainment(charA, charB, (sbyte)(referencedType - ECharacterPropertyReferencedType.AttainmentMusic));
						goto IL_179;
					}
					result = CharacterSortFilter.CompareLifeSkillQualification(charA, charB, (sbyte)(referencedType - ECharacterPropertyReferencedType.QualificationMusic));
					goto IL_179;
				}
				else if (referencedType <= ECharacterPropertyReferencedType.AvoidRateMind)
				{
					if (referencedType == ECharacterPropertyReferencedType.PenetrateOfOuter)
					{
						result = CharacterSortFilter.CompareOuterPenetration(charA, charB);
						goto IL_179;
					}
					if (referencedType != ECharacterPropertyReferencedType.PenetrateOfInner)
					{
						result = CharacterSortFilter.CompareAvoidValue(charA, charB, (sbyte)(referencedType - ECharacterPropertyReferencedType.AvoidRateStrength));
						goto IL_179;
					}
					result = CharacterSortFilter.CompareInnerPenetration(charA, charB);
					goto IL_179;
				}
				else
				{
					if (referencedType == ECharacterPropertyReferencedType.PenetrateResistOfOuter)
					{
						result = CharacterSortFilter.CompareOuterPenetrationResist(charA, charB);
						goto IL_179;
					}
					if (referencedType == ECharacterPropertyReferencedType.PenetrateResistOfInner)
					{
						result = CharacterSortFilter.CompareInnerPenetrationResist(charA, charB);
						goto IL_179;
					}
				}
			}
			else if (referencedType <= ECharacterPropertyReferencedType.AttainmentCombatMusic)
			{
				if (referencedType > ECharacterPropertyReferencedType.QualificationCombatMusic)
				{
					result = CharacterSortFilter.CompareCombatSkillAttainment(charA, charB, (sbyte)(referencedType - ECharacterPropertyReferencedType.AttainmentNeigong));
					goto IL_179;
				}
				result = CharacterSortFilter.CompareCombatSkillQualification(charA, charB, (sbyte)(referencedType - ECharacterPropertyReferencedType.QualificationNeigong));
				goto IL_179;
			}
			else
			{
				if (referencedType <= ECharacterPropertyReferencedType.PersonalityPerceptive)
				{
					result = CharacterSortFilter.ComparePersonality(charA, charB, (sbyte)(referencedType - ECharacterPropertyReferencedType.PersonalityCalm));
					goto IL_179;
				}
				if (referencedType == ECharacterPropertyReferencedType.Attraction)
				{
					result = CharacterSortFilter.CompareAttractionType(charA, charB);
					goto IL_179;
				}
			}
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(31, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Sorting for ");
			defaultInterpolatedStringHandler.AppendFormatted<ECharacterPropertyReferencedType>(referencedType);
			defaultInterpolatedStringHandler.AppendLiteral(" is not implemented");
			throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			IL_179:
			if (!true)
			{
			}
			return result;
		}

		// Token: 0x06007523 RID: 29987 RVA: 0x00448FA4 File Offset: 0x004471A4
		private static int CompareName(CharacterSortingInfo charA, CharacterSortingInfo charB)
		{
			int i = 0;
			while (i < charB.NameBytes.Length)
			{
				bool flag = charA.NameBytes.Length <= i;
				int result;
				if (flag)
				{
					result = -1;
				}
				else
				{
					int comparison = charA.NameBytes[i].CompareTo(charB.NameBytes[i]);
					bool flag2 = comparison == 0;
					if (flag2)
					{
						i++;
						continue;
					}
					result = comparison;
				}
				return result;
			}
			return 1;
		}

		// Token: 0x06007524 RID: 29988 RVA: 0x00449014 File Offset: 0x00447214
		private static int CompareGrade(CharacterSortingInfo charA, CharacterSortingInfo charB)
		{
			return charA.Character.GetOrganizationInfo().Grade.CompareTo(charB.Character.GetOrganizationInfo().Grade);
		}

		// Token: 0x06007525 RID: 29989 RVA: 0x0044904C File Offset: 0x0044724C
		private static int CompareAge(CharacterSortingInfo charA, CharacterSortingInfo charB)
		{
			return charA.Character.GetCurrAge().CompareTo(charB.Character.GetCurrAge());
		}

		// Token: 0x06007526 RID: 29990 RVA: 0x00449077 File Offset: 0x00447277
		private static int CompareHealth(CharacterSortingInfo charA, CharacterSortingInfo charB)
		{
			return charA.HealthPercentage.CompareTo(charB.HealthPercentage);
		}

		// Token: 0x06007527 RID: 29991 RVA: 0x0044908C File Offset: 0x0044728C
		private static int CompareGender(CharacterSortingInfo charA, CharacterSortingInfo charB)
		{
			return charA.Character.GetGender().CompareTo(charB.Character.GetGender());
		}

		// Token: 0x06007528 RID: 29992 RVA: 0x004490B7 File Offset: 0x004472B7
		private static int CompareMorality(CharacterSortingInfo charA, CharacterSortingInfo charB)
		{
			return charA.Morality.CompareTo(charB.Morality);
		}

		// Token: 0x06007529 RID: 29993 RVA: 0x004490CA File Offset: 0x004472CA
		private static int CompareBehaviorType(CharacterSortingInfo charA, CharacterSortingInfo charB)
		{
			return charA.BehaviorType.CompareTo(charB.BehaviorType);
		}

		// Token: 0x0600752A RID: 29994 RVA: 0x004490E0 File Offset: 0x004472E0
		private static int CompareHappiness(CharacterSortingInfo charA, CharacterSortingInfo charB)
		{
			return charA.Character.GetHappiness().CompareTo(charB.Character.GetHappiness());
		}

		// Token: 0x0600752B RID: 29995 RVA: 0x0044910B File Offset: 0x0044730B
		private static int CompareHappinessType(CharacterSortingInfo charA, CharacterSortingInfo charB)
		{
			return charA.HappinessType.CompareTo(charB.HappinessType);
		}

		// Token: 0x0600752C RID: 29996 RVA: 0x0044911E File Offset: 0x0044731E
		private static int CompareFavorability(CharacterSortingInfo charA, CharacterSortingInfo charB)
		{
			return charA.FavorabilityToTaiwu.CompareTo(charB.FavorabilityToTaiwu);
		}

		// Token: 0x0600752D RID: 29997 RVA: 0x00449131 File Offset: 0x00447331
		private static int CompareFavorType(CharacterSortingInfo charA, CharacterSortingInfo charB)
		{
			return charA.FavorType.CompareTo(charB.FavorType);
		}

		// Token: 0x0600752E RID: 29998 RVA: 0x00449144 File Offset: 0x00447344
		private static int CompareFame(CharacterSortingInfo charA, CharacterSortingInfo charB)
		{
			return charA.Fame.CompareTo(charB.Fame);
		}

		// Token: 0x0600752F RID: 29999 RVA: 0x00449157 File Offset: 0x00447357
		private static int CompareFameType(CharacterSortingInfo charA, CharacterSortingInfo charB)
		{
			return charA.FameType.CompareTo(charB.FameType);
		}

		// Token: 0x06007530 RID: 30000 RVA: 0x0044916A File Offset: 0x0044736A
		private static int CompareAttraction(CharacterSortingInfo charA, CharacterSortingInfo charB)
		{
			return charA.Attraction.CompareTo(charB.Attraction);
		}

		// Token: 0x06007531 RID: 30001 RVA: 0x0044917D File Offset: 0x0044737D
		private static int CompareAttractionType(CharacterSortingInfo charA, CharacterSortingInfo charB)
		{
			return charA.AttractionType.CompareTo(charB.AttractionType);
		}

		// Token: 0x06007532 RID: 30002 RVA: 0x00449190 File Offset: 0x00447390
		private static int CompareLivingStatus(CharacterSortingInfo charA, CharacterSortingInfo charB)
		{
			return charA.LivingStatus.CompareTo(charB.LivingStatus);
		}

		// Token: 0x06007533 RID: 30003 RVA: 0x004491A3 File Offset: 0x004473A3
		private static int CompareWorkStatus(CharacterSortingInfo charA, CharacterSortingInfo charB)
		{
			return charA.WorkStatus.CompareTo(charB.WorkStatus);
		}

		// Token: 0x06007534 RID: 30004 RVA: 0x004491B6 File Offset: 0x004473B6
		private static int CompareAttackMedal(CharacterSortingInfo charA, CharacterSortingInfo charB)
		{
			return charA.AttackMedal.CompareTo(charB.AttackMedal);
		}

		// Token: 0x06007535 RID: 30005 RVA: 0x004491C9 File Offset: 0x004473C9
		private static int CompareDefenseMedal(CharacterSortingInfo charA, CharacterSortingInfo charB)
		{
			return charA.DefenseMedal.CompareTo(charB.DefenseMedal);
		}

		// Token: 0x06007536 RID: 30006 RVA: 0x004491DC File Offset: 0x004473DC
		private static int CompareWisdomMedal(CharacterSortingInfo charA, CharacterSortingInfo charB)
		{
			return charA.WisdomMedal.CompareTo(charB.WisdomMedal);
		}

		// Token: 0x06007537 RID: 30007 RVA: 0x004491EF File Offset: 0x004473EF
		private static int CompareDefeatMarkCount(CharacterSortingInfo charA, CharacterSortingInfo charB)
		{
			return charA.DefeatMarkCount.CompareTo(charB.DefeatMarkCount);
		}

		// Token: 0x06007538 RID: 30008 RVA: 0x00449202 File Offset: 0x00447402
		private static int CompareCombatSkillAgeAdjust(CharacterSortingInfo charA, CharacterSortingInfo charB)
		{
			return charA.CombatSkillAgeAdjust.CompareTo(charB.CombatSkillAgeAdjust);
		}

		// Token: 0x06007539 RID: 30009 RVA: 0x00449215 File Offset: 0x00447415
		private static int CompareLifeSkillAgeAdjust(CharacterSortingInfo charA, CharacterSortingInfo charB)
		{
			return charA.LifeSkillAgeAdjust.CompareTo(charB.LifeSkillAgeAdjust);
		}

		// Token: 0x0600753A RID: 30010 RVA: 0x00449228 File Offset: 0x00447428
		private static int ComparePersonality(CharacterSortingInfo charA, CharacterSortingInfo charB, sbyte personalityType)
		{
			return charA.Character.GetPersonality(personalityType).CompareTo(charB.Character.GetPersonality(personalityType));
		}

		// Token: 0x0600753B RID: 30011 RVA: 0x00449258 File Offset: 0x00447458
		private static int CompareCombatSkillQualification(CharacterSortingInfo charA, CharacterSortingInfo charB, sbyte combatSkillType)
		{
			return charA.Character.GetCombatSkillQualification(combatSkillType).CompareTo(charB.Character.GetCombatSkillQualification(combatSkillType));
		}

		// Token: 0x0600753C RID: 30012 RVA: 0x00449288 File Offset: 0x00447488
		private static int CompareCombatSkillAttainment(CharacterSortingInfo charA, CharacterSortingInfo charB, sbyte combatSkillType)
		{
			return charA.Character.GetCombatSkillAttainment(combatSkillType).CompareTo(charB.Character.GetCombatSkillAttainment(combatSkillType));
		}

		// Token: 0x0600753D RID: 30013 RVA: 0x004492B8 File Offset: 0x004474B8
		private static int CompareLifeSkillQualification(CharacterSortingInfo charA, CharacterSortingInfo charB, sbyte lifeSkillType)
		{
			return charA.Character.GetLifeSkillQualification(lifeSkillType).CompareTo(charB.Character.GetLifeSkillQualification(lifeSkillType));
		}

		// Token: 0x0600753E RID: 30014 RVA: 0x004492E8 File Offset: 0x004474E8
		private static int CompareLifeSkillAttainment(CharacterSortingInfo charA, CharacterSortingInfo charB, sbyte lifeSkillType)
		{
			return charA.Character.GetLifeSkillAttainment(lifeSkillType).CompareTo(charB.Character.GetLifeSkillAttainment(lifeSkillType));
		}

		// Token: 0x0600753F RID: 30015 RVA: 0x00449318 File Offset: 0x00447518
		private static int CompareMaxMainAttribute(CharacterSortingInfo charA, CharacterSortingInfo charB, sbyte mainAttributeType)
		{
			return charA.Character.GetMaxMainAttribute(mainAttributeType).CompareTo(charB.Character.GetMaxMainAttribute(mainAttributeType));
		}

		// Token: 0x06007540 RID: 30016 RVA: 0x00449348 File Offset: 0x00447548
		private static int CompareResource(CharacterSortingInfo charA, CharacterSortingInfo charB, sbyte resourceType)
		{
			return charA.Character.GetResource(resourceType).CompareTo(charB.Character.GetResource(resourceType));
		}

		// Token: 0x06007541 RID: 30017 RVA: 0x00449378 File Offset: 0x00447578
		private static int CompareInnerPenetration(CharacterSortingInfo charA, CharacterSortingInfo charB)
		{
			return charA.Character.GetPenetrations().Inner.CompareTo(charB.Character.GetPenetrations().Inner);
		}

		// Token: 0x06007542 RID: 30018 RVA: 0x004493B0 File Offset: 0x004475B0
		private static int CompareOuterPenetration(CharacterSortingInfo charA, CharacterSortingInfo charB)
		{
			return charA.Character.GetPenetrations().Outer.CompareTo(charB.Character.GetPenetrations().Outer);
		}

		// Token: 0x06007543 RID: 30019 RVA: 0x004493E8 File Offset: 0x004475E8
		private static int CompareInnerPenetrationResist(CharacterSortingInfo charA, CharacterSortingInfo charB)
		{
			return charA.Character.GetPenetrations().Inner.CompareTo(charB.Character.GetPenetrations().Inner);
		}

		// Token: 0x06007544 RID: 30020 RVA: 0x00449420 File Offset: 0x00447620
		private static int CompareOuterPenetrationResist(CharacterSortingInfo charA, CharacterSortingInfo charB)
		{
			return charA.Character.GetPenetrations().Outer.CompareTo(charB.Character.GetPenetrations().Outer);
		}

		// Token: 0x06007545 RID: 30021 RVA: 0x00449455 File Offset: 0x00447655
		private unsafe static int CompareHitValue(CharacterSortingInfo charA, CharacterSortingInfo charB, sbyte hitValueType)
		{
			return (ref charA.HitValues.Items.FixedElementField + (IntPtr)hitValueType * 4).CompareTo(*(ref charB.HitValues.Items.FixedElementField + (IntPtr)hitValueType * 4));
		}

		// Token: 0x06007546 RID: 30022 RVA: 0x00449487 File Offset: 0x00447687
		private unsafe static int CompareAvoidValue(CharacterSortingInfo charA, CharacterSortingInfo charB, sbyte hitValueType)
		{
			return (ref charA.AvoidValues.Items.FixedElementField + (IntPtr)hitValueType * 4).CompareTo(*(ref charB.AvoidValues.Items.FixedElementField + (IntPtr)hitValueType * 4));
		}

		// Token: 0x06007547 RID: 30023 RVA: 0x004494BC File Offset: 0x004476BC
		private static int CompareDisorderOfQi(CharacterSortingInfo charA, CharacterSortingInfo charB)
		{
			return charA.Character.GetDisorderOfQi().CompareTo(charB.Character.GetDisorderOfQi());
		}

		// Token: 0x06007548 RID: 30024 RVA: 0x004494E7 File Offset: 0x004476E7
		private static int ComparePreexistenceCharCount(CharacterSortingInfo charA, CharacterSortingInfo charB)
		{
			return charA.Character.GetPreexistenceCharIds().Count.CompareTo(charB.Character.GetPreexistenceCharIds().Count);
		}

		// Token: 0x06007549 RID: 30025 RVA: 0x00449510 File Offset: 0x00447710
		private static int CompareCurrInventoryLoad(CharacterSortingInfo charA, CharacterSortingInfo charB)
		{
			return charA.Character.GetCurrInventoryLoad().CompareTo(charB.Character.GetCurrInventoryLoad());
		}

		// Token: 0x0600754A RID: 30026 RVA: 0x0044953B File Offset: 0x0044773B
		private static int CompareKidnappedCharacterCount(CharacterSortingInfo charA, CharacterSortingInfo charB)
		{
			return charA.KidnappedCharCount.CompareTo(charB.KidnappedCharCount);
		}

		// Token: 0x0600754B RID: 30027 RVA: 0x00449550 File Offset: 0x00447750
		private static int CompareOrganization(CharacterSortingInfo charA, CharacterSortingInfo charB)
		{
			OrganizationInfo charAOrgInfo = charA.Character.GetOrganizationInfo();
			OrganizationInfo charBOrgInfo = charB.Character.GetOrganizationInfo();
			int compareOrgId = charAOrgInfo.OrgTemplateId.CompareTo(charBOrgInfo.OrgTemplateId);
			return (compareOrgId != 0) ? compareOrgId : charAOrgInfo.SettlementId.CompareTo(charBOrgInfo.SettlementId);
		}

		// Token: 0x0600754C RID: 30028 RVA: 0x004495A8 File Offset: 0x004477A8
		private static int CompareConsummateLevel(CharacterSortingInfo charA, CharacterSortingInfo charB)
		{
			return charA.Character.GetConsummateLevel().CompareTo(charB.Character.GetConsummateLevel());
		}

		// Token: 0x04001F1A RID: 7962
		private readonly List<CharacterSortingInfo> _characters;

		// Token: 0x04001F1B RID: 7963
		private readonly List<CharacterSortingInfo>[] _filteredCharacters;

		// Token: 0x04001F1C RID: 7964
		private readonly Encoding _currEncoding;

		// Token: 0x04001F1D RID: 7965
		private CharacterSortFilterSettings _settings;

		// Token: 0x04001F1E RID: 7966
		private List<int> _characterCounts;

		// Token: 0x04001F1F RID: 7967
		private List<int> _maxSortingTypeCharIds;

		// Token: 0x04001F20 RID: 7968
		private HashSet<int> _addedCharIdSet;

		// Token: 0x04001F21 RID: 7969
		private static readonly Dictionary<string, Encoding> LanguageEncodingDict = new Dictionary<string, Encoding>
		{
			{
				"CN",
				Encoding.GetEncoding("GBK")
			},
			{
				"CNH",
				Encoding.GetEncoding("GBK")
			},
			{
				"EN",
				Encoding.ASCII
			},
			{
				"JP",
				Encoding.Unicode
			}
		};
	}
}
