using System;
using System.Runtime.CompilerServices;
using GameData.Common;
using GameData.Domains.Character;
using GameData.Domains.Map;
using GameData.Domains.Organization;

namespace GameData.Domains.TaiwuEvent.EventOption
{
	// Token: 0x020000B1 RID: 177
	public static class OptionConsumeHelper
	{
		// Token: 0x06001B9E RID: 7070 RVA: 0x0017CEF0 File Offset: 0x0017B0F0
		public static bool HasConsumeResource(this OptionConsumeInfo info, int charAId, int charBId)
		{
			bool flag = info.ConsumeType == 8;
			bool result;
			if (flag)
			{
				result = (DomainManager.World.GetLeftDaysInCurrMonth() >= info.ConsumeCount);
			}
			else
			{
				bool flag2 = info.ConsumeType == 9;
				if (flag2)
				{
					Character characterB;
					bool flag3 = DomainManager.Character.TryGetElement_Objects(charBId, out characterB);
					if (flag3)
					{
						bool flag4 = characterB != null;
						if (flag4)
						{
							Settlement settlement = DomainManager.Organization.GetSettlement(characterB.GetOrganizationInfo().SettlementId);
							return DomainManager.Extra.GetAreaSpiritualDebt(settlement.GetLocation().AreaId) >= info.ConsumeCount;
						}
					}
				}
				bool flag5 = info.ConsumeType == 10;
				if (flag5)
				{
					Character characterA;
					bool flag6 = DomainManager.Character.TryGetElement_Objects(charAId, out characterA);
					if (flag6)
					{
						bool flag7 = characterA != null;
						if (flag7)
						{
							return DomainManager.Extra.GetAreaSpiritualDebt(characterA.GetLocation().AreaId) >= info.ConsumeCount;
						}
					}
				}
				Character characterAForResource;
				bool flag8;
				if (DomainManager.Character.TryGetElement_Objects(charAId, out characterAForResource))
				{
					sbyte consumeType = info.ConsumeType;
					flag8 = (consumeType >= 0 && consumeType < 8);
				}
				else
				{
					flag8 = false;
				}
				bool flag9 = flag8;
				if (!flag9)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(30, 1);
					defaultInterpolatedStringHandler.AppendLiteral("character ");
					defaultInterpolatedStringHandler.AppendFormatted<int>(charAId);
					defaultInterpolatedStringHandler.AppendLiteral(" not found exception");
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				result = (characterAForResource.GetResource(info.ConsumeType) >= info.ConsumeCount);
			}
			return result;
		}

		// Token: 0x06001B9F RID: 7071 RVA: 0x0017D074 File Offset: 0x0017B274
		public static int GetHoldCount(this OptionConsumeInfo info, int charAId, int charBId)
		{
			bool flag = info.ConsumeType == 8;
			int result;
			if (flag)
			{
				result = DomainManager.Extra.GetTotalActionPointsRemaining() / 10;
			}
			else
			{
				bool flag2 = info.ConsumeType == 9;
				if (flag2)
				{
					Character characterB;
					bool flag3 = DomainManager.Character.TryGetElement_Objects(charBId, out characterB);
					if (flag3)
					{
						bool flag4 = characterB != null;
						if (flag4)
						{
							Settlement settlement = DomainManager.Organization.GetSettlement(characterB.GetOrganizationInfo().SettlementId);
							return DomainManager.Extra.GetAreaSpiritualDebt(settlement.GetLocation().AreaId);
						}
					}
				}
				bool flag5 = info.ConsumeType == 10;
				if (flag5)
				{
					Character characterA;
					bool flag6 = DomainManager.Character.TryGetElement_Objects(charAId, out characterA);
					if (flag6)
					{
						bool flag7 = characterA != null;
						if (flag7)
						{
							return DomainManager.Extra.GetAreaSpiritualDebt(characterA.GetLocation().AreaId);
						}
					}
				}
				Character characterAForResource;
				bool flag8;
				if (DomainManager.Character.TryGetElement_Objects(charAId, out characterAForResource))
				{
					sbyte consumeType = info.ConsumeType;
					flag8 = (consumeType >= 0 && consumeType < 8);
				}
				else
				{
					flag8 = false;
				}
				bool flag9 = flag8;
				if (!flag9)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(30, 1);
					defaultInterpolatedStringHandler.AppendLiteral("character ");
					defaultInterpolatedStringHandler.AppendFormatted<int>(charAId);
					defaultInterpolatedStringHandler.AppendLiteral(" not found exception");
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				result = characterAForResource.GetResource(info.ConsumeType);
			}
			return result;
		}

		// Token: 0x06001BA0 RID: 7072 RVA: 0x0017D1CC File Offset: 0x0017B3CC
		public static bool DoConsume(this OptionConsumeInfo info, int charIdA, int charIdB)
		{
			bool flag = !info.AutoConsume;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				DataContext context = DomainManager.TaiwuEvent.MainThreadDataContext;
				bool flag2 = info.ConsumeType == 8;
				if (flag2)
				{
					bool flag3 = charIdA != DomainManager.Taiwu.GetTaiwuCharId();
					if (flag3)
					{
						throw new Exception("consume move point can only called by taiwu exception");
					}
					DomainManager.World.AdvanceDaysInMonth(context, info.ConsumeCount);
					result = true;
				}
				else
				{
					bool flag4 = info.ConsumeType == 9;
					if (flag4)
					{
						Character characterB;
						bool flag5 = DomainManager.Character.TryGetElement_Objects(charIdB, out characterB);
						if (flag5)
						{
							bool flag6 = characterB != null;
							if (flag6)
							{
								Settlement settlement = DomainManager.Organization.GetSettlement(characterB.GetOrganizationInfo().SettlementId);
								DataContext dataContext = DomainManager.TaiwuEvent.MainThreadDataContext;
								DomainManager.Extra.ChangeAreaSpiritualDebt(dataContext, settlement.GetLocation().AreaId, -info.ConsumeCount, true, true);
								return true;
							}
						}
					}
					bool flag7 = info.ConsumeType == 10;
					if (flag7)
					{
						Character characterA;
						bool flag8 = DomainManager.Character.TryGetElement_Objects(charIdA, out characterA);
						if (flag8)
						{
							bool flag9 = characterA != null;
							if (flag9)
							{
								DataContext dataContext2 = DomainManager.TaiwuEvent.MainThreadDataContext;
								DomainManager.Extra.ChangeAreaSpiritualDebt(dataContext2, characterA.GetLocation().AreaId, -info.ConsumeCount, true, true);
								return true;
							}
						}
					}
					bool flag10 = info.ConsumeType <= 7;
					if (flag10)
					{
						Character characterA2;
						bool flag11 = DomainManager.Character.TryGetElement_Objects(charIdA, out characterA2);
						if (flag11)
						{
							characterA2.ChangeResource(context, info.ConsumeType, -info.ConsumeCount);
							return true;
						}
					}
					result = false;
				}
			}
			return result;
		}

		// Token: 0x06001BA1 RID: 7073 RVA: 0x0017D370 File Offset: 0x0017B570
		public static OptionConsumeInfo ModifyOptionConsumeInfo(OptionConsumeInfo consumeInfo, EventArgBox argBox)
		{
			sbyte consumeType = consumeInfo.ConsumeType;
			sbyte b = consumeType;
			if (b != 9)
			{
				if (b == 10)
				{
					Location location = DomainManager.Taiwu.GetTaiwu().GetLocation();
					location = DomainManager.Map.GetBlock(location).GetRootBlock().GetLocation();
					short settlementId = DomainManager.Organization.GetSettlementByLocation(location).GetId();
					consumeInfo.ConsumeCount = (int)DomainManager.Taiwu.GetSpiritualDebtFinalCost(settlementId, (short)consumeInfo.ConsumeCount);
				}
			}
			else
			{
				Character character = argBox.GetCharacter("CharacterId");
				short settlementId2 = character.GetOrganizationInfo().SettlementId;
				consumeInfo.ConsumeCount = (int)DomainManager.Taiwu.GetSpiritualDebtFinalCost(settlementId2, (short)consumeInfo.ConsumeCount);
			}
			return consumeInfo;
		}
	}
}
