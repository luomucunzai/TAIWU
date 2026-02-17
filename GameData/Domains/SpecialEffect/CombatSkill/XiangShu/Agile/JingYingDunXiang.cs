using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Agile;

namespace GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Agile
{
	// Token: 0x0200033A RID: 826
	public class JingYingDunXiang : AgileSkillBase
	{
		// Token: 0x060034AE RID: 13486 RVA: 0x002296C5 File Offset: 0x002278C5
		public JingYingDunXiang()
		{
		}

		// Token: 0x060034AF RID: 13487 RVA: 0x002296CF File Offset: 0x002278CF
		public JingYingDunXiang(CombatSkillKey skillKey) : base(skillKey, 16206)
		{
		}

		// Token: 0x060034B0 RID: 13488 RVA: 0x002296DF File Offset: 0x002278DF
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			base.CreateAffectedData(114, EDataModifyType.Custom, -1);
		}

		// Token: 0x060034B1 RID: 13489 RVA: 0x002296F8 File Offset: 0x002278F8
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
				bool flag2 = base.CombatChar.GetMobilityValue() == 0;
				if (flag2)
				{
					result = dataValue;
				}
				else
				{
					int mobilityPercent = base.CombatChar.GetMobilityValue() * 100 / MoveSpecialConstants.MaxMobility + 1;
					int damageUnit = base.CombatChar.GetDamageStepCollection().FatalDamageStep / 10;
					long costMobilityPercent = Math.Min(dataValue / (long)damageUnit, (long)mobilityPercent);
					DomainManager.Combat.ChangeMobilityValue(DomainManager.Combat.Context, base.CombatChar, (int)((long)(-(long)MoveSpecialConstants.MaxMobility) * costMobilityPercent / 100L), true, base.CombatChar, false);
					base.ShowSpecialEffectTipsOnceInFrame(0);
					result = (long)damageUnit * (dataValue / (long)damageUnit - costMobilityPercent);
				}
			}
			return result;
		}
	}
}
