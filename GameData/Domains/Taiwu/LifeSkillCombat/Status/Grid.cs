using System;
using System.Collections.Generic;
using System.Linq;
using Config;
using GameData.Domains.Taiwu.LifeSkillCombat.Operation;
using GameData.Domains.Taiwu.LifeSkillCombat.Snapshot;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Taiwu.LifeSkillCombat.Status
{
	// Token: 0x0200005A RID: 90
	public class Grid : ISerializableGameData
	{
		// Token: 0x1700007E RID: 126
		// (get) Token: 0x06001527 RID: 5415 RVA: 0x0014847E File Offset: 0x0014667E
		// (set) Token: 0x06001528 RID: 5416 RVA: 0x00148486 File Offset: 0x00146686
		public int Index { get; private set; }

		// Token: 0x06001529 RID: 5417 RVA: 0x0014848F File Offset: 0x0014668F
		public Grid()
		{
		}

		// Token: 0x1700007F RID: 127
		// (get) Token: 0x0600152A RID: 5418 RVA: 0x001484A4 File Offset: 0x001466A4
		public OperationGridBase LastHistoryOperation
		{
			get
			{
				int i = this._operations.Count - 1;
				if (i >= 0)
				{
					OperationGridBase operation = this._operations[i];
					bool isActive = operation.IsActive;
					if (!isActive)
					{
						return operation;
					}
					bool flag = i != this._operations.Count - 1;
					if (flag)
					{
						throw new Exception("Active operation must be last one.");
					}
				}
				return null;
			}
		}

		// Token: 0x0600152B RID: 5419 RVA: 0x00148518 File Offset: 0x00146718
		public IEnumerable<OperationGridBase> HistoryOperations()
		{
			int i = 0;
			int len = this._operations.Count;
			while (i < len)
			{
				OperationGridBase operation = this._operations[i];
				bool isActive = operation.IsActive;
				if (isActive)
				{
					bool flag = i != this._operations.Count - 1;
					if (flag)
					{
						throw new Exception("Active operation must be last one.");
					}
					break;
				}
				else
				{
					yield return operation;
					operation = null;
					int num = i;
					i = num + 1;
				}
			}
			yield break;
		}

		// Token: 0x17000080 RID: 128
		// (get) Token: 0x0600152C RID: 5420 RVA: 0x00148528 File Offset: 0x00146728
		public OperationGridBase ActiveOperation
		{
			get
			{
				bool flag = this._operations.Count < 1;
				OperationGridBase result;
				if (flag)
				{
					result = null;
				}
				else
				{
					OperationGridBase last = this._operations[this._operations.Count - 1];
					result = (last.IsActive ? last : null);
				}
				return result;
			}
		}

		// Token: 0x0600152D RID: 5421 RVA: 0x00148578 File Offset: 0x00146778
		public void GetAllOperations(List<OperationGridBase> receiver, bool withClear)
		{
			if (withClear)
			{
				receiver.Clear();
			}
			receiver.AddRange(this._operations);
		}

		// Token: 0x0600152E RID: 5422 RVA: 0x0014859F File Offset: 0x0014679F
		public bool IsSerializedSizeFixed()
		{
			return false;
		}

		// Token: 0x0600152F RID: 5423 RVA: 0x001485A4 File Offset: 0x001467A4
		public int GetSerializedSize()
		{
			int totalSize = 0;
			totalSize += 4;
			totalSize += 2;
			int i = 0;
			int len = this._operations.Count;
			while (i < len)
			{
				totalSize += OperationBase.GetSerializeSizeWithPolymorphism(this._operations[i]);
				i++;
			}
			return totalSize;
		}

		// Token: 0x06001530 RID: 5424 RVA: 0x001485F4 File Offset: 0x001467F4
		public unsafe int Serialize(byte* pData)
		{
			*(int*)pData = this.Index;
			byte* pCurrData = pData + 4;
			Tester.Assert(this._operations.Count < 65535, "");
			*(short*)pCurrData = (short)((ushort)this._operations.Count);
			pCurrData += 2;
			int i = 0;
			int len = this._operations.Count;
			while (i < len)
			{
				pCurrData += OperationBase.SerializeWithPolymorphism(this._operations[i], pCurrData);
				i++;
			}
			return (int)((long)(pCurrData - pData));
		}

		// Token: 0x06001531 RID: 5425 RVA: 0x00148684 File Offset: 0x00146884
		public unsafe int Deserialize(byte* pData)
		{
			this.Index = *(int*)pData;
			byte* pCurrData = pData + 4;
			ushort count = *(ushort*)pCurrData;
			pCurrData += 2;
			this._operations.Clear();
			for (int i = 0; i < (int)count; i++)
			{
				OperationBase operation = null;
				pCurrData += OperationBase.DeserializeWithPolymorphism(ref operation, pCurrData);
				OperationGridBase pOg = operation as OperationGridBase;
				bool flag = pOg != null;
				if (!flag)
				{
					throw new Exception("operation storage in Grid must be OperationGridBase");
				}
				this._operations.Add(pOg);
			}
			return (int)((long)(pCurrData - pData));
		}

		// Token: 0x06001532 RID: 5426 RVA: 0x00148716 File Offset: 0x00146916
		public Grid(int index)
		{
			this.Index = index;
		}

		// Token: 0x06001533 RID: 5427 RVA: 0x00148733 File Offset: 0x00146933
		public IReadOnlyList<OperationGridBase> GetAllOperations()
		{
			return this._operations;
		}

		// Token: 0x17000081 RID: 129
		// (get) Token: 0x06001534 RID: 5428 RVA: 0x0014873B File Offset: 0x0014693B
		public Coordinate2D<sbyte> Coordinate
		{
			get
			{
				return Grid.ToCenterAnchoredCoordinate(this.Index);
			}
		}

		// Token: 0x06001535 RID: 5429 RVA: 0x00148748 File Offset: 0x00146948
		public static int ToGridIndex(Coordinate2D<sbyte> coordinate)
		{
			return (int)((coordinate.Y + 3) * 7 + (coordinate.X + 3));
		}

		// Token: 0x06001536 RID: 5430 RVA: 0x00148770 File Offset: 0x00146970
		public static Coordinate2D<byte> ToCornerAnchoredCoordinate(int index)
		{
			return new Coordinate2D<byte>((byte)(index % 7), (byte)(index / 7));
		}

		// Token: 0x06001537 RID: 5431 RVA: 0x00148790 File Offset: 0x00146990
		public static Coordinate2D<sbyte> ToCenterAnchoredCoordinate(int index)
		{
			Coordinate2D<byte> target = Grid.ToCornerAnchoredCoordinate(index);
			return new Coordinate2D<sbyte>((sbyte)(target.X - 3), (sbyte)(target.Y - 3));
		}

		// Token: 0x06001538 RID: 5432 RVA: 0x001487C0 File Offset: 0x001469C0
		public static IEnumerable<Coordinate2D<sbyte>> OffsetIterate(Coordinate2D<sbyte> origin, sbyte straight, sbyte cross, Func<Coordinate2D<sbyte>, bool> breakCondition = null)
		{
			foreach (Coordinate2D<sbyte> offset in Grid.OffsetStraight)
			{
				int num;
				for (int i = 0; i < (int)straight; i = num + 1)
				{
					Coordinate2D<sbyte> coord = new Coordinate2D<sbyte>((sbyte)Math.Clamp((int)origin.X + (int)offset.X * (i + 1), -128, 127), (sbyte)Math.Clamp((int)origin.Y + (int)offset.Y * (i + 1), -128, 127));
					bool flag = coord.X >= -3 && coord.X <= 3 && coord.Y >= -3 && coord.Y <= 3;
					if (flag)
					{
						bool flag2 = breakCondition != null && breakCondition(coord);
						if (flag2)
						{
							break;
						}
						yield return coord;
					}
					coord = default(Coordinate2D<sbyte>);
					num = i;
				}
				offset = default(Coordinate2D<sbyte>);
			}
			Coordinate2D<sbyte>[] array = null;
			foreach (Coordinate2D<sbyte> offset2 in Grid.OffsetCross)
			{
				int num;
				for (int j = 0; j < (int)cross; j = num + 1)
				{
					Coordinate2D<sbyte> coord2 = new Coordinate2D<sbyte>((sbyte)Math.Clamp((int)origin.X + (int)offset2.X * (j + 1), -128, 127), (sbyte)Math.Clamp((int)origin.Y + (int)offset2.Y * (j + 1), -128, 127));
					bool flag3 = coord2.X >= -3 && coord2.X <= 3 && coord2.Y >= -3 && coord2.Y <= 3;
					if (flag3)
					{
						bool flag4 = breakCondition != null && breakCondition(coord2);
						if (flag4)
						{
							break;
						}
						yield return coord2;
					}
					coord2 = default(Coordinate2D<sbyte>);
					num = j;
				}
				offset2 = default(Coordinate2D<sbyte>);
			}
			Coordinate2D<sbyte>[] array2 = null;
			yield break;
		}

		// Token: 0x06001539 RID: 5433 RVA: 0x001487E5 File Offset: 0x001469E5
		public IEnumerable<OperationGridBase> TakeHistoryOperations()
		{
			int num;
			for (int i = this._operations.Count - 1; i >= 0; i = num - 1)
			{
				OperationGridBase operation = this._operations[i];
				bool isActive = operation.IsActive;
				if (isActive)
				{
					break;
				}
				this._operations.RemoveAt(i);
				yield return operation;
				operation = null;
				num = i;
			}
			yield break;
		}

		// Token: 0x0600153A RID: 5434 RVA: 0x001487F8 File Offset: 0x001469F8
		public OperationGridBase TakeActiveOperation()
		{
			bool flag = this._operations.Count < 1;
			OperationGridBase result;
			if (flag)
			{
				result = null;
			}
			else
			{
				int lastIndex = this._operations.Count - 1;
				OperationGridBase last = this._operations[lastIndex];
				bool isActive = last.IsActive;
				if (isActive)
				{
					this._operations.RemoveAt(lastIndex);
					result = last;
				}
				else
				{
					result = null;
				}
			}
			return result;
		}

		// Token: 0x0600153B RID: 5435 RVA: 0x0014885C File Offset: 0x00146A5C
		public void DropHistoryOperations()
		{
			foreach (OperationGridBase history in this.TakeHistoryOperations())
			{
			}
		}

		// Token: 0x0600153C RID: 5436 RVA: 0x001488A8 File Offset: 0x00146AA8
		public void SetActiveOperation(OperationGridBase operation, Match lifeSkillCombat, IList<StatusSnapshotDiff.GridTrapStateExtraDiff> gridTrapStateExtraDiffs)
		{
			bool flag = operation != null;
			if (flag)
			{
				Tester.Assert(operation.GridIndex == this.Index, "");
			}
			OperationGridBase activeOperation = this.ActiveOperation;
			if (activeOperation != null)
			{
				activeOperation.MakeNonActive();
			}
			this.TakeActiveOperation();
			bool flag2 = operation != null;
			if (flag2)
			{
				this._operations.Add(operation);
				operation.MakeActive();
			}
			OperationPointBase pOp;
			bool flag3;
			if (operation == null && this._operations.Count > 0)
			{
				List<OperationGridBase> operations = this._operations;
				pOp = (operations[operations.Count - 1] as OperationPointBase);
				flag3 = (pOp != null);
			}
			else
			{
				flag3 = false;
			}
			bool flag4 = flag3;
			if (flag4)
			{
				lifeSkillCombat.OnGridActiveFixed(pOp, gridTrapStateExtraDiffs);
			}
		}

		// Token: 0x0600153D RID: 5437 RVA: 0x00148950 File Offset: 0x00146B50
		public OperationPointBase GetThesis()
		{
			int i = this._operations.Count - 1;
			while (i >= 0)
			{
				OperationGridBase operation = this._operations[i];
				bool isActive = operation.IsActive;
				OperationPointBase result;
				if (isActive)
				{
					result = null;
				}
				else
				{
					OperationPointBase thesis = operation as OperationPointBase;
					bool flag = thesis != null;
					if (!flag)
					{
						i--;
						continue;
					}
					result = thesis;
				}
				return result;
			}
			return null;
		}

		// Token: 0x0600153E RID: 5438 RVA: 0x001489BC File Offset: 0x00146BBC
		public int GetThesisScore()
		{
			Coordinate2D<sbyte> coord = this.Coordinate;
			bool flag = Math.Abs(coord.X) == 3 || Math.Abs(coord.Y) == 3;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				bool flag2 = Math.Abs(coord.X) == 2 || Math.Abs(coord.Y) == 2;
				if (flag2)
				{
					result = 1;
				}
				else
				{
					bool flag3 = Math.Abs(coord.X) == 1 || Math.Abs(coord.Y) == 1;
					if (flag3)
					{
						result = 2;
					}
					else
					{
						result = 3;
					}
				}
			}
			return result;
		}

		// Token: 0x0600153F RID: 5439 RVA: 0x00148A50 File Offset: 0x00146C50
		public List<sbyte> ProcessOnGridActiveFixedRecycle()
		{
			Tester.Assert(this.ActiveOperation == null, "");
			List<sbyte> recycled = new List<sbyte>();
			bool flag = this._operations.Count >= 2;
			if (flag)
			{
				OperationPointBase op = this._operations[this._operations.Count - 2] as OperationPointBase;
				bool flag2 = op != null;
				if (flag2)
				{
					recycled.AddRange(from c in op.EffectiveEffectCardTemplateIds
					where LifeSkillCombatEffect.Instance[c].IsSaveCard
					select c);
				}
			}
			recycled.RemoveAll((sbyte c) => LifeSkillCombatEffect.Instance[c].SubEffect == ELifeSkillCombatEffectSubEffect.SelfExtraQuestionOnHouseThesisLowAndRecycleCardAndExchangeOperation);
			return recycled;
		}

		// Token: 0x04000352 RID: 850
		private readonly List<OperationGridBase> _operations = new List<OperationGridBase>();

		// Token: 0x04000353 RID: 851
		public static readonly Coordinate2D<sbyte>[] OffsetStraight = new Coordinate2D<sbyte>[]
		{
			new Coordinate2D<sbyte>(1, 0),
			new Coordinate2D<sbyte>(-1, 0),
			new Coordinate2D<sbyte>(0, 1),
			new Coordinate2D<sbyte>(0, -1)
		};

		// Token: 0x04000354 RID: 852
		public static readonly Coordinate2D<sbyte>[] OffsetCross = new Coordinate2D<sbyte>[]
		{
			new Coordinate2D<sbyte>(1, 1),
			new Coordinate2D<sbyte>(-1, -1),
			new Coordinate2D<sbyte>(1, -1),
			new Coordinate2D<sbyte>(-1, 1)
		};
	}
}
