using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.Character;
using GameData.Domains.CombatSkill;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Neigong.Boss
{
	// Token: 0x020002A6 RID: 678
	public class WanXiangHeShu : BossNeigongBase
	{
		// Token: 0x170001CB RID: 459
		// (get) Token: 0x060031CA RID: 12746 RVA: 0x0021C374 File Offset: 0x0021A574
		private static bool CanAffect
		{
			get
			{
				return !DomainManager.Extra.IsProfessionalSkillUnlockedAndEquipped(41);
			}
		}

		// Token: 0x060031CB RID: 12747 RVA: 0x0021C385 File Offset: 0x0021A585
		public WanXiangHeShu()
		{
		}

		// Token: 0x060031CC RID: 12748 RVA: 0x0021C38F File Offset: 0x0021A58F
		public WanXiangHeShu(CombatSkillKey skillKey) : base(skillKey, 16109)
		{
		}

		// Token: 0x060031CD RID: 12749 RVA: 0x0021C39F File Offset: 0x0021A59F
		protected override void ActivePhase2Effect(DataContext context)
		{
			base.AppendAffectedData(context, base.CharacterId, 69, EDataModifyType.AddPercent, -1);
			base.AppendAffectedData(context, base.CharacterId, 102, EDataModifyType.AddPercent, -1);
		}

		// Token: 0x060031CE RID: 12750 RVA: 0x0021C3C8 File Offset: 0x0021A5C8
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || !WanXiangHeShu.CanAffect;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				int damagePercent = 0;
				int samsaraCharId = base.CurrEnemyChar.GetId();
				List<short> featureIds = DomainManager.Character.GetElement_Objects(samsaraCharId).GetFeatureIds();
				int samsaraCount = featureIds.Exists(new Predicate<short>(Character.IsReincarnationBonusFeature)) ? 9 : DomainManager.Character.GetCharacterSamsaraData(samsaraCharId).DeadCharacters.Count;
				int randomCount = 9 - samsaraCount;
				DataContext context = DomainManager.Combat.Context;
				for (int i = 0; i < randomCount; i++)
				{
					bool flag2 = context.Random.CheckPercentProb(80);
					if (flag2)
					{
						damagePercent += ((dataKey.FieldId == 69) ? 30 : -15);
					}
				}
				bool flag3 = featureIds.Exists(new Predicate<short>(Character.IsProfessionReincarnationBonusFeature));
				if (flag3)
				{
					damagePercent /= 2;
				}
				bool flag4 = damagePercent != 0;
				if (flag4)
				{
					base.ShowSpecialEffectTips(dataKey.CharId == base.CharacterId, 2, 1);
				}
				result = damagePercent;
			}
			return result;
		}

		// Token: 0x04000EC2 RID: 3778
		private const sbyte AddDamagePercent = 30;

		// Token: 0x04000EC3 RID: 3779
		private const sbyte ReduceDamagePercent = -15;

		// Token: 0x04000EC4 RID: 3780
		private const sbyte ChangeInjuryOdds = 80;
	}
}
