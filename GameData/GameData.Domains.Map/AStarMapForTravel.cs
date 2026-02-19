using System;
using System.Collections.Generic;

namespace GameData.Domains.Map;

public class AStarMapForTravel
{
	private class AStarNode
	{
		public short AreaId;

		public int GValue;

		public int HValue;

		public int FValue;

		public AStarNode ParentNode;
	}

	private class OpenNodeHandler
	{
		private List<AStarNode> _openNodeList;

		public void AddOpenNode(AStarNode node)
		{
			if (node == null || _openNodeList.Exists((AStarNode n) => n.AreaId == node.AreaId))
			{
				return;
			}
			bool flag = false;
			for (int num = 0; num < _openNodeList.Count; num++)
			{
				if (_openNodeList[num].FValue > node.FValue)
				{
					_openNodeList.Insert(num, node);
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				_openNodeList.Add(node);
			}
		}

		public void Sort()
		{
			_openNodeList?.Sort(delegate(AStarNode left, AStarNode right)
			{
				if (left.FValue != right.FValue)
				{
					return left.FValue - right.FValue;
				}
				return (left.HValue != right.HValue) ? (left.HValue - right.HValue) : (left.GValue - right.GValue);
			});
		}

		public AStarNode GetNodeByLocation(short areaId)
		{
			for (int i = 0; i < _openNodeList.Count; i++)
			{
				if (_openNodeList[i].AreaId == areaId)
				{
					return _openNodeList[i];
				}
			}
			return null;
		}

		public AStarNode GetBestOpenNode()
		{
			if (_openNodeList.Count <= 0)
			{
				return null;
			}
			AStarNode aStarNode = _openNodeList[0];
			_openNodeList.Remove(aStarNode);
			return aStarNode;
		}

		public void GetReady()
		{
			if (_openNodeList == null)
			{
				_openNodeList = new List<AStarNode>();
			}
			else
			{
				_openNodeList.Clear();
			}
		}
	}

	private Func<short, short, short> _getMoveCost;

	private OpenNodeHandler _openNodeHandler;

	private List<AStarNode> _closedList;

	private short _start;

	private short _end;

	public void InitMap(Func<short, short, short> getCostFunc)
	{
		_getMoveCost = getCostFunc;
		if (_openNodeHandler == null)
		{
			_openNodeHandler = new OpenNodeHandler();
		}
	}

	public void FindWay(short start, short end, ref List<short> path)
	{
		if (_closedList == null)
		{
			_closedList = new List<AStarNode>();
		}
		else
		{
			_closedList.Clear();
		}
		_openNodeHandler.GetReady();
		AStarNode aStarNode = new AStarNode();
		aStarNode.AreaId = start;
		aStarNode.GValue = 0;
		aStarNode.HValue = 0;
		aStarNode.FValue = aStarNode.HValue;
		_closedList.Add(aStarNode);
		if (start == end)
		{
			return;
		}
		_start = aStarNode.AreaId;
		_end = end;
		bool flag = false;
		List<AStarNode> nodeAround = GetNodeAround(aStarNode);
		foreach (AStarNode item in nodeAround)
		{
			if (item.AreaId != _end)
			{
				_openNodeHandler.AddOpenNode(item);
				continue;
			}
			_closedList.Add(item);
			flag = true;
			break;
		}
		while (!flag)
		{
			AStarNode bestOpenNode = _openNodeHandler.GetBestOpenNode();
			if (bestOpenNode == null)
			{
				return;
			}
			_closedList.Add(bestOpenNode);
			List<AStarNode> nodeAround2 = GetNodeAround(bestOpenNode);
			foreach (AStarNode item2 in nodeAround2)
			{
				if (item2.AreaId != _end)
				{
					_openNodeHandler.AddOpenNode(item2);
					continue;
				}
				_closedList.Add(item2);
				flag = true;
				break;
			}
		}
		ConvertResult(ref path);
	}

	private List<AStarNode> GetNodeAround(AStarNode nowNode)
	{
		List<AStarNode> list = new List<AStarNode>();
		HashSet<short> neighborAreas = DomainManager.Map.GetElement_Areas(nowNode.AreaId).NeighborAreas;
		foreach (short item in neighborAreas)
		{
			CreateNode(item, nowNode, list);
		}
		return list;
	}

	private void CreateNode(short nodeLocation, AStarNode nowNode, List<AStarNode> nodeList)
	{
		if (_closedList.Exists((AStarNode node) => node.AreaId == nodeLocation))
		{
			return;
		}
		short num = _getMoveCost(nowNode.AreaId, nodeLocation);
		AStarNode nodeByLocation = _openNodeHandler.GetNodeByLocation(nodeLocation);
		if (nodeByLocation != null)
		{
			int num2 = nowNode.GValue + num;
			if (num2 < nodeByLocation.GValue)
			{
				nodeByLocation.GValue = num2;
				nodeByLocation.FValue = nodeByLocation.GValue + nodeByLocation.HValue;
				nodeByLocation.ParentNode = nowNode;
				_openNodeHandler.Sort();
				nodeList.Add(nodeByLocation);
			}
		}
		else
		{
			nodeByLocation = new AStarNode();
			nodeByLocation.AreaId = nodeLocation;
			nodeByLocation.GValue = nowNode.GValue + num;
			nodeByLocation.HValue = 0;
			nodeByLocation.FValue = nodeByLocation.GValue + nodeByLocation.HValue;
			nodeByLocation.ParentNode = nowNode;
			nodeList.Add(nodeByLocation);
		}
	}

	private void ConvertResult(ref List<short> path)
	{
		path.Clear();
		if (_closedList.Count <= 0)
		{
			return;
		}
		AStarNode aStarNode = _closedList[_closedList.Count - 1];
		while (true)
		{
			path.Add(aStarNode.AreaId);
			if (aStarNode.AreaId == _start)
			{
				break;
			}
			aStarNode = aStarNode.ParentNode;
			if (aStarNode == null)
			{
				break;
			}
			bool flag = true;
		}
		path.Reverse();
	}
}
