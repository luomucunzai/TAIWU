using System;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Common.Attack
{
	// Token: 0x02000585 RID: 1413
	public class AddCombatStateBySkillPower : CombatSkillEffectBase
	{
		// Token: 0x060041D7 RID: 16855 RVA: 0x0026457E File Offset: 0x0026277E
		protected AddCombatStateBySkillPower()
		{
		}

		// Token: 0x060041D8 RID: 16856 RVA: 0x00264588 File Offset: 0x00262788
		protected AddCombatStateBySkillPower(CombatSkillKey skillKey, int type) : base(skillKey, type, -1)
		{
		}

		// Token: 0x060041D9 RID: 16857 RVA: 0x00264595 File Offset: 0x00262795
		public override void OnEnable(DataContext context)
		{
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x060041DA RID: 16858 RVA: 0x002645AA File Offset: 0x002627AA
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x060041DB RID: 16859 RVA: 0x002645C0 File Offset: 0x002627C0
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				bool flag2 = power > 0;
				if (flag2)
				{
					int index = base.IsDirect ? 0 : 1;
					CombatCharacter combatChar = this.StateAddToSelf[index] ? base.CombatChar : base.CurrEnemyChar;
					DomainManager.Combat.AddCombatState(context, combatChar, this.StateTypes[index], this.StateIds[index], (int)(this.StatePowerUnit * power / 10));
					base.ShowSpecialEffectTips(0);
				}
				base.RemoveSelf(context);
			}
		}

		// Token: 0x0400136A RID: 4970
		protected sbyte[] StateTypes;

		// Token: 0x0400136B RID: 4971
		protected short[] StateIds;

		// Token: 0x0400136C RID: 4972
		protected bool[] StateAddToSelf;

		// Token: 0x0400136D RID: 4973
		protected sbyte StatePowerUnit;
	}
}
