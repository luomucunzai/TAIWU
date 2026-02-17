using System;
using GameData.Common;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Common.Agile
{
	// Token: 0x020005B0 RID: 1456
	public class AgileSkillBase : CombatSkillEffectBase
	{
		// Token: 0x170002A9 RID: 681
		// (get) Token: 0x0600434D RID: 17229 RVA: 0x0026AE3C File Offset: 0x0026903C
		protected bool CanAffect
		{
			get
			{
				return base.SkillData.GetCanAffect() && base.CombatChar.GetAffectingMoveSkillId() == base.SkillTemplateId;
			}
		}

		// Token: 0x0600434E RID: 17230 RVA: 0x0026AE61 File Offset: 0x00269061
		protected AgileSkillBase()
		{
		}

		// Token: 0x0600434F RID: 17231 RVA: 0x0026AE80 File Offset: 0x00269080
		protected AgileSkillBase(CombatSkillKey skillKey, int type) : base(skillKey, type, -1)
		{
		}

		// Token: 0x06004350 RID: 17232 RVA: 0x0026AEA4 File Offset: 0x002690A4
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			base.CombatChar.SetAffectingMoveSkillId(base.CombatChar.GetAffectingMoveSkillId(), context);
			DataUid moveSkillUid = base.ParseCombatCharacterDataUid(62);
			base.AutoMonitor(moveSkillUid, new Action<DataContext, DataUid>(this.OnMoveSkillChanged));
			bool flag = !this.ListenCanAffectChange;
			if (!flag)
			{
				DataUid canAffectUid = base.ParseCombatSkillDataUid(9);
				base.AutoMonitor(canAffectUid, new Action<DataContext, DataUid>(this.OnMoveSkillCanAffectChanged));
			}
		}

		// Token: 0x06004351 RID: 17233 RVA: 0x0026AF20 File Offset: 0x00269120
		protected virtual void OnMoveSkillChanged(DataContext context, DataUid dataUid)
		{
			bool flag = base.CombatChar.GetAffectingMoveSkillId() == base.SkillTemplateId;
			if (!flag)
			{
				bool autoRemove = this.AutoRemove;
				if (autoRemove)
				{
					base.RemoveSelf(context);
				}
				else
				{
					this.AgileSkillChanged = true;
				}
			}
		}

		// Token: 0x06004352 RID: 17234 RVA: 0x0026AF62 File Offset: 0x00269162
		protected virtual void OnMoveSkillCanAffectChanged(DataContext context, DataUid dataUid)
		{
		}

		// Token: 0x040013F9 RID: 5113
		protected bool AutoRemove = true;

		// Token: 0x040013FA RID: 5114
		protected bool AgileSkillChanged = false;

		// Token: 0x040013FB RID: 5115
		protected bool ListenCanAffectChange = false;
	}
}
