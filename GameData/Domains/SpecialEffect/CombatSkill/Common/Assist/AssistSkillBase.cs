using System;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.GameDataBridge;

namespace GameData.Domains.SpecialEffect.CombatSkill.Common.Assist
{
	// Token: 0x020005AC RID: 1452
	public class AssistSkillBase : CombatSkillEffectBase
	{
		// Token: 0x170002A8 RID: 680
		// (get) Token: 0x0600432D RID: 17197 RVA: 0x0026A634 File Offset: 0x00268834
		protected bool CanAffect
		{
			get
			{
				CombatSkillData combatSkillData;
				return DomainManager.Combat.TryGetCombatSkillData(base.CharacterId, base.SkillTemplateId, out combatSkillData) && combatSkillData.GetLeftCdFrame() == 0 && combatSkillData.GetCanAffect();
			}
		}

		// Token: 0x0600432E RID: 17198 RVA: 0x0026A66C File Offset: 0x0026886C
		protected AssistSkillBase()
		{
		}

		// Token: 0x0600432F RID: 17199 RVA: 0x0026A676 File Offset: 0x00268876
		protected AssistSkillBase(CombatSkillKey skillKey, int type) : base(skillKey, type, -1)
		{
		}

		// Token: 0x06004330 RID: 17200 RVA: 0x0026A684 File Offset: 0x00268884
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			this._skillCanUseUid = base.ParseCombatSkillDataUid(1);
			this._skillCanAffectUid = base.ParseCombatSkillDataUid(9);
			GameDataBridge.AddPostDataModificationHandler(this._skillCanUseUid, base.DataHandlerKey, new Action<DataContext, DataUid>(this.OnCanUseChanged));
			GameDataBridge.AddPostDataModificationHandler(this._skillCanAffectUid, base.DataHandlerKey, new Action<DataContext, DataUid>(this.OnCanUseChanged));
			bool setConstAffectingOnCombatBegin = this.SetConstAffectingOnCombatBegin;
			if (setConstAffectingOnCombatBegin)
			{
				Events.RegisterHandler_CombatBegin(new Events.OnCombatBegin(this.OnCombatBegin));
			}
		}

		// Token: 0x06004331 RID: 17201 RVA: 0x0026A70F File Offset: 0x0026890F
		public override void OnDisable(DataContext context)
		{
			GameDataBridge.RemovePostDataModificationHandler(this._skillCanUseUid, base.DataHandlerKey);
			GameDataBridge.RemovePostDataModificationHandler(this._skillCanAffectUid, base.DataHandlerKey);
			base.OnDisable(context);
		}

		// Token: 0x06004332 RID: 17202 RVA: 0x0026A73E File Offset: 0x0026893E
		protected virtual void OnCanUseChanged(DataContext context, DataUid dataUid)
		{
		}

		// Token: 0x06004333 RID: 17203 RVA: 0x0026A741 File Offset: 0x00268941
		private void OnCombatBegin(DataContext context)
		{
			this.SetConstAffecting(context, true);
			Events.UnRegisterHandler_CombatBegin(new Events.OnCombatBegin(this.OnCombatBegin));
		}

		// Token: 0x06004334 RID: 17204 RVA: 0x0026A75F File Offset: 0x0026895F
		protected void SetConstAffecting(DataContext context, bool affecting)
		{
			DomainManager.Combat.GetCombatSkillData(base.CharacterId, base.SkillTemplateId).SetConstAffecting(affecting, context);
		}

		// Token: 0x06004335 RID: 17205 RVA: 0x0026A780 File Offset: 0x00268980
		protected void ShowEffectTips(DataContext context)
		{
			DomainManager.Combat.GetCombatSkillData(base.CharacterId, base.SkillTemplateId).SetShowAffectTips(true, context);
		}

		// Token: 0x040013EC RID: 5100
		protected bool SetConstAffectingOnCombatBegin;

		// Token: 0x040013ED RID: 5101
		private DataUid _skillCanUseUid;

		// Token: 0x040013EE RID: 5102
		private DataUid _skillCanAffectUid;
	}
}
