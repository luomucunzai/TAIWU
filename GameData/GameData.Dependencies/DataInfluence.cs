using System;
using System.Collections.Generic;
using GameData.Common;
using GameData.Domains;

namespace GameData.Dependencies;

public class DataInfluence : IEquatable<DataInfluence>
{
	public DataIndicator TargetIndicator;

	public InfluenceCondition Condition;

	public InfluenceScope Scope;

	public readonly List<DataUid> TargetUids;

	public DataInfluence(DataIndicator targetIndicator, InfluenceCondition condition, InfluenceScope scope)
	{
		TargetIndicator = targetIndicator;
		Condition = condition;
		Scope = scope;
		TargetUids = new List<DataUid>();
	}

	public bool Equals(DataInfluence other)
	{
		if (other == null)
		{
			return false;
		}
		if (this == other)
		{
			return true;
		}
		return TargetIndicator.Equals(other.TargetIndicator) && Condition == other.Condition && Scope == other.Scope;
	}

	public override bool Equals(object obj)
	{
		if (obj == null)
		{
			return false;
		}
		if (this == obj)
		{
			return true;
		}
		if (obj.GetType() != GetType())
		{
			return false;
		}
		return Equals((DataInfluence)obj);
	}

	public override int GetHashCode()
	{
		int hashCode = TargetIndicator.GetHashCode();
		hashCode = (hashCode * 397) ^ (int)Condition;
		return (hashCode * 397) ^ (int)Scope;
	}

	public override string ToString()
	{
		ushort domainId = TargetIndicator.DomainId;
		string text = DomainHelper.DomainId2DomainName[domainId];
		string[] array = DomainHelper.DomainId2DataId2FieldName[domainId];
		int count = TargetUids.Count;
		string[] array2 = new string[count];
		string text3;
		switch (TargetIndicator.DataType)
		{
		case DomainDataType.SingleValue:
		case DomainDataType.SingleValueCollection:
		{
			text3 = text;
			for (int j = 0; j < count; j++)
			{
				ushort dataId2 = TargetUids[j].DataId;
				string text4 = array[dataId2];
				array2[j] = text4;
			}
			break;
		}
		case DomainDataType.ElementList:
		{
			ushort dataId3 = TargetIndicator.DataId;
			string text5 = array[dataId3];
			text3 = text + "." + text5;
			for (int k = 0; k < count; k++)
			{
				array2[k] = TargetUids[k].SubId0.ToString();
			}
			break;
		}
		default:
		{
			ushort dataId = TargetIndicator.DataId;
			string text2 = array[dataId];
			text3 = text + "." + text2 + ".";
			string[] array3 = DomainHelper.DomainId2DataId2ObjectFieldId2FieldName[domainId][dataId];
			for (int i = 0; i < count; i++)
			{
				uint subId = TargetUids[i].SubId1;
				array2[i] = array3[subId];
			}
			break;
		}
		}
		return text3 + ".[" + string.Join(", ", array2) + "]";
	}
}
