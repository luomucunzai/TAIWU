using System;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Shixiangmen.Blade
{
	// Token: 0x0200040A RID: 1034
	public class ShiXiangDiTangDao : CombatSkillEffectBase
	{
		// Token: 0x060038EE RID: 14574 RVA: 0x0023C8D3 File Offset: 0x0023AAD3
		public ShiXiangDiTangDao()
		{
		}

		// Token: 0x060038EF RID: 14575 RVA: 0x0023C8DD File Offset: 0x0023AADD
		public ShiXiangDiTangDao(CombatSkillKey skillKey) : base(skillKey, 6200, -1)
		{
		}

		// Token: 0x060038F0 RID: 14576 RVA: 0x0023C8F0 File Offset: 0x0023AAF0
		public override void OnEnable(DataContext context)
		{
			base.ChangeMobilityValue(context, base.CombatChar, MoveSpecialConstants.MaxMobility * 30 / 100);
			DomainManager.Combat.AddMoveDistInSkillPrepare(base.CombatChar, 20, base.IsDirect);
			base.ShowSpecialEffectTips(0);
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x060038F1 RID: 14577 RVA: 0x0023C94A File Offset: 0x0023AB4A
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x060038F2 RID: 14578 RVA: 0x0023C960 File Offset: 0x0023AB60
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				base.RemoveSelf(context);
			}
		}

		// Token: 0x040010A6 RID: 4262
		private const sbyte AddMobilityPercent = 30;

		// Token: 0x040010A7 RID: 4263
		private const sbyte MoveDistInCast = 20;
	}
}
