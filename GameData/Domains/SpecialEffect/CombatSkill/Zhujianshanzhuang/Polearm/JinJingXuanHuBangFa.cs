using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Zhujianshanzhuang.AttackCommon;

namespace GameData.Domains.SpecialEffect.CombatSkill.Zhujianshanzhuang.Polearm
{
	// Token: 0x020001C7 RID: 455
	public class JinJingXuanHuBangFa : RawCreateUnlockEffectBase
	{
		// Token: 0x1700015A RID: 346
		// (get) Token: 0x06002CEC RID: 11500 RVA: 0x00209FB9 File Offset: 0x002081B9
		private int AddHitOdds
		{
			get
			{
				return base.IsDirectOrReverseEffectDoubling ? 300 : 150;
			}
		}

		// Token: 0x1700015B RID: 347
		// (get) Token: 0x06002CED RID: 11501 RVA: 0x00209FCF File Offset: 0x002081CF
		protected override IEnumerable<sbyte> RequireMainAttributeTypes { get; } = new sbyte[]
		{
			0,
			2
		};

		// Token: 0x1700015C RID: 348
		// (get) Token: 0x06002CEE RID: 11502 RVA: 0x00209FD7 File Offset: 0x002081D7
		protected override int RequireMainAttributeValue
		{
			get
			{
				return 65;
			}
		}

		// Token: 0x06002CEF RID: 11503 RVA: 0x00209FDB File Offset: 0x002081DB
		public JinJingXuanHuBangFa()
		{
		}

		// Token: 0x06002CF0 RID: 11504 RVA: 0x00209FF5 File Offset: 0x002081F5
		public JinJingXuanHuBangFa(CombatSkillKey skillKey) : base(skillKey, 9304)
		{
		}

		// Token: 0x06002CF1 RID: 11505 RVA: 0x0020A015 File Offset: 0x00208215
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			base.CreateAffectedData(74, EDataModifyType.AddPercent, base.SkillTemplateId);
			Events.RegisterHandler_CastAttackSkillBegin(new Events.OnCastAttackSkillBegin(this.OnCastAttackSkillBegin));
		}

		// Token: 0x06002CF2 RID: 11506 RVA: 0x0020A042 File Offset: 0x00208242
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CastAttackSkillBegin(new Events.OnCastAttackSkillBegin(this.OnCastAttackSkillBegin));
			base.OnDisable(context);
		}

		// Token: 0x06002CF3 RID: 11507 RVA: 0x0020A060 File Offset: 0x00208260
		private void OnCastAttackSkillBegin(DataContext context, CombatCharacter attacker, CombatCharacter defender, short skillId)
		{
			bool flag = this.SkillKey.IsMatch(attacker.GetId(), skillId) && base.IsReverseOrUsingDirectWeapon;
			if (flag)
			{
				base.ShowSpecialEffectTips(base.IsDirect, 1, 0);
			}
		}

		// Token: 0x06002CF4 RID: 11508 RVA: 0x0020A0A0 File Offset: 0x002082A0
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.SkillKey != this.SkillKey || dataKey.FieldId != 74 || !base.IsReverseOrUsingDirectWeapon;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				result = this.AddHitOdds;
			}
			return result;
		}
	}
}
