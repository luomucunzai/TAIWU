using GameData.Common;
using Redzen.Random;

namespace GameData.Domains.Adventure.AdventureMap;

public class AdvMapNodeNormal : AdvMapNode
{
	public readonly AdvBaseMapBranch AffiliatedBranch;

	private int _randValue = -1;

	public AdvMapPos Offset { get; set; }

	public override AdvMapPos AdjustedPos => AffiliatedBranch.BaseBranch.BasePos + Offset;

	public AdvMapNodeNormal(int terrainId, AdvBaseMapBranch mapBranch, DataContext context, sbyte sevenElementType)
		: base(ENodeType.Normal)
	{
		TerrainId = terrainId;
		AffiliatedBranch = mapBranch;
		BranchIndex = (sbyte)AffiliatedBranch.BranchIndex;
		SevenElementType = sevenElementType;
		if (SevenElementType != -1)
		{
			SevenElementCost = (sbyte)context.Random.Next((int)GlobalConfig.Instance.AdventureNodePersonalityMinCost, GlobalConfig.Instance.AdventureNodePersonalityMaxCost + 1);
		}
		else
		{
			SevenElementCost = 0;
		}
	}

	internal int GetRandomValue(IRandomSource random)
	{
		if (_randValue != -1)
		{
			return _randValue;
		}
		return _randValue = random.Next(0, 100);
	}
}
