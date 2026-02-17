using System;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Shixiangmen.FistAndPalm
{
	// Token: 0x020003F5 RID: 1013
	public class BaWangKaiGongShou : CombatSkillEffectBase
	{
		// Token: 0x06003873 RID: 14451 RVA: 0x0023A703 File Offset: 0x00238903
		public BaWangKaiGongShou()
		{
		}

		// Token: 0x06003874 RID: 14452 RVA: 0x0023A70D File Offset: 0x0023890D
		public BaWangKaiGongShou(CombatSkillKey skillKey) : base(skillKey, 6105, -1)
		{
		}

		// Token: 0x06003875 RID: 14453 RVA: 0x0023A71E File Offset: 0x0023891E
		public override void OnEnable(DataContext context)
		{
			Events.RegisterHandler_AddDirectDamageValue(new Events.OnAddDirectDamageValue(this.OnAddDirectDamageValue));
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06003876 RID: 14454 RVA: 0x0023A745 File Offset: 0x00238945
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_AddDirectDamageValue(new Events.OnAddDirectDamageValue(this.OnAddDirectDamageValue));
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06003877 RID: 14455 RVA: 0x0023A76C File Offset: 0x0023896C
		private void OnAddDirectDamageValue(DataContext context, int attackerId, int defenderId, sbyte bodyPart, bool isInner, int damageValue, short combatSkillId)
		{
			bool flag = attackerId != base.CharacterId || combatSkillId != base.SkillTemplateId || !base.CombatCharPowerMatchAffectRequire(0) || damageValue <= 0;
			if (!flag)
			{
				bool isDirect = base.IsDirect;
				sbyte otherSidePart;
				if (isDirect)
				{
					otherSidePart = ((bodyPart == 3) ? 4 : ((bodyPart == 4) ? 3 : -1));
				}
				else
				{
					otherSidePart = ((bodyPart == 5) ? 6 : ((bodyPart == 6) ? 5 : -1));
				}
				bool flag2 = otherSidePart < 0;
				if (!flag2)
				{
					DomainManager.Combat.AddInjuryDamageValue(base.CombatChar, base.CurrEnemyChar, otherSidePart, isInner ? 0 : damageValue, isInner ? damageValue : 0, combatSkillId, true);
					base.ShowSpecialEffectTips(0);
				}
			}
		}

		// Token: 0x06003878 RID: 14456 RVA: 0x0023A818 File Offset: 0x00238A18
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				base.RemoveSelf(context);
			}
		}
	}
}
