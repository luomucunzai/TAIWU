using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Assist;

namespace GameData.Domains.SpecialEffect.CombatSkill.Shaolinpai.DefenseAndAssist
{
	// Token: 0x02000431 RID: 1073
	public class BianTiTongRenFa : AssistSkillBase
	{
		// Token: 0x060039AF RID: 14767 RVA: 0x0023FD4C File Offset: 0x0023DF4C
		public BianTiTongRenFa()
		{
		}

		// Token: 0x060039B0 RID: 14768 RVA: 0x0023FD56 File Offset: 0x0023DF56
		public BianTiTongRenFa(CombatSkillKey skillKey) : base(skillKey, 1603)
		{
		}

		// Token: 0x060039B1 RID: 14769 RVA: 0x0023FD66 File Offset: 0x0023DF66
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			base.CreateAffectedData(114, EDataModifyType.Custom, -1);
		}

		// Token: 0x060039B2 RID: 14770 RVA: 0x0023FD7C File Offset: 0x0023DF7C
		public override long GetModifiedValue(AffectedDataKey dataKey, long dataValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || !base.CanAffect || dataKey.FieldId != 114;
			long result;
			if (flag)
			{
				result = dataValue;
			}
			else
			{
				EDamageType damageType = (EDamageType)dataKey.CustomParam0;
				bool flag2 = damageType != EDamageType.Direct;
				if (flag2)
				{
					result = dataValue;
				}
				else
				{
					bool inner = dataKey.CustomParam1 == 1;
					bool flag3 = inner == base.IsDirect;
					if (flag3)
					{
						result = dataValue;
					}
					else
					{
						sbyte bodyPart = (sbyte)dataKey.CustomParam2;
						sbyte existInjury = base.CombatChar.GetInjuries().Get(bodyPart, inner);
						bool flag4 = existInjury > 0;
						if (flag4)
						{
							result = dataValue;
						}
						else
						{
							DamageStepCollection stepCollection = base.CombatChar.GetDamageStepCollection();
							int damageStep = (inner ? stepCollection.InnerDamageSteps : stepCollection.OuterDamageSteps)[(int)bodyPart];
							int existDamageValue = base.CombatChar.GetDamageValue(bodyPart, inner);
							bool flag5 = (long)existDamageValue + dataValue <= (long)damageStep;
							if (flag5)
							{
								result = dataValue;
							}
							else
							{
								base.ShowSpecialEffectTips(0);
								result = (long)(damageStep - existDamageValue);
							}
						}
					}
				}
			}
			return result;
		}
	}
}
