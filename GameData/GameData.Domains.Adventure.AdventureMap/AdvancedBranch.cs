namespace GameData.Domains.Adventure.AdventureMap;

public class AdvancedBranch : AdvBaseMapBranch<AdvMapNodeNormal>
{
	public override AdvMapNodeNormal FirstNode => Nodes[0];

	public override AdvMapNodeNormal LastNode => Nodes[Nodes.Count - 1];

	public sbyte EnterLifeSkillType { get; }

	public short EnterLifeSkillVal { get; }

	public AdvancedBranch(AdvMapNodeNormal enter, AdvMapNodeNormal leave, int idx, AdvMapBranch baseBranch, sbyte enterLifeSkillType, short enterLifeSkillVal)
	{
		base.EnterNode = enter;
		base.ExitNode = leave;
		BranchIndex = idx;
		base.BaseBranch = baseBranch;
		EnterLifeSkillType = enterLifeSkillType;
		EnterLifeSkillVal = enterLifeSkillVal;
	}
}
