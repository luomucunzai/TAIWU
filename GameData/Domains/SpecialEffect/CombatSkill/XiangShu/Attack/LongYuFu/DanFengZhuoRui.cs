using System;
using System.Runtime.CompilerServices;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Attack.LongYuFu
{
	// Token: 0x020002FB RID: 763
	public class DanFengZhuoRui : CombatSkillEffectBase
	{
		// Token: 0x06003399 RID: 13209 RVA: 0x00225C16 File Offset: 0x00223E16
		public DanFengZhuoRui()
		{
		}

		// Token: 0x0600339A RID: 13210 RVA: 0x00225C20 File Offset: 0x00223E20
		public DanFengZhuoRui(CombatSkillKey skillKey) : base(skillKey, 17120, -1)
		{
		}

		// Token: 0x0600339B RID: 13211 RVA: 0x00225C31 File Offset: 0x00223E31
		public override void OnEnable(DataContext context)
		{
			Events.RegisterHandler_CostBreathAndStance(new Events.OnCostBreathAndStance(this.OnCostBreathAndStance));
			Events.RegisterHandler_PrepareSkillBegin(new Events.OnPrepareSkillBegin(this.OnPrepareSkillBegin));
		}

		// Token: 0x0600339C RID: 13212 RVA: 0x00225C58 File Offset: 0x00223E58
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CostBreathAndStance(new Events.OnCostBreathAndStance(this.OnCostBreathAndStance));
			Events.UnRegisterHandler_PrepareSkillBegin(new Events.OnPrepareSkillBegin(this.OnPrepareSkillBegin));
		}

		// Token: 0x0600339D RID: 13213 RVA: 0x00225C80 File Offset: 0x00223E80
		private void OnCostBreathAndStance(DataContext context, int charId, bool isAlly, int costBreath, int costStance, short skillId)
		{
			bool flag = base.CharacterId != charId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				this._costBreathStance.Item1 = costBreath;
				this._costBreathStance.Item2 = costStance;
			}
		}

		// Token: 0x0600339E RID: 13214 RVA: 0x00225CC8 File Offset: 0x00223EC8
		private void OnPrepareSkillBegin(DataContext context, int charId, bool isAlly, short skillId)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				Injuries injuries = base.CombatChar.GetInjuries();
				int addedInjury = 0;
				bool flag2 = injuries.Get(3, false) < 4;
				if (flag2)
				{
					DomainManager.Combat.AddInjury(context, base.CombatChar, 3, false, 1, true, false);
					addedInjury++;
				}
				bool flag3 = injuries.Get(4, false) < 4;
				if (flag3)
				{
					DomainManager.Combat.AddInjury(context, base.CombatChar, 4, false, 1, true, false);
					addedInjury++;
				}
				bool flag4 = addedInjury <= 0;
				if (!flag4)
				{
					DomainManager.Combat.AddToCheckFallenSet(base.CombatChar.GetId());
					DomainManager.Combat.ChangeSkillPrepareProgress(base.CombatChar, base.CombatChar.SkillPrepareTotalProgress * 25 * addedInjury / 100);
					base.ChangeBreathValue(context, base.CombatChar, this._costBreathStance.Item1 * 25 * addedInjury / 100);
					base.ChangeStanceValue(context, base.CombatChar, this._costBreathStance.Item2 * 25 * addedInjury / 100);
					base.ShowSpecialEffectTips(0);
				}
			}
		}

		// Token: 0x04000F40 RID: 3904
		private const sbyte InjuryThreshold = 4;

		// Token: 0x04000F41 RID: 3905
		private const sbyte AddPrepareProgressUnit = 25;

		// Token: 0x04000F42 RID: 3906
		private const sbyte RecoverBreathStanceUnit = 25;

		// Token: 0x04000F43 RID: 3907
		[TupleElementNames(new string[]
		{
			"breath",
			"stance"
		})]
		private ValueTuple<int, int> _costBreathStance;
	}
}
