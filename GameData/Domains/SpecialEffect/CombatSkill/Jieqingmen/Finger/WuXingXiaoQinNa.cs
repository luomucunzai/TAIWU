using System;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Jieqingmen.Finger
{
	// Token: 0x020004F7 RID: 1271
	public class WuXingXiaoQinNa : CombatSkillEffectBase
	{
		// Token: 0x06003E4D RID: 15949 RVA: 0x0025551C File Offset: 0x0025371C
		public WuXingXiaoQinNa()
		{
		}

		// Token: 0x06003E4E RID: 15950 RVA: 0x00255526 File Offset: 0x00253726
		public WuXingXiaoQinNa(CombatSkillKey skillKey) : base(skillKey, 13100, -1)
		{
		}

		// Token: 0x06003E4F RID: 15951 RVA: 0x00255537 File Offset: 0x00253737
		public override void OnEnable(DataContext context)
		{
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06003E50 RID: 15952 RVA: 0x0025554C File Offset: 0x0025374C
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06003E51 RID: 15953 RVA: 0x00255564 File Offset: 0x00253764
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				bool flag2 = power > 0;
				if (flag2)
				{
					bool isDirect = base.IsDirect;
					if (isDirect)
					{
						base.ChangeStanceValue(context, base.CurrEnemyChar, -12000 * (int)power / 1000);
					}
					else
					{
						base.ChangeBreathValue(context, base.CurrEnemyChar, -90000 * (int)power / 1000);
					}
					base.ShowSpecialEffectTips(0);
				}
				base.RemoveSelf(context);
			}
		}

		// Token: 0x04001262 RID: 4706
		private const sbyte ReduceBreathStanceUnit = 3;
	}
}
