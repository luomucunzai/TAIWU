using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Config;
using GameData.Common;
using GameData.Domains.Character;
using GameData.Domains.Combat;
using GameData.Domains.Extra;
using GameData.Domains.Taiwu.LifeSkillCombat.Snapshot;
using GameData.Serializer;
using GameData.Utilities;
using Redzen.Random;

namespace GameData.Domains.Taiwu.LifeSkillCombat.Status
{
	// Token: 0x0200005C RID: 92
	public class Player : ISerializableGameData
	{
		// Token: 0x17000082 RID: 130
		// (get) Token: 0x06001546 RID: 5446 RVA: 0x00148D00 File Offset: 0x00146F00
		// (set) Token: 0x06001547 RID: 5447 RVA: 0x00148D08 File Offset: 0x00146F08
		public sbyte Id { get; private set; }

		// Token: 0x17000083 RID: 131
		// (get) Token: 0x06001548 RID: 5448 RVA: 0x00148D11 File Offset: 0x00146F11
		// (set) Token: 0x06001549 RID: 5449 RVA: 0x00148D19 File Offset: 0x00146F19
		public int CharacterId { get; private set; }

		// Token: 0x17000084 RID: 132
		// (get) Token: 0x0600154A RID: 5450 RVA: 0x00148D22 File Offset: 0x00146F22
		// (set) Token: 0x0600154B RID: 5451 RVA: 0x00148D2A File Offset: 0x00146F2A
		public short Attainment { get; private set; }

		// Token: 0x17000085 RID: 133
		// (get) Token: 0x0600154C RID: 5452 RVA: 0x00148D33 File Offset: 0x00146F33
		// (set) Token: 0x0600154D RID: 5453 RVA: 0x00148D3B File Offset: 0x00146F3B
		public int ForceSilentRemainingCount { get; private set; }

		// Token: 0x17000086 RID: 134
		// (get) Token: 0x0600154E RID: 5454 RVA: 0x00148D44 File Offset: 0x00146F44
		// (set) Token: 0x0600154F RID: 5455 RVA: 0x00148D4C File Offset: 0x00146F4C
		public int ForceGiveUpRemainingCount { get; private set; }

		// Token: 0x17000087 RID: 135
		// (get) Token: 0x06001550 RID: 5456 RVA: 0x00148D55 File Offset: 0x00146F55
		// (set) Token: 0x06001551 RID: 5457 RVA: 0x00148D5D File Offset: 0x00146F5D
		public int InducementCount { get; private set; }

		// Token: 0x06001552 RID: 5458 RVA: 0x00148D66 File Offset: 0x00146F66
		public Player()
		{
		}

		// Token: 0x06001553 RID: 5459 RVA: 0x00148D94 File Offset: 0x00146F94
		public void GetBookStates(List<Book> receiver, bool withClear = true)
		{
			if (withClear)
			{
				receiver.Clear();
			}
			receiver.AddRange(this._bookStates);
		}

		// Token: 0x06001554 RID: 5460 RVA: 0x00148DBC File Offset: 0x00146FBC
		public void GetEffectCards(List<sbyte> receiver, bool withClear = true)
		{
			if (withClear)
			{
				receiver.Clear();
			}
			receiver.AddRange(this._effectCards);
		}

		// Token: 0x06001555 RID: 5461 RVA: 0x00148DE4 File Offset: 0x00146FE4
		public void GetFriendlyCharacterStates([TupleElementNames(new string[]
		{
			"CharacterId",
			"Usable",
			"BasePoint"
		})] List<ValueTuple<int, bool, int>> receiver, bool withClear = true)
		{
			if (withClear)
			{
				receiver.Clear();
			}
			receiver.AddRange(this._friendlyCharacterStates);
		}

		// Token: 0x06001556 RID: 5462 RVA: 0x00148E0B File Offset: 0x0014700B
		public bool IsSerializedSizeFixed()
		{
			return false;
		}

		// Token: 0x06001557 RID: 5463 RVA: 0x00148E10 File Offset: 0x00147010
		public int GetSerializedSize()
		{
			int totalSize = 0;
			totalSize++;
			totalSize += 4;
			foreach (Book element in this._bookStates)
			{
				totalSize += element.GetSerializedSize();
			}
			totalSize += 2 + this._effectCards.Count;
			totalSize += 2;
			totalSize += 4;
			totalSize += 4;
			totalSize += 4;
			totalSize += 4;
			totalSize += this.BackCardCollection.GetSerializedSize();
			return totalSize + (2 + 9 * this._friendlyCharacterStates.Count);
		}

		// Token: 0x06001558 RID: 5464 RVA: 0x00148E9C File Offset: 0x0014709C
		public unsafe int Serialize(byte* pData)
		{
			*pData = (byte)this.Id;
			byte* pCurrData = pData + 1;
			*(int*)pCurrData = this.CharacterId;
			pCurrData += 4;
			for (int i = 0; i < 9; i++)
			{
				pCurrData += this._bookStates[i].Serialize(pCurrData);
			}
			Tester.Assert(this._effectCards.Count < 65535, "");
			*(short*)pCurrData = (short)((ushort)this._effectCards.Count);
			pCurrData += 2;
			int j = 0;
			int len = this._effectCards.Count;
			while (j < len)
			{
				*pCurrData = (byte)this._effectCards[j];
				pCurrData++;
				j++;
			}
			*(short*)pCurrData = this.Attainment;
			pCurrData += 2;
			*(int*)pCurrData = this.ForceSilentRemainingCount;
			pCurrData += 4;
			*(int*)pCurrData = this.ForceGiveUpRemainingCount;
			pCurrData += 4;
			*(int*)pCurrData = this.ForcedSilentCount;
			pCurrData += 4;
			*(int*)pCurrData = this.InducementCount;
			pCurrData += 4;
			pCurrData += this.BackCardCollection.Serialize(pCurrData);
			Tester.Assert(this._friendlyCharacterStates.Count < 65535, "");
			*(short*)pCurrData = (short)((ushort)this._friendlyCharacterStates.Count);
			pCurrData += 2;
			int k = 0;
			int len2 = this._friendlyCharacterStates.Count;
			while (k < len2)
			{
				*(int*)pCurrData = this._friendlyCharacterStates[k].Item1;
				pCurrData += 4;
				*pCurrData = (this._friendlyCharacterStates[k].Item2 ? 1 : 0);
				pCurrData++;
				*(int*)pCurrData = this._friendlyCharacterStates[k].Item3;
				pCurrData += 4;
				k++;
			}
			return (int)((long)(pCurrData - pData));
		}

		// Token: 0x06001559 RID: 5465 RVA: 0x00149050 File Offset: 0x00147250
		public unsafe int Deserialize(byte* pData)
		{
			this.Id = *(sbyte*)pData;
			byte* pCurrData = pData + 1;
			this.CharacterId = *(int*)pCurrData;
			pCurrData += 4;
			for (int i = 0; i < 9; i++)
			{
				Book element = default(Book);
				pCurrData += element.Deserialize(pCurrData);
				this._bookStates[i] = element;
			}
			ushort count = *(ushort*)pCurrData;
			pCurrData += 2;
			this._effectCards.Clear();
			for (int j = 0; j < (int)count; j++)
			{
				this._effectCards.Add(*(sbyte*)pCurrData);
				pCurrData++;
			}
			this.Attainment = *(short*)pCurrData;
			pCurrData += 2;
			this.ForceSilentRemainingCount = *(int*)pCurrData;
			pCurrData += 4;
			this.ForceGiveUpRemainingCount = *(int*)pCurrData;
			pCurrData += 4;
			this.ForcedSilentCount = *(int*)pCurrData;
			pCurrData += 4;
			this.InducementCount = *(int*)pCurrData;
			pCurrData += 4;
			pCurrData += this.BackCardCollection.Deserialize(pCurrData);
			ushort count2 = *(ushort*)pCurrData;
			pCurrData += 2;
			this._friendlyCharacterStates.Clear();
			for (int k = 0; k < (int)count2; k++)
			{
				ValueTuple<int, bool, int> b = default(ValueTuple<int, bool, int>);
				b.Item1 = *(int*)pCurrData;
				pCurrData += 4;
				b.Item2 = (*pCurrData != 0);
				pCurrData++;
				b.Item3 = *(int*)pCurrData;
				pCurrData += 4;
				this._friendlyCharacterStates.Add(b);
			}
			return (int)((long)(pCurrData - pData));
		}

		// Token: 0x17000088 RID: 136
		// (get) Token: 0x0600155A RID: 5466 RVA: 0x001491BA File Offset: 0x001473BA
		[TupleElementNames(new string[]
		{
			"CharacterId",
			"Usable",
			"BasePoint"
		})]
		public IReadOnlyList<ValueTuple<int, bool, int>> FriendlyCharacterStates
		{
			[return: TupleElementNames(new string[]
			{
				"CharacterId",
				"Usable",
				"BasePoint"
			})]
			get
			{
				return this._friendlyCharacterStates;
			}
		}

		// Token: 0x0600155B RID: 5467 RVA: 0x001491C2 File Offset: 0x001473C2
		public void SetForceSilentRemainingCount(int value)
		{
			this.ForceSilentRemainingCount = value;
		}

		// Token: 0x0600155C RID: 5468 RVA: 0x001491CC File Offset: 0x001473CC
		public void SetForceGiveUpRemainingCount(int value)
		{
			this.ForceGiveUpRemainingCount = value;
		}

		// Token: 0x0600155D RID: 5469 RVA: 0x001491D8 File Offset: 0x001473D8
		public int UseFriendlyCharacterAndGivePoint(IRandomSource randomSource, int characterId, sbyte lifeSkillType)
		{
			for (int i = 0; i < this._friendlyCharacterStates.Count; i++)
			{
				ValueTuple<int, bool, int> state = this._friendlyCharacterStates[i];
				bool flag = state.Item1 == characterId;
				if (flag)
				{
					state.Item2 = false;
					this._friendlyCharacterStates[i] = state;
					return state.Item3;
				}
			}
			throw new KeyNotFoundException();
		}

		// Token: 0x0600155E RID: 5470 RVA: 0x00149245 File Offset: 0x00147445
		public void DropAllUsableFriendlyCharacters()
		{
			this._friendlyCharacterStates.Clear();
		}

		// Token: 0x0600155F RID: 5471 RVA: 0x00149254 File Offset: 0x00147454
		public Player(DataContext context, Match match, sbyte playerId, int characterId, sbyte lifeSkillType)
		{
			Player.<>c__DisplayClass50_0 CS$<>8__locals1 = new Player.<>c__DisplayClass50_0();
			CS$<>8__locals1.lifeSkillType = lifeSkillType;
			CS$<>8__locals1.characterId = characterId;
			base..ctor();
			this.ForceSilentRemainingCount = 3;
			this.ForceGiveUpRemainingCount = 1;
			this.Id = playerId;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(42, 2);
			defaultInterpolatedStringHandler.AppendLiteral("initializing player house[");
			defaultInterpolatedStringHandler.AppendFormatted<sbyte>(playerId);
			defaultInterpolatedStringHandler.AppendLiteral("] for character ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(CS$<>8__locals1.characterId);
			match.RecordLine(defaultInterpolatedStringHandler.ToStringAndClear(), 0);
			GameData.Domains.Character.Character character = DomainManager.Character.GetElement_Objects(CS$<>8__locals1.characterId);
			List<GameData.Domains.Character.LifeSkillItem> lifeSkills = character.GetLearnedLifeSkills();
			int i;
			int i2;
			for (i = 0; i < 9; i = i2 + 1)
			{
				this._bookStates[i] = new Book(new GameData.Domains.Character.LifeSkillItem
				{
					SkillTemplateId = LifeSkill.Instance.First((Config.LifeSkillItem skl) => (int)skl.Grade == i && skl.Type == CS$<>8__locals1.lifeSkillType).TemplateId
				});
				i2 = i;
			}
			bool flag = lifeSkills == null;
			if (flag)
			{
				IEnumerable<Config.LifeSkillItem> instance = LifeSkill.Instance;
				Func<Config.LifeSkillItem, bool> predicate;
				if ((predicate = CS$<>8__locals1.<>9__2) == null)
				{
					predicate = (CS$<>8__locals1.<>9__2 = ((Config.LifeSkillItem e) => e.Type == CS$<>8__locals1.lifeSkillType));
				}
				foreach (Config.LifeSkillItem lifeSkill in instance.Where(predicate))
				{
					this._bookStates[(int)lifeSkill.Grade] = new Book(new GameData.Domains.Character.LifeSkillItem(lifeSkill.TemplateId));
				}
			}
			else
			{
				foreach (GameData.Domains.Character.LifeSkillItem lifeSkillItem in lifeSkills)
				{
					Config.LifeSkillItem config = LifeSkill.Instance[lifeSkillItem.SkillTemplateId];
					bool flag2 = config.Type != CS$<>8__locals1.lifeSkillType;
					if (!flag2)
					{
						this._bookStates[(int)config.Grade] = new Book(lifeSkillItem);
					}
				}
			}
			this.CharacterId = CS$<>8__locals1.characterId;
			this.Attainment = character.GetLifeSkillAttainment(CS$<>8__locals1.lifeSkillType);
			bool flag3 = CS$<>8__locals1.characterId != DomainManager.Taiwu.GetTaiwuCharId();
			if (flag3)
			{
				DomainManager.Extra.InitAiLifeSkillCombatUsedCard(context, CS$<>8__locals1.lifeSkillType, CS$<>8__locals1.characterId);
			}
			this.BackCardCollection = DomainManager.Extra.GetCharacterLifeSkillCombatUsedCard(context, CS$<>8__locals1.lifeSkillType, CS$<>8__locals1.characterId).Clone();
			this._friendlyCharacterStates = new List<ValueTuple<int, bool, int>>();
			bool flag4 = DomainManager.Taiwu.GetTaiwuCharId() == CS$<>8__locals1.characterId;
			if (flag4)
			{
				for (int j = 0; j < 3; j++)
				{
					int charId = DomainManager.Taiwu.GetElement_CombatGroupCharIds(j);
					bool flag5 = charId == CS$<>8__locals1.characterId || charId < 0;
					if (!flag5)
					{
						GameData.Domains.Character.Character ch = DomainManager.Character.GetElement_Objects(charId);
						int point = (int)ch.GetLifeSkillAttainment(CS$<>8__locals1.lifeSkillType) * context.Random.Next(80, 120) / 100 + (int)ch.GetLifeSkillQualification(CS$<>8__locals1.lifeSkillType) * context.Random.Next(80, 120) / 100;
						this._friendlyCharacterStates.Add(new ValueTuple<int, bool, int>(charId, true, point));
						defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(26, 1);
						defaultInterpolatedStringHandler.AppendLiteral("add friendly character id ");
						defaultInterpolatedStringHandler.AppendFormatted<int>(charId);
						match.RecordLine(defaultInterpolatedStringHandler.ToStringAndClear(), 0);
					}
				}
			}
			else
			{
				bool flag6 = DomainManager.Taiwu.IsInGroup(CS$<>8__locals1.characterId);
				if (!flag6)
				{
					List<int> groupCharList = new List<int>();
					byte creatingType = character.GetCreatingType();
					byte b = creatingType;
					if (b != 1)
					{
						if (b == 2)
						{
							CharacterItem characterConfig = Config.Character.Instance[character.GetTemplateId()];
							bool flag7 = characterConfig.MinionGroupId >= 0;
							if (flag7)
							{
								MinionGroupItem minionGroup = MinionGroup.Instance[characterConfig.MinionGroupId];
								foreach (short templateId in minionGroup.Minions)
								{
									int enemyId = DomainManager.Character.CreateRandomEnemy(context, templateId, null).GetId();
									DomainManager.Character.CompleteCreatingCharacter(enemyId);
									DomainManager.Adventure.AddTemporaryEnemyId(enemyId);
									groupCharList.Add(enemyId);
								}
							}
						}
					}
					else
					{
						DomainManager.Character.GetTaiwuCombatEnemyTeam(context, character, CombatType.Beat, groupCharList);
					}
					foreach (int charId2 in groupCharList)
					{
						bool flag8 = charId2 == CS$<>8__locals1.characterId || charId2 < 0;
						if (!flag8)
						{
							GameData.Domains.Character.Character ch2 = DomainManager.Character.GetElement_Objects(charId2);
							int point2 = (int)ch2.GetLifeSkillAttainment(CS$<>8__locals1.lifeSkillType) * context.Random.Next(80, 120) / 100 + (int)ch2.GetLifeSkillQualification(CS$<>8__locals1.lifeSkillType) * context.Random.Next(80, 120) / 100;
							bool flag9 = point2 <= 0;
							if (!flag9)
							{
								this._friendlyCharacterStates.Add(new ValueTuple<int, bool, int>(charId2, true, point2));
								defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(26, 1);
								defaultInterpolatedStringHandler.AppendLiteral("add friendly character id ");
								defaultInterpolatedStringHandler.AppendFormatted<int>(charId2);
								match.RecordLine(defaultInterpolatedStringHandler.ToStringAndClear(), 0);
							}
						}
					}
				}
			}
			this._friendlyCharacterStates.RemoveAll(([TupleElementNames(new string[]
			{
				"CharacterId",
				"Usable",
				"BasePoint"
			})] ValueTuple<int, bool, int> u) => u.Item1 == CS$<>8__locals1.characterId);
			this.RecalculateBooksPoint(context.Random);
			this.RecruitEffectCards(context.Random, match, 5);
		}

		// Token: 0x17000089 RID: 137
		// (get) Token: 0x06001560 RID: 5472 RVA: 0x00149890 File Offset: 0x00147A90
		public IReadOnlyList<Book> BookStates
		{
			get
			{
				return this._bookStates;
			}
		}

		// Token: 0x06001561 RID: 5473 RVA: 0x00149898 File Offset: 0x00147A98
		public void UpdateBooksCd(Match match, IList<StatusSnapshotDiff.BookStateExtraDiff> bookStateExtraDiffs)
		{
			for (sbyte i = 0; i < 9; i += 1)
			{
				ref Book bookState = ref this._bookStates[(int)i];
				bool isCd = bookState.IsCd;
				if (isCd)
				{
					int preChange = bookState.RemainingCd;
					bookState.RemainingCd--;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(36, 4);
					defaultInterpolatedStringHandler.AppendLiteral("House[");
					defaultInterpolatedStringHandler.AppendFormatted<sbyte>(this.Id);
					defaultInterpolatedStringHandler.AppendLiteral("] update book_state [");
					defaultInterpolatedStringHandler.AppendFormatted(LifeSkill.Instance[bookState.LifeSkill.SkillTemplateId].Name);
					defaultInterpolatedStringHandler.AppendLiteral("] cd ");
					defaultInterpolatedStringHandler.AppendFormatted<int>(preChange);
					defaultInterpolatedStringHandler.AppendLiteral(" -> ");
					defaultInterpolatedStringHandler.AppendFormatted<int>(bookState.RemainingCd);
					match.RecordLine(defaultInterpolatedStringHandler.ToStringAndClear(), 0);
					bookStateExtraDiffs.Add(new StatusSnapshotDiff.BookStateExtraDiff
					{
						OwnerPlayerId = this.Id,
						BookCdIndex = i,
						NewCdValue = bookState.RemainingCd,
						NewDisplayCdValue = bookState.DisplayCd,
						ByPlayerId = -1
					});
				}
			}
		}

		// Token: 0x06001562 RID: 5474 RVA: 0x001499CC File Offset: 0x00147BCC
		public void UpdateBooksDisplayCd(Match match, IList<StatusSnapshotDiff.BookStateExtraDiff> bookStateExtraDiffs)
		{
			for (sbyte i = 0; i < 9; i += 1)
			{
				ref Book bookState = ref this._bookStates[(int)i];
				bool flag = bookState.IsCd != bookState.IsDisplayCd;
				if (flag)
				{
					int preChange = bookState.CoveringCd;
					bookState.CoveringCd = -1;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(50, 4);
					defaultInterpolatedStringHandler.AppendLiteral("House[");
					defaultInterpolatedStringHandler.AppendFormatted<sbyte>(this.Id);
					defaultInterpolatedStringHandler.AppendLiteral("] update book_state [");
					defaultInterpolatedStringHandler.AppendFormatted(LifeSkill.Instance[bookState.LifeSkill.SkillTemplateId].Name);
					defaultInterpolatedStringHandler.AppendLiteral("] cd(display only) ");
					defaultInterpolatedStringHandler.AppendFormatted<int>(preChange);
					defaultInterpolatedStringHandler.AppendLiteral(" -> ");
					defaultInterpolatedStringHandler.AppendFormatted<int>(bookState.CoveringCd);
					match.RecordLine(defaultInterpolatedStringHandler.ToStringAndClear(), 0);
					bookStateExtraDiffs.Add(new StatusSnapshotDiff.BookStateExtraDiff
					{
						OwnerPlayerId = this.Id,
						BookCdIndex = i,
						NewCdValue = bookState.RemainingCd,
						NewDisplayCdValue = bookState.DisplayCd,
						ByPlayerId = -1
					});
				}
			}
		}

		// Token: 0x06001563 RID: 5475 RVA: 0x00149B08 File Offset: 0x00147D08
		public void RecalculateBookPoint(IRandomSource randomSource, int index)
		{
			ref Book bookState = ref this._bookStates[index];
			int basePoint = (int)this.Attainment * randomSource.Next(80, 120) / 100;
			int readPageCount = bookState.LifeSkill.GetReadPagesCount();
			basePoint = basePoint * readPageCount / 5;
			bookState.BasePoint = (int)((short)(basePoint + randomSource.Next(80, 120)));
		}

		// Token: 0x06001564 RID: 5476 RVA: 0x00149B60 File Offset: 0x00147D60
		public void RecalculateBooksPoint(IRandomSource randomSource)
		{
			for (int i = 0; i < 9; i++)
			{
				this.RecalculateBookPoint(randomSource, i);
			}
		}

		// Token: 0x06001565 RID: 5477 RVA: 0x00149B88 File Offset: 0x00147D88
		public ref Book RefBook(Book finder)
		{
			return ref this._bookStates[(int)this.RefBookIndex(finder)];
		}

		// Token: 0x06001566 RID: 5478 RVA: 0x00149B9C File Offset: 0x00147D9C
		public sbyte RefBookIndex(Book finder)
		{
			for (sbyte i = 0; i < 9; i += 1)
			{
				bool flag = this._bookStates[(int)i].LifeSkill.SkillTemplateId == finder.LifeSkill.SkillTemplateId;
				if (flag)
				{
					return i;
				}
			}
			throw new IndexOutOfRangeException();
		}

		// Token: 0x1700008A RID: 138
		// (get) Token: 0x06001567 RID: 5479 RVA: 0x00149BF0 File Offset: 0x00147DF0
		public IReadOnlyList<sbyte> EffectCards
		{
			get
			{
				return this._effectCards;
			}
		}

		// Token: 0x1700008B RID: 139
		// (get) Token: 0x06001568 RID: 5480 RVA: 0x00149BF8 File Offset: 0x00147DF8
		public int RemainEffectCardPoolCount
		{
			get
			{
				return this.BackCardCollection.CountSum;
			}
		}

		// Token: 0x06001569 RID: 5481 RVA: 0x00149C08 File Offset: 0x00147E08
		public IEnumerable<sbyte> RecruitEffectCards(IRandomSource randomSource, Match match, int count)
		{
			List<sbyte> effectCardIds = new List<sbyte>();
			effectCardIds.AddRange(this.BackCardCollection.CardDict.Keys);
			List<sbyte> diff = new List<sbyte>();
			int originalCount = count;
			while (count > 0)
			{
				bool flag = this.EffectCards.Count >= 9;
				if (flag)
				{
					break;
				}
				count--;
				bool flag2 = randomSource != null;
				if (flag2)
				{
					CollectionUtils.Shuffle<sbyte>(randomSource, effectCardIds);
				}
				foreach (sbyte cardId2 in effectCardIds)
				{
					bool flag3 = this.EffectCards.Count >= 9;
					if (flag3)
					{
						break;
					}
					int remain;
					bool flag4 = this.BackCardCollection.CardDict.TryGetValue(cardId2, out remain) && remain > 0;
					if (flag4)
					{
						this.BackCardCollection.CardDict[cardId2] = remain - 1;
						this._effectCards.Add(cardId2);
						diff.Add(cardId2);
						break;
					}
				}
			}
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(40, 5);
			defaultInterpolatedStringHandler.AppendLiteral("House[");
			defaultInterpolatedStringHandler.AppendFormatted<sbyte>(this.Id);
			defaultInterpolatedStringHandler.AppendLiteral("]: pick ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(diff.Count);
			defaultInterpolatedStringHandler.AppendLiteral(" (expected ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(originalCount);
			defaultInterpolatedStringHandler.AppendLiteral(" )，now has ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(this._effectCards.Count);
			defaultInterpolatedStringHandler.AppendLiteral(": {");
			defaultInterpolatedStringHandler.AppendFormatted(string.Join(", ", from cardId in this.EffectCards
			select LifeSkillCombatEffect.Instance[cardId].Name));
			defaultInterpolatedStringHandler.AppendLiteral("}");
			match.RecordLine(defaultInterpolatedStringHandler.ToStringAndClear(), 0);
			return diff;
		}

		// Token: 0x0600156A RID: 5482 RVA: 0x00149E18 File Offset: 0x00148018
		public IEnumerable<sbyte> RecruitEffectCards(Match match, IEnumerable<sbyte> source)
		{
			List<sbyte> diff = new List<sbyte>();
			foreach (sbyte effectCard in source)
			{
				bool flag = this.EffectCards.Count >= 9;
				if (flag)
				{
					break;
				}
				this._effectCards.Add(effectCard);
				diff.Add(effectCard);
			}
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(43, 3);
			defaultInterpolatedStringHandler.AppendLiteral("House[");
			defaultInterpolatedStringHandler.AppendFormatted<sbyte>(this.Id);
			defaultInterpolatedStringHandler.AppendLiteral("]: recruit ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(diff.Count);
			defaultInterpolatedStringHandler.AppendLiteral(" cards via effect，now has ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(this._effectCards.Count);
			match.RecordLine(defaultInterpolatedStringHandler.ToStringAndClear(), 0);
			return diff;
		}

		// Token: 0x0600156B RID: 5483 RVA: 0x00149F08 File Offset: 0x00148108
		public bool DropEffectCard(sbyte card)
		{
			return this._effectCards.Remove(card);
		}

		// Token: 0x04000355 RID: 853
		public const int MaxForceSilentCount = 3;

		// Token: 0x04000356 RID: 854
		public const int MaxForceGiveUpCount = 1;

		// Token: 0x04000357 RID: 855
		public const int BookCount = 9;

		// Token: 0x04000358 RID: 856
		public const int EffectCardCountMax = 9;

		// Token: 0x04000359 RID: 857
		public const int EffectCardInitialCount = 5;

		// Token: 0x0400035A RID: 858
		public const int EffectCardAddPerTurn = 1;

		// Token: 0x0400035D RID: 861
		private readonly Book[] _bookStates = new Book[9];

		// Token: 0x0400035E RID: 862
		private readonly List<sbyte> _effectCards = new List<sbyte>();

		// Token: 0x04000362 RID: 866
		public int ForcedSilentCount;

		// Token: 0x04000364 RID: 868
		public LifeSkillCombatCardCollection BackCardCollection;

		// Token: 0x04000365 RID: 869
		[TupleElementNames(new string[]
		{
			"CharacterId",
			"Usable",
			"BasePoint"
		})]
		private readonly List<ValueTuple<int, bool, int>> _friendlyCharacterStates = new List<ValueTuple<int, bool, int>>();

		// Token: 0x02000984 RID: 2436
		public static class PredefinedId
		{
			// Token: 0x060084BB RID: 33979 RVA: 0x004E4C50 File Offset: 0x004E2E50
			public static string GetName(sbyte playerId)
			{
				bool flag = playerId == 0;
				string result;
				if (flag)
				{
					result = "Self";
				}
				else
				{
					bool flag2 = playerId == 1;
					if (flag2)
					{
						result = "Adversary";
					}
					else
					{
						result = "Unknown";
					}
				}
				return result;
			}

			// Token: 0x060084BC RID: 33980 RVA: 0x004E4C88 File Offset: 0x004E2E88
			public static sbyte GetTheOtherSide(sbyte playerId)
			{
				bool flag = playerId == 0;
				sbyte result;
				if (flag)
				{
					result = 1;
				}
				else
				{
					bool flag2 = playerId == 1;
					if (flag2)
					{
						result = 0;
					}
					else
					{
						result = -1;
					}
				}
				return result;
			}

			// Token: 0x04002811 RID: 10257
			public const sbyte Invalid = -1;

			// Token: 0x04002812 RID: 10258
			public const sbyte Self = 0;

			// Token: 0x04002813 RID: 10259
			public const sbyte Adversary = 1;

			// Token: 0x04002814 RID: 10260
			public const sbyte Count = 2;
		}
	}
}
