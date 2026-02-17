using System;
using GameData.Domains.Item;
using GameData.Utilities;

namespace GameData.Domains.Character.ParallelModifications
{
	// Token: 0x02000835 RID: 2101
	public class SelectEquipmentsModification
	{
		// Token: 0x06007585 RID: 30085 RVA: 0x0044A661 File Offset: 0x00448861
		public SelectEquipmentsModification(Character character, bool removeUnequippedEquipment)
		{
			this.Character = character;
			this.RemoveUnequippedEquipment = removeUnequippedEquipment;
			this.EquippedSkillsChanged = false;
			this.NeiliAllocationChanged = false;
			this.LoopingNeigongChanged = false;
		}

		// Token: 0x04001FC9 RID: 8137
		public readonly Character Character;

		// Token: 0x04001FCA RID: 8138
		public readonly bool RemoveUnequippedEquipment;

		// Token: 0x04001FCB RID: 8139
		public bool EquippedSkillsChanged;

		// Token: 0x04001FCC RID: 8140
		public CombatSkillEquipment CombatSkillEquipment;

		// Token: 0x04001FCD RID: 8141
		public bool NeiliAllocationChanged;

		// Token: 0x04001FCE RID: 8142
		public NeiliAllocation NeiliAllocation;

		// Token: 0x04001FCF RID: 8143
		public bool LoopingNeigongChanged;

		// Token: 0x04001FD0 RID: 8144
		public short LoopingNeigong;

		// Token: 0x04001FD1 RID: 8145
		public ItemKey[] EquippedItems;

		// Token: 0x04001FD2 RID: 8146
		public bool PersonalNeedChanged;

		// Token: 0x04001FD3 RID: 8147
		public ShortList MasteredCombatSkills;

		// Token: 0x04001FD4 RID: 8148
		public bool MasteredSkillsChanged;

		// Token: 0x04001FD5 RID: 8149
		public byte[] GenericSkillSlotAllocation;
	}
}
