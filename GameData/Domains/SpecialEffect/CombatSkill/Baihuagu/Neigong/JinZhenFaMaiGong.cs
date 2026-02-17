using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Neigong;

namespace GameData.Domains.SpecialEffect.CombatSkill.Baihuagu.Neigong
{
	// Token: 0x020005C1 RID: 1473
	public class JinZhenFaMaiGong : ReduceMainAttributeCharacter
	{
		// Token: 0x170002B1 RID: 689
		// (get) Token: 0x060043AD RID: 17325 RVA: 0x0026C4E7 File Offset: 0x0026A6E7
		private int MaxAddValue
		{
			get
			{
				return base.IsDirect ? 100 : 50;
			}
		}

		// Token: 0x170002B2 RID: 690
		// (get) Token: 0x060043AE RID: 17326 RVA: 0x0026C4F7 File Offset: 0x0026A6F7
		protected override bool DirectIsAlly
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170002B3 RID: 691
		// (get) Token: 0x060043AF RID: 17327 RVA: 0x0026C4FA File Offset: 0x0026A6FA
		protected override EDataModifyType ModifyType
		{
			get
			{
				return EDataModifyType.AddPercent;
			}
		}

		// Token: 0x170002B4 RID: 692
		// (get) Token: 0x060043B0 RID: 17328 RVA: 0x0026C4FD File Offset: 0x0026A6FD
		protected override int CurrAddValue
		{
			get
			{
				return Math.Min((int)(base.CurrMainAttribute / 2), this.MaxAddValue) * (base.IsDirect ? 1 : -1);
			}
		}

		// Token: 0x170002B5 RID: 693
		// (get) Token: 0x060043B1 RID: 17329 RVA: 0x0026C51F File Offset: 0x0026A71F
		protected override IReadOnlyList<ushort> FieldIds { get; } = new ushort[]
		{
			9,
			14,
			11,
			16
		};

		// Token: 0x170002B6 RID: 694
		// (get) Token: 0x060043B2 RID: 17330 RVA: 0x0026C527 File Offset: 0x0026A727
		protected override sbyte MainAttributeType
		{
			get
			{
				return 4;
			}
		}

		// Token: 0x060043B3 RID: 17331 RVA: 0x0026C52A File Offset: 0x0026A72A
		public JinZhenFaMaiGong()
		{
		}

		// Token: 0x060043B4 RID: 17332 RVA: 0x0026C54B File Offset: 0x0026A74B
		public JinZhenFaMaiGong(CombatSkillKey skillKey) : base(skillKey, 3004)
		{
		}

		// Token: 0x060043B5 RID: 17333 RVA: 0x0026C572 File Offset: 0x0026A772
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

		// Token: 0x060043B6 RID: 17334 RVA: 0x0026C584 File Offset: 0x0026A784
		protected override bool IsTargetMatchAffect(CombatCharacter target)
		{
			DefeatMarkCollection marks = target.GetDefeatMarkCollection();
			return marks.GetTotalFlawCount() >= 3 || marks.GetTotalAcupointCount() >= 3;
		}

		// Token: 0x060043B7 RID: 17335 RVA: 0x0026C5B5 File Offset: 0x0026A7B5
		protected override void OnAffected()
		{
			base.ShowSpecialEffectTips(0);
		}

		// Token: 0x04001418 RID: 5144
		private const int NeedCount = 3;

		// Token: 0x04001419 RID: 5145
		private const int UnitAffect = 2;
	}
}
