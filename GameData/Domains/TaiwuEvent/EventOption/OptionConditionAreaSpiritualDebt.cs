using System;
using Config;
using GameData.Domains.Character;
using GameData.Domains.Map;
using GameData.Domains.Organization;

namespace GameData.Domains.TaiwuEvent.EventOption
{
	// Token: 0x020000C0 RID: 192
	public class OptionConditionAreaSpiritualDebt : TaiwuEventOptionConditionBase
	{
		// Token: 0x06001BCD RID: 7117 RVA: 0x0017DF8A File Offset: 0x0017C18A
		public OptionConditionAreaSpiritualDebt(short id, short spiritualDebtValue, Func<MapAreaData, short, bool> checkFunc) : base(id)
		{
			this.SpiritualDebtValue = spiritualDebtValue;
			this.ConditionChecker = checkFunc;
		}

		// Token: 0x06001BCE RID: 7118 RVA: 0x0017DFA4 File Offset: 0x0017C1A4
		public override bool CheckCondition(EventArgBox box)
		{
			GameData.Domains.Character.Character character = box.GetCharacter("CharacterId");
			bool flag = character != null;
			if (flag)
			{
				short settlementId = character.GetOrganizationInfo().SettlementId;
				bool flag2 = settlementId >= 0;
				if (flag2)
				{
					Settlement settlement = DomainManager.Organization.GetSettlement(settlementId);
					MapAreaData areaData = DomainManager.Map.GetElement_Areas((int)settlement.GetLocation().AreaId);
					return this.ConditionChecker(areaData, this.SpiritualDebtValue);
				}
			}
			return false;
		}

		// Token: 0x06001BCF RID: 7119 RVA: 0x0017E024 File Offset: 0x0017C224
		public bool CheckCondition(short settlementId)
		{
			bool flag = settlementId >= 0;
			bool result;
			if (flag)
			{
				Settlement settlement = DomainManager.Organization.GetSettlement(settlementId);
				MapAreaData areaData = DomainManager.Map.GetElement_Areas((int)settlement.GetLocation().AreaId);
				result = this.ConditionChecker(areaData, this.SpiritualDebtValue);
			}
			else
			{
				result = false;
			}
			return result;
		}

		// Token: 0x06001BD0 RID: 7120 RVA: 0x0017E07C File Offset: 0x0017C27C
		public override ValueTuple<short, string[]> GetDisplayData(EventArgBox box)
		{
			string areaName = string.Empty;
			GameData.Domains.Character.Character character = box.GetCharacter("CharacterId");
			bool flag = character != null;
			if (flag)
			{
				short settlementId = character.GetOrganizationInfo().SettlementId;
				bool flag2 = settlementId >= 0;
				if (flag2)
				{
					Settlement settlement = DomainManager.Organization.GetSettlement(settlementId);
					MapAreaData areaData = DomainManager.Map.GetElement_Areas((int)settlement.GetLocation().AreaId);
					MapAreaItem areaConfig = MapArea.Instance.GetItem(areaData.GetTemplateId());
					bool flag3 = areaConfig != null;
					if (flag3)
					{
						areaName = areaConfig.Name;
					}
				}
			}
			return new ValueTuple<short, string[]>(this.Id, new string[]
			{
				areaName,
				this.SpiritualDebtValue.ToString()
			});
		}

		// Token: 0x06001BD1 RID: 7121 RVA: 0x0017E138 File Offset: 0x0017C338
		public ValueTuple<short, string[]> GetDisplayData(short settlementId)
		{
			string areaName = string.Empty;
			bool flag = settlementId >= 0;
			if (flag)
			{
				Settlement settlement = DomainManager.Organization.GetSettlement(settlementId);
				MapAreaData areaData = DomainManager.Map.GetElement_Areas((int)settlement.GetLocation().AreaId);
				MapAreaItem areaConfig = MapArea.Instance.GetItem(areaData.GetTemplateId());
				bool flag2 = areaConfig != null;
				if (flag2)
				{
					areaName = areaConfig.Name;
				}
			}
			return new ValueTuple<short, string[]>(this.Id, new string[]
			{
				areaName,
				this.SpiritualDebtValue.ToString()
			});
		}

		// Token: 0x04000677 RID: 1655
		public readonly short SpiritualDebtValue;

		// Token: 0x04000678 RID: 1656
		public readonly Func<MapAreaData, short, bool> ConditionChecker;
	}
}
