using System;
using GameData.Domains;
using GameData.Domains.Character;
using GameData.Domains.Character.Relation;
using GameData.Domains.Information;
using GameData.Domains.TaiwuEvent.EventHelper;

namespace Config.EventConfig;

public class SecretInformationConfig
{
	private static readonly int[] LeadToGoodByCombatBehaviorTypeParam = new int[5] { 20, 18, 16, 14, 12 };

	private static readonly int[] LeadToGoodByRationalityBehaviorTypeParam = new int[5] { 12, 14, 16, 18, 20 };

	private static readonly int[] LeadToGoodByEmotionBehaviorTypeParam = new int[5] { 18, 6, 12, 6, 18 };

	private static readonly int[] LeadToGoodByEmotionBehaviorTypeCorrectionParam = new int[5] { -400, -800, -600, -800, -400 };

	private static readonly int[] LeadToEvilByCombatBehaviorTypeParam = new int[5] { 20, 18, 16, 14, 12 };

	private static readonly int[] LeadToEvilByRationalityBehaviorTypeParam = new int[5] { 12, 14, 16, 18, 20 };

	private static readonly int[] GetLeadToEvilByEmotionBehaviorTypeParam = new int[5] { 18, 6, 12, 6, 18 };

	private static readonly int[] GetLeadToEvilByEmotionBehaviorTypeCorrectionParam = new int[5] { -400, -800, -600, -800, -400 };

	private static readonly int[] WriteOffDebtsByCombatFavorBehaviorTypeParam = new int[5] { 20, 18, 16, 14, 12 };

	private static readonly int[] WriteOffDebtsByCombatDebtsBehaviorTypeParam = new int[5] { 12, 14, 16, 18, 20 };

	private static readonly int[] WriteOffDebtsByRationalityFavorBehaviorTypeParam = new int[5] { 12, 14, 16, 18, 20 };

	private static readonly int[] WriteOffDebtsByRationalityDebtsBehaviorTypeParam = new int[5] { 20, 18, 16, 14, 12 };

	private static readonly int[] WriteOffDebtsByEmotionDebtsBehaviorTypeParam = new int[5] { 12, 20, 16, 20, 12 };

	private static readonly int[] WriteOffDebtsByEmotionBehaviorTypeParam = new int[5] { 18, 6, 12, 6, 18 };

	private static readonly int[] WriteOffDebtsByEmotionBehaviorTypeCorrectionParam = new int[5] { -400, -800, -600, -800, -400 };

	public static readonly int GradeFactor_StartRelation_Difficulty = 6;

	public static readonly int[] BehaviorAdjust_StartRelation_Difficulty = new int[5] { 18, 6, 12, 6, 18 };

	public static readonly int[] FavorFactor_StartRelation_Difficulty = new int[5] { 0, -300, -200, 300, -100 };

	public static readonly int SortValueFactor_StartRelation_Effect = 4;

	public static readonly int HolderCountFactor_StartRelation_Effect = 50;

	public static readonly int[] FavorFactor_StartRelation_Effect = new int[5] { 0, 300, 200, -300, 100 };

	public static readonly int FameDenominator_StartRelation_Effect = 2;

	public static readonly int GradeFactor_EndRelation_Difficulty = 6;

	public static readonly int[] BehaviorAdjust_EndRelation_Difficulty = new int[5] { 18, 6, 12, 6, 18 };

	public static readonly int[] FavorFactor_EndRelation_Difficulty = new int[5] { 100, 300, 200, -300, 0 };

	public static readonly int SortValueFactor_EndRelation_Effect = 4;

	public static readonly int HolderCountFactor_EndRelation_Effect = 50;

	public static readonly int[] FavorFactor_EndRelation_Effect = new int[5] { 100, 300, 200, -300, 0 };

	public static readonly int FameDenominator_EndRelation_Effect = 2;

	public static readonly int[] ReduceDebtBehaviorFactorOfThreatening = new int[5] { 14, 18, 20, 16, 12 };

	public static int GetLeadToGoodByCombatFavorabilityChange(int charId, int metaDataId)
	{
		GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(charId);
		SecretInformationItem secretInformationConfig = DomainManager.Information.GetSecretInformationConfig(metaDataId);
		return -((secretInformationConfig.SortValue - 1) * 50 * LeadToGoodByCombatBehaviorTypeParam[element_Objects.GetBehaviorType()] + 2000);
	}

	public static int GetLeadToGoodByCombatBehaviorTypeOffset(int metaDataId)
	{
		SecretInformationItem secretInformationConfig = DomainManager.Information.GetSecretInformationConfig(metaDataId);
		return secretInformationConfig.SortValue * 50 + 50;
	}

	public static int GetLeadToGoodByRationalityFavorabilityChange(int charId, int metaDataId)
	{
		GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(charId);
		SecretInformationItem secretInformationConfig = DomainManager.Information.GetSecretInformationConfig(metaDataId);
		return -((secretInformationConfig.SortValue - 1) * 25 * LeadToGoodByRationalityBehaviorTypeParam[element_Objects.GetBehaviorType()] + 1000);
	}

	public static int GetLeadToGoodByRationalityBehaviorTypeOffset(int metaDataId)
	{
		SecretInformationItem secretInformationConfig = DomainManager.Information.GetSecretInformationConfig(metaDataId);
		return secretInformationConfig.SortValue * 50 + 50;
	}

	public static int GetLeadToGoodByEmotionBehaviorTypeOffset(int metaDataId)
	{
		SecretInformationItem secretInformationConfig = DomainManager.Information.GetSecretInformationConfig(metaDataId);
		return secretInformationConfig.SortValue * 50 + 50;
	}

	public static int GetLeadToGoodByEmotionDifficultyValue(int charId)
	{
		GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(charId);
		int num = (element_Objects.GetOrganizationInfo().Grade + 1) * 6 + LeadToGoodByEmotionBehaviorTypeParam[element_Objects.GetBehaviorType()];
		int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
		sbyte favorabilityType = FavorabilityType.GetFavorabilityType(DomainManager.Character.GetFavorability(charId, taiwuCharId));
		return Math.Max(favorabilityType * LeadToGoodByEmotionBehaviorTypeCorrectionParam[element_Objects.GetBehaviorType()] / 100 + num, 0);
	}

	public static int GetLeadToGoodByEmotionEffectValue(int charId, int metaDataId)
	{
		GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(charId);
		SecretInformationItem secretInformationConfig = DomainManager.Information.GetSecretInformationConfig(metaDataId);
		bool isSect = Organization.Instance[element_Objects.GetOrganizationInfo().OrgTemplateId].IsSect;
		int num = 0;
		if (isSect)
		{
			SecretInformationProcessor secretInformationProcessor = DomainManager.Information.SecretInformationProcessorPool.Get();
			if (!secretInformationProcessor.Initialize(metaDataId))
			{
				DomainManager.Information.SecretInformationProcessorPool.Return(secretInformationProcessor);
			}
			else
			{
				num = secretInformationProcessor.CalSectPunishLevel_WithCharId(charId) + 1;
				DomainManager.Information.SecretInformationProcessorPool.Return(secretInformationProcessor);
			}
		}
		else
		{
			num = secretInformationConfig.SortValue / 3;
		}
		int num2 = secretInformationConfig.SortValue * 2;
		num2 += num2 * num * 20 / 100;
		int num3 = (EventHelper.IsSecretInformationBroadcast(metaDataId) ? SecretInformation.Instance.GetItem(secretInformationConfig.TemplateId).MaxPersonAmount : Math.Min(EventHelper.GetSecretInformationHolderCount(metaDataId), SecretInformation.Instance.GetItem(secretInformationConfig.TemplateId).MaxPersonAmount));
		int num4 = num3 * 50 / SecretInformation.Instance.GetItem(secretInformationConfig.TemplateId).MaxPersonAmount * num2 / 100;
		return num4 + num4 * element_Objects.GetFame() / 2 / 100;
	}

	public static int GetLeadToEvilByCombatFavorabilityChange(int charId, int metaDataId)
	{
		return 0;
	}

	public static int GetLeadToEvilByCombatBehaviorTypeOffset(int metaDataId)
	{
		SecretInformationItem secretInformationConfig = DomainManager.Information.GetSecretInformationConfig(metaDataId);
		return -(secretInformationConfig.SortValue * 50 + 50);
	}

	public static int GetLeadToEvilByRationalityFavorabilityChange(int charId, int metaDataId)
	{
		return 0;
	}

	public static int GetLeadToEvilByRationalityBehaviorTypeOffset(int metaDataId)
	{
		SecretInformationItem secretInformationConfig = DomainManager.Information.GetSecretInformationConfig(metaDataId);
		return -(secretInformationConfig.SortValue * 50 + 50);
	}

	public static int GetLeadToEvilByEmotionBehaviorTypeOffset(int metaDataId)
	{
		SecretInformationItem secretInformationConfig = DomainManager.Information.GetSecretInformationConfig(metaDataId);
		return -(secretInformationConfig.SortValue * 50 + 50);
	}

	public static int GetLeadToEvilByEmotionDifficultyValue(int charId, int metaDataId)
	{
		return 0;
	}

	public static int GetLeadToEvilByEmotionEffectValue(int charId, int metaDataId)
	{
		return 0;
	}

	public static int WriteOffDebtsByCombatFavorabilityChange(int charId, int metaDataId)
	{
		return 0;
	}

	public static int WriteOffDebtsByCombatDebtsWorth(int charId, int metaDataId)
	{
		GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(charId);
		SecretInformationItem secretInformationConfig = DomainManager.Information.GetSecretInformationConfig(metaDataId);
		return (secretInformationConfig.SortValue - 1) * 50 * WriteOffDebtsByCombatDebtsBehaviorTypeParam[element_Objects.GetBehaviorType()] + 2000;
	}

	public static int WriteOffDebtsByRationalityFavorabilityChange(int charId, int metaDataId)
	{
		return 0;
	}

	public static int WriteOffDebtsByRationalityDebtsWorth(int charId, int metaDataId)
	{
		GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(charId);
		SecretInformationItem secretInformationConfig = DomainManager.Information.GetSecretInformationConfig(metaDataId);
		return (secretInformationConfig.SortValue - 1) * 50 * WriteOffDebtsByRationalityDebtsBehaviorTypeParam[element_Objects.GetBehaviorType()] + 2000;
	}

	public static int WriteOffDebtsByEmotionDebtsWorth(int charId, int metaDataId)
	{
		GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(charId);
		SecretInformationItem secretInformationConfig = DomainManager.Information.GetSecretInformationConfig(metaDataId);
		return (secretInformationConfig.SortValue - 1) * 50 * WriteOffDebtsByEmotionDebtsBehaviorTypeParam[element_Objects.GetBehaviorType()] + 2000;
	}

	public static int WriteOffDebtsByEmotionDifficultyValue(int charId, int metaDataId)
	{
		return 0;
	}

	public static int WriteOffDebtsByEmotionEffectValue(int charId, int metaDataId)
	{
		return 0;
	}
}
