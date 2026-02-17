using System;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Defense;

namespace GameData.Domains.SpecialEffect.CombatSkill.Jieqingmen.DefenseAndAssist
{
	// Token: 0x020004FE RID: 1278
	public class LiuZhuanTianHuang : DefenseSkillBase
	{
		// Token: 0x06003E6E RID: 15982 RVA: 0x00255D2E File Offset: 0x00253F2E
		public LiuZhuanTianHuang()
		{
		}

		// Token: 0x06003E6F RID: 15983 RVA: 0x00255D38 File Offset: 0x00253F38
		public LiuZhuanTianHuang(CombatSkillKey skillKey) : base(skillKey, 13502)
		{
		}

		// Token: 0x06003E70 RID: 15984 RVA: 0x00255D48 File Offset: 0x00253F48
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			Events.RegisterHandler_NormalAttackEnd(new Events.OnNormalAttackEnd(this.OnNormalAttackEnd));
			Events.RegisterHandler_AttackSkillAttackEnd(new Events.OnAttackSkillAttackEnd(this.OnAttackSkillAttackEnd));
		}

		// Token: 0x06003E71 RID: 15985 RVA: 0x00255D77 File Offset: 0x00253F77
		public override void OnDisable(DataContext context)
		{
			base.OnDisable(context);
			Events.UnRegisterHandler_NormalAttackEnd(new Events.OnNormalAttackEnd(this.OnNormalAttackEnd));
			Events.UnRegisterHandler_AttackSkillAttackEnd(new Events.OnAttackSkillAttackEnd(this.OnAttackSkillAttackEnd));
		}

		// Token: 0x06003E72 RID: 15986 RVA: 0x00255DA8 File Offset: 0x00253FA8
		private void OnNormalAttackEnd(DataContext context, CombatCharacter attacker, CombatCharacter defender, sbyte trickType, int pursueIndex, bool hit, bool isFightBack)
		{
			bool flag = hit || defender != base.CombatChar || !base.CanAffect || attacker.NormalAttackHitType != 2;
			if (!flag)
			{
				this.DoEffect(context);
			}
		}

		// Token: 0x06003E73 RID: 15987 RVA: 0x00255DE8 File Offset: 0x00253FE8
		private void OnAttackSkillAttackEnd(CombatContext context, sbyte hitType, bool hit, int index)
		{
			bool flag = context.Defender != base.CombatChar || hit || !base.CanAffect || index > 2 || DomainManager.Combat.GetDamageCompareData().HitType[index] != 2;
			if (!flag)
			{
				this.DoEffect(context);
			}
		}

		// Token: 0x06003E74 RID: 15988 RVA: 0x00255E48 File Offset: 0x00254048
		private void DoEffect(DataContext context)
		{
			sbyte[] types = base.IsDirect ? LiuZhuanTianHuang.DirectAbsorbMainAttributeTypes : LiuZhuanTianHuang.ReverseAbsorbMainAttributeTypes;
			Character srcChar = base.EnemyChar.GetCharacter();
			Character dstChar = this.CharObj;
			foreach (sbyte type in types)
			{
				int absorbValue = Math.Min(3, (int)srcChar.GetCurrMainAttribute(type));
				bool flag = absorbValue <= 0;
				if (!flag)
				{
					srcChar.ChangeCurrMainAttribute(context, type, -absorbValue);
					dstChar.ChangeCurrMainAttribute(context, type, absorbValue);
					base.ShowSpecialEffectTipsOnceInFrame(0);
				}
			}
		}

		// Token: 0x04001269 RID: 4713
		private const int AbsorbMainAttributeValue = 3;

		// Token: 0x0400126A RID: 4714
		private static readonly sbyte[] DirectAbsorbMainAttributeTypes = new sbyte[]
		{
			0,
			3,
			2
		};

		// Token: 0x0400126B RID: 4715
		private static readonly sbyte[] ReverseAbsorbMainAttributeTypes = new sbyte[]
		{
			1,
			4,
			5
		};
	}
}
