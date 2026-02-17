using System;
using GameData.Common;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Agile;

namespace GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Agile
{
	// Token: 0x02000339 RID: 825
	public class JiangXian : AgileSkillBase
	{
		// Token: 0x060034A9 RID: 13481 RVA: 0x002295EE File Offset: 0x002277EE
		public JiangXian()
		{
		}

		// Token: 0x060034AA RID: 13482 RVA: 0x002295F8 File Offset: 0x002277F8
		public JiangXian(CombatSkillKey skillKey) : base(skillKey, 16200)
		{
			this.ListenCanAffectChange = true;
		}

		// Token: 0x060034AB RID: 13483 RVA: 0x00229610 File Offset: 0x00227810
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			this._affecting = false;
			this.OnMoveSkillCanAffectChanged(context, default(DataUid));
		}

		// Token: 0x060034AC RID: 13484 RVA: 0x0022963E File Offset: 0x0022783E
		public override void OnDisable(DataContext context)
		{
			base.OnDisable(context);
			DomainManager.Combat.DisableJumpMove(context, base.CombatChar, base.SkillTemplateId);
		}

		// Token: 0x060034AD RID: 13485 RVA: 0x00229664 File Offset: 0x00227864
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

		// Token: 0x04000F84 RID: 3972
		private bool _affecting;
	}
}
