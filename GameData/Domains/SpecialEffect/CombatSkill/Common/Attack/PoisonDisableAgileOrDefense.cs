using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Common.Attack
{
	// Token: 0x0200059E RID: 1438
	public class PoisonDisableAgileOrDefense : CombatSkillEffectBase
	{
		// Token: 0x17000299 RID: 665
		// (get) Token: 0x060042B9 RID: 17081 RVA: 0x00268387 File Offset: 0x00266587
		private ushort CanAffectFieldId
		{
			get
			{
				return base.IsDirect ? 285 : 287;
			}
		}

		// Token: 0x060042BA RID: 17082 RVA: 0x0026839D File Offset: 0x0026659D
		protected PoisonDisableAgileOrDefense()
		{
		}

		// Token: 0x060042BB RID: 17083 RVA: 0x002683A7 File Offset: 0x002665A7
		protected PoisonDisableAgileOrDefense(CombatSkillKey skillKey, int type) : base(skillKey, type, -1)
		{
		}

		// Token: 0x060042BC RID: 17084 RVA: 0x002683B4 File Offset: 0x002665B4
		public override void OnEnable(DataContext context)
		{
			base.CreateAffectedAllEnemyData(this.CanAffectFieldId, EDataModifyType.Custom, -1);
			Events.RegisterHandler_PrepareSkillEnd(new Events.OnPrepareSkillEnd(this.OnPrepareSkillEnd));
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x060042BD RID: 17085 RVA: 0x002683EA File Offset: 0x002665EA
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_PrepareSkillEnd(new Events.OnPrepareSkillEnd(this.OnPrepareSkillEnd));
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x060042BE RID: 17086 RVA: 0x00268414 File Offset: 0x00266614
		private void OnPrepareSkillEnd(DataContext context, int charId, bool isAlly, short skillId)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId || !DomainManager.Combat.InAttackRange(base.CombatChar) || base.CurrEnemyChar.GetDefeatMarkCollection().PoisonMarkList[(int)this.RequirePoisonType] <= 0;
			if (!flag)
			{
				this._disableSkills = true;
				base.InvalidateCache(context, base.CurrEnemyChar.GetId(), this.CanAffectFieldId);
				base.ShowSpecialEffectTips(0);
			}
		}

		// Token: 0x060042BF RID: 17087 RVA: 0x00268498 File Offset: 0x00266698
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				this._disableSkills = false;
				base.RemoveSelf(context);
			}
		}

		// Token: 0x060042C0 RID: 17088 RVA: 0x002684D4 File Offset: 0x002666D4
		public override bool GetModifiedValue(AffectedDataKey dataKey, bool dataValue)
		{
			ushort fieldId = dataKey.FieldId;
			bool flag = fieldId == 285 || fieldId == 287;
			bool flag2 = !flag;
			bool result;
			if (flag2)
			{
				result = dataValue;
			}
			else
			{
				result = (dataValue && !this._disableSkills);
			}
			return result;
		}

		// Token: 0x040013BF RID: 5055
		protected sbyte RequirePoisonType;

		// Token: 0x040013C0 RID: 5056
		private bool _disableSkills;
	}
}
