using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Common.Agile
{
	// Token: 0x020005B3 RID: 1459
	public class ChangeAttackHitType : AgileSkillBase
	{
		// Token: 0x06004360 RID: 17248 RVA: 0x0026B1D4 File Offset: 0x002693D4
		protected ChangeAttackHitType()
		{
		}

		// Token: 0x06004361 RID: 17249 RVA: 0x0026B1DE File Offset: 0x002693DE
		protected ChangeAttackHitType(CombatSkillKey skillKey, int type) : base(skillKey, type)
		{
		}

		// Token: 0x06004362 RID: 17250 RVA: 0x0026B1EC File Offset: 0x002693EC
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			bool isDirect = base.IsDirect;
			if (isDirect)
			{
				this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 68, -1, -1, -1, -1), EDataModifyType.Custom);
				CombatCharacter combatChar = base.CombatChar;
				combatChar.ChangeHitTypeEffectCount += 1;
			}
			else
			{
				int[] charList = DomainManager.Combat.GetCharacterList(!base.CombatChar.IsAlly);
				for (int i = 0; i < charList.Length; i++)
				{
					bool flag = charList[i] >= 0;
					if (flag)
					{
						this.AffectDatas.Add(new AffectedDataKey(charList[i], 68, -1, -1, -1, -1), EDataModifyType.Custom);
					}
				}
				CombatCharacter combatChar2 = base.CombatChar;
				combatChar2.ChangeAvoidTypeEffectCount += 1;
			}
			Events.RegisterHandler_NormalAttackEnd(new Events.OnNormalAttackEnd(this.OnNormalAttackEnd));
		}

		// Token: 0x06004363 RID: 17251 RVA: 0x0026B2CC File Offset: 0x002694CC
		public override void OnDisable(DataContext context)
		{
			base.OnDisable(context);
			bool isDirect = base.IsDirect;
			if (isDirect)
			{
				CombatCharacter combatChar = base.CombatChar;
				combatChar.ChangeHitTypeEffectCount -= 1;
			}
			else
			{
				CombatCharacter combatChar2 = base.CombatChar;
				combatChar2.ChangeAvoidTypeEffectCount -= 1;
			}
			Events.UnRegisterHandler_NormalAttackEnd(new Events.OnNormalAttackEnd(this.OnNormalAttackEnd));
		}

		// Token: 0x06004364 RID: 17252 RVA: 0x0026B328 File Offset: 0x00269528
		private void OnNormalAttackEnd(DataContext context, CombatCharacter attacker, CombatCharacter defender, sbyte trickType, int pursueIndex, bool hit, bool isFightBack)
		{
			bool flag = !this._affected;
			if (!flag)
			{
				this._affected = false;
				bool flag2 = pursueIndex == 0;
				if (flag2)
				{
					base.ShowSpecialEffectTips(0);
				}
			}
		}

		// Token: 0x06004365 RID: 17253 RVA: 0x0026B360 File Offset: 0x00269560
		public override int GetModifiedValue(AffectedDataKey dataKey, int dataValue)
		{
			bool flag = dataKey.CombatSkillId >= 0 || !base.CanAffect;
			int result;
			if (flag)
			{
				result = dataValue;
			}
			else
			{
				CombatCharacter enemyChar = base.CurrEnemyChar;
				bool flag2 = (base.IsDirect ? enemyChar.ChangeAvoidTypeEffectCount : enemyChar.ChangeHitTypeEffectCount) > 0;
				if (flag2)
				{
					result = dataValue;
				}
				else
				{
					this._affected = true;
					result = (int)this.HitType;
				}
			}
			return result;
		}

		// Token: 0x040013FF RID: 5119
		protected sbyte HitType;

		// Token: 0x04001400 RID: 5120
		private bool _affected;
	}
}
