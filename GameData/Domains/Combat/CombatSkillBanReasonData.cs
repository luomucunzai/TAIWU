using System;
using System.Collections.Generic;
using Config;
using GameData.Domains.Character;
using GameData.Domains.CombatSkill;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Combat
{
	// Token: 0x020006BA RID: 1722
	[SerializableGameData(NotForArchive = true)]
	public struct CombatSkillBanReasonData : ISerializableGameData
	{
		// Token: 0x170003DF RID: 991
		// (get) Token: 0x06006626 RID: 26150 RVA: 0x003AA87C File Offset: 0x003A8A7C
		public ECombatSkillBanReasonType Type
		{
			get
			{
				return (ECombatSkillBanReasonType)this._internalType;
			}
		}

		// Token: 0x170003E0 RID: 992
		// (get) Token: 0x06006627 RID: 26151 RVA: 0x003AA884 File Offset: 0x003A8A84
		public int CostMobility
		{
			get
			{
				return (int)((this.Type == ECombatSkillBanReasonType.MobilityNotEnough) ? this._internalParam0 : 0);
			}
		}

		// Token: 0x170003E1 RID: 993
		// (get) Token: 0x06006628 RID: 26152 RVA: 0x003AA898 File Offset: 0x003A8A98
		public int HasMobility
		{
			get
			{
				return (int)((this.Type == ECombatSkillBanReasonType.MobilityNotEnough) ? this._internalParam1 : 0);
			}
		}

		// Token: 0x170003E2 RID: 994
		// (get) Token: 0x06006629 RID: 26153 RVA: 0x003AA8AC File Offset: 0x003A8AAC
		public int CostBreath
		{
			get
			{
				return (int)((this.Type == ECombatSkillBanReasonType.BreathNotEnough) ? this._internalParam0 : 0);
			}
		}

		// Token: 0x170003E3 RID: 995
		// (get) Token: 0x0600662A RID: 26154 RVA: 0x003AA8C0 File Offset: 0x003A8AC0
		public int HasBreath
		{
			get
			{
				return (int)((this.Type == ECombatSkillBanReasonType.BreathNotEnough) ? this._internalParam1 : 0);
			}
		}

		// Token: 0x170003E4 RID: 996
		// (get) Token: 0x0600662B RID: 26155 RVA: 0x003AA8D4 File Offset: 0x003A8AD4
		public int CostStance
		{
			get
			{
				return (int)((this.Type == ECombatSkillBanReasonType.StanceNotEnough) ? this._internalParam0 : 0);
			}
		}

		// Token: 0x170003E5 RID: 997
		// (get) Token: 0x0600662C RID: 26156 RVA: 0x003AA8E8 File Offset: 0x003A8AE8
		public int HasStance
		{
			get
			{
				return (int)((this.Type == ECombatSkillBanReasonType.StanceNotEnough) ? this._internalParam1 : 0);
			}
		}

		// Token: 0x170003E6 RID: 998
		// (get) Token: 0x0600662D RID: 26157 RVA: 0x003AA8FC File Offset: 0x003A8AFC
		public int CostWug
		{
			get
			{
				return (int)((this.Type == ECombatSkillBanReasonType.WugNotEnough) ? this._internalParam0 : 0);
			}
		}

		// Token: 0x170003E7 RID: 999
		// (get) Token: 0x0600662E RID: 26158 RVA: 0x003AA910 File Offset: 0x003A8B10
		public int HasWug
		{
			get
			{
				return (int)((this.Type == ECombatSkillBanReasonType.WugNotEnough) ? this._internalParam1 : 0);
			}
		}

		// Token: 0x170003E8 RID: 1000
		// (get) Token: 0x0600662F RID: 26159 RVA: 0x003AA924 File Offset: 0x003A8B24
		public int CostNeiliAllocationType
		{
			get
			{
				return (int)((this.Type == ECombatSkillBanReasonType.NeiliAllocationNotEnough) ? this._internalParam0 : -1);
			}
		}

		// Token: 0x170003E9 RID: 1001
		// (get) Token: 0x06006630 RID: 26160 RVA: 0x003AA938 File Offset: 0x003A8B38
		public int CostNeiliAllocationValue
		{
			get
			{
				return (int)((this.Type == ECombatSkillBanReasonType.NeiliAllocationNotEnough) ? this._internalParam1 : 0);
			}
		}

		// Token: 0x170003EA RID: 1002
		// (get) Token: 0x06006631 RID: 26161 RVA: 0x003AA94C File Offset: 0x003A8B4C
		public int HasNeiliAllocationValue
		{
			get
			{
				return (int)((this.Type == ECombatSkillBanReasonType.NeiliAllocationNotEnough) ? this._internalParam2 : 0);
			}
		}

		// Token: 0x06006632 RID: 26162 RVA: 0x003AA960 File Offset: 0x003A8B60
		public CombatSkillBanReasonData(ECombatSkillBanReasonType type, short templateId, GameData.Domains.CombatSkill.CombatSkill combatSkill, CombatCharacter combatChar)
		{
			this._internalType = (sbyte)type;
			this._internalParam0 = (this._internalParam1 = (this._internalParam2 = 0));
			this.CostTricks = (this.HasTricks = null);
			CombatSkillItem configData = Config.CombatSkill.Instance[templateId];
			switch (type)
			{
			case ECombatSkillBanReasonType.None:
			case ECombatSkillBanReasonType.Undefined:
				break;
			case ECombatSkillBanReasonType.StanceNotEnough:
				this._internalParam0 = (sbyte)DomainManager.Combat.GetSkillCostBreathStance(combatChar.GetId(), combatSkill).Outer;
				this._internalParam1 = (sbyte)Math.Clamp(combatChar.GetStanceValue() * 100 / 4000, 0, 100);
				break;
			case ECombatSkillBanReasonType.BreathNotEnough:
				this._internalParam0 = (sbyte)DomainManager.Combat.GetSkillCostBreathStance(combatChar.GetId(), combatSkill).Inner;
				this._internalParam1 = (sbyte)Math.Clamp(combatChar.GetBreathValue() * 100 / 30000, 0, 100);
				break;
			case ECombatSkillBanReasonType.MobilityNotEnough:
				this._internalParam0 = combatSkill.GetCostMobilityPercent();
				this._internalParam1 = (sbyte)Math.Clamp(combatChar.GetMobilityValue() * 100 / MoveSpecialConstants.MaxMobility, 0, 100);
				break;
			case ECombatSkillBanReasonType.TrickNotEnough:
				this.CostTricks = new List<NeedTrick>();
				this.HasTricks = new List<NeedTrick>();
				DomainManager.CombatSkill.GetCombatSkillCostTrick(combatSkill, this.CostTricks, true);
				foreach (NeedTrick needTrick in this.CostTricks)
				{
					NeedTrick needTrick2 = needTrick;
					needTrick2.NeedCount = combatChar.GetTrickCount(needTrick.TrickType);
					NeedTrick hasTrick = needTrick2;
					this.HasTricks.Add(hasTrick);
				}
				break;
			case ECombatSkillBanReasonType.WugNotEnough:
				this._internalParam0 = configData.WugCost;
				this._internalParam1 = (sbyte)Math.Clamp((int)combatChar.GetWugCount(), 0, 127);
				break;
			case ECombatSkillBanReasonType.NeiliAllocationNotEnough:
				this.SetNeiliAllocationParam(combatSkill, combatChar);
				break;
			case ECombatSkillBanReasonType.WeaponTrickMismatch:
			case ECombatSkillBanReasonType.WeaponDestroyed:
			case ECombatSkillBanReasonType.BodyPartBroken:
			case ECombatSkillBanReasonType.SpecialEffectBan:
			case ECombatSkillBanReasonType.CombatConfigBan:
			case ECombatSkillBanReasonType.Silencing:
				break;
			default:
				throw new ArgumentOutOfRangeException("type", type, null);
			}
		}

		// Token: 0x06006633 RID: 26163 RVA: 0x003AAB90 File Offset: 0x003A8D90
		private unsafe void SetNeiliAllocationParam(GameData.Domains.CombatSkill.CombatSkill combatSkill, CombatCharacter combatChar)
		{
			ValueTuple<sbyte, sbyte> costNeiliAllocation = combatSkill.GetCostNeiliAllocation();
			NeiliAllocation currNeiliAllocation = combatChar.GetNeiliAllocation();
			this._internalParam0 = costNeiliAllocation.Item1;
			bool flag = this._internalParam0 < 0;
			if (!flag)
			{
				this._internalParam1 = costNeiliAllocation.Item2;
				this._internalParam2 = (sbyte)Math.Clamp((int)(*(ref currNeiliAllocation.Items.FixedElementField + (IntPtr)this._internalParam0 * 2)), 0, 127);
			}
		}

		// Token: 0x06006634 RID: 26164 RVA: 0x003AABFC File Offset: 0x003A8DFC
		public CombatSkillBanReasonData(CombatSkillBanReasonData other)
		{
			this._internalType = other._internalType;
			this._internalParam0 = other._internalParam0;
			this._internalParam1 = other._internalParam1;
			this._internalParam2 = other._internalParam2;
			this.CostTricks = new List<NeedTrick>(other.CostTricks);
			this.HasTricks = new List<NeedTrick>(other.HasTricks);
		}

		// Token: 0x06006635 RID: 26165 RVA: 0x003AAC5C File Offset: 0x003A8E5C
		public void Assign(CombatSkillBanReasonData other)
		{
			this._internalType = other._internalType;
			this._internalParam0 = other._internalParam0;
			this._internalParam1 = other._internalParam1;
			this._internalParam2 = other._internalParam2;
			this.CostTricks = new List<NeedTrick>(other.CostTricks);
			this.HasTricks = new List<NeedTrick>(other.HasTricks);
		}

		// Token: 0x06006636 RID: 26166 RVA: 0x003AACBC File Offset: 0x003A8EBC
		public bool IsSerializedSizeFixed()
		{
			return false;
		}

		// Token: 0x06006637 RID: 26167 RVA: 0x003AACD0 File Offset: 0x003A8ED0
		public int GetSerializedSize()
		{
			int totalSize = 4;
			bool flag = this.CostTricks != null;
			if (flag)
			{
				totalSize += 2 + 4 * this.CostTricks.Count;
			}
			else
			{
				totalSize += 2;
			}
			bool flag2 = this.HasTricks != null;
			if (flag2)
			{
				totalSize += 2 + 4 * this.HasTricks.Count;
			}
			else
			{
				totalSize += 2;
			}
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x06006638 RID: 26168 RVA: 0x003AAD40 File Offset: 0x003A8F40
		public unsafe int Serialize(byte* pData)
		{
			*pData = (byte)this._internalType;
			byte* pCurrData = pData + 1;
			*pCurrData = (byte)this._internalParam0;
			pCurrData++;
			*pCurrData = (byte)this._internalParam1;
			pCurrData++;
			*pCurrData = (byte)this._internalParam2;
			pCurrData++;
			bool flag = this.CostTricks != null;
			if (flag)
			{
				int elementsCount = this.CostTricks.Count;
				Tester.Assert(elementsCount <= 65535, "");
				*(short*)pCurrData = (short)((ushort)elementsCount);
				pCurrData += 2;
				for (int i = 0; i < elementsCount; i++)
				{
					pCurrData += this.CostTricks[i].Serialize(pCurrData);
				}
			}
			else
			{
				*(short*)pCurrData = 0;
				pCurrData += 2;
			}
			bool flag2 = this.HasTricks != null;
			if (flag2)
			{
				int elementsCount2 = this.HasTricks.Count;
				Tester.Assert(elementsCount2 <= 65535, "");
				*(short*)pCurrData = (short)((ushort)elementsCount2);
				pCurrData += 2;
				for (int j = 0; j < elementsCount2; j++)
				{
					pCurrData += this.HasTricks[j].Serialize(pCurrData);
				}
			}
			else
			{
				*(short*)pCurrData = 0;
				pCurrData += 2;
			}
			int totalSize = (int)((long)(pCurrData - pData));
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x06006639 RID: 26169 RVA: 0x003AAE90 File Offset: 0x003A9090
		public unsafe int Deserialize(byte* pData)
		{
			this._internalType = *(sbyte*)pData;
			byte* pCurrData = pData + 1;
			this._internalParam0 = *(sbyte*)pCurrData;
			pCurrData++;
			this._internalParam1 = *(sbyte*)pCurrData;
			pCurrData++;
			this._internalParam2 = *(sbyte*)pCurrData;
			pCurrData++;
			ushort elementsCount = *(ushort*)pCurrData;
			pCurrData += 2;
			bool flag = elementsCount > 0;
			if (flag)
			{
				bool flag2 = this.CostTricks == null;
				if (flag2)
				{
					this.CostTricks = new List<NeedTrick>((int)elementsCount);
				}
				else
				{
					this.CostTricks.Clear();
				}
				for (int i = 0; i < (int)elementsCount; i++)
				{
					NeedTrick element = default(NeedTrick);
					pCurrData += element.Deserialize(pCurrData);
					this.CostTricks.Add(element);
				}
			}
			else
			{
				List<NeedTrick> costTricks = this.CostTricks;
				if (costTricks != null)
				{
					costTricks.Clear();
				}
			}
			ushort elementsCount2 = *(ushort*)pCurrData;
			pCurrData += 2;
			bool flag3 = elementsCount2 > 0;
			if (flag3)
			{
				bool flag4 = this.HasTricks == null;
				if (flag4)
				{
					this.HasTricks = new List<NeedTrick>((int)elementsCount2);
				}
				else
				{
					this.HasTricks.Clear();
				}
				for (int j = 0; j < (int)elementsCount2; j++)
				{
					NeedTrick element2 = default(NeedTrick);
					pCurrData += element2.Deserialize(pCurrData);
					this.HasTricks.Add(element2);
				}
			}
			else
			{
				List<NeedTrick> hasTricks = this.HasTricks;
				if (hasTricks != null)
				{
					hasTricks.Clear();
				}
			}
			int totalSize = (int)((long)(pCurrData - pData));
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x04001BAD RID: 7085
		[SerializableGameDataField]
		private sbyte _internalType;

		// Token: 0x04001BAE RID: 7086
		[SerializableGameDataField]
		private sbyte _internalParam0;

		// Token: 0x04001BAF RID: 7087
		[SerializableGameDataField]
		private sbyte _internalParam1;

		// Token: 0x04001BB0 RID: 7088
		[SerializableGameDataField]
		private sbyte _internalParam2;

		// Token: 0x04001BB1 RID: 7089
		[SerializableGameDataField]
		public List<NeedTrick> CostTricks;

		// Token: 0x04001BB2 RID: 7090
		[SerializableGameDataField]
		public List<NeedTrick> HasTricks;
	}
}
