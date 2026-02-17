using System;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.Animal.Beast.Neigong
{
	// Token: 0x0200061D RID: 1565
	public class LingXinHuiXing : AnimalEffectBase
	{
		// Token: 0x170002EB RID: 747
		// (get) Token: 0x060045AA RID: 17834 RVA: 0x002731C7 File Offset: 0x002713C7
		private int ReducePower
		{
			get
			{
				return base.IsElite ? -20 : -10;
			}
		}

		// Token: 0x060045AB RID: 17835 RVA: 0x002731D7 File Offset: 0x002713D7
		public LingXinHuiXing()
		{
		}

		// Token: 0x060045AC RID: 17836 RVA: 0x002731E1 File Offset: 0x002713E1
		public LingXinHuiXing(CombatSkillKey skillKey) : base(skillKey)
		{
		}

		// Token: 0x060045AD RID: 17837 RVA: 0x002731EC File Offset: 0x002713EC
		public override void OnEnable(DataContext context)
		{
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x060045AE RID: 17838 RVA: 0x00273201 File Offset: 0x00271401
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x060045AF RID: 17839 RVA: 0x00273218 File Offset: 0x00271418
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			CombatCharacter caster = DomainManager.Combat.GetElement_CombatCharacterDict(charId);
			bool flag = caster.IsAlly == base.CombatChar.IsAlly || !base.IsCurrent || interrupted || !CombatSkillTemplateHelper.IsAttack(skillId);
			if (!flag)
			{
				DomainManager.Combat.ReduceSkillPowerInCombat(context, new ValueTuple<int, short>(charId, skillId), base.EffectKey, this.ReducePower);
				base.ShowSpecialEffectTips(0);
			}
		}
	}
}
