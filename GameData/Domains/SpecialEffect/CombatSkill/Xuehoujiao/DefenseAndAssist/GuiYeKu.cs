using System;
using System.Linq;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Defense;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.Xuehoujiao.DefenseAndAssist
{
	// Token: 0x0200023F RID: 575
	public class GuiYeKu : DefenseSkillBase
	{
		// Token: 0x06002FB4 RID: 12212 RVA: 0x0021413B File Offset: 0x0021233B
		public GuiYeKu()
		{
		}

		// Token: 0x06002FB5 RID: 12213 RVA: 0x0021415C File Offset: 0x0021235C
		public GuiYeKu(CombatSkillKey skillKey) : base(skillKey, 15702)
		{
		}

		// Token: 0x06002FB6 RID: 12214 RVA: 0x00214183 File Offset: 0x00212383
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			Events.RegisterHandler_NormalAttackEnd(new Events.OnNormalAttackEnd(this.OnNormalAttackEnd));
		}

		// Token: 0x06002FB7 RID: 12215 RVA: 0x002141A0 File Offset: 0x002123A0
		public override void OnDisable(DataContext context)
		{
			base.OnDisable(context);
			Events.UnRegisterHandler_NormalAttackEnd(new Events.OnNormalAttackEnd(this.OnNormalAttackEnd));
		}

		// Token: 0x06002FB8 RID: 12216 RVA: 0x002141C0 File Offset: 0x002123C0
		private unsafe void OnNormalAttackEnd(DataContext context, CombatCharacter attacker, CombatCharacter defender, sbyte trickType, int pursueIndex, bool hit, bool isFightBack)
		{
			bool flag = !isFightBack || !hit || attacker != base.CombatChar || !base.CanAffect;
			if (!flag)
			{
				CombatCharacter enemyChar = DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly, false);
				Personalities selfPersonalities = this.CharObj.GetPersonalities();
				Personalities enemyPersonalities = enemyChar.GetCharacter().GetPersonalities();
				int count = this._requirePersonalities.Count((sbyte type) => *selfPersonalities[(int)type] > *enemyPersonalities[(int)type] && context.Random.CheckPercentProb(this.IsDirect ? 50 : 20));
				bool flag2 = count <= 0;
				if (!flag2)
				{
					bool isDirect = base.IsDirect;
					if (isDirect)
					{
						DomainManager.Combat.AddTrick(context, enemyChar, 20, count, false, false);
					}
					else
					{
						DomainManager.Combat.AppendMindDefeatMark(context, enemyChar, count, -1, false);
					}
					base.ShowSpecialEffectTips(0);
				}
			}
		}

		// Token: 0x04000E20 RID: 3616
		private readonly sbyte[] _requirePersonalities = new sbyte[]
		{
			0,
			1,
			2,
			3,
			4
		};

		// Token: 0x04000E21 RID: 3617
		private const sbyte AffectOddsDirect = 50;

		// Token: 0x04000E22 RID: 3618
		private const sbyte AffectOddsReverse = 20;
	}
}
