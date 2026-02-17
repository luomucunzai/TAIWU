using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Agile;

namespace GameData.Domains.SpecialEffect.CombatSkill.Baihuagu.Agile
{
	// Token: 0x020005E1 RID: 1505
	public class TaXueXunMei : AgileSkillBase
	{
		// Token: 0x06004464 RID: 17508 RVA: 0x0026F614 File Offset: 0x0026D814
		public TaXueXunMei()
		{
		}

		// Token: 0x06004465 RID: 17509 RVA: 0x0026F61E File Offset: 0x0026D81E
		public TaXueXunMei(CombatSkillKey skillKey) : base(skillKey, 3404)
		{
		}

		// Token: 0x06004466 RID: 17510 RVA: 0x0026F630 File Offset: 0x0026D830
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
						this.AffectDatas.Add(new AffectedDataKey(charList[i], 132, -1, -1, -1, -1), EDataModifyType.Add);
					}
				}
			}
			else
			{
				this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 132, -1, -1, -1, -1), EDataModifyType.Add);
			}
			base.ShowSpecialEffectTips(0);
		}

		// Token: 0x06004467 RID: 17511 RVA: 0x0026F6E4 File Offset: 0x0026D8E4
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
				bool flag2 = dataKey.FieldId == 132;
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
