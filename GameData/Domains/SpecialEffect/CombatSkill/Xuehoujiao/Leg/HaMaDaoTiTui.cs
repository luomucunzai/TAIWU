using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Xuehoujiao.Leg
{
	// Token: 0x0200022A RID: 554
	public class HaMaDaoTiTui : CombatSkillEffectBase
	{
		// Token: 0x06002F56 RID: 12118 RVA: 0x00212797 File Offset: 0x00210997
		public HaMaDaoTiTui()
		{
		}

		// Token: 0x06002F57 RID: 12119 RVA: 0x002127A1 File Offset: 0x002109A1
		public HaMaDaoTiTui(CombatSkillKey skillKey) : base(skillKey, 15301, -1)
		{
		}

		// Token: 0x06002F58 RID: 12120 RVA: 0x002127B4 File Offset: 0x002109B4
		public override void OnEnable(DataContext context)
		{
			this._reduceBreathStanceCount = 0;
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 204, base.SkillTemplateId, -1, -1, -1), EDataModifyType.AddPercent);
			Events.RegisterHandler_AttackSkillAttackEnd(new Events.OnAttackSkillAttackEnd(this.OnAttackSkillAttackEnd));
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06002F59 RID: 12121 RVA: 0x0021281E File Offset: 0x00210A1E
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_AttackSkillAttackEnd(new Events.OnAttackSkillAttackEnd(this.OnAttackSkillAttackEnd));
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06002F5A RID: 12122 RVA: 0x00212848 File Offset: 0x00210A48
		private void OnAttackSkillAttackEnd(CombatContext context, sbyte hitType, bool hit, int index)
		{
			bool flag = index != 3 || context.SkillKey != this.SkillKey || !base.CombatCharPowerMatchAffectRequire(0);
			if (!flag)
			{
				DomainManager.Combat.ChangeDistance(context, base.CombatChar, base.IsDirect ? 20 : -20);
				base.ShowSpecialEffectTips(0);
				DomainManager.Combat.SetDisplayPosition(context, base.CombatChar.IsAlly, base.IsDirect ? int.MinValue : DomainManager.Combat.GetDisplayPosition(base.CombatChar.IsAlly, DomainManager.Combat.GetCurrentDistance()));
				bool flag2 = !base.IsDirect;
				if (flag2)
				{
					CombatCharacter enemyChar = DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly, false);
					base.CombatChar.SetCurrentPosition(base.CombatChar.GetDisplayPosition(), context);
					enemyChar.SetCurrentPosition(enemyChar.GetDisplayPosition(), context);
				}
				bool flag3 = this._reduceBreathStanceCount < 3;
				if (flag3)
				{
					this._reduceBreathStanceCount++;
					DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 204);
				}
			}
		}

		// Token: 0x06002F5B RID: 12123 RVA: 0x00212990 File Offset: 0x00210B90
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || interrupted;
			if (!flag)
			{
				bool flag2 = skillId != base.SkillTemplateId;
				if (flag2)
				{
					this._reduceBreathStanceCount = 0;
					DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 204);
				}
			}
		}

		// Token: 0x06002F5C RID: 12124 RVA: 0x002129E4 File Offset: 0x00210BE4
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
				bool flag2 = dataKey.FieldId == 204;
				if (flag2)
				{
					result = -30 * this._reduceBreathStanceCount;
				}
				else
				{
					result = 0;
				}
			}
			return result;
		}

		// Token: 0x04000E06 RID: 3590
		private const sbyte ChangeDistance = 20;

		// Token: 0x04000E07 RID: 3591
		private const sbyte BreathStanceReduceUnit = -30;

		// Token: 0x04000E08 RID: 3592
		private const sbyte BreathStanceReduceMaxCount = 3;

		// Token: 0x04000E09 RID: 3593
		private int _reduceBreathStanceCount;
	}
}
