using System;
using System.Collections.Generic;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Agile;

namespace GameData.Domains.SpecialEffect.CombatSkill.Baihuagu.Agile
{
	// Token: 0x020005E5 RID: 1509
	public class ZhiLanYuBu : AgileSkillBase
	{
		// Token: 0x06004476 RID: 17526 RVA: 0x0026F99B File Offset: 0x0026DB9B
		public ZhiLanYuBu()
		{
		}

		// Token: 0x06004477 RID: 17527 RVA: 0x0026F9A5 File Offset: 0x0026DBA5
		public ZhiLanYuBu(CombatSkillKey skillKey) : base(skillKey, 3406)
		{
			this.ListenCanAffectChange = true;
		}

		// Token: 0x06004478 RID: 17528 RVA: 0x0026F9BC File Offset: 0x0026DBBC
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			Events.RegisterHandler_DistanceChanged(new Events.OnDistanceChanged(this.OnDistanceChanged));
			this._affecting = false;
			this.OnMoveSkillCanAffectChanged(context, default(DataUid));
		}

		// Token: 0x06004479 RID: 17529 RVA: 0x0026F9FC File Offset: 0x0026DBFC
		public override void OnDisable(DataContext context)
		{
			base.OnDisable(context);
			Events.UnRegisterHandler_DistanceChanged(new Events.OnDistanceChanged(this.OnDistanceChanged));
			DomainManager.Combat.DisableJumpMove(context, base.CombatChar, base.SkillTemplateId);
		}

		// Token: 0x0600447A RID: 17530 RVA: 0x0026FA34 File Offset: 0x0026DC34
		private void OnDistanceChanged(DataContext context, CombatCharacter mover, short distance, bool isMove, bool isForced)
		{
			bool flag = mover != base.CombatChar || !isMove || isForced || !(base.IsDirect ? (distance < 0) : (distance > 0)) || !this._affecting || DomainManager.Combat.IsMovedByTeammate(base.CombatChar);
			if (!flag)
			{
				Injuries newInjuries = base.CombatChar.GetInjuries().Subtract(base.CombatChar.GetOldInjuries());
				if (this._injuryRandomPool == null)
				{
					this._injuryRandomPool = new List<ValueTuple<sbyte, bool, sbyte>>();
				}
				this._injuryRandomPool.Clear();
				for (sbyte part = 0; part < 7; part += 1)
				{
					ValueTuple<sbyte, sbyte> injury = newInjuries.Get(part);
					bool flag2 = injury.Item1 > 0;
					if (flag2)
					{
						this._injuryRandomPool.Add(new ValueTuple<sbyte, bool, sbyte>(part, false, injury.Item1));
					}
					bool flag3 = injury.Item2 > 0;
					if (flag3)
					{
						this._injuryRandomPool.Add(new ValueTuple<sbyte, bool, sbyte>(part, true, injury.Item2));
					}
				}
				bool flag4 = this._injuryRandomPool.Count > 0;
				if (flag4)
				{
					ValueTuple<sbyte, bool, sbyte> injuryInfo = this._injuryRandomPool[context.Random.Next(0, this._injuryRandomPool.Count)];
					DomainManager.Combat.RemoveInjury(context, base.CombatChar, injuryInfo.Item1, injuryInfo.Item2, Math.Min(2, injuryInfo.Item3), true, false);
					base.ShowSpecialEffectTips(0);
				}
			}
		}

		// Token: 0x0600447B RID: 17531 RVA: 0x0026FBB4 File Offset: 0x0026DDB4
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
					DomainManager.Combat.EnableJumpMove(base.CombatChar, base.SkillTemplateId, base.IsDirect);
				}
				else
				{
					DomainManager.Combat.DisableJumpMove(context, base.CombatChar, base.SkillTemplateId);
				}
			}
		}

		// Token: 0x04001448 RID: 5192
		private const sbyte HealInjuryCount = 2;

		// Token: 0x04001449 RID: 5193
		private bool _affecting;

		// Token: 0x0400144A RID: 5194
		private List<ValueTuple<sbyte, bool, sbyte>> _injuryRandomPool;
	}
}
