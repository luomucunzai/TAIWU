using System;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Wudangpai.Whip
{
	// Token: 0x020003B5 RID: 949
	public class FuChenGong : CombatSkillEffectBase
	{
		// Token: 0x06003704 RID: 14084 RVA: 0x002334BE File Offset: 0x002316BE
		public FuChenGong()
		{
		}

		// Token: 0x06003705 RID: 14085 RVA: 0x002334C8 File Offset: 0x002316C8
		public FuChenGong(CombatSkillKey skillKey) : base(skillKey, 4300, -1)
		{
		}

		// Token: 0x06003706 RID: 14086 RVA: 0x002334D9 File Offset: 0x002316D9
		public override void OnEnable(DataContext context)
		{
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06003707 RID: 14087 RVA: 0x002334EE File Offset: 0x002316EE
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06003708 RID: 14088 RVA: 0x00233504 File Offset: 0x00231704
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				bool flag2 = power > 0;
				if (flag2)
				{
					DomainManager.Combat.ChangeDisorderOfQiRandomRecovery(context, base.IsDirect ? base.CombatChar : base.CurrEnemyChar, (int)(40 * power / 10 * (base.IsDirect ? -1 : 1)), false);
					base.ShowSpecialEffectTips(0);
				}
				base.RemoveSelf(context);
			}
		}

		// Token: 0x0400100D RID: 4109
		private const sbyte ChangeQiDisorderUnit = 40;
	}
}
