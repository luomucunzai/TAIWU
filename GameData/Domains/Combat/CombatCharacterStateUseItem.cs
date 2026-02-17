using System;
using Config;
using Config.Common;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Item;
using GameData.Utilities;

namespace GameData.Domains.Combat
{
	// Token: 0x020006AA RID: 1706
	public class CombatCharacterStateUseItem : CombatCharacterStateBase
	{
		// Token: 0x1700037A RID: 890
		// (get) Token: 0x0600627D RID: 25213 RVA: 0x00380EA0 File Offset: 0x0037F0A0
		private CombatCharacterStateUseItem.EType Type
		{
			get
			{
				bool flag = this._itemKey.ItemType == 8;
				CombatCharacterStateUseItem.EType result;
				if (flag)
				{
					result = CombatCharacterStateUseItem.EType.Poison;
				}
				else
				{
					bool flag2 = this._itemKey.ItemType != 12;
					if (flag2)
					{
						result = CombatCharacterStateUseItem.EType.Invalid;
					}
					else
					{
						short templateId = this._itemKey.TemplateId;
						if (!true)
						{
						}
						CombatCharacterStateUseItem.EType etype;
						if (templateId >= 375)
						{
							if (templateId <= 384)
							{
								etype = CombatCharacterStateUseItem.EType.Custom;
								goto IL_7D;
							}
						}
						else if (templateId >= 73)
						{
							if (templateId <= 81)
							{
								etype = CombatCharacterStateUseItem.EType.Rope;
								goto IL_7D;
							}
							if (templateId == 285)
							{
								etype = CombatCharacterStateUseItem.EType.Rope;
								goto IL_7D;
							}
						}
						etype = CombatCharacterStateUseItem.EType.Invalid;
						IL_7D:
						if (!true)
						{
						}
						result = etype;
					}
				}
				return result;
			}
		}

		// Token: 0x1700037B RID: 891
		// (get) Token: 0x0600627E RID: 25214 RVA: 0x00380F33 File Offset: 0x0037F133
		private IItemConfig Template
		{
			get
			{
				return this._itemKey.GetConfig();
			}
		}

		// Token: 0x1700037C RID: 892
		// (get) Token: 0x0600627F RID: 25215 RVA: 0x00380F40 File Offset: 0x0037F140
		private bool InRange
		{
			get
			{
				return this.Template.MaxUseDistance < 0 || this.CurrentCombatDomain.GetCurrentDistance() <= (short)this.Template.MaxUseDistance;
			}
		}

		// Token: 0x1700037D RID: 893
		// (get) Token: 0x06006280 RID: 25216 RVA: 0x00380F6E File Offset: 0x0037F16E
		private CombatCharacter EnemyChar
		{
			get
			{
				return this.CurrentCombatDomain.GetCombatCharacter(!this.CombatChar.IsAlly, false);
			}
		}

		// Token: 0x1700037E RID: 894
		// (get) Token: 0x06006281 RID: 25217 RVA: 0x00380F8C File Offset: 0x0037F18C
		private CombatItemUseItem UseConfig
		{
			get
			{
				CombatCharacterStateUseItem.EType type = this.Type;
				if (!true)
				{
				}
				CombatItemUseItem result;
				if (type == CombatCharacterStateUseItem.EType.Rope)
				{
					if (this._hit)
					{
						ConfigData<CombatItemUseItem, short> instance = CombatItemUse.Instance;
						AnimalItem animalConfig = this.EnemyChar.AnimalConfig;
						result = instance[(animalConfig != null) ? animalConfig.CatchEffect : 5];
						goto IL_70;
					}
				}
				result = (CombatItemUse.Instance.GetItem(ItemTemplateHelper.GetItemCombatUseEffect(this._itemKey.ItemType, this._itemKey.TemplateId)) ?? CombatItemUse.DefValue.UseRopeFail);
				IL_70:
				if (!true)
				{
				}
				return result;
			}
		}

		// Token: 0x06006282 RID: 25218 RVA: 0x0038100E File Offset: 0x0037F20E
		public CombatCharacterStateUseItem(CombatDomain combatDomain, CombatCharacter combatChar) : base(combatDomain, combatChar, CombatCharacterStateType.UseItem)
		{
			this.IsUpdateOnPause = true;
		}

		// Token: 0x06006283 RID: 25219 RVA: 0x00381024 File Offset: 0x0037F224
		public override void OnEnter()
		{
			base.OnEnter();
			DataContext context = this.CombatChar.GetDataContext();
			this._itemKey = this.CombatChar.UsingItem;
			short displayDistance = this.UseConfig.Distance;
			int distance = (int)this.CurrentCombatDomain.GetCurrentDistance();
			this._hit = this.CheckItemHit(context);
			int playEffectDelay = (this.InRange && distance != (int)displayDistance) ? 6 : 0;
			bool flag = playEffectDelay > 0;
			if (flag)
			{
				this.CurrentCombatDomain.SetDisplayPosition(context, this.CombatChar.IsAlly, this.CurrentCombatDomain.GetDisplayPosition(this.CombatChar.IsAlly, displayDistance));
			}
			base.DelayCall(new CombatCharacterStateBase.CombatCharacterStateDelayCallRequest(this.PlayEffect), playEffectDelay);
			bool flag2 = this.CheckItemRemove();
			if (flag2)
			{
				this.CombatChar.GetCharacter().RemoveInventoryItem(context, this._itemKey, 1, true, false);
			}
		}

		// Token: 0x06006284 RID: 25220 RVA: 0x00381100 File Offset: 0x0037F300
		public override void OnExit()
		{
			this.CombatChar.SetPreparingItem(ItemKey.Invalid, this.CombatChar.GetDataContext());
			this.CombatChar.UsingItem = ItemKey.Invalid;
		}

		// Token: 0x06006285 RID: 25221 RVA: 0x00381130 File Offset: 0x0037F330
		private bool CheckItemHit(DataContext context)
		{
			bool flag = !this.InRange;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				bool flag2 = this.Type != CombatCharacterStateUseItem.EType.Rope;
				if (flag2)
				{
					result = true;
				}
				else
				{
					CombatConfigItem combatConfig = DomainManager.Combat.CombatConfig;
					bool flag3 = combatConfig.CaptureRequireRope >= 0 && this._itemKey.TemplateId != combatConfig.CaptureRequireRope;
					result = (!flag3 && this.CurrentCombatDomain.CheckRopeHit(context.Random, (int)this.Template.Grade));
				}
			}
			return result;
		}

		// Token: 0x06006286 RID: 25222 RVA: 0x003811BC File Offset: 0x0037F3BC
		private bool CheckItemRemove()
		{
			CombatCharacterStateUseItem.EType type = this.Type;
			if (!true)
			{
			}
			bool result;
			if (type != CombatCharacterStateUseItem.EType.Poison)
			{
				result = (type == CombatCharacterStateUseItem.EType.Rope && this._itemKey.TemplateId != 285 && (!this._hit || this.EnemyChar.AnimalConfig != null));
			}
			else
			{
				result = (this.Template.ItemSubType != 802 || this._hit);
			}
			if (!true)
			{
			}
			return result;
		}

		// Token: 0x06006287 RID: 25223 RVA: 0x00381240 File Offset: 0x0037F440
		private void PlayEffect()
		{
			DataContext context = this.CombatChar.GetDataContext();
			bool flag = this._hit && this.Type > CombatCharacterStateUseItem.EType.Poison;
			if (flag)
			{
				this.CombatChar.SetAnimationToLoop(null, context);
				this.EnemyChar.SetAnimationToLoop(null, context);
			}
			bool flag2 = this.Type == CombatCharacterStateUseItem.EType.Rope;
			if (flag2)
			{
				this.CombatChar.SetParticleToLoop(null, context);
				this.CombatChar.SetSoundToLoop(null, context);
			}
			CombatItemUseItem useConfig = this.UseConfig;
			this.CombatChar.SetParticleToPlay(useConfig.Particle, context);
			this.CombatChar.SetSkillSoundToPlay(useConfig.Sound, context);
			this.CombatChar.SetAnimationToPlayOnce(useConfig.Animation, context);
			base.DelayCall(new CombatCharacterStateBase.CombatCharacterStateDelayCallRequest(this.PlayHitAnim), AnimDataCollection.GetEventFrame(useConfig.Animation, "act0", 0));
			base.DelayCall(new CombatCharacterStateBase.CombatCharacterStateDelayCallRequest(this.PlayCastAnim), AnimDataCollection.GetDurationFrame(useConfig.Animation));
		}

		// Token: 0x06006288 RID: 25224 RVA: 0x00381340 File Offset: 0x0037F540
		private void PlayHitAnim()
		{
			DataContext context = this.CombatChar.GetDataContext();
			string enemyAni = null;
			bool hit = this._hit;
			if (hit)
			{
				CombatCharacterStateUseItem.EType type = this.Type;
				if (!true)
				{
				}
				string text;
				if (type != CombatCharacterStateUseItem.EType.Poison)
				{
					text = this.UseConfig.BeHitAnimation;
				}
				else
				{
					text = this.CurrentCombatDomain.GetHittedAni(this.EnemyChar, (int)(this.Template.Grade / 3));
				}
				if (!true)
				{
				}
				enemyAni = text;
				bool flag = this.Type == CombatCharacterStateUseItem.EType.Poison;
				if (flag)
				{
					MedicineItem itemConfig = (MedicineItem)this.Template;
					bool flag2 = itemConfig.ItemSubType != 802;
					if (flag2)
					{
						this.CurrentCombatDomain.AddPoison(context, this.CombatChar, this.EnemyChar, itemConfig.PoisonType, this.Template.Grade / 3 + 1, (int)(itemConfig.EffectValue * (short)GlobalConfig.Instance.ThrowPoisonParam), -1, true, true, default(ItemKey), false, false, false);
					}
					else
					{
						this.CurrentCombatDomain.AddWugIrresistibly(context, this.EnemyChar, this._itemKey);
						this.CurrentCombatDomain.ShowWugKingEffectTips(context, this.CombatChar.GetId(), this.EnemyChar.GetId());
					}
				}
				else
				{
					bool flag3 = this.Type == CombatCharacterStateUseItem.EType.Custom;
					if (flag3)
					{
						Events.RaiseUsedCustomItem(context, this.CombatChar.GetId(), this._itemKey);
					}
					else
					{
						DomainManager.Combat.ClearShowUseSpecialMisc(context);
					}
				}
			}
			else
			{
				bool inRange = this.InRange;
				if (inRange)
				{
					enemyAni = this.CurrentCombatDomain.GetAvoidAni(this.EnemyChar, 2);
				}
				else
				{
					this.CombatChar.SetAttackOutOfRange(true, context);
				}
			}
			bool flag4 = enemyAni != null;
			if (flag4)
			{
				this.EnemyChar.SetAnimationToPlayOnce(enemyAni, context);
			}
		}

		// Token: 0x06006289 RID: 25225 RVA: 0x003814FC File Offset: 0x0037F6FC
		private void PlayCastAnim()
		{
			DataContext context = this.CombatChar.GetDataContext();
			CombatCharacter enemyChar = this.CurrentCombatDomain.GetCombatCharacter(!this.CombatChar.IsAlly, true);
			bool flag = this._itemKey.ItemType == 8;
			if (flag)
			{
				this.CurrentCombatDomain.AddToCheckFallenSet(enemyChar.GetId());
			}
			bool flag2 = this.Type == CombatCharacterStateUseItem.EType.Rope && this._hit;
			if (flag2)
			{
				bool flag3 = enemyChar.AnimalConfig == null;
				if (flag3)
				{
					DomainManager.Combat.AppendGetChar(enemyChar.GetId());
					DomainManager.TaiwuEvent.SetListenerEventActionIntArg("CombatOver", "CharIdSeizedInCombat", enemyChar.GetId());
					DomainManager.TaiwuEvent.SetListenerEventActionISerializableArg("CombatOver", "ItemKeySeizeCharacterInCombat", this._itemKey);
					DomainManager.TaiwuEvent.SetListenerEventActionIntArg("CombatOver", "UseItemKeySeizeCharacterId", this.CombatChar.GetId());
				}
				else
				{
					bool flag4 = !DomainManager.Combat.CombatConfig.CaptureNoCarrier;
					if (flag4)
					{
						ItemKey carrierKey = DomainManager.Item.CreateItem(context, 4, enemyChar.AnimalConfig.CarrierId);
						this.CombatChar.GetCharacter().AddInventoryItem(context, carrierKey, 1, false);
						DomainManager.Combat.AppendGetItem(carrierKey);
						DomainManager.TaiwuEvent.SetListenerEventActionISerializableArg("CombatOver", "ItemKeySeizeCarrierInCombat", this._itemKey);
					}
					else
					{
						DomainManager.TaiwuEvent.SetListenerEventActionISerializableArg("CombatOver", "ItemKeySeizeCarrierInCombat", this._itemKey);
					}
				}
				this.CurrentCombatDomain.EndCombat(context, enemyChar, false, false);
			}
			else
			{
				this.CombatChar.StateMachine.TranslateState(CombatCharacterStateType.Idle);
				this.CurrentCombatDomain.SetDisplayPosition(context, this.CombatChar.IsAlly, int.MinValue);
			}
		}

		// Token: 0x04001AE7 RID: 6887
		private ItemKey _itemKey;

		// Token: 0x04001AE8 RID: 6888
		private bool _hit;

		// Token: 0x02000B4F RID: 2895
		public enum EType
		{
			// Token: 0x04002FD1 RID: 12241
			Invalid = -1,
			// Token: 0x04002FD2 RID: 12242
			Poison,
			// Token: 0x04002FD3 RID: 12243
			Rope,
			// Token: 0x04002FD4 RID: 12244
			Custom
		}
	}
}
