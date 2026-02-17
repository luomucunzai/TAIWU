using System;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Attack;

namespace GameData.Domains.SpecialEffect.CombatSkill.Shixiangmen.FistAndPalm
{
	// Token: 0x020003F6 RID: 1014
	public class DaKaiMenBaShi : TrickBuffFlaw
	{
		// Token: 0x06003879 RID: 14457 RVA: 0x0023A84D File Offset: 0x00238A4D
		public DaKaiMenBaShi()
		{
		}

		// Token: 0x0600387A RID: 14458 RVA: 0x0023A857 File Offset: 0x00238A57
		public DaKaiMenBaShi(CombatSkillKey skillKey) : base(skillKey, 6102)
		{
			this.RequireTrickType = 6;
		}

		// Token: 0x0600387B RID: 14459 RVA: 0x0023A86E File Offset: 0x00238A6E
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			Events.RegisterHandler_PrepareSkillBegin(new Events.OnPrepareSkillBegin(this.PrepareSkillBegin));
			Events.RegisterHandler_AddDirectDamageValue(new Events.OnAddDirectDamageValue(this.AddDirectDamageValue));
		}

		// Token: 0x0600387C RID: 14460 RVA: 0x0023A89D File Offset: 0x00238A9D
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_PrepareSkillBegin(new Events.OnPrepareSkillBegin(this.PrepareSkillBegin));
			Events.UnRegisterHandler_AddDirectDamageValue(new Events.OnAddDirectDamageValue(this.AddDirectDamageValue));
			base.OnDisable(context);
		}

		// Token: 0x0600387D RID: 14461 RVA: 0x0023A8CC File Offset: 0x00238ACC
		private void PrepareSkillBegin(DataContext context, int charId, bool isAlly, short skillId)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				this._directDamageValue = 0;
			}
		}

		// Token: 0x0600387E RID: 14462 RVA: 0x0023A900 File Offset: 0x00238B00
		private void AddDirectDamageValue(DataContext context, int attackerId, int defenderId, sbyte bodyPart, bool isInner, int damageValue, short combatSkillId)
		{
			bool flag = attackerId != base.CharacterId || combatSkillId != base.SkillTemplateId;
			if (!flag)
			{
				this._directDamageValue += damageValue;
			}
		}

		// Token: 0x0600387F RID: 14463 RVA: 0x0023A93C File Offset: 0x00238B3C
		protected override bool OnReverseAffect(DataContext context, int trickCount)
		{
			int fatalDamageValue = this._directDamageValue * trickCount * 10 / 100;
			bool flag = fatalDamageValue <= 0;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				DomainManager.Combat.AddFatalDamageValue(context, base.CurrEnemyChar, fatalDamageValue, -1, -1, base.SkillTemplateId, EDamageType.None);
				DomainManager.Combat.AddToCheckFallenSet(base.CurrEnemyChar.GetId());
				result = true;
			}
			return result;
		}

		// Token: 0x04001087 RID: 4231
		private const int FatalDamageValuePercent = 10;

		// Token: 0x04001088 RID: 4232
		private int _directDamageValue;
	}
}
