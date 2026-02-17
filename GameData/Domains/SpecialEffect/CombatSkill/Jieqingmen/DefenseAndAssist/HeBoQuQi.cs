using System;
using System.Collections.Generic;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Defense;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.Jieqingmen.DefenseAndAssist
{
	// Token: 0x020004FC RID: 1276
	public class HeBoQuQi : DefenseSkillBase
	{
		// Token: 0x06003E66 RID: 15974 RVA: 0x00255B50 File Offset: 0x00253D50
		public HeBoQuQi()
		{
		}

		// Token: 0x06003E67 RID: 15975 RVA: 0x00255B5A File Offset: 0x00253D5A
		public HeBoQuQi(CombatSkillKey skillKey) : base(skillKey, 13501)
		{
		}

		// Token: 0x06003E68 RID: 15976 RVA: 0x00255B6A File Offset: 0x00253D6A
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			Events.RegisterHandler_NormalAttackEnd(new Events.OnNormalAttackEnd(this.OnNormalAttackEnd));
		}

		// Token: 0x06003E69 RID: 15977 RVA: 0x00255B87 File Offset: 0x00253D87
		public override void OnDisable(DataContext context)
		{
			base.OnDisable(context);
			Events.UnRegisterHandler_NormalAttackEnd(new Events.OnNormalAttackEnd(this.OnNormalAttackEnd));
		}

		// Token: 0x06003E6A RID: 15978 RVA: 0x00255BA4 File Offset: 0x00253DA4
		private void OnNormalAttackEnd(DataContext context, CombatCharacter attacker, CombatCharacter defender, sbyte trickType, int pursueIndex, bool hit, bool isFightBack)
		{
			bool flag = !isFightBack || !hit || attacker != base.CombatChar || !base.CanAffect;
			if (!flag)
			{
				CombatCharacter enemyChar = DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly, false);
				IReadOnlyDictionary<int, sbyte> trickDict = (base.IsDirect ? enemyChar : base.CombatChar).GetTricks().Tricks;
				List<sbyte> trickRandomPool = ObjectPool<List<sbyte>>.Instance.Get();
				trickRandomPool.Clear();
				trickRandomPool.AddRange(trickDict.Values);
				trickRandomPool.RemoveAll((sbyte type) => base.IsDirect != base.CombatChar.IsTrickUsable(type));
				bool flag2 = trickRandomPool.Count > 0;
				if (flag2)
				{
					sbyte moveTrick = trickRandomPool[context.Random.Next(0, trickRandomPool.Count)];
					bool isDirect = base.IsDirect;
					if (isDirect)
					{
						bool flag3 = DomainManager.Combat.RemoveTrick(context, enemyChar, moveTrick, 1, false, -1);
						if (flag3)
						{
							DomainManager.Combat.AddTrick(context, base.CombatChar, moveTrick, true);
						}
					}
					else
					{
						DomainManager.Combat.RemoveTrick(context, base.CombatChar, moveTrick, 1, true, -1);
						DomainManager.Combat.AddTrick(context, enemyChar, moveTrick, false);
					}
					base.ShowSpecialEffectTips(0);
				}
				ObjectPool<List<sbyte>>.Instance.Return(trickRandomPool);
			}
		}
	}
}
