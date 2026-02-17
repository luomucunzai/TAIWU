using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Baihuagu.Neigong
{
	// Token: 0x020005C3 RID: 1475
	public class NeiJingLingShuPian : CombatSkillEffectBase
	{
		// Token: 0x060043BC RID: 17340 RVA: 0x0026C66B File Offset: 0x0026A86B
		public NeiJingLingShuPian()
		{
		}

		// Token: 0x060043BD RID: 17341 RVA: 0x0026C678 File Offset: 0x0026A878
		public NeiJingLingShuPian(CombatSkillKey skillKey) : base(skillKey, 3003, -1)
		{
			bool isDirect = base.IsDirect;
			if (isDirect)
			{
				base.CreateAffectedData(119, EDataModifyType.AddPercent, -1);
			}
			else
			{
				base.CreateAffectedAllEnemyData(120, EDataModifyType.AddPercent, -1);
			}
		}

		// Token: 0x060043BE RID: 17342 RVA: 0x0026C6B6 File Offset: 0x0026A8B6
		public override void OnEnable(DataContext context)
		{
		}

		// Token: 0x060043BF RID: 17343 RVA: 0x0026C6BC File Offset: 0x0026A8BC
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			ushort fieldId = dataKey.FieldId;
			bool flag = fieldId - 119 <= 1;
			bool flag2 = !flag;
			int result;
			if (flag2)
			{
				result = 0;
			}
			else
			{
				bool flag3 = !base.IsDirect && !base.IsCurrent;
				if (flag3)
				{
					result = 0;
				}
				else
				{
					bool flag4 = dataKey.CustomParam0 == 0;
					if (flag4)
					{
						base.ShowSpecialEffectTipsOnceInFrame(0);
					}
					ushort fieldId2 = dataKey.FieldId;
					if (!true)
					{
					}
					int num;
					if (fieldId2 != 119)
					{
						if (fieldId2 != 120)
						{
							num = 0;
						}
						else
						{
							num = -50;
						}
					}
					else
					{
						num = 50;
					}
					if (!true)
					{
					}
					result = num;
				}
			}
			return result;
		}

		// Token: 0x0400141C RID: 5148
		private const sbyte HealCountChangePercent = 50;
	}
}
