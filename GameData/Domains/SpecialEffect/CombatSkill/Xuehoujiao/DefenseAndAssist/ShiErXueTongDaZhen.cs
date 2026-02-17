using System;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Assist;

namespace GameData.Domains.SpecialEffect.CombatSkill.Xuehoujiao.DefenseAndAssist
{
	// Token: 0x02000242 RID: 578
	public class ShiErXueTongDaZhen : AssistSkillBase
	{
		// Token: 0x06002FC6 RID: 12230 RVA: 0x002145A7 File Offset: 0x002127A7
		public ShiErXueTongDaZhen()
		{
		}

		// Token: 0x06002FC7 RID: 12231 RVA: 0x002145B1 File Offset: 0x002127B1
		public ShiErXueTongDaZhen(CombatSkillKey skillKey) : base(skillKey, 15806)
		{
			this.SetConstAffectingOnCombatBegin = DomainManager.Combat.IsMainCharacter(base.CombatChar);
		}

		// Token: 0x06002FC8 RID: 12232 RVA: 0x002145D8 File Offset: 0x002127D8
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			bool flag = DomainManager.Combat.IsMainCharacter(base.CombatChar);
			if (flag)
			{
				base.CombatChar.TransferInjuryCommandIsInner = !base.IsDirect;
				base.CombatChar.SetShowTransferInjuryCommand(true, context);
			}
			else
			{
				Events.RegisterHandler_CombatBegin(new Events.OnCombatBegin(this.OnCombatBegin));
			}
		}

		// Token: 0x06002FC9 RID: 12233 RVA: 0x0021463C File Offset: 0x0021283C
		public override void OnDisable(DataContext context)
		{
			base.OnDisable(context);
		}

		// Token: 0x06002FCA RID: 12234 RVA: 0x00214647 File Offset: 0x00212847
		private void OnCombatBegin(DataContext context)
		{
			base.RemoveSelf(context);
			Events.UnRegisterHandler_CombatBegin(new Events.OnCombatBegin(this.OnCombatBegin));
		}

		// Token: 0x06002FCB RID: 12235 RVA: 0x00214664 File Offset: 0x00212864
		protected override void OnCanUseChanged(DataContext context, DataUid dataUid)
		{
			base.CombatChar.SetShowTransferInjuryCommand(base.CanAffect, context);
			base.SetConstAffecting(context, base.CanAffect);
		}
	}
}
