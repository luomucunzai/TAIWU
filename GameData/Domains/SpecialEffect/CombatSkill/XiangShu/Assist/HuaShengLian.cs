using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Assist
{
	// Token: 0x02000324 RID: 804
	public class HuaShengLian : RanChenZiAssistSkillBase
	{
		// Token: 0x06003442 RID: 13378 RVA: 0x002282AC File Offset: 0x002264AC
		public HuaShengLian()
		{
		}

		// Token: 0x06003443 RID: 13379 RVA: 0x002282B6 File Offset: 0x002264B6
		public HuaShengLian(CombatSkillKey skillKey) : base(skillKey, 16411)
		{
			this.RequireBossPhase = 1;
		}

		// Token: 0x06003444 RID: 13380 RVA: 0x002282CD File Offset: 0x002264CD
		protected override void ActivateEffect(DataContext context)
		{
			base.AppendAffectedData(context, 159, EDataModifyType.Custom, -1);
			base.AppendAffectedData(context, 114, EDataModifyType.Custom, -1);
		}

		// Token: 0x06003445 RID: 13381 RVA: 0x002282EB File Offset: 0x002264EB
		protected override void DeactivateEffect(DataContext context)
		{
			base.ClearAffectedData(context);
		}

		// Token: 0x06003446 RID: 13382 RVA: 0x002282F8 File Offset: 0x002264F8
		public override bool GetModifiedValue(AffectedDataKey dataKey, bool dataValue)
		{
			bool flag = dataKey.CharId != base.CharacterId;
			bool result;
			if (flag)
			{
				result = base.GetModifiedValue(dataKey, dataValue);
			}
			else
			{
				bool flag2 = dataKey.FieldId == 159;
				result = (!flag2 && base.GetModifiedValue(dataKey, dataValue));
			}
			return result;
		}

		// Token: 0x06003447 RID: 13383 RVA: 0x00228348 File Offset: 0x00226548
		public override long GetModifiedValue(AffectedDataKey dataKey, long dataValue)
		{
			bool flag = dataKey.CharId != base.CharacterId;
			long result;
			if (flag)
			{
				result = dataValue;
			}
			else
			{
				bool flag2 = dataKey.FieldId == 114;
				if (flag2)
				{
					bool inner = dataKey.CustomParam1 == 1;
					sbyte bodyPart = (sbyte)dataKey.CustomParam2;
					result = Math.Min(dataValue, (long)base.CombatChar.MarkCountChangeToDamageValue(bodyPart, inner, 3));
				}
				else
				{
					result = dataValue;
				}
			}
			return result;
		}

		// Token: 0x04000F6D RID: 3949
		private const sbyte MaxInjuryMark = 3;
	}
}
