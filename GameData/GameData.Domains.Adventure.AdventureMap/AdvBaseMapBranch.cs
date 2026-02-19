using System;
using System.Collections.Generic;
using System.Linq;

namespace GameData.Domains.Adventure.AdventureMap;

public abstract class AdvBaseMapBranch
{
	public int BranchIndex = -1;

	public readonly List<AdvMapNodeNormal> Nodes = new List<AdvMapNodeNormal>();

	public AdvMapBranch BaseBranch { get; protected set; }

	public virtual AdvMapNodeNormal FirstNode { get; protected set; }

	public virtual AdvMapNodeNormal LastNode { get; protected set; }
}
public abstract class AdvBaseMapBranch<T> : AdvBaseMapBranch where T : AdvMapNode
{
	public T EnterNode { get; protected init; }

	public T ExitNode { get; protected init; }

	public AdvMapNode GetNode(AdvMapPos pos)
	{
		if (pos.Equals(EnterNode.AdjustedPos))
		{
			return EnterNode;
		}
		if (pos.Equals(ExitNode.AdjustedPos))
		{
			return ExitNode;
		}
		return Nodes.FirstOrDefault((AdvMapNodeNormal a) => a.AdjustedPos.Equals(pos));
	}

	public AdvMapPos GetOffset(AdvMapPos src)
	{
		return Nodes.First((AdvMapNodeNormal a) => a.AdjustedPos.Equals(src)).Offset;
	}

	public List<AdvMapNodeNormal> FindNextNodes(AdvMapNodeNormal src)
	{
		if (!Nodes.Contains(src))
		{
			throw new Exception("This node doesn't belong to the branch");
		}
		return Nodes.Where((AdvMapNodeNormal a) => a.Offset.Equals(src.Offset + new AdvMapPos(2, 0)) || a.Offset.Equals(src.Offset + new AdvMapPos(1, 1)) || a.Offset.Equals(src.Offset + new AdvMapPos(1, -1))).ToList();
	}
}
