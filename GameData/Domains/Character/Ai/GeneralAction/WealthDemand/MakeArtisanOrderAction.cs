using System;
using GameData.Common;
using GameData.Domains.LifeRecord;
using GameData.Domains.Map;
using GameData.Domains.Organization;
using GameData.Utilities;

namespace GameData.Domains.Character.Ai.GeneralAction.WealthDemand
{
	// Token: 0x02000876 RID: 2166
	public class MakeArtisanOrderAction : IGeneralAction
	{
		// Token: 0x170004AF RID: 1199
		// (get) Token: 0x0600778F RID: 30607 RVA: 0x0045E0F2 File Offset: 0x0045C2F2
		public sbyte ActionEnergyType
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x06007790 RID: 30608 RVA: 0x0045E0F8 File Offset: 0x0045C2F8
		public bool CheckValid(Character selfChar, Character targetChar)
		{
			return OrganizationDomain.GetOrgMemberConfig(targetChar.GetOrganizationInfo()).CraftTypes.Exist(this.LifeSkillType) && DomainManager.Extra.IsArtisanIdle(targetChar.GetId());
		}

		// Token: 0x06007791 RID: 30609 RVA: 0x0045E13A File Offset: 0x0045C33A
		public void ApplyInitialChangesForTaiwu(DataContext context, Character selfChar, Character targetChar)
		{
		}

		// Token: 0x06007792 RID: 30610 RVA: 0x0045E140 File Offset: 0x0045C340
		public void ApplyChanges(DataContext context, Character selfChar, Character targetChar)
		{
			int currDate = DomainManager.World.GetCurrDate();
			Location location = targetChar.GetLocation();
			LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
			Character giftTargetChar;
			bool flag = this.GiftTargetCharId >= 0 && DomainManager.Character.TryGetElement_Objects(this.GiftTargetCharId, out giftTargetChar);
			if (flag)
			{
				DomainManager.Extra.CreateArtisanOrderWithoutCost(context, targetChar, giftTargetChar, this.LifeSkillType, this.ItemSubType);
				lifeRecordCollection.AddOrderProductForOthers(selfChar.GetId(), currDate, targetChar.GetId(), this.GiftTargetCharId, location);
			}
			else
			{
				DomainManager.Extra.CreateArtisanOrderWithoutCost(context, targetChar, selfChar, this.LifeSkillType, this.ItemSubType);
				lifeRecordCollection.AddOrderProduct(selfChar.GetId(), currDate, targetChar.GetId(), location);
			}
		}

		// Token: 0x040020F5 RID: 8437
		public sbyte LifeSkillType;

		// Token: 0x040020F6 RID: 8438
		public short ItemSubType;

		// Token: 0x040020F7 RID: 8439
		public int GiftTargetCharId = -1;
	}
}
