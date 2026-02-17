using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Jingangzong.Special
{
	// Token: 0x020004AD RID: 1197
	public class SuiShiChu : CombatSkillEffectBase
	{
		// Token: 0x06003CB0 RID: 15536 RVA: 0x0024E84D File Offset: 0x0024CA4D
		public SuiShiChu()
		{
		}

		// Token: 0x06003CB1 RID: 15537 RVA: 0x0024E857 File Offset: 0x0024CA57
		public SuiShiChu(CombatSkillKey skillKey) : base(skillKey, 11300, -1)
		{
		}

		// Token: 0x06003CB2 RID: 15538 RVA: 0x0024E868 File Offset: 0x0024CA68
		public override void OnEnable(DataContext context)
		{
			base.CreateAffectedData(base.IsDirect ? 141 : 143, EDataModifyType.Add, -1);
			Events.RegisterHandler_CombatBegin(new Events.OnCombatBegin(this.OnCombatBegin));
			Events.RegisterHandler_NormalAttackCalcHitEnd(new Events.OnNormalAttackCalcHitEnd(this.OnNormalAttackCalcHitEnd));
			Events.RegisterHandler_NormalAttackAllEnd(new Events.OnNormalAttackAllEnd(this.OnNormalAttackAllEnd));
			Events.RegisterHandler_AttackSkillAttackHit(new Events.OnAttackSkillAttackHit(this.OnAttackSkillAttackHit));
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06003CB3 RID: 15539 RVA: 0x0024E8F0 File Offset: 0x0024CAF0
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CombatBegin(new Events.OnCombatBegin(this.OnCombatBegin));
			Events.UnRegisterHandler_NormalAttackCalcHitEnd(new Events.OnNormalAttackCalcHitEnd(this.OnNormalAttackCalcHitEnd));
			Events.UnRegisterHandler_NormalAttackAllEnd(new Events.OnNormalAttackAllEnd(this.OnNormalAttackAllEnd));
			Events.UnRegisterHandler_AttackSkillAttackHit(new Events.OnAttackSkillAttackHit(this.OnAttackSkillAttackHit));
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06003CB4 RID: 15540 RVA: 0x0024E958 File Offset: 0x0024CB58
		private void OnCombatBegin(DataContext context)
		{
			base.AppendAffectedAllEnemyData(context, 309, EDataModifyType.Add, -1);
		}

		// Token: 0x06003CB5 RID: 15541 RVA: 0x0024E96C File Offset: 0x0024CB6C
		private void OnNormalAttackCalcHitEnd(DataContext context, CombatCharacter attacker, CombatCharacter defender, int pursueIndex, bool hit, bool isFightback, bool isMind)
		{
			bool flag = (base.IsDirect ? attacker : defender) == base.CombatChar && !isFightback && hit;
			if (flag)
			{
				this._anyHit = true;
			}
		}

		// Token: 0x06003CB6 RID: 15542 RVA: 0x0024E9A4 File Offset: 0x0024CBA4
		private void OnNormalAttackAllEnd(DataContext context, CombatCharacter attacker, CombatCharacter defender)
		{
			bool flag = this._affected && this._anyHit && (base.IsDirect ? attacker : defender) == base.CombatChar;
			if (flag)
			{
				base.ReduceEffectCount(1);
			}
			this._affected = (this._anyHit = false);
		}

		// Token: 0x06003CB7 RID: 15543 RVA: 0x0024E9F8 File Offset: 0x0024CBF8
		private void OnAttackSkillAttackHit(DataContext context, CombatCharacter attacker, CombatCharacter defender, short skillId, int index, bool critical)
		{
			bool flag = (base.IsDirect ? attacker : defender) == base.CombatChar;
			if (flag)
			{
				this._anyHit = true;
			}
		}

		// Token: 0x06003CB8 RID: 15544 RVA: 0x0024EA28 File Offset: 0x0024CC28
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool _)
		{
			bool flag = this._affected && this._anyHit && (base.IsDirect ? (charId == base.CharacterId) : (isAlly != base.CombatChar.IsAlly));
			if (flag)
			{
				base.ReduceEffectCount(1);
			}
			this._affected = (this._anyHit = false);
			bool flag2 = this.SkillKey.IsMatch(charId, skillId) && base.PowerMatchAffectRequire((int)power, 0);
			if (flag2)
			{
				base.AddMaxEffectCount(true);
			}
		}

		// Token: 0x06003CB9 RID: 15545 RVA: 0x0024EAB4 File Offset: 0x0024CCB4
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = base.EffectCount <= 0;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				ushort fieldId = dataKey.FieldId;
				bool flag2 = fieldId == 141 || fieldId == 143;
				bool flag3 = flag2 && dataKey.CharId == base.CharacterId;
				if (flag3)
				{
					this._affected = true;
					result = 800;
				}
				else
				{
					bool flag4 = dataKey.FieldId == 309 && dataKey.CustomParam0 == (base.IsDirect ? 1 : 0) && dataKey.CustomParam1 == 0 && dataKey.CharId == base.CurrEnemyChar.GetId() && DomainManager.Combat.IsCurrentCombatCharacter(base.CombatChar);
					if (flag4)
					{
						base.ShowSpecialEffectTips(0);
						result = -5;
					}
					else
					{
						result = 0;
					}
				}
			}
			return result;
		}

		// Token: 0x040011DB RID: 4571
		private const short AddEquipAttack = 800;

		// Token: 0x040011DC RID: 4572
		private const sbyte ExtraReduceDurability = -5;

		// Token: 0x040011DD RID: 4573
		private bool _affected;

		// Token: 0x040011DE RID: 4574
		private bool _anyHit;
	}
}
