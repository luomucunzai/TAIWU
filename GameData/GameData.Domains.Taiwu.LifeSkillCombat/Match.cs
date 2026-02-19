using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Config;
using GameData.Common;
using GameData.Domains.Character;
using GameData.Domains.Information;
using GameData.Domains.Item.Display;
using GameData.Domains.Taiwu.LifeSkillCombat.Operation;
using GameData.Domains.Taiwu.LifeSkillCombat.Snapshot;
using GameData.Domains.Taiwu.LifeSkillCombat.Status;
using GameData.Serializer;
using GameData.Utilities;
using Redzen.Random;

namespace GameData.Domains.Taiwu.LifeSkillCombat;

public class Match
{
	public class Ai
	{
		private readonly sbyte _playerId;

		private readonly Dictionary<sbyte, int> _opponentPreciselyBookPointImpressions = new Dictionary<sbyte, int>();

		private readonly Dictionary<sbyte, SortedSet<int>> _opponentVolatileBookPointImpressions = new Dictionary<sbyte, SortedSet<int>>();

		private readonly HashSet<int> _ignoredOperations = new HashSet<int>();

		private readonly HashSet<sbyte> _banCardTemplateIds = new HashSet<sbyte>();

		private sbyte _behaviourTypeStyle = -1;

		private bool _canUseForceAdversary;

		public bool AlwaysUseForceAdversary;

		internal static readonly Dictionary<ELifeSkillCombatEffectType, ELifeSkillCombatEffectGroup[]> LifeSkillCombatEffectTypeGroups = new Dictionary<ELifeSkillCombatEffectType, ELifeSkillCombatEffectGroup[]>
		{
			{
				ELifeSkillCombatEffectType.Common,
				new ELifeSkillCombatEffectGroup[3]
				{
					ELifeSkillCombatEffectGroup.ExtractCard,
					ELifeSkillCombatEffectGroup.FlexibleFall,
					ELifeSkillCombatEffectGroup.EliminateArgument
				}
			},
			{
				ELifeSkillCombatEffectType.BUFF,
				new ELifeSkillCombatEffectGroup[2]
				{
					ELifeSkillCombatEffectGroup.ReinforceArgument,
					ELifeSkillCombatEffectGroup.WeakenArgument
				}
			},
			{
				ELifeSkillCombatEffectType.Strategy,
				new ELifeSkillCombatEffectGroup[2]
				{
					ELifeSkillCombatEffectGroup.RemoveEffect,
					ELifeSkillCombatEffectGroup.DissembleArgument
				}
			},
			{
				ELifeSkillCombatEffectType.Cooling,
				new ELifeSkillCombatEffectGroup[3]
				{
					ELifeSkillCombatEffectGroup.ReduceCooling,
					ELifeSkillCombatEffectGroup.AddCooling,
					ELifeSkillCombatEffectGroup.CompleteCooling
				}
			},
			{
				ELifeSkillCombatEffectType.Special,
				new ELifeSkillCombatEffectGroup[3]
				{
					ELifeSkillCombatEffectGroup.RecycleCard,
					ELifeSkillCombatEffectGroup.QuickArgument,
					ELifeSkillCombatEffectGroup.CaptureArgument
				}
			}
		};

		internal static readonly Dictionary<ELifeSkillCombatEffectGroup, sbyte[]> LifeSkillCombatEffectGroupPersonalityTypes = new Dictionary<ELifeSkillCombatEffectGroup, sbyte[]>
		{
			{
				ELifeSkillCombatEffectGroup.ExtractCard,
				new sbyte[1]
			},
			{
				ELifeSkillCombatEffectGroup.FlexibleFall,
				new sbyte[1] { 1 }
			},
			{
				ELifeSkillCombatEffectGroup.EliminateArgument,
				new sbyte[1] { 3 }
			},
			{
				ELifeSkillCombatEffectGroup.ReinforceArgument,
				new sbyte[2] { 2, 1 }
			},
			{
				ELifeSkillCombatEffectGroup.WeakenArgument,
				new sbyte[2] { 3, 4 }
			},
			{
				ELifeSkillCombatEffectGroup.RemoveEffect,
				new sbyte[1]
			},
			{
				ELifeSkillCombatEffectGroup.DissembleArgument,
				new sbyte[1] { 1 }
			},
			{
				ELifeSkillCombatEffectGroup.ReduceCooling,
				new sbyte[1] { 2 }
			},
			{
				ELifeSkillCombatEffectGroup.AddCooling,
				new sbyte[1] { 4 }
			},
			{
				ELifeSkillCombatEffectGroup.CompleteCooling,
				new sbyte[1] { 3 }
			},
			{
				ELifeSkillCombatEffectGroup.RecycleCard,
				new sbyte[4] { 0, 2, 1, 4 }
			},
			{
				ELifeSkillCombatEffectGroup.QuickArgument,
				new sbyte[4] { 0, 2, 1, 4 }
			},
			{
				ELifeSkillCombatEffectGroup.CaptureArgument,
				new sbyte[4] { 0, 2, 1, 4 }
			}
		};

		public Ai(Match match)
		{
			Player player = match.GetPlayer(match._currentPlayerId);
			GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(player.CharacterId);
			ChangeBehaviourTypeStyle(element_Objects.GetBehaviorType(), match);
			_playerId = player.Id;
			AlwaysUseForceAdversary = false;
		}

		public void TurnClear()
		{
			_canUseForceAdversary = true;
		}

		public void ChangeBehaviourTypeStyle(sbyte behaviourType, Match match)
		{
			_behaviourTypeStyle = behaviourType;
			match.RecordLineAi("BehaviourType changed to " + Config.BehaviorType.Instance[_behaviourTypeStyle].Name);
		}

		public void InferringForOpponentBookPoints(IRandomSource random, Match match)
		{
			sbyte currentPlayerId = match._currentPlayerId;
			sbyte theOtherSide = Player.PredefinedId.GetTheOtherSide(currentPlayerId);
			Player player = match.GetPlayer(theOtherSide);
			Dictionary<sbyte, SortedSet<int>> dictionary = new Dictionary<sbyte, SortedSet<int>>();
			for (sbyte b = 0; b < 9; b++)
			{
				if (TryGetOpponentBookPoint(match, b, out var points))
				{
					SortedSet<int> sortedSet = new SortedSet<int>();
					sortedSet.UnionWith(points);
					dictionary.Add(b, sortedSet);
				}
			}
			HashSet<sbyte> hashSet = new HashSet<sbyte>();
			Grid[] gridStatus = match._gridStatus;
			foreach (Grid grid in gridStatus)
			{
				OperationGridBase activeOperation = grid.ActiveOperation;
				if (activeOperation != null || !(grid.LastHistoryOperation is OperationPointBase operationPointBase) || operationPointBase.PlayerId == currentPlayerId)
				{
					continue;
				}
				if (operationPointBase.EffectiveBookStates.Count == 1)
				{
					_opponentPreciselyBookPointImpressions[player.RefBookIndex(operationPointBase.EffectiveBookStates[0])] = operationPointBase.Point;
				}
				int num = operationPointBase.Point;
				hashSet.Clear();
				foreach (Book effectiveBookState in operationPointBase.EffectiveBookStates)
				{
					hashSet.Add(player.RefBookIndex(effectiveBookState));
				}
				if (hashSet.Count == 0 || _ignoredOperations.Contains(operationPointBase.GridIndex))
				{
					continue;
				}
				_ignoredOperations.Add(operationPointBase.GridIndex);
				foreach (var (item, num3) in _opponentPreciselyBookPointImpressions)
				{
					if (hashSet.Contains(item))
					{
						hashSet.Remove(item);
						num -= num3;
					}
				}
				num = Math.Max(num, 0);
				if (hashSet.Count == 1)
				{
					foreach (sbyte item2 in hashSet)
					{
						_opponentPreciselyBookPointImpressions[item2] = num;
					}
					continue;
				}
				foreach (sbyte item3 in hashSet)
				{
					if (!_opponentVolatileBookPointImpressions.TryGetValue(item3, out var value))
					{
						_opponentVolatileBookPointImpressions.Add(item3, value = new SortedSet<int>());
					}
					value.Add(num / hashSet.Count);
				}
			}
			match.RecordLineAi("Recorded book point after this time inferring：");
			for (sbyte b3 = 0; b3 < 9; b3++)
			{
				SortedSet<int> value2;
				bool flag = dictionary.TryGetValue(b3, out value2);
				SortedSet<int> points2;
				bool flag2 = TryGetOpponentBookPoint(match, b3, out points2);
				string value3 = (flag ? string.Join(", ", dictionary[b3]) : string.Empty);
				string value4 = (flag2 ? string.Join(", ", points2) : string.Empty);
				match.RecordLineAi($"{LifeSkill.Instance[player.BookStates[b3].LifeSkill.SkillTemplateId].Name} {{{value3}}} -> {{{value4}}}", 1);
			}
		}

		public bool TryGetOpponentBookPoint(Match match, sbyte bookIndex, out SortedSet<int> points)
		{
			if (_opponentPreciselyBookPointImpressions.TryGetValue(bookIndex, out var value))
			{
				_opponentVolatileBookPointImpressions.Remove(bookIndex);
				points = new SortedSet<int>();
				points.Add(value);
				return true;
			}
			if (_opponentVolatileBookPointImpressions.TryGetValue(bookIndex, out points) && points != null && points.Count > 0)
			{
				return true;
			}
			sbyte theOtherSide = Player.PredefinedId.GetTheOtherSide(match._currentPlayerId);
			GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(match.GetPlayerCharacterId(theOtherSide));
			switch (_behaviourTypeStyle)
			{
			case 0:
				points = new SortedSet<int> { element_Objects.GetLifeSkillAttainment(match.LifeSkillType) * 4 + element_Objects.GetLifeSkillQualification(match.LifeSkillType) };
				return false;
			case 1:
				points = new SortedSet<int> { element_Objects.GetLifeSkillAttainment(match.LifeSkillType) * 5 + element_Objects.GetLifeSkillQualification(match.LifeSkillType) };
				return false;
			case 2:
				points = new SortedSet<int> { element_Objects.GetLifeSkillAttainment(match.LifeSkillType) * 4 + element_Objects.GetLifeSkillQualification(match.LifeSkillType) };
				return false;
			case 3:
				points = new SortedSet<int> { element_Objects.GetLifeSkillAttainment(match.LifeSkillType) * 3 + element_Objects.GetLifeSkillQualification(match.LifeSkillType) };
				return false;
			case 4:
				points = new SortedSet<int> { element_Objects.GetLifeSkillAttainment(match.LifeSkillType) * 3 + element_Objects.GetLifeSkillQualification(match.LifeSkillType) };
				return false;
			default:
				throw new NotImplementedException();
			}
		}

		public int GetPlayerBookSetPoint(Match match, ushort bookSet, Player player)
		{
			int num = 0;
			IReadOnlyList<Book> bookStates = player.BookStates;
			for (sbyte b = 0; b < 9; b++)
			{
				if (((1 << (int)b) & bookSet) != 0)
				{
					num += bookStates[b].BasePoint;
				}
			}
			return num;
		}

		public int GetCurrentPlayerBookSetPoint(Match match, ushort bookSet)
		{
			return GetPlayerBookSetPoint(match, bookSet, match.GetPlayer(match._currentPlayerId));
		}

		public IEnumerable<Book> GetBookStatesByBookSet(ushort bookSet, Player player)
		{
			IReadOnlyList<Book> bookStates = player.BookStates;
			for (sbyte i = 0; i < 9; i++)
			{
				if (((1 << (int)i) & bookSet) != 0)
				{
					yield return bookStates[i];
				}
			}
		}

		public ushort GetOpponentUsableBookSetInNext(Match match)
		{
			Player player = match.GetPlayer(Player.PredefinedId.GetTheOtherSide(match._currentPlayerId));
			IReadOnlyList<Book> bookStates = player.BookStates;
			int num = 0;
			int i = 0;
			for (int num2 = 9; i < num2; i++)
			{
				if (!bookStates[i].IsDisplayCd || bookStates[i].DisplayCd <= 1)
				{
					num |= 1 << i;
				}
			}
			return (ushort)num;
		}

		public List<int> GetOpponentBookSetPossiblePoints(Match match, ushort bookSet)
		{
			List<int> list = new List<int>();
			for (sbyte b = 0; b < 9; b++)
			{
				if (((1 << (int)b) & bookSet) != 0)
				{
					TryGetOpponentBookPoint(match, b, out var points);
					foreach (int item in points)
					{
						list.Add(item);
					}
					int i = 0;
					for (int num = list.Count - points.Count; i < num; i++)
					{
						foreach (int item2 in points)
						{
							list.Add(list[i] + item2);
						}
					}
				}
			}
			list = list.ToHashSet().ToList();
			list.Sort();
			return list;
		}

		public ushort GetOperationBookSets(OperationPointBase pOp, Match match)
		{
			ushort num = 0;
			IReadOnlyList<Book> effectiveBookStates = pOp.EffectiveBookStates;
			IReadOnlyList<Book> bookStates = match.GetPlayer(pOp.PlayerId).BookStates;
			for (int i = 0; i < effectiveBookStates.Count; i++)
			{
				Book book = effectiveBookStates[i];
				for (int j = 0; j < bookStates.Count; j++)
				{
					if (bookStates[j].LifeSkill.SkillTemplateId == book.LifeSkill.SkillTemplateId)
					{
						num |= (ushort)(1 << j);
						break;
					}
				}
			}
			return num;
		}

		public ushort GetOriginalUsableBookSets(Player player)
		{
			IReadOnlyList<Book> bookStates = player.BookStates;
			int num = 0;
			int i = 0;
			for (int num2 = 9; i < num2; i++)
			{
				if (!bookStates[i].IsCd)
				{
					num |= 1 << i;
				}
			}
			return (ushort)num;
		}

		public List<ushort> GetPossibleBookSets(ushort origin)
		{
			List<ushort> list = new List<ushort>();
			int i = 0;
			for (int num = 9; i < num; i++)
			{
				ushort num2 = (ushort)(1 << i);
				if ((num2 & origin) != 0)
				{
					list.Add(num2);
					int j = 0;
					for (int count = list.Count; j < count; j++)
					{
						list.Add((ushort)(list[j] | num2));
					}
				}
			}
			return list.ToHashSet().ToList();
		}

		public List<ushort> GetUsableBookSets(Player player)
		{
			ushort originalUsableBookSets = GetOriginalUsableBookSets(player);
			return GetPossibleBookSets(originalUsableBookSets);
		}

		public OperationBase GiveDecidedOperation(DataContext context, Match match)
		{
			sbyte playerId = match._currentPlayerId;
			sbyte theOtherSide = Player.PredefinedId.GetTheOtherSide(playerId);
			Player player = match.GetPlayer(playerId);
			Player opponentPlayer = match.GetPlayer(theOtherSide);
			GameData.Domains.Character.Character character = DomainManager.Character.GetElement_Objects(match.GetPlayerCharacterId(playerId));
			IRandomSource random = context.Random;
			InferringForOpponentBookPoints(random, match);
			switch (character.GetBehaviorType())
			{
			case 0:
				if (_behaviourTypeStyle != 4 && match.CalcPlayerScore(theOtherSide) - match.CalcPlayerScore(playerId) >= 5)
				{
					ChangeBehaviourTypeStyle(4, match);
				}
				else if (_behaviourTypeStyle == 4 && match.CalcPlayerScore(playerId) >= match.CalcPlayerScore(theOtherSide))
				{
					ChangeBehaviourTypeStyle(0, match);
				}
				break;
			case 4:
				if (_behaviourTypeStyle != 0 && match.CalcPlayerScore(playerId) - match.CalcPlayerScore(theOtherSide) >= 5)
				{
					ChangeBehaviourTypeStyle(0, match);
				}
				else if (_behaviourTypeStyle == 0 && match.CalcPlayerScore(theOtherSide) >= match.CalcPlayerScore(playerId))
				{
					ChangeBehaviourTypeStyle(4, match);
				}
				break;
			}
			List<sbyte> additionalEffectCardTemplateIds = new List<sbyte>();
			Dictionary<ELifeSkillCombatEffectGroup, int> ratePerGroup = GetCardRatePerGroup();
			_banCardTemplateIds.Clear();
			List<sbyte> list = new List<sbyte>();
			list.AddRange(player.EffectCards);
			CollectionUtils.Shuffle(random, list);
			if (random.CheckPercentProb(ratePerGroup[ELifeSkillCombatEffectGroup.FlexibleFall]))
			{
				ratePerGroup[ELifeSkillCombatEffectGroup.FlexibleFall] = 0;
				foreach (sbyte item6 in list)
				{
					LifeSkillCombatEffectItem lifeSkillCombatEffectItem = LifeSkillCombatEffect.Instance[item6];
					if (lifeSkillCombatEffectItem.Group == ELifeSkillCombatEffectGroup.FlexibleFall && TryUseCardDone(item6))
					{
						break;
					}
				}
			}
			List<ELifeSkillCombatEffectGroup> list2 = new List<ELifeSkillCombatEffectGroup>();
			list2.AddRange(ratePerGroup.Keys);
			CollectionUtils.Shuffle(random, list2);
			foreach (ELifeSkillCombatEffectGroup item7 in list2)
			{
				if (!random.CheckPercentProb(ratePerGroup[item7]))
				{
					continue;
				}
				foreach (sbyte item8 in list)
				{
					LifeSkillCombatEffectItem lifeSkillCombatEffectItem2 = LifeSkillCombatEffect.Instance[item8];
					if (lifeSkillCombatEffectItem2.Group == item7 && TryUseCardDone(item8))
					{
						break;
					}
				}
			}
			bool maybeHasAdditionalOperation;
			OperationList operationList = match.CalcUsableOperationList(additionalEffectCardTemplateIds, Array.Empty<sbyte>(), Array.Empty<int>(), out maybeHasAdditionalOperation, exceptIncompleteMatchingEffectCards: false);
			List<OperationPointBase> usablePopList = new List<OperationPointBase>();
			foreach (OperationBase item9 in operationList)
			{
				if (item9 is OperationPointBase item)
				{
					usablePopList.Add(item);
				}
			}
			if (_canUseForceAdversary)
			{
				_canUseForceAdversary = false;
				OperationBase operationBase = operationList.FirstOrDefault((OperationBase p) => p is OperationPrepareForceAdversary operationPrepareForceAdversary && operationPrepareForceAdversary.Type == OperationPrepareForceAdversary.ForceAdversaryOperation.Silent);
				if (operationBase != null)
				{
					int num = 0;
					switch (_behaviourTypeStyle)
					{
					case 1:
						num = 1;
						break;
					case 2:
						num = 2;
						break;
					case 3:
						num = 3;
						break;
					case 4:
						num = 5;
						break;
					}
					if (DomainManager.Extra.GetAiActionRateAdjust(player.CharacterId, 11, -1) != 0)
					{
						num = 0;
					}
					if (AlwaysUseForceAdversary)
					{
						num = 100;
					}
					match.RecordLineAi($"forceSilentPrepare prob {num}%");
					if (random.CheckPercentProb(num))
					{
						bool flag = false;
						switch (_behaviourTypeStyle)
						{
						case 1:
							flag = random.CheckPercentProb(70);
							break;
						case 2:
							flag = random.CheckPercentProb(50);
							break;
						case 3:
							flag = random.CheckPercentProb(80);
							break;
						case 4:
							flag = random.CheckPercentProb(40);
							break;
						}
						if (flag)
						{
							SecretInformationDisplayPackage secretInformationDisplayPackage = DomainManager.Taiwu.PickLifeSkillCombatCharacterUseSecretInformation(context, player.CharacterId);
							if (secretInformationDisplayPackage != null && secretInformationDisplayPackage.SecretInformationDisplayDataList.Count > 0)
							{
								player.SetForceSilentRemainingCount(player.ForceSilentRemainingCount - 1);
								match.PlayerForcedSecretInformation[player.Id] = 1;
								return new OperationForceAdversary(playerId, match.PlayerSwitchCount, secretInformationDisplayPackage);
							}
						}
						else
						{
							ItemDisplayData itemDisplayData = DomainManager.Taiwu.PickLifeSkillCombatCharacterUseItem(context, player.CharacterId);
							if (itemDisplayData != null)
							{
								player.SetForceSilentRemainingCount(player.ForceSilentRemainingCount - 1);
								match.PlayerForcedSecretInformation[player.Id] = 2;
								return new OperationForceAdversary(playerId, match.PlayerSwitchCount, itemDisplayData);
							}
						}
					}
				}
			}
			OperationBase operationBase2 = DecideFirstPhase();
			if (operationBase2 is OperationPointBase operationPointBase && operationPointBase.EffectiveEffectCardTemplateIds.Count == 0)
			{
				Dictionary<ELifeSkillCombatEffectGroup, int> dictionary = GetCardRatePerGroup();
				List<sbyte> list3 = new List<sbyte>();
				list3.AddRange(player.EffectCards);
				CollectionUtils.Shuffle(random, list3);
				List<ELifeSkillCombatEffectGroup> list4 = new List<ELifeSkillCombatEffectGroup>();
				list4.AddRange(dictionary.Keys);
				CollectionUtils.Shuffle(random, list4);
				dictionary[ELifeSkillCombatEffectGroup.FlexibleFall] = 0;
				_banCardTemplateIds.Clear();
				foreach (ELifeSkillCombatEffectGroup item10 in list4)
				{
					if (!random.CheckPercentProb(dictionary[item10]))
					{
						continue;
					}
					foreach (sbyte item11 in list3)
					{
						LifeSkillCombatEffectItem lifeSkillCombatEffectItem3 = LifeSkillCombatEffect.Instance[item11];
						if (lifeSkillCombatEffectItem3.Group == item10 && !_banCardTemplateIds.Contains(item11) && operationPointBase.PickEffectiveEffectCards(Enumerable.Repeat(item11, 1)).Contains(item11))
						{
							operationPointBase.RegisterEffectiveEffectCards(item11);
							_banCardTemplateIds.Add(item11);
							_banCardTemplateIds.UnionWith(lifeSkillCombatEffectItem3.BanCardList);
							break;
						}
					}
				}
				match.RecordLineAi("try using card in second phase.");
			}
			return operationBase2;
			OperationPointBase CommitUsedBookStates(OperationPointBase pOp, IList<ushort> usableBookSets, int baseLine)
			{
				Book[] array = ((usableBookSets != null && usableBookSets.Count > 0) ? GetBookStatesByBookSet(usableBookSets.GetRandom(random), player).ToArray() : Array.Empty<Book>());
				int num2 = array.Sum((Book b) => b.BasePoint);
				List<(int, bool, int)> usedCharacterStates = null;
				if (num2 < baseLine)
				{
					foreach (var friendlyCharacterState in player.FriendlyCharacterStates)
					{
						if (friendlyCharacterState.Usable)
						{
							usedCharacterStates = new List<(int, bool, int)> { friendlyCharacterState };
						}
					}
				}
				pOp.ClearUsedBookStates(player);
				pOp.CommitUsedBookStates(array, usedCharacterStates);
				return pOp;
			}
			OperationBase DecideFirstPhase()
			{
				switch (_behaviourTypeStyle)
				{
				case 0:
				{
					usablePopList.Sort(Sort4);
					int num6 = GetCurrentPlayerUsableBookPointMax();
					foreach (OperationPointBase item12 in usablePopList)
					{
						if (item12 is OperationAnswer operationAnswer4)
						{
							GetAnswerTargetPossiblePoints(operationAnswer4, out var _, out var _, out var opponentPossiblePointMax7, out var opponentPossiblePointMedium7);
							if (num6 >= opponentPossiblePointMax7)
							{
								List<ushort> usableBookSets19 = GetUsableBookSets(player);
								if (opponentPossiblePointMax7 > 0)
								{
									usableBookSets19.RemoveAll(delegate(ushort set)
									{
										int currentPlayerBookSetPoint18 = GetCurrentPlayerBookSetPoint(match, set);
										return currentPlayerBookSetPoint18 < opponentPossiblePointMedium7;
									});
								}
								else
								{
									usableBookSets19.Sort((ushort a, ushort b) => GetCurrentPlayerBookSetPoint(match, a).CompareTo(GetCurrentPlayerBookSetPoint(match, b)));
									if (usableBookSets19.Count > 0)
									{
										usableBookSets19.RemoveRange(1, usableBookSets19.Count - 1);
									}
								}
								return CommitUsedBookStates(operationAnswer4, usableBookSets19, opponentPossiblePointMedium7);
							}
							if (num6 >= opponentPossiblePointMedium7)
							{
								List<ushort> usableBookSets20 = GetUsableBookSets(player);
								int usableMaxPoint4 = GetCurrentPlayerBookSetPoint(match, GetOriginalUsableBookSets(player));
								usableBookSets20.RemoveAll(delegate(ushort set)
								{
									int currentPlayerBookSetPoint18 = GetCurrentPlayerBookSetPoint(match, set);
									return currentPlayerBookSetPoint18 < opponentPossiblePointMedium7 || currentPlayerBookSetPoint18 > usableMaxPoint4;
								});
								return CommitUsedBookStates(operationAnswer4, usableBookSets20, opponentPossiblePointMedium7);
							}
						}
						else
						{
							if (item12 is OperationQuestionAdditional result2)
							{
								match.RecordLineAi("deciced flexiblefall");
								return result2;
							}
							if (item12 is OperationQuestion operationQuestion2 && num6 > 0)
							{
								int thesisScore3 = match.GetGrid(operationQuestion2.GridIndex).GetThesisScore();
								if (thesisScore3 == 0)
								{
									List<ushort> usableBookSets21 = GetUsableBookSets(player);
									int currentPlayerBookSetPoint10 = GetCurrentPlayerBookSetPoint(match, GetOriginalUsableBookSets(player));
									int min10 = currentPlayerBookSetPoint10 * 20 / 100;
									int max10 = currentPlayerBookSetPoint10 * 40 / 100;
									usableBookSets21.RemoveAll(delegate(ushort set)
									{
										int currentPlayerBookSetPoint18 = GetCurrentPlayerBookSetPoint(match, set);
										return currentPlayerBookSetPoint18 < min10 || currentPlayerBookSetPoint18 > max10;
									});
									return CommitUsedBookStates(operationQuestion2, usableBookSets21, min10);
								}
								if (thesisScore3 >= 1)
								{
									GetOpponentUsableBookSetInNextPossiblePoints(out var _, out var _, out var opponentPossiblePointMax8, out var opponentPossiblePointMedium8, out var opponentPossiblePointMin4);
									if (num6 < opponentPossiblePointMax8)
									{
										if (num6 >= opponentPossiblePointMedium8)
										{
											operationQuestion2.ClearUsedBookStates(player);
											operationQuestion2.CommitUsedBookStates(GetBookStatesByBookSet(GetOriginalUsableBookSets(player), player), null);
											return operationQuestion2;
										}
										if (num6 >= opponentPossiblePointMin4)
										{
											if (thesisScore3 > 1)
											{
												operationQuestion2.ClearUsedBookStates(player);
												operationQuestion2.CommitUsedBookStates(GetBookStatesByBookSet(GetOriginalUsableBookSets(player), player), null);
												return operationQuestion2;
											}
											if (random.CheckPercentProb(50))
											{
												operationQuestion2.ClearUsedBookStates(player);
												operationQuestion2.CommitUsedBookStates(GetBookStatesByBookSet(GetOriginalUsableBookSets(player), player), null);
												return operationQuestion2;
											}
											List<ushort> usableBookSets22 = GetUsableBookSets(player);
											int currentPlayerBookSetPoint11 = GetCurrentPlayerBookSetPoint(match, GetOriginalUsableBookSets(player));
											int min11 = currentPlayerBookSetPoint11 * 20 / 100;
											int max11 = currentPlayerBookSetPoint11 * 80 / 100;
											usableBookSets22.RemoveAll(delegate(ushort set)
											{
												int currentPlayerBookSetPoint18 = GetCurrentPlayerBookSetPoint(match, set);
												return currentPlayerBookSetPoint18 < min11 || currentPlayerBookSetPoint18 > max11;
											});
											return CommitUsedBookStates(operationQuestion2, usableBookSets22, min11);
										}
										List<ushort> usableBookSets23 = GetUsableBookSets(player);
										int currentPlayerBookSetPoint12 = GetCurrentPlayerBookSetPoint(match, GetOriginalUsableBookSets(player));
										int min12 = currentPlayerBookSetPoint12 * 20 / 100;
										int max12 = currentPlayerBookSetPoint12 * 80 / 100;
										usableBookSets23.RemoveAll(delegate(ushort set)
										{
											int currentPlayerBookSetPoint18 = GetCurrentPlayerBookSetPoint(match, set);
											return currentPlayerBookSetPoint18 < min12 || currentPlayerBookSetPoint18 > max12;
										});
										return CommitUsedBookStates(operationQuestion2, usableBookSets23, min12);
									}
									List<ushort> usableBookSets24 = GetUsableBookSets(player);
									if (opponentPossiblePointMax8 > 0)
									{
										usableBookSets24.RemoveAll(delegate(ushort set)
										{
											int currentPlayerBookSetPoint18 = GetCurrentPlayerBookSetPoint(match, set);
											return currentPlayerBookSetPoint18 < opponentPossiblePointMedium8;
										});
									}
									else
									{
										usableBookSets24.Sort((ushort a, ushort b) => GetCurrentPlayerBookSetPoint(match, a).CompareTo(GetCurrentPlayerBookSetPoint(match, b)));
										if (usableBookSets24.Count > 0)
										{
											usableBookSets24.RemoveRange(1, usableBookSets24.Count - 1);
										}
									}
									if (usableBookSets24.Count > 0)
									{
										int usableBookSetPointMin4 = usableBookSets24.Select((ushort set) => GetCurrentPlayerBookSetPoint(match, set)).Min();
										usableBookSets24.RemoveAll((ushort set) => GetCurrentPlayerBookSetPoint(match, set) > usableBookSetPointMin4);
										return CommitUsedBookStates(operationQuestion2, usableBookSets24, opponentPossiblePointMax8);
									}
								}
							}
						}
					}
					break;
				}
				case 1:
				{
					OperationAnswer operationAnswer5 = (OperationAnswer)usablePopList.FirstOrDefault((OperationPointBase operationPointBase2) => operationPointBase2 is OperationAnswer);
					int num7 = GetCurrentPlayerUsableBookPointMax();
					List<OperationQuestion> list8 = new List<OperationQuestion>();
					foreach (OperationPointBase item13 in usablePopList)
					{
						if (item13 is OperationQuestion item5)
						{
							list8.Add(item5);
						}
					}
					if (operationAnswer5 != null)
					{
						Grid grid3 = match.GetGrid(operationAnswer5.GridIndex);
						if (grid3.GetThesisScore() == 0 && list8.Count > 0)
						{
							list8.Sort((OperationQuestion a, OperationQuestion b) => match.GetGrid(b.GridIndex).GetThesisScore().CompareTo(match.GetGrid(a.GridIndex).GetThesisScore()));
							OperationQuestion pOp = list8[0];
							List<ushort> usableBookSets25 = GetUsableBookSets(player);
							int currentPlayerBookSetPoint13 = GetCurrentPlayerBookSetPoint(match, GetOriginalUsableBookSets(player));
							int min13 = currentPlayerBookSetPoint13 * 20 / 100;
							int max13 = currentPlayerBookSetPoint13 * 80 / 100;
							usableBookSets25.RemoveAll(delegate(ushort set)
							{
								int currentPlayerBookSetPoint18 = GetCurrentPlayerBookSetPoint(match, set);
								return currentPlayerBookSetPoint18 < min13 || currentPlayerBookSetPoint18 > max13;
							});
							return CommitUsedBookStates(pOp, usableBookSets25, min13);
						}
						GetAnswerTargetPossiblePoints(operationAnswer5, out var _, out var _, out var opponentPossiblePointMax9, out var opponentPossiblePointMedium9);
						if (num7 >= opponentPossiblePointMax9)
						{
							List<ushort> usableBookSets26 = GetUsableBookSets(player);
							if (opponentPossiblePointMax9 > 0)
							{
								usableBookSets26.RemoveAll(delegate(ushort set)
								{
									int currentPlayerBookSetPoint18 = GetCurrentPlayerBookSetPoint(match, set);
									return currentPlayerBookSetPoint18 < opponentPossiblePointMedium9;
								});
							}
							else
							{
								usableBookSets26.Sort((ushort a, ushort b) => GetCurrentPlayerBookSetPoint(match, a).CompareTo(GetCurrentPlayerBookSetPoint(match, b)));
								if (usableBookSets26.Count > 0)
								{
									usableBookSets26.RemoveRange(1, usableBookSets26.Count - 1);
								}
							}
							return CommitUsedBookStates(operationAnswer5, usableBookSets26, opponentPossiblePointMax9);
						}
						if (num7 >= opponentPossiblePointMedium9)
						{
							List<ushort> usableBookSets27 = GetUsableBookSets(player);
							int usableMaxPoint5 = GetCurrentPlayerBookSetPoint(match, GetOriginalUsableBookSets(player));
							usableBookSets27.RemoveAll(delegate(ushort set)
							{
								int currentPlayerBookSetPoint18 = GetCurrentPlayerBookSetPoint(match, set);
								return currentPlayerBookSetPoint18 < opponentPossiblePointMedium9 || currentPlayerBookSetPoint18 > usableMaxPoint5;
							});
							return CommitUsedBookStates(operationAnswer5, usableBookSets27, opponentPossiblePointMedium9);
						}
					}
					list8.Sort(Sort5);
					GetOpponentUsableBookSetInNextPossiblePoints(out var _, out var _, out var opponentPossiblePointMax10, out var opponentPossiblePointMedium10, out var opponentPossiblePointMin5);
					foreach (OperationQuestion item14 in list8)
					{
						if (num7 < opponentPossiblePointMax10)
						{
							if (item14 is OperationQuestionRhetorical)
							{
								if (num7 >= opponentPossiblePointMedium10)
								{
									if (random.CheckPercentProb(70))
									{
										item14.ClearUsedBookStates(player);
										item14.CommitUsedBookStates(GetBookStatesByBookSet(GetOriginalUsableBookSets(player), player), null);
										return item14;
									}
									List<ushort> usableBookSets28 = GetUsableBookSets(player);
									int currentPlayerBookSetPoint14 = GetCurrentPlayerBookSetPoint(match, GetOriginalUsableBookSets(player));
									int min14 = currentPlayerBookSetPoint14 * 20 / 100;
									int max14 = currentPlayerBookSetPoint14 * 40 / 100;
									usableBookSets28.RemoveAll(delegate(ushort set)
									{
										int currentPlayerBookSetPoint18 = GetCurrentPlayerBookSetPoint(match, set);
										return currentPlayerBookSetPoint18 < min14 || currentPlayerBookSetPoint18 > max14;
									});
									return CommitUsedBookStates(item14, usableBookSets28, min14);
								}
								List<ushort> usableBookSets29 = GetUsableBookSets(player);
								int currentPlayerBookSetPoint15 = GetCurrentPlayerBookSetPoint(match, GetOriginalUsableBookSets(player));
								int min15 = currentPlayerBookSetPoint15 * 20 / 100;
								int max15 = currentPlayerBookSetPoint15 * 40 / 100;
								usableBookSets29.RemoveAll(delegate(ushort set)
								{
									int currentPlayerBookSetPoint18 = GetCurrentPlayerBookSetPoint(match, set);
									return currentPlayerBookSetPoint18 < min15 || currentPlayerBookSetPoint18 > max15;
								});
								return CommitUsedBookStates(item14, usableBookSets29, min15);
							}
							if (num7 >= opponentPossiblePointMedium10)
							{
								item14.ClearUsedBookStates(player);
								item14.CommitUsedBookStates(GetBookStatesByBookSet(GetOriginalUsableBookSets(player), player), null);
								return item14;
							}
							if (num7 >= opponentPossiblePointMin5)
							{
								if (random.CheckPercentProb(50))
								{
									item14.ClearUsedBookStates(player);
									item14.CommitUsedBookStates(GetBookStatesByBookSet(GetOriginalUsableBookSets(player), player), null);
									return item14;
								}
								List<ushort> usableBookSets30 = GetUsableBookSets(player);
								int currentPlayerBookSetPoint16 = GetCurrentPlayerBookSetPoint(match, GetOriginalUsableBookSets(player));
								int min16 = currentPlayerBookSetPoint16 * 20 / 100;
								int max16 = currentPlayerBookSetPoint16 * 40 / 100;
								usableBookSets30.RemoveAll(delegate(ushort set)
								{
									int currentPlayerBookSetPoint18 = GetCurrentPlayerBookSetPoint(match, set);
									return currentPlayerBookSetPoint18 < min16 || currentPlayerBookSetPoint18 > max16;
								});
								return CommitUsedBookStates(item14, usableBookSets30, min16);
							}
							List<ushort> usableBookSets31 = GetUsableBookSets(player);
							int currentPlayerBookSetPoint17 = GetCurrentPlayerBookSetPoint(match, GetOriginalUsableBookSets(player));
							int min17 = currentPlayerBookSetPoint17 * 20 / 100;
							int max17 = currentPlayerBookSetPoint17 * 40 / 100;
							usableBookSets31.RemoveAll(delegate(ushort set)
							{
								int currentPlayerBookSetPoint18 = GetCurrentPlayerBookSetPoint(match, set);
								return currentPlayerBookSetPoint18 < min17 || currentPlayerBookSetPoint18 > max17;
							});
							return CommitUsedBookStates(item14, usableBookSets31, min17);
						}
						List<ushort> usableBookSets32 = GetUsableBookSets(player);
						if (opponentPossiblePointMax10 > 0)
						{
							usableBookSets32.RemoveAll(delegate(ushort set)
							{
								int currentPlayerBookSetPoint18 = GetCurrentPlayerBookSetPoint(match, set);
								return currentPlayerBookSetPoint18 < opponentPossiblePointMedium10;
							});
						}
						else
						{
							usableBookSets32.Sort((ushort a, ushort b) => GetCurrentPlayerBookSetPoint(match, a).CompareTo(GetCurrentPlayerBookSetPoint(match, b)));
							if (usableBookSets32.Count > 0)
							{
								usableBookSets32.RemoveRange(1, usableBookSets32.Count - 1);
							}
						}
						if (usableBookSets32.Count > 0)
						{
							int usableBookSetPointMin5 = usableBookSets32.Select((ushort set) => GetCurrentPlayerBookSetPoint(match, set)).Min();
							usableBookSets32.RemoveAll((ushort set) => GetCurrentPlayerBookSetPoint(match, set) > usableBookSetPointMin5);
							return CommitUsedBookStates(item14, usableBookSets32, usableBookSetPointMin5);
						}
					}
					break;
				}
				case 2:
				{
					usablePopList.Sort(Sort3);
					int num5 = GetCurrentPlayerUsableBookPointMax();
					List<OperationQuestion> list7 = new List<OperationQuestion>();
					foreach (OperationPointBase item15 in usablePopList)
					{
						if (item15 is OperationQuestion item4)
						{
							list7.Add(item4);
						}
						else if (item15 is OperationAnswer operationAnswer3)
						{
							Grid grid2 = match.GetGrid(operationAnswer3.GridIndex);
							if (grid2.GetThesisScore() != 0 || !random.CheckPercentProb(50))
							{
								GetAnswerTargetPossiblePoints(operationAnswer3, out var _, out var _, out var opponentPossiblePointMax5, out var opponentPossiblePointMedium5);
								if (num5 >= opponentPossiblePointMax5)
								{
									if (!random.CheckPercentProb(50))
									{
										List<ushort> usableBookSets12 = GetUsableBookSets(player);
										if (opponentPossiblePointMax5 > 0)
										{
											usableBookSets12.RemoveAll(delegate(ushort set)
											{
												int currentPlayerBookSetPoint18 = GetCurrentPlayerBookSetPoint(match, set);
												return currentPlayerBookSetPoint18 < opponentPossiblePointMedium5;
											});
										}
										else
										{
											usableBookSets12.Sort((ushort a, ushort b) => GetCurrentPlayerBookSetPoint(match, a).CompareTo(GetCurrentPlayerBookSetPoint(match, b)));
											if (usableBookSets12.Count > 0)
											{
												usableBookSets12.RemoveRange(1, usableBookSets12.Count - 1);
											}
										}
										return CommitUsedBookStates(operationAnswer3, usableBookSets12, opponentPossiblePointMax5);
									}
								}
								else if (num5 >= opponentPossiblePointMedium5)
								{
									if (!random.CheckPercentProb(50))
									{
										List<ushort> usableBookSets13 = GetUsableBookSets(player);
										int usableMaxPoint3 = GetCurrentPlayerBookSetPoint(match, GetOriginalUsableBookSets(player));
										usableBookSets13.RemoveAll(delegate(ushort set)
										{
											int currentPlayerBookSetPoint18 = GetCurrentPlayerBookSetPoint(match, set);
											return currentPlayerBookSetPoint18 < opponentPossiblePointMedium5 || currentPlayerBookSetPoint18 > usableMaxPoint3;
										});
										return CommitUsedBookStates(operationAnswer3, usableBookSets13, opponentPossiblePointMedium5);
									}
								}
							}
						}
					}
					GetOpponentUsableBookSetInNextPossiblePoints(out var _, out var _, out var opponentPossiblePointMax6, out var opponentPossiblePointMedium6, out var opponentPossiblePointMin3);
					foreach (OperationQuestion item16 in list7)
					{
						if (num5 < opponentPossiblePointMax6)
						{
							if (item16 is OperationQuestionRhetorical)
							{
								if (num5 >= opponentPossiblePointMedium6)
								{
									if (random.CheckPercentProb(70))
									{
										item16.ClearUsedBookStates(player);
										item16.CommitUsedBookStates(GetBookStatesByBookSet(GetOriginalUsableBookSets(player), player), null);
										return item16;
									}
									List<ushort> usableBookSets14 = GetUsableBookSets(player);
									int currentPlayerBookSetPoint6 = GetCurrentPlayerBookSetPoint(match, GetOriginalUsableBookSets(player));
									int min6 = currentPlayerBookSetPoint6 * 20 / 100;
									int max6 = currentPlayerBookSetPoint6 * 40 / 100;
									usableBookSets14.RemoveAll(delegate(ushort set)
									{
										int currentPlayerBookSetPoint18 = GetCurrentPlayerBookSetPoint(match, set);
										return currentPlayerBookSetPoint18 < min6 || currentPlayerBookSetPoint18 > max6;
									});
									return CommitUsedBookStates(item16, usableBookSets14, min6);
								}
								List<ushort> usableBookSets15 = GetUsableBookSets(player);
								int currentPlayerBookSetPoint7 = GetCurrentPlayerBookSetPoint(match, GetOriginalUsableBookSets(player));
								int min7 = currentPlayerBookSetPoint7 * 20 / 100;
								int max7 = currentPlayerBookSetPoint7 * 40 / 100;
								usableBookSets15.RemoveAll(delegate(ushort set)
								{
									int currentPlayerBookSetPoint18 = GetCurrentPlayerBookSetPoint(match, set);
									return currentPlayerBookSetPoint18 < min7 || currentPlayerBookSetPoint18 > max7;
								});
								return CommitUsedBookStates(item16, usableBookSets15, min7);
							}
							if (num5 >= opponentPossiblePointMedium6)
							{
								item16.ClearUsedBookStates(player);
								item16.CommitUsedBookStates(GetBookStatesByBookSet(GetOriginalUsableBookSets(player), player), null);
								return item16;
							}
							if (num5 >= opponentPossiblePointMin3)
							{
								if (random.CheckPercentProb(50))
								{
									item16.ClearUsedBookStates(player);
									item16.CommitUsedBookStates(GetBookStatesByBookSet(GetOriginalUsableBookSets(player), player), null);
									return item16;
								}
								List<ushort> usableBookSets16 = GetUsableBookSets(player);
								int currentPlayerBookSetPoint8 = GetCurrentPlayerBookSetPoint(match, GetOriginalUsableBookSets(player));
								int min8 = currentPlayerBookSetPoint8 * 20 / 100;
								int max8 = currentPlayerBookSetPoint8 * 40 / 100;
								usableBookSets16.RemoveAll(delegate(ushort set)
								{
									int currentPlayerBookSetPoint18 = GetCurrentPlayerBookSetPoint(match, set);
									return currentPlayerBookSetPoint18 < min8 || currentPlayerBookSetPoint18 > max8;
								});
								return CommitUsedBookStates(item16, usableBookSets16, min8);
							}
							List<ushort> usableBookSets17 = GetUsableBookSets(player);
							int currentPlayerBookSetPoint9 = GetCurrentPlayerBookSetPoint(match, GetOriginalUsableBookSets(player));
							int min9 = currentPlayerBookSetPoint9 * 20 / 100;
							int max9 = currentPlayerBookSetPoint9 * 40 / 100;
							usableBookSets17.RemoveAll(delegate(ushort set)
							{
								int currentPlayerBookSetPoint18 = GetCurrentPlayerBookSetPoint(match, set);
								return currentPlayerBookSetPoint18 < min9 || currentPlayerBookSetPoint18 > max9;
							});
							return CommitUsedBookStates(item16, usableBookSets17, min9);
						}
						List<ushort> usableBookSets18 = GetUsableBookSets(player);
						if (opponentPossiblePointMax6 > 0)
						{
							usableBookSets18.RemoveAll(delegate(ushort set)
							{
								int currentPlayerBookSetPoint18 = GetCurrentPlayerBookSetPoint(match, set);
								return currentPlayerBookSetPoint18 < opponentPossiblePointMedium6;
							});
						}
						else
						{
							usableBookSets18.Sort((ushort a, ushort b) => GetCurrentPlayerBookSetPoint(match, a).CompareTo(GetCurrentPlayerBookSetPoint(match, b)));
							if (usableBookSets18.Count > 0)
							{
								usableBookSets18.RemoveRange(1, usableBookSets18.Count - 1);
							}
						}
						if (usableBookSets18.Count > 0)
						{
							int usableBookSetPointMin3 = usableBookSets18.Select((ushort set) => GetCurrentPlayerBookSetPoint(match, set)).Min();
							usableBookSets18.RemoveAll((ushort set) => GetCurrentPlayerBookSetPoint(match, set) > usableBookSetPointMin3);
							return CommitUsedBookStates(item16, usableBookSets18, opponentPossiblePointMax6);
						}
					}
					break;
				}
				case 3:
				{
					if (match.PlayerSwitchCount == 0)
					{
						match.RecordLineAi("REBEL first turn special silent");
						return new OperationSilent(playerId, match.PlayerSwitchCount);
					}
					List<OperationQuestion> list5 = new List<OperationQuestion>();
					List<OperationQuestion> list6 = new List<OperationQuestion>();
					int num4 = GetCurrentPlayerUsableBookPointMax();
					foreach (OperationPointBase item17 in usablePopList)
					{
						if (item17 is OperationQuestionRhetorical item2)
						{
							list5.Add(item2);
						}
						else if (item17 is OperationQuestion item3)
						{
							list6.Add(item3);
						}
						else if (item17 is OperationAnswer operationAnswer2)
						{
							Grid grid = match.GetGrid(item17.GridIndex);
							int thesisScore2 = grid.GetThesisScore();
							GetAnswerTargetPossiblePoints(operationAnswer2, out var _, out var _, out var opponentPossiblePointMax3, out var opponentPossiblePointMedium3);
							if (thesisScore2 > 1)
							{
								if (num4 >= opponentPossiblePointMax3)
								{
									List<ushort> usableBookSets7 = GetUsableBookSets(player);
									if (opponentPossiblePointMax3 > 0)
									{
										usableBookSets7.RemoveAll(delegate(ushort set)
										{
											int currentPlayerBookSetPoint18 = GetCurrentPlayerBookSetPoint(match, set);
											return currentPlayerBookSetPoint18 < opponentPossiblePointMedium3;
										});
									}
									else
									{
										usableBookSets7.Sort((ushort a, ushort b) => GetCurrentPlayerBookSetPoint(match, a).CompareTo(GetCurrentPlayerBookSetPoint(match, b)));
										if (usableBookSets7.Count > 0)
										{
											usableBookSets7.RemoveRange(1, usableBookSets7.Count - 1);
										}
									}
									return CommitUsedBookStates(operationAnswer2, usableBookSets7, opponentPossiblePointMax3);
								}
								if (num4 >= opponentPossiblePointMedium3)
								{
									List<ushort> usableBookSets8 = GetUsableBookSets(player);
									int usableMaxPoint2 = GetCurrentPlayerBookSetPoint(match, GetOriginalUsableBookSets(player));
									usableBookSets8.RemoveAll(delegate(ushort set)
									{
										int currentPlayerBookSetPoint18 = GetCurrentPlayerBookSetPoint(match, set);
										return currentPlayerBookSetPoint18 < opponentPossiblePointMedium3 || currentPlayerBookSetPoint18 > usableMaxPoint2;
									});
									return CommitUsedBookStates(operationAnswer2, usableBookSets8, opponentPossiblePointMax3);
								}
							}
						}
					}
					GetOpponentUsableBookSetInNextPossiblePoints(out var _, out var _, out var opponentPossiblePointMax4, out var opponentPossiblePointMedium4, out var opponentPossiblePointMin2);
					list5.Sort(Sort2);
					list6.Sort(Sort2);
					list5.AddRange(list6);
					foreach (OperationQuestion item18 in list5)
					{
						if (num4 < opponentPossiblePointMax4)
						{
							if (num4 >= opponentPossiblePointMedium4)
							{
								item18.ClearUsedBookStates(player);
								item18.CommitUsedBookStates(GetBookStatesByBookSet(GetOriginalUsableBookSets(player), player), null);
								return item18;
							}
							if (num4 >= opponentPossiblePointMin2)
							{
								if (random.CheckPercentProb(70))
								{
									item18.ClearUsedBookStates(player);
									item18.CommitUsedBookStates(GetBookStatesByBookSet(GetOriginalUsableBookSets(player), player), null);
									return item18;
								}
								List<ushort> usableBookSets9 = GetUsableBookSets(player);
								int currentPlayerBookSetPoint4 = GetCurrentPlayerBookSetPoint(match, GetOriginalUsableBookSets(player));
								int min4 = currentPlayerBookSetPoint4 * 20 / 100;
								int max4 = currentPlayerBookSetPoint4 * 40 / 100;
								usableBookSets9.RemoveAll(delegate(ushort set)
								{
									int currentPlayerBookSetPoint18 = GetCurrentPlayerBookSetPoint(match, set);
									return currentPlayerBookSetPoint18 < min4 || currentPlayerBookSetPoint18 > max4;
								});
								return CommitUsedBookStates(item18, usableBookSets9, min4);
							}
							List<ushort> usableBookSets10 = GetUsableBookSets(player);
							int currentPlayerBookSetPoint5 = GetCurrentPlayerBookSetPoint(match, GetOriginalUsableBookSets(player));
							int min5 = currentPlayerBookSetPoint5 * 20 / 100;
							int max5 = currentPlayerBookSetPoint5 * 40 / 100;
							usableBookSets10.RemoveAll(delegate(ushort set)
							{
								int currentPlayerBookSetPoint18 = GetCurrentPlayerBookSetPoint(match, set);
								return currentPlayerBookSetPoint18 < min5 || currentPlayerBookSetPoint18 > max5;
							});
							return CommitUsedBookStates(item18, usableBookSets10, min5);
						}
						List<ushort> usableBookSets11 = GetUsableBookSets(player);
						if (opponentPossiblePointMax4 > 0)
						{
							usableBookSets11.RemoveAll(delegate(ushort set)
							{
								int currentPlayerBookSetPoint18 = GetCurrentPlayerBookSetPoint(match, set);
								return currentPlayerBookSetPoint18 < opponentPossiblePointMedium4;
							});
						}
						else
						{
							usableBookSets11.Sort((ushort a, ushort b) => GetCurrentPlayerBookSetPoint(match, a).CompareTo(GetCurrentPlayerBookSetPoint(match, b)));
							if (usableBookSets11.Count > 0)
							{
								usableBookSets11.RemoveRange(1, usableBookSets11.Count - 1);
							}
						}
						if (usableBookSets11.Count > 0)
						{
							int usableBookSetPointMin2 = usableBookSets11.Select((ushort set) => GetCurrentPlayerBookSetPoint(match, set)).Min();
							usableBookSets11.RemoveAll((ushort set) => GetCurrentPlayerBookSetPoint(match, set) > usableBookSetPointMin2);
							return CommitUsedBookStates(item18, usableBookSets11, opponentPossiblePointMax4);
						}
					}
					break;
				}
				case 4:
				{
					usablePopList.Sort(Sort);
					int num2 = GetCurrentPlayerUsableBookPointMax();
					foreach (OperationPointBase item19 in usablePopList)
					{
						if (item19 is OperationAnswer operationAnswer)
						{
							GetAnswerTargetPossiblePoints(operationAnswer, out var _, out var opponentPossiblePoint, out var opponentPossiblePointMax, out var opponentPossiblePointMedium);
							int num3 = ((opponentPossiblePoint.Count > 0) ? opponentPossiblePoint.Min() : 0);
							if (num2 >= opponentPossiblePointMax)
							{
								List<ushort> usableBookSets = GetUsableBookSets(player);
								if (opponentPossiblePointMax > 0)
								{
									usableBookSets.RemoveAll(delegate(ushort set)
									{
										int currentPlayerBookSetPoint18 = GetCurrentPlayerBookSetPoint(match, set);
										return currentPlayerBookSetPoint18 < opponentPossiblePointMedium;
									});
								}
								else
								{
									usableBookSets.Sort((ushort a, ushort b) => GetCurrentPlayerBookSetPoint(match, a).CompareTo(GetCurrentPlayerBookSetPoint(match, b)));
									if (usableBookSets.Count > 0)
									{
										usableBookSets.RemoveRange(1, usableBookSets.Count - 1);
									}
								}
								return CommitUsedBookStates(operationAnswer, usableBookSets, opponentPossiblePointMax);
							}
							if (num2 >= opponentPossiblePointMedium)
							{
								List<ushort> usableBookSets2 = GetUsableBookSets(player);
								int usableMaxPoint = GetCurrentPlayerBookSetPoint(match, GetOriginalUsableBookSets(player));
								usableBookSets2.RemoveAll(delegate(ushort set)
								{
									int currentPlayerBookSetPoint18 = GetCurrentPlayerBookSetPoint(match, set);
									return currentPlayerBookSetPoint18 < opponentPossiblePointMedium || currentPlayerBookSetPoint18 > usableMaxPoint;
								});
								return CommitUsedBookStates(operationAnswer, usableBookSets2, opponentPossiblePointMedium);
							}
							if (num2 >= num3)
							{
								operationAnswer.ClearUsedBookStates(player);
								operationAnswer.CommitUsedBookStates(GetBookStatesByBookSet(GetOriginalUsableBookSets(player), player), null);
								return operationAnswer;
							}
						}
						else
						{
							if (item19 is OperationQuestionAdditional result)
							{
								match.RecordLineAi("decided flexiblefall");
								return result;
							}
							if (item19 is OperationQuestion operationQuestion && num2 > 0)
							{
								int thesisScore = match.GetGrid(operationQuestion.GridIndex).GetThesisScore();
								if (thesisScore == 0)
								{
									List<ushort> usableBookSets3 = GetUsableBookSets(player);
									int currentPlayerBookSetPoint = GetCurrentPlayerBookSetPoint(match, GetOriginalUsableBookSets(player));
									int min = currentPlayerBookSetPoint * 20 / 100;
									int max = currentPlayerBookSetPoint * 30 / 100;
									usableBookSets3.RemoveAll(delegate(ushort set)
									{
										int currentPlayerBookSetPoint18 = GetCurrentPlayerBookSetPoint(match, set);
										return currentPlayerBookSetPoint18 < min || currentPlayerBookSetPoint18 > max;
									});
									return CommitUsedBookStates(operationQuestion, usableBookSets3, min);
								}
								if (thesisScore >= 1)
								{
									GetOpponentUsableBookSetInNextPossiblePoints(out var _, out var _, out var opponentPossiblePointMax2, out var opponentPossiblePointMedium2, out var opponentPossiblePointMin);
									if (num2 < opponentPossiblePointMax2)
									{
										if (num2 >= opponentPossiblePointMedium2)
										{
											operationQuestion.ClearUsedBookStates(player);
											operationQuestion.CommitUsedBookStates(GetBookStatesByBookSet(GetOriginalUsableBookSets(player), player), null);
											return operationQuestion;
										}
										if (num2 >= opponentPossiblePointMin)
										{
											if (thesisScore > 1)
											{
												operationQuestion.ClearUsedBookStates(player);
												operationQuestion.CommitUsedBookStates(GetBookStatesByBookSet(GetOriginalUsableBookSets(player), player), null);
												return operationQuestion;
											}
											if (random.CheckPercentProb(30))
											{
												operationQuestion.ClearUsedBookStates(player);
												operationQuestion.CommitUsedBookStates(GetBookStatesByBookSet(GetOriginalUsableBookSets(player), player), null);
												return operationQuestion;
											}
											List<ushort> usableBookSets4 = GetUsableBookSets(player);
											int currentPlayerBookSetPoint2 = GetCurrentPlayerBookSetPoint(match, GetOriginalUsableBookSets(player));
											int min2 = currentPlayerBookSetPoint2 * 10 / 100;
											int max2 = currentPlayerBookSetPoint2 * 30 / 100;
											usableBookSets4.RemoveAll(delegate(ushort set)
											{
												int currentPlayerBookSetPoint18 = GetCurrentPlayerBookSetPoint(match, set);
												return currentPlayerBookSetPoint18 < min2 || currentPlayerBookSetPoint18 > max2;
											});
											operationQuestion.ClearUsedBookStates(player);
											operationQuestion.CommitUsedBookStates(GetBookStatesByBookSet(usableBookSets4.GetRandom(random), player), null);
											return operationQuestion;
										}
										List<ushort> usableBookSets5 = GetUsableBookSets(player);
										int currentPlayerBookSetPoint3 = GetCurrentPlayerBookSetPoint(match, GetOriginalUsableBookSets(player));
										int min3 = currentPlayerBookSetPoint3 * 20 / 100;
										int max3 = currentPlayerBookSetPoint3 * 80 / 100;
										usableBookSets5.RemoveAll(delegate(ushort set)
										{
											int currentPlayerBookSetPoint18 = GetCurrentPlayerBookSetPoint(match, set);
											return currentPlayerBookSetPoint18 < min3 || currentPlayerBookSetPoint18 > max3;
										});
										operationQuestion.ClearUsedBookStates(player);
										operationQuestion.CommitUsedBookStates(GetBookStatesByBookSet(usableBookSets5.GetRandom(random), player), null);
										return operationQuestion;
									}
									List<ushort> usableBookSets6 = GetUsableBookSets(player);
									if (opponentPossiblePointMax2 > 0)
									{
										usableBookSets6.RemoveAll(delegate(ushort set)
										{
											int currentPlayerBookSetPoint18 = GetCurrentPlayerBookSetPoint(match, set);
											return currentPlayerBookSetPoint18 < opponentPossiblePointMedium2;
										});
									}
									else
									{
										usableBookSets6.Sort((ushort a, ushort b) => GetCurrentPlayerBookSetPoint(match, a).CompareTo(GetCurrentPlayerBookSetPoint(match, b)));
										if (usableBookSets6.Count > 0)
										{
											usableBookSets6.RemoveRange(1, usableBookSets6.Count - 1);
										}
									}
									if (usableBookSets6.Count > 0)
									{
										int usableBookSetPointMin = usableBookSets6.Select((ushort set) => GetCurrentPlayerBookSetPoint(match, set)).Min();
										usableBookSets6.RemoveAll((ushort set) => GetCurrentPlayerBookSetPoint(match, set) > usableBookSetPointMin);
										return CommitUsedBookStates(operationQuestion, usableBookSets6, opponentPossiblePointMax2);
									}
								}
							}
						}
					}
					break;
				}
				}
				match.RecordLineAi("warning: fallback to silent");
				return new OperationSilent(playerId, match.PlayerSwitchCount);
			}
			void GetAnswerTargetPossiblePoints(OperationAnswer answer, out ushort opponentUsingBookSet, out List<int> opponentPossiblePoint, out int opponentPossiblePointMax, out int opponentPossiblePointMedium)
			{
				OperationQuestion answerTarget = match.GetAnswerTarget(answer);
				opponentUsingBookSet = GetOperationBookSets(answerTarget, match);
				opponentPossiblePoint = GetOpponentBookSetPossiblePoints(match, opponentUsingBookSet);
				opponentPossiblePointMax = ((opponentPossiblePoint.Count > 0) ? opponentPossiblePoint.Max() : 0);
				opponentPossiblePointMedium = ((opponentPossiblePoint.Count > 0) ? opponentPossiblePoint[opponentPossiblePoint.Count / 2] : 0);
				match.RecordLineAi($"inferring the book_set to answer {GetBookIndicesSetInspect(opponentUsingBookSet, opponentPlayer)} possible points：{{{string.Join(", ", opponentPossiblePoint)}}}");
			}
			Dictionary<ELifeSkillCombatEffectGroup, int> GetCardRatePerGroup()
			{
				Dictionary<ELifeSkillCombatEffectGroup, int> dictionary2 = new Dictionary<ELifeSkillCombatEffectGroup, int>();
				bool flag2 = match.CalcPlayerScore(opponentPlayer.Id) >= match.CalcPlayerScore(playerId) + 5;
				Dictionary<ELifeSkillCombatEffectType, int> dictionary3 = new Dictionary<ELifeSkillCombatEffectType, int>
				{
					[ELifeSkillCombatEffectType.Common] = 90,
					[ELifeSkillCombatEffectType.BUFF] = 90,
					[ELifeSkillCombatEffectType.Strategy] = 90,
					[ELifeSkillCombatEffectType.Cooling] = 90,
					[ELifeSkillCombatEffectType.Special] = 0
				};
				if (flag2)
				{
					for (ELifeSkillCombatEffectType eLifeSkillCombatEffectType = ELifeSkillCombatEffectType.Common; eLifeSkillCombatEffectType < ELifeSkillCombatEffectType.Count; eLifeSkillCombatEffectType++)
					{
						dictionary3[eLifeSkillCombatEffectType] = 100;
					}
				}
				for (ELifeSkillCombatEffectType eLifeSkillCombatEffectType2 = ELifeSkillCombatEffectType.Common; eLifeSkillCombatEffectType2 < ELifeSkillCombatEffectType.Count; eLifeSkillCombatEffectType2++)
				{
					ELifeSkillCombatEffectGroup[] array = LifeSkillCombatEffectTypeGroups[eLifeSkillCombatEffectType2];
					int value = dictionary3[eLifeSkillCombatEffectType2] / array.Length;
					ELifeSkillCombatEffectGroup[] array2 = array;
					foreach (ELifeSkillCombatEffectGroup key in array2)
					{
						dictionary2[key] = value;
					}
					if (eLifeSkillCombatEffectType2 != ELifeSkillCombatEffectType.BUFF && eLifeSkillCombatEffectType2 != ELifeSkillCombatEffectType.Special)
					{
						ELifeSkillCombatEffectGroup key2 = array.MinBy(GroupToValue);
						dictionary2[array.MaxBy(GroupToValue)] += 10;
						dictionary2[key2] -= 10;
					}
				}
				return dictionary2;
			}
			int GetCurrentPlayerUsableBookPointMax()
			{
				int num2 = player.BookStates.Where((Book book) => !book.IsCd).Sum((Book book) => book.BasePoint);
				match.RecordLineAi($"self non-cd book points X = {num2}");
				return num2;
			}
			void GetOpponentUsableBookSetInNextPossiblePoints(out ushort opponentUsableBookSetInNext, out List<int> opponentPossiblePoint, out int opponentPossiblePointMax, out int opponentPossiblePointMedium, out int opponentPossiblePointMin)
			{
				opponentUsableBookSetInNext = GetOpponentUsableBookSetInNext(match);
				opponentPossiblePoint = GetOpponentBookSetPossiblePoints(match, opponentUsableBookSetInNext);
				if (opponentUsableBookSetInNext == 0 && opponentPossiblePoint.Count <= 0)
				{
					opponentPossiblePoint.Add(1);
				}
				opponentPossiblePointMax = ((opponentPossiblePoint.Count > 0) ? opponentPossiblePoint.Max() : 0);
				opponentPossiblePointMedium = ((opponentPossiblePoint.Count > 0) ? opponentPossiblePoint[opponentPossiblePoint.Count / 2] : 0);
				opponentPossiblePointMin = ((opponentPossiblePoint.Count > 0) ? opponentPossiblePoint.Min() : 0);
				match.RecordLineAi($"inferring the book_set of adversary in next turn {GetBookIndicesSetInspect(opponentUsableBookSetInNext, opponentPlayer)} possible points：[{string.Join(',', opponentPossiblePoint)}]");
			}
			int GetSortingScore(OperationPointBase pOp)
			{
				int num2 = 0;
				if (pOp is OperationQuestion && !(pOp is OperationQuestionRhetorical))
				{
					num2++;
				}
				return num2 + match.GetGrid(pOp.GridIndex).GetThesisScore();
			}
			int GetSortingScore2(OperationQuestion pOp)
			{
				int num2 = 0;
				Grid grid = match.GetGrid(pOp.GridIndex);
				num2 -= Grid.ToCenterAnchoredCoordinate(grid.Index).GetManhattanDistance(grid.Coordinate, Coordinate2D<sbyte>.Zero);
				return num2 + grid.GetThesisScore();
			}
			int GroupToValue(ELifeSkillCombatEffectGroup group)
			{
				if (LifeSkillCombatEffectGroupPersonalityTypes.TryGetValue(group, out var value))
				{
					return value.Sum((sbyte personalityType) => character.GetPersonality(personalityType));
				}
				return 0;
			}
			int Sort(OperationPointBase a, OperationPointBase b)
			{
				Grid grid = match.GetGrid(a.GridIndex);
				Grid grid2 = match.GetGrid(b.GridIndex);
				int num2 = grid2.GetThesisScore().CompareTo(grid.GetThesisScore());
				if (num2 != 0)
				{
					return num2;
				}
				return Grid.ToCenterAnchoredCoordinate(grid.Index).GetManhattanDistance(grid.Coordinate, Coordinate2D<sbyte>.Zero).CompareTo(Grid.ToCenterAnchoredCoordinate(grid2.Index).GetManhattanDistance(grid2.Coordinate, Coordinate2D<sbyte>.Zero));
			}
			int Sort2(OperationPointBase a, OperationPointBase b)
			{
				Grid grid = match.GetGrid(a.GridIndex);
				Grid grid2 = match.GetGrid(b.GridIndex);
				int num2 = grid2.GetThesisScore().CompareTo(grid.GetThesisScore());
				if (num2 != 0)
				{
					return num2;
				}
				return Grid.ToCenterAnchoredCoordinate(grid.Index).GetManhattanDistance(grid.Coordinate, Coordinate2D<sbyte>.Zero).CompareTo(Grid.ToCenterAnchoredCoordinate(grid2.Index).GetManhattanDistance(grid2.Coordinate, Coordinate2D<sbyte>.Zero));
			}
			int Sort3(OperationPointBase a, OperationPointBase b)
			{
				return GetSortingScore(b).CompareTo(GetSortingScore(a));
			}
			int Sort4(OperationPointBase a, OperationPointBase b)
			{
				Grid grid = match.GetGrid(a.GridIndex);
				Grid grid2 = match.GetGrid(b.GridIndex);
				int num2 = grid2.GetThesisScore().CompareTo(grid.GetThesisScore());
				if (num2 != 0)
				{
					return num2;
				}
				return Grid.ToCenterAnchoredCoordinate(grid.Index).GetManhattanDistance(grid.Coordinate, Coordinate2D<sbyte>.Zero).CompareTo(Grid.ToCenterAnchoredCoordinate(grid2.Index).GetManhattanDistance(grid2.Coordinate, Coordinate2D<sbyte>.Zero));
			}
			int Sort5(OperationQuestion a, OperationQuestion b)
			{
				int num2 = GetSortingScore2(b).CompareTo(GetSortingScore2(a));
				if (num2 != 0)
				{
					return num2;
				}
				int num3 = ((a is OperationQuestionRhetorical) ? 1 : 0);
				int value = ((b is OperationQuestionRhetorical) ? 1 : 0);
				return num3.CompareTo(value);
			}
			bool TryUseCardDone(sbyte cardId)
			{
				if (_banCardTemplateIds.Contains(cardId))
				{
					return false;
				}
				LifeSkillCombatEffectItem lifeSkillCombatEffectItem4 = LifeSkillCombatEffect.Instance[cardId];
				if (lifeSkillCombatEffectItem4.IsInstant)
				{
					return false;
				}
				_banCardTemplateIds.UnionWith(lifeSkillCombatEffectItem4.BanCardList);
				additionalEffectCardTemplateIds.Add(cardId);
				if (lifeSkillCombatEffectItem4.Group == ELifeSkillCombatEffectGroup.FlexibleFall)
				{
					ELifeSkillCombatEffectGroup[] array = LifeSkillCombatEffectTypeGroups[ELifeSkillCombatEffectType.Common];
					foreach (ELifeSkillCombatEffectGroup key in array)
					{
						ratePerGroup[key] = 0;
					}
					match.RecordLineAi("try use flexiblefall " + lifeSkillCombatEffectItem4.Name + ", so canceled the rate of flexiblefall");
				}
				else
				{
					match.RecordLineAi("try use card " + lifeSkillCombatEffectItem4.Name);
				}
				return true;
			}
		}

		public void RecordInspect(Match match)
		{
			Player player = match.GetPlayer(Player.PredefinedId.GetTheOtherSide(match._currentPlayerId));
			int num = 0;
			match.RecordLineAi("---- Ai inspect begin ----", num);
			match.RecordLineAi("Current behaviour_type: " + Config.BehaviorType.Instance[_behaviourTypeStyle].Name);
			match.RecordLineAi("Current ban cards in turn: {" + string.Join(", ", _banCardTemplateIds.Select((sbyte id) => LifeSkillCombatEffect.Instance[id].Name)) + "}");
			num++;
			match.RecordLineAi($"Recorded the precisely book_set point of adversary count: {_opponentPreciselyBookPointImpressions.Count}");
			for (sbyte b = 0; b < 9; b++)
			{
				if (TryGetOpponentBookPoint(match, b, out var points))
				{
					match.RecordLineAi($"adversary single book point {LifeSkill.Instance[player.BookStates[b].LifeSkill.SkillTemplateId].Name} memory = {{{string.Join(", ", points)}}} (House[{player.Id}]'s real point: {player.BookStates[b].BasePoint}", num);
				}
			}
			num--;
			match.RecordLineAi("---- Ai inspect end ----", num);
		}

		public static string GetBookIndicesSetInspect(ushort bookSet, Player player, bool isName = true)
		{
			IReadOnlyList<Book> bookStates = player.BookStates;
			string text = string.Empty;
			bool flag = true;
			for (int i = 0; i < 9; i++)
			{
				if (((1 << i) & bookSet) != 0)
				{
					if (!flag)
					{
						text += ", ";
					}
					text += (isName ? LifeSkill.Instance[bookStates[i].LifeSkill.SkillTemplateId].Name : $"{i}");
					flag = false;
				}
			}
			return "{" + text + "}";
		}
	}

	public readonly sbyte LifeSkillType;

	private readonly Grid[] _gridStatus = new Grid[49];

	private sbyte _currentPlayerId;

	private sbyte? _suiciderPlayerId;

	private bool _lastOperationIsSilent;

	public sbyte AcceptSilentPlayerId;

	private readonly Dictionary<sbyte, Ai> _aiStates = new Dictionary<sbyte, Ai>();

	private readonly Player _playerSelf;

	private readonly Player _playerAdversary;

	private static readonly (sbyte X, sbyte Y)[] _Offset8 = new(sbyte, sbyte)[8]
	{
		(0, 1),
		(0, -1),
		(1, 0),
		(-1, 0),
		(1, 1),
		(-1, -1),
		(1, -1),
		(-1, 1)
	};

	private static readonly Coordinate2D<sbyte>[] OffsetStraight = new Coordinate2D<sbyte>[4]
	{
		new Coordinate2D<sbyte>(1, 0),
		new Coordinate2D<sbyte>(-1, 0),
		new Coordinate2D<sbyte>(0, 1),
		new Coordinate2D<sbyte>(0, -1)
	};

	private static readonly Coordinate2D<sbyte>[] OffsetCross = new Coordinate2D<sbyte>[4]
	{
		new Coordinate2D<sbyte>(1, 1),
		new Coordinate2D<sbyte>(-1, -1),
		new Coordinate2D<sbyte>(1, -1),
		new Coordinate2D<sbyte>(-1, 1)
	};

	public readonly Dictionary<sbyte, sbyte> PlayerForcedSecretInformation = new Dictionary<sbyte, sbyte>();

	private StatusSnapshot _beforeSimulated;

	public const byte BoardWidth = 7;

	public const byte BoardRadius = 3;

	public const byte BoardGridCount = 49;

	public const int ScoreForWinner = 12;

	private const int MaxStatisticsFile = 20;

	private readonly StringBuilder _recorder = new StringBuilder();

	public bool SuicideIsForced { get; private set; }

	public int PlayerSwitchCount { get; private set; }

	public sbyte CurrentPlayerId => _currentPlayerId;

	public Match(DataContext context, sbyte lifeSkillType, int characterIdA, int characterIdB, sbyte firstTurnPlayerId)
	{
		LifeSkillType = lifeSkillType;
		_playerSelf = new Player(context, this, 0, characterIdA, lifeSkillType);
		_playerAdversary = new Player(context, this, 1, characterIdB, lifeSkillType);
		if (DomainManager.Character.GetElement_Objects(characterIdA).GetLeaderId() == DomainManager.Character.GetElement_Objects(characterIdB).GetLeaderId())
		{
			_playerSelf.DropAllUsableFriendlyCharacters();
			_playerAdversary.DropAllUsableFriendlyCharacters();
		}
		Setup(firstTurnPlayerId);
	}

	private void Setup(sbyte firstTurnPlayerId)
	{
		int i = 0;
		for (int num = 49; i < num; i++)
		{
			_gridStatus[i] = new Grid(i);
		}
		_suiciderPlayerId = null;
		_currentPlayerId = firstTurnPlayerId;
		_lastOperationIsSilent = false;
		PlayerSwitchCount = 0;
		SuicideIsForced = false;
		AcceptSilentPlayerId = -1;
		RecordLine($"first turn started - House[{_currentPlayerId}]");
	}

	public bool CheckResult(out sbyte winnerPlayerId)
	{
		if (_suiciderPlayerId.HasValue)
		{
			winnerPlayerId = Player.PredefinedId.GetTheOtherSide(_suiciderPlayerId.Value);
			return true;
		}
		winnerPlayerId = -1;
		for (sbyte b = 0; b < 2; b++)
		{
			if (CalcPlayerScore(b) >= 12)
			{
				winnerPlayerId = b;
				return true;
			}
		}
		return false;
	}

	public int GetPlayerCharacterId(sbyte playerId)
	{
		return GetPlayer(playerId).CharacterId;
	}

	public Grid GetGrid(int index)
	{
		return _gridStatus[index];
	}

	public Grid GetGrid(Coordinate2D<sbyte> coordinate)
	{
		return GetGrid(Grid.ToGridIndex(coordinate));
	}

	public Player GetPlayer(sbyte playerId)
	{
		return playerId switch
		{
			0 => _playerSelf, 
			1 => _playerAdversary, 
			_ => throw new IndexOutOfRangeException(), 
		};
	}

	public int CalcPlayerScore(sbyte playerId)
	{
		int num = 0;
		int i = 0;
		for (int num2 = _gridStatus.Length; i < num2; i++)
		{
			Grid grid = _gridStatus[i];
			OperationPointBase thesis = grid.GetThesis();
			if (thesis != null && thesis.PlayerId == playerId)
			{
				num += grid.GetThesisScore();
			}
		}
		return num;
	}

	public OperationQuestion GetAnswerTarget(OperationAnswer answer)
	{
		Grid grid = GetGrid(answer.GridIndex);
		IReadOnlyList<OperationGridBase> allOperations = grid.GetAllOperations();
		for (int num = allOperations.Count - 1; num >= 0; num--)
		{
			if (allOperations[num] is OperationQuestion result)
			{
				return result;
			}
		}
		throw new IndexOutOfRangeException();
	}

	public static IEnumerable<Coordinate2D<sbyte>> OffsetIterate(Coordinate2D<sbyte> origin, sbyte straight, sbyte cross, Func<Coordinate2D<sbyte>, bool> breakCondition = null)
	{
		Coordinate2D<sbyte>[] offsetStraight = OffsetStraight;
		for (int i = 0; i < offsetStraight.Length; i++)
		{
			Coordinate2D<sbyte> offset = offsetStraight[i];
			for (int j = 0; j < straight; j++)
			{
				Coordinate2D<sbyte> coord = new Coordinate2D<sbyte>((sbyte)Math.Clamp(origin.X + offset.X * (j + 1), -128, 127), (sbyte)Math.Clamp(origin.Y + offset.Y * (j + 1), -128, 127));
				if (coord.X >= -3 && coord.X <= 3 && coord.Y >= -3 && coord.Y <= 3)
				{
					if (breakCondition?.Invoke(coord) ?? false)
					{
						break;
					}
					yield return coord;
				}
			}
		}
		Coordinate2D<sbyte>[] offsetCross = OffsetCross;
		for (int k = 0; k < offsetCross.Length; k++)
		{
			Coordinate2D<sbyte> offset2 = offsetCross[k];
			for (int l = 0; l < cross; l++)
			{
				Coordinate2D<sbyte> coord2 = new Coordinate2D<sbyte>((sbyte)Math.Clamp(origin.X + offset2.X * (l + 1), -128, 127), (sbyte)Math.Clamp(origin.Y + offset2.Y * (l + 1), -128, 127));
				if (coord2.X >= -3 && coord2.X <= 3 && coord2.Y >= -3 && coord2.Y <= 3)
				{
					if (breakCondition?.Invoke(coord2) ?? false)
					{
						break;
					}
					yield return coord2;
				}
			}
		}
	}

	public void OnGridActiveFixed(OperationPointBase pOp, IList<StatusSnapshotDiff.GridTrapStateExtraDiff> gridTrapStateExtraDiffs)
	{
		pOp.ProcessOnGridActiveFixed(this, gridTrapStateExtraDiffs);
	}

	public void CommitOperation(DataContext context, OperationBase operation, IList<StatusSnapshotDiff.BookStateExtraDiff> bookStateExtraDiffs, IList<StatusSnapshotDiff.GridTrapStateExtraDiff> gridTrapStateExtraDiffs)
	{
		_beforeSimulated = null;
		RecordLine($"House[{operation.PlayerId}] commit operation -> [{operation}]");
		CommitOperationProcess(context, operation, bookStateExtraDiffs, gridTrapStateExtraDiffs);
		if (AcceptSilentPlayerId == operation.PlayerId && operation is OperationSilent)
		{
			AcceptSilentPlayerId = -1;
		}
	}

	public sbyte CalcTargetPlayerId(sbyte configTargetPlayerId, sbyte selfAnchorPlayerId)
	{
		return selfAnchorPlayerId switch
		{
			0 => (configTargetPlayerId <= 0) ? ((sbyte)1) : ((sbyte)0), 
			1 => (configTargetPlayerId > 0) ? ((sbyte)1) : ((sbyte)0), 
			_ => -1, 
		};
	}

	internal void CommitOperationProcess(DataContext context, OperationBase operation, IList<StatusSnapshotDiff.BookStateExtraDiff> bookStateExtraDiffs, IList<StatusSnapshotDiff.GridTrapStateExtraDiff> gridTrapStateExtraDiffs, bool withProcessActiveOperationCells = true)
	{
		if (operation.GetStamp() >= 0 && operation.GetStamp() < PlayerSwitchCount)
		{
			PredefinedLog.Show(15, operation.Inspect(), operation.GetStamp(), PlayerSwitchCount);
			return;
		}
		Assert(operation.PlayerId == _currentPlayerId, $"{"_currentPlayerId"} mismatch! (stamp: {operation.GetStamp()})");
		_lastOperationIsSilent = false;
		if (operation is OperationSilent)
		{
			SwitchCurrentPlayerProcess(context, bookStateExtraDiffs, gridTrapStateExtraDiffs, withProcessActiveOperationCells);
			_lastOperationIsSilent = true;
		}
		else if (operation is OperationGiveUp)
		{
			_suiciderPlayerId = operation.PlayerId;
		}
		else if (operation is OperationUseEffectCard { PlayerId: var playerId } operationUseEffectCard)
		{
			sbyte effectCardTemplateId = operationUseEffectCard.Info.EffectCardTemplateId;
			LifeSkillCombatEffectItem lifeSkillCombatEffectItem = LifeSkillCombatEffect.Instance[effectCardTemplateId];
			Assert(lifeSkillCombatEffectItem.IsInstant, lifeSkillCombatEffectItem.Name + " is not [Instant]");
			Player player = GetPlayer(playerId);
			player.DropEffectCard(effectCardTemplateId);
			switch (lifeSkillCombatEffectItem.SubEffect)
			{
			case ELifeSkillCombatEffectSubEffect.SelfGridCoverBookStatesWhenAllQuestion:
			case ELifeSkillCombatEffectSubEffect.SelfTrapedInCell:
			case ELifeSkillCombatEffectSubEffect.SelfDoPickByPoint:
			case ELifeSkillCombatEffectSubEffect.SelfThesisChangeAroundHouseActivePointWithParam:
			case ELifeSkillCombatEffectSubEffect.SelfThesisWhenFixedEraseEffectsAroundHouseAllQuestionLowAndThesisLow:
			case ELifeSkillCombatEffectSubEffect.SelfThesisWhenFixedDoCancelAroundHouseAllQuestionLowAndThesisLow:
			case ELifeSkillCombatEffectSubEffect.PointChange:
				gridTrapStateExtraDiffs.Add(new StatusSnapshotDiff.GridTrapStateExtraDiff
				{
					GridIndex = operationUseEffectCard.Info.CellIndex,
					OwnerPlayerId = playerId,
					Type = StatusSnapshotDiff.GridTrapStateExtraDiff.TrapChangeType.Added
				});
				break;
			}
			switch (lifeSkillCombatEffectItem.SubEffect)
			{
			case ELifeSkillCombatEffectSubEffect.SelfEraseAroundSelfThesisHouseQuestionThesis:
			{
				Grid grid3 = GetGrid(operationUseEffectCard.Info.CellIndex2);
				if (grid3 != null)
				{
					OperationPointBase operationPointBase2 = grid3.GetThesis();
					if (operationPointBase2 == null)
					{
						operationPointBase2 = grid3.ActiveOperation as OperationPointBase;
					}
					sbyte b5 = CalcTargetPlayerId(lifeSkillCombatEffectItem.SubEffectParameters[0], playerId);
					if (operationPointBase2 != null && operationPointBase2.PlayerId == b5 && operationPointBase2.Point < GetGrid(operationUseEffectCard.Info.CellIndex).GetThesis().Point)
					{
						grid3.SetActiveOperation(null, this, gridTrapStateExtraDiffs);
						grid3.DropHistoryOperations();
					}
				}
				break;
			}
			case ELifeSkillCombatEffectSubEffect.SelfEraseAroundHouseQuestionEffects:
			{
				Grid grid2 = GetGrid(operationUseEffectCard.Info.CellIndex);
				if (grid2 == null)
				{
					break;
				}
				OperationPointBase operationPointBase = grid2.GetThesis();
				if (operationPointBase == null)
				{
					operationPointBase = grid2.ActiveOperation as OperationPointBase;
				}
				sbyte b4 = CalcTargetPlayerId(lifeSkillCombatEffectItem.SubEffectParameters[0], playerId);
				if (operationPointBase == null || operationPointBase.PlayerId != b4)
				{
					break;
				}
				foreach (sbyte effectiveEffectCardTemplateId in operationPointBase.EffectiveEffectCardTemplateIds)
				{
					switch (LifeSkillCombatEffect.Instance[effectiveEffectCardTemplateId].SubEffect)
					{
					case ELifeSkillCombatEffectSubEffect.SelfGridCoverBookStatesWhenAllQuestion:
					case ELifeSkillCombatEffectSubEffect.SelfTrapedInCell:
					case ELifeSkillCombatEffectSubEffect.SelfDoPickByPoint:
					case ELifeSkillCombatEffectSubEffect.SelfThesisChangeAroundHouseActivePointWithParam:
					case ELifeSkillCombatEffectSubEffect.SelfThesisWhenFixedEraseEffectsAroundHouseAllQuestionLowAndThesisLow:
					case ELifeSkillCombatEffectSubEffect.SelfThesisWhenFixedDoCancelAroundHouseAllQuestionLowAndThesisLow:
					case ELifeSkillCombatEffectSubEffect.PointChange:
						gridTrapStateExtraDiffs.Add(new StatusSnapshotDiff.GridTrapStateExtraDiff
						{
							GridIndex = grid2.Index,
							OwnerPlayerId = b4,
							Type = StatusSnapshotDiff.GridTrapStateExtraDiff.TrapChangeType.Lost
						});
						break;
					}
				}
				operationPointBase.DropEffectiveEffectCards();
				break;
			}
			case ELifeSkillCombatEffectSubEffect.SelfChangeBookCd:
			{
				sbyte b = CalcTargetPlayerId(lifeSkillCombatEffectItem.SubEffectParameters[0], playerId);
				sbyte b2 = lifeSkillCombatEffectItem.SubEffectParameters[3];
				sbyte b3 = lifeSkillCombatEffectItem.SubEffectParameters[4];
				Player player2 = GetPlayer(b);
				List<Book> list = new List<Book>();
				if (operationUseEffectCard.Info.TargetBookStateIndex >= 0)
				{
					list.Add(GetPlayer(operationUseEffectCard.Info.TargetBookOwnerPlayerId).BookStates[operationUseEffectCard.Info.TargetBookStateIndex]);
				}
				else
				{
					list.AddRange(player2.BookStates);
					CollectionUtils.Shuffle(context.Random, list);
				}
				List<sbyte> list2 = new List<sbyte>();
				int num = 0;
				foreach (Book item in list)
				{
					if (num >= b2)
					{
						break;
					}
					if (b3 == sbyte.MaxValue)
					{
						if (GetPlayer(operationUseEffectCard.Info.TargetBookOwnerPlayerId).RefBook(item).RemainingCd == 0)
						{
							continue;
						}
						GetPlayer(operationUseEffectCard.Info.TargetBookOwnerPlayerId).RefBook(item).RemainingCd = 0;
					}
					else if (b == playerId)
					{
						if (GetPlayer(operationUseEffectCard.Info.TargetBookOwnerPlayerId).RefBook(item).RemainingCd <= 0)
						{
							continue;
						}
						GetPlayer(operationUseEffectCard.Info.TargetBookOwnerPlayerId).RefBook(item).RemainingCd -= b3;
					}
					else if (b != playerId)
					{
						GetPlayer(operationUseEffectCard.Info.TargetBookOwnerPlayerId).RefBook(item).RemainingCd += b3;
					}
					num++;
					GetPlayer(operationUseEffectCard.Info.TargetBookOwnerPlayerId).RefBook(item).RemainingCd = Math.Max(0, GetPlayer(operationUseEffectCard.Info.TargetBookOwnerPlayerId).RefBook(item).RemainingCd);
					bookStateExtraDiffs.Add(new StatusSnapshotDiff.BookStateExtraDiff
					{
						OwnerPlayerId = operationUseEffectCard.Info.TargetBookOwnerPlayerId,
						BookCdIndex = player2.RefBookIndex(item),
						NewCdValue = GetPlayer(operationUseEffectCard.Info.TargetBookOwnerPlayerId).RefBook(item).RemainingCd,
						NewDisplayCdValue = GetPlayer(operationUseEffectCard.Info.TargetBookOwnerPlayerId).RefBook(item).DisplayCd,
						ByPlayerId = playerId
					});
				}
				break;
			}
			case ELifeSkillCombatEffectSubEffect.SelfDoPickByPoint:
			{
				Grid grid = GetGrid(operationUseEffectCard.Info.CellIndex);
				if (grid != null)
				{
					OperationPointBase thesis = grid.GetThesis();
					if (thesis != null && thesis.PlayerId == playerId)
					{
						player.RecruitEffectCards(context.Random, this, grid.GetThesisScore());
					}
				}
				break;
			}
			default:
				throw new NotImplementedException();
			}
			RecalculateThesisPoints();
		}
		else if (operation is OperationPointBase operationPointBase3)
		{
			Player player3 = GetPlayer(operationPointBase3.PlayerId);
			Grid grid4 = GetGrid(operationPointBase3.Coordinate);
			OperationGridBase activeOperation = grid4.ActiveOperation;
			OperationAnswer operationAnswer = operationPointBase3 as OperationAnswer;
			sbyte b6 = -1;
			foreach (int useFriendlyCharacterId in operationPointBase3.UseFriendlyCharacterIds)
			{
				operationPointBase3.BasePoint += player3.UseFriendlyCharacterAndGivePoint(context.Random, useFriendlyCharacterId, LifeSkillType);
			}
			foreach (sbyte effectiveEffectCardTemplateId2 in operationPointBase3.EffectiveEffectCardTemplateIds)
			{
				if (LifeSkillCombatEffect.Instance[effectiveEffectCardTemplateId2].SubEffect == ELifeSkillCombatEffectSubEffect.SelfNotCostBookStates)
				{
					b6 = effectiveEffectCardTemplateId2;
					break;
				}
			}
			foreach (sbyte effectiveEffectCardTemplateId3 in operationPointBase3.EffectiveEffectCardTemplateIds)
			{
				switch (LifeSkillCombatEffect.Instance[effectiveEffectCardTemplateId3].SubEffect)
				{
				case ELifeSkillCombatEffectSubEffect.SelfGridCoverBookStatesWhenAllQuestion:
				case ELifeSkillCombatEffectSubEffect.SelfTrapedInCell:
				case ELifeSkillCombatEffectSubEffect.SelfDoPickByPoint:
				case ELifeSkillCombatEffectSubEffect.SelfThesisChangeAroundHouseActivePointWithParam:
				case ELifeSkillCombatEffectSubEffect.SelfThesisWhenFixedEraseEffectsAroundHouseAllQuestionLowAndThesisLow:
				case ELifeSkillCombatEffectSubEffect.SelfThesisWhenFixedDoCancelAroundHouseAllQuestionLowAndThesisLow:
				case ELifeSkillCombatEffectSubEffect.PointChange:
					gridTrapStateExtraDiffs.Add(new StatusSnapshotDiff.GridTrapStateExtraDiff
					{
						GridIndex = operationPointBase3.GridIndex,
						OwnerPlayerId = operationPointBase3.PlayerId,
						Type = StatusSnapshotDiff.GridTrapStateExtraDiff.TrapChangeType.Added
					});
					break;
				}
			}
			if (b6 < 0)
			{
				if (operationPointBase3 is OperationQuestion operationQuestion)
				{
					foreach (Book effectiveBookState in operationQuestion.EffectiveBookStates)
					{
						Assert(!effectiveBookState.IsCd, "using bookState when [IsCd]");
						player3.RefBook(effectiveBookState).RemainingCd = 2;
						bookStateExtraDiffs.Add(new StatusSnapshotDiff.BookStateExtraDiff
						{
							OwnerPlayerId = player3.Id,
							BookCdIndex = player3.RefBookIndex(effectiveBookState),
							NewCdValue = player3.RefBook(effectiveBookState).RemainingCd,
							NewDisplayCdValue = player3.RefBook(effectiveBookState).DisplayCd,
							ByPlayerId = -1
						});
					}
				}
				if (operationAnswer != null)
				{
					bool flag = false;
					foreach (sbyte effectiveEffectCardTemplateId4 in GetAnswerTarget(operationAnswer).EffectiveEffectCardTemplateIds)
					{
						switch (LifeSkillCombatEffect.Instance[effectiveEffectCardTemplateId4].SubEffect)
						{
						case ELifeSkillCombatEffectSubEffect.SelfTrapedInCell:
							if (LifeSkillCombatEffect.Instance[effectiveEffectCardTemplateId4].Type == ELifeSkillCombatEffectType.BUFF)
							{
								int delta = LifeSkillCombatEffect.Instance[effectiveEffectCardTemplateId4].SubEffectParameters[3] * 10;
								operationAnswer.ChangeBasePoint(delta);
							}
							else if (LifeSkillCombatEffect.Instance[effectiveEffectCardTemplateId4].Type == ELifeSkillCombatEffectType.Strategy)
							{
								flag = true;
							}
							gridTrapStateExtraDiffs.Add(new StatusSnapshotDiff.GridTrapStateExtraDiff
							{
								GridIndex = operationAnswer.GridIndex,
								OwnerPlayerId = GetAnswerTarget(operationAnswer).PlayerId,
								Type = StatusSnapshotDiff.GridTrapStateExtraDiff.TrapChangeType.Triggered
							});
							break;
						case ELifeSkillCombatEffectSubEffect.SelfGridCoverBookStatesWhenAllQuestion:
						case ELifeSkillCombatEffectSubEffect.SelfDoPickByPoint:
						case ELifeSkillCombatEffectSubEffect.SelfThesisChangeAroundHouseActivePointWithParam:
						case ELifeSkillCombatEffectSubEffect.SelfThesisWhenFixedEraseEffectsAroundHouseAllQuestionLowAndThesisLow:
						case ELifeSkillCombatEffectSubEffect.SelfThesisWhenFixedDoCancelAroundHouseAllQuestionLowAndThesisLow:
						case ELifeSkillCombatEffectSubEffect.PointChange:
							gridTrapStateExtraDiffs.Add(new StatusSnapshotDiff.GridTrapStateExtraDiff
							{
								GridIndex = operationAnswer.GridIndex,
								OwnerPlayerId = GetAnswerTarget(operationAnswer).PlayerId,
								Type = StatusSnapshotDiff.GridTrapStateExtraDiff.TrapChangeType.Triggered
							});
							break;
						}
					}
					foreach (Book effectiveBookState2 in operationAnswer.EffectiveBookStates)
					{
						Assert(!effectiveBookState2.IsCd, "using bookState when [IsCd]");
						player3.RefBook(effectiveBookState2).RemainingCd = (flag ? 3 : 2);
						bookStateExtraDiffs.Add(new StatusSnapshotDiff.BookStateExtraDiff
						{
							OwnerPlayerId = player3.Id,
							BookCdIndex = player3.RefBookIndex(effectiveBookState2),
							NewCdValue = player3.RefBook(effectiveBookState2).RemainingCd,
							NewDisplayCdValue = player3.RefBook(effectiveBookState2).DisplayCd,
							ByPlayerId = -1
						});
					}
				}
			}
			int num2 = -1;
			foreach (sbyte effectiveEffectCardTemplateId5 in operationPointBase3.EffectiveEffectCardTemplateIds)
			{
				if (LifeSkillCombatEffect.Instance[effectiveEffectCardTemplateId5].SubEffect == ELifeSkillCombatEffectSubEffect.PointChange)
				{
					num2 = effectiveEffectCardTemplateId5;
					break;
				}
			}
			if (num2 >= 0)
			{
				int delta2 = LifeSkillCombatEffect.Instance[num2].SubEffectParameters[3] * 10;
				operationPointBase3.ChangeBasePoint(delta2);
			}
			bool flag2 = false;
			foreach (sbyte effectiveEffectCardTemplateId6 in operationPointBase3.EffectiveEffectCardTemplateIds)
			{
				ELifeSkillCombatEffectSubEffect subEffect = LifeSkillCombatEffect.Instance[effectiveEffectCardTemplateId6].SubEffect;
				ELifeSkillCombatEffectSubEffect eLifeSkillCombatEffectSubEffect = subEffect;
				if ((uint)(eLifeSkillCombatEffectSubEffect - 8) <= 1u)
				{
					flag2 = true;
				}
			}
			if (flag2)
			{
				grid4.SetActiveOperation(operationPointBase3, this, gridTrapStateExtraDiffs);
				grid4.SetActiveOperation(null, this, gridTrapStateExtraDiffs);
			}
			else if (!(activeOperation is OperationPointBase operationPointBase4) || operationPointBase4.Point < operationPointBase3.Point)
			{
				grid4.SetActiveOperation(operationPointBase3, this, gridTrapStateExtraDiffs);
				if (operationAnswer != null)
				{
					grid4.SetActiveOperation(null, this, gridTrapStateExtraDiffs);
					OperationQuestion answerTarget = GetAnswerTarget(operationAnswer);
					Player player4 = GetPlayer(answerTarget.PlayerId);
					foreach (Book effectiveBookState3 in answerTarget.EffectiveBookStates)
					{
						player4.RefBook(effectiveBookState3).RemainingCd++;
						bookStateExtraDiffs.Add(new StatusSnapshotDiff.BookStateExtraDiff
						{
							OwnerPlayerId = player4.Id,
							BookCdIndex = player4.RefBookIndex(effectiveBookState3),
							NewCdValue = player4.RefBook(effectiveBookState3).RemainingCd,
							NewDisplayCdValue = player4.RefBook(effectiveBookState3).DisplayCd,
							ByPlayerId = -1
						});
					}
				}
			}
			foreach (sbyte effectiveEffectCardTemplateId7 in operationPointBase3.EffectiveEffectCardTemplateIds)
			{
				player3.DropEffectCard(effectiveEffectCardTemplateId7);
			}
			SwitchCurrentPlayerProcess(context, bookStateExtraDiffs, gridTrapStateExtraDiffs, withProcessActiveOperationCells);
		}
		else
		{
			if (!(operation is OperationPrepareForceAdversary operationPrepareForceAdversary))
			{
				return;
			}
			Player player5 = GetPlayer(operationPrepareForceAdversary.PlayerId);
			Player player6 = GetPlayer(Player.PredefinedId.GetTheOtherSide(operationPrepareForceAdversary.PlayerId));
			switch (operationPrepareForceAdversary.Type)
			{
			case OperationPrepareForceAdversary.ForceAdversaryOperation.Silent:
			{
				int num3 = 3 - player5.ForceSilentRemainingCount;
				DomainManager.TaiwuEvent.OnLifeSkillCombatForceSilent(player6.CharacterId, (sbyte)player6.ForcedSilentCount, (sbyte)(num3 - player6.ForcedSilentCount));
				break;
			}
			case OperationPrepareForceAdversary.ForceAdversaryOperation.GiveUp:
			{
				player5.SetForceGiveUpRemainingCount(player5.ForceGiveUpRemainingCount - 1);
				short attainment = player5.Attainment;
				short attainment2 = player6.Attainment;
				if ((attainment >= 50 && attainment2 >= 50) ? context.Random.CheckPercentProb((int)Math.Pow(attainment - attainment2 - 80, 1.0499999523162842)) : context.Random.CheckPercentProb((int)Math.Pow(attainment - attainment2 - 50, 1.0499999523162842)))
				{
					_suiciderPlayerId = player6.Id;
				}
				break;
			}
			default:
				throw new ArgumentOutOfRangeException();
			}
		}
	}

	public OperationList CalcUsableOperationList(IEnumerable<sbyte> additionalEffectCards, IEnumerable<sbyte> usedBookStateIndices, IEnumerable<int> usedFriendlyCharacterIds, out bool maybeHasAdditionalOperation, bool exceptIncompleteMatchingEffectCards = true)
	{
		Player player = GetPlayer(_currentPlayerId);
		sbyte playerId = player.Id;
		OperationList operations = new OperationList();
		operations.Add(new OperationSilent(playerId, PlayerSwitchCount));
		operations.Add(new OperationGiveUp(playerId, PlayerSwitchCount));
		if (AcceptSilentPlayerId == playerId)
		{
			maybeHasAdditionalOperation = false;
			return operations;
		}
		HashSet<sbyte> wantUseEffectCards = new HashSet<sbyte>();
		if (additionalEffectCards != null)
		{
			wantUseEffectCards.UnionWith(additionalEffectCards);
		}
		Book[] usedBookStates = usedBookStateIndices.Select((sbyte index) => player.BookStates[index]).ToArray();
		(int CharacterId, bool Usable, int BasePoint)[] usedFriendlyCharacterStates = (from s in usedFriendlyCharacterIds.Select(delegate(int id)
			{
				foreach (var friendlyCharacterState in player.FriendlyCharacterStates)
				{
					if (friendlyCharacterState.CharacterId == id)
					{
						return friendlyCharacterState;
					}
				}
				AdaptableLog.TagWarning("LifeSkillCombat", $"request character {id} isn't exist in PlayerHouse[{playerId}]'s teammate");
				return (CharacterId: -1, Usable: false, BasePoint: 0);
			})
			where s.CharacterId >= 0
			select s).ToArray();
		int point = 0;
		Book[] array = usedBookStates;
		for (int num = 0; num < array.Length; num++)
		{
			Book book = array[num];
			point += book.BasePoint;
		}
		(int, bool, int)[] array2 = usedFriendlyCharacterStates;
		for (int num2 = 0; num2 < array2.Length; num2++)
		{
			(int, bool, int) tuple = array2[num2];
			point += tuple.Item3;
		}
		maybeHasAdditionalOperation = wantUseEffectCards.Any((sbyte index) => LifeSkillCombatEffect.Instance[index].SubEffect == ELifeSkillCombatEffectSubEffect.SelfExtraQuestionAroundHouseThesisLow || LifeSkillCombatEffect.Instance[index].SubEffect == ELifeSkillCombatEffectSubEffect.SelfExtraQuestionOnHouseThesisLowAndTransition || LifeSkillCombatEffect.Instance[index].SubEffect == ELifeSkillCombatEffectSubEffect.SelfExtraQuestionAroundHouseThesisBreakWhenAdversaryThesisHigh || LifeSkillCombatEffect.Instance[index].SubEffect == ELifeSkillCombatEffectSubEffect.SelfExtraQuestionOnHouseThesisLowAndRecycleCardAndExchangeOperation);
		HashSet<Coordinate2D<sbyte>> hashSet = new HashSet<Coordinate2D<sbyte>>();
		HashSet<Coordinate2D<sbyte>> hashSet2 = new HashSet<Coordinate2D<sbyte>>();
		HashSet<Coordinate2D<sbyte>> hashSet3 = new HashSet<Coordinate2D<sbyte>>();
		HashSet<Coordinate2D<sbyte>> additionalIndices = new HashSet<Coordinate2D<sbyte>>();
		int num3 = 0;
		for (int num4 = _gridStatus.Length; num3 < num4; num3++)
		{
			Grid grid = _gridStatus[num3];
			Coordinate2D<sbyte> coordinate = grid.Coordinate;
			OperationPointBase thesis = grid.GetThesis();
			OperationGridBase activeOperation = grid.ActiveOperation;
			if (thesis != null && thesis.PlayerId == player.Id)
			{
				(sbyte, sbyte)[] offset = _Offset8;
				for (int num5 = 0; num5 < offset.Length; num5++)
				{
					var (b, b2) = offset[num5];
					hashSet2.Add(new Coordinate2D<sbyte>((sbyte)(coordinate.X + b), (sbyte)(coordinate.Y + b2)));
				}
			}
			else
			{
				if (activeOperation == null || activeOperation.PlayerId == player.Id || !(activeOperation is OperationQuestion operationQuestion))
				{
					continue;
				}
				hashSet3.Add(grid.Coordinate);
				if (!(operationQuestion is OperationQuestionRhetorical))
				{
					(sbyte, sbyte)[] offset2 = _Offset8;
					for (int num6 = 0; num6 < offset2.Length; num6++)
					{
						var (b3, b4) = offset2[num6];
						hashSet.Add(new Coordinate2D<sbyte>((sbyte)(coordinate.X + b3), (sbyte)(coordinate.Y + b4)));
					}
				}
			}
		}
		for (sbyte b5 = -3; b5 <= 3; b5++)
		{
			for (sbyte b6 = -3; b6 <= 3; b6++)
			{
				if ((Math.Abs(b6) == 3 && Math.Abs(b5) == 3) || (Math.Abs(b6) == 3 && b5 == 0) || (b6 == 0 && Math.Abs(b5) == 3))
				{
					hashSet2.Add(new Coordinate2D<sbyte>(b6, b5));
				}
			}
		}
		foreach (sbyte item in wantUseEffectCards)
		{
			sbyte e = item;
			sbyte b7 = CalcTargetPlayerId(LifeSkillCombatEffect.Instance[e].SubEffectParameters[0], player.Id);
			sbyte num7 = LifeSkillCombatEffect.Instance[e].SubEffectParameters[1];
			sbyte num8 = LifeSkillCombatEffect.Instance[e].SubEffectParameters[2];
			sbyte b8 = LifeSkillCombatEffect.Instance[e].SubEffectParameters[3];
			sbyte cross = num8;
			sbyte straight = num7;
			sbyte house = b7;
			for (int num9 = 0; num9 < _gridStatus.Length; num9++)
			{
				Grid grid2 = _gridStatus[num9];
				OperationPointBase thesis2 = grid2.GetThesis();
				Coordinate2D<sbyte> coord = grid2.Coordinate;
				switch (LifeSkillCombatEffect.Instance[e].SubEffect)
				{
				case ELifeSkillCombatEffectSubEffect.SelfExtraQuestionAroundHouseThesisLow:
					if (thesis2 == null || thesis2.PlayerId != house || thesis2.Point >= point)
					{
						break;
					}
					foreach (Coordinate2D<sbyte> item2 in OffsetIterate(coord, straight, cross))
					{
						if (!AddExtraAdditionalQuestionOnGrid(item2, checkOriginal: true))
						{
							maybeHasAdditionalOperation = true;
						}
					}
					break;
				case ELifeSkillCombatEffectSubEffect.SelfExtraQuestionAroundHouseThesisBreakWhenAdversaryThesisHigh:
					if (thesis2 == null || thesis2.PlayerId != house)
					{
						break;
					}
					foreach (Coordinate2D<sbyte> item3 in OffsetIterate(coord, straight, cross, delegate(Coordinate2D<sbyte> p)
					{
						if (p == coord)
						{
							return false;
						}
						OperationPointBase thesis3 = GetGrid(p).GetThesis();
						return thesis3 != null && thesis3.PlayerId != house && thesis3.Point > point;
					}))
					{
						if (!AddExtraAdditionalQuestionOnGrid(item3, checkOriginal: true))
						{
							maybeHasAdditionalOperation = true;
						}
					}
					break;
				case ELifeSkillCombatEffectSubEffect.SelfExtraQuestionOnHouseThesisLowAndTransition:
					if (thesis2 != null && thesis2.PlayerId == house && thesis2.Point < point && !AddExtraAdditionalQuestionOnGrid(grid2.Coordinate, checkOriginal: false))
					{
						maybeHasAdditionalOperation = true;
					}
					break;
				case ELifeSkillCombatEffectSubEffect.SelfExtraQuestionOnHouseThesisLowAndRecycleCardAndExchangeOperation:
					if (thesis2 != null && thesis2.PlayerId == playerId && !AddExtraAdditionalQuestionOnGrid(grid2.Coordinate, checkOriginal: false))
					{
						maybeHasAdditionalOperation = true;
					}
					break;
				}
				bool AddExtraAdditionalQuestionOnGrid(Coordinate2D<sbyte> target, bool checkOriginal)
				{
					if (checkOriginal)
					{
						if (additionalIndices.Contains(target) || target == coord || GetGrid(target).GetThesis() != null)
						{
							return true;
						}
					}
					else if (additionalIndices.Contains(grid2.Coordinate))
					{
						return true;
					}
					additionalIndices.Add(target);
					operations.Add(new OperationQuestionAdditional(player.Id, PlayerSwitchCount, Grid.ToGridIndex(target), point, wantUseEffectCards, e).CommitUsedBookStates(usedBookStates, usedFriendlyCharacterStates, withPoint: false));
					return false;
				}
			}
		}
		hashSet3.ExceptWith(additionalIndices);
		hashSet2.ExceptWith(additionalIndices);
		hashSet.ExceptWith(additionalIndices);
		hashSet2.ExceptWith(hashSet3);
		hashSet.ExceptWith(hashSet3);
		hashSet.ExceptWith(hashSet2);
		foreach (Coordinate2D<sbyte> item4 in hashSet2)
		{
			if (Math.Abs(item4.X) <= 3 && Math.Abs(item4.Y) <= 3)
			{
				Grid grid3 = GetGrid(item4);
				if (grid3.ActiveOperation == null && grid3.GetThesis() == null)
				{
					operations.Add(new OperationQuestion(player.Id, PlayerSwitchCount, Grid.ToGridIndex(item4), point, wantUseEffectCards).CommitUsedBookStates(usedBookStates, usedFriendlyCharacterStates, withPoint: false));
				}
			}
		}
		foreach (Coordinate2D<sbyte> item5 in hashSet)
		{
			if (Math.Abs(item5.X) <= 3 && Math.Abs(item5.Y) <= 3)
			{
				Grid grid4 = GetGrid(item5);
				if (grid4.ActiveOperation == null && grid4.GetThesis() == null)
				{
					operations.Add(new OperationQuestionRhetorical(player.Id, PlayerSwitchCount, grid4.Index, point, wantUseEffectCards).CommitUsedBookStates(usedBookStates, usedFriendlyCharacterStates, withPoint: false));
				}
			}
		}
		foreach (Coordinate2D<sbyte> item6 in hashSet3)
		{
			if (Math.Abs(item6.X) <= 3 && Math.Abs(item6.Y) <= 3)
			{
				Grid grid5 = GetGrid(item6);
				operations.Add(new OperationAnswer(player.Id, PlayerSwitchCount, grid5.Index, point, wantUseEffectCards).CommitUsedBookStates(usedBookStates, usedFriendlyCharacterStates, withPoint: false));
			}
		}
		if (player.ForceSilentRemainingCount > 0)
		{
			operations.Add(new OperationPrepareForceAdversary(playerId, PlayerSwitchCount, OperationPrepareForceAdversary.ForceAdversaryOperation.Silent));
		}
		if (player.ForceGiveUpRemainingCount > 0)
		{
			operations.Add(new OperationPrepareForceAdversary(playerId, PlayerSwitchCount, OperationPrepareForceAdversary.ForceAdversaryOperation.GiveUp));
		}
		if (exceptIncompleteMatchingEffectCards)
		{
			HashSet<sbyte> hashSet4 = new HashSet<sbyte>();
			HashSet<sbyte> hashSet5 = new HashSet<sbyte>();
			for (int num10 = operations.Count - 1; num10 >= 0; num10--)
			{
				if (operations[num10] is OperationPointBase operationPointBase)
				{
					hashSet4.Clear();
					hashSet4.UnionWith(operationPointBase.PickEffectiveEffectCards(wantUseEffectCards));
					foreach (sbyte item7 in hashSet4)
					{
						operationPointBase.RegisterEffectiveEffectCards(item7);
					}
					hashSet5.Clear();
					hashSet5.UnionWith(wantUseEffectCards);
					hashSet5.ExceptWith(hashSet4);
					if (hashSet5.Count > 0)
					{
						operations.RemoveAt(num10);
					}
				}
			}
		}
		return operations;
	}

	public OperationList CalcUsableSecondPhaseOperationList(OperationUseEffectCard firstPhaseOperation, IEnumerable<sbyte> additionalEffectCards, IEnumerable<sbyte> usedBookStateIndices, IEnumerable<int> usedFriendlyCharacterIds)
	{
		OperationList operationList = new OperationList();
		OperationUseEffectCard.UseEffectCardInfo info = firstPhaseOperation.Info;
		sbyte playerId = firstPhaseOperation.PlayerId;
		sbyte effectCardTemplateId = info.EffectCardTemplateId;
		LifeSkillCombatEffectItem lifeSkillCombatEffectItem = LifeSkillCombatEffect.Instance[effectCardTemplateId];
		switch (lifeSkillCombatEffectItem.SubEffect)
		{
		case ELifeSkillCombatEffectSubEffect.SelfEraseAroundSelfThesisHouseQuestionThesis:
		{
			sbyte b3 = CalcTargetPlayerId(lifeSkillCombatEffectItem.SubEffectParameters[0], playerId);
			foreach (Coordinate2D<sbyte> item in OffsetIterate(_gridStatus[firstPhaseOperation.Info.CellIndex].Coordinate, 1, 1))
			{
				Grid grid = GetGrid(item);
				OperationPointBase thesis = grid.GetThesis();
				if (thesis != null && thesis.PlayerId == b3 && thesis.Point < GetGrid(info.CellIndex).GetThesis().Point)
				{
					info.CellIndex2 = grid.Index;
					operationList.Add(new OperationUseEffectCard(playerId, PlayerSwitchCount, info));
				}
			}
			break;
		}
		case ELifeSkillCombatEffectSubEffect.SelfEraseAroundHouseQuestionEffects:
		{
			sbyte targetPlayerId = CalcTargetPlayerId(lifeSkillCombatEffectItem.SubEffectParameters[0], playerId);
			operationList.AddRange(_gridStatus.Where(delegate(Grid grid2)
			{
				OperationPointBase operationPointBase = grid2.GetThesis();
				if (operationPointBase == null)
				{
					operationPointBase = grid2.ActiveOperation as OperationPointBase;
				}
				return operationPointBase != null && operationPointBase.PlayerId == targetPlayerId;
			}).Select(delegate(Grid grid2)
			{
				info.CellIndex2 = grid2.Index;
				return new OperationUseEffectCard(playerId, PlayerSwitchCount, info);
			}));
			break;
		}
		case ELifeSkillCombatEffectSubEffect.SelfChangeBookCd:
		{
			sbyte b = CalcTargetPlayerId(lifeSkillCombatEffectItem.SubEffectParameters[0], playerId);
			sbyte b2 = lifeSkillCombatEffectItem.SubEffectParameters[4];
			Player player = GetPlayer(b);
			if (b2 == sbyte.MaxValue)
			{
				if (player.BookStates.Any((Book book) => book.RemainingCd != 0))
				{
					info.TargetBookOwnerPlayerId = b;
					operationList.Add(new OperationUseEffectCard(playerId, PlayerSwitchCount, info));
				}
			}
			else if (b == playerId)
			{
				if (player.BookStates.Any((Book book) => book.RemainingCd > 0))
				{
					info.TargetBookOwnerPlayerId = b;
					operationList.Add(new OperationUseEffectCard(playerId, PlayerSwitchCount, info));
				}
			}
			else if (b != playerId)
			{
				operationList.Add(new OperationUseEffectCard(playerId, PlayerSwitchCount, info));
			}
			break;
		}
		case ELifeSkillCombatEffectSubEffect.SelfDoPickByPoint:
			operationList.AddRange(_gridStatus.Where(delegate(Grid grid2)
			{
				OperationPointBase thesis2 = grid2.GetThesis();
				return thesis2 != null && thesis2.PlayerId == playerId;
			}).Select(delegate(Grid grid2)
			{
				info.CellIndex2 = grid2.Index;
				return new OperationUseEffectCard(playerId, PlayerSwitchCount, info);
			}));
			break;
		}
		return operationList;
	}

	public void CalcUsableFirstPhaseEffectCardInfo(sbyte effectCardTemplateId, out List<int> gridIndices, out List<sbyte> bookIndices)
	{
		sbyte currentPlayerId = _currentPlayerId;
		LifeSkillCombatEffectItem lifeSkillCombatEffectItem = LifeSkillCombatEffect.Instance[effectCardTemplateId];
		gridIndices = new List<int>();
		bookIndices = new List<sbyte>();
		ELifeSkillCombatEffectSubEffect subEffect = lifeSkillCombatEffectItem.SubEffect;
		ELifeSkillCombatEffectSubEffect eLifeSkillCombatEffectSubEffect = subEffect;
		if ((uint)(eLifeSkillCombatEffectSubEffect - 4) <= 2u)
		{
			sbyte b = CalcTargetPlayerId(lifeSkillCombatEffectItem.SubEffectParameters[0], currentPlayerId);
			if (ELifeSkillCombatEffectSubEffect.SelfEraseAroundSelfThesisHouseQuestionThesis == lifeSkillCombatEffectItem.SubEffect)
			{
				b = currentPlayerId;
			}
			Grid[] gridStatus = _gridStatus;
			foreach (Grid grid in gridStatus)
			{
				if (grid != null)
				{
					OperationPointBase thesis = grid.GetThesis();
					if (thesis != null && thesis.PlayerId == b)
					{
						gridIndices.Add(grid.Index);
					}
				}
			}
		}
		ELifeSkillCombatEffectSubEffect subEffect2 = lifeSkillCombatEffectItem.SubEffect;
		ELifeSkillCombatEffectSubEffect eLifeSkillCombatEffectSubEffect2 = subEffect2;
		if (eLifeSkillCombatEffectSubEffect2 != ELifeSkillCombatEffectSubEffect.SelfChangeBookCd)
		{
			return;
		}
		sbyte b2 = CalcTargetPlayerId(lifeSkillCombatEffectItem.SubEffectParameters[0], currentPlayerId);
		sbyte b3 = lifeSkillCombatEffectItem.SubEffectParameters[4];
		Player player = GetPlayer(b2);
		if (b3 == sbyte.MaxValue)
		{
			for (sbyte b4 = 0; b4 < 9; b4++)
			{
				if (player.BookStates[b4].RemainingCd != 0)
				{
					bookIndices.Add(b4);
				}
			}
		}
		else if (b2 == currentPlayerId)
		{
			for (sbyte b5 = 0; b5 < 9; b5++)
			{
				if (player.BookStates[b5].RemainingCd > 0)
				{
					bookIndices.Add(b5);
				}
			}
		}
		else if (b2 != currentPlayerId)
		{
			for (sbyte b6 = 0; b6 < 9; b6++)
			{
				bookIndices.Add(b6);
			}
		}
	}

	public List<sbyte> GetNotUsableEffectCardTemplateIds(DataContext context, IEnumerable<sbyte> usedBookStateIndices, IEnumerable<int> usedFriendlyCharacterIds, List<sbyte> selectedEffectCardTemplateIds)
	{
		HashSet<sbyte> hashSet = new HashSet<sbyte>();
		bool flag = false;
		bool maybeHasAdditionalOperation;
		OperationList operations = CalcUsableOperationList(selectedEffectCardTemplateIds, usedBookStateIndices, usedFriendlyCharacterIds, out maybeHasAdditionalOperation);
		foreach (sbyte selectedEffectCardTemplateId in selectedEffectCardTemplateIds)
		{
			LifeSkillCombatEffectItem lifeSkillCombatEffectItem = LifeSkillCombatEffect.Instance[selectedEffectCardTemplateId];
			hashSet.UnionWith(lifeSkillCombatEffectItem.BanCardList);
			hashSet.Add(selectedEffectCardTemplateId);
			if (lifeSkillCombatEffectItem.Group == ELifeSkillCombatEffectGroup.FlexibleFall)
			{
				flag = true;
			}
		}
		HashSet<sbyte> hashSet2 = GetPlayer(_currentPlayerId).EffectCards.ToHashSet();
		foreach (sbyte item in hashSet2)
		{
			if (!hashSet.Contains(item) && IsSpecialCard(item))
			{
				selectedEffectCardTemplateIds.Add(item);
				OperationList operations2 = CalcUsableOperationList(selectedEffectCardTemplateIds, usedBookStateIndices, usedFriendlyCharacterIds, out maybeHasAdditionalOperation);
				selectedEffectCardTemplateIds.RemoveAt(selectedEffectCardTemplateIds.Count - 1);
				if (!IsCardUsableInOperations(item, operations2))
				{
					hashSet.Add(item);
				}
			}
		}
		foreach (sbyte item2 in hashSet2)
		{
			LifeSkillCombatEffectItem lifeSkillCombatEffectItem2 = LifeSkillCombatEffect.Instance[item2];
			if (hashSet.Contains(item2))
			{
				continue;
			}
			if (lifeSkillCombatEffectItem2.Group == ELifeSkillCombatEffectGroup.FlexibleFall && flag)
			{
				hashSet.Add(item2);
				continue;
			}
			if (!lifeSkillCombatEffectItem2.IsInstant && !IsSpecialCard(item2) && !IsCardUsableInOperations(item2, operations))
			{
				hashSet.Add(item2);
			}
			ELifeSkillCombatEffectSubEffect subEffect = lifeSkillCombatEffectItem2.SubEffect;
			ELifeSkillCombatEffectSubEffect eLifeSkillCombatEffectSubEffect = subEffect;
			if (eLifeSkillCombatEffectSubEffect != ELifeSkillCombatEffectSubEffect.SelfChangeBookCd)
			{
				continue;
			}
			sbyte b = CalcTargetPlayerId(lifeSkillCombatEffectItem2.SubEffectParameters[0], _currentPlayerId);
			sbyte b2 = lifeSkillCombatEffectItem2.SubEffectParameters[3];
			sbyte b3 = lifeSkillCombatEffectItem2.SubEffectParameters[4];
			Player player = GetPlayer(b);
			if (b != _currentPlayerId)
			{
				continue;
			}
			bool flag2 = false;
			for (int i = 0; i < 9; i++)
			{
				if (player.BookStates[i].IsCd)
				{
					flag2 = true;
					break;
				}
			}
			if (!flag2)
			{
				hashSet.Add(item2);
			}
		}
		hashSet.RemoveWhere(delegate(sbyte cardId)
		{
			LifeSkillCombatEffectItem lifeSkillCombatEffectItem3 = LifeSkillCombatEffect.Instance[cardId];
			return lifeSkillCombatEffectItem3.IsInstant && lifeSkillCombatEffectItem3.SubEffect != ELifeSkillCombatEffectSubEffect.SelfChangeBookCd;
		});
		return hashSet.ToList();
		static bool IsCardUsableInOperations(sbyte cardId, OperationList operationList)
		{
			foreach (OperationBase operation in operationList)
			{
				if (operation is OperationPointBase operationPointBase && operationPointBase.PickEffectiveEffectCards(Enumerable.Repeat(cardId, 1)).Contains(cardId))
				{
					return true;
				}
			}
			return false;
		}
		static bool IsSpecialCard(sbyte cardId)
		{
			return LifeSkillCombatEffect.Instance[cardId].SubEffect == ELifeSkillCombatEffectSubEffect.SelfExtraQuestionAroundHouseThesisLow || LifeSkillCombatEffect.Instance[cardId].SubEffect == ELifeSkillCombatEffectSubEffect.SelfExtraQuestionOnHouseThesisLowAndTransition || LifeSkillCombatEffect.Instance[cardId].SubEffect == ELifeSkillCombatEffectSubEffect.SelfExtraQuestionAroundHouseThesisBreakWhenAdversaryThesisHigh || LifeSkillCombatEffect.Instance[cardId].SubEffect == ELifeSkillCombatEffectSubEffect.SelfExtraQuestionOnHouseThesisLowAndRecycleCardAndExchangeOperation;
		}
	}

	public Ai GetAiState(sbyte playerId)
	{
		if (!_aiStates.TryGetValue(playerId, out var value) || value == null)
		{
			value = (_aiStates[playerId] = new Ai(this));
		}
		return value;
	}

	public OperationBase CalcAiOperation(DataContext context, OperationList usableOperationList)
	{
		RecordLineAi($"Start ai process, reference operations count: {usableOperationList.Count}");
		Ai aiState = GetAiState(_currentPlayerId);
		OperationBase result = aiState.GiveDecidedOperation(context, this);
		aiState.RecordInspect(this);
		return result;
	}

	public void SwitchCurrentPlayerProcess(DataContext context, IList<StatusSnapshotDiff.BookStateExtraDiff> bookStateExtraDiffs, IList<StatusSnapshotDiff.GridTrapStateExtraDiff> gridTrapStateExtraDiffs, bool withProcessActiveOperationCells = true)
	{
		if (withProcessActiveOperationCells)
		{
			ProcessActiveOperationCells(Player.PredefinedId.GetTheOtherSide(_currentPlayerId), gridTrapStateExtraDiffs);
		}
		RecalculateThesisPoints();
		RecordLine($"TURN {PlayerSwitchCount} IS OVER");
		if (_aiStates.TryGetValue(_currentPlayerId, out var value))
		{
			value.TurnClear();
		}
		_currentPlayerId = Player.PredefinedId.GetTheOtherSide(_currentPlayerId);
		Player player = GetPlayer(_currentPlayerId);
		player.UpdateBooksCd(this, bookStateExtraDiffs);
		player.UpdateBooksDisplayCd(this, bookStateExtraDiffs);
		player.RecruitEffectCards(context.Random, this, 1);
		PlayerSwitchCount++;
	}

	private void ProcessActiveOperationCells(sbyte playerId, IList<StatusSnapshotDiff.GridTrapStateExtraDiff> gridTrapStateExtraDiffs)
	{
		Grid[] gridStatus = _gridStatus;
		foreach (Grid grid in gridStatus)
		{
			if (grid.ActiveOperation != null && grid.ActiveOperation.PlayerId == playerId)
			{
				grid.SetActiveOperation(null, this, gridTrapStateExtraDiffs);
			}
		}
	}

	private void RecalculateThesisPoints()
	{
		Grid[] gridStatus = _gridStatus;
		foreach (Grid grid in gridStatus)
		{
			OperationGridBase operationGridBase = grid.GetThesis();
			if (operationGridBase == null)
			{
				operationGridBase = grid.ActiveOperation;
			}
			if (operationGridBase is OperationPointBase operationPointBase)
			{
				operationPointBase.AdditionalPoint = 0;
			}
		}
		Grid[] gridStatus2 = _gridStatus;
		foreach (Grid grid2 in gridStatus2)
		{
			OperationPointBase thesis = grid2.GetThesis();
			if (thesis == null)
			{
				continue;
			}
			foreach (sbyte effectiveEffectCardTemplateId in thesis.EffectiveEffectCardTemplateIds)
			{
				sbyte num = CalcTargetPlayerId(LifeSkillCombatEffect.Instance[effectiveEffectCardTemplateId].SubEffectParameters[0], thesis.PlayerId);
				sbyte num2 = LifeSkillCombatEffect.Instance[effectiveEffectCardTemplateId].SubEffectParameters[1];
				sbyte num3 = LifeSkillCombatEffect.Instance[effectiveEffectCardTemplateId].SubEffectParameters[2];
				sbyte b = LifeSkillCombatEffect.Instance[effectiveEffectCardTemplateId].SubEffectParameters[3];
				sbyte cross = num3;
				sbyte straight = num2;
				sbyte b2 = num;
				ELifeSkillCombatEffectSubEffect subEffect = LifeSkillCombatEffect.Instance[effectiveEffectCardTemplateId].SubEffect;
				ELifeSkillCombatEffectSubEffect eLifeSkillCombatEffectSubEffect = subEffect;
				if (eLifeSkillCombatEffectSubEffect != ELifeSkillCombatEffectSubEffect.SelfThesisChangeAroundHouseActivePointWithParam)
				{
					continue;
				}
				foreach (Coordinate2D<sbyte> item in OffsetIterate(grid2.Coordinate, straight, cross))
				{
					Grid grid3 = GetGrid(item);
					OperationGridBase operationGridBase2 = grid3.GetThesis();
					if (operationGridBase2 == null)
					{
						operationGridBase2 = grid3.ActiveOperation;
					}
					if (operationGridBase2 != null && operationGridBase2 is OperationPointBase operationPointBase2 && operationGridBase2.PlayerId == b2)
					{
						operationPointBase2.AdditionalPoint += b * 10;
					}
				}
			}
		}
	}

	public StatusSnapshot Dump()
	{
		if (!CheckResult(out var winnerPlayerId))
		{
			winnerPlayerId = -1;
		}
		return new StatusSnapshot(this, _gridStatus, _playerSelf, _playerAdversary, _currentPlayerId, winnerPlayerId);
	}

	public unsafe void Restore(StatusSnapshot snapshot)
	{
		for (int i = 0; i < 49; i++)
		{
			_gridStatus[i] = GameData.Serializer.Serializer.CreateCopy(snapshot.GridStatus[i]);
		}
		fixed (byte* pData = new byte[snapshot.Self.GetSerializedSize()])
		{
			snapshot.Self.Serialize(pData);
			_playerSelf.Deserialize(pData);
		}
		fixed (byte* pData2 = new byte[snapshot.Adversary.GetSerializedSize()])
		{
			snapshot.Adversary.Serialize(pData2);
			_playerAdversary.Deserialize(pData2);
		}
		_currentPlayerId = snapshot.CurrentPlayerId;
		PlayerSwitchCount = snapshot.PlayerSwitchCount;
		Tester.Assert(CalcPlayerScore(0) == snapshot.ScoreSelf);
		Tester.Assert(CalcPlayerScore(1) == snapshot.ScoreAdversary);
		if (CheckResult(out var winnerPlayerId))
		{
			Tester.Assert(winnerPlayerId == snapshot.WinnerPlayerId);
		}
	}

	public void SimulateOperationCommit(DataContext context, OperationBase operation, IList<StatusSnapshotDiff.BookStateExtraDiff> bookStateExtraDiffs, IList<StatusSnapshotDiff.GridTrapStateExtraDiff> gridTrapStateExtraDiffs)
	{
		Tester.Assert(_beforeSimulated == null);
		_beforeSimulated = Dump();
		CommitOperationProcess(context, operation, bookStateExtraDiffs, gridTrapStateExtraDiffs);
	}

	public void SimulateOperationCancel(DataContext context)
	{
		Tester.Assert(_beforeSimulated != null);
		Restore(_beforeSimulated);
		_beforeSimulated = null;
	}

	public void RecordLine(string message, int indent = 0)
	{
		for (int i = 0; i < indent; i++)
		{
			_recorder.Append("  ");
		}
		_recorder.AppendLine(message);
	}

	public void RecordDiff(StatusSnapshotDiff diff)
	{
		int num = 0;
		RecordLine("-----------------------------------------------------", num);
		RecordLine("State changed", num++);
		int i = 0;
		for (int count = diff.GridStatusDiffPrevious.Count; i < count; i++)
		{
			Grid grid = diff.GridStatusDiffPrevious[i];
			Grid grid2 = diff.GridStatusDiffCurrent[i];
			RecordLine($"Grid status {grid2.Coordinate}", num);
			num++;
			RecordLine(GridInspect(grid) ?? "", num);
			RecordLine("-> changed to", num);
			RecordLine(GridInspect(grid2) ?? "", num);
			num--;
		}
		if (diff.ScoreSelfDiff.HasValue)
		{
			RecordLine($"{PlayerName(0)} score -> {diff.ScoreSelfDiff.Value}", num);
		}
		if (diff.ScoreAdversaryDiff.HasValue)
		{
			RecordLine($"{PlayerName(1)} score -> {diff.ScoreAdversaryDiff.Value}", num);
		}
		if (diff.WinnerPlayerId.HasValue)
		{
			RecordLine("Yield winner -> " + PlayerName(diff.WinnerPlayerId.Value), num);
		}
		RecordLine("-----------------------------------------------------", --num);
		static string GridInspect(Grid grid3)
		{
			OperationGridBase activeOperation = grid3.ActiveOperation;
			string value = ((activeOperation == null) ? string.Empty : $"[Current {"ActiveOperation"}: {activeOperation}]");
			OperationPointBase thesis = grid3.GetThesis();
			string value2 = ((thesis == null) ? string.Empty : $"[Current Thesis: {thesis}]");
			return $"[Stacked operations count: {grid3.HistoryOperations().Count()}]{value2}{value}";
		}
		static string PlayerName(sbyte playerId)
		{
			return $"Player[{playerId}]";
		}
	}

	public void RecordLineAi(string message, int indent = 0)
	{
		RecordLine($"[House[{_currentPlayerId}] Ai]: {message}", indent);
	}

	public void Assert(bool condition, string message = "")
	{
		if (!condition)
		{
			RecordLine("--- Assertion failure --- " + message);
			GenerateRecordFile();
			throw new Exception("Assertion failure. " + message);
		}
	}

	public void GenerateRecordFile()
	{
		try
		{
			List<string> list = new List<string>();
			list.AddRange(Directory.EnumerateFiles("../Logs", "LifeSkillCombatLog_*", SearchOption.TopDirectoryOnly));
			if (list.Count > 20)
			{
				list.Sort(delegate(string pathL, string pathR)
				{
					DateTime lastWriteTime = File.GetLastWriteTime(pathL);
					DateTime lastWriteTime2 = File.GetLastWriteTime(pathR);
					return lastWriteTime.CompareTo(lastWriteTime2);
				});
				for (int num = 0; num < list.Count - 20; num++)
				{
					File.Delete(list[num]);
				}
			}
		}
		catch (Exception ex)
		{
			AdaptableLog.TagWarning("LifeSkillCombat Log", ex.Message);
		}
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("../Logs/LifeSkillCombatLog_");
		StringBuilder stringBuilder2 = stringBuilder;
		StringBuilder stringBuilder3 = stringBuilder2;
		StringBuilder.AppendInterpolatedStringHandler handler = new StringBuilder.AppendInterpolatedStringHandler(0, 1, stringBuilder2);
		handler.AppendFormatted(DateTime.Now, "yyyy-MM-dd_HH_mm_ss");
		stringBuilder3.Append(ref handler);
		stringBuilder2 = stringBuilder;
		StringBuilder stringBuilder4 = stringBuilder2;
		handler = new StringBuilder.AppendInterpolatedStringHandler(2, 1, stringBuilder2);
		handler.AppendLiteral("[");
		handler.AppendFormatted(DomainManager.Character.GetElement_Objects(_playerSelf.CharacterId));
		handler.AppendLiteral("]");
		stringBuilder4.Append(ref handler);
		stringBuilder2 = stringBuilder;
		StringBuilder stringBuilder5 = stringBuilder2;
		handler = new StringBuilder.AppendInterpolatedStringHandler(2, 1, stringBuilder2);
		handler.AppendLiteral("[");
		handler.AppendFormatted(DomainManager.Character.GetElement_Objects(_playerAdversary.CharacterId));
		handler.AppendLiteral("]");
		stringBuilder5.Append(ref handler);
		File.WriteAllText($"{stringBuilder}.txt", _recorder.ToString());
	}
}
