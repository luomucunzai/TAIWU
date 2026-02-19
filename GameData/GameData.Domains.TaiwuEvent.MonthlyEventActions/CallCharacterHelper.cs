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

namespace GameData.Domains.TaiwuEvent.MonthlyEventActions;

public static class CallCharacterHelper
{
	public class SearchCharacterRule
	{
		public bool AllowTemporaryCharacter;

		public sbyte SearchRange;

		public List<SearchCharacterSubRule> SubRules;
	}

	public class SearchCharacterSubRule
	{
		public readonly int MinAmount;

		public readonly int MaxAmount;

		public readonly Predicate<GameData.Domains.Character.Character> Predicate;

		public readonly Predicate<GameData.Domains.Character.Character> ConfirmPredicate;

		public readonly Func<DataContext, int> CreateTemporaryCharacterFunc;

		public readonly Action<GameData.Domains.Character.Character> OnCharacterCalled;

		public SearchCharacterSubRule(int minAmount, int maxAmount, Predicate<GameData.Domains.Character.Character> predicate, Predicate<GameData.Domains.Character.Character> confirmPredicate = null, Func<DataContext, int> createTempCharFunc = null, Action<GameData.Domains.Character.Character> onCharacterCalled = null)
		{
			MinAmount = minAmount;
			MaxAmount = maxAmount;
			Predicate = predicate;
			ConfirmPredicate = confirmPredicate;
			CreateTemporaryCharacterFunc = createTempCharFunc;
			OnCharacterCalled = onCharacterCalled;
		}
	}

	private static readonly List<List<Predicate<GameData.Domains.Character.Character>>> PredicatesList = new List<List<Predicate<GameData.Domains.Character.Character>>>();

	public static bool CallCharacters(Location location, sbyte searchRange, CharacterFilterRequirement[] filterRequirements, List<CharacterSet> characterSets, bool allowTemporaryCharacters, bool modifyExternalState, bool hideCharacters = false)
	{
		return CallCharacters(location, searchRange, filterRequirements, characterSets, allowTemporaryCharacters, modifyExternalState, hideCharacters, null);
	}

	public static bool CallCharacters(Location location, SearchCharacterRule searchCharacterRule, List<CharacterSet> characterSets, bool modifyExternalState)
	{
		DataContext mainThreadDataContext = DomainManager.TaiwuEvent.MainThreadDataContext;
		List<short> list = ObjectPool<List<short>>.Instance.Get();
		sbyte stateTemplateIdByAreaId = DomainManager.Map.GetStateTemplateIdByAreaId(location.AreaId);
		sbyte stateId = (sbyte)(stateTemplateIdByAreaId - 1);
		DomainManager.Map.GetAllAreaInState(stateId, list);
		List<GameData.Domains.Character.Character> list2 = ObjectPool<List<GameData.Domains.Character.Character>>.Instance.Get();
		list2.Clear();
		for (int i = 0; i < searchCharacterRule.SubRules.Count; i++)
		{
			SearchCharacterSubRule searchCharacterSubRule = searchCharacterRule.SubRules[i];
			switch (searchCharacterRule.SearchRange)
			{
			case 0:
				GameData.Domains.TaiwuEvent.EventHelper.EventHelper.FindCharacters(searchCharacterSubRule.Predicate, Math.Max(searchCharacterSubRule.MinAmount, searchCharacterSubRule.MaxAmount), list2, location.AreaId, location.AreaId, stateTemplateIdByAreaId);
				break;
			case 1:
				GameData.Domains.TaiwuEvent.EventHelper.EventHelper.FindCharacters(searchCharacterSubRule.Predicate, Math.Max(searchCharacterSubRule.MinAmount, searchCharacterSubRule.MaxAmount), list2, list, stateTemplateIdByAreaId);
				break;
			case 2:
				GameData.Domains.TaiwuEvent.EventHelper.EventHelper.FindCharacters(searchCharacterSubRule.Predicate, Math.Max(searchCharacterSubRule.MinAmount, searchCharacterSubRule.MaxAmount), list2, 0, 135, stateTemplateIdByAreaId);
				break;
			}
			if (searchCharacterSubRule.ConfirmPredicate != null)
			{
				for (int num = list2.Count - 1; num >= 0; num--)
				{
					if (!searchCharacterSubRule.ConfirmPredicate(list2[num]))
					{
						CollectionUtils.SwapAndRemove(list2, num);
					}
				}
			}
			if (characterSets.Count > i)
			{
				CharacterSet value = characterSets[i];
				value.AddRange(list2.ConvertAll((GameData.Domains.Character.Character character) => character.GetId()));
				characterSets[i] = value;
			}
			else
			{
				CharacterSet item = default(CharacterSet);
				item.AddRange(list2.ConvertAll((GameData.Domains.Character.Character character) => character.GetId()));
				characterSets.Add(item);
			}
			foreach (GameData.Domains.Character.Character item2 in list2)
			{
				if (modifyExternalState)
				{
					item2.ActiveExternalRelationState(mainThreadDataContext, 4);
				}
				if (item2.GetAgeGroup() != 0)
				{
					DomainManager.Character.LeaveGroup(mainThreadDataContext, item2);
				}
				DomainManager.Character.GroupMove(mainThreadDataContext, item2, location);
			}
			if (characterSets[i].GetCount() < searchCharacterSubRule.MinAmount && !searchCharacterRule.AllowTemporaryCharacter)
			{
				ObjectPool<List<short>>.Instance.Return(list);
				ObjectPool<List<GameData.Domains.Character.Character>>.Instance.Return(list2);
				return false;
			}
		}
		ObjectPool<List<short>>.Instance.Return(list);
		ObjectPool<List<GameData.Domains.Character.Character>>.Instance.Return(list2);
		return true;
	}

	public static bool CallCharacters(Location location, sbyte searchRange, CharacterFilterRequirement[] filterRequirements, List<CharacterSet> characterSets, bool allowTemporaryCharacters, bool modifyExternalState, bool hideCharacters, Action<DataContext, GameData.Domains.Character.Character> onCharacterCalled)
	{
		DataContext mainThreadDataContext = DomainManager.TaiwuEvent.MainThreadDataContext;
		List<short> list = ObjectPool<List<short>>.Instance.Get();
		sbyte stateTemplateIdByAreaId = DomainManager.Map.GetStateTemplateIdByAreaId(location.AreaId);
		sbyte stateId = (sbyte)(stateTemplateIdByAreaId - 1);
		DomainManager.Map.GetAllAreaInState(stateId, list);
		List<GameData.Domains.Character.Character> list2 = ObjectPool<List<GameData.Domains.Character.Character>>.Instance.Get();
		list2.Clear();
		for (int i = 0; i < filterRequirements.Length; i++)
		{
			CharacterFilterRequirement characterFilterRequirement = filterRequirements[i];
			switch (searchRange)
			{
			case 0:
				GameData.Domains.TaiwuEvent.EventHelper.EventHelper.FindCharacters(characterFilterRequirement.CharacterFilterRuleIds, Math.Max(characterFilterRequirement.MinCharactersRequired, characterFilterRequirement.MaxCharactersRequired), list2, location.AreaId, location.AreaId, stateTemplateIdByAreaId);
				break;
			case 1:
				GameData.Domains.TaiwuEvent.EventHelper.EventHelper.FindCharacters(characterFilterRequirement.CharacterFilterRuleIds, Math.Max(characterFilterRequirement.MinCharactersRequired, characterFilterRequirement.MaxCharactersRequired), list2, list, stateTemplateIdByAreaId);
				break;
			case 2:
				GameData.Domains.TaiwuEvent.EventHelper.EventHelper.FindCharacters(characterFilterRequirement.CharacterFilterRuleIds, Math.Max(characterFilterRequirement.MinCharactersRequired, characterFilterRequirement.MaxCharactersRequired), list2, 0, 135, stateTemplateIdByAreaId);
				break;
			}
			if (characterSets.Count > i)
			{
				CharacterSet value = characterSets[i];
				if (characterFilterRequirement.MaxCharactersRequired > 0 && value.GetCount() >= characterFilterRequirement.MaxCharactersRequired * 3 / 2)
				{
					continue;
				}
				value.AddRange(list2.ConvertAll((GameData.Domains.Character.Character character) => character.GetId()));
				characterSets[i] = value;
			}
			else
			{
				CharacterSet item = default(CharacterSet);
				item.AddRange(list2.ConvertAll((GameData.Domains.Character.Character character) => character.GetId()));
				characterSets.Add(item);
			}
			foreach (GameData.Domains.Character.Character item2 in list2)
			{
				if (modifyExternalState)
				{
					item2.ActiveExternalRelationState(mainThreadDataContext, 4);
				}
				if (item2.GetAgeGroup() != 0)
				{
					DomainManager.Character.LeaveGroup(mainThreadDataContext, item2);
				}
				DomainManager.Character.GroupMove(mainThreadDataContext, item2, location);
				if (hideCharacters)
				{
					if (item2.IsCompletelyInfected())
					{
						Events.RaiseInfectedCharacterLocationChanged(mainThreadDataContext, item2.GetId(), location, Location.Invalid);
					}
					else
					{
						Events.RaiseCharacterLocationChanged(mainThreadDataContext, item2.GetId(), location, Location.Invalid);
					}
				}
				onCharacterCalled?.Invoke(mainThreadDataContext, item2);
			}
			if (characterSets[i].GetCount() < characterFilterRequirement.MinCharactersRequired && !allowTemporaryCharacters)
			{
				ObjectPool<List<short>>.Instance.Return(list);
				ObjectPool<List<GameData.Domains.Character.Character>>.Instance.Return(list2);
				return false;
			}
		}
		ObjectPool<List<short>>.Instance.Return(list);
		ObjectPool<List<GameData.Domains.Character.Character>>.Instance.Return(list2);
		return true;
	}

	public static void ClearCalledCharacters(List<CharacterSet> characterSets, bool unHideCharacters, bool removeExternalState)
	{
		DataContext context = DomainManager.TaiwuEvent.MainThreadDataContext;
		characterSets.ForEach(delegate(CharacterSet charSet)
		{
			HashSet<int> collection = charSet.GetCollection();
			foreach (int item in collection)
			{
				if (DomainManager.Character.TryGetElement_Objects(item, out var element))
				{
					if (unHideCharacters && element.GetLeaderId() != DomainManager.Taiwu.GetTaiwuCharId())
					{
						if (element.IsCompletelyInfected())
						{
							Events.RaiseInfectedCharacterLocationChanged(context, item, Location.Invalid, element.GetLocation());
						}
						else
						{
							Events.RaiseCharacterLocationChanged(context, item, Location.Invalid, element.GetLocation());
						}
					}
					if (removeExternalState)
					{
						element.DeactivateExternalRelationState(context, 4);
					}
					short settlementId = element.GetOrganizationInfo().SettlementId;
					if (settlementId >= 0)
					{
						sbyte b = 3;
						if (DomainManager.Character.TryGetCharacterPrioritizedAction(item, out var action))
						{
							b = (sbyte)Math.Min(127, action.Target.RemainingMonth + b);
						}
						Settlement settlement = DomainManager.Organization.GetSettlement(settlementId);
						PrioritizedActionCooldownSbytes prioritizedActionCooldowns = element.GetPrioritizedActionCooldowns();
						prioritizedActionCooldowns.AddAllActionCooldown(b);
						element.SetPrioritizedActionCooldowns(prioritizedActionCooldowns, context);
						List<NpcTravelTarget> npcTravelTargets = element.GetNpcTravelTargets();
						npcTravelTargets.Clear();
						npcTravelTargets.Add(new NpcTravelTarget(settlement.GetLocation(), 127));
						element.SetNpcTravelTargets(npcTravelTargets, context);
					}
				}
			}
			charSet.Clear();
		});
		characterSets.Clear();
	}

	public static bool CheckCalledCharactersStillValid(CharacterFilterRequirement[] filterRequirements, List<CharacterSet> characterSets, sbyte curStateTemplateId)
	{
		int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
		for (int i = 0; i < filterRequirements.Length; i++)
		{
			if (characterSets.Count <= 0)
			{
				continue;
			}
			CharacterFilterRequirement characterFilterRequirement = filterRequirements[i];
			CharacterSet characterSet = characterSets[i];
			if (characterSet.GetCount() == 0)
			{
				continue;
			}
			PredicatesList.Clear();
			short[] characterFilterRuleIds = characterFilterRequirement.CharacterFilterRuleIds;
			foreach (short characterFilterRulesId in characterFilterRuleIds)
			{
				List<Predicate<GameData.Domains.Character.Character>> list = ObjectPool<List<Predicate<GameData.Domains.Character.Character>>>.Instance.Get();
				CharacterFilterRules.ToPredicates(characterFilterRulesId, list, curStateTemplateId);
				list.RemoveAt(0);
				PredicatesList.Add(list);
			}
			CharacterSet characterSet2 = default(CharacterSet);
			foreach (int item in characterSet.GetCollection())
			{
				if (!DomainManager.Character.TryGetElement_Objects(item, out var element) || element.GetLeaderId() == taiwuCharId)
				{
					characterSet2.Add(item);
					continue;
				}
				bool flag = false;
				foreach (List<Predicate<GameData.Domains.Character.Character>> predicates in PredicatesList)
				{
					flag = CharacterMatchers.MatchAll(element, predicates);
					if (flag)
					{
						break;
					}
				}
				if (flag)
				{
					continue;
				}
				return false;
			}
		}
		return true;
	}

	[Obsolete]
	public static int RemoveInvalidCharacters(CharacterFilterRequirement[] filterRequirements, List<CharacterSet> characterSets, sbyte curStateTemplateId)
	{
		return 0;
	}

	public static int RemoveInvalidCharacters(CharacterFilterRequirement[] filterRequirements, List<CharacterSet> characterSets, Location location, sbyte curStateTemplateId)
	{
		DataContext mainThreadDataContext = DomainManager.TaiwuEvent.MainThreadDataContext;
		int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
		int num = 0;
		for (int i = 0; i < filterRequirements.Length; i++)
		{
			if (characterSets.Count <= 0)
			{
				continue;
			}
			CharacterFilterRequirement characterFilterRequirement = filterRequirements[i];
			CharacterSet value = characterSets[i];
			if (value.GetCount() == 0)
			{
				continue;
			}
			PredicatesList.Clear();
			short[] characterFilterRuleIds = characterFilterRequirement.CharacterFilterRuleIds;
			foreach (short characterFilterRulesId in characterFilterRuleIds)
			{
				List<Predicate<GameData.Domains.Character.Character>> list = ObjectPool<List<Predicate<GameData.Domains.Character.Character>>>.Instance.Get();
				CharacterFilterRules.ToPredicates(characterFilterRulesId, list, curStateTemplateId);
				list.RemoveAt(0);
				PredicatesList.Add(list);
			}
			CharacterSet characterSet = default(CharacterSet);
			foreach (int item in value.GetCollection())
			{
				if (!DomainManager.Character.TryGetElement_Objects(item, out var element) || element.GetLeaderId() == taiwuCharId)
				{
					characterSet.Add(item);
					continue;
				}
				bool flag = element.GetLocation().Equals(location);
				if (flag)
				{
					foreach (List<Predicate<GameData.Domains.Character.Character>> predicates in PredicatesList)
					{
						flag = CharacterMatchers.MatchAll(element, predicates);
						if (flag)
						{
							break;
						}
					}
				}
				if (!flag)
				{
					if (element.IsCompletelyInfected())
					{
						Events.RaiseInfectedCharacterLocationChanged(mainThreadDataContext, item, Location.Invalid, element.GetLocation());
					}
					else
					{
						Events.RaiseCharacterLocationChanged(mainThreadDataContext, item, Location.Invalid, element.GetLocation());
					}
					element.DeactivateExternalRelationState(mainThreadDataContext, 4);
					characterSet.Add(item);
				}
			}
			HashSet<int> collection = characterSet.GetCollection();
			if (collection.Count == 0)
			{
				continue;
			}
			foreach (int item2 in collection)
			{
				value.Remove(item2);
			}
			num += collection.Count;
			characterSets[i] = value;
		}
		return num;
	}

	public static int RemoveInvalidCharacters(SearchCharacterRule searchRule, List<CharacterSet> characterSets)
	{
		DataContext mainThreadDataContext = DomainManager.TaiwuEvent.MainThreadDataContext;
		int num = 0;
		for (int i = 0; i < searchRule.SubRules.Count; i++)
		{
			if (characterSets.Count <= i)
			{
				continue;
			}
			CharacterSet value = characterSets[i];
			SearchCharacterSubRule searchCharacterSubRule = searchRule.SubRules[i];
			CharacterSet characterSet = default(CharacterSet);
			foreach (int item in value.GetCollection())
			{
				if (!DomainManager.Character.TryGetElement_Objects(item, out var element))
				{
					characterSet.Add(item);
				}
				else if (searchCharacterSubRule.Predicate != null && !searchCharacterSubRule.Predicate(element))
				{
					if (element.IsCompletelyInfected())
					{
						Events.RaiseInfectedCharacterLocationChanged(mainThreadDataContext, item, Location.Invalid, element.GetLocation());
					}
					else
					{
						Events.RaiseCharacterLocationChanged(mainThreadDataContext, item, Location.Invalid, element.GetLocation());
					}
					element.DeactivateExternalRelationState(mainThreadDataContext, 4);
					characterSet.Add(item);
				}
			}
			HashSet<int> collection = characterSet.GetCollection();
			if (collection.Count == 0)
			{
				continue;
			}
			foreach (int item2 in collection)
			{
				value.Remove(item2);
			}
			num += collection.Count;
			characterSets[i] = value;
		}
		return num;
	}

	public static bool IsAllCharactersAtLocation(Location location, CharacterFilterRequirement[] filterRequirements, List<CharacterSet> characterSets)
	{
		for (int i = 0; i < filterRequirements.Length; i++)
		{
			CharacterFilterRequirement characterFilterRequirement = filterRequirements[i];
			if (characterSets.Count <= i)
			{
				return false;
			}
			CharacterSet characterSet = characterSets[i];
			int num = 0;
			foreach (int item in characterSet.GetCollection())
			{
				if (DomainManager.Character.TryGetElement_Objects(item, out var element) && element.GetLocation().Equals(location))
				{
					num++;
				}
			}
			if (num < characterFilterRequirement.MinCharactersRequired)
			{
				return false;
			}
		}
		return true;
	}

	public static bool IsAllCharactersAtLocation(Location location, SearchCharacterRule searchRule, List<CharacterSet> characterSets)
	{
		for (int i = 0; i < searchRule.SubRules.Count; i++)
		{
			if (characterSets.Count <= i)
			{
				return false;
			}
			CharacterSet characterSet = characterSets[i];
			int num = 0;
			foreach (int item in characterSet.GetCollection())
			{
				if (DomainManager.Character.TryGetElement_Objects(item, out var element) && element.GetLocation().Equals(location))
				{
					num++;
				}
			}
			if (num < searchRule.SubRules[i].MinAmount)
			{
				return false;
			}
		}
		return true;
	}
}
