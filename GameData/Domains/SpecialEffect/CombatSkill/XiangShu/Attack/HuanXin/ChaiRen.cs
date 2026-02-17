using System;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Attack.HuanXin
{
	// Token: 0x02000312 RID: 786
	public class ChaiRen : CombatSkillEffectBase
	{
		// Token: 0x060033FC RID: 13308 RVA: 0x00227518 File Offset: 0x00225718
		public ChaiRen()
		{
		}

		// Token: 0x060033FD RID: 13309 RVA: 0x00227522 File Offset: 0x00225722
		public ChaiRen(CombatSkillKey skillKey) : base(skillKey, 17103, -1)
		{
		}

		// Token: 0x060033FE RID: 13310 RVA: 0x00227534 File Offset: 0x00225734
		public override void OnEnable(DataContext context)
		{
			CombatCharacter enemyChar = DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly, false);
			DomainManager.Combat.AddCombatState(context, base.CombatChar, 1, (short)(94 + context.Random.Next(0, 10)));
			DomainManager.Combat.AddCombatState(context, enemyChar, 2, (short)(104 + context.Random.Next(0, 10)));
			base.ShowSpecialEffectTips(0);
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x060033FF RID: 13311 RVA: 0x002275BB File Offset: 0x002257BB
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06003400 RID: 13312 RVA: 0x002275D0 File Offset: 0x002257D0
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				bool flag2 = base.PowerMatchAffectRequire((int)power, 0);
				if (flag2)
				{
					CombatCharacter enemyChar = DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly, false);
					foreach (short banableSkillId in enemyChar.GetBanableSkillIds(1, -1))
					{
						DomainManager.Combat.SilenceSkill(context, enemyChar, banableSkillId, 600, 100);
					}
					base.ShowSpecialEffectTips(1);
				}
				base.RemoveSelf(context);
			}
		}

		// Token: 0x04000F5F RID: 3935
		private const short SilenceFrame = 600;
	}
}
