using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Assist;

namespace GameData.Domains.SpecialEffect.CombatSkill.Kongsangpai.DefenseAndAssist
{
	// Token: 0x0200049E RID: 1182
	public class TouGuQiePouFa : AssistSkillBase
	{
		// Token: 0x06003C64 RID: 15460 RVA: 0x0024D4D4 File Offset: 0x0024B6D4
		public TouGuQiePouFa()
		{
		}

		// Token: 0x06003C65 RID: 15461 RVA: 0x0024D4DE File Offset: 0x0024B6DE
		public TouGuQiePouFa(CombatSkillKey skillKey) : base(skillKey, 10702)
		{
		}

		// Token: 0x06003C66 RID: 15462 RVA: 0x0024D4F0 File Offset: 0x0024B6F0
		public override void OnEnable(DataContext context)
		{
			base.CreateAffectedData(69, EDataModifyType.AddPercent, -1);
			Events.RegisterHandler_NormalAttackBegin(new Events.OnNormalAttackBegin(this.OnNormalAttackBegin));
			Events.RegisterHandler_NormalAttackEnd(new Events.OnNormalAttackEnd(this.OnNormalAttackEnd));
			Events.RegisterHandler_PrepareSkillEnd(new Events.OnPrepareSkillEnd(this.OnPrepareSkillEnd));
			Events.RegisterHandler_AttackSkillAttackEnd(new Events.OnAttackSkillAttackEnd(this.OnAttackSkillAttackEnd));
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06003C67 RID: 15463 RVA: 0x0024D564 File Offset: 0x0024B764
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_NormalAttackBegin(new Events.OnNormalAttackBegin(this.OnNormalAttackBegin));
			Events.UnRegisterHandler_NormalAttackEnd(new Events.OnNormalAttackEnd(this.OnNormalAttackEnd));
			Events.UnRegisterHandler_PrepareSkillEnd(new Events.OnPrepareSkillEnd(this.OnPrepareSkillEnd));
			Events.UnRegisterHandler_AttackSkillAttackEnd(new Events.OnAttackSkillAttackEnd(this.OnAttackSkillAttackEnd));
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06003C68 RID: 15464 RVA: 0x0024D5CC File Offset: 0x0024B7CC
		private void OnNormalAttackBegin(DataContext context, CombatCharacter attacker, CombatCharacter defender, sbyte trickType, int pursueIndex)
		{
			bool flag = attacker.GetId() != base.CharacterId;
			if (!flag)
			{
				this._canAffect = (base.CanAffect && base.IsCurrent && this.IsBodyPartCanAffect(base.CombatChar.NormalAttackBodyPart));
			}
		}

		// Token: 0x06003C69 RID: 15465 RVA: 0x0024D61C File Offset: 0x0024B81C
		private void OnNormalAttackEnd(DataContext context, CombatCharacter attacker, CombatCharacter defender, sbyte trickType, int pursueIndex, bool hit, bool isFightBack)
		{
			bool flag = attacker.GetId() != base.CharacterId;
			if (!flag)
			{
				this._canAffect = false;
				bool flag2 = !this._affected;
				if (!flag2)
				{
					this._affected = false;
					base.ShowSpecialEffectTips(0);
				}
			}
		}

		// Token: 0x06003C6A RID: 15466 RVA: 0x0024D668 File Offset: 0x0024B868
		private void OnPrepareSkillEnd(DataContext context, int charId, bool isAlly, short skillId)
		{
			bool flag = charId != base.CharacterId;
			if (!flag)
			{
				this._canAffect = (base.CanAffect && base.IsCurrent && this.IsBodyPartCanAffect(base.CombatChar.SkillAttackBodyPart));
			}
		}

		// Token: 0x06003C6B RID: 15467 RVA: 0x0024D6B4 File Offset: 0x0024B8B4
		private void OnAttackSkillAttackEnd(CombatContext context, sbyte hitType, bool hit, int index)
		{
			bool flag = context.AttackerId != base.CharacterId || !this._affected;
			if (!flag)
			{
				this._affected = false;
				base.ShowSpecialEffectTips(0);
			}
		}

		// Token: 0x06003C6C RID: 15468 RVA: 0x0024D6F4 File Offset: 0x0024B8F4
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId;
			if (!flag)
			{
				this._canAffect = false;
			}
		}

		// Token: 0x06003C6D RID: 15469 RVA: 0x0024D71C File Offset: 0x0024B91C
		private bool IsBodyPartCanAffect(sbyte bodyPart)
		{
			bool flag = bodyPart < 0 || bodyPart >= 7;
			bool flag2 = flag;
			bool result;
			if (flag2)
			{
				result = false;
			}
			else
			{
				ValueTuple<sbyte, sbyte> injuries = base.CurrEnemyChar.GetInjuries().Get(bodyPart);
				result = (injuries.Item1 <= 0 && injuries.Item2 <= 0);
			}
			return result;
		}

		// Token: 0x06003C6E RID: 15470 RVA: 0x0024D778 File Offset: 0x0024B978
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = !this._canAffect || dataKey.CustomParam0 != (base.IsDirect ? 0 : 1) || dataKey.FieldId != 69;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				this._affected = true;
				result = 45;
			}
			return result;
		}

		// Token: 0x040011C7 RID: 4551
		private const sbyte AddDamagePercent = 45;

		// Token: 0x040011C8 RID: 4552
		private bool _canAffect;

		// Token: 0x040011C9 RID: 4553
		private bool _affected;
	}
}
