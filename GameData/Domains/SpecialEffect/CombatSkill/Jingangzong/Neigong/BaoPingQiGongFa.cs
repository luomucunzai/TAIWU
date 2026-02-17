using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Neigong;

namespace GameData.Domains.SpecialEffect.CombatSkill.Jingangzong.Neigong
{
	// Token: 0x020004B3 RID: 1203
	public class BaoPingQiGongFa : ReduceMainAttributeCharacter
	{
		// Token: 0x17000230 RID: 560
		// (get) Token: 0x06003CD8 RID: 15576 RVA: 0x0024F3A8 File Offset: 0x0024D5A8
		private int MaxAddValue
		{
			get
			{
				return base.IsDirect ? 50 : 100;
			}
		}

		// Token: 0x17000231 RID: 561
		// (get) Token: 0x06003CD9 RID: 15577 RVA: 0x0024F3B8 File Offset: 0x0024D5B8
		protected override sbyte MainAttributeType
		{
			get
			{
				return 5;
			}
		}

		// Token: 0x17000232 RID: 562
		// (get) Token: 0x06003CDA RID: 15578 RVA: 0x0024F3BB File Offset: 0x0024D5BB
		protected override bool DirectIsAlly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000233 RID: 563
		// (get) Token: 0x06003CDB RID: 15579 RVA: 0x0024F3BE File Offset: 0x0024D5BE
		protected override EDataModifyType ModifyType
		{
			get
			{
				return EDataModifyType.AddPercent;
			}
		}

		// Token: 0x17000234 RID: 564
		// (get) Token: 0x06003CDC RID: 15580 RVA: 0x0024F3C1 File Offset: 0x0024D5C1
		protected override int CurrAddValue
		{
			get
			{
				return Math.Min((int)(base.CurrMainAttribute / 2), this.MaxAddValue) * (base.IsDirect ? -1 : 1);
			}
		}

		// Token: 0x17000235 RID: 565
		// (get) Token: 0x06003CDD RID: 15581 RVA: 0x0024F3E3 File Offset: 0x0024D5E3
		protected override IReadOnlyList<ushort> FieldIds { get; } = new ushort[]
		{
			38,
			39,
			40,
			41
		};

		// Token: 0x06003CDE RID: 15582 RVA: 0x0024F3EB File Offset: 0x0024D5EB
		public BaoPingQiGongFa()
		{
		}

		// Token: 0x06003CDF RID: 15583 RVA: 0x0024F40C File Offset: 0x0024D60C
		public BaoPingQiGongFa(CombatSkillKey skillKey) : base(skillKey, 11003)
		{
		}

		// Token: 0x06003CE0 RID: 15584 RVA: 0x0024F433 File Offset: 0x0024D633
		protected override IEnumerable<DataUid> GetUpdateAffectingDataUids()
		{
			yield return base.ParseCombatCharacterDataUid(50);
			foreach (int charId in DomainManager.Combat.GetCharacterList(!base.CombatChar.IsAlly))
			{
				bool flag = charId >= 0;
				if (flag)
				{
					yield return base.ParseCombatCharacterDataUid(charId, 50);
				}
			}
			int[] array = null;
			yield break;
		}

		// Token: 0x06003CE1 RID: 15585 RVA: 0x0024F444 File Offset: 0x0024D644
		protected override bool IsTargetMatchAffect(CombatCharacter target)
		{
			DefeatMarkCollection marks = target.GetDefeatMarkCollection();
			return marks.NeiliAllocationMarkCount.Item1 + marks.NeiliAllocationMarkCount.Item2 >= 3;
		}

		// Token: 0x06003CE2 RID: 15586 RVA: 0x0024F47A File Offset: 0x0024D67A
		protected override void OnAffected()
		{
			base.ShowSpecialEffectTips(0);
		}

		// Token: 0x040011E9 RID: 4585
		private const int NeedCount = 3;

		// Token: 0x040011EA RID: 4586
		private const int UnitAffect = 2;
	}
}
