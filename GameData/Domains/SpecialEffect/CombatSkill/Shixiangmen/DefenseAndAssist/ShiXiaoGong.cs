using System;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Assist;
using GameData.GameDataBridge;

namespace GameData.Domains.SpecialEffect.CombatSkill.Shixiangmen.DefenseAndAssist
{
	// Token: 0x02000402 RID: 1026
	public class ShiXiaoGong : AssistSkillBase
	{
		// Token: 0x060038BF RID: 14527 RVA: 0x0023BBCD File Offset: 0x00239DCD
		public ShiXiaoGong()
		{
		}

		// Token: 0x060038C0 RID: 14528 RVA: 0x0023BBD7 File Offset: 0x00239DD7
		public ShiXiaoGong(CombatSkillKey skillKey) : base(skillKey, 6604)
		{
		}

		// Token: 0x060038C1 RID: 14529 RVA: 0x0023BBE8 File Offset: 0x00239DE8
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			this._defeatMarkUid = base.ParseCombatCharacterDataUid(50);
			GameDataBridge.AddPostDataModificationHandler(this._defeatMarkUid, base.DataHandlerKey, new Action<DataContext, DataUid>(this.UpdateAffected));
			this._affected = DomainManager.Combat.IsCharacterHalfFallen(base.CombatChar);
			Events.RegisterHandler_CombatBegin(new Events.OnCombatBegin(this.OnCombatBegin));
		}

		// Token: 0x060038C2 RID: 14530 RVA: 0x0023BC52 File Offset: 0x00239E52
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CombatBegin(new Events.OnCombatBegin(this.OnCombatBegin));
			GameDataBridge.RemovePostDataModificationHandler(this._defeatMarkUid, base.DataHandlerKey);
			base.OnDisable(context);
		}

		// Token: 0x060038C3 RID: 14531 RVA: 0x0023BC81 File Offset: 0x00239E81
		private void OnCombatBegin(DataContext context)
		{
			this.DoReduceEffect(context);
		}

		// Token: 0x060038C4 RID: 14532 RVA: 0x0023BC8C File Offset: 0x00239E8C
		private void UpdateAffected(DataContext context, DataUid dataUid)
		{
			bool affected = DomainManager.Combat.IsCharacterHalfFallen(base.CombatChar);
			bool flag = affected == this._affected;
			if (!flag)
			{
				this._affected = affected;
				bool flag2 = affected;
				if (flag2)
				{
					this.DoReduceEffect(context);
				}
			}
		}

		// Token: 0x060038C5 RID: 14533 RVA: 0x0023BCD0 File Offset: 0x00239ED0
		private void DoReduceEffect(DataContext context)
		{
			bool flag = !base.CanAffect || (!base.IsCurrent && !base.IsEntering);
			if (!flag)
			{
				CombatCharacter enemyChar = DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly, false);
				bool isDirect = base.IsDirect;
				if (isDirect)
				{
					base.ChangeMobilityValue(context, enemyChar, -MoveSpecialConstants.MaxMobility);
					base.ClearAffectingAgileSkill(context, enemyChar);
				}
				else
				{
					base.ChangeBreathValue(context, enemyChar, -30000);
					base.ChangeStanceValue(context, enemyChar, -4000);
				}
				base.ShowEffectTips(context);
				base.ShowSpecialEffectTips(0);
			}
		}

		// Token: 0x0400109E RID: 4254
		private DataUid _defeatMarkUid;

		// Token: 0x0400109F RID: 4255
		private bool _affected;
	}
}
