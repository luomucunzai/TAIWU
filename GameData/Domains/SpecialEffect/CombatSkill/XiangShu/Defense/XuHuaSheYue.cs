using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Defense;

namespace GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Defense
{
	// Token: 0x020002B5 RID: 693
	public class XuHuaSheYue : DefenseSkillBase
	{
		// Token: 0x06003219 RID: 12825 RVA: 0x0021DD54 File Offset: 0x0021BF54
		public XuHuaSheYue()
		{
		}

		// Token: 0x0600321A RID: 12826 RVA: 0x0021DD5E File Offset: 0x0021BF5E
		public XuHuaSheYue(CombatSkillKey skillKey) : base(skillKey, 16312)
		{
		}

		// Token: 0x0600321B RID: 12827 RVA: 0x0021DD70 File Offset: 0x0021BF70
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			this._effectTipsIndex = -1;
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 102, -1, -1, -1, -1), EDataModifyType.AddPercent);
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 71, -1, -1, -1, -1), EDataModifyType.AddPercent);
			Events.RegisterHandler_NormalAttackEnd(new Events.OnNormalAttackEnd(this.OnNormalAttackEnd));
			Events.RegisterHandler_AttackSkillAttackEnd(new Events.OnAttackSkillAttackEnd(this.OnAttackSkillAttackEnd));
		}

		// Token: 0x0600321C RID: 12828 RVA: 0x0021DDF8 File Offset: 0x0021BFF8
		public override void OnDisable(DataContext context)
		{
			base.OnDisable(context);
			Events.UnRegisterHandler_NormalAttackEnd(new Events.OnNormalAttackEnd(this.OnNormalAttackEnd));
			Events.UnRegisterHandler_AttackSkillAttackEnd(new Events.OnAttackSkillAttackEnd(this.OnAttackSkillAttackEnd));
		}

		// Token: 0x0600321D RID: 12829 RVA: 0x0021DE28 File Offset: 0x0021C028
		private void OnNormalAttackEnd(DataContext context, CombatCharacter attacker, CombatCharacter defender, sbyte trickType, int pursueIndex, bool hit, bool isFightBack)
		{
			bool flag = this._effectTipsIndex < 0;
			if (!flag)
			{
				base.ShowSpecialEffectTips((byte)this._effectTipsIndex);
				this._effectTipsIndex = -1;
			}
		}

		// Token: 0x0600321E RID: 12830 RVA: 0x0021DE5C File Offset: 0x0021C05C
		private void OnAttackSkillAttackEnd(CombatContext context, sbyte hitType, bool hit, int index)
		{
			bool flag = this._effectTipsIndex < 0;
			if (!flag)
			{
				base.ShowSpecialEffectTips((byte)this._effectTipsIndex);
				this._effectTipsIndex = -1;
			}
		}

		// Token: 0x0600321F RID: 12831 RVA: 0x0021DE90 File Offset: 0x0021C090
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || !base.CanAffect;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				int dieMarkCount = base.CurrEnemyChar.GetDefeatMarkCollection().DieMarkList.Count;
				bool flag2 = dieMarkCount == 0;
				if (flag2)
				{
					result = 0;
				}
				else
				{
					this._effectTipsIndex = ((dataKey.FieldId == 102) ? 0 : 1);
					result = ((dataKey.FieldId == 102) ? -15 : 15) * dieMarkCount;
				}
			}
			return result;
		}

		// Token: 0x04000ED6 RID: 3798
		private const sbyte ChangeDamagePercent = 15;

		// Token: 0x04000ED7 RID: 3799
		private int _effectTipsIndex;
	}
}
