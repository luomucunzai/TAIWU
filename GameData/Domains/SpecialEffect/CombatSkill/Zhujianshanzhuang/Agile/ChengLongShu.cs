using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Agile;

namespace GameData.Domains.SpecialEffect.CombatSkill.Zhujianshanzhuang.Agile
{
	// Token: 0x020001E9 RID: 489
	public class ChengLongShu : AgileSkillBase
	{
		// Token: 0x06002E18 RID: 11800 RVA: 0x0020DB28 File Offset: 0x0020BD28
		public ChengLongShu()
		{
		}

		// Token: 0x06002E19 RID: 11801 RVA: 0x0020DB32 File Offset: 0x0020BD32
		public ChengLongShu(CombatSkillKey skillKey) : base(skillKey, 9505)
		{
			this.ListenCanAffectChange = true;
		}

		// Token: 0x06002E1A RID: 11802 RVA: 0x0020DB4C File Offset: 0x0020BD4C
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			this._affecting = false;
			this.OnMoveSkillCanAffectChanged(context, default(DataUid));
			base.CreateAffectedData(149, EDataModifyType.Custom, -1);
			base.CreateAffectedData(base.IsDirect ? 230 : 229, EDataModifyType.Custom, -1);
			base.ShowSpecialEffectTips(0);
			base.ShowSpecialEffectTips(1);
		}

		// Token: 0x06002E1B RID: 11803 RVA: 0x0020DBB8 File Offset: 0x0020BDB8
		protected override void OnMoveSkillCanAffectChanged(DataContext context, DataUid dataUid)
		{
			bool canAffect = base.CanAffect;
			bool flag = this._affecting == canAffect;
			if (!flag)
			{
				this._affecting = canAffect;
				DomainManager.Combat.UpdateSkillCanUse(context, base.CombatChar);
			}
		}

		// Token: 0x06002E1C RID: 11804 RVA: 0x0020DBF8 File Offset: 0x0020BDF8
		public override bool GetModifiedValue(AffectedDataKey dataKey, bool dataValue)
		{
			bool flag = !this._affecting;
			bool result;
			if (flag)
			{
				result = dataValue;
			}
			else
			{
				bool flag2 = dataKey.FieldId == 149 && dataKey.CustomParam0 >= 0 && DomainManager.Combat.GetElement_CombatCharacterDict(dataKey.CustomParam0).IsAlly != base.CombatChar.IsAlly;
				if (flag2)
				{
					result = false;
				}
				else
				{
					ushort fieldId = dataKey.FieldId;
					bool flag3 = fieldId - 229 <= 1;
					bool flag4 = flag3;
					result = (flag4 || dataValue);
				}
			}
			return result;
		}

		// Token: 0x04000DBD RID: 3517
		private bool _affecting;
	}
}
