using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Common.Attack
{
	// Token: 0x020005A0 RID: 1440
	public abstract class PowerUpByPoison : PowerUpOnCast
	{
		// Token: 0x1700029C RID: 668
		// (get) Token: 0x060042C6 RID: 17094 RVA: 0x002685E1 File Offset: 0x002667E1
		protected override EDataModifyType ModifyType
		{
			get
			{
				return EDataModifyType.AddPercent;
			}
		}

		// Token: 0x1700029D RID: 669
		// (get) Token: 0x060042C7 RID: 17095
		protected abstract sbyte RequirePoisonType { get; }

		// Token: 0x1700029E RID: 670
		// (get) Token: 0x060042C8 RID: 17096
		protected abstract short DirectStateId { get; }

		// Token: 0x1700029F RID: 671
		// (get) Token: 0x060042C9 RID: 17097
		protected abstract short ReverseStateId { get; }

		// Token: 0x060042CA RID: 17098 RVA: 0x002685E4 File Offset: 0x002667E4
		protected PowerUpByPoison()
		{
		}

		// Token: 0x060042CB RID: 17099 RVA: 0x002685EE File Offset: 0x002667EE
		protected PowerUpByPoison(CombatSkillKey skillKey, int type) : base(skillKey, type)
		{
		}

		// Token: 0x060042CC RID: 17100 RVA: 0x002685FC File Offset: 0x002667FC
		public override void OnEnable(DataContext context)
		{
			CombatCharacter combatChar = base.IsDirect ? DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly, true) : base.CombatChar;
			this.PowerUpValue = (int)(20 * combatChar.GetDefeatMarkCollection().PoisonMarkList[(int)this.RequirePoisonType]);
			base.OnEnable(context);
		}

		// Token: 0x060042CD RID: 17101 RVA: 0x00268658 File Offset: 0x00266858
		protected override void OnCastSelf(DataContext context, sbyte power, bool interrupted)
		{
			int statePower = (int)(power / 10 * 20);
			bool flag = statePower <= 0;
			if (!flag)
			{
				CombatCharacter stateChar = base.IsDirect ? base.EnemyChar : base.CombatChar;
				short stateId = base.IsDirect ? this.DirectStateId : this.ReverseStateId;
				sbyte stateType = base.IsDirect ? 2 : 1;
				DomainManager.Combat.AddCombatState(context, stateChar, stateType, stateId, statePower);
				base.ShowSpecialEffectTips(1);
			}
		}

		// Token: 0x040013C2 RID: 5058
		private const sbyte AddPowerUnit = 20;

		// Token: 0x040013C3 RID: 5059
		private const int StatePowerUnit = 20;
	}
}
