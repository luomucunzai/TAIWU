using System;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Common.Attack
{
	// Token: 0x02000589 RID: 1417
	public class AddOrReduceNeiliAllocation : CombatSkillEffectBase
	{
		// Token: 0x17000286 RID: 646
		// (get) Token: 0x060041F4 RID: 16884 RVA: 0x00264D58 File Offset: 0x00262F58
		protected virtual sbyte NeiliAllocationChange
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x060041F5 RID: 16885 RVA: 0x00264D5B File Offset: 0x00262F5B
		public AddOrReduceNeiliAllocation()
		{
		}

		// Token: 0x060041F6 RID: 16886 RVA: 0x00264D65 File Offset: 0x00262F65
		public AddOrReduceNeiliAllocation(CombatSkillKey skillKey, int type) : base(skillKey, type, -1)
		{
		}

		// Token: 0x060041F7 RID: 16887 RVA: 0x00264D72 File Offset: 0x00262F72
		public override void OnEnable(DataContext context)
		{
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x060041F8 RID: 16888 RVA: 0x00264D88 File Offset: 0x00262F88
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x060041F9 RID: 16889 RVA: 0x00264DA0 File Offset: 0x00262FA0
		protected virtual void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				bool flag2 = base.PowerMatchAffectRequire((int)power, 0);
				if (flag2)
				{
					CombatCharacter combatChar = base.IsDirect ? base.CombatChar : DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly, true);
					combatChar.ChangeNeiliAllocation(context, this.AffectNeiliAllocationType, (int)(base.IsDirect ? this.NeiliAllocationChange : (-(int)this.NeiliAllocationChange)), true, true);
					base.ShowSpecialEffectTips(0);
				}
				base.RemoveSelf(context);
			}
		}

		// Token: 0x04001378 RID: 4984
		protected byte AffectNeiliAllocationType;
	}
}
