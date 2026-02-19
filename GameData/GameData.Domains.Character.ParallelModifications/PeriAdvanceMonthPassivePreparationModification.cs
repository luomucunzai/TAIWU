using System.Collections.Generic;
using GameData.Domains.Item;
using GameData.Domains.Map;

namespace GameData.Domains.Character.ParallelModifications;

public class PeriAdvanceMonthPassivePreparationModification
{
	public Character Character;

	public List<(MapBlockData block, ItemKey itemKey, int amount)> ItemsToBeLost;

	public bool PersonalNeedsChanged;

	public PeriAdvanceMonthPassivePreparationModification(Character character)
	{
		Character = character;
	}
}
