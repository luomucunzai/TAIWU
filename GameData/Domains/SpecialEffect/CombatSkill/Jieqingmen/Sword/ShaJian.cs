using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Jieqingmen.Sword
{
	// Token: 0x020004E6 RID: 1254
	public class ShaJian : CombatSkillEffectBase
	{
		// Token: 0x1700023F RID: 575
		// (get) Token: 0x06003DF7 RID: 15863 RVA: 0x002542EC File Offset: 0x002524EC
		private CombatCharacter TrickChar
		{
			get
			{
				return base.IsDirect ? base.CombatChar : base.EnemyChar;
			}
		}

		// Token: 0x06003DF8 RID: 15864 RVA: 0x00254304 File Offset: 0x00252504
		public ShaJian()
		{
		}

		// Token: 0x06003DF9 RID: 15865 RVA: 0x0025430E File Offset: 0x0025250E
		public ShaJian(CombatSkillKey skillKey) : base(skillKey, 13204, -1)
		{
		}

		// Token: 0x06003DFA RID: 15866 RVA: 0x0025431F File Offset: 0x0025251F
		public override void OnEnable(DataContext context)
		{
			Events.RegisterHandler_PrepareSkillEnd(new Events.OnPrepareSkillEnd(this.OnPrepareSkillEnd));
			Events.RegisterHandler_AddDirectDamageValue(new Events.OnAddDirectDamageValue(this.OnAddDirectDamageValue));
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06003DFB RID: 15867 RVA: 0x00254358 File Offset: 0x00252558
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_PrepareSkillEnd(new Events.OnPrepareSkillEnd(this.OnPrepareSkillEnd));
			Events.UnRegisterHandler_AddDirectDamageValue(new Events.OnAddDirectDamageValue(this.OnAddDirectDamageValue));
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06003DFC RID: 15868 RVA: 0x00254394 File Offset: 0x00252594
		private void OnPrepareSkillEnd(DataContext context, int charId, bool isAlly, short skillId)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId || !DomainManager.Combat.InAttackRange(base.CombatChar);
			if (!flag)
			{
				this._extraFlawCount = (int)this.TrickChar.GetTrickCount(19);
				bool flag2 = this._extraFlawCount <= 0;
				if (!flag2)
				{
					base.AppendAffectedData(context, base.CharacterId, 84, EDataModifyType.Add, base.SkillTemplateId);
					base.ShowSpecialEffectTips(0);
				}
			}
		}

		// Token: 0x06003DFD RID: 15869 RVA: 0x00254418 File Offset: 0x00252618
		private void OnAddDirectDamageValue(DataContext context, int attackerId, int defenderId, sbyte bodyPart, bool isInner, int damageValue, short combatSkillId)
		{
			bool flag = this.SkillKey.IsMatch(attackerId, combatSkillId);
			if (flag)
			{
				this._makeDirectDamage += damageValue;
			}
		}

		// Token: 0x06003DFE RID: 15870 RVA: 0x00254448 File Offset: 0x00252648
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				int trickCount = this.TrickChar.GetContinueTricksAtStart(19);
				bool flag2 = trickCount > 0 && this._makeDirectDamage > 0;
				if (flag2)
				{
					DomainManager.Combat.RemoveTrick(context, this.TrickChar, 19, (byte)trickCount, true, -1);
					CValuePercent percent = 10 * trickCount;
					DomainManager.Combat.AddFatalDamageValue(context, base.CurrEnemyChar, this._makeDirectDamage * percent, -1, -1, -1, EDamageType.None);
					base.ShowSpecialEffectTips(1);
					this._makeDirectDamage = 0;
				}
				base.RemoveSelf(context);
			}
		}

		// Token: 0x06003DFF RID: 15871 RVA: 0x002544F4 File Offset: 0x002526F4
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
				bool flag2 = dataKey.FieldId == 84;
				if (flag2)
				{
					result = this._extraFlawCount;
				}
				else
				{
					result = 0;
				}
			}
			return result;
		}

		// Token: 0x0400124B RID: 4683
		private const int MakeFatalDamagePercentPerTrick = 10;

		// Token: 0x0400124C RID: 4684
		private int _extraFlawCount;

		// Token: 0x0400124D RID: 4685
		private int _makeDirectDamage;
	}
}
