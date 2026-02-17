using System;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.Animal.Loong.Neigong
{
	// Token: 0x02000603 RID: 1539
	public class Qiuniu : AnimalEffectBase
	{
		// Token: 0x06004524 RID: 17700 RVA: 0x00271A0B File Offset: 0x0026FC0B
		public Qiuniu()
		{
		}

		// Token: 0x06004525 RID: 17701 RVA: 0x00271A15 File Offset: 0x0026FC15
		public Qiuniu(CombatSkillKey skillKey) : base(skillKey)
		{
		}

		// Token: 0x06004526 RID: 17702 RVA: 0x00271A20 File Offset: 0x0026FC20
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			Events.RegisterHandler_CombatBegin(new Events.OnCombatBegin(this.OnCombatBegin));
		}

		// Token: 0x06004527 RID: 17703 RVA: 0x00271A3D File Offset: 0x0026FC3D
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CombatBegin(new Events.OnCombatBegin(this.OnCombatBegin));
			base.OnDisable(context);
		}

		// Token: 0x06004528 RID: 17704 RVA: 0x00271A5A File Offset: 0x0026FC5A
		private void OnCombatBegin(DataContext context)
		{
			base.ShowSpecialEffectTips(0);
		}
	}
}
