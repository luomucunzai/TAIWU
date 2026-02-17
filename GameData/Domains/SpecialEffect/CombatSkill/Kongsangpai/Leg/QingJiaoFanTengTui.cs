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

namespace GameData.Domains.SpecialEffect.CombatSkill.Kongsangpai.Leg
{
	// Token: 0x0200048A RID: 1162
	public class QingJiaoFanTengTui : CombatSkillEffectBase
	{
		// Token: 0x06003BE4 RID: 15332 RVA: 0x0024A646 File Offset: 0x00248846
		public QingJiaoFanTengTui()
		{
		}

		// Token: 0x06003BE5 RID: 15333 RVA: 0x0024A671 File Offset: 0x00248871
		public QingJiaoFanTengTui(CombatSkillKey skillKey) : base(skillKey, 10305, -1)
		{
		}

		// Token: 0x06003BE6 RID: 15334 RVA: 0x0024A6A4 File Offset: 0x002488A4
		public override void OnEnable(DataContext context)
		{
			DomainManager.Combat.AddMoveDistInSkillPrepare(base.CombatChar, 30, base.IsDirect);
			this._lastInjuries = base.CombatChar.GetInjuries();
			this._lastOldInjuries = base.CombatChar.GetOldInjuries();
			Dictionary<short, ValueTuple<short, bool, int>> stateDict = base.CombatChar.GetDebuffCombatStateCollection().StateDict;
			this._lastDebuffState.Clear();
			foreach (KeyValuePair<short, ValueTuple<short, bool, int>> state in stateDict)
			{
				this._lastDebuffState.Add(state.Key, new ValueTuple<short, bool>(state.Value.Item1, state.Value.Item2));
			}
			this._injuryUid = new DataUid(8, 10, (ulong)((long)base.CharacterId), 29U);
			this._oldInjuryUid = new DataUid(8, 10, (ulong)((long)base.CharacterId), 30U);
			this._debuffStateUid = new DataUid(8, 10, (ulong)((long)base.CharacterId), 77U);
			GameDataBridge.AddPostDataModificationHandler(this._injuryUid, base.DataHandlerKey, new Action<DataContext, DataUid>(this.OnInjuryChanged));
			GameDataBridge.AddPostDataModificationHandler(this._oldInjuryUid, base.DataHandlerKey, new Action<DataContext, DataUid>(this.OnOldInjuryChanged));
			GameDataBridge.AddPostDataModificationHandler(this._debuffStateUid, base.DataHandlerKey, new Action<DataContext, DataUid>(this.OnDebuffStateChanged));
			Events.RegisterHandler_DistanceChanged(new Events.OnDistanceChanged(this.OnDistanceChanged));
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06003BE7 RID: 15335 RVA: 0x0024A83C File Offset: 0x00248A3C
		public override void OnDisable(DataContext context)
		{
			GameDataBridge.RemovePostDataModificationHandler(this._injuryUid, base.DataHandlerKey);
			GameDataBridge.RemovePostDataModificationHandler(this._oldInjuryUid, base.DataHandlerKey);
			GameDataBridge.RemovePostDataModificationHandler(this._debuffStateUid, base.DataHandlerKey);
			Events.UnRegisterHandler_DistanceChanged(new Events.OnDistanceChanged(this.OnDistanceChanged));
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06003BE8 RID: 15336 RVA: 0x0024A8A4 File Offset: 0x00248AA4
		private void OnInjuryChanged(DataContext context, DataUid dataUid)
		{
			Injuries injuries = base.CombatChar.GetInjuries();
			for (sbyte bodyPart = 0; bodyPart < 7; bodyPart += 1)
			{
				ValueTuple<sbyte, sbyte> currInjury = injuries.Get(bodyPart);
				ValueTuple<sbyte, sbyte> lastInjury = this._lastInjuries.Get(bodyPart);
				bool flag = currInjury.Item1 > lastInjury.Item1;
				if (flag)
				{
					this._addedInjuries.Change(bodyPart, false, (int)(currInjury.Item1 - lastInjury.Item1));
				}
				else
				{
					bool flag2 = this._addedInjuries.Get(bodyPart, false) > currInjury.Item1;
					if (flag2)
					{
						this._addedInjuries.Change(bodyPart, false, (int)(currInjury.Item1 - this._addedInjuries.Get(bodyPart, false)));
					}
				}
				bool flag3 = currInjury.Item1 > lastInjury.Item2;
				if (flag3)
				{
					this._addedInjuries.Change(bodyPart, true, (int)(currInjury.Item2 - lastInjury.Item2));
				}
				else
				{
					bool flag4 = this._addedInjuries.Get(bodyPart, true) > currInjury.Item2;
					if (flag4)
					{
						this._addedInjuries.Change(bodyPart, true, (int)(currInjury.Item2 - this._addedInjuries.Get(bodyPart, true)));
					}
				}
			}
			this._lastInjuries = injuries;
		}

		// Token: 0x06003BE9 RID: 15337 RVA: 0x0024A9D8 File Offset: 0x00248BD8
		private void OnOldInjuryChanged(DataContext context, DataUid dataUid)
		{
			Injuries oldInjuries = base.CombatChar.GetInjuries();
			for (sbyte bodyPart = 0; bodyPart < 7; bodyPart += 1)
			{
				ValueTuple<sbyte, sbyte> currInjury = oldInjuries.Get(bodyPart);
				ValueTuple<sbyte, sbyte> lastInjury = this._lastOldInjuries.Get(bodyPart);
				bool flag = currInjury.Item1 > lastInjury.Item1;
				if (flag)
				{
					this._addedInjuries.Change(bodyPart, false, (int)(currInjury.Item1 - lastInjury.Item1));
				}
				else
				{
					bool flag2 = this._addedInjuries.Get(bodyPart, false) > currInjury.Item1;
					if (flag2)
					{
						this._addedInjuries.Change(bodyPart, false, (int)(currInjury.Item1 - this._addedInjuries.Get(bodyPart, false)));
					}
				}
				bool flag3 = currInjury.Item1 > lastInjury.Item2;
				if (flag3)
				{
					this._addedInjuries.Change(bodyPart, true, (int)(currInjury.Item2 - lastInjury.Item2));
				}
				else
				{
					bool flag4 = this._addedInjuries.Get(bodyPart, true) > currInjury.Item2;
					if (flag4)
					{
						this._addedInjuries.Change(bodyPart, true, (int)(currInjury.Item2 - this._addedInjuries.Get(bodyPart, true)));
					}
				}
			}
			this._lastOldInjuries = oldInjuries;
		}

		// Token: 0x06003BEA RID: 15338 RVA: 0x0024AB0C File Offset: 0x00248D0C
		private void OnDebuffStateChanged(DataContext context, DataUid dataUid)
		{
			Dictionary<short, ValueTuple<short, bool, int>> stateDict = base.CombatChar.GetDebuffCombatStateCollection().StateDict;
			List<short> stateIdList = ObjectPool<List<short>>.Instance.Get();
			stateIdList.Clear();
			stateIdList.AddRange(stateDict.Keys);
			this._addedDebuffState.RemoveAll((short id) => !stateIdList.Contains(id));
			foreach (short stateId in stateIdList)
			{
				bool flag = !this._lastDebuffState.ContainsKey(stateId) || Math.Abs(this._lastDebuffState[stateId].Item1) < Math.Abs(stateDict[stateId].Item1);
				if (flag)
				{
					this._addedDebuffState.Add(stateId);
				}
			}
			this._lastDebuffState.Clear();
			foreach (KeyValuePair<short, ValueTuple<short, bool, int>> state in stateDict)
			{
				this._lastDebuffState.Add(state.Key, new ValueTuple<short, bool>(state.Value.Item1, state.Value.Item2));
			}
			ObjectPool<List<short>>.Instance.Return(stateIdList);
		}

		// Token: 0x06003BEB RID: 15339 RVA: 0x0024AC90 File Offset: 0x00248E90
		private void OnDistanceChanged(DataContext context, CombatCharacter mover, short distance, bool isMove, bool isForced)
		{
			bool flag = mover.GetId() != base.CharacterId || !isMove;
			if (!flag)
			{
				this._movedDist += Math.Abs(distance);
				while (this._movedDist >= 2)
				{
					this._injuryRandomPool.Clear();
					for (sbyte bodyPart = 0; bodyPart < 7; bodyPart += 1)
					{
						ValueTuple<sbyte, sbyte> injury = this._addedInjuries.Get(bodyPart);
						ValueTuple<sbyte, sbyte> oldInjury = this._addedOldInjuries.Get(bodyPart);
						for (int i = 0; i < (int)(injury.Item1 - oldInjury.Item1); i++)
						{
							this._injuryRandomPool.Add(new ValueTuple<sbyte, bool>(bodyPart, false));
						}
						for (int j = 0; j < (int)(injury.Item2 - oldInjury.Item2); j++)
						{
							this._injuryRandomPool.Add(new ValueTuple<sbyte, bool>(bodyPart, true));
						}
					}
					bool flag2 = this._injuryRandomPool.Count > 0;
					if (flag2)
					{
						Injuries injuries = base.CombatChar.GetInjuries();
						ValueTuple<sbyte, bool> removeInjury = this._injuryRandomPool[context.Random.Next(0, this._injuryRandomPool.Count)];
						injuries.Change(removeInjury.Item1, removeInjury.Item2, -1);
						DomainManager.Combat.SetInjuries(context, base.CombatChar, injuries, true, true);
						bool flag3 = this._addedInjuries.Get(removeInjury.Item1, removeInjury.Item2) > 0;
						if (flag3)
						{
							this._addedInjuries.Change(removeInjury.Item1, removeInjury.Item2, -1);
						}
						base.ShowSpecialEffectTips(0);
					}
					bool flag4 = this._addedDebuffState.Count > 0;
					if (flag4)
					{
						DomainManager.Combat.RemoveCombatState(context, base.CombatChar, 2, this._addedDebuffState[context.Random.Next(0, this._addedDebuffState.Count)]);
						base.ShowSpecialEffectTips(1);
					}
					bool flag5 = base.CombatChar.GetFlawCollection().GetTotalCount() > 0 || base.CombatChar.GetAcupointCollection().GetTotalCount() > 0;
					if (flag5)
					{
						DomainManager.Combat.ReduceFlawKeepTimePercent(context, base.CombatChar, 10, true);
						DomainManager.Combat.ReduceAcupointKeepTimePercent(context, base.CombatChar, 10, true);
						base.ShowSpecialEffectTips(2);
					}
					this._movedDist -= 2;
				}
			}
		}

		// Token: 0x06003BEC RID: 15340 RVA: 0x0024AF1C File Offset: 0x0024911C
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				base.RemoveSelf(context);
			}
		}

		// Token: 0x04001190 RID: 4496
		private const sbyte MoveDistInPrepare = 30;

		// Token: 0x04001191 RID: 4497
		private const int AffectDistanceUnit = 2;

		// Token: 0x04001192 RID: 4498
		private const int ReduceFlawOrAcupointKeepTimePercent = 10;

		// Token: 0x04001193 RID: 4499
		private short _movedDist;

		// Token: 0x04001194 RID: 4500
		private Injuries _lastInjuries;

		// Token: 0x04001195 RID: 4501
		private Injuries _lastOldInjuries;

		// Token: 0x04001196 RID: 4502
		private Injuries _addedInjuries;

		// Token: 0x04001197 RID: 4503
		private Injuries _addedOldInjuries;

		// Token: 0x04001198 RID: 4504
		private DataUid _injuryUid;

		// Token: 0x04001199 RID: 4505
		private DataUid _oldInjuryUid;

		// Token: 0x0400119A RID: 4506
		[TupleElementNames(new string[]
		{
			"part",
			"inner"
		})]
		private readonly List<ValueTuple<sbyte, bool>> _injuryRandomPool = new List<ValueTuple<sbyte, bool>>();

		// Token: 0x0400119B RID: 4507
		[TupleElementNames(new string[]
		{
			"power",
			"reverse"
		})]
		private readonly Dictionary<short, ValueTuple<short, bool>> _lastDebuffState = new Dictionary<short, ValueTuple<short, bool>>();

		// Token: 0x0400119C RID: 4508
		private readonly List<short> _addedDebuffState = new List<short>();

		// Token: 0x0400119D RID: 4509
		private DataUid _debuffStateUid;
	}
}
