using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Emeipai.Finger
{
	// Token: 0x0200055B RID: 1371
	public class GuangXiangZhi : CombatSkillEffectBase
	{
		// Token: 0x06004086 RID: 16518 RVA: 0x0025E820 File Offset: 0x0025CA20
		public GuangXiangZhi()
		{
		}

		// Token: 0x06004087 RID: 16519 RVA: 0x0025E82A File Offset: 0x0025CA2A
		public GuangXiangZhi(CombatSkillKey skillKey) : base(skillKey, 2204, -1)
		{
		}

		// Token: 0x06004088 RID: 16520 RVA: 0x0025E83C File Offset: 0x0025CA3C
		public override void OnEnable(DataContext context)
		{
			this._makedDamage.Outer = 0;
			this._makedDamage.Inner = 0;
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 145, base.SkillTemplateId, -1, -1, -1), EDataModifyType.Add);
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 146, base.SkillTemplateId, -1, -1, -1), EDataModifyType.Add);
			Events.RegisterHandler_AddDirectDamageValue(new Events.OnAddDirectDamageValue(this.OnAddDirectDamageValue));
			Events.RegisterHandler_AttackSkillAttackEnd(new Events.OnAttackSkillAttackEnd(this.OnAttackSkillAttackEnd));
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06004089 RID: 16521 RVA: 0x0025E8EF File Offset: 0x0025CAEF
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_AddDirectDamageValue(new Events.OnAddDirectDamageValue(this.OnAddDirectDamageValue));
			Events.UnRegisterHandler_AttackSkillAttackEnd(new Events.OnAttackSkillAttackEnd(this.OnAttackSkillAttackEnd));
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x0600408A RID: 16522 RVA: 0x0025E928 File Offset: 0x0025CB28
		private void OnAddDirectDamageValue(DataContext context, int attackerId, int defenderId, sbyte bodyPart, bool isInner, int damageValue, short combatSkillId)
		{
			bool flag = attackerId != base.CharacterId || combatSkillId != base.SkillTemplateId;
			if (!flag)
			{
				if (isInner)
				{
					this._makedDamage.Inner = damageValue;
				}
				else
				{
					this._makedDamage.Outer = damageValue;
				}
			}
		}

		// Token: 0x0600408B RID: 16523 RVA: 0x0025E978 File Offset: 0x0025CB78
		private void OnAttackSkillAttackEnd(CombatContext context, sbyte hitType, bool hit, int index)
		{
			bool flag = index != 3 || context.SkillKey != this.SkillKey || !base.CombatCharPowerMatchAffectRequire(0);
			if (!flag)
			{
				CombatCharacter enemyChar = DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly, false);
				DomainManager.Combat.ChangeDistance(context, enemyChar, base.IsDirect ? 40 : -40, true);
				base.ShowSpecialEffectTips(0);
				DomainManager.Combat.SetDisplayPosition(context, !base.CombatChar.IsAlly, base.IsDirect ? int.MinValue : DomainManager.Combat.GetDisplayPosition(!base.CombatChar.IsAlly, DomainManager.Combat.GetCurrentDistance()));
				bool flag2 = !base.IsDirect;
				if (flag2)
				{
					base.CombatChar.SetCurrentPosition(base.CombatChar.GetDisplayPosition(), context);
					enemyChar.SetCurrentPosition(enemyChar.GetDisplayPosition(), context);
				}
			}
		}

		// Token: 0x0600408C RID: 16524 RVA: 0x0025EA88 File Offset: 0x0025CC88
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				bool flag2 = base.PowerMatchAffectRequire((int)power, 0);
				if (flag2)
				{
					bool flag3 = (this._makedDamage.Outer > 0 || this._makedDamage.Inner > 0) && (base.IsDirect ? (DomainManager.Combat.GetCurrentDistance() <= 80) : (DomainManager.Combat.GetCurrentDistance() >= 40));
					if (flag3)
					{
						DomainManager.Combat.AddInjuryDamageValue(base.CombatChar, base.CurrEnemyChar, base.CombatChar.SkillAttackBodyPart, this._makedDamage.Outer, this._makedDamage.Inner, base.SkillTemplateId, true);
						base.ShowSpecialEffectTips(1);
					}
				}
				this._makedDamage.Outer = 0;
				this._makedDamage.Inner = 0;
			}
		}

		// Token: 0x0600408D RID: 16525 RVA: 0x0025EB7C File Offset: 0x0025CD7C
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
					result = 10000;
				}
				else
				{
					result = 0;
				}
			}
			return result;
		}

		// Token: 0x040012F3 RID: 4851
		private const sbyte ChangeDistance = 40;

		// Token: 0x040012F4 RID: 4852
		private const sbyte DirectRequireDistance = 80;

		// Token: 0x040012F5 RID: 4853
		private const sbyte ReverseRequireDistance = 40;

		// Token: 0x040012F6 RID: 4854
		private OuterAndInnerInts _makedDamage;
	}
}
