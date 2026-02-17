using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Defense;

namespace GameData.Domains.SpecialEffect.CombatSkill.Shixiangmen.DefenseAndAssist
{
	// Token: 0x02000401 RID: 1025
	public class ShiXiangTieTouGong : DefenseSkillBase
	{
		// Token: 0x060038BB RID: 14523 RVA: 0x0023BA87 File Offset: 0x00239C87
		public ShiXiangTieTouGong()
		{
		}

		// Token: 0x060038BC RID: 14524 RVA: 0x0023BA91 File Offset: 0x00239C91
		public ShiXiangTieTouGong(CombatSkillKey skillKey) : base(skillKey, 6500)
		{
		}

		// Token: 0x060038BD RID: 14525 RVA: 0x0023BAA1 File Offset: 0x00239CA1
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 114, -1, -1, -1, -1), EDataModifyType.Custom);
		}

		// Token: 0x060038BE RID: 14526 RVA: 0x0023BAD8 File Offset: 0x00239CD8
		public override long GetModifiedValue(AffectedDataKey dataKey, long dataValue)
		{
			EDamageType damageType = (EDamageType)dataKey.CustomParam0;
			bool flag = dataKey.CharId != base.CharacterId || damageType != EDamageType.Direct || !base.CanAffect;
			long result;
			if (flag)
			{
				result = dataValue;
			}
			else
			{
				bool isInner = dataKey.CustomParam1 == 1;
				sbyte bodyPart = (sbyte)dataKey.CustomParam2;
				bool flag2 = bodyPart != 2 || isInner == base.IsDirect;
				if (flag2)
				{
					result = dataValue;
				}
				else
				{
					DamageStepCollection damageStepCollection = base.CombatChar.GetDamageStepCollection();
					int originDamageValue = isInner ? base.CombatChar.GetInnerDamageValue()[(int)bodyPart] : base.CombatChar.GetOuterDamageValue()[(int)bodyPart];
					int injuryStep = isInner ? damageStepCollection.InnerDamageSteps[(int)bodyPart] : damageStepCollection.OuterDamageSteps[(int)bodyPart];
					ValueTuple<int, int> damageResult = CombatDomain.CalcInjuryMarkCount((int)Math.Min((long)originDamageValue + dataValue, 2147483647L), injuryStep, -1);
					bool flag3 = damageResult.Item1 > 3;
					if (flag3)
					{
						result = dataValue;
					}
					else
					{
						base.ShowSpecialEffectTips(0);
						result = 0L;
					}
				}
			}
			return result;
		}

		// Token: 0x0400109D RID: 4253
		private const sbyte RequireInjuryCount = 3;
	}
}
