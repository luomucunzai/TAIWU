using System;
using System.Collections.Generic;
using System.Linq;
using Config;
using Config.ConfigCells;
using GameData.ArchiveData;
using GameData.Common;
using GameData.DLC.FiveLoong;
using GameData.Dependencies;
using GameData.Domains.Building;
using GameData.Domains.Character;
using GameData.Domains.Character.Ai;
using GameData.Domains.Character.Relation;
using GameData.Domains.Extra;
using GameData.Domains.Item;
using GameData.Domains.Item.Display;
using GameData.Domains.Map;
using GameData.Domains.Organization;
using GameData.Domains.Organization.Display;
using GameData.Domains.Taiwu.Profession;
using GameData.Domains.Taiwu.VillagerRole;
using GameData.Domains.TaiwuEvent;
using GameData.Domains.TaiwuEvent.EventHelper;
using GameData.Domains.World.Notification;
using GameData.GameDataBridge;
using GameData.Serializer;
using GameData.Utilities;
using NLog;

namespace GameData.Domains.Merchant;

[GameDataDomain(14)]
public class MerchantDomain : BaseGameDataDomain
{
	private class SkillBookTradeInfo
	{
		public int CharId;

		public List<ItemKey> PrivateSkillBooks;

		public List<ItemKey> SectSkillBooks;

		public List<ItemKey> BoughtBooksFromTaiwu;

		public List<ItemKey> SoldBooksToTaiwu;

		public SkillBookTradeInfo(int charId)
		{
			CharId = charId;
			PrivateSkillBooks = new List<ItemKey>();
			SectSkillBooks = new List<ItemKey>();
			BoughtBooksFromTaiwu = new List<ItemKey>();
			SoldBooksToTaiwu = new List<ItemKey>();
		}
	}

	private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

	[DomainData(DomainDataType.SingleValueCollection, true, false, false, false)]
	private readonly Dictionary<int, MerchantData> _merchantData;

	[DomainData(DomainDataType.SingleValue, true, false, true, true, ArrayElementsCount = 7)]
	private int[] _merchantFavorability;

	[DomainData(DomainDataType.SingleValue, true, false, true, true, ArrayElementsCount = 7)]
	private int[] _merchantMoney;

	[DomainData(DomainDataType.ElementList, true, false, true, true, ArrayElementsCount = 7)]
	private MerchantData[] _merchantMaxLevelData;

	public const sbyte MaxLevel = 6;

	public const int MaxFavorability = 100;

	private SkillBookTradeInfo _skillBookTradeInfo;

	private readonly Dictionary<int, MerchantBuyBackData> _merchantBuyBackData = new Dictionary<int, MerchantBuyBackData>();

	private readonly Dictionary<int, MerchantBuyBackData> _caravanBuyBackData = new Dictionary<int, MerchantBuyBackData>();

	private readonly MerchantBuyBackData[] _merchantMaxLevelBuyBackData = new MerchantBuyBackData[7];

	private readonly MerchantBuyBackData[] _branchMerchantBuyBackData = new MerchantBuyBackData[7];

	private MerchantBuyBackData _tempMerchantBuyBackData;

	private MerchantBuyBackData _sectStorySpecialMerchantBuyBackData;

	[DomainData(DomainDataType.SingleValue, true, false, false, false)]
	private int _nextCaravanId;

	[DomainData(DomainDataType.SingleValueCollection, true, false, false, false)]
	private readonly Dictionary<int, MerchantData> _caravanData;

	[DomainData(DomainDataType.SingleValueCollection, true, false, false, false)]
	private readonly Dictionary<int, CaravanPath> _caravanDict;

	private const short CaravanStayDaysInTaiwuVillage = 90;

	private MerchantData _tempMerchantData;

	private int _totalBuyMoney;

	public const int TempCaravanId = -1;

	private static readonly DataInfluence[][] CacheInfluences = new DataInfluence[7][];

	private static readonly DataInfluence[][] CacheInfluencesMerchantMaxLevelData = new DataInfluence[7][];

	private readonly byte[] _dataStatesMerchantMaxLevelData = new byte[2];

	private Queue<uint> _pendingLoadingOperationIds;

	private void OnInitializedDomainData()
	{
	}

	private void InitializeOnInitializeGameDataModule()
	{
	}

	private void InitializeOnEnterNewWorld()
	{
		DataContext currentThreadDataContext = DataContextManager.GetCurrentThreadDataContext();
		InitMerchantMoney(currentThreadDataContext);
	}

	private void OnLoadedArchiveData()
	{
	}

	public override void FixAbnormalDomainArchiveData(DataContext context)
	{
		if (DomainManager.World.IsCurrWorldBeforeVersion(0, 0, 69, 71))
		{
			FixNonExistingMerchantGoods(context);
		}
		if ((object)DomainManager.World.GetCurrWorldGameVersion() == null || DomainManager.World.IsCurrWorldBeforeVersion(0, 0, 74, 32))
		{
			ReCalcCaravanPath(context);
		}
		if ((object)DomainManager.World.GetCurrWorldGameVersion() == null || DomainManager.World.IsCurrWorldBeforeVersion(0, 0, 77))
		{
			InitMerchantBuyBackData(context);
		}
	}

	private void FixNonExistingMerchantGoods(DataContext context)
	{
		List<ItemKey> itemsToRemove = new List<ItemKey>();
		List<int> list = new List<int>();
		int key;
		MerchantData value;
		foreach (KeyValuePair<int, MerchantData> merchantDatum in _merchantData)
		{
			merchantDatum.Deconstruct(out key, out value);
			int item = key;
			MerchantData merchantData = value;
			if (FixAbnormalMerchantData(merchantData))
			{
				list.Add(item);
			}
		}
		foreach (int item3 in list)
		{
			SetElement_MerchantData(item3, _merchantData[item3], context);
		}
		for (int i = 0; i < _merchantMaxLevelData.Length; i++)
		{
			MerchantData value2 = _merchantMaxLevelData[i];
			if (FixAbnormalMerchantData(_merchantMaxLevelData[i]))
			{
				SetElement_MerchantMaxLevelData(i, value2, context);
			}
		}
		list.Clear();
		foreach (KeyValuePair<int, MerchantData> caravanDatum in _caravanData)
		{
			caravanDatum.Deconstruct(out key, out value);
			int item2 = key;
			MerchantData merchantData2 = value;
			if (FixAbnormalMerchantData(merchantData2))
			{
				list.Add(item2);
			}
		}
		foreach (int item4 in list)
		{
			SetElement_CaravanData(item4, _caravanData[item4], context);
		}
		bool FixAbnormalMerchantData(MerchantData merchantData3)
		{
			if (merchantData3 == null || merchantData3.BuyInGoodsList == null)
			{
				return false;
			}
			itemsToRemove.Clear();
			foreach (var (itemKey2, num2) in merchantData3.BuyInGoodsList.Items)
			{
				if (!DomainManager.Item.ItemExists(itemKey2))
				{
					itemsToRemove.Add(itemKey2);
					Logger.Warn($"Removing non-existing item {itemKey2} from merchant.");
				}
				else if (ItemType.IsEquipmentItemType(itemKey2.ItemType))
				{
					EquipmentBase baseEquipment = DomainManager.Item.GetBaseEquipment(itemKey2);
					int equippedCharId = baseEquipment.GetEquippedCharId();
					if (baseEquipment.GetEquippedCharId() >= 0 || baseEquipment.PrevOwner.OwnerType == ItemOwnerType.CharacterInventory || baseEquipment.PrevOwner.OwnerType == ItemOwnerType.CharacterEquipment)
					{
						itemsToRemove.Add(itemKey2);
						Logger.Warn($"Removing already item {itemKey2} from merchant which is already equipped by {equippedCharId}");
					}
				}
			}
			if (itemsToRemove.Count <= 0)
			{
				return false;
			}
			foreach (ItemKey item5 in itemsToRemove)
			{
				merchantData3.BuyInGoodsList.Items.Remove(item5);
			}
			return true;
		}
	}

	public void SetMerchantData(int charId, MerchantData merchantData, DataContext context)
	{
		SetElement_MerchantData(charId, merchantData, context);
	}

	[DomainMethod]
	public MerchantData GetMerchantData(DataContext context, int charId)
	{
		if (!_merchantData.TryGetValue(charId, out var value))
		{
			value = CreateMerchantData(context, charId);
		}
		else
		{
			DomainManager.Extra.TryGetMerchantCharToType(charId, out var type);
			sbyte merchantType = value.MerchantType;
			if ((uint)(merchantType - 7) <= 1u)
			{
				type = value.MerchantType;
			}
			if (value.MerchantType != type)
			{
				RemoveMerchantData(context, charId);
				value = CreateMerchantData(context, charId);
			}
			else
			{
				value.Money = GetMerchantMoney(context, type);
			}
			DomainManager.Extra.TryGetMerchantExtraGoods(charId, out var data);
			sbyte b = data?.SeasonTemplateId ?? (-1);
			if (b != EventHelper.GetCurrSeason())
			{
				value.RefreshSeasonExtraGoods(context, charId);
				SetMerchantData(charId, value, context);
			}
		}
		return value;
	}

	public int GetMerchantMoney(DataContext context, sbyte merchantType)
	{
		if (!_merchantMoney.CheckIndex(merchantType))
		{
			return 0;
		}
		int num = _merchantMoney[merchantType];
		if (num < 0)
		{
			num = 0;
			SetMerchantMoney(context, merchantType, num);
		}
		return num;
	}

	public int SetMerchantMoney(DataContext context, sbyte merchantType, int money)
	{
		if (!_merchantMoney.CheckIndex(merchantType))
		{
			return 0;
		}
		_merchantMoney[merchantType] = money;
		SetMerchantMoney(_merchantMoney, context);
		return money;
	}

	[DomainMethod]
	public void SettleTrade(DataContext context, MerchantTradeArguments merchantTradeArguments)
	{
		Dictionary<ItemKey, int> tradeMoneySources = merchantTradeArguments.TradeMoneySources;
		sbyte buildingMerchantType = merchantTradeArguments.OpenShopEventArguments.BuildingMerchantType;
		int buildingMerchantCaravanId = SharedMethods.GetBuildingMerchantCaravanId(buildingMerchantType, merchantTradeArguments.OpenShopEventArguments.IsHeadBuildingMerchant);
		int buyMoney = merchantTradeArguments.BuyMoney;
		List<ItemSourceChange> itemChangeList = merchantTradeArguments.ItemChangeList;
		GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
		int id = merchantTradeArguments.OpenShopEventArguments.Id;
		MerchantData merchantData = merchantTradeArguments.MerchantData;
		MerchantExtraGoodsData merchantExtraGoods = DomainManager.Extra.GetMerchantExtraGoods(id);
		bool isDebtAreaShop = merchantData.MerchantTemplateId == 53;
		foreach (ItemSourceChange item2 in itemChangeList)
		{
			foreach (ItemKeyAndCount item3 in item2.Items)
			{
				item3.Deconstruct(out var itemKey, out var count);
				ItemKey itemKey2 = itemKey;
				int num = count;
				ItemBase baseItem = DomainManager.Item.GetBaseItem(itemKey2);
				int num2 = item2.PriceChanges[itemKey2];
				if (num > 0)
				{
					if (merchantData.CharId >= 0)
					{
						baseItem.RemoveOwner(ItemOwnerType.Merchant, merchantData.CharId);
					}
					else if (merchantTradeArguments.OpenShopEventArguments.IsFromBuilding)
					{
						baseItem.RemoveOwner(ItemOwnerType.BuildingMerchant, buildingMerchantCaravanId);
					}
					else if (merchantTradeArguments.OpenShopEventArguments.IsSpecialBuilding)
					{
						baseItem.RemoveOwner(ItemOwnerType.BuildingMerchant, -1);
					}
					else
					{
						baseItem.RemoveOwner(ItemOwnerType.Caravan, id);
					}
					if (ModificationStateHelper.IsActive(baseItem.GetModificationState(), 8))
					{
						merchantExtraGoods?.Remove(itemKey2.Id);
						if (ItemTemplateHelper.IsStackable(itemKey2.ItemType, itemKey2.TemplateId))
						{
							DomainManager.Item.RemoveItem(context, itemKey2);
							ItemKey itemKey3 = DomainManager.Item.CreateItem(context, itemKey2.ItemType, itemKey2.TemplateId);
							DomainManager.Taiwu.AddItem(context, itemKey3, num, item2.ItemSourceType);
						}
						else
						{
							DomainManager.Taiwu.AddItem(context, itemKey2, num, item2.ItemSourceType);
						}
					}
					else
					{
						DomainManager.Taiwu.AddItem(context, itemKey2, num, item2.ItemSourceType);
					}
					if (num2 < 0)
					{
						int value = baseItem.GetValue();
						int baseDelta = ProfessionFormulaImpl.Calculate(95, value) * Math.Abs(num);
						DomainManager.Extra.ChangeProfessionSeniority(context, 15, baseDelta);
					}
				}
				else if (num < 0)
				{
					DomainManager.Taiwu.RemoveItem(context, itemKey2, -num, item2.ItemSourceType, deleteItem: false);
					if (merchantData.CharId >= 0)
					{
						baseItem.SetOwner(ItemOwnerType.Merchant, merchantData.CharId);
					}
					else if (merchantTradeArguments.OpenShopEventArguments.IsFromBuilding)
					{
						baseItem.SetOwner(ItemOwnerType.BuildingMerchant, buildingMerchantCaravanId);
					}
					else if (merchantTradeArguments.OpenShopEventArguments.IsSpecialBuilding)
					{
						baseItem.SetOwner(ItemOwnerType.BuildingMerchant, -1);
					}
					else
					{
						baseItem.SetOwner(ItemOwnerType.Caravan, id);
					}
					if (num2 > 0)
					{
						int value2 = baseItem.GetValue();
						int baseDelta2 = ProfessionFormulaImpl.Calculate(96, value2) * Math.Abs(num);
						DomainManager.Extra.ChangeProfessionSeniority(context, 15, baseDelta2);
					}
				}
			}
		}
		if (merchantExtraGoods != null)
		{
			DomainManager.Extra.SetMerchantExtraGoods(context, id, merchantExtraGoods);
		}
		MerchantBuyBackData oldBuyBackData = GetMerchantBuyBackData(merchantTradeArguments.OpenShopEventArguments);
		SetMerchantBuyBackData(merchantTradeArguments.OpenShopEventArguments, merchantTradeArguments.MerchantBuyBackData);
		int num3 = tradeMoneySources.Values.Sum();
		int delta = ((num3 > 0) ? Math.Min(num3, merchantData.Money) : num3);
		switch (merchantTradeArguments.OpenShopEventArguments.MerchantSourceTypeEnum)
		{
		case OpenShopEventArguments.EMerchantSourceType.NormalCharacter:
		case OpenShopEventArguments.EMerchantSourceType.SpecifiedOnBuildingMerchantType:
			if (merchantData.CharId >= 0)
			{
				int argTradeMoney3 = HandleMerchantMoney();
				HandleHeadMoney(argTradeMoney3);
				SetElement_MerchantData(merchantData.CharId, merchantData, context);
			}
			break;
		case OpenShopEventArguments.EMerchantSourceType.NormalCaravan:
		case OpenShopEventArguments.EMerchantSourceType.AdventureCaravan:
		case OpenShopEventArguments.EMerchantSourceType.ProfessionSkillCaravan:
		{
			MerchantData value3;
			if (id > -1)
			{
				num3 = HandleMerchantMoney();
				SetElement_CaravanData(id, merchantData, context);
			}
			else if (id == -1 && TryGetElement_CaravanData(-1, out value3))
			{
				int argTradeMoney4 = HandleMerchantMoney();
				HandleHeadMoney(argTradeMoney4);
				SetElement_CaravanData(id, merchantData, context);
				_totalBuyMoney += buyMoney;
				DomainManager.TaiwuEvent.SetListenerEventActionIntArg("ShopActionComplete", "ConchShip_PresetKey_ShopBuyMoney", _totalBuyMoney);
			}
			break;
		}
		case OpenShopEventArguments.EMerchantSourceType.MerchantHeadBuilding:
		case OpenShopEventArguments.EMerchantSourceType.MerchantBranchBuilding:
			if (buildingMerchantType > -1)
			{
				int argTradeMoney2 = HandleMerchantMoney();
				HandleHeadMoney(argTradeMoney2);
				if (merchantTradeArguments.OpenShopEventArguments.IsHeadBuildingMerchant)
				{
					SetElement_MerchantMaxLevelData(buildingMerchantType, merchantData, context);
				}
				else
				{
					DomainManager.Extra.SetBranchMerchantData(context, buildingMerchantType, merchantData);
				}
			}
			break;
		case OpenShopEventArguments.EMerchantSourceType.SpecialBuilding:
		{
			int argTradeMoney = HandleMerchantMoney();
			HandleHeadMoney(argTradeMoney);
			SetSectStorySpecialMerchantData(context, merchantData);
			break;
		}
		default:
			throw new ArgumentOutOfRangeException();
		}
		DomainManager.TaiwuEvent.SetListenerEventActionBoolArg("ShopActionComplete", "ConchShip_PresetKey_ShopHasAnyTrade", value: true);
		DomainManager.TaiwuEvent.SetListenerEventActionBoolArg("MerchantShopClose", "ConchShip_PresetKey_ShopHasAnyTrade", value: true);
		if (isDebtAreaShop)
		{
			DomainManager.Extra.ChangeAreaSpiritualDebt(context, taiwu.GetLocation().AreaId, num3);
		}
		else
		{
			taiwu.ChangeResource(context, 6, delta);
		}
		if (buyMoney > 0)
		{
			ChangeMerchantCumulativeMoney(context, merchantData.MerchantType, buyMoney);
		}
		DomainManager.Extra.SetMerchantOverFavorData(context, merchantData.MerchantType, merchantTradeArguments.OverFavorData);
		void HandleHeadMoney(int num4)
		{
			if (!isDebtAreaShop)
			{
				int merchantMoney = GetMerchantMoney(context, merchantData.MerchantType);
				merchantMoney -= num4;
				SetMerchantMoney(context, merchantData.MerchantType, merchantMoney);
			}
		}
		int HandleMerchantMoney()
		{
			if (isDebtAreaShop)
			{
				return tradeMoneySources.Values.Sum();
			}
			if (merchantData.CharId >= 0)
			{
				GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(merchantData.CharId);
				int num4 = 0;
				ICollection<ItemKey> collection2;
				if (oldBuyBackData?.BuyInGoodsList.Items == null)
				{
					ICollection<ItemKey> collection = Array.Empty<ItemKey>();
					collection2 = collection;
				}
				else
				{
					ICollection<ItemKey> collection = oldBuyBackData.BuyInGoodsList.Items.Keys;
					collection2 = collection;
				}
				ICollection<ItemKey> collection3 = collection2;
				ICollection<ItemKey> collection4;
				if (merchantTradeArguments.MerchantBuyBackData?.BuyInGoodsList.Items == null)
				{
					ICollection<ItemKey> collection = Array.Empty<ItemKey>();
					collection4 = collection;
				}
				else
				{
					ICollection<ItemKey> collection = merchantTradeArguments.MerchantBuyBackData.BuyInGoodsList.Items.Keys;
					collection4 = collection;
				}
				ICollection<ItemKey> collection5 = collection4;
				foreach (KeyValuePair<ItemKey, int> item4 in tradeMoneySources)
				{
					item4.Deconstruct(out var key, out var value4);
					ItemKey item = key;
					int num5 = value4;
					bool flag = collection3.Contains(item) && !collection5.Contains(item);
					if (num5 < 0 && !flag)
					{
						int num6 = -num5 * 20 / 100;
						int num7 = -num5 - num6;
						element_Objects.ChangeResource(context, 6, num6);
						merchantData.Money += num7;
						num4 += num5;
					}
					else
					{
						int num8 = merchantData.Money - Math.Max(merchantData.Money - num5, 0);
						merchantData.Money -= num8;
						num4 += num8;
					}
				}
				return num4;
			}
			int num9 = Math.Min(tradeMoneySources.Values.Sum(), merchantData.Money);
			merchantData.Money -= num9;
			return num9;
		}
	}

	public void ChangeMerchantCumulativeMoney(DataContext context, sbyte merchantType, int delta)
	{
		if (delta > 0 && DomainManager.Extra.IsProfessionalSkillUnlockedAndEquipped(71))
		{
			delta *= 3;
		}
		int[] merchantFavorability = DomainManager.Merchant.GetMerchantFavorability();
		if (merchantFavorability.CheckIndex(merchantType))
		{
			int value = merchantFavorability[merchantType] + delta;
			int cumulativeMoney = GetCumulativeMoney(100);
			value = Math.Clamp(value, 0, cumulativeMoney);
			merchantFavorability[merchantType] = value;
			DomainManager.Merchant.SetMerchantFavorability(merchantFavorability, context);
		}
	}

	[Obsolete]
	public void ChangeMerchantFavorability(DataContext context, sbyte merchantType, int delta)
	{
		int[] merchantFavorability = DomainManager.Merchant.GetMerchantFavorability();
		int value = DomainManager.Merchant.GetFavorability(merchantFavorability[merchantType]) + delta;
		value = Math.Clamp(value, 0, 100);
		int cumulativeMoney = DomainManager.Merchant.GetCumulativeMoney(value);
		int delta2 = cumulativeMoney - merchantFavorability[merchantType];
		DomainManager.Merchant.ChangeMerchantCumulativeMoney(context, merchantType, delta2);
	}

	public int GetCumulativeMoney(int favorability)
	{
		int num = GlobalConfig.Instance.MerchantFavorabilityMoneyRequirements.Length;
		int[] array = new int[num];
		for (int i = 0; i < num; i++)
		{
			int num2 = 0;
			int num3 = i - 1;
			if (num3 >= 0 && num3 < num)
			{
				num2 = array[num3];
			}
			int num4 = GlobalConfig.Instance.MerchantFavorabilityMoneyRequirements[i];
			array[i] = num2 + num4;
		}
		int num5 = favorability / num - 1;
		int num6 = favorability % num;
		int num7 = 0;
		if (num5 >= 0)
		{
			int num8 = Math.Clamp(num5, 0, num - 1);
			num7 += array[num8];
		}
		if (num6 > 0)
		{
			int num9 = Math.Clamp(num5 + 1, 0, num - 1);
			num7 += (int)(1f * (float)num6 / (float)num * (float)array[num9]);
		}
		return num7;
	}

	public int GetFavorability(int cumulativeMoney)
	{
		int num = 0;
		for (int i = 0; i < GlobalConfig.Instance.MerchantFavorabilityMoneyRequirements.Length; i++)
		{
			int num2 = GlobalConfig.Instance.MerchantFavorabilityMoneyRequirements[i];
			cumulativeMoney -= num2;
			if (cumulativeMoney <= 0)
			{
				num += (int)Math.Floor(10f * (float)(num2 + cumulativeMoney) / (float)num2);
				break;
			}
			num += 10;
		}
		return num;
	}

	[DomainMethod]
	public int GetCurFavorability(sbyte merchantType)
	{
		return Math.Max(0, GetFavorabilityWithDelta(merchantType));
	}

	[DomainMethod]
	public int GetFavorabilityWithDelta(sbyte merchantType, int delta = 0)
	{
		int[] merchantFavorability = DomainManager.Merchant.GetMerchantFavorability();
		int num = (merchantFavorability.CheckIndex(merchantType) ? merchantFavorability[merchantType] : 0);
		if (num + delta < 0)
		{
			return -1;
		}
		return GetFavorability(num + delta);
	}

	[DomainMethod]
	public int[] GetAllFavorability()
	{
		int[] merchantFavorability = DomainManager.Merchant.GetMerchantFavorability();
		int[] array = new int[merchantFavorability.Length];
		for (int i = 0; i < merchantFavorability.Length; i++)
		{
			array[i] = GetFavorability(merchantFavorability[i]);
		}
		return array;
	}

	public bool TryGetMerchantData(int charId, out MerchantData value)
	{
		return TryGetElement_MerchantData(charId, out value);
	}

	public bool MerchantHasTargetItem(int charId, ItemKey itemKey, int amount)
	{
		if (!_merchantData.TryGetValue(charId, out var value))
		{
			return false;
		}
		for (int i = 0; i < 7; i++)
		{
			Inventory goodsList = value.GetGoodsList(i);
			if (goodsList.Items.TryGetValue(itemKey, out var value2) && value2 >= amount)
			{
				return true;
			}
		}
		return false;
	}

	public void RemoveExistingMerchantItem(DataContext context, int charId, ItemKey itemKey, int amount)
	{
		if (!_merchantData.TryGetValue(charId, out var value))
		{
			throw new Exception($"merchant {charId} has no item {itemKey}");
		}
		for (int i = 0; i < 7; i++)
		{
			Inventory goodsList = value.GetGoodsList(i);
			if (goodsList.Items.TryGetValue(itemKey, out var value2) && value2 >= amount)
			{
				DomainManager.Item.RemoveOwner(itemKey, ItemOwnerType.Merchant, charId);
				goodsList.OfflineRemove(itemKey, amount);
				SetElement_MerchantData(charId, value, context);
				return;
			}
		}
		throw new Exception($"merchant {charId} has no item {itemKey}");
	}

	private MerchantData CreateMerchantData(DataContext context, int charId)
	{
		sbyte merchantTemplateId = GetMerchantTemplateId(charId);
		MerchantData merchantData = new MerchantData(charId, merchantTemplateId);
		merchantData.GenerateGoods(context);
		merchantData.Money = GetMerchantMoney(context, merchantData.MerchantType);
		AddElement_MerchantData(charId, merchantData, context);
		return merchantData;
	}

	[DomainMethod]
	public sbyte GetMerchantTemplateId(int charId)
	{
		if (!DomainManager.Character.TryGetElement_Objects(charId, out var element))
		{
			return -1;
		}
		DomainManager.Extra.TryGetMerchantCharToType(charId, out var type);
		sbyte level = 0;
		short settlementId = element.GetOrganizationInfo().SettlementId;
		if (settlementId >= 0)
		{
			level = DomainManager.Merchant.GetMerchantLevel(type, settlementId);
		}
		return MerchantData.FindMerchantTemplateId(type, level);
	}

	public void RemoveMerchantData(DataContext context, int merchantId)
	{
		if (TryGetMerchantData(merchantId, out var value))
		{
			value.RemoveAllGoods(context);
			RemoveElement_MerchantData(merchantId, context);
		}
	}

	public void RemoveObsoleteMerchantData(DataContext context)
	{
		RemoveAllGoodsInMerchantBuyBackData(context);
		foreach (int key in _merchantData.Keys)
		{
			MerchantData data = _merchantData[key];
			data.RemoveAllGoods(context);
			RemoveElement_MerchantData(key, context);
		}
		ClearBuildingMerchantData(context);
		ClearTempCaravan(context);
	}

	public void SetVillagerRoleMerchantType(DataContext context)
	{
		List<int> charIds = new List<int>();
		DomainManager.Extra.GetVillagerRoleCharactersByTemplateId(3, ref charIds);
		foreach (int item in charIds)
		{
			VillagerRoleBase villagerRole = DomainManager.Extra.GetVillagerRole(item);
			if (villagerRole is VillagerRoleMerchant villagerRoleMerchant)
			{
				DomainManager.Taiwu.SetMerchantType(context, item, villagerRoleMerchant.DesignatedMerchantType, immediate: true);
			}
		}
	}

	private void ClearBuildingMerchantData(DataContext context)
	{
		for (sbyte b = 0; b < 7; b++)
		{
			MerchantData element_MerchantMaxLevelData = GetElement_MerchantMaxLevelData(b);
			if (element_MerchantMaxLevelData != null)
			{
				element_MerchantMaxLevelData.RemoveAllGoods(context);
				element_MerchantMaxLevelData.GenerateGoods(context, SharedMethods.GetBuildingMerchantCaravanId(b, isHead: true));
				SetElement_MerchantMaxLevelData(b, element_MerchantMaxLevelData, context);
			}
		}
		for (sbyte b2 = 0; b2 < 7; b2++)
		{
			MerchantData merchantData = DomainManager.Extra.BranchMerchantData[b2];
			if (merchantData != null)
			{
				merchantData.RemoveAllGoods(context);
				merchantData.GenerateGoods(context, SharedMethods.GetBuildingMerchantCaravanId(b2, isHead: false));
				DomainManager.Extra.SetBranchMerchantData(context, b2, merchantData);
			}
		}
	}

	public void InitializeOwnedItems()
	{
		int key;
		MerchantData value;
		foreach (KeyValuePair<int, MerchantData> merchantDatum in _merchantData)
		{
			merchantDatum.Deconstruct(out key, out value);
			int id = key;
			MerchantData merchantData = value;
			InitializeOwnedItemsFromMerchant(ItemOwnerType.Merchant, id, merchantData);
		}
		foreach (KeyValuePair<int, MerchantData> caravanDatum in _caravanData)
		{
			caravanDatum.Deconstruct(out key, out value);
			int id2 = key;
			MerchantData merchantData2 = value;
			InitializeOwnedItemsFromMerchant(ItemOwnerType.Caravan, id2, merchantData2);
		}
		for (sbyte b = 0; b < _merchantMaxLevelData.Length; b++)
		{
			MerchantData merchantData3 = _merchantMaxLevelData[b];
			if (merchantData3 != null)
			{
				int buildingMerchantCaravanId = SharedMethods.GetBuildingMerchantCaravanId(b, isHead: true);
				InitializeOwnedItemsFromMerchant(ItemOwnerType.BuildingMerchant, buildingMerchantCaravanId, merchantData3);
			}
		}
		for (sbyte b2 = 0; b2 < DomainManager.Extra.BranchMerchantData.Length; b2++)
		{
			MerchantData merchantData4 = DomainManager.Extra.BranchMerchantData[b2];
			if (merchantData4 != null)
			{
				int buildingMerchantCaravanId2 = SharedMethods.GetBuildingMerchantCaravanId(b2, isHead: false);
				InitializeOwnedItemsFromMerchant(ItemOwnerType.BuildingMerchant, buildingMerchantCaravanId2, merchantData4);
			}
		}
		SectStorySpecialMerchant sectStorySpecialMerchant = DomainManager.Extra.GetSectStorySpecialMerchant();
		if (sectStorySpecialMerchant != null && sectStorySpecialMerchant.MerchantData != null)
		{
			InitializeOwnedItemsFromMerchant(ItemOwnerType.BuildingMerchant, -1, sectStorySpecialMerchant.MerchantData);
		}
		static void InitializeOwnedItemsFromMerchant(ItemOwnerType ownerType, int ownerId, MerchantData merchantData5)
		{
			ItemKey key2;
			int value2;
			for (int i = 0; i < 7; i++)
			{
				Inventory goodsList = merchantData5.GetGoodsList(i);
				foreach (KeyValuePair<ItemKey, int> item in goodsList.Items)
				{
					item.Deconstruct(out key2, out value2);
					ItemKey itemKey = key2;
					DomainManager.Item.SetOwner(itemKey, ownerType, ownerId);
				}
			}
			if (merchantData5.BuyInGoodsList != null)
			{
				foreach (KeyValuePair<ItemKey, int> item2 in merchantData5.BuyInGoodsList.Items)
				{
					item2.Deconstruct(out key2, out value2);
					ItemKey itemKey2 = key2;
					DomainManager.Item.SetOwner(itemKey2, ownerType, ownerId);
				}
			}
		}
	}

	public bool HasNewGoods(GameData.Domains.Character.Character character)
	{
		OrganizationInfo organizationInfo = character.GetOrganizationInfo();
		OrganizationItem organizationItem = Config.Organization.Instance[organizationInfo.OrgTemplateId];
		OrganizationMemberItem orgMemberConfig = OrganizationDomain.GetOrgMemberConfig(organizationInfo);
		return organizationItem.IsCivilian && organizationInfo.OrgTemplateId != 0 && organizationInfo.Grade == 4 && character.GetCurrAge() >= orgMemberConfig.IdentityActiveAge && !_merchantData.ContainsKey(character.GetId());
	}

	public sbyte GetMerchantLevel(sbyte merchantType, short settlementId)
	{
		Settlement settlement = DomainManager.Organization.GetSettlement(settlementId);
		sbyte orgTemplateId = settlement.GetOrgTemplateId();
		OrganizationItem organizationItem = Config.Organization.Instance[orgTemplateId];
		MerchantTypeItem merchantTypeItem = Config.MerchantType.Instance[merchantType];
		List<Settlement> list = new List<Settlement>();
		DomainManager.Organization.GetAllCivilianSettlements(list);
		foreach (Settlement item in list)
		{
			if (item.GetOrgTemplateId() == orgTemplateId)
			{
				settlement = item;
				break;
			}
		}
		short num = ((merchantTypeItem.CityAttributeType == EMerchantTypeCityAttributeType.Safety) ? settlement.GetSafety() : settlement.GetCulture());
		sbyte value = (sbyte)(organizationItem.MerchantLevel + ((num >= 50) ? 1 : (-1)));
		return Math.Clamp(value, 0, 6);
	}

	[DomainMethod]
	public void GmCmd_AddItem(DataContext context, int charId, sbyte itemType, short templateId, int count, int level)
	{
		GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(charId);
		if (element_Objects.IsMerchant(element_Objects.GetOrganizationInfo()))
		{
			MerchantData merchantData = GetMerchantData(context, charId);
			sbyte max = Math.Min(merchantData.MerchantConfig.Level, 6);
			level = Math.Clamp(level, 0, max);
			Inventory goodsList = merchantData.GetGoodsList(level);
			ItemKey itemKey = DomainManager.Item.CreateItem(context, itemType, templateId);
			goodsList.OfflineAdd(itemKey, count);
			DomainManager.Item.SetOwner(itemKey, ItemOwnerType.Merchant, charId);
			SetElement_MerchantData(charId, merchantData, context);
		}
	}

	[DomainMethod]
	public MerchantOverFavorData GetMerchantOverFavorData(sbyte merchantType)
	{
		return DomainManager.Extra.GetMerchantOverFavorData(merchantType);
	}

	[DomainMethod]
	public List<MerchantInfoAreaData> GetMerchantInfoAreaDataList(sbyte merchantType)
	{
		List<MerchantInfoAreaData> list = new List<MerchantInfoAreaData>();
		Dictionary<short, HashSet<int>> areaMerchantCharDict = DomainManager.Character.GetAreaMerchantCharDict(merchantType);
		foreach (KeyValuePair<short, HashSet<int>> item2 in areaMerchantCharDict)
		{
			item2.Deconstruct(out var key, out var value);
			short areaTemplateId = key;
			HashSet<int> hashSet = value;
			MerchantInfoAreaData item = new MerchantInfoAreaData
			{
				AreaTemplateId = areaTemplateId,
				MerchantCount = hashSet.Count
			};
			list.Add(item);
		}
		foreach (KeyValuePair<int, CaravanPath> item3 in _caravanDict)
		{
			item3.Deconstruct(out var key2, out var value2);
			int key3 = key2;
			CaravanPath caravanPath = value2;
			if (_caravanData[key3].MerchantType == merchantType)
			{
				short areaId = caravanPath.GetCurrLocation().AreaId;
				MapAreaData areaByAreaId = DomainManager.Map.GetAreaByAreaId(areaId);
				short areaTemplateId2 = areaByAreaId.GetTemplateId();
				int num = list.FindIndex((MerchantInfoAreaData d) => d.AreaTemplateId == areaTemplateId2);
				MerchantInfoAreaData merchantInfoAreaData;
				if (num >= 0)
				{
					merchantInfoAreaData = list[num];
				}
				else
				{
					merchantInfoAreaData = new MerchantInfoAreaData
					{
						AreaTemplateId = areaTemplateId2
					};
					list.Add(merchantInfoAreaData);
				}
				merchantInfoAreaData.CaravanCount++;
			}
		}
		return list;
	}

	[DomainMethod]
	public List<MerchantInfoCaravanData> GetMerchantInfoCaravanDataList(DataContext context, sbyte merchantType)
	{
		List<MerchantInfoCaravanData> list = new List<MerchantInfoCaravanData>();
		foreach (KeyValuePair<int, CaravanPath> item in _caravanDict)
		{
			item.Deconstruct(out var key, out var value);
			int num = key;
			CaravanPath caravanPath = value;
			MerchantData merchantData = _caravanData[num];
			if (merchantType >= 0 && merchantData.MerchantType != merchantType)
			{
				continue;
			}
			if (!DomainManager.Extra.TryGetCaravanExtraData(num, out var caravanExtraData))
			{
				caravanExtraData = CreateCaravanExtraData(context, num);
			}
			MerchantInfoCaravanData merchantInfoCaravanData = new MerchantInfoCaravanData
			{
				CaravanId = num,
				MerchantTemplateId = merchantData.MerchantTemplateId,
				CurrentAreaTemplateId = DomainManager.Map.GetAreaByAreaId(caravanPath.GetCurrLocation().AreaId).GetTemplateId(),
				TargetAreaTemplateId = DomainManager.Map.GetAreaByAreaId(caravanPath.GetDestLocation().AreaId).GetTemplateId(),
				StartAreaTemplateId = DomainManager.Map.GetAreaByAreaId(caravanPath.GetSrcLocation().AreaId).GetTemplateId(),
				RemainSettlementInfoList = new List<SettlementDisplayData>(),
				RemainNodeCount = caravanPath.MoveNodes.Count - 1,
				ExtraData = caravanExtraData,
				IsInBrokenArea = MapAreaData.IsBrokenArea(caravanPath.GetCurrLocation().AreaId),
				CaravanPath = new CaravanPath(caravanPath)
			};
			if (caravanExtraData.SettlementIdList != null)
			{
				foreach (short settlementId in caravanExtraData.SettlementIdList)
				{
					SettlementDisplayData displayData = DomainManager.Organization.GetDisplayData(settlementId);
					merchantInfoCaravanData.RemainSettlementInfoList.Add(displayData);
				}
			}
			list.Add(merchantInfoCaravanData);
		}
		return list;
	}

	[DomainMethod]
	public List<MerchantInfoMerchantData> GetMerchantInfoMerchantDataList(sbyte merchantType)
	{
		List<MerchantInfoMerchantData> list = new List<MerchantInfoMerchantData>();
		Dictionary<short, HashSet<int>> areaMerchantCharDict = DomainManager.Character.GetAreaMerchantCharDict(merchantType);
		foreach (var (_, hashSet2) in areaMerchantCharDict)
		{
			foreach (int item2 in hashSet2)
			{
				GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(item2);
				int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
				RelatedCharacter relation;
				short favorability = (DomainManager.Character.TryGetRelation(item2, taiwuCharId, out relation) ? relation.Favorability : short.MinValue);
				short areaId = element_Objects.GetValidLocation().AreaId;
				MapAreaData areaByAreaId = DomainManager.Map.GetAreaByAreaId(areaId);
				short templateId = areaByAreaId.GetTemplateId();
				DomainManager.Organization.TryGetSettlementCharacter(item2, out var settlementChar);
				Settlement settlement = DomainManager.Organization.GetSettlement(settlementChar.GetSettlementId());
				MerchantInfoMerchantData item = new MerchantInfoMerchantData
				{
					CharId = item2,
					BehaviorType = element_Objects.GetBehaviorType(),
					Favorability = favorability,
					NameRelatedData = DomainManager.Character.GetNameRelatedData(item2),
					MerchantTemplateId = DomainManager.Merchant.GetMerchantTemplateId(item2),
					CurrentAreaTemplateId = templateId,
					OrgTemplateId = settlementChar.GetOrgTemplateId(),
					FullBlockName = DomainManager.Map.GetBlockFullName(settlement.GetLocation())
				};
				list.Add(item);
			}
		}
		return list;
	}

	[DomainMethod]
	public SectStorySpecialMerchant GetSectStorySpecialMerchantData(DataContext context)
	{
		int currDate = DomainManager.World.GetCurrDate();
		SectStorySpecialMerchant sectStorySpecialMerchant = DomainManager.Extra.GetSectStorySpecialMerchant();
		MerchantItem merchantItem = Config.Merchant.Instance[(sbyte)51];
		if (sectStorySpecialMerchant?.MerchantData == null)
		{
			sectStorySpecialMerchant = new SectStorySpecialMerchant();
			sectStorySpecialMerchant.MerchantExtraGoodsData = new MerchantExtraGoodsData();
			MerchantData merchantData = new MerchantData(-1, merchantItem.TemplateId);
			merchantData.GenerateGoods(context, merchantItem.Level, -1, sectStorySpecialMerchant.MerchantExtraGoodsData);
			sectStorySpecialMerchant.MerchantExtraGoodsData.SeasonTemplateId = -1;
			sectStorySpecialMerchant.MerchantData = merchantData;
			sectStorySpecialMerchant.RefreshTime = currDate;
			DomainManager.Extra.SetSectStorySpecialMerchant(sectStorySpecialMerchant, context);
		}
		else if (sectStorySpecialMerchant.RefreshTime + merchantItem.RefreshInterval <= currDate)
		{
			sectStorySpecialMerchant.MerchantData.RemoveAllGoods(context);
			_sectStorySpecialMerchantBuyBackData?.RemoveAllGoods(context);
			MerchantData merchantData2 = new MerchantData(-1, merchantItem.TemplateId);
			sectStorySpecialMerchant.MerchantExtraGoodsData.Clear();
			merchantData2.GenerateGoods(context, merchantItem.Level, -1, sectStorySpecialMerchant.MerchantExtraGoodsData);
			sectStorySpecialMerchant.MerchantData = merchantData2;
			sectStorySpecialMerchant.RefreshTime = currDate;
		}
		sectStorySpecialMerchant.MerchantData.Money = DomainManager.Merchant.GetMerchantMoney(context, merchantItem.MerchantType);
		DomainManager.Extra.SetSectStorySpecialMerchant(sectStorySpecialMerchant, context);
		return sectStorySpecialMerchant;
	}

	private void SetSectStorySpecialMerchantData(DataContext context, MerchantData merchantData)
	{
		SectStorySpecialMerchant sectStorySpecialMerchant = DomainManager.Extra.GetSectStorySpecialMerchant();
		sectStorySpecialMerchant.MerchantData = merchantData;
		DomainManager.Extra.SetSectStorySpecialMerchant(sectStorySpecialMerchant, context);
	}

	public ItemKey CreateMerchantRandomItem(DataContext context, short merchantTemplateId)
	{
		ItemKey result = ItemKey.Invalid;
		MerchantItem template = Config.Merchant.Instance[merchantTemplateId];
		List<PresetItemTemplateIdGroup> list = new List<PresetItemTemplateIdGroup>();
		for (int i = 0; i <= 7; i++)
		{
			IList<PresetItemTemplateIdGroup> goodsPreset = MerchantData.GetGoodsPreset(template, i);
			if (goodsPreset != null && goodsPreset.Count > 0)
			{
				list.AddRange(goodsPreset);
			}
		}
		if (list.Count > 0)
		{
			PresetItemTemplateIdGroup random = list.GetRandom(context.Random);
			result = DomainManager.Item.CreateItem(context, random.ItemType, random.StartId);
		}
		return result;
	}

	[DomainMethod]
	public bool CanRefreshMerchantGoods(DataContext context, bool consume = false)
	{
		if (DomainManager.Extra.GetTotalActionPointsRemaining() < GlobalConfig.Instance.RefreshItemApCost)
		{
			return false;
		}
		if (consume)
		{
			DomainManager.World.ConsumeActionPoint(context, GlobalConfig.Instance.RefreshItemApCost);
		}
		return true;
	}

	[DomainMethod]
	public bool RefreshMerchantGoods(DataContext context, int charOrCaravanId, bool isChar, sbyte level, bool isFromBuilding, bool isHeadBuildingMerchant, sbyte buildingMerchantType)
	{
		if (!CanRefreshMerchantGoods(context, consume: true))
		{
			return false;
		}
		GameData.Domains.Character.Character character = null;
		int caravanId = -1;
		MerchantData merchantData = null;
		if (isChar)
		{
			character = DomainManager.Character.GetElement_Objects(charOrCaravanId);
			merchantData = GetMerchantData(context, charOrCaravanId);
		}
		else if (isFromBuilding)
		{
			caravanId = SharedMethods.GetBuildingMerchantCaravanId(buildingMerchantType, isHeadBuildingMerchant);
			merchantData = GetBuildingMerchantData(context, buildingMerchantType, isHeadBuildingMerchant);
		}
		else
		{
			caravanId = charOrCaravanId;
			merchantData = GetCaravanMerchantData(context, charOrCaravanId);
		}
		if (isFromBuilding)
		{
			DomainManager.Extra.SetMerchantExtraGoods(context, SharedMethods.GetBuildingMerchantCaravanId(buildingMerchantType, isHeadBuildingMerchant), merchantData.OfflineRefreshGoods(context, level, character, caravanId));
		}
		else
		{
			DomainManager.Extra.SetMerchantExtraGoods(context, charOrCaravanId, merchantData.OfflineRefreshGoods(context, level, character, caravanId));
		}
		if (isChar)
		{
			SetMerchantData(charOrCaravanId, merchantData, context);
		}
		else if (isFromBuilding)
		{
			if (isHeadBuildingMerchant)
			{
				SetElement_MerchantMaxLevelData(buildingMerchantType, merchantData, context);
			}
			else
			{
				DomainManager.Extra.SetBranchMerchantData(context, buildingMerchantType, merchantData);
			}
		}
		else
		{
			SetCaravanData(charOrCaravanId, merchantData, context);
		}
		return true;
	}

	private static SkillBookTradeInfo CreateSkillBookTradeInfo(DataContext context, int charId)
	{
		SkillBookTradeInfo skillBookTradeInfo = new SkillBookTradeInfo(charId);
		GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(charId);
		DomainManager.Character.GetSkillBookLibrary(context, element_Objects, skillBookTradeInfo.PrivateSkillBooks, skillBookTradeInfo.PrivateSkillBooks);
		OrganizationInfo organizationInfo = element_Objects.GetOrganizationInfo();
		if (OrganizationDomain.IsSect(organizationInfo.OrgTemplateId))
		{
			foreach (ItemKey privateSkillBook in skillBookTradeInfo.PrivateSkillBooks)
			{
				SkillBookItem skillBookItem = Config.SkillBook.Instance[privateSkillBook.TemplateId];
				if (skillBookItem.CombatSkillTemplateId < 0)
				{
					break;
				}
				CombatSkillItem combatSkillItem = Config.CombatSkill.Instance[skillBookItem.CombatSkillTemplateId];
				if (combatSkillItem.SectId == organizationInfo.OrgTemplateId)
				{
					skillBookTradeInfo.SectSkillBooks.Add(privateSkillBook);
				}
			}
		}
		return skillBookTradeInfo;
	}

	[DomainMethod]
	public void FinishBookTrade(DataContext context, int charId, bool isFavor)
	{
		if (_skillBookTradeInfo != null)
		{
			GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(charId);
			if (_skillBookTradeInfo.BoughtBooksFromTaiwu.Count > 0)
			{
				element_Objects.AddInventoryItemList(context, _skillBookTradeInfo.BoughtBooksFromTaiwu);
			}
			DomainManager.Character.DealWithSkillBookLibraryAfterTrading(context, element_Objects, _skillBookTradeInfo.SoldBooksToTaiwu, _skillBookTradeInfo.PrivateSkillBooks);
			_skillBookTradeInfo = null;
		}
	}

	[DomainMethod]
	public void ExchangeBook(DataContext context, int npcId, List<ItemDisplayData> boughtItems, List<ItemDisplayData> soldItems, int selfAuthority, int npcAuthority)
	{
		if (boughtItems != null)
		{
			foreach (ItemDisplayData boughtItem in boughtItems)
			{
				ItemKey key = boughtItem.Key;
				ItemBase baseItem = DomainManager.Item.GetBaseItem(key);
				baseItem.RemoveOwner(ItemOwnerType.Library, npcId);
				DomainManager.Taiwu.AddItem(context, key, boughtItem.Amount, boughtItem.ItemSourceType);
				if (_skillBookTradeInfo.BoughtBooksFromTaiwu.Contains(key))
				{
					_skillBookTradeInfo.BoughtBooksFromTaiwu.Remove(key);
					continue;
				}
				_skillBookTradeInfo.PrivateSkillBooks.Remove(key);
				_skillBookTradeInfo.SectSkillBooks.Remove(key);
				_skillBookTradeInfo.SoldBooksToTaiwu.Add(key);
				short itemSubType = ItemTemplateHelper.GetItemSubType(key.ItemType, key.TemplateId);
				if (itemSubType == 1000)
				{
					int baseDelta = ProfessionFormulaImpl.Calculate(102, baseItem.GetValue());
					DomainManager.Extra.ChangeProfessionSeniority(context, 16, baseDelta);
				}
				else
				{
					int baseDelta2 = ProfessionFormulaImpl.Calculate(51, baseItem.GetValue());
					DomainManager.Extra.ChangeProfessionSeniority(context, 7, baseDelta2);
				}
			}
		}
		if (soldItems != null)
		{
			foreach (ItemDisplayData soldItem in soldItems)
			{
				ItemKey key2 = soldItem.Key;
				_skillBookTradeInfo.BoughtBooksFromTaiwu.Add(key2);
				DomainManager.Taiwu.RemoveItem(context, key2, soldItem.Amount, soldItem.ItemSourceType, deleteItem: false);
				ItemBase baseItem2 = DomainManager.Item.GetBaseItem(key2);
				baseItem2.SetOwner(ItemOwnerType.Library, npcId);
			}
		}
		GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
		taiwu.SpecifyResource(context, 7, selfAuthority);
		GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(npcId);
		element_Objects.SpecifyResource(context, 7, npcAuthority);
		DomainManager.TaiwuEvent.CheckTaiwuStatusImmediately();
	}

	[DomainMethod]
	public List<ItemDisplayData> GetTradeBookDisplayData(DataContext context, int npcId, bool isFavor)
	{
		if (_skillBookTradeInfo == null || _skillBookTradeInfo.CharId != npcId)
		{
			_skillBookTradeInfo = CreateSkillBookTradeInfo(context, npcId);
		}
		return DomainManager.Item.GetItemDisplayDataListOptional(isFavor ? _skillBookTradeInfo.PrivateSkillBooks : _skillBookTradeInfo.SectSkillBooks, -1, -1);
	}

	[DomainMethod]
	public List<ItemDisplayData> GetTradeBackBookDisplayData()
	{
		return DomainManager.Item.GetItemDisplayDataListOptional(_skillBookTradeInfo.BoughtBooksFromTaiwu, -1, -1);
	}

	[DomainMethod]
	public MerchantBuyBackData GetMerchantBuyBackData(OpenShopEventArguments openShopEventArguments)
	{
		MerchantBuyBackData value;
		switch (openShopEventArguments.MerchantSourceTypeEnum)
		{
		case OpenShopEventArguments.EMerchantSourceType.NormalCharacter:
			return _merchantBuyBackData.TryGetValue(openShopEventArguments.Id, out value) ? value : null;
		case OpenShopEventArguments.EMerchantSourceType.MerchantHeadBuilding:
			return _merchantMaxLevelBuyBackData[openShopEventArguments.BuildingMerchantType];
		case OpenShopEventArguments.EMerchantSourceType.MerchantBranchBuilding:
			return _branchMerchantBuyBackData[openShopEventArguments.BuildingMerchantType];
		case OpenShopEventArguments.EMerchantSourceType.SpecialBuilding:
			return _sectStorySpecialMerchantBuyBackData;
		case OpenShopEventArguments.EMerchantSourceType.NormalCaravan:
			return _caravanBuyBackData.TryGetValue(openShopEventArguments.Id, out value) ? value : null;
		case OpenShopEventArguments.EMerchantSourceType.AdventureCaravan:
			return _tempMerchantBuyBackData;
		case OpenShopEventArguments.EMerchantSourceType.ProfessionSkillCaravan:
			return _tempMerchantBuyBackData;
		case OpenShopEventArguments.EMerchantSourceType.SpecifiedOnBuildingMerchantType:
			return _merchantBuyBackData.TryGetValue(openShopEventArguments.Id, out value) ? value : null;
		default:
			throw new ArgumentOutOfRangeException();
		case OpenShopEventArguments.EMerchantSourceType.None:
		case OpenShopEventArguments.EMerchantSourceType.SettlementTreasury:
			return null;
		}
	}

	private void SetMerchantBuyBackData(OpenShopEventArguments openShopEventArguments, MerchantBuyBackData merchantBuyBackData)
	{
		switch (openShopEventArguments.MerchantSourceTypeEnum)
		{
		case OpenShopEventArguments.EMerchantSourceType.None:
			break;
		case OpenShopEventArguments.EMerchantSourceType.NormalCharacter:
			_merchantBuyBackData[openShopEventArguments.Id] = merchantBuyBackData;
			break;
		case OpenShopEventArguments.EMerchantSourceType.MerchantHeadBuilding:
			_merchantMaxLevelBuyBackData[openShopEventArguments.BuildingMerchantType] = merchantBuyBackData;
			break;
		case OpenShopEventArguments.EMerchantSourceType.MerchantBranchBuilding:
			_branchMerchantBuyBackData[openShopEventArguments.BuildingMerchantType] = merchantBuyBackData;
			break;
		case OpenShopEventArguments.EMerchantSourceType.SettlementTreasury:
			break;
		case OpenShopEventArguments.EMerchantSourceType.SpecialBuilding:
			_sectStorySpecialMerchantBuyBackData = merchantBuyBackData;
			break;
		case OpenShopEventArguments.EMerchantSourceType.NormalCaravan:
			_caravanBuyBackData[openShopEventArguments.Id] = merchantBuyBackData;
			break;
		case OpenShopEventArguments.EMerchantSourceType.AdventureCaravan:
			_tempMerchantBuyBackData = merchantBuyBackData;
			break;
		case OpenShopEventArguments.EMerchantSourceType.ProfessionSkillCaravan:
			_tempMerchantBuyBackData = merchantBuyBackData;
			break;
		case OpenShopEventArguments.EMerchantSourceType.SpecifiedOnBuildingMerchantType:
			_merchantBuyBackData[openShopEventArguments.Id] = merchantBuyBackData;
			break;
		default:
			throw new ArgumentOutOfRangeException();
		}
	}

	public bool RemoveBuyBackItem(ItemKey itemKey)
	{
		if (RemoveButBackItemInInventory(_merchantBuyBackData.Values))
		{
			return true;
		}
		if (RemoveButBackItemInInventory(_caravanBuyBackData.Values))
		{
			return true;
		}
		if (RemoveButBackItemInInventory(_merchantMaxLevelBuyBackData))
		{
			return true;
		}
		if (RemoveButBackItemInInventory(_branchMerchantBuyBackData))
		{
			return true;
		}
		return false;
		bool RemoveButBackItemInInventory(IEnumerable<MerchantBuyBackData> buyBackDataCollection)
		{
			foreach (MerchantBuyBackData item in buyBackDataCollection)
			{
				if (item != null && item.BuyInGoodsList.Items.ContainsKey(itemKey))
				{
					item.BuyInGoodsList.OfflineRemove(itemKey, 1);
					ItemBase baseItem = DomainManager.Item.GetBaseItem(itemKey);
					baseItem.RemoveOwner(baseItem.Owner.OwnerType, baseItem.Owner.OwnerId);
					return true;
				}
			}
			return false;
		}
	}

	public ItemKey TryGetBuyBackItemForPersonalNeed(DataContext context, GameData.Domains.Character.Ai.PersonalNeed personalNeed)
	{
		ItemKey result = GetBuyBackItemInInventory(_merchantBuyBackData.Values);
		if (result.IsValid())
		{
			return result;
		}
		result = GetBuyBackItemInInventory(_caravanBuyBackData.Values);
		if (result.IsValid())
		{
			return result;
		}
		result = GetBuyBackItemInInventory(_merchantMaxLevelBuyBackData);
		if (result.IsValid())
		{
			return result;
		}
		result = GetBuyBackItemInInventory(_branchMerchantBuyBackData);
		if (result.IsValid())
		{
			return result;
		}
		return ItemKey.Invalid;
		ItemKey GetBuyBackItemInInventory(IEnumerable<MerchantBuyBackData> buyBackDataCollection)
		{
			foreach (MerchantBuyBackData item in buyBackDataCollection)
			{
				if (item != null)
				{
					ItemKey result2 = item.TryGetGoodInSameGroup(context, personalNeed.ItemType, personalNeed.ItemTemplateId, -2);
					if (result2.IsValid())
					{
						return result2;
					}
				}
			}
			return ItemKey.Invalid;
		}
	}

	private void RemoveAllGoodsInMerchantBuyBackData(DataContext context)
	{
		RemoveAllGoods(_merchantBuyBackData.Values);
		RemoveAllGoods(_caravanBuyBackData.Values);
		RemoveAllGoods(_merchantMaxLevelBuyBackData);
		RemoveAllGoods(_branchMerchantBuyBackData);
		_tempMerchantData?.RemoveAllGoods(context);
		_sectStorySpecialMerchantBuyBackData?.RemoveAllGoods(context);
		_merchantBuyBackData.Clear();
		_caravanBuyBackData.Clear();
		void RemoveAllGoods(IEnumerable<MerchantBuyBackData> buyBackDataCollection)
		{
			foreach (MerchantBuyBackData item in buyBackDataCollection)
			{
				item?.RemoveAllGoods(context);
			}
		}
	}

	private void InitMerchantBuyBackData(DataContext context)
	{
		foreach (int key in _merchantData.Keys)
		{
			MerchantData merchantData = _merchantData[key];
			if (TransferMerchantBuyBackData(merchantData, out var merchantBuyBackData))
			{
				SetMerchantData(key, merchantData, context);
				_merchantBuyBackData[key] = merchantBuyBackData;
			}
		}
		foreach (int key2 in _caravanData.Keys)
		{
			MerchantData merchantData2 = _caravanData[key2];
			if (TransferMerchantBuyBackData(merchantData2, out var merchantBuyBackData2))
			{
				SetCaravanData(key2, merchantData2, context);
				_caravanBuyBackData[key2] = merchantBuyBackData2;
			}
		}
		for (sbyte b = 0; b < _merchantMaxLevelData.Length; b++)
		{
			MerchantData merchantData3 = _merchantMaxLevelData[b];
			if (merchantData3 != null && TransferMerchantBuyBackData(merchantData3, out var merchantBuyBackData3))
			{
				SetElement_MerchantMaxLevelData(b, merchantData3, context);
				_merchantMaxLevelBuyBackData[b] = merchantBuyBackData3;
			}
		}
		for (sbyte b2 = 0; b2 < DomainManager.Extra.BranchMerchantData.Length; b2++)
		{
			MerchantData merchantData4 = DomainManager.Extra.BranchMerchantData[b2];
			if (merchantData4 != null && TransferMerchantBuyBackData(merchantData4, out var merchantBuyBackData4))
			{
				DomainManager.Extra.SetBranchMerchantData(context, b2, merchantData4);
				_branchMerchantBuyBackData[b2] = merchantBuyBackData4;
			}
		}
		SectStorySpecialMerchant sectStorySpecialMerchant = DomainManager.Extra.GetSectStorySpecialMerchant();
		if (sectStorySpecialMerchant?.MerchantData != null && TransferMerchantBuyBackData(sectStorySpecialMerchant?.MerchantData, out var merchantBuyBackData5))
		{
			DomainManager.Extra.SetSectStorySpecialMerchant(sectStorySpecialMerchant, context);
			_sectStorySpecialMerchantBuyBackData = merchantBuyBackData5;
		}
	}

	private bool TransferMerchantBuyBackData(MerchantData merchantData, out MerchantBuyBackData merchantBuyBackData)
	{
		merchantBuyBackData = null;
		if (merchantData.BuyInGoodsList == null || merchantData.BuyInGoodsList.Items.Count == 0)
		{
			return false;
		}
		merchantBuyBackData = new MerchantBuyBackData();
		merchantBuyBackData.MerchantType = merchantData.MerchantType;
		foreach (var (itemKey2, amount) in merchantData.BuyInGoodsList.Items)
		{
			merchantBuyBackData.BuyInGoodsList.OfflineAdd(itemKey2, amount);
			if (merchantData.BuyInPrice.TryGetValue(itemKey2, out var value))
			{
				merchantBuyBackData.BuyInPrice[itemKey2] = value;
			}
		}
		merchantData.BuyInGoodsList?.Items.Clear();
		merchantData.BuyInPrice?.Clear();
		return true;
	}

	[DomainMethod]
	public void PullTradeCaravanLocations(DataContext context)
	{
		RefreshCaravanInTaiwuState(context);
	}

	[DomainMethod]
	public MerchantData GetCaravanMerchantData(DataContext context, int caravanId)
	{
		if (caravanId == -1 && _tempMerchantData != null)
		{
			return _tempMerchantData;
		}
		if (!TryGetElement_CaravanData(caravanId, out var value))
		{
			return null;
		}
		if (caravanId >= 0)
		{
			DomainManager.Extra.TryGetMerchantExtraGoods(caravanId, out var data);
			sbyte b = data?.SeasonTemplateId ?? (-1);
			if (b != EventHelper.GetCurrSeason())
			{
				value.RefreshSeasonExtraGoods(context, caravanId);
				SetCaravanData(caravanId, value, context);
			}
		}
		return value;
	}

	[DomainMethod]
	public MerchantData GetBuildingMerchantData(DataContext context, sbyte merchantType, bool isHead)
	{
		MerchantData[] array = (isHead ? _merchantMaxLevelData : DomainManager.Extra.BranchMerchantData);
		MerchantBuyBackData[] array2 = (isHead ? _merchantMaxLevelBuyBackData : _branchMerchantBuyBackData);
		MerchantTypeItem merchantTypeItem = Config.MerchantType.Instance[merchantType];
		sbyte targetLevel = (isHead ? merchantTypeItem.HeadLevel : merchantTypeItem.BranchLevel);
		MerchantData merchantData = array[merchantType];
		if (merchantData != null && merchantData.MerchantLevel != targetLevel)
		{
			array2[merchantType]?.RemoveAllGoods(context);
			merchantData.RemoveAllGoods(context);
			merchantData = null;
		}
		int buildingMerchantCaravanId = SharedMethods.GetBuildingMerchantCaravanId(merchantType, isHead);
		if (merchantData == null)
		{
			MerchantItem merchantItem = Config.Merchant.Instance.FirstOrDefault((MerchantItem m) => m.Level == targetLevel && m.MerchantType == merchantType);
			Tester.Assert(merchantItem != null);
			array[merchantType] = new MerchantData(-1, merchantItem.TemplateId);
			array[merchantType].GenerateGoods(context, buildingMerchantCaravanId);
			merchantData = array[merchantType];
			if (isHead)
			{
				SetElement_MerchantMaxLevelData(merchantType, merchantData, context);
			}
			else
			{
				DomainManager.Extra.SetBranchMerchantData(context, merchantType, merchantData);
			}
		}
		else
		{
			DomainManager.Extra.TryGetMerchantExtraGoods(buildingMerchantCaravanId, out var data);
			sbyte b = data?.SeasonTemplateId ?? (-1);
			if (b != EventHelper.GetCurrSeason())
			{
				merchantData.RefreshSeasonExtraGoods(context, buildingMerchantCaravanId);
				if (isHead)
				{
					SetElement_MerchantMaxLevelData(merchantType, merchantData, context);
				}
				else
				{
					DomainManager.Extra.SetBranchMerchantData(context, merchantType, merchantData);
				}
			}
		}
		merchantData.Money = GetMerchantMoney(context, merchantType);
		return merchantData;
	}

	private void InitMerchantMoney(DataContext context)
	{
		for (int i = 0; i < Config.Merchant.Instance.Count; i++)
		{
			MerchantItem merchantItem = Config.Merchant.Instance[i];
			if (merchantItem.Level == 6)
			{
				int money = merchantItem.Money * context.Random.Next(80, 120) / 100;
				SetMerchantMoney(context, merchantItem.MerchantType, money);
			}
		}
	}

	public void GenTradeCaravansOnAdvanceMonth(DataContext context)
	{
		if (DomainManager.World.GetMainStoryLineProgress() < 16)
		{
			return;
		}
		foreach (MerchantItem item in (IEnumerable<MerchantItem>)Config.Merchant.Instance)
		{
			if (item.Money <= 0 || item.Level == 6 || DomainManager.World.GetCurrDate() % item.GenerateInterval != 0)
			{
				continue;
			}
			int needMoney = item.Money * context.Random.Next(50, 150) / 100;
			int merchantMoney = GetMerchantMoney(context, item.MerchantType);
			if (needMoney > merchantMoney)
			{
				if (item.Level == 0)
				{
					merchantMoney += item.Money * context.Random.Next(80, 120) / 100;
					SetMerchantMoney(context, item.MerchantType, merchantMoney);
				}
				continue;
			}
			short areaIdByAreaTemplateId = DomainManager.Map.GetAreaIdByAreaTemplateId((short)context.Random.Next(1, 15));
			Location destLocation = new Location(areaIdByAreaTemplateId, DomainManager.Map.GetElement_Areas(areaIdByAreaTemplateId).SettlementInfos[0].BlockId);
			CreateCaravan(item, destLocation);
			merchantMoney -= needMoney;
			SetMerchantMoney(context, item.MerchantType, merchantMoney);
			bool flag = true;
			List<int> caravanStayDaysKeys = DomainManager.Extra.GetCaravanStayDaysKeys();
			foreach (int item2 in caravanStayDaysKeys)
			{
				MerchantData merchantData = _caravanData[item2];
				if (merchantData.MerchantType == item.MerchantType)
				{
					flag = false;
					break;
				}
			}
			if (!(item.Level >= 3 && flag))
			{
				continue;
			}
			int num = context.Random.Next(100);
			int num2 = 0;
			if (num <= 10)
			{
				num2 = 2;
			}
			else if (num <= 20)
			{
				num2 = 1;
			}
			MerchantItem config = null;
			foreach (MerchantItem item3 in (IEnumerable<MerchantItem>)Config.Merchant.Instance)
			{
				if (item3.Level == num2 && item3.MerchantType == item.MerchantType)
				{
					config = item3;
					break;
				}
			}
			Location taiwuVillageLocation = DomainManager.Taiwu.GetTaiwuVillageLocation();
			int id = CreateCaravan(config, taiwuVillageLocation);
			DomainManager.Extra.AddCaravanStayDays(id, 90, context);
			int CreateCaravan(MerchantItem merchantItem, Location end)
			{
				short areaIdByAreaTemplateId2 = DomainManager.Map.GetAreaIdByAreaTemplateId(Config.MerchantType.Instance[merchantItem.MerchantType].HeadArea);
				Location location = new Location(areaIdByAreaTemplateId2, DomainManager.Map.GetElement_Areas(areaIdByAreaTemplateId2).SettlementInfos[0].BlockId);
				int nextCaravanId = GetNextCaravanId();
				MerchantData merchantData2 = new MerchantData(-1, merchantItem.TemplateId);
				SetNextCaravanId(nextCaravanId + 1, context);
				merchantData2.Money = needMoney;
				merchantData2.GenerateGoods(context, nextCaravanId);
				AddElement_CaravanData(nextCaravanId, merchantData2, context);
				List<(Location, short)> list = DomainManager.Map.CalcBlockTravelRoute(context.Random, location, end);
				list.Insert(0, (location, 0));
				AddElement_CaravanDict(nextCaravanId, CreateCaravanPath(list), context);
				CreateCaravanExtraData(context, nextCaravanId);
				MonthlyNotificationCollection monthlyNotificationCollection = DomainManager.World.GetMonthlyNotificationCollection();
				Settlement settlementByLocation = DomainManager.Organization.GetSettlementByLocation(location);
				monthlyNotificationCollection.AddMerchantGoTravelling(settlementByLocation.GetId(), merchantItem.MerchantType);
				return nextCaravanId;
			}
		}
	}

	private CaravanPath CreateCaravanPath(List<(Location location, short cost)> path)
	{
		CaravanPath caravanPath = new CaravanPath();
		int num = 15;
		Location item = path[0].location;
		caravanPath.FullPath.Add(path[0].location);
		caravanPath.MoveNodes.Add(0);
		for (int i = 1; i < path.Count; i++)
		{
			(Location, short) tuple = path[i];
			short areaId = tuple.Item1.AreaId;
			caravanPath.FullPath.Add(tuple.Item1);
			MapBlockData block = DomainManager.Map.GetBlock(tuple.Item1);
			MapBlockData block2 = DomainManager.Map.GetBlock(item);
			if (block.IsCityTown() && !block2.IsCityTown())
			{
				num = 15;
				caravanPath.MoveNodes.Add(i);
			}
			else
			{
				num -= tuple.Item2;
				while (num <= 0)
				{
					num += 15;
					caravanPath.MoveNodes.Add((num > 0) ? i : ((areaId == item.AreaId) ? (i - 1) : (-1)));
				}
			}
			item = tuple.Item1;
		}
		if (caravanPath.MoveNodes[caravanPath.MoveNodes.Count - 1] != path.Count - 1)
		{
			caravanPath.MoveNodes.Add(path.Count - 1);
		}
		caravanPath.MoveWaitDays = 30;
		return caravanPath;
	}

	private void ReCalcCaravanPath(DataContext context)
	{
		foreach (int key in _caravanDict.Keys)
		{
			CaravanPath caravanPath = _caravanDict[key];
			if (caravanPath.MoveNodes.Count <= 1 || caravanPath.MoveWaitDays > 30)
			{
				continue;
			}
			Location currLocation = caravanPath.GetCurrLocation();
			Location destLocation = caravanPath.GetDestLocation();
			int num = caravanPath.FullPath.IndexOf(currLocation);
			List<Location> list = ((num - 1 > 0) ? caravanPath.FullPath.Take(num - 1).ToList() : null);
			List<(Location, short)> list2 = DomainManager.Map.CalcBlockTravelRoute(context.Random, currLocation, destLocation);
			list2.Insert(0, (currLocation, 0));
			CaravanPath caravanPath2 = CreateCaravanPath(list2);
			if (list != null)
			{
				caravanPath2.FullPath.InsertRange(0, list);
				for (int i = 0; i < caravanPath2.MoveNodes.Count; i++)
				{
					if (caravanPath2.MoveNodes[i] >= 0)
					{
						caravanPath2.MoveNodes[i] += list.Count;
					}
				}
			}
			SetElement_CaravanDict(key, caravanPath2, context);
			CaravanExtraData caravanExtraData = CreateCaravanExtraData(context, key);
			List<short> settlementIdList = caravanExtraData.SettlementIdList;
			if (settlementIdList != null && settlementIdList.Count > 0)
			{
				RefreshCaravanExtraDataSettlementIdList(key, ref caravanExtraData.SettlementIdList);
				DomainManager.Extra.SetCaravanExtraData(context, key, caravanExtraData);
			}
		}
	}

	private CaravanExtraData CreateCaravanExtraData(DataContext context, int caravanId)
	{
		if (DomainManager.Extra.TryGetCaravanExtraData(caravanId, out var caravanExtraData))
		{
			return caravanExtraData;
		}
		caravanExtraData = new CaravanExtraData();
		short num = GlobalConfig.Instance.CaravanIncomeCriticalResultRange.First();
		short num2 = GlobalConfig.Instance.CaravanIncomeCriticalResultRange.Last();
		caravanExtraData.IncomeCriticalResult = (short)context.Random.Next((int)num, (int)num2);
		RefreshCaravanExtraDataSettlementIdList(caravanId, ref caravanExtraData.SettlementIdList);
		DomainManager.Extra.SetCaravanExtraData(context, caravanId, caravanExtraData);
		return caravanExtraData;
	}

	private void RefreshCaravanExtraDataSettlementIdList(int caravanId, ref List<short> settlementIdList)
	{
		CaravanPath caravanPath = _caravanDict[caravanId];
		if (settlementIdList == null)
		{
			settlementIdList = new List<short>();
		}
		settlementIdList.Clear();
		for (int i = 1; i < caravanPath.MoveNodes.Count; i++)
		{
			int index = caravanPath.MoveNodes[i];
			if (!caravanPath.FullPath.CheckIndex(index))
			{
				continue;
			}
			Location location = caravanPath.FullPath[index];
			MapBlockData belongSettlementBlock = DomainManager.Map.GetBelongSettlementBlock(location);
			if (belongSettlementBlock != null)
			{
				short num = DomainManager.Organization.GetSettlementByLocation(belongSettlementBlock.GetLocation())?.GetId() ?? (-1);
				if (num >= 0 && !settlementIdList.Contains(num))
				{
					settlementIdList.Add(num);
				}
			}
		}
	}

	public void CaravanMonthEvent(DataContext context)
	{
		Location taiwuVillageLocation = DomainManager.Taiwu.GetTaiwuVillageLocation();
		if (!DomainManager.Map.GetElement_Areas(taiwuVillageLocation.AreaId).StationUnlocked || !DomainManager.World.GetWorldFunctionsStatus(4))
		{
			return;
		}
		EventArgBox globalEventArgumentBox = DomainManager.TaiwuEvent.GetGlobalEventArgumentBox();
		bool arg = false;
		globalEventArgumentBox.Get("CS_PK_CaravanVisit", ref arg);
		if (arg)
		{
			return;
		}
		int arg2 = int.MaxValue;
		if (!globalEventArgumentBox.Get("CS_PK_StationOpenDate", ref arg2))
		{
			return;
		}
		int currDate = DomainManager.World.GetCurrDate();
		if (currDate >= arg2 + 6)
		{
			int monthEventCaravanId = GetMonthEventCaravanId(context);
			if (monthEventCaravanId >= 0)
			{
				DomainManager.World.GetMonthlyEventCollection().AddMerchantVisit();
			}
		}
	}

	public int GetMonthEventCaravanId(DataContext context)
	{
		Location taiwuVillageLocation = DomainManager.Taiwu.GetTaiwuVillageLocation();
		sbyte stateTemplateIdByAreaId = DomainManager.Map.GetStateTemplateIdByAreaId(taiwuVillageLocation.AreaId);
		sbyte mainAreaID = MapState.Instance[stateTemplateIdByAreaId].MainAreaID;
		int goodCount = 2;
		int goodIndex = 1;
		sbyte merchantType = -1;
		if (mainAreaID == 11)
		{
			merchantType = (sbyte)((context.Random.Next(0, 2) != 0) ? 1 : 0);
		}
		else
		{
			short index = MapArea.Instance[mainAreaID].SettlementBlockCore[0];
			List<short> presetBuildingList = MapBlock.Instance[index].PresetBuildingList;
			for (int i = 0; i < presetBuildingList.Count; i++)
			{
				if (presetBuildingList[i] >= 274 && presetBuildingList[i] <= 280)
				{
					BuildingBlockItem buildingBlockItem = BuildingBlock.Instance[presetBuildingList[i]];
					merchantType = buildingBlockItem.MerchantId;
					break;
				}
			}
		}
		int num = DomainManager.Merchant.TryGetCaravanIdByTypeAndLevel(merchantType, 3, goodIndex, goodCount);
		if (num < 0)
		{
			for (int j = 1; j < 6; j++)
			{
				for (sbyte b = 0; b <= 6; b++)
				{
					num = DomainManager.Merchant.TryGetCaravanIdByTypeAndLevel(merchantType, b, j, goodCount);
					if (num >= 0)
					{
						MerchantData caravanMerchantData = DomainManager.Merchant.GetCaravanMerchantData(context, num);
						if (caravanMerchantData != null)
						{
							break;
						}
					}
				}
				if (num >= 0)
				{
					break;
				}
			}
		}
		return num;
	}

	public void UpdateCaravansMove(DataContext context)
	{
		bool flag = false;
		Location location = DomainManager.Taiwu.GetTaiwu().GetLocation();
		int num = (location.IsValid() ? DomainManager.Map.GetStateIdByAreaId(location.AreaId) : (-1));
		foreach (int key2 in _caravanDict.Keys)
		{
			if (!DomainManager.Extra.TryGetCaravanExtraData(key2, out var caravanExtraData))
			{
				continue;
			}
			MerchantData merchantData = _caravanData[key2];
			CaravanPath caravanPath = _caravanDict[key2];
			switch ((CaravanState)caravanExtraData.State)
			{
			case CaravanState.Robbed:
				ApplyMerchantRobbedResult(context, key2, win: false);
				AddInvestedCaravanRobbedMonthlyNotification(caravanPath.GetCurrLocation(), caravanExtraData, merchantData, finished: true);
				continue;
			case CaravanState.RobEnd:
				caravanExtraData.State = 0;
				DomainManager.Extra.SetCaravanExtraData(context, key2, caravanExtraData);
				continue;
			}
			Location destLocation = caravanPath.GetDestLocation();
			MapBlockData rootBlock = DomainManager.Map.GetBlock(destLocation).GetRootBlock();
			Settlement settlementByLocation = DomainManager.Organization.GetSettlementByLocation(rootBlock.GetLocation());
			if (DomainManager.Map.GetStateIdByAreaId(caravanPath.GetCurrLocation().AreaId) == num)
			{
				flag = true;
			}
			if (caravanPath.MoveNodes.Count > 0)
			{
				int num2 = caravanPath.MoveNodes.First();
				for (int i = 1; i < caravanPath.MoveNodes.Count; i++)
				{
					int num3 = caravanPath.MoveNodes[i];
					if (num3 >= 0)
					{
						if (num2 > num3)
						{
							Logger.Warn($"caravan {key2} path has error");
							break;
						}
						num2 = num3;
					}
				}
				caravanPath.MoveWaitDays -= 30;
				if (caravanPath.MoveNodes.Count == 1 && DomainManager.Extra.TryGetCaravanStayDays(key2, out var days))
				{
					caravanPath.MoveWaitDays = Convert.ToInt16(days - 30);
					DomainManager.Extra.SetCaravanStayDays(key2, caravanPath.MoveWaitDays, context);
				}
				if (caravanPath.MoveWaitDays > 0)
				{
					continue;
				}
				caravanPath.MoveWaitDays += 30;
				caravanPath.MoveNodes.RemoveAt(0);
				if (caravanPath.MoveNodes.Count > 0)
				{
					int index = caravanPath.MoveNodes.First();
					if (!caravanPath.FullPath.CheckIndex(index))
					{
						continue;
					}
					Location key = caravanPath.FullPath[index];
					MapBlockData rootBlock2 = DomainManager.Map.GetBlock(key).GetRootBlock();
					Location location2 = rootBlock2.GetLocation();
					if (rootBlock2.IsCityTown())
					{
						Settlement settlementByLocation2 = DomainManager.Organization.GetSettlementByLocation(location2);
						short num4 = settlementByLocation2?.GetId() ?? (-1);
						if (num4 >= 0 && caravanExtraData.SettlementIdList != null && caravanExtraData.SettlementIdList.Contains(num4))
						{
							caravanExtraData.SettlementIdList.Remove(num4);
							short culture = settlementByLocation2.GetCulture();
							bool flag2 = culture > 50;
							bool flag3 = culture < 50;
							bool hasChangedIncomeBonus = false;
							bool hasChangedIncomeCriticalRate = false;
							if (flag2)
							{
								short num5 = (short)Math.Clamp(caravanExtraData.IncomeBonus + Convert.ToInt16(culture * 5 - 250), 0, 32767);
								if (caravanExtraData.IncomeBonus != num5)
								{
									hasChangedIncomeBonus = true;
									caravanExtraData.IncomeBonus = num5;
								}
							}
							else if (flag3)
							{
								short num6 = (short)Math.Clamp(caravanExtraData.IncomeCriticalRate + Convert.ToInt16(200 - 4 * culture), 0, 1000);
								if (caravanExtraData.IncomeCriticalRate != num6)
								{
									hasChangedIncomeCriticalRate = true;
									caravanExtraData.IncomeCriticalRate = num6;
								}
							}
							short safety = settlementByLocation2.GetSafety();
							bool isHighSafety = safety > 50;
							bool isLowSafety = safety < 50;
							bool hasChangedRobbedRate = false;
							short num7 = (short)Math.Clamp(caravanExtraData.RobbedRate + Convert.ToInt16(200 - 4 * safety), 0, 1000);
							if (caravanExtraData.RobbedRate != num7)
							{
								hasChangedRobbedRate = true;
								caravanExtraData.RobbedRate = num7;
							}
							DomainManager.Extra.SetCaravanExtraData(context, key2, caravanExtraData);
							AddInvestedCaravanPassSettlementMonthlyNotification(isLowSafety, isHighSafety, hasChangedRobbedRate, hasChangedIncomeBonus, hasChangedIncomeCriticalRate, num4, settlementByLocation.GetId(), caravanExtraData, merchantData, caravanPath);
						}
					}
					else
					{
						int num8 = context.Random.Next(1000);
						int caravanRobbedRate = SharedMethods.GetCaravanRobbedRate(caravanExtraData.RobbedRate, MapAreaData.IsBrokenArea(location2.AreaId));
						if (num8 < caravanRobbedRate)
						{
							caravanExtraData.State = 1;
							DomainManager.Extra.SetCaravanExtraData(context, key2, caravanExtraData);
							AddInvestedCaravanRobbedMonthlyNotification(location2, caravanExtraData, merchantData, finished: false);
						}
					}
				}
			}
			if (caravanPath.MoveNodes.Count == 0)
			{
				OnCaravanArrive(context, key2, caravanExtraData, merchantData, settlementByLocation.GetId());
			}
			else
			{
				SetElement_CaravanDict(key2, caravanPath, context);
			}
		}
		if (!DomainManager.Map.IsTraveling && flag)
		{
			RefreshCaravanInTaiwuState(context);
		}
	}

	private void AddInvestedCaravanPassSettlementMonthlyNotification(bool isLowSafety, bool isHighSafety, bool hasChangedRobbedRate, bool hasChangedIncomeBonus, bool hasChangedIncomeCriticalRate, short curSettlementId, short targetSettlementId, CaravanExtraData extraData, MerchantData merchantData, CaravanPath caravanPath)
	{
		if (!extraData.IsInvested)
		{
			return;
		}
		MonthlyNotificationCollection monthlyNotificationCollection = DomainManager.World.GetMonthlyNotificationCollection();
		int value = extraData.RobbedRate / 10;
		int num = extraData.IncomeBonus / 10;
		int num2 = extraData.IncomeCriticalRate / 10;
		int value2 = caravanPath.MoveNodes.Count - 1;
		if (hasChangedRobbedRate)
		{
			if (isHighSafety)
			{
				if (hasChangedIncomeBonus)
				{
					monthlyNotificationCollection.AddInvestedCaravanPassHighSafetyHighCultureSettlement(merchantData.MerchantTemplateId, curSettlementId, value2, targetSettlementId, value, num);
				}
				else if (hasChangedIncomeCriticalRate)
				{
					monthlyNotificationCollection.AddInvestedCaravanPassHighSafetyLowCultureSettlement(merchantData.MerchantTemplateId, curSettlementId, value2, targetSettlementId, value, num2);
				}
				else
				{
					monthlyNotificationCollection.AddInvestedCaravanPassHighSafetySettlement(merchantData.MerchantTemplateId, curSettlementId, value2, targetSettlementId, value);
				}
			}
			else if (isLowSafety)
			{
				if (hasChangedIncomeBonus)
				{
					monthlyNotificationCollection.AddInvestedCaravanPassLowSafetyHighCultureSettlement(merchantData.MerchantTemplateId, curSettlementId, value2, targetSettlementId, value, num);
				}
				else if (hasChangedIncomeCriticalRate)
				{
					monthlyNotificationCollection.AddInvestedCaravanPassLowSafetyLowCultureSettlement(merchantData.MerchantTemplateId, curSettlementId, value2, targetSettlementId, value, num2);
				}
				else
				{
					monthlyNotificationCollection.AddInvestedCaravanPassLowSafetySettlement(merchantData.MerchantTemplateId, curSettlementId, value2, targetSettlementId, value);
				}
			}
		}
		else if (hasChangedIncomeBonus)
		{
			monthlyNotificationCollection.AddInvestedCaravanPassHighCultureSettlement(merchantData.MerchantTemplateId, curSettlementId, value2, targetSettlementId, num);
		}
		else if (hasChangedIncomeCriticalRate)
		{
			monthlyNotificationCollection.AddInvestedCaravanPassLowCultureSettlement(merchantData.MerchantTemplateId, curSettlementId, value2, targetSettlementId, num2);
		}
		else
		{
			monthlyNotificationCollection.AddInvestedCaravanPassSettlement(merchantData.MerchantTemplateId, curSettlementId, value2, targetSettlementId);
		}
	}

	private void AddInvestedCaravanRobbedMonthlyNotification(Location location, CaravanExtraData extraData, MerchantData merchantData, bool finished)
	{
		if (extraData.IsInvested)
		{
			MonthlyNotificationCollection monthlyNotificationCollection = DomainManager.World.GetMonthlyNotificationCollection();
			if (!finished)
			{
				monthlyNotificationCollection.AddInvestedCaravanIsRobbed(merchantData.MerchantTemplateId, location);
			}
			else
			{
				monthlyNotificationCollection.AddInvestedCaravanIsRobbedAndFailed(merchantData.MerchantTemplateId, location, extraData.IncomeBonus / 10);
			}
		}
	}

	public void ApplyMerchantRobbedResult(DataContext context, int caravanId, bool win, bool refresh = false)
	{
		if (DomainManager.Extra.TryGetCaravanExtraData(caravanId, out var caravanExtraData))
		{
			if (win)
			{
				MerchantData caravanMerchantData = GetCaravanMerchantData(context, caravanId);
				int delta = GlobalConfig.Instance.CaravanRobbedEventWinAddMerchantFavorability[caravanMerchantData.MerchantLevel];
				ChangeMerchantCumulativeMoney(context, caravanMerchantData.MerchantType, delta);
			}
			else
			{
				short caravanRobbedEventLoseReduceIncomeBonus = GlobalConfig.Instance.CaravanRobbedEventLoseReduceIncomeBonus;
				caravanExtraData.IncomeBonus = Convert.ToInt16(caravanExtraData.IncomeBonus * (100 - caravanRobbedEventLoseReduceIncomeBonus) / 100);
			}
			sbyte caravanRobbedEventEndReduceRobbedRate = GlobalConfig.Instance.CaravanRobbedEventEndReduceRobbedRate;
			caravanExtraData.RobbedRate = Convert.ToInt16(caravanExtraData.RobbedRate * (100 - caravanRobbedEventEndReduceRobbedRate) / 100);
			caravanExtraData.State = 2;
			DomainManager.Extra.SetCaravanExtraData(context, caravanId, caravanExtraData);
			if (refresh)
			{
				RefreshCaravanInTaiwuState(context);
			}
		}
	}

	public void RefreshCaravanInTaiwuState(DataContext context)
	{
		Location location = DomainManager.Taiwu.GetTaiwu().GetLocation();
		if (!location.IsValid())
		{
			return;
		}
		sbyte stateIdByAreaId = DomainManager.Map.GetStateIdByAreaId(location.AreaId);
		List<CaravanDisplayData> list = new List<CaravanDisplayData>();
		foreach (int key in _caravanDict.Keys)
		{
			CaravanPath caravanPath = _caravanDict[key];
			Location currLocation = caravanPath.GetCurrLocation();
			if (DomainManager.Map.GetStateIdByAreaId(currLocation.AreaId) == stateIdByAreaId)
			{
				CaravanDisplayData caravanDisplayData = GetCaravanDisplayData(context, key);
				list.Add(caravanDisplayData);
			}
		}
		list = list.OrderByDescending(delegate(CaravanDisplayData d)
		{
			CaravanState? caravanState = d.ExtraData?.StateEnum;
			return caravanState.HasValue && caravanState == CaravanState.Robbed;
		}).ToList();
		GameData.GameDataBridge.GameDataBridge.AddDisplayEvent(DisplayEventType.RefreshCaravanData, list);
	}

	[DomainMethod]
	public CaravanDisplayData GetCaravanDisplayData(DataContext context, int caravanId)
	{
		if (!_caravanDict.TryGetValue(caravanId, out var value))
		{
			return null;
		}
		short merchantTemplateId = _caravanData[caravanId].MerchantTemplateId;
		MerchantItem merchantItem = Config.Merchant.Instance[merchantTemplateId];
		if (!DomainManager.Extra.TryGetCaravanExtraData(caravanId, out var caravanExtraData))
		{
			caravanExtraData = CreateCaravanExtraData(context, caravanId);
		}
		CaravanDisplayData caravanDisplayData = new CaravanDisplayData
		{
			CaravanId = caravanId,
			MerchantTemplateId = merchantTemplateId,
			TargetArea = value.GetDestLocation().AreaId,
			Favorability = DomainManager.Merchant.GetCurFavorability(merchantItem.MerchantType),
			PathInArea = value.GetRemainCaravanPathInCurrentArea(),
			ExtraData = caravanExtraData
		};
		List<short> settlementIdList = caravanExtraData.SettlementIdList;
		if (settlementIdList != null && settlementIdList.Count > 0)
		{
			caravanDisplayData.SettlementDisplayDataList = new List<SettlementDisplayData>();
			foreach (short settlementId in caravanExtraData.SettlementIdList)
			{
				SettlementDisplayData displayData = DomainManager.Organization.GetDisplayData(settlementId);
				caravanDisplayData.SettlementDisplayDataList.Add(displayData);
			}
		}
		return caravanDisplayData;
	}

	private void OnCaravanArrive(DataContext context, int caravanId, CaravanExtraData extraData, MerchantData merchantData, short targetSettlementId)
	{
		short num = extraData?.IncomeCriticalRate ?? 0;
		short num2 = extraData?.IncomeBonus ?? 1000;
		short num3 = extraData?.IncomeCriticalResult ?? 100;
		int num4 = context.Random.Next(1000);
		bool flag = num4 < num;
		int num5 = merchantData.Money * num2 / 1000;
		if (flag)
		{
			num5 = num5 * num3 / 100;
		}
		int merchantMoney = GetMerchantMoney(context, merchantData.MerchantType);
		merchantMoney += num5;
		SetMerchantMoney(context, merchantData.MerchantType, merchantMoney);
		if (extraData != null && extraData.IsInvested)
		{
			int num6 = GlobalConfig.Instance.InvestCaravanNeedMoney[merchantData.MerchantLevel];
			int num7 = num6 * num2 / 1000;
			if (flag)
			{
				num7 = num7 * num3 / 100;
			}
			DomainManager.Taiwu.GetTaiwu().ChangeResource(context, 6, num7);
			MonthlyNotificationCollection monthlyNotificationCollection = DomainManager.World.GetMonthlyNotificationCollection();
			monthlyNotificationCollection.AddInvestedCaravanArrive(merchantData.MerchantTemplateId, targetSettlementId, num7);
			int baseDelta = ProfessionFormulaImpl.Calculate(98, num6, num7);
			DomainManager.Extra.ChangeProfessionSeniority(context, 15, baseDelta);
		}
		merchantData.RemoveAllGoods(context);
		RemoveElement_CaravanData(caravanId, context);
		RemoveElement_CaravanDict(caravanId, context);
		if (extraData != null)
		{
			DomainManager.Extra.RemoveCaravanExtraData(context, caravanId);
		}
		if (DomainManager.Extra.TryGetCaravanStayDays(caravanId, out var _))
		{
			DomainManager.Extra.RemoveCaravanStayDays(caravanId, context);
		}
		if (DomainManager.Extra.TryGetMerchantExtraGoods(caravanId, out var _))
		{
			DomainManager.Extra.RemoveMerchantExtraGoods(context, caravanId);
		}
	}

	[DomainMethod]
	public void InvestCaravan(DataContext context, int caravanId)
	{
		_caravanData.TryGetValue(caravanId, out var value);
		Tester.Assert(value != null);
		CaravanPath caravanPath = _caravanDict[caravanId];
		Tester.Assert(caravanPath.FullPath.Count >= 1);
		DomainManager.Extra.TryGetCaravanExtraData(caravanId, out var caravanExtraData);
		int num = GlobalConfig.Instance.InvestCaravanNeedMoney[value.MerchantLevel];
		GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
		int resource = taiwu.GetResource(6);
		Tester.Assert(num <= resource);
		taiwu.ChangeResource(context, 6, -num);
		caravanExtraData.IsInvested = true;
		DomainManager.Extra.SetCaravanExtraData(context, caravanId, caravanExtraData);
	}

	[DomainMethod]
	public void ProtectCaravan(DataContext context, int caravanId)
	{
		_caravanData.TryGetValue(caravanId, out var value);
		Tester.Assert(value != null);
		DomainManager.Extra.TryGetCaravanExtraData(caravanId, out var caravanExtraData);
		int num = GlobalConfig.Instance.InvestedCaravanAvoidRobbedNeedAuthorityFactor[value.MerchantLevel];
		int num2 = num * caravanExtraData.RobbedRate / 2;
		GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
		int resource = taiwu.GetResource(7);
		Tester.Assert(num2 <= resource);
		int currDate = DomainManager.World.GetCurrDate();
		taiwu.ChangeResource(context, 7, -num2);
		caravanExtraData.RobbedRate /= 2;
		DomainManager.Extra.SetCaravanExtraData(context, caravanId, caravanExtraData);
		DomainManager.Extra.SetProtectCaravanTime(currDate, context);
	}

	private void CreateTempCaravan(DataContext context, sbyte merchantType, sbyte merchantLevel)
	{
		if (!TryGetElement_CaravanData(-1, out var value))
		{
			AddElement_CaravanData(-1, value, context);
		}
		_tempMerchantBuyBackData?.RemoveAllGoods(context);
		value?.RemoveAllGoods(context);
		value = new MerchantData(-1, MerchantData.FindMerchantTemplateId(merchantType, merchantLevel));
		_caravanData[-1] = value;
		value.GenerateGoods(context);
		value.Money = GetMerchantMoney(context, value.MerchantType);
		SetElement_CaravanData(-1, value, context);
	}

	public void StartTempCaravanAction(sbyte merchantType, sbyte merchantLevel, bool refresh, bool ignoreWorldProgress, bool ignoreFavorability, bool isOpenedByProfessionSkill)
	{
		_totalBuyMoney = 0;
		if (TryGetElement_CaravanData(-1, out var value))
		{
			MerchantItem merchantItem = Config.Merchant.Instance[value.MerchantTemplateId];
			if (refresh || value.MerchantType != merchantType || merchantItem.Level != merchantLevel)
			{
				CreateTempCaravan(DomainManager.TaiwuEvent.MainThreadDataContext, merchantType, merchantLevel);
			}
		}
		else
		{
			CreateTempCaravan(DomainManager.TaiwuEvent.MainThreadDataContext, merchantType, merchantLevel);
		}
		OpenShopEventArguments.EMerchantSourceType eMerchantSourceType = (isOpenedByProfessionSkill ? OpenShopEventArguments.EMerchantSourceType.ProfessionSkillCaravan : OpenShopEventArguments.EMerchantSourceType.AdventureCaravan);
		OpenShopEventArguments arg = new OpenShopEventArguments
		{
			Id = -1,
			Refresh = refresh,
			IgnoreWorldProgress = ignoreWorldProgress,
			IgnoreFavorability = ignoreFavorability,
			BuildingMerchantType = -1,
			MerchantSourceType = (sbyte)eMerchantSourceType
		};
		GameData.GameDataBridge.GameDataBridge.AddDisplayEvent(DisplayEventType.OpenShop, arg);
	}

	public void StartSpecificCharIdAndMerchantTypeAction(int charId, sbyte merchantType, sbyte merchantLevel, bool refresh)
	{
		_totalBuyMoney = 0;
		DataContext mainThreadDataContext = DomainManager.TaiwuEvent.MainThreadDataContext;
		if (merchantType == 8)
		{
			MerchantData value = _merchantData.Values.FirstOrDefault((MerchantData mct) => mct.MerchantType == merchantType);
			if (value != null && value.CharId != charId)
			{
				RemoveElement_MerchantData(value.CharId, mainThreadDataContext);
				value.CharId = charId;
				if (_merchantData.ContainsKey(charId))
				{
					SetElement_MerchantData(charId, value, mainThreadDataContext);
				}
				else
				{
					AddElement_MerchantData(charId, value, mainThreadDataContext);
				}
			}
			else if (value == null)
			{
				bool flag = TryGetElement_MerchantData(charId, out value);
				sbyte merchantTemplateId = MerchantData.FindMerchantTemplateId(merchantType, merchantLevel);
				if (!flag)
				{
					value = new MerchantData(charId, merchantTemplateId);
				}
				else
				{
					value.MerchantTemplateId = merchantTemplateId;
				}
				value.GenerateGoods(mainThreadDataContext);
				value.Money = 0;
				if (flag)
				{
					SetElement_MerchantData(charId, value, mainThreadDataContext);
				}
				else
				{
					AddElement_MerchantData(charId, value, mainThreadDataContext);
				}
			}
		}
		else
		{
			MerchantData value;
			bool flag2 = TryGetElement_MerchantData(charId, out value);
			sbyte b = MerchantData.FindMerchantTemplateId(merchantType, merchantLevel);
			if (!flag2)
			{
				value = new MerchantData(charId, b);
				value.GenerateGoods(mainThreadDataContext);
				value.Money = 0;
			}
			else if (value.MerchantTemplateId != b)
			{
				value.MerchantTemplateId = b;
				value.GenerateGoods(mainThreadDataContext);
				value.Money = 0;
			}
			if (flag2)
			{
				SetElement_MerchantData(charId, value, mainThreadDataContext);
			}
			else
			{
				AddElement_MerchantData(charId, value, mainThreadDataContext);
			}
		}
		short settlementId = DomainManager.Character.GetAliveOrgDeadCharacterOrgInfo(charId).SettlementId;
		bool ignoreFavorability = false;
		if (DomainManager.Organization.TryGetElement_Sects(settlementId, out var element))
		{
			ignoreFavorability = element.CalcApprovingRate() >= 800;
		}
		OpenShopEventArguments arg = new OpenShopEventArguments
		{
			Id = charId,
			Refresh = refresh,
			IgnoreWorldProgress = false,
			IgnoreFavorability = ignoreFavorability,
			MerchantSourceType = 8,
			SettlementId = settlementId
		};
		GameData.GameDataBridge.GameDataBridge.AddDisplayEvent(DisplayEventType.OpenShop, arg);
	}

	public void StartBuildingShopAction(OpenShopEventArguments.EMerchantSourceType merchantSourceType, sbyte merchantType)
	{
		OpenShopEventArguments arg = new OpenShopEventArguments
		{
			BuildingMerchantType = merchantType,
			MerchantSourceType = (sbyte)merchantSourceType
		};
		GameData.GameDataBridge.GameDataBridge.AddDisplayEvent(DisplayEventType.OpenShop, arg);
	}

	public void ClearTempCaravan(DataContext context)
	{
		if (TryGetElement_CaravanData(-1, out var value))
		{
			value.RemoveAllGoods(context);
			RemoveElement_CaravanData(-1, context);
		}
	}

	public void SetCaravanData(int caravanId, MerchantData merchantData, DataContext context)
	{
		SetElement_CaravanData(caravanId, merchantData, context);
	}

	[DomainMethod]
	public List<int> GetTaiwuLocationMaxLevelCaravanIdList()
	{
		Location location = DomainManager.Taiwu.GetTaiwu().GetLocation();
		if (!location.IsValid())
		{
			return null;
		}
		List<int> list = new List<int>();
		foreach (KeyValuePair<int, CaravanPath> item2 in _caravanDict)
		{
			item2.Deconstruct(out var key, out var value);
			int item = key;
			CaravanPath caravanPath = value;
			Location currLocation = caravanPath.GetCurrLocation();
			if (!(currLocation != location))
			{
				list.Add(item);
			}
		}
		return list;
	}

	public bool TryGetFirstTaiwuLocationCaravanId(out int res)
	{
		res = -1;
		Location location = DomainManager.Taiwu.GetTaiwu().GetLocation();
		if (!location.IsValid())
		{
			return false;
		}
		foreach (KeyValuePair<int, CaravanPath> item in _caravanDict)
		{
			item.Deconstruct(out var key, out var value);
			int num = key;
			CaravanPath caravanPath = value;
			Location currLocation = caravanPath.GetCurrLocation();
			if (currLocation == location)
			{
				res = num;
				return true;
			}
		}
		return false;
	}

	private void CorrectCaravanCurrentLocation(int caravanId)
	{
		CaravanPath caravanPath = _caravanDict[caravanId];
		int i;
		for (i = 0; caravanPath.MoveNodes[i] < 0; i++)
		{
		}
		Location location = caravanPath.FullPath[caravanPath.MoveNodes[i]];
		MapBlockData blockData = DomainManager.Map.GetBlockData(location.AreaId, location.BlockId);
		if (!FiveLoongDlcEntry.IsBlockLoongBlock(blockData))
		{
			return;
		}
		List<MapBlockData> list = ObjectPool<List<MapBlockData>>.Instance.Get();
		list.Clear();
		DomainManager.Map.GetNeighborBlocks(location.AreaId, location.BlockId, list, 4);
		foreach (MapBlockData item in list)
		{
			if (FiveLoongDlcEntry.IsBlockLoongBlock(item))
			{
				caravanPath.FullPath[caravanPath.MoveNodes[i]] = new Location(item.AreaId, item.BlockId);
				break;
			}
		}
		ObjectPool<List<MapBlockData>>.Instance.Return(list);
	}

	public int TryGetCaravanIdByTypeAndLevel(sbyte merchantType, sbyte level, int goodIndex, int goodCount)
	{
		foreach (KeyValuePair<int, MerchantData> caravanDatum in _caravanData)
		{
			if (caravanDatum.Value.MerchantType != merchantType || caravanDatum.Value.MerchantConfig.Level != level || caravanDatum.Value.GetGoodsList(goodIndex).Items.Keys.Count < goodCount)
			{
				continue;
			}
			return caravanDatum.Key;
		}
		return -1;
	}

	public void DeleteCaravanItem(DataContext context, int caravanId, int index, ItemKey itemKey)
	{
		MerchantData caravanMerchantData = DomainManager.Merchant.GetCaravanMerchantData(context, caravanId);
		Inventory goodsList = caravanMerchantData.GetGoodsList(index);
		ItemBase baseItem = DomainManager.Item.GetBaseItem(itemKey);
		baseItem.RemoveOwner(ItemOwnerType.Caravan, caravanId);
		goodsList.OfflineRemove(itemKey, 1);
		SetCaravanData(caravanId, caravanMerchantData, context);
	}

	[DomainMethod]
	public void GmCmd_SetCaravanInvested(DataContext context, int caravanId, bool isInvested)
	{
		if (!DomainManager.Extra.TryGetCaravanExtraData(caravanId, out var caravanExtraData))
		{
			caravanExtraData = CreateCaravanExtraData(context, caravanId);
		}
		caravanExtraData.IsInvested = isInvested;
		DomainManager.Extra.SetCaravanExtraData(context, caravanId, caravanExtraData);
	}

	[DomainMethod]
	public void GmCmd_SetAllCaravanInvested(DataContext context, bool isInvested)
	{
		foreach (int key in _caravanData.Keys)
		{
			if (!DomainManager.Extra.TryGetCaravanExtraData(key, out var caravanExtraData))
			{
				caravanExtraData = CreateCaravanExtraData(context, key);
			}
			caravanExtraData.IsInvested = isInvested;
			DomainManager.Extra.SetCaravanExtraData(context, key, caravanExtraData);
		}
	}

	[DomainMethod]
	public void GmCmd_SetCaravanState(DataContext context, int caravanId, sbyte caravanState)
	{
		if (!DomainManager.Extra.TryGetCaravanExtraData(caravanId, out var caravanExtraData))
		{
			caravanExtraData = CreateCaravanExtraData(context, caravanId);
		}
		caravanExtraData.State = caravanState;
		DomainManager.Extra.SetCaravanExtraData(context, caravanId, caravanExtraData);
		RefreshCaravanInTaiwuState(context);
	}

	[DomainMethod]
	public void GmCmd_SetCaravanRobbedRate(DataContext context, int caravanId, short robbedRate)
	{
		if (!DomainManager.Extra.TryGetCaravanExtraData(caravanId, out var caravanExtraData))
		{
			caravanExtraData = CreateCaravanExtraData(context, caravanId);
		}
		caravanExtraData.RobbedRate = (short)Math.Clamp((int)robbedRate, 0, 1000);
		DomainManager.Extra.SetCaravanExtraData(context, caravanId, caravanExtraData);
	}

	[DomainMethod]
	public void GmCmd_SetCaravanIncomeData(DataContext context, int caravanId, short incomeBonus, short incomeCriticalRate, short incomeCriticalResult)
	{
		if (!DomainManager.Extra.TryGetCaravanExtraData(caravanId, out var caravanExtraData))
		{
			caravanExtraData = CreateCaravanExtraData(context, caravanId);
		}
		caravanExtraData.IncomeBonus = (short)Math.Clamp((int)incomeBonus, 0, 1000);
		caravanExtraData.IncomeCriticalRate = (short)Math.Clamp((int)incomeCriticalRate, 0, 1000);
		short min = GlobalConfig.Instance.CaravanIncomeCriticalResultRange.First();
		short max = GlobalConfig.Instance.CaravanIncomeCriticalResultRange.Last();
		caravanExtraData.IncomeCriticalResult = (short)Math.Clamp((int)incomeCriticalResult, (int)min, (int)max);
		DomainManager.Extra.SetCaravanExtraData(context, caravanId, caravanExtraData);
	}

	[DomainMethod]
	public void GmCmd_ProtectCaravan(DataContext context, int caravanId)
	{
		if (DomainManager.Extra.TryGetCaravanExtraData(caravanId, out var caravanExtraData) && caravanExtraData.RobbedRate != 0)
		{
			int currDate = DomainManager.World.GetCurrDate();
			caravanExtraData.RobbedRate /= 2;
			DomainManager.Extra.SetCaravanExtraData(context, caravanId, caravanExtraData);
			DomainManager.Extra.SetProtectCaravanTime(currDate, context);
		}
	}

	[DomainMethod]
	public void GmCmd_ProtectAllCaravan(DataContext context)
	{
		foreach (int key in _caravanData.Keys)
		{
			if (DomainManager.Extra.TryGetCaravanExtraData(key, out var caravanExtraData) && caravanExtraData.RobbedRate != 0)
			{
				caravanExtraData.RobbedRate /= 2;
				DomainManager.Extra.SetCaravanExtraData(context, key, caravanExtraData);
			}
		}
		int currDate = DomainManager.World.GetCurrDate();
		DomainManager.Extra.SetProtectCaravanTime(currDate, context);
	}

	public MerchantDomain()
		: base(7)
	{
		_merchantData = new Dictionary<int, MerchantData>(0);
		_merchantFavorability = new int[7];
		_merchantMoney = new int[7];
		_merchantMaxLevelData = new MerchantData[7];
		_nextCaravanId = 0;
		_caravanData = new Dictionary<int, MerchantData>(0);
		_caravanDict = new Dictionary<int, CaravanPath>(0);
		OnInitializedDomainData();
	}

	private MerchantData GetElement_MerchantData(int elementId)
	{
		return _merchantData[elementId];
	}

	private bool TryGetElement_MerchantData(int elementId, out MerchantData value)
	{
		return _merchantData.TryGetValue(elementId, out value);
	}

	private unsafe void AddElement_MerchantData(int elementId, MerchantData value, DataContext context)
	{
		_merchantData.Add(elementId, value);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(0, DataStates, CacheInfluences, context);
		if (value != null)
		{
			int serializedSize = value.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicSingleValueCollection_Add(14, 0, elementId, serializedSize);
			ptr += value.Serialize(ptr);
		}
		else
		{
			OperationAdder.DynamicSingleValueCollection_Add(14, 0, elementId, 0);
		}
	}

	private unsafe void SetElement_MerchantData(int elementId, MerchantData value, DataContext context)
	{
		_merchantData[elementId] = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(0, DataStates, CacheInfluences, context);
		if (value != null)
		{
			int serializedSize = value.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicSingleValueCollection_Set(14, 0, elementId, serializedSize);
			ptr += value.Serialize(ptr);
		}
		else
		{
			OperationAdder.DynamicSingleValueCollection_Set(14, 0, elementId, 0);
		}
	}

	private void RemoveElement_MerchantData(int elementId, DataContext context)
	{
		_merchantData.Remove(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(0, DataStates, CacheInfluences, context);
		OperationAdder.DynamicSingleValueCollection_Remove(14, 0, elementId);
	}

	private void ClearMerchantData(DataContext context)
	{
		_merchantData.Clear();
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(0, DataStates, CacheInfluences, context);
		OperationAdder.DynamicSingleValueCollection_Clear(14, 0);
	}

	public int[] GetMerchantFavorability()
	{
		return _merchantFavorability;
	}

	public unsafe void SetMerchantFavorability(int[] value, DataContext context)
	{
		_merchantFavorability = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(1, DataStates, CacheInfluences, context);
		byte* ptr = OperationAdder.FixedSingleValue_Set(14, 1, 28);
		for (int i = 0; i < 7; i++)
		{
			((int*)ptr)[i] = _merchantFavorability[i];
		}
		ptr += 28;
	}

	public int[] GetMerchantMoney()
	{
		return _merchantMoney;
	}

	public unsafe void SetMerchantMoney(int[] value, DataContext context)
	{
		_merchantMoney = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(2, DataStates, CacheInfluences, context);
		byte* ptr = OperationAdder.FixedSingleValue_Set(14, 2, 28);
		for (int i = 0; i < 7; i++)
		{
			((int*)ptr)[i] = _merchantMoney[i];
		}
		ptr += 28;
	}

	public MerchantData GetElement_MerchantMaxLevelData(int index)
	{
		return _merchantMaxLevelData[index];
	}

	public unsafe void SetElement_MerchantMaxLevelData(int index, MerchantData value, DataContext context)
	{
		_merchantMaxLevelData[index] = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(index, _dataStatesMerchantMaxLevelData, CacheInfluencesMerchantMaxLevelData, context);
		if (value != null)
		{
			int serializedSize = value.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicElementList_Set(14, 3, index, serializedSize);
			ptr += value.Serialize(ptr);
		}
		else
		{
			OperationAdder.DynamicElementList_Set(14, 3, index, 0);
		}
	}

	private int GetNextCaravanId()
	{
		return _nextCaravanId;
	}

	private unsafe void SetNextCaravanId(int value, DataContext context)
	{
		_nextCaravanId = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(4, DataStates, CacheInfluences, context);
		byte* ptr = OperationAdder.FixedSingleValue_Set(14, 4, 4);
		*(int*)ptr = _nextCaravanId;
		ptr += 4;
	}

	private MerchantData GetElement_CaravanData(int elementId)
	{
		return _caravanData[elementId];
	}

	private bool TryGetElement_CaravanData(int elementId, out MerchantData value)
	{
		return _caravanData.TryGetValue(elementId, out value);
	}

	private unsafe void AddElement_CaravanData(int elementId, MerchantData value, DataContext context)
	{
		_caravanData.Add(elementId, value);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(5, DataStates, CacheInfluences, context);
		if (value != null)
		{
			int serializedSize = value.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicSingleValueCollection_Add(14, 5, elementId, serializedSize);
			ptr += value.Serialize(ptr);
		}
		else
		{
			OperationAdder.DynamicSingleValueCollection_Add(14, 5, elementId, 0);
		}
	}

	private unsafe void SetElement_CaravanData(int elementId, MerchantData value, DataContext context)
	{
		_caravanData[elementId] = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(5, DataStates, CacheInfluences, context);
		if (value != null)
		{
			int serializedSize = value.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicSingleValueCollection_Set(14, 5, elementId, serializedSize);
			ptr += value.Serialize(ptr);
		}
		else
		{
			OperationAdder.DynamicSingleValueCollection_Set(14, 5, elementId, 0);
		}
	}

	private void RemoveElement_CaravanData(int elementId, DataContext context)
	{
		_caravanData.Remove(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(5, DataStates, CacheInfluences, context);
		OperationAdder.DynamicSingleValueCollection_Remove(14, 5, elementId);
	}

	private void ClearCaravanData(DataContext context)
	{
		_caravanData.Clear();
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(5, DataStates, CacheInfluences, context);
		OperationAdder.DynamicSingleValueCollection_Clear(14, 5);
	}

	private CaravanPath GetElement_CaravanDict(int elementId)
	{
		return _caravanDict[elementId];
	}

	private bool TryGetElement_CaravanDict(int elementId, out CaravanPath value)
	{
		return _caravanDict.TryGetValue(elementId, out value);
	}

	private unsafe void AddElement_CaravanDict(int elementId, CaravanPath value, DataContext context)
	{
		_caravanDict.Add(elementId, value);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(6, DataStates, CacheInfluences, context);
		if (value != null)
		{
			int serializedSize = value.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicSingleValueCollection_Add(14, 6, elementId, serializedSize);
			ptr += value.Serialize(ptr);
		}
		else
		{
			OperationAdder.DynamicSingleValueCollection_Add(14, 6, elementId, 0);
		}
	}

	private unsafe void SetElement_CaravanDict(int elementId, CaravanPath value, DataContext context)
	{
		_caravanDict[elementId] = value;
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(6, DataStates, CacheInfluences, context);
		if (value != null)
		{
			int serializedSize = value.GetSerializedSize();
			byte* ptr = OperationAdder.DynamicSingleValueCollection_Set(14, 6, elementId, serializedSize);
			ptr += value.Serialize(ptr);
		}
		else
		{
			OperationAdder.DynamicSingleValueCollection_Set(14, 6, elementId, 0);
		}
	}

	private void RemoveElement_CaravanDict(int elementId, DataContext context)
	{
		_caravanDict.Remove(elementId);
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(6, DataStates, CacheInfluences, context);
		OperationAdder.DynamicSingleValueCollection_Remove(14, 6, elementId);
	}

	private void ClearCaravanDict(DataContext context)
	{
		_caravanDict.Clear();
		BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(6, DataStates, CacheInfluences, context);
		OperationAdder.DynamicSingleValueCollection_Clear(14, 6);
	}

	public override void OnInitializeGameDataModule()
	{
		InitializeOnInitializeGameDataModule();
	}

	public unsafe override void OnEnterNewWorld()
	{
		InitializeOnEnterNewWorld();
		InitializeInternalDataOfCollections();
		foreach (KeyValuePair<int, MerchantData> merchantDatum in _merchantData)
		{
			int key = merchantDatum.Key;
			MerchantData value = merchantDatum.Value;
			if (value != null)
			{
				int serializedSize = value.GetSerializedSize();
				byte* ptr = OperationAdder.DynamicSingleValueCollection_Add(14, 0, key, serializedSize);
				ptr += value.Serialize(ptr);
			}
			else
			{
				OperationAdder.DynamicSingleValueCollection_Add(14, 0, key, 0);
			}
		}
		byte* ptr2 = OperationAdder.FixedSingleValue_Set(14, 1, 28);
		for (int i = 0; i < 7; i++)
		{
			((int*)ptr2)[i] = _merchantFavorability[i];
		}
		ptr2 += 28;
		byte* ptr3 = OperationAdder.FixedSingleValue_Set(14, 2, 28);
		for (int j = 0; j < 7; j++)
		{
			((int*)ptr3)[j] = _merchantMoney[j];
		}
		ptr3 += 28;
		int num = 0;
		for (int k = 0; k < 7; k++)
		{
			MerchantData merchantData = _merchantMaxLevelData[k];
			num = ((merchantData == null) ? (num + 4) : (num + (4 + merchantData.GetSerializedSize())));
		}
		byte* ptr4 = OperationAdder.DynamicElementList_InsertRange(14, 3, 0, 7, num);
		for (int l = 0; l < 7; l++)
		{
			MerchantData merchantData2 = _merchantMaxLevelData[l];
			if (merchantData2 != null)
			{
				byte* ptr5 = ptr4;
				ptr4 += 4;
				int num2 = merchantData2.Serialize(ptr4);
				ptr4 += num2;
				*(int*)ptr5 = num2;
			}
			else
			{
				*(int*)ptr4 = 0;
				ptr4 += 4;
			}
		}
		byte* ptr6 = OperationAdder.FixedSingleValue_Set(14, 4, 4);
		*(int*)ptr6 = _nextCaravanId;
		ptr6 += 4;
		foreach (KeyValuePair<int, MerchantData> caravanDatum in _caravanData)
		{
			int key2 = caravanDatum.Key;
			MerchantData value2 = caravanDatum.Value;
			if (value2 != null)
			{
				int serializedSize2 = value2.GetSerializedSize();
				byte* ptr7 = OperationAdder.DynamicSingleValueCollection_Add(14, 5, key2, serializedSize2);
				ptr7 += value2.Serialize(ptr7);
			}
			else
			{
				OperationAdder.DynamicSingleValueCollection_Add(14, 5, key2, 0);
			}
		}
		foreach (KeyValuePair<int, CaravanPath> item in _caravanDict)
		{
			int key3 = item.Key;
			CaravanPath value3 = item.Value;
			if (value3 != null)
			{
				int serializedSize3 = value3.GetSerializedSize();
				byte* ptr8 = OperationAdder.DynamicSingleValueCollection_Add(14, 6, key3, serializedSize3);
				ptr8 += value3.Serialize(ptr8);
			}
			else
			{
				OperationAdder.DynamicSingleValueCollection_Add(14, 6, key3, 0);
			}
		}
	}

	public override void OnLoadWorld()
	{
		_pendingLoadingOperationIds = new Queue<uint>();
		_pendingLoadingOperationIds.Enqueue(OperationAdder.DynamicSingleValueCollection_GetAll(14, 0));
		_pendingLoadingOperationIds.Enqueue(OperationAdder.FixedSingleValue_Get(14, 1));
		_pendingLoadingOperationIds.Enqueue(OperationAdder.FixedSingleValue_Get(14, 2));
		_pendingLoadingOperationIds.Enqueue(OperationAdder.DynamicElementList_GetAll(14, 3));
		_pendingLoadingOperationIds.Enqueue(OperationAdder.FixedSingleValue_Get(14, 4));
		_pendingLoadingOperationIds.Enqueue(OperationAdder.DynamicSingleValueCollection_GetAll(14, 5));
		_pendingLoadingOperationIds.Enqueue(OperationAdder.DynamicSingleValueCollection_GetAll(14, 6));
	}

	public override int GetData(ushort dataId, ulong subId0, uint subId1, RawDataPool dataPool, bool resetModified)
	{
		switch (dataId)
		{
		case 0:
			throw new Exception($"Not allow to get value of dataId: {dataId}");
		case 1:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 1);
			}
			return GameData.Serializer.Serializer.Serialize(_merchantFavorability, dataPool);
		case 2:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(DataStates, 2);
			}
			return GameData.Serializer.Serializer.Serialize(_merchantMoney, dataPool);
		case 3:
			if (resetModified)
			{
				BaseGameDataDomain.ResetModified(_dataStatesMerchantMaxLevelData, (int)subId0);
			}
			return GameData.Serializer.Serializer.Serialize(_merchantMaxLevelData[(uint)subId0], dataPool);
		case 4:
			throw new Exception($"Not allow to get value of dataId: {dataId}");
		case 5:
			throw new Exception($"Not allow to get value of dataId: {dataId}");
		case 6:
			throw new Exception($"Not allow to get value of dataId: {dataId}");
		default:
			throw new Exception($"Unsupported dataId {dataId}");
		}
	}

	public override void SetData(ushort dataId, ulong subId0, uint subId1, int valueOffset, RawDataPool dataPool, DataContext context)
	{
		switch (dataId)
		{
		case 0:
			throw new Exception($"Not allow to set value of dataId {dataId}");
		case 1:
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref _merchantFavorability);
			SetMerchantFavorability(_merchantFavorability, context);
			break;
		case 2:
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref _merchantMoney);
			SetMerchantMoney(_merchantMoney, context);
			break;
		case 3:
		{
			MerchantData item = _merchantMaxLevelData[(uint)subId0];
			GameData.Serializer.Serializer.Deserialize(dataPool, valueOffset, ref item);
			_merchantMaxLevelData[(uint)subId0] = item;
			SetElement_MerchantMaxLevelData((int)subId0, item, context);
			break;
		}
		case 4:
			throw new Exception($"Not allow to set value of dataId {dataId}");
		case 5:
			throw new Exception($"Not allow to set value of dataId {dataId}");
		case 6:
			throw new Exception($"Not allow to set value of dataId {dataId}");
		default:
			throw new Exception($"Unsupported dataId {dataId}");
		}
	}

	public override int CallMethod(Operation operation, RawDataPool argDataPool, RawDataPool returnDataPool, DataContext context)
	{
		int argsOffset = operation.ArgsOffset;
		switch (operation.MethodId)
		{
		case 0:
		{
			int argsCount24 = operation.ArgsCount;
			int num24 = argsCount24;
			if (num24 == 1)
			{
				int item53 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item53);
				MerchantData merchantData = GetMerchantData(context, item53);
				return GameData.Serializer.Serializer.Serialize(merchantData, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 1:
		{
			int argsCount16 = operation.ArgsCount;
			int num16 = argsCount16;
			if (num16 == 1)
			{
				MerchantTradeArguments item37 = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item37);
				SettleTrade(context, item37);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 2:
		{
			int argsCount3 = operation.ArgsCount;
			int num3 = argsCount3;
			if (num3 == 5)
			{
				int item15 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item15);
				List<ItemDisplayData> item16 = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item16);
				List<ItemDisplayData> item17 = null;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item17);
				int item18 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item18);
				int item19 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item19);
				ExchangeBook(context, item15, item16, item17, item18, item19);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 3:
			if (operation.ArgsCount == 0)
			{
				PullTradeCaravanLocations(context);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		case 4:
		{
			int argsCount19 = operation.ArgsCount;
			int num19 = argsCount19;
			if (num19 == 1)
			{
				int item42 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item42);
				MerchantData caravanMerchantData = GetCaravanMerchantData(context, item42);
				return GameData.Serializer.Serializer.Serialize(caravanMerchantData, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 5:
		{
			int argsCount9 = operation.ArgsCount;
			int num9 = argsCount9;
			if (num9 == 2)
			{
				int item25 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item25);
				bool item26 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item26);
				List<ItemDisplayData> tradeBookDisplayData = GetTradeBookDisplayData(context, item25, item26);
				return GameData.Serializer.Serializer.Serialize(tradeBookDisplayData, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 6:
		{
			int argsCount21 = operation.ArgsCount;
			int num21 = argsCount21;
			if (num21 == 5)
			{
				int item45 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item45);
				sbyte item46 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item46);
				short item47 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item47);
				int item48 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item48);
				int item49 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item49);
				GmCmd_AddItem(context, item45, item46, item47, item48, item49);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 7:
			if (operation.ArgsCount == 0)
			{
				List<int> taiwuLocationMaxLevelCaravanIdList = GetTaiwuLocationMaxLevelCaravanIdList();
				return GameData.Serializer.Serializer.Serialize(taiwuLocationMaxLevelCaravanIdList, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		case 8:
		{
			int argsCount6 = operation.ArgsCount;
			int num6 = argsCount6;
			if (num6 == 1)
			{
				sbyte item22 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item22);
				int curFavorability = GetCurFavorability(item22);
				return GameData.Serializer.Serializer.Serialize(curFavorability, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 9:
		{
			int argsCount23 = operation.ArgsCount;
			int num23 = argsCount23;
			if (num23 == 2)
			{
				int item51 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item51);
				bool item52 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item52);
				FinishBookTrade(context, item51, item52);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 10:
			if (operation.ArgsCount == 0)
			{
				List<ItemDisplayData> tradeBackBookDisplayData = GetTradeBackBookDisplayData();
				return GameData.Serializer.Serializer.Serialize(tradeBackBookDisplayData, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		case 11:
		{
			int argsCount12 = operation.ArgsCount;
			int num12 = argsCount12;
			if (num12 == 1)
			{
				sbyte item32 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item32);
				List<MerchantInfoCaravanData> merchantInfoCaravanDataList = GetMerchantInfoCaravanDataList(context, item32);
				return GameData.Serializer.Serializer.Serialize(merchantInfoCaravanDataList, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 12:
		{
			int argsCount8 = operation.ArgsCount;
			int num8 = argsCount8;
			if (num8 == 1)
			{
				sbyte item24 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item24);
				List<MerchantInfoAreaData> merchantInfoAreaDataList = GetMerchantInfoAreaDataList(item24);
				return GameData.Serializer.Serializer.Serialize(merchantInfoAreaDataList, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 13:
		{
			int argsCount2 = operation.ArgsCount;
			int num2 = argsCount2;
			if (num2 == 1)
			{
				sbyte item14 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item14);
				List<MerchantInfoMerchantData> merchantInfoMerchantDataList = GetMerchantInfoMerchantDataList(item14);
				return GameData.Serializer.Serializer.Serialize(merchantInfoMerchantDataList, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 14:
		{
			int argsCount22 = operation.ArgsCount;
			int num22 = argsCount22;
			if (num22 == 1)
			{
				sbyte item50 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item50);
				MerchantOverFavorData merchantOverFavorData = GetMerchantOverFavorData(item50);
				return GameData.Serializer.Serializer.Serialize(merchantOverFavorData, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 15:
		{
			int argsCount18 = operation.ArgsCount;
			int num18 = argsCount18;
			if (num18 == 2)
			{
				sbyte item40 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item40);
				bool item41 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item41);
				MerchantData buildingMerchantData = GetBuildingMerchantData(context, item40, item41);
				return GameData.Serializer.Serializer.Serialize(buildingMerchantData, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 16:
		{
			int argsCount14 = operation.ArgsCount;
			int num14 = argsCount14;
			if (num14 == 1)
			{
				int item35 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item35);
				CaravanDisplayData caravanDisplayData = GetCaravanDisplayData(context, item35);
				return GameData.Serializer.Serializer.Serialize(caravanDisplayData, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 17:
		{
			int argsCount11 = operation.ArgsCount;
			int num11 = argsCount11;
			if (num11 == 1)
			{
				int item31 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item31);
				InvestCaravan(context, item31);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 18:
			if (operation.ArgsCount == 0)
			{
				int[] allFavorability = GetAllFavorability();
				return GameData.Serializer.Serializer.Serialize(allFavorability, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		case 19:
		{
			int argsCount5 = operation.ArgsCount;
			int num5 = argsCount5;
			if (num5 == 1)
			{
				int item21 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item21);
				ProtectCaravan(context, item21);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 20:
		{
			int argsCount25 = operation.ArgsCount;
			int num25 = argsCount25;
			if (num25 == 1)
			{
				int item54 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item54);
				GmCmd_ProtectCaravan(context, item54);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 21:
			if (operation.ArgsCount == 0)
			{
				GmCmd_ProtectAllCaravan(context);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		case 22:
		{
			int argsCount20 = operation.ArgsCount;
			int num20 = argsCount20;
			if (num20 == 2)
			{
				int item43 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item43);
				short item44 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item44);
				GmCmd_SetCaravanRobbedRate(context, item43, item44);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 23:
		{
			int argsCount17 = operation.ArgsCount;
			int num17 = argsCount17;
			if (num17 == 2)
			{
				int item38 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item38);
				bool item39 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item39);
				GmCmd_SetCaravanInvested(context, item38, item39);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 24:
		{
			int argsCount15 = operation.ArgsCount;
			int num15 = argsCount15;
			if (num15 == 1)
			{
				bool item36 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item36);
				GmCmd_SetAllCaravanInvested(context, item36);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 25:
		{
			int argsCount13 = operation.ArgsCount;
			int num13 = argsCount13;
			if (num13 == 2)
			{
				int item33 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item33);
				sbyte item34 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item34);
				GmCmd_SetCaravanState(context, item33, item34);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 26:
		{
			int argsCount10 = operation.ArgsCount;
			int num10 = argsCount10;
			if (num10 == 4)
			{
				int item27 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item27);
				short item28 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item28);
				short item29 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item29);
				short item30 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item30);
				GmCmd_SetCaravanIncomeData(context, item27, item28, item29, item30);
				return -1;
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 27:
			if (operation.ArgsCount == 0)
			{
				SectStorySpecialMerchant sectStorySpecialMerchantData = GetSectStorySpecialMerchantData(context);
				return GameData.Serializer.Serializer.Serialize(sectStorySpecialMerchantData, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		case 28:
		{
			int argsCount7 = operation.ArgsCount;
			int num7 = argsCount7;
			if (num7 == 1)
			{
				int item23 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item23);
				sbyte merchantTemplateId = GetMerchantTemplateId(item23);
				return GameData.Serializer.Serializer.Serialize(merchantTemplateId, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 29:
		{
			int argsCount4 = operation.ArgsCount;
			int num4 = argsCount4;
			if (num4 == 1)
			{
				OpenShopEventArguments item20 = new OpenShopEventArguments();
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item20);
				MerchantBuyBackData merchantBuyBackData = GetMerchantBuyBackData(item20);
				return GameData.Serializer.Serializer.Serialize(merchantBuyBackData, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 30:
		{
			int argsCount = operation.ArgsCount;
			int num = argsCount;
			if (num == 6)
			{
				int item7 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item7);
				bool item8 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item8);
				sbyte item9 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item9);
				bool item10 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item10);
				bool item11 = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item11);
				sbyte item12 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item12);
				bool item13 = RefreshMerchantGoods(context, item7, item8, item9, item10, item11, item12);
				return GameData.Serializer.Serializer.Serialize(item13, returnDataPool);
			}
			throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
		}
		case 31:
			switch (operation.ArgsCount)
			{
			case 1:
			{
				sbyte item6 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item6);
				int favorabilityWithDelta2 = GetFavorabilityWithDelta(item6);
				return GameData.Serializer.Serializer.Serialize(favorabilityWithDelta2, returnDataPool);
			}
			case 2:
			{
				sbyte item4 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item4);
				int item5 = 0;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item5);
				int favorabilityWithDelta = GetFavorabilityWithDelta(item4, item5);
				return GameData.Serializer.Serializer.Serialize(favorabilityWithDelta, returnDataPool);
			}
			default:
				throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
			}
		case 32:
			switch (operation.ArgsCount)
			{
			case 0:
			{
				bool item3 = CanRefreshMerchantGoods(context);
				return GameData.Serializer.Serializer.Serialize(item3, returnDataPool);
			}
			case 1:
			{
				bool item = false;
				argsOffset += GameData.Serializer.Serializer.Deserialize(argDataPool, argsOffset, ref item);
				bool item2 = CanRefreshMerchantGoods(context, item);
				return GameData.Serializer.Serializer.Serialize(item2, returnDataPool);
			}
			default:
				throw new Exception($"Unsupported argsCount of methodId: {operation.MethodId}");
			}
		default:
			throw new Exception($"Unsupported methodId {operation.MethodId}");
		}
	}

	public override void OnMonitorData(ushort dataId, ulong subId0, uint subId1, bool monitoring)
	{
		switch (dataId)
		{
		case 0:
			return;
		case 1:
			return;
		case 2:
			return;
		case 3:
			return;
		case 4:
			return;
		case 5:
			return;
		case 6:
			return;
		}
		throw new Exception($"Unsupported dataId {dataId}");
	}

	public override int CheckModified(ushort dataId, ulong subId0, uint subId1, RawDataPool dataPool)
	{
		switch (dataId)
		{
		case 0:
			throw new Exception($"Not allow to check modification of dataId {dataId}");
		case 1:
			if (!BaseGameDataDomain.IsModified(DataStates, 1))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 1);
			return GameData.Serializer.Serializer.Serialize(_merchantFavorability, dataPool);
		case 2:
			if (!BaseGameDataDomain.IsModified(DataStates, 2))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(DataStates, 2);
			return GameData.Serializer.Serializer.Serialize(_merchantMoney, dataPool);
		case 3:
			if (!BaseGameDataDomain.IsModified(_dataStatesMerchantMaxLevelData, (int)subId0))
			{
				return -1;
			}
			BaseGameDataDomain.ResetModified(_dataStatesMerchantMaxLevelData, (int)subId0);
			return GameData.Serializer.Serializer.Serialize(_merchantMaxLevelData[(uint)subId0], dataPool);
		case 4:
			throw new Exception($"Not allow to check modification of dataId {dataId}");
		case 5:
			throw new Exception($"Not allow to check modification of dataId {dataId}");
		case 6:
			throw new Exception($"Not allow to check modification of dataId {dataId}");
		default:
			throw new Exception($"Unsupported dataId {dataId}");
		}
	}

	public override void ResetModifiedWrapper(ushort dataId, ulong subId0, uint subId1)
	{
		switch (dataId)
		{
		case 0:
			throw new Exception($"Not allow to reset modification state of dataId {dataId}");
		case 1:
			if (BaseGameDataDomain.IsModified(DataStates, 1))
			{
				BaseGameDataDomain.ResetModified(DataStates, 1);
			}
			break;
		case 2:
			if (BaseGameDataDomain.IsModified(DataStates, 2))
			{
				BaseGameDataDomain.ResetModified(DataStates, 2);
			}
			break;
		case 3:
			if (BaseGameDataDomain.IsModified(_dataStatesMerchantMaxLevelData, (int)subId0))
			{
				BaseGameDataDomain.ResetModified(_dataStatesMerchantMaxLevelData, (int)subId0);
			}
			break;
		case 4:
			throw new Exception($"Not allow to reset modification state of dataId {dataId}");
		case 5:
			throw new Exception($"Not allow to reset modification state of dataId {dataId}");
		case 6:
			throw new Exception($"Not allow to reset modification state of dataId {dataId}");
		default:
			throw new Exception($"Unsupported dataId {dataId}");
		}
	}

	public override bool IsModifiedWrapper(ushort dataId, ulong subId0, uint subId1)
	{
		return dataId switch
		{
			0 => throw new Exception($"Not allow to verify modification state of dataId {dataId}"), 
			1 => BaseGameDataDomain.IsModified(DataStates, 1), 
			2 => BaseGameDataDomain.IsModified(DataStates, 2), 
			3 => BaseGameDataDomain.IsModified(_dataStatesMerchantMaxLevelData, (int)subId0), 
			4 => throw new Exception($"Not allow to verify modification state of dataId {dataId}"), 
			5 => throw new Exception($"Not allow to verify modification state of dataId {dataId}"), 
			6 => throw new Exception($"Not allow to verify modification state of dataId {dataId}"), 
			_ => throw new Exception($"Unsupported dataId {dataId}"), 
		};
	}

	public override void InvalidateCache(BaseGameDataObject sourceObject, DataInfluence influence, DataContext context, bool unconditionallyInfluenceAll)
	{
		switch (influence.TargetIndicator.DataId)
		{
		default:
			throw new Exception($"Unsupported dataId {influence.TargetIndicator.DataId}");
		case 0:
		case 1:
		case 2:
		case 3:
		case 4:
		case 5:
		case 6:
			throw new Exception($"Cannot invalidate cache state of non-cache data {influence.TargetIndicator.DataId}");
		}
	}

	public unsafe override void ProcessArchiveResponse(OperationWrapper operation, byte* pResult)
	{
		switch (operation.DataId)
		{
		case 0:
			ResponseProcessor.ProcessSingleValueCollection_CustomType_Dynamic_Ref(operation, pResult, _merchantData);
			break;
		case 1:
			ResponseProcessor.ProcessSingleValue_BasicType_Fixed_Value_Array(operation, pResult, _merchantFavorability, 7);
			break;
		case 2:
			ResponseProcessor.ProcessSingleValue_BasicType_Fixed_Value_Array(operation, pResult, _merchantMoney, 7);
			break;
		case 3:
			ResponseProcessor.ProcessElementList_CustomType_Dynamic_Ref(operation, pResult, _merchantMaxLevelData, 7);
			break;
		case 4:
			ResponseProcessor.ProcessSingleValue_BasicType_Fixed_Value_Single(operation, pResult, ref _nextCaravanId);
			break;
		case 5:
			ResponseProcessor.ProcessSingleValueCollection_CustomType_Dynamic_Ref(operation, pResult, _caravanData);
			break;
		case 6:
			ResponseProcessor.ProcessSingleValueCollection_CustomType_Dynamic_Ref(operation, pResult, _caravanDict);
			break;
		default:
			throw new Exception($"Unsupported dataId {operation.DataId}");
		}
		if (_pendingLoadingOperationIds == null)
		{
			return;
		}
		uint num = _pendingLoadingOperationIds.Peek();
		if (num == operation.Id)
		{
			_pendingLoadingOperationIds.Dequeue();
			if (_pendingLoadingOperationIds.Count <= 0)
			{
				_pendingLoadingOperationIds = null;
				InitializeInternalDataOfCollections();
				OnLoadedArchiveData();
				DomainManager.Global.CompleteLoading(14);
			}
		}
	}

	private void InitializeInternalDataOfCollections()
	{
	}
}
