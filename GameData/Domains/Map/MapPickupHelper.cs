using System;
using System.Collections.Generic;
using System.Linq;
using Config;
using Config.ConfigCells;
using GameData.Common;
using GameData.Domains.Character;
using GameData.Domains.Information;
using GameData.Domains.Item;
using GameData.Domains.World;
using GameData.Domains.World.Notification;
using GameData.Utilities;

namespace GameData.Domains.Map
{
	// Token: 0x020008BA RID: 2234
	public static class MapPickupHelper
	{
		// Token: 0x06007BBD RID: 31677 RVA: 0x0048F71A File Offset: 0x0048D91A
		public static IEnumerable<MapPickup> IterVisiblePickups(this MapPickupCollection collection, sbyte xiangshuLevel)
		{
			int num;
			for (int i = 0; i < collection.Count; i = num + 1)
			{
				MapPickup pickup = collection.Get(i);
				bool flag = pickup == null;
				if (!flag)
				{
					bool flag2 = !pickup.IsVisible();
					if (!flag2)
					{
						bool flag3 = pickup.Type == MapPickup.EMapPickupType.Event;
						if (!flag3)
						{
							yield return pickup;
							pickup = null;
						}
					}
				}
				num = i;
			}
			yield break;
		}

		// Token: 0x06007BBE RID: 31678 RVA: 0x0048F734 File Offset: 0x0048D934
		public static BoolArray32 CalcMapPickupBanReason(this MapPickup pickup)
		{
			GameData.Domains.Character.Character taiwuChar = DomainManager.Taiwu.GetTaiwu();
			short taiwuLoopingNeigong = taiwuChar.GetLoopingNeigong();
			ItemKey taiwuReadingBookKey = DomainManager.Taiwu.GetCurReadingBook();
			BoolArray32 banReason = default(BoolArray32);
			MapPickup.EMapPickupType type = pickup.Type;
			MapPickup.EMapPickupType emapPickupType = type;
			if (emapPickupType != MapPickup.EMapPickupType.LoopEffect)
			{
				if (emapPickupType == MapPickup.EMapPickupType.ReadEffect)
				{
					if (!taiwuReadingBookKey.IsValid())
					{
						banReason[1] = true;
					}
				}
			}
			else if (taiwuLoopingNeigong < 0)
			{
				banReason[0] = true;
			}
			return banReason;
		}

		// Token: 0x06007BBF RID: 31679 RVA: 0x0048F7B4 File Offset: 0x0048D9B4
		public static short CalcXiangshuMinionTemplateId(this MapPickup pickup)
		{
			short enemyTemplateId = (short)(298 + (int)pickup.XiangshuLevel);
			bool flag = enemyTemplateId > 306;
			if (flag)
			{
				enemyTemplateId = 306;
			}
			return enemyTemplateId;
		}

		// Token: 0x06007BC0 RID: 31680 RVA: 0x0048F7E8 File Offset: 0x0048D9E8
		public static bool CalcCanAutoBeatXiangshuMinion(this MapPickup pickup)
		{
			short enemyTemplateId = pickup.CalcXiangshuMinionTemplateId();
			CharacterItem characterConfig = Config.Character.Instance[enemyTemplateId];
			sbyte enemyConsummateLevel = characterConfig.ConsummateLevel;
			GameData.Domains.Character.Character taiwuChar = DomainManager.Taiwu.GetTaiwu();
			return taiwuChar.GetConsummateLevel() - 6 >= enemyConsummateLevel;
		}

		// Token: 0x06007BC1 RID: 31681 RVA: 0x0048F830 File Offset: 0x0048DA30
		public static bool CalcNeedBattle(this MapPickup pickup)
		{
			return pickup.HasXiangshuMinion && !pickup.CalcCanAutoBeatXiangshuMinion();
		}

		// Token: 0x06007BC2 RID: 31682 RVA: 0x0048F858 File Offset: 0x0048DA58
		public static void ApplyMinionEscape(this MapPickup pickup, DataContext context, Location location)
		{
			short enemyId = pickup.CalcXiangshuMinionTemplateId();
			bool flag = pickup.CalcCanAutoBeatXiangshuMinion();
			if (flag)
			{
				InstantNotificationCollection collection = DomainManager.World.GetInstantNotificationCollection();
				collection.AddMapPickupsEnemyEscape(location, enemyId);
			}
			List<short> enemyIds = new List<short>
			{
				enemyId
			};
			DomainManager.Combat.GetExpAndAuthorityAndAreaSpiritualDebtOutOfCombat(context, enemyIds, 1);
		}

		// Token: 0x06007BC3 RID: 31683 RVA: 0x0048F8AC File Offset: 0x0048DAAC
		public static bool IsVisible(this MapPickup pickup)
		{
			bool flag = pickup == null;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				bool isEventType = pickup.IsEventType;
				if (isEventType)
				{
					result = false;
				}
				else
				{
					bool flag2 = pickup.State == MapPickup.EMapPickupState.Used;
					if (flag2)
					{
						result = false;
					}
					else
					{
						bool flag3 = !pickup.VisibleByResource;
						if (flag3)
						{
							result = false;
						}
						else
						{
							bool ignored = pickup.Ignored;
							if (ignored)
							{
								result = false;
							}
							else
							{
								MapPickupsItem config = pickup.Template;
								sbyte monthInYear = DomainManager.World.GetCurrMonthInYear();
								bool flag4 = !config.CanShowMonths[(int)monthInYear];
								if (flag4)
								{
									result = false;
								}
								else
								{
									sbyte xiangshuProgress = DomainManager.World.GetXiangshuProgress();
									sbyte xiangshuLevel = GameData.Domains.World.SharedMethods.GetXiangshuLevel(xiangshuProgress);
									bool flag5 = xiangshuLevel < pickup.XiangshuLevel;
									if (flag5)
									{
										result = false;
									}
									else
									{
										bool flag6 = !MapPickupHelper.IsPickupInformationConditionMatched(config);
										if (flag6)
										{
											result = false;
										}
										else
										{
											bool flag7 = !MapPickupHelper.IsPickupOrganizationConditionMatched(config);
											result = !flag7;
										}
									}
								}
							}
						}
					}
				}
			}
			return result;
		}

		// Token: 0x06007BC4 RID: 31684 RVA: 0x0048F99C File Offset: 0x0048DB9C
		private static bool IsPickupInformationConditionMatched(MapPickupsItem config)
		{
			int taiwuCharId = DomainManager.Taiwu.GetTaiwu().GetId();
			NormalInformationCollection collection = DomainManager.Information.GetCharacterNormalInformation(taiwuCharId);
			short informationId = config.ShowConditionInformation;
			bool flag = informationId == -1;
			return flag || (collection != null && collection.GetList().Any((NormalInformation i) => i.TemplateId == informationId));
		}

		// Token: 0x06007BC5 RID: 31685 RVA: 0x0048FA10 File Offset: 0x0048DC10
		private static bool IsPickupOrganizationConditionMatched(MapPickupsItem config)
		{
			OrganizationApproving require = config.ShowConditionOrganizationApproving;
			bool flag = require == null || !require.IsValid;
			return flag || (int)DomainManager.Organization.GetSettlementByOrgTemplateId(require.OrgTemplateId).CalcApprovingRate() >= require.ApprovingValue;
		}

		// Token: 0x06007BC6 RID: 31686 RVA: 0x0048FA60 File Offset: 0x0048DC60
		public static int CompareVisiblePickups(MapPickup x, MapPickup y)
		{
			bool xHasBattle = x.HasXiangshuMinion && !x.CalcCanAutoBeatXiangshuMinion();
			bool yHasBattle = y.HasXiangshuMinion && !y.CalcCanAutoBeatXiangshuMinion();
			bool flag = xHasBattle != yHasBattle;
			int result;
			if (flag)
			{
				result = (xHasBattle ? -1 : 1);
			}
			else
			{
				int xiangshuLevelCompare = y.XiangshuLevel.CompareTo(x.XiangshuLevel);
				bool flag2 = xiangshuLevelCompare != 0;
				if (flag2)
				{
					result = xiangshuLevelCompare;
				}
				else
				{
					MapPickup.EMapPickupType xType = x.Type;
					MapPickup.EMapPickupType yType = y.Type;
					int typeCompare = xType.CompareTo(yType);
					bool flag3 = typeCompare != 0;
					if (flag3)
					{
						result = typeCompare;
					}
					else
					{
						result = x.GetHashCode().CompareTo(y.GetHashCode());
					}
				}
			}
			return result;
		}
	}
}
