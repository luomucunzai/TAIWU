using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.Animal.Beast.Neigong
{
	// Token: 0x0200061F RID: 1567
	public class WanShouZhiWang : AnimalEffectBase
	{
		// Token: 0x170002EE RID: 750
		// (get) Token: 0x060045B7 RID: 17847 RVA: 0x00273395 File Offset: 0x00271595
		private int AddDamagePercentUnit
		{
			get
			{
				return base.IsElite ? 40 : 20;
			}
		}

		// Token: 0x060045B8 RID: 17848 RVA: 0x002733A5 File Offset: 0x002715A5
		public WanShouZhiWang()
		{
		}

		// Token: 0x060045B9 RID: 17849 RVA: 0x002733AF File Offset: 0x002715AF
		public WanShouZhiWang(CombatSkillKey skillKey) : base(skillKey)
		{
		}

		// Token: 0x060045BA RID: 17850 RVA: 0x002733BC File Offset: 0x002715BC
		public override void OnEnable(DataContext context)
		{
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 74, -1, -1, -1, -1), EDataModifyType.AddPercent);
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 69, -1, -1, -1, -1), EDataModifyType.AddPercent);
			Events.RegisterHandler_NormalAttackPrepareEnd(new Events.OnNormalAttackPrepareEnd(this.OnNormalAttackPrepareEnd));
			Events.RegisterHandler_AddDirectInjury(new Events.OnAddDirectInjury(this.OnAddDirectInjury));
			Events.RegisterHandler_NormalAttackAllEnd(new Events.OnNormalAttackAllEnd(this.OnNormalAttackAllEnd));
		}

		// Token: 0x060045BB RID: 17851 RVA: 0x00273447 File Offset: 0x00271647
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_NormalAttackPrepareEnd(new Events.OnNormalAttackPrepareEnd(this.OnNormalAttackPrepareEnd));
			Events.UnRegisterHandler_AddDirectInjury(new Events.OnAddDirectInjury(this.OnAddDirectInjury));
			Events.UnRegisterHandler_NormalAttackAllEnd(new Events.OnNormalAttackAllEnd(this.OnNormalAttackAllEnd));
		}

		// Token: 0x060045BC RID: 17852 RVA: 0x00273480 File Offset: 0x00271680
		private void OnNormalAttackPrepareEnd(DataContext context, int charId, bool isAlly)
		{
			bool flag = charId != base.CharacterId;
			if (!flag)
			{
				this._addDirectInjury = false;
			}
		}

		// Token: 0x060045BD RID: 17853 RVA: 0x002734A8 File Offset: 0x002716A8
		private void OnAddDirectInjury(DataContext context, int attackerId, int defenderId, bool isAlly, sbyte bodyPart, sbyte outerMarkCount, sbyte innerMarkCount, short combatSkillId)
		{
			bool flag = attackerId != base.CharacterId || outerMarkCount + innerMarkCount <= 0;
			if (!flag)
			{
				this._addDirectInjury = true;
			}
		}

		// Token: 0x060045BE RID: 17854 RVA: 0x002734DC File Offset: 0x002716DC
		private void OnNormalAttackAllEnd(DataContext context, CombatCharacter attacker, CombatCharacter defender)
		{
			bool flag = attacker.GetId() != base.CharacterId;
			if (!flag)
			{
				bool addDirectInjury = this._addDirectInjury;
				if (addDirectInjury)
				{
					this._perpetualAttackCount++;
					base.CombatChar.NormalAttackFree();
					base.ShowSpecialEffectTips(0);
				}
				else
				{
					this._perpetualAttackCount = 0;
				}
			}
		}

		// Token: 0x060045BF RID: 17855 RVA: 0x0027353C File Offset: 0x0027173C
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || dataKey.CombatSkillId >= 0;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				bool flag2 = dataKey.FieldId == 69;
				if (flag2)
				{
					result = this._perpetualAttackCount * this.AddDamagePercentUnit;
				}
				else
				{
					bool flag3 = dataKey.FieldId == 74;
					if (flag3)
					{
						result = this._perpetualAttackCount * -10;
					}
					else
					{
						result = 0;
					}
				}
			}
			return result;
		}

		// Token: 0x04001493 RID: 5267
		private const int ReduceHitOddsUnit = -10;

		// Token: 0x04001494 RID: 5268
		private bool _addDirectInjury;

		// Token: 0x04001495 RID: 5269
		private int _perpetualAttackCount;
	}
}
