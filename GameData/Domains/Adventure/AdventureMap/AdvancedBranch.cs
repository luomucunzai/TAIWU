using System;

namespace GameData.Domains.Adventure.AdventureMap
{
	// Token: 0x020008CB RID: 2251
	public class AdvancedBranch : AdvBaseMapBranch<AdvMapNodeNormal>
	{
		// Token: 0x170004F4 RID: 1268
		// (get) Token: 0x06007F8B RID: 32651 RVA: 0x004CA247 File Offset: 0x004C8447
		public override AdvMapNodeNormal FirstNode
		{
			get
			{
				return this.Nodes[0];
			}
		}

		// Token: 0x170004F5 RID: 1269
		// (get) Token: 0x06007F8C RID: 32652 RVA: 0x004CA255 File Offset: 0x004C8455
		public override AdvMapNodeNormal LastNode
		{
			get
			{
				return this.Nodes[this.Nodes.Count - 1];
			}
		}

		// Token: 0x170004F6 RID: 1270
		// (get) Token: 0x06007F8D RID: 32653 RVA: 0x004CA26F File Offset: 0x004C846F
		public sbyte EnterLifeSkillType { get; }

		// Token: 0x170004F7 RID: 1271
		// (get) Token: 0x06007F8E RID: 32654 RVA: 0x004CA277 File Offset: 0x004C8477
		public short EnterLifeSkillVal { get; }

		// Token: 0x06007F8F RID: 32655 RVA: 0x004CA27F File Offset: 0x004C847F
		public AdvancedBranch(AdvMapNodeNormal enter, AdvMapNodeNormal leave, int idx, AdvMapBranch baseBranch, sbyte enterLifeSkillType, short enterLifeSkillVal)
		{
			base.EnterNode = enter;
			base.ExitNode = leave;
			this.BranchIndex = idx;
			base.BaseBranch = baseBranch;
			this.EnterLifeSkillType = enterLifeSkillType;
			this.EnterLifeSkillVal = enterLifeSkillVal;
		}
	}
}
