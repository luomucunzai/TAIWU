using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Neigong.Boss
{
	// Token: 0x0200029F RID: 671
	public class QiHanLingQi : BossNeigongBase
	{
		// Token: 0x060031A1 RID: 12705 RVA: 0x0021B9FD File Offset: 0x00219BFD
		public QiHanLingQi()
		{
		}

		// Token: 0x060031A2 RID: 12706 RVA: 0x0021BA07 File Offset: 0x00219C07
		public QiHanLingQi(CombatSkillKey skillKey) : base(skillKey, 16102)
		{
		}

		// Token: 0x060031A3 RID: 12707 RVA: 0x0021BA17 File Offset: 0x00219C17
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			Events.RegisterHandler_DistanceChanged(new Events.OnDistanceChanged(this.OnDistanceChanged));
		}

		// Token: 0x060031A4 RID: 12708 RVA: 0x0021BA34 File Offset: 0x00219C34
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_DistanceChanged(new Events.OnDistanceChanged(this.OnDistanceChanged));
			base.OnDisable(context);
		}

		// Token: 0x060031A5 RID: 12709 RVA: 0x0021BA51 File Offset: 0x00219C51
		protected override void ActivePhase2Effect(DataContext context)
		{
			this._inAttackRange = DomainManager.Combat.InAttackRange(base.CombatChar);
			base.AppendAffectedAllEnemyData(context, 8, EDataModifyType.TotalPercent, -1);
			base.AppendAffectedAllEnemyData(context, 7, EDataModifyType.TotalPercent, -1);
		}

		// Token: 0x060031A6 RID: 12710 RVA: 0x0021BA80 File Offset: 0x00219C80
		private void OnDistanceChanged(DataContext context, CombatCharacter mover, short distance, bool isMove, bool isForced)
		{
			bool inAttackRange = DomainManager.Combat.InAttackRange(base.CombatChar);
			bool flag = inAttackRange == this._inAttackRange;
			if (!flag)
			{
				this._inAttackRange = inAttackRange;
				base.InvalidateAllEnemyCache(context, 8);
				base.InvalidateAllEnemyCache(context, 7);
			}
		}

		// Token: 0x060031A7 RID: 12711 RVA: 0x0021BAC8 File Offset: 0x00219CC8
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool inAttackRange = this._inAttackRange;
			bool flag = inAttackRange;
			if (flag)
			{
				ushort fieldId = dataKey.FieldId;
				bool flag2 = fieldId - 7 <= 1;
				flag = flag2;
			}
			bool flag3 = flag;
			int result;
			if (flag3)
			{
				result = -75;
			}
			else
			{
				result = 0;
			}
			return result;
		}

		// Token: 0x04000EB6 RID: 3766
		private const int ReduceRecoveryOfBreathAndStancePercent = -75;

		// Token: 0x04000EB7 RID: 3767
		private bool _inAttackRange;
	}
}
