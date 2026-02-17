using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Defense;

namespace GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Defense
{
	// Token: 0x020002AA RID: 682
	public class DiZhaoXuanHuoYin : DefenseSkillBase
	{
		// Token: 0x060031DB RID: 12763 RVA: 0x0021C736 File Offset: 0x0021A936
		public DiZhaoXuanHuoYin()
		{
		}

		// Token: 0x060031DC RID: 12764 RVA: 0x0021C740 File Offset: 0x0021A940
		public DiZhaoXuanHuoYin(CombatSkillKey skillKey) : base(skillKey, 16313)
		{
		}

		// Token: 0x060031DD RID: 12765 RVA: 0x0021C750 File Offset: 0x0021A950
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			int[] enemyList = DomainManager.Combat.GetCharacterList(!base.CombatChar.IsAlly);
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 70, -1, -1, -1, -1), EDataModifyType.AddPercent);
			Events.RegisterHandler_NormalAttackBegin(new Events.OnNormalAttackBegin(this.OnNormalAttackBegin));
			Events.RegisterHandler_CastAttackSkillBegin(new Events.OnCastAttackSkillBegin(this.OnCastAttackSkillBegin));
			Events.RegisterHandler_BounceInjury(new Events.OnBounceInjury(this.OnBounceInjury));
		}

		// Token: 0x060031DE RID: 12766 RVA: 0x0021C7E0 File Offset: 0x0021A9E0
		public override void OnDisable(DataContext context)
		{
			base.OnDisable(context);
			Events.UnRegisterHandler_NormalAttackBegin(new Events.OnNormalAttackBegin(this.OnNormalAttackBegin));
			Events.UnRegisterHandler_CastAttackSkillBegin(new Events.OnCastAttackSkillBegin(this.OnCastAttackSkillBegin));
			Events.UnRegisterHandler_BounceInjury(new Events.OnBounceInjury(this.OnBounceInjury));
		}

		// Token: 0x060031DF RID: 12767 RVA: 0x0021C82C File Offset: 0x0021AA2C
		private void OnNormalAttackBegin(DataContext context, CombatCharacter attacker, CombatCharacter defender, sbyte trickType, int pursueIndex)
		{
			bool flag = defender != base.CombatChar;
			if (!flag)
			{
				this._injuriesBeforeAttacked = base.CombatChar.GetInjuries();
			}
		}

		// Token: 0x060031E0 RID: 12768 RVA: 0x0021C860 File Offset: 0x0021AA60
		private void OnCastAttackSkillBegin(DataContext context, CombatCharacter attacker, CombatCharacter defender, short skillId)
		{
			bool flag = defender != base.CombatChar;
			if (!flag)
			{
				this._injuriesBeforeAttacked = base.CombatChar.GetInjuries();
			}
		}

		// Token: 0x060031E1 RID: 12769 RVA: 0x0021C894 File Offset: 0x0021AA94
		private void OnBounceInjury(DataContext context, int attackerId, int defenderId, bool isAlly, sbyte bodyPart, sbyte outerMarkCount, sbyte innerMarkCount)
		{
			bool flag = !this._affected || attackerId != base.CharacterId;
			if (!flag)
			{
				this._affected = false;
				base.ShowSpecialEffectTips(0);
			}
		}

		// Token: 0x060031E2 RID: 12770 RVA: 0x0021C8D0 File Offset: 0x0021AAD0
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = !base.CanAffect;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				bool flag2 = dataKey.FieldId == 70;
				if (flag2)
				{
					sbyte bodyPart = (sbyte)dataKey.CustomParam1;
					bool inner = dataKey.CustomParam0 == 1;
					int addValue = (int)(15 * this._injuriesBeforeAttacked.Get(bodyPart, inner));
					bool flag3 = addValue > 0;
					if (flag3)
					{
						this._affected = true;
					}
					result = addValue;
				}
				else
				{
					result = 0;
				}
			}
			return result;
		}

		// Token: 0x04000EC6 RID: 3782
		private const sbyte AddDamagePercent = 15;

		// Token: 0x04000EC7 RID: 3783
		private Injuries _injuriesBeforeAttacked;

		// Token: 0x04000EC8 RID: 3784
		private bool _affected;
	}
}
