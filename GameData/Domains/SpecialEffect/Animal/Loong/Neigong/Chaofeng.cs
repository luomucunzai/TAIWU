using System;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.Animal.Loong.Neigong
{
	// Token: 0x020005FF RID: 1535
	public class Chaofeng : AnimalEffectBase
	{
		// Token: 0x06004503 RID: 17667 RVA: 0x00271135 File Offset: 0x0026F335
		public Chaofeng()
		{
		}

		// Token: 0x06004504 RID: 17668 RVA: 0x0027113F File Offset: 0x0026F33F
		public Chaofeng(CombatSkillKey skillKey) : base(skillKey)
		{
		}

		// Token: 0x06004505 RID: 17669 RVA: 0x0027114A File Offset: 0x0026F34A
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			Events.RegisterHandler_DistanceChanged(new Events.OnDistanceChanged(this.OnDistanceChanged));
		}

		// Token: 0x06004506 RID: 17670 RVA: 0x00271167 File Offset: 0x0026F367
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_DistanceChanged(new Events.OnDistanceChanged(this.OnDistanceChanged));
			base.OnDisable(context);
		}

		// Token: 0x06004507 RID: 17671 RVA: 0x00271184 File Offset: 0x0026F384
		private void OnDistanceChanged(DataContext context, CombatCharacter mover, short distance, bool isMove, bool isForced)
		{
			bool flag = mover.IsAlly == base.CombatChar.IsAlly || isForced || !isMove || !base.IsCurrent;
			if (!flag)
			{
				bool flag2 = distance < 0;
				if (flag2)
				{
					this._forwardMovedDistance += (int)(-(int)distance);
				}
				else
				{
					bool flag3 = distance > 0;
					if (flag3)
					{
						this._backwardMovedDistance += (int)distance;
					}
				}
				bool flag4 = this._forwardMovedDistance > 5;
				if (flag4)
				{
					this._forwardMovedDistance = this.DoAddInjury(context, mover, this._forwardMovedDistance, false);
				}
				bool flag5 = this._backwardMovedDistance > 5;
				if (flag5)
				{
					this._backwardMovedDistance = this.DoAddInjury(context, mover, this._backwardMovedDistance, true);
				}
			}
		}

		// Token: 0x06004508 RID: 17672 RVA: 0x00271238 File Offset: 0x0026F438
		private int DoAddInjury(DataContext context, CombatCharacter combatChar, int movedDistance, bool isInner)
		{
			int count = movedDistance / 5;
			DomainManager.Combat.AddRandomInjury(context, combatChar, isInner, count, 1, false, -1);
			base.ShowSpecialEffectTips(isInner, 1, 0);
			return movedDistance % 5;
		}

		// Token: 0x04001465 RID: 5221
		private const int AddInjuryRequireDistance = 5;

		// Token: 0x04001466 RID: 5222
		private int _forwardMovedDistance;

		// Token: 0x04001467 RID: 5223
		private int _backwardMovedDistance;
	}
}
