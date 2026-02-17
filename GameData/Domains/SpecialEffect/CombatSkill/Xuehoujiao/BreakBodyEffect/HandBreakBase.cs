using System;
using GameData.Combat.Math;
using GameData.Common;

namespace GameData.Domains.SpecialEffect.CombatSkill.Xuehoujiao.BreakBodyEffect
{
	// Token: 0x02000251 RID: 593
	public class HandBreakBase : BreakBodyEffectBase
	{
		// Token: 0x06003017 RID: 12311 RVA: 0x00215E6F File Offset: 0x0021406F
		protected HandBreakBase()
		{
		}

		// Token: 0x06003018 RID: 12312 RVA: 0x00215E79 File Offset: 0x00214079
		protected HandBreakBase(int charId, int type) : base(charId, type)
		{
			this.AffectBodyParts = new sbyte[]
			{
				3,
				4
			};
		}

		// Token: 0x06003019 RID: 12313 RVA: 0x00215E99 File Offset: 0x00214099
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			base.CreateAffectedData(170, EDataModifyType.Custom, -1);
		}

		// Token: 0x0600301A RID: 12314 RVA: 0x00215EB4 File Offset: 0x002140B4
		public override void OnDataAdded(DataContext context)
		{
			bool flag = DomainManager.Combat.IsCharInCombat(base.CharacterId, true) && base.CombatChar.GetTricks().Tricks.Count > base.CombatChar.GetMaxTrickCount();
			if (flag)
			{
				DomainManager.Combat.RemoveOverflowTrick(context, base.CombatChar, true);
			}
		}

		// Token: 0x0600301B RID: 12315 RVA: 0x00215F14 File Offset: 0x00214114
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
				bool flag2 = dataKey.FieldId == 170;
				if (flag2)
				{
					result = Math.Min(6, dataValue);
				}
				else
				{
					result = base.GetModifiedValue(dataKey, dataValue);
				}
			}
			return result;
		}

		// Token: 0x04000E49 RID: 3657
		private const sbyte MaxTrickCount = 6;
	}
}
