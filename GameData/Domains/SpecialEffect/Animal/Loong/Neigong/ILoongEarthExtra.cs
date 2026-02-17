using System;
using GameData.Common;
using GameData.Domains.Combat;

namespace GameData.Domains.SpecialEffect.Animal.Loong.Neigong
{
	// Token: 0x020005EB RID: 1515
	public interface ILoongEarthExtra
	{
		// Token: 0x060044A2 RID: 17570
		void OnSilenced(DataContext context, CombatCharacter combatChar, short skillId);
	}
}
