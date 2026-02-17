using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Defense;

namespace GameData.Domains.SpecialEffect.CombatSkill.Wuxianjiao.DefenseAndAssist
{
	// Token: 0x020003A4 RID: 932
	public class GuiJiangDaFa : DefenseSkillBase
	{
		// Token: 0x06003698 RID: 13976 RVA: 0x00231249 File Offset: 0x0022F449
		public GuiJiangDaFa()
		{
		}

		// Token: 0x06003699 RID: 13977 RVA: 0x00231253 File Offset: 0x0022F453
		public GuiJiangDaFa(CombatSkillKey skillKey) : base(skillKey, 12707)
		{
		}

		// Token: 0x0600369A RID: 13978 RVA: 0x00231264 File Offset: 0x0022F464
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			if (this.AffectDatas == null)
			{
				this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>
				{
					{
						new AffectedDataKey(base.CharacterId, 102, -1, -1, -1, -1),
						EDataModifyType.AddPercent
					}
				};
			}
			Events.RegisterHandler_NormalAttackEnd(new Events.OnNormalAttackEnd(this.OnNormalAttackEnd));
			Events.RegisterHandler_AttackSkillAttackEnd(new Events.OnAttackSkillAttackEnd(this.OnAttackSkillAttackEnd));
		}

		// Token: 0x0600369B RID: 13979 RVA: 0x002312CA File Offset: 0x0022F4CA
		public override void OnDisable(DataContext context)
		{
			base.OnDisable(context);
			Events.UnRegisterHandler_NormalAttackEnd(new Events.OnNormalAttackEnd(this.OnNormalAttackEnd));
			Events.UnRegisterHandler_AttackSkillAttackEnd(new Events.OnAttackSkillAttackEnd(this.OnAttackSkillAttackEnd));
		}

		// Token: 0x0600369C RID: 13980 RVA: 0x002312FC File Offset: 0x0022F4FC
		private void OnNormalAttackEnd(DataContext context, CombatCharacter attacker, CombatCharacter defender, sbyte trickType, int pursueIndex, bool hit, bool isFightBack)
		{
			bool flag = hit || pursueIndex > 0 || defender != base.CombatChar || !base.CanAffect;
			if (!flag)
			{
				this.DoEffect(context);
			}
		}

		// Token: 0x0600369D RID: 13981 RVA: 0x00231338 File Offset: 0x0022F538
		private void OnAttackSkillAttackEnd(CombatContext context, sbyte hitType, bool hit, int index)
		{
			bool flag = hit || index > 2 || context.Defender != base.CombatChar || !base.CanAffect;
			if (!flag)
			{
				this.DoEffect(context);
			}
		}

		// Token: 0x0600369E RID: 13982 RVA: 0x0023137C File Offset: 0x0022F57C
		private void DoEffect(DataContext context)
		{
			CombatCharacter enemyChar = base.CurrEnemyChar;
			bool isDirect = base.IsDirect;
			if (isDirect)
			{
				byte effectCount = enemyChar.GetTrickCount(20);
				bool flag = effectCount >= 2;
				if (flag)
				{
					int count = (int)(effectCount / 2);
					for (int i = 0; i < count; i++)
					{
						DomainManager.Combat.AddFlaw(context, base.CurrEnemyChar, 3, new CombatSkillKey(base.CharacterId, base.SkillTemplateId), -1, 1, true);
					}
					base.ShowSpecialEffectTips(1);
				}
				else
				{
					bool flag2 = effectCount == 0;
					if (flag2)
					{
						DomainManager.Combat.AddTrick(context, enemyChar, 20, 2, false, false);
						base.ShowSpecialEffectTips(2);
					}
				}
			}
			else
			{
				MindMarkList mindMarkTime = enemyChar.GetMindMarkTime();
				int? num;
				if (mindMarkTime == null)
				{
					num = null;
				}
				else
				{
					List<SilenceFrameData> markList = mindMarkTime.MarkList;
					num = ((markList != null) ? new int?(markList.Count) : null);
				}
				int? num2 = num;
				int effectCount2 = num2.GetValueOrDefault();
				bool flag3 = effectCount2 >= 2;
				if (flag3)
				{
					int count2 = effectCount2 / 2;
					for (int j = 0; j < count2; j++)
					{
						DomainManager.Combat.AddAcupoint(context, base.CurrEnemyChar, 3, new CombatSkillKey(base.CharacterId, base.SkillTemplateId), -1, 1, true);
					}
					base.ShowSpecialEffectTips(1);
				}
				else
				{
					bool flag4 = effectCount2 == 0;
					if (flag4)
					{
						DomainManager.Combat.AppendMindDefeatMark(context, base.CurrEnemyChar, 2, -1, false);
						base.ShowSpecialEffectTips(2);
					}
				}
			}
		}

		// Token: 0x0600369F RID: 13983 RVA: 0x002314F8 File Offset: 0x0022F6F8
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || !base.CanAffect;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				bool flag2 = dataKey.FieldId == 102 && dataKey.CustomParam2 == 0;
				if (flag2)
				{
					base.ShowSpecialEffectTips(0);
					result = -60;
				}
				else
				{
					result = 0;
				}
			}
			return result;
		}

		// Token: 0x04000FE5 RID: 4069
		private const sbyte RequireWuOrMindUnit = 2;

		// Token: 0x04000FE6 RID: 4070
		private const sbyte FlawOrAcupointLevel = 3;

		// Token: 0x04000FE7 RID: 4071
		private const int ChangeDirectDamagePercent = -60;
	}
}
