using System;
using System.Collections.Generic;
using Config;
using GameData.Domains.Character;
using GameData.Domains.Character.AvatarSystem;

namespace GameData.Domains.Organization
{
	// Token: 0x0200064B RID: 1611
	public class SettlementMembersCreationInfo
	{
		// Token: 0x06004854 RID: 18516 RVA: 0x0028CCC8 File Offset: 0x0028AEC8
		public SettlementMembersCreationInfo(sbyte orgTemplateId, short settlementId, sbyte mapStateTemplateId, short areaId, List<short> blockIds, List<short> nearbyBlockIds)
		{
			this.OrgTemplateId = orgTemplateId;
			this.SettlementId = settlementId;
			this.MapStateTemplateId = mapStateTemplateId;
			this.AreaId = areaId;
			this.BlockIds = blockIds;
			this.NearbyBlockIds = nearbyBlockIds;
			this.CoreMemberConfig = null;
			this.CoreMotherAvatar = null;
			this.CoreFatherAvatar = null;
			this.CoreMotherGenome = default(Genome);
			this.CoreFatherGenome = default(Genome);
			this.CoreCharId = -1;
			this.CoreChar = null;
			this.IsCoreCharInfertile = false;
			this.SpouseCharId = -1;
			this.SpouseChar = null;
			this.CreatedCharIds = new List<int>();
		}

		// Token: 0x06004855 RID: 18517 RVA: 0x0028CD68 File Offset: 0x0028AF68
		public void CompleteCreatingCharacters()
		{
			for (int i = this.CreatedCharIds.Count - 1; i >= 0; i--)
			{
				int charId = this.CreatedCharIds[i];
				DomainManager.Character.CompleteCreatingCharacter(charId);
			}
			this.Reset();
		}

		// Token: 0x06004856 RID: 18518 RVA: 0x0028CDB8 File Offset: 0x0028AFB8
		private void Reset()
		{
			this.CoreMotherAvatar = null;
			this.CoreFatherAvatar = null;
			this.CoreMotherGenome = default(Genome);
			this.CoreFatherGenome = default(Genome);
			this.CoreCharId = -1;
			this.CoreChar = null;
			this.IsCoreCharInfertile = false;
			this.SpouseCharId = -1;
			this.SpouseChar = null;
			this.CreatedCharIds.Clear();
		}

		// Token: 0x0400151A RID: 5402
		public readonly sbyte OrgTemplateId;

		// Token: 0x0400151B RID: 5403
		public readonly short SettlementId;

		// Token: 0x0400151C RID: 5404
		public readonly sbyte MapStateTemplateId;

		// Token: 0x0400151D RID: 5405
		public readonly short AreaId;

		// Token: 0x0400151E RID: 5406
		public readonly List<short> BlockIds;

		// Token: 0x0400151F RID: 5407
		public readonly List<short> NearbyBlockIds;

		// Token: 0x04001520 RID: 5408
		public OrganizationMemberItem CoreMemberConfig;

		// Token: 0x04001521 RID: 5409
		public AvatarData CoreMotherAvatar;

		// Token: 0x04001522 RID: 5410
		public AvatarData CoreFatherAvatar;

		// Token: 0x04001523 RID: 5411
		public Genome CoreMotherGenome;

		// Token: 0x04001524 RID: 5412
		public Genome CoreFatherGenome;

		// Token: 0x04001525 RID: 5413
		public int CoreCharId;

		// Token: 0x04001526 RID: 5414
		public GameData.Domains.Character.Character CoreChar;

		// Token: 0x04001527 RID: 5415
		public bool IsCoreCharInfertile;

		// Token: 0x04001528 RID: 5416
		public int SpouseCharId;

		// Token: 0x04001529 RID: 5417
		public GameData.Domains.Character.Character SpouseChar;

		// Token: 0x0400152A RID: 5418
		public readonly List<int> CreatedCharIds;
	}
}
