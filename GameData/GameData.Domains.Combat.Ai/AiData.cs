using System;
using System.Collections.Generic;
using Config;

namespace GameData.Domains.Combat.Ai;

public abstract class AiData
{
	private int _entryPoint;

	private AiContext? _updatingContext;

	protected abstract IReadOnlyList<IAiNode> Nodes { get; }

	protected abstract IReadOnlyList<IAiCondition> Conditions { get; }

	protected abstract IReadOnlyList<IAiAction> Actions { get; }

	public void Update(AiMemoryNew memory, IAiParticipant participant)
	{
		int num = ((_entryPoint >= 0 && _entryPoint < Nodes.Count) ? _entryPoint : 0);
		_entryPoint = 0;
		if (num >= 0 && num < Nodes.Count && !_updatingContext.HasValue && memory != null && participant != null)
		{
			_updatingContext = (memory: memory, participant: participant);
			try
			{
				Update(num);
			}
			catch (Exception ex)
			{
				PredefinedLog.Show(8, "AiData.Update catch exception " + ex.Message + "\nstacktrace:\n" + ex.StackTrace);
			}
			_updatingContext = null;
		}
	}

	private bool Update(int nodeId, int depth = 0)
	{
		if (nodeId < 0 || nodeId >= Nodes.Count)
		{
			return false;
		}
		if (depth > 1000)
		{
			PredefinedLog.Show(8, $"AiData.Update depth overflow {nodeId}");
			return true;
		}
		if (!_updatingContext.HasValue)
		{
			return false;
		}
		foreach (int item in Nodes[nodeId].Update(this))
		{
			if (Update(item, depth + 1))
			{
				return true;
			}
		}
		return Nodes[nodeId].IsAction;
	}

	public void Reset()
	{
		_entryPoint = 0;
	}

	public bool CheckCondition(int conditionId)
	{
		if (conditionId < 0 || conditionId >= Conditions.Count)
		{
			return false;
		}
		if (!_updatingContext.HasValue)
		{
			return false;
		}
		return Conditions[conditionId].Check(_updatingContext.Value.Memory, _updatingContext.Value.Participant);
	}

	public bool ExecuteAction(int actionId)
	{
		if (actionId < 0 || actionId >= Actions.Count)
		{
			return false;
		}
		if (!_updatingContext.HasValue)
		{
			return false;
		}
		Actions[actionId].Execute(_updatingContext.Value.Memory, _updatingContext.Value.Participant);
		return true;
	}

	public void RelayEntry(int nodeId)
	{
		if (nodeId >= 0 && nodeId < Nodes.Count)
		{
			_entryPoint = nodeId;
		}
	}
}
