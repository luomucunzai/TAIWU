using System;
using GameData.Common;
using GameData.Domains;

namespace GameData.Dependencies
{
	// Token: 0x020008E2 RID: 2274
	public class DataDependency
	{
		// Token: 0x060081B1 RID: 33201 RVA: 0x004D78D9 File Offset: 0x004D5AD9
		public DataDependency(DomainDataType sourceType, DataUid[] sourceUids, InfluenceCondition condition, InfluenceScope scope)
		{
			this.SourceType = sourceType;
			this.SourceUids = sourceUids;
			this.Condition = condition;
			this.Scope = scope;
		}

		// Token: 0x060081B2 RID: 33202 RVA: 0x004D7900 File Offset: 0x004D5B00
		public override string ToString()
		{
			ushort domainId = this.SourceUids[0].DomainId;
			string domainName = DomainHelper.DomainId2DomainName[(int)domainId];
			string[] dataId2FieldName = DomainHelper.DomainId2DataId2FieldName[(int)domainId];
			int uidsCount = this.SourceUids.Length;
			string[] dataNames = new string[uidsCount];
			string prefix;
			switch (this.SourceType)
			{
			case DomainDataType.SingleValue:
			case DomainDataType.SingleValueCollection:
				prefix = domainName;
				for (int i = 0; i < uidsCount; i++)
				{
					ushort dataId = this.SourceUids[i].DataId;
					string fieldName = dataId2FieldName[(int)dataId];
					dataNames[i] = fieldName;
				}
				goto IL_16F;
			case DomainDataType.ElementList:
			{
				ushort dataId2 = this.SourceUids[0].DataId;
				string fieldName2 = dataId2FieldName[(int)dataId2];
				prefix = domainName + "." + fieldName2;
				for (int j = 0; j < uidsCount; j++)
				{
					dataNames[j] = this.SourceUids[j].SubId0.ToString();
				}
				goto IL_16F;
			}
			}
			ushort dataId3 = this.SourceUids[0].DataId;
			string fieldName3 = dataId2FieldName[(int)dataId3];
			prefix = domainName + "." + fieldName3 + ".";
			string[] fieldId2FieldName = DomainHelper.DomainId2DataId2ObjectFieldId2FieldName[(int)domainId][(int)dataId3];
			for (int k = 0; k < uidsCount; k++)
			{
				uint fieldId = this.SourceUids[k].SubId1;
				dataNames[k] = fieldId2FieldName[(int)fieldId];
			}
			IL_16F:
			return prefix + ".[" + string.Join(", ", dataNames) + "]";
		}

		// Token: 0x040023D8 RID: 9176
		public readonly DomainDataType SourceType;

		// Token: 0x040023D9 RID: 9177
		public readonly DataUid[] SourceUids;

		// Token: 0x040023DA RID: 9178
		public readonly InfluenceCondition Condition;

		// Token: 0x040023DB RID: 9179
		public readonly InfluenceScope Scope;
	}
}
