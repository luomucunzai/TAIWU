using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.Item;

namespace GameData.Domains.SpecialEffect.EquipmentEffect.RawCreate
{
	// Token: 0x02000198 RID: 408
	public class ShanHeShenJie : RawCreateEquipmentBase
	{
		// Token: 0x17000126 RID: 294
		// (get) Token: 0x06002BD0 RID: 11216 RVA: 0x00206056 File Offset: 0x00204256
		private static CValuePercent ReplacementLossPercent
		{
			get
			{
				return 50;
			}
		}

		// Token: 0x17000127 RID: 295
		// (get) Token: 0x06002BD1 RID: 11217 RVA: 0x0020605F File Offset: 0x0020425F
		protected override int ReduceDurabilityValue
		{
			get
			{
				return 4;
			}
		}

		// Token: 0x06002BD2 RID: 11218 RVA: 0x00206062 File Offset: 0x00204262
		public ShanHeShenJie()
		{
		}

		// Token: 0x06002BD3 RID: 11219 RVA: 0x0020606C File Offset: 0x0020426C
		public ShanHeShenJie(int charId, ItemKey itemKey) : base(charId, itemKey, 30201)
		{
		}

		// Token: 0x06002BD4 RID: 11220 RVA: 0x0020607D File Offset: 0x0020427D
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			base.CreateAffectedData(309, EDataModifyType.Custom, -1);
		}

		// Token: 0x06002BD5 RID: 11221 RVA: 0x00206098 File Offset: 0x00204298
		public override int GetModifiedValue(AffectedDataKey dataKey, int dataValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || dataKey.FieldId != 309 || !base.CanAffect;
			int result;
			if (flag)
			{
				result = dataValue;
			}
			else
			{
				ItemKey changingKey = base.CombatChar.ChangingDurabilityItems.Peek();
				bool flag2 = base.CombatChar.GetRawCreateCollection().EffectEquals(changingKey, this.EquipItemKey);
				if (flag2)
				{
					result = dataValue;
				}
				else
				{
					int replacementValue = dataValue * ShanHeShenJie.ReplacementLossPercent;
					bool flag3 = replacementValue >= 0;
					if (flag3)
					{
						result = dataValue;
					}
					else
					{
						dataValue -= replacementValue;
						DataContext context = DomainManager.Combat.Context;
						base.ChangeDurability(context, base.CombatChar, this.EquipItemKey, replacementValue);
						result = dataValue;
					}
				}
			}
			return result;
		}
	}
}
