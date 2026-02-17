using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Common.Neigong
{
	// Token: 0x02000572 RID: 1394
	public class ChangeNeiliAllocation : CombatSkillEffectBase
	{
		// Token: 0x06004124 RID: 16676 RVA: 0x0026183C File Offset: 0x0025FA3C
		protected ChangeNeiliAllocation()
		{
		}

		// Token: 0x06004125 RID: 16677 RVA: 0x00261846 File Offset: 0x0025FA46
		protected ChangeNeiliAllocation(CombatSkillKey skillKey, int type) : base(skillKey, type, -1)
		{
		}

		// Token: 0x06004126 RID: 16678 RVA: 0x00261853 File Offset: 0x0025FA53
		public override void OnEnable(DataContext context)
		{
			Events.RegisterHandler_CombatBegin(new Events.OnCombatBegin(this.OnCombatBegin));
		}

		// Token: 0x06004127 RID: 16679 RVA: 0x00261868 File Offset: 0x0025FA68
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CombatBegin(new Events.OnCombatBegin(this.OnCombatBegin));
		}

		// Token: 0x06004128 RID: 16680 RVA: 0x00261880 File Offset: 0x0025FA80
		private void OnCombatBegin(DataContext context)
		{
			bool isDirect = base.IsDirect;
			if (isDirect)
			{
				base.AppendAffectedData(context, base.CharacterId, 135, EDataModifyType.AddPercent, -1);
				base.AppendAffectedData(context, base.CharacterId, 136, EDataModifyType.AddPercent, -1);
			}
			else
			{
				base.AppendAffectedAllEnemyData(context, 135, EDataModifyType.AddPercent, -1);
				base.AppendAffectedAllEnemyData(context, 136, EDataModifyType.AddPercent, -1);
			}
			this.DoChangeNeiliAllocation(context);
			bool isCurrent = base.IsCurrent;
			if (isCurrent)
			{
				base.ShowSpecialEffectTips(0);
			}
		}

		// Token: 0x06004129 RID: 16681 RVA: 0x00261900 File Offset: 0x0025FB00
		private void DoChangeNeiliAllocation(DataContext context)
		{
			bool isDirect = base.IsDirect;
			if (isDirect)
			{
				base.CombatChar.ChangeNeiliAllocation(context, this.AffectNeiliAllocationType, 60, false, true);
			}
			else
			{
				bool isCurrent = base.IsCurrent;
				if (isCurrent)
				{
					foreach (CombatCharacter enemyChar in DomainManager.Combat.GetCharacters(!base.CombatChar.IsAlly))
					{
						enemyChar.ChangeNeiliAllocation(context, this.AffectNeiliAllocationType, -30, false, true);
					}
				}
			}
		}

		// Token: 0x0600412A RID: 16682 RVA: 0x0026199C File Offset: 0x0025FB9C
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = !base.IsDirect && !base.IsCurrent;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				ushort fieldId = dataKey.FieldId;
				bool flag2 = fieldId - 135 <= 1;
				bool flag3 = flag2 && dataKey.CustomParam0 == (int)this.AffectNeiliAllocationType;
				if (flag3)
				{
					result = (base.IsDirect ? ((dataKey.FieldId == 135) ? 60 : -30) : ((dataKey.FieldId == 135) ? -30 : 60));
				}
				else
				{
					result = 0;
				}
			}
			return result;
		}

		// Token: 0x0600412B RID: 16683 RVA: 0x00261A34 File Offset: 0x0025FC34
		protected override int GetSubClassSerializedSize()
		{
			return base.GetSubClassSerializedSize() + 1 + 1;
		}

		// Token: 0x0600412C RID: 16684 RVA: 0x00261A50 File Offset: 0x0025FC50
		protected unsafe override int SerializeSubClass(byte* pData)
		{
			byte* pCurrData = pData + base.SerializeSubClass(pData);
			*pCurrData = this.AffectNeiliAllocationType;
			return this.GetSubClassSerializedSize();
		}

		// Token: 0x0600412D RID: 16685 RVA: 0x00261A7C File Offset: 0x0025FC7C
		protected unsafe override int DeserializeSubClass(byte* pData)
		{
			byte* pCurrData = pData + base.DeserializeSubClass(pData);
			this.AffectNeiliAllocationType = *pCurrData;
			return this.GetSubClassSerializedSize();
		}

		// Token: 0x04001328 RID: 4904
		private const sbyte AddValue = 60;

		// Token: 0x04001329 RID: 4905
		private const sbyte ReduceValue = -30;

		// Token: 0x0400132A RID: 4906
		private const sbyte DirectChangeAddPercent = 60;

		// Token: 0x0400132B RID: 4907
		private const sbyte DirectChangeCostPercent = -30;

		// Token: 0x0400132C RID: 4908
		private const sbyte ReverseChangeAddPercent = -30;

		// Token: 0x0400132D RID: 4909
		private const sbyte ReverseChangeCostPercent = 60;

		// Token: 0x0400132E RID: 4910
		protected byte AffectNeiliAllocationType;
	}
}
