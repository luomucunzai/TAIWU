using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.Taiwu.Profession;

namespace GameData.Domains.SpecialEffect.Profession.Hunter
{
	// Token: 0x02000116 RID: 278
	public class HuntingBeasts : ProfessionEffectBase
	{
		// Token: 0x1700011F RID: 287
		// (get) Token: 0x06002A0F RID: 10767 RVA: 0x00201BD9 File Offset: 0x001FFDD9
		protected override short CombatStateId
		{
			get
			{
				return 243;
			}
		}

		// Token: 0x06002A10 RID: 10768 RVA: 0x00201BE0 File Offset: 0x001FFDE0
		public HuntingBeasts()
		{
		}

		// Token: 0x06002A11 RID: 10769 RVA: 0x00201BEA File Offset: 0x001FFDEA
		public HuntingBeasts(int charId) : base(charId)
		{
		}

		// Token: 0x06002A12 RID: 10770 RVA: 0x00201BF8 File Offset: 0x001FFDF8
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			ProfessionData professionData = DomainManager.Extra.GetProfessionData(1);
			this._addPercent = professionData.GetSeniorityHunterAnimalBonus();
			foreach (ushort fieldId in HuntingBeasts.AllFieldIds)
			{
				base.CreateAffectedData(fieldId, EDataModifyType.AddPercent, -1);
			}
		}

		// Token: 0x06002A13 RID: 10771 RVA: 0x00201C70 File Offset: 0x001FFE70
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || !HuntingBeasts.AllFieldIds.Contains(dataKey.FieldId);
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				result = this._addPercent;
			}
			return result;
		}

		// Token: 0x04000CE5 RID: 3301
		private static readonly List<ushort> AllFieldIds = new List<ushort>
		{
			32,
			33,
			34,
			35,
			38,
			39,
			40,
			41,
			44,
			45,
			46,
			47
		};

		// Token: 0x04000CE6 RID: 3302
		private int _addPercent;
	}
}
