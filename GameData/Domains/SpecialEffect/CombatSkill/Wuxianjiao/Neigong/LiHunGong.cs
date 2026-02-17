using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.Character;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.Item;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Neigong;

namespace GameData.Domains.SpecialEffect.CombatSkill.Wuxianjiao.Neigong
{
	// Token: 0x02000390 RID: 912
	public class LiHunGong : ReduceMainAttributeCharacter
	{
		// Token: 0x170001E1 RID: 481
		// (get) Token: 0x06003648 RID: 13896 RVA: 0x0023045E File Offset: 0x0022E65E
		protected override sbyte MainAttributeType
		{
			get
			{
				return 3;
			}
		}

		// Token: 0x170001E2 RID: 482
		// (get) Token: 0x06003649 RID: 13897 RVA: 0x00230461 File Offset: 0x0022E661
		protected override bool DirectIsAlly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170001E3 RID: 483
		// (get) Token: 0x0600364A RID: 13898 RVA: 0x00230464 File Offset: 0x0022E664
		protected override IReadOnlyList<ushort> FieldIds { get; } = new ushort[]
		{
			245
		};

		// Token: 0x170001E4 RID: 484
		// (get) Token: 0x0600364B RID: 13899 RVA: 0x0023046C File Offset: 0x0022E66C
		protected override EDataModifyType ModifyType
		{
			get
			{
				return EDataModifyType.Add;
			}
		}

		// Token: 0x170001E5 RID: 485
		// (get) Token: 0x0600364C RID: 13900 RVA: 0x0023046F File Offset: 0x0022E66F
		protected override int CurrAddValue
		{
			get
			{
				return Math.Min((int)(base.CurrMainAttribute * 10), 1000) * (base.IsDirect ? -1 : 1);
			}
		}

		// Token: 0x0600364D RID: 13901 RVA: 0x00230491 File Offset: 0x0022E691
		public LiHunGong()
		{
		}

		// Token: 0x0600364E RID: 13902 RVA: 0x002304AF File Offset: 0x0022E6AF
		public LiHunGong(CombatSkillKey skillKey) : base(skillKey, 12005)
		{
		}

		// Token: 0x0600364F RID: 13903 RVA: 0x002304D3 File Offset: 0x0022E6D3
		protected override IEnumerable<DataUid> GetUpdateAffectingDataUids()
		{
			yield return base.ParseCharDataUid(59);
			foreach (int charId in DomainManager.Combat.GetCharacterList(!base.CombatChar.IsAlly))
			{
				bool flag = charId >= 0;
				if (flag)
				{
					yield return base.ParseCharDataUid(charId, 59);
				}
			}
			int[] array = null;
			yield break;
		}

		// Token: 0x06003650 RID: 13904 RVA: 0x002304E4 File Offset: 0x0022E6E4
		protected unsafe override bool IsTargetMatchAffect(CombatCharacter target)
		{
			int wugCount = 0;
			EatingItems eatingItems = *target.GetCharacter().GetEatingItems();
			for (int i = 0; i < 9; i++)
			{
				ItemKey key = eatingItems.Get(i);
				bool flag = EatingItems.IsWug(key);
				if (flag)
				{
					wugCount++;
				}
			}
			return wugCount >= 2;
		}

		// Token: 0x06003651 RID: 13905 RVA: 0x00230540 File Offset: 0x0022E740
		protected override void OnAffected()
		{
			base.ShowSpecialEffectTips(0);
		}

		// Token: 0x04000FD5 RID: 4053
		private const int NeedCount = 2;

		// Token: 0x04000FD6 RID: 4054
		private const int AddValueUnit = 10;

		// Token: 0x04000FD7 RID: 4055
		private const int MaxAddValue = 1000;
	}
}
