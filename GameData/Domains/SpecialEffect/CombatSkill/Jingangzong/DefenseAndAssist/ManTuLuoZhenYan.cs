using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Defense;

namespace GameData.Domains.SpecialEffect.CombatSkill.Jingangzong.DefenseAndAssist
{
	// Token: 0x020004C6 RID: 1222
	public class ManTuLuoZhenYan : DefenseSkillBase
	{
		// Token: 0x06003D2D RID: 15661 RVA: 0x00250A43 File Offset: 0x0024EC43
		public ManTuLuoZhenYan()
		{
		}

		// Token: 0x06003D2E RID: 15662 RVA: 0x00250A4D File Offset: 0x0024EC4D
		public ManTuLuoZhenYan(CombatSkillKey skillKey) : base(skillKey, 11604)
		{
		}

		// Token: 0x06003D2F RID: 15663 RVA: 0x00250A60 File Offset: 0x0024EC60
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 102, -1, -1, -1, -1), EDataModifyType.AddPercent);
			Events.RegisterHandler_NormalAttackEnd(new Events.OnNormalAttackEnd(this.OnNormalAttackEnd));
			Events.RegisterHandler_AttackSkillAttackEnd(new Events.OnAttackSkillAttackEnd(this.OnAttackSkillAttackEnd));
			Events.RegisterHandler_BounceInjury(new Events.OnBounceInjury(this.OnBounceInjury));
		}

		// Token: 0x06003D30 RID: 15664 RVA: 0x00250AD8 File Offset: 0x0024ECD8
		public override void OnDisable(DataContext context)
		{
			base.OnDisable(context);
			Events.UnRegisterHandler_NormalAttackEnd(new Events.OnNormalAttackEnd(this.OnNormalAttackEnd));
			Events.UnRegisterHandler_AttackSkillAttackEnd(new Events.OnAttackSkillAttackEnd(this.OnAttackSkillAttackEnd));
			Events.UnRegisterHandler_BounceInjury(new Events.OnBounceInjury(this.OnBounceInjury));
		}

		// Token: 0x06003D31 RID: 15665 RVA: 0x00250B24 File Offset: 0x0024ED24
		private void OnNormalAttackEnd(DataContext context, CombatCharacter attacker, CombatCharacter defender, sbyte trickType, int pursueIndex, bool hit, bool isFightBack)
		{
			bool flag = !this._affected || defender != base.CombatChar;
			if (!flag)
			{
				this._affected = false;
				base.ShowSpecialEffectTips(0);
			}
		}

		// Token: 0x06003D32 RID: 15666 RVA: 0x00250B60 File Offset: 0x0024ED60
		private void OnAttackSkillAttackEnd(CombatContext context, sbyte hitType, bool hit, int index)
		{
			bool flag = !this._affected || context.Defender != base.CombatChar;
			if (!flag)
			{
				this._affected = false;
				base.ShowSpecialEffectTips(0);
			}
		}

		// Token: 0x06003D33 RID: 15667 RVA: 0x00250BA0 File Offset: 0x0024EDA0
		private void OnBounceInjury(DataContext context, int attackerId, int defenderId, bool isAlly, sbyte bodyPart, sbyte outerMarkCount, sbyte innerMarkCount)
		{
			bool flag = attackerId != base.CharacterId || !base.CanAffect;
			if (!flag)
			{
				int addValue = (int)(5 * (outerMarkCount + innerMarkCount));
				bool flag2 = addValue > 0;
				if (flag2)
				{
					base.CombatChar.ChangeNeiliAllocation(context, base.IsDirect ? 2 : 0, addValue, true, true);
					base.ShowSpecialEffectTips(1);
				}
			}
		}

		// Token: 0x06003D34 RID: 15668 RVA: 0x00250C00 File Offset: 0x0024EE00
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

		// Token: 0x04001200 RID: 4608
		private const sbyte ReduceDamagePercent = -30;

		// Token: 0x04001201 RID: 4609
		private const sbyte AddNeiliAllocationUnit = 5;

		// Token: 0x04001202 RID: 4610
		private bool _affected;
	}
}
