using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Agile;

namespace GameData.Domains.SpecialEffect.CombatSkill.Xuannvpai.Agile
{
	// Token: 0x0200028E RID: 654
	public class WangXiaBaBu : AgileSkillBase
	{
		// Token: 0x06003130 RID: 12592 RVA: 0x0021A10C File Offset: 0x0021830C
		public WangXiaBaBu()
		{
		}

		// Token: 0x06003131 RID: 12593 RVA: 0x0021A116 File Offset: 0x00218316
		public WangXiaBaBu(CombatSkillKey skillKey) : base(skillKey, 8403)
		{
			this.ListenCanAffectChange = true;
		}

		// Token: 0x06003132 RID: 12594 RVA: 0x0021A130 File Offset: 0x00218330
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			bool isDirect = base.IsDirect;
			if (isDirect)
			{
				this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 11, -1, -1, -1, -1), EDataModifyType.TotalPercent);
			}
			else
			{
				int[] charList = DomainManager.Combat.GetCharacterList(!base.CombatChar.IsAlly);
				for (int i = 0; i < charList.Length; i++)
				{
					bool flag = charList[i] >= 0;
					if (flag)
					{
						this.AffectDatas.Add(new AffectedDataKey(charList[i], 11, -1, -1, -1, -1), EDataModifyType.TotalPercent);
					}
				}
			}
			bool canAffect = base.CanAffect;
			if (canAffect)
			{
				base.ShowSpecialEffectTips(0);
			}
		}

		// Token: 0x06003133 RID: 12595 RVA: 0x0021A1EC File Offset: 0x002183EC
		protected override void OnMoveSkillCanAffectChanged(DataContext context, DataUid dataUid)
		{
			bool isDirect = base.IsDirect;
			if (isDirect)
			{
				DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 11);
			}
			else
			{
				int[] charList = DomainManager.Combat.GetCharacterList(!base.CombatChar.IsAlly);
				for (int i = 0; i < charList.Length; i++)
				{
					bool flag = charList[i] >= 0;
					if (flag)
					{
						DomainManager.SpecialEffect.InvalidateCache(context, charList[i], 11);
					}
				}
			}
		}

		// Token: 0x06003134 RID: 12596 RVA: 0x0021A26C File Offset: 0x0021846C
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = !base.CanAffect;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				bool flag2 = dataKey.FieldId == 11;
				if (flag2)
				{
					result = (base.IsDirect ? 40 : -40);
				}
				else
				{
					result = 0;
				}
			}
			return result;
		}

		// Token: 0x04000E95 RID: 3733
		private const sbyte ChangeCastSpeed = 40;
	}
}
