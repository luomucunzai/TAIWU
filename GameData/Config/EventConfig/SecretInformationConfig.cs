using System;
using GameData.Domains;
using GameData.Domains.Character;
using GameData.Domains.Character.Relation;
using GameData.Domains.Information;
using GameData.Domains.TaiwuEvent.EventHelper;

namespace Config.EventConfig
{
	// Token: 0x02000011 RID: 17
	public class SecretInformationConfig
	{
		// Token: 0x06000042 RID: 66 RVA: 0x0004F52C File Offset: 0x0004D72C
		public static int GetLeadToGoodByCombatFavorabilityChange(int charId, int metaDataId)
		{
			Character character = DomainManager.Character.GetElement_Objects(charId);
			SecretInformationItem config = DomainManager.Information.GetSecretInformationConfig(metaDataId);
			return -((int)((config.SortValue - 1) * 50) * SecretInformationConfig.LeadToGoodByCombatBehaviorTypeParam[(int)character.GetBehaviorType()] + 2000);
		}

		// Token: 0x06000043 RID: 67 RVA: 0x0004F578 File Offset: 0x0004D778
		public static int GetLeadToGoodByCombatBehaviorTypeOffset(int metaDataId)
		{
			SecretInformationItem config = DomainManager.Information.GetSecretInformationConfig(metaDataId);
			return (int)(config.SortValue * 50 + 50);
		}

		// Token: 0x06000044 RID: 68 RVA: 0x0004F5A4 File Offset: 0x0004D7A4
		public static int GetLeadToGoodByRationalityFavorabilityChange(int charId, int metaDataId)
		{
			Character character = DomainManager.Character.GetElement_Objects(charId);
			SecretInformationItem config = DomainManager.Information.GetSecretInformationConfig(metaDataId);
			return -((int)((config.SortValue - 1) * 25) * SecretInformationConfig.LeadToGoodByRationalityBehaviorTypeParam[(int)character.GetBehaviorType()] + 1000);
		}

		// Token: 0x06000045 RID: 69 RVA: 0x0004F5F0 File Offset: 0x0004D7F0
		public static int GetLeadToGoodByRationalityBehaviorTypeOffset(int metaDataId)
		{
			SecretInformationItem config = DomainManager.Information.GetSecretInformationConfig(metaDataId);
			return (int)(config.SortValue * 50 + 50);
		}

		// Token: 0x06000046 RID: 70 RVA: 0x0004F61C File Offset: 0x0004D81C
		public static int GetLeadToGoodByEmotionBehaviorTypeOffset(int metaDataId)
		{
			SecretInformationItem config = DomainManager.Information.GetSecretInformationConfig(metaDataId);
			return (int)(config.SortValue * 50 + 50);
		}

		// Token: 0x06000047 RID: 71 RVA: 0x0004F648 File Offset: 0x0004D848
		public static int GetLeadToGoodByEmotionDifficultyValue(int charId)
		{
			Character character = DomainManager.Character.GetElement_Objects(charId);
			int baseValue = (int)((character.GetOrganizationInfo().Grade + 1) * 6) + SecretInformationConfig.LeadToGoodByEmotionBehaviorTypeParam[(int)character.GetBehaviorType()];
			int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
			sbyte favorabilityType = FavorabilityType.GetFavorabilityType(DomainManager.Character.GetFavorability(charId, taiwuCharId));
			return Math.Max((int)favorabilityType * SecretInformationConfig.LeadToGoodByEmotionBehaviorTypeCorrectionParam[(int)character.GetBehaviorType()] / 100 + baseValue, 0);
		}

		// Token: 0x06000048 RID: 72 RVA: 0x0004F6C0 File Offset: 0x0004D8C0
		public static int GetLeadToGoodByEmotionEffectValue(int charId, int metaDataId)
		{
			Character character = DomainManager.Character.GetElement_Objects(charId);
			SecretInformationItem config = DomainManager.Information.GetSecretInformationConfig(metaDataId);
			bool isSect = Organization.Instance[character.GetOrganizationInfo().OrgTemplateId].IsSect;
			int punishLevel = 0;
			bool flag = isSect;
			if (flag)
			{
				SecretInformationProcessor processor = DomainManager.Information.SecretInformationProcessorPool.Get();
				bool flag2 = !processor.Initialize(metaDataId);
				if (flag2)
				{
					DomainManager.Information.SecretInformationProcessorPool.Return(processor);
				}
				else
				{
					punishLevel = (int)(processor.CalSectPunishLevel_WithCharId(charId, false) + 1);
					DomainManager.Information.SecretInformationProcessorPool.Return(processor);
				}
			}
			else
			{
				punishLevel = (int)(config.SortValue / 3);
			}
			int baseValue = (int)(config.SortValue * 2);
			baseValue += baseValue * punishLevel * 20 / 100;
			int knownCount = EventHelper.IsSecretInformationBroadcast(metaDataId) ? ((int)SecretInformation.Instance.GetItem(config.TemplateId).MaxPersonAmount) : Math.Min(EventHelper.GetSecretInformationHolderCount(metaDataId), (int)SecretInformation.Instance.GetItem(config.TemplateId).MaxPersonAmount);
			int finalValue = knownCount * 50 / (int)SecretInformation.Instance.GetItem(config.TemplateId).MaxPersonAmount * baseValue / 100;
			return finalValue + finalValue * (int)character.GetFame() / 2 / 100;
		}

		// Token: 0x06000049 RID: 73 RVA: 0x0004F80C File Offset: 0x0004DA0C
		public static int GetLeadToEvilByCombatFavorabilityChange(int charId, int metaDataId)
		{
			return 0;
		}

		// Token: 0x0600004A RID: 74 RVA: 0x0004F820 File Offset: 0x0004DA20
		public static int GetLeadToEvilByCombatBehaviorTypeOffset(int metaDataId)
		{
			SecretInformationItem config = DomainManager.Information.GetSecretInformationConfig(metaDataId);
			return (int)(-(int)(config.SortValue * 50 + 50));
		}

		// Token: 0x0600004B RID: 75 RVA: 0x0004F84C File Offset: 0x0004DA4C
		public static int GetLeadToEvilByRationalityFavorabilityChange(int charId, int metaDataId)
		{
			return 0;
		}

		// Token: 0x0600004C RID: 76 RVA: 0x0004F860 File Offset: 0x0004DA60
		public static int GetLeadToEvilByRationalityBehaviorTypeOffset(int metaDataId)
		{
			SecretInformationItem config = DomainManager.Information.GetSecretInformationConfig(metaDataId);
			return (int)(-(int)(config.SortValue * 50 + 50));
		}

		// Token: 0x0600004D RID: 77 RVA: 0x0004F88C File Offset: 0x0004DA8C
		public static int GetLeadToEvilByEmotionBehaviorTypeOffset(int metaDataId)
		{
			SecretInformationItem config = DomainManager.Information.GetSecretInformationConfig(metaDataId);
			return (int)(-(int)(config.SortValue * 50 + 50));
		}

		// Token: 0x0600004E RID: 78 RVA: 0x0004F8B8 File Offset: 0x0004DAB8
		public static int GetLeadToEvilByEmotionDifficultyValue(int charId, int metaDataId)
		{
			return 0;
		}

		// Token: 0x0600004F RID: 79 RVA: 0x0004F8CC File Offset: 0x0004DACC
		public static int GetLeadToEvilByEmotionEffectValue(int charId, int metaDataId)
		{
			return 0;
		}

		// Token: 0x06000050 RID: 80 RVA: 0x0004F8E0 File Offset: 0x0004DAE0
		public static int WriteOffDebtsByCombatFavorabilityChange(int charId, int metaDataId)
		{
			return 0;
		}

		// Token: 0x06000051 RID: 81 RVA: 0x0004F8F4 File Offset: 0x0004DAF4
		public static int WriteOffDebtsByCombatDebtsWorth(int charId, int metaDataId)
		{
			Character character = DomainManager.Character.GetElement_Objects(charId);
			SecretInformationItem config = DomainManager.Information.GetSecretInformationConfig(metaDataId);
			return (int)((config.SortValue - 1) * 50) * SecretInformationConfig.WriteOffDebtsByCombatDebtsBehaviorTypeParam[(int)character.GetBehaviorType()] + 2000;
		}

		// Token: 0x06000052 RID: 82 RVA: 0x0004F940 File Offset: 0x0004DB40
		public static int WriteOffDebtsByRationalityFavorabilityChange(int charId, int metaDataId)
		{
			return 0;
		}

		// Token: 0x06000053 RID: 83 RVA: 0x0004F954 File Offset: 0x0004DB54
		public static int WriteOffDebtsByRationalityDebtsWorth(int charId, int metaDataId)
		{
			Character character = DomainManager.Character.GetElement_Objects(charId);
			SecretInformationItem config = DomainManager.Information.GetSecretInformationConfig(metaDataId);
			return (int)((config.SortValue - 1) * 50) * SecretInformationConfig.WriteOffDebtsByRationalityDebtsBehaviorTypeParam[(int)character.GetBehaviorType()] + 2000;
		}

		// Token: 0x06000054 RID: 84 RVA: 0x0004F9A0 File Offset: 0x0004DBA0
		public static int WriteOffDebtsByEmotionDebtsWorth(int charId, int metaDataId)
		{
			Character character = DomainManager.Character.GetElement_Objects(charId);
			SecretInformationItem config = DomainManager.Information.GetSecretInformationConfig(metaDataId);
			return (int)((config.SortValue - 1) * 50) * SecretInformationConfig.WriteOffDebtsByEmotionDebtsBehaviorTypeParam[(int)character.GetBehaviorType()] + 2000;
		}

		// Token: 0x06000055 RID: 85 RVA: 0x0004F9EC File Offset: 0x0004DBEC
		public static int WriteOffDebtsByEmotionDifficultyValue(int charId, int metaDataId)
		{
			return 0;
		}

		// Token: 0x06000056 RID: 86 RVA: 0x0004FA00 File Offset: 0x0004DC00
		public static int WriteOffDebtsByEmotionEffectValue(int charId, int metaDataId)
		{
			return 0;
		}

		// Token: 0x0400001D RID: 29
		private static readonly int[] LeadToGoodByCombatBehaviorTypeParam = new int[]
		{
			20,
			18,
			16,
			14,
			12
		};

		// Token: 0x0400001E RID: 30
		private static readonly int[] LeadToGoodByRationalityBehaviorTypeParam = new int[]
		{
			12,
			14,
			16,
			18,
			20
		};

		// Token: 0x0400001F RID: 31
		private static readonly int[] LeadToGoodByEmotionBehaviorTypeParam = new int[]
		{
			18,
			6,
			12,
			6,
			18
		};

		// Token: 0x04000020 RID: 32
		private static readonly int[] LeadToGoodByEmotionBehaviorTypeCorrectionParam = new int[]
		{
			-400,
			-800,
			-600,
			-800,
			-400
		};

		// Token: 0x04000021 RID: 33
		private static readonly int[] LeadToEvilByCombatBehaviorTypeParam = new int[]
		{
			20,
			18,
			16,
			14,
			12
		};

		// Token: 0x04000022 RID: 34
		private static readonly int[] LeadToEvilByRationalityBehaviorTypeParam = new int[]
		{
			12,
			14,
			16,
			18,
			20
		};

		// Token: 0x04000023 RID: 35
		private static readonly int[] GetLeadToEvilByEmotionBehaviorTypeParam = new int[]
		{
			18,
			6,
			12,
			6,
			18
		};

		// Token: 0x04000024 RID: 36
		private static readonly int[] GetLeadToEvilByEmotionBehaviorTypeCorrectionParam = new int[]
		{
			-400,
			-800,
			-600,
			-800,
			-400
		};

		// Token: 0x04000025 RID: 37
		private static readonly int[] WriteOffDebtsByCombatFavorBehaviorTypeParam = new int[]
		{
			20,
			18,
			16,
			14,
			12
		};

		// Token: 0x04000026 RID: 38
		private static readonly int[] WriteOffDebtsByCombatDebtsBehaviorTypeParam = new int[]
		{
			12,
			14,
			16,
			18,
			20
		};

		// Token: 0x04000027 RID: 39
		private static readonly int[] WriteOffDebtsByRationalityFavorBehaviorTypeParam = new int[]
		{
			12,
			14,
			16,
			18,
			20
		};

		// Token: 0x04000028 RID: 40
		private static readonly int[] WriteOffDebtsByRationalityDebtsBehaviorTypeParam = new int[]
		{
			20,
			18,
			16,
			14,
			12
		};

		// Token: 0x04000029 RID: 41
		private static readonly int[] WriteOffDebtsByEmotionDebtsBehaviorTypeParam = new int[]
		{
			12,
			20,
			16,
			20,
			12
		};

		// Token: 0x0400002A RID: 42
		private static readonly int[] WriteOffDebtsByEmotionBehaviorTypeParam = new int[]
		{
			18,
			6,
			12,
			6,
			18
		};

		// Token: 0x0400002B RID: 43
		private static readonly int[] WriteOffDebtsByEmotionBehaviorTypeCorrectionParam = new int[]
		{
			-400,
			-800,
			-600,
			-800,
			-400
		};

		// Token: 0x0400002C RID: 44
		public static readonly int GradeFactor_StartRelation_Difficulty = 6;

		// Token: 0x0400002D RID: 45
		public static readonly int[] BehaviorAdjust_StartRelation_Difficulty = new int[]
		{
			18,
			6,
			12,
			6,
			18
		};

		// Token: 0x0400002E RID: 46
		public static readonly int[] FavorFactor_StartRelation_Difficulty = new int[]
		{
			0,
			-300,
			-200,
			300,
			-100
		};

		// Token: 0x0400002F RID: 47
		public static readonly int SortValueFactor_StartRelation_Effect = 4;

		// Token: 0x04000030 RID: 48
		public static readonly int HolderCountFactor_StartRelation_Effect = 50;

		// Token: 0x04000031 RID: 49
		public static readonly int[] FavorFactor_StartRelation_Effect = new int[]
		{
			0,
			300,
			200,
			-300,
			100
		};

		// Token: 0x04000032 RID: 50
		public static readonly int FameDenominator_StartRelation_Effect = 2;

		// Token: 0x04000033 RID: 51
		public static readonly int GradeFactor_EndRelation_Difficulty = 6;

		// Token: 0x04000034 RID: 52
		public static readonly int[] BehaviorAdjust_EndRelation_Difficulty = new int[]
		{
			18,
			6,
			12,
			6,
			18
		};

		// Token: 0x04000035 RID: 53
		public static readonly int[] FavorFactor_EndRelation_Difficulty = new int[]
		{
			100,
			300,
			200,
			-300,
			0
		};

		// Token: 0x04000036 RID: 54
		public static readonly int SortValueFactor_EndRelation_Effect = 4;

		// Token: 0x04000037 RID: 55
		public static readonly int HolderCountFactor_EndRelation_Effect = 50;

		// Token: 0x04000038 RID: 56
		public static readonly int[] FavorFactor_EndRelation_Effect = new int[]
		{
			100,
			300,
			200,
			-300,
			0
		};

		// Token: 0x04000039 RID: 57
		public static readonly int FameDenominator_EndRelation_Effect = 2;

		// Token: 0x0400003A RID: 58
		public static readonly int[] ReduceDebtBehaviorFactorOfThreatening = new int[]
		{
			14,
			18,
			20,
			16,
			12
		};
	}
}
