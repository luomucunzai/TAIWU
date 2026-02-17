using System;
using GameData.Combat.Math;
using GameData.Common;

namespace GameData.Domains.SpecialEffect.CombatSkill.Xuehoujiao.BreakBodyEffect
{
	// Token: 0x0200024E RID: 590
	public class ChestBreakBase : BreakBodyEffectBase
	{
		// Token: 0x0600300E RID: 12302 RVA: 0x00215D1F File Offset: 0x00213F1F
		protected ChestBreakBase()
		{
		}

		// Token: 0x0600300F RID: 12303 RVA: 0x00215D29 File Offset: 0x00213F29
		protected ChestBreakBase(int charId, int type) : base(charId, type)
		{
			this.AffectBodyParts = new sbyte[1];
		}

		// Token: 0x06003010 RID: 12304 RVA: 0x00215D41 File Offset: 0x00213F41
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 171, -1, -1, -1, -1), EDataModifyType.Custom);
		}

		// Token: 0x06003011 RID: 12305 RVA: 0x00215D70 File Offset: 0x00213F70
		public override void OnDataAdded(DataContext context)
		{
			bool flag = DomainManager.Combat.IsCharInCombat(base.CharacterId, true) && base.CombatChar.GetBreathValue() > base.CombatChar.GetMaxBreathValue();
			if (flag)
			{
				DomainManager.Combat.ChangeBreathValue(context, base.CombatChar, 0, false, null);
			}
		}

		// Token: 0x06003012 RID: 12306 RVA: 0x00215DC8 File Offset: 0x00213FC8
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
				bool flag2 = dataKey.FieldId == 171;
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

		// Token: 0x04000E48 RID: 3656
		private const sbyte MaxBreathPercent = 60;
	}
}
