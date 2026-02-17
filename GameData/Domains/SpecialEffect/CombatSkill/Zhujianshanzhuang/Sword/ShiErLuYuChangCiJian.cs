using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Zhujianshanzhuang.AttackCommon;

namespace GameData.Domains.SpecialEffect.CombatSkill.Zhujianshanzhuang.Sword
{
	// Token: 0x020001B8 RID: 440
	public class ShiErLuYuChangCiJian : SwordUnlockEffectBase
	{
		// Token: 0x17000147 RID: 327
		// (get) Token: 0x06002C71 RID: 11377 RVA: 0x002077A0 File Offset: 0x002059A0
		private int SelfAddCastProgress
		{
			get
			{
				return base.IsDirectOrReverseEffectDoubling ? 80 : 40;
			}
		}

		// Token: 0x17000148 RID: 328
		// (get) Token: 0x06002C72 RID: 11378 RVA: 0x002077B0 File Offset: 0x002059B0
		protected override IEnumerable<sbyte> RequirePersonalityTypes
		{
			get
			{
				yield return 3;
				yield break;
			}
		}

		// Token: 0x17000149 RID: 329
		// (get) Token: 0x06002C73 RID: 11379 RVA: 0x002077CF File Offset: 0x002059CF
		protected override int RequirePersonalityValue
		{
			get
			{
				return 50;
			}
		}

		// Token: 0x06002C74 RID: 11380 RVA: 0x002077D3 File Offset: 0x002059D3
		public ShiErLuYuChangCiJian()
		{
		}

		// Token: 0x06002C75 RID: 11381 RVA: 0x002077DD File Offset: 0x002059DD
		public ShiErLuYuChangCiJian(CombatSkillKey skillKey) : base(skillKey, 9100)
		{
		}

		// Token: 0x06002C76 RID: 11382 RVA: 0x002077ED File Offset: 0x002059ED
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			Events.RegisterHandler_PrepareSkillBegin(new Events.OnPrepareSkillBegin(this.OnPrepareSkillBegin));
		}

		// Token: 0x06002C77 RID: 11383 RVA: 0x0020780A File Offset: 0x00205A0A
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_PrepareSkillBegin(new Events.OnPrepareSkillBegin(this.OnPrepareSkillBegin));
			base.OnDisable(context);
		}

		// Token: 0x06002C78 RID: 11384 RVA: 0x00207828 File Offset: 0x00205A28
		private void OnPrepareSkillBegin(DataContext context, int charId, bool isAlly, short skillId)
		{
			bool flag = charId != base.CharacterId || !CombatSkillTemplateHelper.IsAttack(skillId);
			if (!flag)
			{
				CValuePercent percent = 0;
				bool flag2 = skillId == base.SkillTemplateId && base.IsReverseOrUsingDirectWeapon;
				if (flag2)
				{
					percent += this.SelfAddCastProgress;
				}
				bool flag3 = base.EffectCount > 0;
				if (flag3)
				{
					base.ReduceEffectCount(1);
					percent += 40;
				}
				bool flag4 = percent <= 0;
				if (!flag4)
				{
					DomainManager.Combat.ChangeSkillPrepareProgress(base.CombatChar, base.CombatChar.SkillPrepareTotalProgress * percent);
					base.ShowSpecialEffectTips(base.IsDirect, 1, 0);
				}
			}
		}

		// Token: 0x04000D6B RID: 3435
		private const int EffectAddCastProgress = 40;
	}
}
