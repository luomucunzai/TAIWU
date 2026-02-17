using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Agile;

namespace GameData.Domains.SpecialEffect.CombatSkill.Ranshanpai.Agile
{
	// Token: 0x0200046C RID: 1132
	public class WanLiShenXingZhou : AgileSkillBase
	{
		// Token: 0x06003B2B RID: 15147 RVA: 0x00246E43 File Offset: 0x00245043
		public WanLiShenXingZhou()
		{
		}

		// Token: 0x06003B2C RID: 15148 RVA: 0x00246E4D File Offset: 0x0024504D
		public WanLiShenXingZhou(CombatSkillKey skillKey) : base(skillKey, 7405)
		{
		}

		// Token: 0x06003B2D RID: 15149 RVA: 0x00246E60 File Offset: 0x00245060
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 149, -1, -1, -1, -1), EDataModifyType.Custom);
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 175, -1, -1, -1, -1), EDataModifyType.Custom);
			base.ShowSpecialEffectTips(0);
		}

		// Token: 0x06003B2E RID: 15150 RVA: 0x00246ECC File Offset: 0x002450CC
		public override bool GetModifiedValue(AffectedDataKey dataKey, bool dataValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || !base.CanAffect;
			bool result;
			if (flag)
			{
				result = dataValue;
			}
			else
			{
				bool flag2 = dataKey.FieldId == 149 && dataKey.CustomParam0 >= 0 && DomainManager.Combat.GetElement_CombatCharacterDict(dataKey.CustomParam0).IsAlly != base.CombatChar.IsAlly;
				result = (!flag2 && dataValue);
			}
			return result;
		}

		// Token: 0x06003B2F RID: 15151 RVA: 0x00246F48 File Offset: 0x00245148
		public override int GetModifiedValue(AffectedDataKey dataKey, int dataValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || dataKey.CombatSkillId < 0 || !base.CanAffect;
			int result;
			if (flag)
			{
				result = dataValue;
			}
			else
			{
				short currDistance = DomainManager.Combat.GetCurrentDistance();
				bool flag2 = base.IsDirect ? (currDistance < 50) : (currDistance > 70);
				if (flag2)
				{
					result = dataValue;
				}
				else
				{
					bool flag3 = dataKey.FieldId == 175;
					if (flag3)
					{
						result = 0;
					}
					else
					{
						result = dataValue;
					}
				}
			}
			return result;
		}

		// Token: 0x04001153 RID: 4435
		private const int DirectRequireDistance = 50;

		// Token: 0x04001154 RID: 4436
		private const int ReverseRequireDistance = 70;
	}
}
