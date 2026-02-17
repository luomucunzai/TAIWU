using System;
using GameData.Common;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.Animal.Loong.Neigong
{
	// Token: 0x020005EC RID: 1516
	public class LoongEarthImplementSilencePower : LoongEarthImplementSilence, ILoongEarthExtra
	{
		// Token: 0x060044A3 RID: 17571 RVA: 0x00270174 File Offset: 0x0026E374
		public void OnSilenced(DataContext context, CombatCharacter combatChar, short skillId)
		{
			DomainManager.Combat.ReduceSkillPowerInCombat(context, new CombatSkillKey(combatChar.GetId(), skillId), base.EffectBase.EffectKey, -30);
		}

		// Token: 0x0400144F RID: 5199
		private const int ReducePower = -30;
	}
}
