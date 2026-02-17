using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Defense;

namespace GameData.Domains.SpecialEffect.CombatSkill.NoSect.DefenseAndAssist
{
	// Token: 0x02000474 RID: 1140
	public class ShuiHuoYingQiGong : DefenseSkillBase
	{
		// Token: 0x17000220 RID: 544
		// (get) Token: 0x06003B5F RID: 15199 RVA: 0x002479FF File Offset: 0x00245BFF
		private static CValuePercent ReduceEffectStep
		{
			get
			{
				return 20;
			}
		}

		// Token: 0x06003B60 RID: 15200 RVA: 0x00247A08 File Offset: 0x00245C08
		public ShuiHuoYingQiGong()
		{
			this.AutoRemove = false;
		}

		// Token: 0x06003B61 RID: 15201 RVA: 0x00247A19 File Offset: 0x00245C19
		public ShuiHuoYingQiGong(CombatSkillKey skillKey) : base(skillKey, 300)
		{
			this.AutoRemove = false;
		}

		// Token: 0x06003B62 RID: 15202 RVA: 0x00247A30 File Offset: 0x00245C30
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			this._reduceDamage = 80;
			base.CreateAffectedData(114, EDataModifyType.Custom, -1);
			base.CreateAffectedData(253, EDataModifyType.Custom, -1);
		}

		// Token: 0x06003B63 RID: 15203 RVA: 0x00247A5C File Offset: 0x00245C5C
		public override long GetModifiedValue(AffectedDataKey dataKey, long dataValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || dataKey.FieldId != 114 || !base.CanAffect;
			long result;
			if (flag)
			{
				result = base.GetModifiedValue(dataKey, dataValue);
			}
			else
			{
				EDamageType damageType = (EDamageType)dataKey.CustomParam0;
				bool flag2 = damageType != EDamageType.Direct;
				if (flag2)
				{
					result = base.GetModifiedValue(dataKey, dataValue);
				}
				else
				{
					long reduceValue = dataValue * this._reduceDamage;
					bool flag3 = reduceValue <= 0L;
					if (flag3)
					{
						result = base.GetModifiedValue(dataKey, dataValue);
					}
					else
					{
						base.ShowSpecialEffectTipsOnceInFrame(0);
						bool inner = dataKey.CustomParam1 == 1;
						sbyte bodyPart = (sbyte)dataKey.CustomParam2;
						DamageStepCollection steps = base.CombatChar.GetDamageStepCollection();
						int step = (inner ? steps.InnerDamageSteps : steps.OuterDamageSteps)[(int)bodyPart];
						bool flag4 = dataValue >= (long)(step * ShuiHuoYingQiGong.ReduceEffectStep);
						if (flag4)
						{
							int reduceEffect = (int)dataValue / Math.Max(step * ShuiHuoYingQiGong.ReduceEffectStep, 1);
							int prevReduceDamage = this._reduceDamage;
							this._reduceDamage = Math.Max(this._reduceDamage - reduceEffect, 0);
							bool flag5 = prevReduceDamage != this._reduceDamage;
							if (flag5)
							{
								base.InvalidateCache(DomainManager.Combat.Context, 253);
							}
						}
						result = dataValue - reduceValue;
					}
				}
			}
			return result;
		}

		// Token: 0x06003B64 RID: 15204 RVA: 0x00247BB4 File Offset: 0x00245DB4
		public override List<CombatSkillEffectData> GetModifiedValue(AffectedDataKey dataKey, List<CombatSkillEffectData> dataValue)
		{
			bool flag = dataKey.SkillKey != this.SkillKey || dataKey.FieldId != 253;
			List<CombatSkillEffectData> result;
			if (flag)
			{
				result = base.GetModifiedValue(dataKey, dataValue);
			}
			else
			{
				dataValue.Add(new CombatSkillEffectData(ECombatSkillEffectType.ShuiHuoYingQiGongReduceDamage, this._reduceDamage));
				result = dataValue;
			}
			return result;
		}

		// Token: 0x04001161 RID: 4449
		private const int DefaultReduceDamage = 80;

		// Token: 0x04001162 RID: 4450
		private const int MinReduceDamage = 0;

		// Token: 0x04001163 RID: 4451
		private const int ReduceEffectUnit = 1;

		// Token: 0x04001164 RID: 4452
		private int _reduceDamage;
	}
}
