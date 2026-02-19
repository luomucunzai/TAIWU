namespace GameData.Domains.SpecialEffect;

public static class SpecialEffectUtils
{
	public static bool HasAzureMarrowMakeLoveEffect(int charId)
	{
		return DomainManager.SpecialEffect.ModifyData(charId, -1, 299, dataValue: false);
	}
}
