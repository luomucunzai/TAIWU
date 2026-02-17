using System;
using GameData.Common;
using GameData.Domains.Adventure;
using GameData.Domains.LifeRecord;
using GameData.Domains.Map;
using GameData.Serializer;

namespace GameData.Domains.Character.Ai.PrioritizedAction
{
	// Token: 0x0200085A RID: 2138
	[SerializableGameData(NotForDisplayModule = true)]
	public class FindSpecialMaterialAction : BasePrioritizedAction
	{
		// Token: 0x17000495 RID: 1173
		// (get) Token: 0x060076E4 RID: 30436 RVA: 0x0045964D File Offset: 0x0045784D
		public override short ActionType
		{
			get
			{
				return 7;
			}
		}

		// Token: 0x060076E5 RID: 30437 RVA: 0x00459650 File Offset: 0x00457850
		public override bool CheckValid(Character selfChar)
		{
			bool flag = !base.CheckValid(selfChar);
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				Location location = this.Target.GetRealTargetLocation();
				AreaAdventureData adventuresInArea = DomainManager.Adventure.GetAdventuresInArea(location.AreaId);
				AdventureSiteData site;
				result = (adventuresInArea.AdventureSites.TryGetValue(location.BlockId, out site) && site.IsMaterialResource());
			}
			return result;
		}

		// Token: 0x060076E6 RID: 30438 RVA: 0x004596B4 File Offset: 0x004578B4
		public override void OnStart(DataContext context, Character selfChar)
		{
			int selfCharId = selfChar.GetId();
			LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
			int currDate = DomainManager.World.GetCurrDate();
			Location currLocation = selfChar.GetLocation();
			Location targetLocation = this.Target.GetRealTargetLocation();
			lifeRecordCollection.AddDecideToFindSpecialMaterial(selfCharId, currDate, currLocation, targetLocation);
		}

		// Token: 0x060076E7 RID: 30439 RVA: 0x00459700 File Offset: 0x00457900
		public override void OnInterrupt(DataContext context, Character selfChar)
		{
			int selfCharId = selfChar.GetId();
			LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
			int currDate = DomainManager.World.GetCurrDate();
			Location currLocation = selfChar.GetLocation();
			lifeRecordCollection.AddFinishFIndingSpecialMaterial(selfCharId, currDate, currLocation);
		}

		// Token: 0x060076E8 RID: 30440 RVA: 0x0045973C File Offset: 0x0045793C
		public override bool Execute(DataContext context, Character selfChar)
		{
			return false;
		}
	}
}
