using System;
using System.Collections.Generic;

namespace GameData.Domains.Map
{
	// Token: 0x020008AE RID: 2222
	public class AStarMapForTravel
	{
		// Token: 0x060078A5 RID: 30885 RVA: 0x00465A14 File Offset: 0x00463C14
		public void InitMap(Func<short, short, short> getCostFunc)
		{
			this._getMoveCost = getCostFunc;
			bool flag = this._openNodeHandler == null;
			if (flag)
			{
				this._openNodeHandler = new AStarMapForTravel.OpenNodeHandler();
			}
		}

		// Token: 0x060078A6 RID: 30886 RVA: 0x00465A44 File Offset: 0x00463C44
		public void FindWay(short start, short end, ref List<short> path)
		{
			bool flag = this._closedList == null;
			if (flag)
			{
				this._closedList = new List<AStarMapForTravel.AStarNode>();
			}
			else
			{
				this._closedList.Clear();
			}
			this._openNodeHandler.GetReady();
			AStarMapForTravel.AStarNode nodeStart = new AStarMapForTravel.AStarNode();
			nodeStart.AreaId = start;
			nodeStart.GValue = 0;
			nodeStart.HValue = 0;
			nodeStart.FValue = nodeStart.HValue;
			this._closedList.Add(nodeStart);
			bool flag2 = start == end;
			if (!flag2)
			{
				this._start = nodeStart.AreaId;
				this._end = end;
				bool findFinish = false;
				List<AStarMapForTravel.AStarNode> firstAroundNodeList = this.GetNodeAround(nodeStart);
				foreach (AStarMapForTravel.AStarNode node in firstAroundNodeList)
				{
					bool flag3 = node.AreaId != this._end;
					if (!flag3)
					{
						this._closedList.Add(node);
						findFinish = true;
						break;
					}
					this._openNodeHandler.AddOpenNode(node);
				}
				for (;;)
				{
					bool flag4 = findFinish;
					if (flag4)
					{
						break;
					}
					AStarMapForTravel.AStarNode bestNode = this._openNodeHandler.GetBestOpenNode();
					bool flag5 = bestNode == null;
					if (flag5)
					{
						goto Block_5;
					}
					this._closedList.Add(bestNode);
					List<AStarMapForTravel.AStarNode> nodeList = this.GetNodeAround(bestNode);
					foreach (AStarMapForTravel.AStarNode node2 in nodeList)
					{
						bool flag6 = node2.AreaId != this._end;
						if (!flag6)
						{
							this._closedList.Add(node2);
							findFinish = true;
							break;
						}
						this._openNodeHandler.AddOpenNode(node2);
					}
				}
				this.ConvertResult(ref path);
				Block_5:;
			}
		}

		// Token: 0x060078A7 RID: 30887 RVA: 0x00465C30 File Offset: 0x00463E30
		private List<AStarMapForTravel.AStarNode> GetNodeAround(AStarMapForTravel.AStarNode nowNode)
		{
			List<AStarMapForTravel.AStarNode> nodeList = new List<AStarMapForTravel.AStarNode>();
			HashSet<short> neighbors = DomainManager.Map.GetElement_Areas((int)nowNode.AreaId).NeighborAreas;
			foreach (short areaId in neighbors)
			{
				this.CreateNode(areaId, nowNode, nodeList);
			}
			return nodeList;
		}

		// Token: 0x060078A8 RID: 30888 RVA: 0x00465CA8 File Offset: 0x00463EA8
		private void CreateNode(short nodeLocation, AStarMapForTravel.AStarNode nowNode, List<AStarMapForTravel.AStarNode> nodeList)
		{
			bool flag = this._closedList.Exists((AStarMapForTravel.AStarNode node) => node.AreaId == nodeLocation);
			if (!flag)
			{
				short moveCost = this._getMoveCost(nowNode.AreaId, nodeLocation);
				AStarMapForTravel.AStarNode node2 = this._openNodeHandler.GetNodeByLocation(nodeLocation);
				bool flag2 = node2 != null;
				if (flag2)
				{
					int gValue = nowNode.GValue + (int)moveCost;
					bool flag3 = gValue < node2.GValue;
					if (flag3)
					{
						node2.GValue = gValue;
						node2.FValue = node2.GValue + node2.HValue;
						node2.ParentNode = nowNode;
						this._openNodeHandler.Sort();
						nodeList.Add(node2);
					}
				}
				else
				{
					node2 = new AStarMapForTravel.AStarNode();
					node2.AreaId = nodeLocation;
					node2.GValue = nowNode.GValue + (int)moveCost;
					node2.HValue = 0;
					node2.FValue = node2.GValue + node2.HValue;
					node2.ParentNode = nowNode;
					nodeList.Add(node2);
				}
			}
		}

		// Token: 0x060078A9 RID: 30889 RVA: 0x00465DB8 File Offset: 0x00463FB8
		private void ConvertResult(ref List<short> path)
		{
			path.Clear();
			bool flag = this._closedList.Count > 0;
			if (flag)
			{
				AStarMapForTravel.AStarNode node = this._closedList[this._closedList.Count - 1];
				bool flag3;
				do
				{
					path.Add(node.AreaId);
					bool flag2 = node.AreaId == this._start;
					if (flag2)
					{
						break;
					}
					node = node.ParentNode;
					flag3 = (node == null);
				}
				while (!flag3);
				path.Reverse();
			}
		}

		// Token: 0x04002193 RID: 8595
		private Func<short, short, short> _getMoveCost;

		// Token: 0x04002194 RID: 8596
		private AStarMapForTravel.OpenNodeHandler _openNodeHandler;

		// Token: 0x04002195 RID: 8597
		private List<AStarMapForTravel.AStarNode> _closedList;

		// Token: 0x04002196 RID: 8598
		private short _start;

		// Token: 0x04002197 RID: 8599
		private short _end;

		// Token: 0x02000C41 RID: 3137
		private class AStarNode
		{
			// Token: 0x0400357C RID: 13692
			public short AreaId;

			// Token: 0x0400357D RID: 13693
			public int GValue;

			// Token: 0x0400357E RID: 13694
			public int HValue;

			// Token: 0x0400357F RID: 13695
			public int FValue;

			// Token: 0x04003580 RID: 13696
			public AStarMapForTravel.AStarNode ParentNode;
		}

		// Token: 0x02000C42 RID: 3138
		private class OpenNodeHandler
		{
			// Token: 0x06008E81 RID: 36481 RVA: 0x004FE15C File Offset: 0x004FC35C
			public void AddOpenNode(AStarMapForTravel.AStarNode node)
			{
				bool flag = node == null || this._openNodeList.Exists((AStarMapForTravel.AStarNode n) => n.AreaId == node.AreaId);
				if (!flag)
				{
					bool added = false;
					for (int i = 0; i < this._openNodeList.Count; i++)
					{
						bool flag2 = this._openNodeList[i].FValue > node.FValue;
						if (flag2)
						{
							this._openNodeList.Insert(i, node);
							added = true;
							break;
						}
					}
					bool flag3 = !added;
					if (flag3)
					{
						this._openNodeList.Add(node);
					}
				}
			}

			// Token: 0x06008E82 RID: 36482 RVA: 0x004FE217 File Offset: 0x004FC417
			public void Sort()
			{
				List<AStarMapForTravel.AStarNode> openNodeList = this._openNodeList;
				if (openNodeList != null)
				{
					openNodeList.Sort(delegate(AStarMapForTravel.AStarNode left, AStarMapForTravel.AStarNode right)
					{
						bool flag = left.FValue != right.FValue;
						int result;
						if (flag)
						{
							result = left.FValue - right.FValue;
						}
						else
						{
							bool flag2 = left.HValue != right.HValue;
							if (flag2)
							{
								result = left.HValue - right.HValue;
							}
							else
							{
								result = left.GValue - right.GValue;
							}
						}
						return result;
					});
				}
			}

			// Token: 0x06008E83 RID: 36483 RVA: 0x004FE24C File Offset: 0x004FC44C
			public AStarMapForTravel.AStarNode GetNodeByLocation(short areaId)
			{
				for (int i = 0; i < this._openNodeList.Count; i++)
				{
					bool flag = this._openNodeList[i].AreaId == areaId;
					if (flag)
					{
						return this._openNodeList[i];
					}
				}
				return null;
			}

			// Token: 0x06008E84 RID: 36484 RVA: 0x004FE2A0 File Offset: 0x004FC4A0
			public AStarMapForTravel.AStarNode GetBestOpenNode()
			{
				bool flag = this._openNodeList.Count <= 0;
				AStarMapForTravel.AStarNode result;
				if (flag)
				{
					result = null;
				}
				else
				{
					AStarMapForTravel.AStarNode node = this._openNodeList[0];
					this._openNodeList.Remove(node);
					result = node;
				}
				return result;
			}

			// Token: 0x06008E85 RID: 36485 RVA: 0x004FE2E8 File Offset: 0x004FC4E8
			public void GetReady()
			{
				bool flag = this._openNodeList == null;
				if (flag)
				{
					this._openNodeList = new List<AStarMapForTravel.AStarNode>();
				}
				else
				{
					this._openNodeList.Clear();
				}
			}

			// Token: 0x04003581 RID: 13697
			private List<AStarMapForTravel.AStarNode> _openNodeList;
		}
	}
}
