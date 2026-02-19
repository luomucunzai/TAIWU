using Config;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Item;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Combat;

public class CombatCharacterStateUseItem : CombatCharacterStateBase
{
	public enum EType
	{
		Invalid = -1,
		Poison,
		Rope,
		Custom
	}

	private ItemKey _itemKey;

	private bool _hit;

	private EType Type
	{
		get
		{
			if (_itemKey.ItemType == 8)
			{
				return EType.Poison;
			}
			if (_itemKey.ItemType != 12)
			{
				return EType.Invalid;
			}
			short templateId = _itemKey.TemplateId;
			if (1 == 0)
			{
			}
			EType result;
			switch (templateId)
			{
			case 73:
			case 74:
			case 75:
			case 76:
			case 77:
			case 78:
			case 79:
			case 80:
			case 81:
				result = EType.Rope;
				break;
			case 285:
				result = EType.Rope;
				break;
			case 375:
			case 376:
			case 377:
			case 378:
			case 379:
			case 380:
			case 381:
			case 382:
			case 383:
			case 384:
				result = EType.Custom;
				break;
			default:
				result = EType.Invalid;
				break;
			}
			if (1 == 0)
			{
			}
			return result;
		}
	}

	private IItemConfig Template => _itemKey.GetConfig();

	private bool InRange => Template.MaxUseDistance < 0 || CurrentCombatDomain.GetCurrentDistance() <= Template.MaxUseDistance;

	private CombatCharacter EnemyChar => CurrentCombatDomain.GetCombatCharacter(!CombatChar.IsAlly);

	private CombatItemUseItem UseConfig
	{
		get
		{
			EType type = Type;
			if (1 == 0)
			{
			}
			CombatItemUseItem result = ((type != EType.Rope || !_hit) ? (CombatItemUse.Instance.GetItem(ItemTemplateHelper.GetItemCombatUseEffect(_itemKey.ItemType, _itemKey.TemplateId)) ?? CombatItemUse.DefValue.UseRopeFail) : CombatItemUse.Instance[EnemyChar.AnimalConfig?.CatchEffect ?? 5]);
			if (1 == 0)
			{
			}
			return result;
		}
	}

	public CombatCharacterStateUseItem(CombatDomain combatDomain, CombatCharacter combatChar)
		: base(combatDomain, combatChar, CombatCharacterStateType.UseItem)
	{
		IsUpdateOnPause = true;
	}

	public override void OnEnter()
	{
		base.OnEnter();
		DataContext dataContext = CombatChar.GetDataContext();
		_itemKey = CombatChar.UsingItem;
		short distance = UseConfig.Distance;
		int currentDistance = CurrentCombatDomain.GetCurrentDistance();
		_hit = CheckItemHit(dataContext);
		int num = ((InRange && currentDistance != distance) ? 6 : 0);
		if (num > 0)
		{
			CurrentCombatDomain.SetDisplayPosition(dataContext, CombatChar.IsAlly, CurrentCombatDomain.GetDisplayPosition(CombatChar.IsAlly, distance));
		}
		DelayCall(PlayEffect, num);
		if (CheckItemRemove())
		{
			CombatChar.GetCharacter().RemoveInventoryItem(dataContext, _itemKey, 1, deleteItem: true);
		}
	}

	public override void OnExit()
	{
		CombatChar.SetPreparingItem(ItemKey.Invalid, CombatChar.GetDataContext());
		CombatChar.UsingItem = ItemKey.Invalid;
	}

	private bool CheckItemHit(DataContext context)
	{
		if (!InRange)
		{
			return false;
		}
		if (Type != EType.Rope)
		{
			return true;
		}
		CombatConfigItem combatConfig = DomainManager.Combat.CombatConfig;
		if (combatConfig.CaptureRequireRope >= 0 && _itemKey.TemplateId != combatConfig.CaptureRequireRope)
		{
			return false;
		}
		return CurrentCombatDomain.CheckRopeHit(context.Random, Template.Grade);
	}

	private bool CheckItemRemove()
	{
		EType type = Type;
		if (1 == 0)
		{
		}
		bool result = type switch
		{
			EType.Poison => Template.ItemSubType != 802 || _hit, 
			EType.Rope => _itemKey.TemplateId != 285 && (!_hit || EnemyChar.AnimalConfig != null), 
			_ => false, 
		};
		if (1 == 0)
		{
		}
		return result;
	}

	private void PlayEffect()
	{
		DataContext dataContext = CombatChar.GetDataContext();
		if (_hit && Type != EType.Poison)
		{
			CombatChar.SetAnimationToLoop(null, dataContext);
			EnemyChar.SetAnimationToLoop(null, dataContext);
		}
		if (Type == EType.Rope)
		{
			CombatChar.SetParticleToLoop(null, dataContext);
			CombatChar.SetSoundToLoop(null, dataContext);
		}
		CombatItemUseItem useConfig = UseConfig;
		CombatChar.SetParticleToPlay(useConfig.Particle, dataContext);
		CombatChar.SetSkillSoundToPlay(useConfig.Sound, dataContext);
		CombatChar.SetAnimationToPlayOnce(useConfig.Animation, dataContext);
		DelayCall(PlayHitAnim, AnimDataCollection.GetEventFrame(useConfig.Animation, "act0"));
		DelayCall(PlayCastAnim, AnimDataCollection.GetDurationFrame(useConfig.Animation));
	}

	private void PlayHitAnim()
	{
		DataContext dataContext = CombatChar.GetDataContext();
		string text = null;
		if (_hit)
		{
			EType type = Type;
			if (1 == 0)
			{
			}
			string text2 = ((type != EType.Poison) ? UseConfig.BeHitAnimation : CurrentCombatDomain.GetHittedAni(EnemyChar, Template.Grade / 3));
			if (1 == 0)
			{
			}
			text = text2;
			if (Type == EType.Poison)
			{
				MedicineItem medicineItem = (MedicineItem)Template;
				if (medicineItem.ItemSubType != 802)
				{
					CurrentCombatDomain.AddPoison(dataContext, CombatChar, EnemyChar, medicineItem.PoisonType, (sbyte)(Template.Grade / 3 + 1), medicineItem.EffectValue * GlobalConfig.Instance.ThrowPoisonParam, -1);
				}
				else
				{
					CurrentCombatDomain.AddWugIrresistibly(dataContext, EnemyChar, _itemKey);
					CurrentCombatDomain.ShowWugKingEffectTips(dataContext, CombatChar.GetId(), EnemyChar.GetId());
				}
			}
			else if (Type == EType.Custom)
			{
				Events.RaiseUsedCustomItem(dataContext, CombatChar.GetId(), _itemKey);
			}
			else
			{
				DomainManager.Combat.ClearShowUseSpecialMisc(dataContext);
			}
		}
		else if (InRange)
		{
			text = CurrentCombatDomain.GetAvoidAni(EnemyChar, 2);
		}
		else
		{
			CombatChar.SetAttackOutOfRange(attackOutOfRange: true, dataContext);
		}
		if (text != null)
		{
			EnemyChar.SetAnimationToPlayOnce(text, dataContext);
		}
	}

	private void PlayCastAnim()
	{
		DataContext dataContext = CombatChar.GetDataContext();
		CombatCharacter combatCharacter = CurrentCombatDomain.GetCombatCharacter(!CombatChar.IsAlly, tryGetCoverCharacter: true);
		if (_itemKey.ItemType == 8)
		{
			CurrentCombatDomain.AddToCheckFallenSet(combatCharacter.GetId());
		}
		if (Type == EType.Rope && _hit)
		{
			if (combatCharacter.AnimalConfig == null)
			{
				DomainManager.Combat.AppendGetChar(combatCharacter.GetId());
				DomainManager.TaiwuEvent.SetListenerEventActionIntArg("CombatOver", "CharIdSeizedInCombat", combatCharacter.GetId());
				DomainManager.TaiwuEvent.SetListenerEventActionISerializableArg("CombatOver", "ItemKeySeizeCharacterInCombat", (ISerializableGameData)(object)_itemKey);
				DomainManager.TaiwuEvent.SetListenerEventActionIntArg("CombatOver", "UseItemKeySeizeCharacterId", CombatChar.GetId());
			}
			else if (!DomainManager.Combat.CombatConfig.CaptureNoCarrier)
			{
				ItemKey itemKey = DomainManager.Item.CreateItem(dataContext, 4, combatCharacter.AnimalConfig.CarrierId);
				CombatChar.GetCharacter().AddInventoryItem(dataContext, itemKey, 1);
				DomainManager.Combat.AppendGetItem(itemKey);
				DomainManager.TaiwuEvent.SetListenerEventActionISerializableArg("CombatOver", "ItemKeySeizeCarrierInCombat", (ISerializableGameData)(object)_itemKey);
			}
			else
			{
				DomainManager.TaiwuEvent.SetListenerEventActionISerializableArg("CombatOver", "ItemKeySeizeCarrierInCombat", (ISerializableGameData)(object)_itemKey);
			}
			CurrentCombatDomain.EndCombat(dataContext, combatCharacter, flee: false, playAni: false);
		}
		else
		{
			CombatChar.StateMachine.TranslateState(CombatCharacterStateType.Idle);
			CurrentCombatDomain.SetDisplayPosition(dataContext, CombatChar.IsAlly, int.MinValue);
		}
	}
}
