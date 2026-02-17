using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Common.Agile
{
	// Token: 0x020005B2 RID: 1458
	public abstract class BuffHitOrDebuffAvoid : AgileSkillBase
	{
		// Token: 0x170002AA RID: 682
		// (get) Token: 0x06004359 RID: 17241
		protected abstract sbyte AffectHitType { get; }

		// Token: 0x0600435A RID: 17242 RVA: 0x0026B06B File Offset: 0x0026926B
		protected BuffHitOrDebuffAvoid()
		{
		}

		// Token: 0x0600435B RID: 17243 RVA: 0x0026B075 File Offset: 0x00269275
		protected BuffHitOrDebuffAvoid(CombatSkillKey skillKey, int type) : base(skillKey, type)
		{
			this.ListenCanAffectChange = true;
		}

		// Token: 0x0600435C RID: 17244 RVA: 0x0026B088 File Offset: 0x00269288
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			bool isDirect = base.IsDirect;
			if (isDirect)
			{
				base.CreateAffectedData(36, EDataModifyType.Custom, -1);
				base.CreateAffectedData(37, EDataModifyType.Add, -1);
			}
			else
			{
				base.CreateAffectedAllEnemyData(36, EDataModifyType.Custom, -1);
				base.CreateAffectedAllEnemyData(37, EDataModifyType.Add, -1);
			}
			bool canAffect = base.CanAffect;
			if (canAffect)
			{
				base.ShowSpecialEffectTips(0);
			}
		}

		// Token: 0x0600435D RID: 17245 RVA: 0x0026B0EC File Offset: 0x002692EC
		protected override void OnMoveSkillCanAffectChanged(DataContext context, DataUid dataUid)
		{
			bool isDirect = base.IsDirect;
			if (isDirect)
			{
				base.InvalidateCache(context, 36);
			}
			else
			{
				base.InvalidateAllEnemyCache(context, 36);
			}
		}

		// Token: 0x0600435E RID: 17246 RVA: 0x0026B11C File Offset: 0x0026931C
		public override bool GetModifiedValue(AffectedDataKey dataKey, bool dataValue)
		{
			bool flag = dataKey.CustomParam0 != (int)this.AffectHitType || dataKey.CustomParam1 != (base.IsDirect ? 1 : 0) || !base.CanAffect;
			bool result;
			if (flag)
			{
				result = dataValue;
			}
			else
			{
				bool flag2 = dataKey.FieldId == 36;
				result = (!flag2 && dataValue);
			}
			return result;
		}

		// Token: 0x0600435F RID: 17247 RVA: 0x0026B178 File Offset: 0x00269378
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.CustomParam0 != (int)this.AffectHitType || dataKey.CustomParam1 != (base.IsDirect ? 0 : 1) || !base.CanAffect;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				bool flag2 = dataKey.FieldId == 37;
				if (flag2)
				{
					result = 50;
				}
				else
				{
					result = 0;
				}
			}
			return result;
		}

		// Token: 0x040013FE RID: 5118
		private const sbyte AddEffectPercent = 50;
	}
}
