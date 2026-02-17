using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Attack.XueFeng
{
	// Token: 0x020002D2 RID: 722
	public class XiongBingChuangSanZhen : CombatSkillEffectBase
	{
		// Token: 0x060032A9 RID: 12969 RVA: 0x002203EE File Offset: 0x0021E5EE
		public XiongBingChuangSanZhen()
		{
		}

		// Token: 0x060032AA RID: 12970 RVA: 0x002203F8 File Offset: 0x0021E5F8
		public XiongBingChuangSanZhen(CombatSkillKey skillKey) : base(skillKey, 17071, -1)
		{
		}

		// Token: 0x060032AB RID: 12971 RVA: 0x0022040C File Offset: 0x0021E60C
		public override void OnEnable(DataContext context)
		{
			DomainManager.Combat.AddMoveDistInSkillPrepare(base.CombatChar, 1000, true);
			DomainManager.Combat.AddMoveDistInSkillPrepare(base.CombatChar, 1000, false);
			base.CombatChar.CanNormalAttackInPrepareSkill = true;
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 9, -1, -1, -1, -1), EDataModifyType.TotalPercent);
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 14, -1, -1, -1, -1), EDataModifyType.TotalPercent);
			base.ShowSpecialEffectTips(0);
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x060032AC RID: 12972 RVA: 0x002204B5 File Offset: 0x0021E6B5
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x060032AD RID: 12973 RVA: 0x002204CC File Offset: 0x0021E6CC
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				base.CombatChar.CanNormalAttackInPrepareSkill = false;
				base.RemoveSelf(context);
			}
		}

		// Token: 0x060032AE RID: 12974 RVA: 0x00220510 File Offset: 0x0021E710
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			return 100;
		}

		// Token: 0x04000EFE RID: 3838
		private const sbyte AddAttribute = 100;
	}
}
