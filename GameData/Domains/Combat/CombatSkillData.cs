using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Config;
using GameData.ArchiveData;
using GameData.Common;
using GameData.Dependencies;
using GameData.DomainEvents;
using GameData.Domains.CombatSkill;
using GameData.Serializer;

namespace GameData.Domains.Combat
{
	// Token: 0x020006BC RID: 1724
	[SerializableGameData(NotForDisplayModule = true)]
	public class CombatSkillData : BaseGameDataObject, ISerializableGameData
	{
		// Token: 0x0600663A RID: 26170 RVA: 0x003AB008 File Offset: 0x003A9208
		[ObjectCollectionDependency(8, 29, new ushort[]
		{
			1
		}, Scope = InfluenceScope.Self)]
		private void CalcBanReason(List<CombatSkillBanReasonData> banReason)
		{
			CombatSkillData.<>c__DisplayClass10_0 CS$<>8__locals1;
			CS$<>8__locals1.banReason = banReason;
			CS$<>8__locals1.banReason.Clear();
			bool canUse = this._canUse;
			if (!canUse)
			{
				int charId = this._id.CharId;
				CS$<>8__locals1.skillId = this._id.SkillTemplateId;
				bool flag = !DomainManager.Combat.TryGetElement_CombatCharacterDict(charId, out CS$<>8__locals1.combatChar);
				if (!flag)
				{
					bool flag2 = !DomainManager.CombatSkill.TryGetElement_CombatSkills(new CombatSkillKey(charId, CS$<>8__locals1.skillId), out CS$<>8__locals1.combatSkill);
					if (!flag2)
					{
						CombatDomain combatDomain = DomainManager.Combat;
						CombatSkillItem config = Config.CombatSkill.Instance[CS$<>8__locals1.skillId];
						bool flag3 = !combatDomain.SkillCanUseInCurrCombat(charId, config);
						if (flag3)
						{
							CombatSkillData.<CalcBanReason>g__AddReason|10_0(ECombatSkillBanReasonType.CombatConfigBan, ref CS$<>8__locals1);
						}
						else
						{
							bool flag4 = this._leftCdFrame != 0;
							if (flag4)
							{
								CombatSkillData.<CalcBanReason>g__AddReason|10_0(ECombatSkillBanReasonType.Silencing, ref CS$<>8__locals1);
							}
							else
							{
								bool flag5 = !combatDomain.HasSkillNeedBodyPart(CS$<>8__locals1.combatChar, CS$<>8__locals1.skillId, true);
								if (flag5)
								{
									CombatSkillData.<CalcBanReason>g__AddReason|10_0(ECombatSkillBanReasonType.BodyPartBroken, ref CS$<>8__locals1);
								}
								else
								{
									bool flag6 = config.EquipType == 1 && !combatDomain.WeaponHasNeedTrick(CS$<>8__locals1.combatChar, CS$<>8__locals1.skillId, combatDomain.GetUsingWeaponData(CS$<>8__locals1.combatChar));
									if (flag6)
									{
										CombatSkillData.<CalcBanReason>g__AddReason|10_0(ECombatSkillBanReasonType.WeaponTrickMismatch, ref CS$<>8__locals1);
									}
									else
									{
										bool flag7 = !combatDomain.SkillCostEnough(CS$<>8__locals1.combatChar, CS$<>8__locals1.skillId);
										if (flag7)
										{
											foreach (ECombatSkillBanReasonType type in DomainManager.Combat.CalcSkillCostEnoughBanReasons(CS$<>8__locals1.combatChar, CS$<>8__locals1.skillId))
											{
												CombatSkillData.<CalcBanReason>g__AddReason|10_0(type, ref CS$<>8__locals1);
											}
										}
										bool flag8 = CS$<>8__locals1.banReason.Count == 0;
										if (flag8)
										{
											CombatSkillData.<CalcBanReason>g__AddReason|10_0(ECombatSkillBanReasonType.SpecialEffectBan, ref CS$<>8__locals1);
										}
									}
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x0600663B RID: 26171 RVA: 0x003AB200 File Offset: 0x003A9400
		[ObjectCollectionDependency(17, 2, new ushort[]
		{
			253
		}, Scope = InfluenceScope.CombatSkillDataAffectedByTheSpecialEffects)]
		private void CalcEffectData(List<CombatSkillEffectData> effectData)
		{
			effectData.Clear();
			DomainManager.SpecialEffect.ModifyData(this._id.CharId, this._id.SkillTemplateId, 253, effectData, -1, -1, -1);
		}

		// Token: 0x0600663C RID: 26172 RVA: 0x003AB234 File Offset: 0x003A9434
		[ObjectCollectionDependency(17, 2, new ushort[]
		{
			287,
			285,
			286
		}, Scope = InfluenceScope.CombatSkillDataAffectedByTheSpecialEffects)]
		private bool CalcCanAffect()
		{
			sbyte equipType = Config.CombatSkill.Instance[this._id.SkillTemplateId].EquipType;
			if (!true)
			{
			}
			ushort num;
			switch (equipType)
			{
			case 2:
				num = 287;
				break;
			case 3:
				num = 285;
				break;
			case 4:
				num = 286;
				break;
			default:
				num = ushort.MaxValue;
				break;
			}
			if (!true)
			{
			}
			ushort fieldId = num;
			return fieldId == ushort.MaxValue || DomainManager.SpecialEffect.ModifyData(this._id.CharId, this._id.SkillTemplateId, fieldId, true, -1, -1, -1);
		}

		// Token: 0x0600663D RID: 26173 RVA: 0x003AB2D1 File Offset: 0x003A94D1
		public CombatSkillData(CombatSkillKey id) : this()
		{
			this._id = id;
		}

		// Token: 0x0600663E RID: 26174 RVA: 0x003AB2E2 File Offset: 0x003A94E2
		public void RaiseSkillSilence(DataContext context)
		{
			this.SetSilencing(true, context);
			Events.RaiseSkillSilence(context, this._id);
		}

		// Token: 0x0600663F RID: 26175 RVA: 0x003AB2FB File Offset: 0x003A94FB
		public void RaiseSkillSilenceEnd(DataContext context)
		{
			this.SetSilencing(false, context);
			Events.RaiseSkillSilenceEnd(context, this._id);
		}

		// Token: 0x06006640 RID: 26176 RVA: 0x003AB314 File Offset: 0x003A9514
		public CombatSkillKey GetId()
		{
			return this._id;
		}

		// Token: 0x06006641 RID: 26177 RVA: 0x003AB32C File Offset: 0x003A952C
		public unsafe void SetId(CombatSkillKey id, DataContext context)
		{
			this._id = id;
			base.SetModifiedAndInvalidateInfluencedCache(0, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				byte* pData = OperationAdder.DynamicObjectCollection_SetFixedField<CombatSkillKey>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 0U, 8);
				pData += this._id.Serialize(pData);
			}
		}

		// Token: 0x06006642 RID: 26178 RVA: 0x003AB390 File Offset: 0x003A9590
		public bool GetCanUse()
		{
			return this._canUse;
		}

		// Token: 0x06006643 RID: 26179 RVA: 0x003AB3A8 File Offset: 0x003A95A8
		public unsafe void SetCanUse(bool canUse, DataContext context)
		{
			this._canUse = canUse;
			base.SetModifiedAndInvalidateInfluencedCache(1, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				byte* pData = OperationAdder.DynamicObjectCollection_SetFixedField<CombatSkillKey>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 8U, 1);
				*pData = (this._canUse ? 1 : 0);
				pData++;
			}
		}

		// Token: 0x06006644 RID: 26180 RVA: 0x003AB408 File Offset: 0x003A9608
		public short GetLeftCdFrame()
		{
			return this._leftCdFrame;
		}

		// Token: 0x06006645 RID: 26181 RVA: 0x003AB420 File Offset: 0x003A9620
		public unsafe void SetLeftCdFrame(short leftCdFrame, DataContext context)
		{
			this._leftCdFrame = leftCdFrame;
			base.SetModifiedAndInvalidateInfluencedCache(2, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				byte* pData = OperationAdder.DynamicObjectCollection_SetFixedField<CombatSkillKey>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 9U, 2);
				*(short*)pData = this._leftCdFrame;
				pData += 2;
			}
		}

		// Token: 0x06006646 RID: 26182 RVA: 0x003AB480 File Offset: 0x003A9680
		public short GetTotalCdFrame()
		{
			return this._totalCdFrame;
		}

		// Token: 0x06006647 RID: 26183 RVA: 0x003AB498 File Offset: 0x003A9698
		public unsafe void SetTotalCdFrame(short totalCdFrame, DataContext context)
		{
			this._totalCdFrame = totalCdFrame;
			base.SetModifiedAndInvalidateInfluencedCache(3, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				byte* pData = OperationAdder.DynamicObjectCollection_SetFixedField<CombatSkillKey>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 11U, 2);
				*(short*)pData = this._totalCdFrame;
				pData += 2;
			}
		}

		// Token: 0x06006648 RID: 26184 RVA: 0x003AB4F8 File Offset: 0x003A96F8
		public bool GetConstAffecting()
		{
			return this._constAffecting;
		}

		// Token: 0x06006649 RID: 26185 RVA: 0x003AB510 File Offset: 0x003A9710
		public unsafe void SetConstAffecting(bool constAffecting, DataContext context)
		{
			this._constAffecting = constAffecting;
			base.SetModifiedAndInvalidateInfluencedCache(4, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				byte* pData = OperationAdder.DynamicObjectCollection_SetFixedField<CombatSkillKey>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 13U, 1);
				*pData = (this._constAffecting ? 1 : 0);
				pData++;
			}
		}

		// Token: 0x0600664A RID: 26186 RVA: 0x003AB570 File Offset: 0x003A9770
		public bool GetShowAffectTips()
		{
			return this._showAffectTips;
		}

		// Token: 0x0600664B RID: 26187 RVA: 0x003AB588 File Offset: 0x003A9788
		public unsafe void SetShowAffectTips(bool showAffectTips, DataContext context)
		{
			this._showAffectTips = showAffectTips;
			base.SetModifiedAndInvalidateInfluencedCache(5, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				byte* pData = OperationAdder.DynamicObjectCollection_SetFixedField<CombatSkillKey>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 14U, 1);
				*pData = (this._showAffectTips ? 1 : 0);
				pData++;
			}
		}

		// Token: 0x0600664C RID: 26188 RVA: 0x003AB5E8 File Offset: 0x003A97E8
		public bool GetSilencing()
		{
			return this._silencing;
		}

		// Token: 0x0600664D RID: 26189 RVA: 0x003AB600 File Offset: 0x003A9800
		public unsafe void SetSilencing(bool silencing, DataContext context)
		{
			this._silencing = silencing;
			base.SetModifiedAndInvalidateInfluencedCache(6, context);
			bool isArchive = this.CollectionHelperData.IsArchive;
			if (isArchive)
			{
				byte* pData = OperationAdder.DynamicObjectCollection_SetFixedField<CombatSkillKey>(this.CollectionHelperData.DomainId, this.CollectionHelperData.DataId, this._id, 15U, 1);
				*pData = (this._silencing ? 1 : 0);
				pData++;
			}
		}

		// Token: 0x0600664E RID: 26190 RVA: 0x003AB660 File Offset: 0x003A9860
		public List<CombatSkillBanReasonData> GetBanReason()
		{
			ObjectCollectionDataStates dataStates = this.CollectionHelperData.DataStates;
			bool flag = dataStates.IsCached(this.DataStatesOffset, 7);
			List<CombatSkillBanReasonData> banReason;
			if (flag)
			{
				banReason = this._banReason;
			}
			else
			{
				this.CalcBanReason(this._banReason);
				dataStates.SetCached(this.DataStatesOffset, 7);
				banReason = this._banReason;
			}
			return banReason;
		}

		// Token: 0x0600664F RID: 26191 RVA: 0x003AB6BC File Offset: 0x003A98BC
		public List<CombatSkillEffectData> GetEffectData()
		{
			ObjectCollectionDataStates dataStates = this.CollectionHelperData.DataStates;
			bool flag = dataStates.IsCached(this.DataStatesOffset, 8);
			List<CombatSkillEffectData> effectData;
			if (flag)
			{
				effectData = this._effectData;
			}
			else
			{
				this.CalcEffectData(this._effectData);
				dataStates.SetCached(this.DataStatesOffset, 8);
				effectData = this._effectData;
			}
			return effectData;
		}

		// Token: 0x06006650 RID: 26192 RVA: 0x003AB718 File Offset: 0x003A9918
		public bool GetCanAffect()
		{
			ObjectCollectionDataStates dataStates = this.CollectionHelperData.DataStates;
			bool flag = dataStates.IsCached(this.DataStatesOffset, 9);
			bool canAffect;
			if (flag)
			{
				canAffect = this._canAffect;
			}
			else
			{
				this._canAffect = this.CalcCanAffect();
				dataStates.SetCached(this.DataStatesOffset, 9);
				canAffect = this._canAffect;
			}
			return canAffect;
		}

		// Token: 0x06006651 RID: 26193 RVA: 0x003AB772 File Offset: 0x003A9972
		public CombatSkillData()
		{
		}

		// Token: 0x06006652 RID: 26194 RVA: 0x003AB794 File Offset: 0x003A9994
		public bool IsSerializedSizeFixed()
		{
			return true;
		}

		// Token: 0x06006653 RID: 26195 RVA: 0x003AB7A8 File Offset: 0x003A99A8
		public int GetSerializedSize()
		{
			return 16;
		}

		// Token: 0x06006654 RID: 26196 RVA: 0x003AB7C0 File Offset: 0x003A99C0
		public unsafe int Serialize(byte* pData)
		{
			byte* pCurrData = pData + this._id.Serialize(pData);
			*pCurrData = (this._canUse ? 1 : 0);
			pCurrData++;
			*(short*)pCurrData = this._leftCdFrame;
			pCurrData += 2;
			*(short*)pCurrData = this._totalCdFrame;
			pCurrData += 2;
			*pCurrData = (this._constAffecting ? 1 : 0);
			pCurrData++;
			*pCurrData = (this._showAffectTips ? 1 : 0);
			pCurrData++;
			*pCurrData = (this._silencing ? 1 : 0);
			pCurrData++;
			return (int)((long)(pCurrData - pData));
		}

		// Token: 0x06006655 RID: 26197 RVA: 0x003AB834 File Offset: 0x003A9A34
		public unsafe int Deserialize(byte* pData)
		{
			byte* pCurrData = pData + this._id.Deserialize(pData);
			this._canUse = (*pCurrData != 0);
			pCurrData++;
			this._leftCdFrame = *(short*)pCurrData;
			pCurrData += 2;
			this._totalCdFrame = *(short*)pCurrData;
			pCurrData += 2;
			this._constAffecting = (*pCurrData != 0);
			pCurrData++;
			this._showAffectTips = (*pCurrData != 0);
			pCurrData++;
			this._silencing = (*pCurrData != 0);
			pCurrData++;
			return (int)((long)(pCurrData - pData));
		}

		// Token: 0x06006656 RID: 26198 RVA: 0x003AB8A6 File Offset: 0x003A9AA6
		[CompilerGenerated]
		internal static void <CalcBanReason>g__AddReason|10_0(ECombatSkillBanReasonType banReasonType, ref CombatSkillData.<>c__DisplayClass10_0 A_1)
		{
			A_1.banReason.Add(new CombatSkillBanReasonData(banReasonType, A_1.skillId, A_1.combatSkill, A_1.combatChar));
		}

		// Token: 0x04001BC2 RID: 7106
		[CollectionObjectField(false, true, false, false, false)]
		private CombatSkillKey _id;

		// Token: 0x04001BC3 RID: 7107
		[CollectionObjectField(false, true, false, false, false)]
		private bool _canUse;

		// Token: 0x04001BC4 RID: 7108
		[CollectionObjectField(false, true, false, false, false)]
		private short _leftCdFrame;

		// Token: 0x04001BC5 RID: 7109
		[CollectionObjectField(false, true, false, false, false)]
		private short _totalCdFrame;

		// Token: 0x04001BC6 RID: 7110
		[CollectionObjectField(false, true, false, false, false)]
		private bool _silencing;

		// Token: 0x04001BC7 RID: 7111
		[CollectionObjectField(false, false, true, false, false)]
		private readonly List<CombatSkillBanReasonData> _banReason = new List<CombatSkillBanReasonData>();

		// Token: 0x04001BC8 RID: 7112
		[CollectionObjectField(false, false, true, false, false)]
		private readonly List<CombatSkillEffectData> _effectData = new List<CombatSkillEffectData>();

		// Token: 0x04001BC9 RID: 7113
		[CollectionObjectField(false, false, true, false, false)]
		private bool _canAffect;

		// Token: 0x04001BCA RID: 7114
		[CollectionObjectField(false, true, false, false, false)]
		private bool _constAffecting;

		// Token: 0x04001BCB RID: 7115
		[CollectionObjectField(false, true, false, false, false)]
		private bool _showAffectTips;

		// Token: 0x04001BCC RID: 7116
		public const int FixedSize = 16;

		// Token: 0x04001BCD RID: 7117
		public const int DynamicCount = 0;

		// Token: 0x02000B69 RID: 2921
		internal class FixedFieldInfos
		{
			// Token: 0x04003087 RID: 12423
			public const uint Id_Offset = 0U;

			// Token: 0x04003088 RID: 12424
			public const int Id_Size = 8;

			// Token: 0x04003089 RID: 12425
			public const uint CanUse_Offset = 8U;

			// Token: 0x0400308A RID: 12426
			public const int CanUse_Size = 1;

			// Token: 0x0400308B RID: 12427
			public const uint LeftCdFrame_Offset = 9U;

			// Token: 0x0400308C RID: 12428
			public const int LeftCdFrame_Size = 2;

			// Token: 0x0400308D RID: 12429
			public const uint TotalCdFrame_Offset = 11U;

			// Token: 0x0400308E RID: 12430
			public const int TotalCdFrame_Size = 2;

			// Token: 0x0400308F RID: 12431
			public const uint ConstAffecting_Offset = 13U;

			// Token: 0x04003090 RID: 12432
			public const int ConstAffecting_Size = 1;

			// Token: 0x04003091 RID: 12433
			public const uint ShowAffectTips_Offset = 14U;

			// Token: 0x04003092 RID: 12434
			public const int ShowAffectTips_Size = 1;

			// Token: 0x04003093 RID: 12435
			public const uint Silencing_Offset = 15U;

			// Token: 0x04003094 RID: 12436
			public const int Silencing_Size = 1;
		}
	}
}
