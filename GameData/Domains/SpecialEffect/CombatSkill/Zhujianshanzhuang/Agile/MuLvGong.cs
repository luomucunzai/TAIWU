using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Agile;

namespace GameData.Domains.SpecialEffect.CombatSkill.Zhujianshanzhuang.Agile
{
	// Token: 0x020001EB RID: 491
	public class MuLvGong : AgileSkillBase
	{
		// Token: 0x06002E1F RID: 11807 RVA: 0x0020DCAA File Offset: 0x0020BEAA
		public MuLvGong()
		{
		}

		// Token: 0x06002E20 RID: 11808 RVA: 0x0020DCB4 File Offset: 0x0020BEB4
		public MuLvGong(CombatSkillKey skillKey) : base(skillKey, 9500)
		{
		}

		// Token: 0x06002E21 RID: 11809 RVA: 0x0020DCC4 File Offset: 0x0020BEC4
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			this._affected = false;
			bool isDirect = base.IsDirect;
			if (isDirect)
			{
				this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
				this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 150, -1, -1, -1, -1), EDataModifyType.TotalPercent);
				base.ShowSpecialEffectTips(0);
			}
			else
			{
				Events.RegisterHandler_AddDirectInjury(new Events.OnAddDirectInjury(this.OnAddDirectInjury));
				Events.RegisterHandler_AddDirectFatalDamageMark(new Events.OnAddDirectFatalDamageMark(this.OnAddDirectFatalDamageMark));
			}
		}

		// Token: 0x06002E22 RID: 11810 RVA: 0x0020DD4C File Offset: 0x0020BF4C
		public override void OnDisable(DataContext context)
		{
			base.OnDisable(context);
			bool flag = !base.IsDirect;
			if (flag)
			{
				Events.UnRegisterHandler_AddDirectInjury(new Events.OnAddDirectInjury(this.OnAddDirectInjury));
				Events.UnRegisterHandler_AddDirectFatalDamageMark(new Events.OnAddDirectFatalDamageMark(this.OnAddDirectFatalDamageMark));
			}
		}

		// Token: 0x06002E23 RID: 11811 RVA: 0x0020DD98 File Offset: 0x0020BF98
		private void OnAddDirectInjury(DataContext context, int attackerId, int defenderId, bool isAlly, sbyte bodyPart, sbyte outerMarkCount, sbyte innerMarkCount, short combatSkillId)
		{
			bool flag = defenderId != base.CharacterId || isAlly == base.CombatChar.IsAlly || (outerMarkCount <= 0 && innerMarkCount <= 0) || !base.CanAffect;
			if (!flag)
			{
				this.AddMobility(context);
			}
		}

		// Token: 0x06002E24 RID: 11812 RVA: 0x0020DDE4 File Offset: 0x0020BFE4
		private void OnAddDirectFatalDamageMark(DataContext context, int attackerId, int defenderId, bool isAlly, sbyte bodyPart, int outerMarkCount, int innerMarkCount, short combatSkillId)
		{
			bool flag = defenderId != base.CharacterId || isAlly == base.CombatChar.IsAlly || (outerMarkCount <= 0 && innerMarkCount <= 0) || !base.CanAffect;
			if (!flag)
			{
				this.AddMobility(context);
			}
		}

		// Token: 0x06002E25 RID: 11813 RVA: 0x0020DE30 File Offset: 0x0020C030
		private void AddMobility(DataContext context)
		{
			bool affected = this._affected;
			if (!affected)
			{
				base.ChangeMobilityValue(context, base.CombatChar, MoveSpecialConstants.MaxMobility * MuLvGong.ReverseAdd);
				base.ShowSpecialEffectTips(0);
				this._affected = true;
				Events.RegisterHandler_CombatStateMachineUpdateEnd(new Events.OnCombatStateMachineUpdateEnd(this.OnStateMachineUpdateEnd));
			}
		}

		// Token: 0x06002E26 RID: 11814 RVA: 0x0020DE88 File Offset: 0x0020C088
		private void OnStateMachineUpdateEnd(DataContext context, CombatCharacter combatChar)
		{
			this._affected = false;
			Events.UnRegisterHandler_CombatStateMachineUpdateEnd(new Events.OnCombatStateMachineUpdateEnd(this.OnStateMachineUpdateEnd));
		}

		// Token: 0x06002E27 RID: 11815 RVA: 0x0020DEA4 File Offset: 0x0020C0A4
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = !base.CanAffect || dataKey.CharId != base.CharacterId;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				bool flag2 = dataKey.FieldId == 150;
				if (flag2)
				{
					result = -50;
				}
				else
				{
					result = 0;
				}
			}
			return result;
		}

		// Token: 0x04000DBE RID: 3518
		private const sbyte DirectReduceCost = -50;

		// Token: 0x04000DBF RID: 3519
		private static readonly CValuePercent ReverseAdd = 4;

		// Token: 0x04000DC0 RID: 3520
		private bool _affected;
	}
}
