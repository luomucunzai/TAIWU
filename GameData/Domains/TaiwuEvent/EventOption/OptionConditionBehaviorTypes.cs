using System;
using System.Text;
using Config;
using GameData.Domains.Character;

namespace GameData.Domains.TaiwuEvent.EventOption
{
	// Token: 0x020000C2 RID: 194
	public class OptionConditionBehaviorTypes : TaiwuEventOptionConditionBase
	{
		// Token: 0x06001BD5 RID: 7125 RVA: 0x0017E2B9 File Offset: 0x0017C4B9
		public OptionConditionBehaviorTypes(short id, sbyte b1, sbyte b2, sbyte b3, sbyte b4, sbyte b5, Func<sbyte, sbyte, sbyte, sbyte, sbyte, bool> checkFunc) : base(id)
		{
			this.BehaviorRange = new sbyte[]
			{
				b1,
				b2,
				b3,
				b4,
				b5
			};
			this.ConditionChecker = checkFunc;
		}

		// Token: 0x06001BD6 RID: 7126 RVA: 0x0017E2F0 File Offset: 0x0017C4F0
		public override bool CheckCondition(EventArgBox box)
		{
			GameData.Domains.Character.Character character = box.GetCharacter("CharacterId");
			bool flag = character != null;
			return flag && this.ConditionChecker(this.BehaviorRange[0], this.BehaviorRange[1], this.BehaviorRange[2], this.BehaviorRange[3], this.BehaviorRange[4]);
		}

		// Token: 0x06001BD7 RID: 7127 RVA: 0x0017E350 File Offset: 0x0017C550
		public override ValueTuple<short, string[]> GetDisplayData(EventArgBox box)
		{
			StringBuilder sb = new StringBuilder();
			int i = 0;
			foreach (sbyte beh in this.BehaviorRange)
			{
				bool flag = beh < 0;
				if (!flag)
				{
					bool flag2 = i != 0;
					if (flag2)
					{
						sb.Append('、');
					}
					sb.Append(Config.BehaviorType.Instance[(short)beh].Name);
					i++;
				}
			}
			return new ValueTuple<short, string[]>(this.Id, new string[]
			{
				sb.ToString()
			});
		}

		// Token: 0x0400067C RID: 1660
		public readonly sbyte[] BehaviorRange;

		// Token: 0x0400067D RID: 1661
		public readonly Func<sbyte, sbyte, sbyte, sbyte, sbyte, bool> ConditionChecker;
	}
}
