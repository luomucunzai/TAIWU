using GameData.Common;
using GameData.Domains.Character.AvatarSystem;
using GameData.Domains.Character.AvatarSystem.AvatarRes;
using GameData.Domains.Information.Collection;
using GameData.Domains.LifeRecord;
using GameData.Domains.Map;
using GameData.Domains.World.MonthlyEvent;
using GameData.Utilities;

namespace GameData.Domains.Character.Ai.GeneralAction.SocialStatusRandom;

public class SocialStatusBarbAction : IGeneralAction
{
	public bool Succeed;

	public bool AttractionIncreased;

	public short TargetFrontHairId;

	public short TargetBackHairId;

	public short TargetBeard1Id;

	public short TargetBeard2Id;

	public sbyte ActionEnergyType => 4;

	public bool CheckValid(Character selfChar, Character targetChar)
	{
		return true;
	}

	public void ApplyInitialChangesForTaiwu(DataContext context, Character selfChar, Character targetChar)
	{
		MonthlyEventCollection monthlyEventCollection = DomainManager.World.GetMonthlyEventCollection();
		int id = selfChar.GetId();
		Location location = targetChar.GetLocation();
		monthlyEventCollection.AddAdviseBarb(id, location, targetChar.GetId());
		CharacterDomain.AddLockMovementCharSet(selfChar.GetId());
	}

	public void ApplyChanges(DataContext context, Character selfChar, Character targetChar)
	{
		int id = selfChar.GetId();
		int id2 = targetChar.GetId();
		int currDate = DomainManager.World.GetCurrDate();
		Location location = selfChar.GetLocation();
		LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
		AvatarData avatar = targetChar.GetAvatar();
		if (Succeed)
		{
			AvatarGroup avatarGroup = AvatarManager.Instance.GetAvatarGroup(avatar.AvatarId);
			if (avatarGroup.IsHairless(TargetFrontHairId, TargetBackHairId))
			{
				avatar.SetGrowableElementShowingState(0, show: false);
				DomainManager.Character.InitializeAvatarElementGrowthProgress(context, id2, 0);
			}
			else
			{
				avatar.FrontHairId = TargetFrontHairId;
				avatar.BackHairId = TargetBackHairId;
			}
			if (avatarGroup.Beard1Res.CheckIndex(0) && TargetBeard1Id == avatarGroup.Beard1Res[0].Id)
			{
				avatar.SetGrowableElementShowingState(1, show: false);
				DomainManager.Character.InitializeAvatarElementGrowthProgress(context, id2, 1);
			}
			else
			{
				avatar.Beard1Id = TargetBeard1Id;
			}
			if (avatarGroup.Beard2Res.CheckIndex(0) && TargetBeard2Id == avatarGroup.Beard2Res[0].Id)
			{
				avatar.SetGrowableElementShowingState(2, show: false);
				DomainManager.Character.InitializeAvatarElementGrowthProgress(context, id2, 2);
			}
			else
			{
				avatar.Beard2Id = TargetBeard2Id;
			}
			if (AttractionIncreased)
			{
				targetChar.AddFeature(context, 496, removeMutexFeature: true);
				DomainManager.Character.ChangeFavorabilityOptionalMonthlyEvolution(context, targetChar, selfChar, 3000);
				lifeRecordCollection.AddBarbSucceed(id, currDate, id2, location);
				SecretInformationCollection secretInformationCollection = DomainManager.Information.GetSecretInformationCollection();
				int dataOffset = secretInformationCollection.AddRehaircutSuccess(id, id2);
				int num = DomainManager.Information.AddSecretInformationMetaData(context, dataOffset);
			}
			else
			{
				targetChar.AddFeature(context, 497, removeMutexFeature: true);
				DomainManager.Character.ChangeFavorabilityOptionalMonthlyEvolution(context, targetChar, selfChar, -1500);
				lifeRecordCollection.AddBarbMistake(id, currDate, id2, location);
				SecretInformationCollection secretInformationCollection2 = DomainManager.Information.GetSecretInformationCollection();
				int dataOffset2 = secretInformationCollection2.AddRehaircutIncompleted(id, id2);
				int num2 = DomainManager.Information.AddSecretInformationMetaData(context, dataOffset2);
			}
		}
		else
		{
			avatar.ResetGrowableElementShowingState(0);
			avatar.ResetGrowableElementShowingState(1);
			avatar.ResetGrowableElementShowingState(2);
			avatar.ResetGrowableElementShowingState(6);
			DomainManager.Character.InitializeAvatarElementGrowthProgress(context, id2, 0);
			DomainManager.Character.InitializeAvatarElementGrowthProgress(context, id2, 1);
			DomainManager.Character.InitializeAvatarElementGrowthProgress(context, id2, 2);
			DomainManager.Character.InitializeAvatarElementGrowthProgress(context, id2, 6);
			targetChar.AddFeature(context, 498, removeMutexFeature: true);
			DomainManager.Character.ChangeFavorabilityOptionalMonthlyEvolution(context, selfChar, targetChar, -3000);
			lifeRecordCollection.AddBarbFail(id, currDate, id2, location);
			SecretInformationCollection secretInformationCollection3 = DomainManager.Information.GetSecretInformationCollection();
			int dataOffset3 = secretInformationCollection3.AddRehaircutFail(id, id2);
			int num3 = DomainManager.Information.AddSecretInformationMetaData(context, dataOffset3);
		}
		targetChar.SetAvatar(avatar, context);
	}
}
