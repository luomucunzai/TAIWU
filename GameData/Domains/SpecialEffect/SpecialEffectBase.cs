using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Config;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.Character;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.Item;
using GameData.GameDataBridge;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect
{
	// Token: 0x020000E1 RID: 225
	public class SpecialEffectBase : ISpecialEffectModifier, IFrameCounterHandler
	{
		// Token: 0x170000E3 RID: 227
		// (get) Token: 0x06002896 RID: 10390 RVA: 0x001EFF58 File Offset: 0x001EE158
		public static SpecialEffectBase Invalid
		{
			get
			{
				return new SpecialEffectBase();
			}
		}

		// Token: 0x170000E4 RID: 228
		// (get) Token: 0x06002897 RID: 10391 RVA: 0x001EFF5F File Offset: 0x001EE15F
		// (set) Token: 0x06002898 RID: 10392 RVA: 0x001EFF67 File Offset: 0x001EE167
		public int CharacterId { get; set; }

		// Token: 0x170000E5 RID: 229
		// (get) Token: 0x06002899 RID: 10393 RVA: 0x001EFF70 File Offset: 0x001EE170
		public CombatCharacter CombatChar
		{
			get
			{
				return DomainManager.Combat.GetElement_CombatCharacterDict(this.CharacterId);
			}
		}

		// Token: 0x170000E6 RID: 230
		// (get) Token: 0x0600289A RID: 10394 RVA: 0x001EFF82 File Offset: 0x001EE182
		public CombatCharacter EnemyChar
		{
			get
			{
				return DomainManager.Combat.GetCombatCharacter(!this.CombatChar.IsAlly, false);
			}
		}

		// Token: 0x170000E7 RID: 231
		// (get) Token: 0x0600289B RID: 10395 RVA: 0x001EFF9D File Offset: 0x001EE19D
		public CombatCharacter CurrEnemyChar
		{
			get
			{
				return DomainManager.Combat.GetCombatCharacter(!this.CombatChar.IsAlly, true);
			}
		}

		// Token: 0x170000E8 RID: 232
		// (get) Token: 0x0600289C RID: 10396 RVA: 0x001EFFB8 File Offset: 0x001EE1B8
		public string DataHandlerKey
		{
			get
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(0, 2);
				defaultInterpolatedStringHandler.AppendFormatted(base.GetType().Name);
				defaultInterpolatedStringHandler.AppendFormatted<long>(this.Id);
				return defaultInterpolatedStringHandler.ToStringAndClear();
			}
		}

		// Token: 0x170000E9 RID: 233
		// (get) Token: 0x0600289D RID: 10397 RVA: 0x001EFFF6 File Offset: 0x001EE1F6
		protected bool IsCurrent
		{
			get
			{
				return DomainManager.Combat.IsCurrentCombatCharacter(this.CombatChar);
			}
		}

		// Token: 0x170000EA RID: 234
		// (get) Token: 0x0600289E RID: 10398 RVA: 0x001F0008 File Offset: 0x001EE208
		protected bool IsEntering
		{
			get
			{
				return DomainManager.Combat.GetMainCharacter(this.CombatChar.IsAlly).TeammateBeforeMainChar == this.CharacterId || DomainManager.Combat.GetMainCharacter(this.CombatChar.IsAlly).TeammateAfterMainChar == this.CharacterId;
			}
		}

		// Token: 0x0600289F RID: 10399 RVA: 0x001F005C File Offset: 0x001EE25C
		protected SpecialEffectBase()
		{
			this.CreateFrameCounters();
		}

		// Token: 0x060028A0 RID: 10400 RVA: 0x001F0078 File Offset: 0x001EE278
		protected SpecialEffectBase(int characterId, int type)
		{
			this.CharacterId = characterId;
			this.Type = type;
			this.CharObj = DomainManager.Character.GetElement_Objects(this.CharacterId);
			this.CreateFrameCounters();
		}

		// Token: 0x060028A1 RID: 10401 RVA: 0x001F00C4 File Offset: 0x001EE2C4
		public int GetSerializedSize()
		{
			int totalSize = 16 + this.GetSubClassSerializedSize();
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x060028A2 RID: 10402 RVA: 0x001F00F0 File Offset: 0x001EE2F0
		public unsafe int Serialize(byte* pData)
		{
			*(long*)pData = this.Id;
			byte* pCurrData = pData + 8;
			*(int*)pCurrData = this.CharacterId;
			pCurrData += 4;
			*(int*)pCurrData = this.Type;
			pCurrData += 4;
			pCurrData += this.SerializeSubClass(pCurrData);
			int totalSize = (int)((long)(pCurrData - pData));
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x060028A3 RID: 10403 RVA: 0x001F014C File Offset: 0x001EE34C
		public unsafe int Deserialize(byte* pData)
		{
			this.Id = *(long*)pData;
			byte* pCurrData = pData + 8;
			this.CharacterId = *(int*)pCurrData;
			pCurrData += 4;
			this.Type = *(int*)pCurrData;
			pCurrData += 4;
			DomainManager.Character.TryGetElement_Objects(this.CharacterId, out this.CharObj);
			pCurrData += this.DeserializeSubClass(pCurrData);
			int totalSize = (int)((long)(pCurrData - pData));
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x060028A4 RID: 10404 RVA: 0x001F01C0 File Offset: 0x001EE3C0
		protected virtual int GetSubClassSerializedSize()
		{
			return 0;
		}

		// Token: 0x060028A5 RID: 10405 RVA: 0x001F01D4 File Offset: 0x001EE3D4
		protected unsafe virtual int SerializeSubClass(byte* pData)
		{
			return 0;
		}

		// Token: 0x060028A6 RID: 10406 RVA: 0x001F01E8 File Offset: 0x001EE3E8
		protected unsafe virtual int DeserializeSubClass(byte* pData)
		{
			return 0;
		}

		// Token: 0x060028A7 RID: 10407 RVA: 0x001F01FB File Offset: 0x001EE3FB
		public virtual void OnEnable(DataContext context)
		{
			this.SetupFrameCounters();
		}

		// Token: 0x060028A8 RID: 10408 RVA: 0x001F0205 File Offset: 0x001EE405
		public virtual void OnDataAdded(DataContext context)
		{
		}

		// Token: 0x060028A9 RID: 10409 RVA: 0x001F0208 File Offset: 0x001EE408
		public virtual void OnDisable(DataContext context)
		{
			this.CloseFrameCounters();
			this.ClearMonitors();
		}

		// Token: 0x060028AA RID: 10410 RVA: 0x001F0219 File Offset: 0x001EE419
		public void OnDreamBack(DataContext context)
		{
			this.OnDisable(context);
			this.AffectDatas = null;
		}

		// Token: 0x060028AB RID: 10411 RVA: 0x001F022B File Offset: 0x001EE42B
		public void CreateAffectedData(ushort fieldId, EDataModifyType modifyType, short combatSkillId = -1)
		{
			this.CreateAffectedData(this.CharacterId, fieldId, modifyType, combatSkillId);
		}

		// Token: 0x060028AC RID: 10412 RVA: 0x001F0240 File Offset: 0x001EE440
		public void CreateAffectedData(int charId, ushort fieldId, EDataModifyType modifyType, short combatSkillId = -1)
		{
			AffectedDataKey dataKey = new AffectedDataKey(charId, fieldId, combatSkillId, -1, -1, -1);
			if (this.AffectDatas == null)
			{
				this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			}
			bool flag = this.AffectDatas.TryAdd(dataKey, modifyType);
			if (!flag)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(21, 4);
				defaultInterpolatedStringHandler.AppendFormatted<GameData.Domains.Character.Character>(this.CharObj);
				defaultInterpolatedStringHandler.AppendLiteral(" overwrite ");
				defaultInterpolatedStringHandler.AppendFormatted<AffectedDataKey>(dataKey);
				defaultInterpolatedStringHandler.AppendLiteral(" from ");
				defaultInterpolatedStringHandler.AppendFormatted<EDataModifyType>(this.AffectDatas[dataKey]);
				defaultInterpolatedStringHandler.AppendLiteral(" to ");
				defaultInterpolatedStringHandler.AppendFormatted<EDataModifyType>(modifyType);
				AdaptableLog.Warning(defaultInterpolatedStringHandler.ToStringAndClear(), false);
				this.AffectDatas[dataKey] = modifyType;
			}
		}

		// Token: 0x060028AD RID: 10413 RVA: 0x001F0308 File Offset: 0x001EE508
		public void CreateAffectedAllEnemyData(ushort fieldId, EDataModifyType modifyType, short combatSkillId = -1)
		{
			int[] charList = DomainManager.Combat.GetCharacterList(!this.CombatChar.IsAlly);
			for (int i = 0; i < charList.Length; i++)
			{
				bool flag = charList[i] >= 0;
				if (flag)
				{
					this.CreateAffectedData(charList[i], fieldId, modifyType, combatSkillId);
				}
			}
		}

		// Token: 0x060028AE RID: 10414 RVA: 0x001F035C File Offset: 0x001EE55C
		public void CreateAffectedAllCombatCharData(ushort fieldId, EDataModifyType modifyType, short combatSkillId = -1)
		{
			List<int> allCharList = ObjectPool<List<int>>.Instance.Get();
			allCharList.Clear();
			DomainManager.Combat.GetAllCharInCombat(allCharList);
			for (int i = 0; i < allCharList.Count; i++)
			{
				this.CreateAffectedData(allCharList[i], fieldId, modifyType, combatSkillId);
			}
			ObjectPool<List<int>>.Instance.Return(allCharList);
		}

		// Token: 0x060028AF RID: 10415 RVA: 0x001F03BA File Offset: 0x001EE5BA
		protected void AppendAffectedData(DataContext context, ushort fieldId, EDataModifyType modifyType, short combatSkillId = -1)
		{
			this.AppendAffectedData(context, this.CharacterId, fieldId, modifyType, combatSkillId);
		}

		// Token: 0x060028B0 RID: 10416 RVA: 0x001F03D0 File Offset: 0x001EE5D0
		protected void AppendAffectedData(DataContext context, int charId, ushort fieldId, EDataModifyType modifyType, short combatSkillId = -1)
		{
			AffectedDataKey dataKey = new AffectedDataKey(charId, fieldId, combatSkillId, -1, -1, -1);
			if (this.AffectDatas == null)
			{
				this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			}
			this.AffectDatas.Add(dataKey, modifyType);
			DomainManager.SpecialEffect.AppendDataUid(context, this, dataKey);
		}

		// Token: 0x060028B1 RID: 10417 RVA: 0x001F041C File Offset: 0x001EE61C
		protected void AppendAffectedCurrEnemyData(DataContext context, ushort fieldId, EDataModifyType modifyType, short combatSkillId = -1)
		{
			this.AppendAffectedData(context, this.CurrEnemyChar.GetId(), fieldId, modifyType, combatSkillId);
		}

		// Token: 0x060028B2 RID: 10418 RVA: 0x001F0438 File Offset: 0x001EE638
		protected void AppendAffectedAllEnemyData(DataContext context, ushort fieldId, EDataModifyType modifyType, short combatSkillId = -1)
		{
			int[] charList = DomainManager.Combat.GetCharacterList(!this.CombatChar.IsAlly);
			for (int i = 0; i < charList.Length; i++)
			{
				bool flag = charList[i] >= 0;
				if (flag)
				{
					this.AppendAffectedData(context, charList[i], fieldId, modifyType, combatSkillId);
				}
			}
		}

		// Token: 0x060028B3 RID: 10419 RVA: 0x001F0490 File Offset: 0x001EE690
		protected void AppendAffectedAllCombatCharData(DataContext context, ushort fieldId, EDataModifyType modifyType, short combatSkillId = -1)
		{
			List<int> allCharList = ObjectPool<List<int>>.Instance.Get();
			allCharList.Clear();
			DomainManager.Combat.GetAllCharInCombat(allCharList);
			for (int i = 0; i < allCharList.Count; i++)
			{
				this.AppendAffectedData(context, allCharList[i], fieldId, modifyType, combatSkillId);
			}
			ObjectPool<List<int>>.Instance.Return(allCharList);
		}

		// Token: 0x060028B4 RID: 10420 RVA: 0x001F04F0 File Offset: 0x001EE6F0
		protected void RemoveAffectedData(DataContext context, int charId, ushort fieldId)
		{
			AffectedDataKey dataKey = new AffectedDataKey(charId, fieldId, -1, -1, -1, -1);
			DomainManager.SpecialEffect.RemoveDataUid(context, this, dataKey);
			Dictionary<AffectedDataKey, EDataModifyType> affectDatas = this.AffectDatas;
			if (affectDatas != null)
			{
				affectDatas.Remove(dataKey);
			}
		}

		// Token: 0x060028B5 RID: 10421 RVA: 0x001F052C File Offset: 0x001EE72C
		protected void ClearAffectedData(DataContext context)
		{
			DomainManager.SpecialEffect.RemoveDataUid(context, this);
			Dictionary<AffectedDataKey, EDataModifyType> affectDatas = this.AffectDatas;
			if (affectDatas != null)
			{
				affectDatas.Clear();
			}
		}

		// Token: 0x060028B6 RID: 10422 RVA: 0x001F054E File Offset: 0x001EE74E
		public virtual int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			return 0;
		}

		// Token: 0x060028B7 RID: 10423 RVA: 0x001F0551 File Offset: 0x001EE751
		protected void InvalidateCache(DataContext context, ushort fieldId)
		{
			this.InvalidateCache(context, this.CharacterId, fieldId);
		}

		// Token: 0x060028B8 RID: 10424 RVA: 0x001F0564 File Offset: 0x001EE764
		protected void InvalidateAllEnemyCache(DataContext context, ushort fieldId)
		{
			foreach (int charId in DomainManager.Combat.GetCharacterList(!this.CombatChar.IsAlly))
			{
				bool flag = charId >= 0;
				if (flag)
				{
					this.InvalidateCache(context, charId, fieldId);
				}
			}
		}

		// Token: 0x060028B9 RID: 10425 RVA: 0x001F05B3 File Offset: 0x001EE7B3
		protected void InvalidateCache(DataContext context, int charId, ushort fieldId)
		{
			DomainManager.SpecialEffect.InvalidateCache(context, charId, fieldId);
		}

		// Token: 0x060028BA RID: 10426 RVA: 0x001F05C4 File Offset: 0x001EE7C4
		public virtual bool GetModifiedValue(AffectedDataKey dataKey, bool dataValue)
		{
			return dataValue;
		}

		// Token: 0x060028BB RID: 10427 RVA: 0x001F05C7 File Offset: 0x001EE7C7
		public virtual int GetModifiedValue(AffectedDataKey dataKey, int dataValue)
		{
			return dataValue;
		}

		// Token: 0x060028BC RID: 10428 RVA: 0x001F05CA File Offset: 0x001EE7CA
		public virtual long GetModifiedValue(AffectedDataKey dataKey, long dataValue)
		{
			return dataValue;
		}

		// Token: 0x060028BD RID: 10429 RVA: 0x001F05CD File Offset: 0x001EE7CD
		public virtual HitOrAvoidInts GetModifiedValue(AffectedDataKey dataKey, HitOrAvoidInts dataValue)
		{
			return dataValue;
		}

		// Token: 0x060028BE RID: 10430 RVA: 0x001F05D0 File Offset: 0x001EE7D0
		public virtual NeiliProportionOfFiveElements GetModifiedValue(AffectedDataKey dataKey, NeiliProportionOfFiveElements dataValue)
		{
			return dataValue;
		}

		// Token: 0x060028BF RID: 10431 RVA: 0x001F05D3 File Offset: 0x001EE7D3
		public virtual OuterAndInnerInts GetModifiedValue(AffectedDataKey dataKey, OuterAndInnerInts dataValue)
		{
			return dataValue;
		}

		// Token: 0x060028C0 RID: 10432 RVA: 0x001F05D6 File Offset: 0x001EE7D6
		public virtual List<NeedTrick> GetModifiedValue(AffectedDataKey dataKey, List<NeedTrick> dataValue)
		{
			return dataValue;
		}

		// Token: 0x060028C1 RID: 10433 RVA: 0x001F05D9 File Offset: 0x001EE7D9
		public virtual ValueTuple<sbyte, sbyte> GetModifiedValue(AffectedDataKey dataKey, ValueTuple<sbyte, sbyte> dataValue)
		{
			return dataValue;
		}

		// Token: 0x060028C2 RID: 10434 RVA: 0x001F05DC File Offset: 0x001EE7DC
		public virtual List<ItemKeyAndCount> GetModifiedValue(AffectedDataKey dataKey, List<ItemKeyAndCount> dataValue)
		{
			return dataValue;
		}

		// Token: 0x060028C3 RID: 10435 RVA: 0x001F05DF File Offset: 0x001EE7DF
		public virtual List<CastBoostEffectDisplayData> GetModifiedValue(AffectedDataKey dataKey, List<CastBoostEffectDisplayData> dataValue)
		{
			return dataValue;
		}

		// Token: 0x060028C4 RID: 10436 RVA: 0x001F05E2 File Offset: 0x001EE7E2
		public virtual List<CombatSkillEffectData> GetModifiedValue(AffectedDataKey dataKey, List<CombatSkillEffectData> dataValue)
		{
			return dataValue;
		}

		// Token: 0x060028C5 RID: 10437 RVA: 0x001F05E5 File Offset: 0x001EE7E5
		public virtual BoolArray8 GetModifiedValue(AffectedDataKey dataKey, BoolArray8 dataValue)
		{
			return dataValue;
		}

		// Token: 0x060028C6 RID: 10438 RVA: 0x001F05E8 File Offset: 0x001EE7E8
		public virtual CombatCharacter GetModifiedValue(AffectedDataKey dataKey, CombatCharacter dataValue)
		{
			return dataValue;
		}

		// Token: 0x060028C7 RID: 10439 RVA: 0x001F05EB File Offset: 0x001EE7EB
		public virtual List<int> GetModifiedValue(AffectedDataKey dataKey, List<int> dataValue)
		{
			return dataValue;
		}

		// Token: 0x060028C8 RID: 10440 RVA: 0x001F05F0 File Offset: 0x001EE7F0
		public bool FiveElementsEquals(short skillId, sbyte fiveElement)
		{
			return CombatSkillDomain.FiveElementEquals(this.CharacterId, skillId, fiveElement);
		}

		// Token: 0x060028C9 RID: 10441 RVA: 0x001F0610 File Offset: 0x001EE810
		public bool FiveElementsEquals(AffectedDataKey dataKey, sbyte fiveElement)
		{
			return CombatSkillDomain.FiveElementEquals(dataKey.CharId, dataKey.CombatSkillId, fiveElement);
		}

		// Token: 0x060028CA RID: 10442 RVA: 0x001F0634 File Offset: 0x001EE834
		public bool FiveElementsEquals(CombatSkillKey skillKey, sbyte fiveElement)
		{
			return CombatSkillDomain.FiveElementEquals(skillKey.CharId, skillKey.SkillTemplateId, fiveElement);
		}

		// Token: 0x060028CB RID: 10443 RVA: 0x001F0658 File Offset: 0x001EE858
		public bool FiveElementsEqualsEnemy(short skillId, sbyte fiveElement)
		{
			CombatCharacter enemyChar = DomainManager.Combat.GetCombatCharacter(!this.CombatChar.IsAlly, false);
			return CombatSkillDomain.FiveElementEquals(enemyChar.GetId(), skillId, fiveElement);
		}

		// Token: 0x060028CC RID: 10444 RVA: 0x001F0694 File Offset: 0x001EE894
		protected DataUid ParseCombatCharacterDataUid(ushort fieldId)
		{
			return this.ParseCombatCharacterDataUid(this.CharacterId, fieldId);
		}

		// Token: 0x060028CD RID: 10445 RVA: 0x001F06B4 File Offset: 0x001EE8B4
		protected DataUid ParseCombatCharacterDataUid(int charId, ushort fieldId)
		{
			return new DataUid(8, 10, (ulong)((long)charId), (uint)fieldId);
		}

		// Token: 0x060028CE RID: 10446 RVA: 0x001F06D4 File Offset: 0x001EE8D4
		protected DataUid ParseCharDataUid(ushort fieldId)
		{
			return this.ParseCharDataUid(this.CharacterId, fieldId);
		}

		// Token: 0x060028CF RID: 10447 RVA: 0x001F06F4 File Offset: 0x001EE8F4
		protected DataUid ParseCharDataUid(int charId, ushort fieldId)
		{
			return new DataUid(4, 0, (ulong)((long)charId), (uint)fieldId);
		}

		// Token: 0x060028D0 RID: 10448 RVA: 0x001F0710 File Offset: 0x001EE910
		protected DataUid ParseNeiliAllocationDataUid()
		{
			return this.ParseNeiliAllocationDataUid(this.CharacterId);
		}

		// Token: 0x060028D1 RID: 10449 RVA: 0x001F0730 File Offset: 0x001EE930
		protected DataUid ParseNeiliAllocationDataUid(int charId)
		{
			return this.ParseCombatCharacterDataUid(charId, 3);
		}

		// Token: 0x060028D2 RID: 10450 RVA: 0x001F074C File Offset: 0x001EE94C
		protected int ChangeStanceValue(DataContext context, CombatCharacter character, int addValue)
		{
			return DomainManager.Combat.ChangeStanceValue(context, character, addValue, true, this.CombatChar);
		}

		// Token: 0x060028D3 RID: 10451 RVA: 0x001F0774 File Offset: 0x001EE974
		protected void AbsorbStanceValue(DataContext context, CombatCharacter character, CValuePercent percent)
		{
			int absorbValue = Math.Min(4000 * percent, character.GetStanceValue());
			absorbValue = this.ChangeStanceValue(context, character, -absorbValue);
			this.ChangeStanceValue(context, this.CombatChar, Math.Abs(absorbValue));
		}

		// Token: 0x060028D4 RID: 10452 RVA: 0x001F07B8 File Offset: 0x001EE9B8
		protected int ChangeBreathValue(DataContext context, CombatCharacter character, int addValue)
		{
			return DomainManager.Combat.ChangeBreathValue(context, character, addValue, true, this.CombatChar);
		}

		// Token: 0x060028D5 RID: 10453 RVA: 0x001F07E0 File Offset: 0x001EE9E0
		protected void AbsorbBreathValue(DataContext context, CombatCharacter character, CValuePercent percent)
		{
			int absorbValue = Math.Min(30000 * percent, character.GetBreathValue());
			absorbValue = this.ChangeBreathValue(context, character, -absorbValue);
			this.ChangeBreathValue(context, this.CombatChar, Math.Abs(absorbValue));
		}

		// Token: 0x060028D6 RID: 10454 RVA: 0x001F0824 File Offset: 0x001EEA24
		protected void ChangeMobilityValue(DataContext context, CombatCharacter character, int addValue)
		{
			DomainManager.Combat.ChangeMobilityValue(context, character, addValue, true, this.CombatChar, false);
		}

		// Token: 0x060028D7 RID: 10455 RVA: 0x001F0840 File Offset: 0x001EEA40
		protected bool ClearAffectingAgileSkill(DataContext context, CombatCharacter character)
		{
			return DomainManager.Combat.ClearAffectingAgileSkillByEffect(context, character, this.CombatChar);
		}

		// Token: 0x060028D8 RID: 10456 RVA: 0x001F0864 File Offset: 0x001EEA64
		protected void ChangeDurability(DataContext context, CombatCharacter character, ItemKey itemKey, int delta)
		{
			DomainManager.Combat.ChangeDurability(context, character, itemKey, delta, EChangeDurabilitySourceType.Effect);
		}

		// Token: 0x060028D9 RID: 10457 RVA: 0x001F0878 File Offset: 0x001EEA78
		private void CreateFrameCounters()
		{
			int index = 0;
			foreach (int period in this.CalcFrameCounterPeriods())
			{
				this._frameCounters.Add(new FrameCounter(this, period, index++));
			}
		}

		// Token: 0x060028DA RID: 10458 RVA: 0x001F08DC File Offset: 0x001EEADC
		private void SetupFrameCounters()
		{
			foreach (FrameCounter counter in this._frameCounters)
			{
				counter.Setup();
			}
		}

		// Token: 0x060028DB RID: 10459 RVA: 0x001F0934 File Offset: 0x001EEB34
		private void CloseFrameCounters()
		{
			foreach (FrameCounter counter in this._frameCounters)
			{
				counter.Close();
			}
		}

		// Token: 0x060028DC RID: 10460 RVA: 0x001F098C File Offset: 0x001EEB8C
		protected virtual IEnumerable<int> CalcFrameCounterPeriods()
		{
			yield break;
		}

		// Token: 0x060028DD RID: 10461 RVA: 0x001F099C File Offset: 0x001EEB9C
		public virtual bool IsOn(int counterType)
		{
			return true;
		}

		// Token: 0x060028DE RID: 10462 RVA: 0x001F099F File Offset: 0x001EEB9F
		public virtual void OnProcess(DataContext context, int counterType)
		{
		}

		// Token: 0x060028DF RID: 10463 RVA: 0x001F09A2 File Offset: 0x001EEBA2
		protected bool IsMonitored(DataUid uid)
		{
			List<DataUid> listeningDataUids = this._listeningDataUids;
			return listeningDataUids != null && listeningDataUids.Contains(uid);
		}

		// Token: 0x060028E0 RID: 10464 RVA: 0x001F09B8 File Offset: 0x001EEBB8
		protected void AutoMonitor(DataUid uid, Action<DataContext, DataUid> action)
		{
			bool flag = this.IsMonitored(uid);
			if (!flag)
			{
				if (this._listeningDataUids == null)
				{
					this._listeningDataUids = new List<DataUid>();
				}
				this._listeningDataUids.Add(uid);
				GameDataBridge.AddPostDataModificationHandler(uid, this.DataHandlerKey, action);
			}
		}

		// Token: 0x060028E1 RID: 10465 RVA: 0x001F0A04 File Offset: 0x001EEC04
		protected void InterruptMonitor(DataUid uid)
		{
			bool flag = !this.IsMonitored(uid);
			if (!flag)
			{
				this._listeningDataUids.Remove(uid);
				GameDataBridge.RemovePostDataModificationHandler(uid, this.DataHandlerKey);
			}
		}

		// Token: 0x060028E2 RID: 10466 RVA: 0x001F0A3C File Offset: 0x001EEC3C
		protected void ClearMonitors()
		{
			List<DataUid> listeningDataUids = this._listeningDataUids;
			bool flag = listeningDataUids == null || listeningDataUids.Count <= 0;
			if (!flag)
			{
				foreach (DataUid uid in this._listeningDataUids)
				{
					GameDataBridge.RemovePostDataModificationHandler(uid, this.DataHandlerKey);
				}
				this._listeningDataUids.Clear();
			}
		}

		// Token: 0x04000826 RID: 2086
		public long Id;

		// Token: 0x04000828 RID: 2088
		public int Type;

		// Token: 0x04000829 RID: 2089
		public Dictionary<AffectedDataKey, EDataModifyType> AffectDatas;

		// Token: 0x0400082A RID: 2090
		public GameData.Domains.Character.Character CharObj;

		// Token: 0x0400082B RID: 2091
		private readonly List<FrameCounter> _frameCounters = new List<FrameCounter>();

		// Token: 0x0400082C RID: 2092
		private List<DataUid> _listeningDataUids;
	}
}
