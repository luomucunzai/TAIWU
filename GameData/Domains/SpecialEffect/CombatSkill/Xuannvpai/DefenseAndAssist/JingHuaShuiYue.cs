using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Defense;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.Xuannvpai.DefenseAndAssist
{
	// Token: 0x02000281 RID: 641
	public class JingHuaShuiYue : DefenseSkillBase
	{
		// Token: 0x060030EE RID: 12526 RVA: 0x002191EA File Offset: 0x002173EA
		public JingHuaShuiYue()
		{
		}

		// Token: 0x060030EF RID: 12527 RVA: 0x002191F4 File Offset: 0x002173F4
		public JingHuaShuiYue(CombatSkillKey skillKey) : base(skillKey, 8505)
		{
		}

		// Token: 0x060030F0 RID: 12528 RVA: 0x00219204 File Offset: 0x00217404
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 107, -1, -1, -1, -1), EDataModifyType.Custom);
		}

		// Token: 0x060030F1 RID: 12529 RVA: 0x00219238 File Offset: 0x00217438
		public override int GetModifiedValue(AffectedDataKey dataKey, int dataValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || !base.CanAffect || dataValue < 0;
			int result;
			if (flag)
			{
				result = dataValue;
			}
			else
			{
				CombatCharacter enemyChar = DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly, false);
				bool isDirect = base.IsDirect;
				int avoidOdds;
				if (isDirect)
				{
					avoidOdds = (int)(10 * enemyChar.GetTrickCount(20));
				}
				else
				{
					List<SilenceFrameData> markList = enemyChar.GetMindMarkTime().MarkList;
					avoidOdds = 5 * ((markList != null) ? markList.Count : 0);
				}
				bool flag2 = DomainManager.Combat.Context.Random.CheckPercentProb(avoidOdds);
				if (flag2)
				{
					dataValue = 0;
					base.ShowSpecialEffectTips(0);
				}
				result = dataValue;
			}
			return result;
		}

		// Token: 0x04000E84 RID: 3716
		private const sbyte AvoidOddsPerTrick = 10;

		// Token: 0x04000E85 RID: 3717
		private const sbyte AvoidOddsPerMark = 5;
	}
}
