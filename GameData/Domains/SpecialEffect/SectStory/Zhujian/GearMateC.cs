using System;
using GameData.Combat.Math;
using GameData.Common;

namespace GameData.Domains.SpecialEffect.SectStory.Zhujian
{
	// Token: 0x020000E7 RID: 231
	public class GearMateC : AutoCollectEffectBase
	{
		// Token: 0x0600295B RID: 10587 RVA: 0x00200554 File Offset: 0x001FE754
		public GearMateC(int charId) : base(charId)
		{
		}

		// Token: 0x0600295C RID: 10588 RVA: 0x0020055F File Offset: 0x001FE75F
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			base.CreateAffectedAllEnemyData(286, EDataModifyType.Custom, -1);
		}

		// Token: 0x0600295D RID: 10589 RVA: 0x00200578 File Offset: 0x001FE778
		public override bool GetModifiedValue(AffectedDataKey dataKey, bool dataValue)
		{
			bool flag = dataKey.FieldId != 286;
			bool result;
			if (flag)
			{
				result = dataValue;
			}
			else
			{
				bool flag2 = !this.CharObj.IsCombatSkillEquipped(dataKey.CombatSkillId);
				result = (flag2 && dataValue);
			}
			return result;
		}
	}
}
