using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Attack.ShuFang
{
	// Token: 0x020002E6 RID: 742
	public class SilenceSkillAndMinAttribute : CombatSkillEffectBase
	{
		// Token: 0x0600332E RID: 13102 RVA: 0x002234D1 File Offset: 0x002216D1
		protected SilenceSkillAndMinAttribute()
		{
		}

		// Token: 0x0600332F RID: 13103 RVA: 0x002234DB File Offset: 0x002216DB
		protected SilenceSkillAndMinAttribute(CombatSkillKey skillKey, int type) : base(skillKey, type, -1)
		{
		}

		// Token: 0x06003330 RID: 13104 RVA: 0x002234E8 File Offset: 0x002216E8
		public override void OnEnable(DataContext context)
		{
			base.CreateAffectedAllEnemyData(18, EDataModifyType.Custom, -1);
			base.ShowSpecialEffectTips(2);
			CombatCharacter enemyChar = base.CurrEnemyChar;
			bool flag = enemyChar.GetAffectingMoveSkillId() >= 0;
			if (flag)
			{
				base.ClearAffectingAgileSkill(context, enemyChar);
				base.ShowSpecialEffectTipsOnceInFrame(0);
			}
			bool flag2 = enemyChar.GetAffectingDefendSkillId() >= 0;
			if (flag2)
			{
				DomainManager.Combat.ClearAffectingDefenseSkill(context, enemyChar);
				base.ShowSpecialEffectTipsOnceInFrame(0);
			}
			foreach (short skillId in enemyChar.GetRandomUnrepeatedBanableSkillIds(context.Random, (int)this.SilenceSkillCount, null, -1, 1))
			{
				DomainManager.Combat.SilenceSkill(context, enemyChar, skillId, 2400, 100);
				base.ShowSpecialEffectTipsOnceInFrame(1);
			}
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06003331 RID: 13105 RVA: 0x002235D8 File Offset: 0x002217D8
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
			CombatCharacter enemyChar = DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly, false);
			short moveCd = enemyChar.GetMoveCd();
			bool flag = moveCd < enemyChar.MoveData.MoveCd;
			if (flag)
			{
				enemyChar.MoveData.MoveCd = moveCd;
			}
		}

		// Token: 0x06003332 RID: 13106 RVA: 0x00223638 File Offset: 0x00221838
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				base.RemoveSelf(context);
			}
		}

		// Token: 0x06003333 RID: 13107 RVA: 0x00223670 File Offset: 0x00221870
		public override bool GetModifiedValue(AffectedDataKey dataKey, bool dataValue)
		{
			return true;
		}

		// Token: 0x04000F1E RID: 3870
		protected sbyte SilenceSkillCount;

		// Token: 0x04000F1F RID: 3871
		private const short SilenceFrame = 2400;
	}
}
