using GameData.Common;
using GameData.Domains;

namespace GameData.Dependencies;

public class DataDependency
{
	public readonly DomainDataType SourceType;

	public readonly DataUid[] SourceUids;

	public readonly InfluenceCondition Condition;

	public readonly InfluenceScope Scope;

	public DataDependency(DomainDataType sourceType, DataUid[] sourceUids, InfluenceCondition condition, InfluenceScope scope)
	{
		SourceType = sourceType;
		SourceUids = sourceUids;
		Condition = condition;
		Scope = scope;
	}

	public override string ToString()
	{
		ushort domainId = SourceUids[0].DomainId;
		string text = DomainHelper.DomainId2DomainName[domainId];
		string[] array = DomainHelper.DomainId2DataId2FieldName[domainId];
		int num = SourceUids.Length;
		string[] array2 = new string[num];
		string text3;
		switch (SourceType)
		{
		case DomainDataType.SingleValue:
		case DomainDataType.SingleValueCollection:
		{
			text3 = text;
			for (int j = 0; j < num; j++)
			{
				ushort dataId2 = SourceUids[j].DataId;
				string text4 = array[dataId2];
				array2[j] = text4;
			}
			break;
		}
		case DomainDataType.ElementList:
		{
			ushort dataId3 = SourceUids[0].DataId;
			string text5 = array[dataId3];
			text3 = text + "." + text5;
			for (int k = 0; k < num; k++)
			{
				array2[k] = SourceUids[k].SubId0.ToString();
			}
			break;
		}
		default:
		{
			ushort dataId = SourceUids[0].DataId;
			string text2 = array[dataId];
			text3 = text + "." + text2 + ".";
			string[] array3 = DomainHelper.DomainId2DataId2ObjectFieldId2FieldName[domainId][dataId];
			for (int i = 0; i < num; i++)
			{
				uint subId = SourceUids[i].SubId1;
				array2[i] = array3[subId];
			}
			break;
		}
		}
		return text3 + ".[" + string.Join(", ", array2) + "]";
	}
}
