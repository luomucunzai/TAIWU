using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Common.Assist
{
	// Token: 0x020005AE RID: 1454
	public class NeiliAllocationChangeInjury : AssistSkillBase
	{
		// Token: 0x0600433D RID: 17213 RVA: 0x0026A938 File Offset: 0x00268B38
		protected NeiliAllocationChangeInjury()
		{
		}

		// Token: 0x0600433E RID: 17214 RVA: 0x0026A942 File Offset: 0x00268B42
		protected NeiliAllocationChangeInjury(CombatSkillKey skillKey, int type) : base(skillKey, type)
		{
		}

		// Token: 0x0600433F RID: 17215 RVA: 0x0026A950 File Offset: 0x00268B50
		public override void OnEnable(DataContext context)
		{
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, base.IsDirect ? 102 : 69, -1, -1, -1, -1), EDataModifyType.AddPercent);
			Events.RegisterHandler_NormalAttackEnd(new Events.OnNormalAttackEnd(this.OnNormalAttackEnd));
			Events.RegisterHandler_AttackSkillAttackEnd(new Events.OnAttackSkillAttackEnd(this.OnAttackSkillAttackEnd));
		}

		// Token: 0x06004340 RID: 17216 RVA: 0x0026A9B7 File Offset: 0x00268BB7
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_NormalAttackEnd(new Events.OnNormalAttackEnd(this.OnNormalAttackEnd));
			Events.UnRegisterHandler_AttackSkillAttackEnd(new Events.OnAttackSkillAttackEnd(this.OnAttackSkillAttackEnd));
		}

		// Token: 0x06004341 RID: 17217 RVA: 0x0026A9E0 File Offset: 0x00268BE0
		private void OnNormalAttackEnd(DataContext context, CombatCharacter attacker, CombatCharacter defender, sbyte trickType, int pursueIndex, bool hit, bool isFightBack)
		{
			bool flag = !this._affected || base.CombatChar != (base.IsDirect ? defender : attacker);
			if (!flag)
			{
				this._affected = false;
				base.ShowSpecialEffectTips(0);
				base.ShowEffectTips(context);
			}
		}

		// Token: 0x06004342 RID: 17218 RVA: 0x0026AA30 File Offset: 0x00268C30
		private void OnAttackSkillAttackEnd(CombatContext context, sbyte hitType, bool hit, int index)
		{
			bool flag = !this._affected || base.CombatChar != (base.IsDirect ? context.Defender : context.Attacker);
			if (!flag)
			{
				this._affected = false;
				base.ShowSpecialEffectTips(0);
				base.ShowEffectTips(context);
			}
		}

		// Token: 0x06004343 RID: 17219 RVA: 0x0026AA90 File Offset: 0x00268C90
		public unsafe override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = !base.CanAffect || (!base.IsDirect && (!DomainManager.Combat.IsCurrentCombatCharacter(base.CombatChar) || base.CombatChar.TeammateBeforeMainChar >= 0));
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				short selfValue = *(ref base.CombatChar.GetNeiliAllocation().Items.FixedElementField + (IntPtr)this.RequireNeiliAllocationType * 2);
				short enemyValue = *(ref DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly, false).GetNeiliAllocation().Items.FixedElementField + (IntPtr)this.RequireNeiliAllocationType * 2);
				bool flag2 = !(base.IsDirect ? (selfValue > enemyValue) : (selfValue > enemyValue * 2));
				if (flag2)
				{
					result = 0;
				}
				else
				{
					bool flag3 = dataKey.FieldId == (base.IsDirect ? 102 : 69);
					if (flag3)
					{
						this._affected = true;
						result = (base.IsDirect ? -15 : 15);
					}
					else
					{
						result = 0;
					}
				}
			}
			return result;
		}

		// Token: 0x040013F2 RID: 5106
		private const short DamageChangePercent = 15;

		// Token: 0x040013F3 RID: 5107
		protected byte RequireNeiliAllocationType;

		// Token: 0x040013F4 RID: 5108
		private bool _affected;
	}
}
