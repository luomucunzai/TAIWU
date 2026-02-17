using System;
using System.Runtime.CompilerServices;
using GameData.Domains.Character;

namespace GameData.Domains.TaiwuEvent.EventManager
{
	// Token: 0x020000C9 RID: 201
	public static class AdventureCharacterSortUtils
	{
		// Token: 0x06001BE7 RID: 7143 RVA: 0x0017E730 File Offset: 0x0017C930
		public static void Sort(EventArgBox eventArgBox, bool isMajorChar, int groupId, CharacterSortType characterSortType, bool ascendingOrder)
		{
			if (characterSortType == CharacterSortType.CombatPower)
			{
				AdventureCharacterSortUtils.Sort(eventArgBox, isMajorChar, groupId, new Comparison<int>(AdventureCharacterSortUtils.CompareCombatPower), ascendingOrder);
			}
		}

		// Token: 0x06001BE8 RID: 7144 RVA: 0x0017E760 File Offset: 0x0017C960
		public static void Sort(EventArgBox eventArgBox, bool isMajorChar, int groupId, Comparison<int> comparison, bool ascendingOrder)
		{
			int count = isMajorChar ? eventArgBox.GetAdventureMajorCharacterCount(groupId) : eventArgBox.GetAdventureParticipateCharacterCount(groupId);
			string prefix = isMajorChar ? "MajorCharacter" : "ParticipateCharacter";
			int charA = -1;
			int charB = -1;
			for (int i = 0; i < count - 1; i++)
			{
				for (int j = 0; j < count - i - 1; j++)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(2, 3);
					defaultInterpolatedStringHandler.AppendFormatted(prefix);
					defaultInterpolatedStringHandler.AppendLiteral("_");
					defaultInterpolatedStringHandler.AppendFormatted<int>(groupId);
					defaultInterpolatedStringHandler.AppendLiteral("_");
					defaultInterpolatedStringHandler.AppendFormatted<int>(j);
					eventArgBox.Get(defaultInterpolatedStringHandler.ToStringAndClear(), ref charA);
					defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(2, 3);
					defaultInterpolatedStringHandler.AppendFormatted(prefix);
					defaultInterpolatedStringHandler.AppendLiteral("_");
					defaultInterpolatedStringHandler.AppendFormatted<int>(groupId);
					defaultInterpolatedStringHandler.AppendLiteral("_");
					defaultInterpolatedStringHandler.AppendFormatted<int>(j + 1);
					eventArgBox.Get(defaultInterpolatedStringHandler.ToStringAndClear(), ref charB);
					int result = comparison(charA, charB);
					bool flag = !ascendingOrder;
					if (flag)
					{
						result = -result;
					}
					bool flag2 = result > 0;
					if (flag2)
					{
						int num = charA;
						charA = charB;
						charB = num;
						defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(2, 3);
						defaultInterpolatedStringHandler.AppendFormatted(prefix);
						defaultInterpolatedStringHandler.AppendLiteral("_");
						defaultInterpolatedStringHandler.AppendFormatted<int>(groupId);
						defaultInterpolatedStringHandler.AppendLiteral("_");
						defaultInterpolatedStringHandler.AppendFormatted<int>(j);
						eventArgBox.Set(defaultInterpolatedStringHandler.ToStringAndClear(), charA);
						defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(2, 3);
						defaultInterpolatedStringHandler.AppendFormatted(prefix);
						defaultInterpolatedStringHandler.AppendLiteral("_");
						defaultInterpolatedStringHandler.AppendFormatted<int>(groupId);
						defaultInterpolatedStringHandler.AppendLiteral("_");
						defaultInterpolatedStringHandler.AppendFormatted<int>(j + 1);
						eventArgBox.Set(defaultInterpolatedStringHandler.ToStringAndClear(), charB);
					}
				}
			}
		}

		// Token: 0x06001BE9 RID: 7145 RVA: 0x0017E948 File Offset: 0x0017CB48
		public static int CompareCombatPower(int charIdA, int charIdB)
		{
			Character charA;
			DomainManager.Character.TryGetElement_Objects(charIdA, out charA);
			Character charB;
			DomainManager.Character.TryGetElement_Objects(charIdB, out charB);
			int charAPower = (charA != null) ? charA.GetCombatPower() : 0;
			int charBPower = (charB != null) ? charB.GetCombatPower() : 0;
			return charAPower.CompareTo(charBPower);
		}
	}
}
