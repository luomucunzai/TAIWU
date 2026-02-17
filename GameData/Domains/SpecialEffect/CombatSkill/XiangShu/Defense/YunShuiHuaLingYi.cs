using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Defense;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Defense
{
	// Token: 0x020002B6 RID: 694
	public class YunShuiHuaLingYi : DefenseSkillBase
	{
		// Token: 0x06003220 RID: 12832 RVA: 0x0021DF0C File Offset: 0x0021C10C
		public YunShuiHuaLingYi()
		{
		}

		// Token: 0x06003221 RID: 12833 RVA: 0x0021DF16 File Offset: 0x0021C116
		public YunShuiHuaLingYi(CombatSkillKey skillKey) : base(skillKey, 16300)
		{
		}

		// Token: 0x06003222 RID: 12834 RVA: 0x0021DF26 File Offset: 0x0021C126
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 107, -1, -1, -1, -1), EDataModifyType.Custom);
		}

		// Token: 0x06003223 RID: 12835 RVA: 0x0021DF5C File Offset: 0x0021C15C
		public override int GetModifiedValue(AffectedDataKey dataKey, int dataValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || !base.CanAffect || dataValue < 0;
			int result;
			if (flag)
			{
				result = dataValue;
			}
			else
			{
				int avoidOdds = 10 * base.CurrEnemyChar.GetDefeatMarkCollection().PoisonMarkList.Sum();
				bool flag2 = DomainManager.Combat.Context.Random.CheckPercentProb(avoidOdds);
				if (flag2)
				{
					dataValue = 0;
					base.ShowSpecialEffectTips(0);
				}
				result = dataValue;
			}
			return result;
		}

		// Token: 0x04000ED8 RID: 3800
		private const sbyte AvoidOddsPerMark = 10;
	}
}
