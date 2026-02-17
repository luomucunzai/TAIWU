using System;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Attack;

namespace GameData.Domains.SpecialEffect.CombatSkill.Fulongtan.Throw
{
	// Token: 0x0200050C RID: 1292
	public class FeiShaZouShi : TrickBuffFlaw
	{
		// Token: 0x06003EC9 RID: 16073 RVA: 0x00257229 File Offset: 0x00255429
		public FeiShaZouShi()
		{
		}

		// Token: 0x06003ECA RID: 16074 RVA: 0x00257233 File Offset: 0x00255433
		public FeiShaZouShi(CombatSkillKey skillKey) : base(skillKey, 14300)
		{
			this.RequireTrickType = 0;
		}

		// Token: 0x06003ECB RID: 16075 RVA: 0x0025724A File Offset: 0x0025544A
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			Events.RegisterHandler_PrepareSkillBegin(new Events.OnPrepareSkillBegin(this.PrepareSkillBegin));
			Events.RegisterHandler_AddDirectDamageValue(new Events.OnAddDirectDamageValue(this.AddDirectDamageValue));
		}

		// Token: 0x06003ECC RID: 16076 RVA: 0x00257279 File Offset: 0x00255479
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_PrepareSkillBegin(new Events.OnPrepareSkillBegin(this.PrepareSkillBegin));
			Events.UnRegisterHandler_AddDirectDamageValue(new Events.OnAddDirectDamageValue(this.AddDirectDamageValue));
			base.OnDisable(context);
		}

		// Token: 0x06003ECD RID: 16077 RVA: 0x002572A8 File Offset: 0x002554A8
		private void PrepareSkillBegin(DataContext context, int charId, bool isAlly, short skillId)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				this._directDamageValue = 0;
			}
		}

		// Token: 0x06003ECE RID: 16078 RVA: 0x002572DC File Offset: 0x002554DC
		private void AddDirectDamageValue(DataContext context, int attackerId, int defenderId, sbyte bodyPart, bool isInner, int damageValue, short combatSkillId)
		{
			bool flag = attackerId != base.CharacterId || combatSkillId != base.SkillTemplateId;
			if (!flag)
			{
				this._directDamageValue += damageValue;
			}
		}

		// Token: 0x06003ECF RID: 16079 RVA: 0x00257318 File Offset: 0x00255518
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

		// Token: 0x04001287 RID: 4743
		private const int FatalDamageValuePercent = 10;

		// Token: 0x04001288 RID: 4744
		private int _directDamageValue;
	}
}
