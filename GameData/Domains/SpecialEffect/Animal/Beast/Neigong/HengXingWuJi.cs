using System;
using System.Collections.Generic;
using Config;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.Character;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.Animal.Beast.Neigong
{
	// Token: 0x0200061B RID: 1563
	public class HengXingWuJi : AnimalEffectBase
	{
		// Token: 0x170002E9 RID: 745
		// (get) Token: 0x0600459E RID: 17822 RVA: 0x00272F5A File Offset: 0x0027115A
		private int BouncePower
		{
			get
			{
				return base.IsElite ? 160 : 80;
			}
		}

		// Token: 0x0600459F RID: 17823 RVA: 0x00272F6D File Offset: 0x0027116D
		public HengXingWuJi()
		{
		}

		// Token: 0x060045A0 RID: 17824 RVA: 0x00272F77 File Offset: 0x00271177
		public HengXingWuJi(CombatSkillKey skillKey) : base(skillKey)
		{
		}

		// Token: 0x060045A1 RID: 17825 RVA: 0x00272F84 File Offset: 0x00271184
		public override void OnEnable(DataContext context)
		{
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 111, -1, -1, -1, -1), EDataModifyType.Add);
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 177, -1, -1, -1, -1), EDataModifyType.Custom);
		}

		// Token: 0x060045A2 RID: 17826 RVA: 0x00272FDC File Offset: 0x002711DC
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || dataKey.FieldId != 111;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				bool flag2 = !DomainManager.Combat.InAttackRange(base.CombatChar);
				if (flag2)
				{
					result = 0;
				}
				else
				{
					base.ShowSpecialEffectTipsOnceInFrame(0);
					result = this.BouncePower;
				}
			}
			return result;
		}

		// Token: 0x060045A3 RID: 17827 RVA: 0x00273040 File Offset: 0x00271240
		public override OuterAndInnerInts GetModifiedValue(AffectedDataKey dataKey, OuterAndInnerInts dataValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || dataKey.FieldId != 177;
			OuterAndInnerInts result;
			if (flag)
			{
				result = dataValue;
			}
			else
			{
				CombatConfigItem combatConfig = DomainManager.Combat.CombatConfig;
				result = new OuterAndInnerInts((int)combatConfig.MinDistance, (int)combatConfig.MaxDistance);
			}
			return result;
		}
	}
}
