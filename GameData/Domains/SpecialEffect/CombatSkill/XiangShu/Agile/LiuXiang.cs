using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Agile;

namespace GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Agile
{
	// Token: 0x0200033B RID: 827
	public class LiuXiang : AgileSkillBase
	{
		// Token: 0x060034B2 RID: 13490 RVA: 0x002297D1 File Offset: 0x002279D1
		public LiuXiang()
		{
		}

		// Token: 0x060034B3 RID: 13491 RVA: 0x002297DB File Offset: 0x002279DB
		public LiuXiang(CombatSkillKey skillKey) : base(skillKey, 16203)
		{
		}

		// Token: 0x060034B4 RID: 13492 RVA: 0x002297EC File Offset: 0x002279EC
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 148, -1, -1, -1, -1), EDataModifyType.Custom);
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 217, -1, -1, -1, -1), EDataModifyType.Custom);
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 215, -1, -1, -1, -1), EDataModifyType.Custom);
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 126, -1, -1, -1, -1), EDataModifyType.Custom);
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 131, -1, -1, -1, -1), EDataModifyType.Custom);
			base.ShowSpecialEffectTips(0);
		}

		// Token: 0x060034B5 RID: 13493 RVA: 0x002298B8 File Offset: 0x00227AB8
		public override bool GetModifiedValue(AffectedDataKey dataKey, bool dataValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || !base.CanAffect;
			return flag && dataValue;
		}
	}
}
