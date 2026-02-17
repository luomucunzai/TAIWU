using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Assist;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.Wuxianjiao.DefenseAndAssist
{
	// Token: 0x020003A5 RID: 933
	public class HongYuJiuXiangChong : AssistSkillBase
	{
		// Token: 0x060036A0 RID: 13984 RVA: 0x00231555 File Offset: 0x0022F755
		public HongYuJiuXiangChong()
		{
		}

		// Token: 0x060036A1 RID: 13985 RVA: 0x0023155F File Offset: 0x0022F75F
		public HongYuJiuXiangChong(CombatSkillKey skillKey) : base(skillKey, 12802)
		{
		}

		// Token: 0x060036A2 RID: 13986 RVA: 0x00231570 File Offset: 0x0022F770
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 69, -1, -1, -1, -1), EDataModifyType.AddPercent);
			Events.RegisterHandler_NormalAttackEnd(new Events.OnNormalAttackEnd(this.OnNormalAttackEnd));
			Events.RegisterHandler_AttackSkillAttackEnd(new Events.OnAttackSkillAttackEnd(this.OnAttackSkillAttackEnd));
		}

		// Token: 0x060036A3 RID: 13987 RVA: 0x002315D3 File Offset: 0x0022F7D3
		public override void OnDisable(DataContext context)
		{
			base.OnDisable(context);
			Events.UnRegisterHandler_NormalAttackEnd(new Events.OnNormalAttackEnd(this.OnNormalAttackEnd));
			Events.UnRegisterHandler_AttackSkillAttackEnd(new Events.OnAttackSkillAttackEnd(this.OnAttackSkillAttackEnd));
		}

		// Token: 0x060036A4 RID: 13988 RVA: 0x00231604 File Offset: 0x0022F804
		private void OnNormalAttackEnd(DataContext context, CombatCharacter attacker, CombatCharacter defender, sbyte trickType, int pursueIndex, bool hit, bool isFightBack)
		{
			bool flag = !this._affected || attacker != base.CombatChar;
			if (!flag)
			{
				this._affected = false;
				bool flag2 = pursueIndex == 0;
				if (flag2)
				{
					base.ShowEffectTips(context);
					base.ShowSpecialEffectTips(0);
				}
			}
		}

		// Token: 0x060036A5 RID: 13989 RVA: 0x00231654 File Offset: 0x0022F854
		private void OnAttackSkillAttackEnd(CombatContext context, sbyte hitType, bool hit, int index)
		{
			bool flag = !this._affected || context.Attacker != base.CombatChar;
			if (!flag)
			{
				this._affected = false;
				base.ShowEffectTips(context);
				base.ShowSpecialEffectTips(0);
			}
		}

		// Token: 0x060036A6 RID: 13990 RVA: 0x002316A4 File Offset: 0x0022F8A4
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.CharId != base.CharacterId;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				bool flag2 = dataKey.FieldId == 69;
				if (flag2)
				{
					int addDamage = 5 * (base.IsDirect ? base.CurrEnemyChar : base.CombatChar).GetDefeatMarkCollection().PoisonMarkList.Sum();
					bool flag3 = addDamage > 0;
					if (flag3)
					{
						this._affected = true;
					}
					result = addDamage;
				}
				else
				{
					result = 0;
				}
			}
			return result;
		}

		// Token: 0x04000FE8 RID: 4072
		private const sbyte AddDamageUnit = 5;

		// Token: 0x04000FE9 RID: 4073
		private bool _affected;
	}
}
