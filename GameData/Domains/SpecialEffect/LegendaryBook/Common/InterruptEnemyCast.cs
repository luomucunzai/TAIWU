using System;
using System.Collections.Generic;
using Config;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill;

namespace GameData.Domains.SpecialEffect.LegendaryBook.Common
{
	// Token: 0x02000172 RID: 370
	public class InterruptEnemyCast : CombatSkillEffectBase
	{
		// Token: 0x06002B49 RID: 11081 RVA: 0x00204F6C File Offset: 0x0020316C
		protected InterruptEnemyCast()
		{
			this.IsLegendaryBookEffect = true;
		}

		// Token: 0x06002B4A RID: 11082 RVA: 0x00204F7D File Offset: 0x0020317D
		protected InterruptEnemyCast(CombatSkillKey skillKey, int type) : base(skillKey, type, -1)
		{
			this.IsLegendaryBookEffect = true;
		}

		// Token: 0x06002B4B RID: 11083 RVA: 0x00204F94 File Offset: 0x00203194
		public override void OnEnable(DataContext context)
		{
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 212, base.SkillTemplateId, -1, -1, -1), EDataModifyType.AddPercent);
			Events.RegisterHandler_AttackSkillAttackEnd(new Events.OnAttackSkillAttackEnd(this.OnAttackSkillAttackEnd));
		}

		// Token: 0x06002B4C RID: 11084 RVA: 0x00204FE5 File Offset: 0x002031E5
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_AttackSkillAttackEnd(new Events.OnAttackSkillAttackEnd(this.OnAttackSkillAttackEnd));
		}

		// Token: 0x06002B4D RID: 11085 RVA: 0x00204FFC File Offset: 0x002031FC
		private void OnAttackSkillAttackEnd(CombatContext context, sbyte hitType, bool hit, int index)
		{
			bool flag = context.SkillKey != this.SkillKey || index != 3 || context.Attacker.GetAttackSkillPower() < 100 || base.CombatChar.GetAutoCastingSkill();
			if (!flag)
			{
				short enemyPreparingSkill = base.CurrEnemyChar.GetPreparingSkillId();
				bool flag2 = enemyPreparingSkill < 0 || Config.CombatSkill.Instance[enemyPreparingSkill].Type != Config.CombatSkill.Instance[base.SkillTemplateId].Type;
				if (!flag2)
				{
					DomainManager.Combat.InterruptSkill(context, base.CurrEnemyChar, 100);
				}
			}
		}

		// Token: 0x06002B4E RID: 11086 RVA: 0x002050A4 File Offset: 0x002032A4
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
				bool flag2 = dataKey.FieldId == 212;
				if (flag2)
				{
					result = 50;
				}
				else
				{
					result = 0;
				}
			}
			return result;
		}

		// Token: 0x04000D3D RID: 3389
		private const sbyte AddCastTimePercent = 50;
	}
}
