using System;
using GameData.Common;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Attack;

namespace GameData.Domains.SpecialEffect.CombatSkill.Jingangzong.FistAndPalm
{
	// Token: 0x020004BF RID: 1215
	public class JinGangHeiShaZhang : AddWeaponEquipAttackOnAttack
	{
		// Token: 0x06003D06 RID: 15622 RVA: 0x0024F9D6 File Offset: 0x0024DBD6
		public JinGangHeiShaZhang()
		{
		}

		// Token: 0x06003D07 RID: 15623 RVA: 0x0024F9E0 File Offset: 0x0024DBE0
		public JinGangHeiShaZhang(CombatSkillKey skillKey) : base(skillKey, 11103)
		{
		}

		// Token: 0x06003D08 RID: 15624 RVA: 0x0024F9F0 File Offset: 0x0024DBF0
		protected override void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				bool flag2 = power > 0;
				if (flag2)
				{
					DomainManager.Combat.AddCombatState(context, base.CombatChar, 1, base.IsDirect ? 55 : 56, (int)(power * 2));
					base.ShowSpecialEffectTips(1);
				}
				base.RemoveSelf(context);
			}
		}
	}
}
