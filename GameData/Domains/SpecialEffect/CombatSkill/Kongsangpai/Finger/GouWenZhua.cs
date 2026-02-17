using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Attack;

namespace GameData.Domains.SpecialEffect.CombatSkill.Kongsangpai.Finger
{
	// Token: 0x02000493 RID: 1171
	public class GouWenZhua : ChangePoisonLevel
	{
		// Token: 0x1700022B RID: 555
		// (get) Token: 0x06003C1F RID: 15391 RVA: 0x0024C13D File Offset: 0x0024A33D
		protected override sbyte AffectPoisonType
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x06003C20 RID: 15392 RVA: 0x0024C140 File Offset: 0x0024A340
		public GouWenZhua()
		{
		}

		// Token: 0x06003C21 RID: 15393 RVA: 0x0024C14A File Offset: 0x0024A34A
		public GouWenZhua(CombatSkillKey skillKey) : base(skillKey, 10201)
		{
		}

		// Token: 0x06003C22 RID: 15394 RVA: 0x0024C15A File Offset: 0x0024A35A
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			base.CreateAffectedData(base.IsDirect ? 233 : 232, EDataModifyType.Add, -1);
		}

		// Token: 0x06003C23 RID: 15395 RVA: 0x0024C184 File Offset: 0x0024A384
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

		// Token: 0x040011AD RID: 4525
		private const int DirectPoisonResist = -4;

		// Token: 0x040011AE RID: 4526
		private const int ReversePoisonResist = 2;
	}
}
