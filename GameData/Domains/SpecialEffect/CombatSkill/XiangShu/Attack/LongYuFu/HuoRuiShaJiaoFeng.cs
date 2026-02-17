using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Attack.LongYuFu
{
	// Token: 0x020002FE RID: 766
	public class HuoRuiShaJiaoFeng : CombatSkillEffectBase
	{
		// Token: 0x060033AB RID: 13227 RVA: 0x002262E5 File Offset: 0x002244E5
		public HuoRuiShaJiaoFeng()
		{
		}

		// Token: 0x060033AC RID: 13228 RVA: 0x002262EF File Offset: 0x002244EF
		public HuoRuiShaJiaoFeng(CombatSkillKey skillKey) : base(skillKey, 17123, -1)
		{
		}

		// Token: 0x060033AD RID: 13229 RVA: 0x00226300 File Offset: 0x00224500
		public override void OnEnable(DataContext context)
		{
			int[] enemyList = DomainManager.Combat.GetCharacterList(!base.CombatChar.IsAlly);
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 219, base.SkillTemplateId, -1, -1, -1), EDataModifyType.Custom);
			for (int i = 0; i < enemyList.Length; i++)
			{
				bool flag = enemyList[i] >= 0;
				if (flag)
				{
					this.AffectDatas.Add(new AffectedDataKey(enemyList[i], 169, -1, -1, -1, -1), EDataModifyType.Custom);
				}
			}
			Events.RegisterHandler_CostBreathAndStance(new Events.OnCostBreathAndStance(this.OnCostBreathAndStance));
			Events.RegisterHandler_PrepareSkillBegin(new Events.OnPrepareSkillBegin(this.OnPrepareSkillBegin));
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x060033AE RID: 13230 RVA: 0x002263CD File Offset: 0x002245CD
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CostBreathAndStance(new Events.OnCostBreathAndStance(this.OnCostBreathAndStance));
			Events.UnRegisterHandler_PrepareSkillBegin(new Events.OnPrepareSkillBegin(this.OnPrepareSkillBegin));
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x060033AF RID: 13231 RVA: 0x00226408 File Offset: 0x00224608
		private void OnCostBreathAndStance(DataContext context, int charId, bool isAlly, int costBreath, int costStance, short skillId)
		{
			bool flag = base.CharacterId != charId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				this._costBreathStance.Item1 = costBreath;
				this._costBreathStance.Item2 = costStance;
			}
		}

		// Token: 0x060033B0 RID: 13232 RVA: 0x00226450 File Offset: 0x00224650
		private void OnPrepareSkillBegin(DataContext context, int charId, bool isAlly, short skillId)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				DomainManager.Combat.UpdateSkillCanUse(context, DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly, false));
				Injuries injuries = base.CombatChar.GetInjuries();
				bool canAffect = injuries.Get(3, false) >= 4 || injuries.Get(4, false) >= 4;
				bool flag2 = !canAffect;
				if (!flag2)
				{
					DomainManager.Combat.ChangeSkillPrepareProgress(base.CombatChar, base.CombatChar.SkillPrepareTotalProgress);
					base.ChangeBreathValue(context, base.CombatChar, this._costBreathStance.Item1);
					base.ChangeStanceValue(context, base.CombatChar, this._costBreathStance.Item2);
					base.ShowSpecialEffectTips(0);
				}
			}
		}

		// Token: 0x060033B1 RID: 13233 RVA: 0x00226530 File Offset: 0x00224730
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				DomainManager.Combat.UpdateSkillCanUse(context, DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly, false));
			}
		}

		// Token: 0x060033B2 RID: 13234 RVA: 0x00226584 File Offset: 0x00224784
		public override bool GetModifiedValue(AffectedDataKey dataKey, bool dataValue)
		{
			bool flag = dataKey.FieldId == 219;
			bool result;
			if (flag)
			{
				result = true;
			}
			else
			{
				bool flag2 = dataKey.FieldId == 169 && !dataValue && (dataKey.CustomParam0 == 3 || dataKey.CustomParam0 == 4) && (base.CombatChar.GetPreparingSkillId() == base.SkillTemplateId || base.CombatChar.GetPerformingSkillId() == base.SkillTemplateId);
				result = (flag2 || dataValue);
			}
			return result;
		}

		// Token: 0x04000F48 RID: 3912
		private const sbyte RequireInjury = 4;

		// Token: 0x04000F49 RID: 3913
		[TupleElementNames(new string[]
		{
			"breath",
			"stance"
		})]
		private ValueTuple<int, int> _costBreathStance;
	}
}
