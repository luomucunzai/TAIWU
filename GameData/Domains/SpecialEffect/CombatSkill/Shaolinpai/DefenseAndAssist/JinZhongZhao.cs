using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Defense;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.Shaolinpai.DefenseAndAssist
{
	// Token: 0x02000435 RID: 1077
	public class JinZhongZhao : DefenseSkillBase
	{
		// Token: 0x060039D3 RID: 14803 RVA: 0x00240A40 File Offset: 0x0023EC40
		public JinZhongZhao()
		{
		}

		// Token: 0x060039D4 RID: 14804 RVA: 0x00240A4A File Offset: 0x0023EC4A
		public JinZhongZhao(CombatSkillKey skillKey) : base(skillKey, 1504)
		{
		}

		// Token: 0x060039D5 RID: 14805 RVA: 0x00240A5C File Offset: 0x0023EC5C
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 102, -1, -1, -1, -1), EDataModifyType.AddPercent);
			Events.RegisterHandler_NormalAttackEnd(new Events.OnNormalAttackEnd(this.OnNormalAttackEnd));
			Events.RegisterHandler_AttackSkillAttackEnd(new Events.OnAttackSkillAttackEnd(this.OnAttackSkillAttackEnd));
			Events.RegisterHandler_BounceInjury(new Events.OnBounceInjury(this.OnBounceInjury));
		}

		// Token: 0x060039D6 RID: 14806 RVA: 0x00240AD4 File Offset: 0x0023ECD4
		public override void OnDisable(DataContext context)
		{
			base.OnDisable(context);
			Events.UnRegisterHandler_NormalAttackEnd(new Events.OnNormalAttackEnd(this.OnNormalAttackEnd));
			Events.UnRegisterHandler_AttackSkillAttackEnd(new Events.OnAttackSkillAttackEnd(this.OnAttackSkillAttackEnd));
			Events.UnRegisterHandler_BounceInjury(new Events.OnBounceInjury(this.OnBounceInjury));
		}

		// Token: 0x060039D7 RID: 14807 RVA: 0x00240B20 File Offset: 0x0023ED20
		private void OnNormalAttackEnd(DataContext context, CombatCharacter attacker, CombatCharacter defender, sbyte trickType, int pursueIndex, bool hit, bool isFightBack)
		{
			bool flag = !this._affected || defender != base.CombatChar;
			if (!flag)
			{
				this._affected = false;
				base.ShowSpecialEffectTips(0);
			}
		}

		// Token: 0x060039D8 RID: 14808 RVA: 0x00240B5C File Offset: 0x0023ED5C
		private void OnAttackSkillAttackEnd(CombatContext context, sbyte hitType, bool hit, int index)
		{
			bool flag = !this._affected || context.Defender != base.CombatChar;
			if (!flag)
			{
				this._affected = false;
				base.ShowSpecialEffectTips(0);
			}
		}

		// Token: 0x060039D9 RID: 14809 RVA: 0x00240B9C File Offset: 0x0023ED9C
		private void OnBounceInjury(DataContext context, int attackerId, int defenderId, bool isAlly, sbyte bodyPart, sbyte outerMarkCount, sbyte innerMarkCount)
		{
			bool flag = attackerId != base.CharacterId || !base.CanAffect || (base.IsDirect && base.CombatChar.GetFlawCount().Sum() == 0);
			if (!flag)
			{
				bool isDirect = base.IsDirect;
				if (isDirect)
				{
					List<sbyte> bodyPartRandomPool = ObjectPool<List<sbyte>>.Instance.Get();
					byte[] flawCount = base.CombatChar.GetFlawCount();
					bodyPartRandomPool.Clear();
					for (sbyte part = 0; part < 7; part += 1)
					{
						for (int i = 0; i < (int)flawCount[(int)part]; i++)
						{
							bodyPartRandomPool.Add(part);
						}
					}
					int removeCount = Math.Min(2, bodyPartRandomPool.Count);
					for (int j = 0; j < removeCount; j++)
					{
						int index = context.Random.Next(0, bodyPartRandomPool.Count);
						DomainManager.Combat.RemoveFlaw(context, base.CombatChar, bodyPartRandomPool[index], 0, true, true);
						bodyPartRandomPool.RemoveAt(index);
					}
					bool flag2 = removeCount > 0;
					if (flag2)
					{
						base.ShowSpecialEffectTips(1);
					}
					ObjectPool<List<sbyte>>.Instance.Return(bodyPartRandomPool);
				}
				else
				{
					for (int k = 0; k < 2; k++)
					{
						DomainManager.Combat.AddFlaw(context, base.CurrEnemyChar, 0, this.SkillKey, -1, 1, true);
					}
					base.ShowSpecialEffectTips(1);
				}
			}
		}

		// Token: 0x060039DA RID: 14810 RVA: 0x00240D0C File Offset: 0x0023EF0C
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
					result = -30;
				}
				else
				{
					result = 0;
				}
			}
			return result;
		}

		// Token: 0x040010E8 RID: 4328
		private const sbyte ReduceDamagePercent = -30;

		// Token: 0x040010E9 RID: 4329
		private const sbyte ChangeFlaw = 2;

		// Token: 0x040010EA RID: 4330
		private bool _affected;
	}
}
