using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using GameData.Domains.Taiwu.LifeSkillCombat.Status;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Taiwu.LifeSkillCombat.Operation
{
	// Token: 0x02000060 RID: 96
	[SerializableGameData(NotForDisplayModule = true)]
	public abstract class OperationBase : ISerializableGameData
	{
		// Token: 0x1700008C RID: 140
		// (get) Token: 0x0600157B RID: 5499 RVA: 0x0014B012 File Offset: 0x00149212
		// (set) Token: 0x0600157C RID: 5500 RVA: 0x0014B01A File Offset: 0x0014921A
		public sbyte PlayerId { get; protected set; }

		// Token: 0x0600157D RID: 5501 RVA: 0x0014B023 File Offset: 0x00149223
		public OperationBase()
		{
		}

		// Token: 0x0600157E RID: 5502
		public abstract string Inspect();

		// Token: 0x0600157F RID: 5503 RVA: 0x0014B030 File Offset: 0x00149230
		public override string ToString()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(5, 3);
			defaultInterpolatedStringHandler.AppendFormatted(base.GetType().Name);
			defaultInterpolatedStringHandler.AppendLiteral(" ");
			defaultInterpolatedStringHandler.AppendFormatted(this.Inspect());
			defaultInterpolatedStringHandler.AppendLiteral(" by ");
			defaultInterpolatedStringHandler.AppendFormatted(Player.PredefinedId.GetName(this.PlayerId));
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x06001580 RID: 5504 RVA: 0x0014B09B File Offset: 0x0014929B
		public bool IsSerializedSizeFixed()
		{
			return false;
		}

		// Token: 0x06001581 RID: 5505 RVA: 0x0014B0A0 File Offset: 0x001492A0
		public virtual int GetSerializedSize()
		{
			return 5;
		}

		// Token: 0x06001582 RID: 5506 RVA: 0x0014B0B4 File Offset: 0x001492B4
		public unsafe virtual int Serialize(byte* pData)
		{
			*pData = (byte)this.PlayerId;
			byte* pCurrData = pData + 1;
			*(int*)pCurrData = this.Stamp;
			pCurrData += 4;
			return (int)((long)(pCurrData - pData));
		}

		// Token: 0x06001583 RID: 5507 RVA: 0x0014B0E8 File Offset: 0x001492E8
		public unsafe virtual int Deserialize(byte* pData)
		{
			this.PlayerId = *(sbyte*)pData;
			byte* pCurrData = pData + 1;
			this.Stamp = *(int*)pCurrData;
			pCurrData += 4;
			return (int)((long)(pCurrData - pData));
		}

		// Token: 0x06001584 RID: 5508 RVA: 0x0014B11C File Offset: 0x0014931C
		private static IDictionary<Type, byte> CheckTypeIdMap()
		{
			bool flag = OperationBase._typeIdStableMap == null;
			if (flag)
			{
				List<Type> sortedList = new List<Type>();
				foreach (Type type in typeof(OperationBase).Assembly.GetTypes())
				{
					bool flag2 = typeof(OperationBase).IsAssignableFrom(type);
					if (flag2)
					{
						sortedList.Add(type);
					}
				}
				sortedList.Sort((Type a, Type b) => string.Compare(a.AssemblyQualifiedName, b.AssemblyQualifiedName, StringComparison.Ordinal));
				OperationBase._typeIdStableMap = new Dictionary<Type, byte>();
				int i = 0;
				int len = sortedList.Count;
				while (i < len)
				{
					Tester.Assert(i < 255, "");
					OperationBase._typeIdStableMap.Add(sortedList[i], (byte)i);
					i++;
				}
			}
			return OperationBase._typeIdStableMap;
		}

		// Token: 0x06001585 RID: 5509 RVA: 0x0014B214 File Offset: 0x00149414
		public static int GetSerializeSizeWithPolymorphism(OperationBase operation)
		{
			int size = 0;
			size++;
			return size + operation.GetSerializedSize();
		}

		// Token: 0x06001586 RID: 5510 RVA: 0x0014B238 File Offset: 0x00149438
		public unsafe static int SerializeWithPolymorphism(OperationBase operation, byte* pData)
		{
			IDictionary<Type, byte> typeIdMap = OperationBase.CheckTypeIdMap();
			*pData = typeIdMap[operation.GetType()];
			byte* pCurrentData = pData + 1;
			pCurrentData += operation.Serialize(pCurrentData);
			return (int)((long)(pCurrentData - pData));
		}

		// Token: 0x06001587 RID: 5511 RVA: 0x0014B27C File Offset: 0x0014947C
		public unsafe static int DeserializeWithPolymorphism(ref OperationBase operation, byte* pData)
		{
			byte* pCurrentData = pData;
			IDictionary<Type, byte> typeIdMap = OperationBase.CheckTypeIdMap();
			byte typeId = *pCurrentData;
			pCurrentData++;
			operation = null;
			foreach (KeyValuePair<Type, byte> pair in typeIdMap)
			{
				bool flag = pair.Value == typeId;
				if (flag)
				{
					operation = (Activator.CreateInstance(pair.Key) as OperationBase);
					break;
				}
			}
			Tester.Assert(operation != null, "");
			pCurrentData += operation.Deserialize(pCurrentData);
			return (int)((long)(pCurrentData - pData));
		}

		// Token: 0x06001588 RID: 5512 RVA: 0x0014B328 File Offset: 0x00149528
		unsafe static OperationBase()
		{
			SerializerHolder<OperationBase>.SerializeFunc = delegate(OperationBase item, RawDataPool pool)
			{
				byte* pData;
				int offset = pool.Allocate(OperationBase.GetSerializeSizeWithPolymorphism(item), &pData);
				OperationBase.SerializeWithPolymorphism(item, pData);
				return offset;
			};
			SerializerHolder<OperationBase>.DeserializeFunc = delegate(RawDataPool pool, int offset, ref OperationBase item)
			{
				return OperationBase.DeserializeWithPolymorphism(ref item, pool.GetPointer(offset));
			};
		}

		// Token: 0x06001589 RID: 5513 RVA: 0x0014B355 File Offset: 0x00149555
		protected OperationBase(sbyte playerId, int stamp)
		{
			this.PlayerId = playerId;
			this.Stamp = stamp;
		}

		// Token: 0x0600158A RID: 5514 RVA: 0x0014B36E File Offset: 0x0014956E
		public int GetStamp()
		{
			return this.Stamp;
		}

		// Token: 0x0400037D RID: 893
		protected int Stamp;

		// Token: 0x0400037E RID: 894
		private static IDictionary<Type, byte> _typeIdStableMap;
	}
}
