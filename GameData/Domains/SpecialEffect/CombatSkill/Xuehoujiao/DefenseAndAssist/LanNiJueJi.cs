using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Defense;

namespace GameData.Domains.SpecialEffect.CombatSkill.Xuehoujiao.DefenseAndAssist
{
	// Token: 0x02000240 RID: 576
	public class LanNiJueJi : DefenseSkillBase
	{
		// Token: 0x06002FB9 RID: 12217 RVA: 0x002142AD File Offset: 0x002124AD
		public LanNiJueJi()
		{
		}

		// Token: 0x06002FBA RID: 12218 RVA: 0x002142B7 File Offset: 0x002124B7
		public LanNiJueJi(CombatSkillKey skillKey) : base(skillKey, 15700)
		{
		}

		// Token: 0x06002FBB RID: 12219 RVA: 0x002142C8 File Offset: 0x002124C8
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 102, -1, -1, -1, -1), EDataModifyType.AddPercent);
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, base.IsDirect ? 98 : 99, -1, -1, -1, -1), EDataModifyType.TotalPercent);
			Events.RegisterHandler_NormalAttackBegin(new Events.OnNormalAttackBegin(this.OnNormalAttackBegin));
			Events.RegisterHandler_NormalAttackEnd(new Events.OnNormalAttackEnd(this.OnNormalAttackEnd));
			Events.RegisterHandler_NormalAttackAllEnd(new Events.OnNormalAttackAllEnd(this.OnNormalAttackAllEnd));
		}

		// Token: 0x06002FBC RID: 12220 RVA: 0x00214368 File Offset: 0x00212568
		public override void OnDisable(DataContext context)
		{
			base.OnDisable(context);
			Events.UnRegisterHandler_NormalAttackBegin(new Events.OnNormalAttackBegin(this.OnNormalAttackBegin));
			Events.UnRegisterHandler_NormalAttackEnd(new Events.OnNormalAttackEnd(this.OnNormalAttackEnd));
			Events.UnRegisterHandler_NormalAttackAllEnd(new Events.OnNormalAttackAllEnd(this.OnNormalAttackAllEnd));
		}

		// Token: 0x06002FBD RID: 12221 RVA: 0x002143B4 File Offset: 0x002125B4
		private void OnNormalAttackBegin(DataContext context, CombatCharacter attacker, CombatCharacter defender, sbyte trickType, int pursueIndex)
		{
			bool flag = pursueIndex != 1 || base.CombatChar != defender || !base.CanAffect;
			if (!flag)
			{
				base.ShowSpecialEffectTips(1);
			}
		}

		// Token: 0x06002FBE RID: 12222 RVA: 0x002143EC File Offset: 0x002125EC
		private void OnNormalAttackEnd(DataContext context, CombatCharacter attacker, CombatCharacter defender, sbyte trickType, int pursueIndex, bool hit, bool isFightBack)
		{
			bool flag = pursueIndex != 0 || base.CombatChar != defender || !base.CanAffect;
			if (!flag)
			{
				this._affectingPenetrate = true;
			}
		}

		// Token: 0x06002FBF RID: 12223 RVA: 0x00214420 File Offset: 0x00212620
		private void OnNormalAttackAllEnd(DataContext context, CombatCharacter attacker, CombatCharacter defender)
		{
			bool flag = base.CombatChar != defender;
			if (!flag)
			{
				this._affectingPenetrate = false;
			}
		}

		// Token: 0x06002FC0 RID: 12224 RVA: 0x00214448 File Offset: 0x00212648
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.CharId != base.CharacterId;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				bool flag2 = dataKey.FieldId == 102 && dataKey.CustomParam0 == (base.IsDirect ? 0 : 1) && base.CanAffect;
				if (flag2)
				{
					base.ShowSpecialEffectTips(0);
					result = -30;
				}
				else
				{
					bool flag3 = dataKey.FieldId == (base.IsDirect ? 98 : 99) && this._affectingPenetrate;
					if (flag3)
					{
						result = -80;
					}
					else
					{
						result = 0;
					}
				}
			}
			return result;
		}

		// Token: 0x04000E23 RID: 3619
		private const sbyte ReduceDamagePercent = -30;

		// Token: 0x04000E24 RID: 3620
		private const sbyte ReducePenetrate = -80;

		// Token: 0x04000E25 RID: 3621
		private bool _affectingPenetrate;
	}
}
