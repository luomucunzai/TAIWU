using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Agile;

namespace GameData.Domains.SpecialEffect.CombatSkill.Shaolinpai.Agile
{
	// Token: 0x0200043F RID: 1087
	public class YiWeiDuJiang : AgileSkillBase
	{
		// Token: 0x06003A0A RID: 14858 RVA: 0x00241B23 File Offset: 0x0023FD23
		public YiWeiDuJiang()
		{
		}

		// Token: 0x06003A0B RID: 14859 RVA: 0x00241B2D File Offset: 0x0023FD2D
		public YiWeiDuJiang(CombatSkillKey skillKey) : base(skillKey, 1405)
		{
			this.ListenCanAffectChange = true;
		}

		// Token: 0x06003A0C RID: 14860 RVA: 0x00241B44 File Offset: 0x0023FD44
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			this._affecting = false;
			this.OnMoveSkillCanAffectChanged(context, default(DataUid));
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 149, -1, -1, -1, -1), EDataModifyType.Custom);
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 147, -1, -1, -1, -1), EDataModifyType.Custom);
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

		// Token: 0x06003A0D RID: 14861 RVA: 0x00241C50 File Offset: 0x0023FE50
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

		// Token: 0x06003A0E RID: 14862 RVA: 0x00241CF8 File Offset: 0x0023FEF8
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
				bool flag2 = (dataKey.FieldId == 149 && dataKey.CustomParam0 >= 0 && DomainManager.Combat.GetElement_CombatCharacterDict(dataKey.CustomParam0).IsAlly != base.CombatChar.IsAlly) || dataKey.FieldId == 147;
				if (flag2)
				{
					result = false;
				}
				else
				{
					bool flag3 = dataKey.FieldId == 55 && dataKey.CustomParam0 == (base.IsDirect ? 1 : 0);
					result = (!flag3 && dataValue);
				}
			}
			return result;
		}

		// Token: 0x040010FE RID: 4350
		private bool _affecting;
	}
}
