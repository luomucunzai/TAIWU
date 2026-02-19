using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GameData.Domains.Map;
using GameData.Utilities;

namespace GameData.Domains.Character.Filters;

public static class MapCharacterFilter
{
	public static void ParallelFind(Predicate<Character> predicate, List<Character> results, short areaIdBegin = 0, short areaIdEnd = 135, bool includeInfected = false)
	{
		Parallel.For(areaIdBegin, areaIdEnd, () => new List<Character>(), delegate(int areaId, ParallelLoopState loopState, List<Character> localResults)
		{
			Find(predicate, localResults, (short)areaId, includeInfected);
			return localResults;
		}, delegate(List<Character> localResults)
		{
			lock (results)
			{
				results.AddRange(localResults);
			}
		});
	}

	public static void ParallelFindInfected(Predicate<Character> predicate, List<Character> results, short areaIdBegin = 0, short areaIdEnd = 135)
	{
		Parallel.For(areaIdBegin, areaIdEnd, () => new List<Character>(), delegate(int areaId, ParallelLoopState loopState, List<Character> localResults)
		{
			FindInfected(predicate, localResults, (short)areaId);
			return localResults;
		}, delegate(List<Character> localResults)
		{
			lock (results)
			{
				results.AddRange(localResults);
			}
		});
	}

	public static void ParallelFind(List<Predicate<Character>> predicates, List<Character> results, short areaIdBegin = 0, short areaIdEnd = 135, bool includeInfected = false)
	{
		Parallel.For(areaIdBegin, areaIdEnd, () => new List<Character>(), delegate(int areaId, ParallelLoopState loopState, List<Character> localResults)
		{
			Find(predicates, localResults, (short)areaId, includeInfected);
			return localResults;
		}, delegate(List<Character> localResults)
		{
			lock (results)
			{
				results.AddRange(localResults);
			}
		});
	}

	public static void ParallelFindInfected(List<Predicate<Character>> predicates, List<Character> results, short areaIdBegin = 0, short areaIdEnd = 135)
	{
		Parallel.For(areaIdBegin, areaIdEnd, () => new List<Character>(), delegate(int areaId, ParallelLoopState loopState, List<Character> localResults)
		{
			FindInfected(predicates, localResults, (short)areaId);
			return localResults;
		}, delegate(List<Character> localResults)
		{
			lock (results)
			{
				results.AddRange(localResults);
			}
		});
	}

	public static void ParallelFind(List<Predicate<Character>> predicates, List<Character> results, List<short> areaIdList, bool includeInfected = false)
	{
		Parallel.ForEach(areaIdList, () => new List<Character>(), delegate(short areaId, ParallelLoopState loopState, List<Character> localResults)
		{
			Find(predicates, localResults, areaId, includeInfected);
			return localResults;
		}, delegate(List<Character> localResults)
		{
			lock (results)
			{
				results.AddRange(localResults);
			}
		});
	}

	public static void ParallelFind(Predicate<Character> predicate, List<Character> results, List<short> areaIdList, bool includeInfected = false)
	{
		Parallel.ForEach(areaIdList, () => new List<Character>(), delegate(short areaId, ParallelLoopState loopState, List<Character> localResults)
		{
			Find(predicate, localResults, areaId, includeInfected);
			return localResults;
		}, delegate(List<Character> localResults)
		{
			lock (results)
			{
				results.AddRange(localResults);
			}
		});
	}

	public static void ParallelFindInfected(List<Predicate<Character>> predicates, List<Character> results, List<short> areaIdList)
	{
		Parallel.ForEach(areaIdList, () => new List<Character>(), delegate(short areaId, ParallelLoopState loopState, List<Character> localResults)
		{
			FindInfected(predicates, localResults, areaId);
			return localResults;
		}, delegate(List<Character> localResults)
		{
			lock (results)
			{
				results.AddRange(localResults);
			}
		});
	}

	public static void Find(Predicate<Character> predicate, List<Character> results, short areaId, bool includeInfected = false)
	{
		Span<MapBlockData> areaBlocks = DomainManager.Map.GetAreaBlocks(areaId);
		int i = 0;
		for (int length = areaBlocks.Length; i < length; i++)
		{
			HashSet<int> characterSet = areaBlocks[i].CharacterSet;
			if (characterSet != null)
			{
				foreach (int item in characterSet)
				{
					Character element_Objects = DomainManager.Character.GetElement_Objects(item);
					if (predicate(element_Objects))
					{
						results.Add(element_Objects);
					}
				}
			}
			if (!includeInfected)
			{
				continue;
			}
			characterSet = areaBlocks[i].InfectedCharacterSet;
			if (characterSet == null)
			{
				continue;
			}
			foreach (int item2 in characterSet)
			{
				Character element_Objects2 = DomainManager.Character.GetElement_Objects(item2);
				if (predicate(element_Objects2))
				{
					results.Add(element_Objects2);
				}
			}
		}
	}

	public static void FindInfected(Predicate<Character> predicate, List<Character> results, short areaId)
	{
		Span<MapBlockData> areaBlocks = DomainManager.Map.GetAreaBlocks(areaId);
		int i = 0;
		for (int length = areaBlocks.Length; i < length; i++)
		{
			HashSet<int> infectedCharacterSet = areaBlocks[i].InfectedCharacterSet;
			if (infectedCharacterSet == null)
			{
				continue;
			}
			foreach (int item in infectedCharacterSet)
			{
				Character element_Objects = DomainManager.Character.GetElement_Objects(item);
				if (predicate(element_Objects))
				{
					results.Add(element_Objects);
				}
			}
		}
	}

	public static void FindStateInfected(Predicate<Character> predicate, List<Character> results, sbyte stateId)
	{
		List<short> list = ObjectPool<List<short>>.Instance.Get();
		DomainManager.Map.GetAllAreaInState(stateId, list);
		for (int i = 0; i < list.Count; i++)
		{
			Span<MapBlockData> areaBlocks = DomainManager.Map.GetAreaBlocks(list[i]);
			int j = 0;
			for (int length = areaBlocks.Length; j < length; j++)
			{
				HashSet<int> infectedCharacterSet = areaBlocks[j].InfectedCharacterSet;
				if (infectedCharacterSet == null)
				{
					continue;
				}
				foreach (int item in infectedCharacterSet)
				{
					Character element_Objects = DomainManager.Character.GetElement_Objects(item);
					if (predicate(element_Objects))
					{
						results.Add(element_Objects);
					}
				}
			}
		}
		ObjectPool<List<short>>.Instance.Return(list);
	}

	public static void Find(List<Predicate<Character>> predicates, List<Character> results, short areaId, bool includeInfected = false)
	{
		Span<MapBlockData> areaBlocks = DomainManager.Map.GetAreaBlocks(areaId);
		int i = 0;
		for (int length = areaBlocks.Length; i < length; i++)
		{
			HashSet<int> characterSet = areaBlocks[i].CharacterSet;
			if (characterSet != null)
			{
				foreach (int item in characterSet)
				{
					Character element_Objects = DomainManager.Character.GetElement_Objects(item);
					if (CharacterMatchers.MatchAll(element_Objects, predicates))
					{
						results.Add(element_Objects);
					}
				}
			}
			if (!includeInfected)
			{
				continue;
			}
			characterSet = areaBlocks[i].InfectedCharacterSet;
			if (characterSet == null)
			{
				continue;
			}
			foreach (int item2 in characterSet)
			{
				Character element_Objects2 = DomainManager.Character.GetElement_Objects(item2);
				if (CharacterMatchers.MatchAll(element_Objects2, predicates))
				{
					results.Add(element_Objects2);
				}
			}
		}
	}

	public static void FindTraveling(List<Predicate<Character>> predicates, List<Character> results, bool includeInfected = false)
	{
		List<int> list = ObjectPool<List<int>>.Instance.Get();
		DomainManager.Character.GetCrossAreaTravelingCharacterIds(list);
		foreach (int item in list)
		{
			Character element_Objects = DomainManager.Character.GetElement_Objects(item);
			if (element_Objects.IsCompletelyInfected() && !includeInfected)
			{
				continue;
			}
			if (element_Objects.GetLeaderId() == item)
			{
				HashSet<int> collection = DomainManager.Character.GetGroup(item).GetCollection();
				foreach (int item2 in collection)
				{
					Character element_Objects2 = DomainManager.Character.GetElement_Objects(item2);
					if (CharacterMatchers.MatchAll(element_Objects2, predicates))
					{
						results.Add(element_Objects2);
					}
				}
			}
			else if (CharacterMatchers.MatchAll(element_Objects, predicates))
			{
				results.Add(element_Objects);
			}
		}
		ObjectPool<List<int>>.Instance.Return(list);
	}

	public static void FindTraveling(Predicate<Character> predicate, List<Character> results, bool includeInfected = false)
	{
		List<int> list = ObjectPool<List<int>>.Instance.Get();
		DomainManager.Character.GetCrossAreaTravelingCharacterIds(list);
		foreach (int item in list)
		{
			if (!DomainManager.Character.TryGetElement_Objects(item, out var element) || (element.IsCompletelyInfected() && !includeInfected))
			{
				continue;
			}
			if (element.GetLeaderId() == item)
			{
				HashSet<int> collection = DomainManager.Character.GetGroup(item).GetCollection();
				foreach (int item2 in collection)
				{
					Character element_Objects = DomainManager.Character.GetElement_Objects(item2);
					if (predicate(element_Objects))
					{
						results.Add(element_Objects);
					}
				}
			}
			else if (predicate(element))
			{
				results.Add(element);
			}
		}
		ObjectPool<List<int>>.Instance.Return(list);
	}

	public static void FindHiddenCharacters(Predicate<Character> predicate, List<Character> results, bool includeInfected = false)
	{
		HashSet<int> hashSet = ObjectPool<HashSet<int>>.Instance.Get();
		DomainManager.TaiwuEvent.CollectUnreleasedCalledCharacters(hashSet);
		foreach (int item in hashSet)
		{
			if (!DomainManager.Character.TryGetElement_Objects(item, out var element) || (element.IsCompletelyInfected() && !includeInfected) || !element.IsActiveExternalRelationState(60) || element.GetKidnapperId() >= 0)
			{
				continue;
			}
			Location location = element.GetLocation();
			if (location.IsValid())
			{
				MapBlockData block = DomainManager.Map.GetBlock(location);
				if ((block.CharacterSet != null && block.CharacterSet.Contains(item)) || (block.InfectedCharacterSet != null && block.InfectedCharacterSet.Contains(item)))
				{
					continue;
				}
			}
			if (predicate(element))
			{
				results.Add(element);
			}
		}
		ObjectPool<HashSet<int>>.Instance.Return(hashSet);
	}

	public static void FindKidnappedCharacters(Predicate<Character> predicate, List<Character> results, bool includeInfected = false)
	{
		List<int> list = ObjectPool<List<int>>.Instance.Get();
		DomainManager.Character.GetAllKidnappedCharacterIds(list);
		foreach (int item in list)
		{
			if (DomainManager.Character.TryGetElement_Objects(item, out var element) && (!element.IsCompletelyInfected() || includeInfected) && predicate(element))
			{
				results.Add(element);
			}
		}
		ObjectPool<List<int>>.Instance.Return(list);
	}

	public static void FindInfected(List<Predicate<Character>> predicates, List<Character> results, short areaId)
	{
		Span<MapBlockData> areaBlocks = DomainManager.Map.GetAreaBlocks(areaId);
		int i = 0;
		for (int length = areaBlocks.Length; i < length; i++)
		{
			HashSet<int> infectedCharacterSet = areaBlocks[i].InfectedCharacterSet;
			if (infectedCharacterSet == null)
			{
				continue;
			}
			foreach (int item in infectedCharacterSet)
			{
				Character element_Objects = DomainManager.Character.GetElement_Objects(item);
				if (CharacterMatchers.MatchAll(element_Objects, predicates))
				{
					results.Add(element_Objects);
				}
			}
		}
	}
}
