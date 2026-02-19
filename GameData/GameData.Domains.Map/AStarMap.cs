using System;
using System.Collections.Generic;
using GameData.Utilities;

namespace GameData.Domains.Map;

public class AStarMap
{
	private class AStarNode
	{
		public ByteCoordinate Location;

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
			if (node == null)
			{
				return;
			}
			if (_openNodeList.Count <= 0)
			{
				_openNodeList.Add(node);
			}
			else
			{
				if (_openNodeList.Find((AStarNode e) => e.Location == node.Location) != null)
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

		public AStarNode GetNodeByLocation(ByteCoordinate location)
		{
			for (int i = 0; i < _openNodeList.Count; i++)
			{
				if (_openNodeList[i].Location == location)
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

	private short[,] _mapMoveData;

	private int _mapWidth;

	private int _mapHeight;

	private Func<ByteCoordinate, sbyte> _getMoveCost;

	private OpenNodeHandler _openNodeHandler;

	private List<AStarNode> _closedList;

	private ByteCoordinate _start;

	private ByteCoordinate _end;

	public void InitMap(int mapWidth, int mapHeight, Func<ByteCoordinate, sbyte> getCostFunc)
	{
		_getMoveCost = getCostFunc;
		_mapWidth = mapWidth;
		_mapHeight = mapHeight;
		_mapMoveData = new short[mapWidth, mapHeight];
		if (_openNodeHandler == null)
		{
			_openNodeHandler = new OpenNodeHandler();
		}
	}

	public void FindWay(ByteCoordinate start, ByteCoordinate end, ref List<ByteCoordinate> path, List<ByteCoordinate> avoidPosList = null)
	{
		if (_closedList == null)
		{
			_closedList = new List<AStarNode>();
		}
		else
		{
			_closedList.Clear();
		}
		if (!IsLocationValid(start) || !IsLocationValid(end))
		{
			return;
		}
		if (avoidPosList != null)
		{
			for (int i = 0; i < avoidPosList.Count; i++)
			{
				ByteCoordinate byteCoordinate = avoidPosList[i];
				_mapMoveData[byteCoordinate.X, byteCoordinate.Y] = 1000;
			}
		}
		if (_mapMoveData[end.X, end.Y] <= 0)
		{
			_mapMoveData[end.X, end.Y] = _getMoveCost(end);
		}
		if (-1 == _mapMoveData[end.X, end.Y])
		{
			return;
		}
		_openNodeHandler.GetReady();
		AStarNode aStarNode = new AStarNode();
		aStarNode.Location = start;
		aStarNode.GValue = 0;
		aStarNode.HValue = start.GetManhattanDistance(end);
		aStarNode.FValue = aStarNode.HValue;
		_closedList.Add(aStarNode);
		if (start == end)
		{
			return;
		}
		_start = aStarNode.Location;
		_end = end;
		List<ByteCoordinate> list = ObjectPool<List<ByteCoordinate>>.Instance.Get();
		list.Clear();
		bool flag = false;
		List<AStarNode> nodeAround = GetNodeAround(aStarNode, end);
		foreach (AStarNode item in nodeAround)
		{
			bool flag2 = list.Contains(item.Location);
			if (item.Location != _end && !flag2)
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
			List<AStarNode> nodeAround2 = GetNodeAround(bestOpenNode, end);
			foreach (AStarNode item2 in nodeAround2)
			{
				bool flag3 = list.Contains(item2.Location);
				if (item2.Location != _end && !flag3)
				{
					_openNodeHandler.AddOpenNode(item2);
					continue;
				}
				_closedList.Add(item2);
				flag = true;
				break;
			}
		}
		ObjectPool<List<ByteCoordinate>>.Instance.Return(list);
		ConvertResult(ref path);
	}

	private List<AStarNode> GetNodeAround(AStarNode nowNode, ByteCoordinate end)
	{
		List<AStarNode> list = new List<AStarNode>();
		List<ByteCoordinate> list2 = ObjectPool<List<ByteCoordinate>>.Instance.Get();
		list2.Clear();
		if (nowNode.Location.X > 0)
		{
			list2.Add(new ByteCoordinate((byte)(nowNode.Location.X - 1), nowNode.Location.Y));
		}
		if (nowNode.Location.X < _mapWidth - 1)
		{
			list2.Add(new ByteCoordinate((byte)(nowNode.Location.X + 1), nowNode.Location.Y));
		}
		if (nowNode.Location.Y > 0)
		{
			list2.Add(new ByteCoordinate(nowNode.Location.X, (byte)(nowNode.Location.Y - 1)));
		}
		if (nowNode.Location.Y < _mapHeight - 1)
		{
			list2.Add(new ByteCoordinate(nowNode.Location.X, (byte)(nowNode.Location.Y + 1)));
		}
		for (int i = 0; i < list2.Count; i++)
		{
			CreateNode(list2[i], nowNode, list, end);
		}
		ObjectPool<List<ByteCoordinate>>.Instance.Return(list2);
		return list;
	}

	private void CreateNode(ByteCoordinate nodeLocation, AStarNode nowNode, List<AStarNode> nodeList, ByteCoordinate end)
	{
		if (!IsLocationValid(nodeLocation))
		{
			return;
		}
		if (_mapMoveData[nodeLocation.X, nodeLocation.Y] <= 0)
		{
			_mapMoveData[nodeLocation.X, nodeLocation.Y] = _getMoveCost(nodeLocation);
		}
		if (-1 == _mapMoveData[nodeLocation.X, nodeLocation.Y])
		{
			return;
		}
		for (int i = 0; i < _closedList.Count; i++)
		{
			if (_closedList[i].Location == nodeLocation)
			{
				return;
			}
		}
		AStarNode nodeByLocation = _openNodeHandler.GetNodeByLocation(nodeLocation);
		if (nodeByLocation != null)
		{
			int num = nowNode.GValue + _mapMoveData[nodeLocation.X, nodeLocation.Y];
			if (num < nodeByLocation.GValue)
			{
				nodeByLocation.GValue = num;
				nodeByLocation.FValue = nodeByLocation.GValue + nodeByLocation.HValue;
				nodeByLocation.ParentNode = nowNode;
				_openNodeHandler.Sort();
				nodeList.Add(nodeByLocation);
			}
		}
		else
		{
			nodeByLocation = new AStarNode();
			nodeByLocation.Location = nodeLocation;
			nodeByLocation.GValue = nowNode.GValue + _mapMoveData[nodeLocation.X, nodeLocation.Y];
			nodeByLocation.HValue = end.GetManhattanDistance(nodeLocation);
			nodeByLocation.FValue = nodeByLocation.GValue + nodeByLocation.HValue;
			nodeByLocation.ParentNode = nowNode;
			nodeList.Add(nodeByLocation);
		}
	}

	private bool IsLocationValid(ByteCoordinate pos)
	{
		return pos.X >= 0 && pos.X < _mapWidth && pos.Y >= 0 && pos.Y < _mapHeight;
	}

	private void ConvertResult(ref List<ByteCoordinate> path)
	{
		path.Clear();
		if (_closedList.Count <= 0)
		{
			return;
		}
		AStarNode aStarNode = _closedList[_closedList.Count - 1];
		while (true)
		{
			path.Add(aStarNode.Location);
			if (aStarNode.Location == _start)
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
