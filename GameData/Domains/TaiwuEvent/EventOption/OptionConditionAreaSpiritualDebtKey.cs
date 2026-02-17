using System;
using Config;
using GameData.Domains.Character;
using GameData.Domains.Map;

namespace GameData.Domains.TaiwuEvent.EventOption
{
	// Token: 0x020000C1 RID: 193
	public class OptionConditionAreaSpiritualDebtKey : TaiwuEventOptionConditionBase
	{
		// Token: 0x06001BD2 RID: 7122 RVA: 0x0017E1C9 File Offset: 0x0017C3C9
		public OptionConditionAreaSpiritualDebtKey(short id, string key, short spiritualDebtValue, Func<MapAreaData, short, bool> checkFunc) : base(id)
		{
			this.AreaArgBoxKey = key;
			this.SpiritualDebtValue = spiritualDebtValue;
			this.ConditionChecker = checkFunc;
		}

		// Token: 0x06001BD3 RID: 7123 RVA: 0x0017E1EC File Offset: 0x0017C3EC
		public override bool CheckCondition(EventArgBox box)
		{
			GameData.Domains.Character.Character taiwuChar = DomainManager.Taiwu.GetTaiwu();
			Location location = taiwuChar.GetLocation();
			MapAreaData areaData = DomainManager.Map.GetElement_Areas((int)location.AreaId);
			return this.ConditionChecker(areaData, this.SpiritualDebtValue);
		}

		// Token: 0x06001BD4 RID: 7124 RVA: 0x0017E234 File Offset: 0x0017C434
		public override ValueTuple<short, string[]> GetDisplayData(EventArgBox box)
		{
			string areaName = string.Empty;
			GameData.Domains.Character.Character taiwuChar = DomainManager.Taiwu.GetTaiwu();
			Location location = taiwuChar.GetLocation();
			MapAreaData areaData = DomainManager.Map.GetElement_Areas((int)location.AreaId);
			MapAreaItem areaConfig = MapArea.Instance.GetItem(areaData.GetTemplateId());
			bool flag = areaConfig != null;
			if (flag)
			{
				areaName = areaConfig.Name;
			}
			return new ValueTuple<short, string[]>(this.Id, new string[]
			{
				areaName,
				this.SpiritualDebtValue.ToString()
			});
		}

		// Token: 0x04000679 RID: 1657
		[Obsolete]
		public readonly string AreaArgBoxKey;

		// Token: 0x0400067A RID: 1658
		public readonly short SpiritualDebtValue;

		// Token: 0x0400067B RID: 1659
		public readonly Func<MapAreaData, short, bool> ConditionChecker;
	}
}
