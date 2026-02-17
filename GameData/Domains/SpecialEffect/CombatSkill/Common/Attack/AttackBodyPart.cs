using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.Common.Attack
{
	// Token: 0x0200058D RID: 1421
	public class AttackBodyPart : CombatSkillEffectBase
	{
		// Token: 0x06004214 RID: 16916 RVA: 0x0026530E File Offset: 0x0026350E
		protected AttackBodyPart()
		{
		}

		// Token: 0x06004215 RID: 16917 RVA: 0x00265318 File Offset: 0x00263518
		protected AttackBodyPart(CombatSkillKey skillKey, int type) : base(skillKey, type, -1)
		{
		}

		// Token: 0x06004216 RID: 16918 RVA: 0x00265328 File Offset: 0x00263528
		public override void OnEnable(DataContext context)
		{
			this._affected = false;
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			bool isDirect = base.IsDirect;
			if (isDirect)
			{
				this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 77, base.SkillTemplateId, -1, -1, -1), EDataModifyType.Custom);
			}
			bool flag = !base.IsDirect;
			if (flag)
			{
				Events.RegisterHandler_CastAttackSkillBegin(new Events.OnCastAttackSkillBegin(this.OnCastAttackSkillBegin));
			}
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06004217 RID: 16919 RVA: 0x002653A8 File Offset: 0x002635A8
		public override void OnDisable(DataContext context)
		{
			bool flag = !base.IsDirect;
			if (flag)
			{
				Events.UnRegisterHandler_CastAttackSkillBegin(new Events.OnCastAttackSkillBegin(this.OnCastAttackSkillBegin));
			}
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06004218 RID: 16920 RVA: 0x002653E8 File Offset: 0x002635E8
		private void OnCastAttackSkillBegin(DataContext context, CombatCharacter attacker, CombatCharacter defender, short skillId)
		{
			bool flag = attacker.GetId() != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				base.AppendAffectedData(context, base.CharacterId, 69, EDataModifyType.AddPercent, base.SkillTemplateId);
			}
		}

		// Token: 0x06004219 RID: 16921 RVA: 0x00265434 File Offset: 0x00263634
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				bool flag2 = !interrupted && !base.IsDirect && base.PowerMatchAffectRequire((int)power, 0);
				if (flag2)
				{
					this.OnCastAffectPower(context);
				}
				bool affected = this._affected;
				if (affected)
				{
					base.ShowSpecialEffectTips(0);
				}
				base.RemoveSelf(context);
			}
		}

		// Token: 0x0600421A RID: 16922 RVA: 0x002654A0 File Offset: 0x002636A0
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

		// Token: 0x0600421B RID: 16923 RVA: 0x002654F8 File Offset: 0x002636F8
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.FieldId == 69 && dataKey.CombatSkillId == base.SkillTemplateId && this.BodyParts.Exist((sbyte)dataKey.CustomParam1);
			int result;
			if (flag)
			{
				this._affected = true;
				result = (int)this.ReverseAddDamagePercent;
			}
			else
			{
				result = 0;
			}
			return result;
		}

		// Token: 0x0600421C RID: 16924 RVA: 0x0026554D File Offset: 0x0026374D
		protected virtual void OnCastAffectPower(DataContext context)
		{
		}

		// Token: 0x0400137D RID: 4989
		protected sbyte[] BodyParts;

		// Token: 0x0400137E RID: 4990
		protected sbyte ReverseAddDamagePercent;

		// Token: 0x0400137F RID: 4991
		private bool _affected;
	}
}
