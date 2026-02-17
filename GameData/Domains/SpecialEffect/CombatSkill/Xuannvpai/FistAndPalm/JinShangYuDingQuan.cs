using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Attack;

namespace GameData.Domains.SpecialEffect.CombatSkill.Xuannvpai.FistAndPalm
{
	// Token: 0x02000273 RID: 627
	public class JinShangYuDingQuan : ChangePoisonLevel
	{
		// Token: 0x170001C3 RID: 451
		// (get) Token: 0x0600309E RID: 12446 RVA: 0x00217E17 File Offset: 0x00216017
		protected override sbyte AffectPoisonType
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x0600309F RID: 12447 RVA: 0x00217E1A File Offset: 0x0021601A
		public JinShangYuDingQuan()
		{
		}

		// Token: 0x060030A0 RID: 12448 RVA: 0x00217E24 File Offset: 0x00216024
		public JinShangYuDingQuan(CombatSkillKey skillKey) : base(skillKey, 8104)
		{
		}

		// Token: 0x060030A1 RID: 12449 RVA: 0x00217E34 File Offset: 0x00216034
		protected override void OnAffecting(DataContext context)
		{
			int charId = this.AffectingSkillKey.CharId;
			base.ShowSpecialEffectTips(1);
			bool flag = !this.AffectDatas.ContainsKey(new AffectedDataKey(charId, 194, -1, -1, -1, -1));
			if (flag)
			{
				base.AppendAffectedData(context, charId, 194, EDataModifyType.TotalPercent, -1);
			}
			else
			{
				DomainManager.SpecialEffect.InvalidateCache(context, charId, 194);
			}
		}

		// Token: 0x060030A2 RID: 12450 RVA: 0x00217E9C File Offset: 0x0021609C
		protected override int AffectingGetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.FieldId == 194;
			if (flag)
			{
				bool flag2 = base.IsDirect && dataKey.CharId == base.CharacterId;
				if (flag2)
				{
					return 50;
				}
				bool flag3 = !base.IsDirect && dataKey.CharId == base.CurrEnemyChar.GetId();
				if (flag3)
				{
					return -50;
				}
			}
			return 0;
		}

		// Token: 0x04000E72 RID: 3698
		private const sbyte ReduceSkillPrepareSpeedPercent = 50;
	}
}
