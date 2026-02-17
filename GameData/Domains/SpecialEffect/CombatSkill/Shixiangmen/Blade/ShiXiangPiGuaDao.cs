using System;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Attack;

namespace GameData.Domains.SpecialEffect.CombatSkill.Shixiangmen.Blade
{
	// Token: 0x0200040B RID: 1035
	public class ShiXiangPiGuaDao : TrickBuffFlaw
	{
		// Token: 0x060038F3 RID: 14579 RVA: 0x0023C995 File Offset: 0x0023AB95
		public ShiXiangPiGuaDao()
		{
		}

		// Token: 0x060038F4 RID: 14580 RVA: 0x0023C99F File Offset: 0x0023AB9F
		public ShiXiangPiGuaDao(CombatSkillKey skillKey) : base(skillKey, 6202)
		{
			this.RequireTrickType = 3;
		}

		// Token: 0x060038F5 RID: 14581 RVA: 0x0023C9B6 File Offset: 0x0023ABB6
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			Events.RegisterHandler_PrepareSkillBegin(new Events.OnPrepareSkillBegin(this.PrepareSkillBegin));
			Events.RegisterHandler_AddDirectDamageValue(new Events.OnAddDirectDamageValue(this.AddDirectDamageValue));
		}

		// Token: 0x060038F6 RID: 14582 RVA: 0x0023C9E5 File Offset: 0x0023ABE5
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_PrepareSkillBegin(new Events.OnPrepareSkillBegin(this.PrepareSkillBegin));
			Events.UnRegisterHandler_AddDirectDamageValue(new Events.OnAddDirectDamageValue(this.AddDirectDamageValue));
			base.OnDisable(context);
		}

		// Token: 0x060038F7 RID: 14583 RVA: 0x0023CA14 File Offset: 0x0023AC14
		private void PrepareSkillBegin(DataContext context, int charId, bool isAlly, short skillId)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				this._directDamageValue = 0;
			}
		}

		// Token: 0x060038F8 RID: 14584 RVA: 0x0023CA48 File Offset: 0x0023AC48
		private void AddDirectDamageValue(DataContext context, int attackerId, int defenderId, sbyte bodyPart, bool isInner, int damageValue, short combatSkillId)
		{
			bool flag = attackerId != base.CharacterId || combatSkillId != base.SkillTemplateId;
			if (!flag)
			{
				this._directDamageValue += damageValue;
			}
		}

		// Token: 0x060038F9 RID: 14585 RVA: 0x0023CA84 File Offset: 0x0023AC84
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

		// Token: 0x040010A8 RID: 4264
		private const int FatalDamageValuePercent = 10;

		// Token: 0x040010A9 RID: 4265
		private int _directDamageValue;
	}
}
