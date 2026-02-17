using System;
using System.Collections.Generic;
using System.Linq;
using GameData.Combat.Math;
using GameData.Common;

namespace GameData.Domains.SpecialEffect.SectStory.Shaolin
{
	// Token: 0x020000EE RID: 238
	public abstract class ChangeSubAttributeBase : DemonSlayerTrialEffectBase
	{
		// Token: 0x170000EB RID: 235
		// (get) Token: 0x06002977 RID: 10615
		protected abstract IReadOnlyList<ushort> SubAttributes { get; }

		// Token: 0x06002978 RID: 10616 RVA: 0x00200DFC File Offset: 0x001FEFFC
		protected ChangeSubAttributeBase(int charId, IReadOnlyList<int> parameters) : base(charId)
		{
			this._reduceBonus = -parameters[0];
		}

		// Token: 0x06002979 RID: 10617 RVA: 0x00200E1C File Offset: 0x001FF01C
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			foreach (ushort fieldId in this.SubAttributes)
			{
				base.CreateAffectedData(fieldId, EDataModifyType.Custom, -1);
			}
		}

		// Token: 0x0600297A RID: 10618 RVA: 0x00200E78 File Offset: 0x001FF078
		public override int GetModifiedValue(AffectedDataKey dataKey, int dataValue)
		{
			bool flag = this.SubAttributes.Contains(dataKey.FieldId);
			int result;
			if (flag)
			{
				result = dataValue * this._reduceBonus;
			}
			else
			{
				result = base.GetModifiedValue(dataKey, dataValue);
			}
			return result;
		}

		// Token: 0x04000CCB RID: 3275
		private readonly CValuePercentBonus _reduceBonus;
	}
}
