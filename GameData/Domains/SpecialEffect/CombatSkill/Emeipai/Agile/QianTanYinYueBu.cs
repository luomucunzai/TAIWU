using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Agile;

namespace GameData.Domains.SpecialEffect.CombatSkill.Emeipai.Agile
{
	// Token: 0x0200056A RID: 1386
	public class QianTanYinYueBu : AgileSkillBase
	{
		// Token: 0x17000265 RID: 613
		// (get) Token: 0x060040EC RID: 16620 RVA: 0x00260C0B File Offset: 0x0025EE0B
		private int UnitAffectValue
		{
			get
			{
				return base.IsDirect ? 10 : 20;
			}
		}

		// Token: 0x17000266 RID: 614
		// (get) Token: 0x060040ED RID: 16621 RVA: 0x00260C1B File Offset: 0x0025EE1B
		private int MaxAffectValue
		{
			get
			{
				return base.IsDirect ? 60 : 120;
			}
		}

		// Token: 0x17000267 RID: 615
		// (get) Token: 0x060040EE RID: 16622 RVA: 0x00260C2B File Offset: 0x0025EE2B
		private int AffectDirection
		{
			get
			{
				return base.IsDirect ? -1 : 1;
			}
		}

		// Token: 0x060040EF RID: 16623 RVA: 0x00260C39 File Offset: 0x0025EE39
		public QianTanYinYueBu()
		{
		}

		// Token: 0x060040F0 RID: 16624 RVA: 0x00260C43 File Offset: 0x0025EE43
		public QianTanYinYueBu(CombatSkillKey skillKey) : base(skillKey, 2505)
		{
		}

		// Token: 0x060040F1 RID: 16625 RVA: 0x00260C54 File Offset: 0x0025EE54
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			bool isDirect = base.IsDirect;
			if (isDirect)
			{
				base.CreateAffectedData(321, EDataModifyType.AddPercent, -1);
			}
			else
			{
				base.CreateAffectedAllEnemyData(321, EDataModifyType.AddPercent, -1);
			}
			Events.RegisterHandler_NormalAttackCalcHitEnd(new Events.OnNormalAttackCalcHitEnd(this.OnNormalAttackCalcHitEnd));
		}

		// Token: 0x060040F2 RID: 16626 RVA: 0x00260CA4 File Offset: 0x0025EEA4
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_NormalAttackCalcHitEnd(new Events.OnNormalAttackCalcHitEnd(this.OnNormalAttackCalcHitEnd));
			base.OnDisable(context);
		}

		// Token: 0x060040F3 RID: 16627 RVA: 0x00260CC4 File Offset: 0x0025EEC4
		private void OnNormalAttackCalcHitEnd(DataContext context, CombatCharacter attacker, CombatCharacter defender, int pursueIndex, bool hit, bool isFightback, bool isMind)
		{
			bool flag = base.CombatChar != (base.IsDirect ? attacker : defender) || !hit || pursueIndex > 0;
			if (!flag)
			{
				this._affectValue = Math.Min(this._affectValue + this.UnitAffectValue, this.MaxAffectValue);
				base.ShowSpecialEffectTips(0);
			}
		}

		// Token: 0x060040F4 RID: 16628 RVA: 0x00260D20 File Offset: 0x0025EF20
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.FieldId != 321 || !base.CanAffect || !base.IsCurrent;
			int result;
			if (flag)
			{
				result = base.GetModifyValue(dataKey, currModifyValue);
			}
			else
			{
				result = this._affectValue * this.AffectDirection;
			}
			return result;
		}

		// Token: 0x0400131A RID: 4890
		private int _affectValue;
	}
}
