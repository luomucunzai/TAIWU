using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Config;
using GameData.ArchiveData;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Dependencies;
using GameData.DomainEvents;
using GameData.Domains.Building;
using GameData.Domains.Character;
using GameData.Domains.Character.Relation;
using GameData.Domains.Combat.Ai;
using GameData.Domains.Combat.MixPoison;
using GameData.Domains.CombatSkill;
using GameData.Domains.Item;
using GameData.Domains.SpecialEffect;
using GameData.Domains.SpecialEffect.SectStory.Yuanshan;
using GameData.Domains.SpecialEffect.SectStory.Zhujian;
using GameData.Domains.Taiwu.Profession.SkillsData;
using GameData.Domains.TaiwuEvent;
using GameData.GameDataBridge;
using GameData.Serializer;
using GameData.Utilities;
using Redzen.Random;

namespace GameData.Domains.Combat
{
	// Token: 0x02000691 RID: 1681
	[SerializableGameData(NotForDisplayModule = true)]
	public class CombatCharacter : BaseGameDataObject, IExpressionConverter, IAiParticipant, ISerializableGameData
	{
		// Token: 0x17000351 RID: 849
		// (get) Token: 0x06005F7C RID: 24444 RVA: 0x00365250 File Offset: 0x00363450
		// (set) Token: 0x06005F7D RID: 24445 RVA: 0x00365258 File Offset: 0x00363458
		public BossItem BossConfig { get; private set; }

		// Token: 0x17000352 RID: 850
		// (get) Token: 0x06005F7E RID: 24446 RVA: 0x00365261 File Offset: 0x00363461
		// (set) Token: 0x06005F7F RID: 24447 RVA: 0x00365269 File Offset: 0x00363469
		public AnimalItem AnimalConfig { get; private set; }

		// Token: 0x17000353 RID: 851
		// (get) Token: 0x06005F80 RID: 24448 RVA: 0x00365272 File Offset: 0x00363472
		public bool IsActorSkeleton
		{
			get
			{
				return this.BossConfig == null && this.AnimalConfig == null;
			}
		}

		// Token: 0x17000354 RID: 852
		// (get) Token: 0x06005F81 RID: 24449 RVA: 0x00365288 File Offset: 0x00363488
		public int MaxChangeTrickCount
		{
			get
			{
				return DomainManager.SpecialEffect.ModifyValue(this._id, 301, 12, -1, -1, -1, 0, 0, 0, 0);
			}
		}

		// Token: 0x17000355 RID: 853
		// (get) Token: 0x06005F82 RID: 24450 RVA: 0x003652B3 File Offset: 0x003634B3
		public bool ChangeToMindMark
		{
			get
			{
				return DomainManager.SpecialEffect.ModifyData(this._id, -1, 288, false, -1, -1, -1);
			}
		}

		// Token: 0x17000356 RID: 854
		// (get) Token: 0x06005F83 RID: 24451 RVA: 0x003652CF File Offset: 0x003634CF
		public bool UnyieldingFallen
		{
			get
			{
				return DomainManager.SpecialEffect.ModifyData(this._id, -1, 282, false, -1, -1, -1);
			}
		}

		// Token: 0x17000357 RID: 855
		// (get) Token: 0x06005F84 RID: 24452 RVA: 0x003652EB File Offset: 0x003634EB
		public bool IsAnimal
		{
			get
			{
				return (this.IsAlly && this._id == DomainManager.Combat.GetCarrierAnimalCombatCharId()) || this.AnimalConfig != null;
			}
		}

		// Token: 0x17000358 RID: 856
		// (get) Token: 0x06005F85 RID: 24453 RVA: 0x00365313 File Offset: 0x00363513
		public bool IsMoving
		{
			get
			{
				return this.MoveData.MoveCd > 0;
			}
		}

		// Token: 0x17000359 RID: 857
		// (get) Token: 0x06005F86 RID: 24454 RVA: 0x00365323 File Offset: 0x00363523
		public bool IsJumping
		{
			get
			{
				return this.KeepMoving && (this._jumpPrepareProgress > 0 || this._jumpPreparedDistance > 0);
			}
		}

		// Token: 0x1700035A RID: 858
		// (get) Token: 0x06005F87 RID: 24455 RVA: 0x00365345 File Offset: 0x00363545
		public bool IsUnlockAttack
		{
			get
			{
				return this.UnlockWeaponIndex >= 0;
			}
		}

		// Token: 0x1700035B RID: 859
		// (get) Token: 0x06005F88 RID: 24456 RVA: 0x00365353 File Offset: 0x00363553
		public GameData.Domains.Item.Weapon UnlockWeapon
		{
			get
			{
				return DomainManager.Item.GetElement_Weapons(this._weapons[this.UnlockWeaponIndex].Id);
			}
		}

		// Token: 0x1700035C RID: 860
		// (get) Token: 0x06005F89 RID: 24457 RVA: 0x00365375 File Offset: 0x00363575
		public int UnlockEffectId
		{
			get
			{
				return Config.Weapon.Instance[this.UnlockWeapon.GetTemplateId()].UnlockEffect;
			}
		}

		// Token: 0x1700035D RID: 861
		// (get) Token: 0x06005F8A RID: 24458 RVA: 0x00365391 File Offset: 0x00363591
		public WeaponUnlockEffectItem UnlockEffect
		{
			get
			{
				return WeaponUnlockEffect.Instance[this.UnlockEffectId];
			}
		}

		// Token: 0x1700035E RID: 862
		// (get) Token: 0x06005F8B RID: 24459 RVA: 0x003653A3 File Offset: 0x003635A3
		public short NeedUseSkillFreeId
		{
			get
			{
				short result;
				if (this.CastFreeDataList.Count <= 0)
				{
					result = -1;
				}
				else
				{
					List<CastFreeData> castFreeDataList = this.CastFreeDataList;
					result = castFreeDataList[castFreeDataList.Count - 1].SkillId;
				}
				return result;
			}
		}

		// Token: 0x1700035F RID: 863
		// (get) Token: 0x06005F8C RID: 24460 RVA: 0x003653CE File Offset: 0x003635CE
		public bool NeedChangeSkill
		{
			get
			{
				return this.NeedUseSkillId >= 0 && (this._preparingSkillId < 0 || this.CanCastDuringPrepareSkills.Contains(this.NeedUseSkillId));
			}
		}

		// Token: 0x17000360 RID: 864
		// (get) Token: 0x06005F8D RID: 24461 RVA: 0x003653F9 File Offset: 0x003635F9
		public short NeedUseSkillId
		{
			get
			{
				return (this.NeedUseSkillFreeId >= 0) ? this.NeedUseSkillFreeId : this._combatReserveData.NeedUseSkillId;
			}
		}

		// Token: 0x17000361 RID: 865
		// (get) Token: 0x06005F8E RID: 24462 RVA: 0x00365417 File Offset: 0x00363617
		public bool NeedShowChangeTrick
		{
			get
			{
				return this._combatReserveData.NeedShowChangeTrick && (this._preparingSkillId < 0 || this.CanNormalAttackInPrepareSkill);
			}
		}

		// Token: 0x17000362 RID: 866
		// (get) Token: 0x06005F8F RID: 24463 RVA: 0x0036543B File Offset: 0x0036363B
		public int NeedChangeWeaponIndex
		{
			get
			{
				return this._combatReserveData.NeedChangeWeaponIndex;
			}
		}

		// Token: 0x17000363 RID: 867
		// (get) Token: 0x06005F90 RID: 24464 RVA: 0x00365448 File Offset: 0x00363648
		public ItemKey NeedUseItem
		{
			get
			{
				return this._combatReserveData.NeedUseItem;
			}
		}

		// Token: 0x17000364 RID: 868
		// (get) Token: 0x06005F91 RID: 24465 RVA: 0x00365455 File Offset: 0x00363655
		public sbyte NeedUseOtherAction
		{
			get
			{
				return this._combatReserveData.NeedUseOtherAction;
			}
		}

		// Token: 0x17000365 RID: 869
		// (get) Token: 0x06005F92 RID: 24466 RVA: 0x00365462 File Offset: 0x00363662
		public OtherActionTypeItem PreparingOtherActionTypeConfig
		{
			get
			{
				return (this._preparingOtherAction < 0) ? null : OtherActionType.Instance[this._preparingOtherAction];
			}
		}

		// Token: 0x17000366 RID: 870
		// (get) Token: 0x06005F93 RID: 24467 RVA: 0x00365480 File Offset: 0x00363680
		public ETeammateCommandImplement ExecutingTeammateCommandImplement
		{
			get
			{
				return (this._executingTeammateCommand < 0) ? ETeammateCommandImplement.Invalid : TeammateCommand.Instance[this._executingTeammateCommand].Implement;
			}
		}

		// Token: 0x17000367 RID: 871
		// (get) Token: 0x06005F94 RID: 24468 RVA: 0x003654A4 File Offset: 0x003636A4
		private string DataHandlerKey
		{
			get
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(11, 1);
				defaultInterpolatedStringHandler.AppendLiteral("CombatChar_");
				defaultInterpolatedStringHandler.AppendFormatted<int>(this._id);
				return defaultInterpolatedStringHandler.ToStringAndClear();
			}
		}

		// Token: 0x06005F95 RID: 24469 RVA: 0x003654E0 File Offset: 0x003636E0
		public override string ToString()
		{
			return this._character.ToString();
		}

		// Token: 0x06005F96 RID: 24470 RVA: 0x00365500 File Offset: 0x00363700
		public unsafe void Init(CombatDomain combatDomain, int characterId, DataContext context)
		{
			this._id = characterId;
			this._character = DomainManager.Character.GetElement_Objects(characterId);
			this._combatDomain = combatDomain;
			short charTemplateId = this._character.GetTemplateId();
			bool isBoss = CombatDomain.CharId2BossId.ContainsKey(charTemplateId);
			bool isAnimal = GameData.Domains.Combat.SharedConstValue.CharId2AnimalId.ContainsKey(charTemplateId);
			this.BossConfig = (isBoss ? Boss.Instance[CombatDomain.CharId2BossId[charTemplateId]] : null);
			this._bossPhase = 0;
			this.ChangeBossPhaseEffectId = -1;
			this.AnimalConfig = (isAnimal ? Config.Animal.Instance[GameData.Domains.Combat.SharedConstValue.CharId2AnimalId[charTemplateId]] : null);
			this._breathValue = 30000;
			this._stanceValue = 4000;
			this._oldDisorderOfQi = this._character.GetDisorderOfQi();
			this._neiliType = (this.OriginNeiliType = this._character.GetNeiliType());
			this._avoidToShow.HitType = -1;
			this._currentPosition = (int)(combatDomain.GetCurrentDistance() / 2 * (this.IsAlly ? -1 : 1));
			this._displayPosition = int.MinValue;
			this._mobilityValue = MoveSpecialConstants.MaxMobility;
			this._mobilityLevel = 2;
			this._targetDistance = -1;
			this._mobilityLockEffectCount = 0;
			this._jumpChangeDistanceDuration = -1f;
			this.KeepMoving = false;
			this.PlayerControllingMove = false;
			this.AiTargetDistance = -1;
			this.PlayerTargetDistance = -1;
			this.PlayerChangeTrickType = (this.PlayerChangeTrickBodyPart = -1);
			this.MoveData.Init(context, this);
			this.NeedPauseJumpMove = false;
			ItemKey[] equipments = this._character.GetEquipment();
			sbyte[] weaponSlots = EquipmentSlot.EquipmentType2Slots[0];
			bool flag = isBoss && this.BossConfig.PhaseWeapons != null;
			if (flag)
			{
				short[] weaponList = this.BossConfig.PhaseWeapons[0];
				for (int i = 0; i < weaponSlots.Length; i++)
				{
					bool flag2 = equipments[(int)weaponSlots[i]].IsValid();
					if (flag2)
					{
						this._character.ChangeEquipment(context, weaponSlots[i], -1, ItemKey.Invalid);
					}
				}
				for (int j = 0; j < weaponList.Length; j++)
				{
					ItemKey itemKey = DomainManager.Item.CreateWeapon(context, weaponList[j], 0);
					this._character.AddInventoryItem(context, itemKey, 1, false);
					this._character.ChangeEquipment(context, -1, weaponSlots[j], itemKey);
				}
			}
			for (int k = 0; k < weaponSlots.Length; k++)
			{
				ItemKey weaponKey = equipments[(int)weaponSlots[k]];
				this._weapons[k] = weaponKey;
				bool flag3 = weaponKey.IsValid();
				if (flag3)
				{
					DomainManager.SpecialEffect.AddEquipmentEffect(context, this._id, weaponKey);
				}
			}
			bool allowFreeWeapon = this._character.GetAllowUseFreeWeapon();
			this._weapons[3] = (allowFreeWeapon ? DomainManager.Item.CreateWeapon(context, 0, 0) : ItemKey.Invalid);
			this._weapons[4] = (allowFreeWeapon ? DomainManager.Item.CreateWeapon(context, 1, 0) : ItemKey.Invalid);
			this._weapons[5] = (allowFreeWeapon ? DomainManager.Item.CreateWeapon(context, 2, 0) : ItemKey.Invalid);
			this._weapons[6] = (allowFreeWeapon ? DomainManager.Item.CreateWeapon(context, 884, 0) : ItemKey.Invalid);
			for (sbyte l = 0; l < 7; l += 1)
			{
				ItemKey armorKey = equipments[(int)EquipmentSlotHelper.GetSlotByBodyPartType(l)];
				this.Armors[(int)l] = ((armorKey.IsValid() && DomainManager.Item.GetElement_Armors(armorKey.Id).GetCurrDurability() > 0) ? armorKey : ItemKey.Invalid);
				bool flag4 = armorKey.IsValid();
				if (flag4)
				{
					DomainManager.SpecialEffect.AddEquipmentEffect(context, this._id, armorKey);
				}
			}
			this._usingWeaponIndex = -1;
			this._weaponTrickIndex = 0;
			this._changeTrickProgress = 0;
			this._changeTrickCount = 0;
			this._canChangeTrick = false;
			this._attackingTrickType = -1;
			this.ForbidNormalAttackEffectCount = 0;
			this.CanNormalAttackInPrepareSkill = false;
			this.NeedBreakAttack = false;
			this.IsBreakAttacking = false;
			this.NeedNormalAttackImmediate = false;
			this.NeedNormalAttackSkipPrepare = 0;
			this.NormalAttackBodyPart = -1;
			this.NormalAttackHitType = -1;
			this.PursueAttackCount = 0;
			this.NormalAttackLeftRepeatTimes = 0;
			this.ChangeTrickType = -1;
			this.ChangeTrickBodyPart = -1;
			this.NeedChangeTrickAttack = false;
			this.UnlockWeaponIndex = -1;
			this.FightBackHitType = -1;
			this.IsAutoNormalAttackingSpecial = false;
			this.NeedReduceWeaponDurability = false;
			this.NeedReduceArmorDurability = false;
			this._changingTrick = false;
			this._changeTrickAttack = false;
			this._unlockPrepareValue.Clear();
			for (int m = 0; m < 3; m++)
			{
				this._unlockPrepareValue.Add(0);
			}
			this._tricks.ClearTricks();
			this._maxTrickCount = 0;
			this._defeatMarkCollection = new DefeatMarkCollection();
			this.Immortal = !Config.Character.Instance[this._character.GetTemplateId()].CanDefeat;
			this._defeatMarkUid = new DataUid(8, 10, (ulong)((long)this._id), 50U);
			GameDataBridge.AddPostDataModificationHandler(this._defeatMarkUid, this.DataHandlerKey, new Action<DataContext, DataUid>(this.OnDefeatMarkChanged));
			this.RegisterMarkHandler();
			this._injuries = (this._oldInjuries = this._character.GetInjuries());
			this._injuryAutoHealCollection = new InjuryAutoHealCollection();
			this._oldInjuryAutoHealCollection = new InjuryAutoHealCollection();
			this._damageStepCollection = combatDomain.GetDamageStepCollection(this._id);
			for (sbyte part = 0; part < 7; part += 1)
			{
				this._flawCount[(int)part] = 0;
				this._acupointCount[(int)part] = 0;
			}
			this._flawCollection = new FlawOrAcupointCollection();
			this._acupointCollection = new FlawOrAcupointCollection();
			this._mindMarkTime = new MindMarkList();
			for (sbyte part2 = 0; part2 < 7; part2 += 1)
			{
				this._outerDamageValueToShow[(int)part2] = new IntPair(-1, -1);
				this._innerDamageValueToShow[(int)part2] = new IntPair(-1, -1);
			}
			this._mindDamageValueToShow = -1;
			this._fatalDamageValueToShow = -1;
			this._poison = (this._oldPoison = *this._character.GetPoisoned());
			this._poisonResist = *this._character.GetPoisonResists();
			Array.Clear(this._poisonAffectAccumulator, 0, 6);
			this._poisonResistUid = new DataUid(4, 0, (ulong)((long)this._id), 94U);
			GameDataBridge.AddPostDataModificationHandler(this._poisonResistUid, this.DataHandlerKey, new Action<DataContext, DataUid>(this.OnPoisonResistChanged));
			this._mixPoisonAffectedCount.Clear();
			this._neiliAllocation = default(NeiliAllocation);
			this._originNeiliAllocation = default(NeiliAllocation);
			this._originBaseNeiliAllocation = default(NeiliAllocation);
			NeiliAllocation neiliAllocation = this._character.GetNeiliAllocation();
			NeiliAllocation baseNeiliAllocation = this._character.GetBaseNeiliAllocation();
			for (int n = 0; n < 4; n++)
			{
				*(ref this._neiliAllocation.Items.FixedElementField + (IntPtr)n * 2) = *(ref neiliAllocation.Items.FixedElementField + (IntPtr)n * 2);
				*(ref this._originNeiliAllocation.Items.FixedElementField + (IntPtr)n * 2) = *(ref neiliAllocation.Items.FixedElementField + (IntPtr)n * 2);
				*(ref this._originBaseNeiliAllocation.Items.FixedElementField + (IntPtr)n * 2) = *(ref baseNeiliAllocation.Items.FixedElementField + (IntPtr)n * 2);
				*(ref this._neiliAllocationRecoverProgress.Items.FixedElementField + (IntPtr)n * 2) = 0;
				this.NeiliAllocationAutoRecoverProgress[n] = 0;
			}
			this.OriginXiangshuInfection = (int)this._character.GetXiangshuInfection();
			this.InitSkillList(0, this._neigongList, null);
			this.InitSkillList(1, this._attackSkillList, this.BossConfig);
			this.InitSkillList(2, this._agileSkillList, null);
			this.InitSkillList(3, this._defenceSkillList, null);
			this.InitSkillList(4, this._assistSkillList, null);
			this.EnableEnterCombatSkillEffect(context, this._neigongList);
			this.EnableEnterCombatSkillEffect(context, this._attackSkillList);
			this.EnableEnterCombatSkillEffect(context, this._agileSkillList);
			this.EnableEnterCombatSkillEffect(context, this._defenceSkillList);
			this.EnableEnterCombatSkillEffect(context, this._assistSkillList);
			this.CanCastSkillCostBreath = true;
			this.CanCastSkillCostStance = true;
			this._preparingSkillId = -1;
			this._skillPreparePercent = 0;
			this._performingSkillId = -1;
			this._attackSkillPower = 0;
			this._affectingMoveSkillId = -1;
			this._affectingDefendSkillId = -1;
			this._defendSkillTimePercent = 0;
			this.CastFreeDataList.Clear();
			this.NeedAddEffectAgileSkillId = -1;
			this.DefendSkillTotalFrame = 0;
			this.DefendSkillLeftFrame = 0;
			this.PreventCastSkillEffectCount = 0;
			this.CanCastDirectSkill = true;
			this.CanCastReverseSkill = true;
			this._wugCount = 0;
			CombatResources usableCombatResources = DomainManager.Character.GetUsableCombatResources(this._id);
			sbyte usableHealingCount = usableCombatResources.HealingCount;
			bool allowHeal = Config.Character.Instance[this._character.GetTemplateId()].AllowHeal;
			this._healInjuryCount = (byte)((usableHealingCount > 0 && !isAnimal && allowHeal) ? usableHealingCount : 0);
			sbyte usableDetoxCount = usableCombatResources.DetoxCount;
			this._healPoisonCount = (byte)((usableDetoxCount > 0 && !isAnimal && allowHeal) ? usableDetoxCount : 0);
			this._preparingOtherAction = -1;
			this._otherActionPreparePercent = 0;
			this.UsingItem = ItemKey.Invalid;
			this._preparingItem = ItemKey.Invalid;
			this._useItemPreparePercent = 0;
			this.BuffCombatStatePowerExtraLimit = 0;
			this.DebuffCombatStatePowerExtraLimit = 0;
			this._xiangshuEffectId = -1;
			this._hazardValue = 0;
			this.NeedSelectMercyOption = false;
			this.NeedDelaySettlement = false;
			this.ChangeHitTypeEffectCount = 0;
			this.ChangeAvoidTypeEffectCount = 0;
			this.SpecialAnimationLoop = null;
			this._animationToLoop = null;
			this._animationToPlayOnce = null;
			this._particleToPlay = null;
			this._skillPetAnimation = null;
			this._petParticle = null;
			this._animationTimeScale = 1f;
			bool flag5 = this.IsAlly && this._combatDomain.IsMainCharacter(this);
			if (flag5)
			{
				HunterSkillsData hunterSkillsData = (HunterSkillsData)DomainManager.Extra.GetProfessionData(1).SkillsData;
				this._animalAttackCount = (sbyte)((hunterSkillsData != null) ? Math.Max((int)(3 - hunterSkillsData.UsedCarrierAnimalAttackCount), 0) : 0);
			}
			else
			{
				this._animalAttackCount = 0;
			}
			this.NeedAnimalAttack = false;
			this.ChangeCharId = -1;
			this.ChangeCharFailAni = null;
			Array.Clear(this.TeammateHasCommand, 0, this.TeammateHasCommand.Length);
			this.TeammateBeforeMainChar = -1;
			this.TeammateAfterMainChar = -1;
			this.ActingTeammateCommandChar = null;
			this._showTransferInjuryCommand = false;
			this._executingTeammateCommand = -1;
			this.NeedResetAdvanceTeammateCommandPushCd = false;
			this.NeedResetAdvanceTeammateCommandPullCd = false;
			this.ExecutingTeammateCommandSpecialEffect = -1L;
			this.ExecutingTeammateCommandIndex = -1;
			this.ExecutingTeammateCommandChangeDistance = 0;
			this.ExecutingTeammateCommandConfig = null;
			this._visible = false;
			this.TeammateCommandLeftPrepareFrame = 0;
			this._teammateCommandPreparePercent = 0;
			this.TeammateCommandLeftFrame = -1;
			this._teammateCommandTimePercent = 0;
			this._teammateExitAniLeftFrame = 0;
			this._attackCommandWeaponKey = ItemKey.Invalid;
			this._attackingTrickType = -1;
			this._attackCommandSkillId = -1;
			this._defendCommandSkillId = -1;
			this._showEffectCommandIndex = -1;
			this.CanRecoverMobility = true;
			this.AttackForceMissCount = 0;
			this.AttackForceHitCount = 0;
			this.SkillForceHit = false;
			this.OuterInjuryImmunity = false;
			this.InnerInjuryImmunity = false;
			this.MindImmunity = false;
			this.FlawImmunity = false;
			this.AcupointImmunity = false;
			this._combatReserveData = CombatReserveData.Invalid;
			this._reserveNormalAttack = false;
			this.StateMachine.Init(combatDomain, this);
			this.StateMachine.TranslateState(CombatCharacterStateType.Idle);
		}

		// Token: 0x06005F97 RID: 24471 RVA: 0x00366074 File Offset: 0x00364274
		private void InitSkillList(sbyte equipType, ICollection<short> skillList, BossItem bossConfig = null)
		{
			skillList.Clear();
			bool flag = bossConfig == null;
			if (flag)
			{
				foreach (short ptr in this._character.GetCombatSkillEquipment()[equipType])
				{
					short skillId = ptr;
					bool flag2 = skillId >= 0 && this._character.GetCombatSkillCanAffect(skillId);
					if (flag2)
					{
						skillList.Add(skillId);
					}
				}
			}
			else
			{
				foreach (short skillId2 in bossConfig.PhaseAttackSkills[0])
				{
					skillList.Add(skillId2);
				}
			}
		}

		// Token: 0x06005F98 RID: 24472 RVA: 0x00366114 File Offset: 0x00364314
		private void EnableEnterCombatSkillEffect(DataContext context, IEnumerable<short> skillListInCombat)
		{
			bool flag = !this._combatDomain.IsTeamCharacter(this._id);
			if (!flag)
			{
				foreach (short skillId in skillListInCombat)
				{
					DomainManager.SpecialEffect.Add(context, this._id, skillId, 1, -1);
				}
			}
		}

		// Token: 0x06005F99 RID: 24473 RVA: 0x00366188 File Offset: 0x00364388
		public void OnFrameBegin()
		{
			DataContext context = this.GetDataContext();
			bool flag = this.NeedAddEffectAgileSkillId >= 0;
			if (flag)
			{
				DomainManager.SpecialEffect.Add(context, this._id, this.NeedAddEffectAgileSkillId, 0, -1);
				this.NeedAddEffectAgileSkillId = -1;
			}
			bool skipOnFrameBegin = this.SkipOnFrameBegin;
			if (skipOnFrameBegin)
			{
				this.SkipOnFrameBegin = false;
			}
			else
			{
				bool flag2 = this._showEffectList.ShowEffectList.Count > 0;
				if (flag2)
				{
					this._showEffectList.ShowEffectList.Clear();
				}
				bool flag3 = this.NeedShowEffectList.Count > 0;
				if (flag3)
				{
					this._showEffectList.ShowEffectList.AddRange(this.NeedShowEffectList);
					this.NeedShowEffectList.Clear();
				}
				bool flag4 = this._showCommandList.Count > 0;
				if (flag4)
				{
					this._showCommandList.Clear();
				}
				bool flag5 = this.NeedShowCommandList.Count > 0;
				if (flag5)
				{
					this._showCommandList.AddRange(this.NeedShowCommandList);
					this.NeedShowCommandList.Clear();
				}
				for (sbyte part = 0; part < 7; part += 1)
				{
					this._outerDamageValueToShow[(int)part].First = -1;
					this._outerDamageValueToShow[(int)part].Second = -1;
					this._innerDamageValueToShow[(int)part].First = -1;
					this._innerDamageValueToShow[(int)part].Second = -1;
				}
				this.SetOuterDamageValueToShow(this._outerDamageValueToShow, context);
				this.SetInnerDamageValueToShow(this._innerDamageValueToShow, context);
				this.SetMindDamageValueToShow(-1, context);
				this.SetFatalDamageValueToShow(-1, context);
				bool flag6 = this._newPoisonsToShow.IsNonZero();
				if (flag6)
				{
					this._newPoisonsToShow.Initialize();
					this.SetNewPoisonsToShow(ref this._newPoisonsToShow, context);
				}
			}
		}

		// Token: 0x06005F9A RID: 24474 RVA: 0x0036635C File Offset: 0x0036455C
		public void OnFrameEnd()
		{
			bool flag = this._showEffectList.ShowEffectList.Count > 0;
			if (flag)
			{
				this.SetShowEffectList(this._showEffectList, this.GetDataContext());
			}
			bool flag2 = this._showCommandList.Count > 0;
			if (flag2)
			{
				this.SetShowCommandList(this._showCommandList, this.GetDataContext());
			}
		}

		// Token: 0x06005F9B RID: 24475 RVA: 0x003663BC File Offset: 0x003645BC
		public GameData.Domains.Character.Character GetCharacter()
		{
			return this._character;
		}

		// Token: 0x06005F9C RID: 24476 RVA: 0x003663D4 File Offset: 0x003645D4
		public unsafe void OnCombatEnd(DataContext context)
		{
			CValuePercent stayPercent = this._combatDomain.CombatConfig.StayPercent;
			bool canKeepDamage = this._character.GetCreatingType() == 1 || this._combatDomain.CombatConfig.AffectTemporaryCharacter || DomainManager.Taiwu.IsInGroup(this._character.GetId());
			bool keepDamage = canKeepDamage && (this.IsAlly || !this._combatDomain.CombatConfig.EnemyHealDamage);
			bool flag = keepDamage;
			if (flag)
			{
				for (sbyte part = 0; part < 7; part += 1)
				{
					ValueTuple<sbyte, sbyte> injury = this._injuries.Get(part);
					ValueTuple<sbyte, sbyte> oldInjury = this._oldInjuries.Get(part);
					int newOuter = (int)(injury.Item1 - oldInjury.Item1);
					int newInner = (int)(injury.Item2 - oldInjury.Item2);
					this._injuries.Set(part, false, (sbyte)((int)oldInjury.Item1 + newOuter * stayPercent));
					this._injuries.Set(part, true, (sbyte)((int)oldInjury.Item2 + newInner * stayPercent));
				}
			}
			else
			{
				this._injuries.Initialize();
			}
			this._character.SetInjuries(this._injuries, context);
			bool flag2 = keepDamage;
			if (flag2)
			{
				for (sbyte type = 0; type < 6; type += 1)
				{
					int newPoison = *this._poison[(int)type] - *this._oldPoison[(int)type];
					*this._poison[(int)type] = Math.Clamp(*this._oldPoison[(int)type] + newPoison * stayPercent, 0, 25000);
				}
			}
			else
			{
				this._poison.Initialize();
			}
			this._character.SetPoisoned(ref this._poison, context);
			bool flag3 = keepDamage;
			if (flag3)
			{
				int newQiDisorder = (int)(this._character.GetDisorderOfQi() - this._oldDisorderOfQi);
				int value = Math.Clamp((int)this._oldDisorderOfQi + newQiDisorder * stayPercent, 0, (int)DisorderLevelOfQi.MaxValue);
				this._character.SetDisorderOfQi((short)value, context);
			}
			else
			{
				this._character.SetDisorderOfQi(0, context);
			}
			bool flag4 = canKeepDamage;
			if (flag4)
			{
				sbyte unit = GlobalConfig.Instance.ReduceHealthPerFatalDamageMark[Math.Clamp((int)DomainManager.Combat.GetCombatType(), 0, GlobalConfig.Instance.ReduceHealthPerFatalDamageMark.Length - 1)];
				bool flag5 = this.IsAlly ? this._combatDomain.CombatConfig.SelfFatalDamageReduceHealth : this._combatDomain.CombatConfig.EnemyFatalDamageReduceHealth;
				if (flag5)
				{
					this._character.ChangeHealth(context, (int)(-(int)unit) * this._defeatMarkCollection.FatalDamageMarkCount);
				}
			}
			else
			{
				bool flag6 = !DomainManager.Combat.IsCharInLoot(this._id);
				if (flag6)
				{
					this._character.ClearEatingItems(context);
				}
			}
			List<short> learnedSkills = this._character.GetLearnedCombatSkills();
			for (int i = 0; i < this.ForgetAfterCombatSkills.Count; i++)
			{
				short skillId = this.ForgetAfterCombatSkills[i];
				learnedSkills.Remove(skillId);
				DomainManager.CombatSkill.RemoveCombatSkill(this._id, skillId);
			}
			this._character.SetLearnedCombatSkills(learnedSkills, context);
			foreach (ITeammateCommandInvoker invoker in this._teammateCommandInvokers)
			{
				invoker.Close();
			}
			this._teammateCommandInvokers.Clear();
			bool flag7 = canKeepDamage;
			if (flag7)
			{
				NeiliAllocation extraNeiliAllocation = this._character.GetExtraNeiliAllocation();
				int currNeili = this._character.GetCurrNeili();
				for (int j = 0; j < 4; j++)
				{
					*this._neiliAllocation[j] = (short)Math.Clamp((int)(*this._neiliAllocation[j] - *extraNeiliAllocation[j]), 0, (int)(*this._originBaseNeiliAllocation[j]));
				}
				this._character.SpecifyBaseNeiliAllocation(context, this._neiliAllocation);
				this._character.SpecifyCurrNeili(context, currNeili);
				bool flag8 = this.IsAlly && this._id == DomainManager.Taiwu.GetTaiwuCharId();
				if (flag8)
				{
					DomainManager.Taiwu.UpdateTaiwuNeiliAllocation(context, true);
				}
			}
			else
			{
				this._character.SpecifyBaseNeiliAllocation(context, this._originBaseNeiliAllocation);
			}
			AiController aiController = this.AiController;
			if (aiController != null)
			{
				aiController.UnInit();
			}
			GameDataBridge.RemovePostDataModificationHandler(this._poisonResistUid, this.DataHandlerKey);
			GameDataBridge.RemovePostDataModificationHandler(this._defeatMarkUid, this.DataHandlerKey);
			this.UnRegisterMarkHandler();
			bool flag9 = this.IsTaiwu && DomainManager.Combat.AiOptions.SaveMoveTarget;
			if (flag9)
			{
				DomainManager.Extra.SetLastTargetDistance(this.PlayerTargetDistance, context);
			}
			EventArgBox argBox = DomainManager.TaiwuEvent.GetGlobalEventArgumentBox();
			argBox.Set("IsGuardCombat", false);
		}

		// Token: 0x06005F9D RID: 24477 RVA: 0x003668E8 File Offset: 0x00364AE8
		public void RemoveTempWeapons(DataContext context)
		{
			DomainManager.Item.RemoveItem(context, this._weapons[3]);
			DomainManager.Item.RemoveItem(context, this._weapons[4]);
			DomainManager.Item.RemoveItem(context, this._weapons[5]);
			DomainManager.Item.RemoveItem(context, this._weapons[6]);
		}

		// Token: 0x06005F9E RID: 24478 RVA: 0x00366956 File Offset: 0x00364B56
		public void OfflineChangeInjuries(sbyte bodyPart, bool isInner, sbyte delta)
		{
			this._injuries.Change(bodyPart, isInner, (int)delta);
		}

		// Token: 0x06005F9F RID: 24479 RVA: 0x00366968 File Offset: 0x00364B68
		public DataContext GetDataContext()
		{
			return this._combatDomain.Context;
		}

		// Token: 0x06005FA0 RID: 24480 RVA: 0x00366988 File Offset: 0x00364B88
		[ObjectCollectionDependency(8, 10, new ushort[]
		{
			16,
			18,
			56,
			58,
			26,
			44
		}, Scope = InfluenceScope.Self)]
		[SingleValueDependency(8, new ushort[]
		{
			19
		}, Scope = InfluenceScope.AllCombatCharsInCombat)]
		[ObjectCollectionDependency(17, 2, new ushort[]
		{
			145,
			146,
			273
		}, Scope = InfluenceScope.CombatCharacterAffectedByTheSpecialEffects)]
		private OuterAndInnerShorts CalcAttackRange()
		{
			return this.CalcAttackRangeImmediate(-1, -1);
		}

		// Token: 0x06005FA1 RID: 24481 RVA: 0x003669A4 File Offset: 0x00364BA4
		[ObjectCollectionDependency(17, 2, new ushort[]
		{
			52
		}, Scope = InfluenceScope.CombatCharacterAffectedByTheSpecialEffects)]
		private sbyte CalcHappiness()
		{
			int happiness = (int)this._character.GetHappiness();
			happiness += DomainManager.SpecialEffect.GetModifyValue(this._id, 52, EDataModifyType.Add, -1, -1, -1, EDataSumType.All);
			happiness = Math.Clamp(happiness, -119, 119);
			return (sbyte)happiness;
		}

		// Token: 0x06005FA2 RID: 24482 RVA: 0x003669EC File Offset: 0x00364BEC
		[ObjectCollectionDependency(8, 10, new ushort[]
		{
			132,
			15
		}, Scope = InfluenceScope.Self)]
		[ObjectCollectionDependency(4, 0, new ushort[]
		{
			86
		}, Scope = InfluenceScope.CombatCharOfTheChar)]
		private float CalcChangeDistanceDuration()
		{
			bool flag = this.GetJumpChangeDistanceDuration() >= 0f;
			float result;
			if (flag)
			{
				result = this.GetJumpChangeDistanceDuration();
			}
			else
			{
				short mobilityMoveCd = this.GetMoveCd();
				result = (float)mobilityMoveCd / 60f;
			}
			return result;
		}

		// Token: 0x06005FA3 RID: 24483 RVA: 0x00366A2C File Offset: 0x00364C2C
		[ObjectCollectionDependency(8, 10, new ushort[]
		{
			114
		}, Scope = InfluenceScope.Self)]
		private void CalcTeammateCommandCanUse(List<bool> teammateCommandCanUse)
		{
			for (int i = 0; i < this._teammateCommandBanReasons.Count; i++)
			{
				teammateCommandCanUse[i] = (this._teammateCommandBanReasons[i].Items.Count == 0);
			}
		}

		// Token: 0x06005FA4 RID: 24484 RVA: 0x00366A78 File Offset: 0x00364C78
		[ObjectCollectionDependency(8, 10, new ushort[]
		{
			11
		}, Scope = InfluenceScope.Self)]
		private byte CalcMobilityLevel()
		{
			return CFormula.CalcMobilityLevel(this._mobilityValue);
		}

		// Token: 0x06005FA5 RID: 24485 RVA: 0x00366A98 File Offset: 0x00364C98
		[ObjectCollectionDependency(8, 29, new ushort[]
		{
			2,
			3
		}, Scope = InfluenceScope.CombatCharOfTheCombatSkillData)]
		[ObjectCollectionDependency(8, 30, new ushort[]
		{
			7,
			8
		}, Scope = InfluenceScope.CombatCharOfTheCombatWeaponData)]
		private void CalcSilenceData(SilenceData silenceData)
		{
			if (silenceData.CombatSkill == null)
			{
				silenceData.CombatSkill = new Dictionary<short, SilenceFrameData>();
			}
			silenceData.CombatSkill.Clear();
			foreach (CombatSkillKey key in this.GetCombatSkillKeys())
			{
				CombatSkillData skillData;
				bool flag = !DomainManager.Combat.TryGetCombatSkillData(this._id, key.SkillTemplateId, out skillData);
				if (!flag)
				{
					bool flag2 = skillData.GetLeftCdFrame() == 0;
					if (!flag2)
					{
						SilenceFrameData frameData = SilenceFrameData.Create((int)skillData.GetTotalCdFrame(), (int)skillData.GetLeftCdFrame());
						silenceData.CombatSkill[key.SkillTemplateId] = frameData;
					}
				}
			}
			if (silenceData.WeaponKeys == null)
			{
				silenceData.WeaponKeys = new List<ItemKey>();
			}
			silenceData.WeaponKeys.Clear();
			if (silenceData.WeaponFrames == null)
			{
				silenceData.WeaponFrames = new List<SilenceFrameData>();
			}
			silenceData.WeaponFrames.Clear();
			foreach (ItemKey itemKey in this._weapons)
			{
				CombatWeaponData weaponData;
				bool flag3 = !DomainManager.Combat.TryGetElement_WeaponDataDict(itemKey.Id, out weaponData);
				if (!flag3)
				{
					bool flag4 = weaponData.GetFixedCdLeftFrame() == 0;
					if (!flag4)
					{
						SilenceFrameData frameData2 = SilenceFrameData.Create((int)weaponData.GetFixedCdTotalFrame(), (int)weaponData.GetFixedCdLeftFrame());
						silenceData.WeaponKeys.Add(itemKey);
						silenceData.WeaponFrames.Add(frameData2);
					}
				}
			}
		}

		// Token: 0x06005FA6 RID: 24486 RVA: 0x00366C2C File Offset: 0x00364E2C
		[ObjectCollectionDependency(8, 10, new ushort[]
		{
			76,
			77
		}, Scope = InfluenceScope.Self)]
		private int CalcCombatStateTotalBuffPower()
		{
			int buffPower = this._buffCombatStateCollection.StateDict.Values.Select(new Func<ValueTuple<short, bool, int>, int>(CombatCharacter.PowerSelector)).Sum();
			int debuffPower = this._debuffCombatStateCollection.StateDict.Values.Select(new Func<ValueTuple<short, bool, int>, int>(CombatCharacter.PowerSelector)).Sum();
			return buffPower - debuffPower;
		}

		// Token: 0x06005FA7 RID: 24487 RVA: 0x00366C8F File Offset: 0x00364E8F
		private static int PowerSelector([TupleElementNames(new string[]
		{
			"power",
			"reverse",
			"srcCharId"
		})] ValueTuple<short, bool, int> tuple)
		{
			return (int)tuple.Item1;
		}

		// Token: 0x06005FA8 RID: 24488 RVA: 0x00366C98 File Offset: 0x00364E98
		[ObjectCollectionDependency(8, 10, new ushort[]
		{
			29
		}, Scope = InfluenceScope.Self)]
		[ObjectCollectionDependency(17, 2, new ushort[]
		{
			168,
			169
		}, Scope = InfluenceScope.CombatCharacterAffectedByTheSpecialEffects)]
		private HeavyOrBreakInjuryData CalcHeavyOrBreakInjuryData()
		{
			HeavyOrBreakInjuryData result = default(HeavyOrBreakInjuryData);
			result.Initialize();
			for (sbyte i = 0; i < 7; i += 1)
			{
				bool flag = DomainManager.Combat.CheckBodyPartInjury(this, i, false);
				if (flag)
				{
					result[(int)i] = EHeavyOrBreakType.Break;
				}
				else
				{
					bool flag2 = DomainManager.Combat.CheckBodyPartInjury(this, i, true);
					if (flag2)
					{
						result[(int)i] = EHeavyOrBreakType.Heavy;
					}
				}
			}
			return result;
		}

		// Token: 0x06005FA9 RID: 24489 RVA: 0x00366D0C File Offset: 0x00364F0C
		[ObjectCollectionDependency(8, 10, new ushort[]
		{
			44,
			62
		}, Scope = InfluenceScope.Self)]
		[ObjectCollectionDependency(4, 0, new ushort[]
		{
			86
		}, Scope = InfluenceScope.CombatCharOfTheChar)]
		private short CalcMoveCd()
		{
			short moveSpeed = this._character.GetMoveSpeed();
			int moveCd = CFormula.CalcMoveCd((int)moveSpeed);
			bool flag = this._affectingMoveSkillId >= 0;
			if (flag)
			{
				moveCd *= (int)Config.CombatSkill.Instance[this._affectingMoveSkillId].MoveCdBonus;
			}
			int acupointAddPercent = this._acupointCollection.CalcAcupointParam(5) + this._acupointCollection.CalcAcupointParam(6);
			moveCd += moveCd * acupointAddPercent / 100;
			return (short)moveCd;
		}

		// Token: 0x06005FAA RID: 24490 RVA: 0x00366D8C File Offset: 0x00364F8C
		[ObjectCollectionDependency(17, 2, new ushort[]
		{
			197
		}, Scope = InfluenceScope.CombatCharacterAffectedByTheSpecialEffects)]
		private int CalcMobilityRecoverSpeed()
		{
			return DomainManager.SpecialEffect.ModifyValue(this._id, 197, MoveSpecialConstants.MobilityRecoverSpeed, -1, -1, -1, 0, 0, 0, 0);
		}

		// Token: 0x06005FAB RID: 24491 RVA: 0x00366DC0 File Offset: 0x00364FC0
		[ObjectCollectionDependency(8, 10, new ushort[]
		{
			124
		}, Scope = InfluenceScope.Self)]
		[SingleValueDependency(8, new ushort[]
		{
			4
		}, Scope = InfluenceScope.AllCombatCharsInCombat)]
		[ObjectCollectionDependency(17, 2, new ushort[]
		{
			145,
			146,
			273
		}, Scope = InfluenceScope.CombatCharacterAffectedByTheSpecialEffects)]
		private void CalcCanUnlockAttack(List<bool> canUnlockAttack)
		{
			int needUnlockAttackWeaponIndex = this._combatReserveData.NeedUnlockWeaponIndex;
			canUnlockAttack.Clear();
			for (int i = 0; i < this._unlockPrepareValue.Count; i++)
			{
				canUnlockAttack.Add(this.CalcCanUnlockAttackByWeaponIndex(i));
				bool flag = needUnlockAttackWeaponIndex == i && !canUnlockAttack[i];
				if (flag)
				{
					this.SetCombatReserveData(CombatReserveData.Invalid, this._combatDomain.Context);
				}
			}
		}

		// Token: 0x06005FAC RID: 24492 RVA: 0x00366E38 File Offset: 0x00365038
		private bool CalcCanUnlockAttackByWeaponIndex(int weaponIndex)
		{
			bool flag = !this._weapons[weaponIndex].IsValid();
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				bool flag2 = this._unlockPrepareValue[weaponIndex] < GlobalConfig.Instance.UnlockAttackUnit;
				if (flag2)
				{
					result = false;
				}
				else
				{
					WeaponUnlockEffectItem config = this.GetUnlockEffect(weaponIndex);
					bool flag3 = config == null;
					if (flag3)
					{
						result = false;
					}
					else
					{
						bool ignoreAttackRange = config.IgnoreAttackRange;
						if (ignoreAttackRange)
						{
							result = true;
						}
						else
						{
							short num;
							short num2;
							this.CalcAttackRangeImmediate(-1, weaponIndex).Deconstruct(out num, out num2);
							short min = num;
							short max = num2;
							short currentDistance = DomainManager.Combat.GetCurrentDistance();
							result = (min <= currentDistance && currentDistance <= max);
						}
					}
				}
			}
			return result;
		}

		// Token: 0x06005FAD RID: 24493 RVA: 0x00366EF0 File Offset: 0x003650F0
		[ObjectCollectionDependency(17, 2, new ushort[]
		{
			170
		}, Scope = InfluenceScope.CombatCharacterAffectedByTheSpecialEffects)]
		private int CalcMaxTrickCount()
		{
			int maxTrickCount = 9;
			maxTrickCount = DomainManager.SpecialEffect.ModifyData(this._id, -1, 170, maxTrickCount, -1, -1, -1);
			return Math.Max(maxTrickCount, 0);
		}

		// Token: 0x06005FAE RID: 24494 RVA: 0x00366F28 File Offset: 0x00365128
		[ObjectCollectionDependency(8, 10, new ushort[]
		{
			144
		}, Scope = InfluenceScope.Self)]
		private void CalcValidItems(List<ItemKey> validItems)
		{
			validItems.Clear();
			foreach (ItemKeyAndCount itemKeyAndCount in this.GetValidItemAndCounts())
			{
				validItems.Add(itemKeyAndCount.ItemKey);
			}
		}

		// Token: 0x06005FAF RID: 24495 RVA: 0x00366F8C File Offset: 0x0036518C
		[ObjectCollectionDependency(4, 0, new ushort[]
		{
			58
		}, Scope = InfluenceScope.CombatCharOfTheChar)]
		[ObjectCollectionDependency(17, 2, new ushort[]
		{
			325
		}, Scope = InfluenceScope.CombatCharacterAffectedByTheSpecialEffects)]
		private void CalcValidItemAndCounts(List<ItemKeyAndCount> validItemAndCounts)
		{
			validItemAndCounts.Clear();
			bool isAlly = this.IsAlly;
			if (isAlly)
			{
				validItemAndCounts.Add(DomainManager.Extra.GetEmptyToolKey(DomainManager.Combat.Context));
			}
			foreach (KeyValuePair<ItemKey, int> keyValuePair in this._character.GetInventory().Items)
			{
				ItemKey itemKey2;
				int num;
				keyValuePair.Deconstruct(out itemKey2, out num);
				ItemKey itemKey = itemKey2;
				int count = num;
				bool flag = this.ItemIsValid(itemKey);
				if (flag)
				{
					validItemAndCounts.Add(new ItemKeyAndCount(itemKey, count));
				}
			}
			DomainManager.SpecialEffect.ModifyData(this._id, -1, 325, validItemAndCounts, -1, -1, -1);
		}

		// Token: 0x06005FB0 RID: 24496 RVA: 0x00367060 File Offset: 0x00365260
		private bool ItemIsValid(ItemKey itemKey)
		{
			bool flag = itemKey.GetConsumedFeatureMedals() < 0;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				bool flag2 = itemKey.ItemType == 5;
				if (flag2)
				{
					result = DomainManager.Extra.IsProfessionalSkillUnlockedAndEquipped(44);
				}
				else
				{
					bool flag3 = itemKey.ItemType != 12;
					if (flag3)
					{
						result = true;
					}
					else
					{
						MiscItem config = Config.Misc.Instance[itemKey.TemplateId];
						List<short> requireCombatConfig = config.RequireCombatConfig;
						result = (requireCombatConfig == null || requireCombatConfig.Count <= 0 || config.RequireCombatConfig.Contains(DomainManager.Combat.CombatConfig.TemplateId));
					}
				}
			}
			return result;
		}

		// Token: 0x06005FB1 RID: 24497 RVA: 0x003670FA File Offset: 0x003652FA
		private static bool SkillIdIsValid(short skillId)
		{
			return skillId >= 0;
		}

		// Token: 0x06005FB2 RID: 24498 RVA: 0x00367103 File Offset: 0x00365303
		public void SyncPoisonData(DataContext context)
		{
			this._character.SetPoisoned(ref this._poison, context);
		}

		// Token: 0x06005FB3 RID: 24499 RVA: 0x0036711C File Offset: 0x0036531C
		public IEnumerable<short> GetCombatSkillIds()
		{
			IEnumerable<short> combatSkillIds = Enumerable.Empty<short>();
			bool flag = this._neigongList != null;
			if (flag)
			{
				combatSkillIds = combatSkillIds.Concat(this._neigongList.Where(new Func<short, bool>(CombatCharacter.SkillIdIsValid)));
			}
			bool flag2 = this._attackSkillList != null;
			if (flag2)
			{
				combatSkillIds = combatSkillIds.Concat(this._attackSkillList.Where(new Func<short, bool>(CombatCharacter.SkillIdIsValid)));
			}
			bool flag3 = this._agileSkillList != null;
			if (flag3)
			{
				combatSkillIds = combatSkillIds.Concat(this._agileSkillList.Where(new Func<short, bool>(CombatCharacter.SkillIdIsValid)));
			}
			bool flag4 = this._defenceSkillList != null;
			if (flag4)
			{
				combatSkillIds = combatSkillIds.Concat(this._defenceSkillList.Where(new Func<short, bool>(CombatCharacter.SkillIdIsValid)));
			}
			bool flag5 = this._assistSkillList != null;
			if (flag5)
			{
				combatSkillIds = combatSkillIds.Concat(this._assistSkillList.Where(new Func<short, bool>(CombatCharacter.SkillIdIsValid)));
			}
			return combatSkillIds;
		}

		// Token: 0x06005FB4 RID: 24500 RVA: 0x00367214 File Offset: 0x00365414
		public IEnumerable<CombatSkillKey> GetCombatSkillKeys()
		{
			return from skillId in this.GetCombatSkillIds()
			select new ValueTuple<int, short>(this._id, skillId);
		}

		// Token: 0x06005FB5 RID: 24501 RVA: 0x0036723D File Offset: 0x0036543D
		public IEnumerable<short> GetBannedSkillIds(bool requireNotInfinity = false)
		{
			foreach (CombatSkillKey key in this.GetCombatSkillKeys())
			{
				CombatSkillData data;
				bool flag = !DomainManager.Combat.TryGetCombatSkillData(key.CharId, key.SkillTemplateId, out data);
				if (!flag)
				{
					bool flag2 = requireNotInfinity && data.GetLeftCdFrame() < 0;
					if (!flag2)
					{
						bool flag3 = data.GetLeftCdFrame() != 0;
						if (flag3)
						{
							yield return key.SkillTemplateId;
						}
						data = null;
						key = default(CombatSkillKey);
					}
				}
			}
			IEnumerator<CombatSkillKey> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06005FB6 RID: 24502 RVA: 0x00367254 File Offset: 0x00365454
		public IEnumerable<short> GetBanableSkillIds(sbyte specifyEquipType = -1, sbyte expectEquipType = -1)
		{
			foreach (CombatSkillKey key in this.GetCombatSkillKeys())
			{
				sbyte configEquipType = Config.CombatSkill.Instance[key.SkillTemplateId].EquipType;
				bool flag = configEquipType == 0;
				if (!flag)
				{
					bool flag2 = specifyEquipType >= 0 && configEquipType != specifyEquipType;
					if (!flag2)
					{
						bool flag3 = expectEquipType >= 0 && configEquipType == expectEquipType;
						if (!flag3)
						{
							CombatSkillData data;
							bool flag4 = !DomainManager.Combat.TryGetCombatSkillData(key.CharId, key.SkillTemplateId, out data);
							if (!flag4)
							{
								bool flag5 = data.GetLeftCdFrame() < 0;
								if (!flag5)
								{
									yield return key.SkillTemplateId;
									data = null;
									key = default(CombatSkillKey);
								}
							}
						}
					}
				}
			}
			IEnumerator<CombatSkillKey> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06005FB7 RID: 24503 RVA: 0x00367274 File Offset: 0x00365474
		public short GetRandomBanableSkillId(IRandomSource random, Func<short, bool> predicate = null, sbyte specifyEquipType = -1)
		{
			using (IEnumerator<short> enumerator = this.GetRandomUnrepeatedBanableSkillIds(random, 1, predicate, specifyEquipType, -1).GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					return enumerator.Current;
				}
			}
			return -1;
		}

		// Token: 0x06005FB8 RID: 24504 RVA: 0x003672CC File Offset: 0x003654CC
		public IEnumerable<short> GetRandomUnrepeatedBanableSkillIds(IRandomSource random, int maxCount, Func<short, bool> predicate = null, sbyte specifyEquipType = -1, sbyte expectEquipType = -1)
		{
			bool flag = maxCount <= 0;
			if (flag)
			{
				yield break;
			}
			List<short> prefer = ObjectPool<List<short>>.Instance.Get();
			List<short> normal = ObjectPool<List<short>>.Instance.Get();
			prefer.Clear();
			normal.Clear();
			IEnumerable<short> banableSkillIds = this.GetBanableSkillIds(specifyEquipType, expectEquipType);
			Func<short, bool> <>9__0;
			Func<short, bool> predicate2;
			if ((predicate2 = <>9__0) == null)
			{
				predicate2 = (<>9__0 = ((short skillId) => predicate == null || predicate(skillId)));
			}
			foreach (short skillId3 in banableSkillIds.Where(predicate2))
			{
				CombatSkillData data = DomainManager.Combat.GetCombatSkillData(this._id, skillId3);
				bool flag2 = data.GetLeftCdFrame() == 0;
				if (flag2)
				{
					prefer.Add(skillId3);
				}
				else
				{
					normal.Add(skillId3);
				}
				data = null;
			}
			IEnumerator<short> enumerator = null;
			foreach (short skillId2 in RandomUtils.GetRandomUnrepeated<short>(random, maxCount, prefer, normal))
			{
				yield return skillId2;
			}
			IEnumerator<short> enumerator2 = null;
			ObjectPool<List<short>>.Instance.Return(prefer);
			ObjectPool<List<short>>.Instance.Return(normal);
			yield break;
			yield break;
		}

		// Token: 0x06005FB9 RID: 24505 RVA: 0x00367304 File Offset: 0x00365504
		public IReadOnlyList<short> GetCombatSkillList(sbyte equipType)
		{
			if (!true)
			{
			}
			List<short> result;
			switch (equipType)
			{
			case 0:
				result = this._neigongList;
				break;
			case 1:
				result = this._attackSkillList;
				break;
			case 2:
				result = this._agileSkillList;
				break;
			case 3:
				result = this._defenceSkillList;
				break;
			case 4:
				result = this._assistSkillList;
				break;
			default:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(25, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Invalid skill equip type ");
				defaultInterpolatedStringHandler.AppendFormatted<sbyte>(equipType);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			}
			if (!true)
			{
			}
			return result;
		}

		// Token: 0x06005FBA RID: 24506 RVA: 0x00367395 File Offset: 0x00365595
		public IEnumerable<sbyte> GetAvailableBodyParts()
		{
			sbyte b;
			for (sbyte i = 0; i < 7; i = b + 1)
			{
				bool flag = this.ContainsBodyPart(i);
				if (flag)
				{
					yield return i;
				}
				b = i;
			}
			yield break;
		}

		// Token: 0x06005FBB RID: 24507 RVA: 0x003673A8 File Offset: 0x003655A8
		public bool ContainsBodyPart(sbyte bodyPart)
		{
			if (!true)
			{
			}
			bool result;
			if (bodyPart >= 0)
			{
				switch (bodyPart)
				{
				case 3:
					result = this._character.GetHaveLeftArm();
					break;
				case 4:
					result = this._character.GetHaveRightArm();
					break;
				case 5:
					result = this._character.GetHaveLeftLeg();
					break;
				case 6:
					result = this._character.GetHaveRightLeg();
					break;
				default:
					result = true;
					break;
				}
			}
			else
			{
				result = false;
			}
			if (!true)
			{
			}
			return result;
		}

		// Token: 0x06005FBC RID: 24508 RVA: 0x00367424 File Offset: 0x00365624
		public int GetMaxBreathValue()
		{
			int maxValue = 30000;
			int percent = 100;
			percent = DomainManager.SpecialEffect.ModifyData(this._id, -1, 171, percent, -1, -1, -1);
			return maxValue * percent / 100;
		}

		// Token: 0x06005FBD RID: 24509 RVA: 0x00367464 File Offset: 0x00365664
		public int GetMaxStanceValue()
		{
			int maxValue = 4000;
			int percent = 100;
			percent = DomainManager.SpecialEffect.ModifyData(this._id, -1, 172, percent, -1, -1, -1);
			return maxValue * percent / 100;
		}

		// Token: 0x06005FBE RID: 24510 RVA: 0x003674A4 File Offset: 0x003656A4
		public int CalcBreathRecoverValue(int value)
		{
			value = DomainManager.SpecialEffect.ModifyValue(this._id, 195, value, -1, -1, -1, 0, 0, 0, 0);
			int acupointReduce = this._acupointCollection.CalcAcupointParam(0);
			value = value * (100 - acupointReduce) / 100;
			return value;
		}

		// Token: 0x06005FBF RID: 24511 RVA: 0x003674F0 File Offset: 0x003656F0
		public int CalcStanceRecoverValue(int value)
		{
			value = DomainManager.SpecialEffect.ModifyValue(this._id, 196, value, -1, -1, -1, 0, 0, 0, 0);
			int acupointReduce = this._acupointCollection.CalcAcupointParam(1);
			value = value * (100 - acupointReduce) / 100;
			return value;
		}

		// Token: 0x06005FC0 RID: 24512 RVA: 0x0036753C File Offset: 0x0036573C
		public int GetMaxMobility()
		{
			int maxValue = MoveSpecialConstants.MaxMobility;
			int percent = 100;
			percent = DomainManager.SpecialEffect.ModifyData(this._id, -1, 274, percent, -1, -1, -1);
			return maxValue * percent / 100;
		}

		// Token: 0x06005FC1 RID: 24513 RVA: 0x0036757C File Offset: 0x0036577C
		public void CalcCostTrickStatus(List<NeedTrick> costTricks, Dictionary<sbyte, byte> costTrickDict, Dictionary<sbyte, byte> lackTrickDict)
		{
			costTrickDict.Clear();
			lackTrickDict.Clear();
			foreach (NeedTrick needTrick in costTricks)
			{
				sbyte trickType3 = needTrick.TrickType;
				byte b = costTrickDict[needTrick.TrickType] = needTrick.NeedCount;
				lackTrickDict[trickType3] = b;
			}
			TrickCollection hasTricks = this.GetTricks();
			foreach (KeyValuePair<int, sbyte> keyValuePair in hasTricks.Tricks)
			{
				int num;
				sbyte b2;
				keyValuePair.Deconstruct(out num, out b2);
				sbyte trickType = b2;
				byte lackCount;
				bool flag = lackTrickDict.TryGetValue(trickType, out lackCount) && lackCount > 0;
				if (flag)
				{
					lackTrickDict[trickType] = lackCount - 1;
				}
			}
			List<sbyte> enoughTrickTypes = ObjectPool<List<sbyte>>.Instance.Get();
			enoughTrickTypes.Clear();
			foreach (KeyValuePair<sbyte, byte> keyValuePair2 in lackTrickDict)
			{
				byte b;
				sbyte b2;
				keyValuePair2.Deconstruct(out b2, out b);
				sbyte trickType2 = b2;
				byte lackCount2 = b;
				bool flag2 = lackCount2 == 0;
				if (flag2)
				{
					enoughTrickTypes.Add(trickType2);
				}
			}
			foreach (sbyte enoughTrickType in enoughTrickTypes)
			{
				lackTrickDict.Remove(enoughTrickType);
			}
			ObjectPool<List<sbyte>>.Instance.Return(enoughTrickTypes);
		}

		// Token: 0x06005FC2 RID: 24514 RVA: 0x00367738 File Offset: 0x00365938
		public void CalcInsteadTricks(Dictionary<sbyte, byte> insteadTrickDict, Func<sbyte, bool> insteadPredicate, Dictionary<sbyte, byte> costTrickDict, Dictionary<sbyte, byte> lackTrickDict, int maxInsteadCount = 2147483647, bool onlyInsteadLack = false)
		{
			insteadTrickDict.Clear();
			foreach (sbyte trickType in this.GetTricks().Tricks.Values)
			{
				bool flag = !insteadPredicate(trickType);
				if (!flag)
				{
					bool flag2 = costTrickDict.Count == 0 || (onlyInsteadLack && lackTrickDict.Count == 0);
					if (flag2)
					{
						break;
					}
					bool flag3 = maxInsteadCount <= 0;
					if (flag3)
					{
						break;
					}
					insteadTrickDict[trickType] = (byte)Math.Clamp((int)(insteadTrickDict.GetOrDefault(trickType) + 1), 0, 255);
					sbyte needTrickType = (lackTrickDict.Count > 0) ? lackTrickDict.First<KeyValuePair<sbyte, byte>>().Key : costTrickDict.First<KeyValuePair<sbyte, byte>>().Key;
					byte lackCount;
					bool flag4 = lackTrickDict.TryGetValue(needTrickType, out lackCount);
					if (flag4)
					{
						bool flag5 = lackCount <= 1;
						if (flag5)
						{
							lackTrickDict.Remove(needTrickType);
						}
						else
						{
							lackTrickDict[needTrickType] = lackCount - 1;
						}
					}
					Tester.Assert(costTrickDict[needTrickType] > 0, "");
					costTrickDict[needTrickType] -= 1;
					bool flag6 = costTrickDict[needTrickType] == 0;
					if (flag6)
					{
						costTrickDict.Remove(needTrickType);
					}
					maxInsteadCount--;
				}
			}
		}

		// Token: 0x06005FC3 RID: 24515 RVA: 0x003678BC File Offset: 0x00365ABC
		public void AddInfinityMindMarkProgress(DataContext context, int markCount)
		{
			int addProgress = GlobalConfig.Instance.MindMarkAddInfinityProgress;
			this._mindMarkInfinityProgress += DomainManager.SpecialEffect.ModifyValue(this._id, 305, markCount * addProgress, -1, -1, -1, 0, 0, 0, 0);
			int addMarkCount = 0;
			while (this._mindMarkInfinityProgress >= CFormula.CalcInfinityMindMarkProgress(this._mindMarkInfinityCount))
			{
				this._mindMarkInfinityProgress -= CFormula.CalcInfinityMindMarkProgress(this._mindMarkInfinityCount);
				this._mindMarkInfinityCount++;
				addMarkCount++;
			}
			bool flag = addMarkCount > 0;
			if (flag)
			{
				DomainManager.Combat.AppendMindDefeatMark(context, this, addMarkCount, -1, true);
			}
		}

		// Token: 0x06005FC4 RID: 24516 RVA: 0x00367964 File Offset: 0x00365B64
		public short GetAttackSpeedPercent()
		{
			return this._character.GetAttackSpeed();
		}

		// Token: 0x06005FC5 RID: 24517 RVA: 0x00367984 File Offset: 0x00365B84
		public short GetSkillPrepareSpeed()
		{
			return this._character.GetCastSpeed();
		}

		// Token: 0x06005FC6 RID: 24518 RVA: 0x003679A4 File Offset: 0x00365BA4
		public bool ChangeWugCount(DataContext context, int delta)
		{
			short prev = this.GetWugCount();
			short curr = (short)Math.Clamp((int)prev + delta, 0, (int)GlobalConfig.Instance.MaxWugCount);
			bool flag = curr != prev;
			if (flag)
			{
				this.SetWugCount(curr, context);
			}
			return curr - prev != 0;
		}

		// Token: 0x06005FC7 RID: 24519 RVA: 0x003679EC File Offset: 0x00365BEC
		public unsafe void ChangeToProportion(DataContext context, sbyte fiveElementsType, int maxChangeValue)
		{
			NeiliProportionOfFiveElements currentProportion = this._character.GetNeiliProportionOfFiveElements();
			int existValue = (int)(*currentProportion[(int)fiveElementsType]);
			int changeable = 100 - existValue;
			bool flag = maxChangeValue == 0;
			if (!flag)
			{
				bool flag2 = maxChangeValue < 0 && existValue == 0;
				if (!flag2)
				{
					bool flag3 = maxChangeValue > 0 && changeable == 0;
					if (!flag3)
					{
						maxChangeValue = Math.Min(maxChangeValue, changeable);
						NeiliProportionOfFiveElements delta = this._proportionDelta;
						int totalChangeValue = 0;
						for (int i = 0; i < 5; i++)
						{
							bool flag4 = i == (int)fiveElementsType;
							if (!flag4)
							{
								CValuePercent percent = CValuePercent.Parse((int)(*currentProportion[i]), changeable);
								int changeValue = maxChangeValue * percent;
								bool flag5 = changeValue == 0;
								if (!flag5)
								{
									totalChangeValue += changeValue;
									*delta[i] = (sbyte)((int)(*delta[i]) - changeValue);
								}
							}
						}
						*delta[(int)fiveElementsType] = (sbyte)((int)(*delta[(int)fiveElementsType]) + totalChangeValue);
						this.SetProportionDelta(delta, context);
						sbyte newNeiliType = this._character.GetNeiliType();
						bool flag6 = newNeiliType == this._neiliType;
						if (!flag6)
						{
							this.SetNeiliType(newNeiliType, context);
						}
					}
				}
			}
		}

		// Token: 0x06005FC8 RID: 24520 RVA: 0x00367B18 File Offset: 0x00365D18
		public void SilenceNeiliAllocationAutoRecover(DataContext context, int cdFrame)
		{
			bool flag = this._neiliAllocationCd.Cover(cdFrame);
			if (flag)
			{
				this.SetNeiliAllocationCd(this._neiliAllocationCd, context);
			}
		}

		// Token: 0x06005FC9 RID: 24521 RVA: 0x00367B44 File Offset: 0x00365D44
		public bool TickNeiliAllocationCd(DataContext context)
		{
			bool flag = this._neiliAllocationCd.Tick(1);
			if (flag)
			{
				this.SetNeiliAllocationCd(this._neiliAllocationCd, context);
			}
			return this._neiliAllocationCd.Silencing;
		}

		// Token: 0x06005FCA RID: 24522 RVA: 0x00367B80 File Offset: 0x00365D80
		public unsafe bool AnyLowerThanOriginNeiliAllocation()
		{
			for (byte i = 0; i < 4; i += 1)
			{
				bool flag = *this._neiliAllocation[(int)i] < *this._originNeiliAllocation[(int)i];
				if (flag)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06005FCB RID: 24523 RVA: 0x00367BC8 File Offset: 0x00365DC8
		public unsafe short GetMaxNeiliAllocation(byte type)
		{
			return *this.GetOriginNeiliAllocation()[(int)type] * 3;
		}

		// Token: 0x06005FCC RID: 24524 RVA: 0x00367BF0 File Offset: 0x00365DF0
		public int ApplySpecialEffectToNeiliAllocation(byte type, int addValue)
		{
			bool flag = addValue == 0;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				bool isAdd = addValue > 0;
				bool flag2 = isAdd;
				if (flag2)
				{
					float percent = (float)(100 + DomainManager.SpecialEffect.GetModifyValue(this._id, 135, EDataModifyType.AddPercent, (int)type, -1, -1, EDataSumType.All));
					percent += (float)this.GetAddNeiliAllocationAddPercent(type);
					ValueTuple<int, int> totalPercent = DomainManager.SpecialEffect.GetTotalPercentModifyValue(this._id, -1, 135, (int)type, -1, -1);
					addValue = (int)Math.Floor((double)((float)addValue * percent / 100f));
					addValue = (int)Math.Floor((double)((float)addValue * (100f + (float)totalPercent.Item1 + (float)totalPercent.Item2) / 100f));
					addValue = DomainManager.SpecialEffect.ModifyData(this._id, -1, 135, addValue, -1, -1, -1);
				}
				else
				{
					float percent2 = (float)(100 + DomainManager.SpecialEffect.GetModifyValue(this._id, 136, EDataModifyType.AddPercent, (int)type, -1, -1, EDataSumType.All));
					percent2 += (float)this.GetCostNeiliAllocationAddPercent(type);
					ValueTuple<int, int> totalPercent2 = DomainManager.SpecialEffect.GetTotalPercentModifyValue(this._id, -1, 136, (int)type, -1, -1);
					addValue = (int)Math.Ceiling((double)((float)addValue * percent2 / 100f));
					addValue = (int)Math.Ceiling((double)((float)addValue * (100f + (float)totalPercent2.Item1 + (float)totalPercent2.Item2) / 100f));
					addValue = DomainManager.SpecialEffect.ModifyData(this._id, -1, 136, addValue, -1, -1, -1);
				}
				bool flag3 = addValue != 0;
				if (flag3)
				{
					isAdd = (addValue > 0);
				}
				addValue = (isAdd ? Math.Max(addValue, 1) : Math.Min(addValue, -1));
				result = addValue;
			}
			return result;
		}

		// Token: 0x06005FCD RID: 24525 RVA: 0x00367D90 File Offset: 0x00365F90
		public bool ChangeNeiliAllocationRandom(DataContext context, int addValue, int count, bool applySpecialEffect = true)
		{
			bool changed = false;
			for (int i = 0; i < count; i++)
			{
				changed = (this.ChangeNeiliAllocationRandom(context, addValue, applySpecialEffect) || changed);
			}
			return changed;
		}

		// Token: 0x06005FCE RID: 24526 RVA: 0x00367DC4 File Offset: 0x00365FC4
		public unsafe bool ChangeNeiliAllocationRandom(DataContext context, int addValue, bool applySpecialEffect = true)
		{
			bool flag = addValue == 0;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				bool isAdd = addValue > 0;
				List<byte> neiliAllocationTypes = ObjectPool<List<byte>>.Instance.Get();
				neiliAllocationTypes.Clear();
				for (byte i = 0; i < 4; i += 1)
				{
					bool flag2 = isAdd ? (*this._originNeiliAllocation[(int)i] <= 0) : (*this._neiliAllocation[(int)i] <= 0);
					if (!flag2)
					{
						neiliAllocationTypes.Add(i);
					}
				}
				bool changed = neiliAllocationTypes.Count > 0;
				bool flag3 = changed;
				if (flag3)
				{
					this.ChangeNeiliAllocation(context, neiliAllocationTypes.GetRandom(context.Random), addValue, applySpecialEffect, true);
				}
				ObjectPool<List<byte>>.Instance.Return(neiliAllocationTypes);
				result = changed;
			}
			return result;
		}

		// Token: 0x06005FCF RID: 24527 RVA: 0x00367E8C File Offset: 0x0036608C
		public unsafe int ChangeNeiliAllocation(DataContext context, byte type, int addValue, bool applySpecialEffect = true, bool raiseEvent = true)
		{
			if (applySpecialEffect)
			{
				addValue = this.ApplySpecialEffectToNeiliAllocation(type, addValue);
				bool flag = addValue == 0;
				if (flag)
				{
					return 0;
				}
				bool canChange = DomainManager.SpecialEffect.ModifyData(this._id, -1, 137, true, (addValue > 0) ? 0 : 1, -1, -1);
				bool flag2 = !canChange;
				if (flag2)
				{
					return 0;
				}
			}
			short currValue = *this._neiliAllocation[(int)type];
			short newValue = (short)MathUtils.Clamp((int)currValue + addValue, 0, (int)this._combatDomain.GetMaxNeiliAllocation(this, type));
			int changeValue = (int)(newValue - currValue);
			*this._neiliAllocation[(int)type] = newValue;
			this.SetNeiliAllocation(this._neiliAllocation, context);
			this._defeatMarkCollection.SyncNeiliAllocationMark(context, this);
			this._combatDomain.UpdateTeammateCommandUsable(context, this, this._combatDomain.IsMainCharacter(this) ? ETeammateCommandImplement.ReduceNeiliAllocation : ETeammateCommandImplement.TransferNeiliAllocation);
			if (raiseEvent)
			{
				Events.RaiseNeiliAllocationChanged(context, this._id, type, changeValue);
			}
			return changeValue;
		}

		// Token: 0x06005FD0 RID: 24528 RVA: 0x00367F88 File Offset: 0x00366188
		public unsafe void ChangeAllNeiliAllocation(DataContext context, int addPercent, bool raiseEvent = true)
		{
			for (byte type = 0; type < 4; type += 1)
			{
				short currValue = *this._neiliAllocation[(int)type];
				short newValue = (short)MathUtils.Clamp((int)currValue + (int)currValue * addPercent / 100, 0, (int)this._combatDomain.GetMaxNeiliAllocation(this, type));
				int changeValue = (int)(newValue - currValue);
				*this._neiliAllocation[(int)type] = newValue;
				bool flag = !this._combatDomain.IsMainCharacter(this);
				if (flag)
				{
					this._combatDomain.UpdateTeammateCommandUsable(context, this, ETeammateCommandImplement.TransferNeiliAllocation);
				}
				if (raiseEvent)
				{
					Events.RaiseNeiliAllocationChanged(context, this._id, type, changeValue);
				}
			}
			this.SetNeiliAllocation(this._neiliAllocation, context);
			this._defeatMarkCollection.SyncNeiliAllocationMark(context, this);
		}

		// Token: 0x06005FD1 RID: 24529 RVA: 0x00368044 File Offset: 0x00366244
		private unsafe byte RandomAbsorbNeiliAllocationType(IRandomSource random, CombatCharacter target)
		{
			List<byte> prefer = ObjectPool<List<byte>>.Instance.Get();
			List<byte> normal = ObjectPool<List<byte>>.Instance.Get();
			for (byte i = 0; i < 4; i += 1)
			{
				bool flag = *target._neiliAllocation[(int)i] <= 0;
				if (!flag)
				{
					bool flag2 = *this._neiliAllocation[(int)i] < this.GetMaxNeiliAllocation(i);
					if (flag2)
					{
						prefer.Add(i);
					}
					else
					{
						normal.Add(i);
					}
				}
			}
			byte type = byte.MaxValue;
			bool flag3 = prefer.Count > 0;
			if (flag3)
			{
				type = prefer.GetRandom(random);
			}
			else
			{
				bool flag4 = normal.Count > 0;
				if (flag4)
				{
					type = normal.GetRandom(random);
				}
			}
			ObjectPool<List<byte>>.Instance.Return(prefer);
			ObjectPool<List<byte>>.Instance.Return(normal);
			return type;
		}

		// Token: 0x06005FD2 RID: 24530 RVA: 0x0036811C File Offset: 0x0036631C
		public unsafe bool AbsorbNeiliAllocation(DataContext context, CombatCharacter target, byte type, int value)
		{
			value = Math.Min(value, (int)(*target._neiliAllocation[(int)type]));
			bool flag = value <= 0;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				value = -target.ChangeNeiliAllocation(context, type, -value, true, true);
				this.ChangeNeiliAllocation(context, type, value, true, true);
				GameDataBridge.AddDisplayEvent<int, int, byte>(DisplayEventType.CombatShowAbsorbNeiliAllocation, target._id, this._id, type);
				result = true;
			}
			return result;
		}

		// Token: 0x06005FD3 RID: 24531 RVA: 0x00368188 File Offset: 0x00366388
		public bool AbsorbNeiliAllocationRandom(DataContext context, CombatCharacter target, int value)
		{
			byte type = this.RandomAbsorbNeiliAllocationType(context.Random, target);
			return type != byte.MaxValue && this.AbsorbNeiliAllocation(context, target, type, value);
		}

		// Token: 0x06005FD4 RID: 24532 RVA: 0x003681C0 File Offset: 0x003663C0
		public unsafe void StealNeiliAllocationRandom(DataContext context, CombatCharacter target, CValuePercent percent)
		{
			byte type = this.RandomAbsorbNeiliAllocationType(context.Random, target);
			bool flag = type == byte.MaxValue;
			if (!flag)
			{
				int value = Math.Max((int)(*target._neiliAllocation[(int)type]) * percent, 1);
				this.AbsorbNeiliAllocation(context, target, type, value);
			}
		}

		// Token: 0x06005FD5 RID: 24533 RVA: 0x00368210 File Offset: 0x00366410
		public unsafe void SetNeiliAllocationRecoverProgress(DataContext context, byte type, short percent)
		{
			bool flag = *(ref this._neiliAllocationRecoverProgress.Items.FixedElementField + (IntPtr)type * 2) == percent;
			if (!flag)
			{
				*(ref this._neiliAllocationRecoverProgress.Items.FixedElementField + (IntPtr)type * 2) = percent;
				this.SetNeiliAllocationRecoverProgress(this._neiliAllocationRecoverProgress, context);
			}
		}

		// Token: 0x06005FD6 RID: 24534 RVA: 0x00368264 File Offset: 0x00366464
		public void AddOrUpdateFlawOrAcupoint(DataContext context, sbyte bodyPart, bool isFlaw, sbyte level, bool raiseEvent = true, int leftFrames = -1, int totalFrames = -1)
		{
			bool flag = isFlaw ? this.GetFlawImmunity() : this.GetAcupointImmunity();
			if (flag)
			{
				DomainManager.Combat.ShowImmunityEffectTips(context, this._id, isFlaw ? EMarkType.Flaw : EMarkType.Acupoint);
			}
			else
			{
				FlawOrAcupointCollection dataDict = isFlaw ? this._flawCollection : this._acupointCollection;
				byte[] countList = isFlaw ? this._flawCount : this._acupointCount;
				List<ValueTuple<sbyte, int, int>> dataList = dataDict.BodyPartDict[bodyPart];
				int keepFrames = isFlaw ? GlobalConfig.Instance.FlawBaseKeepTime[(int)level] : GlobalConfig.Instance.AcupointBaseKeepTime[(int)level];
				bool addNew = dataList.Count < (isFlaw ? this.GetMaxFlawCount() : this.GetMaxAcupointCount());
				bool canAddNew = DomainManager.SpecialEffect.ModifyData(this._id, -1, isFlaw ? 126 : 131, true, (int)bodyPart, -1, -1);
				bool flag2 = addNew && !canAddNew;
				if (!flag2)
				{
					bool flag3 = addNew;
					if (flag3)
					{
						byte[] array = countList;
						array[(int)bodyPart] = array[(int)bodyPart] + 1;
						dataList.Add(new ValueTuple<sbyte, int, int>(level, (totalFrames > 0) ? totalFrames : keepFrames, (leftFrames > 0) ? leftFrames : keepFrames));
						if (isFlaw)
						{
							this.SetFlawCount(countList, context);
							this.SetFlawCollection(dataDict, context);
							if (raiseEvent)
							{
								Events.RaiseFlawAdded(context, this, bodyPart, level);
							}
						}
						else
						{
							this.SetAcupointCount(countList, context);
							this.SetAcupointCollection(dataDict, context);
							if (raiseEvent)
							{
								Events.RaiseAcuPointAdded(context, this, bodyPart, level);
							}
						}
						bool flag4 = this._combatDomain.IsMainCharacter(this);
						if (flag4)
						{
							this._combatDomain.UpdateAllTeammateCommandUsable(context, this.IsAlly, isFlaw ? ETeammateCommandImplement.HealFlaw : ETeammateCommandImplement.HealAcupoint);
						}
					}
					else
					{
						int replaceIndex = 0;
						int replaceLevel = int.MaxValue;
						for (int i = 0; i < dataList.Count; i++)
						{
							bool flag5 = (int)dataList[i].Item1 < Math.Min((int)level, replaceLevel);
							if (flag5)
							{
								replaceIndex = i;
								replaceLevel = (int)dataList[i].Item1;
							}
						}
						ValueTuple<sbyte, int, int> entry = dataList[replaceIndex];
						entry.Item1 = level;
						entry.Item2 = (entry.Item3 = keepFrames);
						dataList.RemoveAt(0);
						dataList.Add(entry);
						if (isFlaw)
						{
							this.SetFlawCollection(dataDict, context);
						}
						else
						{
							this.SetAcupointCollection(dataDict, context);
						}
					}
					this._combatDomain.UpdateBodyDefeatMark(context, this, bodyPart);
					bool flag6 = !isFlaw && addNew;
					if (flag6)
					{
						bool flag7 = dataList.Count >= 3;
						if (flag7)
						{
							this._combatDomain.UpdateSkillNeedBodyPartCanUse(context, this);
						}
					}
				}
			}
		}

		// Token: 0x06005FD7 RID: 24535 RVA: 0x00368500 File Offset: 0x00366700
		public int UpgradeRandomFlawOrAcupoint(DataContext context, bool isFlaw, int count = 1, sbyte bodyPart = -1)
		{
			bool flag = isFlaw ? this.GetFlawImmunity() : this.GetAcupointImmunity();
			int result;
			if (flag)
			{
				DomainManager.Combat.ShowImmunityEffectTips(context, this._id, isFlaw ? EMarkType.Flaw : EMarkType.Acupoint);
				result = 0;
			}
			else
			{
				int[] keepTimeArray = isFlaw ? GlobalConfig.Instance.FlawBaseKeepTime : GlobalConfig.Instance.AcupointBaseKeepTime;
				int maxLevel = keepTimeArray.Length - 1;
				FlawOrAcupointCollection collection = isFlaw ? this._flawCollection : this._acupointCollection;
				List<sbyte> bodyPartRandomPool = ObjectPool<List<sbyte>>.Instance.Get();
				List<int> indexRandomPool = ObjectPool<List<int>>.Instance.Get();
				HashSet<sbyte> updateMarkSet = ObjectPool<HashSet<sbyte>>.Instance.Get();
				bodyPartRandomPool.Clear();
				indexRandomPool.Clear();
				updateMarkSet.Clear();
				CombatCharacter.GenerateFlawOrAcupointRandomPool(bodyPart, collection, bodyPartRandomPool, indexRandomPool, maxLevel, false);
				int maxAffectCount = Math.Min(bodyPartRandomPool.Count, count);
				for (int i = 0; i < maxAffectCount; i++)
				{
					int index = context.Random.Next(bodyPartRandomPool.Count);
					List<ValueTuple<sbyte, int, int>> dataList = collection.BodyPartDict[bodyPartRandomPool[index]];
					ValueTuple<sbyte, int, int> entry = dataList[indexRandomPool[index]];
					entry.Item1 = (sbyte)Math.Min((int)(entry.Item1 + 1), maxLevel);
					entry.Item3 = (entry.Item2 = keepTimeArray[(int)entry.Item1]);
					dataList[indexRandomPool[index]] = entry;
					updateMarkSet.Add(bodyPartRandomPool[index]);
					CollectionUtils.SwapAndRemove<sbyte>(bodyPartRandomPool, index);
					CollectionUtils.SwapAndRemove<int>(indexRandomPool, index);
				}
				bool flag2 = maxAffectCount > 0;
				if (flag2)
				{
					if (isFlaw)
					{
						this.SetFlawCollection(collection, context);
					}
					else
					{
						this.SetAcupointCollection(collection, context);
					}
				}
				foreach (sbyte markBodyPart in updateMarkSet)
				{
					this._combatDomain.UpdateBodyDefeatMark(context, this, markBodyPart);
				}
				ObjectPool<List<sbyte>>.Instance.Return(bodyPartRandomPool);
				ObjectPool<List<int>>.Instance.Return(indexRandomPool);
				ObjectPool<HashSet<sbyte>>.Instance.Return(updateMarkSet);
				result = maxAffectCount;
			}
			return result;
		}

		// Token: 0x06005FD8 RID: 24536 RVA: 0x00368738 File Offset: 0x00366938
		public void RemoveRandomFlawOrAcupoint(DataContext context, bool isFlaw, int count = 1)
		{
			FlawOrAcupointCollection dataDict = isFlaw ? this._flawCollection : this._acupointCollection;
			byte[] countList = isFlaw ? this._flawCount : this._acupointCount;
			List<sbyte> bodyPartRandomPool = ObjectPool<List<sbyte>>.Instance.Get();
			List<ValueTuple<sbyte, sbyte>> removedList = new List<ValueTuple<sbyte, sbyte>>();
			bodyPartRandomPool.Clear();
			for (sbyte part = 0; part < 7; part += 1)
			{
				for (int i = 0; i < (int)countList[(int)part]; i++)
				{
					bodyPartRandomPool.Add(part);
				}
			}
			int removeCount = Math.Min(count, bodyPartRandomPool.Count);
			for (int j = 0; j < removeCount; j++)
			{
				int index = context.Random.Next(0, bodyPartRandomPool.Count);
				sbyte part2 = bodyPartRandomPool[index];
				int collectionIndex = context.Random.Next(0, dataDict.BodyPartDict[part2].Count);
				bodyPartRandomPool.RemoveAt(index);
				removedList.Add(new ValueTuple<sbyte, sbyte>(part2, dataDict.BodyPartDict[part2][collectionIndex].Item1));
				byte[] array = countList;
				sbyte b = part2;
				array[(int)b] = array[(int)b] - 1;
				dataDict.BodyPartDict[part2].RemoveAt(collectionIndex);
			}
			if (isFlaw)
			{
				this.SetFlawCount(countList, context);
				this.SetFlawCollection(dataDict, context);
			}
			else
			{
				this.SetAcupointCount(countList, context);
				this.SetAcupointCollection(dataDict, context);
			}
			this._combatDomain.UpdateBodyDefeatMark(context, this);
			bool flag = this._combatDomain.IsMainCharacter(this);
			if (flag)
			{
				if (isFlaw)
				{
					this._combatDomain.UpdateAllTeammateCommandUsable(context, this.IsAlly, ETeammateCommandImplement.HealFlaw);
				}
				else
				{
					this._combatDomain.UpdateAllCommandAvailability(context, this);
				}
			}
			for (int k = 0; k < removedList.Count; k++)
			{
				ValueTuple<sbyte, sbyte> removedItem = removedList[k];
				if (isFlaw)
				{
					Events.RaiseFlawRemoved(context, this, removedItem.Item1, removedItem.Item2);
				}
				else
				{
					Events.RaiseAcuPointRemoved(context, this, removedItem.Item1, removedItem.Item2);
				}
			}
			ObjectPool<List<sbyte>>.Instance.Return(bodyPartRandomPool);
		}

		// Token: 0x06005FD9 RID: 24537 RVA: 0x00368964 File Offset: 0x00366B64
		private static void GenerateFlawOrAcupointRandomPool(sbyte bodyPart, FlawOrAcupointCollection collection, ICollection<sbyte> bodyPartRandomPool, ICollection<int> indexRandomPool, int maxLevel, bool onlyMaxLevel = false)
		{
			CombatCharacter.<>c__DisplayClass398_0 CS$<>8__locals1;
			CS$<>8__locals1.collection = collection;
			CS$<>8__locals1.onlyMaxLevel = onlyMaxLevel;
			CS$<>8__locals1.maxLevel = maxLevel;
			CS$<>8__locals1.bodyPartRandomPool = bodyPartRandomPool;
			CS$<>8__locals1.indexRandomPool = indexRandomPool;
			bool flag = bodyPart < 0;
			if (flag)
			{
				for (sbyte i = 0; i < 7; i += 1)
				{
					CombatCharacter.<GenerateFlawOrAcupointRandomPool>g__AddToRandom|398_0(i, ref CS$<>8__locals1);
				}
			}
			else
			{
				CombatCharacter.<GenerateFlawOrAcupointRandomPool>g__AddToRandom|398_0(bodyPart, ref CS$<>8__locals1);
			}
		}

		// Token: 0x06005FDA RID: 24538 RVA: 0x003689CC File Offset: 0x00366BCC
		public int GetMaxFlawCount()
		{
			int maxCount = 3 + DomainManager.SpecialEffect.GetModifyValue(this._id, 125, EDataModifyType.Add, -1, -1, -1, EDataSumType.All);
			return Math.Max(maxCount, 0);
		}

		// Token: 0x06005FDB RID: 24539 RVA: 0x00368A00 File Offset: 0x00366C00
		public int GetMaxAcupointCount()
		{
			int maxCount = 3 + DomainManager.SpecialEffect.GetModifyValue(this._id, 130, EDataModifyType.Add, -1, -1, -1, EDataSumType.All);
			return Math.Max(maxCount, 0);
		}

		// Token: 0x06005FDC RID: 24540 RVA: 0x00368A38 File Offset: 0x00366C38
		public short GetRecoveryOfFlaw()
		{
			int extraPercent = 0;
			bool flag = this._affectingDefendSkillId >= 0;
			if (flag)
			{
				GameData.Domains.CombatSkill.CombatSkill skill = DomainManager.CombatSkill.GetElement_CombatSkills(new ValueTuple<int, short>(this._id, this._affectingDefendSkillId));
				extraPercent += skill.GetPageEffects().Sum((SkillBreakPageEffectImplementItem pageEffect) => pageEffect.FlawRecoverSpeed);
			}
			return (short)DomainManager.SpecialEffect.ModifyValue(this._id, 185, (int)this._character.GetRecoveryOfFlaw(), -1, -1, -1, 0, extraPercent, 0, 0);
		}

		// Token: 0x06005FDD RID: 24541 RVA: 0x00368AD4 File Offset: 0x00366CD4
		public short GetRecoveryOfAcupoint()
		{
			int extraPercent = 0;
			bool flag = this._affectingDefendSkillId >= 0;
			if (flag)
			{
				GameData.Domains.CombatSkill.CombatSkill skill = DomainManager.CombatSkill.GetElement_CombatSkills(new ValueTuple<int, short>(this._id, this._affectingDefendSkillId));
				extraPercent += skill.GetPageEffects().Sum((SkillBreakPageEffectImplementItem pageEffect) => pageEffect.AcupointRecoverSpeed);
			}
			return (short)DomainManager.SpecialEffect.ModifyValue(this._id, 186, (int)this._character.GetRecoveryOfBlockedAcupoint(), -1, -1, -1, 0, extraPercent, 0, 0);
		}

		// Token: 0x06005FDE RID: 24542 RVA: 0x00368B70 File Offset: 0x00366D70
		public void ClearInjuryAutoHealProgress(DataContext context, bool inner)
		{
			InjuryAutoHealCollection autoHealCollection = this.GetInjuryAutoHealCollection();
			foreach (List<short> progressList in inner ? autoHealCollection.InnerBodyPartList : autoHealCollection.OuterBodyPartList)
			{
				for (int j = 0; j < progressList.Count; j++)
				{
					progressList[j] = 0;
				}
			}
			this.SetInjuryAutoHealCollection(autoHealCollection, context);
		}

		// Token: 0x06005FDF RID: 24543 RVA: 0x00368BE0 File Offset: 0x00366DE0
		public int GetDamageValue(sbyte bodyPart, bool inner)
		{
			return (inner ? this.GetInnerDamageValue() : this.GetOuterDamageValue())[(int)bodyPart];
		}

		// Token: 0x06005FE0 RID: 24544 RVA: 0x00368C08 File Offset: 0x00366E08
		public void SetDamageValue(DataContext context, int leftDamage, sbyte bodyPart, bool inner)
		{
			int[] valueArray = inner ? this.GetInnerDamageValue() : this.GetOuterDamageValue();
			valueArray[(int)bodyPart] = leftDamage;
			if (inner)
			{
				this.SetInnerDamageValue(valueArray, context);
			}
			else
			{
				this.SetOuterDamageValue(valueArray, context);
			}
		}

		// Token: 0x06005FE1 RID: 24545 RVA: 0x00368C48 File Offset: 0x00366E48
		public void AddDamageToShow(DataContext context, int damage, int criticalPercent, sbyte bodyPart, bool inner)
		{
			IntPair value = inner ? this._innerDamageValueToShow[(int)bodyPart] : this._outerDamageValueToShow[(int)bodyPart];
			value.First = Math.Max(value.First, 0) + damage;
			value.Second = Math.Max(value.Second, criticalPercent);
			if (inner)
			{
				this._innerDamageValueToShow[(int)bodyPart] = value;
				this.SetInnerDamageValueToShow(this._innerDamageValueToShow, context);
			}
			else
			{
				this._outerDamageValueToShow[(int)bodyPart] = value;
				this.SetOuterDamageValueToShow(this._outerDamageValueToShow, context);
			}
		}

		// Token: 0x06005FE2 RID: 24546 RVA: 0x00368CE4 File Offset: 0x00366EE4
		public void AddMindDamageToShow(DataContext context, int mindDamage)
		{
			int value = Math.Max(this._mindDamageValueToShow, 0) + mindDamage;
			this.SetMindDamageValueToShow(value, context);
		}

		// Token: 0x06005FE3 RID: 24547 RVA: 0x00368D0C File Offset: 0x00366F0C
		public void AddFatalDamageToShow(DataContext context, int fatalDamage)
		{
			int value = Math.Max(this._fatalDamageValueToShow, 0) + fatalDamage;
			this.SetFatalDamageValueToShow(value, context);
		}

		// Token: 0x06005FE4 RID: 24548 RVA: 0x00368D34 File Offset: 0x00366F34
		public CombatStateCollection GetCombatStateCollection(sbyte type)
		{
			if (!true)
			{
			}
			CombatStateCollection result;
			switch (type)
			{
			case 0:
				result = this._specialCombatStateCollection;
				break;
			case 1:
				result = this._buffCombatStateCollection;
				break;
			case 2:
				result = this._debuffCombatStateCollection;
				break;
			default:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(27, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Invalid combat state type: ");
				defaultInterpolatedStringHandler.AppendFormatted<sbyte>(type);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			}
			if (!true)
			{
			}
			return result;
		}

		// Token: 0x06005FE5 RID: 24549 RVA: 0x00368DAC File Offset: 0x00366FAC
		public void SetCombatStateCollection(sbyte type, CombatStateCollection stateCollection, DataContext context)
		{
			switch (type)
			{
			case 0:
				this.SetSpecialCombatStateCollection(stateCollection, context);
				break;
			case 1:
				this.SetBuffCombatStateCollection(stateCollection, context);
				break;
			case 2:
				this.SetDebuffCombatStateCollection(stateCollection, context);
				break;
			}
		}

		// Token: 0x06005FE6 RID: 24550 RVA: 0x00368DF4 File Offset: 0x00366FF4
		public int GetCombatStatePower(sbyte stateType, short stateId)
		{
			CombatStateCollection stateCollection = this.GetCombatStateCollection(stateType);
			ValueTuple<short, bool, int> tuple;
			return (int)(stateCollection.StateDict.TryGetValue(stateId, out tuple) ? tuple.Item1 : 0);
		}

		// Token: 0x06005FE7 RID: 24551 RVA: 0x00368E28 File Offset: 0x00367028
		public short GetCombatStatePowerLimit(sbyte type)
		{
			short result;
			switch (type)
			{
			case 0:
				result = 500;
				break;
			case 1:
				result = (short)Math.Max((int)(500 + this.BuffCombatStatePowerExtraLimit), 0);
				break;
			case 2:
				result = (short)Math.Max((int)(500 + this.DebuffCombatStatePowerExtraLimit), 0);
				break;
			default:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(27, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Invalid combat state type: ");
				defaultInterpolatedStringHandler.AppendFormatted<sbyte>(type);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			}
			return result;
		}

		// Token: 0x06005FE8 RID: 24552 RVA: 0x00368EB0 File Offset: 0x003670B0
		public unsafe int GetFightBackPower(sbyte hitType)
		{
			GameData.Domains.CombatSkill.CombatSkill skill = (this._affectingDefendSkillId >= 0) ? DomainManager.CombatSkill.GetElement_CombatSkills(new CombatSkillKey(this._id, this._affectingDefendSkillId)) : null;
			HitOrAvoidInts addAvoid = (skill != null) ? skill.GetAddAvoidValueOnCast() : default(HitOrAvoidInts);
			int fightBackPower = (skill != null && *(ref addAvoid.Items.FixedElementField + (IntPtr)hitType * 4) > 0) ? skill.GetFightBackPower() : 0;
			return DomainManager.SpecialEffect.ModifyValue(this._id, 112, fightBackPower, (int)hitType, -1, -1, 0, 0, 0, 0);
		}

		// Token: 0x06005FE9 RID: 24553 RVA: 0x00368F40 File Offset: 0x00367140
		public OuterAndInnerInts GetBouncePower(sbyte attackInnerRatio = 50)
		{
			OuterAndInnerInts bouncePower = (this._affectingDefendSkillId > 0) ? DomainManager.CombatSkill.GetElement_CombatSkills(new CombatSkillKey(this._id, this._affectingDefendSkillId)).GetBouncePower() : new OuterAndInnerInts(0, 0);
			bouncePower.Outer = DomainManager.SpecialEffect.ModifyValue(this._id, 111, bouncePower.Outer, 0, (int)attackInnerRatio, -1, 0, 0, 0, 0);
			bouncePower.Inner = DomainManager.SpecialEffect.ModifyValue(this._id, 111, bouncePower.Inner, 1, (int)attackInnerRatio, -1, 0, 0, 0, 0);
			return bouncePower;
		}

		// Token: 0x06005FEA RID: 24554 RVA: 0x00368FD4 File Offset: 0x003671D4
		public unsafe bool PoisonOverflow(sbyte poisonType)
		{
			return PoisonsAndLevels.CalcPoisonedLevel(*(ref this._poison.Items.FixedElementField + (IntPtr)poisonType * 4)) > 0;
		}

		// Token: 0x06005FEB RID: 24555 RVA: 0x00369004 File Offset: 0x00367204
		public void AddPoisonAffectValue(sbyte poisonType, short value, bool needLessThanThreshold = false)
		{
			bool flag = this.GetCharacter().HasPoisonImmunity(poisonType);
			if (!flag)
			{
				short[] poisonAffectAccumulator = this._poisonAffectAccumulator;
				poisonAffectAccumulator[(int)poisonType] = poisonAffectAccumulator[(int)poisonType] + value;
				short threshold = Poison.Instance[poisonType].AffectNeedValue;
				threshold += (short)DomainManager.SpecialEffect.GetModifyValue(this._id, 243, EDataModifyType.Add, (int)poisonType, -1, -1, EDataSumType.All);
				threshold = Math.Max(1, threshold);
				if (needLessThanThreshold)
				{
					this._poisonAffectAccumulator[(int)poisonType] = (short)Math.Min((int)this._poisonAffectAccumulator[(int)poisonType], (int)(threshold - 1));
				}
				while (this._poisonAffectAccumulator[(int)poisonType] >= threshold)
				{
					short[] poisonAffectAccumulator2 = this._poisonAffectAccumulator;
					poisonAffectAccumulator2[(int)poisonType] = poisonAffectAccumulator2[(int)poisonType] - threshold;
					this._combatDomain.PoisonAffect(this.GetDataContext(), this, poisonType);
				}
			}
		}

		// Token: 0x06005FEC RID: 24556 RVA: 0x003690CC File Offset: 0x003672CC
		private void OnDefeatMarkChanged(DataContext context, DataUid dataUid)
		{
			bool flag = !this.IsAlly;
			if (flag)
			{
				this._combatDomain.UpdateShowUseSpecialMisc(context);
			}
		}

		// Token: 0x06005FED RID: 24557 RVA: 0x003690F4 File Offset: 0x003672F4
		public int CalcAccessoryReducePoisonResist(sbyte poisonType, sbyte poisonLevel)
		{
			int reducePoisonAssist = 0;
			CValuePercent percent = GlobalConfig.Instance.AccessoryReducePoisonPercent;
			ItemKey[] equipment = this._character.GetEquipment();
			for (int slot = 8; slot <= 10; slot++)
			{
				PoisonsAndLevels attachedPoisons;
				bool flag = !DomainManager.Combat.CheckEquipmentPoison(equipment[slot], out attachedPoisons);
				if (!flag)
				{
					ValueTuple<short, sbyte> valueAndLevel = attachedPoisons.GetValueAndLevel(poisonType);
					short value = valueAndLevel.Item1;
					sbyte level = valueAndLevel.Item2;
					bool flag2 = level <= poisonLevel;
					if (!flag2)
					{
						reducePoisonAssist -= (int)value * percent;
					}
				}
			}
			return reducePoisonAssist;
		}

		// Token: 0x06005FEE RID: 24558 RVA: 0x0036918F File Offset: 0x0036738F
		private void OnPoisonResistChanged(DataContext context, DataUid dataUid)
		{
			this.SetPoisonResist(this._character.GetPoisonResists(), context);
			this._combatDomain.UpdatePoisonDefeatMark(context, this);
		}

		// Token: 0x06005FEF RID: 24559 RVA: 0x003691B4 File Offset: 0x003673B4
		public int GetFeatureMedalValue(sbyte medalType)
		{
			return this._character.GetFeatureMedalValue(medalType);
		}

		// Token: 0x06005FF0 RID: 24560 RVA: 0x003691D4 File Offset: 0x003673D4
		[return: TupleElementNames(new string[]
		{
			"add",
			"reduce"
		})]
		public ValueTuple<int, int> GetFeatureSilenceFrameTotalPercent()
		{
			int add = 0;
			int reduce = 0;
			foreach (short featureId in this._character.GetFeatureIds())
			{
				CharacterFeatureItem config = CharacterFeature.Instance[featureId];
				add = Math.Max(add, config.SilenceFramePercent);
				reduce = Math.Min(reduce, config.SilenceFramePercent);
			}
			return new ValueTuple<int, int>(add, reduce);
		}

		// Token: 0x06005FF1 RID: 24561 RVA: 0x00369264 File Offset: 0x00367464
		public bool HasInfectedFeature(ECharacterFeatureInfectedType type)
		{
			if (!true)
			{
			}
			ECharacterFeatureInfectedType type2 = type;
			bool result;
			if (type2 != ECharacterFeatureInfectedType.NotInfected)
			{
				result = (type2 - ECharacterFeatureInfectedType.PartlyInfected <= 1 && this._character.GetFeatureIds().Any((short x) => CharacterFeature.Instance[x].InfectedType == type));
			}
			else
			{
				result = (!this.HasInfectedFeature(ECharacterFeatureInfectedType.PartlyInfected) && !this.HasInfectedFeature(ECharacterFeatureInfectedType.CompletelyInfected));
			}
			if (!true)
			{
			}
			return result;
		}

		// Token: 0x06005FF2 RID: 24562 RVA: 0x003692DC File Offset: 0x003674DC
		public unsafe sbyte GetPersonalityValue(sbyte type)
		{
			return *(ref this._character.GetPersonalities().Items.FixedElementField + type);
		}

		// Token: 0x06005FF3 RID: 24563 RVA: 0x0036930C File Offset: 0x0036750C
		public bool ChangeToEmptyHand(DataContext context)
		{
			bool flag = !Config.Character.Instance[this._character.GetTemplateId()].AllowUseFreeWeapon;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				DomainManager.Combat.ChangeWeapon(context, this, 3, false, true);
				bool flag2 = this.StateMachine.GetCurrentStateType() == CombatCharacterStateType.PrepareSkill && this._preparingSkillId >= 0 && !DomainManager.Combat.WeaponHasNeedTrick(this, this._preparingSkillId, DomainManager.Combat.GetUsingWeaponData(this));
				if (flag2)
				{
					DomainManager.Combat.InterruptSkill(context, this, -1);
				}
				result = true;
			}
			return result;
		}

		// Token: 0x06005FF4 RID: 24564 RVA: 0x003693A0 File Offset: 0x003675A0
		public bool ChangeToEmptyHandOrOther(DataContext context)
		{
			bool flag = this.ChangeToEmptyHand(context);
			bool result;
			if (flag)
			{
				result = true;
			}
			else
			{
				for (int i = 0; i < 3; i++)
				{
					bool flag2 = i == this._usingWeaponIndex || !this._weapons[i].IsValid();
					if (!flag2)
					{
						CombatWeaponData weaponData = DomainManager.Combat.GetElement_WeaponDataDict(this._weapons[i].Id);
						bool flag3 = !weaponData.GetCanChangeTo();
						if (!flag3)
						{
							DomainManager.Combat.ChangeWeapon(context, this, i, false, true);
							return true;
						}
					}
				}
				result = false;
			}
			return result;
		}

		// Token: 0x06005FF5 RID: 24565 RVA: 0x00369441 File Offset: 0x00367641
		public bool CanUnlockAttackByConfig(int index)
		{
			return this.GetUnlockEffect(index) != null;
		}

		// Token: 0x06005FF6 RID: 24566 RVA: 0x00369450 File Offset: 0x00367650
		public WeaponUnlockEffectItem GetUnlockEffect(int index)
		{
			bool flag = !this._weapons.CheckIndex(index);
			WeaponUnlockEffectItem result;
			if (flag)
			{
				result = null;
			}
			else
			{
				ItemKey weaponKey = this._weapons[index];
				bool flag2 = !weaponKey.IsValid();
				if (flag2)
				{
					result = null;
				}
				else
				{
					short weaponId = weaponKey.TemplateId;
					WeaponItem weaponConfig = Config.Weapon.Instance[weaponId];
					result = WeaponUnlockEffect.Instance[weaponConfig.UnlockEffect];
				}
			}
			return result;
		}

		// Token: 0x06005FF7 RID: 24567 RVA: 0x003694C4 File Offset: 0x003676C4
		public CombatWeaponData GetWeaponData(int index = -1)
		{
			index = ((index < 0) ? this._usingWeaponIndex : index);
			ItemKey key = this._weapons[index];
			return this._combatDomain.GetElement_WeaponDataDict(key.Id);
		}

		// Token: 0x06005FF8 RID: 24568 RVA: 0x00369504 File Offset: 0x00367704
		public sbyte GetConfigAttackPointCost()
		{
			GameData.Domains.Item.Weapon weapon = DomainManager.Combat.GetUsingWeapon(this);
			return weapon.GetAttackPreparePointCost();
		}

		// Token: 0x06005FF9 RID: 24569 RVA: 0x00369528 File Offset: 0x00367728
		public void ClearUnlockAttackValue(DataContext context, int index)
		{
			bool flag = !this._unlockPrepareValue.CheckIndex(index);
			if (!flag)
			{
				bool flag2 = this._unlockPrepareValue[index] <= 0;
				if (!flag2)
				{
					this._unlockPrepareValue[index] = 0;
					this.SetUnlockPrepareValue(this._unlockPrepareValue, context);
				}
			}
		}

		// Token: 0x06005FFA RID: 24570 RVA: 0x00369580 File Offset: 0x00367780
		public void ChangeUnlockAttackValue(DataContext context, int index, int delta)
		{
			bool flag = !this._unlockPrepareValue.CheckIndex(index);
			if (!flag)
			{
				bool flag2 = !this.CanUnlockAttackByConfig(index);
				if (!flag2)
				{
					bool flag3 = this.GetWeaponData(index).GetDurability() <= 0;
					if (!flag3)
					{
						bool flag4 = delta > 0;
						if (flag4)
						{
							delta = DomainManager.SpecialEffect.ModifyValue(this._id, 317, delta, -1, -1, -1, 0, 0, 0, 0);
							delta = Math.Max(delta, 1);
						}
						int newValue = Math.Clamp(this._unlockPrepareValue[index] + delta, 0, GlobalConfig.Instance.UnlockAttackUnit);
						bool flag5 = newValue == this._unlockPrepareValue[index];
						if (!flag5)
						{
							this._unlockPrepareValue[index] = newValue;
							this.SetUnlockPrepareValue(this._unlockPrepareValue, context);
						}
					}
				}
			}
		}

		// Token: 0x06005FFB RID: 24571 RVA: 0x0036965C File Offset: 0x0036785C
		public void ChangeAllUnlockAttackValue(DataContext context, int delta)
		{
			for (int i = 0; i < 3; i++)
			{
				this.ChangeUnlockAttackValue(context, i, delta);
			}
		}

		// Token: 0x06005FFC RID: 24572 RVA: 0x00369684 File Offset: 0x00367884
		public void ChangeAllUnlockAttackValue(DataContext context, CValuePercent deltaPercent)
		{
			this.ChangeAllUnlockAttackValue(context, GlobalConfig.Instance.UnlockAttackUnit * deltaPercent);
		}

		// Token: 0x06005FFD RID: 24573 RVA: 0x003696A0 File Offset: 0x003678A0
		public bool LegSkillUseShoes()
		{
			return DomainManager.SpecialEffect.ModifyData(this._id, -1, 88, true, -1, -1, -1);
		}

		// Token: 0x06005FFE RID: 24574 RVA: 0x003696CC File Offset: 0x003678CC
		public bool HasDoingOrReserveCommand()
		{
			bool hasCmd = this.StateMachine.GetCurrentStateType() != CombatCharacterStateType.Idle || this._combatReserveData.AnyReserve || this.NeedNormalAttack || this.NeedChangeTrickAttack || this.NeedUnlockAttack || this.NeedBreakAttack || this.NeedUseSkillFreeId >= 0 || this.ChangeCharId >= 0;
			bool flag = !hasCmd && this._combatDomain.IsMainCharacter(this);
			if (flag)
			{
				int[] charList = this.IsAlly ? this._combatDomain.GetSelfTeam() : this._combatDomain.GetEnemyTeam();
				for (int i = 0; i < this.TeammateHasCommand.Length; i++)
				{
					bool flag2 = !this.TeammateHasCommand[i];
					if (!flag2)
					{
						bool intoCombatField = this._combatDomain.GetElement_CombatCharacterDict(charList[i + 1]).ExecutingTeammateCommandConfig.IntoCombatField;
						if (intoCombatField)
						{
							hasCmd = true;
							break;
						}
					}
				}
			}
			return hasCmd;
		}

		// Token: 0x06005FFF RID: 24575 RVA: 0x003697C8 File Offset: 0x003679C8
		public void ClearAllDoingOrReserveCommand(DataContext context)
		{
			this.NeedChangeTrickAttack = (this.NeedUnlockAttack = (this.NeedBreakAttack = false));
			this.SetPreparingSkillId(-1, context);
			this.SetAffectingMoveSkillId(-1, context);
			this.SetAffectingDefendSkillId(-1, context);
			this.SetPreparingItem(ItemKey.Invalid, context);
			this.SetPreparingOtherAction(-1, context);
			this.SetCombatReserveData(CombatReserveData.Invalid, context);
			this.SetReserveNormalAttack(false, context);
			this.NeedNormalAttackImmediate = false;
			this.KeepMoving = false;
			this.MoveData.ResetJumpState(context, false);
			this.SetAnimationToLoop(null, context);
			this.SetParticleToLoop(null, context);
		}

		// Token: 0x06006000 RID: 24576 RVA: 0x00369864 File Offset: 0x00367A64
		public void SetNeedUseSkillId(DataContext context, short needUseSkillId)
		{
			this.SetCombatReserveData(CombatReserveData.CreateSkill(needUseSkillId), context);
		}

		// Token: 0x06006001 RID: 24577 RVA: 0x00369875 File Offset: 0x00367A75
		public void SetNeedShowChangeTrick(DataContext context, bool needShowChangeTrick)
		{
			this.SetCombatReserveData(CombatReserveData.CreateChangeTrick(needShowChangeTrick), context);
		}

		// Token: 0x06006002 RID: 24578 RVA: 0x00369886 File Offset: 0x00367A86
		public void SetNeedChangeWeaponIndex(DataContext context, int needChangeWeaponIndex)
		{
			this.SetCombatReserveData(CombatReserveData.CreateChangeWeapon(needChangeWeaponIndex), context);
		}

		// Token: 0x06006003 RID: 24579 RVA: 0x00369897 File Offset: 0x00367A97
		public void SetNeedUnlockWeaponIndex(DataContext context, int needUnlockWeaponIndex)
		{
			this.SetCombatReserveData(CombatReserveData.CreateUnlockAttack(needUnlockWeaponIndex), context);
		}

		// Token: 0x06006004 RID: 24580 RVA: 0x003698A8 File Offset: 0x00367AA8
		public void SetNeedUseOtherAction(DataContext context, sbyte needUseOtherAction)
		{
			this.SetCombatReserveData(CombatReserveData.CreateOtherAction(needUseOtherAction), context);
		}

		// Token: 0x06006005 RID: 24581 RVA: 0x003698B9 File Offset: 0x00367AB9
		public void SetNeedUseItem(DataContext context, ItemKey needUseItem)
		{
			this.SetCombatReserveData(CombatReserveData.CreateUseItem(needUseItem), context);
		}

		// Token: 0x06006006 RID: 24582 RVA: 0x003698CA File Offset: 0x00367ACA
		public void SetNeedTeammateCommand(DataContext context, int teammateId, int index)
		{
			this.SetCombatReserveData(CombatReserveData.CreateTeammateCommand(teammateId, index), context);
		}

		// Token: 0x06006007 RID: 24583 RVA: 0x003698DC File Offset: 0x00367ADC
		public void NormalAttackFree()
		{
			this.NeedFreeAttack = true;
		}

		// Token: 0x06006008 RID: 24584 RVA: 0x003698E5 File Offset: 0x00367AE5
		public void FinishFreeAttack()
		{
			this.IsAutoNormalAttacking = false;
		}

		// Token: 0x06006009 RID: 24585 RVA: 0x003698F0 File Offset: 0x00367AF0
		public bool GetOuterInjuryImmunity()
		{
			return this._character.GetOuterInjuryImmunity() || this.OuterInjuryImmunity;
		}

		// Token: 0x0600600A RID: 24586 RVA: 0x00369918 File Offset: 0x00367B18
		public bool GetInnerInjuryImmunity()
		{
			return this._character.GetInnerInjuryImmunity() || this.InnerInjuryImmunity;
		}

		// Token: 0x0600600B RID: 24587 RVA: 0x00369940 File Offset: 0x00367B40
		public bool GetMindImmunity()
		{
			return this._character.GetMindImmunity() || this.MindImmunity;
		}

		// Token: 0x0600600C RID: 24588 RVA: 0x00369968 File Offset: 0x00367B68
		public bool GetFlawImmunity()
		{
			return this._character.GetFlawImmunity() || this.FlawImmunity;
		}

		// Token: 0x0600600D RID: 24589 RVA: 0x00369990 File Offset: 0x00367B90
		public bool GetAcupointImmunity()
		{
			return this._character.GetAcupointImmunity() || this.AcupointImmunity;
		}

		// Token: 0x0600600E RID: 24590 RVA: 0x003699B8 File Offset: 0x00367BB8
		public void ClearAllSound(DataContext context)
		{
			this.SetAttackSoundToPlay(null, context);
			this.SetSkillSoundToPlay(null, context);
			this.SetHitSoundToPlay(null, context);
			this.SetArmorHitSoundToPlay(null, context);
			this.SetWhooshSoundToPlay(null, context);
			this.SetShockSoundToPlay(null, context);
			this.SetStepSoundToPlay(null, context);
			this.SetDieSoundToPlay(null, context);
		}

		// Token: 0x0600600F RID: 24591 RVA: 0x00369A10 File Offset: 0x00367C10
		public void InitTeammateCommand(DataContext context, bool isFirstMove)
		{
			bool flag = !DomainManager.Combat.GetTeamCharacterIds().Contains(this._id);
			if (!flag)
			{
				IReadOnlyList<sbyte> cmdList = DomainManager.Combat.GetPreRandomizedTeammateCommands(context, this._id);
				short favor = DomainManager.Character.GetFavorability(this._id, this._combatDomain.GetMainCharacter(this.IsAlly).GetId());
				sbyte favorType = FavorabilityType.GetFavorabilityType(favor);
				byte initPercentValue = isFirstMove ? 75 : 50;
				CValuePercent initPercent = (int)initPercentValue;
				for (int i = 0; i < 3; i++)
				{
					sbyte cmdType = (cmdList != null && i < cmdList.Count) ? cmdList[i] : -1;
					this._currTeammateCommands.Add(cmdType);
					this._teammateCommandBanReasons.Add(SByteList.Create());
					this._teammateCommandCanUse.Add(false);
					this._teammateCommandCdPercent.Add(initPercentValue);
					this.TeammateCommandCdTotalCount[i] = ((cmdType >= 0) ? TeammateCommand.Instance[cmdType].CdCount : -1);
					this.TeammateCommandCdCurrentCount[i] = this.TeammateCommandCdTotalCount[i] * initPercent;
				}
				this.TeammateCommandCdSpeed = ((favorType >= 5) ? 100 : ((favorType >= 3) ? 85 : ((favorType >= 1) ? 70 : ((favorType >= 0) ? 55 : ((favorType >= -2) ? 40 : ((favorType >= -4) ? 25 : 10))))));
				this.StopCommandEffectCount = 0;
				this.UpdateTeammateCommandOnPrepared(context);
			}
		}

		// Token: 0x06006010 RID: 24592 RVA: 0x00369B8C File Offset: 0x00367D8C
		public int GetTeammateCommandCdSpeed(sbyte cmdType)
		{
			bool showTransferInjuryCommand = this._showTransferInjuryCommand;
			if (showTransferInjuryCommand)
			{
				cmdType = 13;
			}
			ETeammateCommandImplement implement = TeammateCommand.Instance[cmdType].Implement;
			int percent = DomainManager.SpecialEffect.GetModifyValue(this._combatDomain.GetMainCharacter(this.IsAlly).GetId(), 183, EDataModifyType.AddPercent, (int)implement, -1, -1, EDataSumType.All);
			return this.TeammateCommandCdSpeed * 100 / (100 + percent);
		}

		// Token: 0x06006011 RID: 24593 RVA: 0x00369BF8 File Offset: 0x00367DF8
		public void ClearTeammateCommandCd(DataContext context, int index)
		{
			List<sbyte> cmdList = this.GetCurrTeammateCommands();
			bool flag = !cmdList.CheckIndex(index) || cmdList[index] < 0;
			if (!flag)
			{
				CombatCharacter mainChar = DomainManager.Combat.GetMainCharacter(this.IsAlly);
				bool flag2 = this.TeammateCommandCdCurrentCount[index] >= this.TeammateCommandCdTotalCount[index] || mainChar.TeammateBeforeMainChar == this._id || mainChar.TeammateAfterMainChar == this._id;
				if (!flag2)
				{
					this.TeammateCommandCdCurrentCount[index] = this.TeammateCommandCdTotalCount[index];
					List<byte> cdPercent = this.GetTeammateCommandCdPercent();
					bool flag3 = cdPercent[index] > 0;
					if (flag3)
					{
						cdPercent[index] = 0;
						this.SetTeammateCommandCdPercent(cdPercent, context);
					}
					DomainManager.Combat.UpdateTeammateCommandUsable(context, this, cmdList[index]);
				}
			}
		}

		// Token: 0x06006012 RID: 24594 RVA: 0x00369CC8 File Offset: 0x00367EC8
		public void ResetTeammateCommandCd(DataContext context, int index, int cdCount = -1, bool checkEvent = false, bool displayEvent = false)
		{
			sbyte cmdType = this._currTeammateCommands[index];
			bool flag = cdCount < 0;
			if (flag)
			{
				cdCount = TeammateCommand.Instance[cmdType].CdCount;
			}
			CombatCharacter mainChar = DomainManager.Combat.GetMainCharacter(this.IsAlly);
			bool flag2 = mainChar.GetCharacter().IsTreasuryGuard();
			if (flag2)
			{
				cdCount *= GlobalConfig.Instance.TreasuryGuardTeammateCdBonus;
			}
			this.TeammateCommandCdTotalCount[index] = cdCount;
			bool skipCd = checkEvent && this.CheckInvokeSkipCd(context.Random, cmdType);
			bool flag3 = skipCd;
			if (flag3)
			{
				DomainManager.Combat.ShowTeammateCommand(this._id, index, displayEvent);
			}
			this.TeammateCommandCdCurrentCount[index] = (skipCd ? this.TeammateCommandCdTotalCount[index] : 0);
			this._teammateCommandCdPercent[index] = (skipCd ? 0 : 100);
			this.SetTeammateCommandCdPercent(this._teammateCommandCdPercent, context);
			this._combatDomain.UpdateTeammateCommandUsable(context, this, cmdType);
		}

		// Token: 0x06006013 RID: 24595 RVA: 0x00369DB8 File Offset: 0x00367FB8
		private bool CheckInvokeSkipCd(IRandomSource random, sbyte cmdType)
		{
			TeammateCommandItem config = TeammateCommand.Instance[cmdType];
			bool flag = config.Type == ETeammateCommandType.Negative;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				bool flag2 = !this._character.Template.AllowFavorabilitySkipCd;
				if (flag2)
				{
					result = false;
				}
				else
				{
					CombatCharacter mainChar = DomainManager.Combat.GetMainCharacter(this.IsAlly);
					short favor = DomainManager.Character.GetFavorability(this._id, mainChar.GetId());
					bool flag3 = favor < config.FavorLimit[0] || favor > config.FavorLimit[1];
					result = (!flag3 && random.CheckPercentProb(config.AutoProb));
				}
			}
			return result;
		}

		// Token: 0x06006014 RID: 24596 RVA: 0x00369E60 File Offset: 0x00368060
		public bool UpdateTeammateCommandState(DataContext context)
		{
			bool intoCombatField = this.ExecutingTeammateCommandConfig.IntoCombatField;
			if (intoCombatField)
			{
				bool flag = !this._visible;
				if (flag)
				{
					sbyte displayDist = -1;
					bool flag2 = this.ExecutingTeammateCommandImplement.IsAttack() && this._combatDomain.InAttackRange(this);
					if (flag2)
					{
						displayDist = this.GetNormalAttackPosition(this._attackCommandTrickType);
					}
					int displayPos = (displayDist > 0) ? this._combatDomain.GetDisplayPosition(this.IsAlly, (short)displayDist) : int.MinValue;
					this._combatDomain.SetDisplayPosition(context, this.IsAlly, displayPos);
					this.SetAnimationToLoop(this._combatDomain.GetIdleAni(this), context);
					this.SetVisible(true, context);
					this.SetTeammateCommandPreparePercent(0, context);
					bool flag3 = this.TeammateCommandLeftPrepareFrame <= 0;
					if (flag3)
					{
						this.ResetTeammateCommandLeftTime(context);
					}
					return true;
				}
			}
			else
			{
				bool flag4 = this.TeammateCommandLeftFrame < 0;
				if (flag4)
				{
					this.ResetTeammateCommandLeftTime(context);
					bool flag5 = this.ExecutingTeammateCommandImplement == ETeammateCommandImplement.StopEnemy;
					if (flag5)
					{
						int[] enemyTeam = this.IsAlly ? this._combatDomain.GetEnemyTeam() : this._combatDomain.GetSelfTeam();
						for (int i = 1; i < enemyTeam.Length; i++)
						{
							bool flag6 = enemyTeam[i] >= 0;
							if (flag6)
							{
								this._combatDomain.GetElement_CombatCharacterDict(enemyTeam[i]).StopCommandEffectCount++;
							}
						}
						this._combatDomain.UpdateAllTeammateCommandUsable(context, !this.IsAlly, -1);
					}
					bool flag7 = this.ExecutingTeammateCommandImplement == ETeammateCommandImplement.AnimalEffect && this.ExecutingTeammateCommandSpecialEffect < 0L;
					if (flag7)
					{
						string effect = GameData.Domains.Combat.SharedConstValue.AnimalCarrier2Effect[this.AnimalConfig.CarrierId];
						int mainCharId = this._combatDomain.GetMainCharacter(this.IsAlly).GetId();
						this.ExecutingTeammateCommandSpecialEffect = DomainManager.SpecialEffect.Add(context, mainCharId, effect);
					}
					Type effectType;
					bool flag8 = CombatCharacter.TeammateCommandEffects.TryGetValue(this.ExecutingTeammateCommandImplement, out effectType) && this.ExecutingTeammateCommandSpecialEffect < 0L;
					if (flag8)
					{
						SpecialEffectBase effect2 = (SpecialEffectBase)Activator.CreateInstance(effectType, new object[]
						{
							this._id
						});
						this.ExecutingTeammateCommandSpecialEffect = DomainManager.SpecialEffect.Add(context, effect2);
					}
				}
			}
			bool flag9 = this.TeammateCommandLeftPrepareFrame > 0;
			bool result;
			if (flag9)
			{
				this.TeammateCommandLeftPrepareFrame -= 1;
				byte percent = (byte)((this.TeammateCommandLeftPrepareFrame == 0) ? 0 : ((this.TeammateCommandTotalPrepareFrame - this.TeammateCommandLeftPrepareFrame) * 100 / this.TeammateCommandTotalPrepareFrame));
				bool flag10 = this.GetTeammateCommandPreparePercent() != percent;
				if (flag10)
				{
					this.SetTeammateCommandPreparePercent(percent, context);
				}
				bool flag11 = this.TeammateCommandLeftPrepareFrame == 0;
				if (flag11)
				{
					this.ResetTeammateCommandLeftTime(context);
				}
				result = (this.TeammateCommandLeftPrepareFrame == 0);
			}
			else
			{
				bool flag12 = this.TeammateCommandLeftFrame > 0;
				if (flag12)
				{
					this.ReduceTeammateCommandLeftTime(context);
					bool flag13 = this.TeammateCommandLeftFrame == 0;
					if (flag13)
					{
						bool flag14 = this.ExecutingTeammateCommandImplement == ETeammateCommandImplement.StopEnemy;
						if (flag14)
						{
							int[] enemyTeam2 = this.IsAlly ? this._combatDomain.GetEnemyTeam() : this._combatDomain.GetSelfTeam();
							for (int j = 1; j < enemyTeam2.Length; j++)
							{
								bool flag15 = enemyTeam2[j] >= 0;
								if (flag15)
								{
									this._combatDomain.GetElement_CombatCharacterDict(enemyTeam2[j]).StopCommandEffectCount--;
								}
							}
							this._combatDomain.UpdateAllTeammateCommandUsable(context, !this.IsAlly, -1);
						}
						bool flag16 = this.ExecutingTeammateCommandSpecialEffect >= 0L;
						if (flag16)
						{
							DomainManager.SpecialEffect.Remove(context, this.ExecutingTeammateCommandSpecialEffect);
							this.ExecutingTeammateCommandSpecialEffect = -1L;
						}
						this.ClearTeammateCommand(context, false);
					}
					result = false;
				}
				else
				{
					bool flag17 = this._teammateExitAniLeftFrame > 0;
					if (flag17)
					{
						this._teammateExitAniLeftFrame -= 1;
					}
					bool flag18 = this._teammateExitAniLeftFrame <= 0;
					if (flag18)
					{
						this.ClearTeammateCommandData(context);
					}
					result = false;
				}
			}
			return result;
		}

		// Token: 0x06006015 RID: 24597 RVA: 0x0036A284 File Offset: 0x00368484
		public void ResetTeammateCommandLeftTime(DataContext context)
		{
			bool flag = this.ExecutingTeammateCommandConfig.AffectFrame > 0;
			if (flag)
			{
				short affectFrame = this.ExecutingTeammateCommandConfig.AffectFrame;
				ETeammateCommandImplement implement = this.ExecutingTeammateCommandConfig.Implement;
				bool flag2 = implement == ETeammateCommandImplement.Fight || implement == ETeammateCommandImplement.StopEnemy;
				bool flag3 = flag2;
				if (flag3)
				{
					int cmdEffectPercent = DomainManager.SpecialEffect.GetModifyValue(this._combatDomain.GetMainCharacter(this.IsAlly).GetId(), 184, EDataModifyType.Add, (int)this.ExecutingTeammateCommandConfig.Implement, -1, -1, EDataSumType.All);
					affectFrame = (short)((int)affectFrame * (100 + cmdEffectPercent) / 100);
				}
				this.TeammateCommandLeftFrame = (this.TeammateCommandTotalFrame = affectFrame);
				this.SetTeammateCommandTimePercent(100, context);
			}
		}

		// Token: 0x06006016 RID: 24598 RVA: 0x0036A33C File Offset: 0x0036853C
		public void ReduceTeammateCommandLeftTime(DataContext context)
		{
			this.TeammateCommandLeftFrame -= 1;
			byte percent = (byte)(this.TeammateCommandLeftFrame * 100 / this.TeammateCommandTotalFrame);
			bool flag = this.GetTeammateCommandTimePercent() != percent;
			if (flag)
			{
				this.SetTeammateCommandTimePercent(percent, context);
			}
		}

		// Token: 0x06006017 RID: 24599 RVA: 0x0036A384 File Offset: 0x00368584
		public void ClearTeammateCommand(DataContext context, bool interrupt = false)
		{
			bool flag = this._executingTeammateCommand < 0;
			if (!flag)
			{
				CombatCharacter mainChar = this._combatDomain.GetMainCharacter(this.IsAlly);
				this.PartlyClearTeammateCommand(context, interrupt);
				bool flag2 = this.ExecutingTeammateCommandConfig.IntoCombatField && (this.ExecutingTeammateCommandConfig.PrepareFrame > 0 || this.ExecutingTeammateCommandImplement.IsAttack() || this.ExecutingTeammateCommandImplement.IsDefend());
				if (flag2)
				{
					this.TeammateCommandLeftPrepareFrame = 0;
					this.TeammateCommandLeftFrame = 0;
					this._teammateExitAniLeftFrame = 1;
					this.SetTeammateCommandPreparePercent(0, context);
					if (interrupt)
					{
						bool flag3 = mainChar.TeammateBeforeMainChar < 0;
						if (flag3)
						{
							mainChar.SpecialAnimationLoop = null;
						}
						this._combatDomain.SetProperLoopAniAndParticle(context, mainChar, false);
						mainChar.SetTeammateCommandPreparePercent(0, context);
						bool flag4 = !string.IsNullOrEmpty(this._soundToLoop);
						if (flag4)
						{
							this.SetSoundToLoop(string.Empty, context);
						}
					}
				}
				else
				{
					bool flag5 = !this.ExecutingTeammateCommandConfig.IntoCombatField;
					if (flag5)
					{
						this.ResetTeammateCommandCd(context, this.ExecutingTeammateCommandIndex, -1, true, false);
					}
					this.ClearTeammateCommandData(context);
				}
			}
		}

		// Token: 0x06006018 RID: 24600 RVA: 0x0036A4A4 File Offset: 0x003686A4
		public void PartlyClearTeammateCommand(DataContext context, bool interrupt = false)
		{
			CombatCharacter mainChar = this._combatDomain.GetMainCharacter(this.IsAlly);
			bool flag = mainChar.TeammateBeforeMainChar == this._id;
			if (flag)
			{
				mainChar.TeammateBeforeMainChar = -1;
			}
			else
			{
				bool flag2 = mainChar.TeammateAfterMainChar == this._id;
				if (flag2)
				{
					mainChar.TeammateAfterMainChar = -1;
				}
			}
			mainChar.SetParticleToLoop(null, context);
			this.SetParticleToLoop(null, context);
			this.SetParticleToPlay(null, context);
			bool flag3 = this.ExecutingTeammateCommandImplement.IsAttack();
			if (flag3)
			{
				this._combatDomain.ClearDamageCompareData(context);
				this.UpdateAttackCommandWeaponAndTrick(context);
			}
			else
			{
				bool flag4 = this.ExecutingTeammateCommandImplement.IsDefend();
				if (flag4)
				{
					this.UpdateDefendCommandSkill(context);
				}
				else
				{
					bool flag5 = this.ExecutingTeammateCommandImplement == ETeammateCommandImplement.AttackSkill;
					if (flag5)
					{
						this.UpdateAttackCommandSkill(context);
					}
				}
			}
			bool flag6 = (this.ExecutingTeammateCommandConfig.IntoCombatField && this.ExecutingTeammateCommandConfig.PrepareFrame > 0) || this.ExecutingTeammateCommandImplement.IsAttack() || this.ExecutingTeammateCommandImplement.IsDefend();
			if (flag6)
			{
				short posOffset = this.ExecutingTeammateCommandConfig.PosOffset;
				string exitAni = (posOffset < 0 && string.IsNullOrEmpty(this.ExecutingTeammateCommandConfig.BackCharExitAni) && !interrupt) ? this.ExecutingTeammateCommandConfig.BackCharExitAni : "M_004";
				this._combatDomain.SetDisplayPosition(context, this.IsAlly, int.MinValue);
				this.SetAnimationToPlayOnce(exitAni, context);
				this.SetAnimationToLoop(this._combatDomain.GetIdleAni(this), context);
				this.SetDisplayPosition(int.MinValue, context);
				bool flag7 = this.ExecutingTeammateCommandImplement.IsDefend();
				if (flag7)
				{
					this._combatDomain.ClearAffectingDefenseSkill(context, this);
				}
			}
		}

		// Token: 0x06006019 RID: 24601 RVA: 0x0036A650 File Offset: 0x00368850
		private void ClearTeammateCommandData(DataContext context)
		{
			CombatCharacter mainChar = this._combatDomain.GetMainCharacter(this.IsAlly);
			mainChar.TeammateHasCommand[this._combatDomain.GetCharacterList(this.IsAlly).IndexOf(this._id) - 1] = false;
			this.SetExecutingTeammateCommand(-1, context);
			this.SetVisible(false, context);
			this.TeammateCommandLeftFrame = -1;
			this.ExecutingTeammateCommandIndex = -1;
			this._combatDomain.UpdateAllCommandAvailability(context, mainChar);
		}

		// Token: 0x0600601A RID: 24602 RVA: 0x0036A6C4 File Offset: 0x003688C4
		public OuterAndInnerShorts CalcAttackRangeImmediate(short skillId = -1, int weaponIndex = -1)
		{
			bool flag = !this._combatDomain.IsInCombat();
			OuterAndInnerShorts result;
			if (flag)
			{
				result = this._attackRange;
			}
			else
			{
				bool flag2 = skillId < 0 && weaponIndex < 0;
				if (flag2)
				{
					skillId = ((this._preparingSkillId > 0) ? this._preparingSkillId : ((this._performingSkillId > 0) ? this._performingSkillId : -1));
				}
				bool flag3 = weaponIndex < 0;
				if (flag3)
				{
					weaponIndex = this._usingWeaponIndex;
				}
				GameData.Domains.Item.Weapon weapon = DomainManager.Item.GetElement_Weapons(this._weapons[weaponIndex].Id);
				bool flag4 = skillId >= 0 && DomainManager.CombatSkill.GetSkillType(this._id, skillId) == 5;
				if (flag4)
				{
					weapon = DomainManager.Item.GetElement_Weapons(this._weapons[3].Id);
				}
				WeaponItem weaponConfig = Config.Weapon.Instance[weapon.GetTemplateId()];
				sbyte currTrick = this._changeTrickAttack ? this.ChangeTrickType : this._weaponTricks[(int)this._weaponTrickIndex];
				int rangeMid = (int)((weaponConfig.MinDistance + weaponConfig.MaxDistance) / 2);
				byte rangeMidMaxDelta = GlobalConfig.Instance.AttackRangeMidMinDistance;
				ValueTuple<int, int> weaponRange = DomainManager.Item.GetWeaponAttackRange(this._id, weapon.GetItemKey());
				short minDist = (short)weaponRange.Item1;
				short maxDist = (short)weaponRange.Item2;
				bool flag5 = skillId >= 0;
				if (flag5)
				{
					bool flag6 = weaponConfig.TrickDistanceAdjusts.Count > 0;
					if (flag6)
					{
						List<NeedTrick> skillTricks = ObjectPool<List<NeedTrick>>.Instance.Get();
						DomainManager.CombatSkill.GetCombatSkillCostTrick(DomainManager.CombatSkill.GetElement_CombatSkills(new CombatSkillKey(this._id, skillId)), skillTricks, true);
						short trickAddMinDist = 0;
						short trickAddMaxDist = 0;
						for (int i = 0; i < skillTricks.Count; i++)
						{
							sbyte skillTrick = skillTricks[i].TrickType;
							TrickDistanceAdjust trickAdjust = weaponConfig.TrickDistanceAdjusts.Find((TrickDistanceAdjust adjust) => adjust.TrickTemplateId == skillTrick);
							bool flag7 = trickAdjust != null;
							if (flag7)
							{
								trickAddMinDist = Math.Max(trickAddMinDist, trickAdjust.MinDistance);
								trickAddMaxDist = Math.Max(trickAddMaxDist, trickAdjust.MaxDistance);
							}
						}
						ObjectPool<List<NeedTrick>>.Instance.Return(skillTricks);
						minDist -= trickAddMinDist;
						maxDist += trickAddMaxDist;
					}
					minDist -= DomainManager.CombatSkill.GetCombatSkillAddAttackDistance(this._id, skillId, true);
					maxDist += DomainManager.CombatSkill.GetCombatSkillAddAttackDistance(this._id, skillId, false);
				}
				else
				{
					TrickDistanceAdjust trickAdjust2 = weaponConfig.TrickDistanceAdjusts.Find((TrickDistanceAdjust adjust) => adjust.TrickTemplateId == currTrick);
					bool flag8 = trickAdjust2 != null;
					if (flag8)
					{
						minDist -= trickAdjust2.MinDistance;
						maxDist += trickAdjust2.MaxDistance;
					}
				}
				minDist -= (short)DomainManager.SpecialEffect.GetModifyValue(this._id, -1, 145, EDataModifyType.Add, -1, -1, -1, EDataSumType.All);
				maxDist += (short)DomainManager.SpecialEffect.GetModifyValue(this._id, -1, 146, EDataModifyType.Add, -1, -1, -1, EDataSumType.All);
				int effectChangeDist = DomainManager.SpecialEffect.GetModifyValue(this._id, 273, EDataModifyType.Add, (int)minDist, (int)maxDist, -1, EDataSumType.All);
				int acupointChangeDist = this._acupointCollection.CalcAcupointParam(2);
				int changeDist = Math.Max(effectChangeDist, acupointChangeDist);
				minDist = (short)Math.Clamp((int)minDist + changeDist, 20, rangeMid - (int)rangeMidMaxDelta);
				maxDist = (short)Math.Clamp((int)maxDist - changeDist, rangeMid + (int)rangeMidMaxDelta, 120);
				result = new OuterAndInnerShorts(minDist, maxDist);
			}
			return result;
		}

		// Token: 0x0600601B RID: 24603 RVA: 0x0036AA3C File Offset: 0x00368C3C
		public void UpdateTeammateCommandOnPrepared(DataContext context)
		{
			List<sbyte> cmdList = this.GetCurrTeammateCommands();
			bool flag = cmdList.Exists((sbyte x) => x >= 0 && TeammateCommand.Instance[x].RequireTrick);
			if (flag)
			{
				this.UpdateAttackCommandWeaponAndTrick(context);
			}
			bool flag2 = cmdList.Exists((sbyte x) => x >= 0 && TeammateCommand.Instance[x].RequireAttackSkill);
			if (flag2)
			{
				this.UpdateAttackCommandSkill(context);
			}
			bool flag3 = cmdList.Exists((sbyte x) => x >= 0 && TeammateCommand.Instance[x].RequireDefendSkill);
			if (flag3)
			{
				this.UpdateDefendCommandSkill(context);
			}
			this.UpdateTeammateCommandInvokers();
		}

		// Token: 0x0600601C RID: 24604 RVA: 0x0036AAEC File Offset: 0x00368CEC
		public void UpdateAttackCommandWeaponAndTrick(DataContext context)
		{
			List<ItemKey> weaponRandomPool = ObjectPool<List<ItemKey>>.Instance.Get();
			weaponRandomPool.Clear();
			for (int i = 0; i < 3; i++)
			{
				ItemKey weaponKey = this._weapons[i];
				bool flag = weaponKey.IsValid() && DomainManager.Item.GetBaseItem(weaponKey).GetCurrDurability() > 0;
				if (flag)
				{
					weaponRandomPool.Add(weaponKey);
				}
			}
			bool flag2 = weaponRandomPool.Count == 0;
			if (flag2)
			{
				for (int j = 3; j < 7; j++)
				{
					ItemKey weaponKey2 = this._weapons[j];
					bool flag3 = weaponKey2.IsValid();
					if (flag3)
					{
						weaponRandomPool.Add(weaponKey2);
					}
				}
			}
			ItemKey cmdWeaponKey = weaponRandomPool[context.Random.Next(0, weaponRandomPool.Count)];
			CombatWeaponData weaponData = this._combatDomain.GetElement_WeaponDataDict(cmdWeaponKey.Id);
			sbyte[] tricks = weaponData.GetWeaponTricks();
			ObjectPool<List<ItemKey>>.Instance.Return(weaponRandomPool);
			this._combatDomain.ChangeWeapon(context, this, this._weapons.IndexOf(cmdWeaponKey), false, false);
			this.SetUsingWeaponIndex(Array.IndexOf<ItemKey>(this._weapons, cmdWeaponKey), context);
			this.SetAttackCommandWeaponKey(cmdWeaponKey, context);
			this.SetAttackCommandTrickType(tricks[context.Random.Next(0, tricks.Length)], context);
			this._combatDomain.UpdateTeammateCommandUsable(context, this, -1);
		}

		// Token: 0x0600601D RID: 24605 RVA: 0x0036AC50 File Offset: 0x00368E50
		public void UpdateAttackCommandSkill(DataContext context)
		{
			int maxScore = 0;
			List<short> skillRandomPool = ObjectPool<List<short>>.Instance.Get();
			skillRandomPool.Clear();
			for (int i = 0; i < this._attackSkillList.Count; i++)
			{
				short skillId = this._attackSkillList[i];
				bool flag = skillId < 0;
				if (!flag)
				{
					int score = this.AiController.CalcAttackSkillScore(context.Random, skillId, false, -1, -1, -1);
					bool flag2 = score < maxScore;
					if (!flag2)
					{
						bool flag3 = score > maxScore;
						if (flag3)
						{
							maxScore = score;
							skillRandomPool.Clear();
						}
						skillRandomPool.Add(skillId);
					}
				}
			}
			this.SetAttackCommandSkillId((skillRandomPool.Count > 0) ? skillRandomPool.GetRandom(context.Random) : -1, context);
			ObjectPool<List<short>>.Instance.Return(skillRandomPool);
			this._combatDomain.UpdateTeammateCommandUsable(context, this, ETeammateCommandImplement.AttackSkill);
		}

		// Token: 0x0600601E RID: 24606 RVA: 0x0036AD2C File Offset: 0x00368F2C
		public void UpdateDefendCommandSkill(DataContext context)
		{
			List<short> skillRandomPool = ObjectPool<List<short>>.Instance.Get();
			skillRandomPool.Clear();
			for (int i = 0; i < this._defenceSkillList.Count; i++)
			{
				short skillId = this._defenceSkillList[i];
				bool flag = skillId >= 0;
				if (flag)
				{
					skillRandomPool.Add(skillId);
				}
			}
			this.SetDefendCommandSkillId((skillRandomPool.Count > 0) ? skillRandomPool[context.Random.Next(0, skillRandomPool.Count)] : -1, context);
			ObjectPool<List<short>>.Instance.Return(skillRandomPool);
			this._combatDomain.UpdateTeammateCommandUsable(context, this, -1);
		}

		// Token: 0x0600601F RID: 24607 RVA: 0x0036ADD4 File Offset: 0x00368FD4
		public void UpdateTeammateCommandInvokers()
		{
			for (int i = 0; i < this._currTeammateCommands.Count; i++)
			{
				sbyte cmdType = this._currTeammateCommands[i];
				bool flag = cmdType < 0;
				if (!flag)
				{
					TeammateCommandItem cmdConfig = TeammateCommand.Instance[cmdType];
					ITeammateCommandInvoker invoker = this.TryCreateTeammateCommandInvoker(cmdConfig, i);
					bool flag2 = invoker == null;
					if (!flag2)
					{
						invoker.Setup();
						this._teammateCommandInvokers.Add(invoker);
					}
				}
			}
		}

		// Token: 0x06006020 RID: 24608 RVA: 0x0036AE50 File Offset: 0x00369050
		private ITeammateCommandInvoker TryCreateTeammateCommandInvoker(TeammateCommandItem cmdConfig, int i)
		{
			ETeammateCommandType type = cmdConfig.Type;
			if (!true)
			{
			}
			ITeammateCommandInvoker result;
			if (type != ETeammateCommandType.Negative)
			{
				if (type != ETeammateCommandType.GearMate)
				{
					result = null;
				}
				else
				{
					ETeammateCommandImplement implement = cmdConfig.Implement;
					if (!true)
					{
					}
					ITeammateCommandInvoker teammateCommandInvoker;
					if (implement != ETeammateCommandImplement.GearMateA)
					{
						if (implement != ETeammateCommandImplement.GearMateB)
						{
							teammateCommandInvoker = null;
						}
						else
						{
							teammateCommandInvoker = new TeammateCommandInvokerAutoDefend(this._id, i);
						}
					}
					else
					{
						teammateCommandInvoker = new TeammateCommandInvokerCooldown(this._id, i);
					}
					if (!true)
					{
					}
					result = teammateCommandInvoker;
				}
			}
			else
			{
				ETeammateCommandImplement implement2 = cmdConfig.Implement;
				if (!true)
				{
				}
				ITeammateCommandInvoker teammateCommandInvoker;
				if (implement2 != ETeammateCommandImplement.InterruptSkill)
				{
					if (implement2 != ETeammateCommandImplement.InterruptOtherAction)
					{
						teammateCommandInvoker = new TeammateCommandInvokerFrame(this._id, i);
					}
					else
					{
						teammateCommandInvoker = new TeammateCommandInvokerOtherActionProgress(this._id, i);
					}
				}
				else
				{
					teammateCommandInvoker = new TeammateCommandInvokerCombatSkillProgress(this._id, i);
				}
				if (!true)
				{
				}
				result = teammateCommandInvoker;
			}
			if (!true)
			{
			}
			return result;
		}

		// Token: 0x06006021 RID: 24609 RVA: 0x0036AF1C File Offset: 0x0036911C
		public bool UpdateTeammateCharStatus(DataContext context)
		{
			bool flag = !DomainManager.Combat.IsMainCharacter(this);
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				int[] charList = DomainManager.Combat.GetCharacterList(this.IsAlly);
				for (int i = 1; i < charList.Length; i++)
				{
					int charId = charList[i];
					bool flag2 = charId < 0;
					if (!flag2)
					{
						CombatCharacter teammateChar = DomainManager.Combat.GetElement_CombatCharacterDict(charId);
						bool flag3 = teammateChar.GetExecutingTeammateCommand() < 0;
						if (flag3)
						{
							bool flag4 = this.TeammateBeforeMainChar == charId;
							if (flag4)
							{
								this.TeammateBeforeMainChar = -1;
							}
							bool flag5 = this.TeammateAfterMainChar == charId;
							if (flag5)
							{
								this.TeammateAfterMainChar = -1;
							}
						}
						else
						{
							bool flag6 = teammateChar.ExecutingTeammateCommandConfig.IntoCombatField && !teammateChar.GetVisible();
							if (flag6)
							{
								bool flag7 = teammateChar.ExecutingTeammateCommandConfig.PosOffset > 0;
								if (flag7)
								{
									this.TeammateBeforeMainChar = charId;
								}
								else
								{
									this.TeammateAfterMainChar = charId;
								}
							}
							bool flag8 = teammateChar.UpdateTeammateCommandState(context);
							if (flag8)
							{
								this.ActingTeammateCommandChar = teammateChar;
								this.StateMachine.TranslateState(CombatCharacterStateType.TeammateCommand);
								return true;
							}
						}
					}
				}
				result = false;
			}
			return result;
		}

		// Token: 0x06006022 RID: 24610 RVA: 0x0036B04C File Offset: 0x0036924C
		public bool PreparingOrDoingTeammateCommand()
		{
			return this.TeammateAfterMainChar >= 0 || this.TeammateBeforeMainChar >= 0;
		}

		// Token: 0x06006023 RID: 24611 RVA: 0x0036B068 File Offset: 0x00369268
		public bool PreparingTeammateCommand()
		{
			return (this.TeammateAfterMainChar >= 0 && this._combatDomain.GetElement_CombatCharacterDict(this.TeammateAfterMainChar).TeammateCommandLeftPrepareFrame >= 0) || (this.TeammateBeforeMainChar >= 0 && this._combatDomain.GetElement_CombatCharacterDict(this.TeammateBeforeMainChar).TeammateCommandLeftPrepareFrame >= 0);
		}

		// Token: 0x06006024 RID: 24612 RVA: 0x0036B0C8 File Offset: 0x003692C8
		public int CalcTeammateCommandRepairDurabilityValue(ItemKey equipKey)
		{
			EquipmentBase item = equipKey.IsValid() ? DomainManager.Item.TryGetBaseEquipment(equipKey) : null;
			bool flag = item == null;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				short current = item.GetCurrDurability();
				int costed = DomainManager.Combat.EquipmentOldDurability.GetValueOrDefault(equipKey) - (int)current;
				bool flag2 = costed <= 0;
				if (flag2)
				{
					result = 0;
				}
				else
				{
					sbyte repairType = ItemTemplateHelper.GetCraftRequiredLifeSkillType(equipKey.ItemType, equipKey.TemplateId);
					short attainment = this._character.GetLifeSkillAttainment(repairType);
					result = CFormula.CalcPartRepairDurabilityValue(item.GetGrade(), (int)attainment, (int)current, costed);
				}
			}
			return result;
		}

		// Token: 0x06006025 RID: 24613 RVA: 0x0036B160 File Offset: 0x00369360
		public void ChangeAffectingDefenseSkillLeftFrame(DataContext context, CValuePercent delta)
		{
			bool flag = this._affectingDefendSkillId < 0;
			if (!flag)
			{
				int deltaFrame = (int)this.DefendSkillTotalFrame * delta;
				this.DefendSkillLeftFrame = (short)Math.Clamp((int)this.DefendSkillLeftFrame + deltaFrame, 1, (int)this.DefendSkillTotalFrame);
				bool flag2 = this.DefendSkillLeftFrame > 1;
				if (!flag2)
				{
					this.SetAffectingDefendSkillId(-1, context);
					DomainManager.Combat.SetProperLoopAniAndParticle(context, this, false);
				}
			}
		}

		// Token: 0x06006026 RID: 24614 RVA: 0x0036B1CC File Offset: 0x003693CC
		public bool AiCanCast(short skillId)
		{
			CombatSkillData skillData;
			return DomainManager.Combat.TryGetCombatSkillData(this._id, skillId, out skillData) && skillData.GetCanUse();
		}

		// Token: 0x06006027 RID: 24615 RVA: 0x0036B1F8 File Offset: 0x003693F8
		public sbyte AiGetCombatSkillRequireTrickType(short skillId)
		{
			bool flag = skillId < 0;
			sbyte result;
			if (flag)
			{
				result = -1;
			}
			else
			{
				GameData.Domains.CombatSkill.CombatSkill skill;
				bool flag2 = !DomainManager.CombatSkill.TryGetElement_CombatSkills(new ValueTuple<int, short>(this._id, skillId), out skill);
				if (flag2)
				{
					result = -1;
				}
				else
				{
					List<NeedTrick> skillTricks = ObjectPool<List<NeedTrick>>.Instance.Get();
					skillTricks.Clear();
					DomainManager.CombatSkill.GetCombatSkillCostTrick(skill, skillTricks, true);
					skillTricks.RemoveAll((NeedTrick needTrick) => !this._weaponTricks.Exist(needTrick.TrickType) || this.GetTrickCount(needTrick.TrickType) >= needTrick.NeedCount);
					sbyte require = (skillTricks.Count > 0) ? skillTricks[0].TrickType : -1;
					ObjectPool<List<NeedTrick>>.Instance.Return(skillTricks);
					result = require;
				}
			}
			return result;
		}

		// Token: 0x06006028 RID: 24616 RVA: 0x0036B2A0 File Offset: 0x003694A0
		public bool AiCastCheckRange()
		{
			short currentDistance = DomainManager.Combat.GetCurrentDistance();
			OuterAndInnerShorts attackRange = this.GetAttackRange();
			int outOfRange = Math.Max((int)(attackRange.Outer - currentDistance), (int)(currentDistance - attackRange.Inner));
			bool flag = outOfRange < 0;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				int addRange = (int)(this.GetCharacter().GetCombatSkillGridCost(this.GetPreparingSkillId()) * 5 + 5);
				result = (addRange >= outOfRange);
			}
			return result;
		}

		// Token: 0x06006029 RID: 24617 RVA: 0x0036B308 File Offset: 0x00369508
		public int AiGetFirstChangeableWeaponIndex(int minIndex, int maxIndex)
		{
			short distance = DomainManager.Combat.GetCurrentDistance();
			int from = Math.Max(minIndex, 0);
			int to = Math.Min(maxIndex + 1, this._weapons.Length);
			for (int i = from; i < to; i++)
			{
				ItemKey weaponKey = this._weapons[i];
				bool flag = !weaponKey.IsValid();
				if (!flag)
				{
					CombatWeaponData weaponData = DomainManager.Combat.GetElement_WeaponDataDict(weaponKey.Id);
					bool flag2 = !weaponData.GetCanChangeTo();
					if (!flag2)
					{
						OuterAndInnerShorts range = this.CalcAttackRangeImmediate(-1, i);
						bool flag3 = range.Outer <= distance && distance <= range.Inner;
						if (flag3)
						{
							return i;
						}
					}
				}
			}
			return -1;
		}

		// Token: 0x0600602A RID: 24618 RVA: 0x0036B3CC File Offset: 0x003695CC
		public bool AiCanRepair(ItemKey toolKey, ItemKey itemKey)
		{
			return DomainManager.Building.CheckRepairConditionIsMeet(this._id, toolKey, itemKey, BuildingBlockKey.Invalid);
		}

		// Token: 0x0600602B RID: 24619 RVA: 0x0036B3E8 File Offset: 0x003695E8
		public bool AiCanRepair(IEnumerable<ItemKey> tools, ItemKey itemKey)
		{
			return tools.Any((ItemKey tool) => this.AiCanRepair(tool, itemKey));
		}

		// Token: 0x0600602C RID: 24620 RVA: 0x0036B41C File Offset: 0x0036961C
		[return: TupleElementNames(new string[]
		{
			"targetKey",
			"toolKey"
		})]
		public ValueTuple<ItemKey, ItemKey> AiSelectRepairTarget(IEnumerable<sbyte> equipmentSlots)
		{
			DataContext context = DomainManager.Combat.Context;
			ItemKey[] equipments = this.GetCharacter().GetEquipment();
			List<ItemKey> tools = ObjectPool<List<ItemKey>>.Instance.Get();
			tools.Clear();
			tools.AddRange(from itemKey in this.GetValidItems()
			where itemKey.ItemType == 6
			select itemKey);
			List<ItemKey> targets = ObjectPool<List<ItemKey>>.Instance.Get();
			targets.Clear();
			targets.AddRange(from itemKey in (from slot in equipmentSlots
			select equipments[(int)slot]).Where(delegate(ItemKey itemKey)
			{
				ItemKey itemKey2 = itemKey;
				return itemKey2.IsValid();
			})
			let baseItem = DomainManager.Item.GetBaseItem(itemKey)
			where baseItem.GetCurrDurability() <= 0
			where this.AiCanRepair(tools, itemKey)
			select itemKey);
			ValueTuple<ItemKey, ItemKey> result = new ValueTuple<ItemKey, ItemKey>(ItemKey.Invalid, ItemKey.Invalid);
			bool flag = targets.Count > 0;
			if (flag)
			{
				ItemKey targetKey = targets.GetRandom(context.Random);
				ItemKey toolKey2 = tools.First((ItemKey toolKey) => this.AiCanRepair(toolKey, targetKey));
				result = new ValueTuple<ItemKey, ItemKey>(targetKey, toolKey2);
			}
			ObjectPool<List<ItemKey>>.Instance.Return(tools);
			ObjectPool<List<ItemKey>>.Instance.Return(targets);
			return result;
		}

		// Token: 0x0600602D RID: 24621 RVA: 0x0036B608 File Offset: 0x00369808
		[return: TupleElementNames(new string[]
		{
			"weaponKey",
			"index"
		})]
		public IEnumerable<ValueTuple<ItemKey, int>> AiCanChangeToWeapons()
		{
			int num;
			for (int i = 0; i < this._weapons.Length; i = num + 1)
			{
				bool flag = i == this._usingWeaponIndex;
				if (!flag)
				{
					ItemKey weaponKey = this._weapons[i];
					bool flag2 = !weaponKey.IsValid();
					if (!flag2)
					{
						CombatWeaponData weaponData = DomainManager.Combat.GetElement_WeaponDataDict(weaponKey.Id);
						bool canChangeTo = weaponData.GetCanChangeTo();
						if (canChangeTo)
						{
							yield return new ValueTuple<ItemKey, int>(weaponKey, i);
						}
						weaponKey = default(ItemKey);
						weaponData = null;
					}
				}
				num = i;
			}
			yield break;
		}

		// Token: 0x0600602E RID: 24622 RVA: 0x0036B618 File Offset: 0x00369818
		int IExpressionConverter.GetPersonalityValue(int personalityType)
		{
			return (int)this.GetPersonalityValue((sbyte)personalityType);
		}

		// Token: 0x0600602F RID: 24623 RVA: 0x0036B622 File Offset: 0x00369822
		int IExpressionConverter.GetConsummateLevel()
		{
			return (int)(this.IsAlly ? GlobalConfig.Instance.MaxConsummateLevel : this._character.GetConsummateLevel());
		}

		// Token: 0x06006030 RID: 24624 RVA: 0x0036B643 File Offset: 0x00369843
		int IExpressionConverter.GetBehaviorType()
		{
			return (int)this._character.GetBehaviorType();
		}

		// Token: 0x17000368 RID: 872
		// (get) Token: 0x06006031 RID: 24625 RVA: 0x0036B650 File Offset: 0x00369850
		bool IAiParticipant.DisableAi
		{
			get
			{
				TeammateCommandItem executingTeammateCommandConfig = this.ExecutingTeammateCommandConfig;
				return executingTeammateCommandConfig != null && executingTeammateCommandConfig.DisableAi;
			}
		}

		// Token: 0x17000369 RID: 873
		// (get) Token: 0x06006032 RID: 24626 RVA: 0x0036B670 File Offset: 0x00369870
		public bool NoBlockAttack
		{
			get
			{
				return this.GetChangeTrickAttack() || this.IsUnlockAttack;
			}
		}

		// Token: 0x1700036A RID: 874
		// (get) Token: 0x06006033 RID: 24627 RVA: 0x0036B683 File Offset: 0x00369883
		private WeaponItem UsingWeaponConfig
		{
			get
			{
				return Config.Weapon.Instance[this._weapons[this._usingWeaponIndex].TemplateId];
			}
		}

		// Token: 0x1700036B RID: 875
		// (get) Token: 0x06006034 RID: 24628 RVA: 0x0036B6A5 File Offset: 0x003698A5
		private sbyte UsingWeaponAction
		{
			get
			{
				return this.UsingWeaponConfig.WeaponAction;
			}
		}

		// Token: 0x1700036C RID: 876
		// (get) Token: 0x06006035 RID: 24629 RVA: 0x0036B6B2 File Offset: 0x003698B2
		private string AttackPostfix
		{
			get
			{
				BossItem bossConfig = this.BossConfig;
				return ((bossConfig != null) ? bossConfig.AttackEffectPostfix[this._usingWeaponIndex] : null) ?? string.Empty;
			}
		}

		// Token: 0x06006036 RID: 24630 RVA: 0x0036B6DC File Offset: 0x003698DC
		public sbyte GetNormalAttackPosition(sbyte trickType)
		{
			bool flag = this.BossConfig != null;
			sbyte result;
			if (flag)
			{
				result = this.BossConfig.AttackDistances[(int)this._bossPhase][this._usingWeaponIndex];
			}
			else
			{
				bool flag2 = this.AnimalConfig != null;
				if (flag2)
				{
					result = this.AnimalConfig.AttackDistances[this._usingWeaponIndex];
				}
				else
				{
					result = TrickType.Instance[trickType].AttackDistance[(int)this.UsingWeaponAction];
				}
			}
			return result;
		}

		// Token: 0x06006037 RID: 24631 RVA: 0x0036B758 File Offset: 0x00369958
		public string GetNormalAttackParticle(sbyte trickType)
		{
			bool flag = this.BossConfig != null;
			string result;
			if (flag)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(1, 3);
				defaultInterpolatedStringHandler.AppendFormatted(this.BossConfig.AttackParticles[(int)this._bossPhase]);
				defaultInterpolatedStringHandler.AppendLiteral("_");
				defaultInterpolatedStringHandler.AppendFormatted<byte>(this.PursueAttackCount);
				defaultInterpolatedStringHandler.AppendFormatted(this.AttackPostfix);
				result = defaultInterpolatedStringHandler.ToStringAndClear();
			}
			else
			{
				bool flag2 = this.AnimalConfig != null;
				if (flag2)
				{
					result = this.AnimalConfig.AttackParticles[this._usingWeaponIndex];
				}
				else
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(1, 2);
					defaultInterpolatedStringHandler.AppendFormatted(TrickType.Instance[trickType].AttackParticles[(int)this.UsingWeaponAction]);
					defaultInterpolatedStringHandler.AppendLiteral("_");
					defaultInterpolatedStringHandler.AppendFormatted<byte>(this.PursueAttackCount);
					result = defaultInterpolatedStringHandler.ToStringAndClear();
				}
			}
			return result;
		}

		// Token: 0x06006038 RID: 24632 RVA: 0x0036B844 File Offset: 0x00369A44
		public string GetNormalAttackSound(sbyte trickType)
		{
			bool flag = this.BossConfig != null;
			string result;
			if (flag)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(1, 2);
				defaultInterpolatedStringHandler.AppendFormatted(this.BossConfig.AttackSounds[(int)this._bossPhase]);
				defaultInterpolatedStringHandler.AppendLiteral("_");
				defaultInterpolatedStringHandler.AppendFormatted<byte>(this.PursueAttackCount);
				result = defaultInterpolatedStringHandler.ToStringAndClear();
			}
			else
			{
				bool flag2 = this.AnimalConfig != null;
				if (flag2)
				{
					result = this.AnimalConfig.AttackSounds[this._usingWeaponIndex];
				}
				else
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(1, 3);
					defaultInterpolatedStringHandler.AppendFormatted(TrickType.Instance[trickType].SoundEffects[(int)this.UsingWeaponAction]);
					defaultInterpolatedStringHandler.AppendLiteral("_");
					defaultInterpolatedStringHandler.AppendFormatted<byte>(this.PursueAttackCount);
					defaultInterpolatedStringHandler.AppendFormatted(this.UsingWeaponConfig.SwingSoundsSuffix);
					result = defaultInterpolatedStringHandler.ToStringAndClear();
				}
			}
			return result;
		}

		// Token: 0x06006039 RID: 24633 RVA: 0x0036B938 File Offset: 0x00369B38
		public string GetNormalAttackAnimation(sbyte trickType)
		{
			bool flag = this.BossConfig != null;
			string result;
			if (flag)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(1, 3);
				defaultInterpolatedStringHandler.AppendFormatted(this.BossConfig.AttackAnimation);
				defaultInterpolatedStringHandler.AppendLiteral("_");
				defaultInterpolatedStringHandler.AppendFormatted<byte>(this.PursueAttackCount);
				defaultInterpolatedStringHandler.AppendFormatted(this.AttackPostfix);
				result = defaultInterpolatedStringHandler.ToStringAndClear();
			}
			else
			{
				bool flag2 = this.AnimalConfig != null;
				if (flag2)
				{
					result = TrickType.Instance[trickType].AttackAnimations[(int)this.UsingWeaponAction];
				}
				else
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(1, 2);
					defaultInterpolatedStringHandler.AppendFormatted(TrickType.Instance[trickType].AttackAnimations[(int)this.UsingWeaponAction]);
					defaultInterpolatedStringHandler.AppendLiteral("_");
					defaultInterpolatedStringHandler.AppendFormatted<byte>(this.PursueAttackCount);
					result = defaultInterpolatedStringHandler.ToStringAndClear();
				}
			}
			return result;
		}

		// Token: 0x0600603A RID: 24634 RVA: 0x0036BA18 File Offset: 0x00369C18
		public string GetNormalAttackAnimationFull(sbyte trickType)
		{
			string animation = this.GetNormalAttackAnimation(trickType);
			return this.GetNormalAttackAnimationFull(animation);
		}

		// Token: 0x0600603B RID: 24635 RVA: 0x0036BA3C File Offset: 0x00369C3C
		public string GetNormalAttackAnimationFull(string animation)
		{
			bool flag = this.BossConfig != null;
			string result;
			if (flag)
			{
				result = this.BossConfig.AniPrefix[(int)this._bossPhase] + animation;
			}
			else
			{
				bool flag2 = this.AnimalConfig != null;
				if (flag2)
				{
					result = this.AnimalConfig.AniPrefix + animation;
				}
				else
				{
					result = animation;
				}
			}
			return result;
		}

		// Token: 0x0600603C RID: 24636 RVA: 0x0036BA9C File Offset: 0x00369C9C
		public void PlayBeHitSound(DataContext context, WeaponItem weapon, CombatCharacter attacker, bool critical)
		{
			bool flag = attacker.NoBlockAttack || critical;
			if (flag)
			{
				DomainManager.Combat.PlayHitSound(context, this, weapon);
			}
			else
			{
				DomainManager.Combat.PlayBlockSound(context, this);
			}
		}

		// Token: 0x0600603D RID: 24637 RVA: 0x0036BAD4 File Offset: 0x00369CD4
		public void PlayWinAnimation(DataContext context)
		{
			bool isActorSkeleton = this.IsActorSkeleton;
			if (isActorSkeleton)
			{
				sbyte gender = this.GetCharacter().GetDisplayingGender();
				int aniIndex = (int)((gender >= 0 && gender < 2) ? gender : 1);
				this.SetAnimationToPlayOnce(this.WinAni[aniIndex], context);
				this.SetAnimationToLoop(this.WinAniLoop[aniIndex], context);
			}
			else
			{
				this.SetAnimationToLoop(DomainManager.Combat.GetIdleAni(this), context);
			}
		}

		// Token: 0x0600603E RID: 24638 RVA: 0x0036BB40 File Offset: 0x00369D40
		public bool ApplyChangeTrickFlawOrAcupoint(DataContext context, CombatCharacter defender, sbyte bodyPart)
		{
			bool flag = this.PursueAttackCount != 0 || !this.GetChangeTrickAttack();
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				sbyte level = DomainManager.Combat.GetUsingWeapon(this).GetAttackPreparePointCost();
				bool flag2 = this.ChangeTrickFlawOrAcupointType == EFlawOrAcupointType.Flaw;
				if (flag2)
				{
					DomainManager.Combat.AddFlaw(context, defender, level, new ValueTuple<int, short>(-1, -1), bodyPart, 1, true);
				}
				else
				{
					bool flag3 = this.ChangeTrickFlawOrAcupointType == EFlawOrAcupointType.Acupoint;
					if (!flag3)
					{
						return false;
					}
					DomainManager.Combat.AddAcupoint(context, defender, level, new ValueTuple<int, short>(-1, -1), bodyPart, 1, true);
				}
				result = true;
			}
			return result;
		}

		// Token: 0x0600603F RID: 24639 RVA: 0x0036BBE0 File Offset: 0x00369DE0
		public int MarkCountChangeToDamageValue(sbyte bodyPart, bool inner, int count)
		{
			int[] bodyPartSteps = inner ? this._damageStepCollection.InnerDamageSteps : this._damageStepCollection.OuterDamageSteps;
			int existValue = (inner ? this._innerDamageValue : this._outerDamageValue)[(int)bodyPart];
			int canAddInjury = (int)(6 - this._injuries.Get(bodyPart, inner));
			int injuryCount = Math.Min(canAddInjury, count);
			int fatalCount = count - injuryCount;
			int markValue = injuryCount * bodyPartSteps[(int)bodyPart] + fatalCount * this._damageStepCollection.FatalDamageStep;
			return markValue - existValue - ((fatalCount > 0) ? this._fatalDamageValue : 0);
		}

		// Token: 0x06006040 RID: 24640 RVA: 0x0036BC6A File Offset: 0x00369E6A
		public void RemoveInjury(DataContext context, sbyte bodyPart, bool inner, sbyte count = 1)
		{
			DomainManager.Combat.RemoveInjury(context, this, bodyPart, inner, count, true, false);
		}

		// Token: 0x06006041 RID: 24641 RVA: 0x0036BC80 File Offset: 0x00369E80
		public bool RemoveRandomInjury(DataContext context, sbyte count = 1)
		{
			Injuries newInjuries = this._injuries.Subtract(this._oldInjuries);
			bool anyInner = newInjuries.HasAnyInjury(true);
			bool anyOuter = newInjuries.HasAnyInjury(false);
			bool flag = !anyInner && !anyOuter;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				bool inner = context.Random.RandomIsInner(anyInner, anyOuter);
				List<sbyte> pool = ObjectPool<List<sbyte>>.Instance.Get();
				for (sbyte part = 0; part < 7; part += 1)
				{
					bool flag2 = newInjuries.Get(part, inner) > 0;
					if (flag2)
					{
						pool.Add(part);
					}
				}
				this.RemoveInjury(context, pool.GetRandom(context.Random), inner, count);
				ObjectPool<List<sbyte>>.Instance.Return(pool);
				result = true;
			}
			return result;
		}

		// Token: 0x06006042 RID: 24642 RVA: 0x0036BD40 File Offset: 0x00369F40
		private void ListenCharacterField(ushort fieldId, Action<DataContext, DataUid> handler)
		{
			DataUid uid = new DataUid(4, 0, (ulong)((long)this._id), (uint)fieldId);
			GameDataBridge.AddPostDataModificationHandler(uid, this.DataHandlerKey, handler);
			this._markDataUids.Add(uid);
		}

		// Token: 0x06006043 RID: 24643 RVA: 0x0036BD7C File Offset: 0x00369F7C
		public void RegisterMarkHandler()
		{
			this.ListenCharacterField(59, new Action<DataContext, DataUid>(this._defeatMarkCollection.SyncWugMark));
			this.ListenCharacterField(21, new Action<DataContext, DataUid>(this._defeatMarkCollection.SyncQiDisorderMark));
			this.ListenCharacterField(19, new Action<DataContext, DataUid>(this._defeatMarkCollection.SyncHealthMark));
		}

		// Token: 0x06006044 RID: 24644 RVA: 0x0036BDD8 File Offset: 0x00369FD8
		public void UnRegisterMarkHandler()
		{
			foreach (DataUid uid in this._markDataUids)
			{
				GameDataBridge.RemovePostDataModificationHandler(uid, this.DataHandlerKey);
			}
			this._markDataUids.Clear();
		}

		// Token: 0x1700036D RID: 877
		// (get) Token: 0x06006045 RID: 24645 RVA: 0x0036BE40 File Offset: 0x0036A040
		public bool NeedNormalAttack
		{
			get
			{
				bool flag = this.NeedNormalAttackSkipPrepare > 0 || this.NeedFreeAttack || this.NeedChangeTrickAttack;
				bool result;
				if (flag)
				{
					result = true;
				}
				else
				{
					bool canNormalAttackImmediate = this.CanNormalAttackImmediate;
					if (canNormalAttackImmediate)
					{
						result = (this.GetReserveNormalAttack() || this.NeedNormalAttackImmediate);
					}
					else
					{
						this.NeedNormalAttackImmediate = false;
						result = false;
					}
				}
				return result;
			}
		}

		// Token: 0x1700036E RID: 878
		// (get) Token: 0x06006046 RID: 24646 RVA: 0x0036BE9C File Offset: 0x0036A09C
		public bool CanNormalAttackImmediate
		{
			get
			{
				bool flag = this.NeedNormalAttackSkipPrepare > 0 || this.NeedFreeAttack || this.NeedChangeTrickAttack;
				bool result;
				if (flag)
				{
					result = false;
				}
				else
				{
					bool flag2 = this._normalAttackRecovery.Silencing || this.IsJumping;
					if (flag2)
					{
						result = false;
					}
					else
					{
						bool hasDoingOrReserve = this.StateMachine.GetCurrentStateType() > CombatCharacterStateType.Idle || this.PreparingOrDoingTeammateCommand();
						result = (!hasDoingOrReserve || (this._preparingSkillId >= 0 && this.CanNormalAttackInPrepareSkill));
					}
				}
				return result;
			}
		}

		// Token: 0x1700036F RID: 879
		// (get) Token: 0x06006047 RID: 24647 RVA: 0x0036BF22 File Offset: 0x0036A122
		private int AttackSpeed
		{
			get
			{
				return (int)this._character.GetAttackSpeed();
			}
		}

		// Token: 0x06006048 RID: 24648 RVA: 0x0036BF30 File Offset: 0x0036A130
		public int CalcNormalAttackStartupFrames()
		{
			GameData.Domains.Item.Weapon weapon = DomainManager.Combat.GetUsingWeapon(this);
			return this.CalcNormalAttackStartupFrames(weapon);
		}

		// Token: 0x06006049 RID: 24649 RVA: 0x0036BF58 File Offset: 0x0036A158
		public int CalcNormalAttackStartupFrames(GameData.Domains.Item.Weapon weapon)
		{
			WeaponItem config = Config.Weapon.Instance[weapon.GetTemplateId()];
			int frames = weapon.CalcAttackStartupOrRecoveryFrame(this.AttackSpeed, config.BaseStartupFrames);
			FlawOrAcupointCollection acupoint = this._acupointCollection;
			int acupointAddPercent = acupoint.CalcAcupointParam(3) + acupoint.CalcAcupointParam(4);
			frames = DomainManager.SpecialEffect.ModifyValue(this._id, 283, frames, -1, -1, -1, 0, 0, acupointAddPercent, 0);
			return Math.Max(frames, (int)GlobalConfig.Instance.MinPrepareFrame);
		}

		// Token: 0x0600604A RID: 24650 RVA: 0x0036BFD8 File Offset: 0x0036A1D8
		public short CalcNormalAttackAnimationFrames(float animDuration)
		{
			FlawOrAcupointCollection acupoint = this._acupointCollection;
			int acupointAddPercent = acupoint.CalcAcupointParam(3) + acupoint.CalcAcupointParam(4);
			acupointAddPercent /= 5;
			animDuration *= (float)(100 + acupointAddPercent) / 100f;
			return (short)Math.Round((double)(animDuration * 60f), MidpointRounding.AwayFromZero);
		}

		// Token: 0x0600604B RID: 24651 RVA: 0x0036C024 File Offset: 0x0036A224
		public int CalcNormalAttackRecoveryFrames(GameData.Domains.Item.Weapon weapon)
		{
			WeaponItem config = Config.Weapon.Instance[weapon.GetTemplateId()];
			int frames = weapon.CalcAttackStartupOrRecoveryFrame(this.AttackSpeed, config.BaseRecoveryFrames);
			frames = DomainManager.SpecialEffect.ModifyValue(this._id, 321, frames, -1, -1, -1, 0, 0, 0, 0);
			return Math.Max(frames, 1);
		}

		// Token: 0x0600604C RID: 24652 RVA: 0x0036C080 File Offset: 0x0036A280
		public void NormalAttackRecovery(DataContext context)
		{
			bool flag = this.IsAutoNormalAttacking || this.GetChangeTrickAttack();
			if (!flag)
			{
				GameData.Domains.Item.Weapon weapon = DomainManager.Combat.GetUsingWeapon(this);
				int recoveryFrames = this.CalcNormalAttackRecoveryFrames(weapon);
				bool flag2 = this._normalAttackRecovery.Cover(recoveryFrames);
				if (flag2)
				{
					this.SetNormalAttackRecovery(this._normalAttackRecovery, context);
				}
			}
		}

		// Token: 0x0600604D RID: 24653 RVA: 0x0036C0D8 File Offset: 0x0036A2D8
		public short GetOtherActionPrepareFrame(sbyte actionType)
		{
			int prepareFrame = (int)CombatDomain.OtherActionPrepareFrame[(int)actionType];
			if (!true)
			{
			}
			int num;
			if (actionType != 0)
			{
				if (actionType != 1)
				{
					num = 0;
				}
				else
				{
					num = (int)this._character.GetLifeSkillAttainment(9);
				}
			}
			else
			{
				num = (int)this._character.GetLifeSkillAttainment(8);
			}
			if (!true)
			{
			}
			int attainment = num;
			if (!true)
			{
			}
			if (actionType != 0)
			{
				if (actionType != 1)
				{
					num = 0;
				}
				else
				{
					num = 120;
				}
			}
			else
			{
				num = 120;
			}
			if (!true)
			{
			}
			int minPrepareFrame = num;
			bool flag = attainment > 0;
			if (flag)
			{
				prepareFrame = Math.Max(prepareFrame - attainment / 5 * 2, minPrepareFrame);
			}
			int speedPercent = 100;
			if (!true)
			{
			}
			ushort num2;
			switch (actionType)
			{
			case 0:
				num2 = 118;
				break;
			case 1:
				num2 = 121;
				break;
			case 2:
				num2 = 124;
				break;
			default:
				num2 = ushort.MaxValue;
				break;
			}
			if (!true)
			{
			}
			ushort fieldId = num2;
			bool flag2 = fieldId != ushort.MaxValue;
			if (flag2)
			{
				speedPercent += DomainManager.SpecialEffect.GetModifyValue(this._id, fieldId, EDataModifyType.AddPercent, -1, -1, -1, EDataSumType.All);
			}
			prepareFrame = prepareFrame * 100 / Math.Max(speedPercent, GlobalConfig.Instance.HealInjuryPoisonSpeedMinPercent);
			return (short)Math.Clamp(prepareFrame, 0, 32767);
		}

		// Token: 0x17000370 RID: 880
		// (get) Token: 0x0600604E RID: 24654 RVA: 0x0036C1FC File Offset: 0x0036A3FC
		private CombatCharacter MainChar
		{
			get
			{
				return DomainManager.Combat.GetMainCharacter(this.IsAlly);
			}
		}

		// Token: 0x17000371 RID: 881
		// (get) Token: 0x0600604F RID: 24655 RVA: 0x0036C20E File Offset: 0x0036A40E
		private bool IsMainChar
		{
			get
			{
				return DomainManager.Combat.IsMainCharacter(this);
			}
		}

		// Token: 0x06006050 RID: 24656 RVA: 0x0036C21B File Offset: 0x0036A41B
		private bool IsLegSkill(short skillId)
		{
			return skillId >= 0 && DomainManager.CombatSkill.GetSkillType(this._id, skillId) == 5;
		}

		// Token: 0x06006051 RID: 24657 RVA: 0x0036C238 File Offset: 0x0036A438
		private bool IgnoreShoesOrIsNotLegSkill(short skillId)
		{
			return !this.IsLegSkill(skillId) || !this.LegSkillUseShoes();
		}

		// Token: 0x06006052 RID: 24658 RVA: 0x0036C24F File Offset: 0x0036A44F
		private CValuePercent GetEquipmentPower(ItemKey key)
		{
			return (int)DomainManager.Character.GetItemPower(this._id, key);
		}

		// Token: 0x06006053 RID: 24659 RVA: 0x0036C268 File Offset: 0x0036A468
		private GameData.Domains.Item.Weapon GetFinalWeapon(GameData.Domains.Item.Weapon weapon, short skillId, out ItemKey shoesWeaponKey)
		{
			shoesWeaponKey = ItemKey.Invalid;
			bool flag = this.IgnoreShoesOrIsNotLegSkill(skillId);
			GameData.Domains.Item.Weapon result;
			if (flag)
			{
				result = weapon;
			}
			else
			{
				ItemKey shoesKey = this.Armors[5];
				GameData.Domains.Item.Armor shoes = shoesKey.IsValid() ? DomainManager.Item.GetElement_Armors(shoesKey.Id) : null;
				bool flag2 = shoes == null || shoes.GetCurrDurability() <= 0;
				if (flag2)
				{
					result = DomainManager.Item.GetElement_Weapons(this._weapons[3].Id);
				}
				else
				{
					short shoesWeaponTemplateId = Config.Armor.Instance[shoesKey.TemplateId].RelatedWeapon;
					shoesWeaponKey = new ItemKey(0, 0, shoesWeaponTemplateId, -1);
					result = null;
				}
			}
			return result;
		}

		// Token: 0x06006054 RID: 24660 RVA: 0x0036C324 File Offset: 0x0036A524
		private CValuePercentBonus CalcWeaponHitFactor(GameData.Domains.Item.Weapon weapon, sbyte hitType, short skillId)
		{
			ItemKey shoesWeaponKey;
			GameData.Domains.Item.Weapon finalWeapon = this.GetFinalWeapon(weapon, skillId, out shoesWeaponKey);
			bool flag = finalWeapon != null;
			CValuePercentBonus result;
			if (flag)
			{
				result = (int)finalWeapon.GetHitFactors(this._id)[(int)hitType];
			}
			else
			{
				short weaponFactor = Config.Weapon.Instance[shoesWeaponKey.TemplateId].BaseHitFactors[(int)hitType];
				bool flag2 = weaponFactor > 0;
				if (flag2)
				{
					result = (int)weaponFactor * this.GetEquipmentPower(shoesWeaponKey);
				}
				else
				{
					result = (int)weaponFactor;
				}
			}
			return result;
		}

		// Token: 0x06006055 RID: 24661 RVA: 0x0036C3B4 File Offset: 0x0036A5B4
		private CValuePercent CalcWeaponPenetrateFactor(GameData.Domains.Item.Weapon weapon, bool inner, sbyte bodyPart, short skillId)
		{
			ItemKey shoesWeaponKey;
			GameData.Domains.Item.Weapon finalWeapon = this.GetFinalWeapon(weapon, skillId, out shoesWeaponKey);
			int value = (int)((finalWeapon != null) ? finalWeapon.GetPenetrationFactor() : Config.Weapon.Instance[shoesWeaponKey.TemplateId].BasePenetrationFactor);
			value *= this.GetEquipmentPower((finalWeapon != null) ? finalWeapon.GetItemKey() : shoesWeaponKey);
			sbyte innerRatio = (finalWeapon != null) ? DomainManager.Combat.GetElement_WeaponDataDict(finalWeapon.GetId()).GetInnerRatio() : Config.Weapon.Instance[shoesWeaponKey.TemplateId].DefaultInnerRatio;
			CValuePercent finalRatio = (int)(inner ? innerRatio : (100 - innerRatio));
			value *= finalRatio;
			bool ignoreArmor = DomainManager.SpecialEffect.ModifyData(this._id, skillId, 281, false, -1, -1, -1);
			bool flag = bodyPart < 0 || ignoreArmor;
			CValuePercent result;
			if (flag)
			{
				result = value;
			}
			else
			{
				CombatCharacter enemyChar = DomainManager.Combat.GetCombatCharacter(!this.IsAlly, false);
				ItemKey armorKey = enemyChar.Armors[(int)bodyPart];
				GameData.Domains.Item.Armor armor = armorKey.IsValid() ? DomainManager.Item.GetElement_Armors(armorKey.Id) : null;
				int weaponEquipDefense = CombatDomain.CalcWeaponDefend(this, weapon, skillId);
				int armorEquipAttack = CombatDomain.CalcArmorAttack(enemyChar, armor);
				bool flag2 = armorEquipAttack > weaponEquipDefense;
				if (flag2)
				{
					value = CFormula.FormulaCalcWeaponArmorFactor(value, armorEquipAttack, weaponEquipDefense);
				}
				result = value;
			}
			return result;
		}

		// Token: 0x06006056 RID: 24662 RVA: 0x0036C504 File Offset: 0x0036A704
		private CValuePercentBonus CalcArmorAvoidFactor(sbyte hitType, sbyte bodyPart, short skillId)
		{
			CombatCharacter enemyChar = DomainManager.Combat.GetCombatCharacter(!this.IsAlly, false);
			bool ignoreArmor = DomainManager.SpecialEffect.ModifyData(enemyChar.GetId(), skillId, 281, false, -1, -1, -1);
			bool flag = bodyPart < 0 || ignoreArmor;
			CValuePercentBonus result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				ItemKey armorKey = this.Armors[(int)bodyPart];
				GameData.Domains.Item.Armor armor = armorKey.IsValid() ? DomainManager.Item.GetElement_Armors(armorKey.Id) : null;
				bool flag2 = armor == null;
				if (flag2)
				{
					result = 0;
				}
				else
				{
					result = (int)armor.GetAvoidFactors(this._id)[(int)hitType];
				}
			}
			return result;
		}

		// Token: 0x06006057 RID: 24663 RVA: 0x0036C5B8 File Offset: 0x0036A7B8
		private CValuePercentBonus CalcArmorPenetrateResistFactor(GameData.Domains.Item.Weapon weapon, bool inner, sbyte bodyPart, short skillId)
		{
			CombatCharacter enemyChar = DomainManager.Combat.GetCombatCharacter(!this.IsAlly, false);
			bool ignoreArmor = DomainManager.SpecialEffect.ModifyData(enemyChar.GetId(), skillId, 281, false, -1, -1, -1);
			bool flag = bodyPart < 0 || ignoreArmor;
			CValuePercentBonus result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				ItemKey armorKey = this.Armors[(int)bodyPart];
				GameData.Domains.Item.Armor armor = armorKey.IsValid() ? DomainManager.Item.GetElement_Armors(armorKey.Id) : null;
				bool flag2 = armor == null;
				if (flag2)
				{
					result = 0;
				}
				else
				{
					int armorFactor = armor.GetPenetrationResistFactors().Get(inner) * this.GetEquipmentPower(armorKey);
					int weaponEquipAttack = CombatDomain.CalcWeaponAttack(enemyChar, weapon, skillId);
					int armorEquipDefense = CombatDomain.CalcArmorDefend(this, armor);
					bool flag3 = weaponEquipAttack > armorEquipDefense;
					if (flag3)
					{
						armorFactor = CFormula.FormulaCalcWeaponArmorFactor(armorFactor, weaponEquipAttack, armorEquipDefense);
					}
					result = armorFactor;
				}
			}
			return result;
		}

		// Token: 0x06006058 RID: 24664 RVA: 0x0036C6A8 File Offset: 0x0036A8A8
		private int CalcMoveSkillAddHitValue(sbyte hitType)
		{
			bool flag = this._affectingMoveSkillId < 0;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				GameData.Domains.CombatSkill.CombatSkill skill = DomainManager.CombatSkill.GetElement_CombatSkills(new ValueTuple<int, short>(this._id, this._affectingMoveSkillId));
				result = skill.GetAddHitValueOnCast()[(int)hitType];
			}
			return result;
		}

		// Token: 0x06006059 RID: 24665 RVA: 0x0036C6FC File Offset: 0x0036A8FC
		private int CalcDefendSkillAddAvoidValue(sbyte hitType, bool ignoreDefendSkill)
		{
			bool flag = this._affectingDefendSkillId < 0 || ignoreDefendSkill;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				GameData.Domains.CombatSkill.CombatSkill skill = DomainManager.CombatSkill.GetElement_CombatSkills(new ValueTuple<int, short>(this._id, this._affectingDefendSkillId));
				result = skill.GetAddAvoidValueOnCast()[(int)hitType];
			}
			return result;
		}

		// Token: 0x0600605A RID: 24666 RVA: 0x0036C754 File Offset: 0x0036A954
		private int CalcDefendSkillAddPenetrateResistValue(bool inner, bool ignoreDefendSkill)
		{
			bool flag = this._affectingDefendSkillId < 0 || ignoreDefendSkill;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				GameData.Domains.CombatSkill.CombatSkill defendSkill = DomainManager.CombatSkill.GetElement_CombatSkills(new ValueTuple<int, short>(this._id, this._affectingDefendSkillId));
				OuterAndInnerInts addPenetrateResists = defendSkill.GetAddPenetrateResist();
				result = (inner ? addPenetrateResists.Inner : addPenetrateResists.Outer);
			}
			return result;
		}

		// Token: 0x0600605B RID: 24667 RVA: 0x0036C7B4 File Offset: 0x0036A9B4
		private CValueModify CalcTeammateHitModify(sbyte hitType, EDataSumType valueSumType)
		{
			CValueModify result = CValueModify.Zero;
			bool isMainChar = this.IsMainChar;
			if (isMainChar)
			{
				bool flag = valueSumType.ContainsAdd();
				if (flag)
				{
					CValuePercentBonus bonus = DomainManager.SpecialEffect.GetModifyValue(this._id, 184, EDataModifyType.Add, 10, -1, -1, EDataSumType.All);
					int addValue = (from teammateChar in DomainManager.Combat.GetTeammateCharacters(this._id)
					where teammateChar.ExecutingTeammateCommandImplement == ETeammateCommandImplement.AddHit
					let baseValue = teammateChar.GetCharacter().GetHitValues()[(int)hitType]
					select baseValue * teammateChar.ExecutingTeammateCommandConfig.IntArg * bonus).Sum();
					result = result.ChangeA(addValue);
				}
				bool flag2 = valueSumType.ContainsReduce();
				if (flag2)
				{
					int reduceBonus = (from teammateChar in DomainManager.Combat.GetTeammateCharacters(this._id)
					where teammateChar.ExecutingTeammateCommandImplement == ETeammateCommandImplement.ReduceHitAndAvoid
					select teammateChar.ExecutingTeammateCommandConfig.IntArg).Sum();
					result = result.ChangeB(-reduceBonus);
				}
			}
			else
			{
				bool flag3 = valueSumType.ContainsAdd();
				if (flag3)
				{
					bool flag4 = this.ExecutingTeammateCommandImplement == ETeammateCommandImplement.Fight;
					if (flag4)
					{
						result = result.ChangeB(this.ExecutingTeammateCommandConfig.IntArg);
					}
					bool flag5 = this.ExecutingTeammateCommandImplement == ETeammateCommandImplement.Attack;
					if (flag5)
					{
						result = result.ChangeC(DomainManager.SpecialEffect.GetModifyValue(this.MainChar.GetId(), 184, EDataModifyType.Add, 4, -1, -1, EDataSumType.All));
					}
				}
			}
			return result;
		}

		// Token: 0x0600605C RID: 24668 RVA: 0x0036C970 File Offset: 0x0036AB70
		private CValueModify CalcTeammatePenetrateModify()
		{
			CValueModify result = CValueModify.Zero;
			bool isMainChar = this.IsMainChar;
			CValueModify result2;
			if (isMainChar)
			{
				result2 = result;
			}
			else
			{
				bool flag = this.ExecutingTeammateCommandImplement == ETeammateCommandImplement.Fight;
				if (flag)
				{
					result = result.ChangeB(this.ExecutingTeammateCommandConfig.IntArg);
				}
				bool flag2 = this.ExecutingTeammateCommandImplement == ETeammateCommandImplement.Attack;
				if (flag2)
				{
					result = result.ChangeC(DomainManager.SpecialEffect.GetModifyValue(this.MainChar.GetId(), 184, EDataModifyType.Add, 4, -1, -1, EDataSumType.All));
				}
				result2 = result;
			}
			return result2;
		}

		// Token: 0x0600605D RID: 24669 RVA: 0x0036C9F0 File Offset: 0x0036ABF0
		private CValueModify CalcTeammateAvoidModify(sbyte hitType, EDataSumType valueSumType)
		{
			CValueModify result = CValueModify.Zero;
			bool isMainChar = this.IsMainChar;
			if (isMainChar)
			{
				bool flag = valueSumType.ContainsAdd();
				if (flag)
				{
					CValuePercentBonus bonus = DomainManager.SpecialEffect.GetModifyValue(this._id, 184, EDataModifyType.Add, 11, -1, -1, EDataSumType.All);
					int addValue = (from teammateChar in DomainManager.Combat.GetTeammateCharacters(this._id)
					where teammateChar.ExecutingTeammateCommandImplement == ETeammateCommandImplement.AddAvoid
					let baseValue = teammateChar.GetCharacter().GetAvoidValues()[(int)hitType]
					select baseValue * teammateChar.ExecutingTeammateCommandConfig.IntArg * bonus).Sum();
					result = result.ChangeA(addValue);
				}
				bool flag2 = valueSumType.ContainsReduce();
				if (flag2)
				{
					int reduceBonus = (from teammateChar in DomainManager.Combat.GetTeammateCharacters(this._id)
					where teammateChar.ExecutingTeammateCommandImplement == ETeammateCommandImplement.ReduceHitAndAvoid
					select teammateChar.ExecutingTeammateCommandConfig.IntArg).Sum();
					result = result.ChangeB(-reduceBonus);
				}
			}
			else
			{
				bool flag3 = valueSumType.ContainsAdd();
				if (flag3)
				{
					bool flag4 = this.ExecutingTeammateCommandImplement == ETeammateCommandImplement.Fight;
					if (flag4)
					{
						result = result.ChangeB(this.ExecutingTeammateCommandConfig.IntArg);
					}
				}
			}
			return result;
		}

		// Token: 0x0600605E RID: 24670 RVA: 0x0036CB78 File Offset: 0x0036AD78
		private CValueModify CalcTeammatePenetrateResistModify()
		{
			CValueModify result = CValueModify.Zero;
			bool isMainChar = this.IsMainChar;
			CValueModify result2;
			if (isMainChar)
			{
				result2 = result;
			}
			else
			{
				bool flag = this.ExecutingTeammateCommandImplement == ETeammateCommandImplement.Fight;
				if (flag)
				{
					result = result.ChangeB(this.ExecutingTeammateCommandConfig.IntArg);
				}
				result2 = result;
			}
			return result2;
		}

		// Token: 0x0600605F RID: 24671 RVA: 0x0036CBC0 File Offset: 0x0036ADC0
		public int GetHitValue(CombatContext context, sbyte hitType)
		{
			GameData.Domains.CombatSkill.CombatSkill skill = context.Skill;
			int skillHitValue = (skill != null) ? skill.GetHitValue()[(int)hitType] : 0;
			return this.GetHitValue(context.Weapon, hitType, context.BodyPart, skillHitValue, context.SkillTemplateId);
		}

		// Token: 0x06006060 RID: 24672 RVA: 0x0036CC0C File Offset: 0x0036AE0C
		public int GetHitValue(sbyte hitType, sbyte bodyPart = -1, int skillAddPercent = 0, short skillId = -1)
		{
			GameData.Domains.Item.Weapon weapon = DomainManager.Combat.GetUsingWeapon(this);
			return this.GetHitValue(weapon, hitType, bodyPart, skillAddPercent, skillId);
		}

		// Token: 0x06006061 RID: 24673 RVA: 0x0036CC38 File Offset: 0x0036AE38
		public int GetHitValue(GameData.Domains.Item.Weapon weapon, sbyte hitType, sbyte bodyPart, int skillAddPercent = 0, short skillId = -1)
		{
			long value = (long)this._character.GetHitValues()[(int)hitType];
			value = (long)DomainManager.SpecialEffect.ModifyData(this._id, skillId, 158, (int)value, (int)hitType, -1, -1);
			value *= this.CalcWeaponHitFactor(weapon, hitType, skillId);
			bool canAdd = DomainManager.SpecialEffect.ModifyData(this._id, -1, 36, true, (int)hitType, 0, -1);
			bool canReduce = DomainManager.SpecialEffect.ModifyData(this._id, -1, 36, true, (int)hitType, 1, -1);
			CValuePercentBonus addEffectBonus = DomainManager.SpecialEffect.GetModifyValue(this._id, 37, EDataModifyType.Add, (int)hitType, 0, -1, EDataSumType.All);
			CValuePercentBonus reduceEffectBonus = DomainManager.SpecialEffect.GetModifyValue(this._id, 37, EDataModifyType.Add, (int)hitType, 1, -1, EDataSumType.All);
			CombatCharacter enemyChar = this._combatDomain.GetCombatCharacter(!this.IsAlly, true);
			int attackerId = this._id;
			int defenderId = enemyChar.GetId();
			ushort attackerFieldId = (ushort)(56 + hitType);
			ushort defenderFieldId = (ushort)(90 + hitType);
			CValueModify modify = CValueModify.Zero;
			bool flag = canAdd;
			if (flag)
			{
				modify = modify.ChangeA(this.CalcMoveSkillAddHitValue(hitType));
				modify += this.CalcTeammateHitModify(hitType, EDataSumType.OnlyAdd);
				modify += DomainManager.SpecialEffect.GetModify(attackerId, attackerFieldId, (int)skillId, (int)this.PursueAttackCount, (int)bodyPart, EDataSumType.OnlyAdd) * addEffectBonus;
				modify += DomainManager.SpecialEffect.GetModify(defenderId, defenderFieldId, (int)skillId, (int)this.PursueAttackCount, (int)bodyPart, EDataSumType.OnlyAdd) * addEffectBonus;
			}
			bool flag2 = canReduce;
			if (flag2)
			{
				modify += this.CalcTeammateHitModify(hitType, EDataSumType.OnlyReduce);
				modify += DomainManager.SpecialEffect.GetModify(attackerId, attackerFieldId, (int)skillId, (int)this.PursueAttackCount, (int)bodyPart, EDataSumType.OnlyReduce) * reduceEffectBonus;
				modify += DomainManager.SpecialEffect.GetModify(defenderId, defenderFieldId, (int)skillId, (int)this.PursueAttackCount, (int)bodyPart, EDataSumType.OnlyReduce) * reduceEffectBonus;
			}
			modify = modify.MaxB(33);
			value *= modify;
			CValuePercentBonus bonus = skillAddPercent;
			bool flag3 = skillId < 0 && this.GetChangeTrickAttack();
			if (flag3)
			{
				bonus += (int)GlobalConfig.Instance.AttackChangeTrickHitValueAddPercent[(int)weapon.GetAttackPreparePointCost()];
			}
			value *= bonus;
			value = DomainManager.SpecialEffect.ModifyData(attackerId, skillId, attackerFieldId, value, -1, -1, -1);
			value = DomainManager.SpecialEffect.ModifyData(defenderId, skillId, defenderFieldId, value, -1, -1, -1);
			return (int)Math.Clamp(value, 0L, 2147483647L);
		}

		// Token: 0x06006062 RID: 24674 RVA: 0x0036CEB3 File Offset: 0x0036B0B3
		public int GetAvoidValue(CombatContext context, sbyte hitType)
		{
			return this.GetAvoidValue(hitType, context.BodyPart, context.SkillTemplateId, false);
		}

		// Token: 0x06006063 RID: 24675 RVA: 0x0036CECC File Offset: 0x0036B0CC
		public int GetAvoidValue(sbyte hitType, sbyte bodyPart = -1, short skillId = -1, bool ignoreDefendSkill = false)
		{
			int value = this._character.GetAvoidValues()[(int)hitType];
			CombatCharacter enemyChar = this._combatDomain.GetCombatCharacter(!this.IsAlly, false);
			value *= this.CalcArmorAvoidFactor(hitType, bodyPart, skillId);
			bool canAdd = DomainManager.SpecialEffect.ModifyData(this._id, -1, 42, true, (int)hitType, 0, -1);
			bool canReduce = DomainManager.SpecialEffect.ModifyData(this._id, -1, 42, true, (int)hitType, 1, -1);
			CValuePercentBonus addEffectBonus = DomainManager.SpecialEffect.GetModifyValue(this._id, 43, EDataModifyType.Add, (int)hitType, 0, -1, EDataSumType.All);
			CValuePercentBonus reduceEffectBonus = DomainManager.SpecialEffect.GetModifyValue(this._id, 43, EDataModifyType.Add, (int)hitType, 1, -1, EDataSumType.All);
			int attackerId = enemyChar.GetId();
			int defenderId = this._id;
			ushort attackerFieldId = (ushort)(60 + hitType);
			ushort defenderFieldId = (ushort)(94 + hitType);
			CValueModify modify = CValueModify.Zero;
			bool flag = canAdd;
			if (flag)
			{
				modify = modify.ChangeA(this.CalcDefendSkillAddAvoidValue(hitType, ignoreDefendSkill));
				modify += this.CalcTeammateAvoidModify(hitType, EDataSumType.OnlyAdd);
				modify += DomainManager.SpecialEffect.GetModify(attackerId, attackerFieldId, (int)skillId, -1, -1, EDataSumType.OnlyAdd) * addEffectBonus;
				modify += DomainManager.SpecialEffect.GetModify(defenderId, defenderFieldId, (int)skillId, -1, -1, EDataSumType.OnlyAdd) * addEffectBonus;
			}
			bool flag2 = canReduce;
			if (flag2)
			{
				modify += this.CalcTeammateAvoidModify(hitType, EDataSumType.OnlyReduce);
				modify += DomainManager.SpecialEffect.GetModify(attackerId, attackerFieldId, (int)skillId, -1, -1, EDataSumType.OnlyReduce) * reduceEffectBonus;
				modify += DomainManager.SpecialEffect.GetModify(defenderId, defenderFieldId, (int)skillId, -1, -1, EDataSumType.OnlyReduce) * reduceEffectBonus;
			}
			modify = modify.MaxB(33);
			value *= modify;
			value = DomainManager.SpecialEffect.ModifyData(attackerId, skillId, attackerFieldId, value, -1, -1, -1);
			value = DomainManager.SpecialEffect.ModifyData(defenderId, skillId, defenderFieldId, value, -1, -1, -1);
			return Math.Max(value, 1);
		}

		// Token: 0x06006064 RID: 24676 RVA: 0x0036D0C4 File Offset: 0x0036B2C4
		public OuterAndInnerInts GetPenetrate(CombatContext context)
		{
			GameData.Domains.Item.Weapon weapon = context.Weapon;
			sbyte bodyPart = context.BodyPart;
			short skillId = context.SkillTemplateId;
			GameData.Domains.CombatSkill.CombatSkill skill = context.Skill;
			int outer = this.GetPenetrate(false, weapon, bodyPart, skillId, (skill != null) ? skill.GetPenetrations().Outer : 0);
			int inner = this.GetPenetrate(true, weapon, bodyPart, skillId, (skill != null) ? skill.GetPenetrations().Inner : 0);
			return new OuterAndInnerInts(outer, inner);
		}

		// Token: 0x06006065 RID: 24677 RVA: 0x0036D13C File Offset: 0x0036B33C
		public int GetPenetrate(bool inner, GameData.Domains.Item.Weapon weapon, sbyte bodyPart, short skillId = -1, int skillAddPercent = 0)
		{
			long value = (long)(inner ? this._character.GetPenetrations().Inner : this._character.GetPenetrations().Outer);
			value *= this.CalcWeaponPenetrateFactor(weapon, inner, bodyPart, skillId) + skillAddPercent;
			CombatCharacter enemyChar = this._combatDomain.GetCombatCharacter(!this.IsAlly, true);
			int attackerId = this._id;
			int defenderId = enemyChar.GetId();
			ushort attackerFieldId = inner ? 65 : 64;
			ushort defenderFieldId = inner ? 99 : 98;
			CValueModify modify = this.CalcTeammatePenetrateModify();
			modify += DomainManager.SpecialEffect.GetModify(attackerId, attackerFieldId, (int)skillId, (int)bodyPart, -1, EDataSumType.All);
			modify = (modify + DomainManager.SpecialEffect.GetModify(defenderId, defenderFieldId, (int)skillId, (int)bodyPart, -1, EDataSumType.All)).MaxB(33);
			value *= modify;
			value = DomainManager.SpecialEffect.ModifyData(attackerId, skillId, attackerFieldId, value, -1, -1, -1);
			value = DomainManager.SpecialEffect.ModifyData(defenderId, skillId, defenderFieldId, value, -1, -1, -1);
			return (int)Math.Clamp(value, 0L, 2147483647L);
		}

		// Token: 0x06006066 RID: 24678 RVA: 0x0036D260 File Offset: 0x0036B460
		public OuterAndInnerInts GetPenetrateResist(CombatContext context)
		{
			GameData.Domains.Item.Weapon weapon = context.Weapon;
			sbyte bodyPart = context.BodyPart;
			short skillId = context.SkillTemplateId;
			int outer = this.GetPenetrateResist(false, weapon, bodyPart, skillId, false);
			int inner = this.GetPenetrateResist(true, weapon, bodyPart, skillId, false);
			return new OuterAndInnerInts(outer, inner);
		}

		// Token: 0x06006067 RID: 24679 RVA: 0x0036D2B0 File Offset: 0x0036B4B0
		public int GetPenetrateResist(bool inner, GameData.Domains.Item.Weapon weapon, sbyte bodyPart, short skillId = -1, bool ignoreDefendSkill = false)
		{
			long value = (long)(inner ? this._character.GetPenetrationResists().Inner : this._character.GetPenetrationResists().Outer);
			value *= this.CalcArmorPenetrateResistFactor(weapon, inner, bodyPart, skillId);
			CombatCharacter enemyChar = this._combatDomain.GetCombatCharacter(!this.IsAlly, false);
			int attackerId = enemyChar.GetId();
			int defenderId = this._id;
			ushort attackerFieldId = inner ? 67 : 66;
			ushort defenderFieldId = inner ? 101 : 100;
			CValueModify modify = this.CalcTeammatePenetrateResistModify().ChangeA(this.CalcDefendSkillAddPenetrateResistValue(inner, ignoreDefendSkill));
			modify += DomainManager.SpecialEffect.GetModify(attackerId, attackerFieldId, (int)skillId, (int)bodyPart, -1, EDataSumType.All);
			modify = (modify + DomainManager.SpecialEffect.GetModify(defenderId, defenderFieldId, (int)skillId, (int)bodyPart, -1, EDataSumType.All)).MaxB(33);
			value *= modify;
			value = DomainManager.SpecialEffect.ModifyData(attackerId, skillId, attackerFieldId, value, -1, -1, -1);
			value = DomainManager.SpecialEffect.ModifyData(defenderId, skillId, defenderFieldId, value, -1, -1, -1);
			return (int)Math.Clamp(value, 1L, 2147483647L);
		}

		// Token: 0x06006068 RID: 24680 RVA: 0x0036D3D8 File Offset: 0x0036B5D8
		public sbyte GetOrRandomChangeTrickType(IRandomSource random)
		{
			sbyte[] weaponTricks = this.GetWeaponTricks();
			bool flag = this.PlayerChangeTrickType >= 0 && weaponTricks.Exist(this.PlayerChangeTrickType);
			sbyte result;
			if (flag)
			{
				result = this.PlayerChangeTrickType;
			}
			else
			{
				result = weaponTricks.GetRandom(random);
			}
			return result;
		}

		// Token: 0x06006069 RID: 24681 RVA: 0x0036D420 File Offset: 0x0036B620
		public sbyte RandomChangeTrickBodyPart(IRandomSource random, sbyte trickType, short skillId = -1)
		{
			CombatCharacter enemyChar = DomainManager.Combat.GetCombatCharacter(!this.IsAlly, false);
			return DomainManager.Combat.GetAttackBodyPart(this, enemyChar, random, skillId, trickType, -1);
		}

		// Token: 0x0600606A RID: 24682 RVA: 0x0036D45C File Offset: 0x0036B65C
		public sbyte RandomChangeTrickBodyPartByNeiliType(IRandomSource random, sbyte trickType)
		{
			CombatCharacter enemyChar = DomainManager.Combat.GetCombatCharacter(!this.IsAlly, false);
			Dictionary<SkillEffectKey, short> effectDict = this.GetSkillEffectCollection().EffectDict;
			bool flag;
			if (effectDict != null)
			{
				flag = effectDict.Keys.Any((SkillEffectKey x) => x.EffectConfig.TransferProportion > 0);
			}
			else
			{
				flag = false;
			}
			bool flag2 = flag;
			if (flag2)
			{
				byte enemyFiveElements = NeiliType.Instance[enemyChar.OriginNeiliType].FiveElements;
				bool flag3 = enemyFiveElements != 5;
				if (flag3)
				{
					return BodyPartType.TransferFromFiveElementsType(FiveElementsType.Countered[(int)enemyFiveElements]);
				}
				byte selfFiveElements = NeiliType.Instance[this.GetNeiliType()].FiveElements;
				bool flag4 = selfFiveElements != 5;
				if (flag4)
				{
					return BodyPartType.TransferFromFiveElementsType(FiveElementsType.Countering[(int)selfFiveElements]);
				}
			}
			return DomainManager.Combat.GetAttackBodyPart(this, enemyChar, random, -1, trickType, -1);
		}

		// Token: 0x0600606B RID: 24683 RVA: 0x0036D540 File Offset: 0x0036B740
		public sbyte RandomInjuryBodyPart(IRandomSource random, bool inner, IEnumerable<sbyte> partRange = null)
		{
			List<sbyte> pool = ObjectPool<List<sbyte>>.Instance.Get();
			pool.Clear();
			if (partRange == null)
			{
				partRange = this.GetAvailableBodyParts();
			}
			foreach (sbyte possiblePart in partRange)
			{
				bool flag = this._injuries.Get(possiblePart, inner) < 6;
				if (flag)
				{
					pool.Add(possiblePart);
				}
			}
			sbyte part = (pool.Count > 0) ? pool.GetRandom(random) : -1;
			ObjectPool<List<sbyte>>.Instance.Return(pool);
			return part;
		}

		// Token: 0x0600606C RID: 24684 RVA: 0x0036D5E8 File Offset: 0x0036B7E8
		public sbyte RandomInjuryBodyPartMustValid(IRandomSource random, bool inner, IEnumerable<sbyte> partRange = null)
		{
			List<sbyte> pool = ObjectPool<List<sbyte>>.Instance.Get();
			if (partRange == null)
			{
				partRange = this.GetAvailableBodyParts();
			}
			pool.AddRange(partRange);
			sbyte part = this.RandomInjuryBodyPart(random, inner, pool);
			bool flag = part < 0;
			if (flag)
			{
				part = pool.GetRandom(random);
			}
			ObjectPool<List<sbyte>>.Instance.Return(pool);
			return part;
		}

		// Token: 0x17000372 RID: 882
		// (get) Token: 0x0600606D RID: 24685 RVA: 0x0036D63F File Offset: 0x0036B83F
		public bool AnyRawCreate
		{
			get
			{
				return this._rawCreateEffects.Count > 0;
			}
		}

		// Token: 0x0600606E RID: 24686 RVA: 0x0036D650 File Offset: 0x0036B850
		public bool AllRawCreateSlotsBlocked(int effectId)
		{
			SpecialEffectItem config = SpecialEffect.Instance[effectId];
			return !this.GetAllCanRawCreateEquipmentSlots(config.RawCreateType).Any<sbyte>();
		}

		// Token: 0x0600606F RID: 24687 RVA: 0x0036D682 File Offset: 0x0036B882
		public IEnumerable<sbyte> GetAllCanRawCreateEquipmentSlots(ESpecialEffectRawCreateType type)
		{
			if (!true)
			{
			}
			int num;
			switch (type)
			{
			case ESpecialEffectRawCreateType.Sword:
				num = 0;
				break;
			case ESpecialEffectRawCreateType.Blade:
				num = 0;
				break;
			case ESpecialEffectRawCreateType.Polearm:
				num = 0;
				break;
			case ESpecialEffectRawCreateType.Armor:
				num = 1;
				break;
			case ESpecialEffectRawCreateType.Accessory:
				num = 2;
				break;
			default:
				num = -1;
				break;
			}
			if (!true)
			{
			}
			int itemType = num;
			if (!true)
			{
			}
			switch (type)
			{
			case ESpecialEffectRawCreateType.Sword:
				num = 8;
				break;
			case ESpecialEffectRawCreateType.Blade:
				num = 9;
				break;
			case ESpecialEffectRawCreateType.Polearm:
				num = 10;
				break;
			case ESpecialEffectRawCreateType.Armor:
				num = -1;
				break;
			case ESpecialEffectRawCreateType.Accessory:
				num = -1;
				break;
			default:
				num = -1;
				break;
			}
			if (!true)
			{
			}
			int itemSubType = num;
			ItemKey[] equipments = this._character.GetEquipment();
			foreach (sbyte rawCreateSlot in GameData.Domains.Combat.SharedConstValue.AllRawCreateSlots)
			{
				ItemKey equipment = equipments[(int)rawCreateSlot];
				bool flag = !equipment.IsValid() || (itemType >= 0 && itemType != (int)equipment.ItemType);
				if (!flag)
				{
					bool flag2 = this._rawCreateCollection.Contains(equipment);
					if (!flag2)
					{
						short equipmentSubType = ItemTemplateHelper.GetItemSubType(equipment.ItemType, equipment.TemplateId);
						bool flag3 = itemSubType >= 0 && itemSubType != (int)equipmentSubType;
						if (!flag3)
						{
							ItemBase baseItem = DomainManager.Item.GetBaseItem(equipment);
							bool flag4 = baseItem.GetCurrDurability() <= 0;
							if (!flag4)
							{
								yield return rawCreateSlot;
								equipment = default(ItemKey);
								baseItem = null;
							}
						}
					}
				}
			}
			IEnumerator<sbyte> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06006070 RID: 24688 RVA: 0x0036D69C File Offset: 0x0036B89C
		public void InvokeRawCreate(DataContext context, int effectId)
		{
			bool flag = this.IsAlly && DomainManager.Combat.GetAutoCombat() && DomainManager.Combat.AiOptions.AutoUnlock && DomainManager.Combat.AiOptions.SkipRawCreate;
			if (!flag)
			{
				bool flag2 = this._rawCreateEffects.Contains(effectId);
				if (!flag2)
				{
					SpecialEffectItem config = SpecialEffect.Instance[effectId];
					bool flag3 = !this.GetAllCanRawCreateEquipmentSlots(config.RawCreateType).Any<sbyte>();
					if (!flag3)
					{
						this._rawCreateEffects.Add(effectId);
						this.SetRawCreateEffects(this._rawCreateEffects, context);
					}
				}
			}
		}

		// Token: 0x06006071 RID: 24689 RVA: 0x0036D73C File Offset: 0x0036B93C
		public void IgnoreRawCreate(DataContext context, int effectId)
		{
			bool flag = this._rawCreateEffects.Remove(effectId);
			if (flag)
			{
				this.SetRawCreateEffects(this._rawCreateEffects, context);
			}
		}

		// Token: 0x06006072 RID: 24690 RVA: 0x0036D768 File Offset: 0x0036B968
		public void IgnoreAllRawCreate(DataContext context)
		{
			bool flag = this._rawCreateEffects.Count == 0;
			if (!flag)
			{
				this._rawCreateEffects.Clear();
				this.SetRawCreateEffects(this._rawCreateEffects, context);
			}
		}

		// Token: 0x06006073 RID: 24691 RVA: 0x0036D7A4 File Offset: 0x0036B9A4
		public void AutoAllRawCreate(DataContext context)
		{
			bool flag = this._rawCreateEffects.Count == 0;
			if (!flag)
			{
				ItemKey[] equipments = this._character.GetEquipment();
				List<sbyte> validSlots = ObjectPool<List<sbyte>>.Instance.Get();
				for (int i = this._rawCreateEffects.Count - 1; i >= 0; i--)
				{
					int effectId = this._rawCreateEffects[i];
					SpecialEffectItem effectConfig = SpecialEffect.Instance[effectId];
					validSlots.Clear();
					foreach (sbyte slot in this.GetAllCanRawCreateEquipmentSlots(effectConfig.RawCreateType))
					{
						bool allowRawCreate = ItemTemplateHelper.GetAllowRawCreate(equipments[(int)slot].ItemType, equipments[(int)slot].TemplateId);
						if (allowRawCreate)
						{
							validSlots.Add(slot);
						}
					}
					bool flag2 = validSlots.Count > 0;
					if (flag2)
					{
						sbyte validSlot = validSlots.GetRandom(context.Random);
						this.DoRawCreate(context, effectId, validSlot, equipments[(int)validSlot].TemplateId);
					}
					else
					{
						this.IgnoreRawCreate(context, effectId);
					}
				}
				ObjectPool<List<sbyte>>.Instance.Return(validSlots);
			}
		}

		// Token: 0x06006074 RID: 24692 RVA: 0x0036D8F4 File Offset: 0x0036BAF4
		public bool DoRawCreate(DataContext context, int effectId, sbyte equipmentSlot, short newTemplateId)
		{
			bool flag = !GameData.Domains.Combat.SharedConstValue.AllRawCreateSlots.Contains(equipmentSlot);
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				ItemKey[] equipments = this._character.GetEquipment();
				ItemKey oldKey = equipments[(int)equipmentSlot];
				bool flag2 = !oldKey.IsValid() || this._rawCreateCollection.Contains(oldKey);
				if (flag2)
				{
					result = false;
				}
				else
				{
					sbyte itemType = oldKey.ItemType;
					if (!true)
					{
					}
					int num;
					switch (itemType)
					{
					case 0:
						num = Config.Weapon.Instance.Count;
						break;
					case 1:
						num = Config.Armor.Instance.Count;
						break;
					case 2:
						num = Config.Accessory.Instance.Count;
						break;
					default:
						num = -1;
						break;
					}
					if (!true)
					{
					}
					int count = num;
					bool flag3 = newTemplateId < 0 || (int)newTemplateId >= count;
					if (flag3)
					{
						result = false;
					}
					else
					{
						Inventory inventory = this._character.GetInventory();
						short materialId = ItemTemplateHelper.GetRawCreateMaterial(oldKey.ItemType, oldKey.TemplateId, newTemplateId);
						int materialCount = (materialId >= 0) ? inventory.GetInventoryItemCount(5, materialId) : 0;
						int requireMaterialCount = SpecialEffect.Instance[effectId].RawCreateRequireMaterialCount;
						bool flag4 = materialId >= 0 && materialCount < requireMaterialCount;
						if (flag4)
						{
							result = false;
						}
						else
						{
							bool flag5 = !this._rawCreateEffects.Remove(effectId);
							if (flag5)
							{
								result = false;
							}
							else
							{
								this.SetRawCreateEffects(this._rawCreateEffects, context);
								bool flag6 = materialId >= 0;
								if (flag6)
								{
									this._character.RemoveMultiInventoryItem(context, 5, materialId, requireMaterialCount);
								}
								ItemKey newKey = DomainManager.Item.CreateItem(context, oldKey.ItemType, newTemplateId);
								newKey = this.CopyEquipmentData(context, oldKey, newKey);
								equipments[(int)equipmentSlot] = newKey;
								this._character.SetEquipment(equipments, context);
								SpecialEffectItem effectConfig = SpecialEffect.Instance[effectId];
								short equipmentEffectId = effectConfig.RawCreateEffect;
								DomainManager.Item.AddExternEquipmentEffect(context, newKey, equipmentEffectId);
								long specialEffectId = DomainManager.SpecialEffect.AddEquipmentEffect(context, this._id, newKey, equipmentEffectId);
								ItemBase newItem = DomainManager.Item.GetBaseItem(newKey);
								DomainManager.Combat.EquipmentOldDurability[newKey] = (int)newItem.GetCurrDurability();
								this._rawCreateCollection.Add(newKey, oldKey, effectId, specialEffectId);
								this.SetRawCreateCollection(this._rawCreateCollection, context);
								DomainManager.Combat.ShowSpecialEffectTips(this._id, (int)effectConfig.RawCreateTips, 0);
								sbyte itemType2 = oldKey.ItemType;
								sbyte b = itemType2;
								if (b != 0)
								{
									if (b != 1)
									{
										result = true;
									}
									else
									{
										for (sbyte bodyPart = 0; bodyPart < 7; bodyPart += 1)
										{
											bool flag7 = EquipmentSlotHelper.GetSlotByBodyPartType(bodyPart) == equipmentSlot;
											if (flag7)
											{
												this.Armors[(int)bodyPart] = newKey;
											}
										}
										result = true;
									}
								}
								else
								{
									this._weapons[(int)equipmentSlot] = newKey;
									this.SetWeapons(this._weapons, context);
									DomainManager.Combat.InitWeaponData(context, this, (int)equipmentSlot);
									CombatWeaponData weaponData = DomainManager.Combat.GetElement_WeaponDataDict(newKey.Id);
									bool flag8 = (int)equipmentSlot == this._usingWeaponIndex;
									if (flag8)
									{
										this.SetWeaponTricks(weaponData.GetWeaponTricks(), context);
									}
									result = true;
								}
							}
						}
					}
				}
			}
			return result;
		}

		// Token: 0x06006075 RID: 24693 RVA: 0x0036DC18 File Offset: 0x0036BE18
		private ItemKey CopyEquipmentData(DataContext context, ItemKey oldKey, ItemKey newKey)
		{
			EquipmentBase oldEquipment = DomainManager.Item.GetBaseEquipment(oldKey);
			EquipmentBase newEquipment = DomainManager.Item.GetBaseEquipment(newKey);
			ItemBase result = newEquipment;
			newEquipment.ApplyDurabilityEquipmentEffectChange(context, (int)newEquipment.GetEquipmentEffectId(), (int)oldEquipment.GetEquipmentEffectId());
			newEquipment.SetEquipmentEffectId(oldEquipment.GetEquipmentEffectId(), context);
			newEquipment.SetCurrDurability(newEquipment.GetMaxDurability(), context);
			bool flag = ModificationStateHelper.IsActive(oldKey.ModificationState, 2);
			if (flag)
			{
				RefiningEffects refiningEffects = DomainManager.Item.GetRefinedEffects(oldKey);
				ItemBase itemBase = DomainManager.Item.GetBaseItem(newKey);
				result = DomainManager.Item.SetRefinedEffects(context, itemBase, refiningEffects);
				newKey = result.GetItemKey();
			}
			bool flag2 = ModificationStateHelper.IsActive(oldKey.ModificationState, 1);
			if (flag2)
			{
				FullPoisonEffects poisonEffects = DomainManager.Item.GetPoisonEffects(oldKey);
				result = DomainManager.Item.SetAttachedPoisons(context, result, poisonEffects);
				newKey = result.GetItemKey();
			}
			return newKey;
		}

		// Token: 0x06006076 RID: 24694 RVA: 0x0036DCF4 File Offset: 0x0036BEF4
		public void RevertRawCreate(DataContext context, ItemKey newKey)
		{
			ItemKey[] equipments = this._character.GetEquipment();
			int equipmentSlot = equipments.IndexOf(newKey);
			Tester.Assert(equipmentSlot >= 0, "equipmentSlot >= 0");
			int effectId = this._rawCreateCollection.Effects[newKey];
			long specialEffectId = this._rawCreateCollection.SpecialEffects[newKey];
			short equipmentEffectId = SpecialEffect.Instance[effectId].RawCreateEffect;
			ItemKey oldKey;
			this._rawCreateCollection.Remove(newKey, out oldKey);
			this.SetRawCreateCollection(this._rawCreateCollection, context);
			equipments[equipmentSlot] = oldKey;
			this._character.SetEquipment(equipments, context);
			DomainManager.Item.RemoveItem(context, newKey);
			DomainManager.Item.RemoveExternEquipmentEffect(context, newKey, equipmentEffectId);
			DomainManager.SpecialEffect.Remove(context, specialEffectId);
			int weaponIndex = this._weapons.IndexOf(newKey);
			bool flag = weaponIndex >= 0;
			if (flag)
			{
				DomainManager.Combat.RemoveWeaponData(newKey);
				this._weapons[weaponIndex] = oldKey;
				CombatWeaponData weaponData = DomainManager.Combat.GetElement_WeaponDataDict(oldKey.Id);
				this.SetWeaponTricks(weaponData.GetWeaponTricks(), context);
				this.SetWeapons(this._weapons, context);
			}
			for (sbyte bodyPart = 0; bodyPart < 7; bodyPart += 1)
			{
				bool flag2 = (int)EquipmentSlotHelper.GetSlotByBodyPartType(bodyPart) == equipmentSlot;
				if (flag2)
				{
					this.Armors[(int)bodyPart] = oldKey;
				}
			}
		}

		// Token: 0x06006077 RID: 24695 RVA: 0x0036DE5C File Offset: 0x0036C05C
		public void RevertAllRawCreates(DataContext context)
		{
			ItemKey[] equipments = this._character.GetEquipment();
			foreach (sbyte slot in GameData.Domains.Combat.SharedConstValue.AllRawCreateSlots)
			{
				bool flag = this._rawCreateCollection.Contains(equipments[(int)slot]);
				if (flag)
				{
					equipments[(int)slot] = this._rawCreateCollection.Sources[equipments[(int)slot]];
				}
			}
			bool flag2 = this._rawCreateCollection.Any();
			if (flag2)
			{
				this._character.SetEquipment(equipments, context);
			}
			foreach (ItemKey key in this._rawCreateCollection.Effects.Keys)
			{
				DomainManager.Item.RemoveItem(context, key);
			}
			this._rawCreateCollection.Clear();
		}

		// Token: 0x06006078 RID: 24696 RVA: 0x0036DF68 File Offset: 0x0036C168
		public void CreateGangqi(DataContext context, int value)
		{
			bool flag = value <= this._gangqiMax;
			if (!flag)
			{
				this.SetGangqi(value, context);
				this.SetGangqiMax(value, context);
			}
		}

		// Token: 0x06006079 RID: 24697 RVA: 0x0036DF9C File Offset: 0x0036C19C
		public void ChangeGangqi(DataContext context, int delta)
		{
			int newGangqi = Math.Clamp(this._gangqi + delta, 0, this._gangqiMax);
			this.SetGangqi(newGangqi, context);
		}

		// Token: 0x0600607A RID: 24698 RVA: 0x0036DFC8 File Offset: 0x0036C1C8
		public bool CanExecuteTeammateCommandImmediate(ETeammateCommandImplement implement)
		{
			bool pause = DomainManager.Combat.Pause;
			bool result;
			if (pause)
			{
				result = false;
			}
			else
			{
				ECombatReserveType type = this._combatReserveData.Type;
				bool flag = type == ECombatReserveType.Invalid || type == ECombatReserveType.TeammateCommand;
				bool flag2 = !flag;
				if (flag2)
				{
					result = false;
				}
				else
				{
					CombatReserveData prevReserveData = this._combatReserveData;
					this._combatReserveData = CombatReserveData.Invalid;
					bool hasDoingOrReserve = this.HasDoingOrReserveCommand();
					this._combatReserveData = prevReserveData;
					bool flag3 = !hasDoingOrReserve;
					if (flag3)
					{
						result = true;
					}
					else
					{
						bool flag4 = this.StateMachine.GetCurrentStateType() == CombatCharacterStateType.TeammateCommand;
						if (flag4)
						{
							result = true;
						}
						else
						{
							bool flag5 = this._preparingSkillId >= 0;
							bool flag6 = flag5;
							if (flag6)
							{
								flag = (implement == ETeammateCommandImplement.AccelerateCast || implement == ETeammateCommandImplement.InterruptSkill);
								flag6 = flag;
							}
							bool flag7 = flag6;
							if (flag7)
							{
								result = true;
							}
							else
							{
								bool flag8 = (this._preparingOtherAction >= 0 || this._preparingItem.IsValid()) && implement == ETeammateCommandImplement.InterruptOtherAction;
								result = flag8;
							}
						}
					}
				}
			}
			return result;
		}

		// Token: 0x0600607B RID: 24699 RVA: 0x0036E0D4 File Offset: 0x0036C2D4
		public bool CanExecuteReserveTeammateCommand()
		{
			bool flag = this._combatReserveData.Type != ECombatReserveType.TeammateCommand;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				CombatCharacter teammate = DomainManager.Combat.GetElement_CombatCharacterDict(this._combatReserveData.TeammateCharId);
				sbyte cmdType = this._showTransferInjuryCommand ? 13 : teammate.GetCurrTeammateCommands()[this._combatReserveData.TeammateCmdIndex];
				result = this.CanExecuteTeammateCommandImmediate(TeammateCommand.Instance[cmdType].Implement);
			}
			return result;
		}

		// Token: 0x0600607C RID: 24700 RVA: 0x0036E150 File Offset: 0x0036C350
		public bool ExecuteTeammateCommandImmediate(DataContext context, int charId, int index)
		{
			CombatCharacter teammateChar = DomainManager.Combat.GetElement_CombatCharacterDict(charId);
			bool flag = teammateChar.IsAlly != this.IsAlly || !DomainManager.Combat.IsMainCharacter(this);
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				sbyte cmdType = this._showTransferInjuryCommand ? 13 : teammateChar.GetCurrTeammateCommands()[index];
				TeammateCommandItem config = TeammateCommand.Instance[cmdType];
				ETeammateCommandImplement implement = config.Implement;
				teammateChar.ExecutingTeammateCommandConfig = config;
				bool flag2 = implement == ETeammateCommandImplement.Fight;
				if (flag2)
				{
					this.ChangeCharId = charId;
					teammateChar.ExecutingTeammateCommandIndex = index;
					DomainManager.Combat.ClearAllWeaponCd(context, teammateChar);
				}
				else
				{
					bool flag3 = implement == ETeammateCommandImplement.AttackSkill;
					if (flag3)
					{
						this.ChangeCharId = charId;
						teammateChar.ExecutingTeammateCommandIndex = index;
						DomainManager.Combat.CastSkillFree(context, teammateChar, teammateChar.GetAttackCommandSkillId(), ECombatCastFreePriority.Normal);
						int weapon = teammateChar.AiController.GetBestWeaponIndex(context.Random, teammateChar.NeedUseSkillFreeId, false, -1);
						bool flag4 = teammateChar.GetUsingWeaponIndex() != weapon;
						if (flag4)
						{
							DomainManager.Combat.ChangeWeapon(context, teammateChar, weapon, false, false);
						}
						OuterAndInnerShorts attackRange = teammateChar.GetAttackRange();
						short bestDistance = (attackRange.Outer + attackRange.Inner) / 2;
						int moveDistance = (int)(bestDistance - DomainManager.Combat.GetCurrentDistance());
						bool flag5 = DomainManager.Combat.ChangeDistance(context, this, moveDistance);
						if (flag5)
						{
							teammateChar.ExecutingTeammateCommandChangeDistance = -moveDistance;
						}
						DomainManager.Combat.ClearAllWeaponCd(context, teammateChar);
					}
					else
					{
						teammateChar.SetExecutingTeammateCommand(cmdType, context);
						teammateChar.TeammateCommandLeftPrepareFrame = (teammateChar.TeammateCommandTotalPrepareFrame = teammateChar.ExecutingTeammateCommandConfig.PrepareFrame);
						this.TeammateHasCommand[DomainManager.Combat.GetCharacterList(this.IsAlly).IndexOf(charId) - 1] = true;
						bool intoCombatField = teammateChar.ExecutingTeammateCommandConfig.IntoCombatField;
						if (intoCombatField)
						{
							bool flag6 = implement == ETeammateCommandImplement.HealInjury;
							if (flag6)
							{
								teammateChar.TeammateCommandLeftPrepareFrame = (teammateChar.TeammateCommandTotalPrepareFrame = teammateChar.GetOtherActionPrepareFrame(0));
							}
							else
							{
								bool flag7 = implement == ETeammateCommandImplement.HealPoison;
								if (flag7)
								{
									teammateChar.TeammateCommandLeftPrepareFrame = (teammateChar.TeammateCommandTotalPrepareFrame = teammateChar.GetOtherActionPrepareFrame(1));
								}
								else
								{
									bool flag8 = implement == ETeammateCommandImplement.Attack;
									if (flag8)
									{
										ItemKey attackWeapon = teammateChar.GetAttackCommandWeaponKey();
										int attackWeaponIndex = teammateChar.GetWeapons().IndexOf(attackWeapon);
										bool flag9 = teammateChar.GetUsingWeaponIndex() != attackWeaponIndex;
										if (flag9)
										{
											DomainManager.Combat.ChangeWeapon(context, teammateChar, attackWeaponIndex, false, false);
										}
									}
								}
							}
						}
						bool flag10 = teammateChar.ExecutingTeammateCommandConfig.AffectFrame >= 0;
						if (flag10)
						{
							teammateChar.ExecutingTeammateCommandIndex = index;
						}
						else
						{
							bool flag11 = teammateChar.CheckResetTeammateCommandCd(config);
							if (flag11)
							{
								teammateChar.ResetTeammateCommandCd(context, index, -1, true, false);
							}
						}
					}
				}
				teammateChar.SetShowEffectCommandIndex((sbyte)index, context);
				bool flag12 = implement == ETeammateCommandImplement.Fight || implement - ETeammateCommandImplement.Push <= 1;
				bool flag13 = flag12;
				if (flag13)
				{
					this.MoveData.ResetJumpState(context, true);
				}
				bool changeState = this.UpdateTeammateCharStatus(context);
				DomainManager.Combat.UpdateAllCommandAvailability(context, this);
				result = changeState;
			}
			return result;
		}

		// Token: 0x0600607D RID: 24701 RVA: 0x0036E440 File Offset: 0x0036C640
		public bool ExecuteReserveTeammateCommand(DataContext context)
		{
			bool flag = !this.CanExecuteReserveTeammateCommand();
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				int charId = this._combatReserveData.TeammateCharId;
				int index = this._combatReserveData.TeammateCmdIndex;
				this.SetCombatReserveData(CombatReserveData.Invalid, context);
				CombatCharacter teammate = DomainManager.Combat.GetElement_CombatCharacterDict(charId);
				bool flag2 = !teammate.GetTeammateCommandCanUse()[index];
				result = (!flag2 && this.ExecuteTeammateCommandImmediate(context, charId, index));
			}
			return result;
		}

		// Token: 0x0600607E RID: 24702 RVA: 0x0036E4BC File Offset: 0x0036C6BC
		private bool CheckResetTeammateCommandCd(TeammateCommandItem commandConfig)
		{
			bool flag = commandConfig.Type != ETeammateCommandType.Advance;
			bool result;
			if (flag)
			{
				result = true;
			}
			else
			{
				ETeammateCommandImplement implement = commandConfig.Implement;
				ETeammateCommandImplement eteammateCommandImplement = implement;
				if (eteammateCommandImplement != ETeammateCommandImplement.Push)
				{
					if (eteammateCommandImplement != ETeammateCommandImplement.Pull)
					{
						result = true;
					}
					else if (!this.NeedResetAdvanceTeammateCommandPullCd)
					{
						this.NeedResetAdvanceTeammateCommandPullCd = true;
						result = false;
					}
					else
					{
						this.NeedResetAdvanceTeammateCommandPullCd = false;
						result = true;
					}
				}
				else if (!this.NeedResetAdvanceTeammateCommandPushCd)
				{
					this.NeedResetAdvanceTeammateCommandPushCd = true;
					result = false;
				}
				else
				{
					this.NeedResetAdvanceTeammateCommandPushCd = false;
					result = true;
				}
			}
			return result;
		}

		// Token: 0x17000373 RID: 883
		// (get) Token: 0x0600607F RID: 24703 RVA: 0x0036E538 File Offset: 0x0036C738
		public bool AnyUsableTrick
		{
			get
			{
				return this._tricks.Tricks.Values.Any(new Func<sbyte, bool>(this.IsTrickUsable));
			}
		}

		// Token: 0x17000374 RID: 884
		// (get) Token: 0x06006080 RID: 24704 RVA: 0x0036E55B File Offset: 0x0036C75B
		public int UsableTrickCount
		{
			get
			{
				return this._tricks.Tricks.Values.Count(new Func<sbyte, bool>(this.IsTrickUsable));
			}
		}

		// Token: 0x17000375 RID: 885
		// (get) Token: 0x06006081 RID: 24705 RVA: 0x0036E57E File Offset: 0x0036C77E
		public int UselessTrickCount
		{
			get
			{
				return this._tricks.Tricks.Values.Count(new Func<sbyte, bool>(this.IsTrickUseless));
			}
		}

		// Token: 0x06006082 RID: 24706 RVA: 0x0036E5A4 File Offset: 0x0036C7A4
		public bool TrickEquals(sbyte trick1, sbyte trick2)
		{
			bool flag = trick1 == trick2;
			return flag || (this.InterchangeableTricks.Contains(trick1) && this.InterchangeableTricks.Contains(trick2) && this._weaponTricks.Exist(new Predicate<sbyte>(this.InterchangeableTricks.Contains)));
		}

		// Token: 0x06006083 RID: 24707 RVA: 0x0036E600 File Offset: 0x0036C800
		public bool IsTrickUsable(sbyte trickType)
		{
			bool flag = trickType == 21;
			bool result;
			if (flag)
			{
				result = true;
			}
			else
			{
				sbyte[] weaponTricks = this._combatDomain.GetUsingWeaponData(this).GetWeaponTricks();
				bool flag2 = weaponTricks.Contains(trickType);
				result = (flag2 || (this.InterchangeableTricks.Contains(trickType) && this.InterchangeableTricks.Any(new Func<sbyte, bool>(weaponTricks.Contains<sbyte>))));
			}
			return result;
		}

		// Token: 0x06006084 RID: 24708 RVA: 0x0036E668 File Offset: 0x0036C868
		public bool IsTrickUseless(sbyte trickType)
		{
			return !this.IsTrickUsable(trickType);
		}

		// Token: 0x06006085 RID: 24709 RVA: 0x0036E674 File Offset: 0x0036C874
		public int ReplaceUsableTrick(DataContext context, sbyte trickType, int count = -1)
		{
			bool flag = count == 0;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				IReadOnlyDictionary<int, sbyte> tricks = this._tricks.Tricks;
				List<int> indexList = ObjectPool<List<int>>.Instance.Get();
				indexList.Clear();
				indexList.AddRange(tricks.Where(delegate(KeyValuePair<int, sbyte> kvp)
				{
					KeyValuePair<int, sbyte> keyValuePair = kvp;
					bool result2;
					if (keyValuePair.Value != trickType)
					{
						CombatCharacter <>4__this = this;
						keyValuePair = kvp;
						result2 = <>4__this.IsTrickUsable(keyValuePair.Value);
					}
					else
					{
						result2 = false;
					}
					return result2;
				}).Select(delegate(KeyValuePair<int, sbyte> kvp)
				{
					KeyValuePair<int, sbyte> keyValuePair = kvp;
					return keyValuePair.Key;
				}));
				int reserveCount = (count > 0) ? (indexList.Count - Math.Min(count, indexList.Count)) : 0;
				for (int i = 0; i < reserveCount; i++)
				{
					CollectionUtils.SwapAndRemove<int>(indexList, context.Random.Next(indexList.Count));
				}
				foreach (int index in indexList)
				{
					this._tricks.ReplaceTrick(index, trickType);
				}
				int replacedCount = indexList.Count;
				ObjectPool<List<int>>.Instance.Return(indexList);
				this.SetTricks(this._tricks, context);
				result = replacedCount;
			}
			return result;
		}

		// Token: 0x06006086 RID: 24710 RVA: 0x0036E7C8 File Offset: 0x0036C9C8
		public byte GetTrickCount(sbyte type)
		{
			return this.GetTrickCount(type, false);
		}

		// Token: 0x06006087 RID: 24711 RVA: 0x0036E7E4 File Offset: 0x0036C9E4
		public byte GetTrickCount(sbyte type, bool useTrickEquals)
		{
			byte trickCounter = 0;
			foreach (sbyte trickType in this._tricks.Tricks.Values)
			{
				bool flag = useTrickEquals ? this.TrickEquals(trickType, type) : (trickType == type);
				if (flag)
				{
					trickCounter += 1;
				}
			}
			return trickCounter;
		}

		// Token: 0x06006088 RID: 24712 RVA: 0x0036E85C File Offset: 0x0036CA5C
		public sbyte GetTrickAtStart()
		{
			using (IEnumerator<sbyte> enumerator = this._tricks.Tricks.Values.GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					return enumerator.Current;
				}
			}
			return -1;
		}

		// Token: 0x06006089 RID: 24713 RVA: 0x0036E8B8 File Offset: 0x0036CAB8
		public int GetContinueTricksAtStart(sbyte trickType)
		{
			int count = 0;
			foreach (sbyte trick in this._tricks.Tricks.Values)
			{
				bool flag = trick == trickType;
				if (!flag)
				{
					break;
				}
				count++;
			}
			return (count > 1) ? count : 0;
		}

		// Token: 0x0600608A RID: 24714 RVA: 0x0036E930 File Offset: 0x0036CB30
		public int GetContinueTricks(sbyte trickType, List<int> indexList = null)
		{
			IReadOnlyDictionary<int, sbyte> trickDict = this._tricks.Tricks;
			int maxContinueCount = 0;
			int curContinueCount = 0;
			List<int> checkingContinue = ObjectPool<List<int>>.Instance.Get();
			checkingContinue.Clear();
			foreach (KeyValuePair<int, sbyte> keyValuePair in trickDict)
			{
				int num;
				sbyte b;
				keyValuePair.Deconstruct(out num, out b);
				int index = num;
				sbyte trick = b;
				bool flag = trick == trickType;
				if (flag)
				{
					curContinueCount++;
					checkingContinue.Add(index);
					bool flag2 = curContinueCount < 2 || curContinueCount <= maxContinueCount;
					if (!flag2)
					{
						maxContinueCount = curContinueCount;
						if (indexList != null)
						{
							indexList.Clear();
						}
						if (indexList != null)
						{
							indexList.AddRange(checkingContinue);
						}
					}
				}
				else
				{
					curContinueCount = 0;
					checkingContinue.Clear();
				}
			}
			ObjectPool<List<int>>.Instance.Return(checkingContinue);
			return maxContinueCount;
		}

		// Token: 0x0600608B RID: 24715 RVA: 0x0036EA20 File Offset: 0x0036CC20
		public void InvokeExtraUnlockEffect(IExtraUnlockEffect effect, int weaponIndex)
		{
			bool flag = this.CanInvokeExtraUnlockEffect(effect, weaponIndex);
			if (flag)
			{
				this._invokedUnlockEffects.Add(effect);
			}
		}

		// Token: 0x0600608C RID: 24716 RVA: 0x0036EA48 File Offset: 0x0036CC48
		public void DoExtraUnlockEffect(DataContext context, int weaponIndex)
		{
			CollectionUtils.Shuffle<IExtraUnlockEffect>(context.Random, this._invokedUnlockEffects);
			foreach (IExtraUnlockEffect effect in this._invokedUnlockEffects)
			{
				bool flag = this.DoExtraUnlockEffectCost(context, effect, weaponIndex);
				if (flag)
				{
					this._costedUnlockEffects.Add(effect);
				}
			}
			this._invokedUnlockEffects.Clear();
			foreach (IExtraUnlockEffect effect2 in this._costedUnlockEffects)
			{
				effect2.DoAffectAfterCost(context, weaponIndex);
			}
			this._costedUnlockEffects.Clear();
		}

		// Token: 0x0600608D RID: 24717 RVA: 0x0036EB24 File Offset: 0x0036CD24
		private bool CanInvokeExtraUnlockEffect(IExtraUnlockEffect effect, int weaponIndex)
		{
			ItemKey key = this.GetWeapons()[weaponIndex];
			bool flag = !key.IsValid();
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				short durability = DomainManager.Item.GetBaseItem(key).GetCurrDurability();
				int trickCount = effect.IsDirect ? ((int)this.GetTrickCount(12)) : this.UsableTrickCount;
				trickCount += this.TryInsteadTrick(effect, null);
				bool isDirect = effect.IsDirect;
				if (isDirect)
				{
					result = (trickCount >= 2 || durability > 4);
				}
				else
				{
					result = (trickCount >= 3 && durability > 8);
				}
			}
			return result;
		}

		// Token: 0x0600608E RID: 24718 RVA: 0x0036EBB4 File Offset: 0x0036CDB4
		private int TryInsteadTrick(IExtraUnlockEffect effect, DataContext context = null)
		{
			bool flag = !effect.IsDirect && this.IsTrickUsable(12);
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				int maxInsteadCount = DomainManager.SpecialEffect.ModifyData(this._id, -1, effect.IsDirect ? 314 : 313, 0, -1, -1, -1);
				int canInsteadCount = effect.IsDirect ? this.UselessTrickCount : ((int)this.GetTrickCount(12));
				byte requireInsteadCount = effect.IsDirect ? 2 : 3;
				int insteadCount = Math.Min(Math.Min(maxInsteadCount, canInsteadCount), (int)requireInsteadCount);
				bool flag2 = context == null;
				if (flag2)
				{
					result = insteadCount;
				}
				else
				{
					bool isDirect = effect.IsDirect;
					if (isDirect)
					{
						List<sbyte> uselessTricks = ObjectPool<List<sbyte>>.Instance.Get();
						uselessTricks.AddRange(this._tricks.Tricks.Values.Where(new Func<sbyte, bool>(this.IsTrickUseless)));
						IEnumerable<sbyte> costTricks = RandomUtils.GetRandomUnrepeated<sbyte>(context.Random, insteadCount, uselessTricks, null);
						this._combatDomain.RemoveTrick(context, this, costTricks, true);
						ObjectPool<List<sbyte>>.Instance.Return(uselessTricks);
						Events.RaiseUselessTrickInsteadJiTricks(context, this, insteadCount);
					}
					else
					{
						this._combatDomain.RemoveTrick(context, this, 12, (byte)insteadCount, true, -1);
						Events.RaiseJiTrickInsteadCostTricks(context, this, insteadCount);
					}
					result = insteadCount;
				}
			}
			return result;
		}

		// Token: 0x0600608F RID: 24719 RVA: 0x0036ECF8 File Offset: 0x0036CEF8
		private bool DoExtraUnlockEffectCost(DataContext context, IExtraUnlockEffect effect, int weaponIndex)
		{
			bool flag = !this.CanInvokeExtraUnlockEffect(effect, weaponIndex);
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				bool useJiTrick = effect.IsDirect && (int)this.GetTrickCount(12) + this.TryInsteadTrick(effect, null) >= 2;
				bool useDurability = !useJiTrick || !effect.IsDirect;
				bool flag2 = useJiTrick;
				if (flag2)
				{
					int costTrickCount = 2 - this.TryInsteadTrick(effect, context);
					this._combatDomain.RemoveTrick(context, this, 12, (byte)costTrickCount, true, -1);
				}
				bool flag3 = !effect.IsDirect;
				if (flag3)
				{
					int costTrickCount2 = 3 - this.TryInsteadTrick(effect, context);
					List<sbyte> usableTricks = ObjectPool<List<sbyte>>.Instance.Get();
					usableTricks.AddRange(this._tricks.Tricks.Values.Where(new Func<sbyte, bool>(this.IsTrickUsable)));
					IEnumerable<sbyte> costTricks = RandomUtils.GetRandomUnrepeated<sbyte>(context.Random, costTrickCount2, usableTricks, null);
					this._combatDomain.RemoveTrick(context, this, costTricks, true);
					ObjectPool<List<sbyte>>.Instance.Return(usableTricks);
				}
				ItemKey weaponKey = this._weapons[weaponIndex];
				int durability = effect.IsDirect ? 4 : 8;
				bool flag4 = useDurability;
				if (flag4)
				{
					this._combatDomain.ChangeDurability(context, this, weaponKey, -durability, EChangeDurabilitySourceType.Unlock);
				}
				result = true;
			}
			return result;
		}

		// Token: 0x06006090 RID: 24720 RVA: 0x0036EE38 File Offset: 0x0036D038
		public int GetRecoverUnlockAttackValue(ItemKey weaponKey)
		{
			int value = 0;
			foreach (short skillId in this.GetCombatSkillIds())
			{
				GameData.Domains.CombatSkill.CombatSkill skill;
				bool flag = !DomainManager.CombatSkill.TryGetElement_CombatSkills(new ValueTuple<int, short>(this._id, skillId), out skill);
				if (!flag)
				{
					SpecialEffectItem effectConfig = skill.TryGetSpecialEffect();
					bool flag2 = effectConfig == null;
					if (!flag2)
					{
						short itemSubType = ItemTemplateHelper.GetItemSubType(weaponKey.ItemType, weaponKey.TemplateId);
						bool flag3 = itemSubType != effectConfig.AddUnlockValueItemSubType;
						if (!flag3)
						{
							value = Math.Max(value, effectConfig.AddUnlockValue);
						}
					}
				}
			}
			return value;
		}

		// Token: 0x06006091 RID: 24721 RVA: 0x0036EF04 File Offset: 0x0036D104
		public bool WorsenInjury(DataContext context, sbyte bodyPart, bool inner)
		{
			return this.WorsenInjury(context, bodyPart, inner, WorsenConstants.DefaultPercent);
		}

		// Token: 0x06006092 RID: 24722 RVA: 0x0036EF24 File Offset: 0x0036D124
		public bool WorsenInjury(DataContext context, sbyte bodyPart, bool inner, CValuePercent percent)
		{
			bool flag = bodyPart < 0 || bodyPart >= 7;
			bool flag2 = flag;
			bool result;
			if (flag2)
			{
				result = false;
			}
			else
			{
				sbyte injuryCount = this._injuries.Get(bodyPart, inner);
				bool flag3 = injuryCount <= 0;
				if (flag3)
				{
					result = false;
				}
				else
				{
					int step = (inner ? this._damageStepCollection.InnerDamageSteps : this._damageStepCollection.OuterDamageSteps)[(int)bodyPart];
					int addFatalDamage = step * WorsenConstants.WorsenFatalPercent[(int)(injuryCount - 1)] * percent;
					bool flag4 = addFatalDamage > 0;
					if (flag4)
					{
						this._combatDomain.AddFatalDamageValue(context, this, addFatalDamage, inner ? 1 : 0, bodyPart, -1, EDamageType.None);
					}
					result = (addFatalDamage > 0);
				}
			}
			return result;
		}

		// Token: 0x06006093 RID: 24723 RVA: 0x0036EFD8 File Offset: 0x0036D1D8
		public bool WorsenRandomInjury(DataContext context)
		{
			return this.WorsenRandomInjury(context, WorsenConstants.DefaultPercent);
		}

		// Token: 0x06006094 RID: 24724 RVA: 0x0036EFF8 File Offset: 0x0036D1F8
		public bool WorsenRandomInjury(DataContext context, CValuePercent percent)
		{
			return this.WorsenRandomInjury(context, this.RandomWorsenIsInner(context.Random), percent);
		}

		// Token: 0x06006095 RID: 24725 RVA: 0x0036F020 File Offset: 0x0036D220
		public bool WorsenRandomInjury(DataContext context, sbyte bodyPart)
		{
			return this.WorsenRandomInjury(context, bodyPart, WorsenConstants.DefaultPercent);
		}

		// Token: 0x06006096 RID: 24726 RVA: 0x0036F040 File Offset: 0x0036D240
		public bool WorsenRandomInjury(DataContext context, sbyte bodyPart, CValuePercent percent)
		{
			return this.WorsenRandomInjury(context, this.RandomWorsenIsInner(context.Random, bodyPart), percent);
		}

		// Token: 0x06006097 RID: 24727 RVA: 0x0036F068 File Offset: 0x0036D268
		public bool WorsenRandomInjury(DataContext context, bool inner)
		{
			return this.WorsenRandomInjury(context, inner, WorsenConstants.DefaultPercent);
		}

		// Token: 0x06006098 RID: 24728 RVA: 0x0036F088 File Offset: 0x0036D288
		public bool WorsenRandomInjury(DataContext context, bool inner, CValuePercent percent)
		{
			sbyte bodyPart = this.RandomWorsenBodyPart(context.Random, inner);
			return bodyPart >= 0 && this.WorsenInjury(context, bodyPart, inner, percent);
		}

		// Token: 0x06006099 RID: 24729 RVA: 0x0036F0B9 File Offset: 0x0036D2B9
		public void WorsenRepeatableInjury(DataContext context, int count)
		{
			this.WorsenRepeatableInjury(context, count, WorsenConstants.DefaultPercent);
		}

		// Token: 0x0600609A RID: 24730 RVA: 0x0036F0CC File Offset: 0x0036D2CC
		public void WorsenRepeatableInjury(DataContext context, int count, CValuePercent percent)
		{
			for (int i = 0; i < count; i++)
			{
				this.WorsenRandomInjury(context, percent);
			}
		}

		// Token: 0x0600609B RID: 24731 RVA: 0x0036F0F3 File Offset: 0x0036D2F3
		public void WorsenRepeatableInjury(DataContext context, bool inner, int count)
		{
			this.WorsenRepeatableInjury(context, inner, count, WorsenConstants.DefaultPercent);
		}

		// Token: 0x0600609C RID: 24732 RVA: 0x0036F108 File Offset: 0x0036D308
		public void WorsenRepeatableInjury(DataContext context, bool inner, int count, CValuePercent percent)
		{
			for (int i = 0; i < count; i++)
			{
				this.WorsenRandomInjury(context, inner, percent);
			}
		}

		// Token: 0x0600609D RID: 24733 RVA: 0x0036F131 File Offset: 0x0036D331
		public void WorsenUnrepeatedInjury(DataContext context, int count)
		{
			this.WorsenUnrepeatedInjury(context, count, WorsenConstants.DefaultPercent);
		}

		// Token: 0x0600609E RID: 24734 RVA: 0x0036F144 File Offset: 0x0036D344
		public void WorsenUnrepeatedInjury(DataContext context, int count, CValuePercent percent)
		{
			List<sbyte> innerBodyParts = ObjectPool<List<sbyte>>.Instance.Get();
			List<sbyte> outerBodyParts = ObjectPool<List<sbyte>>.Instance.Get();
			for (sbyte i = 0; i < 7; i += 1)
			{
				bool flag = this._injuries.Get(i, true) > 0;
				if (flag)
				{
					innerBodyParts.Add(i);
				}
				bool flag2 = this._injuries.Get(i, false) > 0;
				if (flag2)
				{
					outerBodyParts.Add(i);
				}
			}
			for (int j = 0; j < count; j++)
			{
				bool anyInner = innerBodyParts.Count > 0;
				bool anyOuter = outerBodyParts.Count > 0;
				bool flag3 = !anyInner && !anyOuter;
				if (flag3)
				{
					break;
				}
				bool inner = context.Random.RandomIsInner(anyInner, anyOuter);
				sbyte bodyPart = (inner ? innerBodyParts : outerBodyParts).GetRandom(context.Random);
				bool flag4 = inner;
				if (flag4)
				{
					innerBodyParts.Remove(bodyPart);
				}
				else
				{
					outerBodyParts.Remove(bodyPart);
				}
				this.WorsenInjury(context, bodyPart, inner, percent);
			}
			ObjectPool<List<sbyte>>.Instance.Return(innerBodyParts);
			ObjectPool<List<sbyte>>.Instance.Return(outerBodyParts);
		}

		// Token: 0x0600609F RID: 24735 RVA: 0x0036F266 File Offset: 0x0036D466
		public void WorsenUnrepeatedInjury(DataContext context, bool inner, int count)
		{
			this.WorsenUnrepeatedInjury(context, inner, count, WorsenConstants.DefaultPercent);
		}

		// Token: 0x060060A0 RID: 24736 RVA: 0x0036F278 File Offset: 0x0036D478
		public void WorsenUnrepeatedInjury(DataContext context, bool inner, int count, CValuePercent percent)
		{
			List<sbyte> pool = ObjectPool<List<sbyte>>.Instance.Get();
			for (sbyte i = 0; i < 7; i += 1)
			{
				bool flag = this._injuries.Get(i, inner) > 0;
				if (flag)
				{
					pool.Add(i);
				}
			}
			foreach (sbyte bodyPart in RandomUtils.GetRandomUnrepeated<sbyte>(context.Random, count, pool, null))
			{
				this.WorsenInjury(context, bodyPart, inner, percent);
			}
			ObjectPool<List<sbyte>>.Instance.Return(pool);
		}

		// Token: 0x060060A1 RID: 24737 RVA: 0x0036F320 File Offset: 0x0036D520
		public bool WorsenAllInjury(DataContext context)
		{
			return this.WorsenAllInjury(context, WorsenConstants.DefaultPercent);
		}

		// Token: 0x060060A2 RID: 24738 RVA: 0x0036F340 File Offset: 0x0036D540
		public bool WorsenAllInjury(DataContext context, CValuePercent percent)
		{
			bool anyWorsen = this.WorsenAllInjury(context, true, percent);
			return this.WorsenAllInjury(context, false, percent) || anyWorsen;
		}

		// Token: 0x060060A3 RID: 24739 RVA: 0x0036F368 File Offset: 0x0036D568
		public bool WorsenAllInjury(DataContext context, bool inner)
		{
			return this.WorsenAllInjury(context, inner, WorsenConstants.DefaultPercent);
		}

		// Token: 0x060060A4 RID: 24740 RVA: 0x0036F388 File Offset: 0x0036D588
		public bool WorsenAllInjury(DataContext context, bool inner, CValuePercent percent)
		{
			bool anyWorsen = false;
			for (sbyte i = 0; i < 7; i += 1)
			{
				bool flag = this._injuries.Get(i, inner) > 0;
				if (flag)
				{
					anyWorsen = (this.WorsenInjury(context, i, inner, percent) || anyWorsen);
				}
			}
			return anyWorsen;
		}

		// Token: 0x060060A5 RID: 24741 RVA: 0x0036F3D4 File Offset: 0x0036D5D4
		private bool RandomWorsenIsInner(IRandomSource random)
		{
			bool anyInner = this._injuries.HasAnyInjury(true);
			bool anyOuter = this._injuries.HasAnyInjury(false);
			return random.RandomIsInner(anyInner, anyOuter);
		}

		// Token: 0x060060A6 RID: 24742 RVA: 0x0036F408 File Offset: 0x0036D608
		private bool RandomWorsenIsInner(IRandomSource random, sbyte bodyPart)
		{
			bool flag = bodyPart < 0 || bodyPart >= 7;
			bool flag2 = flag;
			bool result;
			if (flag2)
			{
				result = this.RandomWorsenIsInner(random);
			}
			else
			{
				bool anyInner = this._injuries.Get(bodyPart, true) > 0;
				bool anyOuter = this._injuries.Get(bodyPart, false) > 0;
				result = random.RandomIsInner(anyInner, anyOuter);
			}
			return result;
		}

		// Token: 0x060060A7 RID: 24743 RVA: 0x0036F468 File Offset: 0x0036D668
		private sbyte RandomWorsenBodyPart(IRandomSource random, bool inner)
		{
			List<sbyte> pool = ObjectPool<List<sbyte>>.Instance.Get();
			for (sbyte i = 0; i < 7; i += 1)
			{
				bool flag = this._injuries.Get(i, inner) > 0;
				if (flag)
				{
					pool.Add(i);
				}
			}
			sbyte bodyPart = (pool.Count > 0) ? pool.GetRandom(random) : -1;
			ObjectPool<List<sbyte>>.Instance.Return(pool);
			return bodyPart;
		}

		// Token: 0x060060A8 RID: 24744 RVA: 0x0036F4D8 File Offset: 0x0036D6D8
		public int GetId()
		{
			return this._id;
		}

		// Token: 0x060060A9 RID: 24745 RVA: 0x0036F4F0 File Offset: 0x0036D6F0
		public int GetBreathValue()
		{
			return this._breathValue;
		}

		// Token: 0x060060AA RID: 24746 RVA: 0x0036F508 File Offset: 0x0036D708
		public unsafe void SetBreathValue(int breathValue, DataContext context)
		{
			this._breathValue = breathValue;
			base.SetModifiedAndInvalidateInfluencedCache(1, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				byte* pData = OperationAdder.DynamicObjectCollection_SetFixedField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 4U, 4);
				*(int*)pData = this._breathValue;
				pData += 4;
			}
		}

		// Token: 0x060060AB RID: 24747 RVA: 0x0036F568 File Offset: 0x0036D768
		public int GetStanceValue()
		{
			return this._stanceValue;
		}

		// Token: 0x060060AC RID: 24748 RVA: 0x0036F580 File Offset: 0x0036D780
		public unsafe void SetStanceValue(int stanceValue, DataContext context)
		{
			this._stanceValue = stanceValue;
			base.SetModifiedAndInvalidateInfluencedCache(2, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				byte* pData = OperationAdder.DynamicObjectCollection_SetFixedField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 8U, 4);
				*(int*)pData = this._stanceValue;
				pData += 4;
			}
		}

		// Token: 0x060060AD RID: 24749 RVA: 0x0036F5E0 File Offset: 0x0036D7E0
		public NeiliAllocation GetNeiliAllocation()
		{
			return this._neiliAllocation;
		}

		// Token: 0x060060AE RID: 24750 RVA: 0x0036F5F8 File Offset: 0x0036D7F8
		public unsafe void SetNeiliAllocation(NeiliAllocation neiliAllocation, DataContext context)
		{
			this._neiliAllocation = neiliAllocation;
			base.SetModifiedAndInvalidateInfluencedCache(3, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				byte* pData = OperationAdder.DynamicObjectCollection_SetFixedField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 12U, 8);
				pData += this._neiliAllocation.Serialize(pData);
			}
		}

		// Token: 0x060060AF RID: 24751 RVA: 0x0036F65C File Offset: 0x0036D85C
		public NeiliAllocation GetOriginNeiliAllocation()
		{
			return this._originNeiliAllocation;
		}

		// Token: 0x060060B0 RID: 24752 RVA: 0x0036F674 File Offset: 0x0036D874
		public unsafe void SetOriginNeiliAllocation(NeiliAllocation originNeiliAllocation, DataContext context)
		{
			this._originNeiliAllocation = originNeiliAllocation;
			base.SetModifiedAndInvalidateInfluencedCache(4, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				byte* pData = OperationAdder.DynamicObjectCollection_SetFixedField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 20U, 8);
				pData += this._originNeiliAllocation.Serialize(pData);
			}
		}

		// Token: 0x060060B1 RID: 24753 RVA: 0x0036F6D8 File Offset: 0x0036D8D8
		public NeiliAllocation GetNeiliAllocationRecoverProgress()
		{
			return this._neiliAllocationRecoverProgress;
		}

		// Token: 0x060060B2 RID: 24754 RVA: 0x0036F6F0 File Offset: 0x0036D8F0
		public unsafe void SetNeiliAllocationRecoverProgress(NeiliAllocation neiliAllocationRecoverProgress, DataContext context)
		{
			this._neiliAllocationRecoverProgress = neiliAllocationRecoverProgress;
			base.SetModifiedAndInvalidateInfluencedCache(5, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				byte* pData = OperationAdder.DynamicObjectCollection_SetFixedField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 28U, 8);
				pData += this._neiliAllocationRecoverProgress.Serialize(pData);
			}
		}

		// Token: 0x060060B3 RID: 24755 RVA: 0x0036F754 File Offset: 0x0036D954
		public short GetOldDisorderOfQi()
		{
			return this._oldDisorderOfQi;
		}

		// Token: 0x060060B4 RID: 24756 RVA: 0x0036F76C File Offset: 0x0036D96C
		public unsafe void SetOldDisorderOfQi(short oldDisorderOfQi, DataContext context)
		{
			this._oldDisorderOfQi = oldDisorderOfQi;
			base.SetModifiedAndInvalidateInfluencedCache(6, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				byte* pData = OperationAdder.DynamicObjectCollection_SetFixedField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 36U, 2);
				*(short*)pData = this._oldDisorderOfQi;
				pData += 2;
			}
		}

		// Token: 0x060060B5 RID: 24757 RVA: 0x0036F7CC File Offset: 0x0036D9CC
		public sbyte GetNeiliType()
		{
			return this._neiliType;
		}

		// Token: 0x060060B6 RID: 24758 RVA: 0x0036F7E4 File Offset: 0x0036D9E4
		public unsafe void SetNeiliType(sbyte neiliType, DataContext context)
		{
			this._neiliType = neiliType;
			base.SetModifiedAndInvalidateInfluencedCache(7, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				byte* pData = OperationAdder.DynamicObjectCollection_SetFixedField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 38U, 1);
				*pData = (byte)this._neiliType;
				pData++;
			}
		}

		// Token: 0x060060B7 RID: 24759 RVA: 0x0036F844 File Offset: 0x0036DA44
		public ShowAvoidData GetAvoidToShow()
		{
			return this._avoidToShow;
		}

		// Token: 0x060060B8 RID: 24760 RVA: 0x0036F85C File Offset: 0x0036DA5C
		public unsafe void SetAvoidToShow(ShowAvoidData avoidToShow, DataContext context)
		{
			this._avoidToShow = avoidToShow;
			base.SetModifiedAndInvalidateInfluencedCache(8, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				byte* pData = OperationAdder.DynamicObjectCollection_SetFixedField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 39U, 4);
				pData += this._avoidToShow.Serialize(pData);
			}
		}

		// Token: 0x060060B9 RID: 24761 RVA: 0x0036F8C0 File Offset: 0x0036DAC0
		public int GetCurrentPosition()
		{
			return this._currentPosition;
		}

		// Token: 0x060060BA RID: 24762 RVA: 0x0036F8D8 File Offset: 0x0036DAD8
		public unsafe void SetCurrentPosition(int currentPosition, DataContext context)
		{
			this._currentPosition = currentPosition;
			base.SetModifiedAndInvalidateInfluencedCache(9, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				byte* pData = OperationAdder.DynamicObjectCollection_SetFixedField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 43U, 4);
				*(int*)pData = this._currentPosition;
				pData += 4;
			}
		}

		// Token: 0x060060BB RID: 24763 RVA: 0x0036F93C File Offset: 0x0036DB3C
		public int GetDisplayPosition()
		{
			return this._displayPosition;
		}

		// Token: 0x060060BC RID: 24764 RVA: 0x0036F954 File Offset: 0x0036DB54
		public unsafe void SetDisplayPosition(int displayPosition, DataContext context)
		{
			this._displayPosition = displayPosition;
			base.SetModifiedAndInvalidateInfluencedCache(10, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				byte* pData = OperationAdder.DynamicObjectCollection_SetFixedField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 47U, 4);
				*(int*)pData = this._displayPosition;
				pData += 4;
			}
		}

		// Token: 0x060060BD RID: 24765 RVA: 0x0036F9B8 File Offset: 0x0036DBB8
		public int GetMobilityValue()
		{
			return this._mobilityValue;
		}

		// Token: 0x060060BE RID: 24766 RVA: 0x0036F9D0 File Offset: 0x0036DBD0
		public unsafe void SetMobilityValue(int mobilityValue, DataContext context)
		{
			this._mobilityValue = mobilityValue;
			base.SetModifiedAndInvalidateInfluencedCache(11, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				byte* pData = OperationAdder.DynamicObjectCollection_SetFixedField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 51U, 4);
				*(int*)pData = this._mobilityValue;
				pData += 4;
			}
		}

		// Token: 0x060060BF RID: 24767 RVA: 0x0036FA34 File Offset: 0x0036DC34
		public sbyte GetJumpPrepareProgress()
		{
			return this._jumpPrepareProgress;
		}

		// Token: 0x060060C0 RID: 24768 RVA: 0x0036FA4C File Offset: 0x0036DC4C
		public unsafe void SetJumpPrepareProgress(sbyte jumpPrepareProgress, DataContext context)
		{
			this._jumpPrepareProgress = jumpPrepareProgress;
			base.SetModifiedAndInvalidateInfluencedCache(12, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				byte* pData = OperationAdder.DynamicObjectCollection_SetFixedField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 55U, 1);
				*pData = (byte)this._jumpPrepareProgress;
				pData++;
			}
		}

		// Token: 0x060060C1 RID: 24769 RVA: 0x0036FAB0 File Offset: 0x0036DCB0
		public short GetJumpPreparedDistance()
		{
			return this._jumpPreparedDistance;
		}

		// Token: 0x060060C2 RID: 24770 RVA: 0x0036FAC8 File Offset: 0x0036DCC8
		public unsafe void SetJumpPreparedDistance(short jumpPreparedDistance, DataContext context)
		{
			this._jumpPreparedDistance = jumpPreparedDistance;
			base.SetModifiedAndInvalidateInfluencedCache(13, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				byte* pData = OperationAdder.DynamicObjectCollection_SetFixedField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 56U, 2);
				*(short*)pData = this._jumpPreparedDistance;
				pData += 2;
			}
		}

		// Token: 0x060060C3 RID: 24771 RVA: 0x0036FB2C File Offset: 0x0036DD2C
		public short GetMobilityLockEffectCount()
		{
			return this._mobilityLockEffectCount;
		}

		// Token: 0x060060C4 RID: 24772 RVA: 0x0036FB44 File Offset: 0x0036DD44
		public unsafe void SetMobilityLockEffectCount(short mobilityLockEffectCount, DataContext context)
		{
			this._mobilityLockEffectCount = mobilityLockEffectCount;
			base.SetModifiedAndInvalidateInfluencedCache(14, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				byte* pData = OperationAdder.DynamicObjectCollection_SetFixedField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 58U, 2);
				*(short*)pData = this._mobilityLockEffectCount;
				pData += 2;
			}
		}

		// Token: 0x060060C5 RID: 24773 RVA: 0x0036FBA8 File Offset: 0x0036DDA8
		public float GetJumpChangeDistanceDuration()
		{
			return this._jumpChangeDistanceDuration;
		}

		// Token: 0x060060C6 RID: 24774 RVA: 0x0036FBC0 File Offset: 0x0036DDC0
		public unsafe void SetJumpChangeDistanceDuration(float jumpChangeDistanceDuration, DataContext context)
		{
			this._jumpChangeDistanceDuration = jumpChangeDistanceDuration;
			base.SetModifiedAndInvalidateInfluencedCache(15, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				byte* pData = OperationAdder.DynamicObjectCollection_SetFixedField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 60U, 4);
				*(float*)pData = this._jumpChangeDistanceDuration;
				pData += 4;
			}
		}

		// Token: 0x060060C7 RID: 24775 RVA: 0x0036FC24 File Offset: 0x0036DE24
		public int GetUsingWeaponIndex()
		{
			return this._usingWeaponIndex;
		}

		// Token: 0x060060C8 RID: 24776 RVA: 0x0036FC3C File Offset: 0x0036DE3C
		public unsafe void SetUsingWeaponIndex(int usingWeaponIndex, DataContext context)
		{
			this._usingWeaponIndex = usingWeaponIndex;
			base.SetModifiedAndInvalidateInfluencedCache(16, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				byte* pData = OperationAdder.DynamicObjectCollection_SetFixedField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 64U, 4);
				*(int*)pData = this._usingWeaponIndex;
				pData += 4;
			}
		}

		// Token: 0x060060C9 RID: 24777 RVA: 0x0036FCA0 File Offset: 0x0036DEA0
		public sbyte[] GetWeaponTricks()
		{
			return this._weaponTricks;
		}

		// Token: 0x060060CA RID: 24778 RVA: 0x0036FCB8 File Offset: 0x0036DEB8
		public unsafe void SetWeaponTricks(sbyte[] weaponTricks, DataContext context)
		{
			this._weaponTricks = weaponTricks;
			base.SetModifiedAndInvalidateInfluencedCache(17, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				byte* pData = OperationAdder.DynamicObjectCollection_SetFixedField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 68U, 6);
				for (int i = 0; i < 6; i++)
				{
					pData[i] = (byte)this._weaponTricks[i];
				}
				pData += 6;
			}
		}

		// Token: 0x060060CB RID: 24779 RVA: 0x0036FD30 File Offset: 0x0036DF30
		public byte GetWeaponTrickIndex()
		{
			return this._weaponTrickIndex;
		}

		// Token: 0x060060CC RID: 24780 RVA: 0x0036FD48 File Offset: 0x0036DF48
		public unsafe void SetWeaponTrickIndex(byte weaponTrickIndex, DataContext context)
		{
			this._weaponTrickIndex = weaponTrickIndex;
			base.SetModifiedAndInvalidateInfluencedCache(18, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				byte* pData = OperationAdder.DynamicObjectCollection_SetFixedField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 74U, 1);
				*pData = this._weaponTrickIndex;
				pData++;
			}
		}

		// Token: 0x060060CD RID: 24781 RVA: 0x0036FDAC File Offset: 0x0036DFAC
		public ItemKey[] GetWeapons()
		{
			return this._weapons;
		}

		// Token: 0x060060CE RID: 24782 RVA: 0x0036FDC4 File Offset: 0x0036DFC4
		public unsafe void SetWeapons(ItemKey[] weapons, DataContext context)
		{
			this._weapons = weapons;
			base.SetModifiedAndInvalidateInfluencedCache(19, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				byte* pData = OperationAdder.DynamicObjectCollection_SetFixedField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 75U, 56);
				for (int i = 0; i < 7; i++)
				{
					pData += this._weapons[i].Serialize(pData);
				}
			}
		}

		// Token: 0x060060CF RID: 24783 RVA: 0x0036FE40 File Offset: 0x0036E040
		public sbyte GetAttackingTrickType()
		{
			return this._attackingTrickType;
		}

		// Token: 0x060060D0 RID: 24784 RVA: 0x0036FE58 File Offset: 0x0036E058
		public unsafe void SetAttackingTrickType(sbyte attackingTrickType, DataContext context)
		{
			this._attackingTrickType = attackingTrickType;
			base.SetModifiedAndInvalidateInfluencedCache(20, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				byte* pData = OperationAdder.DynamicObjectCollection_SetFixedField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 131U, 1);
				*pData = (byte)this._attackingTrickType;
				pData++;
			}
		}

		// Token: 0x060060D1 RID: 24785 RVA: 0x0036FEBC File Offset: 0x0036E0BC
		public bool GetCanAttackOutRange()
		{
			return this._canAttackOutRange;
		}

		// Token: 0x060060D2 RID: 24786 RVA: 0x0036FED4 File Offset: 0x0036E0D4
		public unsafe void SetCanAttackOutRange(bool canAttackOutRange, DataContext context)
		{
			this._canAttackOutRange = canAttackOutRange;
			base.SetModifiedAndInvalidateInfluencedCache(21, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				byte* pData = OperationAdder.DynamicObjectCollection_SetFixedField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 132U, 1);
				*pData = (this._canAttackOutRange ? 1 : 0);
				pData++;
			}
		}

		// Token: 0x060060D3 RID: 24787 RVA: 0x0036FF38 File Offset: 0x0036E138
		public sbyte GetChangeTrickProgress()
		{
			return this._changeTrickProgress;
		}

		// Token: 0x060060D4 RID: 24788 RVA: 0x0036FF50 File Offset: 0x0036E150
		public unsafe void SetChangeTrickProgress(sbyte changeTrickProgress, DataContext context)
		{
			this._changeTrickProgress = changeTrickProgress;
			base.SetModifiedAndInvalidateInfluencedCache(22, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				byte* pData = OperationAdder.DynamicObjectCollection_SetFixedField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 133U, 1);
				*pData = (byte)this._changeTrickProgress;
				pData++;
			}
		}

		// Token: 0x060060D5 RID: 24789 RVA: 0x0036FFB4 File Offset: 0x0036E1B4
		public short GetChangeTrickCount()
		{
			return this._changeTrickCount;
		}

		// Token: 0x060060D6 RID: 24790 RVA: 0x0036FFCC File Offset: 0x0036E1CC
		public unsafe void SetChangeTrickCount(short changeTrickCount, DataContext context)
		{
			this._changeTrickCount = changeTrickCount;
			base.SetModifiedAndInvalidateInfluencedCache(23, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				byte* pData = OperationAdder.DynamicObjectCollection_SetFixedField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 134U, 2);
				*(short*)pData = this._changeTrickCount;
				pData += 2;
			}
		}

		// Token: 0x060060D7 RID: 24791 RVA: 0x00370030 File Offset: 0x0036E230
		public bool GetCanChangeTrick()
		{
			return this._canChangeTrick;
		}

		// Token: 0x060060D8 RID: 24792 RVA: 0x00370048 File Offset: 0x0036E248
		public unsafe void SetCanChangeTrick(bool canChangeTrick, DataContext context)
		{
			this._canChangeTrick = canChangeTrick;
			base.SetModifiedAndInvalidateInfluencedCache(24, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				byte* pData = OperationAdder.DynamicObjectCollection_SetFixedField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 136U, 1);
				*pData = (this._canChangeTrick ? 1 : 0);
				pData++;
			}
		}

		// Token: 0x060060D9 RID: 24793 RVA: 0x003700AC File Offset: 0x0036E2AC
		public bool GetChangingTrick()
		{
			return this._changingTrick;
		}

		// Token: 0x060060DA RID: 24794 RVA: 0x003700C4 File Offset: 0x0036E2C4
		public unsafe void SetChangingTrick(bool changingTrick, DataContext context)
		{
			this._changingTrick = changingTrick;
			base.SetModifiedAndInvalidateInfluencedCache(25, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				byte* pData = OperationAdder.DynamicObjectCollection_SetFixedField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 137U, 1);
				*pData = (this._changingTrick ? 1 : 0);
				pData++;
			}
		}

		// Token: 0x060060DB RID: 24795 RVA: 0x00370128 File Offset: 0x0036E328
		public bool GetChangeTrickAttack()
		{
			return this._changeTrickAttack;
		}

		// Token: 0x060060DC RID: 24796 RVA: 0x00370140 File Offset: 0x0036E340
		public unsafe void SetChangeTrickAttack(bool changeTrickAttack, DataContext context)
		{
			this._changeTrickAttack = changeTrickAttack;
			base.SetModifiedAndInvalidateInfluencedCache(26, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				byte* pData = OperationAdder.DynamicObjectCollection_SetFixedField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 138U, 1);
				*pData = (this._changeTrickAttack ? 1 : 0);
				pData++;
			}
		}

		// Token: 0x060060DD RID: 24797 RVA: 0x003701A4 File Offset: 0x0036E3A4
		public bool GetIsFightBack()
		{
			return this._isFightBack;
		}

		// Token: 0x060060DE RID: 24798 RVA: 0x003701BC File Offset: 0x0036E3BC
		public unsafe void SetIsFightBack(bool isFightBack, DataContext context)
		{
			this._isFightBack = isFightBack;
			base.SetModifiedAndInvalidateInfluencedCache(27, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				byte* pData = OperationAdder.DynamicObjectCollection_SetFixedField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 139U, 1);
				*pData = (this._isFightBack ? 1 : 0);
				pData++;
			}
		}

		// Token: 0x060060DF RID: 24799 RVA: 0x00370220 File Offset: 0x0036E420
		public TrickCollection GetTricks()
		{
			return this._tricks;
		}

		// Token: 0x060060E0 RID: 24800 RVA: 0x00370238 File Offset: 0x0036E438
		public unsafe void SetTricks(TrickCollection tricks, DataContext context)
		{
			this._tricks = tricks;
			base.SetModifiedAndInvalidateInfluencedCache(28, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._tricks.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 0, dataSize);
				pData += this._tricks.Serialize(pData);
			}
		}

		// Token: 0x060060E1 RID: 24801 RVA: 0x003702A8 File Offset: 0x0036E4A8
		public Injuries GetInjuries()
		{
			return this._injuries;
		}

		// Token: 0x060060E2 RID: 24802 RVA: 0x003702C0 File Offset: 0x0036E4C0
		public unsafe void SetInjuries(Injuries injuries, DataContext context)
		{
			this._injuries = injuries;
			base.SetModifiedAndInvalidateInfluencedCache(29, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				byte* pData = OperationAdder.DynamicObjectCollection_SetFixedField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 140U, 16);
				pData += this._injuries.Serialize(pData);
			}
		}

		// Token: 0x060060E3 RID: 24803 RVA: 0x00370328 File Offset: 0x0036E528
		public Injuries GetOldInjuries()
		{
			return this._oldInjuries;
		}

		// Token: 0x060060E4 RID: 24804 RVA: 0x00370340 File Offset: 0x0036E540
		public unsafe void SetOldInjuries(Injuries oldInjuries, DataContext context)
		{
			this._oldInjuries = oldInjuries;
			base.SetModifiedAndInvalidateInfluencedCache(30, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				byte* pData = OperationAdder.DynamicObjectCollection_SetFixedField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 156U, 16);
				pData += this._oldInjuries.Serialize(pData);
			}
		}

		// Token: 0x060060E5 RID: 24805 RVA: 0x003703A8 File Offset: 0x0036E5A8
		public InjuryAutoHealCollection GetInjuryAutoHealCollection()
		{
			return this._injuryAutoHealCollection;
		}

		// Token: 0x060060E6 RID: 24806 RVA: 0x003703C0 File Offset: 0x0036E5C0
		public unsafe void SetInjuryAutoHealCollection(InjuryAutoHealCollection injuryAutoHealCollection, DataContext context)
		{
			this._injuryAutoHealCollection = injuryAutoHealCollection;
			base.SetModifiedAndInvalidateInfluencedCache(31, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._injuryAutoHealCollection.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 1, dataSize);
				pData += this._injuryAutoHealCollection.Serialize(pData);
			}
		}

		// Token: 0x060060E7 RID: 24807 RVA: 0x00370430 File Offset: 0x0036E630
		public DamageStepCollection GetDamageStepCollection()
		{
			return this._damageStepCollection;
		}

		// Token: 0x060060E8 RID: 24808 RVA: 0x00370448 File Offset: 0x0036E648
		public unsafe void SetDamageStepCollection(DamageStepCollection damageStepCollection, DataContext context)
		{
			this._damageStepCollection = damageStepCollection;
			base.SetModifiedAndInvalidateInfluencedCache(32, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				byte* pData = OperationAdder.DynamicObjectCollection_SetFixedField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 172U, 64);
				pData += this._damageStepCollection.Serialize(pData);
			}
		}

		// Token: 0x060060E9 RID: 24809 RVA: 0x003704B0 File Offset: 0x0036E6B0
		public int[] GetOuterDamageValue()
		{
			return this._outerDamageValue;
		}

		// Token: 0x060060EA RID: 24810 RVA: 0x003704C8 File Offset: 0x0036E6C8
		public unsafe void SetOuterDamageValue(int[] outerDamageValue, DataContext context)
		{
			this._outerDamageValue = outerDamageValue;
			base.SetModifiedAndInvalidateInfluencedCache(33, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				byte* pData = OperationAdder.DynamicObjectCollection_SetFixedField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 236U, 28);
				for (int i = 0; i < 7; i++)
				{
					*(int*)(pData + (IntPtr)i * 4) = this._outerDamageValue[i];
				}
				pData += 28;
			}
		}

		// Token: 0x060060EB RID: 24811 RVA: 0x00370548 File Offset: 0x0036E748
		public int[] GetInnerDamageValue()
		{
			return this._innerDamageValue;
		}

		// Token: 0x060060EC RID: 24812 RVA: 0x00370560 File Offset: 0x0036E760
		public unsafe void SetInnerDamageValue(int[] innerDamageValue, DataContext context)
		{
			this._innerDamageValue = innerDamageValue;
			base.SetModifiedAndInvalidateInfluencedCache(34, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				byte* pData = OperationAdder.DynamicObjectCollection_SetFixedField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 264U, 28);
				for (int i = 0; i < 7; i++)
				{
					*(int*)(pData + (IntPtr)i * 4) = this._innerDamageValue[i];
				}
				pData += 28;
			}
		}

		// Token: 0x060060ED RID: 24813 RVA: 0x003705E0 File Offset: 0x0036E7E0
		public int GetMindDamageValue()
		{
			return this._mindDamageValue;
		}

		// Token: 0x060060EE RID: 24814 RVA: 0x003705F8 File Offset: 0x0036E7F8
		public unsafe void SetMindDamageValue(int mindDamageValue, DataContext context)
		{
			this._mindDamageValue = mindDamageValue;
			base.SetModifiedAndInvalidateInfluencedCache(35, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				byte* pData = OperationAdder.DynamicObjectCollection_SetFixedField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 292U, 4);
				*(int*)pData = this._mindDamageValue;
				pData += 4;
			}
		}

		// Token: 0x060060EF RID: 24815 RVA: 0x0037065C File Offset: 0x0036E85C
		public int GetFatalDamageValue()
		{
			return this._fatalDamageValue;
		}

		// Token: 0x060060F0 RID: 24816 RVA: 0x00370674 File Offset: 0x0036E874
		public unsafe void SetFatalDamageValue(int fatalDamageValue, DataContext context)
		{
			this._fatalDamageValue = fatalDamageValue;
			base.SetModifiedAndInvalidateInfluencedCache(36, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				byte* pData = OperationAdder.DynamicObjectCollection_SetFixedField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 296U, 4);
				*(int*)pData = this._fatalDamageValue;
				pData += 4;
			}
		}

		// Token: 0x060060F1 RID: 24817 RVA: 0x003706D8 File Offset: 0x0036E8D8
		public IntPair[] GetOuterDamageValueToShow()
		{
			return this._outerDamageValueToShow;
		}

		// Token: 0x060060F2 RID: 24818 RVA: 0x003706F0 File Offset: 0x0036E8F0
		public unsafe void SetOuterDamageValueToShow(IntPair[] outerDamageValueToShow, DataContext context)
		{
			this._outerDamageValueToShow = outerDamageValueToShow;
			base.SetModifiedAndInvalidateInfluencedCache(37, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				byte* pData = OperationAdder.DynamicObjectCollection_SetFixedField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 300U, 56);
				for (int i = 0; i < 7; i++)
				{
					pData += this._outerDamageValueToShow[i].Serialize(pData);
				}
			}
		}

		// Token: 0x060060F3 RID: 24819 RVA: 0x00370770 File Offset: 0x0036E970
		public IntPair[] GetInnerDamageValueToShow()
		{
			return this._innerDamageValueToShow;
		}

		// Token: 0x060060F4 RID: 24820 RVA: 0x00370788 File Offset: 0x0036E988
		public unsafe void SetInnerDamageValueToShow(IntPair[] innerDamageValueToShow, DataContext context)
		{
			this._innerDamageValueToShow = innerDamageValueToShow;
			base.SetModifiedAndInvalidateInfluencedCache(38, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				byte* pData = OperationAdder.DynamicObjectCollection_SetFixedField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 356U, 56);
				for (int i = 0; i < 7; i++)
				{
					pData += this._innerDamageValueToShow[i].Serialize(pData);
				}
			}
		}

		// Token: 0x060060F5 RID: 24821 RVA: 0x00370808 File Offset: 0x0036EA08
		public int GetMindDamageValueToShow()
		{
			return this._mindDamageValueToShow;
		}

		// Token: 0x060060F6 RID: 24822 RVA: 0x00370820 File Offset: 0x0036EA20
		public unsafe void SetMindDamageValueToShow(int mindDamageValueToShow, DataContext context)
		{
			this._mindDamageValueToShow = mindDamageValueToShow;
			base.SetModifiedAndInvalidateInfluencedCache(39, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				byte* pData = OperationAdder.DynamicObjectCollection_SetFixedField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 412U, 4);
				*(int*)pData = this._mindDamageValueToShow;
				pData += 4;
			}
		}

		// Token: 0x060060F7 RID: 24823 RVA: 0x00370884 File Offset: 0x0036EA84
		public int GetFatalDamageValueToShow()
		{
			return this._fatalDamageValueToShow;
		}

		// Token: 0x060060F8 RID: 24824 RVA: 0x0037089C File Offset: 0x0036EA9C
		public unsafe void SetFatalDamageValueToShow(int fatalDamageValueToShow, DataContext context)
		{
			this._fatalDamageValueToShow = fatalDamageValueToShow;
			base.SetModifiedAndInvalidateInfluencedCache(40, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				byte* pData = OperationAdder.DynamicObjectCollection_SetFixedField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 416U, 4);
				*(int*)pData = this._fatalDamageValueToShow;
				pData += 4;
			}
		}

		// Token: 0x060060F9 RID: 24825 RVA: 0x00370900 File Offset: 0x0036EB00
		public byte[] GetFlawCount()
		{
			return this._flawCount;
		}

		// Token: 0x060060FA RID: 24826 RVA: 0x00370918 File Offset: 0x0036EB18
		public unsafe void SetFlawCount(byte[] flawCount, DataContext context)
		{
			this._flawCount = flawCount;
			base.SetModifiedAndInvalidateInfluencedCache(41, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				byte* pData = OperationAdder.DynamicObjectCollection_SetFixedField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 420U, 7);
				for (int i = 0; i < 7; i++)
				{
					pData[i] = this._flawCount[i];
				}
				pData += 7;
			}
		}

		// Token: 0x060060FB RID: 24827 RVA: 0x00370990 File Offset: 0x0036EB90
		public FlawOrAcupointCollection GetFlawCollection()
		{
			return this._flawCollection;
		}

		// Token: 0x060060FC RID: 24828 RVA: 0x003709A8 File Offset: 0x0036EBA8
		public unsafe void SetFlawCollection(FlawOrAcupointCollection flawCollection, DataContext context)
		{
			this._flawCollection = flawCollection;
			base.SetModifiedAndInvalidateInfluencedCache(42, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._flawCollection.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 2, dataSize);
				pData += this._flawCollection.Serialize(pData);
			}
		}

		// Token: 0x060060FD RID: 24829 RVA: 0x00370A18 File Offset: 0x0036EC18
		public byte[] GetAcupointCount()
		{
			return this._acupointCount;
		}

		// Token: 0x060060FE RID: 24830 RVA: 0x00370A30 File Offset: 0x0036EC30
		public unsafe void SetAcupointCount(byte[] acupointCount, DataContext context)
		{
			this._acupointCount = acupointCount;
			base.SetModifiedAndInvalidateInfluencedCache(43, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				byte* pData = OperationAdder.DynamicObjectCollection_SetFixedField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 427U, 7);
				for (int i = 0; i < 7; i++)
				{
					pData[i] = this._acupointCount[i];
				}
				pData += 7;
			}
		}

		// Token: 0x060060FF RID: 24831 RVA: 0x00370AA8 File Offset: 0x0036ECA8
		public FlawOrAcupointCollection GetAcupointCollection()
		{
			return this._acupointCollection;
		}

		// Token: 0x06006100 RID: 24832 RVA: 0x00370AC0 File Offset: 0x0036ECC0
		public unsafe void SetAcupointCollection(FlawOrAcupointCollection acupointCollection, DataContext context)
		{
			this._acupointCollection = acupointCollection;
			base.SetModifiedAndInvalidateInfluencedCache(44, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._acupointCollection.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 3, dataSize);
				pData += this._acupointCollection.Serialize(pData);
			}
		}

		// Token: 0x06006101 RID: 24833 RVA: 0x00370B30 File Offset: 0x0036ED30
		public MindMarkList GetMindMarkTime()
		{
			return this._mindMarkTime;
		}

		// Token: 0x06006102 RID: 24834 RVA: 0x00370B48 File Offset: 0x0036ED48
		public unsafe void SetMindMarkTime(MindMarkList mindMarkTime, DataContext context)
		{
			this._mindMarkTime = mindMarkTime;
			base.SetModifiedAndInvalidateInfluencedCache(45, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._mindMarkTime.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 4, dataSize);
				pData += this._mindMarkTime.Serialize(pData);
			}
		}

		// Token: 0x06006103 RID: 24835 RVA: 0x00370BB8 File Offset: 0x0036EDB8
		public ref PoisonInts GetPoison()
		{
			return ref this._poison;
		}

		// Token: 0x06006104 RID: 24836 RVA: 0x00370BD0 File Offset: 0x0036EDD0
		public unsafe void SetPoison(ref PoisonInts poison, DataContext context)
		{
			this._poison = poison;
			base.SetModifiedAndInvalidateInfluencedCache(46, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				byte* pData = OperationAdder.DynamicObjectCollection_SetFixedField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 434U, 24);
				pData += this._poison.Serialize(pData);
			}
		}

		// Token: 0x06006105 RID: 24837 RVA: 0x00370C40 File Offset: 0x0036EE40
		public ref PoisonInts GetOldPoison()
		{
			return ref this._oldPoison;
		}

		// Token: 0x06006106 RID: 24838 RVA: 0x00370C58 File Offset: 0x0036EE58
		public unsafe void SetOldPoison(ref PoisonInts oldPoison, DataContext context)
		{
			this._oldPoison = oldPoison;
			base.SetModifiedAndInvalidateInfluencedCache(47, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				byte* pData = OperationAdder.DynamicObjectCollection_SetFixedField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 458U, 24);
				pData += this._oldPoison.Serialize(pData);
			}
		}

		// Token: 0x06006107 RID: 24839 RVA: 0x00370CC8 File Offset: 0x0036EEC8
		public ref PoisonInts GetPoisonResist()
		{
			return ref this._poisonResist;
		}

		// Token: 0x06006108 RID: 24840 RVA: 0x00370CE0 File Offset: 0x0036EEE0
		public unsafe void SetPoisonResist(ref PoisonInts poisonResist, DataContext context)
		{
			this._poisonResist = poisonResist;
			base.SetModifiedAndInvalidateInfluencedCache(48, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				byte* pData = OperationAdder.DynamicObjectCollection_SetFixedField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 482U, 24);
				pData += this._poisonResist.Serialize(pData);
			}
		}

		// Token: 0x06006109 RID: 24841 RVA: 0x00370D50 File Offset: 0x0036EF50
		public ref PoisonsAndLevels GetNewPoisonsToShow()
		{
			return ref this._newPoisonsToShow;
		}

		// Token: 0x0600610A RID: 24842 RVA: 0x00370D68 File Offset: 0x0036EF68
		public unsafe void SetNewPoisonsToShow(ref PoisonsAndLevels newPoisonsToShow, DataContext context)
		{
			this._newPoisonsToShow = newPoisonsToShow;
			base.SetModifiedAndInvalidateInfluencedCache(49, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				byte* pData = OperationAdder.DynamicObjectCollection_SetFixedField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 506U, 18);
				pData += this._newPoisonsToShow.Serialize(pData);
			}
		}

		// Token: 0x0600610B RID: 24843 RVA: 0x00370DD8 File Offset: 0x0036EFD8
		public DefeatMarkCollection GetDefeatMarkCollection()
		{
			return this._defeatMarkCollection;
		}

		// Token: 0x0600610C RID: 24844 RVA: 0x00370DF0 File Offset: 0x0036EFF0
		public unsafe void SetDefeatMarkCollection(DefeatMarkCollection defeatMarkCollection, DataContext context)
		{
			this._defeatMarkCollection = defeatMarkCollection;
			base.SetModifiedAndInvalidateInfluencedCache(50, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._defeatMarkCollection.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 5, dataSize);
				pData += this._defeatMarkCollection.Serialize(pData);
			}
		}

		// Token: 0x0600610D RID: 24845 RVA: 0x00370E60 File Offset: 0x0036F060
		public List<short> GetNeigongList()
		{
			return this._neigongList;
		}

		// Token: 0x0600610E RID: 24846 RVA: 0x00370E78 File Offset: 0x0036F078
		public unsafe void SetNeigongList(List<short> neigongList, DataContext context)
		{
			this._neigongList = neigongList;
			base.SetModifiedAndInvalidateInfluencedCache(51, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int elementsCount = this._neigongList.Count;
				int contentSize = 2 * elementsCount;
				int dataSize = 2 + contentSize;
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 6, dataSize);
				*(short*)pData = (short)((ushort)elementsCount);
				pData += 2;
				for (int i = 0; i < elementsCount; i++)
				{
					*(short*)(pData + (IntPtr)i * 2) = this._neigongList[i];
				}
				pData += contentSize;
			}
		}

		// Token: 0x0600610F RID: 24847 RVA: 0x00370F20 File Offset: 0x0036F120
		public List<short> GetAttackSkillList()
		{
			return this._attackSkillList;
		}

		// Token: 0x06006110 RID: 24848 RVA: 0x00370F38 File Offset: 0x0036F138
		public unsafe void SetAttackSkillList(List<short> attackSkillList, DataContext context)
		{
			this._attackSkillList = attackSkillList;
			base.SetModifiedAndInvalidateInfluencedCache(52, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int elementsCount = this._attackSkillList.Count;
				int contentSize = 2 * elementsCount;
				int dataSize = 2 + contentSize;
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 7, dataSize);
				*(short*)pData = (short)((ushort)elementsCount);
				pData += 2;
				for (int i = 0; i < elementsCount; i++)
				{
					*(short*)(pData + (IntPtr)i * 2) = this._attackSkillList[i];
				}
				pData += contentSize;
			}
		}

		// Token: 0x06006111 RID: 24849 RVA: 0x00370FE0 File Offset: 0x0036F1E0
		public List<short> GetAgileSkillList()
		{
			return this._agileSkillList;
		}

		// Token: 0x06006112 RID: 24850 RVA: 0x00370FF8 File Offset: 0x0036F1F8
		public unsafe void SetAgileSkillList(List<short> agileSkillList, DataContext context)
		{
			this._agileSkillList = agileSkillList;
			base.SetModifiedAndInvalidateInfluencedCache(53, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int elementsCount = this._agileSkillList.Count;
				int contentSize = 2 * elementsCount;
				int dataSize = 2 + contentSize;
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 8, dataSize);
				*(short*)pData = (short)((ushort)elementsCount);
				pData += 2;
				for (int i = 0; i < elementsCount; i++)
				{
					*(short*)(pData + (IntPtr)i * 2) = this._agileSkillList[i];
				}
				pData += contentSize;
			}
		}

		// Token: 0x06006113 RID: 24851 RVA: 0x003710A0 File Offset: 0x0036F2A0
		public List<short> GetDefenceSkillList()
		{
			return this._defenceSkillList;
		}

		// Token: 0x06006114 RID: 24852 RVA: 0x003710B8 File Offset: 0x0036F2B8
		public unsafe void SetDefenceSkillList(List<short> defenceSkillList, DataContext context)
		{
			this._defenceSkillList = defenceSkillList;
			base.SetModifiedAndInvalidateInfluencedCache(54, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int elementsCount = this._defenceSkillList.Count;
				int contentSize = 2 * elementsCount;
				int dataSize = 2 + contentSize;
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 9, dataSize);
				*(short*)pData = (short)((ushort)elementsCount);
				pData += 2;
				for (int i = 0; i < elementsCount; i++)
				{
					*(short*)(pData + (IntPtr)i * 2) = this._defenceSkillList[i];
				}
				pData += contentSize;
			}
		}

		// Token: 0x06006115 RID: 24853 RVA: 0x00371160 File Offset: 0x0036F360
		public List<short> GetAssistSkillList()
		{
			return this._assistSkillList;
		}

		// Token: 0x06006116 RID: 24854 RVA: 0x00371178 File Offset: 0x0036F378
		public unsafe void SetAssistSkillList(List<short> assistSkillList, DataContext context)
		{
			this._assistSkillList = assistSkillList;
			base.SetModifiedAndInvalidateInfluencedCache(55, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int elementsCount = this._assistSkillList.Count;
				int contentSize = 2 * elementsCount;
				int dataSize = 2 + contentSize;
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 10, dataSize);
				*(short*)pData = (short)((ushort)elementsCount);
				pData += 2;
				for (int i = 0; i < elementsCount; i++)
				{
					*(short*)(pData + (IntPtr)i * 2) = this._assistSkillList[i];
				}
				pData += contentSize;
			}
		}

		// Token: 0x06006117 RID: 24855 RVA: 0x00371220 File Offset: 0x0036F420
		public short GetPreparingSkillId()
		{
			return this._preparingSkillId;
		}

		// Token: 0x06006118 RID: 24856 RVA: 0x00371238 File Offset: 0x0036F438
		public unsafe void SetPreparingSkillId(short preparingSkillId, DataContext context)
		{
			this._preparingSkillId = preparingSkillId;
			base.SetModifiedAndInvalidateInfluencedCache(56, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				byte* pData = OperationAdder.DynamicObjectCollection_SetFixedField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 524U, 2);
				*(short*)pData = this._preparingSkillId;
				pData += 2;
			}
		}

		// Token: 0x06006119 RID: 24857 RVA: 0x0037129C File Offset: 0x0036F49C
		public byte GetSkillPreparePercent()
		{
			return this._skillPreparePercent;
		}

		// Token: 0x0600611A RID: 24858 RVA: 0x003712B4 File Offset: 0x0036F4B4
		public unsafe void SetSkillPreparePercent(byte skillPreparePercent, DataContext context)
		{
			this._skillPreparePercent = skillPreparePercent;
			base.SetModifiedAndInvalidateInfluencedCache(57, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				byte* pData = OperationAdder.DynamicObjectCollection_SetFixedField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 526U, 1);
				*pData = this._skillPreparePercent;
				pData++;
			}
		}

		// Token: 0x0600611B RID: 24859 RVA: 0x00371318 File Offset: 0x0036F518
		public short GetPerformingSkillId()
		{
			return this._performingSkillId;
		}

		// Token: 0x0600611C RID: 24860 RVA: 0x00371330 File Offset: 0x0036F530
		public unsafe void SetPerformingSkillId(short performingSkillId, DataContext context)
		{
			this._performingSkillId = performingSkillId;
			base.SetModifiedAndInvalidateInfluencedCache(58, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				byte* pData = OperationAdder.DynamicObjectCollection_SetFixedField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 527U, 2);
				*(short*)pData = this._performingSkillId;
				pData += 2;
			}
		}

		// Token: 0x0600611D RID: 24861 RVA: 0x00371394 File Offset: 0x0036F594
		public bool GetAutoCastingSkill()
		{
			return this._autoCastingSkill;
		}

		// Token: 0x0600611E RID: 24862 RVA: 0x003713AC File Offset: 0x0036F5AC
		public unsafe void SetAutoCastingSkill(bool autoCastingSkill, DataContext context)
		{
			this._autoCastingSkill = autoCastingSkill;
			base.SetModifiedAndInvalidateInfluencedCache(59, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				byte* pData = OperationAdder.DynamicObjectCollection_SetFixedField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 529U, 1);
				*pData = (this._autoCastingSkill ? 1 : 0);
				pData++;
			}
		}

		// Token: 0x0600611F RID: 24863 RVA: 0x00371410 File Offset: 0x0036F610
		public byte GetAttackSkillAttackIndex()
		{
			return this._attackSkillAttackIndex;
		}

		// Token: 0x06006120 RID: 24864 RVA: 0x00371428 File Offset: 0x0036F628
		public unsafe void SetAttackSkillAttackIndex(byte attackSkillAttackIndex, DataContext context)
		{
			this._attackSkillAttackIndex = attackSkillAttackIndex;
			base.SetModifiedAndInvalidateInfluencedCache(60, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				byte* pData = OperationAdder.DynamicObjectCollection_SetFixedField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 530U, 1);
				*pData = this._attackSkillAttackIndex;
				pData++;
			}
		}

		// Token: 0x06006121 RID: 24865 RVA: 0x0037148C File Offset: 0x0036F68C
		public byte GetAttackSkillPower()
		{
			return this._attackSkillPower;
		}

		// Token: 0x06006122 RID: 24866 RVA: 0x003714A4 File Offset: 0x0036F6A4
		public unsafe void SetAttackSkillPower(byte attackSkillPower, DataContext context)
		{
			this._attackSkillPower = attackSkillPower;
			base.SetModifiedAndInvalidateInfluencedCache(61, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				byte* pData = OperationAdder.DynamicObjectCollection_SetFixedField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 531U, 1);
				*pData = this._attackSkillPower;
				pData++;
			}
		}

		// Token: 0x06006123 RID: 24867 RVA: 0x00371508 File Offset: 0x0036F708
		public short GetAffectingMoveSkillId()
		{
			return this._affectingMoveSkillId;
		}

		// Token: 0x06006124 RID: 24868 RVA: 0x00371520 File Offset: 0x0036F720
		public unsafe void SetAffectingMoveSkillId(short affectingMoveSkillId, DataContext context)
		{
			this._affectingMoveSkillId = affectingMoveSkillId;
			base.SetModifiedAndInvalidateInfluencedCache(62, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				byte* pData = OperationAdder.DynamicObjectCollection_SetFixedField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 532U, 2);
				*(short*)pData = this._affectingMoveSkillId;
				pData += 2;
			}
		}

		// Token: 0x06006125 RID: 24869 RVA: 0x00371584 File Offset: 0x0036F784
		public short GetAffectingDefendSkillId()
		{
			return this._affectingDefendSkillId;
		}

		// Token: 0x06006126 RID: 24870 RVA: 0x0037159C File Offset: 0x0036F79C
		public unsafe void SetAffectingDefendSkillId(short affectingDefendSkillId, DataContext context)
		{
			this._affectingDefendSkillId = affectingDefendSkillId;
			base.SetModifiedAndInvalidateInfluencedCache(63, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				byte* pData = OperationAdder.DynamicObjectCollection_SetFixedField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 534U, 2);
				*(short*)pData = this._affectingDefendSkillId;
				pData += 2;
			}
		}

		// Token: 0x06006127 RID: 24871 RVA: 0x00371600 File Offset: 0x0036F800
		public byte GetDefendSkillTimePercent()
		{
			return this._defendSkillTimePercent;
		}

		// Token: 0x06006128 RID: 24872 RVA: 0x00371618 File Offset: 0x0036F818
		public unsafe void SetDefendSkillTimePercent(byte defendSkillTimePercent, DataContext context)
		{
			this._defendSkillTimePercent = defendSkillTimePercent;
			base.SetModifiedAndInvalidateInfluencedCache(64, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				byte* pData = OperationAdder.DynamicObjectCollection_SetFixedField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 536U, 1);
				*pData = this._defendSkillTimePercent;
				pData++;
			}
		}

		// Token: 0x06006129 RID: 24873 RVA: 0x0037167C File Offset: 0x0036F87C
		public short GetWugCount()
		{
			return this._wugCount;
		}

		// Token: 0x0600612A RID: 24874 RVA: 0x00371694 File Offset: 0x0036F894
		public unsafe void SetWugCount(short wugCount, DataContext context)
		{
			this._wugCount = wugCount;
			base.SetModifiedAndInvalidateInfluencedCache(65, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				byte* pData = OperationAdder.DynamicObjectCollection_SetFixedField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 537U, 2);
				*(short*)pData = this._wugCount;
				pData += 2;
			}
		}

		// Token: 0x0600612B RID: 24875 RVA: 0x003716F8 File Offset: 0x0036F8F8
		public byte GetHealInjuryCount()
		{
			return this._healInjuryCount;
		}

		// Token: 0x0600612C RID: 24876 RVA: 0x00371710 File Offset: 0x0036F910
		public unsafe void SetHealInjuryCount(byte healInjuryCount, DataContext context)
		{
			this._healInjuryCount = healInjuryCount;
			base.SetModifiedAndInvalidateInfluencedCache(66, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				byte* pData = OperationAdder.DynamicObjectCollection_SetFixedField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 539U, 1);
				*pData = this._healInjuryCount;
				pData++;
			}
		}

		// Token: 0x0600612D RID: 24877 RVA: 0x00371774 File Offset: 0x0036F974
		public byte GetHealPoisonCount()
		{
			return this._healPoisonCount;
		}

		// Token: 0x0600612E RID: 24878 RVA: 0x0037178C File Offset: 0x0036F98C
		public unsafe void SetHealPoisonCount(byte healPoisonCount, DataContext context)
		{
			this._healPoisonCount = healPoisonCount;
			base.SetModifiedAndInvalidateInfluencedCache(67, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				byte* pData = OperationAdder.DynamicObjectCollection_SetFixedField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 540U, 1);
				*pData = this._healPoisonCount;
				pData++;
			}
		}

		// Token: 0x0600612F RID: 24879 RVA: 0x003717F0 File Offset: 0x0036F9F0
		public bool[] GetOtherActionCanUse()
		{
			return this._otherActionCanUse;
		}

		// Token: 0x06006130 RID: 24880 RVA: 0x00371808 File Offset: 0x0036FA08
		public unsafe void SetOtherActionCanUse(bool[] otherActionCanUse, DataContext context)
		{
			this._otherActionCanUse = otherActionCanUse;
			base.SetModifiedAndInvalidateInfluencedCache(68, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				byte* pData = OperationAdder.DynamicObjectCollection_SetFixedField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 541U, 5);
				for (int i = 0; i < 5; i++)
				{
					pData[i] = (this._otherActionCanUse[i] ? 1 : 0);
				}
				pData += 5;
			}
		}

		// Token: 0x06006131 RID: 24881 RVA: 0x00371880 File Offset: 0x0036FA80
		public sbyte GetPreparingOtherAction()
		{
			return this._preparingOtherAction;
		}

		// Token: 0x06006132 RID: 24882 RVA: 0x00371898 File Offset: 0x0036FA98
		public unsafe void SetPreparingOtherAction(sbyte preparingOtherAction, DataContext context)
		{
			this._preparingOtherAction = preparingOtherAction;
			base.SetModifiedAndInvalidateInfluencedCache(69, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				byte* pData = OperationAdder.DynamicObjectCollection_SetFixedField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 546U, 1);
				*pData = (byte)this._preparingOtherAction;
				pData++;
			}
		}

		// Token: 0x06006133 RID: 24883 RVA: 0x003718FC File Offset: 0x0036FAFC
		public byte GetOtherActionPreparePercent()
		{
			return this._otherActionPreparePercent;
		}

		// Token: 0x06006134 RID: 24884 RVA: 0x00371914 File Offset: 0x0036FB14
		public unsafe void SetOtherActionPreparePercent(byte otherActionPreparePercent, DataContext context)
		{
			this._otherActionPreparePercent = otherActionPreparePercent;
			base.SetModifiedAndInvalidateInfluencedCache(70, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				byte* pData = OperationAdder.DynamicObjectCollection_SetFixedField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 547U, 1);
				*pData = this._otherActionPreparePercent;
				pData++;
			}
		}

		// Token: 0x06006135 RID: 24885 RVA: 0x00371978 File Offset: 0x0036FB78
		public bool GetCanSurrender()
		{
			return this._canSurrender;
		}

		// Token: 0x06006136 RID: 24886 RVA: 0x00371990 File Offset: 0x0036FB90
		public unsafe void SetCanSurrender(bool canSurrender, DataContext context)
		{
			this._canSurrender = canSurrender;
			base.SetModifiedAndInvalidateInfluencedCache(71, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				byte* pData = OperationAdder.DynamicObjectCollection_SetFixedField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 548U, 1);
				*pData = (this._canSurrender ? 1 : 0);
				pData++;
			}
		}

		// Token: 0x06006137 RID: 24887 RVA: 0x003719F4 File Offset: 0x0036FBF4
		public bool GetCanUseItem()
		{
			return this._canUseItem;
		}

		// Token: 0x06006138 RID: 24888 RVA: 0x00371A0C File Offset: 0x0036FC0C
		public unsafe void SetCanUseItem(bool canUseItem, DataContext context)
		{
			this._canUseItem = canUseItem;
			base.SetModifiedAndInvalidateInfluencedCache(72, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				byte* pData = OperationAdder.DynamicObjectCollection_SetFixedField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 549U, 1);
				*pData = (this._canUseItem ? 1 : 0);
				pData++;
			}
		}

		// Token: 0x06006139 RID: 24889 RVA: 0x00371A70 File Offset: 0x0036FC70
		public ItemKey GetPreparingItem()
		{
			return this._preparingItem;
		}

		// Token: 0x0600613A RID: 24890 RVA: 0x00371A88 File Offset: 0x0036FC88
		public unsafe void SetPreparingItem(ItemKey preparingItem, DataContext context)
		{
			this._preparingItem = preparingItem;
			base.SetModifiedAndInvalidateInfluencedCache(73, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				byte* pData = OperationAdder.DynamicObjectCollection_SetFixedField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 550U, 8);
				pData += this._preparingItem.Serialize(pData);
			}
		}

		// Token: 0x0600613B RID: 24891 RVA: 0x00371AF0 File Offset: 0x0036FCF0
		public byte GetUseItemPreparePercent()
		{
			return this._useItemPreparePercent;
		}

		// Token: 0x0600613C RID: 24892 RVA: 0x00371B08 File Offset: 0x0036FD08
		public unsafe void SetUseItemPreparePercent(byte useItemPreparePercent, DataContext context)
		{
			this._useItemPreparePercent = useItemPreparePercent;
			base.SetModifiedAndInvalidateInfluencedCache(74, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				byte* pData = OperationAdder.DynamicObjectCollection_SetFixedField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 558U, 1);
				*pData = this._useItemPreparePercent;
				pData++;
			}
		}

		// Token: 0x0600613D RID: 24893 RVA: 0x00371B6C File Offset: 0x0036FD6C
		public CombatReserveData GetCombatReserveData()
		{
			return this._combatReserveData;
		}

		// Token: 0x0600613E RID: 24894 RVA: 0x00371B84 File Offset: 0x0036FD84
		public unsafe void SetCombatReserveData(CombatReserveData combatReserveData, DataContext context)
		{
			this._combatReserveData = combatReserveData;
			base.SetModifiedAndInvalidateInfluencedCache(75, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				byte* pData = OperationAdder.DynamicObjectCollection_SetFixedField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 559U, 12);
				pData += this._combatReserveData.Serialize(pData);
			}
		}

		// Token: 0x0600613F RID: 24895 RVA: 0x00371BEC File Offset: 0x0036FDEC
		public CombatStateCollection GetBuffCombatStateCollection()
		{
			return this._buffCombatStateCollection;
		}

		// Token: 0x06006140 RID: 24896 RVA: 0x00371C04 File Offset: 0x0036FE04
		public unsafe void SetBuffCombatStateCollection(CombatStateCollection buffCombatStateCollection, DataContext context)
		{
			this._buffCombatStateCollection = buffCombatStateCollection;
			base.SetModifiedAndInvalidateInfluencedCache(76, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._buffCombatStateCollection.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 11, dataSize);
				pData += this._buffCombatStateCollection.Serialize(pData);
			}
		}

		// Token: 0x06006141 RID: 24897 RVA: 0x00371C74 File Offset: 0x0036FE74
		public CombatStateCollection GetDebuffCombatStateCollection()
		{
			return this._debuffCombatStateCollection;
		}

		// Token: 0x06006142 RID: 24898 RVA: 0x00371C8C File Offset: 0x0036FE8C
		public unsafe void SetDebuffCombatStateCollection(CombatStateCollection debuffCombatStateCollection, DataContext context)
		{
			this._debuffCombatStateCollection = debuffCombatStateCollection;
			base.SetModifiedAndInvalidateInfluencedCache(77, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._debuffCombatStateCollection.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 12, dataSize);
				pData += this._debuffCombatStateCollection.Serialize(pData);
			}
		}

		// Token: 0x06006143 RID: 24899 RVA: 0x00371CFC File Offset: 0x0036FEFC
		public CombatStateCollection GetSpecialCombatStateCollection()
		{
			return this._specialCombatStateCollection;
		}

		// Token: 0x06006144 RID: 24900 RVA: 0x00371D14 File Offset: 0x0036FF14
		public unsafe void SetSpecialCombatStateCollection(CombatStateCollection specialCombatStateCollection, DataContext context)
		{
			this._specialCombatStateCollection = specialCombatStateCollection;
			base.SetModifiedAndInvalidateInfluencedCache(78, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._specialCombatStateCollection.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 13, dataSize);
				pData += this._specialCombatStateCollection.Serialize(pData);
			}
		}

		// Token: 0x06006145 RID: 24901 RVA: 0x00371D84 File Offset: 0x0036FF84
		public SkillEffectCollection GetSkillEffectCollection()
		{
			return this._skillEffectCollection;
		}

		// Token: 0x06006146 RID: 24902 RVA: 0x00371D9C File Offset: 0x0036FF9C
		public unsafe void SetSkillEffectCollection(SkillEffectCollection skillEffectCollection, DataContext context)
		{
			this._skillEffectCollection = skillEffectCollection;
			base.SetModifiedAndInvalidateInfluencedCache(79, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._skillEffectCollection.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 14, dataSize);
				pData += this._skillEffectCollection.Serialize(pData);
			}
		}

		// Token: 0x06006147 RID: 24903 RVA: 0x00371E0C File Offset: 0x0037000C
		public short GetXiangshuEffectId()
		{
			return this._xiangshuEffectId;
		}

		// Token: 0x06006148 RID: 24904 RVA: 0x00371E24 File Offset: 0x00370024
		public unsafe void SetXiangshuEffectId(short xiangshuEffectId, DataContext context)
		{
			this._xiangshuEffectId = xiangshuEffectId;
			base.SetModifiedAndInvalidateInfluencedCache(80, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				byte* pData = OperationAdder.DynamicObjectCollection_SetFixedField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 571U, 2);
				*(short*)pData = this._xiangshuEffectId;
				pData += 2;
			}
		}

		// Token: 0x06006149 RID: 24905 RVA: 0x00371E88 File Offset: 0x00370088
		public int GetHazardValue()
		{
			return this._hazardValue;
		}

		// Token: 0x0600614A RID: 24906 RVA: 0x00371EA0 File Offset: 0x003700A0
		public unsafe void SetHazardValue(int hazardValue, DataContext context)
		{
			this._hazardValue = hazardValue;
			base.SetModifiedAndInvalidateInfluencedCache(81, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				byte* pData = OperationAdder.DynamicObjectCollection_SetFixedField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 573U, 4);
				*(int*)pData = this._hazardValue;
				pData += 4;
			}
		}

		// Token: 0x0600614B RID: 24907 RVA: 0x00371F04 File Offset: 0x00370104
		public ShowSpecialEffectCollection GetShowEffectList()
		{
			return this._showEffectList;
		}

		// Token: 0x0600614C RID: 24908 RVA: 0x00371F1C File Offset: 0x0037011C
		public unsafe void SetShowEffectList(ShowSpecialEffectCollection showEffectList, DataContext context)
		{
			this._showEffectList = showEffectList;
			base.SetModifiedAndInvalidateInfluencedCache(82, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._showEffectList.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 15, dataSize);
				pData += this._showEffectList.Serialize(pData);
			}
		}

		// Token: 0x0600614D RID: 24909 RVA: 0x00371F8C File Offset: 0x0037018C
		public string GetAnimationToLoop()
		{
			return this._animationToLoop;
		}

		// Token: 0x0600614E RID: 24910 RVA: 0x00371FA4 File Offset: 0x003701A4
		public unsafe void SetAnimationToLoop(string animationToLoop, DataContext context)
		{
			this._animationToLoop = animationToLoop;
			base.SetModifiedAndInvalidateInfluencedCache(83, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int elementsCount = this._animationToLoop.Length;
				int contentSize = 2 * elementsCount;
				int dataSize = 4 + contentSize;
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 16, dataSize);
				*(int*)pData = contentSize;
				pData += 4;
				string animationToLoop2 = this._animationToLoop;
				char* ptr;
				if (animationToLoop2 == null)
				{
					ptr = null;
				}
				else
				{
					fixed (char* ptr2 = animationToLoop2.GetPinnableReference())
					{
						ptr = ptr2;
					}
				}
				char* pChar = ptr;
				for (int i = 0; i < elementsCount; i++)
				{
					*(short*)(pData + (IntPtr)i * 2) = (short)pChar[i];
				}
				char* ptr2 = null;
				pData += contentSize;
			}
		}

		// Token: 0x0600614F RID: 24911 RVA: 0x00372068 File Offset: 0x00370268
		public string GetAnimationToPlayOnce()
		{
			return this._animationToPlayOnce;
		}

		// Token: 0x06006150 RID: 24912 RVA: 0x00372080 File Offset: 0x00370280
		public unsafe void SetAnimationToPlayOnce(string animationToPlayOnce, DataContext context)
		{
			this._animationToPlayOnce = animationToPlayOnce;
			base.SetModifiedAndInvalidateInfluencedCache(84, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int elementsCount = this._animationToPlayOnce.Length;
				int contentSize = 2 * elementsCount;
				int dataSize = 4 + contentSize;
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 17, dataSize);
				*(int*)pData = contentSize;
				pData += 4;
				string animationToPlayOnce2 = this._animationToPlayOnce;
				char* ptr;
				if (animationToPlayOnce2 == null)
				{
					ptr = null;
				}
				else
				{
					fixed (char* ptr2 = animationToPlayOnce2.GetPinnableReference())
					{
						ptr = ptr2;
					}
				}
				char* pChar = ptr;
				for (int i = 0; i < elementsCount; i++)
				{
					*(short*)(pData + (IntPtr)i * 2) = (short)pChar[i];
				}
				char* ptr2 = null;
				pData += contentSize;
			}
		}

		// Token: 0x06006151 RID: 24913 RVA: 0x00372144 File Offset: 0x00370344
		public string GetParticleToPlay()
		{
			return this._particleToPlay;
		}

		// Token: 0x06006152 RID: 24914 RVA: 0x0037215C File Offset: 0x0037035C
		public unsafe void SetParticleToPlay(string particleToPlay, DataContext context)
		{
			this._particleToPlay = particleToPlay;
			base.SetModifiedAndInvalidateInfluencedCache(85, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int elementsCount = this._particleToPlay.Length;
				int contentSize = 2 * elementsCount;
				int dataSize = 4 + contentSize;
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 18, dataSize);
				*(int*)pData = contentSize;
				pData += 4;
				string particleToPlay2 = this._particleToPlay;
				char* ptr;
				if (particleToPlay2 == null)
				{
					ptr = null;
				}
				else
				{
					fixed (char* ptr2 = particleToPlay2.GetPinnableReference())
					{
						ptr = ptr2;
					}
				}
				char* pChar = ptr;
				for (int i = 0; i < elementsCount; i++)
				{
					*(short*)(pData + (IntPtr)i * 2) = (short)pChar[i];
				}
				char* ptr2 = null;
				pData += contentSize;
			}
		}

		// Token: 0x06006153 RID: 24915 RVA: 0x00372220 File Offset: 0x00370420
		public string GetParticleToLoop()
		{
			return this._particleToLoop;
		}

		// Token: 0x06006154 RID: 24916 RVA: 0x00372238 File Offset: 0x00370438
		public unsafe void SetParticleToLoop(string particleToLoop, DataContext context)
		{
			this._particleToLoop = particleToLoop;
			base.SetModifiedAndInvalidateInfluencedCache(86, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int elementsCount = this._particleToLoop.Length;
				int contentSize = 2 * elementsCount;
				int dataSize = 4 + contentSize;
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 19, dataSize);
				*(int*)pData = contentSize;
				pData += 4;
				string particleToLoop2 = this._particleToLoop;
				char* ptr;
				if (particleToLoop2 == null)
				{
					ptr = null;
				}
				else
				{
					fixed (char* ptr2 = particleToLoop2.GetPinnableReference())
					{
						ptr = ptr2;
					}
				}
				char* pChar = ptr;
				for (int i = 0; i < elementsCount; i++)
				{
					*(short*)(pData + (IntPtr)i * 2) = (short)pChar[i];
				}
				char* ptr2 = null;
				pData += contentSize;
			}
		}

		// Token: 0x06006155 RID: 24917 RVA: 0x003722FC File Offset: 0x003704FC
		public string GetSkillPetAnimation()
		{
			return this._skillPetAnimation;
		}

		// Token: 0x06006156 RID: 24918 RVA: 0x00372314 File Offset: 0x00370514
		public unsafe void SetSkillPetAnimation(string skillPetAnimation, DataContext context)
		{
			this._skillPetAnimation = skillPetAnimation;
			base.SetModifiedAndInvalidateInfluencedCache(87, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int elementsCount = this._skillPetAnimation.Length;
				int contentSize = 2 * elementsCount;
				int dataSize = 4 + contentSize;
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 20, dataSize);
				*(int*)pData = contentSize;
				pData += 4;
				string skillPetAnimation2 = this._skillPetAnimation;
				char* ptr;
				if (skillPetAnimation2 == null)
				{
					ptr = null;
				}
				else
				{
					fixed (char* ptr2 = skillPetAnimation2.GetPinnableReference())
					{
						ptr = ptr2;
					}
				}
				char* pChar = ptr;
				for (int i = 0; i < elementsCount; i++)
				{
					*(short*)(pData + (IntPtr)i * 2) = (short)pChar[i];
				}
				char* ptr2 = null;
				pData += contentSize;
			}
		}

		// Token: 0x06006157 RID: 24919 RVA: 0x003723D8 File Offset: 0x003705D8
		public string GetPetParticle()
		{
			return this._petParticle;
		}

		// Token: 0x06006158 RID: 24920 RVA: 0x003723F0 File Offset: 0x003705F0
		public unsafe void SetPetParticle(string petParticle, DataContext context)
		{
			this._petParticle = petParticle;
			base.SetModifiedAndInvalidateInfluencedCache(88, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int elementsCount = this._petParticle.Length;
				int contentSize = 2 * elementsCount;
				int dataSize = 4 + contentSize;
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 21, dataSize);
				*(int*)pData = contentSize;
				pData += 4;
				string petParticle2 = this._petParticle;
				char* ptr;
				if (petParticle2 == null)
				{
					ptr = null;
				}
				else
				{
					fixed (char* ptr2 = petParticle2.GetPinnableReference())
					{
						ptr = ptr2;
					}
				}
				char* pChar = ptr;
				for (int i = 0; i < elementsCount; i++)
				{
					*(short*)(pData + (IntPtr)i * 2) = (short)pChar[i];
				}
				char* ptr2 = null;
				pData += contentSize;
			}
		}

		// Token: 0x06006159 RID: 24921 RVA: 0x003724B4 File Offset: 0x003706B4
		public float GetAnimationTimeScale()
		{
			return this._animationTimeScale;
		}

		// Token: 0x0600615A RID: 24922 RVA: 0x003724CC File Offset: 0x003706CC
		public unsafe void SetAnimationTimeScale(float animationTimeScale, DataContext context)
		{
			this._animationTimeScale = animationTimeScale;
			base.SetModifiedAndInvalidateInfluencedCache(89, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				byte* pData = OperationAdder.DynamicObjectCollection_SetFixedField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 577U, 4);
				*(float*)pData = this._animationTimeScale;
				pData += 4;
			}
		}

		// Token: 0x0600615B RID: 24923 RVA: 0x00372530 File Offset: 0x00370730
		public bool GetAttackOutOfRange()
		{
			return this._attackOutOfRange;
		}

		// Token: 0x0600615C RID: 24924 RVA: 0x00372548 File Offset: 0x00370748
		public unsafe void SetAttackOutOfRange(bool attackOutOfRange, DataContext context)
		{
			this._attackOutOfRange = attackOutOfRange;
			base.SetModifiedAndInvalidateInfluencedCache(90, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				byte* pData = OperationAdder.DynamicObjectCollection_SetFixedField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 581U, 1);
				*pData = (this._attackOutOfRange ? 1 : 0);
				pData++;
			}
		}

		// Token: 0x0600615D RID: 24925 RVA: 0x003725AC File Offset: 0x003707AC
		public string GetAttackSoundToPlay()
		{
			return this._attackSoundToPlay;
		}

		// Token: 0x0600615E RID: 24926 RVA: 0x003725C4 File Offset: 0x003707C4
		public unsafe void SetAttackSoundToPlay(string attackSoundToPlay, DataContext context)
		{
			this._attackSoundToPlay = attackSoundToPlay;
			base.SetModifiedAndInvalidateInfluencedCache(91, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int elementsCount = this._attackSoundToPlay.Length;
				int contentSize = 2 * elementsCount;
				int dataSize = 4 + contentSize;
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 22, dataSize);
				*(int*)pData = contentSize;
				pData += 4;
				string attackSoundToPlay2 = this._attackSoundToPlay;
				char* ptr;
				if (attackSoundToPlay2 == null)
				{
					ptr = null;
				}
				else
				{
					fixed (char* ptr2 = attackSoundToPlay2.GetPinnableReference())
					{
						ptr = ptr2;
					}
				}
				char* pChar = ptr;
				for (int i = 0; i < elementsCount; i++)
				{
					*(short*)(pData + (IntPtr)i * 2) = (short)pChar[i];
				}
				char* ptr2 = null;
				pData += contentSize;
			}
		}

		// Token: 0x0600615F RID: 24927 RVA: 0x00372688 File Offset: 0x00370888
		public string GetSkillSoundToPlay()
		{
			return this._skillSoundToPlay;
		}

		// Token: 0x06006160 RID: 24928 RVA: 0x003726A0 File Offset: 0x003708A0
		public unsafe void SetSkillSoundToPlay(string skillSoundToPlay, DataContext context)
		{
			this._skillSoundToPlay = skillSoundToPlay;
			base.SetModifiedAndInvalidateInfluencedCache(92, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int elementsCount = this._skillSoundToPlay.Length;
				int contentSize = 2 * elementsCount;
				int dataSize = 4 + contentSize;
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 23, dataSize);
				*(int*)pData = contentSize;
				pData += 4;
				string skillSoundToPlay2 = this._skillSoundToPlay;
				char* ptr;
				if (skillSoundToPlay2 == null)
				{
					ptr = null;
				}
				else
				{
					fixed (char* ptr2 = skillSoundToPlay2.GetPinnableReference())
					{
						ptr = ptr2;
					}
				}
				char* pChar = ptr;
				for (int i = 0; i < elementsCount; i++)
				{
					*(short*)(pData + (IntPtr)i * 2) = (short)pChar[i];
				}
				char* ptr2 = null;
				pData += contentSize;
			}
		}

		// Token: 0x06006161 RID: 24929 RVA: 0x00372764 File Offset: 0x00370964
		public string GetHitSoundToPlay()
		{
			return this._hitSoundToPlay;
		}

		// Token: 0x06006162 RID: 24930 RVA: 0x0037277C File Offset: 0x0037097C
		public unsafe void SetHitSoundToPlay(string hitSoundToPlay, DataContext context)
		{
			this._hitSoundToPlay = hitSoundToPlay;
			base.SetModifiedAndInvalidateInfluencedCache(93, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int elementsCount = this._hitSoundToPlay.Length;
				int contentSize = 2 * elementsCount;
				int dataSize = 4 + contentSize;
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 24, dataSize);
				*(int*)pData = contentSize;
				pData += 4;
				string hitSoundToPlay2 = this._hitSoundToPlay;
				char* ptr;
				if (hitSoundToPlay2 == null)
				{
					ptr = null;
				}
				else
				{
					fixed (char* ptr2 = hitSoundToPlay2.GetPinnableReference())
					{
						ptr = ptr2;
					}
				}
				char* pChar = ptr;
				for (int i = 0; i < elementsCount; i++)
				{
					*(short*)(pData + (IntPtr)i * 2) = (short)pChar[i];
				}
				char* ptr2 = null;
				pData += contentSize;
			}
		}

		// Token: 0x06006163 RID: 24931 RVA: 0x00372840 File Offset: 0x00370A40
		public string GetArmorHitSoundToPlay()
		{
			return this._armorHitSoundToPlay;
		}

		// Token: 0x06006164 RID: 24932 RVA: 0x00372858 File Offset: 0x00370A58
		public unsafe void SetArmorHitSoundToPlay(string armorHitSoundToPlay, DataContext context)
		{
			this._armorHitSoundToPlay = armorHitSoundToPlay;
			base.SetModifiedAndInvalidateInfluencedCache(94, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int elementsCount = this._armorHitSoundToPlay.Length;
				int contentSize = 2 * elementsCount;
				int dataSize = 4 + contentSize;
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 25, dataSize);
				*(int*)pData = contentSize;
				pData += 4;
				string armorHitSoundToPlay2 = this._armorHitSoundToPlay;
				char* ptr;
				if (armorHitSoundToPlay2 == null)
				{
					ptr = null;
				}
				else
				{
					fixed (char* ptr2 = armorHitSoundToPlay2.GetPinnableReference())
					{
						ptr = ptr2;
					}
				}
				char* pChar = ptr;
				for (int i = 0; i < elementsCount; i++)
				{
					*(short*)(pData + (IntPtr)i * 2) = (short)pChar[i];
				}
				char* ptr2 = null;
				pData += contentSize;
			}
		}

		// Token: 0x06006165 RID: 24933 RVA: 0x0037291C File Offset: 0x00370B1C
		public string GetWhooshSoundToPlay()
		{
			return this._whooshSoundToPlay;
		}

		// Token: 0x06006166 RID: 24934 RVA: 0x00372934 File Offset: 0x00370B34
		public unsafe void SetWhooshSoundToPlay(string whooshSoundToPlay, DataContext context)
		{
			this._whooshSoundToPlay = whooshSoundToPlay;
			base.SetModifiedAndInvalidateInfluencedCache(95, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int elementsCount = this._whooshSoundToPlay.Length;
				int contentSize = 2 * elementsCount;
				int dataSize = 4 + contentSize;
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 26, dataSize);
				*(int*)pData = contentSize;
				pData += 4;
				string whooshSoundToPlay2 = this._whooshSoundToPlay;
				char* ptr;
				if (whooshSoundToPlay2 == null)
				{
					ptr = null;
				}
				else
				{
					fixed (char* ptr2 = whooshSoundToPlay2.GetPinnableReference())
					{
						ptr = ptr2;
					}
				}
				char* pChar = ptr;
				for (int i = 0; i < elementsCount; i++)
				{
					*(short*)(pData + (IntPtr)i * 2) = (short)pChar[i];
				}
				char* ptr2 = null;
				pData += contentSize;
			}
		}

		// Token: 0x06006167 RID: 24935 RVA: 0x003729F8 File Offset: 0x00370BF8
		public string GetShockSoundToPlay()
		{
			return this._shockSoundToPlay;
		}

		// Token: 0x06006168 RID: 24936 RVA: 0x00372A10 File Offset: 0x00370C10
		public unsafe void SetShockSoundToPlay(string shockSoundToPlay, DataContext context)
		{
			this._shockSoundToPlay = shockSoundToPlay;
			base.SetModifiedAndInvalidateInfluencedCache(96, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int elementsCount = this._shockSoundToPlay.Length;
				int contentSize = 2 * elementsCount;
				int dataSize = 4 + contentSize;
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 27, dataSize);
				*(int*)pData = contentSize;
				pData += 4;
				string shockSoundToPlay2 = this._shockSoundToPlay;
				char* ptr;
				if (shockSoundToPlay2 == null)
				{
					ptr = null;
				}
				else
				{
					fixed (char* ptr2 = shockSoundToPlay2.GetPinnableReference())
					{
						ptr = ptr2;
					}
				}
				char* pChar = ptr;
				for (int i = 0; i < elementsCount; i++)
				{
					*(short*)(pData + (IntPtr)i * 2) = (short)pChar[i];
				}
				char* ptr2 = null;
				pData += contentSize;
			}
		}

		// Token: 0x06006169 RID: 24937 RVA: 0x00372AD4 File Offset: 0x00370CD4
		public string GetStepSoundToPlay()
		{
			return this._stepSoundToPlay;
		}

		// Token: 0x0600616A RID: 24938 RVA: 0x00372AEC File Offset: 0x00370CEC
		public unsafe void SetStepSoundToPlay(string stepSoundToPlay, DataContext context)
		{
			this._stepSoundToPlay = stepSoundToPlay;
			base.SetModifiedAndInvalidateInfluencedCache(97, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int elementsCount = this._stepSoundToPlay.Length;
				int contentSize = 2 * elementsCount;
				int dataSize = 4 + contentSize;
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 28, dataSize);
				*(int*)pData = contentSize;
				pData += 4;
				string stepSoundToPlay2 = this._stepSoundToPlay;
				char* ptr;
				if (stepSoundToPlay2 == null)
				{
					ptr = null;
				}
				else
				{
					fixed (char* ptr2 = stepSoundToPlay2.GetPinnableReference())
					{
						ptr = ptr2;
					}
				}
				char* pChar = ptr;
				for (int i = 0; i < elementsCount; i++)
				{
					*(short*)(pData + (IntPtr)i * 2) = (short)pChar[i];
				}
				char* ptr2 = null;
				pData += contentSize;
			}
		}

		// Token: 0x0600616B RID: 24939 RVA: 0x00372BB0 File Offset: 0x00370DB0
		public string GetDieSoundToPlay()
		{
			return this._dieSoundToPlay;
		}

		// Token: 0x0600616C RID: 24940 RVA: 0x00372BC8 File Offset: 0x00370DC8
		public unsafe void SetDieSoundToPlay(string dieSoundToPlay, DataContext context)
		{
			this._dieSoundToPlay = dieSoundToPlay;
			base.SetModifiedAndInvalidateInfluencedCache(98, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int elementsCount = this._dieSoundToPlay.Length;
				int contentSize = 2 * elementsCount;
				int dataSize = 4 + contentSize;
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 29, dataSize);
				*(int*)pData = contentSize;
				pData += 4;
				string dieSoundToPlay2 = this._dieSoundToPlay;
				char* ptr;
				if (dieSoundToPlay2 == null)
				{
					ptr = null;
				}
				else
				{
					fixed (char* ptr2 = dieSoundToPlay2.GetPinnableReference())
					{
						ptr = ptr2;
					}
				}
				char* pChar = ptr;
				for (int i = 0; i < elementsCount; i++)
				{
					*(short*)(pData + (IntPtr)i * 2) = (short)pChar[i];
				}
				char* ptr2 = null;
				pData += contentSize;
			}
		}

		// Token: 0x0600616D RID: 24941 RVA: 0x00372C8C File Offset: 0x00370E8C
		public string GetSoundToLoop()
		{
			return this._soundToLoop;
		}

		// Token: 0x0600616E RID: 24942 RVA: 0x00372CA4 File Offset: 0x00370EA4
		public unsafe void SetSoundToLoop(string soundToLoop, DataContext context)
		{
			this._soundToLoop = soundToLoop;
			base.SetModifiedAndInvalidateInfluencedCache(99, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int elementsCount = this._soundToLoop.Length;
				int contentSize = 2 * elementsCount;
				int dataSize = 4 + contentSize;
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 30, dataSize);
				*(int*)pData = contentSize;
				pData += 4;
				string soundToLoop2 = this._soundToLoop;
				char* ptr;
				if (soundToLoop2 == null)
				{
					ptr = null;
				}
				else
				{
					fixed (char* ptr2 = soundToLoop2.GetPinnableReference())
					{
						ptr = ptr2;
					}
				}
				char* pChar = ptr;
				for (int i = 0; i < elementsCount; i++)
				{
					*(short*)(pData + (IntPtr)i * 2) = (short)pChar[i];
				}
				char* ptr2 = null;
				pData += contentSize;
			}
		}

		// Token: 0x0600616F RID: 24943 RVA: 0x00372D68 File Offset: 0x00370F68
		public sbyte GetBossPhase()
		{
			return this._bossPhase;
		}

		// Token: 0x06006170 RID: 24944 RVA: 0x00372D80 File Offset: 0x00370F80
		public unsafe void SetBossPhase(sbyte bossPhase, DataContext context)
		{
			this._bossPhase = bossPhase;
			base.SetModifiedAndInvalidateInfluencedCache(100, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				byte* pData = OperationAdder.DynamicObjectCollection_SetFixedField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 582U, 1);
				*pData = (byte)this._bossPhase;
				pData++;
			}
		}

		// Token: 0x06006171 RID: 24945 RVA: 0x00372DE4 File Offset: 0x00370FE4
		public sbyte GetAnimalAttackCount()
		{
			return this._animalAttackCount;
		}

		// Token: 0x06006172 RID: 24946 RVA: 0x00372DFC File Offset: 0x00370FFC
		public unsafe void SetAnimalAttackCount(sbyte animalAttackCount, DataContext context)
		{
			this._animalAttackCount = animalAttackCount;
			base.SetModifiedAndInvalidateInfluencedCache(101, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				byte* pData = OperationAdder.DynamicObjectCollection_SetFixedField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 583U, 1);
				*pData = (byte)this._animalAttackCount;
				pData++;
			}
		}

		// Token: 0x06006173 RID: 24947 RVA: 0x00372E60 File Offset: 0x00371060
		public bool GetShowTransferInjuryCommand()
		{
			return this._showTransferInjuryCommand;
		}

		// Token: 0x06006174 RID: 24948 RVA: 0x00372E78 File Offset: 0x00371078
		public unsafe void SetShowTransferInjuryCommand(bool showTransferInjuryCommand, DataContext context)
		{
			this._showTransferInjuryCommand = showTransferInjuryCommand;
			base.SetModifiedAndInvalidateInfluencedCache(102, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				byte* pData = OperationAdder.DynamicObjectCollection_SetFixedField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 584U, 1);
				*pData = (this._showTransferInjuryCommand ? 1 : 0);
				pData++;
			}
		}

		// Token: 0x06006175 RID: 24949 RVA: 0x00372EDC File Offset: 0x003710DC
		public List<sbyte> GetCurrTeammateCommands()
		{
			return this._currTeammateCommands;
		}

		// Token: 0x06006176 RID: 24950 RVA: 0x00372EF4 File Offset: 0x003710F4
		public unsafe void SetCurrTeammateCommands(List<sbyte> currTeammateCommands, DataContext context)
		{
			this._currTeammateCommands = currTeammateCommands;
			base.SetModifiedAndInvalidateInfluencedCache(103, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int elementsCount = this._currTeammateCommands.Count;
				int contentSize = elementsCount;
				int dataSize = 2 + contentSize;
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 31, dataSize);
				*(short*)pData = (short)((ushort)elementsCount);
				pData += 2;
				for (int i = 0; i < elementsCount; i++)
				{
					pData[i] = (byte)this._currTeammateCommands[i];
				}
				pData += contentSize;
			}
		}

		// Token: 0x06006177 RID: 24951 RVA: 0x00372F98 File Offset: 0x00371198
		public List<byte> GetTeammateCommandCdPercent()
		{
			return this._teammateCommandCdPercent;
		}

		// Token: 0x06006178 RID: 24952 RVA: 0x00372FB0 File Offset: 0x003711B0
		public unsafe void SetTeammateCommandCdPercent(List<byte> teammateCommandCdPercent, DataContext context)
		{
			this._teammateCommandCdPercent = teammateCommandCdPercent;
			base.SetModifiedAndInvalidateInfluencedCache(104, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int elementsCount = this._teammateCommandCdPercent.Count;
				int contentSize = elementsCount;
				int dataSize = 2 + contentSize;
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 32, dataSize);
				*(short*)pData = (short)((ushort)elementsCount);
				pData += 2;
				for (int i = 0; i < elementsCount; i++)
				{
					pData[i] = this._teammateCommandCdPercent[i];
				}
				pData += contentSize;
			}
		}

		// Token: 0x06006179 RID: 24953 RVA: 0x00373054 File Offset: 0x00371254
		public sbyte GetExecutingTeammateCommand()
		{
			return this._executingTeammateCommand;
		}

		// Token: 0x0600617A RID: 24954 RVA: 0x0037306C File Offset: 0x0037126C
		public unsafe void SetExecutingTeammateCommand(sbyte executingTeammateCommand, DataContext context)
		{
			this._executingTeammateCommand = executingTeammateCommand;
			base.SetModifiedAndInvalidateInfluencedCache(105, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				byte* pData = OperationAdder.DynamicObjectCollection_SetFixedField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 585U, 1);
				*pData = (byte)this._executingTeammateCommand;
				pData++;
			}
		}

		// Token: 0x0600617B RID: 24955 RVA: 0x003730D0 File Offset: 0x003712D0
		public bool GetVisible()
		{
			return this._visible;
		}

		// Token: 0x0600617C RID: 24956 RVA: 0x003730E8 File Offset: 0x003712E8
		public unsafe void SetVisible(bool visible, DataContext context)
		{
			this._visible = visible;
			base.SetModifiedAndInvalidateInfluencedCache(106, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				byte* pData = OperationAdder.DynamicObjectCollection_SetFixedField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 586U, 1);
				*pData = (this._visible ? 1 : 0);
				pData++;
			}
		}

		// Token: 0x0600617D RID: 24957 RVA: 0x0037314C File Offset: 0x0037134C
		public byte GetTeammateCommandPreparePercent()
		{
			return this._teammateCommandPreparePercent;
		}

		// Token: 0x0600617E RID: 24958 RVA: 0x00373164 File Offset: 0x00371364
		public unsafe void SetTeammateCommandPreparePercent(byte teammateCommandPreparePercent, DataContext context)
		{
			this._teammateCommandPreparePercent = teammateCommandPreparePercent;
			base.SetModifiedAndInvalidateInfluencedCache(107, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				byte* pData = OperationAdder.DynamicObjectCollection_SetFixedField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 587U, 1);
				*pData = this._teammateCommandPreparePercent;
				pData++;
			}
		}

		// Token: 0x0600617F RID: 24959 RVA: 0x003731C8 File Offset: 0x003713C8
		public byte GetTeammateCommandTimePercent()
		{
			return this._teammateCommandTimePercent;
		}

		// Token: 0x06006180 RID: 24960 RVA: 0x003731E0 File Offset: 0x003713E0
		public unsafe void SetTeammateCommandTimePercent(byte teammateCommandTimePercent, DataContext context)
		{
			this._teammateCommandTimePercent = teammateCommandTimePercent;
			base.SetModifiedAndInvalidateInfluencedCache(108, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				byte* pData = OperationAdder.DynamicObjectCollection_SetFixedField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 588U, 1);
				*pData = this._teammateCommandTimePercent;
				pData++;
			}
		}

		// Token: 0x06006181 RID: 24961 RVA: 0x00373244 File Offset: 0x00371444
		public ItemKey GetAttackCommandWeaponKey()
		{
			return this._attackCommandWeaponKey;
		}

		// Token: 0x06006182 RID: 24962 RVA: 0x0037325C File Offset: 0x0037145C
		public unsafe void SetAttackCommandWeaponKey(ItemKey attackCommandWeaponKey, DataContext context)
		{
			this._attackCommandWeaponKey = attackCommandWeaponKey;
			base.SetModifiedAndInvalidateInfluencedCache(109, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				byte* pData = OperationAdder.DynamicObjectCollection_SetFixedField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 589U, 8);
				pData += this._attackCommandWeaponKey.Serialize(pData);
			}
		}

		// Token: 0x06006183 RID: 24963 RVA: 0x003732C4 File Offset: 0x003714C4
		public sbyte GetAttackCommandTrickType()
		{
			return this._attackCommandTrickType;
		}

		// Token: 0x06006184 RID: 24964 RVA: 0x003732DC File Offset: 0x003714DC
		public unsafe void SetAttackCommandTrickType(sbyte attackCommandTrickType, DataContext context)
		{
			this._attackCommandTrickType = attackCommandTrickType;
			base.SetModifiedAndInvalidateInfluencedCache(110, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				byte* pData = OperationAdder.DynamicObjectCollection_SetFixedField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 597U, 1);
				*pData = (byte)this._attackCommandTrickType;
				pData++;
			}
		}

		// Token: 0x06006185 RID: 24965 RVA: 0x00373340 File Offset: 0x00371540
		public short GetDefendCommandSkillId()
		{
			return this._defendCommandSkillId;
		}

		// Token: 0x06006186 RID: 24966 RVA: 0x00373358 File Offset: 0x00371558
		public unsafe void SetDefendCommandSkillId(short defendCommandSkillId, DataContext context)
		{
			this._defendCommandSkillId = defendCommandSkillId;
			base.SetModifiedAndInvalidateInfluencedCache(111, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				byte* pData = OperationAdder.DynamicObjectCollection_SetFixedField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 598U, 2);
				*(short*)pData = this._defendCommandSkillId;
				pData += 2;
			}
		}

		// Token: 0x06006187 RID: 24967 RVA: 0x003733BC File Offset: 0x003715BC
		public sbyte GetShowEffectCommandIndex()
		{
			return this._showEffectCommandIndex;
		}

		// Token: 0x06006188 RID: 24968 RVA: 0x003733D4 File Offset: 0x003715D4
		public unsafe void SetShowEffectCommandIndex(sbyte showEffectCommandIndex, DataContext context)
		{
			this._showEffectCommandIndex = showEffectCommandIndex;
			base.SetModifiedAndInvalidateInfluencedCache(112, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				byte* pData = OperationAdder.DynamicObjectCollection_SetFixedField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 600U, 1);
				*pData = (byte)this._showEffectCommandIndex;
				pData++;
			}
		}

		// Token: 0x06006189 RID: 24969 RVA: 0x00373438 File Offset: 0x00371638
		public short GetAttackCommandSkillId()
		{
			return this._attackCommandSkillId;
		}

		// Token: 0x0600618A RID: 24970 RVA: 0x00373450 File Offset: 0x00371650
		public unsafe void SetAttackCommandSkillId(short attackCommandSkillId, DataContext context)
		{
			this._attackCommandSkillId = attackCommandSkillId;
			base.SetModifiedAndInvalidateInfluencedCache(113, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				byte* pData = OperationAdder.DynamicObjectCollection_SetFixedField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 601U, 2);
				*(short*)pData = this._attackCommandSkillId;
				pData += 2;
			}
		}

		// Token: 0x0600618B RID: 24971 RVA: 0x003734B4 File Offset: 0x003716B4
		public List<SByteList> GetTeammateCommandBanReasons()
		{
			return this._teammateCommandBanReasons;
		}

		// Token: 0x0600618C RID: 24972 RVA: 0x003734CC File Offset: 0x003716CC
		public unsafe void SetTeammateCommandBanReasons(List<SByteList> teammateCommandBanReasons, DataContext context)
		{
			this._teammateCommandBanReasons = teammateCommandBanReasons;
			base.SetModifiedAndInvalidateInfluencedCache(114, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = 2;
				int elementsCount = this._teammateCommandBanReasons.Count;
				for (int i = 0; i < elementsCount; i++)
				{
					dataSize += this._teammateCommandBanReasons[i].GetSerializedSize();
				}
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 33, dataSize);
				*(short*)pData = (short)((ushort)elementsCount);
				pData += 2;
				for (int j = 0; j < elementsCount; j++)
				{
					pData += this._teammateCommandBanReasons[j].Serialize(pData);
				}
			}
		}

		// Token: 0x0600618D RID: 24973 RVA: 0x0037359C File Offset: 0x0037179C
		public short GetTargetDistance()
		{
			return this._targetDistance;
		}

		// Token: 0x0600618E RID: 24974 RVA: 0x003735B4 File Offset: 0x003717B4
		public unsafe void SetTargetDistance(short targetDistance, DataContext context)
		{
			this._targetDistance = targetDistance;
			base.SetModifiedAndInvalidateInfluencedCache(115, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				byte* pData = OperationAdder.DynamicObjectCollection_SetFixedField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 603U, 2);
				*(short*)pData = this._targetDistance;
				pData += 2;
			}
		}

		// Token: 0x0600618F RID: 24975 RVA: 0x00373618 File Offset: 0x00371818
		public InjuryAutoHealCollection GetOldInjuryAutoHealCollection()
		{
			return this._oldInjuryAutoHealCollection;
		}

		// Token: 0x06006190 RID: 24976 RVA: 0x00373630 File Offset: 0x00371830
		public unsafe void SetOldInjuryAutoHealCollection(InjuryAutoHealCollection oldInjuryAutoHealCollection, DataContext context)
		{
			this._oldInjuryAutoHealCollection = oldInjuryAutoHealCollection;
			base.SetModifiedAndInvalidateInfluencedCache(116, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._oldInjuryAutoHealCollection.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 34, dataSize);
				pData += this._oldInjuryAutoHealCollection.Serialize(pData);
			}
		}

		// Token: 0x06006191 RID: 24977 RVA: 0x003736A0 File Offset: 0x003718A0
		public MixPoisonAffectedCountCollection GetMixPoisonAffectedCount()
		{
			return this._mixPoisonAffectedCount;
		}

		// Token: 0x06006192 RID: 24978 RVA: 0x003736B8 File Offset: 0x003718B8
		public unsafe void SetMixPoisonAffectedCount(MixPoisonAffectedCountCollection mixPoisonAffectedCount, DataContext context)
		{
			this._mixPoisonAffectedCount = mixPoisonAffectedCount;
			base.SetModifiedAndInvalidateInfluencedCache(117, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._mixPoisonAffectedCount.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 35, dataSize);
				pData += this._mixPoisonAffectedCount.Serialize(pData);
			}
		}

		// Token: 0x06006193 RID: 24979 RVA: 0x00373728 File Offset: 0x00371928
		public string GetParticleToLoopByCombatSkill()
		{
			return this._particleToLoopByCombatSkill;
		}

		// Token: 0x06006194 RID: 24980 RVA: 0x00373740 File Offset: 0x00371940
		public unsafe void SetParticleToLoopByCombatSkill(string particleToLoopByCombatSkill, DataContext context)
		{
			this._particleToLoopByCombatSkill = particleToLoopByCombatSkill;
			base.SetModifiedAndInvalidateInfluencedCache(118, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int elementsCount = this._particleToLoopByCombatSkill.Length;
				int contentSize = 2 * elementsCount;
				int dataSize = 4 + contentSize;
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 36, dataSize);
				*(int*)pData = contentSize;
				pData += 4;
				string particleToLoopByCombatSkill2 = this._particleToLoopByCombatSkill;
				char* ptr;
				if (particleToLoopByCombatSkill2 == null)
				{
					ptr = null;
				}
				else
				{
					fixed (char* ptr2 = particleToLoopByCombatSkill2.GetPinnableReference())
					{
						ptr = ptr2;
					}
				}
				char* pChar = ptr;
				for (int i = 0; i < elementsCount; i++)
				{
					*(short*)(pData + (IntPtr)i * 2) = (short)pChar[i];
				}
				char* ptr2 = null;
				pData += contentSize;
			}
		}

		// Token: 0x06006195 RID: 24981 RVA: 0x00373804 File Offset: 0x00371A04
		public SilenceFrameData GetNeiliAllocationCd()
		{
			return this._neiliAllocationCd;
		}

		// Token: 0x06006196 RID: 24982 RVA: 0x0037381C File Offset: 0x00371A1C
		public unsafe void SetNeiliAllocationCd(SilenceFrameData neiliAllocationCd, DataContext context)
		{
			this._neiliAllocationCd = neiliAllocationCd;
			base.SetModifiedAndInvalidateInfluencedCache(119, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				byte* pData = OperationAdder.DynamicObjectCollection_SetFixedField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 605U, 8);
				pData += this._neiliAllocationCd.Serialize(pData);
			}
		}

		// Token: 0x06006197 RID: 24983 RVA: 0x00373884 File Offset: 0x00371A84
		public NeiliProportionOfFiveElements GetProportionDelta()
		{
			return this._proportionDelta;
		}

		// Token: 0x06006198 RID: 24984 RVA: 0x0037389C File Offset: 0x00371A9C
		public unsafe void SetProportionDelta(NeiliProportionOfFiveElements proportionDelta, DataContext context)
		{
			this._proportionDelta = proportionDelta;
			base.SetModifiedAndInvalidateInfluencedCache(120, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				byte* pData = OperationAdder.DynamicObjectCollection_SetFixedField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 613U, 8);
				pData += this._proportionDelta.Serialize(pData);
			}
		}

		// Token: 0x06006199 RID: 24985 RVA: 0x00373904 File Offset: 0x00371B04
		public int GetMindMarkInfinityCount()
		{
			return this._mindMarkInfinityCount;
		}

		// Token: 0x0600619A RID: 24986 RVA: 0x0037391C File Offset: 0x00371B1C
		public unsafe void SetMindMarkInfinityCount(int mindMarkInfinityCount, DataContext context)
		{
			this._mindMarkInfinityCount = mindMarkInfinityCount;
			base.SetModifiedAndInvalidateInfluencedCache(121, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				byte* pData = OperationAdder.DynamicObjectCollection_SetFixedField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 621U, 4);
				*(int*)pData = this._mindMarkInfinityCount;
				pData += 4;
			}
		}

		// Token: 0x0600619B RID: 24987 RVA: 0x00373980 File Offset: 0x00371B80
		public int GetMindMarkInfinityProgress()
		{
			return this._mindMarkInfinityProgress;
		}

		// Token: 0x0600619C RID: 24988 RVA: 0x00373998 File Offset: 0x00371B98
		public unsafe void SetMindMarkInfinityProgress(int mindMarkInfinityProgress, DataContext context)
		{
			this._mindMarkInfinityProgress = mindMarkInfinityProgress;
			base.SetModifiedAndInvalidateInfluencedCache(122, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				byte* pData = OperationAdder.DynamicObjectCollection_SetFixedField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 625U, 4);
				*(int*)pData = this._mindMarkInfinityProgress;
				pData += 4;
			}
		}

		// Token: 0x0600619D RID: 24989 RVA: 0x003739FC File Offset: 0x00371BFC
		public List<TeammateCommandDisplayData> GetShowCommandList()
		{
			return this._showCommandList;
		}

		// Token: 0x0600619E RID: 24990 RVA: 0x00373A14 File Offset: 0x00371C14
		public unsafe void SetShowCommandList(List<TeammateCommandDisplayData> showCommandList, DataContext context)
		{
			this._showCommandList = showCommandList;
			base.SetModifiedAndInvalidateInfluencedCache(123, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int elementsCount = this._showCommandList.Count;
				int contentSize = 8 * elementsCount;
				int dataSize = 2 + contentSize;
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 37, dataSize);
				*(short*)pData = (short)((ushort)elementsCount);
				pData += 2;
				for (int i = 0; i < elementsCount; i++)
				{
					pData += this._showCommandList[i].Serialize(pData);
				}
			}
		}

		// Token: 0x0600619F RID: 24991 RVA: 0x00373ABC File Offset: 0x00371CBC
		public List<int> GetUnlockPrepareValue()
		{
			return this._unlockPrepareValue;
		}

		// Token: 0x060061A0 RID: 24992 RVA: 0x00373AD4 File Offset: 0x00371CD4
		public unsafe void SetUnlockPrepareValue(List<int> unlockPrepareValue, DataContext context)
		{
			this._unlockPrepareValue = unlockPrepareValue;
			base.SetModifiedAndInvalidateInfluencedCache(124, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int elementsCount = this._unlockPrepareValue.Count;
				int contentSize = 4 * elementsCount;
				int dataSize = 2 + contentSize;
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 38, dataSize);
				*(short*)pData = (short)((ushort)elementsCount);
				pData += 2;
				for (int i = 0; i < elementsCount; i++)
				{
					*(int*)(pData + (IntPtr)i * 4) = this._unlockPrepareValue[i];
				}
				pData += contentSize;
			}
		}

		// Token: 0x060061A1 RID: 24993 RVA: 0x00373B7C File Offset: 0x00371D7C
		public List<int> GetRawCreateEffects()
		{
			return this._rawCreateEffects;
		}

		// Token: 0x060061A2 RID: 24994 RVA: 0x00373B94 File Offset: 0x00371D94
		public unsafe void SetRawCreateEffects(List<int> rawCreateEffects, DataContext context)
		{
			this._rawCreateEffects = rawCreateEffects;
			base.SetModifiedAndInvalidateInfluencedCache(125, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int elementsCount = this._rawCreateEffects.Count;
				int contentSize = 4 * elementsCount;
				int dataSize = 2 + contentSize;
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 39, dataSize);
				*(short*)pData = (short)((ushort)elementsCount);
				pData += 2;
				for (int i = 0; i < elementsCount; i++)
				{
					*(int*)(pData + (IntPtr)i * 4) = this._rawCreateEffects[i];
				}
				pData += contentSize;
			}
		}

		// Token: 0x060061A3 RID: 24995 RVA: 0x00373C3C File Offset: 0x00371E3C
		public RawCreateCollection GetRawCreateCollection()
		{
			return this._rawCreateCollection;
		}

		// Token: 0x060061A4 RID: 24996 RVA: 0x00373C54 File Offset: 0x00371E54
		public unsafe void SetRawCreateCollection(RawCreateCollection rawCreateCollection, DataContext context)
		{
			this._rawCreateCollection = rawCreateCollection;
			base.SetModifiedAndInvalidateInfluencedCache(126, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				int dataSize = this._rawCreateCollection.GetSerializedSize();
				byte* pData = OperationAdder.DynamicObjectCollection_SetDynamicField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 40, dataSize);
				pData += this._rawCreateCollection.Serialize(pData);
			}
		}

		// Token: 0x060061A5 RID: 24997 RVA: 0x00373CC4 File Offset: 0x00371EC4
		public SilenceFrameData GetNormalAttackRecovery()
		{
			return this._normalAttackRecovery;
		}

		// Token: 0x060061A6 RID: 24998 RVA: 0x00373CDC File Offset: 0x00371EDC
		public unsafe void SetNormalAttackRecovery(SilenceFrameData normalAttackRecovery, DataContext context)
		{
			this._normalAttackRecovery = normalAttackRecovery;
			base.SetModifiedAndInvalidateInfluencedCache(127, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				byte* pData = OperationAdder.DynamicObjectCollection_SetFixedField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 629U, 8);
				pData += this._normalAttackRecovery.Serialize(pData);
			}
		}

		// Token: 0x060061A7 RID: 24999 RVA: 0x00373D44 File Offset: 0x00371F44
		public bool GetReserveNormalAttack()
		{
			return this._reserveNormalAttack;
		}

		// Token: 0x060061A8 RID: 25000 RVA: 0x00373D5C File Offset: 0x00371F5C
		public unsafe void SetReserveNormalAttack(bool reserveNormalAttack, DataContext context)
		{
			this._reserveNormalAttack = reserveNormalAttack;
			base.SetModifiedAndInvalidateInfluencedCache(128, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				byte* pData = OperationAdder.DynamicObjectCollection_SetFixedField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 637U, 1);
				*pData = (this._reserveNormalAttack ? 1 : 0);
				pData++;
			}
		}

		// Token: 0x060061A9 RID: 25001 RVA: 0x00373DC4 File Offset: 0x00371FC4
		public int GetGangqi()
		{
			return this._gangqi;
		}

		// Token: 0x060061AA RID: 25002 RVA: 0x00373DDC File Offset: 0x00371FDC
		public unsafe void SetGangqi(int gangqi, DataContext context)
		{
			this._gangqi = gangqi;
			base.SetModifiedAndInvalidateInfluencedCache(129, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				byte* pData = OperationAdder.DynamicObjectCollection_SetFixedField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 638U, 4);
				*(int*)pData = this._gangqi;
				pData += 4;
			}
		}

		// Token: 0x060061AB RID: 25003 RVA: 0x00373E44 File Offset: 0x00372044
		public int GetGangqiMax()
		{
			return this._gangqiMax;
		}

		// Token: 0x060061AC RID: 25004 RVA: 0x00373E5C File Offset: 0x0037205C
		public unsafe void SetGangqiMax(int gangqiMax, DataContext context)
		{
			this._gangqiMax = gangqiMax;
			base.SetModifiedAndInvalidateInfluencedCache(130, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				byte* pData = OperationAdder.DynamicObjectCollection_SetFixedField<int>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 642U, 4);
				*(int*)pData = this._gangqiMax;
				pData += 4;
			}
		}

		// Token: 0x060061AD RID: 25005 RVA: 0x00373EC4 File Offset: 0x003720C4
		public int GetMaxTrickCount()
		{
			ObjectCollectionDataStates dataStates = this.CollectionHelperData.DataStates;
			bool flag = dataStates.IsCached(this.DataStatesOffset, 131);
			int maxTrickCount;
			if (flag)
			{
				maxTrickCount = this._maxTrickCount;
			}
			else
			{
				this._maxTrickCount = this.CalcMaxTrickCount();
				dataStates.SetCached(this.DataStatesOffset, 131);
				maxTrickCount = this._maxTrickCount;
			}
			return maxTrickCount;
		}

		// Token: 0x060061AE RID: 25006 RVA: 0x00373F24 File Offset: 0x00372124
		public byte GetMobilityLevel()
		{
			ObjectCollectionDataStates dataStates = this.CollectionHelperData.DataStates;
			bool flag = dataStates.IsCached(this.DataStatesOffset, 132);
			byte mobilityLevel;
			if (flag)
			{
				mobilityLevel = this._mobilityLevel;
			}
			else
			{
				this._mobilityLevel = this.CalcMobilityLevel();
				dataStates.SetCached(this.DataStatesOffset, 132);
				mobilityLevel = this._mobilityLevel;
			}
			return mobilityLevel;
		}

		// Token: 0x060061AF RID: 25007 RVA: 0x00373F84 File Offset: 0x00372184
		public List<bool> GetTeammateCommandCanUse()
		{
			ObjectCollectionDataStates dataStates = this.CollectionHelperData.DataStates;
			bool flag = dataStates.IsCached(this.DataStatesOffset, 133);
			List<bool> teammateCommandCanUse;
			if (flag)
			{
				teammateCommandCanUse = this._teammateCommandCanUse;
			}
			else
			{
				this.CalcTeammateCommandCanUse(this._teammateCommandCanUse);
				dataStates.SetCached(this.DataStatesOffset, 133);
				teammateCommandCanUse = this._teammateCommandCanUse;
			}
			return teammateCommandCanUse;
		}

		// Token: 0x060061B0 RID: 25008 RVA: 0x00373FE8 File Offset: 0x003721E8
		public float GetChangeDistanceDuration()
		{
			ObjectCollectionDataStates dataStates = this.CollectionHelperData.DataStates;
			bool flag = dataStates.IsCached(this.DataStatesOffset, 134);
			float changeDistanceDuration;
			if (flag)
			{
				changeDistanceDuration = this._changeDistanceDuration;
			}
			else
			{
				this._changeDistanceDuration = this.CalcChangeDistanceDuration();
				dataStates.SetCached(this.DataStatesOffset, 134);
				changeDistanceDuration = this._changeDistanceDuration;
			}
			return changeDistanceDuration;
		}

		// Token: 0x060061B1 RID: 25009 RVA: 0x00374048 File Offset: 0x00372248
		public OuterAndInnerShorts GetAttackRange()
		{
			ObjectCollectionDataStates dataStates = this.CollectionHelperData.DataStates;
			bool flag = dataStates.IsCached(this.DataStatesOffset, 135);
			OuterAndInnerShorts attackRange;
			if (flag)
			{
				attackRange = this._attackRange;
			}
			else
			{
				this._attackRange = this.CalcAttackRange();
				dataStates.SetCached(this.DataStatesOffset, 135);
				attackRange = this._attackRange;
			}
			return attackRange;
		}

		// Token: 0x060061B2 RID: 25010 RVA: 0x003740A8 File Offset: 0x003722A8
		public sbyte GetHappiness()
		{
			ObjectCollectionDataStates dataStates = this.CollectionHelperData.DataStates;
			bool flag = dataStates.IsCached(this.DataStatesOffset, 136);
			sbyte happiness;
			if (flag)
			{
				happiness = this._happiness;
			}
			else
			{
				this._happiness = this.CalcHappiness();
				dataStates.SetCached(this.DataStatesOffset, 136);
				happiness = this._happiness;
			}
			return happiness;
		}

		// Token: 0x060061B3 RID: 25011 RVA: 0x00374108 File Offset: 0x00372308
		public SilenceData GetSilenceData()
		{
			ObjectCollectionDataStates dataStates = this.CollectionHelperData.DataStates;
			bool flag = dataStates.IsCached(this.DataStatesOffset, 137);
			SilenceData silenceData;
			if (flag)
			{
				silenceData = this._silenceData;
			}
			else
			{
				this.CalcSilenceData(this._silenceData);
				dataStates.SetCached(this.DataStatesOffset, 137);
				silenceData = this._silenceData;
			}
			return silenceData;
		}

		// Token: 0x060061B4 RID: 25012 RVA: 0x0037416C File Offset: 0x0037236C
		public int GetCombatStateTotalBuffPower()
		{
			ObjectCollectionDataStates dataStates = this.CollectionHelperData.DataStates;
			bool flag = dataStates.IsCached(this.DataStatesOffset, 138);
			int combatStateTotalBuffPower;
			if (flag)
			{
				combatStateTotalBuffPower = this._combatStateTotalBuffPower;
			}
			else
			{
				this._combatStateTotalBuffPower = this.CalcCombatStateTotalBuffPower();
				dataStates.SetCached(this.DataStatesOffset, 138);
				combatStateTotalBuffPower = this._combatStateTotalBuffPower;
			}
			return combatStateTotalBuffPower;
		}

		// Token: 0x060061B5 RID: 25013 RVA: 0x003741CC File Offset: 0x003723CC
		public HeavyOrBreakInjuryData GetHeavyOrBreakInjuryData()
		{
			ObjectCollectionDataStates dataStates = this.CollectionHelperData.DataStates;
			bool flag = dataStates.IsCached(this.DataStatesOffset, 139);
			HeavyOrBreakInjuryData heavyOrBreakInjuryData;
			if (flag)
			{
				heavyOrBreakInjuryData = this._heavyOrBreakInjuryData;
			}
			else
			{
				this._heavyOrBreakInjuryData = this.CalcHeavyOrBreakInjuryData();
				dataStates.SetCached(this.DataStatesOffset, 139);
				heavyOrBreakInjuryData = this._heavyOrBreakInjuryData;
			}
			return heavyOrBreakInjuryData;
		}

		// Token: 0x060061B6 RID: 25014 RVA: 0x0037422C File Offset: 0x0037242C
		public short GetMoveCd()
		{
			ObjectCollectionDataStates dataStates = this.CollectionHelperData.DataStates;
			bool flag = dataStates.IsCached(this.DataStatesOffset, 140);
			short moveCd;
			if (flag)
			{
				moveCd = this._moveCd;
			}
			else
			{
				this._moveCd = this.CalcMoveCd();
				dataStates.SetCached(this.DataStatesOffset, 140);
				moveCd = this._moveCd;
			}
			return moveCd;
		}

		// Token: 0x060061B7 RID: 25015 RVA: 0x0037428C File Offset: 0x0037248C
		public int GetMobilityRecoverSpeed()
		{
			ObjectCollectionDataStates dataStates = this.CollectionHelperData.DataStates;
			bool flag = dataStates.IsCached(this.DataStatesOffset, 141);
			int mobilityRecoverSpeed;
			if (flag)
			{
				mobilityRecoverSpeed = this._mobilityRecoverSpeed;
			}
			else
			{
				this._mobilityRecoverSpeed = this.CalcMobilityRecoverSpeed();
				dataStates.SetCached(this.DataStatesOffset, 141);
				mobilityRecoverSpeed = this._mobilityRecoverSpeed;
			}
			return mobilityRecoverSpeed;
		}

		// Token: 0x060061B8 RID: 25016 RVA: 0x003742EC File Offset: 0x003724EC
		public List<bool> GetCanUnlockAttack()
		{
			ObjectCollectionDataStates dataStates = this.CollectionHelperData.DataStates;
			bool flag = dataStates.IsCached(this.DataStatesOffset, 142);
			List<bool> canUnlockAttack;
			if (flag)
			{
				canUnlockAttack = this._canUnlockAttack;
			}
			else
			{
				this.CalcCanUnlockAttack(this._canUnlockAttack);
				dataStates.SetCached(this.DataStatesOffset, 142);
				canUnlockAttack = this._canUnlockAttack;
			}
			return canUnlockAttack;
		}

		// Token: 0x060061B9 RID: 25017 RVA: 0x00374350 File Offset: 0x00372550
		public List<ItemKey> GetValidItems()
		{
			ObjectCollectionDataStates dataStates = this.CollectionHelperData.DataStates;
			bool flag = dataStates.IsCached(this.DataStatesOffset, 143);
			List<ItemKey> validItems;
			if (flag)
			{
				validItems = this._validItems;
			}
			else
			{
				this.CalcValidItems(this._validItems);
				dataStates.SetCached(this.DataStatesOffset, 143);
				validItems = this._validItems;
			}
			return validItems;
		}

		// Token: 0x060061BA RID: 25018 RVA: 0x003743B4 File Offset: 0x003725B4
		public List<ItemKeyAndCount> GetValidItemAndCounts()
		{
			ObjectCollectionDataStates dataStates = this.CollectionHelperData.DataStates;
			bool flag = dataStates.IsCached(this.DataStatesOffset, 144);
			List<ItemKeyAndCount> validItemAndCounts;
			if (flag)
			{
				validItemAndCounts = this._validItemAndCounts;
			}
			else
			{
				this.CalcValidItemAndCounts(this._validItemAndCounts);
				dataStates.SetCached(this.DataStatesOffset, 144);
				validItemAndCounts = this._validItemAndCounts;
			}
			return validItemAndCounts;
		}

		// Token: 0x060061BB RID: 25019 RVA: 0x00374418 File Offset: 0x00372618
		public CombatCharacter()
		{
			this._weaponTricks = new sbyte[6];
			this._weapons = new ItemKey[7];
			this._tricks = new TrickCollection();
			this._injuryAutoHealCollection = new InjuryAutoHealCollection();
			this._damageStepCollection = new DamageStepCollection();
			this._outerDamageValue = new int[7];
			this._innerDamageValue = new int[7];
			this._outerDamageValueToShow = new IntPair[7];
			this._innerDamageValueToShow = new IntPair[7];
			this._flawCount = new byte[7];
			this._flawCollection = new FlawOrAcupointCollection();
			this._acupointCount = new byte[7];
			this._acupointCollection = new FlawOrAcupointCollection();
			this._mindMarkTime = new MindMarkList();
			this._defeatMarkCollection = new DefeatMarkCollection();
			this._neigongList = new List<short>();
			this._attackSkillList = new List<short>();
			this._agileSkillList = new List<short>();
			this._defenceSkillList = new List<short>();
			this._assistSkillList = new List<short>();
			this._otherActionCanUse = new bool[5];
			this._buffCombatStateCollection = new CombatStateCollection();
			this._debuffCombatStateCollection = new CombatStateCollection();
			this._specialCombatStateCollection = new CombatStateCollection();
			this._skillEffectCollection = new SkillEffectCollection();
			this._showEffectList = new ShowSpecialEffectCollection();
			this._animationToLoop = string.Empty;
			this._animationToPlayOnce = string.Empty;
			this._particleToPlay = string.Empty;
			this._particleToLoop = string.Empty;
			this._skillPetAnimation = string.Empty;
			this._petParticle = string.Empty;
			this._attackSoundToPlay = string.Empty;
			this._skillSoundToPlay = string.Empty;
			this._hitSoundToPlay = string.Empty;
			this._armorHitSoundToPlay = string.Empty;
			this._whooshSoundToPlay = string.Empty;
			this._shockSoundToPlay = string.Empty;
			this._stepSoundToPlay = string.Empty;
			this._dieSoundToPlay = string.Empty;
			this._soundToLoop = string.Empty;
			this._currTeammateCommands = new List<sbyte>();
			this._teammateCommandCdPercent = new List<byte>();
			this._teammateCommandBanReasons = new List<SByteList>();
			this._oldInjuryAutoHealCollection = new InjuryAutoHealCollection();
			this._mixPoisonAffectedCount = new MixPoisonAffectedCountCollection();
			this._particleToLoopByCombatSkill = string.Empty;
			this._showCommandList = new List<TeammateCommandDisplayData>();
			this._unlockPrepareValue = new List<int>();
			this._rawCreateEffects = new List<int>();
			this._rawCreateCollection = new RawCreateCollection();
			this._teammateCommandCanUse = new List<bool>();
			this._silenceData = new SilenceData();
			this._canUnlockAttack = new List<bool>();
			this._validItems = new List<ItemKey>();
			this._validItemAndCounts = new List<ItemKeyAndCount>();
		}

		// Token: 0x060061BC RID: 25020 RVA: 0x00374848 File Offset: 0x00372A48
		public bool IsSerializedSizeFixed()
		{
			return false;
		}

		// Token: 0x060061BD RID: 25021 RVA: 0x0037485C File Offset: 0x00372A5C
		public int GetSerializedSize()
		{
			int totalSize = 810;
			int dataSize = this._tricks.GetSerializedSize();
			totalSize += dataSize;
			int dataSize2 = this._injuryAutoHealCollection.GetSerializedSize();
			totalSize += dataSize2;
			int dataSize3 = this._flawCollection.GetSerializedSize();
			totalSize += dataSize3;
			int dataSize4 = this._acupointCollection.GetSerializedSize();
			totalSize += dataSize4;
			int dataSize5 = this._mindMarkTime.GetSerializedSize();
			totalSize += dataSize5;
			int dataSize6 = this._defeatMarkCollection.GetSerializedSize();
			totalSize += dataSize6;
			int elementsCount = this._neigongList.Count;
			int contentSize = 2 * elementsCount;
			int dataSize7 = 2 + contentSize;
			totalSize += dataSize7;
			int elementsCount2 = this._attackSkillList.Count;
			int contentSize2 = 2 * elementsCount2;
			int dataSize8 = 2 + contentSize2;
			totalSize += dataSize8;
			int elementsCount3 = this._agileSkillList.Count;
			int contentSize3 = 2 * elementsCount3;
			int dataSize9 = 2 + contentSize3;
			totalSize += dataSize9;
			int elementsCount4 = this._defenceSkillList.Count;
			int contentSize4 = 2 * elementsCount4;
			int dataSize10 = 2 + contentSize4;
			totalSize += dataSize10;
			int elementsCount5 = this._assistSkillList.Count;
			int contentSize5 = 2 * elementsCount5;
			int dataSize11 = 2 + contentSize5;
			totalSize += dataSize11;
			int dataSize12 = this._buffCombatStateCollection.GetSerializedSize();
			totalSize += dataSize12;
			int dataSize13 = this._debuffCombatStateCollection.GetSerializedSize();
			totalSize += dataSize13;
			int dataSize14 = this._specialCombatStateCollection.GetSerializedSize();
			totalSize += dataSize14;
			int dataSize15 = this._skillEffectCollection.GetSerializedSize();
			totalSize += dataSize15;
			int dataSize16 = this._showEffectList.GetSerializedSize();
			totalSize += dataSize16;
			int elementsCount6 = this._animationToLoop.Length;
			int contentSize6 = 2 * elementsCount6;
			int dataSize17 = 4 + contentSize6;
			totalSize += dataSize17;
			int elementsCount7 = this._animationToPlayOnce.Length;
			int contentSize7 = 2 * elementsCount7;
			int dataSize18 = 4 + contentSize7;
			totalSize += dataSize18;
			int elementsCount8 = this._particleToPlay.Length;
			int contentSize8 = 2 * elementsCount8;
			int dataSize19 = 4 + contentSize8;
			totalSize += dataSize19;
			int elementsCount9 = this._particleToLoop.Length;
			int contentSize9 = 2 * elementsCount9;
			int dataSize20 = 4 + contentSize9;
			totalSize += dataSize20;
			int elementsCount10 = this._skillPetAnimation.Length;
			int contentSize10 = 2 * elementsCount10;
			int dataSize21 = 4 + contentSize10;
			totalSize += dataSize21;
			int elementsCount11 = this._petParticle.Length;
			int contentSize11 = 2 * elementsCount11;
			int dataSize22 = 4 + contentSize11;
			totalSize += dataSize22;
			int elementsCount12 = this._attackSoundToPlay.Length;
			int contentSize12 = 2 * elementsCount12;
			int dataSize23 = 4 + contentSize12;
			totalSize += dataSize23;
			int elementsCount13 = this._skillSoundToPlay.Length;
			int contentSize13 = 2 * elementsCount13;
			int dataSize24 = 4 + contentSize13;
			totalSize += dataSize24;
			int elementsCount14 = this._hitSoundToPlay.Length;
			int contentSize14 = 2 * elementsCount14;
			int dataSize25 = 4 + contentSize14;
			totalSize += dataSize25;
			int elementsCount15 = this._armorHitSoundToPlay.Length;
			int contentSize15 = 2 * elementsCount15;
			int dataSize26 = 4 + contentSize15;
			totalSize += dataSize26;
			int elementsCount16 = this._whooshSoundToPlay.Length;
			int contentSize16 = 2 * elementsCount16;
			int dataSize27 = 4 + contentSize16;
			totalSize += dataSize27;
			int elementsCount17 = this._shockSoundToPlay.Length;
			int contentSize17 = 2 * elementsCount17;
			int dataSize28 = 4 + contentSize17;
			totalSize += dataSize28;
			int elementsCount18 = this._stepSoundToPlay.Length;
			int contentSize18 = 2 * elementsCount18;
			int dataSize29 = 4 + contentSize18;
			totalSize += dataSize29;
			int elementsCount19 = this._dieSoundToPlay.Length;
			int contentSize19 = 2 * elementsCount19;
			int dataSize30 = 4 + contentSize19;
			totalSize += dataSize30;
			int elementsCount20 = this._soundToLoop.Length;
			int contentSize20 = 2 * elementsCount20;
			int dataSize31 = 4 + contentSize20;
			totalSize += dataSize31;
			int elementsCount21 = this._currTeammateCommands.Count;
			int contentSize21 = elementsCount21;
			int dataSize32 = 2 + contentSize21;
			totalSize += dataSize32;
			int elementsCount22 = this._teammateCommandCdPercent.Count;
			int contentSize22 = elementsCount22;
			int dataSize33 = 2 + contentSize22;
			totalSize += dataSize33;
			int dataSize34 = 2;
			int elementsCount23 = this._teammateCommandBanReasons.Count;
			for (int i = 0; i < elementsCount23; i++)
			{
				dataSize34 += this._teammateCommandBanReasons[i].GetSerializedSize();
			}
			totalSize += dataSize34;
			int dataSize35 = this._oldInjuryAutoHealCollection.GetSerializedSize();
			totalSize += dataSize35;
			int dataSize36 = this._mixPoisonAffectedCount.GetSerializedSize();
			totalSize += dataSize36;
			int elementsCount24 = this._particleToLoopByCombatSkill.Length;
			int contentSize23 = 2 * elementsCount24;
			int dataSize37 = 4 + contentSize23;
			totalSize += dataSize37;
			int elementsCount25 = this._showCommandList.Count;
			int contentSize24 = 8 * elementsCount25;
			int dataSize38 = 2 + contentSize24;
			totalSize += dataSize38;
			int elementsCount26 = this._unlockPrepareValue.Count;
			int contentSize25 = 4 * elementsCount26;
			int dataSize39 = 2 + contentSize25;
			totalSize += dataSize39;
			int elementsCount27 = this._rawCreateEffects.Count;
			int contentSize26 = 4 * elementsCount27;
			int dataSize40 = 2 + contentSize26;
			totalSize += dataSize40;
			int dataSize41 = this._rawCreateCollection.GetSerializedSize();
			return totalSize + dataSize41;
		}

		// Token: 0x060061BE RID: 25022 RVA: 0x00374D10 File Offset: 0x00372F10
		public unsafe int Serialize(byte* pData)
		{
			*(int*)pData = this._id;
			byte* pCurrData = pData + 4;
			*(int*)pCurrData = this._breathValue;
			pCurrData += 4;
			*(int*)pCurrData = this._stanceValue;
			pCurrData += 4;
			pCurrData += this._neiliAllocation.Serialize(pCurrData);
			pCurrData += this._originNeiliAllocation.Serialize(pCurrData);
			pCurrData += this._neiliAllocationRecoverProgress.Serialize(pCurrData);
			*(short*)pCurrData = this._oldDisorderOfQi;
			pCurrData += 2;
			*pCurrData = (byte)this._neiliType;
			pCurrData++;
			pCurrData += this._avoidToShow.Serialize(pCurrData);
			*(int*)pCurrData = this._currentPosition;
			pCurrData += 4;
			*(int*)pCurrData = this._displayPosition;
			pCurrData += 4;
			*(int*)pCurrData = this._mobilityValue;
			pCurrData += 4;
			*pCurrData = (byte)this._jumpPrepareProgress;
			pCurrData++;
			*(short*)pCurrData = this._jumpPreparedDistance;
			pCurrData += 2;
			*(short*)pCurrData = this._mobilityLockEffectCount;
			pCurrData += 2;
			*(float*)pCurrData = this._jumpChangeDistanceDuration;
			pCurrData += 4;
			*(int*)pCurrData = this._usingWeaponIndex;
			pCurrData += 4;
			bool flag = this._weaponTricks.Length != 6;
			if (flag)
			{
				throw new Exception("Elements count of field _weaponTricks is not equal to declaration");
			}
			for (int i = 0; i < 6; i++)
			{
				pCurrData[i] = (byte)this._weaponTricks[i];
			}
			pCurrData += 6;
			*pCurrData = this._weaponTrickIndex;
			pCurrData++;
			bool flag2 = this._weapons.Length != 7;
			if (flag2)
			{
				throw new Exception("Elements count of field _weapons is not equal to declaration");
			}
			for (int j = 0; j < 7; j++)
			{
				pCurrData += this._weapons[j].Serialize(pCurrData);
			}
			*pCurrData = (byte)this._attackingTrickType;
			pCurrData++;
			*pCurrData = (this._canAttackOutRange ? 1 : 0);
			pCurrData++;
			*pCurrData = (byte)this._changeTrickProgress;
			pCurrData++;
			*(short*)pCurrData = this._changeTrickCount;
			pCurrData += 2;
			*pCurrData = (this._canChangeTrick ? 1 : 0);
			pCurrData++;
			*pCurrData = (this._changingTrick ? 1 : 0);
			pCurrData++;
			*pCurrData = (this._changeTrickAttack ? 1 : 0);
			pCurrData++;
			*pCurrData = (this._isFightBack ? 1 : 0);
			pCurrData++;
			pCurrData += this._injuries.Serialize(pCurrData);
			pCurrData += this._oldInjuries.Serialize(pCurrData);
			pCurrData += this._damageStepCollection.Serialize(pCurrData);
			bool flag3 = this._outerDamageValue.Length != 7;
			if (flag3)
			{
				throw new Exception("Elements count of field _outerDamageValue is not equal to declaration");
			}
			for (int k = 0; k < 7; k++)
			{
				*(int*)(pCurrData + (IntPtr)k * 4) = this._outerDamageValue[k];
			}
			pCurrData += 28;
			bool flag4 = this._innerDamageValue.Length != 7;
			if (flag4)
			{
				throw new Exception("Elements count of field _innerDamageValue is not equal to declaration");
			}
			for (int l = 0; l < 7; l++)
			{
				*(int*)(pCurrData + (IntPtr)l * 4) = this._innerDamageValue[l];
			}
			pCurrData += 28;
			*(int*)pCurrData = this._mindDamageValue;
			pCurrData += 4;
			*(int*)pCurrData = this._fatalDamageValue;
			pCurrData += 4;
			bool flag5 = this._outerDamageValueToShow.Length != 7;
			if (flag5)
			{
				throw new Exception("Elements count of field _outerDamageValueToShow is not equal to declaration");
			}
			for (int m = 0; m < 7; m++)
			{
				pCurrData += this._outerDamageValueToShow[m].Serialize(pCurrData);
			}
			bool flag6 = this._innerDamageValueToShow.Length != 7;
			if (flag6)
			{
				throw new Exception("Elements count of field _innerDamageValueToShow is not equal to declaration");
			}
			for (int n = 0; n < 7; n++)
			{
				pCurrData += this._innerDamageValueToShow[n].Serialize(pCurrData);
			}
			*(int*)pCurrData = this._mindDamageValueToShow;
			pCurrData += 4;
			*(int*)pCurrData = this._fatalDamageValueToShow;
			pCurrData += 4;
			bool flag7 = this._flawCount.Length != 7;
			if (flag7)
			{
				throw new Exception("Elements count of field _flawCount is not equal to declaration");
			}
			for (int i2 = 0; i2 < 7; i2++)
			{
				pCurrData[i2] = this._flawCount[i2];
			}
			pCurrData += 7;
			bool flag8 = this._acupointCount.Length != 7;
			if (flag8)
			{
				throw new Exception("Elements count of field _acupointCount is not equal to declaration");
			}
			for (int i3 = 0; i3 < 7; i3++)
			{
				pCurrData[i3] = this._acupointCount[i3];
			}
			pCurrData += 7;
			pCurrData += this._poison.Serialize(pCurrData);
			pCurrData += this._oldPoison.Serialize(pCurrData);
			pCurrData += this._poisonResist.Serialize(pCurrData);
			pCurrData += this._newPoisonsToShow.Serialize(pCurrData);
			*(short*)pCurrData = this._preparingSkillId;
			pCurrData += 2;
			*pCurrData = this._skillPreparePercent;
			pCurrData++;
			*(short*)pCurrData = this._performingSkillId;
			pCurrData += 2;
			*pCurrData = (this._autoCastingSkill ? 1 : 0);
			pCurrData++;
			*pCurrData = this._attackSkillAttackIndex;
			pCurrData++;
			*pCurrData = this._attackSkillPower;
			pCurrData++;
			*(short*)pCurrData = this._affectingMoveSkillId;
			pCurrData += 2;
			*(short*)pCurrData = this._affectingDefendSkillId;
			pCurrData += 2;
			*pCurrData = this._defendSkillTimePercent;
			pCurrData++;
			*(short*)pCurrData = this._wugCount;
			pCurrData += 2;
			*pCurrData = this._healInjuryCount;
			pCurrData++;
			*pCurrData = this._healPoisonCount;
			pCurrData++;
			bool flag9 = this._otherActionCanUse.Length != 5;
			if (flag9)
			{
				throw new Exception("Elements count of field _otherActionCanUse is not equal to declaration");
			}
			for (int i4 = 0; i4 < 5; i4++)
			{
				pCurrData[i4] = (this._otherActionCanUse[i4] ? 1 : 0);
			}
			pCurrData += 5;
			*pCurrData = (byte)this._preparingOtherAction;
			pCurrData++;
			*pCurrData = this._otherActionPreparePercent;
			pCurrData++;
			*pCurrData = (this._canSurrender ? 1 : 0);
			pCurrData++;
			*pCurrData = (this._canUseItem ? 1 : 0);
			pCurrData++;
			pCurrData += this._preparingItem.Serialize(pCurrData);
			*pCurrData = this._useItemPreparePercent;
			pCurrData++;
			pCurrData += this._combatReserveData.Serialize(pCurrData);
			*(short*)pCurrData = this._xiangshuEffectId;
			pCurrData += 2;
			*(int*)pCurrData = this._hazardValue;
			pCurrData += 4;
			*(float*)pCurrData = this._animationTimeScale;
			pCurrData += 4;
			*pCurrData = (this._attackOutOfRange ? 1 : 0);
			pCurrData++;
			*pCurrData = (byte)this._bossPhase;
			pCurrData++;
			*pCurrData = (byte)this._animalAttackCount;
			pCurrData++;
			*pCurrData = (this._showTransferInjuryCommand ? 1 : 0);
			pCurrData++;
			*pCurrData = (byte)this._executingTeammateCommand;
			pCurrData++;
			*pCurrData = (this._visible ? 1 : 0);
			pCurrData++;
			*pCurrData = this._teammateCommandPreparePercent;
			pCurrData++;
			*pCurrData = this._teammateCommandTimePercent;
			pCurrData++;
			pCurrData += this._attackCommandWeaponKey.Serialize(pCurrData);
			*pCurrData = (byte)this._attackCommandTrickType;
			pCurrData++;
			*(short*)pCurrData = this._defendCommandSkillId;
			pCurrData += 2;
			*pCurrData = (byte)this._showEffectCommandIndex;
			pCurrData++;
			*(short*)pCurrData = this._attackCommandSkillId;
			pCurrData += 2;
			*(short*)pCurrData = this._targetDistance;
			pCurrData += 2;
			pCurrData += this._neiliAllocationCd.Serialize(pCurrData);
			pCurrData += this._proportionDelta.Serialize(pCurrData);
			*(int*)pCurrData = this._mindMarkInfinityCount;
			pCurrData += 4;
			*(int*)pCurrData = this._mindMarkInfinityProgress;
			pCurrData += 4;
			pCurrData += this._normalAttackRecovery.Serialize(pCurrData);
			*pCurrData = (this._reserveNormalAttack ? 1 : 0);
			pCurrData++;
			*(int*)pCurrData = this._gangqi;
			pCurrData += 4;
			*(int*)pCurrData = this._gangqiMax;
			pCurrData += 4;
			byte* pBegin = pCurrData;
			pCurrData += 4;
			pCurrData += this._tricks.Serialize(pCurrData);
			int fieldSize = (int)((long)(pCurrData - pBegin) - 4L);
			bool flag10 = fieldSize > 4194304;
			if (flag10)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_tricks");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin = fieldSize;
			byte* pBegin2 = pCurrData;
			pCurrData += 4;
			pCurrData += this._injuryAutoHealCollection.Serialize(pCurrData);
			int fieldSize2 = (int)((long)(pCurrData - pBegin2) - 4L);
			bool flag11 = fieldSize2 > 4194304;
			if (flag11)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_injuryAutoHealCollection");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin2 = fieldSize2;
			byte* pBegin3 = pCurrData;
			pCurrData += 4;
			pCurrData += this._flawCollection.Serialize(pCurrData);
			int fieldSize3 = (int)((long)(pCurrData - pBegin3) - 4L);
			bool flag12 = fieldSize3 > 4194304;
			if (flag12)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_flawCollection");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin3 = fieldSize3;
			byte* pBegin4 = pCurrData;
			pCurrData += 4;
			pCurrData += this._acupointCollection.Serialize(pCurrData);
			int fieldSize4 = (int)((long)(pCurrData - pBegin4) - 4L);
			bool flag13 = fieldSize4 > 4194304;
			if (flag13)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_acupointCollection");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin4 = fieldSize4;
			byte* pBegin5 = pCurrData;
			pCurrData += 4;
			pCurrData += this._mindMarkTime.Serialize(pCurrData);
			int fieldSize5 = (int)((long)(pCurrData - pBegin5) - 4L);
			bool flag14 = fieldSize5 > 4194304;
			if (flag14)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_mindMarkTime");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin5 = fieldSize5;
			byte* pBegin6 = pCurrData;
			pCurrData += 4;
			pCurrData += this._defeatMarkCollection.Serialize(pCurrData);
			int fieldSize6 = (int)((long)(pCurrData - pBegin6) - 4L);
			bool flag15 = fieldSize6 > 4194304;
			if (flag15)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_defeatMarkCollection");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin6 = fieldSize6;
			int elementsCount = this._neigongList.Count;
			int contentSize = 2 * elementsCount;
			bool flag16 = contentSize > 4194300;
			if (flag16)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_neigongList");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pCurrData = contentSize + 2;
			pCurrData += 4;
			*(short*)pCurrData = (short)((ushort)elementsCount);
			pCurrData += 2;
			for (int i5 = 0; i5 < elementsCount; i5++)
			{
				*(short*)(pCurrData + (IntPtr)i5 * 2) = this._neigongList[i5];
			}
			pCurrData += contentSize;
			int elementsCount2 = this._attackSkillList.Count;
			int contentSize2 = 2 * elementsCount2;
			bool flag17 = contentSize2 > 4194300;
			if (flag17)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_attackSkillList");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pCurrData = contentSize2 + 2;
			pCurrData += 4;
			*(short*)pCurrData = (short)((ushort)elementsCount2);
			pCurrData += 2;
			for (int i6 = 0; i6 < elementsCount2; i6++)
			{
				*(short*)(pCurrData + (IntPtr)i6 * 2) = this._attackSkillList[i6];
			}
			pCurrData += contentSize2;
			int elementsCount3 = this._agileSkillList.Count;
			int contentSize3 = 2 * elementsCount3;
			bool flag18 = contentSize3 > 4194300;
			if (flag18)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_agileSkillList");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pCurrData = contentSize3 + 2;
			pCurrData += 4;
			*(short*)pCurrData = (short)((ushort)elementsCount3);
			pCurrData += 2;
			for (int i7 = 0; i7 < elementsCount3; i7++)
			{
				*(short*)(pCurrData + (IntPtr)i7 * 2) = this._agileSkillList[i7];
			}
			pCurrData += contentSize3;
			int elementsCount4 = this._defenceSkillList.Count;
			int contentSize4 = 2 * elementsCount4;
			bool flag19 = contentSize4 > 4194300;
			if (flag19)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_defenceSkillList");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pCurrData = contentSize4 + 2;
			pCurrData += 4;
			*(short*)pCurrData = (short)((ushort)elementsCount4);
			pCurrData += 2;
			for (int i8 = 0; i8 < elementsCount4; i8++)
			{
				*(short*)(pCurrData + (IntPtr)i8 * 2) = this._defenceSkillList[i8];
			}
			pCurrData += contentSize4;
			int elementsCount5 = this._assistSkillList.Count;
			int contentSize5 = 2 * elementsCount5;
			bool flag20 = contentSize5 > 4194300;
			if (flag20)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_assistSkillList");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pCurrData = contentSize5 + 2;
			pCurrData += 4;
			*(short*)pCurrData = (short)((ushort)elementsCount5);
			pCurrData += 2;
			for (int i9 = 0; i9 < elementsCount5; i9++)
			{
				*(short*)(pCurrData + (IntPtr)i9 * 2) = this._assistSkillList[i9];
			}
			pCurrData += contentSize5;
			byte* pBegin7 = pCurrData;
			pCurrData += 4;
			pCurrData += this._buffCombatStateCollection.Serialize(pCurrData);
			int fieldSize7 = (int)((long)(pCurrData - pBegin7) - 4L);
			bool flag21 = fieldSize7 > 4194304;
			if (flag21)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_buffCombatStateCollection");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin7 = fieldSize7;
			byte* pBegin8 = pCurrData;
			pCurrData += 4;
			pCurrData += this._debuffCombatStateCollection.Serialize(pCurrData);
			int fieldSize8 = (int)((long)(pCurrData - pBegin8) - 4L);
			bool flag22 = fieldSize8 > 4194304;
			if (flag22)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_debuffCombatStateCollection");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin8 = fieldSize8;
			byte* pBegin9 = pCurrData;
			pCurrData += 4;
			pCurrData += this._specialCombatStateCollection.Serialize(pCurrData);
			int fieldSize9 = (int)((long)(pCurrData - pBegin9) - 4L);
			bool flag23 = fieldSize9 > 4194304;
			if (flag23)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_specialCombatStateCollection");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin9 = fieldSize9;
			byte* pBegin10 = pCurrData;
			pCurrData += 4;
			pCurrData += this._skillEffectCollection.Serialize(pCurrData);
			int fieldSize10 = (int)((long)(pCurrData - pBegin10) - 4L);
			bool flag24 = fieldSize10 > 4194304;
			if (flag24)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_skillEffectCollection");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin10 = fieldSize10;
			byte* pBegin11 = pCurrData;
			pCurrData += 4;
			pCurrData += this._showEffectList.Serialize(pCurrData);
			int fieldSize11 = (int)((long)(pCurrData - pBegin11) - 4L);
			bool flag25 = fieldSize11 > 4194304;
			if (flag25)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_showEffectList");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin11 = fieldSize11;
			int elementsCount6 = this._animationToLoop.Length;
			int contentSize6 = 2 * elementsCount6;
			bool flag26 = contentSize6 > 4194300;
			if (flag26)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_animationToLoop");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pCurrData = contentSize6 + 4;
			pCurrData += 4;
			*(int*)pCurrData = contentSize6;
			pCurrData += 4;
			string animationToLoop = this._animationToLoop;
			char* ptr;
			if (animationToLoop == null)
			{
				ptr = null;
			}
			else
			{
				fixed (char* ptr2 = animationToLoop.GetPinnableReference())
				{
					ptr = ptr2;
				}
			}
			char* pChar = ptr;
			for (int i10 = 0; i10 < elementsCount6; i10++)
			{
				*(short*)(pCurrData + (IntPtr)i10 * 2) = (short)pChar[i10];
			}
			char* ptr2 = null;
			pCurrData += contentSize6;
			int elementsCount7 = this._animationToPlayOnce.Length;
			int contentSize7 = 2 * elementsCount7;
			bool flag27 = contentSize7 > 4194300;
			if (flag27)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_animationToPlayOnce");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pCurrData = contentSize7 + 4;
			pCurrData += 4;
			*(int*)pCurrData = contentSize7;
			pCurrData += 4;
			string animationToPlayOnce = this._animationToPlayOnce;
			char* ptr3;
			if (animationToPlayOnce == null)
			{
				ptr3 = null;
			}
			else
			{
				fixed (char* ptr4 = animationToPlayOnce.GetPinnableReference())
				{
					ptr3 = ptr4;
				}
			}
			char* pChar2 = ptr3;
			for (int i11 = 0; i11 < elementsCount7; i11++)
			{
				*(short*)(pCurrData + (IntPtr)i11 * 2) = (short)pChar2[i11];
			}
			char* ptr4 = null;
			pCurrData += contentSize7;
			int elementsCount8 = this._particleToPlay.Length;
			int contentSize8 = 2 * elementsCount8;
			bool flag28 = contentSize8 > 4194300;
			if (flag28)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_particleToPlay");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pCurrData = contentSize8 + 4;
			pCurrData += 4;
			*(int*)pCurrData = contentSize8;
			pCurrData += 4;
			string particleToPlay = this._particleToPlay;
			char* ptr5;
			if (particleToPlay == null)
			{
				ptr5 = null;
			}
			else
			{
				fixed (char* ptr6 = particleToPlay.GetPinnableReference())
				{
					ptr5 = ptr6;
				}
			}
			char* pChar3 = ptr5;
			for (int i12 = 0; i12 < elementsCount8; i12++)
			{
				*(short*)(pCurrData + (IntPtr)i12 * 2) = (short)pChar3[i12];
			}
			char* ptr6 = null;
			pCurrData += contentSize8;
			int elementsCount9 = this._particleToLoop.Length;
			int contentSize9 = 2 * elementsCount9;
			bool flag29 = contentSize9 > 4194300;
			if (flag29)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_particleToLoop");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pCurrData = contentSize9 + 4;
			pCurrData += 4;
			*(int*)pCurrData = contentSize9;
			pCurrData += 4;
			string particleToLoop = this._particleToLoop;
			char* ptr7;
			if (particleToLoop == null)
			{
				ptr7 = null;
			}
			else
			{
				fixed (char* ptr8 = particleToLoop.GetPinnableReference())
				{
					ptr7 = ptr8;
				}
			}
			char* pChar4 = ptr7;
			for (int i13 = 0; i13 < elementsCount9; i13++)
			{
				*(short*)(pCurrData + (IntPtr)i13 * 2) = (short)pChar4[i13];
			}
			char* ptr8 = null;
			pCurrData += contentSize9;
			int elementsCount10 = this._skillPetAnimation.Length;
			int contentSize10 = 2 * elementsCount10;
			bool flag30 = contentSize10 > 4194300;
			if (flag30)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_skillPetAnimation");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pCurrData = contentSize10 + 4;
			pCurrData += 4;
			*(int*)pCurrData = contentSize10;
			pCurrData += 4;
			string skillPetAnimation = this._skillPetAnimation;
			char* ptr9;
			if (skillPetAnimation == null)
			{
				ptr9 = null;
			}
			else
			{
				fixed (char* ptr10 = skillPetAnimation.GetPinnableReference())
				{
					ptr9 = ptr10;
				}
			}
			char* pChar5 = ptr9;
			for (int i14 = 0; i14 < elementsCount10; i14++)
			{
				*(short*)(pCurrData + (IntPtr)i14 * 2) = (short)pChar5[i14];
			}
			char* ptr10 = null;
			pCurrData += contentSize10;
			int elementsCount11 = this._petParticle.Length;
			int contentSize11 = 2 * elementsCount11;
			bool flag31 = contentSize11 > 4194300;
			if (flag31)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_petParticle");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pCurrData = contentSize11 + 4;
			pCurrData += 4;
			*(int*)pCurrData = contentSize11;
			pCurrData += 4;
			string petParticle = this._petParticle;
			char* ptr11;
			if (petParticle == null)
			{
				ptr11 = null;
			}
			else
			{
				fixed (char* ptr12 = petParticle.GetPinnableReference())
				{
					ptr11 = ptr12;
				}
			}
			char* pChar6 = ptr11;
			for (int i15 = 0; i15 < elementsCount11; i15++)
			{
				*(short*)(pCurrData + (IntPtr)i15 * 2) = (short)pChar6[i15];
			}
			char* ptr12 = null;
			pCurrData += contentSize11;
			int elementsCount12 = this._attackSoundToPlay.Length;
			int contentSize12 = 2 * elementsCount12;
			bool flag32 = contentSize12 > 4194300;
			if (flag32)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_attackSoundToPlay");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pCurrData = contentSize12 + 4;
			pCurrData += 4;
			*(int*)pCurrData = contentSize12;
			pCurrData += 4;
			string attackSoundToPlay = this._attackSoundToPlay;
			char* ptr13;
			if (attackSoundToPlay == null)
			{
				ptr13 = null;
			}
			else
			{
				fixed (char* ptr14 = attackSoundToPlay.GetPinnableReference())
				{
					ptr13 = ptr14;
				}
			}
			char* pChar7 = ptr13;
			for (int i16 = 0; i16 < elementsCount12; i16++)
			{
				*(short*)(pCurrData + (IntPtr)i16 * 2) = (short)pChar7[i16];
			}
			char* ptr14 = null;
			pCurrData += contentSize12;
			int elementsCount13 = this._skillSoundToPlay.Length;
			int contentSize13 = 2 * elementsCount13;
			bool flag33 = contentSize13 > 4194300;
			if (flag33)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_skillSoundToPlay");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pCurrData = contentSize13 + 4;
			pCurrData += 4;
			*(int*)pCurrData = contentSize13;
			pCurrData += 4;
			string skillSoundToPlay = this._skillSoundToPlay;
			char* ptr15;
			if (skillSoundToPlay == null)
			{
				ptr15 = null;
			}
			else
			{
				fixed (char* ptr16 = skillSoundToPlay.GetPinnableReference())
				{
					ptr15 = ptr16;
				}
			}
			char* pChar8 = ptr15;
			for (int i17 = 0; i17 < elementsCount13; i17++)
			{
				*(short*)(pCurrData + (IntPtr)i17 * 2) = (short)pChar8[i17];
			}
			char* ptr16 = null;
			pCurrData += contentSize13;
			int elementsCount14 = this._hitSoundToPlay.Length;
			int contentSize14 = 2 * elementsCount14;
			bool flag34 = contentSize14 > 4194300;
			if (flag34)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_hitSoundToPlay");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pCurrData = contentSize14 + 4;
			pCurrData += 4;
			*(int*)pCurrData = contentSize14;
			pCurrData += 4;
			string hitSoundToPlay = this._hitSoundToPlay;
			char* ptr17;
			if (hitSoundToPlay == null)
			{
				ptr17 = null;
			}
			else
			{
				fixed (char* ptr18 = hitSoundToPlay.GetPinnableReference())
				{
					ptr17 = ptr18;
				}
			}
			char* pChar9 = ptr17;
			for (int i18 = 0; i18 < elementsCount14; i18++)
			{
				*(short*)(pCurrData + (IntPtr)i18 * 2) = (short)pChar9[i18];
			}
			char* ptr18 = null;
			pCurrData += contentSize14;
			int elementsCount15 = this._armorHitSoundToPlay.Length;
			int contentSize15 = 2 * elementsCount15;
			bool flag35 = contentSize15 > 4194300;
			if (flag35)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_armorHitSoundToPlay");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pCurrData = contentSize15 + 4;
			pCurrData += 4;
			*(int*)pCurrData = contentSize15;
			pCurrData += 4;
			string armorHitSoundToPlay = this._armorHitSoundToPlay;
			char* ptr19;
			if (armorHitSoundToPlay == null)
			{
				ptr19 = null;
			}
			else
			{
				fixed (char* ptr20 = armorHitSoundToPlay.GetPinnableReference())
				{
					ptr19 = ptr20;
				}
			}
			char* pChar10 = ptr19;
			for (int i19 = 0; i19 < elementsCount15; i19++)
			{
				*(short*)(pCurrData + (IntPtr)i19 * 2) = (short)pChar10[i19];
			}
			char* ptr20 = null;
			pCurrData += contentSize15;
			int elementsCount16 = this._whooshSoundToPlay.Length;
			int contentSize16 = 2 * elementsCount16;
			bool flag36 = contentSize16 > 4194300;
			if (flag36)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_whooshSoundToPlay");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pCurrData = contentSize16 + 4;
			pCurrData += 4;
			*(int*)pCurrData = contentSize16;
			pCurrData += 4;
			string whooshSoundToPlay = this._whooshSoundToPlay;
			char* ptr21;
			if (whooshSoundToPlay == null)
			{
				ptr21 = null;
			}
			else
			{
				fixed (char* ptr22 = whooshSoundToPlay.GetPinnableReference())
				{
					ptr21 = ptr22;
				}
			}
			char* pChar11 = ptr21;
			for (int i20 = 0; i20 < elementsCount16; i20++)
			{
				*(short*)(pCurrData + (IntPtr)i20 * 2) = (short)pChar11[i20];
			}
			char* ptr22 = null;
			pCurrData += contentSize16;
			int elementsCount17 = this._shockSoundToPlay.Length;
			int contentSize17 = 2 * elementsCount17;
			bool flag37 = contentSize17 > 4194300;
			if (flag37)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_shockSoundToPlay");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pCurrData = contentSize17 + 4;
			pCurrData += 4;
			*(int*)pCurrData = contentSize17;
			pCurrData += 4;
			string shockSoundToPlay = this._shockSoundToPlay;
			char* ptr23;
			if (shockSoundToPlay == null)
			{
				ptr23 = null;
			}
			else
			{
				fixed (char* ptr24 = shockSoundToPlay.GetPinnableReference())
				{
					ptr23 = ptr24;
				}
			}
			char* pChar12 = ptr23;
			for (int i21 = 0; i21 < elementsCount17; i21++)
			{
				*(short*)(pCurrData + (IntPtr)i21 * 2) = (short)pChar12[i21];
			}
			char* ptr24 = null;
			pCurrData += contentSize17;
			int elementsCount18 = this._stepSoundToPlay.Length;
			int contentSize18 = 2 * elementsCount18;
			bool flag38 = contentSize18 > 4194300;
			if (flag38)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_stepSoundToPlay");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pCurrData = contentSize18 + 4;
			pCurrData += 4;
			*(int*)pCurrData = contentSize18;
			pCurrData += 4;
			string stepSoundToPlay = this._stepSoundToPlay;
			char* ptr25;
			if (stepSoundToPlay == null)
			{
				ptr25 = null;
			}
			else
			{
				fixed (char* ptr26 = stepSoundToPlay.GetPinnableReference())
				{
					ptr25 = ptr26;
				}
			}
			char* pChar13 = ptr25;
			for (int i22 = 0; i22 < elementsCount18; i22++)
			{
				*(short*)(pCurrData + (IntPtr)i22 * 2) = (short)pChar13[i22];
			}
			char* ptr26 = null;
			pCurrData += contentSize18;
			int elementsCount19 = this._dieSoundToPlay.Length;
			int contentSize19 = 2 * elementsCount19;
			bool flag39 = contentSize19 > 4194300;
			if (flag39)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_dieSoundToPlay");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pCurrData = contentSize19 + 4;
			pCurrData += 4;
			*(int*)pCurrData = contentSize19;
			pCurrData += 4;
			string dieSoundToPlay = this._dieSoundToPlay;
			char* ptr27;
			if (dieSoundToPlay == null)
			{
				ptr27 = null;
			}
			else
			{
				fixed (char* ptr28 = dieSoundToPlay.GetPinnableReference())
				{
					ptr27 = ptr28;
				}
			}
			char* pChar14 = ptr27;
			for (int i23 = 0; i23 < elementsCount19; i23++)
			{
				*(short*)(pCurrData + (IntPtr)i23 * 2) = (short)pChar14[i23];
			}
			char* ptr28 = null;
			pCurrData += contentSize19;
			int elementsCount20 = this._soundToLoop.Length;
			int contentSize20 = 2 * elementsCount20;
			bool flag40 = contentSize20 > 4194300;
			if (flag40)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_soundToLoop");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pCurrData = contentSize20 + 4;
			pCurrData += 4;
			*(int*)pCurrData = contentSize20;
			pCurrData += 4;
			string soundToLoop = this._soundToLoop;
			char* ptr29;
			if (soundToLoop == null)
			{
				ptr29 = null;
			}
			else
			{
				fixed (char* ptr30 = soundToLoop.GetPinnableReference())
				{
					ptr29 = ptr30;
				}
			}
			char* pChar15 = ptr29;
			for (int i24 = 0; i24 < elementsCount20; i24++)
			{
				*(short*)(pCurrData + (IntPtr)i24 * 2) = (short)pChar15[i24];
			}
			char* ptr30 = null;
			pCurrData += contentSize20;
			int elementsCount21 = this._currTeammateCommands.Count;
			int contentSize21 = elementsCount21;
			bool flag41 = contentSize21 > 4194300;
			if (flag41)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_currTeammateCommands");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pCurrData = contentSize21 + 2;
			pCurrData += 4;
			*(short*)pCurrData = (short)((ushort)elementsCount21);
			pCurrData += 2;
			for (int i25 = 0; i25 < elementsCount21; i25++)
			{
				pCurrData[i25] = (byte)this._currTeammateCommands[i25];
			}
			pCurrData += contentSize21;
			int elementsCount22 = this._teammateCommandCdPercent.Count;
			int contentSize22 = elementsCount22;
			bool flag42 = contentSize22 > 4194300;
			if (flag42)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_teammateCommandCdPercent");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pCurrData = contentSize22 + 2;
			pCurrData += 4;
			*(short*)pCurrData = (short)((ushort)elementsCount22);
			pCurrData += 2;
			for (int i26 = 0; i26 < elementsCount22; i26++)
			{
				pCurrData[i26] = this._teammateCommandCdPercent[i26];
			}
			pCurrData += contentSize22;
			int elementsCount23 = this._teammateCommandBanReasons.Count;
			byte* pBegin12 = pCurrData;
			pCurrData += 4;
			*(short*)pCurrData = (short)((ushort)elementsCount23);
			pCurrData += 2;
			for (int i27 = 0; i27 < elementsCount23; i27++)
			{
				pCurrData += this._teammateCommandBanReasons[i27].Serialize(pCurrData);
			}
			int fieldSize12 = (int)((long)(pCurrData - pBegin12) - 4L);
			bool flag43 = fieldSize12 > 4194304;
			if (flag43)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_teammateCommandBanReasons");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin12 = fieldSize12;
			byte* pBegin13 = pCurrData;
			pCurrData += 4;
			pCurrData += this._oldInjuryAutoHealCollection.Serialize(pCurrData);
			int fieldSize13 = (int)((long)(pCurrData - pBegin13) - 4L);
			bool flag44 = fieldSize13 > 4194304;
			if (flag44)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_oldInjuryAutoHealCollection");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin13 = fieldSize13;
			byte* pBegin14 = pCurrData;
			pCurrData += 4;
			pCurrData += this._mixPoisonAffectedCount.Serialize(pCurrData);
			int fieldSize14 = (int)((long)(pCurrData - pBegin14) - 4L);
			bool flag45 = fieldSize14 > 4194304;
			if (flag45)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_mixPoisonAffectedCount");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin14 = fieldSize14;
			int elementsCount24 = this._particleToLoopByCombatSkill.Length;
			int contentSize23 = 2 * elementsCount24;
			bool flag46 = contentSize23 > 4194300;
			if (flag46)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_particleToLoopByCombatSkill");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pCurrData = contentSize23 + 4;
			pCurrData += 4;
			*(int*)pCurrData = contentSize23;
			pCurrData += 4;
			string particleToLoopByCombatSkill = this._particleToLoopByCombatSkill;
			char* ptr31;
			if (particleToLoopByCombatSkill == null)
			{
				ptr31 = null;
			}
			else
			{
				fixed (char* ptr32 = particleToLoopByCombatSkill.GetPinnableReference())
				{
					ptr31 = ptr32;
				}
			}
			char* pChar16 = ptr31;
			for (int i28 = 0; i28 < elementsCount24; i28++)
			{
				*(short*)(pCurrData + (IntPtr)i28 * 2) = (short)pChar16[i28];
			}
			char* ptr32 = null;
			pCurrData += contentSize23;
			int elementsCount25 = this._showCommandList.Count;
			int contentSize24 = 8 * elementsCount25;
			bool flag47 = contentSize24 > 4194300;
			if (flag47)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_showCommandList");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pCurrData = contentSize24 + 2;
			pCurrData += 4;
			*(short*)pCurrData = (short)((ushort)elementsCount25);
			pCurrData += 2;
			for (int i29 = 0; i29 < elementsCount25; i29++)
			{
				pCurrData += this._showCommandList[i29].Serialize(pCurrData);
			}
			int elementsCount26 = this._unlockPrepareValue.Count;
			int contentSize25 = 4 * elementsCount26;
			bool flag48 = contentSize25 > 4194300;
			if (flag48)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_unlockPrepareValue");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pCurrData = contentSize25 + 2;
			pCurrData += 4;
			*(short*)pCurrData = (short)((ushort)elementsCount26);
			pCurrData += 2;
			for (int i30 = 0; i30 < elementsCount26; i30++)
			{
				*(int*)(pCurrData + (IntPtr)i30 * 4) = this._unlockPrepareValue[i30];
			}
			pCurrData += contentSize25;
			int elementsCount27 = this._rawCreateEffects.Count;
			int contentSize26 = 4 * elementsCount27;
			bool flag49 = contentSize26 > 4194300;
			if (flag49)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_rawCreateEffects");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pCurrData = contentSize26 + 2;
			pCurrData += 4;
			*(short*)pCurrData = (short)((ushort)elementsCount27);
			pCurrData += 2;
			for (int i31 = 0; i31 < elementsCount27; i31++)
			{
				*(int*)(pCurrData + (IntPtr)i31 * 4) = this._rawCreateEffects[i31];
			}
			pCurrData += contentSize26;
			byte* pBegin15 = pCurrData;
			pCurrData += 4;
			pCurrData += this._rawCreateCollection.Serialize(pCurrData);
			int fieldSize15 = (int)((long)(pCurrData - pBegin15) - 4L);
			bool flag50 = fieldSize15 > 4194304;
			if (flag50)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Size of field ");
				defaultInterpolatedStringHandler.AppendFormatted("_rawCreateCollection");
				defaultInterpolatedStringHandler.AppendLiteral(" must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			*(int*)pBegin15 = fieldSize15;
			return (int)((long)(pCurrData - pData));
		}

		// Token: 0x060061BF RID: 25023 RVA: 0x00377174 File Offset: 0x00375374
		public unsafe int Deserialize(byte* pData)
		{
			this._id = *(int*)pData;
			byte* pCurrData = pData + 4;
			this._breathValue = *(int*)pCurrData;
			pCurrData += 4;
			this._stanceValue = *(int*)pCurrData;
			pCurrData += 4;
			pCurrData += this._neiliAllocation.Deserialize(pCurrData);
			pCurrData += this._originNeiliAllocation.Deserialize(pCurrData);
			pCurrData += this._neiliAllocationRecoverProgress.Deserialize(pCurrData);
			this._oldDisorderOfQi = *(short*)pCurrData;
			pCurrData += 2;
			this._neiliType = *(sbyte*)pCurrData;
			pCurrData++;
			pCurrData += this._avoidToShow.Deserialize(pCurrData);
			this._currentPosition = *(int*)pCurrData;
			pCurrData += 4;
			this._displayPosition = *(int*)pCurrData;
			pCurrData += 4;
			this._mobilityValue = *(int*)pCurrData;
			pCurrData += 4;
			this._jumpPrepareProgress = *(sbyte*)pCurrData;
			pCurrData++;
			this._jumpPreparedDistance = *(short*)pCurrData;
			pCurrData += 2;
			this._mobilityLockEffectCount = *(short*)pCurrData;
			pCurrData += 2;
			this._jumpChangeDistanceDuration = *(float*)pCurrData;
			pCurrData += 4;
			this._usingWeaponIndex = *(int*)pCurrData;
			pCurrData += 4;
			bool flag = this._weaponTricks.Length != 6;
			if (flag)
			{
				throw new Exception("Elements count of field _weaponTricks is not equal to declaration");
			}
			for (int i = 0; i < 6; i++)
			{
				this._weaponTricks[i] = *(sbyte*)(pCurrData + i);
			}
			pCurrData += 6;
			this._weaponTrickIndex = *pCurrData;
			pCurrData++;
			bool flag2 = this._weapons.Length != 7;
			if (flag2)
			{
				throw new Exception("Elements count of field _weapons is not equal to declaration");
			}
			for (int j = 0; j < 7; j++)
			{
				ItemKey element = default(ItemKey);
				pCurrData += element.Deserialize(pCurrData);
				this._weapons[j] = element;
			}
			this._attackingTrickType = *(sbyte*)pCurrData;
			pCurrData++;
			this._canAttackOutRange = (*pCurrData != 0);
			pCurrData++;
			this._changeTrickProgress = *(sbyte*)pCurrData;
			pCurrData++;
			this._changeTrickCount = *(short*)pCurrData;
			pCurrData += 2;
			this._canChangeTrick = (*pCurrData != 0);
			pCurrData++;
			this._changingTrick = (*pCurrData != 0);
			pCurrData++;
			this._changeTrickAttack = (*pCurrData != 0);
			pCurrData++;
			this._isFightBack = (*pCurrData != 0);
			pCurrData++;
			pCurrData += this._injuries.Deserialize(pCurrData);
			pCurrData += this._oldInjuries.Deserialize(pCurrData);
			pCurrData += this._damageStepCollection.Deserialize(pCurrData);
			bool flag3 = this._outerDamageValue.Length != 7;
			if (flag3)
			{
				throw new Exception("Elements count of field _outerDamageValue is not equal to declaration");
			}
			for (int k = 0; k < 7; k++)
			{
				this._outerDamageValue[k] = *(int*)(pCurrData + (IntPtr)k * 4);
			}
			pCurrData += 28;
			bool flag4 = this._innerDamageValue.Length != 7;
			if (flag4)
			{
				throw new Exception("Elements count of field _innerDamageValue is not equal to declaration");
			}
			for (int l = 0; l < 7; l++)
			{
				this._innerDamageValue[l] = *(int*)(pCurrData + (IntPtr)l * 4);
			}
			pCurrData += 28;
			this._mindDamageValue = *(int*)pCurrData;
			pCurrData += 4;
			this._fatalDamageValue = *(int*)pCurrData;
			pCurrData += 4;
			bool flag5 = this._outerDamageValueToShow.Length != 7;
			if (flag5)
			{
				throw new Exception("Elements count of field _outerDamageValueToShow is not equal to declaration");
			}
			for (int m = 0; m < 7; m++)
			{
				IntPair element2 = default(IntPair);
				pCurrData += element2.Deserialize(pCurrData);
				this._outerDamageValueToShow[m] = element2;
			}
			bool flag6 = this._innerDamageValueToShow.Length != 7;
			if (flag6)
			{
				throw new Exception("Elements count of field _innerDamageValueToShow is not equal to declaration");
			}
			for (int n = 0; n < 7; n++)
			{
				IntPair element3 = default(IntPair);
				pCurrData += element3.Deserialize(pCurrData);
				this._innerDamageValueToShow[n] = element3;
			}
			this._mindDamageValueToShow = *(int*)pCurrData;
			pCurrData += 4;
			this._fatalDamageValueToShow = *(int*)pCurrData;
			pCurrData += 4;
			bool flag7 = this._flawCount.Length != 7;
			if (flag7)
			{
				throw new Exception("Elements count of field _flawCount is not equal to declaration");
			}
			for (int i2 = 0; i2 < 7; i2++)
			{
				this._flawCount[i2] = pCurrData[i2];
			}
			pCurrData += 7;
			bool flag8 = this._acupointCount.Length != 7;
			if (flag8)
			{
				throw new Exception("Elements count of field _acupointCount is not equal to declaration");
			}
			for (int i3 = 0; i3 < 7; i3++)
			{
				this._acupointCount[i3] = pCurrData[i3];
			}
			pCurrData += 7;
			pCurrData += this._poison.Deserialize(pCurrData);
			pCurrData += this._oldPoison.Deserialize(pCurrData);
			pCurrData += this._poisonResist.Deserialize(pCurrData);
			pCurrData += this._newPoisonsToShow.Deserialize(pCurrData);
			this._preparingSkillId = *(short*)pCurrData;
			pCurrData += 2;
			this._skillPreparePercent = *pCurrData;
			pCurrData++;
			this._performingSkillId = *(short*)pCurrData;
			pCurrData += 2;
			this._autoCastingSkill = (*pCurrData != 0);
			pCurrData++;
			this._attackSkillAttackIndex = *pCurrData;
			pCurrData++;
			this._attackSkillPower = *pCurrData;
			pCurrData++;
			this._affectingMoveSkillId = *(short*)pCurrData;
			pCurrData += 2;
			this._affectingDefendSkillId = *(short*)pCurrData;
			pCurrData += 2;
			this._defendSkillTimePercent = *pCurrData;
			pCurrData++;
			this._wugCount = *(short*)pCurrData;
			pCurrData += 2;
			this._healInjuryCount = *pCurrData;
			pCurrData++;
			this._healPoisonCount = *pCurrData;
			pCurrData++;
			bool flag9 = this._otherActionCanUse.Length != 5;
			if (flag9)
			{
				throw new Exception("Elements count of field _otherActionCanUse is not equal to declaration");
			}
			for (int i4 = 0; i4 < 5; i4++)
			{
				this._otherActionCanUse[i4] = (pCurrData[i4] != 0);
			}
			pCurrData += 5;
			this._preparingOtherAction = *(sbyte*)pCurrData;
			pCurrData++;
			this._otherActionPreparePercent = *pCurrData;
			pCurrData++;
			this._canSurrender = (*pCurrData != 0);
			pCurrData++;
			this._canUseItem = (*pCurrData != 0);
			pCurrData++;
			pCurrData += this._preparingItem.Deserialize(pCurrData);
			this._useItemPreparePercent = *pCurrData;
			pCurrData++;
			pCurrData += this._combatReserveData.Deserialize(pCurrData);
			this._xiangshuEffectId = *(short*)pCurrData;
			pCurrData += 2;
			this._hazardValue = *(int*)pCurrData;
			pCurrData += 4;
			this._animationTimeScale = *(float*)pCurrData;
			pCurrData += 4;
			this._attackOutOfRange = (*pCurrData != 0);
			pCurrData++;
			this._bossPhase = *(sbyte*)pCurrData;
			pCurrData++;
			this._animalAttackCount = *(sbyte*)pCurrData;
			pCurrData++;
			this._showTransferInjuryCommand = (*pCurrData != 0);
			pCurrData++;
			this._executingTeammateCommand = *(sbyte*)pCurrData;
			pCurrData++;
			this._visible = (*pCurrData != 0);
			pCurrData++;
			this._teammateCommandPreparePercent = *pCurrData;
			pCurrData++;
			this._teammateCommandTimePercent = *pCurrData;
			pCurrData++;
			pCurrData += this._attackCommandWeaponKey.Deserialize(pCurrData);
			this._attackCommandTrickType = *(sbyte*)pCurrData;
			pCurrData++;
			this._defendCommandSkillId = *(short*)pCurrData;
			pCurrData += 2;
			this._showEffectCommandIndex = *(sbyte*)pCurrData;
			pCurrData++;
			this._attackCommandSkillId = *(short*)pCurrData;
			pCurrData += 2;
			this._targetDistance = *(short*)pCurrData;
			pCurrData += 2;
			pCurrData += this._neiliAllocationCd.Deserialize(pCurrData);
			pCurrData += this._proportionDelta.Deserialize(pCurrData);
			this._mindMarkInfinityCount = *(int*)pCurrData;
			pCurrData += 4;
			this._mindMarkInfinityProgress = *(int*)pCurrData;
			pCurrData += 4;
			pCurrData += this._normalAttackRecovery.Deserialize(pCurrData);
			this._reserveNormalAttack = (*pCurrData != 0);
			pCurrData++;
			this._gangqi = *(int*)pCurrData;
			pCurrData += 4;
			this._gangqiMax = *(int*)pCurrData;
			pCurrData += 4;
			pCurrData += 4;
			pCurrData += this._tricks.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._injuryAutoHealCollection.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._flawCollection.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._acupointCollection.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._mindMarkTime.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._defeatMarkCollection.Deserialize(pCurrData);
			pCurrData += 4;
			ushort elementsCount = *(ushort*)pCurrData;
			pCurrData += 2;
			this._neigongList.Clear();
			for (int i5 = 0; i5 < (int)elementsCount; i5++)
			{
				this._neigongList.Add(*(short*)(pCurrData + (IntPtr)i5 * 2));
			}
			pCurrData += 2 * elementsCount;
			pCurrData += 4;
			ushort elementsCount2 = *(ushort*)pCurrData;
			pCurrData += 2;
			this._attackSkillList.Clear();
			for (int i6 = 0; i6 < (int)elementsCount2; i6++)
			{
				this._attackSkillList.Add(*(short*)(pCurrData + (IntPtr)i6 * 2));
			}
			pCurrData += 2 * elementsCount2;
			pCurrData += 4;
			ushort elementsCount3 = *(ushort*)pCurrData;
			pCurrData += 2;
			this._agileSkillList.Clear();
			for (int i7 = 0; i7 < (int)elementsCount3; i7++)
			{
				this._agileSkillList.Add(*(short*)(pCurrData + (IntPtr)i7 * 2));
			}
			pCurrData += 2 * elementsCount3;
			pCurrData += 4;
			ushort elementsCount4 = *(ushort*)pCurrData;
			pCurrData += 2;
			this._defenceSkillList.Clear();
			for (int i8 = 0; i8 < (int)elementsCount4; i8++)
			{
				this._defenceSkillList.Add(*(short*)(pCurrData + (IntPtr)i8 * 2));
			}
			pCurrData += 2 * elementsCount4;
			pCurrData += 4;
			ushort elementsCount5 = *(ushort*)pCurrData;
			pCurrData += 2;
			this._assistSkillList.Clear();
			for (int i9 = 0; i9 < (int)elementsCount5; i9++)
			{
				this._assistSkillList.Add(*(short*)(pCurrData + (IntPtr)i9 * 2));
			}
			pCurrData += 2 * elementsCount5;
			pCurrData += 4;
			pCurrData += this._buffCombatStateCollection.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._debuffCombatStateCollection.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._specialCombatStateCollection.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._skillEffectCollection.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._showEffectList.Deserialize(pCurrData);
			pCurrData += 4;
			uint contentSize = *(uint*)pCurrData;
			pCurrData += 4;
			this._animationToLoop = Encoding.Unicode.GetString(pCurrData, (int)contentSize);
			pCurrData += contentSize;
			pCurrData += 4;
			uint contentSize2 = *(uint*)pCurrData;
			pCurrData += 4;
			this._animationToPlayOnce = Encoding.Unicode.GetString(pCurrData, (int)contentSize2);
			pCurrData += contentSize2;
			pCurrData += 4;
			uint contentSize3 = *(uint*)pCurrData;
			pCurrData += 4;
			this._particleToPlay = Encoding.Unicode.GetString(pCurrData, (int)contentSize3);
			pCurrData += contentSize3;
			pCurrData += 4;
			uint contentSize4 = *(uint*)pCurrData;
			pCurrData += 4;
			this._particleToLoop = Encoding.Unicode.GetString(pCurrData, (int)contentSize4);
			pCurrData += contentSize4;
			pCurrData += 4;
			uint contentSize5 = *(uint*)pCurrData;
			pCurrData += 4;
			this._skillPetAnimation = Encoding.Unicode.GetString(pCurrData, (int)contentSize5);
			pCurrData += contentSize5;
			pCurrData += 4;
			uint contentSize6 = *(uint*)pCurrData;
			pCurrData += 4;
			this._petParticle = Encoding.Unicode.GetString(pCurrData, (int)contentSize6);
			pCurrData += contentSize6;
			pCurrData += 4;
			uint contentSize7 = *(uint*)pCurrData;
			pCurrData += 4;
			this._attackSoundToPlay = Encoding.Unicode.GetString(pCurrData, (int)contentSize7);
			pCurrData += contentSize7;
			pCurrData += 4;
			uint contentSize8 = *(uint*)pCurrData;
			pCurrData += 4;
			this._skillSoundToPlay = Encoding.Unicode.GetString(pCurrData, (int)contentSize8);
			pCurrData += contentSize8;
			pCurrData += 4;
			uint contentSize9 = *(uint*)pCurrData;
			pCurrData += 4;
			this._hitSoundToPlay = Encoding.Unicode.GetString(pCurrData, (int)contentSize9);
			pCurrData += contentSize9;
			pCurrData += 4;
			uint contentSize10 = *(uint*)pCurrData;
			pCurrData += 4;
			this._armorHitSoundToPlay = Encoding.Unicode.GetString(pCurrData, (int)contentSize10);
			pCurrData += contentSize10;
			pCurrData += 4;
			uint contentSize11 = *(uint*)pCurrData;
			pCurrData += 4;
			this._whooshSoundToPlay = Encoding.Unicode.GetString(pCurrData, (int)contentSize11);
			pCurrData += contentSize11;
			pCurrData += 4;
			uint contentSize12 = *(uint*)pCurrData;
			pCurrData += 4;
			this._shockSoundToPlay = Encoding.Unicode.GetString(pCurrData, (int)contentSize12);
			pCurrData += contentSize12;
			pCurrData += 4;
			uint contentSize13 = *(uint*)pCurrData;
			pCurrData += 4;
			this._stepSoundToPlay = Encoding.Unicode.GetString(pCurrData, (int)contentSize13);
			pCurrData += contentSize13;
			pCurrData += 4;
			uint contentSize14 = *(uint*)pCurrData;
			pCurrData += 4;
			this._dieSoundToPlay = Encoding.Unicode.GetString(pCurrData, (int)contentSize14);
			pCurrData += contentSize14;
			pCurrData += 4;
			uint contentSize15 = *(uint*)pCurrData;
			pCurrData += 4;
			this._soundToLoop = Encoding.Unicode.GetString(pCurrData, (int)contentSize15);
			pCurrData += contentSize15;
			pCurrData += 4;
			ushort elementsCount6 = *(ushort*)pCurrData;
			pCurrData += 2;
			this._currTeammateCommands.Clear();
			for (int i10 = 0; i10 < (int)elementsCount6; i10++)
			{
				this._currTeammateCommands.Add(*(sbyte*)(pCurrData + i10));
			}
			pCurrData += elementsCount6;
			pCurrData += 4;
			ushort elementsCount7 = *(ushort*)pCurrData;
			pCurrData += 2;
			this._teammateCommandCdPercent.Clear();
			for (int i11 = 0; i11 < (int)elementsCount7; i11++)
			{
				this._teammateCommandCdPercent.Add(pCurrData[i11]);
			}
			pCurrData += elementsCount7;
			pCurrData += 4;
			ushort elementsCount8 = *(ushort*)pCurrData;
			pCurrData += 2;
			this._teammateCommandBanReasons.Clear();
			for (int i12 = 0; i12 < (int)elementsCount8; i12++)
			{
				SByteList element4 = default(SByteList);
				pCurrData += element4.Deserialize(pCurrData);
				this._teammateCommandBanReasons.Add(element4);
			}
			pCurrData += 4;
			pCurrData += this._oldInjuryAutoHealCollection.Deserialize(pCurrData);
			pCurrData += 4;
			pCurrData += this._mixPoisonAffectedCount.Deserialize(pCurrData);
			pCurrData += 4;
			uint contentSize16 = *(uint*)pCurrData;
			pCurrData += 4;
			this._particleToLoopByCombatSkill = Encoding.Unicode.GetString(pCurrData, (int)contentSize16);
			pCurrData += contentSize16;
			pCurrData += 4;
			ushort elementsCount9 = *(ushort*)pCurrData;
			pCurrData += 2;
			this._showCommandList.Clear();
			for (int i13 = 0; i13 < (int)elementsCount9; i13++)
			{
				TeammateCommandDisplayData element5 = default(TeammateCommandDisplayData);
				pCurrData += element5.Deserialize(pCurrData);
				this._showCommandList.Add(element5);
			}
			pCurrData += 4;
			ushort elementsCount10 = *(ushort*)pCurrData;
			pCurrData += 2;
			this._unlockPrepareValue.Clear();
			for (int i14 = 0; i14 < (int)elementsCount10; i14++)
			{
				this._unlockPrepareValue.Add(*(int*)(pCurrData + (IntPtr)i14 * 4));
			}
			pCurrData += 4 * elementsCount10;
			pCurrData += 4;
			ushort elementsCount11 = *(ushort*)pCurrData;
			pCurrData += 2;
			this._rawCreateEffects.Clear();
			for (int i15 = 0; i15 < (int)elementsCount11; i15++)
			{
				this._rawCreateEffects.Add(*(int*)(pCurrData + (IntPtr)i15 * 4));
			}
			pCurrData += 4 * elementsCount11;
			pCurrData += 4;
			pCurrData += this._rawCreateCollection.Deserialize(pCurrData);
			return (int)((long)(pCurrData - pData));
		}

		// Token: 0x060061C2 RID: 25026 RVA: 0x00377F80 File Offset: 0x00376180
		[CompilerGenerated]
		internal static void <GenerateFlawOrAcupointRandomPool>g__AddToRandom|398_0(sbyte bodyPartKey, ref CombatCharacter.<>c__DisplayClass398_0 A_1)
		{
			List<ValueTuple<sbyte, int, int>> dataList;
			bool flag = !A_1.collection.BodyPartDict.TryGetValue(bodyPartKey, out dataList);
			if (!flag)
			{
				bool flag2 = dataList == null || dataList.Count <= 0;
				if (!flag2)
				{
					for (int i = 0; i < dataList.Count; i++)
					{
						ValueTuple<sbyte, int, int> entry = dataList[i];
						bool flag3 = A_1.onlyMaxLevel ? ((int)entry.Item1 != A_1.maxLevel) : ((int)entry.Item1 >= A_1.maxLevel);
						if (!flag3)
						{
							A_1.bodyPartRandomPool.Add(bodyPartKey);
							A_1.indexRandomPool.Add(i);
						}
					}
				}
			}
		}

		// Token: 0x0400194E RID: 6478
		private GameData.Domains.Character.Character _character;

		// Token: 0x0400194F RID: 6479
		private CombatDomain _combatDomain;

		// Token: 0x04001950 RID: 6480
		public readonly CombatCharacterStateMachine StateMachine = new CombatCharacterStateMachine();

		// Token: 0x04001951 RID: 6481
		public bool IsAlly;

		// Token: 0x04001952 RID: 6482
		public bool IsTaiwu;

		// Token: 0x04001953 RID: 6483
		[CollectionObjectField(false, true, false, true, false)]
		private int _id;

		// Token: 0x04001954 RID: 6484
		public int OriginXiangshuInfection;

		// Token: 0x04001955 RID: 6485
		public sbyte OriginNeiliType;

		// Token: 0x04001956 RID: 6486
		[CollectionObjectField(false, true, false, false, false)]
		private short _oldDisorderOfQi;

		// Token: 0x04001957 RID: 6487
		[CollectionObjectField(false, true, false, false, false)]
		private sbyte _neiliType;

		// Token: 0x04001958 RID: 6488
		[CollectionObjectField(false, true, false, false, false)]
		private NeiliProportionOfFiveElements _proportionDelta;

		// Token: 0x0400195B RID: 6491
		[CollectionObjectField(false, true, false, false, false)]
		private int _breathValue;

		// Token: 0x0400195C RID: 6492
		[CollectionObjectField(false, true, false, false, false)]
		private int _stanceValue;

		// Token: 0x0400195D RID: 6493
		public bool LockMaxBreath = false;

		// Token: 0x0400195E RID: 6494
		public bool LockMaxStance = false;

		// Token: 0x0400195F RID: 6495
		[CollectionObjectField(false, true, false, false, false)]
		private NeiliAllocation _neiliAllocation;

		// Token: 0x04001960 RID: 6496
		[CollectionObjectField(false, true, false, false, false)]
		private NeiliAllocation _originNeiliAllocation;

		// Token: 0x04001961 RID: 6497
		[CollectionObjectField(false, true, false, false, false)]
		private NeiliAllocation _neiliAllocationRecoverProgress;

		// Token: 0x04001962 RID: 6498
		[CollectionObjectField(false, true, false, false, false)]
		private SilenceFrameData _neiliAllocationCd;

		// Token: 0x04001963 RID: 6499
		private NeiliAllocation _originBaseNeiliAllocation;

		// Token: 0x04001964 RID: 6500
		public int[] NeiliAllocationAutoRecoverProgress = new int[4];

		// Token: 0x04001965 RID: 6501
		[CollectionObjectField(false, true, false, false, false)]
		private List<int> _unlockPrepareValue;

		// Token: 0x04001966 RID: 6502
		[CollectionObjectField(false, true, false, false, false)]
		private sbyte _changeTrickProgress;

		// Token: 0x04001967 RID: 6503
		[CollectionObjectField(false, true, false, false, false)]
		private short _changeTrickCount;

		// Token: 0x04001968 RID: 6504
		[CollectionObjectField(false, false, true, false, false)]
		private short _moveCd;

		// Token: 0x04001969 RID: 6505
		[CollectionObjectField(false, true, false, false, false)]
		private int _mobilityValue;

		// Token: 0x0400196A RID: 6506
		[CollectionObjectField(false, false, true, false, false)]
		private byte _mobilityLevel;

		// Token: 0x0400196B RID: 6507
		[CollectionObjectField(false, false, true, false, false)]
		private int _mobilityRecoverSpeed;

		// Token: 0x0400196C RID: 6508
		[CollectionObjectField(false, true, false, false, false)]
		private sbyte _jumpPrepareProgress;

		// Token: 0x0400196D RID: 6509
		[CollectionObjectField(false, true, false, false, false)]
		private short _jumpPreparedDistance;

		// Token: 0x0400196E RID: 6510
		[CollectionObjectField(false, true, false, false, false)]
		private int _usingWeaponIndex;

		// Token: 0x0400196F RID: 6511
		[CollectionObjectField(false, true, false, false, false, ArrayElementsCount = 6)]
		private sbyte[] _weaponTricks;

		// Token: 0x04001970 RID: 6512
		[CollectionObjectField(false, true, false, false, false)]
		private byte _weaponTrickIndex;

		// Token: 0x04001971 RID: 6513
		[CollectionObjectField(false, true, false, false, false, ArrayElementsCount = 7)]
		private ItemKey[] _weapons;

		// Token: 0x04001972 RID: 6514
		[CollectionObjectField(false, true, false, false, false)]
		private List<int> _rawCreateEffects;

		// Token: 0x04001973 RID: 6515
		[CollectionObjectField(false, true, false, false, false)]
		private RawCreateCollection _rawCreateCollection;

		// Token: 0x04001974 RID: 6516
		public readonly ItemKey[] Armors = new ItemKey[7];

		// Token: 0x04001975 RID: 6517
		public readonly Stack<ItemKey> ChangingDurabilityItems = new Stack<ItemKey>();

		// Token: 0x04001976 RID: 6518
		[CollectionObjectField(false, false, true, false, false)]
		private List<ItemKey> _validItems = new List<ItemKey>();

		// Token: 0x04001977 RID: 6519
		[CollectionObjectField(false, false, true, false, false)]
		private List<ItemKeyAndCount> _validItemAndCounts = new List<ItemKeyAndCount>();

		// Token: 0x04001978 RID: 6520
		[CollectionObjectField(false, true, false, false, false)]
		private List<short> _neigongList;

		// Token: 0x04001979 RID: 6521
		[CollectionObjectField(false, true, false, false, false)]
		private List<short> _attackSkillList;

		// Token: 0x0400197A RID: 6522
		[CollectionObjectField(false, true, false, false, false)]
		private List<short> _agileSkillList;

		// Token: 0x0400197B RID: 6523
		[CollectionObjectField(false, true, false, false, false)]
		private List<short> _defenceSkillList;

		// Token: 0x0400197C RID: 6524
		[CollectionObjectField(false, true, false, false, false)]
		private List<short> _assistSkillList;

		// Token: 0x0400197D RID: 6525
		[CollectionObjectField(false, true, false, false, false)]
		private TrickCollection _tricks;

		// Token: 0x0400197E RID: 6526
		[CollectionObjectField(false, false, true, false, false)]
		private int _maxTrickCount;

		// Token: 0x0400197F RID: 6527
		public readonly List<sbyte> InterchangeableTricks = new List<sbyte>();

		// Token: 0x04001980 RID: 6528
		[CollectionObjectField(false, true, false, false, false)]
		private short _wugCount;

		// Token: 0x04001981 RID: 6529
		[CollectionObjectField(false, true, false, false, false)]
		private Injuries _injuries;

		// Token: 0x04001982 RID: 6530
		[CollectionObjectField(false, true, false, false, false)]
		private Injuries _oldInjuries;

		// Token: 0x04001983 RID: 6531
		[CollectionObjectField(false, true, false, false, false)]
		private InjuryAutoHealCollection _injuryAutoHealCollection;

		// Token: 0x04001984 RID: 6532
		[CollectionObjectField(false, true, false, false, false)]
		private InjuryAutoHealCollection _oldInjuryAutoHealCollection;

		// Token: 0x04001985 RID: 6533
		[CollectionObjectField(false, false, true, false, false)]
		private HeavyOrBreakInjuryData _heavyOrBreakInjuryData;

		// Token: 0x04001986 RID: 6534
		public readonly List<short> OuterInjuryAutoHealSpeeds = new List<short>();

		// Token: 0x04001987 RID: 6535
		public readonly List<short> InnerInjuryAutoHealSpeeds = new List<short>();

		// Token: 0x04001988 RID: 6536
		public readonly List<short> OuterOldInjuryAutoHealSpeeds = new List<short>();

		// Token: 0x04001989 RID: 6537
		public readonly List<short> InnerOldInjuryAutoHealSpeeds = new List<short>();

		// Token: 0x0400198A RID: 6538
		[CollectionObjectField(false, true, false, false, false)]
		private PoisonInts _poison;

		// Token: 0x0400198B RID: 6539
		[CollectionObjectField(false, true, false, false, false)]
		private PoisonInts _oldPoison;

		// Token: 0x0400198C RID: 6540
		[CollectionObjectField(false, true, false, false, false)]
		private PoisonInts _poisonResist;

		// Token: 0x0400198D RID: 6541
		[CollectionObjectField(false, true, false, false, false)]
		private MixPoisonAffectedCountCollection _mixPoisonAffectedCount;

		// Token: 0x0400198E RID: 6542
		private readonly short[] _poisonAffectAccumulator = new short[6];

		// Token: 0x0400198F RID: 6543
		private DataUid _poisonResistUid;

		// Token: 0x04001990 RID: 6544
		[CollectionObjectField(false, true, false, false, false, ArrayElementsCount = 7)]
		private byte[] _flawCount;

		// Token: 0x04001991 RID: 6545
		[CollectionObjectField(false, true, false, false, false)]
		private FlawOrAcupointCollection _flawCollection;

		// Token: 0x04001992 RID: 6546
		[CollectionObjectField(false, true, false, false, false, ArrayElementsCount = 7)]
		private byte[] _acupointCount;

		// Token: 0x04001993 RID: 6547
		[CollectionObjectField(false, true, false, false, false)]
		private FlawOrAcupointCollection _acupointCollection;

		// Token: 0x04001994 RID: 6548
		[CollectionObjectField(false, true, false, false, false)]
		private MindMarkList _mindMarkTime;

		// Token: 0x04001995 RID: 6549
		[CollectionObjectField(false, true, false, false, false, ArrayElementsCount = 7)]
		private int[] _outerDamageValue;

		// Token: 0x04001996 RID: 6550
		[CollectionObjectField(false, true, false, false, false, ArrayElementsCount = 7)]
		private int[] _innerDamageValue;

		// Token: 0x04001997 RID: 6551
		[CollectionObjectField(false, true, false, false, false)]
		private int _mindDamageValue;

		// Token: 0x04001998 RID: 6552
		[CollectionObjectField(false, true, false, false, false)]
		private int _fatalDamageValue;

		// Token: 0x04001999 RID: 6553
		[CollectionObjectField(false, true, false, false, false)]
		private int _mindMarkInfinityCount;

		// Token: 0x0400199A RID: 6554
		[CollectionObjectField(false, true, false, false, false)]
		private int _mindMarkInfinityProgress;

		// Token: 0x0400199B RID: 6555
		[CollectionObjectField(false, true, false, false, false)]
		private DamageStepCollection _damageStepCollection;

		// Token: 0x0400199C RID: 6556
		[CollectionObjectField(false, true, false, false, false)]
		private DefeatMarkCollection _defeatMarkCollection;

		// Token: 0x0400199D RID: 6557
		private DataUid _defeatMarkUid;

		// Token: 0x0400199E RID: 6558
		public bool ForceDefeat = false;

		// Token: 0x0400199F RID: 6559
		public bool Immortal;

		// Token: 0x040019A0 RID: 6560
		[CollectionObjectField(false, true, false, false, false)]
		private int _gangqi;

		// Token: 0x040019A1 RID: 6561
		[CollectionObjectField(false, true, false, false, false)]
		private int _gangqiMax;

		// Token: 0x040019A2 RID: 6562
		public bool NeedReduceWeaponDurability;

		// Token: 0x040019A3 RID: 6563
		public bool NeedReduceArmorDurability;

		// Token: 0x040019A4 RID: 6564
		public bool BeCriticalDuringCalcAddInjury;

		// Token: 0x040019A5 RID: 6565
		public int BeCalcInjuryInnerRatio;

		// Token: 0x040019A6 RID: 6566
		[CollectionObjectField(false, true, false, false, false)]
		private short _targetDistance;

		// Token: 0x040019A7 RID: 6567
		[CollectionObjectField(false, true, false, false, false)]
		private short _mobilityLockEffectCount;

		// Token: 0x040019A8 RID: 6568
		[CollectionObjectField(false, false, true, false, false)]
		private float _changeDistanceDuration;

		// Token: 0x040019A9 RID: 6569
		[CollectionObjectField(false, true, false, false, false)]
		private float _jumpChangeDistanceDuration;

		// Token: 0x040019AA RID: 6570
		public bool KeepMoving;

		// Token: 0x040019AB RID: 6571
		public bool MoveForward;

		// Token: 0x040019AC RID: 6572
		public bool PlayerControllingMove;

		// Token: 0x040019AD RID: 6573
		public short AiTargetDistance;

		// Token: 0x040019AE RID: 6574
		public short PlayerTargetDistance;

		// Token: 0x040019AF RID: 6575
		public sbyte PlayerChangeTrickType;

		// Token: 0x040019B0 RID: 6576
		public sbyte PlayerChangeTrickBodyPart;

		// Token: 0x040019B1 RID: 6577
		public MoveData MoveData;

		// Token: 0x040019B2 RID: 6578
		public bool NeedPauseJumpMove;

		// Token: 0x040019B3 RID: 6579
		public short PauseJumpMoveSkillId;

		// Token: 0x040019B4 RID: 6580
		public int PauseJumpMoveDistance;

		// Token: 0x040019B5 RID: 6581
		[CollectionObjectField(false, true, false, false, false)]
		private SilenceFrameData _normalAttackRecovery;

		// Token: 0x040019B6 RID: 6582
		[CollectionObjectField(false, true, false, false, false)]
		private bool _canChangeTrick;

		// Token: 0x040019B7 RID: 6583
		[CollectionObjectField(false, true, false, false, false)]
		private bool _changingTrick;

		// Token: 0x040019B8 RID: 6584
		[CollectionObjectField(false, true, false, false, false)]
		private bool _changeTrickAttack;

		// Token: 0x040019B9 RID: 6585
		[CollectionObjectField(false, true, false, false, false)]
		private bool _isFightBack;

		// Token: 0x040019BA RID: 6586
		[CollectionObjectField(false, true, false, false, false)]
		private sbyte _attackingTrickType;

		// Token: 0x040019BB RID: 6587
		[CollectionObjectField(false, true, false, false, false)]
		private bool _canAttackOutRange;

		// Token: 0x040019BC RID: 6588
		[CollectionObjectField(false, false, true, false, false)]
		private List<bool> _canUnlockAttack = new List<bool>();

		// Token: 0x040019BD RID: 6589
		public byte PursueAttackCount;

		// Token: 0x040019BE RID: 6590
		public sbyte NormalAttackHitType;

		// Token: 0x040019BF RID: 6591
		public sbyte NormalAttackBodyPart;

		// Token: 0x040019C0 RID: 6592
		public sbyte ChangeTrickType;

		// Token: 0x040019C1 RID: 6593
		public sbyte ChangeTrickBodyPart;

		// Token: 0x040019C2 RID: 6594
		public bool NeedChangeTrickAttack;

		// Token: 0x040019C3 RID: 6595
		public EFlawOrAcupointType ChangeTrickFlawOrAcupointType;

		// Token: 0x040019C4 RID: 6596
		public int UnlockWeaponIndex;

		// Token: 0x040019C5 RID: 6597
		public sbyte FightBackHitType;

		// Token: 0x040019C6 RID: 6598
		public bool FightBackWithHit;

		// Token: 0x040019C7 RID: 6599
		public bool NeedUnlockAttack;

		// Token: 0x040019C8 RID: 6600
		public bool NeedNormalAttackImmediate;

		// Token: 0x040019C9 RID: 6601
		public int NeedNormalAttackSkipPrepare;

		// Token: 0x040019CA RID: 6602
		public bool NeedBreakAttack;

		// Token: 0x040019CB RID: 6603
		public bool IsBreakAttacking;

		// Token: 0x040019CC RID: 6604
		public int ForbidNormalAttackEffectCount;

		// Token: 0x040019CD RID: 6605
		public bool CanNormalAttackInPrepareSkill;

		// Token: 0x040019CE RID: 6606
		public byte NormalAttackLeftRepeatTimes;

		// Token: 0x040019CF RID: 6607
		public bool NormalAttackRepeatIsFightBack;

		// Token: 0x040019D0 RID: 6608
		public bool NextAttackNoPrepare;

		// Token: 0x040019D1 RID: 6609
		public bool NeedFreeAttack;

		// Token: 0x040019D2 RID: 6610
		public bool IsAutoNormalAttacking;

		// Token: 0x040019D3 RID: 6611
		public bool IsAutoNormalAttackingSpecial;

		// Token: 0x040019D4 RID: 6612
		[CollectionObjectField(false, true, false, false, false)]
		private short _preparingSkillId;

		// Token: 0x040019D5 RID: 6613
		[CollectionObjectField(false, true, false, false, false)]
		private byte _skillPreparePercent;

		// Token: 0x040019D6 RID: 6614
		[CollectionObjectField(false, true, false, false, false)]
		private short _performingSkillId;

		// Token: 0x040019D7 RID: 6615
		[CollectionObjectField(false, true, false, false, false)]
		private bool _autoCastingSkill;

		// Token: 0x040019D8 RID: 6616
		[CollectionObjectField(false, true, false, false, false)]
		private byte _attackSkillAttackIndex;

		// Token: 0x040019D9 RID: 6617
		[CollectionObjectField(false, true, false, false, false)]
		private byte _attackSkillPower;

		// Token: 0x040019DA RID: 6618
		public int SkillPrepareTotalProgress;

		// Token: 0x040019DB RID: 6619
		public int SkillPrepareCurrProgress;

		// Token: 0x040019DC RID: 6620
		public sbyte SkillAttackBodyPart;

		// Token: 0x040019DD RID: 6621
		public readonly sbyte[] SkillHitType = new sbyte[3];

		// Token: 0x040019DE RID: 6622
		public readonly int[] SkillHitValue = new int[3];

		// Token: 0x040019DF RID: 6623
		public readonly int[] SkillAvoidValue = new int[3];

		// Token: 0x040019E0 RID: 6624
		public int SkillFinalAttackHitIndex;

		// Token: 0x040019E1 RID: 6625
		[CollectionObjectField(false, true, false, false, false)]
		private short _affectingMoveSkillId;

		// Token: 0x040019E2 RID: 6626
		public short NeedAddEffectAgileSkillId;

		// Token: 0x040019E3 RID: 6627
		[CollectionObjectField(false, true, false, false, false)]
		private short _affectingDefendSkillId;

		// Token: 0x040019E4 RID: 6628
		[CollectionObjectField(false, true, false, false, false)]
		private byte _defendSkillTimePercent;

		// Token: 0x040019E5 RID: 6629
		public short DefendSkillTotalFrame;

		// Token: 0x040019E6 RID: 6630
		public short DefendSkillLeftFrame;

		// Token: 0x040019E7 RID: 6631
		[CollectionObjectField(false, true, false, false, false)]
		private ShowAvoidData _avoidToShow;

		// Token: 0x040019E8 RID: 6632
		[CollectionObjectField(false, true, false, false, false, ArrayElementsCount = 7)]
		private IntPair[] _outerDamageValueToShow;

		// Token: 0x040019E9 RID: 6633
		[CollectionObjectField(false, true, false, false, false, ArrayElementsCount = 7)]
		private IntPair[] _innerDamageValueToShow;

		// Token: 0x040019EA RID: 6634
		[CollectionObjectField(false, true, false, false, false)]
		private PoisonsAndLevels _newPoisonsToShow;

		// Token: 0x040019EB RID: 6635
		[CollectionObjectField(false, true, false, false, false)]
		private int _mindDamageValueToShow;

		// Token: 0x040019EC RID: 6636
		[CollectionObjectField(false, true, false, false, false)]
		private int _fatalDamageValueToShow;

		// Token: 0x040019ED RID: 6637
		[CollectionObjectField(false, true, false, false, false)]
		private int _currentPosition;

		// Token: 0x040019EE RID: 6638
		[CollectionObjectField(false, true, false, false, false)]
		private int _displayPosition;

		// Token: 0x040019EF RID: 6639
		[CollectionObjectField(false, true, false, false, false)]
		private short _xiangshuEffectId;

		// Token: 0x040019F0 RID: 6640
		[CollectionObjectField(false, true, false, false, false)]
		private ShowSpecialEffectCollection _showEffectList;

		// Token: 0x040019F1 RID: 6641
		[CollectionObjectField(false, true, false, false, false)]
		private List<TeammateCommandDisplayData> _showCommandList;

		// Token: 0x040019F2 RID: 6642
		[CollectionObjectField(false, true, false, false, false)]
		private string _animationToLoop;

		// Token: 0x040019F3 RID: 6643
		[CollectionObjectField(false, true, false, false, false)]
		private string _animationToPlayOnce;

		// Token: 0x040019F4 RID: 6644
		[CollectionObjectField(false, true, false, false, false)]
		private string _particleToPlay;

		// Token: 0x040019F5 RID: 6645
		[CollectionObjectField(false, true, false, false, false)]
		private string _particleToLoop;

		// Token: 0x040019F6 RID: 6646
		[CollectionObjectField(false, true, false, false, false)]
		private string _particleToLoopByCombatSkill;

		// Token: 0x040019F7 RID: 6647
		[CollectionObjectField(false, true, false, false, false)]
		private string _skillPetAnimation;

		// Token: 0x040019F8 RID: 6648
		[CollectionObjectField(false, true, false, false, false)]
		private string _petParticle;

		// Token: 0x040019F9 RID: 6649
		[CollectionObjectField(false, true, false, false, false)]
		private float _animationTimeScale;

		// Token: 0x040019FA RID: 6650
		[CollectionObjectField(false, true, false, false, false)]
		private bool _attackOutOfRange;

		// Token: 0x040019FB RID: 6651
		[CollectionObjectField(false, true, false, false, false)]
		private string _attackSoundToPlay;

		// Token: 0x040019FC RID: 6652
		[CollectionObjectField(false, true, false, false, false)]
		private string _skillSoundToPlay;

		// Token: 0x040019FD RID: 6653
		[CollectionObjectField(false, true, false, false, false)]
		private string _hitSoundToPlay;

		// Token: 0x040019FE RID: 6654
		[CollectionObjectField(false, true, false, false, false)]
		private string _armorHitSoundToPlay;

		// Token: 0x040019FF RID: 6655
		[CollectionObjectField(false, true, false, false, false)]
		private string _whooshSoundToPlay;

		// Token: 0x04001A00 RID: 6656
		[CollectionObjectField(false, true, false, false, false)]
		private string _shockSoundToPlay;

		// Token: 0x04001A01 RID: 6657
		[CollectionObjectField(false, true, false, false, false)]
		private string _stepSoundToPlay;

		// Token: 0x04001A02 RID: 6658
		[CollectionObjectField(false, true, false, false, false)]
		private string _dieSoundToPlay;

		// Token: 0x04001A03 RID: 6659
		[CollectionObjectField(false, true, false, false, false)]
		private string _soundToLoop;

		// Token: 0x04001A04 RID: 6660
		[CollectionObjectField(false, false, true, false, false)]
		private SilenceData _silenceData = new SilenceData();

		// Token: 0x04001A05 RID: 6661
		public readonly List<ShowSpecialEffectDisplayData> NeedShowEffectList = new List<ShowSpecialEffectDisplayData>();

		// Token: 0x04001A06 RID: 6662
		public readonly List<TeammateCommandDisplayData> NeedShowCommandList = new List<TeammateCommandDisplayData>();

		// Token: 0x04001A07 RID: 6663
		public string SpecialAnimationLoop;

		// Token: 0x04001A08 RID: 6664
		public bool NeedSelectMercyOption;

		// Token: 0x04001A09 RID: 6665
		public bool NeedDelaySettlement;

		// Token: 0x04001A0A RID: 6666
		public bool NeedEnterSpecialShow = false;

		// Token: 0x04001A0B RID: 6667
		[CollectionObjectField(false, true, false, false, false)]
		private sbyte _bossPhase;

		// Token: 0x04001A0C RID: 6668
		public bool NeedChangeBossPhase;

		// Token: 0x04001A0D RID: 6669
		public int ChangeBossPhaseEffectId;

		// Token: 0x04001A0E RID: 6670
		public bool CanCastSkillCostBreath;

		// Token: 0x04001A0F RID: 6671
		public bool CanCastSkillCostStance;

		// Token: 0x04001A10 RID: 6672
		public int PreventCastSkillEffectCount;

		// Token: 0x04001A11 RID: 6673
		public bool CanCastDirectSkill;

		// Token: 0x04001A12 RID: 6674
		public bool CanCastReverseSkill;

		// Token: 0x04001A13 RID: 6675
		public readonly List<short> CanCastDuringPrepareSkills = new List<short>();

		// Token: 0x04001A14 RID: 6676
		public readonly List<short> ForgetAfterCombatSkills = new List<short>();

		// Token: 0x04001A15 RID: 6677
		[CollectionObjectField(false, true, false, false, false)]
		private CombatReserveData _combatReserveData;

		// Token: 0x04001A16 RID: 6678
		[CollectionObjectField(false, true, false, false, false)]
		private bool _reserveNormalAttack;

		// Token: 0x04001A17 RID: 6679
		public readonly List<CastFreeData> CastFreeDataList = new List<CastFreeData>();

		// Token: 0x04001A18 RID: 6680
		public bool CanFleeOutOfRange;

		// Token: 0x04001A19 RID: 6681
		[CollectionObjectField(false, true, false, false, false)]
		private CombatStateCollection _buffCombatStateCollection;

		// Token: 0x04001A1A RID: 6682
		[CollectionObjectField(false, true, false, false, false)]
		private CombatStateCollection _debuffCombatStateCollection;

		// Token: 0x04001A1B RID: 6683
		[CollectionObjectField(false, true, false, false, false)]
		private CombatStateCollection _specialCombatStateCollection;

		// Token: 0x04001A1C RID: 6684
		[CollectionObjectField(false, false, true, false, false)]
		private int _combatStateTotalBuffPower;

		// Token: 0x04001A1D RID: 6685
		public short BuffCombatStatePowerExtraLimit;

		// Token: 0x04001A1E RID: 6686
		public short DebuffCombatStatePowerExtraLimit;

		// Token: 0x04001A1F RID: 6687
		[CollectionObjectField(false, true, false, false, false)]
		private SkillEffectCollection _skillEffectCollection;

		// Token: 0x04001A20 RID: 6688
		public sbyte ChangeHitTypeEffectCount;

		// Token: 0x04001A21 RID: 6689
		public sbyte ChangeAvoidTypeEffectCount;

		// Token: 0x04001A22 RID: 6690
		[CollectionObjectField(false, true, false, false, false)]
		private int _hazardValue;

		// Token: 0x04001A23 RID: 6691
		public AiController AiController;

		// Token: 0x04001A24 RID: 6692
		[CollectionObjectField(false, true, false, false, false)]
		private byte _healInjuryCount;

		// Token: 0x04001A25 RID: 6693
		[CollectionObjectField(false, true, false, false, false)]
		private byte _healPoisonCount;

		// Token: 0x04001A26 RID: 6694
		[CollectionObjectField(false, true, false, false, false)]
		private sbyte _animalAttackCount;

		// Token: 0x04001A27 RID: 6695
		[CollectionObjectField(false, true, false, false, false, ArrayElementsCount = 5)]
		private bool[] _otherActionCanUse;

		// Token: 0x04001A28 RID: 6696
		[CollectionObjectField(false, true, false, false, false)]
		private sbyte _preparingOtherAction;

		// Token: 0x04001A29 RID: 6697
		[CollectionObjectField(false, true, false, false, false)]
		private byte _otherActionPreparePercent;

		// Token: 0x04001A2A RID: 6698
		[CollectionObjectField(false, true, false, false, false)]
		private bool _canSurrender;

		// Token: 0x04001A2B RID: 6699
		[CollectionObjectField(false, true, false, false, false)]
		private bool _canUseItem;

		// Token: 0x04001A2C RID: 6700
		[CollectionObjectField(false, true, false, false, false)]
		private ItemKey _preparingItem;

		// Token: 0x04001A2D RID: 6701
		[CollectionObjectField(false, true, false, false, false)]
		private byte _useItemPreparePercent;

		// Token: 0x04001A2E RID: 6702
		public ItemKey UsingItem;

		// Token: 0x04001A2F RID: 6703
		public sbyte ItemUseType;

		// Token: 0x04001A30 RID: 6704
		public List<sbyte> ItemTargetBodyParts;

		// Token: 0x04001A31 RID: 6705
		public ItemKey NeedRepairItem;

		// Token: 0x04001A32 RID: 6706
		public ItemKey RepairingItem;

		// Token: 0x04001A33 RID: 6707
		public bool NeedInterruptSurrender;

		// Token: 0x04001A34 RID: 6708
		public bool NeedAnimalAttack;

		// Token: 0x04001A35 RID: 6709
		public int ChangeCharId;

		// Token: 0x04001A36 RID: 6710
		public string ChangeCharFailAni;

		// Token: 0x04001A37 RID: 6711
		public string ChangeCharFailParticle;

		// Token: 0x04001A38 RID: 6712
		public string ChangeCharFailSound;

		// Token: 0x04001A39 RID: 6713
		public readonly bool[] TeammateHasCommand = new bool[3];

		// Token: 0x04001A3A RID: 6714
		public int TeammateBeforeMainChar;

		// Token: 0x04001A3B RID: 6715
		public int TeammateAfterMainChar;

		// Token: 0x04001A3C RID: 6716
		public CombatCharacter ActingTeammateCommandChar;

		// Token: 0x04001A3D RID: 6717
		[CollectionObjectField(false, true, false, false, false)]
		private bool _showTransferInjuryCommand;

		// Token: 0x04001A3E RID: 6718
		public readonly int[] TeammateCommandCdTotalCount = new int[3];

		// Token: 0x04001A3F RID: 6719
		public readonly int[] TeammateCommandCdCurrentCount = new int[3];

		// Token: 0x04001A40 RID: 6720
		public int TeammateCommandCdSpeed;

		// Token: 0x04001A41 RID: 6721
		public int StopCommandEffectCount;

		// Token: 0x04001A42 RID: 6722
		public bool TransferInjuryCommandIsInner;

		// Token: 0x04001A43 RID: 6723
		[CollectionObjectField(false, true, false, false, false)]
		private List<sbyte> _currTeammateCommands;

		// Token: 0x04001A44 RID: 6724
		[CollectionObjectField(false, true, false, false, false)]
		private List<SByteList> _teammateCommandBanReasons;

		// Token: 0x04001A45 RID: 6725
		[CollectionObjectField(false, false, true, false, false)]
		private readonly List<bool> _teammateCommandCanUse = new List<bool>();

		// Token: 0x04001A46 RID: 6726
		[CollectionObjectField(false, true, false, false, false)]
		private List<byte> _teammateCommandCdPercent;

		// Token: 0x04001A47 RID: 6727
		[CollectionObjectField(false, true, false, false, false)]
		private sbyte _executingTeammateCommand;

		// Token: 0x04001A48 RID: 6728
		public bool NeedResetAdvanceTeammateCommandPushCd;

		// Token: 0x04001A49 RID: 6729
		public bool NeedResetAdvanceTeammateCommandPullCd;

		// Token: 0x04001A4A RID: 6730
		public long ExecutingTeammateCommandSpecialEffect;

		// Token: 0x04001A4B RID: 6731
		public int ExecutingTeammateCommandIndex;

		// Token: 0x04001A4C RID: 6732
		public int ExecutingTeammateCommandChangeDistance;

		// Token: 0x04001A4D RID: 6733
		public TeammateCommandItem ExecutingTeammateCommandConfig;

		// Token: 0x04001A4E RID: 6734
		[CollectionObjectField(false, true, false, false, false)]
		private bool _visible;

		// Token: 0x04001A4F RID: 6735
		public short TeammateCommandLeftPrepareFrame;

		// Token: 0x04001A50 RID: 6736
		public short TeammateCommandTotalPrepareFrame;

		// Token: 0x04001A51 RID: 6737
		[CollectionObjectField(false, true, false, false, false)]
		private byte _teammateCommandPreparePercent;

		// Token: 0x04001A52 RID: 6738
		public short TeammateCommandLeftFrame;

		// Token: 0x04001A53 RID: 6739
		public short TeammateCommandTotalFrame;

		// Token: 0x04001A54 RID: 6740
		[CollectionObjectField(false, true, false, false, false)]
		private byte _teammateCommandTimePercent;

		// Token: 0x04001A55 RID: 6741
		private short _teammateExitAniLeftFrame;

		// Token: 0x04001A56 RID: 6742
		[CollectionObjectField(false, true, false, false, false)]
		private ItemKey _attackCommandWeaponKey;

		// Token: 0x04001A57 RID: 6743
		[CollectionObjectField(false, true, false, false, false)]
		private sbyte _attackCommandTrickType;

		// Token: 0x04001A58 RID: 6744
		[CollectionObjectField(false, true, false, false, false)]
		private short _attackCommandSkillId;

		// Token: 0x04001A59 RID: 6745
		[CollectionObjectField(false, true, false, false, false)]
		private short _defendCommandSkillId;

		// Token: 0x04001A5A RID: 6746
		[CollectionObjectField(false, true, false, false, false)]
		private sbyte _showEffectCommandIndex;

		// Token: 0x04001A5B RID: 6747
		private readonly List<ITeammateCommandInvoker> _teammateCommandInvokers = new List<ITeammateCommandInvoker>();

		// Token: 0x04001A5C RID: 6748
		public bool CanRecoverMobility;

		// Token: 0x04001A5D RID: 6749
		public sbyte AttackForceMissCount;

		// Token: 0x04001A5E RID: 6750
		public sbyte AttackForceHitCount;

		// Token: 0x04001A5F RID: 6751
		public bool SkillForceHit;

		// Token: 0x04001A60 RID: 6752
		public bool OuterInjuryImmunity;

		// Token: 0x04001A61 RID: 6753
		public bool InnerInjuryImmunity;

		// Token: 0x04001A62 RID: 6754
		public bool MindImmunity;

		// Token: 0x04001A63 RID: 6755
		public bool FlawImmunity;

		// Token: 0x04001A64 RID: 6756
		public bool AcupointImmunity;

		// Token: 0x04001A65 RID: 6757
		public bool SkipOnFrameBegin;

		// Token: 0x04001A66 RID: 6758
		[CollectionObjectField(false, false, true, false, false)]
		private OuterAndInnerShorts _attackRange;

		// Token: 0x04001A67 RID: 6759
		[CollectionObjectField(false, false, true, false, false)]
		private sbyte _happiness;

		// Token: 0x04001A68 RID: 6760
		private readonly string[] WinAni = new string[]
		{
			"C_017_female",
			"C_017"
		};

		// Token: 0x04001A69 RID: 6761
		private readonly string[] WinAniLoop = new string[]
		{
			"C_018_female",
			"C_018"
		};

		// Token: 0x04001A6A RID: 6762
		private readonly List<DataUid> _markDataUids = new List<DataUid>();

		// Token: 0x04001A6B RID: 6763
		private static readonly Dictionary<ETeammateCommandImplement, Type> TeammateCommandEffects = new Dictionary<ETeammateCommandImplement, Type>
		{
			{
				ETeammateCommandImplement.GearMateC,
				typeof(GearMateC)
			},
			{
				ETeammateCommandImplement.VitalDemonA,
				typeof(VitalDemonA)
			},
			{
				ETeammateCommandImplement.VitalDemonB,
				typeof(VitalDemonB)
			},
			{
				ETeammateCommandImplement.VitalDemonC,
				typeof(VitalDemonC)
			}
		};

		// Token: 0x04001A6C RID: 6764
		private const int DirectCostDurability = 4;

		// Token: 0x04001A6D RID: 6765
		private const int ReverseCostDurability = 8;

		// Token: 0x04001A6E RID: 6766
		private const byte DirectCostJiTrick = 2;

		// Token: 0x04001A6F RID: 6767
		private const byte ReverseCostUsableTrick = 3;

		// Token: 0x04001A70 RID: 6768
		private readonly List<IExtraUnlockEffect> _invokedUnlockEffects = new List<IExtraUnlockEffect>();

		// Token: 0x04001A71 RID: 6769
		private readonly List<IExtraUnlockEffect> _costedUnlockEffects = new List<IExtraUnlockEffect>();

		// Token: 0x04001A72 RID: 6770
		public const int FixedSize = 646;

		// Token: 0x04001A73 RID: 6771
		public const int DynamicCount = 41;

		// Token: 0x02000B33 RID: 2867
		internal class FixedFieldInfos
		{
			// Token: 0x04002E9A RID: 11930
			public const uint Id_Offset = 0U;

			// Token: 0x04002E9B RID: 11931
			public const int Id_Size = 4;

			// Token: 0x04002E9C RID: 11932
			public const uint BreathValue_Offset = 4U;

			// Token: 0x04002E9D RID: 11933
			public const int BreathValue_Size = 4;

			// Token: 0x04002E9E RID: 11934
			public const uint StanceValue_Offset = 8U;

			// Token: 0x04002E9F RID: 11935
			public const int StanceValue_Size = 4;

			// Token: 0x04002EA0 RID: 11936
			public const uint NeiliAllocation_Offset = 12U;

			// Token: 0x04002EA1 RID: 11937
			public const int NeiliAllocation_Size = 8;

			// Token: 0x04002EA2 RID: 11938
			public const uint OriginNeiliAllocation_Offset = 20U;

			// Token: 0x04002EA3 RID: 11939
			public const int OriginNeiliAllocation_Size = 8;

			// Token: 0x04002EA4 RID: 11940
			public const uint NeiliAllocationRecoverProgress_Offset = 28U;

			// Token: 0x04002EA5 RID: 11941
			public const int NeiliAllocationRecoverProgress_Size = 8;

			// Token: 0x04002EA6 RID: 11942
			public const uint OldDisorderOfQi_Offset = 36U;

			// Token: 0x04002EA7 RID: 11943
			public const int OldDisorderOfQi_Size = 2;

			// Token: 0x04002EA8 RID: 11944
			public const uint NeiliType_Offset = 38U;

			// Token: 0x04002EA9 RID: 11945
			public const int NeiliType_Size = 1;

			// Token: 0x04002EAA RID: 11946
			public const uint AvoidToShow_Offset = 39U;

			// Token: 0x04002EAB RID: 11947
			public const int AvoidToShow_Size = 4;

			// Token: 0x04002EAC RID: 11948
			public const uint CurrentPosition_Offset = 43U;

			// Token: 0x04002EAD RID: 11949
			public const int CurrentPosition_Size = 4;

			// Token: 0x04002EAE RID: 11950
			public const uint DisplayPosition_Offset = 47U;

			// Token: 0x04002EAF RID: 11951
			public const int DisplayPosition_Size = 4;

			// Token: 0x04002EB0 RID: 11952
			public const uint MobilityValue_Offset = 51U;

			// Token: 0x04002EB1 RID: 11953
			public const int MobilityValue_Size = 4;

			// Token: 0x04002EB2 RID: 11954
			public const uint JumpPrepareProgress_Offset = 55U;

			// Token: 0x04002EB3 RID: 11955
			public const int JumpPrepareProgress_Size = 1;

			// Token: 0x04002EB4 RID: 11956
			public const uint JumpPreparedDistance_Offset = 56U;

			// Token: 0x04002EB5 RID: 11957
			public const int JumpPreparedDistance_Size = 2;

			// Token: 0x04002EB6 RID: 11958
			public const uint MobilityLockEffectCount_Offset = 58U;

			// Token: 0x04002EB7 RID: 11959
			public const int MobilityLockEffectCount_Size = 2;

			// Token: 0x04002EB8 RID: 11960
			public const uint JumpChangeDistanceDuration_Offset = 60U;

			// Token: 0x04002EB9 RID: 11961
			public const int JumpChangeDistanceDuration_Size = 4;

			// Token: 0x04002EBA RID: 11962
			public const uint UsingWeaponIndex_Offset = 64U;

			// Token: 0x04002EBB RID: 11963
			public const int UsingWeaponIndex_Size = 4;

			// Token: 0x04002EBC RID: 11964
			public const uint WeaponTricks_Offset = 68U;

			// Token: 0x04002EBD RID: 11965
			public const int WeaponTricks_Size = 6;

			// Token: 0x04002EBE RID: 11966
			public const uint WeaponTrickIndex_Offset = 74U;

			// Token: 0x04002EBF RID: 11967
			public const int WeaponTrickIndex_Size = 1;

			// Token: 0x04002EC0 RID: 11968
			public const uint Weapons_Offset = 75U;

			// Token: 0x04002EC1 RID: 11969
			public const int Weapons_Size = 56;

			// Token: 0x04002EC2 RID: 11970
			public const uint AttackingTrickType_Offset = 131U;

			// Token: 0x04002EC3 RID: 11971
			public const int AttackingTrickType_Size = 1;

			// Token: 0x04002EC4 RID: 11972
			public const uint CanAttackOutRange_Offset = 132U;

			// Token: 0x04002EC5 RID: 11973
			public const int CanAttackOutRange_Size = 1;

			// Token: 0x04002EC6 RID: 11974
			public const uint ChangeTrickProgress_Offset = 133U;

			// Token: 0x04002EC7 RID: 11975
			public const int ChangeTrickProgress_Size = 1;

			// Token: 0x04002EC8 RID: 11976
			public const uint ChangeTrickCount_Offset = 134U;

			// Token: 0x04002EC9 RID: 11977
			public const int ChangeTrickCount_Size = 2;

			// Token: 0x04002ECA RID: 11978
			public const uint CanChangeTrick_Offset = 136U;

			// Token: 0x04002ECB RID: 11979
			public const int CanChangeTrick_Size = 1;

			// Token: 0x04002ECC RID: 11980
			public const uint ChangingTrick_Offset = 137U;

			// Token: 0x04002ECD RID: 11981
			public const int ChangingTrick_Size = 1;

			// Token: 0x04002ECE RID: 11982
			public const uint ChangeTrickAttack_Offset = 138U;

			// Token: 0x04002ECF RID: 11983
			public const int ChangeTrickAttack_Size = 1;

			// Token: 0x04002ED0 RID: 11984
			public const uint IsFightBack_Offset = 139U;

			// Token: 0x04002ED1 RID: 11985
			public const int IsFightBack_Size = 1;

			// Token: 0x04002ED2 RID: 11986
			public const uint Injuries_Offset = 140U;

			// Token: 0x04002ED3 RID: 11987
			public const int Injuries_Size = 16;

			// Token: 0x04002ED4 RID: 11988
			public const uint OldInjuries_Offset = 156U;

			// Token: 0x04002ED5 RID: 11989
			public const int OldInjuries_Size = 16;

			// Token: 0x04002ED6 RID: 11990
			public const uint DamageStepCollection_Offset = 172U;

			// Token: 0x04002ED7 RID: 11991
			public const int DamageStepCollection_Size = 64;

			// Token: 0x04002ED8 RID: 11992
			public const uint OuterDamageValue_Offset = 236U;

			// Token: 0x04002ED9 RID: 11993
			public const int OuterDamageValue_Size = 28;

			// Token: 0x04002EDA RID: 11994
			public const uint InnerDamageValue_Offset = 264U;

			// Token: 0x04002EDB RID: 11995
			public const int InnerDamageValue_Size = 28;

			// Token: 0x04002EDC RID: 11996
			public const uint MindDamageValue_Offset = 292U;

			// Token: 0x04002EDD RID: 11997
			public const int MindDamageValue_Size = 4;

			// Token: 0x04002EDE RID: 11998
			public const uint FatalDamageValue_Offset = 296U;

			// Token: 0x04002EDF RID: 11999
			public const int FatalDamageValue_Size = 4;

			// Token: 0x04002EE0 RID: 12000
			public const uint OuterDamageValueToShow_Offset = 300U;

			// Token: 0x04002EE1 RID: 12001
			public const int OuterDamageValueToShow_Size = 56;

			// Token: 0x04002EE2 RID: 12002
			public const uint InnerDamageValueToShow_Offset = 356U;

			// Token: 0x04002EE3 RID: 12003
			public const int InnerDamageValueToShow_Size = 56;

			// Token: 0x04002EE4 RID: 12004
			public const uint MindDamageValueToShow_Offset = 412U;

			// Token: 0x04002EE5 RID: 12005
			public const int MindDamageValueToShow_Size = 4;

			// Token: 0x04002EE6 RID: 12006
			public const uint FatalDamageValueToShow_Offset = 416U;

			// Token: 0x04002EE7 RID: 12007
			public const int FatalDamageValueToShow_Size = 4;

			// Token: 0x04002EE8 RID: 12008
			public const uint FlawCount_Offset = 420U;

			// Token: 0x04002EE9 RID: 12009
			public const int FlawCount_Size = 7;

			// Token: 0x04002EEA RID: 12010
			public const uint AcupointCount_Offset = 427U;

			// Token: 0x04002EEB RID: 12011
			public const int AcupointCount_Size = 7;

			// Token: 0x04002EEC RID: 12012
			public const uint Poison_Offset = 434U;

			// Token: 0x04002EED RID: 12013
			public const int Poison_Size = 24;

			// Token: 0x04002EEE RID: 12014
			public const uint OldPoison_Offset = 458U;

			// Token: 0x04002EEF RID: 12015
			public const int OldPoison_Size = 24;

			// Token: 0x04002EF0 RID: 12016
			public const uint PoisonResist_Offset = 482U;

			// Token: 0x04002EF1 RID: 12017
			public const int PoisonResist_Size = 24;

			// Token: 0x04002EF2 RID: 12018
			public const uint NewPoisonsToShow_Offset = 506U;

			// Token: 0x04002EF3 RID: 12019
			public const int NewPoisonsToShow_Size = 18;

			// Token: 0x04002EF4 RID: 12020
			public const uint PreparingSkillId_Offset = 524U;

			// Token: 0x04002EF5 RID: 12021
			public const int PreparingSkillId_Size = 2;

			// Token: 0x04002EF6 RID: 12022
			public const uint SkillPreparePercent_Offset = 526U;

			// Token: 0x04002EF7 RID: 12023
			public const int SkillPreparePercent_Size = 1;

			// Token: 0x04002EF8 RID: 12024
			public const uint PerformingSkillId_Offset = 527U;

			// Token: 0x04002EF9 RID: 12025
			public const int PerformingSkillId_Size = 2;

			// Token: 0x04002EFA RID: 12026
			public const uint AutoCastingSkill_Offset = 529U;

			// Token: 0x04002EFB RID: 12027
			public const int AutoCastingSkill_Size = 1;

			// Token: 0x04002EFC RID: 12028
			public const uint AttackSkillAttackIndex_Offset = 530U;

			// Token: 0x04002EFD RID: 12029
			public const int AttackSkillAttackIndex_Size = 1;

			// Token: 0x04002EFE RID: 12030
			public const uint AttackSkillPower_Offset = 531U;

			// Token: 0x04002EFF RID: 12031
			public const int AttackSkillPower_Size = 1;

			// Token: 0x04002F00 RID: 12032
			public const uint AffectingMoveSkillId_Offset = 532U;

			// Token: 0x04002F01 RID: 12033
			public const int AffectingMoveSkillId_Size = 2;

			// Token: 0x04002F02 RID: 12034
			public const uint AffectingDefendSkillId_Offset = 534U;

			// Token: 0x04002F03 RID: 12035
			public const int AffectingDefendSkillId_Size = 2;

			// Token: 0x04002F04 RID: 12036
			public const uint DefendSkillTimePercent_Offset = 536U;

			// Token: 0x04002F05 RID: 12037
			public const int DefendSkillTimePercent_Size = 1;

			// Token: 0x04002F06 RID: 12038
			public const uint WugCount_Offset = 537U;

			// Token: 0x04002F07 RID: 12039
			public const int WugCount_Size = 2;

			// Token: 0x04002F08 RID: 12040
			public const uint HealInjuryCount_Offset = 539U;

			// Token: 0x04002F09 RID: 12041
			public const int HealInjuryCount_Size = 1;

			// Token: 0x04002F0A RID: 12042
			public const uint HealPoisonCount_Offset = 540U;

			// Token: 0x04002F0B RID: 12043
			public const int HealPoisonCount_Size = 1;

			// Token: 0x04002F0C RID: 12044
			public const uint OtherActionCanUse_Offset = 541U;

			// Token: 0x04002F0D RID: 12045
			public const int OtherActionCanUse_Size = 5;

			// Token: 0x04002F0E RID: 12046
			public const uint PreparingOtherAction_Offset = 546U;

			// Token: 0x04002F0F RID: 12047
			public const int PreparingOtherAction_Size = 1;

			// Token: 0x04002F10 RID: 12048
			public const uint OtherActionPreparePercent_Offset = 547U;

			// Token: 0x04002F11 RID: 12049
			public const int OtherActionPreparePercent_Size = 1;

			// Token: 0x04002F12 RID: 12050
			public const uint CanSurrender_Offset = 548U;

			// Token: 0x04002F13 RID: 12051
			public const int CanSurrender_Size = 1;

			// Token: 0x04002F14 RID: 12052
			public const uint CanUseItem_Offset = 549U;

			// Token: 0x04002F15 RID: 12053
			public const int CanUseItem_Size = 1;

			// Token: 0x04002F16 RID: 12054
			public const uint PreparingItem_Offset = 550U;

			// Token: 0x04002F17 RID: 12055
			public const int PreparingItem_Size = 8;

			// Token: 0x04002F18 RID: 12056
			public const uint UseItemPreparePercent_Offset = 558U;

			// Token: 0x04002F19 RID: 12057
			public const int UseItemPreparePercent_Size = 1;

			// Token: 0x04002F1A RID: 12058
			public const uint CombatReserveData_Offset = 559U;

			// Token: 0x04002F1B RID: 12059
			public const int CombatReserveData_Size = 12;

			// Token: 0x04002F1C RID: 12060
			public const uint XiangshuEffectId_Offset = 571U;

			// Token: 0x04002F1D RID: 12061
			public const int XiangshuEffectId_Size = 2;

			// Token: 0x04002F1E RID: 12062
			public const uint HazardValue_Offset = 573U;

			// Token: 0x04002F1F RID: 12063
			public const int HazardValue_Size = 4;

			// Token: 0x04002F20 RID: 12064
			public const uint AnimationTimeScale_Offset = 577U;

			// Token: 0x04002F21 RID: 12065
			public const int AnimationTimeScale_Size = 4;

			// Token: 0x04002F22 RID: 12066
			public const uint AttackOutOfRange_Offset = 581U;

			// Token: 0x04002F23 RID: 12067
			public const int AttackOutOfRange_Size = 1;

			// Token: 0x04002F24 RID: 12068
			public const uint BossPhase_Offset = 582U;

			// Token: 0x04002F25 RID: 12069
			public const int BossPhase_Size = 1;

			// Token: 0x04002F26 RID: 12070
			public const uint AnimalAttackCount_Offset = 583U;

			// Token: 0x04002F27 RID: 12071
			public const int AnimalAttackCount_Size = 1;

			// Token: 0x04002F28 RID: 12072
			public const uint ShowTransferInjuryCommand_Offset = 584U;

			// Token: 0x04002F29 RID: 12073
			public const int ShowTransferInjuryCommand_Size = 1;

			// Token: 0x04002F2A RID: 12074
			public const uint ExecutingTeammateCommand_Offset = 585U;

			// Token: 0x04002F2B RID: 12075
			public const int ExecutingTeammateCommand_Size = 1;

			// Token: 0x04002F2C RID: 12076
			public const uint Visible_Offset = 586U;

			// Token: 0x04002F2D RID: 12077
			public const int Visible_Size = 1;

			// Token: 0x04002F2E RID: 12078
			public const uint TeammateCommandPreparePercent_Offset = 587U;

			// Token: 0x04002F2F RID: 12079
			public const int TeammateCommandPreparePercent_Size = 1;

			// Token: 0x04002F30 RID: 12080
			public const uint TeammateCommandTimePercent_Offset = 588U;

			// Token: 0x04002F31 RID: 12081
			public const int TeammateCommandTimePercent_Size = 1;

			// Token: 0x04002F32 RID: 12082
			public const uint AttackCommandWeaponKey_Offset = 589U;

			// Token: 0x04002F33 RID: 12083
			public const int AttackCommandWeaponKey_Size = 8;

			// Token: 0x04002F34 RID: 12084
			public const uint AttackCommandTrickType_Offset = 597U;

			// Token: 0x04002F35 RID: 12085
			public const int AttackCommandTrickType_Size = 1;

			// Token: 0x04002F36 RID: 12086
			public const uint DefendCommandSkillId_Offset = 598U;

			// Token: 0x04002F37 RID: 12087
			public const int DefendCommandSkillId_Size = 2;

			// Token: 0x04002F38 RID: 12088
			public const uint ShowEffectCommandIndex_Offset = 600U;

			// Token: 0x04002F39 RID: 12089
			public const int ShowEffectCommandIndex_Size = 1;

			// Token: 0x04002F3A RID: 12090
			public const uint AttackCommandSkillId_Offset = 601U;

			// Token: 0x04002F3B RID: 12091
			public const int AttackCommandSkillId_Size = 2;

			// Token: 0x04002F3C RID: 12092
			public const uint TargetDistance_Offset = 603U;

			// Token: 0x04002F3D RID: 12093
			public const int TargetDistance_Size = 2;

			// Token: 0x04002F3E RID: 12094
			public const uint NeiliAllocationCd_Offset = 605U;

			// Token: 0x04002F3F RID: 12095
			public const int NeiliAllocationCd_Size = 8;

			// Token: 0x04002F40 RID: 12096
			public const uint ProportionDelta_Offset = 613U;

			// Token: 0x04002F41 RID: 12097
			public const int ProportionDelta_Size = 8;

			// Token: 0x04002F42 RID: 12098
			public const uint MindMarkInfinityCount_Offset = 621U;

			// Token: 0x04002F43 RID: 12099
			public const int MindMarkInfinityCount_Size = 4;

			// Token: 0x04002F44 RID: 12100
			public const uint MindMarkInfinityProgress_Offset = 625U;

			// Token: 0x04002F45 RID: 12101
			public const int MindMarkInfinityProgress_Size = 4;

			// Token: 0x04002F46 RID: 12102
			public const uint NormalAttackRecovery_Offset = 629U;

			// Token: 0x04002F47 RID: 12103
			public const int NormalAttackRecovery_Size = 8;

			// Token: 0x04002F48 RID: 12104
			public const uint ReserveNormalAttack_Offset = 637U;

			// Token: 0x04002F49 RID: 12105
			public const int ReserveNormalAttack_Size = 1;

			// Token: 0x04002F4A RID: 12106
			public const uint Gangqi_Offset = 638U;

			// Token: 0x04002F4B RID: 12107
			public const int Gangqi_Size = 4;

			// Token: 0x04002F4C RID: 12108
			public const uint GangqiMax_Offset = 642U;

			// Token: 0x04002F4D RID: 12109
			public const int GangqiMax_Size = 4;
		}
	}
}
