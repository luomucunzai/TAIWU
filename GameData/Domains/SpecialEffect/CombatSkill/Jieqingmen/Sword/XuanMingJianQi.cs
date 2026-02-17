using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Attack;

namespace GameData.Domains.SpecialEffect.CombatSkill.Jieqingmen.Sword
{
	// Token: 0x020004E9 RID: 1257
	public class XuanMingJianQi : PoisonAddInjury
	{
		// Token: 0x17000241 RID: 577
		// (get) Token: 0x06003E13 RID: 15891 RVA: 0x00254B02 File Offset: 0x00252D02
		protected override bool AutoRemove
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06003E14 RID: 15892 RVA: 0x00254B05 File Offset: 0x00252D05
		public XuanMingJianQi()
		{
		}

		// Token: 0x06003E15 RID: 15893 RVA: 0x00254B0F File Offset: 0x00252D0F
		public XuanMingJianQi(CombatSkillKey skillKey) : base(skillKey, 13207)
		{
			this.RequirePoisonType = 2;
		}

		// Token: 0x06003E16 RID: 15894 RVA: 0x00254B26 File Offset: 0x00252D26
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			base.CreateAffectedAllEnemyData(283, EDataModifyType.AddPercent, -1);
			Events.RegisterHandler_NormalAttackAllEnd(new Events.OnNormalAttackAllEnd(this.OnNormalAttackAllEnd));
		}

		// Token: 0x06003E17 RID: 15895 RVA: 0x00254B51 File Offset: 0x00252D51
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_NormalAttackAllEnd(new Events.OnNormalAttackAllEnd(this.OnNormalAttackAllEnd));
			base.OnDisable(context);
		}

		// Token: 0x06003E18 RID: 15896 RVA: 0x00254B70 File Offset: 0x00252D70
		private void OnNormalAttackAllEnd(DataContext context, CombatCharacter attacker, CombatCharacter defender)
		{
			bool flag = !this._affected || defender != base.CombatChar;
			if (!flag)
			{
				base.ReduceEffectCount(1);
				this._affected = false;
				base.ShowSpecialEffectTips(1);
			}
		}

		// Token: 0x06003E19 RID: 15897 RVA: 0x00254BB2 File Offset: 0x00252DB2
		protected override void OnCastMaxPower(DataContext context)
		{
			base.AddMaxEffectCount(true);
		}

		// Token: 0x06003E1A RID: 15898 RVA: 0x00254BC0 File Offset: 0x00252DC0
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.FieldId != 283 || base.EffectCount <= 0 || !base.IsCurrent;
			int result;
			if (flag)
			{
				result = base.GetModifyValue(dataKey, currModifyValue);
			}
			else
			{
				CombatCharacter enemyChar;
				bool flag2 = !DomainManager.Combat.TryGetElement_CombatCharacterDict(dataKey.CharId, out enemyChar);
				if (flag2)
				{
					result = base.GetModifyValue(dataKey, currModifyValue);
				}
				else
				{
					bool flag3 = enemyChar.IsAlly == base.CombatChar.IsAlly;
					if (flag3)
					{
						result = base.GetModifyValue(dataKey, currModifyValue);
					}
					else
					{
						CombatCharacter poisonChar = base.IsDirect ? enemyChar : base.CombatChar;
						byte markCount = poisonChar.GetDefeatMarkCollection().PoisonMarkList[(int)this.RequirePoisonType];
						bool flag4 = markCount == 0;
						if (flag4)
						{
							result = base.GetModifyValue(dataKey, currModifyValue);
						}
						else
						{
							this._affected = true;
							result = XuanMingJianQi.AttackPrepareFrameAddPercent[(int)(markCount - 1)];
						}
					}
				}
			}
			return result;
		}

		// Token: 0x04001257 RID: 4695
		private static readonly int[] AttackPrepareFrameAddPercent = new int[]
		{
			100,
			150,
			200
		};

		// Token: 0x04001258 RID: 4696
		private bool _affected;
	}
}
