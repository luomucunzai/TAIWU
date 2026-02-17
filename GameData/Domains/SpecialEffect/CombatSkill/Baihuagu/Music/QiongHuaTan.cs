using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.Baihuagu.Music
{
	// Token: 0x020005C7 RID: 1479
	public class QiongHuaTan : CombatSkillEffectBase
	{
		// Token: 0x060043D1 RID: 17361 RVA: 0x0026CC8B File Offset: 0x0026AE8B
		public QiongHuaTan()
		{
		}

		// Token: 0x060043D2 RID: 17362 RVA: 0x0026CC95 File Offset: 0x0026AE95
		public QiongHuaTan(CombatSkillKey skillKey) : base(skillKey, 3304, -1)
		{
		}

		// Token: 0x060043D3 RID: 17363 RVA: 0x0026CCA6 File Offset: 0x0026AEA6
		public override void OnEnable(DataContext context)
		{
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x060043D4 RID: 17364 RVA: 0x0026CCBB File Offset: 0x0026AEBB
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x060043D5 RID: 17365 RVA: 0x0026CCD0 File Offset: 0x0026AED0
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				bool flag2 = base.PowerMatchAffectRequire((int)power, 0);
				if (flag2)
				{
					this.DoAffect(context);
				}
				base.RemoveSelf(context);
			}
		}

		// Token: 0x060043D6 RID: 17366 RVA: 0x0026CD1C File Offset: 0x0026AF1C
		private void DoAffect(DataContext context)
		{
			CombatCharacter stateChar = base.IsDirect ? base.CombatChar : base.EnemyChar;
			sbyte stateType = base.IsDirect ? 2 : 1;
			Dictionary<short, ValueTuple<short, bool, int>> stateDict = stateChar.GetCombatStateCollection(stateType).StateDict;
			bool flag = stateDict.Count <= 0;
			if (!flag)
			{
				List<short> pool = ObjectPool<List<short>>.Instance.Get();
				pool.Clear();
				pool.AddRange(stateDict.Where(delegate([TupleElementNames(new string[]
				{
					"power",
					"reverse",
					"srcCharId"
				})] KeyValuePair<short, ValueTuple<short, bool, int>> state)
				{
					KeyValuePair<short, ValueTuple<short, bool, int>> keyValuePair = state;
					return keyValuePair.Value.Item1 >= 300;
				}).Select(delegate([TupleElementNames(new string[]
				{
					"power",
					"reverse",
					"srcCharId"
				})] KeyValuePair<short, ValueTuple<short, bool, int>> state)
				{
					KeyValuePair<short, ValueTuple<short, bool, int>> keyValuePair = state;
					return keyValuePair.Key;
				}));
				foreach (short stateId in pool)
				{
					DomainManager.Combat.RemoveCombatState(context, stateChar, stateType, stateId);
				}
				sbyte newStateType = base.IsDirect ? 1 : 2;
				short newStateId = base.IsDirect ? 222 : 223;
				bool flag2 = pool.Count > 0;
				if (flag2)
				{
					DomainManager.Combat.AddCombatState(context, stateChar, newStateType, newStateId, 100 * pool.Count);
					base.ShowSpecialEffectTips(0);
					base.ShowSpecialEffectTips(1);
				}
				ObjectPool<List<short>>.Instance.Return(pool);
			}
		}

		// Token: 0x04001424 RID: 5156
		private const int StateClearThreshold = 300;

		// Token: 0x04001425 RID: 5157
		private const int StatePowerUnit = 100;
	}
}
