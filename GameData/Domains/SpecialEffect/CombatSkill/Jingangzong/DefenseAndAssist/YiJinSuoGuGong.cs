using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Defense;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.Jingangzong.DefenseAndAssist
{
	// Token: 0x020004CC RID: 1228
	public class YiJinSuoGuGong : DefenseSkillBase
	{
		// Token: 0x06003D58 RID: 15704 RVA: 0x002515BA File Offset: 0x0024F7BA
		public YiJinSuoGuGong()
		{
		}

		// Token: 0x06003D59 RID: 15705 RVA: 0x002515C4 File Offset: 0x0024F7C4
		public YiJinSuoGuGong(CombatSkillKey skillKey) : base(skillKey, 11602)
		{
		}

		// Token: 0x06003D5A RID: 15706 RVA: 0x002515D4 File Offset: 0x0024F7D4
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 126, -1, -1, -1, -1), EDataModifyType.Custom);
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 131, -1, -1, -1, -1), EDataModifyType.Custom);
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 234, -1, -1, -1, -1), EDataModifyType.Custom);
			for (sbyte hitType = 0; hitType < 4; hitType += 1)
			{
				this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, (ushort)(94 + hitType), -1, -1, -1, -1), EDataModifyType.TotalPercent);
			}
			base.ShowSpecialEffectTips(0);
		}

		// Token: 0x06003D5B RID: 15707 RVA: 0x00251690 File Offset: 0x0024F890
		public override bool GetModifiedValue(AffectedDataKey dataKey, bool dataValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || !base.CanAffect || (base.IsDirect ? YiJinSuoGuGong.DirectParts : YiJinSuoGuGong.ReverseParts).IndexOf((sbyte)dataKey.CustomParam0) < 0;
			bool result;
			if (flag)
			{
				result = dataValue;
			}
			else
			{
				ushort fieldId = dataKey.FieldId;
				bool flag2 = fieldId == 126 || fieldId == 131 || fieldId == 234;
				bool flag3 = flag2;
				result = (!flag3 && dataValue);
			}
			return result;
		}

		// Token: 0x06003D5C RID: 15708 RVA: 0x0025171C File Offset: 0x0024F91C
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || !base.CanAffect || (base.IsDirect ? YiJinSuoGuGong.DirectParts : YiJinSuoGuGong.ReverseParts).IndexOf((sbyte)dataKey.CustomParam0) < 0;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				result = 40;
			}
			return result;
		}

		// Token: 0x04001214 RID: 4628
		private const sbyte AddAvoid = 40;

		// Token: 0x04001215 RID: 4629
		private static readonly sbyte[] DirectParts = new sbyte[]
		{
			3,
			4,
			5,
			6
		};

		// Token: 0x04001216 RID: 4630
		private static readonly sbyte[] ReverseParts = new sbyte[]
		{
			0,
			1
		};
	}
}
