using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill;

namespace GameData.Domains.SpecialEffect.LegendaryBook.Posing
{
	// Token: 0x02000138 RID: 312
	public class JingShui : CombatSkillEffectBase
	{
		// Token: 0x06002A78 RID: 10872 RVA: 0x00202A7D File Offset: 0x00200C7D
		public JingShui()
		{
			this.IsLegendaryBookEffect = true;
		}

		// Token: 0x06002A79 RID: 10873 RVA: 0x00202A8E File Offset: 0x00200C8E
		public JingShui(CombatSkillKey skillKey) : base(skillKey, 40104, -1)
		{
			this.IsLegendaryBookEffect = true;
		}

		// Token: 0x06002A7A RID: 10874 RVA: 0x00202AA6 File Offset: 0x00200CA6
		public override void OnEnable(DataContext context)
		{
			base.CreateAffectedData(179, EDataModifyType.TotalPercent, -1);
		}

		// Token: 0x06002A7B RID: 10875 RVA: 0x00202AB8 File Offset: 0x00200CB8
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
				bool flag2 = dataKey.FieldId == 179;
				if (flag2)
				{
					result = -25;
				}
				else
				{
					result = 0;
				}
			}
			return result;
		}

		// Token: 0x04000CF8 RID: 3320
		private const sbyte ChangeFrameCost = -25;
	}
}
