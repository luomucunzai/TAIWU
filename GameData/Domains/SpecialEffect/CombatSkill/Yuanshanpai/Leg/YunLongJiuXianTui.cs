using System;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Yuanshanpai.Leg
{
	// Token: 0x02000204 RID: 516
	public class YunLongJiuXianTui : CombatSkillEffectBase
	{
		// Token: 0x06002EAE RID: 11950 RVA: 0x00210496 File Offset: 0x0020E696
		public YunLongJiuXianTui()
		{
		}

		// Token: 0x06002EAF RID: 11951 RVA: 0x002104A0 File Offset: 0x0020E6A0
		public YunLongJiuXianTui(CombatSkillKey skillKey) : base(skillKey, 5106, -1)
		{
		}

		// Token: 0x06002EB0 RID: 11952 RVA: 0x002104B1 File Offset: 0x0020E6B1
		public override void OnEnable(DataContext context)
		{
			Events.RegisterHandler_PrepareSkillBegin(new Events.OnPrepareSkillBegin(this.OnPrepareSkillBegin));
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
			Events.RegisterHandler_DistanceChanged(new Events.OnDistanceChanged(this.OnDistanceChanged));
		}

		// Token: 0x06002EB1 RID: 11953 RVA: 0x002104EA File Offset: 0x0020E6EA
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_PrepareSkillBegin(new Events.OnPrepareSkillBegin(this.OnPrepareSkillBegin));
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
			Events.UnRegisterHandler_DistanceChanged(new Events.OnDistanceChanged(this.OnDistanceChanged));
		}

		// Token: 0x06002EB2 RID: 11954 RVA: 0x00210524 File Offset: 0x0020E724
		private void OnPrepareSkillBegin(DataContext context, int charId, bool isAlly, short skillId)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId || !base.CombatChar.GetAutoCastingSkill();
			if (!flag)
			{
				DomainManager.Combat.ChangeSkillPrepareProgress(base.CombatChar, base.CombatChar.SkillPrepareTotalProgress * 50 / 100);
			}
		}

		// Token: 0x06002EB3 RID: 11955 RVA: 0x00210580 File Offset: 0x0020E780
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				bool flag2 = base.PowerMatchAffectRequire((int)power, 0) && !base.CombatChar.GetAutoCastingSkill();
				if (flag2)
				{
					base.AddMaxEffectCount(true);
				}
			}
		}

		// Token: 0x06002EB4 RID: 11956 RVA: 0x002105D8 File Offset: 0x0020E7D8
		private void OnDistanceChanged(DataContext context, CombatCharacter mover, short distance, bool isMove, bool isForced)
		{
			bool flag = mover.GetId() != base.CharacterId || base.EffectCount <= 0 || !isMove || isForced || (base.IsDirect ? (distance > 0) : (distance < 0)) || base.CombatChar.GetPreparingSkillId() >= 0;
			if (!flag)
			{
				this._distanceAccumulator += (int)Math.Abs(distance);
				bool flag2 = this._distanceAccumulator >= 2;
				if (flag2)
				{
					this._distanceAccumulator = 0;
					base.ReduceEffectCount(1);
					int costMobility = MoveSpecialConstants.MaxMobility * 10 / 100;
					bool flag3 = (base.CombatChar.GetMobilityValue() >= costMobility || base.SkillInstance.GetCostMobilityPercent() == 0) && DomainManager.Combat.CanCastSkill(base.CombatChar, base.SkillTemplateId, true, true);
					if (flag3)
					{
						bool flag4 = base.SkillInstance.GetCostMobilityPercent() > 0;
						if (flag4)
						{
							base.ChangeMobilityValue(context, base.CombatChar, -costMobility);
						}
						DomainManager.Combat.CastSkillFree(context, base.CombatChar, base.SkillTemplateId, ECombatCastFreePriority.Normal);
						base.ShowSpecialEffectTips(0);
					}
				}
			}
		}

		// Token: 0x04000DE6 RID: 3558
		private const sbyte AutoCastDistance = 2;

		// Token: 0x04000DE7 RID: 3559
		private const sbyte AutoCastCostMobility = 10;

		// Token: 0x04000DE8 RID: 3560
		private const sbyte PrepareProgressPercent = 50;

		// Token: 0x04000DE9 RID: 3561
		private int _distanceAccumulator;
	}
}
