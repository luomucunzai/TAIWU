using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Agile;

namespace GameData.Domains.SpecialEffect.CombatSkill.Jieqingmen.Agile
{
	// Token: 0x02000504 RID: 1284
	public class HunTianYiXingGong : AgileSkillBase
	{
		// Token: 0x06003E97 RID: 16023 RVA: 0x00256735 File Offset: 0x00254935
		public HunTianYiXingGong()
		{
		}

		// Token: 0x06003E98 RID: 16024 RVA: 0x0025673F File Offset: 0x0025493F
		public HunTianYiXingGong(CombatSkillKey skillKey) : base(skillKey, 13408)
		{
		}

		// Token: 0x06003E99 RID: 16025 RVA: 0x00256750 File Offset: 0x00254950
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			base.AppendAffectedData(context, base.CharacterId, 157, EDataModifyType.Custom, -1);
			sbyte targetDistance = base.IsDirect ? 90 : 50;
			bool flag = (short)targetDistance != DomainManager.Combat.GetCurrentDistance();
			if (flag)
			{
				DomainManager.Combat.ChangeDistance(context, base.CombatChar, (int)((short)targetDistance - DomainManager.Combat.GetCurrentDistance()));
			}
			base.ClearAffectedData(context);
		}

		// Token: 0x06003E9A RID: 16026 RVA: 0x002567C4 File Offset: 0x002549C4
		public override void OnDataAdded(DataContext context)
		{
			base.AppendAffectedAllEnemyData(context, 151, EDataModifyType.Custom, -1);
			base.ShowSpecialEffectTips(0);
		}

		// Token: 0x06003E9B RID: 16027 RVA: 0x002567E0 File Offset: 0x002549E0
		public override bool GetModifiedValue(AffectedDataKey dataKey, bool dataValue)
		{
			bool flag = dataKey.FieldId == 157;
			return !flag && dataValue;
		}

		// Token: 0x06003E9C RID: 16028 RVA: 0x00256808 File Offset: 0x00254A08
		public override int GetModifiedValue(AffectedDataKey dataKey, int dataValue)
		{
			bool flag = base.CombatChar.GetAffectingMoveSkillId() != base.SkillTemplateId;
			int result;
			if (flag)
			{
				result = dataValue;
			}
			else
			{
				bool flag2 = dataKey.FieldId == 151 && dataKey.CustomParam0 == (base.IsDirect ? 1 : 0);
				if (flag2)
				{
					result = 0;
				}
				else
				{
					result = dataValue;
				}
			}
			return result;
		}

		// Token: 0x04001278 RID: 4728
		private const sbyte DirectDistance = 90;

		// Token: 0x04001279 RID: 4729
		private const sbyte ReverseDistance = 50;
	}
}
