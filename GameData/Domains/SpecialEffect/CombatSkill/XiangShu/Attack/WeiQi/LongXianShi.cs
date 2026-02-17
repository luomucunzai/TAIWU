using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.GameDataBridge;

namespace GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Attack.WeiQi
{
	// Token: 0x020002DD RID: 733
	public class LongXianShi : CombatSkillEffectBase
	{
		// Token: 0x060032EB RID: 13035 RVA: 0x00221814 File Offset: 0x0021FA14
		public LongXianShi()
		{
		}

		// Token: 0x060032EC RID: 13036 RVA: 0x00221829 File Offset: 0x0021FA29
		public LongXianShi(CombatSkillKey skillKey) : base(skillKey, 17051, -1)
		{
		}

		// Token: 0x060032ED RID: 13037 RVA: 0x00221845 File Offset: 0x0021FA45
		public override void OnEnable(DataContext context)
		{
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
			Events.RegisterHandler_SkillEffectChange(new Events.OnSkillEffectChange(this.OnSkillEffectChange));
		}

		// Token: 0x060032EE RID: 13038 RVA: 0x0022186C File Offset: 0x0021FA6C
		public override void OnDisable(DataContext context)
		{
			bool isSrcSkillPerformed = this.IsSrcSkillPerformed;
			if (isSrcSkillPerformed)
			{
				GameDataBridge.RemovePostDataModificationHandler(this._defeatMarkUid, base.DataHandlerKey);
			}
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
			Events.UnRegisterHandler_SkillEffectChange(new Events.OnSkillEffectChange(this.OnSkillEffectChange));
		}

		// Token: 0x060032EF RID: 13039 RVA: 0x002218BC File Offset: 0x0021FABC
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				CombatCharacter enemyChar = DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly, false);
				DefeatMarkCollection markCollection = enemyChar.GetDefeatMarkCollection();
				this._markRandomPool.Clear();
				for (sbyte part = 0; part < 7; part += 1)
				{
					int flawCount = markCollection.FlawMarkList[(int)part].Count;
					int acupointCount = markCollection.AcupointMarkList[(int)part].Count;
					for (int i = 0; i < flawCount; i++)
					{
						this._markRandomPool.Add(new ValueTuple<sbyte, sbyte>(0, part));
					}
					for (int j = 0; j < acupointCount; j++)
					{
						this._markRandomPool.Add(new ValueTuple<sbyte, sbyte>(1, part));
					}
				}
				int mindCount = markCollection.MindMarkList.Count;
				for (int k = 0; k < mindCount; k++)
				{
					this._markRandomPool.Add(new ValueTuple<sbyte, sbyte>(2, -1));
				}
				bool flag2 = !this.IsSrcSkillPerformed;
				if (flag2)
				{
					bool flag3 = this._markRandomPool.Count > 0 && base.PowerMatchAffectRequire((int)power, 0);
					if (flag3)
					{
						while (this._markRandomPool.Count > 3)
						{
							this._markRandomPool.RemoveAt(context.Random.Next(this._markRandomPool.Count));
						}
						for (int l = 0; l < this._markRandomPool.Count; l++)
						{
							this.RemoveDefeatMark(context, enemyChar, this._markRandomPool[l], markCollection, true);
						}
						enemyChar.SetDefeatMarkCollection(markCollection, context);
						this.IsSrcSkillPerformed = true;
						short effectCount = (short)(this._markRandomPool.Count * 2);
						DomainManager.Combat.AddSkillEffect(context, base.CombatChar, new SkillEffectKey(base.SkillTemplateId, base.IsDirect), effectCount, effectCount, true);
						this._lastMarks = new DefeatMarkCollection(base.CombatChar.GetDefeatMarkCollection());
						this._defeatMarkUid = new DataUid(8, 10, (ulong)((long)base.CharacterId), 50U);
						GameDataBridge.AddPostDataModificationHandler(this._defeatMarkUid, base.DataHandlerKey, new Action<DataContext, DataUid>(this.OnDefeatMarkChanged));
					}
					else
					{
						base.RemoveSelf(context);
					}
				}
				else
				{
					bool flag4 = this._markRandomPool.Count > 0 && base.PowerMatchAffectRequire((int)power, 0);
					if (flag4)
					{
						base.RemoveSelf(context);
					}
				}
			}
		}

		// Token: 0x060032F0 RID: 13040 RVA: 0x00221B54 File Offset: 0x0021FD54
		private void OnDefeatMarkChanged(DataContext context, DataUid dataUid)
		{
			DefeatMarkCollection markCollection = base.CombatChar.GetDefeatMarkCollection();
			this._markRandomPool.Clear();
			for (sbyte part = 0; part < 7; part += 1)
			{
				int newFlawCount = markCollection.FlawMarkList[(int)part].Count - this._lastMarks.FlawMarkList[(int)part].Count;
				int newAcupointCount = markCollection.AcupointMarkList[(int)part].Count - this._lastMarks.AcupointMarkList[(int)part].Count;
				for (int i = 0; i < newFlawCount; i++)
				{
					this._markRandomPool.Add(new ValueTuple<sbyte, sbyte>(0, part));
				}
				for (int j = 0; j < newAcupointCount; j++)
				{
					this._markRandomPool.Add(new ValueTuple<sbyte, sbyte>(1, part));
				}
			}
			int newMindCount = markCollection.MindMarkList.Count - markCollection.MindMarkList.Count;
			for (int k = 0; k < newMindCount; k++)
			{
				this._markRandomPool.Add(new ValueTuple<sbyte, sbyte>(2, -1));
			}
			this._lastMarks = new DefeatMarkCollection(base.CombatChar.GetDefeatMarkCollection());
			bool flag = this._markRandomPool.Count <= 0;
			if (!flag)
			{
				while (this._markRandomPool.Count > base.EffectCount)
				{
					this._markRandomPool.RemoveAt(context.Random.Next(this._markRandomPool.Count));
				}
				for (int l = 0; l < this._markRandomPool.Count; l++)
				{
					this.RemoveDefeatMark(context, base.CombatChar, this._markRandomPool[l], markCollection, false);
				}
				base.CombatChar.SetDefeatMarkCollection(markCollection, context);
				base.ReduceEffectCount(this._markRandomPool.Count);
				base.ShowSpecialEffectTips(0);
			}
		}

		// Token: 0x060032F1 RID: 13041 RVA: 0x00221D40 File Offset: 0x0021FF40
		private void RemoveDefeatMark(DataContext context, CombatCharacter combatChar, [TupleElementNames(new string[]
		{
			"type",
			"part"
		})] ValueTuple<sbyte, sbyte> markInfo, DefeatMarkCollection markCollection, bool randomRemove)
		{
			bool flag = markInfo.Item1 == 0;
			if (flag)
			{
				int flawCount = markCollection.FlawMarkList[(int)markInfo.Item2].Count;
				int flawIndex = randomRemove ? context.Random.Next(flawCount) : (flawCount - 1);
				markCollection.FlawMarkList[(int)markInfo.Item2].RemoveAt(flawIndex);
				DomainManager.Combat.RemoveFlaw(context, combatChar, markInfo.Item2, flawIndex, false, false);
			}
			else
			{
				bool flag2 = markInfo.Item1 == 1;
				if (flag2)
				{
					int acupointCount = markCollection.AcupointMarkList[(int)markInfo.Item2].Count;
					int acupointIndex = randomRemove ? context.Random.Next(acupointCount) : (acupointCount - 1);
					markCollection.AcupointMarkList[(int)markInfo.Item2].RemoveAt(acupointIndex);
					DomainManager.Combat.RemoveAcupoint(context, combatChar, markInfo.Item2, acupointIndex, false, false);
				}
				else
				{
					bool flag3 = markInfo.Item1 == 2;
					if (flag3)
					{
						DomainManager.Combat.RemoveMindDefeatMark(context, combatChar, 1, randomRemove, context.Random.Next(markCollection.MindMarkList.Count));
					}
				}
			}
		}

		// Token: 0x060032F2 RID: 13042 RVA: 0x00221E5C File Offset: 0x0022005C
		private void OnSkillEffectChange(DataContext context, int charId, SkillEffectKey key, short oldCount, short newCount, bool removed)
		{
			bool flag = removed && this.IsSrcSkillPerformed && charId == base.CharacterId && key.SkillId == base.SkillTemplateId && key.IsDirect == base.IsDirect;
			if (flag)
			{
				base.RemoveSelf(context);
			}
		}

		// Token: 0x04000F0D RID: 3853
		private const sbyte MaxReduceMark = 3;

		// Token: 0x04000F0E RID: 3854
		private DataUid _defeatMarkUid;

		// Token: 0x04000F0F RID: 3855
		private DefeatMarkCollection _lastMarks;

		// Token: 0x04000F10 RID: 3856
		[TupleElementNames(new string[]
		{
			"type",
			"part"
		})]
		private readonly List<ValueTuple<sbyte, sbyte>> _markRandomPool = new List<ValueTuple<sbyte, sbyte>>();
	}
}
