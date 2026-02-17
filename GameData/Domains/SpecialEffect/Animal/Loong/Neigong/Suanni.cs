using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.Animal.Loong.Neigong
{
	// Token: 0x02000604 RID: 1540
	public class Suanni : AnimalEffectBase
	{
		// Token: 0x06004529 RID: 17705 RVA: 0x00271A65 File Offset: 0x0026FC65
		public Suanni()
		{
		}

		// Token: 0x0600452A RID: 17706 RVA: 0x00271A6F File Offset: 0x0026FC6F
		public Suanni(CombatSkillKey skillKey) : base(skillKey)
		{
		}

		// Token: 0x0600452B RID: 17707 RVA: 0x00271A7A File Offset: 0x0026FC7A
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			base.CreateAffectedData(89, EDataModifyType.Custom, -1);
			base.CreateAffectedAllEnemyData(191, EDataModifyType.TotalPercent, -1);
		}

		// Token: 0x0600452C RID: 17708 RVA: 0x00271AA0 File Offset: 0x0026FCA0
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.CharId == base.CharacterId || dataKey.FieldId != 191 || !base.IsCurrent;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				EDamageType damageType = (EDamageType)dataKey.CustomParam1;
				bool flag2 = damageType != EDamageType.Direct;
				if (flag2)
				{
					result = 0;
				}
				else
				{
					result = 100;
				}
			}
			return result;
		}

		// Token: 0x0600452D RID: 17709 RVA: 0x00271AFC File Offset: 0x0026FCFC
		public override long GetModifiedValue(AffectedDataKey dataKey, long dataValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || dataKey.FieldId != 89 || dataValue <= 0L;
			long result;
			if (flag)
			{
				result = dataValue;
			}
			else
			{
				bool isInner = dataKey.CustomParam1 == 1;
				sbyte bodyPart = (sbyte)dataKey.CustomParam2;
				CombatCharacter enemyChar = base.CurrEnemyChar;
				int value = (int)Math.Clamp(dataValue, 0L, 2147483647L);
				DomainManager.Combat.AddFatalDamageValue(base.CombatChar.GetDataContext(), enemyChar, value, isInner ? 1 : 0, bodyPart, dataKey.CombatSkillId, EDamageType.None);
				base.ShowSpecialEffectTipsOnceInFrame(0);
				result = 0L;
			}
			return result;
		}

		// Token: 0x04001471 RID: 5233
		private const int FatalDamageTotalPercent = 100;
	}
}
