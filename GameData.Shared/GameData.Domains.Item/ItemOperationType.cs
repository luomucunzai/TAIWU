namespace GameData.Domains.Item;

public static class ItemOperationType
{
	public enum EItemOperationType
	{
		Invalid = -1,
		Repair,
		Disassemble,
		Transfer,
		Take,
		Discard,
		SpecialBreakConvertToExp,
		Feeding,
		Confiscate,
		PutPoisonMaterial,
		ExchangeTools,
		FixItem,
		VillagerCraft,
		PutCraftResource
	}

	public const sbyte Invalid = -1;

	public const sbyte Repair = 0;

	public const sbyte Disassemble = 1;

	public const sbyte Transfer = 2;

	public const sbyte Take = 3;

	public const sbyte Discard = 4;

	public const sbyte SpecialBreakConvertToExp = 5;

	public const sbyte Feeding = 6;

	public const sbyte Confiscate = 7;

	public const sbyte PutPoisonMaterial = 8;

	public const sbyte ExchangeTools = 9;

	public const sbyte FixItem = 10;

	public const sbyte VillagerCraft = 11;

	public const sbyte PutCraftResource = 12;
}
