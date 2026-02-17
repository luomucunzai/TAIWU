using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Agile;

namespace GameData.Domains.SpecialEffect.CombatSkill.Zhujianshanzhuang.Agile
{
	// Token: 0x020001E8 RID: 488
	public class BaFangZhuShi : AgileSkillBase
	{
		// Token: 0x06002E11 RID: 11793 RVA: 0x0020D9B7 File Offset: 0x0020BBB7
		public BaFangZhuShi()
		{
		}

		// Token: 0x06002E12 RID: 11794 RVA: 0x0020D9C1 File Offset: 0x0020BBC1
		public BaFangZhuShi(CombatSkillKey skillKey) : base(skillKey, 9502)
		{
		}

		// Token: 0x06002E13 RID: 11795 RVA: 0x0020D9D1 File Offset: 0x0020BBD1
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			base.CreateAffectedData(74, EDataModifyType.TotalPercent, -1);
			Events.RegisterHandler_NormalAttackCalcHitEnd(new Events.OnNormalAttackCalcHitEnd(this.OnNormalAttackCalcHitEnd));
			Events.RegisterHandler_NormalAttackAllEnd(new Events.OnNormalAttackAllEnd(this.OnNormalAttackAllEnd));
		}

		// Token: 0x06002E14 RID: 11796 RVA: 0x0020DA0B File Offset: 0x0020BC0B
		public override void OnDisable(DataContext context)
		{
			base.OnDisable(context);
			Events.UnRegisterHandler_NormalAttackCalcHitEnd(new Events.OnNormalAttackCalcHitEnd(this.OnNormalAttackCalcHitEnd));
			Events.UnRegisterHandler_NormalAttackAllEnd(new Events.OnNormalAttackAllEnd(this.OnNormalAttackAllEnd));
		}

		// Token: 0x06002E15 RID: 11797 RVA: 0x0020DA3C File Offset: 0x0020BC3C
		private void OnNormalAttackCalcHitEnd(DataContext context, CombatCharacter attacker, CombatCharacter defender, int pursueIndex, bool hit, bool isFightback, bool isMind)
		{
			bool flag = attacker.GetId() != base.CharacterId || hit != base.IsDirect || attacker.IsAutoNormalAttacking || !base.CanAffect || pursueIndex > 0 || this._changingHitOdds;
			if (!flag)
			{
				this._changingHitOdds = true;
				base.CombatChar.NormalAttackFree();
				base.ShowSpecialEffectTips(0);
			}
		}

		// Token: 0x06002E16 RID: 11798 RVA: 0x0020DAA4 File Offset: 0x0020BCA4
		private void OnNormalAttackAllEnd(DataContext context, CombatCharacter attacker, CombatCharacter defender)
		{
			bool flag = attacker.GetId() != base.CharacterId || !attacker.IsAutoNormalAttacking;
			if (!flag)
			{
				this._changingHitOdds = false;
			}
		}

		// Token: 0x06002E17 RID: 11799 RVA: 0x0020DADC File Offset: 0x0020BCDC
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || dataKey.FieldId != 74 || !this._changingHitOdds;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				result = (base.IsDirect ? -50 : 50);
			}
			return result;
		}

		// Token: 0x04000DBB RID: 3515
		private const int ChangeHitOdds = 50;

		// Token: 0x04000DBC RID: 3516
		private bool _changingHitOdds;
	}
}
