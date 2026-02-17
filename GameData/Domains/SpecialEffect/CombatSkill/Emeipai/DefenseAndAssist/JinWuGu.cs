using System;
using System.Collections.Generic;
using Config;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Defense;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.Emeipai.DefenseAndAssist
{
	// Token: 0x02000560 RID: 1376
	public class JinWuGu : DefenseSkillBase
	{
		// Token: 0x060040A8 RID: 16552 RVA: 0x0025F3EE File Offset: 0x0025D5EE
		public JinWuGu()
		{
		}

		// Token: 0x060040A9 RID: 16553 RVA: 0x0025F3F8 File Offset: 0x0025D5F8
		public JinWuGu(CombatSkillKey skillKey) : base(skillKey, 2603)
		{
		}

		// Token: 0x060040AA RID: 16554 RVA: 0x0025F408 File Offset: 0x0025D608
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			Events.RegisterHandler_NormalAttackEnd(new Events.OnNormalAttackEnd(this.OnNormalAttackEnd));
			Events.RegisterHandler_AttackSkillAttackEnd(new Events.OnAttackSkillAttackEnd(this.OnAttackSkillAttackEnd));
		}

		// Token: 0x060040AB RID: 16555 RVA: 0x0025F437 File Offset: 0x0025D637
		public override void OnDisable(DataContext context)
		{
			base.OnDisable(context);
			Events.UnRegisterHandler_NormalAttackEnd(new Events.OnNormalAttackEnd(this.OnNormalAttackEnd));
			Events.UnRegisterHandler_AttackSkillAttackEnd(new Events.OnAttackSkillAttackEnd(this.OnAttackSkillAttackEnd));
		}

		// Token: 0x060040AC RID: 16556 RVA: 0x0025F468 File Offset: 0x0025D668
		private void OnNormalAttackEnd(DataContext context, CombatCharacter attacker, CombatCharacter defender, sbyte trickType, int pursueIndex, bool hit, bool isFightBack)
		{
			bool flag = defender != base.CombatChar || hit || !base.CanAffect || attacker.NormalAttackHitType != 2;
			if (!flag)
			{
				this.DoEffect(context);
			}
		}

		// Token: 0x060040AD RID: 16557 RVA: 0x0025F4AC File Offset: 0x0025D6AC
		private void OnAttackSkillAttackEnd(CombatContext context, sbyte hitType, bool hit, int index)
		{
			bool flag = context.Defender != base.CombatChar || hit || !base.CanAffect || index > 2 || DomainManager.Combat.GetDamageCompareData().HitType[index] != 2;
			if (!flag)
			{
				this.DoEffect(context);
			}
		}

		// Token: 0x060040AE RID: 16558 RVA: 0x0025F50C File Offset: 0x0025D70C
		private void DoEffect(DataContext context)
		{
			bool isDirect = base.IsDirect;
			if (isDirect)
			{
				DomainManager.Combat.AddRandomTrick(context, base.CombatChar, 2);
				base.ShowSpecialEffectTips(0);
			}
			else
			{
				CombatCharacter enemyChar = DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly, false);
				IReadOnlyDictionary<int, sbyte> trickDict = enemyChar.GetTricks().Tricks;
				bool flag = trickDict.Count == 0;
				if (!flag)
				{
					List<NeedTrick> removeTricks = ObjectPool<List<NeedTrick>>.Instance.Get();
					List<int> keyList = ObjectPool<List<int>>.Instance.Get();
					keyList.Clear();
					removeTricks.Clear();
					keyList.AddRange(trickDict.Keys);
					keyList.RemoveAll((int key) => enemyChar.IsTrickUseless(trickDict[key]));
					bool flag2 = keyList.Count > 0;
					if (flag2)
					{
						int removeCount = Math.Min(2, keyList.Count);
						for (int i = 0; i < removeCount; i++)
						{
							int index = context.Random.Next(0, keyList.Count);
							removeTricks.Add(new NeedTrick(trickDict[keyList[index]], 1));
							keyList.RemoveAt(index);
						}
						DomainManager.Combat.RemoveTrick(context, enemyChar, removeTricks, false, false, -1);
						base.ShowSpecialEffectTips(0);
					}
					ObjectPool<List<int>>.Instance.Return(keyList);
					ObjectPool<List<NeedTrick>>.Instance.Return(removeTricks);
				}
			}
		}

		// Token: 0x040012FE RID: 4862
		private const sbyte TrickCount = 2;
	}
}
