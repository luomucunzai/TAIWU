using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.Character;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Attack;

namespace GameData.Domains.SpecialEffect.CombatSkill.Shixiangmen.Polearm
{
	// Token: 0x020003EF RID: 1007
	public class WuHuDuanHunQiang : PowerUpOnCast
	{
		// Token: 0x170001FB RID: 507
		// (get) Token: 0x06003856 RID: 14422 RVA: 0x0023A1E1 File Offset: 0x002383E1
		protected override EDataModifyType ModifyType
		{
			get
			{
				return EDataModifyType.AddPercent;
			}
		}

		// Token: 0x06003857 RID: 14423 RVA: 0x0023A1E4 File Offset: 0x002383E4
		public WuHuDuanHunQiang()
		{
		}

		// Token: 0x06003858 RID: 14424 RVA: 0x0023A1EE File Offset: 0x002383EE
		public WuHuDuanHunQiang(CombatSkillKey skillKey) : base(skillKey, 6301)
		{
		}

		// Token: 0x06003859 RID: 14425 RVA: 0x0023A200 File Offset: 0x00238400
		public override void OnEnable(DataContext context)
		{
			CombatCharacter enemyChar = DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly, false);
			OuterAndInnerShorts selfRange = base.CombatChar.GetAttackRange();
			OuterAndInnerShorts enemyRange = enemyChar.GetAttackRange();
			this.PowerUpValue = Math.Max((int)((base.IsDirect ? (selfRange.Inner - enemyRange.Inner) : (enemyRange.Outer - selfRange.Outer)) / 10 * 20), 0);
			base.OnEnable(context);
		}

		// Token: 0x0400107C RID: 4220
		private const sbyte AddPowerUnit = 20;
	}
}
