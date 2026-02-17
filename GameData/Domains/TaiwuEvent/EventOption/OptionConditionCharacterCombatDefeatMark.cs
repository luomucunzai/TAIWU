using System;
using GameData.Domains.Character;

namespace GameData.Domains.TaiwuEvent.EventOption
{
	// Token: 0x020000C7 RID: 199
	public class OptionConditionCharacterCombatDefeatMark : TaiwuEventOptionConditionBase
	{
		// Token: 0x06001BE4 RID: 7140 RVA: 0x0017E658 File Offset: 0x0017C858
		public OptionConditionCharacterCombatDefeatMark(short id, sbyte combatType, Func<Character, sbyte, bool> checker) : base(id)
		{
			this.ConditionChecker = checker;
			this.CombatType = combatType;
		}

		// Token: 0x06001BE5 RID: 7141 RVA: 0x0017E674 File Offset: 0x0017C874
		public override bool CheckCondition(EventArgBox box)
		{
			bool flag = box == null;
			return !flag && this.ConditionChecker(box.GetCharacter("CharacterId"), this.CombatType);
		}

		// Token: 0x06001BE6 RID: 7142 RVA: 0x0017E6B0 File Offset: 0x0017C8B0
		public override ValueTuple<short, string[]> GetDisplayData(EventArgBox box)
		{
			int charId = box.GetInt("CharacterId");
			ValueTuple<string, string> nameTuple = DomainManager.Character.GetNameRelatedData(charId).GetDisplayName(charId == DomainManager.Taiwu.GetTaiwuCharId());
			byte relatedDefeatMarkCount = GlobalConfig.NeedDefeatMarkCount[(int)this.CombatType];
			return new ValueTuple<short, string[]>(this.Id, new string[]
			{
				nameTuple.Item1 + nameTuple.Item2,
				relatedDefeatMarkCount.ToString()
			});
		}

		// Token: 0x04000686 RID: 1670
		public readonly Func<Character, sbyte, bool> ConditionChecker;

		// Token: 0x04000687 RID: 1671
		public sbyte CombatType;
	}
}
