using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Zhujianshanzhuang.AttackCommon;

namespace GameData.Domains.SpecialEffect.CombatSkill.Zhujianshanzhuang.Polearm
{
	// Token: 0x020001C8 RID: 456
	public class PengLaiXianChi : RawCreateUnlockEffectBase
	{
		// Token: 0x1700015D RID: 349
		// (get) Token: 0x06002CF5 RID: 11509 RVA: 0x0020A0EA File Offset: 0x002082EA
		private int ReduceBreathStance
		{
			get
			{
				return base.IsDirectOrReverseEffectDoubling ? -50 : -25;
			}
		}

		// Token: 0x1700015E RID: 350
		// (get) Token: 0x06002CF6 RID: 11510 RVA: 0x0020A0FA File Offset: 0x002082FA
		protected override IEnumerable<sbyte> RequireMainAttributeTypes { get; } = new sbyte[]
		{
			4,
			5
		};

		// Token: 0x1700015F RID: 351
		// (get) Token: 0x06002CF7 RID: 11511 RVA: 0x0020A102 File Offset: 0x00208302
		protected override int RequireMainAttributeValue
		{
			get
			{
				return 65;
			}
		}

		// Token: 0x06002CF8 RID: 11512 RVA: 0x0020A106 File Offset: 0x00208306
		public PengLaiXianChi()
		{
		}

		// Token: 0x06002CF9 RID: 11513 RVA: 0x0020A124 File Offset: 0x00208324
		public PengLaiXianChi(CombatSkillKey skillKey) : base(skillKey, 9305)
		{
		}

		// Token: 0x06002CFA RID: 11514 RVA: 0x0020A148 File Offset: 0x00208348
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			base.CreateAffectedData(204, EDataModifyType.AddPercent, -1);
		}

		// Token: 0x06002CFB RID: 11515 RVA: 0x0020A164 File Offset: 0x00208364
		protected override void OnAffectedChanged(DataContext context)
		{
			base.OnAffectedChanged(context);
			base.InvalidateCache(context, 204);
			DomainManager.Combat.UpdateSkillCanUse(context, base.CombatChar, base.SkillTemplateId);
			bool affected = base.Affected;
			if (affected)
			{
				base.ShowSpecialEffectTips(base.IsDirect, 1, 0);
			}
		}

		// Token: 0x06002CFC RID: 11516 RVA: 0x0020A1B8 File Offset: 0x002083B8
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.SkillKey != this.SkillKey || dataKey.FieldId != 204 || !base.Affected;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				result = this.ReduceBreathStance;
			}
			return result;
		}
	}
}
