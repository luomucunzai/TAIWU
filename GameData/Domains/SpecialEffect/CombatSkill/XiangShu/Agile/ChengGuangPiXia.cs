using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Agile;

namespace GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Agile
{
	// Token: 0x02000336 RID: 822
	public class ChengGuangPiXia : AgileSkillBase
	{
		// Token: 0x06003497 RID: 13463 RVA: 0x0022927E File Offset: 0x0022747E
		public ChengGuangPiXia()
		{
		}

		// Token: 0x06003498 RID: 13464 RVA: 0x00229288 File Offset: 0x00227488
		public ChengGuangPiXia(CombatSkillKey skillKey) : base(skillKey, 16208)
		{
			this.ListenCanAffectChange = true;
		}

		// Token: 0x06003499 RID: 13465 RVA: 0x002292A0 File Offset: 0x002274A0
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 157, -1, -1, -1, -1), EDataModifyType.Custom);
			this._affecting = false;
			this.OnMoveSkillCanAffectChanged(context, default(DataUid));
		}

		// Token: 0x0600349A RID: 13466 RVA: 0x002292FA File Offset: 0x002274FA
		public override void OnDisable(DataContext context)
		{
			base.OnDisable(context);
			DomainManager.Combat.DisableJumpMove(context, base.CombatChar, base.SkillTemplateId);
		}

		// Token: 0x0600349B RID: 13467 RVA: 0x00229320 File Offset: 0x00227520
		protected override void OnMoveSkillCanAffectChanged(DataContext context, DataUid dataUid)
		{
			bool canAffect = base.CanAffect;
			bool flag = this._affecting == canAffect;
			if (!flag)
			{
				this._affecting = canAffect;
				bool flag2 = canAffect;
				if (flag2)
				{
					DomainManager.Combat.EnableJumpMove(base.CombatChar, base.SkillTemplateId);
				}
				else
				{
					DomainManager.Combat.DisableJumpMove(context, base.CombatChar, base.SkillTemplateId);
				}
			}
		}

		// Token: 0x0600349C RID: 13468 RVA: 0x00229384 File Offset: 0x00227584
		public override bool GetModifiedValue(AffectedDataKey dataKey, bool dataValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || !this._affecting;
			bool result;
			if (flag)
			{
				result = dataValue;
			}
			else
			{
				bool flag2 = dataKey.FieldId == 157;
				result = (!flag2 && dataValue);
			}
			return result;
		}

		// Token: 0x04000F80 RID: 3968
		private bool _affecting;
	}
}
