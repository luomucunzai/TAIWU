using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Xuannvpai.Music
{
	// Token: 0x0200026A RID: 618
	public class HouRenXiYi : CombatSkillEffectBase
	{
		// Token: 0x06003072 RID: 12402 RVA: 0x0021717B File Offset: 0x0021537B
		public HouRenXiYi()
		{
		}

		// Token: 0x06003073 RID: 12403 RVA: 0x00217195 File Offset: 0x00215395
		public HouRenXiYi(CombatSkillKey skillKey) : base(skillKey, 8307, -1)
		{
		}

		// Token: 0x06003074 RID: 12404 RVA: 0x002171B8 File Offset: 0x002153B8
		public override void OnEnable(DataContext context)
		{
			int adoreCharId = base.IsDirect ? base.CurrEnemyChar.GetId() : base.CharacterId;
			HashSet<int> adoreChars = DomainManager.Character.GetRelatedCharIds(adoreCharId, 16384);
			int powerUpCharCount = 0;
			foreach (int charId in adoreChars)
			{
				bool flag = DomainManager.Character.GetRelatedCharIds(charId, 16384).Contains(adoreCharId);
				if (flag)
				{
					powerUpCharCount++;
				}
			}
			this._powerChangeValue = ((powerUpCharCount <= (int)this.MaxCharCount) ? ((int)this.PowerChangeUnit * powerUpCharCount) : ((int)(-(int)this.PowerChangeUnit) * (powerUpCharCount - (int)this.MaxCharCount)));
			bool flag2 = this._powerChangeValue != 0;
			if (flag2)
			{
				this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
				this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 199, base.SkillTemplateId, -1, -1, -1), EDataModifyType.AddPercent);
				base.ShowSpecialEffectTips(this._powerChangeValue > 0, 0, 1);
			}
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06003075 RID: 12405 RVA: 0x002172E4 File Offset: 0x002154E4
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06003076 RID: 12406 RVA: 0x002172FC File Offset: 0x002154FC
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				base.RemoveSelf(context);
			}
		}

		// Token: 0x06003077 RID: 12407 RVA: 0x00217334 File Offset: 0x00215534
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || dataKey.CombatSkillId != base.SkillTemplateId;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				bool flag2 = dataKey.FieldId == 199;
				if (flag2)
				{
					result = this._powerChangeValue;
				}
				else
				{
					result = 0;
				}
			}
			return result;
		}

		// Token: 0x04000E5E RID: 3678
		private sbyte MaxCharCount = 10;

		// Token: 0x04000E5F RID: 3679
		private sbyte PowerChangeUnit = 12;

		// Token: 0x04000E60 RID: 3680
		private int _powerChangeValue;
	}
}
