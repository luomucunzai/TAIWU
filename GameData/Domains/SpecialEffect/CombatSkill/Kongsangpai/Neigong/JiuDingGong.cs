using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Kongsangpai.Neigong
{
	// Token: 0x02000481 RID: 1153
	public class JiuDingGong : CombatSkillEffectBase
	{
		// Token: 0x06003BA7 RID: 15271 RVA: 0x00249043 File Offset: 0x00247243
		public JiuDingGong()
		{
		}

		// Token: 0x06003BA8 RID: 15272 RVA: 0x0024904D File Offset: 0x0024724D
		public JiuDingGong(CombatSkillKey skillKey) : base(skillKey, 10004, -1)
		{
		}

		// Token: 0x06003BA9 RID: 15273 RVA: 0x0024905E File Offset: 0x0024725E
		public override void OnEnable(DataContext context)
		{
			base.CreateAffectedData(199, EDataModifyType.Add, -1);
		}

		// Token: 0x06003BAA RID: 15274 RVA: 0x00249070 File Offset: 0x00247270
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || dataKey.FieldId != 199;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				int healthPercent = CValuePercent.ParseInt((int)this.CharObj.GetHealth(), (int)this.CharObj.GetMaxHealth());
				int addPower = base.IsDirect ? (healthPercent * 40 / 100) : ((100 - healthPercent) * 40 / 100);
				result = Math.Clamp(addPower, 0, 20);
			}
			return result;
		}

		// Token: 0x04001178 RID: 4472
		private const sbyte AddPowerRatio = 40;

		// Token: 0x04001179 RID: 4473
		private const sbyte MaxAddPower = 20;
	}
}
