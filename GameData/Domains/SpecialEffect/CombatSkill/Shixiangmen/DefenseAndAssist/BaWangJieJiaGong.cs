using System;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Defense;

namespace GameData.Domains.SpecialEffect.CombatSkill.Shixiangmen.DefenseAndAssist
{
	// Token: 0x020003FE RID: 1022
	public class BaWangJieJiaGong : DefenseSkillBase
	{
		// Token: 0x060038A3 RID: 14499 RVA: 0x0023B4BB File Offset: 0x002396BB
		public BaWangJieJiaGong()
		{
		}

		// Token: 0x060038A4 RID: 14500 RVA: 0x0023B4C5 File Offset: 0x002396C5
		public BaWangJieJiaGong(CombatSkillKey skillKey) : base(skillKey, 6505)
		{
		}

		// Token: 0x060038A5 RID: 14501 RVA: 0x0023B4D5 File Offset: 0x002396D5
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			Events.RegisterHandler_AddDirectInjury(new Events.OnAddDirectInjury(this.OnAddDirectInjury));
			Events.RegisterHandler_AddDirectFatalDamageMark(new Events.OnAddDirectFatalDamageMark(this.OnAddDirectFatalDamageMark));
		}

		// Token: 0x060038A6 RID: 14502 RVA: 0x0023B504 File Offset: 0x00239704
		public override void OnDisable(DataContext context)
		{
			base.OnDisable(context);
			Events.UnRegisterHandler_AddDirectInjury(new Events.OnAddDirectInjury(this.OnAddDirectInjury));
			Events.UnRegisterHandler_AddDirectFatalDamageMark(new Events.OnAddDirectFatalDamageMark(this.OnAddDirectFatalDamageMark));
		}

		// Token: 0x060038A7 RID: 14503 RVA: 0x0023B534 File Offset: 0x00239734
		private void OnAddDirectInjury(DataContext context, int attackerId, int defenderId, bool isAlly, sbyte bodyPart, sbyte outerMarkCount, sbyte innerMarkCount, short combatSkillId)
		{
			bool flag = (outerMarkCount > 0 || innerMarkCount > 0) && defenderId == base.CharacterId && DomainManager.Combat.GetElement_CombatCharacterDict(attackerId).IsAlly != base.CombatChar.IsAlly;
			if (flag)
			{
				this.ChangeNeiliAllocation(context, (int)outerMarkCount, (int)innerMarkCount);
			}
		}

		// Token: 0x060038A8 RID: 14504 RVA: 0x0023B588 File Offset: 0x00239788
		private void OnAddDirectFatalDamageMark(DataContext context, int attackerId, int defenderId, bool isAlly, sbyte bodyPart, int outerMarkCount, int innerMarkCount, short combatSkillId)
		{
			bool flag = (outerMarkCount > 0 || innerMarkCount > 0) && defenderId == base.CharacterId && DomainManager.Combat.GetElement_CombatCharacterDict(attackerId).IsAlly != base.CombatChar.IsAlly;
			if (flag)
			{
				this.ChangeNeiliAllocation(context, outerMarkCount, innerMarkCount);
			}
		}

		// Token: 0x060038A9 RID: 14505 RVA: 0x0023B5DC File Offset: 0x002397DC
		private unsafe void ChangeNeiliAllocation(DataContext context, int outerMarkCount, int innerMarkCount)
		{
			bool flag = !base.CanAffect;
			if (!flag)
			{
				int changeNeiliAllocation = 5 * (outerMarkCount + innerMarkCount);
				NeiliAllocation selfNeiliAllocation = base.CombatChar.GetNeiliAllocation();
				NeiliAllocation enemyNeiliAllocation = base.CurrEnemyChar.GetNeiliAllocation();
				byte reduceType = base.IsDirect ? 2 : 0;
				byte addType = base.IsDirect ? 0 : 2;
				int selfChange = Math.Min(changeNeiliAllocation, (int)(*(ref selfNeiliAllocation.Items.FixedElementField + (IntPtr)reduceType * 2)));
				int enemyChange = Math.Min(changeNeiliAllocation, (int)(*(ref enemyNeiliAllocation.Items.FixedElementField + (IntPtr)reduceType * 2)));
				bool flag2 = selfChange == 0 && enemyChange == 0;
				if (!flag2)
				{
					bool flag3 = selfChange > 0;
					if (flag3)
					{
						base.CombatChar.ChangeNeiliAllocation(context, reduceType, -selfChange, true, true);
						base.CombatChar.ChangeNeiliAllocation(context, addType, selfChange, true, true);
					}
					bool flag4 = enemyChange > 0;
					if (flag4)
					{
						base.CurrEnemyChar.ChangeNeiliAllocation(context, reduceType, -enemyChange, true, true);
					}
					base.ShowSpecialEffectTipsOnceInFrame(0);
				}
			}
		}

		// Token: 0x04001099 RID: 4249
		private const sbyte ChangeNeiliAllocationUnit = 5;
	}
}
