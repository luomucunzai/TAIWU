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

namespace GameData.Domains.Map;

public static class MapPickupHelper
{
	public static IEnumerable<MapPickup> IterVisiblePickups(this MapPickupCollection collection, sbyte xiangshuLevel)
	{
		for (int i = 0; i < collection.Count; i++)
		{
			MapPickup pickup = collection.Get(i);
			if (pickup != null && pickup.IsVisible() && pickup.Type != MapPickup.EMapPickupType.Event)
			{
				yield return pickup;
			}
		}
	}

	public static BoolArray32 CalcMapPickupBanReason(this MapPickup pickup)
	{
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
		short loopingNeigong = taiwu.GetLoopingNeigong();
		ItemKey curReadingBook = DomainManager.Taiwu.GetCurReadingBook();
		BoolArray32 result = default(BoolArray32);
		switch (pickup.Type)
		{
		case MapPickup.EMapPickupType.LoopEffect:
			if (loopingNeigong < 0)
			{
				((BoolArray32)(ref result))[0] = true;
			}
			break;
		case MapPickup.EMapPickupType.ReadEffect:
			if (!curReadingBook.IsValid())
			{
				((BoolArray32)(ref result))[1] = true;
			}
			break;
		}
		return result;
	}

	public static short CalcXiangshuMinionTemplateId(this MapPickup pickup)
	{
		short num = (short)(298 + pickup.XiangshuLevel);
		if (num > 306)
		{
			num = 306;
		}
		return num;
	}

	public static bool CalcCanAutoBeatXiangshuMinion(this MapPickup pickup)
	{
		short index = pickup.CalcXiangshuMinionTemplateId();
		CharacterItem characterItem = Config.Character.Instance[index];
		sbyte consummateLevel = characterItem.ConsummateLevel;
		GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
		return taiwu.GetConsummateLevel() - 6 >= consummateLevel;
	}

	public static bool CalcNeedBattle(this MapPickup pickup)
	{
		return pickup.HasXiangshuMinion && !pickup.CalcCanAutoBeatXiangshuMinion();
	}

	public static void ApplyMinionEscape(this MapPickup pickup, DataContext context, Location location)
	{
		short num = pickup.CalcXiangshuMinionTemplateId();
		if (pickup.CalcCanAutoBeatXiangshuMinion())
		{
			InstantNotificationCollection instantNotificationCollection = DomainManager.World.GetInstantNotificationCollection();
			instantNotificationCollection.AddMapPickupsEnemyEscape(location, num);
		}
		List<short> enemyTemplateIdList = new List<short> { num };
		DomainManager.Combat.GetExpAndAuthorityAndAreaSpiritualDebtOutOfCombat(context, enemyTemplateIdList);
	}

	public static bool IsVisible(this MapPickup pickup)
	{
		if (pickup == null)
		{
			return false;
		}
		if (pickup.IsEventType)
		{
			return false;
		}
		if (pickup.State == MapPickup.EMapPickupState.Used)
		{
			return false;
		}
		if (!pickup.VisibleByResource)
		{
			return false;
		}
		if (pickup.Ignored)
		{
			return false;
		}
		MapPickupsItem template = pickup.Template;
		sbyte currMonthInYear = DomainManager.World.GetCurrMonthInYear();
		if (!template.CanShowMonths[currMonthInYear])
		{
			return false;
		}
		sbyte xiangshuProgress = DomainManager.World.GetXiangshuProgress();
		sbyte xiangshuLevel = GameData.Domains.World.SharedMethods.GetXiangshuLevel(xiangshuProgress);
		if (xiangshuLevel < pickup.XiangshuLevel)
		{
			return false;
		}
		if (!IsPickupInformationConditionMatched(template))
		{
			return false;
		}
		if (!IsPickupOrganizationConditionMatched(template))
		{
			return false;
		}
		return true;
	}

	private static bool IsPickupInformationConditionMatched(MapPickupsItem config)
	{
		int id = DomainManager.Taiwu.GetTaiwu().GetId();
		NormalInformationCollection characterNormalInformation = DomainManager.Information.GetCharacterNormalInformation(id);
		short informationId = config.ShowConditionInformation;
		if (informationId == -1)
		{
			return true;
		}
		return characterNormalInformation?.GetList().Any((NormalInformation i) => i.TemplateId == informationId) ?? false;
	}

	private static bool IsPickupOrganizationConditionMatched(MapPickupsItem config)
	{
		OrganizationApproving showConditionOrganizationApproving = config.ShowConditionOrganizationApproving;
		if (showConditionOrganizationApproving == null || !showConditionOrganizationApproving.IsValid)
		{
			return true;
		}
		return DomainManager.Organization.GetSettlementByOrgTemplateId(showConditionOrganizationApproving.OrgTemplateId).CalcApprovingRate() >= showConditionOrganizationApproving.ApprovingValue;
	}

	public static int CompareVisiblePickups(MapPickup x, MapPickup y)
	{
		bool flag = x.HasXiangshuMinion && !x.CalcCanAutoBeatXiangshuMinion();
		bool flag2 = y.HasXiangshuMinion && !y.CalcCanAutoBeatXiangshuMinion();
		if (flag != flag2)
		{
			return (!flag) ? 1 : (-1);
		}
		int num = y.XiangshuLevel.CompareTo(x.XiangshuLevel);
		if (num != 0)
		{
			return num;
		}
		MapPickup.EMapPickupType type = x.Type;
		MapPickup.EMapPickupType type2 = y.Type;
		int num2 = type.CompareTo(type2);
		if (num2 != 0)
		{
			return num2;
		}
		return x.GetHashCode().CompareTo(y.GetHashCode());
	}
}
