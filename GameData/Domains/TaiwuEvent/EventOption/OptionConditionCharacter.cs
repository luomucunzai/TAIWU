using System;
using GameData.Domains.Character;

namespace GameData.Domains.TaiwuEvent.EventOption
{
	// Token: 0x020000B6 RID: 182
	public class OptionConditionCharacter : TaiwuEventOptionConditionBase
	{
		// Token: 0x06001BAF RID: 7087 RVA: 0x0017D6F8 File Offset: 0x0017B8F8
		public OptionConditionCharacter(short id, Func<Character, bool> checker) : base(id)
		{
			this.ConditionChecker = checker;
		}

		// Token: 0x06001BB0 RID: 7088 RVA: 0x0017D70C File Offset: 0x0017B90C
		public override bool CheckCondition(EventArgBox box)
		{
			bool flag = box == null;
			return !flag && this.ConditionChecker(box.GetCharacter("CharacterId"));
		}

		// Token: 0x06001BB1 RID: 7089 RVA: 0x0017D740 File Offset: 0x0017B940
		public override ValueTuple<short, string[]> GetDisplayData(EventArgBox box)
		{
			int charId = box.GetInt("CharacterId");
			ValueTuple<string, string> nameTuple = DomainManager.Character.GetNameRelatedData(charId).GetMonasticTitleOrDisplayName(charId == DomainManager.Taiwu.GetTaiwuCharId());
			return new ValueTuple<short, string[]>(this.Id, new string[]
			{
				nameTuple.Item1 + nameTuple.Item2
			});
		}

		// Token: 0x04000660 RID: 1632
		public readonly Func<Character, bool> ConditionChecker;
	}
}
