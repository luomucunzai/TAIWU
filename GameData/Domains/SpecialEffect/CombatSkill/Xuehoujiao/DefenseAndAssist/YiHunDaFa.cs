using System;
using System.Collections.Generic;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Defense;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.Xuehoujiao.DefenseAndAssist
{
	// Token: 0x02000247 RID: 583
	public class YiHunDaFa : DefenseSkillBase
	{
		// Token: 0x06002FF0 RID: 12272 RVA: 0x002152CF File Offset: 0x002134CF
		public YiHunDaFa()
		{
		}

		// Token: 0x06002FF1 RID: 12273 RVA: 0x002152D9 File Offset: 0x002134D9
		public YiHunDaFa(CombatSkillKey skillKey) : base(skillKey, 15704)
		{
		}

		// Token: 0x06002FF2 RID: 12274 RVA: 0x002152E9 File Offset: 0x002134E9
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			Events.RegisterHandler_NormalAttackEnd(new Events.OnNormalAttackEnd(this.OnNormalAttackEnd));
		}

		// Token: 0x06002FF3 RID: 12275 RVA: 0x00215306 File Offset: 0x00213506
		public override void OnDisable(DataContext context)
		{
			base.OnDisable(context);
			Events.UnRegisterHandler_NormalAttackEnd(new Events.OnNormalAttackEnd(this.OnNormalAttackEnd));
		}

		// Token: 0x06002FF4 RID: 12276 RVA: 0x00215324 File Offset: 0x00213524
		private void OnNormalAttackEnd(DataContext context, CombatCharacter attacker, CombatCharacter defender, sbyte trickType, int pursueIndex, bool hit, bool isFightBack)
		{
			bool flag = !isFightBack || !hit || attacker != base.CombatChar || !base.CanAffect;
			if (!flag)
			{
				Injuries newInjuries = base.CombatChar.GetInjuries().Subtract(base.CombatChar.GetOldInjuries());
				List<sbyte> injuryRandomPool = ObjectPool<List<sbyte>>.Instance.Get();
				injuryRandomPool.Clear();
				for (sbyte part = 0; part < 7; part += 1)
				{
					sbyte injury = newInjuries.Get(part, !base.IsDirect);
					for (int i = 0; i < (int)injury; i++)
					{
						injuryRandomPool.Add(part);
					}
				}
				int transferInjury = Math.Min(2, injuryRandomPool.Count);
				for (int j = 0; j < transferInjury; j++)
				{
					int index = context.Random.Next(0, injuryRandomPool.Count);
					sbyte bodyPart = injuryRandomPool[index];
					DomainManager.Combat.RemoveInjury(context, base.CombatChar, bodyPart, !base.IsDirect, 1, true, false);
					DomainManager.Combat.AddInjury(context, base.CurrEnemyChar, bodyPart, !base.IsDirect, 1, true, false);
				}
				ObjectPool<List<sbyte>>.Instance.Return(injuryRandomPool);
				bool flag2 = transferInjury > 0;
				if (flag2)
				{
					DomainManager.Combat.AddToCheckFallenSet(base.CurrEnemyChar.GetId());
					base.ShowSpecialEffectTips(0);
				}
				int transferQiDisorder = (int)(this.CharObj.GetDisorderOfQi() * 15 / 100);
				bool flag3 = transferQiDisorder > 0;
				if (flag3)
				{
					DomainManager.Combat.TransferDisorderOfQi(context, base.CombatChar, base.CurrEnemyChar, transferQiDisorder);
					base.ShowSpecialEffectTips(1);
				}
			}
		}

		// Token: 0x04000E38 RID: 3640
		private const sbyte TransferInjury = 2;

		// Token: 0x04000E39 RID: 3641
		private const sbyte TransferQiDisorder = 15;
	}
}
