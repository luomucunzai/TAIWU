using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Zhujianshanzhuang.AttackCommon;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.Zhujianshanzhuang.Blade
{
	// Token: 0x020001DC RID: 476
	public class GuiPaoDingDaoFa : BladeUnlockEffectBase
	{
		// Token: 0x17000178 RID: 376
		// (get) Token: 0x06002D8F RID: 11663 RVA: 0x0020C345 File Offset: 0x0020A545
		private int AddDamagePercentPerMark
		{
			get
			{
				return base.IsDirectOrReverseEffectDoubling ? 6 : 3;
			}
		}

		// Token: 0x17000179 RID: 377
		// (get) Token: 0x06002D90 RID: 11664 RVA: 0x0020C354 File Offset: 0x0020A554
		protected override IEnumerable<short> RequireWeaponTypes
		{
			get
			{
				yield return 14;
				yield return 15;
				yield break;
			}
		}

		// Token: 0x06002D91 RID: 11665 RVA: 0x0020C373 File Offset: 0x0020A573
		public GuiPaoDingDaoFa()
		{
		}

		// Token: 0x06002D92 RID: 11666 RVA: 0x0020C37D File Offset: 0x0020A57D
		public GuiPaoDingDaoFa(CombatSkillKey skillKey) : base(skillKey, 9204)
		{
		}

		// Token: 0x06002D93 RID: 11667 RVA: 0x0020C38D File Offset: 0x0020A58D
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			base.CreateAffectedData(69, EDataModifyType.AddPercent, -1);
			Events.RegisterHandler_CastAttackSkillBegin(new Events.OnCastAttackSkillBegin(this.OnCastAttackSkillBegin));
		}

		// Token: 0x06002D94 RID: 11668 RVA: 0x0020C3B5 File Offset: 0x0020A5B5
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CastAttackSkillBegin(new Events.OnCastAttackSkillBegin(this.OnCastAttackSkillBegin));
			base.OnDisable(context);
		}

		// Token: 0x06002D95 RID: 11669 RVA: 0x0020C3D4 File Offset: 0x0020A5D4
		private void OnCastAttackSkillBegin(DataContext context, CombatCharacter attacker, CombatCharacter defender, short skillId)
		{
			bool flag = this.SkillKey.IsMatch(attacker.GetId(), skillId) && base.IsReverseOrUsingDirectWeapon;
			if (flag)
			{
				base.ShowSpecialEffectTips(base.IsDirect, 1, 0);
			}
		}

		// Token: 0x06002D96 RID: 11670 RVA: 0x0020C414 File Offset: 0x0020A614
		protected override bool CanDoAffect()
		{
			DefeatMarkCollection marks = base.CombatChar.GetDefeatMarkCollection();
			return marks.MindMarkList.Count > 0 || marks.GetTotalFlawCount() > 0 || marks.GetTotalAcupointCount() > 0;
		}

		// Token: 0x06002D97 RID: 11671 RVA: 0x0020C458 File Offset: 0x0020A658
		public override void DoAffectAfterCost(DataContext context, int weaponIndex)
		{
			DefeatMarkCollection marks = base.CombatChar.GetDefeatMarkCollection();
			List<int> weights = ObjectPool<List<int>>.Instance.Get();
			weights.Add(marks.GetTotalFlawCount());
			weights.Add(marks.GetTotalAcupointCount());
			weights.Add(marks.MindMarkList.Count);
			bool flag = weights.Sum() > 0;
			if (flag)
			{
				base.ShowSpecialEffectTips(base.IsDirect, 2, 1);
			}
			for (int i = 0; i < 5; i++)
			{
				bool flag2 = weights.Sum() == 0;
				if (flag2)
				{
					break;
				}
				int index = RandomUtils.GetRandomIndex(weights, context.Random);
				bool flag3 = index == 0;
				if (flag3)
				{
					DomainManager.Combat.TransferRandomFlaw(context, base.CombatChar, base.EnemyChar);
				}
				bool flag4 = index == 1;
				if (flag4)
				{
					DomainManager.Combat.TransferRandomAcupoint(context, base.CombatChar, base.EnemyChar);
				}
				bool flag5 = index == 2;
				if (flag5)
				{
					DomainManager.Combat.TransferRandomMindDefeatMark(context, base.CombatChar, base.EnemyChar);
				}
				List<int> list = weights;
				int index2 = index;
				list[index2]--;
			}
			ObjectPool<List<int>>.Instance.Return(weights);
		}

		// Token: 0x06002D98 RID: 11672 RVA: 0x0020C594 File Offset: 0x0020A794
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.SkillKey != this.SkillKey || !base.IsReverseOrUsingDirectWeapon;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				bool flag2 = dataKey.FieldId == 69;
				if (flag2)
				{
					result = base.CombatChar.GetDefeatMarkCollection().GetTotalCount() * this.AddDamagePercentPerMark;
				}
				else
				{
					result = 0;
				}
			}
			return result;
		}

		// Token: 0x04000DAE RID: 3502
		private const int MaxTransferCount = 5;
	}
}
