using System;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Attack.HuanXin
{
	// Token: 0x02000314 RID: 788
	public class DuoShen : CombatSkillEffectBase
	{
		// Token: 0x06003406 RID: 13318 RVA: 0x002277D5 File Offset: 0x002259D5
		public DuoShen()
		{
		}

		// Token: 0x06003407 RID: 13319 RVA: 0x002277DF File Offset: 0x002259DF
		public DuoShen(CombatSkillKey skillKey) : base(skillKey, 17105, -1)
		{
		}

		// Token: 0x06003408 RID: 13320 RVA: 0x002277F0 File Offset: 0x002259F0
		public override void OnEnable(DataContext context)
		{
			CombatCharacter enemyChar = DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly, false);
			DomainManager.Combat.AddCombatState(context, base.CombatChar, 1, 114);
			DomainManager.Combat.AddCombatState(context, enemyChar, 2, 115);
			base.ShowSpecialEffectTips(0);
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06003409 RID: 13321 RVA: 0x00227857 File Offset: 0x00225A57
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x0600340A RID: 13322 RVA: 0x0022786C File Offset: 0x00225A6C
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				bool flag2 = base.PowerMatchAffectRequire((int)power, 0);
				if (flag2)
				{
					CombatCharacter enemyChar = DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly, false);
					foreach (short banableSkillId in enemyChar.GetBanableSkillIds(2, -1))
					{
						DomainManager.Combat.SilenceSkill(context, enemyChar, banableSkillId, 600, 100);
					}
					base.ShowSpecialEffectTips(1);
				}
				base.RemoveSelf(context);
			}
		}

		// Token: 0x04000F61 RID: 3937
		private const short SilenceFrame = 600;
	}
}
