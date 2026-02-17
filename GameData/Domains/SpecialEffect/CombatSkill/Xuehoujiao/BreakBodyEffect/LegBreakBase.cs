using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.Combat;

namespace GameData.Domains.SpecialEffect.CombatSkill.Xuehoujiao.BreakBodyEffect
{
	// Token: 0x02000257 RID: 599
	public class LegBreakBase : BreakBodyEffectBase
	{
		// Token: 0x0600302D RID: 12333 RVA: 0x00216475 File Offset: 0x00214675
		protected LegBreakBase()
		{
		}

		// Token: 0x0600302E RID: 12334 RVA: 0x0021647F File Offset: 0x0021467F
		protected LegBreakBase(int charId, int type) : base(charId, type)
		{
			this.AffectBodyParts = new sbyte[]
			{
				5,
				6
			};
		}

		// Token: 0x0600302F RID: 12335 RVA: 0x0021649F File Offset: 0x0021469F
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 274, -1, -1, -1, -1), EDataModifyType.Custom);
		}

		// Token: 0x06003030 RID: 12336 RVA: 0x002164CC File Offset: 0x002146CC
		public override void OnDataAdded(DataContext context)
		{
			bool flag = DomainManager.Combat.IsCharInCombat(base.CharacterId, true) && base.CombatChar.GetMobilityValue() > MoveSpecialConstants.MaxMobility * 50 / 100;
			if (flag)
			{
				DomainManager.Combat.ChangeMobilityValue(context, base.CombatChar, 0, false, null, false);
			}
		}

		// Token: 0x06003031 RID: 12337 RVA: 0x00216524 File Offset: 0x00214724
		public override int GetModifiedValue(AffectedDataKey dataKey, int dataValue)
		{
			bool flag = dataKey.CharId != base.CharacterId;
			int result;
			if (flag)
			{
				result = dataValue;
			}
			else
			{
				bool flag2 = dataKey.FieldId == 274;
				if (flag2)
				{
					result = Math.Min(50, dataValue);
				}
				else
				{
					result = base.GetModifiedValue(dataKey, dataValue);
				}
			}
			return result;
		}

		// Token: 0x04000E50 RID: 3664
		private const sbyte MaxMobilityPercent = 50;
	}
}
