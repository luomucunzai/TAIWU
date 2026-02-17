using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.Item;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Defense;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.Baihuagu.DefenseAndAssist
{
	// Token: 0x020005DC RID: 1500
	public class XueHaiNingBingShu : DefenseSkillBase
	{
		// Token: 0x06004450 RID: 17488 RVA: 0x0026F105 File Offset: 0x0026D305
		public XueHaiNingBingShu()
		{
		}

		// Token: 0x06004451 RID: 17489 RVA: 0x0026F11A File Offset: 0x0026D31A
		public XueHaiNingBingShu(CombatSkillKey skillKey) : base(skillKey, 3507)
		{
		}

		// Token: 0x06004452 RID: 17490 RVA: 0x0026F138 File Offset: 0x0026D338
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 162, -1, -1, -1, -1), EDataModifyType.Custom);
			Events.RegisterHandler_AddPoison(new Events.OnAddPoison(this.OnAddPoison));
		}

		// Token: 0x06004453 RID: 17491 RVA: 0x0026F18C File Offset: 0x0026D38C
		public override void OnDisable(DataContext context)
		{
			base.OnDisable(context);
			Events.UnRegisterHandler_AddPoison(new Events.OnAddPoison(this.OnAddPoison));
			bool flag = this._acceptedPoison.Count > 0 && DomainManager.Combat.IsInCombat() && base.SkillData.GetCanAffect();
			if (flag)
			{
				int totalPoison = 0;
				foreach (sbyte type in this._acceptedPoison.Keys)
				{
					int value = this._acceptedPoison[type];
					DomainManager.Combat.ReducePoison(context, base.CombatChar, type, value, true, false);
					totalPoison += value;
				}
				int statePower = totalPoison / 5;
				bool flag2 = statePower > 0;
				if (flag2)
				{
					DomainManager.Combat.AddCombatState(context, base.CombatChar, 1, base.IsDirect ? 20 : 21, statePower);
				}
				base.ShowSpecialEffectTips(1);
			}
		}

		// Token: 0x06004454 RID: 17492 RVA: 0x0026F294 File Offset: 0x0026D494
		private void OnAddPoison(DataContext context, int attackerId, int defenderId, sbyte poisonType, sbyte level, int addValue, short skillId, bool canBounce)
		{
			bool flag = defenderId != base.CharacterId || !base.CanAffect || (base.IsDirect ? XueHaiNingBingShu.DirectPoisonTypes : XueHaiNingBingShu.ReversePoisonTypes).IndexOf(poisonType) < 0;
			if (!flag)
			{
				bool flag2 = !this._acceptedPoison.ContainsKey(poisonType);
				if (flag2)
				{
					this._acceptedPoison[poisonType] = addValue;
				}
				else
				{
					Dictionary<sbyte, int> acceptedPoison = this._acceptedPoison;
					acceptedPoison[poisonType] += addValue;
				}
				if (canBounce)
				{
					this._bouncePoison = new ValueTuple<sbyte, int, sbyte>(poisonType, addValue, level);
					Events.RegisterHandler_CombatStateMachineUpdateEnd(new Events.OnCombatStateMachineUpdateEnd(this.OnStateMachineUpdateEnd));
				}
			}
		}

		// Token: 0x06004455 RID: 17493 RVA: 0x0026F34C File Offset: 0x0026D54C
		private void OnStateMachineUpdateEnd(DataContext context, CombatCharacter combatChar)
		{
			DomainManager.Combat.AddPoison(context, base.CombatChar, base.CurrEnemyChar, this._bouncePoison.Item1, this._bouncePoison.Item3, this._bouncePoison.Item2, -1, true, false, default(ItemKey), false, false, false);
			base.ShowSpecialEffectTips(0);
			Events.UnRegisterHandler_CombatStateMachineUpdateEnd(new Events.OnCombatStateMachineUpdateEnd(this.OnStateMachineUpdateEnd));
		}

		// Token: 0x06004456 RID: 17494 RVA: 0x0026F3BC File Offset: 0x0026D5BC
		public override bool GetModifiedValue(AffectedDataKey dataKey, bool dataValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || !base.CanAffect || (base.IsDirect ? XueHaiNingBingShu.DirectPoisonTypes : XueHaiNingBingShu.ReversePoisonTypes).IndexOf((sbyte)dataKey.CustomParam0) < 0;
			bool result;
			if (flag)
			{
				result = dataValue;
			}
			else
			{
				bool flag2 = dataKey.FieldId == 162;
				result = (!flag2 && dataValue);
			}
			return result;
		}

		// Token: 0x04001441 RID: 5185
		private const sbyte RequirePoisonPerStatePower = 5;

		// Token: 0x04001442 RID: 5186
		private static readonly sbyte[] DirectPoisonTypes = new sbyte[]
		{
			0,
			3,
			4
		};

		// Token: 0x04001443 RID: 5187
		private static readonly sbyte[] ReversePoisonTypes = new sbyte[]
		{
			1,
			2,
			5
		};

		// Token: 0x04001444 RID: 5188
		private readonly Dictionary<sbyte, int> _acceptedPoison = new Dictionary<sbyte, int>();

		// Token: 0x04001445 RID: 5189
		[TupleElementNames(new string[]
		{
			"type",
			"value",
			"level"
		})]
		private ValueTuple<sbyte, int, sbyte> _bouncePoison;
	}
}
