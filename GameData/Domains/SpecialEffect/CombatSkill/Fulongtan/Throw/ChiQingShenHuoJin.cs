using System;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Fulongtan.Throw
{
	// Token: 0x0200050B RID: 1291
	public class ChiQingShenHuoJin : CombatSkillEffectBase
	{
		// Token: 0x06003EC3 RID: 16067 RVA: 0x002570FD File Offset: 0x002552FD
		public ChiQingShenHuoJin()
		{
		}

		// Token: 0x06003EC4 RID: 16068 RVA: 0x00257107 File Offset: 0x00255307
		public ChiQingShenHuoJin(CombatSkillKey skillKey) : base(skillKey, 14303, -1)
		{
		}

		// Token: 0x06003EC5 RID: 16069 RVA: 0x00257118 File Offset: 0x00255318
		public override void OnEnable(DataContext context)
		{
			Events.RegisterHandler_PrepareSkillBegin(new Events.OnPrepareSkillBegin(this.OnPrepareSkillBegin));
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06003EC6 RID: 16070 RVA: 0x0025713F File Offset: 0x0025533F
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_PrepareSkillBegin(new Events.OnPrepareSkillBegin(this.OnPrepareSkillBegin));
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06003EC7 RID: 16071 RVA: 0x00257168 File Offset: 0x00255368
		private void OnPrepareSkillBegin(DataContext context, int charId, bool isAlly, short skillId)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				DomainManager.Combat.AddCombatState(context, base.CombatChar, 1, base.IsDirect ? 69 : 71, 250);
				DomainManager.Combat.AddCombatState(context, base.CombatChar, 2, base.IsDirect ? 70 : 72, 250);
				base.ShowSpecialEffectTips(0);
				base.ShowSpecialEffectTips(1);
			}
		}

		// Token: 0x06003EC8 RID: 16072 RVA: 0x002571F4 File Offset: 0x002553F4
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				base.RemoveSelf(context);
			}
		}

		// Token: 0x04001286 RID: 4742
		private const int StatePower = 250;
	}
}
