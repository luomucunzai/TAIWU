using System;
using System.Collections.Generic;
using GameData.Utilities;

namespace GameData.Domains.Map
{
	// Token: 0x020008AD RID: 2221
	public class AStarMap
	{
		// Token: 0x0600789E RID: 30878 RVA: 0x004652C0 File Offset: 0x004634C0
		public void InitMap(int mapWidth, int mapHeight, Func<ByteCoordinate, sbyte> getCostFunc)
		{
			this._getMoveCost = getCostFunc;
			this._mapWidth = mapWidth;
			this._mapHeight = mapHeight;
			this._mapMoveData = new short[mapWidth, mapHeight];
			bool flag = this._openNodeHandler == null;
			if (flag)
			{
				this._openNodeHandler = new AStarMap.OpenNodeHandler();
			}
		}

		// Token: 0x0600789F RID: 30879 RVA: 0x00465308 File Offset: 0x00463508
		public void FindWay(ByteCoordinate start, ByteCoordinate end, ref List<ByteCoordinate> path, List<ByteCoordinate> avoidPosList = null)
		{
			bool flag = this._closedList == null;
			if (flag)
			{
				this._closedList = new List<AStarMap.AStarNode>();
			}
			else
			{
				this._closedList.Clear();
			}
			bool flag2 = !this.IsLocationValid(start) || !this.IsLocationValid(end);
			if (!flag2)
			{
				bool flag3 = avoidPosList != null;
				if (flag3)
				{
					for (int i = 0; i < avoidPosList.Count; i++)
					{
						ByteCoordinate pos = avoidPosList[i];
						this._mapMoveData[(int)pos.X, (int)pos.Y] = 1000;
					}
				}
				bool flag4 = this._mapMoveData[(int)end.X, (int)end.Y] <= 0;
				if (flag4)
				{
					this._mapMoveData[(int)end.X, (int)end.Y] = (short)this._getMoveCost(end);
				}
				bool flag5 = -1 == this._mapMoveData[(int)end.X, (int)end.Y];
				if (!flag5)
				{
					this._openNodeHandler.GetReady();
					AStarMap.AStarNode nodeStart = new AStarMap.AStarNode();
					nodeStart.Location = start;
					nodeStart.GValue = 0;
					nodeStart.HValue = start.GetManhattanDistance(end);
					nodeStart.FValue = nodeStart.HValue;
					this._closedList.Add(nodeStart);
					bool flag6 = start == end;
					if (!flag6)
					{
						this._start = nodeStart.Location;
						this._end = end;
						List<ByteCoordinate> endList = ObjectPool<List<ByteCoordinate>>.Instance.Get();
						endList.Clear();
						bool findFinish = false;
						List<AStarMap.AStarNode> firstAroundNodeList = this.GetNodeAround(nodeStart, end);
						foreach (AStarMap.AStarNode node in firstAroundNodeList)
						{
							bool isGroupReach = endList.Contains(node.Location);
							bool flag7 = node.Location != this._end && !isGroupReach;
							if (!flag7)
							{
								this._closedList.Add(node);
								findFinish = true;
								break;
							}
							this._openNodeHandler.AddOpenNode(node);
						}
						for (;;)
						{
							bool flag8 = findFinish;
							if (flag8)
							{
								break;
							}
							AStarMap.AStarNode bestNode = this._openNodeHandler.GetBestOpenNode();
							bool flag9 = bestNode == null;
							if (flag9)
							{
								goto Block_10;
							}
							this._closedList.Add(bestNode);
							List<AStarMap.AStarNode> nodeList = this.GetNodeAround(bestNode, end);
							foreach (AStarMap.AStarNode node2 in nodeList)
							{
								bool isGroupReach2 = endList.Contains(node2.Location);
								bool flag10 = node2.Location != this._end && !isGroupReach2;
								if (!flag10)
								{
									this._closedList.Add(node2);
									findFinish = true;
									break;
								}
								this._openNodeHandler.AddOpenNode(node2);
							}
						}
						ObjectPool<List<ByteCoordinate>>.Instance.Return(endList);
						this.ConvertResult(ref path);
						Block_10:;
					}
				}
			}
		}

		// Token: 0x060078A0 RID: 30880 RVA: 0x00465634 File Offset: 0x00463834
		private List<AStarMap.AStarNode> GetNodeAround(AStarMap.AStarNode nowNode, ByteCoordinate end)
		{
			List<AStarMap.AStarNode> nodeList = new List<AStarMap.AStarNode>();
			List<ByteCoordinate> neighbors = ObjectPool<List<ByteCoordinate>>.Instance.Get();
			neighbors.Clear();
			bool flag = nowNode.Location.X > 0;
			if (flag)
			{
				neighbors.Add(new ByteCoordinate(nowNode.Location.X - 1, nowNode.Location.Y));
			}
			bool flag2 = (int)nowNode.Location.X < this._mapWidth - 1;
			if (flag2)
			{
				neighbors.Add(new ByteCoordinate(nowNode.Location.X + 1, nowNode.Location.Y));
			}
			bool flag3 = nowNode.Location.Y > 0;
			if (flag3)
			{
				neighbors.Add(new ByteCoordinate(nowNode.Location.X, nowNode.Location.Y - 1));
			}
			bool flag4 = (int)nowNode.Location.Y < this._mapHeight - 1;
			if (flag4)
			{
				neighbors.Add(new ByteCoordinate(nowNode.Location.X, nowNode.Location.Y + 1));
			}
			for (int i = 0; i < neighbors.Count; i++)
			{
				this.CreateNode(neighbors[i], nowNode, nodeList, end);
			}
			ObjectPool<List<ByteCoordinate>>.Instance.Return(neighbors);
			return nodeList;
		}

		// Token: 0x060078A1 RID: 30881 RVA: 0x00465788 File Offset: 0x00463988
		private void CreateNode(ByteCoordinate nodeLocation, AStarMap.AStarNode nowNode, List<AStarMap.AStarNode> nodeList, ByteCoordinate end)
		{
			bool flag = !this.IsLocationValid(nodeLocation);
			if (!flag)
			{
				bool flag2 = this._mapMoveData[(int)nodeLocation.X, (int)nodeLocation.Y] <= 0;
				if (flag2)
				{
					this._mapMoveData[(int)nodeLocation.X, (int)nodeLocation.Y] = (short)this._getMoveCost(nodeLocation);
				}
				bool flag3 = -1 == this._mapMoveData[(int)nodeLocation.X, (int)nodeLocation.Y];
				if (!flag3)
				{
					for (int i = 0; i < this._closedList.Count; i++)
					{
						bool flag4 = this._closedList[i].Location == nodeLocation;
						if (flag4)
						{
							return;
						}
					}
					AStarMap.AStarNode node = this._openNodeHandler.GetNodeByLocation(nodeLocation);
					bool flag5 = node != null;
					if (flag5)
					{
						int gValue = nowNode.GValue + (int)this._mapMoveData[(int)nodeLocation.X, (int)nodeLocation.Y];
						bool flag6 = gValue < node.GValue;
						if (flag6)
						{
							node.GValue = gValue;
							node.FValue = node.GValue + node.HValue;
							node.ParentNode = nowNode;
							this._openNodeHandler.Sort();
							nodeList.Add(node);
						}
					}
					else
					{
						node = new AStarMap.AStarNode();
						node.Location = nodeLocation;
						node.GValue = nowNode.GValue + (int)this._mapMoveData[(int)nodeLocation.X, (int)nodeLocation.Y];
						node.HValue = end.GetManhattanDistance(nodeLocation);
						node.FValue = node.GValue + node.HValue;
						node.ParentNode = nowNode;
						nodeList.Add(node);
					}
				}
			}
		}

		// Token: 0x060078A2 RID: 30882 RVA: 0x0046593C File Offset: 0x00463B3C
		private bool IsLocationValid(ByteCoordinate pos)
		{
			return pos.X >= 0 && (int)pos.X < this._mapWidth && pos.Y >= 0 && (int)pos.Y < this._mapHeight;
		}

		// Token: 0x060078A3 RID: 30883 RVA: 0x00465980 File Offset: 0x00463B80
		private void ConvertResult(ref List<ByteCoordinate> path)
		{
			path.Clear();
			bool flag = this._closedList.Count > 0;
			if (flag)
			{
				AStarMap.AStarNode node = this._closedList[this._closedList.Count - 1];
				bool flag3;
				do
				{
					path.Add(node.Location);
					bool flag2 = node.Location == this._start;
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

		// Token: 0x0400218B RID: 8587
		private short[,] _mapMoveData;

		// Token: 0x0400218C RID: 8588
		private int _mapWidth;

		// Token: 0x0400218D RID: 8589
		private int _mapHeight;

		// Token: 0x0400218E RID: 8590
		private Func<ByteCoordinate, sbyte> _getMoveCost;

		// Token: 0x0400218F RID: 8591
		private AStarMap.OpenNodeHandler _openNodeHandler;

		// Token: 0x04002190 RID: 8592
		private List<AStarMap.AStarNode> _closedList;

		// Token: 0x04002191 RID: 8593
		private ByteCoordinate _start;

		// Token: 0x04002192 RID: 8594
		private ByteCoordinate _end;

		// Token: 0x02000C3F RID: 3135
		private class AStarNode
		{
			// Token: 0x04003576 RID: 13686
			public ByteCoordinate Location;

			// Token: 0x04003577 RID: 13687
			public int GValue;

			// Token: 0x04003578 RID: 13688
			public int HValue;

			// Token: 0x04003579 RID: 13689
			public int FValue;

			// Token: 0x0400357A RID: 13690
			public AStarMap.AStarNode ParentNode;
		}

		// Token: 0x02000C40 RID: 3136
		private class OpenNodeHandler
		{
			// Token: 0x06008E7A RID: 36474 RVA: 0x004FDF44 File Offset: 0x004FC144
			public void AddOpenNode(AStarMap.AStarNode node)
			{
				bool flag = node == null;
				if (!flag)
				{
					bool flag2 = this._openNodeList.Count <= 0;
					if (flag2)
					{
						this._openNodeList.Add(node);
					}
					else
					{
						bool flag3 = this._openNodeList.Find((AStarMap.AStarNode e) => e.Location == node.Location) != null;
						if (!flag3)
						{
							bool add = false;
							for (int i = 0; i < this._openNodeList.Count; i++)
							{
								bool flag4 = this._openNodeList[i].FValue > node.FValue;
								if (flag4)
								{
									this._openNodeList.Insert(i, node);
									add = true;
									break;
								}
							}
							bool flag5 = !add;
							if (flag5)
							{
								this._openNodeList.Add(node);
							}
						}
					}
				}
			}

			// Token: 0x06008E7B RID: 36475 RVA: 0x004FE03E File Offset: 0x004FC23E
			public void Sort()
			{
				List<AStarMap.AStarNode> openNodeList = this._openNodeList;
				if (openNodeList != null)
				{
					openNodeList.Sort(delegate(AStarMap.AStarNode left, AStarMap.AStarNode right)
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

			// Token: 0x06008E7C RID: 36476 RVA: 0x004FE074 File Offset: 0x004FC274
			public AStarMap.AStarNode GetNodeByLocation(ByteCoordinate location)
			{
				for (int i = 0; i < this._openNodeList.Count; i++)
				{
					bool flag = this._openNodeList[i].Location == location;
					if (flag)
					{
						return this._openNodeList[i];
					}
				}
				return null;
			}

			// Token: 0x06008E7D RID: 36477 RVA: 0x004FE0CC File Offset: 0x004FC2CC
			public AStarMap.AStarNode GetBestOpenNode()
			{
				bool flag = this._openNodeList.Count <= 0;
				AStarMap.AStarNode result;
				if (flag)
				{
					result = null;
				}
				else
				{
					AStarMap.AStarNode node = this._openNodeList[0];
					this._openNodeList.Remove(node);
					result = node;
				}
				return result;
			}

			// Token: 0x06008E7E RID: 36478 RVA: 0x004FE114 File Offset: 0x004FC314
			public void GetReady()
			{
				bool flag = this._openNodeList == null;
				if (flag)
				{
					this._openNodeList = new List<AStarMap.AStarNode>();
				}
				else
				{
					this._openNodeList.Clear();
				}
			}

			// Token: 0x0400357B RID: 13691
			private List<AStarMap.AStarNode> _openNodeList;
		}
	}
}
