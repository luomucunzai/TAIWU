using System;
using System.Collections.Generic;
using System.Text;
using GameData.Domains.Global;
using GameData.Domains.LifeRecord;
using GameData.Serializer;

namespace GameData.ArchiveData;

public static class ResponseProcessor
{
	public unsafe static void ProcessSingleValue_BasicType_Fixed_Value_Single<T>(OperationWrapper operation, byte* pResult, ref T item) where T : unmanaged
	{
		switch (operation.MethodId)
		{
		case 0:
			break;
		case 1:
			item = *(T*)pResult;
			pResult += sizeof(T);
			break;
		default:
			throw new Exception($"Unsupported methodId {operation.MethodId}");
		}
	}

	public unsafe static void ProcessSingleValue_BasicType_Fixed_Value_Array<T>(OperationWrapper operation, byte* pResult, T[] item, int elementsCount) where T : unmanaged
	{
		switch (operation.MethodId)
		{
		case 0:
			break;
		case 1:
		{
			if (item.Length != elementsCount)
			{
				throw new Exception("Elements count is not equal to declaration");
			}
			for (int i = 0; i < elementsCount; i++)
			{
				item[i] = ((T*)pResult)[i];
			}
			pResult += sizeof(T) * elementsCount;
			break;
		}
		default:
			throw new Exception($"Unsupported methodId {operation.MethodId}");
		}
	}

	public unsafe static void ProcessSingleValue_BasicType_Fixed_Value_List<T>(OperationWrapper operation, byte* pResult, List<T> item) where T : unmanaged
	{
		switch (operation.MethodId)
		{
		case 0:
			break;
		case 1:
		{
			uint num = *(uint*)pResult;
			pResult += 4;
			if (num != 0)
			{
				ushort num2 = *(ushort*)pResult;
				pResult += 2;
				item.Clear();
				for (int i = 0; i < num2; i++)
				{
					item.Add(((T*)pResult)[i]);
				}
				pResult += sizeof(T) * num2;
			}
			break;
		}
		default:
			throw new Exception($"Unsupported methodId {operation.MethodId}");
		}
	}

	public unsafe static void ProcessSingleValue_BasicType_String_Single(OperationWrapper operation, byte* pResult, ref string item)
	{
		switch (operation.MethodId)
		{
		case 0:
			break;
		case 1:
		{
			uint num = *(uint*)pResult;
			pResult += 4;
			item = Encoding.Unicode.GetString(pResult, (int)num);
			pResult += num;
			break;
		}
		default:
			throw new Exception($"Unsupported methodId {operation.MethodId}");
		}
	}

	public unsafe static void ProcessSingleValue_BasicType_String_Array(OperationWrapper operation, byte* pResult, string[] item, int elementsCount)
	{
		switch (operation.MethodId)
		{
		case 0:
			break;
		case 1:
		{
			uint num = *(uint*)pResult;
			pResult += 4;
			if (num == 0)
			{
				break;
			}
			if (item.Length != elementsCount)
			{
				throw new Exception("Elements count is not equal to declaration");
			}
			for (int i = 0; i < elementsCount; i++)
			{
				ushort num2 = *(ushort*)pResult;
				pResult += 2;
				if (num2 > 0)
				{
					item[i] = Encoding.Unicode.GetString(pResult, num2);
					pResult += (int)num2;
				}
				else
				{
					item[i] = null;
				}
			}
			break;
		}
		default:
			throw new Exception($"Unsupported methodId {operation.MethodId}");
		}
	}

	public unsafe static void ProcessSingleValue_BasicType_String_List(OperationWrapper operation, byte* pResult, List<string> item)
	{
		switch (operation.MethodId)
		{
		case 0:
			break;
		case 1:
		{
			uint num = *(uint*)pResult;
			pResult += 4;
			if (num == 0)
			{
				break;
			}
			ushort num2 = *(ushort*)pResult;
			pResult += 2;
			item.Clear();
			for (int i = 0; i < num2; i++)
			{
				ushort num3 = *(ushort*)pResult;
				pResult += 2;
				if (num3 > 0)
				{
					item.Add(Encoding.Unicode.GetString(pResult, num3));
					pResult += (int)num3;
				}
				else
				{
					item.Add(null);
				}
			}
			break;
		}
		default:
			throw new Exception($"Unsupported methodId {operation.MethodId}");
		}
	}

	public unsafe static void ProcessSingleValue_CustomType_Fixed_Value_Single<T>(OperationWrapper operation, byte* pResult, ref T item) where T : struct, ISerializableGameData
	{
		switch (operation.MethodId)
		{
		case 0:
			break;
		case 1:
			pResult += ((ISerializableGameData)item/*cast due to .constrained prefix*/).Deserialize(pResult);
			break;
		default:
			throw new Exception($"Unsupported methodId {operation.MethodId}");
		}
	}

	public unsafe static void ProcessSingleValue_CustomType_Fixed_Value_Array<T>(OperationWrapper operation, byte* pResult, T[] item, int elementsCount, int elementFixedSize) where T : struct, ISerializableGameData
	{
		switch (operation.MethodId)
		{
		case 0:
			break;
		case 1:
		{
			if (item.Length != elementsCount)
			{
				throw new Exception("Elements count is not equal to declaration");
			}
			for (int i = 0; i < elementsCount; i++)
			{
				T val = new T();
				pResult += ((ISerializableGameData)val/*cast due to .constrained prefix*/).Deserialize(pResult);
				item[i] = val;
			}
			break;
		}
		default:
			throw new Exception($"Unsupported methodId {operation.MethodId}");
		}
	}

	public unsafe static void ProcessSingleValue_CustomType_Fixed_Value_List<T>(OperationWrapper operation, byte* pResult, List<T> item, int elementFixedSize) where T : struct, ISerializableGameData
	{
		switch (operation.MethodId)
		{
		case 0:
			break;
		case 1:
		{
			uint num = *(uint*)pResult;
			pResult += 4;
			if (num != 0)
			{
				ushort num2 = *(ushort*)pResult;
				pResult += 2;
				item.Clear();
				for (int i = 0; i < num2; i++)
				{
					T item2 = new T();
					pResult += ((ISerializableGameData)item2/*cast due to .constrained prefix*/).Deserialize(pResult);
					item.Add(item2);
				}
			}
			break;
		}
		default:
			throw new Exception($"Unsupported methodId {operation.MethodId}");
		}
	}

	public unsafe static void ProcessSingleValue_CustomType_Fixed_Ref_Single<T>(OperationWrapper operation, byte* pResult, T item) where T : class, ISerializableGameData, new()
	{
		switch (operation.MethodId)
		{
		case 0:
			break;
		case 1:
			pResult += ((ISerializableGameData)item).Deserialize(pResult);
			break;
		default:
			throw new Exception($"Unsupported methodId {operation.MethodId}");
		}
	}

	public unsafe static void ProcessSingleValue_CustomType_Fixed_Ref_Array<T>(OperationWrapper operation, byte* pResult, T[] item, int elementsCount, int elementFixedSize) where T : class, ISerializableGameData, new()
	{
		switch (operation.MethodId)
		{
		case 0:
			break;
		case 1:
		{
			if (item.Length != elementsCount)
			{
				throw new Exception("Elements count is not equal to declaration");
			}
			for (int i = 0; i < elementsCount; i++)
			{
				T val = item[i];
				pResult += ((ISerializableGameData)val).Deserialize(pResult);
			}
			break;
		}
		default:
			throw new Exception($"Unsupported methodId {operation.MethodId}");
		}
	}

	public unsafe static void ProcessSingleValue_CustomType_Fixed_Ref_List<T>(OperationWrapper operation, byte* pResult, List<T> item, int elementFixedSize) where T : class, ISerializableGameData, new()
	{
		switch (operation.MethodId)
		{
		case 0:
			break;
		case 1:
		{
			uint num = *(uint*)pResult;
			pResult += 4;
			if (num == 0)
			{
				break;
			}
			ushort num2 = *(ushort*)pResult;
			pResult += 2;
			if (num2 > 0)
			{
				int count = item.Count;
				if (num2 < count)
				{
					item.RemoveRange(num2, count - num2);
				}
				for (int i = 0; i < num2; i++)
				{
					if (i < count)
					{
						T val = item[i];
						pResult += ((ISerializableGameData)val).Deserialize(pResult);
					}
					else
					{
						T val2 = new T();
						pResult += ((ISerializableGameData)val2).Deserialize(pResult);
						item.Add(val2);
					}
				}
			}
			else
			{
				item.Clear();
			}
			break;
		}
		default:
			throw new Exception($"Unsupported methodId {operation.MethodId}");
		}
	}

	public unsafe static void ProcessSingleValue_CustomType_Dynamic_Value_Single<T>(OperationWrapper operation, byte* pResult, ref T item) where T : struct, ISerializableGameData
	{
		switch (operation.MethodId)
		{
		case 0:
			break;
		case 1:
		{
			uint num = *(uint*)pResult;
			pResult += 4;
			if (num != 0)
			{
				pResult += ((ISerializableGameData)item/*cast due to .constrained prefix*/).Deserialize(pResult);
			}
			break;
		}
		default:
			throw new Exception($"Unsupported methodId {operation.MethodId}");
		}
	}

	public unsafe static void ProcessSingleValue_CustomType_Dynamic_Value_Array<T>(OperationWrapper operation, byte* pResult, T[] item, int elementsCount) where T : struct, ISerializableGameData
	{
		switch (operation.MethodId)
		{
		case 0:
			break;
		case 1:
		{
			uint num = *(uint*)pResult;
			pResult += 4;
			if (num != 0)
			{
				if (item.Length != elementsCount)
				{
					throw new Exception("Elements count is not equal to declaration");
				}
				for (int i = 0; i < elementsCount; i++)
				{
					T val = new T();
					pResult += ((ISerializableGameData)val/*cast due to .constrained prefix*/).Deserialize(pResult);
					item[i] = val;
				}
			}
			break;
		}
		default:
			throw new Exception($"Unsupported methodId {operation.MethodId}");
		}
	}

	public unsafe static void ProcessSingleValue_CustomType_Dynamic_Value_List<T>(OperationWrapper operation, byte* pResult, List<T> item, int elementFixedSize) where T : struct, ISerializableGameData
	{
		switch (operation.MethodId)
		{
		case 0:
			break;
		case 1:
		{
			uint num = *(uint*)pResult;
			pResult += 4;
			if (num != 0)
			{
				ushort num2 = *(ushort*)pResult;
				pResult += 2;
				item.Clear();
				for (int i = 0; i < num2; i++)
				{
					T item2 = new T();
					pResult += ((ISerializableGameData)item2/*cast due to .constrained prefix*/).Deserialize(pResult);
					item.Add(item2);
				}
			}
			break;
		}
		default:
			throw new Exception($"Unsupported methodId {operation.MethodId}");
		}
	}

	public unsafe static void ProcessSingleValue_CustomType_Dynamic_Ref_Single<T>(OperationWrapper operation, byte* pResult, T item) where T : class, ISerializableGameData, new()
	{
		switch (operation.MethodId)
		{
		case 0:
			break;
		case 1:
		{
			uint num = *(uint*)pResult;
			pResult += 4;
			if (num != 0)
			{
				pResult += ((ISerializableGameData)item).Deserialize(pResult);
			}
			break;
		}
		default:
			throw new Exception($"Unsupported methodId {operation.MethodId}");
		}
	}

	public unsafe static void ProcessSingleValue_CustomType_Dynamic_Ref_Array<T>(OperationWrapper operation, byte* pResult, T[] item, int elementsCount) where T : class, ISerializableGameData, new()
	{
		switch (operation.MethodId)
		{
		case 0:
			break;
		case 1:
		{
			uint num = *(uint*)pResult;
			pResult += 4;
			if (num == 0)
			{
				break;
			}
			if (item.Length != elementsCount)
			{
				throw new Exception("Elements count is not equal to declaration");
			}
			for (int i = 0; i < elementsCount; i++)
			{
				ushort num2 = *(ushort*)pResult;
				pResult += 2;
				if (num2 > 0)
				{
					T val = item[i];
					if (val != null)
					{
						pResult += ((ISerializableGameData)val).Deserialize(pResult);
						continue;
					}
					val = new T();
					pResult += ((ISerializableGameData)val).Deserialize(pResult);
					item[i] = val;
				}
				else
				{
					item[i] = null;
				}
			}
			break;
		}
		default:
			throw new Exception($"Unsupported methodId {operation.MethodId}");
		}
	}

	public unsafe static void ProcessSingleValue_CustomType_Dynamic_Ref_List<T>(OperationWrapper operation, byte* pResult, List<T> item) where T : class, ISerializableGameData, new()
	{
		switch (operation.MethodId)
		{
		case 0:
			break;
		case 1:
		{
			uint num = *(uint*)pResult;
			pResult += 4;
			if (num == 0)
			{
				break;
			}
			ushort num2 = *(ushort*)pResult;
			pResult += 2;
			if (num2 > 0)
			{
				int count = item.Count;
				if (num2 < count)
				{
					item.RemoveRange(num2, count - num2);
				}
				for (int i = 0; i < num2; i++)
				{
					ushort num3 = *(ushort*)pResult;
					pResult += 2;
					if (num3 > 0)
					{
						if (i < count)
						{
							T val = item[i];
							if (val != null)
							{
								pResult += ((ISerializableGameData)val).Deserialize(pResult);
								continue;
							}
							val = new T();
							pResult += ((ISerializableGameData)val).Deserialize(pResult);
							item[i] = val;
						}
						else
						{
							T val2 = new T();
							pResult += ((ISerializableGameData)val2).Deserialize(pResult);
							item.Add(val2);
						}
					}
					else if (i < count)
					{
						item[i] = null;
					}
					else
					{
						item.Add(null);
					}
				}
			}
			else
			{
				item.Clear();
			}
			break;
		}
		default:
			throw new Exception($"Unsupported methodId {operation.MethodId}");
		}
	}

	public unsafe static void ProcessSingleValueCollection_BasicType_Fixed_Value<TKey, TValue>(OperationWrapper operation, byte* pResult, IDictionary<TKey, TValue> collection) where TKey : unmanaged where TValue : unmanaged
	{
		switch (operation.MethodId)
		{
		case 0:
			break;
		case 1:
			break;
		case 2:
		{
			TKey pData = *(TKey*)operation.PData;
			collection[pData] = *(TValue*)pResult;
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
			int num = *(int*)pResult;
			pResult += 4;
			for (int i = 0; i < num; i++)
			{
				TKey key = *(TKey*)pResult;
				pResult += sizeof(TKey);
				collection.Add(key, *(TValue*)pResult);
				pResult += sizeof(TValue);
			}
			break;
		}
		default:
			throw new Exception($"Unsupported methodId {operation.MethodId}");
		}
	}

	public unsafe static void ProcessSingleValueCollection_BasicType_String<TKey>(OperationWrapper operation, byte* pResult, IDictionary<TKey, string> collection) where TKey : unmanaged
	{
		switch (operation.MethodId)
		{
		case 0:
			break;
		case 1:
			break;
		case 2:
		{
			TKey pData = *(TKey*)operation.PData;
			uint num3 = *(uint*)pResult;
			pResult += 4;
			if (num3 != 0)
			{
				collection[pData] = Encoding.Unicode.GetString(pResult, (int)num3);
				pResult += num3;
			}
			else
			{
				collection[pData] = null;
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
			int num = *(int*)pResult;
			pResult += 4;
			for (int i = 0; i < num; i++)
			{
				TKey key = *(TKey*)pResult;
				pResult += sizeof(TKey);
				uint num2 = *(uint*)pResult;
				pResult += 4;
				if (num2 != 0)
				{
					collection.Add(key, Encoding.Unicode.GetString(pResult, (int)num2));
					pResult += num2;
				}
				else
				{
					collection.Add(key, null);
				}
			}
			break;
		}
		default:
			throw new Exception($"Unsupported methodId {operation.MethodId}");
		}
	}

	public unsafe static void ProcessSingleValueCollection_CustomType_Fixed_Value<TKey, TValue>(OperationWrapper operation, byte* pResult, IDictionary<TKey, TValue> collection, int elementFixedSize) where TKey : unmanaged where TValue : struct, ISerializableGameData
	{
		switch (operation.MethodId)
		{
		case 0:
			break;
		case 1:
			break;
		case 2:
		{
			TKey pData = *(TKey*)operation.PData;
			TValue value2 = new TValue();
			((ISerializableGameData)value2/*cast due to .constrained prefix*/).Deserialize(pResult);
			collection[pData] = value2;
			break;
		}
		case 3:
			break;
		case 4:
			break;
		case 5:
		{
			collection.Clear();
			int num = *(int*)pResult;
			pResult += 4;
			for (int i = 0; i < num; i++)
			{
				TKey key = *(TKey*)pResult;
				pResult += sizeof(TKey);
				TValue value = new TValue();
				pResult += ((ISerializableGameData)value/*cast due to .constrained prefix*/).Deserialize(pResult);
				collection.Add(key, value);
			}
			break;
		}
		default:
			throw new Exception($"Unsupported methodId {operation.MethodId}");
		}
	}

	public unsafe static void ProcessSingleValueCollection_CustomType_Fixed_Ref<TKey, TValue>(OperationWrapper operation, byte* pResult, IDictionary<TKey, TValue> collection, int elementFixedSize) where TKey : unmanaged where TValue : class, ISerializableGameData, new()
	{
		switch (operation.MethodId)
		{
		case 0:
			break;
		case 1:
			break;
		case 2:
		{
			TKey pData = *(TKey*)operation.PData;
			TValue val2 = new TValue();
			((ISerializableGameData)val2).Deserialize(pResult);
			collection[pData] = val2;
			break;
		}
		case 3:
			break;
		case 4:
			break;
		case 5:
		{
			collection.Clear();
			int num = *(int*)pResult;
			pResult += 4;
			for (int i = 0; i < num; i++)
			{
				TKey key = *(TKey*)pResult;
				pResult += sizeof(TKey);
				TValue val = new TValue();
				pResult += ((ISerializableGameData)val).Deserialize(pResult);
				collection.Add(key, val);
			}
			break;
		}
		default:
			throw new Exception($"Unsupported methodId {operation.MethodId}");
		}
	}

	public unsafe static void ProcessSingleValueCollection_CustomType_Dynamic_Value<TKey, TValue>(OperationWrapper operation, byte* pResult, IDictionary<TKey, TValue> collection) where TKey : unmanaged where TValue : struct, ISerializableGameData
	{
		switch (operation.MethodId)
		{
		case 0:
			break;
		case 1:
			break;
		case 2:
		{
			TKey pData = *(TKey*)operation.PData;
			pResult += 4;
			TValue value2 = new TValue();
			((ISerializableGameData)value2/*cast due to .constrained prefix*/).Deserialize(pResult);
			collection[pData] = value2;
			break;
		}
		case 3:
			break;
		case 4:
			break;
		case 5:
		{
			collection.Clear();
			int num = *(int*)pResult;
			pResult += 4;
			for (int i = 0; i < num; i++)
			{
				TKey key = *(TKey*)pResult;
				pResult += sizeof(TKey);
				pResult += 4;
				TValue value = new TValue();
				pResult += ((ISerializableGameData)value/*cast due to .constrained prefix*/).Deserialize(pResult);
				collection.Add(key, value);
			}
			break;
		}
		default:
			throw new Exception($"Unsupported methodId {operation.MethodId}");
		}
	}

	public unsafe static void ProcessSingleValueCollection_CustomType_Dynamic_Ref<TKey, TValue>(OperationWrapper operation, byte* pResult, IDictionary<TKey, TValue> collection) where TKey : unmanaged where TValue : class, ISerializableGameData, new()
	{
		switch (operation.MethodId)
		{
		case 0:
			break;
		case 1:
			break;
		case 2:
		{
			TKey pData = *(TKey*)operation.PData;
			uint num3 = *(uint*)pResult;
			pResult += 4;
			if (num3 != 0)
			{
				TValue val2 = new TValue();
				pResult += ((ISerializableGameData)val2).Deserialize(pResult);
				collection[pData] = val2;
			}
			else
			{
				collection[pData] = null;
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
			int num = *(int*)pResult;
			pResult += 4;
			for (int i = 0; i < num; i++)
			{
				TKey key = *(TKey*)pResult;
				pResult += sizeof(TKey);
				uint num2 = *(uint*)pResult;
				pResult += 4;
				if (num2 != 0)
				{
					TValue val = new TValue();
					pResult += ((ISerializableGameData)val).Deserialize(pResult);
					collection.Add(key, val);
				}
				else
				{
					collection.Add(key, null);
				}
			}
			break;
		}
		default:
			throw new Exception($"Unsupported methodId {operation.MethodId}");
		}
	}

	public unsafe static void ProcessElementList_BasicType_Fixed_Value<T>(OperationWrapper operation, byte* pResult, T[] item, int elementsCount, int elementFixedSize) where T : unmanaged
	{
		switch (operation.MethodId)
		{
		case 0:
			throw new NotImplementedException();
		case 1:
			break;
		case 2:
		{
			int pData = *(int*)operation.PData;
			item[pData] = *(T*)pResult;
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
			if (item.Length != elementsCount)
			{
				throw new Exception("Elements count is not equal to declaration");
			}
			pResult += 4;
			for (int i = 0; i < elementsCount; i++)
			{
				item[i] = ((T*)pResult)[i];
			}
			pResult += sizeof(T) * elementsCount;
			break;
		}
		default:
			throw new Exception($"Unsupported methodId {operation.MethodId}");
		}
	}

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
			int pData = *(int*)operation.PData;
			uint num2 = *(uint*)pResult;
			pResult += 4;
			if (num2 != 0)
			{
				item[pData] = Encoding.Unicode.GetString(pResult, (int)num2);
				pResult += num2;
			}
			else
			{
				item[pData] = null;
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
			if (item.Length != elementsCount)
			{
				throw new Exception("Elements count is not equal to declaration");
			}
			pResult += 4;
			for (int i = 0; i < elementsCount; i++)
			{
				uint num = *(uint*)pResult;
				pResult += 4;
				if (num != 0)
				{
					item[i] = Encoding.Unicode.GetString(pResult, (int)num);
					pResult += num;
				}
				else
				{
					item[i] = null;
				}
			}
			break;
		}
		default:
			throw new Exception($"Unsupported methodId {operation.MethodId}");
		}
	}

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
			int pData = *(int*)operation.PData;
			T val2 = new T();
			((ISerializableGameData)val2/*cast due to .constrained prefix*/).Deserialize(pResult);
			item[pData] = val2;
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
			if (item.Length != elementsCount)
			{
				throw new Exception("Elements count is not equal to declaration");
			}
			pResult += 4;
			for (int i = 0; i < elementsCount; i++)
			{
				T val = new T();
				pResult += ((ISerializableGameData)val/*cast due to .constrained prefix*/).Deserialize(pResult);
				item[i] = val;
			}
			break;
		}
		default:
			throw new Exception($"Unsupported methodId {operation.MethodId}");
		}
	}

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
			int pData = *(int*)operation.PData;
			pResult += ((ISerializableGameData)item[pData]).Deserialize(pResult);
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
			if (item.Length != elementsCount)
			{
				throw new Exception("Elements count is not equal to declaration");
			}
			pResult += 4;
			for (int i = 0; i < elementsCount; i++)
			{
				T val = item[i];
				pResult += ((ISerializableGameData)val).Deserialize(pResult);
			}
			break;
		}
		default:
			throw new Exception($"Unsupported methodId {operation.MethodId}");
		}
	}

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
			int pData = *(int*)operation.PData;
			pResult += 4;
			T val2 = new T();
			((ISerializableGameData)val2/*cast due to .constrained prefix*/).Deserialize(pResult);
			item[pData] = val2;
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
			if (item.Length != elementsCount)
			{
				throw new Exception("Elements count is not equal to declaration");
			}
			pResult += 4;
			for (int i = 0; i < elementsCount; i++)
			{
				pResult += 4;
				T val = new T();
				pResult += ((ISerializableGameData)val/*cast due to .constrained prefix*/).Deserialize(pResult);
				item[i] = val;
			}
			break;
		}
		default:
			throw new Exception($"Unsupported methodId {operation.MethodId}");
		}
	}

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
			int pData = *(int*)operation.PData;
			uint num2 = *(uint*)pResult;
			pResult += 4;
			if (num2 != 0)
			{
				T val2 = item[pData];
				if (val2 != null)
				{
					pResult += ((ISerializableGameData)val2).Deserialize(pResult);
					break;
				}
				val2 = new T();
				pResult += ((ISerializableGameData)val2).Deserialize(pResult);
				item[pData] = val2;
			}
			else
			{
				item[pData] = null;
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
			if (item.Length != elementsCount)
			{
				throw new Exception("Elements count is not equal to declaration");
			}
			pResult += 4;
			for (int i = 0; i < elementsCount; i++)
			{
				uint num = *(uint*)pResult;
				pResult += 4;
				if (num != 0)
				{
					T val = item[i];
					if (val != null)
					{
						pResult += ((ISerializableGameData)val).Deserialize(pResult);
						continue;
					}
					val = new T();
					pResult += ((ISerializableGameData)val).Deserialize(pResult);
					item[i] = val;
				}
				else
				{
					item[i] = null;
				}
			}
			break;
		}
		default:
			throw new Exception($"Unsupported methodId {operation.MethodId}");
		}
	}

	public unsafe static void ProcessBinary(OperationWrapper operation, byte* pResult, IBinary block)
	{
		switch (operation.MethodId)
		{
		case 0:
		case 1:
		case 2:
		case 3:
			break;
		case 4:
		{
			block.Clear();
			int num = *(int*)pResult;
			pResult += 4;
			block.Write(pResult, 0, num);
			pResult += num;
			pResult += block.DeserializeMetadata(pResult);
			break;
		}
		default:
			throw new Exception($"Unsupported methodId {operation.MethodId}");
		}
	}

	public unsafe static void Global_GetArchivesInfo(byte* pResult, out ArchiveInfo[] archivesInfo)
	{
		archivesInfo = new ArchiveInfo[5];
		for (int i = 0; i < 5; i++)
		{
			ArchiveInfo archiveInfo = new ArchiveInfo();
			archivesInfo[i] = archiveInfo;
			archiveInfo.Status = (sbyte)(*pResult);
			pResult++;
			if (1 == archiveInfo.Status)
			{
				pResult += DeserializeExtensibleWorldInfo(pResult, out var worldInfo);
				archiveInfo.WorldInfo = worldInfo;
			}
			byte b = *pResult;
			pResult++;
			archiveInfo.BackupWorldsInfo = new List<(long, WorldInfo)>(b);
			for (int j = 0; j < b; j++)
			{
				long item = *(long*)pResult;
				pResult += 8;
				pResult += DeserializeExtensibleWorldInfo(pResult, out var worldInfo2);
				archiveInfo.BackupWorldsInfo.Add((item, worldInfo2));
			}
		}
	}

	public unsafe static void Global_GetEndingArchiveInfo(byte* pResult, out ArchiveInfo archiveInfo)
	{
		archiveInfo = new ArchiveInfo();
		archiveInfo.Status = (sbyte)(*pResult);
		pResult++;
		if (archiveInfo.Status == 1)
		{
			pResult += DeserializeExtensibleWorldInfo(pResult, out var worldInfo);
			archiveInfo.WorldInfo = worldInfo;
			byte b = *pResult;
			pResult++;
			archiveInfo.BackupWorldsInfo = new List<(long, WorldInfo)>(b);
			for (int i = 0; i < b; i++)
			{
				long item = *(long*)pResult;
				pResult += 8;
				pResult += DeserializeExtensibleWorldInfo(pResult, out var worldInfo2);
				archiveInfo.BackupWorldsInfo.Add((item, worldInfo2));
			}
		}
	}

	private unsafe static int DeserializeExtensibleWorldInfo(byte* pData, out WorldInfo worldInfo)
	{
		byte* ptr = pData;
		bool flag = *pData != 0;
		ptr++;
		if (flag)
		{
			int num = *(int*)ptr;
			ptr += 4;
			if (num > 0)
			{
				worldInfo = new WorldInfo();
				ptr += worldInfo.Deserialize(ptr);
			}
			else
			{
				worldInfo = null;
			}
		}
		else
		{
			ushort num2 = *(ushort*)ptr;
			ptr += 2;
			if (num2 > 0)
			{
				FixedWorldInfo fixedWorldInfo = new FixedWorldInfo();
				ptr += fixedWorldInfo.Deserialize(ptr);
				worldInfo = new WorldInfo(fixedWorldInfo);
			}
			else
			{
				worldInfo = null;
			}
		}
		return (int)(ptr - pData);
	}

	public unsafe static void LifeRecord_Get(byte* pResult, out ReadonlyLifeRecordsWithTotalCount records)
	{
		records = new ReadonlyLifeRecordsWithTotalCount();
		records.TotalCount = *(int*)pResult;
		pResult += 4;
		records.Records.Count = *(int*)pResult;
		pResult += 4;
		int num = *(int*)pResult;
		pResult += 4;
		if (num > 0)
		{
			byte[] array = new byte[num];
			fixed (byte* destination = array)
			{
				Buffer.MemoryCopy(pResult, destination, num, num);
			}
			records.Records.RawData = array;
			records.Records.Size = num;
		}
	}

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
		int num = *(int*)pResult;
		pResult += 4;
		if (num > 0)
		{
			byte[] array = new byte[num];
			fixed (byte* destination = array)
			{
				Buffer.MemoryCopy(pResult, destination, num, num);
			}
			records.Records.RawData = array;
			records.Records.Size = num;
		}
	}

	public unsafe static void LifeRecord_GetLast(byte* pResult, out ReadonlyLifeRecords records)
	{
		records = new ReadonlyLifeRecords();
		records.Count = *(int*)pResult;
		pResult += 4;
		int num = *(int*)pResult;
		pResult += 4;
		if (num > 0)
		{
			byte[] array = new byte[num];
			fixed (byte* destination = array)
			{
				Buffer.MemoryCopy(pResult, destination, num, num);
			}
			records.RawData = array;
			records.Size = num;
		}
	}

	public unsafe static void LifeRecord_Search(byte* pResult, out List<int> indexes)
	{
		indexes = new List<int>();
		int num = *(int*)pResult;
		pResult += 4;
		for (int i = 0; i < num; i++)
		{
			int item = *(int*)pResult;
			pResult += 4;
			indexes.Add(item);
		}
	}

	public unsafe static void LifeRecord_GetDead(byte* pResult, out ReadonlyLifeRecords records)
	{
		records = new ReadonlyLifeRecords();
		records.Count = *(int*)pResult;
		pResult += 4;
		int num = *(int*)pResult;
		pResult += 4;
		if (num > 0)
		{
			byte[] array = new byte[num];
			fixed (byte* destination = array)
			{
				Buffer.MemoryCopy(pResult, destination, num, num);
			}
			records.RawData = array;
			records.Size = num;
		}
	}

	public unsafe static void LifeRecord_GetAllByChar(byte* pResult, out ReadonlyLifeRecords records)
	{
		records = new ReadonlyLifeRecords();
		records.Count = *(int*)pResult;
		pResult += 4;
		int num = *(int*)pResult;
		pResult += 4;
		if (num > 0)
		{
			byte[] array = new byte[num];
			fixed (byte* destination = array)
			{
				Buffer.MemoryCopy(pResult, destination, num, num);
			}
			records.RawData = array;
			records.Size = num;
		}
	}

	public unsafe static void ProcessFixedObjectCollection<TKey, TValue>(OperationWrapper operation, byte* pResult, IDictionary<TKey, TValue> collection) where TKey : unmanaged where TValue : class, ISerializableGameData, new()
	{
		switch (operation.MethodId)
		{
		case 0:
			break;
		case 1:
			FixedObjectCollection_Get(operation, pResult, collection);
			break;
		case 2:
			FixedObjectCollection_GetList(operation, pResult, collection);
			break;
		case 3:
			break;
		case 4:
			break;
		case 5:
			break;
		case 6:
			FixedObjectCollection_GetFixedField(operation, pResult, collection);
			break;
		case 7:
			FixedObjectCollection_GetAllIds(operation, pResult, collection);
			break;
		case 8:
			FixedObjectCollection_GetAllObjects(operation, pResult, collection);
			break;
		default:
			throw new Exception($"Unsupported methodId {operation.MethodId}");
		}
	}

	public unsafe static void ProcessDynamicObjectCollection<TKey, TValue>(OperationWrapper operation, byte* pResult, IDictionary<TKey, TValue> collection) where TKey : unmanaged where TValue : class, ISerializableGameData, new()
	{
		switch (operation.MethodId)
		{
		case 0:
			break;
		case 1:
			DynamicObjectCollection_Get(operation, pResult, collection);
			break;
		case 2:
			DynamicObjectCollection_GetList(operation, pResult, collection);
			break;
		case 3:
			break;
		case 4:
			break;
		case 5:
			break;
		case 6:
			DynamicObjectCollection_GetFixedField(operation, pResult, collection);
			break;
		case 7:
			break;
		case 8:
			DynamicObjectCollection_GetDynamicField(operation, pResult, collection);
			break;
		case 9:
			DynamicObjectCollection_GetAllIds(operation, pResult, collection);
			break;
		case 10:
			DynamicObjectCollection_GetAllObjects(operation, pResult, collection);
			break;
		default:
			throw new Exception($"Unsupported methodId {operation.MethodId}");
		}
	}

	private unsafe static void FixedObjectCollection_Get<TKey, TValue>(OperationWrapper operation, byte* pResult, IDictionary<TKey, TValue> collection) where TKey : unmanaged where TValue : class, ISerializableGameData, new()
	{
		TKey pData = *(TKey*)operation.PData;
		if (collection.TryGetValue(pData, out var value))
		{
			((ISerializableGameData)value).Deserialize(pResult);
			return;
		}
		value = new TValue();
		((ISerializableGameData)value).Deserialize(pResult);
		collection.Add(pData, value);
	}

	private unsafe static void FixedObjectCollection_GetList<TKey, TValue>(OperationWrapper operation, byte* pResult, IDictionary<TKey, TValue> collection) where TKey : unmanaged where TValue : class, ISerializableGameData, new()
	{
		int pData = *(int*)operation.PData;
		byte* ptr = operation.PData + 4;
		byte* ptr2 = pResult;
		for (int i = 0; i < pData; i++)
		{
			TKey key = *(TKey*)ptr;
			ptr += sizeof(TKey);
			if (collection.TryGetValue(key, out var value))
			{
				ptr2 += ((ISerializableGameData)value).Deserialize(ptr2);
				continue;
			}
			value = new TValue();
			ptr2 += ((ISerializableGameData)value).Deserialize(ptr2);
			collection.Add(key, value);
		}
	}

	private unsafe static void FixedObjectCollection_GetFixedField<TKey, TValue>(OperationWrapper operation, byte* pResult, IDictionary<TKey, TValue> collection) where TKey : unmanaged where TValue : class, ISerializableGameData, new()
	{
		throw new NotImplementedException();
	}

	private unsafe static void FixedObjectCollection_GetAllIds<TKey, TValue>(OperationWrapper operation, byte* pResult, IDictionary<TKey, TValue> collection) where TKey : unmanaged where TValue : class, ISerializableGameData, new()
	{
		collection.Clear();
		int num = *(int*)pResult;
		byte* ptr = pResult + 4;
		for (int i = 0; i < num; i++)
		{
			TKey key = *(TKey*)ptr;
			ptr += sizeof(TKey);
			collection.Add(key, null);
		}
	}

	private unsafe static void FixedObjectCollection_GetAllObjects<TKey, TValue>(OperationWrapper operation, byte* pResult, IDictionary<TKey, TValue> collection) where TKey : unmanaged where TValue : class, ISerializableGameData, new()
	{
		collection.Clear();
		int num = *(int*)pResult;
		byte* ptr = pResult + 4;
		for (int i = 0; i < num; i++)
		{
			TKey key = *(TKey*)ptr;
			ptr += sizeof(TKey);
			TValue val = new TValue();
			ptr += ((ISerializableGameData)val).Deserialize(ptr);
			collection.Add(key, val);
		}
	}

	private unsafe static void DynamicObjectCollection_Get<TKey, TValue>(OperationWrapper operation, byte* pResult, IDictionary<TKey, TValue> collection) where TKey : unmanaged where TValue : class, ISerializableGameData, new()
	{
		TKey pData = *(TKey*)operation.PData;
		if (collection.TryGetValue(pData, out var value))
		{
			((ISerializableGameData)value).Deserialize(pResult);
			return;
		}
		value = new TValue();
		((ISerializableGameData)value).Deserialize(pResult);
		collection.Add(pData, value);
	}

	private unsafe static void DynamicObjectCollection_GetList<TKey, TValue>(OperationWrapper operation, byte* pResult, IDictionary<TKey, TValue> collection) where TKey : unmanaged where TValue : class, ISerializableGameData, new()
	{
		int pData = *(int*)operation.PData;
		byte* ptr = operation.PData + 4;
		byte* ptr2 = pResult;
		for (int i = 0; i < pData; i++)
		{
			TKey key = *(TKey*)ptr;
			ptr += sizeof(TKey);
			if (collection.TryGetValue(key, out var value))
			{
				ptr2 += ((ISerializableGameData)value).Deserialize(ptr2);
				continue;
			}
			value = new TValue();
			ptr2 += ((ISerializableGameData)value).Deserialize(ptr2);
			collection.Add(key, value);
		}
	}

	private unsafe static void DynamicObjectCollection_GetFixedField<TKey, TValue>(OperationWrapper operation, byte* pResult, IDictionary<TKey, TValue> collection) where TKey : unmanaged where TValue : class, ISerializableGameData, new()
	{
		throw new NotImplementedException();
	}

	private unsafe static void DynamicObjectCollection_GetDynamicField<TKey, TValue>(OperationWrapper operation, byte* pResult, IDictionary<TKey, TValue> collection) where TKey : unmanaged where TValue : class, ISerializableGameData, new()
	{
		throw new NotImplementedException();
	}

	private unsafe static void DynamicObjectCollection_GetAllIds<TKey, TValue>(OperationWrapper operation, byte* pResult, IDictionary<TKey, TValue> collection) where TKey : unmanaged where TValue : class, ISerializableGameData, new()
	{
		collection.Clear();
		int num = *(int*)pResult;
		byte* ptr = pResult + 4;
		for (int i = 0; i < num; i++)
		{
			TKey key = *(TKey*)ptr;
			ptr += sizeof(TKey);
			collection.Add(key, null);
		}
	}

	private unsafe static void DynamicObjectCollection_GetAllObjects<TKey, TValue>(OperationWrapper operation, byte* pResult, IDictionary<TKey, TValue> collection) where TKey : unmanaged where TValue : class, ISerializableGameData, new()
	{
		collection.Clear();
		int num = *(int*)pResult;
		byte* ptr = pResult + 4;
		for (int i = 0; i < num; i++)
		{
			TKey key = *(TKey*)ptr;
			ptr += sizeof(TKey);
			TValue val = new TValue();
			ptr += ((ISerializableGameData)val).Deserialize(ptr);
			collection.Add(key, val);
		}
	}
}
