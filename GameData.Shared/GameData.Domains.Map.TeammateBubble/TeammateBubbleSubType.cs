namespace GameData.Domains.Map.TeammateBubble;

public class TeammateBubbleSubType
{
	public const int CloseFriend = 0;

	public const int XuXiaomao = 1;

	public const int GuoYan = 2;

	public const int SituHuanyue = 3;

	public const int ANiu = 4;

	public const int Family = 5;

	public const int Friend = 6;

	public const int Just = 7;

	public const int Kind = 8;

	public const int Even = 9;

	public const int Rebel = 10;

	public const int Egoistic = 11;

	public const int Count = 12;

	public static int GetPriority(int subtype)
	{
		return subtype switch
		{
			6 => 1, 
			5 => 2, 
			1 => 3, 
			2 => 3, 
			3 => 3, 
			4 => 3, 
			0 => 4, 
			_ => 0, 
		};
	}
}
