using System;
using System.Collections.Generic;
using Config;
using Config.ConfigCells;
using GameData.Domains.Extra;
using GameData.Domains.Item;
using GameData.Serializer;
using SerializableGameDataSourceGenerator;

namespace GameData.Domains.Building;

[AutoGenerateSerializableGameData(NotForArchive = true, NoCopyConstructors = true)]
public class ProductionPool : ISerializableGameData
{
	[SerializableGameDataField]
	public Dictionary<Production, ProductionData> Productions;

	[SerializableGameDataField]
	public int CookAddOn;

	public ProductionPool()
	{
		Productions = new Dictionary<Production, ProductionData>();
		CookAddOn = 0;
	}

	public ProductionPool(int cookAddOn)
	{
		Productions = new Dictionary<Production, ProductionData>();
		CookAddOn = cookAddOn;
	}

	public int GetCreateOrderPrice(out int favor)
	{
		int num = 0;
		favor = 0;
		foreach (KeyValuePair<Production, ProductionData> production2 in Productions)
		{
			production2.Deconstruct(out var key, out var value);
			Production production = key;
			if (value.CanProduce)
			{
				int baseValue = ItemTemplateHelper.GetBaseValue(production.ItemType, production.TemplateId);
				if (baseValue > num)
				{
					num = baseValue;
					favor = ItemTemplateHelper.GetBaseFavorabilityChange(production.ItemType, production.TemplateId);
				}
			}
		}
		return num * GameData.Domains.Extra.SharedConstValue.ArtisanOrderPricePercent / 100;
	}

	public int GetInterceptOrderPrice(bool isDebateWon, out int favor)
	{
		int createOrderPrice = GetCreateOrderPrice(out favor);
		int num = (isDebateWon ? GameData.Domains.Extra.SharedConstValue.ArtisanOrderInterceptDebatePricePercent : GameData.Domains.Extra.SharedConstValue.ArtisanOrderInterceptPricePercent);
		return createOrderPrice * num / 100;
	}

	public int GetProductionWeight(Production production)
	{
		if (!Productions.TryGetValue(production, out var value))
		{
			return 0;
		}
		return value.Weight;
	}

	public bool CanMaterialBeAdded(ItemKey key)
	{
		if (key.ItemType != 5)
		{
			return false;
		}
		MaterialItem materialItem = Material.Instance[key.TemplateId];
		if (materialItem.IsSpecial)
		{
			return false;
		}
		sbyte b = materialItem.Grade;
		if (materialItem.RequiredLifeSkillType == 14)
		{
			b = Math.Min(8, (sbyte)(b + CookAddOn));
		}
		for (int i = 0; i < materialItem.CraftableItemTypes.Count; i++)
		{
			short index = materialItem.CraftableItemTypes[i];
			foreach (short makeItemSubType in MakeItemType.Instance[index].MakeItemSubTypes)
			{
				MakeItemSubTypeItem makeItemSubTypeItem = MakeItemSubType.Instance[makeItemSubType];
				MakeItemResult result = makeItemSubTypeItem.Result;
				if (materialItem.RequiredLifeSkillType == 8 && i == 0)
				{
					b = SharedMethods.GetHerbMaterialTempGrade(b, isManual: true, !makeItemSubTypeItem.IsOdd);
				}
				for (int j = -1; j < 2; j++)
				{
					if (TryGetProductionTemplateId((sbyte)(b + j), result.ItemType, result.TemplateId, out var finalTemplateId) && Productions.TryGetValue(new Production(result.ItemType, finalTemplateId), out var value) && value.CanProduce)
					{
						return true;
					}
				}
			}
		}
		return false;
	}

	public static bool TryGetProductionTemplateId(sbyte targetGrade, sbyte itemType, short baseTemplateId, out short finalTemplateId)
	{
		short groupId = ItemTemplateHelper.GetGroupId(itemType, baseTemplateId);
		if (groupId < 0)
		{
			finalTemplateId = baseTemplateId;
			return true;
		}
		targetGrade = Math.Clamp(targetGrade, 0, 8);
		sbyte grade = ItemTemplateHelper.GetGrade(itemType, baseTemplateId);
		int num = Math.Max(targetGrade - grade, -2);
		short num2 = Convert.ToInt16(baseTemplateId + num);
		bool result = (ItemTemplateHelper.CheckTemplateValid(itemType, num2) ? ItemTemplateHelper.GetGroupId(itemType, num2) : (-1)) == groupId;
		finalTemplateId = num2;
		return result;
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 4;
		num += 4;
		if (Productions != null)
		{
			foreach (KeyValuePair<Production, ProductionData> production in Productions)
			{
				num += production.Key.GetSerializedSize();
				num += production.Value.GetSerializedSize();
			}
		}
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		if (Productions != null)
		{
			*(int*)ptr = Productions.Count;
			ptr += 4;
			foreach (KeyValuePair<Production, ProductionData> production in Productions)
			{
				ptr += production.Key.Serialize(ptr);
				ptr += production.Value.Serialize(ptr);
			}
		}
		else
		{
			*(int*)ptr = 0;
			ptr += 4;
		}
		*(int*)ptr = CookAddOn;
		ptr += 4;
		int num = (int)(ptr - pData);
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		int num = *(int*)ptr;
		ptr += 4;
		if (num > 0)
		{
			if (Productions == null)
			{
				Productions = new Dictionary<Production, ProductionData>();
			}
			else
			{
				Productions.Clear();
			}
			for (int i = 0; i < num; i++)
			{
				Production key = default(Production);
				ptr += key.Deserialize(ptr);
				ProductionData value = default(ProductionData);
				ptr += value.Deserialize(ptr);
				Productions.Add(key, value);
			}
		}
		else
		{
			Productions?.Clear();
		}
		CookAddOn = *(int*)ptr;
		ptr += 4;
		int num2 = (int)(ptr - pData);
		if (num2 > 4)
		{
			return (num2 + 3) / 4 * 4;
		}
		return num2;
	}
}
