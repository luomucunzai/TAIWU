using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Agile;

namespace GameData.Domains.SpecialEffect.CombatSkill.Fulongtan.Agile
{
	// Token: 0x02000535 RID: 1333
	public class LongBaiWei : AgileSkillBase
	{
		// Token: 0x06003FA6 RID: 16294 RVA: 0x0025ACA5 File Offset: 0x00258EA5
		public LongBaiWei()
		{
		}

		// Token: 0x06003FA7 RID: 16295 RVA: 0x0025ACAF File Offset: 0x00258EAF
		public LongBaiWei(CombatSkillKey skillKey) : base(skillKey, 14402)
		{
		}

		// Token: 0x06003FA8 RID: 16296 RVA: 0x0025ACC0 File Offset: 0x00258EC0
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			bool isDirect = base.IsDirect;
			if (isDirect)
			{
				int[] charList = DomainManager.Combat.GetCharacterList(!base.CombatChar.IsAlly);
				for (int i = 0; i < charList.Length; i++)
				{
					bool flag = charList[i] >= 0;
					if (flag)
					{
						this.AffectDatas.Add(new AffectedDataKey(charList[i], 127, -1, -1, -1, -1), EDataModifyType.Add);
					}
				}
			}
			else
			{
				this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 127, -1, -1, -1, -1), EDataModifyType.Add);
			}
			base.ShowSpecialEffectTips(0);
		}

		// Token: 0x06003FA9 RID: 16297 RVA: 0x0025AD70 File Offset: 0x00258F70
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
				bool flag2 = dataKey.FieldId == 127;
				if (flag2)
				{
					result = (base.IsDirect ? 1 : -1);
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
