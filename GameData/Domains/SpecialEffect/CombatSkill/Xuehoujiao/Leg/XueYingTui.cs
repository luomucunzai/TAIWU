using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Attack;

namespace GameData.Domains.SpecialEffect.CombatSkill.Xuehoujiao.Leg
{
	// Token: 0x0200022E RID: 558
	public class XueYingTui : BuffByNeiliAllocation
	{
		// Token: 0x06002F74 RID: 12148 RVA: 0x0021328F File Offset: 0x0021148F
		public XueYingTui()
		{
		}

		// Token: 0x06002F75 RID: 12149 RVA: 0x00213299 File Offset: 0x00211499
		public XueYingTui(CombatSkillKey skillKey) : base(skillKey, 15307)
		{
			this.RequireNeiliAllocationType = 0;
		}

		// Token: 0x06002F76 RID: 12150 RVA: 0x002132B0 File Offset: 0x002114B0
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 204, base.SkillTemplateId, -1, -1, -1), EDataModifyType.TotalPercent);
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 207, base.SkillTemplateId, -1, -1, -1), EDataModifyType.TotalPercent);
		}

		// Token: 0x06002F77 RID: 12151 RVA: 0x00213314 File Offset: 0x00211514
		protected override void OnInvalidCache(DataContext context)
		{
			DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 204);
			DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 207);
			DomainManager.Combat.UpdateSkillCanUse(context, base.CombatChar, base.SkillTemplateId);
		}

		// Token: 0x06002F78 RID: 12152 RVA: 0x00213368 File Offset: 0x00211568
		protected override int GetAffectedModifyValue(AffectedDataKey dataKey)
		{
			bool flag = dataKey.CharId != base.CharacterId || dataKey.CombatSkillId != base.SkillTemplateId;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				ushort fieldId = dataKey.FieldId;
				bool flag2 = fieldId == 204 || fieldId == 207;
				bool flag3 = flag2;
				if (flag3)
				{
					result = -75;
				}
				else
				{
					result = 0;
				}
			}
			return result;
		}

		// Token: 0x04000E15 RID: 3605
		private const sbyte ReduceCostPercent = 75;
	}
}
