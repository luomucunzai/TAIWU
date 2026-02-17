using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Attack.ShuFang
{
	// Token: 0x020002E3 RID: 739
	public class KunLunShiErJinXianLing : CombatSkillEffectBase
	{
		// Token: 0x06003317 RID: 13079 RVA: 0x00222E8D File Offset: 0x0022108D
		public KunLunShiErJinXianLing()
		{
		}

		// Token: 0x06003318 RID: 13080 RVA: 0x00222E97 File Offset: 0x00221097
		public KunLunShiErJinXianLing(CombatSkillKey skillKey) : base(skillKey, 17080, -1)
		{
		}

		// Token: 0x06003319 RID: 13081 RVA: 0x00222EA8 File Offset: 0x002210A8
		public override void OnEnable(DataContext context)
		{
			base.AddMaxEffectCount(true);
			base.ShowSpecialEffectTips(0);
			base.CreateAffectedData(326, EDataModifyType.Custom, -1);
			Events.RegisterHandler_PrepareSkillBegin(new Events.OnPrepareSkillBegin(this.OnPrepareSkillBegin));
			bool flag = base.CombatChar.BossConfig != null;
			if (flag)
			{
				Events.RegisterHandler_AddMindDamage(new Events.OnAddMindDamage(this.OnAddMindDamageValue));
			}
			else
			{
				Events.RegisterHandler_AddDirectInjury(new Events.OnAddDirectInjury(this.OnAddDirectInjury));
			}
			Events.RegisterHandler_SkillEffectChange(new Events.OnSkillEffectChange(this.OnSkillEffectChange));
		}

		// Token: 0x0600331A RID: 13082 RVA: 0x00222F30 File Offset: 0x00221130
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_PrepareSkillBegin(new Events.OnPrepareSkillBegin(this.OnPrepareSkillBegin));
			bool flag = base.CombatChar.BossConfig != null;
			if (flag)
			{
				Events.UnRegisterHandler_AddMindDamage(new Events.OnAddMindDamage(this.OnAddMindDamageValue));
			}
			else
			{
				Events.UnRegisterHandler_AddDirectInjury(new Events.OnAddDirectInjury(this.OnAddDirectInjury));
			}
			Events.UnRegisterHandler_SkillEffectChange(new Events.OnSkillEffectChange(this.OnSkillEffectChange));
		}

		// Token: 0x0600331B RID: 13083 RVA: 0x00222F9C File Offset: 0x0022119C
		private void OnPrepareSkillBegin(DataContext context, int charId, bool isAlly, short skillId)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				bool flag2 = !this.IsSrcSkillPerformed;
				if (flag2)
				{
					this.IsSrcSkillPerformed = true;
				}
				else
				{
					base.RemoveSelf(context);
				}
			}
		}

		// Token: 0x0600331C RID: 13084 RVA: 0x00222FE8 File Offset: 0x002211E8
		private void OnAddMindDamageValue(DataContext context, int attackerId, int defenderId, int damageValue, int markCount, short combatSkillId)
		{
			bool flag = defenderId != base.CharacterId || markCount < 3;
			if (!flag)
			{
				base.ReduceEffectCount(1);
			}
		}

		// Token: 0x0600331D RID: 13085 RVA: 0x00223018 File Offset: 0x00221218
		private void OnAddDirectInjury(DataContext context, int attackerId, int defenderId, bool isAlly, sbyte bodyPart, sbyte outerMarkCount, sbyte innerMarkCount, short combatSkillId)
		{
			bool flag = defenderId != base.CharacterId || (outerMarkCount < 3 && innerMarkCount < 3);
			if (!flag)
			{
				base.ReduceEffectCount(1);
			}
		}

		// Token: 0x0600331E RID: 13086 RVA: 0x00223050 File Offset: 0x00221250
		private void OnSkillEffectChange(DataContext context, int charId, SkillEffectKey key, short oldCount, short newCount, bool removed)
		{
			bool flag = removed && this.IsSrcSkillPerformed && charId == base.CharacterId && key.SkillId == base.SkillTemplateId && key.IsDirect == base.IsDirect;
			if (flag)
			{
				base.RemoveSelf(context);
			}
		}

		// Token: 0x0600331F RID: 13087 RVA: 0x002230A0 File Offset: 0x002212A0
		public override bool GetModifiedValue(AffectedDataKey dataKey, bool dataValue)
		{
			bool flag = dataKey.CharId == base.CharacterId && dataKey.FieldId == 326 && dataKey.CustomParam2 == 1;
			return !flag && base.GetModifiedValue(dataKey, dataValue);
		}

		// Token: 0x04000F1A RID: 3866
		private const sbyte ReduceEffectRequireMarkCount = 3;
	}
}
