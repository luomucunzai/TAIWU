using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Defense;

namespace GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Defense
{
	// Token: 0x020002B2 RID: 690
	public class XinHuaLiaoMeng : DefenseSkillBase
	{
		// Token: 0x06003209 RID: 12809 RVA: 0x0021D99C File Offset: 0x0021BB9C
		public XinHuaLiaoMeng()
		{
		}

		// Token: 0x0600320A RID: 12810 RVA: 0x0021D9A6 File Offset: 0x0021BBA6
		public XinHuaLiaoMeng(CombatSkillKey skillKey) : base(skillKey, 16304)
		{
			this.ListenCanAffectChange = true;
		}

		// Token: 0x0600320B RID: 12811 RVA: 0x0021D9BD File Offset: 0x0021BBBD
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			base.CreateAffectedData(126, EDataModifyType.Custom, -1);
			base.CreateAffectedData(131, EDataModifyType.Custom, -1);
			base.CreateAffectedData(288, EDataModifyType.Custom, -1);
		}

		// Token: 0x0600320C RID: 12812 RVA: 0x0021D9F0 File Offset: 0x0021BBF0
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
				ushort fieldId = dataKey.FieldId;
				bool flag2 = fieldId == 126 || fieldId == 131;
				bool flag3 = flag2;
				if (flag3)
				{
					result = false;
				}
				else
				{
					bool flag4 = dataKey.FieldId == 288;
					result = (flag4 || dataValue);
				}
			}
			return result;
		}
	}
}
