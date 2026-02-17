using System;
using System.Runtime.CompilerServices;
using GameData.Domains.Character;
using GameData.Domains.TaiwuEvent.EventHelper;

namespace GameData.Domains.TaiwuEvent.EventOption
{
	// Token: 0x020000BF RID: 191
	public class OptionConditionProfessionFavor : TaiwuEventOptionConditionBase
	{
		// Token: 0x06001BCA RID: 7114 RVA: 0x0017DDF4 File Offset: 0x0017BFF4
		public OptionConditionProfessionFavor(short id, Func<int, int, bool> checkFunc) : base(id)
		{
			this.ConditionChecker = checkFunc;
		}

		// Token: 0x06001BCB RID: 7115 RVA: 0x0017DE8C File Offset: 0x0017C08C
		public override bool CheckCondition(EventArgBox box)
		{
			int charId = -1;
			bool flag = box.Get("CharacterId", ref charId);
			return flag && this.ConditionChecker(charId, DomainManager.Taiwu.GetTaiwuCharId());
		}

		// Token: 0x06001BCC RID: 7116 RVA: 0x0017DECC File Offset: 0x0017C0CC
		public override ValueTuple<short, string[]> GetDisplayData(EventArgBox box)
		{
			int charId = -1;
			bool flag = box.Get("CharacterId", ref charId);
			ValueTuple<short, string[]> result;
			if (flag)
			{
				Character character = DomainManager.Character.GetElement_Objects(charId);
				sbyte behaviorType = character.GetBehaviorType();
				sbyte favorType = EventHelper.GetProfessionFavorabilityType(behaviorType);
				result = new ValueTuple<short, string[]>(this.Id, new string[]
				{
					"<Language Key=" + this.FavorTypeKeys[(int)(favorType - -6)] + "/>"
				});
			}
			else
			{
				short id = this.Id;
				string[] array = new string[1];
				int num = 0;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(16, 1);
				defaultInterpolatedStringHandler.AppendLiteral("<Language Key=");
				defaultInterpolatedStringHandler.AppendFormatted<sbyte>(6);
				defaultInterpolatedStringHandler.AppendLiteral("/>");
				array[num] = defaultInterpolatedStringHandler.ToStringAndClear();
				result = new ValueTuple<short, string[]>(id, array);
			}
			return result;
		}

		// Token: 0x04000675 RID: 1653
		public readonly Func<int, int, bool> ConditionChecker;

		// Token: 0x04000676 RID: 1654
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
