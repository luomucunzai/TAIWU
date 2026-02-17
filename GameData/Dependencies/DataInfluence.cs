using System;
using System.Collections.Generic;
using GameData.Common;
using GameData.Domains;

namespace GameData.Dependencies
{
	// Token: 0x020008E4 RID: 2276
	public class DataInfluence : IEquatable<DataInfluence>
	{
		// Token: 0x060081B8 RID: 33208 RVA: 0x004D7CB7 File Offset: 0x004D5EB7
		public DataInfluence(DataIndicator targetIndicator, InfluenceCondition condition, InfluenceScope scope)
		{
			this.TargetIndicator = targetIndicator;
			this.Condition = condition;
			this.Scope = scope;
			this.TargetUids = new List<DataUid>();
		}

		// Token: 0x060081B9 RID: 33209 RVA: 0x004D7CE4 File Offset: 0x004D5EE4
		public bool Equals(DataInfluence other)
		{
			bool flag = other == null;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				bool flag2 = this == other;
				result = (flag2 || (this.TargetIndicator.Equals(other.TargetIndicator) && this.Condition == other.Condition && this.Scope == other.Scope));
			}
			return result;
		}

		// Token: 0x060081BA RID: 33210 RVA: 0x004D7D40 File Offset: 0x004D5F40
		public override bool Equals(object obj)
		{
			bool flag = obj == null;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				bool flag2 = this == obj;
				if (flag2)
				{
					result = true;
				}
				else
				{
					bool flag3 = obj.GetType() != base.GetType();
					result = (!flag3 && this.Equals((DataInfluence)obj));
				}
			}
			return result;
		}

		// Token: 0x060081BB RID: 33211 RVA: 0x004D7D90 File Offset: 0x004D5F90
		public override int GetHashCode()
		{
			int hashCode = this.TargetIndicator.GetHashCode();
			hashCode = (hashCode * 397 ^ (int)this.Condition);
			return hashCode * 397 ^ (int)this.Scope;
		}

		// Token: 0x060081BC RID: 33212 RVA: 0x004D7DD4 File Offset: 0x004D5FD4
		public override string ToString()
		{
			ushort domainId = this.TargetIndicator.DomainId;
			string domainName = DomainHelper.DomainId2DomainName[(int)domainId];
			string[] dataId2FieldName = DomainHelper.DomainId2DataId2FieldName[(int)domainId];
			int uidsCount = this.TargetUids.Count;
			string[] dataNames = new string[uidsCount];
			string prefix;
			switch (this.TargetIndicator.DataType)
			{
			case DomainDataType.SingleValue:
			case DomainDataType.SingleValueCollection:
				prefix = domainName;
				for (int i = 0; i < uidsCount; i++)
				{
					ushort dataId = this.TargetUids[i].DataId;
					string fieldName = dataId2FieldName[(int)dataId];
					dataNames[i] = fieldName;
				}
				goto IL_169;
			case DomainDataType.ElementList:
			{
				ushort dataId2 = this.TargetIndicator.DataId;
				string fieldName2 = dataId2FieldName[(int)dataId2];
				prefix = domainName + "." + fieldName2;
				for (int j = 0; j < uidsCount; j++)
				{
					dataNames[j] = this.TargetUids[j].SubId0.ToString();
				}
				goto IL_169;
			}
			}
			ushort dataId3 = this.TargetIndicator.DataId;
			string fieldName3 = dataId2FieldName[(int)dataId3];
			prefix = domainName + "." + fieldName3 + ".";
			string[] fieldId2FieldName = DomainHelper.DomainId2DataId2ObjectFieldId2FieldName[(int)domainId][(int)dataId3];
			for (int k = 0; k < uidsCount; k++)
			{
				uint fieldId = this.TargetUids[k].SubId1;
				dataNames[k] = fieldId2FieldName[(int)fieldId];
			}
			IL_169:
			return prefix + ".[" + string.Join(", ", dataNames) + "]";
		}

		// Token: 0x040023DF RID: 9183
		public DataIndicator TargetIndicator;

		// Token: 0x040023E0 RID: 9184
		public InfluenceCondition Condition;

		// Token: 0x040023E1 RID: 9185
		public InfluenceScope Scope;

		// Token: 0x040023E2 RID: 9186
		public readonly List<DataUid> TargetUids;
	}
}
