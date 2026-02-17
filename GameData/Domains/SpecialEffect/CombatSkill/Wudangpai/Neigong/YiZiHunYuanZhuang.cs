using System;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Wudangpai.Neigong
{
	// Token: 0x020003CC RID: 972
	public class YiZiHunYuanZhuang : CombatSkillEffectBase
	{
		// Token: 0x0600377D RID: 14205 RVA: 0x00235AEA File Offset: 0x00233CEA
		public YiZiHunYuanZhuang()
		{
		}

		// Token: 0x0600377E RID: 14206 RVA: 0x00235AF4 File Offset: 0x00233CF4
		public YiZiHunYuanZhuang(CombatSkillKey skillKey) : base(skillKey, 4002, -1)
		{
		}

		// Token: 0x0600377F RID: 14207 RVA: 0x00235B05 File Offset: 0x00233D05
		public override void OnEnable(DataContext context)
		{
			Events.RegisterHandler_CostBreathAndStance(new Events.OnCostBreathAndStance(this.OnCostBreathAndStance));
		}

		// Token: 0x06003780 RID: 14208 RVA: 0x00235B1A File Offset: 0x00233D1A
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CostBreathAndStance(new Events.OnCostBreathAndStance(this.OnCostBreathAndStance));
		}

		// Token: 0x06003781 RID: 14209 RVA: 0x00235B30 File Offset: 0x00233D30
		private void OnCostBreathAndStance(DataContext context, int charId, bool isAlly, int costBreath, int costStance, short skillId)
		{
			bool flag = base.CharacterId != charId || (base.IsDirect ? costStance : costBreath) > 0;
			if (!flag)
			{
				bool flag2 = base.IsDirect && costBreath > 0 && base.CombatChar.GetStanceValue() < base.CombatChar.GetMaxStanceValue();
				if (flag2)
				{
					int percent = costBreath * 100 / base.CombatChar.GetMaxBreathValue();
					base.ChangeStanceValue(context, base.CombatChar, base.CombatChar.GetMaxStanceValue() * percent / 100);
					base.ShowSpecialEffectTips(0);
				}
				else
				{
					bool flag3 = !base.IsDirect && costStance > 0 && base.CombatChar.GetBreathValue() < base.CombatChar.GetMaxBreathValue();
					if (flag3)
					{
						int percent2 = costStance * 100 / base.CombatChar.GetMaxStanceValue();
						base.ChangeBreathValue(context, base.CombatChar, base.CombatChar.GetMaxBreathValue() * percent2 / 100);
						base.ShowSpecialEffectTips(0);
					}
				}
			}
		}
	}
}
