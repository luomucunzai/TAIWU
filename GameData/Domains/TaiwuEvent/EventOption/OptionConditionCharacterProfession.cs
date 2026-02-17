using System;
using Config;
using GameData.Domains.Character;

namespace GameData.Domains.TaiwuEvent.EventOption
{
	// Token: 0x020000B8 RID: 184
	public class OptionConditionCharacterProfession : TaiwuEventOptionConditionBase
	{
		// Token: 0x06001BB5 RID: 7093 RVA: 0x0017D86E File Offset: 0x0017BA6E
		public OptionConditionCharacterProfession(short id, int professionId, Func<GameData.Domains.Character.Character, int, bool> checker) : base(id)
		{
			this.ConditionChecker = checker;
			this.ProfessionId = professionId;
		}

		// Token: 0x06001BB6 RID: 7094 RVA: 0x0017D888 File Offset: 0x0017BA88
		public override bool CheckCondition(EventArgBox box)
		{
			bool flag = box == null;
			return !flag && this.ConditionChecker(box.GetCharacter("CharacterId"), this.ProfessionId);
		}

		// Token: 0x06001BB7 RID: 7095 RVA: 0x0017D8C4 File Offset: 0x0017BAC4
		public override ValueTuple<short, string[]> GetDisplayData(EventArgBox box)
		{
			int charId = box.GetInt("CharacterId");
			ValueTuple<string, string> nameTuple = DomainManager.Character.GetNameRelatedData(charId).GetDisplayName(charId == DomainManager.Taiwu.GetTaiwuCharId());
			return new ValueTuple<short, string[]>(this.Id, new string[]
			{
				nameTuple.Item1 + nameTuple.Item2,
				Profession.Instance[this.ProfessionId].Name
			});
		}

		// Token: 0x04000663 RID: 1635
		public readonly Func<GameData.Domains.Character.Character, int, bool> ConditionChecker;

		// Token: 0x04000664 RID: 1636
		public int ProfessionId;
	}
}
