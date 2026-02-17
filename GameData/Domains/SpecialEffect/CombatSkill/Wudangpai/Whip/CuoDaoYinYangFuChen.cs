using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Wudangpai.Whip
{
	// Token: 0x020003B4 RID: 948
	public class CuoDaoYinYangFuChen : CombatSkillEffectBase
	{
		// Token: 0x060036FD RID: 14077 RVA: 0x00233297 File Offset: 0x00231497
		public CuoDaoYinYangFuChen()
		{
		}

		// Token: 0x060036FE RID: 14078 RVA: 0x002332A1 File Offset: 0x002314A1
		public CuoDaoYinYangFuChen(CombatSkillKey skillKey) : base(skillKey, 4304, -1)
		{
		}

		// Token: 0x060036FF RID: 14079 RVA: 0x002332B2 File Offset: 0x002314B2
		public override void OnEnable(DataContext context)
		{
			base.CreateAffectedAllEnemyData(286, EDataModifyType.Custom, -1);
			base.ShowSpecialEffectTips(0);
			Events.RegisterHandler_CompareDataCalcFinished(new Events.OnCompareDataCalcFinished(this.OnCompareDataCalcFinished));
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06003700 RID: 14080 RVA: 0x002332EF File Offset: 0x002314EF
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CompareDataCalcFinished(new Events.OnCompareDataCalcFinished(this.OnCompareDataCalcFinished));
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06003701 RID: 14081 RVA: 0x00233318 File Offset: 0x00231518
		private void OnCompareDataCalcFinished(CombatContext context, DamageCompareData compareData)
		{
			bool flag = context.Attacker != base.CombatChar || context.SkillTemplateId != base.SkillTemplateId;
			if (!flag)
			{
				bool isDirect = base.IsDirect;
				if (isDirect)
				{
					compareData.OuterDefendValue = compareData.InnerDefendValue;
				}
				else
				{
					compareData.InnerDefendValue = compareData.OuterDefendValue;
				}
			}
		}

		// Token: 0x06003702 RID: 14082 RVA: 0x00233374 File Offset: 0x00231574
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				foreach (short banableSkillId in base.CurrEnemyChar.GetBanableSkillIds(4, -1))
				{
					CombatSkill skill = DomainManager.CombatSkill.GetElement_CombatSkills(new ValueTuple<int, short>(base.CurrEnemyChar.GetId(), banableSkillId));
					bool flag2 = skill.GetDirection() != (base.IsDirect ? 0 : 1);
					if (!flag2)
					{
						DomainManager.Combat.SilenceSkill(context, base.CurrEnemyChar, banableSkillId, 180, 100);
						base.ShowSpecialEffectTipsOnceInFrame(1);
					}
				}
				base.RemoveSelf(context);
			}
		}

		// Token: 0x06003703 RID: 14083 RVA: 0x00233454 File Offset: 0x00231654
		public override bool GetModifiedValue(AffectedDataKey dataKey, bool dataValue)
		{
			bool flag = dataKey.FieldId != 286;
			bool result;
			if (flag)
			{
				result = dataValue;
			}
			else
			{
				CombatSkill skill;
				bool flag2 = !DomainManager.CombatSkill.TryGetElement_CombatSkills(dataKey.SkillKey, out skill);
				if (flag2)
				{
					result = dataValue;
				}
				else
				{
					bool flag3 = skill.GetDirection() != (base.IsDirect ? 0 : 1);
					result = (flag3 && dataValue);
				}
			}
			return result;
		}

		// Token: 0x0400100C RID: 4108
		private const int SilenceFrame = 180;
	}
}
