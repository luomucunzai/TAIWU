using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Assist;

namespace GameData.Domains.SpecialEffect.CombatSkill.Kongsangpai.DefenseAndAssist
{
	// Token: 0x0200049B RID: 1179
	public class QuXueFa : AssistSkillBase
	{
		// Token: 0x06003C54 RID: 15444 RVA: 0x0024D0BF File Offset: 0x0024B2BF
		public QuXueFa()
		{
		}

		// Token: 0x06003C55 RID: 15445 RVA: 0x0024D0C9 File Offset: 0x0024B2C9
		public QuXueFa(CombatSkillKey skillKey) : base(skillKey, 10705)
		{
			this.SetConstAffectingOnCombatBegin = true;
		}

		// Token: 0x06003C56 RID: 15446 RVA: 0x0024D0E0 File Offset: 0x0024B2E0
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			bool isDirect = base.IsDirect;
			if (isDirect)
			{
				base.CreateAffectedData(73, EDataModifyType.TotalPercent, -1);
			}
			else
			{
				base.CreateAffectedAllEnemyData(243, EDataModifyType.Add, -1);
				base.ShowSpecialEffectTips(0);
			}
		}

		// Token: 0x06003C57 RID: 15447 RVA: 0x0024D128 File Offset: 0x0024B328
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
				bool flag2 = !base.IsDirect && !base.IsCurrent;
				if (flag2)
				{
					result = 0;
				}
				else
				{
					ushort fieldId = dataKey.FieldId;
					bool flag3 = fieldId == 73 || fieldId == 243;
					bool flag4 = flag3;
					if (flag4)
					{
						bool isDirect = base.IsDirect;
						if (isDirect)
						{
							base.ShowSpecialEffectTipsOnceInFrame(0);
						}
						result = ((dataKey.FieldId == 73) ? 50 : QuXueFa.PoisonAffectThresholdValues[dataKey.CustomParam0]);
					}
					else
					{
						result = 0;
					}
				}
			}
			return result;
		}

		// Token: 0x040011C0 RID: 4544
		private const sbyte AddPercent = 50;

		// Token: 0x040011C1 RID: 4545
		private static readonly int[] PoisonAffectThresholdValues = new int[]
		{
			-1,
			-15,
			-25,
			-25,
			-200,
			-200
		};
	}
}
