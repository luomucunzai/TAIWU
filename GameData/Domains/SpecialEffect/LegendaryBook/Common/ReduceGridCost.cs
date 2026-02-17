using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill;

namespace GameData.Domains.SpecialEffect.LegendaryBook.Common
{
	// Token: 0x02000173 RID: 371
	public class ReduceGridCost : CombatSkillEffectBase
	{
		// Token: 0x06002B4F RID: 11087 RVA: 0x002050F7 File Offset: 0x002032F7
		protected ReduceGridCost()
		{
			this.IsLegendaryBookEffect = true;
		}

		// Token: 0x06002B50 RID: 11088 RVA: 0x00205108 File Offset: 0x00203308
		protected ReduceGridCost(CombatSkillKey skillKey, int type) : base(skillKey, type, -1)
		{
			this.IsLegendaryBookEffect = true;
		}

		// Token: 0x06002B51 RID: 11089 RVA: 0x0020511C File Offset: 0x0020331C
		public override void OnEnable(DataContext context)
		{
			short skillTemplateId = base.SkillTemplateId;
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 211, skillTemplateId, -1, -1, -1), EDataModifyType.Custom);
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 202, skillTemplateId, -1, -1, -1), EDataModifyType.Add);
			DomainManager.Extra.RemoveCharacterMasteredCombatSkill(context, base.CharacterId, skillTemplateId);
		}

		// Token: 0x06002B52 RID: 11090 RVA: 0x00205194 File Offset: 0x00203394
		public override void OnDataAdded(DataContext context)
		{
			bool loadedAllArchiveData = DomainManager.Global.GetLoadedAllArchiveData();
			if (loadedAllArchiveData)
			{
				DomainManager.Taiwu.GetTaiwu().UpdateAllocatedGenericGrids(context);
			}
		}

		// Token: 0x06002B53 RID: 11091 RVA: 0x002051C4 File Offset: 0x002033C4
		public override void OnDisable(DataContext context)
		{
			bool flag = DomainManager.Taiwu.GetTaiwu().IsCombatSkillEquipped(base.SkillTemplateId);
			if (flag)
			{
				DomainManager.Taiwu.GetTaiwu().UpdateAllocatedGenericGrids(context);
			}
		}

		// Token: 0x06002B54 RID: 11092 RVA: 0x002051FC File Offset: 0x002033FC
		public override int GetModifiedValue(AffectedDataKey dataKey, int dataValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || dataKey.CombatSkillId != base.SkillTemplateId;
			int result;
			if (flag)
			{
				result = dataValue;
			}
			else
			{
				bool flag2 = dataKey.FieldId == 211;
				if (flag2)
				{
					result = 1;
				}
				else
				{
					result = base.GetModifiedValue(dataKey, dataValue);
				}
			}
			return result;
		}

		// Token: 0x06002B55 RID: 11093 RVA: 0x00205258 File Offset: 0x00203458
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || dataKey.CombatSkillId != base.SkillTemplateId;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				bool flag2 = dataKey.FieldId == 202;
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

		// Token: 0x04000D3E RID: 3390
		private const sbyte AddRequirementPercent = 50;
	}
}
