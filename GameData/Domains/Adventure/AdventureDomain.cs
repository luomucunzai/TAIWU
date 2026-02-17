using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using Config;
using Config.Common;
using GameData.ArchiveData;
using GameData.Common;
using GameData.Common.SingleValueCollection;
using GameData.Dependencies;
using GameData.DomainEvents;
using GameData.Domains.Adventure.AdventureMap;
using GameData.Domains.Adventure.Modifications;
using GameData.Domains.Character;
using GameData.Domains.Character.Ai;
using GameData.Domains.Character.Creation;
using GameData.Domains.Character.Display;
using GameData.Domains.Character.Filters;
using GameData.Domains.Character.Relation;
using GameData.Domains.Extra;
using GameData.Domains.Item;
using GameData.Domains.Item.Display;
using GameData.Domains.Map;
using GameData.Domains.Map.Filters;
using GameData.Domains.Organization;
using GameData.Domains.Taiwu.Profession;
using GameData.Domains.TaiwuEvent;
using GameData.Domains.TaiwuEvent.Enum;
using GameData.Domains.TaiwuEvent.EventHelper;
using GameData.Domains.TaiwuEvent.MonthlyEventActions;
using GameData.Domains.World;
using GameData.Domains.World.MonthlyEvent;
using GameData.Domains.World.Notification;
using GameData.GameDataBridge;
using GameData.Serializer;
using GameData.Utilities;
using NLog;
using Redzen.Random;

namespace GameData.Domains.Adventure
{
	// Token: 0x020008C4 RID: 2244
	[GameDataDomain(10)]
	public class AdventureDomain : BaseGameDataDomain
	{
		// Token: 0x170004EB RID: 1259
		// (get) Token: 0x06007E8D RID: 32397 RVA: 0x004BCB71 File Offset: 0x004BAD71
		// (set) Token: 0x06007E8C RID: 32396 RVA: 0x004BCB5C File Offset: 0x004BAD5C
		private AdvMapPos PlayerPos
		{
			get
			{
				return new AdvMapPos(this._playerPos);
			}
			set
			{
				this._playerPos = value.GetHashCode();
			}
		}

		// Token: 0x06007E8E RID: 32398 RVA: 0x004BCB80 File Offset: 0x004BAD80
		[DomainMethod]
		public unsafe bool EnterAdventure(DataContext context, short adventureId, List<ItemKey> itemKeys)
		{
			context.SwitchRandomSource("EnterAdventure");
			GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
			this._curAdvSiteLocation = taiwu.GetLocation();
			AdventureSiteData siteData = this._adventureAreas[(int)this._curAdvSiteLocation.AreaId].AdventureSites[this._curAdvSiteLocation.BlockId];
			MapBlockData enterBlock = DomainManager.Map.GetBlock(this._curAdvSiteLocation);
			this._enterTerrain = (int)enterBlock.TemplateId;
			this.EnterTerrainWeights = MapBlock.Instance[this._enterTerrain].AdventureTerrainWeights;
			Personalities personalities = taiwu.GetPersonalities();
			for (sbyte personality = 0; personality < 7; personality += 1)
			{
				this._personalities[(int)personality] = (int)(*(ref personalities.Items.FixedElementField + personality));
			}
			this.SetPersonalities(this._personalities, context);
			this.ClearAdvParameters();
			this.AdventureCfg = Adventure.Instance[adventureId];
			this.InitializeMap(context);
			bool flag = DomainManager.World.GetLeftDaysInCurrMonth() < (int)this.AdventureCfg.TimeCost;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				this.WithholdingActionPoint(context, (int)(this.AdventureCfg.TimeCost * 10));
				this._enterItems.Clear();
				bool flag2 = itemKeys != null && itemKeys.Count > 0;
				if (flag2)
				{
					this._enterItems.AddRange(itemKeys);
				}
				this.SetEnterItems(this._enterItems, context);
				int i = 0;
				for (;;)
				{
					int num = i;
					int? num2 = (itemKeys != null) ? new int?(itemKeys.Count) : null;
					if (!(num < num2.GetValueOrDefault() & num2 != null))
					{
						break;
					}
					bool flag3 = itemKeys[i].ItemType == 12;
					if (!flag3)
					{
						goto IL_1CD;
					}
					bool flag4 = itemKeys[i].TemplateId == 210;
					if (!flag4)
					{
						short templateId = itemKeys[i].TemplateId;
						bool flag5 = templateId >= 200 && templateId <= 209;
						if (!flag5)
						{
							goto IL_1CD;
						}
					}
					IL_1E1:
					i++;
					continue;
					IL_1CD:
					taiwu.RemoveInventoryItem(context, itemKeys[i], 1, false, false);
					goto IL_1E1;
				}
				ResourceInts resourceInts = new ResourceInts((from res in this.AdventureCfg.ResCost
				select res * -1).ToArray<int>());
				taiwu.ChangeResources(context, ref resourceInts);
				EventArgBox argBox = this.CreateAdventureEventArgBox(context, this._curAdvSiteLocation, siteData);
				DomainManager.TaiwuEvent.SetNewAdventure(adventureId, argBox);
				this.SetCurAdventureId(adventureId, context);
				this._resultDisplayData.Clear();
				result = true;
			}
			return result;
		}

		// Token: 0x06007E8F RID: 32399 RVA: 0x004BCE2C File Offset: 0x004BB02C
		private EventArgBox CreateAdventureEventArgBox(DataContext context, Location location, AdventureSiteData siteData)
		{
			EventArgBox eventArgBox = DomainManager.TaiwuEvent.GetEventArgBox();
			eventArgBox.Set("AdventureSite", siteData);
			eventArgBox.Set("AdventureLocation", location);
			bool flag = siteData.MonthlyActionKey.IsValid();
			if (flag)
			{
				MonthlyActionBase monthlyAction = DomainManager.TaiwuEvent.GetMonthlyAction(siteData.MonthlyActionKey);
				monthlyAction.EnsurePrerequisites();
				monthlyAction.FillEventArgBox(eventArgBox);
			}
			return eventArgBox;
		}

		// Token: 0x06007E90 RID: 32400 RVA: 0x004BCE9C File Offset: 0x004BB09C
		public void GenerateAdventureMap(DataContext context, string nodeKey)
		{
			AdventureDomain.Logger.Info("Generating initial branch starting at node \"" + nodeKey + "\"");
			AdvMapNodeVertex startingVertex = null;
			foreach (AdvMapNodeVertex vertex in this._vertices)
			{
				bool flag = !nodeKey.Equals(vertex.NodeKey);
				if (!flag)
				{
					bool flag2 = vertex.NodeType > ENodeType.Start;
					if (flag2)
					{
						throw new Exception("The selected node " + nodeKey + " is not a start node.");
					}
					startingVertex = vertex;
				}
			}
			bool flag3 = startingVertex == null;
			if (flag3)
			{
				throw new Exception("Cannot find the starting node with key " + nodeKey + ".");
			}
			AdvMapNodeVertex endingVertex = startingVertex.ConnectedBranchDict.First<KeyValuePair<AdvMapNodeVertex, AdvMapBranch>>().Key;
			this.SetCurMapTrunk(this.GenerateAdvMapTrunk(context, startingVertex, endingVertex), context);
			this.SetPlayerPos(this._curBranch.EnterNode.AdjustedPos.GetHashCode(), context);
			this.RefreshIndicatePath(context);
			this.SetAdventureState(0, context);
		}

		// Token: 0x06007E91 RID: 32401 RVA: 0x004BCFC8 File Offset: 0x004BB1C8
		[DomainMethod]
		public void ArrangeNode(DataContext context, int pos)
		{
			bool flag = this._adventureState != 1;
			if (!flag)
			{
				AdvMapNodeNormal node = this.GetNormalNodeInCurBranch(pos);
				bool flag2 = node == null;
				if (!flag2)
				{
					bool flag3 = this._arrangedNodes.Contains(pos);
					if (flag3)
					{
						this._arrangedNodes.Remove(pos);
					}
					else
					{
						int[] personalityCost = this.GetPersonalitiesCost();
						bool flag4 = this._personalities[(int)node.SevenElementType] + personalityCost[(int)node.SevenElementType] < (int)node.SevenElementCost;
						if (flag4)
						{
							return;
						}
						this._arrangedNodes.Add(pos);
					}
					this._arrangedNodes.Sort(delegate(int a, int b)
					{
						AdvMapNodeNormal aNode = this.GetNormalNodeInCurBranch(a);
						AdvMapNodeNormal bNode = this.GetNormalNodeInCurBranch(b);
						return aNode.Offset.X.CompareTo(bNode.Offset.X);
					});
					this.RefreshIndicatePath(context);
					this.SetArrangedNodes(this._arrangedNodes, context);
					List<int> keyList = new List<int>();
					foreach (int nodePos in this._perceivedNodes)
					{
						bool flag5 = !this._indicatePath.Contains(nodePos);
						if (flag5)
						{
							keyList.Add(nodePos);
						}
					}
					foreach (int nodePos2 in keyList)
					{
						this.PerceiveNode(context, nodePos2);
					}
					this.SetPerceivedNodes(this._perceivedNodes, context);
				}
			}
		}

		// Token: 0x06007E92 RID: 32402 RVA: 0x004BD14C File Offset: 0x004BB34C
		private AdvMapNodeNormal GetNormalNodeInCurBranch(int pos)
		{
			AdvMapPos advMapPos = new AdvMapPos(pos);
			return this.GetNormalNodeInCurBranch(advMapPos);
		}

		// Token: 0x06007E93 RID: 32403 RVA: 0x004BD170 File Offset: 0x004BB370
		private AdvMapNodeNormal GetNormalNodeInCurBranch(AdvMapPos advMapPos)
		{
			AdvMapNode node = this._curBranch.GetNode(advMapPos);
			bool flag = node == null && this._curBranchChosenChar > -1;
			if (flag)
			{
				node = this._curBranch.AdvancedBranch.GetNode(advMapPos);
			}
			return node as AdvMapNodeNormal;
		}

		// Token: 0x06007E94 RID: 32404 RVA: 0x004BD1BC File Offset: 0x004BB3BC
		[DomainMethod]
		public void PerceiveNode(DataContext context, int pos)
		{
			bool flag = this._adventureState != 1;
			if (!flag)
			{
				bool flag2 = this._perceivedNodes.Contains(pos);
				if (flag2)
				{
					this._perceivedNodes.Remove(pos);
				}
				else
				{
					AdvMapNodeNormal node = this.GetNormalNodeInCurBranch(pos);
					sbyte perceptiveCost = node.SevenElementCost;
					int[] personalitiesCost = this.GetPersonalitiesCost();
					bool flag3 = this._personalities[6] + personalitiesCost[6] < (int)perceptiveCost;
					if (flag3)
					{
						return;
					}
					this._perceivedNodes.Add(pos);
				}
				this.SetPerceivedNodes(this._perceivedNodes, context);
			}
		}

		// Token: 0x06007E95 RID: 32405 RVA: 0x004BD24C File Offset: 0x004BB44C
		[DomainMethod]
		public void ConfirmPath(DataContext context)
		{
			bool flag = this._adventureState != 1;
			if (!flag)
			{
				this.SetAdventureState(2, context);
				this._eventBlock = false;
				this.InitPathContent(context);
				this.MoveForward(context);
			}
		}

		// Token: 0x06007E96 RID: 32406 RVA: 0x004BD28C File Offset: 0x004BB48C
		[DomainMethod]
		public ValueTuple<int, int> ConfirmArrived(DataContext context)
		{
			bool eventBlock = this._eventBlock;
			ValueTuple<int, int> result;
			if (eventBlock)
			{
				result = new ValueTuple<int, int>(-1, -1);
			}
			else
			{
				switch (this.CurNode.NodeType)
				{
				case ENodeType.Normal:
				{
					AdvBaseMapBranch branch = (this._curBranch.AdvancedBranch != null && this._curBranch.AdvancedBranch.Nodes.Contains(this.CurNode)) ? this._curBranch.AdvancedBranch : this._curBranch;
					int branchId = branch.BranchIndex;
					AdventureBranch branchCfg = (branchId < this.AdventureCfg.BaseBranches.Count) ? this.AdventureCfg.BaseBranches[branchId] : this.AdventureCfg.AdvancedBranches[branchId - this.AdventureCfg.BaseBranches.Count];
					AdventureMapPoint pointData = this.CurNode.ToAdventureMapPoint();
					sbyte item = this.CurNode.NodeContent.Item1;
					sbyte b = item;
					switch (b)
					{
					case -1:
						this.TriggerGlobalEvent(branchCfg, pointData, delegate
						{
							this.OnEventHandleFinished(context);
						});
						break;
					case 0:
						return this.HandleNodeContent_Event(context, branchCfg, pointData);
					case 1:
						return this.HandleNodeContent_Resource(context, branchCfg, pointData);
					case 2:
						return this.HandleNodeContent_Item(context, branchCfg, pointData);
					case 3:
						return this.HandleNodeContent_Bonus(context, branchCfg, pointData);
					default:
						if (b != 10)
						{
							throw new Exception("Invalid node content type.");
						}
						return this.HandleNodeContent_ExtraEvent(context, branchCfg, pointData);
					}
					break;
				}
				case ENodeType.Start:
					this.SetAdventureState(0, context);
					DomainManager.TaiwuEvent.OnEvent_AdventureReachStartNode((short)this.CurNode.Index);
					break;
				case ENodeType.Transfer:
					this.SetAdventureState(0, context);
					DomainManager.TaiwuEvent.OnEvent_AdventureReachTransferNode((short)this.CurNode.Index);
					break;
				case ENodeType.End:
					this.SetAdventureState(0, context);
					DomainManager.TaiwuEvent.OnEvent_AdventureReachEndNode((short)this.CurNode.Index);
					break;
				}
				result = new ValueTuple<int, int>(-1, -1);
			}
			return result;
		}

		// Token: 0x06007E97 RID: 32407 RVA: 0x004BD4E4 File Offset: 0x004BB6E4
		[DomainMethod]
		public void ExitAdventure(DataContext context, bool isAdventureCompleted)
		{
			AdventureDomain.Logger.Info("Leaving adventure " + (isAdventureCompleted ? "with a complete state" : "with a incomplete state"));
			DomainManager.TaiwuEvent.CheckTaiwuStatusImmediately();
			this.RemoveWithheldActionPoint(context, (int)(this.AdventureCfg.TimeCost * 10));
			if (isAdventureCompleted)
			{
				DomainManager.Taiwu.AddLegacyPoint(context, 39, 100);
			}
			AdventureSiteData curSite = this._adventureAreas[(int)this._curAdvSiteLocation.AreaId].AdventureSites[this._curAdvSiteLocation.BlockId];
			MapBlockData enterBlock = DomainManager.Map.GetBlock(this._curAdvSiteLocation);
			GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
			int originExp = taiwu.GetExp() - this._resultDisplayData.ChangedExp;
			int originSpiritualDebt = DomainManager.Extra.GetAreaSpiritualDebt(enterBlock.AreaId);
			ResourceInts originResources = taiwu.GetResources().Subtract(ref this._resultDisplayData.ChangedResources);
			this._resultDisplayData.SetOrigin(this._curAdventureId, originExp, originSpiritualDebt, originResources);
			this._resultDisplayData.IsFinished = isAdventureCompleted;
			bool flag = (isAdventureCompleted && curSite.SiteState != 2) || DomainManager.Adventure.AdventureCfg.Interruptible == 1;
			if (flag)
			{
				this.RemoveAdventureSite(context, this._curAdvSiteLocation.AreaId, this._curAdvSiteLocation.BlockId, false, isAdventureCompleted);
				Logger logger = AdventureDomain.Logger;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(28, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Removing adventure block at ");
				defaultInterpolatedStringHandler.AppendFormatted<Location>(this._curAdvSiteLocation);
				logger.Info(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			this._curAdvSiteLocation = Location.Invalid;
			int i = 0;
			while (i < this._enterItems.Count)
			{
				bool flag2 = this._enterItems[i].ItemType == 12;
				if (!flag2)
				{
					goto IL_208;
				}
				bool flag3 = this._enterItems[i].TemplateId == 210;
				if (!flag3)
				{
					short templateId = this._enterItems[i].TemplateId;
					bool flag4 = templateId >= 200 && templateId <= 209;
					if (!flag4)
					{
						goto IL_208;
					}
				}
				IL_222:
				i++;
				continue;
				IL_208:
				DomainManager.Item.RemoveItem(context, this._enterItems[i]);
				goto IL_222;
			}
			this._enterItems.Clear();
			DomainManager.Taiwu.ClearAllTemporaryRestrictions(context);
			DomainManager.TaiwuEvent.ClearAllMarriageLook();
			DomainManager.TaiwuEvent.SeriesEventTexture = string.Empty;
			DomainManager.Character.RevertAllTemporaryModificationsOfAllCharacters(context);
			this.RestoreSpentCharacters(context);
			this.RemoveAllTemporaryEnemies(context);
			this.RemoveAllTemporaryIntelligentCharacters(context);
			this.SetCurAdventureId(-1, context);
			context.RestoreRandomSource();
		}

		// Token: 0x06007E98 RID: 32408 RVA: 0x004BD798 File Offset: 0x004BB998
		[DomainMethod]
		public void SwitchBranch(DataContext context)
		{
			bool flag = this._mapTrunks.Count <= 1;
			if (flag)
			{
				throw new Exception("Current branch must not be the first branch.");
			}
			bool flag2 = !this._curBranch.EnterNode.ConnectedBranchDict.Any((KeyValuePair<AdvMapNodeVertex, AdvMapBranch> pair) => pair.Value.EnterNode == this._curBranch.EnterNode && this._chosenBranchKey == pair.Value.BranchKey);
			if (flag2)
			{
				throw new Exception("The enter node is not connected with the target branch.");
			}
			this._eventBlock = true;
			this.ConfirmPersonalitiesChange(context);
			this._curBranch.ExitNode.PrevVertex = null;
			this._curBranch.Nodes.Clear();
			this._curBranch.Nodes.Add(this._curBranch.FirstNode);
			this._mapTrunks.Pop();
			this._curMapTrunk = this._mapTrunks.Peek();
			this._curBranch = this._branches[this._curMapTrunk.BranchIndex];
			this.ClearPathArrangement(context);
			this.SetCurMapTrunk(this.GenerateNextAdvMapTrunk(context), context);
			this.SetPlayerPos(this._curBranch.EnterNode.AdjustedPos.GetHashCode(), context);
			this.RefreshIndicatePath(context);
			this.SetAdventureState(1, context);
		}

		// Token: 0x06007E99 RID: 32409 RVA: 0x004BD8CD File Offset: 0x004BBACD
		[DomainMethod]
		public AdventureResultDisplayData GetAdventureResultDisplayData()
		{
			return this._resultDisplayData;
		}

		// Token: 0x06007E9A RID: 32410 RVA: 0x004BD8D8 File Offset: 0x004BBAD8
		[DomainMethod]
		public void SelectGetItem(DataContext context, List<ItemKey> acceptItems, List<int> acceptCounts)
		{
			bool flag = acceptItems == null || acceptCounts == null;
			if (!flag)
			{
				GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
				for (int i = 0; i < acceptItems.Count; i++)
				{
					ItemKey key = acceptItems[i];
					int itemAmount = acceptCounts[i];
					ItemDisplayData itemData = this._resultDisplayData.ItemList.Find((ItemDisplayData d) => d.Key.Equals(key));
					bool flag2 = itemData == null;
					if (!flag2)
					{
						taiwu.AddInventoryItem(context, key, itemAmount, false);
					}
				}
			}
		}

		// Token: 0x06007E9B RID: 32411 RVA: 0x004BD974 File Offset: 0x004BBB74
		private void ConfirmPersonalitiesChange(DataContext context)
		{
			int[] personalitiesCost = this.GetPersonalitiesCost();
			int[] personalitiesGain = this.GetPersonalitiesGain();
			for (int i = 0; i < 7; i++)
			{
				this._personalities[i] += (int)((sbyte)(personalitiesCost[i] + personalitiesGain[i]));
			}
			this.SetPersonalities(this._personalities, context);
		}

		// Token: 0x06007E9C RID: 32412 RVA: 0x004BD9C8 File Offset: 0x004BBBC8
		private void MoveForward(DataContext context)
		{
			AdvMapNode nextNode = this.GetNextNode(context, this.PlayerPos);
			bool flag = nextNode != null;
			if (flag)
			{
				this.SetPlayerPos(nextNode.AdjustedPos.GetHashCode(), context);
				this.RefreshIndicatePath(context);
				this._contentList.RemoveAt(0);
			}
		}

		// Token: 0x06007E9D RID: 32413 RVA: 0x004BDA20 File Offset: 0x004BBC20
		private void RefreshIndicatePath(DataContext context)
		{
			this._path.Clear();
			this._path.Add(this.CurNode);
			for (;;)
			{
				List<AdvMapNode> path = this._path;
				if (path[path.Count - 1] == this._curBranch.ExitNode)
				{
					goto IL_CE;
				}
				List<AdvMapNode> path2 = this._path;
				AdvMapNode node = this.GetNextNode(context, path2[path2.Count - 1]);
				bool flag = node == null;
				if (flag)
				{
					break;
				}
				this._path.Add(node);
			}
			Logger logger = AdventureDomain.Logger;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(22, 1);
			defaultInterpolatedStringHandler.AppendLiteral("node at index ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(this._path.Count - 1);
			defaultInterpolatedStringHandler.AppendLiteral(" is null");
			logger.AppendWarning(defaultInterpolatedStringHandler.ToStringAndClear());
			IL_CE:
			this.SetIndicatePath((from a in this._path
			select a.AdjustedPos.GetHashCode()).ToList<int>(), context);
		}

		// Token: 0x06007E9E RID: 32414 RVA: 0x004BDB34 File Offset: 0x004BBD34
		private AdvMapNode GetNextNode(DataContext context, AdvMapPos pos)
		{
			return this.GetNextNode(context, this._curBranch.GetNode(pos) ?? this._curBranch.AdvancedBranch.GetNode(pos));
		}

		// Token: 0x06007E9F RID: 32415 RVA: 0x004BDB70 File Offset: 0x004BBD70
		private AdvMapNode GetNextNode(DataContext context, AdvMapNode curNode)
		{
			bool flag = curNode == this._curBranch.EnterNode;
			AdvMapNode result2;
			if (flag)
			{
				result2 = this._curBranch.FirstNode;
			}
			else
			{
				bool flag2 = curNode == this._curBranch.ExitNode;
				if (flag2)
				{
					result2 = null;
				}
				else
				{
					bool flag3 = curNode == this._curBranch.LastNode;
					if (flag3)
					{
						result2 = this._curBranch.ExitNode;
					}
					else
					{
						bool flag4 = this._curBranchChosenChar >= 0;
						if (flag4)
						{
							bool advancedHasArranged = false;
							foreach (AdvMapNodeNormal advMapNodeNormal in this._curBranch.AdvancedBranch.Nodes)
							{
								int pos = advMapNodeNormal.AdjustedPos.GetHashCode();
								bool flag5 = this._arrangedNodes.Contains(pos);
								if (flag5)
								{
									advancedHasArranged = true;
									break;
								}
							}
							bool flag6 = advancedHasArranged;
							if (flag6)
							{
								bool flag7 = curNode == this._curBranch.AdvancedBranch.EnterNode;
								if (flag7)
								{
									return this._curBranch.AdvancedBranch.FirstNode;
								}
								bool flag8 = curNode == this._curBranch.AdvancedBranch.LastNode;
								if (flag8)
								{
									return this._curBranch.AdvancedBranch.ExitNode;
								}
							}
						}
						AdvMapNodeNormal curNormalNode = curNode as AdvMapNodeNormal;
						bool flag9 = curNormalNode == null;
						if (flag9)
						{
							throw new Exception("Given node is not a normal node when it should be.");
						}
						List<AdvMapPos> offsets = (from a in this._arrangedNodes.Select(delegate(int a)
						{
							AdvMapPos advMapPos = new AdvMapPos(a);
							bool flag12 = this._curBranch.AdvancedBranch != null;
							if (flag12)
							{
								AdvMapNodeNormal node = this._curBranch.AdvancedBranch.Nodes.Find((AdvMapNodeNormal n) => n.AdjustedPos.Equals(advMapPos));
								bool flag13 = node != null;
								if (flag13)
								{
									advMapPos = this._curBranch.AdvancedBranch.EnterNode.AdjustedPos;
								}
							}
							AdvMapNodeNormal targetNode = this.GetNormalNodeInCurBranch(advMapPos);
							return targetNode.Offset;
						})
						where a.X > curNormalNode.Offset.X
						select a).ToList<AdvMapPos>();
						offsets.Sort((AdvMapPos a, AdvMapPos b) => a.X.CompareTo(b.X));
						AdvMapPos targetOffset = (offsets.Count > 0) ? offsets[0] : this._curBranch.LastNode.Offset;
						List<AdvMapNodeNormal> nexts = this._curBranch.Nodes.Contains(curNode) ? this._curBranch.FindNextNodes(curNormalNode) : this._curBranch.AdvancedBranch.FindNextNodes(curNormalNode);
						nexts.RemoveAll(delegate(AdvMapNodeNormal a)
						{
							bool flag12 = a.Offset.X > targetOffset.X;
							bool result3;
							if (flag12)
							{
								result3 = true;
							}
							else
							{
								AdvMapPos offset = targetOffset - a.Offset;
								result3 = (Math.Abs(offset.Y) > Math.Abs(offset.X));
							}
							return result3;
						});
						bool flag10 = nexts.Count == 0;
						if (flag10)
						{
							throw new Exception("Error Path Calc");
						}
						bool flag11 = nexts.Any((AdvMapNodeNormal a) => a.Offset.Equals(targetOffset));
						AdvMapNodeNormal result;
						if (flag11)
						{
							result = nexts.First((AdvMapNodeNormal a) => a.Offset.Equals(targetOffset));
						}
						else
						{
							CollectionUtils.Sort<AdvMapNodeNormal>(nexts, delegate(AdvMapNodeNormal a, AdvMapNodeNormal b)
							{
								int va = this._personalities[(int)a.SevenElementType];
								int vb = this._personalities[(int)b.SevenElementType];
								bool flag12 = va != vb;
								int result3;
								if (flag12)
								{
									result3 = vb.CompareTo(va);
								}
								else
								{
									int randValue = curNormalNode.GetRandomValue(context.Random);
									result3 = ((nexts.IndexOf(a) + randValue) % nexts.Count).CompareTo((nexts.IndexOf(b) + randValue) % nexts.Count);
								}
								return result3;
							});
							result = nexts[0];
						}
						result2 = result;
					}
				}
			}
			return result2;
		}

		// Token: 0x06007EA0 RID: 32416 RVA: 0x004BDE84 File Offset: 0x004BC084
		private void WithholdingActionPoint(DataContext context, int value)
		{
			DomainManager.Extra.ConsumeActionPoint(context, value);
			this.SetActionPointWithhold(this._actionPointWithhold + value, context);
		}

		// Token: 0x06007EA1 RID: 32417 RVA: 0x004BDEA4 File Offset: 0x004BC0A4
		public void RemoveWithheldActionPoint(DataContext context, int value)
		{
			bool flag = value > this._actionPointWithhold;
			if (flag)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(71, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Error occurred. Not enough withhold action point: need ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(value);
				defaultInterpolatedStringHandler.AppendLiteral(", but only ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(this._actionPointWithhold);
				defaultInterpolatedStringHandler.AppendLiteral(" left");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			this.SetActionPointWithhold(this._actionPointWithhold - value, context);
		}

		// Token: 0x06007EA2 RID: 32418 RVA: 0x004BDF24 File Offset: 0x004BC124
		public int GetAdvParameter(string key)
		{
			bool flag = !this._parameterMap.ContainsKey(key);
			int result;
			if (flag)
			{
				result = -1;
			}
			else
			{
				bool flag2 = !this.AdventureCfg.AdventureParams.Exists((ValueTuple<string, string, string, string> param) => key.Equals(param.Item1));
				if (flag2)
				{
					result = this._parameterMap[key];
				}
				else
				{
					result = this._advParameters[this._parameterMap[key]];
				}
			}
			return result;
		}

		// Token: 0x06007EA3 RID: 32419 RVA: 0x004BDFB4 File Offset: 0x004BC1B4
		public ValueTuple<string, string, string, string> GetAdvParameterConfig(string key)
		{
			return this.AdventureCfg.AdventureParams.First((ValueTuple<string, string, string, string> param) => key.Equals(param.Item1));
		}

		// Token: 0x06007EA4 RID: 32420 RVA: 0x004BDFF0 File Offset: 0x004BC1F0
		public void SetAdvParameter(string key, int val, DataContext context)
		{
			bool flag = !this.AdventureCfg.AdventureParams.Exists((ValueTuple<string, string, string, string> param) => key.Equals(param.Item1));
			if (flag)
			{
				this._parameterMap[key] = val;
			}
			else
			{
				bool flag2 = !this._parameterMap.ContainsKey(key);
				if (flag2)
				{
					this._parameterMap.Add(key, this._advParameters.Count);
					this._advParameters.Add(val);
				}
				else
				{
					this._advParameters[this._parameterMap[key]] = val;
				}
				this.SetAdvParameters(this._advParameters, context);
			}
		}

		// Token: 0x06007EA5 RID: 32421 RVA: 0x004BE0B8 File Offset: 0x004BC2B8
		public void ClearAdvParameters()
		{
			this._advParameters.Clear();
			this._parameterMap.Clear();
		}

		// Token: 0x06007EA6 RID: 32422 RVA: 0x004BE0D3 File Offset: 0x004BC2D3
		public void SelectBranch(DataContext context, string branchKey)
		{
			AdventureDomain.Logger.Info("Selecting branch \"" + branchKey + "\"");
			this._chosenBranchKey = branchKey;
		}

		// Token: 0x06007EA7 RID: 32423 RVA: 0x004BE0F8 File Offset: 0x004BC2F8
		public void AddNextBranchExtraEvent(string eventGuid, sbyte personality, short weight)
		{
			this._extraEvents.Add(new ValueTuple<string, sbyte, short>(eventGuid, personality, weight));
		}

		// Token: 0x06007EA8 RID: 32424 RVA: 0x004BE110 File Offset: 0x004BC310
		public int GetXDistanceToNextTransferNode()
		{
			bool flag = this._curBranch == null;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				result = (int)(this._curBranch.ExitNode.AdjustedPos.X - new AdvMapPos(this._playerPos).X);
			}
			return result;
		}

		// Token: 0x06007EA9 RID: 32425 RVA: 0x004BE160 File Offset: 0x004BC360
		public void OnEventHandleFinished(DataContext context)
		{
			bool flag = this._onEventFinishCallback != null;
			if (flag)
			{
				Action callback = this._onEventFinishCallback;
				this._onEventFinishCallback = null;
				callback();
			}
			else
			{
				bool flag2 = this._adventureState == 4;
				if (!flag2)
				{
					bool flag3 = this.CurNode.NodeType == ENodeType.End;
					if (flag3)
					{
						this.SetAdventureState(3, context);
					}
					else
					{
						bool flag4 = this.CurNode.NodeType == ENodeType.Start;
						if (flag4)
						{
							this.SetAdventureState(1, context);
						}
						else
						{
							bool flag5 = this.CurNode == this._curBranch.ExitNode;
							if (flag5)
							{
								this.ConfirmPersonalitiesChange(context);
								this.SetCurMapTrunk(this.GenerateNextAdvMapTrunk(context), context);
								this.SetAdventureState(1, context);
								this._arrangedNodes.Clear();
								this.SetArrangedNodes(this._arrangedNodes, context);
								this._perceivedNodes.Clear();
								this.SetPerceivedNodes(this._perceivedNodes, context);
								this.RefreshIndicatePath(context);
							}
							else
							{
								bool flag6 = this.CurNode.NodeType == ENodeType.Normal;
								if (flag6)
								{
									this.MoveForward(context);
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x06007EAA RID: 32426 RVA: 0x004BE288 File Offset: 0x004BC488
		public void AddCharacterToResultDisplay(int charId)
		{
			bool flag = this._curAdventureId <= -1;
			if (!flag)
			{
				CharacterDisplayData data = DomainManager.Character.GetCharacterDisplayData(charId);
				this._resultDisplayData.CharList.Add(data);
			}
		}

		// Token: 0x06007EAB RID: 32427 RVA: 0x004BE2C6 File Offset: 0x004BC4C6
		[DomainMethod]
		public void ClearPathArrangement(DataContext context)
		{
			this._arrangedNodes.Clear();
			this._perceivedNodes.Clear();
			this.SetArrangedNodes(this._arrangedNodes, context);
			this.SetPerceivedNodes(this._perceivedNodes, context);
		}

		// Token: 0x06007EAC RID: 32428 RVA: 0x004BE300 File Offset: 0x004BC500
		private void OnInitializedDomainData()
		{
			this._curAdventureId = -2;
			this._parameterMap = new Dictionary<string, int>();
			this._extraEvents = new List<ValueTuple<string, sbyte, short>>();
			this._temporaryIntelligentCharacters = new HashSet<int>();
			this._temporaryEnemies = new HashSet<int>();
			this._mapTrunks = new Stack<AdventureMapTrunk>();
		}

		// Token: 0x06007EAD RID: 32429 RVA: 0x004BE34D File Offset: 0x004BC54D
		private void InitializeOnInitializeGameDataModule()
		{
			this.InitializeAdventureTypes();
		}

		// Token: 0x06007EAE RID: 32430 RVA: 0x004BE357 File Offset: 0x004BC557
		private void InitializeOnEnterNewWorld()
		{
			this.InitializeAdventureAreas();
			this.InitializeBrokenAreaEnemies();
			this._enemyNestCounts = new int[EnemyNest.Instance.Count];
		}

		// Token: 0x06007EAF RID: 32431 RVA: 0x004BE37D File Offset: 0x004BC57D
		private void OnLoadedArchiveData()
		{
			this.InitializeEnemyNestCounts();
		}

		// Token: 0x06007EB0 RID: 32432 RVA: 0x004BE388 File Offset: 0x004BC588
		public override void FixAbnormalDomainArchiveData(DataContext context)
		{
			this.FixConqueredEnemyNests(context);
			bool flag = DomainManager.World.IsCurrWorldBeforeVersion(0, 0, 70, 58);
			if (flag)
			{
				this.FixOutdatedEnemyNests(context);
			}
		}

		// Token: 0x06007EB1 RID: 32433 RVA: 0x004BE3BC File Offset: 0x004BC5BC
		[SingleValueDependency(10, new ushort[]
		{
			12,
			6,
			7,
			13
		})]
		private void CalcPersonalitiesCost(int[] value)
		{
			Array.Fill<int>(value, 0);
			bool flag = this._curMapTrunk.BranchIndex < 0;
			if (!flag)
			{
				AdvMapBranch curBranch = this._branches[this._curMapTrunk.BranchIndex];
				foreach (int hashCode in this._arrangedNodes)
				{
					AdvMapPos pos = new AdvMapPos(hashCode);
					AdvMapNode node = curBranch.GetNode(pos) ?? curBranch.AdvancedBranch.GetNode(pos);
					bool flag2 = node.SevenElementType < 0 || node.SevenElementCost <= 0;
					if (!flag2)
					{
						value[(int)node.SevenElementType] -= (int)node.SevenElementCost;
					}
				}
				foreach (int hashCode2 in this._perceivedNodes)
				{
					AdvMapPos pos2 = new AdvMapPos(hashCode2);
					AdvMapNode node2 = curBranch.GetNode(pos2) ?? curBranch.AdvancedBranch.GetNode(pos2);
					value[6] -= (int)node2.SevenElementCost;
				}
			}
		}

		// Token: 0x06007EB2 RID: 32434 RVA: 0x004BE518 File Offset: 0x004BC718
		[SingleValueDependency(10, new ushort[]
		{
			12,
			6,
			13
		})]
		private void CalcPersonalitiesGain(int[] value)
		{
			Array.Fill<int>(value, 0);
			bool flag = this._curMapTrunk.BranchIndex < 0;
			if (!flag)
			{
				foreach (int hashCode in this._arrangedNodes)
				{
					AdvMapPos advMapPos = new AdvMapPos(hashCode);
					AdvMapNode node = this._curBranch.GetNode(advMapPos);
					bool flag2 = node == null && this._curBranchChosenChar > -1;
					if (flag2)
					{
						node = this._curBranch.AdvancedBranch.GetNode(advMapPos);
					}
					bool flag3 = node.SevenElementCost <= 0;
					if (!flag3)
					{
						value[(int)PersonalityType.Producing[(int)node.SevenElementType]] += (int)node.SevenElementCost;
					}
				}
			}
		}

		// Token: 0x06007EB3 RID: 32435 RVA: 0x004BE600 File Offset: 0x004BC800
		[SingleValueDependency(10, new ushort[]
		{
			1,
			2,
			13,
			21,
			6
		})]
		private void CalcArrangeableNodes(List<int> value)
		{
			bool flag = this._curMapTrunk.BranchIndex < 0;
			if (flag)
			{
				value.Clear();
			}
			else
			{
				AdvMapBranch curBranch = this._branches[this._curMapTrunk.BranchIndex];
				bool hasArrangedOnAdvancedBranch = false;
				List<AdvMapPos> offsets = this._arrangedNodes.Select(delegate(int a)
				{
					AdvMapPos advMapPos = new AdvMapPos(a);
					bool flag16 = this._curBranch.GetNode(new AdvMapPos(a)) != null;
					AdvMapPos offset;
					if (flag16)
					{
						offset = this._curBranch.GetOffset(advMapPos);
					}
					else
					{
						hasArrangedOnAdvancedBranch = true;
						offset = this._curBranch.AdvancedBranch.GetOffset(advMapPos);
					}
					return offset;
				}).ToList<AdvMapPos>();
				bool hasArrangedOnAdvancedBranch2 = hasArrangedOnAdvancedBranch;
				if (hasArrangedOnAdvancedBranch2)
				{
					offsets.Add(this._curBranch.AdvancedBranch.EnterNode.Offset);
				}
				AdvMapPos curAdvanceBranchEnterPos = AdvMapPos.Error;
				AdvMapPos curAdvanceBranchExitPos = AdvMapPos.Error;
				float enterAndStartSlope = 0f;
				float exitAndEndSlope = 0f;
				bool flag2 = curBranch.AdvancedBranch != null && this._curBranchChosenChar >= 0;
				if (flag2)
				{
					curAdvanceBranchEnterPos = curBranch.AdvancedBranch.EnterNode.Offset;
					curAdvanceBranchExitPos = curBranch.AdvancedBranch.ExitNode.Offset;
					enterAndStartSlope = AdvMapNode.CalcSlope(curBranch.AdvancedBranch.EnterNode.AdjustedPos, curBranch.AdvancedBranch.FirstNode.AdjustedPos);
					exitAndEndSlope = AdvMapNode.CalcSlope(curBranch.AdvancedBranch.ExitNode.AdjustedPos, curBranch.AdvancedBranch.LastNode.AdjustedPos);
				}
				int[] personalitiesCost = this.GetPersonalitiesCost();
				value.Clear();
				List<AdvMapNodeNormal> nodes = new List<AdvMapNodeNormal>(curBranch.Nodes);
				bool activeAdvancedBranch = this._curBranchChosenChar >= 0 && curBranch.AdvancedBranch != null;
				bool flag3 = activeAdvancedBranch;
				if (flag3)
				{
					nodes.AddRange(curBranch.AdvancedBranch.Nodes);
				}
				bool hasArrangedNodeAfterAdvancedBranchEnterPoint = activeAdvancedBranch && this._arrangedNodes.Any(delegate(int a)
				{
					AdvMapNodeNormal arrangedNode = this.GetNormalNodeInCurBranch(a);
					return arrangedNode.Offset.X > curAdvanceBranchEnterPos.X;
				});
				using (List<AdvMapNodeNormal>.Enumerator enumerator = nodes.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						AdvMapNodeNormal node = enumerator.Current;
						bool flag4 = offsets.Any(delegate(AdvMapPos pos)
						{
							AdvMapPos offset = node.Offset - pos;
							return Math.Abs(offset.Y) > Math.Abs(offset.X);
						});
						if (!flag4)
						{
							bool flag5 = offsets.Contains(node.Offset);
							if (!flag5)
							{
								bool flag6 = activeAdvancedBranch;
								if (flag6)
								{
									bool nodeIsInAdvancedBranch = curBranch.AdvancedBranch.Nodes.Contains(node);
									bool flag7 = nodeIsInAdvancedBranch && hasArrangedNodeAfterAdvancedBranchEnterPoint;
									if (flag7)
									{
										continue;
									}
									bool flag8 = !nodeIsInAdvancedBranch & hasArrangedOnAdvancedBranch;
									if (flag8)
									{
										bool flag9 = node.Offset.X >= curAdvanceBranchEnterPos.X && node.Offset.X <= curAdvanceBranchExitPos.X;
										if (flag9)
										{
											continue;
										}
										float targetAndStartSlope = AdvMapNode.CalcSlope(curBranch.AdvancedBranch.FirstNode.AdjustedPos, node.AdjustedPos);
										bool flag10 = (enterAndStartSlope > 0f && targetAndStartSlope > 0f && enterAndStartSlope < targetAndStartSlope) || (enterAndStartSlope < 0f && targetAndStartSlope < 0f && enterAndStartSlope > targetAndStartSlope);
										if (flag10)
										{
											continue;
										}
										float targetAndEndSlope = AdvMapNode.CalcSlope(curBranch.AdvancedBranch.LastNode.AdjustedPos, node.AdjustedPos);
										bool flag11 = (exitAndEndSlope > 0f && targetAndStartSlope > 0f && exitAndEndSlope < targetAndEndSlope) || (exitAndEndSlope < 0f && targetAndStartSlope < 0f && exitAndEndSlope > targetAndEndSlope);
										if (flag11)
										{
											continue;
										}
									}
								}
								bool flag12 = node.SevenElementType < 0;
								if (!flag12)
								{
									bool flag13 = (int)node.SevenElementCost > this._personalities[(int)node.SevenElementType] + personalitiesCost[(int)node.SevenElementType];
									if (!flag13)
									{
										value.Add(node.AdjustedPos.GetHashCode());
									}
								}
							}
						}
					}
				}
				bool flag14 = activeAdvancedBranch;
				if (flag14)
				{
					int firstPos = curBranch.AdvancedBranch.FirstNode.AdjustedPos.GetHashCode();
					bool firstIsLost = !value.Contains(firstPos) && !this._arrangedNodes.Contains(firstPos);
					int enterPos = curBranch.AdvancedBranch.EnterNode.AdjustedPos.GetHashCode();
					bool enterIsLost = !value.Contains(enterPos) && !this._arrangedNodes.Contains(enterPos);
					bool flag15 = firstIsLost || enterIsLost;
					if (flag15)
					{
						value.RemoveAll((int pos) => curBranch.AdvancedBranch.Nodes.Exists((AdvMapNodeNormal n) => n.AdjustedPos.GetHashCode() == pos.GetHashCode()));
					}
				}
			}
		}

		// Token: 0x06007EB4 RID: 32436 RVA: 0x004BEB30 File Offset: 0x004BCD30
		private void InitializeAdventureTypes()
		{
			AdventureDomain.AdventureTypes = new List<short>[AdventureType.Instance.Count];
			for (int i = 0; i < AdventureDomain.AdventureTypes.Length; i++)
			{
				bool flag = AdventureDomain.AdventureTypes[i] == null;
				if (flag)
				{
					AdventureDomain.AdventureTypes[i] = new List<short>();
				}
				else
				{
					AdventureDomain.AdventureTypes[i].Clear();
				}
			}
			foreach (AdventureItem adventureItem in ((IEnumerable<AdventureItem>)Adventure.Instance))
			{
				AdventureDomain.AdventureTypes[(int)adventureItem.Type].Add(adventureItem.TemplateId);
			}
		}

		// Token: 0x06007EB5 RID: 32437 RVA: 0x004BEBEC File Offset: 0x004BCDEC
		public void InitializeBrokenAreaEnemies()
		{
			for (int i = 0; i < this._brokenAreaEnemies.Length; i++)
			{
				this._brokenAreaEnemies[i] = new BrokenAreaData();
			}
			AdventureDomain._brokenAreaEnemyBaseCount = GlobalConfig.Instance.BrokenAreaEnemyCountLevelDist.Sum();
		}

		// Token: 0x06007EB6 RID: 32438 RVA: 0x004BEC34 File Offset: 0x004BCE34
		public void GenerateBrokenAreaInitialEnemies(DataContext context)
		{
			int factor = DomainManager.World.GetHereticsAmountFactor();
			List<MapBlockData> validBlocks = new List<MapBlockData>();
			for (int i = 0; i < this._brokenAreaEnemies.Length; i += 6)
			{
				CollectionUtils.Shuffle<sbyte>(context.Random, this._stateBrokenAreaLevels);
				for (int offset = 0; offset < 6; offset++)
				{
					int index = i + offset;
					short areaId = (short)(45 + index);
					this._brokenAreaEnemies[index].Level = this._stateBrokenAreaLevels[offset];
					DomainManager.Adventure.GetValidBlocksForRandomEnemy(areaId, -1, 3, false, false, false, validBlocks);
					for (int baseLevel = 0; baseLevel < 3; baseLevel++)
					{
						short enemyId = (short)(298 + baseLevel + (int)this._brokenAreaEnemies[index].Level - 1);
						int enemyCount = (int)GlobalConfig.Instance.BrokenAreaEnemyCountLevelDist[baseLevel] * factor / 100;
						this.CreateRandomEnemiesOnValidBlocks(context, Location.Invalid, enemyId, enemyCount, validBlocks);
					}
					this.SetElement_BrokenAreaEnemies(index, this._brokenAreaEnemies[index], context);
				}
			}
			DomainManager.Extra.InitializeTreasureMaterials(context, EMiscGenerateType.Any);
		}

		// Token: 0x06007EB7 RID: 32439 RVA: 0x004BED50 File Offset: 0x004BCF50
		public short GetSwordTombAdventureMaxMonthCount()
		{
			byte hardLevel = DomainManager.World.GetBossInvasionSpeedType();
			return GlobalConfig.Instance.SwordTombAdventureLastMonthCount[(int)hardLevel];
		}

		// Token: 0x06007EB8 RID: 32440 RVA: 0x004BED7C File Offset: 0x004BCF7C
		public void StartNextSwordTombCountDown()
		{
			sbyte[] xiangshuAvatarTasksInOrder = DomainManager.World.GetXiangshuAvatarTasksInOrder();
			AreaAdventureData adventuresInArea = DomainManager.Adventure.GetAdventuresInArea(EventArgBox.TaiwuVillageAreaId);
			short toSetRemainingMonth = this.GetSwordTombAdventureMaxMonthCount();
			Dictionary<sbyte, KeyValuePair<short, AdventureSiteData>> xiangshuAvatarIdToAdventureMap = new Dictionary<sbyte, KeyValuePair<short, AdventureSiteData>>();
			foreach (KeyValuePair<short, AdventureSiteData> pair in adventuresInArea.AdventureSites)
			{
				sbyte xiangshuAvatarId = XiangshuAvatarIds.GetXiangshuAvatarIdBySwordTomb(pair.Value.TemplateId);
				bool flag = xiangshuAvatarId < 0;
				if (!flag)
				{
					bool flag2 = pair.Value.RemainingMonths > 0;
					if (flag2)
					{
						bool flag3 = toSetRemainingMonth < 0;
						if (flag3)
						{
							DomainManager.Adventure.SetAdventureRemainingMonths(DomainManager.TaiwuEvent.MainThreadDataContext, EventArgBox.TaiwuVillageAreaId, pair.Key, toSetRemainingMonth);
						}
						return;
					}
					xiangshuAvatarIdToAdventureMap.Add(xiangshuAvatarId, pair);
				}
			}
			foreach (sbyte xiangshuAvatarId2 in xiangshuAvatarTasksInOrder)
			{
				KeyValuePair<short, AdventureSiteData> pair2;
				bool flag4 = xiangshuAvatarIdToAdventureMap.TryGetValue(xiangshuAvatarId2, out pair2);
				if (flag4)
				{
					DomainManager.Adventure.SetAdventureRemainingMonths(DomainManager.TaiwuEvent.MainThreadDataContext, EventArgBox.TaiwuVillageAreaId, pair2.Key, toSetRemainingMonth);
					break;
				}
			}
		}

		// Token: 0x06007EB9 RID: 32441 RVA: 0x004BEEC0 File Offset: 0x004BD0C0
		public void StopAllSwordTombAdventureCountDown(DataContext context)
		{
			AreaAdventureData adventuresInArea = DomainManager.Adventure.GetAdventuresInArea(EventArgBox.TaiwuVillageAreaId);
			foreach (KeyValuePair<short, AdventureSiteData> pair in adventuresInArea.AdventureSites)
			{
				bool flag = XiangshuAvatarIds.GetXiangshuAvatarIdBySwordTomb(pair.Value.TemplateId) < 0;
				if (!flag)
				{
					pair.Value.RemainingMonths = -1;
					this.SetElement_AdventureAreas((int)EventArgBox.TaiwuVillageAreaId, this._adventureAreas[(int)EventArgBox.TaiwuVillageAreaId], context);
				}
			}
		}

		// Token: 0x06007EBA RID: 32442 RVA: 0x004BEF64 File Offset: 0x004BD164
		public bool IsAdventureVisible(short adventureTemplateId)
		{
			int i = 0;
			int max = this._adventureAreas.Length;
			while (i < max)
			{
				AreaAdventureData adventuresInArea = this._adventureAreas[i];
				foreach (KeyValuePair<short, AdventureSiteData> pair in adventuresInArea.AdventureSites)
				{
					bool flag = pair.Value.TemplateId == adventureTemplateId;
					if (flag)
					{
						return pair.Value.SiteState == 1;
					}
				}
				i++;
			}
			return false;
		}

		// Token: 0x06007EBB RID: 32443 RVA: 0x004BF00C File Offset: 0x004BD20C
		public void GetCharactersInAdventure(Location location, HashSet<int> charIds)
		{
			charIds.Clear();
			AreaAdventureData areaAdventureData = this._adventureAreas[(int)location.AreaId];
			bool flag = !areaAdventureData.AdventureSites.ContainsKey(location.BlockId);
			if (!flag)
			{
				AdventureSiteData site = areaAdventureData.AdventureSites[location.BlockId];
				MonthlyActionBase action = DomainManager.TaiwuEvent.GetMonthlyAction(site.MonthlyActionKey);
				bool flag2 = action == null;
				if (!flag2)
				{
					IMonthlyActionGroup actionGroup = action as IMonthlyActionGroup;
					bool flag3 = actionGroup != null;
					if (flag3)
					{
						ConfigMonthlyAction subAction = actionGroup.GetConfigAction(location.AreaId, location.BlockId);
						bool flag4 = subAction == null;
						if (flag4)
						{
							return;
						}
						subAction.CollectCalledCharacters(charIds);
					}
					else
					{
						action.CollectCalledCharacters(charIds);
					}
					charIds.RemoveWhere(delegate(int charId)
					{
						GameData.Domains.Character.Character character;
						bool flag5 = !DomainManager.Character.TryGetElement_Objects(charId, out character);
						bool result;
						if (flag5)
						{
							result = true;
						}
						else
						{
							bool flag6 = !character.GetLocation().Equals(location);
							result = flag6;
						}
						return result;
					});
				}
			}
		}

		// Token: 0x06007EBC RID: 32444 RVA: 0x004BF100 File Offset: 0x004BD300
		[DomainMethod]
		public List<CharacterDisplayData> GetCharacterDisplayDataListInAdventure(Location location)
		{
			HashSet<int> set = new HashSet<int>();
			this.GetCharactersInAdventure(location, set);
			return DomainManager.Character.GetCharacterDisplayDataList(set.ToList<int>());
		}

		// Token: 0x06007EBD RID: 32445 RVA: 0x004BF134 File Offset: 0x004BD334
		[DomainMethod]
		public AreaAdventureData GetAdventuresInArea(short areaId)
		{
			return this._adventureAreas[(int)areaId];
		}

		// Token: 0x06007EBE RID: 32446 RVA: 0x004BF150 File Offset: 0x004BD350
		[DomainMethod]
		public Location QueryAdventureLocation(short templateId)
		{
			short areaId = 0;
			while ((int)areaId < this._adventureAreas.Length)
			{
				Location location;
				bool flag = this.TryQueryAdventureLocation(templateId, areaId, out location);
				if (flag)
				{
					return location;
				}
				areaId += 1;
			}
			return Location.Invalid;
		}

		// Token: 0x06007EBF RID: 32447 RVA: 0x004BF194 File Offset: 0x004BD394
		[DomainMethod]
		public Location QueryAdventureLocationFirstInCurrent(short templateId)
		{
			short currentArea = DomainManager.Taiwu.GetTaiwu().GetLocation().AreaId;
			Location location;
			bool flag = currentArea >= 0 && (int)currentArea < this._adventureAreas.Length && this.TryQueryAdventureLocation(templateId, currentArea, out location);
			Location result;
			if (flag)
			{
				result = location;
			}
			else
			{
				result = this.QueryAdventureLocation(templateId);
			}
			return result;
		}

		// Token: 0x06007EC0 RID: 32448 RVA: 0x004BF1E8 File Offset: 0x004BD3E8
		private bool TryQueryAdventureLocation(short templateId, short areaId, out Location location)
		{
			AreaAdventureData adventureArea = this._adventureAreas[(int)areaId];
			foreach (KeyValuePair<short, AdventureSiteData> kvp in adventureArea.AdventureSites)
			{
				bool flag = kvp.Value.TemplateId != templateId;
				if (!flag)
				{
					location = new Location(areaId, kvp.Key);
					return true;
				}
			}
			location = Location.Invalid;
			return false;
		}

		// Token: 0x06007EC1 RID: 32449 RVA: 0x004BF284 File Offset: 0x004BD484
		[DomainMethod]
		public Dictionary<Location, AdventureSiteData> GetAdventureSiteDataDict(List<Location> locationList)
		{
			Dictionary<Location, AdventureSiteData> adventureSiteDataDict = new Dictionary<Location, AdventureSiteData>();
			bool flag = locationList != null;
			if (flag)
			{
				foreach (Location location in locationList)
				{
					AdventureSiteData adventureSite;
					bool flag2 = this._adventureAreas[(int)location.AreaId].AdventureSites.TryGetValue(location.BlockId, out adventureSite);
					if (flag2)
					{
						adventureSiteDataDict.Add(location, adventureSite);
					}
				}
			}
			return adventureSiteDataDict;
		}

		// Token: 0x06007EC2 RID: 32450 RVA: 0x004BF314 File Offset: 0x004BD514
		[DomainMethod]
		public List<short> GetAwakeSwordTombs()
		{
			List<short> awakeSwordTombs = new List<short>();
			short areaId = DomainManager.Taiwu.GetTaiwuVillageLocation().AreaId;
			AreaAdventureData areaAdventures = this._adventureAreas[(int)areaId];
			foreach (KeyValuePair<short, AdventureSiteData> keyValuePair in areaAdventures.AdventureSites)
			{
				short num;
				AdventureSiteData adventureSiteData2;
				keyValuePair.Deconstruct(out num, out adventureSiteData2);
				AdventureSiteData adventureSiteData = adventureSiteData2;
				bool flag = XiangshuAvatarIds.GetXiangshuAvatarIdBySwordTomb(adventureSiteData.TemplateId) < 0;
				if (!flag)
				{
					bool flag2 = adventureSiteData.RemainingMonths > 0;
					if (flag2)
					{
						awakeSwordTombs.Add(adventureSiteData.TemplateId);
					}
				}
			}
			return awakeSwordTombs;
		}

		// Token: 0x06007EC3 RID: 32451 RVA: 0x004BF3D8 File Offset: 0x004BD5D8
		[DomainMethod]
		public List<short> GetAttackingSwordTombs()
		{
			List<short> attackingSwordTombs = new List<short>();
			short areaId = DomainManager.Taiwu.GetTaiwuVillageLocation().AreaId;
			AreaAdventureData areaAdventures = this._adventureAreas[(int)areaId];
			foreach (KeyValuePair<short, AdventureSiteData> keyValuePair in areaAdventures.AdventureSites)
			{
				short num;
				AdventureSiteData adventureSiteData2;
				keyValuePair.Deconstruct(out num, out adventureSiteData2);
				AdventureSiteData adventureSiteData = adventureSiteData2;
				sbyte xiangshuAvatarId = XiangshuAvatarIds.GetXiangshuAvatarIdBySwordTomb(adventureSiteData.TemplateId);
				bool flag = xiangshuAvatarId < 0;
				if (!flag)
				{
					bool flag2 = adventureSiteData.RemainingMonths > 0;
					if (!flag2)
					{
						sbyte xiangshuLevel = DomainManager.World.GetXiangshuLevel();
						short xiangshuTemplateId = XiangshuAvatarIds.GetCurrentLevelXiangshuTemplateId(xiangshuAvatarId, xiangshuLevel, true);
						GameData.Domains.Character.Character xiangshu;
						bool flag3 = DomainManager.Character.TryGetFixedCharacterByTemplateId(xiangshuTemplateId, out xiangshu) && xiangshu.GetLocation().IsValid();
						if (flag3)
						{
							attackingSwordTombs.Add(adventureSiteData.TemplateId);
						}
					}
				}
			}
			return attackingSwordTombs;
		}

		// Token: 0x06007EC4 RID: 32452 RVA: 0x004BF4E8 File Offset: 0x004BD6E8
		public bool TryCreateAdventureSite(DataContext context, short areaId, short blockId, short adventureId, MonthlyActionKey monthlyActionKey)
		{
			AdventureItem adventureConfig = Adventure.Instance[adventureId];
			bool flag = adventureConfig.RestrictedByWorldPopulation && !AdventureDomain.CheckPassPopulationRestriction(context.Random);
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				AdventureSiteData siteData = new AdventureSiteData(adventureId, (short)adventureConfig.KeepTime, monthlyActionKey);
				AdventureSiteData conflictSite;
				bool flag2 = this._adventureAreas[(int)areaId].AdventureSites.TryGetValue(blockId, out conflictSite);
				if (flag2)
				{
					AdventureItem conflictAdventureCfg = Adventure.Instance[conflictSite.TemplateId];
					bool flag3 = conflictSite.TemplateId == adventureId;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
					if (flag3)
					{
						Logger logger = AdventureDomain.Logger;
						defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(52, 3);
						defaultInterpolatedStringHandler.AppendLiteral("Skipping Recreation of duplicate adventure ");
						defaultInterpolatedStringHandler.AppendFormatted(adventureConfig.Name);
						defaultInterpolatedStringHandler.AppendLiteral(" at (");
						defaultInterpolatedStringHandler.AppendFormatted<short>(areaId);
						defaultInterpolatedStringHandler.AppendLiteral(", ");
						defaultInterpolatedStringHandler.AppendFormatted<short>(blockId);
						defaultInterpolatedStringHandler.AppendLiteral(").");
						logger.Warn(defaultInterpolatedStringHandler.ToStringAndClear());
						return true;
					}
					bool isTrivial = AdventureType.Instance[conflictAdventureCfg.Type].IsTrivial;
					defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(67, 4);
					defaultInterpolatedStringHandler.AppendLiteral("(");
					defaultInterpolatedStringHandler.AppendFormatted<short>(areaId);
					defaultInterpolatedStringHandler.AppendLiteral(",");
					defaultInterpolatedStringHandler.AppendFormatted<short>(blockId);
					defaultInterpolatedStringHandler.AppendLiteral(") already contains an adventure ");
					defaultInterpolatedStringHandler.AppendFormatted(conflictAdventureCfg.Name);
					defaultInterpolatedStringHandler.AppendLiteral(" which cannot be overwritten by ");
					defaultInterpolatedStringHandler.AppendFormatted(adventureConfig.Name);
					defaultInterpolatedStringHandler.AppendLiteral(".");
					Tester.Assert(isTrivial, defaultInterpolatedStringHandler.ToStringAndClear());
					bool condition = !AdventureType.Instance[adventureConfig.Type].IsTrivial;
					defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(64, 4);
					defaultInterpolatedStringHandler.AppendLiteral("Trying to overwrite adventure ");
					defaultInterpolatedStringHandler.AppendFormatted(conflictAdventureCfg.Name);
					defaultInterpolatedStringHandler.AppendLiteral(" at (");
					defaultInterpolatedStringHandler.AppendFormatted<short>(areaId);
					defaultInterpolatedStringHandler.AppendLiteral(",");
					defaultInterpolatedStringHandler.AppendFormatted<short>(blockId);
					defaultInterpolatedStringHandler.AppendLiteral(") with a trivial adventure ");
					defaultInterpolatedStringHandler.AppendFormatted(adventureConfig.Name);
					defaultInterpolatedStringHandler.AppendLiteral(".");
					Tester.Assert(condition, defaultInterpolatedStringHandler.ToStringAndClear());
					this.RemoveAdventureSite(context, areaId, blockId, false, false);
				}
				this._adventureAreas[(int)areaId].AdventureSites.Add(blockId, siteData);
				bool flag4 = siteData.IsEnemyNest();
				if (flag4)
				{
					sbyte enemyNestTemplateId = AdventureDomain.GetEnemyNestTemplateId(adventureId);
					this.RegisterEnemyNest((short)enemyNestTemplateId);
					this.AddElement_EnemyNestSites(new Location(areaId, blockId), new EnemyNestSiteExtraData(enemyNestTemplateId), context);
				}
				this.SetElement_AdventureAreas((int)areaId, this._adventureAreas[(int)areaId], context);
				Events.RaiseAdventureSiteStateChanged(context, areaId, blockId, siteData);
				result = true;
			}
			return result;
		}

		// Token: 0x06007EC5 RID: 32453 RVA: 0x004BF7A8 File Offset: 0x004BD9A8
		public void ActivateAdventureSite(DataContext context, short areaId, short blockId)
		{
			AdventureSiteData advSite = this._adventureAreas[(int)areaId].AdventureSites[blockId];
			bool flag = advSite.SiteState != 0;
			if (flag)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(66, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Adventure Site at area ");
				defaultInterpolatedStringHandler.AppendFormatted<short>(areaId);
				defaultInterpolatedStringHandler.AppendLiteral(" and block ");
				defaultInterpolatedStringHandler.AppendFormatted<short>(blockId);
				defaultInterpolatedStringHandler.AppendLiteral(" is not waiting to be activated.");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			advSite.SiteState = 1;
			this.SetElement_AdventureAreas((int)areaId, this._adventureAreas[(int)areaId], context);
			bool flag2 = advSite.IsEnemyNest();
			if (flag2)
			{
				this.GenerateRandomEnemiesBySite(context, advSite, areaId, blockId);
			}
			Events.RaiseAdventureSiteStateChanged(context, areaId, blockId, advSite);
		}

		// Token: 0x06007EC6 RID: 32454 RVA: 0x004BF860 File Offset: 0x004BDA60
		public void DeactivateAdventureSite(DataContext context, short areaId, short blockId)
		{
			AdventureSiteData advSite = this._adventureAreas[(int)areaId].AdventureSites[blockId];
			advSite.SiteState = 0;
			this.SetElement_AdventureAreas((int)areaId, this._adventureAreas[(int)areaId], context);
		}

		// Token: 0x06007EC7 RID: 32455 RVA: 0x004BF89C File Offset: 0x004BDA9C
		public void SetAdventureSiteRemainingMonths(DataContext context, short areaId, short blockId, sbyte remainingMonths)
		{
			AdventureSiteData advSite = this._adventureAreas[(int)areaId].AdventureSites[blockId];
			advSite.RemainingMonths = (short)remainingMonths;
			this.SetElement_AdventureAreas((int)areaId, this._adventureAreas[(int)areaId], context);
		}

		// Token: 0x06007EC8 RID: 32456 RVA: 0x004BF8D8 File Offset: 0x004BDAD8
		private void ClearRandomEnemiesBySite(DataContext context, Location nestLocation)
		{
			EnemyNestSiteExtraData nestData = this.GetElement_EnemyNestSites(nestLocation);
			foreach (MapTemplateEnemyInfo randEnemy in nestData.RandomEnemies)
			{
				DomainManager.Map.OnTemplateEnemyLocationChanged(context, randEnemy, new Location(nestLocation.AreaId, randEnemy.BlockId), Location.Invalid);
			}
			nestData.RandomEnemies.Clear();
			this.SetElement_EnemyNestSites(nestLocation, nestData, context);
		}

		// Token: 0x06007EC9 RID: 32457 RVA: 0x004BF96C File Offset: 0x004BDB6C
		private void GenerateRandomEnemiesBySite(DataContext context, AdventureSiteData advSite, short areaId, short blockId)
		{
			Location location = new Location(areaId, blockId);
			short enemyNestTemplateId = EnemyNest.Instance.First((EnemyNestItem nest) => nest.AdventureId == advSite.TemplateId).TemplateId;
			List<short> enemyIds = EnemyNest.Instance[enemyNestTemplateId].Members;
			List<short> baseSpawnAmount = EnemyNest.Instance[enemyNestTemplateId].SpawnAmountFactors;
			int factor = DomainManager.World.GetHereticsAmountFactor();
			List<MapBlockData> validBlocks = ObjectPool<List<MapBlockData>>.Instance.Get();
			DomainManager.Adventure.GetValidBlocksForRandomEnemy(areaId, blockId, 3, false, false, false, validBlocks);
			bool flag = enemyNestTemplateId == 3;
			if (flag)
			{
				sbyte stateTemplateId = DomainManager.Map.GetStateTemplateIdByAreaId(areaId);
				sbyte regionalSectId = MapState.Instance[stateTemplateId].SectID;
				int sectIndex = (int)(regionalSectId - 1);
				DomainManager.Adventure.SetAdventureSiteInitData(context, areaId, blockId, sectIndex);
				bool flag2 = validBlocks.Count > 0;
				if (flag2)
				{
					DomainManager.Adventure.CreateRandomEnemiesOnValidBlocks(context, location, enemyIds[sectIndex], (int)baseSpawnAmount[sectIndex] * factor / 100, validBlocks);
				}
			}
			else
			{
				bool flag3 = validBlocks.Count > 0;
				if (flag3)
				{
					for (int i = 0; i < enemyIds.Count; i++)
					{
						DomainManager.Adventure.CreateRandomEnemiesOnValidBlocks(context, location, enemyIds[i], (int)baseSpawnAmount[i] * factor / 100, validBlocks);
					}
				}
			}
			ObjectPool<List<MapBlockData>>.Instance.Return(validBlocks);
		}

		// Token: 0x06007ECA RID: 32458 RVA: 0x004BFAD5 File Offset: 0x004BDCD5
		[Obsolete("Use GameData.Domains.Map.MapDomain.GetRandomMapBlockDataByFilters instead.")]
		public MapBlockData GetRandomMapBlockDataByFilters(DataContext context, sbyte stateTemplateId, sbyte areaFilterType, List<short> mapBlockSubTypes)
		{
			return DomainManager.Map.GetRandomMapBlockDataByFilters(context.Random, stateTemplateId, areaFilterType, mapBlockSubTypes, false);
		}

		// Token: 0x06007ECB RID: 32459 RVA: 0x004BFAEC File Offset: 0x004BDCEC
		[Obsolete("use GameData.Domains.Map.MapDomain.GetMapBlocksInAreaByFilters instead.")]
		public void GetMapBlocksInAreaByFilters(short areaId, Predicate<MapBlockData> predicate, List<MapBlockData> result)
		{
			DomainManager.Map.GetMapBlocksInAreaByFilters(areaId, predicate, result);
		}

		// Token: 0x06007ECC RID: 32460 RVA: 0x004BFAFC File Offset: 0x004BDCFC
		[Obsolete("use GameData.Domains.Map.MapDomain.GetRandomMapBlockDataInAreaByFilters")]
		public MapBlockData GetRandomMapBlockDataInAreaByFilters(DataContext context, short areaId, List<short> mapBlockSubTypes)
		{
			return DomainManager.Map.GetRandomMapBlockDataInAreaByFilters(context.Random, areaId, mapBlockSubTypes, false);
		}

		// Token: 0x06007ECD RID: 32461 RVA: 0x004BFB14 File Offset: 0x004BDD14
		public unsafe void GetValidBlocksForRandomEnemy(short areaId, short centerBlockId, short maxSteps, bool onAdventureSite, bool onSettlement, bool nearTaiwu, List<MapBlockData> validBlocks)
		{
			validBlocks.Clear();
			List<MapBlockData> taiwuLocationNeighbors = ObjectPool<List<MapBlockData>>.Instance.Get();
			GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
			Location taiwuLocation = (taiwu != null) ? taiwu.GetLocation() : Location.Invalid;
			bool flag = !nearTaiwu && taiwuLocation.IsValid();
			if (flag)
			{
				DomainManager.Map.GetRealNeighborBlocks(taiwuLocation.AreaId, taiwuLocation.BlockId, taiwuLocationNeighbors, 1, false);
			}
			Dictionary<short, AdventureSiteData> sitesInArea = this._adventureAreas[(int)areaId].AdventureSites;
			bool flag2 = centerBlockId != -1;
			if (flag2)
			{
				List<MapBlockData> curBlockNeighbors = ObjectPool<List<MapBlockData>>.Instance.Get();
				DomainManager.Map.GetRealNeighborBlocks(areaId, centerBlockId, curBlockNeighbors, (int)maxSteps, false);
				foreach (MapBlockData blockData in curBlockNeighbors)
				{
					bool flag3 = (nearTaiwu || taiwuLocation.AreaId != areaId || (taiwuLocation.BlockId != blockData.BlockId && !taiwuLocationNeighbors.Contains(blockData))) && (onAdventureSite || !sitesInArea.ContainsKey(blockData.BlockId)) && (onSettlement || (!blockData.IsCityTown() && blockData.BlockType != EMapBlockType.Station)) && MapBlockDataMatchers.IsValidForRandomEnemy(blockData);
					if (flag3)
					{
						validBlocks.Add(blockData);
					}
				}
				curBlockNeighbors.Clear();
				ObjectPool<List<MapBlockData>>.Instance.Return(curBlockNeighbors);
			}
			else
			{
				Span<MapBlockData> areaBlocks = DomainManager.Map.GetAreaBlocks(areaId);
				Span<MapBlockData> span = areaBlocks;
				for (int i = 0; i < span.Length; i++)
				{
					MapBlockData blockData2 = *span[i];
					bool flag4 = (nearTaiwu || taiwuLocation.AreaId != areaId || (taiwuLocation.BlockId != blockData2.BlockId && !taiwuLocationNeighbors.Contains(blockData2))) && (onAdventureSite || !sitesInArea.ContainsKey(blockData2.BlockId)) && (onSettlement || (!blockData2.IsCityTown() && blockData2.BlockType != EMapBlockType.Station)) && MapBlockDataMatchers.IsValidForRandomEnemy(blockData2);
					if (flag4)
					{
						validBlocks.Add(blockData2);
					}
				}
			}
			ObjectPool<List<MapBlockData>>.Instance.Return(taiwuLocationNeighbors);
		}

		// Token: 0x06007ECE RID: 32462 RVA: 0x004BFD30 File Offset: 0x004BDF30
		public void CreateRandomEnemiesOnValidBlocks(DataContext context, Location sourceNestLocation, short enemyTemplateId, int enemyAmount, IList<MapBlockData> validBlocks)
		{
			this.CreateTemporaryEnemiesOnValidBlocks(context, sourceNestLocation, enemyTemplateId, enemyAmount, validBlocks, -1);
		}

		// Token: 0x06007ECF RID: 32463 RVA: 0x004BFD44 File Offset: 0x004BDF44
		public void CreateTemporaryEnemiesOnValidBlocks(DataContext context, Location sourceNestLocation, short enemyTemplateId, int enemyAmount, IList<MapBlockData> validBlocks, sbyte duration = -128)
		{
			bool flag = enemyTemplateId <= 0;
			if (!flag)
			{
				byte creatingType = Config.Character.Instance[enemyTemplateId].CreatingType;
				bool condition = creatingType - 2 <= 1;
				Tester.Assert(condition, "");
				bool flag2 = sourceNestLocation.IsValid() && duration != -1;
				if (flag2)
				{
					string tag = "CreateTemporaryEnemiesOnValidBlocks";
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(92, 2);
					defaultInterpolatedStringHandler.AppendLiteral("Creating Enemy with sourceNestLocation and duration = ");
					defaultInterpolatedStringHandler.AppendFormatted<sbyte>(duration);
					defaultInterpolatedStringHandler.AppendLiteral(" is invalid, changing duration to -1:\n");
					defaultInterpolatedStringHandler.AppendFormatted<StackTrace>(new StackTrace());
					AdaptableLog.TagWarning(tag, defaultInterpolatedStringHandler.ToStringAndClear(), false);
					duration = -1;
				}
				else
				{
					bool flag3 = duration == sbyte.MinValue;
					if (flag3)
					{
						duration = MapTemplateEnemyInfo.DefaultDuration((int)DomainManager.Taiwu.GetTaiwu().GetConsummateLevel());
					}
				}
				for (int i = 0; i < enemyAmount; i++)
				{
					bool flag4 = validBlocks.Count == 0;
					if (flag4)
					{
						break;
					}
					int index = context.Random.Next(0, validBlocks.Count);
					Location validBlockLocation = new Location(validBlocks[index].AreaId, validBlocks[index].BlockId);
					MapTemplateEnemyInfo mapRandomEnemyInfo = new MapTemplateEnemyInfo(enemyTemplateId, validBlockLocation.BlockId, sourceNestLocation.BlockId, duration);
					Events.RaiseTemplateEnemyLocationChanged(context, mapRandomEnemyInfo, Location.Invalid, validBlockLocation);
				}
			}
		}

		// Token: 0x06007ED0 RID: 32464 RVA: 0x004BFEAC File Offset: 0x004BE0AC
		private void InitializeAdventureAreas()
		{
			for (int i = 0; i < this._adventureAreas.Length; i++)
			{
				this._adventureAreas[i] = new AreaAdventureData();
			}
		}

		// Token: 0x06007ED1 RID: 32465 RVA: 0x004BFEE0 File Offset: 0x004BE0E0
		public void ClearAllAdventureSites(DataContext context)
		{
			List<short> blockIds = ObjectPool<List<short>>.Instance.Get();
			short areaId = 0;
			while ((int)areaId < this._adventureAreas.Length)
			{
				AreaAdventureData advArea = this._adventureAreas[(int)areaId];
				blockIds.AddRange(advArea.AdventureSites.Keys);
				foreach (short blockId in blockIds)
				{
					this.RemoveAdventureSite(context, areaId, blockId, false, false);
				}
				areaId += 1;
			}
		}

		// Token: 0x06007ED2 RID: 32466 RVA: 0x004BFF7C File Offset: 0x004BE17C
		public void RemoveAdventureSite(DataContext context, short areaId, short blockId, bool isTimeout, bool isComplete)
		{
			AdventureSiteData site;
			bool flag = !this._adventureAreas[(int)areaId].AdventureSites.TryGetValue(blockId, out site);
			if (!flag)
			{
				site.OnDestroy();
				bool flag2 = site.IsEnemyNest();
				if (flag2)
				{
					Location location = new Location(areaId, blockId);
					this.ClearRandomEnemiesBySite(context, location);
					this.RemoveElement_EnemyNestSites(location, context);
					bool flag3 = site.SiteState <= 1;
					if (flag3)
					{
						sbyte enemyNestId = AdventureDomain.GetEnemyNestTemplateId(site.TemplateId);
						this.UnregisterEnemyNest((short)enemyNestId);
					}
				}
				this._adventureAreas[(int)areaId].AdventureSites.Remove(blockId);
				this.SetElement_AdventureAreas((int)areaId, this._adventureAreas[(int)areaId], context);
				Events.RaiseAdventureRemoved(context, areaId, blockId, isTimeout, isComplete, site);
			}
		}

		// Token: 0x06007ED3 RID: 32467 RVA: 0x004C0036 File Offset: 0x004BE236
		[Obsolete]
		public void TransferAdventure(DataContext context, Location from, Location to)
		{
		}

		// Token: 0x06007ED4 RID: 32468 RVA: 0x004C003C File Offset: 0x004BE23C
		public void RemoveAllJuniorXiangshuAdventures(DataContext context)
		{
			List<short> toRemoveKeyList = new List<short>();
			short areaId = 0;
			while ((int)areaId < this._adventureAreas.Length)
			{
				AreaAdventureData areaAdventureData = this._adventureAreas[(int)areaId];
				bool flag = areaAdventureData == null || areaAdventureData.AdventureSites.Count <= 0;
				if (!flag)
				{
					toRemoveKeyList.Clear();
					foreach (KeyValuePair<short, AdventureSiteData> pair in areaAdventureData.AdventureSites)
					{
						bool flag2 = pair.Value.TemplateId >= 117 && pair.Value.TemplateId <= 134;
						if (flag2)
						{
							toRemoveKeyList.Add(pair.Key);
						}
					}
					bool flag3 = toRemoveKeyList.Count > 0;
					if (flag3)
					{
						int i = 0;
						int max = toRemoveKeyList.Count;
						while (i < max)
						{
							this.RemoveAdventureSite(context, areaId, toRemoveKeyList[i], false, false);
							i++;
						}
					}
				}
				areaId += 1;
			}
		}

		// Token: 0x06007ED5 RID: 32469 RVA: 0x004C016C File Offset: 0x004BE36C
		public void SetAdventureRemainingMonths(DataContext context, short areaId, short blockId, short remainingMonths)
		{
			this._adventureAreas[(int)areaId].AdventureSites[blockId].RemainingMonths = remainingMonths;
			this.SetElement_AdventureAreas((int)areaId, this._adventureAreas[(int)areaId], context);
		}

		// Token: 0x06007ED6 RID: 32470 RVA: 0x004C019A File Offset: 0x004BE39A
		public void SetAdventureSiteInitData(DataContext context, short areaId, short blockId, int initData)
		{
			this._adventureAreas[(int)areaId].AdventureSites[blockId].SiteInitData = initData;
			this.SetElement_AdventureAreas((int)areaId, this._adventureAreas[(int)areaId], context);
		}

		// Token: 0x06007ED7 RID: 32471 RVA: 0x004C01C8 File Offset: 0x004BE3C8
		public int GetAdventureSiteInitData(short areaId, short blockId)
		{
			return this._adventureAreas[(int)areaId].AdventureSites[blockId].SiteInitData;
		}

		// Token: 0x06007ED8 RID: 32472 RVA: 0x004C01F4 File Offset: 0x004BE3F4
		public AdventureSiteData GetAdventureSite(short areaId, short blockId)
		{
			bool flag = areaId < 0 || (int)areaId >= this._adventureAreas.Length;
			AdventureSiteData result;
			if (flag)
			{
				result = null;
			}
			else
			{
				AreaAdventureData adventureArea = this._adventureAreas[(int)areaId];
				result = adventureArea.AdventureSites.GetValueOrDefault(blockId);
			}
			return result;
		}

		// Token: 0x06007ED9 RID: 32473 RVA: 0x004C0238 File Offset: 0x004BE438
		public Location GetAdventureLocationByTemplateId(short templateId)
		{
			short i = 0;
			while ((int)i < this._adventureAreas.Count<AreaAdventureData>())
			{
				foreach (KeyValuePair<short, AdventureSiteData> pair in this._adventureAreas[(int)i].AdventureSites)
				{
					bool flag = pair.Value.TemplateId == templateId;
					if (flag)
					{
						return new Location(i, pair.Key);
					}
				}
				i += 1;
			}
			return Location.Invalid;
		}

		// Token: 0x06007EDA RID: 32474 RVA: 0x004C02E0 File Offset: 0x004BE4E0
		public sbyte GetAdventureSiteState(short areaId, short blockId)
		{
			AdventureSiteData site;
			return this._adventureAreas[(int)areaId].AdventureSites.TryGetValue(blockId, out site) ? site.SiteState : -1;
		}

		// Token: 0x06007EDB RID: 32475 RVA: 0x004C0314 File Offset: 0x004BE514
		public void UpdateAdventuresInArea(DataContext context, short areaId)
		{
			short[] siteKeys = this._adventureAreas[(int)areaId].AdventureSites.Keys.ToArray<short>();
			foreach (short blockId in siteKeys)
			{
				AdventureSiteData site = this._adventureAreas[(int)areaId].AdventureSites[blockId];
				Location location = new Location(areaId, blockId);
				bool flag = site.SiteState == 1;
				if (flag)
				{
					bool flag2 = site.RemainingMonths > 0;
					if (flag2)
					{
						AdventureSiteData adventureSiteData = site;
						adventureSiteData.RemainingMonths -= 1;
					}
					bool flag3 = site.RemainingMonths == 0;
					if (flag3)
					{
						sbyte xiangshuAvatarId = XiangshuAvatarIds.GetXiangshuAvatarIdBySwordTomb(site.TemplateId);
						bool flag4 = xiangshuAvatarId >= 0;
						if (flag4)
						{
							this.SetAdventureRemainingMonths(context, areaId, blockId, -1);
							sbyte xiangshuLevel = DomainManager.World.GetXiangshuLevel();
							short templateId = XiangshuAvatarIds.GetCurrentLevelXiangshuTemplateId(xiangshuAvatarId, xiangshuLevel, true);
							GameData.Domains.Character.Character character = EventHelper.GetOrCreateFixedCharacterByTemplateId(templateId);
							List<Location> locationList = EventHelper.GetBlockLocationGroup(location, true);
							EventHelper.MoveFixedCharacter(character, locationList.Sample<Location>());
							bool flag5 = !DomainManager.Extra.IsOneShotEventHandled(44) && DomainManager.Taiwu.GetTaiwu().GetConsummateLevel() < 4 && !DomainManager.Extra.IsDreamBack();
							if (flag5)
							{
								DomainManager.World.GetMonthlyEventCollection().AddWardOffXiangshuProtection(DomainManager.Taiwu.GetTaiwuCharId(), (int)site.TemplateId);
							}
						}
						else
						{
							this.RemoveAdventureSite(context, areaId, blockId, true, false);
						}
					}
				}
				else
				{
					bool flag6 = site.SiteState >= 2;
					if (flag6)
					{
						bool flag7 = site.RemainingMonths > 0;
						if (flag7)
						{
							AdventureSiteData adventureSiteData2 = site;
							adventureSiteData2.RemainingMonths -= 1;
						}
						bool flag8 = site.RemainingMonths == 0;
						if (flag8)
						{
							this.RemoveAdventureSite(context, areaId, blockId, true, false);
						}
						else
						{
							EnemyNestSiteExtraData enemyNestData = this._enemyNestSites[location];
							this.OfflineAddConqueredEnemyNestIncome(context, site, enemyNestData);
							this.SetElement_EnemyNestSites(location, enemyNestData, context);
						}
					}
				}
			}
			this.SetElement_AdventureAreas((int)areaId, this._adventureAreas[(int)areaId], context);
		}

		// Token: 0x06007EDC RID: 32476 RVA: 0x004C051C File Offset: 0x004BE71C
		public unsafe void ComplementEnemiesBySite(DataContext context, AdventureSiteData advSite, short areaId, short blockId, bool ignoreCountLimit = false)
		{
			Location location = new Location(areaId, blockId);
			EnemyNestSiteExtraData nestSite;
			bool flag = !this._enemyNestSites.TryGetValue(location, out nestSite);
			if (!flag)
			{
				sbyte enemyNestTemplateId = AdventureDomain.GetEnemyNestTemplateId(advSite.TemplateId);
				List<short> enemyIds = EnemyNest.Instance[(short)enemyNestTemplateId].Members;
				List<short> baseSpawnAmount = EnemyNest.Instance[(short)enemyNestTemplateId].SpawnAmountFactors;
				int factor = DomainManager.World.GetHereticsAmountFactor();
				List<MapBlockData> validBlocks = ObjectPool<List<MapBlockData>>.Instance.Get();
				DomainManager.Adventure.GetValidBlocksForRandomEnemy(areaId, blockId, 3, false, false, false, validBlocks);
				bool flag2 = enemyNestTemplateId == 3;
				if (flag2)
				{
					sbyte stateTemplateId = DomainManager.Map.GetStateTemplateIdByAreaId(areaId);
					sbyte regionalSectId = MapState.Instance[stateTemplateId].SectID;
					int sectIndex = (int)(regionalSectId - 1);
					DomainManager.Adventure.SetAdventureSiteInitData(context, areaId, blockId, sectIndex);
					int needAmount = (int)baseSpawnAmount[sectIndex] * factor / 100;
					int hasAmount = nestSite.RandomEnemies.Count;
					bool flag3 = validBlocks.Count > 0 && needAmount > hasAmount;
					if (flag3)
					{
						DomainManager.Adventure.CreateRandomEnemiesOnValidBlocks(context, location, enemyIds[sectIndex], 1, validBlocks);
					}
				}
				else
				{
					bool flag4 = validBlocks.Count > 0;
					if (flag4)
					{
						int count = enemyIds.Count;
						Span<int> hasAmounts;
						Span<int> indexOrders;
						checked
						{
							Span<int> span = new Span<int>(stackalloc byte[unchecked((UIntPtr)count) * 4], count);
							hasAmounts = span;
							int count2 = enemyIds.Count;
							span = new Span<int>(stackalloc byte[unchecked((UIntPtr)count2) * 4], count2);
							indexOrders = span;
						}
						for (int i = 0; i < enemyIds.Count; i++)
						{
							*hasAmounts[i] = 0;
							*indexOrders[i] = i;
						}
						CollectionUtils.Shuffle<int>(context.Random, indexOrders, enemyIds.Count);
						foreach (MapTemplateEnemyInfo mapRandomEnemyInfo in nestSite.RandomEnemies)
						{
							int index = enemyIds.IndexOf(mapRandomEnemyInfo.TemplateId);
							bool flag5 = index < 0;
							if (!flag5)
							{
								(*hasAmounts[index])++;
							}
						}
						for (int j = 0; j < enemyIds.Count; j++)
						{
							int index2 = *indexOrders[j];
							int needAmount2 = (int)baseSpawnAmount[index2] * factor / 100;
							int hasAmount2 = *hasAmounts[index2];
							bool flag6 = needAmount2 <= hasAmount2;
							if (!flag6)
							{
								DomainManager.Adventure.CreateRandomEnemiesOnValidBlocks(context, location, enemyIds[index2], 1, validBlocks);
								ObjectPool<List<MapBlockData>>.Instance.Return(validBlocks);
								return;
							}
						}
						if (ignoreCountLimit)
						{
							short enemyId = enemyIds.GetRandom(context.Random);
							DomainManager.Adventure.CreateRandomEnemiesOnValidBlocks(context, location, enemyId, 1, validBlocks);
						}
					}
				}
				ObjectPool<List<MapBlockData>>.Instance.Return(validBlocks);
			}
		}

		// Token: 0x06007EDD RID: 32477 RVA: 0x004C0800 File Offset: 0x004BEA00
		public unsafe void ComplementEnemiesInBrokenArea(DataContext context, short areaId)
		{
			bool modified = false;
			int brokenAreaOffset = (int)(areaId - 45);
			int factor = DomainManager.World.GetHereticsAmountFactor();
			int num = GlobalConfig.Instance.BrokenAreaEnemyCountLevelDist.Length;
			Span<int> span = new Span<int>(stackalloc byte[checked(unchecked((UIntPtr)num) * 4)], num);
			Span<int> enemyCounts = span;
			enemyCounts.Fill(0);
			BrokenAreaData brokenAreaData = this._brokenAreaEnemies[brokenAreaOffset];
			bool flag = brokenAreaData.RandomEnemies.Count >= AdventureDomain._brokenAreaEnemyBaseCount * factor / 100;
			if (!flag)
			{
				foreach (MapTemplateEnemyInfo enemyInfo in brokenAreaData.RandomEnemies)
				{
					int baseLevel = (int)(enemyInfo.TemplateId - brokenAreaData.BaseXiangshuMinionTemplateId);
					bool flag2 = baseLevel < 0 || baseLevel >= enemyCounts.Length;
					if (!flag2)
					{
						(*enemyCounts[baseLevel])++;
					}
				}
				Location taiwuLocation = DomainManager.Taiwu.GetTaiwu().GetLocation();
				List<MapBlockData> validBlocks = ObjectPool<List<MapBlockData>>.Instance.Get();
				validBlocks.Clear();
				Span<MapBlockData> areaBlocks = DomainManager.Map.GetAreaBlocks(areaId);
				for (int i = 0; i < areaBlocks.Length; i++)
				{
					MapBlockData block = *areaBlocks[i];
					bool flag3 = taiwuLocation != block.GetLocation();
					if (flag3)
					{
						validBlocks.Add(block);
					}
				}
				for (int baseLevel2 = 0; baseLevel2 < 3; baseLevel2++)
				{
					int complementAmount = (int)GlobalConfig.Instance.BrokenAreaEnemyCountLevelDist[baseLevel2] * factor / 100 - *enemyCounts[baseLevel2];
					bool flag4 = complementAmount <= 0;
					if (!flag4)
					{
						short enemyId = (short)Math.Clamp(baseLevel2 + (int)brokenAreaData.BaseXiangshuMinionTemplateId, 298, 306);
						this.CreateRandomEnemiesOnValidBlocks(context, Location.Invalid, enemyId, 1, validBlocks);
						modified = true;
						break;
					}
				}
				ObjectPool<List<MapBlockData>>.Instance.Return(validBlocks);
				bool flag5 = modified;
				if (flag5)
				{
					this.SetElement_BrokenAreaEnemies(brokenAreaOffset, brokenAreaData, context);
				}
			}
		}

		// Token: 0x06007EDE RID: 32478 RVA: 0x004C0A08 File Offset: 0x004BEC08
		public unsafe void PreAdvanceMonth_UpdateRandomEnemies(DataContext context, int areaId)
		{
			PreAdvanceMonthRandomEnemiesModification modification = new PreAdvanceMonthRandomEnemiesModification
			{
				AreaId = (short)areaId
			};
			List<int> charList = ObjectPool<List<int>>.Instance.Get();
			Span<MapBlockData> areaBlocks = DomainManager.Map.GetAreaBlocks(modification.AreaId);
			CharacterMatcherItem matcher = CharacterMatcher.DefValue.CanBeAttackedByRandomEnemy;
			Span<MapBlockData> span = areaBlocks;
			for (int i = 0; i < span.Length; i++)
			{
				MapBlockData blockData = *span[i];
				bool flag = blockData.TemplateEnemyList == null;
				if (!flag)
				{
					bool flag2 = blockData.CharacterSet == null;
					if (!flag2)
					{
						charList.Clear();
						foreach (int charId in blockData.CharacterSet)
						{
							GameData.Domains.Character.Character character = DomainManager.Character.GetElement_Objects(charId);
							bool flag3 = !matcher.Match(character);
							if (!flag3)
							{
								charList.Add(charId);
							}
						}
						bool flag4 = charList.Count == 0;
						if (!flag4)
						{
							foreach (MapTemplateEnemyInfo enemyInfo in blockData.TemplateEnemyList)
							{
								short templateId = enemyInfo.TemplateId;
								bool flag5 = templateId >= 298 && templateId <= 306 && enemyInfo.SourceAdventureBlockId >= 0;
								if (flag5)
								{
									Location srcLocation = new Location((short)areaId, enemyInfo.SourceAdventureBlockId);
									SectStoryHeavenlyTreeExtendable sectStoryHeavenlyTreeExtendable;
									bool flag6 = DomainManager.Extra.TryGetHeavenlyTreeByLocation(srcLocation, out sectStoryHeavenlyTreeExtendable);
									if (flag6)
									{
										continue;
									}
								}
								int charId2 = charList[context.Random.Next(charList.Count)];
								bool isRighteous = Config.Character.Instance[enemyInfo.TemplateId].OrganizationInfo.OrgTemplateId == 18;
								bool flag7 = isRighteous;
								if (flag7)
								{
									GameData.Domains.Character.Character character2 = DomainManager.Character.GetElement_Objects(charId2);
									sbyte fameType = character2.GetFameType();
									bool flag8 = !FameType.IsNonNegative(fameType, true);
									if (flag8)
									{
										modification.RandomEnemyAttackRecords.Add(new ValueTuple<int, MapTemplateEnemyInfo>(charId2, enemyInfo));
									}
								}
								else
								{
									modification.RandomEnemyAttackRecords.Add(new ValueTuple<int, MapTemplateEnemyInfo>(charId2, enemyInfo));
								}
							}
						}
					}
				}
			}
			Dictionary<short, List<int>> animalAreaData;
			bool flag9 = DomainManager.Extra.TryGetAnimalAreaDataByAreaId((short)areaId, out animalAreaData);
			if (flag9)
			{
				foreach (KeyValuePair<short, List<int>> keyValuePair in animalAreaData)
				{
					short templateId;
					List<int> list;
					keyValuePair.Deconstruct(out templateId, out list);
					short blockId = templateId;
					List<int> animalIds = list;
					MapBlockData blockData2 = *areaBlocks[(int)blockId];
					bool flag10 = blockData2.CharacterSet == null;
					if (!flag10)
					{
						charList.Clear();
						foreach (int charId3 in blockData2.CharacterSet)
						{
							GameData.Domains.Character.Character character3 = DomainManager.Character.GetElement_Objects(charId3);
							bool flag11 = character3.GetAgeGroup() == 0;
							if (!flag11)
							{
								charList.Add(charId3);
							}
						}
						bool flag12 = charList.Count == 0;
						if (!flag12)
						{
							foreach (int animalId in animalIds)
							{
								GameData.Domains.Character.Animal animal;
								bool flag13 = DomainManager.Extra.TryGetAnimal(animalId, out animal) && DomainManager.Extra.IsAnimalAbleToAttack(animal, false);
								if (flag13)
								{
									int targetCharId = charList[context.Random.Next(charList.Count)];
									modification.AnimalAttackRecords.Add(new ValueTuple<int, int>(targetCharId, animalId));
								}
							}
						}
					}
				}
			}
			ObjectPool<List<int>>.Instance.Return(charList);
			ParallelModificationsRecorder recorder = context.ParallelModificationsRecorder;
			recorder.RecordType(ParallelModificationType.PreAdvanceMonthUpdateRandomEnemies);
			recorder.RecordParameterClass<PreAdvanceMonthRandomEnemiesModification>(modification);
		}

		// Token: 0x06007EDF RID: 32479 RVA: 0x004C0E74 File Offset: 0x004BF074
		public void ComplementPreAdvanceMonth_UpdateRandomEnemies(DataContext context, PreAdvanceMonthRandomEnemiesModification mod)
		{
			foreach (ValueTuple<int, MapTemplateEnemyInfo> record in mod.RandomEnemyAttackRecords)
			{
				GameData.Domains.Character.Character character = DomainManager.Character.GetElement_Objects(record.Item1);
				Location location = new Location(mod.AreaId, record.Item2.BlockId);
				AiHelper.NpcCombatResultType result = DomainManager.Character.SimulateEnemyAttack(context, record.Item2.TemplateId, character);
				bool flag = result == AiHelper.NpcCombatResultType.MajorDefeat || result == AiHelper.NpcCombatResultType.MinorDefeat;
				if (flag)
				{
					Events.RaiseTemplateEnemyLocationChanged(context, record.Item2, location, Location.Invalid);
				}
				else
				{
					DomainManager.Character.EscapeToRandomNearbyBlock(context, character, 3);
				}
				character.SetInjuries(character.GetInjuries(), context);
			}
			foreach (ValueTuple<int, int> record2 in mod.AnimalAttackRecords)
			{
				GameData.Domains.Character.Character character2;
				bool flag2 = !DomainManager.Character.TryGetElement_Objects(record2.Item1, out character2);
				if (!flag2)
				{
					Location charLocation = character2.GetLocation();
					List<int> animalIds;
					GameData.Domains.Character.Animal animal;
					bool flag3 = !DomainManager.Extra.TryGetAnimalIdsByLocation(charLocation, out animalIds) || !animalIds.Contains(record2.Item2) || !DomainManager.Extra.TryGetAnimal(record2.Item2, out animal);
					if (!flag3)
					{
						AiHelper.NpcCombatResultType result2 = DomainManager.Character.SimulateEnemyAttack(context, animal.CharacterTemplateId, character2);
						bool flag4 = result2 - AiHelper.NpcCombatResultType.MajorDefeat <= 1;
						bool flag5 = flag4;
						if (flag5)
						{
							DomainManager.Extra.ApplyAnimalDeadByAccident(context, animal.Id);
						}
						else
						{
							DomainManager.Character.EscapeToRandomNearbyBlock(context, character2, 3);
						}
					}
				}
			}
		}

		// Token: 0x06007EE0 RID: 32480 RVA: 0x004C105C File Offset: 0x004BF25C
		private void UpdateNormalAreaRandomEnemiesMovement(DataContext context)
		{
			List<MapBlockData> neighborBlocks = ObjectPool<List<MapBlockData>>.Instance.Get();
			List<MapTemplateEnemyInfo> randomEnemies = ObjectPool<List<MapTemplateEnemyInfo>>.Instance.Get();
			foreach (KeyValuePair<Location, EnemyNestSiteExtraData> nestSite in this._enemyNestSites)
			{
				List<MapTemplateEnemyInfo> randomEnemies2 = nestSite.Value.RandomEnemies;
				bool flag = randomEnemies2 == null || randomEnemies2.Count <= 0;
				if (!flag)
				{
					int maxSteps = EnemyNestConstValues.RighteousStrongholdNestIds.Exist((short)nestSite.Value.InitialNestTemplateId) ? 5 : 3;
					this.GetValidBlocksForRandomEnemy(nestSite.Key.AreaId, nestSite.Key.BlockId, (short)maxSteps, false, false, true, neighborBlocks);
					bool flag2 = neighborBlocks.Count == 0;
					if (!flag2)
					{
						randomEnemies.Clear();
						randomEnemies.AddRange(nestSite.Value.RandomEnemies);
						foreach (MapTemplateEnemyInfo randEnemy in randomEnemies)
						{
							MapBlockData targetBlock = neighborBlocks.GetRandom(context.Random);
							bool flag3 = randEnemy.BlockId == targetBlock.BlockId;
							if (!flag3)
							{
								Location srcLocation = new Location(nestSite.Key.AreaId, randEnemy.BlockId);
								Location destLocation = new Location(nestSite.Key.AreaId, targetBlock.BlockId);
								Events.RaiseTemplateEnemyLocationChanged(context, randEnemy, srcLocation, destLocation);
							}
						}
					}
				}
			}
			ObjectPool<List<MapBlockData>>.Instance.Return(neighborBlocks);
			ObjectPool<List<MapTemplateEnemyInfo>>.Instance.Return(randomEnemies);
		}

		// Token: 0x06007EE1 RID: 32481 RVA: 0x004C1238 File Offset: 0x004BF438
		private void UpdateBrokenAreaRandomEnemiesMovement(DataContext context)
		{
			List<MapBlockData> neighborBlocks = ObjectPool<List<MapBlockData>>.Instance.Get();
			List<MapTemplateEnemyInfo> enemyList = ObjectPool<List<MapTemplateEnemyInfo>>.Instance.Get();
			for (int index = 0; index < this._brokenAreaEnemies.Length; index++)
			{
				bool flag = this._brokenAreaEnemies[index].RandomEnemies == null;
				if (flag)
				{
					return;
				}
				enemyList.Clear();
				enemyList.AddRange(this._brokenAreaEnemies[index].RandomEnemies);
				short areaId = (short)(index + 45);
				foreach (MapTemplateEnemyInfo randEnemy in enemyList)
				{
					this.GetValidBlocksForRandomEnemy(areaId, randEnemy.BlockId, 3, false, false, true, neighborBlocks);
					MapBlockData targetBlock = neighborBlocks.GetRandom(context.Random);
					bool flag2 = randEnemy.BlockId == targetBlock.BlockId;
					if (!flag2)
					{
						Location srcLocation = new Location(areaId, randEnemy.BlockId);
						Location destLocation = new Location(areaId, targetBlock.BlockId);
						Events.RaiseTemplateEnemyLocationChanged(context, randEnemy, srcLocation, destLocation);
					}
				}
			}
			ObjectPool<List<MapBlockData>>.Instance.Return(neighborBlocks);
			ObjectPool<List<MapTemplateEnemyInfo>>.Instance.Return(enemyList);
		}

		// Token: 0x06007EE2 RID: 32482 RVA: 0x004C1378 File Offset: 0x004BF578
		public void CheckRandomEnemyAttackTaiwuOnAdvanceMonth()
		{
			GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
			Location location = taiwu.GetLocation();
			bool flag = !location.IsValid();
			if (!flag)
			{
				MonthlyEventCollection collection = DomainManager.World.GetMonthlyEventCollection();
				sbyte taiwuFame = taiwu.GetFame();
				sbyte fameType = FameType.GetFameType(taiwuFame);
				bool canBeSneakyHarmTarget = DomainManager.Taiwu.CanTaiwuBeSneakyHarmfulActionTarget();
				MapBlockData blockData = DomainManager.Map.GetBlock(location);
				bool flag2 = blockData.TemplateEnemyList != null && blockData.TemplateEnemyList.Count > 0;
				if (flag2)
				{
					MapTemplateEnemyInfo maxLevelHeretic = new MapTemplateEnemyInfo(-1, -1, -1);
					MapTemplateEnemyInfo maxLevelRighteous = new MapTemplateEnemyInfo(-1, -1, -1);
					sbyte maxHereticGrade = -1;
					sbyte maxRighteousGrade = -1;
					foreach (MapTemplateEnemyInfo templateEnemy in blockData.TemplateEnemyList)
					{
						CharacterItem config = Config.Character.Instance[templateEnemy.TemplateId];
						bool flag3 = config.OrganizationInfo.OrgTemplateId == 18;
						if (flag3)
						{
							bool flag4 = canBeSneakyHarmTarget && config.OrganizationInfo.Grade > maxRighteousGrade;
							if (flag4)
							{
								maxLevelRighteous = templateEnemy;
								maxRighteousGrade = config.OrganizationInfo.Grade;
							}
						}
						else
						{
							bool flag5 = config.OrganizationInfo.OrgTemplateId == 17;
							if (flag5)
							{
								bool flag6 = canBeSneakyHarmTarget && config.OrganizationInfo.Grade > maxHereticGrade;
								if (flag6)
								{
									maxLevelHeretic = templateEnemy;
									maxHereticGrade = config.OrganizationInfo.Grade;
								}
							}
							else
							{
								bool flag7 = config.OrganizationInfo.OrgTemplateId == 19;
								if (flag7)
								{
									SectStoryHeavenlyTreeExtendable sectStoryHeavenlyTreeExtendable;
									bool flag8 = templateEnemy.SourceAdventureBlockId >= 0 && DomainManager.Extra.TryGetHeavenlyTreeByLocation(location, out sectStoryHeavenlyTreeExtendable);
									if (!flag8)
									{
										bool flag9 = config.OrganizationInfo.Grade > maxHereticGrade;
										if (flag9)
										{
											maxLevelHeretic = templateEnemy;
											maxHereticGrade = config.OrganizationInfo.Grade;
										}
									}
								}
								else
								{
									bool flag10 = config.OrganizationInfo.Grade > maxHereticGrade;
									if (flag10)
									{
										maxLevelHeretic = templateEnemy;
										maxHereticGrade = config.OrganizationInfo.Grade;
									}
								}
							}
						}
					}
					bool flag11 = FameType.AttackByRighteous(fameType) && maxRighteousGrade >= 0 && !this._escapingRandomEnemies.Contains((short)maxHereticGrade);
					if (flag11)
					{
						collection.AddRandomRighteousAttack(location, maxLevelRighteous.TemplateId);
					}
					bool flag12 = maxHereticGrade >= 0 && !this._escapingRandomEnemies.Contains((short)maxHereticGrade);
					if (flag12)
					{
						collection.AddRandomEnemyAttack(location, maxLevelHeretic.TemplateId);
					}
				}
				bool flag13 = blockData.EnemyCharacterSet != null && blockData.EnemyCharacterSet.Count > 0;
				if (flag13)
				{
					int groupCount = (3 + blockData.EnemyCharacterSet.Count) / 4;
					List<GameData.Domains.Character.Character> enemyChars = ObjectPool<List<GameData.Domains.Character.Character>>.Instance.Get();
					enemyChars.Clear();
					foreach (int charId in blockData.EnemyCharacterSet)
					{
						enemyChars.Add(DomainManager.Character.GetElement_Objects(charId));
					}
					enemyChars.Sort((GameData.Domains.Character.Character charA, GameData.Domains.Character.Character charB) => -charA.GetCombatPower().CompareTo(charB.GetCombatPower()));
					for (int i = 0; i < groupCount; i++)
					{
						GameData.Domains.Character.Character charId2 = enemyChars[i];
					}
				}
			}
		}

		// Token: 0x06007EE3 RID: 32483 RVA: 0x004C1718 File Offset: 0x004BF918
		public void UpdateAdventuresInAllAreas(DataContext context)
		{
			Stopwatch sw = new Stopwatch();
			sw.Start();
			for (short areaId = 0; areaId < 45; areaId += 1)
			{
				this.UpdateAdventuresInArea(context, areaId);
			}
			for (short areaId2 = 45; areaId2 < 135; areaId2 += 1)
			{
				this.UpdateAdventuresInArea(context, areaId2);
				this.ComplementEnemiesInBrokenArea(context, areaId2);
			}
			this.UpdateAdventuresInArea(context, 135);
			this.UpdateAdventuresInArea(context, 136);
			this.UpdateAdventuresInArea(context, 137);
			this.UpdateAdventuresInArea(context, 138);
			this.UpdateNormalAreaRandomEnemiesMovement(context);
			this.UpdateBrokenAreaRandomEnemiesMovement(context);
			sw.Stop();
			Logger logger = AdventureDomain.Logger;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(45, 1);
			defaultInterpolatedStringHandler.AppendLiteral("PostAdvanceMonth.UpdateAdventuresInAllAreas: ");
			defaultInterpolatedStringHandler.AppendFormatted<double>(sw.Elapsed.TotalMilliseconds, "N1");
			logger.Info(defaultInterpolatedStringHandler.ToStringAndClear());
		}

		// Token: 0x06007EE4 RID: 32484 RVA: 0x004C180C File Offset: 0x004BFA0C
		public void OnRandomEnemyLocationChange(DataContext context, MapTemplateEnemyInfo mapTemplateEnemyInfo, Location srcLocation, Location destLocation)
		{
			bool flag = srcLocation.IsValid();
			if (flag)
			{
				Location nestLocation = new Location(srcLocation.AreaId, mapTemplateEnemyInfo.SourceAdventureBlockId);
				bool flag2 = nestLocation.IsValid();
				if (flag2)
				{
					EnemyNestSiteExtraData nestSite;
					bool flag3 = this._enemyNestSites.TryGetValue(nestLocation, out nestSite) && nestSite.RandomEnemies.Remove(mapTemplateEnemyInfo);
					if (flag3)
					{
						this.SetElement_EnemyNestSites(nestLocation, nestSite, context);
					}
				}
				else
				{
					bool flag4 = srcLocation.AreaId >= 45 && srcLocation.AreaId < 135;
					if (flag4)
					{
						int brokenAreaIdx = (int)(srcLocation.AreaId - 45);
						BrokenAreaData brokenAreaData = this._brokenAreaEnemies[brokenAreaIdx];
						bool flag5 = brokenAreaData.RandomEnemies.Remove(mapTemplateEnemyInfo);
						if (flag5)
						{
							this.SetElement_BrokenAreaEnemies(brokenAreaIdx, brokenAreaData, context);
						}
					}
				}
			}
			bool flag6 = destLocation.IsValid();
			if (flag6)
			{
				mapTemplateEnemyInfo.BlockId = destLocation.BlockId;
				Location nestLocation2 = new Location(destLocation.AreaId, mapTemplateEnemyInfo.SourceAdventureBlockId);
				bool flag7 = nestLocation2.IsValid();
				if (flag7)
				{
					EnemyNestSiteExtraData nestSite2;
					bool flag8 = this._enemyNestSites.TryGetValue(nestLocation2, out nestSite2);
					if (flag8)
					{
						nestSite2.RandomEnemies.Add(mapTemplateEnemyInfo);
						this.SetElement_EnemyNestSites(nestLocation2, nestSite2, context);
					}
				}
				else
				{
					bool flag9 = destLocation.AreaId >= 45 && destLocation.AreaId < 135;
					if (flag9)
					{
						int brokenAreaIdx2 = (int)(destLocation.AreaId - 45);
						BrokenAreaData brokenAreaData2 = this._brokenAreaEnemies[brokenAreaIdx2];
						brokenAreaData2.RandomEnemies.Add(mapTemplateEnemyInfo);
						this.SetElement_BrokenAreaEnemies(brokenAreaIdx2, brokenAreaData2, context);
					}
				}
			}
		}

		// Token: 0x06007EE5 RID: 32485 RVA: 0x004C19A0 File Offset: 0x004BFBA0
		public unsafe short GenerateDisasterAdventureId(IRandomSource random, MapBlockData mapBlockData)
		{
			bool flag = !this._adventureAreas[(int)mapBlockData.AreaId].AdventureSites.ContainsKey(mapBlockData.BlockId) && random.Next(0, 100) < (int)GlobalConfig.Instance.DisasterAdventureSpawnChance;
			if (flag)
			{
				List<short> availableAdventureIds = new List<short>();
				for (int resourceType = 0; resourceType < 6; resourceType++)
				{
					List<short> advType = this.GetDisasterAdventureTypesByResourceType(resourceType);
					bool flag2 = advType.Count > 0 && *(ref mapBlockData.CurrResources.Items.FixedElementField + (IntPtr)resourceType * 2) >= 100;
					if (flag2)
					{
						short adventureId = advType.GetRandom(random);
						availableAdventureIds.Add(adventureId);
					}
				}
				bool flag3 = availableAdventureIds.Count > 0;
				if (flag3)
				{
					return availableAdventureIds.GetRandom(random);
				}
			}
			return -1;
		}

		// Token: 0x06007EE6 RID: 32486 RVA: 0x004C1A7C File Offset: 0x004BFC7C
		public List<short> GetDisasterAdventureTypesByResourceType(int resourceType)
		{
			return AdventureDomain.AdventureTypes[9 + resourceType];
		}

		// Token: 0x06007EE7 RID: 32487 RVA: 0x004C1A98 File Offset: 0x004BFC98
		public static sbyte GetEnemyNestTemplateId(short adventureId)
		{
			foreach (EnemyNestItem enemyNest in ((IEnumerable<EnemyNestItem>)EnemyNest.Instance))
			{
				bool flag = enemyNest.AdventureId == adventureId;
				if (flag)
				{
					return (sbyte)enemyNest.TemplateId;
				}
			}
			Logger logger = AdventureDomain.Logger;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(42, 2);
			defaultInterpolatedStringHandler.AppendLiteral("adventureId ");
			defaultInterpolatedStringHandler.AppendFormatted(Adventure.Instance[adventureId].Name);
			defaultInterpolatedStringHandler.AppendFormatted<short>(adventureId);
			defaultInterpolatedStringHandler.AppendLiteral(" is not an heretic stronghold.");
			logger.Warn(defaultInterpolatedStringHandler.ToStringAndClear());
			return 0;
		}

		// Token: 0x06007EE8 RID: 32488 RVA: 0x004C1B54 File Offset: 0x004BFD54
		public static bool CheckPassPopulationRestriction(IRandomSource randomSource)
		{
			float prob = DomainManager.World.GetProbAdjustOfCreatingCharacter();
			return randomSource.NextFloat() < prob;
		}

		// Token: 0x06007EE9 RID: 32489 RVA: 0x004C1B7C File Offset: 0x004BFD7C
		public void TryCreateElopeWithLove(DataContext context)
		{
			EventArgBox globalEventArgBox = DomainManager.TaiwuEvent.GetGlobalEventArgumentBox();
			bool canCreate = false;
			bool flag = globalEventArgBox.Contains<int>("AwayForeverTime");
			if (flag)
			{
				int currDate = DomainManager.World.GetCurrDate();
				AdaptableLog.Info("远走高飞还剩" + (currDate - globalEventArgBox.GetInt("AwayForeverTime")).ToString());
				bool flag2 = currDate >= globalEventArgBox.GetInt("AwayForeverTime");
				if (flag2)
				{
					AdaptableLog.Info("远走高飞开始检测" + currDate.ToString());
					bool flag3 = globalEventArgBox.Contains<int>("ForeverLoverId");
					if (flag3)
					{
						AdaptableLog.Info("远走高飞有人" + currDate.ToString());
						int charId = globalEventArgBox.GetInt("ForeverLoverId");
						AdaptableLog.Info("远走高飞有数据" + globalEventArgBox.GetInt("AwayForeverTime").ToString());
						GameData.Domains.Character.Character character;
						DomainManager.Character.TryGetElement_Objects(charId, out character);
						bool flag4 = character == null;
						if (flag4)
						{
							return;
						}
						bool flag5 = character.IsActiveExternalRelationState(60);
						if (flag5)
						{
							return;
						}
						sbyte orgTemplateId = character.GetOrganizationInfo().OrgTemplateId;
						bool flag6 = orgTemplateId == 1 || orgTemplateId == 8 || orgTemplateId - 11 <= 1;
						bool flag7 = flag6;
						if (flag7)
						{
							AdaptableLog.Info("远走高飞可生成");
							bool flag8 = !globalEventArgBox.Contains<int>("StoryForeverLoverId");
							if (flag8)
							{
								canCreate = true;
							}
						}
					}
				}
			}
			bool flag9 = canCreate;
			if (flag9)
			{
				int charId2 = globalEventArgBox.GetInt("ForeverLoverId");
				GameData.Domains.Character.Character character2 = DomainManager.Character.GetElement_Objects(charId2);
				bool flag10 = character2 == null;
				if (!flag10)
				{
					OrganizationInfo orgInfo = character2.GetOrganizationInfo();
					short settlementId = orgInfo.SettlementId;
					Location location = DomainManager.Organization.GetSettlement(settlementId).GetLocation();
					MapBlockData mapBlockData = DomainManager.Map.GetBlock(location);
					short blockId = mapBlockData.BlockId;
					bool flag11 = mapBlockData.RootBlockId >= 0;
					if (flag11)
					{
						blockId = mapBlockData.RootBlockId;
					}
					List<short> blockIdList = new List<short>();
					DomainManager.Map.GetSettlementBlocks(location.AreaId, blockId, blockIdList);
					for (int i = 0; i < blockIdList.Count; i++)
					{
						MapBlockData tempLocation = DomainManager.Map.GetBlock(location.AreaId, blockIdList[i]);
						bool flag12 = !DomainManager.Adventure.GetAdventuresInArea(location.AreaId).AdventureSites.ContainsKey(location.BlockId);
						if (flag12)
						{
							blockId = tempLocation.BlockId;
							break;
						}
					}
					MapBlockData tryMapBlockData = DomainManager.Map.GetBlock(location.AreaId, blockId);
					bool flag13 = tryMapBlockData == null;
					if (!flag13)
					{
						Location tryLocation = tryMapBlockData.GetLocation();
						bool flag14 = !DomainManager.Adventure.GetAdventuresInArea(tryLocation.AreaId).AdventureSites.ContainsKey(tryLocation.BlockId) && character2.GetKidnapperId() < 0;
						if (flag14)
						{
							bool createAdventureResult = DomainManager.Adventure.TryCreateAdventureSite(context, tryLocation.AreaId, tryLocation.BlockId, 28, MonthlyActionKey.Invalid);
							bool flag15 = createAdventureResult;
							if (flag15)
							{
								tryMapBlockData.SetVisible(true, context);
							}
							Location storyLocation = new Location(location.AreaId, blockId);
							bool condition = character2.GetCreatingType() == 1;
							DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(65, 1);
							defaultInterpolatedStringHandler.AppendLiteral("character-");
							defaultInterpolatedStringHandler.AppendFormatted<int>(character2.GetId());
							defaultInterpolatedStringHandler.AppendLiteral(".GetCreatingType() == CreatingType.IntelligentCharacter");
							Tester.Assert(condition, defaultInterpolatedStringHandler.ToStringAndClear());
							DomainManager.Character.GroupMove(context, character2, storyLocation);
							DomainManager.Character.HideCharacterOnMap(context, character2, 16, true);
							AdaptableLog.Info("远走高飞已生成");
							bool flag16 = globalEventArgBox.Contains<int>("AwayForeverTime");
							if (flag16)
							{
								globalEventArgBox.Remove<int>("AwayForeverTime");
							}
							bool flag17 = globalEventArgBox.Contains<int>("ForeverLoverId");
							if (flag17)
							{
								globalEventArgBox.Remove<int>("ForeverLoverId");
							}
							DomainManager.TaiwuEvent.SaveArgToGlobalArgBox<int>("StoryForeverLoverId", charId2);
						}
					}
				}
			}
		}

		// Token: 0x06007EEA RID: 32490 RVA: 0x004C1F90 File Offset: 0x004C0190
		[DomainMethod]
		public void SetCharacterToAdvanceBranch(DataContext context, int charId)
		{
			this.SetCurBranchChosenChar(charId, context);
			bool flag = this._curBranchChosenChar >= 0 && !this._spentCharList.Contains(this._curBranchChosenChar);
			if (flag)
			{
				int index = DomainManager.Taiwu.GetCharCombatGroupIndex(charId);
				bool flag2 = index > -1;
				if (flag2)
				{
					this._spentCharInCombatGroupDict.Add(index, charId);
				}
				this._spentCharList.Add(this._curBranchChosenChar);
				this.SetSpentCharList(this._spentCharList, context);
				DomainManager.Taiwu.LeaveGroup(context, this._curBranchChosenChar, false, true, true);
				foreach (AdvMapNodeNormal node in this._curBranch.AdvancedBranch.Nodes)
				{
					this._teamDetectedNodes.Add(node.AdjustedPos.GetHashCode());
				}
				this.SetTeamDetectedNodes(this._teamDetectedNodes, context);
			}
			this.RefreshIndicatePath(context);
		}

		// Token: 0x06007EEB RID: 32491 RVA: 0x004C20AC File Offset: 0x004C02AC
		[DomainMethod]
		public bool CanSetCharacterToAdvanceBranch()
		{
			AdventureDomain.<>c__DisplayClass139_0 CS$<>8__locals1 = new AdventureDomain.<>c__DisplayClass139_0();
			CS$<>8__locals1.<>4__this = this;
			bool flag = this._spentCharList.Contains(this._curBranchChosenChar) || this._curBranch.AdvancedBranch == null;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				CS$<>8__locals1.offsets = this._arrangedNodes.Select(delegate(int a)
				{
					AdvMapPos advMapPos = new AdvMapPos(a);
					bool flag3 = CS$<>8__locals1.<>4__this._curBranch.GetNode(new AdvMapPos(a)) != null;
					AdvMapPos offset;
					if (flag3)
					{
						offset = CS$<>8__locals1.<>4__this._curBranch.GetOffset(advMapPos);
					}
					else
					{
						offset = CS$<>8__locals1.<>4__this._curBranch.AdvancedBranch.GetOffset(advMapPos);
					}
					return offset;
				}).ToList<AdvMapPos>();
				AdvMapPos enterPos = this._curBranch.AdvancedBranch.EnterNode.Offset;
				AdvMapPos firstNodePos = this._curBranch.AdvancedBranch.FirstNode.Offset;
				AdvMapPos exitPos = this._curBranch.AdvancedBranch.ExitNode.Offset;
				AdvMapPos lastNodePos = this._curBranch.AdvancedBranch.LastNode.Offset;
				CS$<>8__locals1.curAdvanceBranchEnterPos = enterPos + (firstNodePos - enterPos) / 2;
				CS$<>8__locals1.curAdvanceBranchExitPos = lastNodePos + (exitPos - lastNodePos) / 2;
				bool flag2 = CS$<>8__locals1.offsets.Exists((AdvMapPos offset) => offset.X >= CS$<>8__locals1.curAdvanceBranchEnterPos.X && offset.X <= CS$<>8__locals1.curAdvanceBranchExitPos.X);
				if (flag2)
				{
					result = false;
				}
				else
				{
					CS$<>8__locals1.offsets.Add(CS$<>8__locals1.curAdvanceBranchEnterPos);
					CS$<>8__locals1.offsets.Add(CS$<>8__locals1.curAdvanceBranchExitPos);
					result = (CS$<>8__locals1.<CanSetCharacterToAdvanceBranch>g__IsReachable|1(this._curBranch.AdvancedBranch.EnterNode) && CS$<>8__locals1.<CanSetCharacterToAdvanceBranch>g__IsReachable|1(this._curBranch.AdvancedBranch.ExitNode));
				}
			}
			return result;
		}

		// Token: 0x06007EEC RID: 32492 RVA: 0x004C2225 File Offset: 0x004C0425
		[DomainMethod]
		public List<int> GetAdventureSpentCharList()
		{
			return this.GetSpentCharList();
		}

		// Token: 0x06007EED RID: 32493 RVA: 0x004C2230 File Offset: 0x004C0430
		public void RestoreSpentCharacters(DataContext context)
		{
			Logger logger = AdventureDomain.Logger;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(48, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Restoring spent teammates with a total count of ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(this._spentCharList.Count);
			logger.Info(defaultInterpolatedStringHandler.ToStringAndClear());
			foreach (int charId in this._spentCharList)
			{
				DomainManager.Taiwu.JoinGroup(context, charId, true);
			}
			this._spentCharList.Clear();
			this.SetSpentCharList(this._spentCharList, context);
			foreach (KeyValuePair<int, int> keyValuePair in this._spentCharInCombatGroupDict)
			{
				int num;
				int num2;
				keyValuePair.Deconstruct(out num, out num2);
				int index = num;
				int charId2 = num2;
				DomainManager.Taiwu.SetElement_CombatGroupCharIds(index, charId2, context);
			}
			this._spentCharInCombatGroupDict.Clear();
		}

		// Token: 0x06007EEE RID: 32494 RVA: 0x004C2354 File Offset: 0x004C0554
		public void AddTemporaryIntelligentCharacter(int charId)
		{
			this._temporaryIntelligentCharacters.Add(charId);
		}

		// Token: 0x06007EEF RID: 32495 RVA: 0x004C2364 File Offset: 0x004C0564
		public void KeepTemporaryCharacterAfterAdventure(int charId)
		{
			this._temporaryIntelligentCharacters.Remove(charId);
		}

		// Token: 0x06007EF0 RID: 32496 RVA: 0x004C2374 File Offset: 0x004C0574
		public void RemoveAllTemporaryIntelligentCharacters(DataContext context)
		{
			Logger logger = AdventureDomain.Logger;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(68, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Removing all temporary intelligent characters with a total count of ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(this._temporaryIntelligentCharacters.Count);
			logger.Info(defaultInterpolatedStringHandler.ToStringAndClear());
			foreach (int tempCharId in this._temporaryIntelligentCharacters)
			{
				GameData.Domains.Character.Character character;
				bool flag = !DomainManager.Character.TryGetElement_Objects(tempCharId, out character);
				if (!flag)
				{
					DomainManager.Character.RevertAllTemporaryModifications(context, character);
					DomainManager.Character.RemoveTemporaryIntelligentCharacter(context, character);
				}
			}
			this._temporaryIntelligentCharacters.Clear();
		}

		// Token: 0x06007EF1 RID: 32497 RVA: 0x004C2440 File Offset: 0x004C0640
		public void KillIntelligentCharacterInAdventure(DataContext context, GameData.Domains.Character.Character character)
		{
			int charId = character.GetId();
			short adventureId = DomainManager.Adventure.GetCurAdventureId();
			DomainManager.Character.RevertAllTemporaryModifications(context, character);
			bool flag = !this._temporaryIntelligentCharacters.Contains(charId);
			if (flag)
			{
				DomainManager.Character.MakeCharacterDead(context, character, (Adventure.Instance[adventureId].Type == 4) ? 5 : 0, new CharacterDeathInfo(character.GetValidLocation())
				{
					KillerId = -1,
					AdventureId = adventureId
				});
			}
			else
			{
				DomainManager.Character.RemoveTemporaryIntelligentCharacter(context, character);
			}
			this._temporaryIntelligentCharacters.Remove(charId);
		}

		// Token: 0x06007EF2 RID: 32498 RVA: 0x004C24DE File Offset: 0x004C06DE
		public void AddTemporaryEnemyId(int charId)
		{
			this._temporaryEnemies.Add(charId);
		}

		// Token: 0x06007EF3 RID: 32499 RVA: 0x004C24EE File Offset: 0x004C06EE
		public void RemoveTemporaryEnemyId(int charId)
		{
			this._temporaryEnemies.Remove(charId);
		}

		// Token: 0x06007EF4 RID: 32500 RVA: 0x004C2500 File Offset: 0x004C0700
		public void RemoveAllTemporaryEnemies(DataContext context)
		{
			Logger logger = AdventureDomain.Logger;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(53, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Removing all temporary enemies with a total count of ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(this._temporaryEnemies.Count);
			logger.Info(defaultInterpolatedStringHandler.ToStringAndClear());
			foreach (int enemyId in this._temporaryEnemies)
			{
				GameData.Domains.Character.Character enemy;
				bool flag = !DomainManager.Character.TryGetElement_Objects(enemyId, out enemy);
				if (!flag)
				{
					DomainManager.Character.RevertAllTemporaryModifications(context, enemy);
					bool flag2 = enemy.GetCreatingType() == 1;
					if (!flag2)
					{
						DomainManager.Character.RemoveNonIntelligentCharacter(context, enemy);
					}
				}
			}
			this._temporaryEnemies.Clear();
		}

		// Token: 0x06007EF5 RID: 32501 RVA: 0x004C25DC File Offset: 0x004C07DC
		public int GetEnemyNestCount(short enemyNestTemplateId)
		{
			return this._enemyNestCounts[(int)enemyNestTemplateId];
		}

		// Token: 0x06007EF6 RID: 32502 RVA: 0x004C25F8 File Offset: 0x004C07F8
		private void InitializeEnemyNestCounts()
		{
			this._enemyNestCounts = new int[EnemyNest.Instance.Count];
			foreach (AreaAdventureData areaData in this._adventureAreas)
			{
				foreach (AdventureSiteData site in areaData.AdventureSites.Values)
				{
					bool flag = !site.IsEnemyNest();
					if (!flag)
					{
						sbyte enemyNestTemplateId = AdventureDomain.GetEnemyNestTemplateId(site.TemplateId);
						this._enemyNestCounts[(int)enemyNestTemplateId]++;
					}
				}
			}
		}

		// Token: 0x06007EF7 RID: 32503 RVA: 0x004C26B4 File Offset: 0x004C08B4
		private void RegisterEnemyNest(short enemyNestTemplateId)
		{
			this._enemyNestCounts[(int)enemyNestTemplateId]++;
		}

		// Token: 0x06007EF8 RID: 32504 RVA: 0x004C26C8 File Offset: 0x004C08C8
		private void UnregisterEnemyNest(short enemyNestTemplateId)
		{
			this._enemyNestCounts[(int)enemyNestTemplateId]--;
		}

		// Token: 0x06007EF9 RID: 32505 RVA: 0x004C26DC File Offset: 0x004C08DC
		public unsafe void DestroyEnemyNest(DataContext context, short areaId, short enemyNestId, sbyte behaviorType)
		{
			InstantNotificationCollection notificationCollection = DomainManager.World.GetInstantNotificationCollection();
			GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
			int taiwuCharId = taiwu.GetId();
			EnemyNestItem enemyNestCfg = EnemyNest.Instance[enemyNestId];
			ConfigData<CharacterItem, short> instance = Config.Character.Instance;
			List<short> members = enemyNestCfg.Members;
			CharacterItem enemyLeaderCfg = instance[members[members.Count - 1]];
			AdventureItem adventureCfg = Adventure.Instance[enemyNestCfg.AdventureId];
			ProfessionFormulaItem formula = ProfessionFormula.Instance[26];
			int addSeniority = formula.Calculate((int)adventureCfg.CombatDifficulty);
			DomainManager.Extra.ChangeProfessionSeniority(context, 3, addSeniority, true, false);
			DomainManager.Taiwu.AddLegacyPoint(context, 13, 100);
			DomainManager.Extra.ChangeAreaSpiritualDebt(context, areaId, (int)enemyNestCfg.SpiritualDebtChange, true, true);
			taiwu.ChangeResource(context, 6, enemyNestCfg.MoneyReward);
			taiwu.ChangeResource(context, 7, enemyNestCfg.AuthorityReward);
			taiwu.ChangeExp(context, enemyNestCfg.ExpReward);
			this._resultDisplayData.ChangedSpiritualDebt = (int)enemyNestCfg.SpiritualDebtChange;
			this._resultDisplayData.ChangedResources.Add(6, enemyNestCfg.MoneyReward);
			this._resultDisplayData.ChangedResources.Add(7, enemyNestCfg.AuthorityReward);
			this._resultDisplayData.ChangedExp += enemyNestCfg.ExpReward;
			foreach (short randomEnemy in enemyNestCfg.Members)
			{
				bool flag = !this._escapingRandomEnemies.Contains(randomEnemy);
				if (flag)
				{
					this._escapingRandomEnemies.Add(randomEnemy);
				}
			}
			this.SetEscapingRandomEnemies(this._escapingRandomEnemies, context);
			switch (behaviorType)
			{
			case 0:
				taiwu.RecordFameAction(context, 41, -1, 5, false);
				DomainManager.Map.ChangeSettlementSafetyInArea(context, areaId, 1);
				notificationCollection.AddFameIncreased(taiwuCharId);
				break;
			case 1:
				taiwu.RecordFameAction(context, 38, -1, 5, false);
				DomainManager.Map.ChangeSettlementCultureInArea(context, areaId, 1);
				notificationCollection.AddFameIncreased(taiwuCharId);
				break;
			case 2:
				DomainManager.Map.ChangeSettlementSafetyInArea(context, areaId, 1);
				DomainManager.Map.ChangeSettlementCultureInArea(context, areaId, 1);
				break;
			case 3:
			{
				taiwu.RecordFameAction(context, 42, -1, 5, false);
				taiwu.ChangeHappiness(context, 3);
				int moneyGain = *(ref enemyLeaderCfg.Resources.Items.FixedElementField + (IntPtr)6 * 4);
				moneyGain = context.Random.Next(moneyGain * 5, moneyGain * 10 + 1);
				bool flag2 = moneyGain > 0;
				if (flag2)
				{
					taiwu.ChangeResource(context, 6, moneyGain);
					notificationCollection.AddResourceIncreased(taiwuCharId, 6, moneyGain);
				}
				notificationCollection.AddFameDecreased(taiwuCharId);
				notificationCollection.AddHappinessIncreased(taiwuCharId);
				break;
			}
			case 4:
			{
				taiwu.RecordFameAction(context, 44, -1, 5, false);
				notificationCollection.AddFameDecreased(taiwuCharId);
				sbyte stateTemplateId = DomainManager.Map.GetStateTemplateIdByAreaId(areaId);
				sbyte orgTemplateId = MapState.Instance[stateTemplateId].SectID;
				sbyte gender = Gender.GetRandom(context.Random);
				short charTemplateId = OrganizationDomain.GetCharacterTemplateId(orgTemplateId, stateTemplateId, gender);
				OrganizationInfo orgInfo = taiwu.GetOrganizationInfo();
				orgInfo.Grade = 0;
				IntelligentCharacterCreationInfo info = new IntelligentCharacterCreationInfo(taiwu.GetLocation(), orgInfo, charTemplateId);
				info.BaseAttraction = (short)context.Random.Next((int)(enemyLeaderCfg.BaseAttraction / 2), (int)(enemyLeaderCfg.BaseAttraction + 1));
				info.Age = (short)context.Random.Next(16, 25);
				GameData.Domains.Character.Character newCharacter = DomainManager.Character.CreateIntelligentCharacter(context, ref info);
				int newCharId = newCharacter.GetId();
				DomainManager.Character.CompleteCreatingCharacter(newCharId);
				newCharacter.AddFeature(context, 198, false);
				DomainManager.Character.ChangeFavorabilityOptional(context, newCharacter, taiwu, -10000, 0);
				DomainManager.Taiwu.JoinGroup(context, newCharId, true);
				GameDataBridge.AddDisplayEvent<List<int>, bool>(DisplayEventType.OpenGetItem_Character, new List<int>
				{
					newCharId
				}, false);
				DomainManager.Adventure.AddCharacterToResultDisplay(newCharacter.GetId());
				break;
			}
			}
		}

		// Token: 0x06007EFA RID: 32506 RVA: 0x004C2ADC File Offset: 0x004C0CDC
		[Obsolete]
		private void IncreaseSafety(DataContext context, MapAreaData areaData)
		{
			foreach (SettlementInfo settlementInfo in areaData.SettlementInfos)
			{
				bool flag = settlementInfo.SettlementId < 0;
				if (!flag)
				{
					Settlement settlement = DomainManager.Organization.GetSettlement(settlementInfo.SettlementId);
					short safety = (short)Math.Min((int)(settlement.GetSafety() + 1), (int)settlement.GetMaxSafety());
					settlement.SetSafety(safety, context);
				}
			}
		}

		// Token: 0x06007EFB RID: 32507 RVA: 0x004C2B4C File Offset: 0x004C0D4C
		[Obsolete]
		private void IncreaseCulture(DataContext context, MapAreaData areaData)
		{
			foreach (SettlementInfo settlementInfo in areaData.SettlementInfos)
			{
				bool flag = settlementInfo.SettlementId < 0;
				if (!flag)
				{
					Settlement settlement = DomainManager.Organization.GetSettlement(settlementInfo.SettlementId);
					short culture = (short)Math.Min((int)(settlement.GetCulture() + 1), (int)settlement.GetMaxCulture());
					settlement.SetCulture(culture, context);
				}
			}
		}

		// Token: 0x06007EFC RID: 32508 RVA: 0x004C2BBC File Offset: 0x004C0DBC
		public void ConquerEnemyNest(DataContext context, short areaId, short blockId)
		{
			AdventureSiteData siteData = this._adventureAreas[(int)areaId].AdventureSites[blockId];
			sbyte enemyNestTemplateId = AdventureDomain.GetEnemyNestTemplateId(siteData.TemplateId);
			EnemyNestItem enemyNestCfg = EnemyNest.Instance[(short)enemyNestTemplateId];
			siteData.SiteState = 2;
			siteData.RemainingMonths = enemyNestCfg.ConqueredDuration;
			this.SetElement_AdventureAreas((int)areaId, this._adventureAreas[(int)areaId], context);
			AdventureItem adventureCfg = Adventure.Instance[enemyNestCfg.AdventureId];
			ProfessionFormulaItem formula = ProfessionFormula.Instance[26];
			int addSeniority = formula.Calculate((int)adventureCfg.CombatDifficulty);
			DomainManager.Extra.ChangeProfessionSeniority(context, 3, addSeniority, true, false);
			MonthlyActionKey enemyNestKey = new MonthlyActionKey(1, 0);
			EnemyNestMonthlyAction enemyNestAction = (EnemyNestMonthlyAction)DomainManager.TaiwuEvent.GetMonthlyAction(enemyNestKey);
			ConfigMonthlyAction action = enemyNestAction.GetConfigAction(areaId, blockId);
			bool flag = action != null;
			if (flag)
			{
				action.ClearCalledCharacters();
			}
			DomainManager.Taiwu.AddLegacyPoint(context, 13, 100);
			bool flag2 = enemyNestCfg.SpiritualDebtChange < 0;
			if (flag2)
			{
				DomainManager.Extra.ChangeAreaSpiritualDebt(context, areaId, (int)(-(int)enemyNestCfg.SpiritualDebtChange), true, true);
				this._resultDisplayData.ChangedSpiritualDebt = (int)(-(int)enemyNestCfg.SpiritualDebtChange);
			}
			this.UnregisterEnemyNest((short)enemyNestTemplateId);
		}

		// Token: 0x06007EFD RID: 32509 RVA: 0x004C2CE4 File Offset: 0x004C0EE4
		private unsafe void OfflineAddConqueredEnemyNestIncome(DataContext context, AdventureSiteData siteData, EnemyNestSiteExtraData enemyNestData)
		{
			IRandomSource random = context.Random;
			switch (siteData.TemplateId)
			{
			case 29:
			{
				bool flag = random.CheckPercentProb(50);
				if (!flag)
				{
					siteData.SiteState = 3;
					IntPtr intPtr = stackalloc byte[(UIntPtr)9];
					cpblk(intPtr, ref <PrivateImplementationDetails>.D0549B91FCC10FEC2E4FEC58D1E2B3FF4D55C6E98D0AA8F684B7C571C89A0D5A, 9);
					sbyte* weights = intPtr;
					sbyte grade2 = (sbyte)CollectionUtils.GetRandomWeightedElement(context.Random, weights, 9, 100);
					enemyNestData.Tribute = new ValueTuple<sbyte, short, int>(grade2, -1, -1);
				}
				break;
			}
			case 30:
				siteData.SiteState = 3;
				enemyNestData.Tribute = new ValueTuple<sbyte, short, int>(6, -1, random.Next(500, 1001));
				break;
			case 31:
			{
				bool flag2 = random.CheckPercentProb(50);
				if (!flag2)
				{
					siteData.SiteState = 3;
					sbyte itemType = (sbyte)random.Next(0, 2);
					IntPtr intPtr2 = stackalloc byte[(UIntPtr)9];
					cpblk(intPtr2, ref <PrivateImplementationDetails>.7B9422041A536D791EA5D6D44F323B6E503113D0AC02C1E3803AABA8D4FE1FF7, 9);
					sbyte* weights2 = intPtr2;
					int grade = CollectionUtils.GetRandomWeightedElement(random, weights2, 9, 100);
					if (!true)
					{
					}
					short templateId2;
					if (itemType != 0)
					{
						if (itemType != 1)
						{
							throw new ArgumentOutOfRangeException();
						}
						templateId2 = (from item in Config.Armor.Instance
						where (int)item.Grade == grade && item.DropRate > 0
						select item).ToList<ArmorItem>().GetRandom(random).TemplateId;
					}
					else
					{
						templateId2 = (from item in Config.Weapon.Instance
						where (int)item.Grade == grade && item.DropRate > 0
						select item).ToList<WeaponItem>().GetRandom(random).TemplateId;
					}
					if (!true)
					{
					}
					short templateId = templateId2;
					enemyNestData.Tribute = new ValueTuple<sbyte, short, int>(itemType, templateId, 1);
				}
				break;
			}
			case 32:
				siteData.SiteState = 3;
				enemyNestData.Tribute = new ValueTuple<sbyte, short, int>(8, -1, random.Next(500, 1001));
				break;
			case 34:
				siteData.SiteState = 3;
				enemyNestData.Tribute = new ValueTuple<sbyte, short, int>(7, -1, random.Next(250, 501));
				break;
			case 37:
				siteData.SiteState = 3;
				enemyNestData.Tribute = new ValueTuple<sbyte, short, int>((sbyte)random.Next(6), -1, random.Next(500, 1001));
				break;
			}
		}

		// Token: 0x06007EFE RID: 32510 RVA: 0x004C2F1C File Offset: 0x004C111C
		public void CollectTribute(DataContext context, int charId, short areaId, short blockId)
		{
			AreaAdventureData advArea = this._adventureAreas[(int)areaId];
			AdventureSiteData siteData = advArea.AdventureSites[blockId];
			bool flag = siteData.SiteState != 3;
			if (!flag)
			{
				Location nestLocation = new Location(areaId, blockId);
				EnemyNestSiteExtraData enemyNestData = this._enemyNestSites[nestLocation];
				MonthlyNotificationCollection monthlyNotifications = DomainManager.World.GetMonthlyNotificationCollection();
				bool flag2 = enemyNestData.Tribute.Item1 < 0;
				if (flag2)
				{
					this.OfflineAddConqueredEnemyNestIncome(context, siteData, enemyNestData);
				}
				InstantNotificationCollection instantNotifications = DomainManager.World.GetInstantNotificationCollection();
				GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
				ValueTuple<sbyte, short, int> tribute = enemyNestData.Tribute;
				switch (siteData.TemplateId)
				{
				case 29:
				{
					sbyte stateId = DomainManager.Map.GetStateIdByAreaId(areaId);
					bool succeed = DomainManager.Information.GainRandomSettlementInformationByStateIdToCharacter(context, tribute.Item1, taiwu.GetId(), stateId);
					bool flag3 = !succeed;
					if (flag3)
					{
						siteData.SiteState = 2;
						enemyNestData.Tribute = new ValueTuple<sbyte, short, int>(-1, -1, -1);
						this.SetElement_EnemyNestSites(nestLocation, enemyNestData, context);
						this.SetElement_AdventureAreas((int)areaId, advArea, context);
						return;
					}
					break;
				}
				case 30:
					taiwu.ChangeResource(context, tribute.Item1, tribute.Item3);
					instantNotifications.AddResourceIncreased(taiwu.GetId(), tribute.Item1, tribute.Item3);
					break;
				case 31:
				{
					bool flag4 = charId == taiwu.GetId();
					if (flag4)
					{
						ItemKey itemKey = DomainManager.Item.CreateItem(context, tribute.Item1, tribute.Item2);
						taiwu.AddInventoryItem(context, itemKey, tribute.Item3, false);
						List<ItemDisplayData> itemDisplayDataList = DomainManager.Item.GetItemDisplayDataListOptional(new List<ItemKey>
						{
							itemKey
						}, DomainManager.Taiwu.GetTaiwuCharId(), -1, false);
						itemDisplayDataList[0].Amount = tribute.Item3;
						GameDataBridge.AddDisplayEvent<List<ItemDisplayData>, bool, bool>(DisplayEventType.OpenGetItem_Item, itemDisplayDataList, false, true);
					}
					else
					{
						DomainManager.Taiwu.CreateWarehouseItem(context, tribute.Item1, tribute.Item2, tribute.Item3);
					}
					instantNotifications.AddGetItem(taiwu.GetId(), tribute.Item1, tribute.Item2);
					break;
				}
				case 32:
					taiwu.ChangeExp(context, tribute.Item3);
					instantNotifications.AddExpIncreased(taiwu.GetId(), tribute.Item3);
					break;
				case 34:
					taiwu.ChangeResource(context, tribute.Item1, tribute.Item3);
					instantNotifications.AddResourceIncreased(taiwu.GetId(), tribute.Item1, tribute.Item3);
					break;
				case 36:
					siteData.SiteState = 2;
					return;
				case 37:
					taiwu.ChangeResource(context, tribute.Item1, tribute.Item3);
					instantNotifications.AddResourceIncreased(taiwu.GetId(), tribute.Item1, tribute.Item3);
					break;
				case 41:
				{
					List<GameData.Domains.Character.Character> charList = ObjectPool<List<GameData.Domains.Character.Character>>.Instance.Get();
					charList.Clear();
					MapCharacterFilter.Find(new Predicate<GameData.Domains.Character.Character>(CharacterMatchers.MatchCompletelyInfected), charList, areaId, true);
					bool flag5 = charList.Count > 0;
					if (flag5)
					{
						CollectionUtils.Shuffle<GameData.Domains.Character.Character>(context.Random, charList);
						foreach (GameData.Domains.Character.Character character in charList)
						{
							bool flag6 = character.GetLocation().Equals(nestLocation);
							if (!flag6)
							{
								DomainManager.Character.GroupMove(context, character, nestLocation);
								CharacterDomain.AddLockMovementCharSet(character.GetId());
								break;
							}
						}
					}
					ObjectPool<List<GameData.Domains.Character.Character>>.Instance.Return(charList);
					siteData.SiteState = 2;
					return;
				}
				}
				siteData.SiteState = 2;
				enemyNestData.Tribute = new ValueTuple<sbyte, short, int>(-1, -1, -1);
				this.SetElement_EnemyNestSites(nestLocation, enemyNestData, context);
				bool flag7 = charId != taiwu.GetId();
				if (flag7)
				{
					Location location = new Location(areaId, blockId);
					short advTemplateId = siteData.TemplateId;
					monthlyNotifications.AddIncomeFromNest(charId, location, taiwu.GetId(), advTemplateId);
				}
				this.SetElement_AdventureAreas((int)areaId, advArea, context);
			}
		}

		// Token: 0x06007EFF RID: 32511 RVA: 0x004C3358 File Offset: 0x004C1558
		private void FixOutdatedEnemyNests(DataContext context)
		{
			MonthlyActionKey actionKey = MonthlyEventActionsManager.PredefinedKeys["EnemyNestDefault"];
			EnemyNestMonthlyAction enemyNestAction = (EnemyNestMonthlyAction)DomainManager.TaiwuEvent.GetMonthlyAction(actionKey);
			List<Location> toRemoveLocations = new List<Location>();
			short areaId = 0;
			while ((int)areaId < this._adventureAreas.Length)
			{
				AreaAdventureData adventureArea = this._adventureAreas[(int)areaId];
				foreach (KeyValuePair<short, AdventureSiteData> keyValuePair in adventureArea.AdventureSites)
				{
					short num;
					AdventureSiteData adventureSiteData;
					keyValuePair.Deconstruct(out num, out adventureSiteData);
					short blockId = num;
					AdventureSiteData site = adventureSiteData;
					bool flag = site.SiteState >= 2;
					if (!flag)
					{
						bool flag2 = site.MonthlyActionKey.ActionType != 1;
						if (!flag2)
						{
							ConfigMonthlyAction monthlyAction = enemyNestAction.GetConfigAction(areaId, blockId);
							bool flag3 = monthlyAction != null;
							if (flag3)
							{
								bool flag4 = monthlyAction.MajorCharacterSets.Count > 0;
								if (flag4)
								{
									monthlyAction.ClearCalledCharacters();
									Logger logger = AdventureDomain.Logger;
									DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(57, 3);
									defaultInterpolatedStringHandler.AppendLiteral("Clearing characters called by outdated enemy nest ");
									defaultInterpolatedStringHandler.AppendFormatted(Adventure.Instance[site.TemplateId].Name);
									defaultInterpolatedStringHandler.AppendLiteral(" at (");
									defaultInterpolatedStringHandler.AppendFormatted<short>(areaId);
									defaultInterpolatedStringHandler.AppendLiteral(",");
									defaultInterpolatedStringHandler.AppendFormatted<short>(blockId);
									defaultInterpolatedStringHandler.AppendLiteral(")");
									logger.Warn(defaultInterpolatedStringHandler.ToStringAndClear());
								}
							}
							toRemoveLocations.Add(new Location(areaId, blockId));
						}
					}
				}
				areaId += 1;
			}
			foreach (Location location in toRemoveLocations)
			{
				this.RemoveAdventureSite(context, location.AreaId, location.BlockId, true, false);
			}
			enemyNestAction.ResetIntervals();
		}

		// Token: 0x06007F00 RID: 32512 RVA: 0x004C358C File Offset: 0x004C178C
		private void FixConqueredEnemyNests(DataContext context)
		{
			StringBuilder stringBuilder = new StringBuilder();
			int count = 0;
			short areaId = 0;
			while ((int)areaId < this._adventureAreas.Length)
			{
				AreaAdventureData adventureArea = this._adventureAreas[(int)areaId];
				bool changed = false;
				foreach (KeyValuePair<short, AdventureSiteData> keyValuePair in adventureArea.AdventureSites)
				{
					short num;
					AdventureSiteData adventureSiteData;
					keyValuePair.Deconstruct(out num, out adventureSiteData);
					short blockId = num;
					AdventureSiteData site = adventureSiteData;
					bool flag = site.SiteState < 2;
					if (!flag)
					{
						bool flag2 = site.MonthlyActionKey.IsValid();
						if (flag2)
						{
							MonthlyActionBase monthlyAction = DomainManager.TaiwuEvent.GetMonthlyAction(site.MonthlyActionKey);
							IMonthlyActionGroup actionGroup = monthlyAction as IMonthlyActionGroup;
							bool flag3 = actionGroup != null;
							if (flag3)
							{
								monthlyAction = actionGroup.GetConfigAction(areaId, blockId);
							}
							ConfigMonthlyAction configMonthlyAction = monthlyAction as ConfigMonthlyAction;
							bool flag4 = configMonthlyAction != null && configMonthlyAction.MajorCharacterSets.Count > 0;
							if (flag4)
							{
								configMonthlyAction.ClearCalledCharacters();
								Logger logger = AdventureDomain.Logger;
								DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(62, 3);
								defaultInterpolatedStringHandler.AppendLiteral("Clearing characters called by conquered adventure site ");
								defaultInterpolatedStringHandler.AppendFormatted(Adventure.Instance[site.TemplateId].Name);
								defaultInterpolatedStringHandler.AppendLiteral(" at (");
								defaultInterpolatedStringHandler.AppendFormatted<short>(areaId);
								defaultInterpolatedStringHandler.AppendLiteral(",");
								defaultInterpolatedStringHandler.AppendFormatted<short>(blockId);
								defaultInterpolatedStringHandler.AppendLiteral(")");
								logger.Warn(defaultInterpolatedStringHandler.ToStringAndClear());
							}
						}
						bool flag5 = site.RemainingMonths >= 0;
						if (!flag5)
						{
							sbyte enemyNestTemplateId = AdventureDomain.GetEnemyNestTemplateId(site.TemplateId);
							EnemyNestItem enemyNestCfg = EnemyNest.Instance[(short)enemyNestTemplateId];
							bool flag6 = enemyNestCfg.ConqueredDuration < 0;
							if (!flag6)
							{
								site.RemainingMonths = enemyNestCfg.ConqueredDuration;
								changed = true;
								stringBuilder.AppendFormat("Conquered Nest {0}'s Duration: -1 => {1}", Adventure.Instance[site.TemplateId].Name, site.RemainingMonths.ToString());
								stringBuilder.AppendLine();
								count++;
							}
						}
					}
				}
				bool flag7 = changed;
				if (flag7)
				{
					this.SetElement_AdventureAreas((int)areaId, adventureArea, context);
				}
				areaId += 1;
			}
			bool flag8 = count > 0;
			if (flag8)
			{
				Logger logger2 = AdventureDomain.Logger;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(42, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Fixing conquered enemy nests duration (");
				defaultInterpolatedStringHandler.AppendFormatted<int>(count);
				defaultInterpolatedStringHandler.AppendLiteral("):\n");
				defaultInterpolatedStringHandler.AppendFormatted<StringBuilder>(stringBuilder);
				logger2.Warn(defaultInterpolatedStringHandler.ToStringAndClear());
			}
		}

		// Token: 0x06007F01 RID: 32513 RVA: 0x004C3840 File Offset: 0x004C1A40
		[DomainMethod]
		public Location GmCmd_GenerateAdventure(DataContext context, short adventureId)
		{
			Location currentLocation = DomainManager.Taiwu.GetTaiwu().GetLocation();
			short monthlyActionId = -1;
			foreach (MonthlyActionsItem cfg in ((IEnumerable<MonthlyActionsItem>)MonthlyActions.Instance))
			{
				bool flag = cfg.AdventureId != adventureId;
				if (!flag)
				{
					monthlyActionId = cfg.TemplateId;
					break;
				}
			}
			bool flag2 = monthlyActionId < 0;
			Location result;
			if (flag2)
			{
				bool flag3 = adventureId == 144;
				if (flag3)
				{
					GameData.Domains.Character.Character taiwuChar = DomainManager.Taiwu.GetTaiwu();
					int spouseId = -1;
					HashSet<int> relatedCharIds = DomainManager.Character.GetRelatedCharIds(taiwuChar.GetId(), 0);
					foreach (int charId in relatedCharIds)
					{
						GameData.Domains.Character.Character targetChar;
						bool flag4 = !DomainManager.Character.TryGetElement_Objects(charId, out targetChar);
						if (!flag4)
						{
							bool flag5 = targetChar.IsCompletelyInfected();
							if (!flag5)
							{
								bool flag6 = targetChar.GetAgeGroup() != 2;
								if (!flag6)
								{
									bool flag7 = DomainManager.Character.GetAliveSpouse(charId) >= 0;
									if (!flag7)
									{
										bool flag8 = targetChar.IsActiveExternalRelationState(60);
										if (!flag8)
										{
											bool flag9 = !targetChar.GetLocation().IsValid();
											if (!flag9)
											{
												RelatedCharacter selfToTarget = DomainManager.Character.GetRelation(taiwuChar.GetId(), charId);
												bool flag10 = RelationType.ContainBloodExclusionRelations(selfToTarget.RelationType);
												if (!flag10)
												{
													spouseId = charId;
													break;
												}
											}
										}
									}
								}
							}
						}
					}
					bool flag11 = spouseId < 0;
					if (flag11)
					{
						result = Location.Invalid;
					}
					else
					{
						Location location = taiwuChar.GetLocation();
						MonthlyActionKey key = EventHelper.TriggerTaiwuWeddingAdventure(spouseId, location);
						result = location;
					}
				}
				else
				{
					bool flag12 = this._adventureAreas[(int)currentLocation.AreaId].AdventureSites.ContainsKey(currentLocation.BlockId);
					if (flag12)
					{
						result = currentLocation;
					}
					else
					{
						bool flag13 = this.TryCreateAdventureSite(context, currentLocation.AreaId, currentLocation.BlockId, adventureId, MonthlyActionKey.Invalid);
						if (flag13)
						{
							result = currentLocation;
						}
						else
						{
							result = Location.Invalid;
						}
					}
				}
			}
			else
			{
				MonthlyActionsItem monthlyActionCfg = MonthlyActions.Instance[monthlyActionId];
				bool isEnemyNest = monthlyActionCfg.IsEnemyNest;
				if (isEnemyNest)
				{
					bool flag14 = currentLocation.AreaId >= 45;
					if (flag14)
					{
						result = Location.Invalid;
					}
					else
					{
						this.RemoveAdventureSite(context, currentLocation.AreaId, currentLocation.BlockId, false, false);
						EnemyNestMonthlyAction nestAction = (EnemyNestMonthlyAction)DomainManager.TaiwuEvent.GetMonthlyAction(new MonthlyActionKey(1, 0));
						ConfigMonthlyAction action = nestAction.CreateNewConfigAction(monthlyActionId, currentLocation.AreaId);
						action.Location = currentLocation;
						action.TriggerAction();
						bool flag15 = action.Location.IsValid();
						if (flag15)
						{
							action.MonthlyHandler();
							DomainManager.TaiwuEvent.GmCmd_SaveMonthlyActionManager(context);
						}
						result = action.Location;
					}
				}
				else
				{
					bool flag16 = monthlyActionCfg.MinInterval > 0;
					if (flag16)
					{
						ConfigMonthlyAction action2 = (ConfigMonthlyAction)DomainManager.TaiwuEvent.GetMonthlyAction(new MonthlyActionKey(0, monthlyActionId));
						bool flag17 = action2.State == 0;
						if (flag17)
						{
							action2.SelectLocation();
							action2.TriggerAction();
							bool flag18 = action2.Location.IsValid();
							if (flag18)
							{
								AdventureSiteData advSite = this._adventureAreas[(int)action2.Location.AreaId].AdventureSites[action2.Location.BlockId];
								advSite.TemplateId = adventureId;
								action2.MonthlyHandler();
								DomainManager.TaiwuEvent.GmCmd_SaveMonthlyActionManager(context);
							}
						}
						result = action2.Location;
					}
					else
					{
						bool flag19 = ConfigMonthlyActionDefines.OrgTemplateIdToContestForTaiwuBride.ContainsValue(monthlyActionId);
						if (flag19)
						{
							bool flag20 = currentLocation.AreaId >= 45;
							if (flag20)
							{
								result = Location.Invalid;
							}
							else
							{
								MonthlyActionKey key2 = MonthlyEventActionsManager.PredefinedKeys["BrideOpenContestDefault"];
								ConfigWrapperAction wrapperAction = (ConfigWrapperAction)DomainManager.TaiwuEvent.GetMonthlyAction(key2);
								bool flag21 = wrapperAction.CurrConfigMonthlyAction == null;
								if (flag21)
								{
									wrapperAction.CreateWrappedAction(monthlyActionId, -1);
									ConfigMonthlyAction action3 = wrapperAction.CurrConfigMonthlyAction;
									bool flag22 = action3 != null;
									if (flag22)
									{
										AdventureSiteData advSite2 = this._adventureAreas[(int)action3.Location.AreaId].AdventureSites[action3.Location.BlockId];
										advSite2.TemplateId = adventureId;
										action3.MonthlyHandler();
										DomainManager.TaiwuEvent.GmCmd_SaveMonthlyActionManager(context);
										result = wrapperAction.CurrConfigMonthlyAction.Location;
									}
									else
									{
										result = Location.Invalid;
									}
								}
								else
								{
									bool flag23 = wrapperAction.CurrConfigMonthlyAction.ConfigTemplateId == monthlyActionId;
									if (flag23)
									{
										result = wrapperAction.CurrConfigMonthlyAction.Location;
									}
									else
									{
										result = Location.Invalid;
									}
								}
							}
						}
						else
						{
							result = Location.Invalid;
						}
					}
				}
			}
			return result;
		}

		// Token: 0x06007F02 RID: 32514 RVA: 0x004C3D24 File Offset: 0x004C1F24
		[DomainMethod]
		public int GmCmd_GetAdventureParameter(string key)
		{
			return EventHelper.GetAdventureParameter(key);
		}

		// Token: 0x06007F03 RID: 32515 RVA: 0x004C3D3C File Offset: 0x004C1F3C
		[DomainMethod]
		public void GmCmd_SetAdventureParameter(string key, int value)
		{
			EventHelper.SetAdventureParameter(key, value);
		}

		// Token: 0x06007F04 RID: 32516 RVA: 0x004C3D48 File Offset: 0x004C1F48
		[DomainMethod]
		public bool TryInvokeConfirmEnterEvent()
		{
			DomainManager.TaiwuEvent.OnEvent_ConfirmEnterSwordTomb();
			return DomainManager.TaiwuEvent.IsShowingEvent || DomainManager.TaiwuEvent.GetHasListeningEvent();
		}

		// Token: 0x170004EC RID: 1260
		// (get) Token: 0x06007F05 RID: 32517 RVA: 0x004C3D7E File Offset: 0x004C1F7E
		// (set) Token: 0x06007F06 RID: 32518 RVA: 0x004C3D86 File Offset: 0x004C1F86
		public AdventureItem AdventureCfg { get; private set; }

		// Token: 0x170004ED RID: 1261
		// (get) Token: 0x06007F07 RID: 32519 RVA: 0x004C3D8F File Offset: 0x004C1F8F
		private AdvMapNode CurNode
		{
			get
			{
				AdvMapNode result;
				if ((result = this._curBranch.GetNode(this.PlayerPos)) == null)
				{
					AdvancedBranch advancedBranch = this._curBranch.AdvancedBranch;
					result = ((advancedBranch != null) ? advancedBranch.GetNode(this.PlayerPos) : null);
				}
				return result;
			}
		}

		// Token: 0x170004EE RID: 1262
		// (get) Token: 0x06007F08 RID: 32520 RVA: 0x004C3DC3 File Offset: 0x004C1FC3
		public sbyte CurAdventureLifeSkillDifficulty
		{
			get
			{
				return GameData.Domains.Adventure.SharedMethods.GetAdventureLifeSkillDifficulty(this.AdventureCfg.TemplateId, DomainManager.World.GetXiangshuLevel());
			}
		}

		// Token: 0x06007F09 RID: 32521 RVA: 0x004C3DE0 File Offset: 0x004C1FE0
		private void InitializeMap(DataContext context)
		{
			this._triggeredPosSet.Clear();
			this._finishedPosSet.Clear();
			this._vertices.Clear();
			this._branches.Clear();
			this._spentCharList.Clear();
			this._teamDetectedNodes.Clear();
			this._curBranchChosenChar = -1;
			this._mapTrunks.Clear();
			this._onEventFinishCallback = null;
			this._eventBlock = false;
			this._curMapTrunk = new AdventureMapTrunk();
			this._indicatePath.Clear();
			this.SetCurMapTrunk(this._curMapTrunk, context);
			this.ClearPathArrangement(context);
			this.SetPlayerPos(int.MinValue, context);
			this.SetAllowExitAdventure(this.AdventureCfg.Interruptible > 0, context);
			foreach (AdventureStartNode one in this.AdventureCfg.StartNodes)
			{
				this._vertices.Add(new AdvMapNodeVertex(ENodeType.Start, this._vertices.Count, (one.TerrainId >= 0) ? one.TerrainId : this._enterTerrain, one.NodeKey));
			}
			foreach (AdventureTransferNode one2 in this.AdventureCfg.TransferNodes)
			{
				this._vertices.Add(new AdvMapNodeVertex(ENodeType.Transfer, this._vertices.Count, one2.TerrainId, one2.NodeKey));
			}
			foreach (AdventureEndNode one3 in this.AdventureCfg.EndNodes)
			{
				this._vertices.Add(new AdvMapNodeVertex(ENodeType.End, this._vertices.Count, (one3.TerrainId >= 0) ? one3.TerrainId : this._enterTerrain, one3.NodeKey));
			}
			foreach (AdventureBaseBranch one4 in this.AdventureCfg.BaseBranches)
			{
				AdvMapBranch branch = new AdvMapBranch(this._branches.Count, this._vertices[(int)one4.PortA], this._vertices[(int)one4.PortB], one4, context);
				this._branches.Add(branch);
				this._vertices[(int)one4.PortA].ConnectedBranchDict.Add(this._vertices[(int)one4.PortB], branch);
				this._vertices[(int)one4.PortB].ConnectedBranchDict.Add(this._vertices[(int)one4.PortA], branch);
			}
		}

		// Token: 0x06007F0A RID: 32522 RVA: 0x004C4108 File Offset: 0x004C2308
		public AdventureMapTrunk GenerateNextAdvMapTrunk(DataContext context)
		{
			AdvMapNodeVertex startingVertex = this._curBranch.ExitNode;
			Logger logger = AdventureDomain.Logger;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(65, 2);
			defaultInterpolatedStringHandler.AppendLiteral("Generating branch with \"");
			defaultInterpolatedStringHandler.AppendFormatted(startingVertex.NodeKey);
			defaultInterpolatedStringHandler.AppendLiteral("\" as entrance node key, \"");
			defaultInterpolatedStringHandler.AppendFormatted(this._chosenBranchKey);
			defaultInterpolatedStringHandler.AppendLiteral("\" as branch key.");
			logger.Info(defaultInterpolatedStringHandler.ToStringAndClear());
			AdvMapNodeVertex endingVertex = this._curBranch.ExitNode.ConnectedBranchDict.Values.First((AdvMapBranch a) => a.EnterNode == startingVertex && a.BranchKey == this._chosenBranchKey).ExitNode;
			this.SetCurBranchChosenChar(-1, context);
			return this.GenerateAdvMapTrunk(context, startingVertex, endingVertex);
		}

		// Token: 0x06007F0B RID: 32523 RVA: 0x004C41E0 File Offset: 0x004C23E0
		public AdventureMapTrunk GenerateAdvMapTrunk(DataContext context, AdvMapNodeVertex startingVertex, AdvMapNodeVertex endingVertex)
		{
			this._curBranch = startingVertex.ConnectedBranchDict[endingVertex];
			this._curBranch.GenBranch(context, this._extraEvents, null);
			this._extraEvents.Clear();
			AdventureMapTrunk ret = new AdventureMapTrunk();
			ret.BranchIndex = this._curBranch.BranchIndex;
			bool flag = startingVertex.NodeType == ENodeType.Start;
			if (flag)
			{
				ret.Points.Add(startingVertex.ToAdventureMapPoint());
			}
			ret.Points.Add(endingVertex.ToAdventureMapPoint());
			foreach (AdvMapNodeNormal node in this._curBranch.Nodes)
			{
				ret.Points.Add(node.ToAdventureMapPoint());
			}
			bool flag2 = this._curBranch.AdvancedBranch != null;
			if (flag2)
			{
				foreach (AdvMapNodeNormal node2 in this._curBranch.AdvancedBranch.Nodes)
				{
					ret.Points.Add(node2.ToAdventureMapPoint());
				}
			}
			this._curBranch.FillConnect(ret.Connects, false);
			this.FillFollowNodes(context, ret, endingVertex);
			this._mapTrunks.Push(ret);
			return ret;
		}

		// Token: 0x06007F0C RID: 32524 RVA: 0x004C4368 File Offset: 0x004C2568
		private unsafe void FillFollowNodes(DataContext context, AdventureMapTrunk ret, AdvMapNodeVertex nextNode)
		{
			bool flag = nextNode.NodeType == ENodeType.Transfer;
			if (flag)
			{
				List<AdvMapNodeVertex> followNodes = (from a in nextNode.ConnectedBranchDict.Values
				where a.EnterNode == nextNode
				select a.ExitNode).ToList<AdvMapNodeVertex>();
				IntPtr intPtr = stackalloc byte[(UIntPtr)3];
				cpblk(intPtr, ref <PrivateImplementationDetails>.8D9FE7317F066DEACA4FDB6C313194E5BB5D2269ECF672F1AF9FC790A2205991, 3);
				Span<ELinkDir> span = new Span<ELinkDir>(intPtr, 3);
				Span<ELinkDir> toRightDir = span;
				CollectionUtils.Shuffle<ELinkDir>(context.Random, toRightDir, toRightDir.Length);
				for (int i = 0; i < followNodes.Count; i++)
				{
					AdvMapBranch pathBranch = nextNode.ConnectedBranchDict[followNodes[i]];
					pathBranch.EnterDir = *toRightDir[i];
					ret.Points.Add(pathBranch.FirstNode.ToAdventureMapPoint());
					AdvMapPos posA = nextNode.AdjustedPos;
					AdvMapPos posB = posA + pathBranch.EnterDir.Rotate(new AdvMapPos(4, 0));
					ret.Connects.Add(new AdventureMapConnect(posA.GetHashCode(), posB.GetHashCode()));
				}
			}
		}

		// Token: 0x06007F0D RID: 32525 RVA: 0x004C44D0 File Offset: 0x004C26D0
		private ValueTuple<int, int> HandleNodeContent_Event(DataContext context, AdventureBranch branchCfg, AdventureMapPoint pointData)
		{
			this.TriggerGlobalEvent(branchCfg, pointData, delegate
			{
				DomainManager.TaiwuEvent.OnEvent_AdventureEnterNode(pointData);
			});
			return new ValueTuple<int, int>(0, 0);
		}

		// Token: 0x06007F0E RID: 32526 RVA: 0x004C4510 File Offset: 0x004C2710
		private ValueTuple<int, int> HandleNodeContent_Resource(DataContext context, AdventureBranch branchCfg, AdventureMapPoint pointData)
		{
			GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
			pointData.JudgeSuccess = (this._perceivedNodes.Contains(this.CurNode.AdjustedPos.GetHashCode()) || taiwu.GetLifeSkillAttainment((sbyte)this.CurNode.LifeSkillType) >= this.CurNode.LifeSkillRequiredVal);
			byte resType = 0;
			short resAmount = 0;
			bool judgeSuccess = pointData.JudgeSuccess;
			if (judgeSuccess)
			{
				bool flag = !branchCfg.PersonalityContentWeights.CheckIndex((int)pointData.SevenElementType);
				if (flag)
				{
					Logger logger = AdventureDomain.Logger;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(58, 2);
					defaultInterpolatedStringHandler.AppendLiteral("Invalid seven element type ");
					defaultInterpolatedStringHandler.AppendFormatted<sbyte>(pointData.SevenElementType);
					defaultInterpolatedStringHandler.AppendLiteral(" ");
					defaultInterpolatedStringHandler.AppendLiteral("for branch with weights count ");
					defaultInterpolatedStringHandler.AppendFormatted<int>(branchCfg.PersonalityContentWeights.Length);
					logger.Warn(defaultInterpolatedStringHandler.ToStringAndClear());
					this.TriggerGlobalEvent(branchCfg, pointData, delegate
					{
						this.OnEventHandleFinished(context);
					});
					pointData.JudgeSuccess = false;
					return new ValueTuple<int, int>(-1, -1);
				}
				bool flag2 = !branchCfg.PersonalityContentWeights[(int)pointData.SevenElementType].NormalResWeights.CheckIndex(this.CurNode.NodeContent.Item2);
				if (flag2)
				{
					Logger logger2 = AdventureDomain.Logger;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(58, 3);
					defaultInterpolatedStringHandler.AppendLiteral("Invalid node content (");
					defaultInterpolatedStringHandler.AppendFormatted<sbyte>(this.CurNode.NodeContent.Item1);
					defaultInterpolatedStringHandler.AppendLiteral(",");
					defaultInterpolatedStringHandler.AppendFormatted<int>(this.CurNode.NodeContent.Item2);
					defaultInterpolatedStringHandler.AppendLiteral(" ");
					defaultInterpolatedStringHandler.AppendLiteral("for normal resource weights count");
					defaultInterpolatedStringHandler.AppendFormatted<int>(branchCfg.PersonalityContentWeights[(int)pointData.SevenElementType].NormalResWeights.Length);
					defaultInterpolatedStringHandler.AppendLiteral(")");
					logger2.Warn(defaultInterpolatedStringHandler.ToStringAndClear());
					this.TriggerGlobalEvent(branchCfg, pointData, delegate
					{
						this.OnEventHandleFinished(context);
					});
					pointData.JudgeSuccess = false;
					return new ValueTuple<int, int>(-1, -1);
				}
				int index = this._indicatePath.IndexOf(this.CurNode.AdjustedPos.GetHashCode());
				bool flag3 = index > -1 && this._contentList.CheckIndex(index);
				if (flag3)
				{
					resType = (byte)this._contentList[index].Item1;
					resAmount = (short)this._contentList[index].Item2;
				}
				bool flag4 = (int)resType == Config.ResourceType.Instance.Count;
				if (flag4)
				{
					taiwu.ChangeExp(context, (int)resAmount);
					this._resultDisplayData.ChangedExp += (int)resAmount;
				}
				else
				{
					taiwu.ChangeResource(context, (sbyte)resType, (int)resAmount);
					this._resultDisplayData.ChangedResources.Add((sbyte)resType, (int)resAmount);
				}
			}
			this.TriggerGlobalEvent(branchCfg, pointData, delegate
			{
				this.OnEventHandleFinished(context);
			});
			return pointData.JudgeSuccess ? new ValueTuple<int, int>((int)resType, (int)resAmount) : new ValueTuple<int, int>(-1, -1);
		}

		// Token: 0x06007F0F RID: 32527 RVA: 0x004C4854 File Offset: 0x004C2A54
		[DomainMethod]
		public ItemDisplayData GetAdventureGainsItemList(ItemKey itemKey)
		{
			return this._resultDisplayData.ItemList.Find((ItemDisplayData d) => d.Key.TemplateEquals(itemKey));
		}

		// Token: 0x06007F10 RID: 32528 RVA: 0x004C4894 File Offset: 0x004C2A94
		private ValueTuple<int, int> HandleNodeContent_Item(DataContext context, AdventureBranch branchCfg, AdventureMapPoint pointData)
		{
			GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
			pointData.JudgeSuccess = (this._perceivedNodes.Contains(this.CurNode.AdjustedPos.GetHashCode()) || taiwu.GetLifeSkillAttainment((sbyte)this.CurNode.LifeSkillType) >= this.CurNode.LifeSkillRequiredVal);
			byte itemType = 0;
			short itemTemplateId = 0;
			bool judgeSuccess = pointData.JudgeSuccess;
			if (judgeSuccess)
			{
				bool flag = !branchCfg.PersonalityContentWeights.CheckIndex((int)pointData.SevenElementType);
				if (flag)
				{
					Logger logger = AdventureDomain.Logger;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(58, 2);
					defaultInterpolatedStringHandler.AppendLiteral("Invalid seven element type ");
					defaultInterpolatedStringHandler.AppendFormatted<sbyte>(pointData.SevenElementType);
					defaultInterpolatedStringHandler.AppendLiteral(" ");
					defaultInterpolatedStringHandler.AppendLiteral("for branch with weights count ");
					defaultInterpolatedStringHandler.AppendFormatted<int>(branchCfg.PersonalityContentWeights.Length);
					logger.Warn(defaultInterpolatedStringHandler.ToStringAndClear());
					pointData.JudgeSuccess = false;
					this.TriggerGlobalEvent(branchCfg, pointData, delegate
					{
						this.OnEventHandleFinished(context);
					});
					return new ValueTuple<int, int>(-1, -1);
				}
				bool flag2 = !branchCfg.PersonalityContentWeights[(int)pointData.SevenElementType].SpecialResWeights.CheckIndex(this.CurNode.NodeContent.Item2);
				if (flag2)
				{
					Logger logger2 = AdventureDomain.Logger;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(54, 3);
					defaultInterpolatedStringHandler.AppendLiteral("Invalid node content (");
					defaultInterpolatedStringHandler.AppendFormatted<sbyte>(this.CurNode.NodeContent.Item1);
					defaultInterpolatedStringHandler.AppendLiteral(",");
					defaultInterpolatedStringHandler.AppendFormatted<int>(this.CurNode.NodeContent.Item2);
					defaultInterpolatedStringHandler.AppendLiteral(" ");
					defaultInterpolatedStringHandler.AppendLiteral("for special resource weights ");
					defaultInterpolatedStringHandler.AppendFormatted<int>(branchCfg.PersonalityContentWeights[(int)pointData.SevenElementType].SpecialResWeights.Length);
					defaultInterpolatedStringHandler.AppendLiteral(")");
					logger2.Warn(defaultInterpolatedStringHandler.ToStringAndClear());
					pointData.JudgeSuccess = false;
					this.TriggerGlobalEvent(branchCfg, pointData, delegate
					{
						this.OnEventHandleFinished(context);
					});
					return new ValueTuple<int, int>(-1, -1);
				}
				ValueTuple<byte, short, short, short> valueTuple = branchCfg.PersonalityContentWeights[(int)pointData.SevenElementType].SpecialResWeights[this.CurNode.NodeContent.Item2];
				itemType = valueTuple.Item1;
				itemTemplateId = valueTuple.Item2;
				short itemAmount = valueTuple.Item3;
				bool flag3 = itemTemplateId < 0;
				if (flag3)
				{
					int index = this._indicatePath.IndexOf(this.CurNode.AdjustedPos.GetHashCode());
					bool flag4 = index > -1 && this._contentList.CheckIndex(index);
					if (flag4)
					{
						itemTemplateId = (short)this._contentList[index].Item2;
					}
				}
				ItemKey itemKey = new ItemKey((sbyte)itemType, 0, itemTemplateId, -1);
				ItemDisplayData oldItemData = this._resultDisplayData.ItemList.Find((ItemDisplayData d) => d.Key.TemplateEquals(itemKey) && ItemTemplateHelper.IsStackable(itemKey.ItemType, itemKey.TemplateId));
				bool flag5 = oldItemData == null;
				if (flag5)
				{
					ItemKey newItemKey = DomainManager.Item.CreateItem(context, (sbyte)itemType, itemTemplateId);
					ItemDisplayData itemData = DomainManager.Item.GetItemDisplayData(newItemKey, -1);
					itemData.Amount = (int)itemAmount;
					this._resultDisplayData.ItemList.Add(itemData);
				}
				else
				{
					oldItemData.Amount += (int)itemAmount;
				}
			}
			this.TriggerGlobalEvent(branchCfg, pointData, delegate
			{
				this.OnEventHandleFinished(context);
			});
			return pointData.JudgeSuccess ? new ValueTuple<int, int>((int)itemType, (int)itemTemplateId) : new ValueTuple<int, int>(-1, -1);
		}

		// Token: 0x06007F11 RID: 32529 RVA: 0x004C4C44 File Offset: 0x004C2E44
		private ValueTuple<int, int> HandleNodeContent_Bonus(DataContext context, AdventureBranch branchCfg, AdventureMapPoint pointData)
		{
			GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
			pointData.JudgeSuccess = (pointData.JudgeSuccess = (this._perceivedNodes.Contains(this.CurNode.AdjustedPos.GetHashCode()) || taiwu.GetLifeSkillAttainment((sbyte)this.CurNode.LifeSkillType) >= this.CurNode.LifeSkillRequiredVal));
			bool judgeSuccess = pointData.JudgeSuccess;
			if (judgeSuccess)
			{
				this.TriggerGlobalEvent(branchCfg, pointData, delegate
				{
					DomainManager.TaiwuEvent.OnEvent_AdventureEnterNode(pointData);
				});
			}
			else
			{
				this.TriggerGlobalEvent(branchCfg, pointData, delegate
				{
					this.OnEventHandleFinished(context);
				});
			}
			return pointData.JudgeSuccess ? new ValueTuple<int, int>(0, 0) : new ValueTuple<int, int>(-1, -1);
		}

		// Token: 0x06007F12 RID: 32530 RVA: 0x004C4D48 File Offset: 0x004C2F48
		private ValueTuple<int, int> HandleNodeContent_ExtraEvent(DataContext context, AdventureBranch branchCfg, AdventureMapPoint pointData)
		{
			this.TriggerGlobalEvent(branchCfg, pointData, delegate
			{
				DomainManager.TaiwuEvent.TriggerAdventureExtraEvent(this._curBranch.GetExtraEvent(pointData.NodeContentIndex), pointData);
			});
			return new ValueTuple<int, int>(0, 0);
		}

		// Token: 0x06007F13 RID: 32531 RVA: 0x004C4D90 File Offset: 0x004C2F90
		private void TriggerGlobalEvent(AdventureBranch branchCfg, AdventureMapPoint pointData, Action onFinishCallback)
		{
			bool flag = !string.IsNullOrEmpty(branchCfg.GlobalEvent);
			if (flag)
			{
				this._onEventFinishCallback = onFinishCallback;
				DomainManager.TaiwuEvent.TriggerAdventureGlobalEvent(pointData);
			}
			else
			{
				onFinishCallback();
			}
		}

		// Token: 0x06007F14 RID: 32532 RVA: 0x004C4DCE File Offset: 0x004C2FCE
		[DomainMethod]
		public List<ValueTuple<int, int>> GetPathContentList()
		{
			return this._contentList;
		}

		// Token: 0x06007F15 RID: 32533 RVA: 0x004C4DD8 File Offset: 0x004C2FD8
		private void InitPathContent(DataContext context)
		{
			this._contentList.Clear();
			using (List<int>.Enumerator enumerator = this._indicatePath.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					int pos = enumerator.Current;
					AdvMapNode node = this._path.Find((AdvMapNode n) => n.AdjustedPos.GetHashCode() == pos);
					bool flag = node.NodeType != ENodeType.Normal;
					if (flag)
					{
						this._contentList.Add(new ValueTuple<int, int>(-1, -1));
					}
					else
					{
						AdvBaseMapBranch branch = (this._curBranch.AdvancedBranch != null && this._curBranch.AdvancedBranch.Nodes.Contains((AdvMapNodeNormal)node)) ? this._curBranch.AdvancedBranch : this._curBranch;
						int branchId = branch.BranchIndex;
						AdventureBranch branchCfg = (branchId < this.AdventureCfg.BaseBranches.Count) ? this.AdventureCfg.BaseBranches[branchId] : this.AdventureCfg.AdvancedBranches[branchId - this.AdventureCfg.BaseBranches.Count];
						bool flag2 = !branchCfg.PersonalityContentWeights.CheckIndex((int)node.SevenElementType);
						if (flag2)
						{
							this._contentList.Add(new ValueTuple<int, int>(-1, -1));
						}
						else
						{
							sbyte item = node.NodeContent.Item1;
							sbyte b = item;
							switch (b)
							{
							case 0:
								this._contentList.Add(new ValueTuple<int, int>(0, 0));
								break;
							case 1:
							{
								bool flag3 = branchCfg.PersonalityContentWeights[(int)node.SevenElementType].NormalResWeights.CheckIndex(node.NodeContent.Item2);
								if (flag3)
								{
									ValueTuple<byte, short, short> valueTuple = branchCfg.PersonalityContentWeights[(int)node.SevenElementType].NormalResWeights[node.NodeContent.Item2];
									byte resType = valueTuple.Item1;
									short resAmount = valueTuple.Item2;
									int minAmount = Math.Max(1, (int)(resAmount * 90 / 100));
									int maxAmount = (int)(resAmount * 110 / 100);
									resAmount = (short)(context.Random.Next(minAmount, maxAmount) * DomainManager.World.GetGainResourcePercent(4) / 100);
									this._contentList.Add(new ValueTuple<int, int>((int)resType, (int)resAmount));
								}
								else
								{
									this._contentList.Add(new ValueTuple<int, int>(-1, -1));
								}
								break;
							}
							case 2:
							{
								bool flag4 = branchCfg.PersonalityContentWeights[(int)node.SevenElementType].SpecialResWeights.CheckIndex(node.NodeContent.Item2);
								if (flag4)
								{
									ValueTuple<byte, short, short, short> valueTuple2 = branchCfg.PersonalityContentWeights[(int)node.SevenElementType].SpecialResWeights[node.NodeContent.Item2];
									byte itemType = valueTuple2.Item1;
									short templateId = valueTuple2.Item2;
									bool flag5 = templateId < 0;
									if (flag5)
									{
										sbyte grade = (sbyte)RandomUtils.GetRandomIndex(AdventureItemDropRate.Instance[(int)(this.CurAdventureLifeSkillDifficulty - 1)].ItemGradeDropRate, context.Random);
										templateId = ItemDomain.GetRandomItemIdInSubType(context.Random, -templateId - 1, grade);
									}
									this._contentList.Add(new ValueTuple<int, int>((int)itemType, (int)templateId));
								}
								else
								{
									this._contentList.Add(new ValueTuple<int, int>(-1, -1));
								}
								break;
							}
							case 3:
								this._contentList.Add(new ValueTuple<int, int>(0, 0));
								break;
							default:
								if (b != 10)
								{
									this._contentList.Add(new ValueTuple<int, int>(-1, -1));
								}
								else
								{
									this._contentList.Add(new ValueTuple<int, int>(0, 0));
								}
								break;
							}
						}
					}
				}
			}
		}

		// Token: 0x06007F16 RID: 32534 RVA: 0x004C518C File Offset: 0x004C338C
		[Obsolete("EnemyNest can no longer be upgraded")]
		public void UpgradeEnemyNest(DataContext context, short areaId, short blockId, short newAdventureId)
		{
			Location location = new Location(areaId, blockId);
			AdventureSiteData siteData = this._adventureAreas[(int)areaId].AdventureSites[blockId];
			EnemyNestSiteExtraData nestExtraData = this._enemyNestSites[location];
			nestExtraData.UpgradeChance = 0;
			siteData.TemplateId = newAdventureId;
			siteData.SiteState = 0;
			this.ClearRandomEnemiesBySite(context, location);
			this.GenerateRandomEnemiesBySite(context, siteData, areaId, blockId);
			this.SetElement_AdventureAreas((int)areaId, this._adventureAreas[(int)areaId], context);
		}

		// Token: 0x06007F17 RID: 32535 RVA: 0x004C5200 File Offset: 0x004C3400
		[Obsolete("EnemyNest can no longer be upgraded")]
		public bool TryUpgradeEnemyNest(DataContext context, short areaId, short blockId)
		{
			Location location = new Location(areaId, blockId);
			AdventureSiteData siteData = this._adventureAreas[(int)areaId].AdventureSites[blockId];
			EnemyNestSiteExtraData nestExtraData = this._enemyNestSites[location];
			sbyte enemyNestTemplateId = AdventureDomain.GetEnemyNestTemplateId(siteData.TemplateId);
			int curLevel = EnemyNestConstValues.HereticStrongholdNestIds.IndexOf((short)enemyNestTemplateId);
			bool flag = curLevel < 0 || curLevel >= EnemyNestConstValues.HereticStrongholdNestIds.Length - 1;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				short upgradedEnemyNestTemplateId = EnemyNestConstValues.HereticStrongholdNestIds[curLevel + 1];
				EnemyNestItem enemyNestCfg = EnemyNest.Instance[upgradedEnemyNestTemplateId];
				bool flag2 = enemyNestCfg.MonthlyActionId < 0 || enemyNestCfg.AdventureId < 0;
				if (flag2)
				{
					result = false;
				}
				else
				{
					bool flag3 = enemyNestCfg.WorldTotalCountLimit > 0 && this.GetEnemyNestCount(upgradedEnemyNestTemplateId) >= (int)enemyNestCfg.WorldTotalCountLimit;
					if (flag3)
					{
						result = false;
					}
					else
					{
						MonthlyActionKey enemyNestKey = new MonthlyActionKey(1, 0);
						EnemyNestMonthlyAction enemyNestAction = (EnemyNestMonthlyAction)DomainManager.TaiwuEvent.GetMonthlyAction(enemyNestKey);
						ConfigMonthlyAction action = enemyNestAction.GetConfigAction(areaId, blockId);
						action.State = 1;
						action.Month = 0;
						action.ClearCalledCharacters();
						action.ConfigTemplateId = enemyNestCfg.MonthlyActionId;
						nestExtraData.UpgradeChance = 0;
						siteData.TemplateId = enemyNestCfg.AdventureId;
						siteData.SiteState = 0;
						this.UnregisterEnemyNest((short)enemyNestTemplateId);
						this.RegisterEnemyNest(upgradedEnemyNestTemplateId);
						this.ClearRandomEnemiesBySite(context, location);
						this.GenerateRandomEnemiesBySite(context, siteData, areaId, blockId);
						this.SetElement_AdventureAreas((int)areaId, this._adventureAreas[(int)areaId], context);
						result = true;
					}
				}
			}
			return result;
		}

		// Token: 0x06007F18 RID: 32536 RVA: 0x004C538C File Offset: 0x004C358C
		[Obsolete("EnemyNest can no longer be upgraded")]
		public void SetEnemyNestUpgradeChance(DataContext context, short areaId, short blockId, short chance)
		{
			Location location = new Location(areaId, blockId);
			EnemyNestSiteExtraData nestData = this._enemyNestSites[location];
			nestData.UpgradeChance = chance;
			this.SetElement_EnemyNestSites(location, nestData, context);
		}

		// Token: 0x06007F19 RID: 32537 RVA: 0x004C53C4 File Offset: 0x004C35C4
		[DomainMethod]
		public bool EnterAdventureByConfigData(DataContext context, AdventureItem configData, int startNodeIndex, int enterTerrain = 1)
		{
			context.SwitchRandomSource("EnterAdventureByConfigData");
			this._enterTerrain = enterTerrain;
			this.EnterTerrainWeights = MapBlock.Instance[this._enterTerrain].AdventureTerrainWeights;
			for (sbyte personality = 0; personality < 7; personality += 1)
			{
				this._personalities[(int)personality] = 20;
			}
			this.SetPersonalities(this._personalities, context);
			this.AdventureCfg = configData;
			this.ClearAdvParameters();
			this.InitializeMap(context);
			this.GenerateAdventureMap(context, this._vertices[startNodeIndex].NodeKey);
			this.SetCurAdventureId(configData.TemplateId, context);
			return true;
		}

		// Token: 0x06007F1A RID: 32538 RVA: 0x004C5470 File Offset: 0x004C3670
		[DomainMethod]
		public void SelectBranchForPreview(DataContext context, sbyte nodeBranchIndex)
		{
			AdvMapBranch targetBranch = this._curBranch.ExitNode.ConnectedBranchDict.Values.FirstOrDefault((AdvMapBranch node) => node.BranchIndex == (int)nodeBranchIndex && node.EnterNode == this._curBranch.ExitNode);
			bool flag = targetBranch == null;
			if (!flag)
			{
				this.SetCurMapTrunk(this.GenerateAdvMapTrunk(context, targetBranch.EnterNode, targetBranch.ExitNode), context);
				this.SetAdventureState(1, context);
				this.SetPlayerPos(this._curBranch.EnterNode.AdjustedPos.GetHashCode(), context);
				this._arrangedNodes.Clear();
				this.SetArrangedNodes(this._arrangedNodes, context);
				this.RefreshIndicatePath(context);
			}
		}

		// Token: 0x06007F1B RID: 32539 RVA: 0x004C5530 File Offset: 0x004C3730
		[DomainMethod]
		public void ClearBranchForPreview(DataContext context)
		{
			bool flag = this._mapTrunks.Count <= 1;
			if (!flag)
			{
				this._curBranch.ExitNode.PrevVertex = null;
				this._curBranch.Nodes.Clear();
				this._curBranch.Nodes.Add(this._curBranch.FirstNode);
				this._mapTrunks.Pop();
				this._curMapTrunk = null;
				this._curBranch = null;
				this.SetCurMapTrunk(this._mapTrunks.Peek(), context);
				this._curBranch = this._branches[this._curMapTrunk.BranchIndex];
				this.SetPlayerPos(this._curBranch.EnterNode.AdjustedPos.GetHashCode(), context);
				this.ClearPathArrangement(context);
				this.SetCharacterToAdvanceBranch(context, -1);
			}
		}

		// Token: 0x06007F1C RID: 32540 RVA: 0x004C5618 File Offset: 0x004C3818
		private void Test_GenerateAdventures(DataContext context)
		{
			AdaptableLog.TagInfo("Test_GenerateAdventures", "Start testing adventure generation.");
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			foreach (AdventureItem configData in ((IEnumerable<AdventureItem>)Adventure.Instance))
			{
				bool flag = configData.TemplateId == 98;
				if (!flag)
				{
					for (int i = 0; i < 100; i++)
					{
						bool flag2 = !this.EnterAdventureByConfigData(context, configData, 0, 1);
						if (flag2)
						{
							throw new Exception("Unable to create adventure " + configData.Name);
						}
						for (int branchId = 0; branchId < this._branches.Count; branchId++)
						{
							AdvMapBranch branch = this._branches[branchId];
							foreach (AdvMapNodeNormal node in branch.Nodes)
							{
								AdventureMapPoint arg0 = node.ToAdventureMapPoint();
								bool flag3 = arg0.NodeContentType == 0 || arg0.NodeContentType == 3;
								if (flag3)
								{
									int eventIndex = arg0.NodeContentIndex;
									int branchIndex = (int)arg0.AffiliatedBranchIdx;
									AdventurePersonalityContentWeights[] branchContentWeights = (branchIndex < configData.BaseBranches.Count) ? configData.BaseBranches[branchIndex].PersonalityContentWeights : configData.AdvancedBranches[branchIndex - configData.BaseBranches.Count].PersonalityContentWeights;
									bool flag4 = !branchContentWeights.CheckIndex((int)arg0.SevenElementType);
									if (flag4)
									{
										defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(36, 2);
										defaultInterpolatedStringHandler.AppendLiteral("Invalid seven element type ");
										defaultInterpolatedStringHandler.AppendFormatted<sbyte>(arg0.SevenElementType);
										defaultInterpolatedStringHandler.AppendLiteral(" of node ");
										defaultInterpolatedStringHandler.AppendFormatted(arg0.GetDetailedInfo());
										throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
									}
									AdventurePersonalityContentWeights sevenElementWeights = branchContentWeights[(int)arg0.SevenElementType];
									sbyte nodeContentType = arg0.NodeContentType;
									if (!true)
									{
									}
									ValueTuple<string, short>[] array;
									if (nodeContentType != 0)
									{
										if (nodeContentType != 3)
										{
											defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(72, 1);
											defaultInterpolatedStringHandler.AppendLiteral("Incorrect node content type for EventTrigger_AdventureEnterNode: ");
											defaultInterpolatedStringHandler.AppendFormatted<sbyte>(arg0.NodeContentType);
											defaultInterpolatedStringHandler.AppendLiteral(" given.");
											throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
										}
										array = sevenElementWeights.BonusWeights;
									}
									else
									{
										array = sevenElementWeights.EventWeights;
									}
									if (!true)
									{
									}
									ValueTuple<string, short>[] eventWeights = array;
									bool flag5 = !eventWeights.CheckIndex(eventIndex);
									if (flag5)
									{
										defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(28, 2);
										defaultInterpolatedStringHandler.AppendLiteral("Invalid eventIndex ");
										defaultInterpolatedStringHandler.AppendFormatted<int>(eventIndex);
										defaultInterpolatedStringHandler.AppendLiteral(" of node ");
										defaultInterpolatedStringHandler.AppendFormatted(arg0.GetDetailedInfo());
										throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
									}
								}
							}
							bool flag6 = branch.AdvancedBranch == null;
							if (!flag6)
							{
								foreach (AdvMapNodeNormal node2 in branch.AdvancedBranch.Nodes)
								{
									AdventureMapPoint arg = node2.ToAdventureMapPoint();
									bool flag7 = arg.NodeContentType == 0 || arg.NodeContentType == 3;
									if (flag7)
									{
										int eventIndex2 = arg.NodeContentIndex;
										int branchIndex2 = (int)arg.AffiliatedBranchIdx;
										AdventurePersonalityContentWeights[] branchContentWeights2 = (branchIndex2 < configData.BaseBranches.Count) ? configData.BaseBranches[branchIndex2].PersonalityContentWeights : configData.AdvancedBranches[branchIndex2 - configData.BaseBranches.Count].PersonalityContentWeights;
										bool flag8 = !branchContentWeights2.CheckIndex((int)arg.SevenElementType);
										if (flag8)
										{
											defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(36, 2);
											defaultInterpolatedStringHandler.AppendLiteral("Invalid seven element type ");
											defaultInterpolatedStringHandler.AppendFormatted<sbyte>(arg.SevenElementType);
											defaultInterpolatedStringHandler.AppendLiteral(" of node ");
											defaultInterpolatedStringHandler.AppendFormatted(arg.GetDetailedInfo());
											throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
										}
										AdventurePersonalityContentWeights sevenElementWeights2 = branchContentWeights2[(int)arg.SevenElementType];
										sbyte nodeContentType2 = arg.NodeContentType;
										if (!true)
										{
										}
										ValueTuple<string, short>[] array;
										if (nodeContentType2 != 0)
										{
											if (nodeContentType2 != 3)
											{
												defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(72, 1);
												defaultInterpolatedStringHandler.AppendLiteral("Incorrect node content type for EventTrigger_AdventureEnterNode: ");
												defaultInterpolatedStringHandler.AppendFormatted<sbyte>(arg.NodeContentType);
												defaultInterpolatedStringHandler.AppendLiteral(" given.");
												throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
											}
											array = sevenElementWeights2.BonusWeights;
										}
										else
										{
											array = sevenElementWeights2.EventWeights;
										}
										if (!true)
										{
										}
										ValueTuple<string, short>[] eventWeights2 = array;
										bool flag9 = !eventWeights2.CheckIndex(eventIndex2);
										if (flag9)
										{
											defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(28, 2);
											defaultInterpolatedStringHandler.AppendLiteral("Invalid eventIndex ");
											defaultInterpolatedStringHandler.AppendFormatted<int>(eventIndex2);
											defaultInterpolatedStringHandler.AppendLiteral(" of node ");
											defaultInterpolatedStringHandler.AppendFormatted(arg.GetDetailedInfo());
											throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
										}
									}
								}
							}
						}
					}
				}
			}
			string tag = "Test_GenerateAdventures";
			defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(49, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Total of ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(100);
			defaultInterpolatedStringHandler.AppendLiteral(" iterations executed for each adventure.");
			AdaptableLog.TagInfo(tag, defaultInterpolatedStringHandler.ToStringAndClear());
		}

		// Token: 0x06007F1D RID: 32541 RVA: 0x004C5BA0 File Offset: 0x004C3DA0
		private static void Test_GetClosestNeighboringGradeWithValidItem()
		{
			AdventureDomain.Logger.Warn("Testing Sword: ");
			for (sbyte grade = 0; grade < 9; grade += 1)
			{
				sbyte closestGrade = ItemDomain.GetClosestNeighboringGradeWithValidItem<WeaponItem>(grade, Config.Weapon.Instance.ToList<WeaponItem>(), (ValueTuple<WeaponItem, sbyte> pair) => pair.Item1.Grade == pair.Item2 && 8 == pair.Item1.ItemSubType && pair.Item1.DropRate > 0);
				Logger logger = AdventureDomain.Logger;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(33, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Queried grade: ");
				defaultInterpolatedStringHandler.AppendFormatted<sbyte>(grade);
				defaultInterpolatedStringHandler.AppendLiteral(" \t Closest grade: ");
				defaultInterpolatedStringHandler.AppendFormatted<sbyte>(closestGrade);
				logger.Info(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			AdventureDomain.Logger.Warn("Testing Medicine: ");
			for (sbyte grade2 = 0; grade2 < 9; grade2 += 1)
			{
				sbyte closestGrade2 = ItemDomain.GetClosestNeighboringGradeWithValidItem<MedicineItem>(grade2, Config.Medicine.Instance.ToList<MedicineItem>(), (ValueTuple<MedicineItem, sbyte> pair) => pair.Item1.Grade == pair.Item2 && 800 == pair.Item1.ItemSubType && pair.Item1.DropRate > 0);
				Logger logger2 = AdventureDomain.Logger;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(33, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Queried grade: ");
				defaultInterpolatedStringHandler.AppendFormatted<sbyte>(grade2);
				defaultInterpolatedStringHandler.AppendLiteral(" \t Closest grade: ");
				defaultInterpolatedStringHandler.AppendFormatted<sbyte>(closestGrade2);
				logger2.Info(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			AdventureDomain.Logger.Warn("Testing WoodMaterial: ");
			for (sbyte grade3 = 0; grade3 < 9; grade3 += 1)
			{
				sbyte closestGrade3 = ItemDomain.GetClosestNeighboringGradeWithValidItem<MaterialItem>(grade3, Config.Material.Instance.ToList<MaterialItem>(), (ValueTuple<MaterialItem, sbyte> pair) => pair.Item1.Grade == pair.Item2 && 501 == pair.Item1.ItemSubType && pair.Item1.DropRate > 0);
				Logger logger3 = AdventureDomain.Logger;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(33, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Queried grade: ");
				defaultInterpolatedStringHandler.AppendFormatted<sbyte>(grade3);
				defaultInterpolatedStringHandler.AppendLiteral(" \t Closest grade: ");
				defaultInterpolatedStringHandler.AppendFormatted<sbyte>(closestGrade3);
				logger3.Info(defaultInterpolatedStringHandler.ToStringAndClear());
			}
		}

		// Token: 0x06007F1E RID: 32542 RVA: 0x004C5DA8 File Offset: 0x004C3FA8
		private static void TestLifeSkillRequirementDistribution(DataContext context)
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < 9; i++)
			{
				StringBuilder stringBuilder2 = stringBuilder;
				StringBuilder stringBuilder3 = stringBuilder2;
				StringBuilder.AppendInterpolatedStringHandler appendInterpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(3, 1, stringBuilder2);
				appendInterpolatedStringHandler.AppendLiteral("难度");
				appendInterpolatedStringHandler.AppendFormatted<int>(i + 1);
				appendInterpolatedStringHandler.AppendLiteral("\t");
				stringBuilder3.Append(ref appendInterpolatedStringHandler);
			}
			stringBuilder.AppendLine();
			for (int j = 0; j < 1000; j++)
			{
				for (int k = 0; k < 9; k++)
				{
					int baseVal = (int)AdvMapBranch.GetBaseSkillRequirement(context.Random);
					StringBuilder stringBuilder2 = stringBuilder;
					StringBuilder stringBuilder4 = stringBuilder2;
					StringBuilder.AppendInterpolatedStringHandler appendInterpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(1, 1, stringBuilder2);
					appendInterpolatedStringHandler.AppendFormatted<int>(baseVal * (k + 1));
					appendInterpolatedStringHandler.AppendLiteral("\t");
					stringBuilder4.Append(ref appendInterpolatedStringHandler);
				}
				stringBuilder.AppendLine();
			}
			File.WriteAllText("LifeSkillRequirementDist.txt", stringBuilder.ToString());
		}

		// Token: 0x06007F1F RID: 32543 RVA: 0x004C5EA0 File Offset: 0x004C40A0
		private static void Test_AdventureGeneration(DataContext context)
		{
			AdventureDomain.Logger.Info("Test - generate adventures in settlements...");
			for (EMapBlockSubType subType = EMapBlockSubType.TaiwuCun; subType < EMapBlockSubType.Farmland; subType++)
			{
				sbyte stateTemplateId = 0;
				sbyte filterType = -1;
				MapBlockData blockData = DomainManager.Adventure.GetRandomMapBlockDataByFilters(context, stateTemplateId, filterType, new List<short>
				{
					(short)subType
				});
				short adventureId = (short)context.Random.Next(0, Adventure.Instance.Count);
				DomainManager.Adventure.TryCreateAdventureSite(context, blockData.AreaId, blockData.BlockId, adventureId, MonthlyActionKey.Invalid);
			}
			sbyte stateTemplateId2 = 0;
			while ((int)stateTemplateId2 < MapState.Instance.Count)
			{
				for (int i = 0; i < 30; i++)
				{
					sbyte filterType2 = (sbyte)context.Random.Next(-1, 1);
					MapBlockData blockData2 = DomainManager.Adventure.GetRandomMapBlockDataByFilters(context, stateTemplateId2, filterType2, null);
					short adventureId2 = (short)context.Random.Next(0, Adventure.Instance.Count);
					bool flag = blockData2 != null;
					if (flag)
					{
						DomainManager.Adventure.TryCreateAdventureSite(context, blockData2.AreaId, blockData2.BlockId, adventureId2, MonthlyActionKey.Invalid);
					}
				}
				stateTemplateId2 += 1;
			}
		}

		// Token: 0x06007F20 RID: 32544 RVA: 0x004C5FD4 File Offset: 0x004C41D4
		public AdventureDomain() : base(25)
		{
			this._playerPos = 0;
			this._personalities = new int[7];
			this._personalitiesCost = new int[7];
			this._personalitiesGain = new int[7];
			this._indicatePath = new List<int>();
			this._arrangeableNodes = new List<int>();
			this._arrangedNodes = new List<int>();
			this._perceivedNodes = new List<int>();
			this._errorInfo = string.Empty;
			this._adBranchOpenRecord = new Dictionary<int, int>(0);
			this._curAdventureId = 0;
			this._operationBlock = false;
			this._adventureState = 0;
			this._curMapTrunk = new AdventureMapTrunk();
			this._advParameters = new List<int>();
			this._enterItems = new List<ItemKey>();
			this._allowExitAdventure = false;
			this._adventureAreas = new AreaAdventureData[139];
			this._enemyNestSites = new Dictionary<Location, EnemyNestSiteExtraData>(0);
			this._brokenAreaEnemies = new BrokenAreaData[90];
			this._spentCharList = new List<int>();
			this._curBranchChosenChar = 0;
			this._escapingRandomEnemies = new List<short>();
			this._teamDetectedNodes = new List<int>();
			this._actionPointWithhold = 0;
			this.OnInitializedDomainData();
		}

		// Token: 0x06007F21 RID: 32545 RVA: 0x004C61D4 File Offset: 0x004C43D4
		public int GetPlayerPos()
		{
			return this._playerPos;
		}

		// Token: 0x06007F22 RID: 32546 RVA: 0x004C61EC File Offset: 0x004C43EC
		public void SetPlayerPos(int value, DataContext context)
		{
			this._playerPos = value;
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(0, this.DataStates, AdventureDomain.CacheInfluences, context);
		}

		// Token: 0x06007F23 RID: 32547 RVA: 0x004C620C File Offset: 0x004C440C
		public int[] GetPersonalities()
		{
			return this._personalities;
		}

		// Token: 0x06007F24 RID: 32548 RVA: 0x004C6224 File Offset: 0x004C4424
		public void SetPersonalities(int[] value, DataContext context)
		{
			this._personalities = value;
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(1, this.DataStates, AdventureDomain.CacheInfluences, context);
		}

		// Token: 0x06007F25 RID: 32549 RVA: 0x004C6244 File Offset: 0x004C4444
		public int[] GetPersonalitiesCost()
		{
			Thread.MemoryBarrier();
			bool flag = BaseGameDataDomain.IsCached(this.DataStates, 2);
			int[] personalitiesCost;
			if (flag)
			{
				personalitiesCost = this._personalitiesCost;
			}
			else
			{
				int[] value = new int[7];
				this.CalcPersonalitiesCost(value);
				bool lockTaken = false;
				try
				{
					this._spinLockPersonalitiesCost.Enter(ref lockTaken);
					for (int i = 0; i < 7; i++)
					{
						this._personalitiesCost[i] = value[i];
					}
					BaseGameDataDomain.SetCached(this.DataStates, 2);
				}
				finally
				{
					bool flag2 = lockTaken;
					if (flag2)
					{
						this._spinLockPersonalitiesCost.Exit(false);
					}
				}
				Thread.MemoryBarrier();
				personalitiesCost = this._personalitiesCost;
			}
			return personalitiesCost;
		}

		// Token: 0x06007F26 RID: 32550 RVA: 0x004C62FC File Offset: 0x004C44FC
		public int[] GetPersonalitiesGain()
		{
			Thread.MemoryBarrier();
			bool flag = BaseGameDataDomain.IsCached(this.DataStates, 3);
			int[] personalitiesGain;
			if (flag)
			{
				personalitiesGain = this._personalitiesGain;
			}
			else
			{
				int[] value = new int[7];
				this.CalcPersonalitiesGain(value);
				bool lockTaken = false;
				try
				{
					this._spinLockPersonalitiesGain.Enter(ref lockTaken);
					for (int i = 0; i < 7; i++)
					{
						this._personalitiesGain[i] = value[i];
					}
					BaseGameDataDomain.SetCached(this.DataStates, 3);
				}
				finally
				{
					bool flag2 = lockTaken;
					if (flag2)
					{
						this._spinLockPersonalitiesGain.Exit(false);
					}
				}
				Thread.MemoryBarrier();
				personalitiesGain = this._personalitiesGain;
			}
			return personalitiesGain;
		}

		// Token: 0x06007F27 RID: 32551 RVA: 0x004C63B4 File Offset: 0x004C45B4
		public List<int> GetIndicatePath()
		{
			return this._indicatePath;
		}

		// Token: 0x06007F28 RID: 32552 RVA: 0x004C63CC File Offset: 0x004C45CC
		public void SetIndicatePath(List<int> value, DataContext context)
		{
			this._indicatePath = value;
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(4, this.DataStates, AdventureDomain.CacheInfluences, context);
		}

		// Token: 0x06007F29 RID: 32553 RVA: 0x004C63EC File Offset: 0x004C45EC
		public List<int> GetArrangeableNodes()
		{
			Thread.MemoryBarrier();
			bool flag = BaseGameDataDomain.IsCached(this.DataStates, 5);
			List<int> arrangeableNodes;
			if (flag)
			{
				arrangeableNodes = this._arrangeableNodes;
			}
			else
			{
				List<int> value = new List<int>();
				this.CalcArrangeableNodes(value);
				bool lockTaken = false;
				try
				{
					this._spinLockArrangeableNodes.Enter(ref lockTaken);
					this._arrangeableNodes.Assign(value);
					BaseGameDataDomain.SetCached(this.DataStates, 5);
				}
				finally
				{
					bool flag2 = lockTaken;
					if (flag2)
					{
						this._spinLockArrangeableNodes.Exit(false);
					}
				}
				Thread.MemoryBarrier();
				arrangeableNodes = this._arrangeableNodes;
			}
			return arrangeableNodes;
		}

		// Token: 0x06007F2A RID: 32554 RVA: 0x004C648C File Offset: 0x004C468C
		public List<int> GetArrangedNodes()
		{
			return this._arrangedNodes;
		}

		// Token: 0x06007F2B RID: 32555 RVA: 0x004C64A4 File Offset: 0x004C46A4
		public void SetArrangedNodes(List<int> value, DataContext context)
		{
			this._arrangedNodes = value;
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(6, this.DataStates, AdventureDomain.CacheInfluences, context);
		}

		// Token: 0x06007F2C RID: 32556 RVA: 0x004C64C4 File Offset: 0x004C46C4
		public List<int> GetPerceivedNodes()
		{
			return this._perceivedNodes;
		}

		// Token: 0x06007F2D RID: 32557 RVA: 0x004C64DC File Offset: 0x004C46DC
		public void SetPerceivedNodes(List<int> value, DataContext context)
		{
			this._perceivedNodes = value;
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(7, this.DataStates, AdventureDomain.CacheInfluences, context);
		}

		// Token: 0x06007F2E RID: 32558 RVA: 0x004C64FC File Offset: 0x004C46FC
		public string GetErrorInfo()
		{
			return this._errorInfo;
		}

		// Token: 0x06007F2F RID: 32559 RVA: 0x004C6514 File Offset: 0x004C4714
		public void SetErrorInfo(string value, DataContext context)
		{
			this._errorInfo = value;
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(8, this.DataStates, AdventureDomain.CacheInfluences, context);
		}

		// Token: 0x06007F30 RID: 32560 RVA: 0x004C6534 File Offset: 0x004C4734
		private int GetElement_AdBranchOpenRecord(int elementId)
		{
			return this._adBranchOpenRecord[elementId];
		}

		// Token: 0x06007F31 RID: 32561 RVA: 0x004C6554 File Offset: 0x004C4754
		private bool TryGetElement_AdBranchOpenRecord(int elementId, out int value)
		{
			return this._adBranchOpenRecord.TryGetValue(elementId, out value);
		}

		// Token: 0x06007F32 RID: 32562 RVA: 0x004C6573 File Offset: 0x004C4773
		private void AddElement_AdBranchOpenRecord(int elementId, int value, DataContext context)
		{
			this._adBranchOpenRecord.Add(elementId, value);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(9, this.DataStates, AdventureDomain.CacheInfluences, context);
		}

		// Token: 0x06007F33 RID: 32563 RVA: 0x004C6598 File Offset: 0x004C4798
		private void SetElement_AdBranchOpenRecord(int elementId, int value, DataContext context)
		{
			this._adBranchOpenRecord[elementId] = value;
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(9, this.DataStates, AdventureDomain.CacheInfluences, context);
		}

		// Token: 0x06007F34 RID: 32564 RVA: 0x004C65BD File Offset: 0x004C47BD
		private void RemoveElement_AdBranchOpenRecord(int elementId, DataContext context)
		{
			this._adBranchOpenRecord.Remove(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(9, this.DataStates, AdventureDomain.CacheInfluences, context);
		}

		// Token: 0x06007F35 RID: 32565 RVA: 0x004C65E1 File Offset: 0x004C47E1
		private void ClearAdBranchOpenRecord(DataContext context)
		{
			this._adBranchOpenRecord.Clear();
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(9, this.DataStates, AdventureDomain.CacheInfluences, context);
		}

		// Token: 0x06007F36 RID: 32566 RVA: 0x004C6604 File Offset: 0x004C4804
		public short GetCurAdventureId()
		{
			return this._curAdventureId;
		}

		// Token: 0x06007F37 RID: 32567 RVA: 0x004C661C File Offset: 0x004C481C
		public void SetCurAdventureId(short value, DataContext context)
		{
			this._curAdventureId = value;
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(10, this.DataStates, AdventureDomain.CacheInfluences, context);
		}

		// Token: 0x06007F38 RID: 32568 RVA: 0x004C663C File Offset: 0x004C483C
		public bool GetOperationBlock()
		{
			return this._operationBlock;
		}

		// Token: 0x06007F39 RID: 32569 RVA: 0x004C6654 File Offset: 0x004C4854
		public void SetOperationBlock(bool value, DataContext context)
		{
			this._operationBlock = value;
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(11, this.DataStates, AdventureDomain.CacheInfluences, context);
		}

		// Token: 0x06007F3A RID: 32570 RVA: 0x004C6674 File Offset: 0x004C4874
		public sbyte GetAdventureState()
		{
			return this._adventureState;
		}

		// Token: 0x06007F3B RID: 32571 RVA: 0x004C668C File Offset: 0x004C488C
		public void SetAdventureState(sbyte value, DataContext context)
		{
			this._adventureState = value;
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(12, this.DataStates, AdventureDomain.CacheInfluences, context);
		}

		// Token: 0x06007F3C RID: 32572 RVA: 0x004C66AC File Offset: 0x004C48AC
		public AdventureMapTrunk GetCurMapTrunk()
		{
			return this._curMapTrunk;
		}

		// Token: 0x06007F3D RID: 32573 RVA: 0x004C66C4 File Offset: 0x004C48C4
		public void SetCurMapTrunk(AdventureMapTrunk value, DataContext context)
		{
			this._curMapTrunk = value;
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(13, this.DataStates, AdventureDomain.CacheInfluences, context);
		}

		// Token: 0x06007F3E RID: 32574 RVA: 0x004C66E4 File Offset: 0x004C48E4
		public List<int> GetAdvParameters()
		{
			return this._advParameters;
		}

		// Token: 0x06007F3F RID: 32575 RVA: 0x004C66FC File Offset: 0x004C48FC
		public void SetAdvParameters(List<int> value, DataContext context)
		{
			this._advParameters = value;
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(14, this.DataStates, AdventureDomain.CacheInfluences, context);
		}

		// Token: 0x06007F40 RID: 32576 RVA: 0x004C671C File Offset: 0x004C491C
		public List<ItemKey> GetEnterItems()
		{
			return this._enterItems;
		}

		// Token: 0x06007F41 RID: 32577 RVA: 0x004C6734 File Offset: 0x004C4934
		private void SetEnterItems(List<ItemKey> value, DataContext context)
		{
			this._enterItems = value;
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(15, this.DataStates, AdventureDomain.CacheInfluences, context);
		}

		// Token: 0x06007F42 RID: 32578 RVA: 0x004C6754 File Offset: 0x004C4954
		public bool GetAllowExitAdventure()
		{
			return this._allowExitAdventure;
		}

		// Token: 0x06007F43 RID: 32579 RVA: 0x004C676C File Offset: 0x004C496C
		public void SetAllowExitAdventure(bool value, DataContext context)
		{
			this._allowExitAdventure = value;
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(16, this.DataStates, AdventureDomain.CacheInfluences, context);
		}

		// Token: 0x06007F44 RID: 32580 RVA: 0x004C678C File Offset: 0x004C498C
		public AreaAdventureData GetElement_AdventureAreas(int index)
		{
			return this._adventureAreas[index];
		}

		// Token: 0x06007F45 RID: 32581 RVA: 0x004C67A8 File Offset: 0x004C49A8
		private unsafe void SetElement_AdventureAreas(int index, AreaAdventureData value, DataContext context)
		{
			this._adventureAreas[index] = value;
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(index, this._dataStatesAdventureAreas, AdventureDomain.CacheInfluencesAdventureAreas, context);
			bool flag = value != null;
			if (flag)
			{
				int dataSize = value.GetSerializedSize();
				byte* pData = OperationAdder.DynamicElementList_Set(10, 17, index, dataSize);
				pData += value.Serialize(pData);
			}
			else
			{
				OperationAdder.DynamicElementList_Set(10, 17, index, 0);
			}
		}

		// Token: 0x06007F46 RID: 32582 RVA: 0x004C6808 File Offset: 0x004C4A08
		public EnemyNestSiteExtraData GetElement_EnemyNestSites(Location elementId)
		{
			return this._enemyNestSites[elementId];
		}

		// Token: 0x06007F47 RID: 32583 RVA: 0x004C6828 File Offset: 0x004C4A28
		public bool TryGetElement_EnemyNestSites(Location elementId, out EnemyNestSiteExtraData value)
		{
			return this._enemyNestSites.TryGetValue(elementId, out value);
		}

		// Token: 0x06007F48 RID: 32584 RVA: 0x004C6848 File Offset: 0x004C4A48
		private unsafe void AddElement_EnemyNestSites(Location elementId, EnemyNestSiteExtraData value, DataContext context)
		{
			this._enemyNestSites.Add(elementId, value);
			this._modificationsEnemyNestSites.RecordAdding(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(18, this.DataStates, AdventureDomain.CacheInfluences, context);
			bool flag = value != null;
			if (flag)
			{
				int contentSize = value.GetSerializedSize();
				byte* pData = OperationAdder.DynamicSingleValueCollection_Add<Location>(10, 18, elementId, contentSize);
				pData += value.Serialize(pData);
			}
			else
			{
				OperationAdder.DynamicSingleValueCollection_Add<Location>(10, 18, elementId, 0);
			}
		}

		// Token: 0x06007F49 RID: 32585 RVA: 0x004C68BC File Offset: 0x004C4ABC
		private unsafe void SetElement_EnemyNestSites(Location elementId, EnemyNestSiteExtraData value, DataContext context)
		{
			this._enemyNestSites[elementId] = value;
			this._modificationsEnemyNestSites.RecordSetting(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(18, this.DataStates, AdventureDomain.CacheInfluences, context);
			bool flag = value != null;
			if (flag)
			{
				int contentSize = value.GetSerializedSize();
				byte* pData = OperationAdder.DynamicSingleValueCollection_Set<Location>(10, 18, elementId, contentSize);
				pData += value.Serialize(pData);
			}
			else
			{
				OperationAdder.DynamicSingleValueCollection_Set<Location>(10, 18, elementId, 0);
			}
		}

		// Token: 0x06007F4A RID: 32586 RVA: 0x004C692E File Offset: 0x004C4B2E
		private void RemoveElement_EnemyNestSites(Location elementId, DataContext context)
		{
			this._enemyNestSites.Remove(elementId);
			this._modificationsEnemyNestSites.RecordRemoving(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(18, this.DataStates, AdventureDomain.CacheInfluences, context);
			OperationAdder.DynamicSingleValueCollection_Remove<Location>(10, 18, elementId);
		}

		// Token: 0x06007F4B RID: 32587 RVA: 0x004C696A File Offset: 0x004C4B6A
		private void ClearEnemyNestSites(DataContext context)
		{
			this._enemyNestSites.Clear();
			this._modificationsEnemyNestSites.RecordClearing();
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(18, this.DataStates, AdventureDomain.CacheInfluences, context);
			OperationAdder.DynamicSingleValueCollection_Clear(10, 18);
		}

		// Token: 0x06007F4C RID: 32588 RVA: 0x004C69A4 File Offset: 0x004C4BA4
		public BrokenAreaData GetElement_BrokenAreaEnemies(int index)
		{
			return this._brokenAreaEnemies[index];
		}

		// Token: 0x06007F4D RID: 32589 RVA: 0x004C69C0 File Offset: 0x004C4BC0
		private unsafe void SetElement_BrokenAreaEnemies(int index, BrokenAreaData value, DataContext context)
		{
			this._brokenAreaEnemies[index] = value;
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(index, this._dataStatesBrokenAreaEnemies, AdventureDomain.CacheInfluencesBrokenAreaEnemies, context);
			bool flag = value != null;
			if (flag)
			{
				int dataSize = value.GetSerializedSize();
				byte* pData = OperationAdder.DynamicElementList_Set(10, 19, index, dataSize);
				pData += value.Serialize(pData);
			}
			else
			{
				OperationAdder.DynamicElementList_Set(10, 19, index, 0);
			}
		}

		// Token: 0x06007F4E RID: 32590 RVA: 0x004C6A20 File Offset: 0x004C4C20
		public List<int> GetSpentCharList()
		{
			return this._spentCharList;
		}

		// Token: 0x06007F4F RID: 32591 RVA: 0x004C6A38 File Offset: 0x004C4C38
		public void SetSpentCharList(List<int> value, DataContext context)
		{
			this._spentCharList = value;
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(20, this.DataStates, AdventureDomain.CacheInfluences, context);
		}

		// Token: 0x06007F50 RID: 32592 RVA: 0x004C6A58 File Offset: 0x004C4C58
		public int GetCurBranchChosenChar()
		{
			return this._curBranchChosenChar;
		}

		// Token: 0x06007F51 RID: 32593 RVA: 0x004C6A70 File Offset: 0x004C4C70
		public void SetCurBranchChosenChar(int value, DataContext context)
		{
			this._curBranchChosenChar = value;
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(21, this.DataStates, AdventureDomain.CacheInfluences, context);
		}

		// Token: 0x06007F52 RID: 32594 RVA: 0x004C6A90 File Offset: 0x004C4C90
		public List<short> GetEscapingRandomEnemies()
		{
			return this._escapingRandomEnemies;
		}

		// Token: 0x06007F53 RID: 32595 RVA: 0x004C6AA8 File Offset: 0x004C4CA8
		public unsafe void SetEscapingRandomEnemies(List<short> value, DataContext context)
		{
			this._escapingRandomEnemies = value;
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(22, this.DataStates, AdventureDomain.CacheInfluences, context);
			int elementsCount = this._escapingRandomEnemies.Count;
			int contentSize = 2 * elementsCount;
			int dataSize = 2 + contentSize;
			byte* pData = OperationAdder.DynamicSingleValue_Set(10, 22, dataSize);
			*(short*)pData = (short)((ushort)elementsCount);
			pData += 2;
			for (int i = 0; i < elementsCount; i++)
			{
				*(short*)(pData + (IntPtr)i * 2) = this._escapingRandomEnemies[i];
			}
			pData += contentSize;
		}

		// Token: 0x06007F54 RID: 32596 RVA: 0x004C6B28 File Offset: 0x004C4D28
		public List<int> GetTeamDetectedNodes()
		{
			return this._teamDetectedNodes;
		}

		// Token: 0x06007F55 RID: 32597 RVA: 0x004C6B40 File Offset: 0x004C4D40
		public void SetTeamDetectedNodes(List<int> value, DataContext context)
		{
			this._teamDetectedNodes = value;
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(23, this.DataStates, AdventureDomain.CacheInfluences, context);
		}

		// Token: 0x06007F56 RID: 32598 RVA: 0x004C6B60 File Offset: 0x004C4D60
		public int GetActionPointWithhold()
		{
			return this._actionPointWithhold;
		}

		// Token: 0x06007F57 RID: 32599 RVA: 0x004C6B78 File Offset: 0x004C4D78
		public void SetActionPointWithhold(int value, DataContext context)
		{
			this._actionPointWithhold = value;
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(24, this.DataStates, AdventureDomain.CacheInfluences, context);
		}

		// Token: 0x06007F58 RID: 32600 RVA: 0x004C6B96 File Offset: 0x004C4D96
		public override void OnInitializeGameDataModule()
		{
			this.InitializeOnInitializeGameDataModule();
		}

		// Token: 0x06007F59 RID: 32601 RVA: 0x004C6BA0 File Offset: 0x004C4DA0
		public unsafe override void OnEnterNewWorld()
		{
			this.InitializeOnEnterNewWorld();
			this.InitializeInternalDataOfCollections();
			int dataSize = 0;
			for (int i = 0; i < 139; i++)
			{
				AreaAdventureData element = this._adventureAreas[i];
				bool flag = element != null;
				if (flag)
				{
					dataSize += 4 + element.GetSerializedSize();
				}
				else
				{
					dataSize += 4;
				}
			}
			byte* pData = OperationAdder.DynamicElementList_InsertRange(10, 17, 0, 139, dataSize);
			for (int j = 0; j < 139; j++)
			{
				AreaAdventureData element2 = this._adventureAreas[j];
				bool flag2 = element2 != null;
				if (flag2)
				{
					byte* pSubContentSize = pData;
					pData += 4;
					int subContentSize = element2.Serialize(pData);
					pData += subContentSize;
					*(int*)pSubContentSize = subContentSize;
				}
				else
				{
					*(int*)pData = 0;
					pData += 4;
				}
			}
			foreach (KeyValuePair<Location, EnemyNestSiteExtraData> entry in this._enemyNestSites)
			{
				Location elementId = entry.Key;
				EnemyNestSiteExtraData value = entry.Value;
				bool flag3 = value != null;
				if (flag3)
				{
					int contentSize = value.GetSerializedSize();
					byte* pData2 = OperationAdder.DynamicSingleValueCollection_Add<Location>(10, 18, elementId, contentSize);
					pData2 += value.Serialize(pData2);
				}
				else
				{
					OperationAdder.DynamicSingleValueCollection_Add<Location>(10, 18, elementId, 0);
				}
			}
			int dataSize2 = 0;
			for (int k = 0; k < 90; k++)
			{
				BrokenAreaData element3 = this._brokenAreaEnemies[k];
				bool flag4 = element3 != null;
				if (flag4)
				{
					dataSize2 += 4 + element3.GetSerializedSize();
				}
				else
				{
					dataSize2 += 4;
				}
			}
			byte* pData3 = OperationAdder.DynamicElementList_InsertRange(10, 19, 0, 90, dataSize2);
			for (int l = 0; l < 90; l++)
			{
				BrokenAreaData element4 = this._brokenAreaEnemies[l];
				bool flag5 = element4 != null;
				if (flag5)
				{
					byte* pSubContentSize2 = pData3;
					pData3 += 4;
					int subContentSize2 = element4.Serialize(pData3);
					pData3 += subContentSize2;
					*(int*)pSubContentSize2 = subContentSize2;
				}
				else
				{
					*(int*)pData3 = 0;
					pData3 += 4;
				}
			}
			int elementsCount = this._escapingRandomEnemies.Count;
			int contentSize2 = 2 * elementsCount;
			int dataSize3 = 2 + contentSize2;
			byte* pData4 = OperationAdder.DynamicSingleValue_Set(10, 22, dataSize3);
			*(short*)pData4 = (short)((ushort)elementsCount);
			pData4 += 2;
			for (int m = 0; m < elementsCount; m++)
			{
				*(short*)(pData4 + (IntPtr)m * 2) = this._escapingRandomEnemies[m];
			}
			pData4 += contentSize2;
		}

		// Token: 0x06007F5A RID: 32602 RVA: 0x004C6E24 File Offset: 0x004C5024
		public override void OnLoadWorld()
		{
			this._pendingLoadingOperationIds = new Queue<uint>();
			this._pendingLoadingOperationIds.Enqueue(OperationAdder.DynamicElementList_GetAll(10, 17));
			this._pendingLoadingOperationIds.Enqueue(OperationAdder.DynamicSingleValueCollection_GetAll(10, 18));
			this._pendingLoadingOperationIds.Enqueue(OperationAdder.DynamicElementList_GetAll(10, 19));
			this._pendingLoadingOperationIds.Enqueue(OperationAdder.DynamicSingleValue_Get(10, 22));
		}

		// Token: 0x06007F5B RID: 32603 RVA: 0x004C6E94 File Offset: 0x004C5094
		public override int GetData(ushort dataId, ulong subId0, uint subId1, RawDataPool dataPool, bool resetModified)
		{
			int result;
			switch (dataId)
			{
			case 0:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 0);
				}
				result = Serializer.Serialize(this._playerPos, dataPool);
				break;
			case 1:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 1);
				}
				result = Serializer.Serialize(this._personalities, dataPool);
				break;
			case 2:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 2);
				}
				result = Serializer.Serialize(this.GetPersonalitiesCost(), dataPool);
				break;
			case 3:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 3);
				}
				result = Serializer.Serialize(this.GetPersonalitiesGain(), dataPool);
				break;
			case 4:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 4);
				}
				result = Serializer.Serialize(this._indicatePath, dataPool);
				break;
			case 5:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 5);
				}
				result = Serializer.Serialize(this.GetArrangeableNodes(), dataPool);
				break;
			case 6:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 6);
				}
				result = Serializer.Serialize(this._arrangedNodes, dataPool);
				break;
			case 7:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 7);
				}
				result = Serializer.Serialize(this._perceivedNodes, dataPool);
				break;
			case 8:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 8);
				}
				result = Serializer.Serialize(this._errorInfo, dataPool);
				break;
			case 9:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(34, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to get value of dataId: ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 10:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 10);
				}
				result = Serializer.Serialize(this._curAdventureId, dataPool);
				break;
			case 11:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 11);
				}
				result = Serializer.Serialize(this._operationBlock, dataPool);
				break;
			case 12:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 12);
				}
				result = Serializer.Serialize(this._adventureState, dataPool);
				break;
			case 13:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 13);
				}
				result = Serializer.Serialize(this._curMapTrunk, dataPool);
				break;
			case 14:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 14);
				}
				result = Serializer.Serialize(this._advParameters, dataPool);
				break;
			case 15:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 15);
				}
				result = Serializer.Serialize(this._enterItems, dataPool);
				break;
			case 16:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 16);
				}
				result = Serializer.Serialize(this._allowExitAdventure, dataPool);
				break;
			case 17:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this._dataStatesAdventureAreas, (int)subId0);
				}
				result = Serializer.Serialize(this._adventureAreas[(int)subId0], dataPool);
				break;
			case 18:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 18);
					this._modificationsEnemyNestSites.Reset();
				}
				result = Serializer.SerializeModifications<Location>(this._enemyNestSites, dataPool);
				break;
			case 19:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this._dataStatesBrokenAreaEnemies, (int)subId0);
				}
				result = Serializer.Serialize(this._brokenAreaEnemies[(int)subId0], dataPool);
				break;
			case 20:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 20);
				}
				result = Serializer.Serialize(this._spentCharList, dataPool);
				break;
			case 21:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 21);
				}
				result = Serializer.Serialize(this._curBranchChosenChar, dataPool);
				break;
			case 22:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 22);
				}
				result = Serializer.Serialize(this._escapingRandomEnemies, dataPool);
				break;
			case 23:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 23);
				}
				result = Serializer.Serialize(this._teamDetectedNodes, dataPool);
				break;
			case 24:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 24);
				}
				result = Serializer.Serialize(this._actionPointWithhold, dataPool);
				break;
			default:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(19, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Unsupported dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			}
			return result;
		}

		// Token: 0x06007F5C RID: 32604 RVA: 0x004C7348 File Offset: 0x004C5548
		public override void SetData(ushort dataId, ulong subId0, uint subId1, int valueOffset, RawDataPool dataPool, DataContext context)
		{
			switch (dataId)
			{
			case 0:
				Serializer.Deserialize(dataPool, valueOffset, ref this._playerPos);
				this.SetPlayerPos(this._playerPos, context);
				break;
			case 1:
				Serializer.Deserialize(dataPool, valueOffset, ref this._personalities);
				this.SetPersonalities(this._personalities, context);
				break;
			case 2:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(33, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to set value of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 3:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(33, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to set value of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 4:
				Serializer.Deserialize(dataPool, valueOffset, ref this._indicatePath);
				this.SetIndicatePath(this._indicatePath, context);
				break;
			case 5:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(33, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to set value of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 6:
				Serializer.Deserialize(dataPool, valueOffset, ref this._arrangedNodes);
				this.SetArrangedNodes(this._arrangedNodes, context);
				break;
			case 7:
				Serializer.Deserialize(dataPool, valueOffset, ref this._perceivedNodes);
				this.SetPerceivedNodes(this._perceivedNodes, context);
				break;
			case 8:
				Serializer.Deserialize(dataPool, valueOffset, ref this._errorInfo);
				this.SetErrorInfo(this._errorInfo, context);
				break;
			case 9:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(33, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to set value of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 10:
				Serializer.Deserialize(dataPool, valueOffset, ref this._curAdventureId);
				this.SetCurAdventureId(this._curAdventureId, context);
				break;
			case 11:
				Serializer.Deserialize(dataPool, valueOffset, ref this._operationBlock);
				this.SetOperationBlock(this._operationBlock, context);
				break;
			case 12:
				Serializer.Deserialize(dataPool, valueOffset, ref this._adventureState);
				this.SetAdventureState(this._adventureState, context);
				break;
			case 13:
				Serializer.Deserialize(dataPool, valueOffset, ref this._curMapTrunk);
				this.SetCurMapTrunk(this._curMapTrunk, context);
				break;
			case 14:
				Serializer.Deserialize(dataPool, valueOffset, ref this._advParameters);
				this.SetAdvParameters(this._advParameters, context);
				break;
			case 15:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(33, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to set value of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 16:
				Serializer.Deserialize(dataPool, valueOffset, ref this._allowExitAdventure);
				this.SetAllowExitAdventure(this._allowExitAdventure, context);
				break;
			case 17:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(33, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to set value of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 18:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(33, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to set value of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 19:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(33, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to set value of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 20:
				Serializer.Deserialize(dataPool, valueOffset, ref this._spentCharList);
				this.SetSpentCharList(this._spentCharList, context);
				break;
			case 21:
				Serializer.Deserialize(dataPool, valueOffset, ref this._curBranchChosenChar);
				this.SetCurBranchChosenChar(this._curBranchChosenChar, context);
				break;
			case 22:
				Serializer.Deserialize(dataPool, valueOffset, ref this._escapingRandomEnemies);
				this.SetEscapingRandomEnemies(this._escapingRandomEnemies, context);
				break;
			case 23:
				Serializer.Deserialize(dataPool, valueOffset, ref this._teamDetectedNodes);
				this.SetTeamDetectedNodes(this._teamDetectedNodes, context);
				break;
			case 24:
				Serializer.Deserialize(dataPool, valueOffset, ref this._actionPointWithhold);
				this.SetActionPointWithhold(this._actionPointWithhold, context);
				break;
			default:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(19, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Unsupported dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			}
		}

		// Token: 0x06007F5D RID: 32605 RVA: 0x004C77CC File Offset: 0x004C59CC
		public override int CallMethod(Operation operation, RawDataPool argDataPool, RawDataPool returnDataPool, DataContext context)
		{
			int argsOffset = operation.ArgsOffset;
			int result;
			switch (operation.MethodId)
			{
			case 0:
			{
				int argsCount = operation.ArgsCount;
				int num = argsCount;
				if (num != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				short adventureId = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref adventureId);
				Location returnValue = this.GmCmd_GenerateAdventure(context, adventureId);
				result = Serializer.Serialize(returnValue, returnDataPool);
				break;
			}
			case 1:
			{
				int argsCount2 = operation.ArgsCount;
				int num2 = argsCount2;
				if (num2 != 2)
				{
					if (num2 != 3)
					{
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
						defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
						defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
						throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
					}
					AdventureItem configData = null;
					argsOffset += Serializer.DeserializeDefault<AdventureItem>(argDataPool, argsOffset, ref configData);
					int startNodeIndex = 0;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref startNodeIndex);
					int enterTerrain = 0;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref enterTerrain);
					bool returnValue2 = this.EnterAdventureByConfigData(context, configData, startNodeIndex, enterTerrain);
					result = Serializer.Serialize(returnValue2, returnDataPool);
				}
				else
				{
					AdventureItem configData2 = null;
					argsOffset += Serializer.DeserializeDefault<AdventureItem>(argDataPool, argsOffset, ref configData2);
					int startNodeIndex2 = 0;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref startNodeIndex2);
					bool returnValue3 = this.EnterAdventureByConfigData(context, configData2, startNodeIndex2, 1);
					result = Serializer.Serialize(returnValue3, returnDataPool);
				}
				break;
			}
			case 2:
			{
				int argsCount3 = operation.ArgsCount;
				int num3 = argsCount3;
				if (num3 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				sbyte nodeBranchIndex = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref nodeBranchIndex);
				this.SelectBranchForPreview(context, nodeBranchIndex);
				result = -1;
				break;
			}
			case 3:
			{
				int argsCount4 = operation.ArgsCount;
				int num4 = argsCount4;
				if (num4 != 0)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				this.ClearBranchForPreview(context);
				result = -1;
				break;
			}
			case 4:
			{
				int argsCount5 = operation.ArgsCount;
				int num5 = argsCount5;
				if (num5 != 2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				short adventureId2 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref adventureId2);
				List<ItemKey> itemKeys = null;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref itemKeys);
				bool returnValue4 = this.EnterAdventure(context, adventureId2, itemKeys);
				result = Serializer.Serialize(returnValue4, returnDataPool);
				break;
			}
			case 5:
			{
				int argsCount6 = operation.ArgsCount;
				int num6 = argsCount6;
				if (num6 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				int pos = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref pos);
				this.ArrangeNode(context, pos);
				result = -1;
				break;
			}
			case 6:
			{
				int argsCount7 = operation.ArgsCount;
				int num7 = argsCount7;
				if (num7 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				int pos2 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref pos2);
				this.PerceiveNode(context, pos2);
				result = -1;
				break;
			}
			case 7:
			{
				int argsCount8 = operation.ArgsCount;
				int num8 = argsCount8;
				if (num8 != 0)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				this.ConfirmPath(context);
				result = -1;
				break;
			}
			case 8:
			{
				int argsCount9 = operation.ArgsCount;
				int num9 = argsCount9;
				if (num9 != 0)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				ValueTuple<int, int> returnValue5 = this.ConfirmArrived(context);
				result = Serializer.Serialize(returnValue5, returnDataPool);
				break;
			}
			case 9:
			{
				int argsCount10 = operation.ArgsCount;
				int num10 = argsCount10;
				if (num10 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				bool isAdventureCompleted = false;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref isAdventureCompleted);
				this.ExitAdventure(context, isAdventureCompleted);
				result = -1;
				break;
			}
			case 10:
			{
				int argsCount11 = operation.ArgsCount;
				int num11 = argsCount11;
				if (num11 != 0)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				this.SwitchBranch(context);
				result = -1;
				break;
			}
			case 11:
			{
				int argsCount12 = operation.ArgsCount;
				int num12 = argsCount12;
				if (num12 != 0)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				this.ClearPathArrangement(context);
				result = -1;
				break;
			}
			case 12:
			{
				int argsCount13 = operation.ArgsCount;
				int num13 = argsCount13;
				if (num13 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				short areaId = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref areaId);
				AreaAdventureData returnValue6 = this.GetAdventuresInArea(areaId);
				result = Serializer.Serialize(returnValue6, returnDataPool);
				break;
			}
			case 13:
			{
				int argsCount14 = operation.ArgsCount;
				int num14 = argsCount14;
				if (num14 != 0)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				List<short> returnValue7 = this.GetAwakeSwordTombs();
				result = Serializer.Serialize(returnValue7, returnDataPool);
				break;
			}
			case 14:
			{
				int argsCount15 = operation.ArgsCount;
				int num15 = argsCount15;
				if (num15 != 0)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				List<short> returnValue8 = this.GetAttackingSwordTombs();
				result = Serializer.Serialize(returnValue8, returnDataPool);
				break;
			}
			case 15:
			{
				int argsCount16 = operation.ArgsCount;
				int num16 = argsCount16;
				if (num16 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				int charId = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref charId);
				this.SetCharacterToAdvanceBranch(context, charId);
				result = -1;
				break;
			}
			case 16:
			{
				int argsCount17 = operation.ArgsCount;
				int num17 = argsCount17;
				if (num17 != 0)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				bool returnValue9 = this.CanSetCharacterToAdvanceBranch();
				result = Serializer.Serialize(returnValue9, returnDataPool);
				break;
			}
			case 17:
			{
				int argsCount18 = operation.ArgsCount;
				int num18 = argsCount18;
				if (num18 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				List<Location> locationList = null;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref locationList);
				Dictionary<Location, AdventureSiteData> returnValue10 = this.GetAdventureSiteDataDict(locationList);
				result = Serializer.Serialize(returnValue10, returnDataPool);
				break;
			}
			case 18:
			{
				int argsCount19 = operation.ArgsCount;
				int num19 = argsCount19;
				if (num19 != 0)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				AdventureResultDisplayData returnValue11 = this.GetAdventureResultDisplayData();
				result = Serializer.Serialize(returnValue11, returnDataPool);
				break;
			}
			case 19:
			{
				int argsCount20 = operation.ArgsCount;
				int num20 = argsCount20;
				if (num20 != 2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				List<ItemKey> acceptItems = null;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref acceptItems);
				List<int> acceptCounts = null;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref acceptCounts);
				this.SelectGetItem(context, acceptItems, acceptCounts);
				result = -1;
				break;
			}
			case 20:
			{
				int argsCount21 = operation.ArgsCount;
				int num21 = argsCount21;
				if (num21 != 0)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				List<ValueTuple<int, int>> returnValue12 = this.GetPathContentList();
				result = Serializer.Serialize(returnValue12, returnDataPool);
				break;
			}
			case 21:
			{
				int argsCount22 = operation.ArgsCount;
				int num22 = argsCount22;
				if (num22 != 0)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				List<int> returnValue13 = this.GetAdventureSpentCharList();
				result = Serializer.Serialize(returnValue13, returnDataPool);
				break;
			}
			case 22:
			{
				int argsCount23 = operation.ArgsCount;
				int num23 = argsCount23;
				if (num23 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				ItemKey itemKey = default(ItemKey);
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref itemKey);
				ItemDisplayData returnValue14 = this.GetAdventureGainsItemList(itemKey);
				result = Serializer.Serialize(returnValue14, returnDataPool);
				break;
			}
			case 23:
			{
				int argsCount24 = operation.ArgsCount;
				int num24 = argsCount24;
				if (num24 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				short templateId = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref templateId);
				Location returnValue15 = this.QueryAdventureLocation(templateId);
				result = Serializer.Serialize(returnValue15, returnDataPool);
				break;
			}
			case 24:
			{
				int argsCount25 = operation.ArgsCount;
				int num25 = argsCount25;
				if (num25 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				short templateId2 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref templateId2);
				Location returnValue16 = this.QueryAdventureLocationFirstInCurrent(templateId2);
				result = Serializer.Serialize(returnValue16, returnDataPool);
				break;
			}
			case 25:
			{
				int argsCount26 = operation.ArgsCount;
				int num26 = argsCount26;
				if (num26 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				Location location = default(Location);
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref location);
				List<CharacterDisplayData> returnValue17 = this.GetCharacterDisplayDataListInAdventure(location);
				result = Serializer.Serialize(returnValue17, returnDataPool);
				break;
			}
			case 26:
			{
				int argsCount27 = operation.ArgsCount;
				int num27 = argsCount27;
				if (num27 != 0)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				bool returnValue18 = this.TryInvokeConfirmEnterEvent();
				result = Serializer.Serialize(returnValue18, returnDataPool);
				break;
			}
			case 27:
			{
				int argsCount28 = operation.ArgsCount;
				int num28 = argsCount28;
				if (num28 != 2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				string key = null;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref key);
				int value = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref value);
				this.GmCmd_SetAdventureParameter(key, value);
				result = -1;
				break;
			}
			case 28:
			{
				int argsCount29 = operation.ArgsCount;
				int num29 = argsCount29;
				if (num29 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				string key2 = null;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref key2);
				int returnValue19 = this.GmCmd_GetAdventureParameter(key2);
				result = Serializer.Serialize(returnValue19, returnDataPool);
				break;
			}
			default:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(21, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Unsupported methodId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			}
			return result;
		}

		// Token: 0x06007F5E RID: 32606 RVA: 0x004C84C0 File Offset: 0x004C66C0
		public override void OnMonitorData(ushort dataId, ulong subId0, uint subId1, bool monitoring)
		{
			switch (dataId)
			{
			case 0:
				break;
			case 1:
				break;
			case 2:
				break;
			case 3:
				break;
			case 4:
				break;
			case 5:
				break;
			case 6:
				break;
			case 7:
				break;
			case 8:
				break;
			case 9:
				break;
			case 10:
				break;
			case 11:
				break;
			case 12:
				break;
			case 13:
				break;
			case 14:
				break;
			case 15:
				break;
			case 16:
				break;
			case 17:
				break;
			case 18:
				this._modificationsEnemyNestSites.ChangeRecording(monitoring);
				break;
			case 19:
				break;
			case 20:
				break;
			case 21:
				break;
			case 22:
				break;
			case 23:
				break;
			case 24:
				break;
			default:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(19, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Unsupported dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			}
		}

		// Token: 0x06007F5F RID: 32607 RVA: 0x004C85AC File Offset: 0x004C67AC
		public override int CheckModified(ushort dataId, ulong subId0, uint subId1, RawDataPool dataPool)
		{
			int result;
			switch (dataId)
			{
			case 0:
			{
				bool flag = !BaseGameDataDomain.IsModified(this.DataStates, 0);
				if (flag)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 0);
					result = Serializer.Serialize(this._playerPos, dataPool);
				}
				break;
			}
			case 1:
			{
				bool flag2 = !BaseGameDataDomain.IsModified(this.DataStates, 1);
				if (flag2)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 1);
					result = Serializer.Serialize(this._personalities, dataPool);
				}
				break;
			}
			case 2:
			{
				bool flag3 = !BaseGameDataDomain.IsModified(this.DataStates, 2);
				if (flag3)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 2);
					result = Serializer.Serialize(this.GetPersonalitiesCost(), dataPool);
				}
				break;
			}
			case 3:
			{
				bool flag4 = !BaseGameDataDomain.IsModified(this.DataStates, 3);
				if (flag4)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 3);
					result = Serializer.Serialize(this.GetPersonalitiesGain(), dataPool);
				}
				break;
			}
			case 4:
			{
				bool flag5 = !BaseGameDataDomain.IsModified(this.DataStates, 4);
				if (flag5)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 4);
					result = Serializer.Serialize(this._indicatePath, dataPool);
				}
				break;
			}
			case 5:
			{
				bool flag6 = !BaseGameDataDomain.IsModified(this.DataStates, 5);
				if (flag6)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 5);
					result = Serializer.Serialize(this.GetArrangeableNodes(), dataPool);
				}
				break;
			}
			case 6:
			{
				bool flag7 = !BaseGameDataDomain.IsModified(this.DataStates, 6);
				if (flag7)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 6);
					result = Serializer.Serialize(this._arrangedNodes, dataPool);
				}
				break;
			}
			case 7:
			{
				bool flag8 = !BaseGameDataDomain.IsModified(this.DataStates, 7);
				if (flag8)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 7);
					result = Serializer.Serialize(this._perceivedNodes, dataPool);
				}
				break;
			}
			case 8:
			{
				bool flag9 = !BaseGameDataDomain.IsModified(this.DataStates, 8);
				if (flag9)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 8);
					result = Serializer.Serialize(this._errorInfo, dataPool);
				}
				break;
			}
			case 9:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(42, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to check modification of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 10:
			{
				bool flag10 = !BaseGameDataDomain.IsModified(this.DataStates, 10);
				if (flag10)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 10);
					result = Serializer.Serialize(this._curAdventureId, dataPool);
				}
				break;
			}
			case 11:
			{
				bool flag11 = !BaseGameDataDomain.IsModified(this.DataStates, 11);
				if (flag11)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 11);
					result = Serializer.Serialize(this._operationBlock, dataPool);
				}
				break;
			}
			case 12:
			{
				bool flag12 = !BaseGameDataDomain.IsModified(this.DataStates, 12);
				if (flag12)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 12);
					result = Serializer.Serialize(this._adventureState, dataPool);
				}
				break;
			}
			case 13:
			{
				bool flag13 = !BaseGameDataDomain.IsModified(this.DataStates, 13);
				if (flag13)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 13);
					result = Serializer.Serialize(this._curMapTrunk, dataPool);
				}
				break;
			}
			case 14:
			{
				bool flag14 = !BaseGameDataDomain.IsModified(this.DataStates, 14);
				if (flag14)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 14);
					result = Serializer.Serialize(this._advParameters, dataPool);
				}
				break;
			}
			case 15:
			{
				bool flag15 = !BaseGameDataDomain.IsModified(this.DataStates, 15);
				if (flag15)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 15);
					result = Serializer.Serialize(this._enterItems, dataPool);
				}
				break;
			}
			case 16:
			{
				bool flag16 = !BaseGameDataDomain.IsModified(this.DataStates, 16);
				if (flag16)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 16);
					result = Serializer.Serialize(this._allowExitAdventure, dataPool);
				}
				break;
			}
			case 17:
			{
				bool flag17 = !BaseGameDataDomain.IsModified(this._dataStatesAdventureAreas, (int)subId0);
				if (flag17)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this._dataStatesAdventureAreas, (int)subId0);
					result = Serializer.Serialize(this._adventureAreas[(int)subId0], dataPool);
				}
				break;
			}
			case 18:
			{
				bool flag18 = !BaseGameDataDomain.IsModified(this.DataStates, 18);
				if (flag18)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 18);
					int offset = Serializer.SerializeModifications<Location>(this._enemyNestSites, dataPool, this._modificationsEnemyNestSites);
					this._modificationsEnemyNestSites.Reset();
					result = offset;
				}
				break;
			}
			case 19:
			{
				bool flag19 = !BaseGameDataDomain.IsModified(this._dataStatesBrokenAreaEnemies, (int)subId0);
				if (flag19)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this._dataStatesBrokenAreaEnemies, (int)subId0);
					result = Serializer.Serialize(this._brokenAreaEnemies[(int)subId0], dataPool);
				}
				break;
			}
			case 20:
			{
				bool flag20 = !BaseGameDataDomain.IsModified(this.DataStates, 20);
				if (flag20)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 20);
					result = Serializer.Serialize(this._spentCharList, dataPool);
				}
				break;
			}
			case 21:
			{
				bool flag21 = !BaseGameDataDomain.IsModified(this.DataStates, 21);
				if (flag21)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 21);
					result = Serializer.Serialize(this._curBranchChosenChar, dataPool);
				}
				break;
			}
			case 22:
			{
				bool flag22 = !BaseGameDataDomain.IsModified(this.DataStates, 22);
				if (flag22)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 22);
					result = Serializer.Serialize(this._escapingRandomEnemies, dataPool);
				}
				break;
			}
			case 23:
			{
				bool flag23 = !BaseGameDataDomain.IsModified(this.DataStates, 23);
				if (flag23)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 23);
					result = Serializer.Serialize(this._teamDetectedNodes, dataPool);
				}
				break;
			}
			case 24:
			{
				bool flag24 = !BaseGameDataDomain.IsModified(this.DataStates, 24);
				if (flag24)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 24);
					result = Serializer.Serialize(this._actionPointWithhold, dataPool);
				}
				break;
			}
			default:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(19, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Unsupported dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			}
			return result;
		}

		// Token: 0x06007F60 RID: 32608 RVA: 0x004C8C70 File Offset: 0x004C6E70
		public override void ResetModifiedWrapper(ushort dataId, ulong subId0, uint subId1)
		{
			switch (dataId)
			{
			case 0:
			{
				bool flag = !BaseGameDataDomain.IsModified(this.DataStates, 0);
				if (!flag)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 0);
				}
				break;
			}
			case 1:
			{
				bool flag2 = !BaseGameDataDomain.IsModified(this.DataStates, 1);
				if (!flag2)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 1);
				}
				break;
			}
			case 2:
			{
				bool flag3 = !BaseGameDataDomain.IsModified(this.DataStates, 2);
				if (!flag3)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 2);
				}
				break;
			}
			case 3:
			{
				bool flag4 = !BaseGameDataDomain.IsModified(this.DataStates, 3);
				if (!flag4)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 3);
				}
				break;
			}
			case 4:
			{
				bool flag5 = !BaseGameDataDomain.IsModified(this.DataStates, 4);
				if (!flag5)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 4);
				}
				break;
			}
			case 5:
			{
				bool flag6 = !BaseGameDataDomain.IsModified(this.DataStates, 5);
				if (!flag6)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 5);
				}
				break;
			}
			case 6:
			{
				bool flag7 = !BaseGameDataDomain.IsModified(this.DataStates, 6);
				if (!flag7)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 6);
				}
				break;
			}
			case 7:
			{
				bool flag8 = !BaseGameDataDomain.IsModified(this.DataStates, 7);
				if (!flag8)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 7);
				}
				break;
			}
			case 8:
			{
				bool flag9 = !BaseGameDataDomain.IsModified(this.DataStates, 8);
				if (!flag9)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 8);
				}
				break;
			}
			case 9:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(48, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to reset modification state of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 10:
			{
				bool flag10 = !BaseGameDataDomain.IsModified(this.DataStates, 10);
				if (!flag10)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 10);
				}
				break;
			}
			case 11:
			{
				bool flag11 = !BaseGameDataDomain.IsModified(this.DataStates, 11);
				if (!flag11)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 11);
				}
				break;
			}
			case 12:
			{
				bool flag12 = !BaseGameDataDomain.IsModified(this.DataStates, 12);
				if (!flag12)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 12);
				}
				break;
			}
			case 13:
			{
				bool flag13 = !BaseGameDataDomain.IsModified(this.DataStates, 13);
				if (!flag13)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 13);
				}
				break;
			}
			case 14:
			{
				bool flag14 = !BaseGameDataDomain.IsModified(this.DataStates, 14);
				if (!flag14)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 14);
				}
				break;
			}
			case 15:
			{
				bool flag15 = !BaseGameDataDomain.IsModified(this.DataStates, 15);
				if (!flag15)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 15);
				}
				break;
			}
			case 16:
			{
				bool flag16 = !BaseGameDataDomain.IsModified(this.DataStates, 16);
				if (!flag16)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 16);
				}
				break;
			}
			case 17:
			{
				bool flag17 = !BaseGameDataDomain.IsModified(this._dataStatesAdventureAreas, (int)subId0);
				if (!flag17)
				{
					BaseGameDataDomain.ResetModified(this._dataStatesAdventureAreas, (int)subId0);
				}
				break;
			}
			case 18:
			{
				bool flag18 = !BaseGameDataDomain.IsModified(this.DataStates, 18);
				if (!flag18)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 18);
					this._modificationsEnemyNestSites.Reset();
				}
				break;
			}
			case 19:
			{
				bool flag19 = !BaseGameDataDomain.IsModified(this._dataStatesBrokenAreaEnemies, (int)subId0);
				if (!flag19)
				{
					BaseGameDataDomain.ResetModified(this._dataStatesBrokenAreaEnemies, (int)subId0);
				}
				break;
			}
			case 20:
			{
				bool flag20 = !BaseGameDataDomain.IsModified(this.DataStates, 20);
				if (!flag20)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 20);
				}
				break;
			}
			case 21:
			{
				bool flag21 = !BaseGameDataDomain.IsModified(this.DataStates, 21);
				if (!flag21)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 21);
				}
				break;
			}
			case 22:
			{
				bool flag22 = !BaseGameDataDomain.IsModified(this.DataStates, 22);
				if (!flag22)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 22);
				}
				break;
			}
			case 23:
			{
				bool flag23 = !BaseGameDataDomain.IsModified(this.DataStates, 23);
				if (!flag23)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 23);
				}
				break;
			}
			case 24:
			{
				bool flag24 = !BaseGameDataDomain.IsModified(this.DataStates, 24);
				if (!flag24)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 24);
				}
				break;
			}
			default:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(19, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Unsupported dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			}
		}

		// Token: 0x06007F61 RID: 32609 RVA: 0x004C919C File Offset: 0x004C739C
		public override bool IsModifiedWrapper(ushort dataId, ulong subId0, uint subId1)
		{
			bool result;
			switch (dataId)
			{
			case 0:
				result = BaseGameDataDomain.IsModified(this.DataStates, 0);
				break;
			case 1:
				result = BaseGameDataDomain.IsModified(this.DataStates, 1);
				break;
			case 2:
				result = BaseGameDataDomain.IsModified(this.DataStates, 2);
				break;
			case 3:
				result = BaseGameDataDomain.IsModified(this.DataStates, 3);
				break;
			case 4:
				result = BaseGameDataDomain.IsModified(this.DataStates, 4);
				break;
			case 5:
				result = BaseGameDataDomain.IsModified(this.DataStates, 5);
				break;
			case 6:
				result = BaseGameDataDomain.IsModified(this.DataStates, 6);
				break;
			case 7:
				result = BaseGameDataDomain.IsModified(this.DataStates, 7);
				break;
			case 8:
				result = BaseGameDataDomain.IsModified(this.DataStates, 8);
				break;
			case 9:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(49, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to verify modification state of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 10:
				result = BaseGameDataDomain.IsModified(this.DataStates, 10);
				break;
			case 11:
				result = BaseGameDataDomain.IsModified(this.DataStates, 11);
				break;
			case 12:
				result = BaseGameDataDomain.IsModified(this.DataStates, 12);
				break;
			case 13:
				result = BaseGameDataDomain.IsModified(this.DataStates, 13);
				break;
			case 14:
				result = BaseGameDataDomain.IsModified(this.DataStates, 14);
				break;
			case 15:
				result = BaseGameDataDomain.IsModified(this.DataStates, 15);
				break;
			case 16:
				result = BaseGameDataDomain.IsModified(this.DataStates, 16);
				break;
			case 17:
				result = BaseGameDataDomain.IsModified(this._dataStatesAdventureAreas, (int)subId0);
				break;
			case 18:
				result = BaseGameDataDomain.IsModified(this.DataStates, 18);
				break;
			case 19:
				result = BaseGameDataDomain.IsModified(this._dataStatesBrokenAreaEnemies, (int)subId0);
				break;
			case 20:
				result = BaseGameDataDomain.IsModified(this.DataStates, 20);
				break;
			case 21:
				result = BaseGameDataDomain.IsModified(this.DataStates, 21);
				break;
			case 22:
				result = BaseGameDataDomain.IsModified(this.DataStates, 22);
				break;
			case 23:
				result = BaseGameDataDomain.IsModified(this.DataStates, 23);
				break;
			case 24:
				result = BaseGameDataDomain.IsModified(this.DataStates, 24);
				break;
			default:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(19, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Unsupported dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			}
			return result;
		}

		// Token: 0x06007F62 RID: 32610 RVA: 0x004C9428 File Offset: 0x004C7628
		public override void InvalidateCache(BaseGameDataObject sourceObject, DataInfluence influence, DataContext context, bool unconditionallyInfluenceAll)
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			switch (influence.TargetIndicator.DataId)
			{
			case 0:
				break;
			case 1:
				break;
			case 2:
				BaseGameDataDomain.InvalidateSelfAndInfluencedCache(2, this.DataStates, AdventureDomain.CacheInfluences, context);
				return;
			case 3:
				BaseGameDataDomain.InvalidateSelfAndInfluencedCache(3, this.DataStates, AdventureDomain.CacheInfluences, context);
				return;
			case 4:
				break;
			case 5:
				BaseGameDataDomain.InvalidateSelfAndInfluencedCache(5, this.DataStates, AdventureDomain.CacheInfluences, context);
				return;
			case 6:
				break;
			case 7:
				break;
			case 8:
				break;
			case 9:
				break;
			case 10:
				break;
			case 11:
				break;
			case 12:
				break;
			case 13:
				break;
			case 14:
				break;
			case 15:
				break;
			case 16:
				break;
			case 17:
				break;
			case 18:
				break;
			case 19:
				break;
			case 20:
				break;
			case 21:
				break;
			case 22:
				break;
			case 23:
				break;
			case 24:
				break;
			default:
				defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(19, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Unsupported dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(influence.TargetIndicator.DataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(48, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Cannot invalidate cache state of non-cache data ");
			defaultInterpolatedStringHandler.AppendFormatted<ushort>(influence.TargetIndicator.DataId);
			throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
		}

		// Token: 0x06007F63 RID: 32611 RVA: 0x004C959C File Offset: 0x004C779C
		public unsafe override void ProcessArchiveResponse(OperationWrapper operation, byte* pResult)
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			switch (operation.DataId)
			{
			case 0:
				goto IL_1D5;
			case 1:
				goto IL_1D5;
			case 2:
				goto IL_1D5;
			case 3:
				goto IL_1D5;
			case 4:
				goto IL_1D5;
			case 5:
				goto IL_1D5;
			case 6:
				goto IL_1D5;
			case 7:
				goto IL_1D5;
			case 8:
				goto IL_1D5;
			case 9:
				goto IL_1D5;
			case 10:
				goto IL_1D5;
			case 11:
				goto IL_1D5;
			case 12:
				goto IL_1D5;
			case 13:
				goto IL_1D5;
			case 14:
				goto IL_1D5;
			case 15:
				goto IL_1D5;
			case 16:
				goto IL_1D5;
			case 17:
				ResponseProcessor.ProcessElementList_CustomType_Dynamic_Ref<AreaAdventureData>(operation, pResult, this._adventureAreas, 139);
				break;
			case 18:
				ResponseProcessor.ProcessSingleValueCollection_CustomType_Dynamic_Ref<Location, EnemyNestSiteExtraData>(operation, pResult, this._enemyNestSites);
				break;
			case 19:
				ResponseProcessor.ProcessElementList_CustomType_Dynamic_Ref<BrokenAreaData>(operation, pResult, this._brokenAreaEnemies, 90);
				break;
			case 20:
				goto IL_1D5;
			case 21:
				goto IL_1D5;
			case 22:
				ResponseProcessor.ProcessSingleValue_BasicType_Fixed_Value_List<short>(operation, pResult, this._escapingRandomEnemies);
				break;
			case 23:
				goto IL_1D5;
			case 24:
				goto IL_1D5;
			default:
				defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(19, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Unsupported dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.DataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			bool flag = this._pendingLoadingOperationIds != null;
			if (flag)
			{
				uint currPendingOperationId = this._pendingLoadingOperationIds.Peek();
				bool flag2 = currPendingOperationId == operation.Id;
				if (flag2)
				{
					this._pendingLoadingOperationIds.Dequeue();
					bool flag3 = this._pendingLoadingOperationIds.Count <= 0;
					if (flag3)
					{
						this._pendingLoadingOperationIds = null;
						this.InitializeInternalDataOfCollections();
						this.OnLoadedArchiveData();
						DomainManager.Global.CompleteLoading(10);
					}
				}
			}
			return;
			IL_1D5:
			defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(52, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Cannot process archive response of non-archive data ");
			defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.DataId);
			throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
		}

		// Token: 0x06007F64 RID: 32612 RVA: 0x004C97B2 File Offset: 0x004C79B2
		private void InitializeInternalDataOfCollections()
		{
		}

		// Token: 0x040022B0 RID: 8880
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		// Token: 0x040022B1 RID: 8881
		[DomainData(DomainDataType.SingleValue, false, false, true, true)]
		private int _playerPos;

		// Token: 0x040022B2 RID: 8882
		[DomainData(DomainDataType.SingleValue, false, false, true, true, ArrayElementsCount = 7)]
		private int[] _personalities;

		// Token: 0x040022B3 RID: 8883
		[DomainData(DomainDataType.SingleValue, false, true, true, true, ArrayElementsCount = 7)]
		private int[] _personalitiesCost;

		// Token: 0x040022B4 RID: 8884
		[DomainData(DomainDataType.SingleValue, false, true, true, true, ArrayElementsCount = 7)]
		private int[] _personalitiesGain;

		// Token: 0x040022B5 RID: 8885
		[DomainData(DomainDataType.SingleValue, false, false, true, true)]
		private List<int> _indicatePath;

		// Token: 0x040022B6 RID: 8886
		[DomainData(DomainDataType.SingleValue, false, true, true, true)]
		private List<int> _arrangeableNodes;

		// Token: 0x040022B7 RID: 8887
		[DomainData(DomainDataType.SingleValue, false, false, true, true)]
		private List<int> _arrangedNodes;

		// Token: 0x040022B8 RID: 8888
		[DomainData(DomainDataType.SingleValue, false, false, true, true)]
		private List<int> _teamDetectedNodes;

		// Token: 0x040022B9 RID: 8889
		[DomainData(DomainDataType.SingleValue, false, false, true, true)]
		private List<int> _perceivedNodes;

		// Token: 0x040022BA RID: 8890
		[DomainData(DomainDataType.SingleValue, false, false, true, true)]
		private string _errorInfo;

		// Token: 0x040022BB RID: 8891
		[DomainData(DomainDataType.SingleValueCollection, false, false, false, false)]
		private readonly Dictionary<int, int> _adBranchOpenRecord;

		// Token: 0x040022BC RID: 8892
		[DomainData(DomainDataType.SingleValue, false, false, true, true)]
		private short _curAdventureId = -1;

		// Token: 0x040022BD RID: 8893
		[DomainData(DomainDataType.SingleValue, false, false, true, true)]
		private bool _operationBlock = false;

		// Token: 0x040022BE RID: 8894
		[DomainData(DomainDataType.SingleValue, false, false, true, true)]
		private sbyte _adventureState;

		// Token: 0x040022BF RID: 8895
		[DomainData(DomainDataType.SingleValue, false, false, true, true)]
		private AdventureMapTrunk _curMapTrunk;

		// Token: 0x040022C0 RID: 8896
		[DomainData(DomainDataType.SingleValue, false, false, true, true)]
		private List<int> _advParameters;

		// Token: 0x040022C1 RID: 8897
		[DomainData(DomainDataType.SingleValue, false, false, true, false)]
		private List<ItemKey> _enterItems;

		// Token: 0x040022C2 RID: 8898
		private Dictionary<string, int> _parameterMap;

		// Token: 0x040022C3 RID: 8899
		[DomainData(DomainDataType.SingleValue, false, false, true, true)]
		private bool _allowExitAdventure;

		// Token: 0x040022C4 RID: 8900
		[DomainData(DomainDataType.SingleValue, false, false, true, true)]
		private int _actionPointWithhold = 0;

		// Token: 0x040022C5 RID: 8901
		private readonly HashSet<AdvMapPos> _triggeredPosSet = new HashSet<AdvMapPos>();

		// Token: 0x040022C6 RID: 8902
		private readonly HashSet<AdvMapPos> _finishedPosSet = new HashSet<AdvMapPos>();

		// Token: 0x040022C7 RID: 8903
		private List<ValueTuple<string, sbyte, short>> _extraEvents;

		// Token: 0x040022C8 RID: 8904
		private bool _eventBlock = false;

		// Token: 0x040022C9 RID: 8905
		private readonly List<AdvMapNode> _path = new List<AdvMapNode>();

		// Token: 0x040022CA RID: 8906
		private Location _curAdvSiteLocation;

		// Token: 0x040022CB RID: 8907
		private string _chosenBranchKey;

		// Token: 0x040022CC RID: 8908
		private Stack<AdventureMapTrunk> _mapTrunks;

		// Token: 0x040022CD RID: 8909
		private Action _onEventFinishCallback;

		// Token: 0x040022CE RID: 8910
		private readonly AdventureResultDisplayData _resultDisplayData = new AdventureResultDisplayData();

		// Token: 0x040022CF RID: 8911
		[DomainData(DomainDataType.ElementList, true, false, true, false, ArrayElementsCount = 139)]
		private readonly AreaAdventureData[] _adventureAreas;

		// Token: 0x040022D0 RID: 8912
		[DomainData(DomainDataType.SingleValueCollection, true, false, true, true)]
		private readonly Dictionary<Location, EnemyNestSiteExtraData> _enemyNestSites;

		// Token: 0x040022D1 RID: 8913
		[DomainData(DomainDataType.ElementList, true, false, true, false, ArrayElementsCount = 90)]
		private readonly BrokenAreaData[] _brokenAreaEnemies;

		// Token: 0x040022D2 RID: 8914
		private readonly sbyte[] _stateBrokenAreaLevels = new sbyte[]
		{
			1,
			2,
			3,
			4,
			5,
			6
		};

		// Token: 0x040022D3 RID: 8915
		private static int _brokenAreaEnemyBaseCount;

		// Token: 0x040022D4 RID: 8916
		private static List<short>[] AdventureTypes;

		// Token: 0x040022D5 RID: 8917
		[Obsolete]
		public static readonly Dictionary<short, sbyte> AdventureTemplateIdToXiangshuAvatarId = new Dictionary<short, sbyte>
		{
			{
				114,
				0
			},
			{
				111,
				1
			},
			{
				108,
				2
			},
			{
				110,
				3
			},
			{
				109,
				4
			},
			{
				113,
				5
			},
			{
				116,
				6
			},
			{
				115,
				7
			},
			{
				112,
				8
			}
		};

		// Token: 0x040022D6 RID: 8918
		[DomainData(DomainDataType.SingleValue, false, false, true, true)]
		private List<int> _spentCharList;

		// Token: 0x040022D7 RID: 8919
		private readonly Dictionary<int, int> _spentCharInCombatGroupDict = new Dictionary<int, int>();

		// Token: 0x040022D8 RID: 8920
		[DomainData(DomainDataType.SingleValue, false, false, true, true)]
		private int _curBranchChosenChar;

		// Token: 0x040022D9 RID: 8921
		private HashSet<int> _temporaryIntelligentCharacters;

		// Token: 0x040022DA RID: 8922
		private HashSet<int> _temporaryEnemies;

		// Token: 0x040022DB RID: 8923
		[DomainData(DomainDataType.SingleValue, true, false, true, true)]
		private List<short> _escapingRandomEnemies;

		// Token: 0x040022DC RID: 8924
		private int[] _enemyNestCounts;

		// Token: 0x040022DE RID: 8926
		private readonly List<AdvMapBranch> _branches = new List<AdvMapBranch>();

		// Token: 0x040022DF RID: 8927
		private readonly List<AdvMapNodeVertex> _vertices = new List<AdvMapNodeVertex>();

		// Token: 0x040022E0 RID: 8928
		public readonly Dictionary<AdvMapPos, AdvMapNode> NodesDict = new Dictionary<AdvMapPos, AdvMapNode>();

		// Token: 0x040022E1 RID: 8929
		private int _enterTerrain;

		// Token: 0x040022E2 RID: 8930
		public List<ValueTuple<short, short>> EnterTerrainWeights;

		// Token: 0x040022E3 RID: 8931
		private AdvMapBranch _curBranch;

		// Token: 0x040022E4 RID: 8932
		private readonly List<ValueTuple<int, int>> _contentList = new List<ValueTuple<int, int>>();

		// Token: 0x040022E5 RID: 8933
		private static readonly DataInfluence[][] CacheInfluences = new DataInfluence[25][];

		// Token: 0x040022E6 RID: 8934
		private SpinLock _spinLockPersonalitiesCost = new SpinLock(false);

		// Token: 0x040022E7 RID: 8935
		private SpinLock _spinLockPersonalitiesGain = new SpinLock(false);

		// Token: 0x040022E8 RID: 8936
		private SpinLock _spinLockArrangeableNodes = new SpinLock(false);

		// Token: 0x040022E9 RID: 8937
		private static readonly DataInfluence[][] CacheInfluencesAdventureAreas = new DataInfluence[139][];

		// Token: 0x040022EA RID: 8938
		private readonly byte[] _dataStatesAdventureAreas = new byte[35];

		// Token: 0x040022EB RID: 8939
		private SingleValueCollectionModificationCollection<Location> _modificationsEnemyNestSites = SingleValueCollectionModificationCollection<Location>.Create();

		// Token: 0x040022EC RID: 8940
		private static readonly DataInfluence[][] CacheInfluencesBrokenAreaEnemies = new DataInfluence[90][];

		// Token: 0x040022ED RID: 8941
		private readonly byte[] _dataStatesBrokenAreaEnemies = new byte[23];

		// Token: 0x040022EE RID: 8942
		private Queue<uint> _pendingLoadingOperationIds;
	}
}
