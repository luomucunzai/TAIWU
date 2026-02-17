using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill;

namespace GameData.Domains.SpecialEffect.LegendaryBook.Neigong
{
	// Token: 0x02000152 RID: 338
	public class DaYing : CombatSkillEffectBase
	{
		// Token: 0x06002AE5 RID: 10981 RVA: 0x00204384 File Offset: 0x00202584
		public DaYing()
		{
			this.IsLegendaryBookEffect = true;
		}

		// Token: 0x06002AE6 RID: 10982 RVA: 0x00204395 File Offset: 0x00202595
		public DaYing(CombatSkillKey skillKey) : base(skillKey, 40003, -1)
		{
			this.IsLegendaryBookEffect = true;
		}

		// Token: 0x06002AE7 RID: 10983 RVA: 0x002043B0 File Offset: 0x002025B0
		public override void OnEnable(DataContext context)
		{
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 213, base.SkillTemplateId, -1, -1, -1), EDataModifyType.Add);
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 214, base.SkillTemplateId, -1, -1, -1), EDataModifyType.Add);
		}

		// Token: 0x06002AE8 RID: 10984 RVA: 0x00204418 File Offset: 0x00202618
		public override void OnDataAdded(DataContext context)
		{
			bool flag = DomainManager.Global.GetLoadedAllArchiveData() && DomainManager.Taiwu.GetTaiwu().IsCombatSkillEquipped(base.SkillTemplateId);
			if (flag)
			{
				DomainManager.Taiwu.GetTaiwu().UpdateAllocatedGenericGrids(context);
			}
		}

		// Token: 0x06002AE9 RID: 10985 RVA: 0x00204460 File Offset: 0x00202660
		public override void OnDisable(DataContext context)
		{
			bool flag = DomainManager.Taiwu.GetTaiwu().IsCombatSkillEquipped(base.SkillTemplateId);
			if (flag)
			{
				DomainManager.Taiwu.GetTaiwu().UpdateAllocatedGenericGrids(context);
			}
		}

		// Token: 0x06002AEA RID: 10986 RVA: 0x00204498 File Offset: 0x00202698
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
				bool flag2 = dataKey.FieldId == 213;
				if (flag2)
				{
					result = -1;
				}
				else
				{
					bool flag3 = dataKey.FieldId == 214;
					if (flag3)
					{
						result = 3;
					}
					else
					{
						result = 0;
					}
				}
			}
			return result;
		}

		// Token: 0x04000D28 RID: 3368
		private const sbyte ChangeSpecificGrid = -1;

		// Token: 0x04000D29 RID: 3369
		private const sbyte ChangeGenericGrid = 3;
	}
}
