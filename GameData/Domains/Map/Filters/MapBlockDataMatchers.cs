using System;
using GameData.Domains.Adventure;

namespace GameData.Domains.Map.Filters
{
	// Token: 0x020008BD RID: 2237
	public static class MapBlockDataMatchers
	{
		// Token: 0x06007BCA RID: 31690 RVA: 0x0048FC04 File Offset: 0x0048DE04
		public static bool IsNormalOrWild(MapBlockData blockData)
		{
			EMapBlockType blockType = blockData.BlockType;
			return blockType == EMapBlockType.Normal || blockType == EMapBlockType.Wild;
		}

		// Token: 0x06007BCB RID: 31691 RVA: 0x0048FC28 File Offset: 0x0048DE28
		public static bool HasNoAdventure(MapBlockData blockData)
		{
			AreaAdventureData adventureArea = DomainManager.Adventure.GetAdventuresInArea(blockData.AreaId);
			return !adventureArea.AdventureSites.ContainsKey(blockData.BlockId);
		}

		// Token: 0x06007BCC RID: 31692 RVA: 0x0048FC60 File Offset: 0x0048DE60
		public static bool HasNoIntelligentCharacter(MapBlockData blockData)
		{
			return blockData.CharacterSet == null && blockData.InfectedCharacterSet == null;
		}

		// Token: 0x06007BCD RID: 31693 RVA: 0x0048FC88 File Offset: 0x0048DE88
		public static bool IsValidForRandomEnemy(MapBlockData blockData)
		{
			return blockData.BlockSubType != EMapBlockSubType.DLCLoong;
		}

		// Token: 0x06007BCE RID: 31694 RVA: 0x0048FCA8 File Offset: 0x0048DEA8
		public static bool IsValidForJuniorXiangshu(MapBlockData blockData)
		{
			return blockData.BlockSubType != EMapBlockSubType.DLCLoong;
		}

		// Token: 0x06007BCF RID: 31695 RVA: 0x0048FCC8 File Offset: 0x0048DEC8
		public static bool CanBeRuinedByXiangshu(MapBlockData blockData)
		{
			return blockData.TemplateId != 38 && !blockData.IsCityTown() && blockData.BlockSubType != EMapBlockSubType.DLCLoong;
		}
	}
}
