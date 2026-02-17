using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.GameDataBridge;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Attack.RanChenZi
{
	// Token: 0x020002E9 RID: 745
	public class ErJianMoNvYi : CombatSkillEffectBase
	{
		// Token: 0x0600333B RID: 13115 RVA: 0x0022381D File Offset: 0x00221A1D
		public ErJianMoNvYi()
		{
		}

		// Token: 0x0600333C RID: 13116 RVA: 0x0022383D File Offset: 0x00221A3D
		public ErJianMoNvYi(CombatSkillKey skillKey) : base(skillKey, 17131, -1)
		{
		}

		// Token: 0x0600333D RID: 13117 RVA: 0x00223864 File Offset: 0x00221A64
		public override void OnEnable(DataContext context)
		{
			this._lastMarks = new DefeatMarkCollection(base.CombatChar.GetDefeatMarkCollection());
			this._defeatMarkUid = new DataUid(8, 10, (ulong)((long)base.CharacterId), 50U);
			GameDataBridge.AddPostDataModificationHandler(this._defeatMarkUid, base.DataHandlerKey, new Action<DataContext, DataUid>(this.OnDefeatMarkChanged));
			Events.RegisterHandler_PrepareSkillBegin(new Events.OnPrepareSkillBegin(this.OnPrepareSkillBegin));
			Events.RegisterHandler_SkillEffectChange(new Events.OnSkillEffectChange(this.OnSkillEffectChange));
		}

		// Token: 0x0600333E RID: 13118 RVA: 0x002238E1 File Offset: 0x00221AE1
		public override void OnDisable(DataContext context)
		{
			GameDataBridge.RemovePostDataModificationHandler(this._defeatMarkUid, base.DataHandlerKey);
			Events.UnRegisterHandler_PrepareSkillBegin(new Events.OnPrepareSkillBegin(this.OnPrepareSkillBegin));
			Events.UnRegisterHandler_SkillEffectChange(new Events.OnSkillEffectChange(this.OnSkillEffectChange));
		}

		// Token: 0x0600333F RID: 13119 RVA: 0x0022391C File Offset: 0x00221B1C
		private void OnPrepareSkillBegin(DataContext context, int charId, bool isAlly, short skillId)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				bool flag2 = !this.IsSrcSkillPerformed;
				if (flag2)
				{
					this.IsSrcSkillPerformed = true;
					base.AddMaxEffectCount(true);
					sbyte taskStatus = DomainManager.World.GetElement_XiangshuAvatarTaskStatuses(0).JuniorXiangshuTaskStatus;
					bool flag3 = taskStatus > 4;
					if (flag3)
					{
						bool goodEnding = taskStatus == 6;
						Injuries injuries = base.CurrEnemyChar.GetInjuries();
						bool flag4 = goodEnding;
						if (flag4)
						{
							Injuries oldInjuries = base.CurrEnemyChar.GetOldInjuries();
							List<ValueTuple<sbyte, bool>> injuryRandomPool = ObjectPool<List<ValueTuple<sbyte, bool>>>.Instance.Get();
							injuryRandomPool.Clear();
							for (sbyte part = 0; part < 7; part += 1)
							{
								for (int i = 0; i < (int)injuries.Get(part, false); i++)
								{
									injuryRandomPool.Add(new ValueTuple<sbyte, bool>(part, false));
								}
								for (int j = 0; j < (int)injuries.Get(part, true); j++)
								{
									injuryRandomPool.Add(new ValueTuple<sbyte, bool>(part, true));
								}
							}
							bool flag5 = injuryRandomPool.Count > 0;
							if (flag5)
							{
								ValueTuple<sbyte, bool> injuryInfo = injuryRandomPool[context.Random.Next(injuryRandomPool.Count)];
								DomainManager.Combat.RemoveInjury(context, base.CurrEnemyChar, injuryInfo.Item1, injuryInfo.Item2, 1, true, oldInjuries.Get(injuryInfo.Item1, injuryInfo.Item2) > 0);
							}
							ObjectPool<List<ValueTuple<sbyte, bool>>>.Instance.Return(injuryRandomPool);
						}
						else
						{
							bool outerFull = true;
							bool innerFull = true;
							for (sbyte part2 = 0; part2 < 7; part2 += 1)
							{
								bool flag6 = injuries.Get(part2, false) < 6;
								if (flag6)
								{
									outerFull = false;
								}
								bool flag7 = injuries.Get(part2, true) < 6;
								if (flag7)
								{
									innerFull = false;
								}
								bool flag8 = !outerFull && !innerFull;
								if (flag8)
								{
									break;
								}
							}
							bool canAddOuter = !base.CurrEnemyChar.GetOuterInjuryImmunity() && !outerFull;
							bool canAddInner = !base.CurrEnemyChar.GetInnerInjuryImmunity() && !innerFull;
							bool flag9 = canAddOuter || canAddInner;
							if (flag9)
							{
								bool addInner = canAddInner && (!canAddOuter || context.Random.CheckPercentProb(50));
								DomainManager.Combat.AddRandomInjury(context, base.CurrEnemyChar, addInner, 1, 1, true, -1);
							}
						}
						base.ShowSpecialEffectTips(goodEnding, 1, 2);
					}
				}
				else
				{
					base.RemoveSelf(context);
				}
			}
		}

		// Token: 0x06003340 RID: 13120 RVA: 0x00223BA4 File Offset: 0x00221DA4
		private void OnSkillEffectChange(DataContext context, int charId, SkillEffectKey key, short oldCount, short newCount, bool removed)
		{
			bool flag = removed && this.IsSrcSkillPerformed && charId == base.CharacterId && key.SkillId == base.SkillTemplateId && key.IsDirect == base.IsDirect;
			if (flag)
			{
				base.RemoveSelf(context);
			}
		}

		// Token: 0x06003341 RID: 13121 RVA: 0x00223BF4 File Offset: 0x00221DF4
		private void OnDefeatMarkChanged(DataContext context, DataUid dataUid)
		{
			DefeatMarkCollection markCollection = base.CombatChar.GetDefeatMarkCollection();
			int removeNotInjuryMarkCount = 0;
			int removeInjuryMarkCount = 0;
			this._injuryMarkRandomPool.Clear();
			this._notInjuryMarkRandomPool.Clear();
			for (sbyte part = 0; part < 7; part += 1)
			{
				byte outerInjury = markCollection.OuterInjuryMarkList[(int)part];
				byte innerInjury = markCollection.InnerInjuryMarkList[(int)part];
				int flawCount = markCollection.FlawMarkList[(int)part].Count;
				int acupointCount = markCollection.AcupointMarkList[(int)part].Count;
				byte lastOuterInjury = this._lastMarks.OuterInjuryMarkList[(int)part];
				byte lastInnerInjury = this._lastMarks.InnerInjuryMarkList[(int)part];
				int lastFlawCount = this._lastMarks.FlawMarkList[(int)part].Count;
				int lastAcupointCount = this._lastMarks.AcupointMarkList[(int)part].Count;
				bool flag = outerInjury > lastOuterInjury;
				if (flag)
				{
					removeNotInjuryMarkCount += (int)(outerInjury - lastOuterInjury);
				}
				bool flag2 = innerInjury > lastInnerInjury;
				if (flag2)
				{
					removeNotInjuryMarkCount += (int)(innerInjury - lastInnerInjury);
				}
				bool flag3 = flawCount > lastFlawCount;
				if (flag3)
				{
					removeInjuryMarkCount += flawCount - lastFlawCount;
				}
				bool flag4 = acupointCount > lastAcupointCount;
				if (flag4)
				{
					removeInjuryMarkCount += acupointCount - lastAcupointCount;
				}
				for (int i = 0; i < (int)outerInjury; i++)
				{
					this._injuryMarkRandomPool.Add(new ValueTuple<sbyte, bool>(part, false));
				}
				for (int j = 0; j < (int)innerInjury; j++)
				{
					this._injuryMarkRandomPool.Add(new ValueTuple<sbyte, bool>(part, true));
				}
				for (int k = 0; k < flawCount; k++)
				{
					this._notInjuryMarkRandomPool.Add(new ValueTuple<sbyte, sbyte>(0, part));
				}
				for (int l = 0; l < acupointCount; l++)
				{
					this._notInjuryMarkRandomPool.Add(new ValueTuple<sbyte, sbyte>(1, part));
				}
			}
			int mindCount = markCollection.MindMarkList.Count;
			int lastMindCount = this._lastMarks.MindMarkList.Count;
			bool flag5 = mindCount > lastMindCount;
			if (flag5)
			{
				removeInjuryMarkCount += mindCount - lastMindCount;
			}
			for (int m = 0; m < mindCount; m++)
			{
				this._notInjuryMarkRandomPool.Add(new ValueTuple<sbyte, sbyte>(2, -1));
			}
			removeNotInjuryMarkCount = Math.Min(removeNotInjuryMarkCount, this._notInjuryMarkRandomPool.Count);
			removeInjuryMarkCount = Math.Min(removeInjuryMarkCount, this._injuryMarkRandomPool.Count);
			int removeCount = Math.Min(removeNotInjuryMarkCount + removeInjuryMarkCount, base.EffectCount);
			bool flag6 = removeCount <= 0;
			if (!flag6)
			{
				while (removeNotInjuryMarkCount + removeInjuryMarkCount > removeCount)
				{
					bool flag7 = removeNotInjuryMarkCount > 0 && (removeInjuryMarkCount == 0 || context.Random.CheckPercentProb(50));
					if (flag7)
					{
						removeNotInjuryMarkCount--;
					}
					else
					{
						removeInjuryMarkCount--;
					}
				}
				for (int n = 0; n < removeInjuryMarkCount; n++)
				{
					int index = context.Random.Next(this._injuryMarkRandomPool.Count);
					ValueTuple<sbyte, bool> injuryInfo = this._injuryMarkRandomPool[index];
					DomainManager.Combat.RemoveInjury(context, base.CombatChar, injuryInfo.Item1, injuryInfo.Item2, 1, false, false);
					this._injuryMarkRandomPool.RemoveAt(index);
				}
				for (int i2 = 0; i2 < removeNotInjuryMarkCount; i2++)
				{
					int index2 = context.Random.Next(this._notInjuryMarkRandomPool.Count);
					ValueTuple<sbyte, sbyte> markInfo = this._notInjuryMarkRandomPool[index2];
					bool flag8 = markInfo.Item1 == 0;
					if (flag8)
					{
						int flawIndex = context.Random.Next(markCollection.FlawMarkList[(int)markInfo.Item2].Count);
						markCollection.FlawMarkList[(int)markInfo.Item2].RemoveAt(flawIndex);
						DomainManager.Combat.RemoveFlaw(context, base.CombatChar, markInfo.Item2, flawIndex, false, false);
					}
					else
					{
						bool flag9 = markInfo.Item1 == 1;
						if (flag9)
						{
							int acupointIndex = context.Random.Next(markCollection.AcupointMarkList[(int)markInfo.Item2].Count);
							markCollection.AcupointMarkList[(int)markInfo.Item2].RemoveAt(acupointIndex);
							DomainManager.Combat.RemoveAcupoint(context, base.CombatChar, markInfo.Item2, acupointIndex, false, false);
						}
						else
						{
							bool flag10 = markInfo.Item1 == 2;
							if (flag10)
							{
								DomainManager.Combat.RemoveMindDefeatMark(context, base.CombatChar, 1, true, 0);
							}
						}
					}
					this._notInjuryMarkRandomPool.RemoveAt(index2);
				}
				this._lastMarks = new DefeatMarkCollection(base.CombatChar.GetDefeatMarkCollection());
				base.ReduceEffectCount(removeCount);
				DomainManager.Combat.UpdateBodyDefeatMark(context, base.CombatChar);
				base.ShowSpecialEffectTips(0);
			}
		}

		// Token: 0x04000F22 RID: 3874
		private DataUid _defeatMarkUid;

		// Token: 0x04000F23 RID: 3875
		private DefeatMarkCollection _lastMarks;

		// Token: 0x04000F24 RID: 3876
		[TupleElementNames(new string[]
		{
			"part",
			"inner"
		})]
		private readonly List<ValueTuple<sbyte, bool>> _injuryMarkRandomPool = new List<ValueTuple<sbyte, bool>>();

		// Token: 0x04000F25 RID: 3877
		[TupleElementNames(new string[]
		{
			"type",
			"part"
		})]
		private readonly List<ValueTuple<sbyte, sbyte>> _notInjuryMarkRandomPool = new List<ValueTuple<sbyte, sbyte>>();
	}
}
