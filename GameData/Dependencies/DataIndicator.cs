using System;
using System.Runtime.CompilerServices;
using GameData.Common;
using GameData.Domains;

namespace GameData.Dependencies
{
	// Token: 0x020008E3 RID: 2275
	public readonly struct DataIndicator : IEquatable<DataIndicator>
	{
		// Token: 0x060081B3 RID: 33203 RVA: 0x004D7A9E File Offset: 0x004D5C9E
		public DataIndicator(DomainDataType dataType, ushort domainId, ushort dataId)
		{
			this.DataType = dataType;
			this.DomainId = domainId;
			this.DataId = dataId;
		}

		// Token: 0x060081B4 RID: 33204 RVA: 0x004D7AB8 File Offset: 0x004D5CB8
		public bool Equals(DataIndicator other)
		{
			return this.DataType == other.DataType && this.DomainId == other.DomainId && this.DataId == other.DataId;
		}

		// Token: 0x060081B5 RID: 33205 RVA: 0x004D7AF8 File Offset: 0x004D5CF8
		public override bool Equals(object obj)
		{
			bool result;
			if (obj is DataIndicator)
			{
				DataIndicator other = (DataIndicator)obj;
				result = this.Equals(other);
			}
			else
			{
				result = false;
			}
			return result;
		}

		// Token: 0x060081B6 RID: 33206 RVA: 0x004D7B24 File Offset: 0x004D5D24
		public override int GetHashCode()
		{
			int hashCode = (int)this.DataType;
			hashCode = (hashCode * 397 ^ this.DomainId.GetHashCode());
			return hashCode * 397 ^ this.DataId.GetHashCode();
		}

		// Token: 0x060081B7 RID: 33207 RVA: 0x004D7B68 File Offset: 0x004D5D68
		public override string ToString()
		{
			string dataTypeName = Enum.GetName(typeof(DomainDataType), this.DataType);
			string domainName = DomainHelper.DomainId2DomainName[(int)this.DomainId];
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			switch (this.DataType)
			{
			case DomainDataType.SingleValue:
			case DomainDataType.SingleValueCollection:
				return domainName + "(" + dataTypeName + ")";
			case DomainDataType.ElementList:
			{
				string dataName = DomainHelper.DomainId2DataId2FieldName[(int)this.DomainId][(int)this.DataId];
				defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(3, 3);
				defaultInterpolatedStringHandler.AppendFormatted(domainName);
				defaultInterpolatedStringHandler.AppendLiteral(".");
				defaultInterpolatedStringHandler.AppendFormatted(dataName);
				defaultInterpolatedStringHandler.AppendLiteral("(");
				defaultInterpolatedStringHandler.AppendFormatted(dataTypeName);
				defaultInterpolatedStringHandler.AppendLiteral(")");
				return defaultInterpolatedStringHandler.ToStringAndClear();
			}
			}
			string dataName2 = DomainHelper.DomainId2DataId2FieldName[(int)this.DomainId][(int)this.DataId];
			defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(3, 3);
			defaultInterpolatedStringHandler.AppendFormatted(domainName);
			defaultInterpolatedStringHandler.AppendLiteral(".");
			defaultInterpolatedStringHandler.AppendFormatted(dataName2);
			defaultInterpolatedStringHandler.AppendLiteral("(");
			defaultInterpolatedStringHandler.AppendFormatted(dataTypeName);
			defaultInterpolatedStringHandler.AppendLiteral(")");
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x040023DC RID: 9180
		public readonly DomainDataType DataType;

		// Token: 0x040023DD RID: 9181
		public readonly ushort DomainId;

		// Token: 0x040023DE RID: 9182
		public readonly ushort DataId;
	}
}
