using System;
using Config;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Attack.XiangShu
{
	// Token: 0x020002D7 RID: 727
	public class RuXinYuanXiang : CombatSkillEffectBase
	{
		// Token: 0x060032C8 RID: 13000 RVA: 0x00220DC1 File Offset: 0x0021EFC1
		public RuXinYuanXiang()
		{
		}

		// Token: 0x060032C9 RID: 13001 RVA: 0x00220DCB File Offset: 0x0021EFCB
		public RuXinYuanXiang(CombatSkillKey skillKey) : base(skillKey, 17090, -1)
		{
		}

		// Token: 0x060032CA RID: 13002 RVA: 0x00220DDC File Offset: 0x0021EFDC
		public override void OnEnable(DataContext context)
		{
			Events.RegisterHandler_PrepareSkillBegin(new Events.OnPrepareSkillBegin(this.OnPrepareSkillBegin));
		}

		// Token: 0x060032CB RID: 13003 RVA: 0x00220DF1 File Offset: 0x0021EFF1
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_PrepareSkillBegin(new Events.OnPrepareSkillBegin(this.OnPrepareSkillBegin));
		}

		// Token: 0x060032CC RID: 13004 RVA: 0x00220E08 File Offset: 0x0021F008
		private void OnPrepareSkillBegin(DataContext context, int charId, bool isAlly, short skillId)
		{
			bool flag = this.SkillKey.IsMatch(charId, skillId);
			if (flag)
			{
				base.AddMaxEffectCount(true);
			}
			else
			{
				bool flag2 = isAlly != base.CombatChar.IsAlly && base.EffectCount > 0 && Config.CombatSkill.Instance[skillId].EquipType == 1;
				if (flag2)
				{
					DomainManager.Combat.AddGoneMadInjury(context, DomainManager.Combat.GetElement_CombatCharacterDict(charId), skillId, 200);
					base.ReduceEffectCount(1);
					base.ShowSpecialEffectTips(0);
				}
			}
		}

		// Token: 0x04000F07 RID: 3847
		private const short AddGoneMadInjury = 200;
	}
}
