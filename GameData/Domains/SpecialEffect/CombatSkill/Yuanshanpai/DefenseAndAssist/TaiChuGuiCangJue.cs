using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Defense;

namespace GameData.Domains.SpecialEffect.CombatSkill.Yuanshanpai.DefenseAndAssist
{
	// Token: 0x02000209 RID: 521
	public class TaiChuGuiCangJue : DefenseSkillBase
	{
		// Token: 0x06002ECC RID: 11980 RVA: 0x00210B02 File Offset: 0x0020ED02
		public TaiChuGuiCangJue()
		{
		}

		// Token: 0x06002ECD RID: 11981 RVA: 0x00210B0C File Offset: 0x0020ED0C
		public TaiChuGuiCangJue(CombatSkillKey skillKey) : base(skillKey, 5506)
		{
		}

		// Token: 0x06002ECE RID: 11982 RVA: 0x00210B1C File Offset: 0x0020ED1C
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			bool isDirect = base.IsDirect;
			if (isDirect)
			{
				this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 137, -1, -1, -1, -1), EDataModifyType.Custom);
				this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 136, -1, -1, -1, -1), EDataModifyType.Custom);
			}
			else
			{
				int[] charList = DomainManager.Combat.GetCharacterList(!base.CombatChar.IsAlly);
				for (int i = 0; i < charList.Length; i++)
				{
					bool flag = charList[i] >= 0;
					if (flag)
					{
						this.AffectDatas.Add(new AffectedDataKey(charList[i], 137, -1, -1, -1, -1), EDataModifyType.Custom);
						this.AffectDatas.Add(new AffectedDataKey(charList[i], 135, -1, -1, -1, -1), EDataModifyType.Custom);
					}
				}
			}
		}

		// Token: 0x06002ECF RID: 11983 RVA: 0x00210C0C File Offset: 0x0020EE0C
		public override bool GetModifiedValue(AffectedDataKey dataKey, bool dataValue)
		{
			bool flag = !base.CanAffect;
			bool result;
			if (flag)
			{
				result = dataValue;
			}
			else
			{
				bool flag2 = dataKey.FieldId == 137 && dataKey.CustomParam0 == (base.IsDirect ? 1 : 0);
				result = (!flag2 && dataValue);
			}
			return result;
		}

		// Token: 0x06002ED0 RID: 11984 RVA: 0x00210C5C File Offset: 0x0020EE5C
		public override int GetModifiedValue(AffectedDataKey dataKey, int dataValue)
		{
			bool flag = !base.CanAffect && !(base.IsDirect ? (dataValue < 0) : (dataValue > 0));
			int result;
			if (flag)
			{
				result = dataValue;
			}
			else
			{
				bool canChange = DomainManager.SpecialEffect.ModifyData(dataKey.CharId, -1, 137, true, base.IsDirect ? 0 : 1, -1, -1);
				base.ShowSpecialEffectTips(canChange, 0, 1);
				result = (canChange ? (-dataValue * 2) : 0);
			}
			return result;
		}
	}
}
