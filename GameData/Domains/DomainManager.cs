using System;
using GameData.Common;
using GameData.Domains.Adventure;
using GameData.Domains.Building;
using GameData.Domains.Character;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.Extra;
using GameData.Domains.Global;
using GameData.Domains.Information;
using GameData.Domains.Item;
using GameData.Domains.LegendaryBook;
using GameData.Domains.LifeRecord;
using GameData.Domains.Map;
using GameData.Domains.Merchant;
using GameData.Domains.Mod;
using GameData.Domains.Organization;
using GameData.Domains.SpecialEffect;
using GameData.Domains.Taiwu;
using GameData.Domains.TaiwuEvent;
using GameData.Domains.TutorialChapter;
using GameData.Domains.World;

namespace GameData.Domains
{
	// Token: 0x02000021 RID: 33
	public static class DomainManager
	{
		// Token: 0x06000D17 RID: 3351 RVA: 0x000DE260 File Offset: 0x000DC460
		public static BaseGameDataDomain[] GetArchiveAttachedDomains()
		{
			DomainManager.ArchiveAttachedDomains[0] = DomainManager.World;
			DomainManager.ArchiveAttachedDomains[1] = DomainManager.Map;
			DomainManager.ArchiveAttachedDomains[2] = DomainManager.Organization;
			DomainManager.ArchiveAttachedDomains[3] = DomainManager.Character;
			DomainManager.ArchiveAttachedDomains[4] = DomainManager.Taiwu;
			DomainManager.ArchiveAttachedDomains[5] = DomainManager.Item;
			DomainManager.ArchiveAttachedDomains[6] = DomainManager.CombatSkill;
			DomainManager.ArchiveAttachedDomains[7] = DomainManager.Combat;
			DomainManager.ArchiveAttachedDomains[8] = DomainManager.Building;
			DomainManager.ArchiveAttachedDomains[9] = DomainManager.Adventure;
			DomainManager.ArchiveAttachedDomains[10] = DomainManager.LegendaryBook;
			DomainManager.ArchiveAttachedDomains[11] = DomainManager.TaiwuEvent;
			DomainManager.ArchiveAttachedDomains[12] = DomainManager.LifeRecord;
			DomainManager.ArchiveAttachedDomains[13] = DomainManager.Merchant;
			DomainManager.ArchiveAttachedDomains[14] = DomainManager.TutorialChapter;
			DomainManager.ArchiveAttachedDomains[15] = DomainManager.Mod;
			DomainManager.ArchiveAttachedDomains[16] = DomainManager.SpecialEffect;
			DomainManager.ArchiveAttachedDomains[17] = DomainManager.Information;
			DomainManager.ArchiveAttachedDomains[18] = DomainManager.Extra;
			return DomainManager.ArchiveAttachedDomains;
		}

		// Token: 0x06000D18 RID: 3352 RVA: 0x000DE368 File Offset: 0x000DC568
		public static void ResetArchiveAttachedDomains()
		{
			DomainManager.World = new WorldDomain();
			DomainManager.Map = new MapDomain();
			DomainManager.Organization = new OrganizationDomain();
			DomainManager.Character = new CharacterDomain();
			DomainManager.Taiwu = new TaiwuDomain();
			DomainManager.Item = new ItemDomain();
			DomainManager.CombatSkill = new CombatSkillDomain();
			DomainManager.Combat = new CombatDomain();
			DomainManager.Building = new BuildingDomain();
			DomainManager.Adventure = new AdventureDomain();
			DomainManager.LegendaryBook = new LegendaryBookDomain();
			DomainManager.TaiwuEvent = new TaiwuEventDomain();
			DomainManager.LifeRecord = new LifeRecordDomain();
			DomainManager.Merchant = new MerchantDomain();
			DomainManager.TutorialChapter = new TutorialChapterDomain();
			DomainManager.Mod = new ModDomain();
			DomainManager.SpecialEffect = new SpecialEffectDomain();
			DomainManager.Information = new InformationDomain();
			DomainManager.Extra = new ExtraDomain();
			DomainManager.Domains[1] = DomainManager.World;
			DomainManager.Domains[2] = DomainManager.Map;
			DomainManager.Domains[3] = DomainManager.Organization;
			DomainManager.Domains[4] = DomainManager.Character;
			DomainManager.Domains[5] = DomainManager.Taiwu;
			DomainManager.Domains[6] = DomainManager.Item;
			DomainManager.Domains[7] = DomainManager.CombatSkill;
			DomainManager.Domains[8] = DomainManager.Combat;
			DomainManager.Domains[9] = DomainManager.Building;
			DomainManager.Domains[10] = DomainManager.Adventure;
			DomainManager.Domains[11] = DomainManager.LegendaryBook;
			DomainManager.Domains[12] = DomainManager.TaiwuEvent;
			DomainManager.Domains[13] = DomainManager.LifeRecord;
			DomainManager.Domains[14] = DomainManager.Merchant;
			DomainManager.Domains[15] = DomainManager.TutorialChapter;
			DomainManager.Domains[16] = DomainManager.Mod;
			DomainManager.Domains[17] = DomainManager.SpecialEffect;
			DomainManager.Domains[18] = DomainManager.Information;
			DomainManager.Domains[19] = DomainManager.Extra;
		}

		// Token: 0x040000A8 RID: 168
		public static readonly GlobalDomain Global = new GlobalDomain();

		// Token: 0x040000A9 RID: 169
		public static WorldDomain World = new WorldDomain();

		// Token: 0x040000AA RID: 170
		public static MapDomain Map = new MapDomain();

		// Token: 0x040000AB RID: 171
		public static OrganizationDomain Organization = new OrganizationDomain();

		// Token: 0x040000AC RID: 172
		public static CharacterDomain Character = new CharacterDomain();

		// Token: 0x040000AD RID: 173
		public static TaiwuDomain Taiwu = new TaiwuDomain();

		// Token: 0x040000AE RID: 174
		public static ItemDomain Item = new ItemDomain();

		// Token: 0x040000AF RID: 175
		public static CombatSkillDomain CombatSkill = new CombatSkillDomain();

		// Token: 0x040000B0 RID: 176
		public static CombatDomain Combat = new CombatDomain();

		// Token: 0x040000B1 RID: 177
		public static BuildingDomain Building = new BuildingDomain();

		// Token: 0x040000B2 RID: 178
		public static AdventureDomain Adventure = new AdventureDomain();

		// Token: 0x040000B3 RID: 179
		public static LegendaryBookDomain LegendaryBook = new LegendaryBookDomain();

		// Token: 0x040000B4 RID: 180
		public static TaiwuEventDomain TaiwuEvent = new TaiwuEventDomain();

		// Token: 0x040000B5 RID: 181
		public static LifeRecordDomain LifeRecord = new LifeRecordDomain();

		// Token: 0x040000B6 RID: 182
		public static MerchantDomain Merchant = new MerchantDomain();

		// Token: 0x040000B7 RID: 183
		public static TutorialChapterDomain TutorialChapter = new TutorialChapterDomain();

		// Token: 0x040000B8 RID: 184
		public static ModDomain Mod = new ModDomain();

		// Token: 0x040000B9 RID: 185
		public static SpecialEffectDomain SpecialEffect = new SpecialEffectDomain();

		// Token: 0x040000BA RID: 186
		public static InformationDomain Information = new InformationDomain();

		// Token: 0x040000BB RID: 187
		public static ExtraDomain Extra = new ExtraDomain();

		// Token: 0x040000BC RID: 188
		public static readonly BaseGameDataDomain[] Domains = new BaseGameDataDomain[]
		{
			DomainManager.Global,
			DomainManager.World,
			DomainManager.Map,
			DomainManager.Organization,
			DomainManager.Character,
			DomainManager.Taiwu,
			DomainManager.Item,
			DomainManager.CombatSkill,
			DomainManager.Combat,
			DomainManager.Building,
			DomainManager.Adventure,
			DomainManager.LegendaryBook,
			DomainManager.TaiwuEvent,
			DomainManager.LifeRecord,
			DomainManager.Merchant,
			DomainManager.TutorialChapter,
			DomainManager.Mod,
			DomainManager.SpecialEffect,
			DomainManager.Information,
			DomainManager.Extra
		};

		// Token: 0x040000BD RID: 189
		public static readonly ushort[] ArchiveAttachedDomainIds = new ushort[]
		{
			1,
			2,
			3,
			4,
			5,
			6,
			7,
			8,
			9,
			10,
			11,
			12,
			13,
			14,
			15,
			16,
			17,
			18,
			19
		};

		// Token: 0x040000BE RID: 190
		private static readonly BaseGameDataDomain[] ArchiveAttachedDomains = new BaseGameDataDomain[19];
	}
}
