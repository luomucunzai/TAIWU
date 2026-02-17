using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using GameData.Domains.Global;
using GameData.Domains.LifeRecord;
using GameData.Serializer;

namespace GameData.ArchiveData
{
	// Token: 0x02000906 RID: 2310
	public static class ResponseProcessor
	{
		// Token: 0x060082E4 RID: 33508 RVA: 0x004DCC30 File Offset: 0x004DAE30
		public unsafe static void ProcessSingleValue_BasicType_Fixed_Value_Single<[IsUnmanaged] T>(OperationWrapper operation, byte* pResult, ref T item) where T : struct, ValueType
		{
			byte methodId = operation.MethodId;
			byte b = methodId;
			if (b != 0)
			{
				if (b != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(21, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported methodId ");
					defaultInterpolatedStringHandler.AppendFormatted<byte>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				item = *(T*)pResult;
				pResult += sizeof(T);
			}
		}

		// Token: 0x060082E5 RID: 33509 RVA: 0x004DCCA4 File Offset: 0x004DAEA4
		public unsafe static void ProcessSingleValue_BasicType_Fixed_Value_Array<[IsUnmanaged] T>(OperationWrapper operation, byte* pResult, T[] item, int elementsCount) where T : struct, ValueType
		{
			byte methodId = operation.MethodId;
			byte b = methodId;
			if (b != 0)
			{
				if (b != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(21, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported methodId ");
					defaultInterpolatedStringHandler.AppendFormatted<byte>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				bool flag = item.Length != elementsCount;
				if (flag)
				{
					throw new Exception("Elements count is not equal to declaration");
				}
				for (int i = 0; i < elementsCount; i++)
				{
					item[i] = *(T*)(pResult + (IntPtr)i * (IntPtr)sizeof(T));
				}
				pResult += sizeof(T) * elementsCount;
			}
		}

		// Token: 0x060082E6 RID: 33510 RVA: 0x004DCD50 File Offset: 0x004DAF50
		public unsafe static void ProcessSingleValue_BasicType_Fixed_Value_List<[IsUnmanaged] T>(OperationWrapper operation, byte* pResult, List<T> item) where T : struct, ValueType
		{
			byte methodId = operation.MethodId;
			byte b = methodId;
			if (b != 0)
			{
				if (b != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(21, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported methodId ");
					defaultInterpolatedStringHandler.AppendFormatted<byte>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				uint contentSize = *(uint*)pResult;
				pResult += 4;
				bool flag = contentSize == 0U;
				if (!flag)
				{
					ushort elementsCount = *(ushort*)pResult;
					pResult += 2;
					item.Clear();
					for (int i = 0; i < (int)elementsCount; i++)
					{
						item.Add(*(T*)(pResult + (IntPtr)i * (IntPtr)sizeof(T)));
					}
					pResult += sizeof(T) * (int)elementsCount;
				}
			}
		}

		// Token: 0x060082E7 RID: 33511 RVA: 0x004DCE0C File Offset: 0x004DB00C
		public unsafe static void ProcessSingleValue_BasicType_String_Single(OperationWrapper operation, byte* pResult, ref string item)
		{
			byte methodId = operation.MethodId;
			byte b = methodId;
			if (b != 0)
			{
				if (b != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(21, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported methodId ");
					defaultInterpolatedStringHandler.AppendFormatted<byte>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				uint contentSize = *(uint*)pResult;
				pResult += 4;
				item = Encoding.Unicode.GetString(pResult, (int)contentSize);
				pResult += contentSize;
			}
		}

		// Token: 0x060082E8 RID: 33512 RVA: 0x004DCE84 File Offset: 0x004DB084
		public unsafe static void ProcessSingleValue_BasicType_String_Array(OperationWrapper operation, byte* pResult, string[] item, int elementsCount)
		{
			byte methodId = operation.MethodId;
			byte b = methodId;
			if (b != 0)
			{
				if (b != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(21, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported methodId ");
					defaultInterpolatedStringHandler.AppendFormatted<byte>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				uint contentSize = *(uint*)pResult;
				pResult += 4;
				bool flag = contentSize == 0U;
				if (!flag)
				{
					bool flag2 = item.Length != elementsCount;
					if (flag2)
					{
						throw new Exception("Elements count is not equal to declaration");
					}
					for (int i = 0; i < elementsCount; i++)
					{
						ushort subContentSize = *(ushort*)pResult;
						pResult += 2;
						bool flag3 = subContentSize > 0;
						if (flag3)
						{
							item[i] = Encoding.Unicode.GetString(pResult, (int)subContentSize);
							pResult += subContentSize;
						}
						else
						{
							item[i] = null;
						}
					}
				}
			}
		}

		// Token: 0x060082E9 RID: 33513 RVA: 0x004DCF60 File Offset: 0x004DB160
		public unsafe static void ProcessSingleValue_BasicType_String_List(OperationWrapper operation, byte* pResult, List<string> item)
		{
			byte methodId = operation.MethodId;
			byte b = methodId;
			if (b != 0)
			{
				if (b != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(21, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported methodId ");
					defaultInterpolatedStringHandler.AppendFormatted<byte>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				uint contentSize = *(uint*)pResult;
				pResult += 4;
				bool flag = contentSize == 0U;
				if (!flag)
				{
					ushort elementsCount = *(ushort*)pResult;
					pResult += 2;
					item.Clear();
					for (int i = 0; i < (int)elementsCount; i++)
					{
						ushort subContentSize = *(ushort*)pResult;
						pResult += 2;
						bool flag2 = subContentSize > 0;
						if (flag2)
						{
							item.Add(Encoding.Unicode.GetString(pResult, (int)subContentSize));
							pResult += subContentSize;
						}
						else
						{
							item.Add(null);
						}
					}
				}
			}
		}

		// Token: 0x060082EA RID: 33514 RVA: 0x004DD03C File Offset: 0x004DB23C
		public unsafe static void ProcessSingleValue_CustomType_Fixed_Value_Single<T>(OperationWrapper operation, byte* pResult, ref T item) where T : struct, ISerializableGameData
		{
			byte methodId = operation.MethodId;
			byte b = methodId;
			if (b != 0)
			{
				if (b != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(21, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported methodId ");
					defaultInterpolatedStringHandler.AppendFormatted<byte>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				pResult += item.Deserialize(pResult);
			}
		}

		// Token: 0x060082EB RID: 33515 RVA: 0x004DD0A8 File Offset: 0x004DB2A8
		public unsafe static void ProcessSingleValue_CustomType_Fixed_Value_Array<T>(OperationWrapper operation, byte* pResult, T[] item, int elementsCount, int elementFixedSize) where T : struct, ISerializableGameData
		{
			byte methodId = operation.MethodId;
			byte b = methodId;
			if (b != 0)
			{
				if (b != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(21, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported methodId ");
					defaultInterpolatedStringHandler.AppendFormatted<byte>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				bool flag = item.Length != elementsCount;
				if (flag)
				{
					throw new Exception("Elements count is not equal to declaration");
				}
				for (int i = 0; i < elementsCount; i++)
				{
					T element = Activator.CreateInstance<T>();
					pResult += element.Deserialize(pResult);
					item[i] = element;
				}
			}
		}

		// Token: 0x060082EC RID: 33516 RVA: 0x004DD154 File Offset: 0x004DB354
		public unsafe static void ProcessSingleValue_CustomType_Fixed_Value_List<T>(OperationWrapper operation, byte* pResult, List<T> item, int elementFixedSize) where T : struct, ISerializableGameData
		{
			byte methodId = operation.MethodId;
			byte b = methodId;
			if (b != 0)
			{
				if (b != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(21, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported methodId ");
					defaultInterpolatedStringHandler.AppendFormatted<byte>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				uint contentSize = *(uint*)pResult;
				pResult += 4;
				bool flag = contentSize == 0U;
				if (!flag)
				{
					ushort elementsCount = *(ushort*)pResult;
					pResult += 2;
					item.Clear();
					for (int i = 0; i < (int)elementsCount; i++)
					{
						T element = Activator.CreateInstance<T>();
						pResult += element.Deserialize(pResult);
						item.Add(element);
					}
				}
			}
		}

		// Token: 0x060082ED RID: 33517 RVA: 0x004DD210 File Offset: 0x004DB410
		public unsafe static void ProcessSingleValue_CustomType_Fixed_Ref_Single<T>(OperationWrapper operation, byte* pResult, T item) where T : class, ISerializableGameData, new()
		{
			byte methodId = operation.MethodId;
			byte b = methodId;
			if (b != 0)
			{
				if (b != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(21, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported methodId ");
					defaultInterpolatedStringHandler.AppendFormatted<byte>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				pResult += item.Deserialize(pResult);
			}
		}

		// Token: 0x060082EE RID: 33518 RVA: 0x004DD27C File Offset: 0x004DB47C
		public unsafe static void ProcessSingleValue_CustomType_Fixed_Ref_Array<T>(OperationWrapper operation, byte* pResult, T[] item, int elementsCount, int elementFixedSize) where T : class, ISerializableGameData, new()
		{
			byte methodId = operation.MethodId;
			byte b = methodId;
			if (b != 0)
			{
				if (b != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(21, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported methodId ");
					defaultInterpolatedStringHandler.AppendFormatted<byte>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				bool flag = item.Length != elementsCount;
				if (flag)
				{
					throw new Exception("Elements count is not equal to declaration");
				}
				for (int i = 0; i < elementsCount; i++)
				{
					T element = item[i];
					pResult += element.Deserialize(pResult);
				}
			}
		}

		// Token: 0x060082EF RID: 33519 RVA: 0x004DD320 File Offset: 0x004DB520
		public unsafe static void ProcessSingleValue_CustomType_Fixed_Ref_List<T>(OperationWrapper operation, byte* pResult, List<T> item, int elementFixedSize) where T : class, ISerializableGameData, new()
		{
			byte methodId = operation.MethodId;
			byte b = methodId;
			if (b != 0)
			{
				if (b != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(21, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported methodId ");
					defaultInterpolatedStringHandler.AppendFormatted<byte>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				uint contentSize = *(uint*)pResult;
				pResult += 4;
				bool flag = contentSize == 0U;
				if (!flag)
				{
					ushort elementsCount = *(ushort*)pResult;
					pResult += 2;
					bool flag2 = elementsCount > 0;
					if (flag2)
					{
						int destElementsCount = item.Count;
						bool flag3 = (int)elementsCount < destElementsCount;
						if (flag3)
						{
							item.RemoveRange((int)elementsCount, destElementsCount - (int)elementsCount);
						}
						for (int i = 0; i < (int)elementsCount; i++)
						{
							bool flag4 = i < destElementsCount;
							if (flag4)
							{
								T element = item[i];
								pResult += element.Deserialize(pResult);
							}
							else
							{
								T element2 = Activator.CreateInstance<T>();
								pResult += element2.Deserialize(pResult);
								item.Add(element2);
							}
						}
					}
					else
					{
						item.Clear();
					}
				}
			}
		}

		// Token: 0x060082F0 RID: 33520 RVA: 0x004DD440 File Offset: 0x004DB640
		public unsafe static void ProcessSingleValue_CustomType_Dynamic_Value_Single<T>(OperationWrapper operation, byte* pResult, ref T item) where T : struct, ISerializableGameData
		{
			byte methodId = operation.MethodId;
			byte b = methodId;
			if (b != 0)
			{
				if (b != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(21, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported methodId ");
					defaultInterpolatedStringHandler.AppendFormatted<byte>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				uint contentSize = *(uint*)pResult;
				pResult += 4;
				bool flag = contentSize == 0U;
				if (!flag)
				{
					pResult += item.Deserialize(pResult);
				}
			}
		}

		// Token: 0x060082F1 RID: 33521 RVA: 0x004DD4C0 File Offset: 0x004DB6C0
		public unsafe static void ProcessSingleValue_CustomType_Dynamic_Value_Array<T>(OperationWrapper operation, byte* pResult, T[] item, int elementsCount) where T : struct, ISerializableGameData
		{
			byte methodId = operation.MethodId;
			byte b = methodId;
			if (b != 0)
			{
				if (b != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(21, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported methodId ");
					defaultInterpolatedStringHandler.AppendFormatted<byte>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				uint contentSize = *(uint*)pResult;
				pResult += 4;
				bool flag = contentSize == 0U;
				if (!flag)
				{
					bool flag2 = item.Length != elementsCount;
					if (flag2)
					{
						throw new Exception("Elements count is not equal to declaration");
					}
					for (int i = 0; i < elementsCount; i++)
					{
						T element = Activator.CreateInstance<T>();
						pResult += element.Deserialize(pResult);
						item[i] = element;
					}
				}
			}
		}

		// Token: 0x060082F2 RID: 33522 RVA: 0x004DD588 File Offset: 0x004DB788
		public unsafe static void ProcessSingleValue_CustomType_Dynamic_Value_List<T>(OperationWrapper operation, byte* pResult, List<T> item, int elementFixedSize) where T : struct, ISerializableGameData
		{
			byte methodId = operation.MethodId;
			byte b = methodId;
			if (b != 0)
			{
				if (b != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(21, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported methodId ");
					defaultInterpolatedStringHandler.AppendFormatted<byte>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				uint contentSize = *(uint*)pResult;
				pResult += 4;
				bool flag = contentSize == 0U;
				if (!flag)
				{
					ushort elementsCount = *(ushort*)pResult;
					pResult += 2;
					item.Clear();
					for (int i = 0; i < (int)elementsCount; i++)
					{
						T element = Activator.CreateInstance<T>();
						pResult += element.Deserialize(pResult);
						item.Add(element);
					}
				}
			}
		}

		// Token: 0x060082F3 RID: 33523 RVA: 0x004DD644 File Offset: 0x004DB844
		public unsafe static void ProcessSingleValue_CustomType_Dynamic_Ref_Single<T>(OperationWrapper operation, byte* pResult, T item) where T : class, ISerializableGameData, new()
		{
			byte methodId = operation.MethodId;
			byte b = methodId;
			if (b != 0)
			{
				if (b != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(21, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported methodId ");
					defaultInterpolatedStringHandler.AppendFormatted<byte>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				uint contentSize = *(uint*)pResult;
				pResult += 4;
				bool flag = contentSize == 0U;
				if (!flag)
				{
					pResult += item.Deserialize(pResult);
				}
			}
		}

		// Token: 0x060082F4 RID: 33524 RVA: 0x004DD6C4 File Offset: 0x004DB8C4
		public unsafe static void ProcessSingleValue_CustomType_Dynamic_Ref_Array<T>(OperationWrapper operation, byte* pResult, T[] item, int elementsCount) where T : class, ISerializableGameData, new()
		{
			byte methodId = operation.MethodId;
			byte b = methodId;
			if (b != 0)
			{
				if (b != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(21, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported methodId ");
					defaultInterpolatedStringHandler.AppendFormatted<byte>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				uint contentSize = *(uint*)pResult;
				pResult += 4;
				bool flag = contentSize == 0U;
				if (!flag)
				{
					bool flag2 = item.Length != elementsCount;
					if (flag2)
					{
						throw new Exception("Elements count is not equal to declaration");
					}
					for (int i = 0; i < elementsCount; i++)
					{
						ushort subContentSize = *(ushort*)pResult;
						pResult += 2;
						bool flag3 = subContentSize > 0;
						if (flag3)
						{
							T element = item[i];
							bool flag4 = element != null;
							if (flag4)
							{
								pResult += element.Deserialize(pResult);
							}
							else
							{
								element = Activator.CreateInstance<T>();
								pResult += element.Deserialize(pResult);
								item[i] = element;
							}
						}
						else
						{
							item[i] = default(T);
						}
					}
				}
			}
		}

		// Token: 0x060082F5 RID: 33525 RVA: 0x004DD7F0 File Offset: 0x004DB9F0
		public unsafe static void ProcessSingleValue_CustomType_Dynamic_Ref_List<T>(OperationWrapper operation, byte* pResult, List<T> item) where T : class, ISerializableGameData, new()
		{
			byte methodId = operation.MethodId;
			byte b = methodId;
			if (b != 0)
			{
				if (b != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(21, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported methodId ");
					defaultInterpolatedStringHandler.AppendFormatted<byte>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				uint contentSize = *(uint*)pResult;
				pResult += 4;
				bool flag = contentSize == 0U;
				if (!flag)
				{
					ushort elementsCount = *(ushort*)pResult;
					pResult += 2;
					bool flag2 = elementsCount > 0;
					if (flag2)
					{
						int destElementsCount = item.Count;
						bool flag3 = (int)elementsCount < destElementsCount;
						if (flag3)
						{
							item.RemoveRange((int)elementsCount, destElementsCount - (int)elementsCount);
						}
						for (int i = 0; i < (int)elementsCount; i++)
						{
							ushort subContentSize = *(ushort*)pResult;
							pResult += 2;
							bool flag4 = subContentSize > 0;
							if (flag4)
							{
								bool flag5 = i < destElementsCount;
								if (flag5)
								{
									T element = item[i];
									bool flag6 = element != null;
									if (flag6)
									{
										pResult += element.Deserialize(pResult);
									}
									else
									{
										element = Activator.CreateInstance<T>();
										pResult += element.Deserialize(pResult);
										item[i] = element;
									}
								}
								else
								{
									T element2 = Activator.CreateInstance<T>();
									pResult += element2.Deserialize(pResult);
									item.Add(element2);
								}
							}
							else
							{
								bool flag7 = i < destElementsCount;
								if (flag7)
								{
									item[i] = default(T);
								}
								else
								{
									item.Add(default(T));
								}
							}
						}
					}
					else
					{
						item.Clear();
					}
				}
			}
		}

		// Token: 0x060082F6 RID: 33526 RVA: 0x004DD99C File Offset: 0x004DBB9C
		public unsafe static void ProcessSingleValueCollection_BasicType_Fixed_Value<[IsUnmanaged] TKey, [IsUnmanaged] TValue>(OperationWrapper operation, byte* pResult, IDictionary<TKey, TValue> collection) where TKey : struct, ValueType where TValue : struct, ValueType
		{
			switch (operation.MethodId)
			{
			case 0:
				break;
			case 1:
				break;
			case 2:
			{
				TKey elementId = *(TKey*)operation.PData;
				collection[elementId] = *(TValue*)pResult;
				pResult += sizeof(TValue);
				break;
			}
			case 3:
				break;
			case 4:
				break;
			case 5:
			{
				collection.Clear();
				int elementsCount = (int)(*(uint*)pResult);
				pResult += 4;
				for (int i = 0; i < elementsCount; i++)
				{
					TKey elementId2 = *(TKey*)pResult;
					pResult += sizeof(TKey);
					collection.Add(elementId2, *(TValue*)pResult);
					pResult += sizeof(TValue);
				}
				break;
			}
			default:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(21, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Unsupported methodId ");
				defaultInterpolatedStringHandler.AppendFormatted<byte>(operation.MethodId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			}
		}

		// Token: 0x060082F7 RID: 33527 RVA: 0x004DDAA0 File Offset: 0x004DBCA0
		public unsafe static void ProcessSingleValueCollection_BasicType_String<[IsUnmanaged] TKey>(OperationWrapper operation, byte* pResult, IDictionary<TKey, string> collection) where TKey : struct, ValueType
		{
			switch (operation.MethodId)
			{
			case 0:
				break;
			case 1:
				break;
			case 2:
			{
				TKey elementId = *(TKey*)operation.PData;
				uint contentSize = *(uint*)pResult;
				pResult += 4;
				bool flag = contentSize > 0U;
				if (flag)
				{
					collection[elementId] = Encoding.Unicode.GetString(pResult, (int)contentSize);
					pResult += contentSize;
				}
				else
				{
					collection[elementId] = null;
				}
				break;
			}
			case 3:
				break;
			case 4:
				break;
			case 5:
			{
				collection.Clear();
				int elementsCount = (int)(*(uint*)pResult);
				pResult += 4;
				for (int i = 0; i < elementsCount; i++)
				{
					TKey elementId2 = *(TKey*)pResult;
					pResult += sizeof(TKey);
					uint subContentSize = *(uint*)pResult;
					pResult += 4;
					bool flag2 = subContentSize > 0U;
					if (flag2)
					{
						collection.Add(elementId2, Encoding.Unicode.GetString(pResult, (int)subContentSize));
						pResult += subContentSize;
					}
					else
					{
						collection.Add(elementId2, null);
					}
				}
				break;
			}
			default:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(21, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Unsupported methodId ");
				defaultInterpolatedStringHandler.AppendFormatted<byte>(operation.MethodId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			}
		}

		// Token: 0x060082F8 RID: 33528 RVA: 0x004DDBEC File Offset: 0x004DBDEC
		public unsafe static void ProcessSingleValueCollection_CustomType_Fixed_Value<[IsUnmanaged] TKey, TValue>(OperationWrapper operation, byte* pResult, IDictionary<TKey, TValue> collection, int elementFixedSize) where TKey : struct, ValueType where TValue : struct, ISerializableGameData
		{
			switch (operation.MethodId)
			{
			case 0:
				break;
			case 1:
				break;
			case 2:
			{
				TKey elementId = *(TKey*)operation.PData;
				TValue element = Activator.CreateInstance<TValue>();
				element.Deserialize(pResult);
				collection[elementId] = element;
				break;
			}
			case 3:
				break;
			case 4:
				break;
			case 5:
			{
				collection.Clear();
				int elementsCount = (int)(*(uint*)pResult);
				pResult += 4;
				for (int i = 0; i < elementsCount; i++)
				{
					TKey elementId2 = *(TKey*)pResult;
					pResult += sizeof(TKey);
					TValue element2 = Activator.CreateInstance<TValue>();
					pResult += element2.Deserialize(pResult);
					collection.Add(elementId2, element2);
				}
				break;
			}
			default:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(21, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Unsupported methodId ");
				defaultInterpolatedStringHandler.AppendFormatted<byte>(operation.MethodId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			}
		}

		// Token: 0x060082F9 RID: 33529 RVA: 0x004DDD04 File Offset: 0x004DBF04
		public unsafe static void ProcessSingleValueCollection_CustomType_Fixed_Ref<[IsUnmanaged] TKey, TValue>(OperationWrapper operation, byte* pResult, IDictionary<TKey, TValue> collection, int elementFixedSize) where TKey : struct, ValueType where TValue : class, ISerializableGameData, new()
		{
			switch (operation.MethodId)
			{
			case 0:
				break;
			case 1:
				break;
			case 2:
			{
				TKey elementId = *(TKey*)operation.PData;
				TValue element = Activator.CreateInstance<TValue>();
				element.Deserialize(pResult);
				collection[elementId] = element;
				break;
			}
			case 3:
				break;
			case 4:
				break;
			case 5:
			{
				collection.Clear();
				int elementsCount = (int)(*(uint*)pResult);
				pResult += 4;
				for (int i = 0; i < elementsCount; i++)
				{
					TKey elementId2 = *(TKey*)pResult;
					pResult += sizeof(TKey);
					TValue element2 = Activator.CreateInstance<TValue>();
					pResult += element2.Deserialize(pResult);
					collection.Add(elementId2, element2);
				}
				break;
			}
			default:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(21, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Unsupported methodId ");
				defaultInterpolatedStringHandler.AppendFormatted<byte>(operation.MethodId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			}
		}

		// Token: 0x060082FA RID: 33530 RVA: 0x004DDE18 File Offset: 0x004DC018
		public unsafe static void ProcessSingleValueCollection_CustomType_Dynamic_Value<[IsUnmanaged] TKey, TValue>(OperationWrapper operation, byte* pResult, IDictionary<TKey, TValue> collection) where TKey : struct, ValueType where TValue : struct, ISerializableGameData
		{
			switch (operation.MethodId)
			{
			case 0:
				break;
			case 1:
				break;
			case 2:
			{
				TKey elementId = *(TKey*)operation.PData;
				pResult += 4;
				TValue element = Activator.CreateInstance<TValue>();
				element.Deserialize(pResult);
				collection[elementId] = element;
				break;
			}
			case 3:
				break;
			case 4:
				break;
			case 5:
			{
				collection.Clear();
				int elementsCount = (int)(*(uint*)pResult);
				pResult += 4;
				for (int i = 0; i < elementsCount; i++)
				{
					TKey elementId2 = *(TKey*)pResult;
					pResult += sizeof(TKey);
					pResult += 4;
					TValue element2 = Activator.CreateInstance<TValue>();
					pResult += element2.Deserialize(pResult);
					collection.Add(elementId2, element2);
				}
				break;
			}
			default:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(21, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Unsupported methodId ");
				defaultInterpolatedStringHandler.AppendFormatted<byte>(operation.MethodId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			}
		}

		// Token: 0x060082FB RID: 33531 RVA: 0x004DDF38 File Offset: 0x004DC138
		public unsafe static void ProcessSingleValueCollection_CustomType_Dynamic_Ref<[IsUnmanaged] TKey, TValue>(OperationWrapper operation, byte* pResult, IDictionary<TKey, TValue> collection) where TKey : struct, ValueType where TValue : class, ISerializableGameData, new()
		{
			switch (operation.MethodId)
			{
			case 0:
				break;
			case 1:
				break;
			case 2:
			{
				TKey elementId = *(TKey*)operation.PData;
				uint contentSize = *(uint*)pResult;
				pResult += 4;
				bool flag = contentSize > 0U;
				if (flag)
				{
					TValue element = Activator.CreateInstance<TValue>();
					pResult += element.Deserialize(pResult);
					collection[elementId] = element;
				}
				else
				{
					collection[elementId] = default(TValue);
				}
				break;
			}
			case 3:
				break;
			case 4:
				break;
			case 5:
			{
				collection.Clear();
				int elementsCount = (int)(*(uint*)pResult);
				pResult += 4;
				for (int i = 0; i < elementsCount; i++)
				{
					TKey elementId2 = *(TKey*)pResult;
					pResult += sizeof(TKey);
					uint subContentSize = *(uint*)pResult;
					pResult += 4;
					bool flag2 = subContentSize > 0U;
					if (flag2)
					{
						TValue element2 = Activator.CreateInstance<TValue>();
						pResult += element2.Deserialize(pResult);
						collection.Add(elementId2, element2);
					}
					else
					{
						collection.Add(elementId2, default(TValue));
					}
				}
				break;
			}
			default:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(21, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Unsupported methodId ");
				defaultInterpolatedStringHandler.AppendFormatted<byte>(operation.MethodId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			}
		}

		// Token: 0x060082FC RID: 33532 RVA: 0x004DE0A4 File Offset: 0x004DC2A4
		public unsafe static void ProcessElementList_BasicType_Fixed_Value<[IsUnmanaged] T>(OperationWrapper operation, byte* pResult, T[] item, int elementsCount, int elementFixedSize) where T : struct, ValueType
		{
			switch (operation.MethodId)
			{
			case 0:
				throw new NotImplementedException();
			case 1:
				break;
			case 2:
			{
				int index = (int)(*(uint*)operation.PData);
				item[index] = *(T*)pResult;
				pResult += sizeof(T);
				break;
			}
			case 3:
				break;
			case 4:
				break;
			case 5:
				break;
			case 6:
				break;
			case 7:
				break;
			case 8:
			{
				bool flag = item.Length != elementsCount;
				if (flag)
				{
					throw new Exception("Elements count is not equal to declaration");
				}
				pResult += 4;
				for (int i = 0; i < elementsCount; i++)
				{
					item[i] = *(T*)(pResult + (IntPtr)i * (IntPtr)sizeof(T));
				}
				pResult += sizeof(T) * elementsCount;
				break;
			}
			default:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(21, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Unsupported methodId ");
				defaultInterpolatedStringHandler.AppendFormatted<byte>(operation.MethodId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			}
		}

		// Token: 0x060082FD RID: 33533 RVA: 0x004DE1C4 File Offset: 0x004DC3C4
		public unsafe static void ProcessElementList_BasicType_String(OperationWrapper operation, byte* pResult, string[] item, int elementsCount)
		{
			switch (operation.MethodId)
			{
			case 0:
				throw new NotImplementedException();
			case 1:
				break;
			case 2:
			{
				int index = (int)(*(uint*)operation.PData);
				uint contentSize = *(uint*)pResult;
				pResult += 4;
				bool flag = contentSize > 0U;
				if (flag)
				{
					item[index] = Encoding.Unicode.GetString(pResult, (int)contentSize);
					pResult += contentSize;
				}
				else
				{
					item[index] = null;
				}
				break;
			}
			case 3:
				break;
			case 4:
				break;
			case 5:
				break;
			case 6:
				break;
			case 7:
				break;
			case 8:
			{
				bool flag2 = item.Length != elementsCount;
				if (flag2)
				{
					throw new Exception("Elements count is not equal to declaration");
				}
				pResult += 4;
				for (int i = 0; i < elementsCount; i++)
				{
					uint subContentSize = *(uint*)pResult;
					pResult += 4;
					bool flag3 = subContentSize > 0U;
					if (flag3)
					{
						item[i] = Encoding.Unicode.GetString(pResult, (int)subContentSize);
						pResult += subContentSize;
					}
					else
					{
						item[i] = null;
					}
				}
				break;
			}
			default:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(21, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Unsupported methodId ");
				defaultInterpolatedStringHandler.AppendFormatted<byte>(operation.MethodId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			}
		}

		// Token: 0x060082FE RID: 33534 RVA: 0x004DE310 File Offset: 0x004DC510
		public unsafe static void ProcessElementList_CustomType_Fixed_Value<T>(OperationWrapper operation, byte* pResult, T[] item, int elementsCount, int elementFixedSize) where T : struct, ISerializableGameData
		{
			switch (operation.MethodId)
			{
			case 0:
				throw new NotImplementedException();
			case 1:
				break;
			case 2:
			{
				int index = (int)(*(uint*)operation.PData);
				T element = Activator.CreateInstance<T>();
				element.Deserialize(pResult);
				item[index] = element;
				break;
			}
			case 3:
				break;
			case 4:
				break;
			case 5:
				break;
			case 6:
				break;
			case 7:
				break;
			case 8:
			{
				bool flag = item.Length != elementsCount;
				if (flag)
				{
					throw new Exception("Elements count is not equal to declaration");
				}
				pResult += 4;
				for (int i = 0; i < elementsCount; i++)
				{
					T element2 = Activator.CreateInstance<T>();
					pResult += element2.Deserialize(pResult);
					item[i] = element2;
				}
				break;
			}
			default:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(21, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Unsupported methodId ");
				defaultInterpolatedStringHandler.AppendFormatted<byte>(operation.MethodId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			}
		}

		// Token: 0x060082FF RID: 33535 RVA: 0x004DE438 File Offset: 0x004DC638
		public unsafe static void ProcessElementList_CustomType_Fixed_Ref<T>(OperationWrapper operation, byte* pResult, T[] item, int elementsCount, int elementFixedSize) where T : class, ISerializableGameData, new()
		{
			switch (operation.MethodId)
			{
			case 0:
				throw new NotImplementedException();
			case 1:
				break;
			case 2:
			{
				int index = (int)(*(uint*)operation.PData);
				pResult += item[index].Deserialize(pResult);
				break;
			}
			case 3:
				break;
			case 4:
				break;
			case 5:
				break;
			case 6:
				break;
			case 7:
				break;
			case 8:
			{
				bool flag = item.Length != elementsCount;
				if (flag)
				{
					throw new Exception("Elements count is not equal to declaration");
				}
				pResult += 4;
				for (int i = 0; i < elementsCount; i++)
				{
					T element = item[i];
					pResult += element.Deserialize(pResult);
				}
				break;
			}
			default:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(21, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Unsupported methodId ");
				defaultInterpolatedStringHandler.AppendFormatted<byte>(operation.MethodId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			}
		}

		// Token: 0x06008300 RID: 33536 RVA: 0x004DE550 File Offset: 0x004DC750
		public unsafe static void ProcessElementList_CustomType_Dynamic_Value<T>(OperationWrapper operation, byte* pResult, T[] item, int elementsCount) where T : struct, ISerializableGameData
		{
			switch (operation.MethodId)
			{
			case 0:
				throw new NotImplementedException();
			case 1:
				break;
			case 2:
			{
				int index = (int)(*(uint*)operation.PData);
				pResult += 4;
				T element = Activator.CreateInstance<T>();
				element.Deserialize(pResult);
				item[index] = element;
				break;
			}
			case 3:
				break;
			case 4:
				break;
			case 5:
				break;
			case 6:
				break;
			case 7:
				break;
			case 8:
			{
				bool flag = item.Length != elementsCount;
				if (flag)
				{
					throw new Exception("Elements count is not equal to declaration");
				}
				pResult += 4;
				for (int i = 0; i < elementsCount; i++)
				{
					pResult += 4;
					T element2 = Activator.CreateInstance<T>();
					pResult += element2.Deserialize(pResult);
					item[i] = element2;
				}
				break;
			}
			default:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(21, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Unsupported methodId ");
				defaultInterpolatedStringHandler.AppendFormatted<byte>(operation.MethodId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			}
		}

		// Token: 0x06008301 RID: 33537 RVA: 0x004DE684 File Offset: 0x004DC884
		public unsafe static void ProcessElementList_CustomType_Dynamic_Ref<T>(OperationWrapper operation, byte* pResult, T[] item, int elementsCount) where T : class, ISerializableGameData, new()
		{
			switch (operation.MethodId)
			{
			case 0:
				throw new NotImplementedException();
			case 1:
				break;
			case 2:
			{
				int index = (int)(*(uint*)operation.PData);
				uint contentSize = *(uint*)pResult;
				pResult += 4;
				bool flag = contentSize > 0U;
				if (flag)
				{
					T element = item[index];
					bool flag2 = element != null;
					if (flag2)
					{
						pResult += element.Deserialize(pResult);
					}
					else
					{
						element = Activator.CreateInstance<T>();
						pResult += element.Deserialize(pResult);
						item[index] = element;
					}
				}
				else
				{
					item[index] = default(T);
				}
				break;
			}
			case 3:
				break;
			case 4:
				break;
			case 5:
				break;
			case 6:
				break;
			case 7:
				break;
			case 8:
			{
				bool flag3 = item.Length != elementsCount;
				if (flag3)
				{
					throw new Exception("Elements count is not equal to declaration");
				}
				pResult += 4;
				for (int i = 0; i < elementsCount; i++)
				{
					uint subContentSize = *(uint*)pResult;
					pResult += 4;
					bool flag4 = subContentSize > 0U;
					if (flag4)
					{
						T element2 = item[i];
						bool flag5 = element2 != null;
						if (flag5)
						{
							pResult += element2.Deserialize(pResult);
						}
						else
						{
							element2 = Activator.CreateInstance<T>();
							pResult += element2.Deserialize(pResult);
							item[i] = element2;
						}
					}
					else
					{
						item[i] = default(T);
					}
				}
				break;
			}
			default:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(21, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Unsupported methodId ");
				defaultInterpolatedStringHandler.AppendFormatted<byte>(operation.MethodId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			}
		}

		// Token: 0x06008302 RID: 33538 RVA: 0x004DE864 File Offset: 0x004DCA64
		public unsafe static void ProcessBinary(OperationWrapper operation, byte* pResult, IBinary block)
		{
			byte methodId = operation.MethodId;
			byte b = methodId;
			if (b > 3)
			{
				if (b != 4)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(21, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported methodId ");
					defaultInterpolatedStringHandler.AppendFormatted<byte>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				block.Clear();
				int dataSize = (int)(*(uint*)pResult);
				pResult += 4;
				block.Write(pResult, 0, dataSize);
				pResult += dataSize;
				pResult += block.DeserializeMetadata(pResult);
			}
		}

		// Token: 0x06008303 RID: 33539 RVA: 0x004DE8EC File Offset: 0x004DCAEC
		public unsafe static void Global_GetArchivesInfo(byte* pResult, out ArchiveInfo[] archivesInfo)
		{
			archivesInfo = new ArchiveInfo[5];
			for (int i = 0; i < 5; i++)
			{
				ArchiveInfo archiveInfo = new ArchiveInfo();
				archivesInfo[i] = archiveInfo;
				archiveInfo.Status = *(sbyte*)pResult;
				pResult++;
				bool flag = 1 == archiveInfo.Status;
				if (flag)
				{
					WorldInfo worldInfo;
					pResult += ResponseProcessor.DeserializeExtensibleWorldInfo(pResult, out worldInfo);
					archiveInfo.WorldInfo = worldInfo;
				}
				byte backupsCount = *pResult;
				pResult++;
				archiveInfo.BackupWorldsInfo = new List<ValueTuple<long, WorldInfo>>((int)backupsCount);
				for (int j = 0; j < (int)backupsCount; j++)
				{
					long timestamp = *(long*)pResult;
					pResult += 8;
					WorldInfo worldInfo2;
					pResult += ResponseProcessor.DeserializeExtensibleWorldInfo(pResult, out worldInfo2);
					archiveInfo.BackupWorldsInfo.Add(new ValueTuple<long, WorldInfo>(timestamp, worldInfo2));
				}
			}
		}

		// Token: 0x06008304 RID: 33540 RVA: 0x004DE9B0 File Offset: 0x004DCBB0
		public unsafe static void Global_GetEndingArchiveInfo(byte* pResult, out ArchiveInfo archiveInfo)
		{
			archiveInfo = new ArchiveInfo();
			archiveInfo.Status = *(sbyte*)pResult;
			pResult++;
			bool flag = archiveInfo.Status != 1;
			if (!flag)
			{
				WorldInfo worldInfo;
				pResult += ResponseProcessor.DeserializeExtensibleWorldInfo(pResult, out worldInfo);
				archiveInfo.WorldInfo = worldInfo;
				byte backupsCount = *pResult;
				pResult++;
				archiveInfo.BackupWorldsInfo = new List<ValueTuple<long, WorldInfo>>((int)backupsCount);
				for (int i = 0; i < (int)backupsCount; i++)
				{
					long timestamp = *(long*)pResult;
					pResult += 8;
					WorldInfo worldInfo2;
					pResult += ResponseProcessor.DeserializeExtensibleWorldInfo(pResult, out worldInfo2);
					archiveInfo.BackupWorldsInfo.Add(new ValueTuple<long, WorldInfo>(timestamp, worldInfo2));
				}
			}
		}

		// Token: 0x06008305 RID: 33541 RVA: 0x004DEA50 File Offset: 0x004DCC50
		private unsafe static int DeserializeExtensibleWorldInfo(byte* pData, out WorldInfo worldInfo)
		{
			bool isExtensible = *pData != 0;
			byte* pCurrData = pData + 1;
			bool flag = isExtensible;
			if (flag)
			{
				int worldInfoSize = *(int*)pCurrData;
				pCurrData += 4;
				bool flag2 = worldInfoSize > 0;
				if (flag2)
				{
					worldInfo = new WorldInfo();
					pCurrData += worldInfo.Deserialize(pCurrData);
				}
				else
				{
					worldInfo = null;
				}
			}
			else
			{
				ushort worldInfoSize2 = *(ushort*)pCurrData;
				pCurrData += 2;
				bool flag3 = worldInfoSize2 > 0;
				if (flag3)
				{
					FixedWorldInfo fixedWorldInfo = new FixedWorldInfo();
					pCurrData += fixedWorldInfo.Deserialize(pCurrData);
					worldInfo = new WorldInfo(fixedWorldInfo);
				}
				else
				{
					worldInfo = null;
				}
			}
			return (int)((long)(pCurrData - pData));
		}

		// Token: 0x06008306 RID: 33542 RVA: 0x004DEAE0 File Offset: 0x004DCCE0
		public unsafe static void LifeRecord_Get(byte* pResult, out ReadonlyLifeRecordsWithTotalCount records)
		{
			records = new ReadonlyLifeRecordsWithTotalCount();
			records.TotalCount = *(int*)pResult;
			pResult += 4;
			records.Records.Count = *(int*)pResult;
			pResult += 4;
			int dataSize = *(int*)pResult;
			pResult += 4;
			bool flag = dataSize > 0;
			if (flag)
			{
				byte[] rawData = new byte[dataSize];
				byte[] array;
				byte* pRawData;
				if ((array = rawData) == null || array.Length == 0)
				{
					pRawData = null;
				}
				else
				{
					pRawData = &array[0];
				}
				Buffer.MemoryCopy((void*)pResult, (void*)pRawData, (long)dataSize, (long)dataSize);
				array = null;
				records.Records.RawData = rawData;
				records.Records.Size = dataSize;
			}
		}

		// Token: 0x06008307 RID: 33543 RVA: 0x004DEB74 File Offset: 0x004DCD74
		public unsafe static void LifeRecord_GetByDate(byte* pResult, out ReadonlyLifeRecordsWithDate records)
		{
			records = new ReadonlyLifeRecordsWithDate();
			records.CharId = *(int*)pResult;
			pResult += 4;
			records.StartDate = *(int*)pResult;
			pResult += 4;
			records.MonthCount = *(int*)pResult;
			pResult += 4;
			records.Records.Count = *(int*)pResult;
			pResult += 4;
			int dataSize = *(int*)pResult;
			pResult += 4;
			bool flag = dataSize > 0;
			if (flag)
			{
				byte[] rawData = new byte[dataSize];
				byte[] array;
				byte* pRawData;
				if ((array = rawData) == null || array.Length == 0)
				{
					pRawData = null;
				}
				else
				{
					pRawData = &array[0];
				}
				Buffer.MemoryCopy((void*)pResult, (void*)pRawData, (long)dataSize, (long)dataSize);
				array = null;
				records.Records.RawData = rawData;
				records.Records.Size = dataSize;
			}
		}

		// Token: 0x06008308 RID: 33544 RVA: 0x004DEC24 File Offset: 0x004DCE24
		public unsafe static void LifeRecord_GetLast(byte* pResult, out ReadonlyLifeRecords records)
		{
			records = new ReadonlyLifeRecords();
			records.Count = *(int*)pResult;
			pResult += 4;
			int dataSize = *(int*)pResult;
			pResult += 4;
			bool flag = dataSize > 0;
			if (flag)
			{
				byte[] rawData = new byte[dataSize];
				byte[] array;
				byte* pRawData;
				if ((array = rawData) == null || array.Length == 0)
				{
					pRawData = null;
				}
				else
				{
					pRawData = &array[0];
				}
				Buffer.MemoryCopy((void*)pResult, (void*)pRawData, (long)dataSize, (long)dataSize);
				array = null;
				records.RawData = rawData;
				records.Size = dataSize;
			}
		}

		// Token: 0x06008309 RID: 33545 RVA: 0x004DEC9C File Offset: 0x004DCE9C
		public unsafe static void LifeRecord_Search(byte* pResult, out List<int> indexes)
		{
			indexes = new List<int>();
			int recordsCount = *(int*)pResult;
			pResult += 4;
			for (int i = 0; i < recordsCount; i++)
			{
				int index = *(int*)pResult;
				pResult += 4;
				indexes.Add(index);
			}
		}

		// Token: 0x0600830A RID: 33546 RVA: 0x004DECDC File Offset: 0x004DCEDC
		public unsafe static void LifeRecord_GetDead(byte* pResult, out ReadonlyLifeRecords records)
		{
			records = new ReadonlyLifeRecords();
			records.Count = *(int*)pResult;
			pResult += 4;
			int dataSize = *(int*)pResult;
			pResult += 4;
			bool flag = dataSize > 0;
			if (flag)
			{
				byte[] rawData = new byte[dataSize];
				byte[] array;
				byte* pRawData;
				if ((array = rawData) == null || array.Length == 0)
				{
					pRawData = null;
				}
				else
				{
					pRawData = &array[0];
				}
				Buffer.MemoryCopy((void*)pResult, (void*)pRawData, (long)dataSize, (long)dataSize);
				array = null;
				records.RawData = rawData;
				records.Size = dataSize;
			}
		}

		// Token: 0x0600830B RID: 33547 RVA: 0x004DED54 File Offset: 0x004DCF54
		public unsafe static void LifeRecord_GetAllByChar(byte* pResult, out ReadonlyLifeRecords records)
		{
			records = new ReadonlyLifeRecords();
			records.Count = *(int*)pResult;
			pResult += 4;
			int dataSize = *(int*)pResult;
			pResult += 4;
			bool flag = dataSize > 0;
			if (flag)
			{
				byte[] rawData = new byte[dataSize];
				byte[] array;
				byte* pRawData;
				if ((array = rawData) == null || array.Length == 0)
				{
					pRawData = null;
				}
				else
				{
					pRawData = &array[0];
				}
				Buffer.MemoryCopy((void*)pResult, (void*)pRawData, (long)dataSize, (long)dataSize);
				array = null;
				records.RawData = rawData;
				records.Size = dataSize;
			}
		}

		// Token: 0x0600830C RID: 33548 RVA: 0x004DEDCC File Offset: 0x004DCFCC
		public unsafe static void ProcessFixedObjectCollection<[IsUnmanaged] TKey, TValue>(OperationWrapper operation, byte* pResult, IDictionary<TKey, TValue> collection) where TKey : struct, ValueType where TValue : class, ISerializableGameData, new()
		{
			switch (operation.MethodId)
			{
			case 0:
				break;
			case 1:
				ResponseProcessor.FixedObjectCollection_Get<TKey, TValue>(operation, pResult, collection);
				break;
			case 2:
				ResponseProcessor.FixedObjectCollection_GetList<TKey, TValue>(operation, pResult, collection);
				break;
			case 3:
				break;
			case 4:
				break;
			case 5:
				break;
			case 6:
				ResponseProcessor.FixedObjectCollection_GetFixedField<TKey, TValue>(operation, pResult, collection);
				break;
			case 7:
				ResponseProcessor.FixedObjectCollection_GetAllIds<TKey, TValue>(operation, pResult, collection);
				break;
			case 8:
				ResponseProcessor.FixedObjectCollection_GetAllObjects<TKey, TValue>(operation, pResult, collection);
				break;
			default:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(21, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Unsupported methodId ");
				defaultInterpolatedStringHandler.AppendFormatted<byte>(operation.MethodId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			}
		}

		// Token: 0x0600830D RID: 33549 RVA: 0x004DEE84 File Offset: 0x004DD084
		public unsafe static void ProcessDynamicObjectCollection<[IsUnmanaged] TKey, TValue>(OperationWrapper operation, byte* pResult, IDictionary<TKey, TValue> collection) where TKey : struct, ValueType where TValue : class, ISerializableGameData, new()
		{
			switch (operation.MethodId)
			{
			case 0:
				break;
			case 1:
				ResponseProcessor.DynamicObjectCollection_Get<TKey, TValue>(operation, pResult, collection);
				break;
			case 2:
				ResponseProcessor.DynamicObjectCollection_GetList<TKey, TValue>(operation, pResult, collection);
				break;
			case 3:
				break;
			case 4:
				break;
			case 5:
				break;
			case 6:
				ResponseProcessor.DynamicObjectCollection_GetFixedField<TKey, TValue>(operation, pResult, collection);
				break;
			case 7:
				break;
			case 8:
				ResponseProcessor.DynamicObjectCollection_GetDynamicField<TKey, TValue>(operation, pResult, collection);
				break;
			case 9:
				ResponseProcessor.DynamicObjectCollection_GetAllIds<TKey, TValue>(operation, pResult, collection);
				break;
			case 10:
				ResponseProcessor.DynamicObjectCollection_GetAllObjects<TKey, TValue>(operation, pResult, collection);
				break;
			default:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(21, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Unsupported methodId ");
				defaultInterpolatedStringHandler.AppendFormatted<byte>(operation.MethodId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			}
		}

		// Token: 0x0600830E RID: 33550 RVA: 0x004DEF50 File Offset: 0x004DD150
		private unsafe static void FixedObjectCollection_Get<[IsUnmanaged] TKey, TValue>(OperationWrapper operation, byte* pResult, IDictionary<TKey, TValue> collection) where TKey : struct, ValueType where TValue : class, ISerializableGameData, new()
		{
			TKey objectId = *(TKey*)operation.PData;
			TValue instance;
			bool flag = collection.TryGetValue(objectId, out instance);
			if (flag)
			{
				instance.Deserialize(pResult);
			}
			else
			{
				instance = Activator.CreateInstance<TValue>();
				instance.Deserialize(pResult);
				collection.Add(objectId, instance);
			}
		}

		// Token: 0x0600830F RID: 33551 RVA: 0x004DEFA8 File Offset: 0x004DD1A8
		private unsafe static void FixedObjectCollection_GetList<[IsUnmanaged] TKey, TValue>(OperationWrapper operation, byte* pResult, IDictionary<TKey, TValue> collection) where TKey : struct, ValueType where TValue : class, ISerializableGameData, new()
		{
			int objectsCount = (int)(*(uint*)operation.PData);
			byte* pObjectId = operation.PData + 4;
			byte* pObjectData = pResult;
			for (int i = 0; i < objectsCount; i++)
			{
				TKey objectId = *(TKey*)pObjectId;
				pObjectId += sizeof(TKey);
				TValue instance;
				bool flag = collection.TryGetValue(objectId, out instance);
				if (flag)
				{
					pObjectData += instance.Deserialize(pObjectData);
				}
				else
				{
					instance = Activator.CreateInstance<TValue>();
					pObjectData += instance.Deserialize(pObjectData);
					collection.Add(objectId, instance);
				}
			}
		}

		// Token: 0x06008310 RID: 33552 RVA: 0x004DF036 File Offset: 0x004DD236
		private unsafe static void FixedObjectCollection_GetFixedField<[IsUnmanaged] TKey, TValue>(OperationWrapper operation, byte* pResult, IDictionary<TKey, TValue> collection) where TKey : struct, ValueType where TValue : class, ISerializableGameData, new()
		{
			throw new NotImplementedException();
		}

		// Token: 0x06008311 RID: 33553 RVA: 0x004DF040 File Offset: 0x004DD240
		private unsafe static void FixedObjectCollection_GetAllIds<[IsUnmanaged] TKey, TValue>(OperationWrapper operation, byte* pResult, IDictionary<TKey, TValue> collection) where TKey : struct, ValueType where TValue : class, ISerializableGameData, new()
		{
			collection.Clear();
			int objectsCount = (int)(*(uint*)pResult);
			byte* pObjectId = pResult + 4;
			for (int i = 0; i < objectsCount; i++)
			{
				TKey objectId = *(TKey*)pObjectId;
				pObjectId += sizeof(TKey);
				collection.Add(objectId, default(TValue));
			}
		}

		// Token: 0x06008312 RID: 33554 RVA: 0x004DF094 File Offset: 0x004DD294
		private unsafe static void FixedObjectCollection_GetAllObjects<[IsUnmanaged] TKey, TValue>(OperationWrapper operation, byte* pResult, IDictionary<TKey, TValue> collection) where TKey : struct, ValueType where TValue : class, ISerializableGameData, new()
		{
			collection.Clear();
			int objectsCount = (int)(*(uint*)pResult);
			byte* pObjectData = pResult + 4;
			for (int i = 0; i < objectsCount; i++)
			{
				TKey objectId = *(TKey*)pObjectData;
				pObjectData += sizeof(TKey);
				TValue instance = Activator.CreateInstance<TValue>();
				pObjectData += instance.Deserialize(pObjectData);
				collection.Add(objectId, instance);
			}
		}

		// Token: 0x06008313 RID: 33555 RVA: 0x004DF0F8 File Offset: 0x004DD2F8
		private unsafe static void DynamicObjectCollection_Get<[IsUnmanaged] TKey, TValue>(OperationWrapper operation, byte* pResult, IDictionary<TKey, TValue> collection) where TKey : struct, ValueType where TValue : class, ISerializableGameData, new()
		{
			TKey objectId = *(TKey*)operation.PData;
			TValue instance;
			bool flag = collection.TryGetValue(objectId, out instance);
			if (flag)
			{
				instance.Deserialize(pResult);
			}
			else
			{
				instance = Activator.CreateInstance<TValue>();
				instance.Deserialize(pResult);
				collection.Add(objectId, instance);
			}
		}

		// Token: 0x06008314 RID: 33556 RVA: 0x004DF150 File Offset: 0x004DD350
		private unsafe static void DynamicObjectCollection_GetList<[IsUnmanaged] TKey, TValue>(OperationWrapper operation, byte* pResult, IDictionary<TKey, TValue> collection) where TKey : struct, ValueType where TValue : class, ISerializableGameData, new()
		{
			int objectsCount = (int)(*(uint*)operation.PData);
			byte* pObjectId = operation.PData + 4;
			byte* pObjectData = pResult;
			for (int i = 0; i < objectsCount; i++)
			{
				TKey objectId = *(TKey*)pObjectId;
				pObjectId += sizeof(TKey);
				TValue instance;
				bool flag = collection.TryGetValue(objectId, out instance);
				if (flag)
				{
					pObjectData += instance.Deserialize(pObjectData);
				}
				else
				{
					instance = Activator.CreateInstance<TValue>();
					pObjectData += instance.Deserialize(pObjectData);
					collection.Add(objectId, instance);
				}
			}
		}

		// Token: 0x06008315 RID: 33557 RVA: 0x004DF1DE File Offset: 0x004DD3DE
		private unsafe static void DynamicObjectCollection_GetFixedField<[IsUnmanaged] TKey, TValue>(OperationWrapper operation, byte* pResult, IDictionary<TKey, TValue> collection) where TKey : struct, ValueType where TValue : class, ISerializableGameData, new()
		{
			throw new NotImplementedException();
		}

		// Token: 0x06008316 RID: 33558 RVA: 0x004DF1E6 File Offset: 0x004DD3E6
		private unsafe static void DynamicObjectCollection_GetDynamicField<[IsUnmanaged] TKey, TValue>(OperationWrapper operation, byte* pResult, IDictionary<TKey, TValue> collection) where TKey : struct, ValueType where TValue : class, ISerializableGameData, new()
		{
			throw new NotImplementedException();
		}

		// Token: 0x06008317 RID: 33559 RVA: 0x004DF1F0 File Offset: 0x004DD3F0
		private unsafe static void DynamicObjectCollection_GetAllIds<[IsUnmanaged] TKey, TValue>(OperationWrapper operation, byte* pResult, IDictionary<TKey, TValue> collection) where TKey : struct, ValueType where TValue : class, ISerializableGameData, new()
		{
			collection.Clear();
			int objectsCount = (int)(*(uint*)pResult);
			byte* pObjectId = pResult + 4;
			for (int i = 0; i < objectsCount; i++)
			{
				TKey objectId = *(TKey*)pObjectId;
				pObjectId += sizeof(TKey);
				collection.Add(objectId, default(TValue));
			}
		}

		// Token: 0x06008318 RID: 33560 RVA: 0x004DF244 File Offset: 0x004DD444
		private unsafe static void DynamicObjectCollection_GetAllObjects<[IsUnmanaged] TKey, TValue>(OperationWrapper operation, byte* pResult, IDictionary<TKey, TValue> collection) where TKey : struct, ValueType where TValue : class, ISerializableGameData, new()
		{
			collection.Clear();
			int objectsCount = (int)(*(uint*)pResult);
			byte* pObjectData = pResult + 4;
			for (int i = 0; i < objectsCount; i++)
			{
				TKey objectId = *(TKey*)pObjectData;
				pObjectData += sizeof(TKey);
				TValue instance = Activator.CreateInstance<TValue>();
				pObjectData += instance.Deserialize(pObjectData);
				collection.Add(objectId, instance);
			}
		}
	}
}
