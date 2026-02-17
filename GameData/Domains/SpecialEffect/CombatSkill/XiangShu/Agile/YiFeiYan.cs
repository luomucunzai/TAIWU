using System;
using System.Collections.Generic;
using Config;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Agile;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Agile
{
	// Token: 0x02000341 RID: 833
	public class YiFeiYan : AgileSkillBase
	{
		// Token: 0x060034D5 RID: 13525 RVA: 0x0022A1BC File Offset: 0x002283BC
		public YiFeiYan()
		{
		}

		// Token: 0x060034D6 RID: 13526 RVA: 0x0022A1C6 File Offset: 0x002283C6
		public YiFeiYan(CombatSkillKey skillKey) : base(skillKey, 16204)
		{
			this.ListenCanAffectChange = true;
		}

		// Token: 0x060034D7 RID: 13527 RVA: 0x0022A1E0 File Offset: 0x002283E0
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			Events.RegisterHandler_DistanceChanged(new Events.OnDistanceChanged(this.OnDistanceChanged));
			this._affecting = false;
			this.OnMoveSkillCanAffectChanged(context, default(DataUid));
		}

		// Token: 0x060034D8 RID: 13528 RVA: 0x0022A220 File Offset: 0x00228420
		public override void OnDisable(DataContext context)
		{
			base.OnDisable(context);
			Events.UnRegisterHandler_DistanceChanged(new Events.OnDistanceChanged(this.OnDistanceChanged));
			bool flag = this._affecting && base.CombatChar.GetJumpPreparedDistance() > 0;
			if (flag)
			{
				this.RemoveEnemyTricks(context, base.CombatChar.GetJumpPreparedDistance());
			}
			DomainManager.Combat.DisableJumpMove(context, base.CombatChar, base.SkillTemplateId);
		}

		// Token: 0x060034D9 RID: 13529 RVA: 0x0022A290 File Offset: 0x00228490
		private void OnDistanceChanged(DataContext context, CombatCharacter mover, short distance, bool isMove, bool isForced)
		{
			bool flag = mover != base.CombatChar || !isMove || isForced || !this._affecting;
			if (!flag)
			{
				this.RemoveEnemyTricks(context, distance);
			}
		}

		// Token: 0x060034DA RID: 13530 RVA: 0x0022A2D0 File Offset: 0x002284D0
		private void RemoveEnemyTricks(DataContext context, short distance)
		{
			CombatCharacter enemyChar = DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly, false);
			IReadOnlyDictionary<int, sbyte> trickDict = enemyChar.GetTricks().Tricks;
			int removeCount = Math.Min((int)(2 * Math.Abs(distance) / 10), trickDict.Count);
			bool flag = removeCount <= 0;
			if (!flag)
			{
				List<int> keyList = ObjectPool<List<int>>.Instance.Get();
				List<NeedTrick> removeTricks = ObjectPool<List<NeedTrick>>.Instance.Get();
				keyList.Clear();
				keyList.AddRange(trickDict.Keys);
				removeTricks.Clear();
				for (int i = 0; i < removeCount; i++)
				{
					int index = context.Random.Next(0, keyList.Count);
					removeTricks.Add(new NeedTrick(trickDict[keyList[index]], 1));
					keyList.RemoveAt(index);
				}
				DomainManager.Combat.RemoveTrick(context, enemyChar, removeTricks, false, false, -1);
				ObjectPool<List<int>>.Instance.Return(keyList);
				ObjectPool<List<NeedTrick>>.Instance.Return(removeTricks);
				base.ShowSpecialEffectTips(0);
			}
		}

		// Token: 0x060034DB RID: 13531 RVA: 0x0022A3E4 File Offset: 0x002285E4
		protected override void OnMoveSkillCanAffectChanged(DataContext context, DataUid dataUid)
		{
			bool canAffect = base.CanAffect;
			bool flag = this._affecting == canAffect;
			if (!flag)
			{
				this._affecting = canAffect;
				bool flag2 = canAffect;
				if (flag2)
				{
					DomainManager.Combat.EnableJumpMove(base.CombatChar, base.SkillTemplateId);
				}
				else
				{
					DomainManager.Combat.DisableJumpMove(context, base.CombatChar, base.SkillTemplateId);
				}
			}
		}

		// Token: 0x04000F8D RID: 3981
		private const sbyte TrickPerDistance = 2;

		// Token: 0x04000F8E RID: 3982
		private bool _affecting;
	}
}
