using System;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Attack.DaYueYaoChang
{
	// Token: 0x0200031B RID: 795
	public class ReduceResources : CombatSkillEffectBase
	{
		// Token: 0x06003427 RID: 13351 RVA: 0x00227F54 File Offset: 0x00226154
		protected ReduceResources()
		{
		}

		// Token: 0x06003428 RID: 13352 RVA: 0x00227F5E File Offset: 0x0022615E
		protected ReduceResources(CombatSkillKey skillKey, int type) : base(skillKey, type, -1)
		{
		}

		// Token: 0x06003429 RID: 13353 RVA: 0x00227F6B File Offset: 0x0022616B
		public override void OnEnable(DataContext context)
		{
			Events.RegisterHandler_CombatStateMachineUpdateEnd(new Events.OnCombatStateMachineUpdateEnd(this.OnStateMachineUpdateEnd));
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x0600342A RID: 13354 RVA: 0x00227F92 File Offset: 0x00226192
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CombatStateMachineUpdateEnd(new Events.OnCombatStateMachineUpdateEnd(this.OnStateMachineUpdateEnd));
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x0600342B RID: 13355 RVA: 0x00227FBC File Offset: 0x002261BC
		private void OnStateMachineUpdateEnd(DataContext context, CombatCharacter combatChar)
		{
			bool flag = base.CombatChar != combatChar || DomainManager.Combat.Pause;
			if (!flag)
			{
				this._frameCounter++;
				bool flag2 = this._frameCounter < (int)this.AffectFrameCount || !DomainManager.Combat.InAttackRange(base.CombatChar);
				if (!flag2)
				{
					this._frameCounter = 0;
					CombatCharacter enemyChar = DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly, false);
					base.ChangeBreathValue(context, enemyChar, -6000);
					base.ChangeStanceValue(context, enemyChar, -800);
					base.ChangeMobilityValue(context, enemyChar, -MoveSpecialConstants.MaxMobility * 20 / 100);
					base.ShowSpecialEffectTips(0);
				}
			}
		}

		// Token: 0x0600342C RID: 13356 RVA: 0x0022807C File Offset: 0x0022627C
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				base.RemoveSelf(context);
			}
		}

		// Token: 0x04000F6A RID: 3946
		private const sbyte ReducePercent = 20;

		// Token: 0x04000F6B RID: 3947
		protected short AffectFrameCount;

		// Token: 0x04000F6C RID: 3948
		private int _frameCounter;
	}
}
