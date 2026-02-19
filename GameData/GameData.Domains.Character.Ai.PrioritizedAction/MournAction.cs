using GameData.Common;
using GameData.Domains.Information.Collection;
using GameData.Domains.LifeRecord;
using GameData.Domains.Map;
using GameData.Serializer;

namespace GameData.Domains.Character.Ai.PrioritizedAction;

[SerializableGameData(NotForDisplayModule = true)]
public class MournAction : BasePrioritizedAction
{
	public override short ActionType => 4;

	public override bool CheckValid(Character selfChar)
	{
		if (!base.CheckValid(selfChar))
		{
			return false;
		}
		Grave element;
		return DomainManager.Character.TryGetElement_Graves(Target.TargetCharId, out element);
	}

	public override void OnStart(DataContext context, Character selfChar)
	{
		int id = selfChar.GetId();
		int leaderId = selfChar.GetLeaderId();
		if (leaderId >= 0 && leaderId != id)
		{
			DomainManager.Character.LeaveGroup(context, selfChar);
		}
		LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
		int currDate = DomainManager.World.GetCurrDate();
		Location location = selfChar.GetLocation();
		lifeRecordCollection.AddDecideToMourn(id, currDate, Target.TargetCharId, location);
	}

	public override void OnInterrupt(DataContext context, Character selfChar)
	{
		int id = selfChar.GetId();
		LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
		int currDate = DomainManager.World.GetCurrDate();
		Location location = selfChar.GetLocation();
		lifeRecordCollection.AddFinishMourning(id, currDate, Target.TargetCharId, location);
	}

	public override void OnArrival(DataContext context, Character selfChar)
	{
		base.OnArrival(context, selfChar);
		int id = selfChar.GetId();
		if (DomainManager.Character.TryGetElement_Graves(Target.TargetCharId, out var element))
		{
			LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
			int currDate = DomainManager.World.GetCurrDate();
			Location location = element.GetLocation();
			lifeRecordCollection.AddMaintainGrave(id, currDate, Target.TargetCharId, location);
			sbyte level = element.GetLevel();
			short durability = GlobalConfig.Instance.GraveDurabilities[level];
			element.SetDurability(durability, context);
			SecretInformationCollection secretInformationCollection = DomainManager.Information.GetSecretInformationCollection();
			int dataOffset = secretInformationCollection.AddMourn(id, Target.TargetCharId);
			int num = DomainManager.Information.AddSecretInformationMetaData(context, dataOffset);
		}
	}

	public override bool Execute(DataContext context, Character selfChar)
	{
		int id = selfChar.GetId();
		if (!DomainManager.Character.TryGetElement_Graves(Target.TargetCharId, out var element))
		{
			return true;
		}
		sbyte b = element.GetLevel();
		if (b < GlobalConfig.Instance.GraveLevelMoneyCosts.Length - 1)
		{
			short num = GlobalConfig.Instance.GraveLevelMoneyCosts[b + 1];
			int resource = selfChar.GetResource(6);
			if (resource >= num)
			{
				b++;
				element.SetLevel(b, context);
				selfChar.ChangeResource(context, 6, -num);
				LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
				int currDate = DomainManager.World.GetCurrDate();
				Location location = element.GetLocation();
				lifeRecordCollection.AddUpgradeGrave(id, currDate, Target.TargetCharId, location);
			}
		}
		short durability = GlobalConfig.Instance.GraveDurabilities[b];
		element.SetDurability(durability, context);
		return false;
	}
}
