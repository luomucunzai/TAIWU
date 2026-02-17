using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Neigong;

namespace GameData.Domains.SpecialEffect.CombatSkill.Emeipai.Neigong
{
	// Token: 0x0200054B RID: 1355
	public class LingWuGong : ReduceMainAttributeCharacter
	{
		// Token: 0x1700025B RID: 603
		// (get) Token: 0x06004027 RID: 16423 RVA: 0x0025D13F File Offset: 0x0025B33F
		private int MaxAddValue
		{
			get
			{
				return base.IsDirect ? 50 : 100;
			}
		}

		// Token: 0x1700025C RID: 604
		// (get) Token: 0x06004028 RID: 16424 RVA: 0x0025D14F File Offset: 0x0025B34F
		protected override sbyte MainAttributeType
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x1700025D RID: 605
		// (get) Token: 0x06004029 RID: 16425 RVA: 0x0025D152 File Offset: 0x0025B352
		protected override bool DirectIsAlly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700025E RID: 606
		// (get) Token: 0x0600402A RID: 16426 RVA: 0x0025D155 File Offset: 0x0025B355
		protected override IReadOnlyList<ushort> FieldIds { get; } = new ushort[]
		{
			316,
			300
		};

		// Token: 0x1700025F RID: 607
		// (get) Token: 0x0600402B RID: 16427 RVA: 0x0025D15D File Offset: 0x0025B35D
		protected override EDataModifyType ModifyType
		{
			get
			{
				return EDataModifyType.AddPercent;
			}
		}

		// Token: 0x17000260 RID: 608
		// (get) Token: 0x0600402C RID: 16428 RVA: 0x0025D160 File Offset: 0x0025B360
		protected override int CurrAddValue
		{
			get
			{
				return Math.Min((int)(base.CurrMainAttribute / 3), this.MaxAddValue) * (base.IsDirect ? -1 : 1);
			}
		}

		// Token: 0x17000261 RID: 609
		// (get) Token: 0x0600402D RID: 16429 RVA: 0x0025D182 File Offset: 0x0025B382
		protected override bool IsAffect
		{
			get
			{
				return (base.IsDirect ? base.CombatChar : base.EnemyChar).UsableTrickCount >= 5;
			}
		}

		// Token: 0x0600402E RID: 16430 RVA: 0x0025D1A5 File Offset: 0x0025B3A5
		public LingWuGong()
		{
		}

		// Token: 0x0600402F RID: 16431 RVA: 0x0025D1CB File Offset: 0x0025B3CB
		public LingWuGong(CombatSkillKey skillKey) : base(skillKey, 2003)
		{
		}

		// Token: 0x06004030 RID: 16432 RVA: 0x0025D1F8 File Offset: 0x0025B3F8
		protected override bool IsTargetMatchAffect(CombatCharacter target)
		{
			target = (base.IsDirect ? base.CombatChar : base.EnemyChar);
			return target.UsableTrickCount >= 5;
		}

		// Token: 0x06004031 RID: 16433 RVA: 0x0025D22E File Offset: 0x0025B42E
		protected override IEnumerable<DataUid> GetUpdateAffectingDataUids()
		{
			yield return base.ParseCombatCharacterDataUid(28);
			foreach (int charId in DomainManager.Combat.GetCharacterList(!base.CombatChar.IsAlly))
			{
				bool flag = charId >= 0;
				if (flag)
				{
					yield return base.ParseCombatCharacterDataUid(charId, 28);
				}
			}
			int[] array = null;
			yield break;
		}

		// Token: 0x06004032 RID: 16434 RVA: 0x0025D23E File Offset: 0x0025B43E
		protected override void OnAffected()
		{
			base.ShowSpecialEffectTips(0);
		}

		// Token: 0x040012DF RID: 4831
		private const int NeedTrickCount = 5;

		// Token: 0x040012E0 RID: 4832
		private const int UnitValue = 3;
	}
}
