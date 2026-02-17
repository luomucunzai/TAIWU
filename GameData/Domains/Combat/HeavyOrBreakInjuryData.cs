using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using GameData.Serializer;

namespace GameData.Domains.Combat
{
	// Token: 0x020006C3 RID: 1731
	[SerializableGameData(NotForArchive = true)]
	public struct HeavyOrBreakInjuryData : ISerializableGameData
	{
		// Token: 0x060066D6 RID: 26326 RVA: 0x003AEB64 File Offset: 0x003ACD64
		public unsafe void Initialize()
		{
			fixed (sbyte* ptr = &this._types.FixedElementField)
			{
				sbyte* pItems = ptr;
				*(long*)pItems = 0L;
			}
		}

		// Token: 0x17000401 RID: 1025
		public unsafe EHeavyOrBreakType this[int bodyPart]
		{
			get
			{
				bool flag = bodyPart < 0 || bodyPart >= 7;
				bool flag2 = flag;
				if (flag2)
				{
					throw new ArgumentOutOfRangeException("bodyPart", bodyPart, "out of range");
				}
				return (EHeavyOrBreakType)(*(ref this._types.FixedElementField + bodyPart));
			}
			set
			{
				bool flag = bodyPart < 0 || bodyPart >= 7;
				bool flag2 = flag;
				if (flag2)
				{
					throw new ArgumentOutOfRangeException("bodyPart", bodyPart, "out of range");
				}
				*(ref this._types.FixedElementField + bodyPart) = (sbyte)value;
			}
		}

		// Token: 0x060066D9 RID: 26329 RVA: 0x003AEC20 File Offset: 0x003ACE20
		public bool IsSerializedSizeFixed()
		{
			return true;
		}

		// Token: 0x060066DA RID: 26330 RVA: 0x003AEC34 File Offset: 0x003ACE34
		public int GetSerializedSize()
		{
			return 8;
		}

		// Token: 0x060066DB RID: 26331 RVA: 0x003AEC48 File Offset: 0x003ACE48
		public unsafe int Serialize(byte* pData)
		{
			fixed (sbyte* ptr = &this._types.FixedElementField)
			{
				sbyte* pItems = ptr;
				*(long*)pData = *(long*)pItems;
			}
			return 8;
		}

		// Token: 0x060066DC RID: 26332 RVA: 0x003AEC74 File Offset: 0x003ACE74
		public unsafe int Deserialize(byte* pData)
		{
			fixed (sbyte* ptr = &this._types.FixedElementField)
			{
				sbyte* pItems = ptr;
				*(long*)pItems = *(long*)pData;
			}
			return 8;
		}

		// Token: 0x04001BFA RID: 7162
		private const int Capacity = 8;

		// Token: 0x04001BFB RID: 7163
		[FixedBuffer(typeof(sbyte), 8)]
		[SerializableGameDataField]
		private HeavyOrBreakInjuryData.<_types>e__FixedBuffer _types;

		// Token: 0x02000B72 RID: 2930
		[CompilerGenerated]
		[UnsafeValueType]
		[StructLayout(LayoutKind.Sequential, Size = 8)]
		public struct <_types>e__FixedBuffer
		{
			// Token: 0x040030D7 RID: 12503
			public sbyte FixedElementField;
		}
	}
}
