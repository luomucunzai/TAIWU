using System;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.Animal.Beast.Neigong
{
	// Token: 0x02000621 RID: 1569
	public class XuanTianYiShe : AnimalEffectBase
	{
		// Token: 0x060045C8 RID: 17864 RVA: 0x00273717 File Offset: 0x00271917
		public XuanTianYiShe()
		{
		}

		// Token: 0x060045C9 RID: 17865 RVA: 0x00273721 File Offset: 0x00271921
		public XuanTianYiShe(CombatSkillKey skillKey) : base(skillKey)
		{
		}

		// Token: 0x060045CA RID: 17866 RVA: 0x0027372C File Offset: 0x0027192C
		public override void OnEnable(DataContext context)
		{
			Events.RegisterHandler_CombatBegin(new Events.OnCombatBegin(this.OnCombatBegin));
		}

		// Token: 0x060045CB RID: 17867 RVA: 0x00273741 File Offset: 0x00271941
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CombatBegin(new Events.OnCombatBegin(this.OnCombatBegin));
		}

		// Token: 0x060045CC RID: 17868 RVA: 0x00273756 File Offset: 0x00271956
		private void OnCombatBegin(DataContext context)
		{
			base.CombatChar.OuterInjuryAutoHealSpeeds.Add(1);
			base.CombatChar.InnerInjuryAutoHealSpeeds.Add(1);
		}

		// Token: 0x04001497 RID: 5271
		private const sbyte AutoHealSpeed = 1;
	}
}
