using System;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;

namespace GameData.Domains.SpecialEffect.SectStory.Yuanshan
{
	// Token: 0x020000E8 RID: 232
	public class VitalDemonA : VitalDemonEffectBase
	{
		// Token: 0x0600295E RID: 10590 RVA: 0x002005BF File Offset: 0x001FE7BF
		public VitalDemonA(int charId) : base(charId, 1748)
		{
		}

		// Token: 0x0600295F RID: 10591 RVA: 0x002005CF File Offset: 0x001FE7CF
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			Events.RegisterHandler_NormalAttackCalcHitEnd(new Events.OnNormalAttackCalcHitEnd(this.OnNormalAttackCalcHitEnd));
			Events.RegisterHandler_NormalAttackAllEnd(new Events.OnNormalAttackAllEnd(this.OnNormalAttackAllEnd));
		}

		// Token: 0x06002960 RID: 10592 RVA: 0x002005FE File Offset: 0x001FE7FE
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_NormalAttackCalcHitEnd(new Events.OnNormalAttackCalcHitEnd(this.OnNormalAttackCalcHitEnd));
			Events.UnRegisterHandler_NormalAttackAllEnd(new Events.OnNormalAttackAllEnd(this.OnNormalAttackAllEnd));
			base.OnDisable(context);
		}

		// Token: 0x06002961 RID: 10593 RVA: 0x00200630 File Offset: 0x001FE830
		private void OnNormalAttackCalcHitEnd(DataContext context, CombatCharacter attacker, CombatCharacter defender, int pursueIndex, bool hit, bool isFightback, bool isMind)
		{
			bool flag = attacker.IsAlly != base.CombatChar.IsAlly || defender.IsAlly == base.CombatChar.IsAlly;
			if (!flag)
			{
				this._anyHit = true;
			}
		}

		// Token: 0x06002962 RID: 10594 RVA: 0x00200674 File Offset: 0x001FE874
		private void OnNormalAttackAllEnd(DataContext context, CombatCharacter attacker, CombatCharacter defender)
		{
			bool flag = attacker.IsAlly != base.CombatChar.IsAlly || defender.IsAlly == base.CombatChar.IsAlly || !this._anyHit;
			if (!flag)
			{
				this._anyHit = false;
				base.ShowSpecialEffect(0);
				for (byte i = 0; i < 4; i += 1)
				{
					attacker.AbsorbNeiliAllocation(context, defender, i, 3);
				}
				short banableSkillId = defender.GetRandomBanableSkillId(context.Random, null, -1);
				bool flag2 = banableSkillId < 0;
				if (!flag2)
				{
					DomainManager.Combat.SilenceSkill(context, defender, banableSkillId, 240, 100);
					base.ShowSpecialEffect(1);
				}
			}
		}

		// Token: 0x04000CBB RID: 3259
		private const int AbsorbNeiliAllocationValue = 3;

		// Token: 0x04000CBC RID: 3260
		private const int SilenceFrame = 240;

		// Token: 0x04000CBD RID: 3261
		private bool _anyHit;
	}
}
