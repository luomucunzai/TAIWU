using System;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.Item;
using GameData.Serializer;

namespace GameData.Domains.SpecialEffect.CombatSkill.Yuanshanpai.Sword;

public class PuDuJianFa : CombatSkillEffectBase
{
	private const sbyte ChangeInfection = 50;

	public PuDuJianFa()
	{
	}

	public PuDuJianFa(CombatSkillKey skillKey)
		: base(skillKey, 5201, -1)
	{
	}

	public override void OnEnable(DataContext context)
	{
		Events.RegisterHandler_CastAttackSkillBegin(OnCastAttackSkillBegin);
		Events.RegisterHandler_CombatSettlement(OnCombatSettlement);
		Events.RegisterHandler_CastSkillEnd(OnCastSkillEnd);
	}

	public override void OnDisable(DataContext context)
	{
		Events.UnRegisterHandler_CastAttackSkillBegin(OnCastAttackSkillBegin);
		Events.UnRegisterHandler_CombatSettlement(OnCombatSettlement);
		Events.UnRegisterHandler_CastSkillEnd(OnCastSkillEnd);
	}

	private void OnCastAttackSkillBegin(DataContext context, CombatCharacter attacker, CombatCharacter defender, short skillId)
	{
		if (attacker == base.CombatChar && skillId == base.SkillTemplateId)
		{
			IsSrcSkillPrepared = true;
		}
	}

	private void OnCombatSettlement(DataContext context, sbyte combatStatus)
	{
		if (IsSrcSkillPrepared && base.CombatChar.IsAlly && combatStatus == CombatStatusType.EnemyFail && DomainManager.Combat.GetCombatType() == 2)
		{
			GameData.Domains.Character.Character character = DomainManager.Combat.GetMainCharacter(!base.CombatChar.IsAlly).GetCharacter();
			if (character.GetFeatureIds().Contains(217))
			{
				int xiangshuInfection = character.GetXiangshuInfection();
				int delta = (base.IsDirect ? Math.Max(-50, 100 - xiangshuInfection) : Math.Min(50, 200 - xiangshuInfection - 1));
				ItemKey itemKey = DomainManager.Item.CreateItem(context, 12, 73);
				base.CombatChar.GetCharacter().AddInventoryItem(context, itemKey, 1);
				character.ChangeXiangshuInfection(context, delta);
				DomainManager.Combat.AppendGetChar(character.GetId());
				DomainManager.TaiwuEvent.SetListenerEventActionIntArg("CombatOver", "CharIdSeizedInCombat", character.GetId());
				DomainManager.TaiwuEvent.SetListenerEventActionISerializableArg("CombatOver", "ItemKeySeizeCharacterInCombat", (ISerializableGameData)(object)itemKey);
				DomainManager.TaiwuEvent.SetListenerEventActionIntArg("CombatOver", "UseItemKeySeizeCharacterId", base.CombatChar.GetId());
				ShowSpecialEffectTips(0);
			}
		}
	}

	private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
	{
		if (charId == base.CharacterId && skillId == base.SkillTemplateId)
		{
			CombatCharacter mainCharacter = DomainManager.Combat.GetMainCharacter(!base.CombatChar.IsAlly);
			if (!DomainManager.Combat.IsCharacterFallen(mainCharacter))
			{
				RemoveSelf(context);
			}
		}
	}
}
