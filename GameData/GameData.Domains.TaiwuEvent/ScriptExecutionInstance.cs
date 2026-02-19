using System;
using System.Collections;
using System.Collections.Generic;
using CompDevLib.Interpreter;
using GameData.Domains.Character;
using GameData.Domains.Item;
using GameData.Domains.Item.Display;
using GameData.Domains.TaiwuEvent.EventHelper;
using GameData.GameDataBridge;
using GameData.Utilities;

namespace GameData.Domains.TaiwuEvent;

public class ScriptExecutionInstance
{
	public enum ScopeType
	{
		If,
		Loop
	}

	private struct Scope
	{
		public readonly int BeginIndex;

		public readonly int ContentIndent;

		public readonly ScopeType Type;

		public Scope(int beginIndex, int contentIndent, ScopeType type)
		{
			BeginIndex = beginIndex;
			ContentIndent = contentIndent;
			Type = type;
		}
	}

	private struct Branch
	{
		public readonly int BeginIndex;

		public readonly int Indent;

		public readonly bool IsEntered;

		public Branch(int beginIndex, int indent, bool isEntered)
		{
			BeginIndex = beginIndex;
			Indent = indent;
			IsEntered = isEntered;
		}
	}

	private IEnumerator _enumerator;

	private EventScript _currentScript;

	private EventArgBox _argBox;

	private int _executingIndex;

	private int _nextIndex;

	private readonly Stack<Scope> _scopeStack;

	private readonly Stack<Branch> _executedBranchStack;

	private readonly Dictionary<int, int> _loopCounts;

	private EventScriptDebugInfo _debugInfo;

	private bool _logExecution;

	public string NextEvent;

	private List<short> _selectItemSubTypes;

	private List<(sbyte itemType, short itemTemplateId)> _selectItemTemplateIds;

	private List<(sbyte itemType, short itemTemplateId)> _excludeItemTemplateIds;

	private Inventory _toShowGetItems;

	private int[] _toShowGetResources;

	public int ExecutingIndex => _executingIndex;

	public bool IsScriptMonitored => _debugInfo != null;

	public EventArgBox ArgBox => _argBox;

	public bool IsRunning => _enumerator != null;

	public EventScriptId ScriptId => _currentScript.Id;

	private bool HasToShowGetItems => _toShowGetItems != null && _toShowGetItems.Items.Count > 0;

	private bool HasToShowGetResources => _toShowGetResources != null && _toShowGetResources.Sum() > 0;

	public ScriptExecutionInstance()
	{
		_scopeStack = new Stack<Scope>();
		_executedBranchStack = new Stack<Branch>();
		_loopCounts = new Dictionary<int, int>();
		NextEvent = null;
	}

	public void Update(EventScriptRuntime runtime)
	{
		if (_enumerator == null)
		{
			return;
		}
		try
		{
			if (!_enumerator.MoveNext())
			{
				_enumerator = null;
			}
		}
		catch (Exception value)
		{
			_enumerator = null;
			string message = $"Exception occurred when executing script. \n{value}";
			runtime.Debugger?.LogError(message);
			AdaptableLog.TagWarning("ExecuteScript", message, appendWarningMessage: true);
		}
	}

	public void ExecuteScript(EventScriptRuntime runtime, EventScript script, EventArgBox argBox, EventScriptDebugInfo debugInfo = null)
	{
		_currentScript = script;
		_argBox = argBox;
		_scopeStack.Clear();
		_executedBranchStack.Clear();
		_loopCounts.Clear();
		NextEvent = null;
		_debugInfo = debugInfo;
		_logExecution = runtime.LogScriptExecution(script.Id);
		if (_logExecution)
		{
			runtime.Debugger.LogScriptInfo(script.Id);
		}
		if (debugInfo != null && debugInfo.PauseOnStart)
		{
			runtime.IsPaused = true;
		}
		_enumerator = Execute(runtime);
		Update(runtime);
	}

	private IEnumerator Execute(EventScriptRuntime runtime)
	{
		EvaluationStack evaluationStack = runtime.Evaluator.EvaluationStack;
		_executingIndex = 0;
		for (_nextIndex = 1; _currentScript != null && _executingIndex < _currentScript.Instructions.Length; _executingIndex = _nextIndex, _nextIndex++)
		{
			EventInstruction instruction = _currentScript.Instructions[_executingIndex];
			if (_scopeStack.TryPeek(out var scope))
			{
				if (scope.ContentIndent > instruction.Indent)
				{
					if (scope.Type == ScopeType.Loop)
					{
						GotoLoopHead();
						continue;
					}
					_scopeStack.Pop();
				}
				else if (scope.ContentIndent < instruction.Indent)
				{
					throw new TaiwuEventScriptException("Unrecognized indention", _currentScript.Id, _executingIndex, instruction, null);
				}
			}
			if (!runtime.MovingNext)
			{
				while (runtime.IsPaused && !runtime.MovingNext)
				{
					yield return null;
				}
				if (_debugInfo != null && _debugInfo.BreakPoints.TryGetValue(_executingIndex, out var breakPointCondition))
				{
					bool pause = breakPointCondition?.Invoke() ?? true;
					while (pause && !runtime.MovingNext)
					{
						yield return null;
					}
				}
				breakPointCondition = null;
			}
			else
			{
				runtime.MovingNext = false;
			}
			try
			{
				if (_logExecution)
				{
					runtime.Debugger.LogInstructionWithArgs(_executingIndex, instruction);
				}
				ValueInfo valueInfo = instruction.Instruction.Execute(runtime);
				Branch branch;
				while (_executedBranchStack.TryPeek(out branch) && branch.BeginIndex != _executingIndex && branch.Indent >= instruction.Indent)
				{
					_executedBranchStack.Pop();
				}
				if ((int)valueInfo.ValueType > 0)
				{
					if (_logExecution)
					{
						runtime.Debugger.LogInstructionReturn(instruction.AssignToVar, valueInfo);
					}
					_argBox.SetValueFromStack(evaluationStack, instruction.AssignToVar, valueInfo.ValueType);
				}
				else if (!string.IsNullOrEmpty(instruction.AssignToVar))
				{
					throw new Exception("Failed to get return value and assign to \"" + instruction.AssignToVar + "\"");
				}
			}
			catch (Exception innerException)
			{
				throw new TaiwuEventScriptException("Failed to execute instruction", _currentScript.Id, _executingIndex, instruction, innerException);
			}
		}
		try
		{
			UpdateShowGetItems();
		}
		catch (Exception ex)
		{
			Exception e = ex;
			throw new TaiwuEventScriptException("Failed to display get items", ScriptId, _executingIndex, string.Empty, e);
		}
	}

	public void GotoLabel(string label)
	{
		_nextIndex = _currentScript.Labels[label];
	}

	public void ExecuteBranch(int indent, bool executed)
	{
		if (_executedBranchStack.TryPeek(out var result) && result.Indent == indent)
		{
			_executedBranchStack.Pop();
		}
		_executedBranchStack.Push(new Branch(_executingIndex, indent, executed));
	}

	public bool IsBranchEntered(int indent)
	{
		Branch result;
		return _executedBranchStack.TryPeek(out result) && result.Indent == indent && result.IsEntered;
	}

	public void ExitBranch(int beginIndex)
	{
		Branch result;
		while (_executedBranchStack.TryPeek(out result) && result.BeginIndex >= beginIndex)
		{
			_executedBranchStack.Pop();
		}
	}

	public void BreakLoop()
	{
		Scope result;
		while (_scopeStack.TryPop(out result))
		{
			if (result.Type != ScopeType.Loop)
			{
				continue;
			}
			ExitScope(result.ContentIndent);
			_loopCounts.Remove(result.BeginIndex);
			Branch result2;
			while (_executedBranchStack.TryPeek(out result2) && result2.BeginIndex >= result.BeginIndex)
			{
				_executedBranchStack.Pop();
			}
			return;
		}
		throw new Exception("Currently not in loop");
	}

	public void CheckAndAdvanceIteration(int maxCount)
	{
		_loopCounts.TryGetValue(_executingIndex, out var value);
		if (value >= maxCount)
		{
			BreakLoop();
		}
		else
		{
			_loopCounts[_executingIndex] = value + 1;
		}
	}

	public void CheckAndAdvanceIteration(bool condition)
	{
		if (!condition)
		{
			BreakLoop();
		}
	}

	public void GotoLoopHead()
	{
		Scope result;
		while (_scopeStack.TryPop(out result))
		{
			if (result.Type != ScopeType.Loop)
			{
				_scopeStack.Pop();
				continue;
			}
			_nextIndex = result.BeginIndex;
			Branch result2;
			while (_executedBranchStack.TryPeek(out result2) && result2.BeginIndex >= _nextIndex)
			{
				_executedBranchStack.Pop();
			}
			return;
		}
		throw new Exception("Currently not in loop");
	}

	public int GetCurrentIndentAmount()
	{
		Scope result;
		return _scopeStack.TryPeek(out result) ? result.ContentIndent : 0;
	}

	public void EnterScope(ScopeType scopeType)
	{
		int currentIndentAmount = GetCurrentIndentAmount();
		_scopeStack.Push(new Scope(_executingIndex, currentIndentAmount + 1, scopeType));
	}

	public void ExitScope(int indentAmount)
	{
		if (indentAmount <= 0)
		{
			throw new ArgumentException($"Unable to exit scope with contentIndent {indentAmount}.");
		}
		while (_nextIndex < _currentScript.Instructions.Length)
		{
			EventInstruction eventInstruction = _currentScript.Instructions[_nextIndex];
			if (eventInstruction.Indent < indentAmount)
			{
				break;
			}
			_nextIndex++;
		}
		Scope result;
		while (_scopeStack.TryPeek(out result) && result.ContentIndent >= indentAmount)
		{
			if (result.Type == ScopeType.Loop)
			{
				_loopCounts.Remove(result.BeginIndex);
			}
			_scopeStack.Pop();
		}
	}

	public void ExitScript()
	{
		_nextIndex = _currentScript.Instructions.Length;
		_currentScript = null;
	}

	public void RegisterToSelectItemSubTypes(short itemSubType)
	{
		if (_selectItemSubTypes == null)
		{
			_selectItemSubTypes = new List<short>();
		}
		_selectItemSubTypes.Add(itemSubType);
	}

	public void RegisterToSelectItemTemplateIds(sbyte itemType, short itemTemplateId)
	{
		if (_selectItemTemplateIds == null)
		{
			_selectItemTemplateIds = new List<(sbyte, short)>();
		}
		_selectItemTemplateIds.Add((itemType, itemTemplateId));
	}

	public void RegisterToExcludeItemTemplateIds(sbyte itemType, short itemTemplateId)
	{
		if (_excludeItemTemplateIds == null)
		{
			_excludeItemTemplateIds = new List<(sbyte, short)>();
		}
		_excludeItemTemplateIds.Add((itemType, itemTemplateId));
	}

	public void FilterItemForCharacter(GameData.Domains.Character.Character character, string selectItemNameKey, bool includeTransferable = false)
	{
		List<Predicate<ItemKey>> list = new List<Predicate<ItemKey>>();
		list.Add(delegate(ItemKey itemKey)
		{
			if (_selectItemTemplateIds != null && _selectItemTemplateIds.Contains((itemKey.ItemType, itemKey.TemplateId)))
			{
				return true;
			}
			if (_excludeItemTemplateIds != null && _excludeItemTemplateIds.Contains((itemKey.ItemType, itemKey.TemplateId)))
			{
				return false;
			}
			short itemSubType = ItemTemplateHelper.GetItemSubType(itemKey.ItemType, itemKey.TemplateId);
			return _selectItemSubTypes != null && _selectItemSubTypes.Contains(itemSubType);
		});
		GameData.Domains.TaiwuEvent.EventHelper.EventHelper.FilterItemForCharacterByType(character.GetId(), selectItemNameKey, ArgBox, -1, -1, includeTransferable, list);
	}

	public void RegisterToShowGetItem(ItemKey itemKey, int amount)
	{
		if (itemKey.IsValid())
		{
			if (_toShowGetItems == null)
			{
				_toShowGetItems = new Inventory();
			}
			if (_toShowGetItems.Items.TryGetValue(itemKey, out var value))
			{
				_toShowGetItems.Items[itemKey] = value + amount;
			}
			else
			{
				_toShowGetItems.Items.Add(itemKey, amount);
			}
		}
	}

	public void RegisterToShowGetResource(sbyte resourceType, int amount)
	{
		if (_toShowGetResources == null)
		{
			_toShowGetResources = new int[8];
		}
		_toShowGetResources[resourceType] += amount;
	}

	private void UpdateShowGetItems()
	{
		if (!HasToShowGetItems && !HasToShowGetResources)
		{
			return;
		}
		if (!string.IsNullOrEmpty(NextEvent))
		{
			DomainManager.TaiwuEvent.SetListenerWithActionName(NextEvent, ArgBox, "GetItemShowed");
			DomainManager.TaiwuEvent.CheckTaiwuStatusImmediately();
			NextEvent = null;
		}
		List<ItemDisplayData> list = DomainManager.Item.GetItemDisplayDataListOptionalFromInventory(_toShowGetItems, -1, -1);
		if (_toShowGetResources != null)
		{
			if (list == null)
			{
				list = new List<ItemDisplayData>();
			}
			for (sbyte b = 0; b < 8; b++)
			{
				int num = _toShowGetResources[b];
				if (num > 0)
				{
					list.Add(new ItemDisplayData(12, (short)(321 + b))
					{
						Amount = num
					});
				}
			}
		}
		GameData.GameDataBridge.GameDataBridge.AddDisplayEvent(DisplayEventType.OpenGetItem_Item, list, arg2: false, arg3: true);
		_toShowGetItems?.Items?.Clear();
		_toShowGetResources = null;
	}
}
