using System;
using System.Collections.Generic;
using System.Linq;
using Config;
using GameData.Common;
using GameData.Domains.Character;
using GameData.Domains.Combat;
using GameData.Domains.Extra;
using GameData.Domains.Taiwu.LifeSkillCombat.Snapshot;
using GameData.Serializer;
using GameData.Utilities;
using Redzen.Random;

namespace GameData.Domains.Taiwu.LifeSkillCombat.Status;

public class Player : ISerializableGameData
{
	public static class PredefinedId
	{
		public const sbyte Invalid = -1;

		public const sbyte Self = 0;

		public const sbyte Adversary = 1;

		public const sbyte Count = 2;

		public static string GetName(sbyte playerId)
		{
			return playerId switch
			{
				0 => "Self", 
				1 => "Adversary", 
				_ => "Unknown", 
			};
		}

		public static sbyte GetTheOtherSide(sbyte playerId)
		{
			return playerId switch
			{
				0 => 1, 
				1 => 0, 
				_ => -1, 
			};
		}
	}

	public const int MaxForceSilentCount = 3;

	public const int MaxForceGiveUpCount = 1;

	public const int BookCount = 9;

	public const int EffectCardCountMax = 9;

	public const int EffectCardInitialCount = 5;

	public const int EffectCardAddPerTurn = 1;

	private readonly Book[] _bookStates = new Book[9];

	private readonly List<sbyte> _effectCards = new List<sbyte>();

	public int ForcedSilentCount;

	public LifeSkillCombatCardCollection BackCardCollection;

	private readonly List<(int CharacterId, bool Usable, int BasePoint)> _friendlyCharacterStates = new List<(int, bool, int)>();

	public sbyte Id { get; private set; }

	public int CharacterId { get; private set; }

	public short Attainment { get; private set; }

	public int ForceSilentRemainingCount { get; private set; }

	public int ForceGiveUpRemainingCount { get; private set; }

	public int InducementCount { get; private set; }

	public IReadOnlyList<(int CharacterId, bool Usable, int BasePoint)> FriendlyCharacterStates => _friendlyCharacterStates;

	public IReadOnlyList<Book> BookStates => _bookStates;

	public IReadOnlyList<sbyte> EffectCards => _effectCards;

	public int RemainEffectCardPoolCount => BackCardCollection.CountSum;

	public Player()
	{
	}

	public void GetBookStates(List<Book> receiver, bool withClear = true)
	{
		if (withClear)
		{
			receiver.Clear();
		}
		receiver.AddRange(_bookStates);
	}

	public void GetEffectCards(List<sbyte> receiver, bool withClear = true)
	{
		if (withClear)
		{
			receiver.Clear();
		}
		receiver.AddRange(_effectCards);
	}

	public void GetFriendlyCharacterStates(List<(int CharacterId, bool Usable, int BasePoint)> receiver, bool withClear = true)
	{
		if (withClear)
		{
			receiver.Clear();
		}
		receiver.AddRange(_friendlyCharacterStates);
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 0;
		num++;
		num += 4;
		Book[] bookStates = _bookStates;
		foreach (Book book in bookStates)
		{
			num += book.GetSerializedSize();
		}
		num += 2 + _effectCards.Count;
		num += 2;
		num += 4;
		num += 4;
		num += 4;
		num += 4;
		num += BackCardCollection.GetSerializedSize();
		return num + (2 + 9 * _friendlyCharacterStates.Count);
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		*ptr = (byte)Id;
		ptr++;
		*(int*)ptr = CharacterId;
		ptr += 4;
		for (int i = 0; i < 9; i++)
		{
			ptr += _bookStates[i].Serialize(ptr);
		}
		Tester.Assert(_effectCards.Count < 65535);
		*(ushort*)ptr = (ushort)_effectCards.Count;
		ptr += 2;
		int j = 0;
		for (int count = _effectCards.Count; j < count; j++)
		{
			*ptr = (byte)_effectCards[j];
			ptr++;
		}
		*(short*)ptr = Attainment;
		ptr += 2;
		*(int*)ptr = ForceSilentRemainingCount;
		ptr += 4;
		*(int*)ptr = ForceGiveUpRemainingCount;
		ptr += 4;
		*(int*)ptr = ForcedSilentCount;
		ptr += 4;
		*(int*)ptr = InducementCount;
		ptr += 4;
		ptr += BackCardCollection.Serialize(ptr);
		Tester.Assert(_friendlyCharacterStates.Count < 65535);
		*(ushort*)ptr = (ushort)_friendlyCharacterStates.Count;
		ptr += 2;
		int k = 0;
		for (int count2 = _friendlyCharacterStates.Count; k < count2; k++)
		{
			*(int*)ptr = _friendlyCharacterStates[k].CharacterId;
			ptr += 4;
			*ptr = (_friendlyCharacterStates[k].Usable ? ((byte)1) : ((byte)0));
			ptr++;
			*(int*)ptr = _friendlyCharacterStates[k].BasePoint;
			ptr += 4;
		}
		return (int)(ptr - pData);
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		Id = (sbyte)(*ptr);
		ptr++;
		CharacterId = *(int*)ptr;
		ptr += 4;
		for (int i = 0; i < 9; i++)
		{
			Book book = default(Book);
			ptr += book.Deserialize(ptr);
			_bookStates[i] = book;
		}
		ushort num = *(ushort*)ptr;
		ptr += 2;
		_effectCards.Clear();
		for (int j = 0; j < num; j++)
		{
			_effectCards.Add((sbyte)(*ptr));
			ptr++;
		}
		Attainment = *(short*)ptr;
		ptr += 2;
		ForceSilentRemainingCount = *(int*)ptr;
		ptr += 4;
		ForceGiveUpRemainingCount = *(int*)ptr;
		ptr += 4;
		ForcedSilentCount = *(int*)ptr;
		ptr += 4;
		InducementCount = *(int*)ptr;
		ptr += 4;
		ptr += BackCardCollection.Deserialize(ptr);
		ushort num2 = *(ushort*)ptr;
		ptr += 2;
		_friendlyCharacterStates.Clear();
		for (int k = 0; k < num2; k++)
		{
			(int, bool, int) item = new(int, bool, int)
			{
				Item1 = *(int*)ptr
			};
			ptr += 4;
			item.Item2 = *ptr != 0;
			ptr++;
			item.Item3 = *(int*)ptr;
			ptr += 4;
			_friendlyCharacterStates.Add(item);
		}
		return (int)(ptr - pData);
	}

	public void SetForceSilentRemainingCount(int value)
	{
		ForceSilentRemainingCount = value;
	}

	public void SetForceGiveUpRemainingCount(int value)
	{
		ForceGiveUpRemainingCount = value;
	}

	public int UseFriendlyCharacterAndGivePoint(IRandomSource randomSource, int characterId, sbyte lifeSkillType)
	{
		for (int i = 0; i < _friendlyCharacterStates.Count; i++)
		{
			(int, bool, int) value = _friendlyCharacterStates[i];
			if (value.Item1 == characterId)
			{
				value.Item2 = false;
				_friendlyCharacterStates[i] = value;
				return value.Item3;
			}
		}
		throw new KeyNotFoundException();
	}

	public void DropAllUsableFriendlyCharacters()
	{
		_friendlyCharacterStates.Clear();
	}

	public Player(DataContext context, Match match, sbyte playerId, int characterId, sbyte lifeSkillType)
	{
		ForceSilentRemainingCount = 3;
		ForceGiveUpRemainingCount = 1;
		Id = playerId;
		match.RecordLine($"initializing player house[{playerId}] for character {characterId}");
		GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(characterId);
		List<GameData.Domains.Character.LifeSkillItem> learnedLifeSkills = element_Objects.GetLearnedLifeSkills();
		int i;
		for (i = 0; i < 9; i++)
		{
			_bookStates[i] = new Book(new GameData.Domains.Character.LifeSkillItem
			{
				SkillTemplateId = LifeSkill.Instance.First((Config.LifeSkillItem skl) => skl.Grade == i && skl.Type == lifeSkillType).TemplateId
			});
		}
		if (learnedLifeSkills == null)
		{
			foreach (Config.LifeSkillItem item2 in LifeSkill.Instance.Where((Config.LifeSkillItem e) => e.Type == lifeSkillType))
			{
				_bookStates[item2.Grade] = new Book(new GameData.Domains.Character.LifeSkillItem(item2.TemplateId));
			}
		}
		else
		{
			foreach (GameData.Domains.Character.LifeSkillItem item3 in learnedLifeSkills)
			{
				Config.LifeSkillItem lifeSkillItem = LifeSkill.Instance[item3.SkillTemplateId];
				if (lifeSkillItem.Type == lifeSkillType)
				{
					_bookStates[lifeSkillItem.Grade] = new Book(item3);
				}
			}
		}
		CharacterId = characterId;
		Attainment = element_Objects.GetLifeSkillAttainment(lifeSkillType);
		if (characterId != DomainManager.Taiwu.GetTaiwuCharId())
		{
			DomainManager.Extra.InitAiLifeSkillCombatUsedCard(context, lifeSkillType, characterId);
		}
		BackCardCollection = DomainManager.Extra.GetCharacterLifeSkillCombatUsedCard(context, lifeSkillType, characterId).Clone();
		_friendlyCharacterStates = new List<(int, bool, int)>();
		if (DomainManager.Taiwu.GetTaiwuCharId() == characterId)
		{
			for (int num = 0; num < 3; num++)
			{
				int element_CombatGroupCharIds = DomainManager.Taiwu.GetElement_CombatGroupCharIds(num);
				if (element_CombatGroupCharIds != characterId && element_CombatGroupCharIds >= 0)
				{
					GameData.Domains.Character.Character element_Objects2 = DomainManager.Character.GetElement_Objects(element_CombatGroupCharIds);
					int item = element_Objects2.GetLifeSkillAttainment(lifeSkillType) * context.Random.Next(80, 120) / 100 + element_Objects2.GetLifeSkillQualification(lifeSkillType) * context.Random.Next(80, 120) / 100;
					_friendlyCharacterStates.Add((element_CombatGroupCharIds, true, item));
					match.RecordLine($"add friendly character id {element_CombatGroupCharIds}");
				}
			}
		}
		else if (!DomainManager.Taiwu.IsInGroup(characterId))
		{
			List<int> list = new List<int>();
			switch (element_Objects.GetCreatingType())
			{
			case 1:
				DomainManager.Character.GetTaiwuCombatEnemyTeam(context, element_Objects, CombatType.Beat, list);
				break;
			case 2:
			{
				CharacterItem characterItem = Config.Character.Instance[element_Objects.GetTemplateId()];
				if (characterItem.MinionGroupId < 0)
				{
					break;
				}
				MinionGroupItem minionGroupItem = MinionGroup.Instance[characterItem.MinionGroupId];
				foreach (short minion in minionGroupItem.Minions)
				{
					int id = DomainManager.Character.CreateRandomEnemy(context, minion).GetId();
					DomainManager.Character.CompleteCreatingCharacter(id);
					DomainManager.Adventure.AddTemporaryEnemyId(id);
					list.Add(id);
				}
				break;
			}
			}
			foreach (int item4 in list)
			{
				if (item4 != characterId && item4 >= 0)
				{
					GameData.Domains.Character.Character element_Objects3 = DomainManager.Character.GetElement_Objects(item4);
					int num2 = element_Objects3.GetLifeSkillAttainment(lifeSkillType) * context.Random.Next(80, 120) / 100 + element_Objects3.GetLifeSkillQualification(lifeSkillType) * context.Random.Next(80, 120) / 100;
					if (num2 > 0)
					{
						_friendlyCharacterStates.Add((item4, true, num2));
						match.RecordLine($"add friendly character id {item4}");
					}
				}
			}
		}
		_friendlyCharacterStates.RemoveAll(((int CharacterId, bool Usable, int BasePoint) u) => u.CharacterId == characterId);
		RecalculateBooksPoint(context.Random);
		RecruitEffectCards(context.Random, match, 5);
	}

	public void UpdateBooksCd(Match match, IList<StatusSnapshotDiff.BookStateExtraDiff> bookStateExtraDiffs)
	{
		for (sbyte b = 0; b < 9; b++)
		{
			ref Book reference = ref _bookStates[b];
			if (reference.IsCd)
			{
				int remainingCd = reference.RemainingCd;
				reference.RemainingCd--;
				match.RecordLine($"House[{Id}] update book_state [{LifeSkill.Instance[reference.LifeSkill.SkillTemplateId].Name}] cd {remainingCd} -> {reference.RemainingCd}");
				bookStateExtraDiffs.Add(new StatusSnapshotDiff.BookStateExtraDiff
				{
					OwnerPlayerId = Id,
					BookCdIndex = b,
					NewCdValue = reference.RemainingCd,
					NewDisplayCdValue = reference.DisplayCd,
					ByPlayerId = -1
				});
			}
		}
	}

	public void UpdateBooksDisplayCd(Match match, IList<StatusSnapshotDiff.BookStateExtraDiff> bookStateExtraDiffs)
	{
		for (sbyte b = 0; b < 9; b++)
		{
			ref Book reference = ref _bookStates[b];
			if (reference.IsCd != reference.IsDisplayCd)
			{
				int coveringCd = reference.CoveringCd;
				reference.CoveringCd = -1;
				match.RecordLine($"House[{Id}] update book_state [{LifeSkill.Instance[reference.LifeSkill.SkillTemplateId].Name}] cd(display only) {coveringCd} -> {reference.CoveringCd}");
				bookStateExtraDiffs.Add(new StatusSnapshotDiff.BookStateExtraDiff
				{
					OwnerPlayerId = Id,
					BookCdIndex = b,
					NewCdValue = reference.RemainingCd,
					NewDisplayCdValue = reference.DisplayCd,
					ByPlayerId = -1
				});
			}
		}
	}

	public void RecalculateBookPoint(IRandomSource randomSource, int index)
	{
		ref Book reference = ref _bookStates[index];
		int num = Attainment * randomSource.Next(80, 120) / 100;
		int readPagesCount = reference.LifeSkill.GetReadPagesCount();
		num = num * readPagesCount / 5;
		reference.BasePoint = (short)(num + randomSource.Next(80, 120));
	}

	public void RecalculateBooksPoint(IRandomSource randomSource)
	{
		for (int i = 0; i < 9; i++)
		{
			RecalculateBookPoint(randomSource, i);
		}
	}

	public ref Book RefBook(Book finder)
	{
		return ref _bookStates[RefBookIndex(finder)];
	}

	public sbyte RefBookIndex(Book finder)
	{
		for (sbyte b = 0; b < 9; b++)
		{
			if (_bookStates[b].LifeSkill.SkillTemplateId == finder.LifeSkill.SkillTemplateId)
			{
				return b;
			}
		}
		throw new IndexOutOfRangeException();
	}

	public IEnumerable<sbyte> RecruitEffectCards(IRandomSource randomSource, Match match, int count)
	{
		List<sbyte> list = new List<sbyte>();
		list.AddRange(BackCardCollection.CardDict.Keys);
		List<sbyte> list2 = new List<sbyte>();
		int value = count;
		while (count > 0 && EffectCards.Count < 9)
		{
			count--;
			if (randomSource != null)
			{
				CollectionUtils.Shuffle(randomSource, list);
			}
			foreach (sbyte item in list)
			{
				if (EffectCards.Count >= 9)
				{
					break;
				}
				if (BackCardCollection.CardDict.TryGetValue(item, out var value2) && value2 > 0)
				{
					BackCardCollection.CardDict[item] = value2 - 1;
					_effectCards.Add(item);
					list2.Add(item);
					break;
				}
			}
		}
		match.RecordLine($"House[{Id}]: pick {list2.Count} (expected {value} )，now has {_effectCards.Count}: {{{string.Join(", ", EffectCards.Select((sbyte cardId) => LifeSkillCombatEffect.Instance[cardId].Name))}}}");
		return list2;
	}

	public IEnumerable<sbyte> RecruitEffectCards(Match match, IEnumerable<sbyte> source)
	{
		List<sbyte> list = new List<sbyte>();
		foreach (sbyte item in source)
		{
			if (EffectCards.Count >= 9)
			{
				break;
			}
			_effectCards.Add(item);
			list.Add(item);
		}
		match.RecordLine($"House[{Id}]: recruit {list.Count} cards via effect，now has {_effectCards.Count}");
		return list;
	}

	public bool DropEffectCard(sbyte card)
	{
		return _effectCards.Remove(card);
	}
}
