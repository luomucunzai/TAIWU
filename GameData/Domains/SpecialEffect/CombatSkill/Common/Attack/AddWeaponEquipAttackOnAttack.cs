using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Common.Attack
{
	// Token: 0x0200058C RID: 1420
	public class AddWeaponEquipAttackOnAttack : CombatSkillEffectBase
	{
		// Token: 0x1700028C RID: 652
		// (get) Token: 0x0600420C RID: 16908 RVA: 0x002651B7 File Offset: 0x002633B7
		protected virtual short AddWeaponEquipAttack
		{
			get
			{
				return 800;
			}
		}

		// Token: 0x0600420D RID: 16909 RVA: 0x002651BE File Offset: 0x002633BE
		protected AddWeaponEquipAttackOnAttack()
		{
		}

		// Token: 0x0600420E RID: 16910 RVA: 0x002651C8 File Offset: 0x002633C8
		protected AddWeaponEquipAttackOnAttack(CombatSkillKey skillKey, int type) : base(skillKey, type, -1)
		{
		}

		// Token: 0x0600420F RID: 16911 RVA: 0x002651D5 File Offset: 0x002633D5
		public override void OnEnable(DataContext context)
		{
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			Events.RegisterHandler_PrepareSkillEnd(new Events.OnPrepareSkillEnd(this.OnPrepareSkillEnd));
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06004210 RID: 16912 RVA: 0x00265209 File Offset: 0x00263409
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_PrepareSkillEnd(new Events.OnPrepareSkillEnd(this.OnPrepareSkillEnd));
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06004211 RID: 16913 RVA: 0x00265234 File Offset: 0x00263434
		protected virtual void OnPrepareSkillEnd(DataContext context, int charId, bool isAlly, short skillId)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId || !DomainManager.Combat.InAttackRange(base.CombatChar);
			if (!flag)
			{
				base.AppendAffectedData(context, base.CharacterId, 141, EDataModifyType.Add, -1);
				base.ShowSpecialEffectTips(0);
			}
		}

		// Token: 0x06004212 RID: 16914 RVA: 0x00265290 File Offset: 0x00263490
		protected virtual void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				base.RemoveSelf(context);
			}
		}

		// Token: 0x06004213 RID: 16915 RVA: 0x002652C8 File Offset: 0x002634C8
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.CharId != base.CharacterId;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				bool flag2 = dataKey.FieldId == 141;
				if (flag2)
				{
					result = (int)this.AddWeaponEquipAttack;
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
