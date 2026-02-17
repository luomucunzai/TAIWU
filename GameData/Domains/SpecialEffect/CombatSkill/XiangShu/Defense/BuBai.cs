using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Defense;

namespace GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Defense
{
	// Token: 0x020002A9 RID: 681
	public class BuBai : DefenseSkillBase
	{
		// Token: 0x060031D7 RID: 12759 RVA: 0x0021C5F5 File Offset: 0x0021A7F5
		public BuBai()
		{
		}

		// Token: 0x060031D8 RID: 12760 RVA: 0x0021C5FF File Offset: 0x0021A7FF
		public BuBai(CombatSkillKey skillKey) : base(skillKey, 16301)
		{
		}

		// Token: 0x060031D9 RID: 12761 RVA: 0x0021C60F File Offset: 0x0021A80F
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 114, -1, -1, -1, -1), EDataModifyType.Custom);
		}

		// Token: 0x060031DA RID: 12762 RVA: 0x0021C644 File Offset: 0x0021A844
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
				DamageStepCollection damageStepCollection = base.CombatChar.GetDamageStepCollection();
				int originDamageValue = isInner ? base.CombatChar.GetInnerDamageValue()[(int)bodyPart] : base.CombatChar.GetOuterDamageValue()[(int)bodyPart];
				int injuryStep = isInner ? damageStepCollection.InnerDamageSteps[(int)bodyPart] : damageStepCollection.OuterDamageSteps[(int)bodyPart];
				ValueTuple<int, int> damageResult = CombatDomain.CalcInjuryMarkCount((int)Math.Min((long)originDamageValue + dataValue, 2147483647L), injuryStep, -1);
				bool flag2 = (int)base.CombatChar.GetInjuries().Get(bodyPart, isInner) + damageResult.Item1 >= 6;
				if (flag2)
				{
					base.ShowSpecialEffectTipsOnceInFrame(0);
					result = 0L;
				}
				else
				{
					result = dataValue;
				}
			}
			return result;
		}
	}
}
