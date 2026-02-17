using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill;

namespace GameData.Domains.SpecialEffect.LegendaryBook.Neigong
{
	// Token: 0x02000151 RID: 337
	public class DaCheng : CombatSkillEffectBase
	{
		// Token: 0x06002ADF RID: 10975 RVA: 0x00204208 File Offset: 0x00202408
		public DaCheng()
		{
			this.IsLegendaryBookEffect = true;
		}

		// Token: 0x06002AE0 RID: 10976 RVA: 0x00204219 File Offset: 0x00202419
		public DaCheng(CombatSkillKey skillKey) : base(skillKey, 40004, -1)
		{
			this.IsLegendaryBookEffect = true;
		}

		// Token: 0x06002AE1 RID: 10977 RVA: 0x00204234 File Offset: 0x00202434
		public override void OnEnable(DataContext context)
		{
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 213, base.SkillTemplateId, -1, -1, -1), EDataModifyType.Add);
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 214, base.SkillTemplateId, -1, -1, -1), EDataModifyType.Add);
		}

		// Token: 0x06002AE2 RID: 10978 RVA: 0x0020429C File Offset: 0x0020249C
		public override void OnDataAdded(DataContext context)
		{
			bool flag = DomainManager.Global.GetLoadedAllArchiveData() && DomainManager.Taiwu.GetTaiwu().IsCombatSkillEquipped(base.SkillTemplateId);
			if (flag)
			{
				DomainManager.Taiwu.GetTaiwu().UpdateAllocatedGenericGrids(context);
			}
		}

		// Token: 0x06002AE3 RID: 10979 RVA: 0x002042E4 File Offset: 0x002024E4
		public override void OnDisable(DataContext context)
		{
			bool flag = DomainManager.Taiwu.GetTaiwu().IsCombatSkillEquipped(base.SkillTemplateId);
			if (flag)
			{
				DomainManager.Taiwu.GetTaiwu().UpdateAllocatedGenericGrids(context);
			}
		}

		// Token: 0x06002AE4 RID: 10980 RVA: 0x0020431C File Offset: 0x0020251C
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
					result = 1;
				}
				else
				{
					bool flag3 = dataKey.FieldId == 214;
					if (flag3)
					{
						result = -3;
					}
					else
					{
						result = 0;
					}
				}
			}
			return result;
		}

		// Token: 0x04000D26 RID: 3366
		private const sbyte ChangeSpecificGrid = 1;

		// Token: 0x04000D27 RID: 3367
		private const sbyte ChangeGenericGrid = -3;
	}
}
