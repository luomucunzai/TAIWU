using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Agile;

namespace GameData.Domains.SpecialEffect.CombatSkill.Jieqingmen.Agile
{
	// Token: 0x02000502 RID: 1282
	public class BaiChiZhuang : AgileSkillBase
	{
		// Token: 0x06003E8B RID: 16011 RVA: 0x0025644C File Offset: 0x0025464C
		public BaiChiZhuang()
		{
		}

		// Token: 0x06003E8C RID: 16012 RVA: 0x00256456 File Offset: 0x00254656
		public BaiChiZhuang(CombatSkillKey skillKey) : base(skillKey, 13400)
		{
			this.ListenCanAffectChange = true;
		}

		// Token: 0x06003E8D RID: 16013 RVA: 0x00256470 File Offset: 0x00254670
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, base.IsDirect ? 146 : 145, -1, -1, -1, -1), EDataModifyType.Add);
			bool canAffect = base.CanAffect;
			if (canAffect)
			{
				base.ShowSpecialEffectTips(0);
			}
		}

		// Token: 0x06003E8E RID: 16014 RVA: 0x002564D3 File Offset: 0x002546D3
		protected override void OnMoveSkillCanAffectChanged(DataContext context, DataUid dataUid)
		{
			DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, base.IsDirect ? 146 : 145);
		}

		// Token: 0x06003E8F RID: 16015 RVA: 0x002564FC File Offset: 0x002546FC
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || !base.CanAffect;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				bool flag2 = dataKey.FieldId == 145 || dataKey.FieldId == 146;
				if (flag2)
				{
					result = 20;
				}
				else
				{
					result = 0;
				}
			}
			return result;
		}

		// Token: 0x04001274 RID: 4724
		private const sbyte AddAttackRange = 20;
	}
}
