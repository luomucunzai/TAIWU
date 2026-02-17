using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Defense;

namespace GameData.Domains.SpecialEffect.CombatSkill.Baihuagu.DefenseAndAssist
{
	// Token: 0x020005DB RID: 1499
	public class WanBiBuPoFa : DefenseSkillBase
	{
		// Token: 0x0600444B RID: 17483 RVA: 0x0026EF85 File Offset: 0x0026D185
		public WanBiBuPoFa()
		{
		}

		// Token: 0x0600444C RID: 17484 RVA: 0x0026EF8F File Offset: 0x0026D18F
		public WanBiBuPoFa(CombatSkillKey skillKey) : base(skillKey, 3508)
		{
		}

		// Token: 0x0600444D RID: 17485 RVA: 0x0026EF9F File Offset: 0x0026D19F
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			base.CreateAffectedData(114, EDataModifyType.Custom, -1);
		}

		// Token: 0x0600444E RID: 17486 RVA: 0x0026EFB8 File Offset: 0x0026D1B8
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
					sbyte bodyPart = (sbyte)dataKey.CustomParam2;
					int existDamage = (inner ? base.CombatChar.GetInnerDamageValue() : base.CombatChar.GetOuterDamageValue())[(int)bodyPart];
					DamageStepCollection stepCollection = base.CombatChar.GetDamageStepCollection();
					int damageStep = (inner ? stepCollection.InnerDamageSteps : stepCollection.OuterDamageSteps)[(int)bodyPart];
					int newMark = CombatDomain.CalcInjuryMarkCount((int)Math.Min(dataValue + (long)existDamage, 2147483647L), damageStep, -1).Item1;
					sbyte oldMark = base.CombatChar.GetInjuries().Get(bodyPart, inner);
					long returnValue = this.CalcReturnValue(dataValue, newMark, (int)oldMark);
					bool flag3 = dataValue > 0L && returnValue == 0L;
					if (flag3)
					{
						base.ShowSpecialEffectTips(0);
					}
					result = returnValue;
				}
			}
			return result;
		}

		// Token: 0x0600444F RID: 17487 RVA: 0x0026F0D4 File Offset: 0x0026D2D4
		private long CalcReturnValue(long dataValue, int newMark, int oldMark)
		{
			bool isDirect = base.IsDirect;
			long result;
			if (isDirect)
			{
				result = ((newMark < oldMark) ? dataValue : 0L);
			}
			else
			{
				result = ((newMark > oldMark) ? dataValue : 0L);
			}
			return result;
		}
	}
}
