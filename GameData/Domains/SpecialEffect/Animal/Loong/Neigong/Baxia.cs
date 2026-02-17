using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.Animal.Loong.Neigong
{
	// Token: 0x020005FD RID: 1533
	public class Baxia : AnimalEffectBase
	{
		// Token: 0x060044FB RID: 17659 RVA: 0x00270F67 File Offset: 0x0026F167
		public Baxia()
		{
		}

		// Token: 0x060044FC RID: 17660 RVA: 0x00270F71 File Offset: 0x0026F171
		public Baxia(CombatSkillKey skillKey) : base(skillKey)
		{
		}

		// Token: 0x060044FD RID: 17661 RVA: 0x00270F7C File Offset: 0x0026F17C
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			base.CreateAffectedData(320, EDataModifyType.Custom, -1);
		}

		// Token: 0x060044FE RID: 17662 RVA: 0x00270F98 File Offset: 0x0026F198
		public override long GetModifiedValue(AffectedDataKey dataKey, long dataValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || dataKey.FieldId != 320;
			long result;
			if (flag)
			{
				result = base.GetModifiedValue(dataKey, dataValue);
			}
			else
			{
				bool critical = dataKey.CustomParam1 == 1;
				bool flag2 = critical;
				if (flag2)
				{
					result = base.GetModifiedValue(dataKey, dataValue);
				}
				else
				{
					base.ShowSpecialEffectTipsOnceInFrame(0);
					result = 0L;
				}
			}
			return result;
		}
	}
}
