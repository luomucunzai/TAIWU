using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Zhujianshanzhuang.AttackCommon;

namespace GameData.Domains.SpecialEffect.CombatSkill.Zhujianshanzhuang.Polearm
{
	// Token: 0x020001CA RID: 458
	public class ShiFangShanHeZhang : RawCreateUnlockEffectBase
	{
		// Token: 0x17000163 RID: 355
		// (get) Token: 0x06002D03 RID: 11523 RVA: 0x0020A2B7 File Offset: 0x002084B7
		private int AddAttackRange
		{
			get
			{
				return base.IsDirectOrReverseEffectDoubling ? 40 : 20;
			}
		}

		// Token: 0x17000164 RID: 356
		// (get) Token: 0x06002D04 RID: 11524 RVA: 0x0020A2C7 File Offset: 0x002084C7
		protected override IEnumerable<sbyte> RequireMainAttributeTypes { get; } = new sbyte[]
		{
			3
		};

		// Token: 0x17000165 RID: 357
		// (get) Token: 0x06002D05 RID: 11525 RVA: 0x0020A2CF File Offset: 0x002084CF
		protected override int RequireMainAttributeValue
		{
			get
			{
				return 75;
			}
		}

		// Token: 0x06002D06 RID: 11526 RVA: 0x0020A2D3 File Offset: 0x002084D3
		public ShiFangShanHeZhang()
		{
		}

		// Token: 0x06002D07 RID: 11527 RVA: 0x0020A2ED File Offset: 0x002084ED
		public ShiFangShanHeZhang(CombatSkillKey skillKey) : base(skillKey, 9302)
		{
		}

		// Token: 0x06002D08 RID: 11528 RVA: 0x0020A30D File Offset: 0x0020850D
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			base.CreateAffectedData(145, EDataModifyType.Add, base.SkillTemplateId);
			base.CreateAffectedData(146, EDataModifyType.Add, base.SkillTemplateId);
		}

		// Token: 0x06002D09 RID: 11529 RVA: 0x0020A340 File Offset: 0x00208540
		protected override void OnAffectedChanged(DataContext context)
		{
			base.OnAffectedChanged(context);
			base.InvalidateCache(context, 145);
			base.InvalidateCache(context, 146);
			bool affected = base.Affected;
			if (affected)
			{
				base.ShowSpecialEffectTips(base.IsDirect, 1, 0);
			}
		}

		// Token: 0x06002D0A RID: 11530 RVA: 0x0020A38C File Offset: 0x0020858C
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.SkillKey != this.SkillKey;
			bool flag2 = flag;
			if (!flag2)
			{
				ushort fieldId = dataKey.FieldId;
				bool flag3 = fieldId - 145 <= 1;
				flag2 = !flag3;
			}
			bool flag4 = flag2 || !base.Affected;
			int result;
			if (flag4)
			{
				result = 0;
			}
			else
			{
				result = this.AddAttackRange;
			}
			return result;
		}
	}
}
