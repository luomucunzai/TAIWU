using System.Collections.Generic;

namespace GameData.Domains.Adventure.AdventureMap;

public class AdvMapNodeVertex : AdvMapNode
{
	public string NodeKey;

	public readonly Dictionary<AdvMapNodeVertex, AdvMapBranch> ConnectedBranchDict;

	public AdvMapNodeVertex PrevVertex;

	public AdvMapBranch PrevBranch => (PrevVertex != null) ? ConnectedBranchDict[PrevVertex] : null;

	public ELinkDir EnterDir => ELinkDir.Right;

	public override AdvMapPos AdjustedPos
	{
		get
		{
			if (PrevVertex == null)
			{
				return default(AdvMapPos);
			}
			if (PrevBranch.BranchLength <= 0 || PrevBranch.LastNode == null)
			{
				return PrevVertex.AdjustedPos + EnterDir.Offset(2);
			}
			if (PrevBranch.AnchorByStart)
			{
				return PrevBranch.LastNode.AdjustedPos + EnterDir.Offset((NodeType == ENodeType.Start) ? 1 : 2);
			}
			AdvMapPos offset = PrevBranch.LastNode.Offset;
			offset = new AdvMapPos(-offset.X, -offset.Y);
			return PrevVertex.AdjustedPos + EnterDir.Offset((PrevVertex.NodeType == ENodeType.Start) ? 1 : 2) + EnterDir.Reverse().Rotate(offset) + EnterDir.Offset((NodeType == ENodeType.Start) ? 1 : 2);
		}
	}

	public AdvMapNodeVertex(ENodeType nodeType, int index, int terrainId, string nodeKey)
		: base(nodeType)
	{
		Index = index;
		ConnectedBranchDict = new Dictionary<AdvMapNodeVertex, AdvMapBranch>();
		TerrainId = terrainId;
		NodeKey = nodeKey;
	}
}
