using System;
using System.Runtime.CompilerServices;
using GameData.Utilities;

namespace GameData.Domains.Character.Relation
{
	// Token: 0x02000824 RID: 2084
	public class RelationTypeHelper
	{
		// Token: 0x06007558 RID: 30040 RVA: 0x00449A98 File Offset: 0x00447C98
		public static ushort AddRelation(RelatedCharacters relatedChars, int relatedCharId, ushort oriTypes, ushort addingType)
		{
			Tester.Assert(addingType > 0, "");
			for (sbyte i = 0; i < 3; i += 1)
			{
				bool flag = RelationType.ContainBloodRelatedRelations(addingType, i) && RelationType.ContainBloodRelatedRelations(oriTypes, i);
				if (flag)
				{
					ushort oriBloodRelatedType = RelationType.GetBloodRelatedRelation(oriTypes, i);
					bool flag2 = addingType < oriBloodRelatedType;
					ushort result;
					if (flag2)
					{
						relatedChars.Remove(relatedCharId, oriBloodRelatedType);
						relatedChars.Add(relatedCharId, addingType);
						result = ((oriTypes & ~oriBloodRelatedType) | addingType);
					}
					else
					{
						result = oriTypes;
					}
					return result;
				}
			}
			relatedChars.Add(relatedCharId, addingType);
			return oriTypes | addingType;
		}

		// Token: 0x06007559 RID: 30041 RVA: 0x00449B28 File Offset: 0x00447D28
		public static bool AllowAddingRelation(int charId, int relatedCharId, ushort addingType)
		{
			if (addingType <= 256)
			{
				if (addingType <= 16)
				{
					switch (addingType)
					{
					case 1:
						return RelationTypeHelper.AllowAddingBloodRelation(charId, relatedCharId, 1);
					case 2:
						return false;
					case 3:
						break;
					case 4:
						return false;
					default:
						if (addingType == 8)
						{
							return RelationTypeHelper.AllowAddingStepRelation(charId, relatedCharId, 8);
						}
						if (addingType == 16)
						{
							return false;
						}
						break;
					}
				}
				else if (addingType <= 64)
				{
					if (addingType == 32)
					{
						return false;
					}
					if (addingType == 64)
					{
						return RelationTypeHelper.AllowAddingAdoptiveRelation(charId, relatedCharId, 64);
					}
				}
				else
				{
					if (addingType == 128)
					{
						return false;
					}
					if (addingType == 256)
					{
						return false;
					}
				}
			}
			else if (addingType <= 2048)
			{
				if (addingType == 512)
				{
					return RelationTypeHelper.AllowAddingSwornBrotherOrSisterRelation(charId, relatedCharId);
				}
				if (addingType == 1024)
				{
					return RelationTypeHelper.AllowAddingHusbandOrWifeRelation(charId, relatedCharId);
				}
				if (addingType == 2048)
				{
					return RelationTypeHelper.AllowAddingRelation_Direct(charId, relatedCharId, 2048);
				}
			}
			else if (addingType <= 8192)
			{
				if (addingType == 4096)
				{
					return RelationTypeHelper.AllowAddingRelation_Direct(charId, relatedCharId, 4096);
				}
				if (addingType == 8192)
				{
					return RelationTypeHelper.AllowAddingRelation_Direct(charId, relatedCharId, 8192);
				}
			}
			else
			{
				if (addingType == 16384)
				{
					return RelationTypeHelper.AllowAddingAdoredRelation(charId, relatedCharId);
				}
				if (addingType == 32768)
				{
					return RelationTypeHelper.AllowAddingEnemyRelation(charId, relatedCharId);
				}
			}
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(24, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Unsupported addingType: ");
			defaultInterpolatedStringHandler.AppendFormatted<ushort>(addingType);
			throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
		}

		// Token: 0x0600755A RID: 30042 RVA: 0x00449CFC File Offset: 0x00447EFC
		public static bool AllowAddingBloodParentRelation(int charId, int relatedCharId)
		{
			return RelationTypeHelper.AllowAddingBloodRelation(charId, relatedCharId, 1);
		}

		// Token: 0x0600755B RID: 30043 RVA: 0x00449D18 File Offset: 0x00447F18
		public static bool AllowAddingBloodChildRelation(int charId, int relatedCharId)
		{
			return false;
		}

		// Token: 0x0600755C RID: 30044 RVA: 0x00449D2C File Offset: 0x00447F2C
		public static bool AllowAddingBloodBrotherOrSisterRelation(int charId, int relatedCharId)
		{
			return false;
		}

		// Token: 0x0600755D RID: 30045 RVA: 0x00449D40 File Offset: 0x00447F40
		public static bool AllowAddingStepParentRelation(int charId, int relatedCharId)
		{
			return RelationTypeHelper.AllowAddingStepRelation(charId, relatedCharId, 8);
		}

		// Token: 0x0600755E RID: 30046 RVA: 0x00449D5C File Offset: 0x00447F5C
		public static bool AllowAddingStepChildRelation(int charId, int relatedCharId)
		{
			return false;
		}

		// Token: 0x0600755F RID: 30047 RVA: 0x00449D70 File Offset: 0x00447F70
		public static bool AllowAddingStepBrotherOrSisterRelation(int charId, int relatedCharId)
		{
			return false;
		}

		// Token: 0x06007560 RID: 30048 RVA: 0x00449D84 File Offset: 0x00447F84
		public static bool AllowAddingAdoptiveParentRelation(int charId, int relatedCharId)
		{
			return RelationTypeHelper.AllowAddingAdoptiveRelation(charId, relatedCharId, 64);
		}

		// Token: 0x06007561 RID: 30049 RVA: 0x00449DA0 File Offset: 0x00447FA0
		public static bool AllowAddingAdoptiveChildRelation(int charId, int relatedCharId)
		{
			return RelationTypeHelper.AllowAddingAdoptiveRelation(charId, relatedCharId, 128);
		}

		// Token: 0x06007562 RID: 30050 RVA: 0x00449DC0 File Offset: 0x00447FC0
		public static bool AllowAddingAdoptiveBrotherOrSisterRelation(int charId, int relatedCharId)
		{
			return false;
		}

		// Token: 0x06007563 RID: 30051 RVA: 0x00449DD4 File Offset: 0x00447FD4
		public static bool AllowAddingSwornBrotherOrSisterRelation(int charId, int relatedCharId)
		{
			RelatedCharacter relation;
			bool flag = !DomainManager.Character.TryGetRelation(charId, relatedCharId, out relation);
			if (flag)
			{
				relation.RelationType = ushort.MaxValue;
			}
			bool flag2 = DomainManager.Character.HasNominalBloodRelation(charId, relatedCharId, relation);
			bool result;
			if (flag2)
			{
				result = false;
			}
			else
			{
				bool flag3 = relation.RelationType == ushort.MaxValue;
				if (flag3)
				{
					result = true;
				}
				else
				{
					ushort relationTypes = relation.RelationType;
					bool flag4 = relationTypes == 0;
					if (flag4)
					{
						result = true;
					}
					else
					{
						bool flag5 = (relationTypes & 512) > 0;
						if (flag5)
						{
							result = false;
						}
						else
						{
							bool flag6 = RelationType.ContainBloodRelatedRelations(relationTypes);
							result = !flag6;
						}
					}
				}
			}
			return result;
		}

		// Token: 0x06007564 RID: 30052 RVA: 0x00449E74 File Offset: 0x00448074
		public static bool AllowAddingHusbandOrWifeRelation(int charId, int relatedCharId)
		{
			bool flag = DomainManager.Character.GetAliveSpouse(charId) >= 0 || DomainManager.Character.GetAliveSpouse(relatedCharId) >= 0;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				RelatedCharacter relation;
				bool flag2 = !DomainManager.Character.TryGetRelation(charId, relatedCharId, out relation);
				if (flag2)
				{
					relation.RelationType = ushort.MaxValue;
				}
				bool flag3 = DomainManager.Character.HasNominalBloodRelation(charId, relatedCharId, relation);
				if (flag3)
				{
					result = false;
				}
				else
				{
					bool flag4 = relation.RelationType == ushort.MaxValue;
					if (flag4)
					{
						result = true;
					}
					else
					{
						ushort relationTypes = relation.RelationType;
						bool flag5 = relationTypes == 0;
						if (flag5)
						{
							result = true;
						}
						else
						{
							bool flag6 = (relationTypes & 1024) > 0;
							if (flag6)
							{
								result = false;
							}
							else
							{
								bool flag7 = RelationType.ContainBloodExclusionRelations(relationTypes);
								result = !flag7;
							}
						}
					}
				}
			}
			return result;
		}

		// Token: 0x06007565 RID: 30053 RVA: 0x00449F40 File Offset: 0x00448140
		public static bool AllowAddingMentorRelation(int charId, int relatedCharId)
		{
			return RelationTypeHelper.AllowAddingRelation_Direct(charId, relatedCharId, 2048);
		}

		// Token: 0x06007566 RID: 30054 RVA: 0x00449F60 File Offset: 0x00448160
		public static bool AllowAddingMenteeRelation(int charId, int relatedCharId)
		{
			return RelationTypeHelper.AllowAddingRelation_Direct(charId, relatedCharId, 4096);
		}

		// Token: 0x06007567 RID: 30055 RVA: 0x00449F80 File Offset: 0x00448180
		public static bool AllowAddingFriendRelation(int charId, int relatedCharId)
		{
			return RelationTypeHelper.AllowAddingRelation_Direct(charId, relatedCharId, 8192);
		}

		// Token: 0x06007568 RID: 30056 RVA: 0x00449FA0 File Offset: 0x004481A0
		public static bool AllowAddingAdoredRelation(int charId, int relatedCharId)
		{
			bool flag = !RelationTypeHelper.AllowAddingRelation_Direct(charId, relatedCharId, 16384);
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				Character selfChar;
				Character relatedChar;
				bool flag2 = DomainManager.Character.TryGetElement_Objects(charId, out selfChar) && DomainManager.Character.TryGetElement_Objects(relatedCharId, out relatedChar);
				if (flag2)
				{
					bool flag3 = selfChar.GetFeatureIds().Contains(485);
					if (flag3)
					{
						return relatedChar.GetFeatureIds().Contains(484);
					}
					bool flag4 = relatedChar.GetFeatureIds().Contains(485);
					if (flag4)
					{
						return selfChar.GetFeatureIds().Contains(484);
					}
				}
				result = true;
			}
			return result;
		}

		// Token: 0x06007569 RID: 30057 RVA: 0x0044A044 File Offset: 0x00448244
		public static bool AllowAddingEnemyRelation(int charId, int relatedCharId)
		{
			bool flag = !RelationTypeHelper.AllowAddingRelation_Direct(charId, relatedCharId, 32768);
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				Character selfChar;
				Character relatedChar;
				bool flag2 = DomainManager.Character.TryGetElement_Objects(charId, out selfChar) && DomainManager.Character.TryGetElement_Objects(relatedCharId, out relatedChar);
				if (flag2)
				{
					bool flag3 = selfChar.GetFeatureIds().Contains(484);
					if (flag3)
					{
						return relatedChar.GetFeatureIds().Contains(485);
					}
					bool flag4 = relatedChar.GetFeatureIds().Contains(484);
					if (flag4)
					{
						return selfChar.GetFeatureIds().Contains(485);
					}
				}
				result = true;
			}
			return result;
		}

		// Token: 0x0600756A RID: 30058 RVA: 0x0044A0E8 File Offset: 0x004482E8
		private static bool AllowAddingRelation_Direct(int charId, int relatedCharId, ushort addingType)
		{
			RelatedCharacter relation;
			bool flag = !DomainManager.Character.TryGetRelation(charId, relatedCharId, out relation);
			bool result;
			if (flag)
			{
				result = true;
			}
			else
			{
				ushort relationTypes = relation.RelationType;
				bool flag2 = relationTypes == 0;
				if (flag2)
				{
					result = true;
				}
				else
				{
					bool flag3 = (relationTypes & addingType) > 0;
					result = !flag3;
				}
			}
			return result;
		}

		// Token: 0x0600756B RID: 30059 RVA: 0x0044A13C File Offset: 0x0044833C
		private static bool AllowAddingBloodRelation(int charId, int relatedCharId, ushort addingType)
		{
			bool flag = addingType == 1 && DomainManager.Character.HasEnoughBloodParents(charId);
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				RelatedCharacter relation;
				bool flag2 = !DomainManager.Character.TryGetRelation(charId, relatedCharId, out relation);
				if (flag2)
				{
					relation.RelationType = ushort.MaxValue;
				}
				bool flag3 = DomainManager.Character.HasNominalBloodRelation(charId, relatedCharId, relation);
				if (flag3)
				{
					result = false;
				}
				else
				{
					bool flag4 = relation.RelationType == ushort.MaxValue;
					if (flag4)
					{
						result = true;
					}
					else
					{
						ushort relationTypes = relation.RelationType;
						bool flag5 = relationTypes == 0;
						if (flag5)
						{
							result = true;
						}
						else
						{
							bool flag6 = addingType == 1 && RelationType.ContainParentRelations(relationTypes);
							if (flag6)
							{
								result = true;
							}
							else
							{
								bool flag7 = addingType == 2 && RelationType.ContainChildRelations(relationTypes);
								if (flag7)
								{
									result = true;
								}
								else
								{
									bool flag8 = addingType == 4 && RelationType.ContainBrotherOrSisterRelations(relationTypes);
									if (flag8)
									{
										result = true;
									}
									else
									{
										bool flag9 = RelationType.ContainBloodExclusionRelations(relationTypes);
										result = !flag9;
									}
								}
							}
						}
					}
				}
			}
			return result;
		}

		// Token: 0x0600756C RID: 30060 RVA: 0x0044A22C File Offset: 0x0044842C
		private static bool AllowAddingStepRelation(int charId, int relatedCharId, ushort addingType)
		{
			RelatedCharacter relation;
			bool flag = !DomainManager.Character.TryGetRelation(charId, relatedCharId, out relation);
			if (flag)
			{
				relation.RelationType = ushort.MaxValue;
			}
			bool flag2 = DomainManager.Character.HasNominalBloodRelation(charId, relatedCharId, relation);
			bool result;
			if (flag2)
			{
				result = false;
			}
			else
			{
				bool flag3 = relation.RelationType == ushort.MaxValue;
				if (flag3)
				{
					result = true;
				}
				else
				{
					ushort relationTypes = relation.RelationType;
					bool flag4 = relationTypes == 0;
					if (flag4)
					{
						result = true;
					}
					else
					{
						bool flag5 = (relationTypes & addingType) > 0;
						if (flag5)
						{
							result = false;
						}
						else
						{
							bool flag6 = addingType == 8 && (relationTypes & 8) > 0;
							if (flag6)
							{
								result = false;
							}
							else
							{
								bool flag7 = addingType == 16 && (relationTypes & 16) > 0;
								if (flag7)
								{
									result = false;
								}
								else
								{
									bool flag8 = addingType == 32 && (relationTypes & 32) > 0;
									result = !flag8;
								}
							}
						}
					}
				}
			}
			return result;
		}

		// Token: 0x0600756D RID: 30061 RVA: 0x0044A308 File Offset: 0x00448508
		private static bool AllowAddingAdoptiveRelation(int charId, int relatedCharId, ushort addingType)
		{
			RelatedCharacter relation;
			bool flag = !DomainManager.Character.TryGetRelation(charId, relatedCharId, out relation);
			if (flag)
			{
				relation.RelationType = ushort.MaxValue;
			}
			bool flag2 = DomainManager.Character.HasNominalBloodRelation(charId, relatedCharId, relation);
			bool result;
			if (flag2)
			{
				result = false;
			}
			else
			{
				bool flag3 = relation.RelationType == ushort.MaxValue;
				if (flag3)
				{
					result = true;
				}
				else
				{
					ushort relationTypes = relation.RelationType;
					bool flag4 = relationTypes == 0;
					if (flag4)
					{
						result = true;
					}
					else
					{
						bool flag5 = (relationTypes & addingType) > 0;
						if (flag5)
						{
							result = false;
						}
						else
						{
							bool flag6 = RelationType.ContainBloodExclusionRelations(relationTypes);
							result = !flag6;
						}
					}
				}
			}
			return result;
		}
	}
}
