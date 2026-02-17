using System;
using System.Collections.Generic;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Defense;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.Emeipai.DefenseAndAssist
{
	// Token: 0x02000563 RID: 1379
	public class ShengDengQiWuJue : DefenseSkillBase
	{
		// Token: 0x060040C3 RID: 16579 RVA: 0x0025FC6F File Offset: 0x0025DE6F
		public ShengDengQiWuJue()
		{
		}

		// Token: 0x060040C4 RID: 16580 RVA: 0x0025FC84 File Offset: 0x0025DE84
		public ShengDengQiWuJue(CombatSkillKey skillKey) : base(skillKey, 2608)
		{
		}

		// Token: 0x060040C5 RID: 16581 RVA: 0x0025FC9F File Offset: 0x0025DE9F
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			Events.RegisterHandler_NormalAttackEnd(new Events.OnNormalAttackEnd(this.OnNormalAttackEnd));
		}

		// Token: 0x060040C6 RID: 16582 RVA: 0x0025FCBC File Offset: 0x0025DEBC
		public override void OnDisable(DataContext context)
		{
			base.OnDisable(context);
			Events.UnRegisterHandler_NormalAttackEnd(new Events.OnNormalAttackEnd(this.OnNormalAttackEnd));
		}

		// Token: 0x060040C7 RID: 16583 RVA: 0x0025FCDC File Offset: 0x0025DEDC
		private void OnNormalAttackEnd(DataContext context, CombatCharacter attacker, CombatCharacter defender, sbyte trickType, int pursueIndex, bool hit, bool isFightBack)
		{
			bool flag = !isFightBack || !hit || attacker != base.CombatChar || !base.CanAffect;
			if (!flag)
			{
				bool isDirect = base.IsDirect;
				if (isDirect)
				{
					Injuries injuries = base.CombatChar.GetInjuries();
					Injuries newInjuries = injuries.Subtract(base.CombatChar.GetOldInjuries());
					List<sbyte> bodyPartRandomPool = ObjectPool<List<sbyte>>.Instance.Get();
					bodyPartRandomPool.Clear();
					for (sbyte part = 0; part < 7; part += 1)
					{
						ValueTuple<sbyte, sbyte> markCount = newInjuries.Get(part);
						for (int i = 0; i < (int)(markCount.Item1 + markCount.Item2); i++)
						{
							bodyPartRandomPool.Add(part);
						}
					}
					bool flag2 = bodyPartRandomPool.Count > 0;
					if (flag2)
					{
						int removeCount = Math.Min(2, bodyPartRandomPool.Count);
						for (int j = 0; j < removeCount; j++)
						{
							sbyte part2 = bodyPartRandomPool[context.Random.Next(0, bodyPartRandomPool.Count)];
							bool isInner = newInjuries.Get(part2, true) > 0 && (newInjuries.Get(part2, false) <= 0 || context.Random.CheckPercentProb(50));
							injuries.Change(part2, isInner, -1);
							newInjuries.Change(part2, isInner, -1);
						}
						DomainManager.Combat.SetInjuries(context, base.CombatChar, injuries, true, true);
						base.ShowSpecialEffectTips(1);
					}
				}
				else
				{
					DefeatMarkCollection markCollection = base.CombatChar.GetDefeatMarkCollection();
					this._markRandomPool.Clear();
					for (sbyte part3 = 0; part3 < 7; part3 += 1)
					{
						for (int k = 0; k < markCollection.FlawMarkList[(int)part3].Count; k++)
						{
							this._markRandomPool.Add(new ValueTuple<sbyte, sbyte>(0, part3));
						}
						for (int l = 0; l < markCollection.AcupointMarkList[(int)part3].Count; l++)
						{
							this._markRandomPool.Add(new ValueTuple<sbyte, sbyte>(1, part3));
						}
					}
					for (int m = 0; m < markCollection.MindMarkList.Count; m++)
					{
						this._markRandomPool.Add(new ValueTuple<sbyte, sbyte>(2, -1));
					}
					bool flag3 = this._markRandomPool.Count > 0;
					if (flag3)
					{
						int removeCount2 = Math.Min(2, this._markRandomPool.Count);
						for (int n = 0; n < removeCount2; n++)
						{
							int index = context.Random.Next(0, this._markRandomPool.Count);
							ValueTuple<sbyte, sbyte> mark = this._markRandomPool[index];
							bool flag4 = mark.Item1 == 0;
							if (flag4)
							{
								DomainManager.Combat.RemoveFlaw(context, base.CombatChar, mark.Item2, context.Random.Next(0, (int)base.CombatChar.GetFlawCount()[(int)mark.Item2]), true, true);
							}
							else
							{
								bool flag5 = mark.Item1 == 1;
								if (flag5)
								{
									DomainManager.Combat.RemoveAcupoint(context, base.CombatChar, mark.Item2, context.Random.Next(0, (int)base.CombatChar.GetAcupointCount()[(int)mark.Item2]), true, true);
								}
								else
								{
									DomainManager.Combat.RemoveMindDefeatMark(context, base.CombatChar, 1, true, 0);
								}
							}
							this._markRandomPool.RemoveAt(index);
						}
						base.ShowSpecialEffectTips(1);
					}
				}
				DomainManager.Combat.AddRandomTrick(context, base.CombatChar, 3);
				base.ShowSpecialEffectTips(0);
			}
		}

		// Token: 0x04001308 RID: 4872
		private const int FreeTrickCount = 3;

		// Token: 0x04001309 RID: 4873
		private const sbyte HealMarkCount = 2;

		// Token: 0x0400130A RID: 4874
		private readonly List<ValueTuple<sbyte, sbyte>> _markRandomPool = new List<ValueTuple<sbyte, sbyte>>();
	}
}
