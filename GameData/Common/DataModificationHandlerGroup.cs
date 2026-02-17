using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using GameData.Utilities;

namespace GameData.Common
{
	// Token: 0x020008F2 RID: 2290
	public class DataModificationHandlerGroup
	{
		// Token: 0x17000511 RID: 1297
		// (get) Token: 0x06008221 RID: 33313 RVA: 0x004D9B55 File Offset: 0x004D7D55
		public int Count
		{
			get
			{
				return this._handlers.Count;
			}
		}

		// Token: 0x06008222 RID: 33314 RVA: 0x004D9B62 File Offset: 0x004D7D62
		public DataModificationHandlerGroup()
		{
			this._indices = new Dictionary<string, int>();
			this._handlers = new List<ValueTuple<string, Action<DataContext, DataUid>>>();
			this._executionQueue = new List<Action<DataContext, DataUid>>();
		}

		// Token: 0x06008223 RID: 33315 RVA: 0x004D9B90 File Offset: 0x004D7D90
		public void RegisterHandler(string key, Action<DataContext, DataUid> handler)
		{
			int index = this._handlers.Count;
			this._indices.Add(key, index);
			this._handlers.Add(new ValueTuple<string, Action<DataContext, DataUid>>(key, handler));
		}

		// Token: 0x06008224 RID: 33316 RVA: 0x004D9BCC File Offset: 0x004D7DCC
		public bool UnregisterHandler(string key)
		{
			int index;
			bool flag = !this._indices.TryGetValue(key, out index);
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				this._indices.Remove(key);
				int lastIndex = this._handlers.Count - 1;
				bool flag2 = lastIndex != index;
				if (flag2)
				{
					string lastKey = this._handlers[lastIndex].Item1;
					CollectionUtils.SwapAndRemove<ValueTuple<string, Action<DataContext, DataUid>>>(this._handlers, index);
					this._indices[lastKey] = index;
				}
				else
				{
					this._handlers.RemoveAt(index);
				}
				result = true;
			}
			return result;
		}

		// Token: 0x06008225 RID: 33317 RVA: 0x004D9C60 File Offset: 0x004D7E60
		public void ExecuteAll(DataContext context, DataUid uid)
		{
			foreach (ValueTuple<string, Action<DataContext, DataUid>> valueTuple in this._handlers)
			{
				string key = valueTuple.Item1;
				Action<DataContext, DataUid> handler = valueTuple.Item2;
				this._executionQueue.Add(handler);
			}
			foreach (Action<DataContext, DataUid> handler2 in this._executionQueue)
			{
				handler2(context, uid);
			}
			this._executionQueue.Clear();
		}

		// Token: 0x04002428 RID: 9256
		private readonly Dictionary<string, int> _indices;

		// Token: 0x04002429 RID: 9257
		[TupleElementNames(new string[]
		{
			"key",
			"handler"
		})]
		private readonly List<ValueTuple<string, Action<DataContext, DataUid>>> _handlers;

		// Token: 0x0400242A RID: 9258
		private readonly List<Action<DataContext, DataUid>> _executionQueue;
	}
}
