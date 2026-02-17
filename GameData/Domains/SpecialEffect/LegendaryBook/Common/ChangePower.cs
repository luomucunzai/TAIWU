using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill;

namespace GameData.Domains.SpecialEffect.LegendaryBook.Common
{
	// Token: 0x02000171 RID: 369
	public class ChangePower : CombatSkillEffectBase
	{
		// Token: 0x06002B42 RID: 11074 RVA: 0x00204E4D File Offset: 0x0020304D
		protected ChangePower()
		{
			this.IsLegendaryBookEffect = true;
		}

		// Token: 0x06002B43 RID: 11075 RVA: 0x00204E5E File Offset: 0x0020305E
		protected ChangePower(CombatSkillKey skillKey, int type) : base(skillKey, type, -1)
		{
			this.IsLegendaryBookEffect = true;
		}

		// Token: 0x06002B44 RID: 11076 RVA: 0x00204E72 File Offset: 0x00203072
		public override void OnEnable(DataContext context)
		{
			Events.RegisterHandler_CombatBegin(new Events.OnCombatBegin(this.OnCombatBegin));
			Events.RegisterHandler_CombatSettlement(new Events.OnCombatSettlement(this.OnCombatSettlement));
		}

		// Token: 0x06002B45 RID: 11077 RVA: 0x00204E99 File Offset: 0x00203099
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CombatBegin(new Events.OnCombatBegin(this.OnCombatBegin));
			Events.UnRegisterHandler_CombatSettlement(new Events.OnCombatSettlement(this.OnCombatSettlement));
		}

		// Token: 0x06002B46 RID: 11078 RVA: 0x00204EC0 File Offset: 0x002030C0
		private void OnCombatBegin(DataContext context)
		{
			bool flag = !DomainManager.Combat.IsCharInCombat(base.CharacterId, true);
			if (!flag)
			{
				base.AppendAffectedData(context, base.CharacterId, 199, EDataModifyType.AddPercent, -1);
				base.AppendAffectedAllEnemyData(context, 199, EDataModifyType.AddPercent, -1);
			}
		}

		// Token: 0x06002B47 RID: 11079 RVA: 0x00204F0C File Offset: 0x0020310C
		private void OnCombatSettlement(DataContext context, sbyte combatStatus)
		{
			base.ClearAffectedData(context);
		}

		// Token: 0x06002B48 RID: 11080 RVA: 0x00204F18 File Offset: 0x00203118
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.CombatSkillId != base.SkillTemplateId;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				bool flag2 = dataKey.FieldId == 199;
				if (flag2)
				{
					result = ((dataKey.CharId == base.CharacterId) ? 20 : -20);
				}
				else
				{
					result = 0;
				}
			}
			return result;
		}

		// Token: 0x04000D3B RID: 3387
		private const sbyte AddSelfPower = 20;

		// Token: 0x04000D3C RID: 3388
		private const sbyte ReduceEnemyPower = -20;
	}
}
