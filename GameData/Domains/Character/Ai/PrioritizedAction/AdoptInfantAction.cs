using System;
using GameData.Common;
using GameData.Domains.Character.Relation;
using GameData.Domains.Information.Collection;
using GameData.Domains.LifeRecord;
using GameData.Domains.Map;
using GameData.Serializer;

namespace GameData.Domains.Character.Ai.PrioritizedAction
{
	// Token: 0x02000852 RID: 2130
	[SerializableGameData(NotForDisplayModule = true)]
	public class AdoptInfantAction : BasePrioritizedAction
	{
		// Token: 0x1700048E RID: 1166
		// (get) Token: 0x060076A1 RID: 30369 RVA: 0x00457A7E File Offset: 0x00455C7E
		public override short ActionType
		{
			get
			{
				return 10;
			}
		}

		// Token: 0x060076A2 RID: 30370 RVA: 0x00457A84 File Offset: 0x00455C84
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
				bool flag2 = DomainManager.Character.InfantHasPotentialAdopter(this.Target.TargetCharId) && !DomainManager.Character.IsCharacterPotentialInfantAdopter(this.Target.TargetCharId, selfChar.GetId());
				if (flag2)
				{
					result = false;
				}
				else
				{
					Character target;
					bool flag3 = !DomainManager.Character.TryGetElement_Objects(this.Target.TargetCharId, out target);
					if (flag3)
					{
						result = false;
					}
					else
					{
						bool flag4 = target.GetAgeGroup() != 0;
						if (flag4)
						{
							result = false;
						}
						else
						{
							bool flag5 = selfChar.GetLocation().AreaId != target.GetLocation().AreaId;
							result = (!flag5 && (target.GetLeaderId() < 0 || target.GetLeaderId() == selfChar.GetLeaderId()));
						}
					}
				}
			}
			return result;
		}

		// Token: 0x060076A3 RID: 30371 RVA: 0x00457B64 File Offset: 0x00455D64
		public override void OnStart(DataContext context, Character selfChar)
		{
			bool flag = selfChar.GetLeaderId() != selfChar.GetId();
			if (flag)
			{
				DomainManager.Character.LeaveGroup(context, selfChar, true);
			}
			DomainManager.LifeRecord.GetLifeRecordCollection().AddDecideToAdoptFoundling(selfChar.GetId(), DomainManager.World.GetCurrDate(), this.Target.TargetCharId, selfChar.GetLocation());
			DomainManager.Character.AddPotentialAdopterToInfant(this.Target.TargetCharId, selfChar.GetId());
		}

		// Token: 0x060076A4 RID: 30372 RVA: 0x00457BE4 File Offset: 0x00455DE4
		public override void OnInterrupt(DataContext context, Character selfChar)
		{
			bool flag = DomainManager.Character.IsCharacterPotentialInfantAdopter(this.Target.TargetCharId, selfChar.GetId());
			if (flag)
			{
				DomainManager.Character.RemovePotentialAdopterToInfant(this.Target.TargetCharId);
			}
			DomainManager.LifeRecord.GetLifeRecordCollection().AddAdoptFoundlingFail(selfChar.GetId(), DomainManager.World.GetCurrDate(), this.Target.TargetCharId, selfChar.GetLocation());
		}

		// Token: 0x060076A5 RID: 30373 RVA: 0x00457C58 File Offset: 0x00455E58
		public override bool Execute(DataContext context, Character selfChar)
		{
			int charId = selfChar.GetId();
			int targetCharId = this.Target.TargetCharId;
			Character targetChar;
			DomainManager.Character.TryGetElement_Objects(targetCharId, out targetChar);
			int leaderId = targetChar.GetLeaderId();
			bool flag = leaderId >= 0 && leaderId == selfChar.GetLeaderId();
			bool result;
			if (flag)
			{
				selfChar.DeactivateAdvanceMonthStatus(7);
				bool hasNoRelation = RelationTypeHelper.AllowAddingAdoptiveChildRelation(charId, targetCharId);
				int currDate = DomainManager.World.GetCurrDate();
				Location location = selfChar.GetLocation();
				LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
				SecretInformationCollection secretInformationCollection = DomainManager.Information.GetSecretInformationCollection();
				bool flag2 = hasNoRelation;
				int secretInfoOffset;
				if (flag2)
				{
					int spouseId = DomainManager.Character.GetAliveSpouse(charId);
					DomainManager.Character.AddAdoptiveParentRelations(context, targetCharId, charId, currDate);
					bool flag3 = spouseId >= 0;
					if (flag3)
					{
						DomainManager.Character.AddAdoptiveParentRelations(context, targetCharId, spouseId, currDate);
					}
					lifeRecordCollection.AddAdoptFoundlingSucceed(charId, currDate, targetCharId, location);
					secretInfoOffset = secretInformationCollection.AddAdoptChild(charId, targetCharId);
				}
				else
				{
					lifeRecordCollection.AddClaimFoundlingSucceed(charId, currDate, targetCharId, location);
					secretInfoOffset = secretInformationCollection.AddRetrieveChild(charId, targetCharId);
				}
				DomainManager.Information.AddSecretInformationMetaData(context, secretInfoOffset, true);
				DomainManager.Character.MarkInfantAsAdopted(targetCharId);
				DomainManager.Character.ChangeFavorabilityOptionalMonthlyEvolution(context, selfChar, targetChar, 12000);
				DomainManager.Character.ChangeFavorabilityOptionalMonthlyEvolution(context, targetChar, selfChar, 12000);
				selfChar.ChangeHappiness(context, (int)AiHelper.RelationsRelatedConstants.BecomeSwornOrAdoptedFamilyHappinessChange[(int)selfChar.GetBehaviorType()]);
				targetChar.ChangeHappiness(context, (int)AiHelper.RelationsRelatedConstants.BecomeSwornOrAdoptedFamilyHappinessChange[(int)targetChar.GetBehaviorType()]);
				result = true;
			}
			else
			{
				targetChar.SetHealth(targetChar.GetLeftMaxHealth(false), context);
				DomainManager.Character.JoinGroup(context, targetChar, selfChar);
				selfChar.ActivateAdvanceMonthStatus(7);
				result = false;
			}
			return result;
		}
	}
}
