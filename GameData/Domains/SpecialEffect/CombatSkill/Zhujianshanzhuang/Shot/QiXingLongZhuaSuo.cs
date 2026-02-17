using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Zhujianshanzhuang.Shot
{
	// Token: 0x020001BD RID: 445
	public class QiXingLongZhuaSuo : CombatSkillEffectBase
	{
		// Token: 0x06002C98 RID: 11416 RVA: 0x00207FEB File Offset: 0x002061EB
		public QiXingLongZhuaSuo()
		{
		}

		// Token: 0x06002C99 RID: 11417 RVA: 0x00208004 File Offset: 0x00206204
		public QiXingLongZhuaSuo(CombatSkillKey skillKey) : base(skillKey, 9403, -1)
		{
		}

		// Token: 0x06002C9A RID: 11418 RVA: 0x00208024 File Offset: 0x00206224
		public override void OnEnable(DataContext context)
		{
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, base.IsDirect ? 145 : 146, base.SkillTemplateId, -1, -1, -1), EDataModifyType.Add);
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 69, base.SkillTemplateId, -1, -1, -1), EDataModifyType.AddPercent);
			Events.RegisterHandler_AttackSkillAttackEnd(new Events.OnAttackSkillAttackEnd(this.OnAttackSkillAttackEnd));
		}

		// Token: 0x06002C9B RID: 11419 RVA: 0x002080A7 File Offset: 0x002062A7
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_AttackSkillAttackEnd(new Events.OnAttackSkillAttackEnd(this.OnAttackSkillAttackEnd));
		}

		// Token: 0x06002C9C RID: 11420 RVA: 0x002080BC File Offset: 0x002062BC
		private void OnAttackSkillAttackEnd(CombatContext context, sbyte hitType, bool hit, int index)
		{
			bool flag = context.SkillKey != this.SkillKey || index != 2 || !base.CombatCharPowerMatchAffectRequire(0);
			if (!flag)
			{
				CombatCharacter enemyChar = DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly, false);
				short originDist = DomainManager.Combat.GetCurrentDistance();
				DomainManager.Combat.ChangeDistance(context, enemyChar, (int)(base.IsDirect ? this.ChangeDistance : (-(int)this.ChangeDistance)), true);
				base.ShowSpecialEffectTips(0);
				DomainManager.Combat.SetDisplayPosition(context, !base.CombatChar.IsAlly, base.IsDirect ? int.MinValue : DomainManager.Combat.GetDisplayPosition(!base.CombatChar.IsAlly, DomainManager.Combat.GetCurrentDistance()));
				bool flag2 = !base.IsDirect;
				if (flag2)
				{
					base.CombatChar.SetCurrentPosition(base.CombatChar.GetDisplayPosition(), context);
					enemyChar.SetCurrentPosition(enemyChar.GetDisplayPosition(), context);
				}
				bool flag3 = originDist != DomainManager.Combat.GetCurrentDistance() && base.CombatChar.GetTrickCount(12) > 0;
				if (flag3)
				{
					this._addDamage = (int)(base.IsDirect ? (120 - originDist) : (originDist - 20)) * this.AddDamageUnit / 10;
					DomainManager.Combat.RemoveTrick(context, base.CombatChar, 12, 1, true, -1);
					base.ShowSpecialEffectTips(1);
				}
			}
		}

		// Token: 0x06002C9D RID: 11421 RVA: 0x00208250 File Offset: 0x00206450
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || dataKey.CombatSkillId != base.SkillTemplateId;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				bool flag2 = dataKey.FieldId == 145 || dataKey.FieldId == 146;
				if (flag2)
				{
					result = 30;
				}
				else
				{
					bool flag3 = dataKey.FieldId == 69;
					if (flag3)
					{
						result = this._addDamage;
					}
					else
					{
						result = 0;
					}
				}
			}
			return result;
		}

		// Token: 0x04000D70 RID: 3440
		private const sbyte AddAttackRange = 30;

		// Token: 0x04000D71 RID: 3441
		private sbyte ChangeDistance = 60;

		// Token: 0x04000D72 RID: 3442
		private int AddDamageUnit = 5;

		// Token: 0x04000D73 RID: 3443
		private int _addDamage;
	}
}
