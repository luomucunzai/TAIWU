using System;
using GameData.Utilities;

namespace GameData.Serializer
{
	// Token: 0x0200001C RID: 28
	public static class SerializerHolder<T>
	{
		// Token: 0x06000CD5 RID: 3285 RVA: 0x000DBE04 File Offset: 0x000DA004
		public static int Serialize(T item, RawDataPool dataPool)
		{
			bool flag = SerializerHolder<T>.SerializeFunc != null;
			int result;
			if (flag)
			{
				result = SerializerHolder<T>.SerializeFunc(item, dataPool);
			}
			else
			{
				result = Serializer.SerializeDefault<T>(item, dataPool);
			}
			return result;
		}

		// Token: 0x06000CD6 RID: 3286 RVA: 0x000DBE38 File Offset: 0x000DA038
		public static int Deserialize(RawDataPool dataPool, int offset, ref T item)
		{
			bool flag = SerializerHolder<T>.DeserializeFunc != null;
			int result;
			if (flag)
			{
				result = SerializerHolder<T>.DeserializeFunc(dataPool, offset, ref item);
			}
			else
			{
				result = Serializer.DeserializeDefault<T>(dataPool, offset, ref item);
			}
			return result;
		}

		// Token: 0x0400007A RID: 122
		public static SerializerHolder<T>.SerializeFunction SerializeFunc;

		// Token: 0x0400007B RID: 123
		public static SerializerHolder<T>.DeserializeFunction DeserializeFunc;

		// Token: 0x0200090F RID: 2319
		// (Invoke) Token: 0x06008350 RID: 33616
		public delegate int SerializeFunction(T item, RawDataPool dataPool);

		// Token: 0x02000910 RID: 2320
		// (Invoke) Token: 0x06008354 RID: 33620
		public delegate int DeserializeFunction(RawDataPool dataPool, int offset, ref T item);
	}
}
