using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Attack;

namespace GameData.Domains.SpecialEffect.CombatSkill.Baihuagu.Finger
{
	// Token: 0x020005CD RID: 1485
	public class ChangChunZhi : ChangePoisonLevel
	{
		// Token: 0x170002B7 RID: 695
		// (get) Token: 0x060043F8 RID: 17400 RVA: 0x0026DA68 File Offset: 0x0026BC68
		protected override sbyte AffectPoisonType
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x060043F9 RID: 17401 RVA: 0x0026DA6B File Offset: 0x0026BC6B
		public ChangChunZhi()
		{
		}

		// Token: 0x060043FA RID: 17402 RVA: 0x0026DA75 File Offset: 0x0026BC75
		public ChangChunZhi(CombatSkillKey skillKey) : base(skillKey, 3101)
		{
		}

		// Token: 0x060043FB RID: 17403 RVA: 0x0026DA85 File Offset: 0x0026BC85
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			base.CreateAffectedData(base.IsDirect ? 233 : 232, EDataModifyType.Add, -1);
		}

		// Token: 0x060043FC RID: 17404 RVA: 0x0026DAB0 File Offset: 0x0026BCB0
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = base.IsMatchOwnAffect(dataKey.SkillKey);
			if (flag)
			{
				ushort fieldId = dataKey.FieldId;
				if (!true)
				{
				}
				int num;
				if (fieldId != 232)
				{
					if (fieldId != 233)
					{
						num = 0;
					}
					else
					{
						num = -4;
					}
				}
				else
				{
					num = 2;
				}
				if (!true)
				{
				}
				int effectValue = num;
				bool flag2 = effectValue != 0;
				if (flag2)
				{
					base.ShowSpecialEffectTipsOnceInFrame(1);
					return effectValue * base.EffectCount;
				}
			}
			return base.GetModifyValue(dataKey, currModifyValue);
		}

		// Token: 0x0400142F RID: 5167
		private const int DirectPoisonResist = -4;

		// Token: 0x04001430 RID: 5168
		private const int ReversePoisonResist = 2;
	}
}
