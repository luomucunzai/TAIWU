using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Defense;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.Ranshanpai.DefenseAndAssist
{
	// Token: 0x02000468 RID: 1128
	public class XiangNiBiZhuanGong : DefenseSkillBase
	{
		// Token: 0x06003B14 RID: 15124 RVA: 0x002467A3 File Offset: 0x002449A3
		public XiangNiBiZhuanGong()
		{
		}

		// Token: 0x06003B15 RID: 15125 RVA: 0x002467AD File Offset: 0x002449AD
		public XiangNiBiZhuanGong(CombatSkillKey skillKey) : base(skillKey, 7506)
		{
		}

		// Token: 0x06003B16 RID: 15126 RVA: 0x002467C0 File Offset: 0x002449C0
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 102, -1, -1, -1, -1), EDataModifyType.AddPercent);
			Events.RegisterHandler_NormalAttackEnd(new Events.OnNormalAttackEnd(this.OnNormalAttackEnd));
			Events.RegisterHandler_AttackSkillAttackEnd(new Events.OnAttackSkillAttackEnd(this.OnAttackSkillAttackEnd));
			Events.RegisterHandler_BounceInjury(new Events.OnBounceInjury(this.OnBounceInjury));
		}

		// Token: 0x06003B17 RID: 15127 RVA: 0x00246838 File Offset: 0x00244A38
		public override void OnDisable(DataContext context)
		{
			base.OnDisable(context);
			Events.UnRegisterHandler_NormalAttackEnd(new Events.OnNormalAttackEnd(this.OnNormalAttackEnd));
			Events.UnRegisterHandler_AttackSkillAttackEnd(new Events.OnAttackSkillAttackEnd(this.OnAttackSkillAttackEnd));
			Events.UnRegisterHandler_BounceInjury(new Events.OnBounceInjury(this.OnBounceInjury));
		}

		// Token: 0x06003B18 RID: 15128 RVA: 0x00246884 File Offset: 0x00244A84
		private void OnNormalAttackEnd(DataContext context, CombatCharacter attacker, CombatCharacter defender, sbyte trickType, int pursueIndex, bool hit, bool isFightBack)
		{
			bool flag = !this._affected || defender != base.CombatChar;
			if (!flag)
			{
				this._affected = false;
				base.ShowSpecialEffectTips(0);
			}
		}

		// Token: 0x06003B19 RID: 15129 RVA: 0x002468C0 File Offset: 0x00244AC0
		private void OnAttackSkillAttackEnd(CombatContext context, sbyte hitType, bool hit, int index)
		{
			bool flag = !this._affected || context.Defender != base.CombatChar;
			if (!flag)
			{
				this._affected = false;
				base.ShowSpecialEffectTips(0);
			}
		}

		// Token: 0x06003B1A RID: 15130 RVA: 0x00246900 File Offset: 0x00244B00
		private void OnBounceInjury(DataContext context, int attackerId, int defenderId, bool isAlly, sbyte bodyPart, sbyte outerMarkCount, sbyte innerMarkCount)
		{
			bool flag = attackerId != base.CharacterId || !base.CanAffect;
			if (!flag)
			{
				CombatCharacter enemyChar = base.CurrEnemyChar;
				Dictionary<short, ValueTuple<short, bool, int>> stateDict = (base.IsDirect ? base.CombatChar.GetDebuffCombatStateCollection() : enemyChar.GetBuffCombatStateCollection()).StateDict;
				bool flag2 = stateDict.Count == 0;
				if (!flag2)
				{
					List<short> stateRandomPool = ObjectPool<List<short>>.Instance.Get();
					stateRandomPool.Clear();
					stateRandomPool.AddRange(stateDict.Keys);
					short moveStateId = stateRandomPool[context.Random.Next(0, stateRandomPool.Count)];
					ValueTuple<short, bool, int> stateInfo = stateDict[moveStateId];
					ObjectPool<List<short>>.Instance.Return(stateRandomPool);
					bool isDirect = base.IsDirect;
					if (isDirect)
					{
						DomainManager.Combat.RemoveCombatState(context, base.CombatChar, 2, moveStateId);
						DomainManager.Combat.AddCombatState(context, enemyChar, 2, moveStateId, (int)stateInfo.Item1, stateInfo.Item2);
					}
					else
					{
						DomainManager.Combat.RemoveCombatState(context, enemyChar, 1, moveStateId);
						DomainManager.Combat.AddCombatState(context, base.CombatChar, 1, moveStateId, (int)stateInfo.Item1, stateInfo.Item2);
					}
					base.ShowSpecialEffectTips(1);
				}
			}
		}

		// Token: 0x06003B1B RID: 15131 RVA: 0x00246A34 File Offset: 0x00244C34
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || !base.CanAffect;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				bool flag2 = dataKey.FieldId == 102;
				if (flag2)
				{
					this._affected = true;
					result = -45;
				}
				else
				{
					result = 0;
				}
			}
			return result;
		}

		// Token: 0x0400114F RID: 4431
		private const sbyte ReduceDamagePercent = -45;

		// Token: 0x04001150 RID: 4432
		private bool _affected;
	}
}
