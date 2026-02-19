using System;
using System.Collections.Generic;
using Config.Common;
using GameData.Utilities;

namespace Config;

[Serializable]
public class PunishmentTypeItem : ConfigItem<PunishmentTypeItem, short>
{
	public readonly short TemplateId;

	public readonly string Name;

	public readonly string ShortName;

	public readonly EPunishmentTypeDisplayType DisplayType;

	public readonly sbyte Severity;

	public readonly string PunishmentDesc;

	public readonly List<ShortPair> SectPunishmentSeverities;

	public readonly List<ShortPair> CivilianPunishmentSeverities;

	public readonly int ModifySeverityAuthorityCost;

	public readonly uint DlcAppId;

	public PunishmentTypeItem(short templateId, int name, int shortName, EPunishmentTypeDisplayType displayType, sbyte severity, int punishmentDesc, List<ShortPair> sectPunishmentSeverities, List<ShortPair> civilianPunishmentSeverities, int modifySeverityAuthorityCost, uint dlcAppId)
	{
		TemplateId = templateId;
		Name = LocalStringManager.GetConfig("PunishmentType_language", name);
		ShortName = LocalStringManager.GetConfig("PunishmentType_language", shortName);
		DisplayType = displayType;
		Severity = severity;
		PunishmentDesc = LocalStringManager.GetConfig("PunishmentType_language", punishmentDesc);
		SectPunishmentSeverities = sectPunishmentSeverities;
		CivilianPunishmentSeverities = civilianPunishmentSeverities;
		ModifySeverityAuthorityCost = modifySeverityAuthorityCost;
		DlcAppId = dlcAppId;
	}

	public PunishmentTypeItem()
	{
		TemplateId = 0;
		Name = null;
		ShortName = null;
		DisplayType = EPunishmentTypeDisplayType.Criminal;
		Severity = 0;
		PunishmentDesc = null;
		SectPunishmentSeverities = new List<ShortPair>();
		CivilianPunishmentSeverities = new List<ShortPair>();
		ModifySeverityAuthorityCost = 0;
		DlcAppId = 0u;
	}

	public PunishmentTypeItem(short templateId, PunishmentTypeItem other)
	{
		TemplateId = templateId;
		Name = other.Name;
		ShortName = other.ShortName;
		DisplayType = other.DisplayType;
		Severity = other.Severity;
		PunishmentDesc = other.PunishmentDesc;
		SectPunishmentSeverities = other.SectPunishmentSeverities;
		CivilianPunishmentSeverities = other.CivilianPunishmentSeverities;
		ModifySeverityAuthorityCost = other.ModifySeverityAuthorityCost;
		DlcAppId = other.DlcAppId;
	}

	public override int GetTemplateId()
	{
		return TemplateId;
	}

	public override PunishmentTypeItem Duplicate(int templateId)
	{
		return new PunishmentTypeItem((short)templateId, this);
	}

	public sbyte GetSeverity(sbyte stateTemplateId, bool isSect, bool includeDefault = false)
	{
		sbyte result = (sbyte)(includeDefault ? Severity : (-1));
		if (isSect)
		{
			List<ShortPair> sectPunishmentSeverities = SectPunishmentSeverities;
			if (sectPunishmentSeverities == null || sectPunishmentSeverities.Count <= 0)
			{
				return result;
			}
			foreach (ShortPair sectPunishmentSeverity in SectPunishmentSeverities)
			{
				if (sectPunishmentSeverity.First == stateTemplateId)
				{
					return (sbyte)sectPunishmentSeverity.Second;
				}
			}
		}
		else
		{
			List<ShortPair> sectPunishmentSeverities = CivilianPunishmentSeverities;
			if (sectPunishmentSeverities == null || sectPunishmentSeverities.Count <= 0)
			{
				return result;
			}
			foreach (ShortPair civilianPunishmentSeverity in CivilianPunishmentSeverities)
			{
				if (civilianPunishmentSeverity.First == stateTemplateId)
				{
					return (sbyte)civilianPunishmentSeverity.Second;
				}
			}
		}
		return result;
	}
}
