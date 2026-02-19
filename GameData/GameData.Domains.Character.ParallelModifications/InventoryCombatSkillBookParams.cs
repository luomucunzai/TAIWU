namespace GameData.Domains.Character.ParallelModifications;

public readonly struct InventoryCombatSkillBookParams
{
	public readonly short TemplateId;

	public readonly byte PageTypes;

	public InventoryCombatSkillBookParams(short templateId, byte pageTypes)
	{
		TemplateId = templateId;
		PageTypes = pageTypes;
	}
}
