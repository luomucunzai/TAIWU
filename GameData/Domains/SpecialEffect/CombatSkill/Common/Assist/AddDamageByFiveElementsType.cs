using System;
using System.Collections.Generic;
using Config;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Common.Assist
{
	// Token: 0x020005AA RID: 1450
	public class AddDamageByFiveElementsType : AssistSkillBase
	{
		// Token: 0x0600431F RID: 17183 RVA: 0x0026A219 File Offset: 0x00268419
		protected AddDamageByFiveElementsType()
		{
		}

		// Token: 0x06004320 RID: 17184 RVA: 0x0026A223 File Offset: 0x00268423
		protected AddDamageByFiveElementsType(CombatSkillKey skillKey, int type) : base(skillKey, type)
		{
		}

		// Token: 0x06004321 RID: 17185 RVA: 0x0026A230 File Offset: 0x00268430
		public override void OnEnable(DataContext context)
		{
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 69, -1, -1, -1, -1), EDataModifyType.AddPercent);
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 102, -1, -1, -1, -1), EDataModifyType.AddPercent);
			Events.RegisterHandler_NormalAttackEnd(new Events.OnNormalAttackEnd(this.OnNormalAttackEnd));
			Events.RegisterHandler_AttackSkillAttackEnd(new Events.OnAttackSkillAttackEnd(this.OnAttackSkillAttackEnd));
		}

		// Token: 0x06004322 RID: 17186 RVA: 0x0026A2A9 File Offset: 0x002684A9
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_NormalAttackEnd(new Events.OnNormalAttackEnd(this.OnNormalAttackEnd));
			Events.UnRegisterHandler_AttackSkillAttackEnd(new Events.OnAttackSkillAttackEnd(this.OnAttackSkillAttackEnd));
		}

		// Token: 0x06004323 RID: 17187 RVA: 0x0026A2D0 File Offset: 0x002684D0
		private void OnNormalAttackEnd(DataContext context, CombatCharacter attacker, CombatCharacter defender, sbyte trickType, int pursueIndex, bool hit, bool isFightBack)
		{
			bool flag = !this._affected;
			if (!flag)
			{
				this._affected = false;
				base.ShowEffectTips(context);
			}
		}

		// Token: 0x06004324 RID: 17188 RVA: 0x0026A2FC File Offset: 0x002684FC
		private void OnAttackSkillAttackEnd(CombatContext context, sbyte hitType, bool hit, int index)
		{
			bool flag = !this._affected;
			if (!flag)
			{
				this._affected = false;
				base.ShowEffectTips(context);
			}
		}

		// Token: 0x06004325 RID: 17189 RVA: 0x0026A330 File Offset: 0x00268530
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
				sbyte fiveElementsType = (sbyte)NeiliType.Instance[base.CurrEnemyChar.GetNeiliType()].FiveElements;
				bool flag2 = (fiveElementsType == this.CounteringType && dataKey.FieldId == 69) || (fiveElementsType == this.CounteredType && dataKey.FieldId == 102);
				if (flag2)
				{
					this._affected = true;
					result = 50;
				}
				else
				{
					result = 0;
				}
			}
			return result;
		}

		// Token: 0x040013E5 RID: 5093
		private const sbyte AddDamage = 50;

		// Token: 0x040013E6 RID: 5094
		protected sbyte CounteringType;

		// Token: 0x040013E7 RID: 5095
		protected sbyte CounteredType;

		// Token: 0x040013E8 RID: 5096
		private bool _affected;
	}
}
