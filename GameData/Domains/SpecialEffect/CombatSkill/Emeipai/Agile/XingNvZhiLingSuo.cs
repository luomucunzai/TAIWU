using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Agile;

namespace GameData.Domains.SpecialEffect.CombatSkill.Emeipai.Agile
{
	// Token: 0x0200056C RID: 1388
	public class XingNvZhiLingSuo : AgileSkillBase
	{
		// Token: 0x060040F7 RID: 16631 RVA: 0x00260D90 File Offset: 0x0025EF90
		public XingNvZhiLingSuo()
		{
		}

		// Token: 0x060040F8 RID: 16632 RVA: 0x00260D9A File Offset: 0x0025EF9A
		public XingNvZhiLingSuo(CombatSkillKey skillKey) : base(skillKey, 2504)
		{
			this.ListenCanAffectChange = true;
		}

		// Token: 0x060040F9 RID: 16633 RVA: 0x00260DB1 File Offset: 0x0025EFB1
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			base.CreateAffectedData(157, EDataModifyType.Custom, -1);
			Events.RegisterHandler_NormalAttackCalcHitEnd(new Events.OnNormalAttackCalcHitEnd(this.OnNormalAttackCalcHitEnd));
			Events.RegisterHandler_NormalAttackAllEnd(new Events.OnNormalAttackAllEnd(this.OnNormalAttackAllEnd));
		}

		// Token: 0x060040FA RID: 16634 RVA: 0x00260DEE File Offset: 0x0025EFEE
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_NormalAttackCalcHitEnd(new Events.OnNormalAttackCalcHitEnd(this.OnNormalAttackCalcHitEnd));
			Events.UnRegisterHandler_NormalAttackAllEnd(new Events.OnNormalAttackAllEnd(this.OnNormalAttackAllEnd));
			base.OnDisable(context);
		}

		// Token: 0x060040FB RID: 16635 RVA: 0x00260E20 File Offset: 0x0025F020
		private void OnNormalAttackCalcHitEnd(DataContext context, CombatCharacter attacker, CombatCharacter defender, int pursueIndex, bool hit, bool isFightback, bool isMind)
		{
			bool flag = (base.IsDirect ? attacker : defender) != base.CombatChar || !hit || pursueIndex > 0;
			if (!flag)
			{
				this._needAffect = true;
			}
		}

		// Token: 0x060040FC RID: 16636 RVA: 0x00260E5C File Offset: 0x0025F05C
		private void OnNormalAttackAllEnd(DataContext context, CombatCharacter attacker, CombatCharacter defender)
		{
			bool flag = (base.IsDirect ? attacker : defender) != base.CombatChar || !this._needAffect;
			if (!flag)
			{
				base.ShowSpecialEffectTips(0);
				this.DoChangeDistance(context);
				this._needAffect = false;
			}
		}

		// Token: 0x060040FD RID: 16637 RVA: 0x00260EA8 File Offset: 0x0025F0A8
		private void DoChangeDistance(DataContext context)
		{
			short target = base.CombatChar.GetAttackRange().Average;
			short current = DomainManager.Combat.GetCurrentDistance();
			bool flag = current == target;
			if (!flag)
			{
				int delta = Math.Min(2, Math.Abs((int)(target - current))) * Math.Sign((int)(target - current));
				DomainManager.Combat.ChangeDistance(context, base.CombatChar, delta);
			}
		}

		// Token: 0x060040FE RID: 16638 RVA: 0x00260F0C File Offset: 0x0025F10C
		public override bool GetModifiedValue(AffectedDataKey dataKey, bool dataValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || dataKey.FieldId != 157 || !this._needAffect;
			return flag && base.GetModifiedValue(dataKey, dataValue);
		}

		// Token: 0x0400131B RID: 4891
		private const int MaxChangeDistance = 2;

		// Token: 0x0400131C RID: 4892
		private bool _needAffect;
	}
}
