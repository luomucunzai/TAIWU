using System;
using System.Runtime.CompilerServices;
using Config;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Agile;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.Ranshanpai.Agile
{
	// Token: 0x0200046B RID: 1131
	public class LingLiuXu : BuffHitOrDebuffAvoid
	{
		// Token: 0x1700021F RID: 543
		// (get) Token: 0x06003B25 RID: 15141 RVA: 0x00246C40 File Offset: 0x00244E40
		protected override sbyte AffectHitType
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x06003B26 RID: 15142 RVA: 0x00246C43 File Offset: 0x00244E43
		public LingLiuXu()
		{
		}

		// Token: 0x06003B27 RID: 15143 RVA: 0x00246C4D File Offset: 0x00244E4D
		public LingLiuXu(CombatSkillKey skillKey) : base(skillKey, 7406)
		{
		}

		// Token: 0x06003B28 RID: 15144 RVA: 0x00246C60 File Offset: 0x00244E60
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			bool isDirect = base.IsDirect;
			if (isDirect)
			{
				base.CreateAffectedData(248, EDataModifyType.Custom, -1);
			}
			else
			{
				base.CreateAffectedData(234, EDataModifyType.Custom, -1);
			}
		}

		// Token: 0x06003B29 RID: 15145 RVA: 0x00246CA0 File Offset: 0x00244EA0
		public override bool GetModifiedValue(AffectedDataKey dataKey, bool dataValue)
		{
			ushort fieldId = dataKey.FieldId;
			if (!true)
			{
			}
			int num;
			if (fieldId != 234)
			{
				if (fieldId != 248)
				{
					num = -1;
				}
				else
				{
					num = dataKey.CustomParam0;
				}
			}
			else
			{
				num = dataKey.CustomParam1;
			}
			if (!true)
			{
			}
			int hitType = num;
			bool affected = this.CheckAffected(dataKey, hitType);
			bool flag = affected;
			if (flag)
			{
				base.ShowSpecialEffectTipsOnceInFrame(0);
			}
			ushort fieldId2 = dataKey.FieldId;
			if (!true)
			{
			}
			bool result;
			if (fieldId2 != 234)
			{
				if (fieldId2 != 248)
				{
					result = base.GetModifiedValue(dataKey, dataValue);
				}
				else
				{
					result = (dataValue || affected);
				}
			}
			else
			{
				result = (dataValue && !affected);
			}
			if (!true)
			{
			}
			return result;
		}

		// Token: 0x06003B2A RID: 15146 RVA: 0x00246D50 File Offset: 0x00244F50
		private bool CheckAffected(AffectedDataKey dataKey, int hitType)
		{
			bool flag = !base.CanAffect;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				bool isNormalAttack = dataKey.IsNormalAttack;
				if (isNormalAttack)
				{
					result = (hitType == (int)this.AffectHitType);
				}
				else
				{
					int charId = (dataKey.FieldId == 248) ? dataKey.CharId : dataKey.CustomParam2;
					GameData.Domains.CombatSkill.CombatSkill skill;
					bool flag2 = !DomainManager.CombatSkill.TryGetElement_CombatSkills(new ValueTuple<int, short>(charId, dataKey.CombatSkillId), out skill);
					if (flag2)
					{
						short predefinedLogId = 7;
						object arg = base.EffectId;
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(39, 1);
						defaultInterpolatedStringHandler.AppendLiteral("CheckAffected with unexpected skillKey ");
						defaultInterpolatedStringHandler.AppendFormatted<CombatSkillKey>(dataKey.SkillKey);
						PredefinedLog.Show(predefinedLogId, arg, defaultInterpolatedStringHandler.ToStringAndClear());
						result = false;
					}
					else
					{
						int rate = skill.GetHitDistribution()[hitType];
						result = DomainManager.Combat.Context.Random.CheckPercentProb(rate);
					}
				}
			}
			return result;
		}
	}
}
