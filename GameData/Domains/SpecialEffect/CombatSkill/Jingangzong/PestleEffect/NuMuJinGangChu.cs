using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;

namespace GameData.Domains.SpecialEffect.CombatSkill.Jingangzong.PestleEffect
{
	// Token: 0x020004B1 RID: 1201
	public class NuMuJinGangChu : PestleEffectBase
	{
		// Token: 0x06003CCD RID: 15565 RVA: 0x0024F192 File Offset: 0x0024D392
		public NuMuJinGangChu()
		{
		}

		// Token: 0x06003CCE RID: 15566 RVA: 0x0024F19C File Offset: 0x0024D39C
		public NuMuJinGangChu(int charId) : base(charId, 11403)
		{
		}

		// Token: 0x06003CCF RID: 15567 RVA: 0x0024F1AC File Offset: 0x0024D3AC
		public override void OnEnable(DataContext context)
		{
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			int[] charList = DomainManager.Combat.GetCharacterList(!base.CombatChar.IsAlly);
			for (int i = 0; i < charList.Length; i++)
			{
				bool flag = charList[i] >= 0;
				if (flag)
				{
					this.AffectDatas.Add(new AffectedDataKey(charList[i], 116, -1, -1, -1, -1), EDataModifyType.Add);
				}
			}
			base.OnEnable(context);
		}

		// Token: 0x06003CD0 RID: 15568 RVA: 0x0024F224 File Offset: 0x0024D424
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = !base.CanAffect || !DomainManager.Combat.IsCurrentCombatCharacter(base.CombatChar) || base.CombatChar.TeammateBeforeMainChar >= 0;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				bool flag2 = dataKey.FieldId == 116 && dataKey.CustomParam1 == (base.IsDirect ? 0 : 1);
				if (flag2)
				{
					result = 1;
				}
				else
				{
					result = 0;
				}
			}
			return result;
		}
	}
}
