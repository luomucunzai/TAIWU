using System;
using GameData.Common;
using GameData.Domains.LifeRecord;
using GameData.Domains.Map;

namespace GameData.Domains.Character.Ai.GeneralAction.LifeSkillRandom
{
	// Token: 0x02000898 RID: 2200
	public class LifeSkillTeaWineAction : IGeneralAction
	{
		// Token: 0x170004D1 RID: 1233
		// (get) Token: 0x06007839 RID: 30777 RVA: 0x00462FCE File Offset: 0x004611CE
		public sbyte ActionEnergyType
		{
			get
			{
				return 4;
			}
		}

		// Token: 0x0600783A RID: 30778 RVA: 0x00462FD4 File Offset: 0x004611D4
		public bool CheckValid(Character selfChar, Character targetChar)
		{
			return true;
		}

		// Token: 0x0600783B RID: 30779 RVA: 0x00462FE7 File Offset: 0x004611E7
		public void ApplyInitialChangesForTaiwu(DataContext context, Character selfChar, Character targetChar)
		{
			throw new Exception("Current action requires no targetChar.");
		}

		// Token: 0x0600783C RID: 30780 RVA: 0x00462FF4 File Offset: 0x004611F4
		public void ApplyChanges(DataContext context, Character selfChar, Character targetChar)
		{
			int selfCharId = selfChar.GetId();
			LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
			int currDate = DomainManager.World.GetCurrDate();
			Location location = selfChar.GetLocation();
			bool flag = this.TeaWineTemplateId >= 0;
			if (flag)
			{
				selfChar.CreateInventoryItem(context, 9, this.TeaWineTemplateId, this.Amount);
				lifeRecordCollection.AddCollectTeaWineSucceed(selfCharId, currDate, location, 9, this.TeaWineTemplateId);
			}
			else
			{
				lifeRecordCollection.AddCollectTeaWineFail(selfCharId, currDate, location);
			}
		}

		// Token: 0x0400215B RID: 8539
		public short TeaWineTemplateId;

		// Token: 0x0400215C RID: 8540
		public int Amount;
	}
}
