using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.Character;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Neigong.Boss
{
	// Token: 0x0200029D RID: 669
	public class FangTianChiLing : BossNeigongBase
	{
		// Token: 0x06003195 RID: 12693 RVA: 0x0021B75E File Offset: 0x0021995E
		public FangTianChiLing()
		{
		}

		// Token: 0x06003196 RID: 12694 RVA: 0x0021B770 File Offset: 0x00219970
		public FangTianChiLing(CombatSkillKey skillKey) : base(skillKey, 16108)
		{
		}

		// Token: 0x06003197 RID: 12695 RVA: 0x0021B788 File Offset: 0x00219988
		protected override void ActivePhase2Effect(DataContext context)
		{
			base.AppendAffectedData(context, base.CharacterId, 111, EDataModifyType.Add, -1);
			base.AppendAffectedData(context, base.CharacterId, 177, EDataModifyType.Custom, -1);
		}

		// Token: 0x06003198 RID: 12696 RVA: 0x0021B7B4 File Offset: 0x002199B4
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.CharId != base.CharacterId;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				bool flag2 = dataKey.FieldId == 111;
				if (flag2)
				{
					result = (int)this.AddBouncePowerUnit * Math.Abs(dataKey.CustomParam1 - 50) * 2;
				}
				else
				{
					result = 0;
				}
			}
			return result;
		}

		// Token: 0x06003199 RID: 12697 RVA: 0x0021B808 File Offset: 0x00219A08
		public override OuterAndInnerInts GetModifiedValue(AffectedDataKey dataKey, OuterAndInnerInts dataValue)
		{
			bool flag = dataKey.CharId != base.CharacterId;
			OuterAndInnerInts result;
			if (flag)
			{
				result = dataValue;
			}
			else
			{
				bool flag2 = dataKey.FieldId == 177;
				if (flag2)
				{
					dataValue.Outer = (int)DomainManager.Combat.CombatConfig.MinDistance;
					dataValue.Inner = (int)DomainManager.Combat.CombatConfig.MaxDistance;
				}
				result = dataValue;
			}
			return result;
		}

		// Token: 0x04000EB4 RID: 3764
		private sbyte AddBouncePowerUnit = 10;
	}
}
