using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Assist;

namespace GameData.Domains.SpecialEffect.CombatSkill.Baihuagu.DefenseAndAssist
{
	// Token: 0x020005D6 RID: 1494
	public class JingYiBiXie : AssistSkillBase
	{
		// Token: 0x06004426 RID: 17446 RVA: 0x0026E3A6 File Offset: 0x0026C5A6
		public JingYiBiXie()
		{
		}

		// Token: 0x06004427 RID: 17447 RVA: 0x0026E3B0 File Offset: 0x0026C5B0
		public JingYiBiXie(CombatSkillKey skillKey) : base(skillKey, 3601)
		{
			this.SetConstAffectingOnCombatBegin = true;
		}

		// Token: 0x06004428 RID: 17448 RVA: 0x0026E3C8 File Offset: 0x0026C5C8
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			bool isDirect = base.IsDirect;
			if (isDirect)
			{
				this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 106, -1, -1, -1, -1), EDataModifyType.TotalPercent);
			}
			else
			{
				this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 243, -1, -1, -1, -1), EDataModifyType.Add);
			}
		}

		// Token: 0x06004429 RID: 17449 RVA: 0x0026E434 File Offset: 0x0026C634
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
				bool flag2 = dataKey.FieldId == 106;
				if (flag2)
				{
					base.ShowSpecialEffectTipsOnceInFrame(0);
					result = -50;
				}
				else
				{
					bool flag3 = dataKey.FieldId == 243;
					if (flag3)
					{
						result = JingYiBiXie.PoisonAffectThresholdValues[dataKey.CustomParam0];
					}
					else
					{
						result = 0;
					}
				}
			}
			return result;
		}

		// Token: 0x04001438 RID: 5176
		private const sbyte ReducePercent = -50;

		// Token: 0x04001439 RID: 5177
		private static readonly int[] PoisonAffectThresholdValues = new int[]
		{
			1,
			15,
			25,
			25,
			200,
			200
		};
	}
}
