using System;
using System.Collections.Generic;
using Config.ConfigCells.Character;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.Character.ParallelModifications
{
	// Token: 0x02000827 RID: 2087
	public class CreateIntelligentCharacterModification
	{
		// Token: 0x04001F56 RID: 8022
		public Character Self;

		// Token: 0x04001F57 RID: 8023
		public int FatherCharId;

		// Token: 0x04001F58 RID: 8024
		public int MotherCharId;

		// Token: 0x04001F59 RID: 8025
		public bool CreateFatherRelation;

		// Token: 0x04001F5A RID: 8026
		public bool CreateMotherRelation;

		// Token: 0x04001F5B RID: 8027
		public Character Father;

		// Token: 0x04001F5C RID: 8028
		public sbyte FathersLegitimateBoysCount;

		// Token: 0x04001F5D RID: 8029
		public PresetEquipmentItem[] Equipment;

		// Token: 0x04001F5E RID: 8030
		public List<PresetInventoryItem> Inventory;

		// Token: 0x04001F5F RID: 8031
		public List<CombatSkill> CombatSkills;

		// Token: 0x04001F60 RID: 8032
		public List<InventoryCombatSkillBookParams> InventorySkillBooks;

		// Token: 0x04001F61 RID: 8033
		public HashSet<short> SkillWeaponIds;

		// Token: 0x04001F62 RID: 8034
		public int ReincarnationCharId;

		// Token: 0x04001F63 RID: 8035
		public sbyte GrowingSectGrade;

		// Token: 0x04001F64 RID: 8036
		public bool DisableBeReincarnatedBySavedSoul;
	}
}
