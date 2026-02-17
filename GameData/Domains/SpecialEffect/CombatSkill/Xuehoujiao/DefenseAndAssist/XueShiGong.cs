using System;
using System.Collections.Generic;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Assist;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.Xuehoujiao.DefenseAndAssist
{
	// Token: 0x02000245 RID: 581
	public class XueShiGong : AssistSkillBase
	{
		// Token: 0x06002FDA RID: 12250 RVA: 0x00214B78 File Offset: 0x00212D78
		public XueShiGong()
		{
		}

		// Token: 0x06002FDB RID: 12251 RVA: 0x00214B82 File Offset: 0x00212D82
		public XueShiGong(CombatSkillKey skillKey) : base(skillKey, 15803)
		{
		}

		// Token: 0x06002FDC RID: 12252 RVA: 0x00214B92 File Offset: 0x00212D92
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			Events.RegisterHandler_AddDirectInjury(new Events.OnAddDirectInjury(this.OnAddDirectInjury));
		}

		// Token: 0x06002FDD RID: 12253 RVA: 0x00214BAF File Offset: 0x00212DAF
		public override void OnDisable(DataContext context)
		{
			base.OnDisable(context);
			Events.UnRegisterHandler_AddDirectInjury(new Events.OnAddDirectInjury(this.OnAddDirectInjury));
		}

		// Token: 0x06002FDE RID: 12254 RVA: 0x00214BCC File Offset: 0x00212DCC
		private void OnAddDirectInjury(DataContext context, int attackerId, int defenderId, bool isAlly, sbyte bodyPart, sbyte outerMarkCount, sbyte innerMarkCount, short combatSkillId)
		{
			sbyte markCount = base.IsDirect ? outerMarkCount : innerMarkCount;
			bool flag = attackerId != base.CharacterId || markCount < 1 || !base.CanAffect;
			if (!flag)
			{
				bool anyChanged = false;
				for (int i = 0; i < (int)markCount; i++)
				{
					Injuries injuries = base.CombatChar.GetInjuries();
					Injuries newInjuries = injuries.Subtract(base.CombatChar.GetOldInjuries());
					bool flag2 = !this.AnyValidInjuries(ref injuries, ref newInjuries);
					if (flag2)
					{
						break;
					}
					anyChanged = true;
					sbyte part = RandomUtils.GetRandomResult<sbyte>(this.InjuriesFilter(injuries, newInjuries), context.Random);
					DomainManager.Combat.RemoveInjury(context, base.CombatChar, part, !base.IsDirect, 1, true, false);
				}
				bool flag3 = anyChanged;
				if (flag3)
				{
					base.ShowEffectTips(context);
					base.ShowSpecialEffectTips(0);
				}
			}
		}

		// Token: 0x06002FDF RID: 12255 RVA: 0x00214CAC File Offset: 0x00212EAC
		private bool AnyValidInjuries(ref Injuries injuries, ref Injuries newInjuries)
		{
			for (sbyte part = 0; part < 7; part += 1)
			{
				sbyte level = injuries.Get(part, !base.IsDirect);
				bool flag = level > 4 || level <= 0;
				bool flag2 = flag;
				if (!flag2)
				{
					sbyte newLevel = newInjuries.Get(part, !base.IsDirect);
					bool flag3 = newLevel <= 0;
					if (!flag3)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06002FE0 RID: 12256 RVA: 0x00214D24 File Offset: 0x00212F24
		private IEnumerable<ValueTuple<sbyte, short>> InjuriesFilter(Injuries injuries, Injuries newInjuries)
		{
			sbyte b;
			for (sbyte part = 0; part < 7; part = b + 1)
			{
				sbyte level = injuries.Get(part, !base.IsDirect);
				bool flag = level > 4 || level <= 0;
				bool flag2 = flag;
				if (!flag2)
				{
					sbyte newLevel = newInjuries.Get(part, !base.IsDirect);
					bool flag3 = newLevel <= 0;
					if (!flag3)
					{
						yield return new ValueTuple<sbyte, short>(part, (short)newLevel);
					}
				}
				b = part;
			}
			yield break;
		}

		// Token: 0x04000E2E RID: 3630
		private const sbyte RequireMarkCount = 1;

		// Token: 0x04000E2F RID: 3631
		private const sbyte RequireInjuryLevelMax = 4;
	}
}
