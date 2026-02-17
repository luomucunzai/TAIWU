using System;

namespace GameData.Domains.TaiwuEvent.EventOption
{
	// Token: 0x020000BD RID: 189
	public class OptionConditionFavor : TaiwuEventOptionConditionBase
	{
		// Token: 0x06001BC4 RID: 7108 RVA: 0x0017DC28 File Offset: 0x0017BE28
		public OptionConditionFavor(short id, sbyte favorType, Func<int, int, sbyte, bool> checkFunc) : base(id)
		{
			this.FavorType = favorType;
			this.ConditionChecker = checkFunc;
		}

		// Token: 0x06001BC5 RID: 7109 RVA: 0x0017DCC8 File Offset: 0x0017BEC8
		public override bool CheckCondition(EventArgBox box)
		{
			int charId = -1;
			bool flag = box.Get("CharacterId", ref charId);
			return flag && this.ConditionChecker(charId, DomainManager.Taiwu.GetTaiwuCharId(), this.FavorType);
		}

		// Token: 0x06001BC6 RID: 7110 RVA: 0x0017DD10 File Offset: 0x0017BF10
		public override ValueTuple<short, string[]> GetDisplayData(EventArgBox box)
		{
			return new ValueTuple<short, string[]>(this.Id, new string[]
			{
				"<Language Key=" + this.FavorTypeKeys[(int)(this.FavorType - -6)] + "/>"
			});
		}

		// Token: 0x04000670 RID: 1648
		public readonly sbyte FavorType;

		// Token: 0x04000671 RID: 1649
		public readonly Func<int, int, sbyte, bool> ConditionChecker;

		// Token: 0x04000672 RID: 1650
		private readonly string[] FavorTypeKeys = new string[]
		{
			"LK_Favor_Type_0",
			"LK_Favor_Type_1",
			"LK_Favor_Type_2",
			"LK_Favor_Type_3",
			"LK_Favor_Type_4",
			"LK_Favor_Type_5",
			"LK_Favor_Type_6",
			"LK_Favor_Type_7",
			"LK_Favor_Type_8",
			"LK_Favor_Type_9",
			"LK_Favor_Type_10",
			"LK_Favor_Type_11",
			"LK_Favor_Type_12"
		};
	}
}
