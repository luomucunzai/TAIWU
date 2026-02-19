using System;
using Config;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.Item;
using GameData.Domains.LifeRecord;
using GameData.Domains.Map;

namespace GameData.Domains.SpecialEffect.CombatSkill.Wuxianjiao.WugEffect;

public class WugEffectBase : SpecialEffectBase
{
	public delegate void LifeRecordAddTemplate(int charId, int currDate, sbyte itemType, short templateId);

	public delegate void LifeRecordAddTemplate<in T0>(int charId, int currDate, sbyte itemType, short templateId, T0 arg0);

	public delegate void LifeRecordAddTemplate<in T0, in T1>(int charId, int currDate, sbyte itemType, short templateId, T0 arg0, T1 arg1);

	public delegate void LifeRecordAddTemplate<in T0, in T1, in T2>(int charId, int currDate, sbyte itemType, short templateId, T0 arg0, T1 arg1, T2 arg2);

	public delegate void LifeRecordGrownAddTemplate(int charId, int currDate, sbyte itemType, short templateId, sbyte newItemType, short newTemplateId);

	public delegate void LifeRecordRelatedAddTemplate(int srcCharId, int currDate, int dstCharId, Location location, sbyte itemType, short templateId, sbyte newItemType, short newTemplateId);

	private short _wugTemplateId;

	private short _effectTemplateId;

	protected MedicineItem WugConfig;

	protected sbyte CostWugCount;

	protected bool CanAffect => WugConfig.WugGrowthType != 5 || CharObj.GetEatingItems().IndexOfWug(WugConfig.WugType) < 0;

	protected bool IsElite => WugConfig.WugGrowthType != 5 && CharObj.GetEatingItems().ContainsWugKing(WugConfig.WugType);

	protected bool IsGood => WugGrowthType.IsGood(WugConfig.WugGrowthType);

	protected bool IsBad => WugGrowthType.IsBad(WugConfig.WugGrowthType);

	protected bool IsGrown
	{
		get
		{
			sbyte wugGrowthType = WugConfig.WugGrowthType;
			if ((uint)(wugGrowthType - 4) <= 1u)
			{
				return true;
			}
			return false;
		}
	}

	protected bool CanChangeToGrown => WugGrowthType.CanChangeToGrown(WugConfig.WugGrowthType);

	public short WugTemplateId => _wugTemplateId;

	protected WugEffectBase()
	{
	}

	protected WugEffectBase(int charId, int type, short wugTemplateId, short effectId)
		: base(charId, type)
	{
		_wugTemplateId = wugTemplateId;
		_effectTemplateId = effectId;
		WugConfig = Config.Medicine.Instance[wugTemplateId];
	}

	public override void OnEnable(DataContext context)
	{
		Events.RegisterHandler_CombatBegin(OnCombatBegin);
		Events.RegisterHandler_RemoveWug(OnRemoveWug);
	}

	public override void OnDataAdded(DataContext context)
	{
		if (DomainManager.Combat.Started && CheckValid())
		{
			OnCombatBegin(context);
		}
	}

	public override void OnDisable(DataContext context)
	{
		Events.UnRegisterHandler_CombatBegin(OnCombatBegin);
		Events.UnRegisterHandler_RemoveWug(OnRemoveWug);
		if (DomainManager.Combat.IsCharInCombat(base.CharacterId))
		{
			Events.UnRegisterHandler_CombatSettlement(OnCombatSettlement);
			ClearAffectDataAndEvent(context);
		}
	}

	public virtual void OnEffectAdded(DataContext context, short replacedWug)
	{
	}

	private void OnCombatBegin(DataContext context)
	{
		if (DomainManager.Combat.IsCharInCombat(base.CharacterId))
		{
			Events.RegisterHandler_CombatSettlement(OnCombatSettlement);
			AddAffectDataAndEvent(context);
		}
	}

	private void OnCombatSettlement(DataContext context, sbyte combatStatus)
	{
		if (WugGrowthType.IsWugGrowthTypeCombatOnly(WugConfig.WugGrowthType))
		{
			CharObj.AddWug(context, ItemDomain.GetWugTemplateId(WugConfig.WugType, (sbyte)(WugConfig.WugGrowthType + 1)));
		}
		Events.UnRegisterHandler_CombatSettlement(OnCombatSettlement);
		ClearAffectDataAndEvent(context);
	}

	private void OnRemoveWug(DataContext context, int charId, short wugTemplateId)
	{
		if (charId == base.CharacterId && wugTemplateId == WugConfig.TemplateId)
		{
			DomainManager.SpecialEffect.Remove(context, Id);
		}
	}

	protected virtual void AddAffectDataAndEvent(DataContext context)
	{
	}

	protected virtual void ClearAffectDataAndEvent(DataContext context)
	{
	}

	protected void CostWugInCombat(DataContext context)
	{
		if (CostWugCount > 0 && WugGrowthType.IsWugGrowthTypeCombatOnly(WugConfig.WugGrowthType))
		{
			int num = CostWugCount;
			int wugCount = base.CombatChar.GetWugCount();
			int wugCount2 = base.CurrEnemyChar.GetWugCount();
			if (num > 0 && wugCount > 0)
			{
				int num2 = Math.Min(num, wugCount);
				num -= num2;
				base.CombatChar.ChangeWugCount(context, -num2);
			}
			if (num > 0 && wugCount2 > 0)
			{
				int num3 = Math.Min(num, wugCount2);
				num -= num3;
				base.CurrEnemyChar.ChangeWugCount(context, -num3);
			}
			if (num > 0)
			{
				CharObj.RemoveWug(context, WugConfig.TemplateId);
			}
		}
	}

	protected void ShowEffectTips(DataContext context, byte index = 1)
	{
		DomainManager.Combat.ShowSpecialEffectTips(base.CharacterId, _effectTemplateId, new ItemKey(8, byte.MaxValue, WugConfig.TemplateId, -1), index);
	}

	protected virtual void ChangeToGrown(DataContext context)
	{
		short wugTemplateId = ItemDomain.GetWugTemplateId(WugConfig.WugType, 4);
		CharObj.AddWug(context, wugTemplateId);
		LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
		sbyte wugType = WugConfig.WugType;
		if (1 == 0)
		{
		}
		LifeRecordGrownAddTemplate lifeRecordGrownAddTemplate = wugType switch
		{
			0 => lifeRecordCollection.AddWugRedEyeChangeToGrown, 
			1 => lifeRecordCollection.AddWugForestSpiritChangeToGrown, 
			2 => lifeRecordCollection.AddWugBlackBloodChangeToGrown, 
			3 => lifeRecordCollection.AddWugDevilInsideChangeToGrown, 
			5 => lifeRecordCollection.AddWugIceSilkwormChangeToGrown, 
			6 => lifeRecordCollection.AddWugGoldenSilkwormChangeToGrown, 
			_ => null, 
		};
		if (1 == 0)
		{
		}
		LifeRecordGrownAddTemplate lifeRecordGrownAddTemplate2 = lifeRecordGrownAddTemplate;
		if (lifeRecordGrownAddTemplate2 != null)
		{
			AddLifeRecord(lifeRecordGrownAddTemplate2, wugTemplateId);
		}
	}

	protected bool CheckValid()
	{
		return CharObj.GetEatingItems().IndexOfWug(WugConfig.TemplateId) >= 0;
	}

	public static sbyte GetNpcCombatAddGrowthType(GameData.Domains.Character.Character addChar, sbyte wugType, bool direct)
	{
		sbyte b = wugType;
		if (1 == 0)
		{
		}
		sbyte result;
		switch (b)
		{
		case 1:
			if (!ForestSpiritBase.CanGrown(addChar))
			{
				goto default;
			}
			result = 4;
			break;
		case 3:
			if (!DevilInsideBase.CanGrown(addChar))
			{
				goto default;
			}
			result = 4;
			break;
		case 5:
			if (!IceSilkwormBase.CanGrown(addChar))
			{
				goto default;
			}
			result = 4;
			break;
		default:
			result = (sbyte)((!direct) ? 1 : 3);
			break;
		}
		if (1 == 0)
		{
		}
		return result;
	}

	protected void AddLifeRecord(LifeRecordAddTemplate template)
	{
		int currDate = DomainManager.World.GetCurrDate();
		template(base.CharacterId, currDate, WugConfig.ItemType, WugConfig.TemplateId);
	}

	protected void AddLifeRecord<T0>(LifeRecordAddTemplate<T0> template, T0 arg0)
	{
		int currDate = DomainManager.World.GetCurrDate();
		template(base.CharacterId, currDate, WugConfig.ItemType, WugConfig.TemplateId, arg0);
	}

	protected void AddLifeRecord<T0, T1>(LifeRecordAddTemplate<T0, T1> template, T0 arg0, T1 arg1)
	{
		int currDate = DomainManager.World.GetCurrDate();
		template(base.CharacterId, currDate, WugConfig.ItemType, WugConfig.TemplateId, arg0, arg1);
	}

	protected void AddLifeRecord<T0, T1, T2>(LifeRecordAddTemplate<T0, T1, T2> template, T0 arg0, T1 arg1, T2 arg2)
	{
		int currDate = DomainManager.World.GetCurrDate();
		template(base.CharacterId, currDate, WugConfig.ItemType, WugConfig.TemplateId, arg0, arg1, arg2);
	}

	protected void AddLifeRecord(LifeRecordGrownAddTemplate template, short newTemplateId)
	{
		int currDate = DomainManager.World.GetCurrDate();
		template(base.CharacterId, currDate, WugConfig.ItemType, WugConfig.TemplateId, 8, newTemplateId);
	}

	protected void AddLifeRecord(LifeRecordRelatedAddTemplate template, int charId, short newTemplateId)
	{
		int currDate = DomainManager.World.GetCurrDate();
		GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(charId);
		Location location = (CharObj.GetLocation().IsValid() ? CharObj.GetLocation() : element_Objects.GetLocation());
		template(base.CharacterId, currDate, charId, location, WugConfig.ItemType, WugConfig.TemplateId, 8, newTemplateId);
	}

	protected void AddLifeRecord(LifeRecordRelatedAddTemplate template, int srcCharId, int dstCharId, short newTemplateId)
	{
		int currDate = DomainManager.World.GetCurrDate();
		GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(srcCharId);
		GameData.Domains.Character.Character element_Objects2 = DomainManager.Character.GetElement_Objects(dstCharId);
		Location location = (element_Objects.GetLocation().IsValid() ? element_Objects.GetLocation() : element_Objects2.GetLocation());
		template(srcCharId, currDate, dstCharId, location, WugConfig.ItemType, WugConfig.TemplateId, 8, newTemplateId);
	}

	protected override int GetSubClassSerializedSize()
	{
		return 4;
	}

	protected unsafe override int SerializeSubClass(byte* pData)
	{
		byte* ptr = pData;
		*(short*)ptr = _wugTemplateId;
		ptr += 2;
		*(short*)ptr = _effectTemplateId;
		ptr += 2;
		return (int)(ptr - pData);
	}

	protected unsafe override int DeserializeSubClass(byte* pData)
	{
		byte* ptr = pData;
		_wugTemplateId = *(short*)ptr;
		ptr += 2;
		_effectTemplateId = *(short*)ptr;
		ptr += 2;
		WugConfig = Config.Medicine.Instance[_wugTemplateId];
		return (int)(ptr - pData);
	}
}
