using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Agile;

namespace GameData.Domains.SpecialEffect.CombatSkill.Wuxianjiao.Agile
{
	// Token: 0x020003AD RID: 941
	public class ChuangNiTan : AgileSkillBase
	{
		// Token: 0x060036DB RID: 14043 RVA: 0x0023291D File Offset: 0x00230B1D
		public ChuangNiTan()
		{
		}

		// Token: 0x060036DC RID: 14044 RVA: 0x00232927 File Offset: 0x00230B27
		public ChuangNiTan(CombatSkillKey skillKey) : base(skillKey, 12601)
		{
		}

		// Token: 0x060036DD RID: 14045 RVA: 0x00232938 File Offset: 0x00230B38
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			this._affecting = base.CanAffect;
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			bool isDirect = base.IsDirect;
			if (isDirect)
			{
				this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 55, -1, -1, -1, -1), EDataModifyType.Custom);
			}
			else
			{
				int[] charList = DomainManager.Combat.GetCharacterList(!base.CombatChar.IsAlly);
				for (int i = 0; i < charList.Length; i++)
				{
					bool flag = charList[i] >= 0;
					if (flag)
					{
						this.AffectDatas.Add(new AffectedDataKey(charList[i], 55, -1, -1, -1, -1), EDataModifyType.Custom);
					}
				}
			}
			base.ShowSpecialEffectTips(0);
		}

		// Token: 0x060036DE RID: 14046 RVA: 0x002329F4 File Offset: 0x00230BF4
		protected override void OnMoveSkillCanAffectChanged(DataContext context, DataUid dataUid)
		{
			bool canAffect = base.CanAffect;
			bool flag = this._affecting == canAffect;
			if (!flag)
			{
				this._affecting = canAffect;
				bool isDirect = base.IsDirect;
				if (isDirect)
				{
					DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 55);
				}
				else
				{
					int[] charList = DomainManager.Combat.GetCharacterList(!base.CombatChar.IsAlly);
					for (int i = 0; i < charList.Length; i++)
					{
						bool flag2 = charList[i] >= 0;
						if (flag2)
						{
							DomainManager.SpecialEffect.InvalidateCache(context, charList[i], 55);
						}
					}
				}
			}
		}

		// Token: 0x060036DF RID: 14047 RVA: 0x00232A9C File Offset: 0x00230C9C
		public override bool GetModifiedValue(AffectedDataKey dataKey, bool dataValue)
		{
			bool flag = !this._affecting;
			bool result;
			if (flag)
			{
				result = dataValue;
			}
			else
			{
				bool flag2 = dataKey.FieldId == 55 && dataKey.CustomParam0 == (base.IsDirect ? 1 : 0);
				result = (!flag2 && dataValue);
			}
			return result;
		}

		// Token: 0x04000FFF RID: 4095
		private bool _affecting;
	}
}
