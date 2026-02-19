namespace GameData.Domains.Organization.ParallelModifications;

public readonly struct ParallelSettlementModification
{
	public readonly short Culture;

	public readonly short Safety;

	public readonly int Population;

	public ParallelSettlementModification(short culture, short safety, int population)
	{
		Culture = culture;
		Safety = safety;
		Population = population;
	}
}
