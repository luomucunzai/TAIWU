using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Defense;

namespace GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Defense
{
	// Token: 0x020002AD RID: 685
	public class HuanXin : DefenseSkillBase
	{
		// Token: 0x060031EC RID: 12780 RVA: 0x0021CCB5 File Offset: 0x0021AEB5
		public HuanXin()
		{
		}

		// Token: 0x060031ED RID: 12781 RVA: 0x0021CCBF File Offset: 0x0021AEBF
		public HuanXin(CombatSkillKey skillKey) : base(skillKey, 16311)
		{
		}

		// Token: 0x060031EE RID: 12782 RVA: 0x0021CCD0 File Offset: 0x0021AED0
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 102, -1, -1, -1, -1), EDataModifyType.AddPercent);
			Events.RegisterHandler_NormalAttackEnd(new Events.OnNormalAttackEnd(this.OnNormalAttackEnd));
			Events.RegisterHandler_AttackSkillAttackEnd(new Events.OnAttackSkillAttackEnd(this.OnAttackSkillAttackEnd));
		}

		// Token: 0x060031EF RID: 12783 RVA: 0x0021CD33 File Offset: 0x0021AF33
		public override void OnDisable(DataContext context)
		{
			base.OnDisable(context);
			Events.UnRegisterHandler_NormalAttackEnd(new Events.OnNormalAttackEnd(this.OnNormalAttackEnd));
			Events.UnRegisterHandler_AttackSkillAttackEnd(new Events.OnAttackSkillAttackEnd(this.OnAttackSkillAttackEnd));
		}

		// Token: 0x060031F0 RID: 12784 RVA: 0x0021CD64 File Offset: 0x0021AF64
		private void OnNormalAttackEnd(DataContext context, CombatCharacter attacker, CombatCharacter defender, sbyte trickType, int pursueIndex, bool hit, bool isFightBack)
		{
			bool flag = !this._affected || base.CombatChar != defender;
			if (!flag)
			{
				this._affected = false;
				base.ShowSpecialEffectTips(0);
			}
		}

		// Token: 0x060031F1 RID: 12785 RVA: 0x0021CDA0 File Offset: 0x0021AFA0
		private void OnAttackSkillAttackEnd(CombatContext context, sbyte hitType, bool hit, int index)
		{
			bool flag = !this._affected || base.CombatChar != context.Defender;
			if (!flag)
			{
				this._affected = false;
				base.ShowSpecialEffectTips(0);
			}
		}

		// Token: 0x060031F2 RID: 12786 RVA: 0x0021CDE0 File Offset: 0x0021AFE0
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
				int buffCount = base.CombatChar.GetBuffCombatStateCollection().StateDict.Count;
				bool flag2 = buffCount > 0 && dataKey.FieldId == 102;
				if (flag2)
				{
					this._affected = true;
					result = -15 * buffCount;
				}
				else
				{
					result = 0;
				}
			}
			return result;
		}

		// Token: 0x04000ECA RID: 3786
		private const sbyte ReduceDamagePercent = -15;

		// Token: 0x04000ECB RID: 3787
		private bool _affected;
	}
}
