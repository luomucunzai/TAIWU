using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Yuanshanpai.Leg
{
	// Token: 0x02000203 RID: 515
	public class YuanYangLianHuanJiao : CombatSkillEffectBase
	{
		// Token: 0x06002EA8 RID: 11944 RVA: 0x002102E5 File Offset: 0x0020E4E5
		public YuanYangLianHuanJiao()
		{
		}

		// Token: 0x06002EA9 RID: 11945 RVA: 0x002102EF File Offset: 0x0020E4EF
		public YuanYangLianHuanJiao(CombatSkillKey skillKey) : base(skillKey, 5101, -1)
		{
		}

		// Token: 0x06002EAA RID: 11946 RVA: 0x00210300 File Offset: 0x0020E500
		public override void OnEnable(DataContext context)
		{
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, base.IsDirect ? 206 : 205, base.SkillTemplateId, -1, -1, -1), EDataModifyType.TotalPercent);
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 207, base.SkillTemplateId, -1, -1, -1), EDataModifyType.TotalPercent);
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06002EAB RID: 11947 RVA: 0x00210386 File Offset: 0x0020E586
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06002EAC RID: 11948 RVA: 0x0021039C File Offset: 0x0020E59C
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId || !base.PowerMatchAffectRequire((int)power, 0) || this._reduceCost == -75;
			if (!flag)
			{
				this._reduceCost += -25;
				DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, base.IsDirect ? 206 : 205);
				DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 207);
				base.ShowSpecialEffectTips(0);
			}
		}

		// Token: 0x06002EAD RID: 11949 RVA: 0x00210430 File Offset: 0x0020E630
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
				ushort fieldId = dataKey.FieldId;
				bool flag2 = fieldId - 205 <= 2;
				bool flag3 = flag2;
				if (flag3)
				{
					result = this._reduceCost;
				}
				else
				{
					result = 0;
				}
			}
			return result;
		}

		// Token: 0x04000DE3 RID: 3555
		private const int ReduceCostUnit = -25;

		// Token: 0x04000DE4 RID: 3556
		private const int MaxReduceCost = -75;

		// Token: 0x04000DE5 RID: 3557
		private int _reduceCost;
	}
}
