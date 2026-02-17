using System;
using GameData.Serializer;

namespace GameData.Domains.Character.Ai
{
	// Token: 0x02000849 RID: 2121
	[SerializableGameData(NotForDisplayModule = true)]
	public struct AiActionKey : ISerializableGameData, IEquatable<AiActionKey>
	{
		// Token: 0x0600760E RID: 30222 RVA: 0x0044F2F7 File Offset: 0x0044D4F7
		public AiActionKey(sbyte actionType, sbyte actionSubType)
		{
			this.ActionType = actionType;
			this.ActionSubType = actionSubType;
		}

		// Token: 0x0600760F RID: 30223 RVA: 0x0044F308 File Offset: 0x0044D508
		public bool IsSerializedSizeFixed()
		{
			return true;
		}

		// Token: 0x06007610 RID: 30224 RVA: 0x0044F31C File Offset: 0x0044D51C
		public int GetSerializedSize()
		{
			return 2;
		}

		// Token: 0x06007611 RID: 30225 RVA: 0x0044F330 File Offset: 0x0044D530
		public unsafe int Serialize(byte* pData)
		{
			*pData = (byte)this.ActionType;
			pData[1] = (byte)this.ActionSubType;
			return 2;
		}

		// Token: 0x06007612 RID: 30226 RVA: 0x0044F358 File Offset: 0x0044D558
		public unsafe int Deserialize(byte* pData)
		{
			this.ActionType = *(sbyte*)pData;
			this.ActionSubType = *(sbyte*)(pData + 1);
			return 2;
		}

		// Token: 0x06007613 RID: 30227 RVA: 0x0044F380 File Offset: 0x0044D580
		public bool Equals(AiActionKey other)
		{
			return this.ActionType == other.ActionType && this.ActionSubType == other.ActionSubType;
		}

		// Token: 0x06007614 RID: 30228 RVA: 0x0044F3B4 File Offset: 0x0044D5B4
		public override bool Equals(object obj)
		{
			bool result;
			if (obj is AiActionKey)
			{
				AiActionKey other = (AiActionKey)obj;
				result = this.Equals(other);
			}
			else
			{
				result = false;
			}
			return result;
		}

		// Token: 0x06007615 RID: 30229 RVA: 0x0044F3E0 File Offset: 0x0044D5E0
		public override int GetHashCode()
		{
			return HashCode.Combine<sbyte, sbyte>(this.ActionType, this.ActionSubType);
		}

		// Token: 0x0400208E RID: 8334
		public sbyte ActionType;

		// Token: 0x0400208F RID: 8335
		public sbyte ActionSubType;
	}
}
