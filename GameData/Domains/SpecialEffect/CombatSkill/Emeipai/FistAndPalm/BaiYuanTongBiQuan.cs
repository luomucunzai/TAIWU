using System;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Emeipai.FistAndPalm
{
	// Token: 0x0200054F RID: 1359
	public class BaiYuanTongBiQuan : CombatSkillEffectBase
	{
		// Token: 0x0600403B RID: 16443 RVA: 0x0025D2BD File Offset: 0x0025B4BD
		public BaiYuanTongBiQuan()
		{
		}

		// Token: 0x0600403C RID: 16444 RVA: 0x0025D2C7 File Offset: 0x0025B4C7
		public BaiYuanTongBiQuan(CombatSkillKey skillKey) : base(skillKey, 2103, -1)
		{
		}

		// Token: 0x0600403D RID: 16445 RVA: 0x0025D2D8 File Offset: 0x0025B4D8
		public override void OnEnable(DataContext context)
		{
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x0600403E RID: 16446 RVA: 0x0025D2ED File Offset: 0x0025B4ED
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x0600403F RID: 16447 RVA: 0x0025D304 File Offset: 0x0025B504
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				bool flag2 = power > 0;
				if (flag2)
				{
					CombatCharacter agileChar = base.IsDirect ? base.CombatChar : base.CurrEnemyChar;
					int changeValue = MoveSpecialConstants.MaxMobility * (int)power / 100 * (base.IsDirect ? 1 : -1);
					base.ChangeMobilityValue(context, agileChar, changeValue);
					base.ShowSpecialEffectTips(0);
				}
				base.RemoveSelf(context);
			}
		}
	}
}
