using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.Character.Relation;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.Information;

namespace GameData.Domains.SpecialEffect.CombatSkill.Jieqingmen.Sword;

public class JueYiJian : CombatSkillEffectBase
{
	private const int AddShaCount = 9;

	private const int ChangeFavorabilityValue = -10000;

	private static readonly short[] AddDamage = new short[4] { 20, 40, 80, 160 };

	private bool _affected;

	private int _addDamage;

	public JueYiJian()
	{
	}

	public JueYiJian(CombatSkillKey skillKey)
		: base(skillKey, 13201, -1)
	{
	}

	public override void OnEnable(DataContext context)
	{
		Events.RegisterHandler_CastAttackSkillBegin(OnCastAttackSkillBegin);
		Events.RegisterHandler_CastSkillEnd(OnCastSkillEnd);
	}

	public override void OnDisable(DataContext context)
	{
		Events.UnRegisterHandler_CastAttackSkillBegin(OnCastAttackSkillBegin);
		Events.UnRegisterHandler_CastSkillEnd(OnCastSkillEnd);
	}

	private void OnCastAttackSkillBegin(DataContext context, CombatCharacter attacker, CombatCharacter defender, short skillId)
	{
		if (attacker.GetId() != base.CharacterId || skillId != base.SkillTemplateId)
		{
			return;
		}
		short favorability = DomainManager.Character.GetFavorability(base.CurrEnemyChar.GetId(), base.CharacterId);
		sbyte favorabilityType = FavorabilityType.GetFavorabilityType(favorability);
		if (favorabilityType >= 3)
		{
			_addDamage = AddDamage[favorabilityType - 3];
			AppendAffectedData(context, 69, (EDataModifyType)1, base.SkillTemplateId);
			ShowSpecialEffectTips(0);
			if (favorabilityType >= 4)
			{
				_affected = true;
				DomainManager.Combat.AddTrick(context, base.IsDirect ? base.CombatChar : base.EnemyChar, 19, 9, base.IsDirect);
				ShowSpecialEffectTips(1);
			}
		}
	}

	private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
	{
		if (charId == base.CharacterId && skillId == base.SkillTemplateId)
		{
			if (_affected)
			{
				_affected = false;
				MakeRelation(context);
				ChangeFavorability(context);
			}
			RemoveSelf(context);
		}
	}

	private void MakeRelation(DataContext context)
	{
		if (RelationTypeHelper.AllowAddingRelation(base.CurrEnemyChar.GetId(), base.CharacterId, 32768))
		{
			InformationDomain information = DomainManager.Information;
			DomainManager.Character.AddRelation(context, base.CurrEnemyChar.GetId(), base.CharacterId, 32768);
			int metaDataId = information.AddSecretInformationMetaData(context, information.GetSecretInformationCollection().AddBecomeEnemy(base.CharacterId, base.CurrEnemyChar.GetId()), withInitialDistribute: false);
			information.ReceiveSecretInformation(context, metaDataId, base.CharacterId);
			information.ReceiveSecretInformation(context, metaDataId, base.CurrEnemyChar.GetId());
			information.SetSecretInformationCollectionModified(context);
			ShowSpecialEffectTips(2);
		}
	}

	private void ChangeFavorability(DataContext context)
	{
		GameData.Domains.Character.Character character = base.CurrEnemyChar.GetCharacter();
		byte creatingType = character.GetCreatingType();
		if ((uint)creatingType <= 1u)
		{
			GameData.Domains.Character.Character character2 = base.CombatChar.GetCharacter();
			creatingType = character2.GetCreatingType();
			if ((uint)creatingType <= 1u)
			{
				DomainManager.Character.DirectlyChangeFavorabilityOptional(context, character, character2, -10000, -1);
			}
		}
	}

	public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
	{
		if (dataKey.CharId != base.CharacterId || dataKey.CombatSkillId != base.SkillTemplateId)
		{
			return 0;
		}
		if (dataKey.FieldId == 69)
		{
			return _addDamage;
		}
		return 0;
	}
}
