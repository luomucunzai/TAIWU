using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Defense;

namespace GameData.Domains.SpecialEffect.CombatSkill.Jieqingmen.DefenseAndAssist
{
	// Token: 0x02000500 RID: 1280
	public class QiYuanJieEDaFa : DefenseSkillBase
	{
		// Token: 0x06003E78 RID: 15992 RVA: 0x00255F39 File Offset: 0x00254139
		public QiYuanJieEDaFa()
		{
		}

		// Token: 0x06003E79 RID: 15993 RVA: 0x00255F43 File Offset: 0x00254143
		public QiYuanJieEDaFa(CombatSkillKey skillKey) : base(skillKey, 13506)
		{
		}

		// Token: 0x06003E7A RID: 15994 RVA: 0x00255F54 File Offset: 0x00254154
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			base.CreateAffectedData(102, EDataModifyType.TotalPercent, -1);
			base.CreateAffectedData(70, EDataModifyType.AddPercent, -1);
			Events.RegisterHandler_NormalAttackEnd(new Events.OnNormalAttackEnd(this.OnNormalAttackEnd));
			Events.RegisterHandler_AttackSkillAttackEnd(new Events.OnAttackSkillAttackEnd(this.OnAttackSkillAttackEnd));
			Events.RegisterHandler_BounceInjury(new Events.OnBounceInjury(this.OnBounceInjury));
			Events.RegisterHandler_NormalAttackCalcHitEnd(new Events.OnNormalAttackCalcHitEnd(this.OnNormalAttackCalcHitEnd));
			Events.RegisterHandler_AttackSkillAttackHit(new Events.OnAttackSkillAttackHit(this.OnAttackSkillAttackHit));
		}

		// Token: 0x06003E7B RID: 15995 RVA: 0x00255FDC File Offset: 0x002541DC
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_NormalAttackEnd(new Events.OnNormalAttackEnd(this.OnNormalAttackEnd));
			Events.UnRegisterHandler_AttackSkillAttackEnd(new Events.OnAttackSkillAttackEnd(this.OnAttackSkillAttackEnd));
			Events.UnRegisterHandler_BounceInjury(new Events.OnBounceInjury(this.OnBounceInjury));
			Events.UnRegisterHandler_NormalAttackCalcHitEnd(new Events.OnNormalAttackCalcHitEnd(this.OnNormalAttackCalcHitEnd));
			Events.UnRegisterHandler_AttackSkillAttackHit(new Events.OnAttackSkillAttackHit(this.OnAttackSkillAttackHit));
			base.OnDisable(context);
		}

		// Token: 0x06003E7C RID: 15996 RVA: 0x0025604C File Offset: 0x0025424C
		private void OnNormalAttackEnd(DataContext context, CombatCharacter attacker, CombatCharacter defender, sbyte trickType, int pursueIndex, bool hit, bool isFightBack)
		{
			bool flag = defender == base.CombatChar;
			if (flag)
			{
				this.AutoShowAffectTips();
			}
		}

		// Token: 0x06003E7D RID: 15997 RVA: 0x00256070 File Offset: 0x00254270
		private void OnAttackSkillAttackEnd(CombatContext context, sbyte hitType, bool hit, int index)
		{
			bool flag = context.Defender == base.CombatChar;
			if (flag)
			{
				this.AutoShowAffectTips();
			}
		}

		// Token: 0x06003E7E RID: 15998 RVA: 0x00256098 File Offset: 0x00254298
		private void OnBounceInjury(DataContext context, int attackerId, int defenderId, bool isAlly, sbyte bodyPart, sbyte outerMarkCount, sbyte innerMarkCount)
		{
			bool flag = attackerId == base.CharacterId;
			if (flag)
			{
				this.AutoShowAffectTips();
			}
		}

		// Token: 0x06003E7F RID: 15999 RVA: 0x002560BC File Offset: 0x002542BC
		private void OnNormalAttackCalcHitEnd(DataContext context, CombatCharacter attacker, CombatCharacter defender, int pursueIndex, bool hit, bool isFightback, bool isMind)
		{
			bool flag = defender.GetId() != base.CharacterId || pursueIndex != 0 || !hit || isMind;
			if (!flag)
			{
				sbyte bodyPart = attacker.NormalAttackBodyPart;
				this.DoAffect(context, bodyPart);
			}
		}

		// Token: 0x06003E80 RID: 16000 RVA: 0x00256100 File Offset: 0x00254300
		private void OnAttackSkillAttackHit(DataContext context, CombatCharacter attacker, CombatCharacter defender, short skillId, int index, bool critical)
		{
			bool flag = defender.GetId() != base.CharacterId;
			if (!flag)
			{
				sbyte bodyPart = attacker.SkillAttackBodyPart;
				this.DoAffect(context, bodyPart);
			}
		}

		// Token: 0x06003E81 RID: 16001 RVA: 0x00256138 File Offset: 0x00254338
		private void AutoShowAffectTips()
		{
			bool affectedDirect = this._affectedDirect;
			if (affectedDirect)
			{
				base.ShowSpecialEffectTips(0);
			}
			bool affectedBounce = this._affectedBounce;
			if (affectedBounce)
			{
				base.ShowSpecialEffectTips(1);
			}
			this._affectedDirect = (this._affectedBounce = false);
		}

		// Token: 0x06003E82 RID: 16002 RVA: 0x0025617C File Offset: 0x0025437C
		private void DoAffect(DataContext context, sbyte bodyPart)
		{
			bool flag = bodyPart < 0 || bodyPart >= 7;
			bool flag2 = flag;
			if (!flag2)
			{
				bool flag3 = !DomainManager.Combat.CheckBodyPartInjury(base.CombatChar, bodyPart, false);
				if (!flag3)
				{
					CombatCharacter target = base.IsDirect ? base.CombatChar : base.EnemyChar;
					CValuePercent percent = base.IsDirect ? QiYuanJieEDaFa.ChangeEffectCountPercent : (-QiYuanJieEDaFa.ChangeEffectCountPercent);
					bool flag4 = DomainManager.Combat.ChangeSkillEffectRandom(context, target, percent, 1, 1);
					if (flag4)
					{
						base.ShowSpecialEffectTips(2);
					}
				}
			}
		}

		// Token: 0x06003E83 RID: 16003 RVA: 0x0025620C File Offset: 0x0025440C
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = !base.CanAffect;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				int brokenPartCount = DomainManager.Combat.GetBrokenBodyPartCount(base.CombatChar);
				bool flag2 = brokenPartCount == 0;
				if (flag2)
				{
					result = 0;
				}
				else
				{
					bool flag3 = dataKey.FieldId == 102;
					if (flag3)
					{
						this._affectedDirect = true;
						result = -10 * brokenPartCount;
					}
					else
					{
						bool flag4 = dataKey.FieldId == 70;
						if (flag4)
						{
							this._affectedBounce = true;
							result = 20 * brokenPartCount;
						}
						else
						{
							result = 0;
						}
					}
				}
			}
			return result;
		}

		// Token: 0x0400126C RID: 4716
		private const sbyte ReduceDamageUnit = -10;

		// Token: 0x0400126D RID: 4717
		private const sbyte AddDamageUnit = 20;

		// Token: 0x0400126E RID: 4718
		private const sbyte RequireEquipType = 1;

		// Token: 0x0400126F RID: 4719
		private static readonly CValuePercent ChangeEffectCountPercent = 25;

		// Token: 0x04001270 RID: 4720
		private bool _affectedDirect;

		// Token: 0x04001271 RID: 4721
		private bool _affectedBounce;
	}
}
