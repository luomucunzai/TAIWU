using System;
using GameData.Common;
using GameData.Domains.CombatSkill;
using GameData.GameDataBridge;

namespace GameData.Domains.SpecialEffect.CombatSkill.Common.Defense
{
	// Token: 0x02000583 RID: 1411
	public class DefenseSkillBase : CombatSkillEffectBase
	{
		// Token: 0x17000283 RID: 643
		// (get) Token: 0x060041C6 RID: 16838 RVA: 0x00264078 File Offset: 0x00262278
		protected bool CanAffect
		{
			get
			{
				return base.SkillData.GetCanAffect() && base.CombatChar.GetAffectingDefendSkillId() == base.SkillTemplateId;
			}
		}

		// Token: 0x060041C7 RID: 16839 RVA: 0x0026409D File Offset: 0x0026229D
		protected DefenseSkillBase()
		{
		}

		// Token: 0x060041C8 RID: 16840 RVA: 0x002640B5 File Offset: 0x002622B5
		protected DefenseSkillBase(CombatSkillKey skillKey, int type) : base(skillKey, type, -1)
		{
		}

		// Token: 0x060041C9 RID: 16841 RVA: 0x002640D0 File Offset: 0x002622D0
		public override void OnEnable(DataContext context)
		{
			this._defendSkillUid = new DataUid(8, 10, (ulong)((long)base.CharacterId), 63U);
			GameDataBridge.AddPostDataModificationHandler(this._defendSkillUid, base.DataHandlerKey, new Action<DataContext, DataUid>(this.OnDefendSkillChanged));
			bool listenCanAffectChange = this.ListenCanAffectChange;
			if (listenCanAffectChange)
			{
				this._defendSkillCanAffectUid = base.ParseCombatSkillDataUid(9);
				GameDataBridge.AddPostDataModificationHandler(this._defendSkillCanAffectUid, base.DataHandlerKey, new Action<DataContext, DataUid>(this.OnDefendSkillCanAffectChanged));
			}
		}

		// Token: 0x060041CA RID: 16842 RVA: 0x0026414C File Offset: 0x0026234C
		public override void OnDisable(DataContext context)
		{
			GameDataBridge.RemovePostDataModificationHandler(this._defendSkillUid, base.DataHandlerKey);
		}

		// Token: 0x060041CB RID: 16843 RVA: 0x00264164 File Offset: 0x00262364
		private void OnDefendSkillChanged(DataContext context, DataUid dataUid)
		{
			bool flag = base.CombatChar.GetAffectingDefendSkillId() != base.SkillTemplateId && this.AutoRemove;
			if (flag)
			{
				base.RemoveSelf(context);
			}
		}

		// Token: 0x060041CC RID: 16844 RVA: 0x0026419A File Offset: 0x0026239A
		protected virtual void OnDefendSkillCanAffectChanged(DataContext context, DataUid dataUid)
		{
		}

		// Token: 0x04001363 RID: 4963
		protected bool AutoRemove = true;

		// Token: 0x04001364 RID: 4964
		protected bool ListenCanAffectChange = false;

		// Token: 0x04001365 RID: 4965
		private DataUid _defendSkillUid;

		// Token: 0x04001366 RID: 4966
		private DataUid _defendSkillCanAffectUid;
	}
}
