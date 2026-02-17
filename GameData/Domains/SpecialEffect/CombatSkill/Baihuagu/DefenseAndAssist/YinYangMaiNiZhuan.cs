using System;
using System.Collections.Generic;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Defense;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.Baihuagu.DefenseAndAssist
{
	// Token: 0x020005DD RID: 1501
	public class YinYangMaiNiZhuan : DefenseSkillBase
	{
		// Token: 0x06004458 RID: 17496 RVA: 0x0026F453 File Offset: 0x0026D653
		public YinYangMaiNiZhuan()
		{
		}

		// Token: 0x06004459 RID: 17497 RVA: 0x0026F45D File Offset: 0x0026D65D
		public YinYangMaiNiZhuan(CombatSkillKey skillKey) : base(skillKey, 3504)
		{
		}

		// Token: 0x0600445A RID: 17498 RVA: 0x0026F46D File Offset: 0x0026D66D
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			Events.RegisterHandler_NormalAttackEnd(new Events.OnNormalAttackEnd(this.OnNormalAttackEnd));
		}

		// Token: 0x0600445B RID: 17499 RVA: 0x0026F48A File Offset: 0x0026D68A
		public override void OnDisable(DataContext context)
		{
			base.OnDisable(context);
			Events.UnRegisterHandler_NormalAttackEnd(new Events.OnNormalAttackEnd(this.OnNormalAttackEnd));
		}

		// Token: 0x0600445C RID: 17500 RVA: 0x0026F4A8 File Offset: 0x0026D6A8
		private void OnNormalAttackEnd(DataContext context, CombatCharacter attacker, CombatCharacter defender, sbyte trickType, int pursueIndex, bool hit, bool isFightBack)
		{
			bool flag = !isFightBack || !hit || attacker != base.CombatChar || !base.CanAffect;
			if (!flag)
			{
				CombatCharacter stateChar = base.IsDirect ? base.CombatChar : base.CurrEnemyChar;
				Dictionary<short, ValueTuple<short, bool, int>> stateDict = (base.IsDirect ? stateChar.GetDebuffCombatStateCollection() : stateChar.GetBuffCombatStateCollection()).StateDict;
				bool flag2 = stateDict.Count <= 0;
				if (!flag2)
				{
					List<short> stateRandomPool = ObjectPool<List<short>>.Instance.Get();
					stateRandomPool.Clear();
					stateRandomPool.AddRange(stateDict.Keys);
					short key = stateRandomPool[context.Random.Next(0, stateRandomPool.Count)];
					DomainManager.Combat.ReverseCombatState(context, stateChar, base.IsDirect ? 2 : 1, key);
					ObjectPool<List<short>>.Instance.Return(stateRandomPool);
					base.ShowSpecialEffectTips(0);
				}
			}
		}
	}
}
