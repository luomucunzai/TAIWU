using System;
using GameData.Common;
using GameData.Domains.LifeRecord;
using GameData.Domains.Map;

namespace GameData.Domains.Character.Ai.GeneralAction.BehaviorAction
{
	// Token: 0x020008A3 RID: 2211
	public class GainExpByStrollAction : IGeneralAction
	{
		// Token: 0x170004DC RID: 1244
		// (get) Token: 0x06007870 RID: 30832 RVA: 0x00464826 File Offset: 0x00462A26
		public sbyte ActionEnergyType
		{
			get
			{
				return 3;
			}
		}

		// Token: 0x06007871 RID: 30833 RVA: 0x0046482C File Offset: 0x00462A2C
		public bool CheckValid(Character selfChar, Character targetChar)
		{
			return true;
		}

		// Token: 0x06007872 RID: 30834 RVA: 0x0046483F File Offset: 0x00462A3F
		public void ApplyInitialChangesForTaiwu(DataContext context, Character selfChar, Character targetChar)
		{
			throw new Exception("Current action requires no targetChar.");
		}

		// Token: 0x06007873 RID: 30835 RVA: 0x0046484C File Offset: 0x00462A4C
		public void ApplyChanges(DataContext context, Character selfChar, Character targetChar)
		{
			int selfCharId = selfChar.GetId();
			int currDate = DomainManager.World.GetCurrDate();
			Location location = selfChar.GetLocation();
			selfChar.ChangeExp(context, this.ExpGain);
			LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
			lifeRecordCollection.AddGainExpByStroll(selfCharId, currDate, location);
		}

		// Token: 0x04002174 RID: 8564
		public int ExpGain;
	}
}
