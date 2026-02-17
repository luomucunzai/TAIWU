using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.SpecialEffect.Animal;

namespace GameData.Domains.SpecialEffect.SectStory.Baihua
{
	// Token: 0x02000108 RID: 264
	public class XiongZhongSiQi : CarrierEffectBase
	{
		// Token: 0x17000112 RID: 274
		// (get) Token: 0x060029CC RID: 10700 RVA: 0x00201550 File Offset: 0x001FF750
		protected override short CombatStateId
		{
			get
			{
				return 224;
			}
		}

		// Token: 0x060029CD RID: 10701 RVA: 0x00201557 File Offset: 0x001FF757
		public XiongZhongSiQi(int charId) : base(charId)
		{
		}

		// Token: 0x060029CE RID: 10702 RVA: 0x00201562 File Offset: 0x001FF762
		protected override void OnEnableSubClass(DataContext context)
		{
			base.CreateAffectedData(8, EDataModifyType.Custom, -1);
			base.CreateAffectedData(7, EDataModifyType.Custom, -1);
		}

		// Token: 0x060029CF RID: 10703 RVA: 0x0020157C File Offset: 0x001FF77C
		public override int GetModifiedValue(AffectedDataKey dataKey, int dataValue)
		{
			bool flag = dataKey.CharId != base.CharacterId;
			bool flag2 = flag;
			if (!flag2)
			{
				ushort fieldId = dataKey.FieldId;
				bool flag3 = fieldId - 7 <= 1;
				flag2 = !flag3;
			}
			bool flag4 = flag2;
			int result;
			if (flag4)
			{
				result = dataValue;
			}
			else
			{
				result = 0;
			}
			return result;
		}
	}
}
