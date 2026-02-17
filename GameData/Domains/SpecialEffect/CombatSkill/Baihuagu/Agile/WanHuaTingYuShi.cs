using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Agile;

namespace GameData.Domains.SpecialEffect.CombatSkill.Baihuagu.Agile
{
	// Token: 0x020005E3 RID: 1507
	public class WanHuaTingYuShi : BuffHitOrDebuffAvoid
	{
		// Token: 0x170002BE RID: 702
		// (get) Token: 0x0600446A RID: 17514 RVA: 0x0026F765 File Offset: 0x0026D965
		protected override sbyte AffectHitType
		{
			get
			{
				return 3;
			}
		}

		// Token: 0x170002BF RID: 703
		// (get) Token: 0x0600446B RID: 17515 RVA: 0x0026F768 File Offset: 0x0026D968
		private int InfinityMindMarkProgressAddPercent
		{
			get
			{
				return base.IsDirect ? 100 : -50;
			}
		}

		// Token: 0x0600446C RID: 17516 RVA: 0x0026F778 File Offset: 0x0026D978
		public WanHuaTingYuShi()
		{
		}

		// Token: 0x0600446D RID: 17517 RVA: 0x0026F782 File Offset: 0x0026D982
		public WanHuaTingYuShi(CombatSkillKey skillKey) : base(skillKey, 3407)
		{
		}

		// Token: 0x0600446E RID: 17518 RVA: 0x0026F794 File Offset: 0x0026D994
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			bool isDirect = base.IsDirect;
			if (isDirect)
			{
				base.CreateAffectedAllEnemyData(305, EDataModifyType.TotalPercent, -1);
			}
			else
			{
				base.CreateAffectedData(305, EDataModifyType.TotalPercent, -1);
			}
			bool canAffect = base.CanAffect;
			if (canAffect)
			{
				base.ShowSpecialEffectTips(2);
			}
		}

		// Token: 0x0600446F RID: 17519 RVA: 0x0026F7E4 File Offset: 0x0026D9E4
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.FieldId == 305 && base.CanAffect;
			int result;
			if (flag)
			{
				result = this.InfinityMindMarkProgressAddPercent;
			}
			else
			{
				result = base.GetModifyValue(dataKey, currModifyValue);
			}
			return result;
		}
	}
}
