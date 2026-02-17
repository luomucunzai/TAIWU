using System;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.CombatSkill;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.Baihuagu.Finger
{
	// Token: 0x020005CF RID: 1487
	public class ManTianHuaYuShi : CombatSkillEffectBase
	{
		// Token: 0x06004402 RID: 17410 RVA: 0x0026DB5B File Offset: 0x0026BD5B
		public ManTianHuaYuShi()
		{
		}

		// Token: 0x06004403 RID: 17411 RVA: 0x0026DB65 File Offset: 0x0026BD65
		public ManTianHuaYuShi(CombatSkillKey skillKey) : base(skillKey, 3104, -1)
		{
		}

		// Token: 0x06004404 RID: 17412 RVA: 0x0026DB76 File Offset: 0x0026BD76
		public override void OnEnable(DataContext context)
		{
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06004405 RID: 17413 RVA: 0x0026DB8B File Offset: 0x0026BD8B
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06004406 RID: 17414 RVA: 0x0026DBA0 File Offset: 0x0026BDA0
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				int addCount = 0;
				int randomCount = (int)power / base.GetAffectRequirePower(0);
				for (int i = 0; i < randomCount; i++)
				{
					bool flag2 = context.Random.CheckPercentProb(30);
					if (flag2)
					{
						addCount++;
					}
				}
				bool flag3 = addCount > 0;
				if (flag3)
				{
					for (int j = 0; j < addCount; j++)
					{
						bool isDirect = base.IsDirect;
						if (isDirect)
						{
							DomainManager.Combat.AddAcupoint(context, base.CurrEnemyChar, 1, this.SkillKey, -1, 1, true);
						}
						else
						{
							DomainManager.Combat.AddFlaw(context, base.CurrEnemyChar, 1, this.SkillKey, -1, 1, true);
						}
					}
					base.ShowSpecialEffectTips(0);
				}
				base.RemoveSelf(context);
			}
		}

		// Token: 0x04001431 RID: 5169
		private const int FlawOrAcupointLevel = 1;

		// Token: 0x04001432 RID: 5170
		private const sbyte AffectOdds = 30;
	}
}
