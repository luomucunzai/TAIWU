using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;

namespace GameData.Domains.SpecialEffect.Profession.Savage
{
	// Token: 0x02000113 RID: 275
	public class YanXueMing : ProfessionEffectBase
	{
		// Token: 0x1700011C RID: 284
		// (get) Token: 0x060029FE RID: 10750 RVA: 0x00201A2A File Offset: 0x001FFC2A
		protected override short CombatStateId
		{
			get
			{
				return 139;
			}
		}

		// Token: 0x060029FF RID: 10751 RVA: 0x00201A31 File Offset: 0x001FFC31
		public YanXueMing()
		{
		}

		// Token: 0x06002A00 RID: 10752 RVA: 0x00201A3B File Offset: 0x001FFC3B
		public YanXueMing(int charId) : base(charId)
		{
		}

		// Token: 0x06002A01 RID: 10753 RVA: 0x00201A46 File Offset: 0x001FFC46
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			Events.RegisterHandler_CombatBegin(new Events.OnCombatBegin(this.OnCombatBegin));
		}

		// Token: 0x06002A02 RID: 10754 RVA: 0x00201A63 File Offset: 0x001FFC63
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CombatBegin(new Events.OnCombatBegin(this.OnCombatBegin));
			base.OnDisable(context);
		}

		// Token: 0x06002A03 RID: 10755 RVA: 0x00201A80 File Offset: 0x001FFC80
		private void OnCombatBegin(DataContext context)
		{
			base.AppendAffectedAllEnemyData(context, 283, EDataModifyType.AddPercent, -1);
		}

		// Token: 0x06002A04 RID: 10756 RVA: 0x00201A94 File Offset: 0x001FFC94
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.CharId == base.CharacterId;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				bool flag2 = dataKey.FieldId == 283;
				if (flag2)
				{
					result = 20;
				}
				else
				{
					result = 0;
				}
			}
			return result;
		}

		// Token: 0x04000CE1 RID: 3297
		private const sbyte AddPercent = 20;
	}
}
