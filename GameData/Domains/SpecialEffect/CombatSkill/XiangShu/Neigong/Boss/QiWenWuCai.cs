using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Neigong.Boss
{
	// Token: 0x020002A1 RID: 673
	public class QiWenWuCai : BossNeigongBase
	{
		// Token: 0x060031AD RID: 12717 RVA: 0x0021BC24 File Offset: 0x00219E24
		public QiWenWuCai()
		{
		}

		// Token: 0x060031AE RID: 12718 RVA: 0x0021BC36 File Offset: 0x00219E36
		public QiWenWuCai(CombatSkillKey skillKey) : base(skillKey, 16103)
		{
		}

		// Token: 0x060031AF RID: 12719 RVA: 0x0021BC4E File Offset: 0x00219E4E
		public override void OnDisable(DataContext context)
		{
			base.OnDisable(context);
			Events.UnRegisterHandler_CombatStateMachineUpdateEnd(new Events.OnCombatStateMachineUpdateEnd(this.OnStateMachineUpdateEnd));
		}

		// Token: 0x060031B0 RID: 12720 RVA: 0x0021BC6B File Offset: 0x00219E6B
		protected override void ActivePhase2Effect(DataContext context)
		{
			base.AppendAffectedData(context, base.CharacterId, 114, EDataModifyType.Custom, -1);
			Events.RegisterHandler_CombatStateMachineUpdateEnd(new Events.OnCombatStateMachineUpdateEnd(this.OnStateMachineUpdateEnd));
		}

		// Token: 0x060031B1 RID: 12721 RVA: 0x0021BC94 File Offset: 0x00219E94
		private void OnStateMachineUpdateEnd(DataContext context, CombatCharacter combatChar)
		{
			bool flag = base.CombatChar != combatChar || DomainManager.Combat.Pause;
			if (!flag)
			{
				this._frameCounter++;
				bool flag2 = this._frameCounter < (int)this.AddNeiliAllocationFrame;
				if (!flag2)
				{
					this._frameCounter = 0;
					for (byte type = 0; type < 4; type += 1)
					{
						base.CombatChar.ChangeNeiliAllocation(context, type, 1, true, true);
					}
				}
			}
		}

		// Token: 0x060031B2 RID: 12722 RVA: 0x0021BD0C File Offset: 0x00219F0C
		public unsafe override long GetModifiedValue(AffectedDataKey dataKey, long dataValue)
		{
			EDamageType damageType = (EDamageType)dataKey.CustomParam0;
			bool flag = dataKey.CharId != base.CharacterId || damageType != EDamageType.Direct;
			long result;
			if (flag)
			{
				result = dataValue;
			}
			else
			{
				NeiliAllocation selfNeiliAllocation = base.CombatChar.GetNeiliAllocation();
				NeiliAllocation enemyNeiliAllocation = base.CurrEnemyChar.GetNeiliAllocation();
				for (byte type = 0; type < 4; type += 1)
				{
					bool flag2 = *(ref selfNeiliAllocation.Items.FixedElementField + (IntPtr)type * 2) < *(ref enemyNeiliAllocation.Items.FixedElementField + (IntPtr)type * 2) * 2;
					if (flag2)
					{
						return dataValue;
					}
				}
				result = 0L;
			}
			return result;
		}

		// Token: 0x04000EB9 RID: 3769
		private sbyte AddNeiliAllocationFrame = 60;

		// Token: 0x04000EBA RID: 3770
		private int _frameCounter;
	}
}
