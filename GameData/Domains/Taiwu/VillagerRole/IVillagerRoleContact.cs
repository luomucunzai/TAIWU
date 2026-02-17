using System;
using System.Collections.Generic;
using GameData.Common;
using GameData.Domains.Character;
using Redzen.Random;

namespace GameData.Domains.Taiwu.VillagerRole
{
	// Token: 0x02000048 RID: 72
	public interface IVillagerRoleContact
	{
		// Token: 0x17000029 RID: 41
		// (get) Token: 0x06001371 RID: 4977
		bool IncreaseFavorability { get; }

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x06001372 RID: 4978
		bool HasChickenUpgradeEffect { get; }

		// Token: 0x06001373 RID: 4979 RVA: 0x001389D4 File Offset: 0x00136BD4
		void ApplyContactAction(DataContext context, List<Character> targets)
		{
			Character taiwuChar = DomainManager.Taiwu.GetTaiwu();
			int favorChange = this.ContactFavorabilityChange;
			bool flag = !this.IncreaseFavorability;
			if (flag)
			{
				favorChange = -favorChange;
			}
			foreach (Character targetChar in targets)
			{
				DomainManager.Character.ChangeFavorabilityOptionalMonthlyEvolution(context, targetChar, taiwuChar, favorChange);
			}
		}

		// Token: 0x06001374 RID: 4980
		void SelectContactTargets(IRandomSource random, List<Character> selectedCharList, int selectAmount);

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x06001375 RID: 4981
		int ContactFavorabilityChange { get; }

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x06001376 RID: 4982
		int ContactCharacterAmount { get; }
	}
}
