using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using GameData.Common;
using GameData.Domains.Character;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Assist;
using GameData.Utilities;
using Redzen.Random;

namespace GameData.Domains.SpecialEffect.CombatSkill.Kongsangpai.DefenseAndAssist
{
	// Token: 0x02000499 RID: 1177
	public class JiuSeYuChanFa : AssistSkillBase
	{
		// Token: 0x1700022C RID: 556
		// (get) Token: 0x06003C3B RID: 15419 RVA: 0x0024C6BB File Offset: 0x0024A8BB
		private int RemoveMarkCount
		{
			get
			{
				return base.IsDirect ? 9 : int.MaxValue;
			}
		}

		// Token: 0x1700022D RID: 557
		// (get) Token: 0x06003C3C RID: 15420 RVA: 0x0024C6CE File Offset: 0x0024A8CE
		private int CostNeiliAllocation
		{
			get
			{
				return base.IsDirect ? 9 : (9 * Math.Min(this._affectCount + 1, 11));
			}
		}

		// Token: 0x06003C3D RID: 15421 RVA: 0x0024C6EE File Offset: 0x0024A8EE
		public JiuSeYuChanFa()
		{
		}

		// Token: 0x06003C3E RID: 15422 RVA: 0x0024C6FF File Offset: 0x0024A8FF
		public JiuSeYuChanFa(CombatSkillKey skillKey) : base(skillKey, 10708)
		{
		}

		// Token: 0x06003C3F RID: 15423 RVA: 0x0024C716 File Offset: 0x0024A916
		public override void OnEnable(DataContext context)
		{
			CombatDomain.RegisterHandler_CombatCharAboutToFall(new CombatDomain.OnCombatCharAboutToFall(this.OnCharAboutToFall));
		}

		// Token: 0x06003C40 RID: 15424 RVA: 0x0024C72B File Offset: 0x0024A92B
		public override void OnDisable(DataContext context)
		{
			CombatDomain.UnRegisterHandler_CombatCharAboutToFall(new CombatDomain.OnCombatCharAboutToFall(this.OnCharAboutToFall));
		}

		// Token: 0x06003C41 RID: 15425 RVA: 0x0024C740 File Offset: 0x0024A940
		private unsafe void OnCharAboutToFall(DataContext context, CombatCharacter combatChar, int eventIndex)
		{
			bool flag = !base.CanAffect || combatChar != base.CombatChar || eventIndex != 1 || !DomainManager.Combat.DefeatMarkReachFailCount(base.CombatChar);
			if (!flag)
			{
				bool flag2 = (int)(*(ref base.CombatChar.GetNeiliAllocation().Items.FixedElementField + (IntPtr)3 * 2)) < this.CostNeiliAllocation;
				if (!flag2)
				{
					this.GenerateMarkRandomPool();
					bool isDirect = base.IsDirect;
					if (isDirect)
					{
						this.RemoveByMarkRandomPool(context.Random);
					}
					else
					{
						this.RemoveByTempRandomPool(context.Random, new Func<ValueTuple<JiuSeYuChanFa.EMarkType, sbyte>, bool>(JiuSeYuChanFa.IsInjuryMark));
						this.RemoveByTempRandomPool(context.Random, new Func<ValueTuple<JiuSeYuChanFa.EMarkType, sbyte>, bool>(JiuSeYuChanFa.IsNotInjuryMark));
						this.RemoveByTempRandomPool(context.Random, new Func<ValueTuple<JiuSeYuChanFa.EMarkType, sbyte>, bool>(JiuSeYuChanFa.IsFatalMark));
					}
					this.SetAllChangedFields(context);
					base.CombatChar.ChangeNeiliAllocation(context, 3, -this.CostNeiliAllocation, false, true);
					this._affectCount++;
					base.ShowSpecialEffectTips(0);
					base.ShowEffectTips(context);
				}
			}
		}

		// Token: 0x06003C42 RID: 15426 RVA: 0x0024C860 File Offset: 0x0024AA60
		private void GenerateMarkRandomPool()
		{
			if (this._markTypeRandomPool == null)
			{
				this._markTypeRandomPool = new List<ValueTuple<JiuSeYuChanFa.EMarkType, sbyte>>();
			}
			this._markTypeRandomPool.Clear();
			this._markTypeChanged.Reset();
			DefeatMarkCollection markCollection = base.CombatChar.GetDefeatMarkCollection();
			byte[] flawCounts = base.CombatChar.GetFlawCount();
			byte[] acupointCounts = base.CombatChar.GetAcupointCount();
			MindMarkList mindMarks = base.CombatChar.GetMindMarkTime();
			Injuries newInjuries = base.CombatChar.GetInjuries().Subtract(base.CombatChar.GetOldInjuries());
			for (sbyte bodyPart = 0; bodyPart < 7; bodyPart += 1)
			{
				for (int i = 0; i < (int)flawCounts[(int)bodyPart]; i++)
				{
					this._markTypeRandomPool.Add(new ValueTuple<JiuSeYuChanFa.EMarkType, sbyte>(JiuSeYuChanFa.EMarkType.Flaw, bodyPart));
				}
				for (int j = 0; j < (int)acupointCounts[(int)bodyPart]; j++)
				{
					this._markTypeRandomPool.Add(new ValueTuple<JiuSeYuChanFa.EMarkType, sbyte>(JiuSeYuChanFa.EMarkType.Acupoint, bodyPart));
				}
			}
			bool flag = mindMarks.MarkList != null;
			if (flag)
			{
				for (int k = 0; k < mindMarks.MarkList.Count; k++)
				{
					this._markTypeRandomPool.Add(new ValueTuple<JiuSeYuChanFa.EMarkType, sbyte>(JiuSeYuChanFa.EMarkType.Mind, -1));
				}
			}
			for (int l = 0; l < markCollection.FatalDamageMarkCount; l++)
			{
				this._markTypeRandomPool.Add(new ValueTuple<JiuSeYuChanFa.EMarkType, sbyte>(JiuSeYuChanFa.EMarkType.Fatal, -1));
			}
			bool flag2 = newInjuries.HasAnyInjury();
			if (flag2)
			{
				for (sbyte bodyPart2 = 0; bodyPart2 < 7; bodyPart2 += 1)
				{
					ValueTuple<sbyte, sbyte> injury = newInjuries.Get(bodyPart2);
					for (int m = 0; m < (int)injury.Item1; m++)
					{
						this._markTypeRandomPool.Add(new ValueTuple<JiuSeYuChanFa.EMarkType, sbyte>(JiuSeYuChanFa.EMarkType.OuterInjury, bodyPart2));
					}
					for (int n = 0; n < (int)injury.Item2; n++)
					{
						this._markTypeRandomPool.Add(new ValueTuple<JiuSeYuChanFa.EMarkType, sbyte>(JiuSeYuChanFa.EMarkType.InnerInjury, bodyPart2));
					}
				}
			}
		}

		// Token: 0x06003C43 RID: 15427 RVA: 0x0024CA64 File Offset: 0x0024AC64
		private static bool IsInjuryMark([TupleElementNames(new string[]
		{
			"markType",
			"bodyPart"
		})] ValueTuple<JiuSeYuChanFa.EMarkType, sbyte> tup)
		{
			JiuSeYuChanFa.EMarkType item = tup.Item1;
			return item - JiuSeYuChanFa.EMarkType.OuterInjury <= 1;
		}

		// Token: 0x06003C44 RID: 15428 RVA: 0x0024CA8C File Offset: 0x0024AC8C
		private static bool IsNotInjuryMark([TupleElementNames(new string[]
		{
			"markType",
			"bodyPart"
		})] ValueTuple<JiuSeYuChanFa.EMarkType, sbyte> tup)
		{
			JiuSeYuChanFa.EMarkType item = tup.Item1;
			return item <= JiuSeYuChanFa.EMarkType.Mind;
		}

		// Token: 0x06003C45 RID: 15429 RVA: 0x0024CAB4 File Offset: 0x0024ACB4
		private static bool IsFatalMark([TupleElementNames(new string[]
		{
			"markType",
			"bodyPart"
		})] ValueTuple<JiuSeYuChanFa.EMarkType, sbyte> tup)
		{
			return tup.Item1 == JiuSeYuChanFa.EMarkType.Fatal;
		}

		// Token: 0x06003C46 RID: 15430 RVA: 0x0024CAD0 File Offset: 0x0024ACD0
		private int RemoveByMarkRandomPool(IRandomSource random)
		{
			int removedCount = Math.Min(this.RemoveMarkCount, this._markTypeRandomPool.Count);
			for (int i = 0; i < removedCount; i++)
			{
				ValueTuple<JiuSeYuChanFa.EMarkType, sbyte> tuple = this._markTypeRandomPool.GetRandom(random);
				ValueTuple<JiuSeYuChanFa.EMarkType, sbyte> valueTuple = tuple;
				JiuSeYuChanFa.EMarkType type = valueTuple.Item1;
				sbyte bodyPart = valueTuple.Item2;
				this.ErasureDefeatMark(random, type, bodyPart);
				this._markTypeRandomPool.Remove(tuple);
			}
			return removedCount;
		}

		// Token: 0x06003C47 RID: 15431 RVA: 0x0024CB44 File Offset: 0x0024AD44
		private int RemoveByTempRandomPool(IRandomSource random, Func<ValueTuple<JiuSeYuChanFa.EMarkType, sbyte>, bool> predicate)
		{
			if (this._tempMarkTypeRandomPool == null)
			{
				this._tempMarkTypeRandomPool = new List<ValueTuple<JiuSeYuChanFa.EMarkType, sbyte>>();
			}
			this._tempMarkTypeRandomPool.Clear();
			this._tempMarkTypeRandomPool.AddRange(this._markTypeRandomPool.Where(predicate));
			int removedCount = Math.Min(this.RemoveMarkCount, this._tempMarkTypeRandomPool.Count);
			for (int i = 0; i < removedCount; i++)
			{
				ValueTuple<JiuSeYuChanFa.EMarkType, sbyte> tuple = this._tempMarkTypeRandomPool.GetRandom(random);
				ValueTuple<JiuSeYuChanFa.EMarkType, sbyte> valueTuple = tuple;
				JiuSeYuChanFa.EMarkType type = valueTuple.Item1;
				sbyte bodyPart = valueTuple.Item2;
				this.ErasureDefeatMark(random, type, bodyPart);
				this._tempMarkTypeRandomPool.Remove(tuple);
			}
			return removedCount;
		}

		// Token: 0x06003C48 RID: 15432 RVA: 0x0024CBF0 File Offset: 0x0024ADF0
		private void ErasureFlawOrAcupoint(sbyte bodyPart, bool isFlaw, IRandomSource random)
		{
			FlawOrAcupointCollection flawOrAcupointCollection = isFlaw ? base.CombatChar.GetFlawCollection() : base.CombatChar.GetAcupointCollection();
			byte[] flawOrAcupointCounts = isFlaw ? base.CombatChar.GetFlawCount() : base.CombatChar.GetAcupointCount();
			List<ValueTuple<sbyte, int, int>> flawOrAcupointList = flawOrAcupointCollection.BodyPartDict[bodyPart];
			int removeIndex = random.Next(0, flawOrAcupointList.Count);
			byte[] array = flawOrAcupointCounts;
			array[(int)bodyPart] = array[(int)bodyPart] - 1;
			flawOrAcupointList.RemoveAt(removeIndex);
			DefeatMarkCollection markCollection = base.CombatChar.GetDefeatMarkCollection();
			ByteList[] markList = isFlaw ? markCollection.FlawMarkList : markCollection.AcupointMarkList;
			markList[(int)bodyPart].RemoveAt(removeIndex);
		}

		// Token: 0x06003C49 RID: 15433 RVA: 0x0024CC94 File Offset: 0x0024AE94
		private void ErasureMind(IRandomSource random)
		{
			MindMarkList mindMarks = base.CombatChar.GetMindMarkTime();
			int mindMarkIndex = random.Next(0, mindMarks.MarkList.Count);
			mindMarks.MarkList.RemoveAt(mindMarkIndex);
			DefeatMarkCollection markCollection = base.CombatChar.GetDefeatMarkCollection();
			markCollection.MindMarkList.RemoveAt(mindMarkIndex);
		}

		// Token: 0x06003C4A RID: 15434 RVA: 0x0024CCE8 File Offset: 0x0024AEE8
		private void ErasureFatal()
		{
			DefeatMarkCollection markCollection = base.CombatChar.GetDefeatMarkCollection();
			markCollection.FatalDamageMarkCount--;
		}

		// Token: 0x06003C4B RID: 15435 RVA: 0x0024CD10 File Offset: 0x0024AF10
		private void ErasureInjury(sbyte bodyPart, bool isInner)
		{
			base.CombatChar.OfflineChangeInjuries(bodyPart, isInner, -1);
		}

		// Token: 0x06003C4C RID: 15436 RVA: 0x0024CD24 File Offset: 0x0024AF24
		private void ErasureDefeatMark(IRandomSource random, JiuSeYuChanFa.EMarkType type, sbyte bodyPart)
		{
			this._markTypeChanged[(int)type] = true;
			switch (type)
			{
			case JiuSeYuChanFa.EMarkType.Flaw:
			case JiuSeYuChanFa.EMarkType.Acupoint:
				this.ErasureFlawOrAcupoint(bodyPart, type == JiuSeYuChanFa.EMarkType.Flaw, random);
				break;
			case JiuSeYuChanFa.EMarkType.Mind:
				this.ErasureMind(random);
				break;
			case JiuSeYuChanFa.EMarkType.Fatal:
				this.ErasureFatal();
				break;
			case JiuSeYuChanFa.EMarkType.OuterInjury:
			case JiuSeYuChanFa.EMarkType.InnerInjury:
				this.ErasureInjury(bodyPart, type == JiuSeYuChanFa.EMarkType.InnerInjury);
				break;
			default:
				throw new ArgumentOutOfRangeException("type", type, null);
			}
		}

		// Token: 0x06003C4D RID: 15437 RVA: 0x0024CDA8 File Offset: 0x0024AFA8
		private void SetAllChangedFields(DataContext context)
		{
			DefeatMarkCollection markCollection = base.CombatChar.GetDefeatMarkCollection();
			byte[] flawCounts = base.CombatChar.GetFlawCount();
			FlawOrAcupointCollection flawCollection = base.CombatChar.GetFlawCollection();
			byte[] acupointCounts = base.CombatChar.GetAcupointCount();
			FlawOrAcupointCollection acupointCollection = base.CombatChar.GetAcupointCollection();
			MindMarkList mindMarks = base.CombatChar.GetMindMarkTime();
			Injuries injuries = base.CombatChar.GetInjuries();
			bool flag = this._markTypeChanged[0];
			if (flag)
			{
				base.CombatChar.SetFlawCount(flawCounts, context);
				base.CombatChar.SetFlawCollection(flawCollection, context);
			}
			bool flag2 = this._markTypeChanged[1];
			if (flag2)
			{
				base.CombatChar.SetAcupointCount(acupointCounts, context);
				base.CombatChar.SetAcupointCollection(acupointCollection, context);
			}
			bool flag3 = this._markTypeChanged[2];
			if (flag3)
			{
				base.CombatChar.SetMindMarkTime(mindMarks, context);
			}
			bool flag4 = this._markTypeChanged[4] || this._markTypeChanged[5];
			if (flag4)
			{
				DomainManager.Combat.SetInjuries(context, base.CombatChar, injuries, true, true);
			}
			bool flag5 = this._markTypeChanged.Any();
			if (flag5)
			{
				base.CombatChar.SetDefeatMarkCollection(markCollection, context);
			}
		}

		// Token: 0x040011B4 RID: 4532
		private const sbyte CostNeiliAllocationUnit = 9;

		// Token: 0x040011B5 RID: 4533
		private const int DirectRemoveMarkCount = 9;

		// Token: 0x040011B6 RID: 4534
		private const int ReverseRemoveMarkCount = 2147483647;

		// Token: 0x040011B7 RID: 4535
		private const int ReverseCostNeiliAllocationUnitMaxCount = 11;

		// Token: 0x040011B8 RID: 4536
		private List<ValueTuple<JiuSeYuChanFa.EMarkType, sbyte>> _markTypeRandomPool;

		// Token: 0x040011B9 RID: 4537
		private List<ValueTuple<JiuSeYuChanFa.EMarkType, sbyte>> _tempMarkTypeRandomPool;

		// Token: 0x040011BA RID: 4538
		private BoolArray8 _markTypeChanged;

		// Token: 0x040011BB RID: 4539
		private int _affectCount = 0;

		// Token: 0x02000A4B RID: 2635
		private enum EMarkType : sbyte
		{
			// Token: 0x04002A3D RID: 10813
			Flaw,
			// Token: 0x04002A3E RID: 10814
			Acupoint,
			// Token: 0x04002A3F RID: 10815
			Mind,
			// Token: 0x04002A40 RID: 10816
			Fatal,
			// Token: 0x04002A41 RID: 10817
			OuterInjury,
			// Token: 0x04002A42 RID: 10818
			InnerInjury
		}
	}
}
