using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Attack;

namespace GameData.Domains.SpecialEffect.CombatSkill.Jingangzong.Blade
{
	// Token: 0x020004D1 RID: 1233
	public class LiuShenWeiDao : BuffByNeiliAllocation
	{
		// Token: 0x1700023B RID: 571
		// (get) Token: 0x06003D6C RID: 15724 RVA: 0x00251AE9 File Offset: 0x0024FCE9
		protected override bool ShowTipsOnAffecting
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06003D6D RID: 15725 RVA: 0x00251AEC File Offset: 0x0024FCEC
		public LiuShenWeiDao()
		{
		}

		// Token: 0x06003D6E RID: 15726 RVA: 0x00251AF6 File Offset: 0x0024FCF6
		public LiuShenWeiDao(CombatSkillKey skillKey) : base(skillKey, 11207)
		{
			this.RequireNeiliAllocationType = 2;
		}

		// Token: 0x06003D6F RID: 15727 RVA: 0x00251B10 File Offset: 0x0024FD10
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 69, -1, -1, -1, -1), EDataModifyType.AddPercent);
			Events.RegisterHandler_PrepareSkillEnd(new Events.OnPrepareSkillEnd(this.OnPrepareSkillEnd));
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06003D70 RID: 15728 RVA: 0x00251B68 File Offset: 0x0024FD68
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_PrepareSkillEnd(new Events.OnPrepareSkillEnd(this.OnPrepareSkillEnd));
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
			base.OnDisable(context);
		}

		// Token: 0x06003D71 RID: 15729 RVA: 0x00251B98 File Offset: 0x0024FD98
		private unsafe void OnPrepareSkillEnd(DataContext context, int charId, bool isAlly, short skillId)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId || !base.Affecting;
			if (!flag)
			{
				short selfNeiliAllocation = *base.CombatChar.GetNeiliAllocation()[(int)this.RequireNeiliAllocationType];
				short enemyNeiliAllocation = *base.CurrEnemyChar.GetNeiliAllocation()[(int)this.RequireNeiliAllocationType];
				this._directDamageAddPercent = (int)(base.IsDirect ? (selfNeiliAllocation - enemyNeiliAllocation) : (enemyNeiliAllocation - selfNeiliAllocation));
				base.ShowSpecialEffectTips(0);
			}
		}

		// Token: 0x06003D72 RID: 15730 RVA: 0x00251C20 File Offset: 0x0024FE20
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				this._directDamageAddPercent = 0;
			}
		}

		// Token: 0x06003D73 RID: 15731 RVA: 0x00251C54 File Offset: 0x0024FE54
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.CharId == base.CharacterId && dataKey.CombatSkillId == base.SkillTemplateId && dataKey.FieldId == 69;
			int result;
			if (flag)
			{
				result = this._directDamageAddPercent;
			}
			else
			{
				result = base.GetModifyValue(dataKey, currModifyValue);
			}
			return result;
		}

		// Token: 0x0400121A RID: 4634
		private int _directDamageAddPercent;
	}
}
