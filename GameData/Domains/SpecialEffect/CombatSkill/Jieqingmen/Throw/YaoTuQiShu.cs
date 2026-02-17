using System;
using System.Collections.Generic;
using System.Linq;
using Config;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Jieqingmen.Throw
{
	// Token: 0x020004E1 RID: 1249
	public class YaoTuQiShu : CombatSkillEffectBase
	{
		// Token: 0x06003DD2 RID: 15826 RVA: 0x00253883 File Offset: 0x00251A83
		public YaoTuQiShu()
		{
		}

		// Token: 0x06003DD3 RID: 15827 RVA: 0x0025388D File Offset: 0x00251A8D
		public YaoTuQiShu(CombatSkillKey skillKey) : base(skillKey, 13305, -1)
		{
		}

		// Token: 0x06003DD4 RID: 15828 RVA: 0x002538A0 File Offset: 0x00251AA0
		public override void OnEnable(DataContext context)
		{
			base.CreateAffectedData(319, EDataModifyType.Custom, base.SkillTemplateId);
			Events.RegisterHandler_ShaTrickInsteadCostTricks(new Events.OnShaTrickInsteadCostTricks(this.OnShaTrickInsteadCostTricks));
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
			Events.RegisterHandler_CastSkillTrickCosted(new Events.OnCastSkillTrickCosted(this.OnCastSkillTrickCosted));
		}

		// Token: 0x06003DD5 RID: 15829 RVA: 0x002538F7 File Offset: 0x00251AF7
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_ShaTrickInsteadCostTricks(new Events.OnShaTrickInsteadCostTricks(this.OnShaTrickInsteadCostTricks));
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
			Events.UnRegisterHandler_CastSkillTrickCosted(new Events.OnCastSkillTrickCosted(this.OnCastSkillTrickCosted));
		}

		// Token: 0x06003DD6 RID: 15830 RVA: 0x00253930 File Offset: 0x00251B30
		private void OnShaTrickInsteadCostTricks(DataContext context, CombatCharacter character, short skillId)
		{
			bool flag = this.SkillKey.IsMatch(character.GetId(), skillId);
			if (flag)
			{
				base.ShowSpecialEffectTips(0);
			}
		}

		// Token: 0x06003DD7 RID: 15831 RVA: 0x0025395C File Offset: 0x00251B5C
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId || !base.PowerMatchAffectRequire((int)power, 0);
			if (!flag)
			{
				base.AddMaxEffectCount(true);
				DomainManager.Combat.UpdateSkillCostTrickCanUse(context, base.CombatChar);
			}
		}

		// Token: 0x06003DD8 RID: 15832 RVA: 0x002539AC File Offset: 0x00251BAC
		private void OnCastSkillTrickCosted(DataContext context, CombatCharacter combatChar, short skillId, List<NeedTrick> costTricks)
		{
			bool flag = combatChar.GetId() != base.CharacterId || base.EffectCount <= 0;
			if (!flag)
			{
				int count = costTricks.Sum((NeedTrick x) => (int)((x.TrickType != 19) ? x.NeedCount : 0));
				bool flag2 = count <= 0;
				if (!flag2)
				{
					base.ReduceEffectCount(1);
					base.ShowSpecialEffectTips(1);
					DomainManager.Combat.AddTrick(context, base.IsDirect ? base.CombatChar : base.EnemyChar, 19, count, true, false);
				}
			}
		}

		// Token: 0x06003DD9 RID: 15833 RVA: 0x00253A48 File Offset: 0x00251C48
		public override bool GetModifiedValue(AffectedDataKey dataKey, bool dataValue)
		{
			bool flag = dataKey.SkillKey != this.SkillKey || dataKey.FieldId != 319;
			bool result;
			if (flag)
			{
				result = base.GetModifiedValue(dataKey, dataValue);
			}
			else
			{
				bool costSelfShaTrick = dataKey.CustomParam0 == 1;
				result = (costSelfShaTrick == base.IsDirect);
			}
			return result;
		}
	}
}
