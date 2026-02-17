using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;

namespace GameData.Domains.SpecialEffect.SectStory.Yuanshan
{
	// Token: 0x020000EA RID: 234
	public class VitalDemonC : VitalDemonEffectBase
	{
		// Token: 0x06002966 RID: 10598 RVA: 0x0020083C File Offset: 0x001FEA3C
		public VitalDemonC(int charId) : base(charId, 1750)
		{
		}

		// Token: 0x06002967 RID: 10599 RVA: 0x0020084C File Offset: 0x001FEA4C
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			Events.RegisterHandler_AddDirectDamageValue(new Events.OnAddDirectDamageValue(this.OnAddDirectDamageValue));
			Events.RegisterHandler_CombatStateMachineUpdateEnd(new Events.OnCombatStateMachineUpdateEnd(this.OnCombatStateMachineUpdateEnd));
			foreach (CombatCharacter character in DomainManager.Combat.GetCharacters(base.CombatChar.IsAlly))
			{
				base.CreateAffectedData(character.GetId(), 69, EDataModifyType.AddPercent, -1);
				base.CreateAffectedData(character.GetId(), 77, EDataModifyType.Custom, -1);
			}
		}

		// Token: 0x06002968 RID: 10600 RVA: 0x002008F4 File Offset: 0x001FEAF4
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_AddDirectDamageValue(new Events.OnAddDirectDamageValue(this.OnAddDirectDamageValue));
			Events.UnRegisterHandler_CombatStateMachineUpdateEnd(new Events.OnCombatStateMachineUpdateEnd(this.OnCombatStateMachineUpdateEnd));
			base.OnDisable(context);
		}

		// Token: 0x06002969 RID: 10601 RVA: 0x00200924 File Offset: 0x001FEB24
		private void OnAddDirectDamageValue(DataContext context, int attackerId, int defenderId, sbyte bodyPart, bool isInner, int damageValue, short combatSkillId)
		{
			bool flag = combatSkillId >= 0 || damageValue <= 0 || this._affected;
			if (!flag)
			{
				CombatCharacter attacker = DomainManager.Combat.GetElement_CombatCharacterDict(attackerId);
				CombatCharacter defender = DomainManager.Combat.GetElement_CombatCharacterDict(defenderId);
				bool flag2 = attacker.IsAlly != base.CombatChar.IsAlly || defender.IsAlly == base.CombatChar.IsAlly;
				if (!flag2)
				{
					this._affected = true;
					base.ShowSpecialEffect(0);
					base.ShowSpecialEffect(1);
				}
			}
		}

		// Token: 0x0600296A RID: 10602 RVA: 0x002009AC File Offset: 0x001FEBAC
		private void OnCombatStateMachineUpdateEnd(DataContext context, CombatCharacter combatChar)
		{
			bool flag = combatChar.IsAlly == base.CombatChar.IsAlly;
			if (flag)
			{
				this._affected = false;
			}
		}

		// Token: 0x0600296B RID: 10603 RVA: 0x002009D8 File Offset: 0x001FEBD8
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.IsNormalAttack && dataKey.FieldId == 69;
			int result;
			if (flag)
			{
				result = 200;
			}
			else
			{
				result = base.GetModifyValue(dataKey, currModifyValue);
			}
			return result;
		}

		// Token: 0x0600296C RID: 10604 RVA: 0x00200A14 File Offset: 0x001FEC14
		public override bool GetModifiedValue(AffectedDataKey dataKey, bool dataValue)
		{
			bool flag = dataKey.IsNormalAttack && dataKey.FieldId == 77;
			return flag || base.GetModifiedValue(dataKey, dataValue);
		}

		// Token: 0x04000CC0 RID: 3264
		private const int DirectDamageAddPercent = 200;

		// Token: 0x04000CC1 RID: 3265
		private bool _affected;
	}
}
