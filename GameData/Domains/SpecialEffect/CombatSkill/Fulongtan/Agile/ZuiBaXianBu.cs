using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Agile;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.Fulongtan.Agile
{
	// Token: 0x02000536 RID: 1334
	public class ZuiBaXianBu : AgileSkillBase
	{
		// Token: 0x17000256 RID: 598
		// (get) Token: 0x06003FAA RID: 16298 RVA: 0x0025ADB1 File Offset: 0x00258FB1
		private short CurrDistance
		{
			get
			{
				return DomainManager.Combat.GetCurrentDistance();
			}
		}

		// Token: 0x06003FAB RID: 16299 RVA: 0x0025ADBD File Offset: 0x00258FBD
		public ZuiBaXianBu()
		{
		}

		// Token: 0x06003FAC RID: 16300 RVA: 0x0025ADC7 File Offset: 0x00258FC7
		public ZuiBaXianBu(CombatSkillKey skillKey) : base(skillKey, 14401)
		{
		}

		// Token: 0x06003FAD RID: 16301 RVA: 0x0025ADD7 File Offset: 0x00258FD7
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			Events.RegisterHandler_MoveBegin(new Events.OnMoveBegin(this.OnMoveBegin));
			Events.RegisterHandler_MoveEnd(new Events.OnMoveEnd(this.OnMoveEnd));
		}

		// Token: 0x06003FAE RID: 16302 RVA: 0x0025AE06 File Offset: 0x00259006
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_MoveBegin(new Events.OnMoveBegin(this.OnMoveBegin));
			Events.UnRegisterHandler_MoveEnd(new Events.OnMoveEnd(this.OnMoveEnd));
			base.OnDisable(context);
		}

		// Token: 0x06003FAF RID: 16303 RVA: 0x0025AE38 File Offset: 0x00259038
		private void OnMoveBegin(DataContext context, CombatCharacter mover, int distance, bool isJump)
		{
			bool flag = !this.IsMatchCharacter(mover);
			if (!flag)
			{
				this._prevDistance = (int)((base.CanAffect && this.IsMatchAttackRange()) ? this.CurrDistance : -1);
			}
		}

		// Token: 0x06003FB0 RID: 16304 RVA: 0x0025AE78 File Offset: 0x00259078
		private void OnMoveEnd(DataContext context, CombatCharacter mover, int distance, bool isJump)
		{
			bool flag = !this.IsMatch(mover) || this._prevDistance < 0;
			if (!flag)
			{
				this._movedDistance += Math.Abs((int)this.CurrDistance - this._prevDistance);
				bool flag2 = this._movedDistance < 5;
				if (!flag2)
				{
					int affectCount = this._movedDistance / 5;
					this._movedDistance %= 5;
					for (int i = 0; i < affectCount; i++)
					{
						this.DoAffect(context);
					}
				}
			}
		}

		// Token: 0x06003FB1 RID: 16305 RVA: 0x0025AF00 File Offset: 0x00259100
		private bool IsMatch(CombatCharacter mover)
		{
			return this.IsMatchCharacter(mover) && this.IsMatchAttackRange();
		}

		// Token: 0x06003FB2 RID: 16306 RVA: 0x0025AF24 File Offset: 0x00259124
		private bool IsMatchCharacter(CombatCharacter mover)
		{
			return base.IsDirect ? (mover.GetId() == base.CharacterId) : (mover.IsAlly != base.CombatChar.IsAlly);
		}

		// Token: 0x06003FB3 RID: 16307 RVA: 0x0025AF64 File Offset: 0x00259164
		private bool IsMatchAttackRange()
		{
			CombatCharacter rangeCharacter = base.IsDirect ? DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly, false) : base.CombatChar;
			return DomainManager.Combat.InAttackRange(rangeCharacter);
		}

		// Token: 0x06003FB4 RID: 16308 RVA: 0x0025AFAC File Offset: 0x002591AC
		private unsafe void DoAffect(DataContext context)
		{
			List<int> affectOdds = ObjectPool<List<int>>.Instance.Get();
			affectOdds.Clear();
			for (int i = 0; i < ZuiBaXianBu.Type2Affects.Count; i++)
			{
				affectOdds.Add(20);
			}
			bool flag = (*this.CharObj.GetEatingItems()).ContainsWine();
			if (flag)
			{
				affectOdds[RandomUtils.GetRandomIndex(affectOdds, context.Random)] = 100;
			}
			CombatCharacter affectChar = base.IsDirect ? base.CombatChar : DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly, false);
			int[] affectParam = base.IsDirect ? ZuiBaXianBu.DirectParam : ZuiBaXianBu.ReverseParam;
			foreach (KeyValuePair<ZuiBaXianBu.EType, ZuiBaXianBu.TypeDoAffectHandler> keyValuePair in ZuiBaXianBu.Type2Affects)
			{
				ZuiBaXianBu.EType etype;
				ZuiBaXianBu.TypeDoAffectHandler typeDoAffectHandler;
				keyValuePair.Deconstruct(out etype, out typeDoAffectHandler);
				ZuiBaXianBu.EType type = etype;
				ZuiBaXianBu.TypeDoAffectHandler handler = typeDoAffectHandler;
				bool flag2 = !context.Random.CheckPercentProb(affectOdds[(int)type]);
				if (!flag2)
				{
					handler(context, affectChar, affectParam[(int)type]);
					base.ShowSpecialEffectTips((byte)type);
				}
			}
			ObjectPool<List<int>>.Instance.Return(affectOdds);
		}

		// Token: 0x06003FB5 RID: 16309 RVA: 0x0025B0FC File Offset: 0x002592FC
		private static void ChangeMobility(DataContext context, CombatCharacter affectChar, int param)
		{
			CValuePercent percent = param;
			DomainManager.Combat.ChangeMobilityValue(context, affectChar, MoveSpecialConstants.MaxMobility * percent, false, null, false);
			DomainManager.Combat.UpdateSkillCanUse(context, affectChar);
		}

		// Token: 0x06003FB6 RID: 16310 RVA: 0x0025B13C File Offset: 0x0025933C
		private static void ChangeBreath(DataContext context, CombatCharacter affectChar, int param)
		{
			CValuePercent percent = param;
			DomainManager.Combat.ChangeBreathValue(context, affectChar, 30000 * percent, false, null);
			DomainManager.Combat.UpdateSkillCanUse(context, affectChar);
		}

		// Token: 0x06003FB7 RID: 16311 RVA: 0x0025B178 File Offset: 0x00259378
		private static void ChangeStance(DataContext context, CombatCharacter affectChar, int param)
		{
			CValuePercent percent = param;
			DomainManager.Combat.ChangeStanceValue(context, affectChar, 4000 * percent, false, null);
			DomainManager.Combat.UpdateSkillCanUse(context, affectChar);
		}

		// Token: 0x040012BF RID: 4799
		private static readonly Dictionary<ZuiBaXianBu.EType, ZuiBaXianBu.TypeDoAffectHandler> Type2Affects = new Dictionary<ZuiBaXianBu.EType, ZuiBaXianBu.TypeDoAffectHandler>
		{
			{
				ZuiBaXianBu.EType.Mobility,
				new ZuiBaXianBu.TypeDoAffectHandler(ZuiBaXianBu.ChangeMobility)
			},
			{
				ZuiBaXianBu.EType.Breath,
				new ZuiBaXianBu.TypeDoAffectHandler(ZuiBaXianBu.ChangeBreath)
			},
			{
				ZuiBaXianBu.EType.Stance,
				new ZuiBaXianBu.TypeDoAffectHandler(ZuiBaXianBu.ChangeStance)
			}
		};

		// Token: 0x040012C0 RID: 4800
		private const int AffectRequireDistance = 5;

		// Token: 0x040012C1 RID: 4801
		private const int JudgeOdds = 20;

		// Token: 0x040012C2 RID: 4802
		private const int WineOdds = 100;

		// Token: 0x040012C3 RID: 4803
		private static readonly int[] DirectParam = new int[]
		{
			20,
			20,
			20,
			1
		};

		// Token: 0x040012C4 RID: 4804
		private static readonly int[] ReverseParam = new int[]
		{
			-20,
			-20,
			-20,
			-1
		};

		// Token: 0x040012C5 RID: 4805
		private int _prevDistance;

		// Token: 0x040012C6 RID: 4806
		private int _movedDistance;

		// Token: 0x02000A53 RID: 2643
		private enum EType
		{
			// Token: 0x04002A57 RID: 10839
			Mobility,
			// Token: 0x04002A58 RID: 10840
			Breath,
			// Token: 0x04002A59 RID: 10841
			Stance
		}

		// Token: 0x02000A54 RID: 2644
		// (Invoke) Token: 0x06008785 RID: 34693
		private delegate void TypeDoAffectHandler(DataContext context, CombatCharacter affectChar, int param);
	}
}
