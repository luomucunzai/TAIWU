using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using GameData.Common;
using GameData.Domains.Character;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Defense;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Defense
{
	// Token: 0x020002AE RID: 686
	public class JiuBao : DefenseSkillBase
	{
		// Token: 0x060031F3 RID: 12787 RVA: 0x0021CE4F File Offset: 0x0021B04F
		public JiuBao()
		{
		}

		// Token: 0x060031F4 RID: 12788 RVA: 0x0021CE59 File Offset: 0x0021B059
		public JiuBao(CombatSkillKey skillKey) : base(skillKey, 16303)
		{
		}

		// Token: 0x060031F5 RID: 12789 RVA: 0x0021CE6C File Offset: 0x0021B06C
		public override void OnDisable(DataContext context)
		{
			base.OnDisable(context);
			bool flag = base.CombatChar.GetInjuries().GetSum() <= 0 || !base.SkillData.GetCanAffect();
			if (!flag)
			{
				Injuries injuries = base.CombatChar.GetInjuries();
				Injuries oldInjuries = base.CombatChar.GetOldInjuries();
				for (sbyte i = 0; i < 7; i += 1)
				{
					bool flag2 = !DomainManager.Combat.CheckBodyPartInjury(base.CombatChar, i, false);
					if (!flag2)
					{
						ValueTuple<sbyte, sbyte> valueTuple = injuries.Get(i);
						sbyte outer = valueTuple.Item1;
						sbyte inner = valueTuple.Item2;
						ValueTuple<sbyte, sbyte> valueTuple2 = oldInjuries.Get(i);
						sbyte outerOld = valueTuple2.Item1;
						sbyte innerOld = valueTuple2.Item2;
						injuries.Change(i, false, (int)((sbyte)(-(sbyte)Math.Min((int)(outer - outerOld), 3))));
						injuries.Change(i, true, (int)((sbyte)(-(sbyte)Math.Min((int)(inner - innerOld), 3))));
					}
				}
				Injuries newInjuries = injuries.Subtract(oldInjuries);
				List<ValueTuple<bool, bool>> injuryRandomPool = new List<ValueTuple<bool, bool>>();
				injuryRandomPool.Clear();
				for (sbyte bodyPart = 0; bodyPart < 7; bodyPart += 1)
				{
					ValueTuple<sbyte, sbyte> oldInjury = oldInjuries.Get(bodyPart);
					ValueTuple<sbyte, sbyte> newInjury = newInjuries.Get(bodyPart);
					for (int j = 0; j < (int)oldInjury.Item1; j++)
					{
						injuryRandomPool.Add(new ValueTuple<bool, bool>(false, true));
					}
					for (int k = 0; k < (int)oldInjury.Item2; k++)
					{
						injuryRandomPool.Add(new ValueTuple<bool, bool>(true, true));
					}
					for (int l = 0; l < (int)newInjury.Item1; l++)
					{
						injuryRandomPool.Add(new ValueTuple<bool, bool>(false, false));
					}
					for (int m = 0; m < (int)newInjury.Item2; m++)
					{
						injuryRandomPool.Add(new ValueTuple<bool, bool>(true, false));
					}
				}
				int average = injuryRandomPool.Count / 7;
				int remainder = injuryRandomPool.Count % 7;
				injuries.Initialize();
				oldInjuries.Initialize();
				for (sbyte bodyPart2 = 0; bodyPart2 < 7; bodyPart2 += 1)
				{
					int injuryCount = 0;
					for (int n = 0; n < average; n++)
					{
						JiuBao.AllocationInjury(context, injuryRandomPool, bodyPart2, ref injuries, ref oldInjuries, ref injuryCount);
					}
					bool flag3 = remainder > 0 && context.Random.CheckProb(remainder, (int)(7 - bodyPart2));
					if (flag3)
					{
						remainder--;
						JiuBao.AllocationInjury(context, injuryRandomPool, bodyPart2, ref injuries, ref oldInjuries, ref injuryCount);
					}
				}
				base.CombatChar.SetOldInjuries(oldInjuries, context);
				DomainManager.Combat.SetInjuries(context, base.CombatChar, injuries, true, true);
				DomainManager.Combat.UpdateBodyDefeatMark(context, base.CombatChar);
				base.ShowSpecialEffectTips(0);
			}
		}

		// Token: 0x060031F6 RID: 12790 RVA: 0x0021D138 File Offset: 0x0021B338
		private static void AllocationInjury(DataContext context, [TupleElementNames(new string[]
		{
			"outer",
			"old"
		})] List<ValueTuple<bool, bool>> injuryRandomPool, sbyte bodyPart, ref Injuries injuries, ref Injuries oldInjuries, ref int injuryCount)
		{
			int index = context.Random.Next(0, injuryRandomPool.Count);
			ValueTuple<bool, bool> injuryInfo = injuryRandomPool[index];
			CollectionUtils.SwapAndRemove<ValueTuple<bool, bool>>(injuryRandomPool, index);
			injuries.Change(bodyPart, injuryInfo.Item1, 1);
			bool item = injuryInfo.Item2;
			if (item)
			{
				oldInjuries.Change(bodyPart, injuryInfo.Item1, 1);
			}
			else
			{
				injuryCount++;
			}
		}

		// Token: 0x04000ECC RID: 3788
		private const int ReduceInjuryCount = 3;
	}
}
