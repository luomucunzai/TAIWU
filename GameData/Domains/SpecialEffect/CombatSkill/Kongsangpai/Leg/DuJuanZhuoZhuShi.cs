using System;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Kongsangpai.Leg
{
	// Token: 0x02000486 RID: 1158
	public class DuJuanZhuoZhuShi : CombatSkillEffectBase
	{
		// Token: 0x06003BC0 RID: 15296 RVA: 0x0024996A File Offset: 0x00247B6A
		public DuJuanZhuoZhuShi()
		{
		}

		// Token: 0x06003BC1 RID: 15297 RVA: 0x00249974 File Offset: 0x00247B74
		public DuJuanZhuoZhuShi(CombatSkillKey skillKey) : base(skillKey, 10302, -1)
		{
		}

		// Token: 0x06003BC2 RID: 15298 RVA: 0x00249985 File Offset: 0x00247B85
		public override void OnEnable(DataContext context)
		{
			Events.RegisterHandler_AddDirectInjury(new Events.OnAddDirectInjury(this.OnAddDirectInjury));
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06003BC3 RID: 15299 RVA: 0x002499AC File Offset: 0x00247BAC
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_AddDirectInjury(new Events.OnAddDirectInjury(this.OnAddDirectInjury));
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06003BC4 RID: 15300 RVA: 0x002499D4 File Offset: 0x00247BD4
		private void OnAddDirectInjury(DataContext context, int attackerId, int defenderId, bool isAlly, sbyte bodyPart, sbyte outerMarkCount, sbyte innerMarkCount, short combatSkillId)
		{
			bool flag = attackerId != base.CharacterId || combatSkillId != base.SkillTemplateId;
			if (!flag)
			{
				int acupointCount = (int)(base.IsDirect ? outerMarkCount : innerMarkCount);
				bool flag2 = acupointCount > 0;
				if (flag2)
				{
					DomainManager.Combat.AddAcupoint(context, base.CurrEnemyChar, 0, this.SkillKey, bodyPart, acupointCount, true);
					base.ShowSpecialEffectTips(0);
				}
			}
		}

		// Token: 0x06003BC5 RID: 15301 RVA: 0x00249A40 File Offset: 0x00247C40
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
