using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Assist;

namespace GameData.Domains.SpecialEffect.CombatSkill.Baihuagu.DefenseAndAssist
{
	// Token: 0x020005D8 RID: 1496
	public class SanShiLiuShouYiZhenFa : AssistSkillBase
	{
		// Token: 0x170002BD RID: 701
		// (get) Token: 0x06004432 RID: 17458 RVA: 0x0026E7F0 File Offset: 0x0026C9F0
		private int CriticalOddsAddPercent
		{
			get
			{
				return base.IsDirect ? 40 : -40;
			}
		}

		// Token: 0x06004433 RID: 17459 RVA: 0x0026E800 File Offset: 0x0026CA00
		public SanShiLiuShouYiZhenFa()
		{
		}

		// Token: 0x06004434 RID: 17460 RVA: 0x0026E80A File Offset: 0x0026CA0A
		public SanShiLiuShouYiZhenFa(CombatSkillKey skillKey) : base(skillKey, 3600)
		{
			this.SetConstAffectingOnCombatBegin = true;
		}

		// Token: 0x06004435 RID: 17461 RVA: 0x0026E824 File Offset: 0x0026CA24
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			for (sbyte hitType = 0; hitType < 4; hitType += 1)
			{
				base.CreateAffectedData((ushort)((base.IsDirect ? 56 : 90) + hitType), EDataModifyType.AddPercent, -1);
			}
			Events.RegisterHandler_CombatBegin(new Events.OnCombatBegin(this.OnCombatBegin));
			Events.RegisterHandler_NormalAttackBegin(new Events.OnNormalAttackBegin(this.OnNormalAttackBegin));
		}

		// Token: 0x06004436 RID: 17462 RVA: 0x0026E889 File Offset: 0x0026CA89
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CombatBegin(new Events.OnCombatBegin(this.OnCombatBegin));
			Events.UnRegisterHandler_NormalAttackBegin(new Events.OnNormalAttackBegin(this.OnNormalAttackBegin));
			base.OnDisable(context);
		}

		// Token: 0x06004437 RID: 17463 RVA: 0x0026E8B8 File Offset: 0x0026CAB8
		private void OnCombatBegin(DataContext context)
		{
			bool isDirect = base.IsDirect;
			if (isDirect)
			{
				base.AppendAffectedData(context, 254, EDataModifyType.AddPercent, -1);
			}
			else
			{
				base.AppendAffectedAllEnemyData(context, 254, EDataModifyType.AddPercent, -1);
			}
		}

		// Token: 0x06004438 RID: 17464 RVA: 0x0026E8F0 File Offset: 0x0026CAF0
		private void OnNormalAttackBegin(DataContext context, CombatCharacter attacker, CombatCharacter defender, sbyte trickType, int pursueIndex)
		{
			bool flag = base.IsDirect ? (attacker.GetId() != base.CharacterId) : (attacker.IsAlly == base.CombatChar.IsAlly);
			if (!flag)
			{
				bool flag2 = !attacker.GetChangeTrickAttack() || !base.IsCurrent;
				if (!flag2)
				{
					base.ShowSpecialEffectTips(0);
				}
			}
		}

		// Token: 0x06004439 RID: 17465 RVA: 0x0026E954 File Offset: 0x0026CB54
		protected override void OnCanUseChanged(DataContext context, DataUid dataUid)
		{
			base.SetConstAffecting(context, base.CanAffect);
		}

		// Token: 0x0600443A RID: 17466 RVA: 0x0026E968 File Offset: 0x0026CB68
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = !dataKey.IsNormalAttack || !base.CanAffect;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				CombatCharacter dataChar = DomainManager.Combat.GetElement_CombatCharacterDict(dataKey.CharId);
				bool flag2 = dataKey.FieldId == 254;
				if (flag2)
				{
					result = ((base.IsCurrent && dataChar.GetChangeTrickAttack()) ? this.CriticalOddsAddPercent : 0);
				}
				else
				{
					bool flag3 = dataKey.CharId != base.CharacterId || !(base.IsDirect ? base.CombatChar : base.CurrEnemyChar).GetChangeTrickAttack() || dataKey.CustomParam1 != 0;
					if (flag3)
					{
						result = 0;
					}
					else
					{
						result = (base.IsDirect ? 20 : -20);
					}
				}
			}
			return result;
		}

		// Token: 0x0400143F RID: 5183
		private const sbyte ChangePercent = 20;
	}
}
