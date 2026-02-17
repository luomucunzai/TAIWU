using System;
using GameData.Domains.Character;

namespace GameData.Domains.TaiwuEvent.EventOption
{
	// Token: 0x020000B7 RID: 183
	public class OptionConditionCharacterValue : TaiwuEventOptionConditionBase
	{
		// Token: 0x06001BB2 RID: 7090 RVA: 0x0017D7A4 File Offset: 0x0017B9A4
		public OptionConditionCharacterValue(short id, int value, Func<Character, int, bool> checker) : base(id)
		{
			this.ConditionChecker = checker;
			this.Value = value;
		}

		// Token: 0x06001BB3 RID: 7091 RVA: 0x0017D7C0 File Offset: 0x0017B9C0
		public override bool CheckCondition(EventArgBox box)
		{
			bool flag = box == null;
			return !flag && this.ConditionChecker(box.GetCharacter("CharacterId"), this.Value);
		}

		// Token: 0x06001BB4 RID: 7092 RVA: 0x0017D7FC File Offset: 0x0017B9FC
		public override ValueTuple<short, string[]> GetDisplayData(EventArgBox box)
		{
			int charId = box.GetInt("CharacterId");
			ValueTuple<string, string> nameTuple = DomainManager.Character.GetNameRelatedData(charId).GetDisplayName(charId == DomainManager.Taiwu.GetTaiwuCharId());
			return new ValueTuple<short, string[]>(this.Id, new string[]
			{
				nameTuple.Item1 + nameTuple.Item2,
				this.Value.ToString()
			});
		}

		// Token: 0x04000661 RID: 1633
		public readonly Func<Character, int, bool> ConditionChecker;

		// Token: 0x04000662 RID: 1634
		public int Value;
	}
}
