using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.Ranshanpai.Finger
{
	// Token: 0x0200045C RID: 1116
	public class DaYinYangZongHengShou : CombatSkillEffectBase
	{
		// Token: 0x06003AC8 RID: 15048 RVA: 0x00245355 File Offset: 0x00243555
		static DaYinYangZongHengShou()
		{
			SpecialEffectDomain.RegisterResetHandler(new Action(DaYinYangZongHengShou.ActiveInstances.Clear));
		}

		// Token: 0x06003AC9 RID: 15049 RVA: 0x00245378 File Offset: 0x00243578
		private static void SetActiveAndRemoveExistInstance(DataContext context, DaYinYangZongHengShou instance)
		{
			for (int i = DaYinYangZongHengShou.ActiveInstances.Count - 1; i >= 0; i--)
			{
				DaYinYangZongHengShou.ActiveInstances[i].RemoveSelf(context);
			}
			DaYinYangZongHengShou.ActiveInstances.Add(instance);
		}

		// Token: 0x06003ACA RID: 15050 RVA: 0x002453C2 File Offset: 0x002435C2
		private static void UnsetActive(DaYinYangZongHengShou instance)
		{
			Tester.Assert(DaYinYangZongHengShou.ActiveInstances.Remove(instance), "");
		}

		// Token: 0x1700021A RID: 538
		// (get) Token: 0x06003ACB RID: 15051 RVA: 0x002453DB File Offset: 0x002435DB
		private CombatCharacter SrcChar
		{
			get
			{
				return base.IsDirect ? base.CombatChar : base.EnemyChar;
			}
		}

		// Token: 0x1700021B RID: 539
		// (get) Token: 0x06003ACC RID: 15052 RVA: 0x002453F3 File Offset: 0x002435F3
		private CombatCharacter DstChar
		{
			get
			{
				return base.IsDirect ? base.EnemyChar : base.CombatChar;
			}
		}

		// Token: 0x1700021C RID: 540
		// (get) Token: 0x06003ACD RID: 15053 RVA: 0x0024540B File Offset: 0x0024360B
		private sbyte SrcStateType
		{
			get
			{
				return base.IsDirect ? 1 : 2;
			}
		}

		// Token: 0x1700021D RID: 541
		// (get) Token: 0x06003ACE RID: 15054 RVA: 0x00245419 File Offset: 0x00243619
		private sbyte DstStateType
		{
			get
			{
				return base.IsDirect ? 2 : 1;
			}
		}

		// Token: 0x06003ACF RID: 15055 RVA: 0x00245427 File Offset: 0x00243627
		public DaYinYangZongHengShou()
		{
		}

		// Token: 0x06003AD0 RID: 15056 RVA: 0x00245431 File Offset: 0x00243631
		public DaYinYangZongHengShou(CombatSkillKey skillKey) : base(skillKey, 7105, -1)
		{
		}

		// Token: 0x06003AD1 RID: 15057 RVA: 0x00245444 File Offset: 0x00243644
		public override void OnEnable(DataContext context)
		{
			DaYinYangZongHengShou.SetActiveAndRemoveExistInstance(context, this);
			this._affectChar = this.DstChar;
			Dictionary<short, ValueTuple<short, bool, int>> srcStateDict = this.SrcChar.GetCombatStateCollection(this.SrcStateType).StateDict;
			Dictionary<short, ValueTuple<short, bool, int>> dstStateDict = this.DstChar.GetCombatStateCollection(this.DstStateType).StateDict;
			bool flag = srcStateDict.Count > 0;
			if (flag)
			{
				this._copiedStates = new Dictionary<short, ValueTuple<short, bool>>();
				foreach (KeyValuePair<short, ValueTuple<short, bool, int>> stateEntry in srcStateDict)
				{
					ValueTuple<short, bool> valueTuple = CombatDomain.CalcReversedCombatState(stateEntry.Key, stateEntry.Value.Item2);
					short stateId = valueTuple.Item1;
					bool reverse = valueTuple.Item2;
					short power = stateEntry.Value.Item1;
					bool flag2 = dstStateDict.ContainsKey(stateId);
					if (!flag2)
					{
						this._copiedStates.Add(stateId, new ValueTuple<short, bool>(power, reverse));
						DomainManager.Combat.AddCombatState(context, this._affectChar, this.DstStateType, stateId, (int)power, reverse, false);
						base.ShowSpecialEffectTipsOnceInFrame(0);
					}
				}
			}
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
			Events.RegisterHandler_SkillEffectChange(new Events.OnSkillEffectChange(this.OnSkillEffectChange));
			Events.RegisterHandler_CombatStateMachineUpdateEnd(new Events.OnCombatStateMachineUpdateEnd(this.OnCombatStateMachineUpdateEnd));
		}

		// Token: 0x06003AD2 RID: 15058 RVA: 0x002455B0 File Offset: 0x002437B0
		public override void OnDisable(DataContext context)
		{
			bool flag = this._copiedStates != null;
			if (flag)
			{
				foreach (KeyValuePair<short, ValueTuple<short, bool>> stateEntry in this._copiedStates)
				{
					DomainManager.Combat.AddCombatState(context, this._affectChar, this.DstStateType, stateEntry.Key, (int)(-(int)stateEntry.Value.Item1), stateEntry.Value.Item2, false);
				}
			}
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
			Events.UnRegisterHandler_SkillEffectChange(new Events.OnSkillEffectChange(this.OnSkillEffectChange));
			Events.UnRegisterHandler_CombatStateMachineUpdateEnd(new Events.OnCombatStateMachineUpdateEnd(this.OnCombatStateMachineUpdateEnd));
			DaYinYangZongHengShou.UnsetActive(this);
		}

		// Token: 0x06003AD3 RID: 15059 RVA: 0x00245684 File Offset: 0x00243884
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				bool flag2 = base.PowerMatchAffectRequire((int)power, 0);
				if (flag2)
				{
					base.AddMaxEffectCount(true);
				}
				else
				{
					base.RemoveSelf(context);
				}
			}
		}

		// Token: 0x06003AD4 RID: 15060 RVA: 0x002456D0 File Offset: 0x002438D0
		private void OnSkillEffectChange(DataContext context, int charId, SkillEffectKey key, short oldCount, short newCount, bool removed)
		{
			bool flag = charId != base.CharacterId || key != base.EffectKey || newCount > 0;
			if (!flag)
			{
				base.RemoveSelf(context);
			}
		}

		// Token: 0x06003AD5 RID: 15061 RVA: 0x0024570C File Offset: 0x0024390C
		private void OnCombatStateMachineUpdateEnd(DataContext context, CombatCharacter combatChar)
		{
			bool flag = combatChar.GetId() != base.CharacterId || DomainManager.Combat.Pause;
			if (!flag)
			{
				this._autoRemoveCounter++;
				bool flag2 = this._autoRemoveCounter >= 1200;
				if (flag2)
				{
					base.ReduceEffectCount(1);
				}
			}
		}

		// Token: 0x04001135 RID: 4405
		private const int StateFrame = 1200;

		// Token: 0x04001136 RID: 4406
		private static readonly List<DaYinYangZongHengShou> ActiveInstances = new List<DaYinYangZongHengShou>();

		// Token: 0x04001137 RID: 4407
		private CombatCharacter _affectChar;

		// Token: 0x04001138 RID: 4408
		[TupleElementNames(new string[]
		{
			"power",
			"reverse"
		})]
		private Dictionary<short, ValueTuple<short, bool>> _copiedStates;

		// Token: 0x04001139 RID: 4409
		private int _autoRemoveCounter;
	}
}
