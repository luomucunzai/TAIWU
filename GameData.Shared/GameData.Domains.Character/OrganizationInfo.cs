using System;
using Config;
using GameData.Serializer;

namespace GameData.Domains.Character;

[Serializable]
public struct OrganizationInfo : ISerializableGameData, IEquatable<OrganizationInfo>
{
	public sbyte OrgTemplateId;

	public sbyte Grade;

	public bool Principal;

	public short SettlementId;

	public static readonly OrganizationInfo None = new OrganizationInfo(0, 0, principal: true, -1);

	public OrganizationInfo(sbyte orgTemplateId, sbyte grade, bool principal = true, short settlementId = -1)
	{
		OrgTemplateId = orgTemplateId;
		Grade = grade;
		Principal = principal;
		SettlementId = settlementId;
	}

	public OrganizationMemberItem GetOrgMemberConfig()
	{
		short index = Config.Organization.Instance[OrgTemplateId].Members[Grade];
		return OrganizationMember.Instance[index];
	}

	public override string ToString()
	{
		string name = Config.Organization.Instance[OrgTemplateId].Name;
		OrganizationMemberItem orgMemberConfig = GetOrgMemberConfig();
		string text = orgMemberConfig.GradeName;
		if (!Principal)
		{
			string[] spouseAnonymousTitles = orgMemberConfig.SpouseAnonymousTitles;
			if (spouseAnonymousTitles != null && spouseAnonymousTitles.Length > 0)
			{
				text = string.Join('/', orgMemberConfig.SpouseAnonymousTitles);
			}
		}
		return name + text;
	}

	public bool IsSerializedSizeFixed()
	{
		return true;
	}

	public int GetSerializedSize()
	{
		return 8;
	}

	public unsafe int Serialize(byte* pData)
	{
		*pData = (byte)OrgTemplateId;
		pData[1] = (byte)Grade;
		pData[2] = (Principal ? ((byte)1) : ((byte)0));
		((short*)pData)[2] = SettlementId;
		return 8;
	}

	public unsafe int Deserialize(byte* pData)
	{
		OrgTemplateId = (sbyte)(*pData);
		Grade = (sbyte)pData[1];
		Principal = pData[2] != 0;
		SettlementId = ((short*)pData)[2];
		return 8;
	}

	public bool Equals(OrganizationInfo other)
	{
		if (OrgTemplateId == other.OrgTemplateId && Grade == other.Grade && Principal == other.Principal)
		{
			return SettlementId == other.SettlementId;
		}
		return false;
	}

	public override bool Equals(object obj)
	{
		if (obj is OrganizationInfo other)
		{
			return Equals(other);
		}
		return false;
	}

	public override int GetHashCode()
	{
		return (((((OrgTemplateId.GetHashCode() * 397) ^ Grade.GetHashCode()) * 397) ^ Principal.GetHashCode()) * 397) ^ SettlementId.GetHashCode();
	}
}
