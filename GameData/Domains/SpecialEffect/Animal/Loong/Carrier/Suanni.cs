using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.Animal.Loong.Carrier
{
	// Token: 0x02000618 RID: 1560
	public class Suanni : CarrierEffectBase
	{
		// Token: 0x06004589 RID: 17801 RVA: 0x00272A98 File Offset: 0x00270C98
		public Suanni(int charId) : base(charId)
		{
		}

		// Token: 0x170002E7 RID: 743
		// (get) Token: 0x0600458A RID: 17802 RVA: 0x00272AA3 File Offset: 0x00270CA3
		protected override short CombatStateId
		{
			get
			{
				return 203;
			}
		}

		// Token: 0x0600458B RID: 17803 RVA: 0x00272AAC File Offset: 0x00270CAC
		protected override void OnEnableSubClass(DataContext context)
		{
			base.CreateAffectedData(89, EDataModifyType.Custom, -1);
			base.CreateAffectedAllEnemyData(191, EDataModifyType.TotalPercent, -1);
			Events.RegisterHandler_NormalAttackBegin(new Events.OnNormalAttackBegin(this.OnNormalAttackBegin));
			Events.RegisterHandler_NormalAttackEnd(new Events.OnNormalAttackEnd(this.OnNormalAttackEnd));
			Events.RegisterHandler_AttackSkillAttackBegin(new Events.OnAttackSkillAttackBegin(this.OnAttackSkillAttackBegin));
			Events.RegisterHandler_AttackSkillAttackEnd(new Events.OnAttackSkillAttackEnd(this.OnAttackSkillAttackEnd));
		}

		// Token: 0x0600458C RID: 17804 RVA: 0x00272B1C File Offset: 0x00270D1C
		protected override void OnDisableSubClass(DataContext context)
		{
			Events.UnRegisterHandler_NormalAttackBegin(new Events.OnNormalAttackBegin(this.OnNormalAttackBegin));
			Events.UnRegisterHandler_NormalAttackEnd(new Events.OnNormalAttackEnd(this.OnNormalAttackEnd));
			Events.UnRegisterHandler_AttackSkillAttackBegin(new Events.OnAttackSkillAttackBegin(this.OnAttackSkillAttackBegin));
			Events.UnRegisterHandler_AttackSkillAttackEnd(new Events.OnAttackSkillAttackEnd(this.OnAttackSkillAttackEnd));
		}

		// Token: 0x0600458D RID: 17805 RVA: 0x00272B74 File Offset: 0x00270D74
		private void OnNormalAttackBegin(DataContext context, CombatCharacter attacker, CombatCharacter defender, sbyte trickType, int pursueIndex)
		{
			bool flag = attacker.GetId() != base.CharacterId;
			if (!flag)
			{
				this._changeToFatal = context.Random.CheckPercentProb(50);
			}
		}

		// Token: 0x0600458E RID: 17806 RVA: 0x00272BAC File Offset: 0x00270DAC
		private void OnNormalAttackEnd(DataContext context, CombatCharacter attacker, CombatCharacter defender, sbyte trickType, int pursueIndex, bool hit, bool isFightback)
		{
			bool flag = attacker.GetId() != base.CharacterId;
			if (!flag)
			{
				this._changeToFatal = false;
			}
		}

		// Token: 0x0600458F RID: 17807 RVA: 0x00272BD8 File Offset: 0x00270DD8
		private void OnAttackSkillAttackBegin(DataContext context, CombatCharacter attacker, CombatCharacter defender, short skillId, int index, bool hit)
		{
			bool flag = attacker.GetId() != base.CharacterId;
			if (!flag)
			{
				this._changeToFatal = context.Random.CheckPercentProb(50);
			}
		}

		// Token: 0x06004590 RID: 17808 RVA: 0x00272C10 File Offset: 0x00270E10
		private void OnAttackSkillAttackEnd(CombatContext context, sbyte hitType, bool hit, int index)
		{
			bool flag = context.AttackerId != base.CharacterId;
			if (!flag)
			{
				this._changeToFatal = false;
			}
		}

		// Token: 0x06004591 RID: 17809 RVA: 0x00272C40 File Offset: 0x00270E40
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.CharId == base.CharacterId || dataKey.FieldId != 191;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				EDamageType damageType = (EDamageType)dataKey.CustomParam1;
				bool flag2 = damageType != EDamageType.Direct;
				if (flag2)
				{
					result = 0;
				}
				else
				{
					result = 20;
				}
			}
			return result;
		}

		// Token: 0x06004592 RID: 17810 RVA: 0x00272C94 File Offset: 0x00270E94
		public override long GetModifiedValue(AffectedDataKey dataKey, long dataValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || dataKey.FieldId != 89 || dataValue <= 0L || !this._changeToFatal;
			long result;
			if (flag)
			{
				result = dataValue;
			}
			else
			{
				bool isInner = dataKey.CustomParam1 == 1;
				sbyte bodyPart = (sbyte)dataKey.CustomParam2;
				CombatCharacter enemyChar = base.CurrEnemyChar;
				int value = (int)Math.Clamp(dataValue, 0L, 2147483647L);
				DomainManager.Combat.AddFatalDamageValue(base.CombatChar.GetDataContext(), enemyChar, value, isInner ? 1 : 0, bodyPart, dataKey.CombatSkillId, EDamageType.None);
				result = 0L;
			}
			return result;
		}

		// Token: 0x0400148E RID: 5262
		private const int FatalDamageTotalPercent = 20;

		// Token: 0x0400148F RID: 5263
		private const int ChangeToFatalOdds = 50;

		// Token: 0x04001490 RID: 5264
		private bool _changeToFatal;
	}
}
