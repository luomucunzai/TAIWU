using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Shixiangmen.Polearm
{
	// Token: 0x020003E9 RID: 1001
	public class EShaQiangFa : CombatSkillEffectBase
	{
		// Token: 0x0600382E RID: 14382 RVA: 0x002394D5 File Offset: 0x002376D5
		public EShaQiangFa()
		{
		}

		// Token: 0x0600382F RID: 14383 RVA: 0x002394DF File Offset: 0x002376DF
		public EShaQiangFa(CombatSkillKey skillKey) : base(skillKey, 6300, -1)
		{
		}

		// Token: 0x06003830 RID: 14384 RVA: 0x002394F0 File Offset: 0x002376F0
		public override void OnEnable(DataContext context)
		{
			Events.RegisterHandler_AddDirectFatalDamage(new Events.OnAddDirectFatalDamage(this.OnAddDirectFatalDamage));
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06003831 RID: 14385 RVA: 0x00239517 File Offset: 0x00237717
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_AddDirectFatalDamage(new Events.OnAddDirectFatalDamage(this.OnAddDirectFatalDamage));
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06003832 RID: 14386 RVA: 0x00239540 File Offset: 0x00237740
		private void OnAddDirectFatalDamage(CombatContext context, int outer, int inner)
		{
			bool flag = base.EffectCount <= 0 || (base.IsDirect ? context.Attacker : context.Defender) != base.CombatChar;
			if (!flag)
			{
				outer *= EShaQiangFa.AccumulateFatalPercent;
				inner *= EShaQiangFa.AccumulateFatalPercent;
				bool flag2 = outer <= 0 && inner <= 0;
				if (!flag2)
				{
					this._fatalDamage.Outer = this._fatalDamage.Outer + Math.Max(outer, 0);
					this._fatalDamage.Inner = this._fatalDamage.Inner + Math.Max(inner, 0);
					base.ReduceEffectCount(1);
					base.ShowSpecialEffectTips(0);
				}
			}
		}

		// Token: 0x06003833 RID: 14387 RVA: 0x002395F0 File Offset: 0x002377F0
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool _)
		{
			bool flag = !this.SkillKey.IsMatch(charId, skillId);
			if (!flag)
			{
				bool flag2 = base.PowerMatchAffectRequire((int)power, 0);
				if (flag2)
				{
					this.DoAffect(context);
				}
				else
				{
					bool isNonZero = this._fatalDamage.IsNonZero;
					if (isNonZero)
					{
						base.ShowSpecialEffectTips(2);
					}
				}
				this._fatalDamage = new ValueTuple<int, int>(0, 0);
			}
		}

		// Token: 0x06003834 RID: 14388 RVA: 0x00239658 File Offset: 0x00237858
		private void DoAffect(DataContext context)
		{
			base.AddMaxEffectCount(true);
			bool isNonZero = this._fatalDamage.IsNonZero;
			if (isNonZero)
			{
				base.ShowSpecialEffectTips(1);
			}
			bool flag = this._fatalDamage.Outer > 0;
			if (flag)
			{
				DomainManager.Combat.AddFatalDamageValue(context, base.CurrEnemyChar, this._fatalDamage.Outer, 0, -1, -1, EDamageType.None);
			}
			bool flag2 = this._fatalDamage.Inner > 0;
			if (flag2)
			{
				DomainManager.Combat.AddFatalDamageValue(context, base.CurrEnemyChar, this._fatalDamage.Inner, 1, -1, -1, EDamageType.None);
			}
		}

		// Token: 0x04001070 RID: 4208
		private static readonly CValuePercent AccumulateFatalPercent = 20;

		// Token: 0x04001071 RID: 4209
		private OuterAndInnerInts _fatalDamage;
	}
}
