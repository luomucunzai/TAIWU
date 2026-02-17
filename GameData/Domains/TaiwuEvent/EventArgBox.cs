using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using CompDevLib.Interpreter;
using CompDevLib.Interpreter.Parse;
using GameData.Domains.Adventure;
using GameData.Domains.Character;
using GameData.Domains.Character.AvatarSystem;
using GameData.Domains.Character.Display;
using GameData.Domains.Item;
using GameData.Domains.Map;
using GameData.Domains.TaiwuEvent.DisplayEvent;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.TaiwuEvent
{
	// Token: 0x02000072 RID: 114
	[SerializableGameData(NotForDisplayModule = true, NotRestrictCollectionSerializedSize = true)]
	public class EventArgBox : ISerializableGameData, IVariantCollection<string>, IValueSelector
	{
		// Token: 0x170000AE RID: 174
		// (get) Token: 0x06001681 RID: 5761 RVA: 0x001511F9 File Offset: 0x0014F3F9
		public static int TaiwuCharacterId
		{
			get
			{
				return DomainManager.Taiwu.GetTaiwuCharId();
			}
		}

		// Token: 0x170000AF RID: 175
		// (get) Token: 0x06001682 RID: 5762 RVA: 0x00151205 File Offset: 0x0014F405
		public static short TaiwuAreaId
		{
			get
			{
				return DomainManager.Taiwu.GetTaiwu().GetLocation().AreaId;
			}
		}

		// Token: 0x170000B0 RID: 176
		// (get) Token: 0x06001683 RID: 5763 RVA: 0x0015121B File Offset: 0x0014F41B
		public static short TaiwuBlockId
		{
			get
			{
				return DomainManager.Taiwu.GetTaiwu().GetLocation().BlockId;
			}
		}

		// Token: 0x170000B1 RID: 177
		// (get) Token: 0x06001684 RID: 5764 RVA: 0x00151231 File Offset: 0x0014F431
		public static short TaiwuVillageAreaId
		{
			get
			{
				return DomainManager.Taiwu.GetTaiwuVillageLocation().AreaId;
			}
		}

		// Token: 0x170000B2 RID: 178
		// (get) Token: 0x06001685 RID: 5765 RVA: 0x00151242 File Offset: 0x0014F442
		public static short TaiwuVillageBlockId
		{
			get
			{
				return DomainManager.Taiwu.GetTaiwuVillageLocation().BlockId;
			}
		}

		// Token: 0x06001686 RID: 5766 RVA: 0x00151254 File Offset: 0x0014F454
		public static void ShowStatus()
		{
			int instancesCount = EventArgBox._instancesCount;
			int objectCollectionsCount = EventArgBox._objectCollectionsCount;
			bool flag = instancesCount > 0 || objectCollectionsCount > 0;
			if (flag)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(59, 2);
				defaultInterpolatedStringHandler.AppendLiteral("EventArgBox newly created: ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(instancesCount);
				defaultInterpolatedStringHandler.AppendLiteral(" instances, ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(objectCollectionsCount);
				defaultInterpolatedStringHandler.AppendLiteral(" object collections.");
				AdaptableLog.Info(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			EventArgBox._instancesCount = 0;
			EventArgBox._objectCollectionsCount = 0;
		}

		// Token: 0x06001687 RID: 5767 RVA: 0x001512DC File Offset: 0x0014F4DC
		public void Clear()
		{
			Dictionary<string, int> intBox = this._intBox;
			if (intBox != null)
			{
				intBox.Clear();
			}
			Dictionary<string, string> stringBox = this._stringBox;
			if (stringBox != null)
			{
				stringBox.Clear();
			}
			Dictionary<string, float> floatBox = this._floatBox;
			if (floatBox != null)
			{
				floatBox.Clear();
			}
			Dictionary<string, bool> boolBox = this._boolBox;
			if (boolBox != null)
			{
				boolBox.Clear();
			}
			Dictionary<string, ISerializableGameData> serializableObjectBox = this._serializableObjectBox;
			if (serializableObjectBox != null)
			{
				serializableObjectBox.Clear();
			}
			this._intBox = null;
			this._stringBox = null;
			this._floatBox = null;
			this._boolBox = null;
			this._serializableObjectBox = null;
		}

		// Token: 0x06001688 RID: 5768 RVA: 0x00151368 File Offset: 0x0014F568
		public void CloneTo(EventArgBox argBox)
		{
			bool flag = this._intBox != null;
			if (flag)
			{
				foreach (KeyValuePair<string, int> pair in this._intBox)
				{
					Dictionary<string, int> dictionary;
					if ((dictionary = argBox._intBox) == null)
					{
						dictionary = (argBox._intBox = new Dictionary<string, int>());
					}
					dictionary.Add(pair.Key, pair.Value);
				}
			}
			bool flag2 = this._stringBox != null;
			if (flag2)
			{
				foreach (KeyValuePair<string, string> pair2 in this._stringBox)
				{
					Dictionary<string, string> dictionary2;
					if ((dictionary2 = argBox._stringBox) == null)
					{
						dictionary2 = (argBox._stringBox = new Dictionary<string, string>());
					}
					dictionary2.Add(pair2.Key, pair2.Value);
				}
			}
			bool flag3 = this._floatBox != null;
			if (flag3)
			{
				foreach (KeyValuePair<string, float> pair3 in this._floatBox)
				{
					Dictionary<string, float> dictionary3;
					if ((dictionary3 = argBox._floatBox) == null)
					{
						dictionary3 = (argBox._floatBox = new Dictionary<string, float>());
					}
					dictionary3.Add(pair3.Key, pair3.Value);
				}
			}
			bool flag4 = this._boolBox != null;
			if (flag4)
			{
				foreach (KeyValuePair<string, bool> pair4 in this._boolBox)
				{
					Dictionary<string, bool> dictionary4;
					if ((dictionary4 = argBox._boolBox) == null)
					{
						dictionary4 = (argBox._boolBox = new Dictionary<string, bool>());
					}
					dictionary4.Add(pair4.Key, pair4.Value);
				}
			}
			bool flag5 = this._serializableObjectBox != null;
			if (flag5)
			{
				foreach (KeyValuePair<string, ISerializableGameData> pair5 in this._serializableObjectBox)
				{
					Dictionary<string, ISerializableGameData> dictionary5;
					if ((dictionary5 = argBox._serializableObjectBox) == null)
					{
						dictionary5 = (argBox._serializableObjectBox = new Dictionary<string, ISerializableGameData>());
					}
					dictionary5.Add(pair5.Key, this.GetCopyOfSerializableObject(pair5.Value));
				}
			}
		}

		// Token: 0x06001689 RID: 5769 RVA: 0x001515F4 File Offset: 0x0014F7F4
		public void Set(string key, sbyte arg)
		{
			bool flag = string.IsNullOrEmpty(key);
			if (!flag)
			{
				bool flag2 = this._intBox == null;
				if (flag2)
				{
					this._intBox = new Dictionary<string, int>();
				}
				bool flag3 = this._intBox.ContainsKey(key);
				if (flag3)
				{
					this._intBox[key] = (int)arg;
				}
				else
				{
					this._intBox.Add(key, (int)arg);
				}
			}
		}

		// Token: 0x0600168A RID: 5770 RVA: 0x00151654 File Offset: 0x0014F854
		public void Set(string key, byte arg)
		{
			bool flag = string.IsNullOrEmpty(key);
			if (!flag)
			{
				bool flag2 = this._intBox == null;
				if (flag2)
				{
					this._intBox = new Dictionary<string, int>();
				}
				bool flag3 = this._intBox.ContainsKey(key);
				if (flag3)
				{
					this._intBox[key] = (int)arg;
				}
				else
				{
					this._intBox.Add(key, (int)arg);
				}
			}
		}

		// Token: 0x0600168B RID: 5771 RVA: 0x001516B4 File Offset: 0x0014F8B4
		public void Set(string key, ushort arg)
		{
			bool flag = string.IsNullOrEmpty(key);
			if (!flag)
			{
				bool flag2 = this._intBox == null;
				if (flag2)
				{
					this._intBox = new Dictionary<string, int>();
				}
				bool flag3 = this._intBox.ContainsKey(key);
				if (flag3)
				{
					this._intBox[key] = (int)arg;
				}
				else
				{
					this._intBox.Add(key, (int)arg);
				}
			}
		}

		// Token: 0x0600168C RID: 5772 RVA: 0x00151714 File Offset: 0x0014F914
		public void Set(string key, short arg)
		{
			bool flag = string.IsNullOrEmpty(key);
			if (!flag)
			{
				bool flag2 = this._intBox == null;
				if (flag2)
				{
					this._intBox = new Dictionary<string, int>();
				}
				bool flag3 = this._intBox.ContainsKey(key);
				if (flag3)
				{
					this._intBox[key] = (int)arg;
				}
				else
				{
					this._intBox.Add(key, (int)arg);
				}
			}
		}

		// Token: 0x0600168D RID: 5773 RVA: 0x00151774 File Offset: 0x0014F974
		public void Set(string key, int arg)
		{
			bool flag = string.IsNullOrEmpty(key);
			if (!flag)
			{
				bool flag2 = this._intBox == null;
				if (flag2)
				{
					this._intBox = new Dictionary<string, int>();
				}
				bool flag3 = this._intBox.ContainsKey(key);
				if (flag3)
				{
					this._intBox[key] = arg;
				}
				else
				{
					this._intBox.Add(key, arg);
				}
			}
		}

		// Token: 0x0600168E RID: 5774 RVA: 0x001517D4 File Offset: 0x0014F9D4
		public void Set(string key, float arg)
		{
			bool flag = string.IsNullOrEmpty(key);
			if (!flag)
			{
				bool flag2 = this._floatBox == null;
				if (flag2)
				{
					this._floatBox = new Dictionary<string, float>();
				}
				bool flag3 = this._floatBox.ContainsKey(key);
				if (flag3)
				{
					this._floatBox[key] = arg;
				}
				else
				{
					this._floatBox.Add(key, arg);
				}
			}
		}

		// Token: 0x0600168F RID: 5775 RVA: 0x00151834 File Offset: 0x0014FA34
		public void Set(string key, string arg)
		{
			bool flag = string.IsNullOrEmpty(key);
			if (!flag)
			{
				bool flag2 = this._stringBox == null;
				if (flag2)
				{
					this._stringBox = new Dictionary<string, string>();
				}
				bool flag3 = this._stringBox.ContainsKey(key);
				if (flag3)
				{
					this._stringBox[key] = arg;
				}
				else
				{
					this._stringBox.Add(key, arg);
				}
			}
		}

		// Token: 0x06001690 RID: 5776 RVA: 0x00151894 File Offset: 0x0014FA94
		public void Set(string key, bool arg)
		{
			bool flag = string.IsNullOrEmpty(key);
			if (!flag)
			{
				bool flag2 = this._boolBox == null;
				if (flag2)
				{
					this._boolBox = new Dictionary<string, bool>();
				}
				bool flag3 = this._boolBox.ContainsKey(key);
				if (flag3)
				{
					this._boolBox[key] = arg;
				}
				else
				{
					this._boolBox.Add(key, arg);
				}
			}
		}

		// Token: 0x06001691 RID: 5777 RVA: 0x001518F4 File Offset: 0x0014FAF4
		public void Set(string key, ISerializableGameData arg)
		{
			bool flag = string.IsNullOrEmpty(key);
			if (!flag)
			{
				bool flag2 = this._serializableObjectBox == null;
				if (flag2)
				{
					this._serializableObjectBox = new Dictionary<string, ISerializableGameData>();
					EventArgBox._objectCollectionsCount++;
				}
				bool flag3 = arg == null;
				if (flag3)
				{
					this._serializableObjectBox.Remove(key);
				}
				else
				{
					bool flag4 = this._serializableObjectBox.ContainsKey(key);
					if (flag4)
					{
						this._serializableObjectBox[key] = arg;
					}
					else
					{
						this._serializableObjectBox.Add(key, arg);
					}
				}
			}
		}

		// Token: 0x06001692 RID: 5778 RVA: 0x00151980 File Offset: 0x0014FB80
		public void Remove<T>(string key)
		{
			bool flag = typeof(T) == typeof(int) || typeof(T) == typeof(short) || typeof(T) == typeof(ushort) || typeof(T) == typeof(byte) || typeof(T) == typeof(sbyte);
			if (flag)
			{
				Dictionary<string, int> intBox = this._intBox;
				if (intBox != null)
				{
					intBox.Remove(key);
				}
			}
			else
			{
				bool flag2 = typeof(T) == typeof(float);
				if (flag2)
				{
					Dictionary<string, float> floatBox = this._floatBox;
					if (floatBox != null)
					{
						floatBox.Remove(key);
					}
				}
				else
				{
					bool flag3 = typeof(T) == typeof(bool);
					if (flag3)
					{
						Dictionary<string, bool> boolBox = this._boolBox;
						if (boolBox != null)
						{
							boolBox.Remove(key);
						}
					}
					else
					{
						bool flag4 = typeof(T) == typeof(string);
						if (flag4)
						{
							Dictionary<string, string> stringBox = this._stringBox;
							if (stringBox != null)
							{
								stringBox.Remove(key);
							}
						}
						else
						{
							Dictionary<string, ISerializableGameData> serializableObjectBox = this._serializableObjectBox;
							if (serializableObjectBox != null)
							{
								serializableObjectBox.Remove(key);
							}
						}
					}
				}
			}
			bool flag5 = this == DomainManager.TaiwuEvent.GetGlobalEventArgumentBox();
			if (flag5)
			{
				DomainManager.TaiwuEvent.SaveGlobalEventArgumentBox();
			}
		}

		// Token: 0x06001693 RID: 5779 RVA: 0x00151B04 File Offset: 0x0014FD04
		public void GenericSet<T>(string key, T value)
		{
			T t = value;
			T t2 = t;
			if (t2 is int)
			{
				int intValue = t2 as int;
				this.Set(key, intValue);
			}
			else if (t2 is short)
			{
				short shortValue = t2 as short;
				this.Set(key, shortValue);
			}
			else if (t2 is ushort)
			{
				ushort ushortValue = t2 as ushort;
				this.Set(key, ushortValue);
			}
			else if (t2 is byte)
			{
				byte byteValue = t2 as byte;
				this.Set(key, byteValue);
			}
			else if (t2 is sbyte)
			{
				sbyte sbyteValue = t2 as sbyte;
				this.Set(key, sbyteValue);
			}
			else if (t2 is float)
			{
				float floatValue = t2 as float;
				this.Set(key, floatValue);
			}
			else
			{
				string stringValue = t2 as string;
				if (stringValue == null)
				{
					if (t2 is bool)
					{
						bool boolValue = t2 as bool;
						this.Set(key, boolValue);
					}
					else
					{
						ISerializableGameData iSerializableValue = t2 as ISerializableGameData;
						if (iSerializableValue == null)
						{
							DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(46, 2);
							defaultInterpolatedStringHandler.AppendLiteral("Value ");
							defaultInterpolatedStringHandler.AppendFormatted<T>(value);
							defaultInterpolatedStringHandler.AppendLiteral(" of type ");
							defaultInterpolatedStringHandler.AppendFormatted<Type>(typeof(T));
							defaultInterpolatedStringHandler.AppendLiteral(" cannot be saved in EventArgBox");
							throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
						}
						bool flag = !EventArgBox.SerializeObjectMap.ContainsKey(value.GetType());
						if (flag)
						{
							DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(89, 1);
							defaultInterpolatedStringHandler.AppendFormatted<Type>(value.GetType());
							defaultInterpolatedStringHandler.AppendLiteral(" is a type can only set to EventArgBox in runtime,but will not be saved when saving game!");
							AdaptableLog.Warning(defaultInterpolatedStringHandler.ToStringAndClear(), false);
						}
						this.Set(key, iSerializableValue);
					}
				}
				else
				{
					this.Set(key, stringValue);
				}
			}
		}

		// Token: 0x06001694 RID: 5780 RVA: 0x00151D94 File Offset: 0x0014FF94
		public void SetActorKey(string key)
		{
			this.Set("ActorKey", key);
		}

		// Token: 0x06001695 RID: 5781 RVA: 0x00151DA4 File Offset: 0x0014FFA4
		public void SetLeftActorKey(string key)
		{
			this.Set("ConchShip_PresetKey_LeftActorKey", key);
		}

		// Token: 0x06001696 RID: 5782 RVA: 0x00151DB4 File Offset: 0x0014FFB4
		public bool Contains<T>(string key)
		{
			bool flag = typeof(T) == typeof(int) || typeof(T) == typeof(short) || typeof(T) == typeof(ushort) || typeof(T) == typeof(byte) || typeof(T) == typeof(sbyte);
			bool result;
			if (flag)
			{
				result = (this._intBox != null && this._intBox.ContainsKey(key));
			}
			else
			{
				bool flag2 = typeof(T) == typeof(float);
				if (flag2)
				{
					result = (this._floatBox != null && this._floatBox.ContainsKey(key));
				}
				else
				{
					bool flag3 = typeof(T) == typeof(bool);
					if (flag3)
					{
						result = (this._boolBox != null && this._boolBox.ContainsKey(key));
					}
					else
					{
						bool flag4 = typeof(T) == typeof(string);
						if (flag4)
						{
							result = (this._stringBox != null && this._stringBox.ContainsKey(key));
						}
						else
						{
							result = (this._serializableObjectBox != null && this._serializableObjectBox.ContainsKey(key));
						}
					}
				}
			}
			return result;
		}

		// Token: 0x06001697 RID: 5783 RVA: 0x00151F34 File Offset: 0x00150134
		public bool Get(string key, ref sbyte arg)
		{
			bool flag = this._intBox == null;
			bool result2;
			if (flag)
			{
				result2 = false;
			}
			else
			{
				int result;
				bool flag2 = this._intBox.TryGetValue(key, out result);
				if (flag2)
				{
					arg = (sbyte)result;
					result2 = true;
				}
				else
				{
					result2 = false;
				}
			}
			return result2;
		}

		// Token: 0x06001698 RID: 5784 RVA: 0x00151F74 File Offset: 0x00150174
		public bool Get(string key, ref byte arg)
		{
			bool flag = this._intBox == null;
			bool result2;
			if (flag)
			{
				result2 = false;
			}
			else
			{
				int result;
				bool flag2 = this._intBox.TryGetValue(key, out result);
				if (flag2)
				{
					arg = (byte)result;
					result2 = true;
				}
				else
				{
					result2 = false;
				}
			}
			return result2;
		}

		// Token: 0x06001699 RID: 5785 RVA: 0x00151FB4 File Offset: 0x001501B4
		public bool Get(string key, ref ushort arg)
		{
			bool flag = this._intBox == null;
			bool result2;
			if (flag)
			{
				result2 = false;
			}
			else
			{
				int result;
				bool flag2 = this._intBox.TryGetValue(key, out result);
				if (flag2)
				{
					arg = (ushort)result;
					result2 = true;
				}
				else
				{
					result2 = false;
				}
			}
			return result2;
		}

		// Token: 0x0600169A RID: 5786 RVA: 0x00151FF4 File Offset: 0x001501F4
		public bool Get(string key, ref short arg)
		{
			bool flag = this._intBox == null;
			bool result2;
			if (flag)
			{
				result2 = false;
			}
			else
			{
				int result;
				bool flag2 = this._intBox.TryGetValue(key, out result);
				if (flag2)
				{
					arg = (short)result;
					result2 = true;
				}
				else
				{
					result2 = false;
				}
			}
			return result2;
		}

		// Token: 0x0600169B RID: 5787 RVA: 0x00152034 File Offset: 0x00150234
		public bool Get(string key, ref int arg)
		{
			bool flag = this._intBox == null;
			return !flag && this._intBox.TryGetValue(key, out arg);
		}

		// Token: 0x0600169C RID: 5788 RVA: 0x00152068 File Offset: 0x00150268
		public bool Get(string key, ref float arg)
		{
			bool flag = this._floatBox == null;
			return !flag && this._floatBox.TryGetValue(key, out arg);
		}

		// Token: 0x0600169D RID: 5789 RVA: 0x0015209C File Offset: 0x0015029C
		public bool Get(string key, ref string arg)
		{
			bool flag = this._stringBox == null;
			return !flag && this._stringBox.TryGetValue(key, out arg);
		}

		// Token: 0x0600169E RID: 5790 RVA: 0x001520D0 File Offset: 0x001502D0
		public bool Get(string key, ref bool arg)
		{
			bool flag = this._boolBox == null;
			return !flag && this._boolBox.TryGetValue(key, out arg);
		}

		// Token: 0x0600169F RID: 5791 RVA: 0x00152104 File Offset: 0x00150304
		public bool Get<T>(string key, out T arg) where T : ISerializableGameData
		{
			arg = default(T);
			bool flag = this._serializableObjectBox == null;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				ISerializableGameData data;
				bool ret = this._serializableObjectBox.TryGetValue(key, out data);
				bool flag2 = ret && data != null;
				if (flag2)
				{
					arg = (T)((object)data);
				}
				result = (arg != null && ret);
			}
			return result;
		}

		// Token: 0x060016A0 RID: 5792 RVA: 0x0015216C File Offset: 0x0015036C
		public sbyte GetSbyte(string key)
		{
			bool flag = this._intBox == null || !this._intBox.ContainsKey(key);
			sbyte result2;
			if (flag)
			{
				result2 = 0;
			}
			else
			{
				int result;
				this._intBox.TryGetValue(key, out result);
				result2 = (sbyte)result;
			}
			return result2;
		}

		// Token: 0x060016A1 RID: 5793 RVA: 0x001521B4 File Offset: 0x001503B4
		public byte GetByte(string key)
		{
			bool flag = this._intBox == null || !this._intBox.ContainsKey(key);
			byte result2;
			if (flag)
			{
				result2 = 0;
			}
			else
			{
				int result;
				this._intBox.TryGetValue(key, out result);
				result2 = (byte)result;
			}
			return result2;
		}

		// Token: 0x060016A2 RID: 5794 RVA: 0x001521FC File Offset: 0x001503FC
		public short GetShort(string key)
		{
			bool flag = this._intBox == null || !this._intBox.ContainsKey(key);
			short result2;
			if (flag)
			{
				result2 = 0;
			}
			else
			{
				int result;
				this._intBox.TryGetValue(key, out result);
				result2 = (short)result;
			}
			return result2;
		}

		// Token: 0x060016A3 RID: 5795 RVA: 0x00152244 File Offset: 0x00150444
		public ushort GetUshort(string key)
		{
			bool flag = this._intBox == null || !this._intBox.ContainsKey(key);
			ushort result2;
			if (flag)
			{
				result2 = 0;
			}
			else
			{
				int result;
				this._intBox.TryGetValue(key, out result);
				result2 = (ushort)result;
			}
			return result2;
		}

		// Token: 0x060016A4 RID: 5796 RVA: 0x0015228C File Offset: 0x0015048C
		public int GetInt(string key)
		{
			bool flag = this._intBox == null || !this._intBox.ContainsKey(key);
			int result2;
			if (flag)
			{
				result2 = 0;
			}
			else
			{
				int result;
				this._intBox.TryGetValue(key, out result);
				result2 = result;
			}
			return result2;
		}

		// Token: 0x060016A5 RID: 5797 RVA: 0x001522D4 File Offset: 0x001504D4
		public float GetFloat(string key)
		{
			bool flag = this._floatBox == null || !this._floatBox.ContainsKey(key);
			float result2;
			if (flag)
			{
				result2 = 0f;
			}
			else
			{
				float result;
				this._floatBox.TryGetValue(key, out result);
				result2 = result;
			}
			return result2;
		}

		// Token: 0x060016A6 RID: 5798 RVA: 0x00152320 File Offset: 0x00150520
		public string GetString(string key)
		{
			bool flag = this._stringBox == null || !this._stringBox.ContainsKey(key);
			string result2;
			if (flag)
			{
				result2 = null;
			}
			else
			{
				string result;
				this._stringBox.TryGetValue(key, out result);
				result2 = result;
			}
			return result2;
		}

		// Token: 0x060016A7 RID: 5799 RVA: 0x00152368 File Offset: 0x00150568
		public bool GetBool(string key)
		{
			bool flag = this._boolBox == null || !this._boolBox.ContainsKey(key);
			bool result2;
			if (flag)
			{
				result2 = false;
			}
			else
			{
				bool result;
				this._boolBox.TryGetValue(key, out result);
				result2 = result;
			}
			return result2;
		}

		// Token: 0x060016A8 RID: 5800 RVA: 0x001523B0 File Offset: 0x001505B0
		public T Get<T>(string key) where T : ISerializableGameData
		{
			bool flag = this._serializableObjectBox == null || !this._serializableObjectBox.ContainsKey(key);
			T result2;
			if (flag)
			{
				result2 = default(T);
			}
			else
			{
				ISerializableGameData result;
				this._serializableObjectBox.TryGetValue(key, out result);
				result2 = (T)((object)result);
			}
			return result2;
		}

		// Token: 0x060016A9 RID: 5801 RVA: 0x00152404 File Offset: 0x00150604
		public Character GetCharacter(string key)
		{
			bool flag = key == "RoleTaiwu";
			Character result;
			if (flag)
			{
				result = DomainManager.Taiwu.GetTaiwu();
			}
			else
			{
				Character character = null;
				int characterId;
				bool flag2 = this._intBox != null && this._intBox.TryGetValue(key, out characterId);
				if (flag2)
				{
					bool flag3 = !DomainManager.Character.TryGetElement_Objects(characterId, out character);
					if (flag3)
					{
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(53, 2);
						defaultInterpolatedStringHandler.AppendLiteral("failed to get character ");
						defaultInterpolatedStringHandler.AppendFormatted(key);
						defaultInterpolatedStringHandler.AppendLiteral(" from ArgBox!curId in box is ");
						defaultInterpolatedStringHandler.AppendFormatted<int>(characterId);
						AdaptableLog.Warning(defaultInterpolatedStringHandler.ToStringAndClear(), false);
					}
				}
				else
				{
					AdaptableLog.Warning("Failed to get character " + key + " from ArgBox.", false);
				}
				result = character;
			}
			return result;
		}

		// Token: 0x060016AA RID: 5802 RVA: 0x001524CC File Offset: 0x001506CC
		public DeadCharacter GetDeadCharacter(string key)
		{
			bool flag = key == "RoleTaiwu";
			DeadCharacter result;
			if (flag)
			{
				result = null;
			}
			else
			{
				int characterId;
				bool flag2 = this._intBox == null || !this._intBox.TryGetValue(key, out characterId);
				if (flag2)
				{
					result = null;
				}
				else
				{
					result = DomainManager.Character.TryGetDeadCharacter(characterId);
				}
			}
			return result;
		}

		// Token: 0x060016AB RID: 5803 RVA: 0x00152520 File Offset: 0x00150720
		public Character GetAdventureMajorCharacter(int group, int index)
		{
			Dictionary<string, int> intBox = this._intBox;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(16, 2);
			defaultInterpolatedStringHandler.AppendLiteral("MajorCharacter_");
			defaultInterpolatedStringHandler.AppendFormatted<int>(group);
			defaultInterpolatedStringHandler.AppendLiteral("_");
			defaultInterpolatedStringHandler.AppendFormatted<int>(index);
			int characterId;
			bool flag = intBox.TryGetValue(defaultInterpolatedStringHandler.ToStringAndClear(), out characterId);
			Character result;
			if (flag)
			{
				result = DomainManager.Character.GetElement_Objects(characterId);
			}
			else
			{
				result = null;
			}
			return result;
		}

		// Token: 0x060016AC RID: 5804 RVA: 0x00152590 File Offset: 0x00150790
		public int GetAdventureMajorCharacterCount(int group)
		{
			Dictionary<string, int> intBox = this._intBox;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(21, 1);
			defaultInterpolatedStringHandler.AppendLiteral("MajorCharacter_");
			defaultInterpolatedStringHandler.AppendFormatted<int>(group);
			defaultInterpolatedStringHandler.AppendLiteral("_Count");
			int count;
			bool flag = intBox.TryGetValue(defaultInterpolatedStringHandler.ToStringAndClear(), out count);
			int result;
			if (flag)
			{
				result = count;
			}
			else
			{
				result = -1;
			}
			return result;
		}

		// Token: 0x060016AD RID: 5805 RVA: 0x001525EC File Offset: 0x001507EC
		public Character GetAdventureParticipateCharacter(int group, int index)
		{
			Dictionary<string, int> intBox = this._intBox;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(22, 2);
			defaultInterpolatedStringHandler.AppendLiteral("ParticipateCharacter_");
			defaultInterpolatedStringHandler.AppendFormatted<int>(group);
			defaultInterpolatedStringHandler.AppendLiteral("_");
			defaultInterpolatedStringHandler.AppendFormatted<int>(index);
			int characterId;
			bool flag = intBox.TryGetValue(defaultInterpolatedStringHandler.ToStringAndClear(), out characterId);
			Character result;
			if (flag)
			{
				result = DomainManager.Character.GetElement_Objects(characterId);
			}
			else
			{
				result = null;
			}
			return result;
		}

		// Token: 0x060016AE RID: 5806 RVA: 0x0015265C File Offset: 0x0015085C
		public int GetAdventureParticipateCharacterCount(int group)
		{
			Dictionary<string, int> intBox = this._intBox;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(27, 1);
			defaultInterpolatedStringHandler.AppendLiteral("ParticipateCharacter_");
			defaultInterpolatedStringHandler.AppendFormatted<int>(group);
			defaultInterpolatedStringHandler.AppendLiteral("_Count");
			int count;
			bool flag = intBox.TryGetValue(defaultInterpolatedStringHandler.ToStringAndClear(), out count);
			int result;
			if (flag)
			{
				result = count;
			}
			else
			{
				result = -1;
			}
			return result;
		}

		// Token: 0x060016AF RID: 5807 RVA: 0x001526B8 File Offset: 0x001508B8
		public ItemBase GetItem(string key)
		{
			ISerializableGameData itemKey;
			bool flag = this._serializableObjectBox.TryGetValue(key, out itemKey);
			ItemBase result;
			if (flag)
			{
				result = DomainManager.Item.GetBaseItem((ItemKey)itemKey);
			}
			else
			{
				result = null;
			}
			return result;
		}

		// Token: 0x060016B0 RID: 5808 RVA: 0x001526F1 File Offset: 0x001508F1
		public EventArgBox()
		{
			EventArgBox._instancesCount++;
		}

		// Token: 0x060016B1 RID: 5809 RVA: 0x00152708 File Offset: 0x00150908
		public EventArgBox(EventArgBox other)
		{
			EventArgBox._instancesCount++;
			bool flag = other._intBox != null;
			if (flag)
			{
				this._intBox = new Dictionary<string, int>();
				foreach (KeyValuePair<string, int> pair in other._intBox)
				{
					this._intBox.Add(pair.Key, pair.Value);
				}
			}
			bool flag2 = other._stringBox != null;
			if (flag2)
			{
				this._stringBox = new Dictionary<string, string>();
				foreach (KeyValuePair<string, string> pair2 in other._stringBox)
				{
					this._stringBox.Add(pair2.Key, pair2.Value);
				}
			}
			bool flag3 = other._floatBox != null;
			if (flag3)
			{
				this._floatBox = new Dictionary<string, float>();
				foreach (KeyValuePair<string, float> pair3 in other._floatBox)
				{
					this._floatBox.Add(pair3.Key, pair3.Value);
				}
			}
			bool flag4 = other._boolBox != null;
			if (flag4)
			{
				this._boolBox = new Dictionary<string, bool>();
				foreach (KeyValuePair<string, bool> pair4 in other._boolBox)
				{
					this._boolBox.Add(pair4.Key, pair4.Value);
				}
			}
			bool flag5 = other._serializableObjectBox != null;
			if (flag5)
			{
				this._serializableObjectBox = new Dictionary<string, ISerializableGameData>();
				EventArgBox._objectCollectionsCount++;
				foreach (KeyValuePair<string, ISerializableGameData> pair5 in other._serializableObjectBox)
				{
					this._serializableObjectBox.Add(pair5.Key, pair5.Value);
				}
			}
		}

		// Token: 0x060016B2 RID: 5810 RVA: 0x00152988 File Offset: 0x00150B88
		public bool IsSerializedSizeFixed()
		{
			return false;
		}

		// Token: 0x060016B3 RID: 5811 RVA: 0x0015299C File Offset: 0x00150B9C
		public int GetSerializedSize()
		{
			int totalSize = 10;
			bool flag = this._intBox != null;
			if (flag)
			{
				foreach (KeyValuePair<string, int> pair in this._intBox)
				{
					totalSize += 2 + 2 * pair.Key.Length + 4;
				}
			}
			bool flag2 = this._stringBox != null;
			if (flag2)
			{
				foreach (KeyValuePair<string, string> pair2 in this._stringBox)
				{
					totalSize += 2 + 2 * pair2.Key.Length + 2 + 2 * pair2.Value.Length;
				}
			}
			bool flag3 = this._floatBox != null;
			if (flag3)
			{
				foreach (KeyValuePair<string, float> pair3 in this._floatBox)
				{
					totalSize += 2 + 2 * pair3.Key.Length + 4;
				}
			}
			bool flag4 = this._boolBox != null;
			if (flag4)
			{
				foreach (KeyValuePair<string, bool> pair4 in this._boolBox)
				{
					totalSize += 2 + 2 * pair4.Key.Length + 1;
				}
			}
			bool flag5 = this._serializableObjectBox != null;
			if (flag5)
			{
				foreach (KeyValuePair<string, ISerializableGameData> pair5 in this._serializableObjectBox)
				{
					Type type = pair5.Value.GetType();
					bool flag6 = !EventArgBox.SerializeObjectMap.ContainsKey(type);
					if (!flag6)
					{
						totalSize += 3 + 2 * pair5.Key.Length + pair5.Value.GetSerializedSize();
					}
				}
			}
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x060016B4 RID: 5812 RVA: 0x00152C08 File Offset: 0x00150E08
		private unsafe int WriteString(byte* pData, string target)
		{
			bool flag = !string.IsNullOrEmpty(target);
			byte* pCurrData;
			if (flag)
			{
				int elementsCount = target.Length;
				Tester.Assert(elementsCount <= 65535, "");
				*(short*)pData = (short)((ushort)elementsCount);
				pCurrData = pData + 2;
				char* ptr;
				if (target == null)
				{
					ptr = null;
				}
				else
				{
					fixed (char* ptr2 = target.GetPinnableReference())
					{
						ptr = ptr2;
					}
				}
				char* pChar = ptr;
				for (int i = 0; i < elementsCount; i++)
				{
					*(short*)(pCurrData + (IntPtr)i * 2) = (short)pChar[i];
				}
				char* ptr2 = null;
				pCurrData += 2 * elementsCount;
			}
			else
			{
				*(short*)pData = 0;
				pCurrData = pData + 2;
			}
			return (int)((long)(pCurrData - pData));
		}

		// Token: 0x060016B5 RID: 5813 RVA: 0x00152CAC File Offset: 0x00150EAC
		private unsafe string ReadString(ref byte* pData)
		{
			ushort elementsCount = *pData;
			pData += 2;
			bool flag = elementsCount > 0;
			string result;
			if (flag)
			{
				int fieldSize = (int)(2 * elementsCount);
				string resultString = Encoding.Unicode.GetString(pData, fieldSize);
				pData += (IntPtr)fieldSize;
				result = resultString;
			}
			else
			{
				result = string.Empty;
			}
			return result;
		}

		// Token: 0x060016B6 RID: 5814 RVA: 0x00152CF8 File Offset: 0x00150EF8
		public unsafe int Serialize(byte* pData)
		{
			byte* pCurrData = pData;
			bool flag = this._intBox != null;
			if (flag)
			{
				*(short*)pCurrData = (short)((ushort)this._intBox.Count);
				pCurrData += 2;
				foreach (KeyValuePair<string, int> pair in this._intBox)
				{
					pCurrData += this.WriteString(pCurrData, pair.Key);
					*(int*)pCurrData = pair.Value;
					pCurrData += 4;
				}
			}
			else
			{
				*(short*)pCurrData = 0;
				pCurrData += 2;
			}
			bool flag2 = this._stringBox != null;
			if (flag2)
			{
				*(short*)pCurrData = (short)((ushort)this._stringBox.Count);
				pCurrData += 2;
				foreach (KeyValuePair<string, string> pair2 in this._stringBox)
				{
					pCurrData += this.WriteString(pCurrData, pair2.Key);
					pCurrData += this.WriteString(pCurrData, pair2.Value);
				}
			}
			else
			{
				*(short*)pCurrData = 0;
				pCurrData += 2;
			}
			bool flag3 = this._floatBox != null;
			if (flag3)
			{
				*(short*)pCurrData = (short)((ushort)this._floatBox.Count);
				pCurrData += 2;
				foreach (KeyValuePair<string, float> pair3 in this._floatBox)
				{
					pCurrData += this.WriteString(pCurrData, pair3.Key);
					*(float*)pCurrData = pair3.Value;
					pCurrData += 4;
				}
			}
			else
			{
				*(short*)pCurrData = 0;
				pCurrData += 2;
			}
			bool flag4 = this._boolBox != null;
			if (flag4)
			{
				*(short*)pCurrData = (short)((ushort)this._boolBox.Count);
				pCurrData += 2;
				foreach (KeyValuePair<string, bool> pair4 in this._boolBox)
				{
					pCurrData += this.WriteString(pCurrData, pair4.Key);
					*pCurrData = (pair4.Value ? 1 : 0);
					pCurrData++;
				}
			}
			else
			{
				*(short*)pCurrData = 0;
				pCurrData += 2;
			}
			bool flag5 = this._serializableObjectBox != null;
			if (flag5)
			{
				byte* pSerializableBoxHead = pCurrData;
				ushort savedDataCount = 0;
				pCurrData += 2;
				foreach (KeyValuePair<string, ISerializableGameData> pair5 in this._serializableObjectBox)
				{
					Type type = pair5.Value.GetType();
					bool flag6 = !EventArgBox.SerializeObjectMap.ContainsKey(type);
					if (!flag6)
					{
						pCurrData += this.WriteString(pCurrData, pair5.Key);
						*pCurrData = (byte)EventArgBox.SerializeObjectMap[pair5.Value.GetType()];
						pCurrData++;
						pCurrData += pair5.Value.Serialize(pCurrData);
						savedDataCount += 1;
					}
				}
				*(short*)pSerializableBoxHead = (short)savedDataCount;
			}
			else
			{
				*(short*)pCurrData = 0;
				pCurrData += 2;
			}
			int totalSize = (int)((long)(pCurrData - pData));
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x060016B7 RID: 5815 RVA: 0x00153038 File Offset: 0x00151238
		private void CreateSerializableObject(sbyte code, out ISerializableGameData obj)
		{
			switch (code)
			{
			case 0:
				obj = default(Location);
				break;
			case 1:
				obj = new AdventureMapPoint();
				break;
			case 2:
				obj = default(ItemKey);
				break;
			case 3:
				obj = new AdventureSiteData();
				break;
			case 4:
				obj = default(MapTemplateEnemyInfo);
				break;
			case 5:
				obj = new AvatarRelatedData();
				break;
			case 6:
				obj = new EventActorData();
				break;
			case 7:
				obj = ShortList.Create();
				break;
			case 8:
				obj = new AvatarData();
				break;
			case 9:
				obj = default(IntList);
				break;
			default:
				obj = null;
				break;
			}
		}

		// Token: 0x060016B8 RID: 5816 RVA: 0x00153108 File Offset: 0x00151308
		private ISerializableGameData GetCopyOfSerializableObject(ISerializableGameData obj)
		{
			ISerializableGameData result;
			if (obj is Location)
			{
				Location location = (Location)obj;
				result = location;
			}
			else
			{
				AdventureMapPoint adventureMapPoint = obj as AdventureMapPoint;
				if (adventureMapPoint == null)
				{
					if (obj is ItemKey)
					{
						ItemKey itemKey = (ItemKey)obj;
						result = itemKey;
					}
					else
					{
						AdventureSiteData adventureSiteData = obj as AdventureSiteData;
						if (adventureSiteData == null)
						{
							if (obj is MapTemplateEnemyInfo)
							{
								MapTemplateEnemyInfo mapRandomEnemyInfo = (MapTemplateEnemyInfo)obj;
								result = mapRandomEnemyInfo;
							}
							else
							{
								AvatarRelatedData avatarRelatedData = obj as AvatarRelatedData;
								if (avatarRelatedData == null)
								{
									EventActorData eventActorData = obj as EventActorData;
									if (eventActorData == null)
									{
										AvatarData avatarData = obj as AvatarData;
										if (avatarData == null)
										{
											result = null;
										}
										else
										{
											AvatarData avatar = new AvatarData(avatarData);
											result = avatar;
										}
									}
									else
									{
										EventActorData actorData = new EventActorData(eventActorData);
										result = actorData;
									}
								}
								else
								{
									AvatarRelatedData data = new AvatarRelatedData(avatarRelatedData);
									result = data;
								}
							}
						}
						else
						{
							AdventureSiteData advSiteTarget = new AdventureSiteData(adventureSiteData);
							result = advSiteTarget;
						}
					}
				}
				else
				{
					AdventureMapPoint advMapPointTarget = new AdventureMapPoint();
					advMapPointTarget.Assign(adventureMapPoint);
					result = advMapPointTarget;
				}
			}
			return result;
		}

		// Token: 0x060016B9 RID: 5817 RVA: 0x00153220 File Offset: 0x00151420
		public unsafe int Deserialize(byte* pData)
		{
			byte* pCurrData = pData;
			ushort elementsCount = *(ushort*)pCurrData;
			pCurrData += 2;
			bool flag = elementsCount > 0;
			if (flag)
			{
				bool flag2 = this._intBox == null;
				if (flag2)
				{
					this._intBox = new Dictionary<string, int>();
				}
				for (int i = 0; i < (int)elementsCount; i++)
				{
					string key = this.ReadString(ref pCurrData);
					int value = *(int*)pCurrData;
					pCurrData += 4;
					this._intBox.Add(key, value);
				}
			}
			elementsCount = *(ushort*)pCurrData;
			pCurrData += 2;
			bool flag3 = elementsCount > 0;
			if (flag3)
			{
				bool flag4 = this._stringBox == null;
				if (flag4)
				{
					this._stringBox = new Dictionary<string, string>();
				}
				for (int j = 0; j < (int)elementsCount; j++)
				{
					string key2 = this.ReadString(ref pCurrData);
					string value2 = this.ReadString(ref pCurrData);
					this._stringBox.Add(key2, value2);
				}
			}
			elementsCount = *(ushort*)pCurrData;
			pCurrData += 2;
			bool flag5 = elementsCount > 0;
			if (flag5)
			{
				bool flag6 = this._floatBox == null;
				if (flag6)
				{
					this._floatBox = new Dictionary<string, float>();
				}
				for (int k = 0; k < (int)elementsCount; k++)
				{
					string key3 = this.ReadString(ref pCurrData);
					float value3 = *(float*)pCurrData;
					pCurrData += 4;
					this._floatBox.Add(key3, value3);
				}
			}
			elementsCount = *(ushort*)pCurrData;
			pCurrData += 2;
			bool flag7 = elementsCount > 0;
			if (flag7)
			{
				bool flag8 = this._boolBox == null;
				if (flag8)
				{
					this._boolBox = new Dictionary<string, bool>();
				}
				for (int l = 0; l < (int)elementsCount; l++)
				{
					string key4 = this.ReadString(ref pCurrData);
					bool value4 = *pCurrData != 0;
					pCurrData++;
					this._boolBox.Add(key4, value4);
				}
			}
			elementsCount = *(ushort*)pCurrData;
			pCurrData += 2;
			bool flag9 = elementsCount > 0;
			if (flag9)
			{
				bool flag10 = this._serializableObjectBox == null;
				if (flag10)
				{
					this._serializableObjectBox = new Dictionary<string, ISerializableGameData>();
				}
				for (int m = 0; m < (int)elementsCount; m++)
				{
					string key5 = this.ReadString(ref pCurrData);
					sbyte typeCode = *(sbyte*)pCurrData;
					pCurrData++;
					ISerializableGameData data;
					this.CreateSerializableObject(typeCode, out data);
					bool flag11 = data != null;
					if (flag11)
					{
						pCurrData += data.Deserialize(pCurrData);
						this._serializableObjectBox.Add(key5, data);
					}
				}
			}
			int totalSize = (int)((long)(pCurrData - pData));
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x060016BA RID: 5818 RVA: 0x00153478 File Offset: 0x00151678
		public void SetValueFromStack(EvaluationStack evaluationStack, string identifier, EValueType valueType)
		{
			switch (valueType)
			{
			case EValueType.Int:
			{
				int value = evaluationStack.PopUnmanaged<int>();
				this.Set(identifier, value);
				break;
			}
			case EValueType.Float:
			{
				float value2 = evaluationStack.PopUnmanaged<float>();
				this.Set(identifier, value2);
				break;
			}
			case EValueType.Bool:
			{
				bool value3 = evaluationStack.PopUnmanaged<bool>();
				this.Set(identifier, value3);
				break;
			}
			case EValueType.Str:
			{
				string value4 = evaluationStack.PopObject<string>();
				this.Set(identifier, value4);
				break;
			}
			case EValueType.Obj:
			{
				ISerializableGameData value5 = evaluationStack.PopObject<ISerializableGameData>();
				this.Set(identifier, value5);
				break;
			}
			}
		}

		// Token: 0x060016BB RID: 5819 RVA: 0x00153510 File Offset: 0x00151710
		public ValueInfo SelectValue(Evaluator evaluator, string identifier)
		{
			int intVal = 0;
			bool flag = this.Get(identifier, ref intVal);
			ValueInfo result;
			if (flag)
			{
				result = evaluator.PushEvaluationResult(intVal);
			}
			else
			{
				bool boolVal = false;
				bool flag2 = this.Get(identifier, ref boolVal);
				if (flag2)
				{
					result = evaluator.PushEvaluationResult(boolVal);
				}
				else
				{
					float floatVal = 0f;
					bool flag3 = this.Get(identifier, ref floatVal);
					if (flag3)
					{
						result = evaluator.PushEvaluationResult(floatVal);
					}
					else
					{
						string strVal = null;
						bool flag4 = this.Get(identifier, ref strVal);
						if (flag4)
						{
							result = evaluator.PushEvaluationResult(strVal);
						}
						else
						{
							ISerializableGameData objVal;
							bool flag5 = this.Get<ISerializableGameData>(identifier, out objVal);
							if (flag5)
							{
								result = evaluator.PushEvaluationResult(objVal);
							}
							else
							{
								result = ValueInfo.Void;
							}
						}
					}
				}
			}
			return result;
		}

		// Token: 0x040003A9 RID: 937
		public const string ShuffleOption = "ShuffleOptions";

		// Token: 0x040003AA RID: 938
		public const string MainRoleUseAlternativeName = "MainRoleUseAlternativeName";

		// Token: 0x040003AB RID: 939
		public const string TargetRoleUseAlternativeName = "TargetRoleUseAlternativeName";

		// Token: 0x040003AC RID: 940
		public const string NotShowTargetRole = "NotShowTargetRole";

		// Token: 0x040003AD RID: 941
		public const string NotShowMainRole = "NotShowMainRole";

		// Token: 0x040003AE RID: 942
		public const string ForbidViewCharacter = "ForbidViewCharacter";

		// Token: 0x040003AF RID: 943
		public const string ForbidViewSelf = "ForbidViewSelf";

		// Token: 0x040003B0 RID: 944
		public const string HideFavorability = "HideFavorability";

		// Token: 0x040003B1 RID: 945
		public const string HideLeftFavorability = "ConchShip_PresetKey_HideLeftFavorability";

		// Token: 0x040003B2 RID: 946
		public const string MainRoleShowBlush = "ConchShip_PresetKey_MainRoleShowBlush";

		// Token: 0x040003B3 RID: 947
		public const string TargetRoleShowBlush = "ConchShip_PresetKey_TargetRoleShowBlush";

		// Token: 0x040003B4 RID: 948
		public const string LeftRoleShowInjuryInfo = "ConchShip_PresetKey_LeftRoleShowInjuryInfo";

		// Token: 0x040003B5 RID: 949
		public const string RightRoleShowInjuryInfo = "ConchShip_PresetKey_RightRoleShowInjuryInfo";

		// Token: 0x040003B6 RID: 950
		public const string RightCharacterShadow = "ConchShip_PresetKey_RightCharacterShadow";

		// Token: 0x040003B7 RID: 951
		public const string RightForbiddenConsummateLevel = "ConchShip_PresetKey_RightForbiddenConsummateLevel";

		// Token: 0x040003B8 RID: 952
		public const string LeftForbidShowFavorChangeEffect = "CS_PK_LeftForbidShowFavorChangeEffect";

		// Token: 0x040003B9 RID: 953
		public const string RightForbidShowFavorChangeEffect = "CS_PK_RightForbidShowFavorChangeEffect";

		// Token: 0x040003BA RID: 954
		public const string OrgTemplateId = "OrgTemplateId";

		// Token: 0x040003BB RID: 955
		public const string RoleTaiwu = "RoleTaiwu";

		// Token: 0x040003BC RID: 956
		[Obsolete]
		public const string ShowLeftFavorability = "ConchShip_PresetKey_ShowLeftFavorability";

		// Token: 0x040003BD RID: 957
		public const string SelectItemInfo = "SelectItemInfo";

		// Token: 0x040003BE RID: 958
		public const string SelectReadingBookCount = "SelectReadingBookCount";

		// Token: 0x040003BF RID: 959
		public const string SelectNeigongLoopingCount = "SelectNeigongLoopingCount";

		// Token: 0x040003C0 RID: 960
		public const string SelectFuyuFaithCount = "SelectFuyuFaithCount";

		// Token: 0x040003C1 RID: 961
		public const string SelectFameData = "SelectFameData";

		// Token: 0x040003C2 RID: 962
		public const string SelectCountResult = "SelectCountResult";

		// Token: 0x040003C3 RID: 963
		public const string SelectCharacterData = "SelectCharacterData";

		// Token: 0x040003C4 RID: 964
		public const string InputRequestData = "InputRequestData";

		// Token: 0x040003C5 RID: 965
		public const string ActorKey = "ActorKey";

		// Token: 0x040003C6 RID: 966
		public const string LeftActorKey = "ConchShip_PresetKey_LeftActorKey";

		// Token: 0x040003C7 RID: 967
		public const string TargetCharacterTemplateId = "TargetCharacterTemplateId";

		// Token: 0x040003C8 RID: 968
		public const string SelectAvatarEvent = "SelectAvatarEvent";

		// Token: 0x040003C9 RID: 969
		public const string MainCharacterDisplayAge = "MainCharacterDisplayAge";

		// Token: 0x040003CA RID: 970
		public const string MainCharacterDisplayCloth = "MainCharacterDisplayCloth";

		// Token: 0x040003CB RID: 971
		public const string TargetCharacterDisplayAge = "TargetCharacterDisplayAge";

		// Token: 0x040003CC RID: 972
		public const string TargetCharacterDisplayCloth = "TargetCharacterDisplayCloth";

		// Token: 0x040003CD RID: 973
		public const string TargetCharacterDisplayingClothId = "TargetCharacterDisplayingClothId";

		// Token: 0x040003CE RID: 974
		public const string EventInstanceGuid = "EventInstanceGuid";

		// Token: 0x040003CF RID: 975
		public const string CharIdSeizedInCombat = "CharIdSeizedInCombat";

		// Token: 0x040003D0 RID: 976
		public const string ItemKeySeizeCharacterInCombat = "ItemKeySeizeCharacterInCombat";

		// Token: 0x040003D1 RID: 977
		public const string UseItemKeySeizeCharacterId = "UseItemKeySeizeCharacterId";

		// Token: 0x040003D2 RID: 978
		public const string ItemKeySeizeCarrierInCombat = "ItemKeySeizeCarrierInCombat";

		// Token: 0x040003D3 RID: 979
		public const string CarrierItemKeyGotInCombat = "CarrierItemKeyGotInCombat";

		// Token: 0x040003D4 RID: 980
		public const string UsedFuyuSwordInCombat = "UsedFuyuSwordInCombat";

		// Token: 0x040003D5 RID: 981
		public const string DefaultHandleFlag = "DefaultHandleFlag";

		// Token: 0x040003D6 RID: 982
		public const string PresetKeyDateOfNextSwordTombActivate = "ConchShip_PresetKey_DateOfNextSwordTombActivate";

		// Token: 0x040003D7 RID: 983
		public const string ShopBuyMoney = "ConchShip_PresetKey_ShopBuyMoney";

		// Token: 0x040003D8 RID: 984
		public const string ShopHasAnyTrade = "ConchShip_PresetKey_ShopHasAnyTrade";

		// Token: 0x040003D9 RID: 985
		public const string ProtectedByWeiQiCharacter = "ProtectedByWeiQiCharacter";

		// Token: 0x040003DA RID: 986
		public const string MainRoleAdjustClothId = "ConchShip_PresetKey_MainRoleAdjustClothId";

		// Token: 0x040003DB RID: 987
		public const string TargetRoleAdjustClothId = "ConchShip_PresetKey_TargetRoleAdjustClothId";

		// Token: 0x040003DC RID: 988
		public const string TradeItemPrice = "ConchShip_PresetKey_TradeItemPrice";

		// Token: 0x040003DD RID: 989
		public const string SetGivenNameMatchSensitive = "ConchShip_PresetKey_SetGivenNameMatchSensitive";

		// Token: 0x040003DE RID: 990
		public const string SetGivenNameMatchSystemRuleType = "ConchShip_PresetKey_SetGivenNameMatchSystemRuleType";

		// Token: 0x040003DF RID: 991
		public const string CaravanCount = "ConchShip_PresetKey_CaravanCount";

		// Token: 0x040003E0 RID: 992
		public const string CaravanIndex = "ConchShip_PresetKey_CaravanIndex";

		// Token: 0x040003E1 RID: 993
		public const string CaravanPresentCount = "ConchShip_PresetKey_CaravanPresentCount";

		// Token: 0x040003E2 RID: 994
		public const string FinishSkillExecute = "ConchShip_PresetKey_FinishSkillExecute";

		// Token: 0x040003E3 RID: 995
		public const string SpecifyEventBackground = "ConchShip_PresetKey_SpecifyEventBackground";

		// Token: 0x040003E4 RID: 996
		public const string OptionWaitConfirmKey = "ConchShip_PresetKey_OptionWaitConfirm";

		// Token: 0x040003E5 RID: 997
		public const string ConfirmWaitOptionSignal = "ConchShip_PresetKey_ConfirmWaitOptionSignal";

		// Token: 0x040003E6 RID: 998
		public const string JiaoId = "JiaoId";

		// Token: 0x040003E7 RID: 999
		public const string EventLogMainCharacter = "ConchShip_PresetKey_EventLogMainCharacter";

		// Token: 0x040003E8 RID: 1000
		public const string StillAtShaolin = "ConchShip_PresetKey_StillAtShaolin";

		// Token: 0x040003E9 RID: 1001
		public const string StillAtEmei = "ConchShip_PresetKey_StillAtEmei";

		// Token: 0x040003EA RID: 1002
		public const string StillAtBaihua = "ConchShip_PresetKey_StillAtBaihua";

		// Token: 0x040003EB RID: 1003
		public const string StillAtWudang = "ConchShip_PresetKey_StillAtWudang";

		// Token: 0x040003EC RID: 1004
		public const string StillAtYuanshan = "ConchShip_PresetKey_StillAtYuanshan";

		// Token: 0x040003ED RID: 1005
		public const string StillAtShixiang = "ConchShip_PresetKey_StillAtShixiang";

		// Token: 0x040003EE RID: 1006
		public const string StillAtRanshan = "ConchShip_PresetKey_StillAtRanshan";

		// Token: 0x040003EF RID: 1007
		public const string StillAtXuannv = "ConchShip_PresetKey_StillAtXuannv";

		// Token: 0x040003F0 RID: 1008
		public const string StillAtZhujian = "ConchShip_PresetKey_StillAtZhujian";

		// Token: 0x040003F1 RID: 1009
		public const string StillAtKongsang = "ConchShip_PresetKey_StillAtKongsang";

		// Token: 0x040003F2 RID: 1010
		public const string StillAtJingang = "ConchShip_PresetKey_StillAtJingang";

		// Token: 0x040003F3 RID: 1011
		public const string StillAtWuxian = "ConchShip_PresetKey_StillAtWuxian";

		// Token: 0x040003F4 RID: 1012
		public const string StillAtJieqing = "ConchShip_PresetKey_StillAtJieqing";

		// Token: 0x040003F5 RID: 1013
		public const string StillAtFulong = "ConchShip_PresetKey_StillAtFulong";

		// Token: 0x040003F6 RID: 1014
		public const string StillAtXuehou = "ConchShip_PresetKey_StillAtXuehou";

		// Token: 0x040003F7 RID: 1015
		public const string MonkeyRobRoad = "MonkeyRobRoad";

		// Token: 0x040003F8 RID: 1016
		public const string CatchCricketTimes = "CatchCricketTimes";

		// Token: 0x040003F9 RID: 1017
		public const string RoleWoodenMan = "RoleWoodenMan";

		// Token: 0x040003FA RID: 1018
		public const string WaitFinalCatchResult = "WaitFinalCatchResult";

		// Token: 0x040003FB RID: 1019
		public const string WaitForCriketSecond = "WaitForCriketSecond";

		// Token: 0x040003FC RID: 1020
		public const string WaitForCriketFourth = "WaitForCriketFourth";

		// Token: 0x040003FD RID: 1021
		public const string MeetMonkey = "MeetMonkey";

		// Token: 0x040003FE RID: 1022
		public const string SmallVillage_GirlCharId = "GirlCharId";

		// Token: 0x040003FF RID: 1023
		public const string SmallVillage_YouthCharId = "YouthCharId";

		// Token: 0x04000400 RID: 1024
		public const string SmallVillage_BigWigCharId = "BigWigCharId";

		// Token: 0x04000401 RID: 1025
		public const string SmallVillage_ChildCharId = "ChildCharId";

		// Token: 0x04000402 RID: 1026
		public const string SmallVillage_ChuiXingId = "ChuiXingId";

		// Token: 0x04000403 RID: 1027
		public const string SmallVillage_DaoshiAskForWildFood = "DaoshiAskForWildFood";

		// Token: 0x04000404 RID: 1028
		public const string SmallVillage_BattleWithXiangshuMinion = "BattleWithXiangshuMinion";

		// Token: 0x04000405 RID: 1029
		public const string SmallVillage_MeetInfectedVillagerOnMap = "ConchShip_PresetKey_SmallVillage_MeetInfectedVillagerOnMap";

		// Token: 0x04000406 RID: 1030
		public const string VillageChange_SaveInfectedVillagerCount = "ConchShip_PresetKey_VillageChange_SaveInfectedVillagerCount";

		// Token: 0x04000407 RID: 1031
		public const string SaveAnyVillager = "SaveAnyVillager";

		// Token: 0x04000408 RID: 1032
		public const string RiverHaveBoat = "RiverHaveBoat";

		// Token: 0x04000409 RID: 1033
		public const string VillageHaveChanged = "VillageHaveChanged";

		// Token: 0x0400040A RID: 1034
		public const string ChatWithANiu = "ChatWithANiu";

		// Token: 0x0400040B RID: 1035
		public const string ChatWithGuoYan = "ChatWithGuoYan";

		// Token: 0x0400040C RID: 1036
		public const string ChatWithXiaomao = "ChatWithXiaomao";

		// Token: 0x0400040D RID: 1037
		public const string ChatWithHuanyue = "ChatWithHuanyue";

		// Token: 0x0400040E RID: 1038
		public const string TryStealBoat = "TryStealBoat";

		// Token: 0x0400040F RID: 1039
		public const string VillageRecordCount = "VillageRecordCount";

		// Token: 0x04000410 RID: 1040
		public const string AllVillagerDieMonth = "AllVillagerDieMonth";

		// Token: 0x04000411 RID: 1041
		public const string VillagePlayCombatInteract = "VillagePlayCombatInteract";

		// Token: 0x04000412 RID: 1042
		public const string VillageCricketInteract = "VillageCricketInteract";

		// Token: 0x04000413 RID: 1043
		public const string LoopDeliver = "LoopDeliver";

		// Token: 0x04000414 RID: 1044
		public const string DeliverVegetable = "DeliverVegetable";

		// Token: 0x04000415 RID: 1045
		public const string GirlLocation = "GirlLocation";

		// Token: 0x04000416 RID: 1046
		public const string BigWigLocation = "BigWigLocation";

		// Token: 0x04000417 RID: 1047
		public const string ChildLocation = "ChildLocation";

		// Token: 0x04000418 RID: 1048
		public const string YouthLocation = "YouthLocation";

		// Token: 0x04000419 RID: 1049
		public const string VillageLocation = "VillageLocation";

		// Token: 0x0400041A RID: 1050
		public const string RecordResource = "RecordResource";

		// Token: 0x0400041B RID: 1051
		public const string MeetSmallVilliage = "MeetSmallVilliage";

		// Token: 0x0400041C RID: 1052
		public const string StonePotGot = "StonePotGot";

		// Token: 0x0400041D RID: 1053
		public const string HaveReturnedVillage = "HaveReturnedVillage";

		// Token: 0x0400041E RID: 1054
		public const string VillageChangedWithoutTaiwu = "VillageChangedWithoutTaiwu";

		// Token: 0x0400041F RID: 1055
		public const string BrokenDate = "BrokenDate";

		// Token: 0x04000420 RID: 1056
		public const string PostLocation = "PostLocation";

		// Token: 0x04000421 RID: 1057
		public const string WangliuLocation = "WangliuLocation";

		// Token: 0x04000422 RID: 1058
		public const string WangliuFirstMeet = "WangliuFirstMeet";

		// Token: 0x04000423 RID: 1059
		public const string CarterActivated = "CarterActivated";

		// Token: 0x04000424 RID: 1060
		public const string FuyuHiltGuiding = "ConchShip_PresetKey_FuyuHiltGuiding";

		// Token: 0x04000425 RID: 1061
		public const string FuyuHiltCatchUpCount = "ConchShip_PresetKey_FuyuHiltCatchUpCount";

		// Token: 0x04000426 RID: 1062
		public const string IsDreamBackArchive = "IsDreamBackArchive";

		// Token: 0x04000427 RID: 1063
		public const string TaiwuCrossArchiveEventTriggered = "ConchShip_PresetKey_TaiwuCrossArchiveEventTriggered";

		// Token: 0x04000428 RID: 1064
		public const string TaiwuCrossArchiveAvatarData = "ConchShip_PresetKey_TaiwuCrossArchiveAvatarData";

		// Token: 0x04000429 RID: 1065
		public const string TaiwuCrossArchiveDisplayName = "ConchShip_PresetKey_TaiwuCrossArchiveDisplayName";

		// Token: 0x0400042A RID: 1066
		public const string TaiwuCrossArchiveOptionSelected = "ConchShip_PresetKey_TaiwuCrossArchiveAvatarData";

		// Token: 0x0400042B RID: 1067
		public const string TaiwuVillageStationOpenDate = "CS_PK_StationOpenDate";

		// Token: 0x0400042C RID: 1068
		public const string CaravanVisitMonthEventTriggered = "CS_PK_CaravanVisit";

		// Token: 0x0400042D RID: 1069
		public const string OldMonkId = "OldMonk";

		// Token: 0x0400042E RID: 1070
		public const string MissNingId = "MissNing";

		// Token: 0x0400042F RID: 1071
		public const string YirenId = "Yiren";

		// Token: 0x04000430 RID: 1072
		public const string BloodCharacter = "BloodCharacter";

		// Token: 0x04000431 RID: 1073
		public const string DefeatXiangshuMinion = "DefeatXiangshuMinion";

		// Token: 0x04000432 RID: 1074
		public const string SaveVillager = "SaveVillager";

		// Token: 0x04000433 RID: 1075
		public const string OutTaiwuVillage = "OutTaiwuVillage";

		// Token: 0x04000434 RID: 1076
		public const string ReturnSmallVillageEvent = "ReturnSmallVillageEvent";

		// Token: 0x04000435 RID: 1077
		public const string BorrowBoat = "BorrowBoat";

		// Token: 0x04000436 RID: 1078
		public const string TaiwuAncestral = "TaiwuAncestral";

		// Token: 0x04000437 RID: 1079
		public const string HelpAreaSect = "HelpAreaSect";

		// Token: 0x04000438 RID: 1080
		public const string HelpEnemySect = "HelpEnemySect";

		// Token: 0x04000439 RID: 1081
		public const string WaitingForPostStory = "WaitingForPostStory";

		// Token: 0x0400043A RID: 1082
		public const string WaitForWesternMerchants = "WaitForWesternMerchants";

		// Token: 0x0400043B RID: 1083
		public const string WaitForReincarnationOpen = "WaitForReincarnationOpen";

		// Token: 0x0400043C RID: 1084
		public const string TaiwuPostLocation = "TaiwuPostLocation";

		// Token: 0x0400043D RID: 1085
		public const string OldMonkToSwordTombCount = "OldMonkToSwordTombCount";

		// Token: 0x0400043E RID: 1086
		public const string OldMonkSwordTombTalk = "OldMonkSwordTombTalk";

		// Token: 0x0400043F RID: 1087
		public const string WakeUpAfterUsingSwordFirst = "WakeUpAfterUsingSwordFirst";

		// Token: 0x04000440 RID: 1088
		public const string WaitTaiwuShrineComplete = "WaitTaiwuShrineComplete";

		// Token: 0x04000441 RID: 1089
		public const string WakeUpAfterImmortalXuDestory = "WakeUpAfterImmortalXuDestory";

		// Token: 0x04000442 RID: 1090
		public const string WaitForTombImmortalFirst = "WaitForTombImmortalFirst";

		// Token: 0x04000443 RID: 1091
		public const string WaitForTombImmortalSecond = "WaitForTombImmortalSecond";

		// Token: 0x04000444 RID: 1092
		public const string WaitForTombImmortalThird = "WaitForTombImmortalThird";

		// Token: 0x04000445 RID: 1093
		public const string WaitForXiangongBack = "WaitForXiangongBack";

		// Token: 0x04000446 RID: 1094
		public const string WaitForSwordTombAppearance = "WaitForSwordTombAppearance";

		// Token: 0x04000447 RID: 1095
		public const string FarewellXuXiangong = "FarewellXuXiangong";

		// Token: 0x04000448 RID: 1096
		public const string WaitForPurpleBambooAppear = "WaitForPurpleBambooAppear";

		// Token: 0x04000449 RID: 1097
		public const string WaitForRanchenVisit = "WaitForRanchenVisit";

		// Token: 0x0400044A RID: 1098
		public const string WaitTaiwuVillageDestory = "WaitTaiwuVillageDestory";

		// Token: 0x0400044B RID: 1099
		public const string TrySurroundTaiwuVillage = "TrySurroundTaiwuVillage";

		// Token: 0x0400044C RID: 1100
		public const string WaitForXiangongTime = "WaitForXiangongTime";

		// Token: 0x0400044D RID: 1101
		public const string ImmortalXuMoveForSpiriteLand = "ImmortalXuMoveForSpiriteLand";

		// Token: 0x0400044E RID: 1102
		public const string ImmortalXuBattle = "ImmortalXuBattle";

		// Token: 0x0400044F RID: 1103
		public const string MeetLongYufu = "CS_PK_MeetLongYufu";

		// Token: 0x04000450 RID: 1104
		public const string WaitForFirstWulinConference = "WaitForFirstWulinConference";

		// Token: 0x04000451 RID: 1105
		public const string SwordStoveTombName = "SwordStoveTombName";

		// Token: 0x04000452 RID: 1106
		public const string SwordStoveTombId = "SwordStoveTombId";

		// Token: 0x04000453 RID: 1107
		public const string HuanxinCombatDie = "HuanxinCombatDie";

		// Token: 0x04000454 RID: 1108
		public const string FirstMeetJunior = "FirstMeetJunior";

		// Token: 0x04000455 RID: 1109
		public const string WaitForWuXiaoSpirit = "WaitForWuXiaoSpirit";

		// Token: 0x04000456 RID: 1110
		public const string WuXiaoSacrificeStory = "WuXiaoSacrificeStory";

		// Token: 0x04000457 RID: 1111
		public const string WaitWuXiaoDream = "WaitWuXiaoDream";

		// Token: 0x04000458 RID: 1112
		public const string BlackBambooTime = "BlackBambooTime";

		// Token: 0x04000459 RID: 1113
		public const string WaitForBlackBambooBorn = "WaitForBlackBambooBorn";

		// Token: 0x0400045A RID: 1114
		public const string RockBambooCreateMonth = "RockBambooCreateMonth";

		// Token: 0x0400045B RID: 1115
		public const string TaiwuMeetXiangshu = "TaiwuMeetXiangshu";

		// Token: 0x0400045C RID: 1116
		public const string WaitFightXiangshuBegin = "WaitFightXiangshuBegin";

		// Token: 0x0400045D RID: 1117
		public const string SealEvilPoints = "SealEvilPoints";

		// Token: 0x0400045E RID: 1118
		public const string PassOutByUsingSwordFirst = "PassOutByUsingSwordFirst";

		// Token: 0x0400045F RID: 1119
		public const string MarriedTaiwuId = "MarriedTaiwuId";

		// Token: 0x04000460 RID: 1120
		public const string GivenCloth = "GivenCloth";

		// Token: 0x04000461 RID: 1121
		public const string WaitBlackToReturnMainMenu = "WaitBlackToReturnMainMenu";

		// Token: 0x04000462 RID: 1122
		public const string WaitCollectWoodOuter3 = "WaitCollectWoodOuter3";

		// Token: 0x04000463 RID: 1123
		public const string WaitCreateBambooThorn = "WaitCreateBambooThorn";

		// Token: 0x04000464 RID: 1124
		public const string WaitBambooComplete = "WaitBambooComplete";

		// Token: 0x04000465 RID: 1125
		public const string AwayForeverTime = "AwayForeverTime";

		// Token: 0x04000466 RID: 1126
		public const string ForeverLoverId = "ForeverLoverId";

		// Token: 0x04000467 RID: 1127
		public const string StoryForeverLoverId = "StoryForeverLoverId";

		// Token: 0x04000468 RID: 1128
		public const string YuFuTellRanchenziStory = "YuFuTellRanchenziStory";

		// Token: 0x04000469 RID: 1129
		public const string IsQuickStartGame = "CS_PK_IsQuickStartGame";

		// Token: 0x0400046A RID: 1130
		public const string IsGuardCombat = "IsGuardCombat";

		// Token: 0x0400046B RID: 1131
		public const string GuardCombatLevel = "GuardCombatLevel";

		// Token: 0x0400046C RID: 1132
		public const string FulongServantSetGender = "FulongServantSetGender";

		// Token: 0x0400046D RID: 1133
		public const string FulongServantSetTransgender = "FulongServantSetTransgender";

		// Token: 0x0400046E RID: 1134
		public const string FulongServantSetBehaviorType = "FulongServantSetBehaviorType";

		// Token: 0x0400046F RID: 1135
		public const string FulongServantSetLifeSkillType = "FulongServantSetLifeSkillType";

		// Token: 0x04000470 RID: 1136
		public const string FulongServantSetCombatSkillType = "FulongServantSetCombatSkillType";

		// Token: 0x04000471 RID: 1137
		public const string FulongServantSetMainAttributeType = "FulongServantSetMainAttributeType";

		// Token: 0x04000472 RID: 1138
		private Dictionary<string, int> _intBox;

		// Token: 0x04000473 RID: 1139
		private Dictionary<string, string> _stringBox;

		// Token: 0x04000474 RID: 1140
		private Dictionary<string, float> _floatBox;

		// Token: 0x04000475 RID: 1141
		private Dictionary<string, bool> _boolBox;

		// Token: 0x04000476 RID: 1142
		private Dictionary<string, ISerializableGameData> _serializableObjectBox;

		// Token: 0x04000477 RID: 1143
		public static readonly Dictionary<Type, sbyte> SerializeObjectMap = new Dictionary<Type, sbyte>
		{
			{
				typeof(Location),
				0
			},
			{
				typeof(AdventureMapPoint),
				1
			},
			{
				typeof(ItemKey),
				2
			},
			{
				typeof(AdventureSiteData),
				3
			},
			{
				typeof(MapTemplateEnemyInfo),
				4
			},
			{
				typeof(AvatarRelatedData),
				5
			},
			{
				typeof(EventActorData),
				6
			},
			{
				typeof(ShortList),
				7
			},
			{
				typeof(AvatarData),
				8
			},
			{
				typeof(IntList),
				9
			}
		};

		// Token: 0x04000478 RID: 1144
		private static int _instancesCount = 0;

		// Token: 0x04000479 RID: 1145
		private static int _objectCollectionsCount = 0;
	}
}
