using System.Collections.Generic;
using System.Linq;
using Config;
using GameData.Common;
using GameData.Domains.Character;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.Information.Collection;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Attack;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.Wuxianjiao.FistAndPalm;

public class JiuSiLiHunShou : PoisonAddInjury
{
	private const string SecretInformationParticleName = "Particle_Effect_SecretInformation";

	private const string SecretInformationParticleNameReverse = "Particle_Effect_SecretInformationReverse";

	private const string SecretInformationSoundName = "se_effect_secret_information";

	private const int SecretInformationOccurredRate = 30;

	private static bool IsAffectChar(GameData.Domains.Character.Character character)
	{
		return character.GetCreatingType() == 1 && character.GetAgeGroup() == 2;
	}

	public JiuSiLiHunShou()
	{
	}

	public JiuSiLiHunShou(CombatSkillKey skillKey)
		: base(skillKey, 12107)
	{
		RequirePoisonType = 5;
	}

	protected override void OnCastMaxPower(DataContext context)
	{
		CombatCharacter combatChar = base.CombatChar;
		if (!IsAffectChar(combatChar.GetCharacter()))
		{
			return;
		}
		CombatCharacter enemyChar = DomainManager.Combat.GetCombatCharacter(!combatChar.IsAlly);
		if (!IsAffectChar(enemyChar.GetCharacter()))
		{
			return;
		}
		byte b = (base.IsDirect ? enemyChar : combatChar).GetDefeatMarkCollection().PoisonMarkList[RequirePoisonType];
		if (!context.Random.CheckPercentProb(b * 30))
		{
			return;
		}
		int id = enemyChar.GetId();
		List<int> list = ObjectPool<List<int>>.Instance.Get();
		list.Clear();
		list.AddRange(from x in DomainManager.Combat.GetTeamCharacterIds()
			where x != enemyChar.GetId()
			where DomainManager.Character.TryGetElement_Objects(x, out var element) && IsAffectChar(element)
			select x);
		int num = ((list.Count > 0) ? list.GetRandom(context.Random) : (-1));
		ObjectPool<List<int>>.Instance.Return(list);
		if (num >= 0)
		{
			SecretInformationCollection secretInformationCollection = DomainManager.Information.GetSecretInformationCollection();
			int num2 = context.Random.Next(4);
			if (1 == 0)
			{
			}
			int num3 = num2 switch
			{
				0 => secretInformationCollection.AddKidnapInPrivate(id, num), 
				1 => secretInformationCollection.AddPoisonEnemy(id, num), 
				2 => secretInformationCollection.AddPlotHarmEnemy(id, num), 
				3 => secretInformationCollection.AddRape(id, num), 
				_ => -1, 
			};
			if (1 == 0)
			{
			}
			int num4 = num3;
			if (num4 < 0)
			{
				PredefinedLog.Show(7, base.EffectId, $"secretInfoType={num2}");
			}
			else
			{
				int metaDataId = DomainManager.Information.AddSecretInformationMetaData(context, num4, withInitialDistribute: false);
				DomainManager.Information.ReceiveSecretInformation(context, metaDataId, combatChar.GetId());
				ShowSpecialEffectTips(1);
				base.CombatChar.SetSkillSoundToPlay("se_effect_secret_information", context);
				enemyChar.SetParticleToPlay(enemyChar.IsAlly ? "Particle_Effect_SecretInformation" : "Particle_Effect_SecretInformationReverse", context);
			}
		}
	}
}
