using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Config;

namespace GameData.Domains.Combat.Ai
{
	// Token: 0x02000717 RID: 1815
	public abstract class AiData
	{
		// Token: 0x17000434 RID: 1076
		// (get) Token: 0x06006877 RID: 26743
		protected abstract IReadOnlyList<IAiNode> Nodes { get; }

		// Token: 0x17000435 RID: 1077
		// (get) Token: 0x06006878 RID: 26744
		protected abstract IReadOnlyList<IAiCondition> Conditions { get; }

		// Token: 0x17000436 RID: 1078
		// (get) Token: 0x06006879 RID: 26745
		protected abstract IReadOnlyList<IAiAction> Actions { get; }

		// Token: 0x0600687A RID: 26746 RVA: 0x003B6CF8 File Offset: 0x003B4EF8
		public void Update(AiMemoryNew memory, IAiParticipant participant)
		{
			int nodeId = (this._entryPoint < 0 || this._entryPoint >= this.Nodes.Count) ? 0 : this._entryPoint;
			this._entryPoint = 0;
			bool flag = nodeId < 0 || nodeId >= this.Nodes.Count;
			if (!flag)
			{
				bool flag2 = this._updatingContext != null || memory == null || participant == null;
				if (!flag2)
				{
					this._updatingContext = new AiContext?(new ValueTuple<AiMemoryNew, IAiParticipant>(memory, participant));
					try
					{
						this.Update(nodeId, 0);
					}
					catch (Exception e)
					{
						PredefinedLog.Show(8, "AiData.Update catch exception " + e.Message + "\nstacktrace:\n" + e.StackTrace);
					}
					this._updatingContext = null;
				}
			}
		}

		// Token: 0x0600687B RID: 26747 RVA: 0x003B6DD8 File Offset: 0x003B4FD8
		private bool Update(int nodeId, int depth = 0)
		{
			bool flag = nodeId < 0 || nodeId >= this.Nodes.Count;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				bool flag2 = depth > 1000;
				if (flag2)
				{
					short predefinedLogId = 8;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(29, 1);
					defaultInterpolatedStringHandler.AppendLiteral("AiData.Update depth overflow ");
					defaultInterpolatedStringHandler.AppendFormatted<int>(nodeId);
					PredefinedLog.Show(predefinedLogId, defaultInterpolatedStringHandler.ToStringAndClear());
					result = true;
				}
				else
				{
					bool flag3 = this._updatingContext == null;
					if (flag3)
					{
						result = false;
					}
					else
					{
						foreach (int newNodeId in this.Nodes[nodeId].Update(this))
						{
							bool flag4 = this.Update(newNodeId, depth + 1);
							if (flag4)
							{
								return true;
							}
						}
						result = this.Nodes[nodeId].IsAction;
					}
				}
			}
			return result;
		}

		// Token: 0x0600687C RID: 26748 RVA: 0x003B6ED8 File Offset: 0x003B50D8
		public void Reset()
		{
			this._entryPoint = 0;
		}

		// Token: 0x0600687D RID: 26749 RVA: 0x003B6EE4 File Offset: 0x003B50E4
		public bool CheckCondition(int conditionId)
		{
			bool flag = conditionId < 0 || conditionId >= this.Conditions.Count;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				bool flag2 = this._updatingContext == null;
				result = (!flag2 && this.Conditions[conditionId].Check(this._updatingContext.Value.Memory, this._updatingContext.Value.Participant));
			}
			return result;
		}

		// Token: 0x0600687E RID: 26750 RVA: 0x003B6F60 File Offset: 0x003B5160
		public bool ExecuteAction(int actionId)
		{
			bool flag = actionId < 0 || actionId >= this.Actions.Count;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				bool flag2 = this._updatingContext == null;
				if (flag2)
				{
					result = false;
				}
				else
				{
					this.Actions[actionId].Execute(this._updatingContext.Value.Memory, this._updatingContext.Value.Participant);
					result = true;
				}
			}
			return result;
		}

		// Token: 0x0600687F RID: 26751 RVA: 0x003B6FDC File Offset: 0x003B51DC
		public void RelayEntry(int nodeId)
		{
			bool flag = nodeId < 0 || nodeId >= this.Nodes.Count;
			if (!flag)
			{
				this._entryPoint = nodeId;
			}
		}

		// Token: 0x04001C9B RID: 7323
		private int _entryPoint;

		// Token: 0x04001C9C RID: 7324
		private AiContext? _updatingContext;
	}
}
