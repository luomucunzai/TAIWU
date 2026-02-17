using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Config;
using GameData.Domains.Taiwu.LifeSkillCombat.Snapshot;
using GameData.Domains.Taiwu.LifeSkillCombat.Status;
using GameData.Serializer;

namespace GameData.Domains.Taiwu.LifeSkillCombat.Operation
{
	// Token: 0x02000065 RID: 101
	[SerializableGameData(NotForDisplayModule = true)]
	public abstract class OperationPointBase : OperationGridBase
	{
		// Token: 0x17000090 RID: 144
		// (get) Token: 0x060015AB RID: 5547 RVA: 0x0014B99B File Offset: 0x00149B9B
		// (set) Token: 0x060015AC RID: 5548 RVA: 0x0014B9A3 File Offset: 0x00149BA3
		public int BasePoint { get; internal set; }

		// Token: 0x17000091 RID: 145
		// (get) Token: 0x060015AD RID: 5549 RVA: 0x0014B9AC File Offset: 0x00149BAC
		public int Point
		{
			get
			{
				return Math.Max(this.RawPoint, 0);
			}
		}

		// Token: 0x17000092 RID: 146
		// (get) Token: 0x060015AE RID: 5550 RVA: 0x0014B9BA File Offset: 0x00149BBA
		public int RawPoint
		{
			get
			{
				return this.BasePoint + this.AdditionalPoint;
			}
		}

		// Token: 0x060015AF RID: 5551 RVA: 0x0014B9C9 File Offset: 0x00149BC9
		public OperationPointBase()
		{
		}

		// Token: 0x060015B0 RID: 5552 RVA: 0x0014B9F4 File Offset: 0x00149BF4
		public override string Inspect()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(11, 3);
			defaultInterpolatedStringHandler.AppendFormatted(base.Inspect());
			defaultInterpolatedStringHandler.AppendLiteral(" Point[");
			defaultInterpolatedStringHandler.AppendFormatted<int>(this.BasePoint);
			defaultInterpolatedStringHandler.AppendLiteral(" + ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(this.AdditionalPoint);
			defaultInterpolatedStringHandler.AppendLiteral("]");
			string result = defaultInterpolatedStringHandler.ToStringAndClear();
			string part = string.Empty;
			foreach (Book effectiveBookState in this._effectiveBookStates)
			{
				bool flag = !string.IsNullOrEmpty(part);
				if (flag)
				{
					part += ", ";
				}
				part += LifeSkill.Instance[effectiveBookState.LifeSkill.SkillTemplateId].Name;
			}
			result = result + " Books[" + part + "]";
			string part2 = string.Empty;
			foreach (sbyte effectiveEffectCardTemplateId in this._effectiveEffectCardTemplateIds)
			{
				bool flag2 = !string.IsNullOrEmpty(part2);
				if (flag2)
				{
					part2 += ", ";
				}
				part2 += LifeSkillCombatEffect.Instance[effectiveEffectCardTemplateId].Name;
			}
			result = result + " Cards[" + part2 + "]";
			return result;
		}

		// Token: 0x060015B1 RID: 5553 RVA: 0x0014BB9C File Offset: 0x00149D9C
		public void GetEffectiveEffectCardTemplateIds(List<sbyte> receiver, bool withClear = true)
		{
			if (withClear)
			{
				receiver.Clear();
			}
			receiver.AddRange(this._effectiveEffectCardTemplateIds);
		}

		// Token: 0x060015B2 RID: 5554 RVA: 0x0014BBC4 File Offset: 0x00149DC4
		public void GetEffectiveBookStates(List<Book> receiver, bool withClear = true)
		{
			if (withClear)
			{
				receiver.Clear();
			}
			receiver.AddRange(this._effectiveBookStates);
		}

		// Token: 0x060015B3 RID: 5555 RVA: 0x0014BBEC File Offset: 0x00149DEC
		public override int GetSerializedSize()
		{
			int totalSize = 0;
			totalSize += base.GetSerializedSize();
			totalSize += 4;
			totalSize += 4;
			totalSize += 2 + this._effectiveEffectCardTemplateIds.Count;
			totalSize += 2;
			foreach (Book element in this._effectiveBookStates)
			{
				totalSize += element.GetSerializedSize();
			}
			totalSize += 2 + 4 * this.UseFriendlyCharacterIds.Count;
			return totalSize;
		}

		// Token: 0x060015B4 RID: 5556 RVA: 0x0014BC84 File Offset: 0x00149E84
		public unsafe override int Serialize(byte* pData)
		{
			byte* pCurrData = pData + base.Serialize(pData);
			*(int*)pCurrData = this.BasePoint;
			pCurrData += 4;
			*(int*)pCurrData = this.AdditionalPoint;
			pCurrData += 4;
			*(short*)pCurrData = (short)((ushort)this._effectiveEffectCardTemplateIds.Count);
			pCurrData += 2;
			int i = 0;
			int len = this._effectiveEffectCardTemplateIds.Count;
			while (i < len)
			{
				*pCurrData = (byte)this._effectiveEffectCardTemplateIds[i];
				pCurrData++;
				i++;
			}
			*(short*)pCurrData = (short)((ushort)this._effectiveBookStates.Count);
			pCurrData += 2;
			int j = 0;
			int len2 = this._effectiveBookStates.Count;
			while (j < len2)
			{
				pCurrData += this._effectiveBookStates[j].Serialize(pCurrData);
				j++;
			}
			*(short*)pCurrData = (short)((ushort)this.UseFriendlyCharacterIds.Count);
			pCurrData += 2;
			int k = 0;
			int len3 = this.UseFriendlyCharacterIds.Count;
			while (k < len3)
			{
				*(int*)pCurrData = this.UseFriendlyCharacterIds[k];
				pCurrData += 4;
				k++;
			}
			return (int)((long)(pCurrData - pData));
		}

		// Token: 0x060015B5 RID: 5557 RVA: 0x0014BDA4 File Offset: 0x00149FA4
		public unsafe override int Deserialize(byte* pData)
		{
			byte* pCurrData = pData + base.Deserialize(pData);
			this.BasePoint = *(int*)pCurrData;
			pCurrData += 4;
			this.AdditionalPoint = *(int*)pCurrData;
			pCurrData += 4;
			ushort count = *(ushort*)pCurrData;
			pCurrData += 2;
			this._effectiveEffectCardTemplateIds.Clear();
			for (int i = 0; i < (int)count; i++)
			{
				this._effectiveEffectCardTemplateIds.Add(*(sbyte*)pCurrData);
				pCurrData++;
			}
			ushort count2 = *(ushort*)pCurrData;
			pCurrData += 2;
			this._effectiveBookStates.Clear();
			for (int j = 0; j < (int)count2; j++)
			{
				Book element = default(Book);
				pCurrData += element.Deserialize(pCurrData);
				this._effectiveBookStates.Add(element);
			}
			ushort count3 = *(ushort*)pCurrData;
			pCurrData += 2;
			this.UseFriendlyCharacterIds.Clear();
			for (int k = 0; k < (int)count3; k++)
			{
				this.UseFriendlyCharacterIds.Add(*(int*)pCurrData);
				pCurrData += 4;
			}
			return (int)((long)(pCurrData - pData));
		}

		// Token: 0x17000093 RID: 147
		// (get) Token: 0x060015B6 RID: 5558 RVA: 0x0014BEAE File Offset: 0x0014A0AE
		public IReadOnlyList<sbyte> EffectiveEffectCardTemplateIds
		{
			get
			{
				return this._effectiveEffectCardTemplateIds;
			}
		}

		// Token: 0x17000094 RID: 148
		// (get) Token: 0x060015B7 RID: 5559 RVA: 0x0014BEB6 File Offset: 0x0014A0B6
		public IReadOnlyList<Book> EffectiveBookStates
		{
			get
			{
				return this._effectiveBookStates;
			}
		}

		// Token: 0x060015B8 RID: 5560 RVA: 0x0014BEBE File Offset: 0x0014A0BE
		public void ChangeBasePoint(int delta)
		{
			this.BasePoint += delta;
		}

		// Token: 0x060015B9 RID: 5561 RVA: 0x0014BED0 File Offset: 0x0014A0D0
		public OperationPointBase CommitUsedBookStates(IEnumerable<Book> usedBookStates, [TupleElementNames(new string[]
		{
			"CharacterId",
			"Usable",
			"BasePoint"
		})] IEnumerable<ValueTuple<int, bool, int>> usedCharacterStates, bool withPoint = true)
		{
			bool flag = usedBookStates != null;
			if (flag)
			{
				foreach (Book bookState in usedBookStates)
				{
					this._effectiveBookStates.Add(bookState);
					if (withPoint)
					{
						this.BasePoint += bookState.BasePoint;
					}
				}
			}
			bool flag2 = usedCharacterStates != null;
			if (flag2)
			{
				foreach (ValueTuple<int, bool, int> characterState in usedCharacterStates)
				{
					this.UseFriendlyCharacterIds.Add(characterState.Item1);
					if (withPoint)
					{
						this.BasePoint += characterState.Item3;
					}
				}
			}
			return this;
		}

		// Token: 0x060015BA RID: 5562 RVA: 0x0014BFC0 File Offset: 0x0014A1C0
		public OperationPointBase ClearUsedBookStates(Player player, bool withPoint = true)
		{
			if (withPoint)
			{
				foreach (Book bookState in this.EffectiveBookStates)
				{
					this.BasePoint -= bookState.BasePoint;
				}
			}
			if (withPoint)
			{
				IEnumerable<int> useFriendlyCharacterIds = this.UseFriendlyCharacterIds;
				Func<int, ValueTuple<int, bool, int>> <>9__0;
				Func<int, ValueTuple<int, bool, int>> selector;
				if ((selector = <>9__0) == null)
				{
					selector = (<>9__0 = ((int id) => player.FriendlyCharacterStates.First(([TupleElementNames(new string[]
					{
						"CharacterId",
						"Usable",
						"BasePoint"
					})] ValueTuple<int, bool, int> p) => p.Item1 == id)));
				}
				foreach (ValueTuple<int, bool, int> characterState in useFriendlyCharacterIds.Select(selector))
				{
					this.BasePoint -= characterState.Item3;
				}
			}
			this._effectiveBookStates.Clear();
			this.UseFriendlyCharacterIds.Clear();
			return this;
		}

		// Token: 0x060015BB RID: 5563
		public abstract IEnumerable<sbyte> PickEffectiveEffectCards(IEnumerable<sbyte> wantUseEffectCardIds);

		// Token: 0x060015BC RID: 5564 RVA: 0x0014C0D4 File Offset: 0x0014A2D4
		internal void DropEffectiveEffectCards()
		{
			this._effectiveEffectCardTemplateIds.Clear();
		}

		// Token: 0x060015BD RID: 5565 RVA: 0x0014C0E2 File Offset: 0x0014A2E2
		internal void RegisterEffectiveEffectCards(sbyte effectCardTemplateId)
		{
			this._effectiveEffectCardTemplateIds.Add(effectCardTemplateId);
		}

		// Token: 0x060015BE RID: 5566 RVA: 0x0014C0F2 File Offset: 0x0014A2F2
		public void DropCard(sbyte cardTemplateId)
		{
			this._effectiveEffectCardTemplateIds.Remove(cardTemplateId);
		}

		// Token: 0x060015BF RID: 5567 RVA: 0x0014C104 File Offset: 0x0014A304
		public virtual void ProcessOnGridActiveFixed(Match lifeSkillCombat, IList<StatusSnapshotDiff.GridTrapStateExtraDiff> gridTrapStateExtraDiffs)
		{
			Grid grid = lifeSkillCombat.GetGrid(base.GridIndex);
			Player player = lifeSkillCombat.GetPlayer(base.PlayerId);
			bool isRecycle = false;
			foreach (sbyte effectCardId in this.EffectiveEffectCardTemplateIds)
			{
				ELifeSkillCombatEffectSubEffect subEffect = LifeSkillCombatEffect.Instance[effectCardId].SubEffect;
				ELifeSkillCombatEffectSubEffect elifeSkillCombatEffectSubEffect = subEffect;
				if (elifeSkillCombatEffectSubEffect == ELifeSkillCombatEffectSubEffect.SelfExtraQuestionOnHouseThesisLowAndRecycleCardAndExchangeOperation)
				{
					isRecycle = true;
				}
			}
			int i = this.EffectiveEffectCardTemplateIds.Count - 1;
			while (i >= 0)
			{
				sbyte e = this.EffectiveEffectCardTemplateIds[i];
				ELifeSkillCombatEffectSubEffect subEffect2 = LifeSkillCombatEffect.Instance[e].SubEffect;
				ELifeSkillCombatEffectSubEffect elifeSkillCombatEffectSubEffect2 = subEffect2;
				switch (elifeSkillCombatEffectSubEffect2)
				{
				case ELifeSkillCombatEffectSubEffect.SelfGridCoverBookStatesWhenAllQuestion:
				case ELifeSkillCombatEffectSubEffect.SelfDoPickByPoint:
					break;
				case ELifeSkillCombatEffectSubEffect.SelfTrapedInCell:
					this._effectiveEffectCardTemplateIds.RemoveAt(i);
					break;
				case ELifeSkillCombatEffectSubEffect.SelfChangeBookCd:
					break;
				default:
					switch (elifeSkillCombatEffectSubEffect2)
					{
					}
					break;
				}
				IL_FB:
				i--;
				continue;
				goto IL_FB;
			}
			bool flag = isRecycle;
			if (flag)
			{
				List<sbyte> recycled = grid.ProcessOnGridActiveFixedRecycle();
				foreach (sbyte card in recycled)
				{
					this._effectiveEffectCardTemplateIds.Remove(card);
				}
				player.RecruitEffectCards(lifeSkillCombat, recycled);
			}
		}

		// Token: 0x060015C0 RID: 5568 RVA: 0x0014C298 File Offset: 0x0014A498
		protected OperationPointBase(sbyte playerId, int stamp, int gridIndex, int basePoint, IEnumerable<sbyte> wantUseEffectCards) : base(playerId, stamp, gridIndex)
		{
			this.BasePoint = basePoint;
			this.AdditionalPoint = 0;
		}

		// Token: 0x04000384 RID: 900
		public int AdditionalPoint;

		// Token: 0x04000385 RID: 901
		private readonly List<sbyte> _effectiveEffectCardTemplateIds = new List<sbyte>();

		// Token: 0x04000386 RID: 902
		private readonly List<Book> _effectiveBookStates = new List<Book>();

		// Token: 0x04000387 RID: 903
		public readonly List<int> UseFriendlyCharacterIds = new List<int>();
	}
}
