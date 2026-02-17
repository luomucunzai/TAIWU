using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Neigong;

namespace GameData.Domains.SpecialEffect.CombatSkill.Zhujianshanzhuang.Neigong
{
	// Token: 0x020001CD RID: 461
	public class FengJieChuYuanPian : ReduceMainAttributeCharacter
	{
		// Token: 0x1700016A RID: 362
		// (get) Token: 0x06002D16 RID: 11542 RVA: 0x0020A4EB File Offset: 0x002086EB
		private int MaxAddValue
		{
			get
			{
				return base.IsDirect ? 100 : 50;
			}
		}

		// Token: 0x1700016B RID: 363
		// (get) Token: 0x06002D17 RID: 11543 RVA: 0x0020A4FB File Offset: 0x002086FB
		protected override bool DirectIsAlly
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700016C RID: 364
		// (get) Token: 0x06002D18 RID: 11544 RVA: 0x0020A4FE File Offset: 0x002086FE
		protected override IReadOnlyList<ushort> FieldIds { get; } = new ushort[]
		{
			315,
			317
		};

		// Token: 0x1700016D RID: 365
		// (get) Token: 0x06002D19 RID: 11545 RVA: 0x0020A506 File Offset: 0x00208706
		protected override EDataModifyType ModifyType
		{
			get
			{
				return EDataModifyType.AddPercent;
			}
		}

		// Token: 0x1700016E RID: 366
		// (get) Token: 0x06002D1A RID: 11546 RVA: 0x0020A509 File Offset: 0x00208709
		protected override int CurrAddValue
		{
			get
			{
				return Math.Min((int)(base.CurrMainAttribute / 2), this.MaxAddValue) * (base.IsDirect ? 1 : -1);
			}
		}

		// Token: 0x1700016F RID: 367
		// (get) Token: 0x06002D1B RID: 11547 RVA: 0x0020A52B File Offset: 0x0020872B
		protected override sbyte MainAttributeType
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x06002D1C RID: 11548 RVA: 0x0020A52E File Offset: 0x0020872E
		public FengJieChuYuanPian()
		{
		}

		// Token: 0x06002D1D RID: 11549 RVA: 0x0020A554 File Offset: 0x00208754
		public FengJieChuYuanPian(CombatSkillKey skillKey) : base(skillKey, 9005)
		{
		}

		// Token: 0x06002D1E RID: 11550 RVA: 0x0020A580 File Offset: 0x00208780
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

		// Token: 0x06002D1F RID: 11551 RVA: 0x0020A590 File Offset: 0x00208790
		protected override bool IsTargetMatchAffect(CombatCharacter target)
		{
			return DomainManager.Combat.IsCharacterHalfFallen(target);
		}

		// Token: 0x06002D20 RID: 11552 RVA: 0x0020A59D File Offset: 0x0020879D
		protected override void OnAffected()
		{
			base.ShowSpecialEffectTips(0);
			base.ShowSpecialEffectTips(1);
		}

		// Token: 0x04000D8C RID: 3468
		private const int UnitAffect = 2;
	}
}
