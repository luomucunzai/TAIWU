using System;
using System.Collections.Generic;
using Config.Common;

namespace Config;

[Serializable]
public class TaiwuBeHuntedEventItem : ConfigItem<TaiwuBeHuntedEventItem, short>
{
	public readonly short TemplateId;

	public readonly string Name;

	public readonly string HeadEvent;

	public readonly string ResistEvent;

	public readonly string ResistWinEvent;

	public readonly string ResistLoseEvent;

	public readonly string PersuadeEvent;

	public readonly List<sbyte> LifeSkillCombatTypes;

	public readonly string PersuadeWinEvent;

	public readonly string PersuadeLoseEvent;

	public readonly string BribeEvent;

	public readonly string BribeConfirmEvent;

	public readonly string SurrenderEvent;

	public readonly string PunishEvent1;

	public readonly string PunishEvent2;

	public readonly string PunishEvent3;

	public readonly string PunishEvent4;

	public TaiwuBeHuntedEventItem(short templateId, int name, int headEvent, int resistEvent, int resistWinEvent, int resistLoseEvent, int persuadeEvent, List<sbyte> lifeSkillCombatTypes, int persuadeWinEvent, int persuadeLoseEvent, int bribeEvent, int bribeConfirmEvent, int surrenderEvent, int punishEvent1, int punishEvent2, int punishEvent3, int punishEvent4)
	{
		TemplateId = templateId;
		Name = LocalStringManager.GetConfig("TaiwuBeHuntedEvent_language", name);
		HeadEvent = LocalStringManager.GetConfig("TaiwuBeHuntedEvent_language", headEvent);
		ResistEvent = LocalStringManager.GetConfig("TaiwuBeHuntedEvent_language", resistEvent);
		ResistWinEvent = LocalStringManager.GetConfig("TaiwuBeHuntedEvent_language", resistWinEvent);
		ResistLoseEvent = LocalStringManager.GetConfig("TaiwuBeHuntedEvent_language", resistLoseEvent);
		PersuadeEvent = LocalStringManager.GetConfig("TaiwuBeHuntedEvent_language", persuadeEvent);
		LifeSkillCombatTypes = lifeSkillCombatTypes;
		PersuadeWinEvent = LocalStringManager.GetConfig("TaiwuBeHuntedEvent_language", persuadeWinEvent);
		PersuadeLoseEvent = LocalStringManager.GetConfig("TaiwuBeHuntedEvent_language", persuadeLoseEvent);
		BribeEvent = LocalStringManager.GetConfig("TaiwuBeHuntedEvent_language", bribeEvent);
		BribeConfirmEvent = LocalStringManager.GetConfig("TaiwuBeHuntedEvent_language", bribeConfirmEvent);
		SurrenderEvent = LocalStringManager.GetConfig("TaiwuBeHuntedEvent_language", surrenderEvent);
		PunishEvent1 = LocalStringManager.GetConfig("TaiwuBeHuntedEvent_language", punishEvent1);
		PunishEvent2 = LocalStringManager.GetConfig("TaiwuBeHuntedEvent_language", punishEvent2);
		PunishEvent3 = LocalStringManager.GetConfig("TaiwuBeHuntedEvent_language", punishEvent3);
		PunishEvent4 = LocalStringManager.GetConfig("TaiwuBeHuntedEvent_language", punishEvent4);
	}

	public TaiwuBeHuntedEventItem()
	{
		TemplateId = 0;
		Name = null;
		HeadEvent = null;
		ResistEvent = null;
		ResistWinEvent = null;
		ResistLoseEvent = null;
		PersuadeEvent = null;
		LifeSkillCombatTypes = null;
		PersuadeWinEvent = null;
		PersuadeLoseEvent = null;
		BribeEvent = null;
		BribeConfirmEvent = null;
		SurrenderEvent = null;
		PunishEvent1 = null;
		PunishEvent2 = null;
		PunishEvent3 = null;
		PunishEvent4 = null;
	}

	public TaiwuBeHuntedEventItem(short templateId, TaiwuBeHuntedEventItem other)
	{
		TemplateId = templateId;
		Name = other.Name;
		HeadEvent = other.HeadEvent;
		ResistEvent = other.ResistEvent;
		ResistWinEvent = other.ResistWinEvent;
		ResistLoseEvent = other.ResistLoseEvent;
		PersuadeEvent = other.PersuadeEvent;
		LifeSkillCombatTypes = other.LifeSkillCombatTypes;
		PersuadeWinEvent = other.PersuadeWinEvent;
		PersuadeLoseEvent = other.PersuadeLoseEvent;
		BribeEvent = other.BribeEvent;
		BribeConfirmEvent = other.BribeConfirmEvent;
		SurrenderEvent = other.SurrenderEvent;
		PunishEvent1 = other.PunishEvent1;
		PunishEvent2 = other.PunishEvent2;
		PunishEvent3 = other.PunishEvent3;
		PunishEvent4 = other.PunishEvent4;
	}

	public override int GetTemplateId()
	{
		return TemplateId;
	}

	public override TaiwuBeHuntedEventItem Duplicate(int templateId)
	{
		return new TaiwuBeHuntedEventItem((short)templateId, this);
	}
}
