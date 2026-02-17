using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Agile;

namespace GameData.Domains.SpecialEffect.CombatSkill.Ranshanpai.Agile
{
	// Token: 0x0200046F RID: 1135
	public class XingYunQiWuFa : AgileSkillBase
	{
		// Token: 0x06003B3E RID: 15166 RVA: 0x00247345 File Offset: 0x00245545
		public XingYunQiWuFa()
		{
		}

		// Token: 0x06003B3F RID: 15167 RVA: 0x0024734F File Offset: 0x0024554F
		public XingYunQiWuFa(CombatSkillKey skillKey) : base(skillKey, 7403)
		{
		}

		// Token: 0x06003B40 RID: 15168 RVA: 0x00247360 File Offset: 0x00245560
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			int[] charList = DomainManager.Combat.GetCharacterList(!base.CombatChar.IsAlly);
			for (int i = 0; i < charList.Length; i++)
			{
				bool flag = charList[i] >= 0;
				if (flag)
				{
					this.AffectDatas.Add(new AffectedDataKey(charList[i], base.IsDirect ? 71 : 70, -1, -1, -1, -1), EDataModifyType.TotalPercent);
				}
			}
			base.ShowSpecialEffectTips(0);
		}

		// Token: 0x06003B41 RID: 15169 RVA: 0x002473EC File Offset: 0x002455EC
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = !base.CanAffect;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				result = -60;
			}
			return result;
		}

		// Token: 0x04001159 RID: 4441
		private const sbyte ReduceDamage = -60;
	}
}
