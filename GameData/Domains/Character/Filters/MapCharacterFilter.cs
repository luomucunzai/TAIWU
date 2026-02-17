using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using GameData.Domains.Map;
using GameData.Utilities;

namespace GameData.Domains.Character.Filters
{
	// Token: 0x0200083B RID: 2107
	public static class MapCharacterFilter
	{
		// Token: 0x060075B0 RID: 30128 RVA: 0x0044BDCC File Offset: 0x00449FCC
		public static void ParallelFind(Predicate<Character> predicate, List<Character> results, short areaIdBegin = 0, short areaIdEnd = 135, bool includeInfected = false)
		{
			Parallel.For<List<Character>>((int)areaIdBegin, (int)areaIdEnd, () => new List<Character>(), delegate(int areaId, [Nullable(1)] ParallelLoopState loopState, List<Character> localResults)
			{
				MapCharacterFilter.Find(predicate, localResults, (short)areaId, includeInfected);
				return localResults;
			}, delegate(List<Character> localResults)
			{
				List<Character> results2 = results;
				lock (results2)
				{
					results.AddRange(localResults);
				}
			});
		}

		// Token: 0x060075B1 RID: 30129 RVA: 0x0044BE38 File Offset: 0x0044A038
		public static void ParallelFindInfected(Predicate<Character> predicate, List<Character> results, short areaIdBegin = 0, short areaIdEnd = 135)
		{
			Parallel.For<List<Character>>((int)areaIdBegin, (int)areaIdEnd, () => new List<Character>(), delegate(int areaId, [Nullable(1)] ParallelLoopState loopState, List<Character> localResults)
			{
				MapCharacterFilter.FindInfected(predicate, localResults, (short)areaId);
				return localResults;
			}, delegate(List<Character> localResults)
			{
				List<Character> results2 = results;
				lock (results2)
				{
					results.AddRange(localResults);
				}
			});
		}

		// Token: 0x060075B2 RID: 30130 RVA: 0x0044BE9C File Offset: 0x0044A09C
		public static void ParallelFind(List<Predicate<Character>> predicates, List<Character> results, short areaIdBegin = 0, short areaIdEnd = 135, bool includeInfected = false)
		{
			Parallel.For<List<Character>>((int)areaIdBegin, (int)areaIdEnd, () => new List<Character>(), delegate(int areaId, [Nullable(1)] ParallelLoopState loopState, List<Character> localResults)
			{
				MapCharacterFilter.Find(predicates, localResults, (short)areaId, includeInfected);
				return localResults;
			}, delegate(List<Character> localResults)
			{
				List<Character> results2 = results;
				lock (results2)
				{
					results.AddRange(localResults);
				}
			});
		}

		// Token: 0x060075B3 RID: 30131 RVA: 0x0044BF08 File Offset: 0x0044A108
		public static void ParallelFindInfected(List<Predicate<Character>> predicates, List<Character> results, short areaIdBegin = 0, short areaIdEnd = 135)
		{
			Parallel.For<List<Character>>((int)areaIdBegin, (int)areaIdEnd, () => new List<Character>(), delegate(int areaId, [Nullable(1)] ParallelLoopState loopState, List<Character> localResults)
			{
				MapCharacterFilter.FindInfected(predicates, localResults, (short)areaId);
				return localResults;
			}, delegate(List<Character> localResults)
			{
				List<Character> results2 = results;
				lock (results2)
				{
					results.AddRange(localResults);
				}
			});
		}

		// Token: 0x060075B4 RID: 30132 RVA: 0x0044BF6C File Offset: 0x0044A16C
		public static void ParallelFind(List<Predicate<Character>> predicates, List<Character> results, List<short> areaIdList, bool includeInfected = false)
		{
			Parallel.ForEach<short, List<Character>>(areaIdList, () => new List<Character>(), delegate(short areaId, [Nullable(1)] ParallelLoopState loopState, List<Character> localResults)
			{
				MapCharacterFilter.Find(predicates, localResults, areaId, includeInfected);
				return localResults;
			}, delegate(List<Character> localResults)
			{
				List<Character> results2 = results;
				lock (results2)
				{
					results.AddRange(localResults);
				}
			});
		}

		// Token: 0x060075B5 RID: 30133 RVA: 0x0044BFD4 File Offset: 0x0044A1D4
		public static void ParallelFind(Predicate<Character> predicate, List<Character> results, List<short> areaIdList, bool includeInfected = false)
		{
			Parallel.ForEach<short, List<Character>>(areaIdList, () => new List<Character>(), delegate(short areaId, [Nullable(1)] ParallelLoopState loopState, List<Character> localResults)
			{
				MapCharacterFilter.Find(predicate, localResults, areaId, includeInfected);
				return localResults;
			}, delegate(List<Character> localResults)
			{
				List<Character> results2 = results;
				lock (results2)
				{
					results.AddRange(localResults);
				}
			});
		}

		// Token: 0x060075B6 RID: 30134 RVA: 0x0044C03C File Offset: 0x0044A23C
		public static void ParallelFindInfected(List<Predicate<Character>> predicates, List<Character> results, List<short> areaIdList)
		{
			Parallel.ForEach<short, List<Character>>(areaIdList, () => new List<Character>(), delegate(short areaId, [Nullable(1)] ParallelLoopState loopState, List<Character> localResults)
			{
				MapCharacterFilter.FindInfected(predicates, localResults, areaId);
				return localResults;
			}, delegate(List<Character> localResults)
			{
				List<Character> results2 = results;
				lock (results2)
				{
					results.AddRange(localResults);
				}
			});
		}

		// Token: 0x060075B7 RID: 30135 RVA: 0x0044C09C File Offset: 0x0044A29C
		public unsafe static void Find(Predicate<Character> predicate, List<Character> results, short areaId, bool includeInfected = false)
		{
			Span<MapBlockData> blocks = DomainManager.Map.GetAreaBlocks(areaId);
			int i = 0;
			int blocksCount = blocks.Length;
			while (i < blocksCount)
			{
				HashSet<int> charIds = blocks[i]->CharacterSet;
				bool flag = charIds != null;
				if (flag)
				{
					foreach (int charId in charIds)
					{
						Character character = DomainManager.Character.GetElement_Objects(charId);
						bool flag2 = predicate(character);
						if (flag2)
						{
							results.Add(character);
						}
					}
				}
				bool flag3 = !includeInfected;
				if (!flag3)
				{
					charIds = blocks[i]->InfectedCharacterSet;
					bool flag4 = charIds == null;
					if (!flag4)
					{
						foreach (int charId2 in charIds)
						{
							Character character2 = DomainManager.Character.GetElement_Objects(charId2);
							bool flag5 = predicate(character2);
							if (flag5)
							{
								results.Add(character2);
							}
						}
					}
				}
				i++;
			}
		}

		// Token: 0x060075B8 RID: 30136 RVA: 0x0044C1E0 File Offset: 0x0044A3E0
		public unsafe static void FindInfected(Predicate<Character> predicate, List<Character> results, short areaId)
		{
			Span<MapBlockData> blocks = DomainManager.Map.GetAreaBlocks(areaId);
			int i = 0;
			int blocksCount = blocks.Length;
			while (i < blocksCount)
			{
				HashSet<int> charIds = blocks[i]->InfectedCharacterSet;
				bool flag = charIds == null;
				if (!flag)
				{
					foreach (int charId in charIds)
					{
						Character character = DomainManager.Character.GetElement_Objects(charId);
						bool flag2 = predicate(character);
						if (flag2)
						{
							results.Add(character);
						}
					}
				}
				i++;
			}
		}

		// Token: 0x060075B9 RID: 30137 RVA: 0x0044C298 File Offset: 0x0044A498
		public unsafe static void FindStateInfected(Predicate<Character> predicate, List<Character> results, sbyte stateId)
		{
			List<short> areaList = ObjectPool<List<short>>.Instance.Get();
			DomainManager.Map.GetAllAreaInState(stateId, areaList);
			for (int i = 0; i < areaList.Count; i++)
			{
				Span<MapBlockData> blocks = DomainManager.Map.GetAreaBlocks(areaList[i]);
				int j = 0;
				int blocksCount = blocks.Length;
				while (j < blocksCount)
				{
					HashSet<int> charIds = blocks[j]->InfectedCharacterSet;
					bool flag = charIds == null;
					if (!flag)
					{
						foreach (int charId in charIds)
						{
							Character character = DomainManager.Character.GetElement_Objects(charId);
							bool flag2 = predicate(character);
							if (flag2)
							{
								results.Add(character);
							}
						}
					}
					j++;
				}
			}
			ObjectPool<List<short>>.Instance.Return(areaList);
		}

		// Token: 0x060075BA RID: 30138 RVA: 0x0044C3A0 File Offset: 0x0044A5A0
		public unsafe static void Find(List<Predicate<Character>> predicates, List<Character> results, short areaId, bool includeInfected = false)
		{
			Span<MapBlockData> blocks = DomainManager.Map.GetAreaBlocks(areaId);
			int i = 0;
			int blocksCount = blocks.Length;
			while (i < blocksCount)
			{
				HashSet<int> charIds = blocks[i]->CharacterSet;
				bool flag = charIds != null;
				if (flag)
				{
					foreach (int charId in charIds)
					{
						Character character = DomainManager.Character.GetElement_Objects(charId);
						bool flag2 = CharacterMatchers.MatchAll(character, predicates);
						if (flag2)
						{
							results.Add(character);
						}
					}
				}
				bool flag3 = !includeInfected;
				if (!flag3)
				{
					charIds = blocks[i]->InfectedCharacterSet;
					bool flag4 = charIds == null;
					if (!flag4)
					{
						foreach (int charId2 in charIds)
						{
							Character character2 = DomainManager.Character.GetElement_Objects(charId2);
							bool flag5 = CharacterMatchers.MatchAll(character2, predicates);
							if (flag5)
							{
								results.Add(character2);
							}
						}
					}
				}
				i++;
			}
		}

		// Token: 0x060075BB RID: 30139 RVA: 0x0044C4E4 File Offset: 0x0044A6E4
		public static void FindTraveling(List<Predicate<Character>> predicates, List<Character> results, bool includeInfected = false)
		{
			List<int> charIds = ObjectPool<List<int>>.Instance.Get();
			DomainManager.Character.GetCrossAreaTravelingCharacterIds(charIds);
			foreach (int charId in charIds)
			{
				Character character = DomainManager.Character.GetElement_Objects(charId);
				bool flag = character.IsCompletelyInfected() && !includeInfected;
				if (!flag)
				{
					bool flag2 = character.GetLeaderId() == charId;
					if (flag2)
					{
						HashSet<int> collection = DomainManager.Character.GetGroup(charId).GetCollection();
						foreach (int groupCharId in collection)
						{
							Character groupChar = DomainManager.Character.GetElement_Objects(groupCharId);
							bool flag3 = CharacterMatchers.MatchAll(groupChar, predicates);
							if (flag3)
							{
								results.Add(groupChar);
							}
						}
					}
					else
					{
						bool flag4 = CharacterMatchers.MatchAll(character, predicates);
						if (flag4)
						{
							results.Add(character);
						}
					}
				}
			}
			ObjectPool<List<int>>.Instance.Return(charIds);
		}

		// Token: 0x060075BC RID: 30140 RVA: 0x0044C620 File Offset: 0x0044A820
		public static void FindTraveling(Predicate<Character> predicate, List<Character> results, bool includeInfected = false)
		{
			List<int> charIds = ObjectPool<List<int>>.Instance.Get();
			DomainManager.Character.GetCrossAreaTravelingCharacterIds(charIds);
			foreach (int charId in charIds)
			{
				Character character;
				bool flag = !DomainManager.Character.TryGetElement_Objects(charId, out character);
				if (!flag)
				{
					bool flag2 = character.IsCompletelyInfected() && !includeInfected;
					if (!flag2)
					{
						bool flag3 = character.GetLeaderId() == charId;
						if (flag3)
						{
							HashSet<int> collection = DomainManager.Character.GetGroup(charId).GetCollection();
							foreach (int groupCharId in collection)
							{
								Character groupChar = DomainManager.Character.GetElement_Objects(groupCharId);
								bool flag4 = predicate(groupChar);
								if (flag4)
								{
									results.Add(groupChar);
								}
							}
						}
						else
						{
							bool flag5 = predicate(character);
							if (flag5)
							{
								results.Add(character);
							}
						}
					}
				}
			}
			ObjectPool<List<int>>.Instance.Return(charIds);
		}

		// Token: 0x060075BD RID: 30141 RVA: 0x0044C76C File Offset: 0x0044A96C
		public static void FindHiddenCharacters(Predicate<Character> predicate, List<Character> results, bool includeInfected = false)
		{
			HashSet<int> charIds = ObjectPool<HashSet<int>>.Instance.Get();
			DomainManager.TaiwuEvent.CollectUnreleasedCalledCharacters(charIds);
			foreach (int charId in charIds)
			{
				Character character;
				bool flag = !DomainManager.Character.TryGetElement_Objects(charId, out character);
				if (!flag)
				{
					bool flag2 = character.IsCompletelyInfected() && !includeInfected;
					if (!flag2)
					{
						bool flag3 = !character.IsActiveExternalRelationState(60);
						if (!flag3)
						{
							bool flag4 = character.GetKidnapperId() >= 0;
							if (!flag4)
							{
								Location location = character.GetLocation();
								bool flag5 = location.IsValid();
								if (flag5)
								{
									MapBlockData block = DomainManager.Map.GetBlock(location);
									bool flag6 = block.CharacterSet != null && block.CharacterSet.Contains(charId);
									if (flag6)
									{
										continue;
									}
									bool flag7 = block.InfectedCharacterSet != null && block.InfectedCharacterSet.Contains(charId);
									if (flag7)
									{
										continue;
									}
								}
								bool flag8 = predicate(character);
								if (flag8)
								{
									results.Add(character);
								}
							}
						}
					}
				}
			}
			ObjectPool<HashSet<int>>.Instance.Return(charIds);
		}

		// Token: 0x060075BE RID: 30142 RVA: 0x0044C8BC File Offset: 0x0044AABC
		public static void FindKidnappedCharacters(Predicate<Character> predicate, List<Character> results, bool includeInfected = false)
		{
			List<int> charIds = ObjectPool<List<int>>.Instance.Get();
			DomainManager.Character.GetAllKidnappedCharacterIds(charIds);
			foreach (int charId in charIds)
			{
				Character character;
				bool flag = !DomainManager.Character.TryGetElement_Objects(charId, out character);
				if (!flag)
				{
					bool flag2 = character.IsCompletelyInfected() && !includeInfected;
					if (!flag2)
					{
						bool flag3 = predicate(character);
						if (flag3)
						{
							results.Add(character);
						}
					}
				}
			}
			ObjectPool<List<int>>.Instance.Return(charIds);
		}

		// Token: 0x060075BF RID: 30143 RVA: 0x0044C970 File Offset: 0x0044AB70
		public unsafe static void FindInfected(List<Predicate<Character>> predicates, List<Character> results, short areaId)
		{
			Span<MapBlockData> blocks = DomainManager.Map.GetAreaBlocks(areaId);
			int i = 0;
			int blocksCount = blocks.Length;
			while (i < blocksCount)
			{
				HashSet<int> charIds = blocks[i]->InfectedCharacterSet;
				bool flag = charIds == null;
				if (!flag)
				{
					foreach (int charId in charIds)
					{
						Character character = DomainManager.Character.GetElement_Objects(charId);
						bool flag2 = CharacterMatchers.MatchAll(character, predicates);
						if (flag2)
						{
							results.Add(character);
						}
					}
				}
				i++;
			}
		}
	}
}
