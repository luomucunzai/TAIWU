using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Attack.ShuFang
{
	// Token: 0x020002E4 RID: 740
	public class KunLunShiErXieXianLing : CombatSkillEffectBase
	{
		// Token: 0x06003320 RID: 13088 RVA: 0x002230E9 File Offset: 0x002212E9
		public KunLunShiErXieXianLing()
		{
		}

		// Token: 0x06003321 RID: 13089 RVA: 0x002230F3 File Offset: 0x002212F3
		public KunLunShiErXieXianLing(CombatSkillKey skillKey) : base(skillKey, 17083, -1)
		{
		}

		// Token: 0x06003322 RID: 13090 RVA: 0x00223104 File Offset: 0x00221304
		public override void OnEnable(DataContext context)
		{
			base.AddMaxEffectCount(true);
			base.ShowSpecialEffectTips(0);
			base.CreateAffectedData(327, EDataModifyType.Custom, -1);
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

		// Token: 0x06003323 RID: 13091 RVA: 0x0022318C File Offset: 0x0022138C
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

		// Token: 0x06003324 RID: 13092 RVA: 0x002231F8 File Offset: 0x002213F8
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

		// Token: 0x06003325 RID: 13093 RVA: 0x00223244 File Offset: 0x00221444
		private void OnAddMindDamageValue(DataContext context, int attackerId, int defenderId, int damageValue, int markCount, short combatSkillId)
		{
			bool flag = attackerId != base.CharacterId || markCount < 3;
			if (!flag)
			{
				base.ReduceEffectCount(1);
			}
		}

		// Token: 0x06003326 RID: 13094 RVA: 0x00223274 File Offset: 0x00221474
		private void OnAddDirectInjury(DataContext context, int attackerId, int defenderId, bool isAlly, sbyte bodyPart, sbyte outerMarkCount, sbyte innerMarkCount, short combatSkillId)
		{
			bool flag = attackerId != base.CharacterId || (outerMarkCount < 3 && innerMarkCount < 3);
			if (!flag)
			{
				base.ReduceEffectCount(1);
			}
		}

		// Token: 0x06003327 RID: 13095 RVA: 0x002232AC File Offset: 0x002214AC
		private void OnSkillEffectChange(DataContext context, int charId, SkillEffectKey key, short oldCount, short newCount, bool removed)
		{
			bool flag = removed && this.IsSrcSkillPerformed && charId == base.CharacterId && key.SkillId == base.SkillTemplateId && key.IsDirect == base.IsDirect;
			if (flag)
			{
				base.RemoveSelf(context);
			}
		}

		// Token: 0x06003328 RID: 13096 RVA: 0x002232FC File Offset: 0x002214FC
		public override bool GetModifiedValue(AffectedDataKey dataKey, bool dataValue)
		{
			bool flag = dataKey.CharId == base.CharacterId && dataKey.FieldId == 327 && dataKey.CustomParam2 == 1;
			return !flag && base.GetModifiedValue(dataKey, dataValue);
		}

		// Token: 0x04000F1B RID: 3867
		private const sbyte ReduceEffectRequireMarkCount = 3;
	}
}
