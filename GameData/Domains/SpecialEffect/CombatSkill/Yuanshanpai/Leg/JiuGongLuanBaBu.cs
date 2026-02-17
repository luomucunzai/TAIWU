using System;
using System.Collections.Generic;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.GameDataBridge;

namespace GameData.Domains.SpecialEffect.CombatSkill.Yuanshanpai.Leg
{
	// Token: 0x020001FF RID: 511
	public class JiuGongLuanBaBu : CombatSkillEffectBase
	{
		// Token: 0x06002E85 RID: 11909 RVA: 0x0020F3DE File Offset: 0x0020D5DE
		public JiuGongLuanBaBu()
		{
		}

		// Token: 0x06002E86 RID: 11910 RVA: 0x0020F3F3 File Offset: 0x0020D5F3
		public JiuGongLuanBaBu(CombatSkillKey skillKey) : base(skillKey, 5103, -1)
		{
		}

		// Token: 0x06002E87 RID: 11911 RVA: 0x0020F410 File Offset: 0x0020D610
		public override void OnEnable(DataContext context)
		{
			DefeatMarkCollection markCollection = base.CombatChar.GetDefeatMarkCollection();
			this._oldMarks = new DefeatMarkCollection(markCollection);
			this._lastMarks = new DefeatMarkCollection(markCollection);
			this._newMarks = new DefeatMarkCollection();
			this._defeatMarkUid = new DataUid(8, 10, (ulong)((long)base.CharacterId), 50U);
			GameDataBridge.AddPostDataModificationHandler(this._defeatMarkUid, base.DataHandlerKey, new Action<DataContext, DataUid>(this.OnDefeatMarkChanged));
			DomainManager.Combat.AddMoveDistInSkillPrepare(base.CombatChar, 30, base.IsDirect);
			Events.RegisterHandler_DistanceChanged(new Events.OnDistanceChanged(this.OnDistanceChanged));
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06002E88 RID: 11912 RVA: 0x0020F4BF File Offset: 0x0020D6BF
		public override void OnDisable(DataContext context)
		{
			GameDataBridge.RemovePostDataModificationHandler(this._defeatMarkUid, base.DataHandlerKey);
			Events.UnRegisterHandler_DistanceChanged(new Events.OnDistanceChanged(this.OnDistanceChanged));
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06002E89 RID: 11913 RVA: 0x0020F4F8 File Offset: 0x0020D6F8
		private void OnDefeatMarkChanged(DataContext context, DataUid dataUid)
		{
			DefeatMarkCollection markCollection = base.CombatChar.GetDefeatMarkCollection();
			for (sbyte part = 0; part < 7; part += 1)
			{
				int flawCount = markCollection.FlawMarkList[(int)part].Count;
				int lastFlawCount = this._lastMarks.FlawMarkList[(int)part].Count;
				int acupointCount = markCollection.AcupointMarkList[(int)part].Count;
				int lastAcupointCount = this._lastMarks.AcupointMarkList[(int)part].Count;
				bool flag = flawCount > lastFlawCount;
				if (flag)
				{
					for (int i = 0; i < flawCount - lastFlawCount; i++)
					{
						this._newMarks.FlawMarkList[(int)part].Add(0);
					}
				}
				else
				{
					bool flag2 = flawCount < lastFlawCount;
					if (flag2)
					{
						for (int j = 0; j < lastFlawCount - flawCount; j++)
						{
							bool flag3 = this._oldMarks.FlawMarkList[(int)part].Count > 0;
							if (flag3)
							{
								this._oldMarks.FlawMarkList[(int)part].RemoveAt(0);
							}
						}
						while (this._newMarks.FlawMarkList[(int)part].Count > flawCount)
						{
							this._newMarks.FlawMarkList[(int)part].RemoveAt(0);
						}
					}
				}
				this._lastMarks.FlawMarkList[(int)part].Clear();
				this._lastMarks.FlawMarkList[(int)part].AddRange(markCollection.FlawMarkList[(int)part]);
				bool flag4 = acupointCount > lastAcupointCount;
				if (flag4)
				{
					for (int k = 0; k < acupointCount - lastAcupointCount; k++)
					{
						this._newMarks.AcupointMarkList[(int)part].Add(0);
					}
				}
				else
				{
					bool flag5 = acupointCount < lastAcupointCount;
					if (flag5)
					{
						for (int l = 0; l < lastAcupointCount - acupointCount; l++)
						{
							bool flag6 = this._oldMarks.AcupointMarkList[(int)part].Count > 0;
							if (flag6)
							{
								this._oldMarks.AcupointMarkList[(int)part].RemoveAt(0);
							}
						}
						while (this._newMarks.AcupointMarkList[(int)part].Count > acupointCount)
						{
							this._newMarks.AcupointMarkList[(int)part].RemoveAt(0);
						}
					}
				}
				this._lastMarks.AcupointMarkList[(int)part].Clear();
				this._lastMarks.AcupointMarkList[(int)part].AddRange(markCollection.AcupointMarkList[(int)part]);
			}
			int mindCount = markCollection.MindMarkList.Count;
			int lastMindCount = this._lastMarks.MindMarkList.Count;
			bool flag7 = mindCount > lastMindCount;
			if (flag7)
			{
				for (int m = lastMindCount; m < mindCount; m++)
				{
					this._newMarks.MindMarkList.Add(markCollection.MindMarkList[m]);
				}
			}
			else
			{
				bool flag8 = mindCount < lastMindCount;
				if (flag8)
				{
					int n = 0;
					while (n < lastMindCount - mindCount && this._oldMarks.MindMarkList.Count > 0)
					{
						this._oldMarks.MindMarkList.RemoveAt(0);
						n++;
					}
					while (this._newMarks.MindMarkList.Count > mindCount)
					{
						this._newMarks.MindMarkList.RemoveAt(0);
					}
				}
			}
			this._lastMarks.MindMarkList.Clear();
			this._lastMarks.MindMarkList.AddRange(markCollection.MindMarkList);
		}

		// Token: 0x06002E8A RID: 11914 RVA: 0x0020F868 File Offset: 0x0020DA68
		private void OnDistanceChanged(DataContext context, CombatCharacter mover, short distance, bool isMove, bool isForced)
		{
			bool flag = mover.GetId() != base.CharacterId || !isMove || isForced;
			if (!flag)
			{
				bool flag2 = base.IsDirect ? (distance < 0) : (distance > 0);
				if (flag2)
				{
					this._distanceAccumulator += Math.Abs(distance);
				}
				while (this._distanceAccumulator >= 2)
				{
					this._distanceAccumulator -= 2;
					bool flag3 = this._newMarks.GetTotalFlawCount() + this._newMarks.GetTotalAcupointCount() + this._newMarks.MindMarkList.Count > 0;
					if (flag3)
					{
						CombatCharacter enemyChar = DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly, false);
						this._typeRandomPool.Clear();
						for (sbyte part = 0; part < 7; part += 1)
						{
							for (int i = 0; i < this._newMarks.FlawMarkList[(int)part].Count; i++)
							{
								this._typeRandomPool.Add(new ValueTuple<sbyte, sbyte>(0, part));
							}
							for (int j = 0; j < this._newMarks.AcupointMarkList[(int)part].Count; j++)
							{
								this._typeRandomPool.Add(new ValueTuple<sbyte, sbyte>(1, part));
							}
						}
						for (int k = 0; k < this._newMarks.MindMarkList.Count; k++)
						{
							this._typeRandomPool.Add(new ValueTuple<sbyte, sbyte>(2, -1));
						}
						ValueTuple<sbyte, sbyte> transferMark = this._typeRandomPool[context.Random.Next(0, this._typeRandomPool.Count)];
						bool flag4 = transferMark.Item1 == 0;
						if (flag4)
						{
							DomainManager.Combat.TransferFlaw(context, base.CombatChar, enemyChar, transferMark.Item2, context.Random.Next(this._oldMarks.FlawMarkList[(int)transferMark.Item2].Count, this._lastMarks.FlawMarkList[(int)transferMark.Item2].Count));
						}
						else
						{
							bool flag5 = transferMark.Item1 == 1;
							if (flag5)
							{
								DomainManager.Combat.TransferAcupoint(context, base.CombatChar, enemyChar, transferMark.Item2, context.Random.Next(this._oldMarks.AcupointMarkList[(int)transferMark.Item2].Count, this._lastMarks.AcupointMarkList[(int)transferMark.Item2].Count));
							}
							else
							{
								DomainManager.Combat.TransferMindDefeatMark(context, base.CombatChar, enemyChar, context.Random.Next(this._oldMarks.MindMarkList.Count, this._lastMarks.MindMarkList.Count));
							}
						}
						DomainManager.Combat.AddToCheckFallenSet(enemyChar.GetId());
						base.ShowSpecialEffectTips(0);
					}
				}
			}
		}

		// Token: 0x06002E8B RID: 11915 RVA: 0x0020FB64 File Offset: 0x0020DD64
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				base.RemoveSelf(context);
			}
		}

		// Token: 0x04000DD6 RID: 3542
		private const sbyte MoveDistInPrepare = 30;

		// Token: 0x04000DD7 RID: 3543
		private const int AffectDistanceUnit = 2;

		// Token: 0x04000DD8 RID: 3544
		private short _distanceAccumulator;

		// Token: 0x04000DD9 RID: 3545
		private DefeatMarkCollection _oldMarks;

		// Token: 0x04000DDA RID: 3546
		private DefeatMarkCollection _lastMarks;

		// Token: 0x04000DDB RID: 3547
		private DefeatMarkCollection _newMarks;

		// Token: 0x04000DDC RID: 3548
		private DataUid _defeatMarkUid;

		// Token: 0x04000DDD RID: 3549
		private readonly List<ValueTuple<sbyte, sbyte>> _typeRandomPool = new List<ValueTuple<sbyte, sbyte>>();
	}
}
