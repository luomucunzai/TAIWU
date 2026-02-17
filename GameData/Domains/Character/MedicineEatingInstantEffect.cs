using System;
using System.Collections.Generic;
using Config;

namespace GameData.Domains.Character
{
	// Token: 0x02000815 RID: 2069
	public readonly ref struct MedicineEatingInstantEffect
	{
		// Token: 0x17000481 RID: 1153
		// (get) Token: 0x060074A3 RID: 29859 RVA: 0x004458F8 File Offset: 0x00443AF8
		public sbyte PoisonType
		{
			get
			{
				return this.EffectSubType.PoisonType();
			}
		}

		// Token: 0x17000482 RID: 1154
		// (get) Token: 0x060074A4 RID: 29860 RVA: 0x00445905 File Offset: 0x00443B05
		public sbyte DetoxPoisonType
		{
			get
			{
				return this.EffectSubType.DetoxPoisonType();
			}
		}

		// Token: 0x17000483 RID: 1155
		// (get) Token: 0x060074A5 RID: 29861 RVA: 0x00445912 File Offset: 0x00443B12
		public sbyte ApplyPoisonType
		{
			get
			{
				return this.EffectSubType.ApplyPoisonType();
			}
		}

		// Token: 0x17000484 RID: 1156
		// (get) Token: 0x060074A6 RID: 29862 RVA: 0x0044591F File Offset: 0x00443B1F
		public sbyte DetoxWugType
		{
			get
			{
				return EMedicineEffectSubTypeExtension.DetoxWugType(this.EffectType, this.SideEffectValue);
			}
		}

		// Token: 0x17000485 RID: 1157
		// (get) Token: 0x060074A7 RID: 29863 RVA: 0x00445932 File Offset: 0x00443B32
		public EMedicineEffectSubTypeExtension.Operate OperateType
		{
			get
			{
				return this.EffectSubType.OperateType();
			}
		}

		// Token: 0x17000486 RID: 1158
		// (get) Token: 0x060074A8 RID: 29864 RVA: 0x0044593F File Offset: 0x00443B3F
		public bool EffectIsPercentage
		{
			get
			{
				return this.EffectSubType.IsPercentage();
			}
		}

		// Token: 0x17000487 RID: 1159
		// (get) Token: 0x060074A9 RID: 29865 RVA: 0x0044594C File Offset: 0x00443B4C
		public bool EffectIsValue
		{
			get
			{
				return this.EffectSubType.IsValue();
			}
		}

		// Token: 0x060074AA RID: 29866 RVA: 0x0044595C File Offset: 0x00443B5C
		public MedicineEatingInstantEffect(MedicineItem cfg, IReadOnlyList<sbyte> targetBodyParts = null)
		{
			this.Grade = cfg.Grade;
			this.EffectType = cfg.EffectType;
			this.EffectSubType = cfg.EffectSubType;
			this.EffectThresholdValue = cfg.EffectThresholdValue;
			this.EffectValue = cfg.EffectValue;
			this.InjuryRecoveryTimes = cfg.InjuryRecoveryTimes;
			this.SideEffectValue = cfg.SideEffectValue;
			this.TargetBodyParts = targetBodyParts;
		}

		// Token: 0x060074AB RID: 29867 RVA: 0x004459C8 File Offset: 0x00443BC8
		public MedicineEatingInstantEffect(MaterialItem cfg, bool primary)
		{
			if (primary)
			{
				this.Grade = cfg.Grade;
				this.EffectType = cfg.PrimaryEffectType;
				this.EffectSubType = cfg.PrimaryEffectSubType;
				this.EffectThresholdValue = cfg.PrimaryEffectThresholdValue;
				this.EffectValue = cfg.PrimaryEffectValue;
				this.InjuryRecoveryTimes = cfg.PrimaryInjuryRecoveryTimes;
			}
			else
			{
				this.Grade = cfg.Grade;
				this.EffectType = cfg.SecondaryEffectType;
				this.EffectSubType = cfg.SecondaryEffectSubType;
				this.EffectThresholdValue = cfg.SecondaryEffectThresholdValue;
				this.EffectValue = cfg.SecondaryEffectValue;
				this.InjuryRecoveryTimes = cfg.SecondaryInjuryRecoveryTimes;
			}
			this.SideEffectValue = 0;
			this.TargetBodyParts = null;
		}

		// Token: 0x04001EE1 RID: 7905
		public readonly sbyte Grade;

		// Token: 0x04001EE2 RID: 7906
		public readonly EMedicineEffectType EffectType;

		// Token: 0x04001EE3 RID: 7907
		public readonly EMedicineEffectSubType EffectSubType;

		// Token: 0x04001EE4 RID: 7908
		public readonly short EffectThresholdValue;

		// Token: 0x04001EE5 RID: 7909
		public readonly short EffectValue;

		// Token: 0x04001EE6 RID: 7910
		public readonly sbyte InjuryRecoveryTimes;

		// Token: 0x04001EE7 RID: 7911
		public readonly short SideEffectValue;

		// Token: 0x04001EE8 RID: 7912
		public readonly IReadOnlyList<sbyte> TargetBodyParts;
	}
}
