using System;
using System.Collections.Generic;
using System.Linq;
using Config;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Agile;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.Emeipai.Agile
{
	// Token: 0x0200056E RID: 1390
	public class YuNvNuoLian : AgileSkillBase
	{
		// Token: 0x06004101 RID: 16641 RVA: 0x00260F76 File Offset: 0x0025F176
		public YuNvNuoLian()
		{
		}

		// Token: 0x06004102 RID: 16642 RVA: 0x00260F80 File Offset: 0x0025F180
		public YuNvNuoLian(CombatSkillKey skillKey) : base(skillKey, 2503)
		{
		}

		// Token: 0x06004103 RID: 16643 RVA: 0x00260F90 File Offset: 0x0025F190
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			Events.RegisterHandler_DistanceChanged(new Events.OnDistanceChanged(this.OnDistanceChanged));
		}

		// Token: 0x06004104 RID: 16644 RVA: 0x00260FAD File Offset: 0x0025F1AD
		public override void OnDisable(DataContext context)
		{
			base.OnDisable(context);
			Events.UnRegisterHandler_DistanceChanged(new Events.OnDistanceChanged(this.OnDistanceChanged));
		}

		// Token: 0x06004105 RID: 16645 RVA: 0x00260FCC File Offset: 0x0025F1CC
		private void OnDistanceChanged(DataContext context, CombatCharacter mover, short distance, bool isMove, bool isForced)
		{
			bool flag = mover != base.CombatChar || !isMove || isForced || !(base.IsDirect ? (distance < 0) : (distance > 0));
			if (!flag)
			{
				this._distanceAccumulator += (int)Math.Abs(distance);
				while (this._distanceAccumulator >= 10)
				{
					this._distanceAccumulator -= 10;
					bool flag2 = !base.CanAffect;
					if (!flag2)
					{
						CombatCharacter selfChar = base.CombatChar;
						CombatCharacter enemyChar = DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly, false);
						TrickCollection selfTricks = base.CombatChar.GetTricks();
						TrickCollection enemyTricks = enemyChar.GetTricks();
						List<sbyte> selfTrickRandomPool = ObjectPool<List<sbyte>>.Instance.Get();
						List<sbyte> enemyTrickRandomPool = ObjectPool<List<sbyte>>.Instance.Get();
						selfTrickRandomPool.Clear();
						enemyTrickRandomPool.Clear();
						selfTrickRandomPool.AddRange(selfTricks.Tricks.Where(delegate(KeyValuePair<int, sbyte> trick)
						{
							CombatCharacter combatChar = base.CombatChar;
							KeyValuePair<int, sbyte> keyValuePair = trick;
							return combatChar.IsTrickUseless(keyValuePair.Value);
						}).Select(delegate(KeyValuePair<int, sbyte> trick)
						{
							KeyValuePair<int, sbyte> keyValuePair = trick;
							return keyValuePair.Value;
						}));
						enemyTrickRandomPool.AddRange(enemyTricks.Tricks.Where(delegate(KeyValuePair<int, sbyte> trick)
						{
							CombatCharacter combatChar = base.CombatChar;
							KeyValuePair<int, sbyte> keyValuePair = trick;
							return combatChar.IsTrickUsable(keyValuePair.Value);
						}).Select(delegate(KeyValuePair<int, sbyte> trick)
						{
							KeyValuePair<int, sbyte> keyValuePair = trick;
							return keyValuePair.Value;
						}));
						int exchangeCount = Math.Min(Math.Min(selfTrickRandomPool.Count, enemyTrickRandomPool.Count), 3);
						bool flag3 = exchangeCount > 0;
						if (flag3)
						{
							CollectionUtils.Shuffle<sbyte>(context.Random, selfTrickRandomPool);
							CollectionUtils.Shuffle<sbyte>(context.Random, enemyTrickRandomPool);
							List<NeedTrick> selfRemoveTricks = ObjectPool<List<NeedTrick>>.Instance.Get();
							List<NeedTrick> selfAddTricks = ObjectPool<List<NeedTrick>>.Instance.Get();
							List<NeedTrick> enemyRemoveTricks = ObjectPool<List<NeedTrick>>.Instance.Get();
							List<NeedTrick> enemyAddTricks = ObjectPool<List<NeedTrick>>.Instance.Get();
							for (int i = 0; i < exchangeCount; i++)
							{
								sbyte selfTrickType = selfTrickRandomPool[i];
								sbyte enemyTrickType = enemyTrickRandomPool[i];
								selfRemoveTricks.Add(new NeedTrick(selfTrickType, 1));
								selfAddTricks.Add(new NeedTrick(enemyTrickType, 1));
								enemyRemoveTricks.Add(new NeedTrick(enemyTrickType, 1));
								enemyAddTricks.Add(new NeedTrick(selfTrickType, 1));
							}
							DomainManager.Combat.RemoveTrick(context, selfChar, selfRemoveTricks, true, false, -1);
							DomainManager.Combat.RemoveTrick(context, enemyChar, enemyRemoveTricks, false, false, -1);
							DomainManager.Combat.AddTrick(context, selfChar, selfAddTricks, true, false);
							DomainManager.Combat.AddTrick(context, enemyChar, enemyAddTricks, false, false);
							ObjectPool<List<NeedTrick>>.Instance.Return(selfRemoveTricks);
							ObjectPool<List<NeedTrick>>.Instance.Return(selfAddTricks);
							ObjectPool<List<NeedTrick>>.Instance.Return(enemyRemoveTricks);
							ObjectPool<List<NeedTrick>>.Instance.Return(enemyAddTricks);
							base.ShowSpecialEffectTips(0);
						}
						ObjectPool<List<sbyte>>.Instance.Return(selfTrickRandomPool);
						ObjectPool<List<sbyte>>.Instance.Return(enemyTrickRandomPool);
					}
				}
			}
		}

		// Token: 0x0400131D RID: 4893
		private const sbyte NeedMoveDistance = 10;

		// Token: 0x0400131E RID: 4894
		private const sbyte ExchangeTrickCount = 3;

		// Token: 0x0400131F RID: 4895
		private int _distanceAccumulator;
	}
}
