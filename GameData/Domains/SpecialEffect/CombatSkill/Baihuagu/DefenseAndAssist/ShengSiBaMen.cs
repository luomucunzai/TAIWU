using System;
using System.Collections.Generic;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Assist;
using GameData.Utilities;
using Redzen.Random;

namespace GameData.Domains.SpecialEffect.CombatSkill.Baihuagu.DefenseAndAssist
{
	// Token: 0x020005D9 RID: 1497
	public class ShengSiBaMen : AssistSkillBase
	{
		// Token: 0x0600443B RID: 17467 RVA: 0x0026EA27 File Offset: 0x0026CC27
		public ShengSiBaMen()
		{
		}

		// Token: 0x0600443C RID: 17468 RVA: 0x0026EA31 File Offset: 0x0026CC31
		public ShengSiBaMen(CombatSkillKey skillKey) : base(skillKey, 3606)
		{
		}

		// Token: 0x0600443D RID: 17469 RVA: 0x0026EA41 File Offset: 0x0026CC41
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			Events.RegisterHandler_AddDirectInjury(new Events.OnAddDirectInjury(this.OnAddDirectInjury));
			Events.RegisterHandler_AddDirectFatalDamageMark(new Events.OnAddDirectFatalDamageMark(this.OnAddDirectFatalDamageMark));
		}

		// Token: 0x0600443E RID: 17470 RVA: 0x0026EA70 File Offset: 0x0026CC70
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_AddDirectInjury(new Events.OnAddDirectInjury(this.OnAddDirectInjury));
			Events.UnRegisterHandler_AddDirectFatalDamageMark(new Events.OnAddDirectFatalDamageMark(this.OnAddDirectFatalDamageMark));
			base.OnDisable(context);
		}

		// Token: 0x0600443F RID: 17471 RVA: 0x0026EA9F File Offset: 0x0026CC9F
		private void OnAddDirectInjury(DataContext context, int attackerId, int defenderId, bool isAlly, sbyte bodyPart, sbyte outerMarkCount, sbyte innerMarkCount, short combatSkillId)
		{
			this.TryAffect(context, attackerId, defenderId, bodyPart, (int)outerMarkCount, (int)innerMarkCount);
		}

		// Token: 0x06004440 RID: 17472 RVA: 0x0026EAB2 File Offset: 0x0026CCB2
		private void OnAddDirectFatalDamageMark(DataContext context, int attackerId, int defenderId, bool isAlly, sbyte bodyPart, int outerMarkCount, int innerMarkCount, short combatSkillId)
		{
			this.TryAffect(context, attackerId, defenderId, bodyPart, outerMarkCount, innerMarkCount);
		}

		// Token: 0x06004441 RID: 17473 RVA: 0x0026EAC8 File Offset: 0x0026CCC8
		private void TryAffect(DataContext context, int attackerId, int defenderId, sbyte part, int outer, int inner)
		{
			CombatCharacter attacker = DomainManager.Combat.GetElement_CombatCharacterDict(attackerId);
			CombatCharacter defender = DomainManager.Combat.GetElement_CombatCharacterDict(defenderId);
			bool flag = !this.IsAffectMaker(attacker) || !this.IsAffectChar(defender);
			if (!flag)
			{
				bool flag2 = inner > 0;
				if (flag2)
				{
					this.DoAffect(context, defender, part, true);
				}
				bool flag3 = outer > 0;
				if (flag3)
				{
					this.DoAffect(context, defender, part, false);
				}
			}
		}

		// Token: 0x06004442 RID: 17474 RVA: 0x0026EB38 File Offset: 0x0026CD38
		private bool IsAffectMaker(CombatCharacter maker)
		{
			bool isDirect = base.IsDirect;
			bool result;
			if (isDirect)
			{
				result = (maker.IsAlly != base.CombatChar.IsAlly);
			}
			else
			{
				result = (maker.GetId() == base.CharacterId);
			}
			return result;
		}

		// Token: 0x06004443 RID: 17475 RVA: 0x0026EB7C File Offset: 0x0026CD7C
		private bool IsAffectChar(CombatCharacter character)
		{
			bool isDirect = base.IsDirect;
			bool result;
			if (isDirect)
			{
				result = (character.GetId() == base.CharacterId);
			}
			else
			{
				result = (character.IsAlly != base.CombatChar.IsAlly);
			}
			return result;
		}

		// Token: 0x06004444 RID: 17476 RVA: 0x0026EBC0 File Offset: 0x0026CDC0
		private void DoAffect(DataContext context, CombatCharacter affectChar, sbyte bodyPart, bool inner)
		{
			bool flag = !this.TryGetTarget(affectChar, context.Random, ref bodyPart, ref inner);
			if (!flag)
			{
				bool isDirect = base.IsDirect;
				if (isDirect)
				{
					affectChar.RemoveInjury(context, bodyPart, inner, 1);
				}
				else
				{
					affectChar.WorsenInjury(context, bodyPart, inner);
				}
				base.ShowSpecialEffectTips(0);
			}
		}

		// Token: 0x06004445 RID: 17477 RVA: 0x0026EC14 File Offset: 0x0026CE14
		private bool TryGetTarget(CombatCharacter affectChar, IRandomSource random, ref sbyte bodyPart, ref bool inner)
		{
			int maxCount = 0;
			Injuries injuries = affectChar.GetInjuries();
			Injuries affectInjuries = base.IsDirect ? injuries.Subtract(affectChar.GetOldInjuries()) : injuries;
			List<sbyte> innerBodyParts = ObjectPool<List<sbyte>>.Instance.Get();
			List<sbyte> outerBodyParts = ObjectPool<List<sbyte>>.Instance.Get();
			for (sbyte i = 0; i < 7; i += 1)
			{
				bool flag = i == bodyPart;
				if (!flag)
				{
					sbyte innerCount = affectInjuries.Get(i, true);
					sbyte outerCount = affectInjuries.Get(i, false);
					bool flag2 = innerCount <= 0 && outerCount <= 0;
					if (!flag2)
					{
						bool flag3 = (int)innerCount > maxCount || (int)outerCount > maxCount;
						if (flag3)
						{
							maxCount = Math.Max(Math.Max(maxCount, (int)innerCount), (int)outerCount);
							innerBodyParts.Clear();
							outerBodyParts.Clear();
						}
						bool flag4 = (int)innerCount == maxCount;
						if (flag4)
						{
							innerBodyParts.Add(i);
						}
						bool flag5 = (int)outerCount == maxCount;
						if (flag5)
						{
							outerBodyParts.Add(i);
						}
					}
				}
			}
			bool anyInner = innerBodyParts.Count > 0;
			bool anyOuter = outerBodyParts.Count > 0;
			bool flag6 = anyInner || anyOuter;
			if (flag6)
			{
				inner = random.RandomIsInner(anyInner, anyOuter);
				bodyPart = (inner ? innerBodyParts : outerBodyParts).GetRandom(random);
			}
			ObjectPool<List<sbyte>>.Instance.Return(innerBodyParts);
			ObjectPool<List<sbyte>>.Instance.Return(outerBodyParts);
			return anyInner || anyOuter;
		}
	}
}
