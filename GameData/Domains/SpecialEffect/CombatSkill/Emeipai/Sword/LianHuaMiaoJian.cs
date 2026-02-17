using System;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Emeipai.Sword
{
	// Token: 0x0200053B RID: 1339
	public class LianHuaMiaoJian : CombatSkillEffectBase
	{
		// Token: 0x06003FD4 RID: 16340 RVA: 0x0025BBA6 File Offset: 0x00259DA6
		public LianHuaMiaoJian()
		{
		}

		// Token: 0x06003FD5 RID: 16341 RVA: 0x0025BBBE File Offset: 0x00259DBE
		public LianHuaMiaoJian(CombatSkillKey skillKey) : base(skillKey, 2303, -1)
		{
		}

		// Token: 0x06003FD6 RID: 16342 RVA: 0x0025BBDD File Offset: 0x00259DDD
		public override void OnEnable(DataContext context)
		{
			this._affected = false;
			Events.RegisterHandler_AddDirectInjury(new Events.OnAddDirectInjury(this.OnAddDirectInjury));
			Events.RegisterHandler_AddDirectFatalDamageMark(new Events.OnAddDirectFatalDamageMark(this.OnAddDirectFatalDamageMark));
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06003FD7 RID: 16343 RVA: 0x0025BC1D File Offset: 0x00259E1D
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_AddDirectInjury(new Events.OnAddDirectInjury(this.OnAddDirectInjury));
			Events.UnRegisterHandler_AddDirectFatalDamageMark(new Events.OnAddDirectFatalDamageMark(this.OnAddDirectFatalDamageMark));
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06003FD8 RID: 16344 RVA: 0x0025BC58 File Offset: 0x00259E58
		private void OnAddDirectInjury(DataContext context, int attackerId, int defenderId, bool isAlly, sbyte bodyPart, sbyte outerMarkCount, sbyte innerMarkCount, short combatSkillId)
		{
			bool flag = attackerId != base.CharacterId || combatSkillId != base.SkillTemplateId;
			if (!flag)
			{
				bool flag2 = (base.IsDirect ? outerMarkCount : innerMarkCount) >= this.RequireMarkCount;
				if (flag2)
				{
					this.AddAcupoint(context, bodyPart);
				}
			}
		}

		// Token: 0x06003FD9 RID: 16345 RVA: 0x0025BCAC File Offset: 0x00259EAC
		private void OnAddDirectFatalDamageMark(DataContext context, int attackerId, int defenderId, bool isAlly, sbyte bodyPart, int outerMarkCount, int innerMarkCount, short combatSkillId)
		{
			bool flag = attackerId != base.CharacterId || combatSkillId != base.SkillTemplateId;
			if (!flag)
			{
				bool flag2 = (base.IsDirect ? outerMarkCount : innerMarkCount) >= (int)this.RequireMarkCount;
				if (flag2)
				{
					this.AddAcupoint(context, bodyPart);
				}
			}
		}

		// Token: 0x06003FDA RID: 16346 RVA: 0x0025BD00 File Offset: 0x00259F00
		private void AddAcupoint(DataContext context, sbyte bodyPart)
		{
			bool affected = this._affected;
			if (!affected)
			{
				DomainManager.Combat.AddAcupoint(context, base.CurrEnemyChar, this.AcupointLevel, this.SkillKey, bodyPart, 1, true);
				base.ShowSpecialEffectTips(0);
				this._affected = true;
			}
		}

		// Token: 0x06003FDB RID: 16347 RVA: 0x0025BD4C File Offset: 0x00259F4C
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				base.RemoveSelf(context);
			}
		}

		// Token: 0x040012CD RID: 4813
		private sbyte RequireMarkCount = 1;

		// Token: 0x040012CE RID: 4814
		private sbyte AcupointLevel = 2;

		// Token: 0x040012CF RID: 4815
		private bool _affected;
	}
}
