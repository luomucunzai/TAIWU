using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using GameData.Domains.Character;
using GameData.Domains.Map;
using GameData.Domains.Organization;
using GameData.Utilities;

namespace GameData.Domains.TaiwuEvent.FunctionDefinition
{
	// Token: 0x020000A9 RID: 169
	public class OrganizationFunctions
	{
		// Token: 0x06001B21 RID: 6945 RVA: 0x0017B1B0 File Offset: 0x001793B0
		[EventFunction(58)]
		private static void SetSectAllowLearning(EventScriptRuntime runtime, Settlement settlement)
		{
			Sect sect = settlement as Sect;
			bool flag = sect == null;
			if (!flag)
			{
				sect.SetTaiwuExploreStatus(2, runtime.Context);
			}
		}

		// Token: 0x06001B22 RID: 6946 RVA: 0x0017B1E0 File Offset: 0x001793E0
		[EventFunction(59)]
		private static void JoinOrganization(EventScriptRuntime runtime, Character character, Settlement settlement, sbyte grade)
		{
			OrganizationInfo targetOrgInfo = default(OrganizationInfo);
			targetOrgInfo.OrgTemplateId = settlement.GetOrgTemplateId();
			targetOrgInfo.Grade = grade;
			targetOrgInfo.SettlementId = settlement.GetId();
			targetOrgInfo.Principal = true;
			DomainManager.Organization.ChangeOrganization(runtime.Context, character, targetOrgInfo);
		}

		// Token: 0x06001B23 RID: 6947 RVA: 0x0017B234 File Offset: 0x00179434
		[EventFunction(60)]
		private static void SetSectCharApprovedTaiwu(EventScriptRuntime runtime, Character character, bool approved)
		{
			int charId = character.GetId();
			DomainManager.Character.TryCreateRelation(runtime.Context, charId, DomainManager.Taiwu.GetTaiwuCharId());
			SectCharacter sectChar = DomainManager.Organization.GetElement_SectCharacters(charId);
			sectChar.SetApprovedTaiwu(runtime.Context, approved);
		}

		// Token: 0x06001B24 RID: 6948 RVA: 0x0017B27F File Offset: 0x0017947F
		[EventFunction(55)]
		private static void ChangeSafetyForSettlement(EventScriptRuntime runtime, Settlement settlement, int delta)
		{
			settlement.ChangeSafety(runtime.Context, delta);
		}

		// Token: 0x06001B25 RID: 6949 RVA: 0x0017B290 File Offset: 0x00179490
		[EventFunction(56)]
		private static void ChangeCultureForSettlement(EventScriptRuntime runtime, Settlement settlement, int delta)
		{
			settlement.ChangeCulture(runtime.Context, delta);
		}

		// Token: 0x06001B26 RID: 6950 RVA: 0x0017B2A4 File Offset: 0x001794A4
		[EventFunction(227)]
		private static void RegisterSettlementMemberFeature(EventScriptRuntime runtime, Settlement settlement, short featureId, sbyte minGrade = 0, sbyte maxGrade = 8)
		{
			OrgMemberCollection members = settlement.GetMembers();
			for (sbyte grade = 0; grade <= 8; grade += 1)
			{
				HashSet<int> gradeMembers = members.GetMembers(grade);
				bool flag = grade < minGrade || grade > maxGrade;
				if (!flag)
				{
					foreach (int charId in gradeMembers)
					{
						Character character = DomainManager.Character.GetElement_Objects(charId);
						character.AddFeature(runtime.Context, featureId, false);
					}
				}
			}
			DomainManager.Extra.RegisterSettlementMemberFeature(runtime.Context, settlement.GetId(), new SettlementMemberFeature
			{
				FeatureId = featureId,
				MinGrade = minGrade,
				MaxGrade = maxGrade
			});
		}

		// Token: 0x06001B27 RID: 6951 RVA: 0x0017B37C File Offset: 0x0017957C
		[EventFunction(226)]
		private static void SetSectSpiritualDebtInteractionOccurred(EventScriptRuntime runtime, sbyte orgTemplateId)
		{
			Settlement settlement = DomainManager.Organization.GetSettlementByOrgTemplateId(orgTemplateId);
			Sect sect = settlement as Sect;
			bool flag = sect == null;
			if (flag)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(24, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Unable to cast ");
				defaultInterpolatedStringHandler.AppendFormatted<Settlement>(settlement);
				defaultInterpolatedStringHandler.AppendLiteral(" to Sect.");
				throw new InvalidCastException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			sect.SetSpiritualDebtInteractionOccurred(true, runtime.Context);
		}

		// Token: 0x06001B28 RID: 6952 RVA: 0x0017B3F0 File Offset: 0x001795F0
		[EventFunction(243)]
		private static short GetMapBlockSettlement(EventScriptRuntime runtime, MapBlockData mapBlock)
		{
			Location rootLocation = mapBlock.GetRootBlock().GetLocation();
			Settlement settlement = DomainManager.Organization.GetSettlementByLocation(rootLocation);
			return settlement.GetId();
		}

		// Token: 0x06001B29 RID: 6953 RVA: 0x0017B420 File Offset: 0x00179620
		[EventFunction(192)]
		private static short GetRandomSettlementInState(EventScriptRuntime runtime, sbyte stateTemplateId, EOrganizationSettlementType settlementType)
		{
			sbyte stateId = DomainManager.Map.GetStateIdByStateTemplateId((short)stateTemplateId);
			List<short> settlementIds = ObjectPool<List<short>>.Instance.Get();
			DomainManager.Map.GetStateSettlementIds(stateId, settlementIds, true, true);
			bool flag = settlementType != EOrganizationSettlementType.Invalid;
			if (flag)
			{
				for (int i = settlementIds.Count - 1; i >= 0; i--)
				{
					short settlementId = settlementIds[i];
					Settlement settlement = DomainManager.Organization.GetSettlement(settlementId);
					bool flag2 = settlement.OrganizationConfig.SettlementType != settlementType;
					if (flag2)
					{
						CollectionUtils.SwapAndRemove<short>(settlementIds, i);
					}
				}
			}
			bool flag3 = settlementIds.Count == 0;
			short result;
			if (flag3)
			{
				result = -1;
			}
			else
			{
				short randomSettlementId = settlementIds.GetRandom(runtime.Context.Random);
				ObjectPool<List<short>>.Instance.Return(settlementIds);
				result = randomSettlementId;
			}
			return result;
		}

		// Token: 0x06001B2A RID: 6954 RVA: 0x0017B4F4 File Offset: 0x001796F4
		[EventFunction(217)]
		private static ShortList GetSettlementListInState(EventScriptRuntime runtime, sbyte stateTemplateId, EOrganizationSettlementType settlementType)
		{
			sbyte stateId = DomainManager.Map.GetStateIdByStateTemplateId((short)stateTemplateId);
			ShortList shortList = ShortList.Create();
			List<short> settlementIds = shortList.Items;
			DomainManager.Map.GetStateSettlementIds(stateId, settlementIds, true, true);
			bool flag = settlementType != EOrganizationSettlementType.Invalid;
			if (flag)
			{
				for (int i = settlementIds.Count - 1; i >= 0; i--)
				{
					short settlementId = settlementIds[i];
					Settlement settlement = DomainManager.Organization.GetSettlement(settlementId);
					bool flag2 = settlement.OrganizationConfig.SettlementType != settlementType;
					if (flag2)
					{
						CollectionUtils.SwapAndRemove<short>(settlementIds, i);
					}
				}
			}
			return shortList;
		}

		// Token: 0x06001B2B RID: 6955 RVA: 0x0017B598 File Offset: 0x00179798
		[EventFunction(263)]
		private static short GetCharacterSettlement(EventScriptRuntime runtime, Character character)
		{
			return character.GetOrganizationInfo().SettlementId;
		}
	}
}
