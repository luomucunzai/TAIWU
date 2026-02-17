using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Common.Assist
{
	// Token: 0x020005AB RID: 1451
	public class AddDamageByHitType : AssistSkillBase
	{
		// Token: 0x06004326 RID: 17190 RVA: 0x0026A3BE File Offset: 0x002685BE
		protected AddDamageByHitType()
		{
		}

		// Token: 0x06004327 RID: 17191 RVA: 0x0026A3C8 File Offset: 0x002685C8
		protected AddDamageByHitType(CombatSkillKey skillKey, int type) : base(skillKey, type)
		{
		}

		// Token: 0x06004328 RID: 17192 RVA: 0x0026A3D4 File Offset: 0x002685D4
		public override void OnEnable(DataContext context)
		{
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 69, -1, -1, -1, -1), EDataModifyType.AddPercent);
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 102, -1, -1, -1, -1), EDataModifyType.AddPercent);
			Events.RegisterHandler_NormalAttackEnd(new Events.OnNormalAttackEnd(this.OnNormalAttackEnd));
			Events.RegisterHandler_AttackSkillAttackEnd(new Events.OnAttackSkillAttackEnd(this.OnAttackSkillAttackEnd));
		}

		// Token: 0x06004329 RID: 17193 RVA: 0x0026A44D File Offset: 0x0026864D
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_NormalAttackEnd(new Events.OnNormalAttackEnd(this.OnNormalAttackEnd));
			Events.UnRegisterHandler_AttackSkillAttackEnd(new Events.OnAttackSkillAttackEnd(this.OnAttackSkillAttackEnd));
		}

		// Token: 0x0600432A RID: 17194 RVA: 0x0026A474 File Offset: 0x00268674
		private void OnNormalAttackEnd(DataContext context, CombatCharacter attacker, CombatCharacter defender, sbyte trickType, int pursueIndex, bool hit, bool isFightBack)
		{
			bool flag = !this._affected;
			if (!flag)
			{
				this._affected = false;
				base.ShowEffectTips(context);
			}
		}

		// Token: 0x0600432B RID: 17195 RVA: 0x0026A4A0 File Offset: 0x002686A0
		private void OnAttackSkillAttackEnd(CombatContext context, sbyte hitType, bool hit, int index)
		{
			bool flag = !this._affected;
			if (!flag)
			{
				this._affected = false;
				base.ShowEffectTips(context);
			}
		}

		// Token: 0x0600432C RID: 17196 RVA: 0x0026A4D4 File Offset: 0x002686D4
		public unsafe override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || !base.CanAffect;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				Character enemyChar = base.CurrEnemyChar.GetCharacter();
				HitOrAvoidInts selfHit = this.CharObj.GetHitValues();
				HitOrAvoidInts selfAvoid = this.CharObj.GetAvoidValues();
				HitOrAvoidInts enemyHit = enemyChar.GetHitValues();
				HitOrAvoidInts enemyAvoid = enemyChar.GetAvoidValues();
				bool flag2 = ((*(ref selfHit.Items.FixedElementField + (IntPtr)this.HitType * 4) > *(ref enemyHit.Items.FixedElementField + (IntPtr)this.HitType * 4) || *(ref selfAvoid.Items.FixedElementField + (IntPtr)this.HitType * 4) > *(ref enemyAvoid.Items.FixedElementField + (IntPtr)this.HitType * 4)) && dataKey.FieldId == 69) || ((*(ref selfHit.Items.FixedElementField + (IntPtr)this.HitType * 4) < *(ref enemyHit.Items.FixedElementField + (IntPtr)this.HitType * 4) || *(ref selfAvoid.Items.FixedElementField + (IntPtr)this.HitType * 4) < *(ref enemyAvoid.Items.FixedElementField + (IntPtr)this.HitType * 4)) && dataKey.FieldId == 102);
				if (flag2)
				{
					this._affected = true;
					result = 50;
				}
				else
				{
					result = 0;
				}
			}
			return result;
		}

		// Token: 0x040013E9 RID: 5097
		private const sbyte AddDamage = 50;

		// Token: 0x040013EA RID: 5098
		protected sbyte HitType;

		// Token: 0x040013EB RID: 5099
		private bool _affected;
	}
}
