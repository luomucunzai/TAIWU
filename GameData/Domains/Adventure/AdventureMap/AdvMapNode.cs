using System;

namespace GameData.Domains.Adventure.AdventureMap
{
	// Token: 0x020008CD RID: 2253
	public abstract class AdvMapNode
	{
		// Token: 0x06007FA4 RID: 32676 RVA: 0x004CC046 File Offset: 0x004CA246
		public AdvMapNode(ENodeType nodeType)
		{
			this.NodeType = nodeType;
		}

		// Token: 0x06007FA5 RID: 32677 RVA: 0x004CC080 File Offset: 0x004CA280
		public AdventureMapPoint ToAdventureMapPoint()
		{
			AdventureMapPoint ret = new AdventureMapPoint();
			AdvMapPos pos = this.AdjustedPos;
			ret.PosX = pos.X;
			ret.PosY = pos.Y;
			ret.NodeType = (sbyte)this.NodeType;
			ret.TerrainId = this.TerrainId;
			ret.NodeContentType = this.NodeContent.Item1;
			ret.NodeContentIndex = this.NodeContent.Item2;
			ret.SevenElementType = this.SevenElementType;
			ret.SevenElementCost = this.SevenElementCost;
			ret.JudgeSkill = this.LifeSkillType;
			ret.JudgeValue = this.LifeSkillRequiredVal;
			ret.AffiliatedBranchIdx = this.BranchIndex;
			ret.Index = (short)this.Index;
			ret.JudgeSuccess = false;
			return ret;
		}

		// Token: 0x170004FD RID: 1277
		// (get) Token: 0x06007FA6 RID: 32678
		public abstract AdvMapPos AdjustedPos { get; }

		// Token: 0x06007FA7 RID: 32679 RVA: 0x004CC144 File Offset: 0x004CA344
		public static float CalcSlope(AdvMapPos pos1, AdvMapPos pos2)
		{
			return (pos1.X == pos2.X) ? float.MaxValue : ((float)(pos1.Y - pos2.Y) / (float)(pos1.X - pos2.X));
		}

		// Token: 0x04002306 RID: 8966
		public int Index = -1;

		// Token: 0x04002307 RID: 8967
		public ENodeType NodeType;

		// Token: 0x04002308 RID: 8968
		public int TerrainId;

		// Token: 0x04002309 RID: 8969
		public ValueTuple<sbyte, int> NodeContent = new ValueTuple<sbyte, int>(-1, -1);

		// Token: 0x0400230A RID: 8970
		public sbyte SevenElementType = -1;

		// Token: 0x0400230B RID: 8971
		public sbyte SevenElementCost;

		// Token: 0x0400230C RID: 8972
		public short LifeSkillType = -1;

		// Token: 0x0400230D RID: 8973
		public short LifeSkillRequiredVal = 0;

		// Token: 0x0400230E RID: 8974
		public sbyte BranchIndex;
	}
}
