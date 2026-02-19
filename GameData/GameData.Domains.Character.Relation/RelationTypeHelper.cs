using System;
using GameData.Utilities;

namespace GameData.Domains.Character.Relation;

public class RelationTypeHelper
{
	public static ushort AddRelation(RelatedCharacters relatedChars, int relatedCharId, ushort oriTypes, ushort addingType)
	{
		Tester.Assert(addingType != 0);
		for (sbyte b = 0; b < 3; b++)
		{
			if (RelationType.ContainBloodRelatedRelations(addingType, b) && RelationType.ContainBloodRelatedRelations(oriTypes, b))
			{
				ushort bloodRelatedRelation = RelationType.GetBloodRelatedRelation(oriTypes, b);
				if (addingType < bloodRelatedRelation)
				{
					relatedChars.Remove(relatedCharId, bloodRelatedRelation);
					relatedChars.Add(relatedCharId, addingType);
					return (ushort)((oriTypes & ~bloodRelatedRelation) | addingType);
				}
				return oriTypes;
			}
		}
		relatedChars.Add(relatedCharId, addingType);
		return (ushort)(oriTypes | addingType);
	}

	public static bool AllowAddingRelation(int charId, int relatedCharId, ushort addingType)
	{
		return addingType switch
		{
			1 => AllowAddingBloodRelation(charId, relatedCharId, 1), 
			2 => false, 
			4 => false, 
			8 => AllowAddingStepRelation(charId, relatedCharId, 8), 
			16 => false, 
			32 => false, 
			64 => AllowAddingAdoptiveRelation(charId, relatedCharId, 64), 
			128 => false, 
			256 => false, 
			512 => AllowAddingSwornBrotherOrSisterRelation(charId, relatedCharId), 
			1024 => AllowAddingHusbandOrWifeRelation(charId, relatedCharId), 
			2048 => AllowAddingRelation_Direct(charId, relatedCharId, 2048), 
			4096 => AllowAddingRelation_Direct(charId, relatedCharId, 4096), 
			8192 => AllowAddingRelation_Direct(charId, relatedCharId, 8192), 
			16384 => AllowAddingAdoredRelation(charId, relatedCharId), 
			32768 => AllowAddingEnemyRelation(charId, relatedCharId), 
			_ => throw new Exception($"Unsupported addingType: {addingType}"), 
		};
	}

	public static bool AllowAddingBloodParentRelation(int charId, int relatedCharId)
	{
		return AllowAddingBloodRelation(charId, relatedCharId, 1);
	}

	public static bool AllowAddingBloodChildRelation(int charId, int relatedCharId)
	{
		return false;
	}

	public static bool AllowAddingBloodBrotherOrSisterRelation(int charId, int relatedCharId)
	{
		return false;
	}

	public static bool AllowAddingStepParentRelation(int charId, int relatedCharId)
	{
		return AllowAddingStepRelation(charId, relatedCharId, 8);
	}

	public static bool AllowAddingStepChildRelation(int charId, int relatedCharId)
	{
		return false;
	}

	public static bool AllowAddingStepBrotherOrSisterRelation(int charId, int relatedCharId)
	{
		return false;
	}

	public static bool AllowAddingAdoptiveParentRelation(int charId, int relatedCharId)
	{
		return AllowAddingAdoptiveRelation(charId, relatedCharId, 64);
	}

	public static bool AllowAddingAdoptiveChildRelation(int charId, int relatedCharId)
	{
		return AllowAddingAdoptiveRelation(charId, relatedCharId, 128);
	}

	public static bool AllowAddingAdoptiveBrotherOrSisterRelation(int charId, int relatedCharId)
	{
		return false;
	}

	public static bool AllowAddingSwornBrotherOrSisterRelation(int charId, int relatedCharId)
	{
		if (!DomainManager.Character.TryGetRelation(charId, relatedCharId, out var relation))
		{
			relation.RelationType = ushort.MaxValue;
		}
		if (DomainManager.Character.HasNominalBloodRelation(charId, relatedCharId, relation))
		{
			return false;
		}
		if (relation.RelationType == ushort.MaxValue)
		{
			return true;
		}
		ushort relationType = relation.RelationType;
		if (relationType == 0)
		{
			return true;
		}
		if ((relationType & 0x200) != 0)
		{
			return false;
		}
		if (RelationType.ContainBloodRelatedRelations(relationType))
		{
			return false;
		}
		return true;
	}

	public static bool AllowAddingHusbandOrWifeRelation(int charId, int relatedCharId)
	{
		if (DomainManager.Character.GetAliveSpouse(charId) >= 0 || DomainManager.Character.GetAliveSpouse(relatedCharId) >= 0)
		{
			return false;
		}
		if (!DomainManager.Character.TryGetRelation(charId, relatedCharId, out var relation))
		{
			relation.RelationType = ushort.MaxValue;
		}
		if (DomainManager.Character.HasNominalBloodRelation(charId, relatedCharId, relation))
		{
			return false;
		}
		if (relation.RelationType == ushort.MaxValue)
		{
			return true;
		}
		ushort relationType = relation.RelationType;
		if (relationType == 0)
		{
			return true;
		}
		if ((relationType & 0x400) != 0)
		{
			return false;
		}
		if (RelationType.ContainBloodExclusionRelations(relationType))
		{
			return false;
		}
		return true;
	}

	public static bool AllowAddingMentorRelation(int charId, int relatedCharId)
	{
		return AllowAddingRelation_Direct(charId, relatedCharId, 2048);
	}

	public static bool AllowAddingMenteeRelation(int charId, int relatedCharId)
	{
		return AllowAddingRelation_Direct(charId, relatedCharId, 4096);
	}

	public static bool AllowAddingFriendRelation(int charId, int relatedCharId)
	{
		return AllowAddingRelation_Direct(charId, relatedCharId, 8192);
	}

	public static bool AllowAddingAdoredRelation(int charId, int relatedCharId)
	{
		if (!AllowAddingRelation_Direct(charId, relatedCharId, 16384))
		{
			return false;
		}
		if (DomainManager.Character.TryGetElement_Objects(charId, out var element) && DomainManager.Character.TryGetElement_Objects(relatedCharId, out var element2))
		{
			if (element.GetFeatureIds().Contains(485))
			{
				return element2.GetFeatureIds().Contains(484);
			}
			if (element2.GetFeatureIds().Contains(485))
			{
				return element.GetFeatureIds().Contains(484);
			}
		}
		return true;
	}

	public static bool AllowAddingEnemyRelation(int charId, int relatedCharId)
	{
		if (!AllowAddingRelation_Direct(charId, relatedCharId, 32768))
		{
			return false;
		}
		if (DomainManager.Character.TryGetElement_Objects(charId, out var element) && DomainManager.Character.TryGetElement_Objects(relatedCharId, out var element2))
		{
			if (element.GetFeatureIds().Contains(484))
			{
				return element2.GetFeatureIds().Contains(485);
			}
			if (element2.GetFeatureIds().Contains(484))
			{
				return element.GetFeatureIds().Contains(485);
			}
		}
		return true;
	}

	private static bool AllowAddingRelation_Direct(int charId, int relatedCharId, ushort addingType)
	{
		if (!DomainManager.Character.TryGetRelation(charId, relatedCharId, out var relation))
		{
			return true;
		}
		ushort relationType = relation.RelationType;
		if (relationType == 0)
		{
			return true;
		}
		if ((relationType & addingType) != 0)
		{
			return false;
		}
		return true;
	}

	private static bool AllowAddingBloodRelation(int charId, int relatedCharId, ushort addingType)
	{
		if (addingType == 1 && DomainManager.Character.HasEnoughBloodParents(charId))
		{
			return false;
		}
		if (!DomainManager.Character.TryGetRelation(charId, relatedCharId, out var relation))
		{
			relation.RelationType = ushort.MaxValue;
		}
		if (DomainManager.Character.HasNominalBloodRelation(charId, relatedCharId, relation))
		{
			return false;
		}
		if (relation.RelationType == ushort.MaxValue)
		{
			return true;
		}
		ushort relationType = relation.RelationType;
		if (relationType == 0)
		{
			return true;
		}
		if (addingType == 1 && RelationType.ContainParentRelations(relationType))
		{
			return true;
		}
		if (addingType == 2 && RelationType.ContainChildRelations(relationType))
		{
			return true;
		}
		if (addingType == 4 && RelationType.ContainBrotherOrSisterRelations(relationType))
		{
			return true;
		}
		if (RelationType.ContainBloodExclusionRelations(relationType))
		{
			return false;
		}
		return true;
	}

	private static bool AllowAddingStepRelation(int charId, int relatedCharId, ushort addingType)
	{
		if (!DomainManager.Character.TryGetRelation(charId, relatedCharId, out var relation))
		{
			relation.RelationType = ushort.MaxValue;
		}
		if (DomainManager.Character.HasNominalBloodRelation(charId, relatedCharId, relation))
		{
			return false;
		}
		if (relation.RelationType == ushort.MaxValue)
		{
			return true;
		}
		ushort relationType = relation.RelationType;
		if (relationType == 0)
		{
			return true;
		}
		if ((relationType & addingType) != 0)
		{
			return false;
		}
		if (addingType == 8 && (relationType & 8) != 0)
		{
			return false;
		}
		if (addingType == 16 && (relationType & 0x10) != 0)
		{
			return false;
		}
		if (addingType == 32 && (relationType & 0x20) != 0)
		{
			return false;
		}
		return true;
	}

	private static bool AllowAddingAdoptiveRelation(int charId, int relatedCharId, ushort addingType)
	{
		if (!DomainManager.Character.TryGetRelation(charId, relatedCharId, out var relation))
		{
			relation.RelationType = ushort.MaxValue;
		}
		if (DomainManager.Character.HasNominalBloodRelation(charId, relatedCharId, relation))
		{
			return false;
		}
		if (relation.RelationType == ushort.MaxValue)
		{
			return true;
		}
		ushort relationType = relation.RelationType;
		if (relationType == 0)
		{
			return true;
		}
		if ((relationType & addingType) != 0)
		{
			return false;
		}
		if (RelationType.ContainBloodExclusionRelations(relationType))
		{
			return false;
		}
		return true;
	}
}
