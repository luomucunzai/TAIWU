using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Attack;

namespace GameData.Domains.SpecialEffect.CombatSkill.Xuehoujiao.Finger
{
	// Token: 0x02000239 RID: 569
	public class DuLongZhua : ChangePoisonLevel
	{
		// Token: 0x170001B9 RID: 441
		// (get) Token: 0x06002F99 RID: 12185 RVA: 0x00213AAB File Offset: 0x00211CAB
		protected override sbyte AffectPoisonType
		{
			get
			{
				return 3;
			}
		}

		// Token: 0x06002F9A RID: 12186 RVA: 0x00213AAE File Offset: 0x00211CAE
		public DuLongZhua()
		{
		}

		// Token: 0x06002F9B RID: 12187 RVA: 0x00213AB8 File Offset: 0x00211CB8
		public DuLongZhua(CombatSkillKey skillKey) : base(skillKey, 15204)
		{
		}

		// Token: 0x06002F9C RID: 12188 RVA: 0x00213AC8 File Offset: 0x00211CC8
		protected override void OnAffecting(DataContext context)
		{
			int charId = this.AffectingSkillKey.CharId;
			base.ShowSpecialEffectTips(1);
			bool flag = !this.AffectDatas.ContainsKey(new AffectedDataKey(charId, 194, -1, -1, -1, -1));
			if (flag)
			{
				base.AppendAffectedData(context, charId, 194, EDataModifyType.TotalPercent, -1);
			}
			else
			{
				DomainManager.SpecialEffect.InvalidateCache(context, charId, 194);
			}
		}

		// Token: 0x06002F9D RID: 12189 RVA: 0x00213B30 File Offset: 0x00211D30
		protected override int AffectingGetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.FieldId == 194;
			if (flag)
			{
				bool flag2 = base.IsDirect && dataKey.CharId == base.CharacterId;
				if (flag2)
				{
					return 50;
				}
				bool flag3 = !base.IsDirect && dataKey.CharId == base.CurrEnemyChar.GetId();
				if (flag3)
				{
					return -50;
				}
			}
			return 0;
		}

		// Token: 0x04000E1B RID: 3611
		private const sbyte ReduceSkillPrepareSpeedPercent = 50;
	}
}
