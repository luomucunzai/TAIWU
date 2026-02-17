using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Assist;

namespace GameData.Domains.SpecialEffect.CombatSkill.Wuxianjiao.DefenseAndAssist
{
	// Token: 0x020003AB RID: 939
	public class TianSheHuanGuGong : AssistSkillBase
	{
		// Token: 0x060036C9 RID: 14025 RVA: 0x00232335 File Offset: 0x00230535
		public TianSheHuanGuGong()
		{
		}

		// Token: 0x060036CA RID: 14026 RVA: 0x0023233F File Offset: 0x0023053F
		public TianSheHuanGuGong(CombatSkillKey skillKey) : base(skillKey, 12808)
		{
			this.SetConstAffectingOnCombatBegin = true;
		}

		// Token: 0x060036CB RID: 14027 RVA: 0x00232358 File Offset: 0x00230558
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			this._canAffect = false;
			this._affecting = false;
			base.CreateAffectedData(191, EDataModifyType.Custom, -1);
			base.CreateAffectedData(192, EDataModifyType.Custom, -1);
			Events.RegisterHandler_CombatBegin(new Events.OnCombatBegin(this.OnCombatBegin));
			Events.RegisterHandler_NormalAttackBegin(new Events.OnNormalAttackBegin(this.OnNormalAttackBegin));
			Events.RegisterHandler_NormalAttackEnd(new Events.OnNormalAttackEnd(this.OnNormalAttackEnd));
			Events.RegisterHandler_CastAttackSkillBegin(new Events.OnCastAttackSkillBegin(this.OnCastAttackSkillBegin));
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x060036CC RID: 14028 RVA: 0x002323F4 File Offset: 0x002305F4
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CombatBegin(new Events.OnCombatBegin(this.OnCombatBegin));
			Events.UnRegisterHandler_NormalAttackBegin(new Events.OnNormalAttackBegin(this.OnNormalAttackBegin));
			Events.UnRegisterHandler_NormalAttackEnd(new Events.OnNormalAttackEnd(this.OnNormalAttackEnd));
			Events.UnRegisterHandler_CastAttackSkillBegin(new Events.OnCastAttackSkillBegin(this.OnCastAttackSkillBegin));
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
			base.OnDisable(context);
		}

		// Token: 0x060036CD RID: 14029 RVA: 0x00232464 File Offset: 0x00230664
		private void OnCombatBegin(DataContext context)
		{
			this.UpdateAffecting(context);
		}

		// Token: 0x060036CE RID: 14030 RVA: 0x0023246E File Offset: 0x0023066E
		protected override void OnCanUseChanged(DataContext context, DataUid dataUid)
		{
			this.UpdateAffecting(context);
		}

		// Token: 0x060036CF RID: 14031 RVA: 0x00232478 File Offset: 0x00230678
		private void UpdateAffecting(DataContext context)
		{
			bool affecting = base.CanAffect;
			bool flag = affecting == this._affecting;
			if (!flag)
			{
				this._affecting = affecting;
				List<short> speedList = base.IsDirect ? base.CombatChar.OuterInjuryAutoHealSpeeds : base.CombatChar.InnerInjuryAutoHealSpeeds;
				base.SetConstAffecting(context, this._affecting);
				bool affecting2 = this._affecting;
				if (affecting2)
				{
					speedList.Add(1);
				}
				else
				{
					speedList.Remove(1);
					bool flag2 = speedList.Count <= 0;
					if (flag2)
					{
						base.CombatChar.ClearInjuryAutoHealProgress(context, !base.IsDirect);
					}
				}
			}
		}

		// Token: 0x060036D0 RID: 14032 RVA: 0x0023251C File Offset: 0x0023071C
		private void OnNormalAttackBegin(DataContext context, CombatCharacter attacker, CombatCharacter defender, sbyte trickType, int pursueIndex)
		{
			bool flag = defender != base.CombatChar;
			if (!flag)
			{
				this._canAffect = (attacker.NormalAttackBodyPart >= 0 && base.CombatChar.GetInjuries().Get(attacker.NormalAttackBodyPart, !base.IsDirect) < 4);
			}
		}

		// Token: 0x060036D1 RID: 14033 RVA: 0x00232574 File Offset: 0x00230774
		private void OnNormalAttackEnd(DataContext context, CombatCharacter attacker, CombatCharacter defender, sbyte trickType, int pursueIndex, bool hit, bool isFightBack)
		{
			bool flag = defender != base.CombatChar;
			if (!flag)
			{
				this._canAffect = false;
			}
		}

		// Token: 0x060036D2 RID: 14034 RVA: 0x0023259C File Offset: 0x0023079C
		private void OnCastAttackSkillBegin(DataContext context, CombatCharacter attacker, CombatCharacter defender, short skillId)
		{
			bool flag = defender != base.CombatChar;
			if (!flag)
			{
				this._canAffect = (attacker.SkillAttackBodyPart >= 0 && base.CombatChar.GetInjuries().Get(attacker.SkillAttackBodyPart, !base.IsDirect) < 4);
			}
		}

		// Token: 0x060036D3 RID: 14035 RVA: 0x002325F4 File Offset: 0x002307F4
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = isAlly != base.CombatChar.IsAlly && DomainManager.Combat.GetCombatCharacter(base.CombatChar.IsAlly, true) == base.CombatChar;
			if (flag)
			{
				this._canAffect = false;
			}
		}

		// Token: 0x060036D4 RID: 14036 RVA: 0x0023263C File Offset: 0x0023083C
		public override int GetModifiedValue(AffectedDataKey dataKey, int dataValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || dataValue <= 0 || !base.CanAffect || !this._canAffect || dataKey.CustomParam0 != (base.IsDirect ? 0 : 1);
			int result;
			if (flag)
			{
				result = dataValue;
			}
			else
			{
				ushort fieldId = dataKey.FieldId;
				bool flag2 = fieldId - 191 <= 1;
				bool flag3 = flag2;
				if (flag3)
				{
					dataValue = 0;
					base.ShowSpecialEffectTips(0);
				}
				result = dataValue;
			}
			return result;
		}

		// Token: 0x04000FF9 RID: 4089
		private const sbyte AutoHealSpeed = 1;

		// Token: 0x04000FFA RID: 4090
		private const sbyte FatalDamageRequireInjuryCount = 4;

		// Token: 0x04000FFB RID: 4091
		private bool _canAffect;

		// Token: 0x04000FFC RID: 4092
		private bool _affecting;
	}
}
