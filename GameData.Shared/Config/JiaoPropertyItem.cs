using System;
using Config.Common;

namespace Config;

[Serializable]
public class JiaoPropertyItem : ConfigItem<JiaoPropertyItem, short>
{
	public readonly short TemplateId;

	public readonly string Name;

	public readonly string EventDescUp;

	public readonly string EventDescDown;

	public readonly int[] EventChange;

	public readonly int EscapeChange;

	public readonly int AggressivePropertyParam;

	public readonly int AggressiveComfortParam;

	public readonly int AggressiveNotComfortParam;

	public readonly int NeutralityPropertyParam;

	public readonly int ConservedPropertyParam;

	public readonly int ConservedTameParam;

	public readonly int MaxValue;

	public readonly short JiaoRecordTemplateId;

	public readonly short JiaoNurturanceTemplateId;

	public readonly string TipsIcon;

	public readonly string SpecialDescTitle;

	public readonly string SpecialDesc;

	public JiaoPropertyItem(short templateId, int name, int eventDescUp, int eventDescDown, int[] eventChange, int escapeChange, int aggressivePropertyParam, int aggressiveComfortParam, int aggressiveNotComfortParam, int neutralityPropertyParam, int conservedPropertyParam, int conservedTameParam, int maxValue, short jiaoRecordTemplateId, short jiaoNurturanceTemplateId, string tipsIcon, string specialDescTitle, string specialDesc)
	{
		TemplateId = templateId;
		Name = LocalStringManager.GetConfig("JiaoProperty_language", name);
		EventDescUp = LocalStringManager.GetConfig("JiaoProperty_language", eventDescUp);
		EventDescDown = LocalStringManager.GetConfig("JiaoProperty_language", eventDescDown);
		EventChange = eventChange;
		EscapeChange = escapeChange;
		AggressivePropertyParam = aggressivePropertyParam;
		AggressiveComfortParam = aggressiveComfortParam;
		AggressiveNotComfortParam = aggressiveNotComfortParam;
		NeutralityPropertyParam = neutralityPropertyParam;
		ConservedPropertyParam = conservedPropertyParam;
		ConservedTameParam = conservedTameParam;
		MaxValue = maxValue;
		JiaoRecordTemplateId = jiaoRecordTemplateId;
		JiaoNurturanceTemplateId = jiaoNurturanceTemplateId;
		TipsIcon = tipsIcon;
		SpecialDescTitle = specialDescTitle;
		SpecialDesc = specialDesc;
	}

	public JiaoPropertyItem()
	{
		TemplateId = 0;
		Name = null;
		EventDescUp = null;
		EventDescDown = null;
		EventChange = new int[0];
		EscapeChange = 0;
		AggressivePropertyParam = 0;
		AggressiveComfortParam = 0;
		AggressiveNotComfortParam = 0;
		NeutralityPropertyParam = 0;
		ConservedPropertyParam = 0;
		ConservedTameParam = 0;
		MaxValue = 0;
		JiaoRecordTemplateId = 0;
		JiaoNurturanceTemplateId = 0;
		TipsIcon = null;
		SpecialDescTitle = null;
		SpecialDesc = null;
	}

	public JiaoPropertyItem(short templateId, JiaoPropertyItem other)
	{
		TemplateId = templateId;
		Name = other.Name;
		EventDescUp = other.EventDescUp;
		EventDescDown = other.EventDescDown;
		EventChange = other.EventChange;
		EscapeChange = other.EscapeChange;
		AggressivePropertyParam = other.AggressivePropertyParam;
		AggressiveComfortParam = other.AggressiveComfortParam;
		AggressiveNotComfortParam = other.AggressiveNotComfortParam;
		NeutralityPropertyParam = other.NeutralityPropertyParam;
		ConservedPropertyParam = other.ConservedPropertyParam;
		ConservedTameParam = other.ConservedTameParam;
		MaxValue = other.MaxValue;
		JiaoRecordTemplateId = other.JiaoRecordTemplateId;
		JiaoNurturanceTemplateId = other.JiaoNurturanceTemplateId;
		TipsIcon = other.TipsIcon;
		SpecialDescTitle = other.SpecialDescTitle;
		SpecialDesc = other.SpecialDesc;
	}

	public override int GetTemplateId()
	{
		return TemplateId;
	}

	public override JiaoPropertyItem Duplicate(int templateId)
	{
		return new JiaoPropertyItem((short)templateId, this);
	}
}
