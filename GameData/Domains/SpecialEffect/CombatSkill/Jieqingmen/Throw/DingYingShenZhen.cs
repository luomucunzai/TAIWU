using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Jieqingmen.Throw
{
	// Token: 0x020004DA RID: 1242
	public class DingYingShenZhen : CombatSkillEffectBase
	{
		// Token: 0x06003D99 RID: 15769 RVA: 0x0025278F File Offset: 0x0025098F
		public DingYingShenZhen()
		{
		}

		// Token: 0x06003D9A RID: 15770 RVA: 0x00252799 File Offset: 0x00250999
		public DingYingShenZhen(CombatSkillKey skillKey) : base(skillKey, 13307, -1)
		{
		}

		// Token: 0x06003D9B RID: 15771 RVA: 0x002527AA File Offset: 0x002509AA
		public override void OnEnable(DataContext context)
		{
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
			Events.RegisterHandler_GetShaTrick(new Events.OnGetShaTrick(this.OnGetShaTrick));
		}

		// Token: 0x06003D9C RID: 15772 RVA: 0x002527D1 File Offset: 0x002509D1
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
			Events.UnRegisterHandler_GetShaTrick(new Events.OnGetShaTrick(this.OnGetShaTrick));
		}

		// Token: 0x06003D9D RID: 15773 RVA: 0x002527F8 File Offset: 0x002509F8
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				bool flag2 = base.PowerMatchAffectRequire((int)power, 0);
				if (flag2)
				{
					base.AddMaxEffectCount(true);
				}
			}
		}

		// Token: 0x06003D9E RID: 15774 RVA: 0x0025283C File Offset: 0x00250A3C
		private void OnGetShaTrick(DataContext context, int charId, bool isAlly, bool real)
		{
			bool flag = !base.IsCurrent || base.EffectCount <= 0;
			if (!flag)
			{
				bool flag2 = base.IsDirect ? (charId != base.CharacterId) : (base.CombatChar.IsAlly == isAlly);
				if (!flag2)
				{
					base.ReduceEffectCount(1);
					int reduceValue = GlobalConfig.Instance.MaxMobility * DingYingShenZhen.ReduceMobilityPercent;
					base.ChangeMobilityValue(context, base.EnemyChar, -reduceValue);
					base.ShowSpecialEffectTips(0);
					bool flag3 = base.EnemyChar.GetMobilityValue() > 0;
					if (!flag3)
					{
						base.ShowSpecialEffectTips(1);
						for (int i = 0; i < 3; i++)
						{
							DomainManager.Combat.AddAcupoint(context, base.EnemyChar, 3, this.SkillKey, -1, 1, true);
						}
					}
				}
			}
		}

		// Token: 0x04001225 RID: 4645
		private const sbyte AcupointLevel = 3;

		// Token: 0x04001226 RID: 4646
		private const sbyte AcupointCount = 3;

		// Token: 0x04001227 RID: 4647
		private static readonly CValuePercent ReduceMobilityPercent = 50;
	}
}
