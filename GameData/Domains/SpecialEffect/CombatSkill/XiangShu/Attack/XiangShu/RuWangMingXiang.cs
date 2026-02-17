using System;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Attack.XiangShu
{
	// Token: 0x020002D6 RID: 726
	public class RuWangMingXiang : CombatSkillEffectBase
	{
		// Token: 0x060032C2 RID: 12994 RVA: 0x00220C65 File Offset: 0x0021EE65
		public RuWangMingXiang()
		{
		}

		// Token: 0x060032C3 RID: 12995 RVA: 0x00220C6F File Offset: 0x0021EE6F
		public RuWangMingXiang(CombatSkillKey skillKey) : base(skillKey, 17092, -1)
		{
		}

		// Token: 0x060032C4 RID: 12996 RVA: 0x00220C80 File Offset: 0x0021EE80
		public override void OnEnable(DataContext context)
		{
			Events.RegisterHandler_CombatStateMachineUpdateEnd(new Events.OnCombatStateMachineUpdateEnd(this.OnStateMachineUpdateEnd));
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x060032C5 RID: 12997 RVA: 0x00220CA7 File Offset: 0x0021EEA7
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CombatStateMachineUpdateEnd(new Events.OnCombatStateMachineUpdateEnd(this.OnStateMachineUpdateEnd));
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x060032C6 RID: 12998 RVA: 0x00220CD0 File Offset: 0x0021EED0
		private void OnStateMachineUpdateEnd(DataContext context, CombatCharacter combatChar)
		{
			bool flag = base.CombatChar != combatChar || DomainManager.Combat.Pause;
			if (!flag)
			{
				this._frameCounter++;
				bool flag2 = this._frameCounter < 60 || !DomainManager.Combat.InAttackRange(base.CombatChar);
				if (!flag2)
				{
					this._frameCounter = 0;
					CombatCharacter enemyChar = DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly, false);
					bool flag3 = enemyChar.GetCharacter().GetXiangshuInfection() < 200;
					if (flag3)
					{
						enemyChar.GetCharacter().ChangeXiangshuInfection(context, 1 + enemyChar.OriginXiangshuInfection * 10 / 100);
					}
					base.ShowSpecialEffectTips(0);
				}
			}
		}

		// Token: 0x060032C7 RID: 12999 RVA: 0x00220D8C File Offset: 0x0021EF8C
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				base.RemoveSelf(context);
			}
		}

		// Token: 0x04000F05 RID: 3845
		private const sbyte AffectFrameCount = 60;

		// Token: 0x04000F06 RID: 3846
		private int _frameCounter;
	}
}
