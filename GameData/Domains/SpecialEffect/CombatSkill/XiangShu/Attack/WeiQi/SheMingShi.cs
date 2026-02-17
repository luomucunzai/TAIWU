using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.GameDataBridge;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Attack.WeiQi
{
	// Token: 0x020002DF RID: 735
	public class SheMingShi : CombatSkillEffectBase
	{
		// Token: 0x060032FB RID: 13051 RVA: 0x00222146 File Offset: 0x00220346
		public SheMingShi()
		{
		}

		// Token: 0x060032FC RID: 13052 RVA: 0x0022215B File Offset: 0x0022035B
		public SheMingShi(CombatSkillKey skillKey) : base(skillKey, 17054, -1)
		{
		}

		// Token: 0x060032FD RID: 13053 RVA: 0x00222177 File Offset: 0x00220377
		public override void OnEnable(DataContext context)
		{
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
			Events.RegisterHandler_SkillEffectChange(new Events.OnSkillEffectChange(this.OnSkillEffectChange));
		}

		// Token: 0x060032FE RID: 13054 RVA: 0x002221A0 File Offset: 0x002203A0
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
			Events.UnRegisterHandler_SkillEffectChange(new Events.OnSkillEffectChange(this.OnSkillEffectChange));
			foreach (DataUid dataUid in this._defeatMarkUids)
			{
				GameDataBridge.RemovePostDataModificationHandler(dataUid, base.DataHandlerKey);
			}
			this._defeatMarkUids.Clear();
		}

		// Token: 0x060032FF RID: 13055 RVA: 0x0022222C File Offset: 0x0022042C
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				List<ValueTuple<sbyte, sbyte>> markRandomPool = new List<ValueTuple<sbyte, sbyte>>();
				DefeatMarkCollection markCollection = base.CombatChar.GetDefeatMarkCollection();
				markRandomPool.Clear();
				for (sbyte part = 0; part < 7; part += 1)
				{
					int flawCount = markCollection.FlawMarkList[(int)part].Count;
					int acupointCount = markCollection.AcupointMarkList[(int)part].Count;
					for (int i = 0; i < flawCount; i++)
					{
						markRandomPool.Add(new ValueTuple<sbyte, sbyte>(0, part));
					}
					for (int j = 0; j < acupointCount; j++)
					{
						markRandomPool.Add(new ValueTuple<sbyte, sbyte>(1, part));
					}
				}
				int mindCount = markCollection.MindMarkList.Count;
				for (int k = 0; k < mindCount; k++)
				{
					markRandomPool.Add(new ValueTuple<sbyte, sbyte>(2, -1));
				}
				bool flag2 = !this.IsSrcSkillPerformed;
				if (flag2)
				{
					bool flag3 = markRandomPool.Count > 0 && !interrupted;
					if (flag3)
					{
						while (markRandomPool.Count > 3)
						{
							markRandomPool.RemoveAt(context.Random.Next(markRandomPool.Count));
						}
						for (int l = 0; l < markRandomPool.Count; l++)
						{
							ValueTuple<sbyte, sbyte> markInfo = markRandomPool[l];
							bool flag4 = markInfo.Item1 == 0;
							if (flag4)
							{
								int flawCount2 = markCollection.FlawMarkList[(int)markInfo.Item2].Count;
								int flawIndex = context.Random.Next(flawCount2);
								markCollection.FlawMarkList[(int)markInfo.Item2].RemoveAt(flawIndex);
								DomainManager.Combat.RemoveFlaw(context, base.CombatChar, markInfo.Item2, flawIndex, false, false);
							}
							else
							{
								bool flag5 = markInfo.Item1 == 1;
								if (flag5)
								{
									int acupointCount2 = markCollection.AcupointMarkList[(int)markInfo.Item2].Count;
									int acupointIndex = context.Random.Next(acupointCount2);
									markCollection.AcupointMarkList[(int)markInfo.Item2].RemoveAt(acupointIndex);
									DomainManager.Combat.RemoveAcupoint(context, base.CombatChar, markInfo.Item2, acupointIndex, false, false);
								}
								else
								{
									bool flag6 = markInfo.Item1 == 2;
									if (flag6)
									{
										DomainManager.Combat.RemoveMindDefeatMark(context, base.CombatChar, 1, false, 0);
									}
								}
							}
						}
						base.CombatChar.SetDefeatMarkCollection(markCollection, context);
						this.IsSrcSkillPerformed = true;
						short effectCount = (short)(markRandomPool.Count * 2);
						DomainManager.Combat.AddSkillEffect(context, base.CombatChar, new SkillEffectKey(base.SkillTemplateId, base.IsDirect), effectCount, effectCount, true);
						base.AppendAffectedAllEnemyData(context, 129, EDataModifyType.Add, -1);
						base.AppendAffectedAllEnemyData(context, 134, EDataModifyType.Add, -1);
						this._enemyLastMarkCount = new Dictionary<int, ValueTuple<int, int>>();
						foreach (int enemyId in DomainManager.Combat.GetCharacterList(!base.CombatChar.IsAlly))
						{
							bool flag7 = enemyId >= 0;
							if (flag7)
							{
								DataUid defeatMarkUid = new DataUid(8, 10, (ulong)((long)enemyId), 50U);
								DefeatMarkCollection enemyMarks = DomainManager.Combat.GetElement_CombatCharacterDict(enemyId).GetDefeatMarkCollection();
								Dictionary<int, ValueTuple<int, int>> enemyLastMarkCount = this._enemyLastMarkCount;
								int key = enemyId;
								List<bool> mindMarkList = enemyMarks.MindMarkList;
								int item = (mindMarkList != null) ? mindMarkList.Count : 0;
								List<CombatSkillKey> dieMarkList = enemyMarks.DieMarkList;
								enemyLastMarkCount[key] = new ValueTuple<int, int>(item, (dieMarkList != null) ? dieMarkList.Count : 0);
								GameDataBridge.AddPostDataModificationHandler(defeatMarkUid, base.DataHandlerKey, new Action<DataContext, DataUid>(this.OnDefeatMarkChanged));
								this._defeatMarkUids.Add(defeatMarkUid);
							}
						}
					}
					else
					{
						base.RemoveSelf(context);
					}
				}
				else
				{
					bool flag8 = markRandomPool.Count > 0 && !interrupted;
					if (flag8)
					{
						base.RemoveSelf(context);
					}
				}
			}
		}

		// Token: 0x06003300 RID: 13056 RVA: 0x00222628 File Offset: 0x00220828
		private void OnDefeatMarkChanged(DataContext context, DataUid dataUid)
		{
			int enemyId = (int)dataUid.SubId0;
			CombatCharacter enemyChar = DomainManager.Combat.GetElement_CombatCharacterDict(enemyId);
			DefeatMarkCollection markCollection = enemyChar.GetDefeatMarkCollection();
			List<bool> mindMarkList = markCollection.MindMarkList;
			bool hasNewMindMark = ((mindMarkList != null) ? mindMarkList.Count : 0) > this._enemyLastMarkCount[enemyId].Item1;
			bool flag = hasNewMindMark && (base.EffectCount > 1 || context.Random.CheckPercentProb(50));
			if (flag)
			{
				DomainManager.Combat.AppendMindDefeatMark(context, enemyChar, 1, -1, false);
				base.ReduceEffectCount(1);
			}
			bool flag2 = hasNewMindMark;
			if (flag2)
			{
				DomainManager.Combat.AddToCheckFallenSet(enemyChar.GetId());
				base.ShowSpecialEffectTips(0);
			}
			Dictionary<int, ValueTuple<int, int>> enemyLastMarkCount = this._enemyLastMarkCount;
			int key = enemyId;
			List<bool> mindMarkList2 = markCollection.MindMarkList;
			int item = (mindMarkList2 != null) ? mindMarkList2.Count : 0;
			List<CombatSkillKey> dieMarkList = markCollection.DieMarkList;
			enemyLastMarkCount[key] = new ValueTuple<int, int>(item, (dieMarkList != null) ? dieMarkList.Count : 0);
		}

		// Token: 0x06003301 RID: 13057 RVA: 0x00222710 File Offset: 0x00220910
		private void OnSkillEffectChange(DataContext context, int charId, SkillEffectKey key, short oldCount, short newCount, bool removed)
		{
			bool flag = removed && this.IsSrcSkillPerformed && charId == base.CharacterId && key.SkillId == base.SkillTemplateId && key.IsDirect == base.IsDirect;
			if (flag)
			{
				base.RemoveSelf(context);
			}
		}

		// Token: 0x06003302 RID: 13058 RVA: 0x00222760 File Offset: 0x00220960
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.FieldId == 134 || dataKey.FieldId == 129;
			int result;
			if (flag)
			{
				base.ReduceEffectCount(1);
				base.ShowSpecialEffectTips(0);
				result = 1;
			}
			else
			{
				result = 0;
			}
			return result;
		}

		// Token: 0x04000F13 RID: 3859
		private const sbyte MaxReduceMark = 3;

		// Token: 0x04000F14 RID: 3860
		[TupleElementNames(new string[]
		{
			"mind",
			"die"
		})]
		private Dictionary<int, ValueTuple<int, int>> _enemyLastMarkCount;

		// Token: 0x04000F15 RID: 3861
		private readonly List<DataUid> _defeatMarkUids = new List<DataUid>();
	}
}
