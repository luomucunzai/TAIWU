using System;
using System.Collections.Generic;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.Common.Agile
{
	// Token: 0x020005B6 RID: 1462
	public class MakeTricksInterchangeable : AgileSkillBase
	{
		// Token: 0x06004376 RID: 17270 RVA: 0x0026B7E3 File Offset: 0x002699E3
		protected MakeTricksInterchangeable()
		{
		}

		// Token: 0x06004377 RID: 17271 RVA: 0x0026B7ED File Offset: 0x002699ED
		protected MakeTricksInterchangeable(CombatSkillKey skillKey, int type) : base(skillKey, type)
		{
			this.ListenCanAffectChange = base.IsDirect;
		}

		// Token: 0x06004378 RID: 17272 RVA: 0x0026B808 File Offset: 0x00269A08
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			bool isDirect = base.IsDirect;
			if (isDirect)
			{
				this.OnMoveSkillCanAffectChanged(context, default(DataUid));
				base.ShowSpecialEffectTips(0);
			}
			else
			{
				Events.RegisterHandler_GetTrick(new Events.OnGetTrick(this.OnGetTrick));
			}
		}

		// Token: 0x06004379 RID: 17273 RVA: 0x0026B85C File Offset: 0x00269A5C
		public override void OnDisable(DataContext context)
		{
			base.OnDisable(context);
			bool isDirect = base.IsDirect;
			if (isDirect)
			{
				base.CombatChar.InterchangeableTricks.Clear();
				DomainManager.Combat.RemoveOverflowTrick(context, base.CombatChar, true);
			}
			else
			{
				Events.UnRegisterHandler_GetTrick(new Events.OnGetTrick(this.OnGetTrick));
			}
		}

		// Token: 0x0600437A RID: 17274 RVA: 0x0026B8B8 File Offset: 0x00269AB8
		private void OnGetTrick(DataContext context, int charId, bool isAlly, sbyte trickType, bool usable)
		{
			bool flag = isAlly == base.CombatChar.IsAlly || !base.CanAffect || !this.AffectTrickTypes.Contains(trickType) || base.CombatChar.IsTrickUseless(trickType) || !context.Random.CheckPercentProb(50);
			if (!flag)
			{
				CombatCharacter enemyChar = DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly, false);
				bool flag2 = DomainManager.Combat.RemoveTrick(context, enemyChar, trickType, 1, false, -1);
				if (flag2)
				{
					this._needAddTrick = trickType;
					Events.RegisterHandler_CombatStateMachineUpdateEnd(new Events.OnCombatStateMachineUpdateEnd(this.OnStateMachineUpdateEnd));
					base.ShowSpecialEffectTips(0);
				}
			}
		}

		// Token: 0x0600437B RID: 17275 RVA: 0x0026B968 File Offset: 0x00269B68
		protected override void OnMoveSkillCanAffectChanged(DataContext context, DataUid dataUid)
		{
			base.CombatChar.InterchangeableTricks.Clear();
			bool canAffect = base.CanAffect;
			if (canAffect)
			{
				base.CombatChar.InterchangeableTricks.AddRange(this.AffectTrickTypes);
			}
			DomainManager.Combat.RemoveOverflowTrick(context, base.CombatChar, true);
		}

		// Token: 0x0600437C RID: 17276 RVA: 0x0026B9BB File Offset: 0x00269BBB
		private void OnStateMachineUpdateEnd(DataContext context, CombatCharacter combatChar)
		{
			DomainManager.Combat.AddTrick(context, base.CombatChar, this._needAddTrick, true);
			Events.UnRegisterHandler_CombatStateMachineUpdateEnd(new Events.OnCombatStateMachineUpdateEnd(this.OnStateMachineUpdateEnd));
		}

		// Token: 0x04001407 RID: 5127
		private const sbyte ReverseAffectOdds = 50;

		// Token: 0x04001408 RID: 5128
		protected List<sbyte> AffectTrickTypes;

		// Token: 0x04001409 RID: 5129
		private sbyte _needAddTrick;
	}
}
