using System;

namespace GameData.Domains.TaiwuEvent.EventManager;

public static class AdventureCharacterSortUtils
{
	public static void Sort(EventArgBox eventArgBox, bool isMajorChar, int groupId, CharacterSortType characterSortType, bool ascendingOrder)
	{
		if (characterSortType == CharacterSortType.CombatPower)
		{
			Sort(eventArgBox, isMajorChar, groupId, CompareCombatPower, ascendingOrder);
		}
	}

	public static void Sort(EventArgBox eventArgBox, bool isMajorChar, int groupId, Comparison<int> comparison, bool ascendingOrder)
	{
		int num = (isMajorChar ? eventArgBox.GetAdventureMajorCharacterCount(groupId) : eventArgBox.GetAdventureParticipateCharacterCount(groupId));
		string value = (isMajorChar ? "MajorCharacter" : "ParticipateCharacter");
		int arg = -1;
		int arg2 = -1;
		for (int i = 0; i < num - 1; i++)
		{
			for (int j = 0; j < num - i - 1; j++)
			{
				eventArgBox.Get($"{value}_{groupId}_{j}", ref arg);
				eventArgBox.Get($"{value}_{groupId}_{j + 1}", ref arg2);
				int num2 = comparison(arg, arg2);
				if (!ascendingOrder)
				{
					num2 = -num2;
				}
				if (num2 > 0)
				{
					int num3 = arg;
					arg = arg2;
					arg2 = num3;
					eventArgBox.Set($"{value}_{groupId}_{j}", arg);
					eventArgBox.Set($"{value}_{groupId}_{j + 1}", arg2);
				}
			}
		}
	}

	public static int CompareCombatPower(int charIdA, int charIdB)
	{
		DomainManager.Character.TryGetElement_Objects(charIdA, out var element);
		DomainManager.Character.TryGetElement_Objects(charIdB, out var element2);
		int num = element?.GetCombatPower() ?? 0;
		int value = element2?.GetCombatPower() ?? 0;
		return num.CompareTo(value);
	}
}
