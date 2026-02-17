using System;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Xuannvpai.Finger
{
	// Token: 0x02000279 RID: 633
	public class DaTaiYinYiMingZhi : CombatSkillEffectBase
	{
		// Token: 0x060030BC RID: 12476 RVA: 0x00218657 File Offset: 0x00216857
		public DaTaiYinYiMingZhi()
		{
		}

		// Token: 0x060030BD RID: 12477 RVA: 0x00218661 File Offset: 0x00216861
		public DaTaiYinYiMingZhi(CombatSkillKey skillKey) : base(skillKey, 8207, -1)
		{
		}

		// Token: 0x060030BE RID: 12478 RVA: 0x00218672 File Offset: 0x00216872
		public override void OnEnable(DataContext context)
		{
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x060030BF RID: 12479 RVA: 0x00218687 File Offset: 0x00216887
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x060030C0 RID: 12480 RVA: 0x0021869C File Offset: 0x0021689C
		private unsafe void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				bool flag2 = base.PowerMatchAffectRequire((int)power, 0);
				if (flag2)
				{
					NeiliAllocation neiliAllocation = base.CurrEnemyChar.GetNeiliAllocation();
					byte neiliAllocationType = base.IsDirect ? 2 : 0;
					base.CurrEnemyChar.ChangeNeiliAllocation(context, neiliAllocationType, (int)(-(int)(*neiliAllocation[(int)neiliAllocationType])), false, true);
					DomainManager.Combat.AddCombatState(context, base.CurrEnemyChar, 0, 41, 200);
					base.ShowSpecialEffectTips(0);
				}
				base.RemoveSelf(context);
			}
		}

		// Token: 0x04000E79 RID: 3705
		private const short CombatStatePower = 200;
	}
}
