using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.Character;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Defense;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Defense
{
	// Token: 0x020002AB RID: 683
	public class FengShenZhaoMing : DefenseSkillBase
	{
		// Token: 0x060031E3 RID: 12771 RVA: 0x0021C940 File Offset: 0x0021AB40
		public FengShenZhaoMing()
		{
		}

		// Token: 0x060031E4 RID: 12772 RVA: 0x0021C955 File Offset: 0x0021AB55
		public FengShenZhaoMing(CombatSkillKey skillKey) : base(skillKey, 16307)
		{
		}

		// Token: 0x060031E5 RID: 12773 RVA: 0x0021C970 File Offset: 0x0021AB70
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 116, -1, -1, -1, -1), EDataModifyType.Custom);
		}

		// Token: 0x060031E6 RID: 12774 RVA: 0x0021C9A4 File Offset: 0x0021ABA4
		public override void OnDisable(DataContext context)
		{
		}

		// Token: 0x060031E7 RID: 12775 RVA: 0x0021C9A8 File Offset: 0x0021ABA8
		public override OuterAndInnerInts GetModifiedValue(AffectedDataKey dataKey, OuterAndInnerInts dataValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || !base.CanAffect;
			OuterAndInnerInts result;
			if (flag)
			{
				result = dataValue;
			}
			else
			{
				DataContext context = DomainManager.Combat.Context;
				Injuries newInjuries = base.CombatChar.GetInjuries().Subtract(base.CombatChar.GetOldInjuries());
				bool flag2 = dataValue.Outer + dataValue.Inner <= 0 || newInjuries.GetSum() <= 0;
				if (flag2)
				{
					result = dataValue;
				}
				else
				{
					this._injuryRandomPool.Clear();
					for (sbyte part = 0; part < 7; part += 1)
					{
						ValueTuple<sbyte, sbyte> injury = newInjuries.Get(part);
						for (int i = 0; i < (int)injury.Item1; i++)
						{
							this._injuryRandomPool.Add(new ValueTuple<sbyte, bool>(part, false));
						}
						for (int j = 0; j < (int)injury.Item2; j++)
						{
							this._injuryRandomPool.Add(new ValueTuple<sbyte, bool>(part, true));
						}
					}
					while (dataValue.Outer + dataValue.Inner > 0 && this._injuryRandomPool.Count > 0)
					{
						int index = context.Random.Next(0, this._injuryRandomPool.Count);
						ValueTuple<sbyte, bool> injuryInfo = this._injuryRandomPool[index];
						this._injuryRandomPool.RemoveAt(index);
						DomainManager.Combat.ChangeToOldInjury(context, base.CombatChar, injuryInfo.Item1, injuryInfo.Item2, 1);
						bool flag3 = dataValue.Inner <= 0 || (dataValue.Outer > 0 && context.Random.CheckPercentProb(50));
						if (flag3)
						{
							dataValue.Outer--;
						}
						else
						{
							dataValue.Inner--;
						}
					}
					base.ShowSpecialEffectTips(0);
					result = dataValue;
				}
			}
			return result;
		}

		// Token: 0x04000EC9 RID: 3785
		private readonly List<ValueTuple<sbyte, bool>> _injuryRandomPool = new List<ValueTuple<sbyte, bool>>();
	}
}
