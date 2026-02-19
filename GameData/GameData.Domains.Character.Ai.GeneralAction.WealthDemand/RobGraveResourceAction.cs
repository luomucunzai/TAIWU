using System;
using GameData.Common;
using GameData.Domains.Information.Collection;
using GameData.Domains.LifeRecord;
using GameData.Domains.Map;
using GameData.Domains.World.Notification;

namespace GameData.Domains.Character.Ai.GeneralAction.WealthDemand;

public class RobGraveResourceAction : IGeneralAction
{
	public sbyte ResourceType;

	public int Amount;

	public bool Succeed;

	public int TargetGraveId;

	public sbyte ActionEnergyType => 1;

	public unsafe bool CheckValid(Character selfChar, Character targetChar)
	{
		if (!DomainManager.Character.TryGetElement_Graves(TargetGraveId, out var element))
		{
			return false;
		}
		ResourceInts resources = element.GetResources();
		return resources.Items[ResourceType] >= Amount;
	}

	public void ApplyInitialChangesForTaiwu(DataContext context, Character selfChar, Character targetChar)
	{
		throw new Exception("cannot be digging the current Taiwu's grave when he or she is still alive.");
	}

	public unsafe void ApplyChanges(DataContext context, Character selfChar, Character targetChar)
	{
		int id = selfChar.GetId();
		MonthlyNotificationCollection monthlyNotificationCollection = DomainManager.World.GetMonthlyNotificationCollection();
		LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
		Location location = selfChar.GetLocation();
		int currDate = DomainManager.World.GetCurrDate();
		int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
		if (id != taiwuCharId)
		{
			selfChar.ChangeCurrMainAttribute(context, 3, -GlobalConfig.Instance.HarmfulActionCost);
		}
		if (DomainManager.Character.IsTaiwuPeople(TargetGraveId))
		{
			monthlyNotificationCollection.AddDigResource(id, location, TargetGraveId, ResourceType);
		}
		if (Succeed)
		{
			Grave element_Graves = DomainManager.Character.GetElement_Graves(TargetGraveId);
			selfChar.ChangeResource(context, ResourceType, Amount);
			ResourceInts resources = element_Graves.GetResources();
			ref int reference = ref resources.Items[ResourceType];
			reference -= Amount;
			element_Graves.SetResources(ref resources, context);
			lifeRecordCollection.AddRobResourceFromGraveSucceed(id, currDate, TargetGraveId, location, ResourceType, Amount);
			SecretInformationCollection secretInformationCollection = DomainManager.Information.GetSecretInformationCollection();
			int dataOffset = secretInformationCollection.AddRobGraveResource(id, TargetGraveId, ResourceType);
			DomainManager.Information.AddSecretInformationMetaData(context, dataOffset);
		}
		else
		{
			lifeRecordCollection.AddRobResourceFromGraveFail(id, currDate, TargetGraveId, location, ResourceType);
		}
	}
}
