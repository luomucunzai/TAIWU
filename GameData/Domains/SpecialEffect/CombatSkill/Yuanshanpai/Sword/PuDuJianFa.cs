using System;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.Item;

namespace GameData.Domains.SpecialEffect.CombatSkill.Yuanshanpai.Sword
{
	// Token: 0x020001F2 RID: 498
	public class PuDuJianFa : CombatSkillEffectBase
	{
		// Token: 0x06002E46 RID: 11846 RVA: 0x0020E416 File Offset: 0x0020C616
		public PuDuJianFa()
		{
		}

		// Token: 0x06002E47 RID: 11847 RVA: 0x0020E420 File Offset: 0x0020C620
		public PuDuJianFa(CombatSkillKey skillKey) : base(skillKey, 5201, -1)
		{
		}

		// Token: 0x06002E48 RID: 11848 RVA: 0x0020E431 File Offset: 0x0020C631
		public override void OnEnable(DataContext context)
		{
			Events.RegisterHandler_CastAttackSkillBegin(new Events.OnCastAttackSkillBegin(this.OnCastAttackSkillBegin));
			Events.RegisterHandler_CombatSettlement(new Events.OnCombatSettlement(this.OnCombatSettlement));
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06002E49 RID: 11849 RVA: 0x0020E46A File Offset: 0x0020C66A
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CastAttackSkillBegin(new Events.OnCastAttackSkillBegin(this.OnCastAttackSkillBegin));
			Events.UnRegisterHandler_CombatSettlement(new Events.OnCombatSettlement(this.OnCombatSettlement));
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06002E4A RID: 11850 RVA: 0x0020E4A4 File Offset: 0x0020C6A4
		private void OnCastAttackSkillBegin(DataContext context, CombatCharacter attacker, CombatCharacter defender, short skillId)
		{
			bool flag = attacker != base.CombatChar || skillId != base.SkillTemplateId;
			if (!flag)
			{
				this.IsSrcSkillPrepared = true;
			}
		}

		// Token: 0x06002E4B RID: 11851 RVA: 0x0020E4D8 File Offset: 0x0020C6D8
		private void OnCombatSettlement(DataContext context, sbyte combatStatus)
		{
			bool flag = !this.IsSrcSkillPrepared || !base.CombatChar.IsAlly || combatStatus != CombatStatusType.EnemyFail || DomainManager.Combat.GetCombatType() != 2;
			if (!flag)
			{
				Character enemyChar = DomainManager.Combat.GetMainCharacter(!base.CombatChar.IsAlly).GetCharacter();
				bool flag2 = !enemyChar.GetFeatureIds().Contains(217);
				if (!flag2)
				{
					int infection = (int)enemyChar.GetXiangshuInfection();
					int deltaValue = base.IsDirect ? Math.Max(-50, 100 - infection) : Math.Min(50, 200 - infection - 1);
					ItemKey ropeKey = DomainManager.Item.CreateItem(context, 12, 73);
					base.CombatChar.GetCharacter().AddInventoryItem(context, ropeKey, 1, false);
					enemyChar.ChangeXiangshuInfection(context, deltaValue);
					DomainManager.Combat.AppendGetChar(enemyChar.GetId());
					DomainManager.TaiwuEvent.SetListenerEventActionIntArg("CombatOver", "CharIdSeizedInCombat", enemyChar.GetId());
					DomainManager.TaiwuEvent.SetListenerEventActionISerializableArg("CombatOver", "ItemKeySeizeCharacterInCombat", ropeKey);
					DomainManager.TaiwuEvent.SetListenerEventActionIntArg("CombatOver", "UseItemKeySeizeCharacterId", base.CombatChar.GetId());
					base.ShowSpecialEffectTips(0);
				}
			}
		}

		// Token: 0x06002E4C RID: 11852 RVA: 0x0020E628 File Offset: 0x0020C828
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				CombatCharacter enemyChar = DomainManager.Combat.GetMainCharacter(!base.CombatChar.IsAlly);
				bool flag2 = DomainManager.Combat.IsCharacterFallen(enemyChar);
				if (!flag2)
				{
					base.RemoveSelf(context);
				}
			}
		}

		// Token: 0x04000DC5 RID: 3525
		private const sbyte ChangeInfection = 50;
	}
}
