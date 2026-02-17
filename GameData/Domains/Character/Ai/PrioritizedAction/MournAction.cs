using System;
using GameData.Common;
using GameData.Domains.Information.Collection;
using GameData.Domains.LifeRecord;
using GameData.Domains.Map;
using GameData.Serializer;

namespace GameData.Domains.Character.Ai.PrioritizedAction
{
	// Token: 0x02000860 RID: 2144
	[SerializableGameData(NotForDisplayModule = true)]
	public class MournAction : BasePrioritizedAction
	{
		// Token: 0x1700049B RID: 1179
		// (get) Token: 0x0600770B RID: 30475 RVA: 0x0045A96B File Offset: 0x00458B6B
		public override short ActionType
		{
			get
			{
				return 4;
			}
		}

		// Token: 0x0600770C RID: 30476 RVA: 0x0045A970 File Offset: 0x00458B70
		public override bool CheckValid(Character selfChar)
		{
			bool flag = !base.CheckValid(selfChar);
			Grave grave;
			return !flag && DomainManager.Character.TryGetElement_Graves(this.Target.TargetCharId, out grave);
		}

		// Token: 0x0600770D RID: 30477 RVA: 0x0045A9AC File Offset: 0x00458BAC
		public override void OnStart(DataContext context, Character selfChar)
		{
			int selfCharId = selfChar.GetId();
			int groupLeader = selfChar.GetLeaderId();
			bool flag = groupLeader >= 0 && groupLeader != selfCharId;
			if (flag)
			{
				DomainManager.Character.LeaveGroup(context, selfChar, true);
			}
			LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
			int currDate = DomainManager.World.GetCurrDate();
			Location location = selfChar.GetLocation();
			lifeRecordCollection.AddDecideToMourn(selfCharId, currDate, this.Target.TargetCharId, location);
		}

		// Token: 0x0600770E RID: 30478 RVA: 0x0045AA20 File Offset: 0x00458C20
		public override void OnInterrupt(DataContext context, Character selfChar)
		{
			int selfCharId = selfChar.GetId();
			LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
			int currDate = DomainManager.World.GetCurrDate();
			Location location = selfChar.GetLocation();
			lifeRecordCollection.AddFinishMourning(selfCharId, currDate, this.Target.TargetCharId, location);
		}

		// Token: 0x0600770F RID: 30479 RVA: 0x0045AA68 File Offset: 0x00458C68
		public override void OnArrival(DataContext context, Character selfChar)
		{
			base.OnArrival(context, selfChar);
			int selfCharId = selfChar.GetId();
			Grave grave;
			bool flag = !DomainManager.Character.TryGetElement_Graves(this.Target.TargetCharId, out grave);
			if (!flag)
			{
				LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
				int currDate = DomainManager.World.GetCurrDate();
				Location location = grave.GetLocation();
				lifeRecordCollection.AddMaintainGrave(selfCharId, currDate, this.Target.TargetCharId, location);
				sbyte graveLevel = grave.GetLevel();
				short maxDurability = GlobalConfig.Instance.GraveDurabilities[(int)graveLevel];
				grave.SetDurability(maxDurability, context);
				SecretInformationCollection secretInformationCollection = DomainManager.Information.GetSecretInformationCollection();
				int secretInfoOffset = secretInformationCollection.AddMourn(selfCharId, this.Target.TargetCharId);
				int secretInfoId = DomainManager.Information.AddSecretInformationMetaData(context, secretInfoOffset, true);
			}
		}

		// Token: 0x06007710 RID: 30480 RVA: 0x0045AB34 File Offset: 0x00458D34
		public override bool Execute(DataContext context, Character selfChar)
		{
			int selfCharId = selfChar.GetId();
			Grave grave;
			bool flag = !DomainManager.Character.TryGetElement_Graves(this.Target.TargetCharId, out grave);
			bool result;
			if (flag)
			{
				result = true;
			}
			else
			{
				sbyte graveLevel = grave.GetLevel();
				bool flag2 = (int)graveLevel < GlobalConfig.Instance.GraveLevelMoneyCosts.Length - 1;
				if (flag2)
				{
					short moneyRequired = GlobalConfig.Instance.GraveLevelMoneyCosts[(int)(graveLevel + 1)];
					int money = selfChar.GetResource(6);
					bool flag3 = money >= (int)moneyRequired;
					if (flag3)
					{
						graveLevel += 1;
						grave.SetLevel(graveLevel, context);
						selfChar.ChangeResource(context, 6, (int)(-(int)moneyRequired));
						LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
						int currDate = DomainManager.World.GetCurrDate();
						Location location = grave.GetLocation();
						lifeRecordCollection.AddUpgradeGrave(selfCharId, currDate, this.Target.TargetCharId, location);
					}
				}
				short maxDurability = GlobalConfig.Instance.GraveDurabilities[(int)graveLevel];
				grave.SetDurability(maxDurability, context);
				result = false;
			}
			return result;
		}
	}
}
