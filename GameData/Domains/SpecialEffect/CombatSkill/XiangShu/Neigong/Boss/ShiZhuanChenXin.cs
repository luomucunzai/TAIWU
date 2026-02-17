using System;
using GameData.Common;
using GameData.Domains.Character;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Neigong.Boss
{
	// Token: 0x020002A4 RID: 676
	public class ShiZhuanChenXin : CombatSkillEffectBase
	{
		// Token: 0x060031BE RID: 12734 RVA: 0x0021BFDF File Offset: 0x0021A1DF
		public ShiZhuanChenXin()
		{
		}

		// Token: 0x060031BF RID: 12735 RVA: 0x0021BFE9 File Offset: 0x0021A1E9
		public ShiZhuanChenXin(CombatSkillKey skillKey) : base(skillKey, 16110, -1)
		{
		}

		// Token: 0x060031C0 RID: 12736 RVA: 0x0021BFFA File Offset: 0x0021A1FA
		public override void OnEnable(DataContext context)
		{
			CombatDomain.RegisterHandler_CombatCharAboutToFall(new CombatDomain.OnCombatCharAboutToFall(this.OnCharAboutToFall));
		}

		// Token: 0x060031C1 RID: 12737 RVA: 0x0021C00F File Offset: 0x0021A20F
		public override void OnDisable(DataContext context)
		{
			CombatDomain.UnRegisterHandler_CombatCharAboutToFall(new CombatDomain.OnCombatCharAboutToFall(this.OnCharAboutToFall));
		}

		// Token: 0x060031C2 RID: 12738 RVA: 0x0021C024 File Offset: 0x0021A224
		private unsafe void OnCharAboutToFall(DataContext context, CombatCharacter combatChar, int eventIndex)
		{
			bool flag = combatChar != base.CombatChar || eventIndex != 3 || base.CombatChar.GetBossPhase() >= 5 || !DomainManager.Combat.IsCharacterFallen(base.CombatChar);
			if (!flag)
			{
				DomainManager.Combat.Reset(context, base.CombatChar);
				DomainManager.Combat.AddBossPhase(context, base.CombatChar, -1);
				NeiliAllocation originNeiliAllocation = base.CombatChar.GetOriginNeiliAllocation();
				for (byte type = 0; type < 4; type += 1)
				{
					base.CombatChar.ChangeNeiliAllocation(context, type, (int)(*(ref originNeiliAllocation.Items.FixedElementField + (IntPtr)type * 2) * 50 / 100), true, true);
				}
				base.ShowSpecialEffectTips(0);
			}
		}

		// Token: 0x04000EBF RID: 3775
		private const sbyte AddNeiliAllocationPercent = 50;
	}
}
