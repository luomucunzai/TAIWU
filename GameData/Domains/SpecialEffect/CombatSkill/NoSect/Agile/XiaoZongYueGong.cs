using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Agile;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.NoSect.Agile
{
	// Token: 0x02000475 RID: 1141
	public class XiaoZongYueGong : AgileSkillBase
	{
		// Token: 0x06003B65 RID: 15205 RVA: 0x00247C10 File Offset: 0x00245E10
		public XiaoZongYueGong()
		{
		}

		// Token: 0x06003B66 RID: 15206 RVA: 0x00247C1A File Offset: 0x00245E1A
		public XiaoZongYueGong(CombatSkillKey skillKey) : base(skillKey, 200)
		{
		}

		// Token: 0x06003B67 RID: 15207 RVA: 0x00247C2A File Offset: 0x00245E2A
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 175, -1, -1, -1, -1), EDataModifyType.Custom);
			base.ShowSpecialEffectTips(0);
		}

		// Token: 0x06003B68 RID: 15208 RVA: 0x00247C6C File Offset: 0x00245E6C
		public override int GetModifiedValue(AffectedDataKey dataKey, int dataValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || dataKey.CombatSkillId < 0 || !base.CanAffect || !DomainManager.Combat.Context.Random.CheckPercentProb(50);
			int result;
			if (flag)
			{
				result = dataValue;
			}
			else
			{
				bool flag2 = dataKey.FieldId == 175;
				if (flag2)
				{
					result = 0;
				}
				else
				{
					result = dataValue;
				}
			}
			return result;
		}

		// Token: 0x04001165 RID: 4453
		private const sbyte NoCostOdds = 50;
	}
}
