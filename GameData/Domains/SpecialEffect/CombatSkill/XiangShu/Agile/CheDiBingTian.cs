using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Agile;

namespace GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Agile
{
	// Token: 0x02000335 RID: 821
	public class CheDiBingTian : AgileSkillBase
	{
		// Token: 0x06003491 RID: 13457 RVA: 0x00229136 File Offset: 0x00227336
		public CheDiBingTian()
		{
		}

		// Token: 0x06003492 RID: 13458 RVA: 0x00229140 File Offset: 0x00227340
		public CheDiBingTian(CombatSkillKey skillKey) : base(skillKey, 16202)
		{
		}

		// Token: 0x06003493 RID: 13459 RVA: 0x00229150 File Offset: 0x00227350
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			this._inAttackRange = DomainManager.Combat.InAttackRange(base.CombatChar);
			base.CreateAffectedAllEnemyData(9, EDataModifyType.TotalPercent, -1);
			base.CreateAffectedAllEnemyData(14, EDataModifyType.TotalPercent, -1);
			base.CreateAffectedAllEnemyData(11, EDataModifyType.TotalPercent, -1);
			Events.RegisterHandler_DistanceChanged(new Events.OnDistanceChanged(this.OnDistanceChanged));
		}

		// Token: 0x06003494 RID: 13460 RVA: 0x002291AF File Offset: 0x002273AF
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_DistanceChanged(new Events.OnDistanceChanged(this.OnDistanceChanged));
			base.OnDisable(context);
		}

		// Token: 0x06003495 RID: 13461 RVA: 0x002291CC File Offset: 0x002273CC
		private void OnDistanceChanged(DataContext context, CombatCharacter mover, short distance, bool isMove, bool isForced)
		{
			bool inAttackRange = DomainManager.Combat.InAttackRange(base.CombatChar);
			bool flag = inAttackRange == this._inAttackRange;
			if (!flag)
			{
				this._inAttackRange = inAttackRange;
				base.InvalidateAllEnemyCache(context, 9);
				base.InvalidateAllEnemyCache(context, 14);
				base.InvalidateAllEnemyCache(context, 11);
			}
		}

		// Token: 0x06003496 RID: 13462 RVA: 0x00229220 File Offset: 0x00227420
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = base.CanAffect && this._inAttackRange;
			bool flag2 = flag;
			if (flag2)
			{
				ushort fieldId = dataKey.FieldId;
				bool flag3 = fieldId == 9 || fieldId == 11 || fieldId == 14;
				flag2 = flag3;
			}
			bool flag4 = flag2;
			int result;
			if (flag4)
			{
				result = -75;
			}
			else
			{
				result = 0;
			}
			return result;
		}

		// Token: 0x04000F7E RID: 3966
		private const int ReduceSpeedPercent = -75;

		// Token: 0x04000F7F RID: 3967
		private bool _inAttackRange;
	}
}
