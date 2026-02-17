using System;
using System.Collections.Generic;
using Config.ConfigCells.Character;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.Character.Ai;
using GameData.Domains.Character.Ai.PrioritizedAction;
using GameData.Domains.Character.Filters;
using GameData.Domains.Map;
using GameData.Domains.Organization;
using GameData.Domains.TaiwuEvent.EventHelper;
using GameData.Utilities;

namespace GameData.Domains.TaiwuEvent.MonthlyEventActions
{
	// Token: 0x0200008A RID: 138
	public static class CallCharacterHelper
	{
		// Token: 0x060018EC RID: 6380 RVA: 0x00168034 File Offset: 0x00166234
		public static bool CallCharacters(Location location, sbyte searchRange, CharacterFilterRequirement[] filterRequirements, List<CharacterSet> characterSets, bool allowTemporaryCharacters, bool modifyExternalState, bool hideCharacters = false)
		{
			return CallCharacterHelper.CallCharacters(location, searchRange, filterRequirements, characterSets, allowTemporaryCharacters, modifyExternalState, hideCharacters, null);
		}

		// Token: 0x060018ED RID: 6381 RVA: 0x00168058 File Offset: 0x00166258
		public static bool CallCharacters(Location location, CallCharacterHelper.SearchCharacterRule searchCharacterRule, List<CharacterSet> characterSets, bool modifyExternalState)
		{
			DataContext context = DomainManager.TaiwuEvent.MainThreadDataContext;
			List<short> areaList = ObjectPool<List<short>>.Instance.Get();
			sbyte curStateTemplateId = DomainManager.Map.GetStateTemplateIdByAreaId(location.AreaId);
			sbyte curStateId = curStateTemplateId - 1;
			DomainManager.Map.GetAllAreaInState(curStateId, areaList);
			List<Character> targetList = ObjectPool<List<Character>>.Instance.Get();
			targetList.Clear();
			for (int i = 0; i < searchCharacterRule.SubRules.Count; i++)
			{
				CallCharacterHelper.SearchCharacterSubRule subRule = searchCharacterRule.SubRules[i];
				switch (searchCharacterRule.SearchRange)
				{
				case 0:
					EventHelper.FindCharacters(subRule.Predicate, Math.Max(subRule.MinAmount, subRule.MaxAmount), targetList, location.AreaId, location.AreaId, curStateTemplateId);
					break;
				case 1:
					EventHelper.FindCharacters(subRule.Predicate, Math.Max(subRule.MinAmount, subRule.MaxAmount), targetList, areaList, curStateTemplateId);
					break;
				case 2:
					EventHelper.FindCharacters(subRule.Predicate, Math.Max(subRule.MinAmount, subRule.MaxAmount), targetList, 0, 135, curStateTemplateId);
					break;
				}
				bool flag = subRule.ConfirmPredicate != null;
				if (flag)
				{
					for (int j = targetList.Count - 1; j >= 0; j--)
					{
						bool flag2 = !subRule.ConfirmPredicate(targetList[j]);
						if (flag2)
						{
							CollectionUtils.SwapAndRemove<Character>(targetList, j);
						}
					}
				}
				bool flag3 = characterSets.Count > i;
				if (flag3)
				{
					CharacterSet charSet = characterSets[i];
					charSet.AddRange(targetList.ConvertAll<int>((Character character) => character.GetId()));
					characterSets[i] = charSet;
				}
				else
				{
					CharacterSet charSet2 = default(CharacterSet);
					charSet2.AddRange(targetList.ConvertAll<int>((Character character) => character.GetId()));
					characterSets.Add(charSet2);
				}
				foreach (Character participant in targetList)
				{
					if (modifyExternalState)
					{
						participant.ActiveExternalRelationState(context, 4);
					}
					bool flag4 = participant.GetAgeGroup() != 0;
					if (flag4)
					{
						DomainManager.Character.LeaveGroup(context, participant, true);
					}
					DomainManager.Character.GroupMove(context, participant, location);
				}
				bool flag5 = characterSets[i].GetCount() < subRule.MinAmount && !searchCharacterRule.AllowTemporaryCharacter;
				if (flag5)
				{
					ObjectPool<List<short>>.Instance.Return(areaList);
					ObjectPool<List<Character>>.Instance.Return(targetList);
					return false;
				}
			}
			ObjectPool<List<short>>.Instance.Return(areaList);
			ObjectPool<List<Character>>.Instance.Return(targetList);
			return true;
		}

		// Token: 0x060018EE RID: 6382 RVA: 0x00168370 File Offset: 0x00166570
		public static bool CallCharacters(Location location, sbyte searchRange, CharacterFilterRequirement[] filterRequirements, List<CharacterSet> characterSets, bool allowTemporaryCharacters, bool modifyExternalState, bool hideCharacters, Action<DataContext, Character> onCharacterCalled)
		{
			DataContext context = DomainManager.TaiwuEvent.MainThreadDataContext;
			List<short> areaList = ObjectPool<List<short>>.Instance.Get();
			sbyte curStateTemplateId = DomainManager.Map.GetStateTemplateIdByAreaId(location.AreaId);
			sbyte curStateId = curStateTemplateId - 1;
			DomainManager.Map.GetAllAreaInState(curStateId, areaList);
			List<Character> targetList = ObjectPool<List<Character>>.Instance.Get();
			targetList.Clear();
			int i = 0;
			while (i < filterRequirements.Length)
			{
				CharacterFilterRequirement filterReq = filterRequirements[i];
				switch (searchRange)
				{
				case 0:
					EventHelper.FindCharacters(filterReq.CharacterFilterRuleIds, Math.Max(filterReq.MinCharactersRequired, filterReq.MaxCharactersRequired), targetList, location.AreaId, location.AreaId, curStateTemplateId);
					break;
				case 1:
					EventHelper.FindCharacters(filterReq.CharacterFilterRuleIds, Math.Max(filterReq.MinCharactersRequired, filterReq.MaxCharactersRequired), targetList, areaList, curStateTemplateId);
					break;
				case 2:
					EventHelper.FindCharacters(filterReq.CharacterFilterRuleIds, Math.Max(filterReq.MinCharactersRequired, filterReq.MaxCharactersRequired), targetList, 0, 135, curStateTemplateId);
					break;
				}
				bool flag = characterSets.Count > i;
				if (!flag)
				{
					CharacterSet charSet = default(CharacterSet);
					charSet.AddRange(targetList.ConvertAll<int>((Character character) => character.GetId()));
					characterSets.Add(charSet);
					goto IL_1C5;
				}
				CharacterSet charSet2 = characterSets[i];
				bool flag2 = filterReq.MaxCharactersRequired > 0 && charSet2.GetCount() >= filterReq.MaxCharactersRequired * 3 / 2;
				if (!flag2)
				{
					charSet2.AddRange(targetList.ConvertAll<int>((Character character) => character.GetId()));
					characterSets[i] = charSet2;
					goto IL_1C5;
				}
				IL_2D6:
				i++;
				continue;
				IL_1C5:
				foreach (Character participant in targetList)
				{
					if (modifyExternalState)
					{
						participant.ActiveExternalRelationState(context, 4);
					}
					bool flag3 = participant.GetAgeGroup() != 0;
					if (flag3)
					{
						DomainManager.Character.LeaveGroup(context, participant, true);
					}
					DomainManager.Character.GroupMove(context, participant, location);
					if (hideCharacters)
					{
						bool flag4 = participant.IsCompletelyInfected();
						if (flag4)
						{
							Events.RaiseInfectedCharacterLocationChanged(context, participant.GetId(), location, Location.Invalid);
						}
						else
						{
							Events.RaiseCharacterLocationChanged(context, participant.GetId(), location, Location.Invalid);
						}
					}
					if (onCharacterCalled != null)
					{
						onCharacterCalled(context, participant);
					}
				}
				bool flag5 = characterSets[i].GetCount() < filterReq.MinCharactersRequired && !allowTemporaryCharacters;
				if (flag5)
				{
					ObjectPool<List<short>>.Instance.Return(areaList);
					ObjectPool<List<Character>>.Instance.Return(targetList);
					return false;
				}
				goto IL_2D6;
			}
			ObjectPool<List<short>>.Instance.Return(areaList);
			ObjectPool<List<Character>>.Instance.Return(targetList);
			return true;
		}

		// Token: 0x060018EF RID: 6383 RVA: 0x0016869C File Offset: 0x0016689C
		public static void ClearCalledCharacters(List<CharacterSet> characterSets, bool unHideCharacters, bool removeExternalState)
		{
			DataContext context = DomainManager.TaiwuEvent.MainThreadDataContext;
			characterSets.ForEach(delegate(CharacterSet charSet)
			{
				HashSet<int> collection = charSet.GetCollection();
				foreach (int charId in collection)
				{
					Character character;
					bool flag = !DomainManager.Character.TryGetElement_Objects(charId, out character);
					if (!flag)
					{
						bool flag2 = unHideCharacters && character.GetLeaderId() != DomainManager.Taiwu.GetTaiwuCharId();
						if (flag2)
						{
							bool flag3 = character.IsCompletelyInfected();
							if (flag3)
							{
								Events.RaiseInfectedCharacterLocationChanged(context, charId, Location.Invalid, character.GetLocation());
							}
							else
							{
								Events.RaiseCharacterLocationChanged(context, charId, Location.Invalid, character.GetLocation());
							}
						}
						bool removeExternalState2 = removeExternalState;
						if (removeExternalState2)
						{
							character.DeactivateExternalRelationState(context, 4);
						}
						short settlementId = character.GetOrganizationInfo().SettlementId;
						bool flag4 = settlementId >= 0;
						if (flag4)
						{
							sbyte duration = 3;
							BasePrioritizedAction action;
							bool flag5 = DomainManager.Character.TryGetCharacterPrioritizedAction(charId, out action);
							if (flag5)
							{
								duration = (sbyte)Math.Min(127, action.Target.RemainingMonth + (int)duration);
							}
							Settlement settlement = DomainManager.Organization.GetSettlement(settlementId);
							PrioritizedActionCooldownSbytes cooldown = character.GetPrioritizedActionCooldowns();
							cooldown.AddAllActionCooldown(duration);
							character.SetPrioritizedActionCooldowns(cooldown, context);
							List<NpcTravelTarget> travelTargets = character.GetNpcTravelTargets();
							travelTargets.Clear();
							travelTargets.Add(new NpcTravelTarget(settlement.GetLocation(), 127));
							character.SetNpcTravelTargets(travelTargets, context);
						}
					}
				}
				charSet.Clear();
			});
			characterSets.Clear();
		}

		// Token: 0x060018F0 RID: 6384 RVA: 0x001686E8 File Offset: 0x001668E8
		public static bool CheckCalledCharactersStillValid(CharacterFilterRequirement[] filterRequirements, List<CharacterSet> characterSets, sbyte curStateTemplateId)
		{
			int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
			for (int i = 0; i < filterRequirements.Length; i++)
			{
				bool flag = characterSets.Count <= 0;
				if (!flag)
				{
					CharacterFilterRequirement filterReq = filterRequirements[i];
					CharacterSet characterSet = characterSets[i];
					bool flag2 = characterSet.GetCount() == 0;
					if (!flag2)
					{
						CallCharacterHelper.PredicatesList.Clear();
						foreach (short filterRuleId in filterReq.CharacterFilterRuleIds)
						{
							List<Predicate<Character>> predicates = ObjectPool<List<Predicate<Character>>>.Instance.Get();
							CharacterFilterRules.ToPredicates(filterRuleId, predicates, curStateTemplateId);
							predicates.RemoveAt(0);
							CallCharacterHelper.PredicatesList.Add(predicates);
						}
						CharacterSet toRemove = default(CharacterSet);
						foreach (int charId in characterSet.GetCollection())
						{
							Character character;
							bool flag3 = !DomainManager.Character.TryGetElement_Objects(charId, out character) || character.GetLeaderId() == taiwuCharId;
							if (flag3)
							{
								toRemove.Add(charId);
							}
							else
							{
								bool isValid = false;
								foreach (List<Predicate<Character>> predicates2 in CallCharacterHelper.PredicatesList)
								{
									isValid = CharacterMatchers.MatchAll(character, predicates2);
									bool flag4 = isValid;
									if (flag4)
									{
										break;
									}
								}
								bool flag5 = !isValid;
								if (flag5)
								{
									return false;
								}
							}
						}
					}
				}
			}
			return true;
		}

		// Token: 0x060018F1 RID: 6385 RVA: 0x001688A8 File Offset: 0x00166AA8
		[Obsolete]
		public static int RemoveInvalidCharacters(CharacterFilterRequirement[] filterRequirements, List<CharacterSet> characterSets, sbyte curStateTemplateId)
		{
			return 0;
		}

		// Token: 0x060018F2 RID: 6386 RVA: 0x001688BC File Offset: 0x00166ABC
		public static int RemoveInvalidCharacters(CharacterFilterRequirement[] filterRequirements, List<CharacterSet> characterSets, Location location, sbyte curStateTemplateId)
		{
			DataContext context = DomainManager.TaiwuEvent.MainThreadDataContext;
			int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
			int totalInvalidCount = 0;
			for (int i = 0; i < filterRequirements.Length; i++)
			{
				bool flag = characterSets.Count <= 0;
				if (!flag)
				{
					CharacterFilterRequirement filterReq = filterRequirements[i];
					CharacterSet characterSet = characterSets[i];
					bool flag2 = characterSet.GetCount() == 0;
					if (!flag2)
					{
						CallCharacterHelper.PredicatesList.Clear();
						foreach (short filterRuleId in filterReq.CharacterFilterRuleIds)
						{
							List<Predicate<Character>> predicates = ObjectPool<List<Predicate<Character>>>.Instance.Get();
							CharacterFilterRules.ToPredicates(filterRuleId, predicates, curStateTemplateId);
							predicates.RemoveAt(0);
							CallCharacterHelper.PredicatesList.Add(predicates);
						}
						CharacterSet toRemove = default(CharacterSet);
						foreach (int charId in characterSet.GetCollection())
						{
							Character character;
							bool flag3 = !DomainManager.Character.TryGetElement_Objects(charId, out character) || character.GetLeaderId() == taiwuCharId;
							if (flag3)
							{
								toRemove.Add(charId);
							}
							else
							{
								bool isValid = character.GetLocation().Equals(location);
								bool flag4 = isValid;
								if (flag4)
								{
									foreach (List<Predicate<Character>> predicates2 in CallCharacterHelper.PredicatesList)
									{
										isValid = CharacterMatchers.MatchAll(character, predicates2);
										bool flag5 = isValid;
										if (flag5)
										{
											break;
										}
									}
								}
								bool flag6 = isValid;
								if (!flag6)
								{
									bool flag7 = character.IsCompletelyInfected();
									if (flag7)
									{
										Events.RaiseInfectedCharacterLocationChanged(context, charId, Location.Invalid, character.GetLocation());
									}
									else
									{
										Events.RaiseCharacterLocationChanged(context, charId, Location.Invalid, character.GetLocation());
									}
									character.DeactivateExternalRelationState(context, 4);
									toRemove.Add(charId);
								}
							}
						}
						HashSet<int> toRemoveCollection = toRemove.GetCollection();
						bool flag8 = toRemoveCollection.Count == 0;
						if (!flag8)
						{
							foreach (int toRemoveCharId in toRemoveCollection)
							{
								characterSet.Remove(toRemoveCharId);
							}
							totalInvalidCount += toRemoveCollection.Count;
							characterSets[i] = characterSet;
						}
					}
				}
			}
			return totalInvalidCount;
		}

		// Token: 0x060018F3 RID: 6387 RVA: 0x00168B84 File Offset: 0x00166D84
		public static int RemoveInvalidCharacters(CallCharacterHelper.SearchCharacterRule searchRule, List<CharacterSet> characterSets)
		{
			DataContext context = DomainManager.TaiwuEvent.MainThreadDataContext;
			int totalInvalidCount = 0;
			for (int i = 0; i < searchRule.SubRules.Count; i++)
			{
				bool flag = characterSets.Count <= i;
				if (!flag)
				{
					CharacterSet characterSet = characterSets[i];
					CallCharacterHelper.SearchCharacterSubRule subRule = searchRule.SubRules[i];
					CharacterSet toRemove = default(CharacterSet);
					foreach (int charId in characterSet.GetCollection())
					{
						Character character;
						bool flag2 = !DomainManager.Character.TryGetElement_Objects(charId, out character);
						if (flag2)
						{
							toRemove.Add(charId);
						}
						else
						{
							bool flag3 = subRule.Predicate == null || subRule.Predicate(character);
							if (!flag3)
							{
								bool flag4 = character.IsCompletelyInfected();
								if (flag4)
								{
									Events.RaiseInfectedCharacterLocationChanged(context, charId, Location.Invalid, character.GetLocation());
								}
								else
								{
									Events.RaiseCharacterLocationChanged(context, charId, Location.Invalid, character.GetLocation());
								}
								character.DeactivateExternalRelationState(context, 4);
								toRemove.Add(charId);
							}
						}
					}
					HashSet<int> toRemoveCollection = toRemove.GetCollection();
					bool flag5 = toRemoveCollection.Count == 0;
					if (!flag5)
					{
						foreach (int toRemoveCharId in toRemoveCollection)
						{
							characterSet.Remove(toRemoveCharId);
						}
						totalInvalidCount += toRemoveCollection.Count;
						characterSets[i] = characterSet;
					}
				}
			}
			return totalInvalidCount;
		}

		// Token: 0x060018F4 RID: 6388 RVA: 0x00168D54 File Offset: 0x00166F54
		public static bool IsAllCharactersAtLocation(Location location, CharacterFilterRequirement[] filterRequirements, List<CharacterSet> characterSets)
		{
			int i = 0;
			while (i < filterRequirements.Length)
			{
				CharacterFilterRequirement filterReq = filterRequirements[i];
				bool flag = characterSets.Count <= i;
				bool result;
				if (flag)
				{
					result = false;
				}
				else
				{
					CharacterSet characterSet = characterSets[i];
					int readyCount = 0;
					foreach (int charId in characterSet.GetCollection())
					{
						Character character;
						bool flag2 = !DomainManager.Character.TryGetElement_Objects(charId, out character);
						if (!flag2)
						{
							bool flag3 = character.GetLocation().Equals(location);
							if (flag3)
							{
								readyCount++;
							}
						}
					}
					bool flag4 = readyCount < filterReq.MinCharactersRequired;
					if (!flag4)
					{
						i++;
						continue;
					}
					result = false;
				}
				return result;
			}
			return true;
		}

		// Token: 0x060018F5 RID: 6389 RVA: 0x00168E3C File Offset: 0x0016703C
		public static bool IsAllCharactersAtLocation(Location location, CallCharacterHelper.SearchCharacterRule searchRule, List<CharacterSet> characterSets)
		{
			int i = 0;
			while (i < searchRule.SubRules.Count)
			{
				bool flag = characterSets.Count <= i;
				bool result;
				if (flag)
				{
					result = false;
				}
				else
				{
					CharacterSet characterSet = characterSets[i];
					int readyCount = 0;
					foreach (int charId in characterSet.GetCollection())
					{
						Character character;
						bool flag2 = !DomainManager.Character.TryGetElement_Objects(charId, out character);
						if (!flag2)
						{
							bool flag3 = character.GetLocation().Equals(location);
							if (flag3)
							{
								readyCount++;
							}
						}
					}
					bool flag4 = readyCount < searchRule.SubRules[i].MinAmount;
					if (!flag4)
					{
						i++;
						continue;
					}
					result = false;
				}
				return result;
			}
			return true;
		}

		// Token: 0x04000591 RID: 1425
		private static readonly List<List<Predicate<Character>>> PredicatesList = new List<List<Predicate<Character>>>();

		// Token: 0x0200099D RID: 2461
		public class SearchCharacterRule
		{
			// Token: 0x0400286F RID: 10351
			public bool AllowTemporaryCharacter;

			// Token: 0x04002870 RID: 10352
			public sbyte SearchRange;

			// Token: 0x04002871 RID: 10353
			public List<CallCharacterHelper.SearchCharacterSubRule> SubRules;
		}

		// Token: 0x0200099E RID: 2462
		public class SearchCharacterSubRule
		{
			// Token: 0x06008503 RID: 34051 RVA: 0x004E63A1 File Offset: 0x004E45A1
			public SearchCharacterSubRule(int minAmount, int maxAmount, Predicate<Character> predicate, Predicate<Character> confirmPredicate = null, Func<DataContext, int> createTempCharFunc = null, Action<Character> onCharacterCalled = null)
			{
				this.MinAmount = minAmount;
				this.MaxAmount = maxAmount;
				this.Predicate = predicate;
				this.ConfirmPredicate = confirmPredicate;
				this.CreateTemporaryCharacterFunc = createTempCharFunc;
				this.OnCharacterCalled = onCharacterCalled;
			}

			// Token: 0x04002872 RID: 10354
			public readonly int MinAmount;

			// Token: 0x04002873 RID: 10355
			public readonly int MaxAmount;

			// Token: 0x04002874 RID: 10356
			public readonly Predicate<Character> Predicate;

			// Token: 0x04002875 RID: 10357
			public readonly Predicate<Character> ConfirmPredicate;

			// Token: 0x04002876 RID: 10358
			public readonly Func<DataContext, int> CreateTemporaryCharacterFunc;

			// Token: 0x04002877 RID: 10359
			public readonly Action<Character> OnCharacterCalled;
		}
	}
}
