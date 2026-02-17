using System;
using GameData.Common;
using GameData.Domains.Character.AvatarSystem;
using GameData.Domains.Character.AvatarSystem.AvatarRes;
using GameData.Domains.Information.Collection;
using GameData.Domains.LifeRecord;
using GameData.Domains.Map;
using GameData.Domains.World.MonthlyEvent;
using GameData.Utilities;

namespace GameData.Domains.Character.Ai.GeneralAction.SocialStatusRandom
{
	// Token: 0x0200088A RID: 2186
	public class SocialStatusBarbAction : IGeneralAction
	{
		// Token: 0x170004C3 RID: 1219
		// (get) Token: 0x060077F3 RID: 30707 RVA: 0x0046173B File Offset: 0x0045F93B
		public sbyte ActionEnergyType
		{
			get
			{
				return 4;
			}
		}

		// Token: 0x060077F4 RID: 30708 RVA: 0x00461740 File Offset: 0x0045F940
		public bool CheckValid(Character selfChar, Character targetChar)
		{
			return true;
		}

		// Token: 0x060077F5 RID: 30709 RVA: 0x00461754 File Offset: 0x0045F954
		public void ApplyInitialChangesForTaiwu(DataContext context, Character selfChar, Character targetChar)
		{
			MonthlyEventCollection monthlyEventCollection = DomainManager.World.GetMonthlyEventCollection();
			int selfCharId = selfChar.GetId();
			Location location = targetChar.GetLocation();
			monthlyEventCollection.AddAdviseBarb(selfCharId, location, targetChar.GetId());
			CharacterDomain.AddLockMovementCharSet(selfChar.GetId());
		}

		// Token: 0x060077F6 RID: 30710 RVA: 0x00461798 File Offset: 0x0045F998
		public void ApplyChanges(DataContext context, Character selfChar, Character targetChar)
		{
			int selfCharId = selfChar.GetId();
			int targetCharId = targetChar.GetId();
			int currDate = DomainManager.World.GetCurrDate();
			Location location = selfChar.GetLocation();
			LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
			AvatarData avatar = targetChar.GetAvatar();
			bool succeed = this.Succeed;
			if (succeed)
			{
				AvatarGroup avatarGroup = AvatarManager.Instance.GetAvatarGroup((int)avatar.AvatarId);
				bool flag = avatarGroup.IsHairless(this.TargetFrontHairId, this.TargetBackHairId);
				if (flag)
				{
					avatar.SetGrowableElementShowingState(0, false);
					DomainManager.Character.InitializeAvatarElementGrowthProgress(context, targetCharId, 0);
				}
				else
				{
					avatar.FrontHairId = this.TargetFrontHairId;
					avatar.BackHairId = this.TargetBackHairId;
				}
				bool flag2 = avatarGroup.Beard1Res.CheckIndex(0) && this.TargetBeard1Id == avatarGroup.Beard1Res[0].Id;
				if (flag2)
				{
					avatar.SetGrowableElementShowingState(1, false);
					DomainManager.Character.InitializeAvatarElementGrowthProgress(context, targetCharId, 1);
				}
				else
				{
					avatar.Beard1Id = this.TargetBeard1Id;
				}
				bool flag3 = avatarGroup.Beard2Res.CheckIndex(0) && this.TargetBeard2Id == avatarGroup.Beard2Res[0].Id;
				if (flag3)
				{
					avatar.SetGrowableElementShowingState(2, false);
					DomainManager.Character.InitializeAvatarElementGrowthProgress(context, targetCharId, 2);
				}
				else
				{
					avatar.Beard2Id = this.TargetBeard2Id;
				}
				bool attractionIncreased = this.AttractionIncreased;
				if (attractionIncreased)
				{
					targetChar.AddFeature(context, 496, true);
					DomainManager.Character.ChangeFavorabilityOptionalMonthlyEvolution(context, targetChar, selfChar, 3000);
					lifeRecordCollection.AddBarbSucceed(selfCharId, currDate, targetCharId, location);
					SecretInformationCollection secretInformationCollection = DomainManager.Information.GetSecretInformationCollection();
					int secretInfoOffset = secretInformationCollection.AddRehaircutSuccess(selfCharId, targetCharId);
					int secretInfoId = DomainManager.Information.AddSecretInformationMetaData(context, secretInfoOffset, true);
				}
				else
				{
					targetChar.AddFeature(context, 497, true);
					DomainManager.Character.ChangeFavorabilityOptionalMonthlyEvolution(context, targetChar, selfChar, -1500);
					lifeRecordCollection.AddBarbMistake(selfCharId, currDate, targetCharId, location);
					SecretInformationCollection secretInformationCollection2 = DomainManager.Information.GetSecretInformationCollection();
					int secretInfoOffset2 = secretInformationCollection2.AddRehaircutIncompleted(selfCharId, targetCharId);
					int secretInfoId2 = DomainManager.Information.AddSecretInformationMetaData(context, secretInfoOffset2, true);
				}
			}
			else
			{
				avatar.ResetGrowableElementShowingState(0);
				avatar.ResetGrowableElementShowingState(1);
				avatar.ResetGrowableElementShowingState(2);
				avatar.ResetGrowableElementShowingState(6);
				DomainManager.Character.InitializeAvatarElementGrowthProgress(context, targetCharId, 0);
				DomainManager.Character.InitializeAvatarElementGrowthProgress(context, targetCharId, 1);
				DomainManager.Character.InitializeAvatarElementGrowthProgress(context, targetCharId, 2);
				DomainManager.Character.InitializeAvatarElementGrowthProgress(context, targetCharId, 6);
				targetChar.AddFeature(context, 498, true);
				DomainManager.Character.ChangeFavorabilityOptionalMonthlyEvolution(context, selfChar, targetChar, -3000);
				lifeRecordCollection.AddBarbFail(selfCharId, currDate, targetCharId, location);
				SecretInformationCollection secretInformationCollection3 = DomainManager.Information.GetSecretInformationCollection();
				int secretInfoOffset3 = secretInformationCollection3.AddRehaircutFail(selfCharId, targetCharId);
				int secretInfoId3 = DomainManager.Information.AddSecretInformationMetaData(context, secretInfoOffset3, true);
			}
			targetChar.SetAvatar(avatar, context);
		}

		// Token: 0x04002139 RID: 8505
		public bool Succeed;

		// Token: 0x0400213A RID: 8506
		public bool AttractionIncreased;

		// Token: 0x0400213B RID: 8507
		public short TargetFrontHairId;

		// Token: 0x0400213C RID: 8508
		public short TargetBackHairId;

		// Token: 0x0400213D RID: 8509
		public short TargetBeard1Id;

		// Token: 0x0400213E RID: 8510
		public short TargetBeard2Id;
	}
}
