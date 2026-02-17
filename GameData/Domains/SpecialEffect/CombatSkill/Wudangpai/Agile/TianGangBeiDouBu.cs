using System;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Agile;

namespace GameData.Domains.SpecialEffect.CombatSkill.Wudangpai.Agile
{
	// Token: 0x020003E3 RID: 995
	public class TianGangBeiDouBu : AgileSkillBase
	{
		// Token: 0x0600380C RID: 14348 RVA: 0x002388F2 File Offset: 0x00236AF2
		public TianGangBeiDouBu()
		{
		}

		// Token: 0x0600380D RID: 14349 RVA: 0x002388FC File Offset: 0x00236AFC
		public TianGangBeiDouBu(CombatSkillKey skillKey) : base(skillKey, 4403)
		{
		}

		// Token: 0x0600380E RID: 14350 RVA: 0x0023890C File Offset: 0x00236B0C
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			this._distanceAccumulator = 0;
			this._addedNeiliAllocation = 0;
			Events.RegisterHandler_DistanceChanged(new Events.OnDistanceChanged(this.OnDistanceChanged));
			Events.RegisterHandler_NeiliAllocationChanged(new Events.OnNeiliAllocationChanged(this.OnNeiliAllocationChanged));
		}

		// Token: 0x0600380F RID: 14351 RVA: 0x0023894C File Offset: 0x00236B4C
		public override void OnDisable(DataContext context)
		{
			base.OnDisable(context);
			bool flag = DomainManager.Combat.IsInCombat();
			if (flag)
			{
				base.CombatChar.ChangeNeiliAllocation(context, 1, -this._addedNeiliAllocation, true, true);
			}
			Events.UnRegisterHandler_DistanceChanged(new Events.OnDistanceChanged(this.OnDistanceChanged));
			Events.UnRegisterHandler_NeiliAllocationChanged(new Events.OnNeiliAllocationChanged(this.OnNeiliAllocationChanged));
		}

		// Token: 0x06003810 RID: 14352 RVA: 0x002389AC File Offset: 0x00236BAC
		private unsafe void OnDistanceChanged(DataContext context, CombatCharacter mover, short distance, bool isMove, bool isForced)
		{
			bool flag = mover != base.CombatChar || !isMove || isForced;
			if (!flag)
			{
				this._distanceAccumulator += (int)Math.Abs(distance);
				while (this._distanceAccumulator >= 10)
				{
					this._distanceAccumulator -= 10;
					bool flag2 = !base.CanAffect;
					if (!flag2)
					{
						byte srcType = base.IsDirect ? 0 : 2;
						NeiliAllocation neiliAllocation = base.CombatChar.GetNeiliAllocation();
						int addValue = (int)(*(ref neiliAllocation.Items.FixedElementField + (IntPtr)srcType * 2) * 10 / 100);
						bool flag3 = addValue == 0;
						if (!flag3)
						{
							int oldValue = (int)(*(ref neiliAllocation.Items.FixedElementField + 2));
							base.CombatChar.ChangeNeiliAllocation(context, 1, addValue, true, true);
							neiliAllocation = base.CombatChar.GetNeiliAllocation();
							this._addedNeiliAllocation += Math.Max((int)(*(ref neiliAllocation.Items.FixedElementField + 2)) - oldValue, 0);
							base.ShowSpecialEffectTips(0);
						}
					}
				}
			}
		}

		// Token: 0x06003811 RID: 14353 RVA: 0x00238AC4 File Offset: 0x00236CC4
		private void OnNeiliAllocationChanged(DataContext context, int charId, byte type, int changeValue)
		{
			bool flag = charId != base.CharacterId || type != 1 || changeValue >= 0 || this._addedNeiliAllocation <= 0;
			if (!flag)
			{
				this._addedNeiliAllocation = Math.Max(this._addedNeiliAllocation + changeValue, 0);
			}
		}

		// Token: 0x04001064 RID: 4196
		private const sbyte RequireMoveDistance = 10;

		// Token: 0x04001065 RID: 4197
		private const sbyte AddNeiliAllocationPercent = 10;

		// Token: 0x04001066 RID: 4198
		private int _distanceAccumulator;

		// Token: 0x04001067 RID: 4199
		private int _addedNeiliAllocation;
	}
}
