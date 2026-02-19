namespace GameData.Domains.Adventure.AdventureMap;

public abstract class AdvMapNode
{
	public int Index = -1;

	public ENodeType NodeType;

	public int TerrainId;

	public (sbyte, int) NodeContent = (-1, -1);

	public sbyte SevenElementType = -1;

	public sbyte SevenElementCost;

	public short LifeSkillType = -1;

	public short LifeSkillRequiredVal = 0;

	public sbyte BranchIndex;

	public abstract AdvMapPos AdjustedPos { get; }

	public AdvMapNode(ENodeType nodeType)
	{
		NodeType = nodeType;
	}

	public AdventureMapPoint ToAdventureMapPoint()
	{
		AdventureMapPoint adventureMapPoint = new AdventureMapPoint();
		AdvMapPos adjustedPos = AdjustedPos;
		adventureMapPoint.PosX = adjustedPos.X;
		adventureMapPoint.PosY = adjustedPos.Y;
		adventureMapPoint.NodeType = (sbyte)NodeType;
		adventureMapPoint.TerrainId = TerrainId;
		adventureMapPoint.NodeContentType = NodeContent.Item1;
		adventureMapPoint.NodeContentIndex = NodeContent.Item2;
		adventureMapPoint.SevenElementType = SevenElementType;
		adventureMapPoint.SevenElementCost = SevenElementCost;
		adventureMapPoint.JudgeSkill = LifeSkillType;
		adventureMapPoint.JudgeValue = LifeSkillRequiredVal;
		adventureMapPoint.AffiliatedBranchIdx = BranchIndex;
		adventureMapPoint.Index = (short)Index;
		adventureMapPoint.JudgeSuccess = false;
		return adventureMapPoint;
	}

	public static float CalcSlope(AdvMapPos pos1, AdvMapPos pos2)
	{
		return (pos1.X == pos2.X) ? float.MaxValue : ((float)(pos1.Y - pos2.Y) / (float)(pos1.X - pos2.X));
	}
}
