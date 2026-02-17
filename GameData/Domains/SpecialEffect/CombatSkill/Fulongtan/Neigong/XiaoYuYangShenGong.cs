using System;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Fulongtan.Neigong
{
	// Token: 0x02000518 RID: 1304
	public class XiaoYuYangShenGong : CombatSkillEffectBase
	{
		// Token: 0x06003EF5 RID: 16117 RVA: 0x002579EC File Offset: 0x00255BEC
		public XiaoYuYangShenGong()
		{
		}

		// Token: 0x06003EF6 RID: 16118 RVA: 0x002579F6 File Offset: 0x00255BF6
		public XiaoYuYangShenGong(CombatSkillKey skillKey) : base(skillKey, 14003, -1)
		{
		}

		// Token: 0x06003EF7 RID: 16119 RVA: 0x00257A07 File Offset: 0x00255C07
		public override void OnEnable(DataContext context)
		{
			Events.RegisterHandler_AttackSkillAttackHit(new Events.OnAttackSkillAttackHit(this.OnAttackSkillAttackHit));
		}

		// Token: 0x06003EF8 RID: 16120 RVA: 0x00257A1C File Offset: 0x00255C1C
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_AttackSkillAttackHit(new Events.OnAttackSkillAttackHit(this.OnAttackSkillAttackHit));
		}

		// Token: 0x06003EF9 RID: 16121 RVA: 0x00257A34 File Offset: 0x00255C34
		private void OnAttackSkillAttackHit(DataContext context, CombatCharacter attacker, CombatCharacter defender, short skillId, int index, bool critical)
		{
			bool flag = !critical || base.CharacterId != (base.IsDirect ? attacker.GetId() : defender.GetId());
			if (!flag)
			{
				base.CombatChar.RemoveRandomFlawOrAcupoint(context, true, 1);
				base.CombatChar.RemoveRandomFlawOrAcupoint(context, false, 1);
				DomainManager.Combat.RemoveMindDefeatMark(context, base.CombatChar, 1, true, 0);
				base.ShowSpecialEffectTips(0);
			}
		}
	}
}
