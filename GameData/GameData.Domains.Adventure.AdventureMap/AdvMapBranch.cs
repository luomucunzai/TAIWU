using System;
using System.Collections.Generic;
using System.Linq;
using Config;
using GameData.Common;
using GameData.Utilities;
using Redzen.Random;

namespace GameData.Domains.Adventure.AdventureMap;

public class AdvMapBranch : AdvBaseMapBranch<AdvMapNodeVertex>
{
	private readonly AdventureBaseBranch _branchConfigData;

	public AdvancedBranch AdvancedBranch;

	private readonly List<short> _terrains;

	private const int ExtraNodeMultiplier = 4;

	private readonly List<(string, sbyte, short)> _extraEvents;

	private readonly List<short> _contentWeightDistribution;

	public ELinkDir EnterDir { get; set; }

	public AdvMapPos BasePos => base.EnterNode.AdjustedPos + EnterDir.Offset((base.EnterNode.NodeType == ENodeType.Start) ? 1 : 2);

	public bool AnchorByStart => base.ExitNode.PrevVertex == base.EnterNode;

	public int BranchLength { get; private set; }

	public string BranchKey { get; private set; }

	public AdvMapBranch(int branchIdx, AdvMapNodeVertex start, AdvMapNodeVertex end, AdventureBaseBranch branchConfigData, DataContext context)
	{
		BranchIndex = branchIdx;
		base.BaseBranch = this;
		base.EnterNode = start;
		base.ExitNode = end;
		_branchConfigData = branchConfigData;
		_terrains = new List<short>();
		_extraEvents = new List<(string, sbyte, short)>();
		_contentWeightDistribution = new List<short>();
		int num = context.Random.Next((int)((float)branchConfigData.Length * 0.8f), (int)((float)branchConfigData.Length * 1.2f) + 1);
		if (num > 1000)
		{
			throw new Exception("Branch Is Too Large");
		}
		BranchLength = num;
		BranchKey = branchConfigData.BranchKey;
		if (_branchConfigData.TerrainPersonalityWeights == null || _branchConfigData.TerrainPersonalityWeights.Length == 0 || _branchConfigData.TerrainPersonalityWeights.Sum(((byte, short, short[]) weight) => weight.Item2) == 0)
		{
		}
		FirstNode = new AdvMapNodeNormal(-1, this, context, -1);
		SetFirstNode(context);
		Nodes.Add(FirstNode);
	}

	public string GetExtraEvent(int index)
	{
		return _extraEvents[index].Item1;
	}

	private void SetFirstNode(DataContext context)
	{
		var (terrainId, b, lifeSkillType, lifeSkillRequiredVal) = GetRandomNodeInfo(context);
		if (base.EnterNode.NodeType == ENodeType.Start)
		{
			FirstNode.TerrainId = terrainId;
			FirstNode.SevenElementType = b;
			FirstNode.SevenElementCost = (sbyte)context.Random.Next((int)GlobalConfig.Instance.AdventureNodePersonalityMinCost, GlobalConfig.Instance.AdventureNodePersonalityMaxCost + 1);
			FirstNode.LifeSkillType = lifeSkillType;
			FirstNode.LifeSkillRequiredVal = lifeSkillRequiredVal;
			FirstNode.NodeContent = GetRandomContent(context, terrainId, b);
		}
		else
		{
			FirstNode.TerrainId = terrainId;
			FirstNode.SevenElementType = -1;
			FirstNode.LifeSkillType = -1;
			FirstNode.LifeSkillRequiredVal = -1;
			FirstNode.NodeContent = (-1, -1);
		}
		FirstNode.Offset = new AdvMapPos(0, 0);
	}

	public void GenBranch(DataContext context, List<(string, sbyte, short)> extraEvents, HashSet<AdvMapPos> primaryPath = null)
	{
		_extraEvents.Clear();
		_extraEvents.AddRange(extraEvents);
		_contentWeightDistribution.Clear();
		base.ExitNode.PrevVertex = base.EnterNode;
		while (true)
		{
			if (Nodes.Count >= BranchLength)
			{
				List<AdvMapNodeNormal> nodes = Nodes;
				if (nodes[nodes.Count - 1].Offset.X - Nodes[0].Offset.X >= BranchLength * 2)
				{
					break;
				}
			}
			List<int> dirWeight = null;
			switch (EnterDir)
			{
			case ELinkDir.Right:
				dirWeight = new List<int> { 20, 60, 20 };
				break;
			case ELinkDir.UpRight:
				dirWeight = new List<int> { 60, 35, 15 };
				break;
			case ELinkDir.DownRight:
				dirWeight = new List<int> { 15, 35, 60 };
				break;
			default:
				throw new Exception($"不合法的进入方向{EnterDir}");
			}
			List<AdvMapNodeNormal> nodes2 = Nodes;
			AdvMapNodeNormal advMapNodeNormal = nodes2[nodes2.Count - 1];
			if (advMapNodeNormal.Offset.X <= 2)
			{
				for (int i = 0; i < dirWeight.Count; i++)
				{
					dirWeight[i] = ((dirWeight[i] > 35) ? dirWeight[i] : 0);
				}
			}
			else if (advMapNodeNormal.Offset.X <= 4)
			{
				for (int j = 0; j < dirWeight.Count; j++)
				{
					dirWeight[j] = ((dirWeight[j] > 20) ? ((dirWeight[j] <= 35) ? 20 : dirWeight[j]) : 0);
				}
			}
			if (advMapNodeNormal.Offset.Y >= 3)
			{
				dirWeight[0] /= ((advMapNodeNormal.Offset.Y >= 5) ? advMapNodeNormal.Offset.Y : 2);
			}
			else if (advMapNodeNormal.Offset.Y <= -3)
			{
				dirWeight[2] /= ((advMapNodeNormal.Offset.Y <= -5) ? (-advMapNodeNormal.Offset.Y) : 2);
			}
			if (primaryPath != null)
			{
				if (primaryPath.Contains(GetFinalPos(advMapNodeNormal.Offset + new AdvMapPos(1, 1))) || primaryPath.Contains(GetFinalPos(advMapNodeNormal.Offset + new AdvMapPos(2, 2))))
				{
					dirWeight[0] = 0;
				}
				if (primaryPath.Contains(GetFinalPos(advMapNodeNormal.Offset + new AdvMapPos(2, 0))))
				{
					dirWeight[1] = 0;
				}
				if (primaryPath.Contains(GetFinalPos(advMapNodeNormal.Offset + new AdvMapPos(1, -1))) || primaryPath.Contains(GetFinalPos(advMapNodeNormal.Offset + new AdvMapPos(2, -2))))
				{
					dirWeight[2] = 0;
				}
			}
			var (terrainId, sevenElementType, lifeSkillType, lifeSkillRequiredVal) = GetRandomNodeInfo(context);
			switch (RandomUtils.GetRandomResult(from a in Enumerable.Range(0, 3)
				select (a: a, (short)dirWeight[a]), context.Random))
			{
			case 0:
			{
				AdvMapNodeNormal advMapNodeNormal4 = new AdvMapNodeNormal(terrainId, this, context, sevenElementType);
				advMapNodeNormal4.LifeSkillType = lifeSkillType;
				advMapNodeNormal4.LifeSkillRequiredVal = lifeSkillRequiredVal;
				advMapNodeNormal4.Offset = advMapNodeNormal.Offset + new AdvMapPos(1, 1);
				Nodes.Add(advMapNodeNormal4);
				break;
			}
			case 1:
			{
				AdvMapNodeNormal advMapNodeNormal3 = new AdvMapNodeNormal(terrainId, this, context, sevenElementType);
				advMapNodeNormal3.LifeSkillType = lifeSkillType;
				advMapNodeNormal3.LifeSkillRequiredVal = lifeSkillRequiredVal;
				advMapNodeNormal3.Offset = advMapNodeNormal.Offset + new AdvMapPos(2, 0);
				Nodes.Add(advMapNodeNormal3);
				break;
			}
			case 2:
			{
				AdvMapNodeNormal advMapNodeNormal2 = new AdvMapNodeNormal(terrainId, this, context, sevenElementType);
				advMapNodeNormal2.LifeSkillType = lifeSkillType;
				advMapNodeNormal2.LifeSkillRequiredVal = lifeSkillRequiredVal;
				advMapNodeNormal2.Offset = advMapNodeNormal.Offset + new AdvMapPos(1, -1);
				Nodes.Add(advMapNodeNormal2);
				break;
			}
			}
		}
		List<AdvMapNodeNormal> nodes3 = Nodes;
		LastNode = nodes3[nodes3.Count - 1];
		if (BranchLength > 0)
		{
			int num = BranchLength * 4 - Nodes.Count;
			int num3;
			for (int num2 = 0; num2 < num; num2 += num3)
			{
				List<AdvMapPos> list = new List<AdvMapPos>();
				AdvMapPos[] nodePoints = Nodes.Select((AdvMapNodeNormal a) => a.Offset).ToArray();
				foreach (AdvMapNodeNormal node in Nodes)
				{
					list.AddRange(node.Offset.GetAroundPoints());
				}
				list = new HashSet<AdvMapPos>(list).ToList();
				list.RemoveAll(delegate(AdvMapPos validPos)
				{
					if (nodePoints.Exist(validPos))
					{
						return true;
					}
					if (primaryPath != null && primaryPath.Contains(GetFinalPos(validPos)))
					{
						return true;
					}
					IEnumerable<AdvMapPos> enumerable = from a in validPos.GetAroundPoints()
						where nodePoints.Contains(a)
						select a;
					bool flag = false;
					bool flag2 = false;
					foreach (AdvMapPos item7 in enumerable)
					{
						if (validPos.X > item7.X)
						{
							flag = true;
						}
						if (validPos.X < item7.X)
						{
							flag2 = true;
						}
					}
					return !(flag2 && flag);
				});
				if (list.Count == 0)
				{
					break;
				}
				CollectionUtils.Shuffle(context.Random, list);
				num3 = context.Random.Next(1, Math.Min(list.Count, num - num2) + 1);
				for (int num4 = 0; num4 < num3; num4++)
				{
					(byte, sbyte, byte, short) randomNodeInfo = GetRandomNodeInfo(context);
					byte item = randomNodeInfo.Item1;
					sbyte item2 = randomNodeInfo.Item2;
					byte item3 = randomNodeInfo.Item3;
					short item4 = randomNodeInfo.Item4;
					AdvMapNodeNormal advMapNodeNormal5 = new AdvMapNodeNormal(item, this, context, item2);
					advMapNodeNormal5.Offset = list[num4];
					advMapNodeNormal5.LifeSkillType = item3;
					advMapNodeNormal5.LifeSkillRequiredVal = item4;
					Nodes.Add(advMapNodeNormal5);
				}
			}
		}
		for (int num5 = 1; num5 < Nodes.Count; num5++)
		{
			Nodes[num5].NodeContent = GetRandomContent(context, Nodes[num5].TerrainId, Nodes[num5].SevenElementType);
			if (Nodes[num5].NodeContent.Item1 == -1)
			{
				Nodes[num5].LifeSkillType = -1;
				Nodes[num5].LifeSkillRequiredVal = -1;
			}
		}
		if (_extraEvents.Count > 0)
		{
			for (int num6 = 0; num6 < _extraEvents.Count; num6++)
			{
				(string, sbyte, short) tuple2 = _extraEvents[num6];
				sbyte personality = tuple2.Item2;
				short num7 = tuple2.Item3;
				List<AdvMapNodeNormal> list2 = Nodes.Where((AdvMapNodeNormal node) => node.SevenElementType == personality).ToList();
				List<AdvMapNodeNormal> list3 = list2.Where((AdvMapNodeNormal node) => node.NodeContent.Item1 == -1).ToList();
				while (num7 > 0)
				{
					(byte, sbyte, byte, short) randomNodeInfo2 = GetRandomNodeInfo(context, _branchConfigData);
					byte item5 = randomNodeInfo2.Item3;
					short item6 = randomNodeInfo2.Item4;
					if (list3.Count > 0)
					{
						int index = context.Random.Next(0, list3.Count);
						AdvMapNodeNormal advMapNodeNormal6 = list3[index];
						advMapNodeNormal6.NodeContent = (10, num6);
						advMapNodeNormal6.LifeSkillType = item5;
						advMapNodeNormal6.LifeSkillRequiredVal = item6;
						list2.Remove(advMapNodeNormal6);
						list3.RemoveAt(index);
					}
					else
					{
						if (list2.Count <= 0)
						{
							break;
						}
						int index2 = context.Random.Next(0, list2.Count);
						AdvMapNodeNormal advMapNodeNormal7 = list2[index2];
						advMapNodeNormal7.NodeContent = (10, num6);
						advMapNodeNormal7.LifeSkillType = item5;
						advMapNodeNormal7.LifeSkillRequiredVal = item6;
						list2.RemoveAt(index2);
					}
					num7--;
				}
			}
		}
		GenerateAdvancedBranch(context);
	}

	private void GenerateAdvancedBranch(DataContext context)
	{
		List<AdventureAdvancedBranch> list = DomainManager.Adventure.AdventureCfg.AdvancedBranches.Where((AdventureAdvancedBranch advBranch) => advBranch.ParentBranchId == DomainManager.Adventure.AdventureCfg.BaseBranches.IndexOf(_branchConfigData)).ToList();
		if (list.Count != 1)
		{
			return;
		}
		if (Nodes.Count == 0)
		{
			throw new Exception("An empty base branch can't bind advanced branch.");
		}
		AdventureAdvancedBranch adventureAdvancedBranch = list[0];
		int idx = DomainManager.Adventure.AdventureCfg.AdvancedBranches.IndexOf(adventureAdvancedBranch) + DomainManager.Adventure.AdventureCfg.BaseBranches.Count;
		if (adventureAdvancedBranch.Length <= 0)
		{
			throw new Exception("Length of advanced branch can't be zero.");
		}
		short length = adventureAdvancedBranch.Length;
		HashSet<AdvMapPos> hashSet = Nodes.Select((AdvMapNodeNormal node) => node.Offset).ToHashSet();
		HashSet<AdvMapPos> hashSet2 = hashSet.Expand();
		(int, int) xRange = ((EnterDir == ELinkDir.Right) ? 2 : 3, LastNode.Offset.X - 2);
		List<AdvMapPos> list2 = (from pos in hashSet2.Expand().Except(hashSet2).Except(hashSet)
			where pos.X >= xRange.Item1 && pos.X <= xRange.Item2
			select pos).ToList();
		int maxStartX = xRange.Item2 - length;
		List<AdvMapPos> list3 = list2.Where((AdvMapPos pos) => pos.X <= maxStartX).ToList();
		List<AdvMapPos> list4 = new List<AdvMapPos>();
		List<AdvMapPos> list5 = new List<AdvMapPos>();
		List<AdvMapPos> list6 = new List<AdvMapPos>();
		List<AdvMapNodeNormal> list7 = new List<AdvMapNodeNormal>();
		AdvancedBranch = null;
		while (AdvancedBranch == null && list3.Count > 0)
		{
			list4.Clear();
			AdvMapPos startPos = list3[context.Random.Next(0, list3.Count)];
			list4.Add(startPos);
			AdvMapPos advMapPos = startPos;
			while (list4.Count < length)
			{
				AdvMapPos[] aroundPoints = advMapPos.GetAroundPoints();
				short x = advMapPos.X;
				advMapPos = AdvMapPos.Error;
				AdvMapPos[] array = aroundPoints;
				for (int num = 0; num < array.Length; num++)
				{
					AdvMapPos advMapPos2 = array[num];
					if (advMapPos2.X > x && list2.Contains(advMapPos2))
					{
						advMapPos = advMapPos2;
						list4.Add(advMapPos);
						break;
					}
				}
				if (!advMapPos.Equals(AdvMapPos.Error))
				{
					continue;
				}
				list3.Remove(startPos);
				break;
			}
			if (list3.Count == 0)
			{
				AdaptableLog.TagWarning("Adventure", "failed to generate advanced branch.");
				break;
			}
			if (!list3.Contains(startPos))
			{
				continue;
			}
			AdvMapPos enterPos = AdvMapPos.Error;
			AdvMapPos exitPos = AdvMapPos.Error;
			AdvMapPos advMapPos3 = hashSet.First((AdvMapPos pos) => Math.Abs(pos.X - startPos.X) < 2);
			enterPos = startPos + AdvMapPos.Left * 2;
			exitPos = advMapPos + AdvMapPos.Right * 2;
			if (!hashSet.Contains(enterPos))
			{
				if (advMapPos3.Y < startPos.Y)
				{
					enterPos = startPos + AdvMapPos.LowerLeft * 2;
					AddComplements(enterPos, AdvMapPos.Left, AdvMapPos.LowerLeft, hashSet, list5, list6);
				}
				else if (advMapPos3.Y > startPos.Y)
				{
					enterPos = startPos + AdvMapPos.UpperLeft * 2;
					AddComplements(enterPos, AdvMapPos.Left, AdvMapPos.UpperLeft, hashSet, list5, list6);
				}
				if (list6.Count > 0)
				{
					var (terrainId, b, lifeSkillType, lifeSkillRequiredVal) = GetRandomNodeInfo(context, _branchConfigData);
					FirstNode.SevenElementType = b;
					FirstNode.SevenElementCost = (sbyte)context.Random.Next((int)GlobalConfig.Instance.AdventureNodePersonalityMinCost, GlobalConfig.Instance.AdventureNodePersonalityMaxCost + 1);
					FirstNode.LifeSkillType = lifeSkillType;
					FirstNode.LifeSkillRequiredVal = lifeSkillRequiredVal;
					FirstNode.NodeContent = GetRandomContent(context, terrainId, b, _branchConfigData);
					CreateNodes(context, list6, list7);
					Nodes.AddRange(list7);
					list6.Clear();
					FirstNode = list7[0];
				}
			}
			if (!hashSet.Contains(exitPos))
			{
				if (advMapPos3.Y < startPos.Y)
				{
					exitPos = advMapPos + AdvMapPos.LowerRight * 2;
					AddComplements(exitPos, AdvMapPos.Right, AdvMapPos.LowerRight, hashSet, list5, list6);
				}
				else
				{
					exitPos = advMapPos + AdvMapPos.UpperRight * 2;
					AddComplements(exitPos, AdvMapPos.Right, AdvMapPos.UpperRight, hashSet, list5, list6);
				}
				if (list6.Count > 0)
				{
					CreateNodes(context, list6, list7);
					Nodes.AddRange(list7);
					list6.Clear();
					LastNode = list7[0];
				}
			}
			CreateNodes(context, list5, list7);
			Nodes.AddRange(list7);
			AdvMapPos offset = FirstNode.Offset;
			if (offset.X != 0 || offset.Y != 0)
			{
				foreach (AdvMapNodeNormal node in Nodes)
				{
					node.Offset -= offset;
				}
				enterPos -= offset;
				exitPos -= offset;
			}
			AdvancedBranch = new AdvancedBranch(Nodes.First((AdvMapNodeNormal a) => a.Offset.Equals(enterPos)), Nodes.First((AdvMapNodeNormal a) => a.Offset.Equals(exitPos)), idx, this, adventureAdvancedBranch.EnterSkillType, adventureAdvancedBranch.EnterSkillRequire);
			foreach (AdvMapPos item5 in list4)
			{
				(byte, sbyte, byte, short) randomNodeInfo = GetRandomNodeInfo(context, adventureAdvancedBranch);
				byte item = randomNodeInfo.Item1;
				sbyte item2 = randomNodeInfo.Item2;
				byte item3 = randomNodeInfo.Item3;
				short item4 = randomNodeInfo.Item4;
				AdvMapNodeNormal advMapNodeNormal = new AdvMapNodeNormal(item, AdvancedBranch, context, item2);
				advMapNodeNormal.SevenElementCost = 0;
				advMapNodeNormal.LifeSkillType = item3;
				advMapNodeNormal.LifeSkillRequiredVal = item4;
				advMapNodeNormal.Offset = item5 - offset;
				advMapNodeNormal.NodeContent = GetRandomContent(context, item, item2, adventureAdvancedBranch);
				AdvancedBranch.Nodes.Add(advMapNodeNormal);
			}
		}
	}

	private void AddComplements(AdvMapPos initialPos, AdvMapPos checkDir, AdvMapPos complementDir, HashSet<AdvMapPos> basePoints, List<AdvMapPos> complementList, List<AdvMapPos> extensionList)
	{
		List<AdvMapPos> list = new List<AdvMapPos>();
		AdvMapPos advMapPos = initialPos;
		while (!basePoints.Contains(advMapPos))
		{
			list.Add(advMapPos);
			if (basePoints.Contains(advMapPos + checkDir))
			{
				break;
			}
			advMapPos += complementDir;
			if (advMapPos.Y == FirstNode.Offset.Y && advMapPos.X < FirstNode.Offset.X)
			{
				extensionList.Add(advMapPos);
				for (int i = advMapPos.X + 2; i < FirstNode.Offset.X; i += 2)
				{
					extensionList.Add(new AdvMapPos(i, advMapPos.Y));
				}
				break;
			}
			if (advMapPos.Y == LastNode.Offset.Y && advMapPos.X > LastNode.Offset.X)
			{
				extensionList.Add(advMapPos);
				for (int j = LastNode.Offset.X + 2; j < advMapPos.X; j += 2)
				{
					extensionList.Add(new AdvMapPos(j, advMapPos.Y));
				}
				break;
			}
		}
		AdvMapPos advMapPos2 = new AdvMapPos(-complementDir.X, complementDir.Y);
		foreach (AdvMapPos item in list)
		{
			for (advMapPos = item + advMapPos2; !complementList.Contains(advMapPos) && !basePoints.Contains(advMapPos) && !extensionList.Contains(advMapPos); advMapPos += advMapPos2)
			{
				complementList.Add(advMapPos);
			}
		}
		complementList.AddRange(list);
	}

	private void CreateNodes(DataContext context, List<AdvMapPos> positions, List<AdvMapNodeNormal> result)
	{
		result.Clear();
		foreach (AdvMapPos position in positions)
		{
			(byte, sbyte, byte, short) randomNodeInfo = GetRandomNodeInfo(context, _branchConfigData);
			byte item = randomNodeInfo.Item1;
			sbyte item2 = randomNodeInfo.Item2;
			byte item3 = randomNodeInfo.Item3;
			short item4 = randomNodeInfo.Item4;
			AdvMapNodeNormal advMapNodeNormal = new AdvMapNodeNormal(item, this, context, item2);
			advMapNodeNormal.LifeSkillType = item3;
			advMapNodeNormal.LifeSkillRequiredVal = item4;
			advMapNodeNormal.Offset = position;
			advMapNodeNormal.NodeContent = GetRandomContent(context, item, item2, _branchConfigData);
			result.Add(advMapNodeNormal);
		}
	}

	private AdvMapPos GetFinalPos(AdvMapPos offset)
	{
		return BasePos + EnterDir.Rotate(offset);
	}

	private (byte, sbyte, byte, short) GetRandomNodeInfo(DataContext context, AdventureBranch branchConfig = null)
	{
		if (branchConfig == null)
		{
			branchConfig = _branchConfigData;
		}
		if (branchConfig.TerrainPersonalityWeights.Length == 0)
		{
			AdaptableLog.Warning($"{_branchConfigData.BranchKey} from {_branchConfigData.PortA} to {_branchConfigData.PortB} has no personality weight configured..", appendWarningMessage: true);
			return (1, -1, 0, -1);
		}
		if (branchConfig.TerrainPersonalityWeights.Sum(((byte, short, short[]) weight) => weight.Item2) == 0)
		{
			AdaptableLog.Warning($"{_branchConfigData.BranchKey} from {_branchConfigData.PortA} to {_branchConfigData.PortB} personality weight sum is zero..", appendWarningMessage: true);
			return (1, -1, 0, -1);
		}
		(byte, short[]) randomResult = RandomUtils.GetRandomResult(branchConfig.TerrainPersonalityWeights, context.Random);
		byte item = randomResult.Item1;
		sbyte item2 = (sbyte)RandomUtils.GetRandomIndex(randomResult.Item2 ?? AdventureTerrain.Instance[item].EvtWeights.ToArray(), context.Random);
		byte randomResult2 = RandomUtils.GetRandomResult(branchConfig.SkillWeights, context.Random);
		short item3 = (short)(DomainManager.Adventure.CurAdventureLifeSkillDifficulty * GetBaseSkillRequirement(context.Random));
		return (item, item2, randomResult2, item3);
	}

	public static short GetBaseSkillRequirement(IRandomSource random)
	{
		int num = 0;
		foreach (AdventureLifeSkillRequirementItem item in (IEnumerable<AdventureLifeSkillRequirementItem>)AdventureLifeSkillRequirement.Instance)
		{
			num += item.Weight;
		}
		int num2 = random.Next(0, num);
		for (int i = 0; i < AdventureLifeSkillRequirement.Instance.Count; i++)
		{
			num2 -= AdventureLifeSkillRequirement.Instance[i].Weight;
			if (num2 < 0)
			{
				return AdventureLifeSkillRequirement.Instance[i].RequiredValue;
			}
		}
		throw new Exception("Failed to get a random value for base skill requirement");
	}

	private (sbyte, int) GetRandomContent(DataContext context, int terrainId, sbyte personalityType, AdventureBranch branchConfig = null)
	{
		if (branchConfig == null)
		{
			branchConfig = _branchConfigData;
		}
		if (personalityType < 0)
		{
			return (-1, -1);
		}
		if (personalityType >= branchConfig.PersonalityContentWeights.Length)
		{
			return (-1, -1);
		}
		AdventurePersonalityContentWeights adventurePersonalityContentWeights = branchConfig.PersonalityContentWeights[personalityType];
		int randomIndex = RandomUtils.GetRandomIndex(adventurePersonalityContentWeights.ContentTypeWeights, context.Random);
		return randomIndex switch
		{
			0 => (-1, -1), 
			1 => (0, RandomUtils.GetRandomIndex(adventurePersonalityContentWeights.EventWeights, context.Random)), 
			2 => (1, RandomUtils.GetRandomIndex(adventurePersonalityContentWeights.NormalResWeights, context.Random)), 
			3 => (2, RandomUtils.GetRandomIndex(adventurePersonalityContentWeights.SpecialResWeights, context.Random)), 
			4 => (3, RandomUtils.GetRandomIndex(adventurePersonalityContentWeights.BonusWeights, context.Random)), 
			_ => throw new Exception($"Invalid adventure node content type {randomIndex}."), 
		};
	}

	public void FillConnect(List<AdventureMapConnect> connects, bool includeEnterConnect = true)
	{
		if (includeEnterConnect && base.EnterNode.NodeType != ENodeType.Start && BranchLength != 0)
		{
			connects.Add(new AdventureMapConnect(base.EnterNode.AdjustedPos.GetHashCode(), FirstNode.AdjustedPos.GetHashCode()));
		}
		connects.Add(new AdventureMapConnect(LastNode.AdjustedPos.GetHashCode(), base.ExitNode.AdjustedPos.GetHashCode()));
		if (AdvancedBranch != null)
		{
			connects.Add(new AdventureMapConnect(AdvancedBranch.EnterNode.AdjustedPos.GetHashCode(), AdvancedBranch.FirstNode.AdjustedPos.GetHashCode())
			{
				EnterLifeSkillType = AdvancedBranch.EnterLifeSkillType,
				EnterRequiredVal = AdvancedBranch.EnterLifeSkillVal
			});
			connects.Add(new AdventureMapConnect(AdvancedBranch.LastNode.AdjustedPos.GetHashCode(), AdvancedBranch.ExitNode.AdjustedPos.GetHashCode()));
		}
	}
}
