using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.Character.Relation;
using GameData.Domains.Taiwu.Profession;

namespace GameData.Domains.Organization
{
	// Token: 0x02000648 RID: 1608
	public abstract class SettlementCharacter : BaseGameDataObject
	{
		// Token: 0x0600483E RID: 18494 RVA: 0x0028C7F1 File Offset: 0x0028A9F1
		protected SettlementCharacter()
		{
		}

		// Token: 0x0600483F RID: 18495 RVA: 0x0028C7FB File Offset: 0x0028A9FB
		protected SettlementCharacter(int charId, sbyte orgTemplateId, short settlementId)
		{
			this.Id = charId;
			this.OrgTemplateId = orgTemplateId;
			this.SettlementId = settlementId;
			this.ApprovedTaiwu = false;
		}

		// Token: 0x06004840 RID: 18496 RVA: 0x0028C824 File Offset: 0x0028AA24
		public void SetApprovedTaiwu(DataContext context, bool approve)
		{
			bool flag = this.ApprovedTaiwu == approve;
			if (!flag)
			{
				this.SetApprovedTaiwu(approve, context);
				Events.RaiseCharacterApproveTaiwuStatusChanged(context, this, approve, true);
			}
		}

		// Token: 0x06004841 RID: 18497 RVA: 0x0028C854 File Offset: 0x0028AA54
		public short GetApprovingRate()
		{
			bool approved = this.ApprovedTaiwu || ProfessionSkillHandle.DukeSkill_CheckCharacterHasTitle(this.Id);
			bool flag = !approved;
			short result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				bool flag2 = DomainManager.Organization.GetPrisonerSect(this.Id) >= 0;
				if (flag2)
				{
					result = 0;
				}
				else
				{
					int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
					RelatedCharacter relation;
					bool flag3 = !DomainManager.Character.TryGetRelation(this.Id, taiwuCharId, out relation);
					if (flag3)
					{
						result = 0;
					}
					else
					{
						bool flag4 = (relation.RelationType & 32768) > 0;
						if (flag4)
						{
							result = 0;
						}
						else
						{
							sbyte favorType = FavorabilityType.GetFavorabilityType(relation.Favorability);
							int approvingRate = (int)(this.InfluencePower * (short)(60 + 10 * favorType) / 100);
							result = (short)approvingRate;
						}
					}
				}
			}
			return result;
		}

		// Token: 0x06004842 RID: 18498 RVA: 0x0028C91C File Offset: 0x0028AB1C
		public short CalcInfluencePower(Character character, short baseInfluencePower, [TupleElementNames(new string[]
		{
			"character",
			"baseInfluencePower"
		})] Dictionary<int, ValueTuple<Character, short>> baseInfluencePowers, HashSet<int> relatedCharIds)
		{
			bool flag = character.IsCompletelyInfected() || character.GetLegendaryBookOwnerState() >= 2;
			short result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				int value = (int)(baseInfluencePower + this.InfluencePowerBonus);
				relatedCharIds.Clear();
				DomainManager.Character.GetAllRelatedCharIds(this.Id, relatedCharIds, true);
				foreach (int relatedCharId in relatedCharIds)
				{
					ValueTuple<Character, short> item;
					bool flag2 = !baseInfluencePowers.TryGetValue(relatedCharId, out item);
					if (!flag2)
					{
						bool flag3 = !DomainManager.Character.IsCharacterAlive(relatedCharId);
						if (!flag3)
						{
							RelatedCharacter relation = DomainManager.Character.GetRelation(relatedCharId, this.Id);
							ushort relationType = relation.RelationType;
							short favorability = relation.Favorability;
							bool flag4 = (favorability > 0 && RelationType.ContainPositiveRelations(relationType)) || (favorability < 0 && RelationType.ContainNegativeRelations(relationType));
							if (flag4)
							{
								value += (int)(item.Item2 * favorability) / 300000;
							}
						}
					}
				}
				result = (short)value;
			}
			return result;
		}

		// Token: 0x06004843 RID: 18499 RVA: 0x0028CA48 File Offset: 0x0028AC48
		public int GetId()
		{
			return this.Id;
		}

		// Token: 0x06004844 RID: 18500 RVA: 0x0028CA60 File Offset: 0x0028AC60
		public sbyte GetOrgTemplateId()
		{
			return this.OrgTemplateId;
		}

		// Token: 0x06004845 RID: 18501 RVA: 0x0028CA78 File Offset: 0x0028AC78
		public short GetSettlementId()
		{
			return this.SettlementId;
		}

		// Token: 0x06004846 RID: 18502
		public abstract void SetSettlementId(short settlementId, DataContext context);

		// Token: 0x06004847 RID: 18503 RVA: 0x0028CA90 File Offset: 0x0028AC90
		public bool GetApprovedTaiwu()
		{
			return this.ApprovedTaiwu;
		}

		// Token: 0x06004848 RID: 18504
		public abstract void SetApprovedTaiwu(bool approvedTaiwu, DataContext context);

		// Token: 0x06004849 RID: 18505 RVA: 0x0028CAA8 File Offset: 0x0028ACA8
		public short GetInfluencePower()
		{
			return this.InfluencePower;
		}

		// Token: 0x0600484A RID: 18506
		public abstract void SetInfluencePower(short influencePower, DataContext context);

		// Token: 0x0600484B RID: 18507 RVA: 0x0028CAC0 File Offset: 0x0028ACC0
		public short GetInfluencePowerBonus()
		{
			return this.InfluencePowerBonus;
		}

		// Token: 0x0600484C RID: 18508
		public abstract void SetInfluencePowerBonus(short influencePowerBonus, DataContext context);

		// Token: 0x0400150B RID: 5387
		[CollectionObjectField(false, true, false, true, false)]
		protected int Id;

		// Token: 0x0400150C RID: 5388
		[CollectionObjectField(false, true, false, true, false)]
		protected sbyte OrgTemplateId;

		// Token: 0x0400150D RID: 5389
		[CollectionObjectField(false, true, false, false, false)]
		protected short SettlementId;

		// Token: 0x0400150E RID: 5390
		[CollectionObjectField(false, true, false, false, false)]
		protected bool ApprovedTaiwu;

		// Token: 0x0400150F RID: 5391
		[CollectionObjectField(false, true, false, false, false)]
		protected short InfluencePower;

		// Token: 0x04001510 RID: 5392
		[CollectionObjectField(false, true, false, false, false)]
		protected short InfluencePowerBonus;
	}
}
