namespace GameData.Domains.Character;

public static class EquipmentSlot
{
	public const sbyte Invalid = -1;

	public const sbyte Weapon1 = 0;

	public const sbyte Weapon2 = 1;

	public const sbyte Weapon3 = 2;

	public const sbyte Helm = 3;

	public const sbyte Clothing = 4;

	public const sbyte Torso = 5;

	public const sbyte Bracers = 6;

	public const sbyte Boots = 7;

	public const sbyte Accessory1 = 8;

	public const sbyte Accessory2 = 9;

	public const sbyte Accessory3 = 10;

	public const sbyte Carrier = 11;

	public const int Count = 12;

	public static readonly sbyte[][] EquipmentType2Slots = new sbyte[8][]
	{
		new sbyte[3] { 0, 1, 2 },
		new sbyte[1] { 3 },
		new sbyte[1] { 4 },
		new sbyte[1] { 5 },
		new sbyte[1] { 6 },
		new sbyte[1] { 7 },
		new sbyte[3] { 8, 9, 10 },
		new sbyte[1] { 11 }
	};
}
