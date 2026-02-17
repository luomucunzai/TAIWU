using System;
using GameData.Common;

namespace GameData.Dependencies
{
	// Token: 0x020008E5 RID: 2277
	public readonly struct DataUidWithType : IEquatable<DataUidWithType>
	{
		// Token: 0x060081BD RID: 33213 RVA: 0x004D7F6C File Offset: 0x004D616C
		public DataUidWithType(DomainDataType type, DataUid dataUid)
		{
			this.Type = type;
			this.DataUid = dataUid;
		}

		// Token: 0x060081BE RID: 33214 RVA: 0x004D7F80 File Offset: 0x004D6180
		public bool Equals(DataUidWithType other)
		{
			return this.Type == other.Type && this.DataUid.Equals(other.DataUid);
		}

		// Token: 0x060081BF RID: 33215 RVA: 0x004D7FB4 File Offset: 0x004D61B4
		public override bool Equals(object obj)
		{
			bool result;
			if (obj is DataUidWithType)
			{
				DataUidWithType other = (DataUidWithType)obj;
				result = this.Equals(other);
			}
			else
			{
				result = false;
			}
			return result;
		}

		// Token: 0x060081C0 RID: 33216 RVA: 0x004D7FE0 File Offset: 0x004D61E0
		public override int GetHashCode()
		{
			return (int)this.Type * 397 ^ this.DataUid.GetHashCode();
		}

		// Token: 0x040023E3 RID: 9187
		public readonly DomainDataType Type;

		// Token: 0x040023E4 RID: 9188
		public readonly DataUid DataUid;
	}
}
