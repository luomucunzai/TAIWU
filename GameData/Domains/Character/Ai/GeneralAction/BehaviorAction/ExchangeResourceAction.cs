using System;
using GameData.Common;
using GameData.Domains.LifeRecord;
using GameData.Domains.Map;

namespace GameData.Domains.Character.Ai.GeneralAction.BehaviorAction
{
	// Token: 0x020008AA RID: 2218
	public class ExchangeResourceAction : IGeneralAction
	{
		// Token: 0x170004E3 RID: 1251
		// (get) Token: 0x06007893 RID: 30867 RVA: 0x00464EA4 File Offset: 0x004630A4
		public sbyte ActionEnergyType
		{
			get
			{
				return 3;
			}
		}

		// Token: 0x06007894 RID: 30868 RVA: 0x00464EA8 File Offset: 0x004630A8
		public bool CheckValid(Character selfChar, Character targetChar)
		{
			return selfChar.IsInRegularSettlementRange() && selfChar.GetResource(this.SpentResourceType) >= this.SpentResourceAmount;
		}

		// Token: 0x06007895 RID: 30869 RVA: 0x00464EDC File Offset: 0x004630DC
		public void ApplyInitialChangesForTaiwu(DataContext context, Character selfChar, Character targetChar)
		{
			throw new Exception("Current action requires no targetChar.");
		}

		// Token: 0x06007896 RID: 30870 RVA: 0x00464EEC File Offset: 0x004630EC
		public void ApplyChanges(DataContext context, Character selfChar, Character targetChar)
		{
			selfChar.ChangeResource(context, this.SpentResourceType, -this.SpentResourceAmount);
			selfChar.ChangeResource(context, this.GainResourceType, this.GainResourceAmount);
			LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
			int currDate = DomainManager.World.GetCurrDate();
			Location location = selfChar.GetValidLocation();
			Location settlementLocation = DomainManager.Map.GetBelongSettlementBlock(location).GetLocation();
			lifeRecordCollection.AddExchangeResource(selfChar.GetId(), currDate, -1, settlementLocation, this.SpentResourceType, this.SpentResourceAmount, this.GainResourceType, this.GainResourceAmount);
		}

		// Token: 0x04002184 RID: 8580
		public sbyte SpentResourceType;

		// Token: 0x04002185 RID: 8581
		public int SpentResourceAmount;

		// Token: 0x04002186 RID: 8582
		public sbyte GainResourceType;

		// Token: 0x04002187 RID: 8583
		public int GainResourceAmount;
	}
}
