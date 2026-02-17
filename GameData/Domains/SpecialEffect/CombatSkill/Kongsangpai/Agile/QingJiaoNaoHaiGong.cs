using System;
using Config;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Agile;

namespace GameData.Domains.SpecialEffect.CombatSkill.Kongsangpai.Agile
{
	// Token: 0x020004A3 RID: 1187
	public class QingJiaoNaoHaiGong : AgileSkillBase
	{
		// Token: 0x06003C8F RID: 15503 RVA: 0x0024E19B File Offset: 0x0024C39B
		public QingJiaoNaoHaiGong()
		{
		}

		// Token: 0x06003C90 RID: 15504 RVA: 0x0024E1A5 File Offset: 0x0024C3A5
		public QingJiaoNaoHaiGong(CombatSkillKey skillKey) : base(skillKey, 10504)
		{
		}

		// Token: 0x06003C91 RID: 15505 RVA: 0x0024E1B5 File Offset: 0x0024C3B5
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			Events.RegisterHandler_CastLegSkillWithAgile(new Events.OnCastLegSkillWithAgile(this.OnCastLegSkillWithAgile));
		}

		// Token: 0x06003C92 RID: 15506 RVA: 0x0024E1D2 File Offset: 0x0024C3D2
		public override void OnDisable(DataContext context)
		{
			base.OnDisable(context);
			Events.UnRegisterHandler_CastLegSkillWithAgile(new Events.OnCastLegSkillWithAgile(this.OnCastLegSkillWithAgile));
		}

		// Token: 0x06003C93 RID: 15507 RVA: 0x0024E1F0 File Offset: 0x0024C3F0
		private void OnCastLegSkillWithAgile(DataContext context, CombatCharacter combatChar, short legSkillId)
		{
			bool flag = !base.CanAffect || combatChar != base.CombatChar || combatChar.GetAutoCastingSkill();
			if (!flag)
			{
				this.AutoRemove = false;
				this._affectingLegSkill = legSkillId;
				Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
			}
		}

		// Token: 0x06003C94 RID: 15508 RVA: 0x0024E240 File Offset: 0x0024C440
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != this._affectingLegSkill;
			if (!flag)
			{
				bool flag2 = base.PowerMatchAffectRequire((int)power, 0);
				if (flag2)
				{
					short autoCastSkillId = DomainManager.Combat.GetRandomAttackSkill(base.CombatChar, 5, Config.CombatSkill.Instance[skillId].Grade + (base.IsDirect ? -1 : 1), context.Random, base.IsDirect, skillId);
					bool flag3 = autoCastSkillId >= 0;
					if (flag3)
					{
						DomainManager.Combat.CastSkillFree(context, base.CombatChar, autoCastSkillId, ECombatCastFreePriority.Normal);
						base.ShowSpecialEffectTips(0);
					}
				}
				Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
				bool agileSkillChanged = this.AgileSkillChanged;
				if (agileSkillChanged)
				{
					base.RemoveSelf(context);
				}
				else
				{
					this.AutoRemove = true;
				}
			}
		}

		// Token: 0x040011D5 RID: 4565
		private short _affectingLegSkill;
	}
}
