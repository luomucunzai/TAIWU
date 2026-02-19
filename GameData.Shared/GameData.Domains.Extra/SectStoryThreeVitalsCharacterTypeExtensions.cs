using System;

namespace GameData.Domains.Extra;

public static class SectStoryThreeVitalsCharacterTypeExtensions
{
	public static short GetVitalTemplateId(this SectStoryThreeVitalsCharacterType type, bool vitalIsDemon)
	{
		return type switch
		{
			SectStoryThreeVitalsCharacterType.Heaven => (short)(vitalIsDemon ? 585 : 913), 
			SectStoryThreeVitalsCharacterType.Earth => (short)(vitalIsDemon ? 586 : 914), 
			SectStoryThreeVitalsCharacterType.Human => (short)(vitalIsDemon ? 587 : 915), 
			_ => throw new ArgumentOutOfRangeException("type", type, null), 
		};
	}
}
