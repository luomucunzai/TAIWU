using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Config;
using GameData.Common;
using GameData.Utilities;
using Redzen.Random;

namespace GameData.Domains.Adventure.AdventureMap
{
	// Token: 0x020008CC RID: 2252
	public class AdvMapBranch : AdvBaseMapBranch<AdvMapNodeVertex>
	{
		// Token: 0x170004F8 RID: 1272
		// (get) Token: 0x06007F90 RID: 32656 RVA: 0x004CA2B9 File Offset: 0x004C84B9
		// (set) Token: 0x06007F91 RID: 32657 RVA: 0x004CA2C1 File Offset: 0x004C84C1
		public ELinkDir EnterDir { get; set; }

		// Token: 0x170004F9 RID: 1273
		// (get) Token: 0x06007F92 RID: 32658 RVA: 0x004CA2CA File Offset: 0x004C84CA
		public AdvMapPos BasePos
		{
			get
			{
				return base.EnterNode.AdjustedPos + this.EnterDir.Offset((base.EnterNode.NodeType == ENodeType.Start) ? 1 : 2);
			}
		}

		// Token: 0x170004FA RID: 1274
		// (get) Token: 0x06007F93 RID: 32659 RVA: 0x004CA2F8 File Offset: 0x004C84F8
		public bool AnchorByStart
		{
			get
			{
				return base.ExitNode.PrevVertex == base.EnterNode;
			}
		}

		// Token: 0x170004FB RID: 1275
		// (get) Token: 0x06007F94 RID: 32660 RVA: 0x004CA30D File Offset: 0x004C850D
		// (set) Token: 0x06007F95 RID: 32661 RVA: 0x004CA315 File Offset: 0x004C8515
		public int BranchLength { get; private set; }

		// Token: 0x170004FC RID: 1276
		// (get) Token: 0x06007F96 RID: 32662 RVA: 0x004CA31E File Offset: 0x004C851E
		// (set) Token: 0x06007F97 RID: 32663 RVA: 0x004CA326 File Offset: 0x004C8526
		public string BranchKey { get; private set; }

		// Token: 0x06007F98 RID: 32664 RVA: 0x004CA330 File Offset: 0x004C8530
		public AdvMapBranch(int branchIdx, AdvMapNodeVertex start, AdvMapNodeVertex end, AdventureBaseBranch branchConfigData, DataContext context)
		{
			this.BranchIndex = branchIdx;
			base.BaseBranch = this;
			base.EnterNode = start;
			base.ExitNode = end;
			this._branchConfigData = branchConfigData;
			this._terrains = new List<short>();
			this._extraEvents = new List<ValueTuple<string, sbyte, short>>();
			this._contentWeightDistribution = new List<short>();
			int len = context.Random.Next((int)((float)branchConfigData.Length * 0.8f), (int)((float)branchConfigData.Length * 1.2f) + 1);
			bool flag = len > 1000;
			if (flag)
			{
				throw new Exception("Branch Is Too Large");
			}
			this.BranchLength = len;
			this.BranchKey = branchConfigData.BranchKey;
			bool flag2;
			if (this._branchConfigData.TerrainPersonalityWeights != null && this._branchConfigData.TerrainPersonalityWeights.Length != 0)
			{
				flag2 = (this._branchConfigData.TerrainPersonalityWeights.Sum((ValueTuple<byte, short, short[]> weight) => (int)weight.Item2) == 0);
			}
			else
			{
				flag2 = true;
			}
			bool flag3 = flag2;
			if (flag3)
			{
			}
			this.FirstNode = new AdvMapNodeNormal(-1, this, context, -1);
			this.SetFirstNode(context);
			this.Nodes.Add(this.FirstNode);
		}

		// Token: 0x06007F99 RID: 32665 RVA: 0x004CA46C File Offset: 0x004C866C
		public string GetExtraEvent(int index)
		{
			return this._extraEvents[index].Item1;
		}

		// Token: 0x06007F9A RID: 32666 RVA: 0x004CA490 File Offset: 0x004C8690
		private void SetFirstNode(DataContext context)
		{
			ValueTuple<byte, sbyte, byte, short> randomNodeInfo = this.GetRandomNodeInfo(context, null);
			byte terrainId = randomNodeInfo.Item1;
			sbyte personalityId = randomNodeInfo.Item2;
			byte skillId = randomNodeInfo.Item3;
			short skillRequire = randomNodeInfo.Item4;
			bool flag = base.EnterNode.NodeType == ENodeType.Start;
			if (flag)
			{
				this.FirstNode.TerrainId = (int)terrainId;
				this.FirstNode.SevenElementType = personalityId;
				this.FirstNode.SevenElementCost = (sbyte)context.Random.Next((int)GlobalConfig.Instance.AdventureNodePersonalityMinCost, (int)(GlobalConfig.Instance.AdventureNodePersonalityMaxCost + 1));
				this.FirstNode.LifeSkillType = (short)skillId;
				this.FirstNode.LifeSkillRequiredVal = skillRequire;
				this.FirstNode.NodeContent = this.GetRandomContent(context, (int)terrainId, personalityId, null);
			}
			else
			{
				this.FirstNode.TerrainId = (int)terrainId;
				this.FirstNode.SevenElementType = -1;
				this.FirstNode.LifeSkillType = -1;
				this.FirstNode.LifeSkillRequiredVal = -1;
				this.FirstNode.NodeContent = new ValueTuple<sbyte, int>(-1, -1);
			}
			this.FirstNode.Offset = new AdvMapPos(0, 0);
		}

		// Token: 0x06007F9B RID: 32667 RVA: 0x004CA5A4 File Offset: 0x004C87A4
		public void GenBranch(DataContext context, List<ValueTuple<string, sbyte, short>> extraEvents, HashSet<AdvMapPos> primaryPath = null)
		{
			this._extraEvents.Clear();
			this._extraEvents.AddRange(extraEvents);
			this._contentWeightDistribution.Clear();
			base.ExitNode.PrevVertex = base.EnterNode;
			for (;;)
			{
				bool flag;
				if (this.Nodes.Count >= this.BranchLength)
				{
					List<AdvMapNodeNormal> nodes = this.Nodes;
					flag = ((int)(nodes[nodes.Count - 1].Offset.X - this.Nodes[0].Offset.X) < this.BranchLength * 2);
				}
				else
				{
					flag = true;
				}
				if (!flag)
				{
					goto Block_20;
				}
				List<int> dirWeight = null;
				switch (this.EnterDir)
				{
				case ELinkDir.Right:
					dirWeight = new List<int>
					{
						20,
						60,
						20
					};
					goto IL_12B;
				case ELinkDir.UpRight:
					dirWeight = new List<int>
					{
						60,
						35,
						15
					};
					goto IL_12B;
				case ELinkDir.DownRight:
					dirWeight = new List<int>
					{
						15,
						35,
						60
					};
					goto IL_12B;
				}
				break;
				IL_12B:
				List<AdvMapNodeNormal> nodes2 = this.Nodes;
				AdvMapNodeNormal tmpNode = nodes2[nodes2.Count - 1];
				bool flag2 = tmpNode.Offset.X <= 2;
				if (flag2)
				{
					for (int i = 0; i < dirWeight.Count; i++)
					{
						dirWeight[i] = ((dirWeight[i] <= 35) ? 0 : dirWeight[i]);
					}
				}
				else
				{
					bool flag3 = tmpNode.Offset.X <= 4;
					if (flag3)
					{
						for (int j = 0; j < dirWeight.Count; j++)
						{
							dirWeight[j] = ((dirWeight[j] <= 20) ? 0 : ((dirWeight[j] <= 35) ? 20 : dirWeight[j]));
						}
					}
				}
				bool flag4 = tmpNode.Offset.Y >= 3;
				if (flag4)
				{
					List<int> dirWeight2 = dirWeight;
					dirWeight2[0] = dirWeight2[0] / (int)((tmpNode.Offset.Y >= 5) ? tmpNode.Offset.Y : 2);
				}
				else
				{
					bool flag5 = tmpNode.Offset.Y <= -3;
					if (flag5)
					{
						List<int> dirWeight2 = dirWeight;
						dirWeight2[2] = dirWeight2[2] / (int)((tmpNode.Offset.Y <= -5) ? (-tmpNode.Offset.Y) : 2);
					}
				}
				bool flag6 = primaryPath != null;
				if (flag6)
				{
					bool flag7 = primaryPath.Contains(this.GetFinalPos(tmpNode.Offset + new AdvMapPos(1, 1))) || primaryPath.Contains(this.GetFinalPos(tmpNode.Offset + new AdvMapPos(2, 2)));
					if (flag7)
					{
						dirWeight[0] = 0;
					}
					bool flag8 = primaryPath.Contains(this.GetFinalPos(tmpNode.Offset + new AdvMapPos(2, 0)));
					if (flag8)
					{
						dirWeight[1] = 0;
					}
					bool flag9 = primaryPath.Contains(this.GetFinalPos(tmpNode.Offset + new AdvMapPos(1, -1))) || primaryPath.Contains(this.GetFinalPos(tmpNode.Offset + new AdvMapPos(2, -2)));
					if (flag9)
					{
						dirWeight[2] = 0;
					}
				}
				ValueTuple<byte, sbyte, byte, short> randomNodeInfo = this.GetRandomNodeInfo(context, null);
				byte terrainId = randomNodeInfo.Item1;
				sbyte personalityId = randomNodeInfo.Item2;
				byte skillId = randomNodeInfo.Item3;
				short skillRequire = randomNodeInfo.Item4;
				switch (RandomUtils.GetRandomResult<int>(from a in Enumerable.Range(0, 3)
				select new ValueTuple<int, short>(a, (short)dirWeight[a]), context.Random))
				{
				case 0:
				{
					AdvMapNodeNormal node7 = new AdvMapNodeNormal((int)terrainId, this, context, personalityId);
					node7.LifeSkillType = (short)skillId;
					node7.LifeSkillRequiredVal = skillRequire;
					node7.Offset = tmpNode.Offset + new AdvMapPos(1, 1);
					this.Nodes.Add(node7);
					break;
				}
				case 1:
				{
					AdvMapNodeNormal node2 = new AdvMapNodeNormal((int)terrainId, this, context, personalityId);
					node2.LifeSkillType = (short)skillId;
					node2.LifeSkillRequiredVal = skillRequire;
					node2.Offset = tmpNode.Offset + new AdvMapPos(2, 0);
					this.Nodes.Add(node2);
					break;
				}
				case 2:
				{
					AdvMapNodeNormal node3 = new AdvMapNodeNormal((int)terrainId, this, context, personalityId);
					node3.LifeSkillType = (short)skillId;
					node3.LifeSkillRequiredVal = skillRequire;
					node3.Offset = tmpNode.Offset + new AdvMapPos(1, -1);
					this.Nodes.Add(node3);
					break;
				}
				}
			}
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(8, 1);
			defaultInterpolatedStringHandler.AppendLiteral("不合法的进入方向");
			defaultInterpolatedStringHandler.AppendFormatted<ELinkDir>(this.EnterDir);
			throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			Block_20:
			List<AdvMapNodeNormal> nodes3 = this.Nodes;
			this.LastNode = nodes3[nodes3.Count - 1];
			bool flag10 = this.BranchLength > 0;
			if (flag10)
			{
				int extraCount = this.BranchLength * 4 - this.Nodes.Count;
				int useCnt;
				for (int k = 0; k < extraCount; k += useCnt)
				{
					List<AdvMapPos> valid = new List<AdvMapPos>();
					AdvMapPos[] nodePoints = (from a in this.Nodes
					select a.Offset).ToArray<AdvMapPos>();
					foreach (AdvMapNodeNormal one in this.Nodes)
					{
						valid.AddRange(one.Offset.GetAroundPoints());
					}
					valid = new HashSet<AdvMapPos>(valid).ToList<AdvMapPos>();
					Func<AdvMapPos, bool> <>9__3;
					valid.RemoveAll(delegate(AdvMapPos validPos)
					{
						bool flag16 = nodePoints.Exist(validPos);
						bool result;
						if (flag16)
						{
							result = true;
						}
						else
						{
							bool flag17 = primaryPath != null && primaryPath.Contains(this.GetFinalPos(validPos));
							if (flag17)
							{
								result = true;
							}
							else
							{
								IEnumerable<AdvMapPos> aroundPoints = validPos.GetAroundPoints();
								Func<AdvMapPos, bool> predicate;
								if ((predicate = <>9__3) == null)
								{
									predicate = (<>9__3 = ((AdvMapPos a) => nodePoints.Contains(a)));
								}
								IEnumerable<AdvMapPos> nears = aroundPoints.Where(predicate);
								bool hasSmaller = false;
								bool hasGreater = false;
								foreach (AdvMapPos nearbyPos in nears)
								{
									bool flag18 = validPos.X > nearbyPos.X;
									if (flag18)
									{
										hasSmaller = true;
									}
									bool flag19 = validPos.X < nearbyPos.X;
									if (flag19)
									{
										hasGreater = true;
									}
								}
								bool flag20 = hasGreater && hasSmaller;
								result = !flag20;
							}
						}
						return result;
					});
					bool flag11 = valid.Count == 0;
					if (flag11)
					{
						break;
					}
					CollectionUtils.Shuffle<AdvMapPos>(context.Random, valid);
					useCnt = context.Random.Next(1, Math.Min(valid.Count, extraCount - k) + 1);
					for (int l = 0; l < useCnt; l++)
					{
						ValueTuple<byte, sbyte, byte, short> randomNodeInfo2 = this.GetRandomNodeInfo(context, null);
						byte terrainId2 = randomNodeInfo2.Item1;
						sbyte personalityId2 = randomNodeInfo2.Item2;
						byte skillId2 = randomNodeInfo2.Item3;
						short skillRequire2 = randomNodeInfo2.Item4;
						AdvMapNodeNormal node4 = new AdvMapNodeNormal((int)terrainId2, this, context, personalityId2);
						node4.Offset = valid[l];
						node4.LifeSkillType = (short)skillId2;
						node4.LifeSkillRequiredVal = skillRequire2;
						this.Nodes.Add(node4);
					}
				}
			}
			for (int m = 1; m < this.Nodes.Count; m++)
			{
				this.Nodes[m].NodeContent = this.GetRandomContent(context, this.Nodes[m].TerrainId, this.Nodes[m].SevenElementType, null);
				bool flag12 = this.Nodes[m].NodeContent.Item1 == -1;
				if (flag12)
				{
					this.Nodes[m].LifeSkillType = -1;
					this.Nodes[m].LifeSkillRequiredVal = -1;
				}
			}
			bool flag13 = this._extraEvents.Count > 0;
			if (flag13)
			{
				for (int n = 0; n < this._extraEvents.Count; n++)
				{
					ValueTuple<string, sbyte, short> valueTuple = this._extraEvents[n];
					sbyte personality = valueTuple.Item2;
					short count = valueTuple.Item3;
					List<AdvMapNodeNormal> nodesWithValidPersonality = (from node in this.Nodes
					where node.SevenElementType == personality
					select node).ToList<AdvMapNodeNormal>();
					List<AdvMapNodeNormal> emptyNodes = (from node in nodesWithValidPersonality
					where node.NodeContent.Item1 == -1
					select node).ToList<AdvMapNodeNormal>();
					while (count > 0)
					{
						ValueTuple<byte, sbyte, byte, short> randomNodeInfo3 = this.GetRandomNodeInfo(context, this._branchConfigData);
						byte skillId3 = randomNodeInfo3.Item3;
						short skillRequire3 = randomNodeInfo3.Item4;
						bool flag14 = emptyNodes.Count > 0;
						if (flag14)
						{
							int randomIndex = context.Random.Next(0, emptyNodes.Count);
							AdvMapNodeNormal node5 = emptyNodes[randomIndex];
							node5.NodeContent = new ValueTuple<sbyte, int>(10, n);
							node5.LifeSkillType = (short)skillId3;
							node5.LifeSkillRequiredVal = skillRequire3;
							nodesWithValidPersonality.Remove(node5);
							emptyNodes.RemoveAt(randomIndex);
						}
						else
						{
							bool flag15 = nodesWithValidPersonality.Count > 0;
							if (!flag15)
							{
								break;
							}
							int randomIndex2 = context.Random.Next(0, nodesWithValidPersonality.Count);
							AdvMapNodeNormal node6 = nodesWithValidPersonality[randomIndex2];
							node6.NodeContent = new ValueTuple<sbyte, int>(10, n);
							node6.LifeSkillType = (short)skillId3;
							node6.LifeSkillRequiredVal = skillRequire3;
							nodesWithValidPersonality.RemoveAt(randomIndex2);
						}
						count -= 1;
					}
				}
			}
			this.GenerateAdvancedBranch(context);
		}

		// Token: 0x06007F9C RID: 32668 RVA: 0x004CAFB0 File Offset: 0x004C91B0
		private void GenerateAdvancedBranch(DataContext context)
		{
			List<AdventureAdvancedBranch> advancedCfgs = (from advBranch in DomainManager.Adventure.AdventureCfg.AdvancedBranches
			where advBranch.ParentBranchId == DomainManager.Adventure.AdventureCfg.BaseBranches.IndexOf(this._branchConfigData)
			select advBranch).ToList<AdventureAdvancedBranch>();
			bool flag = advancedCfgs.Count != 1;
			if (!flag)
			{
				bool flag2 = this.Nodes.Count == 0;
				if (flag2)
				{
					throw new Exception("An empty base branch can't bind advanced branch.");
				}
				AdventureAdvancedBranch advancedCfg = advancedCfgs[0];
				int advancedIdx = DomainManager.Adventure.AdventureCfg.AdvancedBranches.IndexOf(advancedCfg) + DomainManager.Adventure.AdventureCfg.BaseBranches.Count;
				bool flag3 = advancedCfg.Length <= 0;
				if (flag3)
				{
					throw new Exception("Length of advanced branch can't be zero.");
				}
				short advancedLen = advancedCfg.Length;
				HashSet<AdvMapPos> basePoints = (from node in this.Nodes
				select node.Offset).ToHashSet<AdvMapPos>();
				HashSet<AdvMapPos> expandedPoints = basePoints.Expand();
				ValueTuple<int, int> xRange = new ValueTuple<int, int>((this.EnterDir == ELinkDir.Right) ? 2 : 3, (int)(this.LastNode.Offset.X - 2));
				List<AdvMapPos> validPoints = (from pos in expandedPoints.Expand().Except(expandedPoints).Except(basePoints)
				where (int)pos.X >= xRange.Item1 && (int)pos.X <= xRange.Item2
				select pos).ToList<AdvMapPos>();
				int maxStartX = xRange.Item2 - (int)advancedLen;
				List<AdvMapPos> validStartPoints = (from pos in validPoints
				where (int)pos.X <= maxStartX
				select pos).ToList<AdvMapPos>();
				List<AdvMapPos> result = new List<AdvMapPos>();
				List<AdvMapPos> complements = new List<AdvMapPos>();
				List<AdvMapPos> extensionList = new List<AdvMapPos>();
				List<AdvMapNodeNormal> newNodes = new List<AdvMapNodeNormal>();
				this.AdvancedBranch = null;
				while (this.AdvancedBranch == null && validStartPoints.Count > 0)
				{
					result.Clear();
					AdvMapPos startPos = validStartPoints[context.Random.Next(0, validStartPoints.Count)];
					result.Add(startPos);
					AdvMapPos endPos = startPos;
					while (result.Count < (int)advancedLen)
					{
						AdvMapPos[] aroundPoints = endPos.GetAroundPoints();
						short endPosX = endPos.X;
						endPos = AdvMapPos.Error;
						foreach (AdvMapPos pos3 in aroundPoints)
						{
							bool flag4 = pos3.X <= endPosX || !validPoints.Contains(pos3);
							if (!flag4)
							{
								endPos = pos3;
								result.Add(endPos);
								break;
							}
						}
						bool flag5 = !endPos.Equals(AdvMapPos.Error);
						if (!flag5)
						{
							validStartPoints.Remove(startPos);
							break;
						}
					}
					bool flag6 = validStartPoints.Count == 0;
					if (flag6)
					{
						AdaptableLog.TagWarning("Adventure", "failed to generate advanced branch.", false);
						break;
					}
					bool flag7 = !validStartPoints.Contains(startPos);
					if (!flag7)
					{
						AdvMapPos enterPos = AdvMapPos.Error;
						AdvMapPos exitPos = AdvMapPos.Error;
						AdvMapPos anyPos = basePoints.First((AdvMapPos pos) => Math.Abs((int)(pos.X - startPos.X)) < 2);
						enterPos = startPos + AdvMapPos.Left * 2;
						exitPos = endPos + AdvMapPos.Right * 2;
						bool flag8 = !basePoints.Contains(enterPos);
						if (flag8)
						{
							bool flag9 = anyPos.Y < startPos.Y;
							if (flag9)
							{
								enterPos = startPos + AdvMapPos.LowerLeft * 2;
								this.AddComplements(enterPos, AdvMapPos.Left, AdvMapPos.LowerLeft, basePoints, complements, extensionList);
							}
							else
							{
								bool flag10 = anyPos.Y > startPos.Y;
								if (flag10)
								{
									enterPos = startPos + AdvMapPos.UpperLeft * 2;
									this.AddComplements(enterPos, AdvMapPos.Left, AdvMapPos.UpperLeft, basePoints, complements, extensionList);
								}
							}
							bool flag11 = extensionList.Count > 0;
							if (flag11)
							{
								ValueTuple<byte, sbyte, byte, short> randomNodeInfo = this.GetRandomNodeInfo(context, this._branchConfigData);
								byte terrainId = randomNodeInfo.Item1;
								sbyte personalityId = randomNodeInfo.Item2;
								byte skillId = randomNodeInfo.Item3;
								short skillRequire = randomNodeInfo.Item4;
								this.FirstNode.SevenElementType = personalityId;
								this.FirstNode.SevenElementCost = (sbyte)context.Random.Next((int)GlobalConfig.Instance.AdventureNodePersonalityMinCost, (int)(GlobalConfig.Instance.AdventureNodePersonalityMaxCost + 1));
								this.FirstNode.LifeSkillType = (short)skillId;
								this.FirstNode.LifeSkillRequiredVal = skillRequire;
								this.FirstNode.NodeContent = this.GetRandomContent(context, (int)terrainId, personalityId, this._branchConfigData);
								this.CreateNodes(context, extensionList, newNodes);
								this.Nodes.AddRange(newNodes);
								extensionList.Clear();
								this.FirstNode = newNodes[0];
							}
						}
						bool flag12 = !basePoints.Contains(exitPos);
						if (flag12)
						{
							bool flag13 = anyPos.Y < startPos.Y;
							if (flag13)
							{
								exitPos = endPos + AdvMapPos.LowerRight * 2;
								this.AddComplements(exitPos, AdvMapPos.Right, AdvMapPos.LowerRight, basePoints, complements, extensionList);
							}
							else
							{
								exitPos = endPos + AdvMapPos.UpperRight * 2;
								this.AddComplements(exitPos, AdvMapPos.Right, AdvMapPos.UpperRight, basePoints, complements, extensionList);
							}
							bool flag14 = extensionList.Count > 0;
							if (flag14)
							{
								this.CreateNodes(context, extensionList, newNodes);
								this.Nodes.AddRange(newNodes);
								extensionList.Clear();
								this.LastNode = newNodes[0];
							}
						}
						this.CreateNodes(context, complements, newNodes);
						this.Nodes.AddRange(newNodes);
						AdvMapPos deltaOffset = this.FirstNode.Offset;
						bool flag15 = deltaOffset.X != 0 || deltaOffset.Y != 0;
						if (flag15)
						{
							foreach (AdvMapNodeNormal node3 in this.Nodes)
							{
								node3.Offset -= deltaOffset;
							}
							enterPos -= deltaOffset;
							exitPos -= deltaOffset;
						}
						this.AdvancedBranch = new AdvancedBranch(this.Nodes.First((AdvMapNodeNormal a) => a.Offset.Equals(enterPos)), this.Nodes.First((AdvMapNodeNormal a) => a.Offset.Equals(exitPos)), advancedIdx, this, advancedCfg.EnterSkillType, advancedCfg.EnterSkillRequire);
						foreach (AdvMapPos pos2 in result)
						{
							ValueTuple<byte, sbyte, byte, short> randomNodeInfo2 = this.GetRandomNodeInfo(context, advancedCfg);
							byte terrainId2 = randomNodeInfo2.Item1;
							sbyte personalityId2 = randomNodeInfo2.Item2;
							byte skillId2 = randomNodeInfo2.Item3;
							short skillRequire2 = randomNodeInfo2.Item4;
							AdvMapNodeNormal node2 = new AdvMapNodeNormal((int)terrainId2, this.AdvancedBranch, context, personalityId2);
							node2.SevenElementCost = 0;
							node2.LifeSkillType = (short)skillId2;
							node2.LifeSkillRequiredVal = skillRequire2;
							node2.Offset = pos2 - deltaOffset;
							node2.NodeContent = this.GetRandomContent(context, (int)terrainId2, personalityId2, advancedCfg);
							this.AdvancedBranch.Nodes.Add(node2);
						}
					}
				}
			}
		}

		// Token: 0x06007F9D RID: 32669 RVA: 0x004CB7A8 File Offset: 0x004C99A8
		private void AddComplements(AdvMapPos initialPos, AdvMapPos checkDir, AdvMapPos complementDir, HashSet<AdvMapPos> basePoints, List<AdvMapPos> complementList, List<AdvMapPos> extensionList)
		{
			List<AdvMapPos> localComplements = new List<AdvMapPos>();
			AdvMapPos curPos = initialPos;
			while (!basePoints.Contains(curPos))
			{
				localComplements.Add(curPos);
				bool flag = basePoints.Contains(curPos + checkDir);
				if (flag)
				{
					break;
				}
				curPos += complementDir;
				bool flag2 = curPos.Y == this.FirstNode.Offset.Y && curPos.X < this.FirstNode.Offset.X;
				if (flag2)
				{
					extensionList.Add(curPos);
					for (int i = (int)(curPos.X + 2); i < (int)this.FirstNode.Offset.X; i += 2)
					{
						extensionList.Add(new AdvMapPos(i, (int)curPos.Y));
					}
					break;
				}
				bool flag3 = curPos.Y == this.LastNode.Offset.Y && curPos.X > this.LastNode.Offset.X;
				if (flag3)
				{
					extensionList.Add(curPos);
					for (int j = (int)(this.LastNode.Offset.X + 2); j < (int)curPos.X; j += 2)
					{
						extensionList.Add(new AdvMapPos(j, (int)curPos.Y));
					}
					break;
				}
			}
			AdvMapPos reverseDir = new AdvMapPos((int)(-(int)complementDir.X), (int)complementDir.Y);
			foreach (AdvMapPos complement in localComplements)
			{
				curPos = complement + reverseDir;
				while (!complementList.Contains(curPos) && !basePoints.Contains(curPos) && !extensionList.Contains(curPos))
				{
					complementList.Add(curPos);
					curPos += reverseDir;
				}
			}
			complementList.AddRange(localComplements);
		}

		// Token: 0x06007F9E RID: 32670 RVA: 0x004CB9D4 File Offset: 0x004C9BD4
		private void CreateNodes(DataContext context, List<AdvMapPos> positions, List<AdvMapNodeNormal> result)
		{
			result.Clear();
			foreach (AdvMapPos pos in positions)
			{
				ValueTuple<byte, sbyte, byte, short> randomNodeInfo = this.GetRandomNodeInfo(context, this._branchConfigData);
				byte terrainId = randomNodeInfo.Item1;
				sbyte personalityId = randomNodeInfo.Item2;
				byte skillId = randomNodeInfo.Item3;
				short skillRequire = randomNodeInfo.Item4;
				result.Add(new AdvMapNodeNormal((int)terrainId, this, context, personalityId)
				{
					LifeSkillType = (short)skillId,
					LifeSkillRequiredVal = skillRequire,
					Offset = pos,
					NodeContent = this.GetRandomContent(context, (int)terrainId, personalityId, this._branchConfigData)
				});
			}
		}

		// Token: 0x06007F9F RID: 32671 RVA: 0x004CBA9C File Offset: 0x004C9C9C
		private AdvMapPos GetFinalPos(AdvMapPos offset)
		{
			return this.BasePos + this.EnterDir.Rotate(offset);
		}

		// Token: 0x06007FA0 RID: 32672 RVA: 0x004CBAC8 File Offset: 0x004C9CC8
		private ValueTuple<byte, sbyte, byte, short> GetRandomNodeInfo(DataContext context, AdventureBranch branchConfig = null)
		{
			bool flag = branchConfig == null;
			if (flag)
			{
				branchConfig = this._branchConfigData;
			}
			bool flag2 = branchConfig.TerrainPersonalityWeights.Length == 0;
			ValueTuple<byte, sbyte, byte, short> result;
			if (flag2)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(49, 3);
				defaultInterpolatedStringHandler.AppendFormatted(this._branchConfigData.BranchKey);
				defaultInterpolatedStringHandler.AppendLiteral(" from ");
				defaultInterpolatedStringHandler.AppendFormatted<byte>(this._branchConfigData.PortA);
				defaultInterpolatedStringHandler.AppendLiteral(" to ");
				defaultInterpolatedStringHandler.AppendFormatted<byte>(this._branchConfigData.PortB);
				defaultInterpolatedStringHandler.AppendLiteral(" has no personality weight configured..");
				AdaptableLog.Warning(defaultInterpolatedStringHandler.ToStringAndClear(), true);
				result = new ValueTuple<byte, sbyte, byte, short>(1, -1, 0, -1);
			}
			else
			{
				bool flag3 = branchConfig.TerrainPersonalityWeights.Sum((ValueTuple<byte, short, short[]> weight) => (int)weight.Item2) == 0;
				if (flag3)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(43, 3);
					defaultInterpolatedStringHandler.AppendFormatted(this._branchConfigData.BranchKey);
					defaultInterpolatedStringHandler.AppendLiteral(" from ");
					defaultInterpolatedStringHandler.AppendFormatted<byte>(this._branchConfigData.PortA);
					defaultInterpolatedStringHandler.AppendLiteral(" to ");
					defaultInterpolatedStringHandler.AppendFormatted<byte>(this._branchConfigData.PortB);
					defaultInterpolatedStringHandler.AppendLiteral(" personality weight sum is zero..");
					AdaptableLog.Warning(defaultInterpolatedStringHandler.ToStringAndClear(), true);
					result = new ValueTuple<byte, sbyte, byte, short>(1, -1, 0, -1);
				}
				else
				{
					ValueTuple<byte, short[]> terrainInfo = RandomUtils.GetRandomResult<byte, short[]>(branchConfig.TerrainPersonalityWeights, context.Random);
					byte terrainId = terrainInfo.Item1;
					sbyte sevenElement = (sbyte)RandomUtils.GetRandomIndex(terrainInfo.Item2 ?? AdventureTerrain.Instance[(int)terrainId].EvtWeights.ToArray(), context.Random);
					byte skillId = RandomUtils.GetRandomResult<byte>(branchConfig.SkillWeights, context.Random);
					short skillRequire = (short)DomainManager.Adventure.CurAdventureLifeSkillDifficulty * AdvMapBranch.GetBaseSkillRequirement(context.Random);
					result = new ValueTuple<byte, sbyte, byte, short>(terrainId, sevenElement, skillId, skillRequire);
				}
			}
			return result;
		}

		// Token: 0x06007FA1 RID: 32673 RVA: 0x004CBCC0 File Offset: 0x004C9EC0
		public static short GetBaseSkillRequirement(IRandomSource random)
		{
			int totalWeight = 0;
			foreach (AdventureLifeSkillRequirementItem item in ((IEnumerable<AdventureLifeSkillRequirementItem>)AdventureLifeSkillRequirement.Instance))
			{
				totalWeight += (int)item.Weight;
			}
			int randomValue = random.Next(0, totalWeight);
			for (int i = 0; i < AdventureLifeSkillRequirement.Instance.Count; i++)
			{
				randomValue -= (int)AdventureLifeSkillRequirement.Instance[i].Weight;
				bool flag = randomValue < 0;
				if (flag)
				{
					return AdventureLifeSkillRequirement.Instance[i].RequiredValue;
				}
			}
			throw new Exception("Failed to get a random value for base skill requirement");
		}

		// Token: 0x06007FA2 RID: 32674 RVA: 0x004CBD80 File Offset: 0x004C9F80
		private ValueTuple<sbyte, int> GetRandomContent(DataContext context, int terrainId, sbyte personalityType, AdventureBranch branchConfig = null)
		{
			bool flag = branchConfig == null;
			if (flag)
			{
				branchConfig = this._branchConfigData;
			}
			bool flag2 = personalityType < 0;
			ValueTuple<sbyte, int> result;
			if (flag2)
			{
				result = new ValueTuple<sbyte, int>(-1, -1);
			}
			else
			{
				bool flag3 = (int)personalityType >= branchConfig.PersonalityContentWeights.Length;
				if (flag3)
				{
					result = new ValueTuple<sbyte, int>(-1, -1);
				}
				else
				{
					AdventurePersonalityContentWeights contentWeights = branchConfig.PersonalityContentWeights[(int)personalityType];
					int contentType = RandomUtils.GetRandomIndex(contentWeights.ContentTypeWeights, context.Random);
					switch (contentType)
					{
					case 0:
						result = new ValueTuple<sbyte, int>(-1, -1);
						break;
					case 1:
						result = new ValueTuple<sbyte, int>(0, RandomUtils.GetRandomIndex<string>(contentWeights.EventWeights, context.Random));
						break;
					case 2:
						result = new ValueTuple<sbyte, int>(1, RandomUtils.GetRandomIndex<ValueTuple<byte, short, short>>(contentWeights.NormalResWeights, context.Random));
						break;
					case 3:
						result = new ValueTuple<sbyte, int>(2, RandomUtils.GetRandomIndex<ValueTuple<byte, short, short, short>>(contentWeights.SpecialResWeights, context.Random));
						break;
					case 4:
						result = new ValueTuple<sbyte, int>(3, RandomUtils.GetRandomIndex<string>(contentWeights.BonusWeights, context.Random));
						break;
					default:
					{
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(37, 1);
						defaultInterpolatedStringHandler.AppendLiteral("Invalid adventure node content type ");
						defaultInterpolatedStringHandler.AppendFormatted<int>(contentType);
						defaultInterpolatedStringHandler.AppendLiteral(".");
						throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
					}
					}
				}
			}
			return result;
		}

		// Token: 0x06007FA3 RID: 32675 RVA: 0x004CBED4 File Offset: 0x004CA0D4
		public void FillConnect(List<AdventureMapConnect> connects, bool includeEnterConnect = true)
		{
			bool flag = includeEnterConnect && base.EnterNode.NodeType != ENodeType.Start && this.BranchLength != 0;
			if (flag)
			{
				connects.Add(new AdventureMapConnect(base.EnterNode.AdjustedPos.GetHashCode(), this.FirstNode.AdjustedPos.GetHashCode()));
			}
			connects.Add(new AdventureMapConnect(this.LastNode.AdjustedPos.GetHashCode(), base.ExitNode.AdjustedPos.GetHashCode()));
			bool flag2 = this.AdvancedBranch == null;
			if (!flag2)
			{
				connects.Add(new AdventureMapConnect(this.AdvancedBranch.EnterNode.AdjustedPos.GetHashCode(), this.AdvancedBranch.FirstNode.AdjustedPos.GetHashCode())
				{
					EnterLifeSkillType = this.AdvancedBranch.EnterLifeSkillType,
					EnterRequiredVal = this.AdvancedBranch.EnterLifeSkillVal
				});
				connects.Add(new AdventureMapConnect(this.AdvancedBranch.LastNode.AdjustedPos.GetHashCode(), this.AdvancedBranch.ExitNode.AdjustedPos.GetHashCode()));
			}
		}

		// Token: 0x04002300 RID: 8960
		private readonly AdventureBaseBranch _branchConfigData;

		// Token: 0x04002301 RID: 8961
		public AdvancedBranch AdvancedBranch;

		// Token: 0x04002302 RID: 8962
		private readonly List<short> _terrains;

		// Token: 0x04002303 RID: 8963
		private const int ExtraNodeMultiplier = 4;

		// Token: 0x04002304 RID: 8964
		private readonly List<ValueTuple<string, sbyte, short>> _extraEvents;

		// Token: 0x04002305 RID: 8965
		private readonly List<short> _contentWeightDistribution;
	}
}
