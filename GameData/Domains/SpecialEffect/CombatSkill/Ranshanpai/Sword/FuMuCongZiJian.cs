using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Ranshanpai.Sword
{
	// Token: 0x02000441 RID: 1089
	public class FuMuCongZiJian : CombatSkillEffectBase
	{
		// Token: 0x06003A12 RID: 14866 RVA: 0x00241EA7 File Offset: 0x002400A7
		public FuMuCongZiJian()
		{
		}

		// Token: 0x06003A13 RID: 14867 RVA: 0x00241EB1 File Offset: 0x002400B1
		public FuMuCongZiJian(CombatSkillKey skillKey) : base(skillKey, 7204, -1)
		{
		}

		// Token: 0x06003A14 RID: 14868 RVA: 0x00241EC4 File Offset: 0x002400C4
		public override void OnEnable(DataContext context)
		{
			this._waitingNormalAttackCharId = -1;
			base.CreateAffectedAllEnemyData(base.IsDirect ? 103 : 104, EDataModifyType.AddPercent, -1);
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
			Events.RegisterHandler_NormalAttackAllEnd(new Events.OnNormalAttackAllEnd(this.OnNormalAttackAllEnd));
		}

		// Token: 0x06003A15 RID: 14869 RVA: 0x00241F14 File Offset: 0x00240114
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
			Events.UnRegisterHandler_NormalAttackAllEnd(new Events.OnNormalAttackAllEnd(this.OnNormalAttackAllEnd));
		}

		// Token: 0x06003A16 RID: 14870 RVA: 0x00241F3C File Offset: 0x0024013C
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId;
			if (!flag)
			{
				bool flag2 = skillId == base.SkillTemplateId && base.PowerMatchAffectRequire((int)power, 0);
				if (flag2)
				{
					base.AddMaxEffectCount(true);
				}
			}
		}

		// Token: 0x06003A17 RID: 14871 RVA: 0x00241F80 File Offset: 0x00240180
		private void OnNormalAttackAllEnd(DataContext context, CombatCharacter attacker, CombatCharacter defender)
		{
			bool flag = attacker.GetId() == this._waitingNormalAttackCharId;
			if (flag)
			{
				this._waitingNormalAttackCharId = -1;
			}
			bool flag2 = attacker.GetId() != base.CharacterId || attacker.GetIsFightBack();
			if (!flag2)
			{
				this.TryInvokeEnemyAttack();
			}
		}

		// Token: 0x06003A18 RID: 14872 RVA: 0x00241FCC File Offset: 0x002401CC
		private void TryInvokeEnemyAttack()
		{
			CombatCharacter enemyChar = DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly, false);
			bool flag = base.EffectCount <= 0 || DomainManager.Combat.IsCharacterFallen(enemyChar);
			if (!flag)
			{
				bool flag2 = this._waitingNormalAttackCharId >= 0;
				if (!flag2)
				{
					bool flag3 = enemyChar.StateMachine.GetCurrentStateType() != CombatCharacterStateType.PrepareAttack;
					if (flag3)
					{
						enemyChar.NormalAttackFree();
					}
					this._waitingNormalAttackCharId = enemyChar.GetId();
					base.ReduceEffectCount(1);
					base.ShowSpecialEffectTips(0);
				}
			}
		}

		// Token: 0x06003A19 RID: 14873 RVA: 0x0024205C File Offset: 0x0024025C
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.CharId != this._waitingNormalAttackCharId;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				ushort fieldId = dataKey.FieldId;
				if (!true)
				{
				}
				int num;
				if (fieldId != 103)
				{
					if (fieldId != 104)
					{
						num = 0;
					}
					else
					{
						num = 50;
					}
				}
				else
				{
					num = 200;
				}
				if (!true)
				{
				}
				result = num;
			}
			return result;
		}

		// Token: 0x04001100 RID: 4352
		private const int BounceAddPercent = 200;

		// Token: 0x04001101 RID: 4353
		private const int FightBackAddPercent = 50;

		// Token: 0x04001102 RID: 4354
		private int _waitingNormalAttackCharId;
	}
}
