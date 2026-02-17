using System;
using GameData.Combat.Math;
using GameData.Common;

namespace GameData.Domains.SpecialEffect.CombatSkill.Xuehoujiao.BreakBodyEffect
{
	// Token: 0x02000249 RID: 585
	public class BellyBreakBase : BreakBodyEffectBase
	{
		// Token: 0x06002FFB RID: 12283 RVA: 0x0021574F File Offset: 0x0021394F
		protected BellyBreakBase()
		{
		}

		// Token: 0x06002FFC RID: 12284 RVA: 0x00215759 File Offset: 0x00213959
		protected BellyBreakBase(int charId, int type) : base(charId, type)
		{
			this.AffectBodyParts = new sbyte[]
			{
				1
			};
		}

		// Token: 0x06002FFD RID: 12285 RVA: 0x00215775 File Offset: 0x00213975
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 172, -1, -1, -1, -1), EDataModifyType.Custom);
		}

		// Token: 0x06002FFE RID: 12286 RVA: 0x002157A4 File Offset: 0x002139A4
		public override void OnDataAdded(DataContext context)
		{
			bool flag = DomainManager.Combat.IsCharInCombat(base.CharacterId, true) && base.CombatChar.GetStanceValue() > base.CombatChar.GetMaxStanceValue();
			if (flag)
			{
				DomainManager.Combat.ChangeStanceValue(context, base.CombatChar, 0, false, null);
			}
		}

		// Token: 0x06002FFF RID: 12287 RVA: 0x002157FC File Offset: 0x002139FC
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
				bool flag2 = dataKey.FieldId == 172;
				if (flag2)
				{
					result = Math.Min(60, dataValue);
				}
				else
				{
					result = base.GetModifiedValue(dataKey, dataValue);
				}
			}
			return result;
		}

		// Token: 0x04000E3D RID: 3645
		private const sbyte MaxStancePercent = 60;
	}
}
