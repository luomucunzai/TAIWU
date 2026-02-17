using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Agile;

namespace GameData.Domains.SpecialEffect.CombatSkill.Fulongtan.Agile
{
	// Token: 0x02000532 RID: 1330
	public class ChiJiaoGong : AgileSkillBase
	{
		// Token: 0x06003F96 RID: 16278 RVA: 0x0025A97C File Offset: 0x00258B7C
		public ChiJiaoGong()
		{
		}

		// Token: 0x06003F97 RID: 16279 RVA: 0x0025A986 File Offset: 0x00258B86
		public ChiJiaoGong(CombatSkillKey skillKey) : base(skillKey, 14400)
		{
		}

		// Token: 0x06003F98 RID: 16280 RVA: 0x0025A996 File Offset: 0x00258B96
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 148, -1, -1, -1, -1), EDataModifyType.Custom);
			base.ShowSpecialEffectTips(0);
		}

		// Token: 0x06003F99 RID: 16281 RVA: 0x0025A9D5 File Offset: 0x00258BD5
		public override void OnDataAdded(DataContext context)
		{
			base.OnDataAdded(context);
			base.AppendAffectedAllEnemyData(context, 175, EDataModifyType.AddPercent, -1);
		}

		// Token: 0x06003F9A RID: 16282 RVA: 0x0025A9F0 File Offset: 0x00258BF0
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.CharId == base.CharacterId;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				bool flag2 = dataKey.FieldId == 175;
				if (flag2)
				{
					CombatCharacter moveChar = DomainManager.Combat.GetElement_CombatCharacterDict(dataKey.CharId);
					bool flag3 = base.IsDirect ? moveChar.MoveForward : (!moveChar.MoveForward);
					if (flag3)
					{
						result = 0;
					}
					else
					{
						result = 50;
					}
				}
				else
				{
					result = 0;
				}
			}
			return result;
		}

		// Token: 0x06003F9B RID: 16283 RVA: 0x0025AA64 File Offset: 0x00258C64
		public override bool GetModifiedValue(AffectedDataKey dataKey, bool dataValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || !base.CanAffect;
			bool result;
			if (flag)
			{
				result = dataValue;
			}
			else
			{
				bool flag2 = dataKey.FieldId == 148;
				if (flag2)
				{
					CValueDistanceDelta addDistance = dataKey.CustomParam0;
					bool flag3 = base.IsDirect ? addDistance.IsForward : addDistance.IsBackward;
					result = (flag3 && dataValue);
				}
				else
				{
					result = dataValue;
				}
			}
			return result;
		}

		// Token: 0x040012BB RID: 4795
		private const int ChangeMoveCostMobilityPercent = 50;
	}
}
