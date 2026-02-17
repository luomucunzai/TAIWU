using System;
using System.Collections.Generic;
using GameData.Common;
using GameData.Domains.Taiwu.Profession;

namespace GameData.Domains.Combat.Profession
{
	// Token: 0x0200070A RID: 1802
	public struct ExpAddProfessionSeniorityData
	{
		// Token: 0x06006820 RID: 26656 RVA: 0x003B3B27 File Offset: 0x003B1D27
		public ExpAddProfessionSeniorityData(int professionId, int formulaId, sbyte requireCombatType = -1, bool requireIntelligent = false, sbyte requireOrganization = -1)
		{
			this.ProfessionId = professionId;
			this.FormulaId = formulaId;
			this.RequireCombatType = requireCombatType;
			this.RequireIntelligent = requireIntelligent;
			this.RequireOrganization = requireOrganization;
		}

		// Token: 0x06006821 RID: 26657 RVA: 0x003B3B50 File Offset: 0x003B1D50
		public void DoAddSeniority(DataContext context, int exp)
		{
			int addSeniority = ProfessionFormulaImpl.Calculate(this.FormulaId, exp);
			DomainManager.Extra.ChangeProfessionSeniority(context, this.ProfessionId, addSeniority, true, false);
		}

		// Token: 0x04001C7C RID: 7292
		public static readonly List<ExpAddProfessionSeniorityData> AllData = new List<ExpAddProfessionSeniorityData>
		{
			new ExpAddProfessionSeniorityData(3, 23, 0, true, -1),
			new ExpAddProfessionSeniorityData(3, 24, 1, true, -1),
			new ExpAddProfessionSeniorityData(3, 25, -1, false, 17),
			new ExpAddProfessionSeniorityData(3, 25, -1, false, 18),
			new ExpAddProfessionSeniorityData(5, 40, -1, false, 19)
		};

		// Token: 0x04001C7D RID: 7293
		public int ProfessionId;

		// Token: 0x04001C7E RID: 7294
		public int FormulaId;

		// Token: 0x04001C7F RID: 7295
		public sbyte RequireCombatType;

		// Token: 0x04001C80 RID: 7296
		public bool RequireIntelligent;

		// Token: 0x04001C81 RID: 7297
		public sbyte RequireOrganization;
	}
}
