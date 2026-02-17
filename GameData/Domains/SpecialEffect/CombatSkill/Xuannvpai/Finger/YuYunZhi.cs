using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Xuannvpai.Finger
{
	// Token: 0x0200027F RID: 639
	public class YuYunZhi : CombatSkillEffectBase
	{
		// Token: 0x060030E0 RID: 12512 RVA: 0x00218EB1 File Offset: 0x002170B1
		public YuYunZhi()
		{
		}

		// Token: 0x060030E1 RID: 12513 RVA: 0x00218EBB File Offset: 0x002170BB
		public YuYunZhi(CombatSkillKey skillKey) : base(skillKey, 8202, -1)
		{
		}

		// Token: 0x060030E2 RID: 12514 RVA: 0x00218ECC File Offset: 0x002170CC
		public override void OnEnable(DataContext context)
		{
			base.ShowSpecialEffectTips(0);
			base.CreateAffectedData(327, EDataModifyType.Custom, base.SkillTemplateId);
			Events.RegisterHandler_AddDirectDamageValue(new Events.OnAddDirectDamageValue(this.OnAddDirectDamageValue));
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x060030E3 RID: 12515 RVA: 0x00218F19 File Offset: 0x00217119
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_AddDirectDamageValue(new Events.OnAddDirectDamageValue(this.OnAddDirectDamageValue));
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x060030E4 RID: 12516 RVA: 0x00218F40 File Offset: 0x00217140
		private void OnAddDirectDamageValue(DataContext context, int attackerId, int defenderId, sbyte bodyPart, bool isInner, int damageValue, short combatSkillId)
		{
			bool flag = attackerId != base.CharacterId || combatSkillId != base.SkillTemplateId || damageValue <= 0 || base.IsDirect == isInner;
			if (!flag)
			{
				bool flag2 = DomainManager.Combat.GetElement_CombatCharacterDict(attackerId).GetDefeatMarkCollection().GetTotalCount() > DomainManager.Combat.GetElement_CombatCharacterDict(defenderId).GetDefeatMarkCollection().GetTotalCount();
				if (flag2)
				{
					base.ShowSpecialEffectTips(1);
				}
				else
				{
					DomainManager.Combat.AddInjuryDamageValue(base.CombatChar, base.CombatChar, bodyPart, isInner ? 0 : damageValue, isInner ? damageValue : 0, combatSkillId, true);
				}
			}
		}

		// Token: 0x060030E5 RID: 12517 RVA: 0x00218FE4 File Offset: 0x002171E4
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				base.RemoveSelf(context);
			}
		}

		// Token: 0x060030E6 RID: 12518 RVA: 0x0021901C File Offset: 0x0021721C
		public override bool GetModifiedValue(AffectedDataKey dataKey, bool dataValue)
		{
			bool flag = dataKey.SkillKey == this.SkillKey && dataKey.FieldId == 327 && dataKey.CustomParam0 == (base.IsDirect ? 0 : 1) && dataKey.CustomParam2 == 1;
			return !flag && base.GetModifiedValue(dataKey, dataValue);
		}
	}
}
