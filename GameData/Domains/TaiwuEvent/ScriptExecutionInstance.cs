using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using CompDevLib.Interpreter;
using CompDevLib.Interpreter.Parse;
using GameData.Domains.Character;
using GameData.Domains.Item;
using GameData.Domains.Item.Display;
using GameData.Domains.TaiwuEvent.EventHelper;
using GameData.GameDataBridge;
using GameData.Utilities;

namespace GameData.Domains.TaiwuEvent
{
	// Token: 0x0200007E RID: 126
	public class ScriptExecutionInstance
	{
		// Token: 0x170000B7 RID: 183
		// (get) Token: 0x060016FB RID: 5883 RVA: 0x00154DBB File Offset: 0x00152FBB
		public int ExecutingIndex
		{
			get
			{
				return this._executingIndex;
			}
		}

		// Token: 0x170000B8 RID: 184
		// (get) Token: 0x060016FC RID: 5884 RVA: 0x00154DC3 File Offset: 0x00152FC3
		public bool IsScriptMonitored
		{
			get
			{
				return this._debugInfo != null;
			}
		}

		// Token: 0x170000B9 RID: 185
		// (get) Token: 0x060016FD RID: 5885 RVA: 0x00154DCE File Offset: 0x00152FCE
		public EventArgBox ArgBox
		{
			get
			{
				return this._argBox;
			}
		}

		// Token: 0x170000BA RID: 186
		// (get) Token: 0x060016FE RID: 5886 RVA: 0x00154DD6 File Offset: 0x00152FD6
		public bool IsRunning
		{
			get
			{
				return this._enumerator != null;
			}
		}

		// Token: 0x170000BB RID: 187
		// (get) Token: 0x060016FF RID: 5887 RVA: 0x00154DE1 File Offset: 0x00152FE1
		public EventScriptId ScriptId
		{
			get
			{
				return this._currentScript.Id;
			}
		}

		// Token: 0x06001700 RID: 5888 RVA: 0x00154DEE File Offset: 0x00152FEE
		public ScriptExecutionInstance()
		{
			this._scopeStack = new Stack<ScriptExecutionInstance.Scope>();
			this._executedBranchStack = new Stack<ScriptExecutionInstance.Branch>();
			this._loopCounts = new Dictionary<int, int>();
			this.NextEvent = null;
		}

		// Token: 0x06001701 RID: 5889 RVA: 0x00154E20 File Offset: 0x00153020
		public void Update(EventScriptRuntime runtime)
		{
			bool flag = this._enumerator == null;
			if (!flag)
			{
				try
				{
					bool flag2 = !this._enumerator.MoveNext();
					if (flag2)
					{
						this._enumerator = null;
					}
				}
				catch (Exception e)
				{
					this._enumerator = null;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(43, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Exception occurred when executing script. \n");
					defaultInterpolatedStringHandler.AppendFormatted<Exception>(e);
					string errorMessage = defaultInterpolatedStringHandler.ToStringAndClear();
					EventScriptDebugger debugger = runtime.Debugger;
					if (debugger != null)
					{
						debugger.LogError(errorMessage);
					}
					AdaptableLog.TagWarning("ExecuteScript", errorMessage, true);
				}
			}
		}

		// Token: 0x06001702 RID: 5890 RVA: 0x00154EC0 File Offset: 0x001530C0
		public void ExecuteScript(EventScriptRuntime runtime, EventScript script, EventArgBox argBox, EventScriptDebugInfo debugInfo = null)
		{
			this._currentScript = script;
			this._argBox = argBox;
			this._scopeStack.Clear();
			this._executedBranchStack.Clear();
			this._loopCounts.Clear();
			this.NextEvent = null;
			this._debugInfo = debugInfo;
			this._logExecution = runtime.LogScriptExecution(script.Id);
			bool logExecution = this._logExecution;
			if (logExecution)
			{
				runtime.Debugger.LogScriptInfo(script.Id);
			}
			bool flag = debugInfo != null && debugInfo.PauseOnStart;
			if (flag)
			{
				runtime.IsPaused = true;
			}
			this._enumerator = this.Execute(runtime);
			this.Update(runtime);
		}

		// Token: 0x06001703 RID: 5891 RVA: 0x00154F6B File Offset: 0x0015316B
		private IEnumerator Execute(EventScriptRuntime runtime)
		{
			EvaluationStack evaluationStack = runtime.Evaluator.EvaluationStack;
			this._executingIndex = 0;
			this._nextIndex = 1;
			while (this._currentScript != null && this._executingIndex < this._currentScript.Instructions.Length)
			{
				EventInstruction instruction = this._currentScript.Instructions[this._executingIndex];
				ScriptExecutionInstance.Scope scope;
				bool flag = this._scopeStack.TryPeek(out scope);
				if (flag)
				{
					bool flag2 = scope.ContentIndent > instruction.Indent;
					if (flag2)
					{
						bool flag3 = scope.Type == ScriptExecutionInstance.ScopeType.Loop;
						if (flag3)
						{
							this.GotoLoopHead();
							goto IL_400;
						}
						this._scopeStack.Pop();
					}
					else
					{
						bool flag4 = scope.ContentIndent < instruction.Indent;
						if (flag4)
						{
							throw new TaiwuEventScriptException("Unrecognized indention", this._currentScript.Id, this._executingIndex, instruction, null);
						}
					}
					goto IL_146;
				}
				goto IL_146;
				IL_400:
				this._executingIndex = this._nextIndex;
				this._nextIndex++;
				continue;
				IL_146:
				bool flag5 = !runtime.MovingNext;
				if (flag5)
				{
					while (runtime.IsPaused && !runtime.MovingNext)
					{
						yield return null;
					}
					Func<bool> breakPointCondition;
					bool flag6 = this._debugInfo != null && this._debugInfo.BreakPoints.TryGetValue(this._executingIndex, out breakPointCondition);
					if (flag6)
					{
						bool pause = breakPointCondition == null || breakPointCondition();
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
					bool logExecution = this._logExecution;
					if (logExecution)
					{
						runtime.Debugger.LogInstructionWithArgs(this._executingIndex, instruction);
					}
					ValueInfo valueInfo = instruction.Instruction.Execute(runtime);
					for (;;)
					{
						ScriptExecutionInstance.Branch branch;
						bool flag7 = this._executedBranchStack.TryPeek(out branch) && branch.BeginIndex != this._executingIndex && branch.Indent >= instruction.Indent;
						if (!flag7)
						{
							break;
						}
						this._executedBranchStack.Pop();
					}
					bool flag8 = valueInfo.ValueType > EValueType.Void;
					if (flag8)
					{
						bool logExecution2 = this._logExecution;
						if (logExecution2)
						{
							runtime.Debugger.LogInstructionReturn(instruction.AssignToVar, valueInfo);
						}
						this._argBox.SetValueFromStack(evaluationStack, instruction.AssignToVar, valueInfo.ValueType);
					}
					else
					{
						bool flag9 = !string.IsNullOrEmpty(instruction.AssignToVar);
						if (flag9)
						{
							throw new Exception("Failed to get return value and assign to \"" + instruction.AssignToVar + "\"");
						}
					}
					valueInfo = default(ValueInfo);
				}
				catch (Exception ex)
				{
					Exception e = ex;
					throw new TaiwuEventScriptException("Failed to execute instruction", this._currentScript.Id, this._executingIndex, instruction, e);
				}
				instruction = null;
				goto IL_400;
			}
			try
			{
				this.UpdateShowGetItems();
				yield break;
			}
			catch (Exception ex)
			{
				Exception e2 = ex;
				throw new TaiwuEventScriptException("Failed to display get items", this.ScriptId, this._executingIndex, string.Empty, e2);
			}
			yield break;
		}

		// Token: 0x06001704 RID: 5892 RVA: 0x00154F81 File Offset: 0x00153181
		public void GotoLabel(string label)
		{
			this._nextIndex = this._currentScript.Labels[label];
		}

		// Token: 0x06001705 RID: 5893 RVA: 0x00154F9C File Offset: 0x0015319C
		public void ExecuteBranch(int indent, bool executed)
		{
			ScriptExecutionInstance.Branch branch;
			bool flag = this._executedBranchStack.TryPeek(out branch) && branch.Indent == indent;
			if (flag)
			{
				this._executedBranchStack.Pop();
			}
			this._executedBranchStack.Push(new ScriptExecutionInstance.Branch(this._executingIndex, indent, executed));
		}

		// Token: 0x06001706 RID: 5894 RVA: 0x00154FF0 File Offset: 0x001531F0
		public bool IsBranchEntered(int indent)
		{
			ScriptExecutionInstance.Branch executedBranch;
			return this._executedBranchStack.TryPeek(out executedBranch) && executedBranch.Indent == indent && executedBranch.IsEntered;
		}

		// Token: 0x06001707 RID: 5895 RVA: 0x00155024 File Offset: 0x00153224
		public void ExitBranch(int beginIndex)
		{
			for (;;)
			{
				ScriptExecutionInstance.Branch branch;
				bool flag = this._executedBranchStack.TryPeek(out branch) && branch.BeginIndex >= beginIndex;
				if (!flag)
				{
					break;
				}
				this._executedBranchStack.Pop();
			}
		}

		// Token: 0x06001708 RID: 5896 RVA: 0x00155064 File Offset: 0x00153264
		public void BreakLoop()
		{
			ScriptExecutionInstance.Scope scope;
			bool flag2;
			do
			{
				bool flag = this._scopeStack.TryPop(out scope);
				if (!flag)
				{
					goto IL_82;
				}
				flag2 = (scope.Type != ScriptExecutionInstance.ScopeType.Loop);
			}
			while (flag2);
			this.ExitScope(scope.ContentIndent);
			this._loopCounts.Remove(scope.BeginIndex);
			for (;;)
			{
				ScriptExecutionInstance.Branch branch;
				bool flag3 = this._executedBranchStack.TryPeek(out branch);
				if (!flag3)
				{
					break;
				}
				bool flag4 = branch.BeginIndex >= scope.BeginIndex;
				if (!flag4)
				{
					break;
				}
				this._executedBranchStack.Pop();
			}
			return;
			IL_82:
			throw new Exception("Currently not in loop");
		}

		// Token: 0x06001709 RID: 5897 RVA: 0x00155100 File Offset: 0x00153300
		public void CheckAndAdvanceIteration(int maxCount)
		{
			int currIteration;
			this._loopCounts.TryGetValue(this._executingIndex, out currIteration);
			bool flag = currIteration >= maxCount;
			if (flag)
			{
				this.BreakLoop();
			}
			else
			{
				this._loopCounts[this._executingIndex] = currIteration + 1;
			}
		}

		// Token: 0x0600170A RID: 5898 RVA: 0x0015514C File Offset: 0x0015334C
		public void CheckAndAdvanceIteration(bool condition)
		{
			bool flag = !condition;
			if (flag)
			{
				this.BreakLoop();
			}
		}

		// Token: 0x0600170B RID: 5899 RVA: 0x0015516C File Offset: 0x0015336C
		public void GotoLoopHead()
		{
			ScriptExecutionInstance.Scope scope;
			for (;;)
			{
				bool flag = this._scopeStack.TryPop(out scope);
				if (!flag)
				{
					goto IL_7C;
				}
				bool flag2 = scope.Type != ScriptExecutionInstance.ScopeType.Loop;
				if (!flag2)
				{
					break;
				}
				this._scopeStack.Pop();
			}
			this._nextIndex = scope.BeginIndex;
			for (;;)
			{
				ScriptExecutionInstance.Branch branch;
				bool flag3 = this._executedBranchStack.TryPeek(out branch);
				if (!flag3)
				{
					break;
				}
				bool flag4 = branch.BeginIndex >= this._nextIndex;
				if (!flag4)
				{
					break;
				}
				this._executedBranchStack.Pop();
			}
			return;
			IL_7C:
			throw new Exception("Currently not in loop");
		}

		// Token: 0x0600170C RID: 5900 RVA: 0x00155200 File Offset: 0x00153400
		public int GetCurrentIndentAmount()
		{
			ScriptExecutionInstance.Scope scope;
			return this._scopeStack.TryPeek(out scope) ? scope.ContentIndent : 0;
		}

		// Token: 0x0600170D RID: 5901 RVA: 0x0015522C File Offset: 0x0015342C
		public void EnterScope(ScriptExecutionInstance.ScopeType scopeType)
		{
			int indent = this.GetCurrentIndentAmount();
			this._scopeStack.Push(new ScriptExecutionInstance.Scope(this._executingIndex, indent + 1, scopeType));
		}

		// Token: 0x0600170E RID: 5902 RVA: 0x0015525C File Offset: 0x0015345C
		public void ExitScope(int indentAmount)
		{
			bool flag = indentAmount <= 0;
			if (flag)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(41, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Unable to exit scope with contentIndent ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(indentAmount);
				defaultInterpolatedStringHandler.AppendLiteral(".");
				throw new ArgumentException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			while (this._nextIndex < this._currentScript.Instructions.Length)
			{
				EventInstruction nextInstruction = this._currentScript.Instructions[this._nextIndex];
				bool flag2 = nextInstruction.Indent < indentAmount;
				if (flag2)
				{
					break;
				}
				this._nextIndex++;
			}
			for (;;)
			{
				ScriptExecutionInstance.Scope scope;
				bool flag3 = this._scopeStack.TryPeek(out scope) && scope.ContentIndent >= indentAmount;
				if (!flag3)
				{
					break;
				}
				bool flag4 = scope.Type == ScriptExecutionInstance.ScopeType.Loop;
				if (flag4)
				{
					this._loopCounts.Remove(scope.BeginIndex);
				}
				this._scopeStack.Pop();
			}
		}

		// Token: 0x0600170F RID: 5903 RVA: 0x00155356 File Offset: 0x00153556
		public void ExitScript()
		{
			this._nextIndex = this._currentScript.Instructions.Length;
			this._currentScript = null;
		}

		// Token: 0x06001710 RID: 5904 RVA: 0x00155373 File Offset: 0x00153573
		public void RegisterToSelectItemSubTypes(short itemSubType)
		{
			if (this._selectItemSubTypes == null)
			{
				this._selectItemSubTypes = new List<short>();
			}
			this._selectItemSubTypes.Add(itemSubType);
		}

		// Token: 0x06001711 RID: 5905 RVA: 0x00155396 File Offset: 0x00153596
		public void RegisterToSelectItemTemplateIds(sbyte itemType, short itemTemplateId)
		{
			if (this._selectItemTemplateIds == null)
			{
				this._selectItemTemplateIds = new List<ValueTuple<sbyte, short>>();
			}
			this._selectItemTemplateIds.Add(new ValueTuple<sbyte, short>(itemType, itemTemplateId));
		}

		// Token: 0x06001712 RID: 5906 RVA: 0x001553BF File Offset: 0x001535BF
		public void RegisterToExcludeItemTemplateIds(sbyte itemType, short itemTemplateId)
		{
			if (this._excludeItemTemplateIds == null)
			{
				this._excludeItemTemplateIds = new List<ValueTuple<sbyte, short>>();
			}
			this._excludeItemTemplateIds.Add(new ValueTuple<sbyte, short>(itemType, itemTemplateId));
		}

		// Token: 0x06001713 RID: 5907 RVA: 0x001553E8 File Offset: 0x001535E8
		public void FilterItemForCharacter(Character character, string selectItemNameKey, bool includeTransferable = false)
		{
			List<Predicate<ItemKey>> predicates = new List<Predicate<ItemKey>>();
			predicates.Add(delegate(ItemKey itemKey)
			{
				bool flag = this._selectItemTemplateIds != null && this._selectItemTemplateIds.Contains(new ValueTuple<sbyte, short>(itemKey.ItemType, itemKey.TemplateId));
				bool result;
				if (flag)
				{
					result = true;
				}
				else
				{
					bool flag2 = this._excludeItemTemplateIds != null && this._excludeItemTemplateIds.Contains(new ValueTuple<sbyte, short>(itemKey.ItemType, itemKey.TemplateId));
					if (flag2)
					{
						result = false;
					}
					else
					{
						short itemSubType = ItemTemplateHelper.GetItemSubType(itemKey.ItemType, itemKey.TemplateId);
						result = (this._selectItemSubTypes != null && this._selectItemSubTypes.Contains(itemSubType));
					}
				}
				return result;
			});
			EventHelper.FilterItemForCharacterByType(character.GetId(), selectItemNameKey, this.ArgBox, -1, -1, includeTransferable, predicates);
		}

		// Token: 0x170000BC RID: 188
		// (get) Token: 0x06001714 RID: 5908 RVA: 0x00155426 File Offset: 0x00153626
		private bool HasToShowGetItems
		{
			get
			{
				return this._toShowGetItems != null && this._toShowGetItems.Items.Count > 0;
			}
		}

		// Token: 0x170000BD RID: 189
		// (get) Token: 0x06001715 RID: 5909 RVA: 0x00155446 File Offset: 0x00153646
		private bool HasToShowGetResources
		{
			get
			{
				return this._toShowGetResources != null && this._toShowGetResources.Sum() > 0;
			}
		}

		// Token: 0x06001716 RID: 5910 RVA: 0x00155464 File Offset: 0x00153664
		public void RegisterToShowGetItem(ItemKey itemKey, int amount)
		{
			bool flag = !itemKey.IsValid();
			if (!flag)
			{
				if (this._toShowGetItems == null)
				{
					this._toShowGetItems = new Inventory();
				}
				int oriAmount;
				bool flag2 = this._toShowGetItems.Items.TryGetValue(itemKey, out oriAmount);
				if (flag2)
				{
					this._toShowGetItems.Items[itemKey] = oriAmount + amount;
				}
				else
				{
					this._toShowGetItems.Items.Add(itemKey, amount);
				}
			}
		}

		// Token: 0x06001717 RID: 5911 RVA: 0x001554D6 File Offset: 0x001536D6
		public void RegisterToShowGetResource(sbyte resourceType, int amount)
		{
			if (this._toShowGetResources == null)
			{
				this._toShowGetResources = new int[8];
			}
			this._toShowGetResources[(int)resourceType] += amount;
		}

		// Token: 0x06001718 RID: 5912 RVA: 0x00155500 File Offset: 0x00153700
		private void UpdateShowGetItems()
		{
			bool flag = !this.HasToShowGetItems && !this.HasToShowGetResources;
			if (!flag)
			{
				bool flag2 = !string.IsNullOrEmpty(this.NextEvent);
				if (flag2)
				{
					DomainManager.TaiwuEvent.SetListenerWithActionName(this.NextEvent, this.ArgBox, "GetItemShowed");
					DomainManager.TaiwuEvent.CheckTaiwuStatusImmediately();
					this.NextEvent = null;
				}
				List<ItemDisplayData> itemDisplayDataList = DomainManager.Item.GetItemDisplayDataListOptionalFromInventory(this._toShowGetItems, -1, -1, false);
				bool flag3 = this._toShowGetResources != null;
				if (flag3)
				{
					if (itemDisplayDataList == null)
					{
						itemDisplayDataList = new List<ItemDisplayData>();
					}
					for (sbyte resourceType = 0; resourceType < 8; resourceType += 1)
					{
						int amount = this._toShowGetResources[(int)resourceType];
						bool flag4 = amount > 0;
						if (flag4)
						{
							itemDisplayDataList.Add(new ItemDisplayData(12, (short)(321 + (int)resourceType))
							{
								Amount = amount
							});
						}
					}
				}
				GameDataBridge.AddDisplayEvent<List<ItemDisplayData>, bool, bool>(DisplayEventType.OpenGetItem_Item, itemDisplayDataList, false, true);
				Inventory toShowGetItems = this._toShowGetItems;
				if (toShowGetItems != null)
				{
					Dictionary<ItemKey, int> items = toShowGetItems.Items;
					if (items != null)
					{
						items.Clear();
					}
				}
				this._toShowGetResources = null;
			}
		}

		// Token: 0x0400049D RID: 1181
		private IEnumerator _enumerator;

		// Token: 0x0400049E RID: 1182
		private EventScript _currentScript;

		// Token: 0x0400049F RID: 1183
		private EventArgBox _argBox;

		// Token: 0x040004A0 RID: 1184
		private int _executingIndex;

		// Token: 0x040004A1 RID: 1185
		private int _nextIndex;

		// Token: 0x040004A2 RID: 1186
		private readonly Stack<ScriptExecutionInstance.Scope> _scopeStack;

		// Token: 0x040004A3 RID: 1187
		private readonly Stack<ScriptExecutionInstance.Branch> _executedBranchStack;

		// Token: 0x040004A4 RID: 1188
		private readonly Dictionary<int, int> _loopCounts;

		// Token: 0x040004A5 RID: 1189
		private EventScriptDebugInfo _debugInfo;

		// Token: 0x040004A6 RID: 1190
		private bool _logExecution;

		// Token: 0x040004A7 RID: 1191
		public string NextEvent;

		// Token: 0x040004A8 RID: 1192
		private List<short> _selectItemSubTypes;

		// Token: 0x040004A9 RID: 1193
		[TupleElementNames(new string[]
		{
			"itemType",
			"itemTemplateId"
		})]
		private List<ValueTuple<sbyte, short>> _selectItemTemplateIds;

		// Token: 0x040004AA RID: 1194
		[TupleElementNames(new string[]
		{
			"itemType",
			"itemTemplateId"
		})]
		private List<ValueTuple<sbyte, short>> _excludeItemTemplateIds;

		// Token: 0x040004AB RID: 1195
		private Inventory _toShowGetItems;

		// Token: 0x040004AC RID: 1196
		private int[] _toShowGetResources;

		// Token: 0x02000992 RID: 2450
		public enum ScopeType
		{
			// Token: 0x0400284C RID: 10316
			If,
			// Token: 0x0400284D RID: 10317
			Loop
		}

		// Token: 0x02000993 RID: 2451
		private struct Scope
		{
			// Token: 0x060084EF RID: 34031 RVA: 0x004E55B7 File Offset: 0x004E37B7
			public Scope(int beginIndex, int contentIndent, ScriptExecutionInstance.ScopeType type)
			{
				this.BeginIndex = beginIndex;
				this.ContentIndent = contentIndent;
				this.Type = type;
			}

			// Token: 0x0400284E RID: 10318
			public readonly int BeginIndex;

			// Token: 0x0400284F RID: 10319
			public readonly int ContentIndent;

			// Token: 0x04002850 RID: 10320
			public readonly ScriptExecutionInstance.ScopeType Type;
		}

		// Token: 0x02000994 RID: 2452
		private struct Branch
		{
			// Token: 0x060084F0 RID: 34032 RVA: 0x004E55CF File Offset: 0x004E37CF
			public Branch(int beginIndex, int indent, bool isEntered)
			{
				this.BeginIndex = beginIndex;
				this.Indent = indent;
				this.IsEntered = isEntered;
			}

			// Token: 0x04002851 RID: 10321
			public readonly int BeginIndex;

			// Token: 0x04002852 RID: 10322
			public readonly int Indent;

			// Token: 0x04002853 RID: 10323
			public readonly bool IsEntered;
		}
	}
}
