using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Attack.XueFeng
{
	// Token: 0x020002D4 RID: 724
	public class ZhanWuShu : CombatSkillEffectBase
	{
		// Token: 0x060032B5 RID: 12981 RVA: 0x00220953 File Offset: 0x0021EB53
		public ZhanWuShu()
		{
		}

		// Token: 0x060032B6 RID: 12982 RVA: 0x0022095D File Offset: 0x0021EB5D
		public ZhanWuShu(CombatSkillKey skillKey) : base(skillKey, 17070, -1)
		{
		}

		// Token: 0x060032B7 RID: 12983 RVA: 0x00220970 File Offset: 0x0021EB70
		public override void OnEnable(DataContext context)
		{
			this._affected = false;
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 77, base.SkillTemplateId, -1, -1, -1), EDataModifyType.Custom);
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x060032B8 RID: 12984 RVA: 0x002209C5 File Offset: 0x0021EBC5
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x060032B9 RID: 12985 RVA: 0x002209DC File Offset: 0x0021EBDC
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				bool affected = this._affected;
				if (affected)
				{
					base.ShowSpecialEffectTips(0);
				}
				base.RemoveSelf(context);
			}
		}

		// Token: 0x060032BA RID: 12986 RVA: 0x00220A24 File Offset: 0x0021EC24
		public override bool GetModifiedValue(AffectedDataKey dataKey, bool dataValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || dataKey.CombatSkillId != base.SkillTemplateId;
			bool result;
			if (flag)
			{
				result = dataValue;
			}
			else
			{
				bool flag2 = dataKey.FieldId == 77;
				if (flag2)
				{
					this._affected = true;
					result = true;
				}
				else
				{
					result = dataValue;
				}
			}
			return result;
		}

		// Token: 0x04000F02 RID: 3842
		private bool _affected;
	}
}
