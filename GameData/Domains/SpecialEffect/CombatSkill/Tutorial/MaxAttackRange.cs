using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Tutorial
{
	// Token: 0x020003E6 RID: 998
	public class MaxAttackRange : CombatSkillEffectBase
	{
		// Token: 0x0600381B RID: 14363 RVA: 0x00238D4A File Offset: 0x00236F4A
		public MaxAttackRange()
		{
		}

		// Token: 0x0600381C RID: 14364 RVA: 0x00238D54 File Offset: 0x00236F54
		public MaxAttackRange(CombatSkillKey skillKey) : base(skillKey, -1, -1)
		{
		}

		// Token: 0x0600381D RID: 14365 RVA: 0x00238D64 File Offset: 0x00236F64
		public override void OnEnable(DataContext context)
		{
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 145, base.SkillTemplateId, -1, -1, -1), EDataModifyType.Add);
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 146, base.SkillTemplateId, -1, -1, -1), EDataModifyType.Add);
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x0600381E RID: 14366 RVA: 0x00238DDB File Offset: 0x00236FDB
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x0600381F RID: 14367 RVA: 0x00238DF0 File Offset: 0x00236FF0
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				base.RemoveSelf(context);
			}
		}

		// Token: 0x06003820 RID: 14368 RVA: 0x00238E28 File Offset: 0x00237028
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
				bool flag2 = dataKey.FieldId == 145 || dataKey.FieldId == 146;
				if (flag2)
				{
					result = 10000;
				}
				else
				{
					result = 0;
				}
			}
			return result;
		}
	}
}
