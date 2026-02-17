using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Defense;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Defense
{
	// Token: 0x020002AF RID: 687
	public class SanYuanJiuDunTianDiBian : DefenseSkillBase
	{
		// Token: 0x060031F7 RID: 12791 RVA: 0x0021D19C File Offset: 0x0021B39C
		public SanYuanJiuDunTianDiBian()
		{
		}

		// Token: 0x060031F8 RID: 12792 RVA: 0x0021D1B1 File Offset: 0x0021B3B1
		public SanYuanJiuDunTianDiBian(CombatSkillKey skillKey) : base(skillKey, 16308)
		{
		}

		// Token: 0x060031F9 RID: 12793 RVA: 0x0021D1CC File Offset: 0x0021B3CC
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			this._frameCounter = 0;
			Events.RegisterHandler_CombatStateMachineUpdateEnd(new Events.OnCombatStateMachineUpdateEnd(this.OnStateMachineUpdateEnd));
		}

		// Token: 0x060031FA RID: 12794 RVA: 0x0021D1F0 File Offset: 0x0021B3F0
		public override void OnDisable(DataContext context)
		{
			base.OnDisable(context);
			Events.UnRegisterHandler_CombatStateMachineUpdateEnd(new Events.OnCombatStateMachineUpdateEnd(this.OnStateMachineUpdateEnd));
		}

		// Token: 0x060031FB RID: 12795 RVA: 0x0021D210 File Offset: 0x0021B410
		private void OnStateMachineUpdateEnd(DataContext context, CombatCharacter combatChar)
		{
			bool flag = base.CombatChar != combatChar || DomainManager.Combat.Pause;
			if (!flag)
			{
				this._frameCounter++;
				bool flag2 = this._frameCounter < 120 || !base.CanAffect;
				if (!flag2)
				{
					this._frameCounter = 0;
					this._markRandomPool.Clear();
					CombatCharacter enemyChar = DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly, false);
					DefeatMarkCollection selfMarks = base.CombatChar.GetDefeatMarkCollection();
					DefeatMarkCollection enemyMarks = enemyChar.GetDefeatMarkCollection();
					for (sbyte bodyPart = 0; bodyPart < 7; bodyPart += 1)
					{
						for (int i = 0; i < Math.Min((int)selfMarks.OuterInjuryMarkList[(int)bodyPart], (int)(6 - enemyMarks.OuterInjuryMarkList[(int)bodyPart])); i++)
						{
							this._markRandomPool.Add(new ValueTuple<sbyte, sbyte>(0, bodyPart));
						}
						for (int j = 0; j < Math.Min((int)selfMarks.InnerInjuryMarkList[(int)bodyPart], (int)(6 - enemyMarks.InnerInjuryMarkList[(int)bodyPart])); j++)
						{
							this._markRandomPool.Add(new ValueTuple<sbyte, sbyte>(1, bodyPart));
						}
						for (int k = 0; k < Math.Min(selfMarks.FlawMarkList[(int)bodyPart].Count, enemyChar.GetMaxFlawCount() - enemyMarks.FlawMarkList[(int)bodyPart].Count); k++)
						{
							this._markRandomPool.Add(new ValueTuple<sbyte, sbyte>(2, bodyPart));
						}
						for (int l = 0; l < Math.Min(selfMarks.AcupointMarkList[(int)bodyPart].Count, enemyChar.GetMaxAcupointCount() - enemyMarks.AcupointMarkList[(int)bodyPart].Count); l++)
						{
							this._markRandomPool.Add(new ValueTuple<sbyte, sbyte>(3, bodyPart));
						}
					}
					for (int m = 0; m < selfMarks.MindMarkList.Count; m++)
					{
						this._markRandomPool.Add(new ValueTuple<sbyte, sbyte>(4, -1));
					}
					for (int n = 0; n < selfMarks.DieMarkList.Count; n++)
					{
						this._markRandomPool.Add(new ValueTuple<sbyte, sbyte>(5, -1));
					}
					bool flag3 = this._markRandomPool.Count <= 0;
					if (!flag3)
					{
						ValueTuple<sbyte, sbyte> markInfo = this._markRandomPool[context.Random.Next(0, this._markRandomPool.Count)];
						bool flag4 = markInfo.Item1 == 0 || markInfo.Item1 == 1;
						if (flag4)
						{
							Injuries injuries = base.CombatChar.GetInjuries();
							Injuries oldInjuries = base.CombatChar.GetOldInjuries();
							bool inner = markInfo.Item1 == 1;
							int oldInjury = (int)oldInjuries.Get(markInfo.Item2, inner);
							bool changeOldInjury = oldInjury > 0 && context.Random.CheckProb(oldInjury, (int)injuries.Get(markInfo.Item2, inner));
							bool flag5 = changeOldInjury;
							if (flag5)
							{
								oldInjuries.Change(markInfo.Item2, inner, -1);
								base.CombatChar.SetOldInjuries(oldInjuries, context);
							}
							injuries.Change(markInfo.Item2, inner, -1);
							DomainManager.Combat.SetInjuries(context, base.CombatChar, injuries, true, true);
						}
						else
						{
							bool flag6 = markInfo.Item1 == 2;
							if (flag6)
							{
								DomainManager.Combat.RemoveFlaw(context, base.CombatChar, markInfo.Item2, context.Random.Next(0, selfMarks.FlawMarkList[(int)markInfo.Item2].Count), true, true);
							}
							else
							{
								bool flag7 = markInfo.Item1 == 3;
								if (flag7)
								{
									DomainManager.Combat.RemoveAcupoint(context, base.CombatChar, markInfo.Item2, context.Random.Next(0, selfMarks.AcupointMarkList[(int)markInfo.Item2].Count), true, true);
								}
								else
								{
									bool flag8 = markInfo.Item1 == 4;
									if (flag8)
									{
										DomainManager.Combat.RemoveMindDefeatMark(context, base.CombatChar, 1, true, 0);
									}
									else
									{
										bool flag9 = markInfo.Item1 == 5;
										if (flag9)
										{
											int index = context.Random.Next(0, selfMarks.DieMarkList.Count);
											selfMarks.DieMarkList.RemoveAt(index);
											base.CombatChar.SetDefeatMarkCollection(selfMarks, context);
											DomainManager.Combat.AddToCheckFallenSet(enemyChar.GetId());
										}
									}
								}
							}
						}
						DomainManager.Combat.ChangeBreathValue(context, base.CombatChar, 30000 * SanYuanJiuDunTianDiBian.AddStanceAndBreathPercent, false, null);
						DomainManager.Combat.ChangeStanceValue(context, base.CombatChar, 4000 * SanYuanJiuDunTianDiBian.AddStanceAndBreathPercent, false, null);
						base.ShowSpecialEffectTips(0);
					}
				}
			}
		}

		// Token: 0x04000ECD RID: 3789
		private const sbyte AffectFrame = 120;

		// Token: 0x04000ECE RID: 3790
		private static readonly CValuePercent AddStanceAndBreathPercent = 10;

		// Token: 0x04000ECF RID: 3791
		private int _frameCounter;

		// Token: 0x04000ED0 RID: 3792
		[TupleElementNames(new string[]
		{
			"type",
			"bodyPart"
		})]
		private readonly List<ValueTuple<sbyte, sbyte>> _markRandomPool = new List<ValueTuple<sbyte, sbyte>>();
	}
}
