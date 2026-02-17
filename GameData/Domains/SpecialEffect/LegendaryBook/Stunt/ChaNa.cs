using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill;

namespace GameData.Domains.SpecialEffect.LegendaryBook.Stunt
{
	// Token: 0x0200012B RID: 299
	public class ChaNa : CombatSkillEffectBase
	{
		// Token: 0x06002A50 RID: 10832 RVA: 0x00202425 File Offset: 0x00200625
		public ChaNa()
		{
			this.IsLegendaryBookEffect = true;
		}

		// Token: 0x06002A51 RID: 10833 RVA: 0x00202436 File Offset: 0x00200636
		public ChaNa(CombatSkillKey skillKey) : base(skillKey, 40203, -1)
		{
			this.IsLegendaryBookEffect = true;
		}

		// Token: 0x06002A52 RID: 10834 RVA: 0x00202450 File Offset: 0x00200650
		public override void OnEnable(DataContext context)
		{
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 176, -1, -1, -1, -1), EDataModifyType.AddPercent);
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 199, -1, -1, -1, -1), EDataModifyType.AddPercent);
		}

		// Token: 0x06002A53 RID: 10835 RVA: 0x002024AC File Offset: 0x002006AC
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || dataKey.CombatSkillId != base.SkillTemplateId;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				bool flag2 = dataKey.FieldId == 176;
				if (flag2)
				{
					result = -50;
				}
				else
				{
					bool flag3 = dataKey.FieldId == 199;
					if (flag3)
					{
						result = 100;
					}
					else
					{
						result = 0;
					}
				}
			}
			return result;
		}

		// Token: 0x04000CF0 RID: 3312
		private const sbyte ChangeKeepFrames = -50;

		// Token: 0x04000CF1 RID: 3313
		private const sbyte ChangePower = 100;
	}
}
